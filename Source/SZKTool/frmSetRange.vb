'''<summary>
'''範囲の最小値、最大値を入力するフォーム。
'''</summary>
Public Class frmSetRange
Private iInpMode	As Int16				'0:通常数値 1:時間フォーマット 分指定=9m 時間指定=9h 日指定=9d 時分分割=9:9

''' <summary>
''' 初期化
''' </summary>
''' <param name="dMin">最小値</param>
''' <param name="dMax">最大値</param>
''' <param name="iMode">0:通常値 1:時間フォーマット hh:mm 9m 9h 9d</param>
Public Sub New(ByVal dMin As Double, ByVal dMax As Double, ByVal iMode As Int16)
    InitializeComponent()
    tbMin.Text = CStr(dMin)
    tbMax.Text = CStr(dMax)
	iInpMode	= iMode
End Sub

'Public Sub New(ByVal strMin As String, ByVal strMax As String)
'    InitializeComponent()
'    Dim dMin As Double
'    If Double.TryParse(strMin, dMin) Then
'        tbMin.Text = CStr(dMin)
'    Else
'        tbMin.Text = "0"
'    End If
'    Dim dMax As Double
'    If Double.TryParse(strMax, dMax) Then
'        tbMax.Text = CStr(dMax)
'    Else
'        tbMax.Text = "1"
'    End If
'End Sub

Private Sub frmSetRange_Load(sender As Object, e As EventArgs) Handles MyBase.Load
Try
    '言語により表示を変更
    Call SetFormLanguage(Me, p_iLanguage)

Catch ex As Exception
    Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
    Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub

Private Sub btnOK_Click(sender As Object, e As EventArgs) Handles btnOK.Click
Try
	Select Case iInpMode
	Case 1
		Dim i			As int16
		Dim dRet		As Double
		Dim s()			As String = Nothing
		Dim sTemp(1)	As String
		Dim m(1)		As String									'数値ワーク
		sTemp(0) = tbMin.Text
		sTemp(1) = tbMax.Text
		For i = 0 To 1
			Try
				s = sTemp(i).split(":")					'時分分割入力セパレータ
			Catch ex As Exception
				Me.DialogResult = DialogResult.OK
				Me.Close()
				Exit Sub
			End Try
			Select Case sTemp(i).Substring(s(0).Length-1, 1)
			Case "m","M"
			'分only
				m(i) = (Ctype(sTemp(i).Substring(0,s(0).Length-1), Double) * 60.0).ToString("F1")
			Case "h","H"
			'時間only
				m(i) = (Ctype(sTemp(i).Substring(0,s(0).Length-1), Double) * 60.0 * 60.0).ToString("F1")
			Case "d","D"
			'日only
				m(i) = (Ctype(sTemp(i).Substring(0,s(0).Length-1), Double) * 3600.0 * 24.0).ToString("F1")
			Case Else
			'時分分割または秒指定
				dRet = p_clSpec.ConvertToSeconds(sTemp(i))
				m(i) = dRet.ToString("F1")
			End Select
		Next
		tbMin.Text = m(0)
		tbMax.Text = m(1)
	Case Else
	End Select
    Me.DialogResult = DialogResult.OK
    Me.Close()

Catch ex As Exception
    Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
    Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub

Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
Try
    Me.DialogResult = DialogResult.Cancel
    Me.Close()

Catch ex As Exception
    Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
    Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub

Public Function GetMin() As Double
    Dim dblNum As Double
    If Double.TryParse(tbMin.Text, dblNum) = True Then
        Return dblNum
    End If
    Return 0.0R
End Function

Public Function GetMax() As Double
    Dim dblNum As Double
    If Double.TryParse(tbMax.Text, dblNum) = True Then
        Return dblNum
    End If
    Return 1.0R
End Function


End Class