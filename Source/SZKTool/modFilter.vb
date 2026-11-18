Imports System
Imports System.Threading
Imports System.ComponentModel
Imports System.IO


''' <summary>
''' デジタルフィルタクラス
''' FIR/IIRフィルタを実装
''' 
''' 石川高専 DF_DESIGN出力係数使用版と自作版
''' IIR自作(https://qiita.com/Micochan/items/6be0034a9f75bb0706cd)はバタワース固定、1/2/3次、カットオフ周波数、LPF/HPF選択
''' </summary>
Public Class Filter
'ディジタルフィルタ情報
Public Structure stFilterInfo
	Dim iEnable			As Integer	'フィルタ有効無効
	Dim sKind			As String	'フィルタ種類
	Dim iFIRFilLen		As Int32	'FIRフィルタ パラメータ数
	Dim dFIRCutOffFreq	As Double	'FIRフィルタ カットオフ周波数
	Dim sFIRWinKind		As String	'FIRフィルタ 窓関数種類
	Dim dFIRSmplFreq	As Double	'FIRフィルタ サンプル周波数

	Dim dIIRCutOffFreq	As Double	'IIRフィルタ カットオフ周波数
	Dim iIIRDeg			As Int16	'IIRフィルタ 次数
	Dim sFileName		As String	'フィルタ係数ファイル名
End Structure
'IIR用フィルタ係数構造体 ----------------------------------------------
'バタワース3次IIR係数構造体
Public structure Coef3OrderType
    Dim a0 As Double
    Dim a1 As Double
    Dim a2 As Double
    Dim a3 As Double
    Dim b0 As Double
    Dim b1 As Double
    Dim b2 As Double
    Dim b3 As Double
    Dim z() As Double       'サイズ6
End Structure
'バタワース2次IIR係数構造体
Public structure Coef2OrderType
    Dim a0 As Double
    Dim a1 As Double
    Dim a2 As Double
    Dim b0 As Double
    Dim b1 As Double
    Dim b2 As Double
    Dim z() As Double       'サイズ4
End Structure
'バタワース1次IIR係数構造体
Public structure Coef1OrderType
    Dim a0 As Double
    Dim a1 As Double
    Dim b0 As Double
    Dim b1 As Double
    Dim z() As Double       'サイズ2
End Structure
'----------------------------------------------------------------------
'IIRフィルタパラメータ構造体(石川高専 DF_DESIGN使用の場合)
Public Structure CoefIIR
	Dim dk		 As Double
	Dim da1		 As Double
	Dim da2		 As Double
	Dim db0		 As Double
	Dim db1		 As Double
	Dim db2		 As Double
End Structure

''' <summary>
''' IIRフィルタ処理メイン(バタワース)Integer
''' </summary>
''' <param name="InpD">フィルタ前配列</param>
''' <param name="flt">フィルタ条件</param>
''' <param name="OutD">フィルタ後配列</param>
''' <returns></returns>
Public Function IirFilterButterworth(ByVal InpD() As Int64, ByVal flt As stFilterInfo, ByRef OutD() As Int64) As Int32
Dim FilIir As New Filter
Dim TempIn() As double = Nothing
Dim TempOut() As double = Nothing
Dim deg As Int16			'段数
Dim fc As Double			'カットオフ周波数
Dim Type As Double			'0=LPF,1=HPF
Try
	'Workバッファ準備
	Array.Resize(Tempin, inpD.Length)
	Array.Resize(Tempout, inpD.Length)
	'Array.Copy(inpD, TempIn, InpD.Length)
    InpD.CopyTo(TempIn,0)
	'For i = 0 To inpD.GetUpperBound(0)
	'	TempIn(i) = CType(inpD(i), Double)
	'Next
	fc = flt.dIIRCutOffFreq
	deg = flt.iIIRDeg
	Type = 0			    'LPF
	FilIir.ArrayFilter(MEAS_SAMPLE_FREQ, fc, Type, deg, TempIn, TempOut)
    'Array.Copy(TempOut, OutD, TempOut.Length)
    For i = 0 To TempIn.GetUpperBound(0)
        outD(i) = CType(Tempout(i), Int64)
    Next

Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Return -1
End Try
	Return 0
End Function
''' <summary>
''' IIRフィルタ処理メイン(バタワース)Double
''' </summary>
''' <param name="InpD">フィルタ前配列</param>
''' <param name="flt">フィルタ条件</param>
''' <param name="OutD">フィルタ後配列</param>
''' <returns></returns>
Public Function IirFilterButterworthD(ByVal InpD() As Double, ByVal flt As stFilterInfo, ByRef OutD() As Double) As Int32
Dim FilIir As New Filter
Dim TempIn()	As double = Nothing
Dim TempOut()	As double = Nothing
Dim deg			As Int16			'段数
Dim fc			As Double			'カットオフ周波数
Dim Type		As Double			'0=LPF,1=HPF
Try
	'Workバッファ準備
	Array.Resize(Tempin, inpD.Length)
	Array.Resize(Tempout, inpD.Length)
	'Array.Copy(inpD, TempIn, InpD.Length)
    InpD.CopyTo(TempIn,0)
	'For i = 0 To inpD.GetUpperBound(0)
	'	TempIn(i) = CType(inpD(i), Double)
	'Next
	fc = flt.dIIRCutOffFreq
	deg = flt.iIIRDeg
	Type = 0			    'LPF
	FilIir.ArrayFilter(MEAS_SAMPLE_FREQ, fc, Type, deg, TempIn, TempOut)
    'Array.Copy(TempOut, OutD, TempOut.Length)
    For i = 0 To TempIn.GetUpperBound(0)
        Tempout(i) = If(Tempout(i)=0,Double.NaN,Tempout(i))
        outD(i) = CType(Tempout(i), Double)
    Next

Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Return -1
End Try
	Return 0
End Function

''' <summary>
''' バタワース１次IIRパラメータ算出
''' </summary>
''' <param name="coef">係数構造体</param>
''' <param name="fs">サンプリング周波数</param>
''' <param name="fc">カットオフ周波数</param>
''' <param name="type">0=LPF,1=HPF</param>
Private Sub CreateCoefOrder_1(ByRef coef As Coef1OrderType, ByVal fs As Double, Byval fc As Double, ByVal type As int16)
    Dim fa      As Double
    Dim pfc     As Double
Try 
    fa = 1.0 / (2.0 * Math.PI) * Math.tan(Math.PI * fc / fs)
    pfc = 2.0 * Math.PI * fa
    
    '構造体0クリア
    coef.a0 = 0
    coef.a1 = 0
    coef.b0 = 0
    coef.b1 = 0
    Array.Clear(coef.z, 0, coef.z.Length)
    Select Case type
    Case 0
    'LPF
        coef.b0 = 1.0
        coef.b1 = (pfc - 1.0) / (pfc + 1.0)
        coef.a0 = pfc / (pfc + 1.0)
        coef.a1 = pfc / (pfc + 1.0)
    Case 1
    'HPF
        coef.b0 = 1.0
        coef.b1 = (pfc - 1.0) / (pfc + 1.0)
        coef.a0 = 1.0 / (pfc + 1.0)
        coef.a1 = -1.0 / (pfc + 1.0)
    End Select

Catch ex As Exception
	Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub
''' <summary>
''' バタワース２次IIRパラメータ算出
''' </summary>
''' <param name="coef">係数構造体</param>
''' <param name="fs">サンプリング周波数</param>
''' <param name="fc">カットオフ周波数</param>
''' <param name="type">0=LPF,1=HPF,2=APF</param>
Private Sub CreateCoefOrder_2(ByRef coef As Coef2OrderType, ByVal fs As Double, Byval fc As Double, ByVal type As int16)
    Dim fa      As Double
    Dim pfc     As Double
    Dim RT2     As Double

Try 
    fa = 1.0 / (2.0 * Math.PI) * Math.tan(Math.PI * fc / fs)
    pfc = 2.0 * Math.PI * fa
    RT2 = Math.Sqrt(2.0)
    
    '構造体0クリア
    coef.a0 = 0
    coef.a1 = 0
    coef.a2 = 0
    coef.b0 = 0
    coef.b1 = 0
    coef.b2 = 0
    Array.Clear(coef.z, 0, coef.z.Length)
    Select Case type
    Case 0
    'LPF
        coef.b0 = 1.0
        coef.b1 = (-2.0 + 2.0*pfc*pfc) / (1 + RT2 * pfc + pfc*pfc)
        coef.b2 = (1.0 - RT2*pfc + pfc*pfc) / (1 + RT2*pfc + pfc*pfc)

        coef.a0 = pfc*pfc / (1 + RT2 * pfc + pfc*pfc)
        coef.a1 = 2.0 * pfc*pfc / (1 + RT2*pfc + pfc*pfc)
        coef.a2 = pfc*pfc / (1 + RT2*pfc + pfc*pfc)
    Case 1
    'HPF
        coef.b0 = 1.0
        coef.b1 = (2.0*pfc*pfc - 2.0) / (pfc*pfc + RT2*pfc + 1.0)
        coef.b2 = (pfc*pfc - RT2*pfc + 1.0) / (pfc*pfc + RT2*pfc + 1.0)

        coef.a0 = 1.0 / (pfc*pfc + RT2*pfc + 1.0)
        coef.a1 = -2.0 / (pfc*pfc + RT2*pfc + 1.0)
        coef.a2 = 1.0 / (pfc*pfc + RT2*pfc + 1.0)
    Case 2
    'APF
        Dim w0 As Double = 2.0 * Math.PI*fc / fs
        Dim alpha As Double = Math.sin(w0) / 2.0

        coef.b0 = 1.0
        coef.b1 = -2 * Math.cos(w0) / (1 + alpha)
        coef.b2 = (1 - alpha) / (1 + alpha)

        coef.a0 = (1 - alpha) / (1 + alpha)
        coef.a1 = -2 * Math.cos(w0) / (1 + alpha)
        coef.a2 = (1 + alpha) / (1 + alpha)
    End Select
Catch ex As Exception
	Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub
''' <summary>
''' バタワース３次IIRパラメータ算出
''' </summary>
''' <param name="coef">係数構造体</param>
''' <param name="fs">サンプリング周波数</param>
''' <param name="fc">カットオフ周波数</param>
''' <param name="type">0=LPF,1=HPF</param>
Private Sub CreateCoefOrder_3(ByRef coef As Coef3OrderType, ByVal fs As Double, Byval fc As Double, ByVal type As int16)
    Dim fa      As Double
    Dim pfc     As Double
Try 
    fa = 1.0 / (2.0 * Math.PI) * Math.tan(Math.PI * fc / fs)
    pfc = 2.0 * Math.PI * fa
    
    '構造体0クリア
    coef.a0 = 0
    coef.a1 = 0
    coef.a2 = 0
    coef.a3 = 0
    coef.b0 = 0
    coef.b1 = 0
    coef.b2 = 0
    coef.b3 = 0
    Array.Clear(coef.z, 0, coef.z.Length)
    Select Case type
    Case 0
    'LPF
        Dim tmp As Double = (1.0 + 2.0*pfc + 2.0*pfc*pfc + pfc*pfc*pfc)

        coef.b0 = 1.0
        coef.b1 = (-3.0 - 2.0*pfc + 2.0*pfc*pfc + 3.0*pfc*pfc*pfc) / tmp
        coef.b2 = (3.0 - 2.0*pfc - 2.0*pfc*pfc + 3.0*pfc*pfc*pfc) / tmp
        coef.b3 = (-1.0 + 2.0*pfc - 2.0*pfc*pfc + pfc*pfc*pfc) / tmp

        coef.a0 = (pfc*pfc*pfc) / tmp
        coef.a1 = 3.0*(pfc*pfc*pfc) / tmp
        coef.a2 = 3.0*(pfc*pfc*pfc) / tmp
        coef.a3 = (pfc*pfc*pfc) / tmp
    Case 1
    'HPF
        Dim tmp As Double = (1.0 + 2.0*pfc + 2.0*pfc*pfc + pfc*pfc*pfc)

        coef.b0 = 1.0
        coef.b1 =  (-3.0 - 2.0*pfc + 2.0*pfc*pfc + 3.0*pfc*pfc*pfc) / tmp
        coef.b2 =  (3.0 - 2.0*pfc - 2.0*pfc*pfc + 3.0*pfc*pfc*pfc) / tmp
        coef.b3 = (-1.0 + 2.0*pfc - 2.0*pfc*pfc + pfc*pfc*pfc) / tmp

        coef.a0 = 1.0 / tmp
        coef.a1 = -3.0 / tmp
        coef.a2 =  3.0 / tmp
        coef.a3 = -1.0 / tmp
    End Select
Catch ex As Exception
	Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub
''' <summary>
''' バタワース１次実行
''' </summary>
''' <param name="cf">係数構造体</param>
''' <param name="inp">入力データ</param>
''' <returns>出力データ</returns>
Private Function DoFilter1(ByRef cf As Coef1OrderType,ByVal inp As double ) As Double
    Dim outp As Double
    'calc
    outp = inp*cf.a0 + cf.z(0)*cf.a1  - cf.z(1)*cf.b1
    'unit delay
    cf.z(0) = inp
    cf.z(1) = outp
    return outp
End Function
''' <summary>
''' バタワース２次実行
''' </summary>
''' <param name="cf">係数構造体</param>
''' <param name="inp">入力データ</param>
''' <returns>出力データ</returns>
Private Function DoFilter2(ByRef cf As Coef2OrderType,ByVal inp As double ) As Double
    Dim outp As Double
    'calc
    outp = inp*cf.a0 + cf.z(0)*cf.a1 + cf.z(1)*cf.a2 - cf.z(2)*cf.b1 - cf.z(3)*cf.b2
    'unit delay
    cf.z(1) = cf.z(0)
    cf.z(0) = inp
    cf.z(3) = cf.z(2)
    cf.z(2) = outp

    return outp
End Function
''' <summary>
''' バタワース３次実行
''' </summary>
''' <param name="cf">係数構造体</param>
''' <param name="inp">入力データ</param>
''' <returns>出力データ</returns>
Private Function DoFilter3(ByRef cf As Coef3OrderType,ByVal inp As double ) As Double
    Dim outp As Double
    'calc
    outp = inp*cf.a0 + cf.z(0)*cf.a1 + cf.z(1)*cf.a2 + cf.z(2)*cf.a3 _
            - cf.z(3)*cf.b1 - cf.z(4)*cf.b2 - cf.z(5)*cf.b3
    'unit delay
    cf.z(2) = cf.z(1)
    cf.z(1) = cf.z(0)
    cf.z(0) = inp
    cf.z(5) = cf.z(4)
    cf.z(4) = cf.z(3)
    cf.z(3) = outp
    return outp
End Function

''' <summary>
''' 配列データをIIRフィルタ処理(バタワース)
''' </summary>
''' <param name="fs">サンプル周波数</param>
''' <param name="fc">カットオフ周波数</param>
''' <param name="type">0=LPF,1=HPF</param>
''' <param name="deg">段数(IIR)</param>
''' <param name="InpD">入力データ</param>
''' <param name="OutD">出力データ</param>
Private sub ArrayFilter(ByVal fs As Double, Byval fc As Double, ByVal type As int16, byVal deg As Int16, Byref InpD() As Double, Byref OutD() As Double)
Dim i As int32
Dim coef1 As Coef1OrderType = Nothing
Dim coef2 As Coef2OrderType = Nothing
Dim coef3 As Coef3OrderType = Nothing
Try
    Select Case deg
    Case 1    '1次
        Array.Resize(coef1.z, 2)
        CreateCoefOrder_1(coef1, fs, fc, type)
        For i=0 To InpD.GetUpperBound(0)
			InpD(i) = If(Double.IsNaN(InpD(i)), 0, InpD(i))
            outD(i) = DoFilter1(coef1, InpD(i))
        Next i
    Case 2    '2次
        Array.Resize(coef2.z, 4)
        CreateCoefOrder_2(coef2, fs, fc, type)
        For i=0 To InpD.GetUpperBound(0)
			InpD(i) = If(Double.IsNaN(InpD(i)), 0, InpD(i))
            outD(i) = DoFilter2(coef2, InpD(i))
        Next i
    Case 3    '3次
        Array.Resize(coef3.z, 6)
        CreateCoefOrder_3(coef3, fs, fc, type)
        For i=0 To InpD.GetUpperBound(0)
			InpD(i) = If(Double.IsNaN(InpD(i)), 0, InpD(i))
            outD(i) = DoFilter3(coef3, InpD(i))
        Next i
    End Select

Catch ex As Exception
	Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End sub
'FIRフィルタ
''' <summary>
''' FIRフィルタ処理(Integer)
''' </summary>
''' <param name="data4">サンプル入力</param>
''' <param name="fir_coef">フィルタ係数</param>
''' <param name="data3">出力データ</param>
''' <returns></returns>
Public Function FirFilter(byVal data4() As int64, ByVal fir_coef() As Double, Byref data3() As int64) As Int32
Dim write_idx	As Integer
Dim read_idx	As Integer
Dim tap			As Integer
Dim fir_z()		As Double = Nothing
Dim tap_idx		As Integer
Dim ly			As Double
Dim k			As int64

Try
	if IsNothing(fir_coef)	
		Return -2				'フィルタパラメータの準備が出来ていない
	Else
		tap = fir_coef.Length
	End If
	Array.Resize(fir_z , tap)
    For k = 0 To data3.GetUpperBound(0)
        If write_idx = tap - 1 Then
            write_idx = 0
        Else
            write_idx = (write_idx + 1)
        End If

        read_idx = write_idx

        fir_z(write_idx) = Double.Parse(data4(k))
        ly = 0
        For tap_idx = 0 To tap - 1
            ly = ly + fir_z(read_idx) * fir_coef(tap_idx)
            If read_idx = 0 Then
                read_idx = tap - 1
            Else
                read_idx = read_idx - 1
            End If
        Next tap_idx
        data3(k) = Cint(ly)
    Next k
Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Return -1
End Try
	Return 0
End Function
''' <summary>
''' FIRフィルタ処理(Double)
''' </summary>
''' <param name="data4">サンプル入力</param>
''' <param name="fir_coef">フィルタ係数</param>
''' <param name="data3">出力データ</param>
''' <returns></returns>
Public Function FirFilterD(byVal data4() As Double, ByVal fir_coef() As Double, Byref data3() As Double) As Int32
Dim write_idx	As Integer
Dim read_idx	As Integer
Dim tap			As Integer
Dim fir_z()		As Double = Nothing
Dim tap_idx		As Integer
Dim ly			As Double
Dim k			As int64

Try
	if IsNothing(fir_coef)	
		Return -2				'フィルタパラメータの準備が出来ていない
	Else
		tap = fir_coef.Length
	End If
	Array.Resize(fir_z , tap)
    For k = 0 To data3.GetUpperBound(0)
        If write_idx = tap - 1 Then
            write_idx = 0
        Else
            write_idx = (write_idx + 1)
        End If

        read_idx = write_idx

        fir_z(write_idx) = Double.Parse(data4(k))
        ly = 0
        For tap_idx = 0 To tap - 1
            ly = ly + fir_z(read_idx) * fir_coef(tap_idx)
            If read_idx = 0 Then
                read_idx = tap - 1
            Else
                read_idx = read_idx - 1
            End If
        Next tap_idx
        data3(k) = ly
    Next k
Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Return -1
End Try
	Return 0
End Function
''' <summary>
''' IIRフィルタ処理 (フィルタ係数ファイルから 石川高専 DF_DESIGN使用の場合)
''' </summary>
''' <param name="data4">入力サンプル</param>
''' <param name="coef">フィルタ係数</param>
''' <param name="data3">出力結果</param>
''' <returns></returns>
Public function IirFilter(byVal data4() As int64, ByVal coef() As CoefIIR, Byref data3() As int64) As Int32
	Dim iir_z0(2)	As Double
	Dim iir_z1(2)	As Double
	Dim l_x			As Double
	Dim l_w			As Double
	Dim l_w0		As Double
	Dim l_w1		As Double
	Dim l_w2		As Double
	Dim l_w_a		As Double
	Dim l_w_a0		As Double
	Dim l_w_a1		As Double
	Dim l_w_a2		As Double
	Dim j As int64
	Dim k As int64

Try

    For j = 0 To coef.Length - 1
        For k = 0 To data4.GetUpperBound(0)
            If j = 0 Then
                l_x = data4(k)
            Else
                l_x = data3(k)
            End If
            
            l_w0 = l_x
            l_w1 = iir_z0(0) * (-1 * coef(j).da1)
            l_w2 = iir_z0(1) * (-1 * coef(j).da2)
            l_w = l_w0 + l_w1 + l_w2
            
            l_w_a0 = l_w * (coef(j).dk * coef(j).db0)
            l_w_a1 = iir_z0(0) * (coef(j).dk * coef(j).db1)
            l_w_a2 = iir_z0(1) * (coef(j).dk * coef(j).db2)
            l_w_a = l_w_a0 + l_w_a1 + l_w_a2
            
            iir_z0(1) = iir_z0(0)
            iir_z0(0) = l_w
            
            data3(k) = l_w_a
            
        Next
    Next

Catch ex As Exception
	Return -1
End Try
	Return 0
End function
''' <summary>
''' フィルタ係数ファイルリード((石川高専 DF_DESIGN使用の場合)
''' </summary>
''' <param name="sPath">パラメータファイルパス</param>
''' <param name="Coef">FIR用パラメータ</param>
''' <param name="Coef1">IIR用パラメータ</param>
''' <returns>0:正常 -1:例外発生</returns>
Public function FilterCoefRead(ByVal sType As String, ByVal sPath As String, ByRef Coef() As Double, ByRef Coef1() As CoefIIR) As Int32
	Dim sLineBuf		As	string
	Dim sBuf()			As	string = Nothing
	Dim i				As int64
	Dim j				As int64
	Dim iSec			As int64
Try
	Dim fs As New FileStream(sPath, FileMode.Open, FileAccess.Read,FileShare.ReadWrite)
	Dim sr As TextReader = New StreamReader(fs, Text.Encoding.UTF8)
	Select Case sType
	Case "FIR"  'FIR
		i = 0
		Do 
			sLineBuf = sr.ReadLine()
			If IsNothing(sLineBuf)
				Exit Do
			ElseIf sLineBuf.Length = 0
				Continue do
			End If
			If sLineBuf.Substring(0,1) <> "'"  
				Array.Resize(Coef, i+1)
				coef(i) = Double.Parse(sLineBuf)
				i += 1
			End If
		Loop Until sLineBuf Is Nothing
	Case "IIR"	'IIR
		i = 0		'段数カウント
		j = 0		'段数内カウント
		Do 
			sLineBuf = sr.ReadLine()
			If IsNothing(sLineBuf)
				Exit Do 
			ElseIf sLineBuf.Length = 0
				If iSec <> 0
					i += 1
					j = 0
				End If
				Continue do
			End If
			If sLineBuf.Substring(0,1) <> "'" 
				If sLineBuf.Substring(0,3) = "Sec"
					sBuf = sLineBuf.Split("=")
					iSec = Int64.Parse(sBuf(1))
					Array.Resize(Coef1, iSec)
					i = 0
					Continue do
				End If
				Select Case j
				Case 0
					coef1(i).dk = Double.Parse(sLineBuf)
				Case 1
					coef1(i).da1 = Double.Parse(sLineBuf)
				Case 2
					coef1(i).da2 = Double.Parse(sLineBuf)
				Case 3
					coef1(i).db0 = Double.Parse(sLineBuf)
				Case 4
					coef1(i).db1 = Double.Parse(sLineBuf)
				Case 5
					coef1(i).db2 = Double.Parse(sLineBuf)
				End Select
				j += 1
			End If
		Loop Until sLineBuf Is Nothing Or i > iSec 

	End Select
	sr.Close()
	sr.Dispose()
	Return 0
Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Return -1
End Try
End function
''' <summary>
''' FIRフィルタ窓関数
''' </summary>
''' <param name="flt">フィルタ情報参照</param>
''' <param name="listArray">出力パラメータ配列</param>
''' <returns></returns>
Public Function WindowFunction( ByVal flt As stFilterInfo, Byref listArray() As Double) As Int16
Dim	i			As Int32
Dim coef()		As Double = Nothing
Dim wHamming()	As Double = Nothing
Dim coefWin		As Double = Nothing
Dim omega		As Double
Dim omegac		As Double
Dim tapn2		As int32 =((flt.iFIRFilLen - 1) / 2) + 1
Dim tapArray() As Double = Nothing
Try
	Array.Resize(tapArray, tapn2)
	Array.Resize(wHamming, tapn2)
	Array.Resize(coef, tapn2)
	Array.Resize(listArray, flt.iFIRFilLen)
	'omega = Math.PI / (MEAS_SAMPLE_FREQ / 2)					'Sampling Freq 500Hz
	omega = Math.PI / (flt.dFIRSmplFreq / 2)					'Sampling Freq 
	omega = omega * flt.dFIRCutOffFreq
	'omegac = 1 - (flt.dFIRCutOffFreq / (MEAS_SAMPLE_FREQ / 2))
	omegac = 1 - (flt.dFIRCutOffFreq / (flt.dFIRSmplFreq / 2))

	'半周期分を演算
	For i = 1 To tapn2 - 1
		coef(i) = 1 / (Math.PI * i) * Math.Sin(i * omega)					'omega＝正規化カットオフ周波数
		'窓関数
		Select Case flt.sFIRWinKind
		Case "Blackman"
			'ブラックマン窓
			wHamming(i) = 0.42 + 0.5 * Math.Cos((i * Math.PI) / (tapn2 - 1)) + 0.08 * Math.Cos((i * 2 * Math.PI) / (tapn2 - 1))
		Case "Hann"
		'ハン窓
			wHamming(i) = 0.5 + 0.5 * Math.Cos((i * Math.PI) / (tapn2 - 1))
		Case "Hamming"
		'ハミング窓
			wHamming(i) = 0.54 + 0.46 * Math.Cos((i * Math.PI) / (tapn2 - 1))
		End Select
		coefWin = coef(i) * wHamming(i)
		tapArray(i) = coefWin
	Next i
	tapArray(0) = 1 - omegac

	'左半分をセット
	For i = tapArray.length - 1 To 0 Step -1
		listArray((tapArray.length - 1) - i) = tapArray(i)
	Next
	'右半分をセット
	For i = 1 To tapArray.length - 1 Step +1
		listArray(i + tapArray.length - 1) = tapArray(i)
	Next

Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Return -1
End Try
	Return 0
End Function
''' <summary>
''' 生データのフィルタ処理
'''   Xモード 0.2秒周期のみ IIR BPF処理後、中間値補正なし
'''             その他の周期はIIR LPF 5Hz処理後、中間値補正
'''   XYモード IIR LPF 2Hz処理後、中間値補正
''' </summary>
''' <param name="iData1">入力データ</param>
''' <param name="iData2">出力データ</param>
''' <param name="iXYMode">XYモード</param>
''' <param name="dPeriod">X軸周期</param>
''' <param name="dIgnoreTime">無効時間</param>
''' <returns>0：正常終了</returns>

#If 0          '20200525 退避 前処理でBPFしない関数
Public Function FilterPrevProc(ByRef iData1() As Int64, ByRef iData2() As Int64, ByVal iXYMode As Int16, ByVal dPeriod As Double, Byval dIgnoreTime As Double) As Int32
    Dim i                       As Int32
    Dim	Coef()					As Double = Nothing             'FIR用パラメータ
    Dim	Coef1()					As Filter.CoefIIR = Nothing	    'IIR用パラメータ
    Dim dData1()				As Double = Nothing
    Dim dData2()				As Double = Nothing
    Dim PreArray()              As Int64 = Nothing
    Dim PstArray()              As Int64 = Nothing
    
Try
    '無効時間のデータをその平均値で埋める --------------
    Dim iIgnoreCount            As Int32
    iIgnoreCount = dIgnoreTime * MEAS_BLOCK_DATA_NUM            '無効時間のデータ個数
    '無効時間のデータ平均計算
    Dim AryIgnore() As Int64 = Nothing
    Dim iIgnoreAve As int64
    Array.Resize(AryIgnore, iIgnoreCount)
    Array.Copy(iData1, AryIgnore, iIgnoreCount)
    iIgnoreAve = AryIgnore.Average()
    'Array.Fill(AryIgnore, iIgnoreAve, 0, iIgnoreCount - 1) 何故かコンパイルエラー
    For i=0 To AryIgnore.GetUpperBound(0)
        AryIgnore(i) = iIgnoreAve
    Next
    '''''Array.Copy(AryIgnore, 0, iData1, 0, iIgnoreCount)
    '---------------------------------------------------
    '前配列の準備
    'Int64->Double変換
    Array.Resize(dData1, iData1.Length + AryIgnore.Length)
    Array.Resize(dData2, iData1.Length + AryIgnore.Length)
    Array.Copy(iData1, dData1, AryIgnore.Length)
    Array.Copy(iData1, 0, dData1, AryIgnore.Length -1 , iData1.Length)
    If iXYMode <> 2 Then
    'XY動作モードがX軸モードで上記以外の場合->IIR LPF 10Hz
        ArrayFilter(MEAS_SAMPLE_FREQ, 10, 0, 3, dData1, dData2)                 'IIRフィルタ処理 3段
    Else
    'XY動作モードが円の場合-> IIR LPF 2から3Hz
        ArrayFilter(MEAS_SAMPLE_FREQ, 2, 0, 3, dData1, dData2)                  'IIRフィルタ処理 3段
    End If
    For i = 0 To dData1.GetUpperBound(0) - AryIgnore.Length
        iData2(i) = Ctype(dData2(i+(AryIgnore.Length -1)), Int64)
    Next

            'Dim fs As New FileStream("D:\Test\PostFilter.txt", FileMode.Create, FileAccess.Write, FileShare.ReadWrite)
            'Dim sw As Textwriter = New StreamWriter(fs, Text.Encoding.UTF8)
            'Dim sLineBuf As String = nothing
            ''テキストファイル(CSV)書込み
            'For i = 0 To dData2.GetUpperBound(0)
            '    sLineBuf = dData2(i).ToString
            '    sw.WriteLine(sLineBuf)
            'Next i
            'sw.Close()
            'sw = nothing
        Catch ex As Exception
    Return -1
End Try
    Return 0
End Function
#end if
#If 1
Public Function FilterPrevProc(ByVal iMode As Int16, ByVal iData1() As Int64, ByRef iData2() As Int64, ByVal iXYMode As Int16, ByVal dPeriod As Double, Byval dIgnoreTime As Double) As Int32
    Dim i                       As Int32
    Dim	Coef()					As Double = Nothing             'FIR用パラメータ
    Dim	Coef1()					As Filter.CoefIIR = Nothing	    'IIR用パラメータ
    Dim dData1()				As Double = Nothing
    Dim dData2()				As Double = Nothing
    Dim PreArray()              As Int64 = Nothing
    Dim PstArray()              As Int64 = Nothing
    
Try
    Dim iIgnoreCount            As Int32
    Dim dDummyTime              As Double

    'キャリブレーション時はフィルターパスする ----------------
    If iMode = 1
		'フィルタパス
		Array.Copy(iData1, 0, iData2, 0, iData2.Length)
        Return 0
    End If
    '---------------------------------------------------------

    '無効時間のデータをその平均値で埋める --------------
    If iXYMode = 2 Then
        dDummyTime = dPeriod * 1      '1周期分
    Else
        dDummyTime = dPeriod * 100    '100周期分
    End If
    If dDummyTime > dIgnoreTime
        iIgnoreCount = dDummyTime  * MEAS_BLOCK_DATA_NUM            '無効時間のデータ個数
    Else
        iIgnoreCount = dIgnoreTime * MEAS_BLOCK_DATA_NUM            '無効時間のデータ個数
    End If

    '無効時間のデータ平均計算
    Dim AryIgnore() As Int64 = Nothing

    Dim iIgnoreAve As int64
    Array.Resize(AryIgnore, iIgnoreCount)
    Array.Copy(iData1, AryIgnore, iIgnoreCount)
    iIgnoreAve = AryIgnore.Average()
    'Array.Fill(AryIgnore, iIgnoreAve, 0, iIgnoreCount - 1) 何故かコンパイルエラー
    For i=0 To AryIgnore.GetUpperBound(0)
        AryIgnore(i) = iIgnoreAve
    Next
    ''Array.Copy(AryIgnore, 0, iData1, 0, iIgnoreCount)
    '---------------------------------------------------
    If iXYMode <> 2 AndAlso  dPeriod <= 0.5 Then
    'XY動作モードがX軸モードで動作周期が0.5秒の場合-> IIR BPF
#If 1
        Array.Resize(PreArray, iData1.Length + AryIgnore.Length)
        Array.Resize(PstArray, iData1.Length + AryIgnore.Length)
        Array.Copy(iData1, 0, PreArray, 0, AryIgnore.Length)
        Array.Copy(iData1, 0, PreArray, AryIgnore.Length-1, iData1.Length)

        'IIRフィルタファイル読込
        FilterCoefRead( "IIR", path.Combine(p_dtFile.p_FilterPath , "IIR_BPF_C16_Sbe3_Pbs4_Pbe6_Sbs7_0.1db_60db") & ".txt", Coef, Coef1)
        IirFilter(PreArray, Coef1, PstArray)                        'IIRフィルタ処理 BPF

        Array.Copy(PstArray, AryIgnore.Length-1, iData2, 0, iData2.Length)
        'Array.Copy(iData1, 0, iData2, 0, iData2.Length)
#else
				'フィルタパス
				Array.Copy(iData1, 0, iData2, 0, iData2.Length)
#end If
	Else
		'前配列の準備
		'Int64->Double変換
#If 1
		Array.Resize(dData1, iData1.Length + AryIgnore.Length)
        Array.Resize(dData2, iData1.Length + AryIgnore.Length)
        Array.Copy(iData1, dData1, AryIgnore.Length)
        Array.Copy(iData1, 0, dData1, AryIgnore.Length -1 , iData1.Length)
        If iXYMode <> 2 Then
        '動作モードがX軸モードで上記以外の場合->IIR LPF 10Hz
            ArrayFilter(MEAS_SAMPLE_FREQ, 10, 0, 3, dData1, dData2)               'IIRフィルタ処理 3段 10Hz LPF
        Else
        '動作モードが円の場合-> IIR LPF 10Hz
            ArrayFilter(MEAS_SAMPLE_FREQ, 5, 0, 3, dData1, dData2)               'IIRフィルタ処理 3段 5Hz LPF
        End If
        For i = 0 To dData1.GetUpperBound(0) - AryIgnore.Length
            iData2(i) = Ctype(dData2(i + (AryIgnore.Length - 1)), Int64)
        Next
        Array.Resize(iData2, dData2.Length - AryIgnore.Length)
#else
'フィルタパス
     Array.Copy(iData1, 0, iData2, 0, iData2.Length)
#end if
    End If
'前処理フィルタ結果出力
'Dim fs As New FileStream("D:\Test\PostFilter.txt", FileMode.Create, FileAccess.Write, FileShare.ReadWrite)
'Dim sw As Textwriter = New StreamWriter(fs, Text.Encoding.UTF8)
'Dim sLineBuf As String = nothing
''テキストファイル(CSV)書込み
'For i = 0 To dData2.GetUpperBound(0)
'    sLineBuf = dData2(i).ToString
'    sw.WriteLine(sLineBuf)
'Next i
'sw.Close()
'sw = nothing
Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
    Return -1
End Try
    Return 0
End Function
#end If


Dim		dAz(0 To 50)	As Double						'遅延素子　配列50点
Dim		iaZCnt			As Int32 = 0					'サンプルカウントaZ  規定配列分までのカウント
Public	dZaa			As Double
''' <summary>
''' 遅延素子生成
''' </summary>
''' <param name="dSampl">サンプルデータ</param>
''' <returns></returns>
Public Function FIR2( ByVal dSampl As Double, Byref dOut As Double ,ByRef Coef() As Double) As Int32
Dim i As int32
Try
	Array.Copy(dAz, 0, dAz, 1, dAz.Length-1)			'最後尾を廃棄
	dAz(0) = dSampl										'新データをセット

	fir_p(0) = dAz(0) * Coef(0) + dAz(1) * Coef(1)
	For i=1 To fir_p.getupperbound(0)
		fir_p(i) = fir_p(i-1) + dAz(i+1) * Coef(i+1)
	Next i
	dOut = fir_p(fir_p.getupperbound(0))
	Return 0
Catch ex As OverflowException
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Return -1

Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Return -1
End Try
End Function
Dim fir_p(0 To 49)	As Double
Dim fir_a()			As Double = {
6.237074932031003e-19, _
6.519365368015781e-04, _
1.243193145206731e-03, _
1.545341331314938e-03, _
1.219690279417956e-03, _
-1.308633300379173e-18, _
-2.015481444497656e-03, _
-4.150521278297992e-03, _
-5.226906703516521e-03, _
-4.024258869593409e-03, _
3.101792343338086e-18, _
6.064704797548080e-03, _
1.190235456808759e-02, _
1.435154478230210e-02, _
1.065089255751572e-02, _
-5.318258814903768e-18, _
-1.529750482053906e-02, _
-2.976146660162287e-02, _
-3.603425196620001e-02, _
-2.729525630881510e-02, _
7.111417857862681e-18, _
4.411304735764225e-02, _
9.765054582794508e-02, _
1.491778522165045e-01, _
1.864192084151082e-01, _
2.000000000000000e-01, _
1.864192084151082e-01, _
1.491778522165045e-01, _
9.765054582794508e-02, _
4.411304735764225e-02, _
7.111417857862681e-18, _
-2.729525630881510e-02, _
-3.603425196620001e-02, _
-2.976146660162287e-02, _
-1.529750482053906e-02, _
-5.318258814903768e-18, _
1.065089255751572e-02, _
1.435154478230210e-02, _
1.190235456808759e-02, _
6.064704797548080e-03, _
3.101792343338086e-18, _
-4.024258869593409e-03, _
-5.226906703516521e-03, _
-4.150521278297992e-03, _
-2.015481444497656e-03, _
-1.308633300379173e-18, _
1.219690279417956e-03, _
1.545341331314938e-03, _
1.243193145206731e-03, _
6.519365368015781e-04, _
6.237074932031003e-19 _
}



End Class
