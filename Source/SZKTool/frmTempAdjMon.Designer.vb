<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmTempAdjMon
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
		Me.ChartHP = New System.Windows.Forms.DataVisualization.Charting.Chart()
		Me.ChartPB = New System.Windows.Forms.DataVisualization.Charting.Chart()
		Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
		Me.tbcTemp = New System.Windows.Forms.TabControl()
		Me.TabPage1 = New System.Windows.Forms.TabPage()
		Me.Label2 = New System.Windows.Forms.Label()
		Me.lblTemp2 = New System.Windows.Forms.Label()
		Me.lblMv2 = New System.Windows.Forms.Label()
		Me.Label13 = New System.Windows.Forms.Label()
		Me.Label1 = New System.Windows.Forms.Label()
		Me.lbldmv2 = New System.Windows.Forms.Label()
		Me.lbldn2 = New System.Windows.Forms.Label()
		Me.Label12 = New System.Windows.Forms.Label()
		Me.Label9 = New System.Windows.Forms.Label()
		Me.lblin2 = New System.Windows.Forms.Label()
		Me.ChartPBTemp = New System.Windows.Forms.DataVisualization.Charting.Chart()
		Me.Label10 = New System.Windows.Forms.Label()
		Me.lblpn2 = New System.Windows.Forms.Label()
		Me.TabPage2 = New System.Windows.Forms.TabPage()
		Me.Label11 = New System.Windows.Forms.Label()
		Me.Label14 = New System.Windows.Forms.Label()
		Me.lblMv = New System.Windows.Forms.Label()
		Me.lblTemp = New System.Windows.Forms.Label()
		Me.Label8 = New System.Windows.Forms.Label()
		Me.Label17 = New System.Windows.Forms.Label()
		Me.Label18 = New System.Windows.Forms.Label()
		Me.lbldn = New System.Windows.Forms.Label()
		Me.lbldmv = New System.Windows.Forms.Label()
		Me.lblin = New System.Windows.Forms.Label()
		Me.lblpn = New System.Windows.Forms.Label()
		Me.ChartHPTemp = New System.Windows.Forms.DataVisualization.Charting.Chart()
		Me.Label16 = New System.Windows.Forms.Label()
		Me.TabPage3 = New System.Windows.Forms.TabPage()
		Me.Label20 = New System.Windows.Forms.Label()
		Me.Label21 = New System.Windows.Forms.Label()
		Me.lblMv3 = New System.Windows.Forms.Label()
		Me.lblTemp3 = New System.Windows.Forms.Label()
		Me.Label24 = New System.Windows.Forms.Label()
		Me.Label25 = New System.Windows.Forms.Label()
		Me.Label26 = New System.Windows.Forms.Label()
		Me.lbldn3 = New System.Windows.Forms.Label()
		Me.lbldmv3 = New System.Windows.Forms.Label()
		Me.lblin3 = New System.Windows.Forms.Label()
		Me.lblpn3 = New System.Windows.Forms.Label()
		Me.ChartSGTemp = New System.Windows.Forms.DataVisualization.Charting.Chart()
		Me.Label31 = New System.Windows.Forms.Label()
		Me.ChartSG = New System.Windows.Forms.DataVisualization.Charting.Chart()
		Me.nudXSpan = New System.Windows.Forms.NumericUpDown()
		Me.gbxProbe = New System.Windows.Forms.GroupBox()
		Me.nudPVGain2 = New System.Windows.Forms.NumericUpDown()
		Me.lblPVGain2 = New System.Windows.Forms.Label()
		Me.nudPVBias2 = New System.Windows.Forms.NumericUpDown()
		Me.nudDgain2 = New System.Windows.Forms.NumericUpDown()
		Me.nudD2 = New System.Windows.Forms.NumericUpDown()
		Me.nudI2 = New System.Windows.Forms.NumericUpDown()
		Me.nudP2 = New System.Windows.Forms.NumericUpDown()
		Me.lblD2 = New System.Windows.Forms.Label()
		Me.lblI2 = New System.Windows.Forms.Label()
		Me.lblPVBias2 = New System.Windows.Forms.Label()
		Me.lblDgain2 = New System.Windows.Forms.Label()
		Me.lblP2 = New System.Windows.Forms.Label()
		Me.gbxHP = New System.Windows.Forms.GroupBox()
		Me.nudPVGain = New System.Windows.Forms.NumericUpDown()
		Me.lblPVGain = New System.Windows.Forms.Label()
		Me.nudPVBias = New System.Windows.Forms.NumericUpDown()
		Me.nudDgain = New System.Windows.Forms.NumericUpDown()
		Me.nudD = New System.Windows.Forms.NumericUpDown()
		Me.nudI = New System.Windows.Forms.NumericUpDown()
		Me.nudP = New System.Windows.Forms.NumericUpDown()
		Me.lblD = New System.Windows.Forms.Label()
		Me.lblI = New System.Windows.Forms.Label()
		Me.lblPVBias = New System.Windows.Forms.Label()
		Me.lblDgain = New System.Windows.Forms.Label()
		Me.lblP = New System.Windows.Forms.Label()
		Me.btnGraphReset = New System.Windows.Forms.Button()
		Me.btnMonEnable = New System.Windows.Forms.Button()
		Me.btnHeaterContPB = New System.Windows.Forms.Button()
		Me.lblXSpan = New System.Windows.Forms.Label()
		Me.lblYMax = New System.Windows.Forms.Label()
		Me.lblYMin = New System.Windows.Forms.Label()
		Me.nudYMax = New System.Windows.Forms.NumericUpDown()
		Me.nudYMin = New System.Windows.Forms.NumericUpDown()
		Me.GroupBox1 = New System.Windows.Forms.GroupBox()
		Me.btnHeaterContHP = New System.Windows.Forms.Button()
		Me.nudPBTemp = New System.Windows.Forms.NumericUpDown()
		Me.nudHPTemp = New System.Windows.Forms.NumericUpDown()
		Me.lblTempPBPreset = New System.Windows.Forms.Label()
		Me.lblTempHPPreset = New System.Windows.Forms.Label()
		Me.gbxSG = New System.Windows.Forms.GroupBox()
		Me.nudPVGain3 = New System.Windows.Forms.NumericUpDown()
		Me.lblPVGain3 = New System.Windows.Forms.Label()
		Me.nudPVBias3 = New System.Windows.Forms.NumericUpDown()
		Me.nudDgain3 = New System.Windows.Forms.NumericUpDown()
		Me.nudD3 = New System.Windows.Forms.NumericUpDown()
		Me.nudI3 = New System.Windows.Forms.NumericUpDown()
		Me.nudP3 = New System.Windows.Forms.NumericUpDown()
		Me.lblD3 = New System.Windows.Forms.Label()
		Me.lblI3 = New System.Windows.Forms.Label()
		Me.lblPVBias3 = New System.Windows.Forms.Label()
		Me.lblDgain3 = New System.Windows.Forms.Label()
		Me.lblP3 = New System.Windows.Forms.Label()
		Me.lblTempSGPreset = New System.Windows.Forms.Label()
		Me.nudSGTemp = New System.Windows.Forms.NumericUpDown()
		Me.btnHeaterContSG = New System.Windows.Forms.Button()
		CType(Me.ChartHP,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.ChartPB,System.ComponentModel.ISupportInitialize).BeginInit
		Me.tbcTemp.SuspendLayout
		Me.TabPage1.SuspendLayout
		CType(Me.ChartPBTemp,System.ComponentModel.ISupportInitialize).BeginInit
		Me.TabPage2.SuspendLayout
		CType(Me.ChartHPTemp,System.ComponentModel.ISupportInitialize).BeginInit
		Me.TabPage3.SuspendLayout
		CType(Me.ChartSGTemp,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.ChartSG,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.nudXSpan,System.ComponentModel.ISupportInitialize).BeginInit
		Me.gbxProbe.SuspendLayout
		CType(Me.nudPVGain2,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.nudPVBias2,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.nudDgain2,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.nudD2,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.nudI2,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.nudP2,System.ComponentModel.ISupportInitialize).BeginInit
		Me.gbxHP.SuspendLayout
		CType(Me.nudPVGain,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.nudPVBias,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.nudDgain,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.nudD,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.nudI,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.nudP,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.nudYMax,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.nudYMin,System.ComponentModel.ISupportInitialize).BeginInit
		Me.GroupBox1.SuspendLayout
		CType(Me.nudPBTemp,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.nudHPTemp,System.ComponentModel.ISupportInitialize).BeginInit
		Me.gbxSG.SuspendLayout
		CType(Me.nudPVGain3,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.nudPVBias3,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.nudDgain3,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.nudD3,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.nudI3,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.nudP3,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.nudSGTemp,System.ComponentModel.ISupportInitialize).BeginInit
		Me.SuspendLayout
		'
		'ChartHP
		'
		Me.ChartHP.BorderlineColor = System.Drawing.Color.DarkGray
		Me.ChartHP.BorderlineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Solid
		Me.ChartHP.BorderlineWidth = 2
		Me.ChartHP.Location = New System.Drawing.Point(6, 6)
		Me.ChartHP.Name = "ChartHP"
		Me.ChartHP.Size = New System.Drawing.Size(1067, 372)
		Me.ChartHP.TabIndex = 59
		Me.ChartHP.Text = "Chart2"
		'
		'ChartPB
		'
		Me.ChartPB.BorderlineColor = System.Drawing.Color.DarkGray
		Me.ChartPB.BorderlineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Solid
		Me.ChartPB.BorderlineWidth = 2
		Me.ChartPB.Location = New System.Drawing.Point(6, 6)
		Me.ChartPB.Name = "ChartPB"
		Me.ChartPB.Size = New System.Drawing.Size(1067, 372)
		Me.ChartPB.TabIndex = 60
		Me.ChartPB.Text = "Chart2"
		'
		'Timer1
		'
		'
		'tbcTemp
		'
		Me.tbcTemp.Controls.Add(Me.TabPage1)
		Me.tbcTemp.Controls.Add(Me.TabPage2)
		Me.tbcTemp.Controls.Add(Me.TabPage3)
		Me.tbcTemp.Font = New System.Drawing.Font("Meiryo UI", 9!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.tbcTemp.Location = New System.Drawing.Point(2, 2)
		Me.tbcTemp.Name = "tbcTemp"
		Me.tbcTemp.SelectedIndex = 0
		Me.tbcTemp.Size = New System.Drawing.Size(1087, 675)
		Me.tbcTemp.TabIndex = 61
		'
		'TabPage1
		'
		Me.TabPage1.Controls.Add(Me.Label2)
		Me.TabPage1.Controls.Add(Me.lblTemp2)
		Me.TabPage1.Controls.Add(Me.lblMv2)
		Me.TabPage1.Controls.Add(Me.Label13)
		Me.TabPage1.Controls.Add(Me.Label1)
		Me.TabPage1.Controls.Add(Me.lbldmv2)
		Me.TabPage1.Controls.Add(Me.lbldn2)
		Me.TabPage1.Controls.Add(Me.Label12)
		Me.TabPage1.Controls.Add(Me.Label9)
		Me.TabPage1.Controls.Add(Me.lblin2)
		Me.TabPage1.Controls.Add(Me.ChartPBTemp)
		Me.TabPage1.Controls.Add(Me.Label10)
		Me.TabPage1.Controls.Add(Me.lblpn2)
		Me.TabPage1.Controls.Add(Me.ChartPB)
		Me.TabPage1.Location = New System.Drawing.Point(4, 24)
		Me.TabPage1.Name = "TabPage1"
		Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
		Me.TabPage1.Size = New System.Drawing.Size(1079, 647)
		Me.TabPage1.TabIndex = 0
		Me.TabPage1.Text = "Probe Temperature"
		Me.TabPage1.UseVisualStyleBackColor = true
		'
		'Label2
		'
		Me.Label2.AutoSize = true
		Me.Label2.Font = New System.Drawing.Font("游ゴシック", 12!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.Label2.ForeColor = System.Drawing.Color.Green
		Me.Label2.Location = New System.Drawing.Point(244, 408)
		Me.Label2.Name = "Label2"
		Me.Label2.Size = New System.Drawing.Size(39, 21)
		Me.Label2.TabIndex = 62
		Me.Label2.Text = "Mv:"
		'
		'lblTemp2
		'
		Me.lblTemp2.AutoSize = true
		Me.lblTemp2.Font = New System.Drawing.Font("游ゴシック", 18!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.lblTemp2.Location = New System.Drawing.Point(165, 400)
		Me.lblTemp2.Name = "lblTemp2"
		Me.lblTemp2.Size = New System.Drawing.Size(49, 31)
		Me.lblTemp2.TabIndex = 61
		Me.lblTemp2.Text = "0.0"
		'
		'lblMv2
		'
		Me.lblMv2.AutoSize = true
		Me.lblMv2.Font = New System.Drawing.Font("游ゴシック", 18!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.lblMv2.Location = New System.Drawing.Point(285, 400)
		Me.lblMv2.Name = "lblMv2"
		Me.lblMv2.Size = New System.Drawing.Size(49, 31)
		Me.lblMv2.TabIndex = 61
		Me.lblMv2.Text = "0.0"
		'
		'Label13
		'
		Me.Label13.AutoSize = true
		Me.Label13.Font = New System.Drawing.Font("游ゴシック", 12!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.Label13.ForeColor = System.Drawing.Color.Magenta
		Me.Label13.Location = New System.Drawing.Point(504, 66)
		Me.Label13.Name = "Label13"
		Me.Label13.Size = New System.Drawing.Size(35, 21)
		Me.Label13.TabIndex = 62
		Me.Label13.Text = "dn:"
		'
		'Label1
		'
		Me.Label1.AutoSize = true
		Me.Label1.Font = New System.Drawing.Font("游ゴシック", 12!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.Label1.ForeColor = System.Drawing.Color.Blue
		Me.Label1.Location = New System.Drawing.Point(110, 408)
		Me.Label1.Name = "Label1"
		Me.Label1.Size = New System.Drawing.Size(58, 21)
		Me.Label1.TabIndex = 62
		Me.Label1.Text = "Temp:"
		'
		'lbldmv2
		'
		Me.lbldmv2.AutoSize = true
		Me.lbldmv2.Font = New System.Drawing.Font("游ゴシック", 18!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.lbldmv2.Location = New System.Drawing.Point(153, 59)
		Me.lbldmv2.Name = "lbldmv2"
		Me.lbldmv2.Size = New System.Drawing.Size(28, 31)
		Me.lbldmv2.TabIndex = 61
		Me.lbldmv2.Text = "0"
		'
		'lbldn2
		'
		Me.lbldn2.AutoSize = true
		Me.lbldn2.Font = New System.Drawing.Font("游ゴシック", 18!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.lbldn2.Location = New System.Drawing.Point(536, 59)
		Me.lbldn2.Name = "lbldn2"
		Me.lbldn2.Size = New System.Drawing.Size(28, 31)
		Me.lbldn2.TabIndex = 61
		Me.lbldn2.Text = "0"
		'
		'Label12
		'
		Me.Label12.AutoSize = true
		Me.Label12.Font = New System.Drawing.Font("游ゴシック", 12!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.Label12.ForeColor = System.Drawing.Color.Red
		Me.Label12.Location = New System.Drawing.Point(376, 66)
		Me.Label12.Name = "Label12"
		Me.Label12.Size = New System.Drawing.Size(30, 21)
		Me.Label12.TabIndex = 62
		Me.Label12.Text = "in:"
		'
		'Label9
		'
		Me.Label9.AutoSize = true
		Me.Label9.Font = New System.Drawing.Font("游ゴシック", 12!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.Label9.ForeColor = System.Drawing.Color.Green
		Me.Label9.Location = New System.Drawing.Point(241, 66)
		Me.Label9.Name = "Label9"
		Me.Label9.Size = New System.Drawing.Size(35, 21)
		Me.Label9.TabIndex = 62
		Me.Label9.Text = "pn:"
		'
		'lblin2
		'
		Me.lblin2.AutoSize = true
		Me.lblin2.Font = New System.Drawing.Font("游ゴシック", 18!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.lblin2.Location = New System.Drawing.Point(401, 59)
		Me.lblin2.Name = "lblin2"
		Me.lblin2.Size = New System.Drawing.Size(28, 31)
		Me.lblin2.TabIndex = 61
		Me.lblin2.Text = "0"
		'
		'ChartPBTemp
		'
		Me.ChartPBTemp.BorderlineColor = System.Drawing.Color.DarkGray
		Me.ChartPBTemp.BorderlineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Solid
		Me.ChartPBTemp.BorderlineWidth = 2
		Me.ChartPBTemp.Location = New System.Drawing.Point(7, 381)
		Me.ChartPBTemp.Name = "ChartPBTemp"
		Me.ChartPBTemp.Size = New System.Drawing.Size(1067, 261)
		Me.ChartPBTemp.TabIndex = 60
		Me.ChartPBTemp.Text = "Chart2"
		'
		'Label10
		'
		Me.Label10.AutoSize = true
		Me.Label10.Font = New System.Drawing.Font("游ゴシック", 12!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.Label10.ForeColor = System.Drawing.Color.Blue
		Me.Label10.Location = New System.Drawing.Point(107, 66)
		Me.Label10.Name = "Label10"
		Me.Label10.Size = New System.Drawing.Size(49, 21)
		Me.Label10.TabIndex = 62
		Me.Label10.Text = "dmv:"
		'
		'lblpn2
		'
		Me.lblpn2.AutoSize = true
		Me.lblpn2.Font = New System.Drawing.Font("游ゴシック", 18!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.lblpn2.Location = New System.Drawing.Point(273, 58)
		Me.lblpn2.Name = "lblpn2"
		Me.lblpn2.Size = New System.Drawing.Size(28, 31)
		Me.lblpn2.TabIndex = 61
		Me.lblpn2.Text = "0"
		'
		'TabPage2
		'
		Me.TabPage2.Controls.Add(Me.Label11)
		Me.TabPage2.Controls.Add(Me.Label14)
		Me.TabPage2.Controls.Add(Me.lblMv)
		Me.TabPage2.Controls.Add(Me.lblTemp)
		Me.TabPage2.Controls.Add(Me.Label8)
		Me.TabPage2.Controls.Add(Me.Label17)
		Me.TabPage2.Controls.Add(Me.Label18)
		Me.TabPage2.Controls.Add(Me.lbldn)
		Me.TabPage2.Controls.Add(Me.lbldmv)
		Me.TabPage2.Controls.Add(Me.lblin)
		Me.TabPage2.Controls.Add(Me.lblpn)
		Me.TabPage2.Controls.Add(Me.ChartHPTemp)
		Me.TabPage2.Controls.Add(Me.Label16)
		Me.TabPage2.Controls.Add(Me.ChartHP)
		Me.TabPage2.Location = New System.Drawing.Point(4, 24)
		Me.TabPage2.Name = "TabPage2"
		Me.TabPage2.Padding = New System.Windows.Forms.Padding(3)
		Me.TabPage2.Size = New System.Drawing.Size(1079, 647)
		Me.TabPage2.TabIndex = 1
		Me.TabPage2.Text = "Hot Plate Temprature"
		Me.TabPage2.UseVisualStyleBackColor = true
		'
		'Label11
		'
		Me.Label11.AutoSize = true
		Me.Label11.Font = New System.Drawing.Font("游ゴシック", 12!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.Label11.ForeColor = System.Drawing.Color.Green
		Me.Label11.Location = New System.Drawing.Point(244, 408)
		Me.Label11.Name = "Label11"
		Me.Label11.Size = New System.Drawing.Size(39, 21)
		Me.Label11.TabIndex = 72
		Me.Label11.Text = "Mv:"
		'
		'Label14
		'
		Me.Label14.AutoSize = true
		Me.Label14.Font = New System.Drawing.Font("游ゴシック", 12!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.Label14.ForeColor = System.Drawing.Color.Blue
		Me.Label14.Location = New System.Drawing.Point(110, 408)
		Me.Label14.Name = "Label14"
		Me.Label14.Size = New System.Drawing.Size(58, 21)
		Me.Label14.TabIndex = 73
		Me.Label14.Text = "Temp:"
		'
		'lblMv
		'
		Me.lblMv.AutoSize = true
		Me.lblMv.Font = New System.Drawing.Font("游ゴシック", 18!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.lblMv.Location = New System.Drawing.Point(285, 400)
		Me.lblMv.Name = "lblMv"
		Me.lblMv.Size = New System.Drawing.Size(49, 31)
		Me.lblMv.TabIndex = 63
		Me.lblMv.Text = "0.0"
		'
		'lblTemp
		'
		Me.lblTemp.AutoSize = true
		Me.lblTemp.Font = New System.Drawing.Font("游ゴシック", 18!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.lblTemp.Location = New System.Drawing.Point(165, 400)
		Me.lblTemp.Name = "lblTemp"
		Me.lblTemp.Size = New System.Drawing.Size(49, 31)
		Me.lblTemp.TabIndex = 64
		Me.lblTemp.Text = "0.0"
		'
		'Label8
		'
		Me.Label8.AutoSize = true
		Me.Label8.Font = New System.Drawing.Font("游ゴシック", 12!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.Label8.ForeColor = System.Drawing.Color.Magenta
		Me.Label8.Location = New System.Drawing.Point(504, 66)
		Me.Label8.Name = "Label8"
		Me.Label8.Size = New System.Drawing.Size(35, 21)
		Me.Label8.TabIndex = 69
		Me.Label8.Text = "dn:"
		'
		'Label17
		'
		Me.Label17.AutoSize = true
		Me.Label17.Font = New System.Drawing.Font("游ゴシック", 12!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.Label17.ForeColor = System.Drawing.Color.Red
		Me.Label17.Location = New System.Drawing.Point(376, 66)
		Me.Label17.Name = "Label17"
		Me.Label17.Size = New System.Drawing.Size(30, 21)
		Me.Label17.TabIndex = 70
		Me.Label17.Text = "in:"
		'
		'Label18
		'
		Me.Label18.AutoSize = true
		Me.Label18.Font = New System.Drawing.Font("游ゴシック", 12!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.Label18.ForeColor = System.Drawing.Color.Green
		Me.Label18.Location = New System.Drawing.Point(241, 66)
		Me.Label18.Name = "Label18"
		Me.Label18.Size = New System.Drawing.Size(35, 21)
		Me.Label18.TabIndex = 71
		Me.Label18.Text = "pn:"
		'
		'lbldn
		'
		Me.lbldn.AutoSize = true
		Me.lbldn.Font = New System.Drawing.Font("游ゴシック", 18!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.lbldn.Location = New System.Drawing.Point(536, 59)
		Me.lbldn.Name = "lbldn"
		Me.lbldn.Size = New System.Drawing.Size(28, 31)
		Me.lbldn.TabIndex = 61
		Me.lbldn.Text = "0"
		'
		'lbldmv
		'
		Me.lbldmv.AutoSize = true
		Me.lbldmv.Font = New System.Drawing.Font("游ゴシック", 18!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.lbldmv.Location = New System.Drawing.Point(153, 59)
		Me.lbldmv.Name = "lbldmv"
		Me.lbldmv.Size = New System.Drawing.Size(28, 31)
		Me.lbldmv.TabIndex = 61
		Me.lbldmv.Text = "0"
		'
		'lblin
		'
		Me.lblin.AutoSize = true
		Me.lblin.Font = New System.Drawing.Font("游ゴシック", 18!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.lblin.Location = New System.Drawing.Point(401, 59)
		Me.lblin.Name = "lblin"
		Me.lblin.Size = New System.Drawing.Size(28, 31)
		Me.lblin.TabIndex = 61
		Me.lblin.Text = "0"
		'
		'lblpn
		'
		Me.lblpn.AutoSize = true
		Me.lblpn.Font = New System.Drawing.Font("游ゴシック", 18!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.lblpn.Location = New System.Drawing.Point(273, 58)
		Me.lblpn.Name = "lblpn"
		Me.lblpn.Size = New System.Drawing.Size(28, 31)
		Me.lblpn.TabIndex = 61
		Me.lblpn.Text = "0"
		'
		'ChartHPTemp
		'
		Me.ChartHPTemp.BorderlineColor = System.Drawing.Color.DarkGray
		Me.ChartHPTemp.BorderlineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Solid
		Me.ChartHPTemp.BorderlineWidth = 2
		Me.ChartHPTemp.Location = New System.Drawing.Point(7, 382)
		Me.ChartHPTemp.Name = "ChartHPTemp"
		Me.ChartHPTemp.Size = New System.Drawing.Size(1067, 261)
		Me.ChartHPTemp.TabIndex = 61
		Me.ChartHPTemp.Text = "Chart2"
		'
		'Label16
		'
		Me.Label16.AutoSize = true
		Me.Label16.Font = New System.Drawing.Font("游ゴシック", 12!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.Label16.ForeColor = System.Drawing.Color.Blue
		Me.Label16.Location = New System.Drawing.Point(107, 66)
		Me.Label16.Name = "Label16"
		Me.Label16.Size = New System.Drawing.Size(49, 21)
		Me.Label16.TabIndex = 62
		Me.Label16.Text = "dmv:"
		'
		'TabPage3
		'
		Me.TabPage3.Controls.Add(Me.Label20)
		Me.TabPage3.Controls.Add(Me.Label21)
		Me.TabPage3.Controls.Add(Me.lblMv3)
		Me.TabPage3.Controls.Add(Me.lblTemp3)
		Me.TabPage3.Controls.Add(Me.Label24)
		Me.TabPage3.Controls.Add(Me.Label25)
		Me.TabPage3.Controls.Add(Me.Label26)
		Me.TabPage3.Controls.Add(Me.lbldn3)
		Me.TabPage3.Controls.Add(Me.lbldmv3)
		Me.TabPage3.Controls.Add(Me.lblin3)
		Me.TabPage3.Controls.Add(Me.lblpn3)
		Me.TabPage3.Controls.Add(Me.ChartSGTemp)
		Me.TabPage3.Controls.Add(Me.Label31)
		Me.TabPage3.Controls.Add(Me.ChartSG)
		Me.TabPage3.Location = New System.Drawing.Point(4, 24)
		Me.TabPage3.Name = "TabPage3"
		Me.TabPage3.Size = New System.Drawing.Size(1079, 647)
		Me.TabPage3.TabIndex = 2
		Me.TabPage3.Text = "Strain guage Temprature"
		Me.TabPage3.UseVisualStyleBackColor = true
		'
		'Label20
		'
		Me.Label20.AutoSize = true
		Me.Label20.Font = New System.Drawing.Font("游ゴシック", 12!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.Label20.ForeColor = System.Drawing.Color.Green
		Me.Label20.Location = New System.Drawing.Point(244, 408)
		Me.Label20.Name = "Label20"
		Me.Label20.Size = New System.Drawing.Size(39, 21)
		Me.Label20.TabIndex = 86
		Me.Label20.Text = "Mv:"
		'
		'Label21
		'
		Me.Label21.AutoSize = true
		Me.Label21.Font = New System.Drawing.Font("游ゴシック", 12!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.Label21.ForeColor = System.Drawing.Color.Blue
		Me.Label21.Location = New System.Drawing.Point(110, 408)
		Me.Label21.Name = "Label21"
		Me.Label21.Size = New System.Drawing.Size(58, 21)
		Me.Label21.TabIndex = 87
		Me.Label21.Text = "Temp:"
		'
		'lblMv3
		'
		Me.lblMv3.AutoSize = true
		Me.lblMv3.Font = New System.Drawing.Font("游ゴシック", 18!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.lblMv3.Location = New System.Drawing.Point(285, 400)
		Me.lblMv3.Name = "lblMv3"
		Me.lblMv3.Size = New System.Drawing.Size(49, 31)
		Me.lblMv3.TabIndex = 81
		Me.lblMv3.Text = "0.0"
		'
		'lblTemp3
		'
		Me.lblTemp3.AutoSize = true
		Me.lblTemp3.Font = New System.Drawing.Font("游ゴシック", 18!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.lblTemp3.Location = New System.Drawing.Point(165, 400)
		Me.lblTemp3.Name = "lblTemp3"
		Me.lblTemp3.Size = New System.Drawing.Size(49, 31)
		Me.lblTemp3.TabIndex = 82
		Me.lblTemp3.Text = "0.0"
		'
		'Label24
		'
		Me.Label24.AutoSize = true
		Me.Label24.Font = New System.Drawing.Font("游ゴシック", 12!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.Label24.ForeColor = System.Drawing.Color.Magenta
		Me.Label24.Location = New System.Drawing.Point(504, 66)
		Me.Label24.Name = "Label24"
		Me.Label24.Size = New System.Drawing.Size(35, 21)
		Me.Label24.TabIndex = 83
		Me.Label24.Text = "dn:"
		'
		'Label25
		'
		Me.Label25.AutoSize = true
		Me.Label25.Font = New System.Drawing.Font("游ゴシック", 12!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.Label25.ForeColor = System.Drawing.Color.Red
		Me.Label25.Location = New System.Drawing.Point(376, 66)
		Me.Label25.Name = "Label25"
		Me.Label25.Size = New System.Drawing.Size(30, 21)
		Me.Label25.TabIndex = 84
		Me.Label25.Text = "in:"
		'
		'Label26
		'
		Me.Label26.AutoSize = true
		Me.Label26.Font = New System.Drawing.Font("游ゴシック", 12!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.Label26.ForeColor = System.Drawing.Color.Green
		Me.Label26.Location = New System.Drawing.Point(241, 66)
		Me.Label26.Name = "Label26"
		Me.Label26.Size = New System.Drawing.Size(35, 21)
		Me.Label26.TabIndex = 85
		Me.Label26.Text = "pn:"
		'
		'lbldn3
		'
		Me.lbldn3.AutoSize = true
		Me.lbldn3.Font = New System.Drawing.Font("游ゴシック", 18!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.lbldn3.Location = New System.Drawing.Point(536, 59)
		Me.lbldn3.Name = "lbldn3"
		Me.lbldn3.Size = New System.Drawing.Size(28, 31)
		Me.lbldn3.TabIndex = 75
		Me.lbldn3.Text = "0"
		'
		'lbldmv3
		'
		Me.lbldmv3.AutoSize = true
		Me.lbldmv3.Font = New System.Drawing.Font("游ゴシック", 18!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.lbldmv3.Location = New System.Drawing.Point(153, 59)
		Me.lbldmv3.Name = "lbldmv3"
		Me.lbldmv3.Size = New System.Drawing.Size(28, 31)
		Me.lbldmv3.TabIndex = 76
		Me.lbldmv3.Text = "0"
		'
		'lblin3
		'
		Me.lblin3.AutoSize = true
		Me.lblin3.Font = New System.Drawing.Font("游ゴシック", 18!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.lblin3.Location = New System.Drawing.Point(401, 59)
		Me.lblin3.Name = "lblin3"
		Me.lblin3.Size = New System.Drawing.Size(28, 31)
		Me.lblin3.TabIndex = 77
		Me.lblin3.Text = "0"
		'
		'lblpn3
		'
		Me.lblpn3.AutoSize = true
		Me.lblpn3.Font = New System.Drawing.Font("游ゴシック", 18!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.lblpn3.Location = New System.Drawing.Point(273, 58)
		Me.lblpn3.Name = "lblpn3"
		Me.lblpn3.Size = New System.Drawing.Size(28, 31)
		Me.lblpn3.TabIndex = 78
		Me.lblpn3.Text = "0"
		'
		'ChartSGTemp
		'
		Me.ChartSGTemp.BorderlineColor = System.Drawing.Color.DarkGray
		Me.ChartSGTemp.BorderlineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Solid
		Me.ChartSGTemp.BorderlineWidth = 2
		Me.ChartSGTemp.Location = New System.Drawing.Point(7, 382)
		Me.ChartSGTemp.Name = "ChartSGTemp"
		Me.ChartSGTemp.Size = New System.Drawing.Size(1067, 261)
		Me.ChartSGTemp.TabIndex = 79
		Me.ChartSGTemp.Text = "Chart2"
		'
		'Label31
		'
		Me.Label31.AutoSize = true
		Me.Label31.Font = New System.Drawing.Font("游ゴシック", 12!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.Label31.ForeColor = System.Drawing.Color.Blue
		Me.Label31.Location = New System.Drawing.Point(107, 66)
		Me.Label31.Name = "Label31"
		Me.Label31.Size = New System.Drawing.Size(49, 21)
		Me.Label31.TabIndex = 80
		Me.Label31.Text = "dmv:"
		'
		'ChartSG
		'
		Me.ChartSG.BorderlineColor = System.Drawing.Color.DarkGray
		Me.ChartSG.BorderlineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Solid
		Me.ChartSG.BorderlineWidth = 2
		Me.ChartSG.Location = New System.Drawing.Point(6, 6)
		Me.ChartSG.Name = "ChartSG"
		Me.ChartSG.Size = New System.Drawing.Size(1067, 372)
		Me.ChartSG.TabIndex = 74
		Me.ChartSG.Text = "Chart2"
		'
		'nudXSpan
		'
		Me.nudXSpan.Font = New System.Drawing.Font("Meiryo UI", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.nudXSpan.Increment = New Decimal(New Integer() {10, 0, 0, 0})
		Me.nudXSpan.Location = New System.Drawing.Point(253, 13)
		Me.nudXSpan.Maximum = New Decimal(New Integer() {100000, 0, 0, 0})
		Me.nudXSpan.Name = "nudXSpan"
		Me.nudXSpan.Size = New System.Drawing.Size(76, 27)
		Me.nudXSpan.TabIndex = 13
		Me.nudXSpan.Value = New Decimal(New Integer() {1000, 0, 0, 0})
		'
		'gbxProbe
		'
		Me.gbxProbe.Controls.Add(Me.nudPVGain2)
		Me.gbxProbe.Controls.Add(Me.lblPVGain2)
		Me.gbxProbe.Controls.Add(Me.nudPVBias2)
		Me.gbxProbe.Controls.Add(Me.nudDgain2)
		Me.gbxProbe.Controls.Add(Me.nudD2)
		Me.gbxProbe.Controls.Add(Me.nudI2)
		Me.gbxProbe.Controls.Add(Me.nudP2)
		Me.gbxProbe.Controls.Add(Me.lblD2)
		Me.gbxProbe.Controls.Add(Me.lblI2)
		Me.gbxProbe.Controls.Add(Me.lblPVBias2)
		Me.gbxProbe.Controls.Add(Me.lblDgain2)
		Me.gbxProbe.Controls.Add(Me.lblP2)
		Me.gbxProbe.Font = New System.Drawing.Font("Meiryo UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.gbxProbe.Location = New System.Drawing.Point(1095, 30)
		Me.gbxProbe.Name = "gbxProbe"
		Me.gbxProbe.Size = New System.Drawing.Size(174, 173)
		Me.gbxProbe.TabIndex = 63
		Me.gbxProbe.TabStop = false
		Me.gbxProbe.Text = "Probe Temperature"
		'
		'nudPVGain2
		'
		Me.nudPVGain2.DecimalPlaces = 4
		Me.nudPVGain2.Font = New System.Drawing.Font("Meiryo UI", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.nudPVGain2.Increment = New Decimal(New Integer() {1, 0, 0, 131072})
		Me.nudPVGain2.Location = New System.Drawing.Point(92, 130)
		Me.nudPVGain2.Maximum = New Decimal(New Integer() {300, 0, 0, 0})
		Me.nudPVGain2.Minimum = New Decimal(New Integer() {1, 0, 0, 131072})
		Me.nudPVGain2.Name = "nudPVGain2"
		Me.nudPVGain2.Size = New System.Drawing.Size(74, 27)
		Me.nudPVGain2.TabIndex = 13
		Me.nudPVGain2.Value = New Decimal(New Integer() {1, 0, 0, 131072})
		'
		'lblPVGain2
		'
		Me.lblPVGain2.AutoSize = true
		Me.lblPVGain2.Font = New System.Drawing.Font("Meiryo UI", 9!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.lblPVGain2.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.lblPVGain2.Location = New System.Drawing.Point(4, 135)
		Me.lblPVGain2.Name = "lblPVGain2"
		Me.lblPVGain2.Size = New System.Drawing.Size(48, 15)
		Me.lblPVGain2.TabIndex = 12
		Me.lblPVGain2.Text = "PVGain"
		'
		'nudPVBias2
		'
		Me.nudPVBias2.DecimalPlaces = 1
		Me.nudPVBias2.Font = New System.Drawing.Font("Meiryo UI", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.nudPVBias2.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
		Me.nudPVBias2.Location = New System.Drawing.Point(92, 107)
		Me.nudPVBias2.Maximum = New Decimal(New Integer() {10, 0, 0, 0})
		Me.nudPVBias2.Minimum = New Decimal(New Integer() {10, 0, 0, -2147483648})
		Me.nudPVBias2.Name = "nudPVBias2"
		Me.nudPVBias2.Size = New System.Drawing.Size(74, 27)
		Me.nudPVBias2.TabIndex = 4
		'
		'nudDgain2
		'
		Me.nudDgain2.DecimalPlaces = 2
		Me.nudDgain2.Font = New System.Drawing.Font("Meiryo UI", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.nudDgain2.Increment = New Decimal(New Integer() {1, 0, 0, 131072})
		Me.nudDgain2.Location = New System.Drawing.Point(92, 84)
		Me.nudDgain2.Maximum = New Decimal(New Integer() {300, 0, 0, 0})
		Me.nudDgain2.Name = "nudDgain2"
		Me.nudDgain2.Size = New System.Drawing.Size(74, 27)
		Me.nudDgain2.TabIndex = 3
		'
		'nudD2
		'
		Me.nudD2.DecimalPlaces = 2
		Me.nudD2.Font = New System.Drawing.Font("Meiryo UI", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.nudD2.Increment = New Decimal(New Integer() {1, 0, 0, 131072})
		Me.nudD2.Location = New System.Drawing.Point(92, 61)
		Me.nudD2.Maximum = New Decimal(New Integer() {300, 0, 0, 0})
		Me.nudD2.Name = "nudD2"
		Me.nudD2.Size = New System.Drawing.Size(74, 27)
		Me.nudD2.TabIndex = 2
		'
		'nudI2
		'
		Me.nudI2.DecimalPlaces = 1
		Me.nudI2.Font = New System.Drawing.Font("Meiryo UI", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.nudI2.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
		Me.nudI2.Location = New System.Drawing.Point(92, 38)
		Me.nudI2.Maximum = New Decimal(New Integer() {1000, 0, 0, 0})
		Me.nudI2.Name = "nudI2"
		Me.nudI2.Size = New System.Drawing.Size(74, 27)
		Me.nudI2.TabIndex = 1
		Me.nudI2.Value = New Decimal(New Integer() {1, 0, 0, 0})
		'
		'nudP2
		'
		Me.nudP2.DecimalPlaces = 2
		Me.nudP2.Font = New System.Drawing.Font("Meiryo UI", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.nudP2.Increment = New Decimal(New Integer() {1, 0, 0, 131072})
		Me.nudP2.Location = New System.Drawing.Point(92, 14)
		Me.nudP2.Maximum = New Decimal(New Integer() {60, 0, 0, 0})
		Me.nudP2.Name = "nudP2"
		Me.nudP2.Size = New System.Drawing.Size(74, 27)
		Me.nudP2.TabIndex = 0
		Me.nudP2.Value = New Decimal(New Integer() {123, 0, 0, 65536})
		'
		'lblD2
		'
		Me.lblD2.AutoSize = true
		Me.lblD2.Font = New System.Drawing.Font("Meiryo UI", 9!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.lblD2.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.lblD2.Location = New System.Drawing.Point(67, 65)
		Me.lblD2.Name = "lblD2"
		Me.lblD2.Size = New System.Drawing.Size(16, 15)
		Me.lblD2.TabIndex = 7
		Me.lblD2.Text = "D"
		'
		'lblI2
		'
		Me.lblI2.AutoSize = true
		Me.lblI2.Font = New System.Drawing.Font("Meiryo UI", 9!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.lblI2.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.lblI2.Location = New System.Drawing.Point(69, 43)
		Me.lblI2.Name = "lblI2"
		Me.lblI2.Size = New System.Drawing.Size(12, 15)
		Me.lblI2.TabIndex = 8
		Me.lblI2.Text = "I"
		'
		'lblPVBias2
		'
		Me.lblPVBias2.AutoSize = true
		Me.lblPVBias2.Font = New System.Drawing.Font("Meiryo UI", 9!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.lblPVBias2.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.lblPVBias2.Location = New System.Drawing.Point(4, 111)
		Me.lblPVBias2.Name = "lblPVBias2"
		Me.lblPVBias2.Size = New System.Drawing.Size(89, 15)
		Me.lblPVBias2.TabIndex = 9
		Me.lblPVBias2.Text = "PVBias[deg C]"
		'
		'lblDgain2
		'
		Me.lblDgain2.AutoSize = true
		Me.lblDgain2.Font = New System.Drawing.Font("Meiryo UI", 9!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.lblDgain2.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.lblDgain2.Location = New System.Drawing.Point(46, 88)
		Me.lblDgain2.Name = "lblDgain2"
		Me.lblDgain2.Size = New System.Drawing.Size(40, 15)
		Me.lblDgain2.TabIndex = 10
		Me.lblDgain2.Text = "Dgain"
		'
		'lblP2
		'
		Me.lblP2.AutoSize = true
		Me.lblP2.Font = New System.Drawing.Font("Meiryo UI", 9!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.lblP2.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.lblP2.Location = New System.Drawing.Point(66, 19)
		Me.lblP2.Name = "lblP2"
		Me.lblP2.Size = New System.Drawing.Size(14, 15)
		Me.lblP2.TabIndex = 11
		Me.lblP2.Text = "P"
		'
		'gbxHP
		'
		Me.gbxHP.Controls.Add(Me.nudPVGain)
		Me.gbxHP.Controls.Add(Me.lblPVGain)
		Me.gbxHP.Controls.Add(Me.nudPVBias)
		Me.gbxHP.Controls.Add(Me.nudDgain)
		Me.gbxHP.Controls.Add(Me.nudD)
		Me.gbxHP.Controls.Add(Me.nudI)
		Me.gbxHP.Controls.Add(Me.nudP)
		Me.gbxHP.Controls.Add(Me.lblD)
		Me.gbxHP.Controls.Add(Me.lblI)
		Me.gbxHP.Controls.Add(Me.lblPVBias)
		Me.gbxHP.Controls.Add(Me.lblDgain)
		Me.gbxHP.Controls.Add(Me.lblP)
		Me.gbxHP.Font = New System.Drawing.Font("Meiryo UI", 9.75!)
		Me.gbxHP.Location = New System.Drawing.Point(1095, 209)
		Me.gbxHP.Name = "gbxHP"
		Me.gbxHP.Size = New System.Drawing.Size(174, 171)
		Me.gbxHP.TabIndex = 64
		Me.gbxHP.TabStop = false
		Me.gbxHP.Text = "HP Temperature"
		'
		'nudPVGain
		'
		Me.nudPVGain.DecimalPlaces = 4
		Me.nudPVGain.Font = New System.Drawing.Font("Meiryo UI", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.nudPVGain.Increment = New Decimal(New Integer() {1, 0, 0, 131072})
		Me.nudPVGain.Location = New System.Drawing.Point(92, 132)
		Me.nudPVGain.Maximum = New Decimal(New Integer() {300, 0, 0, 0})
		Me.nudPVGain.Minimum = New Decimal(New Integer() {1, 0, 0, 131072})
		Me.nudPVGain.Name = "nudPVGain"
		Me.nudPVGain.Size = New System.Drawing.Size(74, 27)
		Me.nudPVGain.TabIndex = 13
		Me.nudPVGain.Value = New Decimal(New Integer() {1, 0, 0, 131072})
		'
		'lblPVGain
		'
		Me.lblPVGain.AutoSize = true
		Me.lblPVGain.Font = New System.Drawing.Font("Meiryo UI", 9!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.lblPVGain.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.lblPVGain.Location = New System.Drawing.Point(3, 137)
		Me.lblPVGain.Name = "lblPVGain"
		Me.lblPVGain.Size = New System.Drawing.Size(48, 15)
		Me.lblPVGain.TabIndex = 12
		Me.lblPVGain.Text = "PVGain"
		'
		'nudPVBias
		'
		Me.nudPVBias.DecimalPlaces = 1
		Me.nudPVBias.Font = New System.Drawing.Font("Meiryo UI", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.nudPVBias.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
		Me.nudPVBias.Location = New System.Drawing.Point(92, 108)
		Me.nudPVBias.Maximum = New Decimal(New Integer() {10, 0, 0, 0})
		Me.nudPVBias.Minimum = New Decimal(New Integer() {10, 0, 0, -2147483648})
		Me.nudPVBias.Name = "nudPVBias"
		Me.nudPVBias.Size = New System.Drawing.Size(74, 27)
		Me.nudPVBias.TabIndex = 9
		'
		'nudDgain
		'
		Me.nudDgain.DecimalPlaces = 2
		Me.nudDgain.Font = New System.Drawing.Font("Meiryo UI", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.nudDgain.Increment = New Decimal(New Integer() {1, 0, 0, 131072})
		Me.nudDgain.Location = New System.Drawing.Point(92, 84)
		Me.nudDgain.Maximum = New Decimal(New Integer() {300, 0, 0, 0})
		Me.nudDgain.Name = "nudDgain"
		Me.nudDgain.Size = New System.Drawing.Size(74, 27)
		Me.nudDgain.TabIndex = 8
		'
		'nudD
		'
		Me.nudD.DecimalPlaces = 2
		Me.nudD.Font = New System.Drawing.Font("Meiryo UI", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.nudD.Increment = New Decimal(New Integer() {1, 0, 0, 131072})
		Me.nudD.Location = New System.Drawing.Point(92, 61)
		Me.nudD.Maximum = New Decimal(New Integer() {300, 0, 0, 0})
		Me.nudD.Name = "nudD"
		Me.nudD.Size = New System.Drawing.Size(74, 27)
		Me.nudD.TabIndex = 7
		'
		'nudI
		'
		Me.nudI.DecimalPlaces = 1
		Me.nudI.Font = New System.Drawing.Font("Meiryo UI", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.nudI.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
		Me.nudI.Location = New System.Drawing.Point(92, 38)
		Me.nudI.Maximum = New Decimal(New Integer() {1000, 0, 0, 0})
		Me.nudI.Name = "nudI"
		Me.nudI.Size = New System.Drawing.Size(74, 27)
		Me.nudI.TabIndex = 6
		Me.nudI.Value = New Decimal(New Integer() {1, 0, 0, 0})
		'
		'nudP
		'
		Me.nudP.DecimalPlaces = 2
		Me.nudP.Font = New System.Drawing.Font("Meiryo UI", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.nudP.Increment = New Decimal(New Integer() {1, 0, 0, 131072})
		Me.nudP.Location = New System.Drawing.Point(92, 14)
		Me.nudP.Maximum = New Decimal(New Integer() {60, 0, 0, 0})
		Me.nudP.Name = "nudP"
		Me.nudP.Size = New System.Drawing.Size(74, 27)
		Me.nudP.TabIndex = 5
		Me.nudP.Value = New Decimal(New Integer() {123, 0, 0, 65536})
		'
		'lblD
		'
		Me.lblD.AutoSize = true
		Me.lblD.Font = New System.Drawing.Font("Meiryo UI", 9!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.lblD.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.lblD.Location = New System.Drawing.Point(67, 65)
		Me.lblD.Name = "lblD"
		Me.lblD.Size = New System.Drawing.Size(16, 15)
		Me.lblD.TabIndex = 7
		Me.lblD.Text = "D"
		'
		'lblI
		'
		Me.lblI.AutoSize = true
		Me.lblI.Font = New System.Drawing.Font("Meiryo UI", 9!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.lblI.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.lblI.Location = New System.Drawing.Point(69, 41)
		Me.lblI.Name = "lblI"
		Me.lblI.Size = New System.Drawing.Size(12, 15)
		Me.lblI.TabIndex = 8
		Me.lblI.Text = "I"
		'
		'lblPVBias
		'
		Me.lblPVBias.AutoSize = true
		Me.lblPVBias.Font = New System.Drawing.Font("Meiryo UI", 9!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.lblPVBias.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.lblPVBias.Location = New System.Drawing.Point(4, 112)
		Me.lblPVBias.Name = "lblPVBias"
		Me.lblPVBias.Size = New System.Drawing.Size(89, 15)
		Me.lblPVBias.TabIndex = 9
		Me.lblPVBias.Text = "PVBias[deg C]"
		'
		'lblDgain
		'
		Me.lblDgain.AutoSize = true
		Me.lblDgain.Font = New System.Drawing.Font("Meiryo UI", 9!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.lblDgain.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.lblDgain.Location = New System.Drawing.Point(46, 88)
		Me.lblDgain.Name = "lblDgain"
		Me.lblDgain.Size = New System.Drawing.Size(40, 15)
		Me.lblDgain.TabIndex = 10
		Me.lblDgain.Text = "Dgain"
		'
		'lblP
		'
		Me.lblP.AutoSize = true
		Me.lblP.Font = New System.Drawing.Font("Meiryo UI", 9!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.lblP.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.lblP.Location = New System.Drawing.Point(66, 19)
		Me.lblP.Name = "lblP"
		Me.lblP.Size = New System.Drawing.Size(14, 15)
		Me.lblP.TabIndex = 11
		Me.lblP.Text = "P"
		'
		'btnGraphReset
		'
		Me.btnGraphReset.Font = New System.Drawing.Font("Meiryo UI", 12!)
		Me.btnGraphReset.Location = New System.Drawing.Point(13, 750)
		Me.btnGraphReset.Name = "btnGraphReset"
		Me.btnGraphReset.Size = New System.Drawing.Size(178, 28)
		Me.btnGraphReset.TabIndex = 10
		Me.btnGraphReset.Text = "Graph Reset"
		Me.btnGraphReset.UseVisualStyleBackColor = true
		'
		'btnMonEnable
		'
		Me.btnMonEnable.Font = New System.Drawing.Font("Meiryo UI", 12!)
		Me.btnMonEnable.Location = New System.Drawing.Point(1095, 750)
		Me.btnMonEnable.Name = "btnMonEnable"
		Me.btnMonEnable.Size = New System.Drawing.Size(166, 28)
		Me.btnMonEnable.TabIndex = 17
		Me.btnMonEnable.Text = "Monitor Enable"
		Me.btnMonEnable.UseVisualStyleBackColor = true
		'
		'btnHeaterContPB
		'
		Me.btnHeaterContPB.Font = New System.Drawing.Font("Meiryo UI", 12!)
		Me.btnHeaterContPB.Location = New System.Drawing.Point(869, 684)
		Me.btnHeaterContPB.Name = "btnHeaterContPB"
		Me.btnHeaterContPB.Size = New System.Drawing.Size(216, 28)
		Me.btnHeaterContPB.TabIndex = 15
		Me.btnHeaterContPB.Text = "Probe Heater Cont."
		Me.btnHeaterContPB.UseVisualStyleBackColor = true
		'
		'lblXSpan
		'
		Me.lblXSpan.AutoSize = true
		Me.lblXSpan.Font = New System.Drawing.Font("Meiryo UI", 9!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.lblXSpan.Location = New System.Drawing.Point(182, 17)
		Me.lblXSpan.Name = "lblXSpan"
		Me.lblXSpan.Size = New System.Drawing.Size(48, 15)
		Me.lblXSpan.TabIndex = 67
		Me.lblXSpan.Text = "X Span"
		'
		'lblYMax
		'
		Me.lblYMax.AutoSize = true
		Me.lblYMax.Font = New System.Drawing.Font("Meiryo UI", 9!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.lblYMax.Location = New System.Drawing.Point(16, 18)
		Me.lblYMax.Name = "lblYMax"
		Me.lblYMax.Size = New System.Drawing.Size(43, 15)
		Me.lblYMax.TabIndex = 67
		Me.lblYMax.Text = "Y Max"
		'
		'lblYMin
		'
		Me.lblYMin.AutoSize = true
		Me.lblYMin.Font = New System.Drawing.Font("Meiryo UI", 9!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.lblYMin.Location = New System.Drawing.Point(16, 43)
		Me.lblYMin.Name = "lblYMin"
		Me.lblYMin.Size = New System.Drawing.Size(39, 15)
		Me.lblYMin.TabIndex = 67
		Me.lblYMin.Text = "Y Min"
		'
		'nudYMax
		'
		Me.nudYMax.Font = New System.Drawing.Font("Meiryo UI", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.nudYMax.Increment = New Decimal(New Integer() {10, 0, 0, 0})
		Me.nudYMax.Location = New System.Drawing.Point(87, 13)
		Me.nudYMax.Maximum = New Decimal(New Integer() {100000, 0, 0, 0})
		Me.nudYMax.Name = "nudYMax"
		Me.nudYMax.Size = New System.Drawing.Size(76, 27)
		Me.nudYMax.TabIndex = 11
		Me.nudYMax.Value = New Decimal(New Integer() {1000, 0, 0, 0})
		'
		'nudYMin
		'
		Me.nudYMin.Font = New System.Drawing.Font("Meiryo UI", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.nudYMin.Increment = New Decimal(New Integer() {10, 0, 0, 0})
		Me.nudYMin.Location = New System.Drawing.Point(87, 37)
		Me.nudYMin.Maximum = New Decimal(New Integer() {0, 0, 0, 0})
		Me.nudYMin.Minimum = New Decimal(New Integer() {100000, 0, 0, -2147483648})
		Me.nudYMin.Name = "nudYMin"
		Me.nudYMin.Size = New System.Drawing.Size(76, 27)
		Me.nudYMin.TabIndex = 12
		Me.nudYMin.Value = New Decimal(New Integer() {1000, 0, 0, -2147483648})
		'
		'GroupBox1
		'
		Me.GroupBox1.Controls.Add(Me.nudXSpan)
		Me.GroupBox1.Controls.Add(Me.lblYMin)
		Me.GroupBox1.Controls.Add(Me.nudYMax)
		Me.GroupBox1.Controls.Add(Me.lblYMax)
		Me.GroupBox1.Controls.Add(Me.nudYMin)
		Me.GroupBox1.Controls.Add(Me.lblXSpan)
		Me.GroupBox1.Font = New System.Drawing.Font("游ゴシック", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.GroupBox1.Location = New System.Drawing.Point(13, 675)
		Me.GroupBox1.Name = "GroupBox1"
		Me.GroupBox1.Size = New System.Drawing.Size(344, 71)
		Me.GroupBox1.TabIndex = 68
		Me.GroupBox1.TabStop = false
		'
		'btnHeaterContHP
		'
		Me.btnHeaterContHP.Font = New System.Drawing.Font("Meiryo UI", 12!)
		Me.btnHeaterContHP.Location = New System.Drawing.Point(869, 718)
		Me.btnHeaterContHP.Name = "btnHeaterContHP"
		Me.btnHeaterContHP.Size = New System.Drawing.Size(216, 28)
		Me.btnHeaterContHP.TabIndex = 16
		Me.btnHeaterContHP.Text = "HP Heater Cont."
		Me.btnHeaterContHP.UseVisualStyleBackColor = true
		'
		'nudPBTemp
		'
		Me.nudPBTemp.DecimalPlaces = 1
		Me.nudPBTemp.Font = New System.Drawing.Font("Meiryo UI", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.nudPBTemp.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
		Me.nudPBTemp.Location = New System.Drawing.Point(776, 688)
		Me.nudPBTemp.Maximum = New Decimal(New Integer() {240, 0, 0, 0})
		Me.nudPBTemp.Name = "nudPBTemp"
		Me.nudPBTemp.Size = New System.Drawing.Size(76, 27)
		Me.nudPBTemp.TabIndex = 69
		Me.nudPBTemp.Value = New Decimal(New Integer() {30, 0, 0, 0})
		'
		'nudHPTemp
		'
		Me.nudHPTemp.DecimalPlaces = 1
		Me.nudHPTemp.Font = New System.Drawing.Font("Meiryo UI", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.nudHPTemp.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
		Me.nudHPTemp.Location = New System.Drawing.Point(776, 721)
		Me.nudHPTemp.Maximum = New Decimal(New Integer() {240, 0, 0, 0})
		Me.nudHPTemp.Name = "nudHPTemp"
		Me.nudHPTemp.Size = New System.Drawing.Size(76, 27)
		Me.nudHPTemp.TabIndex = 70
		Me.nudHPTemp.Value = New Decimal(New Integer() {30, 0, 0, 0})
		'
		'lblTempPBPreset
		'
		Me.lblTempPBPreset.AutoSize = true
		Me.lblTempPBPreset.Font = New System.Drawing.Font("Meiryo UI", 9!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.lblTempPBPreset.Location = New System.Drawing.Point(623, 693)
		Me.lblTempPBPreset.Name = "lblTempPBPreset"
		Me.lblTempPBPreset.Size = New System.Drawing.Size(147, 15)
		Me.lblTempPBPreset.TabIndex = 71
		Me.lblTempPBPreset.Text = "PB Temp. preset(deg C)"
		'
		'lblTempHPPreset
		'
		Me.lblTempHPPreset.AutoSize = true
		Me.lblTempHPPreset.Font = New System.Drawing.Font("Meiryo UI", 9!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.lblTempHPPreset.Location = New System.Drawing.Point(622, 726)
		Me.lblTempHPPreset.Name = "lblTempHPPreset"
		Me.lblTempHPPreset.Size = New System.Drawing.Size(148, 15)
		Me.lblTempHPPreset.TabIndex = 71
		Me.lblTempHPPreset.Text = "HP Temp. preset(deg C)"
		'
		'gbxSG
		'
		Me.gbxSG.Controls.Add(Me.nudPVGain3)
		Me.gbxSG.Controls.Add(Me.lblPVGain3)
		Me.gbxSG.Controls.Add(Me.nudPVBias3)
		Me.gbxSG.Controls.Add(Me.nudDgain3)
		Me.gbxSG.Controls.Add(Me.nudD3)
		Me.gbxSG.Controls.Add(Me.nudI3)
		Me.gbxSG.Controls.Add(Me.nudP3)
		Me.gbxSG.Controls.Add(Me.lblD3)
		Me.gbxSG.Controls.Add(Me.lblI3)
		Me.gbxSG.Controls.Add(Me.lblPVBias3)
		Me.gbxSG.Controls.Add(Me.lblDgain3)
		Me.gbxSG.Controls.Add(Me.lblP3)
		Me.gbxSG.Font = New System.Drawing.Font("Meiryo UI", 9.75!)
		Me.gbxSG.Location = New System.Drawing.Point(1095, 386)
		Me.gbxSG.Name = "gbxSG"
		Me.gbxSG.Size = New System.Drawing.Size(174, 170)
		Me.gbxSG.TabIndex = 72
		Me.gbxSG.TabStop = false
		Me.gbxSG.Text = "SG Temperature"
		'
		'nudPVGain3
		'
		Me.nudPVGain3.DecimalPlaces = 4
		Me.nudPVGain3.Font = New System.Drawing.Font("Meiryo UI", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.nudPVGain3.Increment = New Decimal(New Integer() {1, 0, 0, 131072})
		Me.nudPVGain3.Location = New System.Drawing.Point(92, 132)
		Me.nudPVGain3.Maximum = New Decimal(New Integer() {300, 0, 0, 0})
		Me.nudPVGain3.Minimum = New Decimal(New Integer() {1, 0, 0, 131072})
		Me.nudPVGain3.Name = "nudPVGain3"
		Me.nudPVGain3.Size = New System.Drawing.Size(74, 27)
		Me.nudPVGain3.TabIndex = 13
		Me.nudPVGain3.Value = New Decimal(New Integer() {1, 0, 0, 131072})
		'
		'lblPVGain3
		'
		Me.lblPVGain3.AutoSize = true
		Me.lblPVGain3.Font = New System.Drawing.Font("Meiryo UI", 9!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.lblPVGain3.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.lblPVGain3.Location = New System.Drawing.Point(3, 137)
		Me.lblPVGain3.Name = "lblPVGain3"
		Me.lblPVGain3.Size = New System.Drawing.Size(48, 15)
		Me.lblPVGain3.TabIndex = 12
		Me.lblPVGain3.Text = "PVGain"
		'
		'nudPVBias3
		'
		Me.nudPVBias3.DecimalPlaces = 1
		Me.nudPVBias3.Font = New System.Drawing.Font("Meiryo UI", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.nudPVBias3.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
		Me.nudPVBias3.Location = New System.Drawing.Point(92, 108)
		Me.nudPVBias3.Maximum = New Decimal(New Integer() {10, 0, 0, 0})
		Me.nudPVBias3.Minimum = New Decimal(New Integer() {10, 0, 0, -2147483648})
		Me.nudPVBias3.Name = "nudPVBias3"
		Me.nudPVBias3.Size = New System.Drawing.Size(74, 27)
		Me.nudPVBias3.TabIndex = 9
		'
		'nudDgain3
		'
		Me.nudDgain3.DecimalPlaces = 2
		Me.nudDgain3.Font = New System.Drawing.Font("Meiryo UI", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.nudDgain3.Increment = New Decimal(New Integer() {1, 0, 0, 131072})
		Me.nudDgain3.Location = New System.Drawing.Point(92, 84)
		Me.nudDgain3.Maximum = New Decimal(New Integer() {300, 0, 0, 0})
		Me.nudDgain3.Name = "nudDgain3"
		Me.nudDgain3.Size = New System.Drawing.Size(74, 27)
		Me.nudDgain3.TabIndex = 8
		'
		'nudD3
		'
		Me.nudD3.DecimalPlaces = 2
		Me.nudD3.Font = New System.Drawing.Font("Meiryo UI", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.nudD3.Increment = New Decimal(New Integer() {1, 0, 0, 131072})
		Me.nudD3.Location = New System.Drawing.Point(92, 61)
		Me.nudD3.Maximum = New Decimal(New Integer() {300, 0, 0, 0})
		Me.nudD3.Name = "nudD3"
		Me.nudD3.Size = New System.Drawing.Size(74, 27)
		Me.nudD3.TabIndex = 7
		'
		'nudI3
		'
		Me.nudI3.DecimalPlaces = 1
		Me.nudI3.Font = New System.Drawing.Font("Meiryo UI", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.nudI3.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
		Me.nudI3.Location = New System.Drawing.Point(92, 38)
		Me.nudI3.Maximum = New Decimal(New Integer() {1000, 0, 0, 0})
		Me.nudI3.Name = "nudI3"
		Me.nudI3.Size = New System.Drawing.Size(74, 27)
		Me.nudI3.TabIndex = 6
		Me.nudI3.Value = New Decimal(New Integer() {1, 0, 0, 0})
		'
		'nudP3
		'
		Me.nudP3.DecimalPlaces = 2
		Me.nudP3.Font = New System.Drawing.Font("Meiryo UI", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.nudP3.Increment = New Decimal(New Integer() {1, 0, 0, 131072})
		Me.nudP3.Location = New System.Drawing.Point(92, 14)
		Me.nudP3.Maximum = New Decimal(New Integer() {60, 0, 0, 0})
		Me.nudP3.Name = "nudP3"
		Me.nudP3.Size = New System.Drawing.Size(74, 27)
		Me.nudP3.TabIndex = 5
		Me.nudP3.Value = New Decimal(New Integer() {123, 0, 0, 65536})
		'
		'lblD3
		'
		Me.lblD3.AutoSize = true
		Me.lblD3.Font = New System.Drawing.Font("Meiryo UI", 9!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.lblD3.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.lblD3.Location = New System.Drawing.Point(67, 65)
		Me.lblD3.Name = "lblD3"
		Me.lblD3.Size = New System.Drawing.Size(16, 15)
		Me.lblD3.TabIndex = 7
		Me.lblD3.Text = "D"
		'
		'lblI3
		'
		Me.lblI3.AutoSize = true
		Me.lblI3.Font = New System.Drawing.Font("Meiryo UI", 9!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.lblI3.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.lblI3.Location = New System.Drawing.Point(69, 41)
		Me.lblI3.Name = "lblI3"
		Me.lblI3.Size = New System.Drawing.Size(12, 15)
		Me.lblI3.TabIndex = 8
		Me.lblI3.Text = "I"
		'
		'lblPVBias3
		'
		Me.lblPVBias3.AutoSize = true
		Me.lblPVBias3.Font = New System.Drawing.Font("Meiryo UI", 9!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.lblPVBias3.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.lblPVBias3.Location = New System.Drawing.Point(4, 112)
		Me.lblPVBias3.Name = "lblPVBias3"
		Me.lblPVBias3.Size = New System.Drawing.Size(89, 15)
		Me.lblPVBias3.TabIndex = 9
		Me.lblPVBias3.Text = "PVBias[deg C]"
		'
		'lblDgain3
		'
		Me.lblDgain3.AutoSize = true
		Me.lblDgain3.Font = New System.Drawing.Font("Meiryo UI", 9!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.lblDgain3.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.lblDgain3.Location = New System.Drawing.Point(46, 88)
		Me.lblDgain3.Name = "lblDgain3"
		Me.lblDgain3.Size = New System.Drawing.Size(40, 15)
		Me.lblDgain3.TabIndex = 10
		Me.lblDgain3.Text = "Dgain"
		'
		'lblP3
		'
		Me.lblP3.AutoSize = true
		Me.lblP3.Font = New System.Drawing.Font("Meiryo UI", 9!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.lblP3.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.lblP3.Location = New System.Drawing.Point(66, 19)
		Me.lblP3.Name = "lblP3"
		Me.lblP3.Size = New System.Drawing.Size(14, 15)
		Me.lblP3.TabIndex = 11
		Me.lblP3.Text = "P"
		'
		'lblTempSGPreset
		'
		Me.lblTempSGPreset.AutoSize = true
		Me.lblTempSGPreset.Font = New System.Drawing.Font("Meiryo UI", 9!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.lblTempSGPreset.Location = New System.Drawing.Point(622, 759)
		Me.lblTempSGPreset.Name = "lblTempSGPreset"
		Me.lblTempSGPreset.Size = New System.Drawing.Size(149, 15)
		Me.lblTempSGPreset.TabIndex = 75
		Me.lblTempSGPreset.Text = "SG Temp. preset(deg C)"
		'
		'nudSGTemp
		'
		Me.nudSGTemp.DecimalPlaces = 1
		Me.nudSGTemp.Font = New System.Drawing.Font("Meiryo UI", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.nudSGTemp.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
		Me.nudSGTemp.Location = New System.Drawing.Point(776, 754)
		Me.nudSGTemp.Maximum = New Decimal(New Integer() {240, 0, 0, 0})
		Me.nudSGTemp.Name = "nudSGTemp"
		Me.nudSGTemp.Size = New System.Drawing.Size(76, 27)
		Me.nudSGTemp.TabIndex = 74
		Me.nudSGTemp.Value = New Decimal(New Integer() {30, 0, 0, 0})
		'
		'btnHeaterContSG
		'
		Me.btnHeaterContSG.Font = New System.Drawing.Font("Meiryo UI", 12!)
		Me.btnHeaterContSG.Location = New System.Drawing.Point(869, 751)
		Me.btnHeaterContSG.Name = "btnHeaterContSG"
		Me.btnHeaterContSG.Size = New System.Drawing.Size(216, 28)
		Me.btnHeaterContSG.TabIndex = 73
		Me.btnHeaterContSG.Text = "SG Heater Cont."
		Me.btnHeaterContSG.UseVisualStyleBackColor = true
		'
		'frmTempAdjMon
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6!, 12!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(1273, 789)
		Me.Controls.Add(Me.lblTempSGPreset)
		Me.Controls.Add(Me.nudSGTemp)
		Me.Controls.Add(Me.btnHeaterContSG)
		Me.Controls.Add(Me.gbxSG)
		Me.Controls.Add(Me.lblTempHPPreset)
		Me.Controls.Add(Me.lblTempPBPreset)
		Me.Controls.Add(Me.nudHPTemp)
		Me.Controls.Add(Me.nudPBTemp)
		Me.Controls.Add(Me.GroupBox1)
		Me.Controls.Add(Me.btnHeaterContHP)
		Me.Controls.Add(Me.btnHeaterContPB)
		Me.Controls.Add(Me.btnMonEnable)
		Me.Controls.Add(Me.btnGraphReset)
		Me.Controls.Add(Me.gbxHP)
		Me.Controls.Add(Me.gbxProbe)
		Me.Controls.Add(Me.tbcTemp)
		Me.MaximizeBox = false
		Me.MinimizeBox = false
		Me.Name = "frmTempAdjMon"
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
		Me.Text = "Temprature Control Adjustment"
		CType(Me.ChartHP,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.ChartPB,System.ComponentModel.ISupportInitialize).EndInit
		Me.tbcTemp.ResumeLayout(false)
		Me.TabPage1.ResumeLayout(false)
		Me.TabPage1.PerformLayout
		CType(Me.ChartPBTemp,System.ComponentModel.ISupportInitialize).EndInit
		Me.TabPage2.ResumeLayout(false)
		Me.TabPage2.PerformLayout
		CType(Me.ChartHPTemp,System.ComponentModel.ISupportInitialize).EndInit
		Me.TabPage3.ResumeLayout(false)
		Me.TabPage3.PerformLayout
		CType(Me.ChartSGTemp,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.ChartSG,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.nudXSpan,System.ComponentModel.ISupportInitialize).EndInit
		Me.gbxProbe.ResumeLayout(false)
		Me.gbxProbe.PerformLayout
		CType(Me.nudPVGain2,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.nudPVBias2,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.nudDgain2,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.nudD2,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.nudI2,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.nudP2,System.ComponentModel.ISupportInitialize).EndInit
		Me.gbxHP.ResumeLayout(false)
		Me.gbxHP.PerformLayout
		CType(Me.nudPVGain,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.nudPVBias,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.nudDgain,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.nudD,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.nudI,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.nudP,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.nudYMax,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.nudYMin,System.ComponentModel.ISupportInitialize).EndInit
		Me.GroupBox1.ResumeLayout(false)
		Me.GroupBox1.PerformLayout
		CType(Me.nudPBTemp,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.nudHPTemp,System.ComponentModel.ISupportInitialize).EndInit
		Me.gbxSG.ResumeLayout(false)
		Me.gbxSG.PerformLayout
		CType(Me.nudPVGain3,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.nudPVBias3,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.nudDgain3,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.nudD3,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.nudI3,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.nudP3,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.nudSGTemp,System.ComponentModel.ISupportInitialize).EndInit
		Me.ResumeLayout(false)
		Me.PerformLayout

End Sub

	Friend WithEvents ChartHP As DataVisualization.Charting.Chart
	Friend WithEvents ChartPB As DataVisualization.Charting.Chart
	Friend WithEvents Timer1 As Timer
	Friend WithEvents tbcTemp As TabControl
	Friend WithEvents TabPage1 As TabPage
	Friend WithEvents ChartPBTemp As DataVisualization.Charting.Chart
	Friend WithEvents TabPage2 As TabPage
	Friend WithEvents ChartHPTemp As DataVisualization.Charting.Chart
	Friend WithEvents nudXSpan As NumericUpDown
	Friend WithEvents gbxProbe As GroupBox
	Friend WithEvents nudPVBias2 As NumericUpDown
	Friend WithEvents nudDgain2 As NumericUpDown
	Friend WithEvents nudD2 As NumericUpDown
	Friend WithEvents nudI2 As NumericUpDown
	Friend WithEvents nudP2 As NumericUpDown
	Friend WithEvents lblD2 As Label
	Friend WithEvents lblI2 As Label
	Friend WithEvents lblPVBias2 As Label
	Friend WithEvents lblDgain2 As Label
	Friend WithEvents lblP2 As Label
	Friend WithEvents gbxHP As GroupBox
	Friend WithEvents nudPVBias As NumericUpDown
	Friend WithEvents nudDgain As NumericUpDown
	Friend WithEvents nudD As NumericUpDown
	Friend WithEvents nudI As NumericUpDown
	Friend WithEvents nudP As NumericUpDown
	Friend WithEvents lblD As Label
	Friend WithEvents lblI As Label
	Friend WithEvents lblPVBias As Label
	Friend WithEvents lblDgain As Label
	Friend WithEvents lblP As Label
	Friend WithEvents btnGraphReset As Button
	Friend WithEvents btnMonEnable As Button
	Friend WithEvents lblMv2 As Label
	Friend WithEvents lblTemp2 As Label
	Friend WithEvents Label2 As Label
	Friend WithEvents Label1 As Label
	Friend WithEvents lblMv As Label
	Friend WithEvents lblTemp As Label
	Friend WithEvents btnHeaterContPB As Button
	Friend WithEvents lblXSpan As Label
	Friend WithEvents lblYMax As Label
	Friend WithEvents lblYMin As Label
	Friend WithEvents nudYMax As NumericUpDown
	Friend WithEvents nudYMin As NumericUpDown
	Friend WithEvents GroupBox1 As GroupBox
	Friend WithEvents lbldmv2 As Label
	Friend WithEvents Label9 As Label
	Friend WithEvents Label10 As Label
	Friend WithEvents lblpn2 As Label
	Friend WithEvents Label13 As Label
	Friend WithEvents Label12 As Label
	Friend WithEvents lbldn2 As Label
	Friend WithEvents lblin2 As Label
	Friend WithEvents lbldmv As Label
	Friend WithEvents Label16 As Label
	Friend WithEvents lbldn As Label
	Friend WithEvents lblin As Label
	Friend WithEvents lblpn As Label
	Friend WithEvents Label11 As Label
	Friend WithEvents Label14 As Label
	Friend WithEvents Label8 As Label
	Friend WithEvents Label17 As Label
	Friend WithEvents Label18 As Label
	Friend WithEvents btnHeaterContHP As Button
	Friend WithEvents nudPVGain2 As NumericUpDown
	Friend WithEvents lblPVGain2 As Label
	Friend WithEvents nudPVGain As NumericUpDown
	Friend WithEvents lblPVGain As Label
	Friend WithEvents nudPBTemp As NumericUpDown
	Friend WithEvents nudHPTemp As NumericUpDown
	Friend WithEvents lblTempPBPreset As Label
	Friend WithEvents lblTempHPPreset As Label
	Friend WithEvents TabPage3 As TabPage
	Friend WithEvents Label20 As Label
	Friend WithEvents Label21 As Label
	Friend WithEvents lblMv3 As Label
	Friend WithEvents lblTemp3 As Label
	Friend WithEvents Label24 As Label
	Friend WithEvents Label25 As Label
	Friend WithEvents Label26 As Label
	Friend WithEvents lbldn3 As Label
	Friend WithEvents lbldmv3 As Label
	Friend WithEvents lblin3 As Label
	Friend WithEvents lblpn3 As Label
	Friend WithEvents ChartSGTemp As DataVisualization.Charting.Chart
	Friend WithEvents Label31 As Label
	Friend WithEvents ChartSG As DataVisualization.Charting.Chart
	Friend WithEvents gbxSG As GroupBox
	Friend WithEvents nudPVGain3 As NumericUpDown
	Friend WithEvents lblPVGain3 As Label
	Friend WithEvents nudPVBias3 As NumericUpDown
	Friend WithEvents nudDgain3 As NumericUpDown
	Friend WithEvents nudD3 As NumericUpDown
	Friend WithEvents nudI3 As NumericUpDown
	Friend WithEvents nudP3 As NumericUpDown
	Friend WithEvents lblD3 As Label
	Friend WithEvents lblI3 As Label
	Friend WithEvents lblPVBias3 As Label
	Friend WithEvents lblDgain3 As Label
	Friend WithEvents lblP3 As Label
	Friend WithEvents lblTempSGPreset As Label
	Friend WithEvents nudSGTemp As NumericUpDown
	Friend WithEvents btnHeaterContSG As Button
End Class
