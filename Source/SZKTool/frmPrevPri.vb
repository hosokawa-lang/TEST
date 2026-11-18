'''<summary>
'''印刷内容の設定を行うフォーム。
'''</summary>
Public Class frmPrevPri


    '''<summary> コンストラクタ。 </summary>
    '''<param name="strVal">コメント欄の初期値。</param>
	Public Sub New(ByVal strVal As String)
		InitializeComponent()
		txtComment.Text = strVal
		Me.ActiveControl = txtMeasPlace
	End Sub

    Private Sub frmPrevPri_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            '言語により表示を変更
            Call SetFormLanguage(Me, p_iLanguage)

        Catch ex As Exception
            Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
            Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
        End Try
    End Sub


    '''<summary> 印刷用コメントの入力値を取得する。 </summary>
	Public Function GetPComment() As String
		Return txtComment.Text
	End Function

	'''<summary> 測定場所の入力値を取得する。 </summary>
	Public Function GetPMeasPlace() As String
		Return txtMeasPlace.Text
	End Function

	'''<summary> 測定者の入力値を取得する。 </summary>
	Public Function GetPMeasurer() As String
		Return txtMeasurer.Text
	End Function

	'''<summary> 印刷用コメントの入力値を取得する。 </summary>
	Public Sub SetPComment(ByVal sComment As String)
		txtComment.Text = sComment
	End Sub

	'''<summary> 測定場所の入力値を取得する。 </summary>
	Public Sub SetPMeasPlace(ByVal sPlace As String)
		txtMeasPlace.Text = sPlace
	End Sub

	'''<summary> 測定者の入力値を取得する。 </summary>
	Public Sub SetPMeasurer(ByVal sParson As String)
		txtMeasurer.Text = sParson
	End Sub


    Private Sub btnOK_Click(sender As Object, e As EventArgs) Handles btnOK.Click
        Try
            Me.DialogResult = DialogResult.OK
            Me.Close()

        Catch ex As Exception
            Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
            Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
        End Try
    End Sub

End Class