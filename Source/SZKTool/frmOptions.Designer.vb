<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmOptions
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
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmOptions))
		Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
		Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
		Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
		Me.TabControl1 = New System.Windows.Forms.TabControl()
		Me.TabPage1 = New System.Windows.Forms.TabPage()
		Me.gbxSelManage = New System.Windows.Forms.GroupBox()
		Me.lblMeasurerLabel = New System.Windows.Forms.Label()
		Me.lblSampleNameLabel = New System.Windows.Forms.Label()
		Me.cbxMeasurer = New System.Windows.Forms.ComboBox()
		Me.cbxSampleName = New System.Windows.Forms.ComboBox()
		Me.btnMeasurerDelete = New System.Windows.Forms.Button()
		Me.btnSamplenameDelete = New System.Windows.Forms.Button()
		Me.btnMeasurerResist = New System.Windows.Forms.Button()
		Me.btnSamplenameResist = New System.Windows.Forms.Button()
		Me.gbxFactorySetting = New System.Windows.Forms.GroupBox()
		Me.lblFactorySettingContents = New System.Windows.Forms.Label()
		Me.btnFactorySetting = New System.Windows.Forms.Button()
		Me.gbxMainte = New System.Windows.Forms.GroupBox()
		Me.gpxSGHeaterOn = New System.Windows.Forms.GroupBox()
		Me.lblSGHeaterOnResetDate = New System.Windows.Forms.Label()
		Me.lblSGHeaterResetDateL = New System.Windows.Forms.Label()
		Me.nudMainteThr5 = New System.Windows.Forms.NumericUpDown()
		Me.lblSGHeaterPreVal = New System.Windows.Forms.Label()
		Me.lblSGHeaterThr = New System.Windows.Forms.Label()
		Me.btnSGHeaterTimeReset = New System.Windows.Forms.Button()
		Me.lblSGHeatOnTime = New System.Windows.Forms.Label()
		Me.gpxHotPlateHeaterOn = New System.Windows.Forms.GroupBox()
		Me.nudMainteThr2 = New System.Windows.Forms.NumericUpDown()
		Me.lblHpHeaterThr = New System.Windows.Forms.Label()
		Me.lblHeaterOnResetDate = New System.Windows.Forms.Label()
		Me.btnHeatOnTimeReset = New System.Windows.Forms.Button()
		Me.lblHeaterOnResetDateL = New System.Windows.Forms.Label()
		Me.lblHeaterOnPreVal = New System.Windows.Forms.Label()
		Me.lblDevHeatOnTime = New System.Windows.Forms.Label()
		Me.gbxPowOn = New System.Windows.Forms.GroupBox()
		Me.lblPowOnResetDate = New System.Windows.Forms.Label()
		Me.lblPowOnResetDateL = New System.Windows.Forms.Label()
		Me.nudMainteThr1 = New System.Windows.Forms.NumericUpDown()
		Me.lblPowOnPreVal = New System.Windows.Forms.Label()
		Me.lblMainteThr1 = New System.Windows.Forms.Label()
		Me.btnPowOnTimeReset = New System.Windows.Forms.Button()
		Me.lblDevPowOnTime = New System.Windows.Forms.Label()
		Me.gpxProbeHeaterOn = New System.Windows.Forms.GroupBox()
		Me.lblProbeHeaterOnResetDate = New System.Windows.Forms.Label()
		Me.lblProbeHeaterResetDateL = New System.Windows.Forms.Label()
		Me.nudMainteThr4 = New System.Windows.Forms.NumericUpDown()
		Me.lblProbeHeaterPreVal = New System.Windows.Forms.Label()
		Me.lblProbeHeaterThr = New System.Windows.Forms.Label()
		Me.btnProbeHeaterTimeReset = New System.Windows.Forms.Button()
		Me.lblProbeHeatOnTime = New System.Windows.Forms.Label()
		Me.gbxXYAct = New System.Windows.Forms.GroupBox()
		Me.lblZActTime = New System.Windows.Forms.Label()
		Me.lblYActTime = New System.Windows.Forms.Label()
		Me.lblZActPreVal = New System.Windows.Forms.Label()
		Me.lblYActPreVal = New System.Windows.Forms.Label()
		Me.lblXYActResetDate = New System.Windows.Forms.Label()
		Me.lblXYActResetDateL = New System.Windows.Forms.Label()
		Me.nudMainteThr3 = New System.Windows.Forms.NumericUpDown()
		Me.lblXActPreVal = New System.Windows.Forms.Label()
		Me.lblXYActThr1 = New System.Windows.Forms.Label()
		Me.btnXYActTimeReset = New System.Windows.Forms.Button()
		Me.lblXActTime = New System.Windows.Forms.Label()
		Me.gbxMdkSet = New System.Windows.Forms.GroupBox()
		Me.Label33 = New System.Windows.Forms.Label()
		Me.lblStanbyTime = New System.Windows.Forms.Label()
		Me.nudStandbyTime = New System.Windows.Forms.NumericUpDown()
		Me.Label15 = New System.Windows.Forms.Label()
		Me.cbxEnaIntLock = New System.Windows.Forms.CheckBox()
		Me.GroupBox1 = New System.Windows.Forms.GroupBox()
		Me.rbSChinese = New System.Windows.Forms.RadioButton()
		Me.rbKorean = New System.Windows.Forms.RadioButton()
		Me.rbTChinese = New System.Windows.Forms.RadioButton()
		Me.rbJPN = New System.Windows.Forms.RadioButton()
		Me.rbENG = New System.Windows.Forms.RadioButton()
		Me.TabPage10 = New System.Windows.Forms.TabPage()
		Me.gbxZeroAdj = New System.Windows.Forms.GroupBox()
		Me.lblStartPosHP = New System.Windows.Forms.Label()
		Me.nudZeroStartPosHP = New System.Windows.Forms.NumericUpDown()
		Me.Label35 = New System.Windows.Forms.Label()
		Me.lblStaFlacRange = New System.Windows.Forms.Label()
		Me.lblStaMonTime = New System.Windows.Forms.Label()
		Me.nudStaMonTime = New System.Windows.Forms.NumericUpDown()
		Me.nudStaFlacRange = New System.Windows.Forms.NumericUpDown()
		Me.Label30 = New System.Windows.Forms.Label()
		Me.Label13 = New System.Windows.Forms.Label()
		Me.gbxZeroAdjMode = New System.Windows.Forms.GroupBox()
		Me.rbZeroZcont = New System.Windows.Forms.RadioButton()
		Me.rbZeroXcont = New System.Windows.Forms.RadioButton()
		Me.gbxCentPosReg = New System.Windows.Forms.GroupBox()
		Me.lblUnit11 = New System.Windows.Forms.Label()
		Me.lblUnit10 = New System.Windows.Forms.Label()
		Me.btnCenterPosReg = New System.Windows.Forms.Button()
		Me.nudCenterY = New System.Windows.Forms.NumericUpDown()
		Me.nudCenterX = New System.Windows.Forms.NumericUpDown()
		Me.lblCentRegY = New System.Windows.Forms.Label()
		Me.lblCentRegX = New System.Windows.Forms.Label()
		Me.gbxHorizontalConf = New System.Windows.Forms.GroupBox()
		Me.btnHorizonConfirm = New System.Windows.Forms.Button()
		Me.gbxXYModeDrive = New System.Windows.Forms.GroupBox()
		Me.Label1 = New System.Windows.Forms.Label()
		Me.lblXYModeZ = New System.Windows.Forms.Label()
		Me.nudZPeriod = New System.Windows.Forms.NumericUpDown()
		Me.nudZAmp = New System.Windows.Forms.NumericUpDown()
		Me.Label5 = New System.Windows.Forms.Label()
		Me.Label44 = New System.Windows.Forms.Label()
		Me.Label40 = New System.Windows.Forms.Label()
		Me.lblXYModeY = New System.Windows.Forms.Label()
		Me.lblXYModeX = New System.Windows.Forms.Label()
		Me.nudYPeriod = New System.Windows.Forms.NumericUpDown()
		Me.nudXPeriod = New System.Windows.Forms.NumericUpDown()
		Me.nudYAmp = New System.Windows.Forms.NumericUpDown()
		Me.nudXAmp = New System.Windows.Forms.NumericUpDown()
		Me.btnXYStop = New System.Windows.Forms.Button()
		Me.btnXYDrive = New System.Windows.Forms.Button()
		Me.Label42 = New System.Windows.Forms.Label()
		Me.Label41 = New System.Windows.Forms.Label()
		Me.gbxManualDrive = New System.Windows.Forms.GroupBox()
		Me.btnOrigin = New System.Windows.Forms.Button()
		Me.btnYAxisDrive = New System.Windows.Forms.Button()
		Me.btnXAxisDrive = New System.Windows.Forms.Button()
		Me.btnHotPlateDrive = New System.Windows.Forms.Button()
		Me.Label29 = New System.Windows.Forms.Label()
		Me.Label28 = New System.Windows.Forms.Label()
		Me.Label27 = New System.Windows.Forms.Label()
		Me.lblYAxisPos = New System.Windows.Forms.Label()
		Me.lblXAxisPos = New System.Windows.Forms.Label()
		Me.lblHeatPos = New System.Windows.Forms.Label()
		Me.nudYAxisPos = New System.Windows.Forms.NumericUpDown()
		Me.nudXAxisPos = New System.Windows.Forms.NumericUpDown()
		Me.nudHeatPos = New System.Windows.Forms.NumericUpDown()
		Me.gbxCentering = New System.Windows.Forms.GroupBox()
		Me.Label39 = New System.Windows.Forms.Label()
		Me.Label38 = New System.Windows.Forms.Label()
		Me.Label37 = New System.Windows.Forms.Label()
		Me.Label36 = New System.Windows.Forms.Label()
		Me.lblCenterY = New System.Windows.Forms.Label()
		Me.lblCenterX = New System.Windows.Forms.Label()
		Me.lblCentPointPos = New System.Windows.Forms.Label()
		Me.lblCentStartPosHP = New System.Windows.Forms.Label()
		Me.nudCentStartPosHP = New System.Windows.Forms.NumericUpDown()
		Me.Label34 = New System.Windows.Forms.Label()
		Me.btnExecCenter = New System.Windows.Forms.Button()
		Me.TabPage2 = New System.Windows.Forms.TabPage()
		Me.grpDefClr = New System.Windows.Forms.GroupBox()
		Me.lblColorAssignDesc = New System.Windows.Forms.Label()
		Me.lblClrL = New System.Windows.Forms.Label()
		Me.lblClrB = New System.Windows.Forms.Label()
		Me.lblClr0 = New System.Windows.Forms.Label()
		Me.lblClr9 = New System.Windows.Forms.Label()
		Me.lblClr8 = New System.Windows.Forms.Label()
		Me.lblClr7 = New System.Windows.Forms.Label()
		Me.lblClr6 = New System.Windows.Forms.Label()
		Me.lblClr5 = New System.Windows.Forms.Label()
		Me.lblClr4 = New System.Windows.Forms.Label()
		Me.lblClr3 = New System.Windows.Forms.Label()
		Me.lblClr2 = New System.Windows.Forms.Label()
		Me.lblClr1 = New System.Windows.Forms.Label()
		Me.cbY2Opp = New System.Windows.Forms.CheckBox()
		Me.gbxMarker = New System.Windows.Forms.GroupBox()
		Me.nudMarkerSize = New System.Windows.Forms.NumericUpDown()
		Me.lblMarkerSize = New System.Windows.Forms.Label()
		Me.lblMarkerStyle = New System.Windows.Forms.Label()
		Me.cbxMarkerStyle = New System.Windows.Forms.ComboBox()
		Me.gbxStressUnit = New System.Windows.Forms.GroupBox()
		Me.Label17 = New System.Windows.Forms.Label()
		Me.rbtnNewton = New System.Windows.Forms.RadioButton()
		Me.rbtnDigital = New System.Windows.Forms.RadioButton()
		Me.rbtnPer = New System.Windows.Forms.RadioButton()
		Me.gbxGraphYSpan = New System.Windows.Forms.GroupBox()
		Me.rbRYauto = New System.Windows.Forms.RadioButton()
		Me.rbRYFix = New System.Windows.Forms.RadioButton()
		Me.gbxGraphXSpan = New System.Windows.Forms.GroupBox()
		Me.rbRXauto = New System.Windows.Forms.RadioButton()
		Me.rbRXFix = New System.Windows.Forms.RadioButton()
		Me.GroupBox14 = New System.Windows.Forms.GroupBox()
		Me.rbARDFill = New System.Windows.Forms.RadioButton()
		Me.rbARDTransparent = New System.Windows.Forms.RadioButton()
		Me.GroupBox7 = New System.Windows.Forms.GroupBox()
		Me.lblScrollWidth = New System.Windows.Forms.Label()
		Me.lblThinningD = New System.Windows.Forms.Label()
		Me.lblThinningR = New System.Windows.Forms.Label()
		Me.nudScrolWidth = New System.Windows.Forms.NumericUpDown()
		Me.nudThinningValueD = New System.Windows.Forms.NumericUpDown()
		Me.nudThinningValueR = New System.Windows.Forms.NumericUpDown()
		Me.GroupBox4 = New System.Windows.Forms.GroupBox()
		Me.rbHideNgtv = New System.Windows.Forms.RadioButton()
		Me.rbDisplayNgtv = New System.Windows.Forms.RadioButton()
		Me.GroupBox3 = New System.Windows.Forms.GroupBox()
		Me.rbNoGrids = New System.Windows.Forms.RadioButton()
		Me.rbDotGrids = New System.Windows.Forms.RadioButton()
		Me.rbDash = New System.Windows.Forms.RadioButton()
		Me.rbSolid = New System.Windows.Forms.RadioButton()
		Me.TabPage3 = New System.Windows.Forms.TabPage()
		Me.gbxSPC = New System.Windows.Forms.GroupBox()
		Me.nudSPCCalcSample = New System.Windows.Forms.NumericUpDown()
		Me.lblSPCCalcSample = New System.Windows.Forms.Label()
		Me.lblSPCDesc = New System.Windows.Forms.Label()
		Me.cbxSPC = New System.Windows.Forms.CheckBox()
		Me.gbxAutoReg = New System.Windows.Forms.GroupBox()
		Me.lblAutoRegDesc = New System.Windows.Forms.Label()
		Me.cbxAutoReg = New System.Windows.Forms.CheckBox()
		Me.gbxZeroCalib = New System.Windows.Forms.GroupBox()
		Me.btnZeroCalib = New System.Windows.Forms.Button()
		Me.gbxWeightCalib = New System.Windows.Forms.GroupBox()
		Me.btnWeightCalib = New System.Windows.Forms.Button()
		Me.gbxFilter = New System.Windows.Forms.GroupBox()
		Me.cbxFilterEnable = New System.Windows.Forms.CheckBox()
		Me.gbxFilterIIR = New System.Windows.Forms.GroupBox()
		Me.nudIIRDeg = New System.Windows.Forms.NumericUpDown()
		Me.lblIIRDegree = New System.Windows.Forms.Label()
		Me.nudIIRCoffFreq = New System.Windows.Forms.NumericUpDown()
		Me.lblIIRCofFreq = New System.Windows.Forms.Label()
		Me.lblFilterFileName = New System.Windows.Forms.Label()
		Me.btnFilterSelect = New System.Windows.Forms.Button()
		Me.rbIIR = New System.Windows.Forms.RadioButton()
		Me.rbFIR = New System.Windows.Forms.RadioButton()
		Me.gbxFilterFIR = New System.Windows.Forms.GroupBox()
		Me.cbxWinKind = New System.Windows.Forms.ComboBox()
		Me.nudFilterLen = New System.Windows.Forms.NumericUpDown()
		Me.nudFIRCoffFreq = New System.Windows.Forms.NumericUpDown()
		Me.lblFIRWinKind = New System.Windows.Forms.Label()
		Me.lblFIRFilterLen = New System.Windows.Forms.Label()
		Me.lblFIRCofFreq = New System.Windows.Forms.Label()
		Me.gbxFullCalib = New System.Windows.Forms.GroupBox()
		Me.btnFullCalib = New System.Windows.Forms.Button()
		Me.TabPage5 = New System.Windows.Forms.TabPage()
		Me.gpxHiddenSetting = New System.Windows.Forms.GroupBox()
		Me.btnCreateFactorySetting = New System.Windows.Forms.Button()
		Me.cbxTempNoLimitEn = New System.Windows.Forms.CheckBox()
		Me.cbxReadRealAreaEn = New System.Windows.Forms.CheckBox()
		Me.cbxCalcResultFOutEn = New System.Windows.Forms.CheckBox()
		Me.cbxEquipListEn = New System.Windows.Forms.CheckBox()
		Me.cbxScanTimeEn = New System.Windows.Forms.CheckBox()
		Me.cbxMeasTimeEn = New System.Windows.Forms.CheckBox()
		Me.gbxZUpperLimit = New System.Windows.Forms.GroupBox()
		Me.nudZUpperLimit = New System.Windows.Forms.NumericUpDown()
		Me.cbxMultActEn = New System.Windows.Forms.CheckBox()
		Me.cbxCoverILDis = New System.Windows.Forms.CheckBox()
		Me.cbDatafileCalcEnable = New System.Windows.Forms.CheckBox()
		Me.cbxCalibValChangeEn = New System.Windows.Forms.CheckBox()
		Me.cbxDebugFileEn = New System.Windows.Forms.CheckBox()
		Me.cbxZStabDis = New System.Windows.Forms.CheckBox()
		Me.cbxInputEnMeasureing = New System.Windows.Forms.CheckBox()
		Me.cbxPcLifeChkDis = New System.Windows.Forms.CheckBox()
		Me.gbxBuzzerControl = New System.Windows.Forms.GroupBox()
		Me.lblEndOfBuzzerPattern = New System.Windows.Forms.Label()
		Me.nudBZPattern = New System.Windows.Forms.NumericUpDown()
		Me.cbxOkOfHeat = New System.Windows.Forms.CheckBox()
		Me.cbxEndOfMesure = New System.Windows.Forms.CheckBox()
		Me.gbxTimeSet = New System.Windows.Forms.GroupBox()
		Me.lblTime = New System.Windows.Forms.Label()
		Me.lblDate = New System.Windows.Forms.Label()
		Me.nudSecond = New System.Windows.Forms.NumericUpDown()
		Me.nudMinute = New System.Windows.Forms.NumericUpDown()
		Me.nudDay = New System.Windows.Forms.NumericUpDown()
		Me.nudHour = New System.Windows.Forms.NumericUpDown()
		Me.nudMonth = New System.Windows.Forms.NumericUpDown()
		Me.nudYear = New System.Windows.Forms.NumericUpDown()
		Me.btnNow = New System.Windows.Forms.Button()
		Me.btnDateAply = New System.Windows.Forms.Button()
		Me.GroupBox12 = New System.Windows.Forms.GroupBox()
		Me.lblAutoStartTime = New System.Windows.Forms.Label()
		Me.Label2 = New System.Windows.Forms.Label()
		Me.cbAutoStart = New System.Windows.Forms.CheckBox()
		Me.nudAutoStart = New System.Windows.Forms.NumericUpDown()
		Me.GroupBox10 = New System.Windows.Forms.GroupBox()
		Me.btnDeviceSerWite = New System.Windows.Forms.Button()
		Me.dgvEquipList = New System.Windows.Forms.DataGridView()
		Me.SelectColumn = New System.Windows.Forms.DataGridViewImageColumn()
		Me.IPColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
		Me.NameColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
		Me.XYSensSerial = New System.Windows.Forms.DataGridViewTextBoxColumn()
		Me.ZSensSerial = New System.Windows.Forms.DataGridViewTextBoxColumn()
		Me.CF1Column = New System.Windows.Forms.DataGridViewTextBoxColumn()
		Me.ToolCF1Column = New System.Windows.Forms.DataGridViewTextBoxColumn()
		Me.FTPColumn = New System.Windows.Forms.DataGridViewButtonColumn()
		Me.Label3 = New System.Windows.Forms.Label()
		Me.txtPort = New System.Windows.Forms.TextBox()
		Me.TabPage6 = New System.Windows.Forms.TabPage()
		Me.gbxSpecList = New System.Windows.Forms.GroupBox()
		Me.txtSpecName = New System.Windows.Forms.TextBox()
		Me.lblSPECName = New System.Windows.Forms.Label()
		Me.btnSpecCopyFix = New System.Windows.Forms.Button()
		Me.btnSpecCopyFree = New System.Windows.Forms.Button()
		Me.dgvSpec = New System.Windows.Forms.DataGridView()
		Me.DataGridViewTextBoxColumn1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
		Me.DataGridViewTextBoxColumn2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
		Me.dgvCalcOpt = New System.Windows.Forms.DataGridView()
		Me.DataGridViewTextBoxColumn3 = New System.Windows.Forms.DataGridViewTextBoxColumn()
		Me.DataGridViewTextBoxColumn4 = New System.Windows.Forms.DataGridViewTextBoxColumn()
		Me.cbxLotManu = New System.Windows.Forms.CheckBox()
		Me.btnSpcImp = New System.Windows.Forms.Button()
		Me.btnSpcExp = New System.Windows.Forms.Button()
		Me.lblSpecFilter = New System.Windows.Forms.Label()
		Me.btnFixFilter = New System.Windows.Forms.Button()
		Me.txtFixFilter = New System.Windows.Forms.TextBox()
		Me.cboSpecRtgset = New System.Windows.Forms.ComboBox()
		Me.lblHotplateSetting = New System.Windows.Forms.Label()
		Me.btnSaveSpec = New System.Windows.Forms.Button()
		Me.btnDeleteSpec = New System.Windows.Forms.Button()
		Me.lblMethod = New System.Windows.Forms.Label()
		Me.cboMethod = New System.Windows.Forms.ComboBox()
		Me.cboXYModeOp = New System.Windows.Forms.ComboBox()
		Me.lblXYDriveMode = New System.Windows.Forms.Label()
		Me.lvSpec = New System.Windows.Forms.ListView()
		Me.ColumnHeader3 = CType(New System.Windows.Forms.ColumnHeader(),System.Windows.Forms.ColumnHeader)
		Me.TabPage7 = New System.Windows.Forms.TabPage()
		Me.gbxTempControl = New System.Windows.Forms.GroupBox()
		Me.gbxSG = New System.Windows.Forms.GroupBox()
		Me.nudHDifSetTemp3 = New System.Windows.Forms.NumericUpDown()
		Me.lblDifSet = New System.Windows.Forms.Label()
		Me.nudHLowerTemp3 = New System.Windows.Forms.NumericUpDown()
		Me.lblLoLimit3 = New System.Windows.Forms.Label()
		Me.nudPVGain3 = New System.Windows.Forms.NumericUpDown()
		Me.lblPVGain3 = New System.Windows.Forms.Label()
		Me.nudPVBias3 = New System.Windows.Forms.NumericUpDown()
		Me.nudHUpperTemp3 = New System.Windows.Forms.NumericUpDown()
		Me.lblUpLimit3 = New System.Windows.Forms.Label()
		Me.nudDgain3 = New System.Windows.Forms.NumericUpDown()
		Me.nudD3 = New System.Windows.Forms.NumericUpDown()
		Me.nudI3 = New System.Windows.Forms.NumericUpDown()
		Me.nudP3 = New System.Windows.Forms.NumericUpDown()
		Me.lblD3 = New System.Windows.Forms.Label()
		Me.lblI3 = New System.Windows.Forms.Label()
		Me.lblPVBias3 = New System.Windows.Forms.Label()
		Me.lblDgain3 = New System.Windows.Forms.Label()
		Me.lblP3 = New System.Windows.Forms.Label()
		Me.btnPIDMonitor = New System.Windows.Forms.Button()
		Me.gbxPB = New System.Windows.Forms.GroupBox()
		Me.nudPVGain2 = New System.Windows.Forms.NumericUpDown()
		Me.lblPVGain2 = New System.Windows.Forms.Label()
		Me.nudPVBias2 = New System.Windows.Forms.NumericUpDown()
		Me.nudHUpperTemp2 = New System.Windows.Forms.NumericUpDown()
		Me.lblUpLimit2 = New System.Windows.Forms.Label()
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
		Me.nudPVBias = New System.Windows.Forms.NumericUpDown()
		Me.nudHUpperTemp = New System.Windows.Forms.NumericUpDown()
		Me.lblUpLimit = New System.Windows.Forms.Label()
		Me.nudDgain = New System.Windows.Forms.NumericUpDown()
		Me.nudD = New System.Windows.Forms.NumericUpDown()
		Me.nudI = New System.Windows.Forms.NumericUpDown()
		Me.nudP = New System.Windows.Forms.NumericUpDown()
		Me.lblD = New System.Windows.Forms.Label()
		Me.lblI = New System.Windows.Forms.Label()
		Me.lblPVGain = New System.Windows.Forms.Label()
		Me.lblPVBias = New System.Windows.Forms.Label()
		Me.lblDgain = New System.Windows.Forms.Label()
		Me.lblP = New System.Windows.Forms.Label()
		Me.cbxTempLEDEn = New System.Windows.Forms.CheckBox()
		Me.lblLEDTempObsrv = New System.Windows.Forms.Label()
		Me.nudLEDTempObsrv = New System.Windows.Forms.NumericUpDown()
		Me.TabPage9 = New System.Windows.Forms.TabPage()
		Me.gbxZCalib = New System.Windows.Forms.GroupBox()
		Me.lblCalibOK = New System.Windows.Forms.Label()
		Me.lblRemTimeVal = New System.Windows.Forms.Label()
		Me.lblPosDeviLabel = New System.Windows.Forms.Label()
		Me.btnTempGraph = New System.Windows.Forms.Button()
		Me.lblTempPreLabel = New System.Windows.Forms.Label()
		Me.lblRemTimeLabel = New System.Windows.Forms.Label()
		Me.btnZPosCalib = New System.Windows.Forms.Button()
		Me.lblPosDeviVal = New System.Windows.Forms.Label()
		Me.lblTempPreVal = New System.Windows.Forms.Label()
		Me.gbxTempProfile = New System.Windows.Forms.GroupBox()
		Me.btnRtgsImp = New System.Windows.Forms.Button()
		Me.btnRtgsExp = New System.Windows.Forms.Button()
		Me.btnTempClear = New System.Windows.Forms.Button()
		Me.btnRTGLinearTool = New System.Windows.Forms.Button()
		Me.btnTempRepeat = New System.Windows.Forms.Button()
		Me.btnSaveRtgs = New System.Windows.Forms.Button()
		Me.btnDeleteRtgs = New System.Windows.Forms.Button()
		Me.dgvRtgSetting = New System.Windows.Forms.DataGridView()
		Me.Column1 = New System.Windows.Forms.DataGridViewImageColumn()
		Me.Column2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
		Me.Column3 = New System.Windows.Forms.DataGridViewTextBoxColumn()
		Me.Column4 = New System.Windows.Forms.DataGridViewTextBoxColumn()
		Me.txtRtgsetName = New System.Windows.Forms.TextBox()
		Me.lblTempProfileName = New System.Windows.Forms.Label()
		Me.lvRtgset = New System.Windows.Forms.ListView()
		Me.ColumnHeader4 = CType(New System.Windows.Forms.ColumnHeader(),System.Windows.Forms.ColumnHeader)
		Me.TabPage4 = New System.Windows.Forms.TabPage()
		Me.gbxQRCode = New System.Windows.Forms.GroupBox()
		Me.cboECC = New System.Windows.Forms.ComboBox()
		Me.nudPixelInCell = New System.Windows.Forms.NumericUpDown()
		Me.nudCellsInLogo = New System.Windows.Forms.NumericUpDown()
		Me.cbxQRCodeEnable = New System.Windows.Forms.CheckBox()
		Me.lblPixelInCell = New System.Windows.Forms.Label()
		Me.lblCellsInLogo = New System.Windows.Forms.Label()
		Me.lblECCLabel = New System.Windows.Forms.Label()
		Me.gbxLongMeasMode = New System.Windows.Forms.GroupBox()
		Me.cbxLongMeasMode = New System.Windows.Forms.CheckBox()
		Me.nudLTAdoptRate = New System.Windows.Forms.NumericUpDown()
		Me.nudLTMeasInterval = New System.Windows.Forms.NumericUpDown()
		Me.lblLTAdoptRate = New System.Windows.Forms.Label()
		Me.lblLTMeasInterval = New System.Windows.Forms.Label()
		Me.gbxMeasCont = New System.Windows.Forms.GroupBox()
		Me.ckbGreenFlashInMeas = New System.Windows.Forms.CheckBox()
		Me.ckbDoublePBUnlockConfirm = New System.Windows.Forms.CheckBox()
		Me.nudGapCorr = New System.Windows.Forms.NumericUpDown()
		Me.gbxStressDetectSetting = New System.Windows.Forms.GroupBox()
		Me.nudStresDetDuraTime = New System.Windows.Forms.NumericUpDown()
		Me.nudStrsDetADThreshold = New System.Windows.Forms.NumericUpDown()
		Me.nudStrsDetAcqTime = New System.Windows.Forms.NumericUpDown()
		Me.lblDuration = New System.Windows.Forms.Label()
		Me.lblADThreshold = New System.Windows.Forms.Label()
		Me.lblStrsDetAcqTime = New System.Windows.Forms.Label()
		Me.lblGapCorrLabel = New System.Windows.Forms.Label()
		Me.lblStrainStab = New System.Windows.Forms.Label()
		Me.nudStrainStab = New System.Windows.Forms.NumericUpDown()
		Me.lblPresContDeyayTimeLabel = New System.Windows.Forms.Label()
		Me.lblSGTempRangeLabel = New System.Windows.Forms.Label()
		Me.lblPBTempRangeLabel = New System.Windows.Forms.Label()
		Me.nudPresContDeyayTime = New System.Windows.Forms.NumericUpDown()
		Me.nudSGTempRange = New System.Windows.Forms.NumericUpDown()
		Me.nudPBTempRange = New System.Windows.Forms.NumericUpDown()
		Me.nudHPTempRange = New System.Windows.Forms.NumericUpDown()
		Me.lblHPTempRangeLabel = New System.Windows.Forms.Label()
		Me.nudInputGap = New System.Windows.Forms.NumericUpDown()
		Me.lblInputGapLabel = New System.Windows.Forms.Label()
		Me.nudSampleInputWait = New System.Windows.Forms.NumericUpDown()
		Me.lblWaitForInputLabel = New System.Windows.Forms.Label()
		Me.TabPage8 = New System.Windows.Forms.TabPage()
		Me.GroupBox19 = New System.Windows.Forms.GroupBox()
		Me.btnMeasOperDel = New System.Windows.Forms.Button()
		Me.btnMeasPlaceDel = New System.Windows.Forms.Button()
		Me.cbxMeasOperator = New System.Windows.Forms.ComboBox()
		Me.cbxMeasPlace = New System.Windows.Forms.ComboBox()
		Me.Label20 = New System.Windows.Forms.Label()
		Me.Label19 = New System.Windows.Forms.Label()
		Me.GroupBox23 = New System.Windows.Forms.GroupBox()
		Me.Label23 = New System.Windows.Forms.Label()
		Me.nudDskSpc = New System.Windows.Forms.NumericUpDown()
		Me.Label22 = New System.Windows.Forms.Label()
		Me.GroupBox18 = New System.Windows.Forms.GroupBox()
		Me.lblFileWarn = New System.Windows.Forms.Label()
		Me.btnSelStrgFldMeasData = New System.Windows.Forms.Button()
		Me.txtStrgFldMeasData = New System.Windows.Forms.TextBox()
		Me.lblStrgFldMeasData = New System.Windows.Forms.Label()
		Me.chkAutSaveMeasData = New System.Windows.Forms.CheckBox()
		Me.GroupBox20 = New System.Windows.Forms.GroupBox()
		Me.rbtnJpg = New System.Windows.Forms.RadioButton()
		Me.rbtnPdf = New System.Windows.Forms.RadioButton()
		Me.btnSelStrgFld = New System.Windows.Forms.Button()
		Me.txtStrgFld = New System.Windows.Forms.TextBox()
		Me.Label18 = New System.Windows.Forms.Label()
		Me.cbxAutPrnt = New System.Windows.Forms.CheckBox()
		Me.GroupBox5 = New System.Windows.Forms.GroupBox()
		Me.btnMDKBackupper = New System.Windows.Forms.Button()
		Me.cbCanDelete = New System.Windows.Forms.CheckBox()
		Me.cbModifyRecalc = New System.Windows.Forms.CheckBox()
		Me.cbModifyCF = New System.Windows.Forms.CheckBox()
		Me.cbModifySpec = New System.Windows.Forms.CheckBox()
		Me.TabControl2 = New System.Windows.Forms.TabControl()
		Me.btnApply = New System.Windows.Forms.Button()
		Me.btnCancel = New System.Windows.Forms.Button()
		Me.btnOK = New System.Windows.Forms.Button()
		Me.timMain = New System.Windows.Forms.Timer(Me.components)
		Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog()
		Me.ToolTipOption = New System.Windows.Forms.ToolTip(Me.components)
		Me.TabControl1.SuspendLayout
		Me.TabPage1.SuspendLayout
		Me.gbxSelManage.SuspendLayout
		Me.gbxFactorySetting.SuspendLayout
		Me.gbxMainte.SuspendLayout
		Me.gpxSGHeaterOn.SuspendLayout
		CType(Me.nudMainteThr5,System.ComponentModel.ISupportInitialize).BeginInit
		Me.gpxHotPlateHeaterOn.SuspendLayout
		CType(Me.nudMainteThr2,System.ComponentModel.ISupportInitialize).BeginInit
		Me.gbxPowOn.SuspendLayout
		CType(Me.nudMainteThr1,System.ComponentModel.ISupportInitialize).BeginInit
		Me.gpxProbeHeaterOn.SuspendLayout
		CType(Me.nudMainteThr4,System.ComponentModel.ISupportInitialize).BeginInit
		Me.gbxXYAct.SuspendLayout
		CType(Me.nudMainteThr3,System.ComponentModel.ISupportInitialize).BeginInit
		Me.gbxMdkSet.SuspendLayout
		CType(Me.nudStandbyTime,System.ComponentModel.ISupportInitialize).BeginInit
		Me.GroupBox1.SuspendLayout
		Me.TabPage10.SuspendLayout
		Me.gbxZeroAdj.SuspendLayout
		CType(Me.nudZeroStartPosHP,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.nudStaMonTime,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.nudStaFlacRange,System.ComponentModel.ISupportInitialize).BeginInit
		Me.gbxZeroAdjMode.SuspendLayout
		Me.gbxCentPosReg.SuspendLayout
		CType(Me.nudCenterY,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.nudCenterX,System.ComponentModel.ISupportInitialize).BeginInit
		Me.gbxHorizontalConf.SuspendLayout
		Me.gbxXYModeDrive.SuspendLayout
		CType(Me.nudZPeriod,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.nudZAmp,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.nudYPeriod,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.nudXPeriod,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.nudYAmp,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.nudXAmp,System.ComponentModel.ISupportInitialize).BeginInit
		Me.gbxManualDrive.SuspendLayout
		CType(Me.nudYAxisPos,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.nudXAxisPos,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.nudHeatPos,System.ComponentModel.ISupportInitialize).BeginInit
		Me.gbxCentering.SuspendLayout
		CType(Me.nudCentStartPosHP,System.ComponentModel.ISupportInitialize).BeginInit
		Me.TabPage2.SuspendLayout
		Me.grpDefClr.SuspendLayout
		Me.gbxMarker.SuspendLayout
		CType(Me.nudMarkerSize,System.ComponentModel.ISupportInitialize).BeginInit
		Me.gbxStressUnit.SuspendLayout
		Me.gbxGraphYSpan.SuspendLayout
		Me.gbxGraphXSpan.SuspendLayout
		Me.GroupBox14.SuspendLayout
		Me.GroupBox7.SuspendLayout
		CType(Me.nudScrolWidth,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.nudThinningValueD,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.nudThinningValueR,System.ComponentModel.ISupportInitialize).BeginInit
		Me.GroupBox4.SuspendLayout
		Me.GroupBox3.SuspendLayout
		Me.TabPage3.SuspendLayout
		Me.gbxSPC.SuspendLayout
		CType(Me.nudSPCCalcSample,System.ComponentModel.ISupportInitialize).BeginInit
		Me.gbxAutoReg.SuspendLayout
		Me.gbxZeroCalib.SuspendLayout
		Me.gbxWeightCalib.SuspendLayout
		Me.gbxFilter.SuspendLayout
		Me.gbxFilterIIR.SuspendLayout
		CType(Me.nudIIRDeg,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.nudIIRCoffFreq,System.ComponentModel.ISupportInitialize).BeginInit
		Me.gbxFilterFIR.SuspendLayout
		CType(Me.nudFilterLen,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.nudFIRCoffFreq,System.ComponentModel.ISupportInitialize).BeginInit
		Me.gbxFullCalib.SuspendLayout
		Me.TabPage5.SuspendLayout
		Me.gpxHiddenSetting.SuspendLayout
		Me.gbxZUpperLimit.SuspendLayout
		CType(Me.nudZUpperLimit,System.ComponentModel.ISupportInitialize).BeginInit
		Me.gbxBuzzerControl.SuspendLayout
		CType(Me.nudBZPattern,System.ComponentModel.ISupportInitialize).BeginInit
		Me.gbxTimeSet.SuspendLayout
		CType(Me.nudSecond,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.nudMinute,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.nudDay,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.nudHour,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.nudMonth,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.nudYear,System.ComponentModel.ISupportInitialize).BeginInit
		Me.GroupBox12.SuspendLayout
		CType(Me.nudAutoStart,System.ComponentModel.ISupportInitialize).BeginInit
		Me.GroupBox10.SuspendLayout
		CType(Me.dgvEquipList,System.ComponentModel.ISupportInitialize).BeginInit
		Me.TabPage6.SuspendLayout
		Me.gbxSpecList.SuspendLayout
		CType(Me.dgvSpec,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.dgvCalcOpt,System.ComponentModel.ISupportInitialize).BeginInit
		Me.TabPage7.SuspendLayout
		Me.gbxTempControl.SuspendLayout
		Me.gbxSG.SuspendLayout
		CType(Me.nudHDifSetTemp3,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.nudHLowerTemp3,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.nudPVGain3,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.nudPVBias3,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.nudHUpperTemp3,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.nudDgain3,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.nudD3,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.nudI3,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.nudP3,System.ComponentModel.ISupportInitialize).BeginInit
		Me.gbxPB.SuspendLayout
		CType(Me.nudPVGain2,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.nudPVBias2,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.nudHUpperTemp2,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.nudDgain2,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.nudD2,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.nudI2,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.nudP2,System.ComponentModel.ISupportInitialize).BeginInit
		Me.gbxHP.SuspendLayout
		CType(Me.nudPVGain,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.nudPVBias,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.nudHUpperTemp,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.nudDgain,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.nudD,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.nudI,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.nudP,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.nudLEDTempObsrv,System.ComponentModel.ISupportInitialize).BeginInit
		Me.TabPage9.SuspendLayout
		Me.gbxZCalib.SuspendLayout
		Me.gbxTempProfile.SuspendLayout
		CType(Me.dgvRtgSetting,System.ComponentModel.ISupportInitialize).BeginInit
		Me.TabPage4.SuspendLayout
		Me.gbxQRCode.SuspendLayout
		CType(Me.nudPixelInCell,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.nudCellsInLogo,System.ComponentModel.ISupportInitialize).BeginInit
		Me.gbxLongMeasMode.SuspendLayout
		CType(Me.nudLTAdoptRate,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.nudLTMeasInterval,System.ComponentModel.ISupportInitialize).BeginInit
		Me.gbxMeasCont.SuspendLayout
		CType(Me.nudGapCorr,System.ComponentModel.ISupportInitialize).BeginInit
		Me.gbxStressDetectSetting.SuspendLayout
		CType(Me.nudStresDetDuraTime,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.nudStrsDetADThreshold,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.nudStrsDetAcqTime,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.nudStrainStab,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.nudPresContDeyayTime,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.nudSGTempRange,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.nudPBTempRange,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.nudHPTempRange,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.nudInputGap,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.nudSampleInputWait,System.ComponentModel.ISupportInitialize).BeginInit
		Me.TabPage8.SuspendLayout
		Me.GroupBox19.SuspendLayout
		Me.GroupBox23.SuspendLayout
		CType(Me.nudDskSpc,System.ComponentModel.ISupportInitialize).BeginInit
		Me.GroupBox18.SuspendLayout
		Me.GroupBox20.SuspendLayout
		Me.GroupBox5.SuspendLayout
		Me.SuspendLayout
		'
		'TabControl1
		'
		Me.TabControl1.Controls.Add(Me.TabPage1)
		Me.TabControl1.Controls.Add(Me.TabPage10)
		Me.TabControl1.Controls.Add(Me.TabPage2)
		Me.TabControl1.Controls.Add(Me.TabPage3)
		Me.TabControl1.Controls.Add(Me.TabPage5)
		Me.TabControl1.Controls.Add(Me.TabPage6)
		Me.TabControl1.Controls.Add(Me.TabPage7)
		Me.TabControl1.Controls.Add(Me.TabPage9)
		Me.TabControl1.Controls.Add(Me.TabPage4)
		Me.TabControl1.Controls.Add(Me.TabPage8)
		resources.ApplyResources(Me.TabControl1, "TabControl1")
		Me.TabControl1.Multiline = true
		Me.TabControl1.Name = "TabControl1"
		Me.TabControl1.SelectedIndex = 0
		'
		'TabPage1
		'
		Me.TabPage1.Controls.Add(Me.gbxSelManage)
		Me.TabPage1.Controls.Add(Me.gbxFactorySetting)
		Me.TabPage1.Controls.Add(Me.gbxMainte)
		Me.TabPage1.Controls.Add(Me.gbxMdkSet)
		Me.TabPage1.Controls.Add(Me.GroupBox1)
		resources.ApplyResources(Me.TabPage1, "TabPage1")
		Me.TabPage1.Name = "TabPage1"
		Me.TabPage1.UseVisualStyleBackColor = true
		'
		'gbxSelManage
		'
		Me.gbxSelManage.Controls.Add(Me.lblMeasurerLabel)
		Me.gbxSelManage.Controls.Add(Me.lblSampleNameLabel)
		Me.gbxSelManage.Controls.Add(Me.cbxMeasurer)
		Me.gbxSelManage.Controls.Add(Me.cbxSampleName)
		Me.gbxSelManage.Controls.Add(Me.btnMeasurerDelete)
		Me.gbxSelManage.Controls.Add(Me.btnSamplenameDelete)
		Me.gbxSelManage.Controls.Add(Me.btnMeasurerResist)
		Me.gbxSelManage.Controls.Add(Me.btnSamplenameResist)
		resources.ApplyResources(Me.gbxSelManage, "gbxSelManage")
		Me.gbxSelManage.Name = "gbxSelManage"
		Me.gbxSelManage.TabStop = false
		'
		'lblMeasurerLabel
		'
		resources.ApplyResources(Me.lblMeasurerLabel, "lblMeasurerLabel")
		Me.lblMeasurerLabel.Name = "lblMeasurerLabel"
		'
		'lblSampleNameLabel
		'
		resources.ApplyResources(Me.lblSampleNameLabel, "lblSampleNameLabel")
		Me.lblSampleNameLabel.Name = "lblSampleNameLabel"
		'
		'cbxMeasurer
		'
		resources.ApplyResources(Me.cbxMeasurer, "cbxMeasurer")
		Me.cbxMeasurer.FormattingEnabled = true
		Me.cbxMeasurer.Items.AddRange(New Object() {resources.GetString("cbxMeasurer.Items"), resources.GetString("cbxMeasurer.Items1"), resources.GetString("cbxMeasurer.Items2")})
		Me.cbxMeasurer.Name = "cbxMeasurer"
		'
		'cbxSampleName
		'
		resources.ApplyResources(Me.cbxSampleName, "cbxSampleName")
		Me.cbxSampleName.FormattingEnabled = true
		Me.cbxSampleName.Items.AddRange(New Object() {resources.GetString("cbxSampleName.Items"), resources.GetString("cbxSampleName.Items1"), resources.GetString("cbxSampleName.Items2")})
		Me.cbxSampleName.Name = "cbxSampleName"
		'
		'btnMeasurerDelete
		'
		resources.ApplyResources(Me.btnMeasurerDelete, "btnMeasurerDelete")
		Me.btnMeasurerDelete.Name = "btnMeasurerDelete"
		Me.btnMeasurerDelete.UseVisualStyleBackColor = true
		'
		'btnSamplenameDelete
		'
		resources.ApplyResources(Me.btnSamplenameDelete, "btnSamplenameDelete")
		Me.btnSamplenameDelete.Name = "btnSamplenameDelete"
		Me.btnSamplenameDelete.UseVisualStyleBackColor = true
		'
		'btnMeasurerResist
		'
		resources.ApplyResources(Me.btnMeasurerResist, "btnMeasurerResist")
		Me.btnMeasurerResist.Name = "btnMeasurerResist"
		Me.btnMeasurerResist.UseVisualStyleBackColor = true
		'
		'btnSamplenameResist
		'
		resources.ApplyResources(Me.btnSamplenameResist, "btnSamplenameResist")
		Me.btnSamplenameResist.Name = "btnSamplenameResist"
		Me.btnSamplenameResist.UseVisualStyleBackColor = true
		'
		'gbxFactorySetting
		'
		Me.gbxFactorySetting.Controls.Add(Me.lblFactorySettingContents)
		Me.gbxFactorySetting.Controls.Add(Me.btnFactorySetting)
		resources.ApplyResources(Me.gbxFactorySetting, "gbxFactorySetting")
		Me.gbxFactorySetting.Name = "gbxFactorySetting"
		Me.gbxFactorySetting.TabStop = false
		'
		'lblFactorySettingContents
		'
		resources.ApplyResources(Me.lblFactorySettingContents, "lblFactorySettingContents")
		Me.lblFactorySettingContents.Name = "lblFactorySettingContents"
		'
		'btnFactorySetting
		'
		resources.ApplyResources(Me.btnFactorySetting, "btnFactorySetting")
		Me.btnFactorySetting.Name = "btnFactorySetting"
		Me.btnFactorySetting.UseVisualStyleBackColor = true
		'
		'gbxMainte
		'
		Me.gbxMainte.Controls.Add(Me.gpxSGHeaterOn)
		Me.gbxMainte.Controls.Add(Me.gpxHotPlateHeaterOn)
		Me.gbxMainte.Controls.Add(Me.gbxPowOn)
		Me.gbxMainte.Controls.Add(Me.gpxProbeHeaterOn)
		Me.gbxMainte.Controls.Add(Me.gbxXYAct)
		resources.ApplyResources(Me.gbxMainte, "gbxMainte")
		Me.gbxMainte.Name = "gbxMainte"
		Me.gbxMainte.TabStop = false
		'
		'gpxSGHeaterOn
		'
		Me.gpxSGHeaterOn.Controls.Add(Me.lblSGHeaterOnResetDate)
		Me.gpxSGHeaterOn.Controls.Add(Me.lblSGHeaterResetDateL)
		Me.gpxSGHeaterOn.Controls.Add(Me.nudMainteThr5)
		Me.gpxSGHeaterOn.Controls.Add(Me.lblSGHeaterPreVal)
		Me.gpxSGHeaterOn.Controls.Add(Me.lblSGHeaterThr)
		Me.gpxSGHeaterOn.Controls.Add(Me.btnSGHeaterTimeReset)
		Me.gpxSGHeaterOn.Controls.Add(Me.lblSGHeatOnTime)
		resources.ApplyResources(Me.gpxSGHeaterOn, "gpxSGHeaterOn")
		Me.gpxSGHeaterOn.Name = "gpxSGHeaterOn"
		Me.gpxSGHeaterOn.TabStop = false
		'
		'lblSGHeaterOnResetDate
		'
		Me.lblSGHeaterOnResetDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		resources.ApplyResources(Me.lblSGHeaterOnResetDate, "lblSGHeaterOnResetDate")
		Me.lblSGHeaterOnResetDate.Name = "lblSGHeaterOnResetDate"
		'
		'lblSGHeaterResetDateL
		'
		resources.ApplyResources(Me.lblSGHeaterResetDateL, "lblSGHeaterResetDateL")
		Me.lblSGHeaterResetDateL.Name = "lblSGHeaterResetDateL"
		'
		'nudMainteThr5
		'
		Me.nudMainteThr5.Increment = New Decimal(New Integer() {1000, 0, 0, 0})
		resources.ApplyResources(Me.nudMainteThr5, "nudMainteThr5")
		Me.nudMainteThr5.Maximum = New Decimal(New Integer() {60000, 0, 0, 0})
		Me.nudMainteThr5.Name = "nudMainteThr5"
		'
		'lblSGHeaterPreVal
		'
		resources.ApplyResources(Me.lblSGHeaterPreVal, "lblSGHeaterPreVal")
		Me.lblSGHeaterPreVal.Name = "lblSGHeaterPreVal"
		'
		'lblSGHeaterThr
		'
		resources.ApplyResources(Me.lblSGHeaterThr, "lblSGHeaterThr")
		Me.lblSGHeaterThr.Name = "lblSGHeaterThr"
		'
		'btnSGHeaterTimeReset
		'
		resources.ApplyResources(Me.btnSGHeaterTimeReset, "btnSGHeaterTimeReset")
		Me.btnSGHeaterTimeReset.Name = "btnSGHeaterTimeReset"
		Me.btnSGHeaterTimeReset.UseVisualStyleBackColor = true
		'
		'lblSGHeatOnTime
		'
		Me.lblSGHeatOnTime.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		Me.lblSGHeatOnTime.ForeColor = System.Drawing.Color.Blue
		resources.ApplyResources(Me.lblSGHeatOnTime, "lblSGHeatOnTime")
		Me.lblSGHeatOnTime.Name = "lblSGHeatOnTime"
		'
		'gpxHotPlateHeaterOn
		'
		Me.gpxHotPlateHeaterOn.Controls.Add(Me.nudMainteThr2)
		Me.gpxHotPlateHeaterOn.Controls.Add(Me.lblHpHeaterThr)
		Me.gpxHotPlateHeaterOn.Controls.Add(Me.lblHeaterOnResetDate)
		Me.gpxHotPlateHeaterOn.Controls.Add(Me.btnHeatOnTimeReset)
		Me.gpxHotPlateHeaterOn.Controls.Add(Me.lblHeaterOnResetDateL)
		Me.gpxHotPlateHeaterOn.Controls.Add(Me.lblHeaterOnPreVal)
		Me.gpxHotPlateHeaterOn.Controls.Add(Me.lblDevHeatOnTime)
		resources.ApplyResources(Me.gpxHotPlateHeaterOn, "gpxHotPlateHeaterOn")
		Me.gpxHotPlateHeaterOn.Name = "gpxHotPlateHeaterOn"
		Me.gpxHotPlateHeaterOn.TabStop = false
		'
		'nudMainteThr2
		'
		Me.nudMainteThr2.Increment = New Decimal(New Integer() {1000, 0, 0, 0})
		resources.ApplyResources(Me.nudMainteThr2, "nudMainteThr2")
		Me.nudMainteThr2.Maximum = New Decimal(New Integer() {60000, 0, 0, 0})
		Me.nudMainteThr2.Name = "nudMainteThr2"
		'
		'lblHpHeaterThr
		'
		resources.ApplyResources(Me.lblHpHeaterThr, "lblHpHeaterThr")
		Me.lblHpHeaterThr.Name = "lblHpHeaterThr"
		'
		'lblHeaterOnResetDate
		'
		Me.lblHeaterOnResetDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		resources.ApplyResources(Me.lblHeaterOnResetDate, "lblHeaterOnResetDate")
		Me.lblHeaterOnResetDate.Name = "lblHeaterOnResetDate"
		'
		'btnHeatOnTimeReset
		'
		resources.ApplyResources(Me.btnHeatOnTimeReset, "btnHeatOnTimeReset")
		Me.btnHeatOnTimeReset.Name = "btnHeatOnTimeReset"
		Me.btnHeatOnTimeReset.UseVisualStyleBackColor = true
		'
		'lblHeaterOnResetDateL
		'
		resources.ApplyResources(Me.lblHeaterOnResetDateL, "lblHeaterOnResetDateL")
		Me.lblHeaterOnResetDateL.Name = "lblHeaterOnResetDateL"
		'
		'lblHeaterOnPreVal
		'
		resources.ApplyResources(Me.lblHeaterOnPreVal, "lblHeaterOnPreVal")
		Me.lblHeaterOnPreVal.Name = "lblHeaterOnPreVal"
		'
		'lblDevHeatOnTime
		'
		Me.lblDevHeatOnTime.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		Me.lblDevHeatOnTime.ForeColor = System.Drawing.Color.Blue
		resources.ApplyResources(Me.lblDevHeatOnTime, "lblDevHeatOnTime")
		Me.lblDevHeatOnTime.Name = "lblDevHeatOnTime"
		'
		'gbxPowOn
		'
		Me.gbxPowOn.Controls.Add(Me.lblPowOnResetDate)
		Me.gbxPowOn.Controls.Add(Me.lblPowOnResetDateL)
		Me.gbxPowOn.Controls.Add(Me.nudMainteThr1)
		Me.gbxPowOn.Controls.Add(Me.lblPowOnPreVal)
		Me.gbxPowOn.Controls.Add(Me.lblMainteThr1)
		Me.gbxPowOn.Controls.Add(Me.btnPowOnTimeReset)
		Me.gbxPowOn.Controls.Add(Me.lblDevPowOnTime)
		resources.ApplyResources(Me.gbxPowOn, "gbxPowOn")
		Me.gbxPowOn.Name = "gbxPowOn"
		Me.gbxPowOn.TabStop = false
		'
		'lblPowOnResetDate
		'
		Me.lblPowOnResetDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		resources.ApplyResources(Me.lblPowOnResetDate, "lblPowOnResetDate")
		Me.lblPowOnResetDate.Name = "lblPowOnResetDate"
		'
		'lblPowOnResetDateL
		'
		resources.ApplyResources(Me.lblPowOnResetDateL, "lblPowOnResetDateL")
		Me.lblPowOnResetDateL.Name = "lblPowOnResetDateL"
		'
		'nudMainteThr1
		'
		Me.nudMainteThr1.Increment = New Decimal(New Integer() {1000, 0, 0, 0})
		resources.ApplyResources(Me.nudMainteThr1, "nudMainteThr1")
		Me.nudMainteThr1.Maximum = New Decimal(New Integer() {60000, 0, 0, 0})
		Me.nudMainteThr1.Name = "nudMainteThr1"
		'
		'lblPowOnPreVal
		'
		resources.ApplyResources(Me.lblPowOnPreVal, "lblPowOnPreVal")
		Me.lblPowOnPreVal.Name = "lblPowOnPreVal"
		'
		'lblMainteThr1
		'
		resources.ApplyResources(Me.lblMainteThr1, "lblMainteThr1")
		Me.lblMainteThr1.Name = "lblMainteThr1"
		'
		'btnPowOnTimeReset
		'
		resources.ApplyResources(Me.btnPowOnTimeReset, "btnPowOnTimeReset")
		Me.btnPowOnTimeReset.Name = "btnPowOnTimeReset"
		Me.btnPowOnTimeReset.UseVisualStyleBackColor = true
		'
		'lblDevPowOnTime
		'
		Me.lblDevPowOnTime.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		Me.lblDevPowOnTime.ForeColor = System.Drawing.Color.Blue
		resources.ApplyResources(Me.lblDevPowOnTime, "lblDevPowOnTime")
		Me.lblDevPowOnTime.Name = "lblDevPowOnTime"
		'
		'gpxProbeHeaterOn
		'
		Me.gpxProbeHeaterOn.Controls.Add(Me.lblProbeHeaterOnResetDate)
		Me.gpxProbeHeaterOn.Controls.Add(Me.lblProbeHeaterResetDateL)
		Me.gpxProbeHeaterOn.Controls.Add(Me.nudMainteThr4)
		Me.gpxProbeHeaterOn.Controls.Add(Me.lblProbeHeaterPreVal)
		Me.gpxProbeHeaterOn.Controls.Add(Me.lblProbeHeaterThr)
		Me.gpxProbeHeaterOn.Controls.Add(Me.btnProbeHeaterTimeReset)
		Me.gpxProbeHeaterOn.Controls.Add(Me.lblProbeHeatOnTime)
		resources.ApplyResources(Me.gpxProbeHeaterOn, "gpxProbeHeaterOn")
		Me.gpxProbeHeaterOn.Name = "gpxProbeHeaterOn"
		Me.gpxProbeHeaterOn.TabStop = false
		'
		'lblProbeHeaterOnResetDate
		'
		Me.lblProbeHeaterOnResetDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		resources.ApplyResources(Me.lblProbeHeaterOnResetDate, "lblProbeHeaterOnResetDate")
		Me.lblProbeHeaterOnResetDate.Name = "lblProbeHeaterOnResetDate"
		'
		'lblProbeHeaterResetDateL
		'
		resources.ApplyResources(Me.lblProbeHeaterResetDateL, "lblProbeHeaterResetDateL")
		Me.lblProbeHeaterResetDateL.Name = "lblProbeHeaterResetDateL"
		'
		'nudMainteThr4
		'
		Me.nudMainteThr4.Increment = New Decimal(New Integer() {1000, 0, 0, 0})
		resources.ApplyResources(Me.nudMainteThr4, "nudMainteThr4")
		Me.nudMainteThr4.Maximum = New Decimal(New Integer() {60000, 0, 0, 0})
		Me.nudMainteThr4.Name = "nudMainteThr4"
		'
		'lblProbeHeaterPreVal
		'
		resources.ApplyResources(Me.lblProbeHeaterPreVal, "lblProbeHeaterPreVal")
		Me.lblProbeHeaterPreVal.Name = "lblProbeHeaterPreVal"
		'
		'lblProbeHeaterThr
		'
		resources.ApplyResources(Me.lblProbeHeaterThr, "lblProbeHeaterThr")
		Me.lblProbeHeaterThr.Name = "lblProbeHeaterThr"
		'
		'btnProbeHeaterTimeReset
		'
		resources.ApplyResources(Me.btnProbeHeaterTimeReset, "btnProbeHeaterTimeReset")
		Me.btnProbeHeaterTimeReset.Name = "btnProbeHeaterTimeReset"
		Me.btnProbeHeaterTimeReset.UseVisualStyleBackColor = true
		'
		'lblProbeHeatOnTime
		'
		Me.lblProbeHeatOnTime.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		Me.lblProbeHeatOnTime.ForeColor = System.Drawing.Color.Blue
		resources.ApplyResources(Me.lblProbeHeatOnTime, "lblProbeHeatOnTime")
		Me.lblProbeHeatOnTime.Name = "lblProbeHeatOnTime"
		'
		'gbxXYAct
		'
		Me.gbxXYAct.Controls.Add(Me.lblZActTime)
		Me.gbxXYAct.Controls.Add(Me.lblYActTime)
		Me.gbxXYAct.Controls.Add(Me.lblZActPreVal)
		Me.gbxXYAct.Controls.Add(Me.lblYActPreVal)
		Me.gbxXYAct.Controls.Add(Me.lblXYActResetDate)
		Me.gbxXYAct.Controls.Add(Me.lblXYActResetDateL)
		Me.gbxXYAct.Controls.Add(Me.nudMainteThr3)
		Me.gbxXYAct.Controls.Add(Me.lblXActPreVal)
		Me.gbxXYAct.Controls.Add(Me.lblXYActThr1)
		Me.gbxXYAct.Controls.Add(Me.btnXYActTimeReset)
		Me.gbxXYAct.Controls.Add(Me.lblXActTime)
		resources.ApplyResources(Me.gbxXYAct, "gbxXYAct")
		Me.gbxXYAct.Name = "gbxXYAct"
		Me.gbxXYAct.TabStop = false
		'
		'lblZActTime
		'
		Me.lblZActTime.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		Me.lblZActTime.ForeColor = System.Drawing.Color.Blue
		resources.ApplyResources(Me.lblZActTime, "lblZActTime")
		Me.lblZActTime.Name = "lblZActTime"
		'
		'lblYActTime
		'
		Me.lblYActTime.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		Me.lblYActTime.ForeColor = System.Drawing.Color.Blue
		resources.ApplyResources(Me.lblYActTime, "lblYActTime")
		Me.lblYActTime.Name = "lblYActTime"
		'
		'lblZActPreVal
		'
		resources.ApplyResources(Me.lblZActPreVal, "lblZActPreVal")
		Me.lblZActPreVal.Name = "lblZActPreVal"
		'
		'lblYActPreVal
		'
		resources.ApplyResources(Me.lblYActPreVal, "lblYActPreVal")
		Me.lblYActPreVal.Name = "lblYActPreVal"
		'
		'lblXYActResetDate
		'
		Me.lblXYActResetDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		resources.ApplyResources(Me.lblXYActResetDate, "lblXYActResetDate")
		Me.lblXYActResetDate.Name = "lblXYActResetDate"
		'
		'lblXYActResetDateL
		'
		resources.ApplyResources(Me.lblXYActResetDateL, "lblXYActResetDateL")
		Me.lblXYActResetDateL.Name = "lblXYActResetDateL"
		'
		'nudMainteThr3
		'
		Me.nudMainteThr3.Increment = New Decimal(New Integer() {1000, 0, 0, 0})
		resources.ApplyResources(Me.nudMainteThr3, "nudMainteThr3")
		Me.nudMainteThr3.Maximum = New Decimal(New Integer() {60000, 0, 0, 0})
		Me.nudMainteThr3.Name = "nudMainteThr3"
		'
		'lblXActPreVal
		'
		resources.ApplyResources(Me.lblXActPreVal, "lblXActPreVal")
		Me.lblXActPreVal.Name = "lblXActPreVal"
		'
		'lblXYActThr1
		'
		resources.ApplyResources(Me.lblXYActThr1, "lblXYActThr1")
		Me.lblXYActThr1.Name = "lblXYActThr1"
		'
		'btnXYActTimeReset
		'
		resources.ApplyResources(Me.btnXYActTimeReset, "btnXYActTimeReset")
		Me.btnXYActTimeReset.Name = "btnXYActTimeReset"
		Me.btnXYActTimeReset.UseVisualStyleBackColor = true
		'
		'lblXActTime
		'
		Me.lblXActTime.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		Me.lblXActTime.ForeColor = System.Drawing.Color.Blue
		resources.ApplyResources(Me.lblXActTime, "lblXActTime")
		Me.lblXActTime.Name = "lblXActTime"
		'
		'gbxMdkSet
		'
		Me.gbxMdkSet.Controls.Add(Me.Label33)
		Me.gbxMdkSet.Controls.Add(Me.lblStanbyTime)
		Me.gbxMdkSet.Controls.Add(Me.nudStandbyTime)
		Me.gbxMdkSet.Controls.Add(Me.Label15)
		Me.gbxMdkSet.Controls.Add(Me.cbxEnaIntLock)
		resources.ApplyResources(Me.gbxMdkSet, "gbxMdkSet")
		Me.gbxMdkSet.Name = "gbxMdkSet"
		Me.gbxMdkSet.TabStop = false
		'
		'Label33
		'
		resources.ApplyResources(Me.Label33, "Label33")
		Me.Label33.Name = "Label33"
		'
		'lblStanbyTime
		'
		resources.ApplyResources(Me.lblStanbyTime, "lblStanbyTime")
		Me.lblStanbyTime.Name = "lblStanbyTime"
		'
		'nudStandbyTime
		'
		resources.ApplyResources(Me.nudStandbyTime, "nudStandbyTime")
		Me.nudStandbyTime.Maximum = New Decimal(New Integer() {120, 0, 0, 0})
		Me.nudStandbyTime.Name = "nudStandbyTime"
		'
		'Label15
		'
		resources.ApplyResources(Me.Label15, "Label15")
		Me.Label15.Name = "Label15"
		'
		'cbxEnaIntLock
		'
		Me.cbxEnaIntLock.AutoCheck = false
		resources.ApplyResources(Me.cbxEnaIntLock, "cbxEnaIntLock")
		Me.cbxEnaIntLock.ForeColor = System.Drawing.Color.Black
		Me.cbxEnaIntLock.Name = "cbxEnaIntLock"
		Me.cbxEnaIntLock.UseVisualStyleBackColor = true
		'
		'GroupBox1
		'
		Me.GroupBox1.Controls.Add(Me.rbSChinese)
		Me.GroupBox1.Controls.Add(Me.rbKorean)
		Me.GroupBox1.Controls.Add(Me.rbTChinese)
		Me.GroupBox1.Controls.Add(Me.rbJPN)
		Me.GroupBox1.Controls.Add(Me.rbENG)
		resources.ApplyResources(Me.GroupBox1, "GroupBox1")
		Me.GroupBox1.Name = "GroupBox1"
		Me.GroupBox1.TabStop = false
		'
		'rbSChinese
		'
		resources.ApplyResources(Me.rbSChinese, "rbSChinese")
		Me.rbSChinese.Name = "rbSChinese"
		Me.rbSChinese.TabStop = true
		Me.rbSChinese.UseVisualStyleBackColor = true
		'
		'rbKorean
		'
		resources.ApplyResources(Me.rbKorean, "rbKorean")
		Me.rbKorean.Name = "rbKorean"
		Me.rbKorean.TabStop = true
		Me.rbKorean.UseVisualStyleBackColor = true
		'
		'rbTChinese
		'
		resources.ApplyResources(Me.rbTChinese, "rbTChinese")
		Me.rbTChinese.Name = "rbTChinese"
		Me.rbTChinese.TabStop = true
		Me.rbTChinese.UseVisualStyleBackColor = true
		'
		'rbJPN
		'
		resources.ApplyResources(Me.rbJPN, "rbJPN")
		Me.rbJPN.Name = "rbJPN"
		Me.rbJPN.TabStop = true
		Me.rbJPN.UseVisualStyleBackColor = true
		'
		'rbENG
		'
		resources.ApplyResources(Me.rbENG, "rbENG")
		Me.rbENG.Name = "rbENG"
		Me.rbENG.TabStop = true
		Me.rbENG.UseVisualStyleBackColor = true
		'
		'TabPage10
		'
		Me.TabPage10.Controls.Add(Me.gbxZeroAdj)
		Me.TabPage10.Controls.Add(Me.gbxZeroAdjMode)
		Me.TabPage10.Controls.Add(Me.gbxCentPosReg)
		Me.TabPage10.Controls.Add(Me.gbxHorizontalConf)
		Me.TabPage10.Controls.Add(Me.gbxXYModeDrive)
		Me.TabPage10.Controls.Add(Me.gbxManualDrive)
		Me.TabPage10.Controls.Add(Me.gbxCentering)
		resources.ApplyResources(Me.TabPage10, "TabPage10")
		Me.TabPage10.Name = "TabPage10"
		Me.TabPage10.UseVisualStyleBackColor = true
		'
		'gbxZeroAdj
		'
		Me.gbxZeroAdj.Controls.Add(Me.lblStartPosHP)
		Me.gbxZeroAdj.Controls.Add(Me.nudZeroStartPosHP)
		Me.gbxZeroAdj.Controls.Add(Me.Label35)
		Me.gbxZeroAdj.Controls.Add(Me.lblStaFlacRange)
		Me.gbxZeroAdj.Controls.Add(Me.lblStaMonTime)
		Me.gbxZeroAdj.Controls.Add(Me.nudStaMonTime)
		Me.gbxZeroAdj.Controls.Add(Me.nudStaFlacRange)
		Me.gbxZeroAdj.Controls.Add(Me.Label30)
		Me.gbxZeroAdj.Controls.Add(Me.Label13)
		resources.ApplyResources(Me.gbxZeroAdj, "gbxZeroAdj")
		Me.gbxZeroAdj.Name = "gbxZeroAdj"
		Me.gbxZeroAdj.TabStop = false
		'
		'lblStartPosHP
		'
		resources.ApplyResources(Me.lblStartPosHP, "lblStartPosHP")
		Me.lblStartPosHP.Name = "lblStartPosHP"
		'
		'nudZeroStartPosHP
		'
		Me.nudZeroStartPosHP.DecimalPlaces = 3
		Me.nudZeroStartPosHP.Increment = New Decimal(New Integer() {1, 0, 0, 196608})
		resources.ApplyResources(Me.nudZeroStartPosHP, "nudZeroStartPosHP")
		Me.nudZeroStartPosHP.Maximum = New Decimal(New Integer() {50000, 0, 0, 196608})
		Me.nudZeroStartPosHP.Name = "nudZeroStartPosHP"
		Me.nudZeroStartPosHP.Value = New Decimal(New Integer() {1, 0, 0, 0})
		'
		'Label35
		'
		resources.ApplyResources(Me.Label35, "Label35")
		Me.Label35.Name = "Label35"
		'
		'lblStaFlacRange
		'
		resources.ApplyResources(Me.lblStaFlacRange, "lblStaFlacRange")
		Me.lblStaFlacRange.Name = "lblStaFlacRange"
		'
		'lblStaMonTime
		'
		resources.ApplyResources(Me.lblStaMonTime, "lblStaMonTime")
		Me.lblStaMonTime.Name = "lblStaMonTime"
		'
		'nudStaMonTime
		'
		resources.ApplyResources(Me.nudStaMonTime, "nudStaMonTime")
		Me.nudStaMonTime.Maximum = New Decimal(New Integer() {1000, 0, 0, 0})
		Me.nudStaMonTime.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
		Me.nudStaMonTime.Name = "nudStaMonTime"
		Me.nudStaMonTime.Value = New Decimal(New Integer() {1, 0, 0, 0})
		'
		'nudStaFlacRange
		'
		resources.ApplyResources(Me.nudStaFlacRange, "nudStaFlacRange")
		Me.nudStaFlacRange.Maximum = New Decimal(New Integer() {1000, 0, 0, 0})
		Me.nudStaFlacRange.Minimum = New Decimal(New Integer() {5, 0, 0, 0})
		Me.nudStaFlacRange.Name = "nudStaFlacRange"
		Me.nudStaFlacRange.Value = New Decimal(New Integer() {50, 0, 0, 0})
		'
		'Label30
		'
		resources.ApplyResources(Me.Label30, "Label30")
		Me.Label30.Name = "Label30"
		'
		'Label13
		'
		resources.ApplyResources(Me.Label13, "Label13")
		Me.Label13.Name = "Label13"
		'
		'gbxZeroAdjMode
		'
		Me.gbxZeroAdjMode.Controls.Add(Me.rbZeroZcont)
		Me.gbxZeroAdjMode.Controls.Add(Me.rbZeroXcont)
		resources.ApplyResources(Me.gbxZeroAdjMode, "gbxZeroAdjMode")
		Me.gbxZeroAdjMode.Name = "gbxZeroAdjMode"
		Me.gbxZeroAdjMode.TabStop = false
		'
		'rbZeroZcont
		'
		resources.ApplyResources(Me.rbZeroZcont, "rbZeroZcont")
		Me.rbZeroZcont.Checked = true
		Me.rbZeroZcont.Name = "rbZeroZcont"
		Me.rbZeroZcont.TabStop = true
		Me.rbZeroZcont.UseVisualStyleBackColor = true
		'
		'rbZeroXcont
		'
		resources.ApplyResources(Me.rbZeroXcont, "rbZeroXcont")
		Me.rbZeroXcont.Name = "rbZeroXcont"
		Me.rbZeroXcont.UseVisualStyleBackColor = true
		'
		'gbxCentPosReg
		'
		Me.gbxCentPosReg.Controls.Add(Me.lblUnit11)
		Me.gbxCentPosReg.Controls.Add(Me.lblUnit10)
		Me.gbxCentPosReg.Controls.Add(Me.btnCenterPosReg)
		Me.gbxCentPosReg.Controls.Add(Me.nudCenterY)
		Me.gbxCentPosReg.Controls.Add(Me.nudCenterX)
		Me.gbxCentPosReg.Controls.Add(Me.lblCentRegY)
		Me.gbxCentPosReg.Controls.Add(Me.lblCentRegX)
		resources.ApplyResources(Me.gbxCentPosReg, "gbxCentPosReg")
		Me.gbxCentPosReg.Name = "gbxCentPosReg"
		Me.gbxCentPosReg.TabStop = false
		'
		'lblUnit11
		'
		resources.ApplyResources(Me.lblUnit11, "lblUnit11")
		Me.lblUnit11.Name = "lblUnit11"
		'
		'lblUnit10
		'
		resources.ApplyResources(Me.lblUnit10, "lblUnit10")
		Me.lblUnit10.Name = "lblUnit10"
		'
		'btnCenterPosReg
		'
		resources.ApplyResources(Me.btnCenterPosReg, "btnCenterPosReg")
		Me.btnCenterPosReg.Name = "btnCenterPosReg"
		Me.btnCenterPosReg.UseVisualStyleBackColor = true
		'
		'nudCenterY
		'
		resources.ApplyResources(Me.nudCenterY, "nudCenterY")
		Me.nudCenterY.Maximum = New Decimal(New Integer() {32767, 0, 0, 0})
		Me.nudCenterY.Minimum = New Decimal(New Integer() {32767, 0, 0, -2147483648})
		Me.nudCenterY.Name = "nudCenterY"
		'
		'nudCenterX
		'
		resources.ApplyResources(Me.nudCenterX, "nudCenterX")
		Me.nudCenterX.Maximum = New Decimal(New Integer() {32767, 0, 0, 0})
		Me.nudCenterX.Minimum = New Decimal(New Integer() {32767, 0, 0, -2147483648})
		Me.nudCenterX.Name = "nudCenterX"
		'
		'lblCentRegY
		'
		resources.ApplyResources(Me.lblCentRegY, "lblCentRegY")
		Me.lblCentRegY.Name = "lblCentRegY"
		'
		'lblCentRegX
		'
		resources.ApplyResources(Me.lblCentRegX, "lblCentRegX")
		Me.lblCentRegX.Name = "lblCentRegX"
		'
		'gbxHorizontalConf
		'
		Me.gbxHorizontalConf.Controls.Add(Me.btnHorizonConfirm)
		resources.ApplyResources(Me.gbxHorizontalConf, "gbxHorizontalConf")
		Me.gbxHorizontalConf.Name = "gbxHorizontalConf"
		Me.gbxHorizontalConf.TabStop = false
		'
		'btnHorizonConfirm
		'
		resources.ApplyResources(Me.btnHorizonConfirm, "btnHorizonConfirm")
		Me.btnHorizonConfirm.Name = "btnHorizonConfirm"
		Me.btnHorizonConfirm.UseVisualStyleBackColor = true
		'
		'gbxXYModeDrive
		'
		Me.gbxXYModeDrive.Controls.Add(Me.Label1)
		Me.gbxXYModeDrive.Controls.Add(Me.lblXYModeZ)
		Me.gbxXYModeDrive.Controls.Add(Me.nudZPeriod)
		Me.gbxXYModeDrive.Controls.Add(Me.nudZAmp)
		Me.gbxXYModeDrive.Controls.Add(Me.Label5)
		Me.gbxXYModeDrive.Controls.Add(Me.Label44)
		Me.gbxXYModeDrive.Controls.Add(Me.Label40)
		Me.gbxXYModeDrive.Controls.Add(Me.lblXYModeY)
		Me.gbxXYModeDrive.Controls.Add(Me.lblXYModeX)
		Me.gbxXYModeDrive.Controls.Add(Me.nudYPeriod)
		Me.gbxXYModeDrive.Controls.Add(Me.nudXPeriod)
		Me.gbxXYModeDrive.Controls.Add(Me.nudYAmp)
		Me.gbxXYModeDrive.Controls.Add(Me.nudXAmp)
		Me.gbxXYModeDrive.Controls.Add(Me.btnXYStop)
		Me.gbxXYModeDrive.Controls.Add(Me.btnXYDrive)
		Me.gbxXYModeDrive.Controls.Add(Me.Label42)
		Me.gbxXYModeDrive.Controls.Add(Me.Label41)
		resources.ApplyResources(Me.gbxXYModeDrive, "gbxXYModeDrive")
		Me.gbxXYModeDrive.Name = "gbxXYModeDrive"
		Me.gbxXYModeDrive.TabStop = false
		'
		'Label1
		'
		resources.ApplyResources(Me.Label1, "Label1")
		Me.Label1.Name = "Label1"
		'
		'lblXYModeZ
		'
		resources.ApplyResources(Me.lblXYModeZ, "lblXYModeZ")
		Me.lblXYModeZ.Name = "lblXYModeZ"
		'
		'nudZPeriod
		'
		Me.nudZPeriod.DecimalPlaces = 1
		Me.nudZPeriod.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
		resources.ApplyResources(Me.nudZPeriod, "nudZPeriod")
		Me.nudZPeriod.Maximum = New Decimal(New Integer() {10, 0, 0, 0})
		Me.nudZPeriod.Minimum = New Decimal(New Integer() {1, 0, 0, 65536})
		Me.nudZPeriod.Name = "nudZPeriod"
		Me.nudZPeriod.Value = New Decimal(New Integer() {1, 0, 0, 65536})
		'
		'nudZAmp
		'
		resources.ApplyResources(Me.nudZAmp, "nudZAmp")
		Me.nudZAmp.Maximum = New Decimal(New Integer() {10000, 0, 0, 0})
		Me.nudZAmp.Name = "nudZAmp"
		'
		'Label5
		'
		resources.ApplyResources(Me.Label5, "Label5")
		Me.Label5.Name = "Label5"
		'
		'Label44
		'
		resources.ApplyResources(Me.Label44, "Label44")
		Me.Label44.Name = "Label44"
		'
		'Label40
		'
		resources.ApplyResources(Me.Label40, "Label40")
		Me.Label40.Name = "Label40"
		'
		'lblXYModeY
		'
		resources.ApplyResources(Me.lblXYModeY, "lblXYModeY")
		Me.lblXYModeY.Name = "lblXYModeY"
		'
		'lblXYModeX
		'
		resources.ApplyResources(Me.lblXYModeX, "lblXYModeX")
		Me.lblXYModeX.Name = "lblXYModeX"
		'
		'nudYPeriod
		'
		Me.nudYPeriod.DecimalPlaces = 1
		Me.nudYPeriod.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
		resources.ApplyResources(Me.nudYPeriod, "nudYPeriod")
		Me.nudYPeriod.Maximum = New Decimal(New Integer() {10, 0, 0, 0})
		Me.nudYPeriod.Minimum = New Decimal(New Integer() {1, 0, 0, 65536})
		Me.nudYPeriod.Name = "nudYPeriod"
		Me.nudYPeriod.Value = New Decimal(New Integer() {1, 0, 0, 65536})
		'
		'nudXPeriod
		'
		Me.nudXPeriod.DecimalPlaces = 1
		Me.nudXPeriod.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
		resources.ApplyResources(Me.nudXPeriod, "nudXPeriod")
		Me.nudXPeriod.Maximum = New Decimal(New Integer() {10, 0, 0, 0})
		Me.nudXPeriod.Minimum = New Decimal(New Integer() {1, 0, 0, 65536})
		Me.nudXPeriod.Name = "nudXPeriod"
		Me.nudXPeriod.Value = New Decimal(New Integer() {1, 0, 0, 65536})
		'
		'nudYAmp
		'
		resources.ApplyResources(Me.nudYAmp, "nudYAmp")
		Me.nudYAmp.Maximum = New Decimal(New Integer() {10000, 0, 0, 0})
		Me.nudYAmp.Name = "nudYAmp"
		'
		'nudXAmp
		'
		resources.ApplyResources(Me.nudXAmp, "nudXAmp")
		Me.nudXAmp.Maximum = New Decimal(New Integer() {10000, 0, 0, 0})
		Me.nudXAmp.Name = "nudXAmp"
		'
		'btnXYStop
		'
		resources.ApplyResources(Me.btnXYStop, "btnXYStop")
		Me.btnXYStop.Name = "btnXYStop"
		Me.btnXYStop.UseVisualStyleBackColor = true
		'
		'btnXYDrive
		'
		resources.ApplyResources(Me.btnXYDrive, "btnXYDrive")
		Me.btnXYDrive.Name = "btnXYDrive"
		Me.btnXYDrive.UseVisualStyleBackColor = true
		'
		'Label42
		'
		resources.ApplyResources(Me.Label42, "Label42")
		Me.Label42.Name = "Label42"
		'
		'Label41
		'
		resources.ApplyResources(Me.Label41, "Label41")
		Me.Label41.Name = "Label41"
		'
		'gbxManualDrive
		'
		Me.gbxManualDrive.Controls.Add(Me.btnOrigin)
		Me.gbxManualDrive.Controls.Add(Me.btnYAxisDrive)
		Me.gbxManualDrive.Controls.Add(Me.btnXAxisDrive)
		Me.gbxManualDrive.Controls.Add(Me.btnHotPlateDrive)
		Me.gbxManualDrive.Controls.Add(Me.Label29)
		Me.gbxManualDrive.Controls.Add(Me.Label28)
		Me.gbxManualDrive.Controls.Add(Me.Label27)
		Me.gbxManualDrive.Controls.Add(Me.lblYAxisPos)
		Me.gbxManualDrive.Controls.Add(Me.lblXAxisPos)
		Me.gbxManualDrive.Controls.Add(Me.lblHeatPos)
		Me.gbxManualDrive.Controls.Add(Me.nudYAxisPos)
		Me.gbxManualDrive.Controls.Add(Me.nudXAxisPos)
		Me.gbxManualDrive.Controls.Add(Me.nudHeatPos)
		resources.ApplyResources(Me.gbxManualDrive, "gbxManualDrive")
		Me.gbxManualDrive.Name = "gbxManualDrive"
		Me.gbxManualDrive.TabStop = false
		'
		'btnOrigin
		'
		resources.ApplyResources(Me.btnOrigin, "btnOrigin")
		Me.btnOrigin.Name = "btnOrigin"
		Me.btnOrigin.UseVisualStyleBackColor = true
		'
		'btnYAxisDrive
		'
		resources.ApplyResources(Me.btnYAxisDrive, "btnYAxisDrive")
		Me.btnYAxisDrive.Name = "btnYAxisDrive"
		Me.btnYAxisDrive.UseVisualStyleBackColor = true
		'
		'btnXAxisDrive
		'
		resources.ApplyResources(Me.btnXAxisDrive, "btnXAxisDrive")
		Me.btnXAxisDrive.Name = "btnXAxisDrive"
		Me.btnXAxisDrive.UseVisualStyleBackColor = true
		'
		'btnHotPlateDrive
		'
		resources.ApplyResources(Me.btnHotPlateDrive, "btnHotPlateDrive")
		Me.btnHotPlateDrive.Name = "btnHotPlateDrive"
		Me.btnHotPlateDrive.UseVisualStyleBackColor = true
		'
		'Label29
		'
		resources.ApplyResources(Me.Label29, "Label29")
		Me.Label29.Name = "Label29"
		'
		'Label28
		'
		resources.ApplyResources(Me.Label28, "Label28")
		Me.Label28.Name = "Label28"
		'
		'Label27
		'
		resources.ApplyResources(Me.Label27, "Label27")
		Me.Label27.Name = "Label27"
		'
		'lblYAxisPos
		'
		resources.ApplyResources(Me.lblYAxisPos, "lblYAxisPos")
		Me.lblYAxisPos.Name = "lblYAxisPos"
		'
		'lblXAxisPos
		'
		resources.ApplyResources(Me.lblXAxisPos, "lblXAxisPos")
		Me.lblXAxisPos.Name = "lblXAxisPos"
		'
		'lblHeatPos
		'
		resources.ApplyResources(Me.lblHeatPos, "lblHeatPos")
		Me.lblHeatPos.Name = "lblHeatPos"
		'
		'nudYAxisPos
		'
		resources.ApplyResources(Me.nudYAxisPos, "nudYAxisPos")
		Me.nudYAxisPos.Maximum = New Decimal(New Integer() {32767, 0, 0, 0})
		Me.nudYAxisPos.Minimum = New Decimal(New Integer() {32767, 0, 0, -2147483648})
		Me.nudYAxisPos.Name = "nudYAxisPos"
		'
		'nudXAxisPos
		'
		resources.ApplyResources(Me.nudXAxisPos, "nudXAxisPos")
		Me.nudXAxisPos.Maximum = New Decimal(New Integer() {32767, 0, 0, 0})
		Me.nudXAxisPos.Minimum = New Decimal(New Integer() {32767, 0, 0, -2147483648})
		Me.nudXAxisPos.Name = "nudXAxisPos"
		'
		'nudHeatPos
		'
		Me.nudHeatPos.DecimalPlaces = 3
		Me.nudHeatPos.Increment = New Decimal(New Integer() {1, 0, 0, 196608})
		resources.ApplyResources(Me.nudHeatPos, "nudHeatPos")
		Me.nudHeatPos.Maximum = New Decimal(New Integer() {50000, 0, 0, 196608})
		Me.nudHeatPos.Name = "nudHeatPos"
		'
		'gbxCentering
		'
		Me.gbxCentering.Controls.Add(Me.Label39)
		Me.gbxCentering.Controls.Add(Me.Label38)
		Me.gbxCentering.Controls.Add(Me.Label37)
		Me.gbxCentering.Controls.Add(Me.Label36)
		Me.gbxCentering.Controls.Add(Me.lblCenterY)
		Me.gbxCentering.Controls.Add(Me.lblCenterX)
		Me.gbxCentering.Controls.Add(Me.lblCentPointPos)
		Me.gbxCentering.Controls.Add(Me.lblCentStartPosHP)
		Me.gbxCentering.Controls.Add(Me.nudCentStartPosHP)
		Me.gbxCentering.Controls.Add(Me.Label34)
		Me.gbxCentering.Controls.Add(Me.btnExecCenter)
		resources.ApplyResources(Me.gbxCentering, "gbxCentering")
		Me.gbxCentering.Name = "gbxCentering"
		Me.gbxCentering.TabStop = false
		'
		'Label39
		'
		resources.ApplyResources(Me.Label39, "Label39")
		Me.Label39.Name = "Label39"
		'
		'Label38
		'
		resources.ApplyResources(Me.Label38, "Label38")
		Me.Label38.Name = "Label38"
		'
		'Label37
		'
		resources.ApplyResources(Me.Label37, "Label37")
		Me.Label37.Name = "Label37"
		'
		'Label36
		'
		resources.ApplyResources(Me.Label36, "Label36")
		Me.Label36.Name = "Label36"
		'
		'lblCenterY
		'
		Me.lblCenterY.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		Me.lblCenterY.FlatStyle = System.Windows.Forms.FlatStyle.Flat
		resources.ApplyResources(Me.lblCenterY, "lblCenterY")
		Me.lblCenterY.Name = "lblCenterY"
		'
		'lblCenterX
		'
		Me.lblCenterX.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		Me.lblCenterX.CausesValidation = false
		Me.lblCenterX.FlatStyle = System.Windows.Forms.FlatStyle.Flat
		resources.ApplyResources(Me.lblCenterX, "lblCenterX")
		Me.lblCenterX.Name = "lblCenterX"
		'
		'lblCentPointPos
		'
		resources.ApplyResources(Me.lblCentPointPos, "lblCentPointPos")
		Me.lblCentPointPos.Name = "lblCentPointPos"
		'
		'lblCentStartPosHP
		'
		resources.ApplyResources(Me.lblCentStartPosHP, "lblCentStartPosHP")
		Me.lblCentStartPosHP.Name = "lblCentStartPosHP"
		'
		'nudCentStartPosHP
		'
		Me.nudCentStartPosHP.DecimalPlaces = 3
		Me.nudCentStartPosHP.Increment = New Decimal(New Integer() {1, 0, 0, 196608})
		resources.ApplyResources(Me.nudCentStartPosHP, "nudCentStartPosHP")
		Me.nudCentStartPosHP.Maximum = New Decimal(New Integer() {50000, 0, 0, 196608})
		Me.nudCentStartPosHP.Name = "nudCentStartPosHP"
		Me.nudCentStartPosHP.Value = New Decimal(New Integer() {1, 0, 0, 0})
		'
		'Label34
		'
		resources.ApplyResources(Me.Label34, "Label34")
		Me.Label34.Name = "Label34"
		'
		'btnExecCenter
		'
		resources.ApplyResources(Me.btnExecCenter, "btnExecCenter")
		Me.btnExecCenter.Name = "btnExecCenter"
		Me.btnExecCenter.UseVisualStyleBackColor = true
		'
		'TabPage2
		'
		Me.TabPage2.Controls.Add(Me.grpDefClr)
		Me.TabPage2.Controls.Add(Me.gbxMarker)
		Me.TabPage2.Controls.Add(Me.gbxStressUnit)
		Me.TabPage2.Controls.Add(Me.gbxGraphYSpan)
		Me.TabPage2.Controls.Add(Me.gbxGraphXSpan)
		Me.TabPage2.Controls.Add(Me.GroupBox14)
		Me.TabPage2.Controls.Add(Me.GroupBox7)
		Me.TabPage2.Controls.Add(Me.GroupBox4)
		Me.TabPage2.Controls.Add(Me.GroupBox3)
		resources.ApplyResources(Me.TabPage2, "TabPage2")
		Me.TabPage2.Name = "TabPage2"
		Me.TabPage2.UseVisualStyleBackColor = true
		'
		'grpDefClr
		'
		Me.grpDefClr.Controls.Add(Me.lblColorAssignDesc)
		Me.grpDefClr.Controls.Add(Me.lblClrL)
		Me.grpDefClr.Controls.Add(Me.lblClrB)
		Me.grpDefClr.Controls.Add(Me.lblClr0)
		Me.grpDefClr.Controls.Add(Me.lblClr9)
		Me.grpDefClr.Controls.Add(Me.lblClr8)
		Me.grpDefClr.Controls.Add(Me.lblClr7)
		Me.grpDefClr.Controls.Add(Me.lblClr6)
		Me.grpDefClr.Controls.Add(Me.lblClr5)
		Me.grpDefClr.Controls.Add(Me.lblClr4)
		Me.grpDefClr.Controls.Add(Me.lblClr3)
		Me.grpDefClr.Controls.Add(Me.lblClr2)
		Me.grpDefClr.Controls.Add(Me.lblClr1)
		Me.grpDefClr.Controls.Add(Me.cbY2Opp)
		resources.ApplyResources(Me.grpDefClr, "grpDefClr")
		Me.grpDefClr.Name = "grpDefClr"
		Me.grpDefClr.TabStop = false
		'
		'lblColorAssignDesc
		'
		resources.ApplyResources(Me.lblColorAssignDesc, "lblColorAssignDesc")
		Me.lblColorAssignDesc.Name = "lblColorAssignDesc"
		'
		'lblClrL
		'
		Me.lblClrL.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		Me.lblClrL.Cursor = System.Windows.Forms.Cursors.Hand
		resources.ApplyResources(Me.lblClrL, "lblClrL")
		Me.lblClrL.Name = "lblClrL"
		'
		'lblClrB
		'
		Me.lblClrB.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		Me.lblClrB.Cursor = System.Windows.Forms.Cursors.Hand
		resources.ApplyResources(Me.lblClrB, "lblClrB")
		Me.lblClrB.Name = "lblClrB"
		'
		'lblClr0
		'
		Me.lblClr0.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		Me.lblClr0.Cursor = System.Windows.Forms.Cursors.Hand
		resources.ApplyResources(Me.lblClr0, "lblClr0")
		Me.lblClr0.Name = "lblClr0"
		'
		'lblClr9
		'
		Me.lblClr9.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		Me.lblClr9.Cursor = System.Windows.Forms.Cursors.Hand
		resources.ApplyResources(Me.lblClr9, "lblClr9")
		Me.lblClr9.Name = "lblClr9"
		'
		'lblClr8
		'
		Me.lblClr8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		Me.lblClr8.Cursor = System.Windows.Forms.Cursors.Hand
		resources.ApplyResources(Me.lblClr8, "lblClr8")
		Me.lblClr8.Name = "lblClr8"
		'
		'lblClr7
		'
		Me.lblClr7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		Me.lblClr7.Cursor = System.Windows.Forms.Cursors.Hand
		resources.ApplyResources(Me.lblClr7, "lblClr7")
		Me.lblClr7.Name = "lblClr7"
		'
		'lblClr6
		'
		Me.lblClr6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		Me.lblClr6.Cursor = System.Windows.Forms.Cursors.Hand
		resources.ApplyResources(Me.lblClr6, "lblClr6")
		Me.lblClr6.Name = "lblClr6"
		'
		'lblClr5
		'
		Me.lblClr5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		Me.lblClr5.Cursor = System.Windows.Forms.Cursors.Hand
		resources.ApplyResources(Me.lblClr5, "lblClr5")
		Me.lblClr5.Name = "lblClr5"
		'
		'lblClr4
		'
		Me.lblClr4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		Me.lblClr4.Cursor = System.Windows.Forms.Cursors.Hand
		resources.ApplyResources(Me.lblClr4, "lblClr4")
		Me.lblClr4.Name = "lblClr4"
		'
		'lblClr3
		'
		Me.lblClr3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		Me.lblClr3.Cursor = System.Windows.Forms.Cursors.Hand
		resources.ApplyResources(Me.lblClr3, "lblClr3")
		Me.lblClr3.Name = "lblClr3"
		'
		'lblClr2
		'
		Me.lblClr2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		Me.lblClr2.Cursor = System.Windows.Forms.Cursors.Hand
		resources.ApplyResources(Me.lblClr2, "lblClr2")
		Me.lblClr2.Name = "lblClr2"
		'
		'lblClr1
		'
		Me.lblClr1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		Me.lblClr1.Cursor = System.Windows.Forms.Cursors.Hand
		resources.ApplyResources(Me.lblClr1, "lblClr1")
		Me.lblClr1.Name = "lblClr1"
		'
		'cbY2Opp
		'
		resources.ApplyResources(Me.cbY2Opp, "cbY2Opp")
		Me.cbY2Opp.Name = "cbY2Opp"
		Me.cbY2Opp.UseVisualStyleBackColor = true
		'
		'gbxMarker
		'
		Me.gbxMarker.Controls.Add(Me.nudMarkerSize)
		Me.gbxMarker.Controls.Add(Me.lblMarkerSize)
		Me.gbxMarker.Controls.Add(Me.lblMarkerStyle)
		Me.gbxMarker.Controls.Add(Me.cbxMarkerStyle)
		resources.ApplyResources(Me.gbxMarker, "gbxMarker")
		Me.gbxMarker.Name = "gbxMarker"
		Me.gbxMarker.TabStop = false
		'
		'nudMarkerSize
		'
		resources.ApplyResources(Me.nudMarkerSize, "nudMarkerSize")
		Me.nudMarkerSize.Maximum = New Decimal(New Integer() {10, 0, 0, 0})
		Me.nudMarkerSize.Name = "nudMarkerSize"
		'
		'lblMarkerSize
		'
		resources.ApplyResources(Me.lblMarkerSize, "lblMarkerSize")
		Me.lblMarkerSize.Name = "lblMarkerSize"
		'
		'lblMarkerStyle
		'
		resources.ApplyResources(Me.lblMarkerStyle, "lblMarkerStyle")
		Me.lblMarkerStyle.Name = "lblMarkerStyle"
		'
		'cbxMarkerStyle
		'
		Me.cbxMarkerStyle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.cbxMarkerStyle.FormattingEnabled = true
		resources.ApplyResources(Me.cbxMarkerStyle, "cbxMarkerStyle")
		Me.cbxMarkerStyle.Name = "cbxMarkerStyle"
		'
		'gbxStressUnit
		'
		Me.gbxStressUnit.Controls.Add(Me.Label17)
		Me.gbxStressUnit.Controls.Add(Me.rbtnNewton)
		Me.gbxStressUnit.Controls.Add(Me.rbtnDigital)
		Me.gbxStressUnit.Controls.Add(Me.rbtnPer)
		resources.ApplyResources(Me.gbxStressUnit, "gbxStressUnit")
		Me.gbxStressUnit.Name = "gbxStressUnit"
		Me.gbxStressUnit.TabStop = false
		'
		'Label17
		'
		resources.ApplyResources(Me.Label17, "Label17")
		Me.Label17.Name = "Label17"
		'
		'rbtnNewton
		'
		resources.ApplyResources(Me.rbtnNewton, "rbtnNewton")
		Me.rbtnNewton.Name = "rbtnNewton"
		Me.rbtnNewton.TabStop = true
		Me.rbtnNewton.UseVisualStyleBackColor = true
		'
		'rbtnDigital
		'
		resources.ApplyResources(Me.rbtnDigital, "rbtnDigital")
		Me.rbtnDigital.Name = "rbtnDigital"
		Me.rbtnDigital.TabStop = true
		Me.rbtnDigital.UseVisualStyleBackColor = true
		'
		'rbtnPer
		'
		resources.ApplyResources(Me.rbtnPer, "rbtnPer")
		Me.rbtnPer.Name = "rbtnPer"
		Me.rbtnPer.TabStop = true
		Me.rbtnPer.UseVisualStyleBackColor = true
		'
		'gbxGraphYSpan
		'
		Me.gbxGraphYSpan.Controls.Add(Me.rbRYauto)
		Me.gbxGraphYSpan.Controls.Add(Me.rbRYFix)
		resources.ApplyResources(Me.gbxGraphYSpan, "gbxGraphYSpan")
		Me.gbxGraphYSpan.Name = "gbxGraphYSpan"
		Me.gbxGraphYSpan.TabStop = false
		'
		'rbRYauto
		'
		resources.ApplyResources(Me.rbRYauto, "rbRYauto")
		Me.rbRYauto.Name = "rbRYauto"
		Me.rbRYauto.TabStop = true
		Me.rbRYauto.UseVisualStyleBackColor = true
		'
		'rbRYFix
		'
		resources.ApplyResources(Me.rbRYFix, "rbRYFix")
		Me.rbRYFix.Name = "rbRYFix"
		Me.rbRYFix.TabStop = true
		Me.rbRYFix.UseVisualStyleBackColor = true
		'
		'gbxGraphXSpan
		'
		Me.gbxGraphXSpan.Controls.Add(Me.rbRXauto)
		Me.gbxGraphXSpan.Controls.Add(Me.rbRXFix)
		resources.ApplyResources(Me.gbxGraphXSpan, "gbxGraphXSpan")
		Me.gbxGraphXSpan.Name = "gbxGraphXSpan"
		Me.gbxGraphXSpan.TabStop = false
		'
		'rbRXauto
		'
		resources.ApplyResources(Me.rbRXauto, "rbRXauto")
		Me.rbRXauto.Name = "rbRXauto"
		Me.rbRXauto.TabStop = true
		Me.rbRXauto.UseVisualStyleBackColor = true
		'
		'rbRXFix
		'
		resources.ApplyResources(Me.rbRXFix, "rbRXFix")
		Me.rbRXFix.Name = "rbRXFix"
		Me.rbRXFix.TabStop = true
		Me.rbRXFix.UseVisualStyleBackColor = true
		'
		'GroupBox14
		'
		Me.GroupBox14.Controls.Add(Me.rbARDFill)
		Me.GroupBox14.Controls.Add(Me.rbARDTransparent)
		resources.ApplyResources(Me.GroupBox14, "GroupBox14")
		Me.GroupBox14.Name = "GroupBox14"
		Me.GroupBox14.TabStop = false
		'
		'rbARDFill
		'
		resources.ApplyResources(Me.rbARDFill, "rbARDFill")
		Me.rbARDFill.Name = "rbARDFill"
		Me.rbARDFill.TabStop = true
		Me.rbARDFill.UseVisualStyleBackColor = true
		'
		'rbARDTransparent
		'
		resources.ApplyResources(Me.rbARDTransparent, "rbARDTransparent")
		Me.rbARDTransparent.Name = "rbARDTransparent"
		Me.rbARDTransparent.TabStop = true
		Me.rbARDTransparent.UseVisualStyleBackColor = true
		'
		'GroupBox7
		'
		Me.GroupBox7.Controls.Add(Me.lblScrollWidth)
		Me.GroupBox7.Controls.Add(Me.lblThinningD)
		Me.GroupBox7.Controls.Add(Me.lblThinningR)
		Me.GroupBox7.Controls.Add(Me.nudScrolWidth)
		Me.GroupBox7.Controls.Add(Me.nudThinningValueD)
		Me.GroupBox7.Controls.Add(Me.nudThinningValueR)
		resources.ApplyResources(Me.GroupBox7, "GroupBox7")
		Me.GroupBox7.Name = "GroupBox7"
		Me.GroupBox7.TabStop = false
		'
		'lblScrollWidth
		'
		resources.ApplyResources(Me.lblScrollWidth, "lblScrollWidth")
		Me.lblScrollWidth.Name = "lblScrollWidth"
		'
		'lblThinningD
		'
		resources.ApplyResources(Me.lblThinningD, "lblThinningD")
		Me.lblThinningD.Name = "lblThinningD"
		'
		'lblThinningR
		'
		resources.ApplyResources(Me.lblThinningR, "lblThinningR")
		Me.lblThinningR.Name = "lblThinningR"
		'
		'nudScrolWidth
		'
		Me.nudScrolWidth.Increment = New Decimal(New Integer() {10, 0, 0, 0})
		resources.ApplyResources(Me.nudScrolWidth, "nudScrolWidth")
		Me.nudScrolWidth.Maximum = New Decimal(New Integer() {50000, 0, 0, 0})
		Me.nudScrolWidth.Minimum = New Decimal(New Integer() {10, 0, 0, 0})
		Me.nudScrolWidth.Name = "nudScrolWidth"
		Me.nudScrolWidth.Value = New Decimal(New Integer() {100, 0, 0, 0})
		'
		'nudThinningValueD
		'
		Me.nudThinningValueD.Increment = New Decimal(New Integer() {10, 0, 0, 0})
		resources.ApplyResources(Me.nudThinningValueD, "nudThinningValueD")
		Me.nudThinningValueD.Maximum = New Decimal(New Integer() {50000, 0, 0, 0})
		Me.nudThinningValueD.Name = "nudThinningValueD"
		Me.nudThinningValueD.Value = New Decimal(New Integer() {10, 0, 0, 0})
		'
		'nudThinningValueR
		'
		Me.nudThinningValueR.Increment = New Decimal(New Integer() {10, 0, 0, 0})
		resources.ApplyResources(Me.nudThinningValueR, "nudThinningValueR")
		Me.nudThinningValueR.Maximum = New Decimal(New Integer() {50000, 0, 0, 0})
		Me.nudThinningValueR.Name = "nudThinningValueR"
		Me.nudThinningValueR.Value = New Decimal(New Integer() {100, 0, 0, 0})
		'
		'GroupBox4
		'
		Me.GroupBox4.Controls.Add(Me.rbHideNgtv)
		Me.GroupBox4.Controls.Add(Me.rbDisplayNgtv)
		resources.ApplyResources(Me.GroupBox4, "GroupBox4")
		Me.GroupBox4.Name = "GroupBox4"
		Me.GroupBox4.TabStop = false
		'
		'rbHideNgtv
		'
		resources.ApplyResources(Me.rbHideNgtv, "rbHideNgtv")
		Me.rbHideNgtv.Name = "rbHideNgtv"
		Me.rbHideNgtv.TabStop = true
		Me.rbHideNgtv.UseVisualStyleBackColor = true
		'
		'rbDisplayNgtv
		'
		resources.ApplyResources(Me.rbDisplayNgtv, "rbDisplayNgtv")
		Me.rbDisplayNgtv.Name = "rbDisplayNgtv"
		Me.rbDisplayNgtv.TabStop = true
		Me.rbDisplayNgtv.UseVisualStyleBackColor = true
		'
		'GroupBox3
		'
		Me.GroupBox3.Controls.Add(Me.rbNoGrids)
		Me.GroupBox3.Controls.Add(Me.rbDotGrids)
		Me.GroupBox3.Controls.Add(Me.rbDash)
		Me.GroupBox3.Controls.Add(Me.rbSolid)
		resources.ApplyResources(Me.GroupBox3, "GroupBox3")
		Me.GroupBox3.Name = "GroupBox3"
		Me.GroupBox3.TabStop = false
		'
		'rbNoGrids
		'
		resources.ApplyResources(Me.rbNoGrids, "rbNoGrids")
		Me.rbNoGrids.Name = "rbNoGrids"
		Me.rbNoGrids.TabStop = true
		Me.rbNoGrids.UseVisualStyleBackColor = true
		'
		'rbDotGrids
		'
		resources.ApplyResources(Me.rbDotGrids, "rbDotGrids")
		Me.rbDotGrids.Name = "rbDotGrids"
		Me.rbDotGrids.TabStop = true
		Me.rbDotGrids.UseVisualStyleBackColor = true
		'
		'rbDash
		'
		resources.ApplyResources(Me.rbDash, "rbDash")
		Me.rbDash.Name = "rbDash"
		Me.rbDash.TabStop = true
		Me.rbDash.UseVisualStyleBackColor = true
		'
		'rbSolid
		'
		resources.ApplyResources(Me.rbSolid, "rbSolid")
		Me.rbSolid.Name = "rbSolid"
		Me.rbSolid.TabStop = true
		Me.rbSolid.UseVisualStyleBackColor = true
		'
		'TabPage3
		'
		Me.TabPage3.Controls.Add(Me.gbxSPC)
		Me.TabPage3.Controls.Add(Me.gbxAutoReg)
		Me.TabPage3.Controls.Add(Me.gbxZeroCalib)
		Me.TabPage3.Controls.Add(Me.gbxWeightCalib)
		Me.TabPage3.Controls.Add(Me.gbxFilter)
		Me.TabPage3.Controls.Add(Me.gbxFullCalib)
		resources.ApplyResources(Me.TabPage3, "TabPage3")
		Me.TabPage3.Name = "TabPage3"
		Me.TabPage3.UseVisualStyleBackColor = true
		'
		'gbxSPC
		'
		Me.gbxSPC.Controls.Add(Me.nudSPCCalcSample)
		Me.gbxSPC.Controls.Add(Me.lblSPCCalcSample)
		Me.gbxSPC.Controls.Add(Me.lblSPCDesc)
		Me.gbxSPC.Controls.Add(Me.cbxSPC)
		resources.ApplyResources(Me.gbxSPC, "gbxSPC")
		Me.gbxSPC.Name = "gbxSPC"
		Me.gbxSPC.TabStop = false
		'
		'nudSPCCalcSample
		'
		resources.ApplyResources(Me.nudSPCCalcSample, "nudSPCCalcSample")
		Me.nudSPCCalcSample.Maximum = New Decimal(New Integer() {999, 0, 0, 0})
		Me.nudSPCCalcSample.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
		Me.nudSPCCalcSample.Name = "nudSPCCalcSample"
		Me.nudSPCCalcSample.Value = New Decimal(New Integer() {30, 0, 0, 0})
		'
		'lblSPCCalcSample
		'
		resources.ApplyResources(Me.lblSPCCalcSample, "lblSPCCalcSample")
		Me.lblSPCCalcSample.Name = "lblSPCCalcSample"
		'
		'lblSPCDesc
		'
		resources.ApplyResources(Me.lblSPCDesc, "lblSPCDesc")
		Me.lblSPCDesc.Name = "lblSPCDesc"
		'
		'cbxSPC
		'
		resources.ApplyResources(Me.cbxSPC, "cbxSPC")
		Me.cbxSPC.Checked = true
		Me.cbxSPC.CheckState = System.Windows.Forms.CheckState.Checked
		Me.cbxSPC.Name = "cbxSPC"
		Me.cbxSPC.UseVisualStyleBackColor = true
		'
		'gbxAutoReg
		'
		Me.gbxAutoReg.Controls.Add(Me.lblAutoRegDesc)
		Me.gbxAutoReg.Controls.Add(Me.cbxAutoReg)
		resources.ApplyResources(Me.gbxAutoReg, "gbxAutoReg")
		Me.gbxAutoReg.Name = "gbxAutoReg"
		Me.gbxAutoReg.TabStop = false
		'
		'lblAutoRegDesc
		'
		resources.ApplyResources(Me.lblAutoRegDesc, "lblAutoRegDesc")
		Me.lblAutoRegDesc.Name = "lblAutoRegDesc"
		'
		'cbxAutoReg
		'
		resources.ApplyResources(Me.cbxAutoReg, "cbxAutoReg")
		Me.cbxAutoReg.Checked = true
		Me.cbxAutoReg.CheckState = System.Windows.Forms.CheckState.Checked
		Me.cbxAutoReg.Name = "cbxAutoReg"
		Me.cbxAutoReg.UseVisualStyleBackColor = true
		'
		'gbxZeroCalib
		'
		Me.gbxZeroCalib.Controls.Add(Me.btnZeroCalib)
		resources.ApplyResources(Me.gbxZeroCalib, "gbxZeroCalib")
		Me.gbxZeroCalib.Name = "gbxZeroCalib"
		Me.gbxZeroCalib.TabStop = false
		'
		'btnZeroCalib
		'
		resources.ApplyResources(Me.btnZeroCalib, "btnZeroCalib")
		Me.btnZeroCalib.Name = "btnZeroCalib"
		Me.btnZeroCalib.UseVisualStyleBackColor = true
		'
		'gbxWeightCalib
		'
		Me.gbxWeightCalib.Controls.Add(Me.btnWeightCalib)
		resources.ApplyResources(Me.gbxWeightCalib, "gbxWeightCalib")
		Me.gbxWeightCalib.Name = "gbxWeightCalib"
		Me.gbxWeightCalib.TabStop = false
		'
		'btnWeightCalib
		'
		resources.ApplyResources(Me.btnWeightCalib, "btnWeightCalib")
		Me.btnWeightCalib.Name = "btnWeightCalib"
		Me.btnWeightCalib.UseVisualStyleBackColor = true
		'
		'gbxFilter
		'
		Me.gbxFilter.Controls.Add(Me.cbxFilterEnable)
		Me.gbxFilter.Controls.Add(Me.gbxFilterIIR)
		Me.gbxFilter.Controls.Add(Me.rbIIR)
		Me.gbxFilter.Controls.Add(Me.rbFIR)
		Me.gbxFilter.Controls.Add(Me.gbxFilterFIR)
		resources.ApplyResources(Me.gbxFilter, "gbxFilter")
		Me.gbxFilter.Name = "gbxFilter"
		Me.gbxFilter.TabStop = false
		'
		'cbxFilterEnable
		'
		resources.ApplyResources(Me.cbxFilterEnable, "cbxFilterEnable")
		Me.cbxFilterEnable.Name = "cbxFilterEnable"
		Me.cbxFilterEnable.UseVisualStyleBackColor = true
		'
		'gbxFilterIIR
		'
		Me.gbxFilterIIR.Controls.Add(Me.nudIIRDeg)
		Me.gbxFilterIIR.Controls.Add(Me.lblIIRDegree)
		Me.gbxFilterIIR.Controls.Add(Me.nudIIRCoffFreq)
		Me.gbxFilterIIR.Controls.Add(Me.lblIIRCofFreq)
		Me.gbxFilterIIR.Controls.Add(Me.lblFilterFileName)
		Me.gbxFilterIIR.Controls.Add(Me.btnFilterSelect)
		resources.ApplyResources(Me.gbxFilterIIR, "gbxFilterIIR")
		Me.gbxFilterIIR.Name = "gbxFilterIIR"
		Me.gbxFilterIIR.TabStop = false
		'
		'nudIIRDeg
		'
		resources.ApplyResources(Me.nudIIRDeg, "nudIIRDeg")
		Me.nudIIRDeg.Maximum = New Decimal(New Integer() {3, 0, 0, 0})
		Me.nudIIRDeg.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
		Me.nudIIRDeg.Name = "nudIIRDeg"
		Me.nudIIRDeg.Value = New Decimal(New Integer() {1, 0, 0, 0})
		'
		'lblIIRDegree
		'
		resources.ApplyResources(Me.lblIIRDegree, "lblIIRDegree")
		Me.lblIIRDegree.Name = "lblIIRDegree"
		'
		'nudIIRCoffFreq
		'
		Me.nudIIRCoffFreq.DecimalPlaces = 3
		Me.nudIIRCoffFreq.Increment = New Decimal(New Integer() {1, 0, 0, 196608})
		resources.ApplyResources(Me.nudIIRCoffFreq, "nudIIRCoffFreq")
		Me.nudIIRCoffFreq.Maximum = New Decimal(New Integer() {10000, 0, 0, 0})
		Me.nudIIRCoffFreq.Minimum = New Decimal(New Integer() {1, 0, 0, 196608})
		Me.nudIIRCoffFreq.Name = "nudIIRCoffFreq"
		Me.nudIIRCoffFreq.Value = New Decimal(New Integer() {1, 0, 0, 65536})
		'
		'lblIIRCofFreq
		'
		resources.ApplyResources(Me.lblIIRCofFreq, "lblIIRCofFreq")
		Me.lblIIRCofFreq.Name = "lblIIRCofFreq"
		'
		'lblFilterFileName
		'
		resources.ApplyResources(Me.lblFilterFileName, "lblFilterFileName")
		Me.lblFilterFileName.ForeColor = System.Drawing.Color.Blue
		Me.lblFilterFileName.Name = "lblFilterFileName"
		'
		'btnFilterSelect
		'
		resources.ApplyResources(Me.btnFilterSelect, "btnFilterSelect")
		Me.btnFilterSelect.Name = "btnFilterSelect"
		Me.btnFilterSelect.UseVisualStyleBackColor = true
		'
		'rbIIR
		'
		resources.ApplyResources(Me.rbIIR, "rbIIR")
		Me.rbIIR.Name = "rbIIR"
		Me.rbIIR.TabStop = true
		Me.rbIIR.UseVisualStyleBackColor = true
		'
		'rbFIR
		'
		resources.ApplyResources(Me.rbFIR, "rbFIR")
		Me.rbFIR.Name = "rbFIR"
		Me.rbFIR.TabStop = true
		Me.rbFIR.UseVisualStyleBackColor = true
		'
		'gbxFilterFIR
		'
		Me.gbxFilterFIR.Controls.Add(Me.cbxWinKind)
		Me.gbxFilterFIR.Controls.Add(Me.nudFilterLen)
		Me.gbxFilterFIR.Controls.Add(Me.nudFIRCoffFreq)
		Me.gbxFilterFIR.Controls.Add(Me.lblFIRWinKind)
		Me.gbxFilterFIR.Controls.Add(Me.lblFIRFilterLen)
		Me.gbxFilterFIR.Controls.Add(Me.lblFIRCofFreq)
		resources.ApplyResources(Me.gbxFilterFIR, "gbxFilterFIR")
		Me.gbxFilterFIR.Name = "gbxFilterFIR"
		Me.gbxFilterFIR.TabStop = false
		'
		'cbxWinKind
		'
		Me.cbxWinKind.BackColor = System.Drawing.Color.White
		Me.cbxWinKind.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		resources.ApplyResources(Me.cbxWinKind, "cbxWinKind")
		Me.cbxWinKind.FormattingEnabled = true
		Me.cbxWinKind.Name = "cbxWinKind"
		'
		'nudFilterLen
		'
		resources.ApplyResources(Me.nudFilterLen, "nudFilterLen")
		Me.nudFilterLen.Maximum = New Decimal(New Integer() {4097, 0, 0, 0})
		Me.nudFilterLen.Name = "nudFilterLen"
		'
		'nudFIRCoffFreq
		'
		Me.nudFIRCoffFreq.DecimalPlaces = 3
		Me.nudFIRCoffFreq.Increment = New Decimal(New Integer() {1, 0, 0, 196608})
		resources.ApplyResources(Me.nudFIRCoffFreq, "nudFIRCoffFreq")
		Me.nudFIRCoffFreq.Maximum = New Decimal(New Integer() {10000, 0, 0, 0})
		Me.nudFIRCoffFreq.Minimum = New Decimal(New Integer() {1, 0, 0, 196608})
		Me.nudFIRCoffFreq.Name = "nudFIRCoffFreq"
		Me.nudFIRCoffFreq.Value = New Decimal(New Integer() {1, 0, 0, 65536})
		'
		'lblFIRWinKind
		'
		resources.ApplyResources(Me.lblFIRWinKind, "lblFIRWinKind")
		Me.lblFIRWinKind.Name = "lblFIRWinKind"
		'
		'lblFIRFilterLen
		'
		resources.ApplyResources(Me.lblFIRFilterLen, "lblFIRFilterLen")
		Me.lblFIRFilterLen.Name = "lblFIRFilterLen"
		'
		'lblFIRCofFreq
		'
		resources.ApplyResources(Me.lblFIRCofFreq, "lblFIRCofFreq")
		Me.lblFIRCofFreq.Name = "lblFIRCofFreq"
		'
		'gbxFullCalib
		'
		Me.gbxFullCalib.Controls.Add(Me.btnFullCalib)
		resources.ApplyResources(Me.gbxFullCalib, "gbxFullCalib")
		Me.gbxFullCalib.Name = "gbxFullCalib"
		Me.gbxFullCalib.TabStop = false
		'
		'btnFullCalib
		'
		resources.ApplyResources(Me.btnFullCalib, "btnFullCalib")
		Me.btnFullCalib.Name = "btnFullCalib"
		Me.btnFullCalib.UseVisualStyleBackColor = true
		'
		'TabPage5
		'
		Me.TabPage5.Controls.Add(Me.gpxHiddenSetting)
		Me.TabPage5.Controls.Add(Me.gbxBuzzerControl)
		Me.TabPage5.Controls.Add(Me.gbxTimeSet)
		Me.TabPage5.Controls.Add(Me.GroupBox12)
		Me.TabPage5.Controls.Add(Me.GroupBox10)
		resources.ApplyResources(Me.TabPage5, "TabPage5")
		Me.TabPage5.Name = "TabPage5"
		Me.TabPage5.UseVisualStyleBackColor = true
		'
		'gpxHiddenSetting
		'
		Me.gpxHiddenSetting.Controls.Add(Me.btnCreateFactorySetting)
		Me.gpxHiddenSetting.Controls.Add(Me.cbxTempNoLimitEn)
		Me.gpxHiddenSetting.Controls.Add(Me.cbxReadRealAreaEn)
		Me.gpxHiddenSetting.Controls.Add(Me.cbxCalcResultFOutEn)
		Me.gpxHiddenSetting.Controls.Add(Me.cbxEquipListEn)
		Me.gpxHiddenSetting.Controls.Add(Me.cbxScanTimeEn)
		Me.gpxHiddenSetting.Controls.Add(Me.cbxMeasTimeEn)
		Me.gpxHiddenSetting.Controls.Add(Me.gbxZUpperLimit)
		Me.gpxHiddenSetting.Controls.Add(Me.cbxMultActEn)
		Me.gpxHiddenSetting.Controls.Add(Me.cbxCoverILDis)
		Me.gpxHiddenSetting.Controls.Add(Me.cbDatafileCalcEnable)
		Me.gpxHiddenSetting.Controls.Add(Me.cbxCalibValChangeEn)
		Me.gpxHiddenSetting.Controls.Add(Me.cbxDebugFileEn)
		Me.gpxHiddenSetting.Controls.Add(Me.cbxZStabDis)
		Me.gpxHiddenSetting.Controls.Add(Me.cbxInputEnMeasureing)
		Me.gpxHiddenSetting.Controls.Add(Me.cbxPcLifeChkDis)
		resources.ApplyResources(Me.gpxHiddenSetting, "gpxHiddenSetting")
		Me.gpxHiddenSetting.Name = "gpxHiddenSetting"
		Me.gpxHiddenSetting.TabStop = false
		'
		'btnCreateFactorySetting
		'
		resources.ApplyResources(Me.btnCreateFactorySetting, "btnCreateFactorySetting")
		Me.btnCreateFactorySetting.Name = "btnCreateFactorySetting"
		Me.btnCreateFactorySetting.UseVisualStyleBackColor = true
		'
		'cbxTempNoLimitEn
		'
		resources.ApplyResources(Me.cbxTempNoLimitEn, "cbxTempNoLimitEn")
		Me.cbxTempNoLimitEn.Name = "cbxTempNoLimitEn"
		Me.cbxTempNoLimitEn.UseVisualStyleBackColor = true
		'
		'cbxReadRealAreaEn
		'
		resources.ApplyResources(Me.cbxReadRealAreaEn, "cbxReadRealAreaEn")
		Me.cbxReadRealAreaEn.Name = "cbxReadRealAreaEn"
		Me.cbxReadRealAreaEn.UseVisualStyleBackColor = true
		'
		'cbxCalcResultFOutEn
		'
		resources.ApplyResources(Me.cbxCalcResultFOutEn, "cbxCalcResultFOutEn")
		Me.cbxCalcResultFOutEn.Name = "cbxCalcResultFOutEn"
		Me.cbxCalcResultFOutEn.UseVisualStyleBackColor = true
		'
		'cbxEquipListEn
		'
		resources.ApplyResources(Me.cbxEquipListEn, "cbxEquipListEn")
		Me.cbxEquipListEn.Name = "cbxEquipListEn"
		Me.cbxEquipListEn.UseVisualStyleBackColor = true
		'
		'cbxScanTimeEn
		'
		resources.ApplyResources(Me.cbxScanTimeEn, "cbxScanTimeEn")
		Me.cbxScanTimeEn.Name = "cbxScanTimeEn"
		Me.cbxScanTimeEn.UseVisualStyleBackColor = true
		'
		'cbxMeasTimeEn
		'
		resources.ApplyResources(Me.cbxMeasTimeEn, "cbxMeasTimeEn")
		Me.cbxMeasTimeEn.Name = "cbxMeasTimeEn"
		Me.cbxMeasTimeEn.UseVisualStyleBackColor = true
		'
		'gbxZUpperLimit
		'
		Me.gbxZUpperLimit.Controls.Add(Me.nudZUpperLimit)
		resources.ApplyResources(Me.gbxZUpperLimit, "gbxZUpperLimit")
		Me.gbxZUpperLimit.Name = "gbxZUpperLimit"
		Me.gbxZUpperLimit.TabStop = false
		'
		'nudZUpperLimit
		'
		Me.nudZUpperLimit.DecimalPlaces = 3
		Me.nudZUpperLimit.Increment = New Decimal(New Integer() {1, 0, 0, 196608})
		resources.ApplyResources(Me.nudZUpperLimit, "nudZUpperLimit")
		Me.nudZUpperLimit.Maximum = New Decimal(New Integer() {50000, 0, 0, 196608})
		Me.nudZUpperLimit.Name = "nudZUpperLimit"
		'
		'cbxMultActEn
		'
		resources.ApplyResources(Me.cbxMultActEn, "cbxMultActEn")
		Me.cbxMultActEn.Name = "cbxMultActEn"
		Me.cbxMultActEn.UseVisualStyleBackColor = true
		'
		'cbxCoverILDis
		'
		resources.ApplyResources(Me.cbxCoverILDis, "cbxCoverILDis")
		Me.cbxCoverILDis.Name = "cbxCoverILDis"
		Me.cbxCoverILDis.UseVisualStyleBackColor = true
		'
		'cbDatafileCalcEnable
		'
		resources.ApplyResources(Me.cbDatafileCalcEnable, "cbDatafileCalcEnable")
		Me.cbDatafileCalcEnable.Name = "cbDatafileCalcEnable"
		Me.cbDatafileCalcEnable.UseVisualStyleBackColor = true
		'
		'cbxCalibValChangeEn
		'
		resources.ApplyResources(Me.cbxCalibValChangeEn, "cbxCalibValChangeEn")
		Me.cbxCalibValChangeEn.Name = "cbxCalibValChangeEn"
		Me.cbxCalibValChangeEn.UseVisualStyleBackColor = true
		'
		'cbxDebugFileEn
		'
		resources.ApplyResources(Me.cbxDebugFileEn, "cbxDebugFileEn")
		Me.cbxDebugFileEn.Name = "cbxDebugFileEn"
		Me.cbxDebugFileEn.UseVisualStyleBackColor = true
		'
		'cbxZStabDis
		'
		resources.ApplyResources(Me.cbxZStabDis, "cbxZStabDis")
		Me.cbxZStabDis.Name = "cbxZStabDis"
		Me.cbxZStabDis.UseVisualStyleBackColor = true
		'
		'cbxInputEnMeasureing
		'
		resources.ApplyResources(Me.cbxInputEnMeasureing, "cbxInputEnMeasureing")
		Me.cbxInputEnMeasureing.Name = "cbxInputEnMeasureing"
		Me.cbxInputEnMeasureing.UseVisualStyleBackColor = true
		'
		'cbxPcLifeChkDis
		'
		resources.ApplyResources(Me.cbxPcLifeChkDis, "cbxPcLifeChkDis")
		Me.cbxPcLifeChkDis.Name = "cbxPcLifeChkDis"
		Me.cbxPcLifeChkDis.UseVisualStyleBackColor = true
		'
		'gbxBuzzerControl
		'
		Me.gbxBuzzerControl.Controls.Add(Me.lblEndOfBuzzerPattern)
		Me.gbxBuzzerControl.Controls.Add(Me.nudBZPattern)
		Me.gbxBuzzerControl.Controls.Add(Me.cbxOkOfHeat)
		Me.gbxBuzzerControl.Controls.Add(Me.cbxEndOfMesure)
		resources.ApplyResources(Me.gbxBuzzerControl, "gbxBuzzerControl")
		Me.gbxBuzzerControl.Name = "gbxBuzzerControl"
		Me.gbxBuzzerControl.TabStop = false
		'
		'lblEndOfBuzzerPattern
		'
		resources.ApplyResources(Me.lblEndOfBuzzerPattern, "lblEndOfBuzzerPattern")
		Me.lblEndOfBuzzerPattern.Name = "lblEndOfBuzzerPattern"
		'
		'nudBZPattern
		'
		resources.ApplyResources(Me.nudBZPattern, "nudBZPattern")
		Me.nudBZPattern.Maximum = New Decimal(New Integer() {3, 0, 0, 0})
		Me.nudBZPattern.Name = "nudBZPattern"
		'
		'cbxOkOfHeat
		'
		resources.ApplyResources(Me.cbxOkOfHeat, "cbxOkOfHeat")
		Me.cbxOkOfHeat.Name = "cbxOkOfHeat"
		Me.cbxOkOfHeat.UseVisualStyleBackColor = true
		'
		'cbxEndOfMesure
		'
		resources.ApplyResources(Me.cbxEndOfMesure, "cbxEndOfMesure")
		Me.cbxEndOfMesure.Name = "cbxEndOfMesure"
		Me.cbxEndOfMesure.UseVisualStyleBackColor = true
		'
		'gbxTimeSet
		'
		Me.gbxTimeSet.Controls.Add(Me.lblTime)
		Me.gbxTimeSet.Controls.Add(Me.lblDate)
		Me.gbxTimeSet.Controls.Add(Me.nudSecond)
		Me.gbxTimeSet.Controls.Add(Me.nudMinute)
		Me.gbxTimeSet.Controls.Add(Me.nudDay)
		Me.gbxTimeSet.Controls.Add(Me.nudHour)
		Me.gbxTimeSet.Controls.Add(Me.nudMonth)
		Me.gbxTimeSet.Controls.Add(Me.nudYear)
		Me.gbxTimeSet.Controls.Add(Me.btnNow)
		Me.gbxTimeSet.Controls.Add(Me.btnDateAply)
		resources.ApplyResources(Me.gbxTimeSet, "gbxTimeSet")
		Me.gbxTimeSet.Name = "gbxTimeSet"
		Me.gbxTimeSet.TabStop = false
		'
		'lblTime
		'
		resources.ApplyResources(Me.lblTime, "lblTime")
		Me.lblTime.Name = "lblTime"
		'
		'lblDate
		'
		resources.ApplyResources(Me.lblDate, "lblDate")
		Me.lblDate.Name = "lblDate"
		'
		'nudSecond
		'
		resources.ApplyResources(Me.nudSecond, "nudSecond")
		Me.nudSecond.Maximum = New Decimal(New Integer() {59, 0, 0, 0})
		Me.nudSecond.Name = "nudSecond"
		'
		'nudMinute
		'
		resources.ApplyResources(Me.nudMinute, "nudMinute")
		Me.nudMinute.Maximum = New Decimal(New Integer() {59, 0, 0, 0})
		Me.nudMinute.Name = "nudMinute"
		'
		'nudDay
		'
		resources.ApplyResources(Me.nudDay, "nudDay")
		Me.nudDay.Maximum = New Decimal(New Integer() {31, 0, 0, 0})
		Me.nudDay.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
		Me.nudDay.Name = "nudDay"
		Me.nudDay.Value = New Decimal(New Integer() {1, 0, 0, 0})
		'
		'nudHour
		'
		resources.ApplyResources(Me.nudHour, "nudHour")
		Me.nudHour.Maximum = New Decimal(New Integer() {23, 0, 0, 0})
		Me.nudHour.Name = "nudHour"
		'
		'nudMonth
		'
		resources.ApplyResources(Me.nudMonth, "nudMonth")
		Me.nudMonth.Maximum = New Decimal(New Integer() {12, 0, 0, 0})
		Me.nudMonth.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
		Me.nudMonth.Name = "nudMonth"
		Me.nudMonth.Value = New Decimal(New Integer() {1, 0, 0, 0})
		'
		'nudYear
		'
		resources.ApplyResources(Me.nudYear, "nudYear")
		Me.nudYear.Maximum = New Decimal(New Integer() {3000, 0, 0, 0})
		Me.nudYear.Minimum = New Decimal(New Integer() {2000, 0, 0, 0})
		Me.nudYear.Name = "nudYear"
		Me.nudYear.Value = New Decimal(New Integer() {2000, 0, 0, 0})
		'
		'btnNow
		'
		resources.ApplyResources(Me.btnNow, "btnNow")
		Me.btnNow.Name = "btnNow"
		Me.btnNow.UseVisualStyleBackColor = true
		'
		'btnDateAply
		'
		resources.ApplyResources(Me.btnDateAply, "btnDateAply")
		Me.btnDateAply.Name = "btnDateAply"
		Me.btnDateAply.UseVisualStyleBackColor = true
		'
		'GroupBox12
		'
		Me.GroupBox12.Controls.Add(Me.lblAutoStartTime)
		Me.GroupBox12.Controls.Add(Me.Label2)
		Me.GroupBox12.Controls.Add(Me.cbAutoStart)
		Me.GroupBox12.Controls.Add(Me.nudAutoStart)
		resources.ApplyResources(Me.GroupBox12, "GroupBox12")
		Me.GroupBox12.Name = "GroupBox12"
		Me.GroupBox12.TabStop = false
		'
		'lblAutoStartTime
		'
		resources.ApplyResources(Me.lblAutoStartTime, "lblAutoStartTime")
		Me.lblAutoStartTime.Name = "lblAutoStartTime"
		'
		'Label2
		'
		resources.ApplyResources(Me.Label2, "Label2")
		Me.Label2.Name = "Label2"
		'
		'cbAutoStart
		'
		resources.ApplyResources(Me.cbAutoStart, "cbAutoStart")
		Me.cbAutoStart.Checked = true
		Me.cbAutoStart.CheckState = System.Windows.Forms.CheckState.Checked
		Me.cbAutoStart.Name = "cbAutoStart"
		Me.cbAutoStart.UseVisualStyleBackColor = true
		'
		'nudAutoStart
		'
		resources.ApplyResources(Me.nudAutoStart, "nudAutoStart")
		Me.nudAutoStart.Name = "nudAutoStart"
		'
		'GroupBox10
		'
		Me.GroupBox10.Controls.Add(Me.btnDeviceSerWite)
		Me.GroupBox10.Controls.Add(Me.dgvEquipList)
		Me.GroupBox10.Controls.Add(Me.Label3)
		Me.GroupBox10.Controls.Add(Me.txtPort)
		resources.ApplyResources(Me.GroupBox10, "GroupBox10")
		Me.GroupBox10.Name = "GroupBox10"
		Me.GroupBox10.TabStop = false
		'
		'btnDeviceSerWite
		'
		resources.ApplyResources(Me.btnDeviceSerWite, "btnDeviceSerWite")
		Me.btnDeviceSerWite.Name = "btnDeviceSerWite"
		Me.btnDeviceSerWite.UseVisualStyleBackColor = true
		'
		'dgvEquipList
		'
		Me.dgvEquipList.AllowUserToAddRows = false
		Me.dgvEquipList.AllowUserToDeleteRows = false
		Me.dgvEquipList.AllowUserToResizeColumns = false
		Me.dgvEquipList.AllowUserToResizeRows = false
		Me.dgvEquipList.BackgroundColor = System.Drawing.SystemColors.Window
		Me.dgvEquipList.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.dgvEquipList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
		Me.dgvEquipList.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.SelectColumn, Me.IPColumn, Me.NameColumn, Me.XYSensSerial, Me.ZSensSerial, Me.CF1Column, Me.ToolCF1Column, Me.FTPColumn})
		resources.ApplyResources(Me.dgvEquipList, "dgvEquipList")
		Me.dgvEquipList.Name = "dgvEquipList"
		Me.dgvEquipList.RowHeadersVisible = false
		Me.dgvEquipList.RowTemplate.Height = 21
		'
		'SelectColumn
		'
		resources.ApplyResources(Me.SelectColumn, "SelectColumn")
		Me.SelectColumn.Name = "SelectColumn"
		Me.SelectColumn.ReadOnly = true
		'
		'IPColumn
		'
		resources.ApplyResources(Me.IPColumn, "IPColumn")
		Me.IPColumn.Name = "IPColumn"
		Me.IPColumn.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
		Me.IPColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
		'
		'NameColumn
		'
		resources.ApplyResources(Me.NameColumn, "NameColumn")
		Me.NameColumn.Name = "NameColumn"
		Me.NameColumn.ReadOnly = true
		Me.NameColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
		'
		'XYSensSerial
		'
		resources.ApplyResources(Me.XYSensSerial, "XYSensSerial")
		Me.XYSensSerial.Name = "XYSensSerial"
		Me.XYSensSerial.ReadOnly = true
		'
		'ZSensSerial
		'
		resources.ApplyResources(Me.ZSensSerial, "ZSensSerial")
		Me.ZSensSerial.Name = "ZSensSerial"
		Me.ZSensSerial.ReadOnly = true
		'
		'CF1Column
		'
		resources.ApplyResources(Me.CF1Column, "CF1Column")
		Me.CF1Column.Name = "CF1Column"
		Me.CF1Column.ReadOnly = true
		Me.CF1Column.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
		'
		'ToolCF1Column
		'
		resources.ApplyResources(Me.ToolCF1Column, "ToolCF1Column")
		Me.ToolCF1Column.Name = "ToolCF1Column"
		Me.ToolCF1Column.ReadOnly = true
		Me.ToolCF1Column.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
		'
		'FTPColumn
		'
		resources.ApplyResources(Me.FTPColumn, "FTPColumn")
		Me.FTPColumn.Name = "FTPColumn"
		Me.FTPColumn.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
		Me.FTPColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
		Me.FTPColumn.Text = "Set"
		'
		'Label3
		'
		resources.ApplyResources(Me.Label3, "Label3")
		Me.Label3.Name = "Label3"
		'
		'txtPort
		'
		resources.ApplyResources(Me.txtPort, "txtPort")
		Me.txtPort.Name = "txtPort"
		'
		'TabPage6
		'
		Me.TabPage6.Controls.Add(Me.gbxSpecList)
		resources.ApplyResources(Me.TabPage6, "TabPage6")
		Me.TabPage6.Name = "TabPage6"
		Me.TabPage6.UseVisualStyleBackColor = true
		'
		'gbxSpecList
		'
		Me.gbxSpecList.Controls.Add(Me.txtSpecName)
		Me.gbxSpecList.Controls.Add(Me.lblSPECName)
		Me.gbxSpecList.Controls.Add(Me.btnSpecCopyFix)
		Me.gbxSpecList.Controls.Add(Me.btnSpecCopyFree)
		Me.gbxSpecList.Controls.Add(Me.dgvSpec)
		Me.gbxSpecList.Controls.Add(Me.dgvCalcOpt)
		Me.gbxSpecList.Controls.Add(Me.cbxLotManu)
		Me.gbxSpecList.Controls.Add(Me.btnSpcImp)
		Me.gbxSpecList.Controls.Add(Me.btnSpcExp)
		Me.gbxSpecList.Controls.Add(Me.lblSpecFilter)
		Me.gbxSpecList.Controls.Add(Me.btnFixFilter)
		Me.gbxSpecList.Controls.Add(Me.txtFixFilter)
		Me.gbxSpecList.Controls.Add(Me.cboSpecRtgset)
		Me.gbxSpecList.Controls.Add(Me.lblHotplateSetting)
		Me.gbxSpecList.Controls.Add(Me.btnSaveSpec)
		Me.gbxSpecList.Controls.Add(Me.btnDeleteSpec)
		Me.gbxSpecList.Controls.Add(Me.lblMethod)
		Me.gbxSpecList.Controls.Add(Me.cboMethod)
		Me.gbxSpecList.Controls.Add(Me.cboXYModeOp)
		Me.gbxSpecList.Controls.Add(Me.lblXYDriveMode)
		Me.gbxSpecList.Controls.Add(Me.lvSpec)
		resources.ApplyResources(Me.gbxSpecList, "gbxSpecList")
		Me.gbxSpecList.Name = "gbxSpecList"
		Me.gbxSpecList.TabStop = false
		'
		'txtSpecName
		'
		resources.ApplyResources(Me.txtSpecName, "txtSpecName")
		Me.txtSpecName.Name = "txtSpecName"
		'
		'lblSPECName
		'
		resources.ApplyResources(Me.lblSPECName, "lblSPECName")
		Me.lblSPECName.Name = "lblSPECName"
		'
		'btnSpecCopyFix
		'
		resources.ApplyResources(Me.btnSpecCopyFix, "btnSpecCopyFix")
		Me.btnSpecCopyFix.Name = "btnSpecCopyFix"
		Me.btnSpecCopyFix.UseVisualStyleBackColor = true
		'
		'btnSpecCopyFree
		'
		resources.ApplyResources(Me.btnSpecCopyFree, "btnSpecCopyFree")
		Me.btnSpecCopyFree.Name = "btnSpecCopyFree"
		Me.btnSpecCopyFree.UseVisualStyleBackColor = true
		'
		'dgvSpec
		'
		Me.dgvSpec.AllowUserToAddRows = false
		Me.dgvSpec.AllowUserToDeleteRows = false
		Me.dgvSpec.AllowUserToResizeRows = false
		Me.dgvSpec.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
		Me.dgvSpec.BackgroundColor = System.Drawing.SystemColors.Control
		Me.dgvSpec.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.dgvSpec.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
		Me.dgvSpec.ColumnHeadersVisible = false
		Me.dgvSpec.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.DataGridViewTextBoxColumn1, Me.DataGridViewTextBoxColumn2})
		resources.ApplyResources(Me.dgvSpec, "dgvSpec")
		Me.dgvSpec.Name = "dgvSpec"
		Me.dgvSpec.RowHeadersVisible = false
		Me.dgvSpec.RowTemplate.Height = 21
		'
		'DataGridViewTextBoxColumn1
		'
		DataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.White
		DataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.Black
		Me.DataGridViewTextBoxColumn1.DefaultCellStyle = DataGridViewCellStyle1
		Me.DataGridViewTextBoxColumn1.FillWeight = 150!
		resources.ApplyResources(Me.DataGridViewTextBoxColumn1, "DataGridViewTextBoxColumn1")
		Me.DataGridViewTextBoxColumn1.Name = "DataGridViewTextBoxColumn1"
		Me.DataGridViewTextBoxColumn1.ReadOnly = true
		'
		'DataGridViewTextBoxColumn2
		'
		Me.DataGridViewTextBoxColumn2.FillWeight = 80!
		resources.ApplyResources(Me.DataGridViewTextBoxColumn2, "DataGridViewTextBoxColumn2")
		Me.DataGridViewTextBoxColumn2.Name = "DataGridViewTextBoxColumn2"
		'
		'dgvCalcOpt
		'
		Me.dgvCalcOpt.AllowUserToAddRows = false
		Me.dgvCalcOpt.AllowUserToDeleteRows = false
		Me.dgvCalcOpt.AllowUserToResizeColumns = false
		Me.dgvCalcOpt.AllowUserToResizeRows = false
		resources.ApplyResources(Me.dgvCalcOpt, "dgvCalcOpt")
		Me.dgvCalcOpt.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
		Me.dgvCalcOpt.BackgroundColor = System.Drawing.SystemColors.Control
		Me.dgvCalcOpt.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.dgvCalcOpt.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
		Me.dgvCalcOpt.ColumnHeadersVisible = false
		Me.dgvCalcOpt.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.DataGridViewTextBoxColumn3, Me.DataGridViewTextBoxColumn4})
		Me.dgvCalcOpt.Name = "dgvCalcOpt"
		Me.dgvCalcOpt.RowHeadersVisible = false
		Me.dgvCalcOpt.RowTemplate.Height = 21
		'
		'DataGridViewTextBoxColumn3
		'
		DataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.White
		DataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.Black
		Me.DataGridViewTextBoxColumn3.DefaultCellStyle = DataGridViewCellStyle2
		resources.ApplyResources(Me.DataGridViewTextBoxColumn3, "DataGridViewTextBoxColumn3")
		Me.DataGridViewTextBoxColumn3.Name = "DataGridViewTextBoxColumn3"
		'
		'DataGridViewTextBoxColumn4
		'
		Me.DataGridViewTextBoxColumn4.FillWeight = 80!
		resources.ApplyResources(Me.DataGridViewTextBoxColumn4, "DataGridViewTextBoxColumn4")
		Me.DataGridViewTextBoxColumn4.Name = "DataGridViewTextBoxColumn4"
		'
		'cbxLotManu
		'
		resources.ApplyResources(Me.cbxLotManu, "cbxLotManu")
		Me.cbxLotManu.Name = "cbxLotManu"
		Me.cbxLotManu.UseVisualStyleBackColor = true
		'
		'btnSpcImp
		'
		resources.ApplyResources(Me.btnSpcImp, "btnSpcImp")
		Me.btnSpcImp.Name = "btnSpcImp"
		Me.btnSpcImp.UseVisualStyleBackColor = true
		'
		'btnSpcExp
		'
		resources.ApplyResources(Me.btnSpcExp, "btnSpcExp")
		Me.btnSpcExp.Name = "btnSpcExp"
		Me.btnSpcExp.UseVisualStyleBackColor = true
		'
		'lblSpecFilter
		'
		resources.ApplyResources(Me.lblSpecFilter, "lblSpecFilter")
		Me.lblSpecFilter.Name = "lblSpecFilter"
		'
		'btnFixFilter
		'
		resources.ApplyResources(Me.btnFixFilter, "btnFixFilter")
		Me.btnFixFilter.Name = "btnFixFilter"
		Me.btnFixFilter.Tag = "0"
		Me.btnFixFilter.UseVisualStyleBackColor = true
		'
		'txtFixFilter
		'
		resources.ApplyResources(Me.txtFixFilter, "txtFixFilter")
		Me.txtFixFilter.BackColor = System.Drawing.SystemColors.Info
		Me.txtFixFilter.Name = "txtFixFilter"
		'
		'cboSpecRtgset
		'
		Me.cboSpecRtgset.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		resources.ApplyResources(Me.cboSpecRtgset, "cboSpecRtgset")
		Me.cboSpecRtgset.FormattingEnabled = true
		Me.cboSpecRtgset.Name = "cboSpecRtgset"
		'
		'lblHotplateSetting
		'
		resources.ApplyResources(Me.lblHotplateSetting, "lblHotplateSetting")
		Me.lblHotplateSetting.Name = "lblHotplateSetting"
		'
		'btnSaveSpec
		'
		resources.ApplyResources(Me.btnSaveSpec, "btnSaveSpec")
		Me.btnSaveSpec.Name = "btnSaveSpec"
		Me.btnSaveSpec.UseVisualStyleBackColor = true
		'
		'btnDeleteSpec
		'
		resources.ApplyResources(Me.btnDeleteSpec, "btnDeleteSpec")
		Me.btnDeleteSpec.Name = "btnDeleteSpec"
		Me.btnDeleteSpec.UseVisualStyleBackColor = true
		'
		'lblMethod
		'
		resources.ApplyResources(Me.lblMethod, "lblMethod")
		Me.lblMethod.Name = "lblMethod"
		'
		'cboMethod
		'
		resources.ApplyResources(Me.cboMethod, "cboMethod")
		Me.cboMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.cboMethod.FormattingEnabled = true
		Me.cboMethod.Items.AddRange(New Object() {resources.GetString("cboMethod.Items"), resources.GetString("cboMethod.Items1"), resources.GetString("cboMethod.Items2"), resources.GetString("cboMethod.Items3")})
		Me.cboMethod.Name = "cboMethod"
		'
		'cboXYModeOp
		'
		resources.ApplyResources(Me.cboXYModeOp, "cboXYModeOp")
		Me.cboXYModeOp.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.cboXYModeOp.FormattingEnabled = true
		Me.cboXYModeOp.Items.AddRange(New Object() {resources.GetString("cboXYModeOp.Items"), resources.GetString("cboXYModeOp.Items1"), resources.GetString("cboXYModeOp.Items2")})
		Me.cboXYModeOp.Name = "cboXYModeOp"
		'
		'lblXYDriveMode
		'
		resources.ApplyResources(Me.lblXYDriveMode, "lblXYDriveMode")
		Me.lblXYDriveMode.Name = "lblXYDriveMode"
		'
		'lvSpec
		'
		Me.lvSpec.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader3})
		Me.lvSpec.FullRowSelect = true
		Me.lvSpec.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None
		Me.lvSpec.HideSelection = false
		resources.ApplyResources(Me.lvSpec, "lvSpec")
		Me.lvSpec.MultiSelect = false
		Me.lvSpec.Name = "lvSpec"
		Me.lvSpec.Sorting = System.Windows.Forms.SortOrder.Ascending
		Me.lvSpec.UseCompatibleStateImageBehavior = false
		Me.lvSpec.View = System.Windows.Forms.View.Details
		'
		'ColumnHeader3
		'
		resources.ApplyResources(Me.ColumnHeader3, "ColumnHeader3")
		'
		'TabPage7
		'
		Me.TabPage7.Controls.Add(Me.gbxTempControl)
		resources.ApplyResources(Me.TabPage7, "TabPage7")
		Me.TabPage7.Name = "TabPage7"
		Me.TabPage7.UseVisualStyleBackColor = true
		'
		'gbxTempControl
		'
		Me.gbxTempControl.Controls.Add(Me.gbxSG)
		Me.gbxTempControl.Controls.Add(Me.btnPIDMonitor)
		Me.gbxTempControl.Controls.Add(Me.gbxPB)
		Me.gbxTempControl.Controls.Add(Me.gbxHP)
		Me.gbxTempControl.Controls.Add(Me.cbxTempLEDEn)
		Me.gbxTempControl.Controls.Add(Me.lblLEDTempObsrv)
		Me.gbxTempControl.Controls.Add(Me.nudLEDTempObsrv)
		resources.ApplyResources(Me.gbxTempControl, "gbxTempControl")
		Me.gbxTempControl.Name = "gbxTempControl"
		Me.gbxTempControl.TabStop = false
		'
		'gbxSG
		'
		Me.gbxSG.Controls.Add(Me.nudHDifSetTemp3)
		Me.gbxSG.Controls.Add(Me.lblDifSet)
		Me.gbxSG.Controls.Add(Me.nudHLowerTemp3)
		Me.gbxSG.Controls.Add(Me.lblLoLimit3)
		Me.gbxSG.Controls.Add(Me.nudPVGain3)
		Me.gbxSG.Controls.Add(Me.lblPVGain3)
		Me.gbxSG.Controls.Add(Me.nudPVBias3)
		Me.gbxSG.Controls.Add(Me.nudHUpperTemp3)
		Me.gbxSG.Controls.Add(Me.lblUpLimit3)
		Me.gbxSG.Controls.Add(Me.nudDgain3)
		Me.gbxSG.Controls.Add(Me.nudD3)
		Me.gbxSG.Controls.Add(Me.nudI3)
		Me.gbxSG.Controls.Add(Me.nudP3)
		Me.gbxSG.Controls.Add(Me.lblD3)
		Me.gbxSG.Controls.Add(Me.lblI3)
		Me.gbxSG.Controls.Add(Me.lblPVBias3)
		Me.gbxSG.Controls.Add(Me.lblDgain3)
		Me.gbxSG.Controls.Add(Me.lblP3)
		resources.ApplyResources(Me.gbxSG, "gbxSG")
		Me.gbxSG.Name = "gbxSG"
		Me.gbxSG.TabStop = false
		'
		'nudHDifSetTemp3
		'
		Me.nudHDifSetTemp3.DecimalPlaces = 1
		Me.nudHDifSetTemp3.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
		resources.ApplyResources(Me.nudHDifSetTemp3, "nudHDifSetTemp3")
		Me.nudHDifSetTemp3.Maximum = New Decimal(New Integer() {200, 0, 0, 0})
		Me.nudHDifSetTemp3.Name = "nudHDifSetTemp3"
		Me.nudHDifSetTemp3.Value = New Decimal(New Integer() {20, 0, 0, 0})
		'
		'lblDifSet
		'
		resources.ApplyResources(Me.lblDifSet, "lblDifSet")
		Me.lblDifSet.Name = "lblDifSet"
		'
		'nudHLowerTemp3
		'
		Me.nudHLowerTemp3.DecimalPlaces = 1
		Me.nudHLowerTemp3.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
		resources.ApplyResources(Me.nudHLowerTemp3, "nudHLowerTemp3")
		Me.nudHLowerTemp3.Maximum = New Decimal(New Integer() {50, 0, 0, 0})
		Me.nudHLowerTemp3.Name = "nudHLowerTemp3"
		Me.nudHLowerTemp3.Value = New Decimal(New Integer() {30, 0, 0, 0})
		'
		'lblLoLimit3
		'
		resources.ApplyResources(Me.lblLoLimit3, "lblLoLimit3")
		Me.lblLoLimit3.Name = "lblLoLimit3"
		'
		'nudPVGain3
		'
		Me.nudPVGain3.DecimalPlaces = 4
		Me.nudPVGain3.Increment = New Decimal(New Integer() {1, 0, 0, 131072})
		resources.ApplyResources(Me.nudPVGain3, "nudPVGain3")
		Me.nudPVGain3.Maximum = New Decimal(New Integer() {300, 0, 0, 0})
		Me.nudPVGain3.Minimum = New Decimal(New Integer() {1, 0, 0, 131072})
		Me.nudPVGain3.Name = "nudPVGain3"
		Me.nudPVGain3.Value = New Decimal(New Integer() {1, 0, 0, 0})
		'
		'lblPVGain3
		'
		resources.ApplyResources(Me.lblPVGain3, "lblPVGain3")
		Me.lblPVGain3.Name = "lblPVGain3"
		'
		'nudPVBias3
		'
		Me.nudPVBias3.DecimalPlaces = 1
		Me.nudPVBias3.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
		resources.ApplyResources(Me.nudPVBias3, "nudPVBias3")
		Me.nudPVBias3.Maximum = New Decimal(New Integer() {10, 0, 0, 0})
		Me.nudPVBias3.Minimum = New Decimal(New Integer() {10, 0, 0, -2147483648})
		Me.nudPVBias3.Name = "nudPVBias3"
		'
		'nudHUpperTemp3
		'
		Me.nudHUpperTemp3.DecimalPlaces = 1
		Me.nudHUpperTemp3.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
		resources.ApplyResources(Me.nudHUpperTemp3, "nudHUpperTemp3")
		Me.nudHUpperTemp3.Maximum = New Decimal(New Integer() {150, 0, 0, 0})
		Me.nudHUpperTemp3.Minimum = New Decimal(New Integer() {20, 0, 0, 0})
		Me.nudHUpperTemp3.Name = "nudHUpperTemp3"
		Me.nudHUpperTemp3.Value = New Decimal(New Integer() {30, 0, 0, 0})
		'
		'lblUpLimit3
		'
		resources.ApplyResources(Me.lblUpLimit3, "lblUpLimit3")
		Me.lblUpLimit3.Name = "lblUpLimit3"
		'
		'nudDgain3
		'
		Me.nudDgain3.DecimalPlaces = 2
		Me.nudDgain3.Increment = New Decimal(New Integer() {1, 0, 0, 131072})
		resources.ApplyResources(Me.nudDgain3, "nudDgain3")
		Me.nudDgain3.Maximum = New Decimal(New Integer() {300, 0, 0, 0})
		Me.nudDgain3.Name = "nudDgain3"
		'
		'nudD3
		'
		Me.nudD3.DecimalPlaces = 2
		Me.nudD3.Increment = New Decimal(New Integer() {1, 0, 0, 131072})
		resources.ApplyResources(Me.nudD3, "nudD3")
		Me.nudD3.Maximum = New Decimal(New Integer() {300, 0, 0, 0})
		Me.nudD3.Name = "nudD3"
		'
		'nudI3
		'
		Me.nudI3.DecimalPlaces = 1
		Me.nudI3.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
		resources.ApplyResources(Me.nudI3, "nudI3")
		Me.nudI3.Maximum = New Decimal(New Integer() {1000, 0, 0, 0})
		Me.nudI3.Name = "nudI3"
		Me.nudI3.Value = New Decimal(New Integer() {1, 0, 0, 0})
		'
		'nudP3
		'
		Me.nudP3.DecimalPlaces = 2
		Me.nudP3.Increment = New Decimal(New Integer() {1, 0, 0, 131072})
		resources.ApplyResources(Me.nudP3, "nudP3")
		Me.nudP3.Maximum = New Decimal(New Integer() {60, 0, 0, 0})
		Me.nudP3.Name = "nudP3"
		Me.nudP3.Value = New Decimal(New Integer() {123, 0, 0, 65536})
		'
		'lblD3
		'
		resources.ApplyResources(Me.lblD3, "lblD3")
		Me.lblD3.Name = "lblD3"
		'
		'lblI3
		'
		resources.ApplyResources(Me.lblI3, "lblI3")
		Me.lblI3.Name = "lblI3"
		'
		'lblPVBias3
		'
		resources.ApplyResources(Me.lblPVBias3, "lblPVBias3")
		Me.lblPVBias3.Name = "lblPVBias3"
		'
		'lblDgain3
		'
		resources.ApplyResources(Me.lblDgain3, "lblDgain3")
		Me.lblDgain3.Name = "lblDgain3"
		'
		'lblP3
		'
		resources.ApplyResources(Me.lblP3, "lblP3")
		Me.lblP3.Name = "lblP3"
		'
		'btnPIDMonitor
		'
		resources.ApplyResources(Me.btnPIDMonitor, "btnPIDMonitor")
		Me.btnPIDMonitor.Name = "btnPIDMonitor"
		Me.btnPIDMonitor.UseVisualStyleBackColor = true
		'
		'gbxPB
		'
		Me.gbxPB.Controls.Add(Me.nudPVGain2)
		Me.gbxPB.Controls.Add(Me.lblPVGain2)
		Me.gbxPB.Controls.Add(Me.nudPVBias2)
		Me.gbxPB.Controls.Add(Me.nudHUpperTemp2)
		Me.gbxPB.Controls.Add(Me.lblUpLimit2)
		Me.gbxPB.Controls.Add(Me.nudDgain2)
		Me.gbxPB.Controls.Add(Me.nudD2)
		Me.gbxPB.Controls.Add(Me.nudI2)
		Me.gbxPB.Controls.Add(Me.nudP2)
		Me.gbxPB.Controls.Add(Me.lblD2)
		Me.gbxPB.Controls.Add(Me.lblI2)
		Me.gbxPB.Controls.Add(Me.lblPVBias2)
		Me.gbxPB.Controls.Add(Me.lblDgain2)
		Me.gbxPB.Controls.Add(Me.lblP2)
		resources.ApplyResources(Me.gbxPB, "gbxPB")
		Me.gbxPB.Name = "gbxPB"
		Me.gbxPB.TabStop = false
		'
		'nudPVGain2
		'
		Me.nudPVGain2.DecimalPlaces = 4
		Me.nudPVGain2.Increment = New Decimal(New Integer() {1, 0, 0, 131072})
		resources.ApplyResources(Me.nudPVGain2, "nudPVGain2")
		Me.nudPVGain2.Maximum = New Decimal(New Integer() {300, 0, 0, 0})
		Me.nudPVGain2.Minimum = New Decimal(New Integer() {1, 0, 0, 131072})
		Me.nudPVGain2.Name = "nudPVGain2"
		Me.nudPVGain2.Value = New Decimal(New Integer() {1, 0, 0, 0})
		'
		'lblPVGain2
		'
		resources.ApplyResources(Me.lblPVGain2, "lblPVGain2")
		Me.lblPVGain2.Name = "lblPVGain2"
		'
		'nudPVBias2
		'
		Me.nudPVBias2.DecimalPlaces = 1
		Me.nudPVBias2.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
		resources.ApplyResources(Me.nudPVBias2, "nudPVBias2")
		Me.nudPVBias2.Maximum = New Decimal(New Integer() {10, 0, 0, 0})
		Me.nudPVBias2.Minimum = New Decimal(New Integer() {10, 0, 0, -2147483648})
		Me.nudPVBias2.Name = "nudPVBias2"
		'
		'nudHUpperTemp2
		'
		Me.nudHUpperTemp2.DecimalPlaces = 1
		Me.nudHUpperTemp2.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
		resources.ApplyResources(Me.nudHUpperTemp2, "nudHUpperTemp2")
		Me.nudHUpperTemp2.Maximum = New Decimal(New Integer() {400, 0, 0, 0})
		Me.nudHUpperTemp2.Name = "nudHUpperTemp2"
		Me.nudHUpperTemp2.Value = New Decimal(New Integer() {30, 0, 0, 0})
		'
		'lblUpLimit2
		'
		resources.ApplyResources(Me.lblUpLimit2, "lblUpLimit2")
		Me.lblUpLimit2.Name = "lblUpLimit2"
		'
		'nudDgain2
		'
		Me.nudDgain2.DecimalPlaces = 2
		Me.nudDgain2.Increment = New Decimal(New Integer() {1, 0, 0, 131072})
		resources.ApplyResources(Me.nudDgain2, "nudDgain2")
		Me.nudDgain2.Maximum = New Decimal(New Integer() {300, 0, 0, 0})
		Me.nudDgain2.Name = "nudDgain2"
		'
		'nudD2
		'
		Me.nudD2.DecimalPlaces = 2
		Me.nudD2.Increment = New Decimal(New Integer() {1, 0, 0, 131072})
		resources.ApplyResources(Me.nudD2, "nudD2")
		Me.nudD2.Maximum = New Decimal(New Integer() {300, 0, 0, 0})
		Me.nudD2.Name = "nudD2"
		'
		'nudI2
		'
		Me.nudI2.DecimalPlaces = 1
		Me.nudI2.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
		resources.ApplyResources(Me.nudI2, "nudI2")
		Me.nudI2.Maximum = New Decimal(New Integer() {1000, 0, 0, 0})
		Me.nudI2.Name = "nudI2"
		Me.nudI2.Value = New Decimal(New Integer() {1, 0, 0, 0})
		'
		'nudP2
		'
		Me.nudP2.DecimalPlaces = 2
		Me.nudP2.Increment = New Decimal(New Integer() {1, 0, 0, 131072})
		resources.ApplyResources(Me.nudP2, "nudP2")
		Me.nudP2.Maximum = New Decimal(New Integer() {60, 0, 0, 0})
		Me.nudP2.Name = "nudP2"
		Me.nudP2.Value = New Decimal(New Integer() {123, 0, 0, 65536})
		'
		'lblD2
		'
		resources.ApplyResources(Me.lblD2, "lblD2")
		Me.lblD2.Name = "lblD2"
		'
		'lblI2
		'
		resources.ApplyResources(Me.lblI2, "lblI2")
		Me.lblI2.Name = "lblI2"
		'
		'lblPVBias2
		'
		resources.ApplyResources(Me.lblPVBias2, "lblPVBias2")
		Me.lblPVBias2.Name = "lblPVBias2"
		'
		'lblDgain2
		'
		resources.ApplyResources(Me.lblDgain2, "lblDgain2")
		Me.lblDgain2.Name = "lblDgain2"
		'
		'lblP2
		'
		resources.ApplyResources(Me.lblP2, "lblP2")
		Me.lblP2.Name = "lblP2"
		'
		'gbxHP
		'
		Me.gbxHP.Controls.Add(Me.nudPVGain)
		Me.gbxHP.Controls.Add(Me.nudPVBias)
		Me.gbxHP.Controls.Add(Me.nudHUpperTemp)
		Me.gbxHP.Controls.Add(Me.lblUpLimit)
		Me.gbxHP.Controls.Add(Me.nudDgain)
		Me.gbxHP.Controls.Add(Me.nudD)
		Me.gbxHP.Controls.Add(Me.nudI)
		Me.gbxHP.Controls.Add(Me.nudP)
		Me.gbxHP.Controls.Add(Me.lblD)
		Me.gbxHP.Controls.Add(Me.lblI)
		Me.gbxHP.Controls.Add(Me.lblPVGain)
		Me.gbxHP.Controls.Add(Me.lblPVBias)
		Me.gbxHP.Controls.Add(Me.lblDgain)
		Me.gbxHP.Controls.Add(Me.lblP)
		resources.ApplyResources(Me.gbxHP, "gbxHP")
		Me.gbxHP.Name = "gbxHP"
		Me.gbxHP.TabStop = false
		'
		'nudPVGain
		'
		Me.nudPVGain.DecimalPlaces = 4
		Me.nudPVGain.Increment = New Decimal(New Integer() {1, 0, 0, 131072})
		resources.ApplyResources(Me.nudPVGain, "nudPVGain")
		Me.nudPVGain.Maximum = New Decimal(New Integer() {300, 0, 0, 0})
		Me.nudPVGain.Minimum = New Decimal(New Integer() {1, 0, 0, 131072})
		Me.nudPVGain.Name = "nudPVGain"
		Me.nudPVGain.Value = New Decimal(New Integer() {1, 0, 0, 0})
		'
		'nudPVBias
		'
		Me.nudPVBias.DecimalPlaces = 1
		Me.nudPVBias.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
		resources.ApplyResources(Me.nudPVBias, "nudPVBias")
		Me.nudPVBias.Maximum = New Decimal(New Integer() {10, 0, 0, 0})
		Me.nudPVBias.Minimum = New Decimal(New Integer() {10, 0, 0, -2147483648})
		Me.nudPVBias.Name = "nudPVBias"
		'
		'nudHUpperTemp
		'
		Me.nudHUpperTemp.DecimalPlaces = 1
		Me.nudHUpperTemp.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
		resources.ApplyResources(Me.nudHUpperTemp, "nudHUpperTemp")
		Me.nudHUpperTemp.Maximum = New Decimal(New Integer() {400, 0, 0, 0})
		Me.nudHUpperTemp.Name = "nudHUpperTemp"
		Me.nudHUpperTemp.Value = New Decimal(New Integer() {30, 0, 0, 0})
		'
		'lblUpLimit
		'
		resources.ApplyResources(Me.lblUpLimit, "lblUpLimit")
		Me.lblUpLimit.Name = "lblUpLimit"
		'
		'nudDgain
		'
		Me.nudDgain.DecimalPlaces = 2
		Me.nudDgain.Increment = New Decimal(New Integer() {1, 0, 0, 131072})
		resources.ApplyResources(Me.nudDgain, "nudDgain")
		Me.nudDgain.Maximum = New Decimal(New Integer() {300, 0, 0, 0})
		Me.nudDgain.Name = "nudDgain"
		'
		'nudD
		'
		Me.nudD.DecimalPlaces = 2
		Me.nudD.Increment = New Decimal(New Integer() {1, 0, 0, 131072})
		resources.ApplyResources(Me.nudD, "nudD")
		Me.nudD.Maximum = New Decimal(New Integer() {300, 0, 0, 0})
		Me.nudD.Name = "nudD"
		'
		'nudI
		'
		Me.nudI.DecimalPlaces = 1
		Me.nudI.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
		resources.ApplyResources(Me.nudI, "nudI")
		Me.nudI.Maximum = New Decimal(New Integer() {1000, 0, 0, 0})
		Me.nudI.Name = "nudI"
		Me.nudI.Value = New Decimal(New Integer() {1, 0, 0, 0})
		'
		'nudP
		'
		Me.nudP.DecimalPlaces = 2
		Me.nudP.Increment = New Decimal(New Integer() {1, 0, 0, 131072})
		resources.ApplyResources(Me.nudP, "nudP")
		Me.nudP.Maximum = New Decimal(New Integer() {60, 0, 0, 0})
		Me.nudP.Name = "nudP"
		Me.nudP.Value = New Decimal(New Integer() {123, 0, 0, 65536})
		'
		'lblD
		'
		resources.ApplyResources(Me.lblD, "lblD")
		Me.lblD.Name = "lblD"
		'
		'lblI
		'
		resources.ApplyResources(Me.lblI, "lblI")
		Me.lblI.Name = "lblI"
		'
		'lblPVGain
		'
		resources.ApplyResources(Me.lblPVGain, "lblPVGain")
		Me.lblPVGain.Name = "lblPVGain"
		'
		'lblPVBias
		'
		resources.ApplyResources(Me.lblPVBias, "lblPVBias")
		Me.lblPVBias.Name = "lblPVBias"
		'
		'lblDgain
		'
		resources.ApplyResources(Me.lblDgain, "lblDgain")
		Me.lblDgain.Name = "lblDgain"
		'
		'lblP
		'
		resources.ApplyResources(Me.lblP, "lblP")
		Me.lblP.Name = "lblP"
		'
		'cbxTempLEDEn
		'
		resources.ApplyResources(Me.cbxTempLEDEn, "cbxTempLEDEn")
		Me.cbxTempLEDEn.Checked = true
		Me.cbxTempLEDEn.CheckState = System.Windows.Forms.CheckState.Checked
		Me.cbxTempLEDEn.Name = "cbxTempLEDEn"
		Me.cbxTempLEDEn.UseVisualStyleBackColor = true
		'
		'lblLEDTempObsrv
		'
		resources.ApplyResources(Me.lblLEDTempObsrv, "lblLEDTempObsrv")
		Me.lblLEDTempObsrv.Name = "lblLEDTempObsrv"
		'
		'nudLEDTempObsrv
		'
		Me.nudLEDTempObsrv.DecimalPlaces = 1
		Me.nudLEDTempObsrv.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
		resources.ApplyResources(Me.nudLEDTempObsrv, "nudLEDTempObsrv")
		Me.nudLEDTempObsrv.Maximum = New Decimal(New Integer() {300, 0, 0, 0})
		Me.nudLEDTempObsrv.Minimum = New Decimal(New Integer() {30, 0, 0, 0})
		Me.nudLEDTempObsrv.Name = "nudLEDTempObsrv"
		Me.nudLEDTempObsrv.Value = New Decimal(New Integer() {30, 0, 0, 0})
		'
		'TabPage9
		'
		Me.TabPage9.Controls.Add(Me.gbxZCalib)
		Me.TabPage9.Controls.Add(Me.gbxTempProfile)
		resources.ApplyResources(Me.TabPage9, "TabPage9")
		Me.TabPage9.Name = "TabPage9"
		Me.TabPage9.UseVisualStyleBackColor = true
		'
		'gbxZCalib
		'
		Me.gbxZCalib.Controls.Add(Me.lblCalibOK)
		Me.gbxZCalib.Controls.Add(Me.lblRemTimeVal)
		Me.gbxZCalib.Controls.Add(Me.lblPosDeviLabel)
		Me.gbxZCalib.Controls.Add(Me.btnTempGraph)
		Me.gbxZCalib.Controls.Add(Me.lblTempPreLabel)
		Me.gbxZCalib.Controls.Add(Me.lblRemTimeLabel)
		Me.gbxZCalib.Controls.Add(Me.btnZPosCalib)
		Me.gbxZCalib.Controls.Add(Me.lblPosDeviVal)
		Me.gbxZCalib.Controls.Add(Me.lblTempPreVal)
		resources.ApplyResources(Me.gbxZCalib, "gbxZCalib")
		Me.gbxZCalib.Name = "gbxZCalib"
		Me.gbxZCalib.TabStop = false
		'
		'lblCalibOK
		'
		Me.lblCalibOK.BackColor = System.Drawing.Color.GreenYellow
		Me.lblCalibOK.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		resources.ApplyResources(Me.lblCalibOK, "lblCalibOK")
		Me.lblCalibOK.Name = "lblCalibOK"
		'
		'lblRemTimeVal
		'
		resources.ApplyResources(Me.lblRemTimeVal, "lblRemTimeVal")
		Me.lblRemTimeVal.Name = "lblRemTimeVal"
		'
		'lblPosDeviLabel
		'
		resources.ApplyResources(Me.lblPosDeviLabel, "lblPosDeviLabel")
		Me.lblPosDeviLabel.Name = "lblPosDeviLabel"
		'
		'btnTempGraph
		'
		resources.ApplyResources(Me.btnTempGraph, "btnTempGraph")
		Me.btnTempGraph.Name = "btnTempGraph"
		Me.btnTempGraph.UseVisualStyleBackColor = true
		'
		'lblTempPreLabel
		'
		resources.ApplyResources(Me.lblTempPreLabel, "lblTempPreLabel")
		Me.lblTempPreLabel.Name = "lblTempPreLabel"
		'
		'lblRemTimeLabel
		'
		resources.ApplyResources(Me.lblRemTimeLabel, "lblRemTimeLabel")
		Me.lblRemTimeLabel.Name = "lblRemTimeLabel"
		'
		'btnZPosCalib
		'
		resources.ApplyResources(Me.btnZPosCalib, "btnZPosCalib")
		Me.btnZPosCalib.Name = "btnZPosCalib"
		Me.btnZPosCalib.UseVisualStyleBackColor = true
		'
		'lblPosDeviVal
		'
		resources.ApplyResources(Me.lblPosDeviVal, "lblPosDeviVal")
		Me.lblPosDeviVal.Name = "lblPosDeviVal"
		'
		'lblTempPreVal
		'
		resources.ApplyResources(Me.lblTempPreVal, "lblTempPreVal")
		Me.lblTempPreVal.Name = "lblTempPreVal"
		'
		'gbxTempProfile
		'
		Me.gbxTempProfile.Controls.Add(Me.btnRtgsImp)
		Me.gbxTempProfile.Controls.Add(Me.btnRtgsExp)
		Me.gbxTempProfile.Controls.Add(Me.btnTempClear)
		Me.gbxTempProfile.Controls.Add(Me.btnRTGLinearTool)
		Me.gbxTempProfile.Controls.Add(Me.btnTempRepeat)
		Me.gbxTempProfile.Controls.Add(Me.btnSaveRtgs)
		Me.gbxTempProfile.Controls.Add(Me.btnDeleteRtgs)
		Me.gbxTempProfile.Controls.Add(Me.dgvRtgSetting)
		Me.gbxTempProfile.Controls.Add(Me.txtRtgsetName)
		Me.gbxTempProfile.Controls.Add(Me.lblTempProfileName)
		Me.gbxTempProfile.Controls.Add(Me.lvRtgset)
		resources.ApplyResources(Me.gbxTempProfile, "gbxTempProfile")
		Me.gbxTempProfile.Name = "gbxTempProfile"
		Me.gbxTempProfile.TabStop = false
		'
		'btnRtgsImp
		'
		resources.ApplyResources(Me.btnRtgsImp, "btnRtgsImp")
		Me.btnRtgsImp.Name = "btnRtgsImp"
		Me.btnRtgsImp.UseVisualStyleBackColor = true
		'
		'btnRtgsExp
		'
		resources.ApplyResources(Me.btnRtgsExp, "btnRtgsExp")
		Me.btnRtgsExp.Name = "btnRtgsExp"
		Me.btnRtgsExp.UseVisualStyleBackColor = true
		'
		'btnTempClear
		'
		resources.ApplyResources(Me.btnTempClear, "btnTempClear")
		Me.btnTempClear.Name = "btnTempClear"
		Me.btnTempClear.UseVisualStyleBackColor = true
		'
		'btnRTGLinearTool
		'
		resources.ApplyResources(Me.btnRTGLinearTool, "btnRTGLinearTool")
		Me.btnRTGLinearTool.Name = "btnRTGLinearTool"
		Me.btnRTGLinearTool.UseVisualStyleBackColor = true
		'
		'btnTempRepeat
		'
		resources.ApplyResources(Me.btnTempRepeat, "btnTempRepeat")
		Me.btnTempRepeat.Name = "btnTempRepeat"
		Me.btnTempRepeat.UseVisualStyleBackColor = true
		'
		'btnSaveRtgs
		'
		Me.btnSaveRtgs.BackColor = System.Drawing.SystemColors.Control
		resources.ApplyResources(Me.btnSaveRtgs, "btnSaveRtgs")
		Me.btnSaveRtgs.Name = "btnSaveRtgs"
		Me.btnSaveRtgs.UseVisualStyleBackColor = false
		'
		'btnDeleteRtgs
		'
		resources.ApplyResources(Me.btnDeleteRtgs, "btnDeleteRtgs")
		Me.btnDeleteRtgs.Name = "btnDeleteRtgs"
		Me.btnDeleteRtgs.UseVisualStyleBackColor = true
		'
		'dgvRtgSetting
		'
		Me.dgvRtgSetting.AllowUserToResizeColumns = false
		Me.dgvRtgSetting.AllowUserToResizeRows = false
		Me.dgvRtgSetting.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
		Me.dgvRtgSetting.BackgroundColor = System.Drawing.SystemColors.Control
		Me.dgvRtgSetting.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
		Me.dgvRtgSetting.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Column1, Me.Column2, Me.Column3, Me.Column4})
		resources.ApplyResources(Me.dgvRtgSetting, "dgvRtgSetting")
		Me.dgvRtgSetting.Name = "dgvRtgSetting"
		Me.dgvRtgSetting.RowTemplate.Height = 21
		'
		'Column1
		'
		Me.Column1.FillWeight = 59.24399!
		resources.ApplyResources(Me.Column1, "Column1")
		Me.Column1.Name = "Column1"
		Me.Column1.ReadOnly = true
		Me.Column1.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
		'
		'Column2
		'
		DataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight
		DataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.White
		Me.Column2.DefaultCellStyle = DataGridViewCellStyle3
		Me.Column2.FillWeight = 76.6683!
		resources.ApplyResources(Me.Column2, "Column2")
		Me.Column2.Name = "Column2"
		Me.Column2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
		'
		'Column3
		'
		Me.Column3.FillWeight = 79.019!
		resources.ApplyResources(Me.Column3, "Column3")
		Me.Column3.Name = "Column3"
		Me.Column3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
		'
		'Column4
		'
		Me.Column4.FillWeight = 76.84538!
		resources.ApplyResources(Me.Column4, "Column4")
		Me.Column4.Name = "Column4"
		Me.Column4.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
		'
		'txtRtgsetName
		'
		resources.ApplyResources(Me.txtRtgsetName, "txtRtgsetName")
		Me.txtRtgsetName.Name = "txtRtgsetName"
		'
		'lblTempProfileName
		'
		resources.ApplyResources(Me.lblTempProfileName, "lblTempProfileName")
		Me.lblTempProfileName.Name = "lblTempProfileName"
		'
		'lvRtgset
		'
		Me.lvRtgset.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader4})
		Me.lvRtgset.FullRowSelect = true
		Me.lvRtgset.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None
		Me.lvRtgset.HideSelection = false
		resources.ApplyResources(Me.lvRtgset, "lvRtgset")
		Me.lvRtgset.MultiSelect = false
		Me.lvRtgset.Name = "lvRtgset"
		Me.lvRtgset.UseCompatibleStateImageBehavior = false
		Me.lvRtgset.View = System.Windows.Forms.View.Details
		'
		'ColumnHeader4
		'
		resources.ApplyResources(Me.ColumnHeader4, "ColumnHeader4")
		'
		'TabPage4
		'
		Me.TabPage4.Controls.Add(Me.gbxQRCode)
		Me.TabPage4.Controls.Add(Me.gbxLongMeasMode)
		Me.TabPage4.Controls.Add(Me.gbxMeasCont)
		resources.ApplyResources(Me.TabPage4, "TabPage4")
		Me.TabPage4.Name = "TabPage4"
		Me.TabPage4.UseVisualStyleBackColor = true
		'
		'gbxQRCode
		'
		Me.gbxQRCode.Controls.Add(Me.cboECC)
		Me.gbxQRCode.Controls.Add(Me.nudPixelInCell)
		Me.gbxQRCode.Controls.Add(Me.nudCellsInLogo)
		Me.gbxQRCode.Controls.Add(Me.cbxQRCodeEnable)
		Me.gbxQRCode.Controls.Add(Me.lblPixelInCell)
		Me.gbxQRCode.Controls.Add(Me.lblCellsInLogo)
		Me.gbxQRCode.Controls.Add(Me.lblECCLabel)
		resources.ApplyResources(Me.gbxQRCode, "gbxQRCode")
		Me.gbxQRCode.Name = "gbxQRCode"
		Me.gbxQRCode.TabStop = false
		'
		'cboECC
		'
		Me.cboECC.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.cboECC.FormattingEnabled = true
		resources.ApplyResources(Me.cboECC, "cboECC")
		Me.cboECC.Name = "cboECC"
		'
		'nudPixelInCell
		'
		resources.ApplyResources(Me.nudPixelInCell, "nudPixelInCell")
		Me.nudPixelInCell.Maximum = New Decimal(New Integer() {80, 0, 0, 0})
		Me.nudPixelInCell.Minimum = New Decimal(New Integer() {6, 0, 0, 0})
		Me.nudPixelInCell.Name = "nudPixelInCell"
		Me.nudPixelInCell.Value = New Decimal(New Integer() {20, 0, 0, 0})
		'
		'nudCellsInLogo
		'
		resources.ApplyResources(Me.nudCellsInLogo, "nudCellsInLogo")
		Me.nudCellsInLogo.Maximum = New Decimal(New Integer() {20, 0, 0, 0})
		Me.nudCellsInLogo.Minimum = New Decimal(New Integer() {6, 0, 0, 0})
		Me.nudCellsInLogo.Name = "nudCellsInLogo"
		Me.nudCellsInLogo.Value = New Decimal(New Integer() {9, 0, 0, 0})
		'
		'cbxQRCodeEnable
		'
		resources.ApplyResources(Me.cbxQRCodeEnable, "cbxQRCodeEnable")
		Me.cbxQRCodeEnable.Name = "cbxQRCodeEnable"
		Me.cbxQRCodeEnable.UseVisualStyleBackColor = true
		'
		'lblPixelInCell
		'
		resources.ApplyResources(Me.lblPixelInCell, "lblPixelInCell")
		Me.lblPixelInCell.Name = "lblPixelInCell"
		'
		'lblCellsInLogo
		'
		resources.ApplyResources(Me.lblCellsInLogo, "lblCellsInLogo")
		Me.lblCellsInLogo.Name = "lblCellsInLogo"
		'
		'lblECCLabel
		'
		resources.ApplyResources(Me.lblECCLabel, "lblECCLabel")
		Me.lblECCLabel.Name = "lblECCLabel"
		'
		'gbxLongMeasMode
		'
		Me.gbxLongMeasMode.Controls.Add(Me.cbxLongMeasMode)
		Me.gbxLongMeasMode.Controls.Add(Me.nudLTAdoptRate)
		Me.gbxLongMeasMode.Controls.Add(Me.nudLTMeasInterval)
		Me.gbxLongMeasMode.Controls.Add(Me.lblLTAdoptRate)
		Me.gbxLongMeasMode.Controls.Add(Me.lblLTMeasInterval)
		resources.ApplyResources(Me.gbxLongMeasMode, "gbxLongMeasMode")
		Me.gbxLongMeasMode.Name = "gbxLongMeasMode"
		Me.gbxLongMeasMode.TabStop = false
		'
		'cbxLongMeasMode
		'
		resources.ApplyResources(Me.cbxLongMeasMode, "cbxLongMeasMode")
		Me.cbxLongMeasMode.Name = "cbxLongMeasMode"
		Me.cbxLongMeasMode.UseVisualStyleBackColor = true
		'
		'nudLTAdoptRate
		'
		resources.ApplyResources(Me.nudLTAdoptRate, "nudLTAdoptRate")
		Me.nudLTAdoptRate.Maximum = New Decimal(New Integer() {8, 0, 0, 0})
		Me.nudLTAdoptRate.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
		Me.nudLTAdoptRate.Name = "nudLTAdoptRate"
		Me.nudLTAdoptRate.Value = New Decimal(New Integer() {8, 0, 0, 0})
		'
		'nudLTMeasInterval
		'
		resources.ApplyResources(Me.nudLTMeasInterval, "nudLTMeasInterval")
		Me.nudLTMeasInterval.Maximum = New Decimal(New Integer() {60, 0, 0, 0})
		Me.nudLTMeasInterval.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
		Me.nudLTMeasInterval.Name = "nudLTMeasInterval"
		Me.nudLTMeasInterval.Value = New Decimal(New Integer() {60, 0, 0, 0})
		'
		'lblLTAdoptRate
		'
		resources.ApplyResources(Me.lblLTAdoptRate, "lblLTAdoptRate")
		Me.lblLTAdoptRate.Name = "lblLTAdoptRate"
		'
		'lblLTMeasInterval
		'
		resources.ApplyResources(Me.lblLTMeasInterval, "lblLTMeasInterval")
		Me.lblLTMeasInterval.Name = "lblLTMeasInterval"
		'
		'gbxMeasCont
		'
		Me.gbxMeasCont.Controls.Add(Me.ckbGreenFlashInMeas)
		Me.gbxMeasCont.Controls.Add(Me.ckbDoublePBUnlockConfirm)
		Me.gbxMeasCont.Controls.Add(Me.nudGapCorr)
		Me.gbxMeasCont.Controls.Add(Me.gbxStressDetectSetting)
		Me.gbxMeasCont.Controls.Add(Me.lblGapCorrLabel)
		Me.gbxMeasCont.Controls.Add(Me.lblStrainStab)
		Me.gbxMeasCont.Controls.Add(Me.nudStrainStab)
		Me.gbxMeasCont.Controls.Add(Me.lblPresContDeyayTimeLabel)
		Me.gbxMeasCont.Controls.Add(Me.lblSGTempRangeLabel)
		Me.gbxMeasCont.Controls.Add(Me.lblPBTempRangeLabel)
		Me.gbxMeasCont.Controls.Add(Me.nudPresContDeyayTime)
		Me.gbxMeasCont.Controls.Add(Me.nudSGTempRange)
		Me.gbxMeasCont.Controls.Add(Me.nudPBTempRange)
		Me.gbxMeasCont.Controls.Add(Me.nudHPTempRange)
		Me.gbxMeasCont.Controls.Add(Me.lblHPTempRangeLabel)
		Me.gbxMeasCont.Controls.Add(Me.nudInputGap)
		Me.gbxMeasCont.Controls.Add(Me.lblInputGapLabel)
		Me.gbxMeasCont.Controls.Add(Me.nudSampleInputWait)
		Me.gbxMeasCont.Controls.Add(Me.lblWaitForInputLabel)
		resources.ApplyResources(Me.gbxMeasCont, "gbxMeasCont")
		Me.gbxMeasCont.Name = "gbxMeasCont"
		Me.gbxMeasCont.TabStop = false
		'
		'ckbGreenFlashInMeas
		'
		resources.ApplyResources(Me.ckbGreenFlashInMeas, "ckbGreenFlashInMeas")
		Me.ckbGreenFlashInMeas.Name = "ckbGreenFlashInMeas"
		Me.ckbGreenFlashInMeas.UseVisualStyleBackColor = true
		'
		'ckbDoublePBUnlockConfirm
		'
		resources.ApplyResources(Me.ckbDoublePBUnlockConfirm, "ckbDoublePBUnlockConfirm")
		Me.ckbDoublePBUnlockConfirm.Name = "ckbDoublePBUnlockConfirm"
		Me.ckbDoublePBUnlockConfirm.UseVisualStyleBackColor = true
		'
		'nudGapCorr
		'
		resources.ApplyResources(Me.nudGapCorr, "nudGapCorr")
		Me.nudGapCorr.Maximum = New Decimal(New Integer() {1000000, 0, 0, 0})
		Me.nudGapCorr.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
		Me.nudGapCorr.Name = "nudGapCorr"
		Me.nudGapCorr.Value = New Decimal(New Integer() {1, 0, 0, 0})
		'
		'gbxStressDetectSetting
		'
		Me.gbxStressDetectSetting.Controls.Add(Me.nudStresDetDuraTime)
		Me.gbxStressDetectSetting.Controls.Add(Me.nudStrsDetADThreshold)
		Me.gbxStressDetectSetting.Controls.Add(Me.nudStrsDetAcqTime)
		Me.gbxStressDetectSetting.Controls.Add(Me.lblDuration)
		Me.gbxStressDetectSetting.Controls.Add(Me.lblADThreshold)
		Me.gbxStressDetectSetting.Controls.Add(Me.lblStrsDetAcqTime)
		resources.ApplyResources(Me.gbxStressDetectSetting, "gbxStressDetectSetting")
		Me.gbxStressDetectSetting.Name = "gbxStressDetectSetting"
		Me.gbxStressDetectSetting.TabStop = false
		'
		'nudStresDetDuraTime
		'
		resources.ApplyResources(Me.nudStresDetDuraTime, "nudStresDetDuraTime")
		Me.nudStresDetDuraTime.Name = "nudStresDetDuraTime"
		Me.nudStresDetDuraTime.Value = New Decimal(New Integer() {3, 0, 0, 0})
		'
		'nudStrsDetADThreshold
		'
		resources.ApplyResources(Me.nudStrsDetADThreshold, "nudStrsDetADThreshold")
		Me.nudStrsDetADThreshold.Maximum = New Decimal(New Integer() {16777, 0, 0, 0})
		Me.nudStrsDetADThreshold.Name = "nudStrsDetADThreshold"
		Me.nudStrsDetADThreshold.Value = New Decimal(New Integer() {5000, 0, 0, 0})
		'
		'nudStrsDetAcqTime
		'
		resources.ApplyResources(Me.nudStrsDetAcqTime, "nudStrsDetAcqTime")
		Me.nudStrsDetAcqTime.Name = "nudStrsDetAcqTime"
		Me.nudStrsDetAcqTime.Value = New Decimal(New Integer() {20, 0, 0, 0})
		'
		'lblDuration
		'
		resources.ApplyResources(Me.lblDuration, "lblDuration")
		Me.lblDuration.Name = "lblDuration"
		'
		'lblADThreshold
		'
		resources.ApplyResources(Me.lblADThreshold, "lblADThreshold")
		Me.lblADThreshold.Name = "lblADThreshold"
		'
		'lblStrsDetAcqTime
		'
		resources.ApplyResources(Me.lblStrsDetAcqTime, "lblStrsDetAcqTime")
		Me.lblStrsDetAcqTime.Name = "lblStrsDetAcqTime"
		'
		'lblGapCorrLabel
		'
		resources.ApplyResources(Me.lblGapCorrLabel, "lblGapCorrLabel")
		Me.lblGapCorrLabel.Name = "lblGapCorrLabel"
		'
		'lblStrainStab
		'
		resources.ApplyResources(Me.lblStrainStab, "lblStrainStab")
		Me.lblStrainStab.Name = "lblStrainStab"
		'
		'nudStrainStab
		'
		Me.nudStrainStab.DecimalPlaces = 3
		Me.nudStrainStab.Increment = New Decimal(New Integer() {1, 0, 0, 196608})
		resources.ApplyResources(Me.nudStrainStab, "nudStrainStab")
		Me.nudStrainStab.Maximum = New Decimal(New Integer() {10, 0, 0, 0})
		Me.nudStrainStab.Name = "nudStrainStab"
		Me.ToolTipOption.SetToolTip(Me.nudStrainStab, resources.GetString("nudStrainStab.ToolTip"))
		Me.nudStrainStab.Value = New Decimal(New Integer() {1, 0, 0, 0})
		'
		'lblPresContDeyayTimeLabel
		'
		resources.ApplyResources(Me.lblPresContDeyayTimeLabel, "lblPresContDeyayTimeLabel")
		Me.lblPresContDeyayTimeLabel.Name = "lblPresContDeyayTimeLabel"
		'
		'lblSGTempRangeLabel
		'
		resources.ApplyResources(Me.lblSGTempRangeLabel, "lblSGTempRangeLabel")
		Me.lblSGTempRangeLabel.Name = "lblSGTempRangeLabel"
		'
		'lblPBTempRangeLabel
		'
		resources.ApplyResources(Me.lblPBTempRangeLabel, "lblPBTempRangeLabel")
		Me.lblPBTempRangeLabel.Name = "lblPBTempRangeLabel"
		'
		'nudPresContDeyayTime
		'
		resources.ApplyResources(Me.nudPresContDeyayTime, "nudPresContDeyayTime")
		Me.nudPresContDeyayTime.Maximum = New Decimal(New Integer() {1000, 0, 0, 0})
		Me.nudPresContDeyayTime.Name = "nudPresContDeyayTime"
		Me.nudPresContDeyayTime.Value = New Decimal(New Integer() {100, 0, 0, 0})
		'
		'nudSGTempRange
		'
		Me.nudSGTempRange.DecimalPlaces = 1
		resources.ApplyResources(Me.nudSGTempRange, "nudSGTempRange")
		Me.nudSGTempRange.Name = "nudSGTempRange"
		Me.nudSGTempRange.Value = New Decimal(New Integer() {5, 0, 0, 65536})
		'
		'nudPBTempRange
		'
		Me.nudPBTempRange.DecimalPlaces = 1
		resources.ApplyResources(Me.nudPBTempRange, "nudPBTempRange")
		Me.nudPBTempRange.Name = "nudPBTempRange"
		Me.nudPBTempRange.Value = New Decimal(New Integer() {5, 0, 0, 65536})
		'
		'nudHPTempRange
		'
		Me.nudHPTempRange.DecimalPlaces = 1
		resources.ApplyResources(Me.nudHPTempRange, "nudHPTempRange")
		Me.nudHPTempRange.Maximum = New Decimal(New Integer() {20, 0, 0, 0})
		Me.nudHPTempRange.Name = "nudHPTempRange"
		Me.nudHPTempRange.Value = New Decimal(New Integer() {5, 0, 0, 65536})
		'
		'lblHPTempRangeLabel
		'
		resources.ApplyResources(Me.lblHPTempRangeLabel, "lblHPTempRangeLabel")
		Me.lblHPTempRangeLabel.Name = "lblHPTempRangeLabel"
		'
		'nudInputGap
		'
		Me.nudInputGap.DecimalPlaces = 3
		Me.nudInputGap.Increment = New Decimal(New Integer() {1, 0, 0, 196608})
		resources.ApplyResources(Me.nudInputGap, "nudInputGap")
		Me.nudInputGap.Maximum = New Decimal(New Integer() {50, 0, 0, 0})
		Me.nudInputGap.Name = "nudInputGap"
		Me.nudInputGap.Value = New Decimal(New Integer() {10, 0, 0, 0})
		'
		'lblInputGapLabel
		'
		resources.ApplyResources(Me.lblInputGapLabel, "lblInputGapLabel")
		Me.lblInputGapLabel.Name = "lblInputGapLabel"
		'
		'nudSampleInputWait
		'
		resources.ApplyResources(Me.nudSampleInputWait, "nudSampleInputWait")
		Me.nudSampleInputWait.Name = "nudSampleInputWait"
		Me.nudSampleInputWait.Value = New Decimal(New Integer() {10, 0, 0, 0})
		'
		'lblWaitForInputLabel
		'
		resources.ApplyResources(Me.lblWaitForInputLabel, "lblWaitForInputLabel")
		Me.lblWaitForInputLabel.Name = "lblWaitForInputLabel"
		'
		'TabPage8
		'
		Me.TabPage8.Controls.Add(Me.GroupBox19)
		Me.TabPage8.Controls.Add(Me.GroupBox23)
		Me.TabPage8.Controls.Add(Me.GroupBox18)
		resources.ApplyResources(Me.TabPage8, "TabPage8")
		Me.TabPage8.Name = "TabPage8"
		Me.TabPage8.UseVisualStyleBackColor = true
		'
		'GroupBox19
		'
		Me.GroupBox19.Controls.Add(Me.btnMeasOperDel)
		Me.GroupBox19.Controls.Add(Me.btnMeasPlaceDel)
		Me.GroupBox19.Controls.Add(Me.cbxMeasOperator)
		Me.GroupBox19.Controls.Add(Me.cbxMeasPlace)
		Me.GroupBox19.Controls.Add(Me.Label20)
		Me.GroupBox19.Controls.Add(Me.Label19)
		resources.ApplyResources(Me.GroupBox19, "GroupBox19")
		Me.GroupBox19.Name = "GroupBox19"
		Me.GroupBox19.TabStop = false
		'
		'btnMeasOperDel
		'
		resources.ApplyResources(Me.btnMeasOperDel, "btnMeasOperDel")
		Me.btnMeasOperDel.Name = "btnMeasOperDel"
		Me.btnMeasOperDel.TabStop = false
		Me.btnMeasOperDel.UseVisualStyleBackColor = true
		'
		'btnMeasPlaceDel
		'
		resources.ApplyResources(Me.btnMeasPlaceDel, "btnMeasPlaceDel")
		Me.btnMeasPlaceDel.Name = "btnMeasPlaceDel"
		Me.btnMeasPlaceDel.TabStop = false
		Me.btnMeasPlaceDel.UseVisualStyleBackColor = true
		'
		'cbxMeasOperator
		'
		Me.cbxMeasOperator.FormattingEnabled = true
		resources.ApplyResources(Me.cbxMeasOperator, "cbxMeasOperator")
		Me.cbxMeasOperator.Name = "cbxMeasOperator"
		'
		'cbxMeasPlace
		'
		Me.cbxMeasPlace.FormattingEnabled = true
		resources.ApplyResources(Me.cbxMeasPlace, "cbxMeasPlace")
		Me.cbxMeasPlace.Name = "cbxMeasPlace"
		'
		'Label20
		'
		resources.ApplyResources(Me.Label20, "Label20")
		Me.Label20.Name = "Label20"
		'
		'Label19
		'
		resources.ApplyResources(Me.Label19, "Label19")
		Me.Label19.Name = "Label19"
		'
		'GroupBox23
		'
		Me.GroupBox23.Controls.Add(Me.Label23)
		Me.GroupBox23.Controls.Add(Me.nudDskSpc)
		Me.GroupBox23.Controls.Add(Me.Label22)
		resources.ApplyResources(Me.GroupBox23, "GroupBox23")
		Me.GroupBox23.Name = "GroupBox23"
		Me.GroupBox23.TabStop = false
		'
		'Label23
		'
		resources.ApplyResources(Me.Label23, "Label23")
		Me.Label23.Name = "Label23"
		'
		'nudDskSpc
		'
		Me.nudDskSpc.Increment = New Decimal(New Integer() {10, 0, 0, 0})
		resources.ApplyResources(Me.nudDskSpc, "nudDskSpc")
		Me.nudDskSpc.Maximum = New Decimal(New Integer() {50, 0, 0, 0})
		Me.nudDskSpc.Minimum = New Decimal(New Integer() {10, 0, 0, 0})
		Me.nudDskSpc.Name = "nudDskSpc"
		Me.nudDskSpc.Value = New Decimal(New Integer() {10, 0, 0, 0})
		'
		'Label22
		'
		resources.ApplyResources(Me.Label22, "Label22")
		Me.Label22.Name = "Label22"
		'
		'GroupBox18
		'
		Me.GroupBox18.Controls.Add(Me.lblFileWarn)
		Me.GroupBox18.Controls.Add(Me.btnSelStrgFldMeasData)
		Me.GroupBox18.Controls.Add(Me.txtStrgFldMeasData)
		Me.GroupBox18.Controls.Add(Me.lblStrgFldMeasData)
		Me.GroupBox18.Controls.Add(Me.chkAutSaveMeasData)
		Me.GroupBox18.Controls.Add(Me.GroupBox20)
		Me.GroupBox18.Controls.Add(Me.btnSelStrgFld)
		Me.GroupBox18.Controls.Add(Me.txtStrgFld)
		Me.GroupBox18.Controls.Add(Me.Label18)
		Me.GroupBox18.Controls.Add(Me.cbxAutPrnt)
		resources.ApplyResources(Me.GroupBox18, "GroupBox18")
		Me.GroupBox18.Name = "GroupBox18"
		Me.GroupBox18.TabStop = false
		'
		'lblFileWarn
		'
		Me.lblFileWarn.ForeColor = System.Drawing.Color.Red
		resources.ApplyResources(Me.lblFileWarn, "lblFileWarn")
		Me.lblFileWarn.Name = "lblFileWarn"
		'
		'btnSelStrgFldMeasData
		'
		resources.ApplyResources(Me.btnSelStrgFldMeasData, "btnSelStrgFldMeasData")
		Me.btnSelStrgFldMeasData.Image = Global.SZKTool.My.Resources.Resources.OpenFolder2
		Me.btnSelStrgFldMeasData.Name = "btnSelStrgFldMeasData"
		Me.btnSelStrgFldMeasData.TabStop = false
		Me.btnSelStrgFldMeasData.UseVisualStyleBackColor = true
		'
		'txtStrgFldMeasData
		'
		resources.ApplyResources(Me.txtStrgFldMeasData, "txtStrgFldMeasData")
		Me.txtStrgFldMeasData.Name = "txtStrgFldMeasData"
		'
		'lblStrgFldMeasData
		'
		resources.ApplyResources(Me.lblStrgFldMeasData, "lblStrgFldMeasData")
		Me.lblStrgFldMeasData.Name = "lblStrgFldMeasData"
		'
		'chkAutSaveMeasData
		'
		resources.ApplyResources(Me.chkAutSaveMeasData, "chkAutSaveMeasData")
		Me.chkAutSaveMeasData.Name = "chkAutSaveMeasData"
		Me.chkAutSaveMeasData.UseVisualStyleBackColor = true
		'
		'GroupBox20
		'
		Me.GroupBox20.Controls.Add(Me.rbtnJpg)
		Me.GroupBox20.Controls.Add(Me.rbtnPdf)
		resources.ApplyResources(Me.GroupBox20, "GroupBox20")
		Me.GroupBox20.Name = "GroupBox20"
		Me.GroupBox20.TabStop = false
		'
		'rbtnJpg
		'
		resources.ApplyResources(Me.rbtnJpg, "rbtnJpg")
		Me.rbtnJpg.Name = "rbtnJpg"
		Me.rbtnJpg.TabStop = true
		Me.rbtnJpg.UseVisualStyleBackColor = true
		'
		'rbtnPdf
		'
		resources.ApplyResources(Me.rbtnPdf, "rbtnPdf")
		Me.rbtnPdf.Name = "rbtnPdf"
		Me.rbtnPdf.TabStop = true
		Me.rbtnPdf.UseVisualStyleBackColor = true
		'
		'btnSelStrgFld
		'
		resources.ApplyResources(Me.btnSelStrgFld, "btnSelStrgFld")
		Me.btnSelStrgFld.Image = Global.SZKTool.My.Resources.Resources.OpenFolder2
		Me.btnSelStrgFld.Name = "btnSelStrgFld"
		Me.btnSelStrgFld.TabStop = false
		Me.btnSelStrgFld.UseVisualStyleBackColor = true
		'
		'txtStrgFld
		'
		resources.ApplyResources(Me.txtStrgFld, "txtStrgFld")
		Me.txtStrgFld.Name = "txtStrgFld"
		'
		'Label18
		'
		resources.ApplyResources(Me.Label18, "Label18")
		Me.Label18.Name = "Label18"
		'
		'cbxAutPrnt
		'
		resources.ApplyResources(Me.cbxAutPrnt, "cbxAutPrnt")
		Me.cbxAutPrnt.Name = "cbxAutPrnt"
		Me.cbxAutPrnt.UseVisualStyleBackColor = true
		'
		'GroupBox5
		'
		Me.GroupBox5.Controls.Add(Me.btnMDKBackupper)
		Me.GroupBox5.Controls.Add(Me.cbCanDelete)
		Me.GroupBox5.Controls.Add(Me.cbModifyRecalc)
		Me.GroupBox5.Controls.Add(Me.cbModifyCF)
		Me.GroupBox5.Controls.Add(Me.cbModifySpec)
		resources.ApplyResources(Me.GroupBox5, "GroupBox5")
		Me.GroupBox5.Name = "GroupBox5"
		Me.GroupBox5.TabStop = false
		'
		'btnMDKBackupper
		'
		resources.ApplyResources(Me.btnMDKBackupper, "btnMDKBackupper")
		Me.btnMDKBackupper.Name = "btnMDKBackupper"
		Me.btnMDKBackupper.UseVisualStyleBackColor = true
		'
		'cbCanDelete
		'
		resources.ApplyResources(Me.cbCanDelete, "cbCanDelete")
		Me.cbCanDelete.Name = "cbCanDelete"
		Me.cbCanDelete.UseVisualStyleBackColor = true
		'
		'cbModifyRecalc
		'
		resources.ApplyResources(Me.cbModifyRecalc, "cbModifyRecalc")
		Me.cbModifyRecalc.Name = "cbModifyRecalc"
		Me.cbModifyRecalc.UseVisualStyleBackColor = true
		'
		'cbModifyCF
		'
		resources.ApplyResources(Me.cbModifyCF, "cbModifyCF")
		Me.cbModifyCF.Name = "cbModifyCF"
		Me.cbModifyCF.UseVisualStyleBackColor = true
		'
		'cbModifySpec
		'
		resources.ApplyResources(Me.cbModifySpec, "cbModifySpec")
		Me.cbModifySpec.Name = "cbModifySpec"
		Me.cbModifySpec.UseVisualStyleBackColor = true
		'
		'TabControl2
		'
		resources.ApplyResources(Me.TabControl2, "TabControl2")
		Me.TabControl2.Name = "TabControl2"
		Me.TabControl2.SelectedIndex = 0
		'
		'btnApply
		'
		resources.ApplyResources(Me.btnApply, "btnApply")
		Me.btnApply.Name = "btnApply"
		Me.btnApply.UseVisualStyleBackColor = true
		'
		'btnCancel
		'
		resources.ApplyResources(Me.btnCancel, "btnCancel")
		Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
		Me.btnCancel.Name = "btnCancel"
		Me.btnCancel.UseVisualStyleBackColor = true
		'
		'btnOK
		'
		resources.ApplyResources(Me.btnOK, "btnOK")
		Me.btnOK.Name = "btnOK"
		Me.btnOK.UseVisualStyleBackColor = true
		'
		'timMain
		'
		'
		'OpenFileDialog1
		'
		Me.OpenFileDialog1.FileName = "OpenFileDialog1"
		'
		'frmOptions
		'
		resources.ApplyResources(Me, "$this")
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.CancelButton = Me.btnCancel
		Me.Controls.Add(Me.TabControl2)
		Me.Controls.Add(Me.GroupBox5)
		Me.Controls.Add(Me.btnApply)
		Me.Controls.Add(Me.btnCancel)
		Me.Controls.Add(Me.btnOK)
		Me.Controls.Add(Me.TabControl1)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
		Me.KeyPreview = true
		Me.MaximizeBox = false
		Me.MinimizeBox = false
		Me.Name = "frmOptions"
		Me.TabControl1.ResumeLayout(false)
		Me.TabPage1.ResumeLayout(false)
		Me.gbxSelManage.ResumeLayout(false)
		Me.gbxSelManage.PerformLayout
		Me.gbxFactorySetting.ResumeLayout(false)
		Me.gbxMainte.ResumeLayout(false)
		Me.gpxSGHeaterOn.ResumeLayout(false)
		CType(Me.nudMainteThr5,System.ComponentModel.ISupportInitialize).EndInit
		Me.gpxHotPlateHeaterOn.ResumeLayout(false)
		CType(Me.nudMainteThr2,System.ComponentModel.ISupportInitialize).EndInit
		Me.gbxPowOn.ResumeLayout(false)
		CType(Me.nudMainteThr1,System.ComponentModel.ISupportInitialize).EndInit
		Me.gpxProbeHeaterOn.ResumeLayout(false)
		CType(Me.nudMainteThr4,System.ComponentModel.ISupportInitialize).EndInit
		Me.gbxXYAct.ResumeLayout(false)
		CType(Me.nudMainteThr3,System.ComponentModel.ISupportInitialize).EndInit
		Me.gbxMdkSet.ResumeLayout(false)
		Me.gbxMdkSet.PerformLayout
		CType(Me.nudStandbyTime,System.ComponentModel.ISupportInitialize).EndInit
		Me.GroupBox1.ResumeLayout(false)
		Me.GroupBox1.PerformLayout
		Me.TabPage10.ResumeLayout(false)
		Me.gbxZeroAdj.ResumeLayout(false)
		Me.gbxZeroAdj.PerformLayout
		CType(Me.nudZeroStartPosHP,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.nudStaMonTime,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.nudStaFlacRange,System.ComponentModel.ISupportInitialize).EndInit
		Me.gbxZeroAdjMode.ResumeLayout(false)
		Me.gbxZeroAdjMode.PerformLayout
		Me.gbxCentPosReg.ResumeLayout(false)
		Me.gbxCentPosReg.PerformLayout
		CType(Me.nudCenterY,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.nudCenterX,System.ComponentModel.ISupportInitialize).EndInit
		Me.gbxHorizontalConf.ResumeLayout(false)
		Me.gbxXYModeDrive.ResumeLayout(false)
		Me.gbxXYModeDrive.PerformLayout
		CType(Me.nudZPeriod,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.nudZAmp,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.nudYPeriod,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.nudXPeriod,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.nudYAmp,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.nudXAmp,System.ComponentModel.ISupportInitialize).EndInit
		Me.gbxManualDrive.ResumeLayout(false)
		Me.gbxManualDrive.PerformLayout
		CType(Me.nudYAxisPos,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.nudXAxisPos,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.nudHeatPos,System.ComponentModel.ISupportInitialize).EndInit
		Me.gbxCentering.ResumeLayout(false)
		Me.gbxCentering.PerformLayout
		CType(Me.nudCentStartPosHP,System.ComponentModel.ISupportInitialize).EndInit
		Me.TabPage2.ResumeLayout(false)
		Me.grpDefClr.ResumeLayout(false)
		Me.grpDefClr.PerformLayout
		Me.gbxMarker.ResumeLayout(false)
		Me.gbxMarker.PerformLayout
		CType(Me.nudMarkerSize,System.ComponentModel.ISupportInitialize).EndInit
		Me.gbxStressUnit.ResumeLayout(false)
		Me.gbxStressUnit.PerformLayout
		Me.gbxGraphYSpan.ResumeLayout(false)
		Me.gbxGraphYSpan.PerformLayout
		Me.gbxGraphXSpan.ResumeLayout(false)
		Me.gbxGraphXSpan.PerformLayout
		Me.GroupBox14.ResumeLayout(false)
		Me.GroupBox14.PerformLayout
		Me.GroupBox7.ResumeLayout(false)
		Me.GroupBox7.PerformLayout
		CType(Me.nudScrolWidth,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.nudThinningValueD,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.nudThinningValueR,System.ComponentModel.ISupportInitialize).EndInit
		Me.GroupBox4.ResumeLayout(false)
		Me.GroupBox4.PerformLayout
		Me.GroupBox3.ResumeLayout(false)
		Me.GroupBox3.PerformLayout
		Me.TabPage3.ResumeLayout(false)
		Me.gbxSPC.ResumeLayout(false)
		Me.gbxSPC.PerformLayout
		CType(Me.nudSPCCalcSample,System.ComponentModel.ISupportInitialize).EndInit
		Me.gbxAutoReg.ResumeLayout(false)
		Me.gbxAutoReg.PerformLayout
		Me.gbxZeroCalib.ResumeLayout(false)
		Me.gbxWeightCalib.ResumeLayout(false)
		Me.gbxFilter.ResumeLayout(false)
		Me.gbxFilter.PerformLayout
		Me.gbxFilterIIR.ResumeLayout(false)
		Me.gbxFilterIIR.PerformLayout
		CType(Me.nudIIRDeg,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.nudIIRCoffFreq,System.ComponentModel.ISupportInitialize).EndInit
		Me.gbxFilterFIR.ResumeLayout(false)
		Me.gbxFilterFIR.PerformLayout
		CType(Me.nudFilterLen,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.nudFIRCoffFreq,System.ComponentModel.ISupportInitialize).EndInit
		Me.gbxFullCalib.ResumeLayout(false)
		Me.TabPage5.ResumeLayout(false)
		Me.gpxHiddenSetting.ResumeLayout(false)
		Me.gpxHiddenSetting.PerformLayout
		Me.gbxZUpperLimit.ResumeLayout(false)
		CType(Me.nudZUpperLimit,System.ComponentModel.ISupportInitialize).EndInit
		Me.gbxBuzzerControl.ResumeLayout(false)
		Me.gbxBuzzerControl.PerformLayout
		CType(Me.nudBZPattern,System.ComponentModel.ISupportInitialize).EndInit
		Me.gbxTimeSet.ResumeLayout(false)
		Me.gbxTimeSet.PerformLayout
		CType(Me.nudSecond,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.nudMinute,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.nudDay,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.nudHour,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.nudMonth,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.nudYear,System.ComponentModel.ISupportInitialize).EndInit
		Me.GroupBox12.ResumeLayout(false)
		Me.GroupBox12.PerformLayout
		CType(Me.nudAutoStart,System.ComponentModel.ISupportInitialize).EndInit
		Me.GroupBox10.ResumeLayout(false)
		Me.GroupBox10.PerformLayout
		CType(Me.dgvEquipList,System.ComponentModel.ISupportInitialize).EndInit
		Me.TabPage6.ResumeLayout(false)
		Me.gbxSpecList.ResumeLayout(false)
		Me.gbxSpecList.PerformLayout
		CType(Me.dgvSpec,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.dgvCalcOpt,System.ComponentModel.ISupportInitialize).EndInit
		Me.TabPage7.ResumeLayout(false)
		Me.gbxTempControl.ResumeLayout(false)
		Me.gbxTempControl.PerformLayout
		Me.gbxSG.ResumeLayout(false)
		Me.gbxSG.PerformLayout
		CType(Me.nudHDifSetTemp3,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.nudHLowerTemp3,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.nudPVGain3,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.nudPVBias3,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.nudHUpperTemp3,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.nudDgain3,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.nudD3,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.nudI3,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.nudP3,System.ComponentModel.ISupportInitialize).EndInit
		Me.gbxPB.ResumeLayout(false)
		Me.gbxPB.PerformLayout
		CType(Me.nudPVGain2,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.nudPVBias2,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.nudHUpperTemp2,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.nudDgain2,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.nudD2,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.nudI2,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.nudP2,System.ComponentModel.ISupportInitialize).EndInit
		Me.gbxHP.ResumeLayout(false)
		Me.gbxHP.PerformLayout
		CType(Me.nudPVGain,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.nudPVBias,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.nudHUpperTemp,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.nudDgain,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.nudD,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.nudI,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.nudP,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.nudLEDTempObsrv,System.ComponentModel.ISupportInitialize).EndInit
		Me.TabPage9.ResumeLayout(false)
		Me.gbxZCalib.ResumeLayout(false)
		Me.gbxZCalib.PerformLayout
		Me.gbxTempProfile.ResumeLayout(false)
		Me.gbxTempProfile.PerformLayout
		CType(Me.dgvRtgSetting,System.ComponentModel.ISupportInitialize).EndInit
		Me.TabPage4.ResumeLayout(false)
		Me.gbxQRCode.ResumeLayout(false)
		Me.gbxQRCode.PerformLayout
		CType(Me.nudPixelInCell,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.nudCellsInLogo,System.ComponentModel.ISupportInitialize).EndInit
		Me.gbxLongMeasMode.ResumeLayout(false)
		Me.gbxLongMeasMode.PerformLayout
		CType(Me.nudLTAdoptRate,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.nudLTMeasInterval,System.ComponentModel.ISupportInitialize).EndInit
		Me.gbxMeasCont.ResumeLayout(false)
		Me.gbxMeasCont.PerformLayout
		CType(Me.nudGapCorr,System.ComponentModel.ISupportInitialize).EndInit
		Me.gbxStressDetectSetting.ResumeLayout(false)
		Me.gbxStressDetectSetting.PerformLayout
		CType(Me.nudStresDetDuraTime,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.nudStrsDetADThreshold,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.nudStrsDetAcqTime,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.nudStrainStab,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.nudPresContDeyayTime,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.nudSGTempRange,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.nudPBTempRange,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.nudHPTempRange,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.nudInputGap,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.nudSampleInputWait,System.ComponentModel.ISupportInitialize).EndInit
		Me.TabPage8.ResumeLayout(false)
		Me.GroupBox19.ResumeLayout(false)
		Me.GroupBox19.PerformLayout
		Me.GroupBox23.ResumeLayout(false)
		Me.GroupBox23.PerformLayout
		CType(Me.nudDskSpc,System.ComponentModel.ISupportInitialize).EndInit
		Me.GroupBox18.ResumeLayout(false)
		Me.GroupBox18.PerformLayout
		Me.GroupBox20.ResumeLayout(false)
		Me.GroupBox20.PerformLayout
		Me.GroupBox5.ResumeLayout(false)
		Me.GroupBox5.PerformLayout
		Me.ResumeLayout(false)

End Sub

	Friend WithEvents TabControl1 As TabControl
	Friend WithEvents TabPage1 As TabPage
	Friend WithEvents gbxMdkSet As GroupBox
	Friend WithEvents Label15 As Label
	Friend WithEvents cbxEnaIntLock As CheckBox
	Friend WithEvents GroupBox5 As GroupBox
	Friend WithEvents btnMDKBackupper As Button
	Friend WithEvents cbCanDelete As CheckBox
	Friend WithEvents cbModifyRecalc As CheckBox
	Friend WithEvents cbModifyCF As CheckBox
	Friend WithEvents cbModifySpec As CheckBox
	Friend WithEvents GroupBox1 As GroupBox
	Friend WithEvents rbSChinese As RadioButton
	Friend WithEvents rbKorean As RadioButton
	Friend WithEvents rbTChinese As RadioButton
	Friend WithEvents rbJPN As RadioButton
	Friend WithEvents rbENG As RadioButton
	Friend WithEvents TabPage10 As TabPage
	Friend WithEvents gbxCentering As GroupBox
	Friend WithEvents btnExecCenter As Button
	Friend WithEvents gbxZeroAdj As GroupBox
	Friend WithEvents lblStaMonTime As Label
	Friend WithEvents nudStaMonTime As NumericUpDown
	Friend WithEvents nudStaFlacRange As NumericUpDown
	Friend WithEvents Label13 As Label
	Friend WithEvents TabPage2 As TabPage
	Friend WithEvents grpDefClr As GroupBox
	Friend WithEvents lblClrL As Label
	Friend WithEvents lblClrB As Label
	Friend WithEvents lblClr0 As Label
	Friend WithEvents lblClr9 As Label
	Friend WithEvents lblClr8 As Label
	Friend WithEvents lblClr7 As Label
	Friend WithEvents lblClr6 As Label
	Friend WithEvents lblClr5 As Label
	Friend WithEvents lblClr4 As Label
	Friend WithEvents lblClr3 As Label
	Friend WithEvents lblClr2 As Label
	Friend WithEvents lblClr1 As Label
	Friend WithEvents cbY2Opp As CheckBox
	Friend WithEvents gbxGraphXSpan As GroupBox
	Friend WithEvents rbRXauto As RadioButton
	Friend WithEvents rbRXFix As RadioButton
	Friend WithEvents GroupBox14 As GroupBox
	Friend WithEvents rbARDFill As RadioButton
	Friend WithEvents rbARDTransparent As RadioButton
	Friend WithEvents GroupBox7 As GroupBox
	Friend WithEvents GroupBox4 As GroupBox
	Friend WithEvents rbHideNgtv As RadioButton
	Friend WithEvents rbDisplayNgtv As RadioButton
	Friend WithEvents GroupBox3 As GroupBox
	Friend WithEvents rbNoGrids As RadioButton
	Friend WithEvents rbDotGrids As RadioButton
	Friend WithEvents rbDash As RadioButton
	Friend WithEvents rbSolid As RadioButton
	Friend WithEvents TabPage3 As TabPage
	Friend WithEvents TabPage5 As TabPage
	Friend WithEvents GroupBox12 As GroupBox
	Friend WithEvents lblAutoStartTime As Label
	Friend WithEvents Label2 As Label
	Friend WithEvents cbAutoStart As CheckBox
	Friend WithEvents nudAutoStart As NumericUpDown
	Friend WithEvents GroupBox10 As GroupBox
	Friend WithEvents Label3 As Label
	Friend WithEvents dgvEquipList As DataGridView
	Friend WithEvents txtPort As TextBox
	Friend WithEvents TabPage6 As TabPage
	Friend WithEvents gbxSpecList As GroupBox
	Friend WithEvents cbxLotManu As CheckBox
	Friend WithEvents btnSpcImp As Button
	Friend WithEvents btnSpcExp As Button
	Friend WithEvents lblSpecFilter As Label
	Friend WithEvents btnFixFilter As Button
	Friend WithEvents txtFixFilter As TextBox
	Friend WithEvents cboSpecRtgset As ComboBox
	Friend WithEvents lblHotplateSetting As Label
	Friend WithEvents btnSaveSpec As Button
	Friend WithEvents btnDeleteSpec As Button
	Friend WithEvents dgvSpec As DataGridView
	Friend WithEvents DataGridViewTextBoxColumn1 As DataGridViewTextBoxColumn
	Friend WithEvents DataGridViewTextBoxColumn2 As DataGridViewTextBoxColumn
	Friend WithEvents dgvCalcOpt As DataGridView
	Friend WithEvents DataGridViewTextBoxColumn3 As DataGridViewTextBoxColumn
	Friend WithEvents DataGridViewTextBoxColumn4 As DataGridViewTextBoxColumn
	Friend WithEvents lblMethod As Label
	Friend WithEvents cboMethod As ComboBox
	Friend WithEvents cboXYModeOp As ComboBox
	Friend WithEvents lblXYDriveMode As Label
	Friend WithEvents txtSpecName As TextBox
	Friend WithEvents lblSPECName As Label
	Friend WithEvents lvSpec As ListView
	Friend WithEvents ColumnHeader3 As ColumnHeader
	Friend WithEvents TabPage7 As TabPage
	Friend WithEvents gbxTempProfile As GroupBox
	Friend WithEvents btnRtgsImp As Button
	Friend WithEvents btnRtgsExp As Button
	Friend WithEvents btnTempClear As Button
	Friend WithEvents btnRTGLinearTool As Button
	Friend WithEvents btnTempRepeat As Button
	Friend WithEvents btnTempGraph As Button
	Friend WithEvents btnSaveRtgs As Button
	Friend WithEvents btnDeleteRtgs As Button
	Friend WithEvents dgvRtgSetting As DataGridView
	Friend WithEvents Column1 As DataGridViewImageColumn
	Friend WithEvents Column2 As DataGridViewTextBoxColumn
	Friend WithEvents Column3 As DataGridViewTextBoxColumn
	Friend WithEvents Column4 As DataGridViewTextBoxColumn
	Friend WithEvents txtRtgsetName As TextBox
	Friend WithEvents lblTempProfileName As Label
	Friend WithEvents lvRtgset As ListView
	Friend WithEvents ColumnHeader4 As ColumnHeader
	Friend WithEvents TabPage8 As TabPage
	Friend WithEvents GroupBox19 As GroupBox
	Friend WithEvents btnMeasOperDel As Button
	Friend WithEvents btnMeasPlaceDel As Button
	Friend WithEvents cbxMeasOperator As ComboBox
	Friend WithEvents cbxMeasPlace As ComboBox
	Friend WithEvents Label20 As Label
	Friend WithEvents Label19 As Label
	Friend WithEvents GroupBox23 As GroupBox
	Friend WithEvents Label23 As Label
	Friend WithEvents nudDskSpc As NumericUpDown
	Friend WithEvents Label22 As Label
	Friend WithEvents GroupBox18 As GroupBox
	Friend WithEvents lblFileWarn As Label
	Friend WithEvents btnSelStrgFldMeasData As Button
	Friend WithEvents txtStrgFldMeasData As TextBox
	Friend WithEvents lblStrgFldMeasData As Label
	Friend WithEvents chkAutSaveMeasData As CheckBox
	Friend WithEvents GroupBox20 As GroupBox
	Friend WithEvents rbtnJpg As RadioButton
	Friend WithEvents rbtnPdf As RadioButton
	Friend WithEvents btnSelStrgFld As Button
	Friend WithEvents txtStrgFld As TextBox
	Friend WithEvents Label18 As Label
	Friend WithEvents cbxAutPrnt As CheckBox
	Friend WithEvents TabControl2 As TabControl
	Friend WithEvents btnApply As Button
	Friend WithEvents btnCancel As Button
	Friend WithEvents btnOK As Button
	Friend WithEvents timMain As Timer
	Friend WithEvents gbxTempControl As GroupBox
	Friend WithEvents gbxTimeSet As GroupBox
	Friend WithEvents lblTime As Label
	Friend WithEvents lblDate As Label
	Friend WithEvents nudSecond As NumericUpDown
	Friend WithEvents nudMinute As NumericUpDown
	Friend WithEvents nudDay As NumericUpDown
	Friend WithEvents nudHour As NumericUpDown
	Friend WithEvents nudMonth As NumericUpDown
	Friend WithEvents nudYear As NumericUpDown
	Friend WithEvents btnDateAply As Button
	Friend WithEvents btnNow As Button
	Friend WithEvents cbxTempLEDEn As CheckBox
	Friend WithEvents lblLEDTempObsrv As Label
	Protected WithEvents nudLEDTempObsrv As NumericUpDown
	Friend WithEvents cbxCoverILDis As CheckBox
	Friend WithEvents gbxBuzzerControl As GroupBox
	Friend WithEvents cbxOkOfHeat As CheckBox
	Friend WithEvents cbxEndOfMesure As CheckBox
	Friend WithEvents gbxManualDrive As GroupBox
	Friend WithEvents btnYAxisDrive As Button
	Friend WithEvents btnXAxisDrive As Button
	Friend WithEvents btnHotPlateDrive As Button
	Friend WithEvents Label29 As Label
	Friend WithEvents Label28 As Label
	Friend WithEvents Label27 As Label
	Friend WithEvents lblYAxisPos As Label
	Friend WithEvents lblXAxisPos As Label
	Friend WithEvents lblHeatPos As Label
	Friend WithEvents nudYAxisPos As NumericUpDown
	Friend WithEvents nudXAxisPos As NumericUpDown
	Friend WithEvents nudHeatPos As NumericUpDown
	Friend WithEvents lblStaFlacRange As Label
	Friend WithEvents Label30 As Label
	Friend WithEvents Label33 As Label
	Friend WithEvents lblStanbyTime As Label
	Friend WithEvents nudStandbyTime As NumericUpDown
	Friend WithEvents lblStartPosHP As Label
	Friend WithEvents nudZeroStartPosHP As NumericUpDown
	Friend WithEvents Label35 As Label
	Friend WithEvents lblCentStartPosHP As Label
	Friend WithEvents nudCentStartPosHP As NumericUpDown
	Friend WithEvents Label34 As Label
	Friend WithEvents Label39 As Label
	Friend WithEvents Label38 As Label
	Friend WithEvents Label37 As Label
	Friend WithEvents Label36 As Label
	Friend WithEvents lblCenterY As Label
	Friend WithEvents lblCenterX As Label
	Friend WithEvents lblCentPointPos As Label
	Friend WithEvents gbxXYModeDrive As GroupBox
	Friend WithEvents Label44 As Label
	Friend WithEvents Label40 As Label
	Friend WithEvents lblXYModeY As Label
	Friend WithEvents lblXYModeX As Label
	Friend WithEvents nudYPeriod As NumericUpDown
	Friend WithEvents nudXPeriod As NumericUpDown
	Friend WithEvents nudYAmp As NumericUpDown
	Friend WithEvents nudXAmp As NumericUpDown
	Friend WithEvents btnXYStop As Button
	Friend WithEvents btnXYDrive As Button
	Friend WithEvents Label42 As Label
	Friend WithEvents Label41 As Label
	Friend WithEvents gbxFullCalib As GroupBox
	Friend WithEvents btnFullCalib As Button
	Friend WithEvents gbxFilter As GroupBox
	Friend WithEvents btnFilterSelect As Button
	Friend WithEvents lblFilterFileName As Label
	Friend WithEvents OpenFileDialog1 As OpenFileDialog
	Friend WithEvents gbxFilterFIR As GroupBox
	Friend WithEvents cbxFilterEnable As CheckBox
	Friend WithEvents gbxFilterIIR As GroupBox
	Friend WithEvents rbIIR As RadioButton
	Friend WithEvents rbFIR As RadioButton
	Friend WithEvents cbxWinKind As ComboBox
	Friend WithEvents nudFilterLen As NumericUpDown
	Friend WithEvents nudFIRCoffFreq As NumericUpDown
	Friend WithEvents lblFIRWinKind As Label
	Friend WithEvents lblFIRFilterLen As Label
	Friend WithEvents lblFIRCofFreq As Label
	Friend WithEvents nudIIRCoffFreq As NumericUpDown
	Friend WithEvents lblIIRCofFreq As Label
	Friend WithEvents nudIIRDeg As NumericUpDown
	Friend WithEvents lblIIRDegree As Label
	Friend WithEvents gbxMainte As GroupBox
	Friend WithEvents nudMainteThr2 As NumericUpDown
	Friend WithEvents nudMainteThr1 As NumericUpDown
	Friend WithEvents gbxHorizontalConf As GroupBox
	Friend WithEvents btnHorizonConfirm As Button
	Friend WithEvents btnOrigin As Button
	Friend WithEvents lblDevHeatOnTime As Label
	Friend WithEvents lblDevPowOnTime As Label
	Friend WithEvents btnHeatOnTimeReset As Button
	Friend WithEvents btnPowOnTimeReset As Button
	Friend WithEvents gpxHotPlateHeaterOn As GroupBox
	Friend WithEvents lblHpHeaterThr As Label
	Friend WithEvents lblHeaterOnResetDate As Label
	Friend WithEvents lblHeaterOnResetDateL As Label
	Friend WithEvents lblHeaterOnPreVal As Label
	Friend WithEvents gbxPowOn As GroupBox
	Friend WithEvents lblPowOnResetDate As Label
	Friend WithEvents lblPowOnResetDateL As Label
	Friend WithEvents lblPowOnPreVal As Label
	Friend WithEvents lblMainteThr1 As Label
	Friend WithEvents gbxStressDetectSetting As GroupBox
	Friend WithEvents nudStresDetDuraTime As NumericUpDown
	Friend WithEvents nudStrsDetADThreshold As NumericUpDown
	Friend WithEvents nudStrsDetAcqTime As NumericUpDown
	Friend WithEvents lblDuration As Label
	Friend WithEvents lblADThreshold As Label
	Friend WithEvents lblStrsDetAcqTime As Label
	Protected WithEvents nudHUpperTemp As NumericUpDown
	Friend WithEvents lblUpLimit As Label
	Friend WithEvents gbxGraphYSpan As GroupBox
	Friend WithEvents rbRYauto As RadioButton
	Friend WithEvents rbRYFix As RadioButton
	Friend WithEvents gbxStressUnit As GroupBox
	Friend WithEvents Label17 As Label
	Friend WithEvents rbtnNewton As RadioButton
	Friend WithEvents rbtnDigital As RadioButton
	Friend WithEvents rbtnPer As RadioButton
	Friend WithEvents btnSpecCopyFix As Button
	Friend WithEvents btnSpecCopyFree As Button
	Friend WithEvents gbxPB As GroupBox
	Friend WithEvents nudPVBias2 As NumericUpDown
	Protected WithEvents nudHUpperTemp2 As NumericUpDown
	Friend WithEvents lblUpLimit2 As Label
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
	Friend WithEvents btnPIDMonitor As Button
	Friend WithEvents gbxCentPosReg As GroupBox
	Friend WithEvents lblUnit11 As Label
	Friend WithEvents lblUnit10 As Label
	Friend WithEvents btnCenterPosReg As Button
	Friend WithEvents nudCenterY As NumericUpDown
	Friend WithEvents nudCenterX As NumericUpDown
	Friend WithEvents lblCentRegY As Label
	Friend WithEvents lblCentRegX As Label
	Friend WithEvents gbxWeightCalib As GroupBox
	Friend WithEvents btnWeightCalib As Button
	Friend WithEvents nudPVGain2 As NumericUpDown
	Friend WithEvents lblPVGain2 As Label
	Friend WithEvents nudPVGain As NumericUpDown
	Friend WithEvents lblPVGain As Label
	Friend WithEvents gpxProbeHeaterOn As GroupBox
	Friend WithEvents lblProbeHeaterOnResetDate As Label
	Friend WithEvents lblProbeHeaterResetDateL As Label
	Friend WithEvents nudMainteThr4 As NumericUpDown
	Friend WithEvents lblProbeHeaterPreVal As Label
	Friend WithEvents lblProbeHeaterThr As Label
	Friend WithEvents btnProbeHeaterTimeReset As Button
	Friend WithEvents lblProbeHeatOnTime As Label
	Friend WithEvents gbxXYAct As GroupBox
	Friend WithEvents lblXYActResetDate As Label
	Friend WithEvents lblXYActResetDateL As Label
	Friend WithEvents nudMainteThr3 As NumericUpDown
	Friend WithEvents lblXActPreVal As Label
	Friend WithEvents lblXYActThr1 As Label
	Friend WithEvents btnXYActTimeReset As Button
	Friend WithEvents lblXActTime As Label
	Friend WithEvents lblThinningR As Label
	Friend WithEvents nudThinningValueR As NumericUpDown
    Friend WithEvents lblScrollWidth As Label
    Friend WithEvents nudScrolWidth As NumericUpDown
    Friend WithEvents lblThinningD As Label
    Friend WithEvents nudThinningValueD As NumericUpDown
	Friend WithEvents gbxZeroCalib As GroupBox
	Friend WithEvents btnZeroCalib As Button
	Friend WithEvents gbxMarker As GroupBox
	Friend WithEvents lblMarkerStyle As Label
	Friend WithEvents cbxMarkerStyle As ComboBox
	Friend WithEvents nudMarkerSize As NumericUpDown
	Friend WithEvents lblMarkerSize As Label
	Friend WithEvents lblColorAssignDesc As Label
	Friend WithEvents btnDeviceSerWite As Button
	Friend WithEvents gbxZeroAdjMode As GroupBox
	Friend WithEvents rbZeroZcont As RadioButton
	Friend WithEvents rbZeroXcont As RadioButton
	Friend WithEvents Label1 As Label
	Friend WithEvents lblXYModeZ As Label
	Friend WithEvents nudZPeriod As NumericUpDown
	Friend WithEvents nudZAmp As NumericUpDown
	Friend WithEvents Label5 As Label
    Friend WithEvents TabPage4 As TabPage
    Friend WithEvents gbxMeasCont As GroupBox
    Friend WithEvents nudSampleInputWait As NumericUpDown
    Friend WithEvents lblWaitForInputLabel As Label
    Friend WithEvents nudHPTempRange As NumericUpDown
    Friend WithEvents lblHPTempRangeLabel As Label
    Friend WithEvents nudPBTempRange As NumericUpDown
    Friend WithEvents nudPresContDeyayTime As NumericUpDown
	Friend WithEvents lblPBTempRangeLabel As Label
	Friend WithEvents lblPresContDeyayTimeLabel As Label
	Friend WithEvents TabPage9 As TabPage
	Friend WithEvents gbxSG As GroupBox
	Friend WithEvents nudPVGain3 As NumericUpDown
	Friend WithEvents lblPVGain3 As Label
	Friend WithEvents nudPVBias3 As NumericUpDown
	Protected WithEvents nudHUpperTemp3 As NumericUpDown
	Friend WithEvents lblUpLimit3 As Label
	Friend WithEvents nudDgain3 As NumericUpDown
	Friend WithEvents nudD3 As NumericUpDown
	Friend WithEvents nudI3 As NumericUpDown
	Friend WithEvents nudP3 As NumericUpDown
	Friend WithEvents lblD3 As Label
	Friend WithEvents lblI3 As Label
	Friend WithEvents lblPVBias3 As Label
	Friend WithEvents lblDgain3 As Label
	Friend WithEvents lblP3 As Label
	Friend WithEvents lblSGTempRangeLabel As Label
	Friend WithEvents nudSGTempRange As NumericUpDown
	Friend WithEvents gpxSGHeaterOn As GroupBox
	Friend WithEvents lblSGHeaterOnResetDate As Label
	Friend WithEvents lblSGHeaterResetDateL As Label
	Friend WithEvents nudMainteThr5 As NumericUpDown
	Friend WithEvents lblSGHeaterPreVal As Label
	Friend WithEvents lblSGHeaterThr As Label
	Friend WithEvents btnSGHeaterTimeReset As Button
	Friend WithEvents lblSGHeatOnTime As Label
	Protected WithEvents nudHDifSetTemp3 As NumericUpDown
	Friend WithEvents lblDifSet As Label
	Protected WithEvents nudHLowerTemp3 As NumericUpDown
	Friend WithEvents lblLoLimit3 As Label
	Friend WithEvents lblStrainStab As Label
	Friend WithEvents nudStrainStab As NumericUpDown
	Friend WithEvents nudGapCorr As NumericUpDown
	Friend WithEvents lblGapCorrLabel As Label
	Friend WithEvents ToolTipOption As ToolTip
	Friend WithEvents gbxZUpperLimit As GroupBox
	Friend WithEvents nudZUpperLimit As NumericUpDown
	Friend WithEvents nudInputGap As NumericUpDown
	Friend WithEvents lblInputGapLabel As Label
	Friend WithEvents lblYActPreVal As Label
	Friend WithEvents lblZActPreVal As Label
	Friend WithEvents lblZActTime As Label
	Friend WithEvents lblYActTime As Label
	Friend WithEvents cbxDebugFileEn As CheckBox
	Friend WithEvents cbxInputEnMeasureing As CheckBox
	Friend WithEvents cbxCalibValChangeEn As CheckBox
	Friend WithEvents cbxZStabDis As CheckBox
	Friend WithEvents cbxPcLifeChkDis As CheckBox
	Friend WithEvents cbDatafileCalcEnable As CheckBox
	Friend WithEvents SelectColumn As DataGridViewImageColumn
	Friend WithEvents IPColumn As DataGridViewTextBoxColumn
	Friend WithEvents NameColumn As DataGridViewTextBoxColumn
	Friend WithEvents XYSensSerial As DataGridViewTextBoxColumn
	Friend WithEvents ZSensSerial As DataGridViewTextBoxColumn
	Friend WithEvents CF1Column As DataGridViewTextBoxColumn
	Friend WithEvents ToolCF1Column As DataGridViewTextBoxColumn
	Friend WithEvents FTPColumn As DataGridViewButtonColumn
	Friend WithEvents gpxHiddenSetting As GroupBox
	Friend WithEvents cbxMultActEn As CheckBox
	Friend WithEvents btnZPosCalib As Button
	Friend WithEvents gbxZCalib As GroupBox
	Friend WithEvents lblTempPreVal As Label
	Friend WithEvents lblRemTimeVal As Label
	Friend WithEvents lblTempPreLabel As Label
	Friend WithEvents lblRemTimeLabel As Label
	Friend WithEvents lblPosDeviVal As Label
	Friend WithEvents lblPosDeviLabel As Label
	Friend WithEvents lblCalibOK As Label
	Friend WithEvents cbxMeasTimeEn As CheckBox
	Friend WithEvents cbxScanTimeEn As CheckBox
	Friend WithEvents gbxFactorySetting As GroupBox
	Friend WithEvents btnFactorySetting As Button
	Friend WithEvents lblFactorySettingContents As Label
	Friend WithEvents cbxEquipListEn As CheckBox
	Friend WithEvents gbxSelManage As GroupBox
	Friend WithEvents lblSampleNameLabel As Label
	Friend WithEvents cbxSampleName As ComboBox
	Friend WithEvents btnSamplenameResist As Button
	Friend WithEvents lblMeasurerLabel As Label
	Friend WithEvents cbxMeasurer As ComboBox
	Friend WithEvents btnMeasurerDelete As Button
	Friend WithEvents btnSamplenameDelete As Button
	Friend WithEvents btnMeasurerResist As Button
	Friend WithEvents cbxCalcResultFOutEn As CheckBox
	Friend WithEvents cbxReadRealAreaEn As CheckBox
	Friend WithEvents gbxAutoReg As GroupBox
	Friend WithEvents cbxAutoReg As CheckBox
	Friend WithEvents lblAutoRegDesc As Label
	Friend WithEvents gbxLongMeasMode As GroupBox
	Friend WithEvents cbxLongMeasMode As CheckBox
	Friend WithEvents nudLTAdoptRate As NumericUpDown
	Friend WithEvents nudLTMeasInterval As NumericUpDown
	Friend WithEvents lblLTAdoptRate As Label
	Friend WithEvents lblLTMeasInterval As Label
	Friend WithEvents cbxTempNoLimitEn As CheckBox
	Friend WithEvents gbxQRCode As GroupBox
	Friend WithEvents cbxQRCodeEnable As CheckBox
	Friend WithEvents cboECC As ComboBox
	Friend WithEvents nudCellsInLogo As NumericUpDown
	Friend WithEvents lblCellsInLogo As Label
	Friend WithEvents lblECCLabel As Label
	Friend WithEvents lblPixelInCell As Label
	Friend WithEvents nudPixelInCell As NumericUpDown
	Friend WithEvents nudBZPattern As NumericUpDown
	Friend WithEvents lblEndOfBuzzerPattern As Label
	Friend WithEvents btnCreateFactorySetting As Button
	Friend WithEvents gbxSPC As GroupBox
	Friend WithEvents lblSPCDesc As Label
	Friend WithEvents cbxSPC As CheckBox
	Friend WithEvents lblSPCCalcSample As Label
	Friend WithEvents nudSPCCalcSample As NumericUpDown
	Friend WithEvents ckbGreenFlashInMeas As CheckBox
	Friend WithEvents ckbDoublePBUnlockConfirm As CheckBox
End Class
