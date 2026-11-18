Imports System.IO

Public NotInheritable Class frmAbout
	Public p_TitlePart	As String = My.Application.Info.ProductName		'製品名

Private Sub frmAbout_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
	' フォームのタイトルを設定します。
	Dim ApplicationTitle As String = "SIZUKA Tool"
	Me.Text = String.Format("About {0}", ApplicationTitle)

	' バージョン情報ボックスに表示されたテキストをすべて初期化します。
	' TODO: [プロジェクト] メニューの下にある [プロジェクト プロパティ] ダイアログの [アプリケーション] ペインで、アプリケーションのアセンブリ情報を 
	'    カスタマイズします。
	Me.LabelProductName.Text	= p_TitlePart

	Dim s As String = GetAppVersion(Path.Combine(Application.StartupPath, "SZKAutoRegist.exe"))
	Me.lblToolVersion.Text		= space(4) & String.Format("PC Tool   Version : {0}  ({1})", My.Application.Info.Version.ToString, s)

	Dim sCPU0Ver As String = p_RTM(0).p_mltcp.DevSoftVer.ToString("F2")
	Dim sCPU1Ver As String = p_RTM(0).p_mltcp.ComSoftVer.ToString("F2")
	Me.lblMainContVer.Text		= space(4) & 				"Cont. CPU Version : "	& If(sCPU0Ver="0.00", "----",sCPU0Ver)
	Me.lblMainComVer.Text		= space(4) &				"Com. CPU Version : "	& If(sCPU1Ver="0.00", "----",sCPU1Ver)
	Me.LabelCopyright.Text		= space(4) & My.Application.Info.Copyright
	Me.LabelCompanyName.Text	= space(4) & My.Application.Info.CompanyName
	Me.TextBoxDescription.Text	= My.Application.Info.Description
End Sub

Private Sub OKButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OKButton.Click
	Me.Close()
End Sub

End Class
