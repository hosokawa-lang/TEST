<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmTempGraph
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
		Me.lblFG = New System.Windows.Forms.Label()
		Me.btnOK = New System.Windows.Forms.Button()
		Me.pbG = New System.Windows.Forms.PictureBox()
		CType(Me.pbG,System.ComponentModel.ISupportInitialize).BeginInit
		Me.SuspendLayout
		'
		'lblFG
		'
		Me.lblFG.AutoSize = true
		Me.lblFG.ForeColor = System.Drawing.Color.ForestGreen
		Me.lblFG.Location = New System.Drawing.Point(10, 466)
		Me.lblFG.Name = "lblFG"
		Me.lblFG.Size = New System.Drawing.Size(30, 20)
		Me.lblFG.TabIndex = 9
		Me.lblFG.Text = "FG"
		Me.lblFG.Visible = false
		'
		'btnOK
		'
		Me.btnOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
		Me.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK
		Me.btnOK.Font = New System.Drawing.Font("Meiryo UI", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.btnOK.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.btnOK.Location = New System.Drawing.Point(374, 449)
		Me.btnOK.Name = "btnOK"
		Me.btnOK.Size = New System.Drawing.Size(129, 31)
		Me.btnOK.TabIndex = 8
		Me.btnOK.Text = "OK"
		Me.btnOK.UseVisualStyleBackColor = true
		'
		'pbG
		'
		Me.pbG.Dock = System.Windows.Forms.DockStyle.Top
		Me.pbG.Location = New System.Drawing.Point(0, 0)
		Me.pbG.Name = "pbG"
		Me.pbG.Size = New System.Drawing.Size(514, 364)
		Me.pbG.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
		Me.pbG.TabIndex = 7
		Me.pbG.TabStop = false
		'
		'frmTempGraph
		'
		Me.AcceptButton = Me.btnOK
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
		Me.ClientSize = New System.Drawing.Size(514, 487)
		Me.Controls.Add(Me.lblFG)
		Me.Controls.Add(Me.btnOK)
		Me.Controls.Add(Me.pbG)
		Me.Font = New System.Drawing.Font("Meiryo UI", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.MaximizeBox = false
		Me.MinimizeBox = false
		Me.Name = "frmTempGraph"
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
		Me.Text = "Preview"
		CType(Me.pbG,System.ComponentModel.ISupportInitialize).EndInit
		Me.ResumeLayout(false)
		Me.PerformLayout

End Sub

	Friend WithEvents lblFG As Label
	Friend WithEvents btnOK As Button
	Friend WithEvents pbG As PictureBox
End Class
