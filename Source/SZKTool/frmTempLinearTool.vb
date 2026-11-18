Imports System.Runtime.InteropServices
Imports System
Imports System.IO
Imports System.Text
Imports SZKTool.frmOptions
Imports SZKTool.frmMain

Public Class frmTempLinearTool
Public Structure LinearType
	Dim iLinear		As Int32			'0:非リニア	1:リニア
	Dim dTime		As Double			'時間(S)
	Dim dTemp		As Double			'温度(deg C)
End Structure
Private mObj(2)		As LinearType

Sub New()

    ' この呼び出しはデザイナーで必要です。
    InitializeComponent()

    ' InitializeComponent() 呼び出しの後で初期化を追加します。

End Sub
''' <summary>
''' フォームロード
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub frmTempLinearTool_Load(sender As Object, e As EventArgs) Handles MyBase.Load
Try
	'言語切替
	Call SetFormLanguage(Me, p_iLanguage)

	mObj = p_cts.MLinerInfo
	nudWaitTime.Value	= mObj(0).dTime			'ウェイト時間
	nudStartTemp.Value	= mObj(0).dTemp			'開始温度
	nudEndTemp.Value	= mObj(1).dTemp			'終了温度
	'昇温速度
	If mObj(1).dTime <> 0
		Dim dData As Double = (mObj(1).dTemp - mObj(0).dTemp)/(mObj(1).dTime - mObj(0).dTime) * 60.0
		If dData > nudSpeed.Maximum
			nudSpeed.Value		= nudSpeed.Maximum
		Else
			nudSpeed.Value		= dData
		End If
	End If
	'温度設定フィールド範囲設定
	nudStartTemp.Maximum	= p_cts.TempCnt_HUpperLimit
	nudStartTemp.Minimum	= HEATER_LWLMT_NUD
	nudEndTemp.Maximum		= p_cts.TempCnt_HUpperLimit
	nudEndTemp.Minimum		= HEATER_LWLMT_NUD

Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub
Private Sub btnOk_Click(sender As Object, e As EventArgs) Handles btnOk.Click
Dim dSpeed As Double
Try
	mObj(0).dTime		= nudWaitTime.Value		'ウェイト時間
	mObj(0).dTemp		= nudStartTemp.Value	'開始温度
	mObj(1).dTemp		= nudEndTemp.Value		'終了温度
	If mObj(0).dTemp > mObj(1).dTemp
	'降温設定時
		dSpeed = -nudSpeed.Value
	Else
		dSpeed = nudSpeed.Value
	End If
	mObj(1).dTime		= Math.Round((nudEndTemp.Value-nudStartTemp.Value) / dSpeed * 60, 0) + nudWaitTime.Value		'終了時間
	mObj(0).iLinear		= 0
	mObj(1).iLinear		= 1
	Me.Close()
Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub

Private Sub btnCansel_Click(sender As Object, e As EventArgs) Handles btnCansel.Click
	Me.Close()
End Sub
End Class