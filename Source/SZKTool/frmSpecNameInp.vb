Imports System.IO
Imports System.Text
Imports System.Threading
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.Runtime.InteropServices
Imports Microsoft.Office.Interop
Imports SZKTool.clSpec

''' <summary>
''' SPECメンテナンス処理
''' </summary>
Public Class frmSpecNameInp
Private	m_iMode			As int16						'操作するSPEC種類 0:Device  1:Sample
Private m_sSpecName		As String = Nothing				'初期選択SPEC名
Private	m_sList()		As String						'SPEC一覧
Private clSpecV			As New clSpec(1)				'SPECクラス生成


'多言語メッセージ格納
Private m_sStr(eMes.iMAX - 1)		As String   
Enum eMes
	iYes			= 0			'はい
	iNo							'いいえ
	OVerWriteConfirm			'上書き確認
	DeleteConfirm				'削除確認
	OVerWriteConfirmMsg			'同名のSPECがありますが上書きしますか？
	DeleteConfirmMsg			'選択されたSPECを削除しますか？
	iDevice						'デバイス
	iSample						'サンプル
	iMAX
End Enum
Sub New(ByVal i As Int16, ByVal s As String)
    ' この呼び出しはデザイナーで必要です。
    InitializeComponent()
    ' InitializeComponent() 呼び出しの後で初期化を追加します。
	m_iMode		= i
	m_sSpecName = s
End Sub
''' <summary>
''' フォームロード
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub frmSpecNameInp_Load(sender As Object, e As EventArgs) Handles MyBase.Load
	'言語切替
	Call SetFormLanguage(Me, p_iLanguage, ,m_sStr)

	'フォームタイトル
	Select Case m_iMode
	Case eSpec.Device
		Me.Text = m_sStr(eMes.iDevice) & Space(1) & Me.Text
	Case eSpec.Sample
		Me.Text = m_sStr(eMes.iSample) & Space(1) & Me.Text
	End Select


	clSpecV.SpecLangageSet(frmMain.p_sMes)

	btnSaveAs.Enabled	= False
	tbxSpecName.Text	= m_sSpecName
	clSpecV.GetSpecFileList( m_iMode, m_sList )		'ファイルリストを得る
	lbxSpec.Items.Clear
	lbxSpec.Items.AddRange(m_sList)
	lbxSpec.SelectionMode = SelectionMode.MultiExtended

	'btnSaveAs.Enabled = False
	btnDelete.Enabled = False

	SpecContentsDisp(tbxSpecName.Text)				'SPEC内容をデータグリッドビューへ表示



End Sub
''' <summary>
''' 閉じるボタン
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
	Me.Close()
End Sub
''' <summary>
''' 書込みボタン
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub btnOK_Click(sender As Object, e As EventArgs) Handles btnSaveAs.Click
	Dim sSpecDir		As String = Nothing
	Dim s				As ArrayList
	Dim fMsg			As frmMSG
	Dim iRet			As int16 = 0

	m_sSpecName = tbxSpecName.Text
	Select Case m_iMode
	Case eSpec.Device
		sSpecDir = Path.Combine(SIZUKA_PATH, p_sEquipName, DEVSPEC_PATH)
	Case eSpec.Sample
		sSpecDir = Path.Combine(SIZUKA_PATH, p_sEquipName, SAMSPEC_PATH)
	End Select
	s = New ArrayList
	'ファイル存在確認
	If File.Exists(Path.Combine(sSpecDir, m_sSpecName & ".csv"))
	'ファイル存在する。上書き確認
		fMsg = New frmMSG(m_sStr(eMes.OVerWriteConfirmMsg), {m_sStr(eMes.iYes),m_sStr(eMes.iNo)}, m_sStr(eMes.OVerWriteConfirm))
		fMsg.lblMsg.Font = New Font(BASE_FONT, 12)
		fMsg.Width = 400
		fMsg.StartPosition = FormStartPosition.CenterScreen
		fMsg.ShowDialog()
		'Select Case fMsg.p_iRes
		'Case -1, 2
		'Case Else
		'End Select
		IRet = fMsg.p_iRes
		fMsg.Dispose()
	End If
	If iRet = 1	Or iRet = 0	'OverWrite Yes または 新規
		clSpecV.CnvDataTableToAryList(m_iMode, s)										'SPEC用データテーブルをCSVファイルIF用のアレイリストへ変換
		clSpecV.CnvAryListToCSV( Path.Combine(sSpecDir, m_sSpecName & ".csv"), s )		'SPEC CSVへのアレイリストの書込み

		clSpecV.GetSpecFileList( m_iMode, m_sList )					'ファイルリストを得る
		lbxSpec.Items.Clear											'ファイルリスト表示クリア
		lbxSpec.Items.AddRange(m_sList)								'ファイルリスト表示追加
		lbxSpec.SelectedItem = tbxSpecName.Text						'選択状態復帰
		SpecContentsDisp(tbxSpecName.Text)							'選択アイテム内容表示
	End If

End Sub

''' <summary>
''' 入力SPEC(DGVのデータテーブル)を指定ファイル名で出力 新規または上書き
''' </summary>
''' <param name="iT">種別 eSpec.Device, eSpec.Sample</param>
''' <param name="sSpecName">ファイル名(SPEC名 拡張子含まない)</param>
''' <returns>0:正常</returns>
Public Function SpecFileWrite(ByVal iT As Int32, ByVal sSpecName As String) As int32
Dim	s			As ArrayList	= New ArrayList
Dim	sSpecDir	As String		= Nothing
Try
	Select Case iT
	Case eSpec.Device
		sSpecDir = Path.Combine(SIZUKA_PATH, p_sEquipName, DEVSPEC_PATH)
		clSpecV.dtDevSpec = p_clSpec.dtDevSpec
	Case eSpec.Sample
		sSpecDir = Path.Combine(SIZUKA_PATH, p_sEquipName, SAMSPEC_PATH)
		clSpecV.dtSamSpec = p_clSpec.dtSamSpec
	End Select
	clSpecV.CnvDataTableToAryList( iT, s )												'SPEC用データテーブルをCSVファイルIF用のアレイリストへ変換
	clSpecV.CnvAryListToCSV( Path.Combine(sSpecDir, sSpecName & ".csv"), s )			'SPEC CSVへのアレイリストの書込み
	Return 0
Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Return -1
End Try
End Function
''' <summary>
''' 削除ボタン
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
	Dim sSpecDir		As String = Nothing
	Dim fMsg			As frmMSG
	Dim iRet			As int16

	'ファイル削除確認
	fMsg = New frmMSG(m_sStr(eMes.DeleteConfirmMsg), {m_sStr(eMes.iYes),m_sStr(eMes.iNo)}, m_sStr(eMes.DeleteConfirm))
	fMsg.lblMsg.Font = New Font(BASE_FONT, 12)
	fMsg.Width = 400
	fMsg.StartPosition = FormStartPosition.CenterScreen
	fMsg.ShowDialog()
	iRet = fMsg.p_iRes
	fMsg.Dispose()
	If iRet = 1	Or iRet = 0	'OverWrite Yes または 新規
		Select Case m_iMode
		Case eSpec.Device
			sSpecDir = Path.Combine(SIZUKA_PATH, p_sEquipName, DEVSPEC_PATH)
		Case eSpec.Sample
			sSpecDir = Path.Combine(SIZUKA_PATH, p_sEquipName, SAMSPEC_PATH)
		End Select
		For Each s As String In lbxSpec.SelectedItems
			If s <> cFree			'Freeは削除不可
				File.Delete(Path.Combine(sSpecDir, s & ".csv"))
			End If
		Next
		clSpecV.GetSpecFileList( m_iMode, m_sList )		'ファイルリストを得る
		lbxSpec.Items.Clear
		lbxSpec.Items.AddRange(m_sList)

		dgvSpecView.DataSource = Nothing
		tbxSpecName.Text = ""
		lbxSpec.SelectedIndex = 0
	End if	
End Sub
''' <summary>
''' テキストボックステキスト変更
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub tbxSpecName_TextChanged(sender As Object, e As EventArgs) Handles tbxSpecName.TextChanged
	tbxSpecName.BackColor = Color.White
	If modPublic.CheckInvalidFileNameChars(tbxSpecName.Text)
	'不正な文字が含まれている
		tbxSpecName.BackColor = Color.Yellow
		btnSaveAs.Enabled = False
	ElseIf tbxSpecName.Text.Length>=1
		btnSaveAs.Enabled = True
	Else
		btnSaveAs.Enabled = False
	End If
End Sub

''' <summary>
''' リストボックスアイテム選択変更
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub lbxSpec_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lbxSpec.SelectedIndexChanged
Dim sL()	As string = Nothing
	tbxSpecName.Text = lbxSpec.SelectedItem			'名前をつけて保存用のテキストボックス
	'操作ボタンの有効無効
	Select Case lbxSpec.SelectedItems.Count
	Case 0
		btnDelete.Enabled = False
	Case 1
		btnSaveAs.Enabled = True
		btnDelete.Enabled = True
	Case >1
		btnSaveAs.Enabled = False
		btnDelete.Enabled = True
	End Select

	SpecContentsDisp(tbxSpecName.Text)				'SPEC内容をデータグリッドビューへ表示

End Sub
''' <summary>
''' SPEC内容をデータグリッドビューへ表示
''' </summary>
''' <param name="sSpecName"></param>
Public Sub SpecContentsDisp(ByVal sSpecName As String)
	Dim s				As New ArrayList										'CSVファイルIF用のアレイリスト
	Dim sSpecDir		As String = Nothing

	Select Case m_iMode
	Case eSpec.Device
		sSpecDir = Path.Combine(SIZUKA_PATH, p_sEquipName, DEVSPEC_PATH)
	Case eSpec.Sample
		sSpecDir = Path.Combine(SIZUKA_PATH, p_sEquipName, SAMSPEC_PATH)
	End Select
	clSpecV.CSVToDataTable( Path.Combine(sSpecDir, sSpecName & ".csv"), s )		'SPECをCSVファイルから読込み
	clSpecV.SpecItemDisp(m_iMode, sSpecName, dgvSpecView, s)					'SPECをデータグリッドビューへ表示
	dgvViewSetting(dgvSpecView)													'SPECをデータグリッドビュー動作設定
	clSpecV.InputRangeSet(m_iMode, dgvSpecView)									'SPECへ数値入力範囲を設定する


End Sub


''' <summary>
''' SPECのビューを設定
''' </summary>
''' <param name="dgv">デバイスSPECのデータグリッドビュー</param>
Public Sub dgvViewSetting(ByRef dgv As DataGridView)
Try
With dgv
	.ColumnHeadersVisible		= False						'列ヘッダ表示設定
	.RowHeadersVisible			= False						'行ヘッダ表示設定
	.AllowUserToAddRows			= False						'追加用行の許可
	.AllowUserToResizeColumns	= False						'カラム幅変更許可
	.AllowUserToResizeRows		= False						'行高さ変更許可
	.MultiSelect				= False

	.CurrentCell				= Nothing
	.Font						= New Font(BASE_FONT, 10)	'フォント設定
	.RowTemplate.Height			= 21
	'四角いカーソルを消す
	.Columns(0).DefaultCellStyle.SelectionBackColor		= Color.White
	.Columns(0).DefaultCellStyle.SelectionForeColor		= Color.Black
	.Columns(1).DefaultCellStyle.SelectionBackColor		= Color.White
	.Columns(1).DefaultCellStyle.SelectionForeColor		= Color.Black
	'セル列幅設定
	.Columns(0).Width			= .Width * 10/20
	.Columns(1).Width			= .Width * 3/20
	.Columns(2).Width			= .Width - (.Columns(0).Width+.Columns(1).Width) - 3
	'行の高さ
	.AutoResizeRows(DataGridViewAutoSizeRowsMode.AllCells)
	'高さ設定
	.Height						= lbxSpec.Height
	.Columns(1).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
	
	.ReadOnly					= False						'読取専用プロパティ
	'編集可不可カラムの設定
	Dim iLim As int16 = 0
	Select Case m_iMode
	Case eSpec.Device
		iLim = clSpecV.sDevItem.GetUpperBound(0)
	Case eSpec.Sample
		iLim = clSpecV.sSamItem.GetUpperBound(0)
	End Select

	For i=0 To iLim
		.Item(0, i).ReadOnly = True
		.Item(1, i).ReadOnly = True
		.Item(2, i).ReadOnly = False

		Select Case m_iMode
		Case eSpec.Device
			Select Case i
			'選択アイテム
			Case _
				eDevSPEC.HP_CONT, _				'熱板位置制御
				eDevSPEC.METHOD, _				'ゲルタイム判定法
				eDevSPEC.STRS_SENS_MODE, _		'応力検知モード
				eDevSPEC.AMP_MODE, _			'振幅測定モード
				eDevSPEC.TEMP_PROFILE			'温度プロファイル
				.Item(2, i).ReadOnly = True
			Case Else
			End Select

		Case eSpec.Sample
			Select Case i
			Case eSamSPEC.LOT, eSamSPEC.MEASURER
			'選択アイテム+入力
			Case eSamSPEC.SHAPE				'形状
			'選択アイテム
				.Item(2, i).ReadOnly = True
			Case _
				eSamSPEC.EFECT_RANGE, _			'有効範囲
				eSamSPEC.JUDGE_RANGE			'判定範囲
				.Item(2, i).ReadOnly = True
				.Item(2, i).Style.Font = New Font(BASE_FONT, 10)	'フォント設定
			Case Else
			'入力範囲を設定
				'If sSamInputRange(i)<>""
				'	.Item(0, i).ToolTipText = sSamInputRange(i)
				'End If
			End Select

		End Select
	Next i
End With

Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub
''' <summary>
''' SPECの選択状態変更イベント
'''   選択状態をカラムにより反転表示させない
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub dgvSpecView_SelectionChanged(sender As Object, e As EventArgs) Handles dgvSpecView.SelectionChanged
Try
	If Not IsNothing(sender.CurrentCell)
		Select Case sender.CurrentCell.ColumnIndex
		Case 0, 1			'アイテム名、単位	
			sender.ClearSelection					'選択反転させない
		Case 2				'Value
		Case Else
		End Select
	End If
Catch ex As Exception
	'Debug.Print("")
End Try
End Sub
Private Sub dgvSpecView_CellBeginEdit(sender As Object, e As DataGridViewCellCancelEventArgs) Handles dgvSpecView.CellBeginEdit
	Dim iRows	As int32 = sender.CurrentRow.Index
	Select Case m_iMode
	Case eSpec.Device
	Case eSpec.Sample
		Select Case iRows
		Case eSamSPEC.SAMPLE
			clSpecV.spi.sSampleName	= Ctype(sender, DataGridView)(2,eSamSPEC.SAMPLE).Value
		Case eSamSPEC.LOT
			clSpecV.spi.sLot		= Ctype(sender, DataGridView)(2,eSamSPEC.LOT).Value
		Case eSamSPEC.MEASURER
			clSpecV.spi.sMeasurer	= Ctype(sender, DataGridView)(2,eSamSPEC.MEASURER).Value
		End Select
	End Select
End Sub

''' <summary>
''' SPEC 数値セル入力完了
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub dgvSpecView_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs) Handles dgvSpecView.CellEndEdit
	Dim iRows	As int32 = sender.CurrentRow.Index
	Dim iRet	As int32

	Select Case m_iMode
	Case eSpec.Device
		iRet = clSpecV.CheckDevSpecNumericValue(Ctype(sender, DataGridView))
		If iRet<>0
			Select Case iRet
			Case -1		'実数変換エラー
			Case -2		'範囲エラー
			End Select
		Else
		End If

	Case eSpec.Sample
		Dim s		As string = Nothing
		Select Case iRows
		'サンプル名、測定者、LOT変更でFreeモードにしない
		Case eSamSPEC.SAMPLE
			If Not IsNothing(Ctype(sender, DataGridView)(2,eSamSPEC.SAMPLE).Value)
				s = CType(sender, DataGridView)(2,eSamSPEC.SAMPLE).Value
				If s.Contains("""") OrElse s.Contains("\") OrElse s.Contains("'")
					'元に戻す
					Ctype(sender, DataGridView)(2,eSamSPEC.SAMPLE).Value = clSpecV.spi.sSampleName
				ElseIf s.Contains(",")
					'カンマはアンダスコアへ置換え
					Ctype(sender, DataGridView)(2,eSamSPEC.SAMPLE).Value = s.Replace(",", "_")
				End If
				'clSpecV.MenuHistAdd(eSamSPEC.SAMPLE, s)			'履歴メニューへ追加
				clSpecV.spi.sSampleName	= Ctype(sender, DataGridView)(2,eSamSPEC.SAMPLE).Value
			End If
		Case eSamSPEC.LOT
			If Not IsNothing(Ctype(sender, DataGridView)(2,eSamSPEC.LOT).Value)
				s = CType(sender, DataGridView)(2,eSamSPEC.LOT).Value
				If s.Contains("""") OrElse s.Contains("\") OrElse s.Contains("'")
					'元に戻す
					Ctype(sender, DataGridView)(2,eSamSPEC.LOT).Value = clSpecV.spi.sLot
				ElseIf s.Contains(",")
					'カンマはアンダスコアへ置換え
					Ctype(sender, DataGridView)(2,eSamSPEC.LOT).Value = s.Replace(",", "_")
				End If
				s = Ctype(sender, DataGridView)(2,eSamSPEC.LOT).Value
				clSpecV.MenuHistAdd(eSamSPEC.LOT, s)				'履歴メニューへ追加
				clSpecV.spi.sLot	= Ctype(sender, DataGridView)(2,eSamSPEC.Lot).Value
			End If
		Case eSamSPEC.MEASURER
			If Not IsNothing(Ctype(sender, DataGridView)(2,eSamSPEC.MEASURER).Value)
				s = CType(sender, DataGridView)(2,eSamSPEC.MEASURER).Value
				If s.Contains("""") OrElse s.Contains("\") OrElse s.Contains("'")
					'元に戻す
					Ctype(sender, DataGridView)(2,eSamSPEC.MEASURER).Value = clSpecV.spi.sMeasurer
				ElseIf s.Contains(",")
					'カンマはアンダスコアへ置換え
					Ctype(sender, DataGridView)(2,eSamSPEC.MEASURER).Value = s.Replace(",", "_")
				End If
				s = Ctype(sender, DataGridView)(2,eSamSPEC.MEASURER).Value
				'clSpecV.MenuHistAdd(eSamSPEC.MEASURER, s)			'履歴メニューへ追加
				clSpecV.spi.sMeasurer	= Ctype(sender, DataGridView)(2,eSamSPEC.MEASURER).Value
			End If
		Case Else
			iRet = clSpecV.CheckSamSpecNumericValue(Ctype(sender, DataGridView))
			If iRet<>0
				Select Case iRet
				Case -1		'実数変換エラー
				Case -2		'範囲エラー
				End Select
			End If
		End Select

	End Select

End Sub

''' <summary>
''' SPEC マウスダウン
'''  右クリックで選択メニューを表示する
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub dgvSpecView_MouseDown(sender As Object, e As MouseEventArgs) Handles dgvSpecView.MouseDown
	Dim grid As DataGridView = CType(sender, DataGridView)
	If e.Button = MouseButtons.Right
		'指定された（クリックされた）座標位置の情報を取得する
		Dim hti As System.Windows.Forms.DataGridView.HitTestInfo
		hti = grid.HitTest(e.X, e.Y)
		If Not IsNothing(sender.CurrentRow)
			If sender.CurrentRow.Index = hti.RowIndex
				SpecContextMenuShow(hti.RowIndex)
			End If
		End If
	End If

End Sub
''' <summary>
''' SPEC キーダウン
'''   SPACEキーで選択メニューを表示する
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub dgvSpecView_KeyDown(sender As Object, e As KeyEventArgs) Handles dgvSpecView.KeyDown
	Dim dgv = CType(sender, DataGridView)
	Select Case e.KeyCode
	Case Keys.Space
		SpecContextMenuShow(dgv.CurrentRow.Index)

	Case Keys.Tab
		e.Handled = True
	End Select
End Sub

''' <summary>
''' SPEC 選択メニュー表示処理 コンテキストメニューストリップ
''' </summary>
''' <param name="RowIdx"></param>
Private Sub SpecContextMenuShow( ByVal RowIdx As Int32 )
	Dim r As Object
	Dim dgv As DataGridView = dgvSpecView
	Select Case m_iMode
	Case eSpec.Device
		Select Case RowIdx
		Case eDevSPEC.HP_CONT
			r = dgv.GetCellDisplayRectangle(2, eDevSPEC.HP_CONT, False)
			clSpecV.SelectMenuDisp(eSpec.Device, cmsSpecSelect, eDevSPEC.HP_CONT, dgv )
			cmsSpecSelect.Show(dgv, r.Location)

		Case eDevSPEC.METHOD
			r = dgv.GetCellDisplayRectangle(2, eDevSPEC.METHOD, False)
			clSpecV.SelectMenuDisp(eSpec.Device, cmsSpecSelect,eDevSPEC.METHOD, dgv)
			cmsSpecSelect.Show(dgv, r.Location)

		Case eDevSPEC.STRS_SENS_MODE
			r = dgv.GetCellDisplayRectangle(2, eDevSPEC.STRS_SENS_MODE, False)
			clSpecV.SelectMenuDisp(eSpec.Device, cmsSpecSelect,eDevSPEC.STRS_SENS_MODE, dgv)
			cmsSpecSelect.Show(dgv, r.Location)

		'Case eDevSPEC.PB_INP_MODE
		'	r = dgv.GetCellDisplayRectangle(2, eDevSPEC.PB_INP_MODE, False)
		'	clSpecV.SelectMenuDisp(eSpec.Device, cmsSpecSelect,eDevSPEC.PB_INP_MODE, dgv)
		'	cmsSpecSelect.Show(dgv, r.Location)
		End Select
	Case eSpec.Sample
		Select Case RowIdx
		Case eSamSPEC.SAMPLE
			r = dgv.GetCellDisplayRectangle(2, eSamSPEC.SAMPLE, False)
			clSpecV.SelectMenuDisp(eSpec.Sample, cmsSpecSelect, eSamSPEC.SAMPLE, dgv )
			cmsSpecSelect.Show(dgv, r.Location)
		Case eSamSPEC.LOT
			r = dgv.GetCellDisplayRectangle(2, eSamSPEC.LOT, False)
			clSpecV.SelectMenuDisp(eSpec.Sample, cmsSpecSelect, eSamSPEC.LOT, dgv )
			cmsSpecSelect.Show(dgv, r.Location)
		Case eSamSPEC.MEASURER
			r = dgv.GetCellDisplayRectangle(2, eSamSPEC.MEASURER, False)
			clSpecV.SelectMenuDisp(eSpec.Sample, cmsSpecSelect, eSamSPEC.MEASURER, dgv )
			cmsSpecSelect.Show(dgv, r.Location)
		Case eSamSPEC.SHAPE
			r = dgv.GetCellDisplayRectangle(2, eSamSPEC.SHAPE, False)
			clSpecV.SelectMenuDisp(eSpec.Sample, cmsSpecSelect, eSamSPEC.SHAPE, dgv )
			cmsSpecSelect.Show(dgv, r.Location)

		End Select
	End Select
End Sub
''' <summary>
''' SPEC セルダブルクリック
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub dgvSpecView_CellMouseDoubleClick(sender As Object, e As DataGridViewCellMouseEventArgs) Handles dgvSpecView.CellMouseDoubleClick
Dim dgvTemp As DataGridView
Try
	dgvTemp = CType(sender, DataGridView)
	Select Case m_iMode
	Case eSpec.Device
		Select Case e.RowIndex
		Case eDevSPEC.TEMP_PROFILE
			'入力ダイアログを表示
			Dim fcdlg As frmSetRTGSet = New frmSetRTGSet(dgvTemp.Item(1, e.RowIndex).Value.ToString)
			If fcdlg.ShowDialog() = Windows.Forms.DialogResult.OK Then
				'複数選択されている場合、選択行全てにに同じ値を代入する
				dgvTemp.Item(2, e.RowIndex).Value = fcdlg.GetVal()
				If dgvTemp.Item(2, e.RowIndex).Value <> vbNullString Then
				Else
					p_RTM(0).p_strRTGSetName = ""
				End If
				dgvTemp.EndEdit()
			End If
			fcdlg.Dispose()
		End Select

	Case eSpec.Sample
		Select Case e.RowIndex
		Case eSamSPEC.JUDGE_RANGE, eSamSPEC.EFECT_RANGE
			'入力ダイアログを表示
			Dim strApprox() As String = dgvTemp.Item(2, e.RowIndex).Value.ToString.Split("-")
			Dim adlg As frmSetRange = New frmSetRange(strApprox(0), strApprox(1), 1)
			If adlg.ShowDialog() = Windows.Forms.DialogResult.OK Then
				Dim dMin As Double
				Dim dMax As Double
				dMin = adlg.GetMin()
				dMax = adlg.GetMax()
				If dMin < 0.0R Then
					dMin = 0.0R
				End If
				If dMax < dMin Then
					dMax = dMin + 0.1R
				End If
				'If e.RowIndex = eCALC_ROW.iMASK_RANGE Then
				'	'マスクレンジの最大値は測定終了時間より大きい値は不可
				'	If dMax > Double.Parse(dgvTemp(2, eDevSPEC.MEAS_TIME).Value) Then
				'		dMax = Double.Parse(dgvTemp(2, eDevSPEC.MEAS_TIME).Value)
				'	End If
				'End If
				If e.RowIndex = eSamSPEC.EFECT_RANGE Then
					'マスクレンジの最大値は測定終了時間より大きい値は不可
					'If dMax > clSpecV.spi.dMeasureTime
					'	dMax = clSpecV.spi.dMeasureTime
					'End If
				End If
				dgvTemp.Item(2, e.RowIndex).Value = String.Format("{0:0.0}", dMin) + " - " + String.Format("{0:0.0}", dMax)
				adlg.Dispose()
			End If
		End Select

	End Select
Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub

End Class