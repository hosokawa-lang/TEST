<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmCalibFull
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
		Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
		Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
		Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
		Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
		Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
		Me.lblMsg = New System.Windows.Forms.ToolStripStatusLabel()
		Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
		Me.btnXStop = New System.Windows.Forms.Button()
		Me.btnXExec = New System.Windows.Forms.Button()
		Me.timMain = New System.Windows.Forms.Timer(Me.components)
		Me.dgvCPara = New System.Windows.Forms.DataGridView()
		Me.btnXSel = New System.Windows.Forms.Button()
		Me.btnCSel = New System.Windows.Forms.Button()
		Me.gbxSetting = New System.Windows.Forms.GroupBox()
		Me.btnXAllClear = New System.Windows.Forms.Button()
		Me.btnXClear = New System.Windows.Forms.Button()
		Me.btnCClear = New System.Windows.Forms.Button()
		Me.btnCStop = New System.Windows.Forms.Button()
		Me.btnCExec = New System.Windows.Forms.Button()
		Me.gbxMotionSetting = New System.Windows.Forms.GroupBox()
		Me.Panel2 = New System.Windows.Forms.Panel()
		Me.Chart2 = New System.Windows.Forms.DataVisualization.Charting.Chart()
		Me.lblMeasDateVal = New System.Windows.Forms.Label()
		Me.lblMeasDateLabel = New System.Windows.Forms.Label()
		Me.Label1 = New System.Windows.Forms.Label()
		Me.tcPara = New System.Windows.Forms.TabControl()
		Me.tpCircleMotion = New System.Windows.Forms.TabPage()
		Me.tpLinearMotion = New System.Windows.Forms.TabPage()
		Me.dgvXPara = New System.Windows.Forms.DataGridView()
		Me.tpPointMotion = New System.Windows.Forms.TabPage()
		Me.dgvPPara = New System.Windows.Forms.DataGridView()
		Me.tpCirclePhaseX = New System.Windows.Forms.TabPage()
		Me.dgvCParaPhX = New System.Windows.Forms.DataGridView()
		Me.tpCirclePhaseY = New System.Windows.Forms.TabPage()
		Me.dgvCParaPhY = New System.Windows.Forms.DataGridView()
		Me.tpLinearPhase = New System.Windows.Forms.TabPage()
		Me.dgvXParaPh = New System.Windows.Forms.DataGridView()
		Me.tpLinearPhaseZ = New System.Windows.Forms.TabPage()
		Me.dgvPParaPh = New System.Windows.Forms.DataGridView()
		Me.btnCAllSelect = New System.Windows.Forms.Button()
		Me.lblAutoPeeling = New System.Windows.Forms.Label()
		Me.btnCAllClear = New System.Windows.Forms.Button()
		Me.ChartStress = New System.Windows.Forms.DataVisualization.Charting.Chart()
		Me.ChartHosei = New System.Windows.Forms.DataVisualization.Charting.Chart()
		Me.btnZeroAdj = New System.Windows.Forms.Button()
		Me.btnExec = New System.Windows.Forms.Button()
		Me.btnStop = New System.Windows.Forms.Button()
		Me.nudSampleTime = New System.Windows.Forms.NumericUpDown()
		Me.lblSapleTime = New System.Windows.Forms.Label()
		Me.nudAcquisitionTime = New System.Windows.Forms.NumericUpDown()
		Me.lblAcquisitionTime = New System.Windows.Forms.Label()
		Me.GroupBox2 = New System.Windows.Forms.GroupBox()
		Me.btnSOCheck = New System.Windows.Forms.Button()
		Me.btnTemperature = New System.Windows.Forms.Button()
		Me.btnOrigin = New System.Windows.Forms.Button()
		Me.GroupBox3 = New System.Windows.Forms.GroupBox()
		Me.nudPreTemp = New System.Windows.Forms.NumericUpDown()
		Me.nudPeelingTemp = New System.Windows.Forms.NumericUpDown()
		Me.lblPresetTemp = New System.Windows.Forms.Label()
		Me.lblPeelingTemp = New System.Windows.Forms.Label()
		Me.dgvCurrVal = New System.Windows.Forms.DataGridView()
		Me.Column1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
		Me.Column2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
		Me.GroupBox4 = New System.Windows.Forms.GroupBox()
		Me.lblInputGap = New System.Windows.Forms.Label()
		Me.nudInputGap = New System.Windows.Forms.NumericUpDown()
		Me.nudXYWait = New System.Windows.Forms.NumericUpDown()
		Me.nudRisingHPWait = New System.Windows.Forms.NumericUpDown()
		Me.lblHPUpWait = New System.Windows.Forms.Label()
		Me.lblXYWait = New System.Windows.Forms.Label()
		Me.nudMeasureGap = New System.Windows.Forms.NumericUpDown()
		Me.lblMeasureGap = New System.Windows.Forms.Label()
		Me.StatusStrip1.SuspendLayout
		CType(Me.dgvCPara,System.ComponentModel.ISupportInitialize).BeginInit
		Me.gbxSetting.SuspendLayout
		Me.gbxMotionSetting.SuspendLayout
		Me.Panel2.SuspendLayout
		CType(Me.Chart2,System.ComponentModel.ISupportInitialize).BeginInit
		Me.tcPara.SuspendLayout
		Me.tpCircleMotion.SuspendLayout
		Me.tpLinearMotion.SuspendLayout
		CType(Me.dgvXPara,System.ComponentModel.ISupportInitialize).BeginInit
		Me.tpPointMotion.SuspendLayout
		CType(Me.dgvPPara,System.ComponentModel.ISupportInitialize).BeginInit
		Me.tpCirclePhaseX.SuspendLayout
		CType(Me.dgvCParaPhX,System.ComponentModel.ISupportInitialize).BeginInit
		Me.tpCirclePhaseY.SuspendLayout
		CType(Me.dgvCParaPhY,System.ComponentModel.ISupportInitialize).BeginInit
		Me.tpLinearPhase.SuspendLayout
		CType(Me.dgvXParaPh,System.ComponentModel.ISupportInitialize).BeginInit
		Me.tpLinearPhaseZ.SuspendLayout
		CType(Me.dgvPParaPh,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.ChartStress,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.ChartHosei,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.nudSampleTime,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.nudAcquisitionTime,System.ComponentModel.ISupportInitialize).BeginInit
		Me.GroupBox2.SuspendLayout
		Me.GroupBox3.SuspendLayout
		CType(Me.nudPreTemp,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.nudPeelingTemp,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.dgvCurrVal,System.ComponentModel.ISupportInitialize).BeginInit
		Me.GroupBox4.SuspendLayout
		CType(Me.nudInputGap,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.nudXYWait,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.nudRisingHPWait,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.nudMeasureGap,System.ComponentModel.ISupportInitialize).BeginInit
		Me.SuspendLayout
		'
		'lblMsg
		'
		Me.lblMsg.Font = New System.Drawing.Font("Meiryo UI", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.lblMsg.Name = "lblMsg"
		Me.lblMsg.Size = New System.Drawing.Size(0, 17)
		'
		'StatusStrip1
		'
		Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.lblMsg})
		Me.StatusStrip1.Location = New System.Drawing.Point(0, 1019)
		Me.StatusStrip1.Name = "StatusStrip1"
		Me.StatusStrip1.Size = New System.Drawing.Size(1904, 22)
		Me.StatusStrip1.SizingGrip = false
		Me.StatusStrip1.TabIndex = 37
		Me.StatusStrip1.Text = "StatusStrip1"
		'
		'btnXStop
		'
		Me.btnXStop.Anchor = System.Windows.Forms.AnchorStyles.Bottom
		Me.btnXStop.BackColor = System.Drawing.SystemColors.ButtonFace
		Me.btnXStop.Font = New System.Drawing.Font("MS UI Gothic", 9!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.btnXStop.Location = New System.Drawing.Point(1471, 844)
		Me.btnXStop.Name = "btnXStop"
		Me.btnXStop.Size = New System.Drawing.Size(96, 25)
		Me.btnXStop.TabIndex = 5
		Me.btnXStop.Text = "Stop"
		Me.btnXStop.UseVisualStyleBackColor = false
		Me.btnXStop.Visible = false
		'
		'btnXExec
		'
		Me.btnXExec.Anchor = System.Windows.Forms.AnchorStyles.Bottom
		Me.btnXExec.BackColor = System.Drawing.SystemColors.ButtonFace
		Me.btnXExec.Font = New System.Drawing.Font("MS UI Gothic", 9!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.btnXExec.Location = New System.Drawing.Point(1369, 844)
		Me.btnXExec.Name = "btnXExec"
		Me.btnXExec.Size = New System.Drawing.Size(96, 25)
		Me.btnXExec.TabIndex = 4
		Me.btnXExec.Text = "Exec"
		Me.btnXExec.UseVisualStyleBackColor = false
		Me.btnXExec.Visible = false
		'
		'timMain
		'
		'
		'dgvCPara
		'
		Me.dgvCPara.AllowUserToAddRows = false
		Me.dgvCPara.AllowUserToResizeRows = false
		Me.dgvCPara.BackgroundColor = System.Drawing.SystemColors.ControlLightLight
		Me.dgvCPara.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
		Me.dgvCPara.Location = New System.Drawing.Point(0, 0)
		Me.dgvCPara.Name = "dgvCPara"
		Me.dgvCPara.RowTemplate.Height = 21
		Me.dgvCPara.Size = New System.Drawing.Size(1544, 344)
		Me.dgvCPara.TabIndex = 6
		'
		'btnXSel
		'
		Me.btnXSel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left),System.Windows.Forms.AnchorStyles)
		Me.btnXSel.BackColor = System.Drawing.SystemColors.ButtonFace
		Me.btnXSel.Font = New System.Drawing.Font("MS UI Gothic", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.btnXSel.Location = New System.Drawing.Point(10, 825)
		Me.btnXSel.Name = "btnXSel"
		Me.btnXSel.Size = New System.Drawing.Size(117, 40)
		Me.btnXSel.TabIndex = 2
		Me.btnXSel.Text = "Select"
		Me.btnXSel.UseVisualStyleBackColor = false
		'
		'btnCSel
		'
		Me.btnCSel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left),System.Windows.Forms.AnchorStyles)
		Me.btnCSel.BackColor = System.Drawing.SystemColors.ButtonFace
		Me.btnCSel.Font = New System.Drawing.Font("Meiryo UI", 12!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.btnCSel.Location = New System.Drawing.Point(10, 387)
		Me.btnCSel.Name = "btnCSel"
		Me.btnCSel.Size = New System.Drawing.Size(117, 40)
		Me.btnCSel.TabIndex = 7
		Me.btnCSel.Text = "Select"
		Me.btnCSel.UseVisualStyleBackColor = false
		'
		'gbxSetting
		'
		Me.gbxSetting.BackColor = System.Drawing.SystemColors.ControlLight
		Me.gbxSetting.Controls.Add(Me.btnXAllClear)
		Me.gbxSetting.Controls.Add(Me.btnXStop)
		Me.gbxSetting.Controls.Add(Me.btnXClear)
		Me.gbxSetting.Controls.Add(Me.btnXExec)
		Me.gbxSetting.Controls.Add(Me.btnXSel)
		Me.gbxSetting.Font = New System.Drawing.Font("MS UI Gothic", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.gbxSetting.Location = New System.Drawing.Point(6, 10)
		Me.gbxSetting.Name = "gbxSetting"
		Me.gbxSetting.Size = New System.Drawing.Size(1564, 875)
		Me.gbxSetting.TabIndex = 40
		Me.gbxSetting.TabStop = false
		Me.gbxSetting.Text = "X Linear motion"
		Me.gbxSetting.Visible = false
		'
		'btnXAllClear
		'
		Me.btnXAllClear.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left),System.Windows.Forms.AnchorStyles)
		Me.btnXAllClear.BackColor = System.Drawing.SystemColors.ButtonFace
		Me.btnXAllClear.Font = New System.Drawing.Font("MS UI Gothic", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.btnXAllClear.Location = New System.Drawing.Point(274, 824)
		Me.btnXAllClear.Name = "btnXAllClear"
		Me.btnXAllClear.Size = New System.Drawing.Size(117, 40)
		Me.btnXAllClear.TabIndex = 6
		Me.btnXAllClear.Text = "ALL Clear"
		Me.btnXAllClear.UseVisualStyleBackColor = false
		'
		'btnXClear
		'
		Me.btnXClear.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left),System.Windows.Forms.AnchorStyles)
		Me.btnXClear.BackColor = System.Drawing.SystemColors.ButtonFace
		Me.btnXClear.Font = New System.Drawing.Font("MS UI Gothic", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.btnXClear.Location = New System.Drawing.Point(143, 824)
		Me.btnXClear.Name = "btnXClear"
		Me.btnXClear.Size = New System.Drawing.Size(117, 40)
		Me.btnXClear.TabIndex = 3
		Me.btnXClear.Text = "Clear"
		Me.btnXClear.UseVisualStyleBackColor = false
		'
		'btnCClear
		'
		Me.btnCClear.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left),System.Windows.Forms.AnchorStyles)
		Me.btnCClear.BackColor = System.Drawing.SystemColors.ButtonFace
		Me.btnCClear.Font = New System.Drawing.Font("Meiryo UI", 12!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.btnCClear.Location = New System.Drawing.Point(288, 387)
		Me.btnCClear.Name = "btnCClear"
		Me.btnCClear.Size = New System.Drawing.Size(117, 40)
		Me.btnCClear.TabIndex = 8
		Me.btnCClear.Text = "Clear"
		Me.btnCClear.UseVisualStyleBackColor = false
		'
		'btnCStop
		'
		Me.btnCStop.Anchor = System.Windows.Forms.AnchorStyles.Right
		Me.btnCStop.BackColor = System.Drawing.SystemColors.ButtonFace
		Me.btnCStop.Font = New System.Drawing.Font("MS UI Gothic", 9!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.btnCStop.Location = New System.Drawing.Point(1465, 713)
		Me.btnCStop.Name = "btnCStop"
		Me.btnCStop.Size = New System.Drawing.Size(96, 25)
		Me.btnCStop.TabIndex = 10
		Me.btnCStop.Text = "Stop"
		Me.btnCStop.UseVisualStyleBackColor = false
		Me.btnCStop.Visible = false
		'
		'btnCExec
		'
		Me.btnCExec.Anchor = System.Windows.Forms.AnchorStyles.Right
		Me.btnCExec.BackColor = System.Drawing.SystemColors.ButtonFace
		Me.btnCExec.Font = New System.Drawing.Font("MS UI Gothic", 9!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.btnCExec.Location = New System.Drawing.Point(1363, 714)
		Me.btnCExec.Name = "btnCExec"
		Me.btnCExec.Size = New System.Drawing.Size(96, 25)
		Me.btnCExec.TabIndex = 9
		Me.btnCExec.Text = "Exec"
		Me.btnCExec.UseVisualStyleBackColor = false
		Me.btnCExec.Visible = false
		'
		'gbxMotionSetting
		'
		Me.gbxMotionSetting.BackColor = System.Drawing.SystemColors.ControlLight
		Me.gbxMotionSetting.Controls.Add(Me.Panel2)
		Me.gbxMotionSetting.Controls.Add(Me.lblMeasDateVal)
		Me.gbxMotionSetting.Controls.Add(Me.lblMeasDateLabel)
		Me.gbxMotionSetting.Controls.Add(Me.Label1)
		Me.gbxMotionSetting.Controls.Add(Me.tcPara)
		Me.gbxMotionSetting.Controls.Add(Me.btnCAllSelect)
		Me.gbxMotionSetting.Controls.Add(Me.lblAutoPeeling)
		Me.gbxMotionSetting.Controls.Add(Me.btnCAllClear)
		Me.gbxMotionSetting.Controls.Add(Me.btnCClear)
		Me.gbxMotionSetting.Controls.Add(Me.btnCStop)
		Me.gbxMotionSetting.Controls.Add(Me.btnCSel)
		Me.gbxMotionSetting.Controls.Add(Me.btnCExec)
		Me.gbxMotionSetting.Controls.Add(Me.ChartStress)
		Me.gbxMotionSetting.Controls.Add(Me.ChartHosei)
		Me.gbxMotionSetting.Font = New System.Drawing.Font("Meiryo UI", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.gbxMotionSetting.Location = New System.Drawing.Point(6, 12)
		Me.gbxMotionSetting.Name = "gbxMotionSetting"
		Me.gbxMotionSetting.Size = New System.Drawing.Size(1564, 1000)
		Me.gbxMotionSetting.TabIndex = 41
		Me.gbxMotionSetting.TabStop = false
		'
		'Panel2
		'
		Me.Panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		Me.Panel2.Controls.Add(Me.Chart2)
		Me.Panel2.Location = New System.Drawing.Point(37, 463)
		Me.Panel2.Name = "Panel2"
		Me.Panel2.Size = New System.Drawing.Size(167, 70)
		Me.Panel2.TabIndex = 60
		Me.Panel2.Visible = false
		'
		'Chart2
		'
		Me.Chart2.BorderlineColor = System.Drawing.Color.DarkGray
		Me.Chart2.BorderlineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Solid
		Me.Chart2.BorderlineWidth = 2
		Me.Chart2.Location = New System.Drawing.Point(28, 31)
		Me.Chart2.Name = "Chart2"
		Me.Chart2.Size = New System.Drawing.Size(655, 411)
		Me.Chart2.TabIndex = 58
		Me.Chart2.Text = "Chart2"
		'
		'lblMeasDateVal
		'
		Me.lblMeasDateVal.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
		Me.lblMeasDateVal.Location = New System.Drawing.Point(1252, 380)
		Me.lblMeasDateVal.Name = "lblMeasDateVal"
		Me.lblMeasDateVal.Size = New System.Drawing.Size(193, 24)
		Me.lblMeasDateVal.TabIndex = 68
		Me.lblMeasDateVal.Text = "2023/11/22 9:23"
		Me.lblMeasDateVal.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
		'
		'lblMeasDateLabel
		'
		Me.lblMeasDateLabel.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
		Me.lblMeasDateLabel.Location = New System.Drawing.Point(1089, 380)
		Me.lblMeasDateLabel.Name = "lblMeasDateLabel"
		Me.lblMeasDateLabel.Size = New System.Drawing.Size(157, 24)
		Me.lblMeasDateLabel.TabIndex = 68
		Me.lblMeasDateLabel.Text = "Measure Date : "
		Me.lblMeasDateLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
		'
		'Label1
		'
		Me.Label1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
		Me.Label1.AutoSize = true
		Me.Label1.Location = New System.Drawing.Point(1451, 380)
		Me.Label1.Name = "Label1"
		Me.Label1.Size = New System.Drawing.Size(94, 24)
		Me.Label1.TabIndex = 67
		Me.Label1.Text = "Unit : uN"
		'
		'tcPara
		'
		Me.tcPara.Controls.Add(Me.tpCircleMotion)
		Me.tcPara.Controls.Add(Me.tpLinearMotion)
		Me.tcPara.Controls.Add(Me.tpPointMotion)
		Me.tcPara.Controls.Add(Me.tpCirclePhaseX)
		Me.tcPara.Controls.Add(Me.tpCirclePhaseY)
		Me.tcPara.Controls.Add(Me.tpLinearPhase)
		Me.tcPara.Controls.Add(Me.tpLinearPhaseZ)
		Me.tcPara.Location = New System.Drawing.Point(0, 0)
		Me.tcPara.Name = "tcPara"
		Me.tcPara.SelectedIndex = 0
		Me.tcPara.Size = New System.Drawing.Size(1552, 381)
		Me.tcPara.TabIndex = 66
		'
		'tpCircleMotion
		'
		Me.tpCircleMotion.BackColor = System.Drawing.Color.Turquoise
		Me.tpCircleMotion.Controls.Add(Me.dgvCPara)
		Me.tpCircleMotion.Font = New System.Drawing.Font("Meiryo UI", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.tpCircleMotion.ForeColor = System.Drawing.SystemColors.ControlText
		Me.tpCircleMotion.Location = New System.Drawing.Point(4, 33)
		Me.tpCircleMotion.Name = "tpCircleMotion"
		Me.tpCircleMotion.Padding = New System.Windows.Forms.Padding(3)
		Me.tpCircleMotion.Size = New System.Drawing.Size(1544, 344)
		Me.tpCircleMotion.TabIndex = 0
		Me.tpCircleMotion.Text = "XY Circle Motion"
		'
		'tpLinearMotion
		'
		Me.tpLinearMotion.Controls.Add(Me.dgvXPara)
		Me.tpLinearMotion.Font = New System.Drawing.Font("Meiryo UI", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.tpLinearMotion.ForeColor = System.Drawing.SystemColors.ControlText
		Me.tpLinearMotion.Location = New System.Drawing.Point(4, 33)
		Me.tpLinearMotion.Name = "tpLinearMotion"
		Me.tpLinearMotion.Padding = New System.Windows.Forms.Padding(3)
		Me.tpLinearMotion.Size = New System.Drawing.Size(1544, 344)
		Me.tpLinearMotion.TabIndex = 1
		Me.tpLinearMotion.Text = "X Linear Motion"
		Me.tpLinearMotion.UseVisualStyleBackColor = true
		'
		'dgvXPara
		'
		Me.dgvXPara.AllowUserToAddRows = false
		Me.dgvXPara.AllowUserToResizeRows = false
		Me.dgvXPara.BackgroundColor = System.Drawing.SystemColors.ControlLightLight
		Me.dgvXPara.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
		Me.dgvXPara.Location = New System.Drawing.Point(0, 0)
		Me.dgvXPara.Name = "dgvXPara"
		Me.dgvXPara.RowTemplate.Height = 21
		Me.dgvXPara.Size = New System.Drawing.Size(1544, 344)
		Me.dgvXPara.TabIndex = 65
		'
		'tpPointMotion
		'
		Me.tpPointMotion.Controls.Add(Me.dgvPPara)
		Me.tpPointMotion.Font = New System.Drawing.Font("Meiryo UI", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.tpPointMotion.ForeColor = System.Drawing.SystemColors.ControlText
		Me.tpPointMotion.Location = New System.Drawing.Point(4, 33)
		Me.tpPointMotion.Name = "tpPointMotion"
		Me.tpPointMotion.Size = New System.Drawing.Size(1544, 344)
		Me.tpPointMotion.TabIndex = 2
		Me.tpPointMotion.Text = "Z linear Motion"
		Me.tpPointMotion.UseVisualStyleBackColor = true
		'
		'dgvPPara
		'
		Me.dgvPPara.AllowUserToAddRows = false
		Me.dgvPPara.AllowUserToResizeRows = false
		Me.dgvPPara.BackgroundColor = System.Drawing.SystemColors.ControlLightLight
		Me.dgvPPara.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
		Me.dgvPPara.Location = New System.Drawing.Point(0, 0)
		Me.dgvPPara.Name = "dgvPPara"
		Me.dgvPPara.RowTemplate.Height = 21
		Me.dgvPPara.Size = New System.Drawing.Size(1544, 344)
		Me.dgvPPara.TabIndex = 64
		'
		'tpCirclePhaseX
		'
		Me.tpCirclePhaseX.Controls.Add(Me.dgvCParaPhX)
		Me.tpCirclePhaseX.Location = New System.Drawing.Point(4, 33)
		Me.tpCirclePhaseX.Name = "tpCirclePhaseX"
		Me.tpCirclePhaseX.Padding = New System.Windows.Forms.Padding(3)
		Me.tpCirclePhaseX.Size = New System.Drawing.Size(1544, 344)
		Me.tpCirclePhaseX.TabIndex = 3
		Me.tpCirclePhaseX.Text = "Circle Phase X"
		Me.tpCirclePhaseX.UseVisualStyleBackColor = true
		'
		'dgvCParaPhX
		'
		Me.dgvCParaPhX.AllowUserToAddRows = false
		Me.dgvCParaPhX.AllowUserToResizeRows = false
		Me.dgvCParaPhX.BackgroundColor = System.Drawing.SystemColors.ControlLightLight
		Me.dgvCParaPhX.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
		Me.dgvCParaPhX.Location = New System.Drawing.Point(0, 0)
		Me.dgvCParaPhX.Name = "dgvCParaPhX"
		Me.dgvCParaPhX.RowTemplate.Height = 21
		Me.dgvCParaPhX.Size = New System.Drawing.Size(1544, 344)
		Me.dgvCParaPhX.TabIndex = 70
		'
		'tpCirclePhaseY
		'
		Me.tpCirclePhaseY.Controls.Add(Me.dgvCParaPhY)
		Me.tpCirclePhaseY.Location = New System.Drawing.Point(4, 33)
		Me.tpCirclePhaseY.Name = "tpCirclePhaseY"
		Me.tpCirclePhaseY.Size = New System.Drawing.Size(1544, 344)
		Me.tpCirclePhaseY.TabIndex = 4
		Me.tpCirclePhaseY.Text = "Circle PhaseY"
		Me.tpCirclePhaseY.UseVisualStyleBackColor = true
		'
		'dgvCParaPhY
		'
		Me.dgvCParaPhY.AllowUserToAddRows = false
		Me.dgvCParaPhY.AllowUserToResizeRows = false
		Me.dgvCParaPhY.BackgroundColor = System.Drawing.SystemColors.ControlLightLight
		Me.dgvCParaPhY.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
		Me.dgvCParaPhY.Location = New System.Drawing.Point(0, 0)
		Me.dgvCParaPhY.Name = "dgvCParaPhY"
		Me.dgvCParaPhY.RowTemplate.Height = 21
		Me.dgvCParaPhY.Size = New System.Drawing.Size(1544, 344)
		Me.dgvCParaPhY.TabIndex = 71
		'
		'tpLinearPhase
		'
		Me.tpLinearPhase.Controls.Add(Me.dgvXParaPh)
		Me.tpLinearPhase.Location = New System.Drawing.Point(4, 33)
		Me.tpLinearPhase.Name = "tpLinearPhase"
		Me.tpLinearPhase.Size = New System.Drawing.Size(1544, 344)
		Me.tpLinearPhase.TabIndex = 5
		Me.tpLinearPhase.Text = "X linear Phase"
		Me.tpLinearPhase.UseVisualStyleBackColor = true
		'
		'dgvXParaPh
		'
		Me.dgvXParaPh.AllowUserToAddRows = false
		Me.dgvXParaPh.AllowUserToResizeRows = false
		Me.dgvXParaPh.BackgroundColor = System.Drawing.SystemColors.ControlLightLight
		Me.dgvXParaPh.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
		Me.dgvXParaPh.Location = New System.Drawing.Point(0, 0)
		Me.dgvXParaPh.Name = "dgvXParaPh"
		Me.dgvXParaPh.RowTemplate.Height = 21
		Me.dgvXParaPh.Size = New System.Drawing.Size(1544, 344)
		Me.dgvXParaPh.TabIndex = 70
		'
		'tpLinearPhaseZ
		'
		Me.tpLinearPhaseZ.Controls.Add(Me.dgvPParaPh)
		Me.tpLinearPhaseZ.Location = New System.Drawing.Point(4, 33)
		Me.tpLinearPhaseZ.Name = "tpLinearPhaseZ"
		Me.tpLinearPhaseZ.Size = New System.Drawing.Size(1544, 344)
		Me.tpLinearPhaseZ.TabIndex = 6
		Me.tpLinearPhaseZ.Text = "Z Linear Phase"
		Me.tpLinearPhaseZ.UseVisualStyleBackColor = true
		'
		'dgvPParaPh
		'
		Me.dgvPParaPh.AllowUserToAddRows = false
		Me.dgvPParaPh.AllowUserToResizeRows = false
		Me.dgvPParaPh.BackgroundColor = System.Drawing.SystemColors.ControlLightLight
		Me.dgvPParaPh.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
		Me.dgvPParaPh.Location = New System.Drawing.Point(0, 0)
		Me.dgvPParaPh.Name = "dgvPParaPh"
		Me.dgvPParaPh.RowTemplate.Height = 21
		Me.dgvPParaPh.Size = New System.Drawing.Size(1544, 344)
		Me.dgvPParaPh.TabIndex = 70
		'
		'btnCAllSelect
		'
		Me.btnCAllSelect.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left),System.Windows.Forms.AnchorStyles)
		Me.btnCAllSelect.BackColor = System.Drawing.SystemColors.ButtonFace
		Me.btnCAllSelect.Font = New System.Drawing.Font("Meiryo UI", 12!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.btnCAllSelect.Location = New System.Drawing.Point(133, 387)
		Me.btnCAllSelect.Name = "btnCAllSelect"
		Me.btnCAllSelect.Size = New System.Drawing.Size(117, 40)
		Me.btnCAllSelect.TabIndex = 63
		Me.btnCAllSelect.Text = "All Select"
		Me.btnCAllSelect.UseVisualStyleBackColor = false
		'
		'lblAutoPeeling
		'
		Me.lblAutoPeeling.AutoSize = true
		Me.lblAutoPeeling.BackColor = System.Drawing.Color.FromArgb(CType(CType(192,Byte),Integer), CType(CType(192,Byte),Integer), CType(CType(255,Byte),Integer))
		Me.lblAutoPeeling.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		Me.lblAutoPeeling.Font = New System.Drawing.Font("Meiryo UI", 27.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.lblAutoPeeling.ForeColor = System.Drawing.Color.Blue
		Me.lblAutoPeeling.Location = New System.Drawing.Point(591, 463)
		Me.lblAutoPeeling.Name = "lblAutoPeeling"
		Me.lblAutoPeeling.Size = New System.Drawing.Size(646, 49)
		Me.lblAutoPeeling.TabIndex = 50
		Me.lblAutoPeeling.Text = "Setting stripping temperature..."
		Me.lblAutoPeeling.TextAlign = System.Drawing.ContentAlignment.TopCenter
		Me.lblAutoPeeling.Visible = false
		'
		'btnCAllClear
		'
		Me.btnCAllClear.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left),System.Windows.Forms.AnchorStyles)
		Me.btnCAllClear.BackColor = System.Drawing.SystemColors.ButtonFace
		Me.btnCAllClear.Font = New System.Drawing.Font("Meiryo UI", 12!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.btnCAllClear.Location = New System.Drawing.Point(419, 387)
		Me.btnCAllClear.Name = "btnCAllClear"
		Me.btnCAllClear.Size = New System.Drawing.Size(117, 40)
		Me.btnCAllClear.TabIndex = 6
		Me.btnCAllClear.Text = "ALL Clear"
		Me.btnCAllClear.UseVisualStyleBackColor = false
		'
		'ChartStress
		'
		Me.ChartStress.BorderlineColor = System.Drawing.Color.DarkGray
		Me.ChartStress.BorderlineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Solid
		Me.ChartStress.BorderlineWidth = 2
		Me.ChartStress.Location = New System.Drawing.Point(7, 713)
		Me.ChartStress.Name = "ChartStress"
		Me.ChartStress.Size = New System.Drawing.Size(1545, 280)
		Me.ChartStress.TabIndex = 61
		Me.ChartStress.Text = "Chart2"
		'
		'ChartHosei
		'
		Me.ChartHosei.BorderlineColor = System.Drawing.Color.DarkGray
		Me.ChartHosei.BorderlineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Solid
		Me.ChartHosei.BorderlineWidth = 2
		Me.ChartHosei.Location = New System.Drawing.Point(7, 433)
		Me.ChartHosei.Name = "ChartHosei"
		Me.ChartHosei.Size = New System.Drawing.Size(1545, 280)
		Me.ChartHosei.TabIndex = 62
		Me.ChartHosei.Text = "Chart2"
		'
		'btnZeroAdj
		'
		Me.btnZeroAdj.BackColor = System.Drawing.SystemColors.ButtonFace
		Me.btnZeroAdj.Font = New System.Drawing.Font("Meiryo UI", 18!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.btnZeroAdj.Image = Global.SZKTool.My.Resources.Resources.zerobtn
		Me.btnZeroAdj.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me.btnZeroAdj.Location = New System.Drawing.Point(8, 59)
		Me.btnZeroAdj.Name = "btnZeroAdj"
		Me.btnZeroAdj.Size = New System.Drawing.Size(296, 48)
		Me.btnZeroAdj.TabIndex = 4
		Me.btnZeroAdj.Text = "Zero Adjustment"
		Me.btnZeroAdj.UseVisualStyleBackColor = false
		'
		'btnExec
		'
		Me.btnExec.BackColor = System.Drawing.SystemColors.ButtonFace
		Me.btnExec.Font = New System.Drawing.Font("Meiryo UI", 18!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.btnExec.Image = Global.SZKTool.My.Resources.Resources.PlayHS
		Me.btnExec.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me.btnExec.Location = New System.Drawing.Point(10, 109)
		Me.btnExec.Name = "btnExec"
		Me.btnExec.Size = New System.Drawing.Size(296, 48)
		Me.btnExec.TabIndex = 43
		Me.btnExec.Text = "Exec"
		Me.btnExec.UseVisualStyleBackColor = false
		'
		'btnStop
		'
		Me.btnStop.BackColor = System.Drawing.SystemColors.ButtonFace
		Me.btnStop.Font = New System.Drawing.Font("Meiryo UI", 18!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.btnStop.Image = Global.SZKTool.My.Resources.Resources.stopbtn
		Me.btnStop.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me.btnStop.Location = New System.Drawing.Point(8, 159)
		Me.btnStop.Name = "btnStop"
		Me.btnStop.Size = New System.Drawing.Size(296, 48)
		Me.btnStop.TabIndex = 44
		Me.btnStop.Text = "Stop"
		Me.btnStop.UseVisualStyleBackColor = false
		'
		'nudSampleTime
		'
		Me.nudSampleTime.Font = New System.Drawing.Font("MS UI Gothic", 18!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.nudSampleTime.Location = New System.Drawing.Point(222, 3)
		Me.nudSampleTime.Maximum = New Decimal(New Integer() {99999, 0, 0, 0})
		Me.nudSampleTime.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
		Me.nudSampleTime.Name = "nudSampleTime"
		Me.nudSampleTime.Size = New System.Drawing.Size(80, 31)
		Me.nudSampleTime.TabIndex = 45
		Me.nudSampleTime.Value = New Decimal(New Integer() {10, 0, 0, 0})
		'
		'lblSapleTime
		'
		Me.lblSapleTime.AutoSize = true
		Me.lblSapleTime.Font = New System.Drawing.Font("Meiryo UI", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.lblSapleTime.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
		Me.lblSapleTime.Location = New System.Drawing.Point(6, 10)
		Me.lblSapleTime.Name = "lblSapleTime"
		Me.lblSapleTime.Size = New System.Drawing.Size(151, 20)
		Me.lblSapleTime.TabIndex = 46
		Me.lblSapleTime.Text = "Sample time (Sec)"
		'
		'nudAcquisitionTime
		'
		Me.nudAcquisitionTime.DecimalPlaces = 1
		Me.nudAcquisitionTime.Font = New System.Drawing.Font("MS UI Gothic", 18!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.nudAcquisitionTime.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
		Me.nudAcquisitionTime.Location = New System.Drawing.Point(222, 36)
		Me.nudAcquisitionTime.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
		Me.nudAcquisitionTime.Name = "nudAcquisitionTime"
		Me.nudAcquisitionTime.Size = New System.Drawing.Size(80, 31)
		Me.nudAcquisitionTime.TabIndex = 45
		Me.nudAcquisitionTime.Value = New Decimal(New Integer() {50, 0, 0, 0})
		'
		'lblAcquisitionTime
		'
		Me.lblAcquisitionTime.AutoSize = true
		Me.lblAcquisitionTime.Font = New System.Drawing.Font("Meiryo UI", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.lblAcquisitionTime.Location = New System.Drawing.Point(6, 43)
		Me.lblAcquisitionTime.Name = "lblAcquisitionTime"
		Me.lblAcquisitionTime.Size = New System.Drawing.Size(179, 20)
		Me.lblAcquisitionTime.TabIndex = 46
		Me.lblAcquisitionTime.Text = "Acquisition time ( % )"
		'
		'GroupBox2
		'
		Me.GroupBox2.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
		Me.GroupBox2.BackColor = System.Drawing.SystemColors.ControlLight
		Me.GroupBox2.Controls.Add(Me.btnSOCheck)
		Me.GroupBox2.Controls.Add(Me.btnTemperature)
		Me.GroupBox2.Controls.Add(Me.btnOrigin)
		Me.GroupBox2.Controls.Add(Me.btnZeroAdj)
		Me.GroupBox2.Controls.Add(Me.btnExec)
		Me.GroupBox2.Controls.Add(Me.btnStop)
		Me.GroupBox2.Location = New System.Drawing.Point(1580, 11)
		Me.GroupBox2.Name = "GroupBox2"
		Me.GroupBox2.Size = New System.Drawing.Size(312, 314)
		Me.GroupBox2.TabIndex = 47
		Me.GroupBox2.TabStop = false
		'
		'btnSOCheck
		'
		Me.btnSOCheck.BackColor = System.Drawing.SystemColors.ButtonFace
		Me.btnSOCheck.Font = New System.Drawing.Font("Meiryo UI", 18!)
		Me.btnSOCheck.Location = New System.Drawing.Point(9, 259)
		Me.btnSOCheck.Name = "btnSOCheck"
		Me.btnSOCheck.Size = New System.Drawing.Size(296, 48)
		Me.btnSOCheck.TabIndex = 42
		Me.btnSOCheck.Text = "Step-out Check"
		Me.btnSOCheck.UseVisualStyleBackColor = false
		'
		'btnTemperature
		'
		Me.btnTemperature.BackColor = System.Drawing.SystemColors.ButtonFace
		Me.btnTemperature.Font = New System.Drawing.Font("Meiryo UI", 18!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.btnTemperature.Image = Global.SZKTool.My.Resources.Resources.tcbtn
		Me.btnTemperature.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me.btnTemperature.Location = New System.Drawing.Point(8, 9)
		Me.btnTemperature.Name = "btnTemperature"
		Me.btnTemperature.Size = New System.Drawing.Size(296, 48)
		Me.btnTemperature.TabIndex = 4
		Me.btnTemperature.Text = "Temperature control"
		Me.btnTemperature.UseVisualStyleBackColor = false
		'
		'btnOrigin
		'
		Me.btnOrigin.BackColor = System.Drawing.SystemColors.ButtonFace
		Me.btnOrigin.Font = New System.Drawing.Font("Meiryo UI", 18!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.btnOrigin.Location = New System.Drawing.Point(9, 209)
		Me.btnOrigin.Name = "btnOrigin"
		Me.btnOrigin.Size = New System.Drawing.Size(296, 48)
		Me.btnOrigin.TabIndex = 50
		Me.btnOrigin.Text = "Return Origin"
		Me.btnOrigin.UseVisualStyleBackColor = false
		'
		'GroupBox3
		'
		Me.GroupBox3.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
		Me.GroupBox3.BackColor = System.Drawing.SystemColors.ControlLight
		Me.GroupBox3.Controls.Add(Me.lblSapleTime)
		Me.GroupBox3.Controls.Add(Me.nudSampleTime)
		Me.GroupBox3.Controls.Add(Me.nudAcquisitionTime)
		Me.GroupBox3.Controls.Add(Me.lblAcquisitionTime)
		Me.GroupBox3.Location = New System.Drawing.Point(1580, 291)
		Me.GroupBox3.Name = "GroupBox3"
		Me.GroupBox3.Size = New System.Drawing.Size(312, 72)
		Me.GroupBox3.TabIndex = 48
		Me.GroupBox3.TabStop = false
		Me.GroupBox3.Visible = false
		'
		'nudPreTemp
		'
		Me.nudPreTemp.DecimalPlaces = 1
		Me.nudPreTemp.Font = New System.Drawing.Font("MS UI Gothic", 18!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.nudPreTemp.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
		Me.nudPreTemp.Location = New System.Drawing.Point(222, 2)
		Me.nudPreTemp.Maximum = New Decimal(New Integer() {240, 0, 0, 0})
		Me.nudPreTemp.Name = "nudPreTemp"
		Me.nudPreTemp.Size = New System.Drawing.Size(80, 31)
		Me.nudPreTemp.TabIndex = 45
		Me.nudPreTemp.Value = New Decimal(New Integer() {40, 0, 0, 0})
		'
		'nudPeelingTemp
		'
		Me.nudPeelingTemp.DecimalPlaces = 1
		Me.nudPeelingTemp.Font = New System.Drawing.Font("MS UI Gothic", 18!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.nudPeelingTemp.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
		Me.nudPeelingTemp.Location = New System.Drawing.Point(222, 34)
		Me.nudPeelingTemp.Maximum = New Decimal(New Integer() {240, 0, 0, 0})
		Me.nudPeelingTemp.Name = "nudPeelingTemp"
		Me.nudPeelingTemp.Size = New System.Drawing.Size(80, 31)
		Me.nudPeelingTemp.TabIndex = 45
		Me.nudPeelingTemp.Value = New Decimal(New Integer() {220, 0, 0, 0})
		'
		'lblPresetTemp
		'
		Me.lblPresetTemp.AutoSize = true
		Me.lblPresetTemp.Font = New System.Drawing.Font("Meiryo UI", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.lblPresetTemp.Location = New System.Drawing.Point(6, 9)
		Me.lblPresetTemp.Name = "lblPresetTemp"
		Me.lblPresetTemp.Size = New System.Drawing.Size(170, 20)
		Me.lblPresetTemp.TabIndex = 46
		Me.lblPresetTemp.Text = "Preset Temp.(deg C)"
		'
		'lblPeelingTemp
		'
		Me.lblPeelingTemp.AutoSize = true
		Me.lblPeelingTemp.Font = New System.Drawing.Font("Meiryo UI", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.lblPeelingTemp.Location = New System.Drawing.Point(6, 41)
		Me.lblPeelingTemp.Name = "lblPeelingTemp"
		Me.lblPeelingTemp.Size = New System.Drawing.Size(177, 20)
		Me.lblPeelingTemp.TabIndex = 46
		Me.lblPeelingTemp.Text = "Peeling Temp.(deg C)"
		'
		'dgvCurrVal
		'
		Me.dgvCurrVal.AllowUserToAddRows = false
		Me.dgvCurrVal.AllowUserToDeleteRows = false
		Me.dgvCurrVal.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
		DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
		DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control
		DataGridViewCellStyle1.Font = New System.Drawing.Font("MS UI Gothic", 9!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText
		DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
		DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
		DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
		Me.dgvCurrVal.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
		Me.dgvCurrVal.ColumnHeadersHeight = 29
		Me.dgvCurrVal.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
		Me.dgvCurrVal.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Column1, Me.Column2})
		DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
		DataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window
		DataGridViewCellStyle4.Font = New System.Drawing.Font("Meiryo UI", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		DataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlText
		DataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.Transparent
		DataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.ControlText
		DataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
		Me.dgvCurrVal.DefaultCellStyle = DataGridViewCellStyle4
		Me.dgvCurrVal.GridColor = System.Drawing.SystemColors.ButtonHighlight
		Me.dgvCurrVal.Location = New System.Drawing.Point(1580, 661)
		Me.dgvCurrVal.MultiSelect = false
		Me.dgvCurrVal.Name = "dgvCurrVal"
		Me.dgvCurrVal.ReadOnly = true
		DataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
		DataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Control
		DataGridViewCellStyle5.Font = New System.Drawing.Font("MS UI Gothic", 9!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		DataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.WindowText
		DataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight
		DataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText
		DataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
		Me.dgvCurrVal.RowHeadersDefaultCellStyle = DataGridViewCellStyle5
		Me.dgvCurrVal.RowHeadersWidth = 51
		Me.dgvCurrVal.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
		Me.dgvCurrVal.RowTemplate.Height = 28
		Me.dgvCurrVal.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.dgvCurrVal.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect
		Me.dgvCurrVal.Size = New System.Drawing.Size(312, 293)
		Me.dgvCurrVal.TabIndex = 49
		Me.dgvCurrVal.TabStop = false
		'
		'Column1
		'
		Me.Column1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
		DataGridViewCellStyle2.Font = New System.Drawing.Font("MS UI Gothic", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		DataGridViewCellStyle2.ForeColor = System.Drawing.Color.Black
		DataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.White
		DataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.Black
		Me.Column1.DefaultCellStyle = DataGridViewCellStyle2
		Me.Column1.FillWeight = 98.47717!
		Me.Column1.HeaderText = "Item"
		Me.Column1.MinimumWidth = 6
		Me.Column1.Name = "Column1"
		Me.Column1.ReadOnly = true
		Me.Column1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
		'
		'Column2
		'
		Me.Column2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
		DataGridViewCellStyle3.Font = New System.Drawing.Font("MS UI Gothic", 18!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		DataGridViewCellStyle3.ForeColor = System.Drawing.Color.Black
		DataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.White
		DataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.Black
		Me.Column2.DefaultCellStyle = DataGridViewCellStyle3
		Me.Column2.FillWeight = 101.5228!
		Me.Column2.HeaderText = "Value"
		Me.Column2.MinimumWidth = 6
		Me.Column2.Name = "Column2"
		Me.Column2.ReadOnly = true
		'
		'GroupBox4
		'
		Me.GroupBox4.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
		Me.GroupBox4.BackColor = System.Drawing.SystemColors.ControlLight
		Me.GroupBox4.Controls.Add(Me.nudPreTemp)
		Me.GroupBox4.Controls.Add(Me.lblInputGap)
		Me.GroupBox4.Controls.Add(Me.nudInputGap)
		Me.GroupBox4.Controls.Add(Me.nudXYWait)
		Me.GroupBox4.Controls.Add(Me.nudPeelingTemp)
		Me.GroupBox4.Controls.Add(Me.nudRisingHPWait)
		Me.GroupBox4.Controls.Add(Me.lblPresetTemp)
		Me.GroupBox4.Controls.Add(Me.lblHPUpWait)
		Me.GroupBox4.Controls.Add(Me.lblPeelingTemp)
		Me.GroupBox4.Controls.Add(Me.lblXYWait)
		Me.GroupBox4.Controls.Add(Me.nudMeasureGap)
		Me.GroupBox4.Controls.Add(Me.lblMeasureGap)
		Me.GroupBox4.Location = New System.Drawing.Point(1580, 334)
		Me.GroupBox4.Name = "GroupBox4"
		Me.GroupBox4.Size = New System.Drawing.Size(312, 203)
		Me.GroupBox4.TabIndex = 50
		Me.GroupBox4.TabStop = false
		'
		'lblInputGap
		'
		Me.lblInputGap.AutoSize = true
		Me.lblInputGap.Font = New System.Drawing.Font("Meiryo UI", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.lblInputGap.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
		Me.lblInputGap.Location = New System.Drawing.Point(5, 74)
		Me.lblInputGap.Name = "lblInputGap"
		Me.lblInputGap.Size = New System.Drawing.Size(136, 20)
		Me.lblInputGap.TabIndex = 46
		Me.lblInputGap.Text = "Input Gap (mm)"
		'
		'nudInputGap
		'
		Me.nudInputGap.DecimalPlaces = 1
		Me.nudInputGap.Font = New System.Drawing.Font("MS UI Gothic", 18!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.nudInputGap.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
		Me.nudInputGap.Location = New System.Drawing.Point(222, 67)
		Me.nudInputGap.Maximum = New Decimal(New Integer() {50, 0, 0, 0})
		Me.nudInputGap.Name = "nudInputGap"
		Me.nudInputGap.Size = New System.Drawing.Size(80, 31)
		Me.nudInputGap.TabIndex = 45
		Me.nudInputGap.Value = New Decimal(New Integer() {20, 0, 0, 0})
		'
		'nudXYWait
		'
		Me.nudXYWait.DecimalPlaces = 1
		Me.nudXYWait.Font = New System.Drawing.Font("MS UI Gothic", 18!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.nudXYWait.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
		Me.nudXYWait.Location = New System.Drawing.Point(222, 132)
		Me.nudXYWait.Maximum = New Decimal(New Integer() {500, 0, 0, 0})
		Me.nudXYWait.Name = "nudXYWait"
		Me.nudXYWait.Size = New System.Drawing.Size(80, 31)
		Me.nudXYWait.TabIndex = 45
		Me.nudXYWait.Value = New Decimal(New Integer() {200, 0, 0, 0})
		'
		'nudRisingHPWait
		'
		Me.nudRisingHPWait.DecimalPlaces = 1
		Me.nudRisingHPWait.Font = New System.Drawing.Font("MS UI Gothic", 18!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.nudRisingHPWait.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
		Me.nudRisingHPWait.Location = New System.Drawing.Point(222, 166)
		Me.nudRisingHPWait.Maximum = New Decimal(New Integer() {300, 0, 0, 0})
		Me.nudRisingHPWait.Name = "nudRisingHPWait"
		Me.nudRisingHPWait.Size = New System.Drawing.Size(80, 31)
		Me.nudRisingHPWait.TabIndex = 45
		Me.nudRisingHPWait.Value = New Decimal(New Integer() {8, 0, 0, 0})
		'
		'lblHPUpWait
		'
		Me.lblHPUpWait.AutoSize = true
		Me.lblHPUpWait.Font = New System.Drawing.Font("Meiryo UI", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.lblHPUpWait.Location = New System.Drawing.Point(7, 173)
		Me.lblHPUpWait.Name = "lblHPUpWait"
		Me.lblHPUpWait.Size = New System.Drawing.Size(191, 20)
		Me.lblHPUpWait.TabIndex = 46
		Me.lblHPUpWait.Text = "Wait for rising HP (Sec)"
		'
		'lblXYWait
		'
		Me.lblXYWait.AutoSize = true
		Me.lblXYWait.Font = New System.Drawing.Font("Meiryo UI", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.lblXYWait.Location = New System.Drawing.Point(6, 139)
		Me.lblXYWait.Name = "lblXYWait"
		Me.lblXYWait.Size = New System.Drawing.Size(196, 20)
		Me.lblXYWait.TabIndex = 46
		Me.lblXYWait.Text = "Wait for XY Action (Sec)"
		'
		'nudMeasureGap
		'
		Me.nudMeasureGap.DecimalPlaces = 3
		Me.nudMeasureGap.Font = New System.Drawing.Font("MS UI Gothic", 18!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.nudMeasureGap.Increment = New Decimal(New Integer() {1, 0, 0, 196608})
		Me.nudMeasureGap.Location = New System.Drawing.Point(222, 99)
		Me.nudMeasureGap.Maximum = New Decimal(New Integer() {10, 0, 0, 0})
		Me.nudMeasureGap.Minimum = New Decimal(New Integer() {1, 0, 0, 196608})
		Me.nudMeasureGap.Name = "nudMeasureGap"
		Me.nudMeasureGap.Size = New System.Drawing.Size(80, 31)
		Me.nudMeasureGap.TabIndex = 45
		Me.nudMeasureGap.Value = New Decimal(New Integer() {5, 0, 0, 196608})
		'
		'lblMeasureGap
		'
		Me.lblMeasureGap.AutoSize = true
		Me.lblMeasureGap.Font = New System.Drawing.Font("Meiryo UI", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.lblMeasureGap.Location = New System.Drawing.Point(5, 106)
		Me.lblMeasureGap.Name = "lblMeasureGap"
		Me.lblMeasureGap.Size = New System.Drawing.Size(199, 20)
		Me.lblMeasureGap.TabIndex = 46
		Me.lblMeasureGap.Text = "Measurement Gap (mm)"
		'
		'frmCalibFull
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6!, 12!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.ClientSize = New System.Drawing.Size(1904, 1041)
		Me.Controls.Add(Me.GroupBox2)
		Me.Controls.Add(Me.GroupBox4)
		Me.Controls.Add(Me.dgvCurrVal)
		Me.Controls.Add(Me.GroupBox3)
		Me.Controls.Add(Me.StatusStrip1)
		Me.Controls.Add(Me.gbxMotionSetting)
		Me.Controls.Add(Me.gbxSetting)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
		Me.KeyPreview = true
		Me.MaximizeBox = false
		Me.MinimizeBox = false
		Me.Name = "frmCalibFull"
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
		Me.Text = "Calibration(Percent)"
		Me.StatusStrip1.ResumeLayout(false)
		Me.StatusStrip1.PerformLayout
		CType(Me.dgvCPara,System.ComponentModel.ISupportInitialize).EndInit
		Me.gbxSetting.ResumeLayout(false)
		Me.gbxMotionSetting.ResumeLayout(false)
		Me.gbxMotionSetting.PerformLayout
		Me.Panel2.ResumeLayout(false)
		CType(Me.Chart2,System.ComponentModel.ISupportInitialize).EndInit
		Me.tcPara.ResumeLayout(false)
		Me.tpCircleMotion.ResumeLayout(false)
		Me.tpLinearMotion.ResumeLayout(false)
		CType(Me.dgvXPara,System.ComponentModel.ISupportInitialize).EndInit
		Me.tpPointMotion.ResumeLayout(false)
		CType(Me.dgvPPara,System.ComponentModel.ISupportInitialize).EndInit
		Me.tpCirclePhaseX.ResumeLayout(false)
		CType(Me.dgvCParaPhX,System.ComponentModel.ISupportInitialize).EndInit
		Me.tpCirclePhaseY.ResumeLayout(false)
		CType(Me.dgvCParaPhY,System.ComponentModel.ISupportInitialize).EndInit
		Me.tpLinearPhase.ResumeLayout(false)
		CType(Me.dgvXParaPh,System.ComponentModel.ISupportInitialize).EndInit
		Me.tpLinearPhaseZ.ResumeLayout(false)
		CType(Me.dgvPParaPh,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.ChartStress,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.ChartHosei,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.nudSampleTime,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.nudAcquisitionTime,System.ComponentModel.ISupportInitialize).EndInit
		Me.GroupBox2.ResumeLayout(false)
		Me.GroupBox3.ResumeLayout(false)
		Me.GroupBox3.PerformLayout
		CType(Me.nudPreTemp,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.nudPeelingTemp,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.dgvCurrVal,System.ComponentModel.ISupportInitialize).EndInit
		Me.GroupBox4.ResumeLayout(false)
		Me.GroupBox4.PerformLayout
		CType(Me.nudInputGap,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.nudXYWait,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.nudRisingHPWait,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.nudMeasureGap,System.ComponentModel.ISupportInitialize).EndInit
		Me.ResumeLayout(false)
		Me.PerformLayout

End Sub
	Friend WithEvents lblMsg As ToolStripStatusLabel
	Friend WithEvents StatusStrip1 As StatusStrip
	Friend WithEvents btnXStop As Button
	Friend WithEvents btnXExec As Button
	Friend WithEvents timMain As Timer
	Friend WithEvents dgvCPara As DataGridView
	Friend WithEvents btnXSel As Button
	Friend WithEvents btnCSel As Button
	Friend WithEvents gbxSetting As GroupBox
	Friend WithEvents btnXClear As Button
	Friend WithEvents btnCClear As Button
	Friend WithEvents btnCStop As Button
	Friend WithEvents btnCExec As Button
	Friend WithEvents btnXAllClear As Button
	Friend WithEvents gbxMotionSetting As GroupBox
	Friend WithEvents btnCAllClear As Button
	Friend WithEvents btnZeroAdj As Button
	Friend WithEvents btnExec As Button
	Friend WithEvents btnStop As Button
	Friend WithEvents nudSampleTime As NumericUpDown
	Friend WithEvents lblSapleTime As Label
	Friend WithEvents nudAcquisitionTime As NumericUpDown
	Friend WithEvents lblAcquisitionTime As Label
	Friend WithEvents GroupBox2 As GroupBox
	Friend WithEvents GroupBox3 As GroupBox
	Friend WithEvents dgvCurrVal As DataGridView
	Friend WithEvents lblAutoPeeling As Label
	Friend WithEvents nudPeelingTemp As NumericUpDown
	Friend WithEvents lblPeelingTemp As Label
	Friend WithEvents btnTemperature As Button
	Friend WithEvents nudPreTemp As NumericUpDown
	Friend WithEvents lblPresetTemp As Label
	Friend WithEvents Column1 As DataGridViewTextBoxColumn
	Friend WithEvents Column2 As DataGridViewTextBoxColumn
	Friend WithEvents GroupBox4 As GroupBox
	Friend WithEvents lblInputGap As Label
	Friend WithEvents nudInputGap As NumericUpDown
	Friend WithEvents nudXYWait As NumericUpDown
	Friend WithEvents nudRisingHPWait As NumericUpDown
	Friend WithEvents lblXYWait As Label
	Friend WithEvents nudMeasureGap As NumericUpDown
	Friend WithEvents lblHPUpWait As Label
	Friend WithEvents lblMeasureGap As Label
	Friend WithEvents btnSOCheck As Button
	Friend WithEvents btnOrigin As Button
	Friend WithEvents Panel2 As Panel
	Friend WithEvents Chart2 As DataVisualization.Charting.Chart
	Friend WithEvents ChartStress As DataVisualization.Charting.Chart
	Friend WithEvents ChartHosei As DataVisualization.Charting.Chart
	Friend WithEvents btnCAllSelect As Button
	Friend WithEvents tcPara As TabControl
	Friend WithEvents tpCircleMotion As TabPage
	Friend WithEvents tpLinearMotion As TabPage
	Friend WithEvents dgvXPara As DataGridView
	Friend WithEvents tpPointMotion As TabPage
	Friend WithEvents dgvPPara As DataGridView
	Friend WithEvents Label1 As Label
	Friend WithEvents lblMeasDateVal As Label
	Friend WithEvents lblMeasDateLabel As Label
	Friend WithEvents tpCirclePhaseX As TabPage
	Friend WithEvents tpCirclePhaseY As TabPage
	Friend WithEvents tpLinearPhase As TabPage
	Friend WithEvents dgvCParaPhX As DataGridView
	Friend WithEvents dgvCParaPhY As DataGridView
	Friend WithEvents dgvXParaPh As DataGridView
	Friend WithEvents tpLinearPhaseZ As TabPage
	Friend WithEvents dgvPParaPh As DataGridView
End Class
