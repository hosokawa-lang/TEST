<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class dlgPassword
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
		Me.btnOk = New System.Windows.Forms.Button()
		Me.btnCancel = New System.Windows.Forms.Button()
		Me.tbxPassword = New System.Windows.Forms.TextBox()
		Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
		Me.TableLayoutPanel1.SuspendLayout
		Me.SuspendLayout
		'
		'btnOk
		'
		Me.btnOk.Anchor = System.Windows.Forms.AnchorStyles.None
		Me.btnOk.Font = New System.Drawing.Font("Meiryo UI", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.btnOk.Location = New System.Drawing.Point(9, 5)
		Me.btnOk.Name = "btnOk"
		Me.btnOk.Size = New System.Drawing.Size(89, 32)
		Me.btnOk.TabIndex = 1
		Me.btnOk.Text = "OK"
		'
		'btnCancel
		'
		Me.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.None
		Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
		Me.btnCancel.Font = New System.Drawing.Font("Meiryo UI", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.btnCancel.Location = New System.Drawing.Point(116, 5)
		Me.btnCancel.Name = "btnCancel"
		Me.btnCancel.Size = New System.Drawing.Size(89, 32)
		Me.btnCancel.TabIndex = 2
		Me.btnCancel.Text = "Cancel"
		'
		'tbxPassword
		'
		Me.tbxPassword.Font = New System.Drawing.Font("Meiryo UI", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.tbxPassword.Location = New System.Drawing.Point(12, 14)
		Me.tbxPassword.MaxLength = 16
		Me.tbxPassword.Name = "tbxPassword"
		Me.tbxPassword.PasswordChar = Global.Microsoft.VisualBasic.ChrW(9679)
		Me.tbxPassword.Size = New System.Drawing.Size(279, 28)
		Me.tbxPassword.TabIndex = 0
		'
		'TableLayoutPanel1
		'
		Me.TableLayoutPanel1.ColumnCount = 2
		Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.5!))
		Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 49.5!))
		Me.TableLayoutPanel1.Controls.Add(Me.btnOk, 0, 0)
		Me.TableLayoutPanel1.Controls.Add(Me.btnCancel, 1, 0)
		Me.TableLayoutPanel1.Location = New System.Drawing.Point(289, 41)
		Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
		Me.TableLayoutPanel1.RowCount = 1
		Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50!))
		Me.TableLayoutPanel1.Size = New System.Drawing.Size(214, 42)
		Me.TableLayoutPanel1.TabIndex = 3
		'
		'dlgPassword
		'
		Me.AcceptButton = Me.btnOk
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6!, 12!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.CancelButton = Me.btnCancel
		Me.ClientSize = New System.Drawing.Size(515, 91)
		Me.Controls.Add(Me.TableLayoutPanel1)
		Me.Controls.Add(Me.tbxPassword)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
		Me.MaximizeBox = false
		Me.MinimizeBox = false
		Me.Name = "dlgPassword"
		Me.ShowInTaskbar = false
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
		Me.Text = "dlgPassword"
		Me.TableLayoutPanel1.ResumeLayout(false)
		Me.ResumeLayout(false)
		Me.PerformLayout

End Sub
	Friend WithEvents btnOk As System.Windows.Forms.Button
	Friend WithEvents btnCancel As System.Windows.Forms.Button
	Friend WithEvents tbxPassword As TextBox
	Friend WithEvents TableLayoutPanel1 As TableLayoutPanel
End Class
