<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmQRCodeMake
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

	'Windows フォーム デザイナーで必要です。
	Private components As System.ComponentModel.IContainer

	'メモ: 以下のプロシージャは Windows フォーム デザイナーで必要です。
	'Windows フォーム デザイナーを使用して変更できます。  
	'コード エディターを使って変更しないでください。
	<System.Diagnostics.DebuggerStepThrough()> _
	Private Sub InitializeComponent()
		Me.txtText = New System.Windows.Forms.TextBox()
		Me.picQR = New System.Windows.Forms.PictureBox()
		Me.btnGenerate = New System.Windows.Forms.Button()
		Me.btnSave = New System.Windows.Forms.Button()
		Me.lblInfo = New System.Windows.Forms.Label()
		Me.lblLogo = New System.Windows.Forms.Label()
		Me.numLogo = New System.Windows.Forms.NumericUpDown()
		Me.lblECC = New System.Windows.Forms.Label()
		Me.cmbECC = New System.Windows.Forms.ComboBox()
		CType(Me.picQR,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.numLogo,System.ComponentModel.ISupportInitialize).BeginInit
		Me.SuspendLayout
		'
		'txtText
		'
		Me.txtText.Location = New System.Drawing.Point(13, 16)
		Me.txtText.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
		Me.txtText.Multiline = true
		Me.txtText.Name = "txtText"
		Me.txtText.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
		Me.txtText.Size = New System.Drawing.Size(616, 80)
		Me.txtText.TabIndex = 0
		Me.txtText.Text = "https://example.com"
		'
		'picQR
		'
		Me.picQR.BackColor = System.Drawing.Color.White
		Me.picQR.Location = New System.Drawing.Point(13, 114)
		Me.picQR.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
		Me.picQR.Name = "picQR"
		Me.picQR.Size = New System.Drawing.Size(320, 320)
		Me.picQR.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
		Me.picQR.TabIndex = 1
		Me.picQR.TabStop = false
		'
		'btnGenerate
		'
		Me.btnGenerate.AutoSize = true
		Me.btnGenerate.Location = New System.Drawing.Point(428, 204)
		Me.btnGenerate.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
		Me.btnGenerate.Name = "btnGenerate"
		Me.btnGenerate.Size = New System.Drawing.Size(201, 32)
		Me.btnGenerate.TabIndex = 2
		Me.btnGenerate.Text = "QRコード生成"
		Me.btnGenerate.UseVisualStyleBackColor = true
		'
		'btnSave
		'
		Me.btnSave.AutoSize = true
		Me.btnSave.Location = New System.Drawing.Point(428, 240)
		Me.btnSave.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
		Me.btnSave.Name = "btnSave"
		Me.btnSave.Size = New System.Drawing.Size(201, 32)
		Me.btnSave.TabIndex = 2
		Me.btnSave.Text = "画像を保存"
		Me.btnSave.UseVisualStyleBackColor = true
		'
		'lblInfo
		'
		Me.lblInfo.AutoSize = true
		Me.lblInfo.Location = New System.Drawing.Point(358, 322)
		Me.lblInfo.Name = "lblInfo"
		Me.lblInfo.Size = New System.Drawing.Size(79, 20)
		Me.lblInfo.TabIndex = 3
		Me.lblInfo.Text = "----------"
		'
		'lblLogo
		'
		Me.lblLogo.AutoSize = true
		Me.lblLogo.Location = New System.Drawing.Point(349, 118)
		Me.lblLogo.Name = "lblLogo"
		Me.lblLogo.Size = New System.Drawing.Size(145, 20)
		Me.lblLogo.TabIndex = 4
		Me.lblLogo.Text = "ロゴに占めるセル数："
		'
		'numLogo
		'
		Me.numLogo.Location = New System.Drawing.Point(495, 118)
		Me.numLogo.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
		Me.numLogo.Maximum = New Decimal(New Integer() {50, 0, 0, 0})
		Me.numLogo.Minimum = New Decimal(New Integer() {5, 0, 0, 0})
		Me.numLogo.Name = "numLogo"
		Me.numLogo.Size = New System.Drawing.Size(133, 28)
		Me.numLogo.TabIndex = 5
		Me.numLogo.Value = New Decimal(New Integer() {20, 0, 0, 0})
		'
		'lblECC
		'
		Me.lblECC.AutoSize = true
		Me.lblECC.Location = New System.Drawing.Point(349, 156)
		Me.lblECC.Name = "lblECC"
		Me.lblECC.Size = New System.Drawing.Size(121, 20)
		Me.lblECC.TabIndex = 4
		Me.lblECC.Text = "誤り訂正レベル："
		'
		'cmbECC
		'
		Me.cmbECC.FormattingEnabled = true
		Me.cmbECC.Location = New System.Drawing.Point(495, 152)
		Me.cmbECC.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
		Me.cmbECC.Name = "cmbECC"
		Me.cmbECC.Size = New System.Drawing.Size(134, 28)
		Me.cmbECC.TabIndex = 6
		'
		'frmQRCodeMake
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(10!, 20!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(642, 449)
		Me.Controls.Add(Me.cmbECC)
		Me.Controls.Add(Me.numLogo)
		Me.Controls.Add(Me.lblECC)
		Me.Controls.Add(Me.lblLogo)
		Me.Controls.Add(Me.lblInfo)
		Me.Controls.Add(Me.btnSave)
		Me.Controls.Add(Me.btnGenerate)
		Me.Controls.Add(Me.picQR)
		Me.Controls.Add(Me.txtText)
		Me.Font = New System.Drawing.Font("Meiryo UI", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
		Me.Name = "frmQRCodeMake"
		Me.Text = "Form1"
		CType(Me.picQR,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.numLogo,System.ComponentModel.ISupportInitialize).EndInit
		Me.ResumeLayout(false)
		Me.PerformLayout

End Sub

	Friend WithEvents txtText As TextBox
	Friend WithEvents picQR As PictureBox
	Friend WithEvents btnGenerate As Button
	Friend WithEvents btnSave As Button
	Friend WithEvents lblInfo As Label
	Friend WithEvents lblLogo As Label
	Friend WithEvents numLogo As NumericUpDown
	Friend WithEvents lblECC As Label
	Friend WithEvents cmbECC As ComboBox
End Class
