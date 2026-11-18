<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmSetup
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
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmSetup))
		Me.btnStart = New System.Windows.Forms.Button()
		Me.tcVersion = New System.Windows.Forms.TabControl()
		Me.TabPage1 = New System.Windows.Forms.TabPage()
		Me.dgvTVersion = New System.Windows.Forms.DataGridView()
		Me.TabPage2 = New System.Windows.Forms.TabPage()
		Me.dgvMVersion = New System.Windows.Forms.DataGridView()
		Me.bgwSetup = New System.ComponentModel.BackgroundWorker()
		Me.gbxCurrentInfo = New System.Windows.Forms.GroupBox()
		Me.GroupBox2 = New System.Windows.Forms.GroupBox()
		Me.lblCurrentVerM = New System.Windows.Forms.Label()
		Me.GroupBox1 = New System.Windows.Forms.GroupBox()
		Me.lblCurrentVerT = New System.Windows.Forms.Label()
		Me.sstMain = New System.Windows.Forms.StatusStrip()
		Me.sstLabel1 = New System.Windows.Forms.ToolStripStatusLabel()
		Me.btnSwitch = New System.Windows.Forms.Button()
		Me.ckbSilent = New System.Windows.Forms.CheckBox()
		Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
		Me.SplitContainer2 = New System.Windows.Forms.SplitContainer()
		Me.tcVersion.SuspendLayout
		Me.TabPage1.SuspendLayout
		CType(Me.dgvTVersion,System.ComponentModel.ISupportInitialize).BeginInit
		Me.TabPage2.SuspendLayout
		CType(Me.dgvMVersion,System.ComponentModel.ISupportInitialize).BeginInit
		Me.gbxCurrentInfo.SuspendLayout
		Me.GroupBox2.SuspendLayout
		Me.GroupBox1.SuspendLayout
		Me.sstMain.SuspendLayout
		CType(Me.SplitContainer1,System.ComponentModel.ISupportInitialize).BeginInit
		Me.SplitContainer1.Panel1.SuspendLayout
		Me.SplitContainer1.Panel2.SuspendLayout
		Me.SplitContainer1.SuspendLayout
		CType(Me.SplitContainer2,System.ComponentModel.ISupportInitialize).BeginInit
		Me.SplitContainer2.Panel1.SuspendLayout
		Me.SplitContainer2.Panel2.SuspendLayout
		Me.SplitContainer2.SuspendLayout
		Me.SuspendLayout
		'
		'btnStart
		'
		Me.btnStart.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left)  _
            Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
		Me.btnStart.Location = New System.Drawing.Point(13, 19)
		Me.btnStart.Name = "btnStart"
		Me.btnStart.Size = New System.Drawing.Size(119, 34)
		Me.btnStart.TabIndex = 0
		Me.btnStart.Text = "&Setup"
		Me.btnStart.UseVisualStyleBackColor = true
		'
		'tcVersion
		'
		Me.tcVersion.Controls.Add(Me.TabPage1)
		Me.tcVersion.Controls.Add(Me.TabPage2)
		Me.tcVersion.Dock = System.Windows.Forms.DockStyle.Fill
		Me.tcVersion.Location = New System.Drawing.Point(0, 0)
		Me.tcVersion.Name = "tcVersion"
		Me.tcVersion.SelectedIndex = 0
		Me.tcVersion.Size = New System.Drawing.Size(694, 491)
		Me.tcVersion.TabIndex = 1
		'
		'TabPage1
		'
		Me.TabPage1.Controls.Add(Me.dgvTVersion)
		Me.TabPage1.Location = New System.Drawing.Point(4, 26)
		Me.TabPage1.Name = "TabPage1"
		Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
		Me.TabPage1.Size = New System.Drawing.Size(686, 461)
		Me.TabPage1.TabIndex = 0
		Me.TabPage1.Text = "TabPage1"
		Me.TabPage1.UseVisualStyleBackColor = true
		'
		'dgvTVersion
		'
		Me.dgvTVersion.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
		Me.dgvTVersion.Dock = System.Windows.Forms.DockStyle.Fill
		Me.dgvTVersion.Location = New System.Drawing.Point(3, 3)
		Me.dgvTVersion.Name = "dgvTVersion"
		Me.dgvTVersion.Size = New System.Drawing.Size(680, 455)
		Me.dgvTVersion.TabIndex = 0
		'
		'TabPage2
		'
		Me.TabPage2.Controls.Add(Me.dgvMVersion)
		Me.TabPage2.Location = New System.Drawing.Point(4, 22)
		Me.TabPage2.Name = "TabPage2"
		Me.TabPage2.Padding = New System.Windows.Forms.Padding(3)
		Me.TabPage2.Size = New System.Drawing.Size(686, 465)
		Me.TabPage2.TabIndex = 1
		Me.TabPage2.Text = "TabPage2"
		Me.TabPage2.UseVisualStyleBackColor = true
		'
		'dgvMVersion
		'
		Me.dgvMVersion.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
		Me.dgvMVersion.Dock = System.Windows.Forms.DockStyle.Fill
		Me.dgvMVersion.Location = New System.Drawing.Point(3, 3)
		Me.dgvMVersion.Name = "dgvMVersion"
		Me.dgvMVersion.Size = New System.Drawing.Size(680, 459)
		Me.dgvMVersion.TabIndex = 1
		'
		'bgwSetup
		'
		'
		'gbxCurrentInfo
		'
		Me.gbxCurrentInfo.Controls.Add(Me.GroupBox2)
		Me.gbxCurrentInfo.Controls.Add(Me.GroupBox1)
		Me.gbxCurrentInfo.Dock = System.Windows.Forms.DockStyle.Fill
		Me.gbxCurrentInfo.Location = New System.Drawing.Point(0, 0)
		Me.gbxCurrentInfo.Name = "gbxCurrentInfo"
		Me.gbxCurrentInfo.Size = New System.Drawing.Size(694, 81)
		Me.gbxCurrentInfo.TabIndex = 3
		Me.gbxCurrentInfo.TabStop = false
		Me.gbxCurrentInfo.Text = "Current installation status"
		'
		'GroupBox2
		'
		Me.GroupBox2.Controls.Add(Me.lblCurrentVerM)
		Me.GroupBox2.Location = New System.Drawing.Point(355, 20)
		Me.GroupBox2.Name = "GroupBox2"
		Me.GroupBox2.Size = New System.Drawing.Size(342, 51)
		Me.GroupBox2.TabIndex = 1
		Me.GroupBox2.TabStop = false
		Me.GroupBox2.Text = "Sizuka Data Manager"
		'
		'lblCurrentVerM
		'
		Me.lblCurrentVerM.AutoSize = true
		Me.lblCurrentVerM.Font = New System.Drawing.Font("Meiryo UI", 12!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.lblCurrentVerM.Location = New System.Drawing.Point(19, 24)
		Me.lblCurrentVerM.Name = "lblCurrentVerM"
		Me.lblCurrentVerM.Size = New System.Drawing.Size(117, 20)
		Me.lblCurrentVerM.TabIndex = 1
		Me.lblCurrentVerM.Text = "Not installed"
		'
		'GroupBox1
		'
		Me.GroupBox1.Controls.Add(Me.lblCurrentVerT)
		Me.GroupBox1.Location = New System.Drawing.Point(7, 20)
		Me.GroupBox1.Name = "GroupBox1"
		Me.GroupBox1.Size = New System.Drawing.Size(342, 51)
		Me.GroupBox1.TabIndex = 0
		Me.GroupBox1.TabStop = false
		Me.GroupBox1.Text = "Sizuka Tool"
		'
		'lblCurrentVerT
		'
		Me.lblCurrentVerT.AutoSize = true
		Me.lblCurrentVerT.Font = New System.Drawing.Font("Meiryo UI", 12!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.lblCurrentVerT.Location = New System.Drawing.Point(17, 24)
		Me.lblCurrentVerT.Name = "lblCurrentVerT"
		Me.lblCurrentVerT.Size = New System.Drawing.Size(117, 20)
		Me.lblCurrentVerT.TabIndex = 0
		Me.lblCurrentVerT.Text = "Not installed"
		'
		'sstMain
		'
		Me.sstMain.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.sstLabel1})
		Me.sstMain.Location = New System.Drawing.Point(0, 576)
		Me.sstMain.Name = "sstMain"
		Me.sstMain.Size = New System.Drawing.Size(842, 22)
		Me.sstMain.SizingGrip = false
		Me.sstMain.TabIndex = 4
		Me.sstMain.Text = "StatusStrip1"
		'
		'sstLabel1
		'
		Me.sstLabel1.Name = "sstLabel1"
		Me.sstLabel1.Size = New System.Drawing.Size(119, 17)
		Me.sstLabel1.Text = "ToolStripStatusLabel1"
		'
		'btnSwitch
		'
		Me.btnSwitch.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left)  _
            Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
		Me.btnSwitch.Location = New System.Drawing.Point(13, 107)
		Me.btnSwitch.Name = "btnSwitch"
		Me.btnSwitch.Size = New System.Drawing.Size(119, 36)
		Me.btnSwitch.TabIndex = 5
		Me.btnSwitch.Text = "S&witching"
		Me.btnSwitch.UseVisualStyleBackColor = true
		'
		'ckbSilent
		'
		Me.ckbSilent.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left)  _
            Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
		Me.ckbSilent.AutoSize = true
		Me.ckbSilent.Font = New System.Drawing.Font("Meiryo UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.ckbSilent.Location = New System.Drawing.Point(13, 80)
		Me.ckbSilent.Name = "ckbSilent"
		Me.ckbSilent.Size = New System.Drawing.Size(80, 21)
		Me.ckbSilent.TabIndex = 6
		Me.ckbSilent.Text = "Silent UI"
		Me.ckbSilent.UseVisualStyleBackColor = true
		'
		'SplitContainer1
		'
		Me.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill
		Me.SplitContainer1.Location = New System.Drawing.Point(0, 0)
		Me.SplitContainer1.Name = "SplitContainer1"
		'
		'SplitContainer1.Panel1
		'
		Me.SplitContainer1.Panel1.Controls.Add(Me.SplitContainer2)
		'
		'SplitContainer1.Panel2
		'
		Me.SplitContainer1.Panel2.Controls.Add(Me.btnStart)
		Me.SplitContainer1.Panel2.Controls.Add(Me.ckbSilent)
		Me.SplitContainer1.Panel2.Controls.Add(Me.btnSwitch)
		Me.SplitContainer1.Size = New System.Drawing.Size(842, 576)
		Me.SplitContainer1.SplitterDistance = 694
		Me.SplitContainer1.TabIndex = 7
		'
		'SplitContainer2
		'
		Me.SplitContainer2.Dock = System.Windows.Forms.DockStyle.Fill
		Me.SplitContainer2.Location = New System.Drawing.Point(0, 0)
		Me.SplitContainer2.Name = "SplitContainer2"
		Me.SplitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal
		'
		'SplitContainer2.Panel1
		'
		Me.SplitContainer2.Panel1.Controls.Add(Me.gbxCurrentInfo)
		'
		'SplitContainer2.Panel2
		'
		Me.SplitContainer2.Panel2.Controls.Add(Me.tcVersion)
		Me.SplitContainer2.Size = New System.Drawing.Size(694, 576)
		Me.SplitContainer2.SplitterDistance = 81
		Me.SplitContainer2.TabIndex = 4
		'
		'frmSetup
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(8!, 17!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(842, 598)
		Me.Controls.Add(Me.SplitContainer1)
		Me.Controls.Add(Me.sstMain)
		Me.Font = New System.Drawing.Font("Meiryo UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.Icon = CType(resources.GetObject("$this.Icon"),System.Drawing.Icon)
		Me.Margin = New System.Windows.Forms.Padding(4)
		Me.Name = "frmSetup"
		Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show
		Me.Text = "Form1"
		Me.tcVersion.ResumeLayout(false)
		Me.TabPage1.ResumeLayout(false)
		CType(Me.dgvTVersion,System.ComponentModel.ISupportInitialize).EndInit
		Me.TabPage2.ResumeLayout(false)
		CType(Me.dgvMVersion,System.ComponentModel.ISupportInitialize).EndInit
		Me.gbxCurrentInfo.ResumeLayout(false)
		Me.GroupBox2.ResumeLayout(false)
		Me.GroupBox2.PerformLayout
		Me.GroupBox1.ResumeLayout(false)
		Me.GroupBox1.PerformLayout
		Me.sstMain.ResumeLayout(false)
		Me.sstMain.PerformLayout
		Me.SplitContainer1.Panel1.ResumeLayout(false)
		Me.SplitContainer1.Panel2.ResumeLayout(false)
		Me.SplitContainer1.Panel2.PerformLayout
		CType(Me.SplitContainer1,System.ComponentModel.ISupportInitialize).EndInit
		Me.SplitContainer1.ResumeLayout(false)
		Me.SplitContainer2.Panel1.ResumeLayout(false)
		Me.SplitContainer2.Panel2.ResumeLayout(false)
		CType(Me.SplitContainer2,System.ComponentModel.ISupportInitialize).EndInit
		Me.SplitContainer2.ResumeLayout(false)
		Me.ResumeLayout(false)
		Me.PerformLayout

End Sub

	Friend WithEvents btnStart As Button
	Friend WithEvents tcVersion As TabControl
	Friend WithEvents TabPage1 As TabPage
	Friend WithEvents dgvTVersion As DataGridView
	Friend WithEvents TabPage2 As TabPage
	Friend WithEvents dgvMVersion As DataGridView
	Friend WithEvents bgwSetup As System.ComponentModel.BackgroundWorker
	Friend WithEvents gbxCurrentInfo As GroupBox
	Friend WithEvents GroupBox2 As GroupBox
	Friend WithEvents GroupBox1 As GroupBox
	Friend WithEvents lblCurrentVerM As Label
	Friend WithEvents lblCurrentVerT As Label
	Friend WithEvents sstMain As StatusStrip
	Friend WithEvents sstLabel1 As ToolStripStatusLabel
	Friend WithEvents btnSwitch As Button
	Friend WithEvents ckbSilent As CheckBox
	Friend WithEvents SplitContainer1 As SplitContainer
	Friend WithEvents SplitContainer2 As SplitContainer
End Class
