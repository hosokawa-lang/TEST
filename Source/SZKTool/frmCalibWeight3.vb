Imports System.Drawing.Printing
Imports System.Threading
Imports System.IO
Imports System.Text
Imports System.Windows.Forms.DataVisualization.Charting
Imports Microsoft.VisualBasic.FileIO
Imports System.Collections
Imports System.Globalization
Imports SZKTool.frmOptions
Imports SZKTool.RealTimeMeas
Imports SZKTool.modPublic
Imports System.ComponentModel
Imports System.Runtime.CompilerServices
Imports System.Reflection
Imports SZKTool.DataFile

'重量キャリブレーション画面処理
Public Class frmCalibWeight3
Const cMeasCount					As Int16 = 12				'測定回数
Const cAry							As Int16 = cMeasCount - 1
Private p_MeasState                  As Int16 = -1               '-1:未開始 1:X実施中 2:Y実施中 
Private p_MeasStateZ                 As Int16 = -1               '-1:未開始 1:X実施中 2:Y実施中 
Private p_Extract					As Int16 = -1				'-1:未開始 1:実施中
Private p_ExtractZ					As Int16 = -1				'-1:未開始 1:実施中

Private	ds					        As New DataSet			    'データセット
Private	dtStrees			        As New DataTable		    '応力モニタ用データテーブル Table(0)
Private	dtResult			        As New DataTable		    '測定結果用データテーブル	Table(1)
Private	dtVector			        As New DataTable		    'ベクトル表示データテーブル	Table(2)
Private	dtStreesZ			        As New DataTable		    '応力モニタ用データテーブル Table(3)
Private dtRow	                    As DataRow                  'ワーク用データテーブル行

Private dMeasWT(cAry)					As Double					'測定データ 重量
Private dX(cAry)						As Double					'測定データ
Private dY(cAry)						As Double					'測定データ
Private dXY(cAry)						As Double					'測定データ(大きさ)
Private dXh(cAry)						As Double					'測定データ(比率掛け)
Private dYh(cAry)						As Double					'測定データ(比率掛け)
Private dYy(cAry)						As Double					'Y方向ベクトル Y軸成分
Private dYx(cAry) 						As Double					'Y方向ベクトル X軸成分
Private dXx(cAry) 						As Double					'X方向ベクトル

Private dMeasWTZ					As Double					'Z測定データ 重量
Private dZ							As Double					'Z測定データ

Private dAlpha(cAry)					As Double					'Y軸の角度（X軸に対して）
Private dVectorF(cAry)					As Double					'大きさ
Private dVectorTheta(cAry)				As Double					'方向角度
Private dRatio(cAry)					As Double					'レベル比率(Xと同レベルにするための)
Private dRatioX(cAry)					As Double					'レベル比率(Xと同レベルにするための)
Private dRatioY(cAry)					As Double					'レベル比率(Xと同レベルにするための)

Private dCoef						As Double = Double.NaN		'重量N変換係数 (Dig->N)
Private dCoefZ						As Double = Double.NaN		'重量g変換係数 (Dig->g)

Private m_bYSpanAuto				As Boolean = True			'Y軸スパン自動設定
Private m_bYSpanAutoZ				As Boolean = True			'Y軸スパン自動設定
Private m_StressArrayX()			As int64 = Nothing			'平均算出用配列
Private m_StressArrayY()			As int64 = Nothing			'平均算出用配列
Private m_StressArrayZ()			As int64 = Nothing			'平均算出用配列
Private	m_Result					As Int16 = -1				'1:結果更新完了 -1:結果更新待ち

'ベクトルモニタ関係
Private dX1							As Double = Double.NaN		'終点座標
Private dY1							As Double = Double.NaN		'終点座標
Private m_WeightMonitor				As Double = Double.NaN		'重量モニタ値(gf)
Private m_DirectMonitor				As Double = Double.NaN		'方向モニタ値(degree)
'測定結果テーブルフィール配列
'Public sItemStr As String = "Weight,StressX,StressY,Alpha,Vector F,Vector Theta,XY Ratio"
Public sItemStr As String = "No,Degree,Weight,ADX,ADY,Magnitude,Coefficient"
Public sResultTblField() As String = {sItemStr}
Public Enum eXYCol
	iNo = 0
	iDeg
	iWeight
	iADX
	iADY
	iMag
	iCoef
End Enum

'多言語メッセージ格納
Private m_sStr(eMes.iMAX - 1)		As String   
Enum eMes
	ItemStrDgvRst = 0	'重量,応力X,応力Y,α,ベクトルF,ベクトルθ,XYレシオ
	STRESS_MON_TITLE	'応力モニタ
	STRESS_MON_X		'時間[秒]
	STRESS_MON_XST		'X応力[Dig]
	STRESS_MON_YST		'Y応力[Dig]
	STRESS_MON_ZST		'Z応力[Dig]
	VECTOR_MON_TITLE	'ベクトルモニタ
	VECTOR_MON_YST		'Y軸
	VECTOR_MON_XST		'X軸
	iMAX
End Enum
Sub New

    ' この呼び出しはデザイナーで必要です。
    InitializeComponent()

    ' InitializeComponent() 呼び出しの後で初期化を追加します。

	InitialCalibFull()
	sResultTblField = sItemStr.Split(",")


End Sub
Private Sub frmCalibWeight_Load(sender As Object, e As EventArgs) Handles MyBase.Load
	Dim i As int32
	'言語切替
	Call SetFormLanguage(Me, p_iLanguage, ,m_sStr)

	'tbcMode.TabPages(0).Text = "X/Y Stress"
	'tbcMode.TabPages(1).Text = "Z Stress"
	tbcMode.SelectedIndex = 0

	If ds.Tables.Count <= 0
		'XYデータセットのモニタデータ用テーブル
		dtStrees.TableName = "StressMonitor"
		dtStrees.Columns.Add("Sec",			Type.GetType("System.Double"))
		dtStrees.Columns.Add("XADValue",	Type.GetType("System.Int64"))
		dtStrees.Columns.Add("YADValue",	Type.GetType("System.Int64"))
		ds.Tables.Add(dtStrees)

		'データセットの重量校正用Xテーブル
		dtResult.TableName = "Result"
		For i=0 To sResultTblField.GetUpperBound(0)
			dtResult.Columns.Add(sResultTblField(i),	Type.GetType("System.Double"))
		Next
		ds.Tables.Add(dtResult)

		'データセットのベクトル表示用テーブル
		dtVector.TableName = "Vector"
		dtVector.Columns.Add("X", Type.GetType("System.Double"))
		dtVector.Columns.Add("Y", Type.GetType("System.Double"))
		ds.Tables.Add(dtVector)

		'Zデータセットのモニタデータ用テーブル
		dtStreesZ.TableName = "StressMonitorZ"
		dtStreesZ.Columns.Add("SecZ",		Type.GetType("System.Double"))
		dtStreesZ.Columns.Add("ZADValue",	Type.GetType("System.Int64"))
		ds.Tables.Add(dtStreesZ)

	End if

	CsvToDataTable(p_SetPath, p_sStoreFileNameWeight, dtResult)		'ファイルから読込
	WeightCalibDgvDisp( dtResult )									'重量校正結果リスト表示
	Dim d(1) As Double
	Dim s(1) As String
	CsvGetCoef(p_SetPath, p_sStoreFileNameWeight, d, s)
	dCoef	= d(0)
	dCoefZ	= d(1)
	p_XYCoefMeasDate	= s(0)
	p_ZCoefMeasDate		= s(1)
	Select Case tbcMode.SelectedIndex
	Case 0
	'XY Stress
		'パラメータ表示 -------------------------------------
		VectorDataTableInit()											'ベクトルチャート用データテーブル初期化
		p_MeasState = 0
		m_ChartADUpdate = True
		m_ChartVectorUpdate = True
		lblXYMeasDateVal.Text = p_XYCoefMeasDate
	Case 1
	'Z Stress
		p_MeasStateZ = 0
		m_ChartADUpdate = True
		lblZMeasDateVal.Text = p_ZCoefMeasDate
	End Select

	Timer1.Interval = 200
	Timer1.Enabled = True

End Sub
#If 0
''' <summary>
''' 重量変換係数の取得
''' </summary>
''' <param name="dtResult">データテーブル</param>
''' <returns>変換係数  NGの場合はDouble.NaN</returns>
Public Function GetWeightCoef(Byref dtResult As Datatable) As Double
Dim dData As Double = Double.NaN
	Dim dWeight As Double = dtResult.Rows(2).Item(1)		'重量[g]
	Dim dF		As Double = dtResult.Rows(2).Item(4)		'力[dig]
	dData = (dWeight/1000 * cGraviAcc) / dF					'N/dig
	ddata = if(Double.IsInfinity(dData), Double.NaN, dData)
	Return dData
End Function
#end if
'Vectorデータテーブル初期化
Private Sub VectorDataTableInit()
	Dim i As Int32
	Dim dt		As DataTable = dtVector
	Dim dtRow	As DataRow
	dt.Rows.Clear()
	For i = 0 To 1
		dtRow = dt.NewRow
		dtRow(0)	= 0.0
		dtRow(1)	= 0.0
		dt.Rows.Add(dtRow)
	Next i
End Sub
'Vectorデータテーブル更新
Private Sub VectorDataTableUpdate()
	dtVector.Rows(1).Item(0) = p_RTM(0).p_mltcp.CurrStrsX()
	dtVector.Rows(1).Item(1) = p_RTM(0).p_mltcp.CurrStrsY()
End Sub
'重量・方向モニタ値計算
Private Sub	VectorWeightDirectCalc()
	Dim dXY As Double
	dXY =  Math.Sqrt(p_RTM(0).p_mltcp.CurrStrsX()^2 + p_RTM(0).p_mltcp.CurrStrsY()^2)
	m_WeightMonitor = dXY * 1000 * dCoef / cGraviAcc								'Digit * 1000[g] * Coef[N/dig] / 重力加速度
	Dim	dAsinData As Double = p_RTM(0).p_mltcp.CurrStrsY() / dXY
	Dim	dCosData As Double = p_RTM(0).p_mltcp.CurrStrsX() / dXY
	If p_RTM(0).p_mltcp.CurrStrsX()>=0 AndAlso p_RTM(0).p_mltcp.CurrStrsY()>=0
		m_DirectMonitor = Math.Asin(dAsinData) * RadToDegRate						'θ Degree
	ElseIf p_RTM(0).p_mltcp.CurrStrsX()<0 AndAlso p_RTM(0).p_mltcp.CurrStrsY()>=0
		m_DirectMonitor = Math.Acos(dCosData) * RadToDegRate						'θ Degree
	ElseIf p_RTM(0).p_mltcp.CurrStrsX()<0 AndAlso p_RTM(0).p_mltcp.CurrStrsY()<0
		m_DirectMonitor = -Math.Asin(dAsinData) * RadToDegRate + 180				'θ Degree
	ElseIf p_RTM(0).p_mltcp.CurrStrsX()>=0 AndAlso p_RTM(0).p_mltcp.CurrStrsY()<0
		m_DirectMonitor = Math.Asin(dAsinData) * RadToDegRate + 360					'θ Degree
	End If
End Sub			

Private m_Job           As Int16 = -1
Private m_GetCount      As Uint32
Private m_SW1			As New Stopwatch
Private m_SW2			As New Stopwatch
Private m_ChartADUpdate		As Boolean
Private m_ChartVectorUpdate	As Boolean
'タイマーインターバル
Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
Try
	Select Case tbcMode.SelectedIndex
	Case 0
		TimerProcXY()
	Case 1
		TimerProcZ()
	End Select
Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub
Private Sub TimerProcXY
Try
    DataTabeleSet()						'モニタ用データ取得シーケンス処理
	If m_ChartADUpdate 
	'チャート表示初期化
		WeightMonitorChartDisp( _			'応力モニタ表示更新
			ChartAD, _
			ds, _
			nudTimeSpan.Value, _
			nudYSpanMin.Value, _
			nudYSpanMax.Value, _
			m_bYSpanAuto _
		)  
	Else
	'表示更新
		'X軸、Y軸スパン更新
		WeightMonitorChartMinMax( ChartAD, nudTimeSpan.Value, nudYSpanMin.Value, nudYSpanMax.Value, m_bYSpanAuto )		
		ChartAD.DataBind()
		ChartAD.Invalidate()
	End if
	If m_Result = 1
	'抽出結果処理完了
		WeightCalibDgvDisp( dt:=ds.Tables(1) )	'重量校正結果リスト表示
		m_Result = -1
	End if

	VectorDataTableUpdate()				'ベクトルデータテーブル更新
	If m_ChartVectorUpdate 
	'チャート表示初期化
		VectorChartDisp( _					'リアルタイムベクトルチャート表示
			ChartVector, _
			ds, _
			nudTimeSpan.Value, _
			nudYSpanMin.Value, _
			nudYSpanMax.Value, _
			m_bYSpanAuto _
		)
	Else
	'表示更新
		'X軸、Y軸スパン更新
		VectorChartMinMax( nudTimeSpan.Value, nudYSpanMin.Value, nudYSpanMax.Value, m_bYSpanAuto )		
		ChartVector.DataBind()
		ChartVector.Invalidate()
	End if
	VectorWeightDirectCalc()			'重量・方向モニタ値計算



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
	    lblCurrentADValX.Text =  p_RTM(0).p_mltcp.CurrStrsX()
	    lblCurrentADValY.Text =  p_RTM(0).p_mltcp.CurrStrsY()
    End If
	lblCurrentADValLiveX.Text =  p_RTM(0).p_mltcp.CurrStrsLiveX()
	lblCurrentADValLiveY.Text =  p_RTM(0).p_mltcp.CurrStrsLiveY()
	'換算係数
	If Double.IsNaN(dCoef)
		lblCoefValue.Text = "N A"
	Else
		lblCoefValue.Text = dCoef.ToString("E6")
	End If
	'重量モニタ(gf)
	If Double.IsNaN(m_WeightMonitor)
		lblRealWeight.Text = "N A"
	Else
		lblRealWeight.Text = m_WeightMonitor.ToString("F1")
	End If
	'方向モニタ(deg)
	If Double.IsNaN(m_DirectMonitor)
		lblRealDirect.Text = "N A"
	Else
		lblRealDirect.Text = m_DirectMonitor.ToString("F1")
	End If
Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub
Private Sub TimerProcZ
Try
    DataTabeleSetZ()						'モニタ用データ取得シーケンス処理
	If m_ChartADUpdate 
	'チャート表示初期化
		WeightMonitorChartDisp( _			'応力モニタ表示更新
			ChartADZ, _
			ds, _
			nudTimeSpanZ.Value, _
			nudYSpanMinZ.Value, _
			nudYSpanMaxZ.Value, _
			m_bYSpanAutoZ _
		)  
	Else
	'表示更新
		'X軸、Y軸スパン更新
		WeightMonitorChartMinMax(ChartADZ, nudTimeSpanZ.Value, nudYSpanMinZ.Value, nudYSpanMaxZ.Value, m_bYSpanAutoZ )		
		ChartADZ.DataBind()
		ChartADZ.Invalidate()
	End if
	If m_Result = 1
	'抽出結果処理完了
		dCoefZ = dMeasWTZ /dZ
		dCoefZ = If(Double.IsInfinity(dCoefZ),Double.NaN,dCoefZ)
		m_Result = 0
	End if


	'抽出ボタン表示
	If p_ExtractZ = 1
        btnExtractZ.BackColor = Color.Orange
	Else
        btnExtractZ.BackColor = Control.DefaultBackColor
	End if
	'AUTOボタン表示
	If m_bYSpanAutoZ
		btnYSpanAutoZ.BackColor = Color.Orange
	Else
		btnYSpanAutoZ.BackColor = Control.DefaultBackColor
	End If
	'抽出ボタン有効無効
	if p_MeasStateZ = -1 Or p_ExtractZ = 1
		btnExtractZ.Enabled = False
	Else
		btnExtractZ.Enabled = True
	End If
	'重量入力有効無効
	if p_ExtractZ = 1
		nudWeightZ.Enabled = False
	Else
		nudWeightZ.Enabled = True
	End If
    '現在応力値
    If p_MeasStateZ = -1
        lblCurrentADValZ.Text		= "N A"
        lblCurrentGramValZ.Text		= "N A"
    Else
	    lblCurrentADValZ.Text		=  p_RTM(0).p_mltcp.CurrStrsZCorrA() - m_ZeroSetMomo
	    lblCurrentGramValZ.Text		=  ((p_RTM(0).p_mltcp.CurrStrsZCorrA() - m_ZeroSetMomo) * dCoefZ).ToString("F1") & Space(0) & "g"
    End If
	lblCurrentADValLiveZ.Text		= p_RTM(0).p_mltcp.CurrStrsZCorrA()		'[1181] Z応力AD値
	'換算係数
	If Double.IsNaN(dCoef)
		lblCoefValueZ.Text = "N A"
	Else
		lblCoefValueZ.Text = dCoefZ.ToString("E6")
	End If
	'仮入力 Z応力補正A
	''p_RTM(0).p_mltcp.CorrStrsZ_A(1) = valZStrsCorrA
Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub

Dim dArrayX() As Double
Dim dArrayY() As Double

#If 1
'モニタ用データ取得シーケンス処理 XY
Sub DataTabeleSet()
Dim iMesIdx As Int32			'測定回数0～
Try
    Select Case m_Job
    Case -1
        m_Job = 0 

    Case 0
        m_Job = 1
		m_SW1.Restart()

    Case 1
    'モニタ中処理
		'データの追加
		m_GetCount += 1
		dtRow = ds.Tables("StressMonitor").NewRow
		dtRow(0) = m_SW1.Elapsed.TotalMilliseconds / 1000.0
		'dtRow(0) = m_GetCount
		dtRow(1) = p_RTM(0).p_mltcp.CurrStrsX()
		dtRow(2) = p_RTM(0).p_mltcp.CurrStrsY()
		ds.Tables("StressMonitor").Rows.Add(dtRow)

		If p_Extract = 1				'抽出中ステータス確認
		'重量-応力測定処理 ------------------------
			If IsNothing(m_StressArrayX)
			'初回 
				Array.Resize(m_StressArrayX, 1)			'平均算出用 配列確保
				Array.Resize(m_StressArrayY, 1)			'平均算出用 配列確保
				m_SW2.Stop()							
				m_SW2.Restart()							'タイマー開始
			Else
			'2回目以降
				Array.Resize(m_StressArrayX, m_StressArrayX.Length + 1)
				Array.Resize(m_StressArrayY, m_StressArrayY.Length + 1)
			End If
			m_StressArrayX(m_StressArrayX.Length-1) = dtRow(1)		'現在応力値を配列へ追加
			m_StressArrayY(m_StressArrayY.Length-1) = dtRow(2)		'現在応力値を配列へ追加
			If m_SW2.Elapsed.TotalSeconds >= 3
			'10秒経過
				if ds.Tables(1).Rows.Count >= cMeasCount
				'1/3回目の測定
					ds.Tables(1).Clear									'結果データテーブル全行消去
					CalcBufferClear()									'計算変数の初期化
					iMesIdx = 0											'測定回数<=1回目
				Else
					iMesIdx = ds.Tables(1).Rows.Count					'測定回数<=n回目
				End If
				dMeasWT(iMesIdx) = convert.ToDouble( nudWeight.Value )	'重量値のセット
				dX(iMesIdx)		= m_StressArrayX.Average()					'応力平均値のセット
				dY(iMesIdx)		= m_StressArrayY.Average()					'応力平均値のセット
				dXY(iMesIdx)	= Math.Sqrt(dX(iMesIdx)^2 + dY(iMesIdx)^2)	'大きさ計算

				'比率計算処理
				'CalcRatioParameter(iMesIdx)

				m_StressArrayX = Nothing							'平均算出用 配列破棄
				m_StressArrayY = Nothing							'平均算出用 配列破棄
				m_SW2.Stop()
				p_Extract	= -1									'抽出中ステータス->未実行
				Dim sTbl As String = ds.Tables(1).TableName
				dtRow = ds.Tables(sTbl).NewRow
				dtRow(sResultTblField(eXYCol.iNo))		= iMesIdx+1
				dtRow(sResultTblField(eXYCol.iDeg))		= 30 * iMesIdx
				dtRow(sResultTblField(eXYCol.iWeight))	= dMeasWT(iMesIdx)
				dtRow(sResultTblField(eXYCol.iADX))		= dX(iMesIdx)
				dtRow(sResultTblField(eXYCol.iADY))		= dY(iMesIdx)
				dtRow(sResultTblField(eXYCol.iMag))		= dXY(iMesIdx)
				dtRow(sResultTblField(eXYCol.iCoef))	= (dMeasWT(iMesIdx)/1000.0 * cGraviAcc) / dXY(iMesIdx)
				ds.Tables(sTbl).Rows.Add(dtRow)
				if ds.Tables(1).Rows.Count >= cMeasCount
					Dim dXYAve As Double = dXY.Average()
					dCoef = (convert.ToDouble( nudWeight.Value )/1000 * cGraviAcc) / dXYAve					'N/dig
				End If
				m_Result	= 1										'結果更新完了
				p_XYCoefMeasDate = Now()							'測定日時更新


				dgvResult.FirstDisplayedScrollingColumnIndex	= 0
				dgvResult.FirstDisplayedScrollingRowIndex		= iMesIdx
			End If
		End if

    Case 8
    Case 9
		'制御初期化
		m_Job = 0
        m_GetCount = 0
		m_SW1.Stop()
		m_SW1.Reset()
		dtStrees.Clear

	Case 80
		'ゼロオフセット処理
		If p_RTM(0).p_mltcp.P_StressZeroEnable
		'既にZeroオフセット適用時は一旦無効にしてから有効にする
			modfrmMain.ZeroSetProc(frmMain)			'ゼロオフセット処理無効
			System.Threading.Thread.Sleep(100)	
		End If
		modfrmMain.ZeroSetProc(frmMain)				'ゼロオフセット処理有効
		'System.Threading.Thread.Sleep(100)	

		ds.Tables(0).Clear				'蓄積データを削除
		m_SW1.Restart()					'モニタ時間用タイマリスタート
		m_Job = 81

	Case 81
		ds.Tables(0).Clear				'蓄積データを削除
		m_SW1.Restart()					'モニタ時間用タイマリスタート
		m_Job = 1
    End Select
Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub
#Else
'モニタ用データ取得シーケンス処理 XY
Sub DataTabeleSet()
Dim iMesIdx As Int32			'測定回数0～
Try
    Select Case m_Job
    Case -1
        m_Job = 0 

    Case 0
        m_Job = 1
		m_SW1.Restart()

    Case 1
    'モニタ中処理
		'データの追加
		m_GetCount += 1
		dtRow = ds.Tables("StressMonitor").NewRow
		dtRow(0) = m_SW1.Elapsed.TotalMilliseconds / 1000.0
		'dtRow(0) = m_GetCount
		dtRow(1) = p_RTM(0).p_mltcp.CurrStrsX()
		dtRow(2) = p_RTM(0).p_mltcp.CurrStrsY()
		ds.Tables("StressMonitor").Rows.Add(dtRow)

		If p_Extract = 1				'抽出中ステータス確認
		'重量-応力測定処理 ------------------------
			If IsNothing(m_StressArrayX)
			'初回 
				Array.Resize(m_StressArrayX, 1)			'平均算出用 配列確保
				Array.Resize(m_StressArrayY, 1)			'平均算出用 配列確保
				'dX(0) = Double.NaN						'結果初期化
				'dY(0) = Double.NaN						'結果初期化
				m_SW2.Stop()							
				m_SW2.Restart()							'タイマー開始
			Else
			'2回目以降
				Array.Resize(m_StressArrayX, m_StressArrayX.Length + 1)
				Array.Resize(m_StressArrayY, m_StressArrayY.Length + 1)
			End If
			m_StressArrayX(m_StressArrayX.Length-1) = dtRow(1)		'現在応力値を配列へ追加
			m_StressArrayY(m_StressArrayY.Length-1) = dtRow(2)		'現在応力値を配列へ追加
			'If m_SW2.Elapsed.TotalSeconds >= 10
			If m_SW2.Elapsed.TotalSeconds >= 3
			'10秒経過
				if ds.Tables(1).Rows.Count >= 3
				'1/3回目の測定
					ds.Tables(1).Clear									'結果データテーブル全行消去
					CalcBufferClear()									'計算変数の初期化
					iMesIdx = 0											'測定回数<=1回目
				Else
					iMesIdx = ds.Tables(1).Rows.Count					'測定回数<=n回目
				End If
				dMeasWT(iMesIdx) = convert.ToDouble( nudWeight.Value )	'重量値のセット
				dX(iMesIdx) = m_StressArrayX.Average()					'応力平均値のセット
				dY(iMesIdx) = m_StressArrayY.Average()					'応力平均値のセット

#If 0
				'テストデータ
				Select Case iMesIdx
				Case 0
					'１回目
					dX(0) = -2679696.38
					dY(0) = 1936849.24
				Case 1
					'２回目
					dX(1) = -3338742.24
					dY(1) = 45941.24
				Case 2
					'3回目
					dX(2) = -2822116.19607843
					dY(2) = -1857364.33333333
				End Select
#end if

				'比率計算処理
				CalcRatioParameter(iMesIdx)

				m_StressArrayX = Nothing							'平均算出用 配列破棄
				m_StressArrayY = Nothing							'平均算出用 配列破棄
				m_SW2.Stop()
				p_Extract	= -1									'抽出中ステータス->未実行
				Dim sTbl As String = ds.Tables(1).TableName
				dtRow = ds.Tables(sTbl).NewRow
				dtRow(sResultTblField(0))	= dMeasWT(iMesIdx)
				dtRow(sResultTblField(1))	= dX(iMesIdx)
				dtRow(sResultTblField(2))	= dY(iMesIdx)
				dtRow(sResultTblField(3))	= dAlpha(iMesIdx)
				dtRow(sResultTblField(4))	= dVectorF(iMesIdx)
				dtRow(sResultTblField(5))	= dVectorTheta(iMesIdx)
				dtRow(sResultTblField(6))	= dRatio(iMesIdx)
				ds.Tables(sTbl).Rows.Add(dtRow)
				Select Case iMesIdx
				Case 0
					'測定1,2回目の比率はNaN
					ds.Tables(sTbl).Rows(0).Item(3) = Double.NaN
					ds.Tables(sTbl).Rows(0).Item(4) = Double.NaN
					ds.Tables(sTbl).Rows(0).Item(5) = Double.NaN
					ds.Tables(sTbl).Rows(0).Item(6) = Double.NaN
				Case 1
					ds.Tables(sTbl).Rows(0).Item(6) = Double.NaN
					ds.Tables(sTbl).Rows(1).Item(4) = Double.NaN
					ds.Tables(sTbl).Rows(1).Item(5) = Double.NaN
					ds.Tables(sTbl).Rows(1).Item(6) = Double.NaN
				Case 2
					'測定2回目のα値を更新
					ds.Tables(sTbl).Rows(1).Item(3) = dAlpha(1)
					'変換係数の計算
					dCoef = (dMeasWT(iMesIdx)/1000 * cGraviAcc) / dVectorF(iMesIdx)			'uN/dig g/1000*9.8/deg
				End Select

				m_Result	= 1										'結果更新完了
			End If
		End if

    Case 8
    Case 9
		'制御初期化
		m_Job = 0
        m_GetCount = 0
		m_SW1.Stop()
		m_SW1.Reset()
		dtStrees.Clear

	Case 80
		'ゼロオフセット処理
		If p_RTM(0).p_mltcp.P_StressZeroEnable
		'既にZeroオフセット適用時は一旦無効にしてから有効にする
			modfrmMain.ZeroSetProc(frmMain)			'ゼロオフセット処理無効
			System.Threading.Thread.Sleep(100)	
		End If
		modfrmMain.ZeroSetProc(frmMain)				'ゼロオフセット処理有効
		'System.Threading.Thread.Sleep(100)	

		ds.Tables(0).Clear				'蓄積データを削除
		m_SW1.Restart()					'モニタ時間用タイマリスタート
		m_Job = 81

	Case 81
		ds.Tables(0).Clear				'蓄積データを削除
		m_SW1.Restart()					'モニタ時間用タイマリスタート
		m_Job = 1
    End Select
Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub
#end If
Private m_ZeroSetMomo As int32 ＝ 0			'Zゼロ点記憶用
'モニタ用データ取得シーケンス処理 Z
Sub DataTabeleSetZ()
Try
    Select Case m_Job
    Case -1
        m_Job = 0 

    Case 0
		m_ZeroSetMomo = 0
        m_Job = 1
		m_SW1.Restart()

    Case 1
    'モニタ中処理
		'データの追加
		m_GetCount += 1
		dtRow = ds.Tables("StressMonitorZ").NewRow
		dtRow(0) = m_SW1.Elapsed.TotalMilliseconds / 1000.0
		'dtRow(0) = m_GetCount
		''		dtRow(1) = p_RTM(0).p_mltcp.CurrStrsZ()
		dtRow(1) = p_RTM(0).p_mltcp.CurrStrsZCorrA() - m_ZeroSetMomo
		ds.Tables("StressMonitorZ").Rows.Add(dtRow)

		If p_ExtractZ = 1Then				'抽出中ステータス確認
		'重量-応力測定処理 ------------------------
			If IsNothing(m_StressArrayZ)
			'初回 
				Array.Resize(m_StressArrayZ, 1)			'平均算出用 配列確保
				m_SW2.Stop()							
				m_SW2.Restart()							'タイマー開始
			Else
			'2回目以降
				Array.Resize(m_StressArrayZ, m_StressArrayZ.Length + 1)
			End If
			m_StressArrayZ(m_StressArrayZ.Length-1) = dtRow(1)		'現在応力値を配列へ追加
			If m_SW2.Elapsed.TotalSeconds >= 2
			'10秒経過
				dMeasWTZ	= convert.ToDouble( nudWeightZ.Value )		'重量値のセット
				dZ			= m_StressArrayZ.Average()					'応力平均値のセット
				m_StressArrayZ = Nothing								'平均算出用 配列破棄

				m_SW2.Stop()
				p_ExtractZ	= -1									'抽出中ステータス->未実行
				m_Result	= 1										'結果更新完了
				p_ZCoefMeasDate = Now()								'測定日時更新
			End If
		End if

    Case 8
    Case 9
		'制御初期化
		m_Job = 0
        m_GetCount = 0
		m_SW1.Stop()
		m_SW1.Reset()
		dtStreesZ.Clear

	Case 80
		'ゼロオフセット処理
		''If p_RTM(0).p_mltcp.P_StressZeroEnable
		'既にZeroオフセット適用時は一旦無効にしてから有効にする
		''	frmMain.ZeroSetProc()			'ゼロオフセット処理無効
		''	System.Threading.Thread.Sleep(1)	
		''End If
		''frmMain.ZeroSetProc()				'ゼロオフセット処理有効
		''System.Threading.Thread.Sleep(1)	

		'無負荷のAD値を記憶
		m_ZeroSetMomo = p_RTM(0).p_mltcp.CurrStrsZCorrA()

		ds.Tables(3).Clear				'蓄積データを削除
		m_SW1.Restart()					'モニタ時間用タイマリスタート
		m_Job = 81

	Case 81
		ds.Tables(3).Clear				'蓄積データを削除
		m_SW1.Restart()					'モニタ時間用タイマリスタート
		m_Job = 1
    End Select
Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub

''' <summary>
''' XY比率計算処理
''' </summary>
''' <returns></returns>
Private Function	CalcRatioParameter(byVal idx As int32) As Boolean
	Dim dDx As Double
	Dim dDy As Double
Try
	Select Case idx
	Case 0, 1
	'何もしない
	Case 2
	'３回目測定計算
		'Dx = Ax + Bx + Cx
		'Ax	(x1*y1)*(x3^2 - x2^2)
		'Bx	(x2*y2)*(x1^2 - x3^2)
		'Cx	(x3*y3)*(x2^2 - x1^2)
		dDx = (dX(0) * dY(0)) * (dX(2)^2 - dX(1)^2) + (dX(1) * dY(1)) * (dX(0)^2 - dX(2)^2) + (dX(2) * dY(2)) * (dX(1)^2 - dX(0)^2)

		'Dy = Ay + By + Cy
		'Ay	(x1*y1)*(y3^2 - y2^2)
		'By	(x2*y2)*(y1^2 - y3^2)
		'Cy	(x3*y3)*(y2^2 - y1^2)
		dDy = (dX(0) * dY(0)) * (dY(2)^2 - dY(1)^2) + (dX(1) * dY(1)) * (dY(0)^2 - dY(2)^2) + (dX(2) * dY(2)) * (dY(1)^2 - dY(0)^2)

		'R = SQRT(-(Dx)/Dy)
''		dRatio(2) = Math.Sqrt(-(dDx)/dDy)	'比率算出
		dRatio(2) = 1	'比率算出

		dRatioY(2) = (dRatio(2) + 1) / 2
		dRatioX(2) = dRatioY(2) / dRatio(2)

		'dYh(0) = dY(0) * dRatio(2)			'1回目のY応力補正
		'dYh(1) = dY(1) * dRatio(2)			'2回目のY応力補正
		'dYh(2) = dY(2) * dRatio(2)			'3回目のY応力補正
		dYh(0) = dY(0) * dRatioY(2)			'1回目のY応力補正
		dYh(1) = dY(1) * dRatioY(2)			'2回目のY応力補正
		dYh(2) = dY(2) * dRatioY(2)			'3回目のY応力補正

		dXh(0) = dX(0) * dRatioX(2)			'1回目のY応力補正
		dXh(1) = dX(1) * dRatioX(2)			'2回目のY応力補正
		dXh(2) = dX(2) * dRatioX(2)			'3回目のY応力補正

		'dAlpha(1)		= Math.Acos((dX(1)^2	 - dXh(0)^2 + dYh(1)^2   - dYh(0)^2) / (2 * (dXh(0) * dYh(0) - dXh(1)   * dYh(1))))				'2回目のα計算
		'dAlpha(2)		= Math.Acos((dX(2)^2	 - dXh(1)^2 + dYh(2)^2   - dYh(1)^2) / (2 * (dXh(1) * dYh(1) - dXh(2)   * dYh(2))))				'3回目のα計算
		dAlpha(1)		= Math.PI / 2				'2回目のα計算
		dAlpha(2)		= Math.PI / 2				'3回目のα計算
		dYx(1)			= dYh(1) * Math.Cos(dAlpha(1))						'2回目Y方向ベクトル X軸成分
		dYx(2)			= dYh(2) * Math.Cos(dAlpha(2))						'3回目Y方向ベクトル X軸成分
		dYy(1)			= dYh(1) * Math.Sin(dAlpha(1))						'2回目Y方向ベクトル Y軸成分
		dYy(2)			= dYh(2) * Math.Sin(dAlpha(2))						'3回目Y方向ベクトル Y軸成分
		dXx(1)			= dXh(1) + dYx(1)									'2回目X方向ベクトル
		dXx(2)			= dXh(2) + dYx(2)									'3回目X方向ベクトル
		dVectorF(1)		= Math.Sqrt(dXx(1)^2 + dYy(1)^2 )					'2回目F	計算
		dVectorF(2)		= Math.Sqrt(dXx(2)^2 + dYy(2)^2 )					'3回目F	計算
		dVectorTheta(1)	= Math.Atan(Math.Tan(dYy(1)/dXx(1)))				'2回目θ 計算
		dVectorTheta(2)	= Math.Atan(Math.Tan(dYy(2)/dXx(2)))				'3回目θ 計算

	End Select
	Return True
Catch ex As Exception
	Return False
End Try
End Function
#If 0
''' <summary>
''' XY比率計算処理
''' </summary>
''' <returns></returns>
Private Function	CalcRatioParameter(byVal idx As int32) As Boolean
	Dim i	As int32
Try
	Select Case idx
	Case 0
	'１回目測定計算
		dZ(idx) = 1.0
		dRatio(idx) = 1.0
		dRatio(idx) = dZ(idx) * dRatio(idx)
		dYh(idx) = dY(idx) * dRatio(idx)

	Case 1
	'２回目測定計算
		dZ(idx) = 1.0
		dRatio(idx) = 1.0

		dRatio(idx) = dZ(idx) * dRatio(idx)
		dYh(idx)	= dY(idx) * dRatio(idx)

		dAlpha(idx)	= Math.Acos((dX(idx)^2 - dX(idx-1)^2 + dYh(idx)^2 - dYh(idx-1)^2) / (2 * (dX(idx-1) * dYh(idx-1) - dX(idx) * dYh(idx))))			'α	計算
		dYy(idx)	= dYh(idx) * Math.Sin(dAlpha(idx))						'Y方向ベクトル Y軸成分
		dYx(idx)	= dYh(idx) * Math.Cos(dAlpha(idx))						'Y方向ベクトル X軸成分
		dXx(idx)	= dX(idx) + dYx(idx)									'X方向ベクトル

	Case 2
	'３回目測定計算
		dZ(idx) = 1.0
		dRatio(idx) = 1.0
		'dZが1.0に収束するまでループ または 100回まで
		For i = 0 To 99 
			dRatio(idx) = dZ(idx) * dRatio(idx)
			dYh(idx-2) = dY(idx-2) * dRatio(idx)
			dYh(idx-1) = dY(idx-1) * dRatio(idx)
			dYh(idx) = dY(idx) * dRatio(idx)

			dAlpha(idx-1)	= Math.Acos((dX(idx-1)^2 - dX(idx-2)^2 + dYh(idx-1)^2 - dYh(idx-2)^2) / (2 * (dX(idx-2) * dYh(idx-2) - dX(idx-1) * dYh(idx-1))))	'前回α	更新計算
			dAlpha(idx)		= Math.Acos((dX(idx)^2	 - dX(idx-1)^2 + dYh(idx)^2   - dYh(idx-1)^2) / (2 * (dX(idx-1) * dYh(idx-1) - dX(idx)   * dYh(idx))))				'α	計算
			dYx(idx-1)	= dYh(idx-1) * Math.Cos(dAlpha(idx-1))					'Y方向ベクトル X軸成分
			dYx(idx)	= dYh(idx) * Math.Cos(dAlpha(idx))						'Y方向ベクトル X軸成分
			dYy(idx-1)	= dYh(idx-1) * Math.Sin(dAlpha(idx-1))					'Y方向ベクトル Y軸成分
			dYy(idx)	= dYh(idx)	 * Math.Sin(dAlpha(idx))					'Y方向ベクトル Y軸成分
			dXx(idx-1)	= dX(idx-1) + dYx(idx-1)								'X方向ベクトル
			dXx(idx)	= dX(idx)   + dYx(idx)									'X方向ベクトル

			dVectorF(idx)		= Math.Sqrt(dXx(idx)^2 + dYy(idx)^2 )										'F	計算
			dVectorTheta(idx)	= Math.Atan(Math.Tan(dYy(idx)/dXx(idx)))									'θ 計算
			dZ(idx)				= Math.Sqrt((dXx(idx)^2 - dXx(idx-1)^2) / (dYy(idx-1)^2 - dYy(idx)^2))		'確認用比率計算
			If Math.Abs(1.0 - dZ(idx)) < 0.00001
			'dZの収束条件でループ終了
				Exit for
			End If
		Next i
	End Select
	Return True
Catch ex As Exception
	Return False
End Try
End Function
#end If

'比率計算処理バッファ初期化
Private Sub CalcBufferClear()
	For i As Int32 = 0 To 2
		dMeasWT(i)		= Double.NaN										'測定データ 重量
		dX(i)			= Double.NaN										'測定データ
		dY(i)			= Double.NaN										'測定データ
		dYh(i)			= Double.NaN										'測定データ(比率掛け)
		dYy(i)			= Double.NaN										'Y方向ベクトル Y軸成分
		dYx(i)			= Double.NaN 										'Y方向ベクトル X軸成分
		dXx(i)			= Double.NaN 										'X方向ベクトル
		dAlpha(i)		= Double.NaN										'Y軸の角度（X軸に対して）
		dVectorF(i)		= Double.NaN										'大きさ
		dVectorTheta(i) = Double.NaN										'方向角度
		dRatio(i)		= Double.NaN										'レベル比率(Xと同レベルにするための)
	Next i
End Sub
''' <summary>
''' 重量校正応力モニタグラフ表示更新
''' </summary>
''' <param name="oChart"></param>
''' <param name="ds"></param>
''' <param name="iXSpan"></param>
''' <param name="iYMin"></param>
''' <param name="iYMax"></param>
Public Sub WeightMonitorChartDisp(ByRef oChart As Chart, ByVal ds As Dataset, ByVal iXSpan As Uint32, ByVal iYMin As Int64 , ByVal iYMax As Int64 , ByVal bYSpanAuto As Boolean)
Dim sSeriesName As String
Dim iCrt		As Int32 = 0			'使用Tableインデックス
Try
	Select Case oChart.Name
    Case "ChartAD"
		iCrt = 0
    Case Else
		iCrt = 3
	End Select
	if ds.Tables(iCrt).Rows.Count = 0
		Exit sub
	End If
'チャートコントロールの設定
	With oChart
	'初期化
	.Series.Clear()
	.Legends.Clear()
	.Titles.Clear()
	.ChartAreas.Clear()
	.DataSource = ds.Tables(iCrt)

    'timeとX,Yを取得
	'X系列
	sSeriesName = ds.Tables(iCrt).Columns(1).ColumnName
	.Series.Add(sSeriesName)
	.Series(sSeriesName).YValueMembers = sSeriesName
	.Series(sSeriesName).YAxisType = AxisType.Primary
	.Series(sSeriesName).Color = Color.Blue
	.Series(sSeriesName).ChartType = SeriesChartType.FastLine
	.Series(sSeriesName).XValueMember = ds.Tables(iCrt).Columns(0).ColumnName

	If iCrt = 0
		'Y系列
		sSeriesName = ds.Tables(iCrt).Columns(2).ColumnName
		.Series.Add(sSeriesName)
		.Series(sSeriesName).YValueMembers = sSeriesName
		.Series(sSeriesName).YAxisType = AxisType.Secondary
		.Series(sSeriesName).Color = Color.Green
		.Series(sSeriesName).ChartType = SeriesChartType.FastLine
		.Series(sSeriesName).XValueMember = ds.Tables(iCrt).Columns(0).ColumnName
	End If

	'TITLE 作成
	Dim title1 As Title = new Title(m_sStr(eMes.STRESS_MON_TITLE))
	.Titles.Add(title1)
	.Titles.Item(0).Font = New Font(BASE_FONT, 16, FontStyle.Bold)
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

	WeightMonitorChartMinMax(oChart, iXSpan, iYMin, iYMax, bYSpanAuto)
#If 0
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
		YMax = Convert.ToInt64( ds.Tables(0).Compute("MAX(XADValue)", "") )
		YMin = Convert.ToInt64( ds.Tables(0).Compute("MIN(XADValue)", "") )
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

	if bYSpanAuto
		YMax = Convert.ToInt64( ds.Tables(0).Compute("MAX(YADValue)", "") )
		YMin = Convert.ToInt64( ds.Tables(0).Compute("MIN(YADValue)", "") )
		If YMax - YMin <= 0
			YMax = YMax + 1000
			YMin = YMin - 1000
		End If
	Else
		YMax = iYMax
		YMin = iYMin
	End if
	.ChartAreas(0).AxisY2.Maximum = YMax
	.ChartAreas(0).AxisY2.Minimum = YMin
#end If
	'X系列
	'.ChartAreas(0).AxisX.Title                      = "Time[Sec]"
	.ChartAreas(0).AxisX.Title                      = m_sStr(eMes.STRESS_MON_X)
    .ChartAreas(0).AxisX.TitleFont                  = New Font(BASE_FONT, 12, FontStyle.Regular)
	.ChartAreas(0).AxisX.Enabled					= AxisEnabled.Auto
    .ChartAreas(0).AxisX.Crossing                   = Double.NaN
	.ChartAreas(0).AxisX.IntervalType               = DateTimeIntervalType.NotSet
	.ChartAreas(0).AxisX.MajorGrid.Enabled          = True
	.ChartAreas(0).AxisX.MajorGrid.LineDashStyle    = ChartDashStyle.Dot
    .ChartAreas(0).AxisX.MajorGrid.LineColor        = Color.LightGray
	'.ChartAreas(0).AxisX.Interval					= iXSpan / 10
	.ChartAreas(0).AxisX.Interval					= 0
	.ChartAreas(0).AxisX.LabelStyle.Format			= "{0}"
	
	'Y系列
	'.ChartAreas(0).AxisY.Title                      = "Z Stress[Dig] "
	If iCrt = 0
		.ChartAreas(0).AxisY.Title                  = m_sStr(eMes.STRESS_MON_XST)
	ElseIf iCrt = 3

		.ChartAreas(0).AxisY.Title                  = m_sStr(eMes.STRESS_MON_ZST)
	End If
    .ChartAreas(0).AxisY.TitleFont                  = New Font(BASE_FONT, 12, FontStyle.Regular)
    .ChartAreas(0).AxisY.Crossing                   = Double.NaN
	.ChartAreas(0).AxisY.IntervalType               = DateTimeIntervalType.NotSet
	.ChartAreas(0).AxisY.MajorGrid.Enabled          = True
	.ChartAreas(0).AxisY.MajorGrid.LineDashStyle    = ChartDashStyle.Dot
    .ChartAreas(0).AxisY.MajorGrid.LineColor        = Color.LightGray
    .ChartAreas(0).AxisY.LabelStyle.Format			 = "0"

	If iCrt = 0
		'Y2系列
		'.ChartAreas(0).AxisY2.Title                      = "Y Stress[Dig] "
		.ChartAreas(0).AxisY2.Title                      = m_sStr(eMes.STRESS_MON_YST)
		.ChartAreas(0).AxisY2.TitleFont                  = New Font(BASE_FONT, 12, FontStyle.Regular)
		.ChartAreas(0).AxisY2.Crossing                   = Double.NaN
		.ChartAreas(0).AxisY2.IntervalType               = DateTimeIntervalType.NotSet
		.ChartAreas(0).AxisY2.MajorGrid.Enabled          = False
		.ChartAreas(0).AxisY2.MajorGrid.LineDashStyle    = ChartDashStyle.Dot
		.ChartAreas(0).AxisY2.MajorGrid.LineColor        = Color.LightGray
		.ChartAreas(0).AxisY2.LabelStyle.Format			 = "0"
	End If


	'-------------------------------------------------------------------------------------------------------------
    'ChartAreaのサイズ位置調整
	.ChartAreas(0).Position.Auto					= False
	.ChartAreas(0).Position.X						= 0
	.ChartAreas(0).Position.Y						= 10
	.ChartAreas(0).Position.height					= 80
	.ChartAreas(0).Position.Width					= 100

	'データテーブルとの結合
	Try
	.DataBind()
	Catch ex As Exception
	End Try
	m_ChartADUpdate = False
    End With
Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub
Private Sub WeightMonitorChartMinMax(ByRef oChart As Chart, ByVal iXSpan As Uint32, ByVal iYMin As Int64 , ByVal iYMax As Int64 , ByVal bYSpanAuto As Boolean)
Dim iCrt		As Int32 = 0			'使用Tableインデックス
Try
	Select Case oChart.Name
    Case "ChartAD"
		iCrt = 0
    Case Else
		iCrt = 3
	End Select

	With oChart
	.ChartAreas(0).AxisX.Maximum = ds.Tables(iCrt).Rows(ds.Tables(iCrt).Rows.Count-1)(0)
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
	Select Case iCrt
	Case 0
		if bYSpanAuto
			YMax = Convert.ToInt64( ds.Tables(iCrt).Compute("MAX(XADValue)", "") )
			YMin = Convert.ToInt64( ds.Tables(iCrt).Compute("MIN(XADValue)", "") )
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

		if bYSpanAuto
			YMax = Convert.ToInt64( ds.Tables(iCrt).Compute("MAX(YADValue)", "") )
			YMin = Convert.ToInt64( ds.Tables(iCrt).Compute("MIN(YADValue)", "") )
			If YMax - YMin <= 0
				YMax = YMax + 1000
				YMin = YMin - 1000
			End If
		Else
			YMax = iYMax
			YMin = iYMin
		End if
		.ChartAreas(0).AxisY2.Maximum = YMax
		.ChartAreas(0).AxisY2.Minimum = YMin
	Case Else
		if bYSpanAuto
			YMax = Convert.ToInt64( ds.Tables(iCrt).Compute("MAX(ZADValue)", "") )
			YMin = Convert.ToInt64( ds.Tables(iCrt).Compute("MIN(ZADValue)", "") )
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

	End Select
	End With
Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub


''' <summary>
''' 重量校正結果をデータグリッドビューへ表示
''' </summary>
''' <param name="dt"></param>
Public Sub WeightCalibDgvDisp(ByRef dt As DataTable)
Try
Dim i As Int32
Dim dgv As DataGridView = dgvResult
Dim s As String  = m_sStr(eMes.ItemStrDgvRst)
Dim sTableDisp() As String = s.split(",")
With dgv
	'Dim sTbl As String = ds.Tables(1).TableName
	.DataSource = dt
	''読み取った時に何故かここだけNaNが入っていなくnullとなっているので補完する
	'If isDbNull(dt.Rows(0).Item(3))
	'	dt.Rows(0).Item(3) = Double.NaN
	'End If

	'.ReadOnly = True					'読取専用
	.RowHeadersVisible		= False		'列ヘッダ消去
	.AllowUserToAddRows		= False		'追加用行を消去
	.ScrollBars				=ScrollBars.Vertical
	.CurrentCell = Nothing
	.Font = New Font(BASE_FONT, 10)
	.Columns(eXYCol.iNo).DefaultCellStyle.format		= "F0"
	.Columns(eXYCol.iDeg).DefaultCellStyle.format		= "F0"
	.Columns(eXYCol.iWeight).DefaultCellStyle.format	= "F3"
	.Columns(eXYCol.iADX).DefaultCellStyle.format		= "F0"
	.Columns(eXYCol.iADY).DefaultCellStyle.format		= "F0"
	.Columns(eXYCol.iMag).DefaultCellStyle.format		= "F0"
	.Columns(eXYCol.iCoef).DefaultCellStyle.format		= "E6"
	For i=0 To .Columns.Count - 1
		.Columns(i).HeaderText = sTableDisp(i)
		.Columns(i).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
	Next i

	.Columns(eXYCol.iNo).Width						= 40
	.Columns(eXYCol.iDeg).Width						= 60
	.Columns(eXYCol.iWeight).Width					= 90
	.Columns(eXYCol.iADX).Width						= 90
	.Columns(eXYCol.iADY).Width						= 90
	.Columns(eXYCol.iMag).Width						= 90
	.Columns(eXYCol.iCoef).Width					= 120

	'.Columns(1).Visible						= False
	.AllowUserToDeleteRows = True

End With
Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub
Public Sub VectorChartDisp( ByRef oChart As Chart, ByVal ds As Dataset, ByVal iXSpan As Uint32, ByVal iYMin As Int64 , ByVal iYMax As Int64 , ByVal bYSpanAuto As Boolean)
Dim sSeriesName As String
Dim dt			As DataTable = ds.Tables(2)
Try
	if dt.Rows.Count = 0
		Exit sub
	End If
'チャートコントロールの設定
	With oChart
	'初期化
	.Series.Clear()
	.Legends.Clear()
	.Titles.Clear()
	.ChartAreas.Clear()
	.DataSource = dt

    'X,Yを取得
	'系列
	sSeriesName = dt.Columns(0).ColumnName
	.Series.Add(sSeriesName)
	.Series(sSeriesName).YValueMembers = dt.Columns(1).ColumnName
	.Series(sSeriesName).YAxisType = AxisType.Primary
	.Series(sSeriesName).Color = Color.Blue
	.Series(sSeriesName).ChartType = SeriesChartType.Line
	.Series(sSeriesName).XValueMember = dt.Columns(0).ColumnName

    .Series(sSeriesName).BorderWidth = 2						'線幅
    .Series(sSeriesName).MarkerStyle = MarkerStyle.Circle		'マーカーの種類を指定
    .Series(sSeriesName).MarkerSize = 5							'マーカーの大きさを指定
    .Series(sSeriesName).MarkerColor = Color.Blue				'マーカーのカラーを設定

	'TITLE 作成
	Dim title1 As Title = new Title(m_sStr(eMes.VECTOR_MON_TITLE))
	
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
	VectorChartMinMax(iXSpan, iYMin, iYMax, bYSpanAuto)


	.ChartAreas(0).AxisX.Title                      = Space(10) & m_sStr(eMes.VECTOR_MON_YST)
    .ChartAreas(0).AxisX.TitleFont                  = New Font(BASE_FONT, 12, FontStyle.Regular)
    .ChartAreas(0).AxisX.Crossing                   = 0
	.ChartAreas(0).AxisX.IntervalType               = DateTimeIntervalType.NotSet
	.ChartAreas(0).AxisX.MajorGrid.Enabled          = True
	.ChartAreas(0).AxisX.MajorGrid.LineDashStyle    = ChartDashStyle.Dot
    .ChartAreas(0).AxisX.MajorGrid.LineColor        = Color.LightGray
	.ChartAreas(0).AxisX.Interval					= 1500000
	.ChartAreas(0).AxisX.LabelStyle.Format			= "{0}"
	
	'X系列
	.ChartAreas(0).AxisY.Title                      = Space(10) & m_sStr(eMes.VECTOR_MON_XST)
    .ChartAreas(0).AxisY.TitleFont                  = New Font(BASE_FONT, 12, FontStyle.Regular)
    .ChartAreas(0).AxisY.Crossing                   = 0
	.ChartAreas(0).AxisY.IntervalType               = DateTimeIntervalType.NotSet
	.ChartAreas(0).AxisY.MajorGrid.Enabled          = True
	.ChartAreas(0).AxisY.MajorGrid.LineDashStyle    = ChartDashStyle.Dot
    .ChartAreas(0).AxisY.MajorGrid.LineColor        = Color.LightGray
	.ChartAreas(0).AxisY.Interval					= 1500000
    .ChartAreas(0).AxisY.LabelStyle.Format			 = "{0}"



	'-------------------------------------------------------------------------------------------------------------
    'ChartAreaのサイズ位置調整
	.ChartAreas(0).Position.Auto					= False
	.ChartAreas(0).Position.X						= 5
	.ChartAreas(0).Position.Y						= 10
	.ChartAreas(0).Position.height					= 90
	.ChartAreas(0).Position.Width					= 90

	'データテーブルとの結合
	Try
	.DataBind()
	Catch ex As Exception
	End Try
	m_ChartVectorUpdate = False

    End With
Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub
Private Sub	VectorChartMinMax(ByVal iXSpan As Uint32, ByVal iYMin As Int64 , ByVal iYMax As Int64 , ByVal bYSpanAuto As Boolean)
Try
With ChartVector
	'.ChartAreas(0).AxisX.Maximum = dt.Rows(dt.Rows.Count-1)(0)
	'If .ChartAreas(0).AxisX.Maximum-iXSpan <= 0
	'	.ChartAreas(0).AxisX.Minimum =  0
	'Else
	'	.ChartAreas(0).AxisX.Minimum =  .ChartAreas(0).AxisX.Maximum - iXSpan
	'End If
    '.ChartAreas(0).AxisX.Maximum = ds.Tables(0).Rows.Count 
	'If ds.Tables(0).Rows.Count-iXSpan <= 0
	'	.ChartAreas(0).AxisX.Minimum =  0
	'Else
	'	.ChartAreas(0).AxisX.Minimum =  ds.Tables(0).Rows.Count - iXSpan
	'End If

	'Dim YMax As Int64
	'Dim YMin As Int64
	'if bYSpanAuto
	'	YMax = Convert.ToInt64( ds.Tables(0).Compute("MAX(XADValue)", "") )
	'	YMin = Convert.ToInt64( ds.Tables(0).Compute("MIN(XADValue)", "") )
	'	If YMax - YMin <= 0
	'		YMax = YMax + 1000
	'		YMin = YMin - 1000
	'	End If
	'Else
	'	YMax = iYMax
	'	YMin = iYMin
	'End if
	.ChartAreas(0).AxisX.Maximum = 3000000
	.ChartAreas(0).AxisX.Minimum = -3000000
	.ChartAreas(0).AxisY.Maximum = 3000000
	.ChartAreas(0).AxisY.Minimum = -3000000

	'if bYSpanAuto
	'	YMax = Convert.ToInt64( ds.Tables(0).Compute("MAX(YADValue)", "") )
	'	YMin = Convert.ToInt64( ds.Tables(0).Compute("MIN(YADValue)", "") )
	'	If YMax - YMin <= 0
	'		YMax = YMax + 1000
	'		YMin = YMin - 1000
	'	End If
	'Else
	'	YMax = iYMax
	'	YMin = iYMin
	'End if

End With
Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
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
#If 0
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
#end if
'64bit ODBCドライバにはテキストをサポートしていないので直接読む
Public Function CsvToDataTable(ByVal sFolder As String, ByVal sFileName As String, byRef dt As DataTable) As Int32
    Dim parser		As TextFieldParser
	Dim iCnt		As int32 = 0
	Dim sRow		As String() = Nothing
	Dim i			As int32
Try
	parser = New TextFieldParser(Path.Combine(sFolder, sFileName), Encoding.UTF8) With {
		.TextFieldType = FieldType.Delimited
	}
	parser.SetDelimiters(",")			'デリミタ設定
	iCnt = 0
	While (Not parser.EndOfData)
        sRow = parser.ReadFields()	'1行読み込み
		Select Case sRow(0)
		Case "XYCoef"
		Case "ZCoef"
		Case Else
			If iCnt <> 0
				dtRow = dt.NewRow()
				For i=0 to sResultTblField.GetUpperBound(0)
					Select Case sRow(i)
					Case "NaN"
						dtRow(sResultTblField(i))	= Double.NaN
					Case Else
						dtRow(sResultTblField(i))	= sRow(i)
					End Select
				Next i
				dt.Rows.Add(dtRow)
			End If
		End Select
		iCnt += 1
    End While
	Return 0
Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Return -1
End Try
End Function

''' <summary>
''' 重量変換測定結果をリードし重量変換パラメータを得る
''' </summary>
''' <param name="dCoef">重量N変換係数 (Dig->N)</param>
''' <param name="dCoefZ">重量g変換係数 (Dig->g)</param>
''' <returns></returns>
Public Function WeightParameterFileGet(ByRef dCoef As Double, ByRef dCoefZ As Double, ByRef coefXYMeasDate As String, ByRef coefZMeasDate As String )
Dim iRet As int32 = 0
Try
	'データセットの重量校正用Xテーブル
	dtResult.TableName = "Result"
	For i=0 To sResultTblField.GetUpperBound(0)
		dtResult.Columns.Add(sResultTblField(i),	Type.GetType("System.Double"))
	Next
	ds.Tables.Add(dtResult)
	iRet = CsvToDataTable(p_SetPath, p_sStoreFileNameWeight, dtResult)	'XY重量パラメータファイルをリードしデータテーブルへセット
	Dim d(1) As Double
	Dim s(1) As String
	iRet = CsvGetCoef(p_SetPath, p_sStoreFileNameWeight, d, s)			'重量パラメータファイルをリードしZ重量変換係数を取得
	dCoef			= d(0)
	dCoefZ			= d(1)
	coefXYMeasDate	= s(0)
	coefZMeasDate	= s(1)
	Return iRet
Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Return -1
End Try
End Function
Public Function CsvSetCoef(ByVal sFolder As String, ByVal sFileName As String, byRef d() As Double) As int32
	Dim fs As FileStream = Nothing
	Dim sw As Textwriter = Nothing
Try
	fs = New FileStream(Path.Combine(sFolder, sFileName), FileMode.Append, FileAccess.Write, FileShare.ReadWrite)
	sw = New StreamWriter(fs, Encoding.UTF8)
	sw.WriteLine("XYCoef,"	& d(0).ToString & "," & "," & "," & p_XYCoefMeasDate)
	sw.WriteLine("ZCoef,"	& d(1).ToString & "," & dMeasWTZ.ToString("F3") & "," &  dZ.ToString("F0") & "," & p_ZCoefMeasDate)
	sw.Close
	fs.Close
	Return 0
Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Return -1
End Try
End Function
Public Function CsvGetCoef(ByVal sFolder As String, ByVal sFileName As String, byRef d() As Double, ByRef sDate() As String) As int32
	Dim fs As FileStream = Nothing
	Dim sr As TextReader = Nothing
	Dim sBuf	As String
	Dim sItm()	As String = Nothing
Try
	fs = New FileStream(Path.Combine(sFolder, sFileName), FileMode.Open, FileAccess.Read, FileShare.ReadWrite)
	sr = New StreamReader(fs, Encoding.UTF8)
	Do
		sBuf = sr.ReadLine()
		If Not IsNothing(sBuf)
			sItm = sBuf.Split(",")
			If Not IsNothing(sItm)
				Select Case sItm(0)
				Case "XYCoef"
					If sItm(1) = "NaN"
						d(0) = 1
					Else
						d(0) = Double.Parse(sItm(1))
					End If
					If sItm.Length>=5
						sDate(0) = sItm(4)
					Else
						sDate(0) = ""
					End If
				Case "ZCoef"
					If sItm(1) = "NaN"
						d(1) = 1
						dMeasWTZ	= Double.NaN
						dZ			= Double.NaN
					Else
						d(1)		= Double.Parse(sItm(1))
						dMeasWTZ	= Double.Parse(sItm(2))
						dZ			= Double.Parse(sItm(3))
					End If
					If sItm.Length>=5
						sDate(1) = sItm(4)
					Else
						sDate(1) = ""
					End If
				End Select
			End If
		End If
	Loop Until sBuf Is Nothing
	sr.Close
	fs.Close
	Return 0
Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Return -1
End Try
End Function

'YスパンAUTO
Private Sub btnYSpanAuto_Click(sender As Object, e As EventArgs) Handles btnYSpanAuto.Click,btnYSpanAutoZ.Click
	Select Case sender.Name
	Case "btnYSpanAuto"
		m_bYSpanAuto = Not m_bYSpanAuto
	Case Else
		m_bYSpanAutoZ = Not m_bYSpanAutoZ
	End Select
End Sub
'フォームクローズ
Private Sub frmCalibWeight2_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
    '測定停止
    p_MeasState = -1
    p_MeasStateZ = -1

	If ds.Tables(1).Rows.Count = cMeasCount
		'測定結果をファイル保存
		SaveToCSV(ds.Tables(1), Path.Combine(p_SetPath, p_sStoreFileNameWeight), True, ",", "", "'", Encoding.UTF8 )
		Dim d(1) As Double
		d(0) = dCoef
		d(1) = dCoefZ
		CsvSetCoef(p_SetPath, p_sStoreFileNameWeight, d)			'XYZ重量変換係数
	End If

	ds.Clear()
	Me.Dispose
End Sub
'Zero Resetボタン
Private Sub btnZeroReset_Click(sender As Object, e As EventArgs) Handles btnZeroReset.Click, btnZeroResetZ.Click
	m_Job = 80
End Sub
'抽出ボタン
Private Sub btnExtract_Click(sender As Object, e As EventArgs) Handles btnExtract.Click,btnExtractZ.Click
	Select Case sender.Name
	Case "btnExtract"
		If p_Extract = -1
			p_Extract = 1
			btnExtract.BackColor = Color.Orange
		Else
			p_Extract = -1
			btnExtract.BackColor = Control.DefaultBackColor
		End if
	Case Else
		Call WriteLog("Z重量キャリブレーション サンプリングボタンクリック", eLOG_LEVEL.iINFO)
		If p_ExtractZ = -1
			p_ExtractZ = 1
			btnExtractZ.BackColor = Color.Orange
		Else
			p_ExtractZ = -1
			btnExtractZ.BackColor = Control.DefaultBackColor
		End if
	End Select
End Sub
'結果データグリッド消去
Private Sub btnResultdgvClear_Click(sender As Object, e As EventArgs) Handles btnResultdgvClear.Click
	ds.Tables(1).Clear
End Sub

''' <summary>
''' 閉じるボタン
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
	me.Close()
End Sub

Private Sub tbcMode_SelectedIndexChanged(sender As Object, e As EventArgs) Handles tbcMode.SelectedIndexChanged
	m_GetCount = 0
	m_ChartADUpdate = True
	If tbcMode.SelectedIndex = 0
		m_Job = 80
		DataTabeleSet()
		p_MeasStateZ = -1
		p_MeasState = 0
		lblXYMeasDateVal.Text = p_XYCoefMeasDate
	Else
		m_Job = 80
		DataTabeleSetZ()
		p_MeasStateZ = 0
		p_MeasState = -1
		lblZMeasDateVal.Text = p_ZCoefMeasDate
	End If

End Sub

''' <summary>
''' フォームキーダウン
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub frmCalibWeight2_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
	If (e.Modifiers And Keys.Control) = Keys.Control Then
		Select Case e.KeyCode
		Case Keys.K
		'CTRL＋K デバッグ画面
			frmDebug.Show()

		End Select
	End If
End Sub
End Class