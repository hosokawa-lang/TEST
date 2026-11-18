<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmCalibWeight3
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
		Me.gbxMeasure = New System.Windows.Forms.GroupBox()
		Me.lblCurrentADValLiveY = New System.Windows.Forms.Label()
		Me.lblCurrentADValLiveX = New System.Windows.Forms.Label()
		Me.GroupBox2 = New System.Windows.Forms.GroupBox()
		Me.nudYSpanMax = New System.Windows.Forms.NumericUpDown()
		Me.btnYSpanAuto = New System.Windows.Forms.Button()
		Me.lblYSpan = New System.Windows.Forms.Label()
		Me.nudTimeSpan = New System.Windows.Forms.NumericUpDown()
		Me.nudYSpanMin = New System.Windows.Forms.NumericUpDown()
		Me.lblSpan = New System.Windows.Forms.Label()
		Me.btnResultdgvClear = New System.Windows.Forms.Button()
		Me.lblCurrentADValY = New System.Windows.Forms.Label()
		Me.lblCurrentADValX = New System.Windows.Forms.Label()
		Me.lblCoef = New System.Windows.Forms.Label()
		Me.btnExtract = New System.Windows.Forms.Button()
		Me.lblCoefValue = New System.Windows.Forms.Label()
		Me.lblWeightUnit = New System.Windows.Forms.Label()
		Me.Label1 = New System.Windows.Forms.Label()
		Me.lblCurrentAD = New System.Windows.Forms.Label()
		Me.dgvResult = New System.Windows.Forms.DataGridView()
		Me.nudWeight = New System.Windows.Forms.NumericUpDown()
		Me.btnZeroReset = New System.Windows.Forms.Button()
		Me.ChartAD = New System.Windows.Forms.DataVisualization.Charting.Chart()
		Me.GroupBox1 = New System.Windows.Forms.GroupBox()
		Me.ChartVector = New System.Windows.Forms.DataVisualization.Charting.Chart()
		Me.lblRealDirectLabel = New System.Windows.Forms.Label()
		Me.lblRealWeightLabel = New System.Windows.Forms.Label()
		Me.lblRealDirect = New System.Windows.Forms.Label()
		Me.lblRealWeight = New System.Windows.Forms.Label()
		Me.btnClose = New System.Windows.Forms.Button()
		Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
		Me.tbcMode = New System.Windows.Forms.TabControl()
		Me.TabPage1 = New System.Windows.Forms.TabPage()
		Me.TabPage2 = New System.Windows.Forms.TabPage()
		Me.gbxMeasureZ = New System.Windows.Forms.GroupBox()
		Me.lblCurrentADZ = New System.Windows.Forms.Label()
		Me.lblCurrentADValLiveZ = New System.Windows.Forms.Label()
		Me.lblCurrentGramValZ = New System.Windows.Forms.Label()
		Me.GroupBox4 = New System.Windows.Forms.GroupBox()
		Me.nudYSpanMaxZ = New System.Windows.Forms.NumericUpDown()
		Me.btnYSpanAutoZ = New System.Windows.Forms.Button()
		Me.lblYSpanZ = New System.Windows.Forms.Label()
		Me.nudTimeSpanZ = New System.Windows.Forms.NumericUpDown()
		Me.nudYSpanMinZ = New System.Windows.Forms.NumericUpDown()
		Me.lblSpanZ = New System.Windows.Forms.Label()
		Me.lblCurrentADValZ = New System.Windows.Forms.Label()
		Me.lblCoefZ = New System.Windows.Forms.Label()
		Me.btnExtractZ = New System.Windows.Forms.Button()
		Me.lblCoefValueZ = New System.Windows.Forms.Label()
		Me.lblWeightUnitZ = New System.Windows.Forms.Label()
		Me.nudWeightZ = New System.Windows.Forms.NumericUpDown()
		Me.btnZeroResetZ = New System.Windows.Forms.Button()
		Me.ChartADZ = New System.Windows.Forms.DataVisualization.Charting.Chart()
		Me.lblXYMeasDateVal = New System.Windows.Forms.Label()
		Me.lblZMeasDateVal = New System.Windows.Forms.Label()
		Me.gbxMeasure.SuspendLayout
		Me.GroupBox2.SuspendLayout
		CType(Me.nudYSpanMax,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.nudTimeSpan,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.nudYSpanMin,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.dgvResult,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.nudWeight,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.ChartAD,System.ComponentModel.ISupportInitialize).BeginInit
		Me.GroupBox1.SuspendLayout
		CType(Me.ChartVector,System.ComponentModel.ISupportInitialize).BeginInit
		Me.tbcMode.SuspendLayout
		Me.TabPage1.SuspendLayout
		Me.TabPage2.SuspendLayout
		Me.gbxMeasureZ.SuspendLayout
		Me.GroupBox4.SuspendLayout
		CType(Me.nudYSpanMaxZ,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.nudTimeSpanZ,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.nudYSpanMinZ,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.nudWeightZ,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.ChartADZ,System.ComponentModel.ISupportInitialize).BeginInit
		Me.SuspendLayout
		'
		'gbxMeasure
		'
		Me.gbxMeasure.Controls.Add(Me.lblXYMeasDateVal)
		Me.gbxMeasure.Controls.Add(Me.lblCurrentADValLiveY)
		Me.gbxMeasure.Controls.Add(Me.lblCurrentADValLiveX)
		Me.gbxMeasure.Controls.Add(Me.GroupBox2)
		Me.gbxMeasure.Controls.Add(Me.btnResultdgvClear)
		Me.gbxMeasure.Controls.Add(Me.lblCurrentADValY)
		Me.gbxMeasure.Controls.Add(Me.lblCurrentADValX)
		Me.gbxMeasure.Controls.Add(Me.lblCoef)
		Me.gbxMeasure.Controls.Add(Me.btnExtract)
		Me.gbxMeasure.Controls.Add(Me.lblCoefValue)
		Me.gbxMeasure.Controls.Add(Me.lblWeightUnit)
		Me.gbxMeasure.Controls.Add(Me.Label1)
		Me.gbxMeasure.Controls.Add(Me.lblCurrentAD)
		Me.gbxMeasure.Controls.Add(Me.dgvResult)
		Me.gbxMeasure.Controls.Add(Me.nudWeight)
		Me.gbxMeasure.Controls.Add(Me.btnZeroReset)
		Me.gbxMeasure.Controls.Add(Me.ChartAD)
		Me.gbxMeasure.Font = New System.Drawing.Font("Meiryo UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.gbxMeasure.Location = New System.Drawing.Point(6, 6)
		Me.gbxMeasure.Name = "gbxMeasure"
		Me.gbxMeasure.Size = New System.Drawing.Size(673, 767)
		Me.gbxMeasure.TabIndex = 1
		Me.gbxMeasure.TabStop = false
		Me.gbxMeasure.Text = "Measure control"
		'
		'lblCurrentADValLiveY
		'
		Me.lblCurrentADValLiveY.BackColor = System.Drawing.Color.White
		Me.lblCurrentADValLiveY.Font = New System.Drawing.Font("Arial", 18!)
		Me.lblCurrentADValLiveY.Location = New System.Drawing.Point(519, 609)
		Me.lblCurrentADValLiveY.Name = "lblCurrentADValLiveY"
		Me.lblCurrentADValLiveY.Size = New System.Drawing.Size(148, 27)
		Me.lblCurrentADValLiveY.TabIndex = 17
		Me.lblCurrentADValLiveY.Text = "N A"
		Me.lblCurrentADValLiveY.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		'
		'lblCurrentADValLiveX
		'
		Me.lblCurrentADValLiveX.BackColor = System.Drawing.Color.White
		Me.lblCurrentADValLiveX.Font = New System.Drawing.Font("Arial", 18!)
		Me.lblCurrentADValLiveX.Location = New System.Drawing.Point(262, 609)
		Me.lblCurrentADValLiveX.Name = "lblCurrentADValLiveX"
		Me.lblCurrentADValLiveX.Size = New System.Drawing.Size(148, 27)
		Me.lblCurrentADValLiveX.TabIndex = 17
		Me.lblCurrentADValLiveX.Text = "N A"
		Me.lblCurrentADValLiveX.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		'
		'GroupBox2
		'
		Me.GroupBox2.Controls.Add(Me.nudYSpanMax)
		Me.GroupBox2.Controls.Add(Me.btnYSpanAuto)
		Me.GroupBox2.Controls.Add(Me.lblYSpan)
		Me.GroupBox2.Controls.Add(Me.nudTimeSpan)
		Me.GroupBox2.Controls.Add(Me.nudYSpanMin)
		Me.GroupBox2.Controls.Add(Me.lblSpan)
		Me.GroupBox2.Location = New System.Drawing.Point(10, 632)
		Me.GroupBox2.Name = "GroupBox2"
		Me.GroupBox2.Size = New System.Drawing.Size(657, 87)
		Me.GroupBox2.TabIndex = 14
		Me.GroupBox2.TabStop = false
		'
		'nudYSpanMax
		'
		Me.nudYSpanMax.Font = New System.Drawing.Font("Arial", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
		Me.nudYSpanMax.Increment = New Decimal(New Integer() {1000, 0, 0, 0})
		Me.nudYSpanMax.Location = New System.Drawing.Point(337, 18)
		Me.nudYSpanMax.Maximum = New Decimal(New Integer() {9999999, 0, 0, 0})
		Me.nudYSpanMax.Minimum = New Decimal(New Integer() {9999999, 0, 0, -2147483648})
		Me.nudYSpanMax.Name = "nudYSpanMax"
		Me.nudYSpanMax.Size = New System.Drawing.Size(95, 26)
		Me.nudYSpanMax.TabIndex = 12
		Me.nudYSpanMax.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		Me.nudYSpanMax.Value = New Decimal(New Integer() {500000, 0, 0, 0})
		'
		'btnYSpanAuto
		'
		Me.btnYSpanAuto.Font = New System.Drawing.Font("Meiryo UI", 12!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.btnYSpanAuto.Location = New System.Drawing.Point(438, 16)
		Me.btnYSpanAuto.Name = "btnYSpanAuto"
		Me.btnYSpanAuto.Size = New System.Drawing.Size(110, 31)
		Me.btnYSpanAuto.TabIndex = 13
		Me.btnYSpanAuto.Text = "Auto"
		Me.btnYSpanAuto.UseVisualStyleBackColor = true
		'
		'lblYSpan
		'
		Me.lblYSpan.AutoSize = true
		Me.lblYSpan.Font = New System.Drawing.Font("Meiryo UI", 12!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
		Me.lblYSpan.Location = New System.Drawing.Point(19, 21)
		Me.lblYSpan.Name = "lblYSpan"
		Me.lblYSpan.Size = New System.Drawing.Size(189, 20)
		Me.lblYSpan.TabIndex = 10
		Me.lblYSpan.Text = "Stress Span [AD dig]"
		'
		'nudTimeSpan
		'
		Me.nudTimeSpan.Font = New System.Drawing.Font("Arial", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
		Me.nudTimeSpan.Increment = New Decimal(New Integer() {10, 0, 0, 0})
		Me.nudTimeSpan.Location = New System.Drawing.Point(239, 49)
		Me.nudTimeSpan.Maximum = New Decimal(New Integer() {10000, 0, 0, 0})
		Me.nudTimeSpan.Minimum = New Decimal(New Integer() {10, 0, 0, 0})
		Me.nudTimeSpan.Name = "nudTimeSpan"
		Me.nudTimeSpan.Size = New System.Drawing.Size(94, 26)
		Me.nudTimeSpan.TabIndex = 8
		Me.nudTimeSpan.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		Me.nudTimeSpan.Value = New Decimal(New Integer() {100, 0, 0, 0})
		'
		'nudYSpanMin
		'
		Me.nudYSpanMin.Font = New System.Drawing.Font("Arial", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
		Me.nudYSpanMin.Increment = New Decimal(New Integer() {1000, 0, 0, 0})
		Me.nudYSpanMin.Location = New System.Drawing.Point(239, 18)
		Me.nudYSpanMin.Maximum = New Decimal(New Integer() {9999999, 0, 0, 0})
		Me.nudYSpanMin.Minimum = New Decimal(New Integer() {9999999, 0, 0, -2147483648})
		Me.nudYSpanMin.Name = "nudYSpanMin"
		Me.nudYSpanMin.Size = New System.Drawing.Size(95, 26)
		Me.nudYSpanMin.TabIndex = 11
		Me.nudYSpanMin.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		Me.nudYSpanMin.Value = New Decimal(New Integer() {500000, 0, 0, -2147483648})
		'
		'lblSpan
		'
		Me.lblSpan.AutoSize = true
		Me.lblSpan.Font = New System.Drawing.Font("Meiryo UI", 12!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
		Me.lblSpan.Location = New System.Drawing.Point(19, 49)
		Me.lblSpan.Name = "lblSpan"
		Me.lblSpan.Size = New System.Drawing.Size(150, 20)
		Me.lblSpan.TabIndex = 7
		Me.lblSpan.Text = "Time Span [Sec]"
		'
		'btnResultdgvClear
		'
		Me.btnResultdgvClear.Font = New System.Drawing.Font("Meiryo UI", 12!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.btnResultdgvClear.Location = New System.Drawing.Point(532, 15)
		Me.btnResultdgvClear.Name = "btnResultdgvClear"
		Me.btnResultdgvClear.Size = New System.Drawing.Size(132, 34)
		Me.btnResultdgvClear.TabIndex = 16
		Me.btnResultdgvClear.Text = "Clear"
		Me.btnResultdgvClear.UseVisualStyleBackColor = true
		'
		'lblCurrentADValY
		'
		Me.lblCurrentADValY.BackColor = System.Drawing.Color.White
		Me.lblCurrentADValY.Font = New System.Drawing.Font("Arial", 18!)
		Me.lblCurrentADValY.Location = New System.Drawing.Point(514, 609)
		Me.lblCurrentADValY.Name = "lblCurrentADValY"
		Me.lblCurrentADValY.Size = New System.Drawing.Size(148, 27)
		Me.lblCurrentADValY.TabIndex = 9
		Me.lblCurrentADValY.Text = "N A"
		Me.lblCurrentADValY.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.lblCurrentADValY.Visible = false
		'
		'lblCurrentADValX
		'
		Me.lblCurrentADValX.BackColor = System.Drawing.Color.White
		Me.lblCurrentADValX.Font = New System.Drawing.Font("Arial", 18!)
		Me.lblCurrentADValX.Location = New System.Drawing.Point(514, 582)
		Me.lblCurrentADValX.Name = "lblCurrentADValX"
		Me.lblCurrentADValX.Size = New System.Drawing.Size(148, 27)
		Me.lblCurrentADValX.TabIndex = 9
		Me.lblCurrentADValX.Text = "N A"
		Me.lblCurrentADValX.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.lblCurrentADValX.Visible = false
		'
		'lblCoef
		'
		Me.lblCoef.AutoSize = true
		Me.lblCoef.Font = New System.Drawing.Font("Meiryo UI", 12!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
		Me.lblCoef.Location = New System.Drawing.Point(6, 733)
		Me.lblCoef.Name = "lblCoef"
		Me.lblCoef.Size = New System.Drawing.Size(183, 20)
		Me.lblCoef.TabIndex = 14
		Me.lblCoef.Text = "Coefficient(Dig->N):"
		Me.lblCoef.TextAlign = System.Drawing.ContentAlignment.MiddleRight
		'
		'btnExtract
		'
		Me.btnExtract.Font = New System.Drawing.Font("Meiryo UI", 12!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.btnExtract.Location = New System.Drawing.Point(239, 15)
		Me.btnExtract.Name = "btnExtract"
		Me.btnExtract.Size = New System.Drawing.Size(143, 34)
		Me.btnExtract.TabIndex = 3
		Me.btnExtract.Text = "Sampling"
		Me.btnExtract.UseVisualStyleBackColor = true
		'
		'lblCoefValue
		'
		Me.lblCoefValue.BackColor = System.Drawing.Color.White
		Me.lblCoefValue.Font = New System.Drawing.Font("Arial", 18!)
		Me.lblCoefValue.Location = New System.Drawing.Point(216, 730)
		Me.lblCoefValue.Name = "lblCoefValue"
		Me.lblCoefValue.Size = New System.Drawing.Size(205, 27)
		Me.lblCoefValue.TabIndex = 5
		Me.lblCoefValue.Text = "N A"
		Me.lblCoefValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		'
		'lblWeightUnit
		'
		Me.lblWeightUnit.AutoSize = true
		Me.lblWeightUnit.Font = New System.Drawing.Font("Meiryo UI", 12!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
		Me.lblWeightUnit.Location = New System.Drawing.Point(4, 23)
		Me.lblWeightUnit.Name = "lblWeightUnit"
		Me.lblWeightUnit.Size = New System.Drawing.Size(101, 20)
		Me.lblWeightUnit.TabIndex = 2
		Me.lblWeightUnit.Text = "Weight [g]"
		'
		'Label1
		'
		Me.Label1.AutoSize = true
		Me.Label1.BackColor = System.Drawing.SystemColors.ButtonHighlight
		Me.Label1.Font = New System.Drawing.Font("Meiryo UI", 12!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
		Me.Label1.Location = New System.Drawing.Point(445, 613)
		Me.Label1.Name = "Label1"
		Me.Label1.Size = New System.Drawing.Size(50, 20)
		Me.Label1.TabIndex = 2
		Me.Label1.Text = "Y AD"
		'
		'lblCurrentAD
		'
		Me.lblCurrentAD.AutoSize = true
		Me.lblCurrentAD.BackColor = System.Drawing.SystemColors.ButtonHighlight
		Me.lblCurrentAD.Font = New System.Drawing.Font("Meiryo UI", 12!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
		Me.lblCurrentAD.Location = New System.Drawing.Point(187, 613)
		Me.lblCurrentAD.Name = "lblCurrentAD"
		Me.lblCurrentAD.Size = New System.Drawing.Size(51, 20)
		Me.lblCurrentAD.TabIndex = 2
		Me.lblCurrentAD.Text = "X AD"
		'
		'dgvResult
		'
		Me.dgvResult.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
		Me.dgvResult.Location = New System.Drawing.Point(10, 56)
		Me.dgvResult.Name = "dgvResult"
		Me.dgvResult.RowTemplate.Height = 21
		Me.dgvResult.Size = New System.Drawing.Size(654, 121)
		Me.dgvResult.TabIndex = 0
		'
		'nudWeight
		'
		Me.nudWeight.DecimalPlaces = 3
		Me.nudWeight.Font = New System.Drawing.Font("Arial", 18!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
		Me.nudWeight.ForeColor = System.Drawing.Color.Blue
		Me.nudWeight.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
		Me.nudWeight.Location = New System.Drawing.Point(109, 15)
		Me.nudWeight.Maximum = New Decimal(New Integer() {99999, 0, 0, 65536})
		Me.nudWeight.Minimum = New Decimal(New Integer() {99999, 0, 0, -2147418112})
		Me.nudWeight.Name = "nudWeight"
		Me.nudWeight.Size = New System.Drawing.Size(127, 35)
		Me.nudWeight.TabIndex = 1
		Me.nudWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		'
		'btnZeroReset
		'
		Me.btnZeroReset.Font = New System.Drawing.Font("Meiryo UI", 12!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.btnZeroReset.Location = New System.Drawing.Point(385, 15)
		Me.btnZeroReset.Name = "btnZeroReset"
		Me.btnZeroReset.Size = New System.Drawing.Size(143, 34)
		Me.btnZeroReset.TabIndex = 0
		Me.btnZeroReset.Text = "Zero reset"
		Me.btnZeroReset.UseVisualStyleBackColor = true
		'
		'ChartAD
		'
		Me.ChartAD.Location = New System.Drawing.Point(8, 183)
		Me.ChartAD.Name = "ChartAD"
		Me.ChartAD.Size = New System.Drawing.Size(657, 459)
		Me.ChartAD.TabIndex = 6
		Me.ChartAD.Text = "Chart1"
		'
		'GroupBox1
		'
		Me.GroupBox1.Controls.Add(Me.ChartVector)
		Me.GroupBox1.Controls.Add(Me.lblRealDirectLabel)
		Me.GroupBox1.Controls.Add(Me.lblRealWeightLabel)
		Me.GroupBox1.Controls.Add(Me.lblRealDirect)
		Me.GroupBox1.Controls.Add(Me.lblRealWeight)
		Me.GroupBox1.Location = New System.Drawing.Point(685, 4)
		Me.GroupBox1.Name = "GroupBox1"
		Me.GroupBox1.Size = New System.Drawing.Size(676, 762)
		Me.GroupBox1.TabIndex = 2
		Me.GroupBox1.TabStop = false
		'
		'ChartVector
		'
		Me.ChartVector.Location = New System.Drawing.Point(17, 22)
		Me.ChartVector.Name = "ChartVector"
		Me.ChartVector.Size = New System.Drawing.Size(650, 650)
		Me.ChartVector.TabIndex = 17
		Me.ChartVector.Text = "Chart1"
		'
		'lblRealDirectLabel
		'
		Me.lblRealDirectLabel.AutoSize = true
		Me.lblRealDirectLabel.Font = New System.Drawing.Font("Meiryo UI", 12!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
		Me.lblRealDirectLabel.Location = New System.Drawing.Point(11, 731)
		Me.lblRealDirectLabel.Name = "lblRealDirectLabel"
		Me.lblRealDirectLabel.Size = New System.Drawing.Size(135, 20)
		Me.lblRealDirectLabel.TabIndex = 14
		Me.lblRealDirectLabel.Text = "Direction[deg]"
		'
		'lblRealWeightLabel
		'
		Me.lblRealWeightLabel.AutoSize = true
		Me.lblRealWeightLabel.Font = New System.Drawing.Font("Meiryo UI", 12!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
		Me.lblRealWeightLabel.Location = New System.Drawing.Point(11, 700)
		Me.lblRealWeightLabel.Name = "lblRealWeightLabel"
		Me.lblRealWeightLabel.Size = New System.Drawing.Size(96, 20)
		Me.lblRealWeightLabel.TabIndex = 14
		Me.lblRealWeightLabel.Text = "Weight[g]"
		'
		'lblRealDirect
		'
		Me.lblRealDirect.BackColor = System.Drawing.Color.White
		Me.lblRealDirect.Font = New System.Drawing.Font("Arial", 18!)
		Me.lblRealDirect.Location = New System.Drawing.Point(208, 726)
		Me.lblRealDirect.Name = "lblRealDirect"
		Me.lblRealDirect.Size = New System.Drawing.Size(205, 27)
		Me.lblRealDirect.TabIndex = 5
		Me.lblRealDirect.Text = "N A"
		Me.lblRealDirect.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		'
		'lblRealWeight
		'
		Me.lblRealWeight.BackColor = System.Drawing.Color.White
		Me.lblRealWeight.Font = New System.Drawing.Font("Arial", 18!)
		Me.lblRealWeight.Location = New System.Drawing.Point(208, 695)
		Me.lblRealWeight.Name = "lblRealWeight"
		Me.lblRealWeight.Size = New System.Drawing.Size(205, 27)
		Me.lblRealWeight.TabIndex = 5
		Me.lblRealWeight.Text = "N A"
		Me.lblRealWeight.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		'
		'btnClose
		'
		Me.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel
		Me.btnClose.Font = New System.Drawing.Font("Meiryo UI", 12!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.btnClose.Location = New System.Drawing.Point(1245, 819)
		Me.btnClose.Name = "btnClose"
		Me.btnClose.Size = New System.Drawing.Size(132, 34)
		Me.btnClose.TabIndex = 17
		Me.btnClose.Text = "Close"
		Me.btnClose.UseVisualStyleBackColor = true
		'
		'Timer1
		'
		'
		'tbcMode
		'
		Me.tbcMode.Controls.Add(Me.TabPage1)
		Me.tbcMode.Controls.Add(Me.TabPage2)
		Me.tbcMode.Font = New System.Drawing.Font("Meiryo UI", 12!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.tbcMode.Location = New System.Drawing.Point(12, 12)
		Me.tbcMode.Name = "tbcMode"
		Me.tbcMode.SelectedIndex = 0
		Me.tbcMode.Size = New System.Drawing.Size(1382, 805)
		Me.tbcMode.TabIndex = 3
		'
		'TabPage1
		'
		Me.TabPage1.Controls.Add(Me.gbxMeasure)
		Me.TabPage1.Controls.Add(Me.GroupBox1)
		Me.TabPage1.Font = New System.Drawing.Font("Meiryo UI", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.TabPage1.Location = New System.Drawing.Point(4, 29)
		Me.TabPage1.Name = "TabPage1"
		Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
		Me.TabPage1.Size = New System.Drawing.Size(1374, 772)
		Me.TabPage1.TabIndex = 0
		Me.TabPage1.Text = "TabPage1"
		Me.TabPage1.UseVisualStyleBackColor = true
		'
		'TabPage2
		'
		Me.TabPage2.Controls.Add(Me.gbxMeasureZ)
		Me.TabPage2.Font = New System.Drawing.Font("Meiryo UI", 12!)
		Me.TabPage2.Location = New System.Drawing.Point(4, 29)
		Me.TabPage2.Name = "TabPage2"
		Me.TabPage2.Padding = New System.Windows.Forms.Padding(3)
		Me.TabPage2.Size = New System.Drawing.Size(1374, 772)
		Me.TabPage2.TabIndex = 1
		Me.TabPage2.Text = "TabPage2"
		Me.TabPage2.UseVisualStyleBackColor = true
		'
		'gbxMeasureZ
		'
		Me.gbxMeasureZ.Controls.Add(Me.lblZMeasDateVal)
		Me.gbxMeasureZ.Controls.Add(Me.lblCurrentADZ)
		Me.gbxMeasureZ.Controls.Add(Me.lblCurrentADValLiveZ)
		Me.gbxMeasureZ.Controls.Add(Me.lblCurrentGramValZ)
		Me.gbxMeasureZ.Controls.Add(Me.GroupBox4)
		Me.gbxMeasureZ.Controls.Add(Me.lblCurrentADValZ)
		Me.gbxMeasureZ.Controls.Add(Me.lblCoefZ)
		Me.gbxMeasureZ.Controls.Add(Me.btnExtractZ)
		Me.gbxMeasureZ.Controls.Add(Me.lblCoefValueZ)
		Me.gbxMeasureZ.Controls.Add(Me.lblWeightUnitZ)
		Me.gbxMeasureZ.Controls.Add(Me.nudWeightZ)
		Me.gbxMeasureZ.Controls.Add(Me.btnZeroResetZ)
		Me.gbxMeasureZ.Controls.Add(Me.ChartADZ)
		Me.gbxMeasureZ.Font = New System.Drawing.Font("Meiryo UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.gbxMeasureZ.Location = New System.Drawing.Point(6, 6)
		Me.gbxMeasureZ.Name = "gbxMeasureZ"
		Me.gbxMeasureZ.Size = New System.Drawing.Size(673, 767)
		Me.gbxMeasureZ.TabIndex = 2
		Me.gbxMeasureZ.TabStop = false
		Me.gbxMeasureZ.Text = "Measure control"
		'
		'lblCurrentADZ
		'
		Me.lblCurrentADZ.AutoSize = true
		Me.lblCurrentADZ.BackColor = System.Drawing.SystemColors.ButtonHighlight
		Me.lblCurrentADZ.Font = New System.Drawing.Font("Meiryo UI", 12!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
		Me.lblCurrentADZ.Location = New System.Drawing.Point(249, 602)
		Me.lblCurrentADZ.Name = "lblCurrentADZ"
		Me.lblCurrentADZ.Size = New System.Drawing.Size(50, 20)
		Me.lblCurrentADZ.TabIndex = 2
		Me.lblCurrentADZ.Text = "Z AD"
		'
		'lblCurrentADValLiveZ
		'
		Me.lblCurrentADValLiveZ.BackColor = System.Drawing.Color.White
		Me.lblCurrentADValLiveZ.Font = New System.Drawing.Font("Arial", 18!)
		Me.lblCurrentADValLiveZ.Location = New System.Drawing.Point(354, 598)
		Me.lblCurrentADValLiveZ.Name = "lblCurrentADValLiveZ"
		Me.lblCurrentADValLiveZ.Size = New System.Drawing.Size(148, 27)
		Me.lblCurrentADValLiveZ.TabIndex = 81
		Me.lblCurrentADValLiveZ.Text = "N A"
		Me.lblCurrentADValLiveZ.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		'
		'lblCurrentGramValZ
		'
		Me.lblCurrentGramValZ.BackColor = System.Drawing.Color.White
		Me.lblCurrentGramValZ.Font = New System.Drawing.Font("Arial", 18!)
		Me.lblCurrentGramValZ.Location = New System.Drawing.Point(508, 598)
		Me.lblCurrentGramValZ.Name = "lblCurrentGramValZ"
		Me.lblCurrentGramValZ.Size = New System.Drawing.Size(148, 27)
		Me.lblCurrentGramValZ.TabIndex = 80
		Me.lblCurrentGramValZ.Text = "N A"
		Me.lblCurrentGramValZ.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		'
		'GroupBox4
		'
		Me.GroupBox4.Controls.Add(Me.nudYSpanMaxZ)
		Me.GroupBox4.Controls.Add(Me.btnYSpanAutoZ)
		Me.GroupBox4.Controls.Add(Me.lblYSpanZ)
		Me.GroupBox4.Controls.Add(Me.nudTimeSpanZ)
		Me.GroupBox4.Controls.Add(Me.nudYSpanMinZ)
		Me.GroupBox4.Controls.Add(Me.lblSpanZ)
		Me.GroupBox4.Location = New System.Drawing.Point(10, 632)
		Me.GroupBox4.Name = "GroupBox4"
		Me.GroupBox4.Size = New System.Drawing.Size(657, 87)
		Me.GroupBox4.TabIndex = 14
		Me.GroupBox4.TabStop = false
		'
		'nudYSpanMaxZ
		'
		Me.nudYSpanMaxZ.Font = New System.Drawing.Font("Arial", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
		Me.nudYSpanMaxZ.Increment = New Decimal(New Integer() {1000, 0, 0, 0})
		Me.nudYSpanMaxZ.Location = New System.Drawing.Point(337, 18)
		Me.nudYSpanMaxZ.Maximum = New Decimal(New Integer() {9999999, 0, 0, 0})
		Me.nudYSpanMaxZ.Minimum = New Decimal(New Integer() {9999999, 0, 0, -2147483648})
		Me.nudYSpanMaxZ.Name = "nudYSpanMaxZ"
		Me.nudYSpanMaxZ.Size = New System.Drawing.Size(95, 26)
		Me.nudYSpanMaxZ.TabIndex = 12
		Me.nudYSpanMaxZ.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		Me.nudYSpanMaxZ.Value = New Decimal(New Integer() {500000, 0, 0, 0})
		'
		'btnYSpanAutoZ
		'
		Me.btnYSpanAutoZ.Font = New System.Drawing.Font("Meiryo UI", 12!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.btnYSpanAutoZ.Location = New System.Drawing.Point(438, 16)
		Me.btnYSpanAutoZ.Name = "btnYSpanAutoZ"
		Me.btnYSpanAutoZ.Size = New System.Drawing.Size(110, 31)
		Me.btnYSpanAutoZ.TabIndex = 13
		Me.btnYSpanAutoZ.Text = "Auto"
		Me.btnYSpanAutoZ.UseVisualStyleBackColor = true
		'
		'lblYSpanZ
		'
		Me.lblYSpanZ.AutoSize = true
		Me.lblYSpanZ.Font = New System.Drawing.Font("Meiryo UI", 12!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
		Me.lblYSpanZ.Location = New System.Drawing.Point(19, 21)
		Me.lblYSpanZ.Name = "lblYSpanZ"
		Me.lblYSpanZ.Size = New System.Drawing.Size(189, 20)
		Me.lblYSpanZ.TabIndex = 10
		Me.lblYSpanZ.Text = "Stress Span [AD dig]"
		'
		'nudTimeSpanZ
		'
		Me.nudTimeSpanZ.Font = New System.Drawing.Font("Arial", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
		Me.nudTimeSpanZ.Increment = New Decimal(New Integer() {10, 0, 0, 0})
		Me.nudTimeSpanZ.Location = New System.Drawing.Point(239, 49)
		Me.nudTimeSpanZ.Maximum = New Decimal(New Integer() {10000, 0, 0, 0})
		Me.nudTimeSpanZ.Minimum = New Decimal(New Integer() {10, 0, 0, 0})
		Me.nudTimeSpanZ.Name = "nudTimeSpanZ"
		Me.nudTimeSpanZ.Size = New System.Drawing.Size(94, 26)
		Me.nudTimeSpanZ.TabIndex = 8
		Me.nudTimeSpanZ.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		Me.nudTimeSpanZ.Value = New Decimal(New Integer() {100, 0, 0, 0})
		'
		'nudYSpanMinZ
		'
		Me.nudYSpanMinZ.Font = New System.Drawing.Font("Arial", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
		Me.nudYSpanMinZ.Increment = New Decimal(New Integer() {1000, 0, 0, 0})
		Me.nudYSpanMinZ.Location = New System.Drawing.Point(239, 18)
		Me.nudYSpanMinZ.Maximum = New Decimal(New Integer() {9999999, 0, 0, 0})
		Me.nudYSpanMinZ.Minimum = New Decimal(New Integer() {9999999, 0, 0, -2147483648})
		Me.nudYSpanMinZ.Name = "nudYSpanMinZ"
		Me.nudYSpanMinZ.Size = New System.Drawing.Size(95, 26)
		Me.nudYSpanMinZ.TabIndex = 11
		Me.nudYSpanMinZ.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		Me.nudYSpanMinZ.Value = New Decimal(New Integer() {500000, 0, 0, -2147483648})
		'
		'lblSpanZ
		'
		Me.lblSpanZ.AutoSize = true
		Me.lblSpanZ.Font = New System.Drawing.Font("Meiryo UI", 12!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
		Me.lblSpanZ.Location = New System.Drawing.Point(19, 49)
		Me.lblSpanZ.Name = "lblSpanZ"
		Me.lblSpanZ.Size = New System.Drawing.Size(150, 20)
		Me.lblSpanZ.TabIndex = 7
		Me.lblSpanZ.Text = "Time Span [Sec]"
		'
		'lblCurrentADValZ
		'
		Me.lblCurrentADValZ.BackColor = System.Drawing.Color.White
		Me.lblCurrentADValZ.Font = New System.Drawing.Font("Arial", 18!)
		Me.lblCurrentADValZ.Location = New System.Drawing.Point(354, 598)
		Me.lblCurrentADValZ.Name = "lblCurrentADValZ"
		Me.lblCurrentADValZ.Size = New System.Drawing.Size(148, 27)
		Me.lblCurrentADValZ.TabIndex = 9
		Me.lblCurrentADValZ.Text = "N A"
		Me.lblCurrentADValZ.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.lblCurrentADValZ.Visible = false
		'
		'lblCoefZ
		'
		Me.lblCoefZ.AutoSize = true
		Me.lblCoefZ.Font = New System.Drawing.Font("Meiryo UI", 12!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
		Me.lblCoefZ.Location = New System.Drawing.Point(6, 733)
		Me.lblCoefZ.Name = "lblCoefZ"
		Me.lblCoefZ.Size = New System.Drawing.Size(209, 20)
		Me.lblCoefZ.TabIndex = 14
		Me.lblCoefZ.Text = "Coefficient(Dig->gram)"
		'
		'btnExtractZ
		'
		Me.btnExtractZ.Font = New System.Drawing.Font("Meiryo UI", 12!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.btnExtractZ.Location = New System.Drawing.Point(239, 15)
		Me.btnExtractZ.Name = "btnExtractZ"
		Me.btnExtractZ.Size = New System.Drawing.Size(143, 34)
		Me.btnExtractZ.TabIndex = 3
		Me.btnExtractZ.Text = "Sampling"
		Me.btnExtractZ.UseVisualStyleBackColor = true
		'
		'lblCoefValueZ
		'
		Me.lblCoefValueZ.BackColor = System.Drawing.Color.White
		Me.lblCoefValueZ.Font = New System.Drawing.Font("Arial", 18!)
		Me.lblCoefValueZ.Location = New System.Drawing.Point(216, 730)
		Me.lblCoefValueZ.Name = "lblCoefValueZ"
		Me.lblCoefValueZ.Size = New System.Drawing.Size(205, 27)
		Me.lblCoefValueZ.TabIndex = 5
		Me.lblCoefValueZ.Text = "N A"
		Me.lblCoefValueZ.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		'
		'lblWeightUnitZ
		'
		Me.lblWeightUnitZ.AutoSize = true
		Me.lblWeightUnitZ.Font = New System.Drawing.Font("Meiryo UI", 12!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
		Me.lblWeightUnitZ.Location = New System.Drawing.Point(4, 23)
		Me.lblWeightUnitZ.Name = "lblWeightUnitZ"
		Me.lblWeightUnitZ.Size = New System.Drawing.Size(101, 20)
		Me.lblWeightUnitZ.TabIndex = 2
		Me.lblWeightUnitZ.Text = "Weight [g]"
		'
		'nudWeightZ
		'
		Me.nudWeightZ.DecimalPlaces = 3
		Me.nudWeightZ.Font = New System.Drawing.Font("Arial", 18!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
		Me.nudWeightZ.ForeColor = System.Drawing.Color.Blue
		Me.nudWeightZ.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
		Me.nudWeightZ.Location = New System.Drawing.Point(109, 15)
		Me.nudWeightZ.Maximum = New Decimal(New Integer() {99999, 0, 0, 65536})
		Me.nudWeightZ.Minimum = New Decimal(New Integer() {99999, 0, 0, -2147418112})
		Me.nudWeightZ.Name = "nudWeightZ"
		Me.nudWeightZ.Size = New System.Drawing.Size(127, 35)
		Me.nudWeightZ.TabIndex = 1
		Me.nudWeightZ.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		'
		'btnZeroResetZ
		'
		Me.btnZeroResetZ.Font = New System.Drawing.Font("Meiryo UI", 12!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.btnZeroResetZ.Location = New System.Drawing.Point(385, 15)
		Me.btnZeroResetZ.Name = "btnZeroResetZ"
		Me.btnZeroResetZ.Size = New System.Drawing.Size(143, 34)
		Me.btnZeroResetZ.TabIndex = 0
		Me.btnZeroResetZ.Text = "Zero reset"
		Me.btnZeroResetZ.UseVisualStyleBackColor = true
		'
		'ChartADZ
		'
		Me.ChartADZ.Location = New System.Drawing.Point(7, 56)
		Me.ChartADZ.Name = "ChartADZ"
		Me.ChartADZ.Size = New System.Drawing.Size(657, 525)
		Me.ChartADZ.TabIndex = 6
		Me.ChartADZ.Text = "Chart1"
		'
		'lblXYMeasDateVal
		'
		Me.lblXYMeasDateVal.Font = New System.Drawing.Font("Meiryo UI", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.lblXYMeasDateVal.Location = New System.Drawing.Point(498, 733)
		Me.lblXYMeasDateVal.Name = "lblXYMeasDateVal"
		Me.lblXYMeasDateVal.Size = New System.Drawing.Size(166, 20)
		Me.lblXYMeasDateVal.TabIndex = 18
		Me.lblXYMeasDateVal.Text = "2022/11/22 8:00"
		Me.lblXYMeasDateVal.TextAlign = System.Drawing.ContentAlignment.MiddleRight
		'
		'lblZMeasDateVal
		'
		Me.lblZMeasDateVal.Font = New System.Drawing.Font("Meiryo UI", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.lblZMeasDateVal.Location = New System.Drawing.Point(498, 733)
		Me.lblZMeasDateVal.Name = "lblZMeasDateVal"
		Me.lblZMeasDateVal.Size = New System.Drawing.Size(166, 20)
		Me.lblZMeasDateVal.TabIndex = 19
		Me.lblZMeasDateVal.Text = "2022/11/22 8:00"
		Me.lblZMeasDateVal.TextAlign = System.Drawing.ContentAlignment.MiddleRight
		'
		'frmCalibWeight3
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6!, 12!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.CancelButton = Me.btnClose
		Me.ClientSize = New System.Drawing.Size(1400, 858)
		Me.Controls.Add(Me.btnClose)
		Me.Controls.Add(Me.tbcMode)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
		Me.KeyPreview = true
		Me.MaximizeBox = false
		Me.MinimizeBox = false
		Me.Name = "frmCalibWeight3"
		Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
		Me.Text = "Calibration(Weight)"
		Me.gbxMeasure.ResumeLayout(false)
		Me.gbxMeasure.PerformLayout
		Me.GroupBox2.ResumeLayout(false)
		Me.GroupBox2.PerformLayout
		CType(Me.nudYSpanMax,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.nudTimeSpan,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.nudYSpanMin,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.dgvResult,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.nudWeight,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.ChartAD,System.ComponentModel.ISupportInitialize).EndInit
		Me.GroupBox1.ResumeLayout(false)
		Me.GroupBox1.PerformLayout
		CType(Me.ChartVector,System.ComponentModel.ISupportInitialize).EndInit
		Me.tbcMode.ResumeLayout(false)
		Me.TabPage1.ResumeLayout(false)
		Me.TabPage2.ResumeLayout(false)
		Me.gbxMeasureZ.ResumeLayout(false)
		Me.gbxMeasureZ.PerformLayout
		Me.GroupBox4.ResumeLayout(false)
		Me.GroupBox4.PerformLayout
		CType(Me.nudYSpanMaxZ,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.nudTimeSpanZ,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.nudYSpanMinZ,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.nudWeightZ,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.ChartADZ,System.ComponentModel.ISupportInitialize).EndInit
		Me.ResumeLayout(false)

End Sub

	Friend WithEvents gbxMeasure As GroupBox
	Friend WithEvents btnYSpanAuto As Button
	Friend WithEvents nudYSpanMax As NumericUpDown
	Friend WithEvents nudYSpanMin As NumericUpDown
	Friend WithEvents lblYSpan As Label
	Friend WithEvents lblCurrentADValY As Label
	Friend WithEvents lblCurrentADValX As Label
	Friend WithEvents nudTimeSpan As NumericUpDown
	Friend WithEvents lblSpan As Label
	Friend WithEvents ChartAD As DataVisualization.Charting.Chart
	Friend WithEvents btnExtract As Button
	Friend WithEvents lblCurrentAD As Label
	Friend WithEvents lblWeightUnit As Label
	Friend WithEvents nudWeight As NumericUpDown
	Friend WithEvents btnZeroReset As Button
	Friend WithEvents GroupBox1 As GroupBox
	Friend WithEvents btnResultdgvClear As Button
	Friend WithEvents lblCoef As Label
	Friend WithEvents lblCoefValue As Label
	Friend WithEvents dgvResult As DataGridView
	Friend WithEvents GroupBox2 As GroupBox
	Friend WithEvents ChartVector As DataVisualization.Charting.Chart
	Friend WithEvents Timer1 As Timer
	Friend WithEvents lblRealDirectLabel As Label
	Friend WithEvents lblRealWeightLabel As Label
	Friend WithEvents lblRealDirect As Label
	Friend WithEvents lblRealWeight As Label
	Friend WithEvents btnClose As Button
	Friend WithEvents tbcMode As TabControl
	Friend WithEvents TabPage1 As TabPage
	Friend WithEvents TabPage2 As TabPage
	Friend WithEvents gbxMeasureZ As GroupBox
	Friend WithEvents GroupBox4 As GroupBox
	Friend WithEvents nudYSpanMaxZ As NumericUpDown
	Friend WithEvents btnYSpanAutoZ As Button
	Friend WithEvents lblYSpanZ As Label
	Friend WithEvents nudTimeSpanZ As NumericUpDown
	Friend WithEvents nudYSpanMinZ As NumericUpDown
	Friend WithEvents lblSpanZ As Label
	Friend WithEvents lblCoefZ As Label
	Friend WithEvents btnExtractZ As Button
	Friend WithEvents lblCoefValueZ As Label
	Friend WithEvents lblWeightUnitZ As Label
	Friend WithEvents lblCurrentADZ As Label
	Friend WithEvents nudWeightZ As NumericUpDown
	Friend WithEvents btnZeroResetZ As Button
	Friend WithEvents ChartADZ As DataVisualization.Charting.Chart
    Friend WithEvents lblCurrentGramValZ As Label
	Friend WithEvents lblCurrentADValLiveZ As Label
	Friend WithEvents lblCurrentADValLiveY As Label
	Friend WithEvents lblCurrentADValLiveX As Label
	Friend WithEvents Label1 As Label
	Friend WithEvents lblCurrentADValZ As Label
	Friend WithEvents lblXYMeasDateVal As Label
	Friend WithEvents lblZMeasDateVal As Label
End Class
