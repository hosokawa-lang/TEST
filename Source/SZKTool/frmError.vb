Public Class frmError
Private Sw1							As New Stopwatch
Private m_Reset						As Int16	= 0				'1:リセットボタン済み
Private Const cDevErr1OrgMaskbit	As UInt16	= &h7F9C		'原点復帰が必要なエラー1のマスクビット
Private Const cDevErr2OrgMaskbit	As UInt16	= &h03BF		'原点復帰が必要なエラー2のマスクビット
Private Const cDevErr3OrgMaskbit	As UInt16	= &h8087		'原点復帰が必要なエラー3のマスクビット
'多言語メッセージ格納
Private m_sStr(eMes.iMAX - 1)		 As String   
Enum eMes
    iLockRelease = 0	'原点復帰する前にプローブロックを解除してください。
	iMAX
End Enum
Sub New

    ' この呼び出しはデザイナーで必要です。
    InitializeComponent()

    ' InitializeComponent() 呼び出しの後で初期化を追加します。

End Sub
'フォームロード
Private Sub frmError_Load(sender As Object, e As EventArgs) Handles Me.Load
	'言語指定による
    Call SetFormLanguage(Me, p_iLanguage,,m_sStr)
	btnRetOrigin.Visible = False
	'm_Reset = 0
	Me.TopMost = True

	AllOrderBitOff()			'全ての指令ビットをOFF
	p_RTM(0).p_iError = 1
	p_RTM(0).p_MainLink = 0		'frmMainでの通信リンク(書込み)


	MachineErrorLogOut()		'エラー状態をログ出力
    Timer1.Interval = 100
    Timer1.Enabled = True
End Sub
Dim iJob As Int16 = 0
'タイマーインターバル処理
Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
	Timer1.Enabled = False
	MachineErrorDisp()        '本体エラー状態表示


	Select Case iJob
	Case 0		'アイドル
		AllOrderBitOff()			'全ての指令ビットをOFF
		If (p_RTM(0).p_mltcp.DevErr1 And cDevErr1OrgMaskbit)<>0 Or 	(p_RTM(0).p_mltcp.DevErr2 And cDevErr2OrgMaskbit)<>0 Or	(p_RTM(0).p_mltcp.DevErr3 And cDevErr3OrgMaskbit)<>0 Or _
			Not p_RTM(0).p_mltcp.isStatOriginComp()
		'原点復帰が必要なエラー
			iJob = 100

'Label999.Text = ""


		Else
			btnRetOrigin.Visible = False
			iJob = 200
		End If
	
	Case 50	'リセット操作待ち(原点復帰が必要)
		If (p_RTM(0).p_mltcp.DevErr1 And cDevErr1OrgMaskbit)<>0 Or 	(p_RTM(0).p_mltcp.DevErr2 And cDevErr2OrgMaskbit)<>0 Or	(p_RTM(0).p_mltcp.DevErr3 And cDevErr3OrgMaskbit)<>0 Or _
			Not p_RTM(0).p_mltcp.isStatOriginComp()
		'原点復帰が必要なエラー

'Label999.Text = ""

			iJob = 100
		Else
			btnRetOrigin.Visible = False
			iJob = 200
		End If

	Case 100	'リセット操作待ち(原点復帰が必要)
		If m_Reset = 1 Or p_RTM(0).p_mltcp.isStatResetSW Or p_RTM(0).p_mltcp.ErrReset
			iJob = 250
		End If

	Case 200	'リセット操作待ち(原点復帰が不要)
		If m_Reset = 1 Or p_RTM(0).p_mltcp.isStatResetSW Or p_RTM(0).p_mltcp.ErrReset
			iJob = 900
		End If
	Case 250
		Dim f As New frmMSG(m_sStr(eMes.iLockRelease), {"OK"}, Application.ProductName, Me)
		f.ShowDialog
		iJob = 300


	Case 300	'原点復帰操作待ち
		If p_RTM(0).iResetGo = 1
			p_RTM(0).iResetGo = 0
			iJob = 310
			btnRetOrigin.Visible = True
			'Threading.Thread.Sleep(1000)
		End If

	Case 310	'本体リセット処理待ち
		iJob = 350

	Case 350	'原点復帰操作待ち
		If Sw1.IsRunning Or p_RTM(0).p_mltcp.isStatOriginSW 
		'原点復帰ボタンが押された
			iJob = 400
		ElseIf p_RTM(0).ErrorDetect() 
			m_Reset = 0
			Sw1.Stop
			btnRetOrigin.Visible = False
			iJob = 100
''Label999.Text = "原点復帰操作待ちでエラー再発生"
		End If

	Case 400	'原点復帰完了確認
		iJob = 450
		
	Case 450	'原点復帰完了確認
		If p_RTM(0).p_mltcp.isStatOriginComp()							
			iJob = 900
		Elseif Sw1.Elapsed.TotalSeconds > RealTimeMeas.cOriginSearchTimeLimit
			P_RTM(0).p_mltcp.PosOrderOrigin(False)
			iJob = 50
		ElseIf p_RTM(0).ErrorDetect()		'原点復帰中にエラー発生していないか確認
			m_Reset = 0
			Sw1.Stop
			btnRetOrigin.Visible = False
			P_RTM(0).p_mltcp.PosOrderOrigin(False)
			iJob = 50
		End If

	Case 900	'最終エラー確認
		iJob = 950

	Case 950	'最終エラー確認
	'エラー無くなれば自動消滅
		If ( p_RTM(0).p_mltcp.DevErr1 = 0 And p_RTM(0).p_mltcp.DevErr2 = 0 And p_RTM(0).p_mltcp.DevErr3 = 0 And p_RTM(0).p_mltcp.isStatOriginComp()) Or _
			p_RTM(0).p_iConnectState = CONNECT_ST.OFFLINE
			
			Sw1.Stop
			Me.Close
			Me.Dispose
			Exit Sub

		End If
	Case Else
		Me.Close
		Exit Sub

	End Select
	Timer1.Enabled = True

End Sub
''' <summary>
''' エラー内容をランプ表示
''' </summary>
Public Sub MachineErrorDisp()
	With p_RTM(0)

	Lbl1.BackColor = IIf(.p_mltcp.isErrXStressOver,			Color.Orange, SystemColors.Control)
	Lbl2.BackColor = IIf(.p_mltcp.isErrYStressOver,			Color.Orange, SystemColors.Control)
	Lbl3.BackColor = IIf(.p_mltcp.isErrStressDetectUp,		Color.Orange, SystemColors.Control)
	Lbl4.BackColor = IIf(.p_mltcp.isErrStressDetectDown,	Color.Orange, SystemColors.Control)
	Lbl5.BackColor = IIf(.p_mltcp.isErrThermoDiscon,		Color.Orange, SystemColors.Control)
	Lbl6.BackColor = IIf(.p_mltcp.isErrNoHeatUp,			Color.Orange, SystemColors.Control)
	Lbl7.BackColor = IIf(.p_mltcp.isErrHeaterHighDetect,	Color.Orange, SystemColors.Control)
	Lbl8.BackColor = IIf(.p_mltcp.isErrTermoDiscon2,		Color.Orange, SystemColors.Control)
	Lbl9.BackColor = IIf(.p_mltcp.isErrNoHeatUp2,			Color.Orange, SystemColors.Control)
	Lbl10.BackColor = IIf(.p_mltcp.isErrHeaterHighDetect2,	Color.Orange, SystemColors.Control)
	Lbl11.BackColor = IIf(.p_mltcp.isErrXMoveAlarm,			Color.Orange, SystemColors.Control)
	Lbl12.BackColor = IIf(.p_mltcp.isErrXMotorAlm,			Color.Orange, SystemColors.Control)
	Lbl13.BackColor = IIf(.p_mltcp.isErrXMoveTimeout,		Color.Orange, SystemColors.Control)
	Lbl14.BackColor = IIf(.p_mltcp.isErrXNoOrigin,			Color.Orange, SystemColors.Control)
	Lbl15.BackColor = IIf(.p_mltcp.isErrYMoveAlarm,			Color.Orange, SystemColors.Control)
	lbl16.BackColor = IIf(.p_mltcp.isErrYMotorAlm,			Color.Orange, SystemColors.Control)
	lbl17.BackColor = IIf(.p_mltcp.isErrYMoveTimeout,		Color.Orange, SystemColors.Control)
	lbl18.BackColor = IIf(.p_mltcp.isErrYNoOrigin,			Color.Orange, SystemColors.Control)
	Lbl19.BackColor = IIf(.p_mltcp.isErrZMoveAlarm,			Color.Orange, SystemColors.Control)
	Lbl20.BackColor = IIf(.p_mltcp.isErrZMotorAlm,			Color.Orange, SystemColors.Control)
	Lbl21.BackColor = IIf(.p_mltcp.isErrZMoveTimeout,		Color.Orange, SystemColors.Control)
	Lbl22.BackColor = IIf(.p_mltcp.isErrZNoOrigin,			Color.Orange, SystemColors.Control)
	Lbl23.BackColor = IIf(.p_mltcp.isErrNoClose,			Color.Orange, SystemColors.Control)
	Lbl24.BackColor = IIf(.p_mltcp.isErrHPOverheat,			Color.Orange, SystemColors.Control)
	Lbl25.BackColor = IIf(.p_mltcp.isErrCoverOpen,			Color.Orange, SystemColors.Control)
	Lbl26.BackColor = IIf(.p_mltcp.isErrEmgStp,				Color.Orange, SystemColors.Control)
	Lbl27.BackColor = IIf(.p_mltcp.isErrBUAreaErased,		Color.Orange, SystemColors.Control)
	Lbl28.BackColor = IIf(.p_mltcp.isErrXUpLimit,			Color.Orange, SystemColors.Control)
	Lbl29.BackColor = IIf(.p_mltcp.isErrYUpLimit,			Color.Orange, SystemColors.Control)
	Lbl30.BackColor = IIf(.p_mltcp.isErrXStressCom,			Color.Orange, SystemColors.Control)
	Lbl31.BackColor = IIf(.p_mltcp.isErrYStressCom,			Color.Orange, SystemColors.Control)
	Lbl32.BackColor = IIf(.p_mltcp.isErrZStressCom,			Color.Orange, SystemColors.Control)
	Lbl33.BackColor = IIf(.p_mltcp.isErrExternalInt,		Color.Orange, SystemColors.Control)
	Lbl34.BackColor = IIf(.p_mltcp.isErrXOos,				Color.Orange, SystemColors.Control)
	Lbl35.BackColor = IIf(.p_mltcp.isErrYOos,				Color.Orange, SystemColors.Control)
	Lbl36.BackColor = IIf(.p_mltcp.isErrZOos,				Color.Orange, SystemColors.Control)
	Lbl37.BackColor = IIf(.p_mltcp.isErrZStressOver,		Color.Orange, SystemColors.Control)
	Lbl38.BackColor = IIf(.p_mltcp.isErrPBOverheat,			Color.Orange, SystemColors.Control)
	Lbl39.BackColor = IIf(.p_mltcp.isErrZCorrFail1,			Color.Orange, SystemColors.Control)
	Lbl40.BackColor = IIf(.p_mltcp.isErrZCorrFail2,			Color.Orange, SystemColors.Control)
	Lbl41.BackColor = IIf(.p_mltcp.isErrSGTCDis,			Color.Orange, SystemColors.Control)
	Lbl42.BackColor = IIf(.p_mltcp.isErrSGTNoHeatUp,		Color.Orange, SystemColors.Control)
	Lbl43.BackColor = IIf(.p_mltcp.isErrSGTHighDetect,		Color.Orange, SystemColors.Control)
	Lbl44.BackColor = IIf(.p_mltcp.isErrHPFanStop,			Color.Orange, SystemColors.Control)
	Lbl45.BackColor = IIf(.p_mltcp.isErrZUpperDetect,		Color.Orange, SystemColors.Control)
	Lbl46.BackColor = IIf(.p_mltcp.isErrZVibeDetect,		Color.Orange, SystemColors.Control)
	Lbl47.BackColor = IIf(.p_mltcp.isErrSGOverheat,			Color.Orange, SystemColors.Control)
	Lbl48.BackColor = IIf(.p_bDevInternalComError,			Color.Orange, SystemColors.Control)


	End With
End Sub
''' <summary>
''' エラー内容ログファイル出力(発生時のみ)
''' </summary>
Public Sub MachineErrorLogOut()
	With p_RTM(0)

	If .p_mltcp.isErrXStressOver
		Call WriteLog("装置エラー " & "X軸応力がレンジオーバーしました。", eLOG_LEVEL.iWARNING)
	End If
	If .p_mltcp.isErrYStressOver
		Call WriteLog("装置エラー " & "Y軸応力がレンジオーバーしました。", eLOG_LEVEL.iWARNING)
	End If
	If .p_mltcp.isErrStressDetectUp
		Call WriteLog("装置エラー " & "熱板上昇中に規定以上の応力を検知しました。", eLOG_LEVEL.iWARNING)
	End If
	If .p_mltcp.isErrStressDetectDown
		Call WriteLog("装置エラー " & "熱板下昇中に規定以上の応力を検知しました。", eLOG_LEVEL.iWARNING)
	End If
	If .p_mltcp.isErrThermoDiscon
		Call WriteLog("装置エラー " & "熱板の熱電対の断線を検知しました。", eLOG_LEVEL.iWARNING)
	End If
	If .p_mltcp.isErrNoHeatUp
		Call WriteLog("装置エラー " & "熱板ヒータの昇温速度が遅い。", eLOG_LEVEL.iWARNING)
	End If
	If .p_mltcp.isErrHeaterHighDetect
		Call WriteLog("装置エラー " & "熱板ヒータの温度が限界最大値を超えたことを検知しました。", eLOG_LEVEL.iWARNING)
	End If
	If .p_mltcp.isErrTermoDiscon2
		Call WriteLog("装置エラー " & "プローブの熱電対の断線を検知しました。", eLOG_LEVEL.iWARNING)
	End If
	If .p_mltcp.isErrNoHeatUp2
		Call WriteLog("装置エラー " & "プローブヒータの昇温速度が遅い。", eLOG_LEVEL.iWARNING)
	End If
	If .p_mltcp.isErrHeaterHighDetect2
		Call WriteLog("装置エラー " & "プローブヒータの温度が限界最大値を超えたことを検知しました。", eLOG_LEVEL.iWARNING)
	End If
	If .p_mltcp.isErrXMoveAlarm
		Call WriteLog("装置エラー " & "X軸駆動中に原点センサを検知しました。", eLOG_LEVEL.iWARNING)
	End If
	If .p_mltcp.isErrXMotorAlm
		Call WriteLog("装置エラー " & "X軸モータの異常が発生しました。", eLOG_LEVEL.iWARNING)
	End If
	If .p_mltcp.isErrXMoveTimeout
		Call WriteLog("装置エラー " & "X軸駆動のタイムアウトが発生しました。", eLOG_LEVEL.iWARNING)
	End If
	If .p_mltcp.isErrXNoOrigin
		Call WriteLog("装置エラー " & "X軸原点センサーが異常です。", eLOG_LEVEL.iWARNING)
	End If
	If .p_mltcp.isErrYMoveAlarm
		Call WriteLog("装置エラー " & "Y軸駆動中に原点センサを検知しました。", eLOG_LEVEL.iWARNING)
	End If
	If .p_mltcp.isErrYMotorAlm
		Call WriteLog("装置エラー " & "Y軸モータの異常が発生しました。", eLOG_LEVEL.iWARNING)
	End If
	If .p_mltcp.isErrYMoveTimeout
		Call WriteLog("装置エラー " & "Y軸駆動のタイムアウトが発生しました。", eLOG_LEVEL.iWARNING)
	End If
	If .p_mltcp.isErrYNoOrigin
		Call WriteLog("装置エラー " & "Y軸原点センサーが異常です。", eLOG_LEVEL.iWARNING)
	End If
	If .p_mltcp.isErrZMoveAlarm
		Call WriteLog("装置エラー " & "Z軸駆動中に原点センサを検知しました。", eLOG_LEVEL.iWARNING)
	End If
	If .p_mltcp.isErrZMotorAlm
		Call WriteLog("装置エラー " & "Z軸モータの異常が発生しました。", eLOG_LEVEL.iWARNING)
	End If
	If .p_mltcp.isErrZMoveTimeout
		Call WriteLog("装置エラー " & "Z軸駆動のタイムアウトが発生しました。", eLOG_LEVEL.iWARNING)
	End If
	If .p_mltcp.isErrZNoOrigin
		Call WriteLog("装置エラー " & "Z軸原点センサーが異常です。", eLOG_LEVEL.iWARNING)
	End If
	If .p_mltcp.isErrNoClose
		Call WriteLog("装置エラー " & "安全カバーが閉じられていません。", eLOG_LEVEL.iWARNING)
	End If
	If .p_mltcp.isErrHPOverheat
		Call WriteLog("装置エラー " & "熱板温度が高温監視点を超えました。", eLOG_LEVEL.iWARNING)
	End If
	If .p_mltcp.isErrCoverOpen
		Call WriteLog("装置エラー " & "安全カバーが開いています。", eLOG_LEVEL.iWARNING)
	End If
	If .p_mltcp.isErrEmgStp
		Call WriteLog("装置エラー " & "非常停止スイッチが入力されました。", eLOG_LEVEL.iWARNING)
	End If
	If .p_mltcp.isErrBUAreaErased
		Call WriteLog("装置エラー " & "バックアップ記憶領域がクリアされました。", eLOG_LEVEL.iWARNING)
	End If
	If .p_mltcp.isErrXUpLimit
		Call WriteLog("装置エラー " & "X軸上限センサを検知しました。", eLOG_LEVEL.iWARNING)
	End If
	If .p_mltcp.isErrYUpLimit
		Call WriteLog("装置エラー " & "Y軸上限センサを検知しました。", eLOG_LEVEL.iWARNING)
	End If
	If .p_mltcp.isErrXStressCom
		Call WriteLog("装置エラー " & "X軸応力センサとの通信異常が発生しました。", eLOG_LEVEL.iWARNING)
	End If
	If .p_mltcp.isErrYStressCom
		Call WriteLog("装置エラー " & "Y軸応力センサとの通信異常が発生しました。", eLOG_LEVEL.iWARNING)
	End If
	If .p_mltcp.isErrZStressCom
		Call WriteLog("装置エラー " & "Z軸応力センサとの通信異常が発生しました。", eLOG_LEVEL.iWARNING)
	End If
	If .p_mltcp.isErrExternalInt
		Call WriteLog("装置エラー " & "外部からのインターロックを検出しました。", eLOG_LEVEL.iWARNING)
	End If
	If .p_mltcp.isErrXOos
		Call WriteLog("装置エラー " & "X軸の脱調が発生しました。", eLOG_LEVEL.iWARNING)
	End If
	If .p_mltcp.isErrYOos
		Call WriteLog("装置エラー " & "Y軸の脱調が発生しました。", eLOG_LEVEL.iWARNING)
	End If
	If .p_mltcp.isErrZOos
		Call WriteLog("装置エラー " & "Z軸の脱調が発生しました。", eLOG_LEVEL.iWARNING)
	End If
	If .p_mltcp.isErrZStressOver
		Call WriteLog("装置エラー " & "Z軸応力がレンジオーバーしました。", eLOG_LEVEL.iWARNING)
	End If
	If .p_mltcp.isErrPBOverheat
		Call WriteLog("装置エラー " & "プローブ温度が高温監視点を超えました。", eLOG_LEVEL.iWARNING)
	End If
	If .p_mltcp.isErrZCorrFail1
		Call WriteLog("装置エラー " & "Z軸補正時の応力センサが測定範囲外になりました。", eLOG_LEVEL.iWARNING)
	End If
	If .p_mltcp.isErrZCorrFail2
		Call WriteLog("装置エラー " & "Z軸補正時の応力センサとの通信が異常になりました", eLOG_LEVEL.iWARNING)
	End If
	If .p_mltcp.isErrSGTCDis
		Call WriteLog("装置エラー " & "起歪体の熱電対の断線を検知しました。", eLOG_LEVEL.iWARNING)
	End If
	If .p_mltcp.isErrSGTNoHeatUp
		Call WriteLog("装置エラー " & "起歪体ヒータの昇温速度が遅い。", eLOG_LEVEL.iWARNING)
	End If
	If .p_mltcp.isErrSGTHighDetect
		Call WriteLog("装置エラー " & "起歪体ヒータの温度が限界最大値を超えたことを検知しました。", eLOG_LEVEL.iWARNING)
	End If
	If .p_mltcp.isErrHPFanStop
		Call WriteLog("装置エラー " & "熱板用FANが停止しました。", eLOG_LEVEL.iWARNING)
	End If
	If .p_mltcp.isErrZUpperDetect
		Call WriteLog("装置エラー " & "Z軸の駆動中に上限センサを検知しました", eLOG_LEVEL.iWARNING)
	End If
	If .p_mltcp.isErrZVibeDetect
		Call WriteLog("装置エラー " & "指定圧力になる前にZ軸振動が始まりました。", eLOG_LEVEL.iWARNING)
	End If
	If .p_mltcp.isErrSGOverheat
		Call WriteLog("装置エラー " & "起歪体温度が高温監視点を超えました。", eLOG_LEVEL.iWARNING)
	End If
	If .p_bDevInternalComError
		Call WriteLog("装置エラー " & "装置内部の通信エラーが発生しました。", eLOG_LEVEL.iWARNING)
	End If



	End With
End Sub
'エラーリセットボタン
Private Sub btnErrReset_Click(sender As Object, e As EventArgs) Handles btnErrReset.Click
Try
    p_RTM(0).p_iBGWOrder = BGW_ORDER.RESET_ERROR_REP
	m_Reset = 1
	p_RTM(0).p_iError = 0
	Call WriteLog("装置エラー画面 エラーリセットボタンがクリックされました。", eLOG_LEVEL.iINFO)
Catch ex As Exception
    Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
    Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub
'原点復帰ボタン
Private Sub btnRetOrigin1_Click(sender As Object, e As EventArgs) Handles btnRetOrigin.Click
Try
	if p_RTM(0).p_iBGWOrder = BGW_ORDER.NO_ORDER
		'If p_RTM(0).p_mltcp.isILState()　Or p_RTM(0).p_mltcp.isILDisable			'IL(扉閉) OKか? IL無効も見る
			frmMain.btnZero.Enabled		= False			'ゼロ調ボタンの無効化
			frmMain.btnMeasure.Enabled	= False         '測定ボタンの無効化

			Call WriteLog("装置エラー画面 原点復帰ボタンがクリックされました。", eLOG_LEVEL.iINFO)

			p_RTM(0).p_iBGWOrder = BGW_ORDER.ORG_BUTTON_REP
			Sw1.Restart()
		'End if
	End if

Catch ex As Exception
    Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
    Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub
'閉じるボタン
Private Sub Button1_Click(sender As Object, e As EventArgs) Handles btnClose.Click
	Me.Close
	Me.Dispose
End Sub
'全ての指令ビットをオフする
Private Sub AllOrderBitOff()
	If (p_RTM(0).p_mltcp.DevErr1 AND &h8070) Or _
		(p_RTM(0).p_mltcp.DevErr2 AND &h1C02) Or _
		(p_RTM(0).p_mltcp.DevErr3 AND &h7820)
		p_RTM(0).p_mltcp.StartTempCont = 0		'温度制御OFF
	End If
	'異常時は温度以外の指令ビットを全てOFF
	p_RTM(0).p_mltcp.StartSW		= 0
	p_RTM(0).p_mltcp.PosOrder		= 0
	p_RTM(0).p_mltcp.DateSetOrder	= 0
	p_RTM(0).p_mltcp.PosOrderOutOfStepConf(False)
End Sub
End Class