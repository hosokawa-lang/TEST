Imports Microsoft.VisualBasic.FileIO
Imports Microsoft.VisualBasic.ApplicationServices
Imports System.Windows.Forms.DataVisualization.Charting
Imports System.Text
Imports System.IO
Imports SZKTool.frmOptions
Imports System.Security.Permissions
Imports System.ComponentModel
''' <summary>
''' 粘度測定画面
''' </summary>
Public Class frmViscosity

'粘度測定モード（速度依存、時間依存）
Public p_iMeasMode As int16 = -1
Public ReadOnly Property FlcvMeasMode() As Int16
	Get
		FlcvMeasMode = p_iMeasMode
	End Get
End Property

'粘度測定モード（速度依存、時間依存）名称
Public lstViscoMeasureName As New List(Of String)(New String() {"Speed", "Time","Temp."})
Public Enum eViscoMeaure
	SpeedDep = 0
	TimeDep
	TempDep
End Enum

'粘弾測定状態
Public Enum eViscoState
	NOT_START = -1				'未開始
	ZERO_EXEC = 1				'ZERO調実行中
	ZERO_COMP					'ZERO調完了
	RESERV						'予備
	PREPAREING					'準備中
	COND_CONF					'条件確定
	STAB_WAIT					'本体安定待ち
	DATA_TRANS					'測定中(データ取得)
	AUTO_PEELING				'自動剥がし
	AUTO_PEELING_1				'自動剥がし
	FINISHING					'終了中
End Enum

Const cSampleSecond				As Double = MEAS_TIME_RESOLUTION / MEAS_TIME_BLOCK		'サンプル時間(S)

Private	ds				        As New DataSet			    'データセット
Private	dtSpeedDep		        As New DataTable		    '速度依存 データテーブル			Table(0)
Private	dtTimeDep		        As New DataTable		    '時間依存 データテーブル			Table(1)
Private	dtTempDep		        As New DataTable		    '温度依存 データテーブル			Table(2)
Private dtRow	                As DataRow		            'ワーク用データテーブル行

Private m_dFlcvXAxisMax	As Double							'FLCV 横軸MAX チャート表示用
Private m_dFlcvXAxisMin As Double							'FLCV 横軸MIN チャート表示用

Private m_iTotalSec				As int64 = 0				'予想終了日時を表示するための全終了に要する時間(秒)
Private m_dEndPrediction		As DateTime					'終了予測日時 結果

Private iError					As Int16 = 0

Private iY1Sel					As Int16					'Y1軸選択 eClmからセット
Private iY2Sel					As Int16					'Y2軸選択 eClmからセット
Private iXSel					As Int16					'X軸選択 eClmからセット

Private iY1PlotColor			As Color = Color.Blue		'Y1軸系統プロットカラー
Private iY2PlotColor			As Color = Color.Red		'Y2軸系統プロットカラー

Private eNum eClm
	iShearVelocity = 0
	iShearStress
	iViscosity
	iPosPhase
	iSampStrain
	iShearMod
	iStorageMod
	iElasticMod
	iLossTan
	iHPTemp
	iStress
	iTime
End eNum
'データテーブルアイテム名 追加したら言語ファイルも変更すること
Private sDtItemStr()	As String = {"ShearRate","ShearStress","Viscosity","Phase","SampStrain","ShearMod","StorageMod","ElasticMod","Tangent","HPTemp","Stress"}
Private sDtItemUnit()	As String = {"[1/Sec]","[Pa]","[PaS]","[degree]","","[Pa]","[Pa]","[Pa]","","[Deg C]","[uN]"}

Private sDispItemName()	As String
Private sDispItemUnit()	As String
'多言語メッセージ格納
Private m_sStr(eMes.iMAX - 1)		 As String   
Enum eMes
	iZeroStartOK = 0	'ゼロ調整開始準備完了です。
	iZeroPerform		'ゼロ調整中です...
	iZeroCOMP			'ゼロ調整完了しました。測定可能です。
	iWaitStab			'状態安定待ちです... 
	iSpeedChange		'測定中... ( #CNT# / #SUM# ) 振幅[mm] : 
	iTempChange			'設定温度(℃) -> #TEMP#
	iMeasuring			'測定中... ( #CNT# / #SUM# ) 
	iMeasuringSpeed		'振幅[mm] : 
	iMeasuringTemp		'設定温度[℃] : 
	iAccNum				'蓄積サンプル数:
	iMeasEND			'測定終了しました。
	iDispItem			'ShearRate,ShearStress,Viscosity,Phase,SampStrain,ShearMod,StorageMod,ElasticMod,Tangent,HPTemp,Stress
	iDispUnit			'[1/Sec],[Pa],[PaS],[degree],,[Pa],[Pa],[Pa],,[Deg C],[uN]
	iWaveMonTitle		'波形モニタ
	iWaveMonSrs1		'位置
	iWaveMonSrs2		'応力
	iWaveMonX			'時間
	iTimeDepX			'時間[Sec]
	iMES_ENDQ			'測定終了予定時刻は\r\n#YMD#.\r\n になります。よろしいですか?
	iMES_YES			'はい
	iMES_NO				'いいえ
	iMES_TITLE			'操作ガイド
	iMES_TITLE1			'測定開始確認
	iMES_FCALIBINZERO	'フル応力キャリブレーション設定内に０値が見つかったので測定開始できません。フル応力キャリブレーション設定を確認してください。
	INI_HIGH					'初期化...高せん断速度
	INI_RECOVERY				'初期化...回復
	LOW_SHEAR1					'低せん断速度(初期状態)
	HIGH_SHEAR					'高せん断速度(構造破壊)
	LOW_SHEAR2					'低せん断速度(回復過程)
	ELAPSED_TIME				'経過時間:
	ELAPSED_TIME_UNIT			'(秒)
	iMAX
End Enum

''' <summary>
''' コンストラクタ
''' </summary>
Sub New
	' この呼び出しはデザイナーで必要です。
	InitializeComponent()
	' InitializeComponent() 呼び出しの後で初期化を追加します。
End Sub
''' <summary>
''' フォームロード
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub frmViscosity_Load(sender As Object, e As EventArgs) Handles MyBase.Load
Dim i As int32
Try
	p_FormCalibVisible = True

	'言語切替
	Call SetFormLanguage(Me, p_iLanguage,,m_sStr)
	sDispItemName = m_sStr(eMes.iDispItem).Split(",")
	sDispItemUnit = m_sStr(eMes.iDispUnit).Split(",")
	Me.WindowState = FormWindowState.Maximized			'ウィンドウ最大化
	Me.Text &= " [" & My.Application.Info.Version.ToString & "]"

	p_dtFile.ReadBufferClear()

	'リアルタイム値モニター初期設定
	RealTimeMonitorValDispSetting(dgvCurrVal, 1)

	'データテーブル初期化
	If ds.Tables.Count <= 0
		'速度依存データセットのテーブル
		dtSpeedDep.TableName = "SpeedDepTable"
		For i = 0 To sDtItemStr.GetUpperBound(0)
			dtSpeedDep.Columns.Add(sDtItemStr(i),	Type.GetType("System.Double"))
		Next i
		ds.Tables.Add(dtSpeedDep)

		'時間依存データセットのテーブル
		dtTimeDep.TableName = "TimeDepTable"
		For i = 0 To sDtItemStr.GetUpperBound(0)
			dtTimeDep.Columns.Add(sDtItemStr(i),	Type.GetType("System.Double"))
		Next i
		dtTimeDep.Columns.Add("Time",			Type.GetType("System.Double"))
		ds.Tables.Add(dtTimeDep)

		'温度依存データセットのテーブル
		dtTempDep.TableName = "TempDepTable"
		For i = 0 To sDtItemStr.GetUpperBound(0)
			dtTempDep.Columns.Add(sDtItemStr(i),	Type.GetType("System.Double"))
		Next i
		ds.Tables.Add(dtTempDep)
	Else
		ds.Tables("SpeedDepTable").Clear
		ds.Tables("TimeDepTable").Clear
		ds.Tables("TempDepTable").Clear
	End if
	'軸選択コンボボックス初期化
	cbxY1Select.Items.AddRange(sDispItemName)
	cbxY1Select.SelectedIndex = eClm.iViscosity		'Y1軸 粘度
	cbxY1Select.FlatStyle = FlatStyle.Flat

	cbxY2Select.Items.AddRange(sDispItemName)
	cbxY2Select.SelectedIndex = eClm.iShearStress	'Y2軸 せん断応力
	cbxY2Select.FlatStyle = FlatStyle.Flat

	cbxXSelect.Items.AddRange(sDispItemName)
	cbxXSelect.SelectedIndex = eClm.iShearVelocity	'X軸 せん断速度
	cbxXSelect.FlatStyle = FlatStyle.Flat

	'軸初期選択
	iY1Sel	= cbxY1Select.SelectedIndex
	iY2Sel	= cbxY2Select.SelectedIndex
	iXSel	= cbxXSelect.SelectedIndex

	lblY1Select.ForeColor = iY1PlotColor
	lblY2Select.ForeColor = iY2PlotColor


	'測定モード
	btnSpeed.PerformClick()

	'温度設定フィールド範囲設定
	nudPreTemp.Maximum			= p_cts.TempCnt_HUpperLimit
	nudPreTemp.Minimum			= HEATER_LWLMT_NUD
	nudTempDepTempStart.Maximum	= p_cts.TempCnt_HUpperLimit
	nudTempDepTempStart.Minimum	= HEATER_LWLMT_NUD
	nudTempDepTempEnd.Maximum	= p_cts.TempCnt_HUpperLimit
	nudTempDepTempEnd.Minimum	= HEATER_LWLMT_NUD

	'設定初期値
	nudPreTemp.Value			= 20.0
	nudInputGap.Value			= 20.0
	nudMeasureGap.Value			= 0.500
	nudXYWait.Value				= 10.0
	nudRisingHPWait.Value		= 8.0
	nudProbedDiameter.Value		= 15.0

	nudSpeedDepPeriod.Value		= 1.000
	nudSpeedDepAmpStart.Value	= 0.000
	nudSpeedDepAmpEnd.Value		= 5.000
	nudSpeedDepAmpStep.Value	= 0.100
	nudSpeedDepSampleTime.Value			= 10.0
	nudSpeedDepAcquisitionTime.Value	= 30.0

#if 0
	nudTimeDepPeriod.Value		= 1.0
	nudTimeDepAmp.Value			= 1.0
	nudTimeDepCycle.Value		= 5
	nudTimeDepOpeTime.Value		= 1
	nudTimeDepRstTime.Value		= 1
#Else
	nudPreShearAmp.Value		= 5
	nudPreShearTime.Value		= 30
	nudRecoveryAmp.Value		= 0.0
	nudRecoveryTime.Value		= 60
	nudLow1Amp.Value			= 0.5
	nudLow1Time.Value			= 30
	nudHighAmp.Value			= 5
	nudHighTime.Value			= 30
	nudLow2Amp.Value			= 0.5
	nudLow2Time.Value			= 60
#End If


	'グラフ初期表示
	Dim uVp As DataFile.ViscosityCalcParameter
	VpClear(uVp)
	SpeedDepTableAddRow(uVp)

	m_dFlcvXAxisMax = 100
	m_dFlcvXAxisMin = 0


	'Strage-Elastモニタスケール軸設定
	dStgElaGXInterval			= 500
	dStgElaGYInterval			= 500
	dStgElaGXMax				= 1000
	dStgElaGXMin				= -1000
	dStgElaGYMax				= 1000
	dStgElaGYMin				= -1000
	StrgElastGAxisParaSet(0)

	ChartDisp1(False, ChartA, ds)													'フローカーブ メイングラフ
	ChartDisp2(False, ChartB, ds)													'フローカーブ モニタグラフ1
	ChartDisp3(False, ChartC, ds)													'フローカーブ モニタグラフ1

	gbxStrgElastGScaleSetting.Visible = False										'Strage-Elastモニタスケール設定非表示

	lblMsg.ForeColor = Color.Blue

	' 較正中 温度設定
	p_RTM(0).p_mltcp.TempPreset(1) = double.parse(nudPreTemp.Value)
	' 較正中 入力GAP
	p_RTM(0).p_mltcp.InpGap(1)		=  double.parse(nudInputGap.Value)
	' 較正中 GAP
	p_RTM(0).p_mltcp.GapPreset(1)	=  double.parse(nudMeasureGap.Value)
	' XY動作待ち時間
	p_RTM(0).p_mltcp.MesPreWait(1)	=  double.parse(nudXYWait.Value)
	' 熱板上昇待ち時間
	p_RTM(0).p_mltcp.MeasWaitTime(1) =  double.parse(nudRisingHPWait.Value)
	
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

	'時間依存設定を送信
	p_RTM(0).p_FlcvLow1Time			= double.parse(nudLow1Time.Value)
	p_RTM(0).p_FlcvLow1Amplitude	= double.parse(nudLow1Amp.Value)
	p_RTM(0).p_FlcvLow2Time			= double.parse(nudLow2Time.Value)
	p_RTM(0).p_FlcvLow2Amplitude	= double.parse(nudLow2Amp.Value)
	p_RTM(0).p_FlcvHighTime			= double.parse(nudHighTime.Value)
	p_RTM(0).p_FlcvHighAmplitude	= double.parse(nudHighAmp.Value)
	p_RTM(0).p_FlcvPreshearTime		= double.parse(nudPreShearTime.Value)
	p_RTM(0).p_FlcvPreshearAmplitude= double.parse(nudPreShearAmp.Value)


	Me.panel2.Visible = False

	Dim frmZ As New frmCalibFull(1)
	frmZ.sbFullStressCondRead(1, p_ZClbX, p_ZClbC, p_ZClbP)	'ファイル読込み構造体配列へセット
	''frmZ.sbFullStressRALRead(0)	'ファイル読込み構造体配列へセット
	''frmZ.sbFullStressRALRead(1)	'ファイル読込み構造体配列へセット
	''frmZ.sbFullStressRALRead(2)	'ファイル読込み構造体配列へセット
	''frmZ.sbFullStressRALRead(3)	'ファイル読込み構造体配列へセット
	''frmZ.sbFullStressRALRead(4)	'ファイル読込み構造体配列へセット
	''frmZ.sbFullStressRALRead(5)	'ファイル読込み構造体配列へセット
	frmZ.Dispose

	Dim frmC As New frmCalibFull(0)
	frmC.sbFullStressCondRead(0, p_FClbX, p_FClbC, p_ZClbP)	'ファイル読込み構造体配列へセット
	''frmC.sbFullStressRALRead(0)	'ファイル読込み構造体配列へセット
	''frmC.sbFullStressRALRead(1)	'ファイル読込み構造体配列へセット
	''frmC.sbFullStressRALRead(2)	'ファイル読込み構造体配列へセット
	''frmC.sbFullStressRALRead(3)	'ファイル読込み構造体配列へセット
	''frmC.sbFullStressRALRead(4)	'ファイル読込み構造体配列へセット
	''frmC.sbFullStressRALRead(5)	'ファイル読込み構造体配列へセット
	frmC.Dispose

	'C/Xモード


	'常時タイマースタート
	timMain.Interval = 500
	timMain.Enabled = True
Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub
Private Sub VpClear(ByRef uVp As DataFile.ViscosityCalcParameter)
	uVP.dShearVelocity = -1
	uVP.dViscosity = -1
	uVP.dShearStress = -1
	uVP.dXPosPhase = -1
	uVP.dSplStrain = -1
	uVP.dShearMod = -1
	uVP.dXStrgMod = -1
	uVP.dXElasMod = -1
	uVP.dXLossTan = -1
	uVP.dHPTemp = -1
	uVP.dMeasStress = -1
End Sub
''' <summary>
''' 速度依存測定テーブル全消去
''' </summary>
Public Sub SpeedDepTableAllRowClear()
	ds.Tables("SpeedDepTable").Clear
End Sub
''' <summary>
''' 温度依存測定テーブル全消去
''' </summary>
Public Sub TempDepTableAllRowClear()
	ds.Tables("TempDepTable").Clear
End Sub
''' <summary>
''' フローカーブ速度依存 テーブル行追加
''' </summary>
''' <param name="uVp"></param>
Public Sub SpeedDepTableAddRow(ByRef uVp As DataFile.ViscosityCalcParameter)
Try
	dtRow = ds.Tables("SpeedDepTable").NewRow
	dtRow(eClm.iShearVelocity)	= uVp.dShearVelocity
	dtRow(eClm.iShearStress)	= uVp.dShearStress
	dtRow(eClm.iViscosity)		= uVp.dViscosity
	dtRow(eClm.iPosPhase)		= If(Double.IsNaN(uVp.dXPosPhase), 0, uVp.dXPosPhase)  * RadToDegRate
	dtRow(eClm.iSampStrain)		= uVp.dSplStrain
	dtRow(eClm.iShearMod)		= uVp.dShearMod
	dtRow(eClm.iStorageMod)		= uVp.dXStrgMod
	dtRow(eClm.iElasticMod)		= uVp.dXElasMod
	dtRow(eClm.iLossTan)		= If(Double.IsNaN(uVp.dXLossTan), 0, uVp.dXLossTan)
	dtRow(eClm.iHPTemp)			= uVp.dHPTemp
	dtRow(eClm.iStress)			= uVp.dMeasStress * 10^6
	ds.Tables("SpeedDepTable").Rows.Add(dtRow)
Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub
''' <summary>
''' フローカーブ時間依存 テーブル行追加
''' </summary>
Public Sub TimeDepTableAddRow(ByVal Thininng As Int32)
	Dim i As int32
	Dim sTbl As String = "TimeDepTable"
Try

	If IsNothing(p_DtFile.dViscosity)
	'初期表示のため
		dtRow = ds.Tables(sTbl).NewRow
		dtRow(eClm.iShearVelocity)	= -1
		dtRow(eClm.iShearStress)	= -1
		dtRow(eClm.iViscosity)		= -1
		dtRow(eClm.iPosPhase)		= -1
		dtRow(eClm.iSampStrain)		= -1
		dtRow(eClm.iShearMod)		= -1
		dtRow(eClm.iStorageMod)		= -1
		dtRow(eClm.iElasticMod)		= -1
		dtRow(eClm.iLossTan)		= -1
		dtRow(eClm.iHPTemp)			= -1
		dtRow(eClm.iStress)			= -1
		dtRow(eClm.iTime)			= -1
		ds.Tables(sTbl).Rows.Add(dtRow)
		Return
	End If
	ds.Tables(sTbl).Clear
	For i = 0 To p_DtFile.dViscosity.GetUpperBound(0)-MEAS_BLOCK_DATA_NUM Step Thininng
		dtRow = ds.Tables(sTbl).NewRow
		dtRow(eClm.iShearVelocity)	= If( IsNothing(p_DtFile.dShearVelo),0,p_DtFile.dShearVelo(i))
		dtRow(eClm.iShearStress)	= p_DtFile.dShearStress(i)
		dtRow(eClm.iViscosity)		= p_DtFile.dViscosity(i)
		dtRow(eClm.iPosPhase)		= p_DtFile.dXPosPhase(i) * RadToDegRate
		dtRow(eClm.iSampStrain)		= p_DtFile.dSplStrain(i)
		dtRow(eClm.iShearMod)		= p_DtFile.dShearMod(i)
		dtRow(eClm.iStorageMod)		= p_DtFile.dXStrgMod(i)
		dtRow(eClm.iElasticMod)		= p_DtFile.dXElasMod(i)
		dtRow(eClm.iLossTan)		= p_DtFile.dXLossTan(i)
		dtRow(eClm.iHPTemp)			= p_DtFile.iTempBuf(i) / 10.0
		dtRow(eClm.iStress)			= p_DtFile.iHosei(i)
		dtRow(eClm.iTime)			= i * cSampleSecond
		'dtRow(0) = i * cSampleSecond
		'dtRow(1) = p_DtFile.dViscosity(i)
		ds.Tables(sTbl).Rows.Add(dtRow)
	Next
Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub
''' <summary>
''' フローカーブ温度依存 テーブル行追加
''' </summary>
''' <param name="uVp"></param>
Public Sub TempDepTableAddRow(ByRef uVp As DataFile.ViscosityCalcParameter)
	dtRow = ds.Tables("TempDepTable").NewRow
	dtRow(eClm.iShearVelocity)	= uVp.dShearVelocity
	dtRow(eClm.iShearStress)	= uVp.dShearStress
	dtRow(eClm.iViscosity)		= uVp.dViscosity
	dtRow(eClm.iPosPhase)		= uVp.dXPosPhase
	dtRow(eClm.iSampStrain)		= uVp.dSplStrain
	dtRow(eClm.iShearMod)		= uVp.dShearMod
	dtRow(eClm.iStorageMod)		= uVp.dXStrgMod
	dtRow(eClm.iElasticMod)		= uVp.dXElasMod
	dtRow(eClm.iLossTan)		= uVp.dXLossTan
	dtRow(eClm.iHPTemp)			= uVp.dHPTemp
	dtRow(eClm.iStress)			= uVp.dMeasStress * 10^6
	'dtRow(0) = uVp.dTemp
	'dtRow(1) = uVp.dViscosity
	ds.Tables("TempDepTable").Rows.Add(dtRow)
End Sub
Private Sub ConnectStateDisp
	Select Case p_RTM(0).p_iConnectState
	Case CONNECT_ST.OFFLINE         '未接続。
		lblConnectState.BackColor = Color.LightGray
		lblConnectState.Text = "OFFLINE"
		lblConnectState.ForeColor = Color.White
		pctConStts.BackColor = lblConnectState.BackColor

	Case CONNECT_ST.ONLINE, CONNECT_ST.CONNECTING          '接続中。
		Select Case p_RTM(0).p_mltcp.IsLinkError()
		Case 0  'Link OK
			lblConnectState.Text = "ONLINE"
			lblConnectState.BackColor = Color.LightGreen
		Case Else 'Link Error
			lblConnectState.Text = "NO LINK"
			lblConnectState.BackColor = Color.Red
		End Select
		lblConnectState.ForeColor = Color.Black
		pctConStts.BackColor = lblConnectState.BackColor

	Case CONNECT_ST.CANNOTCONNECT   '接続不可。
		lblConnectState.BackColor = Color.PeachPuff
		p_RTM(0).p_iConnectState = CONNECT_ST.OFFLINE
		lblConnectState.Text = "OFFLINE"
		lblConnectState.ForeColor = Color.White
		pctConStts.BackColor = lblConnectState.BackColor
	Case Else
		lblConnectState.BackColor = Color.LightGray
		lblConnectState.Text = "OFFLINE"
		lblConnectState.ForeColor = Color.White
		pctConStts.BackColor = lblConnectState.BackColor
	End Select
End Sub
'
''' <summary>
''' メインタイマーインターバル処理
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
'debug.Print("timMain.Enabled")
End Sub
Private Sub btnENableCororUpdate(ByRef btn As Button)
	If btn.Enabled
		btn.BackColor = Color.LightCyan
	Else
		btn.BackColor = Control.DefaultBackColor
	End If
End Sub
'タイマーインターバルで使用する記憶変数
Dim bColor		As Boolean =False					'ボタン色
Dim iViscoJob	As Int16 = -1						'制御状態変数
Dim iCount		As Int32 = 0						'グラフ更新タイミング用カウンタ
Dim bSetA		As Boolean	= False					'グラフ更新用高速化
Dim bSetB		As Boolean	= False					'グラフ更新用高速化
Dim bSetC		As Boolean	= False					'グラフ更新用高速化
Dim iRealIntervalTime	As Int64 = 1000				'リアルタイム更新
Dim iScanTimeAry()		As Int64 = Nothing
Dim PreMesBlockCnt		As Int32 = 0

Private Sub	TimerIntervalProc()
	Dim dgv As	DataGridView = Nothing
Try
	If p_RTM(0).p_iBGWWork = BGW_WORK.AUTO_CONNECT
	'接続が切れた
		btnClose.Enabled = True
		iError = 1
	Else
		iError = 0
	End If

	'ゼロ調制御表示
	Select Case p_RTM(0).iViscoState
	Case eViscoState.NOT_START
		If p_RTM(0).p_iBGWOrder = BGW_ORDER.NO_ORDER
			btnZeroAdj.Enabled	= True
			btnZeroAdj.BackColor = Color.LightCyan
		Else
			btnZeroAdj.Enabled	= False
			btnZeroAdj.BackColor = Control.DefaultBackColor
		End If
		If p_cts.SGStressStab = 0 Or Math.abs(frmMain.dZStability) < p_cts.SGStressStab
		'Z軸応力安定度確認
		
			btnZeroAdj.Text = "Zero"
			'lblMsg.Text = "Zero Adjustment Ready"
			'lblMsg.Text = m_sStr(eMes.iZeroStartOK)
			lblMsg.Text = ""
			If p_RTM(0).p_mltcp.IsZeroStartSW
				p_RTM(0).iViscoState = eViscoState.ZERO_EXEC
				p_ZeroJob = -1		'ゼロモニタJob変数を初期化
			End If
		Else
			lblMsg.Text = ""
		End If

	Case 0
		If Not p_RTM(0).p_mltcp.IsZeroStartSW
			p_RTM(0).iViscoState = eViscoState.NOT_START
		End If

	Case eViscoState.ZERO_EXEC
		'lblMsg.Text = "Performing zero adjustment"
		lblMsg.Text = m_sStr(eMes.iZeroPerform)
		sbObjBackClrBlink(btnZeroAdj, Color.LightCyan, Color.DarkOrange, bColor)
		frmMain.ZeroMonitorJob(Me.Chart2, Me.Panel2)				'ゼロ調モニタ
		If p_RTM(0).p_mltcp.IsZeroStartSW
		End If
		if p_RTM(0).p_iZeroAdjOk = 1
			p_RTM(0).iViscoState = eViscoState.ZERO_COMP
		ElseIf Not p_RTM(0).p_mltcp.IsZeroStartSW
			p_RTM(0).iViscoState = eViscoState.NOT_START
		End If

	Case eViscoState.ZERO_COMP
		btnZeroAdj.BackColor = Color.DarkOrange
		btnZeroAdj.Text = "Zero READY"
		frmMain.ZeroMonitorJob(Me.Chart2, Me.Panel2)				'ゼロ調モニタ
		if p_RTM(0).p_iZeroAdjOk = 1
		'ゼロ調完了時、開始ボタン有効
			btnExec.Enabled = True
			'lblMsg.Text = "Zero adjustment is complete. Can be measured"
			lblMsg.Text = m_sStr(eMes.iZeroCOMP)
		End If
		If Not p_RTM(0).p_mltcp.IsZeroStartSW
			p_RTM(0).iViscoState = eViscoState.NOT_START
		End If

	'速度/温度依存 -------------------------------------
	Case eViscoState.STAB_WAIT
		btnExec.Enabled = False
		p_RTM(0).p_iZeroAdjOk = -1
		'Dim sText As String = "Waiting for condition stability... "
		Dim sText As String = m_sStr(eMes.iWaitStab) & Space(2)
		Select Case p_RTM(0).p_FlcvMeasMode
		Case eViscoMeaure.SpeedDep
			'lblMsg.Text = "Measuring... "
			'lblMsg.Text = lblMsg.Text & "( " &    (p_RTM(0).p_iFlcvMeasIdx+1).ToString & " / " & p_RTM(0).p_FlcvChgVal.Length.ToString & " ) "
			'lblMsg.Text = lblMsg.Text & "Amplitude(mm) : " & sText
			Dim s As String = m_sStr(eMes.iSpeedChange)
			s = s.Replace("#CNT#", (p_RTM(0).p_iFlcvMeasIdx+1).ToString )
			s = s.Replace("#SUM#", p_RTM(0).p_FlcvChgVal.Length.ToString )
			lblMsg.Text = s & Space(2) & sText
		Case eViscoMeaure.TempDep
			'lblMsg.Text = sText & " Temperature(deg C) -> " & p_RTM(0).p_FlcvChgVal(p_RTM(0).p_iFlcvMeasIdx).ToString("F1")
			Dim s As String = m_sStr(eMes.iTempChange)
			s = s.Replace("#TEMP#", p_RTM(0).p_FlcvChgVal(p_RTM(0).p_iFlcvMeasIdx).ToString("F1"))
			lblMsg.Text = sText & s
		End Select

	'速度/温度依存 -------------------------------------
	Case eViscoState.DATA_TRANS
		'lblMsg.Text = "Measuring... "
		'lblMsg.Text = lblMsg.Text & "( " &    (p_RTM(0).p_iFlcvMeasIdx+1).ToString & " / " & p_RTM(0).p_FlcvChgVal.Length.ToString & " ) "
		btnExec.Enabled = False
		Dim s As String = m_sStr(eMes.iMeasuring)
		s = s.Replace("#CNT#", (p_RTM(0).p_iFlcvMeasIdx+1).ToString )
		s = s.Replace("#SUM#", p_RTM(0).p_FlcvChgVal.Length.ToString )
		lblMsg.Text = s & Space(2)

		Select Case p_RTM(0).p_FlcvMeasMode
		Case eViscoMeaure.SpeedDep
			'lblMsg.Text = lblMsg.Text & "Amplitude(mm) : " & p_RTM(0).p_FlcvChgVal(p_RTM(0).p_iFlcvMeasIdx).ToString("F3")
			lblMsg.Text = lblMsg.Text & m_sStr(eMes.iMeasuringSpeed) & Space (1) & p_RTM(0).p_FlcvChgVal(p_RTM(0).p_iFlcvMeasIdx).ToString("F3")
		Case eViscoMeaure.TempDep
			'lblMsg.Text = lblMsg.Text & "Temperature(deg C) : " & p_RTM(0).p_FlcvChgVal(p_RTM(0).p_iFlcvMeasIdx).ToString("F1")
			lblMsg.Text = lblMsg.Text & m_sStr(eMes.iMeasuringTemp) & Space (1) & p_RTM(0).p_FlcvChgVal(p_RTM(0).p_iFlcvMeasIdx).ToString("F1")
		End Select
		'If p_dtFile.MesBlockCnt <> PreMesBlockCnt                                   '受信ブロックカウンタ更新
			ChartDisp2(bSetB, ChartB, ds)											'StrageMod-ElsaticMod モニタグラフ
			ChartDisp3(bSetC, ChartC, ds)											'応力/位置 位相 モニタグラフ
			PreMesBlockCnt = p_dtFile.MesBlockCnt
		'End If

	Case eViscoState.FINISHING
		btnExec.Enabled = True
		p_RTM(0).iViscoState = eViscoState.NOT_START

	Case Else
	End Select



	'速度/温度依存 制御中処理
	Select Case p_RTM(0).P_ViscoCont
	Case RealTimeMeas.enflcvContRol.VISC_TBL_CLEAR
		btnExec.Enabled = False
		Select Case p_RTM(0).p_FlcvMeasMode
		Case eViscoMeaure.SpeedDep
			SpeedDepTableAllRowClear()
		Case eViscoMeaure.TempDep
			TempDepTableAllRowClear()
		End Select
		PreMesBlockCnt = -1
		p_RTM(0).P_ViscoCont = RealTimeMeas.enflcvContRol.NO_WORK

	Case RealTimeMeas.enflcvContRol.VISC_TBL_ADD
		btnExec.Enabled = False
		Select Case p_RTM(0).p_FlcvMeasMode
		Case eViscoMeaure.SpeedDep
			SpeedDepTableAddRow(p_RTM(0).p_uVp)											'速度依存テーブル行追加
			ChartDisp1(bSetA, ChartA, ds)

		Case eViscoMeaure.TempDep
			TempDepTableAddRow(p_RTM(0).p_uVp)											'温度依存テーブル行追加
			ChartDisp1(bSetA, ChartA, ds)
		End Select

		bSetB = False
		bSetC = False

		p_RTM(0).P_ViscoCont = RealTimeMeas.enflcvContRol.NO_WORK

	Case RealTimeMeas.enflcvContRol.NO_WORK
		btnExec.Enabled = True

	End Select


	'時間依存 -------------------------------------
	Select Case p_RTM(0).p_iBGWOrder
	Case BGW_ORDER.FLCV_START_BUTTON_CNF
		bSetA = False
		bSetB = False
		bSetC = False
		btnExec.Enabled = False

	Case BGW_ORDER.FLCV_MEAS_PROCESS
		If iCount * timMain.Interval  >= iRealInterValTime  Then                'XmS毎にリアルタイムチャート更新(更新タイミングを調整)
			'チャート表示更新 ------------------
			If p_RTM(0).p_FlcvArrayUpdate = 0
				TimeDepTableAddRow(nudThinning.Value)
				ViscosityChartDispT(bSetA, ChartA, ds)							'粘度チャート更新
				ChartDisp2(bSetB, ChartB, ds)									'StrageMod-ElsaticMod モニタグラフ
				ChartDisp3(bSetC, ChartC, ds)									'位相 モニタグラフ
			Else
				Debug.Print("Update!")
			End If
			'進捗表示 蓄積データ数
			Dim sState As String = ""
			Select Case p_RTM(0).iViscoState
			Case RealTimeMeas.eFl3State.DATA_PRO_dTime1
				sState =  m_sStr(eMes.INI_HIGH)
			Case RealTimeMeas.eFl3State.DATA_PRO_dTime2
				sState = m_sStr(eMes.INI_RECOVERY)
			Case RealTimeMeas.eFl3State.DATA_PRO_dTime3
				sState = m_sStr(eMes.LOW_SHEAR1)
			Case RealTimeMeas.eFl3State.DATA_PRO_dTime4
				sState = m_sStr(eMes.HIGH_SHEAR)
			Case RealTimeMeas.eFl3State.DATA_PRO_dTime5
				sState = m_sStr(eMes.LOW_SHEAR2)
			End Select
			lblMsg.Text = m_sStr(eMes.iAccNum) & p_dtFile.DataCount.ToString() & Space(2) & _
						m_sStr(eMes.ELAPSED_TIME) & (p_dtFile.DataCount / MEAS_BLOCK_DATA_NUM).ToString() & m_sStr(eMes.ELAPSED_TIME_UNIT) & Space(2) & _
						sState
			'-----------------------------------
			Select Case iScanTimeAry.Max
			Case < 100
				iRealIntervalTime = 1000
			Case 100 To 200
				iRealIntervalTime = 1500
			Case 200 To 500
				iRealIntervalTime = 2000
			Case 500 To 750
				iRealIntervalTime = 3000
			Case 750 To 1000
				iRealIntervalTime = 4000
			Case > 1000
				iRealIntervalTime = 5000
			End Select
			iCount = 0
			Array.Resize(iScanTimeAry, iCount+1)
			iScanTimeAry(iCount) = p_RTM(0).p_lElapseTime
		Else
			iCount += 1
			Array.Resize(iScanTimeAry, iCount+1)
			iScanTimeAry(iCount) = p_RTM(0).p_lElapseTime
		End If
	Case BGW_ORDER.FLCV_MEAS_PRO_FINISH
		'測定後の処理
		'チャート表示更新 ------------------
		TimeDepTableAddRow(1)
		ViscosityChartDispT(bSetA, ChartA, ds)
		lblMsg.Text = m_sStr(eMes.iAccNum) & p_dtFile.DataCount.ToString()	'蓄積データ数
		'-----------------------------------
		p_RTM(0).iViscoState = eViscoState.NOT_START
		lblMsg.Text = m_sStr(eMes.iMeasEND)					'測定終了メッセージ
		Call WriteLog("フローカーブ 測定終了 " & System.Reflection.MethodBase.GetCurrentMethod.Name, eLOG_LEVEL.iINFO)

	End Select

	'リアルタイム値更新
	RealTimeMonitorValDispUpdate(dgvCurrVal, p_RTM(0).p_mltcp.P_StressZeroEnable, 1)
#If 0
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
#end if
	'ゼロ調ボタン
	If p_RTM(0).p_iZeroAdjOk <= 0
		If Not p_bZStrStabDis		
			If Math.abs(frmMain.dZStability) < p_cts.SGStressStab
				btnZeroAdj.Enabled = True                      'ゼロ調ボタン有効
			Else
				btnZeroAdj.Enabled = False
			End If
		Else
			btnZeroAdj.Enabled = True                      'ゼロ調ボタン有効
		End If
	ElseIf p_RTM(0).p_iZeroAdjOk = 1
		btnZeroAdj.Enabled = True	                     'ゼロ調ボタン有効
	End If

	'温度ボタン
	Dim iTempBtnState As Int16 = IIf( (p_RTM(0).p_iHeatRdy = 3) And (p_RTM(0).p_mltcp.isStatHeaterOn = True) And (p_RTM(0).p_mltcp.isStatHeaterOn2 = True), 1, 0 )
	If iTempBtnState = 0
		If p_RTM(0).p_mltcp.isStatHeaterOn And p_RTM(0).p_mltcp.isStatHeaterOn2
			sbObjBackClrBlink(btnTemperature, Color.LightCyan, Color.DarkOrange, bColor)
		Else
			btnTemperature.BackColor = Color.LightCyan
		End if
		btnTemperature.Text = "Temp"
	Else
		btnTemperature.BackColor = Color.DarkOrange
		btnTemperature.Text = "Temp READY"
	End If
	bColor = Not bColor

	'測定開始ボタン有効無効
	btnExec.Enabled = If( p_RTM(0).p_iZeroAdjOk = 1 AndAlso iTempBtnState = 1, True, False )

	btnENableCororUpdate(btnExec)
	btnENableCororUpdate(btnStop)

	'Online/Offline 表示更新
	ConnectStateDisp()

Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub



''' <summary>
''' フローカーブ メイングラフ表示更新
''' </summary>
''' <param name="bSet">False:未設定 True:既設定</param>
''' <param name="oChart"></param>
''' <param name="ds"></param>
Public Sub ChartDisp1(ByRef bSet As Boolean, ByRef oChart As Chart, ByVal ds As Dataset)
Dim sSeriesName1 As String = Nothing		'Y1軸系列名
Dim sSeriesName2 As String = Nothing		'Y2軸系列名
Dim sTbl		As String = Nothing
Dim dt			As Datatable = dtSpeedDep
Try
	if ds.Tables.Count = 0
		Exit sub
	End If
	Select Case p_RTM(0).p_FlcvMeasMode
	Case eViscoMeaure.SpeedDep
		dt = dtSpeedDep
	Case eViscoMeaure.TempDep
		dt = dtTempDep
	Case eViscoMeaure.TimeDep
		dt = dtTimeDep
	End Select
'チャートコントロールの設定
	With oChart

	sTbl = dt.TableName
	sSeriesName1 = dt.Columns(iY1Sel).ColumnName			'Y1軸系列名
	sSeriesName2 = dt.Columns(iY2Sel).ColumnName			'Y2軸系列名
	If Not bSet
		'初期化
		.Series.Clear()
		.Legends.Clear()
		.Titles.Clear()
		.ChartAreas.Clear()
		.DataSource = dt

		'X,Yを取得
		.Series.Add(sSeriesName1)
		.Series.Add(sSeriesName2)
		.Series(sSeriesName1).YValueMembers = sSeriesName1
		.Series(sSeriesName2).YValueMembers = sSeriesName2
		.Series(sSeriesName1).XValueMember = dt.Columns(iXSel).ColumnName
		.Series(sSeriesName2).XValueMember = dt.Columns(iXSel).ColumnName

		'Y1軸設定
		.Series(sSeriesName1).ChartType		= SeriesChartType.Line
		.Series(sSeriesName1).MarkerSize	= 10
		.Series(sSeriesName1).MarkerStyle	= MarkerStyle.Circle
		.Series(sSeriesName1).YAxisType		= AxisType.Primary
		.Series(sSeriesName1).Color			= iY1PlotColor

		'Y2軸設定
		.Series(sSeriesName2).ChartType		= SeriesChartType.Line
		.Series(sSeriesName2).MarkerSize	= 10
		.Series(sSeriesName2).MarkerStyle	= MarkerStyle.Circle
		.Series(sSeriesName2).YAxisType		= AxisType.Secondary
		.Series(sSeriesName2).Color			= iY2PlotColor

		'TITLE 作成
		Dim title1 As New Title
		'title1.Text =  sDtItemStr(iY1Sel) & "/" & sDtItemStr(iY2Sel) & " - " & sDtItemStr(iXSel)
		title1.Text =  sDispItemName(iY1Sel) & "/" & sDispItemName(iY2Sel) & " - " & sDispItemName(iXSel)
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
		.Legends(0).Enabled = False

		.ChartAreas(0).BackColor						= p_cts.clrGraphBack                             'プロット領域の背景色
		.ChartAreas(0).BorderColor						= Color.DarkGray                                 'プロット領域の境界色

		.ChartAreas(0).AxisX.Title						= sDispItemName(iXSel) & sDispItemUnit(iXSel)
        .ChartAreas(0).AxisX.TitleFont                  = New Font(BASE_FONT, 12, FontStyle.Regular)
		.ChartAreas(0).AxisX.Crossing                   = Double.NaN
		.ChartAreas(0).AxisX.IntervalType               = DateTimeIntervalType.NotSet
		.ChartAreas(0).AxisX.MajorGrid.Enabled          = True
		.ChartAreas(0).AxisX.MajorGrid.LineDashStyle    = ChartDashStyle.Dot
		.ChartAreas(0).AxisX.MajorGrid.LineColor        = Color.LightGray
		.ChartAreas(0).AxisX.Interval					= 0
		.ChartAreas(0).AxisX.LabelStyle.Format			= "F2"

		.ChartAreas(0).AxisY.Title                      = sDispItemName(iY1Sel) & sDispItemUnit(iY1Sel)
		.ChartAreas(0).AxisY.TitleFont                  = New Font(BASE_FONT, 12, FontStyle.Regular)
        .ChartAreas(0).AxisY.TitleForeColor             = iY1PlotColor
		.ChartAreas(0).AxisY.Enabled					= AxisEnabled.Auto
		.ChartAreas(0).AxisY.Crossing                   = Double.NaN
		.ChartAreas(0).AxisY.IntervalType               = DateTimeIntervalType.NotSet
		.ChartAreas(0).AxisY.MajorGrid.Enabled          = True
		.ChartAreas(0).AxisY.MajorGrid.LineDashStyle    = ChartDashStyle.Dot
		.ChartAreas(0).AxisY.MajorGrid.LineColor        = Color.LightGray
		.ChartAreas(0).AxisY.Interval					= 0
		.ChartAreas(0).AxisY.LabelStyle.Format			= "F2"

		.ChartAreas(0).AxisY2.Title                     = sDispItemName(iY2Sel) & sDispItemUnit(iY2Sel)
		.ChartAreas(0).AxisY2.TitleFont                 = New Font(BASE_FONT, 12, FontStyle.Regular)
        .ChartAreas(0).AxisY2.TitleForeColor            = iY2PlotColor
		.ChartAreas(0).AxisY2.Enabled					= AxisEnabled.Auto
		.ChartAreas(0).AxisY2.Crossing                  = Double.NaN
		.ChartAreas(0).AxisY2.IntervalType              = DateTimeIntervalType.NotSet
		.ChartAreas(0).AxisY2.MajorGrid.Enabled         = True
		.ChartAreas(0).AxisY2.MajorGrid.LineDashStyle   = ChartDashStyle.Dot
		.ChartAreas(0).AxisY2.MajorGrid.LineColor       = Color.LightGray
		.ChartAreas(0).AxisY2.Interval					= 0
		.ChartAreas(0).AxisY2.LabelStyle.Format			= "F2"

		'-------------------------------------------------------------------------------------------------------------
		'ChartAreaのサイズ位置調整
		.ChartAreas(0).Position.Auto					= False
		.ChartAreas(0).Position.X						= 0
		.ChartAreas(0).Position.Y						= 20
		.ChartAreas(0).Position.height					= 90
		.ChartAreas(0).Position.Width					= 100

		bSet = True
	End If

    '-----------------------------------------------------------------------------------------------------
    '以下、X軸とY軸の設定
	Dim XMax As Double
	Dim XMin As Double
	Dim YMax As Double
	Dim Y2Max As Double
	Dim YMin As Double
	If dt.Rows.Count>0
		m_dFlcvXAxisMax = Convert.ToDouble( dt.Compute("MAX(" & sDtItemStr(iXSel) & ")", "") )
		m_dFlcvXAxisMin = Convert.ToDouble( dt.Compute("MIN(" & sDtItemStr(iXSel) & ")", "") )
	Else
		m_dFlcvXAxisMax = 100
		m_dFlcvXAxisMin = 0
	End If
	XMax = m_dFlcvXAxisMax
	XMin = m_dFlcvXAxisMin
	'AxsisScaleAdjust(oChart, 0, XMax, XMin)
	oChart.ChartAreas(0).AxisX.LabelStyle.Format			= "F3"

	if (XMax = -1) Or (XMax - XMin <= 0) Or Double.IsNaN(XMax)
		.ChartAreas(0).AxisX.Maximum = 100
		.ChartAreas(0).AxisX.Minimum = 0
		.ChartAreas(0).AxisX.Interval = 10

	Else
		'.ChartAreas(0).AxisX.Maximum = XMax + (XMax * 0.1)
		'.ChartAreas(0).AxisX.Minimum = If( XMin - (XMin * 0.1) < 0, 0, XMin - (XMin * 0.1) )
		.ChartAreas(0).AxisX.Maximum = XMax
		.ChartAreas(0).AxisX.Minimum = 0
		.ChartAreas(0).AxisX.Interval = XMax / 10
		If (dt.Rows.Count-1)<=0
			.ChartAreas(0).AxisX.Interval = (XMax-XMin) / 1
		Else
			.ChartAreas(0).AxisX.Interval = (XMax-XMin) / (dt.Rows.Count-1)
		End If

	End If

	If dt.Rows.Count>0
		YMax	= Convert.ToDouble( dt.Compute("MAX(" & sDtItemStr(iY1Sel) & ")", "") )
		Y2Max	= Convert.ToDouble( dt.Compute("MAX(" & sDtItemStr(iY2Sel) & ")", "") )
		YMin = 0
	Else
		YMax = 10
		YMin = 0
	End If
	AxsisScaleAdjust(oChart, 1, YMax, YMin)
	If YMax<=YMin
		YMax = 10
		YMin = 0
	ElseIf Double.IsNaN(YMax) OrElse Double.IsNaN(YMin)
		YMax = 10
		YMin = 0
	ElseIf Double.IsInfinity(YMax) OrElse Double.IsInfinity(YMin)
		YMax = 10
		YMin = 0
	Else
	End If
	.ChartAreas(0).AxisY.Maximum = YMax
	.ChartAreas(0).AxisY.Minimum = YMin

	AxsisScaleAdjust(oChart, 2, Y2Max, YMin)
	If Y2Max<=YMin
		Y2Max = 10
		YMin = 0
	ElseIf Double.IsNaN(Y2Max) OrElse Double.IsNaN(YMin)
		Y2Max = 10
		YMin = 0
	ElseIf Double.IsInfinity(Y2Max) OrElse Double.IsInfinity(YMin)
		Y2Max = 10
		YMin = 0
	Else
	End If
	.ChartAreas(0).AxisY2.Maximum = Y2Max
	.ChartAreas(0).AxisY2.Minimum = YMin

	'データテーブルとの結合
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
Dim dPreTempXMax	As Double = 100
Dim dPreTempYMax	As Double = 100
Dim	dPreXData		As Double = 0.0
Dim	dPreYData		As Double = 0.0
Dim	dPreShearMod	As Double = 0.0
Dim	dPreXPosPhase	As Double = 0.0
Dim	dPreXLossTan	As Double = 0.0
''' <summary>
''' StorageMod/ElasticModモニタグラフ
''' </summary>
''' <param name="bSet"></param>
''' <param name="oChart"></param>
''' <param name="ds"></param>
Public Sub ChartDisp2(ByRef bSet As Boolean, ByRef oChart As Chart, ByVal ds As Dataset)
Dim sSeriesName1 As String = Nothing		'Y1軸系列名
Dim sTbl		As String = Nothing
Dim dt			As Datatable = dtSpeedDep
Try
	Select Case p_RTM(0).p_FlcvMeasMode
	Case eViscoMeaure.SpeedDep
		dt = dtSpeedDep
	Case eViscoMeaure.TempDep
		dt = dtTempDep
	Case eViscoMeaure.TimeDep
		dt = dtTimeDep
	End Select
'チャートコントロールの設定
	With oChart
	sTbl = dt.TableName
	sSeriesName1 = dt.Columns(7).ColumnName			'Y1軸系列名

	If Not bSet
		'初期化
		.Series.Clear()
		.Legends.Clear()
		.Titles.Clear()
		.ChartAreas.Clear()
		'.DataSource = dt

		'X,Yを取得
		.Series.Add(sSeriesName1)
		.Series(sSeriesName1).YValueMembers = sSeriesName1
		.Series(sSeriesName1).XValueMember = dt.Columns(6).ColumnName

		'Y1軸設定
		.Series(sSeriesName1).ChartType		= SeriesChartType.Line
		.Series(sSeriesName1).BorderWidth	= 3
		.Series(sSeriesName1).MarkerSize	= 8
		.Series(sSeriesName1).MarkerStyle	= MarkerStyle.Circle
		.Series(sSeriesName1).YAxisType		= AxisType.Primary
		.Series(sSeriesName1).Color			= iY1PlotColor

		'TITLE 作成
		Dim title1 As New Title
		title1.Text =  sDispItemName(7) & " - " & sDispItemName(6)
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
		.Legends(0).Enabled = False

		.ChartAreas(0).BackColor						= p_cts.clrGraphBack                             'プロット領域の背景色
		.ChartAreas(0).BorderColor						= Color.DarkGray                                 'プロット領域の境界色

		.ChartAreas(0).AxisX.Title						= sDispItemName(6) & sDispItemUnit(6)
		.ChartAreas(0).AxisX.TitleAlignment				= StringAlignment.Far
        .ChartAreas(0).AxisX.TitleFont                  = New Font(BASE_FONT, 12, FontStyle.Regular)
		.ChartAreas(0).AxisX.Crossing                   = Double.NaN
'		.ChartAreas(0).AxisX.Crossing                   = 0
		.ChartAreas(0).AxisX.IntervalType               = DateTimeIntervalType.NotSet
		.ChartAreas(0).AxisX.MajorGrid.Enabled          = True
		.ChartAreas(0).AxisX.MajorGrid.LineDashStyle    = ChartDashStyle.Dot
		.ChartAreas(0).AxisX.MajorGrid.LineColor        = Color.LightGray
		.ChartAreas(0).AxisX.Interval					= dStgElaGXInterval
		.ChartAreas(0).AxisX.Interval					= 0
		.ChartAreas(0).AxisX.LabelStyle.Format			= "F0"

		'.ChartAreas(0).AxisY.Title                      = sDtItemStr(7) & sDtItemUnit(7)
		.ChartAreas(0).AxisY.Title                      = sDispItemName(7) & sDispItemUnit(7)
		.ChartAreas(0).AxisY.TitleAlignment				= StringAlignment.Far
		.ChartAreas(0).AxisY.TitleFont                  = New Font(BASE_FONT, 12, FontStyle.Regular)
		.ChartAreas(0).AxisY.Enabled					= AxisEnabled.Auto
		.ChartAreas(0).AxisY.Crossing                   = Double.NaN
'		.ChartAreas(0).AxisY.Crossing                   = 0
		.ChartAreas(0).AxisY.IntervalType               = DateTimeIntervalType.NotSet
		.ChartAreas(0).AxisY.MajorGrid.Enabled          = True
		.ChartAreas(0).AxisY.MajorGrid.LineDashStyle    = ChartDashStyle.Dot
		.ChartAreas(0).AxisY.MajorGrid.LineColor        = Color.LightGray
		.ChartAreas(0).AxisY.Interval					= dStgElaGYInterval
		.ChartAreas(0).AxisY.Interval					= 0
		.ChartAreas(0).AxisY.LabelStyle.Format			= "F0"

		'-------------------------------------------------------------------------------------------------------------
		'ChartAreaのサイズ位置調整
		.ChartAreas(0).Position.Auto					= False
		.ChartAreas(0).Position.X						= 0
		.ChartAreas(0).Position.Y						= 20
		.ChartAreas(0).Position.height					= 90
		.ChartAreas(0).Position.Width					= 75

		bSet = True
	End If

    '-----------------------------------------------------------------------------------------------------
    '以下、Ｘ軸とＹ軸の設定
	Dim XMax			As Double
	Dim XMin			As Double
	Dim YMax			As Double
	Dim YMin			As Double
	Dim dTempXMax		As Double = 0.0
	Dim dTempYMax		As Double = 0.0
	Dim dXData			As Double = 0
	Dim dYData			As Double = 0
	Dim dShearMod		As Double = 0
	Dim dXPosPhase		As Double = 0
	Dim dXLossTan		As Double = 0
	Dim bNoData			As Boolean = False
	Dim iLIdx	As Int64 = 0

	'記憶配列から最新データを取得
	Dim bDummy As Boolean = false
	NewDataGet(p_Dtfile.dXStrgMod, dXData, bNoData)
	NewDataGet(p_Dtfile.dXElasMod, dYData, bNoData)
	NewDataGet(p_Dtfile.dShearMod, dShearMod, bDummy)
	NewDataGet(p_Dtfile.dXPosPhase, dXPosPhase, bDummy)
	NewDataGet(p_Dtfile.dXLossTan, dXLossTan, bDummy)
#If 0
	If Not IsNothing(p_Dtfile.dXStrgMod)
		dXData = p_Dtfile.dXStrgMod(p_Dtfile.dXStrgMod.getupperbound(0))
	Else
		dXData = 0
		bNoData = True
	End If
	If Not IsNothing(p_Dtfile.dXElasMod)
		dYData = p_Dtfile.dXElasMod(p_Dtfile.dXElasMod.getupperbound(0))
	Else
		dYData = 0
		bNoData = True
	End If
	If Not IsNothing(p_Dtfile.dShearMod)
		dShearMod = p_Dtfile.dShearMod(p_Dtfile.dShearMod.GetUpperBound(0))
	Else
		dShearMod = 0
	End If
	If Not IsNothing(p_Dtfile.dXPosPhase)
		dXPosPhase = p_Dtfile.dXPosPhase(p_Dtfile.dXPosPhase.GetUpperBound(0))
	Else
		dXPosPhase = 0
	End If
	If Not IsNothing(p_Dtfile.dXLossTan)
		dXLossTan = p_Dtfile.dXLossTan(p_Dtfile.dXLossTan.GetUpperBound(0))
	Else
		dXLossTan = 0
	End If
#end if
	'どれかが0なら取得タイミングが悪いので前回値を使用する
'	If dXData = 0 Or dYData = 0 Or dShearMod = 0 Or dXPosPhase = 0 Or dXLossTan = 0
	'せん断弾性率が0なら取得タイミングが悪いので前回値を使用する
	If dShearMod = 0 
		dXData		= dPreXData
		dYData		= dPreYData
		dShearMod	= dPreShearMod
		dXPosPhase	= dPreXPosPhase
		dXLossTan	= dPreXLossTan
	Else
	'前回値へ退避
		dPreXData		= dXData
		dPreYData		= dYData
		dPreShearMod	= dShearMod
		dPreXPosPhase	= dXPosPhase
		dPreXLossTan	= dXLossTan
	End If

#If 1
	'軸スケールの最大最小値を計算する
	If Not IsNothing(p_Dtfile.dShearMod)
		'どれかが0なら取得タイミングが悪いので前回値を使用する
		If dShearMod = 0 Or dXPosPhase = 0 Or Double.IsNaN(dShearMod) Or Double.IsNaN(dXPosPhase)
			dTempXMax		= dPreTempXMax
			dTempYMax		= dPreTempYMax
		Else
			dTempXMax = dShearMod * Math.Cos(dXPosPhase)
			dTempYMax = dShearMod * Math.Sin(dXPosPhase)
			dPreTempXMax	= dTempXMax						'前回値へ退避
			dPreTempYMax	= dTempYMax						'前回値へ退避
		End If

		XMax = If(dTempXMax>dTempYMax, dTempXMax, dTempYMax)
		XMin = 0
		YMax = XMax
		YMin = 0
	Else
		XMax = dStgElaGXMax
		XMin = dStgElaGXMin
		YMax = dStgElaGYMax
		YMin = dStgElaGYMin
	End If
'If dTempXMax=0 Or dTempYMax=0
'Debug.Print("AAA")
'End If
#else
	XMax = dStgElaGXMax
	XMin = dStgElaGXMin
	YMax = dStgElaGYMax
	YMin = dStgElaGYMin
#end if
	AxsisScaleAdjust(oChart, 1, YMax, YMin)		'最大最小スケールの微調整
	AxsisScaleAdjust(oChart, 0, XMax, XMin)		'最大最小スケールの微調整
	'Max、Minの大小確認
	If (XMax=0 And YMax=0) Or (XMax<=XMin) Or (YMax<=YMin)
'Debug.Print("BBB")
	Else
		.ChartAreas(0).AxisX.Maximum = XMax
		.ChartAreas(0).AxisX.Minimum = XMin
		.ChartAreas(0).AxisY.Maximum = YMax
		.ChartAreas(0).AxisY.Minimum = YMin
		'.ChartAreas(0).AxisX.Maximum = 100
		'.ChartAreas(0).AxisX.Minimum = 0
		'.ChartAreas(0).AxisY.Maximum = 100
		'.ChartAreas(0).AxisY.Minimum = 0
		.ChartAreas(0).AxisX.Interval = XMax / 5
	End If

	If Not IsNothing(.Series(sSeriesName1).Points)
		If .Series(sSeriesName1).Points.Count >= 1
			.Series(sSeriesName1).Points.Clear				'プロットクリア
		End If
	End If
	
	If Not bNoData
		'データ線描画
		.Series(sSeriesName1).Points.AddXY(1E-5,	0)		'X値に限りなく近い0を設定しないとX値１にプロットされてしまう
		.Series(sSeriesName1).Points.AddXY(dXData,	dYData)

		'測定数値表示
		lblStrgModVal.Text		= dXData.ToString("F2")
		lblElasticModVal.Text	= dYData.ToString("F2")
		lblShearModVal.Text		= dShearMod.ToString("F2")
		lblPhaseVal.Text		= (dXPosPhase*RadToDegRate).ToString("F2")
		lblTangentVal.Text		= dPreXLossTan.ToString("F5")
	Else
		.Series(sSeriesName1).Points.AddXY(1E-5, 0)
	End If

    End With
Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub
''' <summary>
''' 計算配列から最新データを取得
''' </summary>
''' <param name="dData">計算データ配列</param>
''' <param name="dNewData">最新データ</param>
''' <param name="bNoData">True:データなし</param>
Private sub NewDataGet(ByVal dData() As Double, ByRef dNewData As Double, ByRef bNoData As Boolean)
Try
	If Not IsNothing(dData)
		If dData.Length<>0
			dNewData = dData(dData.getupperbound(0))
		Else
			dNewData = 0
			bNoData = True
		End If
	Else
		dNewData = 0
		bNoData = True
	End If
Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End sub

Dim dStress()	As Double = Nothing
Dim dPosition() As Double = Nothing
''' <summary>
''' 波形モニタグラフ
''' </summary>
''' <param name="bSet"></param>
''' <param name="oChart"></param>
''' <param name="ds"></param>
Public Sub ChartDisp3(ByRef bSet As Boolean, ByRef oChart As Chart, ByVal ds As Dataset)
Dim sSeriesName1	As String = "Position"		'Y1軸系列名
Dim sSeriesName2	As String = "Stress"		'Y2軸系列名
Dim i				As Int64
Dim j				As Int64
Dim i1TCnt			As Int64 = p_RTM(0).p_mltcp.PeriodX(1) * MEAS_BLOCK_DATA_NUM			'1T分のサンプル数
Dim sIdx			As Int64 = 0
Dim PretempX_Qp		As Double = 999
Dim iPhShift		As Int64 = 0
Try
'チャートコントロールの設定
	With oChart
	If Not bSet
		'初期化
		.Series.Clear()
		.Legends.Clear()
		.Titles.Clear()
		.ChartAreas.Clear()

		'X,Yを取得
		.Series.Add(sSeriesName1)
		.Series.Add(sSeriesName2)
		.Series(sSeriesName1).YValueMembers = sSeriesName1
		.Series(sSeriesName1).XValueMember = "Time"

		'Y1軸設定
		.Series(sSeriesName1).ChartType		= SeriesChartType.Line
		.Series(sSeriesName1).BorderWidth	= 1
		.Series(sSeriesName1).MarkerStyle	= MarkerStyle.None
		.Series(sSeriesName1).YAxisType		= AxisType.Primary
		.Series(sSeriesName1).Color			= iY1PlotColor
		'Y2軸設定
		.Series(sSeriesName2).ChartType		= SeriesChartType.Line
		.Series(sSeriesName2).BorderWidth	= 1
		.Series(sSeriesName2).MarkerStyle	= MarkerStyle.None
		.Series(sSeriesName2).YAxisType		= AxisType.Secondary
		.Series(sSeriesName2).Color			= iY2PlotColor

		'TITLE 作成
		Dim title1 As New Title
		'title1.Text =  "Wave monitor"
		title1.Text =  m_sStr(eMes.iWaveMonTitle)
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
		.Legends(0).Enabled = False

		.ChartAreas(0).BackColor						= p_cts.clrGraphBack                             'プロット領域の背景色
		.ChartAreas(0).BorderColor						= Color.DarkGray                                 'プロット領域の境界色
		.ChartAreas(0).BorderDashStyle                  = ChartDashStyle.Solid

		.ChartAreas(0).AxisX.Title						= m_sStr(eMes.iWaveMonX)
		.ChartAreas(0).AxisX.TitleAlignment				= StringAlignment.Center
        .ChartAreas(0).AxisX.TitleFont                  = New Font(BASE_FONT, 12, FontStyle.Regular)
		.ChartAreas(0).AxisX.Crossing                   = Double.NaN
		.ChartAreas(0).AxisX.IntervalType               = DateTimeIntervalType.NotSet
		.ChartAreas(0).AxisX.MajorGrid.Enabled          = True
		.ChartAreas(0).AxisX.MajorGrid.LineDashStyle    = ChartDashStyle.Dot
		.ChartAreas(0).AxisX.MajorGrid.LineColor        = Color.LightGray
		.ChartAreas(0).AxisX.Interval					=  p_RTM(0).p_mltcp.PeriodX(1) / 2
		.ChartAreas(0).AxisX.LabelStyle.Format			= "F"

		.ChartAreas(0).AxisY.Title                      = m_sStr(eMes.iWaveMonSrs1)
		.ChartAreas(0).AxisY.TitleAlignment				= StringAlignment.Center
		.ChartAreas(0).AxisY.TitleFont                  = New Font(BASE_FONT, 12, FontStyle.Regular)
		.ChartAreas(0).AxisY.TitleForeColor				= iY1PlotColor
		.ChartAreas(0).AxisY.Enabled					= AxisEnabled.Auto
		.ChartAreas(0).AxisY.Crossing                   = 0
		.ChartAreas(0).AxisY.IntervalType               = DateTimeIntervalType.NotSet
		.ChartAreas(0).AxisY.MajorGrid.Enabled          = False
		.ChartAreas(0).AxisY.Interval					= Double.MaxValue

		.ChartAreas(0).AxisY2.Title                     = m_sStr(eMes.iWaveMonSrs2)
		.ChartAreas(0).AxisY2.TitleAlignment			= StringAlignment.Center
		.ChartAreas(0).AxisY2.TitleFont                 = New Font(BASE_FONT, 12, FontStyle.Regular)
		.ChartAreas(0).AxisY2.TitleForeColor            = iY2PlotColor
		.ChartAreas(0).AxisY2.Enabled					= AxisEnabled.Auto
		.ChartAreas(0).AxisY2.Crossing                  = 0
		.ChartAreas(0).AxisY2.IntervalType              = DateTimeIntervalType.NotSet
		.ChartAreas(0).AxisY2.MajorGrid.Enabled         = False
		.ChartAreas(0).AxisY2.Interval					= Double.MaxValue

		'-------------------------------------------------------------------------------------------------------------
		'ChartAreaのサイズ位置調整
		.ChartAreas(0).Position.Auto					= False
		.ChartAreas(0).Position.X						= 0
		.ChartAreas(0).Position.Y						= 20
		.ChartAreas(0).Position.height					= 90
		.ChartAreas(0).Position.Width					= 100

		bSet = True
	End If
    '-----------------------------------------------------------------------------------------------------
	'データ取得 最新１T分のサンプルを取得

	Array.Resize(dStress,	i1TCnt)
	Array.Resize(dPosition, i1TCnt)
	If Not IsNothing( datCal.tempXp_QM )
	'位相ずらしカウントを計算
		iPhShift = p_RTM(0).p_dFullStressPxPhase * ( i1TCnt / (2 * Math.PI) )
	'位置波形の立上り０交差インデックスを求める
		j = 0
		If datCal.tempXp_QM.GetUpperBound(0) > (2 * i1TCnt)
			For i = datCal.tempXp_QM.GetUpperBound(0) To datCal.tempXp_QM.GetUpperBound(0) - (2 * i1TCnt) Step -1
				If j >  i1TCnt
					If datCal.tempXp_QM(i)>=0 And PretempX_Qp<0
						sIdx = i + 1				'立上りインデックス
						Exit For
					End If
				End If
				PretempX_Qp = datCal.tempXp_QM(i)	'前回値を退避
				j += 1 
			Next i
			If sIdx-iPhShift >= 0 And sIdx<>0
				Array.Copy( datCal.tempX_QM,  sIdx-iPhShift,	dStress,	0, i1TCnt)		'応力波形の切り出し
				Array.Copy( datCal.tempXp_QM, sIdx,				dPosition,	0, i1TCnt)		'位置波形の切り出し
			End If
		End If
'debug.Print(tempXp_Q.GetUpperBound(0))
	End If

    '-----------------------------------------------------------------------------------------------------
    '以下、Ｘ軸とＹ軸の設定
	Dim XMax As Double
	Dim XMin As Double
	Dim YMax As Double
	Dim YMin As Double
	Dim Y2Max As Double
	Dim Y2Min As Double

	XMax = p_RTM(0).p_mltcp.PeriodX(1)
	XMin = 0
	If Not IsNothing(dStress)
		Y2Max	= dStress.Max	+ dStress.Max*5/100
		Y2Min	= dStress.Min	+ dStress.Min*5/100
		YMax	= dPosition.Max + dPosition.Max*5/100
		YMin	= dPosition.Min + dPosition.Min*5/100
	Else
		Y2Max	= 1000.0
		Y2Min	= -1000.0
		YMax	= 1000.0
		YMin	= -1000.0
	End If
	If YMax <= YMin
		YMax = 1000.0
		YMin = -1000.0
	End If
	If Y2Max <= Y2Min
		Y2Max = 1000.0
		Y2Min = -1000.0
	End If

'Debug.Print("XMax:{0} XMin:{1} YMax:{2} YMin:{3}  Y2Max:{4} Y2Min:{5} ", XMax, XMin, YMax, YMin, Y2Max, Y2Min )
	If Double.IsNaN(XMax) Or Double.IsNaN(XMin) Or Double.IsNaN(YMax) Or Double.IsNaN(YMin) Or Double.IsNaN(Y2Max) Or Double.IsNaN(Y2Min)
		Debug.Print("NaN !!")
	Else
		.ChartAreas(0).AxisX.Maximum	= XMax
		.ChartAreas(0).AxisX.Minimum	= XMin
		.ChartAreas(0).AxisY.Maximum	= YMax
		.ChartAreas(0).AxisY.Minimum	= YMin
		.ChartAreas(0).AxisY2.Maximum	= Y2Max
		.ChartAreas(0).AxisY2.Minimum	= Y2Min
	End If

	If Not IsNothing(.Series(sSeriesName1).Points)
		If .Series(sSeriesName1).Points.Count >= 1
			.Series(sSeriesName1).Points.Clear				'プロットクリア
		End If
	End If
	If Not IsNothing(.Series(sSeriesName2).Points)
		If .Series(sSeriesName2).Points.Count >= 1
			.Series(sSeriesName2).Points.Clear				'プロットクリア
		End If
	End If
	
	'データ線描画
	For i = 0 To dStress.GetUpperBound(0)
		.Series(sSeriesName2).Points.AddXY(i*0.002, dStress(i))
		.Series(sSeriesName1).Points.AddXY(i*0.002, dPosition(i))
	Next i

    End With
Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub

''' <summary>
''' 軸調整
''' </summary>
''' <param name="iXY">0:X 1:Y1 2:Y2</param>
''' <param name="YMax"></param>
''' <param name="YMin"></param>
Private Sub AxsisScaleAdjust(ByRef oChart As Chart, ByVal iXY As Int32, ByRef YMax As Double, ByRef YMin As Double )
	if YMax <= -1
		YMax = 100
		YMin = 0
	Else
		Dim dYMaxH As Double
		Select Case YMax
		Case < 1.0
			dYMaxH = 1
			Select Case iXY
			Case 0
				oChart.ChartAreas(0).AxisX.LabelStyle.Format			= "F2"
			Case 1
				oChart.ChartAreas(0).AxisY.LabelStyle.Format			= "F2"
			Case 2
				oChart.ChartAreas(0).AxisY2.LabelStyle.Format			= "F2"
			End Select
		Case < 10.0
			dYMaxH = 10
			Select Case iXY
			Case 0
				oChart.ChartAreas(0).AxisX.LabelStyle.Format			= "F1"
			Case 1
				oChart.ChartAreas(0).AxisY.LabelStyle.Format			= "F1"
			Case 2
				oChart.ChartAreas(0).AxisY2.LabelStyle.Format			= "F1"
			End Select
		Case Else
			dYMaxH = Math.Ceiling(YMax/10) * 10
			Select Case iXY
			Case 0
				oChart.ChartAreas(0).AxisX.LabelStyle.Format			= "F0"
			Case 1
				oChart.ChartAreas(0).AxisY.LabelStyle.Format			= "F0"
			Case 2
				oChart.ChartAreas(0).AxisY2.LabelStyle.Format			= "F0"
			End Select

		End Select
		YMax = dYMaxH

		YMin = If(YMin-1 < 0, 0, YMin - 1)
		If YMax - YMin <= 0
			YMax = 1000
			YMin = 0
		Else
			YMax = YMax
			YMin = YMin
		End If
	End If
End Sub
Dim frgToolTip As Boolean = True
'データポイントのマウスツールチップテキストを表示
Private Sub ChartA_GetToolTipText(ByVal sender As Object, ByVal e As System.Windows.Forms.DataVisualization.Charting.ToolTipEventArgs) Handles ChartA.GetToolTipText
Try
	'時間依存は表示しない
	'If p_RTM(0).p_FlcvMeasMode = eViscoMeaure.TimeDep
	'	Return
	'End If

	If e.HitTestResult.ChartElementType = ChartElementType.DataPoint And frgToolTip = True Then
		Dim i		As Integer = e.HitTestResult.PointIndex
		Dim dp		As DataPoint = e.HitTestResult.Series.Points(i)
		Dim iSel	As Int16 = 0
		Dim iSeries As Int16 = 0
		Dim iMaxLen As Int16
		'計算している全ての行を編集
		Dim t As String = Nothing
		If p_RTM(0).p_FlcvMeasMode <> eViscoMeaure.TimeDep
			t = ChartGetComToolTipText(i, iMaxLen)
		End If
		Select Case e.HitTestResult.Series.Name
		Case ChartA.Series(0).Name
			iSel = iY1Sel
			iSeries = 0
		Case ChartA.Series(1).Name
			iSel = iY2Sel
			iSeries = 1
		End Select
		'系列名ツールチップテキストに表示
		Dim s1 As String = Nothing
		Dim s2 As String = Nothing
		'選択Y軸 行
		s1	=  String.Format("{0}{1} : " , e.HitTestResult.Series.Name,			sDtItemUnit(iSel))
		If iMaxLen - s1.Length <> 0
			s1 &= vbTab
		End If
		s1	&=  String.Format("{0:F4}", dp.YValues(0))
		'選択X軸 行
		s2	=  String.Format("{0}{1} : " , ChartA.Series(iSeries).XValueMember,	sDtItemUnit(iXSel))
		If iMaxLen - s2.Length <> 0
			s2 &= vbTab
		End If
		s2	&=  String.Format("{0:F4}", dp.XValue)
		e.Text = s1 & vbCrLf & s2 & vbCrLf & vbCrLf & t
	End If

Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub
''' <summary>
''' フローカーブ計算済み項目をツールチップ表示用文字列へ編集する
''' </summary>
''' <param name="idx">測定データインデックス</param>
''' <param name="iMaxLen">項目タイトルまでの文字数の最大</param>
''' <returns>成形編集された文字列</returns>
Private Function ChartGetComToolTipText(ByVal idx As Int32, ByRef iMaxLen As int16) As String
Dim s As String = Nothing
Dim t() As String = Nothing
Dim i As int32
Try
	'タイトルセット
	Array.Resize(t, sDtItemStr.Length)
	iMaxLen = 0
	For i=0 To t.GetUpperBound(0)
		t(i) = String.Format("{0}{1} : ", sDtItemStr(i), sDtItemUnit(i))
		If iMaxLen < t(i).Length 
			iMaxLen = t(i).Length 
		End If
	Next i
	'後端揃え
	For i=0 To t.GetUpperBound(0)
		If iMaxLen-t(i).Length <> 0
			t(i) &= vbTab
		End If
	Next i
	If Not IsNothing(p_dtFile.uVis1)
		t(0) &= p_dtFile.uVis1(idx).dShearVelocity.ToString("F2")
		t(1) &= p_dtFile.uVis1(idx).dShearStress.ToString("F2")
		t(2) &= p_dtFile.uVis1(idx).dViscosity.ToString("F2")
		t(3) &= p_dtFile.uVis1(idx).dXPosPhase.ToString("F2")
		t(4) &= p_dtFile.uVis1(idx).dSplStrain.ToString("F2")
		t(5) &= p_dtFile.uVis1(idx).dShearMod.ToString("F2")
		t(6) &= p_dtFile.uVis1(idx).dXStrgMod.ToString("F2")
		t(7) &= p_dtFile.uVis1(idx).dXElasMod.ToString("F2")
		t(8) &= p_dtFile.uVis1(idx).dXLossTan.ToString("F2")
		t(9) &= p_dtFile.uVis1(idx).dHPTemp.ToString("F1")
		t(10) &= (p_dtFile.uVis1(idx).dMeasStress * 10^6).ToString("F0")

		For i=0 To t.GetUpperBound(0)
			s &= t(i) & vbCrLf
		Next i
	Else
		s = ""
	End If
	Return s
Catch ex As Exception
	Return ""
End Try	
End Function
''' <summary>
''' フローカーブ 時間依存 グラフ表示更新(横軸 時間)
''' </summary>
''' <param name="bSet">False:未設定 True:既設定</param>
''' <param name="oChart"></param>
''' <param name="ds"></param>
Public Sub ViscosityChartDispT(ByRef bSet As Boolean, ByRef oChart As Chart, ByVal ds As Dataset)
Dim sSeriesName As String = Nothing
Dim sTbl		As String = Nothing
Dim dt			As DataTable = dtTimeDep
Try
	if ds.Tables.Count = 0
		Exit sub
	End If
'チャートコントロールの設定
	With oChart
	sTbl = dt.TableName
	sSeriesName = dt.Columns(iY1Sel).ColumnName			'Y軸系列名
	If Not bSet
		'初期化
		.Series.Clear()
		.Legends.Clear()
		.Titles.Clear()
		.ChartAreas.Clear()
		.DataSource = dt

		'X,Yを取得
		.Series.Add(sSeriesName)
		.Series(sSeriesName).YValueMembers	= sSeriesName
		.Series(sSeriesName).XValueMember	= dt.Columns(eClm.iTime).ColumnName

		.Series(sSeriesName).ChartType		= SeriesChartType.FastLine
		.Series(sSeriesName).MarkerSize		= 2
		.Series(sSeriesName).MarkerStyle	= MarkerStyle.None
		.Series(sSeriesName).YAxisType		= AxisType.Primary
		.Series(sSeriesName).Color			= Color.Blue

		'TITLE 作成
		Dim title1 As Title = new Title
		'title1.Text =  sDtItemStr(iY1Sel) & " - " & "Time"
		title1.Text =  sDispItemName(iY1Sel) & " - " & m_sStr(eMes.iWaveMonX)
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
		.Legends(0).Enabled = False

		.ChartAreas(0).BackColor						= p_cts.clrGraphBack                             'プロット領域の背景色
		.ChartAreas(0).BorderColor						= Color.DarkGray                                 'プロット領域の境界色

		.ChartAreas(0).AxisX.Title                      = m_sStr(eMes.iTimeDepX)
		.ChartAreas(0).AxisX.TitleFont                  = New Font(BASE_FONT, 12, FontStyle.Regular)
		.ChartAreas(0).AxisX.Crossing                   = Double.NaN
		.ChartAreas(0).AxisX.IntervalType               = DateTimeIntervalType.NotSet
		.ChartAreas(0).AxisX.MajorGrid.Enabled          = True
		.ChartAreas(0).AxisX.MajorGrid.LineDashStyle    = ChartDashStyle.Dot
		.ChartAreas(0).AxisX.MajorGrid.LineColor        = Color.LightGray
		.ChartAreas(0).AxisX.Interval					= 0
		.ChartAreas(0).AxisX.LabelStyle.Format			= "F2"

		.ChartAreas(0).AxisY.Title                      = sDispItemName(iY1Sel) & sDispItemUnit(iY1Sel)
		.ChartAreas(0).AxisY.TitleFont                  = New Font(BASE_FONT, 12, FontStyle.Regular)
		.ChartAreas(0).AxisY.Enabled					= AxisEnabled.Auto
		.ChartAreas(0).AxisY.Crossing                   = Double.NaN
		.ChartAreas(0).AxisY.IntervalType               = DateTimeIntervalType.NotSet
		.ChartAreas(0).AxisY.MajorGrid.Enabled          = True
		.ChartAreas(0).AxisY.MajorGrid.LineDashStyle    = ChartDashStyle.Dot
		.ChartAreas(0).AxisY.MajorGrid.LineColor        = Color.LightGray
		.ChartAreas(0).AxisY.Interval					= 0
		.ChartAreas(0).AxisY.LabelStyle.Format			= "F2"

		'-------------------------------------------------------------------------------------------------------------
		'ChartAreaのサイズ位置調整
		.ChartAreas(0).Position.Auto					= False
		.ChartAreas(0).Position.X						= 0
		.ChartAreas(0).Position.Y						= 20
		.ChartAreas(0).Position.height					= 90
		.ChartAreas(0).Position.Width					= 100

		bSet = True
	End If

    '-----------------------------------------------------------------------------------------------------
    'Ｘ軸とＹ軸のスケール設定
	Dim XMax As Double
	Dim XMin As Double
	Dim YMax As Double
	Dim YMin As Double
	XMax = m_dFlcvXAxisMax
	XMin = 0
	if XMax = -1
		.ChartAreas(0).AxisX.Maximum = 100
		.ChartAreas(0).AxisX.Minimum = 0
	Else
		.ChartAreas(0).AxisX.Maximum = XMax + (XMax * 0.1)
		.ChartAreas(0).AxisX.Minimum = 0
	End If

	If Not IsNothing(dt)
		Select Case iY1Sel
		Case eClm.iPosPhase
			YMax = 100
			YMin = 0
		Case eClm.iLossTan
			YMax = 10
			YMin = 0
		'Case eClm.iShearVelocity
		'Case eClm.iShearStress
		'Case eClm.iViscosity
		'Case eClm.iSampStrain
		'Case eClm.iShearMod
		'Case eClm.iStorageMod
		'Case eClm.iElasticMod
		'Case eClm.iHPTemp
		'Case eClm.iStress
		Case Else
			If dt.Rows.Count<>0
				YMax = Convert.ToDouble( dt.Compute("MAX(" & sDtItemStr(iY1Sel) & ")", "") )
				YMin = 0
				if YMax <= YMin
					YMax = 100
					YMin = 0
				Else
					YMax = YMax + (YMax * 0.1)
					Select Case YMax
					Case < 1.0
						YMax = 1
					Case Else
					End Select
					YMin = If( YMin - 1 < 0, YMin, YMin - 1 )
				End If
			End If
		End Select
	Else
		YMax = 100
		YMin = 0
	End If
	.ChartAreas(0).AxisY.Maximum = YMax
	.ChartAreas(0).AxisY.Minimum = YMin

	'データテーブルとの結合
	Try
	.DataBind()
	Catch ex As Exception
		Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
		Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	End Try
	'oChart.Serializer.Save("C:\Test\Chart1.xml")
	'oChart.Update()

    End With
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
	Call WriteLog("フローカーブ Zeroボタンクリック " & System.Reflection.MethodBase.GetCurrentMethod.Name, eLOG_LEVEL.iINFO)

	'If p_RTM(0).p_iBGWOrder = BGW_ORDER.NO_ORDER
		p_dtFile.ReadBufferClear()								'データバッファクリア
		p_dtFile.ViscosityBufferClear()							'データバッファクリア
	
		p_RTM(0).p_iStaMonTime				= Double.Parse(p_cts.iStaFlucRange)
		p_RTM(0).p_iStaFlucRange			= Double.Parse(p_cts.iStaMonTime)
		p_RTM(0).p_iBGWOrder				= BGW_ORDER.ZERO_BUTTON
		If p_RTM(0).p_mltcp.IsZeroStartSW()
			p_ZeroJob = 9				'ゼロ調モニタで終了処理へ
			Me.Panel2.Visible = False
		Else
			'ゼロ調モニタ位置サイズ指定
			Me.Panel2.Location	= New Point(600, 300)
			Me.Panel2.Size		= New Size(710, 470)
			p_RTM(0).p_iStaMonTime = p_cts.iStaMonTime
			p_RTM(0).iViscoState	= eViscoState.ZERO_EXEC			'ステータス->ゼロ調実行中
		End If
	'End If

Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub
#If 0
''' <summary>
''' 原点復帰ボタン
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub btnOrigin_Click(sender As Object, e As EventArgs) Handles btnOrigin.Click
Try
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name, eLOG_LEVEL.iINFO)
	If p_RTM(0).p_iBGWOrder = BGW_ORDER.NO_ORDER
		p_RTM(0).p_iBGWOrder = BGW_ORDER.ORG_BUTTON_REP
	End If

Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub
#end if
''' <summary>
''' 温度制御ボタン
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub btnTemperture_Click(sender As Object, e As EventArgs) Handles btnTemperature.Click
Try
	If Not p_bError
		Call WriteLog("フローカーブ Tempボタンクリック " & System.Reflection.MethodBase.GetCurrentMethod.Name, eLOG_LEVEL.iINFO)
		Select Case p_iMeasMode
		Case eViscoMeaure.TempDep
		'温度依存の場合は開始温度へ
			p_RTM(0).p_mltcp.TempPreset(1)	= double.parse(nudTempDepTempStart.Value)
			p_RTM(0).p_mltcp.Temp2Preset(1) = double.parse(nudTempDepTempStart.Value)
			p_RTM(0).p_mltcp.Temp3Preset(1) = p_clSpec.SGTempGet(p_RTM(0).p_mltcp.TempPreset(1))
		Case Else
			p_RTM(0).p_mltcp.TempPreset(1)	= double.parse(nudPreTemp.Value)
			p_RTM(0).p_mltcp.Temp2Preset(1) = double.parse(nudPreTemp.Value)
			p_RTM(0).p_mltcp.Temp3Preset(1) = p_clSpec.SGTempGet(p_RTM(0).p_mltcp.TempPreset(1))
		End Select
		If p_RTM(0).p_iBGWOrder <> BGW_ORDER.NO_ORDER
			p_RTM(0).p_iBGWOrderNext	= p_RTM(0).p_iBGWOrder
			p_RTM(0).p_iBGWOrder		= BGW_ORDER.TEMP_BUTTON_REP
		Else
			p_RTM(0).p_iBGWOrder = BGW_ORDER.TEMP_BUTTON_REP
		End if

	End if

Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub


''' <summary>
''' 温度設定
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub nudPreTemp_ValueChanged(sender As Object, e As EventArgs) Handles nudPreTemp.ValueChanged
	Select Case p_iMeasMode
	Case eViscoMeaure.TempDep
	'温度依存の場合は開始温度へ
	Case Else
		p_RTM(0).p_mltcp.TempPreset(1)	= double.parse(nudPreTemp.Value)
		p_RTM(0).p_mltcp.Temp2Preset(1) = double.parse(nudPreTemp.Value)
		p_RTM(0).p_mltcp.Temp3Preset(1) = p_clSpec.SGTempGet(p_RTM(0).p_mltcp.TempPreset(1))
	End Select
End Sub
''' <summary>
''' 入力GAP
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub nudInputGap_ValueChanged(sender As Object, e As EventArgs) Handles nudInputGap.ValueChanged
	p_RTM(0).p_mltcp.InpGap(1) =  double.parse(nudInputGap.Value)
End Sub
''' <summary>
''' 測定GAP
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub nudMeasureGap_ValueChanged(sender As Object, e As EventArgs) Handles nudMeasureGap.ValueChanged
	p_RTM(0).p_mltcp.GapPreset(1) =  double.parse(nudMeasureGap.Value)
End Sub
''' <summary>
''' XY動作待ち時間
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub nudXYWait_ValueChanged(sender As Object, e As EventArgs) Handles nudXYWait.ValueChanged
	p_RTM(0).p_mltcp.MesPreWait(1) =  double.parse(nudXYWait.Value)
End Sub


''' <summary>
''' 熱板上昇待ち時間
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub nudRisingHPWait_ValueChanged(sender As Object, e As EventArgs) Handles nudRisingHPWait.ValueChanged
	p_RTM(0).p_mltcp.MeasWaitTime(1) =  double.parse(nudRisingHPWait.Value)
End Sub

''' <summary>
''' 周期変更
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub nudSpeedDepPeriod_ValueChanged(sender As Object, e As EventArgs) Handles nudSpeedDepPeriod.ValueChanged, nudTempDepPeriod.ValueChanged, nudTimeDepPeriod.ValueChanged
	Select Case p_iMeasMode
	Case eViscoMeaure.SpeedDep
		p_RTM(0).p_mltcp.PeriodX(1)	= Double.parse(nudSpeedDepPeriod.Value)
		p_RTM(0).p_mltcp.PeriodY(1)	= Double.parse(nudSpeedDepPeriod.Value)
	Case eViscoMeaure.TimeDep
		p_RTM(0).p_mltcp.PeriodX(1)	= Double.parse(nudTimeDepPeriod.Value)
		p_RTM(0).p_mltcp.PeriodY(1)	= Double.parse(nudTimeDepPeriod.Value)
	Case eViscoMeaure.TempDep
		p_RTM(0).p_mltcp.PeriodX(1)	= Double.parse(nudTempDepPeriod.Value)
		p_RTM(0).p_mltcp.PeriodY(1)	= Double.parse(nudTempDepPeriod.Value)
	End Select

End Sub

''' <summary>
''' 振幅変更
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub nudTimeDepAmp_ValueChanged(sender As Object, e As EventArgs) Handles nudTempDepAmp.ValueChanged, nudLow1Amp.ValueChanged, nudLow2Amp.ValueChanged, nudHighAmp.ValueChanged,nudSpeedDepAmpStart.ValueChanged, nudSpeedDepAmpEnd.ValueChanged, nudRecoveryAmp.ValueChanged, nudPreShearAmp.ValueChanged
	Select Case p_iMeasMode
	Case eViscoMeaure.SpeedDep
		p_RTM(0).p_mltcp.AmplitudeX	= Double.parse(nudSpeedDepAmpStart.Value) * 1000.0
		p_RTM(0).p_mltcp.AmplitudeY	= Double.parse(nudSpeedDepAmpStart.Value) * 1000.0
	Case eViscoMeaure.TempDep
		p_RTM(0).p_mltcp.AmplitudeX	= Double.parse(nudTempDepAmp.Value) * 1000.0
		p_RTM(0).p_mltcp.AmplitudeY	= Double.parse(nudTempDepAmp.Value) * 1000.0
	Case eViscoMeaure.TimeDep
		p_RTM(0).p_mltcp.AmplitudeX	= Double.parse( nudLow1Amp.Value) * 1000.0
		p_RTM(0).p_mltcp.AmplitudeY	= Double.parse( nudLow1Amp.Value) * 1000.0

	End Select
End Sub





''' <summary>
''' フォームクローズ
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub frmViscosity_Closed(sender As Object, e As EventArgs) Handles Me.Closed
	p_FormCalibVisible = False
	frmMain.Show()
End Sub

''' <summary>
''' 停止ボタン
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub btnStop_Click(sender As Object, e As EventArgs) Handles btnStop.Click

	Call WriteLog("フローカーブ 停止ボタンクリック " & System.Reflection.MethodBase.GetCurrentMethod.Name, eLOG_LEVEL.iINFO)
	'他の処理中以外は停止処理しない
	Select Case p_RTM(0).p_iBGWOrder
	Case BGW_ORDER.NO_ORDER
	Case BGW_ORDER.ORG_BUTTON_REP To BGW_ORDER.ORG_BUTTON_REP_1
																'原点復帰中は処理なし
	Case Else
		Select Case p_iMeasMode
		Case eViscoMeaure.SpeedDep
		    p_RTM(0).p_iBGWOrder = BGW_ORDER.FLCV_STOP					'測定処理停止
		Case eViscoMeaure.TimeDep
		    p_RTM(0).p_iBGWOrder = BGW_ORDER.FLCV_CANCEL_BUTTON			'測定処理停止
		End Select
	End Select
	'ゼロ調モニタで終了処理へ
	p_ZeroJob = 9
	Me.Panel2.Visible = False
	p_RTM(0).p_mltcp.StartTempCont = 0								'温度制御停止
	p_RTM(0).p_mltcp.StartSW = 0
	p_RTM(0).p_iZeroAdjOk = 0                                       'ゼロ調正完了クリア

End Sub

''' <summary>
''' クローズボタン
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
	'実行中の間はフォームを閉じない
	Select Case p_RTM(0).p_iBGWOrder
	Case BGW_ORDER.NO_ORDER
		If iError = 0
		End if
		Me.Close
	End select
End Sub

''' <summary>
''' 測定開始ボタン
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub btnExec_Click(sender As Object, e As EventArgs) Handles btnExec.Click
	Dim i		As int32
	Dim j		As int32
	Dim fMsg As frmMSG
	Dim sMsg As String = nothing
	Dim title = m_sStr(emes.iMES_TITLE)
Try
	Call WriteLog("フローカーブ 測定ボタンクリック " & System.Reflection.MethodBase.GetCurrentMethod.Name, eLOG_LEVEL.iINFO)
	If (p_RTM(0).p_iConnectState = CONNECT_ST.CONNECTING) And (p_RTM(0).p_iBGWOrder = BGW_ORDER.NO_ORDER)
	'フルキャリブテーブル0チェック
		'フルキャリブレーション結果を得る
		Dim frmC As New frmCalibFull(0)
		frmC.sbFullStressCondRead(0, p_FClbX, p_FClbC, p_FClbP)			'ファイル読込み構造体配列へセット
		frmC.sbFullStressCondRead(1, p_ZClbX, p_ZClbC, p_ZClbP)			'ファイル読込み構造体配列へセット
		frmC.Dispose()
		Select Case p_iMeasMode
		Case eViscoMeaure.SpeedDep
		Case Else
			For i=0 To p_FClbC.GetUpperBound(0)
				If p_FClbC(i).iFullVal = 0
					Exit for
				End If
			Next
			If i<=p_FClbC.GetUpperBound(0)
				'フル応力キャリブレーション設定内に０値が見つかったので測定開始できません。フル応力キャリブレーション設定を確認してください。
				fMsg = New frmMSG(m_sStr(eMes.iMES_FCALIBINZERO),{"OK"},m_sStr(eMes.iMES_TITLE1),Me)
				fMsg.ShowDialog()
				Exit Sub
			End If
		End Select

		if p_RTM(0).p_iZeroAdjOk = 1
			'測定者情報その他の格納
			With p_RTM(0)
				.p_sMeasPlace	= p_cts.strAutoPrintMesurePlaceValue    '測定場所
				.p_sMeasurer	= p_cts.strAutoPrintOperatorValue       '測定者
				.p_sResinName	= p_clSpec.spi.sSampleName				'樹脂名
				.p_sLot			= p_clSpec.spi.sLot						'LOT名
				.p_sEquipName	= frmMain.cboEquipName.Text				'本体名
				.p_sXYSensorID	= p_cts.sXYSensorSerial(p_cts.iSelMDK)	'XYセンサID
				.p_sZSensorID	= p_cts.sZSensorSerial(p_cts.iSelMDK)	'ZセンサID
				.p_iZPosition	= .p_mltcp.ZeroPos_Z					'ゼロ点位置(測定値)[um]
				.p_dWeightCoef	= p_dNConv								'Z重量換算値
			End With
			'フィルタ動作設定をセット
			frmMain.SetFilterSetting()

			'測定時の軸動作モード
			p_RTM(0).p_FlcvActMode = If(rbCircleAction.Checked, 0, 1)
			'測定条件チェック
			Select Case p_iMeasMode
			Case eViscoMeaure.SpeedDep
			'速度依存
				'振幅ステップ計算
				p_RTM(0).p_FlcvReverse		= ckbFlcvReverse.Checked							'FLCV 逆行測定 有効無効
				Dim dStart		As Double = Double.Parse(nudSpeedDepAmpStart.Value)
				Dim dEnd		As Double = Double.Parse(nudSpeedDepAmpEnd.Value)
				Dim dStep		As Double = Double.Parse(nudSpeedDepAmpStep.Value)
				Dim dStepCount	As Double = ((Math.Abs(dEnd - dStart) / dStep)) + 1
				Dim iStepCount	As Int32  = Math.Round(dStepCount,0)
				p_RTM(0).p_FlcvChgVal = Nothing
				Array.Resize( p_RTM(0).p_FlcvChgVal, iStepCount * If(p_RTM(0).p_FlcvReverse,2,1) )				'測定振幅配列確保
				Array.Resize( p_RTM(0).p_FlcvZero, iStepCount * If(p_RTM(0).p_FlcvReverse,2,1) )			'ゼロストレス配列確保
				if dStart >= dEnd 
				'振幅マイナス方向
					For i = 0 To iStepCount - 1
						p_RTM(0).p_FlcvChgVal(i) = Double.Parse(dStart - (dStep * i))
					Next i
					if p_RTM(0).p_FlcvReverse
						j = iStepCount - 1
						For i = iStepCount To p_RTM(0).p_FlcvChgVal.GetUpperBound(0)
							p_RTM(0).p_FlcvChgVal(i) = p_RTM(0).p_FlcvChgVal(j)
							j -= 1
						Next i
					End If
				Else
				'振幅プラス方向
					For i = 0 To iStepCount - 1
						p_RTM(0).p_FlcvChgVal(i) = Double.Parse(dStart + (dStep * i))
					Next i
					if p_RTM(0).p_FlcvReverse
						j = iStepCount - 1
						For i = dStepCount To p_RTM(0).p_FlcvChgVal.GetUpperBound(0)
							p_RTM(0).p_FlcvChgVal(i) = p_RTM(0).p_FlcvChgVal(j)
							j -= 1
						Next i
					End If
				End If
				p_RTM(0).p_FlcvStart		= Double.Parse(nudSpeedDepAmpStart.Value)			'設定開始をセット
				p_RTM(0).p_FlcvEnd			= Double.Parse(nudSpeedDepAmpEnd.Value)				'設定終了をセット
				p_RTM(0).p_FlcvStep			= Double.Parse(nudSpeedDepAmpStep.Value)			'設定Stepをセット
				p_RTM(0).p_FlcvPeriod		= Double.Parse(nudSpeedDepPeriod.Value)				'設定周期をセット

				'せん断速度(横軸)スケール計算
				Dim uVp As DataFile.ViscosityCalcParameter
				uVp.dPeriod			= p_RTM(0).p_FlcvPeriod									'設定周期
				uVp.dMeasGap		= p_RTM(0).p_mltcp.GapPreset(1) / 10^3					'測定GAP(m)
				uVp.dAmplitude		= p_RTM(0).p_FlcvChgVal.Max / 10^3						'設定振幅(m)
				uVp.dVelocityRate	= 1.0												'せん断速度補正係数
				datCal.ViscosityCalc( uVp )
				m_dFlcvXAxisMax		=  uVp.dShearVelocity								'せん断速度MAX

				uVp.dAmplitude		= p_RTM(0).p_FlcvChgVal.Min / 10^3						'設定振幅(m)
				datCal.ViscosityCalc( uVp )
				m_dFlcvXAxisMin		=  uVp.dShearVelocity								'せん断速度MIN
				p_RTM(0).p_FlcvSample	= nudSpeedDepSampleTime.Value * MEAS_BLOCK_DATA_NUM	'サンプル数
				p_RTM(0).p_FlcvAveSizeRate	= Double.Parse(nudSpeedDepAcquisitionTime.Value)'適用率(%)

			Case eViscoMeaure.TimeDep
			'時間依存
#If 0
				p_RTM(0).p_FlcvChgVal = Nothing
				Array.Resize(p_RTM(0).p_FlcvChgVal, 1)
				p_RTM(0).p_FlcvChgVal(0)	= Double.Parse(nudTimeDepAmp.Value)					'設定振幅をセット
				p_RTM(0).p_FlcvAmplitude	= Double.Parse(nudTimeDepAmp.Value)
				p_RTM(0).p_FlcvPeriod		= Double.Parse(nudTimeDepPeriod.Value)				'設定周期をセット
				p_RTM(0).p_FlcvOpeTime		= Double.Parse(nudTimeDepOpeTime.Value)				'設定動作時間をセット
				p_RTM(0).p_FlcvRstTime		= Double.Parse(nudTimeDepRstTime.Value)				'設定停止時間をセット
				p_RTM(0).p_FlcvCycle		= Int32.Parse(nudTimeDepCycle.Value)				'設定繰返し回数をセット
				'測定条件のゼロ/フルストレスを得る
				Dim iRet As Int32
				iRet = modFrmMain.SetFullStress(p_RTM(0).p_FlcvLow1Amplitude, p_RTM(0).p_FlcvPeriod, p_RTM(0).p_FlcvActMode)
				if iRet = -1 OrElse iRet = -3
					modFrmMain.PreMesureOperationalGuideMsgBox(frmMain, 0)
					Return
				End If
#else
				p_RTM(0).p_FlcvLow1Amplitude		= Double.Parse(nudLow1Amp.Value) 					'FLCV 設定 初期化低速 動作振幅(mm)<時間依存>
				p_RTM(0).p_FlcvLow2Amplitude		= Double.Parse(nudLow2Amp.Value)		            'FLCV 設定 経過観察低速 動作振幅(mm)<時間依存>
				p_RTM(0).p_FlcvHighAmplitude		= Double.Parse(nudHighAmp.Value)		            'FLCV 設定 構造破壊高速 動作振幅(mm)<時間依存>
				p_RTM(0).p_FlcvPreshearAmplitude	= Double.Parse(nudPreShearAmp.Value)	            'FLCV 設定 プリシア高速	動作振幅(mm)<時間依存>
				p_RTM(0).p_FlcvRecoveryAmplitude	= Double.Parse(nudRecoveryAmp.Value)	            'FLCV 設定 回復動作	動作振幅(mm)<時間依存>
				p_RTM(0).p_FlcvLow1Time				= Double.Parse(nudLow1Time.Value)		            'FLCV 設定 初期化低速 動作時間(秒)<時間依存>
				p_RTM(0).p_FlcvLow2Time				= Double.Parse(nudLow2Time.Value)		            'FLCV 設定 経過観察低速 動作時間(秒)<時間依存>
				p_RTM(0).p_FlcvHighTime				= Double.Parse(nudHighTime.Value)		            'FLCV 設定 構造破壊高速 動作時間(秒)<時間依存>
				p_RTM(0).p_FlcvPreshearTime			= Double.Parse(nudPreShearTime.Value)	            'FLCV 設定 プリシア高速 動作時間(秒)<時間依存>
				p_RTM(0).p_FlcvRecoveryTime			= Double.Parse(nudRecoveryTime.Value)	            'FLCV 設定 回復動作 動作時間(秒)<時間依存>
				p_RTM(0).p_FlcvThinning				= Int32.Parse(nudThinning.Value)					'FLCV 設定 グラフ間引き<時間依存>
				p_RTM(0).p_FlcvPeriod				= Double.Parse(nudTimeDepPeriod.Value)				'FLCV 設定 グラフ間引き<時間依存>

				p_RTM(0).p_mltcp.AmplitudeX	= p_RTM(0).p_FlcvPreshearAmplitude * 1000.0											'X軸振幅
				p_RTM(0).p_mltcp.AmplitudeY	= If(p_RTM(0).p_FlcvActMode=0, p_RTM(0).p_FlcvPreshearAmplitude * 1000.0, 0)		'Y軸振幅
				p_RTM(0).p_mltcp.AmplitudeZ = 0
				p_RTM(0).p_mltcp.PeriodX(1)	= p_RTM(0).p_FlcvPeriod												'X軸周期
				p_RTM(0).p_mltcp.PeriodY(1)	= p_RTM(0).p_FlcvPeriod												'Y軸周期
				p_RTM(0).p_mltcp.PeriodZ(1)	= 0

#End If
				m_dFlcvXAxisMax					=	p_RTM(0).p_mltcp.MesPreWait(1) + _
													p_RTM(0).p_mltcp.MeasWaitTime(1) +
													p_RTM(0).p_FlcvLow1Time + _
													p_RTM(0).p_FlcvLow2Time + _
													p_RTM(0).p_FlcvHighTime + _
													p_RTM(0).p_FlcvPreshearTime + _
													p_RTM(0).p_FlcvRecoveryTime



			Case eViscoMeaure.TempDep
			'温度依存
				'温度ステップ計算
				p_RTM(0).p_FlcvChgVal = Nothing
				Dim dStart		As Double = Double.Parse(nudTempDepTempStart.Value)
				Dim dEnd		As Double = Double.Parse(nudTempDepTempEnd.Value)
				Dim dStep		As Double = Double.Parse(nudTempDepTempStep.Value)
				Dim dStepCount	As Double = ((Math.Abs(dEnd - dStart) / dStep)) + 1
				Dim iStepCount	As Int32  = Math.Round(dStepCount,0)
				Array.Resize( p_RTM(0).p_FlcvChgVal, iStepCount * If(p_RTM(0).p_FlcvReverse,2,1) )	'測定温度配列確保
				Array.Resize( p_RTM(0).p_FlcvZero, iStepCount * If(p_RTM(0).p_FlcvReverse,2,1) )	'ゼロストレス配列確保

				For i = 0 To iStepCount - 1
					p_RTM(0).p_FlcvChgVal(i) = Double.Parse(dStart + (dStep * i))
				Next i
				p_RTM(0).p_FlcvPeriod		= Double.Parse(nudTempDepPeriod.Value)				'設定周期をセット
				p_RTM(0).p_FlcvAmplitude	= Double.Parse(nudTempDepAmp.Value)					'設定振幅をセット

				'温度(横軸)スケール計算
				m_dFlcvXAxisMax =  nudTempDepTempEnd.Value							'温度MAX
				m_dFlcvXAxisMin =  0												'温度MIN
				p_RTM(0).p_FlcvStart		= Double.Parse(nudTempDepTempStart.Value)			'設定開始をセット
				p_RTM(0).p_FlcvEnd			= Double.Parse(nudTempDepTempEnd.Value)				'設定終了をセット
				p_RTM(0).p_FlcvStep			= Double.Parse(nudTempDepTempStep.Value)			'設定Stepをセット
				p_RTM(0).p_FlcvSample		= nudTempDepSampleTime.Value * MEAS_BLOCK_DATA_NUM	'サンプル数
				p_RTM(0).p_FlcvAveSizeRate	= Double.Parse(nudTempDepAcquisitionTime.Value)		'適用率(%)

			End Select

			p_RTM(0).p_FlcvPbDiameter	= Double.Parse(nudProbedDiameter.Value)						'プローブ直径(mm)

			p_RTM(0).p_FlcvMeasMode		= p_iMeasMode                                              '測定モードを設定

			'全終了予測日時を計算
			Select Case p_iMeasMode
				Case eViscoMeaure.SpeedDep, eViscoMeaure.TempDep
					m_iTotalSec = 0
					For i = 0 To p_RTM(0).p_FlcvChgVal.GetUpperBound(0)
						m_iTotalSec += p_RTM(0).p_FlcvSample / MEAS_BLOCK_DATA_NUM + If(p_iMeasMode = eViscoMeaure.SpeedDep, 3, 30)
					Next i
				Case eViscoMeaure.TimeDep
					m_iTotalSec = m_dFlcvXAxisMax
			End Select
			m_iTotalSec = m_iTotalSec + Double.Parse(nudRisingHPWait.Value) + Double.Parse(nudXYWait.Value) + p_cts.SampleInputWait	'熱板上昇待ち時間 + XY動作待ち時間 + サンプル投入待ち時間
			Dim dNow As Date = Now											'現在日時取得
			Dim dNeedDate As Datetime = New DateTime( m_iTotalSec * 1e+7 )	'所要日時
			Dim dZeroDate As DateTime = New DateTime( 0 )					'ゼロ日時
			Dim tsInterval As TimeSpan = dNeedDate - dZeroDate				'TimeSpan型変換
			m_dEndPrediction = dNow.Add(tsInterval)							'終了予測日時

			sMsg = m_sStr(eMes.iMES_ENDQ)
			sMsg = sMsg.Replace("#YMD#", m_dEndPrediction.ToString("yyyy/MM/dd HH:mm:ss"))
			fMsg = New frmMSG(sMsg, {m_sStr(eMes.iMES_YES), m_sStr(eMes.iMES_NO)}, title, Me)
			fMsg.lblMsg.Font = New Font(BASE_FONT, 12)
			fMsg.Width = 500
			fMsg.Height = 200
			fMsg.StartPosition = FormStartPosition.CenterScreen
			fMsg.ShowDialog()

			Select Case fMsg.p_iRes
			Case -1, 2	'[×]か[いいえ]を選択
			Case Else
				'FLCV測定開始
				p_RTM(0).p_FlcvPreTemp	= nudPreTemp.Value					'較正中の温度設定
				btnStop.Enabled		= True
				p_RTM(0).iViscoState	= eViscoState.COND_CONF
				p_ZeroJob = 9				'ゼロ調モニタで終了処理へ
				Me.Panel2.Visible = false

				'共通の本体条件を最終送信 --------------------------------
				'入力GAP
				p_RTM(0).p_mltcp.InpGap(1)		=  double.parse(nudInputGap.Value)
				'GAP
				p_RTM(0).p_mltcp.GapPreset(1)	=  double.parse(nudMeasureGap.Value)
				'XY動作待ち時間
				p_RTM(0).p_mltcp.MesPreWait(1)	=  double.parse(nudXYWait.Value)
				'熱板上昇待ち時間
				p_RTM(0).p_mltcp.MeasWaitTime(1) =  double.parse(nudRisingHPWait.Value)
				'---------------------------------------------------

				Select Case p_iMeasMode
				Case eViscoMeaure.SpeedDep
					Call WriteLog("フローカーブ 測定開始 速度依存 " & System.Reflection.MethodBase.GetCurrentMethod.Name, eLOG_LEVEL.iINFO)
					p_RTM(0).p_iBGWOrder	= BGW_ORDER.FLCV_START
				Case eViscoMeaure.TimeDep
					Call WriteLog("フローカーブ 測定開始 時間依存 " & System.Reflection.MethodBase.GetCurrentMethod.Name, eLOG_LEVEL.iINFO)
					p_RTM(0).p_iBGWOrder	= BGW_ORDER.FLCV_START_BUTTON
				Case eViscoMeaure.TempDep
					Call WriteLog("フローカーブ 測定開始 温度依存 " & System.Reflection.MethodBase.GetCurrentMethod.Name, eLOG_LEVEL.iINFO)
					p_RTM(0).p_iBGWOrder	= BGW_ORDER.FLCV_START
				End Select
			End Select
			fMsg.Dispose()

		Else
		End if
	End If
		dPreXData = 0
		dPreYData = 0
		dPreShearMod = 0
		dPreXPosPhase = 0
		dPreXLossTan = 0

Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub
#If 0
''' <summary>
''' 測定条件タブ変更
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub tcMeasCond_SelectedIndexChanged(sender As Object, e As EventArgs) 
	p_iMeasMode = tcMeasCond.SelectedIndex

	Dim uVp As DataFile.ViscosityCalcParameter
	VpClear(uVp)
	'グラフ初期表示
	Select Case p_iMeasMode
	Case eViscoMeaure.SpeedDep
	'速度依存
		cbxY2Select.Enabled = True
		cbxXSelect.Enabled  = True

		cbxY1Select.SelectedIndex = eClm.iViscosity
		cbxY2Select.SelectedIndex = eClm.iShearStress
		cbxXSelect.SelectedIndex  = eClm.iShearVelocity

		ChartA.Height = 450
		If dtSpeedDep.Rows.Count = 0 
			SpeedDepTableAddRow(uVp)
		End If
		m_dFlcvXAxisMax = 100
		m_dFlcvXAxisMin = 0
		p_RTM(0).p_FlcvMeasMode = eViscoMeaure.SpeedDep
		ChartDisp1(False, ChartA, ds)

	Case eViscoMeaure.TempDep
	'温度依存
		cbxY2Select.Enabled = True
		cbxXSelect.Enabled  = True

		cbxY1Select.SelectedIndex = eClm.iViscosity
		cbxY2Select.SelectedIndex = eClm.iStress
		cbxXSelect.SelectedIndex  = eClm.iHPTemp

		ChartA.Height = 450
		If dtTempDep.Rows.Count = 0 
			TempDepTableAddRow(uVp)
		End If
		m_dFlcvXAxisMax = 100
		m_dFlcvXAxisMin = 0
		p_RTM(0).p_FlcvMeasMode = eViscoMeaure.TempDep
		ChartDisp1(False, ChartA, ds)

	Case eViscoMeaure.TimeDep
	'時間依存
		cbxY2Select.Enabled = False
		cbxXSelect.Enabled  = False

		cbxY1Select.SelectedIndex = eClm.iViscosity

		ChartA.Height = 450
		If dtTimeDep.Rows.Count = 0 
			TimeDepTableAddRow(1)
		End If
		m_dFlcvXAxisMax = 100
		m_dFlcvXAxisMin = 0
		p_RTM(0).p_FlcvMeasMode = eViscoMeaure.TimeDep
		m_dFlcvXAxisMax					=	p_RTM(0).p_mltcp.MesPreWait(1) + _
											p_RTM(0).p_mltcp.MeasWaitTime(1) +
											p_RTM(0).p_FlcvLow1Amplitude + _
											p_RTM(0).p_FlcvLow2Amplitude + _
											p_RTM(0).p_FlcvLow2Amplitude
		ViscosityChartDispT(False, ChartA, ds)								'粘度チャート更新


	End Select


End Sub
#end if

''' <summary>
''' 温度依存開始温度
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub nudTempDepTempStart_ValueChanged(sender As Object, e As EventArgs) Handles nudTempDepTempStart.ValueChanged
	If IsNothing(p_RTM(0))
		Exit Sub
	End If
	p_RTM(0).p_mltcp.TempPreset(1)	= double.parse(nudTempDepTempStart.Value)
	p_RTM(0).p_mltcp.Temp2Preset(1) = double.parse(nudTempDepTempStart.Value)
	p_RTM(0).p_mltcp.Temp3Preset(1) = p_clSpec.SGTempGet(p_RTM(0).p_mltcp.TempPreset(1))
End Sub

''' <summary>
''' Functionキー対応機能
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub frmViscosity_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
	Select Case e.KeyCode
	Case 112		'F1キー
		btnTemperature.PerformClick()		'温度
	Case 113		'F2キー
		btnZeroAdj.PerformClick()			'ゼロ調
	Case 114		'F3キー
		If p_RTM(0).p_iBGWOrder = BGW_ORDER.NO_ORDER
			btnExec.PerformClick()			'測定
		Else
			btnStop.PerformClick()			'停止
		End If
	Case 115		'F4キー
		sbReset()							'リセット
	Case 116		'F5キー
		sbOrigin()							'原点復帰
	Case 121		'F10キー				'LED照明切替
		Select Case p_RTM(0).p_mltcp.LedOnOff
		Case 0
			p_RTM(0).p_mltcp.LedOnOff = 1

		Case 1
			p_RTM(0).p_mltcp.LedOnOff = 0
		End Select
	End Select
	If (e.Modifiers And Keys.Control) = Keys.Control Then
		If e.KeyCode = Keys.K Then
			frmDebug.Show()
		End If
	End If

End Sub

''' <summary>
''' エラーリセットボタン
''' </summary>
Private Sub sbReset()
Try
    Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name, eLOG_LEVEL.iINFO)
	If p_RTM(0).p_iBGWOrder = BGW_ORDER.NO_ORDER
		p_RTM(0).p_iBGWOrder = BGW_ORDER.RESET_ERROR_REP
		End If
Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub

''' <summary>
''' 原点復帰指示
''' </summary>
Private Sub sbOrigin()
Try
    Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name, eLOG_LEVEL.iINFO)
	If p_RTM(0).p_iBGWOrder = BGW_ORDER.NO_ORDER
		p_RTM(0).p_iBGWOrder = BGW_ORDER.ORG_BUTTON_REP
	End If
Catch ex As Exception
    Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
    Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub

'軸選択コンボボックス
Private Sub cbxY1Select_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbxY1Select.SelectedIndexChanged, cbxY2Select.SelectedIndexChanged, cbxXSelect.SelectedIndexChanged
Try
	If timMain.Enabled 
		Select Case sender.Name
		Case "cbxY1Select"
			iY1Sel = cbxY1Select.SelectedIndex
			Call WriteLog("フローカーブ Y1軸 変更 " & sDispItemName(iY1Sel), eLOG_LEVEL.iINFO)
		Case "cbxY2Select"
			iY2Sel = cbxY2Select.SelectedIndex
			Call WriteLog("フローカーブ Y2軸 変更 " & sDispItemName(iY2Sel), eLOG_LEVEL.iINFO)
		Case "cbxXSelect"
			iXSel = cbxXSelect.SelectedIndex
			Call WriteLog("フローカーブ X軸 変更 " & sDispItemName(iXSel), eLOG_LEVEL.iINFO)
		End Select

		cbxY1Select.BackColor = Control.DefaultBackColor
		cbxY2Select.BackColor = Control.DefaultBackColor
		cbxXSelect.BackColor = Control.DefaultBackColor
		Select Case p_iMeasMode 
		Case eViscoMeaure.TimeDep
			ViscosityChartDispT(False, ChartA, ds)
		Case Else
			If iY1Sel = iY2Sel
				cbxY1Select.BackColor = Color.Yellow
				cbxY2Select.BackColor = Color.Yellow
			ElseIf iY1Sel = iXSel
				cbxY1Select.BackColor = Color.Yellow
				cbxXSelect.BackColor = Color.Yellow
			ElseIf iY2Sel = iXSel
				cbxY2Select.BackColor = Color.Yellow
				cbxXSelect.BackColor = Color.Yellow
			Else
				ChartDisp1(False, ChartA, ds)
			End If
		End Select
	End If
Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub
'StrageMod-ElasticModグラフスケール設定表示
Private Sub btnScale_Click(sender As Object	, e As EventArgs) Handles btnScale.Click
	gbxStrgElastGScaleSetting.Visible = Not gbxStrgElastGScaleSetting.Visible
End Sub
Dim dStgElaGXMax		As Double
Dim dStgElaGXMin		As Double
Dim dStgElaGYMax		As Double
Dim dStgElaGYMin		As Double
Dim dStgElaGXInterval	As Double
Dim dStgElaGYInterval	As Double
'StrageMod-ElasticModグラフスケール設定 適用ボタン
Private Sub btnStrgElastGScaleAply_Click(sender As Object, e As EventArgs) Handles btnStrgElastGScaleAply.Click
	Dim bJudge As Boolean = True
	'最大最小の大小チェック
	If nudStgElaGXMax.Value <=	nudStgElaGXMin.Value
		nudStgElaGXMax.BackColor = Color.Yellow
		nudStgElaGXMin.BackColor = Color.Yellow
		bJudge = False
	Else
		nudStgElaGXMax.BackColor = Color.White
		nudStgElaGXMin.BackColor = Color.White
	End If
	If nudStgElaGYMax.Value <=	nudStgElaGYMin.Value
		nudStgElaGYMax.BackColor = Color.Yellow
		nudStgElaGYMin.BackColor = Color.Yellow
		bJudge = False
	Else
		nudStgElaGYMax.BackColor = Color.White
		nudStgElaGYMin.BackColor = Color.White
	End If
	'インターバルの大きさチェック
	If nudStgElaGXInterval.Value > Math.Abs(nudStgElaGXMax.Value-nudStgElaGXMin.Value)
		nudStgElaGXInterval.BackColor = Color.Yellow
		bJudge = False
	Else
		nudStgElaGXInterval.BackColor = Color.White
	End If
	If nudStgElaGYInterval.Value > Math.Abs(nudStgElaGYMax.Value-nudStgElaGYMin.Value)
		nudStgElaGYInterval.BackColor = Color.Yellow
		bJudge = False
	Else
		nudStgElaGYInterval.BackColor = Color.White
	End If
	If bJudge 
		StrgElastGAxisParaSet(1)
		ChartB.ChartAreas(0).AxisX.Maximum = dStgElaGXMax
		ChartB.ChartAreas(0).AxisX.Minimum = dStgElaGXMin
		ChartB.ChartAreas(0).AxisY.Maximum = dStgElaGYMax
		ChartB.ChartAreas(0).AxisY.Minimum = dStgElaGYMin
		ChartB.ChartAreas(0).AxisX.Interval= dStgElaGXInterval
		ChartB.ChartAreas(0).AxisY.Interval= dStgElaGYInterval
		gbxStrgElastGScaleSetting.Visible = False
		'ChartDisp2(False, ChartB, ds)													'フローカーブ モニタグラフ1
	End If
End Sub
Private Sub StrgElastGAxisParaSet(ByVal iDir As Int16)
	Select Case iDir
	Case 0
		nudStgElaGXInterval.Value	= dStgElaGXInterval
		nudStgElaGYInterval.Value	= dStgElaGYInterval
		nudStgElaGXMax.Value		= dStgElaGXMax
		nudStgElaGXMin.Value		= dStgElaGXMin
		nudStgElaGYMax.Value		= dStgElaGYMax
		nudStgElaGYMin.Value		= dStgElaGYMin
	Case 1
		dStgElaGXMax		= nudStgElaGXMax.Value
		dStgElaGXMin		= nudStgElaGXMin.Value
		dStgElaGYMax		= nudStgElaGYMax.Value
		dStgElaGYMin		= nudStgElaGYMin.Value
		dStgElaGXInterval	= nudStgElaGXInterval.Value
		dStgElaGYInterval	= nudStgElaGYInterval.Value
	End Select
End Sub

Private Sub frmViscosity_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
	p_RTM(0).p_mltcp.StartTempCont = 0		'温度指令をOFF
	p_RTM(0).p_mltcp.StartSW = 0			'ゼロ調指令をOFF
	p_RTM(0).p_iZeroAdjOk = 0               'ゼロ調正完了OFF
End Sub
'測定モード 切替えと詳細設定表示
Private Sub Button1_Click(sender As Object, e As EventArgs) Handles btnSpeed.Click, btnTime.Click, btnTemp.Click
	Dim ptLoc As New Point(1242, 508)
	gbxConditionSetSpeed.Location = ptLoc
	gbxConditionSetTime.Location = ptLoc
	gbxConditionSetTemp.Location = ptLoc
	btnSpeed.BackColor = Control.DefaultBackColor
	btnTemp.BackColor = Control.DefaultBackColor
	btnTime.BackColor = Control.DefaultBackColor
	gbxConditionSetSpeed.Visible = False
	gbxConditionSetTime.Visible = False	
	gbxConditionSetTemp.Visible = False	

	Dim uVp As DataFile.ViscosityCalcParameter
	VpClear(uVp)
	Select Case sender.Name
	Case "btnSpeed"
		If p_iMeasMode = eViscoMeaure.SpeedDep
			gbxConditionSetSpeed.Visible = True
			gbxConditionSetTemp.Visible = False
			gbxConditionSetTime.Visible = False
		End If
	    p_iMeasMode = eViscoMeaure.SpeedDep
		Sender.BackColor = Color.LightYellow
	'速度依存
		cbxY2Select.Enabled = True
		cbxXSelect.Enabled  = True

		cbxY1Select.SelectedIndex = eClm.iViscosity
		cbxY2Select.SelectedIndex = eClm.iShearStress
		cbxXSelect.SelectedIndex  = eClm.iShearVelocity

		ChartA.Height = 450
		If dtSpeedDep.Rows.Count = 0 
			SpeedDepTableAddRow(uVp)
		End If
		m_dFlcvXAxisMax = 100
		m_dFlcvXAxisMin = 0
		p_RTM(0).p_FlcvMeasMode = eViscoMeaure.SpeedDep
		ChartDisp1(False, ChartA, ds)
	Case "btnTime"
		If p_iMeasMode = eViscoMeaure.TimeDep
			gbxConditionSetSpeed.Visible = False
			gbxConditionSetTemp.Visible = False
			gbxConditionSetTime.Visible = True
		End If
	    p_iMeasMode = eViscoMeaure.TimeDep
		sender.BackColor = Color.LightCyan
	'時間依存
		cbxY2Select.Enabled = False
		cbxXSelect.Enabled  = False

		cbxY1Select.SelectedIndex = eClm.iViscosity

		ChartA.Height = 450
		If dtTimeDep.Rows.Count = 0 
			TimeDepTableAddRow(1)
		End If
		m_dFlcvXAxisMax = 100
		m_dFlcvXAxisMin = 0
		p_RTM(0).p_FlcvMeasMode = eViscoMeaure.TimeDep
		m_dFlcvXAxisMax = p_RTM(0).p_mltcp.MesPreWait(1) +
											p_RTM(0).p_mltcp.MeasWaitTime(1) +
											p_RTM(0).p_FlcvLow1Time +
											p_RTM(0).p_FlcvLow2Time +
											p_RTM(0).p_FlcvHighTime +
											p_RTM(0).p_FlcvPreshearAmplitude +
											p_RTM(0).p_FlcvRecoveryAmplitude
		ViscosityChartDispT(False, ChartA, ds)                              '粘度チャート更新
	Case "btnTemp"
		If p_iMeasMode = eViscoMeaure.TempDep
			gbxConditionSetSpeed.Visible = False
			gbxConditionSetTemp.Visible = True
			gbxConditionSetTime.Visible = False
		End If
	    p_iMeasMode = eViscoMeaure.TempDep
		sender.BackColor = Color.PeachPuff
	'温度依存
		cbxY2Select.Enabled = True
		cbxXSelect.Enabled  = True

		cbxY1Select.SelectedIndex = eClm.iViscosity
		cbxY2Select.SelectedIndex = eClm.iStress
		cbxXSelect.SelectedIndex  = eClm.iHPTemp

		ChartA.Height = 450
		If dtTempDep.Rows.Count = 0 
			TempDepTableAddRow(uVp)
		End If
		m_dFlcvXAxisMax = 100
		m_dFlcvXAxisMin = 0
		p_RTM(0).p_FlcvMeasMode = eViscoMeaure.TempDep
		ChartDisp1(False, ChartA, ds)
	End Select
End Sub

Private Sub btnCloseTemp_Click(sender As Object, e As EventArgs) Handles btnCloseTemp.Click, btnCloseSpeed.Click, btnCloseTime.Click
	gbxConditionSetTemp.Visible = False
	gbxConditionSetSpeed.Visible = False
	gbxConditionSetTime.Visible = False
End Sub
End Class