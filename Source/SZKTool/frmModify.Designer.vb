<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmModify
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
		Me.btnCan = New System.Windows.Forms.Button()
		Me.btnOK = New System.Windows.Forms.Button()
		Me.txtInput = New System.Windows.Forms.TextBox()
		Me.lblMsg = New System.Windows.Forms.Label()
		Me.SuspendLayout
		'
		'btnCan
		'
		Me.btnCan.Location = New System.Drawing.Point(286, 96)
		Me.btnCan.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
		Me.btnCan.Name = "btnCan"
		Me.btnCan.Size = New System.Drawing.Size(87, 29)
		Me.btnCan.TabIndex = 7
		Me.btnCan.Text = "Cancel"
		Me.btnCan.UseVisualStyleBackColor = true
		'
		'btnOK
		'
		Me.btnOK.Location = New System.Drawing.Point(173, 96)
		Me.btnOK.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
		Me.btnOK.Name = "btnOK"
		Me.btnOK.Size = New System.Drawing.Size(87, 29)
		Me.btnOK.TabIndex = 6
		Me.btnOK.Text = "OK"
		Me.btnOK.UseVisualStyleBackColor = true
		'
		'txtInput
		'
		Me.txtInput.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left)  _
            Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
		Me.txtInput.Location = New System.Drawing.Point(30, 50)
		Me.txtInput.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
		Me.txtInput.Name = "txtInput"
		Me.txtInput.Size = New System.Drawing.Size(342, 23)
		Me.txtInput.TabIndex = 5
		'
		'lblMsg
		'
		Me.lblMsg.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left)  _
            Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
		Me.lblMsg.AutoEllipsis = true
		Me.lblMsg.Location = New System.Drawing.Point(14, 22)
		Me.lblMsg.Name = "lblMsg"
		Me.lblMsg.Size = New System.Drawing.Size(376, 22)
		Me.lblMsg.TabIndex = 4
		Me.lblMsg.Text = "Text"
		Me.lblMsg.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
		'
		'frmModify
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(7!, 15!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(405, 148)
		Me.Controls.Add(Me.btnCan)
		Me.Controls.Add(Me.btnOK)
		Me.Controls.Add(Me.txtInput)
		Me.Controls.Add(Me.lblMsg)
		Me.Font = New System.Drawing.Font("Meiryo UI", 9!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
		Me.MaximizeBox = false
		Me.MinimizeBox = false
		Me.Name = "frmModify"
		Me.Text = "Input Modify"
		Me.ResumeLayout(false)
		Me.PerformLayout

End Sub

	Friend WithEvents btnCan As Button
	Friend WithEvents btnOK As Button
	Friend WithEvents txtInput As TextBox
	Friend WithEvents lblMsg As Label
End Class
