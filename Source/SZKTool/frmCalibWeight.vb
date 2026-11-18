Imports System.Drawing.Printing
Imports System.Threading
Imports System.IO
Imports System.Text
Imports System.Windows.Forms.DataVisualization.Charting
Imports Microsoft.VisualBasic.FileIO
Imports System.Collections
Imports System.Globalization
Imports MDKTool2.frmOptions
Imports MDKTool2.RealTimeMeas
Imports MDKTool2.modPublic
Imports System.ComponentModel
Imports System.Runtime.CompilerServices
Imports MathNet.Numerics.Statistics
Imports System.Reflection
Imports MDKTool2.DataFile


Public Class frmCalibWeight


Private m_Rt						As New RealTimeMeas			'frmMainで生成中のRealTimeMeasクラス参照
Private m_fM						As New frmMain				'frmMainクラス参照
Private m_fMcts						As New CToolSettings		'frmMainで生成中のCtoolSettingクラス参照

Private p_sCalbWMode                 As String                   'X or Y
Private p_MeasState                  As Int16 = -1               '-1:未開始 1:X実施中 2:Y実施中 
Private p_Extract					As Int16 = -1				'-1:未開始 1:実施中

Private	ds					        As New DataSet			    'データセット
Private	dtStrees			        As New DataTable		    '応力モニタ用データテーブル Table(0)
Private	dtXWeight			        As New DataTable		    '重量校正用データテーブル	Table(1)
Private	dtYWeight			        As New DataTable		    '重量校正用データテーブル	Table(2)
Private dtRow	                    As DataRow                  'ワーク用データテーブル行

Private dMeasWT						As Double = Double.NaN		'測定データ 重量
Private dMeasAD						As Double = Double.NaN		'測定データ 応力

Private dCoefX						As Double = Double.NaN
Private dCoefY						As Double = Double.NaN

Private m_bYSpanAuto				As Boolean = True			'Y軸スパン自動設定
Private m_StressArray()				As int64 = Nothing			'平均算出用配列
Private	m_Result					As Int16 = -1				'1:結果更新完了 -1:結果更新待ち

Private m_SetPath					As String
Private m_ExportFileNameBaseX		As String
Private m_ExportFileNameBaseY		As String
Public Writeonly ProPerty WeightSetFileNameSet() As String
    Set(value As String)
		m_ExportFileNameBaseX	= "WeightCalibrationX-" & value.Substring(value.Length-3,3) & ".csv"
		m_ExportFileNameBaseY	= "WeightCalibrationY-" & value.Substring(value.Length-3,3) & ".csv"
    End Set
End Property
Sub New
    ' この呼び出しはデザイナーで必要です。
    InitializeComponent()

	' InitializeComponent() 呼び出しの後で初期化を追加します。
	m_fM		= frmMain							'frmMainの参照
	m_Rt		= frmMain.p_RTM(0)					'frmMain.RealTimeMeasクラス参照
	m_fMcts		= frmMain.p_cts						'frmMainで生成中のCtoolSettingクラス参照

	Dim sEquipName As String = m_fMcts.strEquipName(m_fmcts.iSelMDK)
	m_SetPath = Path.Combine(MADOKA_PATH, CALDAT_PATH)
	m_ExportFileNameBaseX = "WeightCalibrationX-" & sEquipName.Substring(sEquipName.Length - 3, 3) & ".csv"
	m_ExportFileNameBaseY = "WeightCalibrationY-" & sEquipName.Substring(sEquipName.Length - 3, 3) & ".csv"

	End Sub
'フォームロード
Private Sub frmCalibWeight_Load(sender As Object, e As EventArgs) Handles MyBase.Load
	'言語切替
	Call SetFormLanguage(Me, p_iLanguage)

    m_fM		= frmMain							'frmMainの参照
	m_Rt		= frmMain.p_RTM(0)					'frmMain.RealTimeMeasクラス参照
	m_fMcts		= frmMain.p_cts						'frmMainで生成中のCtoolSettingクラス参照

	Dim sEquipName As String = m_fMcts.strEquipName(m_fmcts.iSelMDK)
	m_SetPath				= Path.Combine( MADOKA_PATH, CALDAT_PATH )
	WeightSetFileNameSet = sEquipName
	If ds.Tables.Count <= 0
		'データセットのモニタデータ用テーブル
		dtStrees.TableName = "StressMonitor"
		dtStrees.Columns.Add("Sec", Type.GetType("System.Double"))
		dtStrees.Columns.Add("ADValue", Type.GetType("System.Int64"))
		ds.Tables.Add(dtStrees)

		'データセットの重量校正用Xテーブル
		dtXWeight.TableName = "CalibX"
		dtXWeight.Columns.Add("Weight", Type.GetType("System.Double"))
		dtXWeight.Columns.Add("Stress", Type.GetType("System.Double"))
		ds.Tables.Add(dtXWeight)

		'データセットの重量校正用Yテーブル
		dtYWeight.TableName = "CalibY"
		dtYWeight.Columns.Add("Weight", Type.GetType("System.Double"))
		dtYWeight.Columns.Add("Stress", Type.GetType("System.Double"))
		ds.Tables.Add(dtYWeight)
	End if

	'Xパラメータ表示 -------------------------------------
	CsvToDataTable(m_SetPath, m_ExportFileNameBaseX, dtXWeight)		'ファイルから読込
	WeightCalibDgvDisp( 1, ds )										'重量校正結果リスト表示
	WeightCalibChartDisp(1, ResultChart, ds)						'重量校正結果チャート表示
	CalcParameterX(dtXWeight)										'パラメータ計算

	'Yパラメータ表示 -------------------------------------
	CsvToDataTable(m_SetPath, m_ExportFileNameBaseY, dtYWeight)		'ファイルから読込
	WeightCalibDgvDisp( 2, ds )										'重量校正結果リスト表示
	WeightCalibChartDisp(2, ResultChart, ds)						'重量校正結果チャート表示
	CalcParameterY(dtYWeight)										'パラメータ計算

    p_MeasState = -1
	rbX.Checked = True

    Timer1.Interval = 200
    Timer1.Enabled = True
End Sub
'XStartボタン
Private Sub btnXStart_Click(sender As Object, e As EventArgs) Handles btnXStart.Click
    Select case p_MeasState	
    Case -1
    '測定開始
        p_sCalbWMode = "X"
        p_MeasState = 1
        btnXStart.BackColor = Color.Orange
        btnYStart.Enabled = False
		rbX.Checked = True

    Case 1
    '測定停止
        p_sCalbWMode = ""
        p_MeasState = -1
        btnXStart.BackColor = Control.DefaultBackColor
        btnYStart.Enabled = True
    Case 2
    '測定不可
    End Select
End Sub
'YStartボタン
Private Sub btnYStart_Click(sender As Object, e As EventArgs) Handles btnYStart.Click
    Select case p_MeasState	
    Case -1
    '測定開始
        p_sCalbWMode = "Y"
        p_MeasState = 2
        btnYStart.BackColor = Color.Orange
        btnXStart.Enabled = False
		rbY.Checked = True

    Case 2
    '測定停止
        p_sCalbWMode = ""
        p_MeasState = -1
        btnYStart.BackColor = Control.DefaultBackColor
        btnXStart.Enabled = True
    Case 1
    '測定不可
    End Select
End Sub
'抽出ボタン
Private Sub btnExtract_Click(sender As Object, e As EventArgs) Handles btnExtract.Click
	If p_Extract = -1
		p_Extract = 1
        btnExtract.BackColor = Color.Orange
	Else
		p_Extract = -1
        btnExtract.BackColor = Control.DefaultBackColor
	End if
End Sub
'エクスポートボタン
Private Sub btnExport_Click(sender As Object, e As EventArgs) 
	SaveToCSV(ds.Tables(1), Path.Combine(m_SetPath, m_ExportFileNameBaseX), True, ",", "", "'", Encoding.GetEncoding("shift_jis") )
	SaveToCSV(ds.Tables(2), Path.Combine(m_SetPath, m_ExportFileNameBaseY), True, ",", "", "'", Encoding.GetEncoding("shift_jis") )
End Sub
'インポートボタン
Private Sub btnInport_Click(sender As Object, e As EventArgs)
		'[ファイルの種類]ではじめに選択されるものを指定する
		'2番目の「すべてのファイル」が選択されているようにする
		'タイトルを設定する
		'ダイアログボックスを閉じる前に現在のディレクトリを復元するようにする
		'既に存在するファイル名を指定したとき警告する
		'デフォルトでTrueなので指定する必要はない
		'存在しないパスが指定されたとき警告を表示する
		'デフォルトでTrueなので指定する必要はない
		Dim sfd As New SaveFileDialog With {
			.FileName = "WeightCalib.CSV",
			.InitialDirectory = MADOKA_PATH,
			.Filter = "CSV File(*.CSV)|*.CSV|All File(*.*)|*.*",
			.FilterIndex = 1,
			.Title = "Please select the export destination file",
			.RestoreDirectory = True,
			.OverwritePrompt = True,
			.CheckPathExists = True
		}

			'ダイアログを表示する
		If sfd.ShowDialog() = DialogResult.OK Then
		Console.WriteLine(sfd.FileName)
		SaveToCSV(ds.Tables(1), sfd.FileName, False, ",", "", "'", Encoding.GetEncoding("shift_jis") )
	End If
End Sub
'YスパンMax変更
Private Sub nudYSpanMax_ValueChanged(sender As Object, e As EventArgs) Handles nudYSpanMax.ValueChanged
	If nudYSpanMax.Value - nudYSpanMin.Value <= 0
		nudYSpanMax.Value = nudYSpanMin.Value + nudYSpanMax.Increment
	End If
End Sub
'YスパンMin変更
Private Sub nudYSpanMin_ValueChanged(sender As Object, e As EventArgs) Handles nudYSpanMin.ValueChanged
	If nudYSpanMax.Value - nudYSpanMin.Value <= 0
		nudYSpanMin.Value = nudYSpanMax.Value - nudYSpanMin.Increment
	End If
End Sub
'YスパンAUTO
Private Sub btnYSpanAuto_Click(sender As Object, e As EventArgs) Handles btnYSpanAuto.Click
	m_bYSpanAuto = Not m_bYSpanAuto
End Sub
'重量値変更
Private Sub nudWeight_ValueChanged(sender As Object, e As EventArgs) Handles nudWeight.ValueChanged, nudWeightAdd.ValueChanged
	dMeasWT 	= nudWeight.Value			'測定データ 重量
	dMeasAD		= Double.NaN				'測定データ 応力
End Sub


Private m_Job           As Int16 = -1
Private m_GetCount      As Uint32
Private m_SW1			As New Stopwatch
Private m_SW2			As New Stopwatch
'タイマーインターバル
Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
Try
    DataTabeleSet()						'モニタ用データ取得シーケンス処理
    WeightMonitorChartDisp( _			'応力モニタ表示更新
	p_MeasState, _
	ChartAD, _
	ds, _
	nudTimeSpan.Value, _
	nudYSpanMin.Value, _
	nudYSpanMax.Value, _
	m_bYSpanAuto _
	)  
	If m_Result = 1
	'抽出結果処理完了
		WeightCalibDgvDisp( p_MeasState, ds )	'重量校正結果リスト表示

		WeightCalibChartDisp(					'重量校正結果チャート表示
		p_MeasState, _
		ResultChart, _
		ds
		)
		'パラメータ計算
		Select Case p_MeasState
		Case 1
			CalcParameterX(ds.Tables(1))							'Xパラメータ計算
			SaveToCSV(ds.Tables(1), Path.Combine(m_SetPath, m_ExportFileNameBaseX), True, ",", "", "'", Encoding.GetEncoding("shift_jis") )
		Case 2
			CalcParameterY(ds.Tables(2))							'Yパラメータ計算
			SaveToCSV(ds.Tables(2), Path.Combine(m_SetPath, m_ExportFileNameBaseY), True, ",", "", "'", Encoding.GetEncoding("shift_jis") )
		End Select
		m_Result = -1
	End if
	'抽出ボタン表示
	If p_Extract = 1
        btnExtract.BackColor = Color.Orange
	Else
        btnExtract.BackColor = Control.DefaultBackColor
	End if
	'AUTOボタン表示
	If m_bYSpanAuto
		btnYSpanAuto.BackColor = Color.Orange
	Else
		btnYSpanAuto.BackColor = Control.DefaultBackColor
	End If
	'抽出ボタン有効無効
	if p_MeasState = -1 Or p_Extract = 1
		btnExtract.Enabled = False
	Else
		btnExtract.Enabled = True
	End If
	'重量入力有効無効
	if p_Extract = 1
		nudWeight.Enabled = False
	Else
		nudWeight.Enabled = True
	End If
    '現在応力値
    If p_MeasState = -1
        lblCurrentADValX.Text = "N A"
        lblCurrentADValY.Text = "N A"
    Else
	    lblCurrentADValX.Text =  m_Rt.p_mltcp.CurrStrsX()
	    lblCurrentADValY.Text =  m_Rt.p_mltcp.CurrStrsY()
    End If
	'測定応力値
	If Double.IsNaN(dMeasAD)
		lblMeasStress.Text = "N A"
	Else
		lblMeasStress.Text = dMeasAD.ToString("F1")
	End If
	'計算パラメータ
	If Double.IsNaN(dCoefX)
		lblCoefX.Text = "N A"
	Else
		lblCoefX.Text = dCoefX.ToString("E6")
	End If
	If Double.IsNaN(dCoefY)
		lblCoefY.Text = "N A"
	Else
		lblCoefY.Text = dCoefY.ToString("E6")
	End If
Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub

'モニタ用データ取得シーケンス処理
Sub DataTabeleSet()
Try
    Select Case m_Job
    Case -1
        m_Job = 0 

    Case 0
	'モニタ開始待ち
		If p_sCalbWMode <> "" 
            m_Job = 1
			m_SW1.Restart()
		End If

    Case 1
    'モニタ中処理
		If p_sCalbWMode <> "" 
			'データの追加
			m_GetCount += 1
			dtRow = ds.Tables("StressMonitor").NewRow
			dtRow(0) = m_SW1.Elapsed.TotalMilliseconds / 1000.0
			'dtRow(0) = m_GetCount
			dtRow(1) = iif(p_MeasState = 1, m_Rt.p_mltcp.CurrStrsX(), m_Rt.p_mltcp.CurrStrsY())
			ds.Tables("StressMonitor").Rows.Add(dtRow)

			If p_Extract = 1				'抽出中ステータス確認
			'重量-応力測定処理 ------------------------
				If IsNothing(m_StressArray)
				'初回 
					Array.Resize(m_StressArray, 1)			'平均算出用 配列確保
					dMeasAD = Double.NaN					'結果初期化
					m_SW2.Stop()							
					m_SW2.Restart()							'タイマー開始
				Else
				'2回目以降
					Array.Resize(m_StressArray, m_StressArray.Length + 1)
				End If
				m_StressArray(m_StressArray.Length-1) = dtRow(1)		'現在応力値を配列へ追加
				If m_SW2.Elapsed.TotalSeconds >= 10
				'10秒経過
					dMeasWT = convert.ToDouble( nudWeight.Value )		'重量値のセット
					dMeasAD = m_StressArray.Average()					'応力平均値のセット
					m_StressArray = Nothing								'平均算出用 配列破棄
					m_SW2.Stop()
					p_Extract	= -1									'抽出中ステータス->未実行

					Dim sTbl As String = ds.Tables(p_MeasState).TableName
					dtRow = ds.Tables(sTbl).NewRow
					dtRow("Weight") = dMeasWT
					dtRow("Stress") = dMeasAD
					ds.Tables(sTbl).Rows.Add(dtRow)
					m_Result	= 1										'結果更新完了
				End If
			End if
		Else
		'モニタ中止
			m_Job = 9
		End if

    Case 8
    Case 9
		'制御初期化
		m_Job = 0
        m_GetCount = 0
		m_SW1.Stop()
		m_SW1.Reset()
		dtStrees.Clear

    End Select
Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub
''' <summary>
''' 重量校正応力モニタグラフ表示更新
''' </summary>
''' <param name="iMode">1:X 2:Y</param>
''' <param name="oChart"></param>
''' <param name="ds"></param>
''' <param name="iXSpan"></param>
''' <param name="iYMin"></param>
''' <param name="iYMax"></param>
Public Sub WeightMonitorChartDisp(ByVal iMode As Int16, ByRef oChart As Chart, ByVal ds As Dataset, ByVal iXSpan As Uint32, ByVal iYMin As Int64 , ByVal iYMax As Int64 , ByVal bYSpanAuto As Boolean)
Dim sSeriesName As String
Dim sType		As String = IIf(iMode = 1, "X", "Y")
Try
	if ds.Tables(0).Rows.Count = 0 Or iMode = -1
		Exit sub
	End If
'チャートコントロールの設定
	With oChart
	'初期化
	.Series.Clear()
	.Legends.Clear()
	.Titles.Clear()
	.ChartAreas.Clear()
	.DataSource = ds

    'timeとX,Yを取得
	sSeriesName = ds.Tables(0).Columns(1).ColumnName
	.Series.Add(sSeriesName)
	.Series(sSeriesName).YValueMembers = sSeriesName

	.Series(sSeriesName).ChartType = SeriesChartType.FastLine
	'.Series(sSeriesName).ChartType = SeriesChartType.FastPoint
	.Series(sSeriesName).XValueMember = ds.Tables(0).Columns(0).ColumnName

	.Series(sSeriesName).YAxisType = AxisType.Primary
	.Series(sSeriesName).Color = Color.Blue

	'TITLE 作成
	Dim title1 As Title = new Title(sType & " Stress Monitor")
	.Titles.Add(title1)
	.Titles.Item(0).Font = New Font(BASE_FONT, 16, FontStyle.Bold)
    'ChartArea作成
    .ChartAreas.Add("ChartArea1")
	'Legendの作成
	'leg.DockedToChartArea = "ChartArea1"
	dim leg As Legend = new Legend With {
		.Alignment = StringAlignment.Near
	}
	.Legends.Add(leg)
	.Legends(0).Enabled = False


		
    '-----------------------------------------------------------------------------------------------------
    '以下、Ｘ軸とＹ軸の設定
	.ChartAreas(0).AxisX.Maximum = ds.Tables(0).Rows(ds.Tables(0).Rows.Count-1)(0)
	If .ChartAreas(0).AxisX.Maximum-iXSpan <= 0
		.ChartAreas(0).AxisX.Minimum =  0
	Else
		.ChartAreas(0).AxisX.Minimum =  .ChartAreas(0).AxisX.Maximum - iXSpan
	End If
    '.ChartAreas(0).AxisX.Maximum = ds.Tables(0).Rows.Count 
	'If ds.Tables(0).Rows.Count-iXSpan <= 0
	'	.ChartAreas(0).AxisX.Minimum =  0
	'Else
	'	.ChartAreas(0).AxisX.Minimum =  ds.Tables(0).Rows.Count - iXSpan
	'End If
	Dim YMax As Int64
	Dim YMin As Int64
	if bYSpanAuto
		YMax = Convert.ToInt64( ds.Tables(0).Compute("MAX(ADValue)", "") )
		YMin = Convert.ToInt64( ds.Tables(0).Compute("MIN(ADValue)", "") )
		If YMax - YMin <= 0
			YMax = YMax + 1000
			YMin = YMin - 1000
		End If
	Else
		YMax = iYMax
		YMin = iYMin
	End if
	.ChartAreas(0).AxisY.Maximum = YMax
	.ChartAreas(0).AxisY.Minimum = YMin

	.ChartAreas(0).AxisX.Title                      = "Time[Sec]"
    .ChartAreas(0).AxisX.TitleFont                  = New Font(BASE_FONT, 12, FontStyle.Regular)
	.ChartAreas(0).AxisX.Enabled					= AxisEnabled.Auto
    .ChartAreas(0).AxisX.Crossing                   = Double.NaN
	.ChartAreas(0).AxisX.IntervalType               = DateTimeIntervalType.NotSet
	.ChartAreas(0).AxisX.MajorGrid.Enabled          = True
	.ChartAreas(0).AxisX.MajorGrid.LineDashStyle    = ChartDashStyle.Dot
    .ChartAreas(0).AxisX.MajorGrid.LineColor        = Color.LightGray
	.ChartAreas(0).AxisX.Interval					= iXSpan / 10
	.ChartAreas(0).AxisX.LabelStyle.Format			= "{0}"

	.ChartAreas(0).AxisY.Title                      = "Stress[Dig] "
    .ChartAreas(0).AxisY.TitleFont                  = New Font(BASE_FONT, 12, FontStyle.Regular)
    .ChartAreas(0).AxisY.Crossing                   = Double.NaN
	.ChartAreas(0).AxisY.IntervalType               = DateTimeIntervalType.NotSet
	.ChartAreas(0).AxisY.MajorGrid.Enabled          = True
	.ChartAreas(0).AxisY.MajorGrid.LineDashStyle    = ChartDashStyle.Dot
    .ChartAreas(0).AxisY.MajorGrid.LineColor        = Color.LightGray

	'-------------------------------------------------------------------------------------------------------------
    'ChartAreaのサイズ位置調整
	.ChartAreas(0).Position.Auto					= False
	.ChartAreas(0).Position.X						= 20
	.ChartAreas(0).Position.Y						= 20
	.ChartAreas(0).Position.height					= 90
	.ChartAreas(0).Position.Width					= 100

	'データテーブルとの結合
	Try
	.DataBind()
	Catch ex As Exception
	End Try

    End With
Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub
''' <summary>
''' 重量校正結果をデータグリッドビューへ表示
''' </summary>
''' <param name="iMode"></param>
''' <param name="ds"></param>
Public Sub WeightCalibDgvDisp(ByVal iMode As Int16, ByRef ds As DataSet)
Try
Dim dgv As DataGridView = IIf( iMode =1, dgvXResult, dgvYResult )
With dgv
	If iMode = -1
		Return
	End If
	Dim sTbl As String = ds.Tables(iMode).TableName
	.DataSource = ds.Tables(sTbl)

	'.ReadOnly = True					'読取専用
	.RowHeadersVisible = False			'列ヘッダ消去
	.AllowUserToAddRows = False			'追加用行を消去
	.CurrentCell = Nothing
	.Font = New Font("游ゴシック", 10)
	.Columns(0).DefaultCellStyle.format = "F1"
	.Columns(1).DefaultCellStyle.format = "F1"
	.Columns(0).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
	.Columns(0).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
	.Columns(1).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
	.Columns(1).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter

	.Columns(0).Width						= 60
	.Columns(1).Width						= 90

	.AllowUserToDeleteRows = True

End With
Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub
''' <summary>
''' 重量校正結果グラフ表示更新
''' </summary>
''' <param name="iMode">1:X 2:Y</param>
''' <param name="oChart"></param>
''' <param name="ds"></param>
Public Sub WeightCalibChartDisp(ByVal iMode As Int16, ByRef oChart As Chart, ByVal ds As Dataset)
Dim sSeriesName As String
Dim sType		As String = IIf(iMode = 1, "X", "Y")
Dim sTbl		As String = Nothing
Try
	if iMode = -1 Or ds.Tables.Count = 0
		Exit sub
	End If
'チャートコントロールの設定
	With oChart
	'初期化
	.Series.Clear()
	.Legends.Clear()
	.Titles.Clear()
	.ChartAreas.Clear()
	sTbl = ds.Tables(iMode).TableName
	.DataSource = ds.Tables(sTbl)

    'X,Yを取得
	sSeriesName = ds.Tables(sTbl).Columns(1).ColumnName
	.Series.Add(sSeriesName)
	.Series(sSeriesName).YValueMembers = sSeriesName
	.Series(sSeriesName).XValueMember = ds.Tables(sTbl).Columns(0).ColumnName

	.Series(sSeriesName).ChartType		= SeriesChartType.FastPoint
	.Series(sSeriesName).MarkerSize		= 10
	.Series(sSeriesName).MarkerStyle	= MarkerStyle.Circle
	.Series(sSeriesName).YAxisType		= AxisType.Primary
	.Series(sSeriesName).Color			= Color.Blue

	'TITLE 作成
	Dim title1 As Title = new Title(sType & " result Chart")
	.Titles.Add(title1)
	.Titles.Item(0).Font = New Font("meiryo ui", 16, FontStyle.Bold)
    'ChartArea作成
    .ChartAreas.Add("ChartArea1")
	'Legendの作成
	'leg.DockedToChartArea = "ChartArea1"
	Dim leg As Legend = new Legend With {
		.Alignment = StringAlignment.Near
	}
	.Legends.Add(leg)
	.Legends(0).Enabled = False

    '-----------------------------------------------------------------------------------------------------
    '以下、Ｘ軸とＹ軸の設定
	If ds.Tables(iMode).Rows.Count < 1
		Return
	End If
	.ChartAreas(0).AxisX.Maximum = 1000
	.ChartAreas(0).AxisX.Minimum = 0

	Dim YMax As Int64
	Dim YMin As Int64
	YMax = Convert.ToInt64( ds.Tables(sTbl).Compute("MAX(Stress)", "") )
	YMin = Convert.ToInt64( ds.Tables(sTbl).Compute("MIN(Stress)", "") )
	If YMax - YMin <= 0
		YMax = YMax + 1
		YMin = YMin - 1
	End If
	.ChartAreas(0).AxisY.Maximum = YMax+10
	.ChartAreas(0).AxisY.Minimum = YMin-10

	.ChartAreas(0).AxisX.Title                      = "Weight[g] "
    .ChartAreas(0).AxisX.TitleFont                  = New Font("meiryo ui", 12, FontStyle.Regular)
    .ChartAreas(0).AxisX.Crossing                   = Double.NaN
	.ChartAreas(0).AxisX.IntervalType               = DateTimeIntervalType.NotSet
	.ChartAreas(0).AxisX.MajorGrid.Enabled          = True
	.ChartAreas(0).AxisX.MajorGrid.LineDashStyle    = ChartDashStyle.Dot
    .ChartAreas(0).AxisX.MajorGrid.LineColor        = Color.LightGray
	.ChartAreas(0).AxisX.Interval					= 100

	.ChartAreas(0).AxisY.Title                      = "Stress[dig]"
    .ChartAreas(0).AxisY.TitleFont                  = New Font("meiryo ui", 12, FontStyle.Regular)
	.ChartAreas(0).AxisY.Enabled					= AxisEnabled.Auto
    .ChartAreas(0).AxisY.Crossing                   = Double.NaN
	.ChartAreas(0).AxisY.IntervalType               = DateTimeIntervalType.NotSet
	.ChartAreas(0).AxisY.MajorGrid.Enabled          = True
	.ChartAreas(0).AxisY.MajorGrid.LineDashStyle    = ChartDashStyle.Dot
    .ChartAreas(0).AxisY.MajorGrid.LineColor        = Color.LightGray
	.ChartAreas(0).AxisY.Interval					= (.ChartAreas(0).AxisY.Maximum-.ChartAreas(0).AxisY.Minimum) / 5
	.ChartAreas(0).AxisY.LabelStyle.Format			= "{0}"

	'-------------------------------------------------------------------------------------------------------------
    'ChartAreaのサイズ位置調整
	.ChartAreas(0).Position.Auto					= False
	.ChartAreas(0).Position.X						= 0
	.ChartAreas(0).Position.Y						= 20
	.ChartAreas(0).Position.height					= 90
	.ChartAreas(0).Position.Width					= 90

	'データテーブルとの結合
	Try
	.DataBind()
	Catch ex As Exception
	End Try

    End With
Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub

Private Sub rbX_CheckedChanged(sender As Object, e As EventArgs) Handles rbX.CheckedChanged
	if rbX.Checked
		'重量校正結果チャート表示
		WeightCalibChartDisp(1, ResultChart, ds)
		CalcParameterX(dtXWeight)
	End If
End Sub

Private Sub rbY_CheckedChanged(sender As Object, e As EventArgs) Handles rbY.CheckedChanged
	if rbY.Checked
		'重量校正結果チャート表示
		WeightCalibChartDisp(2, ResultChart, ds)
		CalcParameterY(dtYWeight)
	End If
End Sub
Dim m_sDgv		As String	= nothing
Dim m_iRowIdx	As Int16	= 0

''' <summary>
''' データグリッドビュー右クリックメニュー表示
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub dgvYResult_CellContextMenuStripNeeded(sender As Object, e As DataGridViewCellContextMenuStripNeededEventArgs) Handles dgvXResult.CellContextMenuStripNeeded, dgvYResult.CellContextMenuStripNeeded
Try
	If (e.RowIndex < 0) Then Return
	If (e.ColumnIndex < 0) Then Return
	Dim dgv As DataGridView = sender
	m_sDgv		= dgv.Name												'X or Y
	m_iRowIdx	= e.RowIndex
	Dim cell As DataGridViewCell = dgv(e.ColumnIndex, e.RowIndex)	' クリックされたセルの特定
	cell.Selected = True											' セルを選択状態にする。

	'コンテキストメニューストリップを表示
	Dim p As System.Drawing.Point = System.Windows.Forms.Cursor.Position
	cmsResultDgv.Show(p)
Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub
'データグリッドビューコンテキストメニューから削除をクリック
Private Sub DeleteToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DeleteToolStripMenuItem.Click
Try
	Select Case m_sDgv
	Case "dgvXResult"
		dtXWeight.Rows.RemoveAt(m_iRowIdx)				'データテーブル行削除
		WeightCalibChartDisp(1, ResultChart, ds)		'重量校正結果チャート表示
		CalcParameterX(dtXWeight)						'Xパラメータ計算
		SaveToCSV(dtXWeight, Path.Combine(m_SetPath, m_ExportFileNameBaseX), True, ",", "", "'", Encoding.GetEncoding("shift_jis") )
	Case "dgvYResult"
		dtYWeight.Rows.RemoveAt(m_iRowIdx)				'データテーブル行削除
		WeightCalibChartDisp(2, ResultChart, ds)		'重量校正結果チャート表示
		CalcParameterY(dtYWeight)						'Yパラメータ計算
		SaveToCSV(dtYWeight, Path.Combine(m_SetPath, m_ExportFileNameBaseY), True, ",", "", "'", Encoding.GetEncoding("shift_jis") )
	End Select

Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub
''' <summary>
''' 最小二乗法で直線方程式を導き、傾きを返す
''' </summary>
''' <param name="dt">データテーブル</param>
''' <param name="a">傾き</param>
Private Sub	CalcParameter( byVal dt As DataTable, ByRef a As Double )
Dim dX()	As Double = Nothing				'変換前
Dim dY()	As Double = Nothing				'
Dim dNX()	As Double = Nothing				'変換後
Dim dNY()	As Double = Nothing
Dim dRet()  As Double = Nothing
Dim i As int32
Try
	If dt.Rows.Count >= 2
		'変換前配列にデータテーブルからセット
		Array.Resize(dX, dt.Rows.Count)
		Array.Resize(dY, dt.Rows.Count)
		For i = 0 To dt.Rows.Count - 1
			dX(i) = dt.Rows(i)(0)
			dY(i) = dt.Rows(i)(1)
		Next i
		Array.Resize(dNX, dt.Rows.Count)
		Array.Resize(dNY, dt.Rows.Count)
		'最小二乗法を1次式近似で実行
		dRet = p_dtFile.fnLeastSquaresMethod(dX, dY, dNX, dNY, dt.Rows.Count, 1)
		a = dRet(1)		'傾きをセット
	End if
Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	dRet = Nothing
End Try
End Sub
''' <summary>
''' 最小二乗法で算出した傾きの逆数をdCoefXへセット
''' </summary>
Public Function CalcParameterX(byVal dt As DataTable) As Double
	Dim a As Double = Double.NaN
	CalcParameter(dt, a)
	dCoefX = 1/a
	Return(dCoefX)
End Function
''' <summary>
''' 最小二乗法で算出した傾きの逆数をdCoefYへセット
''' </summary>
Public Function CalcParameterY(byVal dt As DataTable) As Double
	Dim a As Double = Double.NaN
	CalcParameter(dt, a)
	dCoefY = 1/a
	Return(dCoefY)
End Function
'クリアボタンのクリック
Private Sub btndgvClear_Click(sender As Object, e As EventArgs) Handles btnXdgvClear.Click,btnYdgvClear.Click
Try
	Dim sTbl	As String = Nothing
	m_sDgv		= sender.Name												'X or Y
	Select Case m_sDgv
	Case "btnXdgvClear"
		dtXWeight.Clear									'データテーブル全行削除
		WeightCalibChartDisp(1, ResultChart, ds)		'重量校正結果チャート表示
		CalcParameterX(dtXWeight)						'パラメータ計算
	Case "btnYdgvClear"
		dtYWeight.Clear									'データテーブル全行削除
		WeightCalibChartDisp(2, ResultChart, ds)		'重量校正結果チャート表示
		CalcParameterY(dtYWeight)						'パラメータ計算
	End Select
	'パラメータ計算
Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub
'フォームクローズ
Private Sub frmCalibWeight_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
    Select case p_MeasState	
    Case -1
    Case 1
    '測定停止
        p_sCalbWMode = ""
        p_MeasState = -1
        btnXStart.BackColor = Control.DefaultBackColor
        btnYStart.BackColor = Control.DefaultBackColor
        btnXStart.Enabled = True
        btnYStart.Enabled = True
    Case 2
    '測定不可
    End Select

	'測定結果をファイル保存
	SaveToCSV(ds.Tables(1), Path.Combine(m_SetPath, m_ExportFileNameBaseX), True, ",", "", "'", Encoding.GetEncoding("shift_jis") )
	SaveToCSV(ds.Tables(2), Path.Combine(m_SetPath, m_ExportFileNameBaseY), True, ",", "", "'", Encoding.GetEncoding("shift_jis") )
End Sub
'フォームクローズ
Private Sub frmCalibWeight_FormClosed(sender As Object, e As FormClosedEventArgs) Handles Me.FormClosed
	Me.Dispose
End Sub
''' <summary>
''' データテーブルをCSVへ保存
''' </summary>
''' <param name="dt">データテーブル</param>
''' <param name="fileName">保存ファイル名</param>
''' <param name="hasHeader">ヘッダ要否</param>
''' <param name="separator">区切り文字</param>
''' <param name="quote">コーテーション文字</param>
''' <param name="replace">データ内のコーテーション文字の変換</param>
''' <param name="ed">エンコーディング</param>
Private Function SaveToCSV(dt As DataTable, fileName As String, hasHeader As Boolean, separator As String, quote As String, replace As String, ed As Encoding) As int32
Try
    Dim rows As Integer = dt.Rows.Count
    Dim cols As Integer = dt.Columns.Count
    Dim text As String
    '保存用のファイルを開く。上書きモードで。
    Dim writer As StreamWriter = New StreamWriter(fileName, False, ed)
    'カラム名を保存するか
    If hasHeader = True Then
        'カラム名を保存する場合
        For i As Integer = 0 To cols - 1
            'カラム名を取得
            If quote <> "" Then
                'データ内のクォーテーション文字のエスケープ処理
                text = dt.Columns(i).ColumnName.Replace(quote, replace)
            Else
                text = dt.Columns(i).ColumnName
            End If
            'レコード内の最後の列かどうか
            If i <> cols - 1 Then
                writer.Write(quote + text + quote + separator)
            Else
                '最後のレコードなら区切り文字は要らない
                writer.WriteLine(quote + text + quote)
            End If
        Next i
    End If
    'データの保存処理
    For i As Integer = 0 To rows - 1
        For j As Integer = 0 To cols - 1
            If (quote <> "") Then
                'データ内のクォーテーション文字のエスケープ処理
                text = dt.Rows(i)(j).ToString().Replace(quote, replace)
            Else
                text = dt.Rows(i)(j).ToString()
            End If
            'レコード内の最後の列かどうか
            If j <> cols - 1 Then
                writer.Write(quote + text + quote + separator)
            Else
                '最後のレコードなら区切り文字は要らない
                writer.WriteLine(quote + text + quote)
            End If
        Next j
    Next i
    'ストリームを閉じる
    writer.Close()
	Return 0
Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Return -1
End Try
End Function
''' <summary>
''' CSVファイルから読込みデータテーブルへセット
''' </summary>
''' <param name="ｓFolder">場所フォルダ名</param>
''' <param name="sFileName">場所ファイル名</param>
''' <returns>0：正常 -1:異常</returns>
Public Function CsvToDataTable(ByVal ｓFolder As String, ByVal sFileName As String, byRef dt As DataTable) As Int32
Try
	dt.Clear()				'一旦全行消去
	'接続文字列
	Dim conString	As String = "Driver={Microsoft Text Driver (*.txt; *.csv)};Dbq=" & ｓFolder & ";Extensions=asc,csv,tab,txt;"
	Dim con			As New System.Data.Odbc.OdbcConnection(conString)

	Dim commText	As String = "SELECT * FROM [" & sFileName & "]"
	Dim da			As New System.Data.Odbc.OdbcDataAdapter(commText, con)
	da.Fill(dt)				'DataTableに格納する
	Return 0
Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Return -1
End Try
End Function
''' <summary>
'''重量換算測定ファイルを読込みデータテーブルへセットする
''' </summary>
''' <param name="dtX">X軸測定結果テーブル</param>
''' <param name="dtY">Y軸測定結果テーブル</param>
Public Sub ReadWeightParameter(ByRef dtX As DataTable, ByRef dtY As DataTable)
	CsvToDataTable(m_SetPath, m_ExportFileNameBaseX, dtX)
	CsvToDataTable(m_SetPath, m_ExportFileNameBaseY, dtY)
End Sub
'重量加算/減算
Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click, btnSub.Click
	Dim dTotal	As Double = nudWeight.Value
	Dim dAddSub	As Double = nudWeightAdd.Value
Try
	Select Case sender.name
	Case "btnAdd"
		dTotal += dAddSub
	Case "btnSub"
		If dTotal-dAddSub < 0
			dTotal = 0
		Else
			dTotal -= dAddSub
		End If
	End Select
	nudWeight.Value = dTotal

Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub
End Class