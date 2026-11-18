Imports System.IO
Imports System.Text
Public Class frmMent


' ログファイルが保存されているフォルダパス
' ※実行ファイル(exe)と同じ階層の "Maintenance" フォルダを指定
Private ReadOnly _basePath As String = Path.Combine(SIZUKA_PATH, p_sEquipName, "Maintenance")
'多言語メッセージ格納
Private m_sStr(eMes.iMAX - 1)		 As String   
Enum eMes
	iClear = 0
	iMAX
End Enum

''' <summary>
''' フォームロード時の処理
''' </summary>
Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
    InitializeForm()
End Sub

''' <summary>
''' 初期化処理：フォルダ確認と日付リストの生成
''' </summary>
Private Sub InitializeForm()
	'言語指定による
    Call SetFormLanguage(Me, p_iLanguage,,m_sStr)

    ' フォルダが存在しない場合は作成（またはエラー表示）
    If Not Directory.Exists(_basePath) Then
        Directory.CreateDirectory(_basePath)
        ' テスト用に空ファイルを作成しない場合は、ここでの処理は不要です
    End If

    ' コンボボックスの初期化
    cboDateList.Items.Clear()

    ' *_PC.txt ファイルを基準に日付一覧を取得する
    ' ※PC情報は必ずあるという前提ですが、全ファイルを走査して重複排除する方法もあります
    Dim files As String() = Directory.GetFiles(_basePath, "*_PC.txt")
    Dim dateList As New List(Of String)()

    For Each filePath As String In files
        ' ファイル名のみ取得 (例: 20231201_PC.txt)
        Dim fileName As String = Path.GetFileName(filePath)
            
        ' "_" で分割して日付部分を取得 (例: 20231201)
        Dim parts As String() = fileName.Split("_"c)
        If parts.Length > 0 Then
            Dim dateStr As String = parts(0)
            dateList.Add(dateStr)
        End If
    Next

    ' 日付を降順（新しい順）にソートしてコンボボックスに追加
    dateList.Sort()
    dateList.Reverse()
    cboDateList.Items.AddRange(dateList.ToArray())

    ' データがある場合は先頭（最新）を選択
    If cboDateList.Items.Count > 0 Then
        cboDateList.SelectedIndex = 0
    End If
End Sub

''' <summary>
''' 日付選択変更時の処理
''' </summary>
Private Sub cboDateList_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboDateList.SelectedIndexChanged
    Dim selectedDate As String = cboDateList.SelectedItem.ToString()

    ' 各ファイルのパスを生成
    Dim pathPC As String = Path.Combine(_basePath, $"{selectedDate}_PC.txt")
    Dim pathVer As String = Path.Combine(_basePath, $"{selectedDate}_Ver.txt")
    Dim pathMnt As String = Path.Combine(_basePath, $"{selectedDate}_Mnt.txt")

    ' ファイル読み込みと表示
    txtPC.Text = ReadFileContent(pathPC)
    txtVer.Text = ReadFileContent(pathVer)
    txtMnt.Text = ReadFileContent(pathMnt)
End Sub

''' <summary>
''' テキストファイルを読み込むヘルパー関数
''' </summary>
''' <param name="filePath">ファイルパス</param>
''' <returns>ファイルの内容。ファイルがない場合はメッセージを返す。</returns>
Private Function ReadFileContent(filePath As String) As String
    If File.Exists(filePath) Then
        Try
            ' エンコード指定 (UTF-8の場合)
            ' ※Shift-JISの場合は Encoding.GetEncoding("Shift_JIS") を使用
            Return File.ReadAllText(filePath, Encoding.UTF8)
        Catch ex As Exception
            Return "読み込みエラー: " & ex.Message
        End Try
    Else
        Return "(情報なし)"
    End If
End Function

End Class