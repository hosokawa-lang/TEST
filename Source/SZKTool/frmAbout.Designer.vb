<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmAbout
	Inherits System.Windows.Forms.Form

	'フォームがコンポーネントの一覧をクリーンアップするために dispose をオーバーライドします。
	<System.Diagnostics.DebuggerNonUserCode()> _
	Protected Overrides Sub Dispose(ByVal disposing As Boolean)
		Try
			If disposing AndAlso components IsNot Nothing Then
				components.Dispose()
			End If
		Finally
			MyBase.Dispose(disposing)
		End Try
	End Sub

	Friend WithEvents TableLayoutPanel As System.Windows.Forms.TableLayoutPanel
	Friend WithEvents LabelProductName As System.Windows.Forms.Label
	Friend WithEvents lblToolVersion As System.Windows.Forms.Label
	Friend WithEvents TextBoxDescription As System.Windows.Forms.TextBox
	Friend WithEvents OKButton As System.Windows.Forms.Button
	Friend WithEvents LabelCopyright As System.Windows.Forms.Label

	'Windows フォーム デザイナーで必要です。
	Private components As System.ComponentModel.IContainer

	'メモ: 以下のプロシージャは Windows フォーム デザイナーで必要です。
	'Windows フォーム デザイナーを使用して変更できます。  
	'コード エディターを使って変更しないでください。
	<System.Diagnostics.DebuggerStepThrough()> _
	Private Sub InitializeComponent()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmAbout))
		Me.TableLayoutPanel = New System.Windows.Forms.TableLayoutPanel()
		Me.lblMainComVer = New System.Windows.Forms.Label()
		Me.lblMainContVer = New System.Windows.Forms.Label()
		Me.LogoPictureBox = New System.Windows.Forms.PictureBox()
		Me.LabelProductName = New System.Windows.Forms.Label()
		Me.lblToolVersion = New System.Windows.Forms.Label()
		Me.LabelCopyright = New System.Windows.Forms.Label()
		Me.LabelCompanyName = New System.Windows.Forms.Label()
		Me.TextBoxDescription = New System.Windows.Forms.TextBox()
		Me.OKButton = New System.Windows.Forms.Button()
		Me.TableLayoutPanel.SuspendLayout
		CType(Me.LogoPictureBox,System.ComponentModel.ISupportInitialize).BeginInit
		Me.SuspendLayout
		'
		'TableLayoutPanel
		'
		Me.TableLayoutPanel.ColumnCount = 2
		Me.TableLayoutPanel.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 32.87197!))
		Me.TableLayoutPanel.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 67.12803!))
		Me.TableLayoutPanel.Controls.Add(Me.lblMainComVer, 1, 3)
		Me.TableLayoutPanel.Controls.Add(Me.lblMainContVer, 1, 2)
		Me.TableLayoutPanel.Controls.Add(Me.LogoPictureBox, 0, 0)
		Me.TableLayoutPanel.Controls.Add(Me.LabelProductName, 1, 0)
		Me.TableLayoutPanel.Controls.Add(Me.lblToolVersion, 1, 1)
		Me.TableLayoutPanel.Controls.Add(Me.LabelCopyright, 1, 4)
		Me.TableLayoutPanel.Controls.Add(Me.LabelCompanyName, 1, 5)
		Me.TableLayoutPanel.Controls.Add(Me.TextBoxDescription, 1, 6)
		Me.TableLayoutPanel.Controls.Add(Me.OKButton, 1, 7)
		Me.TableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill
		Me.TableLayoutPanel.Location = New System.Drawing.Point(9, 8)
		Me.TableLayoutPanel.Name = "TableLayoutPanel"
		Me.TableLayoutPanel.RowCount = 8
		Me.TableLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 18.18182!))
		Me.TableLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.57754!))
		Me.TableLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 17.64706!))
		Me.TableLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 17.1123!))
		Me.TableLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 13.36898!))
		Me.TableLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 17.1123!))
		Me.TableLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 151!))
		Me.TableLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32!))
		Me.TableLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20!))
		Me.TableLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20!))
		Me.TableLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20!))
		Me.TableLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20!))
		Me.TableLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20!))
		Me.TableLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20!))
		Me.TableLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20!))
		Me.TableLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20!))
		Me.TableLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20!))
		Me.TableLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20!))
		Me.TableLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20!))
		Me.TableLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20!))
		Me.TableLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20!))
		Me.TableLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20!))
		Me.TableLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20!))
		Me.TableLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20!))
		Me.TableLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20!))
		Me.TableLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20!))
		Me.TableLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20!))
		Me.TableLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20!))
		Me.TableLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20!))
		Me.TableLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20!))
		Me.TableLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20!))
		Me.TableLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20!))
		Me.TableLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20!))
		Me.TableLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20!))
		Me.TableLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20!))
		Me.TableLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20!))
		Me.TableLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20!))
		Me.TableLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20!))
		Me.TableLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20!))
		Me.TableLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20!))
		Me.TableLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20!))
		Me.TableLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20!))
		Me.TableLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20!))
		Me.TableLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20!))
		Me.TableLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20!))
		Me.TableLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20!))
		Me.TableLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20!))
		Me.TableLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20!))
		Me.TableLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20!))
		Me.TableLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20!))
		Me.TableLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20!))
		Me.TableLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20!))
		Me.TableLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20!))
		Me.TableLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20!))
		Me.TableLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20!))
		Me.TableLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20!))
		Me.TableLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20!))
		Me.TableLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20!))
		Me.TableLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20!))
		Me.TableLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20!))
		Me.TableLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20!))
		Me.TableLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20!))
		Me.TableLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20!))
		Me.TableLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20!))
		Me.TableLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20!))
		Me.TableLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20!))
		Me.TableLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20!))
		Me.TableLayoutPanel.Size = New System.Drawing.Size(490, 322)
		Me.TableLayoutPanel.TabIndex = 0
		'
		'lblMainComVer
		'
		Me.lblMainComVer.Dock = System.Windows.Forms.DockStyle.Fill
		Me.lblMainComVer.Font = New System.Drawing.Font("Meiryo UI", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.lblMainComVer.Location = New System.Drawing.Point(167, 72)
		Me.lblMainComVer.Margin = New System.Windows.Forms.Padding(6, 0, 3, 0)
		Me.lblMainComVer.MaximumSize = New System.Drawing.Size(0, 24)
		Me.lblMainComVer.Name = "lblMainComVer"
		Me.lblMainComVer.Size = New System.Drawing.Size(320, 23)
		Me.lblMainComVer.TabIndex = 2
		Me.lblMainComVer.Text = "本体通信部バージョン"
		Me.lblMainComVer.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
		'
		'lblMainContVer
		'
		Me.lblMainContVer.Dock = System.Windows.Forms.DockStyle.Fill
		Me.lblMainContVer.Font = New System.Drawing.Font("Meiryo UI", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.lblMainContVer.Location = New System.Drawing.Point(167, 48)
		Me.lblMainContVer.Margin = New System.Windows.Forms.Padding(6, 0, 3, 0)
		Me.lblMainContVer.MaximumSize = New System.Drawing.Size(0, 24)
		Me.lblMainContVer.Name = "lblMainContVer"
		Me.lblMainContVer.Size = New System.Drawing.Size(320, 24)
		Me.lblMainContVer.TabIndex = 1
		Me.lblMainContVer.Text = "本体制御部バージョン"
		Me.lblMainContVer.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
		'
		'LogoPictureBox
		'
		Me.LogoPictureBox.Dock = System.Windows.Forms.DockStyle.Fill
		Me.LogoPictureBox.Image = CType(resources.GetObject("LogoPictureBox.Image"),System.Drawing.Image)
		Me.LogoPictureBox.Location = New System.Drawing.Point(3, 3)
		Me.LogoPictureBox.Name = "LogoPictureBox"
		Me.TableLayoutPanel.SetRowSpan(Me.LogoPictureBox, 8)
		Me.LogoPictureBox.Size = New System.Drawing.Size(155, 316)
		Me.LogoPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
		Me.LogoPictureBox.TabIndex = 0
		Me.LogoPictureBox.TabStop = false
		'
		'LabelProductName
		'
		Me.LabelProductName.Dock = System.Windows.Forms.DockStyle.Fill
		Me.LabelProductName.Font = New System.Drawing.Font("Meiryo UI", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.LabelProductName.Location = New System.Drawing.Point(167, 0)
		Me.LabelProductName.Margin = New System.Windows.Forms.Padding(6, 0, 3, 0)
		Me.LabelProductName.MaximumSize = New System.Drawing.Size(0, 24)
		Me.LabelProductName.Name = "LabelProductName"
		Me.LabelProductName.Size = New System.Drawing.Size(320, 24)
		Me.LabelProductName.TabIndex = 0
		Me.LabelProductName.Text = "製品名"
		Me.LabelProductName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
		'
		'lblToolVersion
		'
		Me.lblToolVersion.Dock = System.Windows.Forms.DockStyle.Fill
		Me.lblToolVersion.Font = New System.Drawing.Font("Meiryo UI", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.lblToolVersion.Location = New System.Drawing.Point(167, 25)
		Me.lblToolVersion.Margin = New System.Windows.Forms.Padding(6, 0, 3, 0)
		Me.lblToolVersion.MaximumSize = New System.Drawing.Size(0, 24)
		Me.lblToolVersion.Name = "lblToolVersion"
		Me.lblToolVersion.Size = New System.Drawing.Size(320, 23)
		Me.lblToolVersion.TabIndex = 0
		Me.lblToolVersion.Text = "ツールバージョン"
		Me.lblToolVersion.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
		'
		'LabelCopyright
		'
		Me.LabelCopyright.Dock = System.Windows.Forms.DockStyle.Fill
		Me.LabelCopyright.Font = New System.Drawing.Font("Meiryo UI", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.LabelCopyright.Location = New System.Drawing.Point(167, 95)
		Me.LabelCopyright.Margin = New System.Windows.Forms.Padding(6, 0, 3, 0)
		Me.LabelCopyright.MaximumSize = New System.Drawing.Size(0, 24)
		Me.LabelCopyright.Name = "LabelCopyright"
		Me.LabelCopyright.Size = New System.Drawing.Size(320, 18)
		Me.LabelCopyright.TabIndex = 0
		Me.LabelCopyright.Text = "著作権"
		Me.LabelCopyright.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
		'
		'LabelCompanyName
		'
		Me.LabelCompanyName.Dock = System.Windows.Forms.DockStyle.Fill
		Me.LabelCompanyName.Font = New System.Drawing.Font("Meiryo UI", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.LabelCompanyName.Location = New System.Drawing.Point(167, 113)
		Me.LabelCompanyName.Margin = New System.Windows.Forms.Padding(6, 0, 3, 0)
		Me.LabelCompanyName.MaximumSize = New System.Drawing.Size(0, 24)
		Me.LabelCompanyName.Name = "LabelCompanyName"
		Me.LabelCompanyName.Size = New System.Drawing.Size(320, 23)
		Me.LabelCompanyName.TabIndex = 0
		Me.LabelCompanyName.Text = "会社名"
		Me.LabelCompanyName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
		'
		'TextBoxDescription
		'
		Me.TextBoxDescription.Dock = System.Windows.Forms.DockStyle.Fill
		Me.TextBoxDescription.Font = New System.Drawing.Font("Meiryo UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.TextBoxDescription.Location = New System.Drawing.Point(167, 139)
		Me.TextBoxDescription.Margin = New System.Windows.Forms.Padding(6, 3, 3, 3)
		Me.TextBoxDescription.Multiline = true
		Me.TextBoxDescription.Name = "TextBoxDescription"
		Me.TextBoxDescription.ReadOnly = true
		Me.TextBoxDescription.ScrollBars = System.Windows.Forms.ScrollBars.Both
		Me.TextBoxDescription.Size = New System.Drawing.Size(320, 145)
		Me.TextBoxDescription.TabIndex = 0
		Me.TextBoxDescription.TabStop = false
		Me.TextBoxDescription.Text = "説明 :"&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10)&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10)&"(ランタイムに、ラベルのテキストはアプリケーションのアセンブリ情報に置き換えられます。"&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10)&"プロジェクト デザイナーの [アプリケーション] ペイン"& _ 
    "で、アプリケーションのアセンブリ情報をカスタマイズします。)"
		'
		'OKButton
		'
		Me.OKButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
		Me.OKButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
		Me.OKButton.Font = New System.Drawing.Font("Meiryo UI", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.OKButton.Location = New System.Drawing.Point(365, 290)
		Me.OKButton.Name = "OKButton"
		Me.OKButton.Size = New System.Drawing.Size(122, 29)
		Me.OKButton.TabIndex = 0
		Me.OKButton.Text = "&OK"
		'
		'frmAbout
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6!, 12!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.CancelButton = Me.OKButton
		Me.ClientSize = New System.Drawing.Size(508, 338)
		Me.Controls.Add(Me.TableLayoutPanel)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
		Me.MaximizeBox = false
		Me.MinimizeBox = false
		Me.Name = "frmAbout"
		Me.Padding = New System.Windows.Forms.Padding(9, 8, 9, 8)
		Me.ShowInTaskbar = false
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
		Me.Text = "frmAbout"
		Me.TableLayoutPanel.ResumeLayout(false)
		Me.TableLayoutPanel.PerformLayout
		CType(Me.LogoPictureBox,System.ComponentModel.ISupportInitialize).EndInit
		Me.ResumeLayout(false)

End Sub

	Friend WithEvents lblMainComVer As Label
	Friend WithEvents lblMainContVer As Label
	Friend WithEvents LogoPictureBox As PictureBox
	Friend WithEvents LabelCompanyName As Label
End Class
