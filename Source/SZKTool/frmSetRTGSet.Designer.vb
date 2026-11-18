<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmSetRTGSet
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
		Me.cboRtgsetName = New System.Windows.Forms.ComboBox()
		Me.Label1 = New System.Windows.Forms.Label()
		Me.btnCancel = New System.Windows.Forms.Button()
		Me.btnOK = New System.Windows.Forms.Button()
		Me.lblCalibOK = New System.Windows.Forms.Label()
		Me.SuspendLayout
		'
		'cboRtgsetName
		'
		Me.cboRtgsetName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.cboRtgsetName.FormattingEnabled = true
		Me.cboRtgsetName.Location = New System.Drawing.Point(24, 55)
		Me.cboRtgsetName.Margin = New System.Windows.Forms.Padding(6)
		Me.cboRtgsetName.Name = "cboRtgsetName"
		Me.cboRtgsetName.Size = New System.Drawing.Size(456, 32)
		Me.cboRtgsetName.TabIndex = 11
		'
		'Label1
		'
		Me.Label1.AutoSize = true
		Me.Label1.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.Label1.Location = New System.Drawing.Point(20, 22)
		Me.Label1.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
		Me.Label1.Name = "Label1"
		Me.Label1.Size = New System.Drawing.Size(122, 24)
		Me.Label1.TabIndex = 8
		Me.Label1.Text = "Setting File:"
		'
		'btnCancel
		'
		Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
		Me.btnCancel.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.btnCancel.Location = New System.Drawing.Point(334, 104)
		Me.btnCancel.Margin = New System.Windows.Forms.Padding(6)
		Me.btnCancel.Name = "btnCancel"
		Me.btnCancel.Size = New System.Drawing.Size(150, 46)
		Me.btnCancel.TabIndex = 10
		Me.btnCancel.Text = "Cancel"
		Me.btnCancel.UseVisualStyleBackColor = true
		'
		'btnOK
		'
		Me.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK
		Me.btnOK.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.btnOK.Location = New System.Drawing.Point(172, 104)
		Me.btnOK.Margin = New System.Windows.Forms.Padding(6)
		Me.btnOK.Name = "btnOK"
		Me.btnOK.Size = New System.Drawing.Size(150, 46)
		Me.btnOK.TabIndex = 9
		Me.btnOK.Text = "OK"
		Me.btnOK.UseVisualStyleBackColor = true
		'
		'lblCalibOK
		'
		Me.lblCalibOK.BackColor = System.Drawing.Color.GreenYellow
		Me.lblCalibOK.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		Me.lblCalibOK.Font = New System.Drawing.Font("Meiryo UI", 9!, System.Drawing.FontStyle.Bold)
		Me.lblCalibOK.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.lblCalibOK.Location = New System.Drawing.Point(350, 22)
		Me.lblCalibOK.Name = "lblCalibOK"
		Me.lblCalibOK.Size = New System.Drawing.Size(130, 30)
		Me.lblCalibOK.TabIndex = 35
		Me.lblCalibOK.Text = "Calibrated"
		Me.lblCalibOK.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		'
		'frmSetRTGSet
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(12!, 24!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(504, 174)
		Me.Controls.Add(Me.lblCalibOK)
		Me.Controls.Add(Me.cboRtgsetName)
		Me.Controls.Add(Me.Label1)
		Me.Controls.Add(Me.btnCancel)
		Me.Controls.Add(Me.btnOK)
		Me.Font = New System.Drawing.Font("Meiryo UI", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.Margin = New System.Windows.Forms.Padding(6)
		Me.MaximizeBox = false
		Me.MinimizeBox = false
		Me.Name = "frmSetRTGSet"
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
		Me.Text = "Temperature Setting"
		Me.ResumeLayout(false)
		Me.PerformLayout

End Sub

	Friend WithEvents cboRtgsetName As ComboBox
	Friend WithEvents Label1 As Label
	Friend WithEvents btnCancel As Button
	Friend WithEvents btnOK As Button
	Friend WithEvents lblCalibOK As Label
End Class
