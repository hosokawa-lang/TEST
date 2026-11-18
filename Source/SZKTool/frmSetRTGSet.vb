'''<summary>
'''リアルタイム温度変更の設定を行うフォーム。
'''</summary>
Public Class frmSetRTGSet

'多言語メッセージ格納
Private m_sStr(eMes.iMAX - 1)		 As String   
Enum eMes
	GAPCALIB_OK					'キャリブレーション済
	GAPCALIB_NY					'キャリブレーション未
    
	iMAX
End Enum
'''<summary>
'''コンストラクタ。引数でテキストボックスの内容を初期化する。
'''</summary>
Public Sub New(ByVal str As String)
	InitializeComponent()
    frmOptions.DisplayRTGSETCBO(Me.cboRtgsetName)
	cboRtgsetName.Text = str
End Sub
Private Sub frmSetRTGSet_Load(sender As Object, e As EventArgs) Handles MyBase.Load
    '言語により表示を変更
    Call SetFormLanguage(Me, p_iLanguage,,m_sStr)
End Sub

Private Sub cboRtgsetName_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboRtgsetName.SelectedIndexChanged
	If cboRtgsetName.Text<>""
		Dim bRet As Boolean = frmOptions.fcCheckRTGCalibData(cboRtgsetName.Text)
		sbDispCalibOK(bRet)
		lblCalibOK.Visible = True
	Else
		lblCalibOK.Visible = False
	End If
End Sub
	'''<summary> コンボボックスで指定されたRTGSETの名称を取得する。 </summary>
	Public Function GetVal() As String
	Return cboRtgsetName.Text
End Function

Private Sub	sbDispCalibOK( ByVal bCalibOK As Boolean )
	Select Case bCalibOK
	Case True
		lblCalibOK.Text			= m_sStr(eMes.GAPCALIB_OK)
		lblCalibOK.BackColor	= Color.GreenYellow
	Case Else
		lblCalibOK.Text			= m_sStr(eMes.GAPCALIB_NY)
		lblCalibOK.BackColor = Color.Yellow
	End Select
End Sub
End Class