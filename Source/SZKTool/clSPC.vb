Imports System.Windows.Forms.DataVisualization
Imports System.Windows.Forms.DataVisualization.Charting
Imports System.Xml.Schema
Imports MathNet.Numerics.Distributions
'Imports Microsoft.Office.Interop.Excel

' VB.NET SPC 計算クラス + DataGridView表示 + Chart描画
' 任意サンプル数に対応
Public Class SPCAnalyzer
'SPC構造体
Public Structure stSPC
	Dim sSampleName				As String				'サンプル名
	Dim sLot					As String				'ロット
	Dim sMeasurer				As String				'測定者
	Dim dStdCenter				As Double				'規格標準	オペレーター入力
	Dim dStdUpper				As Double				'規格上限	オペレーター入力
	Dim dStdLower				As Double				'規格下限	オペレーター入力
	Dim dCntCenter				As Double				'CL			先頭30サンプルで計算
	Dim dCntUpper				As Double				'UCL		先頭30サンプルで計算
	Dim dCntLower				As Double				'LCL		先頭30サンプルで計算
	Dim iCalcSample				As Int32				'CLを決定する標本数
	Dim dMax					As Double				'最大値(トレンド)
	Dim dMin					As Double				'最小値(トレンド)
	Dim dAve					As Double				'平均値(トレンド)
	Dim dSgm					As Double				'標準偏差(トレンド)
	Dim dSgmControl				As Double				'管理用σ（CL/UCL/LCL算出用）
	Dim dSgmProcess				As Double				'工程能力用σ（Cp/Cpk算出用）	
	Dim dCa						As Double				'Ca 工程中心化指数(トレンド)
	Dim dCp						As Double				'Cp 工程能力指数(トレンド)
	Dim dCpk					As Double				'Cpk工程能力指数k修正(トレンド)
	Dim XMin					As Int32				'表示トレンドサンプル数 最小
	Dim XMax					As Int32				'表示トレンドサンプル数 最大
	Dim sStatisticalRangeMin	As String 				'統計範囲最小日時 yyyy/mm/dd hh:mm:ss
	Dim sStatisticalRangeMax	As String 				'統計範囲最大日時 yyyy/mm/dd hh:mm:ss
	Dim Values()				As Double				'サンプルデータ配列(トレンド)  同じサンプル名、ロット
End Structure
Private _spc As stSPC
Private _isMovingCell As Boolean = False

'データグリッドビューの項目列文字
Private sDgvItem()			As String = Nothing
Private enum eDgvItem
	ITEM	= 0
	VALUE
	USL
	SL
	LSL
	UCL
	CL
	LCL
	SampForCL
	TotalSamp
	Max
	Min
	Ave
	SigC
	SigP
	CA
	CP
	CPK
End enum
'チャートの項目列文字
Private sChtItem()			As String = Nothing
Private enum eChtItem
	SampleName	= 0
	Lot
	XAxis
	YAxis
End Enum

''' <summary>
''' 初期化
''' </summary>
''' <param name="spc"></param>
Public Sub New(spc As stSPC, dgvItem() As String, chtItem() As String)
    _spc = spc
	sDgvItem = dgvItem
	sChtItem = chtItem
End Sub
Public Sub SetSpc(spc As stSPC)
    _spc = spc
End Sub
''' <summary>
''' 現在のサンプルデータ数を取得
''' </summary>
''' <returns></returns>
Public ReadOnly Property GetValuesCount() As Int32
	Get
		If IsNothing(_spc.Values)
			Return 0
		End If
		Return _spc.Values.Count
	End Get
End Property
''' <summary>
''' サンプル配列へ配列でデータ追加
''' </summary>
''' <param name="vals">サンプルデータ Double型配列</param>
Public Sub SetValues(vals As Double())
Try
    If vals Is Nothing Then
        _spc.Values = Nothing
    Else
        _spc.Values = CType(vals.Clone(), Double())
    End If

Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub
''' <summary>
''' サンプル配列へ1データ追加
''' </summary>
''' <param name="v"></param>
Public Sub AddValue(v As Double)
Try
    If _spc.Values Is Nothing Then
        ReDim _spc.Values(0)
        _spc.Values(0) = v
    Else
        Dim n = _spc.Values.Length
        ReDim Preserve _spc.Values(n)
        _spc.Values(n) = v
    End If

Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub

''' <summary>
''' 項目を現在のサンプルデータで再計算する
''' 規定サンプル数を越えたらCL,UCL,LCLを固定する
''' </summary>
''' <param name="firstN"></param>
Public Sub Calculate(Optional firstN As Integer = 30)
Try
    If _spc.Values Is Nothing OrElse _spc.Values.Length = 0 Then Exit Sub

    Dim n			As Int32 = _spc.Values.Length
    Dim subsetCount As Int32 = n
	Dim subSet()	As Double = Nothing

	Select Case n
	Case <= firstN	
		'最初の firstN サンプルを超えていない
		subsetCount = n
		subset = _spc.Values.Take(subsetCount).ToArray()
	Case Else 
		'最初の firstN サンプルを超えた
		subsetCount = firstN
		subset = _spc.Values.Take(subsetCount).ToArray()
	End Select

	'CL
    _spc.dCntCenter = subset.Average()
    Dim sigma = Math.Sqrt(subset.Sum(Function(x) (x - _spc.dCntCenter) ^ 2) / (If(subsetCount > 1, subsetCount - 1, 1)))

    '_spc.dSgm = sigma
	_spc.dSgmControl = sigma
	'UCL
    _spc.dCntUpper = _spc.dCntCenter + 3 * sigma
	'LCL
    _spc.dCntLower = _spc.dCntCenter - 3 * sigma

    ' ▼ トレンド情報は全データで再計算
    _spc.dMax = _spc.Values.Max()
    _spc.dMin = _spc.Values.Min()
    _spc.dAve = _spc.Values.Average()

	_spc.dSgmProcess = Math.Sqrt(_spc.Values.Sum(Function(x) (x - _spc.dAve) ^ 2) / (If(n > 1, n - 1, 1)))
    _spc.dCa = (_spc.dAve - _spc.dStdCenter) / ((_spc.dStdUpper - _spc.dStdLower) / 2)
    _spc.dCp = If(_spc.dSgmProcess = 0, 0, (_spc.dStdUpper - _spc.dStdLower) / (6 * _spc.dSgmProcess))
    _spc.dCpk = If(_spc.dSgmProcess = 0, 0, Math.Min((_spc.dStdUpper - _spc.dAve) / (3 * _spc.dSgmProcess), (_spc.dAve - _spc.dStdLower) / (3 * _spc.dSgmProcess)))

Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub
''' <summary>
''' 現在のSPC構造体データを取得する
''' </summary>
''' <returns></returns>
Public Function GetResult() As stSPC
    Return _spc
End Function


''' <summary>
'''  DataGridView に計算結果表示
''' </summary>
''' <param name="dgv"></param>
Public Sub ShowResultToGrid(dgv As DataGridView)
Try
    dgv.Rows.Clear()
    dgv.Columns.Clear()
    dgv.DefaultCellStyle.Font = New Font(BASE_FONT, 12)

	' ▼▼▼▼ ヘッダ色変更の追加コード ▼▼▼▼
    ' Windowsのテーマスタイルを無効化しないと背景色が反映されない
    dgv.EnableHeadersVisualStyles = False
        
    ' ヘッダの背景色と文字色を設定 (例: 背景=濃いグレー, 文字=白)
    dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.DimGray
    dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White
        
    ' (任意) ヘッダのフォントや配置も見やすく調整
    dgv.ColumnHeadersDefaultCellStyle.Font = New Font(BASE_FONT, 10, FontStyle.Bold)
    dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
    ' ▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲

    dgv.Columns.Add("Item", sDgvItem(edgvItem.ITEM))
    dgv.Columns.Add("Value", sDgvItem(edgvItem.VALUE))

	dgv.Columns(0).ReadOnly = True
    dgv.Columns(1).ReadOnly = True

    dgv.RowHeadersVisible = False
    dgv.AllowUserToAddRows = False
    dgv.SelectionMode = DataGridViewSelectionMode.CellSelect
	dgv.AllowUserToResizeRows = False
    dgv.MultiSelect = False

	    
    ' ▼ 先頭行に標準規格値を表示（編集可）
    AddEditableRow(dgv, sDgvItem(edgvItem.USL),	_spc.dStdUpper)
    AddEditableRow(dgv, sDgvItem(edgvItem.SL),	_spc.dStdCenter)
    AddEditableRow(dgv, sDgvItem(edgvItem.LSL),	_spc.dStdLower)

    ' ▼ 計算結果
    AddRowToGrid(dgv, sDgvItem(edgvItem.UCL),		_spc.dCntUpper,		"F2")
    AddRowToGrid(dgv, sDgvItem(edgvItem.CL),		_spc.dCntCenter,	"F2")
    AddRowToGrid(dgv, sDgvItem(edgvItem.LCL),		_spc.dCntLower,		"F2")
    AddRowToGrid(dgv, sDgvItem(edgvItem.SampForCL),	_spc.iCalcSample,	"F0")
    AddRowToGrid(dgv, sDgvItem(edgvItem.TotalSamp), If(IsNothing(_spc.Values), 0, _spc.Values.Length), "F0")
    AddRowToGrid(dgv, sDgvItem(edgvItem.Max),		_spc.dMax,			"F2")
    AddRowToGrid(dgv, sDgvItem(edgvItem.Min),		_spc.dMin,			"F2")
    AddRowToGrid(dgv, sDgvItem(edgvItem.Ave),		_spc.dAve,			"F3")
    AddRowToGrid(dgv, sDgvItem(edgvItem.SigC),		_spc.dSgmControl,	"F2")
    AddRowToGrid(dgv, sDgvItem(edgvItem.SigP),		_spc.dSgmProcess,	"F2")
    AddRowToGrid(dgv, sDgvItem(edgvItem.CA),		_spc.dCa,			"F5")
    AddRowToGrid(dgv, sDgvItem(edgvItem.CP),		_spc.dCp,			"F4")
    AddRowToGrid(dgv, sDgvItem(edgvItem.CPK),		_spc.dCpk,			"F2")

    ' ★ 編集可能セルのみ ReadOnly を解除
    For Each r As DataGridViewRow In dgv.Rows
        If r.Cells(0).Value IsNot Nothing Then
            Select Case r.Cells(0).Value.ToString()
                Case sDgvItem(edgvItem.SL), sDgvItem(edgvItem.USL), sDgvItem(edgvItem.LSL)
                    r.Cells(1).ReadOnly = False
                Case Else
                    r.Cells(1).ReadOnly = True
            End Select
        End If
    Next

    'dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
	dgv.Columns(0).Width	= dgv.Width * 0.66
	dgv.Columns(1).Width	= dgv.Width * 0.33
    For Each row As DataGridViewRow In dgv.Rows
        row.Height = 28
    Next

    ' ▼ イベント登録（重複防止）
    RemoveHandler dgv.CellEnter, AddressOf Dgv_CellEnter
    RemoveHandler dgv.CellEndEdit, AddressOf Dgv_CellEndEdit
    AddHandler dgv.CellEnter, AddressOf Dgv_CellEnter
    AddHandler dgv.CellEndEdit, AddressOf Dgv_CellEndEdit

Catch ex As Exception
    Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub



' DataGridView 行追加のヘルパー（クラスレベルのメソッドにしてネストを避ける）
' DataGridView 行追加（表示専用）
Private Sub AddRowToGrid(dgv As DataGridView, name As String, value As Double, sForm As String)
Try
    Dim display As String = String.Empty
    If Not IsNothing(value)
        display = value.ToString(sForm)
    End If
    dgv.Rows.Add(name, display)
Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub

' 編集可能行（SL / USL / LSL）を追加
Private Sub AddEditableRow(dgv As DataGridView, name As String, value As Double)
    dgv.Rows.Add(name, value.ToString("F0"))
    Dim r = dgv.Rows(dgv.Rows.Count - 1)
    r.Cells(0).ReadOnly = True
    r.Cells(1).ReadOnly = False
End Sub
' 編集セル以外へのカーソル移動を禁止
Private Sub Dgv_CellEnter(sender As Object, e As DataGridViewCellEventArgs)
Try
    Dim dgv = CType(sender, DataGridView)
    If e.RowIndex < 0 OrElse e.ColumnIndex < 0 Then
        Exit Sub
    End If

    Dim cell = dgv.Rows(e.RowIndex).Cells(e.ColumnIndex)
    If cell.ReadOnly Then
        MoveToFirstEditableCell(dgv)
        Exit Sub
    End If
Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub

' 編集後チェック → 構造体反映 → 再計算
Private Sub Dgv_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs)
Try
    Dim dgv = CType(sender, DataGridView)
    If e.ColumnIndex <> 1 Then Exit Sub

    Dim key = dgv.Rows(e.RowIndex).Cells(0).Value.ToString()
    Dim txt = dgv.Rows(e.RowIndex).Cells(1).Value?.ToString()
    Dim v As Double
    If Not Double.TryParse(txt, v) Then
        MessageBox.Show("数値を入力してください")
        MoveToFirstEditableCell(dgv)
        Exit Sub
    End If

    Select Case key
    Case sDgvItem(edgvItem.SL)  : _spc.dStdCenter = v
    Case sDgvItem(edgvItem.USL) : _spc.dStdUpper = v
    Case sDgvItem(edgvItem.LSL) : _spc.dStdLower = v
    Case Else : Exit Sub
    End Select

    Calculate()
Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub

' ★ 編集可能セルへ強制移動
Private Sub MoveToFirstEditableCell(dgv As DataGridView)
Try
    If _isMovingCell Then Exit Sub
    _isMovingCell = True

    dgv.BeginInvoke(Sub()
        Try
            For Each r As DataGridViewRow In dgv.Rows
                If r.Cells(1).ReadOnly = False Then
                    dgv.CurrentCell = r.Cells(1)
                    Exit For
                End If
            Next
        Finally
            _isMovingCell = False
        End Try
    End Sub)
Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub








' Chart へトレンド描画
''' <summary>
''' Chartへトレンド表示
''' </summary>
''' <param name="chart">Chartコントロール</param>
''' <param name="xMin">X軸最小値</param>
''' <param name="xMax">X軸最大値</param>
Public Sub DrawTrend(chart As Chart, xMin As Double, xMax As Double)
Try
	'前回のラベル（TextAnnotation）をクリア
    chart.Annotations.Clear()
	chart.Series.Clear()
    chart.ChartAreas.Clear()
	chart.Legends.Clear()

    Dim area As New ChartArea("Trend")
    chart.ChartAreas.Add(area)
	area.BorderColor = Color.Black
	area.BorderWidth = 1
	area.BorderDashStyle = ChartDashStyle.Solid
	area.Position.Auto = False
	area.Position.X			= 0		' 左からの位置 %
	area.Position.Y			= 10	' 上からの位置 %
	area.Position.Width		= 96	' 幅 %
	area.Position.Height	= 90	' 高さ %
	'X表示レンジ
	'=== X 軸範囲設定 ==='
	With area.AxisX
        .Minimum = xMin
        .Maximum = xMax
        ' ▼ X軸は整数ラベルを優先（最大10分割）
        Dim xRange As Double = xMax - xMin
        If xRange > 0 Then
            Dim intInterval As Integer = Math.Max(1, CInt(Math.Ceiling(xRange / 10.0)))
            .Interval = intInterval
            .LabelStyle.Format = "0"
        Else
            .Interval = 1
            .LabelStyle.Format = "0"
        End If
        .IsStartedFromZero = False
        .IsMarginVisible = False
    End With

    ' ▼ Chart タイトル（Bold, 12pt）
    chart.Titles.Clear()
    Dim ttl As New Title($"{sChtItem(eChtItem.SampleName)}：[{_spc.sSampleName}]      {sChtItem(eChtItem.Lot)}：[{_spc.sLot}]")
    ttl.Font = New Font(BASE_FONT, 12, FontStyle.Bold)
    chart.Titles.Add(ttl)

    ' ▼ 軸タイトル
    area.AxisX.Title = sChtItem(eChtItem.XAxis)
    area.AxisX.TitleFont = New Font(BASE_FONT, 10, FontStyle.Bold)
    area.AxisY.Title = sChtItem(eChtItem.YAxis)
    area.AxisY.TitleFont = New Font(BASE_FONT, 10, FontStyle.Bold)
	area.AxisY.LabelStyle.Format = "F2"

    ' ▼ グリッドライン LightGray ドット線
    With area.AxisX.MajorGrid
        .LineColor = Color.LightGray
        .LineDashStyle = ChartDashStyle.Dot
    End With
    With area.AxisY.MajorGrid
        .LineColor = Color.LightGray
        .LineDashStyle = ChartDashStyle.Dot
    End With

    ' ▼ データ系列（緑・マーカー付き）
    Dim sData = New Series("Data") With {
        .ChartType = SeriesChartType.Line,
        .Color = Color.Green,
        .MarkerStyle = MarkerStyle.Circle,
        .MarkerSize = 10,
        .BorderWidth = 2
    }

    Dim hasData As Boolean = (_spc.Values IsNot Nothing AndAlso _spc.Values.Length > 0)
    If hasData Then
        For i = 0 To _spc.Values.Length - 1
            sData.Points.AddXY(i + 1, _spc.Values(i))
        Next
        chart.Series.Add(sData)
	Else
		Exit sub
    End If

    '---------------------------------
    ' Y軸スケール（CL と SL の大きい方の幅を基準に 30% マージン）
    '---------------------------------
    Dim yMin As Double = Double.MaxValue
    Dim yMax As Double = Double.MinValue


    If hasData Then
        yMin = Math.Min(yMin, _spc.Values.Min())
        yMax = Math.Max(yMax, _spc.Values.Max())
    End If

    ' 規格・管理限界を考慮
    yMin = Math.Min(yMin, Math.Min(_spc.dStdLower, _spc.dCntLower))
    yMax = Math.Max(yMax, Math.Max(_spc.dStdUpper, _spc.dCntUpper))

    Dim centerY As Double = _spc.dCntCenter

    ' CL・SL のズレ量
    Dim diffCL As Double = Math.Max(Math.Abs(yMax - centerY), Math.Abs(centerY - yMin))
    Dim diffSL As Double = Math.Max(Math.Abs(_spc.dStdUpper - centerY), Math.Abs(centerY - _spc.dStdLower))

    ' 大きい方を採用
    Dim baseRange As Double = Math.Max(diffCL, diffSL)
    If baseRange = 0 Then baseRange = 1

    ' 20% マージン追加
    Dim margin As Double = baseRange * 0.3
    Dim finalRange As Double = baseRange + margin

    area.AxisY.Minimum = centerY - finalRange
    area.AxisY.Maximum = centerY + finalRange

    ' ▼ Y軸を任意分割（10分割）
    Dim yRange As Double = area.AxisY.Maximum - area.AxisY.Minimum
    If yRange > 0 Then
        area.AxisY.Interval = yRange / 10
    End If

	chart.Invalidate()
	chart.Update()

    ' ▼ 管理限界線（CL / UCL / LCL） 青
	AddControlLine(area, _spc.dCntUpper,	Color.Blue) ' UCL
	AddControlLine(area, _spc.dCntCenter,	Color.Blue) ' CL
	AddControlLine(area, _spc.dCntLower,	Color.Blue) ' LCL
	AddLimitLabelByRelative(chart, area, "UCL", _spc.dCntUpper,		Color.Blue, New Font(BASE_FONT, 8))
	AddLimitLabelByRelative(chart, area, "CL",  _spc.dCntCenter,	Color.Blue, New Font(BASE_FONT, 8))
	AddLimitLabelByRelative(chart, area, "LCL", _spc.dCntLower,		Color.Blue, New Font(BASE_FONT, 8))

    ' ▼ 規格ライン (SL / USL / LSL) 赤
	AddControlLine(area, _spc.dStdUpper,	Color.Red) ' USL
	AddControlLine(area, _spc.dStdCenter,	Color.Red) ' SL
	AddControlLine(area, _spc.dStdLower,	Color.Red) ' LSL
	AddLimitLabelByRelative(chart, area, "USL", _spc.dStdUpper,		Color.Red, New Font(BASE_FONT, 8))
    AddLimitLabelByRelative(chart, area, "SL", _spc.dStdCenter,		Color.Red, New Font(BASE_FONT, 8))
	AddLimitLabelByRelative(chart, area, "LSL", _spc.dStdLower,		Color.Red, New Font(BASE_FONT, 8))

Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub

' ▼ 水平線追加（管理線）
Private Sub AddHLine(chart As Chart, area As ChartArea, name As String, y As Double, clr As Color)
Try
    Dim s = New Series(name) With {
        .ChartType = SeriesChartType.Line,
        .Color = clr,
        .BorderDashStyle = ChartDashStyle.Dash
    }
    For i = 1 To Math.Max(1, If(_spc.Values Is Nothing, 1, _spc.Values.Length))
        s.Points.AddXY(i, y)
    Next
    chart.Series.Add(s)
Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub

#If 0
#Else
' ▼ 規格/管理ライン数値ラベル
Private Sub AddLimitLine(chart As Chart, area As ChartArea, label As String, y As Double, clr As Color, lineWidth As Integer, lblFont As Font)
Try
    Dim s = New Series(label) With {
        .ChartType = SeriesChartType.Line,
        .Color = clr,
        .BorderWidth = lineWidth,
        .IsVisibleInLegend = False
    }

    ' ラインは常に表示範囲いっぱいに引く
    s.Points.AddXY(area.AxisX.Minimum, y)
    s.Points.AddXY(area.AxisX.Maximum, y)
    chart.Series.Add(s)

    ' ▼ 数値ラベル（ピクセル座標で直接配置）
    Dim ta As New TextAnnotation()
    ta.Text = $"{label}={y:F2}"
    ta.ForeColor = clr
    ta.Font = lblFont
    ta.BackColor = Color.White
    ta.LineColor = clr

    ' Annotation を ChartArea に紐付け
    ta.AxisX = area.AxisX
    ta.AxisY = area.AxisY
    ta.IsSizeAlwaysRelative = True

    ta.X = (area.AxisX.Maximum - area.AxisX.Minimum) - ta.Text
    ta.Y = _spc.dStdCenter
    ta.Alignment = ContentAlignment.TopRight
    chart.Annotations.Add(ta)

Catch ex As Exception
    Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub
' ▼ 規格/管理ライン
Private Sub AddControlLine(area As ChartArea, y As Double, clr As Color)
Try
    Dim sl As New StripLine()
    sl.IntervalOffset = y
    sl.StripWidth = 0
    sl.BorderColor = clr
    sl.BorderWidth = 1
    sl.BorderDashStyle = ChartDashStyle.solid

    area.AxisY.StripLines.Add(sl)

Catch ex As Exception
    Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub
''' <summary>
''' ライン数値ラベルの配置
''' </summary>
''' <param name="chart"></param>
''' <param name="area"></param>
''' <param name="label"></param>
''' <param name="yValue"></param>
''' <param name="clr"></param>
''' <param name="font"></param>
Private Sub AddLimitLabelByRelative(chart As Chart, area As ChartArea, label As String, yValue As Double,  clr As Color, font As Font)
Try
    Dim ta As New TextAnnotation()
    ta.Text = $"{label}={yValue:F2}"
    ta.ForeColor = clr
    ta.Font = font
    ta.BackColor = Color.White
    ta.LineColor = clr

    ' ★ Axis 座標系に直接バインド（ズレ防止の決定版）
    ta.AxisX = area.AxisX
    ta.AxisY = area.AxisY

    ' X は表示範囲の右端、Y はライン値そのもの
    ta.X = area.AxisX.Maximum - (area.AxisX.Maximum - area.AxisX.Minimum) * 0.10		'area右端から10%の位置
    ta.Y = yValue

    ta.Alignment = ContentAlignment.TopRight
    ta.IsSizeAlwaysRelative = True

    chart.Annotations.Add(ta)

Catch ex As Exception
    Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub

#End If
' Values をクリアして計算済みフィールドも初期化します
Public Sub ClearValues()
Try
    _spc.Values = Nothing
    _spc.dMax = 0
    _spc.dMin = 0
    _spc.dAve = 0
    _spc.dSgmControl = 0
    _spc.dSgmProcess = 0
    _spc.dCntCenter = 0
    _spc.dCntUpper = 0
    _spc.dCntLower = 0
    _spc.dCa = 0
    _spc.dCp = 0
    _spc.dCpk = 0

Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub
'========================================
' CSV エクスポート機能
'========================================
''' <summary>
''' SPC結果およびサンプルデータをCSVファイルへエクスポート
''' </summary>
''' <param name="filePath">保存先CSVファイルパス</param>
Public Sub ExportToCsv(filePath As String)
Try
    Using sw As New System.IO.StreamWriter(filePath, False, System.Text.Encoding.UTF8)
        ' ▼ ヘッダ情報
        sw.WriteLine($"{sChtItem(eChtItem.SampleName)}, {_spc.sSampleName}")
        sw.WriteLine($"{sChtItem(eChtItem.Lot)}, {_spc.sLot}")
        sw.WriteLine($"Measurer, {_spc.sMeasurer}")
        sw.WriteLine()

        ' ▼ 規格・管理値
        sw.WriteLine($"{sDgvItem(eDgvItem.ITEM)}, {sDgvItem(eDgvItem.VALUE)}")
        sw.WriteLine($"{sDgvItem(eDgvItem.USL)}, {_spc.dStdUpper}")
        sw.WriteLine($"{sDgvItem(eDgvItem.SL)}, {_spc.dStdCenter}")
        sw.WriteLine($"{sDgvItem(eDgvItem.LSL)}, {_spc.dStdLower}")
        sw.WriteLine($"{sDgvItem(eDgvItem.UCL)}, {_spc.dCntUpper}")
        sw.WriteLine($"{sDgvItem(eDgvItem.CL)}, {_spc.dCntCenter}")
        sw.WriteLine($"{sDgvItem(eDgvItem.LCL)}, {_spc.dCntLower}")
        sw.WriteLine($"{sDgvItem(eDgvItem.SampForCL)}, {_spc.iCalcSample}")
        sw.WriteLine($"{sDgvItem(eDgvItem.TotalSamp)}, {If(_spc.Values Is Nothing, 0, _spc.Values.Length)}")
        sw.WriteLine($"{sDgvItem(eDgvItem.Max)}, {_spc.dMax}")
        sw.WriteLine($"{sDgvItem(eDgvItem.Min)}, {_spc.dMin}")
        sw.WriteLine($"{sDgvItem(eDgvItem.Ave)}, {_spc.dAve}")
        sw.WriteLine($"{sDgvItem(eDgvItem.SigC)}, {_spc.dSgmControl}")
        sw.WriteLine($"{sDgvItem(eDgvItem.SigP)}, {_spc.dSgmProcess}")
        sw.WriteLine($"{sDgvItem(eDgvItem.CA)}, {_spc.dCa}")
        sw.WriteLine($"{sDgvItem(eDgvItem.CP)}, {_spc.dCP}")
        sw.WriteLine($"{sDgvItem(eDgvItem.CPK)}, {_spc.dCPK}")

        sw.WriteLine()
        ' ▼ サンプルデータ
        sw.WriteLine("Index,Value")
        If _spc.Values IsNot Nothing Then
            For i As Integer = 0 To _spc.Values.Length - 1
                sw.WriteLine("{0},{1}", i + 1, _spc.Values(i))
            Next
        End If
    End Using

Catch ex As Exception
    Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub


End Class
