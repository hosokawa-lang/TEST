<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmSPC
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
		Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
		Dim ChartArea1 As System.Windows.Forms.DataVisualization.Charting.ChartArea = New System.Windows.Forms.DataVisualization.Charting.ChartArea()
		Dim Legend1 As System.Windows.Forms.DataVisualization.Charting.Legend = New System.Windows.Forms.DataVisualization.Charting.Legend()
		Dim Series1 As System.Windows.Forms.DataVisualization.Charting.Series = New System.Windows.Forms.DataVisualization.Charting.Series()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmSPC))
		Me.dgvSPCList = New System.Windows.Forms.DataGridView()
		Me.Chart1 = New System.Windows.Forms.DataVisualization.Charting.Chart()
		Me.btnAddValue = New System.Windows.Forms.Button()
		Me.btnCalc = New System.Windows.Forms.Button()
		Me.txtValue = New System.Windows.Forms.TextBox()
		Me.txtFirstN = New System.Windows.Forms.TextBox()
		Me.btnClear = New System.Windows.Forms.Button()
		Me.btnAddAll = New System.Windows.Forms.Button()
		Me.nudSampStart = New System.Windows.Forms.NumericUpDown()
		Me.nudSampEnd = New System.Windows.Forms.NumericUpDown()
		Me.lblDispRange = New System.Windows.Forms.Label()
		Me.btnExport = New System.Windows.Forms.Button()
		Me.gpxStatiisticalRange = New System.Windows.Forms.GroupBox()
		Me.lblStatEnd = New System.Windows.Forms.Label()
		Me.dtpStatMax = New System.Windows.Forms.DateTimePicker()
		Me.dtpStatMin = New System.Windows.Forms.DateTimePicker()
		Me.lblStatStart = New System.Windows.Forms.Label()
		Me.ckbStatRange = New System.Windows.Forms.CheckBox()
		CType(Me.dgvSPCList,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.Chart1,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.nudSampStart,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.nudSampEnd,System.ComponentModel.ISupportInitialize).BeginInit
		Me.gpxStatiisticalRange.SuspendLayout
		Me.SuspendLayout
		'
		'dgvSPCList
		'
		Me.dgvSPCList.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight
		DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
		DataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(CType(CType(255,Byte),Integer), CType(CType(255,Byte),Integer), CType(CType(192,Byte),Integer))
		DataGridViewCellStyle1.Font = New System.Drawing.Font("Meiryo UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText
		DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
		DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
		DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
		Me.dgvSPCList.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
		Me.dgvSPCList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
		Me.dgvSPCList.Location = New System.Drawing.Point(11, 17)
		Me.dgvSPCList.Margin = New System.Windows.Forms.Padding(4)
		Me.dgvSPCList.Name = "dgvSPCList"
		Me.dgvSPCList.Size = New System.Drawing.Size(284, 498)
		Me.dgvSPCList.TabIndex = 0
		'
		'Chart1
		'
		ChartArea1.Name = "ChartArea1"
		Me.Chart1.ChartAreas.Add(ChartArea1)
		Legend1.Name = "Legend1"
		Me.Chart1.Legends.Add(Legend1)
		Me.Chart1.Location = New System.Drawing.Point(303, 17)
		Me.Chart1.Margin = New System.Windows.Forms.Padding(4)
		Me.Chart1.Name = "Chart1"
		Series1.ChartArea = "ChartArea1"
		Series1.Legend = "Legend1"
		Series1.Name = "Series1"
		Me.Chart1.Series.Add(Series1)
		Me.Chart1.Size = New System.Drawing.Size(948, 669)
		Me.Chart1.TabIndex = 1
		Me.Chart1.Text = "Chart1"
		'
		'btnAddValue
		'
		Me.btnAddValue.Location = New System.Drawing.Point(158, 767)
		Me.btnAddValue.Margin = New System.Windows.Forms.Padding(4)
		Me.btnAddValue.Name = "btnAddValue"
		Me.btnAddValue.Size = New System.Drawing.Size(131, 33)
		Me.btnAddValue.TabIndex = 2
		Me.btnAddValue.TabStop = false
		Me.btnAddValue.Text = "btnAddValue"
		Me.btnAddValue.UseVisualStyleBackColor = true
		'
		'btnCalc
		'
		Me.btnCalc.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left),System.Windows.Forms.AnchorStyles)
		Me.btnCalc.Location = New System.Drawing.Point(173, 652)
		Me.btnCalc.Margin = New System.Windows.Forms.Padding(4)
		Me.btnCalc.Name = "btnCalc"
		Me.btnCalc.Size = New System.Drawing.Size(122, 33)
		Me.btnCalc.TabIndex = 2
		Me.btnCalc.Text = "&Update"
		Me.btnCalc.UseVisualStyleBackColor = true
		'
		'txtValue
		'
		Me.txtValue.Font = New System.Drawing.Font("Meiryo UI", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.txtValue.Location = New System.Drawing.Point(18, 772)
		Me.txtValue.Margin = New System.Windows.Forms.Padding(4)
		Me.txtValue.Name = "txtValue"
		Me.txtValue.Size = New System.Drawing.Size(132, 28)
		Me.txtValue.TabIndex = 3
		Me.txtValue.TabStop = false
		'
		'txtFirstN
		'
		Me.txtFirstN.Location = New System.Drawing.Point(436, 772)
		Me.txtFirstN.Margin = New System.Windows.Forms.Padding(4)
		Me.txtFirstN.Name = "txtFirstN"
		Me.txtFirstN.Size = New System.Drawing.Size(76, 24)
		Me.txtFirstN.TabIndex = 3
		Me.txtFirstN.TabStop = false
		Me.txtFirstN.Text = "10"
		'
		'btnClear
		'
		Me.btnClear.Location = New System.Drawing.Point(834, 763)
		Me.btnClear.Margin = New System.Windows.Forms.Padding(4)
		Me.btnClear.Name = "btnClear"
		Me.btnClear.Size = New System.Drawing.Size(131, 33)
		Me.btnClear.TabIndex = 2
		Me.btnClear.TabStop = false
		Me.btnClear.Text = "&Clear"
		Me.btnClear.UseVisualStyleBackColor = true
		'
		'btnAddAll
		'
		Me.btnAddAll.Location = New System.Drawing.Point(297, 767)
		Me.btnAddAll.Margin = New System.Windows.Forms.Padding(4)
		Me.btnAddAll.Name = "btnAddAll"
		Me.btnAddAll.Size = New System.Drawing.Size(131, 33)
		Me.btnAddAll.TabIndex = 2
		Me.btnAddAll.TabStop = false
		Me.btnAddAll.Text = "btnAddAll"
		Me.btnAddAll.UseVisualStyleBackColor = true
		'
		'nudSampStart
		'
		Me.nudSampStart.Location = New System.Drawing.Point(167, 522)
		Me.nudSampStart.Maximum = New Decimal(New Integer() {999, 0, 0, 0})
		Me.nudSampStart.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
		Me.nudSampStart.Name = "nudSampStart"
		Me.nudSampStart.Size = New System.Drawing.Size(57, 24)
		Me.nudSampStart.TabIndex = 4
		Me.nudSampStart.Value = New Decimal(New Integer() {1, 0, 0, 0})
		'
		'nudSampEnd
		'
		Me.nudSampEnd.Location = New System.Drawing.Point(239, 522)
		Me.nudSampEnd.Maximum = New Decimal(New Integer() {999, 0, 0, 0})
		Me.nudSampEnd.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
		Me.nudSampEnd.Name = "nudSampEnd"
		Me.nudSampEnd.Size = New System.Drawing.Size(56, 24)
		Me.nudSampEnd.TabIndex = 4
		Me.nudSampEnd.Value = New Decimal(New Integer() {100, 0, 0, 0})
		'
		'lblDispRange
		'
		Me.lblDispRange.AutoSize = true
		Me.lblDispRange.Location = New System.Drawing.Point(17, 524)
		Me.lblDispRange.Name = "lblDispRange"
		Me.lblDispRange.Size = New System.Drawing.Size(94, 17)
		Me.lblDispRange.TabIndex = 5
		Me.lblDispRange.Text = "Display range"
		'
		'btnExport
		'
		Me.btnExport.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left),System.Windows.Forms.AnchorStyles)
		Me.btnExport.Location = New System.Drawing.Point(11, 652)
		Me.btnExport.Margin = New System.Windows.Forms.Padding(4)
		Me.btnExport.Name = "btnExport"
		Me.btnExport.Size = New System.Drawing.Size(122, 33)
		Me.btnExport.TabIndex = 2
		Me.btnExport.Text = "&Export"
		Me.btnExport.UseVisualStyleBackColor = true
		Me.btnExport.Visible = false
		'
		'gpxStatiisticalRange
		'
		Me.gpxStatiisticalRange.Controls.Add(Me.lblStatEnd)
		Me.gpxStatiisticalRange.Controls.Add(Me.dtpStatMax)
		Me.gpxStatiisticalRange.Controls.Add(Me.dtpStatMin)
		Me.gpxStatiisticalRange.Controls.Add(Me.lblStatStart)
		Me.gpxStatiisticalRange.Location = New System.Drawing.Point(12, 571)
		Me.gpxStatiisticalRange.Name = "gpxStatiisticalRange"
		Me.gpxStatiisticalRange.Size = New System.Drawing.Size(284, 79)
		Me.gpxStatiisticalRange.TabIndex = 16
		Me.gpxStatiisticalRange.TabStop = false
		Me.gpxStatiisticalRange.Text = "Statiistical Range"
		'
		'lblStatEnd
		'
		Me.lblStatEnd.AutoSize = true
		Me.lblStatEnd.Location = New System.Drawing.Point(9, 51)
		Me.lblStatEnd.Name = "lblStatEnd"
		Me.lblStatEnd.Size = New System.Drawing.Size(32, 17)
		Me.lblStatEnd.TabIndex = 1
		Me.lblStatEnd.Text = "End"
		'
		'dtpStatMax
		'
		Me.dtpStatMax.CustomFormat = "yyyy/MM/dd HH:mm"
		Me.dtpStatMax.Format = System.Windows.Forms.DateTimePickerFormat.Custom
		Me.dtpStatMax.Location = New System.Drawing.Point(121, 49)
		Me.dtpStatMax.MaxDate = New Date(2200, 12, 31, 0, 0, 0, 0)
		Me.dtpStatMax.MinDate = New Date(2020, 1, 1, 0, 0, 0, 0)
		Me.dtpStatMax.Name = "dtpStatMax"
		Me.dtpStatMax.Size = New System.Drawing.Size(155, 24)
		Me.dtpStatMax.TabIndex = 0
		'
		'dtpStatMin
		'
		Me.dtpStatMin.CustomFormat = "yyyy/MM/dd HH:mm"
		Me.dtpStatMin.Format = System.Windows.Forms.DateTimePickerFormat.Custom
		Me.dtpStatMin.Location = New System.Drawing.Point(121, 20)
		Me.dtpStatMin.MaxDate = New Date(2200, 12, 31, 0, 0, 0, 0)
		Me.dtpStatMin.MinDate = New Date(2020, 1, 1, 0, 0, 0, 0)
		Me.dtpStatMin.Name = "dtpStatMin"
		Me.dtpStatMin.Size = New System.Drawing.Size(156, 24)
		Me.dtpStatMin.TabIndex = 0
		'
		'lblStatStart
		'
		Me.lblStatStart.AutoSize = true
		Me.lblStatStart.Location = New System.Drawing.Point(8, 24)
		Me.lblStatStart.Name = "lblStatStart"
		Me.lblStatStart.Size = New System.Drawing.Size(39, 17)
		Me.lblStatStart.TabIndex = 1
		Me.lblStatStart.Text = "Start"
		'
		'ckbStatRange
		'
		Me.ckbStatRange.AutoSize = true
		Me.ckbStatRange.Location = New System.Drawing.Point(20, 549)
		Me.ckbStatRange.Name = "ckbStatRange"
		Me.ckbStatRange.Size = New System.Drawing.Size(159, 21)
		Me.ckbStatRange.TabIndex = 17
		Me.ckbStatRange.Text = " All Statiistical Range"
		Me.ckbStatRange.UseVisualStyleBackColor = true
		'
		'frmSPC
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(8!, 17!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(1256, 696)
		Me.Controls.Add(Me.ckbStatRange)
		Me.Controls.Add(Me.gpxStatiisticalRange)
		Me.Controls.Add(Me.lblDispRange)
		Me.Controls.Add(Me.nudSampEnd)
		Me.Controls.Add(Me.nudSampStart)
		Me.Controls.Add(Me.txtFirstN)
		Me.Controls.Add(Me.txtValue)
		Me.Controls.Add(Me.btnClear)
		Me.Controls.Add(Me.btnExport)
		Me.Controls.Add(Me.btnCalc)
		Me.Controls.Add(Me.btnAddAll)
		Me.Controls.Add(Me.btnAddValue)
		Me.Controls.Add(Me.Chart1)
		Me.Controls.Add(Me.dgvSPCList)
		Me.Font = New System.Drawing.Font("Meiryo UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
		Me.Icon = CType(resources.GetObject("$this.Icon"),System.Drawing.Icon)
		Me.Margin = New System.Windows.Forms.Padding(4)
		Me.MaximizeBox = false
		Me.Name = "frmSPC"
		Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
		Me.Text = "frmSPC"
		CType(Me.dgvSPCList,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.Chart1,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.nudSampStart,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.nudSampEnd,System.ComponentModel.ISupportInitialize).EndInit
		Me.gpxStatiisticalRange.ResumeLayout(false)
		Me.gpxStatiisticalRange.PerformLayout
		Me.ResumeLayout(false)
		Me.PerformLayout

End Sub

	Friend WithEvents dgvSPCList As DataGridView
	Friend WithEvents Chart1 As DataVisualization.Charting.Chart
	Friend WithEvents btnAddValue As Button
	Friend WithEvents btnCalc As Button
	Friend WithEvents txtValue As TextBox
	Friend WithEvents txtFirstN As TextBox
	Friend WithEvents btnClear As Button
	Friend WithEvents btnAddAll As Button
	Friend WithEvents nudSampStart As NumericUpDown
	Friend WithEvents nudSampEnd As NumericUpDown
	Friend WithEvents lblDispRange As Label
	Friend WithEvents btnExport As Button
	Friend WithEvents gpxStatiisticalRange As GroupBox
	Friend WithEvents dtpStatMax As DateTimePicker
	Friend WithEvents dtpStatMin As DateTimePicker
	Friend WithEvents lblStatEnd As Label
	Friend WithEvents lblStatStart As Label
	Friend WithEvents ckbStatRange As CheckBox
End Class
