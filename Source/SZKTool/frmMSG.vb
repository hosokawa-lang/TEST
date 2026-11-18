''' <summary>
''' メッセージボックスクラス
''' </summary>
Public Class frmMSG
''' <summary>
''' 2個 Ret 1/2, 3個 Ret 2/3/4, 4個 Ret 1/2/3/4  
''' </summary>
Public p_iRes			As Integer = -1
Private m_sMsg			As String
Private m_sBtn()		As String
Private m_sTitle		As String
Private m_frmParent		As Form = Nothing
Private m_iDefBtn		As int16

''' <summary>
''' クラス生成
''' </summary>
''' <param name="sMsg">表示メッセージ(文字列)</param>
''' <param name="sBtn">ボタン文字(文字列配列)</param>
''' <param name="sTitle">タイトル文字列</param>
''' <param name="Parent">親フォーム名(Option)</param>
Public Sub New(ByVal sMsg As String, ByVal sBtn() As String, ByVal sTitle As String, Optional ByVal Parent As Form = Nothing, Optional ByVal iDefBtn As Int16 = 0)
    '引数を取得
    m_sTitle = sTitle
    m_sMsg = sMsg
    ReDim m_sBtn(sBtn.Length - 1)
    Array.Copy(sBtn, m_sBtn, m_sBtn.Length)
    m_frmParent = Parent
	m_iDefBtn	= iDefBtn
    ' この呼び出しはデザイナーで必要です。
    InitializeComponent()
    ' InitializeComponent() 呼び出しの後で初期化を追加します。
End Sub

'フォームLoadイベント
Private Sub frmMSG_Load(sender As Object, e As EventArgs) Handles MyBase.Load
Dim iBtnHeight		As int16 = 36		'ボタン高さ
Dim iBtnInterval	As int16 = 10		'ボタン間隔ピクセル

Try
    '押されたボタン結果初期化
    p_iRes = -1

    '表示更新
    'タイトルを設定
    If m_sTitle.Length > 0 Then
        Me.Text = m_sTitle
    Else
        Me.Text = "Message"
    End If

	'表示メッセージ設定
	lblMsg.TextAlign	= ContentAlignment.MiddleCenter
    lblMsg.Text			= m_sMsg

	'表示ボタン設定
	'Dim fFonts As New Font(BASE_FONT, 12)
	For Each btn As Button In Panel1.Controls
		btn.Font = lblMsg.Font
	Next

    btn1.Visible = False
    btn2.Visible = False
    btn3.Visible = False
    btn4.Visible = False
    btn5.Visible = False
    btn6.Visible = False
    btn7.Visible = False
    btn1.Height	=iBtnHeight
    btn2.Height	=iBtnHeight
    btn3.Height	=iBtnHeight
    btn4.Height	=iBtnHeight
    btn5.Height	=iBtnHeight
    btn6.Height	=iBtnHeight
    btn7.Height	=iBtnHeight
    btn1.Tag = 1
    btn2.Tag = 1
    btn3.Tag = 2
    btn4.Tag = 1
    btn5.Tag = 2
    btn6.Tag = 3
    btn7.Tag = 4
    Select Case m_sBtn.Length
    Case 1  'ボタン1個指定
		btn1.Left = (Me.Width/2) - (btn1.Width/2)
        btn1.Visible = True
        btn1.Text = m_sBtn(0)
    Case 2 'ボタン２個指定
		btn2.Left = Me.Width/2 - (iBtnInterval/2 + btn2.Width )
		btn3.Left = Me.Width/2 + iBtnInterval/2
		Select Case m_iDefBtn
		Case 0
	        btn2.Select()
		Case Else
	        btn3.Select()
		End Select
        btn2.Visible = True
        btn3.Visible = True
        btn2.Text = m_sBtn(0)		'1
        btn3.Text = m_sBtn(1)		'2
    Case 3 'ボタン３個指定
		btn5.Left = Me.Width/2 - (btn5.Width/2 + iBtnInterval + btn5.Width )
		btn6.Left = Me.Width/2 - btn5.Width/2
		btn7.Left = Me.Width/2 + (btn5.Width/2 + iBtnInterval)
		Select Case m_iDefBtn
		Case 0
	        btn5.Select()
		Case 1
	        btn6.Select()
		Case Else
	        btn7.Select()
		End Select
        btn5.Visible = True
        btn6.Visible = True
        btn7.Visible = True
        btn5.Text = m_sBtn(0)		'2
        btn6.Text = m_sBtn(1)		'3
        btn7.Text = m_sBtn(2)		'4
    Case Is >= 4 'ボタン4個以上
        btn4.Select()
		Select Case m_iDefBtn
		Case 0
	        btn4.Select()
		Case 1
	        btn5.Select()
		Case 2
	        btn6.Select()
		Case Else
	        btn7.Select()
		End Select
        btn4.Visible = True
        btn5.Visible = True
        btn6.Visible = True
        btn7.Visible = True
        btn4.Text = m_sBtn(0)		'1
        btn5.Text = m_sBtn(1)		'2
        btn6.Text = m_sBtn(2)		'3
        btn7.Text = m_sBtn(3)		'4
    End Select

	'親フォームが指定されている場合、無効化
	If IsNothing(m_frmParent) = False Then
		m_frmParent.Enabled = False
	End If
	Me.TopMost = True
	Timer1.Interval = 200
	Timer1.Enabled  = True

Catch ex As Exception
    Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
    Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub

'フォームClosingイベント
Private Sub frmMSG_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
Try
    '親フォームを有効化
    If IsNothing(m_frmParent) = False Then
        m_frmParent.Enabled = True
    End If

Catch ex As Exception
    Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
    Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub

'ボタンクリックイベント
Private Sub btn_Click(sender As Object, e As EventArgs) Handles btn1.Click, btn2.Click, btn3.Click, btn4.Click, btn5.Click, btn6.Click, btn7.Click
Try
    Dim btnTemp As Button = CType(sender, Button)
    'ボタン結果
    p_iRes = btnTemp.Tag
    '画面を閉じる
    Me.Close()
Catch ex As Exception
    Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
    Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub
'装置エラーの監視
Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
	If p_Rtm(0).p_bDevError
	    p_iRes = 999
		'画面を閉じる
		Me.Close()
	End If
End Sub
End Class