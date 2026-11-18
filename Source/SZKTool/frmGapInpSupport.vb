Public Class frmGapInpSupport

Private m_Line				As Int16 = 10
Private m_dPresedRate		As Double = 70.0
Private m_dGapVal			As Double = 0.0
'多言語メッセージ格納
Private m_sStr(eMes.iMAX - 1)		As String   
Enum eMes
	DGV_CLM_HD = 0			'測定値[mm]
	iMAX
End Enum
Sub New

    ' この呼び出しはデザイナーで必要です。
    InitializeComponent()

    ' InitializeComponent() 呼び出しの後で初期化を追加します。

End Sub
'フォームロード
Private Sub frmGapInpSupport_Load(sender As Object, e As EventArgs) Handles MyBase.Load
	'言語切替
	Call SetFormLanguage(Me, p_iLanguage,,m_sstr)
	DgvDisp(dgvInp)
End Sub

'入力用データグリッド表示
Public Sub DgvDisp(ByRef dgv As DataGridView )
Try
With dgv
	'.ReadOnly = True					'読取専用
	.RowHeadersVisible = False			'列ヘッダ消去
	.AllowUserToAddRows = False			'追加用行を消去
	.CurrentCell = Nothing
	.Font = New Font(BASE_FONT, 12)

	.Columns.Add("InpValue", m_sStr(eMes.DGV_CLM_HD))	'入力用列追加
	.Rows.Add(m_Line)						'最大行追加

	.Columns(0).DefaultCellStyle.format = "F3"
	.Columns(0).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
	.Columns(0).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
	.Columns(0).Width						= 100
	.AllowUserToDeleteRows = True

End With
Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub
'キャンセルボタン
Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
	Me.Dispose
End Sub
#If Not NEW_SPEC
'適用ボタン
Private Sub btnApply_Click(sender As Object, e As EventArgs) Handles btnApply.Click
	frmMain.MeasGap = m_dGapVal
	Me.Dispose
End Sub
#end if
'厚み計算処理
Private Sub ThicCalc()
Dim dArray() As Double = Nothing
Dim i		As int32		'Dgv行インデックス用
Dim j		As int32		'配列用
Dim dData	As Double		'Double変換ワーク
Dim iError	As Int16 = -1	'変換エラー確認用
Try
With dgvInp
	j = 0
	For i = 0 To .Rows.Count - 1
		If Not isNothing(.Rows(i).Cells(0).Value)
		'行に値が入っているか
			'蓄積用配列を準備
			If isNothing(dArray)
				Array.Resize(dArray, 1)
			Else
				Array.Resize(dArray, dArray.Length + 1)
			End if
			Try
			'Double変換
				dData = Double.Parse(.Rows(i).Cells(0).Value)
			Catch ex As Exception
			'変換エラー発生 ---------------------
				Debug.Print(.Rows(i).Cells(0).Value)
				iError = 1									'確認変数セット
				Array.Resize(dArray, dArray.Length - 1)		'蓄積用配列サイズ/インデックス調整
				j -= 1
			’-----------------------------------
			End Try
			If dArray.Length <> 0 And iError <> 1
			'配列サイズ、変換エラー発生していないこと
				dArray(j) = dData							'配列へDouble型をセット
			Else
				iError = -1									'変換エラー変数初期化
			End if
		End If
		j += 1												'配列用インデックス + 1
	Next i
	If Not IsNothing(dArray)
		If dArray.Length <> 0
		'蓄積配列OKなら演算する
			m_dGapVal = dArray.Average() * m_dPresedRate / 100.0
			lblAve.Text = m_dGapVal.ToString("F3")
		End if
	End If
End With
Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub
'セル内容変更
Private Sub dgvInp_CellValueChanged(sender As Object, e As DataGridViewCellEventArgs) Handles dgvInp.CellValueChanged
	ThicCalc()
End Sub
'圧縮率の変更
Private Sub nudPressRate_ValueChanged(sender As Object, e As EventArgs) Handles nudPressRate.ValueChanged
Try
	m_dPresedRate = Double.Parse(nudPressRate.Value)
	ThicCalc()
Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub
'キーダウン
Private Sub frmGapInpSupport_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
	Select Case e.KeyCode
	Case Keys.Delete
		dgvInp.CurrentCell.Value = ""
		ThicCalc()
	End Select
End Sub
End Class