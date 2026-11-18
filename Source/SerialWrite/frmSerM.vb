Imports SerialWrite.clMelTcp

Public Class frmSerM
Private mltcp				As New clMelTcp				'メモリリンク通信を行うクラス
Private m_sEquipName		As String
Public p_bInterrupt			As Boolean = False          '※割り込みを使用する場合、1スレッド内で行い、フラグがOFFになっていることを確認すること！！
Public p_bytRInterrupt(256) As Byte
Public p_bytWInterrupt()    As Byte

Private Sub frmMain_Load(sender As Object, e As EventArgs) Handles MyBase.Load
	tbxIP.Visible	= False
	tbxIP.Text		= "localhost"
	Timer1.Interval = 200
	Timer1.Start
End Sub

''' <summary>
''' 書き込みベリファイシーケンス開始 ボタン
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub btnWriteVeri_Click(sender As Object, e As EventArgs) Handles btnWriteVeri.Click
	m_Job = 100
End Sub

Dim m_Job As Int32 = 0			'シーケンス制御変数
''' <summary>
''' タイマーインターバル処理
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
	Timer1.Stop
	Select Case m_Job
	Case 0
	'アイドル
	Case 100
	'接続
		lblConnOK.Text		= ""
		lblWriteVeriOK.Text = ""
		Dim frmCon  As frmWaitMes = New frmWaitMes	
		Dim sErr	As String = Nothing
		frmCon.p_sMes = "Connecting..."
		frmCon.Show()
		frmCon.Refresh
		Dim sIP As String = If(CheckBox1.Checked=False,"192.168.10." & nudNode.Value,Replace(tbxIP.Text,Space(1),""))
		If mltcp.TCP_Connect(sIP, 1024, sErr) Then
		'接続できた
			lblConnOK.Text = "Connect OK"
			lblConnOK.ForeColor = Color.Blue
			m_Job += 100
		Else
			lblConnOK.Text = "Connect NG"
			lblConnOK.ForeColor = Color.Red
		'指定したIPアドレスの装置に接続できなかったが起動は継続
			MessageBox.Show("Could not connect to the specified IP address. Check the settings of SIZUKA.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error)
			m_Job = 0
		End If
		frmCon.Dispose()

	Case 200
	'番号 書き込み
		mltcp.DevSereal = nudSerial.Value													'番号セット
		mltcp.WriteOneWord(eMEMADR.iMAIN_UNIT_SERIAL, mltcp.DevSereal)						'[[[[書き込み]]]]
		m_Job += 100

	Case 300
	'トリガ 書き込み
		mltcp.DateSetOrder = mltcp.DateSetOrder Or clMelTcp.eWDateOrderBit.iDEVISE_SERIAL_WRITE		'書き込みトリガON
		mltcp.WriteOneWord(eMEMADR.iTIME_SET_ORDER, mltcp.DateSetOrder)						'[[[[書き込み]]]]
		m_Job += 100

	Case 400
		mltcp.DateSetOrder = mltcp.DateSetOrder AND Not clMelTcp.eWDateOrderBit.iDEVISE_SERIAL_WRITE		'書き込みトリガOFF
		mltcp.WriteOneWord(eMEMADR.iTIME_SET_ORDER, mltcp.DateSetOrder)						'[[[[書き込み]]]]
		m_Job += 100

	Case 500
	'番号 読込み
		mltcp.ReadBatch()																	'[[[[一括読込(通常)]]]]
		m_sEquipName			= mltcp.DevSerealRead										'機器製造番号を取得
		If CInt(m_sEquipName) = nudSerial.Value
			lblWriteVeriOK.Text = "Verify OK -> " & m_sEquipName
			lblWriteVeriOK.ForeColor = Color.Blue
		Else
			lblWriteVeriOK.Text = "Verify NG -> " & m_sEquipName
			lblWriteVeriOK.ForeColor = Color.Red
		End If
		m_Job += 100

	Case 600
	'終了
		mltcp.TCP_DisConnect()
		m_Job = 0

	Case Else
		m_Job = 0
	End Select
	Timer1.Start
End Sub

''' <summary>
''' IPアドレス固定/フリー入力 切替え
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged
	tbxIP.Visible	= Not tbxIP.Visible
	nudNode.Visible	= Not nudNode.Visible
End Sub




'================================================
'整数を2つのByteに変換
'引数	iNum:			変換する整数
'		bByt:			変換後のByte配列
'		iNum:			変換するByte配列数
'戻り値 true:			変換成功
'		false:			変換失敗
'================================================
Public Function ConvIntToByteArry(ByVal iInt As Integer, ByRef bByt() As Byte, Optional ByVal iNum As Integer = 0) As Boolean
	Dim bRtn As Boolean = True
	Dim sTemp As String
	Dim sTemp2 As String
	Dim iLen As Integer
	Dim iCnt As Integer

	Try
		'16進数に変換
		sTemp = Convert.ToString(iInt, 16)

		'偶数桁でない場合、頭に0を付与
		For iCnt = sTemp.Length To iNum * 2 - 1
			sTemp = "0" & sTemp
		Next
		iLen = sTemp.Length

		'配列数が未指定の場合、全ての桁として変換する
		If iNum <= 0 Then
			iNum = (iLen / 2)
		End If

		'配列の確保
		ReDim bByt(iNum - 1)
		'配列の初期化
		For iCnt = 0 To iNum - 1
			bByt(iCnt) = 0
		Next

		'16進数文字列の下位からByte化していく
		For iCnt = 0 To iNum - 1
			'下位桁から順に2桁ずつ16進数文字列を取得
			sTemp2 = Mid(sTemp, iLen - (iCnt * 2 + 1), 2)
			'Byte型に変換
			bByt(iCnt) = CByte("&H" & sTemp2)
		Next

	Catch ex As Exception
		Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
		bRtn = False
	End Try

	Return bRtn
End Function
End Class
