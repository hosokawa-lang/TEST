Imports System.IO
Imports System.Text
Imports System.Threading
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.Runtime.InteropServices
Imports Microsoft.Office.Interop
Imports System.Drawing

Public Class frmHistData
'多言語メッセージ格納
Private m_sStr(eMes.iMAX - 1)		As String   
Enum eMes
	iYes			= 0			'はい
	iNo							'いいえ
	iHistClmItem				'条件名文字列配列
	iColContents				'測定値領域,履歴領域
	iMAX
End Enum
Dim iSelectedIdx As Integer = -1

''' <summary>
''' フォームロード
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub frmHistData_Load(sender As Object, e As EventArgs) Handles Me.Load
Try
	'言語切替
	Call SetFormLanguage(Me, p_iLanguage, ,m_sStr)

	Dim s() As String = m_sStr(eMes.iHistClmItem).Split(",")
	p_dtFile.HistColumnHeadItem = s
	s = m_sStr(eMes.iColContents).Split(",")
	p_dtFile.ColCntents = s

	'スクロール時のちらつき防止 DoubleBuffered有効
	Dim myType As System.Type = GetType(DataGridView)
	Dim myPropertyInfo As System.Reflection.PropertyInfo = myType.GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Instance Or System.Reflection.BindingFlags.NonPublic)
	myPropertyInfo.SetValue(dgvHistHead, True, Nothing)


	Me.Location = p_HistWinPos						'ウィンドウ位置
	Me.Size		= p_HistWinSize						'ウィンドウサイズ
	'Me.Location = New Point(p_HistWinPos.x,	p_HistWinPos.y)				'ウィンドウ位置
	'Me.Size		= New Size(p_HistWinSize.Width, p_HistWinSize.Height)	'ウィンドウサイズ
	HistConditionDisp(0)							'履歴情報データのリスト表示
Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub
	
''' <summary>
''' 履歴データ情報のリスト表示
''' </summary>
''' <param name="iSel"></param>
''' <returns></returns>
Public Function HistConditionDisp(ByVal iSel As Int32) As Int32
	p_dtFile.HistHeadDispInitial( dgvHistHead, p_iDir )			'DGV初期化
	p_dtFile.HistHeadDispUpdate( dgvHistHead, p_iDir )			'DGV値セット
	Return 0
End Function
'履歴データ情報のリスト 選択しない(セル色反転させないため)
Private Sub dgvHistHead_SelectionChanged(sender As Object, e As EventArgs) Handles dgvHistHead.SelectionChanged
	Select Case dgvHistHead.CurrentCell.ColumnIndex
	Case 0	'系列色、アイテム列は反転させない
		sender.ClearSelection()
	End Select
End Sub

''' <summary>
''' セル上マウスクリックイベント
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub dgvHistHead_CellMouseClick(sender As Object, e As DataGridViewCellMouseEventArgs) Handles dgvHistHead.CellMouseClick
Try
	Select Case e.Button
	Case MouseButtons.Left
	'マウス左ボタン

	Case MouseButtons.Right
	'マウス右ボタン
		'cmsEdit.AutoClose = True
	　　dgvHistHead.ClearSelection()
		If e.RowIndex < 0
		'列ヘッダでは何もしない
			Exit Sub
		Else
	　　　　dgvHistHead(e.ColumnIndex, e.RowIndex).Selected = True
		End If
		Select Case p_iDir		'0:系列方向垂直 1:系列方向水平 
		Case 0	'系列方向垂直
			tsmiDelete.Enabled		= If(IsNothing(dgvHistHead(p_dtFile.eHistCol.DateTime, e.RowIndex).value), False, True)				'測定日時空欄は削除無効
			tsmiReadReal.Enabled	= If(e.RowIndex=0, True, False)																		'測定値領域のみ測定値読込有効
			tsmiReadStd.Enabled		= If(e.RowIndex>DataBuf.cHistMax, True, False)														'基準値領域のみ基準値読込有効
			tsmiMoveToStandard.Enabled= tsmiDelete.Enabled																				'データ存在で基準領域移動有効
			tsmiCopyToStandard.Enabled= tsmiDelete.Enabled																				'データ存在で基準領域複写有効
			tsmiExport.Enabled		= tsmiDelete.Enabled																				'データ存在でエクスポート有効
　　　　　　iSelectedIdx = e.RowIndex
		Case 1	'系列方向水平
			'クリック範囲内チェック
			If e.ColumnIndex < 1 Or e.ColumnIndex <> dgvHistHead.CurrentCell.ColumnIndex Or e.RowIndex <> dgvHistHead.CurrentCell.RowIndex
				Exit Sub
			Else
				tsmiDelete.Enabled		= If(IsNothing(dgvHistHead(e.ColumnIndex, p_dtFile.eHistCol.DateTime).value), False, True)		'測定日時空欄は削除無効
				tsmiReadReal.Enabled	= If(e.ColumnIndex=1, True, False)																'測定値領域のみ測定値読込有効
				tsmiReadStd.Enabled		= If(e.ColumnIndex>DataBuf.cHistMax+1, True, False)												'基準値領域のみ基準値読込有効
				tsmiMoveToStandard.Enabled= tsmiDelete.Enabled																			'データ存在で基準領域移動有効
				tsmiCopyToStandard.Enabled= tsmiDelete.Enabled																			'データ存在で基準領域複写有効
				tsmiExport.Enabled		= tsmiDelete.Enabled																			'データ存在でエクスポート有効
			End If
　　　　　　iSelectedIdx = e.ColumnIndex
		End Select
		'コンテキストメニュー表示
		cmsEdit.Show(System.Windows.Forms.Cursor.Position)
	End Select

Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub

''' <summary>
''' コンテキストメニュー 読込
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub tsmiReadStd_Click(sender As Object, e As EventArgs) Handles tsmiReadStd.Click, tsmiReadReal.Click, tsmiFileReadReal.Click, tsmiFileReadStd1.Click, tsmiFileReadStd2.Click 
	Dim iSel As Int16
Try
	Select sender.Name
	Case "tsmiReadReal","tsmiFileReadReal"
		If DialogResult.OK = modFrmMain.InportFileSelect(frmMain, 0, frmMain.DragDropFilePath)
			modfrmMain.cmsDragDropProc(frmMain, 0)
		End If
	Case "tsmiReadStd"
		iSel = iSelectedIdx-(DataBuf.cHistMax+DataBuf.cStdMax-1)
		If DialogResult.OK = modFrmMain.InportFileSelect(frmMain, iSel, frmMain.DragDropFilePath)
			modfrmMain.cmsDragDropProc(frmMain, iSel)
		End If
	Case "tsmiFileReadStd1"
		iSel = 1
		If DialogResult.OK = modFrmMain.InportFileSelect(frmMain, iSel, frmMain.DragDropFilePath)
			modfrmMain.cmsDragDropProc(frmMain, iSel)
		End If
	Case "tsmiFileReadStd2"
		iSel = 2
		If DialogResult.OK = modFrmMain.InportFileSelect(frmMain, iSel, frmMain.DragDropFilePath)
			modfrmMain.cmsDragDropProc(frmMain, iSel)
		End If
	End Select
	p_RTM(0).GelTimeSeach(frmMain.p_iSelectGItem)				'ゲルタイム再検索
	frmMain.ResearchGelTimeOnChart()							'ゲルタイム表示更新
	GelTimeLabelDispUpdate(frmMain)
	GelTimeJudgeColorUpdate(frmMain)							'ゲルタイム判定表示
	ChartDispUpdate(frmMain, 0, False)							'チャート表示更新
	cmsEdit.Close
Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub
#If 1

''' <summary>
''' コンテキストメニュー 削除
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub DeleteToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles tsmiDelete.Click
	Dim sMeasDate As String = Nothing		'現在セルのファイル名日時
	Dim s() As String = Nothing
	Dim idx As int16
Try
	Select Case p_iDir
	Case 0	'垂直方向へ系列
		idx = dgvHistHead.CurrentCell.RowIndex
	Case 1	'水平方向へ系列
		idx = dgvHistHead.CurrentCell.ColumnIndex - 1
	End Select
	p_dtFile.OneDataCut(idx, p_cts.LTModeEnable)	'データの切り抜き
	ChartDispUpdate(frmMain, 0, False)				'チャート表示更新

	p_dtFile.HistHeadDispInitial( dgvHistHead, p_iDir )			'DGV初期化
	p_dtFile.HistHeadDispUpdate( dgvHistHead, p_iDir )			'DGV値セット

	'読み込んだヘッダ情報をSPEC入力領域へ転送する
	p_clSpec.HeaderToSpecSpi(p_dtFile.p_head)
	'SPEC入力領域からのデータでDGV表示更新
	p_clSpec.SpecSpiToDgvUpdate(frmMain)

	p_RTM(0).GelTimeSeach(frmMain.p_iSelectGItem)				'ゲルタイム再検索
	frmMain.ResearchGelTimeOnChart()							'ゲルタイム表示更新
	GelTimeLabelDispUpdate(frmMain)
	GelTimeJudgeColorUpdate(frmMain)							'ゲルタイム判定表示
	ChartDispUpdate(frmMain, 0, False)							'チャート表示更新

	cmsEdit.Close
Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub
#End If


''' <summary>
''' コンテキストメニュー 基準領域へ移動
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub MovetoStandardToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles tsmiMoveStdArea1.Click, tsmiMoveStdArea2.Click
	Dim sMeasDate As String = Nothing		'現在セルのファイル名日時
	Dim s() As String = Nothing
	Dim idx As int16
Try
	Select Case p_iDir
	Case 0	'垂直方向へ系列
		idx = dgvHistHead.CurrentCell.RowIndex
	Case 1	'水平方向へ系列
		idx = dgvHistHead.CurrentCell.ColumnIndex - 1
	End Select
	p_dtFile.OneDataCut(idx, p_cts.LTModeEnable)					'データの切り抜き
	Select Case sender.name
	Case "tsmiMoveStdArea1"
		p_dtFile.OneDataPaste((DataBuf.cHistMax+DataBuf.cStdMax)-1, p_cts.LTModeEnable)	'データの貼付け
	Case "tsmiMoveStdArea2"
		p_dtFile.OneDataPaste((DataBuf.cHistMax+DataBuf.cStdMax), p_cts.LTModeEnable)	'データの貼付け
	End Select

	p_RTM(0).GelTimeSeach(frmMain.p_iSelectGItem)				'ゲルタイム再検索
	frmMain.ResearchGelTimeOnChart()							'ゲルタイム表示更新
	GelTimeLabelDispUpdate(frmMain)
	GelTimeJudgeColorUpdate(frmMain)							'ゲルタイム判定表示
	ChartDispUpdate(frmMain, 0, False)							'チャート表示更新

	p_dtFile.HistHeadDispInitial( dgvHistHead, p_iDir )			'DGV初期化
	p_dtFile.HistHeadDispUpdate( dgvHistHead, p_iDir )			'DGV値セット
	cmsEdit.Close
Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub
''' <summary>
''' コンテキストメニュー 基準領域へコピー
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub CopyToStandardToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles tsmiCopyStdArea1.Click, tsmiCopyStdArea2.Click
	Dim sMeasDate As String = Nothing		'現在セルのファイル名日時
	Dim s() As String = Nothing
	Dim idx As int16
Try
	Select Case p_iDir
	Case 0	'垂直方向へ系列
		idx = dgvHistHead.CurrentCell.RowIndex
	Case 1	'水平方向へ系列
		idx = dgvHistHead.CurrentCell.ColumnIndex - 1
	End Select
	p_dtFile.OneDataCopy(idx, p_cts.LTModeEnable)						'データの複写
	Select Case sender.name
	Case "tsmiCopyStdArea1"
		p_dtFile.OneDataPaste((DataBuf.cHistMax+DataBuf.cStdMax)-1, p_cts.LTModeEnable)	'データの貼付け
	Case "tsmiCopyStdArea2"
		p_dtFile.OneDataPaste((DataBuf.cHistMax+DataBuf.cStdMax), p_cts.LTModeEnable)	'データの貼付け
	End Select

	ChartDispUpdate(frmMain, 0, False)				'チャート表示更新

	p_dtFile.HistHeadDispInitial( dgvHistHead, p_iDir )			'DGV初期化
	p_dtFile.HistHeadDispUpdate( dgvHistHead, p_iDir )			'DGV値セット
	cmsEdit.Close
Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub
''' <summary>
''' コンテキストメニュー 閉じる
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub CloseToolStripMenuItem_Click(sender As Object, e As EventArgs) 
	cmsEdit.Close
End Sub

''' <summary>
''' フォームクローズ
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub frmHistData_Closed(sender As Object, e As EventArgs) Handles Me.Closed
	p_HistWinPos	= Me.Location		'ウィンドウ位置
	p_HistWinSize	= Me.Size			'ウィンドウサイズ
	'p_HistWinPos	= New Point(Me.Location.X, Me.Location.Y)		'ウィンドウ位置
	'p_HistWinSize	= New Size(Me.Size.Height, Me.Size.Height)		'ウィンドウサイズ
End Sub
'表示メニュー 行列入替 垂直方向へ系列
Private Sub tsmiVerticlSeries_Click(sender As Object, e As EventArgs)  Handles tsmiVerticlSeries.Click
	If p_iDir = 1
		p_iDir = 0
		HistConditionDisp(0)							'履歴情報データのリスト表示
	End If
End Sub
'表示メニュー 行列入替 水平方向へ系列
Private Sub tsmiHorizontalSeries_Click(sender As Object, e As EventArgs) Handles tsmiHorizontalSeries.Click
	If p_iDir = 0
		p_iDir = 1
		HistConditionDisp(0)							'履歴情報データのリスト表示
	End If
End Sub
''' <summary>
''' 画面クローズ
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub tsmiMsnClose_Click(sender As Object, e As EventArgs) Handles tsmiMsnClose.Click
	Me.Close()
End Sub

''' <summary>
''' エクスポート処理
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub tsmiExport_Click(sender As Object, e As EventArgs) Handles tsmiExport.Click
	Dim f	As New frmFileExport
	Dim idx As int16
Try
	Select Case p_iDir
	Case 0	'垂直方向へ系列
		idx = dgvHistHead.CurrentCell.RowIndex
	Case 1	'水平方向へ系列
		idx = dgvHistHead.CurrentCell.ColumnIndex - 1
	End Select
	Select Case idx
	Case 0		'測定値領域
	Case 1		'履歴１
		If Not IsNothing(p_dtFile.iOxBuf)
			'p_dtFile.TemporaryEvacuration(p_cts.LTModeEnable)		'測定領域を退避
			p_dtFile.h1ToBCopy(p_cts.LTModeEnable)		'測定領域を退避
		End If
		p_dtFile.h1Toh0Copy(p_cts.LTModeEnable)	'履歴１を測定値へコピー
	Case 2		'履歴２
		If Not IsNothing(p_dtFile.iOxBuf)
			'p_dtFile.TemporaryEvacuration(p_cts.LTModeEnable)		'測定領域を退避
			p_dtFile.h2ToBCopy(p_cts.LTModeEnable)		'測定領域を退避
		End If
		p_dtFile.h2Toh0Copy(p_cts.LTModeEnable)		'履歴２を測定値へコピー
	Case 3		'履歴３
		If Not IsNothing(p_dtFile.iOxBuf)
			'p_dtFile.TemporaryEvacuration(p_cts.LTModeEnable)		'測定領域を退避
			p_dtFile.h3ToBCopy(p_cts.LTModeEnable)		'測定領域を退避
		End If
		p_dtFile.h3Toh0Copy(p_cts.LTModeEnable)	'履歴３を測定値へコピー
	Case 4		'履歴４
		If Not IsNothing(p_dtFile.iOxBuf)
			'p_dtFile.TemporaryEvacuration(p_cts.LTModeEnable)		'測定領域を退避
			p_dtFile.h4ToBCopy(p_cts.LTModeEnable)		'測定領域を退避
		End If
		p_dtFile.h4Toh0Copy(p_cts.LTModeEnable)	'履歴４を測定値へコピー
	Case 5		'履歴５
		If Not IsNothing(p_dtFile.iOxBuf)
			'p_dtFile.TemporaryEvacuration(p_cts.LTModeEnable)		'測定領域を退避
			p_dtFile.h5ToBCopy(p_cts.LTModeEnable)		'測定領域を退避
		End If
		p_dtFile.h5Toh0Copy(p_cts.LTModeEnable)	'履歴５を測定値へコピー
	Case 6		'基準１
		If Not IsNothing(p_dtFile.iOxBuf)
			'p_dtFile.TemporaryEvacuration(p_cts.LTModeEnable)		'測定領域を退避
			p_dtFile.s1ToBCopy(p_cts.LTModeEnable)		'測定領域を退避
		End If
		p_dtFile.S1Toh0Copy(p_cts.LTModeEnable)					'基準１を測定値へコピー
	Case 7		'基準２
		If Not IsNothing(p_dtFile.iOxBuf)
			'p_dtFile.TemporaryEvacuration(p_cts.LTModeEnable)		'測定領域を退避
			p_dtFile.s2ToBCopy(p_cts.LTModeEnable)		'測定領域を退避
		End If
		p_dtFile.S2Toh0Copy(p_cts.LTModeEnable)					'基準２を測定値へコピー
	Case Else
	End Select

	f.ShowDialog()				'エクスポート処理

	Select Case idx
	Case 0		'測定値領域
	Case 1 To 7	'履歴、基準
		If Not IsNothing(p_dtFile.iOxBuf_B)
			'p_dtFile.TemporaryToCurrCopy(p_cts.LTModeEnable)			'測定領域へ戻す
			p_dtFile.BToh0Paste(p_cts.LTModeEnable)			'測定領域へ戻す
		Else
			p_dtFile.MClear
		End if
	Case Else
	End Select


Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub
End Class