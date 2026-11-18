'============================================
'File:   modInpCheck.vb
'Name:   SIZUKA Tool SPEC入力範囲検査 MADOKA modDBstrct.vbより移植
'============================================

'▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼
'   定数
'▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲
#If Not NEW_SPEC
'''<summary>
'''DefaultSpecの行並び
'''</summary>
Public Enum DefSPEC
    RESULTID = 0
    MEASDATE                    '測定日時
    EQUIPNAME                   '装置番号
    RESINNAME                   '樹脂名    
    LOTNUM                      'Lot No
    SAMPLENUM                   'サンプル番号(測定連番)
    XYMODE                      'XY動作モード
    TEMPERATURE = 7             '熱板 設定温度
    TEMPERATURE2                'プローブ 設定温度
    MEASTHR                     '応力判定値
    INPGAP      = 10            '投入ギャップ
    GAP                         'ギャップ値

    XAMP		= 12            'X軸振幅
    YAMP                        'Y軸振幅
    XPERIOD                     'X軸周期
    YPERIOD                     'Y軸周期

    WAIT                        '測定待ち時間
    CLEAR                       '投入待ち時間(クリア)
    XYWAIT                      'XY動作待ち時間
    YAXIS                       'Y軸表示範囲

    LIMITTRQ    = 20            '測定終了条件[%]
    LIMITSTRESS                 '測定終了応力[%]
    LIMITTIME                   '測定終了時間[秒]
    RANGE                       'HP温度変動範囲
    RANGE2                      'PB温度変動範囲
End Enum
#end if
'▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼
'   構造体
'▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲


'▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼
'   API宣言
'▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲

'▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼
'   パブリック変数
'▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲

'▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼
'   プライベート変数
'▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲

Module modInpCheck
#If Not NEW_SPEC

'■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
'   パブリック関数
'■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
'''<summary>
'''引数が測定規格として正常な範囲にあるかを確認する。例:CheckSPECValid(strTEMP, MDB.TEMPERATURE)
'''</summary>
'''<param name="cval">確認する変数。</param>
'''<param name="iDBCol">確認対象の列。MDB列挙体で指定</param>
'''<param name="iSmplTime">サンプリング時間。LimitTimeの判定で使用。0:0.1秒,1:1秒,2:10秒</param>
'''<returns>値が正常な範囲にある場合0、範囲外にある場合-1、小数桁数が範囲外の場合-2、その他のエラーの場合-99。</returns>
'''    Public Function CheckSPECValid(ByVal cval As Object, ByVal iDBCol As Integer, ByVal iSmplTime As Integer, Optional ByVal dTrqFact As Double = 1) As Integer
Public Function CheckSPECValid(ByRef cval As String, ByVal iDBCol As Integer, ByVal iSmplTime As Integer) As Integer
    Dim iVal As Integer
    Dim strVal As String
    Dim iDigit As Integer
    Dim dMinMax() As Double = Nothing
    Dim iMax As Integer
    Dim iMin As Integer

    If IsNothing(cval) Then
        Return -99
    End If
    strVal = cval.ToString()
    strVal = Replace(strVal, " ", "")
    If IsNumeric(strVal) = False Then
        Return -99
    End If

    '入力範囲文字列を取得
    '指定された列の最大・最小値、小数桁数を取得
    iDigit = GetParamMinMaxDigit(iDBCol, iSmplTime, dMinMax)
    If iDigit < 0 Then
        '取得できなかった場合
        Return -99
    End If

    '最小、最大値を整数に変換
    iMin = CInt(dMinMax(0) * 10 ^ iDigit)
    iMax = CInt(dMinMax(1) * 10 ^ iDigit)

    '確認変数を整数値に変換
    iVal = CInt(CDbl(strVal) * 10 ^ iDigit)

    '最小、最大範囲内チェック
    If iMin > iVal OrElse iVal > iMax Then
        Return -1
    End If

    '小数点フォーマットに整形
    Dim strFormVal	As String
	Dim dDt			As Double
	dDt = iVal / (10 ^ iDigit)
    strFormVal = dDt.ToString( FormatToDecPoint(dDt, iDigit) )
    cval = strFormVal

    Return 0
End Function
''' <summary>
''' 小数点書式フォーマット文字列を返す
''' </summary>
''' <param name="d">数値</param>
''' <param name="iDigit">小数点以下桁数</param>
''' <returns></returns>
Public Function FormatToDecPoint(ByVal d As Double, ByVal iDigit As Integer ) As String
Dim s As String = "0"
Dim i As Integer
Try
	If iDigit > 0
		s = s & "."
		For i = 0 To iDigit - 1
			s = s & "0"
		Next i
	End If
Catch ex As Exception
End Try
	Return s
End Function
'''<summary>
'''引数が測定規格として正常な範囲にあるかを確認する
'''</summary>
'''<param name="iDBCol">確認対象の列。MDB列挙体で指定</param>
'''<param name="iSmplTime">サンプリング時間。LimitTimeの判定で使用。0:0.1秒,1:1秒,2:10秒</param>
'''<returns>値が正常な範囲にある場合True、範囲外にある場合False。</returns>
Public Function DispSPECValidRange(ByVal iDBCol As Integer, ByVal iSmplTime As Integer) As String
    Dim dMin As Double
    Dim dMax As Double

    Select Case iDBCol
    '熱板温度[℃]
    Case DefSPEC.TEMPERATURE
        Return "0.0 - 300.0"
    Case DefSPEC.TEMPERATURE2
        Return "0.0 - 300.0"
    'ゲル時間判定値(％)
    Case DefSPEC.MEASTHR                            '<----------------------------------------
	    Return "0.1 - 100.0"
    '投入GAP[mm]
    Case DefSPEC.INPGAP
        Return "0.000 - 100.000"
    'GAP[mm]
    Case DefSPEC.GAP
        Return "0.000 - 30.000"
    '測定待ち時間[S]
    Case DefSPEC.WAIT
        Return "1.0 - 100.0"
    'X,Y振幅[mm]
    Case DefSPEC.XAMP,DefSPEC.YAMP
        Return "0.000 - 10.000"
    'X,Y周期[S]
    Case DefSPEC.XPERIOD,DefSPEC.YPERIOD
        Return "0.0 - 50.0"
    'Y軸表示範囲[%]
    Case DefSPEC.YAXIS                              '<----------------------------------------
        Return "0.1 - 1000.0"
    '投入待ち時間(クリア時間)[S]
    Case DefSPEC.CLEAR
        Return "0.0 - 1000.0"
    'XY動作待機時間[S]
    Case DefSPEC.XYWAIT
        Return "0.0 - 300.0"
    '測定終了条件[%]
    Case DefSPEC.LIMITTRQ                           '<----------------------------------------
        dMin = 0.0
        dMax = 99.999
        Return dMin.ToString("F3") & " - " & dMax.ToString("F3")
    '測定終了応力[%]
    Case DefSPEC.LIMITSTRESS
        Return "0.0 - 100.0"
    '測定終了時間[S]
    Case DefSPEC.LIMITTIME
        Return "0.1 - 999.9"
    '熱板温度許容範囲[℃]
    Case DefSPEC.RANGE
        Return "0.1 - 9.9"
    'プローブ温度許容範囲[℃]
    Case DefSPEC.RANGE2
        Return "0.1 - 9.9"
    Case Else
        Return ""
    End Select
    Return ""
End Function


'''<summary>
'''スペックを入力したデータグリッドビューの値が有効であるかを調べる。
'''</summary>
'''<param name="dgv">調べるDataGridView。</param>
'''<param name="i">調べる行。</param>
'''<param name="iSmplTime">サンプリング時間。LimitTimeの判定で使用。0:0.1秒,1:1秒,2:10秒</param>
'''<returns>'値が正常な範囲にある場合0、範囲外にある場合-1、小数桁数が範囲外の場合-2、その他のエラーの場合-99</returns>
Public Function CheckDGVSpec(ByRef dgv As DataGridView, ByVal i As Integer, ByVal iSmplTime As Integer) As Integer
    Dim iValid As Integer = 0
    Select Case i
    Case eSPEC_ROW.iTEMPR
        iValid = CheckSPECValid(dgv.Item(1, i).Value, DefSPEC.TEMPERATURE, iSmplTime)

    Case eSPEC_ROW.iTEMPR2
        iValid = CheckSPECValid(dgv.Item(1, i).Value, DefSPEC.TEMPERATURE2, iSmplTime)

    Case eSPEC_ROW.iTRQ
        iValid = CheckSPECValid(dgv.Item(1, i).Value, DefSPEC.MEASTHR, iSmplTime)

    Case eSPEC_ROW.iINP_GAP
        iValid = CheckSPECValid(dgv.Item(1, i).Value, DefSPEC.INPGAP, iSmplTime)

    Case eSPEC_ROW.iGAP
        iValid = CheckSPECValid(dgv.Item(1, i).Value, DefSPEC.GAP, iSmplTime)

    Case eSPEC_ROW.iWAIT
        iValid = CheckSPECValid(dgv.Item(1, i).Value, DefSPEC.WAIT, iSmplTime)

    Case eSPEC_ROW.iXAmp
        iValid = CheckSPECValid(dgv.Item(1, i).Value, DefSPEC.XAMP, iSmplTime)

    Case eSPEC_ROW.iYAmp
        iValid = CheckSPECValid(dgv.Item(1, i).Value, DefSPEC.YAMP, iSmplTime)

    Case eSPEC_ROW.iXPeriod
        iValid = CheckSPECValid(dgv.Item(1, i).Value, DefSPEC.XPERIOD, iSmplTime)

    Case eSPEC_ROW.iYPeriod
        iValid = CheckSPECValid(dgv.Item(1, i).Value, DefSPEC.YPERIOD, iSmplTime)

    Case eSPEC_ROW.iY_MAX
        iValid = CheckSPECValid(dgv.Item(1, i).Value, DefSPEC.YAXIS, iSmplTime)

    Case eSPEC_ROW.iCLEAR
        iValid = CheckSPECValid(dgv.Item(1, i).Value, DefSPEC.CLEAR, iSmplTime)

    Case eSPEC_ROW.iXYWAIT
        iValid = CheckSPECValid(dgv.Item(1, i).Value, DefSPEC.XYWAIT, iSmplTime)

    Case eSPEC_ROW.iEND_COND
        iValid = CheckSPECValid(dgv.Item(1, i).Value, DefSPEC.LIMITTRQ, iSmplTime)

    Case eSPEC_ROW.iEND_STRS
        iValid = CheckSPECValid(dgv.Item(1, i).Value, DefSPEC.LIMITSTRESS, iSmplTime)

    Case eSPEC_ROW.iEND_TIME
        'Select Case iSmplTime
        '    Case Is < eSAMPLING.iSPLIT
        '        iValid = CheckSPECValid(dgv.Item(1, i).Value, DefSPEC.LIMITTIME, iSmplTime)
        'End Select

    Case eSPEC_ROW.iTEMPR_TOLER
        iValid = CheckSPECValid(dgv.Item(1, i).Value, DefSPEC.RANGE, iSmplTime)

    Case eSPEC_ROW.iTEMPR2_TOLER
        iValid = CheckSPECValid(dgv.Item(1, i).Value, DefSPEC.RANGE2, iSmplTime)

    End Select
    Return iValid
End Function

'''<summary>
'''データグリッドビューの入力値全てについて有効範囲であるかを調べる。
'''</summary>
'''<param name="dgv">調べるDataGridView。</param>
'''<param name="iSmplTime">サンプリング時間。LimitTimeの判定で使用。0:0.1秒,1:1秒,2:10秒</param>
'''<returns>全ての範囲が有効である場合True。一箇所でも異なる場合はFalseを返す。</returns>
Public Function ValidateAllDGVSpec(ByRef dgv As DataGridView, ByVal iSmplTime As Integer) As Integer
    'If dgv.RowCount - 1 < eSPEC_ROW.iMAX - 1 Then
    '    Return -1
    'End If
    Dim iValid As Integer = 0
    For i As Integer = 0 To dgv.Rows.Count - 1
        Select Case CheckDGVSpec(dgv, i, iSmplTime)
        Case 0
            dgv.Item(1, i).Style.BackColor = Color.White
            dgv.Item(1, i).Style.ForeColor = Color.Black
        Case -2
            dgv.Item(1, i).Style.BackColor = Color.White
            dgv.Item(1, i).Style.ForeColor = Color.Red
            iValid = -2
        Case Else
            dgv.Item(1, i).Style.BackColor = Color.Yellow
            dgv.Item(1, i).Style.ForeColor = Color.Black
            iValid = -1
        End Select
    Next i

    Return iValid
End Function

'''<summary>
'''データグリッドビューの入力値全てについて有効範囲であるかを調べる。
'''</summary>
'''<param name="dgv">調べるDataGridView。</param>
'''<returns>全ての範囲が有効である場合True。一箇所でも異なる場合はFalseを返す。</returns>
Public Function ValidateAllDGVCalc(ByRef dgv As DataGridView) As Integer
    Dim iValid As Integer = 0

    If dgv.RowCount < eCALC_ROW.iMAX - 1 Then
        Return -1
    End If

    For i As Integer = 0 To eCALC_ROW.iMAX - 1
        Select Case i
        Case eCALC_ROW.iMASK_RANGE, eCALC_ROW.iGEL_RANGE
        'Case eCALC_ROW.iSLOP_THR, _
        '     eCALC_ROW.iMASK_RANGE, _
        '     eCALC_ROW.iGEL_RANGE
        '     eCALC_ROW.iGEL_CONV, _
        '     eCALC_ROW.iPEAK_DROP, _
        '     eCALC_ROW.iPEAK_TIME, _
        '     eCALC_ROW.iPEAK_DIFF

            If dgv.Item(1, i).Value Is Nothing Then
                dgv.Item(1, i).Style.BackColor = Color.Yellow
                dgv.Item(1, i).Style.ForeColor = Color.Black
                iValid = -1
            Else
                If dgv.Item(1, i).Value.ToString() = "" Then
                    dgv.Item(1, i).Style.BackColor = Color.Yellow
                    dgv.Item(1, i).Style.ForeColor = Color.Black
                    iValid = -1
                Else
                    dgv.Item(1, i).Style.BackColor = Color.White
                    dgv.Item(1, i).Style.ForeColor = Color.Black
                End If
            End If
        End Select
    Next i

    Return iValid
End Function

'''<summary>
'''測定条件の各要素に合う見出しを引数のデータグリッドビューに表示する。
'''</summary>
Public Sub AddSpecRow(ByRef dgvSpec As DataGridView, ByRef dgvCalcOpt As DataGridView)
    Dim strDesc As String() = frmmain.p_sMes(efrmMainSTR.iDESCRIPTION_SPEC).Split(",")
    Dim name() As String = strDesc
    With dgvSpec
        .Rows.Add(name(eSPEC_ROW.iTEMPR), "")
        .Rows.Add(name(eSPEC_ROW.iTEMPR2), "")
        .Rows.Add(name(eSPEC_ROW.iTRQ), "")
        .Rows.Add(name(eSPEC_ROW.iINP_GAP), "")
        .Rows.Add(name(eSPEC_ROW.iGAP), "")
        .Rows.Add(name(eSPEC_ROW.iXAmp), "")
        .Rows.Add(name(eSPEC_ROW.iYAmp), "")
        .Rows.Add(name(eSPEC_ROW.iXPeriod), "")
        .Rows.Add(name(eSPEC_ROW.iYPeriod), "")
        .Rows.Add(name(eSPEC_ROW.iWAIT), "")
        .Rows.Add(name(eSPEC_ROW.iCLEAR), "")
        .Rows.Add(name(eSPEC_ROW.iXYWAIT), "")
        .Rows.Add(name(eSPEC_ROW.iY_MAX), "")
        .Rows.Add(name(eSPEC_ROW.iEND_COND), "")
        .Rows.Add(name(eSPEC_ROW.iEND_STRS), "")
        .Rows.Add(name(eSPEC_ROW.iEND_TIME), "")
        .Rows.Add(name(eSPEC_ROW.iTEMPR_TOLER), "")
        .Rows.Add(name(eSPEC_ROW.iTEMPR2_TOLER), "")
        .Rows.Add(name(eSPEC_ROW.iPROBE), "")
        .Rows.Add(name(eSPEC_ROW.iAPPENDIX), "")
    End With
    With dgvCalcOpt
        For iCnt As Integer = 0 To eCALC_ROW.iMAX - 1
            Select Case iCnt
            Case eCALC_ROW.iMASK_RANGE      '有効範囲[秒]a~b
                .Rows.Add(frmMain.p_sMes(efrmMainSTR.ROW_APRX_RANGE), "0.0 - 0.0")
            Case eCALC_ROW.iGEL_RANGE       '判定範囲[秒]a~b
                .Rows.Add(frmMain.p_sMes(efrmMainSTR.ROW_VALID_RANGE), "0.0 - 0.0")
            Case Else
                Continue For
            End Select
            .Item(1, iCnt).ReadOnly = True      '有効範囲、判定範囲を編集不可->ダイアログで変更
        Next
    End With
End Sub
'''<summary>
'''引数の文字列から半角英数字と一部の記号以外を取り除く
'''</summary>
Public Sub LimitNameStr(ByRef str As String)
    If IsNothing(str) = True Then
        str = ""
    End If
    Dim strb As System.Text.StringBuilder = New System.Text.StringBuilder()
    For Each ch As Char In str
        If "1234567890abcdefghijklmnopqrstuvwxyzABCDEFGHJIJKLMNOPQRSTUVWXYZ%-_".IndexOf(ch) >= 0 Then
            strb.Append(ch)
        End If
    Next
    str = strb.ToString()
End Sub

'■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
'   プライベート関数
'■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
#end if
End Module
