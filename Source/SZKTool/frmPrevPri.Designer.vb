<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmPrevPri
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
		Me.txtComment = New System.Windows.Forms.TextBox()
		Me.btnCancel = New System.Windows.Forms.Button()
		Me.btnOK = New System.Windows.Forms.Button()
		Me.Label2 = New System.Windows.Forms.Label()
		Me.Label1 = New System.Windows.Forms.Label()
		Me.txtMeasPlace = New System.Windows.Forms.TextBox()
		Me.Label3 = New System.Windows.Forms.Label()
		Me.txtMeasurer = New System.Windows.Forms.TextBox()
		Me.SuspendLayout
		'
		'txtComment
		'
		Me.txtComment.Location = New System.Drawing.Point(14, 122)
		Me.txtComment.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
		Me.txtComment.MaxLength = 100
		Me.txtComment.Multiline = true
		Me.txtComment.Name = "txtComment"
		Me.txtComment.Size = New System.Drawing.Size(303, 76)
		Me.txtComment.TabIndex = 5
		'
		'btnCancel
		'
		Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
		Me.btnCancel.Font = New System.Drawing.Font("Meiryo UI", 9.75!)
		Me.btnCancel.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.btnCancel.Location = New System.Drawing.Point(230, 208)
		Me.btnCancel.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
		Me.btnCancel.Name = "btnCancel"
		Me.btnCancel.Size = New System.Drawing.Size(87, 29)
		Me.btnCancel.TabIndex = 7
		Me.btnCancel.Text = "Cancel"
		Me.btnCancel.UseVisualStyleBackColor = true
		'
		'btnOK
		'
		Me.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK
		Me.btnOK.Font = New System.Drawing.Font("Meiryo UI", 9.75!)
		Me.btnOK.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.btnOK.Location = New System.Drawing.Point(135, 208)
		Me.btnOK.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
		Me.btnOK.Name = "btnOK"
		Me.btnOK.Size = New System.Drawing.Size(87, 29)
		Me.btnOK.TabIndex = 6
		Me.btnOK.Text = "Print"
		Me.btnOK.UseVisualStyleBackColor = true
		'
		'Label2
		'
		Me.Label2.AutoSize = true
		Me.Label2.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.Label2.Location = New System.Drawing.Point(14, 104)
		Me.Label2.Name = "Label2"
		Me.Label2.Size = New System.Drawing.Size(70, 15)
		Me.Label2.TabIndex = 4
		Me.Label2.Text = "Comment:"
		'
		'Label1
		'
		Me.Label1.AutoSize = true
		Me.Label1.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.Label1.Location = New System.Drawing.Point(12, 11)
		Me.Label1.Name = "Label1"
		Me.Label1.Size = New System.Drawing.Size(126, 15)
		Me.Label1.TabIndex = 0
		Me.Label1.Text = "Measurement Place:"
		'
		'txtMeasPlace
		'
		Me.txtMeasPlace.Location = New System.Drawing.Point(12, 30)
		Me.txtMeasPlace.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
		Me.txtMeasPlace.MaxLength = 100
		Me.txtMeasPlace.Name = "txtMeasPlace"
		Me.txtMeasPlace.Size = New System.Drawing.Size(303, 23)
		Me.txtMeasPlace.TabIndex = 1
		'
		'Label3
		'
		Me.Label3.AutoSize = true
		Me.Label3.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.Label3.Location = New System.Drawing.Point(12, 58)
		Me.Label3.Name = "Label3"
		Me.Label3.Size = New System.Drawing.Size(66, 15)
		Me.Label3.TabIndex = 2
		Me.Label3.Text = "Measurer:"
		'
		'txtMeasurer
		'
		Me.txtMeasurer.Location = New System.Drawing.Point(12, 76)
		Me.txtMeasurer.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
		Me.txtMeasurer.MaxLength = 100
		Me.txtMeasurer.Name = "txtMeasurer"
		Me.txtMeasurer.Size = New System.Drawing.Size(303, 23)
		Me.txtMeasurer.TabIndex = 3
		'
		'frmPrevPri
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(7!, 15!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.CancelButton = Me.btnCancel
		Me.ClientSize = New System.Drawing.Size(331, 249)
		Me.Controls.Add(Me.Label3)
		Me.Controls.Add(Me.txtMeasurer)
		Me.Controls.Add(Me.Label1)
		Me.Controls.Add(Me.txtMeasPlace)
		Me.Controls.Add(Me.Label2)
		Me.Controls.Add(Me.txtComment)
		Me.Controls.Add(Me.btnCancel)
		Me.Controls.Add(Me.btnOK)
		Me.Font = New System.Drawing.Font("Meiryo UI", 9!)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
		Me.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
		Me.MaximizeBox = false
		Me.MinimizeBox = false
		Me.Name = "frmPrevPri"
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
		Me.Text = "Print"
		Me.ResumeLayout(false)
		Me.PerformLayout

End Sub
    Friend WithEvents txtComment As System.Windows.Forms.TextBox
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtMeasPlace As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents txtMeasurer As System.Windows.Forms.TextBox
End Class
