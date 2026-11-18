Imports Microsoft.VisualBasic.FileIO
Imports System.IO
Imports Microsoft.VisualBasic.ApplicationServices
Imports System.Threading
Imports SZKTool.frmOptions
'水平確認測定インターフェース画面
Public Class frmHorConf
Public Structure stZeroMem
	Dim iSeq	As Int16		'順番
	Dim iXPos	As Int16		'X座標
	Dim iYPos	As Int16		'Y座標
	Dim iZPos	As String		'ZERO調結果
	Dim iDiff	As int64		'XY原点からの差分
	Dim bMeasEn As Boolean		'測定有無
	Dim iCol	As Int16		'DGVのセル座標
	Dim iRaw	As int16		'DGVのセル座標
End Structure
Public pZeroMem()			As stZeroMem
Public pOriginIndex			As Int16			'XY原点のｐZeroMemのインデックス

Private m_SetPath			As String					'結果ファイル保存先
Private m_sStoreFileName	As String					'最新結果が保存されるファイル名
Private m_sSettingFileName	As String
Private sSetting()()		As String
'多言語メッセージ格納
Private m_sStr(eMes.iMAX - 1)		As String   
Enum eMes
	PROGRESS_POS = 0	'進捗 位置 ： 
	PROGRESS_READ		'読込中...
	PROGRESS_READY		'開始準備完了です。
	PROGRESS_MSG		'測定位置へ治具を設置してください。
	PROGRESS_GUIDE		'水平確認操作ガイド
	PROGRESS_CONT		'継続
	PROGRESS_STOP		'中止
	iMAX
End Enum
Sub New

    ' この呼び出しはデザイナーで必要です。
    InitializeComponent()

    ' InitializeComponent() 呼び出しの後で初期化を追加します。
	Dim sEquipName As String = p_sEquipName
	If p_sEquipName.Substring(0,3)<>"SZK"
		Exit Sub
	End If
	m_SetPath	= Path.Combine( SIZUKA_PATH, p_sEquipName, HORIZON_PATH )
    If Not FileSystem.DirectoryExists(m_SetPath) Then
	'フォルダが存在しない
		FileSystem.CreateDirectory(m_SetPath)
	End if
	m_sSettingFileName	= "HorizontalConfirmation.csv"	'最新較正結果が保存される
	if Not FileSystem.FileExists(Path.Combine(m_SetPath, m_sSettingFileName))
	'ファイルが存在しない
		FileSystem.CopyFile(".\" & HORIZON_PATH & "\" & m_sSettingFileName,	Path.Combine(m_SetPath, m_sSettingFileName))
	End if


End Sub
'フォームロード
Private Sub frmHorConf_Load(sender As Object, e As EventArgs) Handles MyBase.Load

	'言語切替
	Call SetFormLanguage(Me, p_iLanguage, ,m_sStr)
	
	ReadSetting()				'設定ファイルリード

	p_FormCalibVisible = True					'ゼロ調の時frmMain上にZero調モニタを表示させない
	
	lblCurrPosXVal.Text = Nothing
	lblCurrPosYVal.Text = Nothing
	lblCurrPosZVal.Text = Nothing
	lblMaxZVal.Text = Nothing
	lblMinZVal.Text = Nothing
	lblOriginZVal.Text = Nothing

	p_RTM(0).p_dZeroStaPosHP = p_cts.dZeroStaPosHP								'ゼロ調開始位置退避
	If p_cts.dZeroStaPosHP>5.000
		p_cts.dZeroStaPosHP -= 5.000											'ゼロ調開始位置初期化
	Else
		p_cts.dZeroStaPosHP = 0.000												'ゼロ調開始位置初期化
	End If
	
	Timer1.Interval = 200
	Timer1.Enabled = True



End Sub
'現在位置表示
Private sub CurrentPositionDisp()
	lblCurrPosXVal.Text = p_RTM(0).p_mltcp.CurrPosX.ToString
	lblCurrPosYVal.Text = p_RTM(0).p_mltcp.CurrPosY.ToString
	lblCurrPosZVal.Text = p_RTM(0).p_mltcp.HeatBPos.ToString
End sub
'その他情報表示
Private Sub OtherInfoDisp()
	lblOriginZVal.Text = pZeroMem(pOriginIndex).iZPos
	Dim wkAry() As int64 = Nothing
	Array.Resize(wkAry, pZeroMem.Length)
	For i=0 To pZeroMem.GetUpperBound(0)
		wkAry(i) = pZeroMem(i).iZPos
	Next
	lblMaxZVal.Text = wkAry.Max()
	lblMinZVal.Text = wkAry.Min()
	lblMinZVal.Text = pZeroMem(pOriginIndex).iZPos
End Sub
'設定ファイルリード
Private Sub ReadSetting()
Dim sPath As String = Path.Combine(m_SetPath, m_sSettingFileName)
Dim sSetting()() As String
Dim i As int16
Dim j As int16
Dim k As int16
Try
	sSetting = CsvToArray(sPath)
	With dgvResult
		.Rows.Clear()
		.Columns.Clear()
		.RowHeadersVisible = False
		.AllowUserToAddRows = False
		.AllowUserToDeleteRows = False
		.AllowUserToOrderColumns = False
		.AllowUserToResizeColumns = False
		.AllowUserToResizeRows = False
		.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
		.ColumnHeadersDefaultCellStyle.Font = New Font(BASE_FONT, 16, FontStyle.Bold)
		.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing
		.ColumnHeadersHeight = 100
		.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
		.RowTemplate.DefaultCellStyle.WrapMode = DataGridViewTriState.True

		.EnableHeadersVisualStyles = False							'Windowsテーマ優先無効
		'カラムヘッダスタイル設定
		.ColumnHeadersDefaultCellStyle.foreColor = Color.Black
		.ColumnHeadersDefaultCellStyle.BackColor = Color.LightGray

		.DefaultCellStyle.BackColor = Color.White

		.DefaultCellStyle.Font = New Font(BASE_FONT, 16, FontStyle.Bold)
		.RowTemplate.Height = 100

		.ReadOnly = True
		.RowHeadersVisible = False
		.ScrollBars =ScrollBars.Both
		'.RowsDefaultCellStyle.SelectionBackColor = System.Drawing.Color.Transparent
		'カラムヘッダ
		For i=0 To sSetting(0).Length - 1
			.Columns.Add("i",sSetting(0)(i))
			.Columns(i).Width = 120
		Next
		.Columns(0).HeaderText = "y/x Axis"
		'データ行追加, データ行値セット
		For i = 1 To sSetting.Length - 1
			.Rows.Add()
			
			For j = 0 To sSetting(0).Length - 1
				.Rows(i-1).Cells(j).Value = sSetting(i)(j)
			Next 
		Next 
		
		'ソートを禁止
        for each column As DataGridViewColumn in dgvResult.Columns
            column.SortMode = DataGridViewColumnSortMode.NotSortable
        Next

		'X軸,Y軸位置
		j=0
		k=0
		Array.Resize(pZeroMem, (sSetting(0).Length - 1) * (sSetting.Length - 1))
		For j=1 To sSetting.Length - 1
			For i=1 To sSetting(0).Length - 1
				pZeroMem(k).iSeq = k
				pZeroMem(k).iXPos = Int64.Parse(sSetting(0)(i))
				pZeroMem(k).iYPos = Int64.Parse(sSetting(j)(0))
				If sSetting(j)(i) <> "X"
					pZeroMem(k).bMeasEn = False
				Else
					pZeroMem(k).bMeasEn = True
				End If
				if pZeroMem(k).iXPos = 0 And pZeroMem(k).iYPos = 0
					pOriginIndex = k
				End If
				pZeroMem(k).iCol = i
				pZeroMem(k).iRaw = j-1
				k+=1
			next
		Next
		Array.Resize(p_RTM(0).p_Horizon, pZeroMem.Length)
		Array.Copy(pZeroMem, 0, p_RTM(0).p_Horizon, 0, pZeroMem.Length)

		'最左列の背景色
		For i = 0 To dgvResult.RowCount - 1
			dgvResult(0, i).Style.BackColor = Color.LightGray
		Next
		.CellBorderStyle = DataGridViewCellBorderStyle.Single
		.ClearSelection
	End With



Catch ex As Exception

End Try
End Sub
''' <summary>
''' CSVファイルをList化する
''' </summary>
''' <param name="fileName"></param>
''' <returns></returns>
Private Function CsvToArray(ByVal fileName As String) As String()()
Dim cReader		As New System.IO.StreamReader(fileName, System.Text.Encoding.UTF8)
Dim textLines	As New List(Of String())
Dim strArr()()	As String
Try
' 読み込みできる文字がなくなるまで繰り返す
	While (cReader.Peek() >= 0)
		Dim stBuffer As String = cReader.ReadLine()				'ファイルを 1 行ずつ読み込む
		Dim cols() As String									'読み込んだものを追加で格納する
		cols = Split(stBuffer, ",")
		textLines.Add(cols)
	End While
	cReader.Close()
	strArr = textLines.ToArray

Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Return Nothing
End Try
	Return strArr
End Function

Private Function ArrayToCsv(ByVal fileName As String) As Int32
Dim cWriter		As New System.IO.StreamWriter(fileName,False, System.Text.Encoding.UTF8)
Dim i As Int16
Dim j As int16
Dim sLineBuf As String = Nothing
Try
	'カラムヘッダ出力
	For i=0 to sSetting(0).GetUpperBound(0)
		sLineBuf = sLineBuf & sSetting(0)(i) & ","
	Next
	cWriter.WriteLine(sLineBuf)

	'ロウヘッダ＋データ
	For j = 0 To sSetting.GetUpperBound(0)
		sLineBuf = Nothing
		sLineBuf = sLineBuf & sSetting(1)(0) & ","
		For i = 0 To sSetting(0).GetUpperBound(0)
			sLineBuf = sLineBuf & pZeroMem(i).iZPos & ","
		Next
		cWriter.WriteLine(sLineBuf)
	Next


Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Return -1
End Try
Return 0
End Function

'水平確認開始ボタン
Private Sub btnStart_Click(sender As Object, e As EventArgs) Handles btnStart.Click
    if p_RTM(0).p_iBGWOrder = BGW_ORDER.NO_ORDER
	    p_RTM(0).p_iStaMonTime = p_cts.iStaMonTime
		p_RTM(0).p_iStaFlucRange = p_cts.iStaFlucRange
	    p_RTM(0).p_iBGWOrder = BGW_ORDER.HORIZON_BUTTON_START
    End If
End Sub
'水平確認停止ボタン
Private Sub btnStop_Click(sender As Object, e As EventArgs) Handles btnStop.Click
	p_RTM(0).p_iBGWOrder = BGW_ORDER.HORIZON_BUTTON_STOP
	p_RTM(0).p_mltcp.ZeroAdjPosX = 0
    p_RTM(0).p_mltcp.ZeroAdjPosY = 0
End Sub
'フォームクローズ
Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
	p_FormCalibVisible		= False
	p_RTM(0).p_iZeroAdjOk	= -1
	p_RTM(0).p_mltcp.ZeroSW(False)
	p_cts.dZeroStaPosHP = p_RTM(0).p_dZeroStaPosHP									'ゼロ調開始位置を元に戻す
	Me.Close
End Sub
Dim bColor As Boolean
Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
	Timer1.Enabled = False
	Dim ccol As int16
	Dim craw As int16
	Dim idx As Int16
Try
	'ステータスバーへの進捗表示
	Select Case p_RTM(0).p_iBGWOrder
	Case BGW_ORDER.HORIZON_BUTTON_START
		dgvResult.DefaultCellStyle.BackColor = Color.white
	Case BGW_ORDER.HORIZON_BUTTON_CNF
	Case BGW_ORDER.HORIZON_BUTTON_0 To BGW_ORDER.HORIZON_BUTTON_1
		idx = iif(p_RTM(0).p_HrznIdx<>-1, p_RTM(0).p_HrznIdx, pOriginIndex) 
		'ToolStripStatusLabel1.Text = "Progress Point : " & idx & " -> (" &  pZeroMem(idx).iXPos & " , " & pZeroMem(idx).iYPos & ")"
		ToolStripStatusLabel1.Text = m_sStr(eMes.PROGRESS_POS) & idx & " -> (" &  pZeroMem(idx).iXPos & " , " & pZeroMem(idx).iYPos & ")"

		dgvResultDisp()			'dgvResultの定期表示

		'現在測定中のセル表示
		ccol = pZeroMem(idx).iCol
		craw = pZeroMem(idx).iRaw
		dgvResult(ccol,craw).Style.BackColor = Color.Yellow
		dgvResult(ccol,craw).Value = m_sStr(eMes.PROGRESS_READ)

	Case BGW_ORDER.HORIZON_BUTTON_2
	Case BGW_ORDER.HORIZON_BUTTON_3
	Case BGW_ORDER.HORIZON_BUTTON_STOP
	Case BGW_ORDER.HORIZON_BUTTON_STOP_1
		dgvResultDisp()			'dgvResultの定期表示

	Case BGW_ORDER.HORIZON_BUTTON_4		'次のブロック操作待ち
		dgvResultDisp()			'dgvResultの定期表示
		'現在測定中のセル表示
		idx = iif(p_RTM(0).p_HrznIdx<>-1, p_RTM(0).p_HrznIdx, pOriginIndex) 
		ccol = pZeroMem(idx).iCol
		craw = pZeroMem(idx).iRaw
		dgvResult(ccol,craw).Style.BackColor = Color.Yellow
		dgvResult(ccol,craw).Value = m_sStr(eMes.PROGRESS_READ)
		If p_RTM(0).p_iHorizonConfOperation  = 0
			Dim fMsg As frmMSG
			Dim iRet As Int32 = -9
			Dim sMsg As String = Nothing
			Do While iRet = -9
				sMsg = m_sStr(eMes.PROGRESS_MSG)						'"測定位置へ治具を設置してください。"
				fMsg = New frmMSG(sMsg, {m_sStr(eMes.PROGRESS_CONT),m_sStr(eMes.PROGRESS_STOP)}, m_sStr(eMes.PROGRESS_GUIDE), Me)
				fMsg.lblMsg.Font = New Font(BASE_FONT, 12)
				fMsg.Width = 400
				fMsg.StartPosition = FormStartPosition.CenterScreen
				fMsg.ShowDialog()
				Select Case fMsg.p_iRes
				Case -1, 2
					iRet = fMsg.p_iRes
				Case 999											'装置異常発生
					iRet = 2
				Case Else
					If p_RTM(0).p_mltcp.isILDisable					'I/L無効?
						iRet = 0
					End If
					If p_RTM(0).p_mltcp.isILState Then				'扉閉？
						iRet = 0
					End If
				End Select
				fMsg.Dispose()
			Loop
			Select Case iRet
			Case 0
				p_RTM(0).p_iHorizonConfOperation  = 1		'測定GO
			Case 2
				p_RTM(0).p_iHorizonConfOperation  = 2		'測定中止
			End Select
		End If
		
	Case Else
		OtherInfoDisp()			'その他情報表示
		ToolStripStatusLabel1.Text = m_sStr(eMes.PROGRESS_READY)
	End Select
	'ボタン色・有効無効
    if p_RTM(0).p_iBGWOrder >= BGW_ORDER.HORIZON_BUTTON_START And p_RTM(0).p_iBGWOrder <= BGW_ORDER.HORIZON_BUTTON_STOP
	'自動計測中
		'btnStart.BackColor = Color.Yellow
		sbObjBackClrBlink(btnStart, Control.DefaultBackColor, Color.Yellow, bColor)
		btnStart.Enabled = False
		btnXYMove.Enabled = False
		btnZeroAdj.Enabled = False
		btnReload.Enabled = False
		btnClose.Enabled = False
	Else
		btnStart.BackColor = Control.DefaultBackColor
		btnStart.Enabled = True
		btnXYMove.Enabled = True
		btnZeroAdj.Enabled = True
		btnReload.Enabled = True
		btnClose.Enabled = True
    End If
	'ゼロ調ボタン色
	If p_RTM(0).p_mltcp.isStatZeroAdjusting
		btnZeroAdj.BackColor = Color.Yellow
	Else
		btnZeroAdj.BackColor = Control.DefaultBackColor
	End If

	CurrentPositionDisp()		'現在値表示
	lblOriginZVal.Text = pZeroMem(pOriginIndex).iZPos	'中心のZ絶対位置
	If idx <> pOriginIndex
		'原点位置でのZ位置を表示
		cCol = pZeroMem(pOriginIndex).iCol
		cRaw = pZeroMem(pOriginIndex).iRaw
		dgvResult(ccol,craw).Value = pZeroMem(pOriginIndex).iZPos
	End If

Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
	Timer1.Enabled = True
End Sub
'dgvResultの定期表示
Private Sub dgvResultDisp()
	Dim col As int16
	Dim raw As int16
Try
	Array.Copy(p_RTM(0).p_Horizon, 0, pZeroMem, 0, pZeroMem.Length)
	For i=0 To pZeroMem.GetUpperBound(0)
		if pZeroMem(i).bMeasEn
			Col = pZeroMem(i).iCol
			Raw = pZeroMem(i).iRaw
			If pZeroMem(i).iDiff >= 0
				dgvResult(col,raw).Style.ForeColor = Color.Blue
			Else
				dgvResult(col,raw).Style.ForeColor = Color.Red
			End If
			dgvResult(col,raw).Value = pZeroMem(i).iZPos  & vbCrLf & "(" & pZeroMem(i).iDiff  & ")"
		End If
	Next
	'一旦全て白にする
	dgvResult.SelectAll
	For Each c As DataGridViewcell In dgvResult.SelectedCells
		c.Style.BackColor = Color.White
	Next
	dgvResult.ClearSelection
	'最左列の背景色
	For i = 0 To dgvResult.RowCount - 1
		dgvResult(0, i).Style.BackColor = Color.LightGray
	Next


Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub

'設定ファイルリロード
Private Sub btnReload_Click(sender As Object, e As EventArgs) Handles btnReload.Click
	ReadSetting()
End Sub
'XY座標駆動
Private Sub btnXYMove_Click(sender As Object, e As EventArgs) Handles btnXYMove.Click
	Dim i As Int16
	Dim iCol As Int16 = dgvResult.CurrentCell.ColumnIndex
	Dim iRow As Int16 = dgvResult.CurrentCell.RowIndex
Try
	For i = 0 To pZeroMem.GetUpperBound(0)
		if pZeroMem(i).iCol=iCol And pZeroMem(i).iRaw=iRow
			Exit for
		End If
	Next
	If i<=pZeroMem.GetUpperBound(0)
		p_RTM(0).p_mltcp.ZeroAdjPosX = pZeroMem(i).iXPos
		p_RTM(0).p_mltcp.ZeroAdjPosY = pZeroMem(i).iYPos
	End If

Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub
'ゼロ調開始ボタン
Private Sub btnZeroAdj_Click(sender As Object, e As EventArgs) Handles btnZeroAdj.Click
Try
    p_RTM(0).p_iStaMonTime = p_cts.iStaMonTime
    p_RTM(0).p_iStaFlucRange = p_cts.iStaFlucRange
    p_RTM(0).p_iBGWOrder = BGW_ORDER.ZERO_BUTTON

Catch ex As Exception
    Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub

''' <summary>
''' フォーム表示イベント
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub frmHorConf_Shown(sender As Object, e As EventArgs) Handles Me.Shown

End Sub

''' <summary>
''' キーダウン
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub frmHorConf_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
	If (e.Modifiers And Keys.Control) = Keys.Control Then
		Select Case e.KeyCode
		Case Keys.K
		'CTRL＋K デバッグ画面
			frmDebug.Show()

		End Select
	End If
End Sub
End Class