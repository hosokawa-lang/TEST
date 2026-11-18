Imports System.Runtime.InteropServices
Imports System.IO
Imports System.Text.RegularExpressions
Imports System.Windows.Forms.DataVisualization.Charting
Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.FileIO
Imports SZKTool.frmOptions
Imports SZKTool.DataCal

Public Module modPublic
'▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼
'   定数
'▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲
'固定パス
Public const SIZUKA_PATH        As String   = "C:\SZKTool"
Public const MESDAT_PATH        As String   = "Measdata"
Public const LOGDAT_PATH        As String   = "Log"
Public const DBDAT_PATH         As String   = "DB"
Public const CALDAT_PATH        As String   = "CalibSet"
Public const STDDAT_PATH        As String   = "Standard"
Public const RTGSET_PATH        As String   = "Rtgset"
Public const SPEC_PATH			As String   = "Speclist"
Public const AUTO_PATH			As String   = "AutoSave"
Public const DEVSPEC_PATH		As String   = "Speclist\Device"
Public const SAMSPEC_PATH		As String   = "Speclist\Sample"
Public const HORIZON_PATH		As String   = "HorizonSet"
Public const FACTORY_PATH       As String   = "FactorySetting"
Public Const DevListFile		As String	= "DeviceList.ini"
Public Const DevSettingFile		As String	= "DeviceSetting.ini"
Public Const TempParaFile		As String	= "TempPara.ini"
Public Const InpSelectFile		As String	= "InputSelection.ini"
Public Const QRCODE_PATH		As String	= "QRCode"
Public Const APP_Path			As String	= "C:\SZKApps"
Public Const SZK_MES_Path		As String	= "Sizuka Tool"
Public Const SZK_ANA_Path		As String	= "Sizuka Data Manager"
Public Const APPVersions_Path	As String	= "C:\SZKApps\Versions"

'Public Const TRQ_FACT_PERCENT			As Double = 1
'Public Const TRQ_FACT_PERCENT_TO_GFCM	As Double = 0.44

''' <summary>'SQL Server 認証種別</summary>
Public Enum eSQL_AUTH_TYPE
	iWINDWOS = 0        'Windows認証
	iSQLSVR             'SQL Server認証
End Enum

''' <summary>測定最小分解能[mS]</summary>
Public Const MEAS_TIME_RESOLUTION		As Int32 = 2

''' <summary>通信上の更新周期[mS]</summary>
Public Const MEAS_TIME_BLOCK			As Int32 = 1000

''' <summary>測定データマップ上のサンプル数 1000mS/2mS（1秒ごとにブロック更新）</summary>
Public Const MEAS_BLOCK_DATA_NUM		As Int32 = MEAS_TIME_BLOCK / MEAS_TIME_RESOLUTION

''' <summary>サンプリング周波数(Hz)</summary>
Public Const MEAS_SAMPLE_FREQ			As Int32 = MEAS_TIME_BLOCK / MEAS_TIME_RESOLUTION

''' <summary>ラジアン->度変換</summary>
Public Const RadToDegRate				As Double = 180 / Math.Pi

''' <summary>度->ラジアン変換</summary>
Public Const DegToRadRate				As Double = Math.Pi / 180

''' <summary>基本フォント</summary>
Public Const BASE_FONT					As String = "Meiryo UI"


Public Const cXYAmpSwitch				As Int32 = 6000			'XY位置の振幅設定による解像度切替え閾値(um)


'▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼
'   構造体
'▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲
																		 'スペックデータ
'▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼
'   API宣言
'▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲
''' <summary>指定ハンドルの座標を取得</summary>
''' <param name="hWnd">コントロールのハンドル</param>
''' <param name="lpRect">座標</param>
<Runtime.InteropServices.DllImport("user32.dll", CharSet:=Runtime.InteropServices.CharSet.Auto)> _
Public Function GetWindowRect(ByVal hWnd As IntPtr, ByRef lpRect As Rectangle) As Boolean
End Function

'▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼
'   パブリック変数
'▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲
'''<summary>言語設定番号 0：英語 1:日本語 2:中国語簡体  3:韓国語 4:中国語繁体 </summary>
Public p_iLanguage		As Integer

'''<summary>データ/ファイルクラス</summary>
Public p_dtFile			As New DataFile
Public p_clSpec			As New clSpec(0)				'SPECクラス生成
Public datCal			As New DataCal					'計算クラス生成

'''<summary>本体ID[SZK999]</summary>
Public p_sEquipName		As String			'本体ID

'''<summary> リアルタイム測定クラス。 </summary>
Public p_RTM() As RealTimeMeas

'''<summary> 現在のツール設定。frmOptionsにより設定 </summary>
Public p_cts As CToolSettings = New CToolSettings()

'''<summary>XY動作モード文字列のList作成</summary>
Public lstXYMode As New List(Of String)(New String() {"0 : X Only(Square Wave)", "1 : X Only(Sin Wave)", "2 : XY (Sin Wave)"})

'''<summary>ゲルタイム計算方法</summary>
Public lstCalcMethod As New List(Of String)(New String() {"THR", "SLOPE", "EXTRAPOLATION", "PHASE", "MELT"})
Public Enum eCalcMethod
	THR = 0
	SLOPE
	EXTRA
	PHASE
	MELT
End Enum

'測定データ拡張子
Public sDataExt		As String = "dat"

Public lstGelTimeJudge As New List(Of String)(New String() {"NG", "OK", "NA"})		'ゲルタイム判定 1：OK  0：NG  2:無効


'''<summary>外挿法 ゲルタイム選択</summary>
Public lstGelTimeSel	As New List(Of String)(New String() {"Under","Middle","Over"})

'''<summary>重量N換算係数 (Dig->N) 起動時ファイルから計算</summary>
Public p_dNConv			As Double = 1.0
'''<summary>Z重量g換算係数 (Dig->g) 起動時ファイルから計算</summary>
Public p_dZConv			As Double = 1.0

'''<summary>Z重量1g当たりのuN (uN/g) </summary>
Public p_cuNperGram		As Double = 9806.65

'キャリブレーションフォーム,フローカーブフォームが表示されているか（ゼロ調モニタ表示切替に使用）
Public p_FormCalibVisible As Boolean = False

'補正計算処理を行う測定方法の種別
Public Enum eHoseiMeasMode
	GelTime = 0
	FullCalib
	ZeroCalib
	FlowCurve
	GelTimeLT
End Enum
'補正計算処理を行う計算モード
Public Enum eHoseiCalcMode
	Batch = 0
	RealTime = 1
	LTRealTime = 2
	LTBatch = 3
End Enum

'Z補正用フィルタクラス生成
Public p_zFilX As New Filter
Public p_zFilY As New Filter
Public p_zFilA As New Filter
Public p_zFilC As New Filter


'TCP割り込み書込みフラグ
Public p_bInterrupt                         As Boolean = False          '※割り込みを使用する場合、1スレッド内で行い、フラグがOFFになっていることを確認すること！！
'割り込み書込みバイト配列
Public p_bytWInterrupt()                    As Byte
'割り込み書込み結果読込みバイト配列
Public p_bytRInterrupt(256)                 As Byte
Public p_sPrinterName						As String = ""
Public p_sLtPrintMemo                       As String = ""
Public p_sPaperSize                         As String = "A3"
Public p_sbtnTempText						As String = "Temp"				'温度ボタンの状況を表示
Public p_sbtnZeroText						As String = "Zero"				'ゼロ調ボタンの状況を表示
Public p_sbtnOrgText						As String = "Org"				'原点復帰ボタンの状況を表示
Public p_sbtnMeasText						As String = "Meas"				'測定ボタン状況を表示

Public p_iDspErrStts1                       As Integer = 0					'エラー表示用エラーステータス
Public p_iDspErrStts2                       As Integer = 0					'エラー表示用エラーステータス
Public p_bInterRock                         As Boolean						'インターロック状態(True：風防カバーが開いている)
Public p_bInterRockOrgPos                   As Boolean						'インターロック状態(True：原点復帰されていない)
Public p_bInterRockEnable                   As Boolean						'インターロック有効/無効
Public p_bLongTimeMes                       As Boolean = False				'長時間測定モードフラグ

Public p_bBtnEnable							As Boolean = False				'ボタン有効フラグ
Public p_bValid_OfficeVer_Install			As Boolean = False              'Officeインストール判定フラグ

''' <summary>ヒータ温度上下限(degC)</summary>
Public HEATER_UPLMT_DEV						As Double = 400.0		'装置制限
Public HEATER_LWLMT_DEV						As Double = 0.0
Public HEATER_UPLMT_NUD						As Double = 390.0		'入力制限
Public HEATER_LWLMT_NUD						As Double = 0.0



'リモートSQL Serverデータベース接続先設定
'Public p_iSqlSvrExpEnable					As Integer = 0										'リモートサーバーエクスポート機能有効
'Public p_eRmtSqlAuthType					As eSQL_AUTH_TYPE = eSQL_AUTH_TYPE.iWINDWOS			'リモートSQL Server認証種別
'Public p_sRmtSqlSvrName						As String = ".\SQLEXPRESS"                          'リモートSQL Server名                
'Public p_sRmtSqlUserID						As String = ""                                      'リモートSQL Server認証ログイン名
'Public p_sRmtSqlPassword					As String = ""										'リモートSQL Server認証パスワード
'Public p_sRmtSqlExpDataPath					As String = ""										'リモートSQL Serverエクスポートデータファイルの出力先パス

'SQLite DBクラス
Public p_clDB								As New clSizukaDB_SQLite			'SizukaDBクラス

'ログ表示用
Public p_sfrmDebugLog As String = ""


Public p_ZeroJob As Int16 = -1							'ゼロ調整 メインフォーム上のJOB変数
''' <summary> エラー発生中 </summary>
Public p_bError As Boolean = False



'キャリブレーション応力較正情報(1測定単位)
Public Structure stPoint
	Dim bSelected				As Boolean				'較正選択状態
	Dim dAmplitudeX				As Double				'X測定振幅[mm]
	Dim dPeriodX				As Double				'X測定周期[S]
	Dim dAmplitudeY				As Double				'Y測定振幅[mm]
	Dim dPeriodY				As Double				'Y測定周期[S]
	Dim dAmplitudeZ				As Double				'Z測定振幅[mm]
	Dim dPeriodZ				As Double				'Z測定周期[S]
	Dim iSample					As Int32				'サンプル数(2mSサンプルでの)
	Dim iFullVal	            As Int64				'較正結果[AD値]
	'Dim dAlpha					As Double				'XY軸直交角度(rad)
	Dim dPPhaseX				As Double				'直線モード X位置位相差(rad)
	Dim dPPhaseCX				As Double				'円モード X位置位相差(rad)
	Dim dPPhaseCY				As Double				'円モード Y位置位相差(rad)
	Dim dPPhaseZ				As Double				'点モード Z位置位相差(rad)
	Dim iCol					As Int32				'DGV上のカラム値
	Dim iRow					As Int32				'DGV上のライン値
	Dim dMeasDate				As DateTime				'測定日時
End Structure
Public p_FClbX()				As stPoint				'X動作  測定条件・結果(フルストレス)
Public p_FClbC()				As stPoint				'円動作 測定条件・結果(フルストレス)
Public p_FClbP()				As stPoint				'点動作 測定条件・結果(フルストレス)
Public p_FMeasDateX				As String = Nothing
Public p_FMeasDateC				As String = Nothing
Public p_FMeasDateP				As String = Nothing
Public p_ZClbX()				As stPoint				'X動作  測定条件・結果(ゼロストレス)
Public p_ZClbC()				As stPoint				'円動作 測定条件・結果(ゼロストレス)
Public p_ZClbP()				As stPoint				'点動作 測定条件・結果(ゼロストレス)
Public p_ZMeasDateX				As String = Nothing
Public p_ZMeasDateC				As String = Nothing
Public p_ZMeasDateP				As String = Nothing

Public p_SetPath				As String					'キャリブレーション結果ファイル保存先
Public p_sFullStoreFileName		As String					'最新較正結果が保存されるファイル名
Public p_sZeroStoreFileName		As String					'最新較正結果が保存されるファイル名
'Public p_sStoreFileNameRx		As String					'最新較正結果が保存されるファイル名
'Public p_sStoreFileNameRy		As String					'最新較正結果が保存されるファイル名
'Public p_sStoreFileNameAlpha	As String					'最新較正結果が保存されるファイル名
Public p_sStoreFileNamePxPhase	As String					'最新較正結果が保存されるファイル名
Public p_sStoreFileNamePyPhase	As String					'最新較正結果が保存されるファイル名
Public p_sStoreFileNamePzPhase	As String					'最新較正結果が保存されるファイル名
Public p_sStoreFileNameWeight	As String					'最新較正結果が保存されるファイル名
Public p_sMessage				As String                   'ステータスメッセージ
Public p_bCalibFileOutput		As Boolean = True			'キャリブレーション測定データ出力スイッチ
Public p_XYCoefMeasDate			As String = Nothing			'XY重量変換係数測定日時
Public p_ZCoefMeasDate			As String = Nothing			'Z重量変換係数測定日時


Public p_bExportSelect()		As Boolean = Nothing		'エクスポート項目選択記憶
Public p_sExportOutPass			As String = Nothing			'エクスポート出力パス記憶
Public p_iExportThinning		As int32 = 0				'エクスポート間引き数

Public p_iDir					As Int16 = 1				'データ履歴表 データ表方向割当 0:系列方向垂直 1:系列方向水平 
Public p_HistWinSize			As New System.Drawing.Size(1024, 924)		'データ履歴表 ウィンドウサイズ記憶
Public p_HistWinPos				As New System.Drawing.Point(0, 0)			'データ履歴表 ウィンドウ位置記憶

Public p_bDebugFileOutEnable	As Boolean = False			'デバッグファイル出力有効スイッチ（出荷検査時使用）
Public p_bInputEnMeasureing		As Boolean = False			'測定中の入力許可スイッチ
Public p_bCalibValChangeEn		As Boolean = False			'キャリブレーション値のキー入力許可スイッチ
Public p_bZStrStabDis			As Boolean = False			'Z安定度チェックを無効
Public p_bPcLifeCheckDis		As Boolean = False			'PC生存確認チェックを無効
Public p_bMeasTimeEn			As Boolean = False			'測定時間制限 外す
Public p_bScanTimeEn			As Boolean = False			'通信スキャン時間表示を許可
Public p_bEquipListEn			As Boolean = False			'装置リスト編集を許可
Public p_bCalcResultFOutEn		As Boolean = False			'バッチ計算後の結果をファイル出力有効
Public p_bTempNoLimitEn			As Boolean = False			'温度制限を 外す

Public p_iFailedOrderP			As Int32	= 0				'着圧制御時の制御失敗ラッチ変数 1:失敗
Public p_iGuideXYbeforeGap		As Int32	= 0				'測定GAPとXY動作中のガイドメッセージラッチ変数 0:表示しない 1:表示中
															'「測定GAPが決定される前にXY動作中になりました。測定は継続します。」
Public p_SPCSL					As Double	= 0				'SPC機能 規格中央値
Public p_SPCUSL					As Double	= 0				'SPC機能 規格上限値
Public p_SPCLSL					As Double	= 0				'SPC機能 規格下限値

'▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼
'   プライベート変数
'▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲

'■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
'   パブリック関数
'■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■

<STAThread> Sub Main()
	Application.EnableVisualStyles()
    Application.SetCompatibleTextRenderingDefault(False)
	Application.Run(New frmMain)
End Sub

''' <summary>
''' キャリブレーションファイル環境を初期化
''' </summary>
Public Sub InitialCalibFull()
Try
	If p_sEquipName = ""
		Exit sub
	End If
	If p_sEquipName.Substring(0,3)<>"SZK"
		Exit Sub
	End If
	'フォルダが存在しない場合は新規作成する
	Dim sEquipName As String = p_sEquipName
	p_SetPath = Path.Combine( SIZUKA_PATH, p_sEquipName, CALDAT_PATH )
	p_sFullStoreFileName	= "FullStressCondition"	& ".csv"	'最新較正結果が保存される
	p_sZeroStoreFileName	= "ZeroStressCondition"	& ".csv"	'最新較正結果が保存される
	p_sStoreFileNameWeight	= "WeightCalibration"	& ".csv"	'最新較正結果が保存される
    If Not FileSystem.DirectoryExists(p_SetPath) Then
	'フォルダが存在しない
		FileSystem.CreateDirectory(p_SetPath)
	End if
	if Not FileSystem.FileExists(Path.Combine(p_SetPath, p_sFullStoreFileName))
	'ファイルが存在しない
		FileSystem.CopyFile(".\" & CALDAT_PATH & "\" & p_sFullStoreFileName,	Path.Combine(p_SetPath, p_sFullStoreFileName))
	End if
	if Not FileSystem.FileExists(Path.Combine(p_SetPath, p_sZeroStoreFileName))
	'ファイルが存在しない
		FileSystem.CopyFile(".\" & CALDAT_PATH & "\" & p_sZeroStoreFileName,	Path.Combine(p_SetPath, p_sZeroStoreFileName))
	End If
	if Not FileSystem.FileExists(Path.Combine(p_SetPath, p_sStoreFileNameWeight))
		FileSystem.CopyFile(".\" & CALDAT_PATH & "\" & p_sStoreFileNameWeight,		Path.Combine(p_SetPath, p_sStoreFileNameWeight))
    End If
Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub

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

'================================================
'指定背景色の文字色（黒or白）を取得
'引数	lClr:			背景色
'戻り値 文字色
'================================================
Public Function GetForeColor(ByVal lClr As Color) As Color
	Dim clrRtn As Color
	Dim iLumi As Integer

	Try
		'輝度を取得
		With lClr
			iLumi = .R * 0.298912 + .G * 0.586611 + .B * 0.114478
		End With

		If iLumi > 127 Then
			clrRtn = Color.Black
		Else
			clrRtn = Color.White
		End If
	Catch ex As Exception
		Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
		clrRtn = Color.Black
	End Try

	Return clrRtn
End Function

'================================================
'カラーダイアログから選択色を取得
'引数	iDefClr:		デフォルトカラー
'戻り値 0以上:			RGBカラー値
'		-1:				取得失敗
'================================================
Public Function OpenColorDialog(Optional ByVal iDefClr As Integer = 0) As Integer
	Dim iRtn As Integer
	Dim cd As ColorDialog
	Dim dr As DialogResult

	iRtn = -1
	'オブジェクト生成
	cd = New ColorDialog()

	Try
		'初期選択カラー
		cd.Color = System.Drawing.ColorTranslator.FromOle(iDefClr)
		'カスタムカラー表示
		cd.FullOpen = True
		'使用可能な全ての色を基本セットに表示
		cd.SolidColorOnly = True
		'純色のみ表示
		cd.SolidColorOnly = True
		'カスタムカラーを任意で設定
		cd.CustomColors = New Integer() {iDefClr}
		'Helpを表示
		cd.ShowHelp = False

		'カラーダイアログ表示
		dr = cd.ShowDialog()

		'選択終了後
		If dr = DialogResult.OK Then
			'「OK」が押された場合は選択された色を設定
			iRtn = System.Drawing.ColorTranslator.ToOle(cd.Color)
		Else
			'「OK」以外の場合はデフォルト値を設定
			iRtn = iDefClr
		End If
	Catch ex As Exception
		Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
		iRtn = -1
	End Try

	cd.Dispose()
	Return iRtn
End Function

'================================================
'整数を2つのByteに変換
'引数	iNum:			変換する整数
'		bByt:			変換後のByte配列
'		iNum:			変換するByte配列数
'戻り値 true:			変換成功
'		false:			変換失敗
'================================================
Public Function ConvIntToByteArry(ByVal iInt As Integer, ByRef bByt() As Byte, _
									Optional ByVal iNum As Integer = 0) As Boolean
	Dim bRtn As Boolean = True
	Dim sTemp As String
	Dim sTemp2 As String
	Dim iLen As Integer
	Dim iCnt As Integer

	Try
		'16進数に変換
		sTemp = Convert.ToString(iInt, 16)

		'偶数桁でない場合、頭に0を付与
		For iCnt = sTemp.Length To iNum * 2 - 1
			sTemp = "0" & sTemp
		Next
		iLen = sTemp.Length

		'配列数が未指定の場合、全ての桁として変換する
		If iNum <= 0 Then
			iNum = (iLen / 2)
		End If

		'配列の確保
		ReDim bByt(iNum - 1)
		'配列の初期化
		For iCnt = 0 To iNum - 1
			bByt(iCnt) = 0
		Next

		'16進数文字列の下位からByte化していく
		For iCnt = 0 To iNum - 1
			'下位桁から順に2桁ずつ16進数文字列を取得
			sTemp2 = Mid(sTemp, iLen - (iCnt * 2 + 1), 2)
			'Byte型に変換
			bByt(iCnt) = CByte("&H" & sTemp2)
		Next

	Catch ex As Exception
		Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
		bRtn = False
	End Try

	Return bRtn
End Function

''' <summary>
''' 言語ファイルによる画面と文字列変数の初期化
''' </summary>
''' <param name="frm">		画面名</param>
''' <param name="iLanguage">言語番号</param>
''' <param name="tsTool">	Tooltip名(Option)</param>
''' <param name="sMes">		セットされる文字列配列(Option)</param>
Public Sub SetFormLanguage(ByRef frm As Form, ByVal iLanguage As Integer, Optional ByVal tsTool As ToolTip = Nothing, Optional ByRef sMes() As String = Nothing)
	Try
		sbSetFormDispString(frm, eLANGUAGE.iENG, tsTool,sMes)
		sbSetFormDispString(frm, iLanguage, tsTool,sMes)
		frm.Icon = Icon.ExtractAssociatedIcon(Process.GetCurrentProcess().MainModule.FileName)
	Catch ex As Exception

	End Try
End Sub

'================================================
'コントロール名（または、フィールド名）を検索し、オブジェクトを取得
'引数	frm:			フォーム
'		sName:			検索名
'戻り値 オブジェクト
'================================================
Public Function FindControlByFieldName(ByVal frm As Form, ByVal sName As String) As Object
	'まずプロパティ名を探し、見つからなければフィールド名を探す
	Dim tType As System.Type = frm.GetType()

	'検索名のタイプを取得
	Dim piTemp As System.Reflection.PropertyInfo
	piTemp = tType.GetProperty(sName, System.Reflection.BindingFlags.Public Or _
								System.Reflection.BindingFlags.NonPublic Or _
								System.Reflection.BindingFlags.Instance Or _
								System.Reflection.BindingFlags.DeclaredOnly)

	'取得できた場合
	If Not piTemp Is Nothing Then
		'タイプを戻す
		Return piTemp.GetValue(frm, Nothing)
	End If

	'検索名のフィールド情報を取得
	Dim fiTemp As System.Reflection.FieldInfo
	fiTemp = tType.GetField(sName, System.Reflection.BindingFlags.Public Or _
							System.Reflection.BindingFlags.NonPublic Or _
							System.Reflection.BindingFlags.Instance Or _
							System.Reflection.BindingFlags.DeclaredOnly)

	'取得できなかった場合
	If fiTemp Is Nothing Then
		'Nothingを戻す
		Return Nothing
	End If

	'取得した情報のタイプを戻す
	Return fiTemp.GetValue(frm)
End Function

'================================================
'ログ出力
'引数	sMsg:			ログ出力メッセージ
'戻り値 なし
'================================================
Public Sub WriteLog(ByVal sMsg As String, Optional ByVal iLogLvl As Integer = eLOG_LEVEL.iFATAL, Optional ByVal bAppend As Boolean = True)
	Dim sPath As String
	Dim sLine As String = ""

	Try
		'ログファイルパスの設定
		sPath = SIZUKA_PATH & "\" & LOGdat_PATH & "\" & "SizukaTool" & Format(Now, "yyyyMMdd") & ".Log"
		sLine = Format(Now, "yyyy/MM/dd HH:mm:ss.fff ")
		sLine = sLine & "<" & p_sLOG_LEVEL(iLogLvl).PadRight(8) & ">"
		sLine = sLine & sMsg
		'ファイルの存在確認
		If True Then
			'ファイルが存在する場合ログを出力
			'ファイル書き込み
			Dim enc As System.Text.Encoding = System.Text.Encoding.UTF8
			'既にログファイルがあるか確認
			If System.IO.File.Exists(sPath) Then
				'テキストファイルを開く
				Dim bs As Byte() = System.IO.File.ReadAllBytes(sPath)

				'文字コードを判別する
				enc = DetectEncodingFromBOM(bs)
				If enc Is Nothing Then
					'BOMが見つからない場合はSJIS
					enc = System.Text.Encoding.GetEncoding("shift_jis")
				End If
			End If

			Using sWriter As New IO.StreamWriter(sPath, bAppend, enc)
				sWriter.WriteLine(sLine)
			End Using
		End If

		FrmDebugLog(sLine)
	Catch ex As Exception
		Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	End Try
End Sub

'***************************************
''' <summary>指定日数分以上のログデータは削除</summary>
''' <param name="sDirPath">検索先のディレクトリパス</param>
''' <param name="sFileName">検索するファイルパス</param>
''' <param name="iMaxNum">上限ファイル数</param>
''' <returns>True：成功、False：失敗</returns>
Public Function DeleteLog(ByVal sDirPath As String, Optional ByVal sFileName As String = "*", Optional ByVal iMaxNum As Integer = 365) As Boolean
	Dim bRtn As Boolean = True

	Try
		'指定フォルダの指定ファイルパスを取得（サブディレクトリは含まない）
		Dim sFilePaths As String() = System.IO.Directory.GetFiles(sDirPath, sFileName, System.IO.SearchOption.TopDirectoryOnly)

		'DataTableを用意
		Dim tblLog As DataTable = New DataTable("tblLogFile")
		'ファイルパス列、ログ日付列を追加
		tblLog.Columns.Add("FilePath", Type.GetType("System.String"))
		tblLog.Columns.Add("LogDate", Type.GetType("System.DateTime"))

		'データを格納
		Dim iDateLen As Integer = ("yyyyMMdd").Length
		Dim sTemp As String
		Dim sDate As String = ""
		Dim dtRow As DataRow
		For Each sPath As String In sFilePaths
			'ファイル名(拡張子なし)を取得
			sTemp = System.IO.Path.GetFileNameWithoutExtension(sPath)

			'日付部分を取得
			sTemp = Strings.Right(sTemp, iDateLen)

			'年月日のスラッシュを挿入
			sDate = Strings.Mid(sTemp, 1, 4)
			sDate = sDate & "/" & Strings.Mid(sTemp, 5, 2)
			sDate = sDate & "/" & Strings.Mid(sTemp, 7, 2)

			'日付変換可能な場合、DataTableに追加
			If IsDate(sDate) = True Then
				dtRow = tblLog.NewRow
				dtRow("FilePath") = sPath
				dtRow("LogDate") = CDate(sDate)
				tblLog.Rows.Add(dtRow)
			End If
		Next

		'DataTableに1件以上ある場合
		If tblLog.Rows.Count > 0 Then
			'日付でフィルタリング
			Dim dtResults() As DataRow = tblLog.Select("LogDate<='" & Format(Now.AddDays(-1 * iMaxNum), "yyyy/MM/dd") & "'")

			'古いファイルを削除
			For Each dtDel As DataRow In dtResults
				'ファイルを削除
				Try
					System.IO.File.Delete(dtDel("FilePath"))
				Catch ex As Exception
					bRtn = False
				End Try
			Next
		End If

	Catch ex As Exception
		bRtn = False
	End Try

	Return bRtn
End Function

'指定桁数で丸め
Public Function FormatRound(ByVal dVal As Double, ByVal iDigit As Integer) As String
	Dim sRtn As String = CStr(dVal)
	Dim sFormat As String

	Try
		sFormat = "#"

		If iDigit = 0 Then
			sFormat = sFormat & "0"
		Else
			sFormat = sFormat & "0."

			For iCnt = 1 To iDigit
				sFormat = sFormat & "0"
			Next
		End If

		sRtn = Format(dVal, sFormat)

	Catch ex As Exception
		Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	End Try

	Return sRtn
End Function

'ログ表示用
Public Sub FrmDebugLog(ByVal sLog As String)
	Try
		p_sfrmDebugLog = sLog

	Catch ex As Exception
'		Debug.Print("")
	End Try
End Sub

'フォルダ作成
Public Function CreateFolder(ByVal sPath As String) As Boolean
	Try
		'フォルダの存在チェック
		If System.IO.Directory.Exists(sPath) = False Then
			'フォルダ作成
			System.IO.Directory.CreateDirectory(sPath)
		End If

	Catch ex As Exception
		Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
		'異常終了
		Return False
	End Try

	Return True
End Function

'ファイルコピー
Public Function CopyFile(ByVal sSource As String, ByVal sDest As String, ByVal bAppend As Boolean) As Boolean
	Try
		'ファイルの存在チェック
		If System.IO.File.Exists(sSource) = True Then
			'ファイルをコピー(上書き可)
			System.IO.File.Copy(sSource, sDest, bAppend)
		Else
			Return False
		End If
	Catch ex As Exception
		Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
		'異常終了
		Return False
	End Try

	Return True
End Function

'ファイル削除
Public Function DeleteFile(ByVal sPath As String) As Boolean

	Try
		'ファイルの存在チェック
		If System.IO.File.Exists(sPath) = True Then
			'ファイル削除
			System.IO.File.Delete(sPath)
		End If
	Catch ex As Exception
		Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
		'異常終了
		Return False
	End Try

	Return True
End Function

'ファイル名変更
Public Function RenameFile(ByVal sPath As String, ByVal sNewName As String, Optional ByVal bFullPath As Boolean = False) As Boolean
	Try
		'ファイルの存在チェック
		If System.IO.File.Exists(sPath) = True Then
			Dim sDest As String

			If bFullPath = False Then
				'新ファイルパスの設定
				sDest = System.IO.Path.GetDirectoryName(sPath)
				sDest = sDest & "\" & sNewName & System.IO.Path.GetExtension(sPath)
			Else
				sDest = sNewName
			End If

			System.IO.File.Move(sPath, sDest)
		Else
			'異常終了
			Return False
		End If

	Catch ex As Exception
		Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
		Return False
	End Try

	Return True
End Function

'================================================
'ファイル保存先選択コモンダイアログ
'引数	sDefPath:		デフォルトパス
'		sFilter:		フィルタ設定
'戻り値	選択パス
'=======================================================================
Public Function SaveFileDialog(ByVal sDefPath As String, ByVal sFilter As String) As String
	Dim sRtn As String = ""

	'OpenFileDialogクラスのインスタンスを作成
	Dim ofd As New SaveFileDialog()

	'デフォルトのファイルパスを設定
	ofd.FileName = System.IO.Path.GetFileName(sDefPath)
	'デフォルトのディレクトリを設定
	ofd.InitialDirectory = System.IO.Path.GetDirectoryName(sDefPath)
	'[ファイルの種類]に表示される選択肢を設定
	ofd.Filter = sFilter
	'ダイアログボックスを閉じる前に現在のディレクトリを復元するようにする
	ofd.RestoreDirectory = True
	'存在しないファイルを選択した場合の警告OFF
	ofd.CheckFileExists = False

	'ダイアログを表示する
	If ofd.ShowDialog() = DialogResult.OK Then
		'OKボタンがクリックされたとき
		'選択されたファイル名を表示する
		sRtn = ofd.FileName
	End If

	Return sRtn
End Function

'================================================
'ファイル読込先選択コモンダイアログ
'引数	sDefPath:		デフォルトパス
'		sFilter:		フィルタ設定
'戻り値	選択パス
'=======================================================================
Public Function OpenFileDialog(ByVal sDefPath As String, ByVal sFilter As String) As String
	Dim sRtn As String = ""

	'OpenFileDialogクラスのインスタンスを作成
	Dim ofd As New OpenFileDialog()

	'デフォルトのファイルパスを設定
	ofd.FileName = System.IO.Path.GetFileName(sDefPath)
	'デフォルトのディレクトリを設定
	ofd.InitialDirectory = System.IO.Path.GetDirectoryName(sDefPath)
	'[ファイルの種類]に表示される選択肢を設定
	ofd.Filter = sFilter
	'ダイアログボックスを閉じる前に現在のディレクトリを復元するようにする
	ofd.RestoreDirectory = True
	'存在しないファイルを選択した場合の警告OFF
	ofd.CheckFileExists = False

	'ダイアログを表示する
	If ofd.ShowDialog() = DialogResult.OK Then
		'OKボタンがクリックされたとき
		'選択されたファイル名を表示する
		sRtn = ofd.FileName
	End If

	Return sRtn
End Function
'===============================================
'Iniファイルデータ読み込み
'引数   sPath:      Iniファイルパス
'       sSec:       セクション名
'       sItem:      アイテム名
'       sDef:       デフォルト値
'戻り値 データ
'===============================================
Public Function GetIniStrData(ByVal sPath As String, ByVal sSec As String, ByVal sItem As String, _
								Optional ByVal sDef As String = "", Optional ByRef bNum As Boolean = False) As String
	Dim sRtn As String = sDef

	Try
		' 格納用バッファ
		Dim sGet As System.Text.StringBuilder = New System.Text.StringBuilder(512)
		'Iniファイル読み込み
		GetPrivateProfileString(sSec, sItem, sDef, sGet, sGet.Capacity, sPath)
		'読み込みバッファの文字列整頓
		sRtn = sGet.ToString()

		'数値変換不可な場合、引数にFalseを渡す
		If IsNumeric(sRtn) = False Then
			bNum = False
		End If
	Catch ex As Exception
		Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
		sRtn = sDef
	End Try

	Return sRtn
End Function

'===============================================
'Iniファイルデータ書込み
'引数   sPath:      Iniファイルパス
'       sSec:       セクション名
'       sItem:      アイテム名
'       sVal:       書き込み値
'戻り値 なし
'===============================================
Public Sub SetIniData(ByVal sPath As String, ByVal sSec As String, ByVal sItem As String, ByVal sVal As String)
	Try
		'Iniファイル書込み
		WritePrivateProfileString(sSec, sItem, sVal, sPath)
	Catch ex As Exception
		Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	End Try
End Sub
'DataGridViewのデータをコピー
Public Sub CopyDataGridView(ByVal dgvSource As DataGridView, ByVal dgvDest As DataGridView, _
								Optional ByVal bDataOnly As Boolean = True)
	Try
		If bDataOnly = False Then
			'コピー先の行削除
			dgvDest.Rows.Clear()
			dgvDest.Columns.Clear()

			'コピー元から列情報取得
			For Each dgvClm As DataGridViewColumn In dgvSource.Columns
				dgvDest.Columns.Add(dgvClm.Clone)
			Next

			Dim dgrRow As New DataGridViewRow
			dgrRow.CreateCells(dgvDest)

			'コピー元からデータ取得
			For Each dgvRow As DataGridViewRow In dgvSource.Rows
				dgrRow = New DataGridViewRow
				dgrRow.CreateCells(dgvDest)
				For Each dgvcell As DataGridViewCell In dgvRow.Cells
					dgrRow.Cells(dgvcell.ColumnIndex).Value = dgvcell.Value
				Next
				dgvDest.Rows.Add(dgrRow)
			Next
		Else
			'コピー元からデータ取得
			For iRow As Integer = 0 To dgvDest.RowCount - 1
				dgvDest(1, iRow).Value = dgvSource(1, iRow).Value
			Next
		End If

	Catch ex As Exception
		Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	End Try
End Sub
'文字列の取得
Public Function GetStr(ByVal oSource As Object, Optional ByVal sDef As String = "") As String
	Dim sRtn As String = ""

	Try
		sRtn = sDef

		'Nothing出ないとき
		If IsNothing(oSource) = False AndAlso IsDBNull(oSource) = False Then
			sRtn = CStr(oSource)
		End If

	Catch ex As Exception
		Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	End Try

	Return sRtn
End Function
'数値文字列の小数桁が全てゼロであればカットした整数部のみの文字列を返す
Public Function CutDecimalDigitZeroStr(ByVal sNumStr As String) As String
	Dim iDigPos As Integer = sNumStr.IndexOf(".")
	Dim blZeroOnly As Boolean = True
	Dim sRet As String = sNumStr

	If iDigPos < 0 Then
		'小数点が見つからない場合は文字列をそのまま返す
		Return sNumStr
	ElseIf iDigPos = sNumStr.Length - 1 Then
		'小数点で文字列が終わっている場合は小数点を削った文字列を返す
		Return sNumStr.Substring(0, iDigPos)
	Else
		'小数点が見つかった場合は小数桁にゼロ以外が含まれているかチェック
		For i As Integer = iDigPos + 1 To sNumStr.Length - 1
			If sNumStr.Substring(i, 1) <> "0" Then
				'小数点桁にゼロ以外が見つかった
				blZeroOnly = False
				Exit For
			End If
		Next i

		'小数点桁が全てゼロの場合は整数部のみの文字列を取得
		If blZeroOnly Then
			sRet = sNumStr.Substring(0, iDigPos)
		End If
	End If

	Return sRet
End Function
'（西暦年2桁*10000 + 月2桁*100 + 日2桁）の整数値をyyyy/MM/ddのフォーマットへ変換する
Public Function IntToDateStringConv(ByVal int As Int32) As String
Dim s As String
Try	

	s = int.ToString()
	If s.Length <> 6
		Return Nothing
	End If
	If Not IsNothing(s)
		s = "20" & s.Substring(0,2) & "/" & s.Substring(2,2) & "/" & s.Substring(4,2)                  
	End If
	Return s
Catch ex As Exception
	Return Nothing
End Try
End Function
'■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
'   プライベート関数
'■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■

'================================================
'フォームのテキストを設定
'引数	frm:			フォーム
'		iLanguage:		言語Index
'		tsTool:			ToolTip
'戻り値 True:			Tool連携モード
'		false:			本体単独モード、異常発生
'================================================
Private Sub sbSetFormDispString(ByRef frm As Form, ByVal iLanguage As Integer, Optional ByVal tsTool As ToolTip = Nothing, Optional ByRef sMes() As String = Nothing)
	Try
		'フォーム名、言語設定値からファイルパスを設定
		Dim sPath As String = My.Application.Info.DirectoryPath.ToString() & "\Language\" & frm.Name & "_" & CStr(iLanguage) & ".txt"
		'ファイルが存在しない場合、処理を終了
		If System.IO.File.Exists(sPath) = False Then
			Exit Sub
		End If
		''文字コード設定
		'Dim sCode As String
		'Select Case iLanguage
		'	Case eLANGUAGE.iHNT, eLANGUAGE.iKRA, eLANGUAGE.iHNS
		'		sCode = "utf-8"
		'	Case Else
		'		sCode = "Shift_JIS"
		'End Select

		'ファイルを開く
		'Using parser As New FileIO.TextFieldParser(sPath, System.Text.Encoding.GetEncoding(sCode))
		Using parser As New FileIO.TextFieldParser(sPath, System.Text.Encoding.UTF8)
			'ファイルOpen設定
			parser.TextFieldType = FileIO.FieldType.Delimited
			parser.SetDelimiters("==") ' 区切り文字は","
			parser.TrimWhiteSpace = True

			Dim sRowData() As String
			Dim sControl() As String
			'Dim cs As Control()
			Dim oTemp As Object
			Dim iItemIndex As Integer
			Dim sTemp As String
			Dim sText() As String
			Dim iCnt As Integer

			'ファイルの終端までループ
			While Not parser.EndOfData
				'iniファイルから一行読み込み
				sRowData = parser.ReadFields()

				'配列長が1以上の場合
				If sRowData.Length > 1 Then
					'フォーム名の変更の場合
					If sRowData(0).ToUpper = "$THIS.TEXT" Then
						frm.Text = sRowData(1)

						'次のループへ
						Continue While
					End If

					'1要素目からコントロール情報を取得(コントロール名.表示プロパティ)
					sControl = Split(sRowData(0), ".")

					'コントロール情報の配列長が2未満の場合、次のループへ
					If sControl.Length > 1 Then
						'コントロール名からオブジェクトを取得
						oTemp = FindControlByFieldName(frm, sControl(0))
						'オブジェクトが見つかった場合
						If IsNothing(oTemp) = False Then
							'表示プロパティに応じて表示を変更
							Select Case sControl(1).ToUpper
								Case "TEXT"
									sText = Split(sRowData(1), "\r\n")
									oTemp.Text = sText(0)
									For iCnt = 1 To sText.Length - 1
										oTemp.Text = oTemp.Text & vbCrLf & sText(iCnt)
									Next
								Case "HEADERTEXT"
									sText = Split(sRowData(1), "\r\n")
									oTemp.HeaderText = sText(0)
									For iCnt = 1 To sText.Length - 1
										oTemp.HeaderText = oTemp.HeaderText & vbCrLf & sText(iCnt)
									Next
								Case "TOOLTIP"
									If IsNothing(tsTool) = False Then
										sRowData(1) = Replace(sRowData(1), "\r\n", vbCrLf)
										tsTool.SetToolTip(oTemp, sRowData(1))
									End If
								Case "TOOLTIPTEXT"
									oTemp.ToolTipText = sRowData(1)
								Case Else
									sTemp = sControl(1).ToUpper
									'表示プロパティに「ITEM」が含まれる場合
									If InStr(sTemp, "ITEMS") > 0 Then
										sTemp = Replace(sTemp, "ITEMS", "")
										'項目数が0の場合はIndexを付与する
										If sTemp.Length = 0 Then
											sTemp = 0
										End If
										'アイテムIndexを取得
										If Integer.TryParse(sTemp, iItemIndex) = True Then
											oTemp.Items(iItemIndex) = sRowData(1)
										End If
									End If
							End Select

						End If
					Else
						If sRowData(0) = "STRING" Then
							sText = Split(sRowData(1), ":", 2)
							If sText.Length > 1 AndAlso IsNumeric(sText(0)) = True Then
								iCnt = CInt(sText(0))
								Select Case frm.Name
								Case frmMain.Name
									If iCnt < frmMain.p_sMes.Length Then
										frmMain.p_sMes(iCnt) = Replace(sText(1), "\r\n", vbCrLf)
									End If
								Case Else
									If iCnt <= sMes.GetUpperBound(0)
										sMes(iCnt) = Replace(sText(1), "\r\n", vbCrLf)
									End If
								End Select
							End If
						End If
					End If
				End If
			End While
		End Using
	Catch ex As Exception
		Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	End Try
End Sub

'''<summary> BackgroundWorkerの終了待機。 </summary>
Public Sub WaitBgwEnd(ByVal sBgwName As String, ByRef blBgwAlive As Boolean)
	'バックグランド処理終了時間計測開始
	Dim stpw As Stopwatch = Stopwatch.StartNew()

	'バックグランド処理が実行中の間は待機
	Do While (blBgwAlive = True)
		System.Threading.Thread.Sleep(100)

		'時間計測停止
		stpw.Stop()

		'5秒タイムアウト
		If (stpw.ElapsedMilliseconds > 5000) Then
			WriteLog("BackgroundWorker end wait timeout.", eLOG_LEVEL.iDEBUG)
			Exit Do
		End If

		'時間計測再開
		stpw.Start()
	Loop

	Debug.Print(sBgwName & " : End time = " & stpw.ElapsedMilliseconds.ToString & "[ms]")
End Sub
''' <summary>
''' BOMを調べて、文字コードを判別する。
''' </summary>
''' <param name="bytes">文字コードを調べるデータ。</param>
''' <returns>BOMが見つかった時は、対応するEncodingオブジェクト。
''' 見つからなかった時は、null。</returns>
Public Function DetectEncodingFromBOM(bytes As Byte()) As System.Text.Encoding

	If bytes.Length < 2 Then
		Return Nothing
	End If
	If (bytes(0) = &HFE) AndAlso (bytes(1) = &HFF) Then
		'UTF-16 BE
		Return New System.Text.UnicodeEncoding(True, True)
	End If
	If (bytes(0) = &HFF) AndAlso (bytes(1) = &HFE) Then
		If (4 <= bytes.Length) AndAlso _
			(bytes(2) = &H0) AndAlso (bytes(3) = &H0) Then
			'UTF-32 LE
			Return New System.Text.UTF32Encoding(False, True)
		End If
		'UTF-16 LE
		Return New System.Text.UnicodeEncoding(False, True)
	End If
	If bytes.Length < 3 Then
		Return Nothing
	End If
	If (bytes(0) = &HEF) AndAlso (bytes(1) = &HBB) AndAlso _
		(bytes(2) = &HBF) Then
		'UTF-8
		Return New System.Text.UTF8Encoding(True, True)
	End If
	If bytes.Length < 4 Then
		Return Nothing
	End If
	If (bytes(0) = &H0) AndAlso (bytes(1) = &H0) AndAlso _
		(bytes(2) = &HFE) AndAlso (bytes(3) = &HFF) Then
		'UTF-32 BE
		Return New System.Text.UTF32Encoding(True, True)
	End If

	Return Nothing
End Function

'''<summary> Iniファイルへの書き込み（UTF-8版）。 </summary>
Public Function WritePrivateProfileString(ByVal lpAppName As String, _
											ByVal lpKeyName As String, _
											ByVal lpString As String, _
											ByVal lpFileName As String) As Integer

	Dim sr As System.IO.StreamReader
	Dim s As String = ""
	Dim s1 As String = ""
	Dim s2 As String = ""
	Dim sArry() As String
	Dim blFindApp As Boolean = False
	Dim blFindKey As Boolean = False

	Try
		If System.IO.File.Exists(lpFileName) Then

			'テキストファイルを開く
			Dim bs As Byte() = System.IO.File.ReadAllBytes(lpFileName)

			'文字コードを判別する
			Dim enc As System.Text.Encoding = DetectEncodingFromBOM(bs)

			If enc Is Nothing Then
				'BOMが見つからない場合はSJISとして開く
				sr = New System.IO.StreamReader(lpFileName, System.Text.Encoding.UTF8)
			Else
				'BOMが見つかった場合はBOMが付加したUnicodeかUTF-8として開く
				sr = New System.IO.StreamReader(lpFileName)
			End If

			'内容を一行ずつ読み込む
			While sr.Peek() > -1
				s = sr.ReadLine()

				If s Is Nothing Then
					Exit While
				End If

				If (blFindApp = False) Then
					'セクションが見つかっていない
					If s.IndexOf("[" & lpAppName & "]") >= 0 Then
						'セクションが見つかった
						blFindApp = True
					End If
					s1 &= s & vbCrLf
				Else
					'セクションが見つかっているがキーが見つかっていない
					If s.IndexOf("[") = 0 Then
						'次のセクションが見つかった場合は、キーを追加
						s2 &= lpKeyName & "=" & lpString & vbCrLf
#If 1
						'残りの全行を読み込む()
						While sr.Peek() > -1
							s2 &= s & vbCrLf

							s = sr.ReadLine()

							If s Is Nothing Then
								Exit While
							End If
						End While
#end if
					ElseIf s.IndexOf("=") > 0 Then
						'キー行が見つかった場合は、キー名と値に分解
						sArry = Split(s, "=")

						If sArry(0).Trim.ToUpper = lpKeyName.Trim.ToUpper Then
							'キー名が一致した場合は値を書き換え
							blFindKey = True
							s2 &= sArry(0) & "=" & lpString & vbCrLf
#If 1
							'残りの全行を読み込む
							While sr.Peek() > -1
								s = sr.ReadLine()

								If s Is Nothing Then
									Exit While
								End If

								s2 &= s & vbCrLf
							End While
#end if
						Else
							'キー名が一致しない場合は読み込みのみ
							s2 &= s & vbCrLf
						End If
					Else
						'セクションでもキーでもない行は読み込みのみ
						s2 &= s & vbCrLf
					End If
				End If
			End While

			'閉じる
			sr.Close()

			If (blFindApp = False) Then
				'セクションが見つかっていない場合はセクション, キーを追加
				s2 &= "[" & lpAppName & "]" & vbCrLf
				s2 &= lpKeyName & "=" & lpString & vbCrLf
			ElseIf (blFindApp = True) And (blFindKey = False) Then
				'セクションが見つかっているがキーが見つかっていない場合はキーを追加（最終セクションだった場合）
				s2 &= lpKeyName & "=" & lpString & vbCrLf
			End If

		Else
			'Iniファイルが無かった場合は、セクション, キーを追加
			s1 = "[" & lpAppName & "]" & vbCrLf
			s2 = lpKeyName & "=" & lpString & vbCrLf
		End If

		'全行書き込み
		My.Computer.FileSystem.WriteAllText(lpFileName, s1 & s2, False)

	Catch ex As Exception
	    Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
		Return -1
	End Try

	Return 0
End Function

'''<summary> Iniファイルからの文字列の読み込み（UTF-8版）。 </summary>
Public Function GetPrivateProfileString(ByVal lpAppName As String, _
										ByVal lpKeyName As String, ByVal lpDefault As String, _
										ByRef lpReturnedString As System.Text.StringBuilder, _
										ByVal nSize As Integer, ByVal lpFileName As String) As Integer

	Dim sr As System.IO.StreamReader
	Dim s As String = ""
	Dim sArry() As String
	Dim blFindApp As Boolean = False
	Dim blFindKey As Boolean = False

	Try
		If System.IO.File.Exists(lpFileName) Then

			'テキストファイルを開く
			Dim bs As Byte() = System.IO.File.ReadAllBytes(lpFileName)

			'文字コードを判別する
			Dim enc As System.Text.Encoding = DetectEncodingFromBOM(bs)

			If enc Is Nothing Then
				'BOMが見つからない場合はSJISとして開く
				sr = New System.IO.StreamReader(lpFileName, System.Text.Encoding.UTF8)
			Else
				'BOMが見つかった場合はBOMが付加したUnicodeかUTF-8として開く
				sr = New System.IO.StreamReader(lpFileName)
			End If

			'内容を一行ずつ読み込む
			While sr.Peek() > -1
				s = sr.ReadLine()

				If s Is Nothing Then
					Exit While
				End If

				If (blFindApp = False) Then
					'セクションが見つかっていない
					If s.IndexOf("[" & lpAppName & "]") >= 0 Then
						'セクションが見つかった
						blFindApp = True
					End If
				Else
					'セクションが見つかっているがキーが見つかっていない
					If s.IndexOf("[") = 0 Then
						'次のセクションが見つかった
						Exit While
					ElseIf s.IndexOf("=") > 0 Then
						'キー行が見つかった場合は、キー名と値に分解
						sArry = Split(s, "=")

						If sArry(0).Trim.ToUpper = lpKeyName.Trim.ToUpper Then
							'キー名が一致した場合は値を取得
							blFindKey = True
							lpReturnedString.Clear()
							lpReturnedString.Append(sArry(1))
							Exit While
						Else
							'キー名が一致しない場合は読み込みのみ
						End If
					Else
						'セクションでもキーでもない行は読み込みのみ
					End If
				End If
			End While

			'閉じる
			sr.Close()

			If (blFindApp = False) Then
				'セクションが見つかっていない場合はデフォルト値を返す
				lpReturnedString.Clear()
				lpReturnedString.Append(lpDefault)
			ElseIf (blFindApp = True) And (blFindKey = False) Then
				'セクションが見つかっているがキーが見つかっていない場合はデフォルト値を返す（最終セクションだった場合）
				lpReturnedString.Clear()
				lpReturnedString.Append(lpDefault)
			End If
		Else
			'Iniファイルが無かった場合は、デフォルト値を返す
			lpReturnedString.Clear()
			lpReturnedString.Append(lpDefault)
		End If

	Catch ex As Exception
		Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
		Return -1
	End Try

	Return 0
End Function

'''<summary> Iniファイルからの数値の読み込み（UTF-8版）。 </summary>
Public Function GetPrivateProfileInt(ByVal lpAppName As String, _
										ByVal lpKeyName As String, _
										ByVal nDefault As Integer, _
										ByVal lpFileName As String) As Integer

	Dim sr As New System.Text.StringBuilder(50)
	Dim iRet As Integer

	Try
		'Iniファイルからの文字列の読み込み
		iRet = GetPrivateProfileString(lpAppName, lpKeyName, nDefault.ToString(), sr, sr.Capacity, lpFileName)

		'数値判定
		If (iRet = 0) AndAlso (IsNumeric(sr.ToString()) = True) Then
			iRet = CInt(sr.ToString())
		Else
			iRet = nDefault
		End If

	Catch ex As Exception
		Return -1
	End Try

	Return iRet
End Function

'''<summary> Iniファイルからの実数の読み込み（UTF-8版）。 </summary>
Public Function GetPrivateProfilefloat(ByVal lpAppName As String, _
										ByVal lpKeyName As String, _
										ByVal nDefault As Double, _
										ByVal lpFileName As String) As Double

	Dim sr As New System.Text.StringBuilder(50)
	Dim iRet As Double

	Try
		'Iniファイルからの文字列の読み込み
		iRet = GetPrivateProfileString(lpAppName, lpKeyName, nDefault.ToString(), sr, sr.Capacity, lpFileName)

		'数値判定
		If (iRet = 0) AndAlso (IsNumeric(sr.ToString()) = True) Then
			iRet = CDbl(sr.ToString())
		Else
			iRet = nDefault
		End If

	Catch ex As Exception
		Return -1
	End Try

	Return iRet
End Function
''' <summary>
''' テキストファイル内の区切り文字がカンマかタブかを判定
''' </summary>
''' <param name="sFilePath">区切り文字を調べるテキストファイルパス。</param>
''' <param name="enc">エンコーディング。</param>
''' <returns>区切り文字。
''' わからなかった時は、空文字。</returns>
Public Function DetectTextDelimiter(ByVal sFilePath As String, ByVal enc As System.Text.Encoding) As String
	Dim sDelimiter As String = ""

	If File.Exists(sFilePath) = True Then
		'タブが見つかった場合は区切り文字はタブと判定
		Dim sr As StreamReader = New StreamReader(sFilePath, enc)
		Dim sLine As String = sr.ReadLine()
		If sLine.IndexOf(vbTab) > 0 Then
			sDelimiter = vbTab
		Else
			sDelimiter = ","
		End If
		sr.Close()
	End If

	Return sDelimiter
End Function
''' <summary>
''' ファイルのパスに使用できない文字が含まれているかチェック
''' </summary>
''' <param name="sSrcText">チェック元の文字列。</param>
''' <param name="blRet">不正なファイル名をチェックする正規表現の戻りの有無。</param>
''' <param name="rCheck">不正なファイル名をチェックする正規表現。</param>
''' <returns>True:含まれている
''' False:含まれていない</returns>
Public Function CheckInvalidPathChars(ByVal sSrcText As String, Optional ByVal blRet As Boolean = False, Optional ByRef rCheck As Regex = Nothing) As Boolean
	'不正なファイル名をチェックする正規表現
	Dim r As New Regex( _
			"[\x00-\x1f<>:""/\\|?*]" & _
			"|^(CON|PRN|AUX|NUL|COM[0-9]|LPT[0-9]|CLOCK\$)(\.|$)" & _
			"|[\. ]", _
		System.Text.RegularExpressions.RegexOptions.IgnoreCase)

	'正規表現の戻りを要求
	If blRet Then
		rCheck = r
	End If

	'マッチしたら、不正なファイル名が含まれている
	If r.IsMatch(sSrcText) Then
		Return True
	End If

	'不正なファイル名が含まれていない
	Return False
End Function

''' <summary>
''' ファイルのパスに使用できない文字を別の文字に置換
''' </summary>
''' <param name="sSrcText">置換元の文字列。</param>
''' <param name="sReplaceChar">置換文字。</param>
''' <returns>置換文字で置換された文字列。
''' ファイルのパスに使用できない文字がなかった時は、置換元の文字列。</returns>
Public Function ReplaceInvalidPathChars(ByVal sSrcText As String, ByVal sReplaceChar As String) As String
	Dim sDstText As String = sSrcText

	'不正なファイル名をチェックする正規表現
	Dim r As Regex = Nothing

	'ファイルのパスに使用できない文字が含まれているかチェック
	If CheckInvalidPathChars(sSrcText, True, r) Then
		'不正なファイル名の置換
		sDstText = r.Replace(sSrcText, sReplaceChar)
	End If

	Return sDstText
End Function
''' <summary>
''' ファイル名に使用できない文字が含まれているかチェック
''' </summary>
''' <param name="sSrcText">対象文字列</param>
''' <returns>TRUE：含まれている FALSE：含まれていない</returns>
Public Function CheckInvalidFileNameChars(ByVal sSrcText As String) As Boolean
	Dim bRet As Boolean = True
	If IsNothing(sSrcText)				'対象文字列が無
		Return bRet
	End If
	If sSrcText.Length < 1				'対象文字列長が0以下
		Return bRet
	End If

	If sSrcText.Contains("\")
		Return bRet
	End If
	If sSrcText.Contains("/")
		Return bRet
	End If
	If sSrcText.Contains(":")
		Return bRet
	End If
	If sSrcText.Contains("*")
		Return bRet
	End If
	If sSrcText.Contains("?")
		Return bRet
	End If
	If sSrcText.Contains("""")
		Return bRet
	End If
	If sSrcText.Contains("<")
		Return bRet
	End If
	If sSrcText.Contains(">")
		Return bRet
	End If
	If sSrcText.Contains("|")
		Return bRet
	End If
	If sSrcText.Contains("[")
		Return bRet
	End If
	If sSrcText.Contains("]")
		Return bRet
	End If
	If sSrcText.Contains(",")
		Return bRet
	End If
	Return False
End Function



''' <summary>
''' Double配列を設定値で埋める
''' </summary>
''' <param name="desArr"></param>
''' <param name="dfill"></param>
Public Sub FillDoubleArr(ByVal desArr As Double(), ByVal dfill As Double)
    If desArr.Length > 0 Then
        Dim arr As IEnumerable(Of Double) = Enumerable.Repeat(dfill, desArr.Length)
        Array.Copy(arr.ToArray(), desArr, desArr.Length)
    End If
End Sub

''' <summary>
''' 共通リアルタイム値表示 データグリッドビューの初期設定
''' 12項目
''' </summary>
''' <param name="dv"></param>
''' <param name="iType">0:キャリブレーション 1:フローカーブ</param>
Public Sub RealTimeMonitorValDispSetting(ByRef dv As DatagridView, ByVal iType As int16)
	With dv
		.Rows.Add( "HP Temp.[deg C]",	"0.0" )
		.Rows.Add( "PB Temp.[deg C]",	"0.0" )
		.Rows.Add( "SG Temp.[deg C]",	"0.0" )
		.Rows.Add( "HP MV[%]",			"0.0" )
		.Rows.Add( "PB MV[%]",			"0.0" )
		.Rows.Add( "SG MV[%]",			"0.0" )
		.Rows.Add( "X[mm]",				"0.000" )
		.Rows.Add( "Y[mm]",				"0.000" )
		.Rows.Add( "Z[mm]",				"0.000" )
		.Rows.Add( "X Stress[AD]",		"0" )
		.Rows.Add( "Y Stress[AD]",		"0" )
		.Rows.Add( "Z Stress[AD]",		"0" )
		.Rows.Add( "Z Str.CR[g/10S]",	"0.000" )

		.AllowUserToResizeColumns = False
		.AllowUserToResizeRows = False	
		.ColumnHeadersVisible = False
		.rowHeadersVisible = False
		.Columns(1).DefaultCellStyle.Alignment =     DataGridViewContentAlignment.MiddleRight
		.GridColor = Color.DarkGray
		.BackgroundColor = SystemColors.Control
		Select Case iType
		Case 1	'フローカーブ
			'.ScrollBars = ScrollBars.Vertical
			.Height = 28 * 13
			.Columns(0).DefaultCellStyle.Font = New Font(BASE_FONT, 11, FontStyle.Regular)
			.Refresh
		Case Else
			.Height = 28 * 13
			.Columns(0).DefaultCellStyle.Font = New Font(BASE_FONT, 11, FontStyle.Regular)
			.Refresh
		End Select
	End With
End Sub
''' <summary>
''' 共通 リアルタイム値更新
''' </summary>
''' <param name="dv"></param>
''' <param name="bZeroEnable"></param>
Public Sub	RealTimeMonitorValDispUpdate( Byref dv As DatagridView, ByVal bZeroEnable As Boolean, ByVal iType As int16)
	With dv
		.Item(1,0).Value = p_RTM(0).p_mltcp.HeatTempAct.ToString("F1")				'HP温度
		.Item(1,1).Value = p_RTM(0).p_mltcp.HeatTemp2Act.ToString("F1")				'PB温度
		.Item(1,2).Value = p_RTM(0).p_mltcp.HeatTemp3Act.ToString("F1")				'SZ温度
		.Item(1,3).Value = p_RTM(0).p_mltcp.HeatTempOpe.ToString("F1")				'HPMV値
		.Item(1,4).Value = p_RTM(0).p_mltcp.HeatTemp2Ope.ToString("F1")				'PBMV値
		.Item(1,5).Value = p_RTM(0).p_mltcp.HeatTemp3Ope.ToString("F1")				'SGMV値
		.Item(1,6).Value = (p_RTM(0).p_mltcp.CurrPosX * 0.001).ToString("F3")		'X軸現在位置
		.Item(1,7).Value = (p_RTM(0).p_mltcp.CurrPosY * 0.001).ToString("F3")		'Y軸現在位置
		.Item(1,8).Value = (p_RTM(0).p_mltcp.HeatBPos * 0.001).ToString("F3")		'Z軸現在位置
		.Item(1,9).Value = p_RTM(0).p_mltcp.CurrStrsX().ToString					'X応力
		.Item(1,10).Value = p_RTM(0).p_mltcp.CurrStrsY().ToString					'Y応力
		.Item(1,11).Value = p_RTM(0).p_mltcp.CurrStrsZgram().ToString("F1")			'Z応力
		.Item(1,12).Value = frmMain.dZStability.ToString("F3")						'Z安定度

		if bZeroEnable
		'ゼロオフセット有効
			.Item(0,8).Value = "ΔX Stress[AD]"				'X応力名
			.Item(0,9).Value = "ΔY Stress[AD]"				'Y応力名
			.Item(0,10).Value = "ΔZ Stress[g]"				'Y応力名
		Else
			.Item(0,8).Value = "X Stress[AD]"				'X応力名
			.Item(0,9).Value = "Y Stress[AD]"				'Y応力名
			.Item(0,10).Value = "Z Stress[g]"				'Y応力名
		End If
	End With
End Sub

''' <summary>
''' 測定条件の100％応力較正値をセット(較正設定ファイルを読込み応力値と減衰比を得る)
''' </summary>
''' <param name="iMode">0:フル値を得る 2:ゼロ値を得る(測定時)</param>
''' <param name="ZClbX">応力値</param>
''' <param name="ZClbC">応力値</param>
''' <param name="ZClbP">応力値</param>
''' <param name="FClbX">応力値</param>
''' <param name="FClbC">応力値</param>
''' <param name="FClbP">応力値</param>
''' <param name="dData">応力値</param>
''' <param name="dAttenuation">2秒周期からの比率</param>
''' <param name="dAlpha">位相角</param>
''' <param name="dPxPhase">x位置位相差</param>
''' <param name="dPyPhase">y位置位相差</param>
''' <param name="dPzPhase">y位置位相差</param>
''' <param name="iActMode">応力検知モード 0,1,2</param>
''' <param name="dAmp">指定振幅</param>
''' <param name="dPrd">指定周期</param>
''' <returns></returns>
Public function FullStressCalibValueGet( _
							ByVal iMode As Int16, _
							ByRef ZClbX() As stPoint, ByRef ZClbC() As stPoint, ByRef ZClbP() As stPoint, _
							ByRef FClbX() As stPoint, ByRef FClbC() As stPoint, ByRef FClbP() As stPoint, _
							ByRef dData as Double, _
							ByRef dAttenuation as Double, _
							ByRef dAlpha as Double, ByRef dPxPhase as Double, ByRef dPyPhase as Double, ByRef dPzPhase as Double, _
							ByVal iActMode As Int16, _
							ByVal dAmp as double, _
							ByVal dPrd as double _
							) As Int32
''Public function FullStressCalibValueGet( _
''							ByVal iMode As Int16, _
''							ByRef ZClbX() As stPoint, ByRef ZClbC() As stPoint, _
''							ByRef FClbX() As stPoint, ByRef FClbC() As stPoint, _
''							ByRef dData as Double, _
''							ByRef dAttenuation as Double, _
''							ByRef dRx as Double, ByRef dRy as Double, ByRef dAlpha as Double, ByRef dPxPhase as Double, ByRef dPyPhase as Double, _
''							ByVal iActMode As Int16, _
''							ByVal dAmp as double, _
''							ByVal dPrd as double _
''							) As Int32
Dim i As int32
Dim uD			As stPoint() = Nothing		'ストレス情報
Dim uD1			As stPoint() = Nothing		'ストレス情報(iMode1,2の時はZERO用)
Dim uD2			As stPoint() = Nothing		'ストレス情報(iMode1,2の時はFULL用)
Dim sidx		As Int32 = -1				'指定周期のIdx
Dim eidx		As Int32 = -1				'指定周期のIdx
Dim sidx2		As Int32 = -1				'２秒周期のIdx
Dim eidx2		As Int32 = -1				'２秒周期のIdx
Dim dFullVal	As Double = -1				'指定条件からの応力値
Dim dFullVal2	As Double = -1				'指定振幅の2秒周期応力値(フルストレステーブル)
Dim dFullVal3	As Double = -1				'指定振幅の指定周期応力値(フルストレステーブル)
Dim uDa()		As stPoint = Nothing		'指定周期のキャリブレーション情報(Zero)
Dim uDb()		As stPoint = Nothing		'２秒周期のキャリブレーション情報(Full)
Dim uDc()		As stPoint = Nothing		'指定周期のキャリブレーション情報(Full)
Dim i2PointOver As Integer = 0				'2点間範囲を超えた指定周期
Try

	Select Case iMode
	Case 0			'フル応力値を得る
		Select Case iActMode
		Case clSpec.eStrsSens.Line
			If IsNothing(FClbX)
				Return -2
			End If
			uD1 = FClbX
			uD2 = FClbX
		Case clSpec.eStrsSens.Circle
			If IsNothing(FClbC)
				Return -2
			End If
			uD1 = FClbC
			uD2 = FClbC
		Case clSpec.eStrsSens.Point
			If IsNothing(FClbP)
				Return -2
			End If
			uD1 = FClbP
			uD2 = FClbP
		End Select
	Case 2			'ゼロ応力値を得る
		Select Case iActMode
		Case clSpec.eStrsSens.Line
			If IsNothing(ZClbX) Or IsNothing(FClbX)
				Return -2
			End If
			uD1 = ZClbX
			uD2 = FClbX
		Case clSpec.eStrsSens.Circle
			If IsNothing(ZClbC) Or IsNothing(FClbC)
				Return -2
			End If
			uD1 = ZClbC
			uD2 = FClbC
		Case clSpec.eStrsSens.Point
			If IsNothing(ZClbP) Or IsNothing(FClbP)
				Return -2
			End If
			uD1 = ZClbP
			uD2 = FClbP
		End Select
	End Select

	CutOutSelectPeriod(iActMode, dPrd, uD1, uDa )			'指定周期を切り出す uD1(Full/Zero) -> uDa
	If IsNothing(uDa)
		Return -2
	End If
	CutOutSelectPeriod(iActMode, 2.0, uD2, uDb )			'2秒周期を切り出す uD2(Full) -> uDb
	If IsNothing(uDb)
		Return -3
	End If
	CutOutSelectPeriod(iActMode, dPrd, uD2, uDc )			'指定周期を切り出す uD2(Full/Zero) -> uDc
	If IsNothing(uDc)
		Return -4
	End If

	'---------------------------------------------------------------------
	'完全一致を探す
	'指定周期で検索 ----------------------------------------------
	Dim dAmplitude(1) As Double
	eidx = -1
	i2PointOver = 0
	For i = 0 To uDa.GetUpperBound(0)
		Select Case iActMode
		Case clSpec.eStrsSens.Line, clSpec.eStrsSens.Circle
			dAmplitude(0) = uDa(i).dAmplitudeX
		Case clSpec.eStrsSens.Point
			dAmplitude(0) = uDa(i).dAmplitudeZ
		End Select
		if dAmplitude(0) = dAmp
			dFullVal	= uDa(i).iFullVal		'100%応力
			Select Case iActMode
			Case clSpec.eStrsSens.Line
				dPxPhase	= uDa(i).dPPhaseX		'X位置位相差
			Case clSpec.eStrsSens.Circle
				dPxPhase	= uDa(i).dPPhaseCX		'CX位置位相差
				dPyPhase	= uDa(i).dPPhaseCY		'CY位置位相差
			Case clSpec.eStrsSens.Point
				dPzPhase	= uDa(i).dPPhaseZ		'Z位置位相差
			End Select
			Exit For					
		Elseif i+1 <= uDa.GetUpperBound(0)
			Select Case iActMode
			Case clSpec.eStrsSens.Line, clSpec.eStrsSens.Circle
				dAmplitude(1) = uDa(i+1).dAmplitudeX
			Case clSpec.eStrsSens.Point
				dAmplitude(1) = uDa(i+1).dAmplitudeZ
			End Select
			if dAmplitude(0) < dAmp AndAlso dAmplitude(1) > dAmp
			'指定振幅が２点間へ入った
				eidx = i + 1
				sidx = i
				Exit For					
			End If
		End If
	Next i
	if i > uDa.GetUpperBound(0)
	'完全一致はなかった
	'振幅2点間を直線補完する
		if eidx = -1
		'指定振幅が表外にある
		'最後尾の2点間を延長する
			sidx = uDa.GetUpperBound(0) - 1
			eidx = uDa.GetUpperBound(0)
			i2PointOver = 1
		End If

	End If
	if eidx <> -1
	'フルストレスの線形補完
		LinearTwoPoint(0, uDa, dAmp, sidx, eidx, dFullVal, iActMode)
		If i2PointOver = 0
			Select Case iActMode
			Case clSpec.eStrsSens.Line
			'x位置位相差の線形補完
				LinearTwoPoint(1, uDa, dAmp, sidx, eidx, dPxPhase, iActMode)
			Case clSpec.eStrsSens.Circle
			'x位置位相差の線形補完
				LinearTwoPoint(2, uDa, dAmp, sidx, eidx, dPxPhase, iActMode)
			'y位置位相差の線形補完
				LinearTwoPoint(3, uDa, dAmp, sidx, eidx, dPyPhase, iActMode)
			Case clSpec.eStrsSens.Point
			'Z位置位相差の線形補完
				LinearTwoPoint(4, uDa, dAmp, sidx, eidx, dPzPhase, iActMode)
			End Select
		''x位置位相差の線形補完
		'	LinearTwoPoint(1, uDa, dAmp, sidx, eidx, dPxPhase)
		''y位置位相差の線形補完
		'	LinearTwoPoint(2, uDa, dAmp, sidx, eidx, dPyPhase)
		''Z位置位相差の線形補完
		'	LinearTwoPoint(3, uDa, dAmp, sidx, eidx, dPzPhase)
		Else
		'位相は2点外の場合は最終インデックスの値を使用する
			Select Case iActMode
			Case clSpec.eStrsSens.Line
				dPxPhase = uDa(eidx).dPPhaseX
			Case clSpec.eStrsSens.Circle
				dPxPhase	= uDa(eidx).dPPhaseCX		'CX位置位相差
				dPyPhase	= uDa(eidx).dPPhaseCY		'CY位置位相差
			Case clSpec.eStrsSens.Point
				dPzPhase	= uDa(eidx).dPPhaseZ		'Z位置位相差
			End Select
		End If
	End if

	'---------------------------------------------------------------------
	'指定周期で検索 フル------------------------------------------------
	eidx2 = -1
	For i = 0 To uDc.GetUpperBound(0)
		Select Case iActMode
		Case clSpec.eStrsSens.Line, clSpec.eStrsSens.Circle
			dAmplitude(0) = uDc(i).dAmplitudeX
		Case clSpec.eStrsSens.Point
			dAmplitude(0) = uDc(i).dAmplitudeZ
		End Select
		'if uDc(i).dAmplitudeX = dAmp
		if dAmplitude(0) = dAmp
			dFullVal3	= uDc(i).iFullVal		'指定周期の100%応力
			Exit For
		Elseif i+1 <= uDc.GetUpperBound(0)
			Select Case iActMode
			Case clSpec.eStrsSens.Line, clSpec.eStrsSens.Circle
				dAmplitude(1) = uDc(i+1).dAmplitudeX
			Case clSpec.eStrsSens.Point
				dAmplitude(1) = uDc(i+1).dAmplitudeZ
			End Select
			'if uDc(i).dAmplitudeX < dAmp AndAlso uDc(i+1).dAmplitudeX > dAmp
			if dAmplitude(0) < dAmp AndAlso dAmplitude(1) > dAmp
			'指定振幅が２点間へ入った
				eidx2 = i + 1
				sidx2 = i
				Exit For
			End If
		End If
	Next i
	if i > uDc.GetUpperBound(0)
	'完全一致はなかった
	'2秒周期の２点間補間
		if eidx2 = -1
		'指定振幅が表外にある
		'最後尾の2点間を延長する
			sidx2 = uDc.GetUpperBound(0) - 1
			eidx2 = uDc.GetUpperBound(0)
			i2PointOver = 1
		End If

	End If 
	if eidx2 <> -1
	'フルストレスの線形補完(指定周期)
		LinearTwoPoint(0, uDc, dAmp, sidx2, eidx2, dFullVal3, iActMode)
	End if

	'2秒周期で検索 フル------------------------------------------------
	eidx2 = -1
	For i = 0 To uDb.GetUpperBound(0)
		Select Case iActMode
		Case clSpec.eStrsSens.Line, clSpec.eStrsSens.Circle
			dAmplitude(0) = uDb(i).dAmplitudeX
		Case clSpec.eStrsSens.Point
			dAmplitude(0) = uDb(i).dAmplitudeZ
		End Select
		'if uDb(i).dAmplitudeX = dAmp
		if dAmplitude(0) = dAmp
			dFullVal2	= uDb(i).iFullVal		'2秒周期の100%応力
			Exit For
		Elseif i+1 <= uDb.GetUpperBound(0)
			'if uDb(i).dAmplitudeX < dAmp AndAlso uDb(i+1).dAmplitudeX > dAmp
			Select Case iActMode
			Case clSpec.eStrsSens.Line, clSpec.eStrsSens.Circle
				dAmplitude(1) = uDb(i+1).dAmplitudeX
			Case clSpec.eStrsSens.Point
				dAmplitude(1) = uDb(i+1).dAmplitudeZ
			End Select
			if dAmplitude(0) < dAmp AndAlso dAmplitude(1) > dAmp
			'指定振幅が２点間へ入った
				eidx2 = i + 1
				sidx2 = i
				Exit For
			End If
		End If
	Next i
	if i > uDb.GetUpperBound(0)
	'完全一致はなかった
	'2秒周期の２点間補間
		if eidx2 = -1
		'指定振幅が表外にある
		'最後尾の2点間を延長する
			sidx2 = uDb.GetUpperBound(0) - 1
			eidx2 = uDb.GetUpperBound(0)
		End If
	End If 
	if eidx2 <> -1
	'フルストレスの線形補完(2秒)
		LinearTwoPoint(0, uDb, dAmp, sidx2, eidx2, dFullVal2, iActMode)
	End if
	'debug.Print(dFullVal2 & "  " & dFullVal3 & "  " & a & "  " & b)
	If dAmp = 0
		dAttenuation	= 1
	Else
		dAttenuation	= if(dFullVal2 = 0, 1, dFullVal2 / dFullVal3)					'（指定振幅の2秒周期応力値）/（指定振幅の指定周期応力値）
	End If

	Select Case iMode
	Case 0	'フルキャリブレーション
		dData			= dFullVal * dAttenuation
		If dData <= 0
		'0以下はありえない(キャリブレーションしていない？)
			Return -5
		End If
''	Case 1	'ゼロキャリブレーション
''		dData			= dFullVal * dAttenuation
	Case 2		'測定時（ゲルタイム、ゼロキャリブレーション, フローカーブ...）
		dData			= dFullVal
	End Select
	Return 0
Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Return -1
End Try
End	function
''' <summary>
''' 指定周期のキャリブレーション情報を切り出す
''' </summary>
''' <param name="iActMode">0,1,2</param>
''' <param name="dPrd"></param>
''' <param name="uD1"></param>
''' <param name="uDa"></param>
Private Sub	CutOutSelectPeriod(ByVal iActMode As int16, ByVal dPrd As Double, ByVal uD1 As stPoint(), ByRef uDa As stPoint())
	Dim i As Integer
	Dim j As Integer
Try
	j = 0
	uDa = Nothing
	Dim dPeriod As Double = 0
	For i = 0 To uD1.GetUpperBound(0)
		Select Case iActMode
		Case clSpec.eStrsSens.Line,clSpec.eStrsSens.Circle
			dPeriod = uD1(i).dPeriodX
		Case clSpec.eStrsSens.Point
			dPeriod = uD1(i).dPeriodZ
		End Select
		If dPeriod = dPrd
			If IsNothing(uDa)
				Array.Resize(uDa, 1)
			Else
				Array.Resize(uDa, uDa.Length + 1)
			End If
			uDa(j) = uD1(i)
			j += 1
		End If
	Next i

Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub
''' <summary>
''' ２点間を通る直線式を最小二乗法で算出しｙ値を計算
''' </summary>
''' <param name="iMode">0:Full,1:XPhase,2:CXPhaze,3:CYPhaze,4:ZPhaze</param>
''' <param name="uDa"></param>
''' <param name="dAmp"></param>
''' <param name="sidx"></param>
''' <param name="eidx"></param>
''' <param name="dData"></param>
''' <returns></returns>
Private Function LinearTwoPoint(ByVal iMode As Int16, ByVal uDa As stPoint(), ByVal dAmp As Double, ByVal sidx As int16, ByVal eidx As Int16, Byref dData As Double, ByVal iActMode As int16) As Int32
Dim dX()		As Double = Nothing			'変換前
Dim dY()		As Double = Nothing			'
Dim dNX()		As Double = Nothing			'変換後
Dim dNY()		As Double = Nothing
Dim dRet()		As Double = Nothing
Dim a			As Double					'直線式 傾き
Dim b			As Double					'直線式 Y切片
Try
	'ワーク領域確保
	Array.Resize(dX, 2)
	Array.Resize(dY, 2)
	Array.Resize(dNX, 2)
	Array.Resize(dNY, 2)
	'変換前配列にデータテーブルからセット
	Select Case iActMode
	Case clSpec.eStrsSens.Point
		dX(0) = uDa(sidx).dAmplitudeZ
		dX(1) = uDa(eidx).dAmplitudeZ
	Case Else
		dX(0) = uDa(sidx).dAmplitudeX
		dX(1) = uDa(eidx).dAmplitudeX
	End Select
	Select Case iMode
	Case 0
		dY(0) = uDa(sidx).iFullVal
		dY(1) = uDa(eidx).iFullVal
	Case 1
		dY(0) = uDa(sidx).dPPhaseX
		dY(1) = uDa(eidx).dPPhaseX
	Case 2
		dY(0) = uDa(sidx).dPPhaseCX
		dY(1) = uDa(eidx).dPPhaseCX
	Case 3
		dY(0) = uDa(sidx).dPPhaseCY
		dY(1) = uDa(eidx).dPPhaseCY
	Case 4
		dY(0) = uDa(sidx).dPPhaseZ
		dY(1) = uDa(eidx).dPPhaseZ
	End Select
	'最小二乗法を1次式近似で実行
	dRet = datCal.fnLeastSquaresMethod(dX, dY, dNX, dNY, 2, 1)
	a = dRet(1)		'傾きをセット
	b = dRet(0)		'Y切片をセット
	dData = a * dAmp + b
	Return 0

Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Return -1
End Try
End Function

''' <summary>
''' ヘッダ部コメント等 改行変換
''' </summary>
''' <param name="iDir">方向 0:表示用->ファイル用 1:ファイル用->表示用</param>
''' <param name="Src">元文字列</param>
''' <returns></returns>
Public Function HeaderCommentConvert(ByVal iDir As Int32, ByRef Src As String) As String
Dim Des As String = Nothing
Try
	If Not IsNothing(Src)
		Select Case iDir
		Case 0	'表示用->ファイル用(改行を\r\n)
			Des = Src.Replace(vbCrLf,"\r\n")
		Case 1	'ファイル用->表示用(\r\nを改行)
			Des = Src.Replace("\r\n",vbCrLf)
		End Select
	End If
	Return Des
Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Return Nothing
End Try
End Function
''' <summary>
''' 文字列が英数字かどうかを判定します
''' </summary>
''' <remarks>大文字・小文字を区別しません</remarks>
''' <param name="target">対象の文字列</param>
''' <returns>文字列が英数字の場合はtrue、それ以外はfalse</returns>
public Function IsAlphanumeric(ByVal target As String) As Boolean
    return new Regex("^[0-9a-zA-Z]+$").IsMatch(target)
End Function
Public Sub Test()
	Dim Vec1 As New System.windows.Vector(1,1)
	Dim Vec2 As New System.windows.Vector(6,0)
	Dim Ans As Double = System.windows.Vector.AngleBetween(Vec2, Vec1) * Math.PI / 180
	'Dim AnsAdd As System.windows.Vector = System.windows.Vector.Add(Vec2, Vec1)
	'Dim AnsAddVal As Double = AnsAdd.Length
	Dim AnsP As System.windows.Vector = Vec2 + vec1
	Debug.Print(AnsP.Length) 

	Dim AnsCos As Double = Math.Sqrt(Vec1.Length^2 + Vec2.Length^2 - 2*Vec1.Length*Vec2.Length*Math.Cos(Math.PI-Ans))' / Math.Sqrt(2)
	'Dim AnsCos As Double = Math.Sqrt(Vec1.Length^2 + Vec2.Length^2 - 2*Vec1.Length*Vec2.Length*Math.Cos(Ans))' / Math.Sqrt(2)

End Sub
''' <summary>
''' 数値入力コントロール初期化
''' </summary>
''' <param name="oNud">数値入力コントロール</param>
''' <param name="dMin">最小値</param>
''' <param name="dMax">最大値</param>
''' <param name="iDecPlace">小数点以下桁</param>
''' <param name="dIncriment">インクリメント</param>
''' <param name="dInitVal">初期値</param>
Public Sub	sbNudInitial(ByRef oNud As NumericUpDown, ByVal dMin As Double, ByVal dMax As Double, ByVal iDecPlace As int32, ByVal dIncriment As Double, ByVal dInitVal As Double)
Try
	oNud.Minimum		= dMin
	oNud.Maximum		= dMax
	oNud.DecimalPlaces	= iDecPlace
	oNud.Increment		= dIncriment
	oNud.Value			= dInitVal

Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub
''' <summary>
''' 指定されたフルパスのファイルのバージョン情報を取得する
''' </summary>
''' <param name="filePath">対象ファイルのフルパス</param>
''' <returns>バージョン文字列（取得できない場合は空文字）</returns>
Public Function GetAppVersion(filePath As String) As String
Try
    If File.Exists(filePath) Then
        Try
            ' バージョン情報を取得
            Dim verInfo As FileVersionInfo = FileVersionInfo.GetVersionInfo(filePath)
            
            ' FileVersion (ファイルバージョン) を返す
            ' ※必要に応じて ProductVersion (製品バージョン) に変更してください
            Return verInfo.FileVersion
            
            ' 【参考】フォーマットを "1.0.0.0" のように厳密に整えたい場合:
            ' Return $"{verInfo.FileMajorPart}.{verInfo.FileMinorPart}.{verInfo.FileBuildPart}.{verInfo.FilePrivatePart}"
        Catch ex As Exception
			Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
            Return Nothing
        End Try
    Else
        Return Nothing
    End If
Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Return Nothing
End Try
End Function


<Extension()>
Public Function GetStringSafe(reader As IDataReader, index As Integer) As String
    If reader.IsDBNull(index) Then Return ""
    Return reader.GetString(index)
End Function
<Extension()>
Public Function GetDoubleSafe(reader As IDataReader, index As Integer) As Double
    If reader.IsDBNull(index) Then Return 0.0
    Dim val = reader.GetValue(index)
    If TypeOf val Is Double Then Return CDbl(val)
    If IsNumeric(val) Then Return CDbl(val)
    Return 0.0
End Function
<Extension()>
Public Function GetIntSafe(reader As IDataReader, index As Integer) As Integer
    If reader.IsDBNull(index) Then Return 0
    Dim val = reader.GetValue(index)
    If TypeOf val Is Integer Then Return CInt(val)
    If IsNumeric(val) Then Return CInt(val)
    Return 0
End Function
''' <summary>
''' 安全実行ラッパー：例外が発生しても継続し、ログに残す
''' </summary>
''' <param name="sFunc">関数名</param>
''' <param name="action">実行するアクション</param>
''' <param name="context">コンテキスト情報</param>
Public Sub SafeExec(ByVal sFunc As String, action As Action, Optional context As String = "")
	Try
		action()
	Catch ex As Exception
		Try
			Call WriteLog(sFunc & " SafeExec EX in " & context & " : " & ex.Message, eLOG_LEVEL.iFATAL)
		Catch
			'最悪ここでも落ちないように無視
		End Try
	End Try
End Sub

''' <summary>
''' 安全実行ラッパー：例外が発生しても継続し、ログに残す個々で例外をキャッチして継続する、複数の Action を順次実行し、個々で例外をキャッチして継続する
''' </summary>
''' <param name="sFunc">関数名</param>
''' <param name="context">コンテキスト情報</param>
''' <param name="actions">実行するアクションの配列</param>
Public Sub SafeExecContinue(ByVal sFunc As String, ByVal context As String, ParamArray actions() As Action)
Try
	If actions Is Nothing OrElse actions.Length = 0 Then
		Return
	End If

	For idx As Integer = 0 To actions.Length - 1
		Try
			Dim act As Action = actions(idx)
			If act IsNot Nothing Then
				act()
			End If
		Catch ex As Exception
			Try
				Call WriteLog(sFunc & " SafeExecContinue[" & context & "] action[" & idx.ToString() & "] exception: " & ex.Message, eLOG_LEVEL.iFATAL)
			Catch
				'ログ失敗でも継続
			End Try
		End Try
	Next
Catch ex As Exception
	Try
		Call WriteLog("SafeExecContinue[" & context & "] outer exception: " & ex.Message, eLOG_LEVEL.iFATAL)
	Catch
	End Try
End Try
End Sub

End Module
