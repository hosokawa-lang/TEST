'''<summary>
'''初回起動時の設定画面。
'''</summary>
Public Class frmWelcome
'多言語メッセージ格納
Private m_sStr(eMes.iMAX - 1)		 As String   
Enum eMes
	iMAX
End Enum

Private Sub frmWelcome_Load(sender As Object, e As EventArgs) Handles MyBase.Load
Try
	'言語切替
	Call SetFormLanguage(Me, p_iLanguage, ,m_sStr)

Catch ex As Exception
    Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
    Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub

Private Sub frmWelcome_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
	'e.Cancel = True
End Sub

Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
Try
    Close()

Catch ex As Exception
    Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
    Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub
Public Function GetIP() As String
		Return "192.168.10." & nudIP.Text
	End Function
Public Function GetPort() As String
    Return txtPort.Text
End Function


End Class