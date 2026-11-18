<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmTempLinearTool
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
		Me.GroupBox1 = New System.Windows.Forms.GroupBox()
		Me.nudSpeed = New System.Windows.Forms.NumericUpDown()
		Me.nudEndTemp = New System.Windows.Forms.NumericUpDown()
		Me.nudStartTemp = New System.Windows.Forms.NumericUpDown()
		Me.nudWaitTime = New System.Windows.Forms.NumericUpDown()
		Me.lblSpeed = New System.Windows.Forms.Label()
		Me.lblEndTime = New System.Windows.Forms.Label()
		Me.lblStartTime = New System.Windows.Forms.Label()
		Me.lblWaitTime = New System.Windows.Forms.Label()
		Me.btnCansel = New System.Windows.Forms.Button()
		Me.btnOk = New System.Windows.Forms.Button()
		Me.GroupBox1.SuspendLayout
		CType(Me.nudSpeed,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.nudEndTemp,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.nudStartTemp,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.nudWaitTime,System.ComponentModel.ISupportInitialize).BeginInit
		Me.SuspendLayout
		'
		'GroupBox1
		'
		Me.GroupBox1.Controls.Add(Me.nudSpeed)
		Me.GroupBox1.Controls.Add(Me.nudEndTemp)
		Me.GroupBox1.Controls.Add(Me.nudStartTemp)
		Me.GroupBox1.Controls.Add(Me.nudWaitTime)
		Me.GroupBox1.Controls.Add(Me.lblSpeed)
		Me.GroupBox1.Controls.Add(Me.lblEndTime)
		Me.GroupBox1.Controls.Add(Me.lblStartTime)
		Me.GroupBox1.Controls.Add(Me.lblWaitTime)
		Me.GroupBox1.Location = New System.Drawing.Point(4, 2)
		Me.GroupBox1.Margin = New System.Windows.Forms.Padding(4)
		Me.GroupBox1.Name = "GroupBox1"
		Me.GroupBox1.Padding = New System.Windows.Forms.Padding(4)
		Me.GroupBox1.Size = New System.Drawing.Size(299, 157)
		Me.GroupBox1.TabIndex = 0
		Me.GroupBox1.TabStop = false
		'
		'nudSpeed
		'
		Me.nudSpeed.DecimalPlaces = 2
		Me.nudSpeed.Location = New System.Drawing.Point(155, 115)
		Me.nudSpeed.Maximum = New Decimal(New Integer() {10, 0, 0, 0})
		Me.nudSpeed.Name = "nudSpeed"
		Me.nudSpeed.Size = New System.Drawing.Size(120, 24)
		Me.nudSpeed.TabIndex = 3
		'
		'nudEndTemp
		'
		Me.nudEndTemp.DecimalPlaces = 1
		Me.nudEndTemp.Location = New System.Drawing.Point(155, 85)
		Me.nudEndTemp.Maximum = New Decimal(New Integer() {240, 0, 0, 0})
		Me.nudEndTemp.Name = "nudEndTemp"
		Me.nudEndTemp.Size = New System.Drawing.Size(120, 24)
		Me.nudEndTemp.TabIndex = 2
		'
		'nudStartTemp
		'
		Me.nudStartTemp.DecimalPlaces = 1
		Me.nudStartTemp.Location = New System.Drawing.Point(155, 55)
		Me.nudStartTemp.Maximum = New Decimal(New Integer() {240, 0, 0, 0})
		Me.nudStartTemp.Name = "nudStartTemp"
		Me.nudStartTemp.Size = New System.Drawing.Size(120, 24)
		Me.nudStartTemp.TabIndex = 1
		'
		'nudWaitTime
		'
		Me.nudWaitTime.DecimalPlaces = 1
		Me.nudWaitTime.Location = New System.Drawing.Point(155, 25)
		Me.nudWaitTime.Maximum = New Decimal(New Integer() {10000, 0, 0, 0})
		Me.nudWaitTime.Name = "nudWaitTime"
		Me.nudWaitTime.Size = New System.Drawing.Size(120, 24)
		Me.nudWaitTime.TabIndex = 0
		'
		'lblSpeed
		'
		Me.lblSpeed.AutoSize = true
		Me.lblSpeed.Font = New System.Drawing.Font("Meiryo UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.lblSpeed.Location = New System.Drawing.Point(9, 117)
		Me.lblSpeed.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
		Me.lblSpeed.Name = "lblSpeed"
		Me.lblSpeed.Size = New System.Drawing.Size(130, 17)
		Me.lblSpeed.TabIndex = 0
		Me.lblSpeed.Text = "Speed (deg C/min)"
		'
		'lblEndTime
		'
		Me.lblEndTime.AutoSize = true
		Me.lblEndTime.Font = New System.Drawing.Font("Meiryo UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.lblEndTime.Location = New System.Drawing.Point(9, 87)
		Me.lblEndTime.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
		Me.lblEndTime.Name = "lblEndTime"
		Me.lblEndTime.Size = New System.Drawing.Size(124, 17)
		Me.lblEndTime.TabIndex = 0
		Me.lblEndTime.Text = "End Temp (deg C)"
		'
		'lblStartTime
		'
		Me.lblStartTime.AutoSize = true
		Me.lblStartTime.Font = New System.Drawing.Font("Meiryo UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.lblStartTime.Location = New System.Drawing.Point(9, 57)
		Me.lblStartTime.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
		Me.lblStartTime.Name = "lblStartTime"
		Me.lblStartTime.Size = New System.Drawing.Size(131, 17)
		Me.lblStartTime.TabIndex = 0
		Me.lblStartTime.Text = "Start Temp (deg C)"
		'
		'lblWaitTime
		'
		Me.lblWaitTime.AutoSize = true
		Me.lblWaitTime.Font = New System.Drawing.Font("Meiryo UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.lblWaitTime.Location = New System.Drawing.Point(9, 27)
		Me.lblWaitTime.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
		Me.lblWaitTime.Name = "lblWaitTime"
		Me.lblWaitTime.Size = New System.Drawing.Size(110, 17)
		Me.lblWaitTime.TabIndex = 0
		Me.lblWaitTime.Text = "Wait Time (Sec)"
		'
		'btnCansel
		'
		Me.btnCansel.DialogResult = System.Windows.Forms.DialogResult.Cancel
		Me.btnCansel.Location = New System.Drawing.Point(209, 166)
		Me.btnCansel.Name = "btnCansel"
		Me.btnCansel.Size = New System.Drawing.Size(94, 29)
		Me.btnCansel.TabIndex = 5
		Me.btnCansel.Text = "Cansel"
		Me.btnCansel.UseVisualStyleBackColor = true
		'
		'btnOk
		'
		Me.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK
		Me.btnOk.Location = New System.Drawing.Point(113, 166)
		Me.btnOk.Name = "btnOk"
		Me.btnOk.Size = New System.Drawing.Size(90, 29)
		Me.btnOk.TabIndex = 4
		Me.btnOk.Text = "Ok"
		Me.btnOk.UseVisualStyleBackColor = true
		'
		'frmTempLinearTool
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(8!, 17!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(311, 201)
		Me.ControlBox = false
		Me.Controls.Add(Me.btnOk)
		Me.Controls.Add(Me.btnCansel)
		Me.Controls.Add(Me.GroupBox1)
		Me.Font = New System.Drawing.Font("Meiryo UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.Margin = New System.Windows.Forms.Padding(4)
		Me.Name = "frmTempLinearTool"
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
		Me.Text = "Linear tool"
		Me.GroupBox1.ResumeLayout(false)
		Me.GroupBox1.PerformLayout
		CType(Me.nudSpeed,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.nudEndTemp,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.nudStartTemp,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.nudWaitTime,System.ComponentModel.ISupportInitialize).EndInit
		Me.ResumeLayout(false)

End Sub

	Friend WithEvents GroupBox1 As GroupBox
	Friend WithEvents nudSpeed As NumericUpDown
	Friend WithEvents nudEndTemp As NumericUpDown
	Friend WithEvents nudStartTemp As NumericUpDown
	Friend WithEvents nudWaitTime As NumericUpDown
	Friend WithEvents lblSpeed As Label
	Friend WithEvents lblEndTime As Label
	Friend WithEvents lblStartTime As Label
	Friend WithEvents lblWaitTime As Label
	Friend WithEvents btnCansel As Button
	Friend WithEvents btnOk As Button
End Class
