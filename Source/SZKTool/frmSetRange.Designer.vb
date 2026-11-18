<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmSetRange
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
		Me.btnCancel = New System.Windows.Forms.Button()
		Me.btnOK = New System.Windows.Forms.Button()
		Me.Label6 = New System.Windows.Forms.Label()
		Me.Label4 = New System.Windows.Forms.Label()
		Me.Label3 = New System.Windows.Forms.Label()
		Me.tbMax = New System.Windows.Forms.TextBox()
		Me.tbMin = New System.Windows.Forms.TextBox()
		Me.lblHint = New System.Windows.Forms.Label()
		Me.SuspendLayout
		'
		'btnCancel
		'
		Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
		Me.btnCancel.Font = New System.Drawing.Font("Meiryo UI", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.btnCancel.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.btnCancel.Location = New System.Drawing.Point(188, 152)
		Me.btnCancel.Name = "btnCancel"
		Me.btnCancel.Size = New System.Drawing.Size(121, 41)
		Me.btnCancel.TabIndex = 13
		Me.btnCancel.Text = "Cancel"
		Me.btnCancel.UseVisualStyleBackColor = true
		'
		'btnOK
		'
		Me.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK
		Me.btnOK.Font = New System.Drawing.Font("Meiryo UI", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.btnOK.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.btnOK.Location = New System.Drawing.Point(30, 152)
		Me.btnOK.Name = "btnOK"
		Me.btnOK.Size = New System.Drawing.Size(121, 41)
		Me.btnOK.TabIndex = 12
		Me.btnOK.Text = "OK"
		Me.btnOK.UseVisualStyleBackColor = true
		'
		'Label6
		'
		Me.Label6.AutoSize = true
		Me.Label6.Font = New System.Drawing.Font("Meiryo UI", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.Label6.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.Label6.Location = New System.Drawing.Point(183, 15)
		Me.Label6.Name = "Label6"
		Me.Label6.Size = New System.Drawing.Size(69, 26)
		Me.Label6.TabIndex = 8
		Me.Label6.Text = "Max :"
		'
		'Label4
		'
		Me.Label4.AutoSize = true
		Me.Label4.Font = New System.Drawing.Font("Meiryo UI", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.Label4.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.Label4.Location = New System.Drawing.Point(35, 15)
		Me.Label4.Name = "Label4"
		Me.Label4.Size = New System.Drawing.Size(63, 26)
		Me.Label4.TabIndex = 7
		Me.Label4.Text = "Min :"
		'
		'Label3
		'
		Me.Label3.AutoSize = true
		Me.Label3.Font = New System.Drawing.Font("MS UI Gothic", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.Label3.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.Label3.Location = New System.Drawing.Point(161, 60)
		Me.Label3.Name = "Label3"
		Me.Label3.Size = New System.Drawing.Size(21, 21)
		Me.Label3.TabIndex = 10
		Me.Label3.Text = "-"
		'
		'tbMax
		'
		Me.tbMax.Font = New System.Drawing.Font("Meiryo UI", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.tbMax.Location = New System.Drawing.Point(188, 53)
		Me.tbMax.Name = "tbMax"
		Me.tbMax.Size = New System.Drawing.Size(121, 34)
		Me.tbMax.TabIndex = 11
		'
		'tbMin
		'
		Me.tbMin.Font = New System.Drawing.Font("Meiryo UI", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.tbMin.Location = New System.Drawing.Point(30, 53)
		Me.tbMin.Name = "tbMin"
		Me.tbMin.Size = New System.Drawing.Size(121, 34)
		Me.tbMin.TabIndex = 9
		Me.tbMin.Text = "00000"
		'
		'lblHint
		'
		Me.lblHint.Font = New System.Drawing.Font("MS UI Gothic", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.lblHint.Location = New System.Drawing.Point(30, 92)
		Me.lblHint.Name = "lblHint"
		Me.lblHint.Size = New System.Drawing.Size(279, 55)
		Me.lblHint.TabIndex = 14
		Me.lblHint.Text = "The following input methods are available: Minutes = 9m Hours = 9h Days = 9d Time"& _ 
    " division = 9:9"
		'
		'frmSetRange
		'
		Me.AcceptButton = Me.btnOK
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6!, 12!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.CancelButton = Me.btnCancel
		Me.ClientSize = New System.Drawing.Size(353, 204)
		Me.Controls.Add(Me.lblHint)
		Me.Controls.Add(Me.btnCancel)
		Me.Controls.Add(Me.btnOK)
		Me.Controls.Add(Me.Label6)
		Me.Controls.Add(Me.Label4)
		Me.Controls.Add(Me.Label3)
		Me.Controls.Add(Me.tbMax)
		Me.Controls.Add(Me.tbMin)
		Me.MaximizeBox = false
		Me.MinimizeBox = false
		Me.Name = "frmSetRange"
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
		Me.Text = "Range"
		Me.ResumeLayout(false)
		Me.PerformLayout

End Sub

	Friend WithEvents btnCancel As Button
	Friend WithEvents btnOK As Button
	Friend WithEvents Label6 As Label
	Friend WithEvents Label4 As Label
	Friend WithEvents Label3 As Label
	Friend WithEvents tbMax As TextBox
	Friend WithEvents tbMin As TextBox
	Friend WithEvents lblHint As Label
End Class
