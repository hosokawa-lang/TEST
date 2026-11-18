Imports System.Drawing.Printing
Imports System.Threading
Imports System.IO
Imports System.Windows.Forms.DataVisualization.Charting
Imports System.Collections
Imports System.Globalization
Imports System.ComponentModel
Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.ApplicationServices
Imports Microsoft.VisualBasic.FileIO
Imports SZKTool.frmMain
Imports SZKTool.frmOptions
Imports SZKTool.RealTimeMeas
Imports SZKTool.modPublic
Imports SZKTool.DataFile
Imports SZKTool.frmAbout
Imports SZKTool.clSpec
Imports SZKTool.Filter
Imports SZKTool.DataCal
Module modFrmMain

Public Sub frmMainIni(ByVal m_sbLog As System.Text.StringBuilder)
	Try
	'初期化開始。
	Call m_sbLog.Append("SZKT Init Started:" + Now.ToString() + vbCrLf)
	'設定ファイルを読み込む。
	Call p_cts.LoadDeviceListINI()
	'Call p_cts.LoadDeviceSettingINI(p_sEquipName)				'装置パラメータ読込
	'Call p_cts.LoadTempParaINI(p_sEquipName)					'温調パラメータ読込

	Dim iVer As Integer
	Dim bRet As Boolean
	p_bValid_OfficeVer_Install = True       'Office2007以降インストール



	'Officeのバージョンを取得
''	bRet = fnGetOfficeVersition(iVer)
	'Office 2016 の場合は 16 
	'Office 2013 の場合は 15
	'Office 2007 の場合は 12
	'Office 2003 の場合は 11
	'Office XP(2002) の場合は 10
	'Office 2000 の場合は 9
	'PDF保存はOffice2007より有効
	If bRet = False OrElse iVer < 12 Then
		'画像-PDF変換アプリもない場合
		If System.IO.File.Exists(m_sImg2PdfPath) = False Then
			'PDF保存は不可の為、画像保存
			p_cts.iAutoPrintTypeValue = 0
			p_bValid_OfficeVer_Install = False
		End If
	End If

	Call m_sbLog.Append("DeviceSetting.ini loaded" + vbCrLf)
			'表示言語を設定。
	Dim culture As String
	If p_cts.iLang >= 0 Then
		If p_cts.iLang = 1 Then
			culture = "ja-JP"
		ElseIf p_cts.iLang = 2 Then
			culture = "zh-Hant"
		ElseIf p_cts.iLang = 3 Then
			culture = "ko-KR"
		ElseIf p_cts.iLang = 4 Then
			culture = "zh-Hans"
		Else
			culture = "en-US"
		End If
	Else
		'初回起動時には、PCの言語設定に応じて表示言語を設定する。
		If Thread.CurrentThread.CurrentCulture.Name = "ja-JP" Then
			p_cts.iLang = 1
			culture = "ja-JP"
		ElseIf Thread.CurrentThread.CurrentCulture.Name = "zh-Hant" Then
			p_cts.iLang = 2
			culture = "zh-Hant"
		ElseIf Thread.CurrentThread.CurrentCulture.Name = "ko-KR" Then
			p_cts.iLang = 3
			culture = "ko-KR"
		ElseIf Thread.CurrentThread.CurrentCulture.Name = "zh-Hans" Then
			p_cts.iLang = 4
			culture = "zh-Hans"
		Else
			p_cts.iLang = 0
			culture = "en-US"
		End If
	End If

	p_iLanguage = p_cts.iLang
	'リモートSQL Serverデータベース接続先を設定
	'p_iSqlSvrExpEnable	= p_cts.iSqlSvrExpEnable
	'p_eRmtSqlAuthType	= p_cts.iSqlSvrExpAuthType
	'p_sRmtSqlSvrName	= p_cts.sSqlSvrExpSvrName
	'p_sRmtSqlUserID		= p_cts.sSqlSvrExpUserName
	'p_sRmtSqlPassword	= p_cts.sSqlSvrExpPassword
	'p_sRmtSqlExpDataPath = p_cts.sSqlSvrExpDataPath

	Call m_sbLog.Append("culture string:" + culture + vbCrLf)
	Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(culture)
	Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture()


	'DB初期化
	If p_cts.DBAutoReg = 1
		If p_cts.DBSel = 0
			Dim sDBFilePAth As String = Path.Combine(SIZUKA_PATH, DBDAT_PATH, "SizukaDB.mdf")
			If Not File.Exists(sDBFilePAth)
				'SizukaDBの初期化
				Dim iRet As int32
				iRet = AttachForSizukaDBPrc()
			End if
		Else
		'SQLite選択時
			p_clDB.InitializeSizukaDB()											'SQLite DB初期化
			p_clDB.CheckSizukaDB()												'DB使用可能かチェック
		End if
	End If

Catch ex As Exception
	Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Call MakeLogFile(m_sbLog.ToString() + ex.ToString())
End Try
End Sub
''' <summary>
''' SZKAutoRegistを管理者権限で起動(SizukaDB.Bakの復元)
''' </summary>
''' <returns></returns>
Public Function AttachForSizukaDBPrc() As Int32
Try
	Dim psi As New ProcessStartInfo()
	psi.FileName = ".\SZKAutoRegist.exe"
	psi.Arguments = "/RESTORE"
	psi.UseShellExecute = True
	psi.Verb = "runas"

	Try
		Using p As Process = Process.Start(psi)
			p.WaitForExit()
			Return p.ExitCode
		End Using
	Catch ex As Exception
		WriteLog("管理者実行キャンセル: " & ex.Message)
		Return -1
	End Try
Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Return -1
End Try
End Function

'データ管理フォルダ環境生成
Public Sub	SetForderCreate()
	If p_sEquipName = ""
		p_sEquipName = "SZK999"
	End If
	If p_sEquipName.Length <> 6
		Exit Sub
	End If
	If p_sEquipName.Substring(0,3)<>"SZK"
		Exit Sub
	End If
	'フォルダ関係の検査と作成 -------------------------------------------
	'測定データ保存フォルダ生成
	If Not FileSystem.DirectoryExists(Path.Combine(SIZUKA_PATH, p_sEquipName, MESDAT_PATH)) Then
		FileSystem.CreateDirectory(Path.Combine(SIZUKA_PATH, p_sEquipName, MESDAT_PATH))
	End If
	'データファイルパス設定
	p_dtFile.InitDataPath()
	'キャリブレーション関係フォルダ生成
	InitialCalibFull()
	'基準データ保存フォルダ生成
	Dim sStdPath As String = Path.Combine(SIZUKA_PATH, p_sEquipName, STDDAT_PATH)
	If System.IO.Directory.Exists(sStdPath) = False Then
		System.IO.Directory.CreateDirectory(sStdPath)
	End If
	'SPECフォルダ生成
	Dim sSpecDir As String = Path.Combine(SIZUKA_PATH, p_sEquipName, SPEC_PATH)
    If Not My.Computer.FileSystem.DirectoryExists(sSpecDir) Then
        My.Computer.FileSystem.CreateDirectory(sSpecDir)
    End If
	'デバイスSPECフォルダ生成
	Dim sDevSpecDir As String = Path.Combine(SIZUKA_PATH, p_sEquipName, DEVSPEC_PATH)
    If Not My.Computer.FileSystem.DirectoryExists(sDevSpecDir) Then
        My.Computer.FileSystem.CreateDirectory(sDevSpecDir)
    End If
	'サンプルSPECフォルダ生成
	Dim sSamSpecDir As String = Path.Combine(SIZUKA_PATH, p_sEquipName, SAMSPEC_PATH)
    If Not My.Computer.FileSystem.DirectoryExists(sSamSpecDir) Then
        My.Computer.FileSystem.CreateDirectory(sSamSpecDir)
    End If
	'水平確認動作設定フォルダ生成
	Dim sHorizonDir As String = Path.Combine(SIZUKA_PATH, p_sEquipName, HORIZON_PATH)
    If Not My.Computer.FileSystem.DirectoryExists(sHorizonDir) Then
        My.Computer.FileSystem.CreateDirectory(sHorizonDir)
    End If
	'温度プロファイルフォルダ生成
	Dim sRtgPath As String = Path.Combine(SIZUKA_PATH, p_sEquipName, RTGSET_PATH)
	If System.IO.Directory.Exists(sRtgPath) = False Then
		System.IO.Directory.CreateDirectory(sRtgPath)
	End If
	'測定結果ローカル自動保存の保存先
    p_cts.sAutoSaveMeasDataFolder = Path.Combine(SIZUKA_PATH, p_sEquipName, MESDAT_PATH, AUTO_PATH)
	'--------------------------------------------------------------------
End Sub

''' <summary>
''' チャート表示更新
''' </summary>
''' <param name="iThinningType">0:精 1:粗</param>
''' <param name="bReSet">True:チャート再設定あり False:チャート再設定なし</param>
Public Sub ChartDispUpdate(ByRef frm As frmMain, ByVal iThinningType As Int16, ByVal bReSet As Boolean, _
							Optional ByVal hd As DataFileHeader=Nothing, _
							Optional ByVal sMethod As String=Nothing)
Dim i As Int32 = 0
Try
	If IsNothing(p_Rtm) Or IsNothing(frm.p_sSelectGItem)
		Return
	End If
	Dim stCa	As ChartArg = Nothing
	stCa.iThinningType		= iThinningType
	stCa.sSelectGItem		= frm.p_sSelectGItem
	stCa.iSelectGItem		= frm.p_iSelectGItem
	stCa.bHist				= frm.m_HistDraw
	stCa.bStd				= frm.m_StdDraw
	stCa.dYSpanPer			= p_RTM(0).p_dYDispArea
	'If IsNothing(hd.sFileName)
	'If IsNothing(If(hd.sFileName="",p_dtFile.p_head.sFileName,hd.sFileName)) 
	If IsNothing(If(hd.sFileName="",p_dtFile.p_head.sFileName,hd.sFileName)) OrElse IsNothing(hd.sFileName)
		stCa.dLimitTime			= p_RTM(0).p_dMeasTimeLimit
		stCa.sCalcMethod		= If(p_RTM(0).p_sCalcMethod="", p_dtFile.p_head.sCalcMethod, p_RTM(0).p_sCalcMethod)
		stCa.iGelTimeJudge		= p_RTM(0).p_iGelTimeJudge
		stCa.dJudgeRangeMin		= p_RTM(0).p_dJudgeRangeMin
		stCa.dJudgeRangeMax		= p_RTM(0).p_dJudgeRangeMax
		stCa.dAprxRangeMin		= p_RTM(0).p_dAprxRangeMin
		stCa.dAprxRangeMax		= p_RTM(0).p_dAprxRangeMax
		stCa.dGelTime			= p_RTM(0).p_dGelTime
		stCa.dToqueThreshold	= p_RTM(0).p_dToqueThreshold
		stCa.dFullStressADVal	= p_RTM(0).p_dFullStressADVal
		stCa.dMeltTemp			= p_RTM(0).p_dMeltTemp
		stCa.dMeltVisco			= p_RTM(0).p_dMeltVisco
		If p_RTM(0).p_strRTGSetName<>""
			Dim dMin As Double = 9999
			Dim dMax As Double = -9999
			For i=0 To p_RTM(0).p_rtgset.mdbr.GetUpperBound(0)
				If p_RTM(0).p_rtgset.mdbr(i).dHPTemp = Double.MinValue
					Exit For
				End If
				If p_RTM(0).p_rtgset.mdbr(i).dHPTemp>dMax
					dMax = p_RTM(0).p_rtgset.mdbr(i).dHPTemp
				End If
				If p_RTM(0).p_rtgset.mdbr(i).dHPTemp<=dMin
					dMin = p_RTM(0).p_rtgset.mdbr(i).dHPTemp
				End If
			Next
			stCa.dTempMax			= dMax
			stCa.dTempMin			= dMin
		Else
			stCa.dTempMax			= p_cts.MainGraphXMax(p_cts.iChartType)
			stCa.dTempMin			= p_cts.MainGraphXMin(p_cts.iChartType)
		End If

	'ElseIf Not IsNothing(p_dtFile.p_head.sFileName)
	Else
		stCa.dLimitTime			= p_dtFile.p_head.dMeasTimeLimit
		If IsNothing(sMethod)
			stCa.sCalcMethod		= p_dtFile.p_head.sCalcMethod
		Else
			stCa.sCalcMethod		= sMethod
		End If
		stCa.iGelTimeJudge		= lstGelTimeJudge.IndexOf(p_dtFile.p_head.sGelTimeJudge)
#If 0
		Dim sMinMax() As String = Nothing
		sMinMax = p_dtFile.p_head.sJudgeRange.Split("-")
		stCa.dJudgeRangeMin		= CType(Trim(sMinMax(0)), Double)
		stCa.dJudgeRangeMax		= CType(Trim(sMinMax(1)), Double)
		sMinMax = p_dtFile.p_head.sMaskTime.Split("-")
		stCa.dAprxRangeMin		= CType(Trim(sMinMax(0)), Double)
		stCa.dAprxRangeMax		= CType(Trim(sMinMax(1)), Double)
#Else
		stCa.dJudgeRangeMin		= p_clSpec.spi.dJudgeRangeMin
		stCa.dJudgeRangeMax		= p_clSpec.spi.dJudgeRangeMax
		stCa.dAprxRangeMin		= p_clSpec.spi.dEffectRangeMin
		stCa.dAprxRangeMax		= p_clSpec.spi.dEffectRangeMax
#end if
		stCa.dGelTime			= p_dtFile.p_head.dGelTime
		'stCa.dToqueThreshold	= p_dtFile.p_head.sThreshold
		stCa.dToqueThreshold	= p_clSpec.spi.dThreshold
		stCa.dFullStressADVal	= p_dtFile.p_head.sFullCalibValue
		stCa.dMeltTemp			= p_dtFile.p_head.dGelTimeTemp
		stCa.dMeltVisco			= p_dtFile.p_head.dGelTimeVisco
		If p_RTM(0).p_strRTGSetName<>""
			Dim dMin As Double = 9999
			Dim dMax As Double = -9999
			For i=0 To p_RTM(0).p_rtgset.mdbr.GetUpperBound(0)
				If p_RTM(0).p_rtgset.mdbr(i).dHPTemp = Double.MinValue
					Exit For
				End If
				If p_RTM(0).p_rtgset.mdbr(i).dHPTemp>dMax
					dMax = p_RTM(0).p_rtgset.mdbr(i).dHPTemp
				End If
				If p_RTM(0).p_rtgset.mdbr(i).dHPTemp<=dMin
					dMin = p_RTM(0).p_rtgset.mdbr(i).dHPTemp
				End If
			Next
			stCa.dTempMax			= dMax
			stCa.dTempMin			= dMin
		Else
			stCa.dTempMax			= p_cts.MainGraphXMax(p_cts.iChartType)
			stCa.dTempMin			= p_cts.MainGraphXMin(p_cts.iChartType)
		End if
	End If

	If p_cts.LTModeEnable OrElse _
		p_dtFile.p_head.sMeasureType = p_dtFile.sMeasindCode(eMesCD.LTM) OrElse _
		p_dtFile.p_head_s(0).sMeasureType = p_dtFile.sMeasindCode(eMesCD.LTM) OrElse _
		p_dtFile.p_head_s(1).sMeasureType = p_dtFile.sMeasindCode(eMesCD.LTM)
		'LTM条件で禁止項目が選択されていたら応力%へ強制変更
		If frm.rbYStress.Checked Or frm.rbZStress.Checked Or frm.rbXStress.Checked Or frm.rbZPosition.Checked
			frm.rbCorrectP.Checked	= True
			stCa.sSelectGItem		= frm.rbCorrectP.Name
			frm.p_sSelectGItem		= frm.rbCorrectP.Name
		End If
		LTRealChartDisp1( stCa, p_dtFile, frm.Chart1, frm.clExtra, p_cts, bReSet )
	Else
		RealChartDisp1( stCa, p_dtFile, frm.Chart1, frm.clExtra, p_cts, bReSet )
	End If

Catch ex As Exception
    Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub
'本体ステータス状態表示
Public Sub MachineStateDisp()
	frmMain.Lbl4.BackColor = IIf(p_RTM(0).p_mltcp.isStatMeasuring, Color.Cyan, SystemColors.Control)
	frmMain.Lbl5.BackColor = IIf(p_RTM(0).p_mltcp.isStatPreMeasure, Color.Cyan, SystemColors.Control)
	frmMain.Lbl6.BackColor = IIf(p_RTM(0).p_mltcp.isStatMeasuringTempo, Color.Cyan, SystemColors.Control)
	frmMain.Lbl7.BackColor = IIf(p_RTM(0).p_mltcp.isStatZeroAdjusting, Color.Cyan, SystemColors.Control)
	frmMain.Lbl8.BackColor = IIf(p_RTM(0).p_mltcp.isStatHeaterOn, Color.Cyan, SystemColors.Control)
	frmMain.lbl52.BackColor = IIf(p_RTM(0).p_mltcp.isStatHeaterOn2, Color.Cyan, SystemColors.Control)
	frmMain.Lbl9.BackColor = IIf(p_RTM(0).p_mltcp.isStatHeatPanelDetectComp, Color.Cyan, SystemColors.Control)

	frmMain.Lbl50.BackColor = IIf(p_RTM(0).p_mltcp.isStatXYAction, Color.Cyan, SystemColors.Control)
	frmMain.Lbl51.BackColor = IIf(p_RTM(0).p_mltcp.isStatGoingUpDown, Color.Cyan, SystemColors.Control)


End Sub
'Officeのバージョンを取得する。
Private Function fnGetOfficeVersition(ByRef iVer As Integer) As Boolean
Dim oApp As Object = Nothing
Dim Ver As Decimal

Try
	oApp = CreateObject("Word.Application")

	'OfficeがインストールされWordのバージョンが取得できる場合
	If oApp Is Nothing = False Then
		Ver = CType(oApp.Version.ToString, Decimal)
		iVer = Ver
	Else
		'Officeがインストールされていない場合
		Return False
	End If
	Return True

Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Return False
Finally
	If Not oApp Is Nothing Then
		Try
			oApp.Quit()
		Finally
			System.Runtime.InteropServices.Marshal.ReleaseComObject(oApp)
		End Try
	End If
End Try
End Function
'''<summary>
'''積算値をログ出力する。
'''</summary>
Public Sub WriteIntegLog()
	Dim ts As TimeSpan
	Dim sTemp As String
	WriteLog("--------------------------------------------------", eLOG_LEVEL.iINFO)
	WriteLog("積算時間  リセット日", eLOG_LEVEL.iINFO)

	ts = TimeSpan.FromSeconds(CDbl(p_RTM(0).p_ulIntegTimePowerOn))
	sTemp = String.Format("{0}:{1:mm\:ss}", ts.Days * 24 + ts.Hours, ts).PadLeft(12, " ")
	WriteLog("   本体電源ON        ==>  " & sTemp & "  " & p_RTM(0).p_sIntegResetDatePowerOn, eLOG_LEVEL.iINFO)

	ts = TimeSpan.FromSeconds(CDbl(p_RTM(0).p_ulIntegTimeHeaterOn))
	sTemp = String.Format("{0}:{1:mm\:ss}", ts.Days * 24 + ts.Hours, ts).PadLeft(12, " ")
	WriteLog("   熱板ヒータON      ==>  " & sTemp & "  " & p_RTM(0).p_sIntegResetDateHeaterOn, eLOG_LEVEL.iINFO)

	ts = TimeSpan.FromSeconds(CDbl(p_RTM(0).p_ulIntegTimeHeater2On))
	sTemp = String.Format("{0}:{1:mm\:ss}", ts.Days * 24 + ts.Hours, ts).PadLeft(12, " ")
	WriteLog("   プローブヒータON  ==>  " & sTemp & "  " & p_RTM(0).p_sIntegResetDateHeater2On, eLOG_LEVEL.iINFO)

	ts = TimeSpan.FromSeconds(CDbl(p_RTM(0).p_ulIntegTimeXOn))
	sTemp = String.Format("{0}:{1:mm\:ss}", ts.Days * 24 + ts.Hours, ts).PadLeft(12, " ")
	WriteLog("   X軸動作　　　　  ==>  " & sTemp & "  " & p_RTM(0).p_sIntegResetDateXOn, eLOG_LEVEL.iINFO)

	ts = TimeSpan.FromSeconds(CDbl(p_RTM(0).p_ulIntegTimeYOn))
	sTemp = String.Format("{0}:{1:mm\:ss}", ts.Days * 24 + ts.Hours, ts).PadLeft(12, " ")
	WriteLog("   Y軸動作　　　　  ==>  " & sTemp & "  " & p_RTM(0).p_sIntegResetDateYOn, eLOG_LEVEL.iINFO)

	ts = TimeSpan.FromSeconds(CDbl(p_RTM(0).p_ulIntegTimeYOn))
	sTemp = String.Format("{0}:{1:mm\:ss}", ts.Days * 24 + ts.Hours, ts).PadLeft(12, " ")
	WriteLog("   Z軸動作　　　　  ==>  " & sTemp & "  " & p_RTM(0).p_sIntegResetDateZOn, eLOG_LEVEL.iINFO)
	WriteLog("--------------------------------------------------", eLOG_LEVEL.iINFO)
End Sub

'''<summary> ボタンに通信先機器の状態を表示する。 </summary>
'''<param name="btn">表示先のボタン。</param>
'''<param name="iFlag">機器状態のフラグ変数。0:OFF, 1:ON, その他:未確認</param>
'''<param name="iRdy">RDY表示 0:OFF, 1:ON, その他:未確認</param>
Public Sub sbDispStateToButton(ByRef btn As Button, ByVal iFlag As Integer, Optional ByVal iRdy As Integer = -1)
Try
	Dim s As String = Nothing
	'フラグの判定
	Select Case iFlag
	Case 0      'OFF
		If btn.BackColor <> Color.LightBlue Then
			btn.BackColor = Color.LightBlue
		End If
	Case 1      'ON
		'レディ判定
		If iRdy = 1 Or iRdy = -1 Then
			'レディON
			If btn.BackColor <> Color.DarkOrange Then
				btn.BackColor = Color.DarkOrange
			End If
		Else 'レディOFF(点滅)
			'0.5秒間隔で点滅させる
			If Fix((Now.Millisecond / 1000) + 0.5) >= 1 Then
				btn.BackColor = Color.DarkOrange
			Else
				btn.BackColor = Color.LightBlue
			End If
		End If
	Case Else   '未習得
		If btn.BackColor <> Color.LightGray Then
			btn.BackColor = Color.LightGray
		End If
	End Select
	Select Case btn.Name
	Case "btnTemp"
		s = p_sbtnTempText
	Case "btnZero"
		s = p_sbtnZeroText
	Case "btnMeasure"
		s = p_sbtnMeasText
	'Case "btnOrigin"
	'	s = p_sbtnOrgText
	End Select
	'レディ判定
	If iRdy <> -1 Then
		'レディON
		If iRdy = 1 Then
			'レディ状態を表示
			btn.Text = s & " RDY"
			btn.BackColor = Color.DarkOrange
		Else
			If btn.Text <> s Then
				btn.Text = s
			End If
		End If
	Else
		If btn.Text <> s Then
			btn.Text = s
		End If
	End If

Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub
''' <summary>
''' 測定後メッセージ
''' </summary>
''' <param name="iMode">0：通常測定 1:フルキャリブレーション(自動剥し) 2:ゼロキャリブレーション 3:フローカーブ 4:フルキャリブレーション(手動剥し)</param>
''' <returns>0:Yes 1:No</returns>
Public Function AfterMeasureOperationGuideMsgBox(ByRef frm As frmMain, ByVal iMode As Int16) As Int32
Dim fMsg As frmMSG
Dim iRet As Int32 = -9
Dim sMsg As String = Nothing
Dim iDup As int16 = 0
Dim sBtnTxt() As String = Nothing
Try
	Select Case iMode
	Case 0,  3    '測定,フローカーブ
		Do While iRet = -9
			Array.Resize(sBtnTxt, 2)
			sBtnTxt(0) = frm.p_sMes(efrmMainSTR.iOK).Substring(0, frm.p_sMes(efrmMainSTR.iOK).Length-4)
			sBtnTxt(1) = frm.p_sMes(efrmMainSTR.iCANCEL).Substring(0, frm.p_sMes(efrmMainSTR.iCANCEL).Length-4)

			sMsg = frm.p_sMes(If(iDup=0, efrmMainSTR.iDIAG_GEL_END, efrmMainSTR.iDIAG_END_LOCK))
			fMsg = New frmMSG(sMsg, If(iDup=0, {sBtnTxt(0)}, {sBtnTxt(0), sBtnTxt(1)}), frm.m_sGuideMsgTitle, frm, iDup)
			fMsg.lblMsg.Font = New Font(BASE_FONT, 12)
			fMsg.Width = 400
			fMsg.StartPosition = FormStartPosition.CenterScreen
			fMsg.ShowDialog()
			Select Case fMsg.p_iRes
			Case 999	'装置エラー発生
				Return fMsg.p_iRes
			Case 1	'OK
				If p_cts.DoublePBUnlockConfirm
					If iDup > 0
						If p_RTM(0).p_mltcp.isILDisable					'I/L無効?
							iRet = 0
						End If
						If p_RTM(0).p_mltcp.isILState Then            '扉閉？
							iRet = 0
						End If
					End If
					iDup += 1 
				Else
					If p_RTM(0).p_mltcp.isILDisable					'I/L無効?
						iRet = 0
					End If
					If p_RTM(0).p_mltcp.isILState Then            '扉閉？
						iRet = 0
					End If
				End If
			Case 2	'キャンセル

			End Select
			fMsg.Dispose()
		Loop
#If 0
	Case  1    'フルキャリブレーション(自動剥し)
		Do While iRet = -9
			sMsg = frm.p_sMes(efrmMainSTR.iDIAG_FCLB_END)
			fMsg = New frmMSG(sMsg, {frm.p_sMes(efrmMainSTR.iYES),frm.p_sMes(efrmMainSTR.iNO)}, frm.m_sGuideMsgTitle, frm)
			fMsg.lblMsg.Font = New Font(BASE_FONT, 12)
			fMsg.Width = 400
			fMsg.StartPosition = FormStartPosition.CenterScreen
			fMsg.ShowDialog()
			Select Case fMsg.p_iRes
			Case 999	'装置エラー発生
				Return fMsg.p_iRes
			Case -1, 2
				iRet = fMsg.p_iRes
			Case Else
				If p_RTM(0).p_mltcp.isILDisable					'I/L無効?
					iRet = 0
				End If
				If p_RTM(0).p_mltcp.isILState Then				'扉閉？
					iRet = 0
				End If
			End Select
			fMsg.Dispose()
		Loop
#end if
#If 0
	Case  4    'フルキャリブレーション(手動はがし)
		Do While iRet = -9
			sMsg = frm.p_sMes(efrmMainSTR.iDIAG_FCLB_END1)
			fMsg = New frmMSG(sMsg, {frm.p_sMes(efrmMainSTR.iOK),frm.p_sMes(efrmMainSTR.iCANCEL)}, frm.m_sGuideMsgTitle, frm)
			fMsg.lblMsg.Font = New Font(BASE_FONT, 10)
			fMsg.Width = 400
			fMsg.StartPosition = FormStartPosition.CenterScreen
			fMsg.ShowDialog()
			Select Case fMsg.p_iRes
			Case 999	'装置エラー発生
				Return fMsg.p_iRes
			Case -1, 2
				iRet = fMsg.p_iRes
			Case Else
				If p_RTM(0).p_mltcp.isILDisable					'I/L無効?
					iRet = 0
				End If
				If p_RTM(0).p_mltcp.isILState Then				'扉閉？
					iRet = 0
				End If
			End Select
			fMsg.Dispose()
		Loop
#end if
	Case  5    'フルキャリブレーション(ロック外し下降 OKのみ)
		Do While iRet = -9
			Array.Resize(sBtnTxt, 2)
			sBtnTxt(0) = frm.p_sMes(efrmMainSTR.iOK).Substring(0, frm.p_sMes(efrmMainSTR.iOK).Length-4)
			sBtnTxt(1) = frm.p_sMes(efrmMainSTR.iCANCEL).Substring(0, frm.p_sMes(efrmMainSTR.iCANCEL).Length-4)

			sMsg = frm.p_sMes(If(iDup=0, efrmMainSTR.iDIAG_FCLB_END1, efrmMainSTR.iDIAG_END_LOCK))
			fMsg = New frmMSG(sMsg, If(iDup=0, {sBtnTxt(0)}, {sBtnTxt(0), sBtnTxt(1)}), frm.m_sGuideMsgTitle, frm, iDup)
			fMsg.lblMsg.Font = New Font(BASE_FONT, 10)
			fMsg.Width = 400
			fMsg.StartPosition = FormStartPosition.CenterScreen
			fMsg.ShowDialog()
			Select Case fMsg.p_iRes
			Case 999	'装置エラー発生
				Return fMsg.p_iRes
			Case -1, 2
				iRet = fMsg.p_iRes
			Case 1
				If p_cts.DoublePBUnlockConfirm
					If iDup > 0
						If p_RTM(0).p_mltcp.isILDisable					'I/L無効?
							iRet = 0
						End If
						If p_RTM(0).p_mltcp.isILState Then            '扉閉？
							iRet = 0
						End If
					End If
					iDup += 1 
				Else
					If p_RTM(0).p_mltcp.isILDisable					'I/L無効?
						iRet = 0
					End If
					If p_RTM(0).p_mltcp.isILState Then				'扉閉？
						iRet = 0
					End If
				End If
			End Select
			fMsg.Dispose()
		Loop
	Case  6    'フルキャリブレーション(剥し処理 YesNo)
		sMsg = frm.p_sMes(efrmMainSTR.iDIAG_FCLB_END2)
		fMsg = New frmMSG(sMsg, {frm.p_sMes(efrmMainSTR.iYES),frm.p_sMes(efrmMainSTR.iNo)}, frm.m_sGuideMsgTitle, frm)
		fMsg.lblMsg.Font = New Font(BASE_FONT, 10)
		fMsg.Width = 400
		fMsg.StartPosition = FormStartPosition.CenterScreen
		fMsg.ShowDialog()
		Select Case fMsg.p_iRes
		Case 999	'装置エラー発生
			Return fMsg.p_iRes
		Case -1, 2
			iRet = fMsg.p_iRes
		Case Else
			If fMsg.p_iRes = 1
				iRet = 0
			End if
		End Select
		fMsg.Dispose()
	Case 2      'ゼロキャリブレーション
		Do While iRet = -9
			sMsg = frm.p_sMes(efrmMainSTR.iDIAG_ZCLB_END)
			fMsg = New frmMSG(sMsg, {"OK"}, frm.m_sGuideMsgTitle, frm)
			fMsg.lblMsg.Font = New Font(BASE_FONT, 12)
			fMsg.Width = 400
			fMsg.StartPosition = FormStartPosition.CenterScreen
			fMsg.ShowDialog()
			Select Case fMsg.p_iRes
			Case 999	'装置エラー発生
				Return fMsg.p_iRes
			Case Else
				If p_RTM(0).p_mltcp.isILDisable					'I/L無効?
					iRet = 0
				End If
				If p_RTM(0).p_mltcp.isILState Then				'扉閉？
					iRet = 0
				End If
			End Select
			fMsg.Dispose()
		Loop
	End Select

Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
Return iRet
End Function
Public Function PreMesureOperationalGuideMsgBox(ByRef frm As frmMain, ByVal iMode As Int16)
Dim fMsg As frmMSG
Dim iRet As Int32
Dim sMsg As String = Nothing
Try
	Select Case iMode
	Case 0
		sMsg = frm.p_sMes(efrmMainSTR.iNO_CALIB_DATA)
	End Select
	fMsg = New frmMSG(sMsg, {"OK"}, frm.m_sGuideMsgTitle, frm)
	fMsg.lblMsg.Font = New Font(BASE_FONT, 12)
	fMsg.Width = 400
	fMsg.StartPosition = FormStartPosition.CenterScreen
	fMsg.ShowDialog()

	If fMsg.p_iRes = 2 Then     '[×]か[いいえ]を選択
		iRet = 1
	Else
		iRet = 0
	End If
	fMsg.Dispose()

Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
Return iRet
End Function
'測定条件の100％応力較正値をセット(較正設定ファイルを読込み設定)
Public Function SetFullStress(ByVal dAmp As Double, ByVal dPeriod As Double, ByVal iStressSensMode As int16) As int32
Dim dData			As Double
Dim dAttenuation	As Double
Dim dAlpha			As Double
Dim dPxPhase		As Double
Dim dPyPhase		As Double
Dim dPzPhase		As Double
Dim iRet			As Int32  = 0
Try
	'フルキャリブレーション結果を得る
	Dim frmC As New frmCalibFull(0)
	frmC.sbFullStressCondRead(0, p_FClbX, p_FClbC, p_FClbP)  'ファイル読込み構造体配列へセット
	''frmC.sbFullStressRALRead(2) 'ファイル読込み構造体配列へセット
	''frmC.sbFullStressRALRead(3) 'ファイル読込み構造体配列へセット
	''frmC.sbFullStressRALRead(4) 'ファイル読込み構造体配列へセット
	''frmC.sbFullStressRALRead(5) 'ファイル読込み構造体配列へセット
	frmC.Dispose()
	If FullStressCalibValueGet(0, p_ZClbX, p_ZClbC, p_ZClbP, p_FClbX, p_FClbC, p_FClbP, dData, dAttenuation, dAlpha, dPxPhase, dPyPhase, dPzPhase, iStressSensMode, dAmp, dPeriod) = 0 Then
		p_RTM(0).p_dFullStressADVal			= dData
		p_RTM(0).p_dFullStressAttenuation	= dAttenuation
		'p_RTM(0).p_dFullStressAlpha			= dAlpha
		p_RTM(0).p_dFullStressPxPhase		= dPxPhase
		p_RTM(0).p_dFullStressPyPhase		= dPyPhase
		p_RTM(0).p_dFullStressPzPhase		= dPzPhase
	Else
		p_RTM(0).p_dFullStressADVal			= -1
		'p_RTM(0).p_dFullStressAlpha			= -1
		p_RTM(0).p_dFullStressPxPhase		= -1
		p_RTM(0).p_dFullStressPyPhase		= -1
		p_RTM(0).p_dFullStressPzPhase		= -1
		iRet = -2
	End If

	'ゼロキャリブレーション結果を得る
	Dim frmZ As New frmCalibFull(1)
	frmZ.sbFullStressCondRead(1, p_ZClbX, p_ZClbC, p_ZClbP)  'ファイル読込み構造体配列へセット
	''frmZ.sbFullStressRALRead(2) 'ファイル読込み構造体配列へセット
	''frmZ.sbFullStressRALRead(3) 'ファイル読込み構造体配列へセット
	''frmZ.sbFullStressRALRead(4) 'ファイル読込み構造体配列へセット
	''frmZ.sbFullStressRALRead(5) 'ファイル読込み構造体配列へセット
	''Dim i As int32
	''For i=0 To p_ZClbP.GetUpperBound(0)
	''	Debug.Print(p_ZClbP(i).iFullVal & "," & p_ZClbP(i).dAmplitudeZ & "," & i)
	''Next
	frmZ.Dispose()
	If FullStressCalibValueGet(2, p_ZClbX, p_ZClbC, p_ZClbP, p_FClbX, p_FClbC, p_FClbP, dData, dAttenuation, dAlpha, dPxPhase, dPyPhase, dPzPhase, iStressSensMode, dAmp, dPeriod) = 0 Then
		p_RTM(0).p_dFullStressZero = dData
		p_RTM(0).p_dZeroStressPxPhase = dPxPhase
		p_RTM(0).p_dZeroStressPyPhase = dPyPhase
		p_RTM(0).p_dZeroStressPzPhase = dPzPhase
	Else
		p_RTM(0).p_dFullStressZero = -1
		p_RTM(0).p_dZeroStressPxPhase = -1
		p_RTM(0).p_dZeroStressPyPhase = -1
		p_RTM(0).p_dZeroStressPZPhase = -1
		iRet = -3
	End If
	Return iRet
Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Return -1
End Try
End Function
'測定開始・停止ボタンのキャプションを状態に合わせて更新
Public Sub sbMeasureBottonStateDisp(ByRef frm As frmMain)
	If p_RTM(0).p_iConnectState = CONNECT_ST.CONNECTING Then        'ONLINE
		If p_RTM(0).p_MeasureState = RealTimeMeas.eMeasStat.NOT_START Then           '未測定
			'開始
			frm.btnMeasure.Text = frm.p_sMes(If(p_cts.LTModeEnable, efrmMainSTR.BUTTON_MEASLT, efrmMainSTR.BUTTON_MEAS))
			frm.btnMeasure.Image = My.Resources.PlayHS
		Else
			'停止
			frm.btnMeasure.Text = frm.p_sMes(If(p_cts.LTModeEnable, efrmMainSTR.BUTTON_STOPLT, efrmMainSTR.BUTTON_STOP))
			frm.btnMeasure.Image = My.Resources.stopbtn
		End If
	Else                                                        'OFFLINE
		'開始
		frm.btnMeasure.Text = frm.p_sMes(If(p_cts.LTModeEnable, efrmMainSTR.BUTTON_MEASLT, efrmMainSTR.BUTTON_MEAS))
		frm.btnMeasure.Image = My.Resources.PlayHS
	End If
End Sub


''' <summary>
''' 測定値領域の再計算処理
''' </summary>
Public Sub	ReCalclationMainProc(ByRef frm As frmMain)
Try
	If IsNothing(p_dtFile.iOxBuf)
		Exit Sub
	End If
#If 0
	modFrmMain.ReCalclationProc(frm, 1)     '再計算処理 ファイル上書き保存
#else
	modFrmMain.ReCalclationProc(frm, 2)     '再計算処理 ファイル上書き無し
#end if
	UnderStatusLabelDisp( frm.ToolStripStatusLabel1, frm.p_sMes(efrmMainSTR.iRECALC_FILE) & p_dtFile.GetDataFileName)				'出力ファイル名
	Select Case p_clSpec.spi.iGelTimeMethod
	Case eCalcMethod.THR, eCalcMethod.SLOPE
	'THR,スロープ
		frm.rbCorrect.Checked = True                                                                            '補正データ画面の表示
		frm.p_sSelectGItem	= frm.rbCorrect.Name
		frm.p_iSelectGItem	= Array.IndexOf(sGraphSelectionArry, frm.p_sSelectGItem)
	Case eCalcMethod.EXTRA
		'外挿法
		'接線調整する準備
		frm.clExtra.ExtraPrePareProc(0, frm.Chart1, p_dtFile, frm.nudStartTim.Value, frm.nudMidTim.Value, frm.nudEndTim.Value)
		frm.clExtra.GelTimeCrossLineDisp(frm.Chart1)        '接線表示とゲルタイム計算
		Select Case frm.cboGelTSel.SelectedIndex
		Case 0
			p_RTM(0).p_dGelTime = frm.clExtra.LowerGelT
		Case 1
			p_RTM(0).p_dGelTime = frm.clExtra.MiddlGelT
		Case 2
			p_RTM(0).p_dGelTime = frm.clExtra.UpperGelT
		End Select

	Case eCalcMethod.PHASE
	'位相差法
		frm.rbPosPhase.Checked = True
		frm.p_sSelectGItem	= frm.rbPosPhase.Name
		frm.p_iSelectGItem	= Array.IndexOf(sGraphSelectionArry, frm.p_sSelectGItem)
	Case eCalcMethod.MELT
	'溶融粘度法
		frm.rbViscosity.Checked = True
		frm.p_sSelectGItem = frm.rbViscosity.Name
		frm.p_iSelectGItem	= Array.IndexOf(sGraphSelectionArry, frm.p_sSelectGItem)
	End Select
	frm.RealTrendUpdate = False
	p_cts.iChartType = If(frm.tsmiXAxisTemp.Checked, 1, 0)
	If p_iScrlStart = 0
		ChartDispUpdate(frm, 0, frm.RealTrendUpdate)		'チャート表示更新
	Else
		ChartDispUpdate(frm, 1, frm.RealTrendUpdate)		'チャート表示更新
	End If

	'p_DtFile.p_Head.iState						= 0							'測定データ状態 クリア



Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub
''' <summary>
''' 測定値領域の再計算処理(ヘッダー情報から)
''' </summary>
Public Sub	ReCalclationMainProcFromHeader(ByRef frm As frmMain, ByVal hd As DataFileHeader)
Try
	'ReCalclationProc(2, hd)     '再計算処理 元ファイルへは書き込まない
	'UnderStatusLabelDisp( ToolStripStatusLabel1, p_sMes(efrmMainSTR.iRECALC_FILE) & p_dtFile.GetDataFileName)				'出力ファイル名
	Select Case hd.sCalcMethod
	Case lstCalcMethod(eCalcMethod.THR), lstCalcMethod(eCalcMethod.SLOPE)
	'THR,スロープ
		frm.rbCorrect.Checked = True                                                                            '補正データ画面の表示
		frm.p_sSelectGItem	= frm.rbCorrect.Name
		frm.p_iSelectGItem	= Array.IndexOf(sGraphSelectionArry, frm.p_sSelectGItem)
	Case lstCalcMethod(eCalcMethod.EXTRA)
		'外挿法
		'接線調整する準備

		frm.clExtra.ExtraPrePareProc(1, frm.Chart1, p_dtFile, frm.nudStartTim.Value, frm.nudMidTim.Value, frm.nudEndTim.Value)
		frm.clExtra.GelTimeCrossLineDisp(frm.Chart1)        '接線表示とゲルタイム計算
		Select Case frm.cboGelTSel.SelectedIndex
		Case 0
			p_RTM(0).p_dGelTime = frm.clExtra.LowerGelT
		Case 1
			p_RTM(0).p_dGelTime = frm.clExtra.MiddlGelT
		Case 2
			p_RTM(0).p_dGelTime = frm.clExtra.UpperGelT
		End Select

	Case lstCalcMethod(eCalcMethod.PHASE)
	'位相差法
		frm.rbPosPhase.Checked = True
		frm.p_sSelectGItem	= frm.rbPosPhase.Name
		frm.p_iSelectGItem	= Array.IndexOf(sGraphSelectionArry, frm.p_sSelectGItem)
	Case lstCalcMethod(eCalcMethod.MELT)
	'溶融粘度法
		frm.rbViscosity.Checked = True
		frm.p_sSelectGItem	= frm.rbViscosity.Name
		frm.p_iSelectGItem	= Array.IndexOf(sGraphSelectionArry, frm.p_sSelectGItem)
	End Select
	frm.RealTrendUpdate = False
	p_cts.iChartType = If(frm.tsmiXAxisTemp.Checked, 1, 0)
	If p_iScrlStart = 0
		ChartDispUpdate(frm, 0, frm.RealTrendUpdate, hd)		'チャート表示更新
	Else
		ChartDispUpdate(frm, 1, frm.RealTrendUpdate, hd)		'チャート表示更新
	End If



Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub
''' <summary>
''' 再計算処理 (メモリ内に測定結果が残っているとき)
'''   インポート時はヘッダ情報を元に再計算しデータファイルへはアクセス無し
'''   上書き許可アイテム
'''		計算方法
'''		判定値
'''		有効データ範囲
'''		規格判定範囲
'''		プローブ直径
'''		サンプル名
'''		ロットNo
'''		測定者
''' </summary>
''' <param name="iUpdate">データファイルを 0:新規に作成 1:上書き 2:更新しない</param>
''' <param name="hd">ヘッダ情報(オプション)</param>
Public Sub ReCalclationProc(ByRef frm As frmMain,  ByVal iUpdate As Int16, optional hd As DataFileHeader = Nothing)
Try
	UnderStatusLabelDisp( frm.ToolStripStatusLabel1, frm.p_sMes(efrmMainSTR.iRECALC_PROC))							'再計算中 表示
	frm.StatusStrip1.Refresh
	frm.Cursor = Cursors.WaitCursor
	frm.lblCalclating.BackColor = frm.Chart1.ChartAreas(0).BackColor
	frm.lblCalclating.Visible = True
	frm.lblCalclating.Refresh


	Select Case IsNothing(hd.sDateTime)
	Case True
		'測定直後またはの再計算処理 ----------------------------------------------------------------------------------
		'SPEC入力をSPEC構造体へ一括セット
		p_clSpec.dgvToSpi(eSpec.Device, frm.dgvDevSpec, p_clSpec.spi)							'現在のDGV値をSPI構造体へセット
		p_clSpec.dgvToSpi(eSpec.Sample, frm.dgvSamSpec, p_clSpec.spi)							'現在のDGV値をSPI構造体へセット

		'スペックDGVをリンクエリアへセット
		frm.sbSpecToLinkArea()

		'測定条件の100％応力較正値をセット(較正設定ファイルを読込み設定)
		Dim dAmp As Double		= p_clSpec.spi.dXAmplitude
		Dim dPeriod As Double	= p_clSpec.spi.dXPeriod
		'キャリブレーション値を得る
		Dim iRet As Int32
		iRet = modFrmMain.SetFullStress(dAmp, dPeriod, p_clSpec.spi.iStressSensMode)
		if iRet < 0
			modfrmMain.PreMesureOperationalGuideMsgBox(frm, 0)
			Return
		End If
		'フィルタ動作設定をセット
		frm.SetFilterSetting()
		'測定者情報その他の格納
		With p_RTM(0)
#If 1
			.p_sMeasPlace		= p_cts.strAutoPrintMesurePlaceValue            '測定場所
			.p_sMeasurer		= p_cts.strAutoPrintOperatorValue               '測定者
			.p_sResinName		= p_clSpec.spi.sSampleName						'樹脂名
			.p_sLot				= p_clspec.spi.sLot								'LOT名
			.p_dDiameter		= p_clspec.spi.dProbDiameter				    'プローブ直径
			.p_sCalcMethod		= lstCalcMethod(p_clspec.spi.iGelTimeMethod)	'ゲルタイム計算方法
			'.p_sEquipName		= frm.cboEquipName.Text							'本体名
			'.p_iZPosition		= .p_mltcp.ZeroPos_Z							'熱板位置(um)
			'.p_dWeightCoef		= p_cts.WeightCoef()							'dig->g換算係数
			'熱板温度許容範囲
			'.p_dTempStubRange	= p_clspec.spi.dHPTempOkRange
			'.p_dTempStubRange2	= p_clspec.spi.dPBTempOkRange

			'ゲルタイム判定範囲
			.p_dJudgeRangeMin	= p_clspec.spi.dJudgeRangeMin
			.p_dJudgeRangeMax	= p_clspec.spi.dJudgeRangeMax

			'マスク範囲
			.p_dAprxRangeMin	= p_clspec.spi.dEffectRangeMin
			.p_dAprxRangeMax	= p_clspec.spi.dEffectRangeMax

			'.p_sDeviceSpecName	= p_clSpec.spi.sDeviceSpecName					'デバイスSPEC名
			'.p_sSampleSpecName	= p_clSpec.spi.sSampleSpecName					'サンプルSPEC名

			'.p_sShape			= p_clSpec.sShape(p_clSpec.spi.iShape)			'形状
		#If 0
			Select Case p_clSpec.spi.iShape
			Case eShape.Sheet
				.p_sThickness	= p_clSpec.spi.dThickness.ToString("F3")		'厚み(mm)
				.p_sStacks		= p_clSpec.spi.dStack.ToString("F0")			'積層枚数
			Case eShape.Powder
				.p_sSpecGravity	= p_clSpec.spi.dThickness.ToString("F3")		'密度
				.p_sWeight		= p_clSpec.spi.dStack.ToString("F3")			'重量
			Case eShape.Liquid
				.p_sSpecGravity	= p_clSpec.spi.dThickness.ToString("F3")		'密度
				.p_sVolume		= p_clSpec.spi.dStack.ToString("F3")			'体積
			End Select
			.p_iHPPosCont		= p_clSpec.spi.iHPPosCont						'熱板位置制御方法 0,1,2
			.p_dMeasureStress	= p_clSpec.spi.dMeasureStress					'測定時圧力設定(kPa)
		#end if

#end if
		End With
		'#######################################
		Dim cp As stCalcPara = Nothing
		cp = p_cp											'直近の計算パラメータを設定
		cp.iMeas			= eHoseiMeasMode.GelTime
		cp.iMode			= eHoseiCalcMode.Batch
		cp.cts				= p_cts
		cp.clDt				= p_dtFile
		cp.iSampleStart		= 0
		cp.iSampleNum		= p_dtFile.iHosei.Length
		'フィルタ動作設定をセット
		Dim Fltset As stFilterInfo = Nothing
		Fltset.iEnable			= p_cts.iFilterEnable
		Fltset.sKind			= p_cts.sFilterKind	        'フィルタ種類 FIR/IIR
		Fltset.dFIRCutOffFreq	= p_cts.dFIRCutOffFreq		'FIR カットオフ周波数
		Fltset.iFIRFilLen		= p_cts.iFilterLen		    'FIR パラメータ長
		Fltset.sFIRWinKind		= p_cts.sFilterWinKind		'FIR 窓関数種
		Fltset.dIIRCutOffFreq	= p_cts.dIIRCutOffFreq		'IIR カットオフ周波数
		Fltset.iIIRDeg			= p_cts.iIIRDeg			    'IIR フィルタ次数
		Fltset.dFIRSmplFreq		= MEAS_SAMPLE_FREQ			'サンプル周波数
		cp.fltSet				= Fltset
		'cp.dXYWait			= p_mltcp.MesPreWait(1) + p_mltcp.MeasWaitTime(1)
'変更できないパラメータ
'---------------------------------------------------------------------
		'cp.dPeriod		= p_clSpec.spi.dXPeriod					'変更できない
		'cp.dAmplitude		= p_clSpec.spi.dXAmplitude			'変更できない
		'cp.dXYWait			= p_clSpec.spi.dSampleInputWait + p_clSpec.spi.dInputToHPUpWait + p_clSpec.spi.dHPUpToXYActWait
		'cp.dZeroStress		= p_RTM(0).p_dFullStressZero
		'cp.dAttenuation		= p_RTM(0).p_dFullStressAttenuation
		'cp.dDiameter		= p_RTM(0).p_dDiameter
		'cp.dMeasGap			= p_RTM(0).p_mltcp.GapPreset(1)
		'cp.dFullStress		= p_RTM(0).p_dFullStressADVal
		'cp.dPxPhase			= p_RTM(0).p_dFullStressPxPhase
		'cp.dPyPhase			= p_RTM(0).p_dFullStressPyPhase
		'cp.dPzPhase			= p_RTM(0).p_dFullStressPzPhase
		'cp.iZeroPos			= p_RTM(0).p_iZPosition
		'cp.iHPPosControl	= p_clSpec.spi.iHPPosCont								'熱板位置制御方法 0,1,2
		'cp.dMeasureStress	= p_clSpec.spi.dMeasureStress
'---------------------------------------------------------------------
		cp.dDiffInterval	= p_clSpec.spi.dDiffInterval
		cp.iCalcType		= p_clSpec.spi.iGelTimeMethod
		cp.dXYWeightCoef	= p_dNConv
		p_RTM(0).ReCalcProc(iUpdate, cp)			'再計算処理

		'#######################################
		frm.Chart1.Annotations.Clear()                              '注釈を全て削除

		'計算法毎のチャート表示後の追加表示
		Select Case p_clSpec.spi.iGelTimeMethod
		Case eCalcMethod.THR,eCalcMethod.SLOPE,eCalcMethod.PHASE,eCalcMethod.MELT

		Case eCalcMethod.EXTRA
			'外挿法前処理
			frm.clExtra.ExtraPrePareProc(0, frm.Chart1, p_dtFile, frm.nudStartTim.Value, frm.nudMidTim.Value, frm.nudEndTim.Value)
			'外挿法時はゲルタイム再計算
			frm.clExtra.GelTimeCrossLineDisp(frm.Chart1)
		End Select
		GelTimeLabelDispUpdate(frm)															'ゲルタイム値ラベル表示更新




	Case Else
	'ファイルインポート時、データヘッダを元に再計算 ----------------------------------------------------------------------------------------
		
		'測定者情報その他の格納
		With p_RTM(0)
			.p_sMeasPlace		= p_cts.strAutoPrintMesurePlaceValue                '測定場所
			.p_sMeasurer		= hd.sMeasurer                                      '測定者
			.p_sResinName		= hd.sSampleName 				                    'サンプル名
			.p_sLot				= hd.sLot					                        'LOT名
			.p_dDiameter		= CType(hd.dPBDiameter, Double)		                'プローブ直径
			.p_sCalcMethod		= hd.sCalcMethod									'ゲルタイム計算方法
			.p_sEquipName		= hd.sEquipName										'本体名
			.p_sXYSensorID		= hd.sXYSensorID									'XYセンサID
			.p_sZSensorID		= hd.sZSensorID										'ZセンサID
			.p_iZPosition		= hd.dZZeroPosition									'熱板ゼロ位置(um)
			.p_dWeightCoef		= hd.dZWeightCoef									'Z dig->g換算係数
			'熱板温度許容範囲
			.p_dTempStubRange	= CType(hd.sHPTempFlucRange, Double)
			.p_dTempStubRange2	= CType(hd.sPBTempFlucRange, Double)
			'サンプル情報
			Select Case hd.sShape
			Case eShape.Sheet
				.p_sThickness	= hd.sThickness									'厚み(mm)
				.p_sStacks		= hd.dStacks.ToString("F0")						'積層枚数
			Case eShape.Powder
				.p_sSpecGravity	= Ctype(hd.sThickness, Double).ToString("F3")	'密度
				.p_sWeight		= hd.dStacks.ToString("F3")						'重量
			Case eShape.Liquid
				.p_sSpecGravity	= Ctype(hd.sThickness, Double).ToString("F3")	'密度
				.p_sVolume		= hd.dStacks.ToString("F3")						'体積
			Case eShape.Rcc
				.p_sThickness	= hd.sThickness									'基材厚み(mm)
				.p_sStacks		= hd.dStacks.ToString("F0")						'積層枚数
			End Select
			.p_iHPPosCont		= hd.sHPPosControl								'熱板位置制御方法 0,1,2
			.p_dMeasureStress	= hd.dMeasurePress									'測定時圧力設定(kPa)
			'ゲルタイム判定範囲
			RangeConvToMinMaxData(hd.sJudgeRange, .p_dJudgeRangeMin, .p_dJudgeRangeMax)
			'マスク範囲
			RangeConvToMinMaxData(hd.sMaskTime, .p_dAprxRangeMin, .p_dAprxRangeMax)

		End With

		'#######################################
		Dim cp As stCalcPara = Nothing
		cp.iMeas			= eHoseiMeasMode.GelTime
		cp.iMode			= eHoseiCalcMode.Batch
		cp.cts				= p_cts.Clone()							'複製をコピー
		cp.cts.WeightCoefZ	= hd.dZWeightCoef
		cp.cts.GapCorr		= If(hd.dGapCorrect=0, cp.cts.GapCorr, hd.dGapCorrect)
		cp.cts.InputGap		= hd.dInputGap
		cp.clDt				= p_dtFile
		cp.iSampleStart		= 0
		cp.iSampleNum		= p_dtFile.iHosei.Length
		'ヘッダ保存されたフィルタパラメータの略式をstFilterInfoタイプのデータへ変換する
		p_dtFile.FilterConvHeaderToFilset(hd.sFilter, cp.fltSet)				'フィルタ動作設定をセット
		cp.dXYWait			= hd.dHPLiftWait+ hd.dXYActWait 
		cp.dZeroStress		= Double.Parse(hd.sZeroCalibValue)
		cp.dAttenuation		= Double.Parse(hd.sFullStressAttenuation)
		cp.dDiameter		= hd.dPBDiameter
		cp.dMeasGap			= hd.dGapValue
		cp.dFullStress		= Double.Parse(hd.sFullCalibValue)
		cp.dPxPhase			= Double.Parse(hd.sFullStressPhaseX)
		cp.dPyPhase			= Double.Parse(hd.sFullStressPhaseY)
		cp.dPzPhase			= Double.Parse(hd.sFullStressPhaseZ)
		cp.dZeroPxPhase		= Math.PI
		cp.dZeroPyPhase		= Math.PI
		cp.dZeroPzPhase		= Math.PI
		cp.iZeroPos			= hd.dZZeroPosition
		cp.iHPPosControl	= hd.sHPPosControl									'熱板位置制御方法 0,1,2
		cp.dMeasureStress	= hd.dMeasurePress
		cp.iCalcType		= lstCalcMethod.IndexOf(hd.sCalcMethod)
		cp.dDiffInterval	= hd.dDiffInterval
		cp.iStrsSensMode	= hd.sStrsDetectMode								'応力検知モード0,1,2
		cp.iAmpMode			= hd.sAmpMode										'Z振動モード

		'RCC時の基材厚み
		Select Case hd.sShape
		Case eShape.Rcc
			cp.dRCCBaseThickness= Ctype(hd.sThickness, Double)
		Case Else
			cp.dRCCBaseThickness= 0.0
		End Select

		Select Case cp.iStrsSensMode
		Case eStrsSens.Point
			cp.dPeriod			= hd.dZPrd
			cp.dAmplitude		= hd.dZAmp
		Case Else
			cp.dPeriod			= hd.dXPrd
			cp.dAmplitude		= hd.dXAmp
		End Select


		If cp.iStrsSensMode=clSpec.eStrsSens.Point AndAlso cp.iAmpMode=clSpec.eAmpMode.StsConst
		'点モードかつ応力一定時のみ
			'指定周期のFull応力値とFullPhase値を取得
			'FULLキャリブレーション結果読込
			Dim frmF As New frmCalibFull(0)
			Dim i As int32
			Dim j As int32
			Dim iRet As int32
			iRet = frmF.sbFullStressCondRead(0, p_FClbX, p_FClbC, p_FClbP)
			frmF.Dispose()
			Array.Resize(cp.dCalibAmp,			0)
			Array.Resize(cp.dCalibFullStress,	0)
			Array.Resize(cp.dCalibFullPhase,	0)
			For i = 0 To p_FClbP.GetUpperBound(0)
				If p_FClbP(i).dPeriodZ = cp.dPeriod
					j = cp.dCalibAmp.Length+1	
					Array.Resize(cp.dCalibAmp,			j)
					Array.Resize(cp.dCalibFullStress,	j)
					Array.Resize(cp.dCalibFullPhase,	j)
					cp.dCalibAmp(j-1)			= p_FClbP(i).dAmplitudeZ
					cp.dCalibFullStress(j-1)	= p_FClbP(i).iFullVal
					cp.dCalibFullPhase(j-1)		= p_FClbP(i).dPPhaseZ
				End If
			Next
		Else
			cp.dCalibAmp			= Nothing
			cp.dCalibFullStress		= Nothing
			cp.dCalibFullPhase		= Nothing
		End If

		Dim p_dNConvSave	As Double = p_dNConv					'現在のXY応力換算係数を退避(WeightCoef)
		cp.dXYWeightCoef	= hd.dXYWeightCoef

		p_cp				= cp									'計算パラメータを退避
		p_RTM(0).ReCalcProc(iUpdate, cp)							'再計算処理

		p_dNConv			= p_dNConvSave							'XY応力換算係数を復帰

		'#######################################
		frm.Chart1.Annotations.Clear()                              '注釈を全て削除

		'計算法毎のチャート表示後の追加表示
		Select Case hd.sCalcMethod
		Case lstCalcMethod(eCalcMethod.THR),lstCalcMethod(eCalcMethod.SLOPE),lstCalcMethod(eCalcMethod.PHASE),lstCalcMethod(eCalcMethod.MELT)

		Case lstCalcMethod(eCalcMethod.EXTRA)
			frm.nudStartTim.Value	= hd.sExtStartTime
			frm.nudMidTim.Value		= hd.sExtMiddleTime
			frm.nudEndTim.Value		= hd.sExtEndTime
			frm.nudExpoSample.Value	= hd.sExtTanSample
			frm.nudThinning.Value	= hd.sExtTanThinning
			'開始、中間、終了点を設定
			frm.clExtra.SetParameterTime(hd.sExtStartTime, hd.sExtMiddleTime, hd.sExtEndTime)
			'外挿法前処理
			frm.clExtra.ExtraPrePareProc(1, frm.Chart1, p_dtFile, frm.nudStartTim.Value, frm.nudMidTim.Value, frm.nudEndTim.Value)
			'外挿法時はゲルタイム再計算
			frm.clExtra.GelTimeCrossLineDisp(frm.Chart1)
			frm.lblUnderBaseA.Text	= frm.clExtra.LowerGelT.ToString("F2")
			frm.lblMidA.Text		= frm.clExtra.MiddlGelT.ToString("F2")
			frm.lblOverBaseA.Text	= frm.clExtra.UpperGelT.ToString("F2")
			frm.ExpGelTimeDisp()            'ゲルタイム表示更新

			'外挿法 操作パネル表示
			frm.gbxExpoPad.Visible = True
			
		End Select
		GelTimeLabelDispUpdate(frm)															'ゲルタイム値ラベル表示更新
	End Select
	GelTimeJudgeColorUpdate(frm)															'ゲルタイム判定表示更新

Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
Finally
	UnderStatusLabelDisp( frm.ToolStripStatusLabel1, "")							'表示クリア
	frm.Cursor = Cursors.Default
	frm.lblCalclating.Visible = False
End Try
End Sub
#If 0
''' <summary>
''' ヘッダ保存されたフィルタパラメータの略式をstFilterInfoタイプのデータへ変換する
''' </summary>
''' <param name="sFilter"></param>
''' <param name="Fltset"></param>
Public Sub FilterConvHeaderToFilset(ByVal sFilter As String, Byref Fltset As stFilterInfo)
	Dim fil As stFilterInfo = Nothing
	Dim s() As String = sFilter.Split("-")					'FIR-1.00-1001-Hamming
	If IsNothing(s)
		Fltset.iEnable = 0
	ElseIf s.Length <> 4
		Fltset.iEnable = 0
	Else
		Fltset.iEnable = 1
		Select Case s(0)
		Case "FIR"
			Fltset.sKind			= s(0)				        'フィルタ種類 FIR/IIR
			Fltset.dFIRCutOffFreq	= Ctype(s(1), Double)		'FIR カットオフ周波数
			Fltset.iFIRFilLen		= Ctype(s(2), integer)      'FIR パラメータ長
			Fltset.sFIRWinKind		= s(3)						'FIR 窓関数種
		Case Else
			Fltset.dIIRCutOffFreq	= Ctype(s(1), Double)		'IIR カットオフ周波数
			Fltset.iIIRDeg			= Ctype(s(2), integer)      'IIR フィルタ次数
		End Select
		Fltset.dFIRSmplFreq			= MEAS_SAMPLE_FREQ			'FIR サンプル周波数
	End If
End Sub
#end if
''' <summary>
''' ヘッダ保存された範囲フォーマットをデータへ変換
''' </summary>
''' <param name="ss"></param>
''' <param name="dMin"></param>
''' <param name="dMax"></param>
Public Sub	RangeConvToMinMaxData(ByVal ss As String, ByRef dMin As Double, ByRef dMax As Double)
	Dim s() As String = ss.Split("-")							'0.0 - 0.0
	If IsNothing(s)
	ElseIf s.Length <> 2
	Else
		dMin = CType(Trim(s(0)), Double)
		dMax = CType(Trim(s(1)), Double)
	End If
End Sub
''' <summary>
''' ゲルタイム結果表示更新
''' </summary>
''' <param name="frm"></param>
Public Sub	GelTimeLabelDispUpdate(ByRef frm As frmMain)
	Dim dTemp() As Double
	Dim sFrm As String
	Select Case p_cts.LTModeEnable Or p_dtFile.p_head.sMeasureType=p_dtFile.sMeasindCode(eMesCD.LTM)
	Case True
		dTemp = p_dtFile.iLT_Hosei
		sFrm = "F0"
		frm.lblResult1.Text = frm.p_sMes(efrmMainSTR.iGelTimeLblMin)
	Case Else
		dTemp = p_dtFile.iHosei
		sFrm = "F2"
		frm.lblResult1.Text = frm.p_sMes(efrmMainSTR.iGelTimeLblSec)
	End Select
	If IsNothing(dTemp)
		frm.lblResult1Val.Visible	= False
		frm.lblResult1.Visible		= False
		frm.lblResult2Val.Visible	= False
		frm.lblResult2.Visible		= False
		frm.lblResult3Val.Visible	= False
		frm.lblResult3.Visible		= False
		Exit Sub
	End If
	Select Case p_clSpec.spi.iGelTimeMethod
	Case eCalcMethod.THR,eCalcMethod.SLOPE,eCalcMethod.PHASE,eCalcMethod.EXTRA
		Dim sGelTime As String = If(p_cts.LTModeEnable, (p_RTM(0).p_dGelTime/60).ToString("F2"), p_RTM(0).p_dGelTime.ToString("F2"))
		'frm.lblResult1.Text			= frm.p_sMes(efrmMainSTR.iRESULT1)
		'frm.lblResult1Val.Text		= If(p_RTM(0).p_dGelTime > 0, p_RTM(0).p_dGelTime.ToString(sFrm), "N/A")				'ゲルタイム結果表示
		frm.lblResult1Val.Text		= sGelTime																				'ゲルタイム結果表示
		frm.lblResult1Val.Visible	= True
		frm.lblResult1.Visible		= True
		frm.lblResult2Val.Visible	= False
		frm.lblResult2.Visible		= False
		frm.lblResult3Val.Visible	= False
		frm.lblResult3.Visible		= False

	Case eCalcMethod.MELT
		Dim sGelTime As String = If(p_cts.LTModeEnable, (p_RTM(0).p_dGelTime/60).ToString("F2"), p_RTM(0).p_dGelTime.ToString("F2"))
		'frm.lblResult1.Text			= frm.p_sMes(efrmMainSTR.iRESULT1)
		'frm.lblResult1Val.Text		= If(p_RTM(0).p_dGelTime > 0, p_RTM(0).p_dGelTime.ToString(sFrm), "N/A")				'ゲルタイム結果表示
		frm.lblResult1Val.Text		= sGelTime																				'ゲルタイム結果表示
		frm.lblResult1Val.Visible	= True
		frm.lblResult1.Visible		= True
		frm.lblResult2.Text			= frm.p_sMes(efrmMainSTR.iRESULT2)
		frm.lblResult2Val.Text		= p_RTM(0).p_dMeltTemp.ToString("F1")						'温度結果表示
		frm.lblResult2Val.Visible	= True
		frm.lblResult2.Visible		= True
		frm.lblResult3.Text			= frm.p_sMes(efrmMainSTR.iRESULT3)
		frm.lblResult3Val.Text		= p_RTM(0).p_dMeltVisco.ToString("F1")						'粘度結果表示
		frm.lblResult3Val.Visible	= True
		frm.lblResult3.Visible		= True
	End Select
	'判定による背景色変更
	GelTimeJudgeColorUpdate(frm)
End Sub
''' <summary>
''' ゲルタイム判定表示更新
''' </summary>
Public Sub	GelTimeJudgeColorUpdate(ByRef frm As frmMain)
	Select Case p_RTM(0).p_iGelTimeJudge
	Case 0	'NG
		frm.lblResult1Val.BackColor = Color.LightPink
	Case 1	'OK
		frm.lblResult1Val.BackColor = Color.LightGreen
	Case Else
		frm.lblResult1Val.BackColor = Color.White
	End Select
End Sub

'FIFOバッファクリア,グラフ表示エリアクリア
Public Sub RealDataClear(ByRef frm As frmMain)
Try
	p_dtFile.ReadBufferClear()                              'データバッファクリア
	p_dtFile.LTReadBufferClear()                            'データバッファクリア 長期間欠モード
	p_dtFile.DataArrayFiFoClear()							'重ね書き用履歴データ配列クリア
	frm.clExtra.AllDispClear(frm.Chart1)                    '外挿法の線を削除
	p_RTM(0).p_dGelTime = -1                                'ゲルタイム補助線削除

	frm.gbxStdCond.Visible = False							'基準ヘッダ非表示
	'rbReal.Checked = False									'基準ヘッダ表示選択のReal選択解除
	'rbStd1.Checked = True									'基準ヘッダ表示選択のStd1選択

	frm.lblResult1Val.Visible	= False
	frm.lblResult2Val.Visible	= False
	frm.lblResult3Val.Visible	= False
	frm.lblResult1.Visible		= False
	frm.lblResult2.Visible		= False
	frm.lblResult3.Visible		= False

	frm.gbxExpoPad.Visible		= False							'外挿法操作パネル消去

Catch ex As Exception
End Try
End Sub
'スケール固定値を初期化
Public Sub ScaleClear(ByRef frm As frmMain)
Try
	Select Case p_cts.LTModeEnable
	Case True
		'X軸設定 初期値
		p_cts.MainGraphXMax(p_cts.iChartType) = 100.0
		p_cts.MainGraphXMin(p_cts.iChartType) = 0.0
		frm.nudXscaleMax.Value = 100.0
		frm.nudXscaleMin.Value = 0.0
		'スクロール幅
		frm.nudXSpan.Value		= 600.0
	Case Else
		'X軸設定 初期値
		p_cts.MainGraphXMax(p_cts.iChartType) = 100.0
		p_cts.MainGraphXMin(p_cts.iChartType) = 0.0
		frm.nudXscaleMax.Value = 100.0
		frm.nudXscaleMin.Value = 0.0
		'スクロール幅
		frm.nudXSpan.Value		= 10.0			
	End Select
Catch ex As Exception
End Try
End Sub
'FIFOバッファクリア,グラフ表示エリアクリア
Public Sub HistoryDataClear(ByRef frm As frmMain)
Try
	'p_dtFile.ReadBufferClear()                             'データバッファクリア
	p_dtFile.DataArrayFiFoClear()						    '重ね書き用履歴データ配列クリア
	'clExtra.AllDispClear(Chart1)                           '外挿法の線を削除
	'p_RTM(0).p_dGelTime = -1                               'ゲルタイム補助線削除

	'gbxStdCond.Visible = False								'基準ヘッダ非表示
	'rbReal.Checked = False									'基準ヘッダ表示選択のReal選択解除
	'rbStd1.Checked = True									'基準ヘッダ表示選択のStd1選択

	frm.lblResult1Val.Visible	= False
	frm.lblResult2Val.Visible	= False
	frm.lblResult3Val.Visible	= False
	frm.lblResult1.Visible		= False
	frm.lblResult2.Visible		= False
	frm.lblResult3.Visible		= False

Catch ex As Exception
End Try
End Sub
'基準値バッファクリア,グラフ表示エリアクリア
Public Sub StandardDataClear(ByRef frm As frmMain)
Try
	p_dtFile.ClearStandard()
	'frm.btnStdInfo.Visible	= False								'基準値情報ボタン
	'frm.gbxStdCond.Visible	= False								'基準値情報GBX
	'rbStd1.Checked		= True								'基準値選択RB
	'rbStd2.Checked		= False								'基準値選択RB
	'rbReal.Checked		= False								'基準値選択RB

Catch ex As Exception
End Try
End Sub


''' <summary>
''' ゼロ調進捗チャート表示
''' </summary>
''' <param name="Chart2"></param>
''' <param name="ds"></param>
Public Sub ZpositionChartDisp(ByRef frm As frmMain, ByVal iMode As Int16, ByRef Chart2 As Chart, ByVal ds As DataSet, ByVal dRangeUpper As Double, ByVal dRangeLower As Double)
	Dim sSeriesName As String
	Try
		'チャートコントロールの設定
		With Chart2
			'初期化
			.Series.Clear()
			.Legends.Clear()
			.Titles.Clear()
			.ChartAreas.Clear()
			.DataSource = ds

			'timeとX,Yを取得
			sSeriesName = ds.Tables(0).Columns(1).ColumnName
			.Series.Add(sSeriesName)
			.Series(sSeriesName).XValueMember = ds.Tables(0).Columns(0).ColumnName
			.Series(sSeriesName).YValueMembers = sSeriesName
			.Series(sSeriesName).ChartType = SeriesChartType.FastLine
			.Series(sSeriesName).Color = Color.Blue
			If iMode = 0 Then
				.Series(sSeriesName).Enabled = False
			Else
				'.Series(sSeriesName).Enabled = True
			End If

			'TITLE 作成
			'Dim title1 As Title = New Title("Z Position Observe")
			Dim title1 As Title = New Title(frm.p_sMes(efrmMainSTR.iZOVS_TITLE))
			.Titles.Add(title1)
			.Titles.Item(0).Font = New Font(BASE_FONT, 16, FontStyle.Bold)
			'ChartArea作成
			.ChartAreas.Add("ChartArea1")
			'Legendの作成
			Dim leg As Legend = New Legend()
			leg.DockedToChartArea = "ChartArea1"
			leg.Alignment = StringAlignment.Near
			.Legends.Add(leg)
			.Legends(0).Enabled = False
			.DataBind()


			'-----------------------------------------------------------------------------------------------------
			'以下、Ｘ軸とＹ軸の設定
			'.ChartAreas(0).AxisX.Title = "Time [Sec] "
			.ChartAreas(0).AxisX.Title = frm.p_sMes(efrmMainSTR.iZMON_TIME)
			.ChartAreas(0).AxisX.TitleFont = New Font(BASE_FONT, 12, FontStyle.Regular)
			.ChartAreas(0).AxisX.Crossing = 0
			.ChartAreas(0).AxisX.IntervalType = DateTimeIntervalType.NotSet
			.ChartAreas(0).AxisX.MajorGrid.Enabled = True
			.ChartAreas(0).AxisX.MajorGrid.LineDashStyle = ChartDashStyle.Dot
			.ChartAreas(0).AxisX.MajorGrid.LineColor = Color.LightGray

			'.ChartAreas(0).AxisY.Title = "Z Position [mm] "
			.ChartAreas(0).AxisY.Title = frm.p_sMes(efrmMainSTR.iZMON_POS)
			.ChartAreas(0).AxisY.TitleFont = New Font(BASE_FONT, 12, FontStyle.Regular)
			.ChartAreas(0).AxisY.Crossing = 0
			.ChartAreas(0).AxisY.IntervalType = DateTimeIntervalType.NotSet
			.ChartAreas(0).AxisY.MajorGrid.Enabled = True
			.ChartAreas(0).AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dot
			.ChartAreas(0).AxisY.MajorGrid.LineColor = Color.LightGray

			'表示アップデート
			ZpositionChartUpdate(iMode, Chart2, ds, dRangeUpper, dRangeLower)
			frm.m_ZeroChartUpdate = False
		End With
	Catch ex As Exception
	End Try
End Sub

Public Sub ZpositionChartUpdate(ByVal iMode As Int16, ByRef Chart2 As Chart, ByVal ds As DataSet, ByVal dRangeUpper As Double, ByVal dRangeLower As Double)
	Try
		'チャートコントロールの設定
		With Chart2
			.DataSource = ds

			If iMode = 0 Then
				.Series(0).Enabled = False
			End If

			'-----------------------------------------------------------------------------------------------------
			'以下、Ｘ軸とＹ軸の設定
			.ChartAreas(0).AxisX.Maximum = ds.Tables(0).Rows.Count              '過去設定安定時間+10秒を表示
			If ds.Tables(0).Rows.Count - (p_cts.iStaMonTime + 0) < 0 Then
				.ChartAreas(0).AxisX.Minimum = 0
			Else
				.ChartAreas(0).AxisX.Minimum = ds.Tables(0).Rows.Count - (p_cts.iStaMonTime + 0)
			End If

			'Dim YMax As Double = p_RTM(0).p_mltcp.HeatBPos / 1000.0 + 0.01          '現在値+0.010mm
			'Dim YMin As Double = p_RTM(0).p_mltcp.HeatBPos / 1000.0 - 0.01          '現在値-0.010mm
			Dim YMax As Double 
			Dim YMin As Double
			If dRangeUpper-dRangeLower<=0
				YMax = p_RTM(0).p_mltcp.HeatBPos / 1000.0 + 0.01          '現在値+0.01 mm
				YMin = p_RTM(0).p_mltcp.HeatBPos / 1000.0 - 0.01          '現在値-0.01 mm
			Else
				YMax = p_RTM(0).p_mltcp.HeatBPos / 1000.0 + ((dRangeUpper-dRangeLower) * 2)          '現在値+(dRangeUpper-dRangeLower) * 2 mm
				YMin = p_RTM(0).p_mltcp.HeatBPos / 1000.0 - ((dRangeUpper-dRangeLower) * 2)          '現在値-(dRangeUpper-dRangeLower) * 2 mm
			End If
			.ChartAreas(0).AxisY.Maximum = YMax
			.ChartAreas(0).AxisY.Minimum = YMin

			'OK範囲 水平補助線追加 
			.ChartAreas(0).AxisY.StripLines.Clear()

'			If ds.Tables(0).Rows.Count > p_cts.iStaMonTime \ 2 Then
				Dim Lower As New StripLine()
				Lower.Text = $"Lower: {dRangeLower.ToString("F3")}"
				Lower.Font = New Font(BASE_FONT, 9, FontStyle.Regular)
				Lower.ForeColor = Color.DarkGray
				Lower.Interval = 0
				Lower.IntervalOffset = dRangeLower
				Lower.TextAlignment = StringAlignment.Far               'Farr	右端
				Lower.TextLineAlignment = StringAlignment.Near          'Near	下側
				Lower.BorderWidth = 1
				Lower.BorderDashStyle = ChartDashStyle.DashDot
				Lower.BorderColor = Color.Red
				.ChartAreas(0).AxisY.StripLines.Add(Lower)

				Dim Upper As New StripLine()
				Upper.Text = $"Upper: {dRangeUpper.ToString("F3")}"
				Upper.Font = New Font(BASE_FONT, 9, FontStyle.Regular)
				Upper.ForeColor = Color.DarkGray
				Upper.Interval = 0
				Upper.IntervalOffset = dRangeUpper
				Upper.TextAlignment = StringAlignment.Far               'Far	右端
				Upper.TextLineAlignment = StringAlignment.Far           'Far	上側
				Upper.BorderWidth = 1
				Upper.BorderDashStyle = ChartDashStyle.DashDot
				Upper.BorderColor = Color.Red
				.ChartAreas(0).AxisY.StripLines.Add(Upper)
'			End If
			.DataBind()

		End With
	Catch ex As Exception
	End Try
End Sub

'応力ゼロ機能ON
Public Sub ZeroSetProc(ByRef frm As frmMain)
	If Not p_RTM(0).p_mltcp.P_StressZeroEnable Then
		p_RTM(0).p_mltcp.P_XStressZero = p_RTM(0).p_mltcp.CurrStrsX()                    'X応力
		p_RTM(0).p_mltcp.P_YStressZero = p_RTM(0).p_mltcp.CurrStrsY()                    'Y応力
''		p_RTM(0).p_mltcp.P_ZStressZero = p_RTM(0).p_mltcp.CurrStrsZ()                    'Z応力
''		p_RTM(0).p_mltcp.P_ZStressZero = p_RTM(0).p_mltcp.CurrStrsZCorrA()               'Z応力(補正後AD)
		p_RTM(0).p_mltcp.P_StressZeroEnable = True
		'応力ゼロ機能によるラベル変更
		frm.lblCurrXStrs.Text = "Δ" & frm.lblCurrXStrs.Text
		frm.lblCurrYStrs.Text = "Δ" & frm.lblCurrYStrs.Text
		frm.lblCurrZStrs.Text = "Δ" & frm.lblCurrZStrs.Text
		frm.tsmiStressZeroSet.Checked = True

	Else
		p_RTM(0).p_mltcp.P_XStressZero = 0                    'X応力
		p_RTM(0).p_mltcp.P_YStressZero = 0                    'Y応力
		p_RTM(0).p_mltcp.P_ZStressZero = 0                    'Z応力
		p_RTM(0).p_mltcp.P_StressZeroEnable = False
		'応力ゼロ機能によるラベル変更
		frm.lblCurrXStrs.Text = frm.lblCurrXStrs.Text.Substring(1)
		frm.lblCurrYStrs.Text = frm.lblCurrYStrs.Text.Substring(1)
		frm.lblCurrZStrs.Text = frm.lblCurrZStrs.Text.Substring(1)
		frm.tsmiStressZeroSet.Checked = False
	End If
End Sub


''' <summary>
''' インポートファイル選択ダイアログ
''' </summary>
''' <param name="iArea">0:測定値エリア 1:基準値エリア</param>
''' <param name="sPath">ファイルパス配列</param>
''' <returns></returns>
Public Function InportFileSelect(ByRef frm As frmMain, ByVal iArea As Int32, ByRef sPath() As String) As DialogResult
Dim iRetD As DialogResult
Try
	sPath = Nothing
	With frm.OpenFileDialog1
		.Title = "Select files for " & If(iArea=0, "Measurement area", "Standard area")
		.Multiselect = True								'複数選択ON
		.DefaultExt = sDataExt
		.Filter = "Data File|*." & sDataExt & "|" + "All File|*.*"
		.InitialDirectory = p_dtFile.DATA_STORE_PATH	'初期ディレクトリ
		.RestoreDirectory = True						'ディレクトリ復元ON
		.FileName = Nothing
		iRetD = .ShowDialog()
		If iRetD = DialogResult.Cancel Then
		Else
			sPath = .FileNames
		End If
	End With
	Return iRetD
Catch ex As Exception
	Return -1
End Try
End Function
''' <summary>
''' 指定領域読込み処理
''' </summary>
''' <param name="iSel">0:測定値領域,1:基準値領域1,2:基準値領域2</param>
Public Sub cmsDragDropProc(ByRef frm As frmMain, ByVal iSel As Int32)
Dim bCalc	As Boolean
Dim sMsg	As String
Dim fMsg	As frmMSG
Dim f As frmSpecNameInp = Nothing
Dim iRet As int32
Dim bProc As Boolean = False

	UnderStatusLabelDisp( frm.ToolStripStatusLabel1, frm.DragDropFilePath(0) )			'ファイルパス表示
	'現在の測定モードと同一か判定 今のところは通常GEL測定と間欠測定
	'判定方法
	'間欠モード時にファイル名先頭３文字がLTMであること(間欠測定)   または
	'間欠モード以外の時にファイル名文字数が25以下であること(通常GEL)
	Select Case Path.GetFileName(frm.DragDropFilePath(0)).Substring(0,3)
	Case p_dtFile.sMeasindCode(eMesCD.LTM)
		If p_cts.LTModeEnable
			bProc = True
		Else
			bProc = False
		End If
	Case p_dtFile.sMeasindCode(eMesCD.GLA)
		If p_cts.LTModeEnable
			bProc = False
		Else
			bProc = True
		End If
	Case Else
		If Not p_cts.LTModeEnable AndAlso Path.GetFileName(frm.DragDropFilePath(0)).Length <= 25
			bProc = True
		Else
			bProc = False
		End If
	End Select
	'If (Path.GetFileName(frm.DragDropFilePath(0)).Substring(0,3)=p_dtFile.sMeasindCode(eMesCD.LTM) And p_cts.LTModeEnable) Or _
	'	(Path.GetFileName(frm.DragDropFilePath(0)).Length <= 25 And Not p_cts.LTModeEnable)
	If bProc
		Dim sHead As DataFile.DataFileHeader = Nothing
		Select Case iSel
		Case 0		'測定値領域
			'SPI領域へ反映するか判断
			Dim iSPIEna As Int16 = -1
			sMsg = frm.p_sMes(efrmMainSTR.iSpecOverWrite_MSG)
			fMsg = New frmMSG(sMsg, {frm.p_sMes(efrmMainSTR.iYES),frm.p_sMes(efrmMainSTR.iNO)}, frm.m_sGuideMsgTitle, frm)
			fMsg.StartPosition = FormStartPosition.CenterScreen
			fMsg.ShowDialog()
			Select Case fMsg.p_iRes
			Case 1		'1:はい
				iSPIEna = 1
			Case Else	'2:いいえ
			End Select
			fMsg.Dispose()

			frm.lblCalclating.text = frm.p_sMes(efrmMainSTR.iReading)
			frm.lblCalclating.Visible = True
			frm.lblCalclating.Refresh

			If Not IsNothing(p_dtFile.iOxBuf)           '既にデータがある場合は上書き確認し、しない場合は現在値を履歴へ移してから上書き
#If 1
			'常にFIFO処理
				sMsg = frm.p_sMes(efrmMainSTR.iFILE_OVERWRITE_MSG1)							'上書きしますか？上書きする場合は元のデータは履歴データへ移動します。
				fMsg = New frmMSG(sMsg, {frm.p_sMes(efrmMainSTR.iYES),frm.p_sMes(efrmMainSTR.iNO)}, frm.m_sGuideMsgTitle, frm)
				fMsg.lblMsg.Font = New Font(BASE_FONT, 12)
				fMsg.Width = 400
				fMsg.StartPosition = FormStartPosition.CenterScreen
				fMsg.ShowDialog()
				iRet = fMsg.p_iRes
				fMsg.Dispose()
				Select Case iRet
				Case 1	'Yes(履歴移動&上書き)
					'測定データを履歴へ移動
					p_dtFile.DataArrayFiFo(p_cts.LTModeEnable)                                '履歴データFIFO処理
					'ファイルリード
					iRet = p_dtFile.RealTimeMeasDataFileReadText(iSel, frm.DragDropFilePath, bCalc, sHead)		'ファイル読み込んで測定値領域へ転送
				Case 2	'No(処理キャンセル)
					frm.lblCalclating.Visible = False
					Exit Sub
				End Select
#end if
			Else
				'ファイルリード
				iRet = p_dtFile.RealTimeMeasDataFileReadText(iSel, frm.DragDropFilePath, bCalc, sHead)		'ファイル読み込んで測定値領域へ転送
			End If
			If iRet <> 0
				UnderStatusLabelDisp( frm.ToolStripStatusLabel1, frm.p_sMes(efrmMainSTR.iInvalidFileFormat) & Space(2) & frm.DragDropFilePath(0)  )			'読込失敗
				Exit Sub
			End If

			'p_DtFile.p_Head.iState						= 1							'ファイルから読み込んだ直後

			If iSPIEna=1
				'現在のSPEC DGV内容を「Free-Backup」へ退避する
				f = New frmSpecNameInp(eSpec.Device, "")
				f.SpecFileWrite(eSpec.Device, "Free-Backup")
				f.Dispose()
				f = New frmSpecNameInp(eSpec.Sample, "")
				f.SpecFileWrite(eSpec.Sample, "Free-Backup")
				f.Dispose()
				'読み込んだヘッダ情報をSPEC入力領域へ転送する
				p_clSpec.HeaderToSpecSpi(sHead)
				'SPEC入力領域からのデータでDGV表示更新
				p_clSpec.SpecSpiToDgvUpdate(frm)

				frmMain.DevSpecToFree()							'Freeモード移行
				'測定値領域諸設定
				p_dtFile.SetDataFileName					= sHead.sFileName			'ファイル名
				p_RTM(0).p_iZPosition						= sHead.dZZeroPosition		'ゼロ調完了位置
				p_RTM(0).p_dWeightCoef						= sHead.dXYWeightCoef		'XY重量変換
				p_RTM(0).p_dFullStressADVal					= sHead.sFullCalibValue		'フルキャリブレーション値
				p_RTM(0).p_dToqueThreshold					= sHead.sThreshold			'閾値
				p_RTM(0).p_dMeasTimeLimit					= sHead.dMeasTimeLimit		'測定時間
				p_RTM(0).p_strRTGSetName					= sHead.sTempProfile		'温度プロファイル名

				'リンク結合
				p_clSpec.sbSpecToLinkArea()

				'温度プロファイル設定がある場合は初期温度を設定
				If p_RTM(0).p_strRTGSetName		<> "" 
					Dim bRet As Boolean
					bRet = p_RTM(0).sbLoadRTGSetting(p_RTM(0).p_strRTGSetName)
					If bRet
						p_RTM(0).p_mltcp.TempPreset(1)	= p_RTM(0).p_rtgset.mdbr(0).dHPTemp
						p_RTM(0).p_mltcp.Temp2Preset(1) = p_RTM(0).p_rtgset.mdbr(0).dHPTemp
						p_RTM(0).p_mltcp.Temp3Preset(1) = p_clSpec.SGTempGet(p_RTM(0).p_rtgset.mdbr(0).dHPTemp)
					Else
					End if
				End If

				frm.lblCalclating.Text = frm.p_sMes(efrmMainSTR.iCalucurating)
				frm.Refresh

				If (Not bCalc) And sHead.sMeasureType=p_dtFile.sMeasindCode(eMesCD.GEL)
				'計算済データが無いので再計算する
					Select Case sHead.sStrsDetectMode
					Case clSpec.eStrsSens.Point
						datCal.AveTempClear(sHead.dZPrd)			'クリア
					Case Else
						datCal.AveTempClear(sHead.dXPrd)			'クリア
					End Select
					ReCalclationProc(frm, 2, sHead)										'再計算処理 元ファイルへは書き込まない
					'計算で更新されたヘッダ情報をセットする
					sHead.dZAfterPos	= p_dtFile.p_head.dZAfterPos
					sHead.dRealGap		= p_dtFile.p_head.dRealGap
					ReCalclationMainProcFromHeader(frm, sHead)							'再計算処理後
				End If
				frm.lblCalclating.Visible = False
				frm.btnStdInfo.Visible	= True											'基準値情報ボタン
				frm.rbReal.Checked		= True											'測定値ラジオボタン
				'既にリスト表示中なら更新する
				If frmHistData.Visible
					frmHistData.HistConditionDisp(0)
				End If


				p_dtFile.p_head = sHead					'マスタヘッダ情報へ読み込んだヘッダを上書き
				frmMain.MethodChanged(p_clSpec.spi.iGelTimeMethod)
				'チャート更新 ゲルタイム判定更新
				iRet = p_RTM(0).GelTimeSeach(frm.p_iSelectGItem)	'ゲルタイム再検索
				frmMain.ResearchGelTimeOnChart()					'ゲルタイム表示更新
				GelTimeLabelDispUpdate(frm)
				GelTimeJudgeColorUpdate(frmMain)					'ゲルタイム判定表示
			Else
				If (Not bCalc) And sHead.sMeasureType=p_dtFile.sMeasindCode(eMesCD.GEL)
				'計算済データが無いので再計算する
					Select Case sHead.sStrsDetectMode
					Case clSpec.eStrsSens.Point
						datCal.AveTempClear(sHead.dZPrd)			'クリア
					Case Else
						datCal.AveTempClear(sHead.dXPrd)			'クリア
					End Select
					ReCalclationProc(frm, 2, sHead)										'再計算処理 元ファイルへは書き込まない
					ReCalclationMainProcFromHeader(frm, sHead)							'再計算処理後
				End If
				frm.lblCalclating.Visible = False
				frm.btnStdInfo.Visible	= True											'基準値情報ボタン
				frm.rbReal.Checked		= True											'測定値ラジオボタン
				'既にリスト表示中なら更新する
				If frmHistData.Visible
					frmHistData.HistConditionDisp(0)
				End If

			End If
			Call WriteLog("測定値領域へファイルから読込まれました。- " & frm.DragDropFilePath(0), eLOG_LEVEL.iINFO)
		
		Case 1, 2		'基準値領域
			iRet = p_dtFile.RealTimeMeasDataFileReadText(iSel, frm.DragDropFilePath, bCalc, sHead)		'ファイル読み込んで基準値領域へ転送
			If iRet <> 0
				UnderStatusLabelDisp( frm.ToolStripStatusLabel1, frm.p_sMes(efrmMainSTR.iInvalidFileFormat) & Space(2) & frm.DragDropFilePath(0)  )			'読込失敗
				Exit Sub
			End If
			frm.btnStdInfo.Visible	= True											'基準値情報ボタン
			frm.rbstd1.Checked		= True											'基準値1ラジオボタン
			If Not bCalc  And (sHead.sMeasureType=p_dtFile.sMeasindCode(eMesCD.GEL) Or sHead.sMeasureType=p_dtFile.sMeasindCode(eMesCD.GLA))
			'計算済データが無いので再計算する
				UnderStatusLabelDisp( frm.ToolStripStatusLabel1, frm.p_sMes(efrmMainSTR.iRECALC_PROC))							'再計算中 表示
				frm.StatusStrip1.Refresh
				frm.Cursor = Cursors.WaitCursor
				frm.lblCalclating.BackColor = frm.Chart1.ChartAreas(0).BackColor
				frm.lblCalclating.Visible = True
				frm.lblCalclating.Refresh
			
				'基準値の再計算
				datCal.AveTempClear(sHead.dXPrd)									'クリア
				p_dtFile.ReCalclationStd(frm, iSel, p_cts)								'再計算処理

				UnderStatusLabelDisp( frm.ToolStripStatusLabel1, "")					'表示クリア
				frm.Cursor = Cursors.Default
				frm.lblCalclating.Visible = False
			Else
				If  sHead.sMeasureType=p_dtFile.sMeasindCode(eMesCD.GEL)
					If IsNothing(p_dtFile.iOxBuf)
					'測定領域にデータ無い
						p_Rtm(0).p_dFullStressADVal			= sHead.sFullCalibValue		'フルキャリブレーション値
					End If
				Else
					If IsNothing(p_dtFile.iLT_MSec)
					'測定領域にデータ無い
						p_Rtm(0).p_dFullStressADVal			= sHead.sFullCalibValue		'フルキャリブレーション値
					End If
				End If
			End If
			'既にリスト表示中なら更新する
			If frmHistData.Visible
				frmHistData.HistConditionDisp(0)
			End If
			Call WriteLog("基準値領域へファイルから読込まれました。- " & frm.DragDropFilePath(0), eLOG_LEVEL.iINFO)
		End Select

		frm.RealTrendUpdate = False
		ChartDispUpdate(frm, 0, frm.RealTrendUpdate)           'チャート表示更新 ------------------
	Else
		'現在の測定モードと異なる測定データである メッセージ
		UnderStatusLabelDisp( frm.ToolStripStatusLabel1, frm.DragDropFilePath(0) & " 測定モードが異なります。" )			'ファイルパス表示
		Call WriteLog("現在の測定モードと異なる測定データを読込もうとしました。- " & frm.DragDropFilePath(0), eLOG_LEVEL.iINFO)
	End If

End Sub




''' <summary>
''' SPEC入力環境の初期化
''' </summary>
''' <param name="frm"></param>
Public Sub SpecInputInit(ByRef frm As frmMain)
Dim iRet		As int32
Dim sSpecDir	As String
Dim s			As New ArrayList										'CSVファイルIF用のアレイリスト
Try

	'---------------------------------------------------------------
	'新SPEC入力
	frm.pnlSPEC.Visible		= True
	frm.pnlSPEC.Location	= New Point(1400,29)
	frm.pnlSPEC.Size		= New Size(515,987)


	frm.cbxDevSPEC.Enabled = False
	frm.cbxSamSPEC.Enabled = False

	'-----------------------------------------------------------------------------------------------------------------------------
	'サンプルSPECコントロールレイアウト
#If 0
	Dim iSamBaseY As Int32 = frm.dgvDevSpec.Top+frm.dgvDevSpec.height + 16
	frm.lblSamSPECNameLabel.Location	= New Point(5, iSamBaseY+9)
	frm.cbxSamSPEC.Location				= New Point(123, iSamBaseY+5)
	frm.dgvSamSPEC.Location				= New Point(0, iSamBaseY+39)
	frm.cbxSamSPEC.Width				= 300
	frm.dgvSamSPEC.Width				= 510
	frm.btnSamSpecEdit.Location			= New Point(frm.cbxSamSPEC.Left + frm.cbxSamSPEC.Width + 2, iSamBaseY+5)
	frm.btnSamSpecEdit.Width			= 85
#else
	frm.lblSamSPECNameLabel.Location	= New Point(5, 9)
	frm.cbxSamSPEC.Location				= New Point(123, 5)
	frm.dgvSamSPEC.Location				= New Point(0, 39)
	frm.cbxSamSPEC.Width				= 300
	frm.dgvSamSPEC.Width				= 510
	frm.btnSamSpecEdit.Location			= New Point(frm.cbxSamSPEC.Left + frm.cbxSamSPEC.Width + 2, 5)
	frm.btnSamSpecEdit.Width			= 85
#End If

	'サンプルSPEC選択コンボボックス初期化
	'登録されているサンプルSPECのリスト取得
	sSpecDir = Path.Combine(SIZUKA_PATH, p_sEquipName, SAMSPEC_PATH)
	iRet = p_clSpec.SpecFileGet(frm.cbxSamSPEC)
	If iRet <> 0
		'ファイルがない場合はデフォルトSPECをコピー
		File.Copy(Path.Combine(My.Application.Info.DirectoryPath, "defaultSampleSpec.csv"), Path.Combine(sSpecDir,"Free.csv")) 
		p_clSpec.SpecFileGet(frm.cbxSamSPEC)
	End If
	s		= New ArrayList												'CSVファイルIF用のアレイリスト
	Dim sSamSpecName	As String
	'起動時はFreeで
	frm.cbxSamSPEC.Text = cFree
	sSamSpecName = frm.cbxSamSPEC.Text
	p_clSpec.dgvSpecDispUpdate(eSpec.Sample, frm, sSamSpecName)			'SPEC データグリッドビュー表示更新 ファイル読込から


	'-----------------------------------------------------------------------------------------------------------------------------
	'デバイスSPECコントロールレイアウト
#If 0
	frm.lblDevSPECNameLabel.Location	= New Point(5, 9)
	frm.cbxDevSPEC.Location				= New Point(123, 5)
	frm.dgvDevSPEC.Location				= New Point(0, 39)
#else
	Dim iDevBaseY As Int32 = frm.dgvSamSpec.Top+frm.dgvSamSpec.height + 16
	frm.lblDevSPECNameLabel.Location	= New Point(5, iDevBaseY+9)
	frm.cbxDevSPEC.Location				= New Point(123, iDevBaseY+5)
	frm.dgvDevSPEC.Location				= New Point(0, iDevBaseY+39)
	frm.cbxDevSPEC.Width				= 300
	frm.dgvDevSPEC.Width				= 510
	frm.btnDevSpecEdit.Location			= New Point(frm.cbxDevSPEC.Left + frm.cbxDevSPEC.Width + 2, iDevBaseY+5)
	frm.btnDevSpecEdit.Width			= 85
#End If

	'デバイスSPEC選択コンボボックス初期化
	'登録されているデバイスSPECのリスト取得
	sSpecDir = Path.Combine(SIZUKA_PATH, p_sEquipName, DEVSPEC_PATH)

#If 0
	Dim i As int32
	Dim sVisDevSpecName() As String = {"高粘度(10～100PaS)測定.csv","超高粘度(100～1000PaS)測定.csv","低中粘度(1～10PaS)測定.csv","低粘度(1PaS以下)測定.csv"}
	Dim sVisDevSpecPath() As String = {Path.Combine(sSpecDir,sVisDevSpecName(0)),Path.Combine(sSpecDir,sVisDevSpecName(1)), _
										Path.Combine(sSpecDir,sVisDevSpecName(2)),Path.Combine(sSpecDir,sVisDevSpecName(3))}
	'点モード粘度測定用デバイスSPECファイル無ければコピーする
	For i=0 To sVisDevSpecName.GetUpperBound(0)
		If Not File.Exists(Path.Combine(sSpecDir,sVisDevSpecName(i)))
			File.Copy(Path.Combine(My.Application.Info.DirectoryPath, sVisDevSpecName(i)), sVisDevSpecPath(i), True)
		End if
	Next
#end if

	If Not File.Exists(Path.Combine(sSpecDir,"Free.csv"))
		File.Copy(Path.Combine(My.Application.Info.DirectoryPath, "defaultDeviceSpec.csv"), Path.Combine(sSpecDir,"Free.csv"), True) 
	End If
	iRet = p_clSpec.SpecFileGet(frm.cbxDevSPEC)
	If iRet <> 0
	End If

	Dim sDevSpecName	As String
	'起動時はFreeで
	frm.cbxDevSPEC.Text = cFree
	sDevSpecName = frm.cbxDevSPEC.Text
	p_clSpec.dgvSpecDispUpdate(eSpec.Device, frm, sDevSpecName)			'SPEC データグリッドビュー表示更新 ファイル読込から


	frm.cbxDevSPEC.Enabled = True
	frm.cbxSamSPEC.Enabled = True

Catch ex As Exception

End Try
End Sub
Public p_cp As stCalcPara = Nothing			'直近計算パラメータの退避
''' <summary>
''' SLOPE法の再計算
''' </summary>
''' <returns></returns>
Public Function SlopeRecalc() As int32
Try
	If p_cp.iCalcType = eCalcMethod.SLOPE And Not IsNothing(p_cp)
		p_cp.dDiffInterval = p_clSpec.spi.dDiffInterval						'微分区間(秒)

		datCal.MakeHoseiToSlope(p_cp)										'補正応力からSLOPE計算

		'SLOPEのフィルタ処理 -------------------
		If p_cts.iFilterEnable = 1
		'フィルタ有効
			Dim fil				As New Filter									'フィルタクラス生成
			Dim	Coef()			As Double		= Nothing						'FIRフィルタパラメータ(窓関数から生成)
			Dim i				As Int64
			Dim iSlope()		As Double		= Nothing
			Dim FilSet			As stFilterInfo = Nothing
			FilSet.iEnable			= 1										'フィルタ有効無効
			FilSet.sKind			= p_cts.sFilterKind						'フィルタ種類
			FilSet.iFIRFilLen		= p_cts.iFilterLen						'FIRフィルタ パラメータ数
			FilSet.dFIRCutOffFreq	= p_cts.dFIRCutOffFreq					'FIRフィルタ カットオフ周波数
			FilSet.sFIRWinKind		= p_cts.sFilterWinKind					'FIRフィルタ 窓関数種類
			FilSet.dFIRSmplFreq		= MEAS_SAMPLE_FREQ						'FIRフィルタ サンプル周波数
			FilSet.dIIRCutOffFreq	= p_cts.dIIRCutOffFreq					'IIRフィルタ カットオフ周波数
			FilSet.iIIRDeg			= p_cts.iIIRDeg							'IIRフィルタ 次数
			FilSet.sFileName		= p_cts.sFilterFileName					'フィルタ係数ファイル名

			Array.Resize(iSlope,		p_cp.cldt.iSlope.Length)			'フィルタ用補正データ領域確保
			Select Case FilSet.sKind
			Case "FIR"
				fil.WindowFunction(FilSet,	Coef)							'FIRフィルタパラメータ作成
				fil.FirFilterD(p_cp.cldt.iSlope,		Coef, iSlope)		'FIRフィルタ
				'遅延分をずらす
				Dim filLen As Int32 = FilSet.iFIRFilLen/2
				Array.Copy(iSlope,		filLen, iSlope,		0, iSlope.Length-filLen)
				'遅延分をNAN
				For i=p_cp.clDt.iHosei.Length - filLen To p_cp.clDt.iHosei.getupperbound(0)
					iSlope(i)				= Double.NaN
				Next
			Case "IIR"
				fil.IirFilterButterworthD(p_cp.cldt.iSlope,		FilSet, iSlope)		'IIRバタワース
			End Select
			Array.Copy(iSlope,		p_cp.cldt.iSlope,		iSlope.Length)					'最終補正データをフィルタ済補正データで上書き
		End If
		'------------------------------------------


		'== ゲルタイム計算処理 ==
		Dim p_dGeltime As Double = -1
		p_dtFile.CalcGelTime(eCalcMethod.SLOPE,	frmMain.p_iSelectGItem, p_dGelTime, p_clSpec.spi.dThreshold , 300, p_clSpec.spi.dEffectRangeMin, p_clSpec.spi.dEffectRangeMax,,,,p_cts)
		'ゲルタイム判定範囲設定
		p_RTM(0).p_dJudgeRangeMin		= p_clSpec.spi.dJudgeRangeMin
		p_RTM(0).p_dJudgeRangeMax		= p_clSpec.spi.dJudgeRangeMax

		'ゲルタイム判定
		Select Case p_dGelTime
		Case p_RTM(0).p_dJudgeRangeMin To p_RTM(0).p_dJudgeRangeMax
			p_RTM(0).p_iGelTimeJudge = 1         'OK
		Case Else
			If (p_RTM(0).p_dJudgeRangeMax - p_RTM(0).p_dJudgeRangeMin) <> 0
				p_RTM(0).p_iGelTimeJudge = 0     'NG
			Else
				p_RTM(0).p_iGelTimeJudge = 2     'NA
			End If
		End Select
		'チャート更新に必要なリアルタイム領域変数を設定
		p_RTM(0).p_dMeasTimeLimit		= p_clSpec.spi.dMeasureTime
		p_RTM(0).p_sCalcMethod			= lstCalcMethod( p_clSpec.spi.iGelTimeMethod )
		p_RTM(0).p_dAprxRangeMin		= p_clSpec.spi.dEffectRangeMin
		p_RTM(0).p_dAprxRangeMax		= p_clSpec.spi.dEffectRangeMax
		p_RTM(0).p_dGelTime				= p_dGelTime
		p_RTM(0).p_dToqueThreshold		= p_clSpec.spi.dThreshold

	End If
	Return 0
Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Return -1
End Try
End Function

'測定データ印刷 ヘッダ情報リスト項目文字列
Public p_sMeasCondForPrint() As string = {
"測定日時", _
"本体名", _
"XYセンサシリアルNo", _
"ZセンサシリアルNo", _
"測定ファイル名", _
" ", _
"サンプルSPEC", _
"サンプル名", _
"ロット№", _
"測定者", _
"形状", _
"厚さ[mm]/密度[g/ml]", _
"積層数[枚]/重量[g]/体積[ml]", _
"有効範囲[秒]", _
"規格判定範囲[秒]", _
" ", _
"デバイスSPEC", _
"熱板位置制御", _
"ゲルタイム判定法", _
"判定値[％]", _
"測定温度(熱板/プローブ)[℃]", _
"投入GAP[mm]", _
"測定GAP[mm]", _
"目標圧力[kPa]", _
"着圧制御ステップ数", _
"プローブ直径[mm]", _
"応力検知モード", _
"振幅[mm]", _
"周期[秒]", _
"投入から熱板上昇開始待ち[秒]", _
"圧着後の待機時間[秒]", _
"測定終了時間[秒]", _
"測定終了圧力[%]", _
"温度プロファイル", _
"微分区間(SLOPE)[秒]", _
" ", _
"ゲルタイム[秒]", _
"MELT温度[℃]", _
"MELT粘度[PaS]", _
" ", _
"メモ" _
}
Public p_sMeasCondValForPrint() As String = Nothing							'印刷ヘッダ項目データ配列
Public p_bReportCat()			As Boolean = {True,True,True,True}			'印刷カテゴリ選択
Public p_iCategory				As int16									'印刷カテゴリ選択No

''' <summary>
''' ヘッダ情報リスト項目 データ部編集
''' </summary>
Public Sub MeasCondValueArrayMakeForPrint()
Try
	Array.Resize(Of String)(p_sMeasCondValForPrint, p_sMeasCondForPrint.Length)
	p_sMeasCondValForPrint(0) = p_dtFile.p_head.sDateTime
	p_sMeasCondValForPrint(1) = p_dtFile.p_head.sEquipName
	p_sMeasCondValForPrint(2) = p_dtFile.p_head.sXYSensorID
	p_sMeasCondValForPrint(3) = p_dtFile.p_head.sZSensorID
	p_sMeasCondValForPrint(4) = p_dtFile.p_head.sFileName.Substring(0,14)
	p_sMeasCondValForPrint(5) = Space(1)
	p_sMeasCondValForPrint(6) = p_dtFile.p_head.sSampleSpec
	p_sMeasCondValForPrint(7) = p_dtFile.p_head.sSampleName
	p_sMeasCondValForPrint(8) = p_dtFile.p_head.sLot
	p_sMeasCondValForPrint(9) = p_dtFile.p_head.sMeasurer
	p_sMeasCondValForPrint(10) = p_clspec.sShape(p_dtFile.p_head.sShape)
'	p_sMeasCondValForPrint(11) = p_dtFile.p_head.dThickness.ToString("F2")
	p_sMeasCondValForPrint(11) = p_dtFile.p_head.sThickness
	p_sMeasCondValForPrint(12) = p_dtFile.p_head.dStacks.ToString("F2")
	p_sMeasCondValForPrint(13) = p_dtFile.p_head.sMaskTime
	p_sMeasCondValForPrint(14) = p_dtFile.p_head.sJudgeRange
	p_sMeasCondValForPrint(15) = Space(1)
	p_sMeasCondValForPrint(16) = p_dtFile.p_head.sDeviceSpec
	p_sMeasCondValForPrint(17) = p_clspec.sHCont(p_dtFile.p_head.sHPPosControl)
	p_sMeasCondValForPrint(18) = p_dtFile.p_head.sCalcMethod
	p_sMeasCondValForPrint(19) = p_dtFile.p_head.sThreshold
	p_sMeasCondValForPrint(20) = p_dtFile.p_head.dHPTemp.ToString("F1") & "/" & p_dtFile.p_head.dPBTemp.ToString("F1")
	p_sMeasCondValForPrint(21) = p_dtFile.p_head.dInputGap.ToString("F2")
	p_sMeasCondValForPrint(22) = p_dtFile.p_head.dGapValue.ToString("F3")
	p_sMeasCondValForPrint(23) = p_dtFile.p_head.dMeasurePress.ToString("F1")
	p_sMeasCondValForPrint(24) = p_dtFile.p_head.iPressContStep
	p_sMeasCondValForPrint(25) = p_dtFile.p_head.dPBDiameter.ToString("F1")
	p_sMeasCondValForPrint(26) = p_clSpec.sStrsSens(p_dtFile.p_head.sStrsDetectMode)
	Select Case p_dtFile.p_head.sStrsDetectMode
	Case clSpec.eStrsSens.Circle, clSpec.eStrsSens.Line
		p_sMeasCondValForPrint(27) = p_dtFile.p_head.dXAmp.ToString("F3")
		p_sMeasCondValForPrint(28) = p_dtFile.p_head.dXPrd.ToString("F1")
	Case clSpec.eStrsSens.Point
		p_sMeasCondValForPrint(27) = p_dtFile.p_head.dZAmp.ToString("F3")
		p_sMeasCondValForPrint(28) = p_dtFile.p_head.dZPrd.ToString("F1")
	Case Else
		p_sMeasCondValForPrint(27) = p_dtFile.p_head.dXAmp.ToString("F3")
		p_sMeasCondValForPrint(28) = p_dtFile.p_head.dXPrd.ToString("F1")
	End Select
	p_sMeasCondValForPrint(29) = p_dtFile.p_head.dHPLiftWait.ToString("F1")
	p_sMeasCondValForPrint(30) = p_dtFile.p_head.dXYActWait.ToString("F1")
	p_sMeasCondValForPrint(31) = p_dtFile.p_head.dMeasTimeLimit.ToString("F1")
	p_sMeasCondValForPrint(32) = p_dtFile.p_head.dMeasEndStress.ToString("F1")
	p_sMeasCondValForPrint(33) = If(p_dtFile.p_head.sTempProfile="",Space(1),p_dtFile.p_head.sTempProfile)
	p_sMeasCondValForPrint(34) = p_dtFile.p_head.dDiffInterval.ToString("F1")
	p_sMeasCondValForPrint(35) = Space(1)
	p_sMeasCondValForPrint(36) = p_dtFile.p_head.dGelTime.ToString("F2")
	p_sMeasCondValForPrint(37) = p_dtFile.p_head.dGelTimeTemp.ToString("F1")
	p_sMeasCondValForPrint(38) = p_dtFile.p_head.dGelTimeVisco.ToString("F1")
	p_sMeasCondValForPrint(39) = Space(1)
	p_sMeasCondValForPrint(40) = p_sLtPrintMemo

Catch ex As Exception
    Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub
Public Sub	CurrentMeasurePrintReport(ByRef frm As frmMain, sender As Object, e As PrintPageEventArgs, ByRef Grp As Graphics)

    '印刷サイズ、ロケーション(余白を除く)
    Dim iPrntW		As Integer
    Dim iPrntH		As Integer
    Dim iPrntT		As Integer
    Dim iPrntL		As Integer
    '描画座標
    Dim sngX		As Single
    Dim sngY		As Single
    Dim sngX2		As Single
    Dim sngY2		As Single
    '拡大率
    'Dim sngRate		As Single
    'フォントの設定
    'Dim fntDrw As Font = New Font(BASE_FONT, 10)
    Dim pfSize As PointF
    Dim sVal As String = ""
    Dim sbrsClr As SolidBrush = New SolidBrush(Color.Black)

	Dim uRExtWaku		As Rectangle = Nothing
	Dim uRCondListItem	As Rectangle = Nothing
	Dim i				As Int32
Try
    frm.PrintDocument1.DefaultPageSettings.Landscape = True		'横向き
    '余白を除いた印刷サイズ、ロケーションの取得
    With e.MarginBounds
        iPrntT = .Top
        iPrntL = .Left
        iPrntW = .Width
        iPrntH = .Height
    End With
    With frm.PrintDocument1.DefaultPageSettings
        iPrntW = iPrntW - (.HardMarginX * 2)
        iPrntH = iPrntH - (.HardMarginY * 2)
    End With

     'フォントサイズの設定(827=A4高さ[1/100インチ] ※A4を基準にフォントを決定) 
'    sngRate = PrintDocument1.DefaultPageSettings.PaperSize.Width / 827
'    sngRate = PrintDocument1.DefaultPageSettings.PaperSize.Width / 1654
'    fntDrw = New Font(BASE_FONT, 12 * sngRate)
    frm.lblStrLen.Font = New Font(BASE_FONT, 12)

    '日時の描画
    sngX = sngX + (iPrntW * 0.73) : sngY = sngY + (iPrntH * 0.035)
    sngX2 = iPrntW * 0.2 : sngY2 = 0.01
    frm.lblStrLen.Text = "Print Date : " & Format(Now, cDateFormYMDHm)
    frm.lblStrLen.Font = New Font(BASE_FONT, 12, FontStyle.Underline)
    fnFormatDrawStr(frm.lblStrLen, Grp, sngX2, sngY, pfSize)
    Grp.DrawString(frm.lblStrLen.Text, frm.lblStrLen.Font, Brushes.Black, sngX + sngX2 - pfSize.X, sngY - pfSize.Y)

	'ページタイトルの描画
    frm.lblStrLen.Text = "Sizuka Measurement report " & "(Category " & (p_iCategory+1).tostring & ")"
    frm.lblStrLen.Font = New Font(BASE_FONT, 18, FontStyle.Bold Or FontStyle.Underline)
    Grp.DrawString(frm.lblStrLen.Text, frm.lblStrLen.Font, Brushes.Black, uRExtWaku.X + 20, uRExtWaku.Y + 10)


	Dim iListXStart As int32 = 20
	Dim iListYStart As int32 = 50
	Dim iListWidth	As int32 = 200
	Dim iListHeight	As int32 = 20
	'条件タイトル文字列 描画
    frm.lblStrLen.Font = New Font(BASE_FONT, 12, FontStyle.Regular)
	uRCondListItem.X		= uRExtWaku.X + iListXStart
	uRCondListItem.Y		= uRExtWaku.Y + iListYStart
	uRCondListItem.Width	= iListWidth
	uRCondListItem.Height	= iListHeight
	For i=0 To p_sMeasCondForPrint.GetUpperBound(0)
		frm.lblStrLen.Text = p_sMeasCondForPrint(i)
		fnFormatDrawStr(frm.lblStrLen, Grp, uRCondListItem.Width, uRCondListItem.Height, pfSize)
		uRCondListItem.Y += pfSize.Y
		Grp.DrawString(frm.lblStrLen.Text, frm.lblStrLen.Font, Brushes.Black, uRCondListItem)
	Next

	'条件データ文字列 描画
	MeasCondValueArrayMakeForPrint()
	uRCondListItem.X		= uRExtWaku.X + iListXStart + iListWidth
	uRCondListItem.Y		= uRExtWaku.Y + iListYStart
	uRCondListItem.Width	= iListWidth
	uRCondListItem.Height	= iListHeight
	Dim dPreSizeY As Double
	For i=0 To p_sMeasCondValForPrint.GetUpperBound(0)
		frm.lblStrLen.Text = p_sMeasCondValForPrint(i)
		fnFormatDrawStr(frm.lblStrLen, Grp, uRCondListItem.Width, uRCondListItem.Height, pfSize)
		If frm.lblStrLen.Text<>""
			uRCondListItem.Y += pfSize.Y
			dPreSizeY = pfSize.Y
		Else
			uRCondListItem.Y += dPreSizeY
		End If
		Grp.DrawString(frm.lblStrLen.Text, frm.lblStrLen.Font, Brushes.Black, uRCondListItem)
	Next


	'グラフ描画
	Dim pGraph As New PictureBox
	Dim cGraph As New Chart

''	Dim dZoom	As Double = 0.42	'拡大率
	Dim dZoom	As Double = 0.40	'拡大率
	Dim stca As ChartArg = Nothing
	Dim iGraphMax As Int16 = 4

	pGraph.SizeMode = PictureBoxSizeMode.StretchImage

	stCa.iThinningType		= 0
	stCa.bHist				= False
	stCa.bStd				= False
	stCa.dYSpanPer			= p_RTM(0).p_dYDispArea
	stCa.dLimitTime			= p_dtFile.p_head.dMeasTimeLimit
	stCa.sCalcMethod		= p_dtFile.p_head.sCalcMethod
	stCa.iGelTimeJudge		= lstGelTimeJudge.IndexOf(p_dtFile.p_head.sGelTimeJudge)

	Dim sMinMax() As String = Nothing
	sMinMax = p_dtFile.p_head.sJudgeRange.Split("-")
	stCa.dJudgeRangeMin		= CType(Trim(sMinMax(0)), Double)
	stCa.dJudgeRangeMax		= CType(Trim(sMinMax(1)), Double)
	sMinMax = p_dtFile.p_head.sMaskTime.Split("-")
	stCa.dAprxRangeMin		= CType(Trim(sMinMax(0)), Double)
	stCa.dAprxRangeMax		= CType(Trim(sMinMax(1)), Double)
	stCa.dGelTime			= p_dtFile.p_head.dGelTime
	stCa.dToqueThreshold	= p_dtFile.p_head.sThreshold
	stCa.dFullStressADVal	= p_dtFile.p_head.sFullCalibValue
	stCa.dMeltTemp			= p_dtFile.p_head.dGelTimeTemp
	stCa.dMeltVisco			= p_dtFile.p_head.dGelTimeVisco
	stCa.dTempMin			= p_RTM(0).p_rtgset.mdbr(0).dHPTemp
	For i=1 To p_RTM(0).p_rtgset.mdbr.GetUpperBound(0)
		If p_RTM(0).p_rtgset.mdbr(i).dHPTemp = 0
			Exit For
		End If
	Next
	stCa.dTempMax			= p_RTM(0).p_rtgset.mdbr(i-1).dHPTemp

	Dim iStartGX	As Int32 = 450
	Dim iStartGXAdd As Int32 = 600
	Dim iStartGY	As Int32 = 50
	Dim iStartGYAdd	As Int32 = 350
	Select Case p_iCategory
	Case 0	'カテゴリ１
		iGraphMax = 4
		For i=iGraphMax - 1 To 0 Step -1
			Select Case i
			Case 0	'応力
				stCa.sSelectGItem		= sGraphSelectionArry(iGrpItmN.rbCorrect)
				stCa.iSelectGItem		= iGrpItmN.rbCorrect
				iStartGXAdd = 0
				iStartGYAdd = 0
			Case 1	'応力％
				stCa.sSelectGItem		= sGraphSelectionArry(iGrpItmN.rbCorrectP)
				stCa.iSelectGItem		= iGrpItmN.rbCorrectP
				iStartGXAdd = 600
				iStartGYAdd = 0
			Case 2	'粘度
				stCa.sSelectGItem		= sGraphSelectionArry(iGrpItmN.rbViscosity)
				stCa.iSelectGItem		= iGrpItmN.rbViscosity
				iStartGXAdd = 0
				iStartGYAdd = 350
			Case 3	'位相
				stCa.sSelectGItem		= sGraphSelectionArry(iGrpItmN.rbPosPhase)
				stCa.iSelectGItem		= iGrpItmN.rbPosPhase
				iStartGXAdd = 600
				iStartGYAdd = 350
			End Select

			'Chart描画
			'RealChartDisp1(stCa, p_dtFile, cGraph, frm.clExtra, p_cts, False)
			Select Case p_cts.LTModeEnable
			Case False
				RealChartDisp1(stCa, p_dtFile, cGraph, frm.clExtra, p_cts, False)
			Case Else
				LTRealChartDisp1(stCa, p_dtFile, cGraph, frm.clExtra, p_cts, False)
			End Select

			Dim img				As New Bitmap(cGraph.Size.Width, cGraph.Size.Height)
			Dim uRPic As New Rectangle(New Point(0, 0), img.Size)
			cGraph.DrawToBitmap(img,  uRPic)
			'トリミング
			Dim cropRect		As New Rectangle(10, 50, img.Size.Width-(0+0), img.Size.Height-(0+0))
			Dim croppedImage	As New Bitmap(cropRect.Width, cropRect.Height)
			Using g As Graphics = Graphics.FromImage(croppedImage)
				g.DrawImage(img, New Rectangle(0, 0, croppedImage.Width, croppedImage.Height), cropRect, GraphicsUnit.Pixel)
				img = croppedImage
			End Using
			pGraph.SizeMode = PictureBoxSizeMode.StretchImage
			pGraph.Image = img
			Dim picSize As New Size(cGraph.Size.Width*dZoom, cGraph.Size.Height*dZoom)
			Grp.DrawImage(pGraph.Image, iStartGX+iStartGXAdd, iStartGY+iStartGYAdd,	picsize.Width, picSize.Height)
		Next i

	Case 1	'カテゴリ2
		iGraphMax = 6
		For i=iGraphMax - 1 To 0 Step -1
			Select Case i
			Case 0	'せん断弾性率
				stCa.sSelectGItem		= sGraphSelectionArry(iGrpItmN.rbShearMod)
				stCa.iSelectGItem		= iGrpItmN.rbShearMod
				iStartGXAdd = 0
				iStartGYAdd = 0
			Case 1	'貯蔵弾性率
				stCa.sSelectGItem		= sGraphSelectionArry(iGrpItmN.rbStorageMod)
				stCa.iSelectGItem		= iGrpItmN.rbStorageMod
				iStartGXAdd = 600
				iStartGYAdd = 0
			Case 2	'損失弾性率
				stCa.sSelectGItem		= sGraphSelectionArry(iGrpItmN.rbElasticMod)
				stCa.iSelectGItem		= iGrpItmN.rbElasticMod
				iStartGXAdd = 0
				iStartGYAdd = 350
			Case 3	'損失正接
				stCa.sSelectGItem		= sGraphSelectionArry(iGrpItmN.rbLossTangent)
				stCa.iSelectGItem		= iGrpItmN.rbLossTangent
				iStartGXAdd = 600
				iStartGYAdd = 350
			Case 4	'サンプル歪
				stCa.sSelectGItem		= sGraphSelectionArry(iGrpItmN.rbSampleStrain)
				stCa.iSelectGItem		= iGrpItmN.rbSampleStrain
				iStartGXAdd = 0
				iStartGYAdd = 350 * 2
			Case 5	'せん断速度
				stCa.sSelectGItem		= sGraphSelectionArry(iGrpItmN.rbShearVelo)
				stCa.iSelectGItem		= iGrpItmN.rbShearMod
				iStartGXAdd = 600
				iStartGYAdd = 350 * 2
			End Select

			'Chart描画
			'RealChartDisp1(stCa, p_dtFile, cGraph, frm.clExtra, p_cts, False)
			Select Case p_cts.LTModeEnable
			Case False
				RealChartDisp1(stCa, p_dtFile, cGraph, frm.clExtra, p_cts, False)
			Case Else
				LTRealChartDisp1(stCa, p_dtFile, cGraph, frm.clExtra, p_cts, False)
			End Select

			Dim img				As New Bitmap(cGraph.Size.Width, cGraph.Size.Height)
			Dim uRPic As New Rectangle(New Point(0, 0), img.Size)
			cGraph.DrawToBitmap(img,  uRPic)
			'トリミング
			Dim cropRect		As New Rectangle(10, 50, img.Size.Width-(0+0), img.Size.Height-(0+0))
			Dim croppedImage	As New Bitmap(cropRect.Width, cropRect.Height)
			Using g As Graphics = Graphics.FromImage(croppedImage)
				g.DrawImage(img, New Rectangle(0, 0, croppedImage.Width, croppedImage.Height), cropRect, GraphicsUnit.Pixel)
				img = croppedImage
			End Using
			pGraph.Image = img
			Dim picSize As New Size(cGraph.Size.Width*dZoom, cGraph.Size.Height*dZoom)
			Grp.DrawImage(pGraph.Image, iStartGX+iStartGXAdd, iStartGY+iStartGYAdd,	picsize.Width, picSize.Height)
		Next i
	Case 2	'カテゴリ3
		iGraphMax = 4
		For i=iGraphMax - 1 To 0 Step -1
			Select Case i
			Case 0	'X応力
				stCa.sSelectGItem		= sGraphSelectionArry(iGrpItmN.rbXStress)
				stCa.iSelectGItem		= iGrpItmN.rbXStress
				iStartGXAdd = 0
				iStartGYAdd = 0
			Case 1	'Y応力
				stCa.sSelectGItem		= sGraphSelectionArry(iGrpItmN.rbYStress)
				stCa.iSelectGItem		= iGrpItmN.rbYStress
				iStartGXAdd = 0
				iStartGYAdd = 350
			Case 2	'Z応力
				stCa.sSelectGItem		= sGraphSelectionArry(iGrpItmN.rbZStress)
				stCa.iSelectGItem		= iGrpItmN.rbZStress
				iStartGXAdd = 0
				iStartGYAdd = 350 * 2
			Case 3	'Z位置
				stCa.sSelectGItem		= sGraphSelectionArry(iGrpItmN.rbZPosition)
				stCa.iSelectGItem		= iGrpItmN.rbZPosition
				iStartGXAdd = 600
				iStartGYAdd = 0
			End Select

			'Chart描画
			'RealChartDisp1(stCa, p_dtFile, cGraph, frm.clExtra, p_cts, False)
			Select Case p_cts.LTModeEnable
			Case False
				RealChartDisp1(stCa, p_dtFile, cGraph, frm.clExtra, p_cts, False)
			Case Else
				LTRealChartDisp1(stCa, p_dtFile, cGraph, frm.clExtra, p_cts, False)
			End Select

			Dim img				As New Bitmap(cGraph.Size.Width, cGraph.Size.Height)
			Dim uRPic As New Rectangle(New Point(0, 0), img.Size)
			cGraph.DrawToBitmap(img,  uRPic)
			'トリミング
			Dim cropRect		As New Rectangle(10, 50, img.Size.Width-(0+0), img.Size.Height-(0+0))
			Dim croppedImage	As New Bitmap(cropRect.Width, cropRect.Height)
			Using g As Graphics = Graphics.FromImage(croppedImage)
				g.DrawImage(img, New Rectangle(0, 0, croppedImage.Width, croppedImage.Height), cropRect, GraphicsUnit.Pixel)
				img = croppedImage
			End Using
			pGraph.Image = img
			Dim picSize As New Size(cGraph.Size.Width*dZoom, cGraph.Size.Height*dZoom)
			Grp.DrawImage(pGraph.Image, iStartGX+iStartGXAdd, iStartGY+iStartGYAdd,	picsize.Width, picSize.Height)
		Next i
	Case 3	'カテゴリ4
		iGraphMax = 4
		For i=iGraphMax - 1 To 0 Step -1
			Select Case i
			Case 0	'HP温度
				stCa.sSelectGItem		= sGraphSelectionArry(iGrpItmN.rbHPTemp)
				stCa.iSelectGItem		= iGrpItmN.rbHPTemp
				iStartGXAdd = 0
				iStartGYAdd = 0
			Case 1	'PB温度
				stCa.sSelectGItem		= sGraphSelectionArry(iGrpItmN.rbPBTemp)
				stCa.iSelectGItem		= iGrpItmN.rbPBTemp
				iStartGXAdd = 600
				iStartGYAdd = 0
			Case 2	'HPMV
				stCa.sSelectGItem		= sGraphSelectionArry(iGrpItmN.rbHPMV)
				stCa.iSelectGItem		= iGrpItmN.rbHPMV
				iStartGXAdd = 0
				iStartGYAdd = 350
			Case 3	'PBMV
				stCa.sSelectGItem		= sGraphSelectionArry(iGrpItmN.rbPBMV)
				stCa.iSelectGItem		= iGrpItmN.rbPBMV
				iStartGXAdd = 600
				iStartGYAdd = 350
			End Select

			'Chart描画
			'RealChartDisp1(stCa, p_dtFile, cGraph, frm.clExtra, p_cts, False)
			Select Case p_cts.LTModeEnable
			Case False
				RealChartDisp1(stCa, p_dtFile, cGraph, frm.clExtra, p_cts, False)
			Case Else
				LTRealChartDisp1(stCa, p_dtFile, cGraph, frm.clExtra, p_cts, False)
			End Select

			Dim img				As New Bitmap(cGraph.Size.Width, cGraph.Size.Height)
			Dim uRPic			As New Rectangle(New Point(0, 0), img.Size)
			cGraph.DrawToBitmap(img,  uRPic)
			'トリミング
			Dim cropRect		As New Rectangle(10, 50, img.Size.Width-(0+0), img.Size.Height-(0+0))
			Dim croppedImage	As New Bitmap(cropRect.Width, cropRect.Height)
			Using g As Graphics = Graphics.FromImage(croppedImage)
				g.DrawImage(img, New Rectangle(0, 0, croppedImage.Width, croppedImage.Height), cropRect, GraphicsUnit.Pixel)
				img = croppedImage
			End Using
			pGraph.Image = img
			Dim picSize As New Size(cGraph.Size.Width*dZoom, cGraph.Size.Height*dZoom)
			Grp.DrawImage(pGraph.Image, iStartGX+iStartGXAdd, iStartGY+iStartGYAdd,	picsize.Width, picSize.Height)
		Next i
	End Select

Catch ex As Exception
    Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
    Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub

''' <summary>
''' 文字列描画サイズの判定、設定
''' </summary>
''' <param name="lblStr">文字列のフォント、テキストを指定したラベル</param>
''' <param name="gr">描画するグラフィックオブジェクト</param>
''' <param name="sngWidth">指定幅</param>
''' <param name="sngHeight">指定高さ</param>
''' <param name="pfSize">設定後の文字列サイズ</param>
''' <returns>True:設定完了 False:異常</returns>
Private Function fnFormatDrawStr(ByRef lblStr As Label, ByVal gr As Graphics, ByVal sngWidth As Single, ByVal sngHeight As Single, Optional ByRef pfSize As PointF = Nothing) As Boolean
    Dim bRtn As Boolean = True
    Dim iFntSize As Integer
    Dim sTemp As String
Try
    '現在値を取得
    iFntSize = lblStr.Font.Size
    sTemp = lblStr.Text

    '文字描画サイズの高さの範囲判定
    While iFntSize > 0
        '文字列描画のサイズ取得
        pfSize = gr.MeasureString(lblStr.Text, lblStr.Font)

        '指定の高さより大きい
        If pfSize.Y > sngHeight Then
            'フォントサイズの変更
            iFntSize = iFntSize - 1
            lblStr.Font = New Font(lblStr.Font.Name, iFntSize)
        Else
            Exit While
        End If
    End While

    'フォントサイズ設定後、横幅の範囲判定
    If iFntSize > 0 Then
        While sTemp.Length > 0
            '文字列描画のサイズ取得
            pfSize = gr.MeasureString(lblStr.Text, lblStr.Font)

            '指定の幅より大きい
            If pfSize.X > sngWidth Then
                '文字列の変更
                sTemp = Strings.Left(sTemp, sTemp.Length - 1)
                lblStr.Text = sTemp & "..."
            Else
                Exit While
            End If
        End While

        '入りきらなかった場合
        If sTemp.Length = 0 Then
            bRtn = False
            lblStr.Text = ""
        End If
    Else
        '入りきらなかった場合
        bRtn = False
        lblStr.Text = ""
    End If

Catch ex As Exception
    Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
    Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
    bRtn = False
End Try

    Return bRtn
End Function

End Module
