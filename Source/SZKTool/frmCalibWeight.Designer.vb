<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmCalibWeight
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
		Dim ChartArea2 As System.Windows.Forms.DataVisualization.Charting.ChartArea = New System.Windows.Forms.DataVisualization.Charting.ChartArea()
		Me.gbxMeasure = New System.Windows.Forms.GroupBox()
		Me.btnYSpanAuto = New System.Windows.Forms.Button()
		Me.nudYSpanMax = New System.Windows.Forms.NumericUpDown()
		Me.nudYSpanMin = New System.Windows.Forms.NumericUpDown()
		Me.lblYSpan = New System.Windows.Forms.Label()
		Me.lblCurrentADValY = New System.Windows.Forms.Label()
		Me.lblCurrentADValX = New System.Windows.Forms.Label()
		Me.nudTimeSpan = New System.Windows.Forms.NumericUpDown()
		Me.lblSpan = New System.Windows.Forms.Label()
		Me.ChartAD = New System.Windows.Forms.DataVisualization.Charting.Chart()
		Me.lblADUnit = New System.Windows.Forms.Label()
		Me.lblMeasStress = New System.Windows.Forms.Label()
		Me.btnSub = New System.Windows.Forms.Button()
		Me.btnAdd = New System.Windows.Forms.Button()
		Me.btnExtract = New System.Windows.Forms.Button()
		Me.lblCurrentAD = New System.Windows.Forms.Label()
		Me.lblWeightUnit = New System.Windows.Forms.Label()
		Me.nudWeightAdd = New System.Windows.Forms.NumericUpDown()
		Me.nudWeight = New System.Windows.Forms.NumericUpDown()
		Me.btnYStart = New System.Windows.Forms.Button()
		Me.btnXStart = New System.Windows.Forms.Button()
		Me.GroupBox1 = New System.Windows.Forms.GroupBox()
		Me.btnYdgvClear = New System.Windows.Forms.Button()
		Me.btnXdgvClear = New System.Windows.Forms.Button()
		Me.Label2 = New System.Windows.Forms.Label()
		Me.Label1 = New System.Windows.Forms.Label()
		Me.GroupBox2 = New System.Windows.Forms.GroupBox()
		Me.rbY = New System.Windows.Forms.RadioButton()
		Me.rbX = New System.Windows.Forms.RadioButton()
		Me.Label7 = New System.Windows.Forms.Label()
		Me.Label6 = New System.Windows.Forms.Label()
		Me.lblCoefY = New System.Windows.Forms.Label()
		Me.lblCoefX = New System.Windows.Forms.Label()
		Me.ResultChart = New System.Windows.Forms.DataVisualization.Charting.Chart()
		Me.dgvYResult = New System.Windows.Forms.DataGridView()
		Me.dgvXResult = New System.Windows.Forms.DataGridView()
		Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
		Me.cmsResultDgv = New System.Windows.Forms.ContextMenuStrip(Me.components)
		Me.DeleteToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
		Me.SaveFileDialog = New System.Windows.Forms.SaveFileDialog()
		Me.ss1 = New System.Windows.Forms.StatusStrip()
		Me.gbxMeasure.SuspendLayout
		CType(Me.nudYSpanMax,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.nudYSpanMin,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.nudTimeSpan,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.ChartAD,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.nudWeightAdd,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.nudWeight,System.ComponentModel.ISupportInitialize).BeginInit
		Me.GroupBox1.SuspendLayout
		Me.GroupBox2.SuspendLayout
		CType(Me.ResultChart,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.dgvYResult,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.dgvXResult,System.ComponentModel.ISupportInitialize).BeginInit
		Me.cmsResultDgv.SuspendLayout
		Me.SuspendLayout
		'
		'gbxMeasure
		'
		Me.gbxMeasure.Controls.Add(Me.btnYSpanAuto)
		Me.gbxMeasure.Controls.Add(Me.nudYSpanMax)
		Me.gbxMeasure.Controls.Add(Me.nudYSpanMin)
		Me.gbxMeasure.Controls.Add(Me.lblYSpan)
		Me.gbxMeasure.Controls.Add(Me.lblCurrentADValY)
		Me.gbxMeasure.Controls.Add(Me.lblCurrentADValX)
		Me.gbxMeasure.Controls.Add(Me.nudTimeSpan)
		Me.gbxMeasure.Controls.Add(Me.lblSpan)
		Me.gbxMeasure.Controls.Add(Me.ChartAD)
		Me.gbxMeasure.Controls.Add(Me.lblADUnit)
		Me.gbxMeasure.Controls.Add(Me.lblMeasStress)
		Me.gbxMeasure.Controls.Add(Me.btnSub)
		Me.gbxMeasure.Controls.Add(Me.btnAdd)
		Me.gbxMeasure.Controls.Add(Me.btnExtract)
		Me.gbxMeasure.Controls.Add(Me.lblCurrentAD)
		Me.gbxMeasure.Controls.Add(Me.lblWeightUnit)
		Me.gbxMeasure.Controls.Add(Me.nudWeightAdd)
		Me.gbxMeasure.Controls.Add(Me.nudWeight)
		Me.gbxMeasure.Controls.Add(Me.btnYStart)
		Me.gbxMeasure.Controls.Add(Me.btnXStart)
		Me.gbxMeasure.Location = New System.Drawing.Point(2, 12)
		Me.gbxMeasure.Name = "gbxMeasure"
		Me.gbxMeasure.Size = New System.Drawing.Size(1142, 367)
		Me.gbxMeasure.TabIndex = 0
		Me.gbxMeasure.TabStop = false
		Me.gbxMeasure.Text = "Measure control"
		'
		'btnYSpanAuto
		'
		Me.btnYSpanAuto.Font = New System.Drawing.Font("游ゴシック", 12!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.btnYSpanAuto.Location = New System.Drawing.Point(389, 331)
		Me.btnYSpanAuto.Name = "btnYSpanAuto"
		Me.btnYSpanAuto.Size = New System.Drawing.Size(68, 31)
		Me.btnYSpanAuto.TabIndex = 13
		Me.btnYSpanAuto.Text = "Auto"
		Me.btnYSpanAuto.UseVisualStyleBackColor = true
		'
		'nudYSpanMax
		'
		Me.nudYSpanMax.Font = New System.Drawing.Font("Arial", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
		Me.nudYSpanMax.Increment = New Decimal(New Integer() {1000, 0, 0, 0})
		Me.nudYSpanMax.Location = New System.Drawing.Point(288, 333)
		Me.nudYSpanMax.Maximum = New Decimal(New Integer() {9999999, 0, 0, 0})
		Me.nudYSpanMax.Minimum = New Decimal(New Integer() {9999999, 0, 0, -2147483648})
		Me.nudYSpanMax.Name = "nudYSpanMax"
		Me.nudYSpanMax.Size = New System.Drawing.Size(95, 26)
		Me.nudYSpanMax.TabIndex = 12
		Me.nudYSpanMax.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		Me.nudYSpanMax.Value = New Decimal(New Integer() {500000, 0, 0, 0})
		'
		'nudYSpanMin
		'
		Me.nudYSpanMin.Font = New System.Drawing.Font("Arial", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
		Me.nudYSpanMin.Increment = New Decimal(New Integer() {1000, 0, 0, 0})
		Me.nudYSpanMin.Location = New System.Drawing.Point(190, 333)
		Me.nudYSpanMin.Maximum = New Decimal(New Integer() {9999999, 0, 0, 0})
		Me.nudYSpanMin.Minimum = New Decimal(New Integer() {9999999, 0, 0, -2147483648})
		Me.nudYSpanMin.Name = "nudYSpanMin"
		Me.nudYSpanMin.Size = New System.Drawing.Size(95, 26)
		Me.nudYSpanMin.TabIndex = 11
		Me.nudYSpanMin.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		Me.nudYSpanMin.Value = New Decimal(New Integer() {500000, 0, 0, -2147483648})
		'
		'lblYSpan
		'
		Me.lblYSpan.AutoSize = true
		Me.lblYSpan.Font = New System.Drawing.Font("游ゴシック", 12!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.lblYSpan.Location = New System.Drawing.Point(10, 336)
		Me.lblYSpan.Name = "lblYSpan"
		Me.lblYSpan.Size = New System.Drawing.Size(174, 21)
		Me.lblYSpan.TabIndex = 10
		Me.lblYSpan.Text = "Stress Span [AD dig]"
		'
		'lblCurrentADValY
		'
		Me.lblCurrentADValY.BackColor = System.Drawing.Color.White
		Me.lblCurrentADValY.Font = New System.Drawing.Font("Arial", 18!)
		Me.lblCurrentADValY.Location = New System.Drawing.Point(985, 333)
		Me.lblCurrentADValY.Name = "lblCurrentADValY"
		Me.lblCurrentADValY.Size = New System.Drawing.Size(148, 27)
		Me.lblCurrentADValY.TabIndex = 9
		Me.lblCurrentADValY.Text = "N A"
		Me.lblCurrentADValY.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		'
		'lblCurrentADValX
		'
		Me.lblCurrentADValX.BackColor = System.Drawing.Color.White
		Me.lblCurrentADValX.Font = New System.Drawing.Font("Arial", 18!)
		Me.lblCurrentADValX.Location = New System.Drawing.Point(826, 333)
		Me.lblCurrentADValX.Name = "lblCurrentADValX"
		Me.lblCurrentADValX.Size = New System.Drawing.Size(148, 27)
		Me.lblCurrentADValX.TabIndex = 9
		Me.lblCurrentADValX.Text = "N A"
		Me.lblCurrentADValX.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		'
		'nudTimeSpan
		'
		Me.nudTimeSpan.Font = New System.Drawing.Font("Arial", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
		Me.nudTimeSpan.Increment = New Decimal(New Integer() {10, 0, 0, 0})
		Me.nudTimeSpan.Location = New System.Drawing.Point(604, 333)
		Me.nudTimeSpan.Maximum = New Decimal(New Integer() {1000, 0, 0, 0})
		Me.nudTimeSpan.Name = "nudTimeSpan"
		Me.nudTimeSpan.Size = New System.Drawing.Size(66, 26)
		Me.nudTimeSpan.TabIndex = 8
		Me.nudTimeSpan.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		Me.nudTimeSpan.Value = New Decimal(New Integer() {100, 0, 0, 0})
		'
		'lblSpan
		'
		Me.lblSpan.AutoSize = true
		Me.lblSpan.Font = New System.Drawing.Font("游ゴシック", 12!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.lblSpan.Location = New System.Drawing.Point(463, 336)
		Me.lblSpan.Name = "lblSpan"
		Me.lblSpan.Size = New System.Drawing.Size(141, 21)
		Me.lblSpan.TabIndex = 7
		Me.lblSpan.Text = "Time Span [Sec]"
		'
		'ChartAD
		'
		Me.ChartAD.Location = New System.Drawing.Point(10, 57)
		Me.ChartAD.Name = "ChartAD"
		Me.ChartAD.Size = New System.Drawing.Size(1126, 272)
		Me.ChartAD.TabIndex = 6
		Me.ChartAD.Text = "Chart1"
		'
		'lblADUnit
		'
		Me.lblADUnit.AutoSize = true
		Me.lblADUnit.Font = New System.Drawing.Font("游ゴシック", 12!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.lblADUnit.Location = New System.Drawing.Point(790, 23)
		Me.lblADUnit.Name = "lblADUnit"
		Me.lblADUnit.Size = New System.Drawing.Size(188, 21)
		Me.lblADUnit.TabIndex = 5
		Me.lblADUnit.Text = "Average value [AD dig]"
		'
		'lblMeasStress
		'
		Me.lblMeasStress.BackColor = System.Drawing.Color.White
		Me.lblMeasStress.Font = New System.Drawing.Font("Arial", 18!)
		Me.lblMeasStress.Location = New System.Drawing.Point(984, 20)
		Me.lblMeasStress.Name = "lblMeasStress"
		Me.lblMeasStress.Size = New System.Drawing.Size(148, 27)
		Me.lblMeasStress.TabIndex = 4
		Me.lblMeasStress.Text = "N A"
		Me.lblMeasStress.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		'
		'btnSub
		'
		Me.btnSub.Font = New System.Drawing.Font("游ゴシック", 12!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.btnSub.Location = New System.Drawing.Point(724, 16)
		Me.btnSub.Name = "btnSub"
		Me.btnSub.Size = New System.Drawing.Size(43, 34)
		Me.btnSub.TabIndex = 3
		Me.btnSub.Text = "-"
		Me.btnSub.UseVisualStyleBackColor = true
		'
		'btnAdd
		'
		Me.btnAdd.Font = New System.Drawing.Font("游ゴシック", 12!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.btnAdd.Location = New System.Drawing.Point(679, 16)
		Me.btnAdd.Name = "btnAdd"
		Me.btnAdd.Size = New System.Drawing.Size(43, 34)
		Me.btnAdd.TabIndex = 3
		Me.btnAdd.Text = "+"
		Me.btnAdd.UseVisualStyleBackColor = true
		'
		'btnExtract
		'
		Me.btnExtract.Font = New System.Drawing.Font("游ゴシック", 12!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.btnExtract.Location = New System.Drawing.Point(457, 16)
		Me.btnExtract.Name = "btnExtract"
		Me.btnExtract.Size = New System.Drawing.Size(85, 34)
		Me.btnExtract.TabIndex = 3
		Me.btnExtract.Text = "Extract"
		Me.btnExtract.UseVisualStyleBackColor = true
		'
		'lblCurrentAD
		'
		Me.lblCurrentAD.AutoSize = true
		Me.lblCurrentAD.Font = New System.Drawing.Font("游ゴシック", 12!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.lblCurrentAD.Location = New System.Drawing.Point(678, 336)
		Me.lblCurrentAD.Name = "lblCurrentAD"
		Me.lblCurrentAD.Size = New System.Drawing.Size(144, 21)
		Me.lblCurrentAD.TabIndex = 2
		Me.lblCurrentAD.Text = "Current AD Value"
		'
		'lblWeightUnit
		'
		Me.lblWeightUnit.AutoSize = true
		Me.lblWeightUnit.Font = New System.Drawing.Font("游ゴシック", 12!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.lblWeightUnit.Location = New System.Drawing.Point(190, 23)
		Me.lblWeightUnit.Name = "lblWeightUnit"
		Me.lblWeightUnit.Size = New System.Drawing.Size(133, 21)
		Me.lblWeightUnit.TabIndex = 2
		Me.lblWeightUnit.Text = "Total Weight [g]"
		'
		'nudWeightAdd
		'
		Me.nudWeightAdd.DecimalPlaces = 3
		Me.nudWeightAdd.Font = New System.Drawing.Font("Arial", 18!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
		Me.nudWeightAdd.ForeColor = System.Drawing.Color.Blue
		Me.nudWeightAdd.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
		Me.nudWeightAdd.Location = New System.Drawing.Point(546, 16)
		Me.nudWeightAdd.Maximum = New Decimal(New Integer() {99999, 0, 0, 65536})
		Me.nudWeightAdd.Minimum = New Decimal(New Integer() {99999, 0, 0, -2147418112})
		Me.nudWeightAdd.Name = "nudWeightAdd"
		Me.nudWeightAdd.Size = New System.Drawing.Size(127, 35)
		Me.nudWeightAdd.TabIndex = 1
		Me.nudWeightAdd.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		'
		'nudWeight
		'
		Me.nudWeight.DecimalPlaces = 3
		Me.nudWeight.Font = New System.Drawing.Font("Arial", 18!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
		Me.nudWeight.ForeColor = System.Drawing.Color.Blue
		Me.nudWeight.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
		Me.nudWeight.Location = New System.Drawing.Point(324, 16)
		Me.nudWeight.Maximum = New Decimal(New Integer() {99999, 0, 0, 65536})
		Me.nudWeight.Minimum = New Decimal(New Integer() {99999, 0, 0, -2147418112})
		Me.nudWeight.Name = "nudWeight"
		Me.nudWeight.Size = New System.Drawing.Size(127, 35)
		Me.nudWeight.TabIndex = 1
		Me.nudWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		'
		'btnYStart
		'
		Me.btnYStart.Font = New System.Drawing.Font("游ゴシック", 12!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.btnYStart.Location = New System.Drawing.Point(101, 16)
		Me.btnYStart.Name = "btnYStart"
		Me.btnYStart.Size = New System.Drawing.Size(85, 34)
		Me.btnYStart.TabIndex = 1
		Me.btnYStart.Text = "Y Start"
		Me.btnYStart.UseVisualStyleBackColor = true
		'
		'btnXStart
		'
		Me.btnXStart.Font = New System.Drawing.Font("游ゴシック", 12!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.btnXStart.Location = New System.Drawing.Point(10, 16)
		Me.btnXStart.Name = "btnXStart"
		Me.btnXStart.Size = New System.Drawing.Size(85, 34)
		Me.btnXStart.TabIndex = 0
		Me.btnXStart.Text = "X Start"
		Me.btnXStart.UseVisualStyleBackColor = true
		'
		'GroupBox1
		'
		Me.GroupBox1.Controls.Add(Me.btnYdgvClear)
		Me.GroupBox1.Controls.Add(Me.btnXdgvClear)
		Me.GroupBox1.Controls.Add(Me.Label2)
		Me.GroupBox1.Controls.Add(Me.Label1)
		Me.GroupBox1.Controls.Add(Me.GroupBox2)
		Me.GroupBox1.Controls.Add(Me.Label7)
		Me.GroupBox1.Controls.Add(Me.Label6)
		Me.GroupBox1.Controls.Add(Me.lblCoefY)
		Me.GroupBox1.Controls.Add(Me.lblCoefX)
		Me.GroupBox1.Controls.Add(Me.ResultChart)
		Me.GroupBox1.Controls.Add(Me.dgvYResult)
		Me.GroupBox1.Controls.Add(Me.dgvXResult)
		Me.GroupBox1.Location = New System.Drawing.Point(2, 385)
		Me.GroupBox1.Name = "GroupBox1"
		Me.GroupBox1.Size = New System.Drawing.Size(1142, 410)
		Me.GroupBox1.TabIndex = 1
		Me.GroupBox1.TabStop = false
		Me.GroupBox1.Text = "Result"
		'
		'btnYdgvClear
		'
		Me.btnYdgvClear.Font = New System.Drawing.Font("游ゴシック", 12!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.btnYdgvClear.Location = New System.Drawing.Point(221, 380)
		Me.btnYdgvClear.Name = "btnYdgvClear"
		Me.btnYdgvClear.Size = New System.Drawing.Size(139, 29)
		Me.btnYdgvClear.TabIndex = 17
		Me.btnYdgvClear.Text = "Clear"
		Me.btnYdgvClear.UseVisualStyleBackColor = true
		'
		'btnXdgvClear
		'
		Me.btnXdgvClear.Font = New System.Drawing.Font("游ゴシック", 12!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.btnXdgvClear.Location = New System.Drawing.Point(31, 380)
		Me.btnXdgvClear.Name = "btnXdgvClear"
		Me.btnXdgvClear.Size = New System.Drawing.Size(139, 29)
		Me.btnXdgvClear.TabIndex = 16
		Me.btnXdgvClear.Text = "Clear"
		Me.btnXdgvClear.UseVisualStyleBackColor = true
		'
		'Label2
		'
		Me.Label2.AutoSize = true
		Me.Label2.Font = New System.Drawing.Font("游ゴシック", 12!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.Label2.Location = New System.Drawing.Point(800, 17)
		Me.Label2.Name = "Label2"
		Me.Label2.Size = New System.Drawing.Size(64, 21)
		Me.Label2.TabIndex = 15
		Me.Label2.Text = "Y Coef."
		'
		'Label1
		'
		Me.Label1.AutoSize = true
		Me.Label1.Font = New System.Drawing.Font("游ゴシック", 12!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.Label1.Location = New System.Drawing.Point(523, 18)
		Me.Label1.Name = "Label1"
		Me.Label1.Size = New System.Drawing.Size(64, 21)
		Me.Label1.TabIndex = 14
		Me.Label1.Text = "X Coef."
		'
		'GroupBox2
		'
		Me.GroupBox2.Controls.Add(Me.rbY)
		Me.GroupBox2.Controls.Add(Me.rbX)
		Me.GroupBox2.Location = New System.Drawing.Point(398, 14)
		Me.GroupBox2.Name = "GroupBox2"
		Me.GroupBox2.Size = New System.Drawing.Size(101, 27)
		Me.GroupBox2.TabIndex = 12
		Me.GroupBox2.TabStop = false
		'
		'rbY
		'
		Me.rbY.AutoSize = true
		Me.rbY.Location = New System.Drawing.Point(52, 8)
		Me.rbY.Name = "rbY"
		Me.rbY.Size = New System.Drawing.Size(30, 16)
		Me.rbY.TabIndex = 0
		Me.rbY.TabStop = true
		Me.rbY.Text = "Y"
		Me.rbY.UseVisualStyleBackColor = true
		'
		'rbX
		'
		Me.rbX.AutoSize = true
		Me.rbX.Location = New System.Drawing.Point(6, 8)
		Me.rbX.Name = "rbX"
		Me.rbX.Size = New System.Drawing.Size(30, 16)
		Me.rbX.TabIndex = 0
		Me.rbX.TabStop = true
		Me.rbX.Text = "X"
		Me.rbX.UseVisualStyleBackColor = true
		'
		'Label7
		'
		Me.Label7.AutoSize = true
		Me.Label7.Font = New System.Drawing.Font("游ゴシック", 12!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.Label7.Location = New System.Drawing.Point(250, 15)
		Me.Label7.Name = "Label7"
		Me.Label7.Size = New System.Drawing.Size(76, 21)
		Me.Label7.TabIndex = 11
		Me.Label7.Text = "Y Result"
		'
		'Label6
		'
		Me.Label6.AutoSize = true
		Me.Label6.Font = New System.Drawing.Font("游ゴシック", 12!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.Label6.Location = New System.Drawing.Point(55, 14)
		Me.Label6.Name = "Label6"
		Me.Label6.Size = New System.Drawing.Size(76, 21)
		Me.Label6.TabIndex = 10
		Me.Label6.Text = "X Result"
		'
		'lblCoefY
		'
		Me.lblCoefY.BackColor = System.Drawing.Color.White
		Me.lblCoefY.Font = New System.Drawing.Font("Arial", 18!)
		Me.lblCoefY.Location = New System.Drawing.Point(870, 13)
		Me.lblCoefY.Name = "lblCoefY"
		Me.lblCoefY.Size = New System.Drawing.Size(205, 27)
		Me.lblCoefY.TabIndex = 6
		Me.lblCoefY.Text = "N A"
		Me.lblCoefY.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		'
		'lblCoefX
		'
		Me.lblCoefX.BackColor = System.Drawing.Color.White
		Me.lblCoefX.Font = New System.Drawing.Font("Arial", 18!)
		Me.lblCoefX.Location = New System.Drawing.Point(589, 13)
		Me.lblCoefX.Name = "lblCoefX"
		Me.lblCoefX.Size = New System.Drawing.Size(205, 27)
		Me.lblCoefX.TabIndex = 5
		Me.lblCoefX.Text = "N A"
		Me.lblCoefX.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		'
		'ResultChart
		'
		ChartArea2.Name = "ChartArea1"
		Me.ResultChart.ChartAreas.Add(ChartArea2)
		Me.ResultChart.Location = New System.Drawing.Point(398, 44)
		Me.ResultChart.Name = "ResultChart"
		Me.ResultChart.Size = New System.Drawing.Size(738, 360)
		Me.ResultChart.TabIndex = 2
		Me.ResultChart.Text = "Chart1"
		'
		'dgvYResult
		'
		Me.dgvYResult.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
		Me.dgvYResult.Location = New System.Drawing.Point(202, 44)
		Me.dgvYResult.Name = "dgvYResult"
		Me.dgvYResult.RowTemplate.Height = 21
		Me.dgvYResult.Size = New System.Drawing.Size(176, 332)
		Me.dgvYResult.TabIndex = 1
		'
		'dgvXResult
		'
		Me.dgvXResult.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
		Me.dgvXResult.Location = New System.Drawing.Point(10, 44)
		Me.dgvXResult.Name = "dgvXResult"
		Me.dgvXResult.RowTemplate.Height = 21
		Me.dgvXResult.Size = New System.Drawing.Size(176, 332)
		Me.dgvXResult.TabIndex = 0
		'
		'Timer1
		'
		'
		'cmsResultDgv
		'
		Me.cmsResultDgv.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.DeleteToolStripMenuItem})
		Me.cmsResultDgv.Name = "cmsResultDgv"
		Me.cmsResultDgv.Size = New System.Drawing.Size(108, 26)
		'
		'DeleteToolStripMenuItem
		'
		Me.DeleteToolStripMenuItem.Name = "DeleteToolStripMenuItem"
		Me.DeleteToolStripMenuItem.Size = New System.Drawing.Size(107, 22)
		Me.DeleteToolStripMenuItem.Text = "&Delete"
		'
		'ss1
		'
		Me.ss1.Location = New System.Drawing.Point(0, 797)
		Me.ss1.Name = "ss1"
		Me.ss1.Size = New System.Drawing.Size(1145, 22)
		Me.ss1.TabIndex = 2
		Me.ss1.Text = "StatusStrip1"
		'
		'frmCalibWeight
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6!, 12!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(1145, 819)
		Me.Controls.Add(Me.ss1)
		Me.Controls.Add(Me.GroupBox1)
		Me.Controls.Add(Me.gbxMeasure)
		Me.DoubleBuffered = true
		Me.MaximizeBox = false
		Me.Name = "frmCalibWeight"
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
		Me.Text = "Calibration(Weight)"
		Me.gbxMeasure.ResumeLayout(false)
		Me.gbxMeasure.PerformLayout
		CType(Me.nudYSpanMax,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.nudYSpanMin,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.nudTimeSpan,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.ChartAD,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.nudWeightAdd,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.nudWeight,System.ComponentModel.ISupportInitialize).EndInit
		Me.GroupBox1.ResumeLayout(false)
		Me.GroupBox1.PerformLayout
		Me.GroupBox2.ResumeLayout(false)
		Me.GroupBox2.PerformLayout
		CType(Me.ResultChart,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.dgvYResult,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.dgvXResult,System.ComponentModel.ISupportInitialize).EndInit
		Me.cmsResultDgv.ResumeLayout(false)
		Me.ResumeLayout(false)
		Me.PerformLayout

End Sub

	Friend WithEvents gbxMeasure As GroupBox
	Friend WithEvents nudTimeSpan As NumericUpDown
	Friend WithEvents lblSpan As Label
	Friend WithEvents ChartAD As DataVisualization.Charting.Chart
	Friend WithEvents lblADUnit As Label
	Friend WithEvents lblMeasStress As Label
	Friend WithEvents btnExtract As Button
	Friend WithEvents lblWeightUnit As Label
	Friend WithEvents nudWeight As NumericUpDown
	Friend WithEvents btnYStart As Button
	Friend WithEvents btnXStart As Button
	Friend WithEvents GroupBox1 As GroupBox
	Friend WithEvents Label7 As Label
	Friend WithEvents Label6 As Label
	Friend WithEvents lblCoefY As Label
	Friend WithEvents lblCoefX As Label
	Friend WithEvents ResultChart As DataVisualization.Charting.Chart
	Friend WithEvents dgvYResult As DataGridView
	Friend WithEvents dgvXResult As DataGridView
	Friend WithEvents lblCurrentADValX As Label
	Friend WithEvents lblCurrentAD As Label
	Friend WithEvents Timer1 As Timer
	Friend WithEvents nudYSpanMax As NumericUpDown
	Friend WithEvents nudYSpanMin As NumericUpDown
	Friend WithEvents lblYSpan As Label
	Friend WithEvents btnYSpanAuto As Button
	Friend WithEvents GroupBox2 As GroupBox
	Friend WithEvents rbY As RadioButton
	Friend WithEvents rbX As RadioButton
	Friend WithEvents cmsResultDgv As ContextMenuStrip
	Friend WithEvents DeleteToolStripMenuItem As ToolStripMenuItem
	Friend WithEvents SaveFileDialog As SaveFileDialog
	Friend WithEvents Label2 As Label
	Friend WithEvents Label1 As Label
	Friend WithEvents lblCurrentADValY As Label
	Friend WithEvents btnYdgvClear As Button
	Friend WithEvents btnXdgvClear As Button
	Friend WithEvents ss1 As StatusStrip
	Friend WithEvents btnAdd As Button
	Friend WithEvents nudWeightAdd As NumericUpDown
	Friend WithEvents btnSub As Button
End Class
