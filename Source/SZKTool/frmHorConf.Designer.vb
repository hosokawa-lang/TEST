<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmHorConf
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
		Me.btnStart = New System.Windows.Forms.Button()
		Me.btnStop = New System.Windows.Forms.Button()
		Me.dgvResult = New System.Windows.Forms.DataGridView()
		Me.btnClose = New System.Windows.Forms.Button()
		Me.btnReload = New System.Windows.Forms.Button()
		Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
		Me.ToolStripStatusLabel1 = New System.Windows.Forms.ToolStripStatusLabel()
		Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
		Me.btnZeroAdj = New System.Windows.Forms.Button()
		Me.btnXYMove = New System.Windows.Forms.Button()
		Me.gpxOtherInf = New System.Windows.Forms.GroupBox()
		Me.lblMinZLabel = New System.Windows.Forms.Label()
		Me.lblMaxZLabel = New System.Windows.Forms.Label()
		Me.lblOriginZLabel = New System.Windows.Forms.Label()
		Me.lblMinZVal = New System.Windows.Forms.Label()
		Me.lblMaxZVal = New System.Windows.Forms.Label()
		Me.lblOriginZVal = New System.Windows.Forms.Label()
		Me.lblCurrYPosLabel = New System.Windows.Forms.Label()
		Me.lblCurrXPosLabel = New System.Windows.Forms.Label()
		Me.lblCurrZPosLabel = New System.Windows.Forms.Label()
		Me.lblCurrPosYVal = New System.Windows.Forms.Label()
		Me.lblCurrPosXVal = New System.Windows.Forms.Label()
		Me.lblCurrPosZVal = New System.Windows.Forms.Label()
		Me.gpxCurrPos = New System.Windows.Forms.GroupBox()
		CType(Me.dgvResult,System.ComponentModel.ISupportInitialize).BeginInit
		Me.StatusStrip1.SuspendLayout
		Me.gpxOtherInf.SuspendLayout
		Me.gpxCurrPos.SuspendLayout
		Me.SuspendLayout
		'
		'btnStart
		'
		Me.btnStart.Font = New System.Drawing.Font("Meiryo UI", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.btnStart.Image = Global.SZKTool.My.Resources.Resources.PlayHS
		Me.btnStart.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me.btnStart.Location = New System.Drawing.Point(905, 15)
		Me.btnStart.Margin = New System.Windows.Forms.Padding(4)
		Me.btnStart.Name = "btnStart"
		Me.btnStart.Size = New System.Drawing.Size(162, 44)
		Me.btnStart.TabIndex = 0
		Me.btnStart.Text = "Start"
		Me.btnStart.UseVisualStyleBackColor = true
		'
		'btnStop
		'
		Me.btnStop.Font = New System.Drawing.Font("Meiryo UI", 15.75!)
		Me.btnStop.Image = Global.SZKTool.My.Resources.Resources.stopbtn
		Me.btnStop.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me.btnStop.Location = New System.Drawing.Point(905, 66)
		Me.btnStop.Margin = New System.Windows.Forms.Padding(4)
		Me.btnStop.Name = "btnStop"
		Me.btnStop.Size = New System.Drawing.Size(162, 46)
		Me.btnStop.TabIndex = 1
		Me.btnStop.Text = "Stop"
		Me.btnStop.UseVisualStyleBackColor = true
		'
		'dgvResult
		'
		Me.dgvResult.AllowUserToAddRows = false
		Me.dgvResult.AllowUserToDeleteRows = false
		Me.dgvResult.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
		Me.dgvResult.Location = New System.Drawing.Point(6, 15)
		Me.dgvResult.Margin = New System.Windows.Forms.Padding(4)
		Me.dgvResult.Name = "dgvResult"
		Me.dgvResult.RowTemplate.Height = 21
		Me.dgvResult.Size = New System.Drawing.Size(568, 541)
		Me.dgvResult.TabIndex = 6
		'
		'btnClose
		'
		Me.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel
		Me.btnClose.Font = New System.Drawing.Font("Meiryo UI", 15.75!)
		Me.btnClose.Location = New System.Drawing.Point(905, 512)
		Me.btnClose.Margin = New System.Windows.Forms.Padding(4)
		Me.btnClose.Name = "btnClose"
		Me.btnClose.Size = New System.Drawing.Size(162, 44)
		Me.btnClose.TabIndex = 5
		Me.btnClose.Text = "Close"
		Me.btnClose.UseVisualStyleBackColor = true
		'
		'btnReload
		'
		Me.btnReload.Font = New System.Drawing.Font("Meiryo UI", 15.75!)
		Me.btnReload.Location = New System.Drawing.Point(905, 280)
		Me.btnReload.Margin = New System.Windows.Forms.Padding(4)
		Me.btnReload.Name = "btnReload"
		Me.btnReload.Size = New System.Drawing.Size(162, 50)
		Me.btnReload.TabIndex = 4
		Me.btnReload.Text = "Reload"
		Me.btnReload.UseVisualStyleBackColor = true
		'
		'StatusStrip1
		'
		Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripStatusLabel1})
		Me.StatusStrip1.Location = New System.Drawing.Point(0, 577)
		Me.StatusStrip1.Name = "StatusStrip1"
		Me.StatusStrip1.Padding = New System.Windows.Forms.Padding(1, 0, 16, 0)
		Me.StatusStrip1.Size = New System.Drawing.Size(1077, 25)
		Me.StatusStrip1.TabIndex = 3
		Me.StatusStrip1.Text = "StatusStrip1"
		'
		'ToolStripStatusLabel1
		'
		Me.ToolStripStatusLabel1.Font = New System.Drawing.Font("Meiryo UI", 12!)
		Me.ToolStripStatusLabel1.Name = "ToolStripStatusLabel1"
		Me.ToolStripStatusLabel1.Size = New System.Drawing.Size(178, 20)
		Me.ToolStripStatusLabel1.Text = "ToolStripStatusLabel1"
		'
		'Timer1
		'
		'
		'btnZeroAdj
		'
		Me.btnZeroAdj.Font = New System.Drawing.Font("Meiryo UI", 15.75!)
		Me.btnZeroAdj.Image = Global.SZKTool.My.Resources.Resources.zerobtn
		Me.btnZeroAdj.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me.btnZeroAdj.Location = New System.Drawing.Point(905, 135)
		Me.btnZeroAdj.Margin = New System.Windows.Forms.Padding(4)
		Me.btnZeroAdj.Name = "btnZeroAdj"
		Me.btnZeroAdj.Size = New System.Drawing.Size(162, 40)
		Me.btnZeroAdj.TabIndex = 2
		Me.btnZeroAdj.Text = "Zero Adj."
		Me.btnZeroAdj.UseVisualStyleBackColor = true
		'
		'btnXYMove
		'
		Me.btnXYMove.Font = New System.Drawing.Font("Meiryo UI", 15.75!)
		Me.btnXYMove.Location = New System.Drawing.Point(905, 212)
		Me.btnXYMove.Margin = New System.Windows.Forms.Padding(4)
		Me.btnXYMove.Name = "btnXYMove"
		Me.btnXYMove.Size = New System.Drawing.Size(162, 47)
		Me.btnXYMove.TabIndex = 3
		Me.btnXYMove.Text = "XY Move"
		Me.btnXYMove.UseVisualStyleBackColor = true
		'
		'gpxOtherInf
		'
		Me.gpxOtherInf.Controls.Add(Me.lblMinZLabel)
		Me.gpxOtherInf.Controls.Add(Me.lblMaxZLabel)
		Me.gpxOtherInf.Controls.Add(Me.lblOriginZLabel)
		Me.gpxOtherInf.Controls.Add(Me.lblMinZVal)
		Me.gpxOtherInf.Controls.Add(Me.lblMaxZVal)
		Me.gpxOtherInf.Controls.Add(Me.lblOriginZVal)
		Me.gpxOtherInf.Font = New System.Drawing.Font("Meiryo UI", 9.75!)
		Me.gpxOtherInf.Location = New System.Drawing.Point(579, 280)
		Me.gpxOtherInf.Margin = New System.Windows.Forms.Padding(4)
		Me.gpxOtherInf.Name = "gpxOtherInf"
		Me.gpxOtherInf.Padding = New System.Windows.Forms.Padding(4)
		Me.gpxOtherInf.Size = New System.Drawing.Size(314, 276)
		Me.gpxOtherInf.TabIndex = 4
		Me.gpxOtherInf.TabStop = false
		Me.gpxOtherInf.Text = "Other Information"
		'
		'lblMinZLabel
		'
		Me.lblMinZLabel.AutoSize = true
		Me.lblMinZLabel.Font = New System.Drawing.Font("Meiryo UI", 12!)
		Me.lblMinZLabel.Location = New System.Drawing.Point(8, 189)
		Me.lblMinZLabel.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
		Me.lblMinZLabel.Name = "lblMinZLabel"
		Me.lblMinZLabel.Size = New System.Drawing.Size(229, 20)
		Me.lblMinZLabel.TabIndex = 1
		Me.lblMinZLabel.Text = "Z Position of Min Value [um]"
		'
		'lblMaxZLabel
		'
		Me.lblMaxZLabel.AutoSize = true
		Me.lblMaxZLabel.Font = New System.Drawing.Font("Meiryo UI", 12!)
		Me.lblMaxZLabel.Location = New System.Drawing.Point(6, 105)
		Me.lblMaxZLabel.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
		Me.lblMaxZLabel.Name = "lblMaxZLabel"
		Me.lblMaxZLabel.Size = New System.Drawing.Size(233, 20)
		Me.lblMaxZLabel.TabIndex = 1
		Me.lblMaxZLabel.Text = "Z Position of Max Value [um]"
		'
		'lblOriginZLabel
		'
		Me.lblOriginZLabel.AutoSize = true
		Me.lblOriginZLabel.Font = New System.Drawing.Font("Meiryo UI", 12!)
		Me.lblOriginZLabel.Location = New System.Drawing.Point(8, 28)
		Me.lblOriginZLabel.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
		Me.lblOriginZLabel.Name = "lblOriginZLabel"
		Me.lblOriginZLabel.Size = New System.Drawing.Size(233, 20)
		Me.lblOriginZLabel.TabIndex = 1
		Me.lblOriginZLabel.Text = "Z Position of X-Y Origin [um]"
		'
		'lblMinZVal
		'
		Me.lblMinZVal.BackColor = System.Drawing.SystemColors.ControlLightLight
		Me.lblMinZVal.Font = New System.Drawing.Font("MS UI Gothic", 18!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.lblMinZVal.Location = New System.Drawing.Point(144, 219)
		Me.lblMinZVal.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
		Me.lblMinZVal.Name = "lblMinZVal"
		Me.lblMinZVal.Size = New System.Drawing.Size(148, 35)
		Me.lblMinZVal.TabIndex = 0
		Me.lblMinZVal.Text = "Label1"
		Me.lblMinZVal.TextAlign = System.Drawing.ContentAlignment.MiddleRight
		'
		'lblMaxZVal
		'
		Me.lblMaxZVal.BackColor = System.Drawing.SystemColors.ControlLightLight
		Me.lblMaxZVal.Font = New System.Drawing.Font("MS UI Gothic", 18!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.lblMaxZVal.Location = New System.Drawing.Point(144, 129)
		Me.lblMaxZVal.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
		Me.lblMaxZVal.Name = "lblMaxZVal"
		Me.lblMaxZVal.Size = New System.Drawing.Size(148, 35)
		Me.lblMaxZVal.TabIndex = 0
		Me.lblMaxZVal.Text = "Label1"
		Me.lblMaxZVal.TextAlign = System.Drawing.ContentAlignment.MiddleRight
		'
		'lblOriginZVal
		'
		Me.lblOriginZVal.BackColor = System.Drawing.SystemColors.ControlLightLight
		Me.lblOriginZVal.Font = New System.Drawing.Font("MS UI Gothic", 18!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.lblOriginZVal.Location = New System.Drawing.Point(144, 51)
		Me.lblOriginZVal.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
		Me.lblOriginZVal.Name = "lblOriginZVal"
		Me.lblOriginZVal.Size = New System.Drawing.Size(148, 35)
		Me.lblOriginZVal.TabIndex = 0
		Me.lblOriginZVal.Text = "Label1"
		Me.lblOriginZVal.TextAlign = System.Drawing.ContentAlignment.MiddleRight
		'
		'lblCurrYPosLabel
		'
		Me.lblCurrYPosLabel.AutoSize = true
		Me.lblCurrYPosLabel.Font = New System.Drawing.Font("Meiryo UI", 12!)
		Me.lblCurrYPosLabel.Location = New System.Drawing.Point(8, 178)
		Me.lblCurrYPosLabel.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
		Me.lblCurrYPosLabel.Name = "lblCurrYPosLabel"
		Me.lblCurrYPosLabel.Size = New System.Drawing.Size(212, 20)
		Me.lblCurrYPosLabel.TabIndex = 8
		Me.lblCurrYPosLabel.Text = "Y Position of current [um]"
		'
		'lblCurrXPosLabel
		'
		Me.lblCurrXPosLabel.AutoSize = true
		Me.lblCurrXPosLabel.Font = New System.Drawing.Font("Meiryo UI", 12!)
		Me.lblCurrXPosLabel.Location = New System.Drawing.Point(8, 103)
		Me.lblCurrXPosLabel.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
		Me.lblCurrXPosLabel.Name = "lblCurrXPosLabel"
		Me.lblCurrXPosLabel.Size = New System.Drawing.Size(213, 20)
		Me.lblCurrXPosLabel.TabIndex = 9
		Me.lblCurrXPosLabel.Text = "X Position of current [um]"
		'
		'lblCurrZPosLabel
		'
		Me.lblCurrZPosLabel.AutoSize = true
		Me.lblCurrZPosLabel.Font = New System.Drawing.Font("Meiryo UI", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.lblCurrZPosLabel.Location = New System.Drawing.Point(8, 29)
		Me.lblCurrZPosLabel.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
		Me.lblCurrZPosLabel.Name = "lblCurrZPosLabel"
		Me.lblCurrZPosLabel.Size = New System.Drawing.Size(212, 20)
		Me.lblCurrZPosLabel.TabIndex = 10
		Me.lblCurrZPosLabel.Text = "Z Position of current [um]"
		'
		'lblCurrPosYVal
		'
		Me.lblCurrPosYVal.BackColor = System.Drawing.SystemColors.ControlLightLight
		Me.lblCurrPosYVal.Font = New System.Drawing.Font("MS UI Gothic", 18!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.lblCurrPosYVal.Location = New System.Drawing.Point(148, 201)
		Me.lblCurrPosYVal.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
		Me.lblCurrPosYVal.Name = "lblCurrPosYVal"
		Me.lblCurrPosYVal.Size = New System.Drawing.Size(148, 35)
		Me.lblCurrPosYVal.TabIndex = 5
		Me.lblCurrPosYVal.Text = "Label1"
		Me.lblCurrPosYVal.TextAlign = System.Drawing.ContentAlignment.MiddleRight
		'
		'lblCurrPosXVal
		'
		Me.lblCurrPosXVal.BackColor = System.Drawing.SystemColors.ControlLightLight
		Me.lblCurrPosXVal.Font = New System.Drawing.Font("MS UI Gothic", 18!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.lblCurrPosXVal.Location = New System.Drawing.Point(148, 126)
		Me.lblCurrPosXVal.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
		Me.lblCurrPosXVal.Name = "lblCurrPosXVal"
		Me.lblCurrPosXVal.Size = New System.Drawing.Size(148, 35)
		Me.lblCurrPosXVal.TabIndex = 6
		Me.lblCurrPosXVal.Text = "Label1"
		Me.lblCurrPosXVal.TextAlign = System.Drawing.ContentAlignment.MiddleRight
		'
		'lblCurrPosZVal
		'
		Me.lblCurrPosZVal.BackColor = System.Drawing.SystemColors.ControlLightLight
		Me.lblCurrPosZVal.Font = New System.Drawing.Font("MS UI Gothic", 18!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.lblCurrPosZVal.Location = New System.Drawing.Point(148, 57)
		Me.lblCurrPosZVal.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
		Me.lblCurrPosZVal.Name = "lblCurrPosZVal"
		Me.lblCurrPosZVal.Size = New System.Drawing.Size(148, 35)
		Me.lblCurrPosZVal.TabIndex = 7
		Me.lblCurrPosZVal.Text = "Label1"
		Me.lblCurrPosZVal.TextAlign = System.Drawing.ContentAlignment.MiddleRight
		'
		'gpxCurrPos
		'
		Me.gpxCurrPos.Controls.Add(Me.lblCurrPosYVal)
		Me.gpxCurrPos.Controls.Add(Me.lblCurrYPosLabel)
		Me.gpxCurrPos.Controls.Add(Me.lblCurrPosZVal)
		Me.gpxCurrPos.Controls.Add(Me.lblCurrXPosLabel)
		Me.gpxCurrPos.Controls.Add(Me.lblCurrPosXVal)
		Me.gpxCurrPos.Controls.Add(Me.lblCurrZPosLabel)
		Me.gpxCurrPos.Font = New System.Drawing.Font("Meiryo UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.gpxCurrPos.Location = New System.Drawing.Point(579, 15)
		Me.gpxCurrPos.Margin = New System.Windows.Forms.Padding(4)
		Me.gpxCurrPos.Name = "gpxCurrPos"
		Me.gpxCurrPos.Padding = New System.Windows.Forms.Padding(4)
		Me.gpxCurrPos.Size = New System.Drawing.Size(314, 257)
		Me.gpxCurrPos.TabIndex = 11
		Me.gpxCurrPos.TabStop = false
		Me.gpxCurrPos.Text = "Current position"
		'
		'frmHorConf
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(7!, 15!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.CancelButton = Me.btnClose
		Me.ClientSize = New System.Drawing.Size(1077, 602)
		Me.ControlBox = false
		Me.Controls.Add(Me.gpxCurrPos)
		Me.Controls.Add(Me.gpxOtherInf)
		Me.Controls.Add(Me.StatusStrip1)
		Me.Controls.Add(Me.btnClose)
		Me.Controls.Add(Me.dgvResult)
		Me.Controls.Add(Me.btnReload)
		Me.Controls.Add(Me.btnXYMove)
		Me.Controls.Add(Me.btnZeroAdj)
		Me.Controls.Add(Me.btnStop)
		Me.Controls.Add(Me.btnStart)
		Me.Font = New System.Drawing.Font("Meiryo UI", 9!)
		Me.KeyPreview = true
		Me.Margin = New System.Windows.Forms.Padding(4)
		Me.MaximizeBox = false
		Me.MinimizeBox = false
		Me.Name = "frmHorConf"
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
		Me.Text = "Horizontal Confirmation"
		CType(Me.dgvResult,System.ComponentModel.ISupportInitialize).EndInit
		Me.StatusStrip1.ResumeLayout(false)
		Me.StatusStrip1.PerformLayout
		Me.gpxOtherInf.ResumeLayout(false)
		Me.gpxOtherInf.PerformLayout
		Me.gpxCurrPos.ResumeLayout(false)
		Me.gpxCurrPos.PerformLayout
		Me.ResumeLayout(false)
		Me.PerformLayout

End Sub

	Friend WithEvents btnStart As Button
	Friend WithEvents btnStop As Button
	Friend WithEvents dgvResult As DataGridView
	Friend WithEvents btnClose As Button
	Friend WithEvents btnReload As Button
	Friend WithEvents StatusStrip1 As StatusStrip
	Friend WithEvents ToolStripStatusLabel1 As ToolStripStatusLabel
	Friend WithEvents Timer1 As Timer
	Friend WithEvents btnZeroAdj As Button
	Friend WithEvents btnXYMove As Button
	Friend WithEvents gpxOtherInf As GroupBox
	Friend WithEvents lblMinZLabel As Label
	Friend WithEvents lblMaxZLabel As Label
	Friend WithEvents lblOriginZLabel As Label
	Friend WithEvents lblMinZVal As Label
	Friend WithEvents lblMaxZVal As Label
	Friend WithEvents lblOriginZVal As Label
	Friend WithEvents lblCurrYPosLabel As Label
	Friend WithEvents lblCurrXPosLabel As Label
	Friend WithEvents lblCurrZPosLabel As Label
	Friend WithEvents lblCurrPosYVal As Label
	Friend WithEvents lblCurrPosXVal As Label
	Friend WithEvents lblCurrPosZVal As Label
	Friend WithEvents gpxCurrPos As GroupBox
End Class
