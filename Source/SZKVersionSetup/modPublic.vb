Imports System.IO
Imports System.Security.AccessControl

Module modPublic
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
Public Const APP_Path_Old		As String	= "C:\Program Files\Cyber"
Public Const APP_Path			As String	= "C:\SZKApps"
Public Const SZK_MES_Path		As String	= "Sizuka Tool"
Public Const SZK_ANA_Path		As String	= "Sizuka Data Manager"
Public Const APPVersions_Path	As String	= "C:\SZKApps\Versions"
Public Const SizukaToolAPPName	As String	= "Sizuka Tool"
Public Const SizukaAnaAPPName	As String	= "Sizuka Data Manager"
Public Const SizukaToolEXEName	As String	= "SZKTool.exe"
Public Const SizukaAnaEXEName	As String	= "SZKAnaTool.exe"
Public Const SizukaToolMSIName	As String	= "SizukaSetup.msi"
Public Const SizukaAnaMSIName	As String	= "SizukaAnaSetup.msi"
Public Const SETUPFILES			As String	= "SetupFiles"
Public Const DISC_FILE			As String	= "Discription.txt"
Public Const UnInstallerInfo	As String	= "AppUninstallerInfo.ps1"
Public Const SZKToolUnInstalBat	As String	= "SizukaToolVersionUp.bat"
Public Const SZKAnaUnInstalBat	As String	= "SizukaAnaVersionUp.bat"
Public Const MSDotNet481Folder	As String	= "DotNetFX481"
Public Const MSSetupEXE			As String	= "setup.exe"
Public Const SizukaToolMSI		As String	= "SizukaSetup.msi"
Public Const SizukaAnaMSI		As String	= "SizukaAnaSetup.msi"
Public Const SizukaToolINI		As String	= "Devicelist.ini"
Public Const SizukaAnaINI		As String	= "AppAna.ini"


Public p_sLOG_LEVEL() As String = {"DEBUG", "INFO", "WARNING", "FATAL"}
Enum eLOG_LEVEL
    iDEBUG = 0          'デバッグ出力
    iINFO               'Information
    iWARNING            '警告
    iFATAL              '異常
End Enum
''' <summary>基本フォント</summary>
Public Const BASE_FONT					As String = "Meiryo UI"

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
		sPath = SIZUKA_PATH & "\" & LOGdat_PATH & "\" & "SizukaSetup" & Format(Now, "yyyyMMdd") & ".Log"
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

	Catch ex As Exception
		Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	End Try
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


End Module


'初期化ファイル簡易読込みクラス
Public Class IniFile
Private _data As New Dictionary(Of String, Dictionary(Of String, String))
Public Sub Load(path As String)
    Dim currentSection As String = ""
    For Each line As String In File.ReadAllLines(path)
        line = line.Trim()
        ' 空行・コメント除外
        If line = "" OrElse line.StartsWith(";") Then
            Continue For
        End If
        ' セクション
        If line.StartsWith("[") AndAlso line.EndsWith("]") Then
            currentSection = line.Substring(1, line.Length - 2)
            If Not _data.ContainsKey(currentSection) Then
                _data(currentSection) = New Dictionary(Of String, String)
            End If
            Continue For
        End If
        ' Key=Value
        Dim pos As Integer = line.IndexOf("=")
        If pos > 0 Then
            Dim key As String = line.Substring(0, pos).Trim()
            Dim value As String = line.Substring(pos + 1).Trim()
            If currentSection <> "" Then
                _data(currentSection)(key) = value
            End If
        End If
    Next
End Sub

Public Function ReadValue(section As String, key As String, Optional defaultValue As String = "") As String
	If _data.ContainsKey(section) Then
        If _data(section).ContainsKey(key) Then
            Return _data(section)(key)
        End If
    End If
    Return defaultValue
End Function

End Class


