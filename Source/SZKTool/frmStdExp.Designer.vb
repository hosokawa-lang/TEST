<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmStdExp
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
		Me.components = New System.ComponentModel.Container()
		Me.tbxFilename = New System.Windows.Forms.TextBox()
		Me.lblFilename = New System.Windows.Forms.Label()
		Me.btnOk = New System.Windows.Forms.Button()
		Me.Button1 = New System.Windows.Forms.Button()
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.SuspendLayout
		'
		'tbxFilename
		'
		Me.tbxFilename.Font = New System.Drawing.Font("MS UI Gothic", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.tbxFilename.Location = New System.Drawing.Point(90, 27)
		Me.tbxFilename.Name = "tbxFilename"
		Me.tbxFilename.Size = New System.Drawing.Size(411, 23)
		Me.tbxFilename.TabIndex = 0
		'
		'lblFilename
		'
		Me.lblFilename.AutoSize = true
		Me.lblFilename.Font = New System.Drawing.Font("MS UI Gothic", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.lblFilename.Location = New System.Drawing.Point(12, 30)
		Me.lblFilename.Name = "lblFilename"
		Me.lblFilename.Size = New System.Drawing.Size(71, 16)
		Me.lblFilename.TabIndex = 1
		Me.lblFilename.Text = "File name"
		'
		'btnOk
		'
		Me.btnOk.Font = New System.Drawing.Font("MS UI Gothic", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.btnOk.Location = New System.Drawing.Point(277, 74)
		Me.btnOk.Name = "btnOk"
		Me.btnOk.Size = New System.Drawing.Size(109, 29)
		Me.btnOk.TabIndex = 2
		Me.btnOk.Text = "&OK"
		Me.btnOk.UseVisualStyleBackColor = true
		'
		'Button1
		'
		Me.Button1.Font = New System.Drawing.Font("MS UI Gothic", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.Button1.Location = New System.Drawing.Point(392, 74)
		Me.Button1.Name = "Button1"
		Me.Button1.Size = New System.Drawing.Size(109, 29)
		Me.Button1.TabIndex = 3
		Me.Button1.Text = "&Cansel"
		Me.Button1.UseVisualStyleBackColor = true
		'
		'frmStdExp
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6!, 12!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(508, 110)
		Me.Controls.Add(Me.Button1)
		Me.Controls.Add(Me.btnOk)
		Me.Controls.Add(Me.lblFilename)
		Me.Controls.Add(Me.tbxFilename)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
		Me.MaximizeBox = false
		Me.MinimizeBox = false
		Me.Name = "frmStdExp"
		Me.Text = "Standard Data Save"
		Me.ResumeLayout(false)
		Me.PerformLayout

End Sub

    Friend WithEvents tbxFilename As TextBox
    Friend WithEvents lblFilename As Label
    Friend WithEvents btnOk As Button
    Friend WithEvents Button1 As Button
    Friend WithEvents ToolTip1 As ToolTip
End Class
