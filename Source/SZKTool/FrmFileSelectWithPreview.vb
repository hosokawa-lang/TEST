Imports System.Drawing.Configuration
Imports System.IO
Imports System.Runtime.InteropServices
Imports System.Runtime.Remoting
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel
Imports System.Windows.Interop
Imports System.Windows.Shapes
Imports ZXing.QrCode.Internal

'汎用測定ファイル選択フォーム処理
Public Class FrmFileSelectWithPreview

'選択ファイル情報
Public Class FileGroup
    Public Property BaseName	As String
    Public Property Files		As List(Of String)
	Public Property FolderPath	As String

    Public Sub New()
        Files = New List(Of String)
    End Sub
End Class
Public p_Selectfiles As List(Of FileGroup)

Public Property SelectedFiles As List(Of String)			'選択ファイルリスト

Private m_currentDir		As String								'ルートフォルダ
Private m_highlightedLines	As New HashSet(Of Integer)				'ハイライト保持
Private m_showMarkedOnly	As Boolean = False						'True:プレビューリストのマークされた行のみ抽出
Private m_iArea				As Int16								'0:測定値領域 1:基準値領域1 2:基準値領域2
Private m_previewTopIndex 	As Integer = 0							'プレビュースクロール位置退避 インデックス
Private m_checkedMap		As New HashSet(Of String)				'チェック状態の保持用
Private m_checkedGroups		As New Dictionary(Of String, FileGroup)	'ファイルグループチェック
Private m_timeCache			As New Dictionary(Of String, Double)	'測定ファイル読込キャッシュ

'多言語メッセージ格納
Private m_sStr(eMes.iMAX - 1)		As String   
Enum eMes
	Clear = 0								'クリア
	PreviewCulumn							'No.,項目,値,説明
	FileListColumn							'ファイル名,更新日時,サイズ,測定時間[秒]
	StandardOneOnly							'基準値領域への読込みは1件のみです
	iMAX
End Enum
Private m_sPreviewColumn()	As String = Nothing
Private m_sFileListColumn()	As String = Nothing

Public Sub New(startDir As String, iArea As int16)
    InitializeComponent()
    m_currentDir	= startDir
	m_iArea			= iArea
End Sub

''' <summary>
''' フォームロード
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub Frm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
	'言語指定による
    Call SetFormLanguage(Me, p_iLanguage, , m_sStr)
	m_sPreviewColumn = m_sStr(eMes.PreviewCulumn).Split(",")
	m_sFileListColumn = m_sStr(eMes.FileListColumn).Split(",")

	LoadTree(m_currentDir)		'ツリービューを設定
	LvInitialize()				'ファイル選択リストビュー初期設定

	'初期ノードを選択
	tvFolders.HideSelection		= False
	Dim targetPath As String	= System.IO.Path.Combine(m_currentDir, p_sEquipName)
	Dim node					= FindNodeByPath(tvFolders.Nodes(0), targetPath)
	If Not IsNothing(node)
		tvFolders.SelectedNode = node
	Else
		tvFolders.SelectedNode = tvFolders.Nodes(0)
	End If
	node.EnsureVisible()
End Sub
''' <summary>
''' フォーム 初表示イベント
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub FrmFileSelectWithPreview_Shown(sender As Object, e As EventArgs) Handles Me.Shown
	tvFolders.Focus()
	tvFolders.Refresh()
	LvPreviewInitialize()		'内容プレビューリストビュー初期設定


	Me.StartPosition	= FormStartPosition.CenterParent
	Me.Size				= New Size(1360, 800)

	'スプリットコンテナ境界比率設定
	SplitContainer1.SplitterDistance = CInt(SplitContainer1.Width * 0.5)
	SplitContainer2.SplitterDistance = CInt(SplitContainer2.Width * 0.2)
End Sub

''' <summary>
''' プレビューLV初期化
''' </summary>
Private Sub LvPreviewInitialize()
    lvPreview.View = View.Details
    lvPreview.FullRowSelect = True
    lvPreview.GridLines		= True
    lvPreview.MultiSelect	= True

    lvPreview.Columns.Clear()
    lvPreview.Columns.Add(m_sPreviewColumn(0), 40)
    lvPreview.Columns.Add(m_sPreviewColumn(1), 200)
    lvPreview.Columns.Add(m_sPreviewColumn(2), 200)
    lvPreview.Columns.Add(m_sPreviewColumn(3), 300)
End Sub
''' <summary>
''' ノード検索
''' </summary>
''' <param name="node"></param>
''' <param name="path"></param>
''' <returns></returns>
Private Function FindNodeByPath(node As TreeNode, path As String) As TreeNode
    If node.Tag.ToString().Equals(path, StringComparison.OrdinalIgnoreCase) Then
        Return node
    End If
    For Each child As TreeNode In node.Nodes
        Dim result = FindNodeByPath(child, path)
        If result IsNot Nothing Then Return result
    Next
    Return Nothing
End Function
Private Sub lvFiles_ItemSelectionChanged(sender As Object, e As ListViewItemSelectionChangedEventArgs) Handles lvFiles.ItemSelectionChanged
	'プレビュースクロール位置の退避
    If e.IsSelected = False Then
        If lvPreview.TopItem IsNot Nothing Then
            m_previewTopIndex = lvPreview.TopItem.Index
        End If
    End If
End Sub

''' <summary>
''' ファイルリストビュー選択変化イベント
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub lvFiles_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lvFiles.SelectedIndexChanged
Try
	If lvFiles.SelectedItems.Count = 0 Then Exit Sub
	Dim fg As FileGroup = CType(lvFiles.SelectedItems(0).Tag, FileGroup)
	If fg.Files Is Nothing OrElse fg.Files.Count = 0 Then Exit Sub
	Dim filePath As String = fg.Files. FirstOrDefault(Function(f) f.Contains("#000000"))
	If String.IsNullOrEmpty(filePath) Then
		filePath = fg.Files.First()
	End If
	''Dim filePath = lvFiles.SelectedItems(0).Tag.ToString()
	Dim topIndex As Integer = If(lvPreview.TopItem IsNot Nothing, lvPreview.TopItem.Index, 0)
    lvPreview.BeginUpdate()
	If lvPreview.Columns.Count = 0 Then
		LvPreviewInitialize()
	End If
	lvPreview.Items.Clear()
    Dim lines = IO.File.ReadLines(filePath)
    Dim rowIndex As Integer = 0
    For Each line In lines
        ' ★ [Data]で終了
        If line.Trim().Equals("[Data]", StringComparison.OrdinalIgnoreCase) Then  Exit For
        ' 空行スキップ
        If String.IsNullOrWhiteSpace(line)									Then Continue For

		Dim currentIndex = rowIndex
		rowIndex += 1					'先にカウントアップ

	    ' ★ マークのみ表示時はスキップ
		If m_showMarkedOnly AndAlso Not m_highlightedLines.Contains(currentIndex ) Then
			Continue For
		End If

        ' カンマ分割（最大3列）
        Dim parts = line.Split(","c)

        Dim itemName As String = If(parts.Length > 0, parts(0).Trim(), "")
        Dim value As String = If(parts.Length > 1, parts(1).Trim(), "")
		value = value.Trim(""""c)											'ダブルクォーテーション除去
        Dim desc As String = If(parts.Length > 2, parts(2).Trim(), "")

        Dim item As New ListViewItem((currentIndex +1).ToString())
        item.SubItems.Add(itemName)
        item.SubItems.Add(value)
        item.SubItems.Add(desc)

        ' ハイライト
        If m_highlightedLines.Contains(currentIndex ) Then
            item.BackColor = Color.Yellow
        End If
        lvPreview.Items.Add(item)
        ' 上限（必要なら）
        If rowIndex > 150 Then Exit For
    Next
    lvPreview.EndUpdate()
	'プレビュースクロール位置 復元
	Me.BeginInvoke(Sub()
		If lvPreview.Items.Count > 0 Then
			Dim safeIndex = Math.Min(m_previewTopIndex, lvPreview.Items.Count - 1)
			lvPreview.TopItem = lvPreview.Items(safeIndex)
		End If
	End Sub)
Catch ex As Exception
    lvPreview.Items.Clear()
    lvPreview.Items.Add(New ListViewItem({"エラー", ex.Message}))
End Try
End Sub

''' <summary>
''' 開くボタンクリック
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub btnOpen_Click(sender As Object, e As EventArgs) Handles btnOpen.Click
    If lvFiles.CheckedItems.Count = 0 Then Exit Sub
    Dim result As New List(Of FileGroup)
	If m_iArea<>0 AndAlso lvFiles.CheckedItems.Count>1
	'基準値領域への読込は単一グループのみ
		Dim sMsg As String = m_sStr(eMes.StandardOneOnly)
		Dim fMsg As New frmMSG(sMsg, {"OK"}, frmMain.m_sGuideMsgTitle)
		fMsg.StartPosition = FormStartPosition.CenterScreen
		fMsg.ShowDialog()
		fMsg.Dispose()
	Else
		For Each fg In m_checkedGroups.Values
			result.Add(fg)
		Next
		p_Selectfiles = result
		Me.DialogResult = DialogResult.OK
	End If
End Sub

''' <summary>
''' 中止ボタンクリック
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
    Me.DialogResult = DialogResult.Cancel
End Sub

''' <summary>
''' プレビューLVマウスクリック
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub lvPreview_MouseClick(sender As Object, e As MouseEventArgs) Handles lvPreview.MouseClick
	If m_showMarkedOnly Then Exit sub				'マーク行のみの表示の時は終了する
    Dim hit = lvPreview.HitTest(e.Location)
    If hit.Item Is Nothing Then Exit Sub
    Dim index As Integer = hit.Item.Index
    If m_highlightedLines.Contains(index) Then
        m_highlightedLines.Remove(index)
        hit.Item.BackColor = Color.White
    Else
        m_highlightedLines.Add(index)
        hit.Item.BackColor = Color.Yellow
    End If
End Sub

''' <summary>
''' フォルダツリービューへロード
''' </summary>
''' <param name="rootPath"></param>
Private Sub LoadTree(rootPath As String)
	tvFolders.BeginUpdate()
    tvFolders.Nodes.Clear()
    Dim rootNode As New TreeNode(IO.Path.GetFileName(rootPath))
    rootNode.Tag = rootPath
    tvFolders.Nodes.Add(rootNode)
    ' 再帰で構築
    AddDirectoryNodes(rootNode, rootPath, 0)
    ' 全展開
    tvFolders.ExpandAll()

	'レベル1のみ展開
	'ExpandLevel1(rootNode)




	Dim imgList As New ImageList()
	imgList.Images.Add(My.Resources.mmcndmgr_22)	'フォルダイメージアイコン
	tvFolders.ImageList = imgList
	tvFolders.ImageIndex = 0
	tvFolders.SelectedImageIndex = 0

	tvFolders.EndUpdate()
End Sub
''' <summary>
''' フォルダ名でノード追加 再帰
''' </summary>
''' <param name="parentNode">親ノード</param>
''' <param name="level">レベル</param>
Private Sub AddDirectoryNodes(parentNode As TreeNode, path As String, level As Integer)
    For Each dir As String In IO.Directory.GetDirectories(path)
        Dim dirName As String = IO.Path.GetFileName(dir)
        ' 階層フィルタ判定
        If Not IsAllowedByLevel(dirName, level) Then
            Continue For
        End If
        ' --- Measdata特別処理 ---
		Select Case level
		Case 1	' Measdataは表示しないが中身は展開
			If dirName.Equals("Measdata", StringComparison.OrdinalIgnoreCase) Then
            ' ノードは作らないが中身は展開
				AddDirectoryNodes(parentNode, dir, level + 1)
			End If
			Continue For
		Case Else
			' ノード作成
			Dim node As New TreeNode(dirName)
			node.Tag = dir   ' フルパス保持（後で使う場合に便利）
			parentNode.Nodes.Add(node)
			' 再帰で下位フォルダ処理
			AddDirectoryNodes(node, dir, level + 1)
		End Select
    Next
End Sub

Private Sub ExpandLevel1(rootNode As TreeNode)
    ' ルートは展開
    rootNode.Expand()
    ' level1（SZKXXX）だけ展開
    For Each node As TreeNode In rootNode.Nodes
        node.Expand()
    Next
End Sub
''' <summary>
''' 階層レベル毎のフィルタ条件処理
''' </summary>
''' <param name="folderName">処理するフォルダ名</param>
''' <param name="level">階層レベル</param>
''' <returns></returns>
Private Function IsAllowedByLevel(folderName As String, level As Integer) As Boolean
    Select Case level
    Case 0
        ' SZK000
        Return Regex.IsMatch(folderName, "^SZK\d{3}$", RegexOptions.IgnoreCase)
    Case 1
        ' Measdata
        'Return folderName.Equals("Measdata", StringComparison.OrdinalIgnoreCase)
		Return True
    Case 2
        ' GEL / GLA / LTM
        Return {"GEL", "GLA", "LTM"}.Contains(folderName, StringComparer.OrdinalIgnoreCase)
    Case 3
        ' 202310
        Return Regex.IsMatch(folderName, "^20\d{4}$")
    Case Else
        Return False
    End Select
End Function

''' <summary>
''' フォルダツリービュー選択後イベント
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Async Sub tvFolders_AfterSelect(sender As Object, e As TreeViewEventArgs) Handles tvFolders.AfterSelect

    Dim path = e.Node.Tag.ToString()
    m_currentDir = path

    lvFiles.Items.Clear()
    lvPreview.Clear()

    ' ★バックグラウンド処理
    Dim items = Await Task.Run(Function() CreateFileItems())

    ' ★UIスレッドで反映
    lvFiles.BeginUpdate()
    For Each item In items
        lvFiles.Items.Add(item)
    Next
    lvFiles.EndUpdate()

End Sub
''' <summary>
''' ファイルLV表示(非同期)
''' </summary>
''' <returns></returns>
Private Function CreateFileItems() As List(Of ListViewItem)
    Dim result As New List(Of ListViewItem)
    Try
        Dim files = IO.Directory.GetFiles(m_currentDir, "*." & sDataExt)
        Dim fileGroups As New Dictionary(Of String, List(Of IO.FileInfo))
        For Each f In files
            Dim fi As New IO.FileInfo(f)
            Dim baseName As String = fi.Name
            Dim idx As Integer = baseName.IndexOf("#"c)
            If idx >= 0 Then baseName = baseName.Substring(0, idx)
            If Not fileGroups.ContainsKey(baseName) Then
                fileGroups(baseName) = New List(Of IO.FileInfo)
            End If
            fileGroups(baseName).Add(fi)
        Next
        For Each kv In fileGroups.OrderBy(Function(x) x.Key)
            Dim baseName = kv.Key
            Dim group = kv.Value
            Dim fg As New FileGroup
            fg.BaseName = baseName
			fg.FolderPath = m_currentDir   'キー生成のため 追加
            For Each f In group.OrderBy(Function(x) x.Name)
                fg.Files.Add(f.FullName)
            Next
            Dim totalSize As Long = group.Sum(Function(f) f.Length)
            Dim baseFile = group.FirstOrDefault(Function(f) f.Name.Contains("#000000"))
            If baseFile Is Nothing Then
                baseFile = group.OrderBy(Function(f) f.Name).First()
            End If
            Dim item As New ListViewItem(baseName)
            item.SubItems.Add(baseFile.LastWriteTime.ToString("yyyy/MM/dd HH:mm"))
            item.SubItems.Add((totalSize \ 1024).ToString("N0") & " KB")
            ' ★測定時間
            Dim measureTime = GetMeasurementTimeSec(fg)
            item.SubItems.Add(measureTime.ToString("F1"))
            ' ★チェック復元
			Dim key As String = m_currentDir & "|" & baseName
			If m_checkedMap.Contains(key) Then
				item.Checked = True
				If Not m_checkedGroups.ContainsKey(key) Then
					m_checkedGroups(key) = fg
				End If
			End If
			item.Tag = fg
            result.Add(item)
        Next
    Catch
    End Try

    Return result

End Function
''' <summary>
''' キー生成
''' </summary>
''' <param name="fg">ファイルグループ</param>
''' <returns></returns>
Private Function GetGroupKey(fg As FileGroup) As String
    Return fg.FolderPath & "|" & fg.BaseName
End Function
''' <summary>
''' ファイル選択LVの初期化
''' </summary>
Private Sub LvInitialize()
	lvFiles.View			= View.Details
	lvFiles.FullRowSelect	= True
	lvFiles.GridLines		= True
	lvFiles.MultiSelect		= True
	lvFiles.CheckBoxes 		= True
	lvFiles.Columns.Clear()
	lvFiles.Columns.Add(m_sFileListColumn(0), 160)
	lvFiles.Columns.Add(m_sFileListColumn(1), 140)
	lvFiles.Columns.Add(m_sFileListColumn(2), 80, HorizontalAlignment.Right)
	lvFiles.Columns.Add(m_sFileListColumn(3), 100, HorizontalAlignment.Right)

End Sub
''' <summary>
''' ファイル選択LVへロードする
''' </summary>
Private Sub LoadFiles()
    lvFiles.Items.Clear()
    lvPreview.Clear()
    Try
        Dim files = IO.Directory.GetFiles(m_currentDir, "*." & sDataExt)
        ' グループ
        Dim fileGroups As New Dictionary(Of String, List(Of IO.FileInfo))
        For Each f In files
            Dim fi As New IO.FileInfo(f)
            Dim baseName As String = fi.Name
            Dim idx As Integer = baseName.IndexOf("#"c)
            If idx >= 0 Then
                baseName = baseName.Substring(0, idx)
            End If
            If Not fileGroups.ContainsKey(baseName) Then
                fileGroups(baseName) = New List(Of IO.FileInfo)
            End If
            fileGroups(baseName).Add(fi)
        Next
        ' 昇順
        For Each kv In fileGroups.OrderBy(Function(x) x.Key)
            Dim baseName = kv.Key
            Dim group = kv.Value
            ' ★ FileGroup生成
            Dim fg As New FileGroup
            fg.BaseName = baseName
            ' ★ フルパス格納（昇順）
            For Each f In group.OrderBy(Function(x) x.Name)
                fg.Files.Add(f.FullName)
            Next
            ' サイズ合計
            Dim totalSize As Long = group.Sum(Function(f) f.Length)
            ' #000000優先
            Dim baseFile = group.FirstOrDefault(Function(f) f.Name.Contains("#000000"))
            If baseFile Is Nothing Then
                baseFile = group.OrderBy(Function(f) f.Name).First()
            End If
            Dim item As New ListViewItem(baseName)
            item.SubItems.Add(baseFile.LastWriteTime.ToString("yyyy/MM/dd HH:mm"))
            item.SubItems.Add((totalSize \ 1024).ToString("N0") & " KB")
			'測定時間をセット
			Dim measureTime As Double = GetMeasurementTimeSec(fg)
			item.SubItems.Add(measureTime.ToString("F0"))
            ' ★ Tagにグループ保持
            item.Tag = fg
			' ★チェック状態復元
			If m_checkedMap.Contains(baseName) Then
				item.Checked = True
			End If
            lvFiles.Items.Add(item)
        Next
    Catch ex As Exception
    End Try
End Sub

''' <summary>
''' フォームリサイズイベント
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub Form1_Resize(sender As Object, e As EventArgs) Handles Me.Resize
Try
    Dim ratio1 As Double = 0.2
    Dim ratio2 As Double = 0.6
    ratio1 = SplitContainer1.SplitterDistance / SplitContainer1.Width
    ratio2 = SplitContainer2.SplitterDistance / SplitContainer2.Width
    SplitContainer1.SplitterDistance = CInt(SplitContainer1.Width * ratio1)
    SplitContainer2.SplitterDistance = CInt(SplitContainer2.Width * ratio2)
Catch ex As Exception
End Try
End Sub
'ファイルLVマウスダブルクリック
Private Sub lvFiles_DoubleClick(sender As Object, e As EventArgs) Handles lvFiles.DoubleClick
	btnOpen.PerformClick()
End Sub
'マーク行のみ表示切替チェックボックス
Private Sub ckbMarkOnlyDisp_CheckedChanged(sender As Object, e As EventArgs) Handles ckbMarkOnlyDisp.CheckedChanged
	m_showMarkedOnly = Not m_showMarkedOnly
	' 再読み込み
    lvFiles_SelectedIndexChanged(Nothing, Nothing)
End Sub
''' <summary>
''' マーククリアボタン
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub btnMarkClear_Click(sender As Object, e As EventArgs) Handles btnMarkClear.Click
	m_highlightedLines.Clear()
	' 再読み込み
    lvFiles_SelectedIndexChanged(Nothing, Nothing)
End Sub

''' <summary>
''' 測定時間取得（秒）
''' </summary>
''' 
Private Function GetMeasurementTimeSec(fg As FileGroup) As Double
    If fg Is Nothing Then Return 0
    ' ★キャッシュ確認
    If m_timeCache.ContainsKey(fg.BaseName) Then
        Return m_timeCache(fg.BaseName)
    End If
    Dim result As Double = 0
    Try
        Dim targetFile As String = fg.Files.Last()
        ' --- ヘッダ確認 ---
        For Each line In IO.File.ReadLines(targetFile)
            If line.StartsWith("Actual measurement Time[Sec]", StringComparison.OrdinalIgnoreCase) Then
                Dim parts = line.Split(","c)
                If parts.Length > 1 Then
                    Double.TryParse(parts(1).Trim(""""), result)
                    Exit For
                End If
            End If
            If line.Trim().Equals("[Data]", StringComparison.OrdinalIgnoreCase) Then Exit For
        Next
        ' --- 無い場合 ---
        If result = 0 Then
            Dim lastLine As String = Nothing
            For Each line In IO.File.ReadLines(targetFile)
                If Not String.IsNullOrWhiteSpace(line) Then
                    lastLine = line
                End If
            Next
            If lastLine IsNot Nothing Then
                Dim parts = lastLine.Split(","c)
                Dim ms As Double
                If Double.TryParse(parts(0), ms) Then
                    result = ms / 1000.0
                End If
            End If
        End If
    Catch
    End Try
    ' ★キャッシュ保存
    m_timeCache(fg.BaseName) = result
    Return result
End Function

''' <summary>
''' ファイル選択LVチェックボックスイベント
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub lvFiles_ItemChecked(sender As Object, e As ItemCheckedEventArgs) Handles lvFiles.ItemChecked
    Dim fg As FileGroup = CType(e.Item.Tag, FileGroup)
    If fg Is Nothing Then Exit Sub
    Dim key As String = GetGroupKey(fg)
    If e.Item.Checked Then
        m_checkedMap.Add(key)
        If Not m_checkedGroups.ContainsKey(key) Then
            m_checkedGroups(key) = fg
        End If
    Else
        m_checkedMap.Remove(key)
        m_checkedGroups.Remove(key)
    End If
End Sub



End Class