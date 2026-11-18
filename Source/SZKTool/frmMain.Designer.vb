<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMain
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
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMain))
		Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
		Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
		Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
		Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
		Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
		Dim ChartArea1 As System.Windows.Forms.DataVisualization.Charting.ChartArea = New System.Windows.Forms.DataVisualization.Charting.ChartArea()
		Dim Title1 As System.Windows.Forms.DataVisualization.Charting.Title = New System.Windows.Forms.DataVisualization.Charting.Title()
		Dim ListViewItem1 As System.Windows.Forms.ListViewItem = New System.Windows.Forms.ListViewItem(New String() {"qqq", "1.23"}, -1)
		Dim ListViewItem2 As System.Windows.Forms.ListViewItem = New System.Windows.Forms.ListViewItem(New String() {"www", "2.569"}, -1)
		Dim ChartArea2 As System.Windows.Forms.DataVisualization.Charting.ChartArea = New System.Windows.Forms.DataVisualization.Charting.ChartArea()
		Dim Legend1 As System.Windows.Forms.DataVisualization.Charting.Legend = New System.Windows.Forms.DataVisualization.Charting.Legend()
		Dim Series1 As System.Windows.Forms.DataVisualization.Charting.Series = New System.Windows.Forms.DataVisualization.Charting.Series()
		Me.timMain = New System.Windows.Forms.Timer(Me.components)
		Me.Lbl2 = New System.Windows.Forms.Label()
		Me.Lbl3 = New System.Windows.Forms.Label()
		Me.GroupBox0 = New System.Windows.Forms.GroupBox()
		Me.lbl52 = New System.Windows.Forms.Label()
		Me.Lbl4 = New System.Windows.Forms.Label()
		Me.Lbl5 = New System.Windows.Forms.Label()
		Me.Lbl51 = New System.Windows.Forms.Label()
		Me.Lbl6 = New System.Windows.Forms.Label()
		Me.Lbl7 = New System.Windows.Forms.Label()
		Me.Lbl50 = New System.Windows.Forms.Label()
		Me.Lbl8 = New System.Windows.Forms.Label()
		Me.Lbl9 = New System.Windows.Forms.Label()
		Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog()
		Me.ToolTipMain = New System.Windows.Forms.ToolTip(Me.components)
		Me.btnTemp = New System.Windows.Forms.Button()
		Me.btnZero = New System.Windows.Forms.Button()
		Me.nudYAxisZoom = New System.Windows.Forms.NumericUpDown()
		Me.nudYscaleMax = New System.Windows.Forms.NumericUpDown()
		Me.nudYscaleMin = New System.Windows.Forms.NumericUpDown()
		Me.nudXscaleMax = New System.Windows.Forms.NumericUpDown()
		Me.nudXscaleMin = New System.Windows.Forms.NumericUpDown()
		Me.nudXdivision = New System.Windows.Forms.NumericUpDown()
		Me.nudYdivision = New System.Windows.Forms.NumericUpDown()
		Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
		Me.ToolStripStatusLabel1 = New System.Windows.Forms.ToolStripStatusLabel()
		Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
		Me.tsmiFile = New System.Windows.Forms.ToolStripMenuItem()
		Me.tsmiPrint = New System.Windows.Forms.ToolStripMenuItem()
		Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
		Me.tsmiImport = New System.Windows.Forms.ToolStripMenuItem()
		Me.tsmiMeasAreaImport = New System.Windows.Forms.ToolStripMenuItem()
		Me.tsmiStdAreaImport = New System.Windows.Forms.ToolStripMenuItem()
		Me.tsmiStdAreaImport1 = New System.Windows.Forms.ToolStripMenuItem()
		Me.tsmiStdAreaImport2 = New System.Windows.Forms.ToolStripMenuItem()
		Me.tsmiExport = New System.Windows.Forms.ToolStripMenuItem()
		Me.tsmiStandardExp = New System.Windows.Forms.ToolStripMenuItem()
		Me.tsmiTextData = New System.Windows.Forms.ToolStripMenuItem()
		Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
		Me.tsmiExit = New System.Windows.Forms.ToolStripMenuItem()
		Me.tsmiGraph = New System.Windows.Forms.ToolStripMenuItem()
		Me.tsmiRecalculation = New System.Windows.Forms.ToolStripMenuItem()
		Me.ToolStripSeparator9 = New System.Windows.Forms.ToolStripSeparator()
		Me.tsmiHistoryDraw = New System.Windows.Forms.ToolStripMenuItem()
		Me.tsmiHistoryClear = New System.Windows.Forms.ToolStripMenuItem()
		Me.ToolStripSeparator5 = New System.Windows.Forms.ToolStripSeparator()
		Me.tsmiStandardDraw = New System.Windows.Forms.ToolStripMenuItem()
		Me.tsmiStandardClear = New System.Windows.Forms.ToolStripMenuItem()
		Me.ToolStripSeparator10 = New System.Windows.Forms.ToolStripSeparator()
		Me.tsmiAllClear = New System.Windows.Forms.ToolStripMenuItem()
		Me.ToolStripSeparator12 = New System.Windows.Forms.ToolStripSeparator()
		Me.tsmiFixedX = New System.Windows.Forms.ToolStripMenuItem()
		Me.tsmiFixedY = New System.Windows.Forms.ToolStripMenuItem()
		Me.ToolStripSeparator4 = New System.Windows.Forms.ToolStripSeparator()
		Me.tsmiXAxisTemp = New System.Windows.Forms.ToolStripMenuItem()
		Me.ToolStripSeparator13 = New System.Windows.Forms.ToolStripSeparator()
		Me.tsmiDigitalMonitorChange = New System.Windows.Forms.ToolStripMenuItem()
		Me.ToolStripSeparator17 = New System.Windows.Forms.ToolStripSeparator()
		Me.tsmiSaveGraph = New System.Windows.Forms.ToolStripMenuItem()
		Me.tsmiMeasure = New System.Windows.Forms.ToolStripMenuItem()
		Me.tsmiGelTime = New System.Windows.Forms.ToolStripMenuItem()
		Me.tsmiFlowCurve = New System.Windows.Forms.ToolStripMenuItem()
		Me.tsmiRemote = New System.Windows.Forms.ToolStripMenuItem()
		Me.tsmiTemp = New System.Windows.Forms.ToolStripMenuItem()
		Me.tsmiZeroAdj = New System.Windows.Forms.ToolStripMenuItem()
		Me.ToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
		Me.tsmiResetError = New System.Windows.Forms.ToolStripMenuItem()
		Me.tsmiReturnOrigin = New System.Windows.Forms.ToolStripMenuItem()
		Me.ToolStripSeparator14 = New System.Windows.Forms.ToolStripSeparator()
		Me.tsmiLEDLight = New System.Windows.Forms.ToolStripMenuItem()
		Me.tsmi7SegBright = New System.Windows.Forms.ToolStripMenuItem()
		Me.tsmiCmb7SegBright = New System.Windows.Forms.ToolStripComboBox()
		Me.tsmiSettings = New System.Windows.Forms.ToolStripMenuItem()
		Me.tsmiOptions = New System.Windows.Forms.ToolStripMenuItem()
		Me.ToolStripSeparator6 = New System.Windows.Forms.ToolStripSeparator()
		Me.tsmiCaliblation = New System.Windows.Forms.ToolStripMenuItem()
		Me.tsmiWeight = New System.Windows.Forms.ToolStripMenuItem()
		Me.tsmiFullStress = New System.Windows.Forms.ToolStripMenuItem()
		Me.tsmiZeroStress = New System.Windows.Forms.ToolStripMenuItem()
		Me.tsmiHorizontalConfirmation = New System.Windows.Forms.ToolStripMenuItem()
		Me.ToolStripSeparator7 = New System.Windows.Forms.ToolStripSeparator()
		Me.tsmiStressZeroSet = New System.Windows.Forms.ToolStripMenuItem()
		Me.ToolStripSeparator8 = New System.Windows.Forms.ToolStripSeparator()
		Me.tsmiFixToFree = New System.Windows.Forms.ToolStripMenuItem()
		Me.ToolStripSeparator18 = New System.Windows.Forms.ToolStripSeparator()
		Me.tsmiAdminMode = New System.Windows.Forms.ToolStripMenuItem()
		Me.ToolStripSeparator19 = New System.Windows.Forms.ToolStripSeparator()
		Me.tsmiQrCodeCreate = New System.Windows.Forms.ToolStripMenuItem()
		Me.tsmiHelp = New System.Windows.Forms.ToolStripMenuItem()
		Me.tsmiShowHelp = New System.Windows.Forms.ToolStripMenuItem()
		Me.ToolStripSeparator3 = New System.Windows.Forms.ToolStripSeparator()
		Me.tsmiMent = New System.Windows.Forms.ToolStripMenuItem()
		Me.ToolStripSeparator20 = New System.Windows.Forms.ToolStripSeparator()
		Me.tsmiAbout = New System.Windows.Forms.ToolStripMenuItem()
		Me.cboEquipName = New System.Windows.Forms.ComboBox()
		Me.lblEquipment = New System.Windows.Forms.Label()
		Me.btnErrReset = New System.Windows.Forms.Button()
		Me.btnMeasure = New System.Windows.Forms.Button()
		Me.lblConnectState = New System.Windows.Forms.Label()
		Me.pctConStts = New System.Windows.Forms.PictureBox()
		Me.lblDevAlive = New System.Windows.Forms.Label()
		Me.btnOrigin = New System.Windows.Forms.Button()
		Me.lblMachineVer = New System.Windows.Forms.Label()
		Me.lblCpuVer = New System.Windows.Forms.Label()
		Me.pnlSPEC = New System.Windows.Forms.Panel()
		Me.btnSamSpecEdit = New System.Windows.Forms.Button()
		Me.btnDevSpecEdit = New System.Windows.Forms.Button()
		Me.cbxSamSPEC = New System.Windows.Forms.ComboBox()
		Me.dgvSamSpec = New System.Windows.Forms.DataGridView()
		Me.lblSamSPECNameLabel = New System.Windows.Forms.Label()
		Me.dgvDevSpec = New System.Windows.Forms.DataGridView()
		Me.cbxDevSPEC = New System.Windows.Forms.ComboBox()
		Me.lblDevSPECNameLabel = New System.Windows.Forms.Label()
		Me.timerShowState = New System.Windows.Forms.Timer(Me.components)
		Me.Panel1 = New System.Windows.Forms.Panel()
		Me.rbZPosition = New System.Windows.Forms.RadioButton()
		Me.rbShearVelo = New System.Windows.Forms.RadioButton()
		Me.rbCorrectP = New System.Windows.Forms.RadioButton()
		Me.rbLossTangent = New System.Windows.Forms.RadioButton()
		Me.rbElasticMod = New System.Windows.Forms.RadioButton()
		Me.rbStorageMod = New System.Windows.Forms.RadioButton()
		Me.rbShearMod = New System.Windows.Forms.RadioButton()
		Me.rbSampleStrain = New System.Windows.Forms.RadioButton()
		Me.rbPosPhase = New System.Windows.Forms.RadioButton()
		Me.rbViscosity = New System.Windows.Forms.RadioButton()
		Me.rbZStress = New System.Windows.Forms.RadioButton()
		Me.rbPBMV = New System.Windows.Forms.RadioButton()
		Me.rbPBTemp = New System.Windows.Forms.RadioButton()
		Me.rbHPMV = New System.Windows.Forms.RadioButton()
		Me.rbHPTemp = New System.Windows.Forms.RadioButton()
		Me.rbCorrect = New System.Windows.Forms.RadioButton()
		Me.rbYStress = New System.Windows.Forms.RadioButton()
		Me.rbXStress = New System.Windows.Forms.RadioButton()
		Me.lblYRangeOverLamp = New System.Windows.Forms.Label()
		Me.lblXRangeOverLamp = New System.Windows.Forms.Label()
		Me.btnAllClear = New System.Windows.Forms.Button()
		Me.lblElapsedVal = New System.Windows.Forms.Label()
		Me.pnlRealG = New System.Windows.Forms.Panel()
		Me.btnSPC = New System.Windows.Forms.Button()
		Me.lblRangeWarning = New System.Windows.Forms.Label()
		Me.lblGuideXYbeforeGap = New System.Windows.Forms.Label()
		Me.lblFailedOrderP = New System.Windows.Forms.Label()
		Me.pnlThinning = New System.Windows.Forms.Panel()
		Me.lblExpand = New System.Windows.Forms.Label()
		Me.gpxScrol = New System.Windows.Forms.GroupBox()
		Me.lblScrolWidth = New System.Windows.Forms.Label()
		Me.nudXSpan = New System.Windows.Forms.NumericUpDown()
		Me.gpxAxisSetting = New System.Windows.Forms.GroupBox()
		Me.gbxXscale = New System.Windows.Forms.GroupBox()
		Me.Label2 = New System.Windows.Forms.Label()
		Me.btnXSpanAply = New System.Windows.Forms.Button()
		Me.ckbXFixed = New System.Windows.Forms.CheckBox()
		Me.gbxYscale = New System.Windows.Forms.GroupBox()
		Me.Label3 = New System.Windows.Forms.Label()
		Me.btnYSpanAply = New System.Windows.Forms.Button()
		Me.ckbYFixed = New System.Windows.Forms.CheckBox()
		Me.gpxThinning = New System.Windows.Forms.GroupBox()
		Me.nudDispThinningoutRough = New System.Windows.Forms.NumericUpDown()
		Me.nudDispThinningoutDetail = New System.Windows.Forms.NumericUpDown()
		Me.lblDispThinningRough = New System.Windows.Forms.Label()
		Me.lblDispThinningDetail = New System.Windows.Forms.Label()
		Me.gpxXType = New System.Windows.Forms.GroupBox()
		Me.rbXTime = New System.Windows.Forms.RadioButton()
		Me.rbXTemp = New System.Windows.Forms.RadioButton()
		Me.Panel2 = New System.Windows.Forms.Panel()
		Me.Chart2 = New System.Windows.Forms.DataVisualization.Charting.Chart()
		Me.lblCalclating = New System.Windows.Forms.Label()
		Me.pnlSteress = New System.Windows.Forms.Panel()
		Me.lblNoLoadStrVal = New System.Windows.Forms.Label()
		Me.lblZStrsCRVal = New System.Windows.Forms.Label()
		Me.lblNoLoadStrLabel = New System.Windows.Forms.Label()
		Me.lblCurrXStrs = New System.Windows.Forms.Label()
		Me.lblZStrsCR = New System.Windows.Forms.Label()
		Me.lblCurrYStrs = New System.Windows.Forms.Label()
		Me.lblCurrZStrsVal = New System.Windows.Forms.Label()
		Me.lblCurrYStrsVal = New System.Windows.Forms.Label()
		Me.lblCurrXStrsVal = New System.Windows.Forms.Label()
		Me.lblCurrZStrs = New System.Windows.Forms.Label()
		Me.lblYAuto = New System.Windows.Forms.Label()
		Me.lblXAuto = New System.Windows.Forms.Label()
		Me.Button1 = New System.Windows.Forms.Button()
		Me.lblLongTimeMode = New System.Windows.Forms.Label()
		Me.gbxStdCond = New System.Windows.Forms.GroupBox()
		Me.btnStdCondClose = New System.Windows.Forms.Button()
		Me.rbReal = New System.Windows.Forms.RadioButton()
		Me.rbStd1 = New System.Windows.Forms.RadioButton()
		Me.rbStd2 = New System.Windows.Forms.RadioButton()
		Me.dgvStd = New System.Windows.Forms.DataGridView()
		Me.Column1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
		Me.Column2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
		Me.Label1 = New System.Windows.Forms.Label()
		Me.lblZRangeOverLamp = New System.Windows.Forms.Label()
		Me.pnlCalculation = New System.Windows.Forms.Panel()
		Me.lblShearVeloVal = New System.Windows.Forms.Label()
		Me.lblElasticModVal = New System.Windows.Forms.Label()
		Me.lblTangentLabel = New System.Windows.Forms.Label()
		Me.lblShearVeloLabel = New System.Windows.Forms.Label()
		Me.lblTangentVal = New System.Windows.Forms.Label()
		Me.lblElasticModLabel = New System.Windows.Forms.Label()
		Me.lblStorageModVal = New System.Windows.Forms.Label()
		Me.lblStrgModLabel = New System.Windows.Forms.Label()
		Me.lblShearModVal = New System.Windows.Forms.Label()
		Me.lblShearModLabel = New System.Windows.Forms.Label()
		Me.lblSmplStrnVal = New System.Windows.Forms.Label()
		Me.lblPhaseVal = New System.Windows.Forms.Label()
		Me.lblPhaseLabel = New System.Windows.Forms.Label()
		Me.lblSmpStrnLabel = New System.Windows.Forms.Label()
		Me.lblViscoVal = New System.Windows.Forms.Label()
		Me.lblViscoLabel = New System.Windows.Forms.Label()
		Me.lblStressPVal = New System.Windows.Forms.Label()
		Me.lblCurrStressPLabel = New System.Windows.Forms.Label()
		Me.lblStressVal = New System.Windows.Forms.Label()
		Me.lblCurrStressLabel = New System.Windows.Forms.Label()
		Me.btnStdInfo = New System.Windows.Forms.Button()
		Me.btnThinning = New System.Windows.Forms.Button()
		Me.lblChartCoordinate = New System.Windows.Forms.Label()
		Me.gbxCurrentVal = New System.Windows.Forms.GroupBox()
		Me.lblCurrSGMVVal = New System.Windows.Forms.Label()
		Me.lblCurrSGMV = New System.Windows.Forms.Label()
		Me.lblCurrSGTempVal = New System.Windows.Forms.Label()
		Me.lblCurrSGTemp = New System.Windows.Forms.Label()
		Me.pnlPosition = New System.Windows.Forms.Panel()
		Me.lblZConfPosLabel = New System.Windows.Forms.Label()
		Me.lblZConfPosVal = New System.Windows.Forms.Label()
		Me.lblZeroPosVal = New System.Windows.Forms.Label()
		Me.lblZeroPosLabel = New System.Windows.Forms.Label()
		Me.lblGAPVal = New System.Windows.Forms.Label()
		Me.lblCurrXPos = New System.Windows.Forms.Label()
		Me.lblGAPLabel = New System.Windows.Forms.Label()
		Me.lblCurrXPosVal = New System.Windows.Forms.Label()
		Me.lblCurrYPosVal = New System.Windows.Forms.Label()
		Me.lblCurrZPosVal = New System.Windows.Forms.Label()
		Me.lblCurrYPos = New System.Windows.Forms.Label()
		Me.lblCurrZPos = New System.Windows.Forms.Label()
		Me.lblCurrPBMVVal = New System.Windows.Forms.Label()
		Me.lblCurrPBMV = New System.Windows.Forms.Label()
		Me.lblCurrPBTempVal = New System.Windows.Forms.Label()
		Me.lblCurrPBTemp = New System.Windows.Forms.Label()
		Me.lblCurrHPMV = New System.Windows.Forms.Label()
		Me.lblCurrHPTemp = New System.Windows.Forms.Label()
		Me.lblElapsedTime = New System.Windows.Forms.Label()
		Me.lblCurrHPMVVal = New System.Windows.Forms.Label()
		Me.lblCurrHPTempVal = New System.Windows.Forms.Label()
		Me.lblResult1Val = New System.Windows.Forms.Label()
		Me.lblResult1 = New System.Windows.Forms.Label()
		Me.lblResult3Val = New System.Windows.Forms.Label()
		Me.lblResult2Val = New System.Windows.Forms.Label()
		Me.lblResult3 = New System.Windows.Forms.Label()
		Me.lblResult2 = New System.Windows.Forms.Label()
		Me.Chart1 = New System.Windows.Forms.DataVisualization.Charting.Chart()
		Me.gbxHistgram = New System.Windows.Forms.GroupBox()
		Me.bynHistClose = New System.Windows.Forms.Button()
		Me.pnlHistOpe = New System.Windows.Forms.Panel()
		Me.lvStat = New System.Windows.Forms.ListView()
		Me.ColumnHeader1 = CType(New System.Windows.Forms.ColumnHeader(),System.Windows.Forms.ColumnHeader)
		Me.ColumnHeader2 = CType(New System.Windows.Forms.ColumnHeader(),System.Windows.Forms.ColumnHeader)
		Me.lblSecWidth = New System.Windows.Forms.Label()
		Me.nudSection = New System.Windows.Forms.NumericUpDown()
		Me.Chart3 = New System.Windows.Forms.DataVisualization.Charting.Chart()
		Me.gbxExpoPad = New System.Windows.Forms.GroupBox()
		Me.btnExtrapolationSave = New System.Windows.Forms.Button()
		Me.cboGelTSel = New System.Windows.Forms.ComboBox()
		Me.lblMiddleLine = New System.Windows.Forms.Label()
		Me.lblOverBaseLine = New System.Windows.Forms.Label()
		Me.lblMidA = New System.Windows.Forms.Label()
		Me.lblOverBaseA = New System.Windows.Forms.Label()
		Me.lblUnderBaseA = New System.Windows.Forms.Label()
		Me.lblUnderBaseLine = New System.Windows.Forms.Label()
		Me.lblGelTimeSelection = New System.Windows.Forms.Label()
		Me.lblThinningOut = New System.Windows.Forms.Label()
		Me.lblExpoSample = New System.Windows.Forms.Label()
		Me.nudEndTim = New System.Windows.Forms.NumericUpDown()
		Me.nudThinning = New System.Windows.Forms.NumericUpDown()
		Me.nudExpoSample = New System.Windows.Forms.NumericUpDown()
		Me.nudStartTim = New System.Windows.Forms.NumericUpDown()
		Me.nudMidTim = New System.Windows.Forms.NumericUpDown()
		Me.btnDigChange = New System.Windows.Forms.Button()
		Me.bgwInterrupt = New System.ComponentModel.BackgroundWorker()
		Me.bgwTrendUpdate = New System.ComponentModel.BackgroundWorker()
		Me.PrintDocument1 = New System.Drawing.Printing.PrintDocument()
		Me.cmsDragDrop = New System.Windows.Forms.ContextMenuStrip(Me.components)
		Me.tsmiReal = New System.Windows.Forms.ToolStripMenuItem()
		Me.tsmiStandard1 = New System.Windows.Forms.ToolStripMenuItem()
		Me.tsmiStandard2 = New System.Windows.Forms.ToolStripMenuItem()
		Me.tsmiSepSlope = New System.Windows.Forms.ToolStripSeparator()
		Me.tsmiSLOPERecalc = New System.Windows.Forms.ToolStripMenuItem()
		Me.ToolStripSeparator11 = New System.Windows.Forms.ToolStripSeparator()
		Me.tsmiCansel = New System.Windows.Forms.ToolStripMenuItem()
		Me.cmsSpecSelect = New System.Windows.Forms.ContextMenuStrip(Me.components)
		Me.ToolStripMenuItem2 = New System.Windows.Forms.ToolStripMenuItem()
		Me.ToolStripMenuItem4 = New System.Windows.Forms.ToolStripMenuItem()
		Me.ToolStripMenuItem3 = New System.Windows.Forms.ToolStripMenuItem()
		Me.cmsHistHead = New System.Windows.Forms.ContextMenuStrip(Me.components)
		Me.tsmiHistDel = New System.Windows.Forms.ToolStripMenuItem()
		Me.ToolStripSeparator15 = New System.Windows.Forms.ToolStripSeparator()
		Me.tsmiHistCut = New System.Windows.Forms.ToolStripMenuItem()
		Me.tsmiHistPaste = New System.Windows.Forms.ToolStripMenuItem()
		Me.ToolStripSeparator16 = New System.Windows.Forms.ToolStripSeparator()
		Me.tsmiHistUndo = New System.Windows.Forms.ToolStripMenuItem()
		Me.SaveFileDialog1 = New System.Windows.Forms.SaveFileDialog()
		Me.lblStrLen = New System.Windows.Forms.Label()
		Me.lblInterlockVal = New System.Windows.Forms.Label()
		Me.Label5 = New System.Windows.Forms.Label()
		Me.bgwErrorControl = New System.ComponentModel.BackgroundWorker()
		Me.lblAdminEna = New System.Windows.Forms.Label()
		Me.ilState = New System.Windows.Forms.ImageList(Me.components)
		Me.lblLTMMode = New System.Windows.Forms.Label()
		Me.bgwCalculation = New System.ComponentModel.BackgroundWorker()
		Me.btnQRcode = New System.Windows.Forms.Button()
		Me.txtQR = New System.Windows.Forms.TextBox()
		Me.pnlQR = New System.Windows.Forms.Panel()
		Me.QRSimuTimer = New System.Windows.Forms.Timer(Me.components)
		Me.GroupBox0.SuspendLayout
		CType(Me.nudYAxisZoom,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.nudYscaleMax,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.nudYscaleMin,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.nudXscaleMax,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.nudXscaleMin,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.nudXdivision,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.nudYdivision,System.ComponentModel.ISupportInitialize).BeginInit
		Me.StatusStrip1.SuspendLayout
		Me.MenuStrip1.SuspendLayout
		CType(Me.pctConStts,System.ComponentModel.ISupportInitialize).BeginInit
		Me.pnlSPEC.SuspendLayout
		CType(Me.dgvSamSpec,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.dgvDevSpec,System.ComponentModel.ISupportInitialize).BeginInit
		Me.Panel1.SuspendLayout
		Me.pnlRealG.SuspendLayout
		Me.pnlThinning.SuspendLayout
		Me.gpxScrol.SuspendLayout
		CType(Me.nudXSpan,System.ComponentModel.ISupportInitialize).BeginInit
		Me.gpxAxisSetting.SuspendLayout
		Me.gbxXscale.SuspendLayout
		Me.gbxYscale.SuspendLayout
		Me.gpxThinning.SuspendLayout
		CType(Me.nudDispThinningoutRough,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.nudDispThinningoutDetail,System.ComponentModel.ISupportInitialize).BeginInit
		Me.gpxXType.SuspendLayout
		Me.Panel2.SuspendLayout
		CType(Me.Chart2,System.ComponentModel.ISupportInitialize).BeginInit
		Me.pnlSteress.SuspendLayout
		Me.gbxStdCond.SuspendLayout
		CType(Me.dgvStd,System.ComponentModel.ISupportInitialize).BeginInit
		Me.pnlCalculation.SuspendLayout
		Me.gbxCurrentVal.SuspendLayout
		Me.pnlPosition.SuspendLayout
		CType(Me.Chart1,System.ComponentModel.ISupportInitialize).BeginInit
		Me.gbxHistgram.SuspendLayout
		Me.pnlHistOpe.SuspendLayout
		CType(Me.nudSection,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.Chart3,System.ComponentModel.ISupportInitialize).BeginInit
		Me.gbxExpoPad.SuspendLayout
		CType(Me.nudEndTim,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.nudThinning,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.nudExpoSample,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.nudStartTim,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.nudMidTim,System.ComponentModel.ISupportInitialize).BeginInit
		Me.cmsDragDrop.SuspendLayout
		Me.cmsSpecSelect.SuspendLayout
		Me.cmsHistHead.SuspendLayout
		Me.pnlQR.SuspendLayout
		Me.SuspendLayout
		'
		'timMain
		'
		'
		'Lbl2
		'
		Me.Lbl2.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
		Me.Lbl2.AutoSize = true
		Me.Lbl2.Font = New System.Drawing.Font("Meiryo UI", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.Lbl2.Location = New System.Drawing.Point(1126, 7)
		Me.Lbl2.Name = "Lbl2"
		Me.Lbl2.Size = New System.Drawing.Size(37, 20)
		Me.Lbl2.TabIndex = 26
		Me.Lbl2.Text = "----"
		Me.Lbl2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me.Lbl2.Visible = false
		'
		'Lbl3
		'
		Me.Lbl3.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
		Me.Lbl3.AutoSize = true
		Me.Lbl3.Font = New System.Drawing.Font("Meiryo UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.Lbl3.Location = New System.Drawing.Point(1015, 9)
		Me.Lbl3.Name = "Lbl3"
		Me.Lbl3.Size = New System.Drawing.Size(120, 17)
		Me.Lbl3.TabIndex = 26
		Me.Lbl3.Text = "Scan Time(mS) : "
		Me.Lbl3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me.Lbl3.Visible = false
		'
		'GroupBox0
		'
		Me.GroupBox0.Controls.Add(Me.lbl52)
		Me.GroupBox0.Controls.Add(Me.Lbl4)
		Me.GroupBox0.Controls.Add(Me.Lbl5)
		Me.GroupBox0.Controls.Add(Me.Lbl51)
		Me.GroupBox0.Controls.Add(Me.Lbl6)
		Me.GroupBox0.Controls.Add(Me.Lbl7)
		Me.GroupBox0.Controls.Add(Me.Lbl50)
		Me.GroupBox0.Controls.Add(Me.Lbl8)
		Me.GroupBox0.Controls.Add(Me.Lbl9)
		Me.GroupBox0.Font = New System.Drawing.Font("Meiryo UI", 15.75!)
		Me.GroupBox0.Location = New System.Drawing.Point(17, 706)
		Me.GroupBox0.Name = "GroupBox0"
		Me.GroupBox0.Size = New System.Drawing.Size(128, 173)
		Me.GroupBox0.TabIndex = 30
		Me.GroupBox0.TabStop = false
		Me.GroupBox0.Visible = false
		'
		'lbl52
		'
		Me.lbl52.BackColor = System.Drawing.SystemColors.ButtonHighlight
		Me.lbl52.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		Me.lbl52.Font = New System.Drawing.Font("Meiryo UI", 8.25!)
		Me.lbl52.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.lbl52.Location = New System.Drawing.Point(13, 103)
		Me.lbl52.Name = "lbl52"
		Me.lbl52.Size = New System.Drawing.Size(109, 16)
		Me.lbl52.TabIndex = 37
		Me.lbl52.Text = "Heating (PB)"
		Me.lbl52.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.lbl52.Visible = false
		'
		'Lbl4
		'
		Me.Lbl4.BackColor = System.Drawing.SystemColors.ButtonHighlight
		Me.Lbl4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		Me.Lbl4.Font = New System.Drawing.Font("Meiryo UI", 8.25!)
		Me.Lbl4.Location = New System.Drawing.Point(13, 23)
		Me.Lbl4.Name = "Lbl4"
		Me.Lbl4.Size = New System.Drawing.Size(109, 16)
		Me.Lbl4.TabIndex = 35
		Me.Lbl4.Text = "Measuring2"
		Me.Lbl4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.Lbl4.Visible = false
		'
		'Lbl5
		'
		Me.Lbl5.BackColor = System.Drawing.SystemColors.ButtonHighlight
		Me.Lbl5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		Me.Lbl5.Font = New System.Drawing.Font("Meiryo UI", 8.25!)
		Me.Lbl5.Location = New System.Drawing.Point(13, 39)
		Me.Lbl5.Name = "Lbl5"
		Me.Lbl5.Size = New System.Drawing.Size(109, 16)
		Me.Lbl5.TabIndex = 34
		Me.Lbl5.Text = "Ready Meas."
		Me.Lbl5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.Lbl5.Visible = false
		'
		'Lbl51
		'
		Me.Lbl51.BackColor = System.Drawing.SystemColors.ButtonHighlight
		Me.Lbl51.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		Me.Lbl51.Font = New System.Drawing.Font("Meiryo UI", 8.25!)
		Me.Lbl51.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.Lbl51.Location = New System.Drawing.Point(13, 151)
		Me.Lbl51.Name = "Lbl51"
		Me.Lbl51.Size = New System.Drawing.Size(109, 16)
		Me.Lbl51.TabIndex = 36
		Me.Lbl51.Text = "Lifting"
		Me.Lbl51.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.Lbl51.Visible = false
		'
		'Lbl6
		'
		Me.Lbl6.BackColor = System.Drawing.SystemColors.ButtonHighlight
		Me.Lbl6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		Me.Lbl6.Font = New System.Drawing.Font("Meiryo UI", 8.25!)
		Me.Lbl6.Location = New System.Drawing.Point(13, 55)
		Me.Lbl6.Name = "Lbl6"
		Me.Lbl6.Size = New System.Drawing.Size(109, 16)
		Me.Lbl6.TabIndex = 33
		Me.Lbl6.Text = "Measuring"
		Me.Lbl6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.Lbl6.Visible = false
		'
		'Lbl7
		'
		Me.Lbl7.BackColor = System.Drawing.SystemColors.ButtonHighlight
		Me.Lbl7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		Me.Lbl7.Font = New System.Drawing.Font("Meiryo UI", 8.25!)
		Me.Lbl7.Location = New System.Drawing.Point(13, 71)
		Me.Lbl7.Name = "Lbl7"
		Me.Lbl7.Size = New System.Drawing.Size(109, 16)
		Me.Lbl7.TabIndex = 32
		Me.Lbl7.Text = "Zero Adj."
		Me.Lbl7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.Lbl7.Visible = false
		'
		'Lbl50
		'
		Me.Lbl50.BackColor = System.Drawing.SystemColors.ButtonHighlight
		Me.Lbl50.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		Me.Lbl50.Font = New System.Drawing.Font("Meiryo UI", 8.25!)
		Me.Lbl50.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.Lbl50.Location = New System.Drawing.Point(13, 135)
		Me.Lbl50.Name = "Lbl50"
		Me.Lbl50.Size = New System.Drawing.Size(109, 16)
		Me.Lbl50.TabIndex = 36
		Me.Lbl50.Text = "XY In Action"
		Me.Lbl50.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.Lbl50.Visible = false
		'
		'Lbl8
		'
		Me.Lbl8.BackColor = System.Drawing.SystemColors.ButtonHighlight
		Me.Lbl8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		Me.Lbl8.Font = New System.Drawing.Font("Meiryo UI", 8.25!)
		Me.Lbl8.Location = New System.Drawing.Point(13, 87)
		Me.Lbl8.Name = "Lbl8"
		Me.Lbl8.Size = New System.Drawing.Size(109, 16)
		Me.Lbl8.TabIndex = 31
		Me.Lbl8.Text = "Heating (HP)"
		Me.Lbl8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.Lbl8.Visible = false
		'
		'Lbl9
		'
		Me.Lbl9.BackColor = System.Drawing.SystemColors.ButtonHighlight
		Me.Lbl9.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		Me.Lbl9.Font = New System.Drawing.Font("Meiryo UI", 8.25!)
		Me.Lbl9.Location = New System.Drawing.Point(13, 119)
		Me.Lbl9.Name = "Lbl9"
		Me.Lbl9.Size = New System.Drawing.Size(109, 16)
		Me.Lbl9.TabIndex = 30
		Me.Lbl9.Text = "H.P. detection"
		Me.Lbl9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.Lbl9.Visible = false
		'
		'OpenFileDialog1
		'
		Me.OpenFileDialog1.FileName = "OpenFileDialog1"
		'
		'btnTemp
		'
		Me.btnTemp.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left),System.Windows.Forms.AnchorStyles)
		Me.btnTemp.BackColor = System.Drawing.Color.LightGray
		Me.btnTemp.Cursor = System.Windows.Forms.Cursors.Hand
		Me.btnTemp.Enabled = false
		Me.btnTemp.Font = New System.Drawing.Font("Meiryo UI", 15.75!, System.Drawing.FontStyle.Bold)
		Me.btnTemp.Image = CType(resources.GetObject("btnTemp.Image"),System.Drawing.Image)
		Me.btnTemp.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me.btnTemp.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.btnTemp.Location = New System.Drawing.Point(4, 965)
		Me.btnTemp.Name = "btnTemp"
		Me.btnTemp.Size = New System.Drawing.Size(160, 46)
		Me.btnTemp.TabIndex = 39
		Me.btnTemp.Text = "Temp"
		Me.ToolTipMain.SetToolTip(Me.btnTemp, "Heater Switch")
		Me.btnTemp.UseVisualStyleBackColor = false
		'
		'btnZero
		'
		Me.btnZero.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left),System.Windows.Forms.AnchorStyles)
		Me.btnZero.BackColor = System.Drawing.Color.LightGray
		Me.btnZero.Cursor = System.Windows.Forms.Cursors.Hand
		Me.btnZero.Enabled = false
		Me.btnZero.Font = New System.Drawing.Font("Meiryo UI", 15.75!, System.Drawing.FontStyle.Bold)
		Me.btnZero.Image = CType(resources.GetObject("btnZero.Image"),System.Drawing.Image)
		Me.btnZero.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me.btnZero.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.btnZero.Location = New System.Drawing.Point(169, 965)
		Me.btnZero.Name = "btnZero"
		Me.btnZero.Size = New System.Drawing.Size(160, 46)
		Me.btnZero.TabIndex = 40
		Me.btnZero.Text = "Zero"
		Me.ToolTipMain.SetToolTip(Me.btnZero, "Zero Switch")
		Me.btnZero.UseVisualStyleBackColor = false
		'
		'nudYAxisZoom
		'
		Me.nudYAxisZoom.DecimalPlaces = 2
		Me.nudYAxisZoom.Font = New System.Drawing.Font("Meiryo UI", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.nudYAxisZoom.Increment = New Decimal(New Integer() {10, 0, 0, 0})
		Me.nudYAxisZoom.Location = New System.Drawing.Point(3, 77)
		Me.nudYAxisZoom.Maximum = New Decimal(New Integer() {100000, 0, 0, 0})
		Me.nudYAxisZoom.Minimum = New Decimal(New Integer() {1, 0, 0, 131072})
		Me.nudYAxisZoom.Name = "nudYAxisZoom"
		Me.nudYAxisZoom.Size = New System.Drawing.Size(136, 32)
		Me.nudYAxisZoom.TabIndex = 81
		Me.nudYAxisZoom.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		Me.ToolTipMain.SetToolTip(Me.nudYAxisZoom, "Y軸の表示範囲[%]")
		Me.nudYAxisZoom.Value = New Decimal(New Integer() {100, 0, 0, 0})
		Me.nudYAxisZoom.Visible = false
		'
		'nudYscaleMax
		'
		Me.nudYscaleMax.DecimalPlaces = 3
		Me.nudYscaleMax.Font = New System.Drawing.Font("Meiryo UI", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.nudYscaleMax.Location = New System.Drawing.Point(143, 14)
		Me.nudYscaleMax.Maximum = New Decimal(New Integer() {99999999, 0, 0, 0})
		Me.nudYscaleMax.Minimum = New Decimal(New Integer() {99999999, 0, 0, -2147483648})
		Me.nudYscaleMax.Name = "nudYscaleMax"
		Me.nudYscaleMax.Size = New System.Drawing.Size(114, 28)
		Me.nudYscaleMax.TabIndex = 73
		Me.ToolTipMain.SetToolTip(Me.nudYscaleMax, "Span maximum")
		Me.nudYscaleMax.Value = New Decimal(New Integer() {1, 0, 0, 0})
		'
		'nudYscaleMin
		'
		Me.nudYscaleMin.DecimalPlaces = 3
		Me.nudYscaleMin.Font = New System.Drawing.Font("Meiryo UI", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.nudYscaleMin.Location = New System.Drawing.Point(10, 14)
		Me.nudYscaleMin.Maximum = New Decimal(New Integer() {99999999, 0, 0, 0})
		Me.nudYscaleMin.Minimum = New Decimal(New Integer() {99999999, 0, 0, -2147483648})
		Me.nudYscaleMin.Name = "nudYscaleMin"
		Me.nudYscaleMin.Size = New System.Drawing.Size(117, 28)
		Me.nudYscaleMin.TabIndex = 72
		Me.ToolTipMain.SetToolTip(Me.nudYscaleMin, "Span minimum")
		'
		'nudXscaleMax
		'
		Me.nudXscaleMax.Font = New System.Drawing.Font("Meiryo UI", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.nudXscaleMax.Location = New System.Drawing.Point(143, 14)
		Me.nudXscaleMax.Maximum = New Decimal(New Integer() {4000, 0, 0, 0})
		Me.nudXscaleMax.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
		Me.nudXscaleMax.Name = "nudXscaleMax"
		Me.nudXscaleMax.Size = New System.Drawing.Size(114, 28)
		Me.nudXscaleMax.TabIndex = 68
		Me.ToolTipMain.SetToolTip(Me.nudXscaleMax, "Span maximum")
		Me.nudXscaleMax.Value = New Decimal(New Integer() {1, 0, 0, 0})
		'
		'nudXscaleMin
		'
		Me.nudXscaleMin.Font = New System.Drawing.Font("Meiryo UI", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.nudXscaleMin.Location = New System.Drawing.Point(10, 14)
		Me.nudXscaleMin.Maximum = New Decimal(New Integer() {4000, 0, 0, 0})
		Me.nudXscaleMin.Name = "nudXscaleMin"
		Me.nudXscaleMin.Size = New System.Drawing.Size(118, 28)
		Me.nudXscaleMin.TabIndex = 67
		Me.ToolTipMain.SetToolTip(Me.nudXscaleMin, "Span minimum")
		'
		'nudXdivision
		'
		Me.nudXdivision.Font = New System.Drawing.Font("Meiryo UI", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.nudXdivision.Location = New System.Drawing.Point(10, 48)
		Me.nudXdivision.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
		Me.nudXdivision.Name = "nudXdivision"
		Me.nudXdivision.Size = New System.Drawing.Size(118, 28)
		Me.nudXdivision.TabIndex = 69
		Me.ToolTipMain.SetToolTip(Me.nudXdivision, "Division number")
		Me.nudXdivision.Value = New Decimal(New Integer() {1, 0, 0, 0})
		'
		'nudYdivision
		'
		Me.nudYdivision.Font = New System.Drawing.Font("Meiryo UI", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.nudYdivision.Location = New System.Drawing.Point(10, 46)
		Me.nudYdivision.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
		Me.nudYdivision.Name = "nudYdivision"
		Me.nudYdivision.Size = New System.Drawing.Size(118, 28)
		Me.nudYdivision.TabIndex = 74
		Me.ToolTipMain.SetToolTip(Me.nudYdivision, "Division number")
		Me.nudYdivision.Value = New Decimal(New Integer() {1, 0, 0, 0})
		'
		'StatusStrip1
		'
		Me.StatusStrip1.ImageScalingSize = New System.Drawing.Size(20, 20)
		Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripStatusLabel1})
		Me.StatusStrip1.Location = New System.Drawing.Point(0, 1016)
		Me.StatusStrip1.Name = "StatusStrip1"
		Me.StatusStrip1.Size = New System.Drawing.Size(1904, 25)
		Me.StatusStrip1.TabIndex = 34
		Me.StatusStrip1.Text = "StatusStrip1"
		'
		'ToolStripStatusLabel1
		'
		Me.ToolStripStatusLabel1.Font = New System.Drawing.Font("Meiryo UI", 12!)
		Me.ToolStripStatusLabel1.Name = "ToolStripStatusLabel1"
		Me.ToolStripStatusLabel1.Size = New System.Drawing.Size(57, 20)
		Me.ToolStripStatusLabel1.Text = "Ready"
		'
		'MenuStrip1
		'
		Me.MenuStrip1.Dock = System.Windows.Forms.DockStyle.None
		Me.MenuStrip1.Font = New System.Drawing.Font("Yu Gothic UI", 12!)
		Me.MenuStrip1.ImageScalingSize = New System.Drawing.Size(20, 20)
		Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsmiFile, Me.tsmiGraph, Me.tsmiMeasure, Me.tsmiRemote, Me.tsmiSettings, Me.tsmiHelp})
		Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
		Me.MenuStrip1.Name = "MenuStrip1"
		Me.MenuStrip1.Padding = New System.Windows.Forms.Padding(4, 2, 0, 2)
		Me.MenuStrip1.Size = New System.Drawing.Size(407, 29)
		Me.MenuStrip1.TabIndex = 35
		Me.MenuStrip1.Text = "MenuStrip1"
		'
		'tsmiFile
		'
		Me.tsmiFile.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsmiPrint, Me.ToolStripSeparator2, Me.tsmiImport, Me.tsmiExport, Me.ToolStripSeparator1, Me.tsmiExit})
		Me.tsmiFile.Name = "tsmiFile"
		Me.tsmiFile.Size = New System.Drawing.Size(46, 25)
		Me.tsmiFile.Text = "&File"
		'
		'tsmiPrint
		'
		Me.tsmiPrint.Image = CType(resources.GetObject("tsmiPrint.Image"),System.Drawing.Image)
		Me.tsmiPrint.ImageTransparentColor = System.Drawing.Color.Magenta
		Me.tsmiPrint.Name = "tsmiPrint"
		Me.tsmiPrint.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.P),System.Windows.Forms.Keys)
		Me.tsmiPrint.Size = New System.Drawing.Size(181, 26)
		Me.tsmiPrint.Text = "&Print..."
		'
		'ToolStripSeparator2
		'
		Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
		Me.ToolStripSeparator2.Size = New System.Drawing.Size(178, 6)
		Me.ToolStripSeparator2.Visible = false
		'
		'tsmiImport
		'
		Me.tsmiImport.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsmiMeasAreaImport, Me.tsmiStdAreaImport})
		Me.tsmiImport.Image = CType(resources.GetObject("tsmiImport.Image"),System.Drawing.Image)
		Me.tsmiImport.Name = "tsmiImport"
		Me.tsmiImport.Size = New System.Drawing.Size(181, 26)
		Me.tsmiImport.Text = "&Import"
		'
		'tsmiMeasAreaImport
		'
		Me.tsmiMeasAreaImport.Image = CType(resources.GetObject("tsmiMeasAreaImport.Image"),System.Drawing.Image)
		Me.tsmiMeasAreaImport.Name = "tsmiMeasAreaImport"
		Me.tsmiMeasAreaImport.Size = New System.Drawing.Size(214, 26)
		Me.tsmiMeasAreaImport.Text = "Measurement area"
		'
		'tsmiStdAreaImport
		'
		Me.tsmiStdAreaImport.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsmiStdAreaImport1, Me.tsmiStdAreaImport2})
		Me.tsmiStdAreaImport.Image = CType(resources.GetObject("tsmiStdAreaImport.Image"),System.Drawing.Image)
		Me.tsmiStdAreaImport.Name = "tsmiStdAreaImport"
		Me.tsmiStdAreaImport.Size = New System.Drawing.Size(214, 26)
		Me.tsmiStdAreaImport.Text = "Standard area"
		'
		'tsmiStdAreaImport1
		'
		Me.tsmiStdAreaImport1.Name = "tsmiStdAreaImport1"
		Me.tsmiStdAreaImport1.Size = New System.Drawing.Size(125, 26)
		Me.tsmiStdAreaImport1.Text = "Area &1"
		'
		'tsmiStdAreaImport2
		'
		Me.tsmiStdAreaImport2.Name = "tsmiStdAreaImport2"
		Me.tsmiStdAreaImport2.Size = New System.Drawing.Size(125, 26)
		Me.tsmiStdAreaImport2.Text = "Area &2"
		'
		'tsmiExport
		'
		Me.tsmiExport.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsmiStandardExp, Me.tsmiTextData})
		Me.tsmiExport.Image = CType(resources.GetObject("tsmiExport.Image"),System.Drawing.Image)
		Me.tsmiExport.Name = "tsmiExport"
		Me.tsmiExport.Size = New System.Drawing.Size(181, 26)
		Me.tsmiExport.Text = "&Export"
		'
		'tsmiStandardExp
		'
		Me.tsmiStandardExp.Image = CType(resources.GetObject("tsmiStandardExp.Image"),System.Drawing.Image)
		Me.tsmiStandardExp.ImageTransparentColor = System.Drawing.Color.Magenta
		Me.tsmiStandardExp.Name = "tsmiStandardExp"
		Me.tsmiStandardExp.Size = New System.Drawing.Size(183, 26)
		Me.tsmiStandardExp.Text = "Standard Data"
		'
		'tsmiTextData
		'
		Me.tsmiTextData.Image = CType(resources.GetObject("tsmiTextData.Image"),System.Drawing.Image)
		Me.tsmiTextData.Name = "tsmiTextData"
		Me.tsmiTextData.Size = New System.Drawing.Size(183, 26)
		Me.tsmiTextData.Text = "Text Data"
		'
		'ToolStripSeparator1
		'
		Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
		Me.ToolStripSeparator1.Size = New System.Drawing.Size(178, 6)
		'
		'tsmiExit
		'
		Me.tsmiExit.Image = CType(resources.GetObject("tsmiExit.Image"),System.Drawing.Image)
		Me.tsmiExit.Name = "tsmiExit"
		Me.tsmiExit.ShortcutKeys = CType((System.Windows.Forms.Keys.Alt Or System.Windows.Forms.Keys.F4),System.Windows.Forms.Keys)
		Me.tsmiExit.Size = New System.Drawing.Size(181, 26)
		Me.tsmiExit.Text = "E&xit"
		'
		'tsmiGraph
		'
		Me.tsmiGraph.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsmiRecalculation, Me.ToolStripSeparator9, Me.tsmiHistoryDraw, Me.tsmiHistoryClear, Me.ToolStripSeparator5, Me.tsmiStandardDraw, Me.tsmiStandardClear, Me.ToolStripSeparator10, Me.tsmiAllClear, Me.ToolStripSeparator12, Me.tsmiFixedX, Me.tsmiFixedY, Me.ToolStripSeparator4, Me.tsmiXAxisTemp, Me.ToolStripSeparator13, Me.tsmiDigitalMonitorChange, Me.ToolStripSeparator17, Me.tsmiSaveGraph})
		Me.tsmiGraph.Name = "tsmiGraph"
		Me.tsmiGraph.Size = New System.Drawing.Size(65, 25)
		Me.tsmiGraph.Text = "&Graph"
		'
		'tsmiRecalculation
		'
		Me.tsmiRecalculation.Enabled = false
		Me.tsmiRecalculation.Image = CType(resources.GetObject("tsmiRecalculation.Image"),System.Drawing.Image)
		Me.tsmiRecalculation.ImageTransparentColor = System.Drawing.Color.Magenta
		Me.tsmiRecalculation.Name = "tsmiRecalculation"
		Me.tsmiRecalculation.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.R),System.Windows.Forms.Keys)
		Me.tsmiRecalculation.Size = New System.Drawing.Size(315, 26)
		Me.tsmiRecalculation.Text = "&Recalculation"
		Me.tsmiRecalculation.Visible = false
		'
		'ToolStripSeparator9
		'
		Me.ToolStripSeparator9.Name = "ToolStripSeparator9"
		Me.ToolStripSeparator9.Size = New System.Drawing.Size(312, 6)
		Me.ToolStripSeparator9.Visible = false
		'
		'tsmiHistoryDraw
		'
		Me.tsmiHistoryDraw.Checked = true
		Me.tsmiHistoryDraw.CheckState = System.Windows.Forms.CheckState.Checked
		Me.tsmiHistoryDraw.Name = "tsmiHistoryDraw"
		Me.tsmiHistoryDraw.Size = New System.Drawing.Size(315, 26)
		Me.tsmiHistoryDraw.Text = "History drawing"
		'
		'tsmiHistoryClear
		'
		Me.tsmiHistoryClear.Name = "tsmiHistoryClear"
		Me.tsmiHistoryClear.Size = New System.Drawing.Size(315, 26)
		Me.tsmiHistoryClear.Text = "History Clear"
		'
		'ToolStripSeparator5
		'
		Me.ToolStripSeparator5.Name = "ToolStripSeparator5"
		Me.ToolStripSeparator5.Size = New System.Drawing.Size(312, 6)
		'
		'tsmiStandardDraw
		'
		Me.tsmiStandardDraw.Checked = true
		Me.tsmiStandardDraw.CheckState = System.Windows.Forms.CheckState.Checked
		Me.tsmiStandardDraw.Name = "tsmiStandardDraw"
		Me.tsmiStandardDraw.Size = New System.Drawing.Size(315, 26)
		Me.tsmiStandardDraw.Text = "Standard drawing"
		'
		'tsmiStandardClear
		'
		Me.tsmiStandardClear.Name = "tsmiStandardClear"
		Me.tsmiStandardClear.Size = New System.Drawing.Size(315, 26)
		Me.tsmiStandardClear.Text = "Standard Clear"
		'
		'ToolStripSeparator10
		'
		Me.ToolStripSeparator10.Name = "ToolStripSeparator10"
		Me.ToolStripSeparator10.Size = New System.Drawing.Size(312, 6)
		'
		'tsmiAllClear
		'
		Me.tsmiAllClear.Name = "tsmiAllClear"
		Me.tsmiAllClear.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.F9),System.Windows.Forms.Keys)
		Me.tsmiAllClear.Size = New System.Drawing.Size(315, 26)
		Me.tsmiAllClear.Text = "All Clear"
		'
		'ToolStripSeparator12
		'
		Me.ToolStripSeparator12.Name = "ToolStripSeparator12"
		Me.ToolStripSeparator12.Size = New System.Drawing.Size(312, 6)
		'
		'tsmiFixedX
		'
		Me.tsmiFixedX.Name = "tsmiFixedX"
		Me.tsmiFixedX.Size = New System.Drawing.Size(315, 26)
		Me.tsmiFixedX.Text = "Fixed X axis"
		'
		'tsmiFixedY
		'
		Me.tsmiFixedY.Name = "tsmiFixedY"
		Me.tsmiFixedY.Size = New System.Drawing.Size(315, 26)
		Me.tsmiFixedY.Text = "Fixed Y axis"
		'
		'ToolStripSeparator4
		'
		Me.ToolStripSeparator4.Name = "ToolStripSeparator4"
		Me.ToolStripSeparator4.Size = New System.Drawing.Size(312, 6)
		'
		'tsmiXAxisTemp
		'
		Me.tsmiXAxisTemp.Name = "tsmiXAxisTemp"
		Me.tsmiXAxisTemp.Size = New System.Drawing.Size(315, 26)
		Me.tsmiXAxisTemp.Text = "X axis temperature"
		'
		'ToolStripSeparator13
		'
		Me.ToolStripSeparator13.Name = "ToolStripSeparator13"
		Me.ToolStripSeparator13.Size = New System.Drawing.Size(312, 6)
		'
		'tsmiDigitalMonitorChange
		'
		Me.tsmiDigitalMonitorChange.Image = CType(resources.GetObject("tsmiDigitalMonitorChange.Image"),System.Drawing.Image)
		Me.tsmiDigitalMonitorChange.Name = "tsmiDigitalMonitorChange"
		Me.tsmiDigitalMonitorChange.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.F12),System.Windows.Forms.Keys)
		Me.tsmiDigitalMonitorChange.Size = New System.Drawing.Size(315, 26)
		Me.tsmiDigitalMonitorChange.Text = "Digital Monitor change"
		'
		'ToolStripSeparator17
		'
		Me.ToolStripSeparator17.Name = "ToolStripSeparator17"
		Me.ToolStripSeparator17.Size = New System.Drawing.Size(312, 6)
		'
		'tsmiSaveGraph
		'
		Me.tsmiSaveGraph.Name = "tsmiSaveGraph"
		Me.tsmiSaveGraph.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.F11),System.Windows.Forms.Keys)
		Me.tsmiSaveGraph.Size = New System.Drawing.Size(315, 26)
		Me.tsmiSaveGraph.Text = "Save graph to file"
		'
		'tsmiMeasure
		'
		Me.tsmiMeasure.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsmiGelTime, Me.tsmiFlowCurve})
		Me.tsmiMeasure.Name = "tsmiMeasure"
		Me.tsmiMeasure.Size = New System.Drawing.Size(82, 25)
		Me.tsmiMeasure.Text = "&Measure"
		'
		'tsmiGelTime
		'
		Me.tsmiGelTime.Checked = true
		Me.tsmiGelTime.CheckState = System.Windows.Forms.CheckState.Checked
		Me.tsmiGelTime.Image = CType(resources.GetObject("tsmiGelTime.Image"),System.Drawing.Image)
		Me.tsmiGelTime.Name = "tsmiGelTime"
		Me.tsmiGelTime.Size = New System.Drawing.Size(162, 26)
		Me.tsmiGelTime.Text = "Gel Time"
		'
		'tsmiFlowCurve
		'
		Me.tsmiFlowCurve.Image = CType(resources.GetObject("tsmiFlowCurve.Image"),System.Drawing.Image)
		Me.tsmiFlowCurve.Name = "tsmiFlowCurve"
		Me.tsmiFlowCurve.Size = New System.Drawing.Size(162, 26)
		Me.tsmiFlowCurve.Text = "Flow Curve"
		'
		'tsmiRemote
		'
		Me.tsmiRemote.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsmiTemp, Me.tsmiZeroAdj, Me.ToolStripMenuItem1, Me.tsmiResetError, Me.tsmiReturnOrigin, Me.ToolStripSeparator14, Me.tsmiLEDLight, Me.tsmi7SegBright})
		Me.tsmiRemote.Name = "tsmiRemote"
		Me.tsmiRemote.Size = New System.Drawing.Size(76, 25)
		Me.tsmiRemote.Text = "&Remote"
		'
		'tsmiTemp
		'
		Me.tsmiTemp.Name = "tsmiTemp"
		Me.tsmiTemp.ShortcutKeys = System.Windows.Forms.Keys.F1
		Me.tsmiTemp.Size = New System.Drawing.Size(286, 26)
		Me.tsmiTemp.Text = "HP Temperature Control"
		'
		'tsmiZeroAdj
		'
		Me.tsmiZeroAdj.Name = "tsmiZeroAdj"
		Me.tsmiZeroAdj.ShortcutKeys = System.Windows.Forms.Keys.F2
		Me.tsmiZeroAdj.Size = New System.Drawing.Size(286, 26)
		Me.tsmiZeroAdj.Text = "Zero adjustmennt Control"
		'
		'ToolStripMenuItem1
		'
		Me.ToolStripMenuItem1.Name = "ToolStripMenuItem1"
		Me.ToolStripMenuItem1.ShortcutKeys = System.Windows.Forms.Keys.F3
		Me.ToolStripMenuItem1.Size = New System.Drawing.Size(286, 26)
		Me.ToolStripMenuItem1.Text = "Measurement Control"
		'
		'tsmiResetError
		'
		Me.tsmiResetError.Name = "tsmiResetError"
		Me.tsmiResetError.ShortcutKeys = System.Windows.Forms.Keys.F4
		Me.tsmiResetError.Size = New System.Drawing.Size(286, 26)
		Me.tsmiResetError.Text = "Reset &Error Condition"
		'
		'tsmiReturnOrigin
		'
		Me.tsmiReturnOrigin.Name = "tsmiReturnOrigin"
		Me.tsmiReturnOrigin.ShortcutKeys = System.Windows.Forms.Keys.F5
		Me.tsmiReturnOrigin.Size = New System.Drawing.Size(286, 26)
		Me.tsmiReturnOrigin.Text = "Return Origin"
		'
		'ToolStripSeparator14
		'
		Me.ToolStripSeparator14.Name = "ToolStripSeparator14"
		Me.ToolStripSeparator14.Size = New System.Drawing.Size(283, 6)
		'
		'tsmiLEDLight
		'
		Me.tsmiLEDLight.Name = "tsmiLEDLight"
		Me.tsmiLEDLight.ShortcutKeys = System.Windows.Forms.Keys.F10
		Me.tsmiLEDLight.Size = New System.Drawing.Size(286, 26)
		Me.tsmiLEDLight.Text = "LED Light"
		'
		'tsmi7SegBright
		'
		Me.tsmi7SegBright.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsmiCmb7SegBright})
		Me.tsmi7SegBright.Name = "tsmi7SegBright"
		Me.tsmi7SegBright.Size = New System.Drawing.Size(286, 26)
		Me.tsmi7SegBright.Text = "7Seg LED Brightness"
		'
		'tsmiCmb7SegBright
		'
		Me.tsmiCmb7SegBright.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.tsmiCmb7SegBright.DropDownWidth = 60
		Me.tsmiCmb7SegBright.Font = New System.Drawing.Font("Yu Gothic UI", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.tsmiCmb7SegBright.MaxDropDownItems = 15
		Me.tsmiCmb7SegBright.Name = "tsmiCmb7SegBright"
		Me.tsmiCmb7SegBright.Size = New System.Drawing.Size(121, 29)
		'
		'tsmiSettings
		'
		Me.tsmiSettings.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsmiOptions, Me.ToolStripSeparator6, Me.tsmiCaliblation, Me.tsmiHorizontalConfirmation, Me.ToolStripSeparator7, Me.tsmiStressZeroSet, Me.ToolStripSeparator8, Me.tsmiFixToFree, Me.ToolStripSeparator18, Me.tsmiAdminMode, Me.ToolStripSeparator19, Me.tsmiQrCodeCreate})
		Me.tsmiSettings.Name = "tsmiSettings"
		Me.tsmiSettings.Size = New System.Drawing.Size(78, 25)
		Me.tsmiSettings.Text = "&Settings"
		'
		'tsmiOptions
		'
		Me.tsmiOptions.Image = CType(resources.GetObject("tsmiOptions.Image"),System.Drawing.Image)
		Me.tsmiOptions.ImageTransparentColor = System.Drawing.Color.Magenta
		Me.tsmiOptions.Name = "tsmiOptions"
		Me.tsmiOptions.ShortcutKeys = CType((System.Windows.Forms.Keys.Alt Or System.Windows.Forms.Keys.F1),System.Windows.Forms.Keys)
		Me.tsmiOptions.Size = New System.Drawing.Size(249, 26)
		Me.tsmiOptions.Text = "&Options"
		'
		'ToolStripSeparator6
		'
		Me.ToolStripSeparator6.Name = "ToolStripSeparator6"
		Me.ToolStripSeparator6.Size = New System.Drawing.Size(246, 6)
		'
		'tsmiCaliblation
		'
		Me.tsmiCaliblation.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsmiWeight, Me.tsmiFullStress, Me.tsmiZeroStress})
		Me.tsmiCaliblation.Image = CType(resources.GetObject("tsmiCaliblation.Image"),System.Drawing.Image)
		Me.tsmiCaliblation.Name = "tsmiCaliblation"
		Me.tsmiCaliblation.Size = New System.Drawing.Size(249, 26)
		Me.tsmiCaliblation.Text = "&Caliblation"
		'
		'tsmiWeight
		'
		Me.tsmiWeight.Name = "tsmiWeight"
		Me.tsmiWeight.Size = New System.Drawing.Size(156, 26)
		Me.tsmiWeight.Text = "&Weight"
		'
		'tsmiFullStress
		'
		Me.tsmiFullStress.Name = "tsmiFullStress"
		Me.tsmiFullStress.Size = New System.Drawing.Size(156, 26)
		Me.tsmiFullStress.Text = "&Full stress"
		'
		'tsmiZeroStress
		'
		Me.tsmiZeroStress.Name = "tsmiZeroStress"
		Me.tsmiZeroStress.Size = New System.Drawing.Size(156, 26)
		Me.tsmiZeroStress.Text = "&Zero stress"
		'
		'tsmiHorizontalConfirmation
		'
		Me.tsmiHorizontalConfirmation.Image = CType(resources.GetObject("tsmiHorizontalConfirmation.Image"),System.Drawing.Image)
		Me.tsmiHorizontalConfirmation.Name = "tsmiHorizontalConfirmation"
		Me.tsmiHorizontalConfirmation.Size = New System.Drawing.Size(249, 26)
		Me.tsmiHorizontalConfirmation.Text = "&Horizontal confirmation"
		'
		'ToolStripSeparator7
		'
		Me.ToolStripSeparator7.Name = "ToolStripSeparator7"
		Me.ToolStripSeparator7.Size = New System.Drawing.Size(246, 6)
		'
		'tsmiStressZeroSet
		'
		Me.tsmiStressZeroSet.Name = "tsmiStressZeroSet"
		Me.tsmiStressZeroSet.Size = New System.Drawing.Size(249, 26)
		Me.tsmiStressZeroSet.Text = "Stress Zero Enable"
		'
		'ToolStripSeparator8
		'
		Me.ToolStripSeparator8.Name = "ToolStripSeparator8"
		Me.ToolStripSeparator8.Size = New System.Drawing.Size(246, 6)
		'
		'tsmiFixToFree
		'
		Me.tsmiFixToFree.Image = CType(resources.GetObject("tsmiFixToFree.Image"),System.Drawing.Image)
		Me.tsmiFixToFree.Name = "tsmiFixToFree"
		Me.tsmiFixToFree.Size = New System.Drawing.Size(249, 26)
		Me.tsmiFixToFree.Text = "Fix -> Free Area Copy"
		Me.tsmiFixToFree.Visible = false
		'
		'ToolStripSeparator18
		'
		Me.ToolStripSeparator18.Name = "ToolStripSeparator18"
		Me.ToolStripSeparator18.Size = New System.Drawing.Size(246, 6)
		Me.ToolStripSeparator18.Visible = false
		'
		'tsmiAdminMode
		'
		Me.tsmiAdminMode.Image = CType(resources.GetObject("tsmiAdminMode.Image"),System.Drawing.Image)
		Me.tsmiAdminMode.Name = "tsmiAdminMode"
		Me.tsmiAdminMode.Size = New System.Drawing.Size(249, 26)
		Me.tsmiAdminMode.Text = "&Admin mode"
		'
		'ToolStripSeparator19
		'
		Me.ToolStripSeparator19.Name = "ToolStripSeparator19"
		Me.ToolStripSeparator19.Size = New System.Drawing.Size(246, 6)
		'
		'tsmiQrCodeCreate
		'
		Me.tsmiQrCodeCreate.Name = "tsmiQrCodeCreate"
		Me.tsmiQrCodeCreate.Size = New System.Drawing.Size(249, 26)
		Me.tsmiQrCodeCreate.Text = "&QR code creation"
		'
		'tsmiHelp
		'
		Me.tsmiHelp.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsmiShowHelp, Me.ToolStripSeparator3, Me.tsmiMent, Me.ToolStripSeparator20, Me.tsmiAbout})
		Me.tsmiHelp.Name = "tsmiHelp"
		Me.tsmiHelp.Size = New System.Drawing.Size(54, 25)
		Me.tsmiHelp.Text = "&Help"
		'
		'tsmiShowHelp
		'
		Me.tsmiShowHelp.Image = CType(resources.GetObject("tsmiShowHelp.Image"),System.Drawing.Image)
		Me.tsmiShowHelp.ImageTransparentColor = System.Drawing.Color.Magenta
		Me.tsmiShowHelp.Name = "tsmiShowHelp"
		Me.tsmiShowHelp.Size = New System.Drawing.Size(169, 26)
		Me.tsmiShowHelp.Text = "Show &Help"
		'
		'ToolStripSeparator3
		'
		Me.ToolStripSeparator3.Name = "ToolStripSeparator3"
		Me.ToolStripSeparator3.Size = New System.Drawing.Size(166, 6)
		'
		'tsmiMent
		'
		Me.tsmiMent.Name = "tsmiMent"
		Me.tsmiMent.Size = New System.Drawing.Size(169, 26)
		Me.tsmiMent.Text = "&Mentenance"
		'
		'ToolStripSeparator20
		'
		Me.ToolStripSeparator20.Name = "ToolStripSeparator20"
		Me.ToolStripSeparator20.Size = New System.Drawing.Size(166, 6)
		'
		'tsmiAbout
		'
		Me.tsmiAbout.Name = "tsmiAbout"
		Me.tsmiAbout.Size = New System.Drawing.Size(169, 26)
		Me.tsmiAbout.Text = "&About"
		'
		'cboEquipName
		'
		Me.cboEquipName.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
		Me.cboEquipName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.cboEquipName.Enabled = false
		Me.cboEquipName.FlatStyle = System.Windows.Forms.FlatStyle.Popup
		Me.cboEquipName.Font = New System.Drawing.Font("Meiryo UI", 12!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.cboEquipName.FormattingEnabled = true
		Me.cboEquipName.Items.AddRange(New Object() {"1", "2", "3", "4", "5", "6", "7", "8", "9", "10"})
		Me.cboEquipName.Location = New System.Drawing.Point(1628, 1)
		Me.cboEquipName.MinimumSize = New System.Drawing.Size(85, 0)
		Me.cboEquipName.Name = "cboEquipName"
		Me.cboEquipName.Size = New System.Drawing.Size(140, 28)
		Me.cboEquipName.TabIndex = 37
		'
		'lblEquipment
		'
		Me.lblEquipment.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
		Me.lblEquipment.BackColor = System.Drawing.Color.Transparent
		Me.lblEquipment.Font = New System.Drawing.Font("Meiryo UI", 12!)
		Me.lblEquipment.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.lblEquipment.Location = New System.Drawing.Point(1532, 3)
		Me.lblEquipment.Name = "lblEquipment"
		Me.lblEquipment.Size = New System.Drawing.Size(95, 24)
		Me.lblEquipment.TabIndex = 38
		Me.lblEquipment.Text = "Equipment"
		Me.lblEquipment.TextAlign = System.Drawing.ContentAlignment.MiddleRight
		'
		'btnErrReset
		'
		Me.btnErrReset.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left),System.Windows.Forms.AnchorStyles)
		Me.btnErrReset.BackColor = System.Drawing.Color.LightGray
		Me.btnErrReset.Cursor = System.Windows.Forms.Cursors.Hand
		Me.btnErrReset.Font = New System.Drawing.Font("メイリオ", 15.75!)
		Me.btnErrReset.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me.btnErrReset.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.btnErrReset.Location = New System.Drawing.Point(929, 966)
		Me.btnErrReset.Name = "btnErrReset"
		Me.btnErrReset.Size = New System.Drawing.Size(118, 40)
		Me.btnErrReset.TabIndex = 41
		Me.btnErrReset.Text = "Error Reset"
		Me.btnErrReset.UseVisualStyleBackColor = false
		Me.btnErrReset.Visible = false
		'
		'btnMeasure
		'
		Me.btnMeasure.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left),System.Windows.Forms.AnchorStyles)
		Me.btnMeasure.Cursor = System.Windows.Forms.Cursors.Hand
		Me.btnMeasure.Enabled = false
		Me.btnMeasure.Font = New System.Drawing.Font("Meiryo UI", 15.75!, System.Drawing.FontStyle.Bold)
		Me.btnMeasure.Image = CType(resources.GetObject("btnMeasure.Image"),System.Drawing.Image)
		Me.btnMeasure.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me.btnMeasure.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.btnMeasure.Location = New System.Drawing.Point(334, 965)
		Me.btnMeasure.Name = "btnMeasure"
		Me.btnMeasure.Size = New System.Drawing.Size(160, 46)
		Me.btnMeasure.TabIndex = 42
		Me.btnMeasure.Tag = "0"
		Me.btnMeasure.Text = "Meas"
		Me.btnMeasure.UseVisualStyleBackColor = true
		'
		'lblConnectState
		'
		Me.lblConnectState.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
		Me.lblConnectState.BackColor = System.Drawing.Color.LightGray
		Me.lblConnectState.Font = New System.Drawing.Font("Meiryo UI", 14.25!, System.Drawing.FontStyle.Bold)
		Me.lblConnectState.ForeColor = System.Drawing.Color.White
		Me.lblConnectState.ImageKey = "(なし)"
		Me.lblConnectState.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.lblConnectState.Location = New System.Drawing.Point(1777, 1)
		Me.lblConnectState.Name = "lblConnectState"
		Me.lblConnectState.Size = New System.Drawing.Size(126, 28)
		Me.lblConnectState.TabIndex = 43
		Me.lblConnectState.Text = "OFFLINE"
		Me.lblConnectState.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		'
		'pctConStts
		'
		Me.pctConStts.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
		Me.pctConStts.BackColor = System.Drawing.Color.LightGray
		Me.pctConStts.BackgroundImage = CType(resources.GetObject("pctConStts.BackgroundImage"),System.Drawing.Image)
		Me.pctConStts.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
		Me.pctConStts.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.pctConStts.Location = New System.Drawing.Point(1727, 3)
		Me.pctConStts.Name = "pctConStts"
		Me.pctConStts.Size = New System.Drawing.Size(49, 24)
		Me.pctConStts.TabIndex = 44
		Me.pctConStts.TabStop = false
		Me.pctConStts.Visible = false
		'
		'lblDevAlive
		'
		Me.lblDevAlive.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
		Me.lblDevAlive.AutoSize = true
		Me.lblDevAlive.Location = New System.Drawing.Point(1172, 13)
		Me.lblDevAlive.Name = "lblDevAlive"
		Me.lblDevAlive.Size = New System.Drawing.Size(71, 12)
		Me.lblDevAlive.TabIndex = 47
		Me.lblDevAlive.Text = "-----------"
		Me.lblDevAlive.Visible = false
		'
		'btnOrigin
		'
		Me.btnOrigin.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left),System.Windows.Forms.AnchorStyles)
		Me.btnOrigin.BackColor = System.Drawing.Color.LightGray
		Me.btnOrigin.Cursor = System.Windows.Forms.Cursors.Hand
		Me.btnOrigin.Font = New System.Drawing.Font("メイリオ", 15.75!)
		Me.btnOrigin.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me.btnOrigin.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.btnOrigin.Location = New System.Drawing.Point(909, 968)
		Me.btnOrigin.Name = "btnOrigin"
		Me.btnOrigin.Size = New System.Drawing.Size(122, 37)
		Me.btnOrigin.TabIndex = 41
		Me.btnOrigin.Text = "Origin"
		Me.btnOrigin.UseVisualStyleBackColor = false
		Me.btnOrigin.Visible = false
		'
		'lblMachineVer
		'
		Me.lblMachineVer.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
		Me.lblMachineVer.Font = New System.Drawing.Font("Meiryo UI", 12!)
		Me.lblMachineVer.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me.lblMachineVer.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.lblMachineVer.Location = New System.Drawing.Point(1249, 3)
		Me.lblMachineVer.Name = "lblMachineVer"
		Me.lblMachineVer.Size = New System.Drawing.Size(113, 24)
		Me.lblMachineVer.TabIndex = 38
		Me.lblMachineVer.Text = "Machine Ver."
		Me.lblMachineVer.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me.lblMachineVer.Visible = false
		'
		'lblCpuVer
		'
		Me.lblCpuVer.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
		Me.lblCpuVer.BackColor = System.Drawing.SystemColors.Control
		Me.lblCpuVer.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.lblCpuVer.FlatStyle = System.Windows.Forms.FlatStyle.Flat
		Me.lblCpuVer.Font = New System.Drawing.Font("Meiryo UI", 12!)
		Me.lblCpuVer.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.lblCpuVer.Location = New System.Drawing.Point(1368, 5)
		Me.lblCpuVer.Name = "lblCpuVer"
		Me.lblCpuVer.Size = New System.Drawing.Size(146, 20)
		Me.lblCpuVer.TabIndex = 38
		Me.lblCpuVer.Text = "0.00"
		Me.lblCpuVer.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.lblCpuVer.Visible = false
		'
		'pnlSPEC
		'
		Me.pnlSPEC.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
		Me.pnlSPEC.Controls.Add(Me.btnSamSpecEdit)
		Me.pnlSPEC.Controls.Add(Me.btnDevSpecEdit)
		Me.pnlSPEC.Controls.Add(Me.cbxSamSPEC)
		Me.pnlSPEC.Controls.Add(Me.dgvSamSpec)
		Me.pnlSPEC.Controls.Add(Me.lblSamSPECNameLabel)
		Me.pnlSPEC.Controls.Add(Me.dgvDevSpec)
		Me.pnlSPEC.Controls.Add(Me.cbxDevSPEC)
		Me.pnlSPEC.Controls.Add(Me.lblDevSPECNameLabel)
		Me.pnlSPEC.Location = New System.Drawing.Point(1396, 192)
		Me.pnlSPEC.Name = "pnlSPEC"
		Me.pnlSPEC.Size = New System.Drawing.Size(265, 233)
		Me.pnlSPEC.TabIndex = 21
		'
		'btnSamSpecEdit
		'
		Me.btnSamSpecEdit.Font = New System.Drawing.Font("Meiryo UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.btnSamSpecEdit.Location = New System.Drawing.Point(188, 134)
		Me.btnSamSpecEdit.Name = "btnSamSpecEdit"
		Me.btnSamSpecEdit.Size = New System.Drawing.Size(75, 28)
		Me.btnSamSpecEdit.TabIndex = 2
		Me.btnSamSpecEdit.Text = "Edit"
		Me.btnSamSpecEdit.UseVisualStyleBackColor = true
		'
		'btnDevSpecEdit
		'
		Me.btnDevSpecEdit.Font = New System.Drawing.Font("Meiryo UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.btnDevSpecEdit.Location = New System.Drawing.Point(188, 3)
		Me.btnDevSpecEdit.Name = "btnDevSpecEdit"
		Me.btnDevSpecEdit.Size = New System.Drawing.Size(75, 28)
		Me.btnDevSpecEdit.TabIndex = 5
		Me.btnDevSpecEdit.Text = "Edit"
		Me.btnDevSpecEdit.UseVisualStyleBackColor = true
		'
		'cbxSamSPEC
		'
		Me.cbxSamSPEC.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.cbxSamSPEC.FlatStyle = System.Windows.Forms.FlatStyle.Popup
		Me.cbxSamSPEC.Font = New System.Drawing.Font("Meiryo UI", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.cbxSamSPEC.FormattingEnabled = true
		Me.cbxSamSPEC.Location = New System.Drawing.Point(124, 134)
		Me.cbxSamSPEC.Name = "cbxSamSPEC"
		Me.cbxSamSPEC.Size = New System.Drawing.Size(57, 28)
		Me.cbxSamSPEC.TabIndex = 1
		'
		'dgvSamSpec
		'
		Me.dgvSamSpec.AllowUserToAddRows = false
		Me.dgvSamSpec.AllowUserToDeleteRows = false
		Me.dgvSamSpec.AllowUserToResizeRows = false
		Me.dgvSamSpec.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
		Me.dgvSamSpec.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
		Me.dgvSamSpec.ColumnHeadersVisible = false
		DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
		DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window
		DataGridViewCellStyle1.Font = New System.Drawing.Font("Meiryo UI", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText
		DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
		DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
		DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
		Me.dgvSamSpec.DefaultCellStyle = DataGridViewCellStyle1
		Me.dgvSamSpec.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnKeystroke
		Me.dgvSamSpec.Location = New System.Drawing.Point(2, 168)
		Me.dgvSamSpec.Name = "dgvSamSpec"
		Me.dgvSamSpec.RowHeadersVisible = false
		Me.dgvSamSpec.RowTemplate.Height = 30
		Me.dgvSamSpec.RowTemplate.ReadOnly = true
		Me.dgvSamSpec.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.dgvSamSpec.Size = New System.Drawing.Size(261, 57)
		Me.dgvSamSpec.TabIndex = 3
		'
		'lblSamSPECNameLabel
		'
		Me.lblSamSPECNameLabel.AutoSize = true
		Me.lblSamSPECNameLabel.Font = New System.Drawing.Font("Meiryo UI", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.lblSamSPECNameLabel.Location = New System.Drawing.Point(5, 137)
		Me.lblSamSPECNameLabel.Name = "lblSamSPECNameLabel"
		Me.lblSamSPECNameLabel.Size = New System.Drawing.Size(112, 20)
		Me.lblSamSPECNameLabel.TabIndex = 3
		Me.lblSamSPECNameLabel.Text = "Sample SPEC"
		'
		'dgvDevSpec
		'
		Me.dgvDevSpec.AllowUserToAddRows = false
		Me.dgvDevSpec.AllowUserToDeleteRows = false
		Me.dgvDevSpec.AllowUserToResizeRows = false
		Me.dgvDevSpec.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
		Me.dgvDevSpec.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
		Me.dgvDevSpec.ColumnHeadersVisible = false
		DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
		DataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window
		DataGridViewCellStyle2.Font = New System.Drawing.Font("Meiryo UI", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		DataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText
		DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight
		DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText
		DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
		Me.dgvDevSpec.DefaultCellStyle = DataGridViewCellStyle2
		Me.dgvDevSpec.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnKeystroke
		Me.dgvDevSpec.Location = New System.Drawing.Point(0, 39)
		Me.dgvDevSpec.Name = "dgvDevSpec"
		Me.dgvDevSpec.RowHeadersVisible = false
		Me.dgvDevSpec.RowTemplate.Height = 30
		Me.dgvDevSpec.RowTemplate.ReadOnly = true
		Me.dgvDevSpec.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.dgvDevSpec.Size = New System.Drawing.Size(263, 86)
		Me.dgvDevSpec.TabIndex = 6
		'
		'cbxDevSPEC
		'
		Me.cbxDevSPEC.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.cbxDevSPEC.FlatStyle = System.Windows.Forms.FlatStyle.Popup
		Me.cbxDevSPEC.Font = New System.Drawing.Font("Meiryo UI", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.cbxDevSPEC.FormattingEnabled = true
		Me.cbxDevSPEC.Location = New System.Drawing.Point(123, 5)
		Me.cbxDevSPEC.Name = "cbxDevSPEC"
		Me.cbxDevSPEC.Size = New System.Drawing.Size(58, 28)
		Me.cbxDevSPEC.TabIndex = 4
		'
		'lblDevSPECNameLabel
		'
		Me.lblDevSPECNameLabel.AutoSize = true
		Me.lblDevSPECNameLabel.Font = New System.Drawing.Font("Meiryo UI", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.lblDevSPECNameLabel.Location = New System.Drawing.Point(5, 9)
		Me.lblDevSPECNameLabel.Name = "lblDevSPECNameLabel"
		Me.lblDevSPECNameLabel.Size = New System.Drawing.Size(106, 20)
		Me.lblDevSPECNameLabel.TabIndex = 0
		Me.lblDevSPECNameLabel.Text = "Device SPEC"
		'
		'timerShowState
		'
		'
		'Panel1
		'
		Me.Panel1.BackColor = System.Drawing.SystemColors.GradientInactiveCaption
		Me.Panel1.Controls.Add(Me.rbZPosition)
		Me.Panel1.Controls.Add(Me.rbShearVelo)
		Me.Panel1.Controls.Add(Me.rbCorrectP)
		Me.Panel1.Controls.Add(Me.rbLossTangent)
		Me.Panel1.Controls.Add(Me.rbElasticMod)
		Me.Panel1.Controls.Add(Me.rbStorageMod)
		Me.Panel1.Controls.Add(Me.rbShearMod)
		Me.Panel1.Controls.Add(Me.rbSampleStrain)
		Me.Panel1.Controls.Add(Me.rbPosPhase)
		Me.Panel1.Controls.Add(Me.rbViscosity)
		Me.Panel1.Controls.Add(Me.rbZStress)
		Me.Panel1.Controls.Add(Me.rbPBMV)
		Me.Panel1.Controls.Add(Me.rbPBTemp)
		Me.Panel1.Controls.Add(Me.rbHPMV)
		Me.Panel1.Controls.Add(Me.rbHPTemp)
		Me.Panel1.Controls.Add(Me.rbCorrect)
		Me.Panel1.Controls.Add(Me.rbYStress)
		Me.Panel1.Controls.Add(Me.rbXStress)
		Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
		Me.Panel1.Location = New System.Drawing.Point(0, 0)
		Me.Panel1.Name = "Panel1"
		Me.Panel1.Size = New System.Drawing.Size(1368, 62)
		Me.Panel1.TabIndex = 51
		'
		'rbZPosition
		'
		Me.rbZPosition.AutoSize = true
		Me.rbZPosition.Font = New System.Drawing.Font("Meiryo UI", 12!)
		Me.rbZPosition.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.rbZPosition.Location = New System.Drawing.Point(353, 37)
		Me.rbZPosition.Name = "rbZPosition"
		Me.rbZPosition.Size = New System.Drawing.Size(104, 24)
		Me.rbZPosition.TabIndex = 64
		Me.rbZPosition.TabStop = true
		Me.rbZPosition.Text = "Z Position"
		Me.rbZPosition.UseVisualStyleBackColor = true
		'
		'rbShearVelo
		'
		Me.rbShearVelo.AutoSize = true
		Me.rbShearVelo.Font = New System.Drawing.Font("Meiryo UI", 12!)
		Me.rbShearVelo.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.rbShearVelo.Location = New System.Drawing.Point(757, 9)
		Me.rbShearVelo.Name = "rbShearVelo"
		Me.rbShearVelo.Size = New System.Drawing.Size(108, 24)
		Me.rbShearVelo.TabIndex = 63
		Me.rbShearVelo.TabStop = true
		Me.rbShearVelo.Text = "Shear rate"
		Me.rbShearVelo.UseVisualStyleBackColor = true
		'
		'rbCorrectP
		'
		Me.rbCorrectP.AutoSize = true
		Me.rbCorrectP.Font = New System.Drawing.Font("Meiryo UI", 12!)
		Me.rbCorrectP.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.rbCorrectP.Location = New System.Drawing.Point(107, 9)
		Me.rbCorrectP.Name = "rbCorrectP"
		Me.rbCorrectP.Size = New System.Drawing.Size(92, 24)
		Me.rbCorrectP.TabIndex = 62
		Me.rbCorrectP.TabStop = true
		Me.rbCorrectP.Text = "Stress%"
		Me.rbCorrectP.UseVisualStyleBackColor = true
		'
		'rbLossTangent
		'
		Me.rbLossTangent.AutoSize = true
		Me.rbLossTangent.Font = New System.Drawing.Font("Meiryo UI", 12!)
		Me.rbLossTangent.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.rbLossTangent.Location = New System.Drawing.Point(1164, 9)
		Me.rbLossTangent.Name = "rbLossTangent"
		Me.rbLossTangent.Size = New System.Drawing.Size(89, 24)
		Me.rbLossTangent.TabIndex = 61
		Me.rbLossTangent.TabStop = true
		Me.rbLossTangent.Text = "Tangent"
		Me.rbLossTangent.UseVisualStyleBackColor = true
		'
		'rbElasticMod
		'
		Me.rbElasticMod.AutoSize = true
		Me.rbElasticMod.Font = New System.Drawing.Font("Meiryo UI", 12!)
		Me.rbElasticMod.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.rbElasticMod.Location = New System.Drawing.Point(1028, 9)
		Me.rbElasticMod.Name = "rbElasticMod"
		Me.rbElasticMod.Size = New System.Drawing.Size(114, 24)
		Me.rbElasticMod.TabIndex = 61
		Me.rbElasticMod.TabStop = true
		Me.rbElasticMod.Text = "Elastic Mod"
		Me.rbElasticMod.UseVisualStyleBackColor = true
		'
		'rbStorageMod
		'
		Me.rbStorageMod.AutoSize = true
		Me.rbStorageMod.Font = New System.Drawing.Font("Meiryo UI", 12!)
		Me.rbStorageMod.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.rbStorageMod.Location = New System.Drawing.Point(889, 9)
		Me.rbStorageMod.Name = "rbStorageMod"
		Me.rbStorageMod.Size = New System.Drawing.Size(126, 24)
		Me.rbStorageMod.TabIndex = 61
		Me.rbStorageMod.TabStop = true
		Me.rbStorageMod.Text = "Storage Mod"
		Me.rbStorageMod.UseVisualStyleBackColor = true
		'
		'rbShearMod
		'
		Me.rbShearMod.AutoSize = true
		Me.rbShearMod.Font = New System.Drawing.Font("Meiryo UI", 12!)
		Me.rbShearMod.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.rbShearMod.Location = New System.Drawing.Point(617, 9)
		Me.rbShearMod.Name = "rbShearMod"
		Me.rbShearMod.Size = New System.Drawing.Size(110, 24)
		Me.rbShearMod.TabIndex = 61
		Me.rbShearMod.TabStop = true
		Me.rbShearMod.Text = "Shear Mod"
		Me.rbShearMod.UseVisualStyleBackColor = true
		'
		'rbSampleStrain
		'
		Me.rbSampleStrain.AutoSize = true
		Me.rbSampleStrain.Font = New System.Drawing.Font("Meiryo UI", 12!)
		Me.rbSampleStrain.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.rbSampleStrain.Location = New System.Drawing.Point(475, 9)
		Me.rbSampleStrain.Name = "rbSampleStrain"
		Me.rbSampleStrain.Size = New System.Drawing.Size(123, 24)
		Me.rbSampleStrain.TabIndex = 61
		Me.rbSampleStrain.TabStop = true
		Me.rbSampleStrain.Text = "Samp.Strain"
		Me.rbSampleStrain.UseVisualStyleBackColor = true
		'
		'rbPosPhase
		'
		Me.rbPosPhase.AutoSize = true
		Me.rbPosPhase.Font = New System.Drawing.Font("Meiryo UI", 12!)
		Me.rbPosPhase.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.rbPosPhase.Location = New System.Drawing.Point(353, 9)
		Me.rbPosPhase.Name = "rbPosPhase"
		Me.rbPosPhase.Size = New System.Drawing.Size(73, 24)
		Me.rbPosPhase.TabIndex = 61
		Me.rbPosPhase.TabStop = true
		Me.rbPosPhase.Text = "Phase"
		Me.rbPosPhase.UseVisualStyleBackColor = true
		'
		'rbViscosity
		'
		Me.rbViscosity.AutoSize = true
		Me.rbViscosity.Font = New System.Drawing.Font("Meiryo UI", 12!)
		Me.rbViscosity.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.rbViscosity.Location = New System.Drawing.Point(223, 9)
		Me.rbViscosity.Name = "rbViscosity"
		Me.rbViscosity.Size = New System.Drawing.Size(95, 24)
		Me.rbViscosity.TabIndex = 61
		Me.rbViscosity.TabStop = true
		Me.rbViscosity.Text = "Viscosity"
		Me.rbViscosity.UseVisualStyleBackColor = true
		'
		'rbZStress
		'
		Me.rbZStress.AutoSize = true
		Me.rbZStress.Font = New System.Drawing.Font("Meiryo UI", 12!)
		Me.rbZStress.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.rbZStress.Location = New System.Drawing.Point(223, 37)
		Me.rbZStress.Name = "rbZStress"
		Me.rbZStress.Size = New System.Drawing.Size(90, 24)
		Me.rbZStress.TabIndex = 59
		Me.rbZStress.TabStop = true
		Me.rbZStress.Text = "Z Stress"
		Me.rbZStress.UseVisualStyleBackColor = true
		'
		'rbPBMV
		'
		Me.rbPBMV.AutoSize = true
		Me.rbPBMV.Font = New System.Drawing.Font("Meiryo UI", 12!)
		Me.rbPBMV.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.rbPBMV.Location = New System.Drawing.Point(889, 35)
		Me.rbPBMV.Name = "rbPBMV"
		Me.rbPBMV.Size = New System.Drawing.Size(77, 24)
		Me.rbPBMV.TabIndex = 58
		Me.rbPBMV.TabStop = true
		Me.rbPBMV.Text = "PB MV"
		Me.rbPBMV.UseVisualStyleBackColor = true
		'
		'rbPBTemp
		'
		Me.rbPBTemp.AutoSize = true
		Me.rbPBTemp.Font = New System.Drawing.Font("Meiryo UI", 12!)
		Me.rbPBTemp.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.rbPBTemp.Location = New System.Drawing.Point(757, 35)
		Me.rbPBTemp.Name = "rbPBTemp"
		Me.rbPBTemp.Size = New System.Drawing.Size(95, 24)
		Me.rbPBTemp.TabIndex = 57
		Me.rbPBTemp.TabStop = true
		Me.rbPBTemp.Text = "PB Temp"
		Me.rbPBTemp.UseVisualStyleBackColor = true
		'
		'rbHPMV
		'
		Me.rbHPMV.AutoSize = true
		Me.rbHPMV.Font = New System.Drawing.Font("Meiryo UI", 12!)
		Me.rbHPMV.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.rbHPMV.Location = New System.Drawing.Point(617, 37)
		Me.rbHPMV.Name = "rbHPMV"
		Me.rbHPMV.Size = New System.Drawing.Size(78, 24)
		Me.rbHPMV.TabIndex = 3
		Me.rbHPMV.TabStop = true
		Me.rbHPMV.Text = "HP MV"
		Me.rbHPMV.UseVisualStyleBackColor = true
		'
		'rbHPTemp
		'
		Me.rbHPTemp.AutoSize = true
		Me.rbHPTemp.Font = New System.Drawing.Font("Meiryo UI", 12!)
		Me.rbHPTemp.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.rbHPTemp.Location = New System.Drawing.Point(475, 37)
		Me.rbHPTemp.Name = "rbHPTemp"
		Me.rbHPTemp.Size = New System.Drawing.Size(96, 24)
		Me.rbHPTemp.TabIndex = 2
		Me.rbHPTemp.TabStop = true
		Me.rbHPTemp.Text = "HP Temp"
		Me.rbHPTemp.UseVisualStyleBackColor = true
		'
		'rbCorrect
		'
		Me.rbCorrect.AutoSize = true
		Me.rbCorrect.Font = New System.Drawing.Font("Meiryo UI", 12!)
		Me.rbCorrect.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.rbCorrect.Location = New System.Drawing.Point(6, 9)
		Me.rbCorrect.Name = "rbCorrect"
		Me.rbCorrect.Size = New System.Drawing.Size(75, 24)
		Me.rbCorrect.TabIndex = 1
		Me.rbCorrect.TabStop = true
		Me.rbCorrect.Text = "Stress"
		Me.rbCorrect.UseVisualStyleBackColor = true
		'
		'rbYStress
		'
		Me.rbYStress.AutoSize = true
		Me.rbYStress.Font = New System.Drawing.Font("Meiryo UI", 12!)
		Me.rbYStress.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.rbYStress.Location = New System.Drawing.Point(107, 37)
		Me.rbYStress.Name = "rbYStress"
		Me.rbYStress.Size = New System.Drawing.Size(90, 24)
		Me.rbYStress.TabIndex = 0
		Me.rbYStress.TabStop = true
		Me.rbYStress.Text = "Y Stress"
		Me.rbYStress.UseVisualStyleBackColor = true
		'
		'rbXStress
		'
		Me.rbXStress.AutoSize = true
		Me.rbXStress.Font = New System.Drawing.Font("Meiryo UI", 12!)
		Me.rbXStress.Location = New System.Drawing.Point(6, 37)
		Me.rbXStress.Name = "rbXStress"
		Me.rbXStress.Size = New System.Drawing.Size(91, 24)
		Me.rbXStress.TabIndex = 0
		Me.rbXStress.TabStop = true
		Me.rbXStress.Text = "X Stress"
		Me.rbXStress.UseVisualStyleBackColor = true
		'
		'lblYRangeOverLamp
		'
		Me.lblYRangeOverLamp.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left),System.Windows.Forms.AnchorStyles)
		Me.lblYRangeOverLamp.BackColor = System.Drawing.SystemColors.ButtonHighlight
		Me.lblYRangeOverLamp.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		Me.lblYRangeOverLamp.Font = New System.Drawing.Font("Meiryo UI", 8.25!)
		Me.lblYRangeOverLamp.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.lblYRangeOverLamp.Location = New System.Drawing.Point(1067, 885)
		Me.lblYRangeOverLamp.Name = "lblYRangeOverLamp"
		Me.lblYRangeOverLamp.Size = New System.Drawing.Size(128, 18)
		Me.lblYRangeOverLamp.TabIndex = 56
		Me.lblYRangeOverLamp.Text = "Y Stress range over"
		Me.lblYRangeOverLamp.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.lblYRangeOverLamp.Visible = false
		'
		'lblXRangeOverLamp
		'
		Me.lblXRangeOverLamp.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left),System.Windows.Forms.AnchorStyles)
		Me.lblXRangeOverLamp.BackColor = System.Drawing.SystemColors.ButtonHighlight
		Me.lblXRangeOverLamp.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		Me.lblXRangeOverLamp.Font = New System.Drawing.Font("Meiryo UI", 8.25!)
		Me.lblXRangeOverLamp.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.lblXRangeOverLamp.Location = New System.Drawing.Point(1067, 868)
		Me.lblXRangeOverLamp.Name = "lblXRangeOverLamp"
		Me.lblXRangeOverLamp.Size = New System.Drawing.Size(128, 17)
		Me.lblXRangeOverLamp.TabIndex = 56
		Me.lblXRangeOverLamp.Text = "X Stress range over"
		Me.lblXRangeOverLamp.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.lblXRangeOverLamp.Visible = false
		'
		'btnAllClear
		'
		Me.btnAllClear.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
		Me.btnAllClear.Font = New System.Drawing.Font("Meiryo UI", 14.25!)
		Me.btnAllClear.Location = New System.Drawing.Point(1059, 969)
		Me.btnAllClear.Name = "btnAllClear"
		Me.btnAllClear.Size = New System.Drawing.Size(153, 36)
		Me.btnAllClear.TabIndex = 80
		Me.btnAllClear.Text = "Clear"
		Me.btnAllClear.UseVisualStyleBackColor = true
		'
		'lblElapsedVal
		'
		Me.lblElapsedVal.BackColor = System.Drawing.Color.White
		Me.lblElapsedVal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		Me.lblElapsedVal.Font = New System.Drawing.Font("MS UI Gothic", 24!)
		Me.lblElapsedVal.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.lblElapsedVal.Location = New System.Drawing.Point(10, 19)
		Me.lblElapsedVal.Name = "lblElapsedVal"
		Me.lblElapsedVal.Size = New System.Drawing.Size(145, 42)
		Me.lblElapsedVal.TabIndex = 52
		Me.lblElapsedVal.Text = "108.25"
		Me.lblElapsedVal.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		'
		'pnlRealG
		'
		Me.pnlRealG.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom)  _
            Or System.Windows.Forms.AnchorStyles.Left)  _
            Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
		Me.pnlRealG.BackColor = System.Drawing.SystemColors.ButtonHighlight
		Me.pnlRealG.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.pnlRealG.Controls.Add(Me.btnSPC)
		Me.pnlRealG.Controls.Add(Me.lblRangeWarning)
		Me.pnlRealG.Controls.Add(Me.lblGuideXYbeforeGap)
		Me.pnlRealG.Controls.Add(Me.lblFailedOrderP)
		Me.pnlRealG.Controls.Add(Me.pnlThinning)
		Me.pnlRealG.Controls.Add(Me.Panel2)
		Me.pnlRealG.Controls.Add(Me.lblCalclating)
		Me.pnlRealG.Controls.Add(Me.pnlSteress)
		Me.pnlRealG.Controls.Add(Me.lblYAuto)
		Me.pnlRealG.Controls.Add(Me.lblXAuto)
		Me.pnlRealG.Controls.Add(Me.Button1)
		Me.pnlRealG.Controls.Add(Me.lblLongTimeMode)
		Me.pnlRealG.Controls.Add(Me.gbxStdCond)
		Me.pnlRealG.Controls.Add(Me.Label1)
		Me.pnlRealG.Controls.Add(Me.nudYAxisZoom)
		Me.pnlRealG.Controls.Add(Me.lblZRangeOverLamp)
		Me.pnlRealG.Controls.Add(Me.pnlCalculation)
		Me.pnlRealG.Controls.Add(Me.btnStdInfo)
		Me.pnlRealG.Controls.Add(Me.btnThinning)
		Me.pnlRealG.Controls.Add(Me.lblChartCoordinate)
		Me.pnlRealG.Controls.Add(Me.lblYRangeOverLamp)
		Me.pnlRealG.Controls.Add(Me.lblXRangeOverLamp)
		Me.pnlRealG.Controls.Add(Me.gbxCurrentVal)
		Me.pnlRealG.Controls.Add(Me.Panel1)
		Me.pnlRealG.Controls.Add(Me.lblResult1Val)
		Me.pnlRealG.Controls.Add(Me.lblResult1)
		Me.pnlRealG.Controls.Add(Me.lblResult3Val)
		Me.pnlRealG.Controls.Add(Me.lblResult2Val)
		Me.pnlRealG.Controls.Add(Me.lblResult3)
		Me.pnlRealG.Controls.Add(Me.lblResult2)
		Me.pnlRealG.Controls.Add(Me.Chart1)
		Me.pnlRealG.Location = New System.Drawing.Point(4, 34)
		Me.pnlRealG.Name = "pnlRealG"
		Me.pnlRealG.Size = New System.Drawing.Size(1372, 925)
		Me.pnlRealG.TabIndex = 53
		'
		'btnSPC
		'
		Me.btnSPC.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left),System.Windows.Forms.AnchorStyles)
		Me.btnSPC.Font = New System.Drawing.Font("Meiryo UI", 12!)
		Me.btnSPC.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.btnSPC.Location = New System.Drawing.Point(352, 888)
		Me.btnSPC.Name = "btnSPC"
		Me.btnSPC.Size = New System.Drawing.Size(170, 30)
		Me.btnSPC.TabIndex = 90
		Me.btnSPC.Text = "SPC"
		Me.btnSPC.UseVisualStyleBackColor = true
		'
		'lblRangeWarning
		'
		Me.lblRangeWarning.AutoSize = true
		Me.lblRangeWarning.BackColor = System.Drawing.SystemColors.Info
		Me.lblRangeWarning.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		Me.lblRangeWarning.Font = New System.Drawing.Font("Meiryo UI", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
		Me.lblRangeWarning.Location = New System.Drawing.Point(255, 842)
		Me.lblRangeWarning.Name = "lblRangeWarning"
		Me.lblRangeWarning.Size = New System.Drawing.Size(198, 22)
		Me.lblRangeWarning.TabIndex = 89
		Me.lblRangeWarning.Text = "グラフ表示範囲の警告を表示"
		Me.lblRangeWarning.Visible = false
		'
		'lblGuideXYbeforeGap
		'
		Me.lblGuideXYbeforeGap.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom)  _
            Or System.Windows.Forms.AnchorStyles.Left)  _
            Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
		Me.lblGuideXYbeforeGap.AutoSize = true
		Me.lblGuideXYbeforeGap.BackColor = System.Drawing.Color.Transparent
		Me.lblGuideXYbeforeGap.Font = New System.Drawing.Font("Meiryo UI", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.lblGuideXYbeforeGap.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me.lblGuideXYbeforeGap.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.lblGuideXYbeforeGap.Location = New System.Drawing.Point(172, 88)
		Me.lblGuideXYbeforeGap.Name = "lblGuideXYbeforeGap"
		Me.lblGuideXYbeforeGap.Size = New System.Drawing.Size(542, 20)
		Me.lblGuideXYbeforeGap.TabIndex = 88
		Me.lblGuideXYbeforeGap.Text = "XY movement started before the measurement GAP was determined"
		Me.lblGuideXYbeforeGap.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me.lblGuideXYbeforeGap.Visible = false
		'
		'lblFailedOrderP
		'
		Me.lblFailedOrderP.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom)  _
            Or System.Windows.Forms.AnchorStyles.Left)  _
            Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
		Me.lblFailedOrderP.AutoSize = true
		Me.lblFailedOrderP.BackColor = System.Drawing.Color.Yellow
		Me.lblFailedOrderP.Font = New System.Drawing.Font("Meiryo UI", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.lblFailedOrderP.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me.lblFailedOrderP.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.lblFailedOrderP.Location = New System.Drawing.Point(172, 67)
		Me.lblFailedOrderP.Name = "lblFailedOrderP"
		Me.lblFailedOrderP.Size = New System.Drawing.Size(544, 20)
		Me.lblFailedOrderP.TabIndex = 87
		Me.lblFailedOrderP.Text = "Failure in pressure control means it is equivalent to GAP specification"
		Me.lblFailedOrderP.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me.lblFailedOrderP.Visible = false
		'
		'pnlThinning
		'
		Me.pnlThinning.BackColor = System.Drawing.SystemColors.Info
		Me.pnlThinning.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		Me.pnlThinning.Controls.Add(Me.lblExpand)
		Me.pnlThinning.Controls.Add(Me.gpxScrol)
		Me.pnlThinning.Controls.Add(Me.gpxAxisSetting)
		Me.pnlThinning.Controls.Add(Me.gpxThinning)
		Me.pnlThinning.Controls.Add(Me.gpxXType)
		Me.pnlThinning.Location = New System.Drawing.Point(247, 163)
		Me.pnlThinning.Name = "pnlThinning"
		Me.pnlThinning.Size = New System.Drawing.Size(306, 471)
		Me.pnlThinning.TabIndex = 66
		Me.pnlThinning.Visible = false
		'
		'lblExpand
		'
		Me.lblExpand.AutoSize = true
		Me.lblExpand.Location = New System.Drawing.Point(279, 241)
		Me.lblExpand.Name = "lblExpand"
		Me.lblExpand.Size = New System.Drawing.Size(17, 12)
		Me.lblExpand.TabIndex = 101
		Me.lblExpand.Text = "▽"
		'
		'gpxScrol
		'
		Me.gpxScrol.Controls.Add(Me.lblScrolWidth)
		Me.gpxScrol.Controls.Add(Me.nudXSpan)
		Me.gpxScrol.Font = New System.Drawing.Font("Meiryo UI", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.gpxScrol.Location = New System.Drawing.Point(7, 396)
		Me.gpxScrol.Name = "gpxScrol"
		Me.gpxScrol.Size = New System.Drawing.Size(289, 63)
		Me.gpxScrol.TabIndex = 100
		Me.gpxScrol.TabStop = false
		Me.gpxScrol.Text = "Scroll setting"
		'
		'lblScrolWidth
		'
		Me.lblScrolWidth.AutoSize = true
		Me.lblScrolWidth.Font = New System.Drawing.Font("Meiryo UI", 12!)
		Me.lblScrolWidth.Location = New System.Drawing.Point(5, 28)
		Me.lblScrolWidth.Name = "lblScrolWidth"
		Me.lblScrolWidth.Size = New System.Drawing.Size(144, 20)
		Me.lblScrolWidth.TabIndex = 62
		Me.lblScrolWidth.Text = "Scroll Width(Sec)"
		'
		'nudXSpan
		'
		Me.nudXSpan.Font = New System.Drawing.Font("Meiryo UI", 12!)
		Me.nudXSpan.Location = New System.Drawing.Point(159, 25)
		Me.nudXSpan.Maximum = New Decimal(New Integer() {1000, 0, 0, 0})
		Me.nudXSpan.Minimum = New Decimal(New Integer() {10, 0, 0, 0})
		Me.nudXSpan.Name = "nudXSpan"
		Me.nudXSpan.Size = New System.Drawing.Size(113, 28)
		Me.nudXSpan.TabIndex = 61
		Me.nudXSpan.Value = New Decimal(New Integer() {10, 0, 0, 0})
		'
		'gpxAxisSetting
		'
		Me.gpxAxisSetting.Controls.Add(Me.gbxXscale)
		Me.gpxAxisSetting.Controls.Add(Me.ckbXFixed)
		Me.gpxAxisSetting.Controls.Add(Me.gbxYscale)
		Me.gpxAxisSetting.Controls.Add(Me.ckbYFixed)
		Me.gpxAxisSetting.Font = New System.Drawing.Font("Meiryo UI", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.gpxAxisSetting.Location = New System.Drawing.Point(7, 5)
		Me.gpxAxisSetting.Name = "gpxAxisSetting"
		Me.gpxAxisSetting.Size = New System.Drawing.Size(289, 233)
		Me.gpxAxisSetting.TabIndex = 99
		Me.gpxAxisSetting.TabStop = false
		Me.gpxAxisSetting.Text = "Axis setting"
		'
		'gbxXscale
		'
		Me.gbxXscale.Controls.Add(Me.nudXscaleMax)
		Me.gbxXscale.Controls.Add(Me.nudXscaleMin)
		Me.gbxXscale.Controls.Add(Me.Label2)
		Me.gbxXscale.Controls.Add(Me.btnXSpanAply)
		Me.gbxXscale.Controls.Add(Me.nudXdivision)
		Me.gbxXscale.Location = New System.Drawing.Point(16, 44)
		Me.gbxXscale.Name = "gbxXscale"
		Me.gbxXscale.Size = New System.Drawing.Size(265, 80)
		Me.gbxXscale.TabIndex = 99
		Me.gbxXscale.TabStop = false
		'
		'Label2
		'
		Me.Label2.AutoSize = true
		Me.Label2.Location = New System.Drawing.Point(127, 16)
		Me.Label2.Name = "Label2"
		Me.Label2.Size = New System.Drawing.Size(16, 20)
		Me.Label2.TabIndex = 3
		Me.Label2.Text = "-"
		'
		'btnXSpanAply
		'
		Me.btnXSpanAply.Font = New System.Drawing.Font("Meiryo UI", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.btnXSpanAply.Location = New System.Drawing.Point(143, 47)
		Me.btnXSpanAply.Name = "btnXSpanAply"
		Me.btnXSpanAply.Size = New System.Drawing.Size(114, 29)
		Me.btnXSpanAply.TabIndex = 70
		Me.btnXSpanAply.Text = "Apply"
		Me.btnXSpanAply.UseVisualStyleBackColor = true
		'
		'ckbXFixed
		'
		Me.ckbXFixed.AutoSize = true
		Me.ckbXFixed.Font = New System.Drawing.Font("Meiryo UI", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.ckbXFixed.Location = New System.Drawing.Point(16, 23)
		Me.ckbXFixed.Name = "ckbXFixed"
		Me.ckbXFixed.Size = New System.Drawing.Size(128, 24)
		Me.ckbXFixed.TabIndex = 66
		Me.ckbXFixed.Text = "X Fixed scale"
		Me.ckbXFixed.UseVisualStyleBackColor = true
		'
		'gbxYscale
		'
		Me.gbxYscale.Controls.Add(Me.nudYscaleMax)
		Me.gbxYscale.Controls.Add(Me.nudYscaleMin)
		Me.gbxYscale.Controls.Add(Me.Label3)
		Me.gbxYscale.Controls.Add(Me.nudYdivision)
		Me.gbxYscale.Controls.Add(Me.btnYSpanAply)
		Me.gbxYscale.Location = New System.Drawing.Point(16, 146)
		Me.gbxYscale.Name = "gbxYscale"
		Me.gbxYscale.Size = New System.Drawing.Size(265, 78)
		Me.gbxYscale.TabIndex = 99
		Me.gbxYscale.TabStop = false
		'
		'Label3
		'
		Me.Label3.AutoSize = true
		Me.Label3.Location = New System.Drawing.Point(127, 18)
		Me.Label3.Name = "Label3"
		Me.Label3.Size = New System.Drawing.Size(16, 20)
		Me.Label3.TabIndex = 5
		Me.Label3.Text = "-"
		'
		'btnYSpanAply
		'
		Me.btnYSpanAply.Font = New System.Drawing.Font("Meiryo UI", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.btnYSpanAply.Location = New System.Drawing.Point(143, 45)
		Me.btnYSpanAply.Name = "btnYSpanAply"
		Me.btnYSpanAply.Size = New System.Drawing.Size(114, 29)
		Me.btnYSpanAply.TabIndex = 75
		Me.btnYSpanAply.Text = "Apply"
		Me.btnYSpanAply.UseVisualStyleBackColor = true
		'
		'ckbYFixed
		'
		Me.ckbYFixed.AutoSize = true
		Me.ckbYFixed.Font = New System.Drawing.Font("Meiryo UI", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.ckbYFixed.Location = New System.Drawing.Point(16, 126)
		Me.ckbYFixed.Name = "ckbYFixed"
		Me.ckbYFixed.Size = New System.Drawing.Size(127, 24)
		Me.ckbYFixed.TabIndex = 71
		Me.ckbYFixed.Text = "Y Fixed scale"
		Me.ckbYFixed.UseVisualStyleBackColor = true
		'
		'gpxThinning
		'
		Me.gpxThinning.Controls.Add(Me.nudDispThinningoutRough)
		Me.gpxThinning.Controls.Add(Me.nudDispThinningoutDetail)
		Me.gpxThinning.Controls.Add(Me.lblDispThinningRough)
		Me.gpxThinning.Controls.Add(Me.lblDispThinningDetail)
		Me.gpxThinning.Font = New System.Drawing.Font("Meiryo UI", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.gpxThinning.Location = New System.Drawing.Point(7, 303)
		Me.gpxThinning.Name = "gpxThinning"
		Me.gpxThinning.Size = New System.Drawing.Size(289, 90)
		Me.gpxThinning.TabIndex = 78
		Me.gpxThinning.TabStop = false
		Me.gpxThinning.Text = "Display thinning"
		'
		'nudDispThinningoutRough
		'
		Me.nudDispThinningoutRough.Font = New System.Drawing.Font("Meiryo UI", 12!)
		Me.nudDispThinningoutRough.Increment = New Decimal(New Integer() {10, 0, 0, 0})
		Me.nudDispThinningoutRough.Location = New System.Drawing.Point(159, 21)
		Me.nudDispThinningoutRough.Maximum = New Decimal(New Integer() {1000000, 0, 0, 0})
		Me.nudDispThinningoutRough.Minimum = New Decimal(New Integer() {100, 0, 0, 0})
		Me.nudDispThinningoutRough.Name = "nudDispThinningoutRough"
		Me.nudDispThinningoutRough.Size = New System.Drawing.Size(114, 28)
		Me.nudDispThinningoutRough.TabIndex = 78
		Me.nudDispThinningoutRough.Value = New Decimal(New Integer() {200, 0, 0, 0})
		'
		'nudDispThinningoutDetail
		'
		Me.nudDispThinningoutDetail.Font = New System.Drawing.Font("Meiryo UI", 12!)
		Me.nudDispThinningoutDetail.Increment = New Decimal(New Integer() {10, 0, 0, 0})
		Me.nudDispThinningoutDetail.Location = New System.Drawing.Point(159, 55)
		Me.nudDispThinningoutDetail.Maximum = New Decimal(New Integer() {1000000, 0, 0, 0})
		Me.nudDispThinningoutDetail.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
		Me.nudDispThinningoutDetail.Name = "nudDispThinningoutDetail"
		Me.nudDispThinningoutDetail.Size = New System.Drawing.Size(114, 28)
		Me.nudDispThinningoutDetail.TabIndex = 79
		Me.nudDispThinningoutDetail.Value = New Decimal(New Integer() {10, 0, 0, 0})
		'
		'lblDispThinningRough
		'
		Me.lblDispThinningRough.AutoSize = true
		Me.lblDispThinningRough.Font = New System.Drawing.Font("Meiryo UI", 12!)
		Me.lblDispThinningRough.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.lblDispThinningRough.Location = New System.Drawing.Point(26, 23)
		Me.lblDispThinningRough.Name = "lblDispThinningRough"
		Me.lblDispThinningRough.Size = New System.Drawing.Size(60, 20)
		Me.lblDispThinningRough.TabIndex = 62
		Me.lblDispThinningRough.Text = "Rough"
		'
		'lblDispThinningDetail
		'
		Me.lblDispThinningDetail.AutoSize = true
		Me.lblDispThinningDetail.Font = New System.Drawing.Font("Meiryo UI", 12!)
		Me.lblDispThinningDetail.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.lblDispThinningDetail.Location = New System.Drawing.Point(26, 57)
		Me.lblDispThinningDetail.Name = "lblDispThinningDetail"
		Me.lblDispThinningDetail.Size = New System.Drawing.Size(53, 20)
		Me.lblDispThinningDetail.TabIndex = 62
		Me.lblDispThinningDetail.Text = "Detail"
		'
		'gpxXType
		'
		Me.gpxXType.Controls.Add(Me.rbXTime)
		Me.gpxXType.Controls.Add(Me.rbXTemp)
		Me.gpxXType.Font = New System.Drawing.Font("Meiryo UI", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.gpxXType.Location = New System.Drawing.Point(7, 255)
		Me.gpxXType.Name = "gpxXType"
		Me.gpxXType.Size = New System.Drawing.Size(289, 47)
		Me.gpxXType.TabIndex = 76
		Me.gpxXType.TabStop = false
		Me.gpxXType.Text = "X axis Type"
		'
		'rbXTime
		'
		Me.rbXTime.AutoSize = true
		Me.rbXTime.Location = New System.Drawing.Point(6, 20)
		Me.rbXTime.Name = "rbXTime"
		Me.rbXTime.Size = New System.Drawing.Size(65, 24)
		Me.rbXTime.TabIndex = 76
		Me.rbXTime.TabStop = true
		Me.rbXTime.Text = "Time"
		Me.rbXTime.UseVisualStyleBackColor = true
		'
		'rbXTemp
		'
		Me.rbXTemp.AutoSize = true
		Me.rbXTemp.Location = New System.Drawing.Point(113, 20)
		Me.rbXTemp.Name = "rbXTemp"
		Me.rbXTemp.Size = New System.Drawing.Size(126, 24)
		Me.rbXTemp.TabIndex = 77
		Me.rbXTemp.TabStop = true
		Me.rbXTemp.Text = "Temperature"
		Me.rbXTemp.UseVisualStyleBackColor = true
		'
		'Panel2
		'
		Me.Panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		Me.Panel2.Controls.Add(Me.Chart2)
		Me.Panel2.Location = New System.Drawing.Point(12, 175)
		Me.Panel2.Name = "Panel2"
		Me.Panel2.Size = New System.Drawing.Size(230, 76)
		Me.Panel2.TabIndex = 59
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
		'lblCalclating
		'
		Me.lblCalclating.AutoSize = true
		Me.lblCalclating.BackColor = System.Drawing.Color.Transparent
		Me.lblCalclating.Font = New System.Drawing.Font("Meiryo UI", 24!)
		Me.lblCalclating.Location = New System.Drawing.Point(522, 452)
		Me.lblCalclating.Name = "lblCalclating"
		Me.lblCalclating.Size = New System.Drawing.Size(227, 41)
		Me.lblCalclating.TabIndex = 72
		Me.lblCalclating.Text = "calculation ..."
		Me.lblCalclating.Visible = false
		'
		'pnlSteress
		'
		Me.pnlSteress.BackColor = System.Drawing.SystemColors.GradientInactiveCaption
		Me.pnlSteress.Controls.Add(Me.lblNoLoadStrVal)
		Me.pnlSteress.Controls.Add(Me.lblZStrsCRVal)
		Me.pnlSteress.Controls.Add(Me.lblNoLoadStrLabel)
		Me.pnlSteress.Controls.Add(Me.lblCurrXStrs)
		Me.pnlSteress.Controls.Add(Me.lblZStrsCR)
		Me.pnlSteress.Controls.Add(Me.lblCurrYStrs)
		Me.pnlSteress.Controls.Add(Me.lblCurrZStrsVal)
		Me.pnlSteress.Controls.Add(Me.lblCurrYStrsVal)
		Me.pnlSteress.Controls.Add(Me.lblCurrXStrsVal)
		Me.pnlSteress.Controls.Add(Me.lblCurrZStrs)
		Me.pnlSteress.Location = New System.Drawing.Point(876, 545)
		Me.pnlSteress.Name = "pnlSteress"
		Me.pnlSteress.Size = New System.Drawing.Size(155, 370)
		Me.pnlSteress.TabIndex = 86
		Me.pnlSteress.Visible = false
		'
		'lblNoLoadStrVal
		'
		Me.lblNoLoadStrVal.BackColor = System.Drawing.Color.White
		Me.lblNoLoadStrVal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		Me.lblNoLoadStrVal.Font = New System.Drawing.Font("MS UI Gothic", 21.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.lblNoLoadStrVal.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.lblNoLoadStrVal.Location = New System.Drawing.Point(3, 231)
		Me.lblNoLoadStrVal.Name = "lblNoLoadStrVal"
		Me.lblNoLoadStrVal.Size = New System.Drawing.Size(145, 30)
		Me.lblNoLoadStrVal.TabIndex = 67
		Me.lblNoLoadStrVal.Text = "-0000000"
		Me.lblNoLoadStrVal.TextAlign = System.Drawing.ContentAlignment.MiddleRight
		'
		'lblZStrsCRVal
		'
		Me.lblZStrsCRVal.BackColor = System.Drawing.Color.White
		Me.lblZStrsCRVal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		Me.lblZStrsCRVal.Font = New System.Drawing.Font("MS UI Gothic", 21.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.lblZStrsCRVal.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.lblZStrsCRVal.Location = New System.Drawing.Point(3, 180)
		Me.lblZStrsCRVal.Name = "lblZStrsCRVal"
		Me.lblZStrsCRVal.Size = New System.Drawing.Size(145, 30)
		Me.lblZStrsCRVal.TabIndex = 63
		Me.lblZStrsCRVal.Text = "-0000000"
		Me.lblZStrsCRVal.TextAlign = System.Drawing.ContentAlignment.MiddleRight
		'
		'lblNoLoadStrLabel
		'
		Me.lblNoLoadStrLabel.AutoSize = true
		Me.lblNoLoadStrLabel.Font = New System.Drawing.Font("Meiryo UI", 12!)
		Me.lblNoLoadStrLabel.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.lblNoLoadStrLabel.Location = New System.Drawing.Point(3, 209)
		Me.lblNoLoadStrLabel.Name = "lblNoLoadStrLabel"
		Me.lblNoLoadStrLabel.Size = New System.Drawing.Size(129, 20)
		Me.lblNoLoadStrLabel.TabIndex = 66
		Me.lblNoLoadStrLabel.Text = "Z No load (Dig)"
		'
		'lblCurrXStrs
		'
		Me.lblCurrXStrs.AutoSize = true
		Me.lblCurrXStrs.Font = New System.Drawing.Font("Meiryo UI", 12!)
		Me.lblCurrXStrs.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.lblCurrXStrs.Location = New System.Drawing.Point(3, 2)
		Me.lblCurrXStrs.Name = "lblCurrXStrs"
		Me.lblCurrXStrs.Size = New System.Drawing.Size(118, 20)
		Me.lblCurrXStrs.TabIndex = 55
		Me.lblCurrXStrs.Text = "X Stress (Dig)"
		'
		'lblZStrsCR
		'
		Me.lblZStrsCR.AutoSize = true
		Me.lblZStrsCR.Font = New System.Drawing.Font("Meiryo UI", 12!)
		Me.lblZStrsCR.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.lblZStrsCR.Location = New System.Drawing.Point(3, 160)
		Me.lblZStrsCR.Name = "lblZStrsCR"
		Me.lblZStrsCR.Size = New System.Drawing.Size(140, 20)
		Me.lblZStrsCR.TabIndex = 62
		Me.lblZStrsCR.Text = "Z Str.CR (g/10S)"
		'
		'lblCurrYStrs
		'
		Me.lblCurrYStrs.AutoSize = true
		Me.lblCurrYStrs.Font = New System.Drawing.Font("Meiryo UI", 12!)
		Me.lblCurrYStrs.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.lblCurrYStrs.Location = New System.Drawing.Point(3, 56)
		Me.lblCurrYStrs.Name = "lblCurrYStrs"
		Me.lblCurrYStrs.Size = New System.Drawing.Size(117, 20)
		Me.lblCurrYStrs.TabIndex = 55
		Me.lblCurrYStrs.Text = "Y Stress (Dig)"
		'
		'lblCurrZStrsVal
		'
		Me.lblCurrZStrsVal.BackColor = System.Drawing.Color.White
		Me.lblCurrZStrsVal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		Me.lblCurrZStrsVal.Font = New System.Drawing.Font("MS UI Gothic", 21.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.lblCurrZStrsVal.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.lblCurrZStrsVal.Location = New System.Drawing.Point(3, 129)
		Me.lblCurrZStrsVal.Name = "lblCurrZStrsVal"
		Me.lblCurrZStrsVal.Size = New System.Drawing.Size(145, 30)
		Me.lblCurrZStrsVal.TabIndex = 61
		Me.lblCurrZStrsVal.Text = "-0000000"
		Me.lblCurrZStrsVal.TextAlign = System.Drawing.ContentAlignment.MiddleRight
		'
		'lblCurrYStrsVal
		'
		Me.lblCurrYStrsVal.BackColor = System.Drawing.Color.White
		Me.lblCurrYStrsVal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		Me.lblCurrYStrsVal.Font = New System.Drawing.Font("MS UI Gothic", 21.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.lblCurrYStrsVal.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.lblCurrYStrsVal.Location = New System.Drawing.Point(3, 76)
		Me.lblCurrYStrsVal.Name = "lblCurrYStrsVal"
		Me.lblCurrYStrsVal.Size = New System.Drawing.Size(145, 30)
		Me.lblCurrYStrsVal.TabIndex = 52
		Me.lblCurrYStrsVal.Text = "-0000000"
		Me.lblCurrYStrsVal.TextAlign = System.Drawing.ContentAlignment.MiddleRight
		'
		'lblCurrXStrsVal
		'
		Me.lblCurrXStrsVal.BackColor = System.Drawing.Color.White
		Me.lblCurrXStrsVal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		Me.lblCurrXStrsVal.Font = New System.Drawing.Font("MS UI Gothic", 21.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.lblCurrXStrsVal.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.lblCurrXStrsVal.Location = New System.Drawing.Point(3, 23)
		Me.lblCurrXStrsVal.Name = "lblCurrXStrsVal"
		Me.lblCurrXStrsVal.Size = New System.Drawing.Size(145, 30)
		Me.lblCurrXStrsVal.TabIndex = 52
		Me.lblCurrXStrsVal.Text = "-0000000"
		Me.lblCurrXStrsVal.TextAlign = System.Drawing.ContentAlignment.MiddleRight
		'
		'lblCurrZStrs
		'
		Me.lblCurrZStrs.AutoSize = true
		Me.lblCurrZStrs.Font = New System.Drawing.Font("Meiryo UI", 12!)
		Me.lblCurrZStrs.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.lblCurrZStrs.Location = New System.Drawing.Point(3, 109)
		Me.lblCurrZStrs.Name = "lblCurrZStrs"
		Me.lblCurrZStrs.Size = New System.Drawing.Size(117, 20)
		Me.lblCurrZStrs.TabIndex = 60
		Me.lblCurrZStrs.Text = "Z Stress (Dig)"
		'
		'lblYAuto
		'
		Me.lblYAuto.AutoSize = true
		Me.lblYAuto.Font = New System.Drawing.Font("Meiryo UI", 12!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.lblYAuto.Location = New System.Drawing.Point(14, 82)
		Me.lblYAuto.Name = "lblYAuto"
		Me.lblYAuto.Size = New System.Drawing.Size(64, 20)
		Me.lblYAuto.TabIndex = 85
		Me.lblYAuto.Text = "Y:Auto"
		'
		'lblXAuto
		'
		Me.lblXAuto.AutoSize = true
		Me.lblXAuto.Font = New System.Drawing.Font("Meiryo UI", 12!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.lblXAuto.Location = New System.Drawing.Point(1125, 888)
		Me.lblXAuto.Name = "lblXAuto"
		Me.lblXAuto.Size = New System.Drawing.Size(67, 20)
		Me.lblXAuto.TabIndex = 84
		Me.lblXAuto.Text = "X:Auto"
		'
		'Button1
		'
		Me.Button1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
		Me.Button1.Location = New System.Drawing.Point(601, 895)
		Me.Button1.Name = "Button1"
		Me.Button1.Size = New System.Drawing.Size(126, 23)
		Me.Button1.TabIndex = 83
		Me.Button1.Text = "Register DB"
		Me.Button1.UseVisualStyleBackColor = true
		Me.Button1.Visible = false
		'
		'lblLongTimeMode
		'
		Me.lblLongTimeMode.BackColor = System.Drawing.SystemColors.Info
		Me.lblLongTimeMode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		Me.lblLongTimeMode.Font = New System.Drawing.Font("Meiryo UI", 24!)
		Me.lblLongTimeMode.Location = New System.Drawing.Point(281, 148)
		Me.lblLongTimeMode.Name = "lblLongTimeMode"
		Me.lblLongTimeMode.Size = New System.Drawing.Size(663, 128)
		Me.lblLongTimeMode.TabIndex = 72
		Me.lblLongTimeMode.Text = "The graph function has been stopped  For long-term mode."
		Me.lblLongTimeMode.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.lblLongTimeMode.Visible = false
		'
		'gbxStdCond
		'
		Me.gbxStdCond.Controls.Add(Me.btnStdCondClose)
		Me.gbxStdCond.Controls.Add(Me.rbReal)
		Me.gbxStdCond.Controls.Add(Me.rbStd1)
		Me.gbxStdCond.Controls.Add(Me.rbStd2)
		Me.gbxStdCond.Controls.Add(Me.dgvStd)
		Me.gbxStdCond.Location = New System.Drawing.Point(6, 319)
		Me.gbxStdCond.Name = "gbxStdCond"
		Me.gbxStdCond.Size = New System.Drawing.Size(340, 379)
		Me.gbxStdCond.TabIndex = 71
		Me.gbxStdCond.TabStop = false
		Me.gbxStdCond.Visible = false
		'
		'btnStdCondClose
		'
		Me.btnStdCondClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
		Me.btnStdCondClose.Location = New System.Drawing.Point(288, 11)
		Me.btnStdCondClose.Name = "btnStdCondClose"
		Me.btnStdCondClose.Size = New System.Drawing.Size(48, 23)
		Me.btnStdCondClose.TabIndex = 72
		Me.btnStdCondClose.Text = "Close"
		Me.btnStdCondClose.UseVisualStyleBackColor = true
		'
		'rbReal
		'
		Me.rbReal.AutoSize = true
		Me.rbReal.Font = New System.Drawing.Font("MS UI Gothic", 9!, System.Drawing.FontStyle.Bold)
		Me.rbReal.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.rbReal.Location = New System.Drawing.Point(217, 18)
		Me.rbReal.Name = "rbReal"
		Me.rbReal.Size = New System.Drawing.Size(50, 16)
		Me.rbReal.TabIndex = 71
		Me.rbReal.TabStop = true
		Me.rbReal.Text = "Real"
		Me.rbReal.UseVisualStyleBackColor = true
		'
		'rbStd1
		'
		Me.rbStd1.AutoSize = true
		Me.rbStd1.Font = New System.Drawing.Font("MS UI Gothic", 9!, System.Drawing.FontStyle.Bold)
		Me.rbStd1.Location = New System.Drawing.Point(9, 18)
		Me.rbStd1.Name = "rbStd1"
		Me.rbStd1.Size = New System.Drawing.Size(83, 16)
		Me.rbStd1.TabIndex = 70
		Me.rbStd1.TabStop = true
		Me.rbStd1.Text = "Standard1"
		Me.rbStd1.UseVisualStyleBackColor = true
		'
		'rbStd2
		'
		Me.rbStd2.AutoSize = true
		Me.rbStd2.Font = New System.Drawing.Font("MS UI Gothic", 9!, System.Drawing.FontStyle.Bold)
		Me.rbStd2.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.rbStd2.Location = New System.Drawing.Point(110, 18)
		Me.rbStd2.Name = "rbStd2"
		Me.rbStd2.Size = New System.Drawing.Size(83, 16)
		Me.rbStd2.TabIndex = 70
		Me.rbStd2.TabStop = true
		Me.rbStd2.Text = "Standard2"
		Me.rbStd2.UseVisualStyleBackColor = true
		'
		'dgvStd
		'
		Me.dgvStd.AllowUserToAddRows = false
		Me.dgvStd.AllowUserToDeleteRows = false
		Me.dgvStd.Anchor = System.Windows.Forms.AnchorStyles.None
		Me.dgvStd.ColumnHeadersHeight = 29
		Me.dgvStd.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
		Me.dgvStd.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Column1, Me.Column2})
		DataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
		DataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window
		DataGridViewCellStyle5.Font = New System.Drawing.Font("MS UI Gothic", 9!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		DataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText
		DataGridViewCellStyle5.SelectionBackColor = System.Drawing.Color.Transparent
		DataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.ControlText
		DataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
		Me.dgvStd.DefaultCellStyle = DataGridViewCellStyle5
		Me.dgvStd.GridColor = System.Drawing.SystemColors.ButtonHighlight
		Me.dgvStd.Location = New System.Drawing.Point(7, 40)
		Me.dgvStd.MultiSelect = false
		Me.dgvStd.Name = "dgvStd"
		Me.dgvStd.ReadOnly = true
		Me.dgvStd.RowHeadersWidth = 51
		Me.dgvStd.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
		Me.dgvStd.RowTemplate.Height = 28
		Me.dgvStd.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.dgvStd.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect
		Me.dgvStd.Size = New System.Drawing.Size(312, 71)
		Me.dgvStd.TabIndex = 69
		Me.dgvStd.TabStop = false
		'
		'Column1
		'
		Me.Column1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
		DataGridViewCellStyle3.Font = New System.Drawing.Font("MS UI Gothic", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		DataGridViewCellStyle3.ForeColor = System.Drawing.Color.Black
		DataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.White
		DataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.Black
		Me.Column1.DefaultCellStyle = DataGridViewCellStyle3
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
		DataGridViewCellStyle4.Font = New System.Drawing.Font("MS UI Gothic", 18!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		DataGridViewCellStyle4.ForeColor = System.Drawing.Color.Black
		DataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.White
		DataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.Black
		Me.Column2.DefaultCellStyle = DataGridViewCellStyle4
		Me.Column2.FillWeight = 101.5228!
		Me.Column2.HeaderText = "Value"
		Me.Column2.MinimumWidth = 6
		Me.Column2.Name = "Column2"
		Me.Column2.ReadOnly = true
		'
		'Label1
		'
		Me.Label1.AutoSize = true
		Me.Label1.Font = New System.Drawing.Font("Meiryo UI", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.Label1.Location = New System.Drawing.Point(141, 82)
		Me.Label1.Name = "Label1"
		Me.Label1.Size = New System.Drawing.Size(25, 20)
		Me.Label1.TabIndex = 82
		Me.Label1.Text = "％"
		Me.Label1.Visible = false
		'
		'lblZRangeOverLamp
		'
		Me.lblZRangeOverLamp.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left),System.Windows.Forms.AnchorStyles)
		Me.lblZRangeOverLamp.BackColor = System.Drawing.SystemColors.ButtonHighlight
		Me.lblZRangeOverLamp.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		Me.lblZRangeOverLamp.Font = New System.Drawing.Font("Meiryo UI", 8.25!)
		Me.lblZRangeOverLamp.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.lblZRangeOverLamp.Location = New System.Drawing.Point(1067, 903)
		Me.lblZRangeOverLamp.Name = "lblZRangeOverLamp"
		Me.lblZRangeOverLamp.Size = New System.Drawing.Size(128, 18)
		Me.lblZRangeOverLamp.TabIndex = 80
		Me.lblZRangeOverLamp.Text = "Z Stress range over"
		Me.lblZRangeOverLamp.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.lblZRangeOverLamp.Visible = false
		'
		'pnlCalculation
		'
		Me.pnlCalculation.BackColor = System.Drawing.SystemColors.GradientInactiveCaption
		Me.pnlCalculation.Controls.Add(Me.lblShearVeloVal)
		Me.pnlCalculation.Controls.Add(Me.lblElasticModVal)
		Me.pnlCalculation.Controls.Add(Me.lblTangentLabel)
		Me.pnlCalculation.Controls.Add(Me.lblShearVeloLabel)
		Me.pnlCalculation.Controls.Add(Me.lblTangentVal)
		Me.pnlCalculation.Controls.Add(Me.lblElasticModLabel)
		Me.pnlCalculation.Controls.Add(Me.lblStorageModVal)
		Me.pnlCalculation.Controls.Add(Me.lblStrgModLabel)
		Me.pnlCalculation.Controls.Add(Me.lblShearModVal)
		Me.pnlCalculation.Controls.Add(Me.lblShearModLabel)
		Me.pnlCalculation.Controls.Add(Me.lblSmplStrnVal)
		Me.pnlCalculation.Controls.Add(Me.lblPhaseVal)
		Me.pnlCalculation.Controls.Add(Me.lblPhaseLabel)
		Me.pnlCalculation.Controls.Add(Me.lblSmpStrnLabel)
		Me.pnlCalculation.Controls.Add(Me.lblViscoVal)
		Me.pnlCalculation.Controls.Add(Me.lblViscoLabel)
		Me.pnlCalculation.Controls.Add(Me.lblStressPVal)
		Me.pnlCalculation.Controls.Add(Me.lblCurrStressPLabel)
		Me.pnlCalculation.Controls.Add(Me.lblStressVal)
		Me.pnlCalculation.Controls.Add(Me.lblCurrStressLabel)
		Me.pnlCalculation.Location = New System.Drawing.Point(1037, 545)
		Me.pnlCalculation.Name = "pnlCalculation"
		Me.pnlCalculation.Size = New System.Drawing.Size(155, 370)
		Me.pnlCalculation.TabIndex = 77
		Me.pnlCalculation.Visible = false
		'
		'lblShearVeloVal
		'
		Me.lblShearVeloVal.BackColor = System.Drawing.Color.White
		Me.lblShearVeloVal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		Me.lblShearVeloVal.Font = New System.Drawing.Font("MS UI Gothic", 21.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.lblShearVeloVal.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.lblShearVeloVal.Location = New System.Drawing.Point(3, 333)
		Me.lblShearVeloVal.Name = "lblShearVeloVal"
		Me.lblShearVeloVal.Size = New System.Drawing.Size(145, 30)
		Me.lblShearVeloVal.TabIndex = 77
		Me.lblShearVeloVal.Text = "0.0"
		Me.lblShearVeloVal.TextAlign = System.Drawing.ContentAlignment.MiddleRight
		Me.lblShearVeloVal.Visible = false
		'
		'lblElasticModVal
		'
		Me.lblElasticModVal.BackColor = System.Drawing.Color.White
		Me.lblElasticModVal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		Me.lblElasticModVal.Font = New System.Drawing.Font("MS UI Gothic", 21.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.lblElasticModVal.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.lblElasticModVal.Location = New System.Drawing.Point(6, 439)
		Me.lblElasticModVal.Name = "lblElasticModVal"
		Me.lblElasticModVal.Size = New System.Drawing.Size(145, 30)
		Me.lblElasticModVal.TabIndex = 71
		Me.lblElasticModVal.Text = "0.0"
		Me.lblElasticModVal.TextAlign = System.Drawing.ContentAlignment.MiddleRight
		Me.lblElasticModVal.Visible = false
		'
		'lblTangentLabel
		'
		Me.lblTangentLabel.AutoSize = true
		Me.lblTangentLabel.Font = New System.Drawing.Font("Meiryo UI", 12!)
		Me.lblTangentLabel.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.lblTangentLabel.Location = New System.Drawing.Point(8, 471)
		Me.lblTangentLabel.Name = "lblTangentLabel"
		Me.lblTangentLabel.Size = New System.Drawing.Size(71, 20)
		Me.lblTangentLabel.TabIndex = 74
		Me.lblTangentLabel.Text = "Tangent"
		Me.lblTangentLabel.Visible = false
		'
		'lblShearVeloLabel
		'
		Me.lblShearVeloLabel.AutoSize = true
		Me.lblShearVeloLabel.Font = New System.Drawing.Font("Meiryo UI", 12!)
		Me.lblShearVeloLabel.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.lblShearVeloLabel.Location = New System.Drawing.Point(3, 313)
		Me.lblShearVeloLabel.Name = "lblShearVeloLabel"
		Me.lblShearVeloLabel.Size = New System.Drawing.Size(135, 20)
		Me.lblShearVeloLabel.TabIndex = 76
		Me.lblShearVeloLabel.Text = "Shear Rate(1/S)"
		Me.lblShearVeloLabel.Visible = false
		'
		'lblTangentVal
		'
		Me.lblTangentVal.BackColor = System.Drawing.Color.White
		Me.lblTangentVal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		Me.lblTangentVal.Font = New System.Drawing.Font("MS UI Gothic", 21.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.lblTangentVal.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.lblTangentVal.Location = New System.Drawing.Point(7, 491)
		Me.lblTangentVal.Name = "lblTangentVal"
		Me.lblTangentVal.Size = New System.Drawing.Size(145, 30)
		Me.lblTangentVal.TabIndex = 75
		Me.lblTangentVal.Text = "0.0"
		Me.lblTangentVal.TextAlign = System.Drawing.ContentAlignment.MiddleRight
		Me.lblTangentVal.Visible = false
		'
		'lblElasticModLabel
		'
		Me.lblElasticModLabel.AutoSize = true
		Me.lblElasticModLabel.Font = New System.Drawing.Font("Meiryo UI", 12!)
		Me.lblElasticModLabel.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.lblElasticModLabel.Location = New System.Drawing.Point(7, 420)
		Me.lblElasticModLabel.Name = "lblElasticModLabel"
		Me.lblElasticModLabel.Size = New System.Drawing.Size(129, 20)
		Me.lblElasticModLabel.TabIndex = 70
		Me.lblElasticModLabel.Text = "Elastic Mod(Pa)"
		Me.lblElasticModLabel.Visible = false
		'
		'lblStorageModVal
		'
		Me.lblStorageModVal.BackColor = System.Drawing.Color.White
		Me.lblStorageModVal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		Me.lblStorageModVal.Font = New System.Drawing.Font("MS UI Gothic", 21.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.lblStorageModVal.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.lblStorageModVal.Location = New System.Drawing.Point(3, 386)
		Me.lblStorageModVal.Name = "lblStorageModVal"
		Me.lblStorageModVal.Size = New System.Drawing.Size(145, 30)
		Me.lblStorageModVal.TabIndex = 69
		Me.lblStorageModVal.Text = "0.0"
		Me.lblStorageModVal.TextAlign = System.Drawing.ContentAlignment.MiddleRight
		Me.lblStorageModVal.Visible = false
		'
		'lblStrgModLabel
		'
		Me.lblStrgModLabel.AutoSize = true
		Me.lblStrgModLabel.Font = New System.Drawing.Font("Meiryo UI", 12!)
		Me.lblStrgModLabel.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.lblStrgModLabel.Location = New System.Drawing.Point(3, 366)
		Me.lblStrgModLabel.Name = "lblStrgModLabel"
		Me.lblStrgModLabel.Size = New System.Drawing.Size(141, 20)
		Me.lblStrgModLabel.TabIndex = 68
		Me.lblStrgModLabel.Text = "Storage Mod(Pa)"
		Me.lblStrgModLabel.Visible = false
		'
		'lblShearModVal
		'
		Me.lblShearModVal.BackColor = System.Drawing.Color.White
		Me.lblShearModVal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		Me.lblShearModVal.Font = New System.Drawing.Font("MS UI Gothic", 21.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.lblShearModVal.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.lblShearModVal.Location = New System.Drawing.Point(3, 282)
		Me.lblShearModVal.Name = "lblShearModVal"
		Me.lblShearModVal.Size = New System.Drawing.Size(145, 30)
		Me.lblShearModVal.TabIndex = 67
		Me.lblShearModVal.Text = "0.0"
		Me.lblShearModVal.TextAlign = System.Drawing.ContentAlignment.MiddleRight
		Me.lblShearModVal.Visible = false
		'
		'lblShearModLabel
		'
		Me.lblShearModLabel.AutoSize = true
		Me.lblShearModLabel.Font = New System.Drawing.Font("Meiryo UI", 12!)
		Me.lblShearModLabel.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.lblShearModLabel.Location = New System.Drawing.Point(3, 262)
		Me.lblShearModLabel.Name = "lblShearModLabel"
		Me.lblShearModLabel.Size = New System.Drawing.Size(125, 20)
		Me.lblShearModLabel.TabIndex = 66
		Me.lblShearModLabel.Text = "Shear Mod(Pa)"
		Me.lblShearModLabel.Visible = false
		'
		'lblSmplStrnVal
		'
		Me.lblSmplStrnVal.BackColor = System.Drawing.Color.White
		Me.lblSmplStrnVal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		Me.lblSmplStrnVal.Font = New System.Drawing.Font("MS UI Gothic", 21.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.lblSmplStrnVal.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.lblSmplStrnVal.Location = New System.Drawing.Point(3, 231)
		Me.lblSmplStrnVal.Name = "lblSmplStrnVal"
		Me.lblSmplStrnVal.Size = New System.Drawing.Size(145, 30)
		Me.lblSmplStrnVal.TabIndex = 65
		Me.lblSmplStrnVal.Text = "0.0"
		Me.lblSmplStrnVal.TextAlign = System.Drawing.ContentAlignment.MiddleRight
		'
		'lblPhaseVal
		'
		Me.lblPhaseVal.BackColor = System.Drawing.Color.White
		Me.lblPhaseVal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		Me.lblPhaseVal.Font = New System.Drawing.Font("MS UI Gothic", 21.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.lblPhaseVal.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.lblPhaseVal.Location = New System.Drawing.Point(3, 180)
		Me.lblPhaseVal.Name = "lblPhaseVal"
		Me.lblPhaseVal.Size = New System.Drawing.Size(145, 30)
		Me.lblPhaseVal.TabIndex = 63
		Me.lblPhaseVal.Text = "0.0"
		Me.lblPhaseVal.TextAlign = System.Drawing.ContentAlignment.MiddleRight
		'
		'lblPhaseLabel
		'
		Me.lblPhaseLabel.AutoSize = true
		Me.lblPhaseLabel.Font = New System.Drawing.Font("Meiryo UI", 12!)
		Me.lblPhaseLabel.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.lblPhaseLabel.Location = New System.Drawing.Point(3, 160)
		Me.lblPhaseLabel.Name = "lblPhaseLabel"
		Me.lblPhaseLabel.Size = New System.Drawing.Size(100, 20)
		Me.lblPhaseLabel.TabIndex = 62
		Me.lblPhaseLabel.Text = "Phase(Deg)"
		'
		'lblSmpStrnLabel
		'
		Me.lblSmpStrnLabel.AutoSize = true
		Me.lblSmpStrnLabel.Font = New System.Drawing.Font("Meiryo UI", 12!)
		Me.lblSmpStrnLabel.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.lblSmpStrnLabel.Location = New System.Drawing.Point(3, 211)
		Me.lblSmpStrnLabel.Name = "lblSmpStrnLabel"
		Me.lblSmpStrnLabel.Size = New System.Drawing.Size(105, 20)
		Me.lblSmpStrnLabel.TabIndex = 64
		Me.lblSmpStrnLabel.Text = "Samp.Strain"
		'
		'lblViscoVal
		'
		Me.lblViscoVal.BackColor = System.Drawing.Color.White
		Me.lblViscoVal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		Me.lblViscoVal.Font = New System.Drawing.Font("MS UI Gothic", 21.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.lblViscoVal.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.lblViscoVal.Location = New System.Drawing.Point(3, 129)
		Me.lblViscoVal.Name = "lblViscoVal"
		Me.lblViscoVal.Size = New System.Drawing.Size(145, 30)
		Me.lblViscoVal.TabIndex = 61
		Me.lblViscoVal.Text = "0.0"
		Me.lblViscoVal.TextAlign = System.Drawing.ContentAlignment.MiddleRight
		'
		'lblViscoLabel
		'
		Me.lblViscoLabel.AutoSize = true
		Me.lblViscoLabel.Font = New System.Drawing.Font("Meiryo UI", 12!)
		Me.lblViscoLabel.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.lblViscoLabel.Location = New System.Drawing.Point(3, 108)
		Me.lblViscoLabel.Name = "lblViscoLabel"
		Me.lblViscoLabel.Size = New System.Drawing.Size(120, 20)
		Me.lblViscoLabel.TabIndex = 60
		Me.lblViscoLabel.Text = "Viscosity(PaS)"
		'
		'lblStressPVal
		'
		Me.lblStressPVal.BackColor = System.Drawing.Color.White
		Me.lblStressPVal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		Me.lblStressPVal.Font = New System.Drawing.Font("MS UI Gothic", 21.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.lblStressPVal.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.lblStressPVal.Location = New System.Drawing.Point(3, 76)
		Me.lblStressPVal.Name = "lblStressPVal"
		Me.lblStressPVal.Size = New System.Drawing.Size(145, 30)
		Me.lblStressPVal.TabIndex = 59
		Me.lblStressPVal.Text = "0.0"
		Me.lblStressPVal.TextAlign = System.Drawing.ContentAlignment.MiddleRight
		'
		'lblCurrStressPLabel
		'
		Me.lblCurrStressPLabel.AutoSize = true
		Me.lblCurrStressPLabel.Font = New System.Drawing.Font("Meiryo UI", 12!)
		Me.lblCurrStressPLabel.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.lblCurrStressPLabel.Location = New System.Drawing.Point(3, 56)
		Me.lblCurrStressPLabel.Name = "lblCurrStressPLabel"
		Me.lblCurrStressPLabel.Size = New System.Drawing.Size(93, 20)
		Me.lblCurrStressPLabel.TabIndex = 58
		Me.lblCurrStressPLabel.Text = "Stress (%)"
		'
		'lblStressVal
		'
		Me.lblStressVal.BackColor = System.Drawing.Color.White
		Me.lblStressVal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		Me.lblStressVal.Font = New System.Drawing.Font("MS UI Gothic", 21.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.lblStressVal.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.lblStressVal.Location = New System.Drawing.Point(3, 23)
		Me.lblStressVal.Name = "lblStressVal"
		Me.lblStressVal.Size = New System.Drawing.Size(145, 30)
		Me.lblStressVal.TabIndex = 57
		Me.lblStressVal.Text = "0"
		Me.lblStressVal.TextAlign = System.Drawing.ContentAlignment.MiddleRight
		'
		'lblCurrStressLabel
		'
		Me.lblCurrStressLabel.AutoSize = true
		Me.lblCurrStressLabel.Font = New System.Drawing.Font("Meiryo UI", 12!)
		Me.lblCurrStressLabel.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.lblCurrStressLabel.Location = New System.Drawing.Point(3, 2)
		Me.lblCurrStressLabel.Name = "lblCurrStressLabel"
		Me.lblCurrStressLabel.Size = New System.Drawing.Size(98, 20)
		Me.lblCurrStressLabel.TabIndex = 56
		Me.lblCurrStressLabel.Text = "Stress (uN)"
		'
		'btnStdInfo
		'
		Me.btnStdInfo.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left),System.Windows.Forms.AnchorStyles)
		Me.btnStdInfo.Font = New System.Drawing.Font("Meiryo UI", 12!)
		Me.btnStdInfo.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.btnStdInfo.Location = New System.Drawing.Point(176, 888)
		Me.btnStdInfo.Name = "btnStdInfo"
		Me.btnStdInfo.Size = New System.Drawing.Size(170, 30)
		Me.btnStdInfo.TabIndex = 68
		Me.btnStdInfo.Text = "Standard Info"
		Me.btnStdInfo.UseVisualStyleBackColor = true
		'
		'btnThinning
		'
		Me.btnThinning.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left),System.Windows.Forms.AnchorStyles)
		Me.btnThinning.Font = New System.Drawing.Font("Meiryo UI", 12!)
		Me.btnThinning.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.btnThinning.Location = New System.Drawing.Point(2, 888)
		Me.btnThinning.Name = "btnThinning"
		Me.btnThinning.Size = New System.Drawing.Size(170, 30)
		Me.btnThinning.TabIndex = 63
		Me.btnThinning.Text = "Thinning"
		Me.btnThinning.UseVisualStyleBackColor = true
		'
		'lblChartCoordinate
		'
		Me.lblChartCoordinate.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
		Me.lblChartCoordinate.AutoSize = true
		Me.lblChartCoordinate.Font = New System.Drawing.Font("Meiryo UI", 12!)
		Me.lblChartCoordinate.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.lblChartCoordinate.Location = New System.Drawing.Point(827, 884)
		Me.lblChartCoordinate.Name = "lblChartCoordinate"
		Me.lblChartCoordinate.Size = New System.Drawing.Size(37, 20)
		Me.lblChartCoordinate.TabIndex = 55
		Me.lblChartCoordinate.Text = "----"
		'
		'gbxCurrentVal
		'
		Me.gbxCurrentVal.BackColor = System.Drawing.SystemColors.GradientInactiveCaption
		Me.gbxCurrentVal.Controls.Add(Me.lblCurrSGMVVal)
		Me.gbxCurrentVal.Controls.Add(Me.lblCurrSGMV)
		Me.gbxCurrentVal.Controls.Add(Me.lblCurrSGTempVal)
		Me.gbxCurrentVal.Controls.Add(Me.lblCurrSGTemp)
		Me.gbxCurrentVal.Controls.Add(Me.pnlPosition)
		Me.gbxCurrentVal.Controls.Add(Me.lblCurrPBMVVal)
		Me.gbxCurrentVal.Controls.Add(Me.lblCurrPBMV)
		Me.gbxCurrentVal.Controls.Add(Me.lblCurrPBTempVal)
		Me.gbxCurrentVal.Controls.Add(Me.lblCurrPBTemp)
		Me.gbxCurrentVal.Controls.Add(Me.lblCurrHPMV)
		Me.gbxCurrentVal.Controls.Add(Me.lblCurrHPTemp)
		Me.gbxCurrentVal.Controls.Add(Me.lblElapsedTime)
		Me.gbxCurrentVal.Controls.Add(Me.lblCurrHPMVVal)
		Me.gbxCurrentVal.Controls.Add(Me.lblCurrHPTempVal)
		Me.gbxCurrentVal.Controls.Add(Me.lblElapsedVal)
		Me.gbxCurrentVal.Controls.Add(Me.GroupBox0)
		Me.gbxCurrentVal.Dock = System.Windows.Forms.DockStyle.Right
		Me.gbxCurrentVal.Font = New System.Drawing.Font("Meiryo UI", 9.75!)
		Me.gbxCurrentVal.Location = New System.Drawing.Point(1203, 62)
		Me.gbxCurrentVal.Name = "gbxCurrentVal"
		Me.gbxCurrentVal.Size = New System.Drawing.Size(165, 859)
		Me.gbxCurrentVal.TabIndex = 54
		Me.gbxCurrentVal.TabStop = false
		Me.gbxCurrentVal.Visible = false
		'
		'lblCurrSGMVVal
		'
		Me.lblCurrSGMVVal.BackColor = System.Drawing.Color.White
		Me.lblCurrSGMVVal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		Me.lblCurrSGMVVal.Font = New System.Drawing.Font("MS UI Gothic", 24!)
		Me.lblCurrSGMVVal.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.lblCurrSGMVVal.Location = New System.Drawing.Point(10, 408)
		Me.lblCurrSGMVVal.Name = "lblCurrSGMVVal"
		Me.lblCurrSGMVVal.Size = New System.Drawing.Size(145, 42)
		Me.lblCurrSGMVVal.TabIndex = 81
		Me.lblCurrSGMVVal.Text = "000.0"
		Me.lblCurrSGMVVal.TextAlign = System.Drawing.ContentAlignment.MiddleRight
		'
		'lblCurrSGMV
		'
		Me.lblCurrSGMV.AutoSize = true
		Me.lblCurrSGMV.Font = New System.Drawing.Font("Meiryo UI", 12!)
		Me.lblCurrSGMV.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.lblCurrSGMV.Location = New System.Drawing.Point(12, 387)
		Me.lblCurrSGMV.Name = "lblCurrSGMV"
		Me.lblCurrSGMV.Size = New System.Drawing.Size(96, 20)
		Me.lblCurrSGMV.TabIndex = 80
		Me.lblCurrSGMV.Text = "SG MV (%)"
		'
		'lblCurrSGTempVal
		'
		Me.lblCurrSGTempVal.BackColor = System.Drawing.Color.White
		Me.lblCurrSGTempVal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		Me.lblCurrSGTempVal.Font = New System.Drawing.Font("MS UI Gothic", 24!)
		Me.lblCurrSGTempVal.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.lblCurrSGTempVal.Location = New System.Drawing.Point(11, 342)
		Me.lblCurrSGTempVal.Name = "lblCurrSGTempVal"
		Me.lblCurrSGTempVal.Size = New System.Drawing.Size(145, 42)
		Me.lblCurrSGTempVal.TabIndex = 79
		Me.lblCurrSGTempVal.Text = "000.0"
		Me.lblCurrSGTempVal.TextAlign = System.Drawing.ContentAlignment.MiddleRight
		'
		'lblCurrSGTemp
		'
		Me.lblCurrSGTemp.AutoSize = true
		Me.lblCurrSGTemp.Font = New System.Drawing.Font("Meiryo UI", 12!)
		Me.lblCurrSGTemp.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.lblCurrSGTemp.Location = New System.Drawing.Point(7, 322)
		Me.lblCurrSGTemp.Name = "lblCurrSGTemp"
		Me.lblCurrSGTemp.Size = New System.Drawing.Size(148, 20)
		Me.lblCurrSGTemp.TabIndex = 78
		Me.lblCurrSGTemp.Text = "SG Temp. (deg C)"
		'
		'pnlPosition
		'
		Me.pnlPosition.BackColor = System.Drawing.SystemColors.GradientInactiveCaption
		Me.pnlPosition.Controls.Add(Me.lblZConfPosLabel)
		Me.pnlPosition.Controls.Add(Me.lblZConfPosVal)
		Me.pnlPosition.Controls.Add(Me.lblZeroPosVal)
		Me.pnlPosition.Controls.Add(Me.lblZeroPosLabel)
		Me.pnlPosition.Controls.Add(Me.lblGAPVal)
		Me.pnlPosition.Controls.Add(Me.lblCurrXPos)
		Me.pnlPosition.Controls.Add(Me.lblGAPLabel)
		Me.pnlPosition.Controls.Add(Me.lblCurrXPosVal)
		Me.pnlPosition.Controls.Add(Me.lblCurrYPosVal)
		Me.pnlPosition.Controls.Add(Me.lblCurrZPosVal)
		Me.pnlPosition.Controls.Add(Me.lblCurrYPos)
		Me.pnlPosition.Controls.Add(Me.lblCurrZPos)
		Me.pnlPosition.Location = New System.Drawing.Point(7, 483)
		Me.pnlPosition.Name = "pnlPosition"
		Me.pnlPosition.Size = New System.Drawing.Size(155, 370)
		Me.pnlPosition.TabIndex = 77
		Me.pnlPosition.Visible = false
		'
		'lblZConfPosLabel
		'
		Me.lblZConfPosLabel.AutoSize = true
		Me.lblZConfPosLabel.Font = New System.Drawing.Font("Meiryo UI", 12!)
		Me.lblZConfPosLabel.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.lblZConfPosLabel.Location = New System.Drawing.Point(2, 261)
		Me.lblZConfPosLabel.Name = "lblZConfPosLabel"
		Me.lblZConfPosLabel.Size = New System.Drawing.Size(153, 20)
		Me.lblZConfPosLabel.TabIndex = 68
		Me.lblZConfPosLabel.Text = "Z Conf. Pos. (mm)"
		'
		'lblZConfPosVal
		'
		Me.lblZConfPosVal.BackColor = System.Drawing.Color.White
		Me.lblZConfPosVal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		Me.lblZConfPosVal.Font = New System.Drawing.Font("MS UI Gothic", 21.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.lblZConfPosVal.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.lblZConfPosVal.Location = New System.Drawing.Point(4, 282)
		Me.lblZConfPosVal.Name = "lblZConfPosVal"
		Me.lblZConfPosVal.Size = New System.Drawing.Size(145, 30)
		Me.lblZConfPosVal.TabIndex = 67
		Me.lblZConfPosVal.Text = "00.000"
		Me.lblZConfPosVal.TextAlign = System.Drawing.ContentAlignment.MiddleRight
		'
		'lblZeroPosVal
		'
		Me.lblZeroPosVal.BackColor = System.Drawing.Color.White
		Me.lblZeroPosVal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		Me.lblZeroPosVal.Font = New System.Drawing.Font("MS UI Gothic", 21.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.lblZeroPosVal.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.lblZeroPosVal.Location = New System.Drawing.Point(3, 231)
		Me.lblZeroPosVal.Name = "lblZeroPosVal"
		Me.lblZeroPosVal.Size = New System.Drawing.Size(145, 30)
		Me.lblZeroPosVal.TabIndex = 67
		Me.lblZeroPosVal.Text = "00.000"
		Me.lblZeroPosVal.TextAlign = System.Drawing.ContentAlignment.MiddleRight
		'
		'lblZeroPosLabel
		'
		Me.lblZeroPosLabel.AutoSize = true
		Me.lblZeroPosLabel.Font = New System.Drawing.Font("Meiryo UI", 12!)
		Me.lblZeroPosLabel.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.lblZeroPosLabel.Location = New System.Drawing.Point(3, 211)
		Me.lblZeroPosLabel.Name = "lblZeroPosLabel"
		Me.lblZeroPosLabel.Size = New System.Drawing.Size(148, 20)
		Me.lblZeroPosLabel.TabIndex = 66
		Me.lblZeroPosLabel.Text = "Z Zero Pos. (mm)"
		'
		'lblGAPVal
		'
		Me.lblGAPVal.BackColor = System.Drawing.Color.White
		Me.lblGAPVal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		Me.lblGAPVal.Font = New System.Drawing.Font("MS UI Gothic", 21.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.lblGAPVal.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.lblGAPVal.Location = New System.Drawing.Point(3, 180)
		Me.lblGAPVal.Name = "lblGAPVal"
		Me.lblGAPVal.Size = New System.Drawing.Size(145, 30)
		Me.lblGAPVal.TabIndex = 65
		Me.lblGAPVal.Text = "00.000"
		Me.lblGAPVal.TextAlign = System.Drawing.ContentAlignment.MiddleRight
		'
		'lblCurrXPos
		'
		Me.lblCurrXPos.AutoSize = true
		Me.lblCurrXPos.Font = New System.Drawing.Font("Meiryo UI", 12!)
		Me.lblCurrXPos.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.lblCurrXPos.Location = New System.Drawing.Point(3, 2)
		Me.lblCurrXPos.Name = "lblCurrXPos"
		Me.lblCurrXPos.Size = New System.Drawing.Size(108, 20)
		Me.lblCurrXPos.TabIndex = 55
		Me.lblCurrXPos.Text = "X Pos. (mm)"
		'
		'lblGAPLabel
		'
		Me.lblGAPLabel.AutoSize = true
		Me.lblGAPLabel.Font = New System.Drawing.Font("Meiryo UI", 12!)
		Me.lblGAPLabel.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.lblGAPLabel.Location = New System.Drawing.Point(3, 160)
		Me.lblGAPLabel.Name = "lblGAPLabel"
		Me.lblGAPLabel.Size = New System.Drawing.Size(91, 20)
		Me.lblGAPLabel.TabIndex = 64
		Me.lblGAPLabel.Text = "GAP (mm)"
		'
		'lblCurrXPosVal
		'
		Me.lblCurrXPosVal.BackColor = System.Drawing.Color.White
		Me.lblCurrXPosVal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		Me.lblCurrXPosVal.Font = New System.Drawing.Font("MS UI Gothic", 21.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.lblCurrXPosVal.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.lblCurrXPosVal.Location = New System.Drawing.Point(3, 23)
		Me.lblCurrXPosVal.Name = "lblCurrXPosVal"
		Me.lblCurrXPosVal.Size = New System.Drawing.Size(145, 30)
		Me.lblCurrXPosVal.TabIndex = 52
		Me.lblCurrXPosVal.Text = "00.000"
		Me.lblCurrXPosVal.TextAlign = System.Drawing.ContentAlignment.MiddleRight
		'
		'lblCurrYPosVal
		'
		Me.lblCurrYPosVal.BackColor = System.Drawing.Color.White
		Me.lblCurrYPosVal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		Me.lblCurrYPosVal.Font = New System.Drawing.Font("MS UI Gothic", 21.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.lblCurrYPosVal.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.lblCurrYPosVal.Location = New System.Drawing.Point(3, 76)
		Me.lblCurrYPosVal.Name = "lblCurrYPosVal"
		Me.lblCurrYPosVal.Size = New System.Drawing.Size(145, 30)
		Me.lblCurrYPosVal.TabIndex = 52
		Me.lblCurrYPosVal.Text = "00.000"
		Me.lblCurrYPosVal.TextAlign = System.Drawing.ContentAlignment.MiddleRight
		'
		'lblCurrZPosVal
		'
		Me.lblCurrZPosVal.BackColor = System.Drawing.Color.White
		Me.lblCurrZPosVal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		Me.lblCurrZPosVal.Font = New System.Drawing.Font("MS UI Gothic", 21.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.lblCurrZPosVal.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.lblCurrZPosVal.Location = New System.Drawing.Point(3, 129)
		Me.lblCurrZPosVal.Name = "lblCurrZPosVal"
		Me.lblCurrZPosVal.Size = New System.Drawing.Size(145, 30)
		Me.lblCurrZPosVal.TabIndex = 52
		Me.lblCurrZPosVal.Text = "00.000"
		Me.lblCurrZPosVal.TextAlign = System.Drawing.ContentAlignment.MiddleRight
		'
		'lblCurrYPos
		'
		Me.lblCurrYPos.AutoSize = true
		Me.lblCurrYPos.Font = New System.Drawing.Font("Meiryo UI", 12!)
		Me.lblCurrYPos.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.lblCurrYPos.Location = New System.Drawing.Point(3, 58)
		Me.lblCurrYPos.Name = "lblCurrYPos"
		Me.lblCurrYPos.Size = New System.Drawing.Size(107, 20)
		Me.lblCurrYPos.TabIndex = 55
		Me.lblCurrYPos.Text = "Y Pos. (mm)"
		'
		'lblCurrZPos
		'
		Me.lblCurrZPos.AutoSize = true
		Me.lblCurrZPos.Font = New System.Drawing.Font("Meiryo UI", 12!)
		Me.lblCurrZPos.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.lblCurrZPos.Location = New System.Drawing.Point(3, 108)
		Me.lblCurrZPos.Name = "lblCurrZPos"
		Me.lblCurrZPos.Size = New System.Drawing.Size(107, 20)
		Me.lblCurrZPos.TabIndex = 55
		Me.lblCurrZPos.Text = "Z Pos. (mm)"
		'
		'lblCurrPBMVVal
		'
		Me.lblCurrPBMVVal.BackColor = System.Drawing.Color.White
		Me.lblCurrPBMVVal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		Me.lblCurrPBMVVal.Font = New System.Drawing.Font("MS UI Gothic", 24!)
		Me.lblCurrPBMVVal.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.lblCurrPBMVVal.Location = New System.Drawing.Point(10, 277)
		Me.lblCurrPBMVVal.Name = "lblCurrPBMVVal"
		Me.lblCurrPBMVVal.Size = New System.Drawing.Size(145, 42)
		Me.lblCurrPBMVVal.TabIndex = 59
		Me.lblCurrPBMVVal.Text = "000.0"
		Me.lblCurrPBMVVal.TextAlign = System.Drawing.ContentAlignment.MiddleRight
		'
		'lblCurrPBMV
		'
		Me.lblCurrPBMV.AutoSize = true
		Me.lblCurrPBMV.Font = New System.Drawing.Font("Meiryo UI", 12!)
		Me.lblCurrPBMV.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.lblCurrPBMV.Location = New System.Drawing.Point(13, 257)
		Me.lblCurrPBMV.Name = "lblCurrPBMV"
		Me.lblCurrPBMV.Size = New System.Drawing.Size(95, 20)
		Me.lblCurrPBMV.TabIndex = 58
		Me.lblCurrPBMV.Text = "PB MV (%)"
		'
		'lblCurrPBTempVal
		'
		Me.lblCurrPBTempVal.BackColor = System.Drawing.Color.White
		Me.lblCurrPBTempVal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		Me.lblCurrPBTempVal.Font = New System.Drawing.Font("MS UI Gothic", 24!)
		Me.lblCurrPBTempVal.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.lblCurrPBTempVal.Location = New System.Drawing.Point(10, 214)
		Me.lblCurrPBTempVal.Name = "lblCurrPBTempVal"
		Me.lblCurrPBTempVal.Size = New System.Drawing.Size(145, 42)
		Me.lblCurrPBTempVal.TabIndex = 57
		Me.lblCurrPBTempVal.Text = "000.0"
		Me.lblCurrPBTempVal.TextAlign = System.Drawing.ContentAlignment.MiddleRight
		'
		'lblCurrPBTemp
		'
		Me.lblCurrPBTemp.AutoSize = true
		Me.lblCurrPBTemp.Font = New System.Drawing.Font("Meiryo UI", 12!)
		Me.lblCurrPBTemp.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.lblCurrPBTemp.Location = New System.Drawing.Point(11, 194)
		Me.lblCurrPBTemp.Name = "lblCurrPBTemp"
		Me.lblCurrPBTemp.Size = New System.Drawing.Size(147, 20)
		Me.lblCurrPBTemp.TabIndex = 56
		Me.lblCurrPBTemp.Text = "PB Temp. (deg C)"
		'
		'lblCurrHPMV
		'
		Me.lblCurrHPMV.AutoSize = true
		Me.lblCurrHPMV.Font = New System.Drawing.Font("Meiryo UI", 12!)
		Me.lblCurrHPMV.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.lblCurrHPMV.Location = New System.Drawing.Point(11, 127)
		Me.lblCurrHPMV.Name = "lblCurrHPMV"
		Me.lblCurrHPMV.Size = New System.Drawing.Size(96, 20)
		Me.lblCurrHPMV.TabIndex = 55
		Me.lblCurrHPMV.Text = "HP MV (%)"
		'
		'lblCurrHPTemp
		'
		Me.lblCurrHPTemp.AutoSize = true
		Me.lblCurrHPTemp.Font = New System.Drawing.Font("Meiryo UI", 12!)
		Me.lblCurrHPTemp.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.lblCurrHPTemp.Location = New System.Drawing.Point(11, 63)
		Me.lblCurrHPTemp.Name = "lblCurrHPTemp"
		Me.lblCurrHPTemp.Size = New System.Drawing.Size(148, 20)
		Me.lblCurrHPTemp.TabIndex = 55
		Me.lblCurrHPTemp.Text = "HP Temp. (deg C)"
		'
		'lblElapsedTime
		'
		Me.lblElapsedTime.AutoSize = true
		Me.lblElapsedTime.Font = New System.Drawing.Font("Meiryo UI", 12!)
		Me.lblElapsedTime.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.lblElapsedTime.Location = New System.Drawing.Point(12, -1)
		Me.lblElapsedTime.Name = "lblElapsedTime"
		Me.lblElapsedTime.Size = New System.Drawing.Size(115, 20)
		Me.lblElapsedTime.TabIndex = 55
		Me.lblElapsedTime.Text = "Elapsed (Sec)"
		'
		'lblCurrHPMVVal
		'
		Me.lblCurrHPMVVal.BackColor = System.Drawing.Color.White
		Me.lblCurrHPMVVal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		Me.lblCurrHPMVVal.Font = New System.Drawing.Font("MS UI Gothic", 24!)
		Me.lblCurrHPMVVal.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.lblCurrHPMVVal.Location = New System.Drawing.Point(10, 148)
		Me.lblCurrHPMVVal.Name = "lblCurrHPMVVal"
		Me.lblCurrHPMVVal.Size = New System.Drawing.Size(145, 42)
		Me.lblCurrHPMVVal.TabIndex = 52
		Me.lblCurrHPMVVal.Text = "000.0"
		Me.lblCurrHPMVVal.TextAlign = System.Drawing.ContentAlignment.MiddleRight
		'
		'lblCurrHPTempVal
		'
		Me.lblCurrHPTempVal.BackColor = System.Drawing.Color.White
		Me.lblCurrHPTempVal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		Me.lblCurrHPTempVal.Font = New System.Drawing.Font("MS UI Gothic", 24!)
		Me.lblCurrHPTempVal.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.lblCurrHPTempVal.Location = New System.Drawing.Point(10, 84)
		Me.lblCurrHPTempVal.Name = "lblCurrHPTempVal"
		Me.lblCurrHPTempVal.Size = New System.Drawing.Size(145, 42)
		Me.lblCurrHPTempVal.TabIndex = 52
		Me.lblCurrHPTempVal.Text = "000.0"
		Me.lblCurrHPTempVal.TextAlign = System.Drawing.ContentAlignment.MiddleRight
		'
		'lblResult1Val
		'
		Me.lblResult1Val.BackColor = System.Drawing.Color.White
		Me.lblResult1Val.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		Me.lblResult1Val.Font = New System.Drawing.Font("MS UI Gothic", 24!)
		Me.lblResult1Val.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.lblResult1Val.Location = New System.Drawing.Point(1047, 80)
		Me.lblResult1Val.Name = "lblResult1Val"
		Me.lblResult1Val.Size = New System.Drawing.Size(145, 42)
		Me.lblResult1Val.TabIndex = 73
		Me.lblResult1Val.Text = "0.00"
		Me.lblResult1Val.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.lblResult1Val.Visible = false
		'
		'lblResult1
		'
		Me.lblResult1.AutoSize = true
		Me.lblResult1.BackColor = System.Drawing.Color.Transparent
		Me.lblResult1.Font = New System.Drawing.Font("Meiryo UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.lblResult1.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.lblResult1.Location = New System.Drawing.Point(1050, 64)
		Me.lblResult1.Name = "lblResult1"
		Me.lblResult1.Size = New System.Drawing.Size(102, 17)
		Me.lblResult1.TabIndex = 75
		Me.lblResult1.Text = "Gel Time (Sec)"
		Me.lblResult1.Visible = false
		'
		'lblResult3Val
		'
		Me.lblResult3Val.BackColor = System.Drawing.Color.White
		Me.lblResult3Val.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		Me.lblResult3Val.Font = New System.Drawing.Font("MS UI Gothic", 24!)
		Me.lblResult3Val.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.lblResult3Val.Location = New System.Drawing.Point(894, 80)
		Me.lblResult3Val.Name = "lblResult3Val"
		Me.lblResult3Val.Size = New System.Drawing.Size(145, 42)
		Me.lblResult3Val.TabIndex = 74
		Me.lblResult3Val.Text = "0.00"
		Me.lblResult3Val.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.lblResult3Val.Visible = false
		'
		'lblResult2Val
		'
		Me.lblResult2Val.BackColor = System.Drawing.Color.White
		Me.lblResult2Val.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		Me.lblResult2Val.Font = New System.Drawing.Font("MS UI Gothic", 24!)
		Me.lblResult2Val.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.lblResult2Val.Location = New System.Drawing.Point(740, 80)
		Me.lblResult2Val.Name = "lblResult2Val"
		Me.lblResult2Val.Size = New System.Drawing.Size(145, 42)
		Me.lblResult2Val.TabIndex = 74
		Me.lblResult2Val.Text = "0.00"
		Me.lblResult2Val.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.lblResult2Val.Visible = false
		'
		'lblResult3
		'
		Me.lblResult3.AutoSize = true
		Me.lblResult3.BackColor = System.Drawing.Color.Transparent
		Me.lblResult3.Font = New System.Drawing.Font("Meiryo UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.lblResult3.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.lblResult3.Location = New System.Drawing.Point(897, 64)
		Me.lblResult3.Name = "lblResult3"
		Me.lblResult3.Size = New System.Drawing.Size(143, 17)
		Me.lblResult3.TabIndex = 76
		Me.lblResult3.Text = "Temperature (deg C)"
		Me.lblResult3.Visible = false
		'
		'lblResult2
		'
		Me.lblResult2.AutoSize = true
		Me.lblResult2.BackColor = System.Drawing.Color.Transparent
		Me.lblResult2.Font = New System.Drawing.Font("Meiryo UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.lblResult2.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.lblResult2.Location = New System.Drawing.Point(743, 64)
		Me.lblResult2.Name = "lblResult2"
		Me.lblResult2.Size = New System.Drawing.Size(143, 17)
		Me.lblResult2.TabIndex = 76
		Me.lblResult2.Text = "Temperature (deg C)"
		Me.lblResult2.Visible = false
		'
		'Chart1
		'
		Me.Chart1.AllowDrop = true
		Me.Chart1.BorderlineColor = System.Drawing.Color.Black
		ChartArea1.AxisX.LabelStyle.Interval = 0R
		ChartArea1.Name = "ChartArea1"
		Me.Chart1.ChartAreas.Add(ChartArea1)
		Me.Chart1.Location = New System.Drawing.Point(-2, 65)
		Me.Chart1.Name = "Chart1"
		Me.Chart1.Size = New System.Drawing.Size(1206, 856)
		Me.Chart1.TabIndex = 55
		Me.Chart1.Text = "Chart1"
		Title1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
		Title1.Name = "Title1"
		Title1.Text = "Real time measure trend"
		Me.Chart1.Titles.Add(Title1)
		'
		'gbxHistgram
		'
		Me.gbxHistgram.BackColor = System.Drawing.Color.White
		Me.gbxHistgram.Controls.Add(Me.bynHistClose)
		Me.gbxHistgram.Controls.Add(Me.pnlHistOpe)
		Me.gbxHistgram.Controls.Add(Me.Chart3)
		Me.gbxHistgram.Font = New System.Drawing.Font("Meiryo UI", 9!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.gbxHistgram.Location = New System.Drawing.Point(1398, 441)
		Me.gbxHistgram.Name = "gbxHistgram"
		Me.gbxHistgram.Size = New System.Drawing.Size(327, 193)
		Me.gbxHistgram.TabIndex = 91
		Me.gbxHistgram.TabStop = false
		Me.gbxHistgram.Text = "GroupBox1"
		Me.gbxHistgram.Visible = false
		'
		'bynHistClose
		'
		Me.bynHistClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
		Me.bynHistClose.Location = New System.Drawing.Point(244, 164)
		Me.bynHistClose.Name = "bynHistClose"
		Me.bynHistClose.Size = New System.Drawing.Size(75, 23)
		Me.bynHistClose.TabIndex = 93
		Me.bynHistClose.Text = "Close"
		Me.bynHistClose.UseVisualStyleBackColor = true
		'
		'pnlHistOpe
		'
		Me.pnlHistOpe.Controls.Add(Me.lvStat)
		Me.pnlHistOpe.Controls.Add(Me.lblSecWidth)
		Me.pnlHistOpe.Controls.Add(Me.nudSection)
		Me.pnlHistOpe.Location = New System.Drawing.Point(400, 61)
		Me.pnlHistOpe.Name = "pnlHistOpe"
		Me.pnlHistOpe.Size = New System.Drawing.Size(139, 185)
		Me.pnlHistOpe.TabIndex = 92
		'
		'lvStat
		'
		Me.lvStat.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lvStat.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1, Me.ColumnHeader2})
		Me.lvStat.Dock = System.Windows.Forms.DockStyle.Bottom
		Me.lvStat.GridLines = true
		Me.lvStat.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None
		Me.lvStat.HideSelection = false
		Me.lvStat.Items.AddRange(New System.Windows.Forms.ListViewItem() {ListViewItem1, ListViewItem2})
		Me.lvStat.Location = New System.Drawing.Point(0, 32)
		Me.lvStat.MultiSelect = false
		Me.lvStat.Name = "lvStat"
		Me.lvStat.Scrollable = false
		Me.lvStat.Size = New System.Drawing.Size(139, 153)
		Me.lvStat.TabIndex = 93
		Me.lvStat.UseCompatibleStateImageBehavior = false
		Me.lvStat.View = System.Windows.Forms.View.Details
		'
		'lblSecWidth
		'
		Me.lblSecWidth.AutoSize = true
		Me.lblSecWidth.Font = New System.Drawing.Font("Meiryo UI", 9!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.lblSecWidth.Location = New System.Drawing.Point(3, 6)
		Me.lblSecWidth.Name = "lblSecWidth"
		Me.lblSecWidth.Size = New System.Drawing.Size(64, 15)
		Me.lblSecWidth.TabIndex = 92
		Me.lblSecWidth.Text = "Sec.width"
		'
		'nudSection
		'
		Me.nudSection.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
		Me.nudSection.DecimalPlaces = 1
		Me.nudSection.Font = New System.Drawing.Font("Meiryo UI", 9!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.nudSection.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
		Me.nudSection.Location = New System.Drawing.Point(73, 3)
		Me.nudSection.Maximum = New Decimal(New Integer() {50, 0, 0, 0})
		Me.nudSection.Minimum = New Decimal(New Integer() {1, 0, 0, 65536})
		Me.nudSection.Name = "nudSection"
		Me.nudSection.Size = New System.Drawing.Size(63, 23)
		Me.nudSection.TabIndex = 91
		Me.nudSection.Value = New Decimal(New Integer() {10, 0, 0, 0})
		'
		'Chart3
		'
		ChartArea2.Name = "ChartArea1"
		Me.Chart3.ChartAreas.Add(ChartArea2)
		Me.Chart3.Dock = System.Windows.Forms.DockStyle.Fill
		Legend1.Name = "Legend1"
		Me.Chart3.Legends.Add(Legend1)
		Me.Chart3.Location = New System.Drawing.Point(3, 19)
		Me.Chart3.Name = "Chart3"
		Series1.ChartArea = "ChartArea1"
		Series1.Legend = "Legend1"
		Series1.Name = "Series1"
		Me.Chart3.Series.Add(Series1)
		Me.Chart3.Size = New System.Drawing.Size(321, 171)
		Me.Chart3.TabIndex = 90
		Me.Chart3.Text = "Chart3"
		Me.Chart3.Visible = false
		'
		'gbxExpoPad
		'
		Me.gbxExpoPad.BackColor = System.Drawing.SystemColors.Info
		Me.gbxExpoPad.Controls.Add(Me.btnExtrapolationSave)
		Me.gbxExpoPad.Controls.Add(Me.cboGelTSel)
		Me.gbxExpoPad.Controls.Add(Me.lblMiddleLine)
		Me.gbxExpoPad.Controls.Add(Me.lblOverBaseLine)
		Me.gbxExpoPad.Controls.Add(Me.lblMidA)
		Me.gbxExpoPad.Controls.Add(Me.lblOverBaseA)
		Me.gbxExpoPad.Controls.Add(Me.lblUnderBaseA)
		Me.gbxExpoPad.Controls.Add(Me.lblUnderBaseLine)
		Me.gbxExpoPad.Controls.Add(Me.lblGelTimeSelection)
		Me.gbxExpoPad.Controls.Add(Me.lblThinningOut)
		Me.gbxExpoPad.Controls.Add(Me.lblExpoSample)
		Me.gbxExpoPad.Controls.Add(Me.nudEndTim)
		Me.gbxExpoPad.Controls.Add(Me.nudThinning)
		Me.gbxExpoPad.Controls.Add(Me.nudExpoSample)
		Me.gbxExpoPad.Controls.Add(Me.nudStartTim)
		Me.gbxExpoPad.Controls.Add(Me.nudMidTim)
		Me.gbxExpoPad.Location = New System.Drawing.Point(1580, 34)
		Me.gbxExpoPad.Name = "gbxExpoPad"
		Me.gbxExpoPad.Size = New System.Drawing.Size(329, 272)
		Me.gbxExpoPad.TabIndex = 60
		Me.gbxExpoPad.TabStop = false
		Me.gbxExpoPad.Visible = false
		'
		'btnExtrapolationSave
		'
		Me.btnExtrapolationSave.Font = New System.Drawing.Font("Meiryo UI", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.btnExtrapolationSave.Location = New System.Drawing.Point(6, 272)
		Me.btnExtrapolationSave.Name = "btnExtrapolationSave"
		Me.btnExtrapolationSave.Size = New System.Drawing.Size(312, 32)
		Me.btnExtrapolationSave.TabIndex = 62
		Me.btnExtrapolationSave.Text = "&Save"
		Me.btnExtrapolationSave.UseVisualStyleBackColor = true
		Me.btnExtrapolationSave.Visible = false
		'
		'cboGelTSel
		'
		Me.cboGelTSel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.cboGelTSel.FlatStyle = System.Windows.Forms.FlatStyle.Popup
		Me.cboGelTSel.Font = New System.Drawing.Font("Meiryo UI", 12!)
		Me.cboGelTSel.FormattingEnabled = true
		Me.cboGelTSel.Location = New System.Drawing.Point(216, 129)
		Me.cboGelTSel.Name = "cboGelTSel"
		Me.cboGelTSel.Size = New System.Drawing.Size(100, 28)
		Me.cboGelTSel.TabIndex = 61
		'
		'lblMiddleLine
		'
		Me.lblMiddleLine.AutoSize = true
		Me.lblMiddleLine.Font = New System.Drawing.Font("Meiryo UI", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.lblMiddleLine.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.lblMiddleLine.Location = New System.Drawing.Point(9, 233)
		Me.lblMiddleLine.Name = "lblMiddleLine"
		Me.lblMiddleLine.Size = New System.Drawing.Size(91, 20)
		Me.lblMiddleLine.TabIndex = 60
		Me.lblMiddleLine.Text = "Middle line"
		'
		'lblOverBaseLine
		'
		Me.lblOverBaseLine.AutoSize = true
		Me.lblOverBaseLine.Font = New System.Drawing.Font("Meiryo UI", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.lblOverBaseLine.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.lblOverBaseLine.Location = New System.Drawing.Point(9, 201)
		Me.lblOverBaseLine.Name = "lblOverBaseLine"
		Me.lblOverBaseLine.Size = New System.Drawing.Size(119, 20)
		Me.lblOverBaseLine.TabIndex = 60
		Me.lblOverBaseLine.Text = "Over base line"
		'
		'lblMidA
		'
		Me.lblMidA.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		Me.lblMidA.Font = New System.Drawing.Font("Meiryo UI", 12!)
		Me.lblMidA.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.lblMidA.Location = New System.Drawing.Point(216, 231)
		Me.lblMidA.Name = "lblMidA"
		Me.lblMidA.Size = New System.Drawing.Size(103, 24)
		Me.lblMidA.TabIndex = 60
		Me.lblMidA.Text = "------"
		Me.lblMidA.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		'
		'lblOverBaseA
		'
		Me.lblOverBaseA.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		Me.lblOverBaseA.Font = New System.Drawing.Font("Meiryo UI", 12!)
		Me.lblOverBaseA.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.lblOverBaseA.Location = New System.Drawing.Point(216, 199)
		Me.lblOverBaseA.Name = "lblOverBaseA"
		Me.lblOverBaseA.Size = New System.Drawing.Size(103, 24)
		Me.lblOverBaseA.TabIndex = 60
		Me.lblOverBaseA.Text = "------"
		Me.lblOverBaseA.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		'
		'lblUnderBaseA
		'
		Me.lblUnderBaseA.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		Me.lblUnderBaseA.Font = New System.Drawing.Font("Meiryo UI", 12!)
		Me.lblUnderBaseA.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.lblUnderBaseA.Location = New System.Drawing.Point(216, 166)
		Me.lblUnderBaseA.Name = "lblUnderBaseA"
		Me.lblUnderBaseA.Size = New System.Drawing.Size(103, 24)
		Me.lblUnderBaseA.TabIndex = 60
		Me.lblUnderBaseA.Text = "------"
		Me.lblUnderBaseA.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		'
		'lblUnderBaseLine
		'
		Me.lblUnderBaseLine.AutoSize = true
		Me.lblUnderBaseLine.Font = New System.Drawing.Font("Meiryo UI", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.lblUnderBaseLine.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.lblUnderBaseLine.Location = New System.Drawing.Point(9, 168)
		Me.lblUnderBaseLine.Name = "lblUnderBaseLine"
		Me.lblUnderBaseLine.Size = New System.Drawing.Size(130, 20)
		Me.lblUnderBaseLine.TabIndex = 60
		Me.lblUnderBaseLine.Text = "Under base line"
		'
		'lblGelTimeSelection
		'
		Me.lblGelTimeSelection.AutoSize = true
		Me.lblGelTimeSelection.Font = New System.Drawing.Font("Meiryo UI", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.lblGelTimeSelection.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.lblGelTimeSelection.Location = New System.Drawing.Point(9, 132)
		Me.lblGelTimeSelection.Name = "lblGelTimeSelection"
		Me.lblGelTimeSelection.Size = New System.Drawing.Size(146, 20)
		Me.lblGelTimeSelection.TabIndex = 60
		Me.lblGelTimeSelection.Text = "Gel time selection"
		'
		'lblThinningOut
		'
		Me.lblThinningOut.AutoSize = true
		Me.lblThinningOut.Font = New System.Drawing.Font("Meiryo UI", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.lblThinningOut.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.lblThinningOut.Location = New System.Drawing.Point(9, 95)
		Me.lblThinningOut.Name = "lblThinningOut"
		Me.lblThinningOut.Size = New System.Drawing.Size(108, 20)
		Me.lblThinningOut.TabIndex = 60
		Me.lblThinningOut.Text = "Thinning out"
		'
		'lblExpoSample
		'
		Me.lblExpoSample.AutoSize = true
		Me.lblExpoSample.Font = New System.Drawing.Font("Meiryo UI", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.lblExpoSample.Location = New System.Drawing.Point(9, 62)
		Me.lblExpoSample.Name = "lblExpoSample"
		Me.lblExpoSample.Size = New System.Drawing.Size(173, 20)
		Me.lblExpoSample.TabIndex = 59
		Me.lblExpoSample.Text = "Extrapolation sample"
		'
		'nudEndTim
		'
		Me.nudEndTim.DecimalPlaces = 3
		Me.nudEndTim.Font = New System.Drawing.Font("Meiryo UI", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.nudEndTim.Location = New System.Drawing.Point(216, 25)
		Me.nudEndTim.Maximum = New Decimal(New Integer() {1000, 0, 0, 0})
		Me.nudEndTim.Name = "nudEndTim"
		Me.nudEndTim.Size = New System.Drawing.Size(102, 28)
		Me.nudEndTim.TabIndex = 56
		Me.nudEndTim.Value = New Decimal(New Integer() {250, 0, 0, 0})
		'
		'nudThinning
		'
		Me.nudThinning.Font = New System.Drawing.Font("Meiryo UI", 12!)
		Me.nudThinning.Location = New System.Drawing.Point(216, 93)
		Me.nudThinning.Maximum = New Decimal(New Integer() {1000, 0, 0, 0})
		Me.nudThinning.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
		Me.nudThinning.Name = "nudThinning"
		Me.nudThinning.Size = New System.Drawing.Size(58, 28)
		Me.nudThinning.TabIndex = 58
		Me.nudThinning.Value = New Decimal(New Integer() {10, 0, 0, 0})
		'
		'nudExpoSample
		'
		Me.nudExpoSample.Font = New System.Drawing.Font("Meiryo UI", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.nudExpoSample.Location = New System.Drawing.Point(216, 57)
		Me.nudExpoSample.Maximum = New Decimal(New Integer() {10000, 0, 0, 0})
		Me.nudExpoSample.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
		Me.nudExpoSample.Name = "nudExpoSample"
		Me.nudExpoSample.Size = New System.Drawing.Size(58, 28)
		Me.nudExpoSample.TabIndex = 58
		Me.nudExpoSample.Value = New Decimal(New Integer() {500, 0, 0, 0})
		'
		'nudStartTim
		'
		Me.nudStartTim.DecimalPlaces = 3
		Me.nudStartTim.Font = New System.Drawing.Font("Meiryo UI", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.nudStartTim.Location = New System.Drawing.Point(6, 25)
		Me.nudStartTim.Maximum = New Decimal(New Integer() {1000, 0, 0, 0})
		Me.nudStartTim.Name = "nudStartTim"
		Me.nudStartTim.Size = New System.Drawing.Size(102, 28)
		Me.nudStartTim.TabIndex = 58
		Me.nudStartTim.Value = New Decimal(New Integer() {30, 0, 0, 0})
		'
		'nudMidTim
		'
		Me.nudMidTim.DecimalPlaces = 3
		Me.nudMidTim.Font = New System.Drawing.Font("Meiryo UI", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.nudMidTim.Location = New System.Drawing.Point(111, 25)
		Me.nudMidTim.Maximum = New Decimal(New Integer() {1000, 0, 0, 0})
		Me.nudMidTim.Name = "nudMidTim"
		Me.nudMidTim.Size = New System.Drawing.Size(102, 28)
		Me.nudMidTim.TabIndex = 57
		Me.nudMidTim.Value = New Decimal(New Integer() {100, 0, 0, 0})
		'
		'btnDigChange
		'
		Me.btnDigChange.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
		Me.btnDigChange.Font = New System.Drawing.Font("Meiryo UI", 12!)
		Me.btnDigChange.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.btnDigChange.Location = New System.Drawing.Point(1218, 970)
		Me.btnDigChange.Name = "btnDigChange"
		Me.btnDigChange.Size = New System.Drawing.Size(153, 35)
		Me.btnDigChange.TabIndex = 81
		Me.btnDigChange.Text = "Select digit"
		Me.btnDigChange.UseVisualStyleBackColor = true
		'
		'bgwInterrupt
		'
		'
		'bgwTrendUpdate
		'
		'
		'PrintDocument1
		'
		'
		'cmsDragDrop
		'
		Me.cmsDragDrop.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsmiReal, Me.tsmiStandard1, Me.tsmiStandard2, Me.tsmiSepSlope, Me.tsmiSLOPERecalc, Me.ToolStripSeparator11, Me.tsmiCansel})
		Me.cmsDragDrop.Name = "cmsDragDrop"
		Me.cmsDragDrop.Size = New System.Drawing.Size(179, 126)
		'
		'tsmiReal
		'
		Me.tsmiReal.Name = "tsmiReal"
		Me.tsmiReal.Size = New System.Drawing.Size(178, 22)
		Me.tsmiReal.Text = "&Real data area"
		'
		'tsmiStandard1
		'
		Me.tsmiStandard1.Name = "tsmiStandard1"
		Me.tsmiStandard1.Size = New System.Drawing.Size(178, 22)
		Me.tsmiStandard1.Text = "Standard data area&1"
		'
		'tsmiStandard2
		'
		Me.tsmiStandard2.Name = "tsmiStandard2"
		Me.tsmiStandard2.Size = New System.Drawing.Size(178, 22)
		Me.tsmiStandard2.Text = "Standard data area&2"
		'
		'tsmiSepSlope
		'
		Me.tsmiSepSlope.Name = "tsmiSepSlope"
		Me.tsmiSepSlope.Size = New System.Drawing.Size(175, 6)
		Me.tsmiSepSlope.Visible = false
		'
		'tsmiSLOPERecalc
		'
		Me.tsmiSLOPERecalc.Name = "tsmiSLOPERecalc"
		Me.tsmiSLOPERecalc.Size = New System.Drawing.Size(178, 22)
		Me.tsmiSLOPERecalc.Text = "S&LOPE Recalc."
		Me.tsmiSLOPERecalc.Visible = false
		'
		'ToolStripSeparator11
		'
		Me.ToolStripSeparator11.Name = "ToolStripSeparator11"
		Me.ToolStripSeparator11.Size = New System.Drawing.Size(175, 6)
		'
		'tsmiCansel
		'
		Me.tsmiCansel.Name = "tsmiCansel"
		Me.tsmiCansel.Size = New System.Drawing.Size(178, 22)
		Me.tsmiCansel.Text = "&Cansel"
		'
		'cmsSpecSelect
		'
		Me.cmsSpecSelect.Font = New System.Drawing.Font("Meiryo UI", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.cmsSpecSelect.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItem2, Me.ToolStripMenuItem3})
		Me.cmsSpecSelect.Name = "cmsSpecSelect"
		Me.cmsSpecSelect.Size = New System.Drawing.Size(236, 52)
		'
		'ToolStripMenuItem2
		'
		Me.ToolStripMenuItem2.Checked = true
		Me.ToolStripMenuItem2.CheckOnClick = true
		Me.ToolStripMenuItem2.CheckState = System.Windows.Forms.CheckState.Checked
		Me.ToolStripMenuItem2.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItem4})
		Me.ToolStripMenuItem2.Name = "ToolStripMenuItem2"
		Me.ToolStripMenuItem2.Size = New System.Drawing.Size(235, 24)
		Me.ToolStripMenuItem2.Text = "ToolStripMenuItem2"
		'
		'ToolStripMenuItem4
		'
		Me.ToolStripMenuItem4.Name = "ToolStripMenuItem4"
		Me.ToolStripMenuItem4.Size = New System.Drawing.Size(235, 24)
		Me.ToolStripMenuItem4.Text = "ToolStripMenuItem4"
		'
		'ToolStripMenuItem3
		'
		Me.ToolStripMenuItem3.Name = "ToolStripMenuItem3"
		Me.ToolStripMenuItem3.Size = New System.Drawing.Size(235, 24)
		Me.ToolStripMenuItem3.Text = "ToolStripMenuItem3"
		'
		'cmsHistHead
		'
		Me.cmsHistHead.Font = New System.Drawing.Font("Meiryo UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.cmsHistHead.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsmiHistDel, Me.ToolStripSeparator15, Me.tsmiHistCut, Me.tsmiHistPaste, Me.ToolStripSeparator16, Me.tsmiHistUndo})
		Me.cmsHistHead.Name = "cmsHistHead"
		Me.cmsHistHead.Size = New System.Drawing.Size(119, 104)
		'
		'tsmiHistDel
		'
		Me.tsmiHistDel.Name = "tsmiHistDel"
		Me.tsmiHistDel.Size = New System.Drawing.Size(118, 22)
		Me.tsmiHistDel.Text = "&Delete"
		'
		'ToolStripSeparator15
		'
		Me.ToolStripSeparator15.Name = "ToolStripSeparator15"
		Me.ToolStripSeparator15.Size = New System.Drawing.Size(115, 6)
		'
		'tsmiHistCut
		'
		Me.tsmiHistCut.Name = "tsmiHistCut"
		Me.tsmiHistCut.Size = New System.Drawing.Size(118, 22)
		Me.tsmiHistCut.Text = "&Cut"
		'
		'tsmiHistPaste
		'
		Me.tsmiHistPaste.Name = "tsmiHistPaste"
		Me.tsmiHistPaste.Size = New System.Drawing.Size(118, 22)
		Me.tsmiHistPaste.Text = "&Paste"
		'
		'ToolStripSeparator16
		'
		Me.ToolStripSeparator16.Name = "ToolStripSeparator16"
		Me.ToolStripSeparator16.Size = New System.Drawing.Size(115, 6)
		'
		'tsmiHistUndo
		'
		Me.tsmiHistUndo.Name = "tsmiHistUndo"
		Me.tsmiHistUndo.Size = New System.Drawing.Size(118, 22)
		Me.tsmiHistUndo.Text = "&Undo"
		'
		'lblStrLen
		'
		Me.lblStrLen.AutoSize = true
		Me.lblStrLen.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.lblStrLen.Location = New System.Drawing.Point(0, 0)
		Me.lblStrLen.Name = "lblStrLen"
		Me.lblStrLen.Size = New System.Drawing.Size(50, 12)
		Me.lblStrLen.TabIndex = 82
		Me.lblStrLen.Text = "lblStrLen"
		Me.lblStrLen.Visible = false
		'
		'lblInterlockVal
		'
		Me.lblInterlockVal.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
		Me.lblInterlockVal.BackColor = System.Drawing.Color.LightGray
		Me.lblInterlockVal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		Me.lblInterlockVal.Font = New System.Drawing.Font("Meiryo UI", 14.25!, System.Drawing.FontStyle.Bold)
		Me.lblInterlockVal.ForeColor = System.Drawing.Color.White
		Me.lblInterlockVal.ImageKey = "(なし)"
		Me.lblInterlockVal.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.lblInterlockVal.Location = New System.Drawing.Point(1383, 2)
		Me.lblInterlockVal.Name = "lblInterlockVal"
		Me.lblInterlockVal.Size = New System.Drawing.Size(126, 28)
		Me.lblInterlockVal.TabIndex = 83
		Me.lblInterlockVal.Text = "OPEN"
		Me.lblInterlockVal.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		'
		'Label5
		'
		Me.Label5.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
		Me.Label5.BackColor = System.Drawing.Color.Transparent
		Me.Label5.Font = New System.Drawing.Font("Meiryo UI", 12!)
		Me.Label5.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me.Label5.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.Label5.Location = New System.Drawing.Point(1297, 4)
		Me.Label5.Name = "Label5"
		Me.Label5.Size = New System.Drawing.Size(84, 24)
		Me.Label5.TabIndex = 84
		Me.Label5.Text = "Interlock"
		Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
		'
		'lblAdminEna
		'
		Me.lblAdminEna.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
		Me.lblAdminEna.FlatStyle = System.Windows.Forms.FlatStyle.Flat
		Me.lblAdminEna.ImageIndex = 0
		Me.lblAdminEna.ImageList = Me.ilState
		Me.lblAdminEna.Location = New System.Drawing.Point(1512, 5)
		Me.lblAdminEna.Margin = New System.Windows.Forms.Padding(0)
		Me.lblAdminEna.Name = "lblAdminEna"
		Me.lblAdminEna.Size = New System.Drawing.Size(24, 24)
		Me.lblAdminEna.TabIndex = 85
		'
		'ilState
		'
		Me.ilState.ImageStream = CType(resources.GetObject("ilState.ImageStream"),System.Windows.Forms.ImageListStreamer)
		Me.ilState.TransparentColor = System.Drawing.Color.Transparent
		Me.ilState.Images.SetKeyName(0, "Lock")
		Me.ilState.Images.SetKeyName(1, "UnLock")
		'
		'lblLTMMode
		'
		Me.lblLTMMode.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom)  _
            Or System.Windows.Forms.AnchorStyles.Left)  _
            Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
		Me.lblLTMMode.AutoSize = true
		Me.lblLTMMode.BackColor = System.Drawing.Color.PaleGreen
		Me.lblLTMMode.Font = New System.Drawing.Font("Meiryo UI", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.lblLTMMode.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me.lblLTMMode.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.lblLTMMode.Location = New System.Drawing.Point(566, 5)
		Me.lblLTMMode.Name = "lblLTMMode"
		Me.lblLTMMode.Size = New System.Drawing.Size(347, 24)
		Me.lblLTMMode.TabIndex = 84
		Me.lblLTMMode.Text = "Intermittent measurement mode"&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10)
		Me.lblLTMMode.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me.lblLTMMode.Visible = false
		'
		'btnQRcode
		'
		Me.btnQRcode.Dock = System.Windows.Forms.DockStyle.Fill
		Me.btnQRcode.Font = New System.Drawing.Font("Meiryo UI", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.btnQRcode.Location = New System.Drawing.Point(0, 0)
		Me.btnQRcode.Name = "btnQRcode"
		Me.btnQRcode.Size = New System.Drawing.Size(509, 35)
		Me.btnQRcode.TabIndex = 92
		Me.btnQRcode.Text = "QR code now available..."
		Me.btnQRcode.UseVisualStyleBackColor = true
		'
		'txtQR
		'
		Me.txtQR.Dock = System.Windows.Forms.DockStyle.Bottom
		Me.txtQR.Font = New System.Drawing.Font("Meiryo UI", 9!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.txtQR.Location = New System.Drawing.Point(0, 35)
		Me.txtQR.Name = "txtQR"
		Me.txtQR.Size = New System.Drawing.Size(509, 23)
		Me.txtQR.TabIndex = 93
		Me.txtQR.Visible = false
		'
		'pnlQR
		'
		Me.pnlQR.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
		Me.pnlQR.Controls.Add(Me.btnQRcode)
		Me.pnlQR.Controls.Add(Me.txtQR)
		Me.pnlQR.Location = New System.Drawing.Point(1383, 955)
		Me.pnlQR.Name = "pnlQR"
		Me.pnlQR.Size = New System.Drawing.Size(509, 58)
		Me.pnlQR.TabIndex = 94
		'
		'QRSimuTimer
		'
		Me.QRSimuTimer.Interval = 5
		'
		'frmMain
		'
		Me.AllowDrop = true
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6!, 12!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(1904, 1041)
		Me.Controls.Add(Me.pnlQR)
		Me.Controls.Add(Me.gbxHistgram)
		Me.Controls.Add(Me.lblAdminEna)
		Me.Controls.Add(Me.lblInterlockVal)
		Me.Controls.Add(Me.lblLTMMode)
		Me.Controls.Add(Me.Label5)
		Me.Controls.Add(Me.lblStrLen)
		Me.Controls.Add(Me.gbxExpoPad)
		Me.Controls.Add(Me.pnlSPEC)
		Me.Controls.Add(Me.lblCpuVer)
		Me.Controls.Add(Me.cboEquipName)
		Me.Controls.Add(Me.lblMachineVer)
		Me.Controls.Add(Me.lblDevAlive)
		Me.Controls.Add(Me.lblEquipment)
		Me.Controls.Add(Me.Lbl2)
		Me.Controls.Add(Me.Lbl3)
		Me.Controls.Add(Me.btnDigChange)
		Me.Controls.Add(Me.pnlRealG)
		Me.Controls.Add(Me.btnAllClear)
		Me.Controls.Add(Me.pctConStts)
		Me.Controls.Add(Me.btnOrigin)
		Me.Controls.Add(Me.btnErrReset)
		Me.Controls.Add(Me.lblConnectState)
		Me.Controls.Add(Me.btnMeasure)
		Me.Controls.Add(Me.MenuStrip1)
		Me.Controls.Add(Me.StatusStrip1)
		Me.Controls.Add(Me.btnTemp)
		Me.Controls.Add(Me.btnZero)
		Me.KeyPreview = true
		Me.Name = "frmMain"
		Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
		Me.Text = "Sizuka tool"
		Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
		Me.GroupBox0.ResumeLayout(false)
		CType(Me.nudYAxisZoom,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.nudYscaleMax,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.nudYscaleMin,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.nudXscaleMax,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.nudXscaleMin,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.nudXdivision,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.nudYdivision,System.ComponentModel.ISupportInitialize).EndInit
		Me.StatusStrip1.ResumeLayout(false)
		Me.StatusStrip1.PerformLayout
		Me.MenuStrip1.ResumeLayout(false)
		Me.MenuStrip1.PerformLayout
		CType(Me.pctConStts,System.ComponentModel.ISupportInitialize).EndInit
		Me.pnlSPEC.ResumeLayout(false)
		Me.pnlSPEC.PerformLayout
		CType(Me.dgvSamSpec,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.dgvDevSpec,System.ComponentModel.ISupportInitialize).EndInit
		Me.Panel1.ResumeLayout(false)
		Me.Panel1.PerformLayout
		Me.pnlRealG.ResumeLayout(false)
		Me.pnlRealG.PerformLayout
		Me.pnlThinning.ResumeLayout(false)
		Me.pnlThinning.PerformLayout
		Me.gpxScrol.ResumeLayout(false)
		Me.gpxScrol.PerformLayout
		CType(Me.nudXSpan,System.ComponentModel.ISupportInitialize).EndInit
		Me.gpxAxisSetting.ResumeLayout(false)
		Me.gpxAxisSetting.PerformLayout
		Me.gbxXscale.ResumeLayout(false)
		Me.gbxXscale.PerformLayout
		Me.gbxYscale.ResumeLayout(false)
		Me.gbxYscale.PerformLayout
		Me.gpxThinning.ResumeLayout(false)
		Me.gpxThinning.PerformLayout
		CType(Me.nudDispThinningoutRough,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.nudDispThinningoutDetail,System.ComponentModel.ISupportInitialize).EndInit
		Me.gpxXType.ResumeLayout(false)
		Me.gpxXType.PerformLayout
		Me.Panel2.ResumeLayout(false)
		CType(Me.Chart2,System.ComponentModel.ISupportInitialize).EndInit
		Me.pnlSteress.ResumeLayout(false)
		Me.pnlSteress.PerformLayout
		Me.gbxStdCond.ResumeLayout(false)
		Me.gbxStdCond.PerformLayout
		CType(Me.dgvStd,System.ComponentModel.ISupportInitialize).EndInit
		Me.pnlCalculation.ResumeLayout(false)
		Me.pnlCalculation.PerformLayout
		Me.gbxCurrentVal.ResumeLayout(false)
		Me.gbxCurrentVal.PerformLayout
		Me.pnlPosition.ResumeLayout(false)
		Me.pnlPosition.PerformLayout
		CType(Me.Chart1,System.ComponentModel.ISupportInitialize).EndInit
		Me.gbxHistgram.ResumeLayout(false)
		Me.pnlHistOpe.ResumeLayout(false)
		Me.pnlHistOpe.PerformLayout
		CType(Me.nudSection,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.Chart3,System.ComponentModel.ISupportInitialize).EndInit
		Me.gbxExpoPad.ResumeLayout(false)
		Me.gbxExpoPad.PerformLayout
		CType(Me.nudEndTim,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.nudThinning,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.nudExpoSample,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.nudStartTim,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.nudMidTim,System.ComponentModel.ISupportInitialize).EndInit
		Me.cmsDragDrop.ResumeLayout(false)
		Me.cmsSpecSelect.ResumeLayout(false)
		Me.cmsHistHead.ResumeLayout(false)
		Me.pnlQR.ResumeLayout(false)
		Me.pnlQR.PerformLayout
		Me.ResumeLayout(false)
		Me.PerformLayout

End Sub
	Friend WithEvents timMain As Timer
	Friend WithEvents Lbl2 As Label
	Friend WithEvents Lbl3 As Label
	Friend WithEvents GroupBox0 As GroupBox
	Friend WithEvents Lbl9 As Label
	Friend WithEvents Lbl8 As Label
	Friend WithEvents Lbl7 As Label
	Friend WithEvents Lbl6 As Label
	Friend WithEvents Lbl5 As Label
	Friend WithEvents Lbl4 As Label
	Friend WithEvents OpenFileDialog1 As OpenFileDialog
	Friend WithEvents ToolTipMain As ToolTip
	Friend WithEvents StatusStrip1 As StatusStrip
	Friend WithEvents ToolStripStatusLabel1 As ToolStripStatusLabel
	Friend WithEvents MenuStrip1 As MenuStrip
	Friend WithEvents tsmiFile As ToolStripMenuItem
	Friend WithEvents tsmiPrint As ToolStripMenuItem
	Friend WithEvents ToolStripSeparator2 As ToolStripSeparator
	Friend WithEvents tsmiExport As ToolStripMenuItem
	Friend WithEvents tsmiStandardExp As ToolStripMenuItem
	Friend WithEvents ToolStripSeparator1 As ToolStripSeparator
	Friend WithEvents tsmiExit As ToolStripMenuItem
	Friend WithEvents tsmiGraph As ToolStripMenuItem
	Friend WithEvents tsmiRecalculation As ToolStripMenuItem
	Friend WithEvents ToolStripSeparator9 As ToolStripSeparator
	Friend WithEvents tsmiRemote As ToolStripMenuItem
	Friend WithEvents tsmiResetError As ToolStripMenuItem
	Friend WithEvents tsmiSettings As ToolStripMenuItem
	Friend WithEvents tsmiOptions As ToolStripMenuItem
	Friend WithEvents tsmiHelp As ToolStripMenuItem
	Friend WithEvents tsmiShowHelp As ToolStripMenuItem
	Friend WithEvents ToolStripSeparator3 As ToolStripSeparator
	Friend WithEvents tsmiAbout As ToolStripMenuItem
	Friend WithEvents cboEquipName As ComboBox
	Friend WithEvents lblEquipment As Label
	Friend WithEvents btnTemp As Button
	Friend WithEvents btnZero As Button
	Friend WithEvents btnErrReset As Button
	Friend WithEvents btnMeasure As Button
	Friend WithEvents timerShowState As Timer
	Friend WithEvents pctConStts As PictureBox
	Friend WithEvents lblDevAlive As Label
	Friend WithEvents btnOrigin As Button
	Friend WithEvents lblMachineVer As Label
	Friend WithEvents lblCpuVer As Label
	Friend WithEvents rbYStress As RadioButton
	Friend WithEvents rbXStress As RadioButton
	Friend WithEvents rbCorrect As RadioButton
	Friend WithEvents lblElapsedVal As Label
	Friend WithEvents pnlRealG As Panel
	Friend WithEvents Panel1 As Panel
	Friend WithEvents Lbl50 As Label
	Friend WithEvents Lbl51 As Label
	Friend WithEvents rbHPMV As RadioButton
	Friend WithEvents rbHPTemp As RadioButton
	Friend WithEvents lblConnectState As Label
	Friend WithEvents bgwInterrupt As System.ComponentModel.BackgroundWorker
	Friend WithEvents tsmiCaliblation As ToolStripMenuItem
	Friend WithEvents tsmiHorizontalConfirmation As ToolStripMenuItem
	Friend WithEvents gbxCurrentVal As GroupBox
	Friend WithEvents lblCurrXPos As Label
	Friend WithEvents lblCurrHPMV As Label
	Friend WithEvents lblCurrHPTemp As Label
	Friend WithEvents lblElapsedTime As Label
	Friend WithEvents lblCurrHPMVVal As Label
	Friend WithEvents lblCurrHPTempVal As Label
	Friend WithEvents lblCurrZPos As Label
	Friend WithEvents lblCurrYPos As Label
	Friend WithEvents lblCurrZPosVal As Label
	Friend WithEvents lblCurrYPosVal As Label
	Friend WithEvents lblCurrXPosVal As Label
	Friend WithEvents lblCurrYStrs As Label
	Friend WithEvents lblCurrXStrs As Label
	Friend WithEvents lblCurrYStrsVal As Label
	Friend WithEvents lblCurrXStrsVal As Label
	Friend WithEvents btnAllClear As Button
	Friend WithEvents tsmiHistoryDraw As ToolStripMenuItem
	Friend WithEvents tsmiFixedX As ToolStripMenuItem
	Friend WithEvents tsmiFixedY As ToolStripMenuItem
	Friend WithEvents Chart1 As System.Windows.Forms.DataVisualization.Charting.Chart
	Friend WithEvents Chart2 As System.Windows.Forms.DataVisualization.Charting.Chart
	Friend WithEvents Panel2 As Panel
	Friend WithEvents bgwTrendUpdate As System.ComponentModel.BackgroundWorker
	Friend WithEvents tsmiStressZeroSet As ToolStripMenuItem
	Friend WithEvents tsmiHistoryClear As ToolStripMenuItem
	Friend WithEvents ToolStripSeparator5 As ToolStripSeparator
	Friend WithEvents tsmiTemp As ToolStripMenuItem
	Friend WithEvents tsmiZeroAdj As ToolStripMenuItem
	Friend WithEvents ToolStripMenuItem1 As ToolStripMenuItem
	Friend WithEvents ToolStripSeparator6 As ToolStripSeparator
	Friend WithEvents ToolStripSeparator7 As ToolStripSeparator
	Friend WithEvents ToolStripSeparator8 As ToolStripSeparator
	Friend WithEvents tsmiFixToFree As ToolStripMenuItem
	Friend WithEvents PrintDocument1 As Printing.PrintDocument
	Friend WithEvents lblCurrPBMVVal As Label
	Friend WithEvents lblCurrPBMV As Label
	Friend WithEvents lblCurrPBTempVal As Label
	Friend WithEvents lblCurrPBTemp As Label
	Friend WithEvents lbl52 As Label
	Friend WithEvents rbPBMV As RadioButton
	Friend WithEvents rbPBTemp As RadioButton
	Friend WithEvents tsmiFullStress As ToolStripMenuItem
	Friend WithEvents tsmiWeight As ToolStripMenuItem
	Friend WithEvents nudEndTim As NumericUpDown
	Friend WithEvents nudMidTim As NumericUpDown
	Friend WithEvents nudStartTim As NumericUpDown
	Friend WithEvents gbxExpoPad As GroupBox
	Friend WithEvents nudExpoSample As NumericUpDown
	Friend WithEvents nudThinning As NumericUpDown
	Friend WithEvents lblThinningOut As Label
	Friend WithEvents lblExpoSample As Label
	Friend WithEvents cboGelTSel As ComboBox
	Friend WithEvents lblMiddleLine As Label
	Friend WithEvents lblOverBaseLine As Label
	Friend WithEvents lblMidA As Label
	Friend WithEvents lblOverBaseA As Label
	Friend WithEvents lblUnderBaseA As Label
	Friend WithEvents lblUnderBaseLine As Label
	Friend WithEvents lblGelTimeSelection As Label
	Friend WithEvents btnExtrapolationSave As Button
	Friend WithEvents lblChartCoordinate As Label
	Friend WithEvents tsmiMeasure As ToolStripMenuItem
	Friend WithEvents tsmiGelTime As ToolStripMenuItem
	Friend WithEvents tsmiFlowCurve As ToolStripMenuItem
	Friend WithEvents lblCurrZStrsVal As Label
	Friend WithEvents lblCurrZStrs As Label
	Friend WithEvents rbZStress As RadioButton
	Friend WithEvents tsmiZeroStress As ToolStripMenuItem
	Friend WithEvents lblXRangeOverLamp As Label
	Friend WithEvents lblYRangeOverLamp As Label
    Friend WithEvents rbViscosity As RadioButton
    Friend WithEvents rbLossTangent As RadioButton
    Friend WithEvents rbElasticMod As RadioButton
    Friend WithEvents rbStorageMod As RadioButton
    Friend WithEvents rbShearMod As RadioButton
    Friend WithEvents rbSampleStrain As RadioButton
    Friend WithEvents rbPosPhase As RadioButton
    Friend WithEvents rbCorrectP As RadioButton
    Friend WithEvents nudXSpan As NumericUpDown
    Friend WithEvents lblScrolWidth As Label
    Friend WithEvents ToolStripSeparator4 As ToolStripSeparator
    Friend WithEvents tsmiXAxisTemp As ToolStripMenuItem
    Friend WithEvents tsmiReturnOrigin As ToolStripMenuItem
    Friend WithEvents lblDispThinningDetail As Label
    Friend WithEvents lblDispThinningRough As Label
    Friend WithEvents nudDispThinningoutDetail As NumericUpDown
    Friend WithEvents nudDispThinningoutRough As NumericUpDown
    Friend WithEvents pnlThinning As Panel
    Friend WithEvents btnThinning As Button
    Friend WithEvents tsmiStandardDraw As ToolStripMenuItem
    Friend WithEvents tsmiStandardClear As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator10 As ToolStripSeparator
	Friend WithEvents btnStdInfo As Button
	Friend WithEvents dgvStd As DataGridView
	Friend WithEvents Column1 As DataGridViewTextBoxColumn
	Friend WithEvents Column2 As DataGridViewTextBoxColumn
	Friend WithEvents rbStd2 As RadioButton
	Friend WithEvents rbStd1 As RadioButton
	Friend WithEvents gbxStdCond As GroupBox
	Friend WithEvents rbReal As RadioButton
	Friend WithEvents btnStdCondClose As Button
	Friend WithEvents cmsDragDrop As ContextMenuStrip
	Friend WithEvents tsmiStandard1 As ToolStripMenuItem
	Friend WithEvents tsmiCansel As ToolStripMenuItem
	Friend WithEvents ToolStripSeparator11 As ToolStripSeparator
	Friend WithEvents tsmiReal As ToolStripMenuItem
	Friend WithEvents lblCalclating As Label
	Friend WithEvents lblResult1Val As Label
	Friend WithEvents lblResult2Val As Label
	Friend WithEvents lblResult2 As Label
	Friend WithEvents lblResult1 As Label
	Friend WithEvents lblResult3 As Label
	Friend WithEvents lblResult3Val As Label
	Friend WithEvents tsmiTextData As ToolStripMenuItem
	Friend WithEvents tsmiAllClear As ToolStripMenuItem
	Friend WithEvents ToolStripSeparator12 As ToolStripSeparator
	Friend WithEvents pnlCalculation As Panel
	Friend WithEvents lblStressPVal As Label
	Friend WithEvents lblStressVal As Label
	Friend WithEvents pnlPosition As Panel
	Friend WithEvents lblViscoVal As Label
	Friend WithEvents lblTangentVal As Label
	Friend WithEvents lblElasticModVal As Label
	Friend WithEvents lblStorageModVal As Label
	Friend WithEvents lblShearModVal As Label
	Friend WithEvents lblSmplStrnVal As Label
	Friend WithEvents lblPhaseVal As Label
	Friend WithEvents ToolStripSeparator13 As ToolStripSeparator
	Friend WithEvents tsmiDigitalMonitorChange As ToolStripMenuItem
	Friend WithEvents btnDigChange As Button
	Friend WithEvents tsmiImport As ToolStripMenuItem
	Friend WithEvents tsmiMeasAreaImport As ToolStripMenuItem
	Friend WithEvents tsmiStdAreaImport As ToolStripMenuItem
	Friend WithEvents ToolStripSeparator14 As ToolStripSeparator
	Friend WithEvents tsmiLEDLight As ToolStripMenuItem
	Friend WithEvents tsmi7SegBright As ToolStripMenuItem
	Friend WithEvents tsmiCmb7SegBright As ToolStripComboBox
	Friend WithEvents pnlSPEC As Panel
	Friend WithEvents cbxDevSPEC As ComboBox
	Friend WithEvents lblDevSPECNameLabel As Label
	Friend WithEvents dgvDevSpec As DataGridView
	Friend WithEvents cmsSpecSelect As ContextMenuStrip
	Friend WithEvents ToolStripMenuItem2 As ToolStripMenuItem
	Friend WithEvents ToolStripMenuItem4 As ToolStripMenuItem
	Friend WithEvents ToolStripMenuItem3 As ToolStripMenuItem
	Friend WithEvents lblZRangeOverLamp As Label
	Friend WithEvents lblSamSPECNameLabel As Label
	Friend WithEvents cbxSamSPEC As ComboBox
	Friend WithEvents dgvSamSpec As DataGridView
	Friend WithEvents nudYAxisZoom As NumericUpDown
	Friend WithEvents Label1 As Label
	Friend WithEvents cmsHistHead As ContextMenuStrip
	Friend WithEvents tsmiHistDel As ToolStripMenuItem
	Friend WithEvents ToolStripSeparator15 As ToolStripSeparator
	Friend WithEvents tsmiHistCut As ToolStripMenuItem
	Friend WithEvents tsmiHistPaste As ToolStripMenuItem
	Friend WithEvents ToolStripSeparator16 As ToolStripSeparator
	Friend WithEvents tsmiHistUndo As ToolStripMenuItem
	Friend WithEvents btnSamSpecEdit As Button
	Friend WithEvents btnDevSpecEdit As Button
	Friend WithEvents lblZStrsCRVal As Label
	Friend WithEvents lblZStrsCR As Label
	Friend WithEvents gbxYscale As GroupBox
	Friend WithEvents nudYscaleMax As NumericUpDown
	Friend WithEvents nudYscaleMin As NumericUpDown
	Friend WithEvents gbxXscale As GroupBox
	Friend WithEvents nudXscaleMax As NumericUpDown
	Friend WithEvents nudXscaleMin As NumericUpDown
	Friend WithEvents ckbYFixed As CheckBox
	Friend WithEvents ckbXFixed As CheckBox
	Friend WithEvents btnYSpanAply As Button
	Friend WithEvents btnXSpanAply As Button
	Friend WithEvents gpxXType As GroupBox
	Friend WithEvents rbXTime As RadioButton
	Friend WithEvents rbXTemp As RadioButton
	Friend WithEvents nudXdivision As NumericUpDown
	Friend WithEvents gpxThinning As GroupBox
	Friend WithEvents Label3 As Label
	Friend WithEvents nudYdivision As NumericUpDown
	Friend WithEvents Label2 As Label
	Friend WithEvents gpxAxisSetting As GroupBox
	Friend WithEvents gpxScrol As GroupBox
	Friend WithEvents tsmiStdAreaImport1 As ToolStripMenuItem
	Friend WithEvents tsmiStdAreaImport2 As ToolStripMenuItem
	Friend WithEvents tsmiStandard2 As ToolStripMenuItem
	Friend WithEvents rbShearVelo As RadioButton
	Friend WithEvents lblShearVeloVal As Label
	Friend WithEvents lblTangentLabel As Label
	Friend WithEvents lblShearVeloLabel As Label
	Friend WithEvents lblElasticModLabel As Label
	Friend WithEvents lblStrgModLabel As Label
	Friend WithEvents lblShearModLabel As Label
	Friend WithEvents lblPhaseLabel As Label
	Friend WithEvents lblSmpStrnLabel As Label
	Friend WithEvents lblViscoLabel As Label
	Friend WithEvents lblCurrStressPLabel As Label
	Friend WithEvents lblCurrStressLabel As Label
	Friend WithEvents Button1 As Button
	Friend WithEvents tsmiSepSlope As ToolStripSeparator
	Friend WithEvents tsmiSLOPERecalc As ToolStripMenuItem
	Friend WithEvents ToolStripSeparator17 As ToolStripSeparator
	Friend WithEvents tsmiSaveGraph As ToolStripMenuItem
	Friend WithEvents SaveFileDialog1 As SaveFileDialog
	Friend WithEvents lblXAuto As Label
	Friend WithEvents lblYAuto As Label
	Friend WithEvents lblExpand As Label
	Friend WithEvents rbZPosition As RadioButton
	Friend WithEvents lblStrLen As Label
	Friend WithEvents lblInterlockVal As Label
	Friend WithEvents Label5 As Label
	Friend WithEvents bgwErrorControl As System.ComponentModel.BackgroundWorker
	Friend WithEvents lblCurrSGMVVal As Label
	Friend WithEvents lblCurrSGMV As Label
	Friend WithEvents lblCurrSGTempVal As Label
	Friend WithEvents lblCurrSGTemp As Label
	Friend WithEvents lblGAPVal As Label
	Friend WithEvents lblGAPLabel As Label
	Friend WithEvents pnlSteress As Panel
	Friend WithEvents lblNoLoadStrVal As Label
	Friend WithEvents lblNoLoadStrLabel As Label
	Friend WithEvents lblZeroPosVal As Label
	Friend WithEvents lblZeroPosLabel As Label
	Friend WithEvents lblAdminEna As Label
	Friend WithEvents ilState As ImageList
	Friend WithEvents tsmiAdminMode As ToolStripMenuItem
	Friend WithEvents lblLongTimeMode As Label
	Friend WithEvents lblLTMMode As Label
	Friend WithEvents lblFailedOrderP As Label
	Friend WithEvents bgwCalculation As System.ComponentModel.BackgroundWorker
	Friend WithEvents lblGuideXYbeforeGap As Label
	Friend WithEvents lblRangeWarning As Label
	Friend WithEvents Chart3 As DataVisualization.Charting.Chart
	Friend WithEvents gbxHistgram As GroupBox
	Friend WithEvents nudSection As NumericUpDown
	Friend WithEvents pnlHistOpe As Panel
	Friend WithEvents lblSecWidth As Label
	Friend WithEvents lvStat As ListView
	Friend WithEvents ColumnHeader1 As ColumnHeader
	Friend WithEvents ColumnHeader2 As ColumnHeader
	Friend WithEvents bynHistClose As Button
	Friend WithEvents tsmiQrCodeCreate As ToolStripMenuItem
	Friend WithEvents btnQRcode As Button
	Friend WithEvents txtQR As TextBox
	Friend WithEvents pnlQR As Panel
	Friend WithEvents QRSimuTimer As Timer
	Friend WithEvents ToolStripSeparator18 As ToolStripSeparator
	Friend WithEvents ToolStripSeparator19 As ToolStripSeparator
	Friend WithEvents lblZConfPosVal As Label
	Friend WithEvents lblZConfPosLabel As Label
	Friend WithEvents btnSPC As Button
	Friend WithEvents tsmiMent As ToolStripMenuItem
	Friend WithEvents ToolStripSeparator20 As ToolStripSeparator
End Class
