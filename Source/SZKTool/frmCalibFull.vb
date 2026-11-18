Imports Microsoft.VisualBasic.FileIO
Imports Microsoft.VisualBasic.ApplicationServices
Imports System.Windows.Forms.DataVisualization.Charting
Imports System.Text
Imports System.IO
Imports SZKTool.frmOptions
Imports SZKTool.RealTimeMeas
Imports System.Security.Permissions



''' <summary>
''' キャリブレーション処理画面
''' </summary>
Public Class frmCalibFull
#If 1
'WndProcをオーバライドしフォーム移動禁止
<SecurityPermission(SecurityAction.Demand, Flags:=SecurityPermissionFlag.UnmanagedCode)> Protected Overrides Sub WndProc(ByRef m As Message)
    Const WM_SYSCOMMAND As Integer = &H112
    Const SC_MOVE As Long = &HF010L

    If m.Msg = WM_SYSCOMMAND AndAlso _
        (m.WParam.ToInt64() And &HFFF0L) = SC_MOVE Then
        m.Result = IntPtr.Zero
        Return
    End If

    MyBase.WndProc(m)
End Sub
#end if
'▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼
'   定数
'▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲
'Private Const iPeakNum			As Int32	= 100							'較正結果に必要なピーク数（周期数*2）
Public Const cCalibAllPeriodNum	As Int32	= 10							'サンプル取得周期数
Public Const cCalibCalPeriodNum	As Int32	= 2								'サンプル平均化周期数

'▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼
'   構造体
'▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲

'▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼
'   パブリック変数
'▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲

'▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼
'   プライベート変数
'▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲
Private m_sMessage				As String                   'ステータスメッセージ
Private m_iCalibDataTime		As Int32	= 10			'キャリブレーションデータ取得時間(秒)
Private m_iTotalSec				As int64 = 0				'予想終了日時を表示するための全終了に要する時間(秒)
Private m_dEndPrediction		As DateTime					'終了予測日時 結果

Private	ds						As New Dataset
Private	dtStress				As New DataTable			'応力 データテーブル
Private	dtHosei					As New DataTable			'応力補正データテーブル

Private iError					As Int16 = 0

Private iCalMode				As Int16					'0:Full Stress 1:Zero Stress

Private bNew					As Boolean = True

'モニタグラフ用データテーブル名
Private sDataTblName()			As String = _
{	"StrsMon", _
	"WaveMon" _
}
Private eNum etbleNum
	iStrs = 0
	iWave
End eNum
Private sAreaId()			As String = _
{	"X Full", _
	"C Full", _
	"P Full", _
	"X PosPhase", _
	"CX PosPhase", _
	"CY PosPhase", _
	"Z PosPhase" _
}
Private eNum eAreaId
	XFull = 0
	CFull
	PFull
	XPhase
	CXPhase
	CYPhase
	ZPhase
End eNum
'多言語メッセージ格納
Private m_sStr(eMes.iMAX - 1)		 As String   
Enum eMes
    iCaliblationTitleFull = 0	'キャリブレーション(フル応力)
    iCaliblationTitleZero		'キャリブレーション(ゼロ応力)
	iCalibSSZeroStartOK			'ゼロ調整 開始可能です
	iCalibSSZeroPrePare			'ゼロ調整 準備中です
	iCalibSSZeroComp			'ゼロ調整 完了しました。測定開始可能です。
	iCalibSSMeasPrePare			'キャリブレーション測定準備中です。
	iCalibSSXYActWait			'XY動作待ち時間(Sec) -> 
	iCalibSSMeasMes				'キャリブレーション測定中 (周期, 振幅) : (#PRD#, #AMP#)  #DATACNT# / #SAMPLE#  終了予定日時 : #DATE#
	iCalibSSMeasComp			'キャリブレーション測定終了しました。
	iCalibSSAutoPeel			'自動剥し処理中です。
    iCalibSSNoSelect			'測定条件が選択されていません。
    iCalibSSEndTimeQ			'測定終了日時は \r\n#DATE# の予定です。\r\n実行しますか?
    iYES                        'はい(&Y)
    iYES_TO_ALL                 'すべてはい(&A)
    iNO                         'いいえ(&N)
    iOK                         'OK(&O)
    iCANCEL		                'キャンセル(&C)
	iSTRESS_MON					'応力モニタ
	iWAVE_MON					'波形モニタ
	iMON_X						'時間 [m秒]
	iSTRESS_MON_Y				'応力[uN]
	iWAVE_MON_Y					'レベル[Dig]
	iMAX
End Enum

'■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
'   コンストラクタ
'■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
'======================================================================
''' <summary>キャリブ画面生成</summary>
Public Sub New(ByVal mode As int16)
	bNew = True			'初期化中有効
	' この呼び出しはデザイナーで必要です。
	InitializeComponent()
	' InitializeComponent() 呼び出しの後で初期化を追加します。
	iCalMode	= mode							'動作モード設定
	InitialCalibFull()							'設定ファイル環境初期化
End Sub
''' <summary>
''' フォームロード
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub frmCalib_Load(sender As Object, e As EventArgs) Handles MyBase.Load
Try
	p_FormCalibVisible = True

	p_RTM(0).p_iCalibMode	= iCalMode				'RealTimeMeasスレッドへ伝える

	'今回使用するFull/Zeroキャリブレーションファイルフルパスをリアルタイム測定スレッドへ伝える



	'言語切替
	Call SetFormLanguage(Me, p_iLanguage, ,m_sStr)
	Me.Text = If(iCalMode=0, m_sStr(eMes.iCaliblationTitleFull), m_sStr(eMes.iCaliblationTitleZero) )
	'Me.FormBorderStyle = FormBorderStyle.None
	InitialCalibFull()								'設定ファイル環境初期化

	Me.WindowState = FormWindowState.Maximized
	'応力設定ファイルを読込みグリッド表示
	Select Case iCalMode
	Case 0	'フルキャリブレーション測定モード
		sbFullStressCondRead(0, p_FClbX, p_FClbC, p_FClbP)		'フル応力
	Case 1	'ゼロキャリブレーション測定モード
		sbFullStressCondRead(0, p_FClbX, p_FClbC, p_FClbP)		'フル応力
		sbFullStressCondRead(1, p_ZClbX, p_ZClbC, p_ZClbP)		'ゼロ応力
	End Select
	p_RTM(0).p_CaliblationState.iState = RealTimeMeas.eCalibState.NOT_START

	'更新日時の表示
	UpdateDisp()
	
	'リアルタイム値モニター初期設定
	RealTimeMonitorValDispSetting(dgvCurrVal, 0)

	If ds.Tables.Count <= 0
		'データセットの補正値モニタテーブル
		dtHosei.TableName = sDataTblName(etbleNum.iStrs)
		dtHosei.Columns.Add("Time", Type.GetType("System.Int64"))
		dtHosei.Columns.Add("Hosei", Type.GetType("System.Double"))
		ds.Tables.Add(dtHosei)

		'データセットの応力モニタテーブル
		dtStress.TableName = sDataTblName(etbleNum.iWave)
		dtStress.Columns.Add("Time", Type.GetType("System.Int64"))
		dtStress.Columns.Add("XData", Type.GetType("System.Int64"))
		dtStress.Columns.Add("YData", Type.GetType("System.Int64"))
		ds.Tables.Add(dtStress)
	End if
	'条件設定初期値
	Select Case iCalMode
	Case 0		'フルストレス
		nudPreTemp.Value			= 30		' 較正中 温度設定
		nudPeelingTemp.Value		= 220		' 自動剥がし温度
		nudInputGap.Value			= 20.0		' 較正中 入力GAP
		nudMeasureGap.Value			= 0.005		' 較正中 GAP
		nudXYWait.Value				= 200		' XY動作待ち時間
		nudRisingHPWait.Value		= 5			' 熱板上昇待ち時間
		nudSampleTime.Value			= 10		' サンプル取得時間
		nudAcquisitionTime.Value	= 30		' サンプル有効比率％

	Case 1		'ゼロストレス
		nudPreTemp.Value			= 30		' 較正中 温度設定
		nudPeelingTemp.Value		= 30		' 自動剥がし温度
		nudInputGap.Value			= 20.0		' 較正中 入力GAP
		nudMeasureGap.Value			= 5.0		' 較正中 GAP
		nudXYWait.Value				= 5			' XY動作待ち時間
		nudRisingHPWait.Value		= 3			' 熱板上昇待ち時間
		nudSampleTime.Value			= 10		' サンプル取得時間
		nudAcquisitionTime.Value	= 30		' サンプル有効比率％

	End Select

	'温度設定フィールド範囲設定
	nudPreTemp.Maximum		= p_cts.TempCnt_HUpperLimit
	nudPreTemp.Minimum		= 0
	nudPeelingTemp.Maximum	= p_cts.TempCnt_HUpperLimit
	nudPeelingTemp.Minimum	= 0

	'グラフ初期表示
	'補正値テーブル ダミー
	Dim dtRow As DataRow = Nothing
	ds.Tables(sDataTblName(etbleNum.iStrs)).Clear
	dtRow = ds.Tables(sDataTblName(etbleNum.iStrs)).NewRow
	dtRow(0) = 0
	dtRow(1) = 0
	ds.Tables(sDataTblName(etbleNum.iStrs)).Rows.Add(dtRow)
	'グラフ表示
	CorrectChartDisp( ChartHosei, ds, m_iCalibDataTime * 1000, p_cts )

	'応力値テーブル ダミー
	ds.Tables(sDataTblName(etbleNum.iWave)).Clear
	dtRow = ds.Tables(sDataTblName(etbleNum.iWave)).NewRow
	dtRow(0) = 0
	dtRow(1) = 0
	dtRow(2) = 0
	ds.Tables(sDataTblName(etbleNum.iWave)).Rows.Add(dtRow)
	'グラフ表示
	XYWaveChartDisp( ChartStress, ds, m_iCalibDataTime * 1000, clSpec.eStrsSens.Circle )

	'常時タイマースタート
	timMain.Interval = 250
	timMain.Enabled = True

	btnXExec.Enabled	= False
	btnCExec.Enabled	= False
	btnExec.Enabled		= False
	lblMsg.ForeColor = Color.Blue

	m_iCalibDataTime = nudSampleTime.Value

	' 較正中 温度設定
    p_RTM(0).p_mltcp.TempPreset(1) = Double.parse(nudPreTemp.Value)
	' 較正中 入力GAP
    p_RTM(0).p_mltcp.InpGap(1)		=  Double.parse(nudInputGap.Value)
	' 較正中 GAP
    p_RTM(0).p_mltcp.GapPreset(1)	=  Double.parse(nudMeasureGap.Value)
	' XY動作待ち時間
    p_RTM(0).p_mltcp.MesPreWait(1)	=  Double.parse(nudXYWait.Value)
	' 熱板上昇待ち時間
    p_RTM(0).p_mltcp.MeasWaitTime(1) =  Double.parse(nudRisingHPWait.Value)
	
	If p_RTM(0).p_mltcp.IsZeroStartSW
		if p_RTM(0).p_iZeroAdjOk = 1
			p_RTM(0).p_CaliblationState.iState = RealTimeMeas.eCalibState.ZERO_COMP
		Else
			p_RTM(0).p_CaliblationState.iState = RealTimeMeas.eCalibState.ZERO_EXEC
		End If
	End If
	
	'温度設定を送信
    p_RTM(0).p_mltcp.TempPreset(1)	= double.parse(nudPreTemp.Value)
    p_RTM(0).p_mltcp.Temp2Preset(1) = double.parse(nudPreTemp.Value)
	p_RTM(0).p_mltcp.Temp3Preset(1) = p_clSpec.SGTempGet(p_RTM(0).p_mltcp.TempPreset(1))


	iError = 0			'エラー発生時はファイル保存しない
	bNew = False		'初期化中解除


Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub
''' <summary>
''' Update日時を表示
''' </summary>
Private Sub	UpdateDisp()
Try
	Select Case iCalMode
	Case 0
		Select Case tcPara.SelectedIndex
		Case 0
			lblMeasDateVal.Text = p_FMeasDateC
		Case 1
			lblMeasDateVal.Text = p_FMeasDateX
		Case 2
			lblMeasDateVal.Text = p_FMeasDateP
		End Select
	Case 1
		Select Case tcPara.SelectedIndex
		Case 0
			lblMeasDateVal.Text = p_ZMeasDateC
		Case 1
			lblMeasDateVal.Text = p_ZMeasDateX
		Case 2
			lblMeasDateVal.Text = p_ZMeasDateP
		End Select
	End Select
	'If lblMeasDateVal.Text<>""
	'End If

Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub
'カラムヘッダが表示されていないと保存時にカラムヘッダの文字列が取れないのでここに保持する
Dim sXClmHeader()	As String = Nothing
Dim sCClmHeader()	As String = Nothing
Dim sPClmHeader()	As String = Nothing
Dim sClmHeader()	As String
''' <summary>
''' 応力設定ファイルを読込みグリッド表示
''' </summary>
''' <param name="iReadType">0:Fullを読み込む 1:Zeroを読み込む</param>
''' <param name="p_ClbX"></param>
''' <param name="p_ClbC"></param>
''' <param name="p_ClbP"></param>
''' <returns></returns>
Public Function sbFullStressCondRead(ByVal iReadType As Int16, ByRef p_ClbX() As stPoint, ByRef p_ClbC() As stPoint, ByRef p_ClbP() As stPoint) As int32
	Dim iRowCount	As Int16 = 0
	Dim iRowCount1	As Int16 = 0
    Dim parser		As TextFieldParser 
	Dim dgvTemp		As DataGridView = Nothing
	Dim i			As Integer
	Dim dData		As Double
	Dim bRes		As Boolean
	Dim sTemp		As string
	Dim sMode		As String = Nothing
	Dim sSection	As String = Nothing
	Dim fclb()		As stPoint = Nothing
	Dim iCnt		As Int32 = 0
	Dim iRet		As Int32 = 0
	Try
		parser = New TextFieldParser(Path.Combine(p_SetPath, if(iReadType = 0, p_sFullStoreFileName, p_sZeroStoreFileName)), Encoding.UTF8) With {
			.TextFieldType = FieldType.Delimited
		}
		parser.SetDelimiters(",")			'デリミタ設定
		'CSVファイルからデータグリッドビューへセットする---------------------------
        ' データをすべてクリア
        dgvXPara.Rows.Clear()
        dgvCPara.Rows.Clear()
        dgvPPara.Rows.Clear()
        dgvXParaPh.Rows.Clear()
        dgvCParaPhX.Rows.Clear()
        dgvCParaPhY.Rows.Clear()
        dgvPParaPh.Rows.Clear()

		While (Not parser.EndOfData)
            Dim row As String() = parser.ReadFields()	'1行読み込み
			Select Case row(0)		'動作モード
			Case sAreaId(eAreaId.XFull)
				sMode=row(0)
				dgvTemp = dgvXPara
				iRowCount = 0
				sSection = "X_FullZero"
				If row.Length >= 2
					If iReadType=0
						p_FMeasDateX = row(1)
					Else
						p_ZMeasDateX = row(1)
					End If
				End If
				Continue While							'X動作セクション開始
			Case sAreaId(eAreaId.CFull)
				sMode=row(0)
				dgvTemp = dgvCPara
				iRowCount = 0
				sSection = "C_FullZero"
				If row.Length >= 2
					If iReadType=0
						p_FMeasDateC = row(1)
					Else
						p_ZMeasDateC = row(1)
					End If
				End If
				Continue While							'円動作セクション開始
			Case sAreaId(eAreaId.PFull)
				sMode=row(0)
				dgvTemp = dgvPPara
				iRowCount = 0
				sSection = "P_FullZero"
				If row.Length >= 2
					If iReadType=0
						p_FMeasDateP = row(1)
					Else
						p_ZMeasDateP = row(1)
					End If
				End If
				Continue While							'点動作セクション開始
			Case sAreaId(eAreaId.XPhase)
				sSection = row(0)
				fclb = if(iReadType = 1, p_ZClbX, p_FClbX)
				dgvTemp = dgvXParaPh
				iRowCount1 = 0
				iCnt  = 0


				Continue While							'直線動作PhaseXセクション開始
			Case sAreaId(eAreaId.CXPhase)
				sSection = row(0)
				fclb = if(iReadType = 1, p_ZClbC, p_FClbC)
				dgvTemp = dgvCParaPhX
				iRowCount1 = 0
				iCnt  = 0
				Continue While							'円動作PhaseXセクション開始
			Case sAreaId(eAreaId.CYPhase)
				sSection = row(0)
				fclb = if(iReadType = 1, p_ZClbC, p_FClbC)
				dgvTemp = dgvCParaPhY
				iRowCount1 = 0
				iCnt  = 0
				Continue While							'円動作PhaseYセクション開始
			Case sAreaId(eAreaId.ZPhase)
				sSection = row(0)
				fclb = if(iReadType = 1, p_ZClbP, p_FClbP)
				dgvTemp = dgvPParaPh
				iRowCount1 = 0
				iCnt  = 0
				Continue While							'点動作PhaseZセクション開始
			Case "[S]:[mm]"
				'列数調整 空文字要素を削除
				Dim idx As Int16 = Array.IndexOf(row, "")
				If idx>0
					Array.Resize(row, idx)
				End If

			End Select

			Select Case sSection
			Case "X_FullZero","C_FullZero","P_FullZero"
				If iRowCount = 0 then
					Select Case sMode		'動作モード
					Case "X Full"
						Array.Resize(sXClmHeader,row.Length)
						sClmHeader = sXClmHeader
					Case "C Full"
						Array.Resize(sCClmHeader,row.Length)
						sClmHeader = sCClmHeader
					Case "P Full"
						Array.Resize(sPClmHeader,row.Length)
						sClmHeader = sPClmHeader
					End Select
				'カラムヘッダを表示
					dgvTemp.ColumnCount = row.Length
					For i = 0 To row.Length - 1
						bRes = Double.TryParse(row(i) ,dData)
						If not bRes Then
							sTemp = row(i)
						Else
							sTemp = dData.ToString("F3")
						End If
						dgvTemp.Columns(i).Name = sTemp
						sClmHeader(i) = sTemp
					Next
				Else
				'読み込んだデータ(1行をDataGridViewに表示する)
					'周期値のフォーマット
					bRes = Double.TryParse(row(0) ,dData)
					If not bRes Then
						sTemp = row(0)
					Else
						sTemp = dData.ToString("F1")
					End If
					row(0) = sTemp
					dgvTemp.Rows.Add(row)
				End If
				iRowCount	+= 1
			End Select

			Select Case sSection
			Case "X PosPhase","CX PosPhase","CY PosPhase","Z PosPhase"
				If iRowCount1 = 0 then
					Select Case sSection		'動作モード
					Case "X PosPhase"
						Array.Resize(sXClmHeader,row.Length)
						sClmHeader = sXClmHeader
					Case "CX PosPhase"
						Array.Resize(sCClmHeader,row.Length)
						sClmHeader = sCClmHeader
					Case "CY PosPhase"
						Array.Resize(sCClmHeader,row.Length)
						sClmHeader = sCClmHeader
					Case "Z PosPhase"
						Array.Resize(sPClmHeader,row.Length)
						sClmHeader = sPClmHeader
					End Select
				'カラムヘッダを表示
					dgvTemp.ColumnCount = row.Length
					For i = 0 To row.Length - 1
						bRes = Double.TryParse(row(i) ,dData)
						If not bRes Then
							sTemp = row(i)
						Else
							sTemp = dData.ToString("F3")
						End If
						dgvTemp.Columns(i).Name = sTemp
						sClmHeader(i) = sTemp
					Next
				Else
				'読み込んだデータ(1行をDataGridViewに表示する)
					'周期値のフォーマット
					bRes = Double.TryParse(row(0) ,dData)
					If not bRes Then
						sTemp = row(0)
					Else
						sTemp = dData.ToString("F1")
					End If
					row(0) = sTemp
					dgvTemp.Rows.Add(row)
				End If
				iRowCount1	+= 1
			End Select
        End While
		'キャリブレーションエリア終了
		'較正情報配列へ設定 --------------------------------------
		iRet += SetClb( dgvXPara, dgvXParaPh,	dgvXParaPh,		p_ClbX )
		iRet += SetClb( dgvCPara, dgvCParaPhX,	dgvCParaPhY,	p_ClbC )
		iRet += SetClb( dgvPPara, dgvPParaPh,	dgvPParaPh,		p_ClbP )

		'グリッドビュー表示設定 ---------------------------------
		InitialSetDgvDisp( dgvXPara )
		InitialSetDgvDisp( dgvCPara )
		InitialSetDgvDisp( dgvPPara )

		InitialSetDgvDisp( dgvXParaPh )
		InitialSetDgvDisp( dgvCParaPhX )
		InitialSetDgvDisp( dgvCParaPhY )
		InitialSetDgvDisp( dgvPParaPh )
	parser.Close
	parser.Dispose
	Return iRet
Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Return -1
End Try
End Function
#If 0
''' <summary>
''' 100％応力比率,位相角 設定ファイルを読込み
''' </summary>
''' <param name="iMode">2:Alpha 3:x位置位相差 4:y位置位相差 4:z位置位相差</param>
''' <returns></returns>
Public Function sbFullStressRALRead(ByVal iMode As Int16) As int32
	Dim iRowCount	As Int16 = 0
    Dim parser		As TextFieldParser 
	Dim i			As Integer
	Dim iCnt		As Integer
	Dim dData		As Double
	Dim bRes		As Boolean
	Dim sMode		As String = Nothing
	Dim sFileName	As String = Nothing
	Dim fclb()		As stPoint = Nothing
Try
	Select Case iMode
	Case 2
		sFileName = p_sStoreFileNameAlpha
	Case 3
		sFileName = p_sStoreFileNamePxPhase
	Case 4
		sFileName = p_sStoreFileNamePyPhase
	Case 5
		sFileName = p_sStoreFileNamePzPhase
	End Select
	parser = New TextFieldParser(Path.Combine(p_SetPath, sFileName), Encoding.UTF8) With {
			.TextFieldType = FieldType.Delimited
						}
		parser.SetDelimiters(",")			'デリミタ設定

	While (Not parser.EndOfData)
        Dim row As String() = parser.ReadFields()	'1行読み込み
		Select Case row(0)		'動作モード
		Case "X"	
			sMode=row(0)
			fclb = p_FClbX
			Continue While							'X動作セクション開始
		Case "C"
			sMode=row(0)
			iRowCount = 0
			fclb = p_FClbC
			Continue While							'円動作セクション開始
		Case "P"
			sMode=row(0)
			iRowCount = 0
			fclb = p_FClbP
			Continue While							'円動作セクション開始
		End Select
		'Select Case sMode
		'Case "C"
			If iRowCount = 0 then
			Else
			'読み込んだデータ(周期値,,,,,)
				For i = 0 To row.GetUpperBound(0)
					If i = 0
					Else
						bRes = Double.TryParse(row(i) ,dData)
						If iCnt>fclb.GetUpperBound(0)
							Exit for
						End If
						Select Case iMode
						Case 2	'位相角
							If bRes 
								fclb(iCnt).dAlpha = dData			'位相角セット
							Else
								fclb(iCnt).dAlpha = Double.NaN
							End If
						Case 3	'x位置位相差
							If bRes 
								fclb(iCnt).dPPhaseX = dData			'位相角セット
							Else
								fclb(iCnt).dPPhaseX = Double.NaN
							End If
						Case 4	'y位置位相差
							If bRes 
								fclb(iCnt).dPPhaseY = dData			'位相角セット
							Else
								fclb(iCnt).dPPhaseY = Double.NaN
							End If
						Case 5	'z位置位相差
							If bRes 
								fclb(iCnt).dPPhaseZ = dData			'位相角セット
							Else
								fclb(iCnt).dPPhaseZ = Double.NaN
							End If
						End Select
						iCnt += 1
					End If
				Next i
			End If
		'End Select
		iRowCount += 1
    End While
	Return 0
Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Return -1
End Try
End Function
#end if
''' <summary>
''' データグリッドビューの表示・機能を初期設定
''' </summary>
''' <param name="dgv"></param>
''' <returns></returns>
Function InitialSetDgvDisp( byref dgv As DataGridView ) As Int32
Try
	With dgv
		'データグリッドビューの表示設定 ---------------------------------------------------------
		.EnableHeadersVisualStyles = False							'Windowsテーマ優先無効
		'カラムヘッダスタイル設定
		.ColumnHeadersDefaultCellStyle.foreColor = Color.Black
		.ColumnHeadersDefaultCellStyle.BackColor = Color.LightGray
		.ColumnHeadersDefaultCellStyle.Font = New Font("Verdana", 12, FontStyle.Bold)
		.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
		'行ヘッダ表示消去        
		.RowHeadersVisible=False
		For Each column As DataGridViewColumn in .Columns
			column.SortMode = DataGridViewColumnSortMode.NotSortable		'ソート不可
			column.Width = 120												'セル幅
			column.DefaultCellStyle.Alignment=DataGridViewContentAlignment.MiddleCenter
			column.DefaultCellStyle.Font = New Font("calibri", 18, FontStyle.Regular)
		Next
		'列ヘッダ表示形式
		.Columns(0).DefaultCellStyle.Font = New Font("Verdana", 12, FontStyle.Bold)
		.Columns(0).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
		.Columns(0).ReadOnly = True
		.Columns(0).Width = 120

		'複数セル選択許可
		.MultiSelect = True
		.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect
		.ClearSelection()
		If p_bCalibValChangeEn			'キャリブレーション値の入力許可スイッチ
		Else
			.EditMode = DataGridViewEditMode.EditProgrammatically		'編集
		End If

		.Columns(0).Frozen		= True								'左1列を固定


		.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells
		.RowTemplate.Height = 30

		'カラムヘッダ高さ
		.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing
		.ColumnHeadersHeight = 70

	End With
	Return 0
Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Return -1
End Try
End Function
''' <summary>
''' 測定情報配列へ設定 
''' </summary>
''' <param name="dgv">データグリッドビュー参照</param>
''' <returns>0:成功 -1:例外発生</returns>
Private Function setclb(byref dgv As DataGridView, byref dgvph0 As DataGridView, byref dgvph1 As DataGridView, byref stClb As stPoint()) As Int32
Try
	Dim i As Int16
	Dim j As Int16
	Dim iCnt As Int16 = 0
	Dim bRes As Boolean
	Array.Resize(stClb, (dgv.Rowcount-0)*(dgv.ColumnCount-1))
	For i=0 To dgv.Rowcount - 1		'周期条件数 最終行は空欄
		For j=0 To dgv.ColumnCount - 1	'振幅条件数
			stClb(iCnt).bSelected		= False
			If j<> 0 Then	'1列目は行ヘッダー文字列
				Select Case dgv.Name
				Case "dgvXPara"
					bRes = double.TryParse(dgv.Item(0, i).Value, stClb(iCnt).dPeriodX)		'X軸 周期
					stClb(iCnt).dPeriodY	= 1
					stClb(iCnt).dPeriodZ	= 1
					bRes = double.TryParse(dgv.Columns(j).name, stClb(iCnt).dAmplitudeX)	'X軸 振幅
					stClb(iCnt).dAmplitudeY = 0
					stClb(iCnt).dAmplitudeZ = 0
					stClb(iCnt).iSample		= CalcSampleNumber(clSpec.eStrsSens.Line, stClb(iCnt))			'サンプル数計算
					stClb(iCnt).dPPhaseX	= dgvph0.Item(j, i).Value										'x位相測定値
				Case "dgvCPara"
					bRes = double.TryParse(dgv.Item(0, i).Value, stClb(iCnt).dPeriodX)
					stClb(iCnt).dPeriodY = stClb(iCnt).dPeriodX
					bRes = double.TryParse(dgv.Columns(j).name, stClb(iCnt).dAmplitudeX)
					stClb(iCnt).dAmplitudeY = stClb(iCnt).dAmplitudeX
					stClb(iCnt).dPeriodZ	= 1
					stClb(iCnt).dAmplitudeZ = 0
					stClb(iCnt).iSample		= CalcSampleNumber(clSpec.eStrsSens.Circle, stClb(iCnt))		'サンプル数計算
					stClb(iCnt).dPPhaseCX	= dgvph0.Item(j, i).Value										'x位相測定値
					stClb(iCnt).dPPhaseCY	= dgvph1.Item(j, i).Value										'y位相測定値
				Case "dgvPPara"
					bRes = double.TryParse(dgv.Item(0, i).Value, stClb(iCnt).dPeriodZ)		'Z軸 周期
					stClb(iCnt).dPeriodY	= 1
					stClb(iCnt).dPeriodX	= 1
					bRes = double.TryParse(dgv.Columns(j).name, stClb(iCnt).dAmplitudeZ)	'Z軸 振幅
					stClb(iCnt).dAmplitudeY = 0
					stClb(iCnt).dAmplitudeX = 0
					stClb(iCnt).iSample		= CalcSampleNumber(clSpec.eStrsSens.Point, stClb(iCnt))			'サンプル数計算
					stClb(iCnt).dPPhaseZ	= dgvph0.Item(j, i).Value										'z位相測定値
				End Select

				bRes = int64.TryParse(dgv.Item(j, i).Value, stClb(iCnt).iFullVal)			'応力測定値

				'stClb(iCnt).iSample = CalcSampleNumber(stClb(iCnt))						'サンプル数計算
				iCnt += 1
			End If
		Next j
	Next i
	Return 0
Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Return -1
End Try
End Function
''' <summary>
''' 取得する全サンプル数を計算する
''' 取得する秒数から
''' </summary>
''' <param name="iMode">測定モード</param>
''' <param name="stClb"></param>
''' <returns></returns>
Function CalcSampleNumber(ByVal iMode As Int16, ByRef stClb As stPoint) As Int32
Try
	'Dim imiliSecT As Int32 = stClb.dPeriodX * 1000 			'測定周期をmSec変換
	'Dim iNum As Int32 = (imiliSecT * iPeakNum / 2) / 2		'1周期で2ピーク出現、固定2msサンプリングで計算 
	'Return iNum
#If 0	
	Return  m_iCalibDataTime * MEAS_BLOCK_DATA_NUM 
#Else
	Dim iNum As int32 = m_iCalibDataTime * MEAS_BLOCK_DATA_NUM
	Select Case iMode
	Case clSpec.eStrsSens.Circle, clSpec.eStrsSens.Line
		iNum = stClb.dPeriodX * 10 * MEAS_BLOCK_DATA_NUM

	Case clSpec.eStrsSens.Point
		iNum = stClb.dPeriodZ * 10 * MEAS_BLOCK_DATA_NUM
	End Select
	Return  iNum

#End If
Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Return -1
End Try
End Function
''' <summary>
''' 較正結果をファイル保存
''' </summary>
''' <returns></returns>
Private Function StoreFile() As Int32
	Dim dgv				As DataGridView = Nothing
	Dim i				As Int16
	Dim textFile		As IO.StreamWriter			' -- 出力するファイル
　　Dim textLine		As String = Nothing			' -- 書き出す１行
　　Dim cellData		As String = ""				' -- セルのデータ
	Dim sColHeader(9)	As String
Try
　　' --- ファイルのインスタンスを作る
　	textFile = New IO.StreamWriter(Path.Combine(p_SetPath, If(iCalMode=0,p_sFullStoreFileName,p_sZeroStoreFileName)), False, Encoding.UTF8)	' -- ファイル名、上書き、エンコーディング
	'応力更正値の書込み
	For i = 0 To 2
		Select Case i
		Case 0
			textLine = sAreaId(eAreaId.XFull) & "," & If(iCalMode=0,p_FMeasDateX,p_ZMeasDateX)		'識別子行を書き出す
			dgv = dgvXPara
			sClmHeader = sXClmHeader
		Case 1
			textLine = sAreaId(eAreaId.CFull) & "," & If(iCalMode=0,p_FMeasDateC,p_ZMeasDateC)		'識別子行を書き出す
			dgv = dgvCPara
			sClmHeader = sCClmHeader
		Case 2
			textLine = sAreaId(eAreaId.PFull) & "," & If(iCalMode=0,p_FMeasDateP,p_ZMeasDateP)		'識別子行を書き出す
			dgv = dgvPPara
			sClmHeader = sPClmHeader
		End Select
		textFile.WriteLine(textLine)			' -- 終端文字付きで１行を書き出す
		' ---① 列見出しを書き出す
		textLine = ""							' -- 行の文字列をクリア
		' --- １行目（列見出し）を書き出す
		For myCol As Integer = 0 To dgv.ColumnCount - 1
			If myCol = dgv.ColumnCount - 1 Then
				textLine += sClmHeader(myCol)			' -- 最終列ならカンマを付けない
			Else
				textLine += sClmHeader(myCol) & ","		' -- 途中の列ならカンマを付加
			End If
		Next
		sColHeader(i) = textLine
		textFile.WriteLine(textLine) ' -- 終端文字付きで１行を書き出す
		' ---② セルのデータを書き出す
		For myRow As Integer = 0 To dgv.RowCount - 1
			textLine = ""											' -- 行の文字列をクリア
			For myCol As Integer = 0 To dgv.ColumnCount - 1
				If (dgv(myCol, myRow).Value = Nothing) Then
					cellData = ""
				Else
					cellData = dgv(myCol, myRow).Value
				End If
				If (myCol = dgv.ColumnCount - 1) Then
					textLine += cellData				' -- 最終列ならカンマを付けない
				Else
					textLine += cellData & ","			' -- 途中の列には項目の後にカンマを付加する
				End If
			Next
			textFile.WriteLine(textLine) ' -- 終端文字付きで１行を書き出す
		Next
		' ---
	Next i
#If 1
	'位相値の書込み
	For i = 0 To 3
		Select Case i
		Case 0
			textLine = sAreaId(eAreaId.XPhase) & "," & If(iCalMode=0,p_FMeasDateX,p_ZMeasDateX)		'識別子行を書き出す
			dgv = dgvXParaPh
			sClmHeader = sXClmHeader
		Case 1
			textLine = sAreaId(eAreaId.CXPhase) & "," & If(iCalMode=0,p_FMeasDateC,p_ZMeasDateC)		'識別子行を書き出す
			dgv = dgvCParaPhX
			sClmHeader = sCClmHeader
		Case 2
			textLine = sAreaId(eAreaId.CYPhase) & "," & If(iCalMode=0,p_FMeasDateC,p_ZMeasDateC)		'識別子行を書き出す
			dgv = dgvCParaPhY
			sClmHeader = sCClmHeader
		Case 3
			textLine = sAreaId(eAreaId.ZPhase) & "," & If(iCalMode=0,p_FMeasDateP,p_ZMeasDateP)		'識別子行を書き出す
			dgv = dgvPParaPh
			sClmHeader = sPClmHeader
		End Select
		textFile.WriteLine(textLine)			' -- 終端文字付きで１行を書き出す
		' ---① 列見出しを書き出す
		textLine = ""							' -- 行の文字列をクリア
		' --- １行目（列見出し）を書き出す
		For myCol As Integer = 0 To dgv.ColumnCount - 1
			If myCol = dgv.ColumnCount - 1 Then
				textLine += sClmHeader(myCol)			' -- 最終列ならカンマを付けない
			Else
				textLine += sClmHeader(myCol) & ","		' -- 途中の列ならカンマを付加
			End If
		Next
		sColHeader(i) = textLine
		textFile.WriteLine(textLine) ' -- 終端文字付きで１行を書き出す
		' ---② セルのデータを書き出す
		For myRow As Integer = 0 To dgv.RowCount - 1
			textLine = ""											' -- 行の文字列をクリア
			For myCol As Integer = 0 To dgv.ColumnCount - 1
				If (dgv(myCol, myRow).Value = Nothing) Then
					cellData = ""
				Else
					cellData = dgv(myCol, myRow).Value
				End If
				If (myCol = dgv.ColumnCount - 1) Then
					textLine += cellData				' -- 最終列ならカンマを付けない
				Else
					textLine += cellData & ","			' -- 途中の列には項目の後にカンマを付加する
				End If
			Next
			textFile.WriteLine(textLine) ' -- 終端文字付きで１行を書き出す
		Next
		' ---
	Next i
#else
	'線モード X位置位相差の書込み
	textLine = sAreaId(eAreaId.XPhase)		'識別子行を書き出す
	textFile.WriteLine(textLine)			' -- 終端文字付きで１行を書き出す
	textLine = ""							' -- 行の文字列をクリア
	' --- １行目（列見出し）を書き出す
	textFile.WriteLine(sColHeader(0)) ' -- 終端文字付きで１行を書き出す
	Dim sCol() As String = sColHeader(0).Split(",")
	Dim iCnt As int32
	iCnt = 0
	For myRow As Integer = 0 To dgvXPara.RowCount - 1
		textLine = ""											' -- 行の文字列をクリア
		For myCol As Integer = 0 To sCol.GetUpperBound(0)
			If myCol = 0
				cellData = dgvXPara(myCol, myRow).Value
			Else
				If myRow = p_FClbX(iCnt).iRow And myCol = p_FClbX(iCnt).iCol+1
					Select Case iCalMode
					Case 0	'フルキャリブ
						CellData = p_FClbX(iCnt).dPPhaseX.ToString("F6")
					Case 1	'ゼロキャリブ
						CellData = p_ZClbX(iCnt).dPPhaseX.ToString("F6")
					End Select
					iCnt += 1
				End If
			End If
			If myCol = sCol.GetUpperBound(0)
				textLine += CellData				' -- 最終列ならカンマを付けない
			Else
				textLine += cellData & ","			' -- 途中の列には項目の後にカンマを付加する
			End If
		Next
		textFile.WriteLine(textLine)			' -- 終端文字付きで１行を書き出す
	Next

	'円モード X位置位相差の書込み
	textLine = sAreaId(eAreaId.CXPhase)		'識別子行を書き出す
	textFile.WriteLine(textLine)			' -- 終端文字付きで１行を書き出す
	textLine = ""							' -- 行の文字列をクリア
	' --- １行目（列見出し）を書き出す
	textFile.WriteLine(sColHeader(0)) ' -- 終端文字付きで１行を書き出す
	sCol = sColHeader(1).Split(",")
	iCnt = 0
	For myRow As Integer = 0 To dgvCPara.RowCount - 1
		textLine = ""											' -- 行の文字列をクリア
		For myCol As Integer = 0 To sCol.GetUpperBound(0)
			If myCol = 0
				cellData = dgvCPara(myCol, myRow).Value
			Else
				If myRow = p_FClbC(iCnt).iRow And myCol = p_FClbC(iCnt).iCol+1
					Select Case iCalMode
					Case 0	'フルキャリブ
						CellData = p_FClbC(iCnt).dPPhaseCX.ToString("F6")
					Case 1	'ゼロキャリブ
						CellData = p_ZClbC(iCnt).dPPhaseCX.ToString("F6")
					End Select
					iCnt += 1
				End If
			End If
			If myCol = sCol.GetUpperBound(0)
				textLine += CellData				' -- 最終列ならカンマを付けない
			Else
				textLine += cellData & ","			' -- 途中の列には項目の後にカンマを付加する
			End If
		Next
		textFile.WriteLine(textLine)			' -- 終端文字付きで１行を書き出す
	Next

	'円モード Y位置位相差の書込み
	textLine = sAreaId(eAreaId.CYPhase)		'識別子行を書き出す
	textFile.WriteLine(textLine)			' -- 終端文字付きで１行を書き出す
	textLine = ""							' -- 行の文字列をクリア
	' --- １行目（列見出し）を書き出す
	textFile.WriteLine(sColHeader(1)) ' -- 終端文字付きで１行を書き出す
	sCol = sColHeader(1).Split(",")
	iCnt = 0
	For myRow As Integer = 0 To dgvCPara.RowCount - 1
		textLine = ""											' -- 行の文字列をクリア
		For myCol As Integer = 0 To sCol.GetUpperBound(0)
			If myCol = 0
				cellData = dgvCPara(myCol, myRow).Value
			Else
				If myRow = p_FClbC(iCnt).iRow And myCol = p_FClbC(iCnt).iCol+1
					Select Case iCalMode
					Case 0	'フルキャリブ
						CellData = p_FClbC(iCnt).dPPhaseCY.ToString("F6")
					Case 1	'ゼロキャリブ
						CellData = p_ZClbC(iCnt).dPPhaseCY.ToString("F6")
					End Select
					iCnt += 1
				End If
			End If
			If myCol = sCol.GetUpperBound(0)
				textLine += CellData				' -- 最終列ならカンマを付けない
			Else
				textLine += cellData & ","			' -- 途中の列には項目の後にカンマを付加する
			End If
		Next
		textFile.WriteLine(textLine)			' -- 終端文字付きで１行を書き出す
	Next

	'点モード Z位置位相差の書込み
	textLine = sAreaId(eAreaId.ZPhase)		'識別子行を書き出す
	textFile.WriteLine(textLine)			' -- 終端文字付きで１行を書き出す
	textLine = ""							' -- 行の文字列をクリア
	' --- １行目（列見出し）を書き出す
	textFile.WriteLine(sColHeader(1)) ' -- 終端文字付きで１行を書き出す
	sCol = sColHeader(2).Split(",")
	iCnt = 0
	For myRow As Integer = 0 To dgvPPara.RowCount - 1
		textLine = ""											' -- 行の文字列をクリア
		For myCol As Integer = 0 To sCol.GetUpperBound(0)
			If myCol = 0
				cellData = dgvPPara(myCol, myRow).Value
			Else
				If myRow = p_FClbP(iCnt).iRow And myCol = p_FClbP(iCnt).iCol+1
					Select Case iCalMode
					Case 0	'フルキャリブ
						CellData = p_FClbP(iCnt).dPPhaseZ.ToString("F6")
					Case 1	'ゼロキャリブ
						CellData = p_ZClbP(iCnt).dPPhaseZ.ToString("F6")
					End Select
					iCnt += 1
				End If
			End If
			If myCol = sCol.GetUpperBound(0)
				textLine += CellData				' -- 最終列ならカンマを付けない
			Else
				textLine += cellData & ","			' -- 途中の列には項目の後にカンマを付加する
			End If
		Next
		textFile.WriteLine(textLine)			' -- 終端文字付きで１行を書き出す
	Next
#End If
	textFile.Close()			' -- StreamWriter を閉じて
	textFile.Dispose()			' -- StreamWriter を解放Return 0
	Return 0
Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Return -1
End Try
End Function
#If 0
''' <summary>
''' 較正結果（応力）をファイル保存
''' </summary>
''' <returns></returns>
Private Function StoreFile() As Int32
	Dim dgv			As DataGridView = Nothing
	Dim i			As Int16
	Dim textFile	As IO.StreamWriter			' -- 出力するファイル
　　Dim textLine	As String = Nothing			' -- 書き出す１行
　　Dim cellData	As String					' -- セルのデータ
Try
　　' --- ファイルのインスタンスを作る
　	textFile = New IO.StreamWriter(Path.Combine(p_SetPath, If(iCalMode=0,p_sFullStoreFileName,p_sZeroStoreFileName)), False, Encoding.UTF8)	' -- ファイル名、上書き、エンコーディング

	For i = 0 To 2
		Select Case i
		Case 0
			textLine = "X"			'識別子行を書き出す
			dgv = dgvXPara
			sClmHeader = sXClmHeader
		Case 1
			textLine = "C"			'識別子行を書き出す
			dgv = dgvCPara
			sClmHeader = sCClmHeader
		Case 2
			textLine = "P"			'識別子行を書き出す
			dgv = dgvPPara
			sClmHeader = sPClmHeader
		End Select
		textFile.WriteLine(textLine)			' -- 終端文字付きで１行を書き出す
		' ---① 列見出しを書き出す
		textLine = ""							' -- 行の文字列をクリア
		' --- １行目（列見出し）を書き出す
		For myCol As Integer = 0 To dgv.ColumnCount - 1
			If myCol = dgv.ColumnCount - 1 Then
					textLine += sClmHeader(myCol)			' -- 最終列ならカンマを付けない
			Else
				textLine += sClmHeader(myCol) & ","		' -- 途中の列ならカンマを付加
			End If
		Next
		textFile.WriteLine(textLine) ' -- 終端文字付きで１行を書き出す
		' ---② セルのデータを書き出す
		For myRow As Integer = 0 To dgv.RowCount - 1
			textLine = ""											' -- 行の文字列をクリア
			For myCol As Integer = 0 To dgv.ColumnCount - 1
				If (dgv(myCol, myRow).Value = Nothing) Then
					cellData = ""
				Else
					cellData = dgv(myCol, myRow).Value
				End If
				If (myCol = dgv.ColumnCount - 1) Then
					textLine += cellData				' -- 最終列ならカンマを付けない
				Else
					textLine += cellData & ","			' -- 途中の列には項目の後にカンマを付加する
				End If
			Next
			textFile.WriteLine(textLine) ' -- 終端文字付きで１行を書き出す
		Next
		' ---
	Next i
	textFile.Close()			' -- StreamWriter を閉じて
	textFile.Dispose()			' -- StreamWriter を解放Return 0
	Return 0
Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Return -1
End Try
End Function
#end if

#If 0
''' <summary>
''' 較正結果（比率・位相角）をファイル保存
''' </summary>
''' <param name="iMode">2:Alpha 3:x位置位相差 4:y位置位相差</param>
''' <returns></returns>
Private Function StoreFileRAL(Byval iMode As int16) As Int32
	Dim iCnt		As Int16
	Dim dgv			As DataGridView = Nothing
	Dim i			As Int16
	Dim textFile	As IO.StreamWriter			' -- 出力するファイル
　　Dim textLine	As String = Nothing			' -- 書き出す１行
　　Dim cellData	As String = Nothing			' -- セルのデータ
	Dim stClb()		As stPoint = Nothing		'動作 測定条件・結果
	Dim sFileName	As String = Nothing
Try
	Select Case iMode
	Case 2
		sFileName = p_sStoreFileNameAlpha
	Case 3
		sFileName = p_sStoreFileNamePxPhase
	Case 4
		sFileName = p_sStoreFileNamePyPhase
	Case 5
		sFileName = p_sStoreFileNamePzPhase
	End Select
　　' --- ファイルのインスタンスを作る
　	textFile = New IO.StreamWriter(Path.Combine(p_SetPath, sFileName), False, Encoding.UTF8)	' -- ファイル名、上書き、エンコーディング

	For i = 0 To 2
		Select Case i
		Case 0
			textLine = "X"			'識別子行を書き出す
			dgv = dgvXPara
			stClb = p_FClbX
			sClmHeader = sXClmHeader
		Case 1
			textLine = "C"			'識別子行を書き出す
			dgv = dgvCPara
			stClb = p_FClbC
			sClmHeader = sCClmHeader
		Case 2
			textLine = "P"			'識別子行を書き出す
			dgv = dgvPPara
			stClb = p_FClbP
			sClmHeader = sPClmHeader
		End Select
		textFile.WriteLine(textLine)			' -- 終端文字付きで１行を書き出す
		' ---① 列見出しを書き出す
		textLine = ""							' -- 行の文字列をクリア
		' --- １行目（列見出し）を書き出す
'		For myCol As Integer = 0 To dgv.ColumnCount - 1
		For myCol As Integer = 0 To sClmHeader.GetUpperBound(0)
			If myCol = sClmHeader.GetUpperBound(0) Then
				textLine += sClmHeader(myCol)			' -- 最終列ならカンマを付けない
			Else
				textLine += sClmHeader(myCol) & ","		' -- 途中の列ならカンマを付加
			End If
		Next
		textFile.WriteLine(textLine) ' -- 終端文字付きで１行を書き出す
		' ---② セルのデータを書き出す
		iCnt = 0
		For myRow As Integer = 0 To dgv.RowCount - 1
			textLine = ""											' -- 行の文字列をクリア
			For myCol As Integer = 0 To dgv.ColumnCount - 1
				If myCol = 0
					If (dgv(myCol, myRow).Value = Nothing) Then
						cellData = ""
					Else
						cellData = dgv(myCol, myRow).Value
					End If
				Else
					Select Case iMode
					Case 2
						CellData = stClb(iCnt).dAlpha.ToString("F6")
					Case 3
						CellData = stClb(iCnt).dPPhaseX.ToString("F6")
					Case 4
						CellData = stClb(iCnt).dPPhaseY.ToString("F6")
					Case 5
						CellData = stClb(iCnt).dPPhaseZ.ToString("F6")
					End Select
					iCnt += 1
				End If
				If (myCol = dgv.ColumnCount - 1) Then
					textLine += CellData				' -- 最終列ならカンマを付けない
				Else
					textLine += cellData & ","			' -- 途中の列には項目の後にカンマを付加する
				End If
			Next
			textFile.WriteLine(textLine) ' -- 終端文字付きで１行を書き出す
		Next
		' ---
	Next i
	textFile.Close()			' -- StreamWriter を閉じて
	textFile.Dispose()			' -- StreamWriter を解放Return 0
	Return 0
Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Return -1
End Try
End Function
#end if
''' <summary>
''' 測定箇所選択確定ボタン
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub btnXConf_Click(sender As Object, e As EventArgs) Handles btnXSel.Click, btnXClear.Click, btnCSel.Click,btnCClear.Click
Try
	Dim idx As Int16
	Dim dgv As DataGridView = Nothing
	Dim tx_clb() As stPoint = Nothing
	Dim tc_clb() As stPoint = Nothing
	Dim tp_clb() As stPoint = Nothing
	Dim clb() As stPoint = Nothing

	Dim bSelect As Boolean
	Dim iColor As Color
	tx_clb = If(iCalmode = 0, p_FClbX, p_ZClbX)
	tc_clb = If(iCalmode = 0, p_FClbC, p_ZClbC)
	tp_clb = If(iCalmode = 0, p_FClbP, p_ZClbP)
	Select Case tcPara.SelectedIndex
	Case 0
		select Case sender.name
		Case "btnCSel"		'Select
			dgv = dgvCPara
			clb = tc_clb
			bSelect = True
			iColor = Color.Yellow
		Case "btnCClear"	'Clear
			dgv = dgvCPara
			clb = tc_clb
			bSelect = False
			iColor = Color.White
		End Select
	Case 1
		select Case sender.name
		Case "btnCSel"		'Select
			dgv = dgvXPara
			clb = tx_clb
			bSelect = True
			iColor = Color.Yellow
		Case "btnCClear"	'Clear
			dgv = dgvXPara
			clb = tx_clb
			bSelect = False
			iColor = Color.White
		End Select
	Case 2
		select Case sender.name
		Case "btnCSel"		'Select
			dgv = dgvPPara
			clb = tp_clb
			bSelect = True
			iColor = Color.Yellow
		Case "btnCClear"	'Clear
			dgv = dgvPPara
			clb = tp_clb
			bSelect = False
			iColor = Color.White
		End Select
	Case Else
		Exit Sub
	End Select
#If 0
	select Case sender.name
	Case "btnXSel"		'Select
		dgv = dgvXPara
		clb = tx_clb
		bSelect = True
		iColor = Color.Yellow
	Case "btnCSel"		'Select
		dgv = dgvCPara
		clb = tc_clb
		bSelect = True
		iColor = Color.Yellow
	Case "btnXClear"	'Clear
		dgv = dgvXPara
		clb = tx_clb
		bSelect = False
		iColor = Color.White
	Case "btnCClear"	'Clear
		dgv = dgvCPara
		clb = tc_clb
		bSelect = False
		iColor = Color.White
	End Select
#end if
	'保持状態の設定
	For Each c As DataGridViewCell In dgv.SelectedCells
		If c.ColumnIndex <> 0	'1列目の表示はヘッダ部なのでスキップ
			idx = (dgv.ColumnCount - 1) * c.RowIndex + (c.ColumnIndex - 1)	'応力較正情報インデックス計算
			If idx < 0 Then
				dgv(c.ColumnIndex, c.RowIndex).Selected = False
				dgv(c.ColumnIndex, c.RowIndex).style.BackColor = Color.White
			Else
				clb(idx).bSelected = bSelect		'測定選択状態とする
				clb(idx).iCol = c.ColumnIndex		'グリッド座標位置を保存
				clb(idx).iRow = c.RowIndex
				dgv(c.ColumnIndex,c.RowIndex).Style.BackColor = iColor
			End If
			dgv(c.ColumnIndex, c.RowIndex).Selected = False					'選択表示状態を解除
			'Console.WriteLine("{0}, {1}, {2}", c.ColumnIndex, c.RowIndex, idx)
		End If
	Next
Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub
''' <summary>
''' 全セレクトボタン
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub btnCAllSel_Click(sender As Object, e As EventArgs) Handles btnCAllSelect.Click
	Dim idx As Int16
	Dim dgv As DataGridView = Nothing
	Dim clb() As stPoint = Nothing
Try
	Select Case tcPara.SelectedIndex
	Case 0
		dgv = dgvCPara
		clb = If(iCalMode=0, p_FClbC, p_ZClbC)
	Case 1
		dgv = dgvXPara
		clb = If(iCalMode=0, p_FClbX, p_ZClbX)
	Case 2
		dgv = dgvPPara
		clb = If(iCalMode=0, p_FClbP, p_ZClbP)
	Case Else
		Exit Sub
	End Select
	'保持状態の設定
	dgv.SelectAll
	For Each c As DataGridViewCell In dgv.SelectedCells
		If c.ColumnIndex <> 0	'1列目の表示はヘッダ部なのでスキップ
			idx = (dgv.ColumnCount - 1) * c.RowIndex + (c.ColumnIndex - 1)	'応力較正情報インデックス計算
			If idx < 0 Then
				dgv(c.ColumnIndex, c.RowIndex).Selected = False
				dgv(c.ColumnIndex, c.RowIndex).style.BackColor = Color.White
			Else
				clb(idx).bSelected = True			'測定選択状態とする
				clb(idx).iCol = c.ColumnIndex		'グリッド座標位置を保存
				clb(idx).iRow = c.RowIndex
				dgv(c.ColumnIndex,c.RowIndex).Style.BackColor = Color.Yellow
			End If
			dgv(c.ColumnIndex, c.RowIndex).Selected = False					'選択表示状態を解除
			'Console.WriteLine("{0}, {1}, {2}", c.ColumnIndex, c.RowIndex, idx)
		Else
			dgv(c.ColumnIndex, c.RowIndex).Selected = False
			dgv(c.ColumnIndex,c.RowIndex).Style.BackColor = Color.LightGray
		End If
	Next
Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub
''' <summary>
''' 選択全クリアボタン
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub btnXAllClear_Click(sender As Object, e As EventArgs) Handles btnXAllClear.Click, btnCAllClear.Click
	Dim i	As int16
	Dim j	As int16
	Dim idx As Int16
	Dim clb() As stPoint = Nothing
	Dim dgv	As DataGridView = Nothing
Try
	Select Case tcPara.SelectedIndex
	Case 0
		clb = If(iCalMode=0, p_FClbC, p_ZClbC)
		dgv  = dgvCPara
	Case 1
		clb = If(iCalMode=0, p_FClbX, p_ZClbX)
		dgv  = dgvXPara
	Case 2
		clb = If(iCalMode=0, p_FClbP, p_ZClbP)
		dgv  = dgvPPara
	Case Else
		Exit Sub
	End Select
	select Case sender.name
	Case "btnCAllClear"	
		For i = 1 To dgv.ColumnCount-1
			For j = 0 To dgv.RowCount-1
				idx = (dgv.ColumnCount - 1) * j + (i - 1)	'応力較正情報インデックス計算
				dgv(i,j).Selected = False					'選択解除
				dgv(i,j).Style.BackColor = Color.White		'背景 白
				clb(idx).bSelected = False					'選択解除とする
			Next
		Next
	End Select
Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub
#If 0
''' <summary>
''' 応力較正処理開始ボタン
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub btnExec_Click(sender As Object, e As EventArgs) Handles btnXExec.Click, btnCExec.Click
	Dim Xclb() As stPoint = Nothing
	Dim Cclb() As stPoint = Nothing
Try
	Xclb = If(iCalMode=0, p_FClbX, p_ZClbX)
	Cclb = If(iCalMode=0, p_FClbC, p_ZClbC)
	Select Case sender.name
	Case "btnXExec"
		Me.btnXStop.Enabled=True
		Me.btnXExec.Enabled = False
		Me.btnXSel.Enabled=False
		Me.btnXClear.Enabled = False
		Me.btnXAllClear.Enabled = False
		Me.btnCExec.Enabled = False
		Me.btnCStop.Enabled = False
		m_Rt.p_ClbX = Xclb
	    m_Rt.p_iBGWOrder = BGW_ORDER.CALIB_START_X

	Case "btnCExec"
		Me.btnCStop.Enabled=True
		Me.btnCExec.Enabled = False
		Me.btnCSel.Enabled=False
		Me.btnCClear.Enabled = False
		Me.btnCAllClear.Enabled = False
		Me.btnXExec.Enabled = False
		Me.btnXStop.Enabled = False
		m_Rt.p_ClbC = Cclb
	    m_Rt.p_iBGWOrder = BGW_ORDER.CALIB_START_C
	End Select

Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub
''' <summary>
''' 応力較正処理停止ボタン
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub btnStop_Click(sender As Object, e As EventArgs) Handles btnXStop.Click, btnCStop.Click
Try
	Select Case sender.name
	Case "btnXStop"
		Me.btnXExec.Enabled = True
		Me.btnXSel.Enabled=True
		Me.btnXClear.Enabled = True
		Me.btnXAllClear.Enabled = True
		Me.btnCExec.Enabled = True
		Me.btnCStop.Enabled = True
	Case "btnCStop"
		Me.btnCExec.Enabled = False
		Me.btnCSel.Enabled=True
		Me.btnCAllSelect.Enabled=True
		Me.btnCClear.Enabled = True
		Me.btnCAllClear.Enabled = True
		Me.btnXExec.Enabled = True
		Me.btnXStop.Enabled = True
	End Select
    m_Rt.p_iBGWOrder = BGW_ORDER.CALIB_STOP

Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub
#end If

Private m_iPreIndex As Int16 = -1			'条件配列インデックス変化確認用
Private m_SW1		As New Stopwatch		'XY動作待ちカウント
''' <summary>
''' 常時タイマーイベント処理
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub timMain_Tick(sender As Object, e As EventArgs) Handles timMain.Tick
	timMain.Enabled = False
	Try
	TimerIntervalProc()
	Catch ex As Exception
		Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
		Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	End Try
	timMain.Enabled = True
End Sub
Dim bColor			As Boolean = False
Dim bXYWaitComp		As Boolean =False		'キャリブレーション開始時のウェイト完了
Dim dCurrPeriod		As Double				'現在実行中の周期(秒)
Private Sub	TimerIntervalProc()
	Dim iCol As Int16
	Dim iRaw As Int16
	Dim iIdx As Int16
	Dim dgv As	DataGridView = Nothing
	Dim Xclb() As stPoint = Nothing
	Dim Cclb() As stPoint = Nothing
	Dim Pclb() As stPoint = Nothing
	Dim iStrsMode As Int32 = 0
	Dim iXSpan	As Int32 = 10 * 1000

Try
	If p_RTM(0).p_iBGWWork = BGW_WORK.AUTO_CONNECT
	'接続が切れた
		p_RTM(0).p_CaliblationState.iState	= RealTimeMeas.eCalibState.NOT_START
		p_RTM(0).p_MeasureState				= RealTimeMeas.eMeasStat.NOT_START
		p_RTM(0).CalJob						= RealTimeMeas.eClbJob.IDLE
		btnSOCheck.Enabled = True
		iError = 1
	Else
		iError = 0
	End If
	Xclb = If(iCalMode=0, p_FClbX, p_ZClbX)
	Cclb = If(iCalMode=0, p_FClbC, p_ZClbC)
	Pclb = If(iCalMode=0, p_FClbP, p_ZClbP)

	'キャリブレーション工程状態 RealTimeMeas.eCalibState
	Select case p_RTM(0).p_CaliblationState.iState
	Case RealTimeMeas.eCalibState.NOT_START
		btnZeroAdj.BackColor = Control.DefaultBackColor
		btnZeroAdj.Text = "Zero"
		'lblMsg.Text = "Zero Adjustment Ready"
		'lblMsg.Text = m_sStr(eMes.iCalibSSZeroStartOK)
		lblMsg.Text = ""
		if p_RTM(0).p_iZeroAdjOk = 1
		'ゼロ調完了時、開始ボタン有効
			btnXExec.Enabled = True
			btnCExec.Enabled = True
			btnExec.Enabled = True
		Else
			btnXExec.Enabled = False
			btnCExec.Enabled = False
			btnExec.Enabled = False
		End If
		lblAutoPeeling.Visible = False
		frmMain.ZeroMonitorJob(Me.Chart2, Me.Panel2)				'ゼロ調モニタ

	Case RealTimeMeas.eCalibState.ZERO_EXEC			'ゼロ調実行中
		lblMsg.Text = m_sStr(eMes.iCalibSSZeroPrePare)
		sbObjBackClrBlink(btnZeroAdj, Control.DefaultBackColor, Color.DarkOrange, bColor)
		If p_RTM(0).p_mltcp.IsZeroStartSW
			btnExec.Enabled = False
		End If
		frmMain.ZeroMonitorJob(Me.Chart2, Me.Panel2)				'ゼロ調モニタ
		m_SW1.Reset

	Case RealTimeMeas.eCalibState.ZERO_COMP			'ゼロ調完了
		btnZeroAdj.BackColor = Color.DarkOrange
		btnZeroAdj.Text = "Zero READY"
		if p_RTM(0).p_iZeroAdjOk = 1
		'ゼロ調完了時、開始ボタン有効
			btnXExec.Enabled = True
			btnCExec.Enabled = True
			btnExec.Enabled = True
			'lblMsg.Text = "Zero adjustment is complete. Can be measured"
			lblMsg.Text = m_sStr(eMes.iCalibSSZeroComp)
			bXYWaitComp = False									'キャリブレーション開始時のウェイト完了
		End If
		'm_fm.ZeroMonitorJob(Me.Chart2, Me.Panel2)				'ゼロ調モニタ
		frmMain.ZeroMonitorJob(Me.Chart2, Me.Panel2)				'ゼロ調モニタ
	
	Case RealTimeMeas.eCalibState.PREPAREING		'準備中
		Select case p_RTM(0).p_CaliblationState.sMode
		Case "X"
			btnXExec.BackColor = Color.YellowGreen
			btnCExec.BackColor = Control.DefaultBackColor
			iStrsMode = clSpec.eStrsSens.Line
		Case "C"
			btnCExec.BackColor = Color.YellowGreen
			btnXExec.BackColor = Control.DefaultBackColor
			iStrsMode = clSpec.eStrsSens.Circle
		Case "P"
			btnXExec.BackColor = Color.YellowGreen
			btnCExec.BackColor = Control.DefaultBackColor
			iStrsMode = clSpec.eStrsSens.Point
		End Select
		btnExec.BackColor = Color.YellowGreen
		btnXSel.Enabled = False
		btnCSel.Enabled = False
		btnCAllSelect.Enabled = False
		btnXClear.Enabled = False
		btncClear.Enabled = False
		btnXAllClear.Enabled = False
		btnCAllClear.Enabled = False
		btnExec.Enabled = False
		'lblMsg.Text = "Prepare for measurement"
		lblMsg.Text = m_sStr(eMes.iCalibSSMeasPrePare)
		m_SW1.Restart

	Case RealTimeMeas.eCalibState.COND_CONF			'条件インデックス確定
		Select Case p_RTM(0).p_CaliblationState.sMode
		Case "X"
			dgv = dgvXPara
			tcPara.SelectTab(1)
		Case "C"
			dgv = dgvCPara
			tcPara.SelectTab(0)
		Case "P"
			dgv = dgvPPara
			tcPara.SelectTab(2)
		End Select
		iCol = p_RTM(0).p_CaliblationState.iCol
		iRaw = p_RTM(0).p_CaliblationState.iRaw
		If iCol<0 Or iRaw<0 
			dgv(iCol, iRaw).Style.BackColor = Color.YellowGreen
		End If

		'XY動作待ちカウントダウン表示
		If Not bXYWaitComp
			If m_SW1.IsRunning
				Dim iTimerSec As Int32 = (p_RTM(0).p_mltcp.MesPreWait(1) + p_RTM(0).p_mltcp.MeasWaitTime(1) + p_RTM(0).p_mltcp.ClearTime(1) + 5) - m_SW1.Elapsed.TotalSeconds
				If iTimerSec < 0
					iTimerSec = 0
				End If
				lblMsg.Text = m_sStr(eMes.iCalibSSXYActWait) & iTimerSec.ToString("F0")
			End If
		End If



	Case RealTimeMeas.eCalibState.DATA_TRANS, RealTimeMeas.eCalibState.DispUpWait		'データ転送中、データ表示更新ウェイト
		m_SW1.Stop
		m_SW1.Reset
		bXYWaitComp = True
		iCol = p_RTM(0).p_CaliblationState.iCol
		iRaw = p_RTM(0).p_CaliblationState.iRaw
		Select Case p_RTM(0).p_CaliblationState.sMode
		Case "X"
			iIdx = p_RTM(0).p_ClbXIdx
			If Xclb(iIdx).bSelected
				sbObjBackClrBlink(btnXExec, Control.DefaultBackColor, Color.Yellow, bColor)
				'ステータスバーメッセージ
				Dim sMes As String = m_sStr(eMes.iCalibSSMeasMes)
				sMes = sMes.Replace("#PRD#",		Xclb(iIdx).dPeriodX.ToString("F1"))
				sMes = sMes.Replace("#AMP#",		Xclb(iIdx).dAmplitudeX.ToString("F3"))
				sMes = sMes.Replace("#DATACNT#",	p_RTM(0).p_iDataCount.ToString)
				sMes = sMes.Replace("#SAMPLE#",		Xclb(iIdx).iSample+(MEAS_BLOCK_DATA_NUM*Math.Ceiling(Xclb(iIdx).dPeriodX)).ToString)
				sMes = sMes.Replace("#DATE#",		m_dEndPrediction.ToString("yyyy/MM/dd HH:mm:ss"))
				lblMsg.Text = sMes
				'測定中セル色変更
				If iCol<>0  
					'frmmain.sbDgvBackClrBlink(dgvXPara, iCol, iRaw, Color.Yellow, Color.white)
					dgvXPara(iCol, iRaw).Style.BackColor=Color.YellowGreen
					dgvXPara(iCol, iRaw).Value = "measureing"
					dgvXPara.FirstDisplayedScrollingColumnIndex = iCol
					dgvXPara.FirstDisplayedScrollingRowIndex	= iRaw
					If p_RTM(0).p_ClbXIdx <> m_iPreIndex AndAlso m_iPreIndex >= 0
						'インデックス更新で前回結果表示
						dgvXPara(Xclb(m_iPreIndex).iCol, Xclb(m_iPreIndex).iRow).Value = Xclb(m_iPreIndex).iFullVal.ToString
						dgvXPara(Xclb(m_iPreIndex).iCol, Xclb(m_iPreIndex).iRow).Style.BackColor=Color.Yellow
						dgvXParaPh(Xclb(m_iPreIndex).iCol, Xclb(m_iPreIndex).iRow).Value = Xclb(m_iPreIndex).dPPhaseX.ToString
					End If
					m_iPreIndex = p_RTM(0).p_ClbXIdx
					dCurrPeriod = Xclb(iIdx).dPeriodX
				End If
			End If
			iStrsMode = clSpec.eStrsSens.Line
			iXSpan = If(Xclb(iIdx).dPeriodX=0,  1 * cCalibAllPeriodNum * 1000, Xclb(iIdx).dPeriodX * cCalibAllPeriodNum * 1000)
		Case "C"
			iIdx = p_RTM(0).p_ClbCIdx
			If Cclb(iIdx).bSelected	
				sbObjBackClrBlink(btnCExec, Control.DefaultBackColor, Color.Yellow, bColor)
				'ステータスバーメッセージ
				Dim sMes As String = m_sStr(eMes.iCalibSSMeasMes)
				sMes = sMes.Replace("#PRD#",		Cclb(iIdx).dPeriodX.ToString("F1"))
				sMes = sMes.Replace("#AMP#",		Cclb(iIdx).dAmplitudeX.ToString("F3"))
				sMes = sMes.Replace("#DATACNT#",	p_RTM(0).p_iDataCount.ToString)
				sMes = sMes.Replace("#SAMPLE#",		Cclb(iIdx).iSample+(MEAS_BLOCK_DATA_NUM*Math.Ceiling(Cclb(iIdx).dPeriodX)).ToString)
				sMes = sMes.Replace("#DATE#",		m_dEndPrediction.ToString("yyyy/MM/dd HH:mm:ss"))
				lblMsg.Text = sMes
				'測定中セルブリンク
				'frmmain.sbDgvBackClrBlink(dgvCPara, iCol, iRaw, Color.Yellow, Color.white)
				dgvCPara(iCol, iRaw).Style.BackColor=Color.YellowGreen
				dgvCPara(iCol, iRaw).Value = "measureing"
				dgvCPara.FirstDisplayedScrollingColumnIndex = iCol
				dgvCPara.FirstDisplayedScrollingRowIndex	= iRaw
				If p_RTM(0).p_ClbCIdx <> m_iPreIndex AndAlso m_iPreIndex >= 0
					'インデックス更新で前回結果表示
					dgvCPara(Cclb(m_iPreIndex).iCol, Cclb(m_iPreIndex).iRow).Value = Cclb(m_iPreIndex).iFullVal.ToString
					dgvCPara(Cclb(m_iPreIndex).iCol, Cclb(m_iPreIndex).iRow).Style.BackColor=Color.Yellow
					dgvCParaPhX(Cclb(m_iPreIndex).iCol, Cclb(m_iPreIndex).iRow).Value = Cclb(m_iPreIndex).dPPhaseCX.ToString
					dgvCParaPhY(Cclb(m_iPreIndex).iCol, Cclb(m_iPreIndex).iRow).Value = Cclb(m_iPreIndex).dPPhaseCY.ToString
				End If
				m_iPreIndex = p_RTM(0).p_ClbCIdx
				dCurrPeriod = Cclb(iIdx).dPeriodX
			End If
			iStrsMode = clSpec.eStrsSens.Circle
			iXSpan = If(Cclb(iIdx).dPeriodX=0,  1 * cCalibAllPeriodNum * 1000, Cclb(iIdx).dPeriodX * cCalibAllPeriodNum * 1000)
		Case "P"
			iIdx = p_RTM(0).p_ClbPIdx
			If Pclb(iIdx).bSelected
				sbObjBackClrBlink(btnXExec, Control.DefaultBackColor, Color.Yellow, bColor)
				'ステータスバーメッセージ
				Dim sMes As String = m_sStr(eMes.iCalibSSMeasMes)
				sMes = sMes.Replace("#PRD#",		Pclb(iIdx).dPeriodZ.ToString("F1"))
				sMes = sMes.Replace("#AMP#",		Pclb(iIdx).dAmplitudeZ.ToString("F3"))
				sMes = sMes.Replace("#DATACNT#",	p_RTM(0).p_iDataCount.ToString)
				sMes = sMes.Replace("#SAMPLE#",		Pclb(iIdx).iSample+(MEAS_BLOCK_DATA_NUM*Math.Ceiling(Pclb(iIdx).dPeriodZ)).ToString)
				sMes = sMes.Replace("#DATE#",		m_dEndPrediction.ToString("yyyy/MM/dd HH:mm:ss"))
				lblMsg.Text = sMes
				'測定中セル色変更
				If iCol<>0  
					'frmmain.sbDgvBackClrBlink(dgvXPara, iCol, iRaw, Color.Yellow, Color.white)
					dgvPPara(iCol, iRaw).Style.BackColor=Color.YellowGreen
					dgvPPara(iCol, iRaw).Value = "measureing"
					dgvPPara.FirstDisplayedScrollingColumnIndex = iCol
					dgvPPara.FirstDisplayedScrollingRowIndex	= iRaw
					If p_RTM(0).p_ClbPIdx <> m_iPreIndex AndAlso m_iPreIndex >= 0
						'インデックス更新で前回結果表示
						dgvPPara(Pclb(m_iPreIndex).iCol, Pclb(m_iPreIndex).iRow).Value = Pclb(m_iPreIndex).iFullVal.ToString
						dgvPPara(Pclb(m_iPreIndex).iCol, Pclb(m_iPreIndex).iRow).Style.BackColor=Color.Yellow
						dgvPParaPh(Pclb(m_iPreIndex).iCol, Pclb(m_iPreIndex).iRow).Value = Pclb(m_iPreIndex).dPPhaseZ.ToString
					End If
					m_iPreIndex = p_RTM(0).p_ClbPIdx
					dCurrPeriod = Pclb(iIdx).dPeriodX
				End If
			End If
			iStrsMode = clSpec.eStrsSens.Point
			iXSpan = If(Pclb(iIdx).dPeriodZ=0, 1 * cCalibAllPeriodNum * 1000, Pclb(iIdx).dPeriodZ * cCalibAllPeriodNum * 1000)
		End Select
		sbObjBackClrBlink(btnExec, Control.DefaultBackColor, Color.Yellow, bColor)

		'データテーブルセット
		DataTabeleSet(p_dtFile)
		'グラフ更新
		XYWaveChartDisp( ChartStress, ds, iXSpan, iStrsMode )
		CorrectChartDisp( ChartHosei, ds, iXSpan, p_cts )
		p_RTM(0).p_DispUpdate += 1



	Case RealTimeMeas.eCalibState.FINISHING		'終了中
		iCol = p_RTM(0).p_CaliblationState.iCol
		iRaw = p_RTM(0).p_CaliblationState.iRaw
		'結果全更新
		Select Case p_RTM(0).p_CaliblationState.sMode
		Case "X"
			For i = 0 To Xclb.GetUpperBound(0)
				If Xclb(i).bSelected
					dgvXPara(Xclb(i).iCol, Xclb(i).iRow).Value = Xclb(i).iFullVal.ToString
					dgvXPara(Xclb(i).iCol, Xclb(i).iRow).Style.BackColor=Color.Yellow
					dgvXParaPh(Xclb(i).iCol, Xclb(i).iRow).Value = Xclb(i).dPPhaseX.ToString("F6")
				End If
			Next
			btnXExec.BackColor = Color.White
		Case "C"
			For i = 0 To Cclb.GetUpperBound(0)
				If Cclb(i).bSelected
					dgvCPara(Cclb(i).iCol, Cclb(i).iRow).Value = Cclb(i).iFullVal.ToString
					dgvCPara(Cclb(i).iCol, Cclb(i).iRow).Style.BackColor=Color.Yellow
					dgvCParaPhX(Cclb(i).iCol, Cclb(i).iRow).Value = Cclb(i).dPPhaseCX.ToString("F6")
					dgvCParaPhY(Cclb(i).iCol, Cclb(i).iRow).Value = Cclb(i).dPPhaseCY.ToString("F6")
				End If
			Next
			btnCExec.BackColor = Color.White
		Case "P"
			For i = 0 To Pclb.GetUpperBound(0)
				If Pclb(i).bSelected
					dgvPPara(Pclb(i).iCol, Pclb(i).iRow).Value = Pclb(i).iFullVal.ToString
					dgvPPara(Pclb(i).iCol, Pclb(i).iRow).Style.BackColor=Color.Yellow
					dgvPParaPh(Pclb(i).iCol, Pclb(i).iRow).Value = Pclb(i).dPPhaseZ.ToString("F6")
				End If
			Next
			btnXExec.BackColor = Color.White
		End Select
		m_iPreIndex = -1
		'lblMsg.Text = "Measurement finished"
		'lblMsg.Text = m_sStr(p_RTM(0).p_CaliblationState.sMode & "-Mode" & eMes.iCalibSSMeasComp)

		StoreFile()			'測定結果をファイル上書き保存


		'終了時のボタン状態
		btnXExec.BackColor = DefaultBackColor
		btnCExec.BackColor = DefaultBackColor
		btnExec.BackColor = DefaultBackColor

		btnXExec.Enabled = True
		btnCExec.Enabled = True
		btnXStop.Enabled = True
		btnCStop.Enabled = True

		btnXSel.Enabled = True
		btnXClear.Enabled = True
		btnXAllClear.Enabled = True
		btnCSel.Enabled = True
		btnCAllSelect.Enabled = True
		btnCClear.Enabled = True
		btnCAllClear.Enabled = True
		btnSOCheck.Enabled	= True

		'リアルタイム側で設定しているが何故か変わらないのでここで強制セット 20230912
		p_RTM(0).p_CaliblationState.iState = RealTimeMeas.eCalibState.NOT_START




	Case RealTimeMeas.eCalibState.AUTO_PEELING, RealTimeMeas.eCalibState.MANU_PEELING		'自動・手動剥がし
		Select Case p_RTM(0).p_iBGWOrder
		Case BGW_ORDER.NO_ORDER
			p_RTM(0).p_CaliblationState.iState = RealTimeMeas.eCalibState.NOT_START
			lblAutoPeeling.Visible = False
			lblMsg.Text = ""
		Case Else
			lblAutoPeeling.Visible = True
			'lblMsg.Text = "Running auto peeling."
			lblMsg.Text = m_sStr(eMes.iCalibSSAutoPeel)
		End Select

	Case Else

	End Select
	'リアルタイム値更新
	RealTimeMonitorValDispUpdate(dgvCurrVal, p_RTM(0).p_mltcp.P_StressZeroEnable, 0)

	'原点復帰ボタン
	if p_RTM(0).p_mltcp.IsPosOrderOrigin
		sbObjBackClrBlink(btnOrigin, Control.DefaultBackColor, Color.DarkOrange, bColor)
	Else
		btnOrigin.BackColor = Control.DefaultBackColor
	End If
	If p_RTM(0).p_mltcp.StartSW  = 0 Then		
		btnOrigin.Enabled = True		'原点復帰ボタン有効  ゼロ調、測定中以外なら
	Else
		btnOrigin.Enabled = False
	End If
	'脱調確認ボタン
	btnSOCheck.BackColor = Control.DefaultBackColor
	If p_RTM(0).p_mltcp.StartSW  = 0 Then		
		btnSOCheck.Enabled = True		'原点復帰ボタン有効  ゼロ調、測定中以外なら
	Else
		btnSOCheck.Enabled = False
	End If

	'温度ボタン
	Dim iTempBtnState As Int16 = IIf( (p_RTM(0).p_iHeatRdy = 3) And ((p_RTM(0).p_mltcp.isStatHeaterOn = True) Or (p_RTM(0).p_mltcp.isStatHeaterOn2 = True)), 1, 0 )
	If iTempBtnState = 0
		If p_RTM(0).p_mltcp.isStatHeaterOn Or p_RTM(0).p_mltcp.isStatHeaterOn2
			sbObjBackClrBlink(btnTemperature, Control.DefaultBackColor, Color.DarkOrange, bColor)
		Else
			btnTemperature.BackColor = Control.DefaultBackColor
		End if
		btnTemperature.Text = "Temp"
	Else
		btnTemperature.BackColor = Color.DarkOrange
		btnTemperature.Text = "Temp READY"
	End If

	'ゼロ調ボタン
	If p_RTM(0).p_iZeroAdjOk <= 0
		If Not p_bZStrStabDis		
			If Math.abs(frmMain.dZStability) < p_cts.SGStressStab
				btnZeroadj.Enabled = True                      'ゼロ調ボタン有効
			Else
				btnZeroadj.Enabled = False
			End If
		Else
			btnZeroadj.Enabled = True                      'ゼロ調ボタン有効
		End If
	ElseIf p_RTM(0).p_iZeroAdjOk = 1
		btnZeroadj.Enabled = True	                     'ゼロ調ボタン有効
		btnZeroAdj.BackColor = Color.DarkOrange
		btnZeroAdj.Text = "Zero READY"
	End If

	If	p_RTM(0).p_bDevError AndAlso m_bPreDevError <> p_RTM(0).p_bDevError
	'停止ボタン
		CalibStopBtn()
	End If
	m_bPreDevError = p_RTM(0).p_bDevError

	bColor = Not bColor
Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub
Private m_bPreDevError As Boolean = False
Private m_Job As Int32 = -1

'モニタ用データ取得シーケンス処理
Sub DataTabeleSet(ByVal dfil As Datafile)
Dim dtRow As DataRow = Nothing
Try
    Select Case m_Job
    Case -1
        m_Job = 0 

    Case 0
	'モニタ開始待ち
		If p_RTM(0).p_CaliblationState.iState = RealTimeMeas.eCalibState.DATA_TRANS
            m_Job = 1
			m_SW1.Restart()
		End If

    Case 1
    'モニタ中処理
		If p_RTM(0).p_CaliblationState.iState = RealTimeMeas.eCalibState.DATA_TRANS
			If NOt IsNothing(dfil.iHosei)
				ds.Tables(sDataTblName(etbleNum.iStrs)).Clear
				Dim d As Double = MEAS_BLOCK_DATA_NUM * 1 * dCurrPeriod
				For i = 0 To dfil.iHosei.Length - 1 - d
					'補正値テーブル
					dtRow = ds.Tables(sDataTblName(etbleNum.iStrs)).NewRow
					dtRow(0) = 2 * i
					dtRow(1) = dfil.iHosei(i+d)
					ds.Tables(sDataTblName(etbleNum.iStrs)).Rows.Add(dtRow)

				Next i
			Else
			'モニタ中止
				m_Job = 9
			End if
			If NOt IsNothing(datCal.TempX_QM)
				'応力テーブル
				ds.Tables(sDataTblName(etbleNum.iWave)).Clear
				'Dim d As Double = 0 * dCurrPeriod
				Dim d As Double = MEAS_BLOCK_DATA_NUM * 1 * dCurrPeriod
				For i = 0 To datCal.TempX_QM.Length - 1
					dtRow = ds.Tables(sDataTblName(etbleNum.iWave)).NewRow
					dtRow(0) = 2 * i
					Select Case p_RTM(0).p_CaliblationState.sMode
					Case "X", "P"
						If NOt IsNothing(datCal.TempX_QM)
						Try
							dtRow(1) = If(Double.IsNaN(datCal.TempX_QM(i+d)),0,datCal.TempX_QM(i+d))
							dtRow(2) = 0
						Catch ex As Exception
							'Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
							Exit Sub
						End Try
						End If
					Case "C"
						If NOt IsNothing(datCal.TempX_QM) And NOt IsNothing(datCal.TempY_QM)
						Try
							dtRow(1) = If(Double.IsNaN(datCal.TempX_QM(i+d)),0,datCal.TempX_QM(i+d))
							dtRow(2) = If(Double.IsNaN(datCal.TempY_QM(i+d)),0,datCal.TempY_QM(i+d))
						Catch ex As Exception
							'Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
							Exit Sub
						End Try
						End If
					End Select
					ds.Tables(sDataTblName(etbleNum.iWave)).Rows.Add(dtRow)
				Next i
			Else
			'モニタ中止
				m_Job = 9
			End if
		Else
		'モニタ中止
			m_Job = 9
		End if

    Case 8
    Case 9
		'制御初期化
		m_Job = 0
		dtHosei.Clear
		dtStress.Clear

    End Select
Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub
''' <summary>
''' 波形グラフ表示更新
''' </summary>
''' <param name="oChart"></param>
''' <param name="ds"></param>
Private Sub XYWaveChartDisp(ByRef oChart As Chart, ByVal ds As Dataset, byval iXSpan As Uint32 , ByVal iActMode As int16)
Dim sSeriesName As String
Dim sTbl		As String = sDataTblName(etbleNum.iWave)
Try
	'if ds.Tables(sTbl).Rows.Count = 0
	'	Exit sub
	'End If
'チャートコントロールの設定
	With oChart
	'初期化
	.Series.Clear()
	.Legends.Clear()
	.Titles.Clear()
	.ChartAreas.Clear()
	.DataSource = ds.Tables(sTbl)


    'timeとX,Yを取得
	Select Case iActMode
	Case clSpec.eStrsSens.Line, clSpec.eStrsSens.Point
		sSeriesName = ds.Tables(sTbl).Columns(1).ColumnName			'XData/ZData
		.Series.Add(sSeriesName)
		.Series(sSeriesName).YValueMembers = sSeriesName

		.Series(sSeriesName).ChartType = SeriesChartType.FastLine
		.Series(sSeriesName).XValueMember = ds.Tables(sTbl).Columns(0).ColumnName	'時間

		.Series(sSeriesName).YAxisType = AxisType.Primary
		.Series(sSeriesName).Color = Color.Blue
	Case clSpec.eStrsSens.Circle
		For i = 1 To 2
			sSeriesName = ds.Tables(sTbl).Columns(i).ColumnName
			.Series.Add(sSeriesName)
			.Series(sSeriesName).YValueMembers = sSeriesName

			.Series(sSeriesName).ChartType = SeriesChartType.FastLine
			.Series(sSeriesName).XValueMember = ds.Tables(sTbl).Columns(0).ColumnName	'時間
			Select Case i
			Case 1
				.Series(sSeriesName).YAxisType = AxisType.Primary
				.Series(sSeriesName).Color = Color.Blue
			Case 2
				.Series(sSeriesName).YAxisType = AxisType.Primary
				.Series(sSeriesName).Color = Color.Green
			End Select
		Next i
	End Select

	'TITLE 作成
	Dim title1 As Title = new Title(m_sStr(eMes.iWAVE_MON))
	.Titles.Add(title1)
	.Titles.Item(0).Font = New Font(BASE_FONT, 16, FontStyle.Bold)
    'ChartArea作成
    .ChartAreas.Add("ChartArea1")
	'Legendの作成
	'leg.DockedToChartArea = "ChartArea1"
	Dim leg As Legend = new Legend With {
		.Alignment = StringAlignment.Near
	}
	.Legends.Add(leg)
	.Legends(0).Enabled = false


		
    '-----------------------------------------------------------------------------------------------------
    '以下、Ｘ軸とＹ軸の設定
    .ChartAreas(0).AxisX.Maximum =  iXSpan
	'If ds.Tables(sTbl).Rows.Count-iXSpan <= 0
	'	.ChartAreas(0).AxisX.Minimum =  0
	'Else
	'	.ChartAreas(0).AxisX.Minimum =  ds.Tables(sTbl).Rows.Count - iXSpan
	'End If
	.ChartAreas(0).AxisX.Minimum =  0
	Dim YMax0 As Double = 0
	Dim YMax1 As Double = 0
	Dim YMax2 As Double = 0
	Dim YMin0 As Double = 0
	Dim YMin1 As Double = 0
	Dim YMin2 As Double = 0

	Dim YMax As Double = 0
	Dim YMin As Double = 0
	Select Case iActMode
	Case clSpec.eStrsSens.Line, clSpec.eStrsSens.Point
		if ds.Tables(sTbl).Rows.Count > 1
			YMax = Convert.ToDouble(ds.Tables(sTbl).Compute("MAX(XData)", ""))
			YMin = Convert.ToDouble(ds.Tables(sTbl).Compute("MIN(XData)", ""))
			If Double.IsNaN(YMax) or Double.IsNaN(YMin)
			ElseIf YMax <> YMin
				.ChartAreas(0).AxisY.Maximum = YMax
				.ChartAreas(0).AxisY.Minimum = YMin
			Else
				.ChartAreas(0).AxisY.Maximum = 1000
				.ChartAreas(0).AxisY.Minimum = -1000
			End If
		Else
			.ChartAreas(0).AxisY.Maximum = 1000
			.ChartAreas(0).AxisY.Minimum = -1000
		End If
	Case clSpec.eStrsSens.Circle
		if ds.Tables(sTbl).Rows.Count > 1
			YMax0 = Convert.ToDouble(ds.Tables(sTbl).Compute("MAX(XData)", ""))
			YMax1 = Convert.ToDouble(ds.Tables(sTbl).Compute("MAX(YData)", ""))
			YMin0 = Convert.ToDouble(ds.Tables(sTbl).Compute("MIN(XData)", ""))
			YMin1 = Convert.ToDouble(ds.Tables(sTbl).Compute("MIN(YData)", ""))
			YMax2 = iif(YMax0>YMax1, YMax0, YMax1)
			YMin2 = iif(YMin0>YMin1, YMin1, YMin0)

			YMax = if(.ChartAreas(0).AxisY.Maximum>YMax2, .ChartAreas(0).AxisY.Maximum, YMax2)
			YMin = if(.ChartAreas(0).AxisY.Minimum<YMin2, .ChartAreas(0).AxisY.Minimum, YMin2)
			If Double.IsNaN(YMax) or Double.IsNaN(YMin)
			ElseIf YMax > YMin
				.ChartAreas(0).AxisY.Maximum = YMax
				.ChartAreas(0).AxisY.Minimum = YMin
			End If
		Else
			.ChartAreas(0).AxisY.Maximum = 1000
			.ChartAreas(0).AxisY.Minimum = -1000
		End If
	End Select


    .ChartAreas(0).AxisX.Title                      = m_sStr(eMes.iMON_X)
    .ChartAreas(0).AxisX.TitleFont                  = New Font(BASE_FONT, 12, FontStyle.Regular)
	.ChartAreas(0).AxisX.Enabled					= AxisEnabled.Auto
    .ChartAreas(0).AxisX.Crossing                   = Double.NaN
	.ChartAreas(0).AxisX.IntervalType               = DateTimeIntervalType.NotSet
	.ChartAreas(0).AxisX.MajorGrid.Enabled          = True
	.ChartAreas(0).AxisX.MajorGrid.LineDashStyle    = ChartDashStyle.Dot
    .ChartAreas(0).AxisX.MajorGrid.LineColor        = Color.LightGray
	.ChartAreas(0).AxisX.Interval					= iXSpan / 10


	.ChartAreas(0).AxisY.Title                      = m_sStr(eMes.iWAVE_MON_Y)
    .ChartAreas(0).AxisY.TitleFont                  = New Font(BASE_FONT, 12, FontStyle.Regular)
    .ChartAreas(0).AxisY.Crossing                   = Double.NaN
	.ChartAreas(0).AxisY.IntervalType               = DateTimeIntervalType.NotSet
	.ChartAreas(0).AxisY.MajorGrid.Enabled          = True
	.ChartAreas(0).AxisY.MajorGrid.LineDashStyle    = ChartDashStyle.Dot
    .ChartAreas(0).AxisY.MajorGrid.LineColor        = Color.LightGray

	'-------------------------------------------------------------------------------------------------------------
	.ChartAreas(0).Position.Auto					= false
	.ChartAreas(0).Position.X						= 2
	.ChartAreas(0).Position.Y						= 20
	.ChartAreas(0).Position.height					= 78
	.ChartAreas(0).Position.Width					= 97
	Try
	.DataBind()
	Catch ex As Exception
	End Try

    End With
Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub
''' <summary>
''' 応力補正グラフ表示更新
''' </summary>
''' <param name="oChart"></param>
''' <param name="ds"></param>
Private Sub CorrectChartDisp(ByRef oChart As Chart, ByVal ds As Dataset, byval iXSpan As Uint32, ByVal cts As CToolSettings )
Dim sSeriesName As String
Dim sTbl		As String = sDataTblName(etbleNum.iStrs)
Try
	'if ds.Tables(sTbl).Rows.Count = 0
	'	Exit sub
	'End If
	'If ds.Tables(sTbl).Rows.Count < iXSpan / 1000 * 500
	'	Exit sub
	'End If
'チャートコントロールの設定
	With oChart
	'初期化
	.Series.Clear()
	.Legends.Clear()
	.Titles.Clear()
	.ChartAreas.Clear()
	.DataSource = ds.Tables(sTbl)

    'timeとX,Yを取得
	For i = 1 To 1
		sSeriesName = ds.Tables(sTbl).Columns(i).ColumnName
		.Series.Add(sSeriesName)
		.Series(sSeriesName).YValueMembers = sSeriesName

		.Series(sSeriesName).ChartType = SeriesChartType.FastLine
		'.Series(sSeriesName).ChartType = SeriesChartType.FastPoint
		.Series(sSeriesName).XValueMember = ds.Tables(sTbl).Columns(0).ColumnName

		Select Case i
		Case 1
			.Series(sSeriesName).YAxisType = AxisType.Primary
			.Series(sSeriesName).Color = Color.Blue
		End Select
	Next i

	'TITLE 作成
	Dim title1 As Title = new Title(m_sStr(eMes.iSTRESS_MON))
	.Titles.Add(title1)
	.Titles.Item(0).Font = New Font(BASE_FONT, 16, FontStyle.Bold)
    'ChartArea作成
    .ChartAreas.Add("ChartArea1")
	'Legendの作成
	'leg.DockedToChartArea = "ChartArea1"
	Dim leg As Legend = new Legend With {
		.Alignment = StringAlignment.Near
	}
	.Legends.Add(leg)
	.Legends(0).Enabled = false


		
    '-----------------------------------------------------------------------------------------------------
    '以下、Ｘ軸とＹ軸の設定
    .ChartAreas(0).AxisX.Maximum =  iXSpan
	'If ds.Tables(sTbl).Rows.Count-iXSpan <= 0
	'	.ChartAreas(0).AxisX.Minimum =  0
	'Else
	'	.ChartAreas(0).AxisX.Minimum =  ds.Tables(sTbl).Rows.Count - iXSpan
	'End If
	.ChartAreas(0).AxisX.Minimum =  0
	Dim YMax0 As Double = 0
	Dim YMax1 As Double = 0
	Dim YMax2 As Double = 0
	Dim YMin0 As Double = 0
	Dim YMin1 As Double = 0
	Dim YMin2 As Double = 0
	if ds.Tables(sTbl).Rows.Count > 1
		YMax0 = Convert.ToDouble(ds.Tables(sTbl).Compute("MAX(Hosei)", ""))
		YMin0 = Convert.ToDouble(ds.Tables(sTbl).Compute("MIN(Hosei)", ""))
		If (YMax0 <= YMin0) Or Double.IsNaN(YMax0) Or Double.IsNaN(YMax1)
			.ChartAreas(0).AxisY.Maximum = 1000
			.ChartAreas(0).AxisY.Minimum = -1000
		Else	
			.ChartAreas(0).AxisY.Maximum = YMax0
			.ChartAreas(0).AxisY.Minimum = YMin0
		End If
	Else
		.ChartAreas(0).AxisY.Maximum = 1000
		.ChartAreas(0).AxisY.Minimum = -1000
	End If
    
    .ChartAreas(0).AxisX.Title                      = m_sStr(eMes.iMON_X)
    .ChartAreas(0).AxisX.TitleFont                  = New Font(BASE_FONT, 12, FontStyle.Regular)
	.ChartAreas(0).AxisX.Enabled					= AxisEnabled.Auto
    .ChartAreas(0).AxisX.Crossing                   = Double.NaN
	.ChartAreas(0).AxisX.IntervalType               = DateTimeIntervalType.NotSet
	.ChartAreas(0).AxisX.MajorGrid.Enabled          = True
	.ChartAreas(0).AxisX.MajorGrid.LineDashStyle    = ChartDashStyle.Dot
    .ChartAreas(0).AxisX.MajorGrid.LineColor        = Color.LightGray
	.ChartAreas(0).AxisX.Interval					= iXSpan / 10


   	.ChartAreas(0).AxisY.Title						= m_sStr(eMes.iSTRESS_MON_Y)
    .ChartAreas(0).AxisY.TitleFont                  = New Font(BASE_FONT, 12, FontStyle.Regular)
    .ChartAreas(0).AxisY.Crossing                   = Double.NaN
	.ChartAreas(0).AxisY.IntervalType               = DateTimeIntervalType.NotSet
	.ChartAreas(0).AxisY.MajorGrid.Enabled          = True
	.ChartAreas(0).AxisY.MajorGrid.LineDashStyle    = ChartDashStyle.Dot
    .ChartAreas(0).AxisY.MajorGrid.LineColor        = Color.LightGray
	.ChartAreas(0).AxisY.LabelStyle.Format			= "0.0"


	'-------------------------------------------------------------------------------------------------------------
	.ChartAreas(0).Position.Auto					= false
	.ChartAreas(0).Position.X						= 2
	.ChartAreas(0).Position.Y						= 20
	.ChartAreas(0).Position.height					= 78
	.ChartAreas(0).Position.Width					= 97
	Try
	.DataBind()
	Catch ex As Exception
	End Try

    End With
Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub


''' <summary>
''' フォームFormClosingイベント
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub frmCalib_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing

Try
	'「閉じる」ボタンが押された時
	If e.CloseReason = CloseReason.UserClosing Then
		'オンラインの場合
		If p_RTM(0).p_iConnectState = CONNECT_ST.CONNECTING Then
			'実行中(ゼロ調開始から)の間はフォームを閉じない
			'If p_RTM(0).p_CaliblationState.iState > RealTimeMeas.eCalibState.ZERO_COMP _
			If p_RTM(0).p_CaliblationState.iState >= RealTimeMeas.eCalibState.ZERO_EXEC _
			Then
				e.Cancel = True
				Exit Sub
			End If
		End If
		If iError = 0
			StoreFile()			'測定結果をファイル上書き保存
		End if
	End If
	p_FormCalibVisible = False

	'温度制御開始Bit OFF
	p_RTM(0).p_mltcp.StartTempCont = 0                                   

	'全ての選択状態等をクリア
	Dim i As int32
	Dim j As int32
	Dim k As int32
	Dim dgv As DataGridView
	Dim clb() As stPoint
	Dim idx	  As Int32
	For k = 0 To 2
		Select Case k
		Case 0
			clb = If(iCalMode=0, p_FClbC, p_ZClbC)
			dgv  = dgvCPara
		Case 1
			clb = If(iCalMode=0, p_FClbX, p_ZClbX)
			dgv  = dgvXPara
		Case 2
			clb = If(iCalMode=0, p_FClbP, p_ZClbP)
			dgv  = dgvPPara
		Case Else
			clb = If(iCalMode=0, p_FClbC, p_ZClbC)
			dgv  = dgvCPara
		End Select
		For i = 1 To dgv.ColumnCount-1
			For j = 0 To dgv.RowCount-1
				idx = (dgv.ColumnCount - 1) * j + (i - 1)	'応力較正情報インデックス計算
				dgv(i,j).Selected = False					'選択解除
				dgv(i,j).Style.BackColor = Color.White		'背景 白
				clb(idx).bSelected = False					'選択解除とする
			Next
		Next
	Next


Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub


''' <summary>
''' 脱調確認ボタン
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub btnSOCheck_Click(sender As Object, e As EventArgs) Handles btnSOCheck.Click
    Call WriteLog("Calib 脱調確認ボタンクリック " & System.Reflection.MethodBase.GetCurrentMethod.Name, eLOG_LEVEL.iINFO)
	If p_RTM(0).p_iBGWOrder = BGW_ORDER.NO_ORDER
		'脱調確認
		p_RTM(0).p_iBGWOrder = BGW_ORDER.OutOfStepConf_REP                                      '脱調確認シーケンスへ
	End If
#If 0
	'実行中の間はフォームを閉じない
	If p_RTM(0).p_CaliblationState.iState >= RealTimeMeas.eCalibState.NOT_START And _
		p_RTM(0).p_CaliblationState.iState <= RealTimeMeas.eCalibState.ZERO_COMP _
	Then
		If iError = 0
			StoreFile()			'測定結果をファイル上書き保存
		End if
		Me.Close
	End If
#end if
End Sub

''' <summary>
''' フォーム表示イベント
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub frmCalib_Shown(sender As Object, e As EventArgs) Handles Me.Shown

	Dim i As Int16
Try
	dgvXPara.CurrentCell = Nothing	'選択表示状態をクリア
	'最左列の背景色
	For i = 0 To dgvXPara.RowCount - 1
		dgvXPara(0, i).Style.BackColor = Color.LightGray
	Next

	dgvCPara.CurrentCell = Nothing	'選択表示状態をクリア
	'最左列の背景色
	For i = 0 To dgvCPara.RowCount - 1
		dgvCPara(0, i).Style.BackColor = Color.LightGray
	Next

	dgvPPara.CurrentCell = Nothing	'選択表示状態をクリア
	'最左列の背景色
	For i = 0 To dgvPPara.RowCount - 1
		dgvPPara(0, i).Style.BackColor = Color.LightGray
	Next

Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub


''' <summary>
''' ゼロ調正開始
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub btnZeroAdj_Click(sender As Object, e As EventArgs) Handles btnZeroAdj.Click

Try

	'If p_RTM(0).p_iBGWOrder = BGW_ORDER.NO_ORDER
		p_dtFile.ReadBufferClear()								'データバッファクリア

		p_RTM(0).p_iStaMonTime				= Double.Parse(p_cts.iStaFlucRange)
		p_RTM(0).p_iStaFlucRange			= Double.Parse(p_cts.iStaMonTime)
	
		p_RTM(0).p_iBGWOrder				= BGW_ORDER.ZERO_BUTTON
		If p_RTM(0).p_mltcp.IsZeroStartSW()
			p_ZeroJob = 9						'ゼロ調モニタで終了処理へ
			Me.Panel2.Visible	= False
			btnSOCheck.Enabled	= True
		    Call WriteLog("Calib Zeroボタン(停止) " & System.Reflection.MethodBase.GetCurrentMethod.Name, eLOG_LEVEL.iINFO)
		Else
			Me.Panel2.Location	= New Point(600, 300)
			Me.Panel2.Size		= New Size(710, 470)
			p_RTM(0).p_iStaMonTime = p_cts.iStaMonTime
			p_RTM(0).p_CaliblationState.iState	= eCalibState.ZERO_EXEC							'較正用ステータス->ゼロ調実行中
			btnSOCheck.Enabled	= False
		    Call WriteLog("Calib Zeroボタン(開始) " & System.Reflection.MethodBase.GetCurrentMethod.Name, eLOG_LEVEL.iINFO)
		End If
	'End If

Catch ex As Exception
    Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
    Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub

''' <summary>
''' 開始ボタン
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub btnExec_Click(sender As Object, e As EventArgs) Handles btnExec.Click
	Dim fMsg As frmMSG
	Dim sMsg As String = nothing
	Dim title = "Operation guide"
	Dim Cclb() As stPoint = Nothing
	Dim Xclb() As stPoint = Nothing
	Dim Pclb() As stPoint = Nothing
Try
	If (p_RTM(0).p_iConnectState = CONNECT_ST.CONNECTING) And (p_RTM(0).p_iBGWOrder = BGW_ORDER.NO_ORDER)

		Select case p_RTM(0).p_CaliblationState.iState
		Case RealTimeMeas.eCalibState.ZERO_COMP			'ゼロ調完了
			'測定者情報その他の格納
			With p_RTM(0)
				.p_sMeasPlace	= p_cts.strAutoPrintMesurePlaceValue      '測定場所
				.p_sMeasurer	= p_cts.strAutoPrintOperatorValue         '測定者
				'.p_sMeasComment = m_fm.txtComment.Text						'測定コメント
				'.p_sResinName	= m_fm.cboMeasName.Text						'樹脂名
				'.p_sLot			= m_fm.cboSmplName.Text						'LOT名
				'.p_sSensor		= m_fm.txtStirRod.Text						'撹拌棒
				'.p_sSmplAmount	= m_fm.txtAmount.Text						'サンプル量
				'.p_sCalcMethod	= m_fm.cboMethod.Text						'ゲルタイム計算方法
				'.p_sEquipName	= m_fm.cboEquipName.Text					'本体名
				.p_iZPosition	= .p_mltcp.ZeroPos_Z						'熱板ゼロ点位置(um)
				.p_dWeightCoef	= p_dNConv									'重量換算値
			End With

			'フィルタ動作設定をセット
			frmMain.SetFilterSetting()

			p_RTM(0).p_ClbMode="A"
			p_RTM(0).p_ClbX = If(iCalMode=0, p_FClbX, p_ZClbX)
			p_RTM(0).p_ClbC = If(iCalMode=0, p_FClbC, p_ZClbC)
			p_RTM(0).p_ClbZ = If(iCalMode=0, p_FClbP, p_ZClbP)



			'測定選択はあるか？
			Dim iMode As Int32
			Dim iFirstIdx As int32
			Dim iRet(2) As int32
			For iMode = 0 To 2
				iRet(iMode) = p_RTM(0).FirstIndexSerchInCalibArray(iMode, iFirstIdx)
			Next
			If iRet(0)=0 Or iRet(1)=0 Or iRet(2)=0
			Else
				sMsg = m_sStr(eMes.iCalibSSNoSelect)
				fMsg = New frmMSG(sMsg, {"OK"}, title, Me)
				fMsg.lblMsg.Font = New Font(BASE_FONT, 14)
				fMsg.Width = 400
				fMsg.StartPosition = FormStartPosition.CenterScreen
				fMsg.ShowDialog()
				Return
			End If


			'全終了予測日時を計算
			Dim i As Int32
			cClb = If(iCalMode=0, p_FClbC, p_ZClbC )
			xClb = If(iCalMode=0, p_FClbX, p_ZClbX )
			pClb = If(iCalMode=0, p_FClbP, p_ZClbP )
			m_iTotalSec= nudXYWait.Value
			For i = 0 To xClb.GetUpperBound(0)
				If XClb(i).bSelected
					'11周期取得
					m_iTotalSec += XClb(i).iSample*1.1 / MEAS_BLOCK_DATA_NUM + 3
				End If
			Next i
			For i = 0 To cClb.GetUpperBound(0)
				If Cclb(i).bSelected
					'11周期取得
					m_iTotalSec += Cclb(i).iSample*1.1 / MEAS_BLOCK_DATA_NUM + 3
				End If
			Next i
			For i = 0 To Pclb.GetUpperBound(0)
				If Pclb(i).bSelected
				'Z振幅変更に10秒消費、11周期取得
					m_iTotalSec += Pclb(i).iSample*1.1 / MEAS_BLOCK_DATA_NUM + 2 + 10
				End If
			Next i
			Dim dNow As Date = Now											'現在日時取得
			Dim dNeedDate As Datetime = New DateTime( m_iTotalSec * 1e+7 )	'所要日時
			Dim dZeroDate As DateTime = New DateTime( 0 )					'ゼロ日時
			Dim tsInterval As TimeSpan = dNeedDate - dZeroDate				'TimeSpan型変換
			m_dEndPrediction = dNow.Add(tsInterval)							'終了予測日時

			'sMsg = "The estimated end date and time will be " & vbCrLf & _
			'		m_dEndPrediction.ToString("yyyy/MM/dd HH:mm:ss") & "." & vbCrLf & _
			'		"Are you sure?"
			sMsg = m_sStr(eMes.iCalibSSEndTimeQ)
			sMsg = sMsg.Replace("#DATE#", m_dEndPrediction.ToString("yyyy/MM/dd HH:mm:ss"))

			fMsg = New frmMSG(sMsg, {m_sStr(eMes.iYES), m_sStr(eMes.iNO)}, title, Me)
			fMsg.lblMsg.Font = New Font(BASE_FONT, 14)
			fMsg.Width = 500
			fMsg.Height = 200
			fMsg.StartPosition = FormStartPosition.CenterScreen
			fMsg.ShowDialog()

			Select Case fMsg.p_iRes
			Case 1
				'キャリブレーション開始
				p_RTM(0).p_iBGWOrder = BGW_ORDER.CALIB_START_0
				'p_RTM(0).p_iBGWOrder = BGW_ORDER.CALIB_START_1			'Xモード廃止のためCモードから開始
				'btnExec.Enabled = False
				btnStop.Enabled = True
				btnXExec.Enabled = False
				btnXSel.Enabled=False
				btnXClear.Enabled = False
				btnXAllClear.Enabled = False
				btnXExec.Enabled = False
				btnXStop.Enabled = False

				btnCExec.Enabled = False
				btnCSel.Enabled=False
				btnCAllSelect.Enabled=False
				btnCClear.Enabled = False
				btnCAllClear.Enabled = False
				btnCExec.Enabled = False
				btnCStop.Enabled = False
				p_ZeroJob = 9						'ゼロ調モニタで終了処理へ
				Me.Panel2.Visible = False
				Call WriteLog("Calib 測定ボタン " & System.Reflection.MethodBase.GetCurrentMethod.Name, eLOG_LEVEL.iINFO)
			Case 2
			Case Else
			End Select
			fMsg.Dispose()
		Case Else
			p_RTM(0).p_iStaMonTime		= p_cts.iStaMonTime
			p_RTM(0).p_iStaFlucRange	= p_cts.iStaFlucRange
			p_RTM(0).p_CalibPreTemp		= nudPreTemp.Value						'較正中の温度設定
			p_RTM(0).p_iBGWOrder		= BGW_ORDER.CALIB_TEMP
		End Select

	End If

Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try

End Sub


''' <summary>
''' 停止ボタン
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub btnStop_Click(sender As Object, e As EventArgs) Handles btnStop.Click
	CalibStopBtn()
End Sub
'キャリブレーション測定停止処理
Private Sub CalibStopBtn()
Try
	Call WriteLog("Calib 停止ボタン " & System.Reflection.MethodBase.GetCurrentMethod.Name, eLOG_LEVEL.iINFO)
	'btnExec.Enabled = False
	'他の処理中以外は停止処理しない
	Select Case p_RTM(0).p_iBGWOrder
	Case BGW_ORDER.NO_ORDER
	Case BGW_ORDER.AUTO_PEELING To BGW_ORDER.AUTO_PEELING_6
		p_RTM(0).p_iBGWOrder = BGW_ORDER.AUTO_PEELING_STOP			'自動剥がし停止

	Case BGW_ORDER.MANU_PEELING To BGW_ORDER.MANU_PEELING_3
		p_RTM(0).p_iBGWOrder = BGW_ORDER.MANU_PEELING_STOP			'手動剥がし停止

	Case BGW_ORDER.ORG_BUTTON_REP To BGW_ORDER.ORG_BUTTON_REP_1
		'原点復帰中は処理なし

	Case BGW_ORDER.ZERO_BUTTON To BGW_ORDER.ZERO_BUTTON_2
		'ゼロ調中は処理なし

	Case Else
	    p_RTM(0).p_iBGWOrder = BGW_ORDER.CALIB_STOP					'較正処理停止

	End Select
	''p_RTM(0).p_mltcp.StartTempCont = 0								'温度制御停止

	'ゼロ調モニタで終了処理へ
	p_ZeroJob = 9
	Me.Panel2.Visible = False
	p_RTM(0).p_mltcp.StartSW = 0
	p_RTM(0).p_iZeroAdjOk = 0                                       'ゼロ調正完了クリア
	p_RTM(0).CalJob = eClbJob.IDLE									'ジョブ変数初期化


	p_RTM(0).p_CaliblationState.iState = RealTimeMeas.eCalibState.NOT_START


	Me.btnXExec.Enabled = False
	Me.btnXSel.Enabled=True
	Me.btnXClear.Enabled = True
	Me.btnXAllClear.Enabled = True
	Me.btnXStop.Enabled = True

	Me.btnCExec.Enabled = False
	Me.btnCSel.Enabled=True
	Me.btnCAllSelect.Enabled=True
	Me.btnCClear.Enabled = True
	Me.btnCAllClear.Enabled = True
	Me.btnCStop.Enabled = True

Catch ex As Exception
    Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub



''' <summary>
'''サンプル取得時間
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub nudSampleTime_ValueChanged(sender As Object, e As EventArgs) Handles nudSampleTime.ValueChanged
Dim Xclb() As stPoint = Nothing
Dim Cclb() As stPoint = Nothing
	
	Xclb = If(iCalMode=0,p_FClbX,p_ZClbX)
	Cclb = If(iCalMode=0,p_FClbC,p_ZClbC)

	If isNothing(Xclb) Or isNothing(Cclb) 
		Exit Sub
	End If
	m_iCalibDataTime = nudSampleTime.Value
	For i=0 To Xclb.getupperbound(0)
		Xclb(i).iSample = CalcSampleNumber(clSpec.eStrsSens.Line, Xclb(i))						'サンプル数計算
	Next i
	For i=0 To Cclb.getupperbound(0)
		Cclb(i).iSample = CalcSampleNumber(clSpec.eStrsSens.Circle, Cclb(i))					'サンプル数計算
	Next i
End Sub


''' <summary>
''' 平均化する後半データの割合
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub nudAcquisitionTime_ValueChanged(sender As Object, e As EventArgs) Handles nudAcquisitionTime.ValueChanged
Try
	 'p_RTM(0).p_ClbAveSizeRate = nudAcquisitionTime.value

Catch ex As Exception
End Try
End Sub

''' <summary>
''' 自動剥がし処理時の設定温度
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub NumericUpDown1_ValueChanged(sender As Object, e As EventArgs) Handles nudPeelingTemp.ValueChanged
Try
	 p_RTM(0).p_AutoPeelingTemp = nudPeelingTemp.value
Catch ex As Exception
End Try
End Sub


''' <summary>
''' 原点復帰ボタン
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub btnOrigin_Click(sender As Object, e As EventArgs) Handles btnOrigin.Click

Try
    Call WriteLog("Calib 原点復帰ボタンクリック " & System.Reflection.MethodBase.GetCurrentMethod.Name, eLOG_LEVEL.iINFO)

	If p_RTM(0).p_iBGWOrder = BGW_ORDER.NO_ORDER
		p_RTM(0).p_iBGWOrder = BGW_ORDER.ORG_BUTTON_REP
	End If

Catch ex As Exception
    Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
    Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub


''' <summary>
''' 温度制御ボタン
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub btnTemperture_Click(sender As Object, e As EventArgs) Handles btnTemperature.Click
Try
	If Not p_bError
		Call WriteLog("Calib Tempボタン クリック " & System.Reflection.MethodBase.GetCurrentMethod.Name, eLOG_LEVEL.iINFO)
		If p_RTM(0).p_iBGWOrder <> BGW_ORDER.NO_ORDER
			p_RTM(0).p_iBGWOrderNext	= p_RTM(0).p_iBGWOrder
			p_RTM(0).p_iBGWOrder		= BGW_ORDER.TEMP_BUTTON_REP
		Else
			p_RTM(0).p_CalibPreTemp = nudPreTemp.Value
			p_RTM(0).p_iBGWOrder = BGW_ORDER.TEMP_BUTTON_REP
		End if

	End if
Catch ex As Exception
    Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
    Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub


''' <summary>
''' 較正中の温度設定
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub nudPreTemp_ValueChanged(sender As Object, e As EventArgs) Handles nudPreTemp.ValueChanged
	If Not bNew 
		p_RTM(0).p_mltcp.TempPreset(1)	= double.parse(nudPreTemp.Value)
		p_RTM(0).p_mltcp.Temp2Preset(1) = double.parse(nudPreTemp.Value)
		p_RTM(0).p_mltcp.Temp3Preset(1) = p_clSpec.SGTempGet(p_RTM(0).p_mltcp.TempPreset(1))
		p_RTM(0).p_CalibPreTemp		= double.parse(nudPreTemp.Value)
	End If
End Sub

''' <summary>
''' 較正中 入力GAP
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub nudInputGap_ValueChanged(sender As Object, e As EventArgs) Handles nudInputGap.ValueChanged
	If Not bNew 
		p_RTM(0).p_mltcp.InpGap(1) =  double.parse(nudInputGap.Value)
	End If
End Sub

''' <summary>
''' 較正中 GAP
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub nudMeasureGap_ValueChanged(sender As Object, e As EventArgs) Handles nudMeasureGap.ValueChanged
	If Not bNew 
		p_RTM(0).p_mltcp.GapPreset(1) =  double.parse(nudMeasureGap.Value)
	End If
End Sub

''' <summary>
''' XY動作待ち時間
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub nudXYWait_ValueChanged(sender As Object, e As EventArgs) Handles nudXYWait.ValueChanged
	If Not bNew 
		p_RTM(0).p_mltcp.MesPreWait(1) =  double.parse(nudXYWait.Value)
	End If
End Sub

''' <summary>
''' 熱板上昇待ち時間
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub nudRisingHPWait_ValueChanged(sender As Object, e As EventArgs) Handles nudRisingHPWait.ValueChanged
	If Not bNew 
		p_RTM(0).p_mltcp.MeasWaitTime(1) =  double.parse(nudRisingHPWait.Value)
	End If
End Sub


Private Sub frmCalib_Closed(sender As Object, e As EventArgs) Handles Me.Closed

	frmMain.Show()
End Sub

''' <summary>
''' ファンクションキー対応
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub frmCalibFull_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
	Select Case e.KeyCode
	Case 112		'F1キー
		btnTemperature.PerformClick()			'温度
	Case 113		'F2キー
		btnZeroAdj.PerformClick()				'ゼロ調
	Case 114		'F3キー
		If p_RTM(0).p_iBGWOrder = BGW_ORDER.NO_ORDER
			btnExec.PerformClick()				'測定
		Else
			btnStop.PerformClick()				'停止
		End If
	Case 115		'F4キー
		'btnReset_Click( sender, e )			'リセット
	Case 116		'F5キー
		btnOrigin.PerformClick()				'原点復帰
	End Select

	If (e.Modifiers And Keys.Control) = Keys.Control Then
		If e.KeyCode = Keys.K Then								'CTRL＋K デバッグ画面
			frmDebug.Show()
		End If
	End If

End Sub

''' <summary>
''' タブコントロールインデックス変化
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub tcPara_SelectedIndexChanged(sender As Object, e As EventArgs) Handles tcPara.SelectedIndexChanged
	UpdateDisp()		'更新日時の表示
End Sub
End Class
