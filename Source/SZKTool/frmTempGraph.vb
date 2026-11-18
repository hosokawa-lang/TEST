'''<summary>
'''温度変化のプレビューグラフを表示するフォーム。
'''</summary>
Public Class frmTempGraph

'''<summary> 温度グラフ表示クラス。 </summary>
Private m_cTemp		As FChart = Nothing

Private m_dTX		As Double() = New Double(1) {0.0R, 0.0R}
Private m_dTY		As Double() = New Double(1) {0.0R, 0.0R}
Private m_iGIdx		As Integer = 0
Private m_picSize	As New System.Drawing.Size(400, 400)
Public Property picBoxSize() As System.Drawing.Size
	Get
		picBoxSize = m_picSize
	End Get
    Set(value As System.Drawing.Size)
		m_picSize = value
    End Set
End Property

'多言語メッセージ格納
Private m_sStr(eMes.iMAX - 1)		As String   
Enum eMes
	AXSIS_TITLE = 0			'軸タイトル
	iMAX
End Enum

Private Sub frmTempGraph_Load(sender As Object, e As EventArgs) Handles MyBase.Load
		
End Sub

Private Sub frmTempGraph_MouseWheel(sender As Object, e As MouseEventArgs) Handles Me.MouseWheel
    Const D_ZM_IN As Double = 0.9R
    Const L_ZM_OUT As Double = 1.1R

    Try
        'グラフの拡大縮小
        If e.Delta > 0 Then
            m_cTemp.ZoomChange(D_ZM_IN, D_ZM_IN)
        Else
            m_cTemp.ZoomChange(L_ZM_OUT, L_ZM_OUT)
        End If

    Catch ex As Exception
        Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
        Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
    End Try
End Sub

''' <summary>
''' グラフ描画 FChart使用
''' </summary>
''' <param name="strTitle">グラフウィンドウタイトル（プロファイル名）</param>
''' <param name="d1x">X軸数値配列</param>
''' <param name="d1y">Y軸数値配列</param>
Public Sub DispLine(ByVal strTitle As String, ByRef d1x As Double(), ByRef d1y As Double())
    Try
        Me.Text = strTitle
		'言語により表示を変更
		Call SetFormLanguage(Me, p_iLanguage,,m_sStr)
		pbG.SizeMode = PictureBoxSizeMode.StretchImage		'ピクチャボックス伸縮可

        If IsNothing(m_cTemp) Then
            'FChartを宣言して関連付け、軸の属性を設定
			Dim s() As String = m_sStr(eMes.AXSIS_TITLE).Split(",")
            m_cTemp = New FChart(pbG, lblFG, New System.Drawing.Point(50, 40))
            m_cTemp.XAxis.NarrowestRange	= 1.0R
            m_cTemp.XAxis.LowestVal			= 0.0R
            m_cTemp.XAxis.HighestVal		= 100000.0R
            m_cTemp.XAxis.MinVal			= 0.0R
            m_cTemp.XAxis.MaxVal			= 1000.0R
            m_cTemp.XAxis.Text				= s(0)
            m_cTemp.Y1Axis.LowestVal		= -300.0R
            m_cTemp.Y1Axis.HighestVal		= 300.0R
            m_cTemp.Y1Axis.MinVal			= -300.0R
            m_cTemp.Y1Axis.MaxVal			= 300.0R
            m_cTemp.Y1Axis.NarrowestRange	= 1.0R
            m_cTemp.Y1Axis.Text				= s(1)
			m_cTemp.AxisFont				= New Font(BASE_FONT, 12, FontStyle.Bold)
        Else
            Exit Sub
        End If

        m_dTX = d1x.Clone()
        m_dTY = d1y.Clone()

        '線を追加
        m_iGIdx = m_cTemp.AddData(m_dTX, m_dTY, Nothing, "Temperature")		'データ系列追加
        m_cTemp.Data(m_iGIdx).DrawType = 0									'線プロット
        m_cTemp.Data(m_iGIdx).Pen = New Pen(Brushes.Red, 2)					'ペン指定

        m_cTemp.FitScale()		'オートスケール描画
        m_cTemp.Redraw()		'データプロット描画

    Catch ex As Exception
        Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
        Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
    End Try
End Sub

Private Sub frmTempGraph_Resize(sender As Object, e As EventArgs) Handles Me.Resize
	pbG.Width	= Me.Width * 0.92
	pbG.height	= Me.height * 0.8
End Sub

End Class