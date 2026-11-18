Public Class frmModify

    '▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼
    '   定数
    '▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲

    '▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼
    '   構造体
    '▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲

    '▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼
    '   API宣言
    '▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲

    '▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼
    '   パブリック変数
    '▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲

    Public p_stxtInput As String
    Public p_blReplaceCommaToUnderbar As Boolean = False            'カンマ使用不可（アンダーバーに置換）

    '▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼
    '   プライベート変数
    '▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲


    '■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
    '   コンストラクタ／デストラクタ
    '■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■

    '■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
    '   フォームのロード／アンロード／アクティブ
    '■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■

    'フォームLoad
    Private Sub frmModify_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            '言語切替
            Call SetFormLanguage(Me, p_iLanguage)

        Catch ex As Exception
            Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
            Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
        End Try
    End Sub

    'フォームShown
    Private Sub frmModify_Shown(sender As Object, e As EventArgs) Handles MyBase.Shown
        Try
            '変数初期化
            p_stxtInput = ""

            '入力を全選択
            txtInput.Focus()
            txtInput.SelectAll()

        Catch ex As Exception
            Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
            Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
        End Try
    End Sub

    '■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
    '   ボタンイベント
    '■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■

    '[OK]ボタンクリックイベント
    Private Sub btnOK_Click(sender As Object, e As EventArgs) Handles btnOK.Click
        Try
            '入力を更新
            If p_blReplaceCommaToUnderbar = True Then
                'カンマ使用不可（アンダーバーに置換）
                p_stxtInput = txtInput.Text.Replace(",", "_")
            Else
                p_stxtInput = txtInput.Text
            End If
            '画面を閉じる
            Me.Close()

        Catch ex As Exception
            Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
            Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
        End Try
    End Sub

    '[Cancel]ボタンクリックイベント
    Private Sub btnCan_Click(sender As Object, e As EventArgs) Handles btnCan.Click
        Try
            '画面を閉じる
            Me.Close()

        Catch ex As Exception
            Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
            Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
        End Try
    End Sub

    '■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
    '   コントロールイベント
    '■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■

    '入力変更イベント
    Private Sub txtInput_TextChanged(sender As Object, e As EventArgs) Handles txtInput.TextChanged
        Try
            '入力が無い場合、登録できないようにする
            If txtInput.Text.Length = 0 Then
                btnOK.Enabled = False
            Else
                btnOK.Enabled = True
            End If

        Catch ex As Exception
            Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
            Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
        End Try
    End Sub

    '■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
    '   パブリック関数
    '■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■

    '■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
    '   プライベート関数
    '■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■

End Class