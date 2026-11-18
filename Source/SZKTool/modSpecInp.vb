Imports System.Drawing.Printing
Imports System.Threading
Imports System.IO
Imports System.text
Imports System.Windows.Forms
Imports System.Windows.Forms.DataVisualization.Charting
Imports System.Collections
Imports System.Globalization
Imports System.ComponentModel
Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.ApplicationServices
Imports Microsoft.VisualBasic.FileIO
Imports SZKTool.frmOptions
Imports SZKTool.RealTimeMeas
Imports SZKTool.modPublic
Imports SZKTool.DataFile
Imports SZKTool.frmAbout
Imports SZKTool.frmMain
''' <summary>
''' SPEC管理のクラス
''' </summary>
Public Class clSpec
'スペックメンバ構造体
Public structure SpecInfo
	Dim sDeviceSpecName		As String								'デバイスSPEC名 保存ファイル名にも使用 自由入力：Free
	Dim iHPPosCont			As Int32								'熱板位置制御		0:GAP 1:着圧指定 2:常時圧力
	Dim iGelTimeMethod		As Int32								'ゲルタイム判定方法 0:THR 1:SLOPE 2:EXTRAPOLATION 3:PHASE 4:MELT
	Dim dThreshold			As Double								'判定値 [%]
	Dim dHPSetTemp			As Double								'熱板温度設定 [℃]
	Dim dPBSetTemp			As Double								'プローブ温度設定 [℃]
	Dim dSampleGap			As Double								'サンプル投入GAP [mm]			Optionから
	Dim dMeasureGap			As Double								'測定中GAP [mm]
	Dim dMeasureStress		As Double								'測定中圧力 [kPa]
	Dim iPressContStep		As int32								'着圧制御ステップ数
	Dim dProbDiameter		As Double								'プローブ直径 [mm]
	Dim iStressSensMode		As int32								'動作軌跡				1:線 0:円 2:点
	'Dim iProbInputMode		As int32								'プローブ動作設定の入力方式	0:直接 1:間接
	Dim iAmpMode			As Int32								'振幅測定モード 0:振幅一定 1:圧力一定
	Dim dXAmplitude			As Double								'振幅X [mm]
	Dim dYAmplitude			As Double								'振幅Y [mm]
	Dim dZAmplitude			As Double								'振幅Y [mm]
	Dim dXPeriod			As Double								'周期X [秒]
	Dim dYPeriod			As Double								'周期Y [秒]
	Dim dZPeriod			As Double								'周期Z [秒]
	Dim dShearSpeed			As Double								'せん断速度 [1/s]
	Dim dSampleStrain		As Double								'サンプル歪 [L/t]
	Dim dAmpModeStress		As Double								'振幅測定モード 応力振幅P-P 上限応力(%)
	Dim dAmpModeStressN		As Double								'上限応力値(uN)
	Dim dInputToHPUpWait	As Double								'サンプル投入から熱板上昇開始までの時間 [秒]
	Dim dHPUpToXYActWait	As Double								'熱板上昇開始からXY動作開始までの時間 [秒]
	Dim dMeasureTime		As Double								'測定時間設定 [秒]
	Dim dMeasureEndStress	As Double								'測定終了応力 [%]
	Dim sTempProfileName	As String								'温度プロファイル名 空:使用しない

	Dim dDiffInterval		As Double								'SLOPE微分区間(秒)

	Dim dSampleInputWait	As Double								'サンプル投入待ち時間 [秒]		Optionから
	Dim dHPTempOkRange		As Double								'熱板温度許容範囲 [℃]			Optionから
	Dim dPBTempOkRange		As Double								'プローブ温度許容範囲 [℃]		Optionから

	Dim sSampleSpecName		As String								'サンプルSPEC名 保存ファイル名にも使用 自由入力：Free
	Dim sSampleName			As String								'サンプル名
	Dim sLot				As String								'ロットNo
	Dim sMeasurer			As String								'測定者
	Dim iShape				As int32								'サンプル形状 0:シート 1:粉 2:液 3:RCC
	Dim sThickness			As String								'サンプル厚み [mm]
	Dim dStack				As Double								'積層枚数
	Dim dEffectRangeMin		As Double								'データ有効範囲 最小 [秒]
	Dim dEffectRangeMax		As Double								'データ有効範囲 最大 [秒]
	Dim dJudgeRangeMin		As Double								'GT判定範囲 最小 [秒]
	Dim dJudgeRangeMax		As Double								'GT判定範囲 最大 [秒]
End Structure

Public	spi					As SpecInfo								'SPEC入力情報保持

Private	iEventFrom			As Int16 = 0							'どこからのイベント 0:frmMain 1:frmSpecNameInp

Public	dtDevSpec			As DataTable							'デバイスSPEC データテーブル
Public	dtSamSpec			As DataTable							'サンプルSPEC データテーブル
Private	dr					As Datarow								'データROW(ワーク用)


Public	Enum eSpec
	Device		= 0			'デバイスSpec
	Sample					'サンプルSpec
End Enum
'デバイスSPECアイテム順番
Public Enum eDevSPEC
	HP_CONT			= 0
	METHOD
	THRETHOLD
	TEMPERATURE
	MEAS_GAP		= 4
	MEAS_STRESS
	PRESSCONT_STEP	
	PB_DIAMETER
	STRS_SENS_MODE	
	AMP_MODE		= 9
	AMPLITUDE		
	PERIOD
	AMPMODE_STRESS
	HP_UP_WAIT
	ACTION_WAIT		= 14
	MEAS_TIME
	MEAS_END_STRESS
	TEMP_PROFILE
	DIFF_INTERVAL

	FINAL
End Enum


''' <summary>
''' デバイスSPEC数値入力範囲
''' 最小値-最大値、文字列へのフォーマットは最小値の小数点位置が使用される
''' </summary>
Dim sDevInputRange() As String ={ _
	"", _								'0
	"", _
	"0.0-100.0", _
	"0.0-240.0", _
	"0.000-40.000", _
	"0.0-50.0", _
	"1-10", _
	"0.0-50.0", _
	"", _
	"", _								'10		AMP_MODE
	"0.000-10.000", _					
	"0.2-10.0", _
	"0.0-100.0", _						'13		AMPMODE_STRESS
	"0.0-3600.0", _
	"0.0-3600.0", _						'15
	"0.0-3600.0", _						'
	"0.0-100.0", _						'
	"", _								'
	"0.000-100.000" _						'
}
'測定時間数値入力範囲
Dim sDevInputRange_MeasTime() As String = { _
	"0.0-3600.0", _						'通常時
	"0.0-864000.0" _					'間欠測定時 10DAY
}


''' <summary>
''' データテーブル定義列名
''' </summary>
Public sColumn() As String ={ _
	"Items", _
	"Units", _
	"Values" _
}
''' <summary>
''' デバイスSPEC 設定項目名
''' </summary>
Public sDevItem() As String ={ _
	"熱板位置制御", _
	"ゲルタイム判定法", _
	"判定値", _
	"測定温度(熱板/プローブ)", _
	"測定GAP", _
	"目標圧力", _
	"着圧制御分解能", _
	"プローブ直径", _
	"応力検知モード", _
	"振幅測定モード", _
	"振幅", _
	"周期", _
	"上限応力", _
	"投入から熱板上昇開始まで", _
	"圧着後の待機時間", _
	"測定終了時間", _
	"測定終了応力", _
	"温度プロファイル", _
	"微分区間(SLOPE)" _
}
Public sDevItemHint() As String ={ _
	"熱板位置制御", _
	"ゲルタイム判定法", _
	"判定値", _
	"測定温度(熱板/プローブ)", _
	"測定GAP", _
	"目標圧力", _
	"着圧制御分解能", _
	"プローブ直径", _
	"応力検知モード", _
	"振幅測定モード", _
	"振幅", _
	"周期", _
	"上限応力", _
	"投入から熱板上昇開始まで", _
	"圧着後の待機時間", _
	"測定終了時間 以下の入力方式が使用可 分単位=9m 時間単位=9h 時分分割=h:m", _
	"測定終了応力", _
	"温度プロファイル", _
	"微分区間(SLOPE)" _
}
''' <summary>
''' デバイスSPEC 設定項目単位
''' </summary>
Public sDevUnit() As String ={ _
	"", _
	"", _
	"%", _
	"℃", _
	"mm", _
	"KPa", _
	"um", _
	"mm", _
	"", _
	"", _
	"mm", _
	"秒", _
	"%", _
	"秒", _
	"秒", _
	"秒", _
	"%", _
	"", _
	"秒" _
}

'熱板位置制御
Public sHCont()			As String ={ "GAP", "着圧指定", "常時圧力" }

'ゲルタイム判定法
''Dim sGelMethod()		As String ={ "THR", "SLOPE","EXTRAPOLATION","PHASE","MELT" }
'応力検知モード

Public sStrsSens()		As String ={"円","線","点" }

''プローブ動作設定の入力方式
'Public sProbInpMethod()	As String ={ "直接", "間接" }

'振幅測定モード
Public sAmpMode()		As String ={ "振幅一定", "応力一定" }


'プローブ動作設定の入力方式 により切り替えるアイテム名
Dim sDevSpecPBInpMode0_Item()		As String = {"振幅", "周期"}
Dim sDevSpecPBInpMode1_Item()		As String = {"せん断速度 x 3.14", "サンプルひずみ"}
Dim sDevSpecPBInpMode0_Unit()		As String = {"mm", "秒"}
Dim sDevSpecPBInpMode1_Unit()		As String = {"1/s", "L/t"}
Dim sDevSpecPBInpMode0_Range()		As String = {"0.000-10.000", "0.2-10.0"}
Dim sDevSpecPBInpMode1_Range()		As String = {"0.00000-500.00000", "0.00000-500.00000"}

'熱板位置制御内訳
Public	Enum eHCont
	Gap			= 0			'GAP
	OrderP					'着圧指定
	ConstantP				'常時圧力
End Enum
'応力検知モード内訳
Public	Enum eStrsSens
	Circle	= 0				'円
	Line					'線
	Point					'点
End Enum
'振幅測定モード内訳
Public	Enum eAmpMode
	AmpConst		= 0		'振幅一定
	StsConst				'応力一定
End Enum
'プローブ動作設定の入力方式内訳
Public	Enum ePBInpMode
	Direct		= 0			'直接
	Indirect				'間接
End Enum



'サンプルSPECアイテム順番
Public	Enum eSamSPEC
	SAMPLE		= 0
	LOT			
	MEASURER
	SHAPE
	THICKNESS
	STACKS
	EFECT_RANGE	= 6
	JUDGE_RANGE

	FINAL
End Enum
''' <summary>
''' サンプルSPEC数値入力範囲
''' 最小値-最大値、文字列へのフォーマットは最小値の小数点位置が使用される
''' </summary>
Public sSamInputRange() As String ={ _
	"", _					'0
	"", _					
	"", _
	"", _
	"0.000-30.000", _
	"1-99", _
	"0.0-3600.0", _			'6
	"0.0-3600.0" _
}
'有効データ、規格データ範囲
Public sSamInputRange_AppStdTime() As String = { _
	"0.0-3600.0", _						'通常時
	"0.0-864000.0" _					'間欠測定時 10DAY
}
''' <summary>
''' サンプルSPEC 設定項目名
''' </summary>
Public sSamItem() As String ={ _
	"サンプル名", _
	"ロットNo", _
	"測定者", _
	"形状", _
	"厚み", _
	"積層枚数", _
	"有効データ範囲", _
	"規格判定範囲" _
}
Public sSamItemHint() As String ={ _
	"サンプル名", _
	"ロットNo", _
	"測定者", _
	"形状", _
	"厚み", _
	"積層枚数", _
	"有効データ範囲", _
	"規格判定範囲" _
}
''' <summary>
''' デバイスSPEC 設定項目単位
''' </summary>
Public sSamUnit() As String ={ _
	"", _
	"", _
	"", _
	"", _
	"mm", _
	"枚", _
	"秒", _
	"秒" _
}

'形状
Public sShape()					As String = { "シート", "粉", "液", "RCC" }
Dim sSamSpecShape_hint()		As String = {"形状", "形状", "形状", "形状:厚みx積層枚数がサンプル厚として計算に使用されます。"}

Dim sSamSpecShape0_Item()		As String = {"厚み", "積層枚数"}
Dim sSamSpecShape1_Item()		As String = {"密度", "重量"}
Dim sSamSpecShape2_Item()		As String = {"密度", "体積"}

Dim sSamSpecShape0_hint()		As String = {"厚み", "積層枚数"}
Dim sSamSpecShape1_hint()		As String = {"密度", "重量"}
Dim sSamSpecShape2_hint()		As String = {"密度", "体積"}

Dim sSamSpecShape0_Unit()		As String = {"mm", "枚"}
Dim sSamSpecShape1_Unit()		As String = {"g/ml", "g"}
Dim sSamSpecShape2_Unit()		As String = {"g/ml", "ml"}
Dim sSamSpecShape0_Range()		As String = {"0.000-30.000", "1-99"}
Dim sSamSpecShape1_Range()		As String = {"0.000-20.000", "0.000-20.000"}
Dim sSamSpecShape2_Range()		As String = {"0.000-20.000", "0.000-10.000"}

'形状内訳
Public	Enum eShape
	Sheet			= 0		'シート
	Powder					'粉体
	Liquid					'液体
	Rcc						'RCC
End Enum




Dim sUnderMsg1()		As String = {"不正な文字が入力されました。", "設定範囲外の値が入力されました。"}


Private dgvTrgt			As DataGridView			'ワーク記憶用データグリッドビュー
Public const cFree		As String		= "Free"
Public const cQRCode	As String		= "QRCode"
Sub New(ByVal iEv As int16)
	iEventFrom = iEv					'どこからのイベントを処理するか
End Sub
''' <summary>
''' 多言語文字列初期化
''' </summary>
Public Sub SpecLangageSet(ByVal sMes() As String)
	Dim s() As String = Nothing

	s = sMes(efrmMainSTR.iDevItem).Split(",")
	Array.Copy(s, sDevItem, s.Length)

	s = sMes(efrmMainSTR.iDevUnit).Split(",")
	Array.Copy(s, sDevUnit, s.Length)

	s = sMes(efrmMainSTR.iHCont).Split(",")
	Array.Copy(s, sHCont, s.Length)

	s = sMes(efrmMainSTR.iStrsSens).Split(",")
	Array.Copy(s, sStrsSens, s.Length)

	s = sMes(efrmMainSTR.iAmpMode).Split(",")
	Array.Copy(s, sAmpMode, s.Length)

	's = sMes(efrmMainSTR.iProbInpMethod).Split(",")
	'Array.Copy(s, sProbInpMethod, s.Length)

	s = sMes(efrmMainSTR.iDevSpecPBInpMode0_Item).Split(",")
	Array.Copy(s, sDevSpecPBInpMode0_Item, s.Length)

	s = sMes(efrmMainSTR.iDevSpecPBInpMode1_Item).Split(",")
	Array.Copy(s, sDevSpecPBInpMode1_Item, s.Length)

	s = sMes(efrmMainSTR.iDevSpecPBInpMode0_Unit).Split(",")
	Array.Copy(s, sDevSpecPBInpMode0_Unit, s.Length)




	s = sMes(efrmMainSTR.iSamItem).Split(",")
	Array.Copy(s, sSamItem, s.Length)

	s = sMes(efrmMainSTR.iSamUnit).Split(",")
	Array.Copy(s, sSamUnit, s.Length)

	s = sMes(efrmMainSTR.iShape).Split(",")
	Array.Copy(s, sShape, s.Length)

	s = sMes(efrmMainSTR.iSamSpecShape0_Item).Split(",")
	Array.Copy(s, sSamSpecShape0_Item, s.Length)

	s = sMes(efrmMainSTR.iSamSpecShape1_Item).Split(",")
	Array.Copy(s, sSamSpecShape1_Item, s.Length)

	s = sMes(efrmMainSTR.iSamSpecShape2_Item).Split(",")
	Array.Copy(s, sSamSpecShape2_Item, s.Length)

	s = sMes(efrmMainSTR.iSamSpecShape0_Unit).Split(",")
	Array.Copy(s, sSamSpecShape0_Unit, s.Length)

	s = sMes(efrmMainSTR.iUnderMsg1).Split(",")
	Array.Copy(s, sUnderMsg1, s.Length)

	s = sMes(efrmMainSTR.iDevHint).Split(",")
	Array.Copy(s, sDevItemHint, s.Length)

	s = sMes(efrmMainSTR.iSamHint).Split(",")
	Array.Copy(s, sSamItemHint, s.Length)

	s = sMes(efrmMainSTR.iShapeHint).Split(",")
	Array.Copy(s, sSamSpecShape_hint, s.Length)

	s = sMes(efrmMainSTR.iSamSpecShape0_Hint).Split(",")
	Array.Copy(s, sSamSpecShape0_Hint, s.Length)

	s = sMes(efrmMainSTR.iSamSpecShape1_Hint).Split(",")
	Array.Copy(s, sSamSpecShape1_Hint, s.Length)

	s = sMes(efrmMainSTR.iSamSpecShape2_Hint).Split(",")
	Array.Copy(s, sSamSpecShape2_Hint, s.Length)


	'温度設定範囲を更新
	sDevInputRange(eDevSPEC.TEMPERATURE) = HEATER_LWLMT_NUD.ToString("F1") & "-" & p_cts.TempCnt_HUpperLimit.ToString("F1")

End Sub

''' <summary>
''' SPECファイルを現在の言語で変換
''' </summary>
Private Sub LanguageItemReplace(ByVal iT As Int16, ByRef dgv As DataGridView)
	Dim i As int32
	Select Case iT
	Case eSpec.Device
		For i=0 To sDevItem.GetUpperBound(0)
			dgv(0, i).Value = sDevItem(i)
		Next i
		For i=0 To sDevUnit.GetUpperBound(0)
			dgv(1, i).Value = sDevUnit(i)
		Next i
		'プローブ制御の入力方式による名称、単位、入力範囲を変更
		'Select Case dgv(2, eDevSPEC.PB_INP_MODE).Value
		'Case sProbInpMethod(ePBInpMode.Direct)
			dgv(0, eDevSPEC.AMPLITUDE).Value		= sDevSpecPBInpMode0_Item(0)
			dgv(1, eDevSPEC.AMPLITUDE).Value		= sDevSpecPBInpMode0_Unit(0)
			SetToolTipToDgvRow(iT, dgv, eDevSPEC.AMPLITUDE, sDevSpecPBInpMode0_Range(0)) 
			dgv(0, eDevSPEC.PERIOD).Value			= sDevSpecPBInpMode0_Item(1)
			dgv(1, eDevSPEC.PERIOD).Value			= sDevSpecPBInpMode0_Unit(1)
			SetToolTipToDgvRow(iT, dgv, eDevSPEC.PERIOD, sDevSpecPBInpMode0_Range(1)) 
		'Case sProbInpMethod(ePBInpMode.Indirect)
		'	dgv(0, eDevSPEC.AMPLITUDE).Value		= sDevSpecPBInpMode1_Item(0)
		'	dgv(1, eDevSPEC.AMPLITUDE).Value		= sDevSpecPBInpMode1_Unit(0)
		'	SetToolTipToDgvRow(dgv, eDevSPEC.AMPLITUDE, sDevSpecPBInpMode1_Range(0)) 
		'	dgv(0, eDevSPEC.PERIOD).Value			= sDevSpecPBInpMode1_Item(1)
		'	dgv(1, eDevSPEC.PERIOD).Value			= sDevSpecPBInpMode1_Unit(1)
		'	SetToolTipToDgvRow(dgv, eDevSPEC.PERIOD, sDevSpecPBInpMode1_Range(1)) 
		'End Select

	Case eSpec.Sample
		For i=0 To sSamItem.GetUpperBound(0)
			dgv(0, i).Value = sSamItem(i)
		Next i
		For i=0 To sSamUnit.GetUpperBound(0)
			dgv(1, i).Value = sSamUnit(i)
		Next i
		'サンプル形状による名称、単位、入力範囲を変更
		Select Case dgv(2, eSamSPEC.SHAPE).Value	
		Case sShape(eShape.Sheet)
			'hintをスイッチ
			sSamItemHint(eSamSPEC.SHAPE)			= sSamSpecShape_hint(0)
			sSamItemHint(eSamSPEC.THICKNESS)		= sSamSpecShape0_hint(0)
			sSamItemHint(eSamSPEC.STACKS)			= sSamSpecShape0_hint(1)

			dgv(0, eSamSPEC.THICKNESS).Value		= sSamSpecShape0_Item(0)
			dgv(1, eSamSPEC.THICKNESS).Value		= sSamSpecShape0_Unit(0)
			SetToolTipToDgvRow(iT, dgv, eSamSPEC.THICKNESS, sSamSpecShape0_Range(0)) 
			dgv(0, eSamSPEC.STACKS).Value			= sSamSpecShape0_Item(1)
			dgv(1, eSamSPEC.STACKS).Value			= sSamSpecShape0_Unit(1)
			SetToolTipToDgvRow(iT, dgv, eSamSPEC.STACKS, sSamSpecShape0_Range(1)) 
		Case sShape(eShape.Powder)
			'hintをスイッチ
			sSamItemHint(eSamSPEC.SHAPE)			= sSamSpecShape_hint(1)
			sSamItemHint(eSamSPEC.THICKNESS)		= sSamSpecShape1_hint(0)
			sSamItemHint(eSamSPEC.STACKS)			= sSamSpecShape1_hint(1)

			dgv(0, eSamSPEC.THICKNESS).Value		= sSamSpecShape1_Item(0)
			dgv(1, eSamSPEC.THICKNESS).Value		= sSamSpecShape1_Unit(0)
			SetToolTipToDgvRow(iT, dgv, eSamSPEC.THICKNESS, sSamSpecShape1_Range(0)) 
			dgv(0, eSamSPEC.STACKS).Value			= sSamSpecShape1_Item(1)
			dgv(1, eSamSPEC.STACKS).Value			= sSamSpecShape1_Unit(1)
			SetToolTipToDgvRow(iT, dgv, eSamSPEC.THICKNESS, sSamSpecShape1_Range(1)) 
		Case sShape(eShape.Liquid)
			'hintをスイッチ
			sSamItemHint(eSamSPEC.SHAPE)			= sSamSpecShape_hint(2)
			sSamItemHint(eSamSPEC.THICKNESS)		= sSamSpecShape2_hint(0)
			sSamItemHint(eSamSPEC.STACKS)			= sSamSpecShape2_hint(1)

			dgv(0, eSamSPEC.THICKNESS).Value		= sSamSpecShape2_Item(0)
			dgv(1, eSamSPEC.THICKNESS).Value		= sSamSpecShape2_Unit(0)
			SetToolTipToDgvRow(iT, dgv, eSamSPEC.THICKNESS, sSamSpecShape2_Range(0)) 
			dgv(0, eSamSPEC.STACKS).Value			= sSamSpecShape2_Item(1)
			dgv(1, eSamSPEC.STACKS).Value			= sSamSpecShape2_Unit(1)
			SetToolTipToDgvRow(iT, dgv, eSamSPEC.THICKNESS, sSamSpecShape2_Range(1)) 
		Case sShape(eShape.Rcc)
			'hintをスイッチ
			sSamItemHint(eSamSPEC.SHAPE)			= sSamSpecShape_hint(3)
			sSamItemHint(eSamSPEC.THICKNESS)		= sSamSpecShape0_hint(0)
			sSamItemHint(eSamSPEC.STACKS)			= sSamSpecShape0_hint(1)

			dgv(0, eSamSPEC.THICKNESS).Value		= sSamSpecShape0_Item(0)
			dgv(1, eSamSPEC.THICKNESS).Value		= sSamSpecShape0_Unit(0)
			SetToolTipToDgvRow(iT, dgv, eSamSPEC.THICKNESS, sSamSpecShape0_Range(0)) 
			dgv(0, eSamSPEC.STACKS).Value			= sSamSpecShape0_Item(1)
			dgv(1, eSamSPEC.STACKS).Value			= sSamSpecShape0_Unit(1)
			SetToolTipToDgvRow(iT, dgv, eSamSPEC.STACKS, sSamSpecShape0_Range(1)) 
	End Select
	End Select

End Sub
''' <summary>
''' SPECデータテーブル定義
''' </summary>
''' <param name="dt">データテーブル</param>
''' <returns></returns>
Public Function SpecDefine(ByRef dt As DataTable) As int32
Try
	dt = Nothing
	dt = New DataTable()
	'データテーブルにヘッダーを作る
	For i=0 To sColumn.GetUpperBound(0)
		dt.Columns.Add(sColumn(i),GetType(String))
	Next i
	Return 0
Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Return -1
End Try
End Function
''' <summary>
''' データテーブルへデータ読込
''' </summary>
''' <param name="dgv">SPECのデータグリッドビュー</param>
''' <param name="AryL">データテーブルイメージのアレイリスト</param>
''' <returns></returns>
Public Function SpecItemDisp(ByVal iT As Int32, ByVal sSpecName As String, ByRef dgv As DataGridView, ByVal AryL As ArrayList) As Int32
Dim i	As int32
Dim dt	As DataTable = Nothing
Dim Lmt	As Int32 = 0
Dim s()	As String = Nothing
Try
	'読み込んだSPECファイルが正常か確認
	If CheckSpecFile(iT, AryL) <> 0
		Return -9
	End If
	'データテーブル定義
	Select Case iT
	Case eSpec.Device
		dt = dtDevSpec
		Lmt	= sDevItem.GetUpperBound(0)
	Case eSpec.Sample
		dt = dtSamSpec
		Lmt	= sSamItem.GetUpperBound(0)
	End Select
	SpecDefine( dt )

	For i=0 To Lmt
		dr		= dt.NewRow()
		If Not IsNothing(AryL)
			Dim sAry() As String = AryL(i)
			dr(0)	= sAry(0)				'Item
			dr(1)	= sAry(1)				'UNIT
			Select Case iT
			Case eSpec.Device	'デバイスSPEC
				Select Case i
				Case eDevSPEC.HP_CONT		'熱板位置制御
					dr(2)	= sHCont(SAry(2))

				Case eDevSPEC.METHOD			'ゲルタイム判定法
					dr(2)	= lstCalcMethod(sAry(2))

				Case eDevSPEC.STRS_SENS_MODE	'応力検知モード
					dr(2)	= sStrsSens(sAry(2))

				'Case eDevSPEC.PB_INP_MODE	'プローブ動作設定の入力方式
				'	dr(2)	= sProbInpMethod(sAry(2))

				Case eDevSPEC.AMP_MODE			'振幅測定モード
					dr(2)	= sAmpMode(sAry(2))

				Case eDevSPEC.THRETHOLD
					dr(2)	= sAry(2)			'Value

				Case eDevSPEC.TEMPERATURE
					dr(2)	= sAry(2)			'Value

				Case eDevSPEC.AMPMODE_STRESS
					dr(2)	= sAry(2)			'Value

				Case eDevSPEC.MEAS_GAP
					dr(2)	= sAry(2)			'Value

				Case eDevSPEC.MEAS_STRESS
					dr(2)	= sAry(2)			'Value

				Case eDevSPEC.PRESSCONT_STEP
					dr(2)	= sAry(2)			'Value

				Case eDevSPEC.PB_DIAMETER
					dr(2)	= sAry(2)			'Value

				Case eDevSPEC.AMPLITUDE
					dr(2)	= sAry(2)			'Value

				Case eDevSPEC.PERIOD
					dr(2)	= sAry(2)			'Value

				Case eDevSPEC.HP_UP_WAIT
					dr(2)	= sAry(2)			'Value

				Case eDevSPEC.ACTION_WAIT
					dr(2)	= sAry(2)			'Value

				Case eDevSPEC.MEAS_TIME
					dr(2)	= sAry(2)			'Value

				Case eDevSPEC.MEAS_END_STRESS
					dr(2)	= sAry(2)			'Value

				Case eDevSPEC.TEMP_PROFILE
					dr(2)	= sAry(2)			'Value

				Case eDevSPEC.DIFF_INTERVAL
					dr(2)	= sAry(2)			'Value

				End Select
			Case eSpec.Sample	'サンプルSPEC
				Select Case i
				Case eSamSPEC.SHAPE		'形状
					dr(2)	= sShape(sAry(2))

				Case eSamSPEC.SAMPLE
					dr(2)	= sAry(2)			'Value

				Case eSamSPEC.LOT
					dr(2)	= sAry(2)			'Value

				Case eSamSPEC.MEASURER
					dr(2)	= sAry(2)			'Value

				Case eSamSPEC.THICKNESS
					dr(2)	= sAry(2)			'Value

				Case eSamSPEC.STACKS
					dr(2)	= sAry(2)			'Value

				Case eSamSPEC.EFECT_RANGE
					dr(2)	= sAry(2)			'Value

				Case eSamSPEC.JUDGE_RANGE
					dr(2)	= sAry(2)			'Value

				End Select
			End Select
		Else
			dr(0)	= sDevItem(i)				'Item
			dr(1)	= sDevUnit(i)				'UNIT
			dr(2)	= ""						'Value
		End If
		dt.Rows.Add(dr)
	Next i
	dgv.DataSource = dt				'データソースセット
	Select Case iT
	Case eSpec.Device
		dtDevSpec = dt
		spi.sDeviceSpecName = sSpecName
	Case eSpec.Sample
		dtSamSpec = dt
		spi.sSampleSpecName = sSpecName
	End Select
	LanguageItemReplace(iT, dgv)	'言語変換
	Return 0
Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Return -1
End Try
End Function

''' <summary>
''' 登録されているデバイスSPECのリスト取得しコンボボックスへセット
''' </summary>
''' <param name="cbx"></param>
''' <returns>0:正常終了 -1:異常終了 -2:ファイルなし</returns>
Public Function	SpecFileGet(ByRef cbx As ComboBox) As int32
	Dim sFileName()		As String = Nothing
	Dim iRet			As Int32
Try
	'SPECファイル名一覧を得る
	Select Case cbx.Name
	Case "cbxDevSPEC"
		iRet = GetSpecFileList(0, sFileName)
	Case "cbxSamSPEC"
		iRet = GetSpecFileList(1, sFileName)
	End Select
	If iRet = 0
		cbx.Items.Clear()
		cbx.Items.AddRange(sFileName)
	End If
	Return iRet
Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Return -1
End Try
End Function
''' <summary>
''' SPECファイル名一覧を得る
''' </summary>
''' <param name="iSel"></param>
''' <param name="sFileName"></param>
''' <returns></returns>
Public Function GetSpecFileList(ByVal iSel As Int16, ByRef sFileName() As string) As int32
	Dim sPath			As String = Nothing
	Dim sFullFileName()	As String = Nothing
	Dim i				As int32
Try
	Select Case iSel
	Case 0
		sPath = Path.Combine(SIZUKA_PATH, p_sEquipName, DEVSPEC_PATH)
	Case 1
		sPath = Path.Combine(SIZUKA_PATH, p_sEquipName, SAMSPEC_PATH)
	End Select
	'ファイル一覧取得 TOPフォルダのみ
	sFullFileName = Directory.GetFiles(sPath, "*.csv", System.IO.SearchOption.TopDirectoryOnly)
	If sFullFileName.Length > 0
		'ファイル名を抜粋
		Array.Resize(sFileName, sFullFileName.Length)
		For i=0 To sFullFileName.GetUpperBound(0)
			sFileName(i) = Path.GetFileNameWithoutExtension(sFullFileName(i))
		Next
	Else
	'ファイルがない
		Return -2
	End If

	Return 0
Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Return -1
End Try
End Function
''' <summary>
''' デバイスSPECのビューを設定
''' </summary>
''' <param name="dgv">デバイスSPECのデータグリッドビュー</param>
''' <param name="bLock">セル直接入力の禁止/許可 選択項目は選択メニューを表示するときにチェック</param>
Public Sub dgvDevSetting(ByRef dgv As DataGridView, Optional bLock As Boolean = False)
Dim i		As int32
Try
With dgv
	.ReadOnly					= bLock						'読取専用プロパティ
	.ColumnHeadersVisible		= False						'列ヘッダ表示設定
	.RowHeadersVisible			= False						'行ヘッダ表示設定
	.AllowUserToAddRows			= False						'追加用行の許可
	.AllowUserToResizeColumns	= False						'カラム幅変更許可
	.AllowUserToResizeRows		= False						'行高さ変更許可
	.MultiSelect				= False

	.CurrentCell				= Nothing
	.Font						= New Font(BASE_FONT, 14)	'フォント設定
	.RowTemplate.Height			= 30
	'四角いカーソルを消す
	.Columns(0).DefaultCellStyle.SelectionBackColor		= Color.White
	.Columns(0).DefaultCellStyle.SelectionForeColor		= Color.Black
	.Columns(1).DefaultCellStyle.SelectionBackColor		= Color.White
	.Columns(1).DefaultCellStyle.SelectionForeColor		= Color.Black
	'セル列幅設定
	.Columns(0).Width			= .Width * 10/20
	.Columns(1).Width			= .Width * 3/20
	.Columns(2).Width			= .Width - (.Columns(0).Width+.Columns(1).Width)
	''行の高さ
	'.AutoResizeRows(DataGridViewAutoSizeRowsMode.AllCells)
	'高さ設定
	.Height						= .RowTemplate.Height * sDevItem.Length + 3
	.Columns(1).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

	Select Case p_cts.LTModeEnable
	Case False
		'編集可不可カラムの設定
		For i=0 To sDevItem.GetUpperBound(0)
			.Item(0, i).ReadOnly = True
			.Item(1, i).ReadOnly = True
			.Item(2, i).ReadOnly = False
			Select Case i
			'選択アイテム
			Case _
				eDevSPEC.HP_CONT, _			'熱板位置制御
				eDevSPEC.METHOD, _			'ゲルタイム判定法
				eDevSPEC.STRS_SENS_MODE, _	'応力検知モード
				eDevSPEC.AMP_MODE, _		'振幅測定モード
				eDevSPEC.TEMP_PROFILE		'温度プロファイル
				.Item(2, i).ReadOnly = True
				.Rows(i).DefaultCellStyle.BackColor = Color.White
			Case eDevSPEC.DIFF_INTERVAL
				.Rows(i).DefaultCellStyle.BackColor = Color.White
			Case Else
			'入力範囲を設定
			'If sDevInputRange(i)<>""
			'	.Item(0, i).ToolTipText = sDevInputRange(i)
			'End If
			End Select
		Next i
	Case True
		'編集可不可カラムの設定
		For i=0 To sDevItem.GetUpperBound(0)
			.Item(0, i).ReadOnly = True
			.Item(1, i).ReadOnly = True
			.Item(2, i).ReadOnly = False
			Select Case i
			'選択アイテム
			Case _
				eDevSPEC.HP_CONT, _			'熱板位置制御
				eDevSPEC.STRS_SENS_MODE, _	'応力検知モード
				eDevSPEC.AMP_MODE, _		'振幅測定モード
				eDevSPEC.TEMP_PROFILE		'温度プロファイル
				.Item(2, i).ReadOnly = True
			Case _
				eDevSPEC.METHOD, _			'ゲルタイム判定法
				eDevSPEC.DIFF_INTERVAL		'SLOPE微分間隔
				.Item(2, i).ReadOnly = True
				.item(2, eDevSPEC.METHOD).Value = lstCalcMethod(eCalcMethod.THR)
				.Rows(i).DefaultCellStyle.BackColor = Color.LightGray
			Case Else
			'入力範囲を設定
			'If sDevInputRange(i)<>""
			'	.Item(0, i).ToolTipText = sDevInputRange(i)
			'End If
			End Select
		Next i
	End Select
End With

Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub
''' <summary>
''' サンプルSPECのビューを設定
''' </summary>
''' <param name="dgv">サンプルSPECのデータグリッドビュー</param>
Public Sub dgvSamSetting(ByRef dgv As DataGridView, Optional bLock As Boolean = False)
Dim i		As int32
Try
With dgv
	.ReadOnly					= False						'読取専用プロパティ
	.ColumnHeadersVisible		= False						'列ヘッダ表示設定
	.RowHeadersVisible			= False						'行ヘッダ表示設定
	.AllowUserToAddRows			= False						'追加用行の許可
	.AllowUserToResizeColumns	= False						'カラム幅変更許可
	.AllowUserToResizeRows		= False						'行高さ変更許可
	.MultiSelect				= False

	.CurrentCell				= Nothing
	.Font						= New Font(BASE_FONT, 14)	'フォント設定
	.RowTemplate.Height			= 30
	'四角いカーソルを消す
	.Columns(0).DefaultCellStyle.SelectionBackColor		= Color.White
	.Columns(0).DefaultCellStyle.SelectionForeColor		= Color.Black
	.Columns(1).DefaultCellStyle.SelectionBackColor		= Color.White
	.Columns(1).DefaultCellStyle.SelectionForeColor		= Color.Black
	'セル列幅設定
	.Columns(0).Width			= .Width * 10/20
	.Columns(1).Width			= .Width * 3/20
	.Columns(2).Width			= .Width - (.Columns(0).Width+.Columns(1).Width)
	''行の高さ
	'.AutoResizeRows(DataGridViewAutoSizeRowsMode.AllCells)
	'高さ設定
	.Height						= .RowTemplate.Height * sSamItem.Length + 3
	.Columns(1).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
	'編集可不可カラムの設定
	For i=0 To sSamItem.GetUpperBound(0)
		.Item(0, i).ReadOnly = True
		.Item(1, i).ReadOnly = True
		.Item(2, i).ReadOnly = False
		Select Case i
		Case  eSamSPEC.SAMPLE, eSamSPEC.LOT, eSamSPEC.MEASURER
		'選択アイテム+入力
		'サンプル名、ロット、測定者は編集ロックしない
			.Item(2, i).ReadOnly = False
		Case eSamSPEC.SHAPE				'形状
		'選択アイテム
			.Item(2, i).ReadOnly = True
		Case _
			eSamSPEC.EFECT_RANGE, _		'有効範囲
			eSamSPEC.JUDGE_RANGE		'判定範囲
			.Item(2, i).ReadOnly = True
			.Item(2, i).Style.Font = New Font(BASE_FONT, 13)	'フォント設定
		Case Else
			.Item(2, i).ReadOnly = bLock
		End Select
	Next i
End With
Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub
''' <summary>
''' データテーブルからCSV出力用アレイリスト変換
''' </summary>
''' <param name="AryL">アレイリスト</param>
''' <returns></returns>
Public Function	CnvDataTableToAryList(ByVal iT As Int32, ByRef AryL As ArrayList) As int32
	Dim ss() As Object = Nothing
	Dim i As int32
	Dim idx As Int32
Try
	Select Case iT
	Case eSpec.Device
		If Not IsNothing(dtDevSpec)
			For i = 0 To dtDevSpec.Rows.Count-1
				ss = dtDevSpec.Rows(i).ItemArray
				'選択アイテムはインデックスへ変換
				Select Case i
				Case eDevSPEC.HP_CONT		'熱板位置制御
					idx = Array.IndexOf(sHCont, ss(2))
					ss(2) = idx

				Case eDevSPEC.METHOD			'ゲルタイム判定法
					idx = Array.IndexOf(lstCalcMethod.ToArray, ss(2))
					ss(2) = idx
	
				Case eDevSPEC.STRS_SENS_MODE	'応力検知モード
					idx = Array.IndexOf(sStrsSens, ss(2))
					ss(2) = idx

				'Case eDevSPEC.PB_INP_MODE	'プローブ動作設定の入力方式
				'	idx = Array.IndexOf(sProbInpMethod, ss(2))
				'	ss(2) = idx

				Case eDevSPEC.AMP_MODE		'振幅測定モード
					idx = Array.IndexOf(sAmpMode, ss(2))
					ss(2) = idx

				Case Else
				End Select
				AryL.Add(ss)
			Next
		End If
	Case eSpec.Sample
		If Not IsNothing(dtSamSpec)
			For i = 0 To dtSamSpec.Rows.Count-1
				ss = dtSamSpec.Rows(i).ItemArray
				'選択アイテムはインデックスへ変換
				Select Case i
				Case eSamSPEC.SHAPE		'形状
					idx = Array.IndexOf(sShape, ss(2))
					ss(2) = idx
				Case Else
				End Select
				AryL.Add(ss)
			Next
		End If
	End Select
	Return 0
Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Return -1
End Try
End Function
'サンプルSPECメニュー選択リスト蓄積配列
'Dim sSampleAcum()	As String = {"Sample name"}
'Dim sLotAcum()		As String = {"Lot"}
'Dim sMeasurerAcum() As String = {"Measurer"}
#If 1
''' <summary>
''' サンプルSPECでキー入力された項目をメニューへ追加
''' </summary>
''' <param name="iRow"></param>
''' <param name="sVal"></param>
Public Sub MenuHistAdd(byVal iRow As int32, ByVal sVal As String)
	Select Case iRow
	Case eSamSPEC.SAMPLE
		If Array.IndexOf(p_cts.p_sSampleAcum, sVal) < 0
		'既にある場合は追加しない
			If sVal.Length>0
				Array.Resize(p_cts.p_sSampleAcum, p_cts.p_sSampleAcum.Length+1)
				p_cts.p_sSampleAcum(p_cts.p_sSampleAcum.GetUpperBound(0)) = sVal
			End If
		End If
	Case eSamSPEC.LOT
		If Array.IndexOf(p_cts.p_sLotAcum, sVal) < 0
		'既にある場合は追加しない
			If sVal.Length>0
				Array.Resize(p_cts.p_sLotAcum, p_cts.p_sLotAcum.Length+1)
				p_cts.p_sLotAcum(p_cts.p_sLotAcum.GetUpperBound(0)) = sVal
			End If
		End If
	Case eSamSPEC.MEASURER
		If Array.IndexOf(p_cts.p_sMeasurerAcum, sVal) < 0
		'既にある場合は追加しない
			If sVal.Length>0
				Array.Resize(p_cts.p_sMeasurerAcum, p_cts.p_sMeasurerAcum.Length+1)
				p_cts.p_sMeasurerAcum(p_cts.p_sMeasurerAcum.GetUpperBound(0)) = sVal
			End If
		End If
	End Select
End Sub
#end If
''' <summary>
''' 選択メニューを表示
''' </summary>
''' <param name="cmsM"></param>
''' <param name="iSel"></param>
''' <param name="dgv"></param>
Public Sub SelectMenuDisp(ByVal iT As Int32, ByRef cmsM As ContextMenuStrip,  ByVal iSel As Int32, ByRef dgv As Datagridview)
	cmsM.Items.Clear()
	Select Case iT
	Case eSpec.Device
		Select Case iSel
		Case eDevSPEC.HP_CONT		'熱板位置制御
			For Each s As String in sHCont
				cmsM.Items.Add(s, Nothing, New EventHandler(AddressOf HCONT))
			Next
			dgvTrgt = dgv
		Case eDevSPEC.METHOD			'ゲルタイム判定法
			For Each s As String in lstCalcMethod
				cmsM.Items.Add(s, Nothing, New EventHandler(AddressOf METHOD))
			Next
			dgvTrgt = dgv
		Case eDevSPEC.STRS_SENS_MODE	'応力検知モード
			For Each s As String in sStrsSens
				cmsM.Items.Add(s, Nothing, New EventHandler(AddressOf STRSSENS))
			Next
			dgvTrgt = dgv
		'Case eDevSPEC.PB_INP_MODE	'プローブ動作設定の入力方式
		'	For Each s As String in sProbInpMethod
		'		cmsM.Items.Add(s, Nothing, New EventHandler(AddressOf PROBINP))
		'	Next
		'	dgvTrgt = dgv

		Case eDevSPEC.AMP_MODE		'振幅測定モード
			For Each s As String in sAmpMode
				cmsM.Items.Add(s, Nothing, New EventHandler(AddressOf AMPMODE))
			Next
			dgvTrgt = dgv
		
		End Select
	Case eSpec.Sample
		Select Case iSel
		Case eSamSPEC.SAMPLE		'Sample
			For Each s As String in p_cts.p_sSampleAcum
				cmsM.Items.Add(s, Nothing, New EventHandler(AddressOf SAMP))
			Next
			dgvTrgt = dgv
		Case eSamSPEC.LOT		'LOT
			For Each s As String in p_cts.p_sLotAcum
				cmsM.Items.Add(s, Nothing, New EventHandler(AddressOf LOT))
			Next
			dgvTrgt = dgv
		Case eSamSPEC.MEASURER	'測定者
			For Each s As String in p_cts.p_sMeasurerAcum
				cmsM.Items.Add(s, Nothing, New EventHandler(AddressOf MES))
			Next
			dgvTrgt = dgv
		Case eSamSPEC.SHAPE		'形状
			For Each s As String in sShape
				cmsM.Items.Add(s, Nothing, New EventHandler(AddressOf SHAPE))
			Next
			dgvTrgt = dgv
		End Select
	End Select

End Sub
'コンテキストメニュークリックイベントハンドラ関数(dgvDevSPEC)
''' <summary>
''' 熱板位置制御 選択イベント
''' </summary>
Private Sub HCONT(ByVal sender As Object, ByVal e As System.EventArgs)
	dgvTrgt(2, eDevSPEC.HP_CONT).Value = sender.ToString

	'変更があった場合は青
	Dim cColor As color = If(frmMain.sPreEditVal<>dgvTrgt(2,eDevSPEC.HP_CONT).Value, Color.Blue, Color.Black)
	dgvTrgt(2,eDevSPEC.HP_CONT).Style.ForeColor = cColor

	Select Case dgvTrgt(2, eDevSPEC.HP_CONT).Value
	Case sHcont(eHCont.Gap)
		spiDevItemSetAndFreeMode(spi.iHPPosCont, eHCont.Gap)

	Case sHcont(eHCont.OrderP)
		spiDevItemSetAndFreeMode(spi.iHPPosCont, eHCont.OrderP)

	Case sHcont(eHCont.ConstantP)
		spiDevItemSetAndFreeMode(spi.iHPPosCont, eHCont.ConstantP)

	End Select
End Sub
''' <summary>
''' ゲルタイム判定手法 選択イベント
''' </summary>
Private Sub METHOD(ByVal sender As Object, ByVal e As System.EventArgs)
	If p_cts.LTModeEnable
	'間欠測定モード時はTHRのみ
		dgvTrgt(2, eDevSPEC.METHOD).Value = lstCalcMethod(eCalcMethod.THR)
		Exit Sub
	End If
	dgvTrgt(2, eDevSPEC.METHOD).Value = sender.ToString

	'変更があった場合は青
	Dim cColor As color = If(frmMain.sPreEditVal<>dgvTrgt(2,eDevSPEC.METHOD).Value, Color.Blue, Color.Black)
	dgvTrgt(2,eDevSPEC.METHOD).Style.ForeColor = cColor

	Select Case dgvTrgt(2, eDevSPEC.METHOD).Value
	Case lstCalcMethod(eCalcMethod.THR)
		spiDevItemSetAndFreeMode(spi.iGelTimeMethod, eCalcMethod.THR)

	Case lstCalcMethod(eCalcMethod.SLOPE)
		spiDevItemSetAndFreeMode(spi.iGelTimeMethod, eCalcMethod.SLOPE)

	Case lstCalcMethod(eCalcMethod.PHASE)
		spiDevItemSetAndFreeMode(spi.iGelTimeMethod, eCalcMethod.PHASE)

	Case lstCalcMethod(eCalcMethod.EXTRA)
		spiDevItemSetAndFreeMode(spi.iGelTimeMethod, eCalcMethod.EXTRA)
		ExtraMethodInit(frmMain)
	Case lstCalcMethod(eCalcMethod.MELT)
		spiDevItemSetAndFreeMode(spi.iGelTimeMethod, eCalcMethod.MELT)

	End Select
	If iEventFrom = 0
	'frmMain上でのイベントじのみ
		p_RTM(0).p_sCalcMethod = lstCalcMethod(p_clSpec.spi.iGelTimeMethod)
		frmMain.MethodChanged(spi.iGelTimeMethod)
		Dim iRet As int32
		'チャート更新 ゲルタイム判定更新
		'p_dtFile.p_head.iState		= 0							'測定データ状態のクリア
		iRet = p_RTM(0).GelTimeSeach(frmMain.p_iSelectGItem)	'ゲルタイム再検索
		frmMain.ResearchGelTimeOnChart()						'ゲルタイム表示更新(チャート内)
		GelTimeJudgeColorUpdate(frmMain)						'ゲルタイム判定表示
		GelTimeLabelDispUpdate(frmMain)							'ゲルタイム表示更新
		'ChartDispUpdate(frmMain, 0, True, ,)					'チャート表示更新
	End if
End Sub
Private Sub ExtraMethodInit(ByRef frm As frmMain)
	'外挿法
	'接線調整する準備
	frm.clExtra.ExtraPrePareProc(0, frm.Chart1, p_dtFile, frm.nudStartTim.Value, frm.nudMidTim.Value, frm.nudEndTim.Value)
	frm.clExtra.GelTimeCrossLineDisp(frm.Chart1)        '接線表示とゲルタイム計算
	Select Case frm.cboGelTSel.SelectedIndex
	Case 0
		p_RTM(0).p_dGelTime = frm.clExtra.LowerGelT
	Case 1
		p_RTM(0).p_dGelTime = frm.clExtra.MiddlGelT
	Case 2
		p_RTM(0).p_dGelTime = frm.clExtra.UpperGelT
	End Select
End Sub

''' <summary>
''' 応力検知モード 選択イベント
''' </summary>
Private Sub STRSSENS(ByVal sender As Object, ByVal e As System.EventArgs)
	dgvTrgt(2, eDevSPEC.STRS_SENS_MODE).Value = sender.ToString

	'変更があった場合は青
	Dim cColor As color = If(frmMain.sPreEditVal<>dgvTrgt(2,eDevSPEC.STRS_SENS_MODE).Value, Color.Blue, Color.black)
	dgvTrgt(2,eDevSPEC.STRS_SENS_MODE).Style.ForeColor = cColor

	'入力アイテム切替
	Select Case sender.ToString
	Case sStrsSens(eStrsSens.Line)	'線
		spiDevItemSetAndFreeMode(spi.iStressSensMode, eStrsSens.Line)

	Case sStrsSens(eStrsSens.Circle)	'円
		spiDevItemSetAndFreeMode(spi.iStressSensMode, eStrsSens.Circle)

	Case sStrsSens(eStrsSens.Point)	'点
		spiDevItemSetAndFreeMode(spi.iStressSensMode, eStrsSens.Point)

	'点が選択されたら直接モードのみ
		'If dgvTrgt(2, eDevSPEC.PB_INP_MODE).Value	<>  sProbInpMethod(0)
		'	dgvTrgt(2, eDevSPEC.PB_INP_MODE).Value = sProbInpMethod(0)
		'	ProbInpSub(sProbInpMethod(0))			'直接が選択されたイベント処理
		'End If
	End Select
End Sub
''' <summary>
''' 振幅測定モード 選択イベント
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub AMPMODE(ByVal sender As Object, ByVal e As System.EventArgs)
	dgvTrgt(2, eDevSPEC.AMP_MODE).Value = sender.ToString

	'変更があった場合は青
	Dim cColor As color = If(frmMain.sPreEditVal<>dgvTrgt(2,eDevSPEC.AMP_MODE).Value, Color.Blue, Color.black)
	dgvTrgt(2,eDevSPEC.AMP_MODE).Style.ForeColor = cColor

	'入力アイテム切替
	Select Case sender.ToString
	Case sAmpMode(eAmpMode.AmpConst)	'振幅一定
		spiDevItemSetAndFreeMode(spi.iAmpMode, eAmpMode.AmpConst)

	Case sAmpMode(eAmpMode.StsConst)	'応力一定
		spiDevItemSetAndFreeMode(spi.iAmpMode, eAmpMode.StsConst)

	End Select
End Sub
#if 0
''' <summary>
''' プローブ動作設定の入力方式 選択イベント
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub PROBINP(ByVal sender As Object, ByVal e As System.EventArgs)
	If dgvTrgt(2, eDevSPEC.STRS_SENS_MODE).Value <> sStrsSens(2)  '点以外
	'点選択されている場合は直接方式のみ
		dgvTrgt(2, eDevSPEC.PB_INP_MODE).Value = sender.ToString
		Select Case dgvTrgt(2, eDevSPEC.PB_INP_MODE).Value
		Case sProbInpMethod(0)		'間接->直接
			'spiDevItemSetAndFreeMode(spi.iProbInputMode, ePBInpMode.Direct)

		Case sProbInpMethod(1)		'直接->間接
			'spiDevItemSetAndFreeMode(spi.iProbInputMode, ePBInpMode.InDirect)

		End Select

		'入力アイテムの切替
		ProbInpSub(sender.ToString)			'プローブ入力方式変更イベントのサブ関数
	End If
End Sub
#end if
Dim m_dAmp			As Double		'間接へ切り替えた時の退避
Dim m_dPeriod		As Double		'間接へ切り替えた時の退避
''' <summary>
''' プローブ動作設定の入力方式 選択時の表示項目変更と設定値再計算
'''   入力方式は直接のみ、間接は参考表示
''' </summary>
''' <param name="s"></param>
Private Sub ProbInpSub(ByVal s As String)
	'Dim dStrain		As Double
	'Dim dShearSp	As Double
	'Dim dGap		As Double
	'Dim dAmp		As Double
	'Dim dPeriod		As Double
	Dim iKeta		As Int16
	Dim dMinMax(1)	As Double
	'Select Case s
	'Case sProbInpMethod(0)
	'直接入力
		SetToolTipToDgvRow(eSpec.Device, dgvTrgt, eDevSPEC.AMPLITUDE, sDevSpecPBInpMode0_Range(0)) 
		SetToolTipToDgvRow(eSpec.Device, dgvTrgt, eDevSPEC.PERIOD, sDevSpecPBInpMode0_Range(1)) 
#If 0
		If dgvTrgt(0, eDevSPEC.AMPLITUDE).Value	<>  sDevSpecPBInpMode0_Item(0)
		'間接->直接変更（振幅、周期へ切替）
			dStrain		= Double.Parse(dgvTrgt(2, eDevSPEC.PERIOD).Value)				'ひずみ
			dShearSp	= Double.Parse(dgvTrgt(2, eDevSPEC.AMPLITUDE).Value)			'剪断速度
			dGap		= Double.Parse(dgvTrgt(2, eDevSPEC.MEAS_GAP).Value)				'GAP
			dAmp		= CalcAmpFromStrainGap(dStrain, dGap)							'振幅
			dPeriod		= CalcPrdFromStrainShearSpd(dStrain, dShearSp)					'周期
			iKeta = FloatFormatAndMinMax(dgvTrgt(0, eDevSPEC.AMPLITUDE).ToolTipText, dMinMax)
			dgvTrgt(2, eDevSPEC.AMPLITUDE).Value	=  dAmp.ToString("F" & iKeta)
			iKeta = FloatFormatAndMinMax(dgvTrgt(0, eDevSPEC.PERIOD).ToolTipText, dMinMax)
			dgvTrgt(2, eDevSPEC.PERIOD).Value		=  dPeriod.ToString("F" &iKeta)
		End If
#else
			iKeta = FloatFormatAndMinMax(dgvTrgt(2, eDevSPEC.AMPLITUDE).ToolTipText, dMinMax)
			dgvTrgt(2, eDevSPEC.AMPLITUDE).Value	=  m_dAmp.ToString("F" & iKeta)
			iKeta = FloatFormatAndMinMax(dgvTrgt(2, eDevSPEC.PERIOD).ToolTipText, dMinMax)
			dgvTrgt(2, eDevSPEC.PERIOD).Value		=  m_dPeriod.ToString("F" &iKeta)
			dgvTrgt(2, eDevSPEC.AMPLITUDE).ReadOnly = False
			dgvTrgt(2, eDevSPEC.PERIOD).ReadOnly	= False
#end if
		dgvTrgt(0, eDevSPEC.AMPLITUDE).Value		=  sDevSpecPBInpMode0_Item(0)		'項目名称変更
		dgvTrgt(1, eDevSPEC.AMPLITUDE).Value		=  sDevSpecPBInpMode0_Unit(0)		'単位変更
		dgvTrgt(0, eDevSPEC.PERIOD).Value			=  sDevSpecPBInpMode0_Item(1)		'項目名称変更
		dgvTrgt(1, eDevSPEC.PERIOD).Value			=  sDevSpecPBInpMode0_Unit(1)		'単位変更

'	Case sProbInpMethod(1)
'	'間接入力
'		SetToolTipToDgvRow(dgvTrgt, eDevSPEC.AMPLITUDE, sDevSpecPBInpMode1_Range(0)) 
'		SetToolTipToDgvRow(dgvTrgt, eDevSPEC.PERIOD, sDevSpecPBInpMode1_Range(1)) 
'#If 1
'		If dgvTrgt(0, eDevSPEC.AMPLITUDE).Value	<>  sDevSpecPBInpMode1_Item(0)
'		'直接->間接変更 (サンプルひずみ、剪断速度へ切替)
'			dPeriod		= Double.Parse(dgvTrgt(2, eDevSPEC.PERIOD).Value)				'周期
'			dAmp		= Double.Parse(dgvTrgt(2, eDevSPEC.AMPLITUDE).Value)			'振幅
'			dGap		= Double.Parse(dgvTrgt(2, eDevSPEC.MEAS_GAP).Value)				'GAP
'			dStrain		= CalcStrainFromAmpGap(dAmp, dGap)											'ひずみ
'			dShearSp	= CalcShearSpdFromStrainPrd(dStrain, dPeriod)								'剪断速度
'			iKeta = FloatFormatAndMinMax(dgvTrgt(0, eDevSPEC.AMPLITUDE).ToolTipText, dMinMax)
'			dgvTrgt(2, eDevSPEC.AMPLITUDE).Value	=  dShearSp.ToString("F" & iKeta)
'			iKeta = FloatFormatAndMinMax(dgvTrgt(0, eDevSPEC.PERIOD).ToolTipText, dMinMax)
'			dgvTrgt(2, eDevSPEC.PERIOD).Value		=  dStrain.ToString("F" & iKeta)


'			dgvTrgt(2, eDevSPEC.AMPLITUDE).ReadOnly = True
'			dgvTrgt(2, eDevSPEC.PERIOD).ReadOnly	= True
'			m_dPeriod	= dPeriod
'			m_dAmp		= dAmp
'		End If
'#end if
'		dgvTrgt(0, eDevSPEC.AMPLITUDE).Value		=  sDevSpecPBInpMode1_Item(0)		'項目名称変更
'		dgvTrgt(1, eDevSPEC.AMPLITUDE).Value		=  sDevSpecPBInpMode1_Unit(0)		'単位変更
'		dgvTrgt(0, eDevSPEC.PERIOD).Value			=  sDevSpecPBInpMode1_Item(1)		'項目名称変更
'		dgvTrgt(1, eDevSPEC.PERIOD).Value			=  sDevSpecPBInpMode1_Unit(1)		'単位変更

	'End Select
End Sub
''' <summary>
''' 振幅計算 サンプル歪とGAPから
''' </summary>
''' <param name="dStrain"></param>
''' <param name="dGap"></param>
''' <returns></returns>
Private Function CalcAmpFromStrainGap(ByVal dStrain As Double, ByVal dGap As Double) As Double
	Return Math.Round(dStrain * dGap * 2, 3)											'振幅
End Function
''' <summary>
''' 周期計算 サンプル歪とせん断速度から
''' </summary>
''' <param name="dStrain"></param>
''' <param name="dShearSpd"></param>
''' <returns></returns>
Private Function CalcPrdFromStrainShearSpd(ByVal dStrain As Double, ByVal dShearSpd As Double) As Double
	Dim d As Double = Math.Round((dStrain * 2 ) / dShearSpd, 3)									'周期
	If Double.IsNaN(d)
		d = 1
	End If
	If Double.IsInfinity(d)
		d = 1
	End If
	Return d									'周期
End Function
''' <summary>
''' サンプル歪計算 振幅とGAPから
''' </summary>
''' <param name="dAmp"></param>
''' <param name="dGap"></param>
''' <returns></returns>
Private Function CalcStrainFromAmpGap(ByVal dAmp As Double, ByVal dGap As Double) As Double
	Return Math.Round(dAmp / ( dGap * 2 ), 4)											'ひずみ
End Function
''' <summary>
''' せん断速度計算 サンプル歪と周期から
''' </summary>
''' <param name="dStrain"></param>
''' <param name="dPeriod"></param>
''' <returns></returns>
Private Function CalcShearSpdFromStrainPrd(ByVal dStrain As Double, ByVal dPeriod As Double) As Double
	Return Math.Round((dStrain * 2 ) / dPeriod, 4)										'剪断速度
End Function
#If 0
''' <summary>
''' 測定圧力[KPa]をAD値へ変換
''' 	p_clSpec.CalcMeasureStrsToADValue(設定圧力[KPa], PB直径[mm], p_cts.WeightCoefZ)
''' </summary>
''' <param name="dMs">			測定圧力[KPa]</param>
''' <param name="dDiameter">	プローブ直径[mm]</param>
''' <param name="dWeightCoef">	重量換算係数</param>
''' <returns>AD値</returns>
Public Function CalcMeasureStrsToADValue(ByVal dMs As Double, ByVal dDiameter As Double, ByVal dWeightCoef As Double) As Double
Dim dPBSq	As Double		'プローブ面積[m2]
Dim dWeight	As Double		'荷重[N]
Dim dADVal	As Double		'AD値
Try
	dPBSq = (dDiameter/1000/2)^2 * Math.PI
	dWeight = (dMs * 1000) * dPBSq											'荷重[N]
	dADVal = dWeight / (dWeightCoef * cGraviAcc/1000)
	Return dADVal
Catch ex As Exception
	Return Double.NaN
End Try
End Function
#end if



''' <summary>
''' Sampメニュー選択イベント
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub SAMP(ByVal sender As Object, ByVal e As System.EventArgs)
	dgvTrgt(2, eSamSPEC.SAMPLE).Value = sender.ToString
	spi.sSampleName = sender.ToString
End Sub
''' <summary>
''' Lotメニュー選択イベント
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub LOT(ByVal sender As Object, ByVal e As System.EventArgs)
	dgvTrgt(2, eSamSPEC.LOT).Value = sender.ToString
	spi.sLot = sender.ToString
End Sub

''' <summary>
''' 測定者メニュー選択イベント
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub MES(ByVal sender As Object, ByVal e As System.EventArgs)
	dgvTrgt(2, eSamSPEC.MEASURER).Value = sender.ToString
	spi.sMeasurer = sender.ToString
End Sub
''' <summary>
''' 形状メニュー選択イベント
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub SHAPE(ByVal sender As Object, ByVal e As System.EventArgs)
	Dim iKeta As int16 = 0
	Dim dMinMax(1) As Double
	dgvTrgt(2, eSamSPEC.SHAPE).Value = sender.ToString

	'変更があった場合は青
	Dim cColor As color = If(frmMain.sPreEditVal<>dgvTrgt(2,eSamSPEC.SHAPE).Value, Color.Blue, Color.black)
	dgvTrgt(2,eSamSPEC.SHAPE).Style.ForeColor = cColor

	'入力アイテムの切替
	Select Case sender.ToString
	Case sShape(eShape.Sheet), sShape(eShape.Rcc)
	'シート
		If dgvTrgt(0, eSamSPEC.SHAPE).Value	<>  sSamSpecShape0_Item(0)
		'->シートへ変更（厚み、積層枚数へ切替）
		End If
		'hintをスイッチ
		If sender.ToString = sShape(eShape.Sheet)
			sSamItemHint(eSamSPEC.SHAPE)				= sSamSpecShape_hint(0)
		Else
			sSamItemHint(eSamSPEC.SHAPE)				= sSamSpecShape_hint(3)
		End If
		sSamItemHint(eSamSPEC.THICKNESS)				= sSamSpecShape0_hint(0)
		sSamItemHint(eSamSPEC.STACKS)					= sSamSpecShape0_hint(1)
		SetToolTipToDgvRow(eSpec.Sample, dgvTrgt, eSamSPEC.SHAPE, Nothing) 

		dgvTrgt(0, eSamSPEC.THICKNESS).Value			=  sSamSpecShape0_Item(0)
		dgvTrgt(1, eSamSPEC.THICKNESS).Value			=  sSamSpecShape0_unit(0)
		SetToolTipToDgvRow(eSpec.Sample, dgvTrgt, eSamSPEC.THICKNESS, sSamSpecShape0_Range(0)) 
		dgvTrgt(0, eSamSPEC.STACKS).Value				=  sSamSpecShape0_Item(1)
		dgvTrgt(1, eSamSPEC.STACKS).Value				=  sSamSpecShape0_unit(1)
		SetToolTipToDgvRow(eSpec.Sample,dgvTrgt, eSamSPEC.STACKS, sSamSpecShape0_Range(1)) 

		iKeta = FloatFormatAndMinMax(dgvTrgt(2, eSamSPEC.THICKNESS).ToolTipText, dMinMax)
		dgvTrgt(2, eSamSPEC.THICKNESS).Value			=  spi.sThickness

		iKeta = FloatFormatAndMinMax(dgvTrgt(2, eSamSPEC.STACKS).ToolTipText, dMinMax)
		dgvTrgt(2, eSamSPEC.STACKS).Value				=  spi.dStack.ToString("F" & iKeta)
		
		spiSamItemSetAndFreeMode(spi.iShape,			eShape.Sheet)

	Case sShape(eShape.Powder)
	'粉
		If dgvTrgt(0, eSamSPEC.SHAPE).Value	<>  sSamSpecShape0_Item(0)
		'->粉へ変更 (密度、重量へ切替)
		End If
		'hintをスイッチ
		sSamItemHint(eSamSPEC.SHAPE)					= sSamSpecShape_hint(1)
		sSamItemHint(eSamSPEC.THICKNESS)				= sSamSpecShape1_hint(0)
		sSamItemHint(eSamSPEC.STACKS)					= sSamSpecShape1_hint(1)
		SetToolTipToDgvRow(eSpec.Sample, dgvTrgt, eSamSPEC.SHAPE, Nothing) 

		dgvTrgt(0, eSamSPEC.THICKNESS).Value			=  sSamSpecShape1_Item(0)
		dgvTrgt(1, eSamSPEC.THICKNESS).Value			=  sSamSpecShape1_unit(0)
		SetToolTipToDgvRow(eSpec.Sample, dgvTrgt, eSamSPEC.THICKNESS, sSamSpecShape1_Range(0)) 
		dgvTrgt(0, eSamSPEC.STACKS).Value				=  sSamSpecShape1_Item(1)
		dgvTrgt(1, eSamSPEC.STACKS).Value				=  sSamSpecShape1_unit(1)
		SetToolTipToDgvRow(eSpec.Sample, dgvTrgt, eSamSPEC.STACKS, sSamSpecShape1_Range(1)) 

		iKeta = FloatFormatAndMinMax(dgvTrgt(2, eSamSPEC.THICKNESS).ToolTipText, dMinMax)
		dgvTrgt(2, eSamSPEC.THICKNESS).Value			=  Ctype(spi.sThickness, Double).ToString("F" & iKeta)

		iKeta = FloatFormatAndMinMax(dgvTrgt(2, eSamSPEC.STACKS).ToolTipText, dMinMax)
		dgvTrgt(2, eSamSPEC.STACKS).Value				=  spi.dStack.ToString("F" & iKeta)

		spiSamItemSetAndFreeMode(spi.iShape,			eShape.Powder)

	Case sShape(eShape.Liquid)
	'液
		If dgvTrgt(0, eSamSPEC.SHAPE).Value	<>  sSamSpecShape2_Item(0)
		'->液へ変更 (密度、体積へ切替)
		End If
		'hintをスイッチ
		sSamItemHint(eSamSPEC.SHAPE)					= sSamSpecShape_hint(2)
		sSamItemHint(eSamSPEC.THICKNESS)				= sSamSpecShape2_hint(0)
		sSamItemHint(eSamSPEC.STACKS)					= sSamSpecShape2_hint(1)
		SetToolTipToDgvRow(eSpec.Sample, dgvTrgt, eSamSPEC.SHAPE, Nothing) 

		dgvTrgt(0, eSamSPEC.THICKNESS).Value			=  sSamSpecShape2_Item(0)
		dgvTrgt(1, eSamSPEC.THICKNESS).Value			=  sSamSpecShape2_unit(0)
		SetToolTipToDgvRow(eSpec.Sample, dgvTrgt, eSamSPEC.THICKNESS, sSamSpecShape2_Range(0)) 
		dgvTrgt(0, eSamSPEC.STACKS).Value				=  sSamSpecShape2_Item(1)
		dgvTrgt(1, eSamSPEC.STACKS).Value				=  sSamSpecShape2_unit(1)
		SetToolTipToDgvRow(eSpec.Sample, dgvTrgt, eSamSPEC.STACKS, sSamSpecShape2_Range(1)) 

		iKeta = FloatFormatAndMinMax(dgvTrgt(2, eSamSPEC.THICKNESS).ToolTipText, dMinMax)
		dgvTrgt(2, eSamSPEC.THICKNESS).Value			=  CType(spi.sThickness, Double).ToString("F" & iKeta)

		iKeta = FloatFormatAndMinMax(dgvTrgt(2, eSamSPEC.STACKS).ToolTipText, dMinMax)
		dgvTrgt(2, eSamSPEC.STACKS).Value				=  spi.dStack.ToString("F" & iKeta)

		spiSamItemSetAndFreeMode(spi.iShape,			eShape.Liquid)

	End Select
End Sub
Const cTempSep As String = "/"
''' <summary>
''' デバイスSPEC 数値入力チェック
''' </summary>
''' <param name="dgv"></param>
''' <param name="bFreeDis">True:FreeMode移行しない</param>
''' <returns>-1:不正文字、-2：範囲外</returns>
Public Function	CheckDevSpecNumericValue(ByRef dgv As DataGridView, Optional bFreeDis As Boolean = False) As int32
	Dim iRet	As int32
	Dim iTRet	As int32 = 0
	UnderStatusLabelDisp( frmMain.ToolStripStatusLabel1, "")							'クリア
	For i=0 To sDevInputRange.Length - 1
		Select Case i
		Case eDevSPEC.HP_CONT
		'選択もの
		Case eDevSPEC.METHOD
		'選択もの
		'Case eDevSPEC.PB_INP_MODE
		''選択もの
		Case eDevSPEC.AMP_MODE
		'選択もの
		Case eDevSPEC.STRS_SENS_MODE
		'選択もの
		Case eDevSPEC.TEMP_PROFILE
		'別窓もの
		Case eDevSPEC.TEMPERATURE
		'温度入力 特殊 
			Dim s() As String = Nothing
			If 0 <= dgv(2, i).Value.indexOf(cTempSep)
			'999/999形式 前HP 後PB
				 s = dgv(2, i).Value.split(cTempSep)
			End If
			Dim dTemp(1) As Double
			iRet = CheckDevSpecNumericValueSub(dgv, i, dTemp, s)
			If iRet<>0
				dgv(2, i).Style.BackColor = Color.Yellow		'エラーセル背景色
				Select Case iRet
				Case -1		'実数変換エラー
					UnderStatusLabelDisp( frmMain.ToolStripStatusLabel1, sUnderMsg1(0), Color.Orange)					'不正文字
				Case -2		'範囲エラー
					UnderStatusLabelDisp( frmMain.ToolStripStatusLabel1, sUnderMsg1(1), Color.Orange)					'範囲外
				End Select
			Else
				dgv(2, i).Style.BackColor = dgv.DefaultCellStyle.BackColor
				'HP/PB温度同一の場合は1回の入力で
				If IsNothing(s)
				'同期設定
					dgv(2, i).Value = Ctype(dgv(2, i).Value.ToString & cTempSep & dgv(2, i).Value.ToString, Object)
					spiDevItemSetAndFreeMode(spi.dHPSetTemp, dTemp(0), bFreeDis)
					spiDevItemSetAndFreeMode(spi.dPBSetTemp, dTemp(0), bFreeDis)

				Else
				'独立設定
					spiDevItemSetAndFreeMode(spi.dHPSetTemp, dTemp(0), bFreeDis)
					spiDevItemSetAndFreeMode(spi.dPBSetTemp, dTemp(1), bFreeDis)

				End If
			End If
		Case eDevSPEC.MEAS_TIME
		'測定終了時間設定
			Dim s() As String = Nothing
			Dim dTemp(0) As Double
			Try
				s = dgv(2, i).Value.split(":")					'時分分割入力セパレータ
			Catch ex As Exception
				dgv(2, i).Style.BackColor = Color.Yellow		'エラーセル背景色
				UnderStatusLabelDisp( frmMain.ToolStripStatusLabel1, sUnderMsg1(0), Color.Orange)					'不正文字
				iTRet += -1
				Continue for
			End Try
			Dim m() As String = Nothing							'数値ワーク
			Array.Resize(m, 1)
			Select Case s(0).Substring(s(0).Length-1, 1)
			Case "m","M"
			'分only
				m(0) = (Ctype(s(0).Substring(0,s(0).Length-1), Double) * 60.0).ToString("F1")
				iRet = CheckDevSpecNumericValueSub(dgv, i, dTemp, m, p_cts.LTModeEnable)
			Case "h","H"
			'時間only
				m(0) = (Ctype(s(0).Substring(0,s(0).Length-1), Double) * 60.0 * 60.0).ToString("F1")
				iRet = CheckDevSpecNumericValueSub(dgv, i, dTemp, m, p_cts.LTModeEnable)
			Case "d","D"
			'日only
				m(0) = (Ctype(s(0).Substring(0,s(0).Length-1), Double) * 3600.0 * 24.0).ToString("F1")
				iRet = CheckDevSpecNumericValueSub(dgv, i, dTemp, m, p_cts.LTModeEnable)
			Case Else
			'時分分割または秒指定
				iRet = ConvertToSeconds(dgv(2, i).Value)
				m(0) = iRet.ToString("F1")
				iRet = CheckDevSpecNumericValueSub(dgv, i, dTemp, m, p_cts.LTModeEnable)
				If p_bMeasTimeEn
				'測定時間制限 許可フラグ ONの時
					If iRet=-2
						iRet = 0
					End If
				End If
			End Select
			If iRet<0
			'チェック異常
				dgv(2, i).Style.BackColor = Color.Yellow		'エラーセル背景色
				Select Case iRet
				Case -1		'実数変換エラー
					UnderStatusLabelDisp( frmMain.ToolStripStatusLabel1, sUnderMsg1(0), Color.Orange)			'不正文字
				Case -2		'範囲エラー
					UnderStatusLabelDisp( frmMain.ToolStripStatusLabel1, sUnderMsg1(1), Color.Orange)			'範囲外
				End Select
			Else
			'チェック正常
				dgv(2, i).Value = dTemp(0).ToString("F1")									'dgvセルへセット
				dgv(2, i).Style.BackColor = dgv.DefaultCellStyle.BackColor					'セル背景色
				spiDevItemSetAndFreeMode(spi.dMeasureTime,		dTemp(0), bFreeDis)			'Free移行不可
			End If
		Case Else
		'数値入力
			Dim s() As String = Nothing
			Dim dTemp(0) As Double
			iRet = CheckDevSpecNumericValueSub(dgv, i, dTemp, s)
			If p_bMeasTimeEn
			'測定時間制限 許可フラグ ONの時
				Select Case i
				Case eDevSPEC.MEAS_TIME
					iRet = 0					
				End Select
			End If
			iTRet += iRet
			If iRet<>0
				dgv(2, i).Style.BackColor = Color.Yellow		'エラーセル背景色
				Select Case iRet
				Case -1		'実数変換エラー
					UnderStatusLabelDisp( frmMain.ToolStripStatusLabel1, sUnderMsg1(0), Color.Orange)					'不正文字
				Case -2		'範囲エラー
					UnderStatusLabelDisp( frmMain.ToolStripStatusLabel1, sUnderMsg1(1), Color.Orange)					'範囲外
				End Select
			Else
				dgv(2, i).Style.BackColor = dgv.DefaultCellStyle.BackColor
				Select Case i
				Case eDevSPEC.THRETHOLD
					spiDevItemSetAndFreeMode(spi.dThreshold, dTemp(0), bFreeDis)

				Case eDevSPEC.AMPMODE_STRESS
					spiDevItemSetAndFreeMode(spi.dAmpModeStress, dTemp(0), bFreeDis)

				Case eDevSPEC.MEAS_GAP		
					spiDevItemSetAndFreeMode(spi.dMeasureGap, dTemp(0), bFreeDis)

				Case eDevSPEC.MEAS_STRESS
					spiDevItemSetAndFreeMode(spi.dMeasureStress, dTemp(0), bFreeDis)

				Case eDevSPEC.PRESSCONT_STEP
					spiDevItemSetAndFreeMode(spi.iPressContStep, dTemp(0), bFreeDis)

				Case eDevSPEC.PB_DIAMETER
					spiDevItemSetAndFreeMode(spi.dProbDiameter, dTemp(0), bFreeDis)

				Case eDevSPEC.AMPLITUDE
					'Select Case spi.iProbInputMode
					'Case 0	'直接
						spiDevItemSetAndFreeMode(spi.dXAmplitude, dTemp(0), bFreeDis)
						spiDevItemSetAndFreeMode(spi.dYAmplitude, dTemp(0), bFreeDis)
						spiDevItemSetAndFreeMode(spi.dZAmplitude, dTemp(0), bFreeDis)

					'Case 1	'間接
					'	spiDevItemSetAndFreeMode(spi.dShearSpeed,	dTemp(0))
					'	Dim dAmp As Double
					'	Dim dPrd As Double
					'	dAmp = CalcAmpFromStrainGap(spi.dSampleStrain, spi.dMeasureGap)
					'	dPrd = CalcPrdFromStrainShearSpd(spi.dSampleStrain, spi.dShearSpeed)
					'	spiDevItemSetAndFreeMode(spi.dXAmplitude,	dAmp)
					'	spiDevItemSetAndFreeMode(spi.dYAmplitude,	dAmp)
					'	spiDevItemSetAndFreeMode(spi.dZAmplitude,	dAmp)
					'	spiDevItemSetAndFreeMode(spi.dXPeriod,		dPrd)
					'	spiDevItemSetAndFreeMode(spi.dYPeriod,		dPrd)
					'	spiDevItemSetAndFreeMode(spi.dZPeriod,		dPrd)

					'End Select
				Case eDevSPEC.PERIOD
					'Select Case spi.iProbInputMode
					'Case 0	'直接
						spiDevItemSetAndFreeMode(spi.dXPeriod,		dTemp(0), bFreeDis)
						spiDevItemSetAndFreeMode(spi.dYPeriod,		dTemp(0), bFreeDis)
						spiDevItemSetAndFreeMode(spi.dZPeriod,		dTemp(0), bFreeDis)

					'Case 1	'間接
					'	spi.dSampleStrain	= dTemp(0)
					'	Dim dAmp As Double
					'	Dim dPrd As Double
					'	dAmp = CalcAmpFromStrainGap(spi.dSampleStrain, spi.dMeasureGap)
					'	dPrd = CalcPrdFromStrainShearSpd(spi.dSampleStrain, spi.dShearSpeed)
					'	spiDevItemSetAndFreeMode(spi.dXAmplitude,	dAmp)
					'	spiDevItemSetAndFreeMode(spi.dYAmplitude,	dAmp)
					'	spiDevItemSetAndFreeMode(spi.dZAmplitude,	dAmp)
					'	spiDevItemSetAndFreeMode(spi.dXPeriod,		dPrd)
					'	spiDevItemSetAndFreeMode(spi.dYPeriod,		dPrd)
					'	spiDevItemSetAndFreeMode(spi.dZPeriod,		dPrd)

					'End Select

				Case eDevSPEC.AMPMODE_STRESS
					spiDevItemSetAndFreeMode(spi.dAmpModeStress, dTemp(0), bFreeDis)

				Case eDevSPEC.HP_UP_WAIT
					spiDevItemSetAndFreeMode(spi.dInputToHPUpWait,	dTemp(0), bFreeDis)

				Case eDevSPEC.ACTION_WAIT
					spiDevItemSetAndFreeMode(spi.dHPUpToXYActWait,	dTemp(0), bFreeDis)

				'Case eDevSPEC.MEAS_TIME
				'	spiDevItemSetAndFreeMode(spi.dMeasureTime,		dTemp(0), bFreeDis)

				Case eDevSPEC.MEAS_END_STRESS
					spiDevItemSetAndFreeMode(spi.dMeasureEndStress,	dTemp(0), bFreeDis)

				Case eDevSPEC.DIFF_INTERVAL
					spiDevItemSetAndFreeMode(spi.dDiffInterval,	dTemp(0), bFreeDis)
					If p_cts.LTModeEnable
						dgv(2, i).Style.BackColor = Color.LightGray
					Else
						dgv(2, i).Style.BackColor = dgv.DefaultCellStyle.BackColor
					End If

				End Select
			End If
		End Select
	Next
	'応力上限(%)をフルストレス値から重量(N)へ変換
	If p_RTM(0).p_iBGWOrder = BGW_ORDER.NO_ORDER
	'制御測定中は行わない
		Select Case spi.iStressSensMode
		Case eStrsSens.Circle, eStrsSens.Line
			iRet = modFrmMain.SetFullStress(spi.dXAmplitude, spi.dXPeriod, spi.iStressSensMode)				'キャリブレーション値を取得
		Case eStrsSens.Point
			iRet = modFrmMain.SetFullStress(spi.dZAmplitude, spi.dZPeriod, spi.iStressSensMode)				'キャリブレーション値を取得
		End Select
		If iRet = 0
			spi.dAmpModeStressN = (spi.dAmpModeStress / 100) * p_RTM(0).p_dFullStressADVal
		End If
	End If
	Return iTRet
End Function


''' <summary>
''' hh:mm:ssフォーマット文字列を秒数に変換
''' セパレータが無い場合は秒で扱う
''' </summary>
''' <param name="timeString"></param>
''' <returns></returns>
Function ConvertToSeconds(ByVal timeString As String) As Double
Try
    ' 時:分:秒 の形式で分割
    Dim parts As String() = timeString.Split(":"c)
    ' 時、分、秒をそれぞれ取得し、整数に変換
    Dim hours	As Double = 0
    Dim minutes As Double = 0
    Dim seconds As Double = 0
	If parts.Length>1
		For i=0 To parts.GetUpperBound(0)
			Select Case i
			Case 0
				hours	= Ctype(parts(i), Double)
			Case 1
				minutes = Ctype(parts(i), Double)
			Case 2
				seconds = Ctype(parts(i), Double)
				If seconds<0 Or seconds>60
					Return -1
				End If
			End Select
		Next
	Else
		seconds = Ctype(parts(0), Double)
	End If
	If minutes<0 Or minutes>60
		Return -1
	End If
    ' 合計秒数を計算
    Return (hours * 3600) + (minutes * 60) + seconds
Catch ex As Exception
	Return -1
End Try
End Function


''' <summary>
''' デバイスSPEC 数値入力チェックサブ
''' </summary>
''' <param name="dgv"></param>
''' <param name="iRow"></param>
''' <param name="s"></param>
''' <returns></returns>
Public Function CheckDevSpecNumericValueSub(ByRef dgv As DataGridView, ByVal iRow As int32, Byref dTemp() As Double, Optional s() As String = Nothing, Optional bLTM As Boolean=False) As int32
	Dim sTemp	As String
	Dim sTemp1	As String = Nothing
	Dim i		As int32
	Dim lmt		As int32 = If( IsNothing(s), 0, s.Length-1 )	'ループ回数 （温度入力が２つあるため）
	Dim iRet	As Int32 = 0
Try
	For i = 0 To lmt
		'実数変換 ---------------------------
		Try
			If lmt >= 1 Or bLTM
				sTemp = s(i)
			Else
				sTemp = dgv(2, iRow).Value
			End If
			dTemp(i) = Double.Parse(sTemp)
		Catch ex As Exception
			Return -1		'実数変換エラー
		End Try
		'範囲チェック -----------------------
		Dim dMinMax(1) As Double
		'入力範囲文字列から小数点以下桁数と最小/最大値を得る
		Dim iKeta As Int16 = FloatFormatAndMinMax(dgv(2, iRow).ToolTipText, dMinMax)
		If dMinMax.Length=2
			Select Case dTemp(i)
			Case dMinMax(0) To dMinMax(1)
			Case Else
				iRet = -2		'範囲エラー
			End Select
		Else
			iRet = 0
		End If
		'フォーマット -----------------------
		If IsNothing(stemp1)
			sTemp1 = dtemp(i).ToString("F" & iKeta)
		Else
			sTemp1 &= cTempSep & dtemp(i).ToString("F" & iKeta)
		End if
	Next i
	dgv(2, iRow).Value = sTemp1
	Return iRet

Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Return -1
End Try
End Function
''' <summary>
''' 入力範囲文字列から小数点以下桁数と最大最小値を返す
''' </summary>
''' <param name="s"> 例） 0.00-20.00</param>
''' <param name="dMinMax">最小-最大</param>
''' <returns></returns>
Private Function FloatFormatAndMinMax(ByVal s As String, ByRef dMinMax() As Double) As int16
	If s.Length <> 0
		Dim sMinMax() As String = s.Split("-")
		dMinMax(0) = CType(sMinMax(0), Double)
		dMinMax(1) = CType(sMinMax(1), Double)
		If sMinMax(0).IndexOf(".") = -1
			Return 0
		Else
			Dim sFloat() As String 
			sFloat = sMinMax(0).Split(".")
			Return sFloat(1).Length
		End If
	Else
		Return 0
	End If
End Function
''' <summary>
''' サンプルSPEC 数値入力チェック
''' </summary>
''' <param name="dgv"></param>
''' <param name="bFreeDis">True:FreeMode移行しない</param>
''' <returns>-1:不正文字、-2：範囲外</returns>
Public Function	CheckSamSpecNumericValue(ByRef dgv As DataGridView, Optional bFreeDis As Boolean = False) As int32
	Dim iRet As int32
	'Dim dTemp As Double
Try
	UnderStatusLabelDisp( frmMain.ToolStripStatusLabel1, "")							'クリア
	For i=0 To sSamInputRange.Length - 1
		Select Case i
		Case eSamSPEC.SAMPLE
		'文字数チェックするか？
		Case eSamSPEC.LOT
		'文字数チェックするか？
		Case eSamSPEC.MEASURER
		'文字数チェックするか？
		Case eSamSPEC.SHAPE
		'選択もの
		Case eSamSPEC.EFECT_RANGE
		'別窓もの
		Case eSamSPEC.JUDGE_RANGE
		'別窓もの
			'Select Case spi.iShape
			'Case eShape.Sheet
			''特殊入力
			'	Dim s() As String = Nothing
			'	If 0 <= dgv(2, i).Value.indexOf(cTempSep)
			'	'999/999形式 前:シート厚 後:基材厚
			'		 s = dgv(2, i).Value.split(cTempSep)
			'	End If
			'	Dim dTmp(1) As Double
			'	iRet = CheckDevSpecNumericValueSub(dgv, i, dTmp, s)
			'	If iRet<>0
			'		dgv(2, i).Style.BackColor = Color.Yellow		'エラーセル背景色
			'		Select Case iRet
			'		Case -1		'実数変換エラー
			'			UnderStatusLabelDisp( frmMain.ToolStripStatusLabel1, sUnderMsg1(0), Color.Orange)					'不正文字
			'		Case -2		'範囲エラー
			'			UnderStatusLabelDisp( frmMain.ToolStripStatusLabel1, sUnderMsg1(1), Color.Orange)					'範囲外
			'		End Select
			'	Else
			'		dgv(2, i).Style.BackColor = dgv.DefaultCellStyle.BackColor
			'		'一回の入力でシート厚:9.999 基材厚:0.000 
			'		If IsNothing(s)
			'		'同期設定
			'			dgv(2, i).Value = dTmp(0).ToString("F3") & cTempSep & "0.000"
			'		Else
			'		'独立設定
			'			dgv(2, i).Value = dTmp(0).ToString("F3") & cTempSep & dTmp(1).ToString("F3")
			'		End If
			'		spiDevItemSetAndFreeMode(spi.sThickness, dgv(2, i).Value)
			'	End If
			'Case Else
			'	iRet = CheckSamSpecNumericValueSub(dgv, i)
			'	If iRet>=1
			'		Exit For
			'	End If
			'End Select

		Case Else
		'Case eSamSPEC.THICKNESS
		'Case eSamSPEC.STACKS
		'数値入力
			iRet = CheckSamSpecNumericValueSub(dgv, i)
			If iRet>=1
				Exit For
			End If
		End Select
	Next
	Return iRet

Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Return -1
End Try
End Function
''' <summary>
''' サンプルSPEC 数値入力チェック サブ
''' </summary>
''' <param name="dgv"></param>
''' <param name="i"></param>
''' <returns></returns>
Private Function CheckSamSpecNumericValueSub(ByRef dgv As DataGridView, ByVal i As int32) As Int32
	Dim iRet As int32
	Dim dTemp As Double
Try
	iRet = CheckSamSpecNumericValueSub(dgv, i, dTemp)
	If iRet<>0
		dgv(2, i).Style.BackColor = Color.Yellow		'エラーセル背景色
		Select Case iRet
		Case -1		'実数変換エラー
			UnderStatusLabelDisp( frmMain.ToolStripStatusLabel1, sUnderMsg1(0), Color.Orange)					'不正文字
			Return 1
		Case -2		'範囲エラー
			UnderStatusLabelDisp( frmMain.ToolStripStatusLabel1, sUnderMsg1(1), Color.Orange)					'範囲外
			Return 2
		End Select
	Else
		dgv(2, i).Style.BackColor = dgv.DefaultCellStyle.BackColor
		Select Case i
		Case eSamSPEC.THICKNESS
			spiSamItemSetAndFreeMode(spi.sThickness,	dTemp)

		Case eSamSPEC.STACKS
			spiSamItemSetAndFreeMode(spi.dStack,		dTemp)

		End Select
	End If
	Return 0
Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Return -1
End Try
End Function
''' <summary>
''' デバイスSPEC関係のSPI構造体値と変更があればセットし、Freeモードへ移行
''' </summary>
''' <param name="d">SPI値</param>
''' <param name="s">変更値</param>
''' <param name="bFreeDisable">True:Freeへ移行しない</param>
Private Sub	spiDevItemSetAndFreeMode( ByRef d As Object, ByVal s As Object, Optional bFreeDisable As Boolean = False )
	If d <> s
		d = s
		If Not bFreeDisable
			frmMain.DevSpecToFree()							'Freeモード移行
		End If
	End If
End Sub
''' <summary>
''' サンプルSPEC関係のSPI構造体値と変更があればセットし、Freeモードへ移行
''' </summary>
''' <param name="d">SPI値</param>
''' <param name="s">変更値</param>
''' <param name="bFreeDisable">True:Freeへ移行しない</param>
Private Sub	spiSamItemSetAndFreeMode( ByRef d As Object, ByVal s As Object, Optional bFreeDisable As Boolean = False )
	If d <> s
		d = s
		If Not bFreeDisable
			frmMain.SamSpecToFree()							'Freeモード移行
		End If
	End If
End Sub




''' <summary>
''' サンプルSPEC 数値入力チェックサブ
''' </summary>
''' <param name="dgv"></param>
''' <param name="iRow"></param>
''' <param name="s"></param>
''' <returns></returns>
Private Function CheckSamSpecNumericValueSub(ByRef dgv As DataGridView, ByVal iRow As int32, ByRef dTemp As Double, Optional s() As String = Nothing) As int32
	Dim sTemp	As String
	'実数変換 ---------------------------
	Try
		sTemp = dgv(2, iRow).Value
		dTemp = Double.Parse(sTemp)
	Catch ex As Exception
		Return -1		'実数変換エラー
	End Try
	'範囲チェック -----------------------
	Dim sMinMax() As String = Nothing
	'sMinMax = sSamInputRange(iRow).Split("-")
	sMinMax = dgv(2, iRow).ToolTipText.Split("-")
	Select Case dTemp
	Case Cdbl(sMinMax(0)) To cDbl(sMinMax(1))
	Case Else
		Return -2		'範囲エラー
	End Select
	'フォーマット -----------------------
	Dim sFloat() As String = sMinMax(0).Split(".")
	If sFloat.Length >= 2
	'小数点指定の場合
		sTemp = dtemp.ToString("F" & sFloat(1).Length)
	End if
	dgv(2, iRow).Value = sTemp
	Return 0
End Function

''' <summary>
''' 入力範囲をデータグリッドビュー０列ToolTipTextへセットする
''' </summary>
''' <param name="iT"></param>
''' <param name="dgv"></param>
Public Sub InputRangeSet(ByVal iT As int32, ByRef dgv As DataGridView)
	Dim i As Int32
	Select Case iT
	Case eSpec.Device
	'デバイスSPEC
		For i=0 To dgv.RowCount-1
			Select Case i
			Case eDevSPEC.AMPLITUDE
				Select Case dgv(0, eDevSPEC.AMPLITUDE).Value
				Case sDevSpecPBInpMode0_Item(0)
				'振幅項目である
					SetToolTipToDgvRow(iT, dgv, eDevSPEC.AMPLITUDE, sDevSpecPBInpMode0_Range(0)) 
			
				Case sDevSpecPBInpMode1_Item(0)
				'せん断速度 x 3.14項目である
					SetToolTipToDgvRow(iT, dgv, eDevSPEC.AMPLITUDE, sDevSpecPBInpMode1_Range(0)) 
				Case Else

				End Select
			Case eDevSPEC.PERIOD
				Select Case dgv(0, eDevSPEC.PERIOD).Value
				Case sDevSpecPBInpMode0_Item(1)
				'周期項目である
					SetToolTipToDgvRow(iT, dgv, eDevSPEC.PERIOD, sDevSpecPBInpMode0_Range(1)) 
			
				Case sDevSpecPBInpMode1_Item(1)
				'サンプルひずみ項目である
					SetToolTipToDgvRow(iT, dgv, eDevSPEC.PERIOD, sDevSpecPBInpMode1_Range(1)) 
				Case Else

				End Select
			Case eDevSPEC.MEAS_TIME
				'間欠測定モード時の測定終了時間の入力範囲変更(最大10日)
				If p_cts.LTModeEnable
					sDevInputRange(i) = sDevInputRange_MeasTime(1)
				Else
					sDevInputRange(i) = sDevInputRange_MeasTime(0)
				End If
				SetToolTipToDgvRow(iT, dgv, i, sDevInputRange(i)) 
			Case Else
			'変更なし項目
				SetToolTipToDgvRow(iT, dgv, i, sDevInputRange(i)) 
			End Select

		Next
	Case eSpec.Sample
	'サンプルSPEC
		For i=0 To dgv.RowCount-1
			Select Case i
			Case eSamSPEC.THICKNESS
				Select Case dgv(0, eSamSPEC.THICKNESS).Value
				Case sSamSpecShape0_Item(0)
				'厚み項目である
					SetToolTipToDgvRow(iT, dgv, eSamSPEC.THICKNESS, sSamSpecShape0_Range(0)) 
			
				Case sSamSpecShape1_Item(0)
				'密度項目である
					SetToolTipToDgvRow(iT, dgv, eSamSPEC.THICKNESS, sSamSpecShape1_Range(0)) 

				Case sSamSpecShape2_Item(0)
				'密度項目である
					SetToolTipToDgvRow(iT, dgv, eSamSPEC.THICKNESS, sSamSpecShape2_Range(0)) 
				Case Else

				End Select
			Case eSamSPEC.STACKS
				Select Case dgv(0, eSamSPEC.STACKS).Value
				Case sSamSpecShape0_Item(1)
				'積層枚数項目である
					SetToolTipToDgvRow(iT, dgv, eSamSPEC.STACKS, sSamSpecShape0_Range(1)) 
			
				Case sSamSpecShape1_Item(1)
				'重量項目である
					SetToolTipToDgvRow(iT, dgv, eSamSPEC.STACKS, sSamSpecShape1_Range(1)) 

				Case sSamSpecShape2_Item(1)
				'体積項目である
					SetToolTipToDgvRow(iT, dgv, eSamSPEC.STACKS, sSamSpecShape2_Range(1)) 
				Case Else

				End Select
			Case eSamSPEC.EFECT_RANGE,eSamSPEC.JUDGE_RANGE
				'間欠測定モード時の測定終了時間の入力範囲変更(最大10日)
				If p_cts.LTModeEnable
					sSamInputRange(i) = sSamInputRange_AppStdTime(1)
				Else
					sSamInputRange(i) = sSamInputRange_AppStdTime(0)
				End If
				SetToolTipToDgvRow(iT, dgv, i, sSamInputRange(i)) 
			Case Else
			'変更なし項目
				SetToolTipToDgvRow(iT, dgv, i, sSamInputRange(i)) 
			End Select
		Next
	End Select
End Sub

''' <summary>
''' SPECファイル正常確認
''' </summary>
''' <param name="iT"></param>
''' <param name="AryL"></param>
''' <returns></returns>
Public Function CheckSpecFile(ByVal iT As int32, ByVal AryL As ArrayList) As Int32
Try
	Select Case iT
	Case eSpec.Device
		'行数確認
		If AryL.Count <> eDevSPEC.FINAL
			Return -9
		End If
		'プローブ動作設定の入力方式(直接/間接)により項目名・単位を変更
		'Select Case AryL(edevSPEC.PB_INP_MODE)(2)
		'Case ePBInpMode.Direct
			AryL(eDevSPEC.AMPLITUDE)(0) = sDevSpecPBInpMode0_Item(0)			'項目名
			AryL(eDevSPEC.PERIOD)(0)	= sDevSpecPBInpMode0_Item(1)			'項目名
			AryL(eDevSPEC.AMPLITUDE)(1) = sDevSpecPBInpMode0_Unit(0)			'単位
			AryL(eDevSPEC.PERIOD)(1)	= sDevSpecPBInpMode0_Unit(1)			'単位

		'Case ePBInpMode.Indirect
		'	AryL(eDevSPEC.AMPLITUDE)(0) = sDevSpecPBInpMode1_Item(0)			'項目名
		'	AryL(eDevSPEC.PERIOD)(0)	= sDevSpecPBInpMode1_Item(1)			'項目名
		'	AryL(eDevSPEC.AMPLITUDE)(1) = sDevSpecPBInpMode1_Unit(0)			'単位
		'	AryL(eDevSPEC.PERIOD)(1)	= sDevSpecPBInpMode1_Unit(1)			'単位
		'End Select

	Case eSpec.Sample
		'行数確認
		If AryL.Count <> eSamSPEC.FINAL
			Return -9
		End If
		'形状により項目名・単位を変更
		Select Case AryL(eSamSPEC.SHAPE)(2)
		Case eShape.Sheet
			AryL(eSamSPEC.THICKNESS)(0) = sSamSpecShape0_Item(0)				'項目名
			AryL(eSamSPEC.STACKS)(0)	= sSamSpecShape0_Item(1)				'項目名
			AryL(eSamSPEC.THICKNESS)(1) = sSamSpecShape0_Unit(0)				'単位
			AryL(eSamSPEC.STACKS)(1)	= sSamSpecShape0_Unit(1)				'単位

		Case eShape.Powder
			AryL(eSamSPEC.THICKNESS)(0) = sSamSpecShape1_Item(0)				'項目名
			AryL(eSamSPEC.STACKS)(0)	= sSamSpecShape1_Item(1)				'項目名
			AryL(eSamSPEC.THICKNESS)(1) = sSamSpecShape1_Unit(0)				'単位
			AryL(eSamSPEC.STACKS)(1)	= sSamSpecShape1_Unit(1)				'単位

		Case eShape.Liquid
			AryL(eSamSPEC.THICKNESS)(0) = sSamSpecShape2_Item(0)				'項目名
			AryL(eSamSPEC.STACKS)(0)	= sSamSpecShape2_Item(1)				'項目名
			AryL(eSamSPEC.THICKNESS)(1) = sSamSpecShape2_Unit(0)				'単位
			AryL(eSamSPEC.STACKS)(1)	= sSamSpecShape2_Unit(1)				'単位

		End Select
	End Select
	Return 0
Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Return -1
End Try
End Function




















''' <summary>
''' カンマ区切りファイル(CSVファイル UTF-8)をアレイリストへ読込む
''' </summary>
''' <param name="sPath">CSVファイルフルパス</param>
''' <param name="AryL">リストアレイ</param>
Public Function CSVToDataTable(ByVal sPath As String, ByRef AryL As ArrayList) As int32
Try
	Dim csvRecords As New ArrayList
	Dim tfp As New FileIO.TextFieldParser(sPath, 	Encoding.UTF8)
	tfp.TextFieldType = FileIO.FieldType.Delimited
	tfp.Delimiters = New String() {","}				'区切り文字を,とする
	tfp.HasFieldsEnclosedInQuotes = True			'フィールドを"で囲み、改行文字、区切り文字を含めることができるか
	tfp.TrimWhiteSpace = True						'フィールドの前後からスペースを削除する

	While Not tfp.EndOfData
		Dim fields As String() = tfp.ReadFields()	'フィールドを読み込む
		If fields.Length > 3
		'値アイテム内の[,]文字対応
			Dim i As Int16 = 0
			For i=3 To fields.GetUpperBound(0)
				fields(2) &= "," & fields(i)
			Next

		End If
		csvRecords.Add(fields)						'保存
	End While
	tfp.Close()
	AryL = csvRecords
	Return 0
Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Return -1
End Try
End Function
''' <summary>
''' アレイリストデータをカンマ区切りファイルへ書込む
''' </summary>
''' <param name="sPath"></param>
''' <param name="AryL"></param>
''' <returns></returns>
Public Function CnvAryListToCSV(ByVal sPath As String, ByVal AryL As ArrayList) As int32
    '書き込むファイルを開く
    Dim sr As New System.IO.StreamWriter(sPath, False, encoding.UTF8)
    Dim i As Integer
	Dim sBuf As String = Nothing
Try
	'全行書込み
    For i=0 To AryL.Count - 1
		sBuf = AryL.Item(i)(0) & "," & AryL.Item(i)(1) & "," & AryL.Item(i)(2) & vbCrLf
        sr.Write(sBuf)
    Next i
    '閉じる
    sr.Close()
	Return 0
Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Return -1
End Try
End Function















''' <summary>
''' スペック情報をRealTimeスレッドへ引き渡し
''' </summary>
Public Sub sbSpecToLinkArea()
'Public Sub sbSpecToLinkArea(ByRef frm As Form, ByVal dgv As DataGridVIEW)
Try
	'熱板温度範囲
	spi.dHPTempOkRange			= p_cts.HPTempRange()		'詳細設定より
	'プローブ温度範囲
	spi.dPBTempOkRange			= p_cts.PBTempRange()		'詳細設定より
	'投入待ち時間
	spi.dSampleInputWait		= p_cts.SampleInputWait		'詳細設定より
	'投入Gap
	spi.dSampleGap				= p_cts.InputGap			'詳細設定より


	If IsNothing(p_RTM) Then
		Exit Sub
	End If
	'リンクエリアへセット -------------------
	If Not (Application.OpenForms().OfType(Of frmCalibFull).Any Or Application.OpenForms().OfType(Of frmViscosity).Any) Then
		'測定モード（熱板位置制御）
		p_RTM(0).p_mltcp.MeasureMode		= spi.iHPPosCont
		'100%応力較正測定/粘弾性測定が行われていない
		If Not (Application.OpenForms().OfType(Of frmTempAdjMon).Any) Then
			'温度調整画面起動中でない
			If p_RTM(0).p_strRTGSetName.Length = 0 Then
				'プロファイル制御でない
				'熱板温度
				p_RTM(0).p_mltcp.TempPreset(1)	= spi.dHPSetTemp
				'プローブ温度
				p_RTM(0).p_mltcp.Temp2Preset(1) = spi.dPBSetTemp
				'起歪体温度
				p_RTM(0).p_mltcp.Temp3Preset(1) = SGTempGet(spi.dHPSetTemp)
			End If
		End If
		'GAP
		p_RTM(0).p_mltcp.GapPreset(1)		= spi.dMeasureGap
		'測定圧力
		p_RTM(0).p_mltcp.MeasureStress		= datCal.CalcMeasureStrsToADValue(spi.dMeasureStress, spi.dProbDiameter, p_cts.WeightCoefZ)
		'投入GAP
		p_RTM(0).p_mltcp.InpGap(1)			= spi.dSampleGap
		'測定待ち時間
		p_RTM(0).p_mltcp.MeasWaitTime(1)	= spi.dInputToHPUpWait
		'投入待ち時間（クリア時間）
		p_RTM(0).p_mltcp.ClearTime(1)		= p_cts.SampleInputWait()		'詳細設定より
		'XY動作待機時間
		p_RTM(0).p_mltcp.MesPreWait(1)		= spi.dHPUpToXYActWait
		'XY動作モード
		'p_RTM(0).p_mltcp.ActMode			= 2							'SIN波固定(削除された!! 5/30)

		'着圧制御ディレイ時間[mS]
		p_RTM(0).p_mltcp.PressContDelay		= p_cts.PressContDelayTime
		'着圧制御STEP
		p_RTM(0).p_mltcp.PressContStep		= spi.iPressContStep

		'振幅測定モード
		p_RTM(0).p_mltcp.AmpMode			= spi.iAmpMode

		'振幅測定モード 上限応力(uN->AD) 1gf = 9806.65N
		p_RTM(0).p_mltcp.AmpModeAmplitude	= spi.dAmpModeStressN / (p_cts.WeightCoefZ * p_cuNperGram)		'デジタル値へ変換


		'応力検知モードとプローブ動作設定を考慮
		Select Case spi.iStressSensMode
		Case eStrsSens.Line		'線
			'Select Case spi.iProbInputMode
			'Case 0	'直接
				spi.dSampleStrain					= CalcStrainFromAmpGap(spi.dXAmplitude, spi.dMeasureGap)
				spi.dShearSpeed						= CalcShearSpdFromStrainPrd(spi.dSampleStrain, spi.dXPeriod)
				If Not p_cts.LTModeEnable	'長期間欠モード時はRealTime制御で設定する
					'X軸振幅
					p_RTM(0).p_mltcp.AmplitudeX			= spi.dXAmplitude * 1000.0
					'Y軸振幅
					p_RTM(0).p_mltcp.AmplitudeY			= 0
					'Z軸振幅
					p_RTM(0).p_mltcp.AmplitudeZ			= 0
				End If
				'X軸周期
				p_RTM(0).p_mltcp.PeriodX(0)			= spi.dXPeriod * 10.0
				'Y軸周期
				p_RTM(0).p_mltcp.PeriodY(0)			= 1 * 10.0
				'Z軸周期
				p_RTM(0).p_mltcp.PeriodZ(0)			= 1 * 10.0

			'Case 1	'間接
			'	spi.dXAmplitude						= CalcAmpFromStrainGap(spi.dSampleStrain, spi.dMeasureGap)
			'	spi.dXPeriod						= CalcPrdFromStrainShearSpd(spi.dSampleStrain, spi.dShearSpeed)
			'	'X軸振幅
			'	p_RTM(0).p_mltcp.AmplitudeX			= spi.dXAmplitude * 1000.0
			'	'Y軸振幅
			'	p_RTM(0).p_mltcp.AmplitudeY			= 0
			'	'Z軸振幅
			'	p_RTM(0).p_mltcp.AmplitudeZ			= 0
			'	'X軸周期
			'	p_RTM(0).p_mltcp.PeriodX(0)			= spi.dXPeriod * 10.0
			'	'Y軸周期
			'	p_RTM(0).p_mltcp.PeriodY(0)			= 1 * 10.0
			'	'Z軸周期
			'	p_RTM(0).p_mltcp.PeriodZ(0)			= 1 * 10.0

			'End Select
			
		Case eStrsSens.Circle		'円
			If Not p_cts.LTModeEnable	'長期間欠モード時はRealTime制御で設定する
				'X軸振幅
				p_RTM(0).p_mltcp.AmplitudeX			= spi.dXAmplitude * 1000.0
				'Y軸振幅
				p_RTM(0).p_mltcp.AmplitudeY			= spi.dYAmplitude * 1000.0
				'Z軸振幅
				p_RTM(0).p_mltcp.AmplitudeZ			= 0
			End If
			'X軸周期
			p_RTM(0).p_mltcp.PeriodX(0)			= spi.dXPeriod * 10.0
			'Y軸周期
			p_RTM(0).p_mltcp.PeriodY(0)			= spi.dYPeriod * 10.0
			'Z軸周期
			p_RTM(0).p_mltcp.PeriodZ(0)			= 1 * 10.0

			spi.dSampleStrain					= CalcStrainFromAmpGap(spi.dXAmplitude, spi.dMeasureGap)
			spi.dShearSpeed						= CalcShearSpdFromStrainPrd(spi.dSampleStrain, spi.dXPeriod)

		Case eStrsSens.Point		'点
			If Not p_cts.LTModeEnable	'長期間欠モード時はRealTime制御で設定する
				'X軸振幅
				p_RTM(0).p_mltcp.AmplitudeX			= 0
				'Y軸振幅
				p_RTM(0).p_mltcp.AmplitudeY			= 0
				'Z軸振幅
				p_RTM(0).p_mltcp.AmplitudeZ			= spi.dZAmplitude * 1000.0
			End If
			'X軸周期
			p_RTM(0).p_mltcp.PeriodX(0)			= 1 * 10.0
			'Y軸周期
			p_RTM(0).p_mltcp.PeriodY(0)			= 1 * 10.0
			'Z軸周期
			p_RTM(0).p_mltcp.PeriodZ(0)			= spi.dZPeriod * 10.0

			spi.dSampleStrain					= CalcStrainFromAmpGap(spi.dZAmplitude, spi.dMeasureGap)
			spi.dShearSpeed						= CalcShearSpdFromStrainPrd(spi.dSampleStrain, spi.dZPeriod)

		End Select
	Else
	'キャリブレーション、粘度測定
		p_RTM(0).p_mltcp.MeasureMode		= 0		'GAP固定
		'振幅測定モード
		p_RTM(0).p_mltcp.AmpMode			= 0		'振幅一定モード固定 （応力一定モードを設定すると振幅変更中となるため）
	End If




	'熱板温度範囲
	p_RTM(0).p_mltcp.TempOkRange(1)			= p_cts.HPTempRange()		'詳細設定より
	'プローブ温度範囲
	p_RTM(0).p_mltcp.Temp2OkRange(1)		= p_cts.PBTempRange()		'詳細設定より
	'起歪体温度範囲
	p_RTM(0).p_mltcp.Temp3OkRange(1)		= p_cts.SGTempRange()		'詳細設定より
	'トルク判定閾値 (判定用に)
	p_RTM(0).p_dToqueThreshold				= spi.dThreshold
	'測定終了応力
	p_RTM(0).p_dStressLimit					= spi.dMeasureEndStress
	'測定終了時間
	'If p_RTM(0).p_strRTGSetName.Length = 0 Then
	''プロファイル制御でない
		p_RTM(0).p_dMeasTimeLimit			= spi.dMeasureTime
	'Else
	'	p_RTM(0).p_dMeasTimeLimit			= p_RTM(0).p_rtgset.dTTotal
	'End If
	'微分区間(SLOPE)
	p_RTM(0).p_dDiffInterval				= spi.dDiffInterval


Catch ex As Exception
    Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
    Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub
Public Function SGTempGet(ByVal dHPTemp As Double) As Double
	Dim dTemp As Double = 0.0
	dTemp = dHPTemp - p_cts.TempCnt_HDifSetTemp3
	If dTemp > p_cts.TempCnt_HUpperLimit3
		dTemp = p_cts.TempCnt_HUpperLimit3
	ElseIf dTemp < p_cts.TempCnt_HLowerLimit3
		dTemp = p_cts.TempCnt_HLowerLimit3
	Else
		dTemp = dHPTemp - p_cts.TempCnt_HDifSetTemp3
	End If
	Return dTemp
End Function
''' <summary>
''' SPEC データグリッドビュー表示更新 ファイル読込から
''' </summary>
''' <param name="iT">0,1</param>
''' <param name="frm">frmMainオブジェクト</param>
''' <param name="sSpecName">SPEC名(ファイル名)</param>
Public Sub dgvSpecDispUpdate(ByVal iT As int16, ByRef frm As frmMain, ByVal sSpecName As String)
Dim sSpecDir	As String = Nothing
Dim s			As New ArrayList
	Select Case iT
	Case eSpec.Device
		sSpecDir = Path.Combine(SIZUKA_PATH, p_sEquipName, DEVSPEC_PATH)
		CSVToDataTable(Path.Combine(sSpecDir, sSpecName & ".csv"), s )	'SPECをCSVファイルから読込み
		SpecItemDisp(iT, sSpecName, frm.dgvdevSpec, s)					'SPECをデータグリッドビューへ表示
		dgvDevSetting(frm.dgvDevSpec, Not p_cts.AdminEnable)			'SPECをデータグリッドビュー動作設定
		InputRangeSet(iT, frm.dgvDevSpec)								'SPECへ数値入力範囲を設定する
		dgvToSpi(iT, frm.dgvDevSpec, p_clSpec.spi)						'現在のDGV値をSPI構造体へセット

	Case eSpec.Sample
		sSpecDir = Path.Combine(SIZUKA_PATH, p_sEquipName, SAMSPEC_PATH)
		CSVToDataTable(Path.Combine(sSpecDir, sSpecName & ".csv"), s )	'SPECをCSVファイルから読込み
		SpecItemDisp(iT, sSpecName, frm.dgvSamSpec, s)					'SPECをデータグリッドビューへ表示
		dgvSamSetting(frm.dgvSamSpec, Not p_cts.AdminEnable)			'SPECをデータグリッドビュー動作設定
		InputRangeSet(iT, frm.dgvSamSpec)								'SPECへ数値入力範囲を設定する
		dgvToSpi(iT, frm.dgvSamSpec, p_clSpec.spi)						'現在のDGV値をSPI構造体へセット

	End Select
End Sub

''' <summary>
''' SPEC データグリッドビュー表示内容をファイルへ保存 
''' </summary>
''' <param name="iT"></param>
''' <param name="frm"></param>
''' <param name="sSpecName"></param>
Public Sub specWrite(ByVal iT As int16, ByRef frm As frmMain, ByVal sSpecName As String)
Dim sSpecDir	As String = Nothing
Dim s			As New ArrayList
	Select Case iT
	Case eSpec.Device
		sSpecDir = Path.Combine(SIZUKA_PATH, p_sEquipName, DEVSPEC_PATH)
	Case eSpec.Sample
		sSpecDir = Path.Combine(SIZUKA_PATH, p_sEquipName, SAMSPEC_PATH)
	End Select
	CnvDataTableToAryList(iT, s)										'SPEC用データテーブルをCSVファイルIF用のアレイリストへ変換
	CnvAryListToCSV( Path.Combine(sSpecDir, sSpecName & ".csv"), s )	'SPEC CSVへのアレイリストの書込み
End Sub

''' <summary>
''' 測定結果ファイルのヘッダ部をDGVへ表示するときに使用
''' サンプルSPECの形状に従って項目名を変える
''' </summary>
''' <param name="iSh"></param>
''' <param name="sItem"></param>
Public Sub	CondDispRowsAddForShape( ByVal iSh As Int16, ByVal sHeadItem() As String, Byref sItem() As String )
	Select Case iSh
	Case eShape.Sheet
		sItem(0) = sHeadItem(eHead.Thickness)
		sItem(1) = sHeadItem(eHead.Stacks)

	Case eShape.Powder
		sItem(0) = sHeadItem(eHead.SpecificGravity)
		sItem(1) = sHeadItem(eHead.Weight)

	Case eShape.Liquid
		sItem(0) = sHeadItem(eHead.SpecificGravity)
		sItem(1) = sHeadItem(eHead.Volume)

	End Select

End Sub

''' <summary>
''' 現在のSPEC DGVからSPI構造体へセットする
''' </summary>
''' <param name="spi"></param>
Public Sub dgvToSpi(ByVal iT As Int32, ByRef dgv As DataGridView, ByRef spi As SpecInfo)
Dim s() As String = Nothing
Try
	Select Case iT
	Case eSpec.Device
		spi.iHPPosCont			= Array.IndexOf(sHCont, dgv(2, eDevSPEC.HP_CONT).Value)					'熱板位置制御		0:GAP 1:着圧指定 2:常時圧力
		spi.iGelTimeMethod		= Array.IndexOf(lstCalcMethod.ToArray, dgv(2, eDevSPEC.METHOD).Value)	'ゲルタイム判定方法 0:THR 1:SLOPE 2:EXTRAPOLATION 3:PHASE 4:MELT
		spi.dThreshold			= dgv(2, eDevSPEC.THRETHOLD).Value										'判定値 [%]
		s = dgv(2, eDevSPEC.TEMPERATURE).Value.Split("/")
		spi.dHPSetTemp			= Ctype(Trim(s(0)), Double)												'熱板温度設定 [℃]
		spi.dPBSetTemp			= Ctype(Trim(s(1)), Double)												'プローブ温度設定 [℃]
		'spi.dSampleGap			= dgv(2, eDevSPEC.SAMPLE_GAP).Value										'サンプル投入GAP [mm]
		spi.dMeasureGap			= dgv(2, eDevSPEC.MEAS_GAP).Value										'測定中GAP [mm]
		spi.dMeasureStress		= dgv(2, eDevSPEC.MEAS_STRESS).Value									'測定中圧力 [kPa]
		spi.iPressContStep		= dgv(2, eDevSPEC.PRESSCONT_STEP).Value									'着圧制御ステップ数
		spi.dProbDiameter		= dgv(2, eDevSPEC.PB_DIAMETER).Value									'プローブ直径 [mm]
		spi.iStressSensMode		= Array.IndexOf(sStrsSens, dgv(2, eDevSPEC.STRS_SENS_MODE).Value)		'応力検知モード				1:線 0:円 2:点
		spi.iAmpMode			= Array.IndexOf(sAmpMode, dgv(2, eDevSPEC.AMP_MODE).Value)				'振幅測定モード				0:振幅一定 1:応力一定
		'spi.iProbInputMode		= Array.IndexOf(sProbInpMethod, dgv(2, eDevSPEC.PB_INP_MODE).Value)		'プローブ動作設定の入力方式	0:直接 1:間接
		Dim dA	As Double = dgv(2, eDevSPEC.AMPLITUDE).Value
		Dim dP	As Double = dgv(2, eDevSPEC.PERIOD).Value
		Dim dSP As Double = dgv(2, eDevSPEC.AMPLITUDE).Value
		Dim dST As Double = dgv(2, eDevSPEC.PERIOD).Value
		'Select Case spi.iProbInputMode
		'Case ePBInpMode.Direct
			spi.dSampleStrain		= CalcStrainFromAmpGap(dA, spi.dMeasureGap)					'サンプル歪 [L/t]
			spi.dShearSpeed			= CalcShearSpdFromStrainPrd(spi.dSampleStrain, dP)			'せん断速度 [1/s]
		'Case ePBInpMode.Indirect
		'	dA = CalcAmpFromStrainGap(dSt, spi.dMeasureGap)
		'	dP = CalcPrdFromStrainShearSpd(dST, dSP)
		'	spi.dShearSpeed			= dSP														'せん断速度 [1/s]
		'	spi.dSampleStrain		= dST														'サンプル歪 [L/t]
		'End Select
		Select Case spi.iStressSensMode
		Case eStrsSens.Line
			spi.dXAmplitude			= dA			'振幅X [mm]
			spi.dYAmplitude			= 0				'振幅Y [mm]
			spi.dZAmplitude			= 0				'振幅Z [mm]
			spi.dXPeriod			= dP			'周期X [秒]
			spi.dYPeriod			= 1				'周期Y [秒]
			spi.dZPeriod			= 1				'周期Z [秒]
		Case eStrsSens.Circle
			spi.dXAmplitude			= dA			'振幅X [mm]
			spi.dYAmplitude			= dA			'振幅Y [mm]
			spi.dZAmplitude			= 0				'振幅Z [mm]
			spi.dXPeriod			= dP			'周期X [秒]
			spi.dYPeriod			= dP			'周期Y [秒]
			spi.dZPeriod			= 1				'周期Z [秒]
		Case eStrsSens.Point
			spi.dXAmplitude			= 0				'振幅X [mm]
			spi.dYAmplitude			= 0				'振幅Y [mm]
			spi.dZAmplitude			= dA			'振幅Z [mm]
			spi.dXPeriod			= 1				'周期X [秒]
			spi.dYPeriod			= 1				'周期Y [秒]
			spi.dZPeriod			= dP			'周期Z [秒]
		End Select

		spi.dAmpModeStress		= dgv(2, eDevSPEC.AMPMODE_STRESS).Value					'上限応力[uN] 振幅測定モード 
		spi.dInputToHPUpWait	= dgv(2, eDevSPEC.HP_UP_WAIT).Value						'サンプル投入から熱板上昇開始までの時間 [秒]
		spi.dHPUpToXYActWait	= dgv(2, eDevSPEC.ACTION_WAIT).Value					'熱板上昇開始からXY動作開始までの時間 [秒]
		spi.dMeasureTime		= dgv(2, eDevSPEC.MEAS_TIME).Value						'測定時間設定 [秒]
		spi.dMeasureEndStress	= dgv(2, eDevSPEC.MEAS_END_STRESS).Value				'測定終了応力 [%]
		spi.sTempProfileName	= dgv(2, eDevSPEC.TEMP_PROFILE).Value					'温度プロファイル名 空:使用しない
		spi.dDiffInterval		= dgv(2, eDevSPEC.DIFF_INTERVAL).Value					'微分区間(SLOPE) [秒]


	Case eSpec.Sample
		spi.sSampleName			= dgv(2, eSamSPEC.SAMPLE).Value							'サンプル名
		spi.sLot				= dgv(2, eSamSPEC.LOT).Value							'ロットNo
		spi.sMeasurer			= dgv(2, eSamSPEC.MEASURER).Value						'測定者
		spi.iShape				= Array.IndexOf(sShape, dgv(2, eSamSPEC.SHAPE).Value)	'サンプル形状 0:シート 1:粉 2:液 3:RCC
		spi.sThickness			= dgv(2, eSamSPEC.THICKNESS).Value						'サンプル厚み [mm]/密度[g/cm3]
		spi.dStack				= dgv(2, eSamSPEC.STACKS).Value							'積層枚数/重量[g]/体積[cm3]
		s = dgv(2, eSamSPEC.EFECT_RANGE).Value.Split("-")
		spi.dEffectRangeMin		= Ctype(Trim(s(0)), Double)								'データ有効範囲 最小 [秒]
		spi.dEffectRangeMax		= Ctype(Trim(s(1)), Double)								'データ有効範囲 最大 [秒]
		s = dgv(2, eSamSPEC.JUDGE_RANGE).Value.Split("-")
		spi.dJudgeRangeMin		= Ctype(Trim(s(0)), Double)								'GT判定範囲 最小 [秒]
		spi.dJudgeRangeMax		= Ctype(Trim(s(1)), Double)								'GT判定範囲 最大 [秒]

	End Select
	spi.dSampleInputWait	= p_cts.SampleInputWait										'サンプル投入待ち時間 [秒]		Optionから
	spi.dSampleGap			= p_cts.InputGap											'投入GAP [mm]					Optionから
	spi.dHPTempOkRange		= p_cts.HPTempRange											'熱板温度許容範囲 [℃]			Optionから
	spi.dPBTempOkRange		= p_cts.PBTempRange											'プローブ温度許容範囲 [℃]		Optionから
Catch ex As Exception
    Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
    Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub


''' <summary>
''' 数値項目の入力範囲を行単位でToolTipTextへセットする
''' </summary>
''' <param name="dgv">対象データグリッドビュー</param>
''' <param name="iRow">行インデックス</param>
''' <param name="sRange">設定文字列</param>
Private Sub SetToolTipToDgvRow(ByVal iT As Int16, ByRef dgv As DataGridView, ByVal iRow As Int16, ByVal sRange As String)
	Dim i As int16
	For i=1 To dgv.Columns.Count - 1
		dgv(i, iRow).ToolTipText = sRange
	Next
	Select Case iT
	Case eSpec.Device
		If iRow = eDevSPEC.MEAS_TIME AndAlso Not p_cts.LTModeEnable
			Dim iClm As Int32 = sDevItemHint(iRow).IndexOf(vbCrLf)
			dgv(0, iRow).ToolTipText = sDevItemHint(iRow).Substring(0,iClm)  '測定終了時間だけ切り出す
		Else
			dgv(0, iRow).ToolTipText = sDevItemHint(iRow)
		End If
	Case eSpec.Sample
		dgv(0, iRow).ToolTipText = sSamItemHint(iRow)
	End Select
End Sub 

''' <summary>
''' ヘッダ情報をSPEC入力領域へ転送
''' </summary>
Public Sub HeaderToSpecSpi(ByVal hd As DataFile.DataFileHeader)
Dim s() As String = Nothing
Try
		If IsNothing(hd.sFileName)
			Exit Sub
		End If
		spi.iHPPosCont			= hd.sHPPosControl											'熱板位置制御		0:GAP 1:着圧指定 2:常時圧力
		spi.iGelTimeMethod		= Array.IndexOf(lstCalcMethod.ToArray, hd.sCalcMethod)		'ゲルタイム判定方法 0:THR 1:SLOPE 2:EXTRAPOLATION 3:PHASE 4:MELT
		spi.dThreshold			= Ctype(hd.sThreshold, Double)								'判定値 [%]
		spi.dHPSetTemp			= hd.dHPTemp												'熱板温度設定 [℃]
		spi.dPBSetTemp			= hd.dPBTemp												'プローブ温度設定 [℃]
		spi.dSampleGap			= hd.dInputGap												'サンプル投入GAP [mm]
		spi.dMeasureGap			= hd.dGapValue												'測定中GAP [mm]
		spi.dMeasureStress		= hd.dMeasurePress											'測定中圧力 [kPa]
		spi.iPressContStep		= hd.iPressContStep											'着圧制御ステップ数
		spi.dProbDiameter		= hd.dPBDiameter											'プローブ直径 [mm]
		spi.iStressSensMode		= hd.sStrsDetectMode										'応力検知モード				1:線 0:円 2:点
		spi.iAmpMode			= hd.sAmpMode												'振幅測定モード				0:振幅一定 1:応力一定
		'spi.iProbInputMode		= 0															'プローブ動作設定の入力方式	0:直接 1:間接
		Dim dA	As Double = hd.dXAmp
		Dim dP	As Double = hd.dXPrd
		Dim dSP As Double = hd.dXAmp
		Dim dST As Double = hd.dXPrd
		'Select Case spi.iProbInputMode
		'Case ePBInpMode.Direct
			spi.dSampleStrain		= CalcStrainFromAmpGap(dA, spi.dMeasureGap)					'サンプル歪 [L/t]
			spi.dShearSpeed			= CalcShearSpdFromStrainPrd(spi.dSampleStrain, dP)			'せん断速度 [1/s]
		'Case ePBInpMode.Indirect
		'	dA = CalcAmpFromStrainGap(dSt, spi.dMeasureGap)
		'	dP = CalcPrdFromStrainShearSpd(dST, dSP)
		'	spi.dShearSpeed			= dSP														'せん断速度 [1/s]
		'	spi.dSampleStrain		= dST														'サンプル歪 [L/t]
		'End Select
		Select Case spi.iStressSensMode
		Case eStrsSens.Line
			spi.dXAmplitude			= hd.dXAmp			'振幅X [mm]
			spi.dYAmplitude			= 0					'振幅Y [mm]
			spi.dZAmplitude			= 0					'振幅Z [mm]
			spi.dXPeriod			= hd.dXPrd			'周期X [秒]
			spi.dYPeriod			= 1					'周期Y [秒]
			spi.dZPeriod			= 1					'周期Z [秒]
		Case eStrsSens.Circle
			spi.dXAmplitude			= hd.dXAmp			'振幅X [mm]
			spi.dYAmplitude			= hd.dYAmp			'振幅Y [mm]
			spi.dZAmplitude			= 0					'振幅Z [mm]
			spi.dXPeriod			= hd.dXPrd			'周期X [秒]
			spi.dYPeriod			= hd.dYPrd			'周期Y [秒]
			spi.dZPeriod			= 1					'周期Z [秒]
		Case eStrsSens.Point
			spi.dXAmplitude			= 0					'振幅X [mm]
			spi.dYAmplitude			= 0					'振幅Y [mm]
			spi.dZAmplitude			= hd.dZAmp			'振幅Z [mm]
			spi.dXPeriod			= 1					'周期X [秒]
			spi.dYPeriod			= 1					'周期Y [秒]
			spi.dZPeriod			= hd.dZPrd			'周期Z [秒]
		End Select

		spi.dAmpModeStress		= hd.dAmpModeStress										'上限応力[uN] 振幅測定モード 
		spi.dInputToHPUpWait	= hd.dHPLiftWait										'サンプル投入から熱板上昇開始までの時間 [秒]
		spi.dHPUpToXYActWait	= hd.dXYActWait											'熱板上昇開始からXY動作開始までの時間 [秒]
		spi.dMeasureTime		= hd.dMeasTimeLimit										'測定時間設定 [秒]
		spi.dMeasureEndStress	= hd.dMeasEndStress										'測定終了応力 [%]
		
		'温度プロファイルファイルチェック ファイルが存在しないならSPEC表示を空欄にする
		Dim uRtg		As RtgSetF = Nothing
		Dim bZPosEnable As Boolean
		Dim bRet		As Boolean
		bRet = frmOptions.LoadRTGSET(hd.sTempProfile, p_sEquipName, uRtg, bZPosEnable) 
		If bRet
			spi.sTempProfileName	= hd.sTempProfile									'温度プロファイル名 空:使用しない
		Else
			spi.sTempProfileName	= ""												'温度プロファイル名 空:使用しない
		End If
		spi.dDiffInterval		= hd.dDiffInterval										'微分区間(SLOPE) [秒]


		spi.sSampleName			= hd.sSampleName										'サンプル名
		spi.sLot				= hd.sLot												'ロットNo
		spi.sMeasurer			= hd.sMeasurer											'測定者
		spi.iShape				= hd.sShape												'サンプル形状 0:シート 1:粉 2:液 3:RCC
		spi.sThickness			= hd.sThickness											'サンプル厚み [mm]/密度[g/cm3]
		spi.dStack				= hd.dStacks											'積層枚数/重量[g]/体積[cm3]
		s = hd.sMaskTime.Split("-")
		spi.dEffectRangeMin		= Ctype(Trim(s(0)), Double)								'データ有効範囲 最小 [秒]
		spi.dEffectRangeMax		= Ctype(Trim(s(1)), Double)								'データ有効範囲 最大 [秒]
		s = hd.sJudgeRange.Split("-")
		spi.dJudgeRangeMin		= Ctype(Trim(s(0)), Double)								'GT判定範囲 最小 [秒]
		spi.dJudgeRangeMax		= Ctype(Trim(s(1)), Double)								'GT判定範囲 最大 [秒]

	spi.dSampleInputWait	= hd.dInputWait												'サンプル投入待ち時間 [秒]
	spi.dSampleGap			= hd.dInputGap												'投入GAP [mm]
	spi.dHPTempOkRange		= hd.sHPTempFlucRange										'熱板温度許容範囲 [℃]		
	spi.dPBTempOkRange		= hd.sPBTempFlucRange										'プローブ温度許容範囲 [℃]	
Catch ex As Exception
    Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
    Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try

End Sub
''' <summary>
''' スペック入力領域からSPEC入力DGVへセットし表示更新
''' </summary>
''' <param name="frm"></param>
Public Sub SpecSpiToDgvUpdate(ByRef frm As frmMain)
Try
	With frm.dgvDevSpec
	.Rows(eDevSPEC.HP_CONT).Cells(2).Value				= sHCont(p_clSpec.spi.iHPPosCont)
	.Rows(eDevSPEC.METHOD).Cells(2).Value				= lstCalcMethod(p_clSpec.spi.iGelTimeMethod)
	.Rows(eDevSPEC.THRETHOLD).Cells(2).Value			= p_clSpec.spi.dThreshold.ToString("F1")
	.Rows(eDevSPEC.TEMPERATURE).Cells(2).Value			= p_clSpec.spi.dHPSetTemp.ToString("F1") & "/" & p_clSpec.spi.dPBSetTemp.ToString("F1")
	.Rows(eDevSPEC.MEAS_GAP).Cells(2).Value				= p_clSpec.spi.dMeasureGap
	.Rows(eDevSPEC.MEAS_STRESS).Cells(2).Value			= p_clSpec.spi.dMeasureStress
	.Rows(eDevSPEC.PRESSCONT_STEP).Cells(2).Value		= p_clSpec.spi.iPressContStep
	.Rows(eDevSPEC.PB_DIAMETER).Cells(2).Value			= p_clSpec.spi.dProbDiameter
	.Rows(eDevSPEC.STRS_SENS_MODE).Cells(2).Value		= sStrsSens(p_clSpec.spi.iStressSensMode)
	.Rows(eDevSPEC.AMP_MODE).Cells(2).Value				= sAmpMode(p_clSpec.spi.iAmpMode)
	Select Case spi.iStressSensMode
	Case eStrsSens.Line
		.Rows(eDevSPEC.AMPLITUDE).Cells(2).Value			= p_clSpec.spi.dXAmplitude.ToString("F3")
		.Rows(eDevSPEC.PERIOD).Cells(2).Value				= p_clSpec.spi.dXPeriod.ToString("F3")
	Case eStrsSens.Circle
		.Rows(eDevSPEC.AMPLITUDE).Cells(2).Value			= p_clSpec.spi.dXAmplitude.ToString("F3")
		.Rows(eDevSPEC.PERIOD).Cells(2).Value				= p_clSpec.spi.dXPeriod.ToString("F3")
	Case eStrsSens.Point
		.Rows(eDevSPEC.AMPLITUDE).Cells(2).Value			= p_clSpec.spi.dZAmplitude.ToString("F3")
		.Rows(eDevSPEC.PERIOD).Cells(2).Value				= p_clSpec.spi.dZPeriod.ToString("F3")
	End Select
	.Rows(eDevSPEC.AMPMODE_STRESS).Cells(2).Value		= p_clSpec.spi.dAmpModeStress
	.Rows(eDevSPEC.HP_UP_WAIT).Cells(2).Value			= p_clSpec.spi.dInputToHPUpWait.ToString("F1")
	.Rows(eDevSPEC.ACTION_WAIT).Cells(2).Value			= p_clSpec.spi.dHPUpToXYActWait.ToString("F1")
	.Rows(eDevSPEC.MEAS_TIME).Cells(2).Value			= p_clSpec.spi.dMeasureTime.ToString("F1")
	.Rows(eDevSPEC.MEAS_END_STRESS).Cells(2).Value		= p_clSpec.spi.dMeasureEndStress.ToString("F1")
	.Rows(eDevSPEC.TEMP_PROFILE).Cells(2).Value			= p_clSpec.spi.sTempProfileName
	.Rows(eDevSPEC.DIFF_INTERVAL).Cells(2).Value		= p_clSpec.spi.dDiffInterval.ToString("F1")
	End With

	With frm.dgvSamSpec
	.Rows(eSamSPEC.SAMPLE).Cells(2).Value				= spi.sSampleName		'サンプル名
	.Rows(eSamSPEC.LOT).Cells(2).Value					= spi.sLot				'ロットNo
	.Rows(eSamSPEC.MEASURER).Cells(2).Value				= spi.sMeasurer			'測定者
	IF spi.iShape=-1
		.Rows(eSamSPEC.SHAPE).Cells(2).Value			= sShape(0)
	Else
		.Rows(eSamSPEC.SHAPE).Cells(2).Value			= sShape(spi.iShape)	'サンプル形状 0:シート 1:粉 2:液
	End If
	.Rows(eSamSPEC.THICKNESS).Cells(2).Value			= spi.sThickness		'サンプル厚み [mm]/密度[g/cm3]
	.Rows(eSamSPEC.STACKS).Cells(2).Value				= spi.dStack			'積層枚数/重量[g]/体積[cm3]
	.Rows(eSamSPEC.EFECT_RANGE).Cells(2).Value			= spi.dEffectRangeMin.ToString("F1") & "-" & spi.dEffectRangeMax.ToString("F1")
	.Rows(eSamSPEC.JUDGE_RANGE).Cells(2).Value			= spi.dJudgeRangeMin.ToString("F1") & "-" & spi.dJudgeRangeMax.ToString("F1")
	End With

Catch ex As Exception
    Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
    Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub

''' <summary>エクスポート時の変更ヘッダ項目をセット</summary>
Public Sub spiSpecToPHead()
Try
	With p_dtFile
	.p_head.sDeviceSpec				= p_clSpec.spi.sDeviceSpecName						'デバイスSPEC名
	.p_head.sCalcMethod				= lstCalcMethod(p_clSpec.spi.iGelTimeMethod)		'ゲルタイム計算方法
	.p_head.sThreshold				= p_clSpec.spi.dThreshold.ToString("F1")			'応力判定値
	.p_head.sSampleSpec				= p_clSpec.spi.sSampleSpecName						'サンプルSPEC名
	.p_head.sSampleName				= p_clSpec.spi.sSampleName							'サンプル名
	.p_head.sLot					= p_clSpec.spi.sLot									'ロット
	.p_head.sMeasurer				= p_clSpec.spi.sMeasurer							'測定者
	.p_head.sShape					= p_clSpec.sShape(p_clSpec.spi.iShape)				'形状
	.p_head.sThickness				= p_clSpec.spi.sThickness							'厚み/密度
	.p_head.dStacks					= p_clSpec.spi.dStack								'積層数/重量/体積
	.p_head.sMaskTime				= p_clSpec.spi.dEffectRangeMin.ToString("F1") & " - " & p_clSpec.spi.dEffectRangeMax.ToString("F1")	'マスク範囲
	.p_head.sJudgeRange				= p_clSpec.spi.dJudgeRangeMin.ToString("F1") & " - " & p_clSpec.spi.dJudgeRangeMax.ToString("F1")	'判定範囲


	'エクスポート時に変更しないヘッダ項目
#If 0
	.p_head.sHPPosControl			= p_clSpec.sHCont(p_clSpec.spi.iHPPosCont)			'熱板位置制御
	.p_head.dHPTemp					= p_clSpec.spi.dHPSetTemp							'熱板温度設定(℃)
	.p_head.dPBTemp					= p_clSpec.spi.dPBSetTemp							'プローブ温度設定(℃)
	.p_head.dInputGap				= p_clSpec.spi.dSampleGap							'投入GAP
	.p_head.dGapValue				= p_clSpec.spi.dMeasureGap							'測定GAP
	.p_head.dMeasurePress			= p_clSpec.spi.dMeasureStress						'測定圧力設定
	.p_head.iPressContStep			= p_clSpec.spi.iPressContStep						'圧力制御STEP
	.p_head.iPressContDelay			= p_cts.PressContDelayTime							'圧力制御Delay time
	.p_head.dPBDiameter				= p_clSpec.spi.dProbDiameter						'プローブ直径
	.p_head.sStrsDetectMode			= p_clSpec.sStrsSens(p_clSpec.spi.iStressSensMode)	'応力検知モード
	Select Case p_clSpec.spi.iStressSensMode
	Case eStrsSens.Line
		.p_head.dXAmp					= p_clSpec.spi.dXAmplitude						'X軸振幅(mm)
		.p_head.dYAmp					= 0												'Y軸振幅(mm)
		.p_head.dZAmp					= 0												'Z軸振幅(mm)
		.p_head.dXPrd					= p_clSpec.spi.dXPeriod							'X軸周期(秒)
		.p_head.dYPrd					= 1												'Y軸周期(秒)
		.p_head.dZPrd					= 1												'Z軸周期(秒)
	Case eStrsSens.Circle
		.p_head.dXAmp					= p_clSpec.spi.dXAmplitude						'X軸振幅(mm)
		.p_head.dYAmp					= p_clSpec.spi.dYAmplitude						'Y軸振幅(mm)
		.p_head.dZAmp					= 0												'Z軸振幅(mm)
		.p_head.dXPrd					= p_clSpec.spi.dXPeriod							'X軸周期(秒)
		.p_head.dYPrd					= p_clSpec.spi.dYPeriod							'Y軸周期(秒)
		.p_head.dZPrd					= 1												'Z軸周期(秒)
	Case eStrsSens.Point
		.p_head.dXAmp					= 0												'X軸振幅(mm)
		.p_head.dYAmp					= 0												'Y軸振幅(mm)
		.p_head.dZAmp					= p_clSpec.spi.dZAmplitude						'Z軸振幅(mm)
		.p_head.dXPrd					= 1												'X軸周期(秒)
		.p_head.dYPrd					= 1												'Y軸周期(秒)
		.p_head.dZPrd					= p_clSpec.spi.dZPeriod							'Z軸周期(秒)
	End Select
	.p_head.dZZeroPosition			= p_iZPosition										'熱板位置(um)
	.p_head.dZNoLoadStress			= p_mltcp.ZeroStrs_Z								'無負荷Z応力(補正後)
	.p_head.dAValue					= p_mltcp.ZStrsCorrFactA							'係数A
	.p_head.dBValue					= p_mltcp.ZStrsCorrFactB							'係数B
	.p_head.dHPLiftWait				= p_clSpec.spi.dInputToHPUpWait						'投入から熱板上昇までの待ち時間
	.p_head.dInputWait				= p_clSpec.spi.dSampleInputWait						'投入待ち時間
	.p_head.dXYActWait				= p_clSpec.spi.dHPUpToXYActWait						'XY動作待ち時間
	.p_head.dMeasTimeLimit			= p_clSpec.spi.dMeasureTime							'測定時間リミット
	.p_head.dMeasEndStress			= p_clSpec.spi.dMeasureEndStress					'測定終了応力
	.p_head.sTempProfile			= p_clSpec.spi.sTempProfileName						'温度プロファイル名

	.p_head.dMeasEndStress		= p_dStressLimit									'測定終了応力
	.p_head.dMeasTimeLimit		= p_dMeasTimeLimit									'測定時間リミット
	.p_head.sTempProfile		= p_clSpec.spi.sTempProfileName						'温度プロファイル名

	.p_head.dGelTime			= -1.0												'ゲルタイム
	.p_head.sGelTimeJudge		= "NA"												'ゲルタイム判定
#end if

	End With
Catch ex As Exception
    Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
    Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub
''' <summary>
''' 時間範囲入力処理
''' </summary>
''' <param name="sender"></param>
''' <param name="iRowIndex"></param>
''' <returns></returns>
Public Function CheckSpecTimeSpanInput(ByVal sender As Object, ByVal iRowIndex As Int16, ByVal bWind As Boolean) As Int32
Dim dgvTemp As DataGridView
Dim iRet	As Int32
Try
	dgvTemp = CType(sender, DataGridView)
	Select Case iRowIndex
	Case eSamSPEC.JUDGE_RANGE, eSamSPEC.EFECT_RANGE
		'入力ダイアログを表示
		Dim strApprox() As String = dgvTemp.Item(2, iRowIndex).Value.ToString.Split("-")
		Dim sRange()	As String = Nothing
		sRange = p_clSpec.sSamInputRange_AppStdTime(If(p_cts.LTModeEnable,1,0)).Split("-")
		Dim adlg As frmSetRange = New frmSetRange(strApprox(0), strApprox(1), 1)
		If bWind
			iRet = adlg.ShowDialog()
		Else
			iRet = Windows.Forms.DialogResult.OK
		End If
		If iRet = Windows.Forms.DialogResult.OK Then
			Dim dMin As Double
			Dim dMax As Double
			dMin = adlg.GetMin()
			dMax = adlg.GetMax()
			If dMin < 0.0R Then
				dMin = 0.0R
			End If
			If spi.dMeasureTime <> 0
				'マスクレンジの最大値は測定終了時間より大きい値は不可
				If dMax > spi.dMeasureTime
					dMax = spi.dMeasureTime
				End If
			End If
			If dMax > sRange(1) Or dMin < sRange(0)
			'範囲チェックNG
				dgvTemp(2, iRowIndex).Style.BackColor = Color.Yellow		'エラーセル背景色
				UnderStatusLabelDisp( frmMain.ToolStripStatusLabel1, sUnderMsg1(1), Color.Orange)					'範囲外
				iRet = -2
			ElseIf dMin > dMax
			'最大最小の大小関係
				dgvTemp(2, iRowIndex).Style.BackColor = Color.Yellow		'エラーセル背景色
				UnderStatusLabelDisp( frmMain.ToolStripStatusLabel1, sUnderMsg1(1), Color.Orange)					'範囲外
				iRet = -3
			Else
			'チェックOK
				dgvTemp(2, iRowIndex).Style.BackColor = dgvTemp.DefaultCellStyle.BackColor							'セル背景色
				UnderStatusLabelDisp( frmMain.ToolStripStatusLabel1, "")											'範囲外クリア
				iRet = 0
			End If
			dgvTemp.Item(2, iRowIndex).Value = String.Format("{0:0.0}", dMin) + " - " + String.Format("{0:0.0}", dMax)
			Select Case iRowIndex
			Case eSamSPEC.JUDGE_RANGE
				If p_clSpec.spi.dJudgeRangeMin <> dMin
					p_clSpec.spi.dJudgeRangeMin = dMin
					p_RTM(0).p_dJudgeRangeMin	= dMin
					'p_dtFile.p_head.iState		= 0			'測定データ状態のクリア
					frmMain.samSpecToFree()			'Freeモードへ
				End If
				If p_clSpec.spi.dJudgeRangeMax <> dMax
					p_clSpec.spi.dJudgeRangeMax = dMax
					p_RTM(0).p_dJudgeRangeMax	= dMax
					'p_dtFile.p_head.iState		= 0			'測定データ状態のクリア
					frmMain.samSpecToFree()			'Freeモードへ
				End If

			Case eSamSPEC.EFECT_RANGE
				If p_clSpec.spi.dEffectRangeMin <> dMin
					p_clSpec.spi.dEffectRangeMin = dMin
					p_RTM(0).p_dAprxRangeMin	 = dMin
					'p_dtFile.p_head.iState		= 0			'測定データ状態のクリア
					frmMain.samSpecToFree()			'Freeモードへ
				End If
				If p_clSpec.spi.dEffectRangeMax <> dMax
					p_clSpec.spi.dEffectRangeMax = dMax
					p_RTM(0).p_dAprxRangeMax	 = dMax
					'p_dtFile.p_head.iState		= 0			'測定データ状態のクリア
					frmMain.samSpecToFree()			'Freeモードへ
				End If
			End Select
			'チャート更新 ゲルタイム判定更新
			'ChartDispUpdate(Me, 0, False)
			'iRet = p_RTM(0).GelTimeSeach(p_iSelectGItem)			'ゲルタイム再検索
			'ResearchGelTimeOnChart()								'ゲルタイム表示更新
			'GelTimeJudgeColorUpdate(Me)								'ゲルタイム判定表示
			'GelTimeLabelDispUpdate(Me)								'ゲルタイム値更新
			'ChartDispUpdate(Me, 0, False)

			adlg.Dispose()
		End If
	End Select
	Return 0
Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Return -1
End Try
End Function

End Class

