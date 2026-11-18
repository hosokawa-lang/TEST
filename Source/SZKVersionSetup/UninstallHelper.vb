Imports System.Diagnostics
Imports System.IO
Imports System.Text.RegularExpressions
Imports Microsoft.Win32

Module UninstallHelper

''' <summary>
''' インストールされているアプリケーション名からGUIDを取得します（完全一致）
''' </summary>
''' <param name="appName">アンインストール対象のアプリ名</param>
''' <returns>GUID文字列（見つからなければNothing）</returns>
Public Function GetInstalledAppGuidByDisplayName(appName As String) As String
    Dim uninstallPath As String = "SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall"
    Dim guidPattern As New Regex("\{[0-9A-Fa-f\-]{36}\}")

    ' 64bit / 32bit 両方から検索
    Dim views As RegistryView() = {RegistryView.Registry64, RegistryView.Registry32}

    For Each view In views
        Using baseKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, view).OpenSubKey(uninstallPath)
            If baseKey Is Nothing Then Continue For

            For Each subKeyName In baseKey.GetSubKeyNames()
                Using subKey = baseKey.OpenSubKey(subKeyName)
                    If subKey Is Nothing Then Continue For

                    Dim displayName = TryCast(subKey.GetValue("DisplayName"), String)
                    If displayName Is Nothing Then Continue For

                    If String.Equals(displayName.Trim(), appName.Trim(), StringComparison.OrdinalIgnoreCase) Then
                        ' UninstallString 内の GUID
                        Dim uninstallString = TryCast(subKey.GetValue("UninstallString"), String)
                        If uninstallString IsNot Nothing Then
                            Dim m = guidPattern.Match(uninstallString)
                            If m.Success Then Return m.Value
                        End If

                        ' サブキー名が GUID の場合
                        If guidPattern.IsMatch(subKeyName) Then
                            Return subKeyName
                        End If
                    End If
                End Using
            Next
        End Using
    Next

    Return Nothing
End Function
''' <summary>
''' GUIDからアプリケーションをアンインストールします（サイレント）
''' </summary>
''' <param name="guid">アンインストール対象のGUID</param>
''' <param name="bQn">False:UIあり True:UIなし</param>
''' <returns>成功すればTrue、失敗すればFalse</returns>
Public Function UninstallApplicationByGuid(guid As String, ByVal bQn As Boolean) As Boolean
    If String.IsNullOrWhiteSpace(guid) Then Return False

    Try
		Dim sQn As String = If(bQn, "/qn", "")
        Dim psi As New ProcessStartInfo()
        psi.FileName = "msiexec.exe"
        psi.Arguments = $"/x {guid} {sQn} /norestart"

        ' CMD と同じ動作にするため
        psi.UseShellExecute = True      ' 重要：False では環境が変わることがある
        psi.CreateNoWindow = False      ' CMD 版に近い挙動
        psi.Verb = "runas"              ' 管理者権限で実行

        Dim proc = Process.Start(psi)
        proc.WaitForExit()              ' CMD 版のように終了まで待つ

        Return proc.ExitCode = 0
    Catch ex As Exception
        Console.WriteLine("アンインストールエラー: " & ex.Message)
        Return False
    End Try
End Function

''' <summary>
''' アプリケーションをインストールします
''' </summary>
''' <param name="sMsiFilePath">msiファイルへのフルパス</param>
''' <param name="sInstallPath">インストールパス</param>
''' <param name="bQn">False:UIあり True:UIなし</param>
''' <returns>成功すればTrue、失敗すればFalse</returns>
Public Function InstallApplication(ByVal sMsiFilePath As String, ByVal sInstallPath As String, ByVal bQn As Boolean) As Boolean
Try
	'Dim tempMsi = Path.Combine(Path.GetTempPath(), Path.GetFileName( sMsiFilePath ))
	'File.Copy(sMsiFilePath, tempMsi, True)

	Dim sQn As String = If(bQn, "/qn", "")
    Dim psi As New ProcessStartInfo()
    psi.FileName = "msiexec.exe"
	psi.Arguments =
    $"/i ""{sMsiFilePath}"" TARGETDIR=""{sInstallPath}"" {sQn} /norestart /L*V C:\SZKApps\Install.log"
    ''psi.Arguments = $"/i ""{sMsiFilePath}"" /qn /norestart  /L*V C:\SZKApps\Install.log"
    psi.UseShellExecute = True
    psi.CreateNoWindow = True
	psi.Verb = "runas"				'管理者権限で実行

    Dim proc = Process.Start(psi)
    proc.WaitForExit()              ' CMD 版のように終了まで待つ
    Return True

Catch ex As Exception
    Console.WriteLine("インストール実行エラー: " & ex.Message)
    Return False
End Try
End Function
''' <summary>
''' ファイルのバージョン情報を取得
''' </summary>
''' <param name="sExePath">ファイルフルパス</param>
''' <returns></returns>
Public Function GetExeFileInfo(ByVal sExePath As String) As System.Diagnostics.FileVersionInfo
Try
Dim vi As System.Diagnostics.FileVersionInfo = System.Diagnostics.FileVersionInfo.GetVersionInfo( sExePath )
	Return vi
Catch ex As Exception
    Return Nothing
End Try
End Function

End Module
