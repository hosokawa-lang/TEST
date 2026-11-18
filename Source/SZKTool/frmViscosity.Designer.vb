<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmViscosity
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
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmViscosity))
		Me.dgvCurrVal = New System.Windows.Forms.DataGridView()
		Me.Column1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
		Me.Column2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
		Me.GroupBox2 = New System.Windows.Forms.GroupBox()
		Me.btnClose = New System.Windows.Forms.Button()
		Me.btnTemperature = New System.Windows.Forms.Button()
		Me.btnOrigin = New System.Windows.Forms.Button()
		Me.btnZeroAdj = New System.Windows.Forms.Button()
		Me.btnExec = New System.Windows.Forms.Button()
		Me.btnStop = New System.Windows.Forms.Button()
		Me.GroupBox4 = New System.Windows.Forms.GroupBox()
		Me.rbXAction = New System.Windows.Forms.RadioButton()
		Me.rbCircleAction = New System.Windows.Forms.RadioButton()
		Me.lblProbedDiameterLabel = New System.Windows.Forms.Label()
		Me.nudPreTemp = New System.Windows.Forms.NumericUpDown()
		Me.lblInputGap = New System.Windows.Forms.Label()
		Me.nudInputGap = New System.Windows.Forms.NumericUpDown()
		Me.nudXYWait = New System.Windows.Forms.NumericUpDown()
		Me.nudProbedDiameter = New System.Windows.Forms.NumericUpDown()
		Me.nudRisingHPWait = New System.Windows.Forms.NumericUpDown()
		Me.lblPresetTemp = New System.Windows.Forms.Label()
		Me.lblHPUpWait = New System.Windows.Forms.Label()
		Me.lblXYWait = New System.Windows.Forms.Label()
		Me.nudMeasureGap = New System.Windows.Forms.NumericUpDown()
		Me.lblMeasureGap = New System.Windows.Forms.Label()
		Me.GroupBox3 = New System.Windows.Forms.GroupBox()
		Me.lblSpeedDepSampleTime = New System.Windows.Forms.Label()
		Me.nudSpeedDepSampleTime = New System.Windows.Forms.NumericUpDown()
		Me.nudSpeedDepAcquisitionTime = New System.Windows.Forms.NumericUpDown()
		Me.lblSpeedDepAcquisitionTime = New System.Windows.Forms.Label()
		Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
		Me.lblMsg = New System.Windows.Forms.ToolStripStatusLabel()
		Me.gbxSpeedDep = New System.Windows.Forms.GroupBox()
		Me.ckbFlcvReverse = New System.Windows.Forms.CheckBox()
		Me.lblSpeedDepStepWidthLabel = New System.Windows.Forms.Label()
		Me.nudSpeedDepAmpStep = New System.Windows.Forms.NumericUpDown()
		Me.nudSpeedDepAmpEnd = New System.Windows.Forms.NumericUpDown()
		Me.nudSpeedDepAmpStart = New System.Windows.Forms.NumericUpDown()
		Me.nudSpeedDepPeriod = New System.Windows.Forms.NumericUpDown()
		Me.lblSpeedDepAmpEndLabel = New System.Windows.Forms.Label()
		Me.lblSpeedDepAmpStartLabel = New System.Windows.Forms.Label()
		Me.lblSpeedDepPeriodLabel = New System.Windows.Forms.Label()
		Me.gbxHighShear = New System.Windows.Forms.GroupBox()
		Me.lblHighAmpLabel = New System.Windows.Forms.Label()
		Me.lblHighTimeLabel = New System.Windows.Forms.Label()
		Me.nudHighAmp = New System.Windows.Forms.NumericUpDown()
		Me.nudHighTime = New System.Windows.Forms.NumericUpDown()
		Me.gbxLowShear2 = New System.Windows.Forms.GroupBox()
		Me.lblLow2AmpLabel = New System.Windows.Forms.Label()
		Me.lblLow2TimeLabel = New System.Windows.Forms.Label()
		Me.nudLow2Amp = New System.Windows.Forms.NumericUpDown()
		Me.nudLow2Time = New System.Windows.Forms.NumericUpDown()
		Me.gbxLowShear1 = New System.Windows.Forms.GroupBox()
		Me.lblLow1AmpLabel = New System.Windows.Forms.Label()
		Me.lblLow1TimeLabel = New System.Windows.Forms.Label()
		Me.nudLow1Amp = New System.Windows.Forms.NumericUpDown()
		Me.nudLow1Time = New System.Windows.Forms.NumericUpDown()
		Me.lblTimeDepPeriodLabel = New System.Windows.Forms.Label()
		Me.nudTimeDepPeriod = New System.Windows.Forms.NumericUpDown()
		Me.timMain = New System.Windows.Forms.Timer(Me.components)
		Me.GroupBox5 = New System.Windows.Forms.GroupBox()
		Me.lblTempDepSampleTimeLabel = New System.Windows.Forms.Label()
		Me.nudTempDepSampleTime = New System.Windows.Forms.NumericUpDown()
		Me.nudTempDepAcquisitionTime = New System.Windows.Forms.NumericUpDown()
		Me.lblTempDepAcquisitionTime = New System.Windows.Forms.Label()
		Me.GroupBox1 = New System.Windows.Forms.GroupBox()
		Me.nudTempDepAmp = New System.Windows.Forms.NumericUpDown()
		Me.lblTempDepAmpLabel = New System.Windows.Forms.Label()
		Me.lblTempDepStepWidthLabel = New System.Windows.Forms.Label()
		Me.nudTempDepTempStep = New System.Windows.Forms.NumericUpDown()
		Me.nudTempDepTempEnd = New System.Windows.Forms.NumericUpDown()
		Me.nudTempDepTempStart = New System.Windows.Forms.NumericUpDown()
		Me.nudTempDepPeriod = New System.Windows.Forms.NumericUpDown()
		Me.lblTempDepTempEndLabel = New System.Windows.Forms.Label()
		Me.lblTempDepTempStartLabel = New System.Windows.Forms.Label()
		Me.lblTempDepPeriodLabel = New System.Windows.Forms.Label()
		Me.ChartA = New System.Windows.Forms.DataVisualization.Charting.Chart()
		Me.ChartB = New System.Windows.Forms.DataVisualization.Charting.Chart()
		Me.Panel2 = New System.Windows.Forms.Panel()
		Me.Chart2 = New System.Windows.Forms.DataVisualization.Charting.Chart()
		Me.Panel1 = New System.Windows.Forms.Panel()
		Me.pctConStts = New System.Windows.Forms.PictureBox()
		Me.lblConnectState = New System.Windows.Forms.Label()
		Me.lblXSelect = New System.Windows.Forms.Label()
		Me.lblY2Select = New System.Windows.Forms.Label()
		Me.lblY1Select = New System.Windows.Forms.Label()
		Me.cbxXSelect = New System.Windows.Forms.ComboBox()
		Me.cbxY2Select = New System.Windows.Forms.ComboBox()
		Me.cbxY1Select = New System.Windows.Forms.ComboBox()
		Me.FlowLayoutPanel1 = New System.Windows.Forms.FlowLayoutPanel()
		Me.lblStrgMod = New System.Windows.Forms.Label()
		Me.lblStrgModVal = New System.Windows.Forms.Label()
		Me.lblElasticMod = New System.Windows.Forms.Label()
		Me.lblElasticModVal = New System.Windows.Forms.Label()
		Me.lblShearMod = New System.Windows.Forms.Label()
		Me.lblShearModVal = New System.Windows.Forms.Label()
		Me.lblPahase = New System.Windows.Forms.Label()
		Me.lblPhaseVal = New System.Windows.Forms.Label()
		Me.lblTangent = New System.Windows.Forms.Label()
		Me.lblTangentVal = New System.Windows.Forms.Label()
		Me.GroupBox6 = New System.Windows.Forms.GroupBox()
		Me.btnScale = New System.Windows.Forms.Button()
		Me.gbxStrgElastGScaleSetting = New System.Windows.Forms.GroupBox()
		Me.btnStrgElastGScaleAply = New System.Windows.Forms.Button()
		Me.nudStgElaGYMin = New System.Windows.Forms.NumericUpDown()
		Me.nudStgElaGYMax = New System.Windows.Forms.NumericUpDown()
		Me.lblYMin = New System.Windows.Forms.Label()
		Me.lblYMax = New System.Windows.Forms.Label()
		Me.lblXMin = New System.Windows.Forms.Label()
		Me.lblXMax = New System.Windows.Forms.Label()
		Me.nudStgElaGXMin = New System.Windows.Forms.NumericUpDown()
		Me.nudStgElaGYInterval = New System.Windows.Forms.NumericUpDown()
		Me.nudStgElaGXInterval = New System.Windows.Forms.NumericUpDown()
		Me.nudStgElaGXMax = New System.Windows.Forms.NumericUpDown()
		Me.ChartC = New System.Windows.Forms.DataVisualization.Charting.Chart()
		Me.gbxConditionSetSpeed = New System.Windows.Forms.GroupBox()
		Me.btnCloseSpeed = New System.Windows.Forms.Button()
		Me.gbxConditionSetTemp = New System.Windows.Forms.GroupBox()
		Me.btnCloseTemp = New System.Windows.Forms.Button()
		Me.gbxConditionSetTime = New System.Windows.Forms.GroupBox()
		Me.btnCloseTime = New System.Windows.Forms.Button()
		Me.nudThinning = New System.Windows.Forms.NumericUpDown()
		Me.lblThinningLabel = New System.Windows.Forms.Label()
		Me.gbxRecovery = New System.Windows.Forms.GroupBox()
		Me.lblRecoveryAmpLabel = New System.Windows.Forms.Label()
		Me.lblRecoveryTimeLabel = New System.Windows.Forms.Label()
		Me.nudRecoveryAmp = New System.Windows.Forms.NumericUpDown()
		Me.nudRecoveryTime = New System.Windows.Forms.NumericUpDown()
		Me.gbxPreShear = New System.Windows.Forms.GroupBox()
		Me.lblPreShearAmpLabel = New System.Windows.Forms.Label()
		Me.lblPreShearTimeLabel = New System.Windows.Forms.Label()
		Me.nudPreShearAmp = New System.Windows.Forms.NumericUpDown()
		Me.nudPreShearTime = New System.Windows.Forms.NumericUpDown()
		Me.gbxMeasMode = New System.Windows.Forms.GroupBox()
		Me.btnTemp = New System.Windows.Forms.Button()
		Me.btnTime = New System.Windows.Forms.Button()
		Me.btnSpeed = New System.Windows.Forms.Button()
		CType(Me.dgvCurrVal,System.ComponentModel.ISupportInitialize).BeginInit
		Me.GroupBox2.SuspendLayout
		Me.GroupBox4.SuspendLayout
		CType(Me.nudPreTemp,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.nudInputGap,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.nudXYWait,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.nudProbedDiameter,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.nudRisingHPWait,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.nudMeasureGap,System.ComponentModel.ISupportInitialize).BeginInit
		Me.GroupBox3.SuspendLayout
		CType(Me.nudSpeedDepSampleTime,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.nudSpeedDepAcquisitionTime,System.ComponentModel.ISupportInitialize).BeginInit
		Me.StatusStrip1.SuspendLayout
		Me.gbxSpeedDep.SuspendLayout
		CType(Me.nudSpeedDepAmpStep,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.nudSpeedDepAmpEnd,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.nudSpeedDepAmpStart,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.nudSpeedDepPeriod,System.ComponentModel.ISupportInitialize).BeginInit
		Me.gbxHighShear.SuspendLayout
		CType(Me.nudHighAmp,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.nudHighTime,System.ComponentModel.ISupportInitialize).BeginInit
		Me.gbxLowShear2.SuspendLayout
		CType(Me.nudLow2Amp,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.nudLow2Time,System.ComponentModel.ISupportInitialize).BeginInit
		Me.gbxLowShear1.SuspendLayout
		CType(Me.nudLow1Amp,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.nudLow1Time,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.nudTimeDepPeriod,System.ComponentModel.ISupportInitialize).BeginInit
		Me.GroupBox5.SuspendLayout
		CType(Me.nudTempDepSampleTime,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.nudTempDepAcquisitionTime,System.ComponentModel.ISupportInitialize).BeginInit
		Me.GroupBox1.SuspendLayout
		CType(Me.nudTempDepAmp,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.nudTempDepTempStep,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.nudTempDepTempEnd,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.nudTempDepTempStart,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.nudTempDepPeriod,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.ChartA,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.ChartB,System.ComponentModel.ISupportInitialize).BeginInit
		Me.Panel2.SuspendLayout
		CType(Me.Chart2,System.ComponentModel.ISupportInitialize).BeginInit
		Me.Panel1.SuspendLayout
		CType(Me.pctConStts,System.ComponentModel.ISupportInitialize).BeginInit
		Me.FlowLayoutPanel1.SuspendLayout
		Me.GroupBox6.SuspendLayout
		Me.gbxStrgElastGScaleSetting.SuspendLayout
		CType(Me.nudStgElaGYMin,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.nudStgElaGYMax,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.nudStgElaGXMin,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.nudStgElaGYInterval,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.nudStgElaGXInterval,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.nudStgElaGXMax,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.ChartC,System.ComponentModel.ISupportInitialize).BeginInit
		Me.gbxConditionSetSpeed.SuspendLayout
		Me.gbxConditionSetTemp.SuspendLayout
		Me.gbxConditionSetTime.SuspendLayout
		CType(Me.nudThinning,System.ComponentModel.ISupportInitialize).BeginInit
		Me.gbxRecovery.SuspendLayout
		CType(Me.nudRecoveryAmp,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.nudRecoveryTime,System.ComponentModel.ISupportInitialize).BeginInit
		Me.gbxPreShear.SuspendLayout
		CType(Me.nudPreShearAmp,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.nudPreShearTime,System.ComponentModel.ISupportInitialize).BeginInit
		Me.gbxMeasMode.SuspendLayout
		Me.SuspendLayout
		'
		'dgvCurrVal
		'
		Me.dgvCurrVal.AllowUserToAddRows = false
		Me.dgvCurrVal.AllowUserToDeleteRows = false
		Me.dgvCurrVal.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
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
		DataGridViewCellStyle4.Font = New System.Drawing.Font("MS UI Gothic", 9!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		DataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlText
		DataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.Transparent
		DataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.ControlText
		DataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
		Me.dgvCurrVal.DefaultCellStyle = DataGridViewCellStyle4
		Me.dgvCurrVal.GridColor = System.Drawing.SystemColors.ButtonHighlight
		Me.dgvCurrVal.Location = New System.Drawing.Point(1586, 619)
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
		Me.dgvCurrVal.Size = New System.Drawing.Size(312, 254)
		Me.dgvCurrVal.TabIndex = 50
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
		'GroupBox2
		'
		Me.GroupBox2.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
		Me.GroupBox2.BackColor = System.Drawing.SystemColors.ControlLight
		Me.GroupBox2.Controls.Add(Me.btnClose)
		Me.GroupBox2.Controls.Add(Me.btnTemperature)
		Me.GroupBox2.Controls.Add(Me.btnOrigin)
		Me.GroupBox2.Controls.Add(Me.btnZeroAdj)
		Me.GroupBox2.Controls.Add(Me.btnExec)
		Me.GroupBox2.Controls.Add(Me.btnStop)
		Me.GroupBox2.Location = New System.Drawing.Point(1587, 43)
		Me.GroupBox2.Name = "GroupBox2"
		Me.GroupBox2.Size = New System.Drawing.Size(312, 166)
		Me.GroupBox2.TabIndex = 51
		Me.GroupBox2.TabStop = false
		'
		'btnClose
		'
		Me.btnClose.BackColor = System.Drawing.SystemColors.ButtonFace
		Me.btnClose.Font = New System.Drawing.Font("MS UI Gothic", 18!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.btnClose.Location = New System.Drawing.Point(10, 262)
		Me.btnClose.Name = "btnClose"
		Me.btnClose.Size = New System.Drawing.Size(296, 48)
		Me.btnClose.TabIndex = 42
		Me.btnClose.Text = "Close"
		Me.btnClose.UseVisualStyleBackColor = false
		Me.btnClose.Visible = false
		'
		'btnTemperature
		'
		Me.btnTemperature.BackColor = System.Drawing.SystemColors.ButtonFace
		Me.btnTemperature.Font = New System.Drawing.Font("MS UI Gothic", 18!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.btnTemperature.Image = Global.SZKTool.My.Resources.Resources.tcbtn
		Me.btnTemperature.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me.btnTemperature.Location = New System.Drawing.Point(8, 5)
		Me.btnTemperature.Name = "btnTemperature"
		Me.btnTemperature.Size = New System.Drawing.Size(296, 40)
		Me.btnTemperature.TabIndex = 4
		Me.btnTemperature.Text = "Temperature control"
		Me.btnTemperature.UseVisualStyleBackColor = false
		'
		'btnOrigin
		'
		Me.btnOrigin.BackColor = System.Drawing.SystemColors.ButtonFace
		Me.btnOrigin.Font = New System.Drawing.Font("MS UI Gothic", 18!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.btnOrigin.Location = New System.Drawing.Point(9, 209)
		Me.btnOrigin.Name = "btnOrigin"
		Me.btnOrigin.Size = New System.Drawing.Size(296, 48)
		Me.btnOrigin.TabIndex = 50
		Me.btnOrigin.Text = "Return Origin"
		Me.btnOrigin.UseVisualStyleBackColor = false
		Me.btnOrigin.Visible = false
		'
		'btnZeroAdj
		'
		Me.btnZeroAdj.BackColor = System.Drawing.SystemColors.ButtonFace
		Me.btnZeroAdj.Font = New System.Drawing.Font("MS UI Gothic", 18!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.btnZeroAdj.Image = Global.SZKTool.My.Resources.Resources.zerobtn
		Me.btnZeroAdj.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me.btnZeroAdj.Location = New System.Drawing.Point(8, 44)
		Me.btnZeroAdj.Name = "btnZeroAdj"
		Me.btnZeroAdj.Size = New System.Drawing.Size(296, 40)
		Me.btnZeroAdj.TabIndex = 4
		Me.btnZeroAdj.Text = "Zero Adjustment"
		Me.btnZeroAdj.UseVisualStyleBackColor = false
		'
		'btnExec
		'
		Me.btnExec.BackColor = System.Drawing.SystemColors.ButtonFace
		Me.btnExec.Font = New System.Drawing.Font("MS UI Gothic", 18!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.btnExec.Image = Global.SZKTool.My.Resources.Resources.PlayHS
		Me.btnExec.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me.btnExec.Location = New System.Drawing.Point(8, 83)
		Me.btnExec.Name = "btnExec"
		Me.btnExec.Size = New System.Drawing.Size(296, 40)
		Me.btnExec.TabIndex = 43
		Me.btnExec.Text = "Exec"
		Me.btnExec.UseVisualStyleBackColor = false
		'
		'btnStop
		'
		Me.btnStop.BackColor = System.Drawing.SystemColors.ButtonFace
		Me.btnStop.Font = New System.Drawing.Font("MS UI Gothic", 18!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.btnStop.Image = Global.SZKTool.My.Resources.Resources.stopbtn
		Me.btnStop.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me.btnStop.Location = New System.Drawing.Point(8, 122)
		Me.btnStop.Name = "btnStop"
		Me.btnStop.Size = New System.Drawing.Size(296, 40)
		Me.btnStop.TabIndex = 44
		Me.btnStop.Text = "Stop"
		Me.btnStop.UseVisualStyleBackColor = false
		'
		'GroupBox4
		'
		Me.GroupBox4.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
		Me.GroupBox4.BackColor = System.Drawing.SystemColors.ControlLight
		Me.GroupBox4.Controls.Add(Me.rbXAction)
		Me.GroupBox4.Controls.Add(Me.rbCircleAction)
		Me.GroupBox4.Controls.Add(Me.lblProbedDiameterLabel)
		Me.GroupBox4.Controls.Add(Me.nudPreTemp)
		Me.GroupBox4.Controls.Add(Me.lblInputGap)
		Me.GroupBox4.Controls.Add(Me.nudInputGap)
		Me.GroupBox4.Controls.Add(Me.nudXYWait)
		Me.GroupBox4.Controls.Add(Me.nudProbedDiameter)
		Me.GroupBox4.Controls.Add(Me.nudRisingHPWait)
		Me.GroupBox4.Controls.Add(Me.lblPresetTemp)
		Me.GroupBox4.Controls.Add(Me.lblHPUpWait)
		Me.GroupBox4.Controls.Add(Me.lblXYWait)
		Me.GroupBox4.Controls.Add(Me.nudMeasureGap)
		Me.GroupBox4.Controls.Add(Me.lblMeasureGap)
		Me.GroupBox4.Location = New System.Drawing.Point(1587, 203)
		Me.GroupBox4.Name = "GroupBox4"
		Me.GroupBox4.Size = New System.Drawing.Size(312, 227)
		Me.GroupBox4.TabIndex = 53
		Me.GroupBox4.TabStop = false
		'
		'rbXAction
		'
		Me.rbXAction.AutoSize = true
		Me.rbXAction.Font = New System.Drawing.Font("Meiryo UI", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.rbXAction.Location = New System.Drawing.Point(104, 200)
		Me.rbXAction.Name = "rbXAction"
		Me.rbXAction.Size = New System.Drawing.Size(111, 24)
		Me.rbXAction.TabIndex = 50
		Me.rbXAction.Text = "X direction"
		Me.rbXAction.UseVisualStyleBackColor = true
		'
		'rbCircleAction
		'
		Me.rbCircleAction.AutoSize = true
		Me.rbCircleAction.Checked = true
		Me.rbCircleAction.Font = New System.Drawing.Font("Meiryo UI", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.rbCircleAction.Location = New System.Drawing.Point(10, 200)
		Me.rbCircleAction.Name = "rbCircleAction"
		Me.rbCircleAction.Size = New System.Drawing.Size(70, 24)
		Me.rbCircleAction.TabIndex = 50
		Me.rbCircleAction.TabStop = true
		Me.rbCircleAction.Text = "Circle"
		Me.rbCircleAction.UseVisualStyleBackColor = true
		'
		'lblProbedDiameterLabel
		'
		Me.lblProbedDiameterLabel.AutoSize = true
		Me.lblProbedDiameterLabel.Font = New System.Drawing.Font("Meiryo UI", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.lblProbedDiameterLabel.Location = New System.Drawing.Point(6, 173)
		Me.lblProbedDiameterLabel.Name = "lblProbedDiameterLabel"
		Me.lblProbedDiameterLabel.Size = New System.Drawing.Size(178, 20)
		Me.lblProbedDiameterLabel.TabIndex = 49
		Me.lblProbedDiameterLabel.Text = "Probe diameter (mm)"
		'
		'nudPreTemp
		'
		Me.nudPreTemp.DecimalPlaces = 1
		Me.nudPreTemp.Font = New System.Drawing.Font("Meiryo UI", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.nudPreTemp.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
		Me.nudPreTemp.Location = New System.Drawing.Point(222, 2)
		Me.nudPreTemp.Maximum = New Decimal(New Integer() {240, 0, 0, 0})
		Me.nudPreTemp.Name = "nudPreTemp"
		Me.nudPreTemp.Size = New System.Drawing.Size(80, 32)
		Me.nudPreTemp.TabIndex = 45
		Me.nudPreTemp.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		Me.nudPreTemp.Value = New Decimal(New Integer() {250, 0, 0, 65536})
		'
		'lblInputGap
		'
		Me.lblInputGap.AutoSize = true
		Me.lblInputGap.Font = New System.Drawing.Font("Meiryo UI", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.lblInputGap.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
		Me.lblInputGap.Location = New System.Drawing.Point(5, 42)
		Me.lblInputGap.Name = "lblInputGap"
		Me.lblInputGap.Size = New System.Drawing.Size(136, 20)
		Me.lblInputGap.TabIndex = 46
		Me.lblInputGap.Text = "Input Gap (mm)"
		'
		'nudInputGap
		'
		Me.nudInputGap.DecimalPlaces = 1
		Me.nudInputGap.Font = New System.Drawing.Font("Meiryo UI", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.nudInputGap.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
		Me.nudInputGap.Location = New System.Drawing.Point(222, 35)
		Me.nudInputGap.Maximum = New Decimal(New Integer() {50, 0, 0, 0})
		Me.nudInputGap.Name = "nudInputGap"
		Me.nudInputGap.Size = New System.Drawing.Size(80, 32)
		Me.nudInputGap.TabIndex = 45
		Me.nudInputGap.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		Me.nudInputGap.Value = New Decimal(New Integer() {20, 0, 0, 0})
		'
		'nudXYWait
		'
		Me.nudXYWait.DecimalPlaces = 1
		Me.nudXYWait.Font = New System.Drawing.Font("Meiryo UI", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.nudXYWait.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
		Me.nudXYWait.Location = New System.Drawing.Point(222, 100)
		Me.nudXYWait.Maximum = New Decimal(New Integer() {500, 0, 0, 0})
		Me.nudXYWait.Name = "nudXYWait"
		Me.nudXYWait.Size = New System.Drawing.Size(80, 32)
		Me.nudXYWait.TabIndex = 45
		Me.nudXYWait.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		Me.nudXYWait.Value = New Decimal(New Integer() {50, 0, 0, 0})
		'
		'nudProbedDiameter
		'
		Me.nudProbedDiameter.DecimalPlaces = 2
		Me.nudProbedDiameter.Font = New System.Drawing.Font("Meiryo UI", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.nudProbedDiameter.Increment = New Decimal(New Integer() {1, 0, 0, 131072})
		Me.nudProbedDiameter.Location = New System.Drawing.Point(222, 168)
		Me.nudProbedDiameter.Maximum = New Decimal(New Integer() {999, 0, 0, 0})
		Me.nudProbedDiameter.Minimum = New Decimal(New Integer() {1, 0, 0, 131072})
		Me.nudProbedDiameter.Name = "nudProbedDiameter"
		Me.nudProbedDiameter.Size = New System.Drawing.Size(80, 32)
		Me.nudProbedDiameter.TabIndex = 45
		Me.nudProbedDiameter.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		Me.nudProbedDiameter.Value = New Decimal(New Integer() {20, 0, 0, 0})
		'
		'nudRisingHPWait
		'
		Me.nudRisingHPWait.DecimalPlaces = 1
		Me.nudRisingHPWait.Font = New System.Drawing.Font("Meiryo UI", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.nudRisingHPWait.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
		Me.nudRisingHPWait.Location = New System.Drawing.Point(222, 134)
		Me.nudRisingHPWait.Maximum = New Decimal(New Integer() {300, 0, 0, 0})
		Me.nudRisingHPWait.Name = "nudRisingHPWait"
		Me.nudRisingHPWait.Size = New System.Drawing.Size(80, 32)
		Me.nudRisingHPWait.TabIndex = 45
		Me.nudRisingHPWait.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		Me.nudRisingHPWait.Value = New Decimal(New Integer() {8, 0, 0, 0})
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
		'lblHPUpWait
		'
		Me.lblHPUpWait.AutoSize = true
		Me.lblHPUpWait.Font = New System.Drawing.Font("Meiryo UI", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.lblHPUpWait.Location = New System.Drawing.Point(7, 141)
		Me.lblHPUpWait.Name = "lblHPUpWait"
		Me.lblHPUpWait.Size = New System.Drawing.Size(191, 20)
		Me.lblHPUpWait.TabIndex = 46
		Me.lblHPUpWait.Text = "Wait for rising HP (Sec)"
		'
		'lblXYWait
		'
		Me.lblXYWait.AutoSize = true
		Me.lblXYWait.Font = New System.Drawing.Font("Meiryo UI", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.lblXYWait.Location = New System.Drawing.Point(6, 107)
		Me.lblXYWait.Name = "lblXYWait"
		Me.lblXYWait.Size = New System.Drawing.Size(196, 20)
		Me.lblXYWait.TabIndex = 46
		Me.lblXYWait.Text = "Wait for XY Action (Sec)"
		'
		'nudMeasureGap
		'
		Me.nudMeasureGap.DecimalPlaces = 3
		Me.nudMeasureGap.Font = New System.Drawing.Font("Meiryo UI", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.nudMeasureGap.Increment = New Decimal(New Integer() {1, 0, 0, 196608})
		Me.nudMeasureGap.Location = New System.Drawing.Point(222, 67)
		Me.nudMeasureGap.Maximum = New Decimal(New Integer() {10, 0, 0, 0})
		Me.nudMeasureGap.Minimum = New Decimal(New Integer() {1, 0, 0, 196608})
		Me.nudMeasureGap.Name = "nudMeasureGap"
		Me.nudMeasureGap.Size = New System.Drawing.Size(80, 32)
		Me.nudMeasureGap.TabIndex = 45
		Me.nudMeasureGap.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		Me.nudMeasureGap.Value = New Decimal(New Integer() {5, 0, 0, 65536})
		'
		'lblMeasureGap
		'
		Me.lblMeasureGap.AutoSize = true
		Me.lblMeasureGap.Font = New System.Drawing.Font("Meiryo UI", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.lblMeasureGap.Location = New System.Drawing.Point(5, 74)
		Me.lblMeasureGap.Name = "lblMeasureGap"
		Me.lblMeasureGap.Size = New System.Drawing.Size(199, 20)
		Me.lblMeasureGap.TabIndex = 46
		Me.lblMeasureGap.Text = "Measurement Gap (mm)"
		'
		'GroupBox3
		'
		Me.GroupBox3.BackColor = System.Drawing.Color.Transparent
		Me.GroupBox3.Controls.Add(Me.lblSpeedDepSampleTime)
		Me.GroupBox3.Controls.Add(Me.nudSpeedDepSampleTime)
		Me.GroupBox3.Controls.Add(Me.nudSpeedDepAcquisitionTime)
		Me.GroupBox3.Controls.Add(Me.lblSpeedDepAcquisitionTime)
		Me.GroupBox3.Location = New System.Drawing.Point(8, 212)
		Me.GroupBox3.Name = "GroupBox3"
		Me.GroupBox3.Size = New System.Drawing.Size(312, 76)
		Me.GroupBox3.TabIndex = 52
		Me.GroupBox3.TabStop = false
		'
		'lblSpeedDepSampleTime
		'
		Me.lblSpeedDepSampleTime.AutoSize = true
		Me.lblSpeedDepSampleTime.Font = New System.Drawing.Font("Meiryo UI", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.lblSpeedDepSampleTime.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
		Me.lblSpeedDepSampleTime.Location = New System.Drawing.Point(6, 10)
		Me.lblSpeedDepSampleTime.Name = "lblSpeedDepSampleTime"
		Me.lblSpeedDepSampleTime.Size = New System.Drawing.Size(151, 20)
		Me.lblSpeedDepSampleTime.TabIndex = 46
		Me.lblSpeedDepSampleTime.Text = "Sample time (Sec)"
		'
		'nudSpeedDepSampleTime
		'
		Me.nudSpeedDepSampleTime.Font = New System.Drawing.Font("Meiryo UI", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.nudSpeedDepSampleTime.Location = New System.Drawing.Point(206, 3)
		Me.nudSpeedDepSampleTime.Maximum = New Decimal(New Integer() {99999, 0, 0, 0})
		Me.nudSpeedDepSampleTime.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
		Me.nudSpeedDepSampleTime.Name = "nudSpeedDepSampleTime"
		Me.nudSpeedDepSampleTime.Size = New System.Drawing.Size(98, 32)
		Me.nudSpeedDepSampleTime.TabIndex = 45
		Me.nudSpeedDepSampleTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		Me.nudSpeedDepSampleTime.Value = New Decimal(New Integer() {10, 0, 0, 0})
		'
		'nudSpeedDepAcquisitionTime
		'
		Me.nudSpeedDepAcquisitionTime.DecimalPlaces = 1
		Me.nudSpeedDepAcquisitionTime.Font = New System.Drawing.Font("Meiryo UI", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.nudSpeedDepAcquisitionTime.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
		Me.nudSpeedDepAcquisitionTime.Location = New System.Drawing.Point(206, 36)
		Me.nudSpeedDepAcquisitionTime.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
		Me.nudSpeedDepAcquisitionTime.Name = "nudSpeedDepAcquisitionTime"
		Me.nudSpeedDepAcquisitionTime.Size = New System.Drawing.Size(98, 32)
		Me.nudSpeedDepAcquisitionTime.TabIndex = 45
		Me.nudSpeedDepAcquisitionTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		Me.nudSpeedDepAcquisitionTime.Value = New Decimal(New Integer() {50, 0, 0, 0})
		'
		'lblSpeedDepAcquisitionTime
		'
		Me.lblSpeedDepAcquisitionTime.AutoSize = true
		Me.lblSpeedDepAcquisitionTime.Font = New System.Drawing.Font("Meiryo UI", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.lblSpeedDepAcquisitionTime.Location = New System.Drawing.Point(6, 41)
		Me.lblSpeedDepAcquisitionTime.Name = "lblSpeedDepAcquisitionTime"
		Me.lblSpeedDepAcquisitionTime.Size = New System.Drawing.Size(179, 20)
		Me.lblSpeedDepAcquisitionTime.TabIndex = 46
		Me.lblSpeedDepAcquisitionTime.Text = "Acquisition time ( % )"
		'
		'StatusStrip1
		'
		Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.lblMsg})
		Me.StatusStrip1.Location = New System.Drawing.Point(0, 1019)
		Me.StatusStrip1.Name = "StatusStrip1"
		Me.StatusStrip1.Size = New System.Drawing.Size(1904, 22)
		Me.StatusStrip1.SizingGrip = false
		Me.StatusStrip1.TabIndex = 55
		Me.StatusStrip1.Text = "StatusStrip1"
		'
		'lblMsg
		'
		Me.lblMsg.Font = New System.Drawing.Font("Yu Gothic UI", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.lblMsg.Name = "lblMsg"
		Me.lblMsg.Size = New System.Drawing.Size(0, 17)
		'
		'gbxSpeedDep
		'
		Me.gbxSpeedDep.BackColor = System.Drawing.Color.Transparent
		Me.gbxSpeedDep.Controls.Add(Me.ckbFlcvReverse)
		Me.gbxSpeedDep.Controls.Add(Me.lblSpeedDepStepWidthLabel)
		Me.gbxSpeedDep.Controls.Add(Me.nudSpeedDepAmpStep)
		Me.gbxSpeedDep.Controls.Add(Me.nudSpeedDepAmpEnd)
		Me.gbxSpeedDep.Controls.Add(Me.nudSpeedDepAmpStart)
		Me.gbxSpeedDep.Controls.Add(Me.nudSpeedDepPeriod)
		Me.gbxSpeedDep.Controls.Add(Me.lblSpeedDepAmpEndLabel)
		Me.gbxSpeedDep.Controls.Add(Me.lblSpeedDepAmpStartLabel)
		Me.gbxSpeedDep.Controls.Add(Me.lblSpeedDepPeriodLabel)
		Me.gbxSpeedDep.Location = New System.Drawing.Point(8, 22)
		Me.gbxSpeedDep.Name = "gbxSpeedDep"
		Me.gbxSpeedDep.Size = New System.Drawing.Size(312, 176)
		Me.gbxSpeedDep.TabIndex = 57
		Me.gbxSpeedDep.TabStop = false
		'
		'ckbFlcvReverse
		'
		Me.ckbFlcvReverse.AutoSize = true
		Me.ckbFlcvReverse.Font = New System.Drawing.Font("Meiryo UI", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.ckbFlcvReverse.Location = New System.Drawing.Point(10, 145)
		Me.ckbFlcvReverse.Name = "ckbFlcvReverse"
		Me.ckbFlcvReverse.Size = New System.Drawing.Size(202, 24)
		Me.ckbFlcvReverse.TabIndex = 49
		Me.ckbFlcvReverse.Text = "Reverce measurement"
		Me.ckbFlcvReverse.UseVisualStyleBackColor = true
		'
		'lblSpeedDepStepWidthLabel
		'
		Me.lblSpeedDepStepWidthLabel.AutoSize = true
		Me.lblSpeedDepStepWidthLabel.Font = New System.Drawing.Font("Meiryo UI", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.lblSpeedDepStepWidthLabel.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
		Me.lblSpeedDepStepWidthLabel.Location = New System.Drawing.Point(6, 118)
		Me.lblSpeedDepStepWidthLabel.Name = "lblSpeedDepStepWidthLabel"
		Me.lblSpeedDepStepWidthLabel.Size = New System.Drawing.Size(188, 20)
		Me.lblSpeedDepStepWidthLabel.TabIndex = 48
		Me.lblSpeedDepStepWidthLabel.Text = "Amp. Step width (mm)"
		'
		'nudSpeedDepAmpStep
		'
		Me.nudSpeedDepAmpStep.DecimalPlaces = 3
		Me.nudSpeedDepAmpStep.Font = New System.Drawing.Font("Meiryo UI", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.nudSpeedDepAmpStep.Increment = New Decimal(New Integer() {1, 0, 0, 196608})
		Me.nudSpeedDepAmpStep.Location = New System.Drawing.Point(206, 111)
		Me.nudSpeedDepAmpStep.Maximum = New Decimal(New Integer() {1, 0, 0, 0})
		Me.nudSpeedDepAmpStep.Minimum = New Decimal(New Integer() {1, 0, 0, 196608})
		Me.nudSpeedDepAmpStep.Name = "nudSpeedDepAmpStep"
		Me.nudSpeedDepAmpStep.Size = New System.Drawing.Size(98, 32)
		Me.nudSpeedDepAmpStep.TabIndex = 45
		Me.nudSpeedDepAmpStep.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		Me.nudSpeedDepAmpStep.Value = New Decimal(New Integer() {1, 0, 0, 65536})
		'
		'nudSpeedDepAmpEnd
		'
		Me.nudSpeedDepAmpEnd.DecimalPlaces = 3
		Me.nudSpeedDepAmpEnd.Font = New System.Drawing.Font("Meiryo UI", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.nudSpeedDepAmpEnd.Increment = New Decimal(New Integer() {1, 0, 0, 196608})
		Me.nudSpeedDepAmpEnd.Location = New System.Drawing.Point(206, 76)
		Me.nudSpeedDepAmpEnd.Maximum = New Decimal(New Integer() {10, 0, 0, 0})
		Me.nudSpeedDepAmpEnd.Name = "nudSpeedDepAmpEnd"
		Me.nudSpeedDepAmpEnd.Size = New System.Drawing.Size(98, 32)
		Me.nudSpeedDepAmpEnd.TabIndex = 45
		Me.nudSpeedDepAmpEnd.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		Me.nudSpeedDepAmpEnd.Value = New Decimal(New Integer() {2, 0, 0, 0})
		'
		'nudSpeedDepAmpStart
		'
		Me.nudSpeedDepAmpStart.DecimalPlaces = 3
		Me.nudSpeedDepAmpStart.Font = New System.Drawing.Font("Meiryo UI", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.nudSpeedDepAmpStart.Increment = New Decimal(New Integer() {1, 0, 0, 196608})
		Me.nudSpeedDepAmpStart.Location = New System.Drawing.Point(206, 42)
		Me.nudSpeedDepAmpStart.Maximum = New Decimal(New Integer() {10, 0, 0, 0})
		Me.nudSpeedDepAmpStart.Name = "nudSpeedDepAmpStart"
		Me.nudSpeedDepAmpStart.Size = New System.Drawing.Size(98, 32)
		Me.nudSpeedDepAmpStart.TabIndex = 45
		Me.nudSpeedDepAmpStart.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		Me.nudSpeedDepAmpStart.Value = New Decimal(New Integer() {1, 0, 0, 0})
		'
		'nudSpeedDepPeriod
		'
		Me.nudSpeedDepPeriod.DecimalPlaces = 3
		Me.nudSpeedDepPeriod.Font = New System.Drawing.Font("Meiryo UI", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.nudSpeedDepPeriod.Increment = New Decimal(New Integer() {1, 0, 0, 131072})
		Me.nudSpeedDepPeriod.Location = New System.Drawing.Point(206, 8)
		Me.nudSpeedDepPeriod.Maximum = New Decimal(New Integer() {10, 0, 0, 0})
		Me.nudSpeedDepPeriod.Minimum = New Decimal(New Integer() {1, 0, 0, 131072})
		Me.nudSpeedDepPeriod.Name = "nudSpeedDepPeriod"
		Me.nudSpeedDepPeriod.Size = New System.Drawing.Size(98, 32)
		Me.nudSpeedDepPeriod.TabIndex = 45
		Me.nudSpeedDepPeriod.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		Me.nudSpeedDepPeriod.Value = New Decimal(New Integer() {1, 0, 0, 0})
		'
		'lblSpeedDepAmpEndLabel
		'
		Me.lblSpeedDepAmpEndLabel.AutoSize = true
		Me.lblSpeedDepAmpEndLabel.Font = New System.Drawing.Font("Meiryo UI", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.lblSpeedDepAmpEndLabel.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
		Me.lblSpeedDepAmpEndLabel.Location = New System.Drawing.Point(6, 83)
		Me.lblSpeedDepAmpEndLabel.Name = "lblSpeedDepAmpEndLabel"
		Me.lblSpeedDepAmpEndLabel.Size = New System.Drawing.Size(172, 20)
		Me.lblSpeedDepAmpEndLabel.TabIndex = 48
		Me.lblSpeedDepAmpEndLabel.Text = "Amplitude End (mm)"
		'
		'lblSpeedDepAmpStartLabel
		'
		Me.lblSpeedDepAmpStartLabel.AutoSize = true
		Me.lblSpeedDepAmpStartLabel.Font = New System.Drawing.Font("Meiryo UI", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.lblSpeedDepAmpStartLabel.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
		Me.lblSpeedDepAmpStartLabel.Location = New System.Drawing.Point(6, 49)
		Me.lblSpeedDepAmpStartLabel.Name = "lblSpeedDepAmpStartLabel"
		Me.lblSpeedDepAmpStartLabel.Size = New System.Drawing.Size(180, 20)
		Me.lblSpeedDepAmpStartLabel.TabIndex = 47
		Me.lblSpeedDepAmpStartLabel.Text = "Amplitude Start (mm)"
		'
		'lblSpeedDepPeriodLabel
		'
		Me.lblSpeedDepPeriodLabel.AutoSize = true
		Me.lblSpeedDepPeriodLabel.Font = New System.Drawing.Font("Meiryo UI", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.lblSpeedDepPeriodLabel.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
		Me.lblSpeedDepPeriodLabel.Location = New System.Drawing.Point(6, 15)
		Me.lblSpeedDepPeriodLabel.Name = "lblSpeedDepPeriodLabel"
		Me.lblSpeedDepPeriodLabel.Size = New System.Drawing.Size(105, 20)
		Me.lblSpeedDepPeriodLabel.TabIndex = 47
		Me.lblSpeedDepPeriodLabel.Text = "Period (Sec)"
		'
		'gbxHighShear
		'
		Me.gbxHighShear.Controls.Add(Me.lblHighAmpLabel)
		Me.gbxHighShear.Controls.Add(Me.lblHighTimeLabel)
		Me.gbxHighShear.Controls.Add(Me.nudHighAmp)
		Me.gbxHighShear.Controls.Add(Me.nudHighTime)
		Me.gbxHighShear.Font = New System.Drawing.Font("Meiryo UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.gbxHighShear.ForeColor = System.Drawing.Color.Blue
		Me.gbxHighShear.Location = New System.Drawing.Point(9, 237)
		Me.gbxHighShear.Name = "gbxHighShear"
		Me.gbxHighShear.Size = New System.Drawing.Size(293, 72)
		Me.gbxHighShear.TabIndex = 68
		Me.gbxHighShear.TabStop = false
		Me.gbxHighShear.Text = "HighShere"
		'
		'lblHighAmpLabel
		'
		Me.lblHighAmpLabel.AutoSize = true
		Me.lblHighAmpLabel.Font = New System.Drawing.Font("Meiryo UI", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.lblHighAmpLabel.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
		Me.lblHighAmpLabel.Location = New System.Drawing.Point(6, 20)
		Me.lblHighAmpLabel.Name = "lblHighAmpLabel"
		Me.lblHighAmpLabel.Size = New System.Drawing.Size(130, 19)
		Me.lblHighAmpLabel.TabIndex = 47
		Me.lblHighAmpLabel.Text = "Amplitude (mm)"
		'
		'lblHighTimeLabel
		'
		Me.lblHighTimeLabel.AutoSize = true
		Me.lblHighTimeLabel.Font = New System.Drawing.Font("Meiryo UI", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.lblHighTimeLabel.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
		Me.lblHighTimeLabel.Location = New System.Drawing.Point(6, 43)
		Me.lblHighTimeLabel.Name = "lblHighTimeLabel"
		Me.lblHighTimeLabel.Size = New System.Drawing.Size(165, 19)
		Me.lblHighTimeLabel.TabIndex = 47
		Me.lblHighTimeLabel.Text = "Operating time (Sec)"
		'
		'nudHighAmp
		'
		Me.nudHighAmp.DecimalPlaces = 3
		Me.nudHighAmp.Font = New System.Drawing.Font("Meiryo UI", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.nudHighAmp.Increment = New Decimal(New Integer() {1, 0, 0, 196608})
		Me.nudHighAmp.Location = New System.Drawing.Point(195, 13)
		Me.nudHighAmp.Maximum = New Decimal(New Integer() {10, 0, 0, 0})
		Me.nudHighAmp.Name = "nudHighAmp"
		Me.nudHighAmp.Size = New System.Drawing.Size(98, 27)
		Me.nudHighAmp.TabIndex = 45
		Me.nudHighAmp.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		Me.nudHighAmp.Value = New Decimal(New Integer() {1, 0, 0, 0})
		'
		'nudHighTime
		'
		Me.nudHighTime.DecimalPlaces = 1
		Me.nudHighTime.Font = New System.Drawing.Font("Meiryo UI", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.nudHighTime.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
		Me.nudHighTime.Location = New System.Drawing.Point(195, 41)
		Me.nudHighTime.Maximum = New Decimal(New Integer() {1000, 0, 0, 0})
		Me.nudHighTime.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
		Me.nudHighTime.Name = "nudHighTime"
		Me.nudHighTime.Size = New System.Drawing.Size(98, 27)
		Me.nudHighTime.TabIndex = 45
		Me.nudHighTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		Me.nudHighTime.Value = New Decimal(New Integer() {1, 0, 0, 0})
		'
		'gbxLowShear2
		'
		Me.gbxLowShear2.Controls.Add(Me.lblLow2AmpLabel)
		Me.gbxLowShear2.Controls.Add(Me.lblLow2TimeLabel)
		Me.gbxLowShear2.Controls.Add(Me.nudLow2Amp)
		Me.gbxLowShear2.Controls.Add(Me.nudLow2Time)
		Me.gbxLowShear2.Font = New System.Drawing.Font("Meiryo UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.gbxLowShear2.ForeColor = System.Drawing.Color.Blue
		Me.gbxLowShear2.Location = New System.Drawing.Point(9, 307)
		Me.gbxLowShear2.Name = "gbxLowShear2"
		Me.gbxLowShear2.Size = New System.Drawing.Size(293, 72)
		Me.gbxLowShear2.TabIndex = 68
		Me.gbxLowShear2.TabStop = false
		Me.gbxLowShear2.Text = "LowShere2"
		'
		'lblLow2AmpLabel
		'
		Me.lblLow2AmpLabel.AutoSize = true
		Me.lblLow2AmpLabel.Font = New System.Drawing.Font("Meiryo UI", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.lblLow2AmpLabel.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
		Me.lblLow2AmpLabel.Location = New System.Drawing.Point(6, 20)
		Me.lblLow2AmpLabel.Name = "lblLow2AmpLabel"
		Me.lblLow2AmpLabel.Size = New System.Drawing.Size(130, 19)
		Me.lblLow2AmpLabel.TabIndex = 47
		Me.lblLow2AmpLabel.Text = "Amplitude (mm)"
		'
		'lblLow2TimeLabel
		'
		Me.lblLow2TimeLabel.AutoSize = true
		Me.lblLow2TimeLabel.Font = New System.Drawing.Font("Meiryo UI", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.lblLow2TimeLabel.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
		Me.lblLow2TimeLabel.Location = New System.Drawing.Point(6, 43)
		Me.lblLow2TimeLabel.Name = "lblLow2TimeLabel"
		Me.lblLow2TimeLabel.Size = New System.Drawing.Size(165, 19)
		Me.lblLow2TimeLabel.TabIndex = 47
		Me.lblLow2TimeLabel.Text = "Operating time (Sec)"
		'
		'nudLow2Amp
		'
		Me.nudLow2Amp.DecimalPlaces = 3
		Me.nudLow2Amp.Font = New System.Drawing.Font("Meiryo UI", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.nudLow2Amp.Increment = New Decimal(New Integer() {1, 0, 0, 196608})
		Me.nudLow2Amp.Location = New System.Drawing.Point(195, 13)
		Me.nudLow2Amp.Maximum = New Decimal(New Integer() {10, 0, 0, 0})
		Me.nudLow2Amp.Name = "nudLow2Amp"
		Me.nudLow2Amp.Size = New System.Drawing.Size(98, 27)
		Me.nudLow2Amp.TabIndex = 45
		Me.nudLow2Amp.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		Me.nudLow2Amp.Value = New Decimal(New Integer() {1, 0, 0, 0})
		'
		'nudLow2Time
		'
		Me.nudLow2Time.DecimalPlaces = 1
		Me.nudLow2Time.Font = New System.Drawing.Font("Meiryo UI", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.nudLow2Time.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
		Me.nudLow2Time.Location = New System.Drawing.Point(195, 41)
		Me.nudLow2Time.Maximum = New Decimal(New Integer() {1000, 0, 0, 0})
		Me.nudLow2Time.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
		Me.nudLow2Time.Name = "nudLow2Time"
		Me.nudLow2Time.Size = New System.Drawing.Size(98, 27)
		Me.nudLow2Time.TabIndex = 45
		Me.nudLow2Time.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		Me.nudLow2Time.Value = New Decimal(New Integer() {1, 0, 0, 0})
		'
		'gbxLowShear1
		'
		Me.gbxLowShear1.Controls.Add(Me.lblLow1AmpLabel)
		Me.gbxLowShear1.Controls.Add(Me.lblLow1TimeLabel)
		Me.gbxLowShear1.Controls.Add(Me.nudLow1Amp)
		Me.gbxLowShear1.Controls.Add(Me.nudLow1Time)
		Me.gbxLowShear1.Font = New System.Drawing.Font("Meiryo UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.gbxLowShear1.ForeColor = System.Drawing.Color.Blue
		Me.gbxLowShear1.Location = New System.Drawing.Point(9, 172)
		Me.gbxLowShear1.Name = "gbxLowShear1"
		Me.gbxLowShear1.Size = New System.Drawing.Size(293, 72)
		Me.gbxLowShear1.TabIndex = 68
		Me.gbxLowShear1.TabStop = false
		Me.gbxLowShear1.Text = "LowShere1"
		'
		'lblLow1AmpLabel
		'
		Me.lblLow1AmpLabel.AutoSize = true
		Me.lblLow1AmpLabel.Font = New System.Drawing.Font("Meiryo UI", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.lblLow1AmpLabel.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
		Me.lblLow1AmpLabel.Location = New System.Drawing.Point(6, 20)
		Me.lblLow1AmpLabel.Name = "lblLow1AmpLabel"
		Me.lblLow1AmpLabel.Size = New System.Drawing.Size(130, 19)
		Me.lblLow1AmpLabel.TabIndex = 47
		Me.lblLow1AmpLabel.Text = "Amplitude (mm)"
		'
		'lblLow1TimeLabel
		'
		Me.lblLow1TimeLabel.AutoSize = true
		Me.lblLow1TimeLabel.Font = New System.Drawing.Font("Meiryo UI", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.lblLow1TimeLabel.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
		Me.lblLow1TimeLabel.Location = New System.Drawing.Point(6, 43)
		Me.lblLow1TimeLabel.Name = "lblLow1TimeLabel"
		Me.lblLow1TimeLabel.Size = New System.Drawing.Size(165, 19)
		Me.lblLow1TimeLabel.TabIndex = 47
		Me.lblLow1TimeLabel.Text = "Operating time (Sec)"
		'
		'nudLow1Amp
		'
		Me.nudLow1Amp.DecimalPlaces = 3
		Me.nudLow1Amp.Font = New System.Drawing.Font("Meiryo UI", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.nudLow1Amp.Increment = New Decimal(New Integer() {1, 0, 0, 196608})
		Me.nudLow1Amp.Location = New System.Drawing.Point(195, 13)
		Me.nudLow1Amp.Maximum = New Decimal(New Integer() {10, 0, 0, 0})
		Me.nudLow1Amp.Name = "nudLow1Amp"
		Me.nudLow1Amp.Size = New System.Drawing.Size(98, 27)
		Me.nudLow1Amp.TabIndex = 45
		Me.nudLow1Amp.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		Me.nudLow1Amp.Value = New Decimal(New Integer() {1, 0, 0, 0})
		'
		'nudLow1Time
		'
		Me.nudLow1Time.DecimalPlaces = 1
		Me.nudLow1Time.Font = New System.Drawing.Font("Meiryo UI", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.nudLow1Time.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
		Me.nudLow1Time.Location = New System.Drawing.Point(195, 41)
		Me.nudLow1Time.Maximum = New Decimal(New Integer() {1000, 0, 0, 0})
		Me.nudLow1Time.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
		Me.nudLow1Time.Name = "nudLow1Time"
		Me.nudLow1Time.Size = New System.Drawing.Size(98, 27)
		Me.nudLow1Time.TabIndex = 45
		Me.nudLow1Time.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		Me.nudLow1Time.Value = New Decimal(New Integer() {1, 0, 0, 0})
		'
		'lblTimeDepPeriodLabel
		'
		Me.lblTimeDepPeriodLabel.AutoSize = true
		Me.lblTimeDepPeriodLabel.Font = New System.Drawing.Font("Meiryo UI", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.lblTimeDepPeriodLabel.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
		Me.lblTimeDepPeriodLabel.Location = New System.Drawing.Point(15, 389)
		Me.lblTimeDepPeriodLabel.Name = "lblTimeDepPeriodLabel"
		Me.lblTimeDepPeriodLabel.Size = New System.Drawing.Size(101, 19)
		Me.lblTimeDepPeriodLabel.TabIndex = 47
		Me.lblTimeDepPeriodLabel.Text = "Period (Sec)"
		'
		'nudTimeDepPeriod
		'
		Me.nudTimeDepPeriod.DecimalPlaces = 3
		Me.nudTimeDepPeriod.Font = New System.Drawing.Font("Meiryo UI", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.nudTimeDepPeriod.Increment = New Decimal(New Integer() {1, 0, 0, 131072})
		Me.nudTimeDepPeriod.Location = New System.Drawing.Point(204, 387)
		Me.nudTimeDepPeriod.Maximum = New Decimal(New Integer() {10, 0, 0, 0})
		Me.nudTimeDepPeriod.Minimum = New Decimal(New Integer() {1, 0, 0, 131072})
		Me.nudTimeDepPeriod.Name = "nudTimeDepPeriod"
		Me.nudTimeDepPeriod.Size = New System.Drawing.Size(98, 27)
		Me.nudTimeDepPeriod.TabIndex = 45
		Me.nudTimeDepPeriod.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		Me.nudTimeDepPeriod.Value = New Decimal(New Integer() {1, 0, 0, 0})
		'
		'timMain
		'
		'
		'GroupBox5
		'
		Me.GroupBox5.BackColor = System.Drawing.Color.Transparent
		Me.GroupBox5.Controls.Add(Me.lblTempDepSampleTimeLabel)
		Me.GroupBox5.Controls.Add(Me.nudTempDepSampleTime)
		Me.GroupBox5.Controls.Add(Me.nudTempDepAcquisitionTime)
		Me.GroupBox5.Controls.Add(Me.lblTempDepAcquisitionTime)
		Me.GroupBox5.Location = New System.Drawing.Point(12, 211)
		Me.GroupBox5.Name = "GroupBox5"
		Me.GroupBox5.Size = New System.Drawing.Size(312, 77)
		Me.GroupBox5.TabIndex = 59
		Me.GroupBox5.TabStop = false
		'
		'lblTempDepSampleTimeLabel
		'
		Me.lblTempDepSampleTimeLabel.AutoSize = true
		Me.lblTempDepSampleTimeLabel.Font = New System.Drawing.Font("Meiryo UI", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.lblTempDepSampleTimeLabel.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
		Me.lblTempDepSampleTimeLabel.Location = New System.Drawing.Point(6, 10)
		Me.lblTempDepSampleTimeLabel.Name = "lblTempDepSampleTimeLabel"
		Me.lblTempDepSampleTimeLabel.Size = New System.Drawing.Size(151, 20)
		Me.lblTempDepSampleTimeLabel.TabIndex = 46
		Me.lblTempDepSampleTimeLabel.Text = "Sample time (Sec)"
		'
		'nudTempDepSampleTime
		'
		Me.nudTempDepSampleTime.Font = New System.Drawing.Font("Meiryo UI", 14!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.nudTempDepSampleTime.Location = New System.Drawing.Point(206, 3)
		Me.nudTempDepSampleTime.Maximum = New Decimal(New Integer() {99999, 0, 0, 0})
		Me.nudTempDepSampleTime.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
		Me.nudTempDepSampleTime.Name = "nudTempDepSampleTime"
		Me.nudTempDepSampleTime.Size = New System.Drawing.Size(98, 31)
		Me.nudTempDepSampleTime.TabIndex = 45
		Me.nudTempDepSampleTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		Me.nudTempDepSampleTime.Value = New Decimal(New Integer() {10, 0, 0, 0})
		'
		'nudTempDepAcquisitionTime
		'
		Me.nudTempDepAcquisitionTime.DecimalPlaces = 1
		Me.nudTempDepAcquisitionTime.Font = New System.Drawing.Font("Meiryo UI", 14!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.nudTempDepAcquisitionTime.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
		Me.nudTempDepAcquisitionTime.Location = New System.Drawing.Point(206, 36)
		Me.nudTempDepAcquisitionTime.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
		Me.nudTempDepAcquisitionTime.Name = "nudTempDepAcquisitionTime"
		Me.nudTempDepAcquisitionTime.Size = New System.Drawing.Size(98, 31)
		Me.nudTempDepAcquisitionTime.TabIndex = 45
		Me.nudTempDepAcquisitionTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		Me.nudTempDepAcquisitionTime.Value = New Decimal(New Integer() {50, 0, 0, 0})
		'
		'lblTempDepAcquisitionTime
		'
		Me.lblTempDepAcquisitionTime.AutoSize = true
		Me.lblTempDepAcquisitionTime.Font = New System.Drawing.Font("Meiryo UI", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.lblTempDepAcquisitionTime.Location = New System.Drawing.Point(6, 41)
		Me.lblTempDepAcquisitionTime.Name = "lblTempDepAcquisitionTime"
		Me.lblTempDepAcquisitionTime.Size = New System.Drawing.Size(179, 20)
		Me.lblTempDepAcquisitionTime.TabIndex = 46
		Me.lblTempDepAcquisitionTime.Text = "Acquisition time ( % )"
		'
		'GroupBox1
		'
		Me.GroupBox1.BackColor = System.Drawing.Color.Transparent
		Me.GroupBox1.Controls.Add(Me.nudTempDepAmp)
		Me.GroupBox1.Controls.Add(Me.lblTempDepAmpLabel)
		Me.GroupBox1.Controls.Add(Me.lblTempDepStepWidthLabel)
		Me.GroupBox1.Controls.Add(Me.nudTempDepTempStep)
		Me.GroupBox1.Controls.Add(Me.nudTempDepTempEnd)
		Me.GroupBox1.Controls.Add(Me.nudTempDepTempStart)
		Me.GroupBox1.Controls.Add(Me.nudTempDepPeriod)
		Me.GroupBox1.Controls.Add(Me.lblTempDepTempEndLabel)
		Me.GroupBox1.Controls.Add(Me.lblTempDepTempStartLabel)
		Me.GroupBox1.Controls.Add(Me.lblTempDepPeriodLabel)
		Me.GroupBox1.Location = New System.Drawing.Point(12, 22)
		Me.GroupBox1.Name = "GroupBox1"
		Me.GroupBox1.Size = New System.Drawing.Size(312, 177)
		Me.GroupBox1.TabIndex = 58
		Me.GroupBox1.TabStop = false
		'
		'nudTempDepAmp
		'
		Me.nudTempDepAmp.DecimalPlaces = 3
		Me.nudTempDepAmp.Font = New System.Drawing.Font("Meiryo UI", 14!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.nudTempDepAmp.Increment = New Decimal(New Integer() {1, 0, 0, 196608})
		Me.nudTempDepAmp.Location = New System.Drawing.Point(206, 41)
		Me.nudTempDepAmp.Maximum = New Decimal(New Integer() {10, 0, 0, 0})
		Me.nudTempDepAmp.Name = "nudTempDepAmp"
		Me.nudTempDepAmp.Size = New System.Drawing.Size(98, 31)
		Me.nudTempDepAmp.TabIndex = 49
		Me.nudTempDepAmp.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		Me.nudTempDepAmp.Value = New Decimal(New Integer() {1, 0, 0, 0})
		'
		'lblTempDepAmpLabel
		'
		Me.lblTempDepAmpLabel.AutoSize = true
		Me.lblTempDepAmpLabel.Font = New System.Drawing.Font("Meiryo UI", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.lblTempDepAmpLabel.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
		Me.lblTempDepAmpLabel.Location = New System.Drawing.Point(6, 48)
		Me.lblTempDepAmpLabel.Name = "lblTempDepAmpLabel"
		Me.lblTempDepAmpLabel.Size = New System.Drawing.Size(137, 20)
		Me.lblTempDepAmpLabel.TabIndex = 50
		Me.lblTempDepAmpLabel.Text = "Amplitude (mm)"
		'
		'lblTempDepStepWidthLabel
		'
		Me.lblTempDepStepWidthLabel.AutoSize = true
		Me.lblTempDepStepWidthLabel.Font = New System.Drawing.Font("Meiryo UI", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.lblTempDepStepWidthLabel.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
		Me.lblTempDepStepWidthLabel.Location = New System.Drawing.Point(6, 146)
		Me.lblTempDepStepWidthLabel.Name = "lblTempDepStepWidthLabel"
		Me.lblTempDepStepWidthLabel.Size = New System.Drawing.Size(156, 20)
		Me.lblTempDepStepWidthLabel.TabIndex = 48
		Me.lblTempDepStepWidthLabel.Text = "Step width (deg C)"
		'
		'nudTempDepTempStep
		'
		Me.nudTempDepTempStep.DecimalPlaces = 1
		Me.nudTempDepTempStep.Font = New System.Drawing.Font("Meiryo UI", 14!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.nudTempDepTempStep.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
		Me.nudTempDepTempStep.Location = New System.Drawing.Point(206, 139)
		Me.nudTempDepTempStep.Maximum = New Decimal(New Integer() {10, 0, 0, 0})
		Me.nudTempDepTempStep.Minimum = New Decimal(New Integer() {1, 0, 0, 65536})
		Me.nudTempDepTempStep.Name = "nudTempDepTempStep"
		Me.nudTempDepTempStep.Size = New System.Drawing.Size(98, 31)
		Me.nudTempDepTempStep.TabIndex = 45
		Me.nudTempDepTempStep.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		Me.nudTempDepTempStep.Value = New Decimal(New Integer() {100, 0, 0, 65536})
		'
		'nudTempDepTempEnd
		'
		Me.nudTempDepTempEnd.DecimalPlaces = 1
		Me.nudTempDepTempEnd.Font = New System.Drawing.Font("Meiryo UI", 14!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.nudTempDepTempEnd.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
		Me.nudTempDepTempEnd.Location = New System.Drawing.Point(206, 106)
		Me.nudTempDepTempEnd.Maximum = New Decimal(New Integer() {250, 0, 0, 0})
		Me.nudTempDepTempEnd.Name = "nudTempDepTempEnd"
		Me.nudTempDepTempEnd.Size = New System.Drawing.Size(98, 31)
		Me.nudTempDepTempEnd.TabIndex = 45
		Me.nudTempDepTempEnd.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		Me.nudTempDepTempEnd.Value = New Decimal(New Integer() {200, 0, 0, 0})
		'
		'nudTempDepTempStart
		'
		Me.nudTempDepTempStart.DecimalPlaces = 1
		Me.nudTempDepTempStart.Font = New System.Drawing.Font("Meiryo UI", 14!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.nudTempDepTempStart.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
		Me.nudTempDepTempStart.Location = New System.Drawing.Point(206, 74)
		Me.nudTempDepTempStart.Maximum = New Decimal(New Integer() {250, 0, 0, 0})
		Me.nudTempDepTempStart.Name = "nudTempDepTempStart"
		Me.nudTempDepTempStart.Size = New System.Drawing.Size(98, 31)
		Me.nudTempDepTempStart.TabIndex = 45
		Me.nudTempDepTempStart.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		Me.nudTempDepTempStart.Value = New Decimal(New Integer() {30, 0, 0, 0})
		'
		'nudTempDepPeriod
		'
		Me.nudTempDepPeriod.DecimalPlaces = 3
		Me.nudTempDepPeriod.Font = New System.Drawing.Font("Meiryo UI", 14!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.nudTempDepPeriod.Increment = New Decimal(New Integer() {1, 0, 0, 131072})
		Me.nudTempDepPeriod.Location = New System.Drawing.Point(206, 8)
		Me.nudTempDepPeriod.Maximum = New Decimal(New Integer() {10, 0, 0, 0})
		Me.nudTempDepPeriod.Minimum = New Decimal(New Integer() {1, 0, 0, 131072})
		Me.nudTempDepPeriod.Name = "nudTempDepPeriod"
		Me.nudTempDepPeriod.Size = New System.Drawing.Size(98, 31)
		Me.nudTempDepPeriod.TabIndex = 45
		Me.nudTempDepPeriod.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		Me.nudTempDepPeriod.Value = New Decimal(New Integer() {1, 0, 0, 0})
		'
		'lblTempDepTempEndLabel
		'
		Me.lblTempDepTempEndLabel.AutoSize = true
		Me.lblTempDepTempEndLabel.Font = New System.Drawing.Font("Meiryo UI", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.lblTempDepTempEndLabel.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
		Me.lblTempDepTempEndLabel.Location = New System.Drawing.Point(6, 113)
		Me.lblTempDepTempEndLabel.Name = "lblTempDepTempEndLabel"
		Me.lblTempDepTempEndLabel.Size = New System.Drawing.Size(161, 20)
		Me.lblTempDepTempEndLabel.TabIndex = 48
		Me.lblTempDepTempEndLabel.Text = "Temp.  End (deg C)"
		'
		'lblTempDepTempStartLabel
		'
		Me.lblTempDepTempStartLabel.AutoSize = true
		Me.lblTempDepTempStartLabel.Font = New System.Drawing.Font("Meiryo UI", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.lblTempDepTempStartLabel.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
		Me.lblTempDepTempStartLabel.Location = New System.Drawing.Point(6, 81)
		Me.lblTempDepTempStartLabel.Name = "lblTempDepTempStartLabel"
		Me.lblTempDepTempStartLabel.Size = New System.Drawing.Size(164, 20)
		Me.lblTempDepTempStartLabel.TabIndex = 47
		Me.lblTempDepTempStartLabel.Text = "Temp. Start (deg C)"
		'
		'lblTempDepPeriodLabel
		'
		Me.lblTempDepPeriodLabel.AutoSize = true
		Me.lblTempDepPeriodLabel.Font = New System.Drawing.Font("Meiryo UI", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.lblTempDepPeriodLabel.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
		Me.lblTempDepPeriodLabel.Location = New System.Drawing.Point(6, 15)
		Me.lblTempDepPeriodLabel.Name = "lblTempDepPeriodLabel"
		Me.lblTempDepPeriodLabel.Size = New System.Drawing.Size(105, 20)
		Me.lblTempDepPeriodLabel.TabIndex = 47
		Me.lblTempDepPeriodLabel.Text = "Period (Sec)"
		'
		'ChartA
		'
		Me.ChartA.BorderlineColor = System.Drawing.Color.Black
		Me.ChartA.Location = New System.Drawing.Point(12, 49)
		Me.ChartA.Name = "ChartA"
		Me.ChartA.Size = New System.Drawing.Size(1554, 453)
		Me.ChartA.TabIndex = 59
		Me.ChartA.Text = "Chart1"
		'
		'ChartB
		'
		Me.ChartB.BorderlineColor = System.Drawing.Color.Black
		Me.ChartB.Location = New System.Drawing.Point(12, 512)
		Me.ChartB.Name = "ChartB"
		Me.ChartB.Size = New System.Drawing.Size(777, 490)
		Me.ChartB.TabIndex = 60
		Me.ChartB.Text = "Chart2"
		'
		'Panel2
		'
		Me.Panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		Me.Panel2.Controls.Add(Me.Chart2)
		Me.Panel2.Location = New System.Drawing.Point(27, 63)
		Me.Panel2.Name = "Panel2"
		Me.Panel2.Size = New System.Drawing.Size(137, 87)
		Me.Panel2.TabIndex = 61
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
		Me.Chart2.Text = "Chart3"
		'
		'Panel1
		'
		Me.Panel1.Controls.Add(Me.pctConStts)
		Me.Panel1.Controls.Add(Me.lblConnectState)
		Me.Panel1.Controls.Add(Me.lblXSelect)
		Me.Panel1.Controls.Add(Me.lblY2Select)
		Me.Panel1.Controls.Add(Me.lblY1Select)
		Me.Panel1.Controls.Add(Me.cbxXSelect)
		Me.Panel1.Controls.Add(Me.cbxY2Select)
		Me.Panel1.Controls.Add(Me.cbxY1Select)
		Me.Panel1.Location = New System.Drawing.Point(0, 0)
		Me.Panel1.Name = "Panel1"
		Me.Panel1.Size = New System.Drawing.Size(1904, 42)
		Me.Panel1.TabIndex = 62
		'
		'pctConStts
		'
		Me.pctConStts.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
		Me.pctConStts.BackColor = System.Drawing.Color.LightGray
		Me.pctConStts.BackgroundImage = CType(resources.GetObject("pctConStts.BackgroundImage"),System.Drawing.Image)
		Me.pctConStts.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
		Me.pctConStts.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.pctConStts.Location = New System.Drawing.Point(1722, 10)
		Me.pctConStts.Name = "pctConStts"
		Me.pctConStts.Size = New System.Drawing.Size(49, 24)
		Me.pctConStts.TabIndex = 46
		Me.pctConStts.TabStop = false
		Me.pctConStts.Visible = false
		'
		'lblConnectState
		'
		Me.lblConnectState.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
		Me.lblConnectState.BackColor = System.Drawing.Color.LightGray
		Me.lblConnectState.Font = New System.Drawing.Font("Meiryo UI", 14.25!, System.Drawing.FontStyle.Bold)
		Me.lblConnectState.ForeColor = System.Drawing.Color.White
		Me.lblConnectState.ImageKey = "(なし)"
		Me.lblConnectState.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.lblConnectState.Location = New System.Drawing.Point(1772, 10)
		Me.lblConnectState.Name = "lblConnectState"
		Me.lblConnectState.Size = New System.Drawing.Size(126, 24)
		Me.lblConnectState.TabIndex = 45
		Me.lblConnectState.Text = "OFFLINE"
		Me.lblConnectState.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		'
		'lblXSelect
		'
		Me.lblXSelect.AutoSize = true
		Me.lblXSelect.Font = New System.Drawing.Font("Meiryo UI", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.lblXSelect.Location = New System.Drawing.Point(547, 13)
		Me.lblXSelect.Name = "lblXSelect"
		Me.lblXSelect.Size = New System.Drawing.Size(57, 20)
		Me.lblXSelect.TabIndex = 2
		Me.lblXSelect.Text = "X Axis"
		'
		'lblY2Select
		'
		Me.lblY2Select.AutoSize = true
		Me.lblY2Select.Font = New System.Drawing.Font("Meiryo UI", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.lblY2Select.Location = New System.Drawing.Point(279, 11)
		Me.lblY2Select.Name = "lblY2Select"
		Me.lblY2Select.Size = New System.Drawing.Size(66, 20)
		Me.lblY2Select.TabIndex = 2
		Me.lblY2Select.Text = "Y2 Axis"
		'
		'lblY1Select
		'
		Me.lblY1Select.AutoSize = true
		Me.lblY1Select.Font = New System.Drawing.Font("Meiryo UI", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.lblY1Select.Location = New System.Drawing.Point(13, 13)
		Me.lblY1Select.Name = "lblY1Select"
		Me.lblY1Select.Size = New System.Drawing.Size(66, 20)
		Me.lblY1Select.TabIndex = 1
		Me.lblY1Select.Text = "Y1 Axis"
		'
		'cbxXSelect
		'
		Me.cbxXSelect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.cbxXSelect.Font = New System.Drawing.Font("Meiryo UI", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.cbxXSelect.FormattingEnabled = true
		Me.cbxXSelect.Location = New System.Drawing.Point(611, 8)
		Me.cbxXSelect.Name = "cbxXSelect"
		Me.cbxXSelect.Size = New System.Drawing.Size(169, 28)
		Me.cbxXSelect.TabIndex = 0
		'
		'cbxY2Select
		'
		Me.cbxY2Select.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.cbxY2Select.Font = New System.Drawing.Font("Meiryo UI", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.cbxY2Select.FormattingEnabled = true
		Me.cbxY2Select.Location = New System.Drawing.Point(351, 8)
		Me.cbxY2Select.Name = "cbxY2Select"
		Me.cbxY2Select.Size = New System.Drawing.Size(169, 28)
		Me.cbxY2Select.TabIndex = 0
		'
		'cbxY1Select
		'
		Me.cbxY1Select.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.cbxY1Select.Font = New System.Drawing.Font("Meiryo UI", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.cbxY1Select.FormattingEnabled = true
		Me.cbxY1Select.Location = New System.Drawing.Point(85, 8)
		Me.cbxY1Select.Name = "cbxY1Select"
		Me.cbxY1Select.Size = New System.Drawing.Size(169, 28)
		Me.cbxY1Select.TabIndex = 0
		'
		'FlowLayoutPanel1
		'
		Me.FlowLayoutPanel1.Anchor = System.Windows.Forms.AnchorStyles.None
		Me.FlowLayoutPanel1.Controls.Add(Me.lblStrgMod)
		Me.FlowLayoutPanel1.Controls.Add(Me.lblStrgModVal)
		Me.FlowLayoutPanel1.Controls.Add(Me.lblElasticMod)
		Me.FlowLayoutPanel1.Controls.Add(Me.lblElasticModVal)
		Me.FlowLayoutPanel1.Controls.Add(Me.lblShearMod)
		Me.FlowLayoutPanel1.Controls.Add(Me.lblShearModVal)
		Me.FlowLayoutPanel1.Controls.Add(Me.lblPahase)
		Me.FlowLayoutPanel1.Controls.Add(Me.lblPhaseVal)
		Me.FlowLayoutPanel1.Controls.Add(Me.lblTangent)
		Me.FlowLayoutPanel1.Controls.Add(Me.lblTangentVal)
		Me.FlowLayoutPanel1.Location = New System.Drawing.Point(5, 12)
		Me.FlowLayoutPanel1.Name = "FlowLayoutPanel1"
		Me.FlowLayoutPanel1.Size = New System.Drawing.Size(169, 317)
		Me.FlowLayoutPanel1.TabIndex = 63
		'
		'lblStrgMod
		'
		Me.lblStrgMod.AutoSize = true
		Me.lblStrgMod.Font = New System.Drawing.Font("Meiryo UI", 12!)
		Me.lblStrgMod.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.lblStrgMod.Location = New System.Drawing.Point(3, 0)
		Me.lblStrgMod.Name = "lblStrgMod"
		Me.lblStrgMod.Size = New System.Drawing.Size(108, 20)
		Me.lblStrgMod.TabIndex = 59
		Me.lblStrgMod.Text = "Storage Mod"
		'
		'lblStrgModVal
		'
		Me.lblStrgModVal.BackColor = System.Drawing.Color.White
		Me.lblStrgModVal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		Me.lblStrgModVal.Font = New System.Drawing.Font("Meiryo UI", 18!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.lblStrgModVal.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.lblStrgModVal.Location = New System.Drawing.Point(3, 20)
		Me.lblStrgModVal.Name = "lblStrgModVal"
		Me.lblStrgModVal.Size = New System.Drawing.Size(166, 42)
		Me.lblStrgModVal.TabIndex = 60
		Me.lblStrgModVal.Text = "000.0"
		Me.lblStrgModVal.TextAlign = System.Drawing.ContentAlignment.MiddleRight
		'
		'lblElasticMod
		'
		Me.lblElasticMod.AutoSize = true
		Me.lblElasticMod.Font = New System.Drawing.Font("Meiryo UI", 12!)
		Me.lblElasticMod.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.lblElasticMod.Location = New System.Drawing.Point(3, 62)
		Me.lblElasticMod.Name = "lblElasticMod"
		Me.lblElasticMod.Size = New System.Drawing.Size(96, 20)
		Me.lblElasticMod.TabIndex = 61
		Me.lblElasticMod.Text = "Elastic Mod"
		'
		'lblElasticModVal
		'
		Me.lblElasticModVal.BackColor = System.Drawing.Color.White
		Me.lblElasticModVal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		Me.lblElasticModVal.Font = New System.Drawing.Font("Meiryo UI", 18!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.lblElasticModVal.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.lblElasticModVal.Location = New System.Drawing.Point(3, 82)
		Me.lblElasticModVal.Name = "lblElasticModVal"
		Me.lblElasticModVal.Size = New System.Drawing.Size(166, 42)
		Me.lblElasticModVal.TabIndex = 62
		Me.lblElasticModVal.Text = "000.0"
		Me.lblElasticModVal.TextAlign = System.Drawing.ContentAlignment.MiddleRight
		'
		'lblShearMod
		'
		Me.lblShearMod.AutoSize = true
		Me.lblShearMod.Font = New System.Drawing.Font("Meiryo UI", 12!)
		Me.lblShearMod.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.lblShearMod.Location = New System.Drawing.Point(3, 124)
		Me.lblShearMod.Name = "lblShearMod"
		Me.lblShearMod.Size = New System.Drawing.Size(92, 20)
		Me.lblShearMod.TabIndex = 63
		Me.lblShearMod.Text = "Shear Mod"
		'
		'lblShearModVal
		'
		Me.lblShearModVal.BackColor = System.Drawing.Color.White
		Me.lblShearModVal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		Me.lblShearModVal.Font = New System.Drawing.Font("Meiryo UI", 18!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.lblShearModVal.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.lblShearModVal.Location = New System.Drawing.Point(3, 144)
		Me.lblShearModVal.Name = "lblShearModVal"
		Me.lblShearModVal.Size = New System.Drawing.Size(166, 42)
		Me.lblShearModVal.TabIndex = 64
		Me.lblShearModVal.Text = "000.0"
		Me.lblShearModVal.TextAlign = System.Drawing.ContentAlignment.MiddleRight
		'
		'lblPahase
		'
		Me.lblPahase.AutoSize = true
		Me.lblPahase.Font = New System.Drawing.Font("Meiryo UI", 12!)
		Me.lblPahase.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.lblPahase.Location = New System.Drawing.Point(3, 186)
		Me.lblPahase.Name = "lblPahase"
		Me.lblPahase.Size = New System.Drawing.Size(55, 20)
		Me.lblPahase.TabIndex = 65
		Me.lblPahase.Text = "Phase"
		'
		'lblPhaseVal
		'
		Me.lblPhaseVal.BackColor = System.Drawing.Color.White
		Me.lblPhaseVal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		Me.lblPhaseVal.Font = New System.Drawing.Font("Meiryo UI", 18!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.lblPhaseVal.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.lblPhaseVal.Location = New System.Drawing.Point(3, 206)
		Me.lblPhaseVal.Name = "lblPhaseVal"
		Me.lblPhaseVal.Size = New System.Drawing.Size(166, 42)
		Me.lblPhaseVal.TabIndex = 66
		Me.lblPhaseVal.Text = "000.0"
		Me.lblPhaseVal.TextAlign = System.Drawing.ContentAlignment.MiddleRight
		'
		'lblTangent
		'
		Me.lblTangent.AutoSize = true
		Me.lblTangent.Font = New System.Drawing.Font("Meiryo UI", 12!)
		Me.lblTangent.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.lblTangent.Location = New System.Drawing.Point(3, 248)
		Me.lblTangent.Name = "lblTangent"
		Me.lblTangent.Size = New System.Drawing.Size(71, 20)
		Me.lblTangent.TabIndex = 67
		Me.lblTangent.Text = "Tangent"
		'
		'lblTangentVal
		'
		Me.lblTangentVal.BackColor = System.Drawing.Color.White
		Me.lblTangentVal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		Me.lblTangentVal.Font = New System.Drawing.Font("Meiryo UI", 18!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.lblTangentVal.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.lblTangentVal.Location = New System.Drawing.Point(3, 268)
		Me.lblTangentVal.Name = "lblTangentVal"
		Me.lblTangentVal.Size = New System.Drawing.Size(166, 42)
		Me.lblTangentVal.TabIndex = 68
		Me.lblTangentVal.Text = "000.0"
		Me.lblTangentVal.TextAlign = System.Drawing.ContentAlignment.MiddleRight
		'
		'GroupBox6
		'
		Me.GroupBox6.Controls.Add(Me.btnScale)
		Me.GroupBox6.Controls.Add(Me.FlowLayoutPanel1)
		Me.GroupBox6.Location = New System.Drawing.Point(600, 529)
		Me.GroupBox6.Name = "GroupBox6"
		Me.GroupBox6.Size = New System.Drawing.Size(180, 465)
		Me.GroupBox6.TabIndex = 64
		Me.GroupBox6.TabStop = false
		'
		'btnScale
		'
		Me.btnScale.Font = New System.Drawing.Font("Meiryo UI", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.btnScale.Location = New System.Drawing.Point(8, 426)
		Me.btnScale.Name = "btnScale"
		Me.btnScale.Size = New System.Drawing.Size(166, 33)
		Me.btnScale.TabIndex = 64
		Me.btnScale.Text = "Scale setting"
		Me.btnScale.UseVisualStyleBackColor = true
		'
		'gbxStrgElastGScaleSetting
		'
		Me.gbxStrgElastGScaleSetting.Controls.Add(Me.btnStrgElastGScaleAply)
		Me.gbxStrgElastGScaleSetting.Controls.Add(Me.nudStgElaGYMin)
		Me.gbxStrgElastGScaleSetting.Controls.Add(Me.nudStgElaGYMax)
		Me.gbxStrgElastGScaleSetting.Controls.Add(Me.lblYMin)
		Me.gbxStrgElastGScaleSetting.Controls.Add(Me.lblYMax)
		Me.gbxStrgElastGScaleSetting.Controls.Add(Me.lblXMin)
		Me.gbxStrgElastGScaleSetting.Controls.Add(Me.lblXMax)
		Me.gbxStrgElastGScaleSetting.Controls.Add(Me.nudStgElaGXMin)
		Me.gbxStrgElastGScaleSetting.Controls.Add(Me.nudStgElaGYInterval)
		Me.gbxStrgElastGScaleSetting.Controls.Add(Me.nudStgElaGXInterval)
		Me.gbxStrgElastGScaleSetting.Controls.Add(Me.nudStgElaGXMax)
		Me.gbxStrgElastGScaleSetting.Font = New System.Drawing.Font("Meiryo UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.gbxStrgElastGScaleSetting.Location = New System.Drawing.Point(273, 700)
		Me.gbxStrgElastGScaleSetting.Name = "gbxStrgElastGScaleSetting"
		Me.gbxStrgElastGScaleSetting.Size = New System.Drawing.Size(321, 158)
		Me.gbxStrgElastGScaleSetting.TabIndex = 65
		Me.gbxStrgElastGScaleSetting.TabStop = false
		Me.gbxStrgElastGScaleSetting.Text = "Scale Setting"
		'
		'btnStrgElastGScaleAply
		'
		Me.btnStrgElastGScaleAply.Font = New System.Drawing.Font("Meiryo UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.btnStrgElastGScaleAply.Location = New System.Drawing.Point(214, 117)
		Me.btnStrgElastGScaleAply.Name = "btnStrgElastGScaleAply"
		Me.btnStrgElastGScaleAply.Size = New System.Drawing.Size(96, 35)
		Me.btnStrgElastGScaleAply.TabIndex = 3
		Me.btnStrgElastGScaleAply.Text = "&Apply"
		Me.btnStrgElastGScaleAply.UseVisualStyleBackColor = true
		'
		'nudStgElaGYMin
		'
		Me.nudStgElaGYMin.Font = New System.Drawing.Font("Meiryo UI", 9.75!)
		Me.nudStgElaGYMin.Location = New System.Drawing.Point(101, 90)
		Me.nudStgElaGYMin.Maximum = New Decimal(New Integer() {1000000, 0, 0, 0})
		Me.nudStgElaGYMin.Minimum = New Decimal(New Integer() {1000000, 0, 0, -2147483648})
		Me.nudStgElaGYMin.Name = "nudStgElaGYMin"
		Me.nudStgElaGYMin.Size = New System.Drawing.Size(96, 24)
		Me.nudStgElaGYMin.TabIndex = 2
		Me.nudStgElaGYMin.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		'
		'nudStgElaGYMax
		'
		Me.nudStgElaGYMax.Font = New System.Drawing.Font("Meiryo UI", 9.75!)
		Me.nudStgElaGYMax.Location = New System.Drawing.Point(101, 67)
		Me.nudStgElaGYMax.Maximum = New Decimal(New Integer() {1000000, 0, 0, 0})
		Me.nudStgElaGYMax.Minimum = New Decimal(New Integer() {1000000, 0, 0, -2147483648})
		Me.nudStgElaGYMax.Name = "nudStgElaGYMax"
		Me.nudStgElaGYMax.Size = New System.Drawing.Size(96, 24)
		Me.nudStgElaGYMax.TabIndex = 2
		Me.nudStgElaGYMax.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		'
		'lblYMin
		'
		Me.lblYMin.AutoSize = true
		Me.lblYMin.Font = New System.Drawing.Font("Meiryo UI", 9.75!)
		Me.lblYMin.Location = New System.Drawing.Point(18, 94)
		Me.lblYMin.Name = "lblYMin"
		Me.lblYMin.Size = New System.Drawing.Size(71, 17)
		Me.lblYMin.TabIndex = 1
		Me.lblYMin.Text = "Y Axis Min"
		'
		'lblYMax
		'
		Me.lblYMax.AutoSize = true
		Me.lblYMax.Font = New System.Drawing.Font("Meiryo UI", 9.75!)
		Me.lblYMax.Location = New System.Drawing.Point(18, 70)
		Me.lblYMax.Name = "lblYMax"
		Me.lblYMax.Size = New System.Drawing.Size(75, 17)
		Me.lblYMax.TabIndex = 1
		Me.lblYMax.Text = "Y Axis Max"
		'
		'lblXMin
		'
		Me.lblXMin.AutoSize = true
		Me.lblXMin.Font = New System.Drawing.Font("Meiryo UI", 9.75!)
		Me.lblXMin.Location = New System.Drawing.Point(18, 41)
		Me.lblXMin.Name = "lblXMin"
		Me.lblXMin.Size = New System.Drawing.Size(72, 17)
		Me.lblXMin.TabIndex = 1
		Me.lblXMin.Text = "X Axis Min"
		'
		'lblXMax
		'
		Me.lblXMax.AutoSize = true
		Me.lblXMax.Font = New System.Drawing.Font("Meiryo UI", 9.75!)
		Me.lblXMax.Location = New System.Drawing.Point(18, 19)
		Me.lblXMax.Name = "lblXMax"
		Me.lblXMax.Size = New System.Drawing.Size(76, 17)
		Me.lblXMax.TabIndex = 1
		Me.lblXMax.Text = "X Axis Max"
		'
		'nudStgElaGXMin
		'
		Me.nudStgElaGXMin.Font = New System.Drawing.Font("Meiryo UI", 9.75!)
		Me.nudStgElaGXMin.Location = New System.Drawing.Point(101, 38)
		Me.nudStgElaGXMin.Maximum = New Decimal(New Integer() {1000000, 0, 0, 0})
		Me.nudStgElaGXMin.Minimum = New Decimal(New Integer() {1000000, 0, 0, -2147483648})
		Me.nudStgElaGXMin.Name = "nudStgElaGXMin"
		Me.nudStgElaGXMin.Size = New System.Drawing.Size(96, 24)
		Me.nudStgElaGXMin.TabIndex = 0
		Me.nudStgElaGXMin.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		'
		'nudStgElaGYInterval
		'
		Me.nudStgElaGYInterval.DecimalPlaces = 1
		Me.nudStgElaGYInterval.Font = New System.Drawing.Font("Meiryo UI", 9.75!)
		Me.nudStgElaGYInterval.Location = New System.Drawing.Point(214, 87)
		Me.nudStgElaGYInterval.Maximum = New Decimal(New Integer() {1000000, 0, 0, 0})
		Me.nudStgElaGYInterval.Minimum = New Decimal(New Integer() {1, 0, 0, 65536})
		Me.nudStgElaGYInterval.Name = "nudStgElaGYInterval"
		Me.nudStgElaGYInterval.Size = New System.Drawing.Size(96, 24)
		Me.nudStgElaGYInterval.TabIndex = 0
		Me.nudStgElaGYInterval.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		Me.nudStgElaGYInterval.Value = New Decimal(New Integer() {100, 0, 0, 0})
		'
		'nudStgElaGXInterval
		'
		Me.nudStgElaGXInterval.DecimalPlaces = 1
		Me.nudStgElaGXInterval.Font = New System.Drawing.Font("Meiryo UI", 9.75!)
		Me.nudStgElaGXInterval.Location = New System.Drawing.Point(214, 38)
		Me.nudStgElaGXInterval.Maximum = New Decimal(New Integer() {1000000, 0, 0, 0})
		Me.nudStgElaGXInterval.Minimum = New Decimal(New Integer() {1, 0, 0, 65536})
		Me.nudStgElaGXInterval.Name = "nudStgElaGXInterval"
		Me.nudStgElaGXInterval.Size = New System.Drawing.Size(96, 24)
		Me.nudStgElaGXInterval.TabIndex = 0
		Me.nudStgElaGXInterval.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		Me.nudStgElaGXInterval.Value = New Decimal(New Integer() {100, 0, 0, 0})
		'
		'nudStgElaGXMax
		'
		Me.nudStgElaGXMax.Font = New System.Drawing.Font("Meiryo UI", 9.75!)
		Me.nudStgElaGXMax.Location = New System.Drawing.Point(101, 16)
		Me.nudStgElaGXMax.Maximum = New Decimal(New Integer() {1000000, 0, 0, 0})
		Me.nudStgElaGXMax.Minimum = New Decimal(New Integer() {1000000, 0, 0, -2147483648})
		Me.nudStgElaGXMax.Name = "nudStgElaGXMax"
		Me.nudStgElaGXMax.Size = New System.Drawing.Size(96, 24)
		Me.nudStgElaGXMax.TabIndex = 0
		Me.nudStgElaGXMax.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		'
		'ChartC
		'
		Me.ChartC.BorderlineColor = System.Drawing.Color.Black
		Me.ChartC.Location = New System.Drawing.Point(795, 512)
		Me.ChartC.Name = "ChartC"
		Me.ChartC.Size = New System.Drawing.Size(771, 490)
		Me.ChartC.TabIndex = 66
		Me.ChartC.Text = "Chart2"
		'
		'gbxConditionSetSpeed
		'
		Me.gbxConditionSetSpeed.BackColor = System.Drawing.Color.LightYellow
		Me.gbxConditionSetSpeed.Controls.Add(Me.btnCloseSpeed)
		Me.gbxConditionSetSpeed.Controls.Add(Me.GroupBox3)
		Me.gbxConditionSetSpeed.Controls.Add(Me.gbxSpeedDep)
		Me.gbxConditionSetSpeed.Font = New System.Drawing.Font("Meiryo UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.gbxConditionSetSpeed.Location = New System.Drawing.Point(831, 65)
		Me.gbxConditionSetSpeed.Name = "gbxConditionSetSpeed"
		Me.gbxConditionSetSpeed.Size = New System.Drawing.Size(330, 299)
		Me.gbxConditionSetSpeed.TabIndex = 67
		Me.gbxConditionSetSpeed.TabStop = false
		Me.gbxConditionSetSpeed.Text = "Speed Condition setting"
		Me.gbxConditionSetSpeed.Visible = false
		'
		'btnCloseSpeed
		'
		Me.btnCloseSpeed.Font = New System.Drawing.Font("Meiryo UI", 9!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.btnCloseSpeed.Location = New System.Drawing.Point(300, 5)
		Me.btnCloseSpeed.Name = "btnCloseSpeed"
		Me.btnCloseSpeed.Size = New System.Drawing.Size(24, 24)
		Me.btnCloseSpeed.TabIndex = 71
		Me.btnCloseSpeed.Text = "X"
		Me.btnCloseSpeed.UseVisualStyleBackColor = true
		'
		'gbxConditionSetTemp
		'
		Me.gbxConditionSetTemp.BackColor = System.Drawing.Color.PeachPuff
		Me.gbxConditionSetTemp.Controls.Add(Me.btnCloseTemp)
		Me.gbxConditionSetTemp.Controls.Add(Me.GroupBox5)
		Me.gbxConditionSetTemp.Controls.Add(Me.GroupBox1)
		Me.gbxConditionSetTemp.Font = New System.Drawing.Font("Meiryo UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.gbxConditionSetTemp.Location = New System.Drawing.Point(495, 65)
		Me.gbxConditionSetTemp.Name = "gbxConditionSetTemp"
		Me.gbxConditionSetTemp.Size = New System.Drawing.Size(330, 299)
		Me.gbxConditionSetTemp.TabIndex = 67
		Me.gbxConditionSetTemp.TabStop = false
		Me.gbxConditionSetTemp.Text = "Temperature Condition setting"
		Me.gbxConditionSetTemp.Visible = false
		'
		'btnCloseTemp
		'
		Me.btnCloseTemp.Font = New System.Drawing.Font("Meiryo UI", 9!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.btnCloseTemp.Location = New System.Drawing.Point(300, 5)
		Me.btnCloseTemp.Name = "btnCloseTemp"
		Me.btnCloseTemp.Size = New System.Drawing.Size(24, 24)
		Me.btnCloseTemp.TabIndex = 71
		Me.btnCloseTemp.Text = "X"
		Me.btnCloseTemp.UseVisualStyleBackColor = true
		'
		'gbxConditionSetTime
		'
		Me.gbxConditionSetTime.BackColor = System.Drawing.Color.LightCyan
		Me.gbxConditionSetTime.Controls.Add(Me.btnCloseTime)
		Me.gbxConditionSetTime.Controls.Add(Me.lblTimeDepPeriodLabel)
		Me.gbxConditionSetTime.Controls.Add(Me.nudThinning)
		Me.gbxConditionSetTime.Controls.Add(Me.nudTimeDepPeriod)
		Me.gbxConditionSetTime.Controls.Add(Me.lblThinningLabel)
		Me.gbxConditionSetTime.Controls.Add(Me.gbxLowShear2)
		Me.gbxConditionSetTime.Controls.Add(Me.gbxHighShear)
		Me.gbxConditionSetTime.Controls.Add(Me.gbxRecovery)
		Me.gbxConditionSetTime.Controls.Add(Me.gbxPreShear)
		Me.gbxConditionSetTime.Controls.Add(Me.gbxLowShear1)
		Me.gbxConditionSetTime.Font = New System.Drawing.Font("Meiryo UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.gbxConditionSetTime.Location = New System.Drawing.Point(1242, 508)
		Me.gbxConditionSetTime.Name = "gbxConditionSetTime"
		Me.gbxConditionSetTime.Size = New System.Drawing.Size(313, 461)
		Me.gbxConditionSetTime.TabIndex = 68
		Me.gbxConditionSetTime.TabStop = false
		Me.gbxConditionSetTime.Text = "Time Condition setting"
		Me.gbxConditionSetTime.Visible = false
		'
		'btnCloseTime
		'
		Me.btnCloseTime.Font = New System.Drawing.Font("Meiryo UI", 9!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.btnCloseTime.Location = New System.Drawing.Point(281, 3)
		Me.btnCloseTime.Name = "btnCloseTime"
		Me.btnCloseTime.Size = New System.Drawing.Size(24, 24)
		Me.btnCloseTime.TabIndex = 71
		Me.btnCloseTime.Text = "X"
		Me.btnCloseTime.UseVisualStyleBackColor = true
		'
		'nudThinning
		'
		Me.nudThinning.Font = New System.Drawing.Font("Meiryo UI", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.nudThinning.Location = New System.Drawing.Point(204, 420)
		Me.nudThinning.Maximum = New Decimal(New Integer() {1000, 0, 0, 0})
		Me.nudThinning.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
		Me.nudThinning.Name = "nudThinning"
		Me.nudThinning.Size = New System.Drawing.Size(98, 27)
		Me.nudThinning.TabIndex = 70
		Me.nudThinning.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		Me.nudThinning.Value = New Decimal(New Integer() {100, 0, 0, 0})
		'
		'lblThinningLabel
		'
		Me.lblThinningLabel.AutoSize = true
		Me.lblThinningLabel.Font = New System.Drawing.Font("Meiryo UI", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.lblThinningLabel.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
		Me.lblThinningLabel.Location = New System.Drawing.Point(15, 422)
		Me.lblThinningLabel.Name = "lblThinningLabel"
		Me.lblThinningLabel.Size = New System.Drawing.Size(124, 19)
		Me.lblThinningLabel.TabIndex = 69
		Me.lblThinningLabel.Text = "Thinning(Num.)"
		'
		'gbxRecovery
		'
		Me.gbxRecovery.Controls.Add(Me.lblRecoveryAmpLabel)
		Me.gbxRecovery.Controls.Add(Me.lblRecoveryTimeLabel)
		Me.gbxRecovery.Controls.Add(Me.nudRecoveryAmp)
		Me.gbxRecovery.Controls.Add(Me.nudRecoveryTime)
		Me.gbxRecovery.Font = New System.Drawing.Font("Meiryo UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.gbxRecovery.ForeColor = System.Drawing.Color.Blue
		Me.gbxRecovery.Location = New System.Drawing.Point(9, 95)
		Me.gbxRecovery.Name = "gbxRecovery"
		Me.gbxRecovery.Size = New System.Drawing.Size(293, 72)
		Me.gbxRecovery.TabIndex = 68
		Me.gbxRecovery.TabStop = false
		Me.gbxRecovery.Text = "Recovery"
		'
		'lblRecoveryAmpLabel
		'
		Me.lblRecoveryAmpLabel.AutoSize = true
		Me.lblRecoveryAmpLabel.Font = New System.Drawing.Font("Meiryo UI", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.lblRecoveryAmpLabel.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
		Me.lblRecoveryAmpLabel.Location = New System.Drawing.Point(6, 20)
		Me.lblRecoveryAmpLabel.Name = "lblRecoveryAmpLabel"
		Me.lblRecoveryAmpLabel.Size = New System.Drawing.Size(130, 19)
		Me.lblRecoveryAmpLabel.TabIndex = 47
		Me.lblRecoveryAmpLabel.Text = "Amplitude (mm)"
		'
		'lblRecoveryTimeLabel
		'
		Me.lblRecoveryTimeLabel.AutoSize = true
		Me.lblRecoveryTimeLabel.Font = New System.Drawing.Font("Meiryo UI", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.lblRecoveryTimeLabel.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
		Me.lblRecoveryTimeLabel.Location = New System.Drawing.Point(6, 43)
		Me.lblRecoveryTimeLabel.Name = "lblRecoveryTimeLabel"
		Me.lblRecoveryTimeLabel.Size = New System.Drawing.Size(165, 19)
		Me.lblRecoveryTimeLabel.TabIndex = 47
		Me.lblRecoveryTimeLabel.Text = "Operating time (Sec)"
		'
		'nudRecoveryAmp
		'
		Me.nudRecoveryAmp.DecimalPlaces = 3
		Me.nudRecoveryAmp.Font = New System.Drawing.Font("Meiryo UI", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.nudRecoveryAmp.Increment = New Decimal(New Integer() {1, 0, 0, 196608})
		Me.nudRecoveryAmp.Location = New System.Drawing.Point(195, 13)
		Me.nudRecoveryAmp.Maximum = New Decimal(New Integer() {10, 0, 0, 0})
		Me.nudRecoveryAmp.Name = "nudRecoveryAmp"
		Me.nudRecoveryAmp.Size = New System.Drawing.Size(98, 27)
		Me.nudRecoveryAmp.TabIndex = 45
		Me.nudRecoveryAmp.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		Me.nudRecoveryAmp.Value = New Decimal(New Integer() {1, 0, 0, 0})
		'
		'nudRecoveryTime
		'
		Me.nudRecoveryTime.DecimalPlaces = 1
		Me.nudRecoveryTime.Font = New System.Drawing.Font("Meiryo UI", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.nudRecoveryTime.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
		Me.nudRecoveryTime.Location = New System.Drawing.Point(195, 41)
		Me.nudRecoveryTime.Maximum = New Decimal(New Integer() {1000, 0, 0, 0})
		Me.nudRecoveryTime.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
		Me.nudRecoveryTime.Name = "nudRecoveryTime"
		Me.nudRecoveryTime.Size = New System.Drawing.Size(98, 27)
		Me.nudRecoveryTime.TabIndex = 45
		Me.nudRecoveryTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		Me.nudRecoveryTime.Value = New Decimal(New Integer() {1, 0, 0, 0})
		'
		'gbxPreShear
		'
		Me.gbxPreShear.Controls.Add(Me.lblPreShearAmpLabel)
		Me.gbxPreShear.Controls.Add(Me.lblPreShearTimeLabel)
		Me.gbxPreShear.Controls.Add(Me.nudPreShearAmp)
		Me.gbxPreShear.Controls.Add(Me.nudPreShearTime)
		Me.gbxPreShear.Font = New System.Drawing.Font("Meiryo UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.gbxPreShear.ForeColor = System.Drawing.Color.Blue
		Me.gbxPreShear.Location = New System.Drawing.Point(9, 28)
		Me.gbxPreShear.Name = "gbxPreShear"
		Me.gbxPreShear.Size = New System.Drawing.Size(293, 72)
		Me.gbxPreShear.TabIndex = 68
		Me.gbxPreShear.TabStop = false
		Me.gbxPreShear.Text = "PreShear"
		'
		'lblPreShearAmpLabel
		'
		Me.lblPreShearAmpLabel.AutoSize = true
		Me.lblPreShearAmpLabel.Font = New System.Drawing.Font("Meiryo UI", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.lblPreShearAmpLabel.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
		Me.lblPreShearAmpLabel.Location = New System.Drawing.Point(6, 20)
		Me.lblPreShearAmpLabel.Name = "lblPreShearAmpLabel"
		Me.lblPreShearAmpLabel.Size = New System.Drawing.Size(130, 19)
		Me.lblPreShearAmpLabel.TabIndex = 47
		Me.lblPreShearAmpLabel.Text = "Amplitude (mm)"
		'
		'lblPreShearTimeLabel
		'
		Me.lblPreShearTimeLabel.AutoSize = true
		Me.lblPreShearTimeLabel.Font = New System.Drawing.Font("Meiryo UI", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.lblPreShearTimeLabel.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
		Me.lblPreShearTimeLabel.Location = New System.Drawing.Point(6, 43)
		Me.lblPreShearTimeLabel.Name = "lblPreShearTimeLabel"
		Me.lblPreShearTimeLabel.Size = New System.Drawing.Size(165, 19)
		Me.lblPreShearTimeLabel.TabIndex = 47
		Me.lblPreShearTimeLabel.Text = "Operating time (Sec)"
		'
		'nudPreShearAmp
		'
		Me.nudPreShearAmp.DecimalPlaces = 3
		Me.nudPreShearAmp.Font = New System.Drawing.Font("Meiryo UI", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.nudPreShearAmp.Increment = New Decimal(New Integer() {1, 0, 0, 196608})
		Me.nudPreShearAmp.Location = New System.Drawing.Point(195, 13)
		Me.nudPreShearAmp.Maximum = New Decimal(New Integer() {10, 0, 0, 0})
		Me.nudPreShearAmp.Name = "nudPreShearAmp"
		Me.nudPreShearAmp.Size = New System.Drawing.Size(98, 27)
		Me.nudPreShearAmp.TabIndex = 45
		Me.nudPreShearAmp.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		Me.nudPreShearAmp.Value = New Decimal(New Integer() {1, 0, 0, 0})
		'
		'nudPreShearTime
		'
		Me.nudPreShearTime.DecimalPlaces = 1
		Me.nudPreShearTime.Font = New System.Drawing.Font("Meiryo UI", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.nudPreShearTime.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
		Me.nudPreShearTime.Location = New System.Drawing.Point(195, 41)
		Me.nudPreShearTime.Maximum = New Decimal(New Integer() {1000, 0, 0, 0})
		Me.nudPreShearTime.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
		Me.nudPreShearTime.Name = "nudPreShearTime"
		Me.nudPreShearTime.Size = New System.Drawing.Size(98, 27)
		Me.nudPreShearTime.TabIndex = 45
		Me.nudPreShearTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		Me.nudPreShearTime.Value = New Decimal(New Integer() {1, 0, 0, 0})
		'
		'gbxMeasMode
		'
		Me.gbxMeasMode.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
		Me.gbxMeasMode.Controls.Add(Me.btnTemp)
		Me.gbxMeasMode.Controls.Add(Me.btnTime)
		Me.gbxMeasMode.Controls.Add(Me.btnSpeed)
		Me.gbxMeasMode.Font = New System.Drawing.Font("MS UI Gothic", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.gbxMeasMode.Location = New System.Drawing.Point(1590, 437)
		Me.gbxMeasMode.Name = "gbxMeasMode"
		Me.gbxMeasMode.Size = New System.Drawing.Size(308, 65)
		Me.gbxMeasMode.TabIndex = 69
		Me.gbxMeasMode.TabStop = false
		Me.gbxMeasMode.Text = "Measurement Mode"
		'
		'btnTemp
		'
		Me.btnTemp.Font = New System.Drawing.Font("Meiryo UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.btnTemp.Location = New System.Drawing.Point(199, 20)
		Me.btnTemp.Name = "btnTemp"
		Me.btnTemp.Size = New System.Drawing.Size(109, 32)
		Me.btnTemp.TabIndex = 0
		Me.btnTemp.Text = "Temperature"
		Me.btnTemp.UseVisualStyleBackColor = true
		'
		'btnTime
		'
		Me.btnTime.Font = New System.Drawing.Font("Meiryo UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.btnTime.Location = New System.Drawing.Point(101, 20)
		Me.btnTime.Name = "btnTime"
		Me.btnTime.Size = New System.Drawing.Size(94, 32)
		Me.btnTime.TabIndex = 0
		Me.btnTime.Text = "Time"
		Me.btnTime.UseVisualStyleBackColor = true
		'
		'btnSpeed
		'
		Me.btnSpeed.Font = New System.Drawing.Font("Meiryo UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.btnSpeed.Location = New System.Drawing.Point(3, 20)
		Me.btnSpeed.Name = "btnSpeed"
		Me.btnSpeed.Size = New System.Drawing.Size(94, 32)
		Me.btnSpeed.TabIndex = 0
		Me.btnSpeed.Text = "Speed"
		Me.btnSpeed.UseVisualStyleBackColor = true
		'
		'frmViscosity
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6!, 12!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(1904, 1041)
		Me.Controls.Add(Me.gbxMeasMode)
		Me.Controls.Add(Me.gbxConditionSetTime)
		Me.Controls.Add(Me.gbxConditionSetTemp)
		Me.Controls.Add(Me.gbxConditionSetSpeed)
		Me.Controls.Add(Me.Panel2)
		Me.Controls.Add(Me.gbxStrgElastGScaleSetting)
		Me.Controls.Add(Me.GroupBox6)
		Me.Controls.Add(Me.Panel1)
		Me.Controls.Add(Me.dgvCurrVal)
		Me.Controls.Add(Me.StatusStrip1)
		Me.Controls.Add(Me.GroupBox4)
		Me.Controls.Add(Me.GroupBox2)
		Me.Controls.Add(Me.ChartA)
		Me.Controls.Add(Me.ChartB)
		Me.Controls.Add(Me.ChartC)
		Me.KeyPreview = true
		Me.MaximizeBox = false
		Me.MinimizeBox = false
		Me.Name = "frmViscosity"
		Me.Text = "frmViscosity"
		CType(Me.dgvCurrVal,System.ComponentModel.ISupportInitialize).EndInit
		Me.GroupBox2.ResumeLayout(false)
		Me.GroupBox4.ResumeLayout(false)
		Me.GroupBox4.PerformLayout
		CType(Me.nudPreTemp,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.nudInputGap,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.nudXYWait,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.nudProbedDiameter,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.nudRisingHPWait,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.nudMeasureGap,System.ComponentModel.ISupportInitialize).EndInit
		Me.GroupBox3.ResumeLayout(false)
		Me.GroupBox3.PerformLayout
		CType(Me.nudSpeedDepSampleTime,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.nudSpeedDepAcquisitionTime,System.ComponentModel.ISupportInitialize).EndInit
		Me.StatusStrip1.ResumeLayout(false)
		Me.StatusStrip1.PerformLayout
		Me.gbxSpeedDep.ResumeLayout(false)
		Me.gbxSpeedDep.PerformLayout
		CType(Me.nudSpeedDepAmpStep,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.nudSpeedDepAmpEnd,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.nudSpeedDepAmpStart,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.nudSpeedDepPeriod,System.ComponentModel.ISupportInitialize).EndInit
		Me.gbxHighShear.ResumeLayout(false)
		Me.gbxHighShear.PerformLayout
		CType(Me.nudHighAmp,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.nudHighTime,System.ComponentModel.ISupportInitialize).EndInit
		Me.gbxLowShear2.ResumeLayout(false)
		Me.gbxLowShear2.PerformLayout
		CType(Me.nudLow2Amp,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.nudLow2Time,System.ComponentModel.ISupportInitialize).EndInit
		Me.gbxLowShear1.ResumeLayout(false)
		Me.gbxLowShear1.PerformLayout
		CType(Me.nudLow1Amp,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.nudLow1Time,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.nudTimeDepPeriod,System.ComponentModel.ISupportInitialize).EndInit
		Me.GroupBox5.ResumeLayout(false)
		Me.GroupBox5.PerformLayout
		CType(Me.nudTempDepSampleTime,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.nudTempDepAcquisitionTime,System.ComponentModel.ISupportInitialize).EndInit
		Me.GroupBox1.ResumeLayout(false)
		Me.GroupBox1.PerformLayout
		CType(Me.nudTempDepAmp,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.nudTempDepTempStep,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.nudTempDepTempEnd,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.nudTempDepTempStart,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.nudTempDepPeriod,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.ChartA,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.ChartB,System.ComponentModel.ISupportInitialize).EndInit
		Me.Panel2.ResumeLayout(false)
		CType(Me.Chart2,System.ComponentModel.ISupportInitialize).EndInit
		Me.Panel1.ResumeLayout(false)
		Me.Panel1.PerformLayout
		CType(Me.pctConStts,System.ComponentModel.ISupportInitialize).EndInit
		Me.FlowLayoutPanel1.ResumeLayout(false)
		Me.FlowLayoutPanel1.PerformLayout
		Me.GroupBox6.ResumeLayout(false)
		Me.gbxStrgElastGScaleSetting.ResumeLayout(false)
		Me.gbxStrgElastGScaleSetting.PerformLayout
		CType(Me.nudStgElaGYMin,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.nudStgElaGYMax,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.nudStgElaGXMin,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.nudStgElaGYInterval,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.nudStgElaGXInterval,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.nudStgElaGXMax,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.ChartC,System.ComponentModel.ISupportInitialize).EndInit
		Me.gbxConditionSetSpeed.ResumeLayout(false)
		Me.gbxConditionSetTemp.ResumeLayout(false)
		Me.gbxConditionSetTime.ResumeLayout(false)
		Me.gbxConditionSetTime.PerformLayout
		CType(Me.nudThinning,System.ComponentModel.ISupportInitialize).EndInit
		Me.gbxRecovery.ResumeLayout(false)
		Me.gbxRecovery.PerformLayout
		CType(Me.nudRecoveryAmp,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.nudRecoveryTime,System.ComponentModel.ISupportInitialize).EndInit
		Me.gbxPreShear.ResumeLayout(false)
		Me.gbxPreShear.PerformLayout
		CType(Me.nudPreShearAmp,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.nudPreShearTime,System.ComponentModel.ISupportInitialize).EndInit
		Me.gbxMeasMode.ResumeLayout(false)
		Me.ResumeLayout(false)
		Me.PerformLayout

End Sub

	Friend WithEvents dgvCurrVal As DataGridView
	Friend WithEvents Column1 As DataGridViewTextBoxColumn
	Friend WithEvents Column2 As DataGridViewTextBoxColumn
	Friend WithEvents GroupBox2 As GroupBox
	Friend WithEvents btnClose As Button
	Friend WithEvents btnTemperature As Button
	Friend WithEvents btnOrigin As Button
	Friend WithEvents btnZeroAdj As Button
	Friend WithEvents btnExec As Button
	Friend WithEvents btnStop As Button
	Friend WithEvents GroupBox4 As GroupBox
	Friend WithEvents nudPreTemp As NumericUpDown
	Friend WithEvents lblInputGap As Label
	Friend WithEvents nudInputGap As NumericUpDown
	Friend WithEvents nudXYWait As NumericUpDown
	Friend WithEvents nudRisingHPWait As NumericUpDown
	Friend WithEvents lblPresetTemp As Label
	Friend WithEvents lblHPUpWait As Label
	Friend WithEvents lblXYWait As Label
	Friend WithEvents nudMeasureGap As NumericUpDown
	Friend WithEvents lblMeasureGap As Label
	Friend WithEvents GroupBox3 As GroupBox
	Friend WithEvents lblSpeedDepSampleTime As Label
	Friend WithEvents nudSpeedDepSampleTime As NumericUpDown
	Friend WithEvents nudSpeedDepAcquisitionTime As NumericUpDown
	Friend WithEvents lblSpeedDepAcquisitionTime As Label
	Friend WithEvents StatusStrip1 As StatusStrip
	Friend WithEvents lblMsg As ToolStripStatusLabel
	Friend WithEvents gbxSpeedDep As GroupBox
	Friend WithEvents lblSpeedDepStepWidthLabel As Label
	Friend WithEvents lblSpeedDepAmpEndLabel As Label
	Friend WithEvents lblSpeedDepAmpStartLabel As Label
	Friend WithEvents lblSpeedDepPeriodLabel As Label
	Friend WithEvents timMain As Timer
	Friend WithEvents nudSpeedDepAmpStep As NumericUpDown
	Friend WithEvents nudSpeedDepAmpEnd As NumericUpDown
	Friend WithEvents nudSpeedDepAmpStart As NumericUpDown
	Friend WithEvents nudSpeedDepPeriod As NumericUpDown
	Friend WithEvents lblProbedDiameterLabel As Label
	Friend WithEvents nudProbedDiameter As NumericUpDown
	Friend WithEvents ChartA As DataVisualization.Charting.Chart
	Friend WithEvents ChartB As DataVisualization.Charting.Chart
	Friend WithEvents Panel2 As Panel
	Friend WithEvents Chart2 As DataVisualization.Charting.Chart
	Friend WithEvents ckbFlcvReverse As CheckBox
	Friend WithEvents GroupBox5 As GroupBox
	Friend WithEvents lblTempDepSampleTimeLabel As Label
	Friend WithEvents nudTempDepSampleTime As NumericUpDown
	Friend WithEvents nudTempDepAcquisitionTime As NumericUpDown
	Friend WithEvents lblTempDepAcquisitionTime As Label
	Friend WithEvents GroupBox1 As GroupBox
	Friend WithEvents nudTempDepAmp As NumericUpDown
	Friend WithEvents lblTempDepAmpLabel As Label
	Friend WithEvents lblTempDepStepWidthLabel As Label
	Friend WithEvents nudTempDepTempStep As NumericUpDown
	Friend WithEvents nudTempDepTempEnd As NumericUpDown
	Friend WithEvents nudTempDepTempStart As NumericUpDown
	Friend WithEvents nudTempDepPeriod As NumericUpDown
	Friend WithEvents lblTempDepTempEndLabel As Label
	Friend WithEvents lblTempDepTempStartLabel As Label
	Friend WithEvents lblTempDepPeriodLabel As Label
    Friend WithEvents rbXAction As RadioButton
    Friend WithEvents rbCircleAction As RadioButton
    Friend WithEvents Panel1 As Panel
    Friend WithEvents lblXSelect As Label
    Friend WithEvents lblY2Select As Label
    Friend WithEvents lblY1Select As Label
    Friend WithEvents cbxXSelect As ComboBox
    Friend WithEvents cbxY2Select As ComboBox
    Friend WithEvents cbxY1Select As ComboBox
    Friend WithEvents FlowLayoutPanel1 As FlowLayoutPanel
    Friend WithEvents lblStrgMod As Label
    Friend WithEvents lblStrgModVal As Label
    Friend WithEvents lblElasticMod As Label
    Friend WithEvents lblElasticModVal As Label
    Friend WithEvents lblShearMod As Label
    Friend WithEvents lblShearModVal As Label
    Friend WithEvents lblPahase As Label
    Friend WithEvents lblPhaseVal As Label
    Friend WithEvents lblTangent As Label
    Friend WithEvents lblTangentVal As Label
    Friend WithEvents GroupBox6 As GroupBox
    Friend WithEvents btnScale As Button
    Friend WithEvents gbxStrgElastGScaleSetting As GroupBox
    Friend WithEvents btnStrgElastGScaleAply As Button
    Friend WithEvents nudStgElaGYMin As NumericUpDown
    Friend WithEvents nudStgElaGYMax As NumericUpDown
    Friend WithEvents lblYMin As Label
    Friend WithEvents lblYMax As Label
    Friend WithEvents lblXMin As Label
    Friend WithEvents lblXMax As Label
    Friend WithEvents nudStgElaGXMin As NumericUpDown
    Friend WithEvents nudStgElaGXMax As NumericUpDown
    Friend WithEvents nudStgElaGYInterval As NumericUpDown
    Friend WithEvents nudStgElaGXInterval As NumericUpDown
    Friend WithEvents ChartC As DataVisualization.Charting.Chart
	Friend WithEvents pctConStts As PictureBox
	Friend WithEvents lblConnectState As Label
	Friend WithEvents gbxLowShear1 As GroupBox
	Friend WithEvents lblLow1AmpLabel As Label
	Friend WithEvents nudLow1Amp As NumericUpDown
	Friend WithEvents nudLow1Time As NumericUpDown
	Friend WithEvents lblLow1TimeLabel As Label
	Friend WithEvents gbxHighShear As GroupBox
	Friend WithEvents lblHighAmpLabel As Label
	Friend WithEvents lblHighTimeLabel As Label
	Friend WithEvents nudHighAmp As NumericUpDown
	Friend WithEvents nudHighTime As NumericUpDown
	Friend WithEvents gbxLowShear2 As GroupBox
	Friend WithEvents lblLow2AmpLabel As Label
	Friend WithEvents lblLow2TimeLabel As Label
	Friend WithEvents nudLow2Amp As NumericUpDown
	Friend WithEvents nudLow2Time As NumericUpDown
	Friend WithEvents lblTimeDepPeriodLabel As Label
	Friend WithEvents nudTimeDepPeriod As NumericUpDown
	Friend WithEvents gbxConditionSetSpeed As GroupBox
	Friend WithEvents gbxConditionSetTemp As GroupBox
	Friend WithEvents gbxConditionSetTime As GroupBox
	Friend WithEvents gbxRecovery As GroupBox
	Friend WithEvents lblRecoveryAmpLabel As Label
	Friend WithEvents lblRecoveryTimeLabel As Label
	Friend WithEvents nudRecoveryAmp As NumericUpDown
	Friend WithEvents nudRecoveryTime As NumericUpDown
	Friend WithEvents gbxPreShear As GroupBox
	Friend WithEvents lblPreShearAmpLabel As Label
	Friend WithEvents lblPreShearTimeLabel As Label
	Friend WithEvents nudPreShearAmp As NumericUpDown
	Friend WithEvents nudPreShearTime As NumericUpDown
	Friend WithEvents nudThinning As NumericUpDown
	Friend WithEvents lblThinningLabel As Label
	Friend WithEvents btnCloseTime As Button
	Friend WithEvents btnCloseSpeed As Button
	Friend WithEvents btnCloseTemp As Button
	Friend WithEvents gbxMeasMode As GroupBox
	Friend WithEvents btnSpeed As Button
	Friend WithEvents btnTemp As Button
	Friend WithEvents btnTime As Button
End Class
