'Imports System.Collections
Imports System.ComponentModel
'Imports System.Drawing
Imports System.Drawing.Printing
Imports System.Globalization
Imports System.IO
'Imports System.Runtime.CompilerServices
'Imports System.Threading
Imports System.Windows
'Imports System.Windows.Forms
Imports System.Windows.Forms.DataVisualization.Charting
Imports MathNet.Numerics.Statistics
'Imports Microsoft.VisualBasic.FileIO
Imports Microsoft.VisualBasic
'Imports System.Windows.Input
Imports Microsoft.VisualBasic.ApplicationServices
Imports QRCoder
Imports SZKTool.clSpec
Imports SZKTool.DataFile
Imports SZKTool.Filter
Imports SZKTool.frmAbout
Imports SZKTool.frmOptions
Imports SZKTool.modChart
Imports SZKTool.modPublic
Imports SZKTool.RealTimeMeas

Public Class frmMain
'▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼
'   構造体
'▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲


'▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼
'   パブリック変数
'▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲
''' <summary> Language File 文字列格納場所 </summary>
Public p_sMes(efrmMainSTR.iMAX - 1) As String
Public ReadOnly Property GetMultMes(ByVal iEnum As Int32) As String
	Get
		GetMultMes = p_sMes(iEnum)
	End Get
End Property

Public p_pdocPrint As PrintDocument             'LightPrint適用PrintDocument

'▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼
'   メンバ変数
'▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲
'''<summary> ログ出力用StringBuilder。未処理例外以外をCatchした時に出力。 </summary>
Private m_sbLog As System.Text.StringBuilder = New System.Text.StringBuilder()


'Private m_blBgwAlive_1			As Boolean = False   'スレッド生存中
'Private m_blBgwAlive_Online		As Boolean = False   'スレッド生存中
Private m_blBgwAlive_Interrupt	As Boolean = False   'スレッド生存中
Private m_blBgwAlive_Trend		As Boolean = False   'スレッド生存中
'Private m_blBgwAlive_ReCalc		As Boolean = False   'スレッド生存中
'Private m_blBgwAlive_Error		As Boolean = False   'スレッド生存中
'Private m_iOnline				As Integer = 0          '

Delegate sub ElapseDispDelegate()
Private Sub Label2Disp(byval value As string)
	Lbl2.Text = value
End Sub

'キー入力、マウス操作監視用
Private sw_KeyObs		As New Stopwatch 
Private	m_bIsPressKey	As Boolean = False
Private	m_iPreMousePosX As Int32
Private	m_iPreMousePosY As Int32

private bFrmNew			As Boolean = True					'コントロール初期化中

Private m_bQRCodeDragDropEna  As Boolean = True				'QRコードドラッグドロップ許可


'''<summary> 固定選択規格に対応する可変温度設定。 </summary>
Public	p_sSelectGItem	As String			'リアルタイムトレンドの項目選択
Public	p_iSelectGItem	As Int16			'リアルタイムトレンドの項目選択番号
'Private sChartPathName	As String			'チャート表示用ファイルパス
Public	m_HistDraw		As Boolean			'測定チャート履歴描画	有効/無効
Public	m_StdDraw		As Boolean			'測定チャート基準描画	有効/無効

Public m_sGuideMsgTitle	As String = "Operation guide"
Public Property MesBoxTitle() As String
	Get
		MesBoxTitle = m_sGuideMsgTitle
	End Get
    Set(value As String)
		m_sGuideMsgTitle = value
    End Set
End property


''' <summary>
''' ゲルタイム計算 外挿法クラス生成
''' </summary>
Public clExtra			As New Extra



Private Coefzx() As Double
Private Coefzy() As Double
Private Coefza() As Double
Private Coefzc() As Double

Private cbxEquipEn As Boolean = False		'装置IDコンボリスト使用有無
'■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
'   コンストラクタ／デストラクタ
'■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
Public Sub New()
	' この呼び出しはデザイナーで必要です。
	InitializeComponent()

	' InitializeComponent() 呼び出しの後で初期化を追加します。
End Sub
'''<summary> 例外の内容についてエラーログを出力する </summary>
Private Overloads Shared Sub MakeLogFile(ByVal e As Exception, ByVal strLog As String)
	Try
		'ログファイル作成
		Dim msg As String = "A running error has occurred." + vbCrLf + "We apologize for the inconvenience, but please report the following details to the manufacturer." + vbCrLf + vbCrLf + vbCrLf
		msg += "--- SIZUKA Tool " + My.Application.Info.Version.ToString() + " Log File " + Now.ToString() + " ---" + vbCrLf
		msg += e.ToString() + vbCrLf

		'REPORT_ERROR_MSG
		If e.InnerException IsNot Nothing Then
			msg += "ie:" + vbCrLf + e.InnerException.ToString()
		End If
		My.Computer.FileSystem.WriteAllText(strLog, msg & vbCrLf & vbCrLf, True)

	Catch ex As Exception
		Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	End Try
End Sub

''<summary> strTxtの内容についてstrLogのパスにログを出力する </summary>
Private Overloads Shared Sub MakeLogFile(ByRef strTxt As String, ByVal strLog As String)
	Try
		'ログファイル作成
		Dim msg As String
		msg = "--- SIZUKA Tool " + My.Application.Info.Version.ToString() + " Log File " + Now.ToString() + " ---" + vbCrLf
		msg += strTxt + vbCrLf
		My.Computer.FileSystem.WriteAllText(strLog, msg, False)
	Catch ex As Exception
		Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	End Try
End Sub

'''<summary> strTxtの内容についてログを出力する </summary>
Public Overloads Shared Sub MakeLogFile(ByRef strTxt As String)
	Try
		MakeLogFile(strTxt, SIZUKA_PATH & "\" & "Exlog" + Now.Hour.ToString() + "h.txt")
	Catch ex As Exception
		Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	End Try
End Sub
	'■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
	'   フォームのロード／アンロード／アクティブ
	'■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■

''' <summary>
''' メイン画面ロード
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub frmMain_Load(sender As Object, e As EventArgs) Handles MyBase.Load 
Try
	Dim iRet As Int32
	
	Me.Visible = False

	Call p_cts.LoadDeviceListINI()
	p_sEquipName = p_cts.strEquipName(p_cts.iSelMDK)			'共有 装置IDを取得
	modFrmMain.frmMainIni(m_sbLog)								'初期化

	'言語により表示を変更
	modPublic.SetFormLanguage(Me, p_iLanguage, ToolTipMain)

	Call p_cts.LoadDeviceSettingINI(p_sEquipName)				'装置毎設定読込
	iRet = p_cts.LoadTempParaINI(p_sEquipName)					'温調パラメータ読込
	iRet += p_cts.InputSelectionFileRead(p_sEquipName)			'サンプルSPEC入力選択リスト読込
	If iRet<>0
	'無ければ初期値で作成
		iRet = p_cts.InputSelectionFileSave(p_sEquipName)
	End If
	Me.Text &= " [" & My.Application.Info.Version.ToString & "]"

	'Spec入力項目関係
	p_clSpec.SpecLangageSet(p_sMes)
	'ゲルタイムチャート用にコピー ------
	ChartItemSet(p_sMes)
	sChartItemName = p_sMes(efrmMainSTR.CHART_DISP_ITEM).Split(",")		'ゲルタイムチャート項目
	sChartItemUnit = p_sMes(efrmMainSTR.CHART_DISP_UNIT).Split(",")		'ゲルタイムチャート項目単位
	'------------------------------------
	lstGelTimeSel = p_sMes(efrmMainSTR.EXT_GEL_CBX).Split(",").ToList()	'外挿法 ゲルタイム選択コンボボックス用リスト
	m_sGuideMsgTitle = p_sMes(efrmMainSTR.iOPE_GUIDE_TITLE)				'操作ガイドウィンドウ用共通タイトル

	'p_sEquipName = p_cts.strEquipName(p_cts.iSelMDK)            '本体IDをセット

	p_sbtnZeroText = btnZero.Text				'ボタン名の退避
	p_sbtnMeasText = btnMeasure.Text			'ボタン名の退避
''	p_sbtnOrgText = btnOrigin.Text				'ボタン名の退避
 
	'起動時に過去のログを削除
	Dim sLogPath As String = Path.Combine(SIZUKA_PATH, LOGDAT_PATH)
	'DeleteLog(sLogPath, "SizukaTool*.Log")
	DeleteLog(sLogPath, "Sizuka*.Log")
	DeleteLog(sLogPath, "Exlog*.txt")
	'ログフォルダ作成
	If System.IO.Directory.Exists(sLogPath) = False Then
		System.IO.Directory.CreateDirectory(sLogPath)
	End If
	Call WriteLog("==========================", eLOG_LEVEL.iINFO)
	Call WriteLog("SIZUKA-START (Ver." & My.Application.Info.Version.ToString & ")", eLOG_LEVEL.iINFO)
	Call WriteLog("==========================", eLOG_LEVEL.iINFO)
	m_sbLog.Append("Loading frmMain" + vbCrLf)

	'DeviceList.iniファイルが存在しない場合は、初期設定を行う
	If Not FileIO.FileSystem.FileExists(Path.Combine(My.Application.Info.DirectoryPath.ToString(), DevListFile))

        m_sbLog.Append("Opening frmWelcome" + vbCrLf)
        '初回起動画面を開く
        Dim frmw As frmWelcome = New frmWelcome()
        frmw.ShowDialog()
        p_cts.strIP(0)		= frmw.GetIP()			'IPアドレス
        p_cts.strPort		= frmw.GetPort()		'送受信ポート番号
        frmw.Dispose()
        m_sbLog.Append("frmWelcome Closed" + vbCrLf)

		'>>> 指定されたIPへ接続し製造番号を取得する
		Dim mltcp	As New MemLinkTCP				'メモリリンク通信を行うクラス
		Dim sErr	As String = Nothing
		Dim frmCon	As frmWaitMes = New frmWaitMes()
		frmCon.p_sMes = p_sMes(efrmMainSTR.CONNECTING)
		frmCon.Show()
		frmCon.Refresh

		If mltcp.TCP_Connect(p_cts.strIP(0), p_cts.strPort, sErr) Then
		'接続できた
			sErr = Nothing
			mltcp.p_strEquipIP = p_cts.strIP(0)													'内部IPアドレスをセット
			iRet = mltcp.ReadBatch(p_dtFile)													'[[[[一括読込(通常)]]]]
			If iRet = 0
			'受信成功時
				p_sEquipName			= mltcp.DevSerealRead									'機器製造番号を取得
			Else
			'受信失敗時
				p_sEquipName			= "999"													'初期値
			End If
			p_cts.strEquipName(p_cts.iSelMDK) = "SZK" & CInt(p_sEquipName).ToString("999")		'本体IDをフォーマット
			DeviceComboUpdate()																	'機器名称コンボボックスを設定
			mltcp.TCP_DisConnect()
			mltcp = Nothing
		Else
		'指定したIPアドレスの装置に接続できなかったが起動は継続
			MessageBox.Show(p_sMes(efrmMainSTR.INVALID_IP),  System.Windows.Forms.Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error)
		End If
		frmCon.Dispose()
		'DeviceList初期化ファイル保存
        p_cts.SaveAsDeviceListINI()							
	Else

    End If

	'多重起動を許可するか？
	If p_cts.MultiActivate = 0
		'多重起動をチェックする
		If System.Diagnostics.Process.GetProcessesByName( _
			System.Diagnostics.Process.GetCurrentProcess.ProcessName).Length > 1 Then
			'すでに起動していると判断する
			MessageBox.Show("Multiple activation is not possible.",  System.Windows.Forms.Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error)
			 System.Windows.Forms.Application.Exit()		'強制終了
		End If
	End If
	
	'p_sEquipName = p_cts.strEquipName(p_cts.iSelMDK)            '各設定フォルダ作成のために 本体IDをセット

	modFrmMain.SetForderCreate()											'データ管理フォルダ環境生成


	'機器名称コンボボックスを設定
	cboEquipName.Enabled = False
	DeviceComboUpdate()
	cboEquipName.Enabled = cbxEquipEn

	'測定結果 ゲルタイム を無効設定
	lblElapsedVal.Text = "N/A"                      
	lblElapsedVal.TextAlign = ContentAlignment.MiddleCenter

	'履歴描画設定
	m_HistDraw = tsmiHistoryDraw.Checked
	'基準値描画設定
	m_StdDraw = tsmiStandardDraw.Checked
	'Y軸スパン設定
	tsmiFixedY.Checked		= If(p_cts.iRTYAxis = 0, False, True)
	'X軸スパン設定
	tsmiFixedX.Checked		= If(p_cts.iRTXAxis = 0, False, True)
	'X軸チャートタイプ
	tsmiXAxisTemp.Checked	= If(p_cts.iChartType = 0, False, True)

	'グラフ精細度初期設定
	nudDispThinningoutRough.Value	= p_cts.iDetailsR
	nudDispThinningoutDetail.Value	= p_cts.iDetailsD
	
	'数値入力ボックス初期化 最小 最大 小数以下 インクリメント 初期値
	'グラフ設定 X軸
	sbNudInitial(nudXscaleMin, 0,			4000,		0, 1, 0)
	sbNudInitial(nudXscaleMax, 0,			4000,		0, 1, 1)
	sbNudInitial(nudXdivision, 1,			100,		0, 1, 10)
	'グラフ設定 Y軸
	sbNudInitial(nudYscaleMin, -99999999,	99999999,	3, 1, 0)
	sbNudInitial(nudYscaleMax, -99999999,	99999999,	3, 1, 1)
	sbNudInitial(nudYdivision, 1,			100,		0, 1, 10)
	'グラフ設定 間引き
	sbNudInitial(nudDispThinningoutRough, 100,			10000,		0, 1, 100)
	sbNudInitial(nudDispThinningoutDetail, 1,			10000,		0, 1, 10)
	'グラフ設定 スクロール幅
	sbNudInitial(nudXSpan,		1,			1000,		0, 1, 10)


	'操作ボタン有効無効

	'Offline状態の無効設定
	sbConnectEnable(False)
	sbEnableFunctions()


	'リアルタイム測定用クラスを初期化
	p_RTM		= New RealTimeMeas(0) {}
	p_RTM(0)	= New RealTimeMeas(Me)
	p_RTM(0).p_strDataFolder		= p_dtFile.DATA_STORE_PATH
	p_RTM(0).StartBGW()												'本体制御用スレッド開始
	p_RTM(0).p_strEquipIP			= p_cts.strIP(p_cts.iSelMDK)    '本体選択コンボのIPをセット
	p_RTM(0).p_iBGWWork				= BGW_WORK.AUTO_CONNECT         '自動接続制御開始
	p_RTM(0).p_dPosHeatBHome		= p_cts.HeatBHomePos()          '熱板待機位置をリアルタイムスレッドへ渡す
	p_RTM(0).p_mltcp.HeaterUpper(1) = HEATER_UPLMT_DEV				'熱板ヒーター上限値
	
	'応力検知パラメータ
	p_RTM(0).p_mltcp.StressDetectAcquisitionTime	= p_cts.StresDetAcqTime
	p_RTM(0).p_mltcp.StressDetectADThreshold		= p_cts.StresDetADThreshold
	p_RTM(0).p_mltcp.StressDetectDurationTime		= p_cts.StresDetDuraTime

	'接続直後は日付時刻設定
	p_RTM(0).p_iBGWOrder = BGW_ORDER.SETDATE_REP    '日付時刻設定制御へ

	'キャリブレーションファイル読込
	iRet = CalibFileRead()
	Select Case iRet
	Case 0		'全て読込みOK
	Case < 0	'読込み失敗
	'キャリブレーション値が確定されていないので正確な測定ができません。\r\n工場出荷時設定へ戻すかメーカへ問い合わせてください。
	'続行しますか? いいえを選択するとアプリケーションを終了します。
		If MsgBox(p_sMes(efrmMainSTR.iCALIBRATION_NOT_CONFIRMED) & vbCrLf & p_sMes(efrmMainSTR.iCONTINUE_CONFIRM), MsgBoxStyle.Critical+MsgBoxStyle.YesNo+MsgBoxStyle.DefaultButton2,  System.Windows.Forms.Application.ProductName) = MsgBoxResult.No
			 System.Windows.Forms.Application.Exit()		'強制終了
		End If
	End Select

	'メインタイマー処理開始
	timMain.Interval = 200
	timMain.Start()

	'状態表示タイマを開始
	timerShowState.Interval = 500
	timerShowState.Start()

#If 0
	'温度プロファイル制御スレッド開始
	If bgwInterrupt.IsBusy = False Then
		bgwInterrupt.RunWorkerAsync()                           
	End If
#end if

	'トレンド更新制御スレッド開始
	If bgwTrendUpdate.IsBusy = False Then
		bgwTrendUpdate.RunWorkerAsync()                         
	End If
#If 0
	'装置エラー画面制御スレッド開始
	If bgwErrorControl.IsBusy = False Then
		bgwErrorControl.RunWorkerAsync()                         
	End If
#end if

	'チャート初期表示
	rbCorrect.Checked = True
	p_sSelectGItem = rbCorrect.Name
	p_iSelectGItem = Array.IndexOf(sGraphSelectionArry, p_sSelectGItem)
	RealTimeThininingValueR = p_cts.iDetailsR
	RealTimeThininingValueD = p_cts.iDetailsD
	m_RealTrendUpdate = False                           'リアルタイムチャート初期化
	ChartDispUpdate(Me, 0, m_RealTrendUpdate)

	'数値モニタ初期表示(位置)
	gbxCurrentVal.Visible		= True
	pnlPosition.Visible			= True

	pnlCalculation.Visible		= False
	gbxCurrentVal.Controls.Add(pnlCalculation)
	pnlCalculation.Location		= pnlPosition.Location
	pnlCalculation.Width		= pnlPosition.Width

	pnlSteress.Visible			= False
	gbxCurrentVal.Controls.Add(pnlSteress)
	pnlSteress.Location			= pnlPosition.Location
	pnlSteress.Width			= pnlPosition.Width

	'X軸スクロール設定表示有無
	gpxScrol.Enabled = If(p_cts.iRTXAxis = 1, False, True)
	'測定データ情報ボタン消去
	''btnStdInfo.Visible = True
	'基準値データグリッドビュー消去
	gbxStdCond.Visible = False
	'基準値選択を1
	'rbStd1.Checked = True

	'7SegLED輝度 初期化
	Dim sSet(15) As String
	For i=0 To 15
		sSet(i) = i.ToString
	Next
	tsmiCmb7SegBright.Items.Clear
	tsmiCmb7SegBright.Items.AddRange(sSet)
	tsmiCmb7SegBright.SelectedIndex = p_cts.LED7SegBrightness


	'外挿法グループボックス初期化 ------------------------------------
	'ゲルタイム選択
	cboGelTSel.Items.Clear()
	cboGelTSel.Items.AddRange(lstGelTimeSel.ToArray)
	cboGelTSel.SelectedIndex = 0
	gbxExpoPad.Visible = False
	gbxExpoPad.Location = New System.Drawing.Point(890, 100)

	

	'スペック入力環境の初期化
	modFrmMain.SpecInputInit(Me)

	Dim FilSet As stFilterInfo = Nothing
	FilSet.iEnable = 1
	FilSet.sKind			= "FIR"				'フィルタ種類 FIR/IIR
	FilSet.dFIRCutOffFreq	= 0.4				'FIR カットオフ周波数
	FilSet.iFIRFilLen		= 51				'FIR パラメータ長
	FilSet.sFIRWinKind		= "Hamming"	        'FIR 窓関数種
	FilSet.dFIRSmplFreq		= 4					'FIR サンプル周波数

	p_zFilX.WindowFunction(FilSet, Coefzx)
	p_zFilY.WindowFunction(FilSet, Coefzy)

	FilSet.iEnable = 1
	FilSet.sKind			= "FIR"				'フィルタ種類 FIR/IIR
	FilSet.dFIRCutOffFreq	= 0.5				'FIR カットオフ周波数
	FilSet.iFIRFilLen		= 101				'FIR パラメータ長
	FilSet.sFIRWinKind		= "Hamming"	        'FIR 窓関数種
	FilSet.dFIRSmplFreq		= 4					'FIR サンプル周波数
	p_zFilA.WindowFunction(FilSet, Coefza)
	p_zFilC.WindowFunction(FilSet, Coefzc)




	'チャート軸のAuto表示制御
	lblXAuto.Visible = IF(p_cts.iRTXAxis=0,True,False)
	lblYAuto.Visible = IF(p_cts.iRTYAxis=0,True,False)


	'温度プロファイル確認と初期温度設定
	If dgvDevSpec.Item(2, eDevSPEC.TEMP_PROFILE).Value <> ""
		'設定された温度プロファイル
		Dim bRet As Boolean = p_RTM(0).sbLoadRTGSetting(dgvDevSpec.Item(2, eDevSPEC.TEMP_PROFILE).Value)
		If Not bRet
			dgvDevSpec.Item(2, eDevSPEC.TEMP_PROFILE).Value = ""
			p_RTM(0).p_strRTGSetName = ""
		Else
			'初期温度を設定
			p_RTM(0).p_strRTGSetName = dgvDevSpec.Item(2, eDevSPEC.TEMP_PROFILE).Value
			p_RTM(0).p_mltcp.TempPreset(1)	= p_RTM(0).p_rtgset.mdbr(0).dHPTemp
			p_RTM(0).p_mltcp.Temp2Preset(1) = p_RTM(0).p_rtgset.mdbr(0).dHPTemp
			p_RTM(0).p_mltcp.Temp3Preset(1) = p_clSpec.SGTempGet(p_RTM(0).p_rtgset.mdbr(0).dHPTemp)		'起歪体温度
		End If
	End If

	'管理者モード初期化
	AdminModeSwitch(1)

	'測定ボタン表示
	sbMeasureBottonStateDisp(Me)


	'スケール設定、GELTime言語ラベル表示
	sbScaleSetLangchange(p_cts)

	'スケール設定パッド初期値
	pnlThinning.Height = 259
	lblExpand.Text = "▽"



	Me.Visible	= True		'フォーム表示
	bFrmNew		= False		'コントロール初期化完了

Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Me.Visible	= True		'フォーム表示
	bFrmNew		= False		'コントロール初期化完了
End Try
End Sub
''' <summary>
''' キャリブレーションファイル群を読込む
''' </summary>
''' <returns>0:成功 -1:例外発生 -2:重量変換パラメータ読込失敗(初期値を設定) -3:ゼロキャリブレーションファイル読込失敗 -4:フルキャリブレーション読込失敗</returns>
Public Function CalibFileRead() As Int32
Try
	Dim iRet	As int32 = 0
	Dim iTRet	As int32 = 0
	'重量変換パラメータセット
	Dim frmC As New frmCalibWeight3
	iRet = frmC.WeightParameterFileGet(p_dNConv, p_dZConv, p_XYCoefMeasDate, p_ZCoefMeasDate)
	If iRet<>0 or p_dNConv = 0
		MessageBox.Show(p_sMes(efrmMainSTR.NOT_WEIGHT_CONV),  System.Windows.Forms.Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error)
		p_cts.WeightCoef	= 2.4E-6
		p_cts.WeightCoefZ	= 1
		iTRet = -2
	Else
		p_cts.WeightCoef	= p_dNConv
		p_cts.WeightCoefZ	= p_dZConv
	End If
	frmC.Dispose()

	'ZEROキャリブレーション結果読込
	Dim frmZ As New frmCalibFull(1)
	iRet = frmZ.sbFullStressCondRead(1, p_ZClbX, p_ZClbC, p_ZClbP)
	If iRet<>0
		MessageBox.Show(p_sMes(efrmMainSTR.NOT_ZERO_CALIB),  System.Windows.Forms.Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error)
		iTRet = -3
	End If
	frmZ.Dispose()

	'FULLキャリブレーション結果読込
	Dim frmF As New frmCalibFull(0)
	iRet = frmF.sbFullStressCondRead(0, p_FClbX, p_FClbC, p_FClbP)
	If iRet<>0
		MessageBox.Show(p_sMes(efrmMainSTR.NOT_FULL_CALIB),  System.Windows.Forms.Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error)
		iTRet = -4
	End If
	frmF.Dispose()
	Return iTRet
Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Return -1
End Try
End Function

''' <summary>
''' About表示
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub tsmiAbout_Click(sender As Object, e As EventArgs) Handles tsmiAbout.Click
Try
    Dim frmab As New frmAbout
    With frmab
        .ShowDialog()
        .Dispose()
    End With

Catch ex As Exception
    Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
    Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub
''' <summary>
''' 機器選択コンボボックス更新
''' </summary>
Public Sub DeviceComboUpdate()
	cboEquipName.Enabled = False
	Me.cboEquipName.SelectedIndex = p_cts.iSelMDK
	For i As Integer = 0 To 9
		Me.cboEquipName.Items(i) = p_cts.strEquipName(i)
	Next
	cboEquipName.Enabled = cbxEquipEn
End Sub

''' <summary>
''' クロージング
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub frmMain_Closing(sender As Object, e As CancelEventArgs) Handles MyBase.Closing
	
	UnderStatusLabelDisp(ToolStripStatusLabel1, p_sMes(efrmMainSTR.APPLI_CLOSING))
	''UnderStatusLabelDisp("The application is closing...")
	Me.Refresh()
	If bgwInterrupt.IsBusy Then
		'常時読込みスレッド停止
		bgwInterrupt.CancelAsync()
		Call WaitBgwEnd("bgwInterrupt", m_blBgwAlive_Interrupt)
		bgwInterrupt.Dispose()
	End If
	If bgwTrendUpdate.IsBusy Then
		'トレンド更新スレッド停止
		bgwTrendUpdate.CancelAsync
		'Call WaitBgwEnd("bgwTrendUpdate", m_blBgwAlive_Trend)		'なぜかキャンセルを受け付けないので強制終了
		bgwTrendUpdate.Dispose()
	End If
#If 0
	If bgwErrorControl.IsBusy Then
		'トレンド更新スレッド停止
		bgwErrorControl.CancelAsync
		'Call WaitBgwEnd("bgwTrendUpdate", m_blBgwAlive_Error)		'なぜかキャンセルを受け付けないので強制終了
		bgwErrorControl.Dispose()
	End If
#end if
End Sub
'フォームクロージング
Private Sub frmMain_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
	Dim sSpecName	As String = Nothing
	Dim iRet		As Int32
Try
	If cbxDevSPEC.Text = cFree
		sSpecName = cbxDevSPEC.Text
		p_clSpec.specWrite(eSpec.Device, Me, sSpecName)

	End If
	If cbxSamSPEC.Text = cFree
		sSpecName = cbxSamSPEC.Text
		p_clSpec.specWrite(eSpec.Sample, Me, sSpecName)

	End If

	'詳細設定の保存
	p_cts.SaveAsDeviceListINI()
	p_cts.SaveAsDeviceSettingINI(p_sEquipName)
	iRet = p_cts.SaveAsTempParaINI(p_sEquipName)
	If iRet<0
		MsgBox(p_sMes(efrmMainSTR.iTempParaWrtFail), MsgBoxStyle.Critical+MsgBoxStyle.OkOnly, System.Windows.Forms.Application.ProductName)
	End If

	'プログラム終了前の処理
	m_sbLog.AppendLine("Closing frmMain:" + Now.ToString())
	MakeLogFile(m_sbLog.ToString, "C:\SZKTool\Exlog_prev.txt")
	timMain.Stop()
	timerShowState.Stop()

	'バックグラウンド処理の終了
	p_RTM(0).CancelAndDisposeBGW()


	'積算値をログ出力
	modFrmMain.WriteIntegLog()
	WriteLog("----------------------SIZUKA Tool CLOSE" & vbCrLf & vbCrLf, eLOG_LEVEL.iINFO)

Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub
''' <summary>
''' スペック情報をRealTimeスレッドへ引き渡し
''' </summary>
Public Sub sbSpecToLinkArea()


	'p_clSpec.sbSpecToLinkArea( Me, dgvDevSpec )
	p_clSpec.sbSpecToLinkArea()


End Sub
'[Exit]の処理。プログラムを終了する
Private Sub tsExit_Click(sender As Object, e As EventArgs) Handles tsmiExit.Click
Try
	Me.Close()

Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub
Dim foptDlg As frmOptions
'[Options]の処理。ツールの設定を行う
Private Sub tsOptions_Click(sender As Object, e As EventArgs) Handles tsmiOptions.Click
Try
	Dim iPrevLang As Integer = p_cts.iLang
	Dim sPreMethod As String = p_cts.strDefDspMth
	'Summaryオプション
	Dim sPreSumYstDay As String = p_cts.sSumYstDay      '年間開始日（オプション画面を開く前の値）
	Dim sPreSumMstDay As String = p_cts.sSumMstDay      '月間開始日（オプション画面を開く前の値）
	'Ver.--- 詳細オプション本体設定
	Call WriteLog("詳細設定がクリックされました。", eLOG_LEVEL.iINFO)

	'本体設定を反映
	With p_cts
		'.iEachZeroAdj = p_RTM(0).p_iEachZeroAdj
		'.iRotaStartLate = p_RTM(0).p_iRotaStartLate
		'表示状態(本体との状態)：オンライン、新メモリ、インターロックの状態
		If p_RTM(0).p_iConnectState = CONNECT_ST.ONLINE Then
			'オンラインの場合
			If p_bInterRockEnable = True Then
				'インターロック有効
				'.iBodySetEnable = 1
				.iBodySetEnable = eMDK_BODY_TYPE.iINTERLOCK_ON
			Else
				'インターロック無効
				.iBodySetEnable = eMDK_BODY_TYPE.iINTERLOCK_OFF
			End If
		Else
			'オフライン状態：入力不可
			.iBodySetEnable = eMDK_BODY_TYPE.iOFFLINE
		End If
	End With
	'Dim foptDlg As frmOptions = New frmOptions(p_cts)
	foptDlg = New frmOptions(p_cts)

	'[OK]または[適用]が押された場合

	'オプションダイアログを開く
	foptDlg.ShowDialog()
	'	foptDlg.Activate()
	If foptDlg.p_bDlgApply = True Then
	Call WriteLog("詳細設定 適用/OKボタンがクリックされました。", eLOG_LEVEL.iINFO)
		'適用ボタンが押された
		cboEquipName.SelectedIndex = p_cts.iSelMDK
		p_RTM(0).p_strEquipIP = p_cts.strIP(p_cts.iSelMDK)
		p_RTM(0).p_strEquipPort = p_cts.strPort
		p_RTM(0).p_iAutoStart = p_cts.iAutoStart
		'熱板 温度制御 PID
		p_RTM(0).p_mltcp.P_PID(1) = p_cts.TempCnt_P
		p_RTM(0).p_mltcp.I_PID(1) = p_cts.TempCnt_I
		p_RTM(0).p_mltcp.D_PID(1) = p_cts.TempCnt_D
		p_RTM(0).p_mltcp.DGain(1) = p_cts.TempCnt_Dgain
		p_RTM(0).p_mltcp.PVBias(1) = p_cts.TempCnt_PVBias
		p_RTM(0).p_mltcp.PVGain(1) = p_cts.TempCnt_PVGain
		''p_RTM(0).p_mltcp.HeaterUpper(1) = p_cts.TempCnt_HUpperLimit
		p_RTM(0).p_mltcp.HeaterUpper(1) = HEATER_UPLMT_DEV					'ヒーター上限値
		'プローブ 温度制御 PID
		p_RTM(0).p_mltcp.P_PID2(1) = p_cts.TempCnt_P2
		p_RTM(0).p_mltcp.I_PID2(1) = p_cts.TempCnt_I2
		p_RTM(0).p_mltcp.D_PID2(1) = p_cts.TempCnt_D2
		p_RTM(0).p_mltcp.DGain2(1) = p_cts.TempCnt_Dgain2
		p_RTM(0).p_mltcp.PVBias2(1) = p_cts.TempCnt_PVBias2
		p_RTM(0).p_mltcp.PVGain2(1) = p_cts.TempCnt_PVGain2
		'p_RTM(0).p_mltcp.HeaterUpper2(1) = p_cts.TempCnt_HUpperLimit2
		p_RTM(0).p_mltcp.HeaterUpper2(1) = HEATER_UPLMT_DEV					'ヒーター上限値
		'起歪体 温度制御 PID
		p_RTM(0).p_mltcp.P_PID3(1) = p_cts.TempCnt_P3
		p_RTM(0).p_mltcp.I_PID3(1) = p_cts.TempCnt_I3
		p_RTM(0).p_mltcp.D_PID3(1) = p_cts.TempCnt_D3
		p_RTM(0).p_mltcp.DGain3(1) = p_cts.TempCnt_Dgain3
		p_RTM(0).p_mltcp.PVBias3(1) = p_cts.TempCnt_PVBias3
		p_RTM(0).p_mltcp.PVGain3(1) = p_cts.TempCnt_PVGain3
		'p_RTM(0).p_mltcp.HeaterUpper3(1) = p_cts.TempCnt_HUpperLimit3
		p_RTM(0).p_mltcp.HeaterUpper3(1) = HEATER_UPLMT_DEV
		'熱板LED監視温度, 有効ビット
		''p_RTM(0).p_mltcp.LedOnOff = p_cts.TempLED
		''If p_cts.TempLEDEn Then
		''	p_RTM(0).p_mltcp.Config1 = p_RTM(0).p_mltcp.Config1 Or MemLinkTCP.eWConfigBit.iTempLEDEn
		''Else
		''	p_RTM(0).p_mltcp.Config1 = p_RTM(0).p_mltcp.Config1 And Not MemLinkTCP.eWConfigBit.iTempLEDEn
		''End If
		'ブザーコントロール
		If p_cts.EndOfBuzzerEn Then
			p_RTM(0).p_mltcp.Config1 = p_RTM(0).p_mltcp.Config1 Or MemLinkTCP.eWConfigBit.iEndBzz
		Else
			p_RTM(0).p_mltcp.Config1 = p_RTM(0).p_mltcp.Config1 And Not MemLinkTCP.eWConfigBit.iEndBzz
		End If
		'測定終了ガイドブザー音パターン
		p_RTM(0).p_mltcp.Config1	= p_RTM(0).p_mltcp.Config1 AND &h000F
		p_RTM(0).p_mltcp.Config1	= p_RTM(0).p_mltcp.Config1 Or (p_cts.EndOfBuzzerPattern << 4)

		If p_cts.BuzzerOkOfHeatEn Then
			p_RTM(0).p_mltcp.Config1 = p_RTM(0).p_mltcp.Config1 Or MemLinkTCP.eWConfigBit.iHeatOK
		Else
			p_RTM(0).p_mltcp.Config1 = p_RTM(0).p_mltcp.Config1 And Not MemLinkTCP.eWConfigBit.iHeatOK
		End If
		'IL無効
		If p_cts.CoverILDis Then
			p_RTM(0).p_mltcp.Config1 = p_RTM(0).p_mltcp.Config1 Or MemLinkTCP.eWConfigBit.iCoverILDis
		Else
			p_RTM(0).p_mltcp.Config1 = p_RTM(0).p_mltcp.Config1 And Not MemLinkTCP.eWConfigBit.iCoverILDis
		End If
		'If p_cts.CoverILDis Then
		'	p_RTM(0).p_mltcp.Config1 = p_RTM(0).p_mltcp.Config1 Or MemLinkTCP.eWConfigBit.iILAllStop
		'Else
		'	p_RTM(0).p_mltcp.Config1 = p_RTM(0).p_mltcp.Config1 And Not MemLinkTCP.eWConfigBit.iILAllStop
		'End If
		'スタンバイ時間
		p_RTM(0).p_mltcp.StanbyTime		= p_cts.StanbyTime
		'ゼロ調開始位置(um)
		p_RTM(0).p_mltcp.ZeroStartPos	= p_cts.dZeroStaPosHP * 1000.0
		'中央出し開始位置(um)
		p_RTM(0).p_mltcp.CenterStartPos = p_cts.dCenterStaPosHP * 1000.0
		'測定者コンボリストの格納
		'Call sbSetMeasureCombo(p_cts.sAutoPrintOperatorList, p_cts.strAutoPrintOperatorValue)
		'リモートSQL Serverデータベース接続先を設定
		'p_iSqlSvrExpEnable = p_cts.iSqlSvrExpEnable
		'p_eRmtSqlAuthType = p_cts.iSqlSvrExpAuthType
		'p_sRmtSqlSvrName = p_cts.sSqlSvrExpSvrName
		'p_sRmtSqlUserID = p_cts.sSqlSvrExpUserName
		'p_sRmtSqlPassword = p_cts.sSqlSvrExpPassword
		'p_sRmtSqlExpDataPath = p_cts.sSqlSvrExpDataPath

		'応力検知パラメータ
		p_RTM(0).p_mltcp.StressDetectAcquisitionTime	= p_cts.StresDetAcqTime
		p_RTM(0).p_mltcp.StressDetectADThreshold		= p_cts.StresDetADThreshold
		p_RTM(0).p_mltcp.StressDetectDurationTime		= p_cts.StresDetDuraTime

		tsmiFixedX.Checked		= IIf(p_cts.iRTXAxis = 0, False, True)        ' リアルタイムグラフX軸
		tsmiFixedY.Checked		= IIf(p_cts.iRTYAxis = 0, False, True)        ' リアルタイムグラフY軸

		RealTimeThininingValueR = p_cts.iDetailsR					'リアルタイムグラフ時間間引き
		RealTimeThininingValueD = p_cts.iDetailsD					'リアルタイムグラフ時間間引き

		'ゼロ調モード
		''p_RTM(0).p_mltcp.ZeroAdjMode = p_cts.ZeroAdjMode


		DeviceComboUpdate()											'機器選択コンボボックス更新

		'Spec入力項目関係
		p_clSpec.SpecLangageSet(p_sMes)
		p_clSpec.InputRangeSet(eSpec.Device, dgvDevSpec)			'SPECへ数値入力範囲を設定する
		p_clSpec.dgvDevSetting(dgvDevSpec)							'デバイススペックDGVの再設定
		p_clSpec.CheckDevSpecNumericValue(dgvDevSpec, False)		'デバイススペックDGVの入力チェック

		p_clSpec.InputRangeSet(eSpec.Sample, dgvSamSpec)			'SPECへ数値入力範囲を設定する
		p_clSpec.dgvSamSetting(dgvSamSpec)							'サンプルスペックDGVの再設定
		p_clSpec.CheckSamSpecNumericValue(dgvSamSpec, False)		'サンプルスペックDGVの入力チェック

		p_clspec.CheckSpecTimeSpanInput(dgvSamSpec, esamSpec.EFECT_RANGE, False)
		p_clspec.CheckSpecTimeSpanInput(dgvSamSpec, esamSpec.Judge_RANGE, False)
	Else
		Call WriteLog("詳細設定 キャンセルボタンがクリックされました。", eLOG_LEVEL.iINFO)
	End If
	'入力に変化がある場合、書き込み処理
	''With p_cts
	''    If .iEachZeroAdj <> p_RTM(0).p_iEachZeroAdj OrElse _
	''        .iRotaStartLate <> p_RTM(0).p_iRotaStartLate _
	''    Then
	''        p_RTM(0).p_iEachZeroAdj = .iEachZeroAdj
	''        p_RTM(0).p_iRotaStartLate = .iRotaStartLate
	''        p_RTM(0).p_iBGWOrder = BGW_ORDER.SET_BODY
	''    End If
	''End With

	'言語選択を変更
	If iPrevLang <> p_cts.iLang Then
		MessageBox.Show(p_sMes(efrmMainSTR.LANGUAGE_CHANGE_WHEN_RESTARTED), m_sGuideMsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Information)
		Call WriteLog("言語選択に変更がありました。", eLOG_LEVEL.iINFO)
	End If
	'測定モードに変更があったか
	If foptDlg.P_LTModeChange
		AllClear()
		Call WriteLog("測定モードに変更がありました。", eLOG_LEVEL.iINFO)
		If p_cts.LTModeEnable
			p_clSpec.spi.iGelTimeMethod = eCalcMethod.THR
		End If
	End If
	foptDlg.Dispose()

	'ツールの設定に応じて、表に読込専用属性を設定する
	If dgvDevSpec.Item(2, eDevSPEC.TEMP_PROFILE).Value <> vbNullString

		'設定された温度プロファイル
		Dim bRet As Boolean = p_RTM(0).sbLoadRTGSetting(p_RTM(0).p_strRTGSetName)
		If Not bRet
			dgvDevSpec.Item(2, eDevSPEC.TEMP_PROFILE).Value = ""
			p_RTM(0).p_strRTGSetName = dgvDevSpec.Item(2, eDevSPEC.TEMP_PROFILE).Value
		Else
			'初期温度を設定
			p_RTM(0).p_strRTGSetName = dgvDevSpec.Item(2, eDevSPEC.TEMP_PROFILE).Value
			p_RTM(0).p_mltcp.TempPreset(1)	= p_RTM(0).p_rtgset.mdbr(0).dHPTemp
			p_RTM(0).p_mltcp.Temp2Preset(1) = p_RTM(0).p_rtgset.mdbr(0).dHPTemp
			p_RTM(0).p_mltcp.Temp3Preset(1) = p_clSpec.SGTempGet(p_RTM(0).p_rtgset.mdbr(0).dHPTemp)		'起歪体温度
		End If
	Else
		p_RTM(0).p_strRTGSetName = ""
	End If

	'スケール設定、GELTime言語ラベル表示
	sbScaleSetLangchange(p_cts)

	'Chart表示更新
	m_RealTrendUpdate = False                           'リアルタイムチャート初期化
	ChartDispUpdate(Me, 0, m_RealTrendUpdate)
	Select Case p_RTM(0).p_sCalcMethod
	Case lstCalcMethod(eCalcMethod.THR)
	'THR法
	Case lstCalcMethod(eCalcMethod.SLOPE)
	'スロープ法
	Case lstCalcMethod(eCalcMethod.EXTRA)
		'外挿法
		'接線調整する準備
		clExtra.ExtraPrePareProc(0, Chart1, p_dtFile, nudStartTim.Value, nudMidTim.Value, nudEndTim.Value)
		clExtra.GelTimeCrossLineDisp(Chart1)        '接線表示とゲルタイム計算
		Select Case cboGelTSel.SelectedIndex
		Case 0
			p_RTM(0).p_dGelTime = clExtra.LowerGelT
		Case 1
			p_RTM(0).p_dGelTime = clExtra.MiddlGelT
		Case 2
			p_RTM(0).p_dGelTime = clExtra.UpperGelT
		End Select
	End Select

	If p_bMeasTimeEn
	'測定時間 無制限のとき
		nudXscaleMin.Maximum = 999999
		nudXscaleMax.Maximum = 999999

	Else		
		nudXscaleMin.Maximum = 4000
		nudXscaleMax.Maximum = 4000

	End If

	'QRコード作成メニュー 有効
	tsmiQrCodeCreate.Enabled = p_cts.QRModeEnable


Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub
'■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
'   プライベート関数
'■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■

#If 0
''' <summary>
''' 原点復帰
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub tsmiReturnOrigin_Click(sender As Object, e As EventArgs) Handles tsmiReturnOrigin.Click
Try
	btnOrigin_Click(sender,e)
Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub
#end if
Dim		iMainCount			As Int32 = 0

Dim		iZStabJob			As Int32 = 0
Dim		dZStabilityBufY()	As Double = Nothing			'Z応力安定度算出用バッファ
Dim		dZStabilityBufX()	As Double = Nothing			'Z応力安定度算出用バッファ
Dim		swZS1S				As New Stopwatch
Dim		swZS60S				As New Stopwatch
Public	dZStability			As Double = 99.999			'Z応力安定度 g/10Sec

''' <summary>
''' Z応力安定度算出処理
''' </summary>
Private Sub	ZStabilityCheckJob()
Try
	Select Case iZStabJob
	Case 0 
		If IsNothing(dZStabilityBufY)
			Array.Resize(Of Double)(dZStabilityBufY, 1)
			Array.Resize(Of Double)(dZStabilityBufX, 1)
			swZS1S.Stop
			swZS1S.Restart
			swZS60S.Stop
			swZS60S.Restart
			iZStabJob += 10
		End If
	Case 10
		If swZS60S.Elapsed.TotalMilliseconds > 60000 And dZStabilityBufY.Length >= 60
		'60秒分のデータ溜まった
			swZS60S.Stop
			swZS60S.Reset
			Array.Copy(dZStabilityBufY, 1, dZStabilityBufY, 0, dZStabilityBufY.Length-1)
			Array.Copy(dZStabilityBufX, 1, dZStabilityBufX, 0, dZStabilityBufX.Length-1)
			dZStabilityBufY(dZStabilityBufY.GetUpperBound(0)) = p_RTM(0).p_mltcp.CurrStrsZCorrA()
			For i=0 To dZStabilityBufY.GetUpperBound(0)
				dZStabilityBufX(i) = i
			Next
			'最小二乗法 傾き算出
			Dim nX()	As Double = Nothing
			Dim nY()	As Double = Nothing
			Dim dPara() As Double = Nothing
			dPara = datCal.fnLeastSquaresMethod(dZStabilityBufX, dZStabilityBufY, nX, nY, dZStabilityBufX.Length, 1)		'最小二乗法 一次式
			dZStability = dPara(1) * 10.0 * p_cts.WeightCoefZ()
			p_RTM(0).p_dZStability = dZStability					'リアルタイムスレッドへ渡す
			swZS1S.reStart
			iZStabJob += 10
		Else
			If swZS1S.Elapsed.TotalMilliseconds >= 1000
			'1秒経過
				swZS1S.Stop
				swZS1S.Reset
				Array.Resize(Of Double)(dZStabilityBufY, dZStabilityBufY.Length+1)
				Array.Resize(Of Double)(dZStabilityBufX, dZStabilityBufX.Length+1)
				dZStabilityBufY(dZStabilityBufY.GetUpperBound(0)) = p_RTM(0).p_mltcp.CurrStrsZCorrA()
				swZS1S.Start
			End If
		End If
	Case 20
		If swZS1S.Elapsed.TotalMilliseconds >= 1000
		'1秒経過
			swZS1S.Stop
			swZS1S.Reset
			Array.Copy(dZStabilityBufY, 1, dZStabilityBufY, 0, dZStabilityBufY.Length-1)
			Array.Copy(dZStabilityBufX, 1, dZStabilityBufX, 0, dZStabilityBufX.Length-1)
			dZStabilityBufY(dZStabilityBufY.GetUpperBound(0)) = p_RTM(0).p_mltcp.CurrStrsZCorrA()
			For i=0 To dZStabilityBufY.GetUpperBound(0)
				dZStabilityBufX(i) = i
			Next
			'最小二乗法 傾き算出
			Dim nX()	As Double = Nothing
			Dim nY()	As Double = Nothing
			Dim dPara() As Double = Nothing
			dPara = datCal.fnLeastSquaresMethod(dZStabilityBufX, dZStabilityBufY, nX, nY, dZStabilityBufX.Length, 1)		'最小二乗法 一次式
			dZStability = dPara(1) * 10.0 * p_cts.WeightCoefZ()
			p_RTM(0).p_dZStability = dZStability					'リアルタイムスレッドへ渡す
			swZS1S.Start
		End If
	Case Else
		iZStabJob = 0		
	End Select

Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	iZStabJob = 0		
End Try
End Sub
''' <summary>
''' メインタイマインターバル処理 250ms
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub timMain_Tick(sender As Object, e As EventArgs) Handles timMain.Tick
timMain.Enabled = False
Try
	

	NormalLinkIntervalProc()					'常時リンク項目のリンク領域へ転送

	If Not p_FormCalibVisible Then
		ZeroMonitorJob(Me.Chart2, Me.Panel2)    'ゼロ調モニタ
	End If

	ZStabilityCheckJob()						'Z応力安定度チェック制御

	If p_bDebugFileOutEnable
		p_dtfile.DebugTextOut()		'デバッグファイル出力
	End If


Catch ex As Exception
	Call FrmDebugLog(System.Reflection.MethodBase.GetCurrentMethod.Name & ex.Message)
	Console.WriteLine(ex.Message)
End Try
timMain.Enabled = True
End Sub

''' <summary>
''' 常時リンク項目のリンク領域へ転送
''' </summary>
Private Sub NormalLinkIntervalProc()
Try
	MachineStateDisp()                      '本体状態表示
	'MachineErrorDisp()						'本体エラー表示

	If p_RTM(0).p_MainLink = 0 Then             '1:frmMainのDgvからの通信リンク(書込み側)を切る


		'PC->MCN 常時リンク項目 -------------------------------------------------
		sbSpecToLinkArea()


		'温度制御 PID etc 
		p_RTM(0).p_mltcp.P_PID(1) = p_cts.TempCnt_P
		p_RTM(0).p_mltcp.I_PID(1) = p_cts.TempCnt_I
		p_RTM(0).p_mltcp.D_PID(1) = p_cts.TempCnt_D
		p_RTM(0).p_mltcp.DGain(1) = p_cts.TempCnt_Dgain
		p_RTM(0).p_mltcp.PVBias(1) = p_cts.TempCnt_PVBias
		p_RTM(0).p_mltcp.PVGain(1) = p_cts.TempCnt_PVGain
		'p_RTM(0).p_mltcp.HeaterUpper(1) = p_cts.TempCnt_HUpperLimit
		p_RTM(0).p_mltcp.HeaterUpper(1) = HEATER_UPLMT_DEV					'ヒーター上限値

		p_RTM(0).p_mltcp.P_PID2(1) = p_cts.TempCnt_P2
		p_RTM(0).p_mltcp.I_PID2(1) = p_cts.TempCnt_I2
		p_RTM(0).p_mltcp.D_PID2(1) = p_cts.TempCnt_D2
		p_RTM(0).p_mltcp.DGain2(1) = p_cts.TempCnt_Dgain2
		p_RTM(0).p_mltcp.PVBias2(1) = p_cts.TempCnt_PVBias2
		p_RTM(0).p_mltcp.PVGain2(1) = p_cts.TempCnt_PVGain2
		p_RTM(0).p_mltcp.HeaterUpper2(1) = p_cts.TempCnt_HUpperLimit2
		p_RTM(0).p_mltcp.HeaterUpper2(1) = HEATER_UPLMT_DEV

		p_RTM(0).p_mltcp.P_PID3(1) = p_cts.TempCnt_P3
		p_RTM(0).p_mltcp.I_PID3(1) = p_cts.TempCnt_I3
		p_RTM(0).p_mltcp.D_PID3(1) = p_cts.TempCnt_D3
		p_RTM(0).p_mltcp.DGain3(1) = p_cts.TempCnt_Dgain3
		p_RTM(0).p_mltcp.PVBias3(1) = p_cts.TempCnt_PVBias3
		p_RTM(0).p_mltcp.PVGain3(1) = p_cts.TempCnt_PVGain3
		p_RTM(0).p_mltcp.HeaterUpper3(1) = p_cts.TempCnt_HUpperLimit3

		'スタンバイ時間
		p_RTM(0).p_mltcp.StanbyTime = p_cts.StanbyTime

		'ゼロ調開始位置
		p_RTM(0).p_mltcp.ZeroStartPos = p_cts.dZeroStaPosHP * 1000.0
		'中央出し開始位置
		p_RTM(0).p_mltcp.CenterStartPos = p_cts.dCenterStaPosHP * 1000.0

		'インターロック無効等
		p_RTM(0).p_mltcp.AfterMeasureBuzzerSet(p_cts.EndOfBuzzerEn)
		p_RTM(0).p_mltcp.TempOKBuzzerSet(p_cts.BuzzerOkOfHeatEn)
		p_RTM(0).p_mltcp.InterLockSet(p_cts.CoverILDis)
		'測定終了ガイド音パターン
		p_RTM(0).p_mltcp.EndOfPattern((p_cts.EndOfBuzzerPattern<<4))
		'測定中パトライト緑点滅
		p_RTM(0).p_mltcp.GreenFlashSet(p_cts.GreenFlashInMeas)

'Debug.Print(p_RTM(0).p_mltcp.Config1)
		

		'有効範囲、判定範囲
		p_RTM(0).p_dAprxRangeMax	= p_clSpec.spi.dEffectRangeMax
		p_RTM(0).p_dAprxRangeMin	= p_clSpec.spi.dEffectRangeMin
		p_RTM(0).p_dJudgeRangeMax	= p_clSpec.spi.dJudgeRangeMax
		p_RTM(0).p_dJudgeRangeMin	= p_clSpec.spi.dJudgeRangeMin

		'Z軸上限リミット位置
		p_RTM(0).p_mltcp.ZUpperLimit = p_cts.ZUpperLimitPos


	End If

	'装置操作中
	If p_RTM(0).p_mltcp.StartSW <> 0 Then
		'測定中
		p_RTM(0).p_mltcp.InOperation = 1
	Else
		If m_bIsPressKey = True Then
			'キー操作監視
			p_RTM(0).p_mltcp.InOperation = 1
			If sw_KeyObs.Elapsed.TotalSeconds < 5 Then  '5秒間ラッチ
				m_bIsPressKey = True
			Else
				m_bIsPressKey = False
				sw_KeyObs.Stop()
			End If
		Else
			'マウス変化監視
			If (m_iPreMousePosX = MousePosition.X) And (m_iPreMousePosY = MousePosition.Y) Then
				p_RTM(0).p_mltcp.InOperation = 0
			Else
				p_RTM(0).p_mltcp.InOperation = 1
			End If
			m_iPreMousePosX = MousePosition.X
			m_iPreMousePosY = MousePosition.Y
			p_RTM(0).p_mltcp.InOperation = 0
		End If
	End If
	'ブザー設定,IL設定
	'p_RTM(0).p_mltcp.Config1 = p_cts.EndOfBuzzerEn Or p_cts.BuzzerOkOfHeatEn Or p_cts.CoverILDis Or p_cts.AllILDis

	'------------------------------------------------------------------------
	'超過判定
	With p_RTM(0)
		'--- メンテナンス積算時間の超過判定
		'本体電源ON積算時間
		.p_blIntegTimePowerOn	= .ChkMntIntegTimeOver(.p_ulIntegTimePowerOn,	p_cts.iMntChkIntegHourPowerOn)
		'熱板ﾋｰﾀON積算時間
		.p_blIntegTimeHeaterOn	= .ChkMntIntegTimeOver(.p_ulIntegTimeHeaterOn,	p_cts.iMntChkIntegHourHeaterOn)
		'PBﾋｰﾀON積算時間
		.p_blIntegTimeHeater2On = .ChkMntIntegTimeOver(.p_ulIntegTimeHeater2On, p_cts.iMntChkIntegHourHeaterOn2)
		'SGﾋｰﾀON積算時間
		.p_blIntegTimeHeater3On = .ChkMntIntegTimeOver(.p_ulIntegTimeHeater3On, p_cts.iMntChkIntegHourHeaterOn3)
		'X動作ON積算時間
		.p_blIntegTimeXOn		= .ChkMntIntegTimeOver(.p_ulIntegTimeXOn,		p_cts.iMntChkIntegHourXAct)
		'Y動作ON積算時間
		.p_blIntegTimeYOn		= .ChkMntIntegTimeOver(.p_ulIntegTimeYOn,		p_cts.iMntChkIntegHourYAct)
		'Z動作ON積算時間
		.p_blIntegTimeZOn		= .ChkMntIntegTimeOver(.p_ulIntegTimeZOn,		p_cts.iMntChkIntegHourZAct)
		'--- メンテナンスリセット日からの経過年数超過判定
		'本体電源ON積算時間
		.p_blIntegResetDatePowerOn		= .ChkMntRstDayOver(.p_sIntegResetDatePowerOn, p_cts.iMntChkRstYearPowerOn)
		'熱板ﾋｰﾀON積算時間
		.p_blIntegResetDateHeaterOn		= .ChkMntRstDayOver(.p_sIntegResetDateHeaterOn, p_cts.iMntChkRstYearHeaterOn)
		'プローブﾋｰﾀON積算時間
		.p_blIntegResetDateHeater2On	= .ChkMntRstDayOver(.p_sIntegResetDateHeater2On, p_cts.iMntChkRstYearHeaterOn2)
		'起歪体ﾋｰﾀON積算時間
		.p_blIntegResetDateHeater3On	= .ChkMntRstDayOver(.p_sIntegResetDateHeater3On, p_cts.iMntChkRstYearHeaterOn3)
		'X動作ON積算時間
		.p_blIntegResetDateXOn			= .ChkMntRstDayOver(.p_sIntegResetDateXOn, p_cts.iMntChkRstYearXAct)
		'Y動作ON積算時間
		.p_blIntegResetDateYOn			= .ChkMntRstDayOver(.p_sIntegResetDateYOn, p_cts.iMntChkRstYearYAct)
		'Z動作ON積算時間
		.p_blIntegResetDateZOn			= .ChkMntRstDayOver(.p_sIntegResetDateZOn, p_cts.iMntChkRstYearZAct)
		'下記のどれかが超過している
		If .p_blIntegTimePowerOn Or .p_blIntegTimeHeaterOn Or
			.p_blIntegTimeHeater2On Or .p_blIntegTimeHeater3On Or
			.p_blIntegTimeXOn Or _
			.p_blIntegTimeYOn Or _
			.p_blIntegTimeZOn Or _
			.p_blIntegResetDatePowerOn Or .p_blIntegResetDateHeaterOn Or _
			.p_blIntegResetDateHeater2On Or .p_blIntegResetDateHeater3On Or _
			.p_blIntegResetDateXOn Or _
			.p_blIntegResetDateYOn Or _
			.p_blIntegResetDateZOn _
		Then
			'本日まだ警告を表示していない
			If (p_cts.sMntTodayChkDate = "") Or (p_cts.sMntTodayChkDate < Now.ToString("yyyy/MM/dd")) Then
				'メッセージの作成
				Dim frmMsgOver As New frmMSG(p_sMes(efrmMainSTR.iMNT_CNT_OVER), {"OK"}, "", Me)
				frmMsgOver.StartPosition = FormStartPosition.CenterScreen
				frmMsgOver.lblMsg.Font = New Drawing.Font(BASE_FONT, 14)
				'表示メッセージの設定
				frmMsgOver.lblMsg.Text = p_sMes(efrmMainSTR.iMNT_CNT_OVER)

				'メイン画面が最大化以外の場合最大化にする
				'If Me.WindowState <> FormWindowState.Maximized Then
				'    Me.WindowState = FormWindowState.Maximized
				'End If

				'メッセージ表示
				frmMsgOver.Show(Me)

				'メンテナンス積算時間超過チェック済み最終日をIniファイルに保存
				p_cts.sMntTodayChkDate = Now.ToString("yyyy/MM/dd")

				'装置毎設定パラメータ保存
				p_cts.SaveAsDeviceSettingINI(p_sEquipName)
			End If
		End If
	End With


Catch ex As Exception
	Call FrmDebugLog(System.Reflection.MethodBase.GetCurrentMethod.Name & ex.Message)
	Console.WriteLine(ex.Message)
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub


'''<summary> 接続状態等の表示更新。リアルタイム処理で使用 </summary>
Private Sub timerShowState_Tick(sender As Object, e As EventArgs) Handles timerShowState.Tick
timerShowState.Enabled = False
Try
	StateIntervalProc()     'ステータス等の表示インターバル処理
Catch ex As Exception
	Call FrmDebugLog(System.Reflection.MethodBase.GetCurrentMethod.Name & ex.Message)
	Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
timerShowState.Enabled = True
End Sub
Dim iCount				As Int16 = 0     'リアルタイム測定チャート更新タイミングのためのカウンタ
Dim bPreTempCont		As Boolean = False
Dim bPreZeroCont		As Boolean = False
Dim bPreMeasCont		As Boolean = False
Dim dsReal				As DataSet = New DataSet()
Private m_iRealTimeChartUpdateMiriSec As Int64 = 1000				'リアルタイムチャート表示更新インターバル(mS)
Dim iScanTimeAry()		As Int64 = Nothing
Dim iErrorCnt			As Int16 = 0								'起動時のエラーウィンドウ緩衝用
Dim iMeasEndConfirm		As Int16 = -1								'終了確認メッセージ
Dim Sw1					As New Stopwatch

Dim m_RealTrendUpdate	As Boolean = False							'チャート再描画時に再設定を行うか設定
Public Property RealTrendUpdate() As Boolean
	Get
		RealTrendUpdate = m_RealTrendUpdate
	End Get
	Set(value As Boolean)
		m_RealTrendUpdate = value
	End Set
End Property

''' <summary>
''' 状態更新 インターバル処理
''' </summary>
Private Sub StateIntervalProc()
Dim bMesStartEna	As Boolean = False
Dim iBCcnt			As Integer = 0
Dim iRet			As Int32

Try
	'ステータスラベル 背景色
	If ToolStripStatusLabel1.BackColor <> Forms.Control.DefaultBackColor Then
		iBCcnt += 1
		If iBCcnt >= 3 Then
			iBCcnt = 0
			ToolStripStatusLabel1.BackColor = Forms.Control.DefaultBackColor
		End If
	End If

	'接続状態を表示に反映
	sbRefreshStateLbl()

	'機器製造番号変更によるコンボボックスリストの更新
	If p_RTM(0).p_iConnectState = CONNECT_ST.CONNECTING AndAlso p_RTM(0).p_ConnectAfterProc = 2
		DeviceComboUpdate()											'機器名称コンボボックスを設定
		SetForderCreate()											'データ管理フォルダ環境生成
		p_RTM(0).p_ConnectAfterProc = 0								'処理完了 リアルタイムスレッドへ通知
	End If

	'操作ボタン状態を表示
	If p_RTM(0).p_iConnectState = CONNECT_ST.CONNECTING Then
		sbConnectEnable(True)

		'温度ボタン
		Dim iTempBtnState As Int16 = IIf((p_RTM(0).p_iHeatRdy = 3) And (p_RTM(0).p_mltcp.isStatHeaterOn = True) And (p_RTM(0).p_mltcp.isStatHeaterOn2 = True), 1, 0)
		modFrmMain.sbDispStateToButton(btnTemp, IIf((p_RTM(0).p_mltcp.isStatHeaterOn And p_RTM(0).p_mltcp.isStatHeaterOn2), 1, 0), iTempBtnState)
		'If p_RTM(0).p_mltcp.isStatHeaterOn <> bPreTempCont Then
		'	If p_RTM(0).p_mltcp.isStatHeaterOn Then
		'		UnderStatusLabelDisp(ToolStripStatusLabel1, p_sMes(efrmMainSTR.iTEMP_CONT_START))					'温度制御開始メッセージ
		'	End If
		'	bPreTempCont = p_RTM(0).p_mltcp.isStatHeaterOn
		'End If
		btnTemp.Enabled = True                      '温度制御ボタン有効

		'ゼロ調ボタン
		modFrmMain.sbDispStateToButton(btnZero, IIf(p_RTM(0).p_mltcp.IsZeroStartSW, 1, 0), p_RTM(0).p_iZeroAdjOk)
		Select Case p_RTM(0).p_iBGWOrder
		Case BGW_ORDER.NO_ORDER
		'ゼロ調開始前
			If p_RTM(0).p_iZeroAdjOk <= 0
				If Not p_bZStrStabDis		
					If p_cts.SGStressStab = 0 Or Math.abs(dZStability) < p_cts.SGStressStab
						btnZero.Enabled = True                      'ゼロ調ボタン有効
					Else
						btnZero.Enabled = False
					End If
				Else
				'デバッグ Z応力安定度チェック無効
					btnZero.Enabled = True                      'ゼロ調ボタン有効
				End If
			ElseIf p_RTM(0).p_iZeroAdjOk = 1
				btnZero.Enabled = True	                     'ゼロ調ボタン有効
			End If
		Case BGW_ORDER.ZERO_BUTTON_CNF, BGW_ORDER.ZERO_BUTTON_0, BGW_ORDER.ZERO_BUTTON_1
		'ゼロ調中または完了
			If p_RTM(0).p_iZeroAdjOk <= 0
				btnZero.Enabled = True	                     'ゼロ調ボタン有効
			ElseIf p_RTM(0).p_iZeroAdjOk = 1
				btnZero.Enabled = True	                     'ゼロ調ボタン有効
			End If
		Case Else
			btnZero.Enabled = False                     'ゼロ調ボタン無効
		End Select
#If 0
		'原点復帰ボタン
		modFrmMain.sbDispStateToButton(btnOrigin, IIf(p_RTM(0).p_mltcp.IsPosOrderOrigin, 1, 0), IIf(p_RTM(0).p_mltcp.isStatOriginComp, 1, 0))
		If p_RTM(0).p_mltcp.StartSW = 0 Then
			btnOrigin.Enabled = True        '原点復帰ボタン有効  ゼロ調、測定中以外なら
		End If
#end if

		'測定ボタン
		'測定準備完了(温度OK、Zero調済、エラーなし)
		If p_RTM(0).p_iHeatRdy = 3					AndAlso
			(p_RTM(0).p_mltcp.StartSW Or &H1) <> 0	AndAlso
			p_RTM(0).p_iZeroAdjOk = 1				AndAlso
			p_iDspErrStts1 = 0						AndAlso
			p_iDspErrStts2 = 0 _
		Then
			'測定開始有効フラグON
			bMesStartEna = True
		ElseIf p_RTM(0).p_iHeatRdy <> 3 AndAlso p_RTM(0).p_MeasureState <> RealTimeMeas.eMeasStat.NOT_START Then
			'測定開始で温度範囲が外れても停止ボタンを無効にしない
			bMesStartEna = True
		End If

		If p_RTM(0).p_iBGWOrder <> BGW_ORDER.START_RTG And
			p_RTM(0).p_iBGWOrder <> BGW_ORDER.ZERO_BUTTON _
		Then
			'測定ボタンの有効設定
			btnMeasure.Enabled = bMesStartEna
		End If
		modFrmMain.sbMeasureBottonStateDisp(Me)                              '測定開始/停止 表示切替

	Else
		'温度ボタン
		modFrmMain.sbDispStateToButton(btnTemp, 0)
		btnTemp.Enabled = False
		'ゼロ調ボタン
		modFrmMain.sbDispStateToButton(btnZero, 0)
		btnZero.Enabled = False
#If 0
		'原点復帰ボタン
		modFrmMain.sbDispStateToButton(btnOrigin, 0)
		btnOrigin.Enabled = False
#end If
		'測定ボタン
		btnMeasure.Enabled = False
	End If

	'メニュー項目
	If Not p_RTM(0).p_mltcp.IsZeroStartSW Then
		Panel2.Visible = False
	End If
	'外挿法PAD Saveボタン
	If IsNothing(p_dtFile.iHosei) Or p_RTM(0).p_iBGWOrder <> BGW_ORDER.NO_ORDER Then
		btnExtrapolationSave.Enabled = False
	Else
		btnExtrapolationSave.Enabled = True
	End If
	'SPCボタン制御
	If p_cts.SPCEn = 1
	'SPC機能 有効
		btnSPC.Visible = True					'SPCボタン表示
		If Not IsNothing(dSPCValues)
		'サンプル配列あり
			btnSPC.Text = $"SPC [{If(dSPCValues.Length=1 And dSPCValues(0)=-1, 0, dSPCValues.Length)}]"		'ボタンへ配列数表示
			If dSPCValues.Length >= p_cts.SPCCalcSample
			'CL計算のためのサンプル数に達した
				btnSPC.Enabled = True
				btnSPC.BackColor = Color.LightGreen
			Else
				btnSPC.Enabled = True
				btnSPC.BackColor = Control.DefaultBackColor
			End If
		Else
			btnSPC.Text = "SPC"
			btnSPC.Enabled = True
			btnSPC.BackColor = Control.DefaultBackColor
		End If
#If 0
		'SPC 統計範囲変更対応
		If p_bSpcRangeEnb And isNothing(p_frmSPC)
		'SPC統計範囲変更要求
			btnSPC.PerformClick()		'SPCボタンクリック起動
			p_bSpcRangeEnb = False
		End If
#end if
	Else
		btnSPC.Visible = False
	End If



#If 1 Then
	'キャリブレーション中の測定はチャート表示しない
	Select Case p_RTM(0).p_iBGWOrder
	Case BGW_ORDER.ZERO_BUTTON_N, BGW_ORDER.ZERO_BUTTON_CNF
		If p_RTM(0).p_iZeroAdjOk = -9
		'ゼロ調整時、本体応答タイムオーバー
			UnderStatusLabelDisp(ToolStripStatusLabel1, p_sMes(efrmMainSTR.iDeviceTimeOut))
			p_RTM(0).p_iZeroAdjOk = -10
		End If
	Case BGW_ORDER.ZERO_BUTTON_2
		If p_RTM(0).p_iZeroAdjOk = 1
		'ゼロ調整完了 
			UnderStatusLabelDisp(ToolStripStatusLabel1, p_sMes(efrmMainSTR.iZeroAdjComp))
		End If
		
	Case BGW_ORDER.START_BUTTON To BGW_ORDER.MEAS_PREPARE_1
		btnAllClear.Enabled = False
		iMeasEndConfirm = -1									'脱調確認指令ON? 初期化
	Case BGW_ORDER.MEAS_PROCESS
		lblElapsedVal.TextAlign = ContentAlignment.MiddleRight

#If 1
		If p_bMeasTimeEn
		'測定時間 無制限のとき
			Dim sMes As String = p_sMes(efrmMainSTR.iACCUM_SAMP)
			sMes = sMes.Replace("#NUM#", p_dtFile.DataCount.ToString())
			UnderStatusLabelDisp(ToolStripStatusLabel1, sMes)															'蓄積データ数
		Else
	'リアルタイムグラフ更新
			If iCount * timerShowState.Interval >= m_iRealTimeChartUpdateMiriSec Then                 '指定mS毎にリアルタイムチャート更新
				'チャート表示更新 ------------------
				ChartDispUpdate(Me, 1, m_RealTrendUpdate)
				Dim sMes As String = p_sMes(efrmMainSTR.iACCUM_SAMP)
				sMes = sMes.Replace("#NUM#", p_dtFile.DataCount.ToString())
				UnderStatusLabelDisp(ToolStripStatusLabel1, sMes)															'蓄積データ数
				'-----------------------------------
				Select Case iScanTimeAry.Max
				Case < 100
					m_iRealTimeChartUpdateMiriSec = 1000
				Case 100 To 200
					m_iRealTimeChartUpdateMiriSec = 1500
				Case 200 To 500
					m_iRealTimeChartUpdateMiriSec = 2000
				Case 500 To 750
					m_iRealTimeChartUpdateMiriSec = 3000
				Case 750 To 1000
					m_iRealTimeChartUpdateMiriSec = 4000
				Case > 1000
					m_iRealTimeChartUpdateMiriSec = 5000
				End Select

				iCount = 0
				Array.Resize(iScanTimeAry, iCount + 1)
				iScanTimeAry(iCount) = p_RTM(0).p_lElapseTime
			Else
				iCount += 1
				Array.Resize(iScanTimeAry, iCount + 1)
				iScanTimeAry(iCount) = p_RTM(0).p_lElapseTime
			End If
		End If

#end If

#if 0
	Case BGW_ORDER.MEAS_PRO_DATA
		'ゲルタイム計算中
		UnderStatusLabelDisp(ToolStripStatusLabel1, p_sMes(efrmMainSTR.iMEAS_END_CALC))
		Chart1.Cursor = Cursors.WaitCursor
		lblCalclating.BackColor = Chart1.ChartAreas(0).BackColor
		lblCalclating.Visible = True
#End If

	Case BGW_ORDER.LT_DATACAL_DISP
	'長期間欠測定モード データ更新時
		'チャート表示更新 ------------------
		ChartDispUpdate(Me, 1, m_RealTrendUpdate)
		Dim sMes As String = p_sMes(efrmMainSTR.iACCUM_SAMP)
		sMes = sMes.Replace("#NUM#", p_RTM(0).p_LTDataPointNum.ToString())
		UnderStatusLabelDisp(ToolStripStatusLabel1, sMes)															'蓄積データ数
		If p_RTM(0).p_DispUpdate = 1
			p_RTM(0).p_DispUpdate = 0
		End If

		'-----------------------------------

	'Case BGW_ORDER.MEAS_PRO_DATA, BGW_ORDER.MEAS_PRO_FINISH_1
	Case BGW_ORDER.WAIT_LOCKOUT, BGW_ORDER.LT_WAIT_LOCKOUT
	'プローブロック外し待ち
		If iMeasEndConfirm = 1									'脱調確認指令ON?
		'既に動作済みなので何もしない
		Else
		p_RTM(0).p_mltcp.ZeroSW(False)                                                               'ZERO調制御開始Bit OFF
		p_RTM(0).p_mltcp.MeasSW(False)                                                               '測定開始指令Bit OFF

		'測定終了確認
		iRet = modfrmMain.AfterMeasureOperationGuideMsgBox(Me, 0)
		If iRet = 0 Then
#If 0
			'熱板下降
			p_RTM(0).p_iHotPlatePos = 0
			p_RTM(0).p_iBGWOrder = BGW_ORDER.HOT_PLATE_DRV_REP                                      'Z軸手動移動
#else
			'脱調確認
			'p_RTM(0).p_iBGWOrder = BGW_ORDER.OutOfStepConf_REP                                     '脱調確認シーケンスへ
			p_RTM(0).p_mltcp.PosOrderOutOfStepConf(True)											'脱調確認指令ON
			iMeasEndConfirm = 1
			Sw1.Restart()																			'脱調確認指令OFF用タイマー 15秒後にOFF
			Chart1.Cursor = Cursors.WaitCursor
			p_RTM(0).p_MeasureState =  eMeasStat.LOCKOUT_OK											'測定状態をロック解除済をセット これで計算開始

			If p_RTM(0).p_iBGWOrder = BGW_ORDER.WAIT_LOCKOUT
				'ゲルタイム計算中 表示
				UnderStatusLabelDisp(ToolStripStatusLabel1, p_sMes(efrmMainSTR.iMEAS_END_CALC))
				Chart1.Cursor = Cursors.WaitCursor
				lblCalclating.BackColor = Chart1.ChartAreas(0).BackColor
				lblCalclating.Text		= p_sMes(efrmMainSTR.iCalucurating)
				lblCalclating.Visible	= True
				Me.Refresh
			End If
#end if
		Else
			p_RTM(0).p_iBGWOrder = BGW_ORDER.NO_ORDER                                               'アイドル -> そのまま終了
		End If

		Dim sMes As String = p_sMes(efrmMainSTR.iACCUM_SAMP)
		sMes = sMes.Replace("#NUM#", p_dtFile.DataCount.ToString())
		UnderStatusLabelDisp(ToolStripStatusLabel1, sMes)											           '蓄積データ数

		If p_RTM(0).p_strRTGSetName.Length = 0 Then
			'設定温度へ戻す(温度プロファイル使用考慮)
			p_RTM(0).p_mltcp.TempPreset(1)		= p_clSpec.spi.dHPSetTemp	'熱板温度
			p_RTM(0).p_mltcp.Temp2Preset(1)		= p_clSpec.spi.dPBSetTemp	'プローブ温度
			p_RTM(0).p_mltcp.Temp3Preset(1)		= p_clSpec.SGTempGet(p_clSpec.spi.dHPSetTemp)	'起歪体温度
		Else
		'温度プロファイル中は何もしない
		End If

		'変更のあった項目の青色フォントをデフォルトへ戻す
		'変更あったSPECがFREEなら一旦セーブ
		If cbxDevSPEC.Text = cFree
			p_clSpec.specWrite(eSpec.Device, Me, cbxDevSPEC.Text)
		End If
		If cbxSamSPEC.Text = cFree
			p_clSpec.specWrite(eSpec.Sample, Me, cbxSamSPEC.Text)
		End If
		''p_clSpec.dgvSpecDispUpdate(eSpec.Sample, Me, cbxSamSPEC.Text)
		''p_clSpec.dgvSpecDispUpdate(eSpec.Device, Me, cbxDevSPEC.Text)

		LongTimeModeDispCont(False)				'長時間測定表示制御
		InputDisableForMeasure(0)				'測定時入力禁止解除
		End If		

	Case BGW_ORDER.MEAS_BEFORE_CANCEL, BGW_ORDER.LT_MEAS_BEFORE_CANCEL,BGW_ORDER.MEAS_ERROR_CANCEL, BGW_ORDER.LT_MEAS_ERROR_CANCEL
		InputDisableForMeasure(0)				'測定時入力禁止解除

	Case BGW_ORDER.MEAS_PRO_FINISH, BGW_ORDER.LT_MEAS_PRO_FINISH
		lblCalclating.SetTextEx(p_sMes(efrmMainSTR.iCalucurating) & (datCal.CalCount / datCal.CalCountTotal * 100).ToString("F0") & "%", lblCalclating.BackColor)

	Case BGW_ORDER.MEAS_PRO_FINISH_2, BGW_ORDER.LT_MEAS_PRO_FINISH_2
	'計算終了
		'計算中 表示OFF
		Chart1.Cursor = Cursors.Default
		lblCalclating.Visible = False

		'計算後の画面表示処理
		Select Case p_RTM(0).p_sCalcMethod
		'外挿法
		Case lstCalcMethod(eCalcMethod.EXTRA)
			'接線調整する準備
			clExtra.ExtraPrePareProc(0, Chart1, p_dtFile, nudStartTim.Value, nudMidTim.Value, nudEndTim.Value)
			clExtra.GelTimeCrossLineDisp(Chart1)        '接線表示とゲルタイム計算
			Select Case cboGelTSel.SelectedIndex
			Case 0
				p_RTM(0).p_dGelTime = clExtra.LowerGelT
			Case 1
				p_RTM(0).p_dGelTime = clExtra.MiddlGelT
			Case 2
				p_RTM(0).p_dGelTime = clExtra.UpperGelT
			End Select
		End Select
		'測定終了時のゲルタイム表示とグラフ選択
		Select Case p_RTM(0).p_sCalcMethod
		Case lstCalcMethod(eCalcMethod.THR), lstCalcMethod(eCalcMethod.SLOPE), lstCalcMethod(eCalcMethod.PHASE)
			If p_RTM(0).p_dGelTime < 0 Then
				lblResult1Val.Text		= "N/A"
				lblResult1Val.TextAlign = ContentAlignment.MiddleCenter
			Else
				Dim sGelTime As String = If(p_cts.LTModeEnable, (p_RTM(0).p_dGelTime/60).ToString("F0"), p_RTM(0).p_dGelTime.ToString("F2"))
				'lblResult1Val.Text		= p_RTM(0).p_dGelTime.ToString("F2")                             'ゲルタイム結果表示
				lblResult1Val.Text		= sGelTime														'ゲルタイム結果表示
				lblResult1Val.TextAlign = ContentAlignment.MiddleRight
			End If
			'lblResult1.Text			= p_sMes(efrmMainSTR.iRESULT1)
			Select Case p_cts.LTModeEnable
			Case True
				lblResult1.Text		= p_sMes(efrmMainSTR.iGelTimeLblMin)
			Case Else
				lblResult1.Text		= p_sMes(efrmMainSTR.iGelTimeLblSec)
			End Select
			lblResult1Val.Visible	= True
			lblResult1.Visible		= True
			lblResult2Val.Visible	= False
			lblResult2.Visible		= False
			lblResult3Val.Visible	= False
			lblResult3.Visible		= False
		Case lstCalcMethod(eCalcMethod.MELT)
			If p_RTM(0).p_dGelTime < 0 Then
				lblResult1Val.Text		= "N/A"															'ゲルタイム結果表示
				lblResult1Val.TextAlign = ContentAlignment.MiddleCenter
				lblResult2Val.Text		= "N/A"								                            '温度結果表示
				lblResult2Val.TextAlign = ContentAlignment.MiddleCenter
				lblResult3Val.Text		= "N/A"									                        '粘度結果表示
				lblResult3Val.TextAlign = ContentAlignment.MiddleCenter
			Else
				Dim sGelTime As String = If(p_cts.LTModeEnable, (p_RTM(0).p_dGelTime/60).ToString("F0"), p_RTM(0).p_dGelTime.ToString("F1"))
				lblResult1Val.Text		= sGelTime														'ゲルタイム結果表示
				lblResult1Val.TextAlign = ContentAlignment.MiddleRight
				lblResult2Val.Text		= p_RTM(0).p_dMeltTemp.ToString("F1")                           '温度結果表示
				lblResult2Val.TextAlign = ContentAlignment.MiddleRight
				lblResult3Val.Text		= p_RTM(0).p_dMeltVisco.ToString("F1")                          '粘度結果表示
				lblResult3Val.TextAlign = ContentAlignment.MiddleRight
			End If
			Select Case p_cts.LTModeEnable
			Case True
				lblResult1.Text		= p_sMes(efrmMainSTR.iGelTimeLblMin)
			Case Else
				lblResult1.Text		= p_sMes(efrmMainSTR.iGelTimeLblSec)
			End Select
			lblResult2.Text			= p_sMes(efrmMainSTR.iRESULT2)
			lblResult3.Text			= p_sMes(efrmMainSTR.iRESULT3)
			lblResult1Val.Visible	= True
			lblResult1.Visible		= True
			lblResult2Val.Visible	= True
			lblResult2.Visible		= True
			lblResult3Val.Visible	= True
			lblResult3.Visible		= True
		End Select
		Select Case p_RTM(0).p_iGelTimeJudge
		Case 0	'NG
			lblResult1Val.BackColor = Color.LightPink
		Case 1	'OK
			lblResult1Val.BackColor = Color.LightGreen
		Case Else
			lblResult1Val.BackColor = Color.White
		End Select


		Select Case p_RTM(0).p_sCalcMethod
		Case lstCalcMethod(eCalcMethod.THR),lstCalcMethod(eCalcMethod.SLOPE),lstCalcMethod(eCalcMethod.EXTRA)
			rbCorrect.Enabled = True
			rbCorrect.Checked = True                                                                    '補正データ画面の表示
			rbCorrect.PerformClick()
		Case lstCalcMethod(eCalcMethod.PHASE)
			rbPosPhase.Enabled = True
			rbPosPhase.Checked = True                                                                   
			rbPosPhase.PerformClick()
		Case lstCalcMethod(eCalcMethod.MELT)
			rbViscosity.Enabled = True
			rbViscosity.Checked = True                                                                  
			rbViscosity.PerformClick()
		End Select

		m_RealTrendUpdate = False
		ChartDispUpdate(Me, 0, m_RealTrendUpdate)															'詳細で再描画

		Dim sMes As String = p_sMes(efrmMainSTR.iMEAS_END_GEL)
		sMes = sMes.Replace("#GELTIME#", If(p_RTM(0).p_dGelTime >= 0, p_RTM(0).p_dGelTime.ToString(),"NA"))
		UnderStatusLabelDisp(ToolStripStatusLabel1, sMes)																	'測定終了メッセージ
'-----------------------------------------------------------------------------------------



	Case BGW_ORDER.ZERO_BUTTON, BGW_ORDER.ZERO_BUTTON_CNF, BGW_ORDER.ZERO_BUTTON_1
		lblElapsedVal.Text = "N/A"
		lblElapsedVal.TextAlign = ContentAlignment.MiddleCenter

	Case BGW_ORDER.CALIB_STOP_1
		If m_iFrmCalib = 0 Then
			'フルストレス測定終了確認
			'ロック外して下降(OK)->剥し温度上昇(Yes,No)

			'熱板下降確認
			iRet = modFrmMain.AfterMeasureOperationGuideMsgBox(Me, 5)									'ロック外し(OK)
			If iRet = 0 Then
				'熱板下降
				p_RTM(0).p_mltcp.PosHeatB = 0															'目標位置セット
				p_RTM(0).p_mltcp.PosOrderZ(True)														'Z駆動指令BIT ON

				'手動剥がしシーケンス実行確認
				p_RTM(0).p_iBGWOrder = BGW_ORDER.CALIB_STOP_2											'剥がし処理確認
			Else
			'装置エラー発生
				p_RTM(0).p_iBGWOrder = BGW_ORDER.NO_ORDER                                               '
			End If
		Else
			'ゼロストレス測定終了確認
			iRet = modFrmMain.AfterMeasureOperationGuideMsgBox(Me, 2)
			If iRet = 0 Then
				'熱板下降
				p_RTM(0).p_mltcp.PosHeatB = 0															'目標位置セット
				p_RTM(0).p_mltcp.PosOrderZ(True)														'Z駆動指令BIT ON
				'下降終わるまでここで待つ
				System.Threading.Thread.Sleep(500)
				Do While p_RTM(0).p_mltcp.isStatGoingUpDown()
					If p_RTM(0).p_bDevError
						Exit Do			'エラー発生したら抜ける
					End If
					System.Threading.Thread.Sleep(10)
				Loop
				p_RTM(0).p_mltcp.PosOrderZ(False)														'Z駆動指令BIT OFF
				p_RTM(0).p_iBGWOrder = BGW_ORDER.NO_ORDER   
			End If'
		End If

	Case BGW_ORDER.CALIB_STOP_2
		'手動剥しシーケンス確認
		iRet = modFrmMain.AfterMeasureOperationGuideMsgBox(Me, 6)									'剥し温度上昇(Yes,No)
		If iRet = 0 Then
			'手動剥がしシーケンス開始
			p_RTM(0).p_mltcp.PosHeatB = 0															'目標位置セット
			p_RTM(0).p_iBGWOrder = BGW_ORDER.MANU_PEELING                                           '手動剥がし処理
		Else
			p_RTM(0).p_mltcp.StartTempCont = 0                                                      '温度制御開始Bit OFF
			p_RTM(0).p_iBGWOrder = BGW_ORDER.NO_ORDER                                               '
		End If
		'下降終わるまでここで待つ
		System.Threading.Thread.Sleep(500)
		Do While p_RTM(0).p_mltcp.isStatGoingUpDown()
			If p_RTM(0).p_bDevError
				Exit Do			'エラー発生したら抜ける
			End If
			System.Threading.Thread.Sleep(10)
		Loop
		p_RTM(0).p_mltcp.PosOrderZ(False)														'Z駆動指令BIT OFF

	Case BGW_ORDER.FLCV_STOP_1
		'フローカーブ測定終了確認
		iRet = modFrmMain.AfterMeasureOperationGuideMsgBox(Me, 3)

#If 0
		'熱板下降
		p_RTM(0).p_iHotPlatePos = 0
		p_RTM(0).p_iBGWOrder = BGW_ORDER.HOT_PLATE_DRV_REP                                              'Z軸手動移動
#else
		If iRet = 0
			'脱調確認
			p_RTM(0).p_iBGWOrder = BGW_ORDER.OutOfStepConf_REP                                      '脱調確認シーケンスへ
		Else
			p_RTM(0).p_iBGWOrder = BGW_ORDER.NO_ORDER                                                   'アイドル
		End If
#end if


	Case BGW_ORDER.FLCV_MEAS_PRO_FINISH_1
		'測定後の処理
		'フローカーブ 時間依存
		'間引き無しで再描画
		'ChartDispUpdate(0)

		'測定終了確認
		iRet = modFrmMain.AfterMeasureOperationGuideMsgBox(Me, 3)
		If iRet = 0 Then
#If 0
			'熱板下降
			p_RTM(0).p_iHotPlatePos = 0
			p_RTM(0).p_iBGWOrder = BGW_ORDER.HOT_PLATE_DRV_REP                                          'Z軸手動移動
#else
			'脱調確認
			p_RTM(0).p_iBGWOrder = BGW_ORDER.OutOfStepConf_REP                                      '脱調確認シーケンスへ
#end if
		Else
			p_RTM(0).p_iBGWOrder = BGW_ORDER.NO_ORDER                                                   'アイドル
		End If
	Case Else
	End Select
#End If
	If Sw1.Elapsed.TotalSeconds > 10
		'測定終了確認して10秒経過後、応答ビットを確認後 脱調確認コマンドをOFF
		If p_RTM(0).p_mltcp.isOosConf
			p_RTM(0).p_mltcp.PosOrderOutOfStepConf(False)						'脱調確認指令OFF
			iMeasEndConfirm = -1
			Sw1.Reset()
		End If
	End If

	'本体CPU死活信号表示
	lblDevAlive.Text = p_RTM(0).p_mltcp.DevAlive.ToString("D6")
	'本体ソフトバージョン表示(制御基板+通信基板)
	Dim sCPU0Ver As String = p_RTM(0).p_mltcp.DevSoftVer.ToString("F2")
	Dim sCPU1Ver As String = p_RTM(0).p_mltcp.ComSoftVer.ToString("F2")
	lblCpuVer.Text = If(sCPU0Ver="0.00", "----",sCPU0Ver) & " + " & If(sCPU1Ver="0.00", "----",sCPU1Ver)

#If 1
	'エラー画面表示制御
	If p_RTM(0).p_iConnectState = CONNECT_ST.CONNECTING Then
		If (p_RTM(0).p_mltcp.DevErr1 And &HFFFF) <> 0 Or
			p_RTM(0).p_mltcp.DevErr2 <> 0 Or
			(p_RTM(0).p_mltcp.DevErr3 And &HF9FF) <> 0 Or
			p_RTM(0).p_bDevInternalComError
			If iErrorCnt > 2
				'エラー一覧表示画面
				frmError.ShowDialog()
				p_bError = True
				p_RTM(0).p_MeasureState = eMeasStat.NOT_START
			Else
				iErrorCnt += 1
			End If
		Else
			p_bError = False
			iErrorCnt = 0
		End If
	End If
#end If

	'応力レンジオーバーランプ制御
	If p_RTM(0).p_mltcp.isErrXStressOver Then
		lblXRangeOverLamp.BackColor = Color.Red
		lblXRangeOverLamp.ForeColor = Color.White
	Else
		lblXRangeOverLamp.BackColor = Forms.Control.DefaultBackColor
		lblXRangeOverLamp.ForeColor = Forms.Control.DefaultForeColor
	End If
	If p_RTM(0).p_mltcp.isErrYStressOver Then
		lblYRangeOverLamp.BackColor = Color.Red
		lblYRangeOverLamp.ForeColor = Color.White
	Else
		lblYRangeOverLamp.BackColor = Forms.Control.DefaultBackColor
		lblYRangeOverLamp.ForeColor = Forms.Control.DefaultForeColor
	End If
	If p_RTM(0).p_mltcp.isErrZStressOver Then
		lblZRangeOverLamp.BackColor = Color.Red
		lblZRangeOverLamp.ForeColor = Color.White
	Else
		lblZRangeOverLamp.BackColor = Forms.Control.DefaultBackColor
		lblZRangeOverLamp.ForeColor = Forms.Control.DefaultForeColor
	End If

	'インターロック状態表示
	If p_RTM(0).p_mltcp.isILDisable			'IL無効も見る
		lblInterlockVal.Text		= "DISABLE"
		lblInterlockVal.BackColor	= Color.Gray
		lblInterlockVal.ForeColor	= Color.White
	ElseIf p_RTM(0).p_mltcp.isILState()		'IL(扉閉) OKか? 
		lblInterlockVal.Text		= "CLOSE"
		lblInterlockVal.BackColor	= Color.GreenYellow
		lblInterlockVal.ForeColor	= Color.Black
	Else
		lblInterlockVal.Text		= "OPEN"
		lblInterlockVal.BackColor	= Color.Yellow
		lblInterlockVal.ForeColor	= Color.Black
	End If

	'チャート系列選択の長期間欠モード制御
	If p_cts.LTModeEnable Or p_dtFile.p_head.sMeasureType = p_dtFile.sMeasindCode(eMesCD.LTM)
		rbYStress.Enabled	= False
		rbZStress.Enabled	= False
		rbXStress.Enabled	= False
		rbZPosition.Enabled	= False
		If rbYStress.Checked Or rbZStress.Checked Or rbXStress.Checked Or rbZPosition.Checked
			rbCorrectP.Checked	= True
		End If
		nudXscaleMax.Maximum	= 10 * 24 * 60
		nudXscaleMin.Maximum	= 10 * 24 * 60
		nudXSpan.Maximum		= 10 * 24 * 60 * 60
		lblLTMMode.Visible		= True
	Else
		rbYStress.Enabled	= True
		rbZStress.Enabled	= True
		rbXStress.Enabled	= True
		rbZPosition.Enabled	= True
		nudXscaleMax.Maximum	= 4000
		nudXscaleMin.Maximum	= 4000
		nudXSpan.Maximum		= 1000
		lblLTMMode.Visible		= False
	End If

	'着圧制御警告表示更新
	lblFailedOrderP.Visible = If( p_iFailedOrderP = 1, True, False )
	'測定GAP、XY動作のガイド表示更新
	lblGuideXYbeforeGap.Visible = If( p_iGuideXYbeforeGap = 1 Or (p_RTM(0).p_mltcp.DevErr3 And &H0200) <> 0, True, False )

	'QRコード 受付ボタン制御
	If p_RTM(0).p_MeasureState = RealTimeMeas.eMeasStat.NOT_START Then
		'測定中以外
		If p_cts.QRModeEnable
			Dim sBuf() As String = Nothing
			pnlQR.Visible = True
			Select Case p_iQRState
			Case clQRCodeControl.eQRState.Preparing
			'準備中
				btnQRcode.Text = p_sMes(efrmMainSTR.iQRCodePrepare)
				btnQRcode.BackColor = Color.LightCyan
				cbxSamSPEC.Enabled = True
				cbxDevSPEC.Enabled = True
			Case clQRCodeControl.eQRState.Receipting
			'受付中
				btnQRcode.Text = p_sMes(efrmMainSTR.iQRCodeReceipt)
				btnQRcode.BackColor = Color.LightYellow
				cbxSamSPEC.Enabled = True
				cbxDevSPEC.Enabled = True
				If txtQR.Text <> ""
					sBuf = txtQR.Text.Split(",")
					If  sBuf.Length >= 36
						'QRCode読込後
						'コンボリストQRCodeを選択
						If Not cbxSamSPEC.Items.Contains(cQRCode)
							cbxSamSPEC.Items.Add(cQRCode)
						End If
						cbxSamSPEC.Text = cQRCode
						If Not cbxDevSPEC.Items.Contains(cQRCode)
							cbxDevSPEC.Items.Add(cQRCode)
						End If
						cbxDevSPEC.Text = cQRCode
						'読込んだQRコードを展開しSPEC表示する
						QRCodeTextExpand(txtQR.Text, dgvSamSpec, dgvDevSpec)
						'QRCode SPEC ファイル保存
						p_clSpec.specWrite(eSpec.Sample, Me, cQRCode)
						p_clSpec.specWrite(eSpec.Device, Me, cQRCode)
						p_iQRState = clQRCodeControl.eQRState.Complete
					End If
				End If
			Case clQRCodeControl.eQRState.Complete
			'受付完了
				btnQRcode.Text = p_sMes(efrmMainSTR.iQRCodeComplete)
				btnQRcode.BackColor = Color.LightGreen
				cbxSamSPEC.Enabled = False
				cbxDevSPEC.Enabled = False
				'サンプルSPEC入力監視
				If cbxDevSPEC.Text = cFree Or cbxSamSPEC.Text <> cQRCode
					p_iQRState = clQRCodeControl.eQRState.Preparing
				End If
				'デバイスSPEC入力監視
				If cbxDevSPEC.Text = cFree Or cbxDevSPEC.Text <> cQRCode
					p_iQRState = clQRCodeControl.eQRState.Preparing
				End If

				If dgvDevSpec.Item(2, eDevSPEC.TEMP_PROFILE).Value <> vbNullString
				'設定された温度プロファイル
					Dim bRet As Boolean = p_RTM(0).sbLoadRTGSetting(p_RTM(0).p_strRTGSetName)
					If Not bRet
						dgvDevSpec.Item(2, eDevSPEC.TEMP_PROFILE).Value = ""
						p_RTM(0).p_strRTGSetName = dgvDevSpec.Item(2, eDevSPEC.TEMP_PROFILE).Value
					Else
						'初期温度を設定
						p_RTM(0).p_strRTGSetName = dgvDevSpec.Item(2, eDevSPEC.TEMP_PROFILE).Value
						p_RTM(0).p_mltcp.TempPreset(1)	= p_RTM(0).p_rtgset.mdbr(0).dHPTemp
						p_RTM(0).p_mltcp.Temp2Preset(1) = p_RTM(0).p_rtgset.mdbr(0).dHPTemp
						p_RTM(0).p_mltcp.Temp3Preset(1) = p_clSpec.SGTempGet(p_RTM(0).p_rtgset.mdbr(0).dHPTemp)		'起歪体温度
					End If
				Else
					p_RTM(0).p_strRTGSetName = ""
				End If

			End Select
		Else
			pnlQR.Visible = False
		End If
	End If


	'デバッグ情報表示
	Lbl2.Visible = p_bScanTimeEn
	Lbl3.Visible = p_bScanTimeEn
	Lbl2.Text = p_RTM(0).p_lElapseTime              '通信スキャン時間


'InputDisableForMeasure(0)				'測定時入力禁止解除

Catch ex As Exception
	Me.Cursor = Cursors.Default
	Call FrmDebugLog(System.Reflection.MethodBase.GetCurrentMethod.Name & ex.Message)
	Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub
Public Shared Sub UnderStatusLabelDisp(ByRef tsslDisp As ToolStripLabel, ByVal sMes As String, Optional BC As Color = Nothing)
	tsslDisp.Text			= sMes
	tsslDisp.BackColor		= If(IsNothing(BC), Forms.Control.DefaultBackColor, BC)
End Sub
Public p_iQRState As int32 = -1
'================================================
'接続、未接続中のボタンの有効/ 無効を設定
'引数	bEna:			有効/無効フラグ
'戻り値 なし
'================================================
Private Sub sbConnectEnable(ByVal bEna As Boolean)
Try
	tsmiRemote.Enabled = bEna
	btnTemp.Enabled = bEna
	btnErrReset.Enabled = bEna

Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub

'''<summary>
'''ユーザーによる操作を許可する。
'''</summary>
Private Sub sbEnableFunctions()
#If Not NEW_SPEC
Try
	p_bBtnEnable = True

	MenuStrip1.Enabled = True
	''btnClearList.Enabled = True
	''btnDeselect.Enabled = True
	''btnRecalculate.Enabled = True
	''btnDispAprx.Enabled = True
	''btnDispReal.Enabled = True
	''btnDispRA.Enabled = True
	''btnGTStatRedraw.Enabled = True
	cboMeasName.Enabled = True
	cboSmplName.Enabled = True
	''cboSmplTime.Enabled = True
	cboMethod.Enabled = True
	''cboXaxis.Enabled = True
	''cboY1axis.Enabled = True
	''cboY2axis.Enabled = True
	cboSPECFix.Enabled = True
	''dgvMeasResult.Enabled = True
	dgvSpec.Enabled = True
	dgvCalcOpt.Enabled = True
	tsmiAbout.Enabled = True
	tsmiExit.Enabled = True
	tsmiFile.Enabled = True
	tsmiGraph.Enabled = True
	tsmiHelp.Enabled = True
	'tsmiImport.Enabled = True
	tsmiOptions.Enabled = True
	tsmiPrint.Enabled = True
	tsmiRecalculation.Enabled = True
	tsmiSettings.Enabled = True
	tsmiShowHelp.Enabled = True
	cboEquipName.Enabled = True
	tcSpecDisp.Enabled = True
#If FLAG_SHAREWARE = False Then
'sbForbitShareFunc()
#Else
	''btnUploadSpec.Enabled = True
	''btnDownloadSpec.Enabled = True
	btnTemp.Enabled = True
	btnZero.Enabled = True
	tsmiRemote.Enabled = True
	tsmiResetError.Enabled = True
	txtStirRod.Enabled = True
	txtAmount.Enabled = True
#End If
	''cboMeasName2.Enabled = True
	''cboSmplName2.Enabled = True
	''cboSmplTime2.Enabled = True
	''dgvSpec2.Enabled = True
	''txtStirRod2.Enabled = True
	''txtAmount2.Enabled = True
	''cboMethod2.Enabled = True
	''dgvCalcOpt2.Enabled = True
	btnErrReset.Enabled = True

Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
#end If
End Sub
'''<summary> 接続状態表示を行う </summary>
Private Sub sbRefreshStateLbl()
Static Dim iPrevConnect As Integer = -1
Static Dim bPreFlg As Boolean = False

Try
	'If p_RTM(0).p_iConnectState <> iPrevConnect Then
	Select Case p_RTM(0).p_iConnectState
	Case CONNECT_ST.OFFLINE         '未接続。
		lblConnectState.BackColor = Color.LightGray
		lblConnectState.Text = "OFFLINE"
		lblConnectState.ForeColor = Color.White
		pctConStts.BackColor = lblConnectState.BackColor
		p_RTM(0).p_iHeatingHP = -1
		p_RTM(0).p_iHeatingPB = -1
		p_RTM(0).p_iHeatingSG = -1
		p_RTM(0).p_iAdjustedZero = -1

	Case CONNECT_ST.ONLINE, CONNECT_ST.CONNECTING          '接続中。
		Select Case p_RTM(0).p_mltcp.IsLinkError()
		Case 0  'Link OK
			lblConnectState.Text = "ONLINE"
			lblConnectState.BackColor = Color.LightGreen
		Case Else 'Link Error
			lblConnectState.Text = "NO LINK"
			lblConnectState.BackColor = Color.Red
		End Select
		'lblConnectState.BackColor = Color.LightGreen
		'lblConnectState.Text = "ONLINE"
		lblConnectState.ForeColor = Color.Black
		pctConStts.BackColor = lblConnectState.BackColor

	Case CONNECT_ST.CANNOTCONNECT   '接続不可。
		lblConnectState.BackColor = Color.PeachPuff
		p_RTM(0).p_iConnectState = CONNECT_ST.OFFLINE
		lblConnectState.Text = "OFFLINE"
		lblConnectState.ForeColor = Color.White
		pctConStts.BackColor = lblConnectState.BackColor
		''m_iDrawPopup = 0
		''sbRedrawGraph()
		MessageBox.Show(p_sMes(efrmMainSTR.COULD_NOT_CONNECT) + vbCrLf + p_RTM(0).p_ro_strErr, "SIZUKA Tool", MessageBoxButtons.OK, MessageBoxIcon.Error)
		m_sbLog.AppendLine("Cannot connect:" + p_RTM(0).p_ro_strErr)
		MakeLogFile(m_sbLog.ToString())
	Case Else
		lblConnectState.BackColor = Color.LightGray
		lblConnectState.Text = "OFFLINE"
		lblConnectState.ForeColor = Color.White
		pctConStts.BackColor = lblConnectState.BackColor
		p_RTM(0).p_iHeatingHP = -1
		p_RTM(0).p_iHeatingPB = -1
		p_RTM(0).p_iHeatingSG = -1
		p_RTM(0).p_iAdjustedZero = -1
	End Select

	'Link時に有効なコントロールを設定
	Dim bFlg As Boolean = False
	If p_RTM(0).p_iConnectState = CONNECT_ST.CONNECTING Then
		bFlg = True
	End If
	If p_bBtnEnable = True Then
		sbConnectEnable(bFlg)
	End If

Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub
''' <summary>
''' TEMPボタンクリック
''' </summary>
''' <remarks></remarks>
Private Sub btnTemp_Click(sender As Object, e As EventArgs) Handles btnTemp.Click
Try
	If Not p_bError Then
		If p_RTM(0).p_iBGWOrder <> BGW_ORDER.NO_ORDER Then
			p_RTM(0).p_iBGWOrderNext = p_RTM(0).p_iBGWOrder
			p_RTM(0).p_iBGWOrder = BGW_ORDER.TEMP_BUTTON_REP
		Else
			p_RTM(0).p_iBGWOrder = BGW_ORDER.TEMP_BUTTON_REP
		End If
		If p_RTM(0).p_mltcp.isStatHeaterOn Or p_RTM(0).p_mltcp.isStatHeaterOn2
			UnderStatusLabelDisp(ToolStripStatusLabel1, "")					'クリア
			Call WriteLog("TEMPボタン(停止) " & System.Reflection.MethodBase.GetCurrentMethod.Name, eLOG_LEVEL.iINFO)
		Else
			UnderStatusLabelDisp(ToolStripStatusLabel1, p_sMes(efrmMainSTR.iTEMP_CONT_START))					'温度制御開始メッセージ
			Call WriteLog("TEMPボタン(開始) " & System.Reflection.MethodBase.GetCurrentMethod.Name, eLOG_LEVEL.iINFO)
		End If

	End If

Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub
''' <summary>
''' ゼロ調ボタンクリック
''' </summary>
''' <remarks></remarks>
Private Sub btnZero_Click(sender As Object, e As EventArgs) Handles btnZero.Click
Try
	ZeroAdj()
Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub
Public Function ZeroAdj() As int16
Dim lRet As int32
Try
	Select Case p_RTM(0).p_iBGWOrder
	Case BGW_ORDER.ZERO_BUTTON_CNF, BGW_ORDER.ZERO_BUTTON_0, BGW_ORDER.ZERO_BUTTON_1, BGW_ORDER.NO_ORDER
	'リアルタイム処理無実行、ゼロ調中の時
		UnderStatusLabelDisp(ToolStripStatusLabel1, "")			'クリア
		If p_RTM(0).p_mltcp.isILState()　Or p_RTM(0).p_mltcp.isILDisable			'IL(扉閉) OKか? IL無効も見る
			If p_RTM(0).p_mltcp.isStatOriginComp									'原点復帰完了済か？
				If p_bError Then
					UnderStatusLabelDisp(ToolStripStatusLabel1, p_sMes(efrmMainSTR.iErrorNotContinue))			'エラー発生中のため継続できません。
					Return -9
				End If
				btnMeasure.Enabled = False          '測定ボタンの無効化


				p_RTM(0).p_CaliblationState.iState = RealTimeMeas.eCalibState.NOT_START
				p_RTM(0).p_iZeroHPStart		= p_cts.dZeroStaPosHP * 1000
				p_RTM(0).p_iStaMonTime		= p_cts.iStaMonTime
				p_RTM(0).p_iStaFlucRange	= p_cts.iStaFlucRange
				p_RTM(0).p_dZstressCR		= p_RTM(0).p_dZStability			'Z安定度をラッチ

				p_RTM(0).p_mltcp.ZeroStartPos	= p_cts.dZeroStaPosHP * 1000
				p_RTM(0).p_mltcp.ZeroAdjPosX	= 0
				p_RTM(0).p_mltcp.ZeroAdjPosY	= 0
				Panel2.Location = New System.Drawing.Point(270, 140)
				Panel2.Size		= New System.Drawing.Size(710, 470)
				p_RTM(0).p_iBGWOrder = BGW_ORDER.ZERO_BUTTON
				If p_RTM(0).p_mltcp.IsZeroStartSW
					UnderStatusLabelDisp(ToolStripStatusLabel1, "")			'クリア
					Call WriteLog("Zeroボタン(停止) " & System.Reflection.MethodBase.GetCurrentMethod.Name, eLOG_LEVEL.iINFO)
				Else
					UnderStatusLabelDisp(ToolStripStatusLabel1, p_sMes(efrmMainSTR.iZERO_CONT_START))               'ゼロ調制御開始メッセージ
					Call WriteLog("Zeroボタン(開始) " & System.Reflection.MethodBase.GetCurrentMethod.Name, eLOG_LEVEL.iINFO)
				End If
				lRet = 0

			Else
				Call WriteLog("Zero開始エラー 原点復帰未実行 " & System.Reflection.MethodBase.GetCurrentMethod.Name, eLOG_LEVEL.iINFO)
				UnderStatusLabelDisp(ToolStripStatusLabel1, p_sMes(efrmMainSTR.iRETUN_ORIGIN))			'原点復帰が行われていません
				lRet = -2
			End If
		Else
			Call WriteLog("Zero開始エラー 扉開 " & System.Reflection.MethodBase.GetCurrentMethod.Name, eLOG_LEVEL.iINFO)
			UnderStatusLabelDisp(ToolStripStatusLabel1, p_sMes(efrmMainSTR.iDoorOpen))					'扉が開いています
			lRet = -3

		End If
	Case Else
	End Select
	Return lRet
Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Return -1
End Try
End Function

''' <summary>
''' 測定開始ボタンクリック
''' </summary>
''' <remarks></remarks>
Private Sub btnMeasure_Click(sender As Object, e As EventArgs) Handles btnMeasure.Click
Dim iRet As Int32
Dim f	As frmMSG = Nothing
Dim i As int32
Dim dAmp As Double
Dim dPeriod As Double
Try
	Select Case p_clSpec.spi.iStressSensMode
	Case eStrsSens.Line, eStrsSens.Circle
		'線または円
		dAmp	= p_clSpec.spi.dXAmplitude
		dPeriod = p_clSpec.spi.dXPeriod
	Case eStrsSens.Point
		'点
		dAmp	= p_clSpec.spi.dZAmplitude
		dPeriod = p_clSpec.spi.dZPeriod
	End Select

	'装置エラー発生中
	If p_bError
		'装置エラー発生中のため、測定開始できません
		f = New frmMSG(p_sMes(efrmMainSTR.iMeasStartConf1), {"OK"}, p_sMes(efrmMainSTR.iMeasStartConf), Me)
		f.ShowDialog()
		Exit Sub
	End If
	'デバイスSpecパラメータチェック
	iRet = p_clSpec.CheckDevSpecNumericValue(dgvDevSpec, True)
	If iRet<>0
		'デバイスSPECの入力エラーを確認してください
		f = New frmMSG(p_sMes(efrmMainSTR.iMeasStartConf2), {"OK"}, p_sMes(efrmMainSTR.iMeasStartConf), Me)
		f.ShowDialog()
		Exit Sub
	End If
	'サンプルSpecパラメータチェック
	iRet = p_clSpec.CheckSamSpecNumericValue(dgvSamSpec, True)
	If iRet<>0
		'サンプルSPECの入力エラーを確認してください
		f = New frmMSG(p_sMes(efrmMainSTR.iMeasStartConf3), {"OK"}, p_sMes(efrmMainSTR.iMeasStartConf), Me)
		f.ShowDialog()
		Exit Sub
	End If

	If p_clSpec.spi.iAmpMode=eAmpMode.StsConst AndAlso (p_clSpec.spi.iStressSensMode = eStrsSens.Circle Or p_clSpec.spi.iStressSensMode = eStrsSens.Line)
	'円・線の時 応力一定は出来ない
		'動作軌跡が円または線の場合は、応力一定を選択できません
		f = New frmMSG(p_sMes(efrmMainSTR.iMeasStartConf4), {"OK"}, p_sMes(efrmMainSTR.iMeasStartConf), Me)
		f.ShowDialog()
		Exit Sub
	End If
	If p_clSpec.spi.iHPPosCont=eHCont.ConstantP AndAlso p_clSpec.spi.iStressSensMode = eStrsSens.Point
	'点の時 常時圧力は出来ない
		'動作軌跡が点の場合は、常時圧力を選択できません
		f = New frmMSG(p_sMes(efrmMainSTR.iMeasStartConf5), {"OK"}, p_sMes(efrmMainSTR.iMeasStartConf), Me)
		f.ShowDialog()
		Exit Sub
	End If

	'長期間欠測定モード時の追加チェック
	If p_cts.LTModeEnable
		If Not p_clSpec.spi.iGelTimeMethod = eCalcMethod.THR
		'GelTime算出方法はTHRのみ
			'長期間欠測定モードではTHR以外は選択できません
			f = New frmMSG(p_sMes(efrmMainSTR.iMeasStartConf7), {"OK"}, p_sMes(efrmMainSTR.iMeasStartConf), Me)
			f.ShowDialog()
			Exit Sub
		End If
		If (p_cts.LTMeasureInterval * 60) < 10 * dPeriod + 3 + 1
		'測定インターバル時間は10周期＋安定待ち時間以上でないと開始できない
			'長期間欠測定モードのインターバル時間と動作周期の設定が不正です。
			f = New frmMSG(p_sMes(efrmMainSTR.iMeasStartConf8), {"OK"}, p_sMes(efrmMainSTR.iMeasStartConf), Me)
			f.ShowDialog()
			Exit Sub
		End If

		

	End If



	'フルキャリブテーブル0チェック
		'フルキャリブレーション結果を得る
		Dim frmC	As New frmCalibFull(0)
		Dim Clb()	As stPoint = Nothing
		frmC.sbFullStressCondRead(0, p_FClbX, p_FClbC, p_FClbP)			'ファイル読込み構造体配列へセット
		frmC.Dispose()
		Select Case p_clSpec.spi.iStressSensMode
		Case eStrsSens.Circle		'円
			Clb = p_FClbC
		Case eStrsSens.Line			'線
			Clb = p_FClbX
		Case eStrsSens.Point		'点
			Clb = p_FClbP
		End Select
		For i=0 To Clb.GetUpperBound(0)
			If Clb(i).iFullVal = 0
				Exit for
			End If
		Next
		If i<=Clb.GetUpperBound(0)
			'フル応力キャリブレーション設定内に０値が見つかったので測定開始できません。フル応力キャリブレーション設定を確認してください。
			f = New frmMSG(p_sMes(efrmMainSTR.iMeasStartConf6),{"OK"},p_sMes(efrmMainSTR.iMeasStartConf),Me)
			f.ShowDialog()
			Exit Sub
		End If



	'測定ボタンの無効化
	btnMeasure.Enabled = False
	'測定開始時
	If p_RTM(0).p_MeasureState = RealTimeMeas.eMeasStat.NOT_START Then

		'SPEC入力をSPEC構造体へ一括セット
		p_clSpec.dgvToSpi(eSpec.Device, dgvDevSpec, p_clSpec.spi)	
		p_clSpec.dgvToSpi(eSpec.Sample, dgvSamSpec, p_clSpec.spi)	
		
		ZeroResetProc()                                         '応力オートゼロ 有効
		p_RTM(0).p_CaliblationState.iState = RealTimeMeas.eCalibState.NOT_START
		p_cts.strAutoPrintOperatorValue = p_clspec.spi.sMeasurer						'測定者

		'測定者情報その他の格納
		p_RTM(0).p_sMeasurer		= p_clspec.spi.sMeasurer						'測定者
		p_RTM(0).p_sResinName		= p_clspec.spi.sSampleName						'樹脂名
		p_RTM(0).p_sLot				= p_clspec.spi.sLot								'LOT名
		p_RTM(0).p_dDiameter		= p_clspec.spi.dProbDiameter					'プローブ直径
		p_RTM(0).p_sCalcMethod		= lstCalcMethod(p_clspec.spi.iGelTimeMethod)	'ゲルタイム計算方法
		p_RTM(0).p_sEquipName		= cboEquipName.Text								'本体名
		p_RTM(0).p_sXYSensorID		= p_cts.sXYSensorSerial(p_cts.iSelMDK)			'XYセンサID
		p_RTM(0).p_sZSensorID		= p_cts.sZSensorSerial(p_cts.iSelMDK)			'ZセンサID

		p_RTM(0).p_iZPosition		= p_RTM(0).p_mltcp.ZeroPos_Z					'熱板ゼロ点位置(um)
		p_RTM(0).p_dWeightCoef		= p_cts.WeightCoef()							'dig->N換算係数
		'熱板温度許容範囲
		p_RTM(0).p_dTempStubRange	= p_clspec.spi.dHPTempOkRange
		p_RTM(0).p_dTempStubRange2	= p_clspec.spi.dPBTempOkRange
		'ゲルタイム判定範囲
		p_RTM(0).p_dJudgeRangeMin	= p_clspec.spi.dJudgeRangeMin
		p_RTM(0).p_dJudgeRangeMax	= p_clspec.spi.dJudgeRangeMax
		'マスク範囲
		p_RTM(0).p_dAprxRangeMin	= p_clspec.spi.dEffectRangeMin
		p_RTM(0).p_dAprxRangeMax	= p_clspec.spi.dEffectRangeMax
		p_RTM(0).p_strRTGSetName	= p_clspec.spi.sTempProfileName					'温度プロファイル名

		'スペックDGVをリンクエリアへセット
		sbSpecToLinkArea()

		'測定条件の100％応力較正値をセット(較正設定ファイルを読込み設定)
		'応力検知モードで選別
		Select Case p_clSpec.spi.iStressSensMode
		Case eStrsSens.Line, eStrsSens.Circle
			'線または円
			dAmp	= p_clSpec.spi.dXAmplitude
			dPeriod = p_clSpec.spi.dXPeriod
		Case eStrsSens.Point
			'点
			dAmp	= p_clSpec.spi.dZAmplitude
			dPeriod = p_clSpec.spi.dZPeriod
		End Select
		p_cts.SaveAsDeviceSettingINI(p_sEquipName)		'装置毎設定パラメータ保存
		iRet = modFrmMain.SetFullStress(dAmp, dPeriod, p_clspec.spi.iStressSensMode)				'キャリブレーション値を取得
		If iRet < 0
			modfrmMain.PreMesureOperationalGuideMsgBox(Me, 0)			'エラーメッセージ キャリブレーションデータが無い -> 測定中止
			Exit Sub
		End If
		p_cts.LoadDeviceSettingINI(p_sEquipName, False)		'装置毎の詳細設定の読込
		p_cts.LoadTempParaINI(p_sEquipName, False)			'温調パラメータ読込

		RealTimeThinning(0) = p_cts.iDetailsR()			'間引き設定
		RealTimeThinning(1) = p_cts.iDetailsD()			'間引き設定

		SetFilterSetting()								'フィルタ動作設定をセット

		p_dtFile.DataArrayFiFo(p_cts.LTModeEnable)      '履歴データFIFO処理
		p_dtFile.ReadBufferClear()                      'データバッファクリア
		p_ZeroJob = 9									'ゼロ調モニタを終了処理へ
		modfrmMain.ZeroSetProc(Me)                      '応力オートゼロ有効
		clExtra.AllDispClear(Chart1)                    '全ての補助線消去
		
		'ゲルタイム計算方法によりチャート系列を変更
		Select Case p_clSpec.spi.iGelTimeMethod
		Case eCalcMethod.THR, eCalcMethod.SLOPE
			rbCorrect.Checked	= True
			p_sSelectGItem		= rbCorrect.Name
			p_iSelectGItem		= Array.IndexOf(sGraphSelectionArry, p_sSelectGItem)
		Case eCalcMethod.PHASE
			rbPosPhase.Checked	= True
			p_sSelectGItem		= rbPosPhase.Name
			p_iSelectGItem		= Array.IndexOf(sGraphSelectionArry, p_sSelectGItem)
		Case eCalcMethod.MELT
			rbViscosity.Checked = True
			p_sSelectGItem		= rbViscosity.Name
			p_iSelectGItem		= Array.IndexOf(sGraphSelectionArry, p_sSelectGItem)
		Case eCalcMethod.EXTRA
			'外挿法
			clExtra.InitializeApp()
			clExtra.SetParameterTime(nudStartTim.Value, nudMidTim.Value, nudEndTim.Value)           '開始、中間点、終了点 初期設定
			rbCorrect.Checked	= True
			p_sSelectGItem		= rbCorrect.Name
			p_iSelectGItem		= Array.IndexOf(sGraphSelectionArry, p_sSelectGItem)
		End Select
		
		'測定数値結果非表示
		lblResult1Val.Visible	= False
		lblResult2Val.Visible	= False
		lblResult3Val.Visible	= False
		lblResult1.Visible		= False
		lblResult2.Visible		= False
		lblResult3.Visible		= False
		m_RealTrendUpdate		= False                           'リアルタイムチャート初期化

		'数値モニタ表示(計算)
		pnlPosition.Visible		= False
		pnlCalculation.Visible	= Not pnlPosition.Visible		

		InputDisableForMeasure(1)					'測定時入力禁止設定

		'温度プロファイル制御スレッド開始
		If bgwInterrupt.IsBusy = False
			If p_clspec.spi.sTempProfileName<>""
				bgwInterrupt.RunWorkerAsync()		'温度プロファイルスレッド開始
			End If
		End If
		Select Case p_cts.LTModeEnable
		Case False
			'長時間測定表示制御
			LongTimeModeDispCont(True)
			p_RTM(0).p_iBGWOrder = BGW_ORDER.START_BUTTON
		Case Else
		'長時間間欠測定
			p_RTM(0).p_iBGWOrder = BGW_ORDER.LT_START_BUTTON
		End Select
		'長時間測定表示制御
		LongTimeModeDispCont(True)
		'着圧指定時の制御失敗ラッチ変数リセット
		p_iFailedOrderP = 0
		'ガイドメッセージラッチ変数リセット
		p_iGuideXYbeforeGap = 0
		Call WriteLog("Measボタン(開始) " & System.Reflection.MethodBase.GetCurrentMethod.Name, eLOG_LEVEL.iINFO)
	Else
		'InputDisableForMeasure(0)					'測定時入力禁止解除
		'温度プロファイル制御スレッド停止
		If bgwInterrupt.IsBusy Then
			bgwInterrupt.CancelAsync()				'温度プロファイルスレッド停止
		End If
		'長時間測定表示制御
		LongTimeModeDispCont(False)

		Select Case p_cts.LTModeEnable
		Case False
			p_RTM(0).p_iBGWOrder = BGW_ORDER.CANCEL_BUTTON
		Case True
			p_RTM(0).p_iBGWOrder = BGW_ORDER.LT_CANCEL_BUTTON
		End Select
		Call WriteLog("Measボタン(停止) " & System.Reflection.MethodBase.GetCurrentMethod.Name, eLOG_LEVEL.iINFO)
	End If

	modFrmMain.sbMeasureBottonStateDisp(Me)                              '測定開始/停止 表示切替
	btnMeasure.Enabled = True

Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	InputDisableForMeasure(0)				'測定時入力禁止解除
End Try
End Sub
'温度プロファイル制御スレッド停止
Public Sub StopTempInterrupt()
	If bgwInterrupt.IsBusy Then
		bgwInterrupt.CancelAsync()				'温度プロファイルスレッド停止
	End If
End Sub
''' <summary>
''' 長時間測定時の表示コントロール有効無効設定
''' </summary>
''' <param name="bMeas"></param>
Private Sub LongTimeModeDispCont(ByVal bMeas As Boolean)
	If p_bMeasTimeEn
	'測定時間 無制限のとき
		If bMeas
			Chart1.Visible				= False
			lblYAuto.Visible			= False
			lblXAuto.Visible			= False
			lblChartCoordinate.Visible	= False
			btnThinning.Enabled			= False
			lblLongTimeMode.Visible		= True
		Else
			Chart1.Visible				= True
			lblYAuto.Visible			= True
			lblXAuto.Visible			= True
			lblChartCoordinate.Visible	= True
			btnThinning.Enabled			= True
			lblLongTimeMode.Visible		= False
		End If
	Else		
		Chart1.Visible				= True
		lblYAuto.Visible			= True
		lblXAuto.Visible			= True
		lblChartCoordinate.Visible	= True
		btnThinning.Enabled			= True
		lblLongTimeMode.Visible		= False

	End If
End Sub
'測定時入力禁止/許可
Private Sub InputDisableForMeasure(ByVal iMode As int16)
	Select Case iMode
	Case 0	'測定終了以降
		pnlSPEC.Enabled			= True
		cboEquipName.Enabled	= cbxEquipEn
		tsmiFile.Enabled		= True
		tsmiMeasure.Enabled		= True
		tsmiSettings.Enabled	= True
		btnAllClear.Enabled		= True

	Case 1	'測定開始時
		If p_bInputEnMeasureing		'測定時の入力許可スイッチ
		Else
			pnlSPEC.Enabled			= False
			cboEquipName.Enabled	= False
			tsmiFile.Enabled		= False
			tsmiMeasure.Enabled		= False
			tsmiSettings.Enabled	= False
			btnAllClear.Enabled		= False
		End If

	End Select
End Sub
'エラーリセットボタン
Private Sub btnErr_Click(sender As Object, e As EventArgs) Handles btnErrReset.Click
	Try
		If p_bError Then
			Exit Sub
		End If
		If p_RTM(0).p_iBGWOrder = BGW_ORDER.NO_ORDER
			p_RTM(0).p_iBGWOrder = BGW_ORDER.RESET_ERROR_REP
		End If
	Catch ex As Exception
		Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
		Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	End Try
End Sub
#If 0
''' <summary>
''' 原点復帰要求ボタン
''' </summary>
Private Sub btnOrigin_Click(sender As Object, e As EventArgs) Handles btnOrigin.Click
	Try
		If p_bError Then
			Exit Sub
		End If
		btnZero.Enabled = False             'ゼロ調ボタンの無効化
		btnMeasure.Enabled = False          '測定ボタンの無効化

		Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name, eLOG_LEVEL.iINFO)

		p_RTM(0).p_iBGWOrder = BGW_ORDER.ORG_BUTTON_REP

	Catch ex As Exception
		Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
		Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	End Try
End Sub
#end if
'フィルタ動作設定
Public Sub SetFilterSetting()
	Try
		p_RTM(0).p_FltSetting.iEnable			= p_cts.iFilterEnable      'フィルタ有効
		p_RTM(0).p_FltSetting.sKind				= p_cts.sFilterKind        'フィルタ種類 FIR/IIR
		p_RTM(0).p_FltSetting.dFIRCutOffFreq	= p_cts.dFIRCutOffFreq     'FIR カットオフ周波数
		p_RTM(0).p_FltSetting.iFIRFilLen		= p_cts.iFilterLen         'FIR パラメータ長
		p_RTM(0).p_FltSetting.sFIRWinKind		= p_cts.sFilterWinKind     'FIR 窓関数種
		p_RTM(0).p_FltSetting.dIIRCutOffFreq	= p_cts.dIIRCutOffFreq     'IIR カットオフ周波数
		p_RTM(0).p_FltSetting.iIIRDeg			= p_cts.iIIRDeg            'IIR フィルタ次数

		''p_RTM(0).p_FltSetting.dFIRCutOffFreq	= MEAS_SAMPLE_FREQ			'FIR サンプル周波数
		p_RTM(0).p_FltSetting.dFIRSmplFreq		= MEAS_SAMPLE_FREQ			'FIR サンプル周波数

		'p_RTM(0).p_FltSetting.sFileName = p_cts.sFilterFileName		'IIR パラメータファイル名
	Catch ex As Exception

	End Try
End Sub

Private Sub cboEquipName_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboEquipName.SelectedIndexChanged
Try
	If bFrmNew
		Exit Sub
	End If
	If p_RTM(0).p_sEquipName = cboEquipName.SelectedItem
		Exit Sub
	End If
	If Not IsNothing(p_RTM) 
		If p_RTM(0).p_iBGWorder=BGW_ORDER.NO_ORDER AND p_RTM(0).p_iBGWOrderNext=BGW_ORDER.NO_ORDER
			'装置名を変更した場合にLog出力
			Call WriteLog("装置名が変更されました : " & cboEquipName.SelectedItem, eLOG_LEVEL.iINFO)

			'コンボボックス変更時に、設定を変更する。(保存はしない)
			p_cts.iSelMDK = cboEquipName.SelectedIndex
			If p_RTM(0).p_ConnectAfterProc = 0
				p_RTM(0).p_iBGWWork = BGW_ORDER.DISCONNECT			'装置番号変更のみの場合は切断無し
			End If
			p_RTM(0).p_sEquipName	= cboEquipName.Text
			p_sEquipName			= cboEquipName.Text
			p_RTM(0).p_strEquipIP	= p_cts.strIP(p_cts.iSelMDK)
			p_RTM(0).p_sXYSensorID	= p_cts.sXYSensorSerial(p_cts.iSelMDK)
			p_RTM(0).p_sZSensorID	= p_cts.sZSensorSerial(p_cts.iSelMDK)
			p_RTM(0).p_strEquipPort = p_cts.strPort
			p_RTM(0).p_iAutoStart	= p_cts.iAutoStart

			'初期化ファイルの切替読込み
			modFrmMain.SetForderCreate()								'データ管理フォルダ環境生成
			'初期化ファイルを読込む
			p_cts.LoadDeviceSettingINI(modPublic.p_sEquipName, False)			'装置毎設定読込
			p_cts.LoadTempParaINI(modPublic.p_sEquipName, False)				'温調パラメータ読込

			'重量変換パラメータセット
			Dim frmC As New frmCalibWeight3					'キャリブレーションデータパスの設定が行われる
			frmC.WeightParameterFileGet(p_dNConv, p_dZConv, p_XYCoefMeasDate, p_ZCoefMeasDate)
			frmC.Dispose()
			frmC = Nothing

			'測定データパスの設定
			p_dtFile.InitDataPath()


		End If
	End If

Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub
'フォームキーダウン(キー入力監視用)
Private Sub frmMain_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
	m_bIsPressKey = True
	sw_KeyObs.Restart()

	If (e.Modifiers And Keys.Control) = Keys.Control Then
		If e.KeyCode = Keys.K Then								'CTRL＋K デバッグ画面
			frmDebug.Show()
		End If
	End If
End Sub
#If 0
'================================================
'ラベルオブジェクトの背景色を点滅させる
'引数	lbl:			ラベルオブジェクト
'		sText:			セットする文字列
'戻り値 なし
'================================================
Public Sub sbObjBackClrBlink(ByRef oBlink As Object, ByVal clr1 As Color, ByVal clr2 As Color, ByVal bColor As Boolean)

	Try
		'背景色の入れ替え
		If bColor Then
			oBlink.BackColor = clr2
		Else
			oBlink.BackColor = clr1
		End If
		'文字色の設定
		oBlink.ForeColor = GetForeColor(oBlink.BackColor)

	Catch ex As Exception
		Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
		Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	End Try
End Sub

'================================================
'DGV特定セルの背景色を点滅させる
'引数	DGV:			オブジェクト

'		sText:			セットする文字列
'戻り値 なし
'================================================
Public Sub sbDgvBackClrBlink(ByRef oBlink As Object, ByVal c As Int16, ByVal r As Int16, ByVal clr1 As Color, ByVal clr2 As Color)
	Try
		'背景色の入れ替え
		If oBlink(c, r).Style.BackColor = clr1 Then
			oBlink(c, r).Style.BackColor = clr2
		Else
			oBlink(c, r).Style.BackColor = clr1
		End If

		'文字色の設定
		oBlink(c, r).Style.ForeColor = GetForeColor(oBlink(c, r).Style.BackColor)

	Catch ex As Exception
		Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
		Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	End Try
End Sub
#end if
'測定データ選択ラジオボタンクリック
Private Sub rbCorrect_Click(sender As Object, e As EventArgs) Handles rbYStress.Click, rbZStress.Click, rbXStress.Click, rbZPosition.Click, rbPBTemp.Click, rbPBMV.Click, rbHPTemp.Click, rbHPMV.Click, rbCorrect.Click, rbCorrectP.Click, rbViscosity.Click, rbStorageMod.Click, rbShearMod.Click, rbShearVelo.Click, rbSampleStrain.Click, rbPosPhase.Click, rbLossTangent.Click, rbElasticMod.Click
	p_sSelectGItem = sender.Name
	p_iSelectGItem = Array.IndexOf(sGraphSelectionArry, p_sSelectGItem)
	nudYscaleMin.Value				= p_cts.MainGraphYMin(p_iSelectGItem)	'Y軸SPAN
	nudYscaleMax.Value				= p_cts.MainGraphYMax(p_iSelectGItem)	'Y軸SPAN
	nudYdivision.Value				= p_cts.MainGraphYDiv(p_iSelectGItem)	'Y軸目盛り分割数

	Select Case p_RTM(0).p_iBGWOrder
	Case BGW_ORDER.MEAS_PROCESS
	'測定中
		m_RealTrendUpdate = False
		ChartDispUpdate(Me, 1, m_RealTrendUpdate)				'チャート表示更新
	Case Else
	'測定後
		m_RealTrendUpdate = False
		ChartDispUpdate(Me, 0, m_RealTrendUpdate)               'チャート表示更新
	End Select
End Sub
''' <summary>
''' フォームサイズ変更
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub frmMain_Resize(sender As Object, e As EventArgs) Handles MyBase.Resize

End Sub
'グラフ再計算-描画
Private Sub tsmiRedraw_Click(sender As Object, e As EventArgs) Handles tsmiRecalculation.Click
	ReCalclationMainProc( Me )
End Sub

''' <summary>
''' オンラインオフラインステータス表示ラベルクリック 
''' 手動切断->自動接続
''' </summary>
Private Sub lblConnectState_Click(sender As Object, e As EventArgs) Handles lblConnectState.Click
	If p_RTM(0).p_iConnectState = CONNECT_ST.CONNECTING Then
		p_RTM(0).p_iBGWOrder = BGW_ORDER.DISCONNECT_LINK
	Else
		p_RTM(0).p_iBGWOrder = BGW_ORDER.CONNECT_LINK
	End If
End Sub
Const ZPOSDEF	As Int16 = 1000			'Z位置温度補正分解能(m秒)
'================================================
'割り込み処理スレッド 温度プロファイル処理
'================================================
Private Sub bgwInterrupt_DoWork(sender As Object, e As DoWorkEventArgs) Handles bgwInterrupt.DoWork
Dim iPrfIndex	As Integer = -1
Dim iPrePrfIndex As Integer = -1
Dim dtNow		As Date
Dim dtStart		As Date
Dim dtUpdate	As Date
Dim dtUpdate1	As Date
Dim dTemp		As Double
Dim dPreTemp	As Double
Dim bytTemp()	As Byte = Nothing
Dim dTempInc	As Double
Dim sw1			As New Stopwatch
Dim sw2			As New Stopwatch
Dim iJob		As Int16 = 0
Dim dZPosCal	As Double
Dim dPreZPosCal	As Double = 0
Dim i1SCount	As Int32				'Z位置温度補正分解能(秒)カウント
Dim iZDrvJob	As Int16 = -1			'Z軸駆動制御
Dim dHPPos		As Double
Dim dPreHPPos	As Double

Dim iPrsCJob	As Int16 = -1			'着圧制御時のZ位置温度補正制御

'スレッドの設定
Me.bgwInterrupt.WorkerReportsProgress = True
Me.bgwInterrupt.WorkerSupportsCancellation = True

m_blBgwAlive_Interrupt = True       'スレッド生存中

Do
Try
	System.Threading.Thread.Sleep(90)
	'常時読込み 停止
	If bgwInterrupt.CancellationPending Then
		e.Cancel = True
		Exit Do
	End If

	'現在日時取得
	dtNow = Now

	Select Case iJob
	Case 0
		Select Case p_cts.p_ZCalibTempPro
		Case -1
			'温度プロファイル開始検知 測定ステータスがデータ転送中 かつ 測定データ有効
			If p_RTM(0).p_MeasureState = eMeasStat.DATA_TRANS AndAlso p_RTM(0).p_mltcp.bMesDataEnable Then
				If iPrfIndex < 0 Then
					'温度プロファイルが実行されている場合のみ
					If p_RTM(0).p_rtgset.dTiming(0) > 0 Then
						'温度プロファイル開始日時、インデックス初期化
						dtStart = dtNow
						dtUpdate = dtNow.AddYears(1)        '更新時間をありえない日時に設定
						Console.WriteLine(vbCrLf & "温度プロファイル開始")
						iPrfIndex = 0

						i1SCount = 0
						sw1.Restart
						sw2.Restart
						dHPPos = 0
						iPrsCJob = -1
						iJob = 10

					End If
				End If
			Else
				If iPrfIndex >= 0 Then
					iPrfIndex = -1
				End If
			End If
		Case Else
			If p_RTM(0).p_iZeroAdjOk = 1
			'ゼロ調完了した
				If iPrfIndex < 0 Then
					'温度プロファイルが実行されている場合のみ
					If p_RTM(0).p_rtgset.dTiming(0) > 0 Then
						'温度プロファイル開始日時、インデックス初期化
						dtStart = dtNow
						dtUpdate = dtNow.AddYears(1)        '更新時間をありえない日時に設定
						Console.WriteLine(vbCrLf & "温度プロファイル開始")
						iPrfIndex = 0
						sw1.Restart
						iPrsCJob = -1
						i1SCount = 0
						iJob = 10

					End If
				End If
			End If
		End Select

	Case 10
		'書込み温度の初期化
		dTemp = -1

		'温度プロファイル中の場合
		If iPrfIndex >= 0 AndAlso iPrfIndex < m_iRTGSETMaxRow Then
			'現在時刻から判定
#If 1
			Select Case dtNow
			'変更点に到達
			Case Is >= dtStart.Addseconds(p_RTM(0).p_rtgset.dTiming(iPrfIndex))
				Debug.Print(".p_rtgset.dTiming(iPrfIndex)=" & p_RTM(0).p_rtgset.dTiming(iPrfIndex))
				Debug.Print("dtStart.AddSeconds(.p_rtgset.dTiming(iPrfIndex))=" & dtNow.Subtract(dtStart).TotalSeconds)
				'更新温度の設定
				dTemp = p_RTM(0).p_rtgset.mdbr(iPrfIndex).dHPTemp
				dPreTemp = dTemp

				'更新時間の取得
				dtUpdate = dtNow

				'前回インデックス設定
				iPrePrfIndex = iPrfIndex
				'次のインデックスをMAXで初期化
				iPrfIndex = m_iRTGSETMaxRow
				For iCnt = iPrePrfIndex + 1 To m_iRTGSETMaxRow - 1
					'指定時間が0以上の場合のインデックスを取得
					If p_RTM(0).p_rtgset.dTiming(iCnt) > 0 Then
						'次インデックス設定
						iPrfIndex = iCnt
						'線形の場合の1秒毎の上がり幅を設定
						dTempInc = (p_RTM(0).p_rtgset.mdbr(iPrfIndex).dHPTemp - dTemp) / (p_RTM(0).p_rtgset.dTiming(iPrfIndex) - p_RTM(0).p_rtgset.dTiming(iPrePrfIndex))
						'ループを抜ける
						Exit For
					End If
				Next
			'1秒周期でアップロード
			Case Is >= dtUpdate.AddMilliseconds(ZPOSDEF)
				'線形補間設定の場合
				If p_RTM(0).p_rtgset.iLinearFlag(iPrfIndex) > 0 Then
					'Debug.Print("Linear=" & dtStart.Subtract(Now).TotalSeconds)
					'更新温度の設定
					dTemp = dPreTemp + dTempInc * (dtNow.Subtract(dtUpdate).TotalMilliseconds/1000)
					dPreTemp = dTemp
					'更新時間の取得
				End If
				dtUpdate = dtNow
				Debug.Print("dTemp=" & dTemp & " 1SCount=" & i1SCount)
				Debug.Print("dtStart=" & dtStart & " dtNow=" & dtNow)

			End Select
#end if
			Select Case p_cts.p_ZCalibTempPro
			Case -1
				'Z位置 温度補正計算 -----------------------------------------------------------------------------------------------------
				If IsNothing(p_RTM(0).p_rtgset.dZCalibPos)
					'iJob = 0
					'キャリブレーションなしの場合はZ駆動を行わない
					iZDrvJob = -1
					dZPosCal = 0
					dPreZPosCal = 0 
				Else
					If i1SCount <= p_RTM(0).p_rtgset.dZCalibPos.GetUpperBound(0)
					'補正配列確認
						Select Case p_clSpec.spi.iHPPosCont
						Case eHCont.Gap				'GAP制御
							If sw1.Elapsed.TotalMilliseconds >= ZPOSDEF
								dZPosCal = (p_RTM(0).p_mltcp.ZeroPos_Z - p_clSpec.spi.dMeasureGap*1000.0) - p_RTM(0).p_rtgset.dZCalibPos(i1SCount)
								Console.WriteLine("GAP ZPosSet=" & dZPosCal & Space(1) & p_RTM(0).p_rtgset.dZCalibPos(i1SCount) & Space(1) & i1SCount)
								i1SCount += 1
								sw1.Restart()
							End If
						Case eHCont.OrderP			'着圧制御
							Select Case iPrsCJob
							Case -1
							'上昇待ち時間処理しない
								If sw2.Elapsed.TotalSeconds > p_clSpec.spi.dInputToHPUpWait
									dPreHPPos = Ctype(p_RTM(0).p_mltcp.HeatBPos, Double)
									sw2.Restart()
									iPrsCJob = 0						
								End If
								If sw1.Elapsed.TotalMilliseconds >= ZPOSDEF
									i1SCount += 1
									sw1.Restart()
								End If
							Case 0
							'同値を検出確認
								If sw2.Elapsed.Totalseconds >= 2
									If dPreHPPos <> Ctype(p_RTM(0).p_mltcp.HeatBPos, Double)
										dPreHPPos = Ctype(p_RTM(0).p_mltcp.HeatBPos, Double)
										sw2.Restart()
									Else
									'同値検出
										dHPPos = Ctype(p_RTM(0).p_mltcp.HeatBPos, Double)		'Z位置確定
										iPrsCJob = 10						
									End If
								End If
								If sw1.Elapsed.TotalMilliseconds >= ZPOSDEF
									i1SCount += 1
									sw1.Restart()
								End If
					
							Case 10
							'補正値計算
								If sw1.Elapsed.TotalMilliseconds >= ZPOSDEF
									dZPosCal = dHPPos - p_RTM(0).p_rtgset.dZCalibPos(i1SCount)	'Z位置補正
									Console.WriteLine("PRSC ZPosSet=" & dZPosCal & Space(1) & p_RTM(0).p_rtgset.dZCalibPos(i1SCount) & Space(1) & i1SCount)
									i1SCount += 1
									sw1.Restart()
								End If

							Case Else
								iPrsCJob = -1
							End Select
						End Select
					Else
					'プロファイルプログラム終了
						e.Result = "COMPLETE"
						Exit Do
					End If
				End If
				'Z位置 温度補正計算 -----------------------------------------------------------------------------------------------------
			Case Else
				'Z位置補正値作成
				Select Case iPrsCJob
				Case -1
					iPrsCJob = 0
				Case 0
					dHPPos = Ctype(p_RTM(0).p_mltcp.ZeroPos_Z, Double)		'Z位置確定(初回)
					Dim bRet As Boolean
					If i1SCount < p_RTM(0).p_rtgset.dTTotal
						'作成日時行
						bRet = foptDlg.fcSaveRTGSETContents(p_RTM(0).p_rtgset.strName, 1)
						'１回目の測定行
						bRet = foptDlg.fcSaveRTGSETContents(p_RTM(0).p_rtgset.strName, 3)		'セクション名書込み
						bRet = foptDlg.fcSaveRTGSETContents(p_RTM(0).p_rtgset.strName, 0, p_RTM(0).p_mltcp.HeatTempAct, dHPPos - Ctype(p_RTM(0).p_mltcp.ZeroPos_Z, Double), i1SCount)
						i1SCount += 1
						iPrsCJob = 1
					End If
				Case 1
					Dim bRet As Boolean
					If sw1.Elapsed.TotalMilliseconds >= ZPOSDEF
						sw1.Restart()
						If i1SCount < p_RTM(0).p_rtgset.dTTotal
							'1行追加モードで書き出し
							bRet = foptDlg.fcSaveRTGSETContents(p_RTM(0).p_rtgset.strName, 0, p_RTM(0).p_mltcp.HeatTempAct, dHPPos - Ctype(p_RTM(0).p_mltcp.ZeroPos_Z, Double), i1SCount)
							i1SCount += 1
						Else
						'データ採取終了
							iPrsCJob = 10
						End If
					End If
				Case 10
					'データ採取終了処理
					'キャリブレーション有効行
					foptDlg.fcSaveRTGSETContents(p_RTM(0).p_rtgset.strName, 2)
					e.Result = "Z COMPLETE"
					Exit Do
				Case Else
				End Select
			End Select
		Else
			If p_cts.p_ZCalibTempPro=-1
			'プロファイルプログラム終了
				e.Result = "COMPLETE"
				Exit Do
			Else
				iPrsCJob = -1
			End If
		End If

		'書込み値が設定されている場合
		If dTemp >= 0 Then
			'現在の割り込みがない場合
			If p_bInterrupt = False Then
				'パラメータの更新配列の設定
				'書込み電文を取得
				''WriteLog("Temperature profile value set : " & dTemp.ToString("0.0"), eLOG_LEVEL.iINFO)
				p_RTM(0).p_mltcp.TempPreset(1) = dTemp			'HP温度
				p_RTM(0).p_mltcp.Temp2Preset(1) = dTemp			'PB温度
				p_RTM(0).p_mltcp.Temp3Preset(1) = p_clSpec.SGTempGet(dTemp)		'起歪体温度
				''WriteLog("Temperature profile value set : OK", eLOG_LEVEL.iINFO)
			End If
		End If

		Select Case p_cts.p_ZCalibTempPro
		Case -1
		'Z位置温度補正駆動処理 -----------
			Select Case iZDrvJob
			Case -1
				'Z位置補正値があり、前回値と異なる
				If dZPosCal > 0 AndAlso dZPosCal <> dPreZPosCal
					p_RTM(0).p_mltcp.PosHeatB = CType(dZPosCal, int64)			'Z目標位置設定
					p_RTM(0).p_mltcp.PosOrderZ(1)								'Z駆動指令ON
					iZDrvJob = 0
				End If
			Case 0 To 9
				If iZDrvJob >= 1
					iZDrvJob = 10
				Else
					iZDrvJob += 1
				End If
			Case 10		'200mS後
				Console.WriteLine("ZPos Move")
				p_RTM(0).p_mltcp.PosOrderZ(0)									'Z駆動指令OFF
				dPreZPosCal = dZPosCal											'前回駆動Z位置を退避
				iZDrvJob = -1
			Case Else
				iZDrvJob = -1
			End Select
			'---------------------------------
			If p_RTM(0).p_MeasureState = eMeasStat.NOT_START Then
				p_RTM(0).p_mltcp.TempPreset(1)	= p_RTM(0).p_rtgset.mdbr(0).dHPTemp		'HP温度
				p_RTM(0).p_mltcp.Temp2Preset(1) = p_RTM(0).p_rtgset.mdbr(0).dHPTemp		'PB温度
				p_RTM(0).p_mltcp.Temp3Preset(1) = p_clSpec.SGTempGet(p_RTM(0).p_rtgset.mdbr(0).dHPTemp)		'起歪体温度
				e.Result = "DEVSTOP"
				Exit Do
			End If
			'測定リミット時間[秒]のチェック(プロファイル途中でも終了)
			If sw1.Elapsed.TotalSeconds >= p_RTM(0).p_dMeasTimeLimit
				e.Result = "LIMIT"
				Exit Do

			End If
		Case Else
		'Z位置補正値作成
			If dtNow >= dtUpdate1.AddMilliseconds(ZPOSDEF)
				If dHPPos = 0
					dHPPos = Ctype(p_RTM(0).p_mltcp.ZeroPos_Z, Double)
				End If
				p_RTM(0).p_rtgset.dCurrPreTemp = p_RTM(0).p_mltcp.TempPreset(1)
				p_RTM(0).p_rtgset.dCurrPosDevi = dHPPos - Ctype(p_RTM(0).p_mltcp.ZeroPos_Z, Double)
				p_RTM(0).p_rtgset.iElaspeTime = i1SCount
				If i1SCount < p_RTM(0).p_rtgset.dTTotal
					If iPrsCJob = -1 
						foptDlg.fcSaveRTGSETContents(p_RTM(0).p_rtgset.strName, 0, p_RTM(0).p_mltcp.HeatTempAct, dHPPos - Ctype(p_RTM(0).p_mltcp.ZeroPos_Z, Double), i1SCount)
						i1SCount += 1
					End If
				Else
					'データ採取終了処理
					foptDlg.fcSaveRTGSETContents(p_RTM(0).p_rtgset.strName, 2)			'キャリブレーション有効行更新
					e.Result = "Z COMPLETE"
					Exit Do
				End If
				dtUpdate1 = Now
			End If
		End Select
	End Select
	If p_RTM(0).p_iError = 1
	'エラー発生したらスレッド終了
		e.Result = "ERR ABORT"
		Exit Do
	End If
Catch ex As Exception
	Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Call FrmDebugLog(System.Reflection.MethodBase.GetCurrentMethod.Name & ex.Message)
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
'debug.Print(sw1.Elapsed.TotalMilliseconds)
Loop
p_RTM(0).p_mltcp.PosOrderZ(0)									'Z駆動指令OFF

m_blBgwAlive_Interrupt = False          'スレッド終了
End Sub
Private Sub bgwInterrupt_RunWorkerCompleted(sender As Object, e As RunWorkerCompletedEventArgs) Handles bgwInterrupt.RunWorkerCompleted
Try
	p_cts.p_ZCalibTempPro = -1
	'初期温度
	p_RTM(0).p_mltcp.TempPreset(1)	= p_RTM(0).p_rtgset.mdbr(0).dHPTemp		'HP温度
	p_RTM(0).p_mltcp.Temp2Preset(1) = p_RTM(0).p_rtgset.mdbr(0).dHPTemp		'PB温度
	p_RTM(0).p_mltcp.Temp3Preset(1) = p_clSpec.SGTempGet(p_RTM(0).p_rtgset.mdbr(0).dHPTemp)		'起歪体温度
	Select Case e.Result
	Case "Z COMPLETE"
		p_RTM(0).p_iBGWOrder = BGW_ORDER.ZERO_BUTTON
		Call WriteLog("温度プロファイル キャリブレーション終了", eLOG_LEVEL.iINFO)

	Case "COMPLETE" ,"LIMIT"
		'測定終了
		p_RTM(0).p_iBGWOrder		= BGW_ORDER.CANCEL_BUTTON			'測定手動終了中止制御
		p_RTM(0).p_iBGWOrderNext	= BGW_ORDER.CANCEL_BUTTON			'測定手動終了中止制御
		Call WriteLog("温度プロファイル プログラム終了(または時間制限)", eLOG_LEVEL.iINFO)

	Case "ERR ABORT","DEVSTOP"
		p_RTM(0).p_mltcp.ZeroSW(False)
		p_RTM(0).p_mltcp.MeasSW(False)
		Call WriteLog("温度プロファイル キャリブレーション終了(中止またはエラー発生)", eLOG_LEVEL.iINFO)
	End Select
	
Catch ex As Exception
	Call FrmDebugLog(System.Reflection.MethodBase.GetCurrentMethod.Name & ex.Message)
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub
'================================================
'トレンド更新処理スレッド
'================================================
Private Sub bgwTrendUpdate_DoWork(sender As Object, e As DoWorkEventArgs) Handles bgwTrendUpdate.DoWork
Dim iPrfIndex		As Integer = -1
Dim iPrePrfIndex	As Integer = -1
Dim dtNow			As Date
Dim iColor			As Color

'スレッドの設定
Me.bgwTrendUpdate.WorkerReportsProgress = True
Me.bgwTrendUpdate.WorkerSupportsCancellation = True

m_blBgwAlive_Trend = True       'スレッド生存中
Dim bPreVisible As Boolean = False

Do
Try
	'sw1.Restart
	'常時読込み 停止
	If bgwTrendUpdate.CancellationPending Then
		e.Cancel = True
		Exit Do
	End If
	Select Case p_RTM(0).p_iBGWOrder
	Case BGW_ORDER.MEAS_PROCESS
		lblElapsedVal.SetTextEx(p_RTM(0).p_ElapsedMeasTime.ToString("F1"), lblElapsedVal.BackColor)                '測定経過時間表示
	Case BGW_ORDER.LT_MEAS_PROCESS_WAIT,BGW_ORDER.LT_MEAS_STAB_WAIT,BGW_ORDER.LT_MEAS_PROCESS,BGW_ORDER.LT_DATACAL 
		lblElapsedVal.SetTextEx(p_RTM(0).p_ElapsedMeasTime.ToString("F0"), lblElapsedVal.BackColor)                '測定経過時間表示
	Case Else
	End Select

	'現在日時取得
	dtNow = Now
	'リアルタイム値更新
	lblCurrHPTempVal.SetTextEx(p_RTM(0).p_mltcp.HeatTempAct.ToString("F1"),		lblCurrHPTempVal.BackColor)		'HP温度
	lblCurrPBTempVal.SetTextEx(p_RTM(0).p_mltcp.HeatTemp2Act.ToString("F1"),	lblCurrPBTempVal.BackColor)		'PB温度
	lblCurrSGTempVal.SetTextEx(p_RTM(0).p_mltcp.HeatTemp3Act.ToString("F1"),	lblCurrSGTempVal.BackColor)		'SG温度
	lblCurrHPMVVal.SetTextEx(p_RTM(0).p_mltcp.HeatTempOpe.ToString("F1"),		lblCurrHPMVVal.BackColor)		'HP MV値
	lblCurrPBMVVal.SetTextEx(p_RTM(0).p_mltcp.HeatTemp2Ope.ToString("F1"),		lblCurrPBMVVal.BackColor)		'PB MV値
	lblCurrSGMVVal.SetTextEx(p_RTM(0).p_mltcp.HeatTemp3Ope.ToString("F1"),		lblCurrSGMVVal.BackColor)		'SG MV値

	'位置ページ
	lblCurrXPosVal.SetTextEx((p_RTM(0).p_mltcp.CurrPosX * 0.001).ToString("F3"),lblCurrXPosVal.BackColor)		'X軸現在位置
	lblCurrYPosVal.SetTextEx((p_RTM(0).p_mltcp.CurrPosY * 0.001).ToString("F3"),lblCurrYPosVal.BackColor)		'Y軸現在位置
	lblCurrZPosVal.SetTextEx((p_RTM(0).p_mltcp.HeatBPos * 0.001).ToString("F3"),lblCurrZPosVal.BackColor)		'Z軸現在位置
	If p_RTM(0).p_dMesGap < 0
		'測定GAPが負数の時
		iColor = Color.Yellow
	Else
		iColor = Color.White
	End If
	lblGAPVal.SetTextEx((p_RTM(0).p_dMesGap).ToString("F3"), iColor)											'リアル測定GAP(mm)
	lblZeroPosVal.SetTextEx((p_RTM(0).p_mltcp.ZeroPos_Z/1000.0).ToString("F3"),	lblZeroPosVal.BackColor)		'Z ゼロ点位置(mm)
	'Z 着圧決定位置(mm)
	If p_RTM(0).p_mltcp.DevSoftVer>=1.03
		Dim dData As Double = Double.Parse(p_RTM(0).p_mltcp.ZConfirmPos)
		lblZConfPosVal.SetTextEx((dData/10000.0).ToString("F3"),	lblZConfPosVal.BackColor) 
	Else
		lblZConfPosVal.SetTextEx(("N/A"),	lblZConfPosVal.BackColor) 'Z 着圧決定位置(mm)
	End If
	'応力ページ
	lblCurrXStrsVal.SetTextEx(p_RTM(0).p_mltcp.CurrStrsX().ToString,			lblCurrXStrsVal.BackColor)			'X応力
	lblCurrYStrsVal.SetTextEx(p_RTM(0).p_mltcp.CurrStrsY().ToString,			lblCurrYStrsVal.BackColor)			'Y応力
	lblCurrZStrsVal.SetTextEx(p_RTM(0).p_mltcp.CurrStrsZgram().ToString("F0"),	lblCurrZStrsVal.BackColor)			'Z応力
	lblZStrsCRVal.SetTextEx(dZStability.ToString("F3"),							lblZStrsCRVal.BackColor)			'Z応力安定度(g/10S)
	lblNoLoadStrVal.SetTextEx(p_RTM(0).p_mltcp.ZeroStrs_Z.ToString(),			lblNoLoadStrVal.BackColor)			'Z無負荷応力(Dig)

	'計算結果ページ
	If p_cts.LTModeEnable
		If Not IsNothing(p_dtFile.iLT_Hosei)
			Dim iPt As Int32 = p_dtFile.iLT_Hosei.GetUpperBound(0)
			lblStressVal.SetTextEx(p_dtFile.iLT_Hosei(iPt).ToString("F1"),						lblStressVal.BackColor)				'応力
			lblStressPVal.SetTextEx(p_dtFile.dLT_Hosei100(iPt).ToString("F1"),					lblStressPVal.BackColor)			'応力%
			If (p_dtFile.p_curViscosity = 0 Or Double.IsNaN(p_dtFile.p_curViscosity)) And (p_RTM(0).p_mltcp.StartSW = 2)
			'測定中で粘度が0またはNANの時
				iColor = Color.Yellow
			Else
				iColor = Color.White
			End If
			lblViscoVal.SetTextEx(p_dtFile.dLT_Viscosity(iPt).ToString("F1"), 	iColor)												'粘度
			lblPhaseVal.SetTextEx((p_dtFile.dLT_XPosPhase(iPt) * RadToDegRate).ToString("F2"),	lblPhaseVal.BackColor)				'位相
			lblSmplStrnVal.SetTextEx(p_dtFile.dLT_SplStrain(iPt).ToString("F3"),				lblSmplStrnVal.BackColor)			'サンプル歪
		End If
	Else
		lblStressVal.SetTextEx(p_dtFile.p_curStress.ToString(),						lblStressVal.BackColor)					'応力
		lblStressPVal.SetTextEx(p_dtFile.p_curStressP.ToString("F1"),				lblStressPVal.BackColor)				'応力%
		If (p_dtFile.p_curViscosity = 0 Or Double.IsNaN(p_dtFile.p_curViscosity)) And (p_RTM(0).p_mltcp.StartSW = 2)
		'測定中で粘度が0またはNANの時
			iColor = Color.Yellow
		Else
			iColor = Color.White
		End If
		lblViscoVal.SetTextEx(p_dtFile.p_curViscosity.ToString("F1"),				iColor)									'粘度
		lblPhaseVal.SetTextEx((p_dtFile.p_curPhase * RadToDegRate).ToString("F2"),	lblPhaseVal.BackColor)					'位相
		lblSmplStrnVal.SetTextEx(p_dtFile.p_curSplStrain.ToString("F3"),			lblSmplStrnVal.BackColor)				'サンプル歪
	End If

	'非表示---
	lblShearModVal.SetTextEx(p_dtFile.p_curShearMod.ToString("F1"),			lblShearModVal.BackColor)						'剪断弾性率
	lblShearVeloVal.SetTextEx(p_dtFile.p_curShearVelo.ToString("F2"),		lblShearVeloVal.BackColor)						'剪断速度
	lblStorageModVal.SetTextEx(p_dtFile.p_curStrgMod.ToString("F1"),		lblStorageModVal.BackColor)						'貯蔵弾性率
	lblElasticModVal.SetTextEx(p_dtFile.p_curElasMod.ToString("F1"),		lblElasticModVal.BackColor)						'損失弾性率
	lblTangentVal.SetTextEx(p_dtFile.p_curLosTan.ToString("F3"),			lblTangentVal.BackColor)						'損失正接
	'---------
	System.Threading.Thread.Sleep(100)
	'計算進捗
	If lblCalclating.Visible		'datCal.Busy
		If Not bPreVisible
			lblCalclating.SetTextEx(p_sMes(efrmMainSTR.iCalucurating),		lblCalclating.BackColor)
			System.Threading.Thread.Sleep(1000)
		ElseIf Not datCal.CalCountTotal=0 AndAlso lblCalclating.Text.Substring(0,2) = p_sMes(efrmMainSTR.iCalucurating).Substring(0,2)
			lblCalclating.SetTextEx(p_sMes(efrmMainSTR.iCalucurating) & (datCal.CalCount / datCal.CalCountTotal * 100).ToString("F0") & "%",		lblCalclating.BackColor)
		End If

	End If
	bPreVisible = lblCalclating.Visible
	
Catch ex As Exception
	Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Call FrmDebugLog(System.Reflection.MethodBase.GetCurrentMethod.Name & ex.Message)
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
'debug.Print(sw1.Elapsed.TotalMilliseconds)
Loop
m_blBgwAlive_Trend = False          'スレッド終了
End Sub
#If 0
'エラー監視スレッド
Private Sub bgwErrorControl_DoWork(sender As Object, e As DoWorkEventArgs) Handles bgwErrorControl.DoWork

'スレッドの設定
Me.bgwErrorControl.WorkerReportsProgress = True
Me.bgwErrorControl.WorkerSupportsCancellation = True

m_blBgwAlive_Error = True       'スレッド生存中
Do
Try
	If p_RTM(0).p_iConnectState = CONNECT_ST.CONNECTING Then
		If (p_RTM(0).p_mltcp.DevErr1 And &HFFFF) <> 0 Or
			p_RTM(0).p_mltcp.DevErr2 <> 0 Or
			(p_RTM(0).p_mltcp.DevErr3 And &HFFFF) <> 0 Then
			If iErrorCnt > 2
				'エラー一覧表示画面
				frmError.ShowDialog()
				p_bError = True
			Else
				iErrorCnt += 1
			End If
		Else
			p_bError = False
			iErrorCnt = 0
		End If
	End If
	System.Threading.Thread.Sleep(500)
Catch ex As Exception
	Call FrmDebugLog(System.Reflection.MethodBase.GetCurrentMethod.Name & ex.Message)
End Try
Loop
m_blBgwAlive_Error = False          'スレッド終了
End Sub
#end if
'再計算処理
'Private Sub ReCalclationToolStripMenuItem_Click(sender As Object, e As EventArgs)
'	ReCalclationProc(1)     '再計算処理 ファイル上書き保存
'	UnderStatusLabelDisp( ToolStripStatusLabel1, p_sMes(efrmMainSTR.iRECALC_FILE) & p_dtFile.GetDataFileName)	'出力ファイル名
'	rbCorrect.Enabled = True
'	rbCorrect.Checked = True                                                                '補正データ画面の表示
'End Sub


Private m_iFrmCalib As Int16 = 0        '0:フルストレス 1:ゼロストレス
'メニュー-キャリブレーション(Percent)クリック
Private Sub PercentToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles tsmiFullStress.Click, tsmiZeroStress.Click
	Try
		'重量変換パラメータセット
		Dim frmC As New frmCalibWeight3
		frmC.WeightParameterFileGet(p_dNConv, p_dZConv, p_XYCoefMeasDate, p_ZCoefMeasDate)
		frmC.Dispose()

		Select Case sender.Name
		Case "tsmiFullStress"
			'フルストレスモード
			Call WriteLog("フルストレスキャリブレーション画面オープン", eLOG_LEVEL.iINFO)
			m_iFrmCalib = 0
		Case "tsmiZeroStress"
			'ゼロストレスモード
			Call WriteLog("ゼロストレスキャリブレーション画面オープン", eLOG_LEVEL.iINFO)
			m_iFrmCalib = 1
		End Select
		p_RTM(0).p_frmCalibObj = New frmCalibFull(m_iFrmCalib)          'オブジェクト参照生成
		p_RTM(0).p_frmCalibObj.ShowDialog()                             'フォーム表示
		p_RTM(0).p_frmCalibObj.Dispose()                                'フォーム消滅
		modFrmMain.HistoryDataClear(Me)                                 '測定用バッファクリア

		AllClear()														'全てクリア
		Call WriteLog("キャリブレーション画面クローズ", eLOG_LEVEL.iINFO)

		'Me.Hide
	Catch ex As Exception
		Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
		Call FrmDebugLog(System.Reflection.MethodBase.GetCurrentMethod.Name & ex.Message)
		Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	End Try
End Sub
'メニュー-キャリブレーション(Weight)クリック
Private Sub WeightToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles tsmiWeight.Click
	Try
		Call WriteLog("重量キャリブレーション画面オープン", eLOG_LEVEL.iINFO)

		Dim frmW2 As New frmCalibWeight3
		frmW2.ShowDialog()
		Dim d(1)	As Double
		Dim s(1)	As String
		frmW2.CsvGetCoef(p_SetPath, p_sStoreFileNameWeight, d, s)
		p_cts.WeightCoef	= d(0)
		p_cts.WeightCoefZ	= d(1)
		frmW2.Dispose()

		Call WriteLog("重量キャリブレーション画面クローズ", eLOG_LEVEL.iINFO)
	Catch ex As Exception
		Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
		Call FrmDebugLog(System.Reflection.MethodBase.GetCurrentMethod.Name & ex.Message)
		Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	End Try
End Sub
'メニュー-熱板水平確認クリック
Private Sub HorizontalConfirmationToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles tsmiHorizontalConfirmation.Click
	Try
		frmHorConf.ShowDialog()
	Catch ex As Exception
		Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
		Call FrmDebugLog(System.Reflection.MethodBase.GetCurrentMethod.Name & ex.Message)
	End Try
End Sub
'履歴描画有効無効
Private Sub HistoryDrawingEnableddisabledToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles tsmiHistoryDraw.Click
	tsmiHistoryDraw.Checked = Not tsmiHistoryDraw.Checked
	m_HistDraw = tsmiHistoryDraw.Checked
End Sub
'履歴描画クリア
Private Sub tsmiHistoryClear_Click(sender As Object, e As EventArgs) Handles tsmiHistoryClear.Click
	modFrmMain.HistoryDataClear(Me)
	m_RealTrendUpdate = False
	ChartDispUpdate(Me, 0, m_RealTrendUpdate)                   'チャート表示更新
	UnderStatusLabelDisp( ToolStripStatusLabel1, p_sMes(efrmMainSTR.iDATA_CLEAR))							'クリア

End Sub
'基準値描画有効無効
Private Sub tsmiStandardDraw_Click(sender As Object, e As EventArgs) Handles tsmiStandardDraw.Click
	tsmiStandardDraw.Checked = Not tsmiStandardDraw.Checked
	m_StdDraw = tsmiStandardDraw.Checked
End Sub
'基準値描画クリア
Private Sub tsmiStandardClear_Click(sender As Object, e As EventArgs) Handles tsmiStandardClear.Click
	modFrmMain.StandardDataClear(Me)
	m_RealTrendUpdate = False
	ChartDispUpdate(Me, 0, m_RealTrendUpdate)                   'チャート表示更新
	UnderStatusLabelDisp( ToolStripStatusLabel1, p_sMes(efrmMainSTR.iDATA_CLEAR))							'クリア
End Sub
'全て描画クリア
Private Sub tsmiAllClear_Click(sender As Object, e As EventArgs) Handles tsmiAllClear.Click, btnAllClear.Click
	AllClear()
	Call WriteLog("全てクリアボタンがクリックされました。", eLOG_LEVEL.iINFO)

	'Test1
	'Dim s As String
	's = p_dtFile.sMeasindCode(eMesCD.CLB) & "C" & "-" & Format(Now, "yyyyMMddHHmmss") & "-" & p_Rtm(0).p_mltcp.PeriodX(1).ToString("F1") & "$" & (p_Rtm(0).p_mltcp.AmplitudeX / 1000.0).ToString("F3")

End Sub
'グラフ設定 X軸固定 有無
Private Sub tsmiFixedTime_Click(sender As Object, e As EventArgs) Handles tsmiFixedX.Click
	tsmiFixedX.Checked = Not tsmiFixedX.Checked
	ckbXFixed.Checked = tsmiFixedX.Checked
	ScaleModeChange(0, tsmiFixedX.Checked, True)
	lblXAuto.Visible = Not tsmiFixedX.Checked
	'固定変更の時は適用処理を行う
	If ckbXFixed.Checked
		btnXSpanAply.PerformClick()
	End If
End Sub
'グラフ設定 Y軸固定 有無
Private Sub tsmiFixedY_Click(sender As Object, e As EventArgs) Handles tsmiFixedY.Click
	tsmiFixedY.Checked = Not tsmiFixedY.Checked
	ckbYFixed.Checked = tsmiFixedY.Checked
	ScaleModeChange(1, tsmiFixedY.Checked, True)
	lblYAuto.Visible = Not tsmiFixedY.Checked
	'固定変更の時は適用処理を行う
	If ckbYFixed.Checked
		btnYSpanAply.PerformClick()
	End If
End Sub
'グラフ設定 X軸 時間・温度切り替え
Private Sub XAxisTemperatureToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles tsmiXAxisTemp.Click
	tsmiXAxisTemp.Checked = Not tsmiXAxisTemp.Checked
	XaxisTypeCahnge(tsmiXAxisTemp.Checked)
End Sub
'グラフ設定 X軸固定チェックボックス
Private Sub ckbXFixed_Click(sender As Object, e As EventArgs) Handles ckbXFixed.Click
	ScaleModeChange(0, ckbXFixed.Checked, True)
	lblXAuto.Visible = Not ckbXFixed.Checked
	'固定変更の時は適用処理を行う
	If ckbXFixed.Checked
		btnXSpanAply.PerformClick()
	End If
End Sub
'グラフ設定 Y軸固定チェックボックス
Private Sub ckbYFixed_Click(sender As Object, e As EventArgs) Handles ckbYFixed.Click
	ScaleModeChange(1, ckbYFixed.Checked, True)
	lblYAuto.Visible = Not ckbYFixed.Checked
	'固定変更の時は適用処理を行う
	If ckbYFixed.Checked
		btnYSpanAply.PerformClick()
	End If
End Sub
'グラフ設定 X軸タイプ 時間
Private Sub rbXTime_Click(sender As Object, e As EventArgs) Handles rbXTime.Click
	XaxisTypeCahnge(If(rbXTime.Checked,0,1))
	Call WriteLog("グラフX軸 時間へ変更", eLOG_LEVEL.iINFO)
End Sub
'グラフ設定 X軸タイプ 時間
Private Sub rbXTemp_Click(sender As Object, e As EventArgs) Handles rbXTemp.Click
	XaxisTypeCahnge(If(rbXTime.Checked,0,1))
	Call WriteLog("グラフX軸 温度へ変更", eLOG_LEVEL.iINFO)
End Sub


''' <summary>
''' 全てクリア
''' </summary>
Private Sub AllClear()
	modFrmMain.HistoryDataClear(Me)
	modFrmMain.StandardDataClear(Me)
	modFrmMain.RealDataClear(Me)
	modFrmMain.ScaleClear(Me)
	m_RealTrendUpdate = False									'チャート設定

	'着圧指定時の制御失敗ラッチ変数リセット
	p_iFailedOrderP = 0
	'ガイドメッセージラッチ変数リセット
	p_iGuideXYbeforeGap = 0

	ChartDispUpdate(Me, 0, m_RealTrendUpdate)                   'チャート表示更新
	UnderStatusLabelDisp( ToolStripStatusLabel1, p_sMes(efrmMainSTR.iDATA_CLEAR))							'クリア
	If frmHistData.Visible
		frmHistData.Close
	End If
End Sub

''' <summary>
''' グラフ軸モード変更(固定/自動)
''' </summary>
''' <param name="axis">0/1:X/Y軸</param>
''' <param name="bFixed">True：固定 False：自動</param>
Private Sub	ScaleModeChange(Byval axis As int16, ByVal bFixed As Boolean, ByVal bChartUpdate As Boolean)
Try
	Select Case axis
	Case 0
	'X軸
		p_cts.iRTXAxis = If(bFixed, 1, 0)
		ckbXFixed.Checked		= bFixed					'X固定 有効/無効
		'gbxXscale.Enabled		= bFixed					'範囲入力 可/不可
		btnXSpanAply.Enabled	= bFixed					'範囲入力は可、適用不可
		tsmiFixedX.Checked		= bFixed
		gpxScrol.Enabled		= Not bFixed
		If bFixed Then
			p_iScrlStart = 0
		End If
	Case 1
	'Y軸
		p_cts.iRTYAxis = If(bFixed, 1, 0)
		ckbYFixed.Checked		= bFixed					'Y固定 有効/無効
		'gbxYscale.Enabled		= bFixed					'範囲入力 可/不可
		btnYSpanAply.Enabled	= bFixed					'範囲入力は可、適用不可
		tsmiFixedY.Checked		= bFixed
	End Select
	m_RealTrendUpdate = False
	If bChartUpdate
		If YSpanCheck() = 0
			Me.Cursor = Cursors.WaitCursor
			ChartDispUpdate(Me, 0, m_RealTrendUpdate)                   'チャート表示更新
			Me.Cursor = Cursors.Default
		End If
	End If

Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub
''' <summary>
''' グラフX軸タイプ変更
''' </summary>
''' <param name="bType"></param>
Private Sub XaxisTypeCahnge(ByVal bType As Boolean)
Try
	p_cts.iChartType		= If(bType, 1, 0)
	tsmiXAxisTemp.Checked	= bType
	rbXTime.Checked			= Not bType			
	rbXTemp.Checked			= btype

	nudXscaleMax.Value		= p_cts.MainGraphXMax(p_cts.iChartType)
	nudXscaleMin.Value		= p_cts.MainGraphXMin(p_cts.iChartType)
	nudXdivision.Value		= p_cts.MainGraphXDiv(p_cts.iChartType)

	Select Case p_RTM(0).p_iBGWOrder
	Case BGW_ORDER.MEAS_PROCESS
	Case Else
	'測定中でないときは再描画
		Me.Cursor = Cursors.WaitCursor
		Me.Refresh
		m_RealTrendUpdate = False
		ChartDispUpdate(Me, 0, m_RealTrendUpdate)                   'チャート表示更新
		Me.Cursor = Cursors.Default
	End Select
	sbScaleSetLangchange(p_cts)										'スケール設定等のラベル表示制御
Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub

''' <summary>
''' スケール設定等のラベル表示制御
''' </summary>
''' <param name="cts"></param>
Private Sub sbScaleSetLangchange(ByVal cts As CToolSettings)
	Select Case cts.LTModeEnable
	Case True
		lblResult1.Text		= p_sMes(efrmMainSTR.iGelTimeLblMin)
		lblScrolWidth.Text	= p_sMes(efrmMainSTR.iScrolWidthMin)
		ckbXFixed.Text		= If(cts.iChartType=0, p_sMes(efrmMainSTR.iXScaleFixedMin), p_sMes(efrmMainSTR.iXScaleFixedDegC))
		gpxScrol.Enabled	= If(cts.iChartType=0, True, False)
		gpxThinning.Enabled	= False
	Case Else
		lblResult1.Text		= p_sMes(efrmMainSTR.iGelTimeLblSec)
		lblScrolWidth.Text	= p_sMes(efrmMainSTR.iScrolWidthSec)
		ckbXFixed.Text		= If(cts.iChartType=0, p_sMes(efrmMainSTR.iXScaleFixedSec), p_sMes(efrmMainSTR.iXScaleFixedDegC))
		gpxScrol.Enabled	= If(cts.iChartType=0, True, False)
		gpxThinning.Enabled	= True
	End Select
End Sub





Dim ds As New DataSet
Dim dt As New DataTable
Dim SwDs As New Stopwatch
Public ZPosGetCount As UInt32 = 0
Public m_ZeroChartUpdate As Boolean = True
''' <summary>
''' ゼロ調モニタ制御
''' </summary>
Public Sub ZeroMonitorJob(ByRef frmChartX As Chart, ByRef frmPanelX As Panel)
Dim dtRow As DataRow
Dim Ary() As Double = Nothing
Dim i As Int32
Dim RangeUpper As Double = 0
Dim RangeLower As Double = 0
Try
	'ゼロ調中のＺ位置モニタ
	Select Case p_ZeroJob
	Case -1
		'待機中
		If p_RTM(0).p_mltcp.IsZeroStartSW 'And ((Double.Parse(p_RTM(0).p_mltcp.HeatBPos()) / 1000.0 - 5) >= p_cts.dZeroStaPosHP) Then
			'列の作成
			dt.Columns.Add("Time", Type.GetType("System.UInt32"))
			dt.Columns.Add("Z-Poition", Type.GetType("System.Double"))
			ds.Tables.Add(dt)
			ZPosGetCount = 0

			p_ZeroJob = 0
		End If
	Case 0
		'If dt.TableName<>"ZeroPosMon"
		'	p_ZeroJob = -1
		If p_RTM(0).p_mltcp.IsMeasStartSW Then
			Panel2.Visible = False
			'frmPanelX.Visible = False
			p_ZeroJob = 9
		ElseIf p_RTM(0).p_mltcp.IsZeroStartSW Then
			'データの追加
			ZPosGetCount = 0

			modFrmMain.ZpositionChartDisp(Me, 0, Me.Chart2, ds, 0, 0)       'ゼロ調進捗チャート表示
			'ZpositionChartDisp(0, frmChartX, ds, 0, 0)						'ゼロ調進捗チャート表示
			m_ZeroChartUpdate = True
			SwDs.Restart()
			p_ZeroJob = 1
		Else
			'ゼロ調中止で終了処理へ
			p_ZeroJob = 0
		End If
	Case 1
		'ゼロ調中の処理
		'If Not (P_RTM(0).p_mltcp.IsZeroStartSW) Or P_RTM(0).p_iZeroAdjOk
		''ゼロ調完了で終了処理へ
		'	ZeroJob = 9
		'End If
		If Not p_RTM(0).p_mltcp.IsZeroStartSW Then
			'ゼロ調中止で終了処理へ
			p_ZeroJob = 9
		ElseIf p_RTM(0).p_mltcp.IsMeasStartSW Then
			p_ZeroJob = 9
		ElseIf SwDs.Elapsed.TotalSeconds >= 1 Then
			'1秒毎に更新
			'データの追加
			ZPosGetCount += 1
			dtRow = ds.Tables(0).NewRow
			dtRow(0) = ZPosGetCount
			dtRow(1) = (Double.Parse(p_RTM(0).p_mltcp.HeatBPos()) / 1000.0)
			ds.Tables(0).Rows.Add(dtRow)


			Array.Resize(Ary, p_cts.iStaMonTime)

			For i = 0 To Ary.GetUpperBound(0)
				If ds.Tables(0).Rows.Count > p_cts.iStaMonTime Then
					Ary(i) = ds.Tables(0).Rows.Item(ds.Tables(0).Rows.Count - p_cts.iStaMonTime + i).ItemArray(1)
				Else
					If i >= ds.Tables(0).Rows.Count Then
					Else
						Ary(i) = ds.Tables(0).Rows.Item(i).ItemArray(1)
					End If
				End If
			Next
			Dim MedArray() As Double = Nothing
			For i = 0 To Ary.GetUpperBound(0)
				If Ary(i) <> 0 Then
					Array.Resize(MedArray, i + 1)
					MedArray(i) = Ary(i)
				Else
					Exit For
				End If
			Next
			If IsNothing(MedArray)
				Exit Sub
			End If

			'ゼロ調OK範囲表示用
			Dim dMed As Double = MedArray.Average
			'RangeUpper = dMed + (p_cts.iStaFlucRange / 2 / 1000)
			'RangeLower = dMed - (p_cts.iStaFlucRange / 2 / 1000)
			RangeUpper = dMed + (p_RTM(0).p_iStaFlucRange / 2 / 1000)
			RangeLower = dMed - (p_RTM(0).p_iStaFlucRange / 2 / 1000)
			If m_ZeroChartUpdate Then
				'初期表示
				modfrmMain.ZpositionChartDisp(Me, 1, frmChartX, ds, RangeUpper, RangeLower)         'ゼロ調進捗チャート表示
			Else
				'表示更新
				ZpositionChartUpdate(1, frmChartX, ds, RangeUpper, RangeLower)                      'ゼロ調進捗チャート表示更新
			End If

			frmPanelX.Visible = True

			'範囲内表示制御(設定秒数継続)
			If p_RTM(0).p_iZeroAdjOk = 1 Then
				frmPanelX.BackColor = Color.LightGreen
			Else
				frmPanelX.BackColor = Forms.Control.DefaultBackColor
			End If
			SwDs.Restart()
		End If
	Case 9
		'終了処理
		'ゼロ調結果を出力
''				ConvertDataTableToCsv(dt, Path.Combine(SIZUKA_PATH, p_sEquipName, MESDAT_PATH) & "\Zero_" & Now.ToString("yyMMddHHmmss") & ".csv", True)
		p_ZeroJob = 0
		dt.Clear()
		ds.Clear()
		Panel2.Visible = False
		'frmChartX.Visible = False
	End Select
Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Call FrmDebugLog(System.Reflection.MethodBase.GetCurrentMethod.Name & ex.Message)
	'p_ZeroJob = 0
End Try
End Sub
'DataTableをCsvファイルへ書き出す
Public Sub ConvertDataTableToCsv(dt As DataTable, csvPath As String, writeHeader As Boolean)
	Try
		'CSVファイルに書き込むときに使うEncoding
		Dim enc As System.Text.Encoding = System.Text.Encoding.GetEncoding("UTF-8")

		'書き込むファイルを開く
		Dim sr As New System.IO.StreamWriter(csvPath, False, enc)

		Dim colCount As Integer = dt.Columns.Count
		Dim lastColIndex As Integer = colCount - 1
		Dim i As Integer

		'ヘッダを書き込む
		If writeHeader Then
			For i = 0 To colCount - 1
				'ヘッダの取得
				Dim field As String = dt.Columns(i).Caption
				'フィールドを書き込む
				sr.Write(field)
				'カンマを書き込む
				If lastColIndex > i Then
					sr.Write(","c)
				End If
			Next
			'改行する
			sr.Write(vbCrLf)
		End If

		'レコードを書き込む
		Dim row As DataRow
		For Each row In dt.Rows
			For i = 0 To colCount - 1
				'フィールドの取得
				Dim field As String = row(i).ToString()
				'フィールドを書き込む
				sr.Write(field)
				'カンマを書き込む
				If lastColIndex > i Then
					sr.Write(","c)
				End If
			Next
			'改行する
			sr.Write(vbCrLf)
		Next

		'閉じる
		sr.Close()
	Catch ex As Exception

	End Try
End Sub

''応力ゼロ機能OFF
'Private Sub StressZeroResetToolStripMenuItem_Click(sender As Object, e As EventArgs)
'	ZeroResetProc()
'End Sub
'応力ゼロ機能OFF
Public Sub ZeroResetProc()
	If p_RTM(0).p_mltcp.P_StressZeroEnable Then
		p_RTM(0).p_mltcp.P_XStressZero = 0                    'X応力
		p_RTM(0).p_mltcp.P_YStressZero = 0                    'Y応力
		p_RTM(0).p_mltcp.P_ZStressZero = 0                    'Z応力
		p_RTM(0).p_mltcp.P_StressZeroEnable = False
		'応力ゼロ機能によるラベル変更
		lblCurrXStrs.Text = lblCurrXStrs.Text.Substring(1)
		lblCurrYStrs.Text = lblCurrYStrs.Text.Substring(1)
		lblCurrZStrs.Text = lblCurrZStrs.Text.Substring(1)
	End If
End Sub
'応力ゼロ機能ON/OFF
Private Sub ToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles tsmiStressZeroSet.Click
	modFrmMain.ZeroSetProc(Me)
End Sub
'メニュー 温度コントロール
Private Sub tsmiTemp_Click(sender As Object, e As EventArgs) Handles tsmiTemp.Click
	If btnTemp.Enabled Then
		btnTemp.Focus()
		btnTemp.PerformClick()
	End If
End Sub
'ゼロ調コントロール
Private Sub tsmiZeroAdj_Click(sender As Object, e As EventArgs) Handles tsmiZeroAdj.Click
	
If btnZero.Enabled Then
		btnZero.Focus()
		btnZero.PerformClick()
	End If
End Sub
'測定コントロール
Private Sub ToolStripMenuItem1_Click_1(sender As Object, e As EventArgs) Handles ToolStripMenuItem1.Click
	If btnMeasure.Enabled Then
		btnMeasure.Focus()
		btnMeasure.PerformClick()
	End If
End Sub
'エラーリセット
Private Sub tsmiResetError_Click(sender As Object, e As EventArgs) Handles tsmiResetError.Click
Try
    Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name, eLOG_LEVEL.iINFO)

	If p_RTM(0).p_iBGWOrder = BGW_ORDER.NO_ORDER
		p_RTM(0).p_iBGWOrder = BGW_ORDER.RESET_ERROR_REP
	End If
Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub
'原点復帰
Private Sub tsmiReturnOrigin_Click(sender As Object, e As EventArgs) Handles tsmiReturnOrigin.Click
Try
    Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name, eLOG_LEVEL.iINFO)

    if Not p_RTM(0).p_mltcp.isStatMeasuringTempo()
'		If p_RTM(0).p_mltcp.isILState()　Or p_RTM(0).p_mltcp.isILDisable			'IL(扉閉) OKか? IL無効も見る
       		'if p_RTM(0).p_mltcp.IsPosOrderOrigin()
			''OFFする
			'    p_RTM(0).p_mltcp.PosOrderOrigin(False)									    '原点駆動指令BIT OFF
			'Else
			'ONする
           		if p_RTM(0).p_iBGWOrder = BGW_ORDER.NO_ORDER then
					p_RTM(0).p_iBGWOrder = BGW_ORDER.ORG_BUTTON_REP
				End If
       		'End If
'	  	End If
  	End If

Catch ex As Exception
    Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub



#If 1 Then


Private Sub Button2_Click(sender As Object, e As EventArgs)
	Try

	Catch ex As OverflowException
		Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
		Return
	End Try

	End Sub
#End If

''' <summary>
''' 外挿法条件 開始点、中点、終了点更新イベント処理
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub nudStartTim_ValueChanged(sender As Object, e As EventArgs) Handles nudStartTim.ValueChanged, nudMidTim.ValueChanged, nudEndTim.ValueChanged, nudExpoSample.ValueChanged, nudThinning.ValueChanged
Dim i As Int32 = 0
Dim dVal As Double
Try
	If IsNothing(p_dtFile.iHosei) Then
		lblUnderBaseA.Text = ""
		lblMidA.Text = ""
		lblOverBaseA.Text = ""
		Return
	End If
	If Not clExtra.MatrixEnable Then
		Return
	End If
	Select Case sender.Name
	Case "nudExpoSample"
		dVal = nudExpoSample.Value
	Case "nudThinning"
		dVal = nudThinning.Value
	End Select
	clExtra.PointValueChange(Chart1, sender.Name, p_RTM(0).p_dFullStressADVal, dVal, nudStartTim.Value, nudMidTim.Value, nudEndTim.Value)
	lblUnderBaseA.Text	= clExtra.LowerGelT.ToString("F2")
	lblMidA.Text		= clExtra.MiddlGelT.ToString("F2")
	lblOverBaseA.Text	= clExtra.UpperGelT.ToString("F2")
	ExpGelTimeDisp()            'ゲルタイム表示更新

Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub

Private m_dChart1MouseDownVal	As Double = Double.NaN
Private m_dChart1MouseUpVal		As Double = Double.NaN
Private m_iMove					As Int16 = -1
Private m_MouseDBtn				As Integer = 0					'マウスダウン時のボタン
''' <summary>
''' チャート上のマウスダウン
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub Chart1_MouseDown(sender As Object, e As MouseEventArgs) Handles Chart1.MouseDown
Try
#If 0
Debug.Print("X=" & Chart1.ChartAreas(0).InnerPlotPosition.X)
Debug.Print("R=" & Chart1.ChartAreas(0).InnerPlotPosition.Right)
Debug.Print("Y=" & Chart1.ChartAreas(0).InnerPlotPosition.Y)
Debug.Print("B=" & Chart1.ChartAreas(0).InnerPlotPosition.Bottom)
Debug.Print("eX=" & e.x / Chart1.Size.Width * 100)
Debug.Print("eY=" & e.y / Chart1.Size.Height * 100)
#end if
	'測定状態チェック
	Select Case p_RTM(0).p_iBGWOrder
	Case BGW_ORDER.START_BUTTON To BGW_ORDER.MEAS_PRO_FINISH
	Case Else
	'測定中以外
		m_MouseDBtn = e.Button
		Select Case e.Button
		Case MouseButtons.Left
		'マウス左ボタン
			Select Case p_clSpec.spi.iGelTimeMethod
			Case eCalcMethod.THR, eCalcMethod.PHASE
			'THR法,Phase法
				Dim dFull As Double = If(p_clSpec.spi.iGelTimeMethod = eCalcMethod.THR, p_RTM(0).p_dFullStressADVal, 90.0)
				If Double.IsNaN(m_dChart1MouseDownVal) Then
					If rbCorrectP.Checked
						m_dChart1MouseDownVal = Chart1.ChartAreas(0).AxisY.PixelPositionToValue(e.Y)
					Else
						m_dChart1MouseDownVal = Chart1.ChartAreas(0).AxisY.PixelPositionToValue(e.Y) / dFull * 100.0
					End If
					Dim dThresh As Double
					'If 	p_DtFile.p_Head.iState = 1							'測定データ読込直後
					'	dThresh = Ctype(p_DtFile.p_head.sThreshold, Double)
					'Else
						dThresh  = p_RTM(0).p_dToqueThreshold
					'End If
					'始点
					Select Case m_dChart1MouseDownVal
					Case dThresh - 1 To dThresh + 1
						m_iMove = 1
					Case Else
						m_dChart1MouseDownVal = Double.NaN
					End Select
				End If
			Case eCalcMethod.SLOPE
			'SLOPE法
				If Double.IsNaN(m_dChart1MouseDownVal) Then
					Dim dY2Max As Double
					If IsNothing(p_dtFile.iSlope)
						dY2Max = Chart1.ChartAreas(0).AxisY2.Maximum
					ElseIf p_RTM(0).p_dAprxRangeMax<>0
						Dim dMax As Double
						GetMaxMin1ArrayRange(p_dtFile.iSlope, p_clSpec.spi.dEffectRangeMin, p_clSpec.spi.dEffectRangeMax, dMax)
						dY2Max = dMax
					Else
						dY2Max = p_dtFile.iSlope.Max
					End If

					m_dChart1MouseDownVal = Chart1.ChartAreas(0).AxisY2.PixelPositionToValue(e.Y) / dY2Max * 100.0
					Dim dThresh As Double
					'If 	p_DtFile.p_Head.iState = 1							'測定データ読込直後
					'	dThresh = Ctype(p_DtFile.p_head.sThreshold, Double)
					'Else
						dThresh  = p_RTM(0).p_dToqueThreshold
					'End If
					'始点
					Select Case m_dChart1MouseDownVal
						Case dThresh - 1 To dThresh + 1
							m_iMove = 1
						Case Else
							m_dChart1MouseDownVal = Double.NaN
					End Select
				End If
			Case eCalcMethod.EXTRA
			'外挿法
				clExtra.MouseDownProc(Chart1, e)
			End Select
		Case MouseButtons.Right
		'マウス右ボタン
			cmsDragDrop.AutoClose = False
			'コンテキストメニュー表示(データ読込)
			cmsDragDrop.Items(0).Visible = p_cts.ReadRealAreaEn			'測定値領域への読込許可?
			cmsDragDrop.Show(Chart1, e.X-10, e.Y-10)
		End Select
	End Select

Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub
''' <summary>
''' チャート上のマウスムーブ
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub Chart1_MouseMove(sender As Object, e As MouseEventArgs) Handles Chart1.MouseMove
Dim dX As Double
Dim dY As Double
Dim iThinning As Int16

'------------------------------------------------------------------------------------
Try
'チャート更新中はチャートの諸設定が確立されていないのでエラーとなる->処理なし無視する
	dX = Chart1.ChartAreas(0).AxisX.PixelPositionToValue(e.X)
Catch ex As Exception
	Exit Sub
End Try
'------------------------------------------------------------------------------------
Try
	'マウスカーソル位置座標のモニタ ------
	Select Case p_RTM(0).p_iBGWOrder
	Case BGW_ORDER.MEAS_PROCESS
		iThinning = 0
	Case Else
		iThinning = 1
	End Select
	Select Case p_cts.LTModeEnable Or p_dtFile.p_head.sMeasureType = p_dtFile.sMeasindCode(eMesCD.LTM)
	Case True	'LTM
		If Not IsNothing(p_dtFile.iLT_Hosei) Then
			dX = Chart1.ChartAreas(0).AxisX.PixelPositionToValue(e.X)
			dY = Chart1.ChartAreas(0).AxisY.PixelPositionToValue(e.Y)
			'閾値 範囲外チェック
			If dY >= Chart1.ChartAreas(0).AxisY.Maximum Or  dY <= Chart1.ChartAreas(0).AxisY.Minimum
				Exit Sub
			End If
			lblChartCoordinate.Text = "(" & dX.ToString("F2") & " , " & dY.ToString("F2") & ")"
		Else
			lblChartCoordinate.Text = ""
		End If
		'-------------------------------------
		'閾値変更による表示更新
		'測定状態チェック
		Select Case p_RTM(0).p_iBGWOrder
		Case BGW_ORDER.LT_START_BUTTON To BGW_ORDER.LT_MEAS_PRO_FINISH
			Chart1.Cursor = Cursors.Cross
		Case Else
			'測定中以外
			Select Case p_clSpec.spi.iGelTimeMethod
			Case eCalcMethod.THR, eCalcMethod.PHASE
				Dim dFull As Double = If(p_clSpec.spi.iGelTimeMethod = eCalcMethod.THR, p_RTM(0).p_dFullStressADVal, 90.0)
				If rbCorrectP.Checked
					dY = Chart1.ChartAreas(0).AxisY.PixelPositionToValue(e.Y)
				Else
					dY = Chart1.ChartAreas(0).AxisY.PixelPositionToValue(e.Y) / dFull * 100.0
				End If			
				Dim dThresh As Double
				dThresh  = p_RTM(0).p_dToqueThreshold
				'カーソル変更は閾値±1%で
				Select Case dY
				Case dThresh - 1 To dThresh + 1
					Chart1.Cursor = Cursors.SizeNS
				Case Else
					Chart1.Cursor = Cursors.Cross
				End Select
				If m_iMove = 1 Then
					Dim iRet As int32
					dgvDevSpec(2, eDevSPEC.THRETHOLD).Value = dY.ToString("F1")
					p_RTM(0).p_dToqueThreshold	= dY
					p_clSpec.spi.dThreshold		= dY	
					iRet = p_RTM(0).GelTimeSeach(p_iSelectGItem)      'ゲルタイム再検索
					If iRet = 0
						ResearchGelTimeOnChart()			'ゲルタイムチャート更新
						GelTimeLabelDispUpdate(Me)			'ゲルタイム値更新
						GelTimeJudgeColorUpdate(Me)			'ゲルタイム判定表示
					Else
					End If
					'p_DtFile.p_Head.iState = 0			'測定データ状態クリア
				End If
			End Select
		End Select

	Case Else	'通常
		If Not IsNothing(p_dtFile.iHosei) Then
			Select Case p_cts.iChartType
			Case 0	'時間
				Select Case p_RTM(0).p_iBGWOrder
				Case BGW_ORDER.START_BUTTON To BGW_ORDER.MEAS_PRO_FINISH
					Select Case p_cts.iRTXAxis
					Case 0  'Auto
						dX = Chart1.ChartAreas(0).AxisX.PixelPositionToValue(e.X) * p_cts.iScrlWidth / (Chart1.ChartAreas(0).AxisX.Maximum - Chart1.ChartAreas(0).AxisX.Minimum) + (p_iScrlStart / MEAS_BLOCK_DATA_NUM)
					Case 1  'fixed
						dX = Chart1.ChartAreas(0).AxisX.PixelPositionToValue(e.X) * (p_cts.MainGraphXMax(p_cts.iChartType)-p_cts.MainGraphXMin(p_cts.iChartType)) / (Chart1.ChartAreas(0).AxisX.Maximum-Chart1.ChartAreas(0).AxisX.Minimum)
					End Select
				Case Else
					Select Case p_cts.iRTXAxis
					Case 0  'Auto
						dX = Chart1.ChartAreas(0).AxisX.PixelPositionToValue(e.X) * (p_dtFile.iHosei.Length / MEAS_BLOCK_DATA_NUM)  / Chart1.ChartAreas(0).AxisX.Maximum
					Case 1  'fixed
						dX = Chart1.ChartAreas(0).AxisX.PixelPositionToValue(e.X) * (p_cts.MainGraphXMax(p_cts.iChartType)-p_cts.MainGraphXMin(p_cts.iChartType)) / (Chart1.ChartAreas(0).AxisX.Maximum-Chart1.ChartAreas(0).AxisX.Minimum)
					End Select
				End Select
	#If 0
	Debug.Print("ex=" & Chart1.ChartAreas(0).AxisX.PixelPositionToValue(e.X))
	Debug.Print("ey=" & Chart1.ChartAreas(0).AxisX.PixelPositionToValue(e.Y))
	Debug.Print("dX=" & dx)
	#end if
			Case 1	'温度
				dX = Chart1.ChartAreas(0).AxisX.PixelPositionToValue(e.X)
			End Select
			dY = Chart1.ChartAreas(0).AxisY.PixelPositionToValue(e.Y)
			'閾値 範囲外チェック
			If dY >= Chart1.ChartAreas(0).AxisY.Maximum Or  dY <= Chart1.ChartAreas(0).AxisY.Minimum
				Exit Sub
			End If
			lblChartCoordinate.Text = "(" & dX.ToString("F2") & " , " & dY.ToString("F2") & ")"
		Else
			lblChartCoordinate.Text = ""
		End If
		'-------------------------------------

		'閾値変更による表示更新
		'測定状態チェック
		Select Case p_RTM(0).p_iBGWOrder
		Case BGW_ORDER.START_BUTTON To BGW_ORDER.MEAS_PRO_FINISH
			Chart1.Cursor = Cursors.Cross
		Case Else
			'測定中以外
			Select Case p_iSelectGItem
			Case iGrpItmN.rbCorrect, iGrpItmN.rbCorrectP, iGrpItmN.rbPosPhase
				Select Case p_clSpec.spi.iGelTimeMethod
				Case eCalcMethod.THR, eCalcMethod.PHASE
					Dim dFull As Double = If(p_clSpec.spi.iGelTimeMethod = eCalcMethod.THR, p_RTM(0).p_dFullStressADVal, 90.0)
					If rbCorrectP.Checked
						dY = Chart1.ChartAreas(0).AxisY.PixelPositionToValue(e.Y)
					Else
						dY = Chart1.ChartAreas(0).AxisY.PixelPositionToValue(e.Y) / dFull * 100.0
					End If			
					Dim dThresh As Double
					'If 	p_DtFile.p_Head.iState = 1							'測定データ読込直後
					'	dThresh = Ctype(p_DtFile.p_head.sThreshold, Double)
					'Else
						dThresh  = p_RTM(0).p_dToqueThreshold
					'End If
					'カーソル変更は閾値±1%で
					Select Case dY
					Case dThresh - 1 To dThresh + 1
						Chart1.Cursor = Cursors.SizeNS
					Case Else
						Chart1.Cursor = Cursors.Cross
					End Select
					If m_iMove = 1 Then
						Dim iRet As int32
						dgvDevSpec(2, eDevSPEC.THRETHOLD).Value = dY.ToString("F1")
						p_RTM(0).p_dToqueThreshold	= dY
						p_clSpec.spi.dThreshold		= dY	
						iRet = p_RTM(0).GelTimeSeach(p_iSelectGItem)      'ゲルタイム再検索
						If iRet = 0
							ResearchGelTimeOnChart()			'ゲルタイムチャート更新
							GelTimeLabelDispUpdate(Me)			'ゲルタイム値更新
							GelTimeJudgeColorUpdate(Me)			'ゲルタイム判定表示
						Else
						End If
						'p_DtFile.p_Head.iState = 0			'測定データ状態クリア
					End If
				Case eCalcMethod.SLOPE
					'SLOPE法
					Dim dY2Max As Double
					If IsNothing(p_dtFile.iSlope)
						dY2Max = Chart1.ChartAreas(0).AxisY2.Maximum
					ElseIf p_clSpec.spi.dEffectRangeMax<>0
						Dim dMax As Double
						GetMaxMin1ArrayRange(p_dtFile.iSlope, p_clSpec.spi.dEffectRangeMin, p_clSpec.spi.dEffectRangeMax, dMax)
						dY2Max = dMax
					Else
						dY2Max = p_dtFile.iSlope.Max
					End If
					dY = Chart1.ChartAreas(0).AxisY2.PixelPositionToValue(e.Y) / dY2Max * 100.0
					If dY > 100.0
						Chart1.Cursor = Cursors.Cross
						Return
					End If

					Dim dThresh As Double
					'If 	p_DtFile.p_Head.iState = 1							'測定データ読込直後
					'	dThresh = Ctype(p_DtFile.p_head.sThreshold, Double)
					'Else
						dThresh  = p_RTM(0).p_dToqueThreshold
					'End If
					'カーソル変更は閾値±1%で
					Select Case dY
						Case dThresh - 1 To dThresh + 1
							Chart1.Cursor = Cursors.SizeNS
						Case Else
							Chart1.Cursor = Cursors.Cross
					End Select
					If m_iMove = 1 Then
						dgvDevSpec(2, eDevSPEC.THRETHOLD).Value = dY.ToString("F1")
						p_RTM(0).p_dToqueThreshold	= dY
						p_clSpec.spi.dThreshold		= dY	
						p_RTM(0).GelTimeSeach(p_iSelectGItem)       'ゲルタイム再検索
						ResearchGelTimeOnChart()					'ゲルタイムチャート更新
						GelTimeLabelDispUpdate(Me)					'ゲルタイム値更新
					End If
				Case Else
				End Select
			Case eCalcMethod.EXTRA
				'外挿法
				If IsNothing(clExtra) Then
					Return
				End If
				If p_sSelectGItem <> "rbCorrect"
					Return
				End If
				clExtra.MouseMoveProc(Chart1, e, dX)

				If clExtra.SelPoint <> -1 Then
					Select Case clExtra.SelPoint
					Case 0
						nudStartTim.Value = dX
						lblUnderBaseA.Text = clExtra.LowerGelT.ToString("F2")
						ExpGelTimeDisp()
					Case 1
						nudMidTim.Value = dX
						lblMidA.Text = clExtra.MiddlGelT.ToString("F2")
						ExpGelTimeDisp()
					Case 2
						nudEndTim.Value = dX
						lblOverBaseA.Text = clExtra.UpperGelT.ToString("F2")
						ExpGelTimeDisp()
					Case Else
					End Select
				End If
			End Select
		End Select
	End Select


Catch ex As Exception
	'Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
Finally
End Try
End Sub
''' <summary>
''' チャート上のマウスアップ
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub Chart1_MouseUp(sender As Object, e As MouseEventArgs) Handles Chart1.MouseUp
	Dim dX As Double
	Dim dY As Double
	Try
		'閾値変更による表示更新
		'測定状態チェック
		Select Case p_RTM(0).p_iBGWOrder
		Case BGW_ORDER.START_BUTTON To BGW_ORDER.MEAS_PRO_FINISH
		Case Else
			'測定中以外
			Select Case p_clSpec.spi.iGelTimeMethod
			Case eCalcMethod.THR, eCalcMethod.PHASE
				'THR法,Phase法
				Dim dFull As Double = If(p_clSpec.spi.iGelTimeMethod = eCalcMethod.THR, p_RTM(0).p_dFullStressADVal, 90.0)
				If m_iMove = 1 Then
					If rbCorrectP.Checked
						m_dChart1MouseUpVal = Chart1.ChartAreas(0).AxisY.PixelPositionToValue(e.Y)
					Else
						m_dChart1MouseUpVal = Chart1.ChartAreas(0).AxisY.PixelPositionToValue(e.Y) / dFull * 100.0
					End If
					dY = m_dChart1MouseUpVal
					dgvDevSpec(2, eDevSPEC.THRETHOLD).Value = dY.ToString("F1")
					p_RTM(0).p_dToqueThreshold	= dY
					p_clSpec.spi.dThreshold		= dY	

					m_dChart1MouseDownVal = Double.NaN
					m_dChart1MouseUpVal = Double.NaN
					Chart1.Cursor = Cursors.Default

					p_RTM(0).GelTimeSeach(p_iSelectGItem)             'ゲルタイム再検索
					m_RealTrendUpdate = False
					ChartDispUpdate(Me, 0, m_RealTrendUpdate)           'チャート表示更新 ------------------
					GelTimeLabelDispUpdate(Me)							'ゲルタイム値結果表示
					m_iMove = -1
				End If
			Case eCalcMethod.SLOPE
				'SLOPE法
				If m_iMove = 1 Then
					Dim dY2Max As Double
					If IsNothing(p_dtFile.iSlope)
						dY2Max = Chart1.ChartAreas(0).AxisY2.Maximum
					ElseIf p_clSpec.spi.dEffectRangeMax<>0
						Dim dMax As Double
						GetMaxMin1ArrayRange(p_dtFile.iSlope, p_clSpec.spi.dEffectRangeMin, p_clSpec.spi.dEffectRangeMax, dMax)
						dY2Max = dMax
					Else
						dY2Max = p_dtFile.iSlope.Max
					End If

					m_dChart1MouseUpVal = Chart1.ChartAreas(0).AxisY2.PixelPositionToValue(e.Y) / dY2Max * 100.0
					If m_dChart1MouseUpVal > 100
						dY = 100
					Else
						dY = m_dChart1MouseUpVal
					End If
					dgvDevSpec(2, eDevSPEC.THRETHOLD).Value = dY.ToString("F1")
					p_RTM(0).p_dToqueThreshold	= dY
					p_clSpec.spi.dThreshold		= dY	
					m_dChart1MouseDownVal = Double.NaN
					m_dChart1MouseUpVal = Double.NaN
					Chart1.Cursor = Cursors.Default

					p_RTM(0).GelTimeSeach(p_iSelectGItem)             'ゲルタイム再検索
					m_RealTrendUpdate = False
					ChartDispUpdate(Me, 0, m_RealTrendUpdate)           'チャート表示更新 ------------------
					GelTimeLabelDispUpdate(Me)							'ゲルタイム値結果表示
					m_iMove = -1
				End If

			Case eCalcMethod.EXTRA
				'外挿法
				clExtra.MouseUpProc(Chart1, e, dX)
				ExpGelTimeDisp()                    'ゲルタイム表示更新
			End Select
		End Select
	Catch ex As Exception
	Finally
	End Try
End Sub
''' <summary>
''' チャート1のマウスダブルクリック
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub Chart1_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles Chart1.MouseDoubleClick
Try
	Select Case e.Button
	Case MouseButtons.Left
	'左マウスボタン
	'X,Y軸スケールモード切替 自動/手動
		If	lblYAuto.Visible = True
			ScaleModeChange(0, True, True)
			ScaleModeChange(1, True, True)
			lblXAuto.Visible = False
			lblYAuto.Visible = False
		Else
			ScaleModeChange(0, False, True)
			ScaleModeChange(1, False, True)
			lblXAuto.Visible = True
			lblYAuto.Visible = True
		End If

	Case Else
	End Select
Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub





'外挿法 ゲルタイム選択表示
Public Sub ExpGelTimeDisp()
Try
	Select Case cboGelTSel.SelectedIndex
	Case 0, -1
		p_RTM(0).p_dGelTime = clExtra.LowerGelT
	Case 1
		p_RTM(0).p_dGelTime = clExtra.MiddlGelT
	Case 2
		p_RTM(0).p_dGelTime = clExtra.UpperGelT
	End Select
	lblResult1Val.Text = p_RTM(0).p_dGelTime.ToString("F2")                                      'ゲルタイム結果表示
	clExtra.GelTSelect = cboGelTSel.Text                                                         'ゲルタイム選択セット

Catch ex As Exception
	Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub

''' <summary>
''' ゲルタイムチャート上の表示更新（dgv変更、マウスドラッグ時）
''' 計算方法 THR、PHASE、SLOPE
''' </summary>
Public Sub	ResearchGelTimeOnChart()
Try
	p_RTM(0).p_dToqueThreshold	= p_clSpec.spi.dThreshold
	p_RTM(0).GelTimeSeach(p_iSelectGItem)             'ゲルタイム再検索
	'Threshold変更によるゲルタイム表示更新
	Dim stCa As ChartArg = Nothing
	stCa.iGelTimeJudge		= p_RTM(0).p_iGelTimeJudge
	stCa.dJudgeRangeMin		= p_RTM(0).p_dJudgeRangeMin
	stCa.dJudgeRangeMax		= p_RTM(0).p_dJudgeRangeMax
	stCa.dAprxRangeMin		= p_RTM(0).p_dAprxRangeMin
	stCa.dAprxRangeMax		= p_RTM(0).p_dAprxRangeMax
	stCa.sCalcMethod		= If(p_RTM(0).p_sCalcMethod="", p_dtFile.p_head.sCalcMethod,p_RTM(0).p_sCalcMethod)
	stCa.dGelTime			= p_RTM(0).p_dGelTime
	stCa.dToqueThreshold	= p_RTM(0).p_dToqueThreshold
	Select Case p_iSelectGItem
	Case iGrpItmN.rbCorrectP
		stCa.dFullStressADVal	= 100
	Case Else
		stCa.dFullStressADVal	= p_RTM(0).p_dFullStressADVal
	End Select
	'ゲルタイム補助線表示
	If p_cts.LTModeEnable Or p_dtFile.p_head.sMeasureType = p_dtFile.sMeasindCode(eMesCD.LTM)
		LTChartUpdateGelTimeLine(
			stca, _
			Chart1,
			clExtra,
			p_sSelectGItem,
			p_cts,
			1 _                                     '時間間引きタイプ
		)
	Else
		ChartUpdateGelTimeLine(
			stca, _
			Chart1,
			clExtra,
			p_sSelectGItem,
			p_cts,
			1 _                                     '時間間引きタイプ
		)
	End If

Catch ex As Exception
	Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub

'測定履歴情報グループボックスマウスドラッグ移動 ------------------------------------
Dim isDraging As Boolean
Dim diffPoint As System.Drawing.Point = Nothing

Private Sub gbxStdCond_MouseDown(sender As Object, e As MouseEventArgs) Handles gbxStdCond.MouseDown, gbxExpoPad.MouseDown
    If e.Button <> MouseButtons.Left
        Return
    End If
    isDraging = True
    diffPoint = e.Location
	sender.Cursor = Cursors.SizeAll

End Sub
Private Sub gbxStdCond_MouseMove(sender As Object, e As MouseEventArgs) Handles gbxStdCond.MouseMove, gbxExpoPad.MouseMove
    If Not isDraging
        Return
    End If
    Dim x As Integer = sender.Location.X + e.X - diffPoint.X
	Dim y As Integer = sender.Location.Y + e.Y - diffPoint.Y
    sender.Location = New System.Drawing.Point(x, y)
End Sub
Private Sub gbxStdCond_MouseUp(sender As Object, e As MouseEventArgs) Handles gbxStdCond.MouseUp,  gbxExpoPad.MouseUp
    isDraging = False
	sender.Cursor = Cursors.Default
    If e.Button <> MouseButtons.Left
        Return
    End If
End Sub

'グラフ設定パネルマウスドラッグ移動 ------------------------------------
Dim isDraging1 As Boolean
Dim diffPoint1 As System.Drawing.Point = Nothing

Private Sub pnlThinning_MouseDown(sender As Object, e As MouseEventArgs) Handles pnlThinning.MouseDown,gpxAxisSetting.MouseDown,gpxXType.MouseDown,gpxThinning.MouseDown
    If e.Button <> MouseButtons.Left
        Return
    End If
    isDraging1 = True
    diffPoint1 = e.Location
	sender.Cursor = Cursors.SizeAll

End Sub
Private Sub pnlThinning_MouseMove(sender As Object, e As MouseEventArgs) Handles pnlThinning.MouseMove,gpxAxisSetting.MouseMove,gpxXType.MouseMove,gpxThinning.MouseMove
    If Not isDraging1
        Return
    End If
    Dim x As Integer = pnlThinning.Location.X + e.X - diffPoint1.X
	Dim y As Integer = pnlThinning.Location.Y + e.Y - diffPoint1.Y
	If Chart1.Left<=x And Chart1.Width>=x
		If Chart1.Top<=y And Chart1.Height>=y
			pnlThinning.Location = New System.Drawing.Point(x, y)
		End If
	End If
End Sub
Private Sub pnlThinning_MouseUp(sender As Object, e As MouseEventArgs) Handles pnlThinning.MouseUp,gpxAxisSetting.MouseUp,gpxXType.MouseUp,gpxThinning.MouseUp
    isDraging1 = False
	pnlThinningLocation = pnlThinning.Location		'位置記憶
	sender.Cursor = Cursors.Default
    If e.Button <> MouseButtons.Left
        Return
    End If
End Sub
Private Sub pnlThinning_DoubleClick(sender As Object, e As EventArgs) Handles pnlThinning.DoubleClick
	pnlThinning.Visible = False
End Sub
Private Sub lblExpand_Click(sender As Object, e As EventArgs) Handles lblExpand.Click
	If pnlThinning.Height = 259
		pnlThinning.Height = 470
		lblExpand.Text = "△"
	Else
		pnlThinning.Height = 259
		lblExpand.Text = "▽"
	End If
End Sub




'外挿法 ゲルタイム選択変更
Private Sub cboGelTSel_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboGelTSel.SelectedIndexChanged
	ExpGelTimeDisp()                    '外挿法 ゲルタイム選択表示
End Sub
''' <summary>
''' 外挿法 セーブボタン データファイル更新
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub btnExtrapolationSave_Click(sender As Object, e As EventArgs) Handles btnExtrapolationSave.Click
	'clExtra.DataFileUpdate(p_dtFile)    'データファイル更新(外挿法)

	clExtra.FileHeaderSet(p_dtFile.p_head)										'外挿法 ヘッダ情報セット
	p_dtFile.RealTimeMeasDataFileSaveText(eMeasMode.Normal, False, p_cts)		'上書き更新

End Sub
'ゲルタイム測定
Private Sub GelTimeToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles tsmiGelTime.Click
		p_RTM(0).p_MeasMode = 0
End Sub
'粘度測定
Private Sub FlowCurveToolStripMenuItem_Click_1(sender As Object, e As EventArgs) Handles tsmiFlowCurve.Click
	Try
		p_RTM(0).p_MeasMode = 1

		p_RTM(0).p_mltcp.StartTempCont = 0		'温度指令をOFF
		p_RTM(0).p_mltcp.StartSW = 0			'ゼロ調指令をOFF
		p_RTM(0).p_iZeroAdjOk = -1              'ゼロ調正完了OFF

		'ZEROキャリブレーション結果読込
		Dim frmZ As New frmCalibFull(1)
		frmZ.sbFullStressCondRead(1, p_ZClbX, p_ZClbC, p_ZClbP)
		frmZ.Dispose()
		'Fullキャリブレーション結果読込
		Dim frmF As New frmCalibFull(0)
		frmF.sbFullStressCondRead(0, p_FClbX, p_FClbC, p_ZClbP)
		frmF.Dispose()

		'測定画面表示
		Dim frmV As New frmViscosity
		frmV.ShowDialog()
		frmV.Dispose()

		p_dtFile.ReadBufferClear()                              'データバッファクリア
		p_dtFile.DataArrayFiFoClear()	                        '重ね書き用履歴データ配列クリア
		clExtra.AllDispClear(Chart1)                            '外挿法の線を削除
		p_RTM(0).p_dGelTime = -1                                'ゲルタイム補助線削除

		UnderStatusLabelDisp(ToolStripStatusLabel1, Nothing)	'表示クリア
		p_RTM(0).p_MeasMode = 0

		'Me.Hide
	Catch ex As Exception
		Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
		Call FrmDebugLog(System.Reflection.MethodBase.GetCurrentMethod.Name & ex.Message)
		Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	End Try
End Sub

Dim pnlThinningLocation As System.Drawing.Point = New System.Drawing.Point(115, 135)
'表示設定パネル表示ON/OFF
Private Sub btnThinning_Click(sender As Object, e As EventArgs) Handles btnThinning.Click
Try
	'表示設定パネル表示
		pnlThinning.Visible =  Not pnlThinning.Visible
		ScaleModeChange(0, If(p_cts.iRTXAxis = 1, True, False), False)
		ScaleModeChange(1, If(p_cts.iRTYAxis = 1, True, False), False)
		pnlThinning.Location = pnlThinningLocation								'初期位置
		If pnlThinning.Visible
			nudDispThinningoutRough.Value	= p_cts.iDetailsR 		'間引き(粗い)
			nudDispThinningoutDetail.Value	= p_cts.iDetailsD 		'間引き(細い)
			nudXscaleMin.Value				= p_cts.MainGraphXMin(p_cts.iChartType)	'X軸SPAN
			nudXscaleMax.Value				= p_cts.MainGraphXMax(p_cts.iChartType)	'X軸SPAN
			nudXdivision.Value				= p_cts.MainGraphXDiv(p_cts.iChartType)	'X軸目盛り分割数
			nudYscaleMin.Value				= p_cts.MainGraphYMin(p_iSelectGItem)	'Y軸SPAN
			nudYscaleMax.Value				= p_cts.MainGraphYMax(p_iSelectGItem)	'Y軸SPAN
			nudYdivision.Value				= p_cts.MainGraphYDiv(p_iSelectGItem)	'Y軸目盛り分割数
			rbXTime.Checked					= If(p_cts.iChartType=0,True,False)		'X軸 時間/温度
			rbXTemp.Checked					= If(p_cts.iChartType=0,False,True)		'X軸 時間/温度
			ckbXFixed.Focus()

		End If
Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub
'スクロール幅/間引き 設定
Private Sub nudXSpan_ValueChanged(sender As Object, e As EventArgs) Handles nudXSpan.ValueChanged,nudDispThinningoutRough.ValueChanged,nudDispThinningoutDetail.ValueChanged
	If bFrmNew
		Exit Sub
	End If
	Select Case sender.Name
	Case "nudXSpan"
		p_cts.iScrlWidth = nudXSpan.Value					'X軸スパン設定
		Call WriteLog("グラフスクロール幅 変更" & p_cts.iScrlWidth.ToString, eLOG_LEVEL.iINFO)

	Case "nudDispThinningoutRough"
		p_cts.iDetailsR = nudDispThinningoutRough.Value		'間引き(粗)
		RealTimeThinning(0) = p_cts.iDetailsR
		m_RealTrendUpdate = False
		ChartDispUpdate(Me, 0, m_RealTrendUpdate)           'チャート表示更新 ------------------
		Call WriteLog("グラフ間引き設定(測定中) 変更" & p_cts.iDetailsR.ToString, eLOG_LEVEL.iINFO)

	Case "nudDispThinningoutDetail"							'間引き(精)
		p_cts.iDetailsD = nudDispThinningoutDetail.Value
		RealTimeThinning(1) = p_cts.iDetailsD
		m_RealTrendUpdate = False
		ChartDispUpdate(Me, 0, m_RealTrendUpdate)           'チャート表示更新 ------------------
		Call WriteLog("グラフ間引き設定(測定後) 変更" & p_cts.iDetailsD.ToString, eLOG_LEVEL.iINFO)

		'MethodChanged(p_clSpec.spi.iGelTimeMethod)
	End Select
End Sub
'スクロール幅設定
Private Sub nudXSpan_KeyDown(sender As Object, e As KeyEventArgs) Handles nudXSpan.KeyDown,nudDispThinningoutRough.KeyDown,nudDispThinningoutDetail.KeyDown
	Select Case e.KeyCode
	Case Keys.Enter, Keys.Tab
		Select Case sender.Name
		Case "nudXSpan"
			p_cts.iScrlWidth = nudXSpan.Value		'X軸スパン設定
			'btnScrolWidth_Click(sender,e)
		Case "nudDispThinningoutRough"
			p_cts.iDetailsR = nudDispThinningoutRough.Value
			RealTimeThinning(0) = p_cts.iDetailsR
			'btnScrolWidth_Click(sender,e)
		Case "nudDispThinningoutDetail"
			p_cts.iDetailsD = nudDispThinningoutDetail.Value
			RealTimeThinning(1) = p_cts.iDetailsD
			'btnScrolWidth_Click(sender,e)
		End Select
	Case Keys.Escape
	'何もしないで閉じる
		Select Case sender.Name
		Case "nudXSpan", "nudDispThinningoutRough", "nudDispThinningoutDetail"
			pnlThinning.Visible =  False
		End Select
	Case Else
	End Select
End Sub
Private Sub nmdDispThinningoutRough_ValueChanged(sender As Object, e As EventArgs) Handles nudDispThinningoutRough.ValueChanged
	If bFrmNew
		Exit Sub
	End If
	p_cts.iDetailsR = nudDispThinningoutRough.Value
	RealTimeThinning(0) = p_cts.iDetailsR
End Sub

Private Sub nmdDispThinningoutDetail_ValueChanged(sender As Object, e As EventArgs) Handles nudDispThinningoutDetail.ValueChanged
	If bFrmNew
		Exit Sub
	End If
	p_cts.iDetailsD = nudDispThinningoutDetail.Value
	RealTimeThinning(1) = p_cts.iDetailsD
End Sub
'Z軸駆動(トグル)
Private Sub btnP1u_Click(sender As Object, e As EventArgs) 
Try
	If p_RTM(0).p_mltcp.IsPosOrderZ()
		p_RTM(0).p_mltcp.PosOrderZ(False)									'Z駆動指令BIT OFF
		sender.BackColor = Button.DefaultBackColor
	Else
		sender.BackColor = Color.Yellow
		Select Case sender.Name
		Case "btnP1u"
			p_RTM(0).p_mltcp.PosHeatB = p_RTM(0).p_mltcp.HeatBPos + 1
			p_RTM(0).p_mltcp.PosOrderZ(True)									'Z駆動指令BIT On

		Case "btnP10u"
			p_RTM(0).p_mltcp.PosHeatB = p_RTM(0).p_mltcp.HeatBPos + 10
			p_RTM(0).p_mltcp.PosOrderZ(True)									'Z駆動指令BIT On

		Case "btnP100u"
			p_RTM(0).p_mltcp.PosHeatB = p_RTM(0).p_mltcp.HeatBPos + 100
			p_RTM(0).p_mltcp.PosOrderZ(True)									'Z駆動指令BIT On

		Case "btnM1u"
			p_RTM(0).p_mltcp.PosHeatB = p_RTM(0).p_mltcp.HeatBPos - 1
			p_RTM(0).p_mltcp.PosOrderZ(True)									'Z駆動指令BIT On

		Case "btnM10u"
			p_RTM(0).p_mltcp.PosHeatB = p_RTM(0).p_mltcp.HeatBPos - 10
			p_RTM(0).p_mltcp.PosOrderZ(True)									'Z駆動指令BIT On

		Case "btnM100u"
			p_RTM(0).p_mltcp.PosHeatB = p_RTM(0).p_mltcp.HeatBPos - 100
			p_RTM(0).p_mltcp.PosOrderZ(True)									'Z駆動指令BIT On

		End Select
	End If
Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub
'Chart1ドラッグイン
Private Sub Chart1_DragEnter(sender As Object, e As Forms.DragEventArgs) Handles Chart1.DragEnter
    ' ドラッグされたデータがファイルかチェック
    If e.Data.GetDataPresent(DataFormats.FileDrop) Then
        Dim files() As String = CType(e.Data.GetData(DataFormats.FileDrop), String())
        ' ファイルが1つ以上ある場合
        If files.Length > 0 Then
            ' 最初のファイルの拡張子を取得
            Dim ext As String = IO.Path.GetExtension(files(0)).ToLower()
            ' 許可する拡張子を指定
            Dim allowedExts As String() = {".dat"}
            If allowedExts.Contains(ext) Then
                e.Effect = DragDropEffects.Copy   ' ドロップ可能
            Else
                e.Effect = DragDropEffects.None   ' ドロップ不可
            End If
        End If
    Else
        e.Effect = DragDropEffects.None
    End If
End Sub

Public DragDropFilePath As String() = Nothing		'ドラッグドロップファイルリスト配列
Public iDragDrop As Int32 = 0
''' <summary>
''' ファイル読込み（基準値・測定値領域）
''' Chart1ドラッグドロップ
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub Chart1_DragDrop(sender As Object, e As Forms.DragEventArgs) Handles Chart1.DragDrop
	cmsDragDrop.AutoClose = True
	'ファイルリスト作成
    DragDropFilePath = CType( e.Data.GetData(DataFormats.FileDrop, False), String())
	'コンテキストメニュー表示
	iDragDrop = 1				'1:ドラッグドロップで処理
	cmsDragDrop.Show(e.X, e.Y)
End Sub


''' <summary>
''' コンテキストメニュー 開くとき
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub cmsDragDrop_Opening(sender As Object, e As CancelEventArgs) Handles cmsDragDrop.Opening
	cmsDragDrop.Font = New Drawing.Font(BASE_FONT, 12)		'文字フォント設定
	If p_clSpec.spi.iGelTimeMethod = eCalcMethod.SLOPE And _
			Not IsNothing(p_dtFile.iHosei) And Not IsNothing(p_cp)
		tsmiSLOPERecalc.Visible = True
		tsmiSepSlope.Visible = True
	Else
		tsmiSLOPERecalc.Visible = False
		tsmiSepSlope.Visible = False
	End If
End Sub

''' <summary>
''' コンテキストメニューからマウスポインタを外すと消す
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub cmsDragDrop_MouseLeave(sender As Object, e As EventArgs) Handles cmsDragDrop.MouseLeave
	cmsDragDrop.Close
End Sub
''' <summary>
''' コンテキストメニュークリック
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub cmsDragDrop_ItemClicked(sender As Object, e As ToolStripItemClickedEventArgs) Handles cmsDragDrop.ItemClicked
	'Dim sPath As String = Nothing
	cmsDragDrop.Close

#If 1
	Dim iArea	As Int32
	Select Case e.ClickedItem.Name
	Case "tsmiStandard1"				'基準値
		iArea = 1
	Case "tsmiStandard2"				'基準値
		iArea = 2
	Case "tsmiReal"				'測定値
		iArea = 0
	Case "tsmiCansel"				'キャンセル
		DragDropFilePath = Nothing
		iDragDrop = 0
		Exit sub
	End Select
	Dim dlg As New FrmFileSelectWithPreview(Path.Combine(SIZUKA_PATH),iArea)
	If dlg.ShowDialog() = DialogResult.OK
		'グループ単位で配列化
		Dim selectedFile() As String = dlg.p_Selectfiles(0).Files.ToArray()
		DragDropFilePath = selectedFile
		'選択ファイル読込処理
		modFrmMain.cmsDragDropProc(Me, iArea)
	End If


#else



	Select Case e.ClickedItem.Name
#If 0
	Case "tsmiStandard"				'基準値
		If iDragDrop = 1
			modfrmMain.cmsDragDropProc(Me, 1)
		Else
			cmsDragDrop.Close
			If DialogResult.OK = modFrmMain.InportFileSelect(Me, 1, DragDropFilePath)
				modfrmMain.cmsDragDropProc(Me, 1)
			End If
		End If
#end if
	Case "tsmiStandard1"				'基準値1
		If iDragDrop = 1
			modfrmMain.cmsDragDropProc(Me, 1)
		Else
			cmsDragDrop.Close
			If DialogResult.OK = modFrmMain.InportFileSelect(Me, 1, DragDropFilePath)
				modfrmMain.cmsDragDropProc(Me, 1)
			End If
		End If
	Case "tsmiStandard2"				'基準値2
		If iDragDrop = 1
			modfrmMain.cmsDragDropProc(Me, 2)
		Else
			cmsDragDrop.Close
			If DialogResult.OK = modFrmMain.InportFileSelect(Me, 1, DragDropFilePath)
				modfrmMain.cmsDragDropProc(Me, 2)
			End If
		End If
	Case "tsmiReal"					'測定値
		If iDragDrop = 1
			modfrmMain.cmsDragDropProc(Me, 0)
		Else
'			cmsDragDrop.Close
			If DialogResult.OK = modFrmMain.InportFileSelect(Me, 0, DragDropFilePath)
				modfrmMain.cmsDragDropProc(Me, 0)
			End If
		End If
	Case "tsmiSLOPERecalc"
		p_cp.iCalcType = p_clSpec.spi.iGelTimeMethod
		SlopeRecalc()						'SLOPE再計算処理
		ChartDispUpdate(Me, 0, True)		'チャート表示更新
		GelTimeLabelDispUpdate(Me)			'ゲルタイム値ラベル表示更新
		GelTimeJudgeColorUpdate(Me)			'ゲルタイム判定表示更新

	Case "tsmiCansel"				'キャンセル
		DragDropFilePath = Nothing
	End Select
#end if
'	cmsDragDrop.Close
	iDragDrop = 0
End Sub
'ファイルメニュー Export 基準データとして保存
Private Sub tsmiStandardExp_Click(sender As Object, e As EventArgs) Handles tsmiStandardExp.Click
	Dim s As String = Nothing
	Dim sFileName As String
	
	If IsNothing(p_dtFile.iHosei)
	'有効なデータがない
		Return
	End If
	If IsNothing(p_dtFile.p_head.sFileName)
		sFileName = Format(Now, "yyyyMMddHHmmss")
	Else
		Dim sBuf() As String = p_dtFile.p_head.sFileName.Split("#")
		sFileName = sBuf(0)
	End If

	'基準データファイル名指定
	Dim sedlg As frmStdExp = New frmStdExp(sFileName)
	sedlg.ShowDialog()
	If sedlg.DialogResult = DialogResult.Ok
		sFileName = sedlg.FileName
		p_dtFile.RealTimeMeasDataFileSaveText(eMeasMode.Normal, False, p_cts, Path.Combine(SIZUKA_PATH, p_sEquipName, STDDAT_PATH), sFileName )

	End If

End Sub
''' <summary>
''' 基準値表示ボタンクリック
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub btnStdInfo_Click(sender As Object, e As EventArgs) Handles btnStdInfo.Click
#If 1
	If Not frmHistData.Visible
		frmHistData.Show()
	Else
		frmHistData.Close()
	End If
#else

#end if
	End Sub
'基準値表示閉じるボタン
Private Sub btnStdCondClose_Click(sender As Object, e As EventArgs) Handles btnStdCondClose.Click
	gbxStdCond.Visible = Not gbxStdCond.Visible
End Sub
''' <summary>
''' 基準値情報表示
''' </summary>
''' <param name="iSel"></param>
Public Function stdConditionDisp(ByVal iSel As Int32) As Int32
	Dim sHead As DataFile.DataFileHeader = Nothing
	Select Case iSel
	Case 0		'基準値１
		If IsNothing(p_dtFile.iOxBuf_s1)
			Return -1
		Else
			sHead = p_dtFile.p_head_s(0)
		End If
	Case 1		'基準値２
		If IsNothing(p_dtFile.iOxBuf_s2)
			Return -1
		Else
			sHead = p_dtFile.p_head_s(1)
		End If
	Case 2		'測定値
		If IsNothing(p_dtFile.iOxBuf)
			Return -1
		Else
			sHead = p_dtFile.p_head
		End If
	End Select
	p_dtFile.CondDispInitial( dgvStd, sHead )			'DGV初期化
	p_dtFile.CondDispUpdate( dgvStd, sHead )			'DGV値セット
	rbStd1.ForeColor	= p_cts.clrY1Default(7)			'データ選択色セット
	rbStd2.ForeColor	= p_cts.clrY1Default(8)			'データ選択色セット
	rbReal.ForeColor	= p_cts.clrY1Default(1)			'データ選択色セット

	gbxStdCond.Height	= dgvStd.Height + 40
	gbxStdCond.Width	= dgvStd.Width
	gbxStdCond.BackColor= Forms.Control.DefaultBackColor

	dgvStd.Location		= New System.Drawing.Point(0, 40)				'表示初期位置
	Return 0
End Function
''' <summary>
''' データ選択ラジオボタンクリック
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub rbStd1_Click(sender As Object, e As EventArgs) Handles rbStd1.Click, rbStd2.Click, rbReal.Click
	Select Case sender.Name
	Case "rbStd1"
		stdConditionDisp(0)
	Case "rbStd2"
		stdConditionDisp(1)
	Case "rbReal"
		stdConditionDisp(2)
	End Select
End Sub

'------------------------------------------------------------------------

''' <summary>
''' エクスポート テキストファイル
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub TextDataToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles tsmiTextData.Click
	frmFileExport.ShowDialog
End Sub

''' <summary>
''' 数値モニタ切り替え
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub tsmiDigitalMonitorChange_Click(sender As Object, e As EventArgs) Handles tsmiDigitalMonitorChange.Click, btnDigChange.Click
	If pnlPosition.Visible
		pnlPosition.Visible		= False
		pnlSteress.Visible		= True
		pnlCalculation.Visible	= False
	ElseIf pnlSteress.Visible
		pnlPosition.Visible		= False
		pnlSteress.Visible		= False
		pnlCalculation.Visible	= True
	ElseIf pnlCalculation.Visible
		pnlPosition.Visible		= True
		pnlSteress.Visible		= False
		pnlCalculation.Visible	= False
	End If
End Sub

''' <summary>
''' インポートファイル（測定値、基準値） ファイルメニュー
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub tsmiMeasAreaImport_Click(sender As Object, e As EventArgs) Handles tsmiMeasAreaImport.Click, tsmiStdAreaImport1.Click, tsmiStdAreaImport2.Click
	'異種測定モードで混在できない
	Dim iRet	As Int32 = 0
	Dim iArea	As Int32
	Select Case sender.name
	Case "tsmiStdAreaImport1"				'基準値
		iArea = 1
	Case "tsmiStdAreaImport2"				'基準値
		iArea = 2
	Case "tsmiMeasAreaImport"				'測定値
		iArea = 0
	End Select
	Dim dlg As New FrmFileSelectWithPreview(Path.Combine(SIZUKA_PATH), iArea)
	If dlg.ShowDialog() = DialogResult.OK
		For Each grp In dlg.p_Selectfiles
			Try
				'グループ単位で配列化
				Dim selectedFile() As String = grp.Files.ToArray()
				DragDropFilePath = selectedFile
				'選択ファイル読込処理
				modFrmMain.cmsDragDropProc(Me, iArea)
			Catch ex As Exception
				Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
			End Try
		Next
	End If
End Sub

Private Sub LEDLightToolStripMenuItem_Click(sender As Object, e As EventArgs) 
End Sub

''' <summary>
''' LEDライトON/OFF 設定メニュー
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub tsmiLEDLight_Click(sender As Object, e As EventArgs) Handles tsmiLEDLight.Click
	Select Case p_RTM(0).p_mltcp.LedOnOff
	Case 0
		p_RTM(0).p_mltcp.LedOnOff = 1
		tsmiLEDLight.Checked = True

	Case 1
		p_RTM(0).p_mltcp.LedOnOff = 0
		tsmiLEDLight.Checked = False
	End Select
End Sub
''' <summary>
''' ７セグLED明るさ調整
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub tsmiLEDBrightness_SelectedIndexChanged(sender As Object, e As EventArgs) Handles tsmiCmb7SegBright.SelectedIndexChanged
	p_cts.LED7SegBrightness = tsmiCmb7SegBright.SelectedIndex
	If Not IsNothing(p_RTM)
		p_RTM(0).p_mltcp.SSegBrightness = tsmiCmb7SegBright.SelectedIndex
	End If
End Sub


'------------------------------------------------------------------------
'新SPEC入力 イベント処理
''' <summary>
''' デバイスSPECの選択状態変更イベント
'''   選択状態をカラムにより反転表示させない
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub dgvDevSpec_SelectionChanged(sender As Object, e As EventArgs) Handles dgvDevSpec.SelectionChanged, dgvSamSpec.SelectionChanged
Try
	If Not IsNothing(sender.CurrentCell)
		Select Case sender.CurrentCell.ColumnIndex
		Case 0, 1			'アイテム名、単位	
			sender.ClearSelection					'選択反転させない
		Case Else
		End Select
	End If
Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub
Public sPreEditVal As String
''' <summary>
''' デバイスSPEC 数値セル入力開始
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub dgvDevSpec_CellBeginEdit(sender As Object, e As DataGridViewCellCancelEventArgs) Handles dgvDevSpec.CellBeginEdit, dgvSamSpec.CellBeginEdit
Try
	Dim iRows	As int32 = sender.CurrentRow.Index
	sPreEditVal = sender(2, iRows).Value
Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub
''' <summary>
''' デバイスSPEC 数値セル入力完了
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub dgvDevSpec_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs) Handles dgvDevSpec.CellEndEdit
	Dim iRows	As int32 = sender.CurrentRow.Index
	Dim iRet	As int32
Try
	iRet = p_clSpec.CheckDevSpecNumericValue(Ctype(sender, DataGridView))
	If iRet<>0
		'Select Case iRet
		'Case -1		'実数変換エラー
		'	Exit Sub
		'Case -2		'範囲エラー
		'	Exit Sub
		'End Select
		Exit sub
	Else
		'変更後の処理
		Select Case iRows
		Case eDevSPEC.THRETHOLD				'閾値
			If Not IsNothing(p_dtFile.iHosei)
				Select Case p_RTM(0).p_iBGWOrder
				Case BGW_ORDER.START_BUTTON To BGW_ORDER.MEAS_PRO_FINISH
				Case Else
				'測定中以外
					Select Case p_RTM(0).p_sCalcMethod
					Case lstCalcMethod(eCalcMethod.THR), lstCalcMethod(eCalcMethod.PHASE), lstCalcMethod(eCalcMethod.SLOPE)
						'THR法,Phase法,SLOPE法
						ResearchGelTimeOnChart()					'ゲルタイム再検索
						GelTimeLabelDispUpdate(Me)					'ゲルタイム値結果表示
					End Select
				End Select
			End If
		End Select
		'変更があった場合は青
		Dim cColor As color = If(sPreEditVal<>sender(2,iRows).Value, Color.Blue, Color.black)
		sender(2,iRows).Style.ForeColor = cColor
	End If

Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub
''' <summary>
''' サンプルSPEC 数値セル入力完了
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub SamDevSpec_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs) Handles dgvSamSpec.CellEndEdit
	Dim iRows	As int32 = sender.CurrentRow.Index
	Dim iRet	As int32
	Dim s		As string = Nothing
Try

	Select Case iRows
	'サンプル名、測定者、LOT変更でFreeモードにしない
	Case eSamSPEC.SAMPLE
		If Not IsNothing(Ctype(sender, DataGridView)(2,eSamSPEC.SAMPLE).Value)
			Dim ss As String = CType(sender, DataGridView)(2,eSamSPEC.SAMPLE).Value
			If ss.Contains("""") OrElse ss.Contains("\") OrElse ss.Contains("'")
				'元に戻す
				Ctype(sender, DataGridView)(2,eSamSPEC.SAMPLE).Value = p_clSpec.spi.sSampleName
			ElseIf ss.Contains(",")
				'カンマはアンダスコアへ置換え
				Ctype(sender, DataGridView)(2,eSamSPEC.SAMPLE).Value = ss.Replace(",", "_")
			End If
			'p_clSpec.MenuHistAdd(eSamSPEC.SAMPLE, ss)				'履歴メニューへ追加
			p_clSpec.spi.sSampleName	= Ctype(sender, DataGridView)(2,eSamSPEC.SAMPLE).Value
		End If
	Case eSamSPEC.LOT
		If Not IsNothing(Ctype(sender, DataGridView)(2,eSamSPEC.LOT).Value)
			Dim ss As String = CType(sender, DataGridView)(2,eSamSPEC.LOT).Value
			If ss.Contains("""") OrElse ss.Contains("\") OrElse ss.Contains("'")
				'元に戻す
				Ctype(sender, DataGridView)(2,eSamSPEC.LOT).Value = p_clSpec.spi.sLot
			ElseIf ss.Contains(",")
				'カンマはアンダスコアへ置換え
				Ctype(sender, DataGridView)(2,eSamSPEC.LOT).Value = ss.Replace(",","_")
			End If
			ss = Ctype(sender, DataGridView)(2,eSamSPEC.LOT).Value 
			p_clSpec.MenuHistAdd(eSamSPEC.LOT, ss)				'履歴メニューへ追加
			p_clSpec.spi.sLot		= Ctype(sender, DataGridView)(2,eSamSPEC.LOT).Value
		End If
	Case eSamSPEC.MEASURER
		If Not IsNothing(Ctype(sender, DataGridView)(2,eSamSPEC.MEASURER).Value)
			Dim ss As String = CType(sender, DataGridView)(2,eSamSPEC.MEASURER).Value
			If ss.Contains("""") OrElse ss.Contains("\") OrElse ss.Contains("'")
				'元に戻す
				Ctype(sender, DataGridView)(2,eSamSPEC.MEASURER).Value = p_clSpec.spi.sMeasurer
			ElseIf ss.Contains(",")
				'カンマはアンダスコアへ置換え
				Ctype(sender, DataGridView)(2,eSamSPEC.MEASURER).Value = ss.Replace(",","_")
			End If
			ss = Ctype(sender, DataGridView)(2,eSamSPEC.MEASURER).Value 
			'p_clSpec.MenuHistAdd(eSamSPEC.MEASURER, ss)			'履歴メニューへ追加
			p_clSpec.spi.sMeasurer	= Ctype(sender, DataGridView)(2,eSamSPEC.MEASURER).Value
		End If
	Case Else
		iRet = p_clSpec.CheckSamSpecNumericValue(Ctype(sender, DataGridView))
		If iRet<>0
			Select Case iRet
			Case -1		'実数変換エラー
			Case -2		'範囲エラー
			End Select
		Else
		End If
	End Select
	'変更があった場合は青
	Dim cColor As color = If(sPreEditVal<>sender(2,iRows).Value, Color.Blue, Color.black)
	sender(2,iRows).Style.ForeColor = cColor

Catch ex As Exception
	Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Call FrmDebugLog(System.Reflection.MethodBase.GetCurrentMethod.Name & ex.Message)
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub

''' <summary>
''' デバイスSPEC マウスダウン
'''  右クリックで選択メニューを表示する
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub dgvDevSpec_MouseDown(sender As Object, e As MouseEventArgs) Handles dgvDevSpec.MouseDown
	'編集ロック
	If Not p_cts.AdminEnable
		Exit Sub
	End If
	If e.Button = MouseButtons.Right
		Dim grid As DataGridView = CType(sender, DataGridView)
		'指定された（クリックされた）座標位置の情報を取得する
		Dim hti As System.Windows.Forms.DataGridView.HitTestInfo
		hti = grid.HitTest(e.X, e.Y)
		If Not IsNothing(sender.CurrentRow)
			If sender.CurrentRow.Index = hti.RowIndex
				sPreEditVal = dgvDevSpec(2, sender.currentrow.index).Value	'変更前を退避
				DevSpecContextMenuShow(hti.RowIndex)					'コンボリスト表示
			End If
		End If
	End If

End Sub
''' <summary>
''' サンプルSPEC マウスダウン
'''  右クリックで選択メニューを表示する
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub dgvSamSpec_MouseDown(sender As Object, e As MouseEventArgs) Handles dgvSamSpec.MouseDown

	'編集ロック
	If Not p_cts.AdminEnable
		Exit Sub
	End If
	If e.Button = MouseButtons.Right
		Dim grid As DataGridView = CType(sender, DataGridView)
		'指定された（クリックされた）座標位置の情報を取得する
		Dim hti As System.Windows.Forms.DataGridView.HitTestInfo
		hti = grid.HitTest(e.X, e.Y)
		If Not IsNothing(sender.CurrentRow)
			If sender.CurrentRow.Index = hti.RowIndex
				SamSpecContextMenuShow(hti.RowIndex)

			End If
		End If
	End If

End Sub
''' <summary>
''' デバイスSPEC キーダウン
'''   SPACEキーで選択メニューを表示する
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub dgvDevSpec_KeyDown(sender As Object, e As KeyEventArgs) Handles dgvDevSpec.KeyDown
	Dim dgv = CType(sender, DataGridView)
	Select Case e.KeyCode
	Case Keys.Space
		'編集ロック
		If Not p_cts.AdminEnable
			Exit Sub
		End If
		DevSpecContextMenuShow(dgv.CurrentRow.Index)

	Case Keys.Tab
		e.Handled = True
	End Select
End Sub
''' <summary>
''' サンプルSPEC キーダウン
'''   SPACEキーで選択メニューを表示する
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub dgvSamSpec_KeyDown(sender As Object, e As KeyEventArgs) Handles dgvSamSpec.KeyDown
	Dim dgv = CType(sender, DataGridView)
	Select Case e.KeyCode
	Case Keys.Space
		'編集ロック
		If Not p_cts.AdminEnable
			Exit Sub
		End If
		SamSpecContextMenuShow(dgv.CurrentRow.Index)
	Case Keys.Tab
		e.Handled = True
	End Select
End Sub
''' <summary>
''' デバイスSPEC 選択メニュー表示処理 コンテキストメニューストリップ
''' </summary>
''' <param name="RowIdx"></param>
Private Sub DevSpecContextMenuShow( ByVal RowIdx As Int32 )
	Dim r As Object
	Select Case RowIdx
	Case eDevSPEC.HP_CONT
		r = dgvDevSpec.GetCellDisplayRectangle(2, eDevSPEC.HP_CONT, False)
		p_clSpec.SelectMenuDisp(eSpec.Device, cmsSpecSelect, eDevSPEC.HP_CONT, dgvDevSpec )
		cmsSpecSelect.Show(dgvDevSpec, r.Location)

	Case eDevSPEC.METHOD
		r = dgvDevSpec.GetCellDisplayRectangle(2, eDevSPEC.METHOD, False)
		p_clSpec.SelectMenuDisp(eSpec.Device, cmsSpecSelect,eDevSPEC.METHOD, dgvDevSpec)
		cmsSpecSelect.Show(dgvDevSpec, r.Location)

	Case eDevSPEC.STRS_SENS_MODE
		r = dgvDevSpec.GetCellDisplayRectangle(2, eDevSPEC.STRS_SENS_MODE, False)
		p_clSpec.SelectMenuDisp(eSpec.Device, cmsSpecSelect,eDevSPEC.STRS_SENS_MODE, dgvDevSpec)
		cmsSpecSelect.Show(dgvDevSpec, r.Location)

	Case eDevSPEC.AMP_MODE
		r = dgvDevSpec.GetCellDisplayRectangle(2, eDevSPEC.AMP_MODE, False)
		p_clSpec.SelectMenuDisp(eSpec.Device, cmsSpecSelect,eDevSPEC.AMP_MODE, dgvDevSpec)
		cmsSpecSelect.Show(dgvDevSpec, r.Location)

	'Case eDevSPEC.PB_INP_MODE
	'	r = dgvDevSpec.GetCellDisplayRectangle(2, eDevSPEC.PB_INP_MODE, False)
	'	p_clSpec.SelectMenuDisp(eSpec.Device, cmsSpecSelect,eDevSPEC.PB_INP_MODE, dgvDevSpec)
	'	cmsSpecSelect.Show(dgvDevSpec, r.Location)
	End Select
End Sub
''' <summary>
''' サンプルSPEC 選択メニュー表示処理 コンテキストメニューストリップ
''' </summary>
''' <param name="RowIdx"></param>
Private Sub SamSpecContextMenuShow( ByVal RowIdx As Int32 )
	Dim r As Object
	Select Case RowIdx
	Case eSamSPEC.SAMPLE
		r = dgvSamSpec.GetCellDisplayRectangle(2, eSamSPEC.SAMPLE, False)
		p_clSpec.SelectMenuDisp(eSpec.Sample, cmsSpecSelect, eSamSPEC.SAMPLE, dgvSamSpec )
		cmsSpecSelect.Show(dgvSamSpec, r.Location)
	Case eSamSPEC.LOT
		r = dgvSamSpec.GetCellDisplayRectangle(2, eSamSPEC.LOT, False)
		p_clSpec.SelectMenuDisp(eSpec.Sample, cmsSpecSelect, eSamSPEC.LOT, dgvSamSpec )
		cmsSpecSelect.Show(dgvSamSpec, r.Location)
	Case eSamSPEC.MEASURER
		r = dgvSamSpec.GetCellDisplayRectangle(2, eSamSPEC.MEASURER, False)
		p_clSpec.SelectMenuDisp(eSpec.Sample, cmsSpecSelect, eSamSPEC.MEASURER, dgvSamSpec )
		cmsSpecSelect.Show(dgvSamSpec, r.Location)
	Case eSamSPEC.SHAPE
		r = dgvSamSpec.GetCellDisplayRectangle(2, eSamSPEC.SHAPE, False)
		p_clSpec.SelectMenuDisp(eSpec.Sample, cmsSpecSelect, eSamSPEC.SHAPE, dgvSamSpec )
		cmsSpecSelect.Show(dgvSamSpec, r.Location)

	End Select
End Sub

''' <summary>
''' デバイスSPEC セルダブルクリック
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub dgvDevSpec_CellMouseDoubleClick(sender As Object, e As DataGridViewCellMouseEventArgs) Handles dgvDevSpec.CellMouseDoubleClick
Dim dgvTemp As DataGridView
Try
	'編集ロック
	If Not p_cts.AdminEnable
		Exit Sub
	End If
	dgvTemp = CType(sender, DataGridView)
	Select Case e.RowIndex
	Case eDevSPEC.TEMP_PROFILE
		'入力ダイアログを表示
		Dim fcdlg As frmSetRTGSet = New frmSetRTGSet(dgvTemp.Item(1, e.RowIndex).Value.ToString)
		If fcdlg.ShowDialog() = Windows.Forms.DialogResult.OK Then
			'複数選択されている場合、選択行全てにに同じ値を代入する
			dgvTemp.Item(2, e.RowIndex).Value = fcdlg.GetVal()
			If dgvTemp.Item(2, e.RowIndex).Value <> vbNullString Then
				'初期温度を設定
				p_RTM(0).p_strRTGSetName		= dgvTemp.Item(2, e.RowIndex).Value
				p_RTM(0).sbLoadRTGSetting(p_RTM(0).p_strRTGSetName)
				p_RTM(0).p_mltcp.TempPreset(1)	= p_RTM(0).p_rtgset.mdbr(0).dHPTemp
				p_RTM(0).p_mltcp.Temp2Preset(1) = p_RTM(0).p_rtgset.mdbr(0).dHPTemp
				p_RTM(0).p_mltcp.Temp3Preset(1) = p_clSpec.SGTempGet(p_RTM(0).p_rtgset.mdbr(0).dHPTemp)
			Else
				p_RTM(0).p_strRTGSetName = ""
			End If
			dgvTemp.EndEdit()
		End If
		If p_clSpec.spi.sTempProfileName <> p_RTM(0).p_strRTGSetName
			p_clSpec.spi.sTempProfileName = p_RTM(0).p_strRTGSetName
			DevSpecToFree()			'Freeモードへ
		End If
		fcdlg.Dispose()
	End Select
Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub
''' <summary>
''' サンプルSPEC セルダブルクリック
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub dgvSamSpec_CellMouseDoubleClick(sender As Object, e As DataGridViewCellMouseEventArgs) Handles dgvSamSpec.CellMouseDoubleClick
Dim iRet	As Int32
Try
	'編集ロック
	If Not p_cts.AdminEnable
		Exit Sub
	Else
		p_clSpec.CheckSpecTimeSpanInput(sender, e.RowIndex, True)
		'チャート更新 ゲルタイム判定更新
		'ChartDispUpdate(Me, 0, False)
		iRet = p_RTM(0).GelTimeSeach(p_iSelectGItem)			'ゲルタイム再検索
		ResearchGelTimeOnChart()								'ゲルタイム表示更新
		GelTimeJudgeColorUpdate(Me)								'ゲルタイム判定表示
		GelTimeLabelDispUpdate(Me)								'ゲルタイム値更新
		ChartDispUpdate(Me, 0, False)
	End If
#If 0
Dim dgvTemp As DataGridView
	dgvTemp = CType(sender, DataGridView)
	Select Case e.RowIndex
	Case eSamSPEC.JUDGE_RANGE, eSamSPEC.EFECT_RANGE
		'入力ダイアログを表示
		Dim strApprox() As String = dgvTemp.Item(2, e.RowIndex).Value.ToString.Split("-")
		Dim sRange()	As String = Nothing
		sRange = p_clSpec.sSamInputRange_AppStdTime(If(p_cts.LTModeEnable,1,0)).Split("-")
		Dim adlg As frmSetRange = New frmSetRange(strApprox(0), strApprox(1), 1)
		If adlg.ShowDialog() = Windows.Forms.DialogResult.OK Then
			Dim dMin As Double
			Dim dMax As Double
			dMin = adlg.GetMin()
			dMax = adlg.GetMax()
			If dMin < 0.0R Then
				dMin = 0.0R
			End If
			If dMax < dMin Then
				dMax = dMin + 0.1R
			End If
			'Select Case e.RowIndex
			'Case eCALC_ROW.iMASK_RANGE
				If Double.Parse(dgvDevSpec(2, eDevSPEC.MEAS_TIME).Value)<>0
					'マスクレンジの最大値は測定終了時間より大きい値は不可
					If dMax > Double.Parse(dgvDevSpec(2, eDevSPEC.MEAS_TIME).Value) Then
						dMax = Double.Parse(dgvDevSpec(2, eDevSPEC.MEAS_TIME).Value)
					End If
				End If
				If dMax > sRange(1) Or dMin < sRange(0)
				'範囲チェックNG
					UnderStatusLabelDisp( ToolStripStatusLabel1, sUnderMsg1(1), Color.Orange)					'範囲外
				End If
			'Case eCALC_ROW.iGEL_RANGE
			'End Select
			dgvTemp.Item(2, e.RowIndex).Value = String.Format("{0:0.0}", dMin) + " - " + String.Format("{0:0.0}", dMax)
			Select Case e.RowIndex
			Case eSamSPEC.JUDGE_RANGE
				If p_clSpec.spi.dJudgeRangeMin <> dMin
					p_clSpec.spi.dJudgeRangeMin = dMin
					p_RTM(0).p_dJudgeRangeMin	= dMin
					'p_dtFile.p_head.iState		= 0			'測定データ状態のクリア
					DevSpecToFree()			'Freeモードへ
				End If
				If p_clSpec.spi.dJudgeRangeMax <> dMax
					p_clSpec.spi.dJudgeRangeMax = dMax
					p_RTM(0).p_dJudgeRangeMax	= dMax
					'p_dtFile.p_head.iState		= 0			'測定データ状態のクリア
					DevSpecToFree()			'Freeモードへ
				End If

			Case eSamSPEC.EFECT_RANGE
				If p_clSpec.spi.dEffectRangeMin <> dMin
					p_clSpec.spi.dEffectRangeMin = dMin
					p_RTM(0).p_dAprxRangeMin	 = dMin
					'p_dtFile.p_head.iState		= 0			'測定データ状態のクリア
					DevSpecToFree()			'Freeモードへ
				End If
				If p_clSpec.spi.dEffectRangeMax <> dMax
					p_clSpec.spi.dEffectRangeMax = dMax
					p_RTM(0).p_dAprxRangeMax	 = dMax
					'p_dtFile.p_head.iState		= 0			'測定データ状態のクリア
					DevSpecToFree()			'Freeモードへ
				End If
			End Select
			'チャート更新 ゲルタイム判定更新
			'ChartDispUpdate(Me, 0, False)
			iRet = p_RTM(0).GelTimeSeach(p_iSelectGItem)			'ゲルタイム再検索
			ResearchGelTimeOnChart()								'ゲルタイム表示更新
			GelTimeJudgeColorUpdate(Me)								'ゲルタイム判定表示
			GelTimeLabelDispUpdate(Me)								'ゲルタイム値更新
			ChartDispUpdate(Me, 0, False)

			adlg.Dispose()
		End If
	End Select
#end if
Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub
Dim iDevFree As Int32 = 0

''' <summary>
''' デバイスSPEC Name 切替 コンボリスト
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub cbxDevSPEC_SelectionChangeCommitted(sender As Object, e As EventArgs) Handles cbxDevSPEC.SelectionChangeCommitted
	Dim s				As New ArrayList										'CSVファイルIF用のアレイリスト
	Dim sSpecDir		As String = Path.Combine(SIZUKA_PATH, p_sEquipName, DEVSPEC_PATH)
	Dim sSpecName		As String
Try
	If Me.ActiveControl Is cbxDevSPEC
	'ユーザーが操作して変更があった時のみ動作
		If cbxDevSPEC.Text.Length > 0 'And (iDevFree = 0)
			'SPEC名がFree以外の時、固定SPECをファイルから読込む その前にFreeをセーブ
			If cbxDevSPEC.Text<>cFree
				sSpecName = cFree
				p_clSpec.specWrite(eSpec.Device, Me, sSpecName)

			End If
		
			'固定SPECを読込
			sSpecName = cbxDevSPEC.Text
			p_clSpec.dgvSpecDispUpdate(eSpec.Device, Me, sSpecName)

			MethodChanged(p_clSpec.spi.iGelTimeMethod)											'判定方法による初期表示
			Call WriteLog("DeviceSpec 変更 " & sSpecName, eLOG_LEVEL.iINFO)

		Else
			iDevFree = 0
		End If
		'If cbxDevSPEC.Text = cFree
		'	cbxDevSPEC.BackColor = Color.LemonChiffon
		'Else
		'	cbxDevSPEC.BackColor = Color.White
		'End If

		If dgvDevSpec.Item(2, eDevSPEC.TEMP_PROFILE).Value <> vbNullString
		'設定された温度プロファイル
			Dim bRet As Boolean = p_RTM(0).sbLoadRTGSetting(dgvDevSpec.Item(2, eDevSPEC.TEMP_PROFILE).Value)
			If Not bRet
				dgvDevSpec.Item(2, eDevSPEC.TEMP_PROFILE).Value = ""
				p_RTM(0).p_strRTGSetName = dgvDevSpec.Item(2, eDevSPEC.TEMP_PROFILE).Value
			Else
				'初期温度を設定
				p_RTM(0).p_strRTGSetName = dgvDevSpec.Item(2, eDevSPEC.TEMP_PROFILE).Value
				p_RTM(0).p_mltcp.TempPreset(1)	= p_RTM(0).p_rtgset.mdbr(0).dHPTemp
				p_RTM(0).p_mltcp.Temp2Preset(1) = p_RTM(0).p_rtgset.mdbr(0).dHPTemp
				p_RTM(0).p_mltcp.Temp3Preset(1) = p_clSpec.SGTempGet(p_RTM(0).p_rtgset.mdbr(0).dHPTemp)		'起歪体温度
			End If
		Else
			p_RTM(0).p_strRTGSetName = ""
		End If

	End If
Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub
Dim iSamFree As Int32 = 0
''' <summary>
''' サンプルSPEC Name 切替 コンボリスト
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub cbxSamSPEC_SelectionChangeCommitted(sender As Object, e As EventArgs) Handles cbxSamSPEC.SelectionChangeCommitted
	Dim s				As New ArrayList										'CSVファイルIF用のアレイリスト
	Dim sSpecDir		As String = Path.Combine(SIZUKA_PATH, p_sEquipName, SAMSPEC_PATH)
	Dim sSpecName		As String
Try
	If Me.ActiveControl Is cbxSamSPEC
	'ユーザーが操作して変更があった時のみ動作
		If cbxSamSPEC.Text.Length > 0 'And (cbxSamSPEC.Text<>cFree And iSamFree = 0)
			'SPEC名がFree以外の時、固定SPECをファイルから読込む その前にFreeをセーブ
			If cbxSamSPEC.Text<>cFree

				sSpecName = cFree
				p_clSpec.specWrite(eSpec.Sample, Me, sSpecName)

			End If

			'固定SPECを読込
			sSpecName = cbxSamSPEC.Text
			p_clSpec.dgvSpecDispUpdate(eSpec.Sample, Me, sSpecName)
			Call WriteLog("SampleSpec 変更 " & sSpecName, eLOG_LEVEL.iINFO)

		Else
			iSamFree = 0
		End If
		'If cbxSamSPEC.Text = cFree
		'	cbxSamSPEC.BackColor = Color.LemonChiffon
		'Else
		'	cbxSamSPEC.BackColor = Color.White
		'End If
	End If
Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub
''' <summary>
''' デバイスSPEC動作モードをFreeとする
''' </summary>
Public Sub DevSpecToFree()
	iDevFree = 1
	cbxDevSPEC.SetTextEx(cFree, cbxDevSPEC.BackColor)
End Sub
''' <summary>
''' サンプルSPEC動作モードをFreeとする
''' </summary>
Public Sub SamSpecToFree()
	iSamFree = 1
	cbxSamSPEC.SetTextEx(cFree, cbxSamSPEC.BackColor)
End Sub

'計算手法変更処理
Public Sub MethodChanged(ByVal iCalc As Int32)
Try
	'外挿法パラメータPAD表示制御
	Select Case iCalc
	Case eCalcMethod.THR, eCalcMethod.SLOPE
		gbxExpoPad.Visible = False
		rbCorrect.Checked = True
		p_sSelectGItem = rbCorrect.Name
		p_iSelectGItem = Array.indexof(sGraphSelectionArry, p_sSelectGItem)
		tsmiXAxisTemp.Checked	= False
		rbXTime.Checked			= True

	Case eCalcMethod.PHASE
		gbxExpoPad.Visible = False
		rbPosPhase.Checked = True
		p_sSelectGItem = rbPosPhase.Name
		p_iSelectGItem = Array.indexof(sGraphSelectionArry, p_sSelectGItem)
		tsmiXAxisTemp.Checked	= False
		rbXTime.Checked			= True

	Case eCalcMethod.MELT
		gbxExpoPad.Visible = False
		rbViscosity.Checked = True
		p_sSelectGItem = rbViscosity.Name
		p_iSelectGItem = Array.indexof(sGraphSelectionArry, p_sSelectGItem)
		'tsmiXAxisTemp.Checked = True

	Case eCalcMethod.EXTRA
		gbxExpoPad.Visible = True
		'時間換算レートをセット
		clExtra.TimeToSampleRate = (MEAS_TIME_RESOLUTION / MEAS_TIME_BLOCK) * RealTimeThinning(1)
		If IsNothing(p_dtFile.iHosei) Then
			btnExtrapolationSave.Enabled = False
		Else
			btnExtrapolationSave.Enabled = True
		End If
		tsmiXAxisTemp.Checked	= False
		rbXTime.Checked			= True

	End Select

	p_cts.iChartType = If(tsmiXAxisTemp.Checked, 1, 0)
	m_RealTrendUpdate = False                           'リアルタイムチャート初期化
	ChartDispUpdate(Me, 0, m_RealTrendUpdate)

Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub




''' <summary>
''' Y軸表示倍率 変更
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub nudYAxisZoom_ValueChanged(sender As Object, e As EventArgs) Handles nudYAxisZoom.ValueChanged
	If Not IsNothing(p_RTM)
		p_RTM(0).p_dYDispArea = CType(sender, NumericUpDown).Value
		If p_iScrlStart = 0
			ChartDispUpdate(Me, 0, m_RealTrendUpdate)		'チャート表示更新
		Else
			ChartDispUpdate(Me, 1, m_RealTrendUpdate)		'チャート表示更新
		End If
	End If
End Sub
''' <summary>
''' Z応力モニタ表示ラベル-ダブルクリック
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub lblCurrZStrsVal_DoubleClick(sender As Object, e As EventArgs) Handles lblCurrZStrsVal.DoubleClick
	If p_RTM(0).p_iConnectState = CONNECT_ST.CONNECTING														'ONLINE
		If p_RTM(0).p_MeasureState = RealTimeMeas.eMeasStat.NOT_START										'未測定中
			If p_RTM(0).p_iBGWOrder = BGW_ORDER.NO_ORDER And p_RTM(0).p_iBGWOrderNext = BGW_ORDER.NO_ORDER	'アイドル中
				p_RTM(0).p_iBGWOrder = BGW_ORDER.ZNOLOAD_STRESS_SET_REP										'Z無負荷応力設定制御開始

				UnderStatusLabelDisp(ToolStripStatusLabel1, "Z無負荷応力設定を要求しました。")

			End If
		End If
	End If
End Sub

''' <summary>
''' SPECファイル編集ボタンイベント
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub btnDevSpecNew_Click(sender As Object, e As EventArgs) Handles btnDevSpecEdit.Click, btnSamSpecEdit.Click
	'Dim s			As ArrayList
	'Dim sSpecDir	As String
	Dim sSpecName	As String

	Select Case sender.name
	Case "btnDevSpecEdit"
		'現状ファイルへ書込み
		sSpecName = cbxDevSPEC.Text
		p_clSpec.specWrite(eSpec.Device, Me, sSpecName)

		Dim frmD		As New frmSpecNameInp(eSpec.Device, sSpecName)
		'編集ダイアログ表示
		frmD.ShowDialog()
		frmD.Dispose()

		'SPEC選択コンボ更新
		p_clSpec.SpecFileGet(cbxDevSPEC)
		If cbxDevSPEC.Items.IndexOf(sSpecName) = -1
		'削除された
			cbxDevSPEC.Text = cFree
			'SPEC内容を更新
			sSpecName = cbxDevSPEC.Text
		Else
			cbxDevSPEC.Text = sSpecName
		End If
		p_clSpec.dgvSpecDispUpdate(eSpec.Device, Me, sSpecName)

	Case "btnSamSpecEdit"
		'現状ファイルへ書込み
		sSpecName = cbxSamSPEC.Text
		p_clSpec.specWrite(eSpec.Sample, Me, sSpecName)

		Dim frmD		As New frmSpecNameInp(eSpec.Sample, sSpecName)
		'編集ダイアログ表示
		frmD.ShowDialog()
		frmD.Dispose()

		'SPEC選択コンボ更新
		p_clSpec.SpecFileGet(cbxSamSPEC)
		If cbxSamSPEC.Items.IndexOf(sSpecName) = -1
		'削除された
			cbxSamSPEC.Text = cFree
			'SPEC内容を更新
			sSpecName = cbxSamSPEC.Text
		Else
			cbxSamSPEC.Text = sSpecName
		End If
		p_clSpec.dgvSpecDispUpdate(eSpec.Sample, Me, sSpecName)
	End Select

End Sub

Dim buttonToolTip As New ToolTip
''' <summary>
''' SPECコンボボックスツールチップを表示
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub cbxSPEC_MouseHover(sender As Object, e As EventArgs) Handles cbxSamSPEC.MouseHover, cbxDevSPEC.MouseHover
	Dim cbx As ComboBox = If(sender.Name="cbxDevSPEC", cbxDevSPEC,cbxSamSPEC)
	buttonToolTip.ToolTipTitle = ""
	buttonToolTip.UseFading = True
	buttonToolTip.UseAnimation = True
	buttonToolTip.IsBalloon = True
	buttonToolTip.ShowAlways = True
	buttonToolTip.AutoPopDelay = 5000
	buttonToolTip.InitialDelay = 200
	buttonToolTip.ReshowDelay = 0
	buttonToolTip.SetToolTip(cbx, cbx.Text)

	'リスト部分の幅を限界まで広げる
	cbx.DropDownWidth = cbx.Width + (pnlSPEC.width - cbx.Right)
End Sub


''' <summary>
''' X軸固定スパン更新ボタン
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub btnXSpanAply_Click(sender As Object, e As EventArgs) Handles btnXSpanAply.Click
Try
	If nudXscaleMax.Value - nudXscaleMin.Value <= 0
		nudXscaleMax.BackColor = Color.Yellow
		nudXscaleMin.BackColor = Color.Yellow
	ElseIf p_bMeasTimeEn AndAlso nudXscaleMax.Value - nudXscaleMin.Value > 4000
		nudXscaleMax.BackColor = Color.Yellow
		nudXscaleMin.BackColor = Color.Yellow
	Else
		Dim iRet As Int32
		'Y軸範囲チェック
		iRet = YSpanCheck()
		If iRet<>0
			Exit Sub
		End if
		p_cts.MainGraphXMax(p_cts.iChartType)		= nudXscaleMax.Value
		p_cts.MainGraphXMin(p_cts.iChartType)		= nudXscaleMin.Value
		p_cts.MainGraphXDiv(p_cts.iChartType)		= nudXdivision.Value
		nudXscaleMax.BackColor = Color.White
		nudXscaleMin.BackColor = Color.White
		m_RealTrendUpdate = False
		ChartDispUpdate(Me, 0, m_RealTrendUpdate)           'チャート表示更新 ------------------
		'dPreInpXMax = nudXscaleMax.Value
	End If
Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub
''' <summary>
''' Y軸固定スパン更新ボタン
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub btnYSpanAply_Click(sender As Object, e As EventArgs) Handles btnYSpanAply.Click
	If nudYscaleMax.Value - nudYscaleMin.Value > 0
		Dim iRet As Int32
		'Y軸範囲チェック
		iRet = YSpanCheck()
		If iRet<>0
			Exit Sub
		End if
		p_cts.MainGraphYMax(p_iSelectGItem)		= nudYscaleMax.Value
		p_cts.MainGraphYMin(p_iSelectGItem)		= nudYscaleMin.Value
		p_cts.MainGraphYDiv(p_iSelectGItem)		= nudYdivision.Value
		nudYscaleMax.BackColor = Color.White
		nudYscaleMin.BackColor = Color.White
		m_RealTrendUpdate = False
		ChartDispUpdate(Me, 0, m_RealTrendUpdate)           'チャート表示更新 ------------------
		'dPreInpYMax = nudYscaleMax.Value

	Else
		nudYscaleMax.BackColor = Color.Yellow
		nudYscaleMin.BackColor = Color.Yellow
	End If
End Sub
'Dim dPreInpXMin As Double = 0
'Dim dPreInpYMin As Double = 0
'Dim dPreInpXMax As Double = 0
'Dim dPreInpYMax As Double = 0
'Dim dChgXRatio	As Double = 0
'Dim dChgYRatio	As Double = 0

''' <summary>
''' グラフ表示範囲の安全チェック(過度なズームはチャート内でオーバーフローエラーが発生する)
''' </summary>
''' <returns>0:安全 -1:例外発生 1:安全でない 2:Y範囲異常</returns>
Private Function YSpanCheck() As Int32
Try
	Dim dXSpan			As Double = If( p_cts.iRTXAxis = 1, nudXscaleMax.Value - nudXscaleMin.Value, (Chart1.ChartAreas(0).AxisX.Maximum - Chart1.ChartAreas(0).AxisX.Minimum)/50)
	Dim dYMax			As Double = 0
	Dim dYMin			As Double = 0
	Dim dXMax			As Double = 0
	Dim dXMin			As Double = 0
	Dim ScaleFactorX	As Double = 0
	Dim ScaleFactorY	As Double = 0
	Dim dThresh			As Double = 0				'安全度しきい値
	Dim dThreshYRange	As Double = 0				'Y軸指定範囲しきい値
	Dim iRet			As Int32 = 0
	Dim sMsg			As String = ""
	Dim sMsg1			As String = ""
	Dim sMsg2			As String = ""
	'Dim dRangeLimX		As Double
	Dim dRangeLimY		As Double
	'Dim ScaleFactorXY	As Double
	Dim dThreshPer		As Double
	Dim dThreshPerLim	As Double
	'警告メッセージの初期設定
	lblRangeWarning.TextAlign =ContentAlignment.TopLeft

	'X軸対象データの最大値を取得する
	If Not IsNothing(p_dtFile.iOxBuf)
		If p_cts.iChartType = 0
			dXMax = p_dtFile.iOxBuf.Length * 2 / 1000		'秒数換算
		Else
			dXMax = p_dtFile.iTempBuf.Max / 10.0			'温度換算
		End If
	Else
		Return 0
	End If
	Dim dXSpanInp		As Double	= If( p_cts.iRTXAxis = 1, nudXscaleMax.Value - nudXscaleMin.Value, (Chart1.ChartAreas(0).AxisX.Maximum - Chart1.ChartAreas(0).AxisX.Minimum)/50)
	Dim dYSpanInp		As Double	= If( p_cts.iRTYAxis = 1, nudYscaleMax.Value - nudYscaleMin.Value, (Chart1.ChartAreas(0).AxisY.Maximum - Chart1.ChartAreas(0).AxisY.Minimum))
	'Y軸対象データの最小・最大値を取得する
	Select Case p_iSelectGItem
	Case iGrpItmN.rbCorrect
		dYMax=p_dtFile.iHosei.Max
		dYMin=If(Double.IsNaN(p_dtFile.iHosei.Min), 0, p_dtFile.iHosei.Min)
		dThreshPer = dYSpanInp / (dYMax - dYMin) * 100

	Case iGrpItmN.rbCorrectP
		dYMax=p_dtFile.dHosei100.Max
		dYMin=If(Double.IsNaN(p_dtFile.dHosei100.Min), 0, p_dtFile.dHosei100.Min)

	Case iGrpItmN.rbViscosity
		dYMax=p_dtFile.dViscosity.Max
		dYMin=If(Double.IsNaN(p_dtFile.dViscosity.Min), 0, p_dtFile.dViscosity.Min)
		dThreshPer		= dYSpanInp / (dYMax - dYMin) * 100
		dThreshPerLim	= 1E-4

	Case iGrpItmN.rbPosPhase
		dYMax=p_dtFile.dXPosPhase.Max
		dYMin=If(Double.IsNaN(p_dtFile.dXPosPhase.Min), 0, p_dtFile.dXPosPhase.Min)

	Case iGrpItmN.rbSampleStrain
		dYMax=p_dtFile.dSplStrain.Max
		dYMin=If(Double.IsNaN(p_dtFile.dSplStrain.Min), 0, p_dtFile.dSplStrain.Min)

	Case iGrpItmN.rbShearMod
		dYMax=p_dtFile.dShearMod.Max
		dYMin=If(Double.IsNaN(p_dtFile.dShearMod.Min), 0, p_dtFile.dShearMod.Min)
		dThreshPer		= dYSpanInp / (dYMax - dYMin) * 100
		dThreshPerLim	= 1E-3

	Case iGrpItmN.rbShearVelo
		dYMax=p_dtFile.dShearVelo.Max
		dYMin=If(Double.IsNaN(p_dtFile.dShearVelo.Min), 0, p_dtFile.dShearVelo.Min)

	Case iGrpItmN.rbStorageMod
		dYMax=p_dtFile.dXStrgMod.Max
		dYMin=If(Double.IsNaN(p_dtFile.dXStrgMod.Min), 0, p_dtFile.dXStrgMod.Min)
		dThreshPer		= dYSpanInp / (dYMax - dYMin) * 100
		dThreshPerLim	= 1E-4

	Case iGrpItmN.rbElasticMod
		dYMax=p_dtFile.dXElasMod.Max
		dYMin=If(Double.IsNaN(p_dtFile.dXElasMod.Min), 0, p_dtFile.dXElasMod.Min)
		dThreshPer		= dYSpanInp / (dYMax - dYMin) * 100
		dThreshPerLim	= 1E-3

	Case iGrpItmN.rbLossTangent
		dYMax=p_dtFile.dXLossTan.Max
		dYMin=If(Double.IsNaN(p_dtFile.dXLossTan.Min), 0, p_dtFile.dXLossTan.Min)

	Case iGrpItmN.rbXStress
		dYMax=p_dtFile.iOxBuf.Max
		dYMin=p_dtFile.iOxBuf.Min
		dThreshYRange = 10
		If dYSpanInp < dThreshYRange
			iRet = 2
		End If
	Case iGrpItmN.rbYStress
		dYMax=p_dtFile.iOyBuf.Max
		dYMin=p_dtFile.iOyBuf.Min
		dThreshYRange = 10
		If dYSpanInp < dThreshYRange
			iRet = 2
		End If
	Case iGrpItmN.rbZStress
		dYMax=p_dtFile.dOzBuf.Max
		dYMin=If(Double.IsNaN(p_dtFile.dOzBuf.Min), 0, p_dtFile.dOzBuf.Min)
		dThreshYRange = 1
		If dYSpanInp < dThreshYRange
			iRet = 2
		End If
	Case iGrpItmN.rbZPosition
		dYMax=p_dtFile.iPzBuf.Max
		dYMin=p_dtFile.iPzBuf.Min
		dThreshYRange = 0.1
		If dYSpanInp < dThreshYRange
			iRet = 2
		End If
	Case iGrpItmN.rbHPTemp
		dYMax=p_dtFile.iTempBuf.Max / 10
		dYMin=p_dtFile.iTempBuf.Min / 10
		dThreshYRange = 1
		If dYSpanInp < dThreshYRange
			iRet = 2
		End If
	Case iGrpItmN.rbHPMV
		dYMax=p_dtFile.iMvBuf.Max / 10
		dYMin=p_dtFile.iMvBuf.Min / 10
		dThreshYRange = 1
		If dYSpanInp < dThreshYRange
			iRet = 2
		End If
	Case iGrpItmN.rbPBTemp
		dYMax=p_dtFile.iTempBuf2.Max / 10
		dYMin=p_dtFile.iTempBuf2.Min / 10
		dThreshYRange = 1
		If dYSpanInp < dThreshYRange
			iRet = 2
		End If
	Case iGrpItmN.rbPBMV
		dYMax=p_dtFile.iMvBuf2.Max / 10
		dYMin=p_dtFile.iMvBuf2.Min / 10
		dThreshYRange = 1
		If dYSpanInp < dThreshYRange
			iRet = 2
		End If
	End Select
	Dim dXSpanData		As Double	= dXMax - dXMin
	Dim dYSpanData		As Double	= dYMax - dYMin
#If 1
	Select Case iRet
    Case 2	'Y範囲設定外
		sMsg  = vbCrLf & Space(4) & p_sMes(efrmMainSTR.iGrpRangeTooSmall) & Space(6) 
		sMsg1 = Space(4) & p_sMes(efrmMainSTR.iGrpYAxisRangeOrder)
		sMsg1 = sMsg1.Replace("XXXX",dThreshYRange.ToString("F3"))
		lblRangeWarning.Text = sMsg & vbCrLf & sMsg1 & vbCrLf & " "
		lblRangeWarning.Location = New Drawing.Point((Chart1.Left+Chart1.Width)/2-lblRangeWarning.Width/2, (Chart1.Top+Chart1.Height)/2)
		nudYscaleMax.BackColor = Color.Yellow
		nudYscaleMin.BackColor = Color.Yellow
		lblRangeWarning.Visible = True
		Return iRet
	Case 3	'X範囲設定外
		sMsg  = vbCrLf & Space(4) & p_sMes(efrmMainSTR.iGrpRangeTooSmall) & Space(6) 
		sMsg1 = Space(4) & p_sMes(efrmMainSTR.iGrpXAxisRangeOrder)
		sMsg1 = sMsg1.Replace("XXXX",dThreshYRange.ToString("F3"))
		lblRangeWarning.Text = sMsg & vbCrLf & sMsg1 & vbCrLf & " "
		lblRangeWarning.Location = New Drawing.Point((Chart1.Left+Chart1.Width)/2-lblRangeWarning.Width/2, (Chart1.Top+Chart1.Height)/2)
		nudXscaleMax.BackColor = Color.Yellow
		nudXscaleMin.BackColor = Color.Yellow
		lblRangeWarning.Visible = True
		Return iRet
	End Select
#end if
#If 0

	'軸のデータ範囲と指示範囲を取得する
	'Y軸データ範囲によりしきい値を変更する
	Select Case dYSpanData
	Case > 1E9
		dThresh = 1E5
	Case > 1E8
		dThresh = 1E4
	Case Else
		Select Case p_iSelectGItem
		Case iGrpItmN.rbZPosition, iGrpItmN.rbXStress, iGrpItmN.rbYStress, iGrpItmN.rbZStress
			dThresh = 1E3
		Case Else
			dThresh = 1E6
		End Select
	End Select
	ScaleFactorY	= dYSpanData / dYSpanInp
	ScaleFactorX	= dXSpanData / dXSpanInp
	ScaleFactorXY	= ScaleFactorY / ScaleFactorX
	Dim iIntJudge	As int32 = 0
	Dim iTempY	As Int32 = dYMax
	Dim dLimY	As Double

	'前回値入力が変わったら比率計算
	If dPreInpXMax <> nudXscaleMax.Value
		If dPreInpXMax<>0
			dChgXRatio = dPreInpXMax / nudXscaleMax.Value
		Else
			dChgXRatio = 1
		End if
		'dPreInpXMax = nudXscaleMax.Value
	Else
		dChgXRatio = 1
	End If
	If dPreInpYMax <> nudYscaleMax.Value
		If dPreInpYMax<>0
			dChgYRatio = dPreInpYMax / nudYscaleMax.Value
		Else
			dChgYRatio = 1
		End if
		'dPreInpYMax = nudYscaleMax.Value
	Else
		dChgYRatio = 1
	End If

	'Y軸の最大値のピクセルを計算しチェック
	dLimY = Chart1.ChartAreas(0).AxisY.ValueToPixelPosition(nudYscaleMin.Value) - Chart1.ChartAreas(0).AxisY.ValueToPixelPosition(nudYscaleMax.Value)
	dLimY = dLimY * dChgYRatio
	dLimY = dLimY / (nudYscaleMax.Value-nudYscaleMin.Value) * dYMax
	If dLimY < Int32.MinValue OR dLimY > Int32.MaxValue
		iIntJudge = 1
	End If
	'X軸の最大値のピクセルを計算しチェック
	Dim iTempX	As Int32 = dXMax * 50
	Dim dLimX	As Double
	dLimX = Chart1.ChartAreas(0).AxisX.ValueToPixelPosition(nudXscaleMaX.Value*50) - Chart1.ChartAreas(0).AxisX.ValueToPixelPosition(nudXscaleMin.Value*50)
	dLimX = dLimX * dChgXRatio
	dLimX = dLimX / (nudXscaleMax.Value-nudXscaleMin.Value) * (dXMax*50)
	If dLimX < Int32.MinValue Or dLimX > Int32.MaxValue
		iIntJudge = 2
	End If
	
	'XY面積のピクセル数
	Dim dLimXY	As Double = dLimY * dLimX 
	If dLimXY < Int32.MinValue Or dLimXY > Int32.MaxValue
		iIntJudge = 3
	End If
	Dim dLimXYSqrt As Double = Math.Sqrt(dLimXY)

	If ScaleFactorXY > dThresh AndAlso (p_cts.iRTXAxis = 1 OrElse p_cts.iRTYAxis = 1)' Or (iIntJudge <> 0)
		nudXscaleMax.BackColor = Color.Yellow
		nudXscaleMin.BackColor = Color.Yellow
		nudYscaleMax.BackColor = Color.Yellow
		nudYscaleMin.BackColor = Color.Yellow
		sMsg = vbCrLf & Space(4) & "指定された範囲では表示できませんでした。" & vbCrLf & Space(4) & "X軸かY軸の範囲どちらかを再設定してください。" & vbCrLf
		sMsg1 = "  X軸範囲変更の場合は、範囲を  XXXX  以下にしてください。"
		sMsg2 = "  Y軸範囲変更の場合は、範囲を  XXXX  以上にしてください。"
		'推奨する安全範囲を逆算する
		dRangeLimX = dXSpanData / (ScaleFactorY / dThresh) - 0.5
		dRangeLimY = dYSpanData / (ScaleFactorX * dThresh)
		sMsg1 = sMsg1.Replace("XXXX", If( dRangeLimX<1, "----", dRangeLimX.ToString("F0")))
		sMsg2 = sMsg2.Replace("XXXX", dRangeLimY.ToString("F3"))
		lblRangeWarning.Text = sMsg & vbCrLf & sMsg1 & vbCrLf & sMsg2 & vbCrLf & " "
		lblRangeWarning.Location = New Point((Chart1.Left+Chart1.Width)/2-lblRangeWarning.Width/2, (Chart1.Top+Chart1.Height)/2)
		lblRangeWarning.Visible = True
		Return 1
	End If
#else
	If  dThreshPerLim > dThreshPer AndAlso (p_cts.iRTXAxis = 1 OrElse p_cts.iRTYAxis = 1)
		nudYscaleMax.BackColor = Color.Yellow
		nudYscaleMin.BackColor = Color.Yellow
		sMsg  = vbCrLf & Space(4) & p_sMes(efrmMainSTR.iGrpRangeTooSmall) & Space(6) 
		sMsg1 = Space(4) & p_sMes(efrmMainSTR.iGrpYAxisRangeOrder)
		'推奨する安全範囲を逆算する
		dRangeLimY = dYSpanData / 100 * dThreshPerLim
		sMsg1 = sMsg1.Replace("XXXX", dRangeLimY.ToString("F3"))
		lblRangeWarning.Text = sMsg & vbCrLf & sMsg1 & vbCrLf & " "
		lblRangeWarning.Location = New Drawing.Point((Chart1.Left+Chart1.Width)/2-lblRangeWarning.Width/2, (Chart1.Top+Chart1.Height)/2)
		lblRangeWarning.Visible = True
		Return 1
	End If
#end if
	nudXscaleMax.BackColor = Color.White
	nudXscaleMin.BackColor = Color.White
	nudYscaleMax.BackColor = Color.White
	nudYscaleMin.BackColor = Color.White
	lblRangeWarning.Visible = False
	Return 0
Catch ex As Exception
    Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
    Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Return -1
End Try
End Function

''' <summary>
''' 表示中のグラフをファイルへイメージ保存
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub tsmiSaveGraph_Click(sender As Object, e As EventArgs) Handles tsmiSaveGraph.Click
Dim iRetD As int32
Dim cifrm As ChartImageFormat
Try
	With SaveFileDialog1
		.DefaultExt = "jpg"
		.InitialDirectory = SIZUKA_PATH
		.Title		= p_sMes(efrmMainSTR.iGraphImageSave)
		.FileName	= "ChartImage.jpg"
		.Filter		= p_sMes(efrmMainSTR.iGraphImageSaveFilter)
		iRetD = .ShowDialog()
		If iRetD = DialogResult.Cancel Then
			Return
		End If
		Select Case Path.GetExtension(.FileName).ToLower
		Case ".jpg"
			cifrm = ChartImageFormat.Jpeg
		Case ".bmp"
			cifrm = ChartImageFormat.Bmp
		Case ".png"
			cifrm = ChartImageFormat.Png
		Case ".tiff"
			cifrm = ChartImageFormat.Tiff
		Case Else
			Return
		End Select
		Chart1.SaveImage(.FileName, cifrm)
	End With
Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub
''' <summary>
''' ファイルメニュー 印刷
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub tsmiPrint_Click(sender As Object, e As EventArgs) Handles tsmiPrint.Click
	If Not IsNothing(p_dtFile.iHosei)
	'測定領域に計算済データがあること
		Call WriteLog("ファイル 印刷クリック", eLOG_LEVEL.iINFO)
		p_pdocPrint = PrintDocument1
		frmLightPrint.ShowDialog()			'印刷指示画面表示
	End If
End Sub
''' <summary>
''' 測定データ印刷イベント
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub PrintDocument1_PrintPage(sender As Object, e As PrintPageEventArgs) Handles PrintDocument1.PrintPage
Dim Grp As System.Drawing.Graphics = e.Graphics
Try
	p_sMeasCondForPrint = p_sMes(efrmMainSTR.iPrintRepListItem).Split(",")			'ヘッダリスト項目の多言語
	'1ページ目
	p_iCategory = 0	
	If p_bReportCat(0)
		CurrentMeasurePrintReport(Me, sender, e, Grp)								'印刷処理
		If p_bReportCat(1) Or p_bReportCat(2) Or p_bReportCat(3)
			PrintDocument1.PrintController.OnEndPage(PrintDocument1, e)				'印刷イベント内のページ終了
			Grp = PrintDocument1.PrintController.OnStartPage(PrintDocument1, e)		'印刷イベント内の新ページ開始
		Else
			e.HasMorePages = False
			Exit Sub
		End If
	End If
	p_iCategory += 1

	'２ページ目
	If p_bReportCat(1)
		CurrentMeasurePrintReport(Me,sender, e, Grp)								'印刷処理
		If p_bReportCat(2) Or p_bReportCat(3)
			PrintDocument1.PrintController.OnEndPage(PrintDocument1, e)				'印刷イベント内のページ終了
			Grp = PrintDocument1.PrintController.OnStartPage(PrintDocument1, e)		'印刷イベント内の新ページ開始
		Else
			e.HasMorePages = False
			Exit Sub
		End If
	End If
	p_iCategory += 1

	'３ページ目
	If p_bReportCat(2)
		CurrentMeasurePrintReport(Me,sender, e, Grp)								'印刷処理
		If p_bReportCat(3)
			PrintDocument1.PrintController.OnEndPage(PrintDocument1, e)				'印刷イベント内のページ終了
			Grp = PrintDocument1.PrintController.OnStartPage(PrintDocument1, e)		'印刷イベント内の新ページ開始
		Else
			e.HasMorePages = False
			Exit Sub
		End If
	End If
	p_iCategory += 1

	'４ページ目
	If p_bReportCat(3)
		CurrentMeasurePrintReport(Me,sender, e, Grp)								'印刷処理
		e.HasMorePages = False
	End If

Catch ex As Exception
    Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
    Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub

Dim Proc As Process
Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
#If 0
	'Dim ff As New TextFileEncryption
	'Dim sPath As String = "C:\Test\ttt.txt"
	'ff.EncryptTextToFile("adasdsad", sPath)

	'Dim ss As String = ff.DecryptTextFromFile(sPath)
	'Debug.Print(ss)

	'SizukaDBへ保存
	If IsNothing(p_dtFile.p_head.sFileName)
		Exit Sub
	End If
	Proc = New Process
	Dim sPath As String = Path.Combine(SIZUKA_PATH, p_sEquipName, MESDAT_PATH, p_dtFile.p_head.sFileName.Substring(0,6), p_dtFile.p_head.sFileName)
	Proc.StartInfo.FileName= "SZKDBRegist"								
	Proc.StartInfo.Arguments = "/I" & Space(1) & "/P" & sPath
	Proc.StartInfo.UseShellExecute	= False								'OSシェルを使用するか
	Proc.StartInfo.CreateNoWindow	= True								'プロセスを新しいウィンドウで起動するか
	Proc.StartInfo.Verb				= "runas"							'管理者権限で実行

	'プロセス終了をイベント発生で知らせる
	Proc.SynchronizingObject		= Me								'自身スレッドでイベントハンドラ実行
	AddHandler Proc.Exited, New EventHandler(AddressOf proc_Exited)		'イベントハンドラ追加
	Proc.EnableRaisingEvents		= True								'プロセス終了時にイベント発生許可

	Proc.Start()
#end If
#If 0
	'プロセス終了まで待つ
	Proc.WaitForExit()
	Select Case Proc.ExitCode
	Case 0
		MsgBox("追加されました。")
	Case 1
		MsgBox("既に登録されています。")
	Case < 0
		MsgBox("登録に失敗しました。" & Proc.ExitCode.ToString)
	End Select
#end If
#If 0
	LogisticCurve(1)
#end If

#If 0
	frmBarCodeMake.Show
#end if

#If 0
'統計ヒストグラム
	Dim sw1 As New Stopwatch
	sw1.Reset
	sw1.Start
#If 0
	'正規分布サンプル作成 1000個、平均:10 標準偏差:2.0
	Values = MathNet.Numerics.Generate.Normal(100, 10, 2)

	''Values = {15.23, 16.29, 20.24, 21.36, 17.96, 17.25, 21.36, 17.96, 17.25, 21.36, 17.96, 17.25, 21.36, 17.96, 17.25, 21.36, 17.96, 17.25}
	''Values = {15.23, 16.29, 20.2, 16.4, 17.96}
#else
	Dim r As New MathNet.Numerics.Random.MersenneTwister
	Dim x As Int32
	Values = Nothing
	Array.Resize(Values, 100)
	For x=0 To Values.GetUpperBound(0)
		Values(x) = r.NextDouble() * 10 + 5
	Next 
#end if
	'ヒストグラム作成
	Dim Histogram As Histogram = Nothing
	nudSection.Value = 1.0				'初期階級幅
	HistogramGraphUpdate(Values, nudSection.Value, Histogram)
	'ヒストグラムグラフ表示
	HistogramGraph(Chart3, gbxHistgramLocation, New Drawing.Size(800, 400), Histogram, Values, gbxHistgram, nudSection, pnlHistOpe)

	lblSecWidth.Text = "Section Width"

	'統計情報ListView初期化
	lvStat.Items.Clear
	lvStat.FullRowSelect	= False
	lvStat.GridLines		= True
	lvStat.Sorting			= SortOrder.None
	lvStat.View				= View.Details
	Dim columnName = New ColumnHeader
	Dim columnData = New ColumnHeader
	columnName.Text	= "Name"
	columnData.Text	= "Value"
	Dim colHeaderRegValue() As ColumnHeader =  {columnName, columnData}
	lvStat.Columns.AddRange(colHeaderRegValue)

	'統計情報ListView 行追加
	Dim item1() As String = {"Sample size",		Values.Count.ToString("F0")}
	Dim item2() As String = {"Maximum",			Values.Maximum.ToString("F4")}
	Dim item3() As String = {"Minimum",			Values.Minimum.ToString("F4")}
	Dim item4() As String = {"Median",			Values.Median.ToString("F5")}
	Dim item5() As String = {"Average",			Values.Average.ToString("F5")}
	Dim item6() As String = {"Std. Deviation",	Values.StandardDeviation.ToString("F8")}
	Dim item7() As String = {"Variance",		Values.Variance.ToString("F8")}
	Dim item8() As String = {"+3σ",			(Values.Average+3*Values.StandardDeviation).ToString("F8")}
	Dim item9() As String = {"-3σ",			(Values.Average-3*Values.StandardDeviation).ToString("F8")}
	lvStat.Items.Add(New ListViewItem(item1))
	lvStat.Items.Add(New ListViewItem(item2))
	lvStat.Items.Add(New ListViewItem(item3))
	lvStat.Items.Add(New ListViewItem(item4))
	lvStat.Items.Add(New ListViewItem(item5))
	lvStat.Items.Add(New ListViewItem(item6))
	lvStat.Items.Add(New ListViewItem(item7))
	lvStat.Items.Add(New ListViewItem(item8))
	lvStat.Items.Add(New ListViewItem(item9))

	pnlHistOpe.Width	-= 30 
	pnlHistOpe.Left		+= 15 
	lvStat.Columns(0).Width = pnlHistOpe.Width * 0.5
	lvStat.Columns(1).Width = pnlHistOpe.Width * 0.5
	lvStat.Height = 200

    Console.WriteLine("最大値 {0}", values.Maximum())
    Console.WriteLine("最小値 {0}", values.Minimum())
    Console.WriteLine("中央値 {0}", values.Median())
    Console.WriteLine("平均値 {0}", values.Average())
    Console.WriteLine("母標準偏差 {0}", values.PopulationStandardDeviation())
    Console.WriteLine("標準偏差 {0}", values.StandardDeviation())
    Console.WriteLine("母分散 {0}", values.PopulationVariance())
    Console.WriteLine("分散 {0}", values.Variance())
    Console.WriteLine("歪度 {0}", values.Skewness())
    Console.WriteLine("尖度 {0}", values.Kurtosis())

	Dim i As int32
	For i = 0 To Histogram.BucketCount - 1
		Console.WriteLine("区分{0}:{1}", i, Histogram(i).Count)
	Next

	sw1.Stop
    Console.WriteLine("計算時間(ms) {0}", sw1.Elapsed.TotalMilliseconds)
	
#end if
#If 0
	'QRコードリーダシミュレーション開始
	StartSimulatedScan("有沢製作所,1-3-1,Measure,0,0.026,1,0,0,0,0,1,0,50,170,170,0.04,20,1,15,0,0,0.05,1,0,3,3,3600,40,,10,15,100,30000,0,10,2")
#End If


#If 1
	'標準出力を利用して非同期起動のSZKAutoResistの結果を取得する
	Dim p As New Process()
	p.StartInfo.FileName = "SZKAutoRegist.exe"
	p.StartInfo.Arguments = "1" & Space(1) & Path.Combine( p_dtFile.DATA_STORE_PATH , "GEL" , "202507", "20250702145424#000000.dat") & Space(1) & "SPC" & Space(1) & "NOREC"
	p.StartInfo.UseShellExecute = False
	p.StartInfo.RedirectStandardOutput = True
	p.StartInfo.RedirectStandardError = True
	p.StartInfo.StandardOutputEncoding = System.Text.Encoding.UTF8
	'イベントハンドラを定義
	AddHandler p.OutputDataReceived,	AddressOf OnOutputDataReceived
	AddHandler p.ErrorDataReceived,		AddressOf OnErrorDataReceived
	AddHandler p.Exited, Sub()
		Debug.WriteLine("処理完了")
	End Sub
	p.EnableRaisingEvents = True
	p.Start()
	p.BeginOutputReadLine()
	p.BeginErrorReadLine()



#end if
#If 0
	'メンテナンス情報画面表示
	frmMent.ShowDialog()
#End If

End Sub






'ヒストグラム用サンプル配列
Dim Values() As Double
''' <summary>
''' ヒストグラム作成
''' </summary>
''' <param name="values">標本配列</param>
''' <param name="dSp">区間幅</param>
''' <param name="Histogram">ヒストグラムクラス</param>
Private Sub HistogramGraphUpdate(ByRef values() As Double, ByVal dSp As Double, ByRef Histogram As Histogram)
	'ヒストグラム作成
	Dim iNBuckets As Int32 = 10
	'整数単位で区切る
''	Dim Histogram As New Histogram(Values, iNBuckets, Math.Floor(values.Minimum), Math.Ceiling(values.Maximum))
	'限の良い整数で区切る
	'Dim dSp	 As Double = nudSection.Value		'区切り幅
	Dim dLow As Double = Math.Floor(values.Min()	/ dSp) * dSp 
	Dim dUpp As Double = Math.Ceiling(values.Max()	/ dSp) * dSp 
	iNBuckets = CInt((dUpp - dLow) / dSp)
	Histogram = New Histogram(values, iNBuckets, dLow, dUpp)

End Sub
''' <summary>
''' ヒストグラム グラフ表示 
''' グループボックス上に配置したChartコントロールへ表示する
''' </summary>
''' <param name="oChart">チャートコントロール</param>
''' <param name="cLocation">表示Location</param>
''' <param name="cSize">表示サイズ  幅min500 高さmin200</param>
''' <param name="hist">MathNet.Numerics.Statistics.Histogram クラス</param>
''' <param name="oGpx">グループボックスコントロール</param>
''' <param name="oNudSec">階級幅数値入力コントロール</param>
''' <param name="oPnlOp">操作用パネルコントロール</param>
Private Sub HistogramGraph(ByRef oChart As Chart, ByVal cLocation As Drawing.Point, ByVal cSize As Drawing.Size,  _
			ByVal hist As MathNet.Numerics.Statistics.Histogram, ByRef Values() As Double, ByRef oGpx As GroupBox, ByRef oNudSec As NumericUpDown, ByRef oPnlOp As Panel)
Try
    oGpx.Visible = False
	oGpx.Text	= "Histogram"
    oChart.Series.Clear()
    oChart.ChartAreas.Clear()
    oChart.Titles.Clear()
    oChart.Legends.Clear()
    oChart.Annotations.Clear()

    ' ChartArea設定
    Dim area1 As New ChartArea("aria1")
    area1.AxisX.Title = "Value Range"
    area1.AxisY.Title = "Frequency"

    area1.AxisX.Interval = 1
	Area1.AxisX.MajorGrid.Enabled			= True
	Area1.AxisX.MajorGrid.LineDashStyle		= ChartDashStyle.Dot
    Area1.AxisX.MajorGrid.LineColor			= Color.Gray
	area1.AxisX.IsLabelAutoFit				= False
	area1.AxisX.IntervalAutoMode			= IntervalAutoMode.FixedCount
    area1.AxisX.LabelStyle.Font				= New Font(BASE_FONT, 9.0F)
    area1.AxisX.LabelStyle.IsStaggered		= True
    area1.AxisX.LabelStyle.Angle			= -45	' ラベル斜め表示

	Area1.AxisY.MajorGrid.Enabled          = True
	Area1.AxisY.MajorGrid.LineDashStyle    = ChartDashStyle.Dot
    Area1.AxisY.MajorGrid.LineColor        = Color.Gray

	With area1.Position
		.X		= 0
		.Y		= 0
		.Width	= 70
		.Height = 100
	End With	
    oChart.ChartAreas.Add(area1)

    ' X軸範囲固定
	' X軸範囲をヒストグラム + μ ±3σ まで拡張
	Dim mean	As Double = Values.Average()				' 平均値
	Dim sigma	As Double = Values.StandardDeviation()		' 標準偏差
	Dim rawMinX As Double = hist(0).LowerBound
	Dim rawMaxX As Double = hist(hist.BucketCount - 1).UpperBound
	Dim minX As Double = Math.Min(rawMinX, mean - 3 * sigma) - oNudSec.Value
	Dim maxX As Double = Math.Max(rawMaxX, mean + 3 * sigma) + oNudSec.Value
	If Double.IsNaN(minX) Or Double.IsNaN(maxX)
		area1.AxisX.Minimum = rawMinX - oNudSec.Value
		area1.AxisX.Maximum = rawMaxX + oNudSec.Value
	Else
		area1.AxisX.Minimum = minX
		area1.AxisX.Maximum = maxX
	End If

    '----------------------------------------
    ' ヒストグラム棒グラフ
    '----------------------------------------
   Dim title1 As Title = New Title("Histogram")
    Dim sHist  As New Series("Hist") With {
        .LegendText = "Histogram",
		.IsVisibleInLegend = False,
        .ChartType = SeriesChartType.Column
    }
	''sHist("PointWidth") = "1"   ' バー幅最大

	oChart.ChartAreas(0).AxisX.CustomLabels.Clear()
    ' データ追加
    For i As Integer = 0 To hist.BucketCount - 1
        Dim b = hist(i)
        Dim mid = (b.LowerBound + b.UpperBound) / 2
        Dim p As New DataPoint(mid, b.Count)
        p.Label = b.Count.ToString()
        sHist.Points.Add(p)
        '区間ラベル
        Dim cl As New CustomLabel(b.LowerBound, b.UpperBound, _
								$"{Math.Round(b.LowerBound, 1)}–{Math.Round(b.UpperBound, 1)}", _
								0, LabelMarkStyle.None)
        oChart.ChartAreas(0).AxisX.CustomLabels.Add(cl)
	Next
    oChart.Series.Add(sHist)

    '----------------------------------------
    ' Y軸最大値
    '----------------------------------------
	' ChartArea自動スケーリングを確定させる
    oChart.DataBind()
    oChart.Update()
	oChart.ChartAreas(0).RecalculateAxesScale()

   '----------------------------------------
    ' ±3σ・平均線
    '----------------------------------------
    Dim yMax	As Double = sHist.Points.Max(Function(p) p.YValues(0)) * 1.1
	Dim xMin	As Double = area1.AxisX.Minimum
	Dim xMax	As Double = area1.AxisX.Minimum
    AddVerticalLineWithLabel(oChart, mean,				yMax, xMin, xMax, Color.DarkGreen,	ChartDashStyle.Dash, "μ")
	If Double.IsNaN(sigma)
	Else
		AddVerticalLineWithLabel(oChart, mean - 3 * sigma,	yMax, xMin, yMax, Color.Red,		ChartDashStyle.Dash, "-3σ")
		AddVerticalLineWithLabel(oChart, mean + 3 * sigma,	yMax, yMax, xMin, Color.Red,		ChartDashStyle.Dash, "+3σ")
	End If

	'グラフサイズ、配置
    oGpx.Location	= cLocation
    oGpx.Size		= cSize

	oPnlOp.Location = New Drawing.Point((oGpx.Width-5) * area1.Position.Width / 100, 20)
	oPnlOp.Size		= New Drawing.Size((oGpx.Width-5) * (100-area1.Position.Width) / 100, oGpx.Height - (lvStat.height - oNudSec.Height-10))

	oChart.Visible = True
    oGpx.Visible = True

Catch ex As Exception
    Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub
''' <summary>
''' ヒストグラム 平均、±3σ線を表示
''' </summary>
''' <param name="chart"></param>
''' <param name="xVal"></param>
''' <param name="yMax"></param>
''' <param name="minX"></param>
''' <param name="maxX"></param>
''' <param name="color"></param>
''' <param name="style"></param>
''' <param name="label"></param>
Private Sub AddVerticalLineWithLabel(chart As Chart, xVal As Double, yMax As Double, minX As Double, maxX As Double, color As Color, style As ChartDashStyle, label As String)
Try
    Dim s As New Series(label)
    s.ChartType = SeriesChartType.Line
    s.Color = color
    s.BorderDashStyle = style
    s.BorderWidth = 2
    s.IsVisibleInLegend = False

    ' 縦線
    s.Points.AddXY(xVal, 0)
    s.Points.AddXY(xVal, yMax)

    ' ラベル用 DataPoint
    Dim pLabel As New DataPoint(xVal, yMax)
    pLabel.Label = label
    pLabel.LabelForeColor = color
    pLabel.LabelAngle = 0
    pLabel.IsValueShownAsLabel = True
    s.Points.Add(pLabel)

    chart.Series.Add(s)

Catch ex As Exception
    Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub
''' <summary>
''' ヒストグラム 区間設定数値入力ボックス
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub nudSection_ValueChanged(sender As Object, e As EventArgs) Handles nudSection.ValueChanged
Try
	If Not bFrmNew
		'ヒストグラム作成
		Dim Histogram As Histogram = Nothing
		HistogramGraphUpdate(Values, nudSection.Value, Histogram)
		'グラフ更新
		HistogramGraph(Chart3, New Drawing.Point(gbxHistgram.Left, gbxHistgram.Top),New Drawing.Size(gbxHistgram.Width, gbxHistgram.Height), _
			Histogram, Values, gbxHistgram, nudSection, pnlHistOpe)
		pnlHistOpe.Width	-= 30 
		pnlHistOpe.Left		+= 15 
	End If

Catch ex As Exception
    Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub
''' <summary>
''' ヒストグラム 閉じるボタン
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub bynHistClose_Click(sender As Object, e As EventArgs) Handles bynHistClose.Click
    gbxHistgram.Visible = False
End Sub
'ヒストグラムパネルマウスドラッグ移動 ------------------------------------
Dim isDraging2			As Boolean
Dim diffPoint2			As System.Drawing.Point = Nothing
Dim gbxHistgramLocation As System.Drawing.Point = New System.Drawing.Point(100, 200)
Private Sub gbxHistgram_MouseDown(sender As Object, e As MouseEventArgs) Handles gbxHistgram.MouseDown
    If e.Button <> MouseButtons.Left
        Return
    End If
    isDraging2 = True
    diffPoint2 = e.Location
	sender.Cursor = Cursors.SizeAll

End Sub
Private Sub gbxHistgram_MouseMove(sender As Object, e As MouseEventArgs) Handles gbxHistgram.MouseMove
    If Not isDraging2
        Return
    End If
    Dim x As Integer = sender.Location.X + e.X - diffPoint2.X
	Dim y As Integer = sender.Location.Y + e.Y - diffPoint2.Y
	If sender.parent.Left<=x And sender.parent.Width>=x
		If sender.parent.Top<=y And sender.parent.Height>=y
			sender.Location = New System.Drawing.Point(x, y)
		End If
	End If
End Sub
Private Sub gbxHistgram_MouseUp(sender As Object, e As MouseEventArgs) Handles gbxHistgram.MouseUp
    isDraging2 = False
	gbxHistgramLocation = gbxHistgram.Location		'位置記憶
	sender.Cursor = Cursors.Default
    If e.Button <> MouseButtons.Left
        Return
    End If
End Sub
Private Sub Test
        ' サンプルデータ
        Dim xData() As Double = {0.0, 0.5, 1.0, 1.5, 2.0}
        Dim yData() As Double = {6.0, 6.8, 8.5, 11.9, 18.5}

        ' ====== (1) シンプルな指数近似 y = a*exp(bx) ======
        Dim p = MathNet.Numerics.Fit.Exponential(xData, yData)
        Console.WriteLine($"y = {p.A} * exp({p.R} * x)")

        ' ====== (2) シフト込み y = a*exp(bx) + c ======
        ' Fit.Curve でカスタム関数を渡す
'        Dim f = Function(xx As Double, aa As Double, bb As Double, cc As Double) aa * Math.Exp(bb * xx) + cc
        'Dim coeff = MathNet.Numerics.Fit.Curve(xData, yData, f, 1.0, 1.0, 1.0) ' 初期値 a=1,b=1,c=1
        'Console.WriteLine($"y = {coeff.Item1} * exp({coeff.Item2} * x) + {coeff.Item3}")
        Dim f = Function(xx As Double, aa As Double, bb As Double) aa * xx + bb
        Dim coeff = MathNet.Numerics.Fit.Curve(xData, yData, f, 0, 0, 1.0) ' 初期値 a=0,b=0,c=1
        Console.WriteLine($"y = {coeff.Item1} * x + {coeff.Item2}")

        Console.ReadLine()
    End Sub
#If 0
    Sub Test1()
        Dim xData() As Double = {0, 1, 2, 3, 4, 5}
        Dim yData() As Double = {2.0, 3.5, 5.5, 7.2, 8.1, 9.0}

        Dim L As Double = 10.0  ' 上限値（固定）
        Dim f = Function(x As Double, k As Double, c As Double) L - (L - c) * Math.Exp(-k * x)

        ' 初期値を適当に与える（ここが重要）
        Dim coeff = MathNet.Numerics.Fit.Curve(xData, yData, f, 0.5, yData(0)) ' k=0.5, c=最初のデータ

        Console.WriteLine($"y = {L} - ({L} - {coeff.Item2}) * exp(-{coeff.Item1} * x)")
        Console.ReadLine()
    End Sub
    Sub Test2()
        Dim xData() As Double = {0, 1, 2, 3, 4, 5}
        Dim yData() As Double = {2.0, 3.5, 5.5, 7.2, 8.1, 9.0}

		Array.Resize(xData, p_dtFile.dHosei100.Length)
		Array.Resize(yData, p_dtFile.dHosei100.Length)
		Array.Copy(p_dtFile.iMSec, xData, p_dtFile.iMSec.Length)
		Array.Copy(p_dtFile.dHosei100, yData, p_dtFile.dHosei100.Length)

        Dim L As Double = 100.0  ' 上限（固定）
        Dim f = Function(x As Double, k As Double, x0 As Double, c As Double) _
                    L / (1 + Math.Exp(-k * (x - x0))) + c

        ' 初期値: k=1, x0=中央値, c=最小値
        Dim coeff = MathNet.Numerics.Fit.Curve(xData, yData, f, 1.0, 2.0, yData.Min())

        Console.WriteLine($"y = {L}/(1 + exp(-{coeff.Item1}(x-{coeff.Item2}))) + {coeff.Item3}")
        Console.ReadLine()
    End Sub
#end if
#If 0
Sub LogisticCurve(ByVal iMode As int16)
	Dim xArr() As double = Nothing
	Dim yArr() As double = Nothing
	Dim Slope() As Double = Nothing
	Select Case iMode
	Case 0
        '=== CSV 読み込み（先頭行をヘッダとしてスキップ）===
        Dim filePath As String = "C:\Temp\SampleData.csv"
        Dim lines = File.ReadAllLines(filePath)
        If lines.Length <= 1 Then
            Console.WriteLine("データがありません。")
            Return
        End If

        Dim xs As New List(Of Double)
        Dim ys As New List(Of Double)
        Dim ci = CultureInfo.InvariantCulture

        For i = 1 To lines.Length - 1 ' 0行目はヘッダ
            If String.IsNullOrWhiteSpace(lines(i)) Then Continue For
            Dim parts = lines(i).Split(","c)
            If parts.Length < 2 Then Continue For

            Dim xv As Double
            Dim yv As Double
            If Double.TryParse(parts(0).Trim().Trim("'"c, """"c), NumberStyles.Any, ci, xv) AndAlso
               Double.TryParse(parts(1).Trim().Trim("'"c, """"c), NumberStyles.Any, ci, yv) Then
                xs.Add(xv)
                ys.Add(yv)
            End If
        Next

        xArr	= xs.ToArray()
        yArr	= ys.ToArray()
	Case 1
		Array.Resize(xArr, p_dtFile.dHosei100.Length)
		Array.Resize(yArr, p_dtFile.dHosei100.Length)
		Array.Resize(slope, p_dtFile.iSlope.Length)
		Array.Copy(p_dtFile.iMSec,		0, xArr,	0, xArr.Length)
		Array.Copy(p_dtFile.dHosei100,	0, yArr,	0, yArr.Length)
		Array.Copy(p_dtFile.iSlope,		0, Slope,	0, Slope.Length)
	End Select
		'サンプルデータ調整(Y0値を除く)
		Dim iCutIdx As Int32 = 0
		Dim iLen	As Int32 = xArr.Length-iCutIdx
		Array.Copy(xArr,	iCutIdx,	xArr,	0,	iLen)
		Array.Copy(yArr,	iCutIdx,	yArr,	0,	iLen)
		Array.Copy(Slope,	iCutIdx,	Slope,	0,	iLen)
		Array.Resize(xArr,	iLen)
		Array.Resize(yArr,	iLen)
		Array.Resize(Slope,	iLen)

        If xArr.Length < 3 Then
            Console.WriteLine("有効なデータ点が不足しています。")
            Return
        End If

		'初期値を推定(L0、k0、x00)
        Dim L0	As Double = 100
        Dim k0	As Double = 0.0001
        Dim x00 As Double = 0
		Dim SlopeMax As Double = Slope.Max
		For i=0 To Slope.Length-1
			If sLopeMax <= Slope(i)
				x00 = xArr(i)				'Slope最大時のX値をx00とする
				Exit For
			End If
		Next
		FitInitialValCalc(xArr, yArr, L0, k0, x00)

        '=== ロジスティック関数 y = L / (1 + Exp(-k * (x - x0))) ===
        ' 重要：Fit.Curve のオーバーロードに合わせて
        '       Func(Of Double, Double, Double, Double, Double) 形式（x, L, k, x0 → y）で定義します。
        Dim logistic As Func(Of Double, Double, Double, Double, Double) =
             Function(x, pL, pK, pX0) pL / (1.0 + Math.Exp(-pK * (x - pX0)))
        '=== フィット ===
        ' オーバーロード：Fit.Curve(x(), y(), f(x, p1, p2, p3), p1_0, p2_0, p3_0)
		'Dim fitted = Fit.Curve(xArr, yArr, logistic, L0, k0, x00)
		Dim fitted = Fit.Curve(xArr, yArr, logistic, L0, k0, x00)
		Dim fittedL As Double	= fitted.Item1
		Dim fittedK	As Double	= fitted.Item2
		'Dim fittedX0 As Double	= fitted.Item3
		Dim fittedX0 As Double	= x00

		Console.WriteLine("ロジスティック曲線フィット結果")
		Console.WriteLine($"L = {fittedL}")
		Console.WriteLine($"k = {-fittedK}")
		Console.WriteLine($"x0 = {fittedX0}")
		p_dtFile.iSlope(0).ToString
#If 0
        ' 予測例
        Dim testX As Double = 150000
		Dim pred As Double = logistic(testX, fittedL, fittedK, fittedX0)
		Console.WriteLine()
        Console.WriteLine($"X={testX} → 予測Y={pred}")

        ' 参考：適合度（RSSとR^2）
        Dim rss As Double = 0.0
        Dim yMean As Double = yArr.Average()
        Dim tss As Double = 0.0
        For i = 0 To xArr.Length - 1
            Dim yhat = logistic(xArr(i), fittedL, fittedK, fittedX0)
            rss += (yArr(i) - yhat) * (yArr(i) - yhat)
            tss += (yArr(i) - yMean) * (yArr(i) - yMean)
        Next
        Dim r2 As Double = 1.0 - If(tss = 0.0, Double.NaN, rss / tss)
        Console.WriteLine($"RSS = {rss}")
        Console.WriteLine($"R^2 = {r2}")
#end if
End Sub
Private Sub FitInitialValCalc(ByRef xArr() As Double, ByRef yArr() As Double,  _
	ByRef dL0 As Double, ByRef dk As Double,  ByRef dx0 As Double)
Try
	' --- 初期値の自動推定（ロジスティック y = L / (1 + Exp(-k*(x - x0)))）---
	' 1) L0: 上側数%の平均を使って飽和値を推定（外れ値に強い）
	Dim xAr() As Double = xArr
	Dim yAr() As Double = yArr
	Dim n As Integer = yArr.Length
	Dim topRate As Double = 0.05 ' 上位5%
	Dim m As Integer = Math.Max(3, CInt(n * topRate))
	Dim idx = Enumerable.Range(0, n).OrderByDescending(Function(i) yAr(i)).Take(m)
	Dim L0 As Double = idx.Average(Function(i) yAr(i)) * 1.08	'8%増し
	If L0 <= 0 Then L0 = Math.Max(yArr.Max(), 1.0)

#If 0
	' 2) x0 の初期値: y ≈ L0/2 となる点の x を線形補間で推定
	Dim half As Double = L0 / 2.0
	Dim pX0 As Double = xArr(0)
	For i = 1 To n - 1
		If (yArr(i - 1) <= half AndAlso yArr(i) >= half) OrElse (yArr(i - 1) >= half AndAlso yArr(i) <= half) Then
			Dim x1 = xArr(i - 1) : Dim x2 = xArr(i)
			Dim y1 = yArr(i - 1) : Dim y2 = yArr(i)
			If y2 <> y1 Then
				Dim t As Double = (half - y1) / (y2 - y1)
				pX0 = x1 + t * (x2 - x1)
			Else
				pX0 = x1
			End If
			Exit For
		End If
	Next
#else
	Dim pX0 As Double = dx0
#end if

	' 3) k0 の初期値: x ≈ x0 周辺の傾きから推定（dy/dx at x0 = L*k/4 ⇒ k ≈ 4*slope/L）
	'    近傍ウィンドウで単回帰して傾きを取る
	Dim win As Integer = Math.Max(5, n \ 100) ' 全体の1%か最低5点
	Dim nearIdx = Enumerable.Range(0, n) _
		.OrderBy(Function(i) Math.Abs(xAr(i) - pX0)) _
		.Take(win) _
		.ToArray()

	' 最小二乗の傾き slope = Cov(x,y)/Var(x)
	Dim xm = nearIdx.Average(Function(i) xAr(i))
	Dim ym = nearIdx.Average(Function(i) yAr(i))
	Dim num As Double = 0.0
	Dim den As Double = 0.0
	For Each i In nearIdx
		Dim dx = xArr(i) - xm
		num += dx * (yArr(i) - ym)
		den += dx * dx
	Next
	Dim slope As Double = If(den = 0.0, 0.0, num / den)

	Dim pK As Double = 4.0 * slope / Math.Max(L0, 1.0)
	If pK <= 0 OrElse Double.IsNaN(pK) OrElse Double.IsInfinity(pK) Then
		' フォールバック（スケールに基づく）
		pK = 1.0 / Math.Max(xArr.Max() - xArr.Min(), 1.0)
	End If

	' --- これで初期値が pL=L0, pK, pX0 に決まりました ---
	Dim pL As Double = L0
	dL0 = pL
	dk	= pK
	'dx0	= pX0
Catch ex As Exception
    Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub
#end if


#If 0
Private Sub proc_Exited(ByVal sender As Object, ByVal e As EventArgs)
	'プロセス終了コードを確認
	Select Case Proc.ExitCode
	Case 0
		MsgBox("DB追加されました。")
	Case 1
		MsgBox("既に登録されています。")
	Case < 0
		MsgBox("DB登録に失敗しました。" & Proc.ExitCode.ToString)
	End Select
End Sub
#end if

''' <summary>
''' 管理者モード有効/無効切替 有効にするにはパスワード承認
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub lblAdminEna_MouseClick(sender As Object, e As MouseEventArgs) Handles lblAdminEna.MouseClick
Try
	If e.Button = MouseButtons.Left
		AdminModeSwitch()
	End If
Catch ex As Exception
    Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try

End Sub
Private Sub tsmiAdminiMode_Click(sender As Object, e As EventArgs) Handles tsmiAdminMode.Click
	AdminModeSwitch()
End Sub
''' <summary>
''' ロック制約スイッチ
''' </summary>
''' <param name="iMode">0:通常 1:起動時</param>
Private Sub AdminModeSwitch(Optional iMode As Int16 = 0)
Dim iRes As int16
Try
	If p_cts.AdminUse = 1
	'ロック制約する
		If Not p_cts.AdminEnable And iMode = 0
		'パスワード承認
			Dim dlg As New dlgPassword
			iRes = dlg.ShowDialog()
			Select Case iRes
			Case DialogResult.OK
			'照合OK
				p_cts.AdminEnable = Not p_cts.AdminEnable

			Case DialogResult.Cancel
			'キャンセル
				UnderStatusLabelDisp(ToolStripStatusLabel1, p_sMes(efrmMainSTR.iAdminMode3))		'パスワード違います
			End Select
			dlg.Dispose
		ElseIf iMode = 0
			p_cts.AdminEnable = Not p_cts.AdminEnable
		Else
		End If

		'イメージ変更
		If p_cts.AdminEnable
			lblAdminEna.ImageIndex = 1
			UnderStatusLabelDisp(ToolStripStatusLabel1, p_sMes(efrmMainSTR.iAdminMode1))			'ロックを解除しました
			Call WriteLog(p_sMes(efrmMainSTR.iAdminMode1), eLOG_LEVEL.iINFO)
			p_clSpec.dgvDevSetting(dgvDevSpec, False)												'SPECをデータグリッドビュー動作設定
			p_clSpec.dgvSamSetting(dgvSamSpec, False)												'SPECをデータグリッドビュー動作設定
			tsmiAdminMode.Image = ilState.Images(1)													'メニュー イメージ変更
			btnDevSpecEdit.Enabled		= True														'編集ボタン
			btnSamSpecEdit.Enabled		= True														'編集ボタン
			tsmiCaliblation.Enabled		= True														'キャリブレーションメニュー
			tsmiHorizontalConfirmation.Enabled = True												'水平確認メニュー
			tsmiQrCodeCreate.Enabled	= p_cts.QRModeEnable										'QRコード作成メニュー

		Else
			lblAdminEna.ImageIndex = 0
			UnderStatusLabelDisp(ToolStripStatusLabel1, p_sMes(efrmMainSTR.iAdminMode2))			'ロックを設定しました
			Call WriteLog(p_sMes(efrmMainSTR.iAdminMode2), eLOG_LEVEL.iINFO)
			p_clSpec.dgvDevSetting(dgvDevSpec,True)													'SPECをデータグリッドビュー動作設定
			p_clSpec.dgvSamSetting(dgvSamSpec, True)												'SPECをデータグリッドビュー動作設定
			tsmiAdminMode.Image = ilState.Images(0)													'メニュー イメージ変更
			btnDevSpecEdit.Enabled		= False														'編集ボタン
			btnSamSpecEdit.Enabled		= False														'編集ボタン

			tsmiCaliblation.Enabled		= False														'キャリブレーションメニュー
			tsmiHorizontalConfirmation.Enabled = False												'水平確認メニュー
			tsmiQrCodeCreate.Enabled	= False														'QRコード作成メニュー
		End If
	Else
	'ロック制約しない
		tsmiAdminMode.Enabled	= False
		lblAdminEna.Enabled		= False
	End If
Catch ex As Exception
    Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub

Private Sub tsmiShowHelp_Click(sender As Object, e As EventArgs) Handles tsmiShowHelp.Click
	Dim psi As New ProcessStartInfo("SizukaManual.pdf")
	psi.UseShellExecute = True
	Process.Start(psi)
End Sub
Private Sub tsmiMent_Click(sender As Object, e As EventArgs) Handles tsmiMent.Click
	'メンテナンス情報画面表示
	frmMent.ShowDialog()
End Sub



''' <summary>
''' QRコード作成
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub tsmiQrCodeCreate_Click(sender As Object, e As EventArgs) Handles tsmiQrCodeCreate.Click
Try
	If Not p_cts.QRModeEnable
		Exit Sub
	End If
'QRコード生成
	Dim iRet As Int32
	'現在の設定(サンプルSPEC,デバイスSPEC,その他詳細設定一部をカンマ区切りでテキスト化)を作成する
	Dim sText As String = Nothing
	QRCodeTextMake(sText)
	Dim clQR	As New clQRCodeControl(p_cts.QREcc, p_cts.QRLogoCells, PATH.Combine(My.Application.Info.DirectoryPath, "QRCodeLogo.png"), p_cts.QRPixelInCel)
	clQR.p_qrImagePath = Path.Combine( SIZUKA_PATH, QRCODE_PATH, p_clSpec.spi.sSampleName & ".png" )
	clQR.p_qrText = sText

	iRet = clQR.GenerateQR()			'QRコード生成
	Select Case iRet
	Case 0
		QRSaveImageAsNamed(clQR)		'名前を付けて保存
	Case Else
	End Select
Catch ex As Exception
    Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub
''' <summary>
''' QRコードイメージの名前を付けて保存
''' </summary>
''' <returns></returns>
Private Function QRSaveImageAsNamed(ByRef clQR As clQRCodeControl) As Int32
Try
    Using sfd As New SaveFileDialog()
        sfd.Title = p_sMes(efrmMainSTR.iQRCodeSaveTitle)
        sfd.Filter = p_sMes(efrmMainSTR.iQRCodeSaveFileKind)
        sfd.FileName = Path.GetFileName(clQR.p_qrImagePath)
		sfd.InitialDirectory = Path.GetDirectoryName(clQR.p_qrImagePath)
        If sfd.ShowDialog() = DialogResult.OK Then
            clQR.p_picQR.Image.Save(sfd.FileName, Imaging.ImageFormat.Png)
            MessageBox.Show(p_sMes(efrmMainSTR.iQRCodeSaveFinished) & sfd.FileName, My.Application.Info.ProductName)
        End If
    End Using
	Return 0
Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Return -1
End Try
End Function

''' <summary>
''' QRコード内データを作成
''' </summary>
''' <param name="sText"></param>
Private Sub QRCodeTextMake(ByRef sText As String)
Try
	'サンプルSPEC
	sText &= p_clSpec.spi.sSampleName & ","					'0
	sText &= p_clSpec.spi.sLot & ","
	sText &= p_clSpec.spi.sMeasurer & ","
	sText &= p_clSpec.spi.iShape & ","
	sText &= p_clSpec.spi.sThickness & ","
	sText &= p_clSpec.spi.dStack & ","
	sText &= p_clSpec.spi.dEffectRangeMax & ","
	sText &= p_clSpec.spi.dEffectRangeMin & ","
	sText &= p_clSpec.spi.dJudgeRangeMax & ","
	sText &= p_clSpec.spi.dJudgeRangeMin & ","				'9

	'デバイスSPEC
	sText &= p_clSpec.spi.iHPPosCont & ","					'10
	sText &= p_clSpec.spi.iGelTimeMethod & ","
	sText &= p_clSpec.spi.dThreshold & ","
	sText &= p_clSpec.spi.dHPSetTemp & ","
	sText &= p_clSpec.spi.dPBSetTemp & ","
	sText &= p_clSpec.spi.dMeasureGap & ","
	sText &= p_clSpec.spi.dMeasureStress & ","
	sText &= p_clSpec.spi.iPressContStep & ","
	sText &= p_clSpec.spi.dProbDiameter & ","
	sText &= p_clSpec.spi.iStressSensMode & ","
	sText &= p_clSpec.spi.iAmpMode & ","					'20
	Select Case p_clSpec.spi.iStressSensMode
	Case eStrsSens.Line, eStrsSens.Circle
		sText &= p_clSpec.spi.dXAmplitude & ","				'21
		sText &= p_clSpec.spi.dXPeriod & ","
	Case eStrsSens.Point
		sText &= p_clSpec.spi.dZAmplitude & ","				'21
		sText &= p_clSpec.spi.dZPeriod & ","
	End Select
	sText &= p_clSpec.spi.dAmpModeStress & ","				'23
	sText &= p_clSpec.spi.dInputToHPUpWait & ","
	sText &= p_clSpec.spi.dHPUpToXYActWait & ","
	sText &= p_clSpec.spi.dMeasureTime & ","
	sText &= p_clSpec.spi.dMeasureEndStress & ","
	sText &= p_clSpec.spi.sTempProfileName & ","
	sText &= p_clSpec.spi.dDiffInterval & ","

	'投入GAP
	sText &= p_cts.InputGap & ","							'30
	'着圧制御ステップ遅延時間
	sText &= p_cts.PressContDelayTime & ","					'31
	'Z荷重とたわみの比例定数
	sText &= p_cts.GapCorr & ","							'32
	'間欠モード
	sText &= If(p_cts.LTModeEnable, 1, 0) & ","				'33
	sText &= p_cts.LTMeasureInterval & ","					'34
	sText &= p_cts.LTAdoptRate								'35

Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub
''' <summary>
''' QRコード内データを展開しDGVへセットする
''' </summary>
''' <param name="sText"></param>
Private Sub QRCodeTextExpand(ByRef sText As String, ByRef dgvSam As DataGridView, ByRef dgvDev As DataGridView)
	Dim sBuf() As String
Try

	sBuf = sText.Split(",")
	'サンプルSPEC
	dgvSam.Item(2, eSamSPEC.SAMPLE).Value		= sBuf(0)
	dgvSam.Item(2, eSamSPEC.LOT).Value			= sBuf(1)
	dgvSam.Item(2, eSamSPEC.MEASURER).Value		= sBuf(2)
	dgvSam.Item(2, eSamSPEC.SHAPE).Value		= p_clSpec.sShape(Int16.Parse(sBuf(3)))
	dgvSam.Item(2, eSamSPEC.THICKNESS).Value	= sBuf(4)
	dgvSam.Item(2, eSamSPEC.STACKS).Value		= sBuf(5)
	dgvSam.Item(2, eSamSPEC.EFECT_RANGE).Value	= sBuf(7) & " - " & sBuf(6)
	dgvSam.Item(2, eSamSPEC.JUDGE_RANGE).Value	= sBuf(9) & " - " & sBuf(8)
	'デバイスSPEC
	dgvDev.Item(2, eDevSPEC.HP_CONT).Value			= p_clSpec.sHCont(Int16.Parse(sBuf(10)))
	dgvDev.Item(2, eDevSPEC.METHOD).Value			= lstCalcMethod(Int16.Parse(sBuf(11)))
	dgvDev.Item(2, eDevSPEC.THRETHOLD).Value		= Double.Parse(sBuf(12)).ToString("F1")
	dgvDev.Item(2, eDevSPEC.TEMPERATURE).Value		= Double.Parse(sBuf(13)).ToString("F1") & "/" & Double.Parse(sBuf(14)).ToString("F1")
	dgvDev.Item(2, eDevSPEC.MEAS_GAP).Value			= Double.Parse(sBuf(15)).ToString("F3")
	dgvDev.Item(2, eDevSPEC.MEAS_STRESS).Value		= Double.Parse(sBuf(16)).ToString("F1")
	dgvDev.Item(2, eDevSPEC.PRESSCONT_STEP).Value	= sBuf(17)
	dgvDev.Item(2, eDevSPEC.PB_DIAMETER).Value		= Double.Parse(sBuf(18)).ToString("F1")
	dgvDev.Item(2, eDevSPEC.STRS_SENS_MODE).Value	= p_clSpec.sStrsSens(Int16.Parse(sBuf(19)))
	dgvDev.Item(2, eDevSPEC.AMP_MODE).Value			= p_clSpec.sAmpMode(Int16.Parse(sBuf(20)))
	dgvDev.Item(2, eDevSPEC.AMPLITUDE).Value		= Double.Parse(sBuf(21)).ToString("F3")
	dgvDev.Item(2, eDevSPEC.PERIOD).Value			= Double.Parse(sBuf(22)).ToString("F1")
	dgvDev.Item(2, eDevSPEC.AMPMODE_STRESS).Value	= Double.Parse(sBuf(23)).ToString("F1")
	dgvDev.Item(2, eDevSPEC.HP_UP_WAIT).Value		= Double.Parse(sBuf(24)).ToString("F1")
	dgvDev.Item(2, eDevSPEC.ACTION_WAIT).Value		= Double.Parse(sBuf(25)).ToString("F1")
	dgvDev.Item(2, eDevSPEC.MEAS_TIME).Value		= Double.Parse(sBuf(26)).ToString("F1")
	dgvDev.Item(2, eDevSPEC.MEAS_END_STRESS).Value	= Double.Parse(sBuf(27)).ToString("F1")
	dgvDev.Item(2, eDevSPEC.TEMP_PROFILE).Value		= sBuf(28)
	dgvDev.Item(2, eDevSPEC.DIFF_INTERVAL).Value	= Double.Parse(sBuf(29)).ToString("F3")

	p_clSpec.spi.sSampleName		= sBuf(0)
	p_clSpec.spi.sLot				= sBuf(1)
	p_clSpec.spi.sMeasurer			= sBuf(2)
	p_clSpec.spi.iShape				= sBuf(3)
	p_clSpec.spi.sThickness			= sBuf(4)
	p_clSpec.spi.dStack				= sBuf(5)
	p_clSpec.spi.dEffectRangeMax	= sBuf(6)
	p_clSpec.spi.dEffectRangeMin	= sBuf(7)
	p_clSpec.spi.dJudgeRangeMax		= sBuf(8)
	p_clSpec.spi.dJudgeRangeMin		= sBuf(9)

	'デバイスSPEC
	p_clSpec.spi.iHPPosCont			= sBuf(10)
	p_clSpec.spi.iGelTimeMethod		= sBuf(11)
	p_clSpec.spi.dThreshold			= sBuf(12)
	p_clSpec.spi.dHPSetTemp			= sBuf(13)
	p_clSpec.spi.dPBSetTemp			= sBuf(14)
	p_clSpec.spi.dMeasureGap		= sBuf(15)
	p_clSpec.spi.dMeasureStress		= sBuf(16)
	p_clSpec.spi.iPressContStep		= sBuf(17)
	p_clSpec.spi.dProbDiameter		= sBuf(18)
	p_clSpec.spi.iStressSensMode	= sBuf(19)
	p_clSpec.spi.iAmpMode			= sBuf(20)
	Select Case p_clSpec.spi.iStressSensMode
	Case eStrsSens.Line, eStrsSens.Circle
		p_clSpec.spi.dXAmplitude	= sBuf(21)
		p_clSpec.spi.dXPeriod		= sBuf(22)
		p_clSpec.spi.dYAmplitude	= sBuf(21)
		p_clSpec.spi.dYPeriod		= sBuf(22)
		p_clSpec.spi.dZAmplitude	= 0
		p_clSpec.spi.dZPeriod		= 1
	Case eStrsSens.Point
		p_clSpec.spi.dXAmplitude	= 0
		p_clSpec.spi.dXPeriod		= 1
		p_clSpec.spi.dYAmplitude	= 0
		p_clSpec.spi.dYPeriod		= 1
		p_clSpec.spi.dZAmplitude	= sBuf(21)
		p_clSpec.spi.dZPeriod		= sBuf(22)
	End Select
	p_clSpec.spi.dAmpModeStress		= sBuf(23)
	p_clSpec.spi.dInputToHPUpWait	= sBuf(24)
	p_clSpec.spi.dHPUpToXYActWait	= sBuf(25)
	p_clSpec.spi.dMeasureTime		= sBuf(26)
	p_clSpec.spi.dMeasureEndStress	= sBuf(27)
	p_clSpec.spi.sTempProfileName	= sBuf(28)
	p_clSpec.spi.dDiffInterval		= sBuf(29)

	'投入GAP
	p_cts.InputGap					= sBuf(30)
	'着圧制御ステップ遅延時間
	p_cts.PressContDelayTime		= sBuf(31)
	'Z荷重とたわみの比例定数
	p_cts.GapCorr					= sBuf(32)
	'間欠モード
	p_cts.LTModeEnable				= If(sBuf(33)="1", True, False)
	p_cts.LTMeasureInterval			= sBuf(34)
	p_cts.LTAdoptRate				= sBuf(35)


Catch ex As Exception
    Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub
''' <summary>
''' QR code ボタン クリック
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub btnQRcode_Click(sender As Object, e As EventArgs) Handles btnQRcode.Click
	If p_RTM(0).p_MeasureState = RealTimeMeas.eMeasStat.NOT_START Then
	'測定中以外
		Select Case p_iQRState
		Case clQRCodeControl.eQRState.Preparing
			'準備中
			p_iQRState = clQRCodeControl.eQRState.Receipting			'受付中へ
			Me.ImeMode = ImeMode.Disable								'IME無効
			txtQR.Clear
			txtQR.Focus()
			txtQR.Visible = False
		Case clQRCodeControl.eQRState.Receipting
			'受付中
			p_iQRState = clQRCodeControl.eQRState.Preparing				'準備中へ
		Case clQRCodeControl.eQRState.Complete
			'受付完了
			p_iQRState = clQRCodeControl.eQRState.Receipting			'受付中へ
			Me.ImeMode = ImeMode.Disable								'IME無効
			txtQR.Clear
			txtQR.Focus()
			txtQR.Visible = False
		End Select
	End If
End Sub
''' <summary>
''' フォーム キープレスイベント
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub frmMain_KeyPress(sender As Object, e As KeyPressEventArgs) Handles Me.KeyPress
Try
#If 0
    If Asc(e.KeyChar) = Keys.Enter Then
        e.Handled = True
    End If
    txtQR.Text &= e.KeyChar
#else
	If p_RTM(0).p_MeasureState = RealTimeMeas.eMeasStat.NOT_START Then
	'測定中以外
		'QRコード文字蓄積
		If p_cts.QRModeEnable AndAlso p_iQRState = clQRCodeControl.eQRState.Receipting
		'QRコードモード有効かつ受付中
			If e.KeyChar = ChrW(13) Then      ' Enter = 読取終了
				Dim qr As String = txtQR.ToString()
				' QR読み取り文字列
				'Debug.Print("QR = " & qr)
				Console.WriteLine("QRCode Read=" & qr)
			Else
				txtQR.AppendText(e.KeyChar)
			End if
		End if
	End if

#end If
Catch ex As Exception
    Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub
''' <summary>
''' フォーム Deactivateイベント
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub frmMain_Deactivate(sender As Object, e As EventArgs) Handles Me.Deactivate
	'QRコード 受付解除
	If p_cts.QRModeEnable AndAlso p_iQRState = clQRCodeControl.eQRState.Receipting
		p_iQRState = clQRCodeControl.eQRState.Preparing			'準備中へ
	End If
End Sub


'QRコードシミュレーション  -------------------------
Private simulatedData As String
Private pos As Integer = 0
''' <summary>
''' QRコードリーダ シミュレーション
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub QRSimuTimer_Tick(sender As Object, e As EventArgs) Handles QRSimuTimer.Tick
	If p_cts.QRModeEnable AndAlso p_iQRState = clQRCodeControl.eQRState.Receipting
		QRSimuTimer.Enabled = False
		If pos < simulatedData.Length Then
			' KeyPressEventArgs を手動で生成して直接コール
			Dim ch As Char = simulatedData(pos)
			Dim ev As New KeyPressEventArgs(ch)
			frmMain_KeyPress(Me, ev)
			pos += 1
		Else
			QRSimuTimer.Stop()
			Exit sub
		End If
		QRSimuTimer.Enabled = True
	Else
		QRSimuTimer.Stop()
	End If
End Sub
'QRコード送出開始
Private Sub StartSimulatedScan(text As String)
    simulatedData = text & vbCrLf   ' ← 最後にEnter相当（CR/LF）を付けておく
    pos = 0
	Me.ImeMode = ImeMode.Disable
    QRSimuTimer.Interval = 1
    QRSimuTimer.Start()
End Sub
'-------------------------------------------------

'QRコードイメージファイルドラッグドロップ -----
Private Sub frmMain_DragEnter(sender As Object, e As Forms.DragEventArgs) Handles Me.DragEnter
	If Not m_bQRCodeDragDropEna
		Exit Sub
	End If
    ' ドラッグされたデータがファイルかチェック
    If e.Data.GetDataPresent(DataFormats.FileDrop)
        Dim files() As String = CType(e.Data.GetData(DataFormats.FileDrop), String())

        ' ファイルが1つ以上ある場合
        If files.Length > 0 Then
            ' 最初のファイルの拡張子を取得
            Dim ext As String = IO.Path.GetExtension(files(0)).ToLower()

            ' 許可する拡張子を指定
            Dim allowedExts As String() = {".png", ".jpg", ".jpeg", ".bmp", ".gif"}

            If allowedExts.Contains(ext) Then
                e.Effect = DragDropEffects.Copy   ' ドロップ可能
            Else
                e.Effect = DragDropEffects.None   ' ドロップ不可
            End If
        End If
    Else
        e.Effect = DragDropEffects.None
    End If

End Sub
Private Sub frmMain_DragDrop(sender As Object, e As Forms.DragEventArgs) Handles Me.DragDrop
    Dim files() As String = CType(e.Data.GetData(DataFormats.FileDrop), String())
	Dim uQr		As New clQRCodeControl(p_cts.QREcc, p_cts.QRLogoCells, PATH.Combine(My.Application.Info.DirectoryPath, "QRCodeLogo.png"), p_cts.QRPixelInCel)
	Dim sText	As String = Nothing
	Dim iRet	As Int32
	If p_RTM(0).p_MeasureState = RealTimeMeas.eMeasStat.NOT_START Then
	'測定中以外
		If files IsNot Nothing AndAlso files.Length > 0 Then
			Dim path As String = files(0)
			Try
				' 画像を読み込んで PictureBox に設定
				uQr.p_picQR.Image = Image.FromFile(path)
				iRet = uQr.Decord(sText)
				Select Case iRet
				Case 0
					Call WriteLog("QRコード ドラッグドロップ読込み成功",eLOG_LEVEL.iINFO)
					p_iQRState = clQRCodeControl.eQRState.Receipting
					txtQR.Text = sText
				Case Else
					Call WriteLog("QRコード ドラッグドロップ読込み失敗",eLOG_LEVEL.iINFO)
					MessageBox.Show("デコード失敗しました。")
				End Select
			Catch ex As Exception
				MessageBox.Show("画像を読み込めませんでした。" & vbCrLf & ex.Message)
			End Try
		End If
	End If
End Sub

'=== SPC機能 =========================================================================================================================================
Dim iSPCJob As Int32 = 0									'SPC画面表示ジョブ変数
Public p_bSpcRangeAll	As Boolean = True					'SPC 統計範囲 全/指定 記憶
Public p_iSpcDispMin	As Int32  = 1							'SPC 表示範囲開始 記憶
Public p_iSpcDispMax	As Int32  = 100							'SPC 表示範囲終了 記憶
Public p_sSpcRangeMin	As String = "2020/01/01 00:00"		'SPC機能 統計範囲(初期値 全範囲)
Public p_sSpcRangeMax	As String = "2200/12/31 00:00"		'SPC機能 統計範囲(初期値 全範囲)
Public p_frmSPC			As frmSPC = Nothing					'開いているSPCフォーム参照 記憶
Public p_frmSPCLocation As New Point						'SPCフォーム位置 記憶
Public dSPCValues()		As Double							'SPC用ゲルタイム配列

'------------------
''' <summary>
''' SPCボタン クリック
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub btnSPC_Click(sender As Object, e As EventArgs) Handles btnSPC.Click
	btnSPCClick()
End Sub

Public sub btnSPCClick()
	Select Case p_cts.DBSel
	Case 0
		btnSPCClick_SQLServer()		'SQLServer版
	Case 1
		btnSPCClick_SQLite()		'SQLite版
	End Select
End sub

''' <summary>
''' SPCボタン クリック シーケンス処理
''' SQLite版
''' <param name="iMode">0:通常クリック時 1:dSPCValuesのみ取得</param>
''' </summary>
Public Sub btnSPCClick_SQLite(ByVal Optional iMode As Int32 = 0)
	Dim uHead	As DataFileHeader
	Dim ss		As String = ""
	Dim sw		As New Stopwatch
	Dim iRet	As Int32
Try
	iSPCJob = 1
	Do
		Select Case iSPCJob
		Case 1
		'現在の条件でSPC情報取得指令を出す
			If p_cts.LTModeEnable
				uHead.sMeasureType		= "LTM"
			Else
				uHead.sMeasureType		= "GEL"
			End If
			uHead.sEquipName		= p_sEquipName
			uHead.sSampleName		= p_clSpec.spi.sSampleName
			uHead.sLot				= p_clSpec.spi.sLot
			uHead.sCalcMethod		= lstCalcMethod(p_clSpec.spi.iGelTimeMethod)
			uHead.sThreshold		= p_clSpec.spi.dThreshold.ToString
			'測定種別、装置ID、サンプル名、ロット名、計算方法、閾値
			ss = uHead.sMeasureType & "," & uHead.sEquipName & "," & uHead.sSampleName & "," & uHead.sLot & "," & uHead.sCalcMethod & "," & uHead.sThreshold

			'現在のSPC情報を取得する
			Dim uPara	As clSizukaDB_SQLite.stStaticsParameter = Nothing
			'SPC選択
			uPara.sMeasType			= uHead.sMeasureType
			uPara.sEquipID			= uHead.sEquipName
			uPara.sSampleName		= uHead.sSampleName
			uPara.sLot				= uHead.sLot
			uPara.sCalculationMethod= uHead.sCalcMethod
			uPara.sStressThreshold	= uHead.sThreshold
			uPara.sStatMin			= p_sSpcRangeMin
			uPara.sStatMax			= p_sSpcRangeMax
			iRet = SpcSelectProc(uPara, dSPCValues)
			If iRet=0
				If iMode =1
				'dSPCValuesのみ取得する
					iSPCJob = 0
					Exit Do
				Else
					iSPCJob = 20
				End If
			Else
				iSPCJob = 10
			End If

		Case 10
		'エラー終了
			iSPCJob = 0
			dSPCValues = Nothing
			Exit Do

		Case 20
		'OK
			If Not IsNothing(dSPCValues)
			'サンプル配列有り
				'SPCフォーム開いているか確認する
				For Each f As Form In Forms.Application.OpenForms
					If TypeOf f Is frmSPC Then
					'既に開いていたら表示して終了
						f.WindowState = FormWindowState.Normal
						f.Activate()
						Return
					End If
				Next
				'開いていない場合は新規表示して終了
				Dim frm As New frmSPC(Me, dSPCValues, p_sSpcRangeMin, p_sSpcRangeMax)
				p_frmSPC = frm		'表示したSPCフォーム参照を記憶
				frm.Show
			End if
			iSPCJob = 0
			Exit Do
		End Select
	Loop
Catch ex As Exception
    Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub
''' <summary>
''' SPC用データサンプル（ゲルタイム）を取得
''' </summary>
''' <param name="uPara">抽出条件パラメータ</param>
''' <param name="dVal">抽出条件パラメータ</param>
''' <returns></returns>
Private Function SpcSelectProc(ByVal uPara As clSizukaDB_SQLite.stStaticsParameter, ByRef dVal() As Double) As Int32
	Dim iRet As Int32 = 0
Try
	'追加した測定と同条件のデータを選択する
	'装置ID、サンプル名、ロット、件数、(ゲル時間...)件数分
	Dim sSql	As String = p_clDB.p_SpcSqlBase_SQLite
	Dim dVals()	As Double = Nothing
	Dim iGelCnt	As Int32 = 0
	p_clDB.StaticsSqlEdit(sSql, uPara)				'SQL文を編集
	iRet = p_clDB.fnSpcSelectExecute(sSql, dVals)	'SQL文を実行
	If iRet=0
		If Not IsNothing(dVals)
			If dVals.Length>=1
				Array.Resize(dVal, 0)
				For i=0 To dVals.GetUpperBound(0)
					If dVals(i) > 0
						'無効ゲルタイムはスキップ
						Array.Resize(dVal, dVal.Length+1)
						dVal(iGelCnt) = dVals(i)
						iGelCnt += 1
					End If
				Next
				iRet = 0
				WriteLog("SPC Select Count:" & iGelCnt.ToString, eLOG_LEVEL.iINFO)
			Else
				iRet = -2
				WriteLog("SPC Select None.", eLOG_LEVEL.iINFO)
			End if
		Else
			iRet = -2
			WriteLog("SPC Select None.", eLOG_LEVEL.iINFO)
		End if
	Else
		iRet = -2
		WriteLog("SPC Select Failure.", eLOG_LEVEL.iINFO)
	End If
	Return iRet
Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Return -1
End Try
End Function
''' <summary>
''' SPCボタン クリック シーケンス処理
''' SQLServer版
''' </summary>
Public Sub btnSPCClick_SQLServer()
	Dim uHead	As DataFileHeader
	Dim ss		As String = ""
	Dim sw		As New Stopwatch
Try
	iSPCJob = 1
	Do
		Select Case iSPCJob
		Case 1
		'現在の条件でSPC情報取得指令を出す
			If p_cts.LTModeEnable
				uHead.sMeasureType		= "LTM"
			Else
				uHead.sMeasureType		= "GEL"
			End If
			uHead.sEquipName		= p_sEquipName
			uHead.sSampleName		= p_clSpec.spi.sSampleName
			uHead.sLot				= p_clSpec.spi.sLot
			uHead.sCalcMethod		= lstCalcMethod(p_clSpec.spi.iGelTimeMethod)
			uHead.sThreshold		= p_clSpec.spi.dThreshold.ToString
			'測定種別、装置ID、サンプル名、ロット名、計算方法、閾値
			ss = uHead.sMeasureType & "," & uHead.sEquipName & "," & uHead.sSampleName & "," & uHead.sLot & "," & uHead.sCalcMethod & "," & uHead.sThreshold

			'現在のSPC情報を取得する
			AutoRejistExec(True, uHead.sMeasureType, ss, False)
			iSPCJob = 10
			sw.Reset()
			sw.Start()

		Case 10
		'受信待ち
			If sw.Elapsed.TotalMilliseconds > 1000
			'１秒越えたらあきらめる
				iSPCJob = 0
				dSPCValues = Nothing
				Exit Do
			End If

		Case 20
		'受信OK
			If Not IsNothing(dSPCValues)
			'サンプル配列有り
				'SPCフォーム開いているか確認する
				For Each f As Form In Forms.Application.OpenForms
					If TypeOf f Is frmSPC Then
					'既に開いていたら表示して終了
						f.WindowState = FormWindowState.Normal
						f.Activate()
						Return
					End If
				Next
				'開いていない場合は新規表示して終了
				Dim frm As New frmSPC(Me, dSPCValues, p_sSpcRangeMin, p_sSpcRangeMax)
				p_frmSPC = frm		'表示したSPCフォーム参照を記憶
				frm.Show
			End if
			iSPCJob = 0
			Exit Do
		End Select
	Loop
Catch ex As Exception
    Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub
''' <summary>
''' SZKAutoRegistを起動
''' </summary>
''' <param name="bAsync">False:SPCなし True:SPCあり('標準出力を利用して非同期起動のSZKAutoResistの結果を取得する)</param>
''' <param name="sMeasType">測定種類 3桁の識別文字</param>
''' <param name="sFileName">測定ファイル名</param>
''' <returns></returns>,
Public Function AutoRejistExec(ByVal bAsync As Boolean, ByVal sMeasType As String, ByVal sFileName As String, Optional ByVal bREC As Boolean = True) As Int32
Try
	Dim p As New Process()
	p.StartInfo.FileName = "SZKAutoRegist.exe"
	Dim sFileNum As String = "0"
	If Not sFileName.Contains(",")
		sFileNum = (p_dtFile.MesFileNumber+1).ToString
		Select Case sMeasType
		Case p_dtFile.sMeasindCode(eMesCD.GEL)
			p.StartInfo.Arguments = sFileNum & Space(1) & Path.Combine( p_dtFile.GetDataFullPath(p_dtFile.eMesCD.GEL), sFileName )
		Case p_dtFile.sMeasindCode(eMesCD.GLA)
			p.StartInfo.Arguments = sFileNum & Space(1) & Path.Combine( p_dtFile.GetDataFullPath(p_dtFile.eMesCD.GLA), sFileName )
		Case p_dtFile.sMeasindCode(eMesCD.LTM)
			p.StartInfo.Arguments = sFileNum & Space(1) & Path.Combine( p_dtFile.GetDataFullPath(p_dtFile.eMesCD.LTM), sFileName )
		Case else
			p.StartInfo.Arguments = sFileNum & Space(1) & Path.Combine( p_dtFile.GetDataFullPath(p_dtFile.eMesCD.GEL), sFileName )
		End Select
	Else
		p.StartInfo.Arguments = sFileNum & Space(1) & sFileName
	End If
	'引数用にスペースを"#"へ置換
	Dim sSpcRangeMin As String = Replace(p_sSpcRangeMin, Space(1), "#")
	Dim sSpcRangeMax As String = Replace(p_sSpcRangeMax, Space(1), "#")
	If bAsync
	'SPC機能 有効
		p.StartInfo.Arguments = p.StartInfo.Arguments & Space(1) & "SPC" & Space(1) & If( bREC=True, "REC", "NOREC") & Space(1) & sSpcRangeMin & Space(1) & sSpcRangeMax
		p.StartInfo.UseShellExecute			= False
		p.StartInfo.RedirectStandardOutput	= True
		p.StartInfo.RedirectStandardError	= True
		p.StartInfo.StandardOutputEncoding	= System.Text.Encoding.UTF8
		'イベントハンドラを定義		
		AddHandler p.OutputDataReceived,	AddressOf OnOutputDataReceived			'標準出力 Outputイベント発生
		AddHandler p.ErrorDataReceived,		AddressOf OnErrorDataReceived			'標準出力 エラーイベント発生
		AddHandler p.Exited, Sub()
			Debug.WriteLine(p.StartInfo.FileName & " 処理完了")
		End Sub
		p.EnableRaisingEvents = True
		p.Start()
		p.BeginOutputReadLine()
		p.BeginErrorReadLine()
	Else
	'SPC機能 無効
		p.StartInfo.Arguments = p.StartInfo.Arguments & Space(1) & "NOSPC" & Space(1) & If( bREC=True, "REC", "NOREC") & Space(1) & sSpcRangeMin & Space(1) & sSpcRangeMax
		p.Start()
	End If
	Return 0
Catch ex As Exception
    Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Return -1
End Try
End Function

''' <summary>
''' 標準出力 Outputイベント発生
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub OnOutputDataReceived(sender As Object, e As DataReceivedEventArgs)
	Dim sData	As String = Nothing
	Dim sGel()	As String = Nothing
Try
    If e.Data Is Nothing 
		Return
	End If
	'Debug.Print(e.Data)
	If e.Data.Contains("@SZKDBResist@")
	'SPC用データである
		dSPCValues	= Nothing
		sData	= e.Data
		sData	= sData.Replace("@SZKDBResist@", "")
		sGel	= sData.Split(",")
		If Not IsNothing(sGel)
			Array.Resize(dSPCValues, sGel.Length)
			For i=0 To sGel.GetUpperBound(0)
				dSPCValues(i) = Double.Parse(sGel(i))
			Next
		End If
	Else
	    Call WriteLog("SPC DB Select:" & e.Data, eLOG_LEVEL.iFATAL)
	End If

	'受信完了 SPCボタンシーケンス進行
	If iSPCJob = 10		
		iSPCJob = 20
	End If
Catch ex As Exception
    Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub
''' <summary>
''' 標準出力 エラーイベント発生
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub OnErrorDataReceived(sender As Object, e As DataReceivedEventArgs)
Try
    If e.Data Is Nothing 
		Return
	End If
Catch ex As Exception
    Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub



	'------------------------------------------------------------------------
End Class