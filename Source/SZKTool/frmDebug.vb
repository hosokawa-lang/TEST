Imports System.ComponentModel
Imports System.Drawing.Printing
Imports System.Threading
Imports System.IO
Imports System.Windows.Forms.DataVisualization.Charting
Imports Microsoft.VisualBasic.FileIO
Imports System.Collections
Imports System.Globalization
Imports SZKTool.frmOptions
Imports SZKTool.RealTimeMeas
Imports SZKTool.modPublic
Imports SZKTool.MemLinkTCP
Imports System.Runtime.CompilerServices

Public Class frmDebug
Public Const cAREA_FILE As String = "SizukaArea_5.txt"
'保持アドレス設定種別
Private Enum eSET_HOLD
	iADDR_ONLY = 0
	iROW_ONLY
	iALL
End Enum
'表示列情報
Enum eDGV_AREA_CLM
	iAREA = 0               'アドレス
	iITEM                   '項目名
	iVALUE                  '値
	iINC                    '増加量
End Enum
'新規
Public Sub New()
	' この呼び出しはデザイナーで必要です。
	InitializeComponent()
	' InitializeComponent() 呼び出しの後で初期化を追加します。
End Sub

'フォームロード
Private Sub frmDebug_Load(sender As Object, e As EventArgs) Handles MyBase.Load
	Call sbSetArea()                'デバッグ用データグリッドビューにエリア情報設定
	Timer1.Interval = 200
	Timer1.Enabled = True
End Sub
''' <summary>
''' リストビューへ読込制御変数を表示
''' </summary>
''' <returns></returns>
Public Function ReadValueDisp() As Int32
Dim iCnt As Int32
Dim iAdr As Int32
Dim iData As Int32
Dim sData As String = Nothing
Try
	While iCnt < dgvArea.RowCount
		iAdr = dgvArea(eDGV_AREA_CLM.iAREA, iCnt).Value                             'リンクアドレス取得
		Select Case iAdr
		Case >= R_TOP_LNK_ADR
			iData = p_RTM(0).p_mltcp.m_iRAry(iAdr - R_TOP_LNK_ADR)
		Case Else
			iData = p_RTM(0).p_mltcp.m_iWAry(iAdr - W_TOP_LNK_ADR)
		End Select
		Select Case iDataDisp
		Case 0	'10進
			sData = iData.ToString()
		Case 1	'16進
			sData = String.Format("{0:X4}", Ctype(iData, UInt16))
		End Select
		dgvArea(eDGV_AREA_CLM.iVALUE, iCnt).Value = sData					'リストビュー値セット
		iCnt += 1        '行カウント+1
	End While
Catch ex As Exception
	Debug.Print(ex.Message)
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
Return 0
End Function
delegate Function ReadValueDispDelegate() As Int32
Dim iDataDisp As Int16 = 0			'表示方式 (0：10進数 1：16進数)
'10進/16進 表示切替
Private Sub btnHex_Click(sender As Object, e As EventArgs) Handles btnHex.Click
Try
	iDataDisp = IIf(iDataDisp = 0, 1, 0)
Catch ex As Exception
	Debug.Print(ex.Message)
End Try
End Sub
''' <summary>
''' リストビューから書込み保持変数へセット
''' </summary>
''' <returns></returns>
Public Function WriteValueSet() As Int32
	Dim i       As Int32
	Dim iCnt    As Int32
	Dim iAdr    As Int32
	Dim iData       As Int32 = -1
	Dim iTime(2)    As UInt32
	For i = 0 To iTime.GetUpperBound(0) - 1
		iTime(i) = 0
	Next
	Try
		While iCnt < dgvArea.RowCount
			With p_RTM(0)
			iAdr = dgvArea(eDGV_AREA_CLM.iAREA, iCnt).Value                             'リンクアドレス取得
			Select Case iAdr
			Case MemLinkTCP.eMEMADR.iTEMP_PRS
				.p_mltcp.TempPreset(0) = dgvArea(eDGV_AREA_CLM.iVALUE, iCnt).Value
			Case MemLinkTCP.eMEMADR.iGAP_PRS
				.p_mltcp.GapPreset(0) = dgvArea(eDGV_AREA_CLM.iVALUE, iCnt).Value
			Case MemLinkTCP.eMEMADR.iMES_WAIT
				.p_mltcp.MeasWaitTime(0) = dgvArea(eDGV_AREA_CLM.iVALUE, iCnt).Value
			Case MemLinkTCP.eMEMADR.iCLR_TIME
				.p_mltcp.ClearTime(0) = dgvArea(eDGV_AREA_CLM.iVALUE, iCnt).Value
			Case MemLinkTCP.eMEMADR.iTMP_RNG
				.p_mltcp.TempOkRange(0) = dgvArea(eDGV_AREA_CLM.iVALUE, iCnt).Value
			Case MemLinkTCP.eMEMADR.iMAIN_UNIT_SERIAL
				.p_mltcp.DevSereal = dgvArea(eDGV_AREA_CLM.iVALUE, iCnt).Value
			Case MemLinkTCP.eMEMADR.iLED_LIGHT
					.p_mltcp.LedOnOff = dgvArea(eDGV_AREA_CLM.iVALUE, iCnt).Value
			Case MemLinkTCP.eMEMADR.iHEAT_P_PID
					.p_mltcp.P_PID(0) = dgvArea(eDGV_AREA_CLM.iVALUE, iCnt).Value
			Case MemLinkTCP.eMEMADR.iHEAT_I_PID
					.p_mltcp.I_PID(0) = dgvArea(eDGV_AREA_CLM.iVALUE, iCnt).Value
			Case MemLinkTCP.eMEMADR.iHEAT_D_PID
					.p_mltcp.D_PID(0) = dgvArea(eDGV_AREA_CLM.iVALUE, iCnt).Value
			Case MemLinkTCP.eMEMADR.iHEAT_DGAIN
					.p_mltcp.DGain(0) = dgvArea(eDGV_AREA_CLM.iVALUE, iCnt).Value
			Case MemLinkTCP.eMEMADR.iHEAT_PVBIAS
					.p_mltcp.PVBias(0) = dgvArea(eDGV_AREA_CLM.iVALUE, iCnt).Value
			Case MemLinkTCP.eMEMADR.iSTANBY_TIME
					.p_mltcp.StanbyTime = dgvArea(eDGV_AREA_CLM.iVALUE, iCnt).Value
			Case MemLinkTCP.eMEMADR.iIN_OPERATION
					.p_mltcp.InOperation = dgvArea(eDGV_AREA_CLM.iVALUE, iCnt).Value
			Case MemLinkTCP.eMEMADR.iCONFIG1
					.p_mltcp.Config1 = dgvArea(eDGV_AREA_CLM.iVALUE, iCnt).Value
			Case MemLinkTCP.eMEMADR.iSTART_TEMP_CONT
					.p_mltcp.StartTempCont = dgvArea(eDGV_AREA_CLM.iVALUE, iCnt).Value
			Case MemLinkTCP.eMEMADR.iSTART_SW
					.p_mltcp.StartSW = dgvArea(eDGV_AREA_CLM.iVALUE, iCnt).Value
			Case MemLinkTCP.eMEMADR.iERR_RESET
					.p_mltcp.ErrReset = dgvArea(eDGV_AREA_CLM.iVALUE, iCnt).Value
			Case MemLinkTCP.eMEMADR.iHEATER_LIMIT
					.p_mltcp.HeaterUpper(0) = dgvArea(eDGV_AREA_CLM.iVALUE, iCnt).Value
			Case MemLinkTCP.eMEMADR.iINP_GAP
					.p_mltcp.InpGap(0) = dgvArea(eDGV_AREA_CLM.iVALUE, iCnt).Value
			Case MemLinkTCP.eMEMADR.iMES_PRE_WAIT
					.p_mltcp.MesPreWait(0) = dgvArea(eDGV_AREA_CLM.iVALUE, iCnt).Value
			Case MemLinkTCP.eMEMADR.iACT_MODE
					.p_mltcp.ActMode = dgvArea(eDGV_AREA_CLM.iVALUE, iCnt).Value
			Case MemLinkTCP.eMEMADR.iAMPLITUDE_X
					.p_mltcp.AmplitudeX = dgvArea(eDGV_AREA_CLM.iVALUE, iCnt).Value
			Case MemLinkTCP.eMEMADR.iAMPLITUDE_Y
					.p_mltcp.AmplitudeY = dgvArea(eDGV_AREA_CLM.iVALUE, iCnt).Value
			Case MemLinkTCP.eMEMADR.iPERIOD_X
					.p_mltcp.PeriodX(0) = dgvArea(eDGV_AREA_CLM.iVALUE, iCnt).Value
			Case MemLinkTCP.eMEMADR.iPERIOD_Y
					.p_mltcp.PeriodY(0) = dgvArea(eDGV_AREA_CLM.iVALUE, iCnt).Value

			Case MemLinkTCP.eMEMADR.iPOS_HEATB_L
				If iData = -1 then
					iData = dgvArea(eDGV_AREA_CLM.iVALUE, iCnt).Value
				else
					.p_mltcp.PosHeatB = MemLinkTCP.DWordFromWords( iData, dgvArea(eDGV_AREA_CLM.iVALUE, iCnt).Value)
					iData = -1
				End If
			Case MemLinkTCP.eMEMADR.iPOS_HEATB_H
				If iData = -1 then
					iData = dgvArea(eDGV_AREA_CLM.iVALUE, iCnt).Value
				else
					.p_mltcp.PosHeatB = MemLinkTCP.DWordFromWords( dgvArea(eDGV_AREA_CLM.iVALUE, iCnt).Value, iData )
					iData = -1
				End If

			Case MemLinkTCP.eMEMADR.iPOS_X
					.p_mltcp.PosX = dgvArea(eDGV_AREA_CLM.iVALUE, iCnt).Value
			Case MemLinkTCP.eMEMADR.iPOS_Y
					.p_mltcp.PosY = dgvArea(eDGV_AREA_CLM.iVALUE, iCnt).Value
			Case MemLinkTCP.eMEMADR.iPOS_ORDER
					.p_mltcp.PosOrder = dgvArea(eDGV_AREA_CLM.iVALUE, iCnt).Value

#If 0
			Case MemLinkTCP.eMEMADR.iTIME_SET_1
				iTime(0) = dgvArea(eDGV_AREA_CLM.iVALUE, iCnt).Value
			Case MemLinkTCP.eMEMADR.iTIME_SET_2
				iTime(1) = dgvArea(eDGV_AREA_CLM.iVALUE, iCnt).Value
			Case MemLinkTCP.eMEMADR.iTIME_SET_3
				iTime(2) = dgvArea(eDGV_AREA_CLM.iVALUE, iCnt).Value
				.p_mltcp.TimeSet = iTime

			Case MemLinkTCP.eMEMADR.iTIME_SET_ORDER
					.p_mltcp.DateSetOrder = dgvArea(eDGV_AREA_CLM.iVALUE, iCnt).Value

#end if
			Case Else
			End Select
			End With
			iCnt += 1        '行カウント+1
		End While
	Catch ex As Exception
		Debug.Print(ex.Message)
		Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	End Try
	Return 0
End Function
delegate Function WriteValueSetDelegate() As Int32


'================================================
'エリア表示データグリッドビュー設定
'引数	なし
'戻り値	なし
'================================================
Private Sub sbSetArea()
	Try
		'パスの設定
		Dim sPath As String = My.Application.Info.DirectoryPath & "\" & cAREA_FILE
		'パスの存在確認
		If System.IO.File.Exists(sPath) = False Then
			Exit Sub
		End If
		'ファイル読み込み
		Using parser As New FileIO.TextFieldParser(sPath, System.Text.Encoding.UTF8)
			parser.TextFieldType = FileIO.FieldType.Delimited   '区切り形式
			parser.SetDelimiters("=")                           '区切り文字は「=」
			parser.TrimWhiteSpace = True                        '空白はトリム

			Dim sReadRow() As String
			Dim sPrmVal() As String
			While Not parser.EndOfData
				'ファイルから1行読み込み
				sReadRow = parser.ReadFields()

				'1行配列が2以上の場合のみパラメータを取得
				If sReadRow.Length > 1 Then
					If sReadRow(0) = "Param" Then
						'2要素目をコンマで区切る
						sPrmVal = Split(sReadRow(1), ",")

						'要素数が1以上かつ、エリアが数値変換できる
						If sPrmVal.Length > 0 AndAlso
							IsNumeric(sPrmVal(eDGV_AREA_CLM.iAREA)) = True Then

							'行の追加
							dgvArea.Rows.Add()

							'表示の初期化
							Dim iCnt As Integer
							For iCnt = 0 To dgvArea.ColumnCount - 1
								dgvArea(iCnt, dgvArea.Rows.Count - 1).Value = ""
							Next

							'設定値の表示
							With dgvArea.Rows(dgvArea.Rows.Count - 1)
								For iCnt = 0 To sPrmVal.Length - 1
									If iCnt > dgvArea.ColumnCount - 1 Then                  '列数を超える場合ループを抜ける
										Exit For
									End If

									.Cells(iCnt).Value = sPrmVal(iCnt)                      '設定値の表示
								Next
							End With
						End If
					End If
				End If
			End While

		End Using
	Catch ex As Exception
		Debug.Print(ex.Message)
	End Try
End Sub

'================================================
'エリア表示データグリッドビュー保存
'引数	なし
'戻り値	なし
'================================================
Private Sub sbOutArea()
	Dim sPath As String
	Dim sTemp As String

	Try
		'パスの設定
		sPath = My.Application.Info.DirectoryPath & "\" & cAREA_FILE

		'ファイル書き込み
		Using sWriter As New IO.StreamWriter(sPath, False, System.Text.Encoding.UTF8)
			sWriter.WriteLine("[SIZUKA AREA]")
			sWriter.WriteLine("#Param=Address(,Disp.Name,Init.Value,Increase)")
			For iCnt = 0 To dgvArea.RowCount - 1
				'書込み行の設定
				sTemp = "Param="
				For iCnt2 As Integer = 0 To dgvArea.ColumnCount - 1
					sTemp = sTemp & CStr(dgvArea(iCnt2, iCnt).Value) & ","
				Next
				'最終のコンマを除く
				sTemp = Strings.Left(sTemp, sTemp.Length - 1)
				sWriter.WriteLine(sTemp)
			Next
		End Using
	Catch ex As Exception
		Debug.Print(ex.Message)
	End Try
End Sub


'オブジェクトを文字列に変換
Public Function fnConvertStr(ByVal oObj As Object) As String
	Dim sRtn As String = ""

	Try
		If IsNothing(oObj) = False Then
			sRtn = oObj
		End If
	Catch ex As Exception
		sRtn = ""
	End Try

	Return sRtn
End Function

'指定行の指定ビットを取得
Private Function fnGetBitByVal(ByVal iRow As Integer, ByVal iBit As Integer) As Boolean
	Dim bRtn As Boolean = False

	Try
		'行番号、ビットの確認
		If iRow < 0 OrElse iRow >= dgvArea.Rows.Count OrElse
			iBit < 0 Then
			Return ""
		End If

		'現在値を取得
		Dim sTemp As String = fnConvertStr(dgvArea(clmVal.Name, iRow).Value)
		Dim iTemp As Integer

		'数値に変換可能かどうか判定
		If sTemp.Length > 0 AndAlso IsNumeric(sTemp) = True Then
			iTemp = CInt(sTemp)

			'指定ビットのみを取り出し
			iTemp = iTemp And Math.Pow(2, iBit)

			'0のときはビットがOFF
			If iTemp = 0 Then
				bRtn = False
			Else
				bRtn = True
			End If
		End If
	Catch ex As Exception
		bRtn = False
	End Try

	Return bRtn
End Function

'指定行の指定ビットを設定して、値を取得
Private Function fnSetBit(ByVal iRow As Integer, ByVal iBit As Integer, ByVal bFlg As Boolean) As String
	Dim sRtn As String = ""

	Try
		'行番号、ビットの確認
		If iRow < 0 OrElse iRow >= dgvArea.Rows.Count OrElse
			iBit < 0 Then
			Return ""
		End If

		'現在値を取得
		Dim sTemp As String = fnConvertStr(dgvArea(clmVal.Name, iRow).Value)
		Dim iTemp As Integer

		'数値に変換可能かどうか判定
		If sTemp.Length > 0 AndAlso IsNumeric(sTemp) = True Then
			iTemp = CInt(sTemp)

			If bFlg = True Then
				'ON
				iTemp = iTemp Or Math.Pow(2, iBit)
			Else
				'OFF
				iTemp = iTemp And Not (Math.Pow(2, iBit))
			End If

		End If

		sRtn = CStr(iTemp)

		If sRtn.Length > 0 Then
			dgvArea(clmVal.Name, iRow).Value = sRtn
		End If
	Catch ex As Exception
		sRtn = ""
	End Try

	Return sRtn
End Function
'フォームクローズ
Private Sub frmDebug_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
	'sbOutArea()
End Sub
'タイマーインターバル
Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
	ReadValueDisp()                                         'リストビュー表示更新(デバッグ用 CTRL+Kで表示)

	lblBGWOrder.Text		= p_RTM(0).p_iBGWOrder
	lblBGWOrderNext.Text	= p_RTM(0).p_iBGWOrderNext
	lblMeasureState.Text	= p_RTM(0).p_MeasureState
	lblCalJob.Text			= p_RTM(0).CalJob
	lblDevErr.Text			= p_RTM(0).p_iDevErrorCount



End Sub
'最上位画面設定
Private Sub cbTopMost_CheckedChanged(sender As Object, e As EventArgs) Handles cbTopMost.CheckedChanged
	Me.TopMost = Not Me.TopMost
End Sub

End Class