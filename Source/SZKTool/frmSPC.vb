Imports System.ComponentModel
Imports System.Globalization
Imports SZKTool.SPCAnalyzer

Public Class frmSPC
Private spc			As SPCAnalyzer.stSPC
Private analyzer	As SPCAnalyzer
Private frmM		As frmMain
Private m_bInit		As Boolean = True

'多言語メッセージ格納
Private m_sStr(eMes.iMAX - 1)		 As String   
Enum eMes
	iClear = 0
	iDgvItem
	iChtItem
	iMAX
End Enum
Private m_sDgvItem() As String = Nothing
Private m_sChtItem() As String = Nothing
Private m_sStatMin	As String = Nothing
Private m_sStatMax	As String = Nothing

Private dValues() As Double = {125,127,128,124,126,123,122,132,124,122,120,125,128,124,126,123,128,124,126,123,128,124,126,123,128,124,126,123,128,124,126,123,128,124,126,123,128,124,126,123,128,124,126,123,128,124,126,123}
Sub New(ByRef f As frmMain, ByVal dVal() As Double, ByVal sStatMin As String, ByVal sStatMax As String)
    ' この呼び出しはデザイナーで必要です。
    InitializeComponent()
    ' InitializeComponent() 呼び出しの後で初期化を追加します。
	dValues				= dVal
	m_sStatMin			= sStatMin
	m_sStatMax			= sStatMax
	frmM				= f

End Sub
''' <summary>
''' frmMainのSPCサンプル配列で更新
''' </summary>
Public Sub SetVal()
	dValues = frmM.dSPCValues
End Sub

''' <summary>
''' フォームロード
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
Try
	'言語指定による
    Call SetFormLanguage(Me, p_iLanguage,,m_sStr)
	m_sDgvItem = m_sStr(eMes.iDgvItem).Split(",")
	m_sChtItem = m_sStr(eMes.iChtItem).Split(",")

	Me.Text &= $" [{p_clSpec.spi.sSampleName}]-[{p_clSpec.spi.sLot}]"
    ' SPC構造体初期化
    spc = New stSPC With {
        .sSampleName			= p_clSpec.spi.sSampleName,
        .sLot					= p_clSpec.spi.sLot,
        .sMeasurer				= p_clSpec.spi.sMeasurer,
		.iCalcSample			= p_cts.SPCCalcSample,
        .dStdCenter				= p_SPCSL,
        .dStdUpper				= p_SPCUSL,
        .dStdLower				= p_SPCLSL,
        .XMax					= frmM.p_iSpcDispMax,
        .XMin					= frmM.p_iSpcDispMin,
        .sStatisticalRangeMin	= m_sStatMin,
        .sStatisticalRangeMax	= m_sStatMax
    }
    analyzer = New SPCAnalyzer(spc, m_sDgvItem, m_sChtItem )	'SPCクラス生成
	analyzer.SetValues(dValues)						'サンプルデータ配列セット
	btnCalc.PerformClick()							'再計算-表示
	txtFirstN.Text		= spc.iCalcSample.ToString	'計算標本数
	nudSampStart.Value	= spc.XMin					'トレンド表示区間 開始
	nudSampEnd.Value	= spc.XMax					'トレンド表示区間 終了
	dtpStatMin.Value	= spc.sStatisticalRangeMin
	dtpStatMax.Value	= spc.sStatisticalRangeMax
	dgvSPCList.Item(1, 0).Value = p_SPCUSL
	dgvSPCList.Item(1, 1).Value = p_SPCSL
	dgvSPCList.Item(1, 2).Value = p_SPCLSL
	m_bSpcRangeEnb = False
	ckbStatRange.Checked = frmM.p_bSpcRangeAll
	gpxStatiisticalRange.Visible = Not frmM.p_bSpcRangeAll
	Me.Location = New Point(frmM.p_frmSPCLocation.X, frmM.p_frmSPCLocation.Y)

	m_bInit = False
Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub
' 測定値追加ボタン
Private Sub btnAddValue_Click_1(sender As Object, e As EventArgs) Handles btnAddValue.Click, btnAddAll.Click
Try
	Select Case sender.Name
	Case "btnAddAll"
		analyzer.SetValues(dValues)
		btnCalc.PerformClick()
	Case Else
		Dim v As Double
		If Double.TryParse(txtValue.Text, v) Then
			analyzer.AddValue(v)
			txtValue.Clear()
			btnCalc.PerformClick()
		End If
	End Select
Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub
' 計算実行ボタン
Private Sub btnCalc_Click(sender As Object, e As EventArgs) Handles btnCalc.Click
Try
	If m_bSpcRangeEnb
	'統計範囲変更
		If dtpStatMax.Value>dtpStatMin.Value
			'統計範囲を記憶
			frmM.p_sSpcRangeMin = spc.sStatisticalRangeMin
			frmM.p_sSpcRangeMax = spc.sStatisticalRangeMax
			frmM.btnSPCClick()
		Else
			gpxStatiisticalRange.BackColor = Color.Yellow
		End If
	End If

	SetVal()										'frmMainのサンプル配列を取り込む
    spc.dStdUpper	= P_SPCUSL
    spc.dStdCenter	= P_SPCSL
    spc.dStdLower	= P_SPCLSL
	analyzer.SetSpc(spc)
	analyzer.SetValues(dValues)						'サンプルデータ配列セット
	analyzer.Calculate(p_cts.SPCCalcSample)
    analyzer.ShowResultToGrid(dgvSPCList)
	Dim iStart As Int32 = nudSampStart.Value
	Dim iEnd As	Int32   = analyzer.GetValuesCount
	If iStart >= iEnd
		iStart = 1
	End If
	If analyzer.GetValuesCount>=nudSampEnd.Value
		iEnd = nudSampEnd.Value
	End If
	analyzer.DrawTrend(Chart1, iStart, iEnd)
Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try

End Sub
'クリアボタン
Private Sub btnClear_Click(sender As Object, e As EventArgs) Handles btnClear.Click
	analyzer.ClearValues()
	btnCalc.PerformClick()
End Sub
'ENTERキー
Private Sub txtValue_KeyDown(sender As Object, e As KeyEventArgs) Handles txtValue.KeyDown
	If e.KeyCode = Keys.Enter
		btnAddValue.PerformClick()
	End If
End Sub
''' <summary>
''' USL,SL,LSL編集
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub DataGridView1_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs) Handles dgvSPCList.CellEndEdit
Try
	Select Case dgvSPCList.CurrentRow.Index
	Case 0	'USL
		p_SPCUSL = dgvSPCList.CurrentCell.Value
	Case 1	'SL
		p_SPCSL = dgvSPCList.CurrentCell.Value
	Case 2	'LSL
		p_SPCLSL = dgvSPCList.CurrentCell.Value
	End Select
Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub
''' <summary>
''' エクスポート機能ボタン
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub btnExport_Click(sender As Object, e As EventArgs) Handles btnExport.Click
	Dim sfd As New SaveFileDialog()
	sfd.Filter = "CSV files (*.csv)|*.csv"
	sfd.FileName = "SPC_Result.csv"

	If sfd.ShowDialog() = DialogResult.OK Then
		analyzer.ExportToCsv(sfd.FileName)
	End If
End Sub

''' <summary>
''' フォームクロージング
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub frmSPC_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
	'表示範囲 記憶
	frmM.p_iSpcDispMin	= spc.XMin
	frmM.p_iSpcDispMax	= spc.XMax
	'統計範囲を記憶
	frmM.p_sSpcRangeMin = spc.sStatisticalRangeMin
	frmM.p_sSpcRangeMax = spc.sStatisticalRangeMax
	'規格値を記憶
	P_SPCSL				= spc.dStdCenter
	P_SPCUSL			= spc.dStdUpper
	P_SPCLSL			= spc.dStdLower
	'SPCフォーム表示場所を記憶
	frmM.p_frmSPCLocation = New Windows.Point(Me.Location.X, Me.Location.Y)

End Sub
Dim m_bSpcRangeEnb As Boolean = False			'統計範囲変更フラグ
''' <summary>
''' 統計範囲 始まり値変更
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub dtpStatMin_ValueChanged(sender As Object, e As EventArgs) Handles dtpStatMin.ValueChanged
	spc.sStatisticalRangeMin = dtpStatMin.Value.ToString("yyyy/MM/dd HH:mm", CultureInfo.InvariantCulture)
	m_bSpcRangeEnb = True
End Sub
''' <summary>
''' 統計範囲 終わり値変更
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub dtpStatMax_ValueChanged(sender As Object, e As EventArgs) Handles dtpStatMax.ValueChanged
	spc.sStatisticalRangeMax = dtpStatMax.Value.ToString("yyyy/MM/dd HH:mm", CultureInfo.InvariantCulture)
	m_bSpcRangeEnb = True
End Sub
''' <summary>
''' 統計範囲 全/指定 チェックボックス変更
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub ckbStatRange_CheckedChanged(sender As Object, e As EventArgs) Handles ckbStatRange.CheckedChanged
	If Not m_bInit 
		gpxStatiisticalRange.Visible = Not ckbStatRange.Checked		'範囲設定グループボックス表示/非表示
		frmM.p_bSpcRangeAll = ckbStatRange.Checked					'統計範囲 全/指定フラグ 記憶
		If ckbStatRange.Checked
		'指定範囲
			dtpStatMin.Value = dtpStatMin.MinDate
			dtpStatMax.Value = dtpStatMin.MaxDate
		Else
		'全範囲 本日0:00から6日間
			Dim dt		As DateTime	= Now
			Dim dtE		As DateTime = DateAdd(DateInterval.Day, 6, dt)
			Dim roundMin As DateTime = New DateTime(dt.Year, dt.Month, dt.Day, 0, 0, 0)
			Dim roundMax As DateTime = New DateTime(dtE.Year, dtE.Month, dtE.Day, 0, 0, 0)

			spc.sStatisticalRangeMin = roundMin.ToString("yyyy/MM/dd HH:mm", CultureInfo.InvariantCulture)
			spc.sStatisticalRangeMax = roundMax.ToString("yyyy/MM/dd HH:mm", CultureInfo.InvariantCulture)
			frmM.p_sSpcRangeMin = spc.sStatisticalRangeMin
			frmM.p_sSpcRangeMax = spc.sStatisticalRangeMax
			dtpStatMin.Value = frmM.p_sSpcRangeMin
			dtpStatMax.Value = frmM.p_sSpcRangeMax
		End If
	End If
End Sub
''' <summary>
''' 表示範囲 変更
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub nudSampStart_ValueChanged(sender As Object, e As EventArgs) Handles nudSampStart.ValueChanged, nudSampEnd.ValueChanged
	spc.XMin = nudSampStart.Value
	spc.XMax = nudSampEnd.Value
End Sub

End Class