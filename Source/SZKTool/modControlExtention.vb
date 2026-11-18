Imports System.Runtime.CompilerServices
Module ControlExtentions
''' <summary>
''' 
''' </summary>
''' <param name="self"></param>
''' <param name="value"></param>
''' <param name="iColor"></param>
<Extension()> Public Sub SetTextEx(ByVal self As Control, ByVal value As String, ByVal iColor As Color)
Try
        ' コントロールが破棄されたのであれば、そのコントロールにアクセスするのは危険なので、処理を止める
        If Not self.IsHandleCreated Then
            'Exit Sub
        End If

        ' わかるならこっち
        'self.Invoke(Sub() self.Text = value)

        ' よくわかんない場合はこっち
        ' InvokeRequired が True の場合(=別スレッドの中にいるので)、Invoke() してもらって、
        ' もう一回 UI スレッドで SetText() を実行するようにお願いする
        ' UI スレッドにいる場合は、いつも通りにアクセスしても大丈夫
        Dim ac As Action(Of Control, String, Color) = AddressOf SetTextEx
        If self.InvokeRequired Then
            self.Invoke(ac, New Object() {self, value, iColor})
        Else
            self.Text		= value
			self.BackColor	= iColor
        End If
Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub
End Module
