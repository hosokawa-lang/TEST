Imports System.Drawing.Printing
Imports System.Threading
Imports System.IO
Imports System.Windows.Forms.DataVisualization.Charting
Imports Microsoft.VisualBasic.FileIO
Imports System.Collections
Imports System.Globalization
Imports SZKTool.frmOptions
Imports SZKTool.RealTimeMeas
Imports SZKTool.modPublic
Imports SZKTool.DataFile
Imports System.ComponentModel
Imports System.Runtime.CompilerServices
''' <summary>
''' 外挿線でゲルタイムを計算する方法のクラス
''' </summary>
Public Class Extra
Sub New()
	InitializeApp()
End Sub
'接線パラメータ構造体定義
Public Structure stTangentMatrix
	Dim a				As Double
	Dim b				As Double
End Structure

'接線パラメータ表
Private  m_Tan()		As stTangentMatrix = Nothing			
Public ReadOnly Property MatrixEnable() As Boolean
	Get
		MatrixEnable = Not IsNothing(m_Tan)
	End Get
End Property

'接線作成サンプル数
Private  iTanSamp		As int32 = 500
Public Property TangMakeSampleCount() As int32
	Get
		TangMakeSampleCount = iTanSamp
	End Get
	Set(Value As int32)
		iTanSamp = Value
	End Set
End Property

'データ間引き数
Private  iThining		As Int32 = 10
Public Property Thining() As int32
	Get
		Thining = iThining
	End Get
	Set(Value As int32)
		iThining = Value
	End Set
End Property

'開始接線の点(秒)
Private  dStartTime		As Double		
Public Property StartTime() As Double
	Get
		StartTime = dStartTime
	End Get
	Set(Value As Double)
		dStartTime = Value
	End Set
End Property

'中間接線の点(秒)
Private  dMidTime		As Double		
Public Property MidTime() As Double
	Get
		MidTime = dMidTime
	End Get
	Set(Value As Double)
		dMidTime = Value
	End Set
End Property

'終了接線の点(秒)
Private  dEndTime		As Double		
Public Property EndTime() As Double
	Get
		EndTime = dEndTime
	End Get
	Set(Value As Double)
		dEndTime = Value
	End Set
End Property

'ゲルタイム選択
Private sGelTSelect		As String = "Under"
Public Property GelTSelect As String
	Get
		GelTSelect = sGelTSelect
	End Get
    Set(value As String)
		sGelTSelect = value
    End Set
End Property

'時間->個数 換算係数
'Private  dRate			As Double = (MEAS_TIME_RESOLUTION / MEAS_TIME_BLOCK) * 10
Private  dRate			As Double = (MEAS_TIME_RESOLUTION / MEAS_TIME_BLOCK)
Public Property TimeToSampleRate() As Double
	Get
		TimeToSampleRate = dRate
	End Get
	Set(Value As Double)
		dRate = Value
	End Set
End Property

'マウスダウン時の時間座標
Private dMouseDownVal	As Double = Double.NaN
'マウスダウン時の時間座標
Private dMouseUpVal		As Double = Double.NaN

'マウスドラッグ中の選択点 0:開始点 1:中間点 2:終点 -1:未選択
Private iSelPoint		As Int16  = -1		
Public Property SelPoint() As Int16
	Get
		SelPoint = iSelPoint
	End Get
	Set(Value As int16)
		iSelPoint = Value
	End Set
End Property

'マウス選択範囲
Private iMouseRange		As Int16  = 5

'現在描画中の接線傾き
Private dA1				As Double = Double.NaN		'Under
Private dA2				As Double = Double.NaN		'Middle
Private dA3				As Double = Double.NaN		'Over
Public ReadOnly Property A1() As Double
	Get
		A1 = dA1
	End Get
End Property
Public ReadOnly Property A2() As Double
	Get
		A2 = dA2
	End Get
End Property
Public ReadOnly Property A3() As Double
	Get
		A3 = dA3
	End Get
End Property




Private anoSet1			As New LineAnnotation		'接線パラメータ開始点注釈
Private anoSet2			As New LineAnnotation		'接線パラメータ中間点注釈
Private anoSet3			As New LineAnnotation		'接線パラメータ終了点注釈
Private iStartSelIdx	As int32					'接線パラメータ表内の開始点Index
Private iMiddlSelIdx	As int32					'接線パラメータ表内の中点Index
Private iFinalSelIdx	As int32					'接線パラメータ表内の終了点Index
Private dLowerGelTX		As Double					'下側GelTime座標
Public ReadOnly Property LowerGelT As Double
	Get
		LowerGelT = dLowerGelTX
	End Get
End Property

Private dLowerGelTY		As Double					'下側GelTime座標
Private dMiddlGelTX		As Double					'中点GelTime座標
Public ReadOnly Property MiddlGelT As Double
	Get
		MiddlGelT = dMiddlGelTX
	End Get
End Property
Private dMiddlGelTY		As Double					'中点GelTime座標
Private dUpperGelTX		As Double					'上側GelTime座標
Public ReadOnly Property UpperGelT As Double
	Get
		UpperGelT = dUpperGelTX
	End Get
End Property
Private dUpperGelTY		As Double					'上側GelTime座標
''' <summary>
''' 開始点、中間点、終了点の時間を初期設定
''' </summary>
''' <param name="a"></param>
''' <param name="b"></param>
''' <param name="c"></param>
Public sub SetParameterTime(byVal a As Double, byVal b As Double, byVal c As Double)
	dStartTime		= a		'開始接線の点(秒)
	dMidTime		= b		'中間接線の点(秒)
	dEndTime		= c		'終了接線の点(秒)
End sub

''' <summary>
''' 接線調整する準備処理
''' </summary>
''' <param name="iMode">開始中間終了点 0:自動設定 1:外部指定</param>
''' <param name="oChart">対象チャート</param>
''' <param name="dt">データファイルクラス</param>
''' <param name="dVal1"></param>
''' <param name="dVal2"></param>
''' <param name="dVal3"></param>
Public Sub ExtraPrePareProc(ByVal iMode As Int16, ByRef oChart As Chart, ByRef dt As DataFile, ByRef dVal1 As Double, ByRef dVal2 As Double, ByRef dVal3 As Double)
Dim dTimeSpan As Double
Try
	If IsNothing(dt.iHosei)
		Return
	Else
		dTimeSpan = dt.iHosei.Length * (MEAS_TIME_RESOLUTION / MEAS_TIME_BLOCK)
	End If
	CreateTanMatrix()									'接線パラメータ表の生成
	oChart.Annotations.Clear							'全ての注釈ラインを消去
	StartLineDisp(oChart)								'開始点(下ベースライン)の接線を引く
	MiddleLineDisp(oChart)								'中間点(中間ライン)の接線を引く
	EndLineDisp(oChart)									'終了点(上ベースライン)の接線を引く
	If iMode <> 1
		If iStartSelIdx = 0 And iMiddlSelIdx = 0 And iFinalSelIdx = 0
		'開始・中間・終了点が全て0の場合は全測定時間を6分割して均等割り付けする
			dVal1 = dTimeSpan * 1 / 6
			dVal2 = dTimeSpan * 1 / 2
			dVal3 = dTimeSpan * 5 / 6
		Else
			dVal1 = (iStartSelIdx * iThining) * (MEAS_TIME_RESOLUTION / MEAS_TIME_BLOCK)
			dVal2 = (iMiddlSelIdx * iThining) * (MEAS_TIME_RESOLUTION / MEAS_TIME_BLOCK)
			dVal3 = (iFinalSelIdx * iThining) * (MEAS_TIME_RESOLUTION / MEAS_TIME_BLOCK)
		End If
	Else
	End if
	ExtrapoCursorAllDisp(oChart)						'開始・中間・終了点のカーソル表示
Catch ex As Exception
End Try
End Sub
'クラス初期化
Public Sub InitializeApp
	dLowerGelTY		= Double.NaN
	dMiddlGelTX		= Double.NaN
	dMiddlGelTY		= Double.NaN
	dUpperGelTX		= Double.NaN
	dUpperGelTY		= Double.NaN
	m_Tan			= Nothing			'接線パラメータテーブル
	iTanSamp		= 500				'接線作成サンプル数
	iThining		= 10				'データ間引き数

End Sub
Public Sub AllDispClear(ByRef oChart As Chart)
	oChart.Annotations.Clear							'全ての注釈ラインを消去
	oChart.ChartAreas(0).AxisX.StripLines.Clear()		'全てのストリップラインを消去
	oChart.ChartAreas(0).AxisY.StripLines.Clear()		'全てのストリップラインを消去
End Sub
''' <summary>
''' 接線パラメータ表の生成(m_Tan)
''' </summary>
Public Sub CreateTanMatrix()
	Dim dTimeAry()	As Double = Nothing								'間引き後タイム
	Dim dData()		As Double = Nothing								'間引き後データ
	Dim dCoef		As Double = 1.0									'補正データの変換係数
Try
	'配列の確保
	m_Tan = Nothing
	Array.Resize(dTimeAry, p_dtFile.iHosei.Length \ iThining )
	Array.Resize(dData, p_dtFile.iHosei.Length \ iThining )
	Array.Resize(m_Tan, p_dtFile.iHosei.Length \ iThining )
	''補正データ変換係数の計算
	''Select Case iStrsUnit
	''Case 0	'dig
	''	dCoef = 1.0
	''Case 1	'%
	''	dCoef = 100.0 / dFullMax
	''Case 2	'N
	''	dCoef = 1.0
	''End Select

	'間引き処理 時刻配列の作成
	Dim j As int32
	j = 0
	For i = 0 To p_dtFile.iHosei.GetUpperBound(0) Step iThining
		dTimeAry(j) = i * MEAS_TIME_RESOLUTION / MEAS_TIME_BLOCK		'タイムテーブル
		dData(j) = CType(p_dtFile.iHosei(i)* dCoef, Double)				'データテーブル
		j += 1
	Next
	MakeTangentMatrix(dTimeAry, dData, m_Tan, iTanSamp)			'接線パラメータ表の作成

Catch ex As Exception
End Try
End Sub
''' <summary>
''' ２本の接線交点を求め、そのゲルタイム時間を表示する
''' </summary>
''' <param name="oChart">チャート</param>
Public Sub GelTimeCrossLineDisp(ByRef oChart As Chart)
	Dim Data(,) As Double ={{0,0},{0,0},{0,0}}
	Dim Back(Data.GetLength(0)-1,Data.GetLength(1)-1) As Double
	Dim SolX As Double = Double.NaN		'解X
	Dim SolY As Double = Double.NaN		'解Y
	Dim bRet As Boolean
Try
	If IsNothing(m_Tan)
		Return
	End If
	oChart.ChartAreas(0).AxisX.StripLines.Clear()
	oChart.ChartAreas(0).AxisY.StripLines.Clear()

	'開始交点座標を求める ---------------------------------------------
	'2線のパラメータをセット
	'下ベースライン式
	Data(0, 0) = -m_Tan(iStartSelIdx).a
	Data(1, 0) = 1.0
	Data(2, 0) = m_Tan(iStartSelIdx).b
	'中点ライン式
	Data(0, 1) = -m_Tan(iMiddlSelIdx).a
	Data(1, 1) = 1.0
	Data(2, 1) = m_Tan(iMiddlSelIdx).b
	Array.Copy(Data, Back, Data.Length)		'元データをバックアップ
	bRet = GaussJordan(Data, 2)				'連立方程式を解く
	If bRet
		dLowerGelTX = Data(Data.GetUpperBound(0), 0)
		dLowerGelTY = Data(Data.GetUpperBound(0), 1)
	Else
		dLowerGelTX = Double.NaN
		dLowerGelTY = Double.NaN
	End If
	Array.Copy(Back, Data, Back.Length)		'元データを復帰

	Dim Time_EXPL As New StripLine()
	Dim dLGelTimeG As Double	= dLowerGelTX / dRate
	Time_EXPL.Font				= New Font(BASE_FONT, 12, FontStyle.Regular)
    Time_EXPL.Text				= $"GelTime: {dLowerGelTX.ToString("F2")} Sec"
    Time_EXPL.ForeColor			= Color.Blue
    oChart.BackColor			= Color.White
	Time_EXPL.TextAlignment		= StringAlignment.Far
	Time_EXPL.Interval			= 0
	Time_EXPL.IntervalOffset	= dLGelTimeG
	Time_EXPL.BorderWidth		= 0.8
	Time_EXPL.BorderDashStyle	= ChartDashStyle.DashDot
	Time_EXPL.BorderColor		= Color.Red
	oChart.ChartAreas(0).AxisX.StripLines.Add(Time_EXPL)

	'終了交点 ----------------------------------------------------
	'2線のパラメータをセット
	'上ベースライン式
	Data(0, 0) = -m_Tan(iFinalSelIdx).a
	Data(1, 0) = 1.0
	Data(2, 0) = m_Tan(iFinalSelIdx).b
	'中点ライン式
	Data(0, 1) = -m_Tan(iMiddlSelIdx).a
	Data(1, 1) = 1.0
	Data(2, 1) = m_Tan(iMiddlSelIdx).b
	Array.Copy(Data, Back, Data.Length)		'元データをバックアップ
	bRet = GaussJordan(Data, 2)				'連立方程式を解く
	If bRet
		dUpperGelTX = Data(Data.GetUpperBound(0), 0)
		dUpperGelTY = Data(Data.GetUpperBound(0), 1)
	Else
		dUpperGelTX = Double.NaN
		dUpperGelTY = Double.NaN
	End If
	Array.Copy(Back, Data, Back.Length)		'元データを復帰

	Dim Time_EXPH As New StripLine()
	Dim dHGelTimeG As Double	= dUpperGelTX / dRate
	Time_EXPH.Font				= New Font(BASE_FONT, 12, FontStyle.Regular)
    Time_EXPH.Text				= $"GelTime: {dUpperGelTX.ToString("F2")} Sec"
    Time_EXPH.ForeColor			= Color.Blue
    oChart.BackColor			= Color.White
	Time_EXPH.TextAlignment		= StringAlignment.Far
	Time_EXPH.Interval			= 0
	Time_EXPH.IntervalOffset	= dHGelTimeG
	Time_EXPH.BorderWidth		= 0.8
	Time_EXPH.BorderDashStyle	= ChartDashStyle.DashDot
	Time_EXPH.BorderColor		= Color.Red
	oChart.ChartAreas(0).AxisX.StripLines.Add(Time_EXPH)

	'中点座標を求める --------------------------------------------------
	dMiddlGelTX = (dLowerGelTX + dUpperGelTX) / 2
	dMiddlGelTY	= (dLowerGelTX + dUpperGelTX) / 2
	Dim Time_EXPM As New StripLine()
	Dim dMGelTimeG As Double	= dMiddlGelTX / dRate
	Time_EXPM.Font				= New Font(BASE_FONT, 12, FontStyle.Regular)
    Time_EXPM.Text				= $"GelTime: {dMiddlGelTX.ToString("F2")} Sec"
    Time_EXPM.ForeColor			= Color.Blue
    oChart.BackColor			= Color.White
	Time_EXPM.TextAlignment		= StringAlignment.Far
	Time_EXPM.Interval			= 0
	Time_EXPM.IntervalOffset	= dMGelTimeG
	Time_EXPM.BorderWidth		= 0.8
	Time_EXPM.BorderDashStyle	= ChartDashStyle.DashDot
	Time_EXPM.BorderColor		= Color.Red
	oChart.ChartAreas(0).AxisX.StripLines.Add(Time_EXPM)

Catch ex As Exception

End Try
End Sub


''' <summary>
''' 開始、中点、終了位置縦カーソル表示
''' </summary>
''' <param name="oC"></param>
''' <param name="dX"></param>
Public Sub ExtrapoCursorDisp(ByRef oC As Chart, ByRef ano As LineAnnotation, ByVal iType As Int16, ByVal dX As Double)
Dim ptStart As New point
Dim ptEnd	As New point
Try
With oC
    ptStart.X = dX / dRate
    ptStart.Y = 0
    ptEnd.X = ptStart.X
    ptEnd.Y = .ChartAreas(0).AxisY.Maximum

	'注釈初期設定
	ano.AxisX = .ChartAreas(0).AxisX
	ano.AxisY = .ChartAreas(0).AxisY
	ano.IsSizeAlwaysRelative = False
	ano.AllowSelecting = False
	ano.AllowMoving = False
	ano.LineColor = Color.Green
	ano.LineWidth = 1
	ano.LineDashStyle = ChartDashStyle.DashDot
	ano.Visible = True
	ano.AnchorY = .ChartAreas(0).AxisY.Maximum
	Select Case iType
	Case 0
		ano.Name = "S"
	Case 1
		ano.Name = "M"
	Case 2
		ano.Name = "F"
	End Select

	'注釈位置設定
	ano.X		= ptStart.X
	ano.Y		= ptStart.Y
	ano.Height	= ptEnd.Y - ptStart.Y
	ano.Width	= ptEnd.X - ptStart.X
	.Annotations.Add(ano)					'注釈を追加
	'.UpdateAnnotations()					'注釈更新

End With
Catch ex As Exception

End Try
End Sub
''' <summary>
''' 接線 y=ax+b直線をchartコントロール上へ描画(LineAnnotation使用)
''' </summary>
''' <param name="oChart"></param>
''' <param name="a"></param>
''' <param name="b"></param>
Public Sub DrawStraightLineInChart(Byref oChart As Chart, Byval a As Double, ByVal b As Double )
Dim ptStart As New point
Dim ptEnd	As New point
Dim ano		As New LineAnnotation
Try
	With oChart
#If 1
	'始点の直線座標を計算
	ptStart.X	= 0
	ptStart.Y	= b
	'始点描画範囲調整
	If ptStart.Y<.ChartAreas(0).AxisY.Minimum
		ptStart.Y	= 0
		ptStart.X	= ((ptStart.Y - b) / a) / dRate
	End If
	If ptStart.Y>.ChartAreas(0).AxisY.Maximum
		ptStart.Y	= .ChartAreas(0).AxisY.Maximum
		ptStart.X	= ((ptStart.Y - b) / a) / dRate
	End If
	'終点の直線座標を計算
	ptEnd.X		=  .ChartAreas(0).AxisX.Maximum
	ptEnd.Y		=  (ptEnd.X * dRate) * a + b

	'終点描画範囲調整
	If ptEnd.Y < .ChartAreas(0).AxisY.Minimum
		ptEnd.Y		= 0
		ptEnd.X		= (ptEnd.Y - b) / a / dRate
	End If
	If ptEnd.Y > .ChartAreas(0).AxisY.Maximum
		ptEnd.X		= ((.ChartAreas(0).AxisY.Maximum - b) / a) / dRate
		ptEnd.Y		= .ChartAreas(0).AxisY.Maximum
	End If
#else
	ptStart.X	= 0
	ptStart.Y	= b
	ptEnd.X		=  .ChartAreas(0).AxisX.Maximum
	ptEnd.Y		=  (ptEnd.X * 0.2) * a + b
#end if
	'注釈初期設定
	ano.AxisX = .ChartAreas(0).AxisX
	ano.AxisY = .ChartAreas(0).AxisY
	ano.IsSizeAlwaysRelative = False
	ano.AllowSelecting = False
	ano.AllowMoving = False
	ano.LineColor = Color.Blue
	ano.LineWidth = 1
	ano.LineDashStyle = ChartDashStyle.dot
	ano.Visible = True

	'注釈位置設定
	ano.X		= ptStart.X
	ano.Y		= ptStart.Y
	ano.Height	= ptEnd.Y - ptStart.Y
	ano.Width	= ptEnd.X - ptStart.X
	.Annotations.Add(ano)					'注釈を追加
	'.UpdateAnnotations()					'注釈更新
	End With
Catch ex As Exception
    Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub

''' <summary>
''' 接線パラメータ表を作成する
''' </summary>
''' <param name="aryTim">時間配列</param>
''' <param name="aryData">データ配列</param>
''' <param name="mTan">接線パラメータ表</param>
''' <param name="iSamp">最小二乗法サンプル数</param>
Public Sub MakeTangentMatrix(ByVal aryTim() As Double, ByVal aryData() As Double, ByRef mTan() As stTangentMatrix, ByVal iSamp As int32 )
Dim i As int32
Dim j As int32
Dim dX()	As Double = Nothing				'変換前
Dim dY()	As Double = Nothing
Dim dNX()	As Double = Nothing				'変換後
Dim dNY()	As Double = Nothing
Dim dRet()  As Double = Nothing

Try
	If IsNothing(aryData)
		Return
	End If
	Array.Resize(dX,iSamp)
	Array.Resize(dY,iSamp)
	For i = 0 To aryData.GetUpperBound(0) Step 1
		If i > iSamp And i < aryData.GetUpperBound(0) - iSamp  
			Array.Copy(aryTim, i, dX, 0, iSamp)
			Array.Copy(aryData, i, dY, 0, iSamp)
			dRet = datCal.fnLeastSquaresMethod(dX, dY, dNX, dNY, iSamp, 1)			'最小二乗法
			j = i + iSamp / 2
			mTan(j).a = dRet(1)			'傾き
			mTan(j).b = dRet(0)			'Y切片
			'Array.Resize(dX,iSamp)
			'Array.Resize(dY,iSamp)
		End If
	Next

Catch ex As Exception
    Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub

''' <summary>
''' GaussJordan  ガウス－ジョルダン法で連立方程式を解く
''' </summary>
''' <param name="value">2次元の配列(最初の次元が横、次が縦の係数)</param>
''' <param name="count">？元連立方程式であるか（？行？列の正方行列）</param>
''' <returns>方程式が解ければTrueを返し、そうでなければFalseを返す(0除算)</returns>
Public Function GaussJordan(ByRef Value(,) As Double, count As Long) As Boolean
    Dim a As Double          '注目式の未知数の係数
    Dim i As Long, j As Long '縦の係数操作のためのカウンタ
    Dim k As Long            '横の係数操作のためのカウンタ
    Dim temp As Double       '注目式以外の未知数の係数
Try
    '1つ目の未知数から、順に処理していく(上から下へ)
    For i = 0& To count - 1&
        
        '注目式の未知数の係数を1にするためにその係数を格納
        a = value(i, i)
        
        '0除算、オーバーフロー除け
        If Math.Abs(a) < 0.0001 Then GaussJordan = False: Exit Function
        
        '注目式の未知数の係数を1にするため注目式全体をaで割る
        For k = i To count
            value(k, i) = value(k, i) / a
        Next k
        
        '注目式以外の未知数の係数を0にする
        For j = 0& To count - 1&
            '注目式以外なら実行する
            If j <> i Then
                '注目式以外の未知数の係数を格納
                temp = value(i, j)
                'その値を0にするため、注目式との演算を行う
                For k = i To count
                    value(k, j) = value(k, j) - temp * value(k, i)
                Next k
            End If
        Next j
    Next i

    Return True

Catch ex As Exception
    Return False
End Try
End Function
'開始・中間・終了点のカーソル表示
Public Sub ExtrapoCursorAllDisp(ByRef oChart As Chart)
	ExtrapoCursorDisp(oChart, anoSet1, 0, dStartTime)
	ExtrapoCursorDisp(oChart, anoSet2, 1, dMidTime)
	ExtrapoCursorDisp(oChart, anoSet3, 2, dEndTime)
End Sub
'開始点(下ベースライン)の接線を引く
Public Sub StartLineDisp(ByRef oChart As Chart)
	Dim i As Int32
	Dim j As Int32
Try
	If IsNothing(m_Tan)
		Return
	End If
	i = (dStartTime / (MEAS_TIME_RESOLUTION / MEAS_TIME_BLOCK)) \ iThining
	If i > m_Tan.GetUpperBound(0)
		'最初の接線を探す
		For j = 0 To  m_Tan.GetUpperBound(0)
			If m_Tan(j).a <> 0
				i = j
				Exit For
			End If
		Next j
	End If
	DrawStraightLineInChart(oChart, m_Tan(i).a, m_Tan(i).b)			'y=aX+b直線を描画
	dA1 = m_Tan(i).a
	iStartSelIdx = i
Catch ex As Exception
End Try
End Sub
'中間点(中間ライン)の接線を引く
Public Sub MiddleLineDisp(ByRef oChart As Chart)
	Dim i As Int32
	Dim j As Int32
Try
	If IsNothing(m_Tan)
		Return
	End If
	i = (dMidTime / (MEAS_TIME_RESOLUTION / MEAS_TIME_BLOCK)) \ iThining
	If i > m_Tan.GetUpperBound(0)
		'傾きの最大値を探す
		Dim dBuf As Double = Double.MinValue
		Dim iMaxIdx As Int32 = 0
		For j = 0 To  m_Tan.GetUpperBound(0)
			If dBuf < m_Tan(j).a
				dBuf = m_Tan(j).a
				iMaxIdx = j
			End If
		Next j
		i = iMaxIdx
	End If
	DrawStraightLineInChart(oChart, m_Tan(i).a, m_Tan(i).b)			'y=aX+b直線を描画
	dA2 = m_Tan(i).a
	iMiddlSelIdx = i
Catch ex As Exception
End Try
End Sub
'終了点(下ベースライン)の接線を引く
Public Sub EndLineDisp(ByRef oChart As Chart)
	Dim i As Int32
Try
	If IsNothing(m_Tan)
		Return
	End If
	i = (dEndTime / (MEAS_TIME_RESOLUTION / MEAS_TIME_BLOCK)) \ iThining
	If i > m_Tan.GetUpperBound(0)
		'最後の接線を探す
		For j = m_Tan.GetUpperBound(0) To 0 Step -1
			If m_Tan(j).a <> 0
				i = j
				Exit For
			End If
		Next j
	End If
	DrawStraightLineInChart(oChart, m_Tan(i).a, m_Tan(i).b)			'y=aX+b直線を描画
	dA3 = m_Tan(i).a
	iFinalSelIdx = i
Catch ex As Exception
End Try
End Sub
''' <summary>
''' パラメータ変更処理
''' </summary>
''' <param name="oChart">チャート</param>
''' <param name="NudName">サンプル数Or間引きが変更？</param>
''' <param name="dVal">サンプル数Or間引き</param>
''' <param name="dVal1">開始点</param>
''' <param name="dVal2">中間点</param>
''' <param name="dVal3">終了点</param>
Public Sub PointValueChange(ByRef oChart As Chart, ByVal NudName As String, ByVal dMax As Double, ByVal dVal As Double, ByVal dVal1 As Double, ByVal dVal2 As Double, ByVal dVal3 As Double)
	If Not MatrixEnable
		Return
	End If
	If IsNothing(p_dtFile.iHosei)
		Return
	End if
	Select Case NudName
	Case "nudExpoSample"
		TangMakeSampleCount = dVal
	Case "nudThinning"
		Thining = dVal
	End select
	CreateTanMatrix()				'接線パラメータ表の生成
	oChart.Annotations.Clear
	StartTime	= dVal1		'秒
	MidTime		= dVal2		'秒
	EndTime		= dVal3		'秒
	StartLineDisp(oChart)				'開始点(下ベースライン)の接線を引く
	MiddleLineDisp(oChart)				'中間点(中間ライン)の接線を引く
	EndLineDisp(oChart)					'終了点(上ベースライン)の接線を引く
	ExtrapoCursorAllDisp(oChart)		'開始・中間・終了点のカーソル表示
	GelTimeCrossLineDisp(oChart)		'ゲルタイム表示

End Sub

''' <summary>
''' マウスダウン処理
''' </summary>
''' <param name="oChart"></param>
''' <param name="e"></param>
Public Sub	MouseDownProc(ByRef oChart As Chart, ByVal e As  MouseEventArgs)
	if Double.IsNaN(dMouseDownVal)
	    dMouseDownVal = oChart.ChartAreas(0).AxisX.PixelPositionToValue(e.X) * TimeToSampleRate
        '始点
		Select Case dMouseDownVal
		Case StartTime - iMouseRange To StartTime + iMouseRange
			iSelPoint = 0
		Case MidTime - iMouseRange To MidTime + iMouseRange
			iSelPoint = 1
		Case EndTime - iMouseRange To EndTime + iMouseRange
			iSelPoint = 2
		Case Else
			iSelPoint = -1
			dMouseDownVal = Double.NaN
		End Select
	End If
End Sub
''' <summary>
''' マウスムーブ処理
''' </summary>
''' <param name="oChart"></param>
''' <param name="e"></param>
''' <param name="dVal"></param>
Public Sub MouseMoveProc(ByRef oChart As Chart, ByVal e As  MouseEventArgs, ByRef dVal As Double)
Dim dX As Double
	If IsNothing(p_dtFile.iHosei)
		Return
	End if
	dX = oChart.ChartAreas(0).AxisX.PixelPositionToValue(e.X) * TimeToSampleRate
	Select Case dX
	Case StartTime - iMouseRange To StartTime + iMouseRange
		oChart.Cursor = Cursors.SizeWE
	Case MidTime - iMouseRange To MidTime + iMouseRange
		oChart.Cursor = Cursors.SizeWE
	Case EndTime - iMouseRange To EndTime + iMouseRange
		oChart.Cursor = Cursors.SizeWE
	Case Else
		oChart.Cursor = Cursors.Cross
	End Select
	If iSelPoint<>-1
		Select Case iSelPoint
		Case 0	'開始点
			If dX < dMidTime
				dVal = dX
			Else
				dVal = dMidTime
			End If
		Case 1	'中間点
			If dX > dStartTime AndAlso dX < dEndTime
				dVal = dX
			Else
				dVal = (dStartTime + dEndTime) / 2
			End If
		Case 2	'終了点
			If dX > dMidTime
				dVal = dX
			Else
				dVal = dMidTime
			End If
		Case Else
		End Select
	End If
End Sub
''' <summary>
''' マウスアップ処理
''' </summary>
''' <param name="oChart"></param>
''' <param name="e"></param>
''' <param name="dVal"></param>
Public Sub MouseUpProc(ByRef oChart As Chart, ByVal e As  MouseEventArgs, ByRef dVal As Double)
	dMouseUpVal = oChart.ChartAreas(0).AxisX.PixelPositionToValue(e.X) * TimeToSampleRate
	If iSelPoint <> -1
		dVal		= dMouseUpVal
	End If
	dMouseDownVal	= Double.NaN
	dMouseUpVal		= Double.NaN
	iSelPoint = -1
	oChart.Cursor = Cursors.Default

End Sub
''' <summary>
''' 外挿法 ヘッダ情報セット
''' </summary>
''' <param name="head"></param>
Public Sub FileHeaderSet(ByRef head As DataFileHeader)
	head.sExtStartTime		= StartTime.ToString("F3")		'接線開始点
	head.sExtmiddleTime		= MidTime.ToString("F3")		'接線中点
	head.sExtEndTime		= EndTime.ToString("F3")		'接線終了点

	head.sExtTanSample		= TangMakeSampleCount.ToString("F3")
	head.sExtTanThinning	= Thining.ToString("F3")

	head.sExtGelTimeSelect	= GelTSelect					'ゲルタイム選択(Under/Middle/Over)
	head.sExtGelTimeUnder	= LowerGelT.ToString("F2")
	head.sExtGelTimeMiddle	= MiddlGelT.ToString("F2")
	head.sExtGelTimeOver	= UpperGelT.ToString("F2")

	'確定されたゲルタイムをセット
	Select Case GelTSelect
	Case lstGelTimeSel(0)
		head.dGelTime		= Ctype(head.sExtGelTimeUnder,Double)
	Case lstGelTimeSel(1)
		head.dGelTime		= Ctype(head.sExtGelTimeMiddle,Double)
	Case lstGelTimeSel(2)
		head.dGelTime		= Ctype(head.sExtGelTimeOver,Double)
	End Select
End Sub



End Class
