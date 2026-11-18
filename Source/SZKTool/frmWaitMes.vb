Public Class frmWaitMes
Public p_sMes		As String	= Nothing				'メッセージ
Public p_FColor		As Color	= Color.Black			'文字色
Public p_BkColor	As Color	= Color.Transparent		'背景色
Sub New()
    ' この呼び出しはデザイナーで必要です。
    InitializeComponent()
    ' InitializeComponent() 呼び出しの後で初期化を追加します。
End Sub
Private Sub frmWaitMes_Load(sender As Object, e As EventArgs) Handles MyBase.Load
	lblMessage.Text			= p_sMes
	lblMessage.ForeColor	= p_FColor
	lblMessage.BackColor	= p_BkColor
End Sub
End Class