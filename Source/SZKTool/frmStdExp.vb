Imports System
Imports System.IO
Public Class frmStdExp
Public Sub New(ByVal sF As String)
	' この呼び出しはデザイナーで必要です。
	InitializeComponent()
	' InitializeComponent() 呼び出しの後で初期化を追加します。
	sFileName = sF

End Sub
''' <summary>
''' 設定ファイル名
''' </summary>
Dim sFileName As String
Public Property FileName As String
	Get
		FileName = sFileName
	End Get
	Set(value As String)
		sFileName = value
	End Set
End Property
Dim invalidChar As Char() = Path.GetInvalidPathChars()
''' <summary>
''' フォームロード
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub frmStdExp_Load(sender As Object, e As EventArgs) Handles Me.Load
	tbxFilename.Text = sFileName
End Sub
''' <summary>
''' OKボタン
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub btnOk_Click(sender As Object, e As EventArgs) Handles btnOk.Click
	If tbxFilename.Text.IndexOfAny(invalidChar) < 0
	'未使用
		sFileName = tbxFilename.Text
		
		Me.DialogResult = DialogResult.OK
		Call WriteLog("基準ファイルエクスポート - " & sFileName, eLOG_LEVEL.iINFO)

	Else
	'ファイル名に使用出来ない文字あり
		tbxFilename.BackColor = Color.Yellow
	End If
End Sub
''' <summary>
''' キャンセルボタン
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
	Me.DialogResult = DialogResult.Cancel
End Sub
End Class