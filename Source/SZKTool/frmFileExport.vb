Imports System
Imports System.Drawing.Printing
Imports System.Threading
Imports System.IO
Imports System.Windows.Forms.DataVisualization.Charting
Imports Microsoft.VisualBasic.ApplicationServices
Imports Microsoft.VisualBasic.FileIO
Imports SZKTool.DataFile
Imports SZKTool.frmOptions
Imports SZKTool.frmMain
Imports SZKTool.RealTimeMeas

''' <summary>
''' テキストファイルエクスポートUI
''' </summary>
Public Class frmFileExport
'指定されたエクスポートファイルパス
Public p_sExportPath				As String = Nothing
Public p_iExptThinning				As int32 = 1
Public p_bItemSel()					As Boolean


Private m_dSumpleAfter				As Double = 0			'間伐後のサンプル数
Private m_dSumpleRate				As Double = 0			'間伐前からの圧縮率
Private m_sItemName()				As String				'項目名

'多言語メッセージ格納
Private m_sStr(eMes.iMAX - 1)		As String   
Enum eMes
	CMDLG_TITLE = 0							'エクスポートするファイル名を入力してください。
	CMDLG_FILTER							'テキストファイル| * .txt; *。csv
	ERR_DATA_NONE							'測定データがありません。
	ERR_EXT_NONE							'拡張子が指定されていません。
	ERR_NOSPC_FOLDER						'フォルダが指定されていません。
	ERR_NOEXT_FOLDER						'フォルダが存在しません。
	ERR_NOSPC_FILE							'ファイル名が指定されていません。
	MES_EXPRT_ING							'エクスポート中...
	MES_EXPRT_CMP							'エクスポートが完了しました。
	ERR_EXPRT								'エクスポートエラーが発生しました。
	ERR_NOSEL								'1つ以上のアイテムを選択します。
	UNT_SAMP								'サンプル
	LVW_COL1								'測定項目
	iMAX
End Enum

''' <summary>
''' フォームロード
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub frmFileExport_Load(sender As Object, e As EventArgs) Handles MyBase.Load
Dim i As int32
Try
	'言語指定による
    Call SetFormLanguage(Me, p_iLanguage, , m_sStr)

	'エクスポート選択項目配列確保
	p_dtFile.ItemArrayMake(m_sItemName)
	'エクスポート選択状態配列確保
	Array.Resize(p_bItemSel, m_sItemName.Length)

	If Not IsNothing(p_bExportSelect)
		Array.Copy(p_bExportSelect, p_bItemSel, p_bItemSel.Length)
	Else
		p_bItemSel = Array.ConvertAll(p_bItemSel, Function() True)
	End If
	If Not IsNothing(p_sExportOutPass)
		p_sExportPath = p_sExportOutPass					'エクスポート出力パス記憶
	Else
		p_sExportPath = SIZUKA_PATH
	End If
	If p_iExportThinning<>0
		p_iExptThinning = p_iExportThinning					'エクスポート間引き数
	Else
		p_iExptThinning = 1
	End If


	With lvItem
	.Clear
	.Font = New Font(BASE_FONT,12)
    .Columns.Add(m_sStr(eMes.LVW_COL1), 400, HorizontalAlignment.Left)
	.FullRowSelect = True
	.MultiSelect = True
	.CheckBoxes = True
    .View = View.Details
	.Size = New Size(400, 20*26 + 25)
	End With

	tssl1.Font = New Font(BASE_FONT, 10)
	For i = 0 To m_sItemName.getupperbound(0)
	    lvItem.Items.Add(m_sItemName(i), i)
		lvItem.Items(i).Checked = p_bItemSel(i)
	Next
	tbxOutputPath.Text = p_sExportPath
	p_sExportPath = tbxOutputPath.Text

	CalcSampleRate()			'間伐数によるサンプル数、圧縮率計算
	nudSampleThinning.Value = p_iExptThinning

	tssl1.ForeColor = Color.Black
	tssl1.Text = ""
	Me.FormBorderStyle = FormBorderStyle.FixedSingle

Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)

End Try
End Sub
''' <summary>
''' 間伐数によるサンプル数、圧縮率計算
''' </summary>
Private Sub CalcSampleRate()
Try
	If IsNothing(p_dtFile.iOxBuf)
		m_dSumpleAfter	= 0
		m_dSumpleRate	= 0
		lblSampleResult.Text = ""
	Else
		If p_dtFile.iOxBuf.Length < nudSampleThinning.Value
			nudSampleThinning.Value = p_dtFile.iOxBuf.Length
			m_dSumpleAfter	= p_dtFile.iOxBuf.Length / nudSampleThinning.Value
			m_dSumpleRate	= m_dSumpleAfter / p_dtFile.iOxBuf.Length * 100
		Else
			m_dSumpleAfter	= p_dtFile.iOxBuf.Length / nudSampleThinning.Value
			m_dSumpleRate	= m_dSumpleAfter / p_dtFile.iOxBuf.Length * 100
		End If
		lblSampleResult.Text =	$"{p_dtFile.iOxBuf.Length:F0}->{m_dSumpleAfter:F0}{m_sStr(eMes.UNT_SAMP)} ({m_dSumpleRate:F3}%)"
	End If
Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub

''' <summary>
''' ボタンイベント処理
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click, btnBrowse.Click, btnOutput.Click, btnAllSelect.Click, btnAllClear.Click, btnClear.Click, btnSelect.Click, btnMeasSelect.Click
Try
	Select Case sender.Name
	Case "btnClose"				'閉じる
		Me.Close
	Case "btnSelect"			'選択
		For Each itm As ListViewItem In lvitem.SelectedItems
			itm.Checked = True 
		Next
		ssLabelUpdate(-1)
	Case "btnClear"				'選択クリア
		For Each itm As ListViewItem In lvitem.SelectedItems
			itm.Checked = False
		Next
		ssLabelUpdate(-1)
	Case "btnAllSelect"			'全選択
		For Each itm As ListViewItem In lvitem.Items
			itm.Checked = True 
		Next
		ssLabelUpdate(-1)
	Case "btnAllClear"			'選択全クリア
		For Each itm As ListViewItem In lvitem.Items
			itm.Checked = False
		Next
		ssLabelUpdate(-1)
	Case "btnMeasSelect"			'測定項目のみ選択
		For Each itm As ListViewItem In lvitem.Items
			If itm.Index < 11 
				itm.Checked = True
			Else
				itm.Checked = False
			End If
		Next
		ssLabelUpdate(-1)
	Case "btnOutput"			'エクスポート処理
		ssLabelUpdate(-1)
		If CheckPath() <> 0			'パス設定チェック
			Exit Sub
		ElseIf CheckSel() <> 0		'項目選択のチェック
			Exit Sub
		Else
			'p_clSpec.spiSpecToPHead()		'変更可能なヘッダ項目のセット
			Dim iRet As Int32
			iRet = p_dtFile.RealTimeMeasDataFileSaveText(eMeasMode.Normal, False, p_cts, p_sExportPath, ,p_iExptThinning, p_bItemSel)
			If iRet = 0
				ssLabelUpdate(6)
			Else
				ssLabelUpdate(7)
			End If
			'実行条件を記憶へ
			Array.Resize(p_bExportSelect, p_bItemSel.Length)		'エクスポート項目選択記憶
			Array.Copy(p_bItemSel, p_bExportSelect, p_bItemSel.Length)
			p_sExportOutPass = p_sExportPath						'エクスポート出力パス記憶
			p_iExportThinning = p_iExptThinning						'エクスポート間引き数
		End If
	Case "btnBrowse"			'出力ファイルブラウズ
		ssLabelUpdate(-1)
		With svFileDlg
		.Title = m_sStr(eMes.CMDLG_TITLE)
		.FileName = If(IsNothing(p_sExportPath), "", tbxOutputPath.Text)
		.RestoreDirectory =True
		.DefaultExt = "csv"
		'.Filter		=  m_sStr(eMes.CMDLG_FILTER)
		If .ShowDialog() = DialogResult.OK
			p_sExportPath = .FileName
			tbxOutputPath.Text = .FileName

		If CheckPath() <> 0			'パス設定チェック
			Exit Sub
		ElseIf CheckSel() <> 0		'項目選択のチェック
			Exit Sub
		Else
			'p_clSpec.spiSpecToPHead()		'変更可能なヘッダ項目のセット
			Dim iRet As Int32
			iRet = p_dtFile.RealTimeMeasDataFileSaveText(eMeasMode.Normal, False, p_cts, p_sExportPath, ,p_iExptThinning, p_bItemSel)
			If iRet = 0
				ssLabelUpdate(6)
			Else
				ssLabelUpdate(7)
			End If
			'実行条件を記憶へ
			Array.Resize(p_bExportSelect, p_bItemSel.Length)		'エクスポート項目選択記憶
			Array.Copy(p_bItemSel, p_bExportSelect, p_bItemSel.Length)
			p_sExportOutPass = p_sExportPath						'エクスポート出力パス記憶
			p_iExportThinning = p_iExptThinning						'エクスポート間引き数

			Call WriteLog("ファイルエクスポート - " & p_sExportPath & " 間引き=" & p_iExptThinning.ToString, eLOG_LEVEL.iINFO)
		End If

		End If
		End With



	End Select
Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub

''' <summary>
''' エクスポートパスロストフォーカス
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub tbxOutputPath_LostFocus(sender As Object, e As EventArgs) Handles tbxOutputPath.LostFocus
Try
	p_sExportPath = tbxOutputPath.Text
	ssLabelUpdate(-1)
Catch ex As Exception
End Try
End Sub

''' <summary>
''' 間引き数変更
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub nudSampleThinning_ValueChanged(sender As Object, e As EventArgs) Handles nudSampleThinning.ValueChanged
	CalcSampleRate()			'間伐数によるサンプル数、圧縮率計算
	p_iExptThinning = nudSampleThinning.Value
	ssLabelUpdate(-1)

End Sub
''' <summary>
''' パス設定の検査
''' </summary>
''' <returns></returns>
Private Function CheckPath() As Int32
	Dim iRet As int32 = -1
	If IsNothing(p_dtFile.iOxBuf)
		ssLabelUpdate(0)
	ElseIf Path.GetExtension(tbxOutputPath.Text) = ""
		ssLabelUpdate(1)
	ElseIf IsNothing(Path.GetDirectoryName(tbxOutputPath.Text))
		ssLabelUpdate(2)

	ElseIf Not Directory.Exists(Path.GetDirectoryName(tbxOutputPath.Text))
		ssLabelUpdate(3)

	ElseIf IsNothing(Path.GetFileName(tbxOutputPath.Text))
		ssLabelUpdate(4)
	Else
		tssl1.ForeColor = Color.Black
		ssLabelUpdate(5)
		iRet = 0
	End If
	Return iRet
End Function
''' <summary>
''' 選択状態の検査
''' </summary>
''' <returns></returns>
Private Function CheckSel() As Int32
	Dim iRet As int32 = -1
	Dim i As int32
	For i=0 To lvItem.Items.Count - 1
		p_bItemSel(i) = lvItem.Items(i).Checked
		If lvItem.Items(i).Checked
			iRet = 0					'1個でもあればOK
		End If
	Next
	If iRet<>0
		ssLabelUpdate(8)
	End If
	Return iRet
End Function
''' <summary>
''' ステータスストリップラベルへ状態を表示更新
''' </summary>
''' <param name="iT"></param>
Private Sub ssLabelUpdate(ByVal iT As Int32)
	Select Case iT
	Case -1				'クリア
		tssl1.ForeColor = Color.Black
		tssl1.Text = ""
	Case 0
		tssl1.ForeColor = Color.Red
		tssl1.Text = m_sStr(eMes.ERR_DATA_NONE)
	Case 1
		tssl1.ForeColor = Color.Red
		tssl1.Text = m_sStr(eMes.ERR_EXT_NONE)
	Case 2
		tssl1.ForeColor = Color.Red
		tssl1.Text = m_sStr(eMes.ERR_NOSPC_FOLDER)
	Case 3
		tssl1.ForeColor = Color.Red
		tssl1.Text = m_sStr(eMes.ERR_NOEXT_FOLDER)
	Case 4
		tssl1.ForeColor = Color.Red
		tssl1.Text = m_sStr(eMes.ERR_NOSPC_FILE)
	Case 5
		tssl1.ForeColor = Color.Black
		tssl1.Text = m_sStr(eMes.MES_EXPRT_ING)
	Case 6
		tssl1.ForeColor = Color.Blue
		tssl1.Text = m_sStr(eMes.MES_EXPRT_CMP)
	Case 7
		tssl1.ForeColor = Color.Red
		tssl1.Text = m_sStr(eMes.ERR_EXPRT)
	Case 8
		tssl1.ForeColor = Color.Red
		tssl1.Text = m_sStr(eMes.ERR_NOSEL)
	End Select
End Sub

'リストビューマウスドラッグ複数選択 ---------------------
Private bIsMouseDownlvItem As Boolean = False	
Private Sub lvItem_MouseDown(sender As Object, e As MouseEventArgs) Handles lvItem.MouseDown
	bIsMouseDownlvItem = True
End Sub
Private Sub lvItem_MouseUp(sender As Object, e As MouseEventArgs) Handles lvItem.MouseUp
	bIsMouseDownlvItem = False
End Sub
Private Sub lvItem_MouseMove(sender As Object, e As MouseEventArgs) Handles lvItem.MouseMove
Dim tlvItem As ListViewItem
	tlvItem = lvItem.GetItemAt(e.X, e.Y)		'マウス位置からリストビューアイテムを得る
	If Not IsNothing(tlvItem)	'アイテム存在する?
		If bIsMouseDownlvItem	'マウスドラッグ中?
			tlvItem.Selected = True
		End If
	End If
End Sub

	'----------------------------------------------------------

End Class