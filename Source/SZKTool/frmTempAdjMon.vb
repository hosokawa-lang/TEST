Imports System.Drawing.Printing
Imports System.Threading
Imports System.IO
Imports System.Windows.Forms.DataVisualization.Charting
Imports System.Collections
Imports System.Globalization
Imports System.ComponentModel
Imports System.Runtime.CompilerServices
Imports System.Reflection
Imports Microsoft.VisualBasic.FileIO
Imports SZKTool.frmOptions
Imports SZKTool.RealTimeMeas
Imports SZKTool.modPublic

Public Class frmTempAdjMon
'多言語メッセージ格納
Private m_sStr(eMes.iMAX - 1)		As String   
Private m_InitFlag					As Int32 = 1				'ロード初期化中
Enum eMes
	TITLE_PB_PID=0				'プローブ 温度 PIDモニタ
	TITLE_PB_TEMP_MV			'プローブ 温度/MVモニタ
	TITLE_HP_PID				'熱板 温度 PIDモニタ
	TITLE_HP_TEMP_MV			'熱板 温度/MVモニタ
	TITLE_SG_PID				'起歪体 温度 PIDモニタ
	TITLE_SG_TEMP_MV			'起歪体 温度/MVモニタ
	AXIS_Y_VAL					'値[デジタル]
	AXIS_Y_TEMP					'温度[℃]
	AXIS_X_CNT					'200mS[回]
	AXIS_Y_MV					'MV[%]
	iMAX
End Enum
Sub New
    ' この呼び出しはデザイナーで必要です。
    InitializeComponent()
    ' InitializeComponent() 呼び出しの後で初期化を追加します。

End Sub

''' <summary>
''' コントロールのDoubleBufferedプロパティをTrueにする
''' </summary>
''' <param name="control">対象のコントロール</param>
Public Shared Sub EnableDoubleBuffering(control As Control)
    control.GetType().InvokeMember( _
        "DoubleBuffered", _
        BindingFlags.NonPublic Or BindingFlags.Instance Or BindingFlags.SetProperty, _
        Nothing, _
        control, _
        New Object() {True})
End Sub

Private Sub frmTempAdjMon_Load(sender As Object, e As EventArgs) Handles MyBase.Load
	
	'言語指定による
    Call SetFormLanguage(Me, p_iLanguage, , m_sStr)
	
	'温度コントロール
    'HP パラメータ
    nudP.Value      = p_cts.TempCnt_P
    nudI.Value      = p_cts.TempCnt_I
    nudD.Value      = p_cts.TempCnt_D
    nudDgain.Value  = p_cts.TempCnt_Dgain
    nudPVBias.Value = p_cts.TempCnt_PVBias
    nudPVGain.Value = p_cts.TempCnt_PVGain
    'Probe パラメータ
    nudP2.Value     = p_cts.TempCnt_P2
    nudI2.Value     = p_cts.TempCnt_I2
    nudD2.Value     = p_cts.TempCnt_D2
    nudDgain2.Value = p_cts.TempCnt_Dgain2
    nudPVBias2.Value = p_cts.TempCnt_PVBias2
    nudPVGain2.Value = p_cts.TempCnt_PVGain2
    '起歪体 パラメータ
    nudP3.Value     = p_cts.TempCnt_P3
    nudI3.Value     = p_cts.TempCnt_I3
    nudD3.Value     = p_cts.TempCnt_D3
    nudDgain3.Value = p_cts.TempCnt_Dgain3
    nudPVBias3.Value = p_cts.TempCnt_PVBias3
    nudPVGain3.Value = p_cts.TempCnt_PVGain3

	nudXSpan.Value = 1000
	nudYMax.Value = 1000
	nudYMin.Value = -1000

	'温度設定入力値 初期値
	nudHPTemp.Value = 30.0
	nudPBTemp.Value = 30.0
	nudSGTemp.Value = 30.0

	'温度設定フィールド範囲設定
	nudHPTemp.Maximum	= p_cts.TempCnt_HUpperLimit
	nudHPTemp.Minimum	= 0
	nudPBTemp.Maximum	= p_cts.TempCnt_HUpperLimit2
	nudPBTemp.Minimum	= 0
	nudSGTemp.Maximum	= p_cts.TempCnt_HUpperLimit
	nudSGTemp.Minimum	= 0

	'熱板温度
	p_RTM(0).p_mltcp.TempPreset(1)	= Double.Parse(nudHPTemp.Value)
	'プローブ温度
	p_RTM(0).p_mltcp.Temp2Preset(1)	= Double.Parse(nudPBTemp.Value)
	'起歪体温度
	p_RTM(0).p_mltcp.Temp3Preset(1)	= Double.Parse(nudSGTemp.Value)


    Timer1.Interval = 100
    Timer1.Enabled = True

	'ダブルバッファ有効
	EnableDoubleBuffering(ChartPB)
	EnableDoubleBuffering(ChartPBTemp)

	m_InitFlag		= 0				'ロード初期化終了

End Sub


''' <summary>
''' 温度PID調整 グラフ表示更新
''' </summary>
''' <param name="iMode"></param>
''' <param name="oChart"></param>
''' <param name="ds"></param>
Public Sub TempAdjChartDisp(ByVal iMode As Int16, ByRef oChart As Chart, ByVal ds As Dataset , byval iXSpan As Uint32, byval iYMax As Int32, byval iYMin As Int32)
Dim sSeriesName As String
'Dim sType		As String = IIf(oChart.Name = "ChartHP", "HP", "PB")
'Dim sTbl		As String = IIf(oChart.Name = "ChartHP", "hpPIDTbl", "pbPIDTbl")
Dim sType		As String = oChart.Name.Substring(5, 2) 'ChartHP/ChartPB/ChartSG
Dim sTbl		As String = "XXPIDTbl".Replace("XX", sType.ToLower)
Try
	if ds.Tables.Count=0
		Exit sub
	End If
	'チャートコントロールの設定
	With oChart
	'初期化
	.Series.Clear()
	.Legends.Clear()
	.Titles.Clear()
	.ChartAreas.Clear()
	.DataSource = ds.Tables(sTbl)

    'timeとX,Yを取得
	For i = 1 To 4
		sSeriesName = ds.Tables(sTbl).Columns(i).ColumnName
		.Series.Add(sSeriesName)
		.Series(sSeriesName).XValueMember = ds.Tables(sTbl).Columns(0).ColumnName
		.Series(sSeriesName).YValueMembers = sSeriesName
		.Series(sSeriesName).ChartType = SeriesChartType.FastLine
		Select Case i
		Case 1
			.Series(sSeriesName).Color = Color.Blue
		Case 2
			.Series(sSeriesName).Color = Color.Green
		Case 3
			.Series(sSeriesName).Color = Color.Red
		Case 4
			.Series(sSeriesName).Color = Color.Magenta
		End Select
	Next i

	'TITLE 作成
	Dim title1 As Title = Nothing
	Select Case sType
	Case "PB"
		Title1 = new Title(m_sStr(eMes.TITLE_PB_PID))
	Case "HP"
		Title1 = new Title(m_sStr(eMes.TITLE_HP_PID))
	Case "SG"
		Title1 = new Title(m_sStr(eMes.TITLE_SG_PID))
	End Select
	.Titles.Add(title1)
	.Titles.Item(0).Font = New Font(BASE_FONT, 16, FontStyle.Bold)
    'ChartArea作成
    .ChartAreas.Add("ChartArea1")
	'Legendの作成
	dim leg As Legend = new Legend()
    'leg.DockedToChartArea = "ChartArea1"
    leg.Alignment = StringAlignment.Near
    .Legends.Add(leg)
	.Legends(0).Enabled = False
	Try
	.DataBind()
	Catch ex As Exception
	End Try


		
    '-----------------------------------------------------------------------------------------------------
    '以下、Ｘ軸とＹ軸の設定
    .ChartAreas(0).AxisX.Maximum = ds.Tables(sTbl).Rows.Count
	If ds.Tables(sTbl).Rows.Count-iXSpan <= 0
		.ChartAreas(0).AxisX.Minimum =  0
	Else
		.ChartAreas(0).AxisX.Minimum = ds.Tables(sTbl).Rows.Count - iXSpan
	End If
    .ChartAreas(0).AxisY.Maximum = iYMax
	.ChartAreas(0).AxisY.Minimum = iYMin

    .ChartAreas(0).AxisX.Title                      = m_sStr(eMes.AXIS_X_CNT)
    .ChartAreas(0).AxisX.TitleFont                  = New Font(BASE_FONT, 12, FontStyle.Regular)
    .ChartAreas(0).AxisX.Crossing                   = 0
	.ChartAreas(0).AxisX.IntervalType               = DateTimeIntervalType.NotSet
	.ChartAreas(0).AxisX.MajorGrid.Enabled          = True
	.ChartAreas(0).AxisX.MajorGrid.LineDashStyle    = ChartDashStyle.Dot
    .ChartAreas(0).AxisX.MajorGrid.LineColor        = Color.LightGray
    .ChartAreas(0).AxisX.Interval					= iXSpan / 10

    .ChartAreas(0).AxisY.Title                      = m_sStr(eMes.AXIS_Y_VAL)
    .ChartAreas(0).AxisY.TitleFont                  = New Font(BASE_FONT, 12, FontStyle.Regular)
    .ChartAreas(0).AxisY.Crossing                   = iYMin
	.ChartAreas(0).AxisY.IntervalType               = DateTimeIntervalType.NotSet
	.ChartAreas(0).AxisY.MajorGrid.Enabled          = True
	.ChartAreas(0).AxisY.MajorGrid.LineDashStyle    = ChartDashStyle.Dot
    .ChartAreas(0).AxisY.MajorGrid.LineColor        = Color.LightGray

	.ChartAreas(0).Position.Auto = false
	.ChartAreas(0).Position.X = 1
	.ChartAreas(0).Position.Y = 100
	.ChartAreas(0).Position.height = 85
	.ChartAreas(0).Position.Width = 92

    End With
Catch ex As Exception
End Try
End Sub
''' <summary>
''' 温度PID調整 グラフ表示更新
''' </summary>
''' <param name="iMode"></param>
''' <param name="oChart"></param>
''' <param name="ds"></param>
Public Sub TempAdjChartDisp2(ByVal iMode As Int16, ByRef oChart As Chart, ByVal ds As Dataset, byval iXSpan As Uint32 )
Dim sSeriesName As String
'Dim sType		As String = IIf(oChart.Name = "ChartHPTemp", "HP", "PB")
'Dim sTbl		As String = IIf(oChart.Name = "ChartHPTemp", "hpTempTbl", "pbTempTbl")
Dim sType		As String = oChart.Name.Substring(5, 2) 'ChartHP/ChartPB/ChartSG
Dim sTbl		As String = "XXTempTbl".Replace("XX", sType.ToLower)
Try
	if ds.Tables.Count=0
		Exit sub
	End If
'チャートコントロールの設定
	With oChart
	'初期化
	.Series.Clear()
	.Legends.Clear()
	.Titles.Clear()
	.ChartAreas.Clear()
	.DataSource = ds.Tables(sTbl)

    'timeとX,Yを取得
	For i = 1 To 2
		sSeriesName = ds.Tables(sTbl).Columns(i).ColumnName
		.Series.Add(sSeriesName)
		.Series(sSeriesName).YValueMembers = sSeriesName

		.Series(sSeriesName).ChartType = SeriesChartType.FastLine
		'.Series(sSeriesName).ChartType = SeriesChartType.FastPoint
		.Series(sSeriesName).XValueMember = ds.Tables(sTbl).Columns(0).ColumnName

		Select Case i
		Case 1
			'.Series(sSeriesName).XAxisType = AxisType.Primary
			.Series(sSeriesName).YAxisType = AxisType.Primary
			.Series(sSeriesName).Color = Color.Blue
		Case 2
			'.Series(sSeriesName).XAxisType = AxisType.Secondary
			.Series(sSeriesName).YAxisType = AxisType.Secondary
			.Series(sSeriesName).Color = Color.Green
		End Select
	Next i

	'TITLE 作成
	Dim title1 As Title = Nothing
	Select Case sType
	Case "PB"
		Title1 = new Title(m_sStr(eMes.TITLE_PB_TEMP_MV))
	Case "HP"
		Title1 = new Title(m_sStr(eMes.TITLE_HP_TEMP_MV))
	Case "SG"
		Title1 = new Title(m_sStr(eMes.TITLE_SG_TEMP_MV))
	End Select
	.Titles.Add(title1)
	.Titles.Item(0).Font = New Font(BASE_FONT, 16, FontStyle.Bold)
    'ChartArea作成
    .ChartAreas.Add("ChartArea1")
	'Legendの作成
	dim leg As Legend = new Legend()
    'leg.DockedToChartArea = "ChartArea1"
    leg.Alignment = StringAlignment.Near
    .Legends.Add(leg)
	.Legends(0).Enabled = false


		
    '-----------------------------------------------------------------------------------------------------
    '以下、Ｘ軸とＹ軸の設定
    .ChartAreas(0).AxisX.Maximum = ds.Tables(sTbl).Rows.Count 
	If ds.Tables(sTbl).Rows.Count-iXSpan <= 0
		.ChartAreas(0).AxisX.Minimum =  0
		'.ChartAreas(0).AxisX2.Minimum =  0
	Else
		.ChartAreas(0).AxisX.Minimum =  ds.Tables(sTbl).Rows.Count - iXSpan
		'.ChartAreas(0).AxisX2.Minimum =  ds.Tables(0).Rows.Count - iXSpan
	End If
	Dim YMax As Double = If(sType="SG", 100, 200)
	Dim YMin As Double = 0
    .ChartAreas(0).AxisY.Maximum = YMax
	.ChartAreas(0).AxisY.Minimum = YMin

    
    .ChartAreas(0).AxisX.Title                      = m_sStr(eMes.AXIS_X_CNT)
    .ChartAreas(0).AxisX.TitleFont                  = New Font(BASE_FONT, 12, FontStyle.Regular)
	.ChartAreas(0).AxisX.Enabled					= AxisEnabled.Auto
    .ChartAreas(0).AxisX.Crossing                   = Double.NaN
	.ChartAreas(0).AxisX.IntervalType               = DateTimeIntervalType.NotSet
	.ChartAreas(0).AxisX.MajorGrid.Enabled          = True
	.ChartAreas(0).AxisX.MajorGrid.LineDashStyle    = ChartDashStyle.Dot
    .ChartAreas(0).AxisX.MajorGrid.LineColor        = Color.LightGray
	.ChartAreas(0).AxisX.Interval					= iXSpan / 10

 '   .ChartAreas(0).AxisX2.Enabled					= AxisEnabled.True
	'.ChartAreas(0).AxisX2.Crossing					= Double.MaxValue
	'.ChartAreas(0).AxisX.IntervalType				= DateTimeIntervalType.NotSet
	'.ChartAreas(0).AxisX2.MajorGrid.Enabled			= True
	'.ChartAreas(0).AxisX2.Interval					= iXSpan / 10


	.ChartAreas(0).AxisY.Title                      = m_sStr(eMes.AXIS_Y_TEMP)
    .ChartAreas(0).AxisY.TitleFont                  = New Font(BASE_FONT, 12, FontStyle.Regular)
    .ChartAreas(0).AxisY.Crossing                   = Double.NaN
	.ChartAreas(0).AxisY.IntervalType               = DateTimeIntervalType.NotSet
	.ChartAreas(0).AxisY.MajorGrid.Enabled          = True
	.ChartAreas(0).AxisY.MajorGrid.LineDashStyle    = ChartDashStyle.Dot
    .ChartAreas(0).AxisY.MajorGrid.LineColor        = Color.LightGray

	'------------------------------------------------------------------------------------------------------------
	.ChartAreas(0).Axisy2.Enabled					= AxisEnabled.Auto
	.ChartAreas(0).AxisY2.Maximum					= 100
	.ChartAreas(0).AxisY2.Minimum					= 0
	.ChartAreas(0).AxisY2.Title						= m_sStr(eMes.AXIS_Y_MV)
	.ChartAreas(0).AxisY2.TitleFont					= New Font(BASE_FONT, 12, FontStyle.Regular)
	.ChartAreas(0).AxisY2.IntervalType				= DateTimeIntervalType.NotSet
	.ChartAreas(0).AxisY2.Crossing					= Double.NaN
	.ChartAreas(0).AxisY2.MajorGrid.Enabled			= True
	.ChartAreas(0).AxisY2.MajorGrid.LineDashStyle	= ChartDashStyle.Dot
	.ChartAreas(0).AxisY2.MajorGrid.LineColor		= Color.LightGray
	.ChartAreas(0).AxisY2.TitleAlignment			= StringAlignment.Center


	'-------------------------------------------------------------------------------------------------------------
	.ChartAreas(0).Position.Auto					= false
	.ChartAreas(0).Position.X						= 2
	.ChartAreas(0).Position.Y						= 20
	.ChartAreas(0).Position.height					= 78
	.ChartAreas(0).Position.Width					= 97
	Try
	.DataBind()
	Catch ex As Exception
	End Try

    End With
Catch ex As Exception
End Try
End Sub

Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
Dim iRet As Int32
Trｙ
	Timer1.Enabled=False
	iRet = TempAdjMonProc(SW8)			'温度PID制御量モニタ

	If iRet = 1
		Select Case tbcTemp.SelectedIndex
		Case 0
			TempAdjChartDisp(0, ChartPB, dsTemp, nudXSpan.Value, nudYMax.Value, nudYMin.Value )
			TempAdjChartDisp2(0, ChartPBTemp, dsTemp, nudXSpan.Value )
		Case 1
			TempAdjChartDisp(0, ChartHP, dsTemp, nudXSpan.Value, nudYMax.Value, nudYMin.Value )
			TempAdjChartDisp2(0, ChartHPTemp, dsTemp, nudXSpan.Value )
		Case 2
			TempAdjChartDisp(0, ChartSG, dsTemp, nudXSpan.Value, nudYMax.Value, nudYMin.Value )
			TempAdjChartDisp2(0, ChartSGTemp, dsTemp, nudXSpan.Value )
		End Select
	End if

	'ヒータ制御ボタン表示
	If p_RTM(0).p_mltcp.isStatHeaterOn2
		btnHeaterContPB.BackColor = Color.Orange
	Else
		btnHeaterContPB.BackColor = Control.DefaultBackColor
	End if
	If p_RTM(0).p_mltcp.isStatHeaterOn
		btnHeaterContHP.BackColor = Color.Orange
	Else
		btnHeaterContHP.BackColor = Control.DefaultBackColor
	End if
	If p_RTM(0).p_mltcp.isStatHeaterOn3
		btnHeaterContSG.BackColor = Color.Orange
	Else
		btnHeaterContSG.BackColor = Control.DefaultBackColor
	End if
	'モニタ有効ボタン表示
	If TempMonEn
		btnMonEnable.BackColor = Color.Orange
	Else
		btnMonEnable.BackColor = Control.DefaultBackColor
	End if

	'dmv等の表示
	lbldmv.Text		= p_RTM(0).p_mltcp.Hp_dmv
	lblpn.Text		= p_RTM(0).p_mltcp.Hp_pn
	lblin.Text		= p_RTM(0).p_mltcp.Hp_in
	lbldn.Text		= p_RTM(0).p_mltcp.Hp_dn
	lbldmv2.Text	= p_RTM(0).p_mltcp.Pb_dmv
	lblpn2.Text		= p_RTM(0).p_mltcp.Pb_pn
	lblin2.Text		= p_RTM(0).p_mltcp.Pb_in
	lbldn2.Text		= p_RTM(0).p_mltcp.Pb_dn
	lbldmv3.Text	= p_RTM(0).p_mltcp.Sg_dmv
	lblpn3.Text		= p_RTM(0).p_mltcp.Sg_pn
	lblin3.Text		= p_RTM(0).p_mltcp.Sg_in
	lbldn3.Text		= p_RTM(0).p_mltcp.Sg_dn

	'温度、MV数値表示
	lblTemp.Text	= p_RTM(0).p_mltcp.HeatTempAct.ToString("F1")
	lblMv.Text		= p_RTM(0).p_mltcp.HeatTempOpe.ToString("F1")
	lblTemp2.Text	= p_RTM(0).p_mltcp.HeatTemp2Act.ToString("F1")
	lblMv2.Text		= p_RTM(0).p_mltcp.HeatTemp2Ope.ToString("F1")
	lblTemp3.Text	= p_RTM(0).p_mltcp.HeatTemp3Act.ToString("F1")
	lblMv3.Text		= p_RTM(0).p_mltcp.HeatTemp3Ope.ToString("F1")

	'PB 温度移動平均時間
	'p_RTM(0).p_mltcp.PbTempAveTime(1) = m_dPBAveTime

Catch ex As Exception

End Try	
	Timer1.Enabled=True
End Sub
'温度PIDモニタ用変数
Dim		TempMonEn		As Boolean = False		'ロギング有効
Dim		MonReset		As int16 = 0			'リセットタイミング
Dim		iJob			As int32 = -1			'シーケンス変数
Dim		dsTemp			As New Dataset
Dim		dtHP			As New DataTable		'HP PID データテーブル
Dim		dtHPT			As New DataTable		'HP 温度 データテーブル

Dim		dtPB			As New DataTable		'PB PID データテーブル
Dim		dtPBT			As New DataTable		'PB 温度 データテーブル

Dim		dtSG			As New DataTable		'SG PID データテーブル
Dim		dtSGT			As New DataTable		'SG 温度 データテーブル

Dim		GetCount		As UInt32 = 0			'データ行カウント
Dim		SW8				As New Stopwatch

'温度PID制御量モニタ
Public Function	TempAdjMonProc(ByRef sw8 As Stopwatch) As int32
Dim dtRowHp		As DataRow
Dim dtRowHpT	As DataRow
Dim dtRowPb		As DataRow
Dim dtRowPbT	As DataRow
Dim dtRowSg		As DataRow
Dim dtRowSgT	As DataRow
Dim iRet		As int32 = 0

Try
	'ゼロ調中のＺ位置モニタ
	Select Case iJob
	Case -1
	'データテーブル初期化
		If TempMonEn 'And (p_mltcp.isStatHeaterOn2 Or p_mltcp.isStatHeaterOn)
			'列の作成
			dtHP.Columns.Add("200mS Count", Type.GetType("System.UInt32"))
			dtHP.Columns.Add("dmv", Type.GetType("System.Int32"))
			dtHP.Columns.Add("pn", Type.GetType("System.Int32"))
			dtHP.Columns.Add("in", Type.GetType("System.Int32"))
			dtHP.Columns.Add("dn", Type.GetType("System.Int32"))
			dtHP.TableName = "hpPIDTbl"
			dsTemp.Tables.Add(dtHP)

			dtHPT.Columns.Add("200mS Count", Type.GetType("System.UInt32"))
			dtHPT.Columns.Add("Temp", Type.GetType("System.Double"))
			dtHPT.Columns.Add("Mv", Type.GetType("System.Double"))
			dtHPT.TableName = "hpTempTbl"
			dsTemp.Tables.Add(dtHPT)

			dtPB.Columns.Add("200mS Count", Type.GetType("System.UInt32"))
			dtPB.Columns.Add("dmv", Type.GetType("System.Int32"))
			dtPB.Columns.Add("pn", Type.GetType("System.Int32"))
			dtPB.Columns.Add("in", Type.GetType("System.Int32"))
			dtPB.Columns.Add("dn", Type.GetType("System.Int32"))
			dtPB.TableName = "pbPIDTbl"
			dsTemp.Tables.Add(dtPB)

			dtPBT.Columns.Add("200mS Count", Type.GetType("System.UInt32"))
			dtPBT.Columns.Add("Temp", Type.GetType("System.Double"))
			dtPBT.Columns.Add("Mv", Type.GetType("System.Double"))
			dtPBT.TableName = "pbTempTbl"
			dsTemp.Tables.Add(dtPBT)

			dtSG.Columns.Add("200mS Count", Type.GetType("System.UInt32"))
			dtSG.Columns.Add("dmv", Type.GetType("System.Int32"))
			dtSG.Columns.Add("pn", Type.GetType("System.Int32"))
			dtSG.Columns.Add("in", Type.GetType("System.Int32"))
			dtSG.Columns.Add("dn", Type.GetType("System.Int32"))
			dtSG.TableName = "sgPIDTbl"
			dsTemp.Tables.Add(dtSG)

			dtSGT.Columns.Add("200mS Count", Type.GetType("System.UInt32"))
			dtSGT.Columns.Add("Temp", Type.GetType("System.Double"))
			dtSGT.Columns.Add("Mv", Type.GetType("System.Double"))
			dtSGT.TableName = "sgTempTbl"
			dsTemp.Tables.Add(dtSGT)

			iJob = 0
		End if

	Case 0
		If TempMonEn 'And (p_mltcp.isStatHeaterOn2 Or p_mltcp.isStatHeaterOn)
			GetCount = 0
			iJob = 1
			sw8.reStart()
		End if

	Case 1
	'モニタ中の処理
		If TempMonEn 'And (p_mltcp.isStatHeaterOn2 Or p_mltcp.isStatHeaterOn)
			'データの追加
			If sw8.Elapsed.TotalMilliseconds >= 200
				GetCount += 1
				dtRowHp = dtHP.NewRow
				dtRowHp(0) = GetCount
				dtRowHp(1) = p_RTM(0).p_mltcp.Hp_dmv
				dtRowHp(2) = p_RTM(0).p_mltcp.Hp_pn
				dtRowHp(3) = p_RTM(0).p_mltcp.Hp_in
				dtRowHp(4) = p_RTM(0).p_mltcp.Hp_dn
				dtHP.Rows.Add(dtRowHp)

				dtRowHPT = dtHPT.NewRow
				dtRowHPT(0) = GetCount
				dtRowHPT(1) = p_RTM(0).p_mltcp.HeatTempAct()
				dtRowHPT(2) = p_RTM(0).p_mltcp.HeatTempOpe()
				dtHPT.Rows.Add(dtRowHpT)

				dtRowPb = dtPB.NewRow
				dtRowPb(0) = GetCount
				dtRowPb(1) = p_RTM(0).p_mltcp.Pb_dmv
				dtRowPb(2) = p_RTM(0).p_mltcp.Pb_pn
				dtRowPb(3) = p_RTM(0).p_mltcp.Pb_in
				dtRowPb(4) = p_RTM(0).p_mltcp.Pb_dn
				dtPB.Rows.Add(dtRowPb)

				dtRowPbT = dtPBT.NewRow
				dtRowPbT(0) = GetCount
				dtRowPbT(1) = p_RTM(0).p_mltcp.HeatTemp2Act()
				dtRowPbT(2) = p_RTM(0).p_mltcp.HeatTemp2Ope()
				dtPBT.Rows.Add(dtRowPbT)

				dtRowSg = dtSG.NewRow
				dtRowSg(0) = GetCount
				dtRowSg(1) = p_RTM(0).p_mltcp.Sg_dmv
				dtRowSg(2) = p_RTM(0).p_mltcp.Sg_pn
				dtRowSg(3) = p_RTM(0).p_mltcp.Sg_in
				dtRowSg(4) = p_RTM(0).p_mltcp.Sg_dn
				dtSG.Rows.Add(dtRowSg)

				dtRowSGT = dtSGT.NewRow
				dtRowSGT(0) = GetCount
				dtRowSGT(1) = p_RTM(0).p_mltcp.HeatTemp3Act()
				dtRowSGT(2) = p_RTM(0).p_mltcp.HeatTemp3Ope()
				dtSGT.Rows.Add(dtRowSGT)

				iRet = 1		'データ更新あり
				sw8.Restart()
			End if

			if MonReset <> 0
				iJob = 8
			End if
		Else
			iJob = 9
		End if
	Case 8
	'グラフリセット処理
		dtHP.Clear
		dtPB.Clear
		dtSG.Clear
		dtPBT.Clear
		dtHPT.Clear
		dtSGT.Clear
		dsTemp.Clear
		iJob = 0
		MonReset = 0

	Case 9
	'終了処理
		iJob = 0
		MonReset = 0
		TempMonEn = False
		dtHP.Clear
		dtPB.Clear
		dtSG.Clear
		dtPBT.Clear
		dtHPT.Clear
		dtSGT.Clear
		dsTemp.Clear
	End Select
Catch ex As Exception
	Return -1
End Try
	Return iRet
End Function

Private Sub btnGraphReset_Click(sender As Object, e As EventArgs) Handles btnGraphReset.Click
	MonReset = 1
End Sub
'monitor Enable
Private Sub btnMonEnable_Click(sender As Object, e As EventArgs) Handles btnMonEnable.Click
	TempMonEn = Not TempMonEn
End Sub
'Heater On/Off
Private Sub btnHeaterCont_Click(sender As Object, e As EventArgs) Handles btnHeaterContPB.Click, btnHeaterContHP.Click, btnHeaterContSG.Click
	Select Case sender.Name
	Case "btnHeaterContPB"
		If p_RTM(0).p_iBGWOrder <> BGW_ORDER.NO_ORDER
			p_RTM(0).p_iBGWOrderNext	= p_RTM(0).p_iBGWOrder
		End if
		p_RTM(0).p_iBGWOrder		= BGW_ORDER.TEMP_PB_BUTTON_REP
	Case "btnHeaterContHP"
		If p_RTM(0).p_iBGWOrder <> BGW_ORDER.NO_ORDER
			p_RTM(0).p_iBGWOrderNext	= p_RTM(0).p_iBGWOrder
		End if
		p_RTM(0).p_iBGWOrder		= BGW_ORDER.TEMP_HP_BUTTON_REP
	Case "btnHeaterContSG"
		If p_RTM(0).p_iBGWOrder <> BGW_ORDER.NO_ORDER
			p_RTM(0).p_iBGWOrderNext	= p_RTM(0).p_iBGWOrder
		End if
		p_RTM(0).p_iBGWOrder		= BGW_ORDER.TEMP_SG_BUTTON_REP
	End Select
End Sub

'Private Sub nudPBAveTime_ValueChanged(sender As Object, e As EventArgs) 
'	m_dPBAveTime = nudPBAveTime.Value
'End Sub

Private Sub nudPBTemp_ValueChanged(sender As Object, e As EventArgs) Handles nudPBTemp.ValueChanged
	p_RTM(0).p_mltcp.Temp2Preset(1)	= Double.Parse(nudPBTemp.Value)
End Sub

Private Sub nudHPTemp_ValueChanged(sender As Object, e As EventArgs) Handles nudHPTemp.ValueChanged
	p_RTM(0).p_mltcp.TempPreset(1)	= Double.Parse(nudHPTemp.Value)
End Sub

Private Sub nudSGTemp_ValueChanged(sender As Object, e As EventArgs) Handles nudSGTemp.ValueChanged
	p_RTM(0).p_mltcp.Temp3Preset(1)	= Double.Parse(nudSGTemp.Value)
End Sub

''' <summary>
''' フォームクローズ
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub frmTempAdjMon_Closed(sender As Object, e As EventArgs) Handles Me.Closed
	TempMonEn = False			'モニタ無効
	'全ヒータ指令OFF
	if p_RTM(0).p_mltcp.isStatHeaterOn() Or p_RTM(0).p_mltcp.isStatHeaterOn2() Or p_RTM(0).p_mltcp.StartTempCont<>0
		If p_RTM(0).p_iBGWOrder <> BGW_ORDER.NO_ORDER
			p_RTM(0).p_iBGWOrderNext	= p_RTM(0).p_iBGWOrder
		End if
		p_RTM(0).p_iBGWOrder		= BGW_ORDER.TEMP_BUTTON_REP
	End if
	
	'パラメータ記憶領域へ反映
	p_cts.TempCnt_P2 		=	nudP2.Value
	p_cts.TempCnt_P3 		=	nudP3.Value
    p_cts.TempCnt_P			=	nudP.Value
    p_cts.TempCnt_I2		=	nudI2.Value
    p_cts.TempCnt_I3		=	nudI3.Value
    p_cts.TempCnt_I			=	nudI.Value
    p_cts.TempCnt_D2		=	nudD2.Value
    p_cts.TempCnt_D3		=	nudD3.Value
    p_cts.TempCnt_D			=	nudD.Value
    p_cts.TempCnt_Dgain2	=	nudDgain2.Value
    p_cts.TempCnt_Dgain3	=	nudDgain3.Value
    p_cts.TempCnt_Dgain		=	nudDgain.Value
    p_cts.TempCnt_PVBias2	=	nudPVBias2.Value
    p_cts.TempCnt_PVBias3	=	nudPVBias3.Value
    p_cts.TempCnt_PVBias	=	nudPVBias.Value
    p_cts.TempCnt_PVGain2	=	nudPVGain2.Value
    p_cts.TempCnt_PVGain3	=	nudPVGain3.Value
    p_cts.TempCnt_PVGain	=	nudPVGain.Value
End Sub
Private Sub nudP2_ValueChanged(sender As Object, e As EventArgs) Handles nudP2.ValueChanged
	if　m_InitFlag = 0
		p_cts.TempCnt_P2 = nudP2.Value
	End If
End Sub

Private Sub nudI2_ValueChanged(sender As Object, e As EventArgs) Handles nudI2.ValueChanged
	if　m_InitFlag = 0
		p_cts.TempCnt_I2		=	nudI2.Value
	End If
End Sub

Private Sub nudD2_ValueChanged(sender As Object, e As EventArgs) Handles nudD2.ValueChanged
	if　m_InitFlag = 0
		p_cts.TempCnt_D2		=	nudD2.Value
	End If
End Sub

Private Sub nudDgain2_ValueChanged(sender As Object, e As EventArgs) Handles nudDgain2.ValueChanged
	if　m_InitFlag = 0
		p_cts.TempCnt_Dgain2	=	nudDgain2.Value
	End If
End Sub

Private Sub nudPVBias2_ValueChanged(sender As Object, e As EventArgs) Handles nudPVBias2.ValueChanged
	if　m_InitFlag = 0
		p_cts.TempCnt_PVBias2		=	nudPVBias2.Value
	End If
End Sub

Private Sub nudP3_ValueChanged(sender As Object, e As EventArgs) Handles nudP3.ValueChanged
	if　m_InitFlag = 0
		p_cts.TempCnt_P3 = nudP3.Value
	End If
End Sub

Private Sub nudI3_ValueChanged(sender As Object, e As EventArgs) Handles nudI3.ValueChanged
	if　m_InitFlag = 0
		p_cts.TempCnt_I3		=	nudI3.Value
	End If
End Sub

Private Sub nudD3_ValueChanged(sender As Object, e As EventArgs) Handles nudD3.ValueChanged
	if　m_InitFlag = 0
		p_cts.TempCnt_D3		=	nudD3.Value
	End If
End Sub

Private Sub nudDgain3_ValueChanged(sender As Object, e As EventArgs) Handles nudDgain3.ValueChanged
	if　m_InitFlag = 0
		p_cts.TempCnt_Dgain3	=	nudDgain3.Value
	End If
End Sub

Private Sub nudPVBias3_ValueChanged(sender As Object, e As EventArgs) Handles nudPVBias3.ValueChanged
	if　m_InitFlag = 0
		p_cts.TempCnt_PVBias3		=	nudPVBias3.Value
	End If
End Sub
Private Sub nudPVGain3_ValueChanged(sender As Object, e As EventArgs) Handles nudPVGain3.ValueChanged
	if　m_InitFlag = 0
		p_cts.TempCnt_PVGain3		=	nudPVGain3.Value
	End If
End Sub


Private Sub nudP_ValueChanged(sender As Object, e As EventArgs) Handles nudP.ValueChanged
	if　m_InitFlag = 0
		p_cts.TempCnt_P		=	nudP.Value
	End If
End Sub

Private Sub nudI_ValueChanged(sender As Object, e As EventArgs) Handles nudI.ValueChanged
	if　m_InitFlag = 0
		 p_cts.TempCnt_I		=	nudI.Value
	End If
End Sub

Private Sub nudD_ValueChanged(sender As Object, e As EventArgs) Handles nudD.ValueChanged
	if　m_InitFlag = 0
		p_cts.TempCnt_D		=	nudD.Value
	End If
End Sub

Private Sub nudDgain_ValueChanged(sender As Object, e As EventArgs) Handles nudDgain.ValueChanged
	if　m_InitFlag = 0
		p_cts.TempCnt_Dgain		=	nudDgain.Value
	End If
End Sub

Private Sub nudPVBias_ValueChanged(sender As Object, e As EventArgs) Handles nudPVBias.ValueChanged
	if　m_InitFlag = 0
		p_cts.TempCnt_PVBias		=	nudPVBias.Value
	End If
End sub
Private Sub nudPVGain2_ValueChanged(sender As Object, e As EventArgs) Handles nudPVGain2.ValueChanged
	if　m_InitFlag = 0
		p_cts.TempCnt_PVGain2 = nudPVGain2.Value
	End If
End Sub

Private Sub nudPVGain_ValueChanged(sender As Object, e As EventArgs) Handles nudPVGain.ValueChanged
	if　m_InitFlag = 0
		p_cts.TempCnt_PVGain = nudPVGain.Value
	End If
End Sub

End Class