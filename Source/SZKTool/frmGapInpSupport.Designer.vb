<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmGapInpSupport
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
		Me.nudPressRate = New System.Windows.Forms.NumericUpDown()
		Me.lblPressRate = New System.Windows.Forms.Label()
		Me.dgvInp = New System.Windows.Forms.DataGridView()
		Me.lblAve = New System.Windows.Forms.Label()
		Me.lblAvelbl = New System.Windows.Forms.Label()
		Me.btnApply = New System.Windows.Forms.Button()
		Me.btnCancel = New System.Windows.Forms.Button()
		CType(Me.nudPressRate,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.dgvInp,System.ComponentModel.ISupportInitialize).BeginInit
		Me.SuspendLayout
		'
		'nudPressRate
		'
		Me.nudPressRate.DecimalPlaces = 1
		Me.nudPressRate.Font = New System.Drawing.Font("Meiryo UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.nudPressRate.Location = New System.Drawing.Point(235, 266)
		Me.nudPressRate.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
		Me.nudPressRate.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
		Me.nudPressRate.Name = "nudPressRate"
		Me.nudPressRate.Size = New System.Drawing.Size(77, 24)
		Me.nudPressRate.TabIndex = 3
		Me.nudPressRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		Me.nudPressRate.Value = New Decimal(New Integer() {700, 0, 0, 65536})
		'
		'lblPressRate
		'
		Me.lblPressRate.AutoSize = true
		Me.lblPressRate.Font = New System.Drawing.Font("Meiryo UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.lblPressRate.Location = New System.Drawing.Point(153, 245)
		Me.lblPressRate.Name = "lblPressRate"
		Me.lblPressRate.Size = New System.Drawing.Size(121, 17)
		Me.lblPressRate.TabIndex = 1
		Me.lblPressRate.Text = "Pressure Rate[%]"
		'
		'dgvInp
		'
		Me.dgvInp.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
		Me.dgvInp.Location = New System.Drawing.Point(3, 2)
		Me.dgvInp.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
		Me.dgvInp.Name = "dgvInp"
		Me.dgvInp.RowHeadersVisible = false
		Me.dgvInp.RowTemplate.Height = 21
		Me.dgvInp.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
		Me.dgvInp.Size = New System.Drawing.Size(142, 385)
		Me.dgvInp.TabIndex = 0
		'
		'lblAve
		'
		Me.lblAve.AutoSize = true
		Me.lblAve.Font = New System.Drawing.Font("Yu Gothic UI", 18!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.lblAve.Location = New System.Drawing.Point(212, 24)
		Me.lblAve.Name = "lblAve"
		Me.lblAve.Size = New System.Drawing.Size(72, 32)
		Me.lblAve.TabIndex = 3
		Me.lblAve.Text = "0.000"
		Me.lblAve.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		'
		'lblAvelbl
		'
		Me.lblAvelbl.AutoSize = true
		Me.lblAvelbl.Font = New System.Drawing.Font("Meiryo UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.lblAvelbl.Location = New System.Drawing.Point(153, 2)
		Me.lblAvelbl.Name = "lblAvelbl"
		Me.lblAvelbl.Size = New System.Drawing.Size(143, 17)
		Me.lblAvelbl.TabIndex = 4
		Me.lblAvelbl.Text = "Calcurate value[mm]"
		'
		'btnApply
		'
		Me.btnApply.DialogResult = System.Windows.Forms.DialogResult.OK
		Me.btnApply.Font = New System.Drawing.Font("Yu Gothic UI", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.btnApply.Location = New System.Drawing.Point(156, 307)
		Me.btnApply.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
		Me.btnApply.Name = "btnApply"
		Me.btnApply.Size = New System.Drawing.Size(156, 32)
		Me.btnApply.TabIndex = 1
		Me.btnApply.Text = "&Apply"
		Me.btnApply.UseVisualStyleBackColor = true
		'
		'btnCancel
		'
		Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
		Me.btnCancel.Font = New System.Drawing.Font("Yu Gothic UI", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.btnCancel.Location = New System.Drawing.Point(156, 347)
		Me.btnCancel.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
		Me.btnCancel.Name = "btnCancel"
		Me.btnCancel.Size = New System.Drawing.Size(156, 35)
		Me.btnCancel.TabIndex = 2
		Me.btnCancel.Text = "&Cancel"
		Me.btnCancel.UseVisualStyleBackColor = true
		'
		'frmGapInpSupport
		'
		Me.AcceptButton = Me.btnApply
		Me.AutoScaleDimensions = New System.Drawing.SizeF(7!, 15!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.CancelButton = Me.btnCancel
		Me.ClientSize = New System.Drawing.Size(316, 398)
		Me.ControlBox = false
		Me.Controls.Add(Me.btnCancel)
		Me.Controls.Add(Me.btnApply)
		Me.Controls.Add(Me.lblAvelbl)
		Me.Controls.Add(Me.lblAve)
		Me.Controls.Add(Me.dgvInp)
		Me.Controls.Add(Me.lblPressRate)
		Me.Controls.Add(Me.nudPressRate)
		Me.Font = New System.Drawing.Font("Meiryo UI", 9!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.KeyPreview = true
		Me.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
		Me.Name = "frmGapInpSupport"
		Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
		Me.Text = "Measure Gap Input Support"
		Me.TopMost = true
		CType(Me.nudPressRate,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.dgvInp,System.ComponentModel.ISupportInitialize).EndInit
		Me.ResumeLayout(false)
		Me.PerformLayout

End Sub

	Friend WithEvents nudPressRate As NumericUpDown
	Friend WithEvents lblPressRate As Label
	Friend WithEvents dgvInp As DataGridView
	Friend WithEvents lblAve As Label
	Friend WithEvents lblAvelbl As Label
	Friend WithEvents btnApply As Button
	Friend WithEvents btnCancel As Button
End Class
