'============================================
'File:   DataFile.vb
'Name:   測定データファイル制御クラス
'Desc:   測定データのアクセス管理を行う
'Author: Hosokawa
'Date:   2020/01/28
'============================================
Imports System.IO
Imports System.Runtime.InteropServices
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.Security.Cryptography
Imports System.Text
Imports System.Threading
Imports Microsoft.Office.Interop
Imports SZKTool.clSpec
Imports SZKTool.Filter
Imports SZKTool.frmMain
Imports SZKTool.frmOptions

Public Class DataFile 
inherits DataBuf

'▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼
'   定数
'▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲
#Const PreFilter = 0
Public Const cGraviAcc	As Double	= 9.80665						'重力加速度(m/s2)
'Public Const cHistMax	As Int32	= 5								'履歴バッファ数(1-5)
Private const sComnt	As String	= "'"							'コメント行
'ヘッダ項目順番
#If 1
Public enum eHead
'1
	Section1			= 0
	SampleSpec				
	SampleName 
	Lot 
	Measurer				
	Shape
	Thickness				
	Stacks
	SpecificGravity
	Weight
	Volume
	MaskTime
	JudgeRange
'2
	Section2
	DeviceSpec			
	HPPosControl
	HPTemprature		
	PBTemprature 
	SGTemp
	HPTempFlucRange
	PBTempFlucRange
	TempProfile
	InputGap
	GapValue
	MeasurePresure
	PressContStep
	PressContDelay
	PBDiameter
	StressDetectMode
	AmplitudeMode
	LimitStress
	XAmplitude			
	XPeriod				
	YAmplitude
	YPeriod
	ZAmplitude
	ZPeriod
	InputWait			
	HPLiftWait
	XYActWait
	MeasTimeLimit
	MeasEndStress
'3
	Section3
	FileName
	DateTime
	MeasureType
	CalculationMethod
	StressThreshold
	SLOPEDiffInterval
	GelTime					
	GelTimeJudge
	GelTimeTemp
	GelTimeVisco
	Filter			
	OOSValX							'脱調確認結果 X (um)
	OOSValY							'脱調確認結果 Y (um)
	OOSValZ							'脱調確認結果 Z (um)
	CPUCommErrCnt					'CPU間通信エラーカウント
	ActMeasTime						'実測定時間[秒]
	AfterZPos						'着圧決定Z位置[um]
	RealGap							'実測定GAP[mm]
'4
	Section4
	ToolVersion			
	EquipVersion
	EquipID 
	XYSensID 
	ZSensID 

	IntegTimePowOn					'本体ON積算時間[秒]
	IntegTimeHeatOn					'HP ON積算時間[秒]
	IntegTimePBHeat					'PB ON積算時間[秒]
	IntegTimeSGHeat					'SG ON積算時間[秒]
	IntegTimeXAct					'X軸動作 ON積算時間[秒]
	IntegTimeYAct					'Y軸動作 ON積算時間[秒]
	IntegTimeZAct					'Z軸動作 ON積算時間[秒]
'5
	Section5
	XYWeightCoef			
	ZWeightCoef
	FullCalibrationVlue		
	ZeroCalibrationVlue
	FullStressAttenuation
	FullStressPxPhase		
	FullStressPyPhase
	FullStressPzPhase
	GAPCORR
	NoLoadZStress
	ZSTAB
	ZeroAdjStabMonTime
	ZeroAdjFlucRange
	ZZeroPosition
	HPCenterX
	HPCenterY

	TempCnt_P
	TempCnt_I
	TempCnt_D
	TempCnt_Dgain
	TempCnt_PVBias
	TempCnt_PVGain

	TempCnt_P2
	TempCnt_I2
	TempCnt_D2
	TempCnt_Dgain2
	TempCnt_PVBias2
	TempCnt_PVGain2

	TempCnt_P3
	TempCnt_I3
	TempCnt_D3
	TempCnt_Dgain3
	TempCnt_PVBias3
	TempCnt_PVGain3




'7
	Section7
	ExtStartTime			
	ExtMiddleTime	
	ExtEndTime		
	ExtTanSample			
	ExtTanThinning	
	ExtGelTimeSelect	
	ExtGelTimeUnder	
	ExtGelTimeMiddle	
	ExtGelTimeOver	
'8
	Section8				
	MELTStartTemp					'MELT法 開始温度(℃)
	MELTEndTemp						'MELT法 終了温度(℃)
	MELTWaitTime					'MELT法 待ち時間(Sec)
	MELTSpeed						'MELT法 昇温速度(℃/min)
	MELTTemperature					'MELT法 結果温度(℃)
	MELTViscosity					'MELT法 結果粘度(PaS)
'9
	Section9				
	FLCTemp			 				'FLC 設定温度(℃)
	ProbeDiameter		 			'FLC プローブ直径(mm)
	FLCSampleTime		 			'FLC サンプル取得時間(Sec)
	FLCSampleAccRate	 			'FLC サンプル取得率(%)
	FLCAmplitude		 			'FLC 振幅(mm)
	FLCPeriod			 			'FLC 周期(Sec)
					
	FLC1AmplitudeStart	 			'FLC1 開始振幅(mm)
	FLC1AmplitudeEnd	 			'FLC1 終了振幅(mm)
	FLC1AmplitudeStep	 			'FLC1 Step振幅(mm)
	FLC1MeasRev		 				'FLC1 逆順測定有効/無効
						
	FLC2Repeat			 			'FLC2 繰返し回数(回)
	FLC2Ope			 				'FLC2 動作時間(Sec)
	FLC2Rest			 			'FLC2 停止時間(Sec)
						
	FLC3TempStart		 			'FLC3 開始温度(℃)
	FLC3TempEnd		 				'FLC3 終了温度(℃)
	FLC3TempStep		 			'FLC3 Step温度(℃)
'10
	Section10

	Max
End enum
#else
Public enum eHead
'1
	Section1			= 0
	ToolVersion			= 1
	EquipVersion
	FileName
	DateTime
	MeasureType
	EquipID 
	XYSensID 
	ZSensID 
'2
	Section2			= 9
	DeviceSpec			
	HPPosControl
	CalculationMethod   = 12
	StressThreshold
	HPTemprature		
	PBTemprature 
	InputGap
	GapValue
	MeasurePresure
	PressContStep
	PressContDelay
	PBDiameter
	StressDetectMode	= 22
	AmplitudeMode		= 23
	XAmplitude			
	XPeriod				
	YAmplitude
	YPeriod
	ZAmplitude
	ZPeriod
	ZZeroPosition
	NoLoadZStress
	InputWait			
	HPLiftWait
	XYActWait
	MeasTimeLimit
	MeasEndStress
	TempProfile
'3
	Section3				= 38
	SampleSpec				
	SampleName 
	Lot 
	Measurer				
	Shape
	Thickness				
	Stacks
	SpecificGravity
	Weight
	Volume
	MaskTime
	JudgeRange
'4
	Section4				= 51
	XYWeightCoef			
	ZWeightCoef
	HPTempFlucRange
	PBTempFlucRange
	FullCalibrationVlue		
	ZeroCalibrationVlue
	FullStressAttenuation
	FullStressPxPhase		
	FullStressPyPhase
	FullStressPzPhase
	ZeroAdjStartPos
	ZeroAdjStabMonTime
	ZeroAdjFlucRange
	HPCenterX
	HPCenterY
	Filter			
	SGTemp
	GAPCORR
	ZSTAB
'5
	Section5				= 71
	GelTime					
	GelTimeTemp
	GelTimeVisco
	GelTimeJudge
'6
	Section6				= 76
	SLOPEDiffInterval
'7
	Section7				= 78
	ExtStartTime			
	ExtMiddleTime	
	ExtEndTime		
	ExtTanSample			
	ExtTanThinning	
	ExtGelTimeSelect	
	ExtGelTimeUnder	
	ExtGelTimeMiddle	
	ExtGelTimeOver	
'8
	Section8				= 88
	MELTStartTemp					'MELT法 開始温度(℃)
	MELTEndTemp						'MELT法 終了温度(℃)
	MELTWaitTime					'MELT法 待ち時間(Sec)
	MELTSpeed						'MELT法 昇温速度(℃/min)
	MELTTemperature					'MELT法 結果温度(℃)
	MELTViscosity					'MELT法 結果粘度(PaS)
'9
	Section9				= 95
	FLCTemp			 				'FLC 設定温度(℃)
	ProbeDiameter		 			'FLC プローブ直径(mm)
	FLCSampleTime		 			'FLC サンプル取得時間(Sec)
	FLCSampleAccRate	 			'FLC サンプル取得率(%)
	FLCAmplitude		 			'FLC 振幅(mm)
	FLCPeriod			 			'FLC 周期(Sec)
					
	FLC1AmplitudeStart	 	= 102	'FLC1 開始振幅(mm)
	FLC1AmplitudeEnd	 			'FLC1 終了振幅(mm)
	FLC1AmplitudeStep	 			'FLC1 Step振幅(mm)
	FLC1MeasRev		 				'FLC1 逆順測定有効/無効
						
	FLC2Repeat			 			'FLC2 繰返し回数(回)
	FLC2Ope			 				'FLC2 動作時間(Sec)
	FLC2Rest			 			'FLC2 停止時間(Sec)
						
	FLC3TempStart		 			'FLC3 開始温度(℃)
	FLC3TempEnd		 				'FLC3 終了温度(℃)
	FLC3TempStep		 	= 111	'FLC3 Step温度(℃)
'10
	Section10				= 112

	Max
End enum
#end if
'GelTime データ列順番
Private enum eDataColmnGel
	MSec		= 0
	XSrc		
	YSrc
	ZSrc
	XPos
	YPos 
	ZPos 
	Temp
	Mv
	Temp2
	Mv2
	Hosei		= 11
	HoseiP
	Visco
	XPosPhase
	SampleStrain
	ShearMod
	ShearVelo
	XStorageMod
	XElasticMod
	XLossTan
	Max
End enum
'LTM データ列順番
Private enum eDataColmnLTM
	MSec		= 0
	Temp
	Mv
	Temp2
	Mv2
	Hosei		= 5
	HoseiP
	Visco
	XPosPhase
	SampleStrain
	ShearMod
	ShearVelo
	XStorageMod
	XElasticMod
	XLossTan
	Max
End enum
'履歴データ表示順番
#If 1
Public enum eHistCol
	Color				= 0
	FileName
	SampleName 
	SampleSpec				
	Lot 
	Measurer				
	Shape
	Thickness				
	Stacks
	MaskTime
	JudgeRange

	DeviceSpec			
	HPPosControl
	HPTemprature		
	PBTemprature 
	SGTemp
	HPTempFlucRange
	PBTempFlucRange
	TempProfile
	InputGap
	GapValue
	MeasurePresure
	PressContStep
	PressContDelay
	PBDiameter
	StressDetectMode
	AmplitudeMode
	LimitStress
	XAmplitude			
	XPeriod				
	InputWait			
	HPLiftWait
	XYActWait
	MeasTimeLimit
	MeasEndStress

	DateTime
	MeasureType
	CalculationMethod
	StressThreshold
	SLOPEDiffInterval
	GelTime					
	GelTimeJudge
	GelTimeTemp
	GelTimeVisco
	Filter			

	ToolVersion			
	EquipVersion
	EquipID 
	XYSensID 
	ZSensID 

	XYWeightCoef			
	ZWeightCoef
	FullCalibrationVlue		
	ZeroCalibrationVlue
	FullStressAttenuation
	FullStressPxPhase		
	FullStressPyPhase
	FullStressPzPhase
	GAPCORR
	NoLoadZStress
	ZSTAB
	ZeroAdjStabMonTime
	ZeroAdjFlucRange
	ZZeroPosition
	ZAfterPosition
	RealGap
	HPCenterX
	HPCenterY
	Max
End enum
#else
Public enum eHistCol
	Color				= 0
	FileName
	DateTime
	HPPosControl
	CalculationMethod
	StressThreshold
	HPTemprature		
	PBTemprature 
	GapValue
	MeasurePresure
	PBDiameter
	StressDetectMode
	XAmplitude			
	XPeriod				
	InputWait			
	HPLiftWait
	XYActWait
	MeasTimeLimit
	MeasEndStress
	TempProfile
	SampleName 
	Lot 
	Measurer				
	Shape
	Thickness				
	Stacks
	MaskTime
	JudgeRange
	Filter					
	GelTime					
	GelTimeTemp
	GelTimeVisco
	Max
End enum
#End If
'セクション区切り行
Public Const cHeader			As String	= "[header]"
Public Const cData				As String	= "[data]"

'
''' <summary>測定ファイル１ファイルの秒数</summary>
Public Const cSecPerOneFile		As Int64	= 500
''' <summary>測定ファイル１ファイルのサンプル数 500秒分</summary>
Public Const cMesFileLineNum	As Int64 = MEAS_BLOCK_DATA_NUM * cSecPerOneFile		
'▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼
'   構造体
'▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲
'測定データファイル ヘッダ
Public Structure DataFileHeader
	Dim sToolVersion			As String		'PCソフトウェアバージョン
	Dim sEquipVersion			As String		'本体ソフトウェアバージョン
	Dim sFileName				As String		'自ファイル名
	Dim sDateTime				As String		'測定開始日時(年月日時分秒)
	Dim sMeasureType			As String		'測定種類 (Gel,Cal,FL1,FL2,FL3,)
	Dim sEquipName				As String		'本体識別
	Dim sXYSensorID				As String		'XYセンサID
	Dim sZSensorID				As String		'ZセンサID

	Dim sDeviceSpec				As String		'デバイスSPEC名
	Dim sHPPosControl			As Int16		'熱板位置制御 0:1:2:
	Dim sCalcMethod				As String		'ゲルタイム計算方法
	Dim sThreshold				As String		'応力判定値[%]
	Dim dHPTemp					As Double		'熱板温度[℃]
	Dim dPBTemp					As Double		'プローブ温度[℃]
	Dim dInputGap				As Double		'投入GAP[mm]
	Dim dGapValue				As Double		'GAP値[mm]
	Dim dMeasurePress			As Double		'測定圧力[kPa]
	Dim iPressContStep			As int32		'圧力制御STEP数
	Dim iPressContDelay			As int32		'圧力制御ディレイ時間[mS]
	Dim dPBDiameter				As Double		'プローブ直径[mm]
	Dim sStrsDetectMode			As Int16		'応力検知モード
	Dim sAmpMode				As Int16		'振幅測定モード
	Dim dAmpModeStress			As Double		'上限応力[%] 振幅測定モード
	Dim dXAmp					As Double		'X軸振幅[mm]
	Dim dXPrd					As Double		'X軸周期[秒]
	Dim dYAmp					As Double		'Y軸振幅[mm]
	Dim dYPrd					As Double		'Y軸周期[秒]
	Dim dZAmp					As Double		'Z軸振幅[mm]
	Dim dZPrd					As Double		'Z軸周期[秒]
	Dim dZZeroPosition			As Double		'Z軸ゼロ位置[mm]
	Dim dZNoLoadStress			As Double		'Z軸無負荷応力[g]
	Dim dInputWait				As Double		'投入待ち時間
	Dim dHPLiftWait				As Double		'熱板上昇待ち時間
	Dim dXYActWait				As Double		'XY動作待ち時間
	Dim dMeasTimeLimit			As Double		'Gel 測定終了リミット時間
	Dim dMeasEndStress			As Double		'Gel 測定終了応力
	Dim sTempProfile			As String		'Gel 温度プロファイル名

	Dim dDiffInterval			As Double		'SLOPE 微分区間[秒]
	Dim dLimitStress			As Double		'制限応力[%]


	Dim sSampleSpec				As String		'サンプルSPEC名
	Dim sSampleName				As String		'サンプル名
	Dim sLot					As String		'ロット
	Dim sMeasurer				As String		'測定者
	Dim sShape					As Int16		'形状  0:シート 1:粉 2:液
	Dim sThickness				As String		'厚み/密度
	Dim dStacks					As Double		'積層枚数/重量/体積
	Dim sMaskTime				As String		'Gel マスク範囲(秒) 999.9 - 999.9
	Dim sJudgeRange				As String		'Gel ゲルタイム判定範囲(秒) 999.9 - 999.9


	Dim dXYWeightCoef			As Double		'重量変換係数[N/dig]
	Dim dZWeightCoef			As Double		'重量変換係数[g/dig]
	Dim sHPTempFlucRange		As String		'HP温度変動許容範囲(℃)
	Dim sPBTempFlucRange		As String		'PB温度変動許容範囲(℃)
	Dim sFullCalibValue			As String		'フルキャリブレーション値
	Dim sZeroCalibValue			As String		'ゼロキャリブレーション値
	Dim sFullStressAttenuation	As String		'フルストレス減衰比
	Dim sFullStressPhaseX		As String		'フルストレス位相X
	Dim sFullStressPhaseY		As String		'フルストレス位相Y
	Dim sFullStressPhaseZ		As String		'フルストレス位相Z
	'Dim sZeroAdjStartPos		As String		'ゼロ調 HP開始位置(mm)
	Dim sZeroAdjStabMonTime		As String		'ゼロ調 安定監視時間(Sec)
	Dim sZeroAdjFlucRange		As String		'ゼロ調 変動許容範囲(um)
	Dim sHPCenterX				As String		'HP中心位置X
	Dim sHPCenterY				As String		'HP中心位置Y
	Dim sFilter					As String		'フィルタ条件
	Dim dSGTemp					As Double		'起歪体設定温度[℃]
	Dim dGapCorrect				As Double		'GAP補正値[dig/um] Z荷重とたわみの比例定数
	Dim dZStab					As Double		'Z安定度[g/10S]

    'HP 温度パラメータ
    Dim dTempCnt_P				As Double
    Dim dTempCnt_I				As Double
    Dim dTempCnt_d				As Double
    Dim dTempCnt_Dgain			As Double
    Dim dTempCnt_PVBias			As Double
    Dim dTempCnt_PVGain			As Double
    'PB 温度パラメータ
    Dim dTempCnt_P2				As Double
    Dim dTempCnt_I2				As Double
    Dim dTempCnt_d2				As Double
    Dim dTempCnt_Dgain2			As Double
    Dim dTempCnt_PVBias2		As Double
    Dim dTempCnt_PVGain2		As Double
    'SG 温度パラメータ
    Dim dTempCnt_P3				As Double
    Dim dTempCnt_I3				As Double
    Dim dTempCnt_d3				As Double
    Dim dTempCnt_Dgain3			As Double
    Dim dTempCnt_PVBias3		As Double
    Dim dTempCnt_PVGain3		As Double

	Dim dGelTime				As Double		'Gel ゲルタイム(秒)
	Dim dGelTimeTemp			As Double		'Melt 判定時の温度[℃]
	Dim dGelTimeVisco			As Double		'Melt 判定時の粘度[]
	Dim sGelTimeJudge			As String		'Gel 判定結果 OK,NG,NA

	Dim dOosValX				As Double		'脱調確認結果 X (um)
	Dim dOosValY				As Double		'脱調確認結果 Y (um)
	Dim dOosValZ				As Double		'脱調確認結果 Z (um)

	Dim iCpuCommErrCount		As Int32		'CPU間通信エラーカウント

	Dim iMeasActTime			As Int64		'実測定時間(秒)
	Dim dZAfterPos				As Double		'着圧決定Z位置(um)
	Dim dRealGap				As Double		'実測定GAP(mm)

	Dim iIntegTimePowOn			As UInt32		'本体ON積算時間[秒]
	Dim iIntegTimeHeatOn		As UInt32		'HP ON積算時間[秒]
	Dim iIntegTimePBHeat		As UInt32		'PB ON積算時間[秒]
	Dim iIntegTimeSGHeat		As UInt32		'SG ON積算時間[秒]
	Dim iIntegTimeXAct			As UInt32		'X軸動作 ON積算時間[秒]
	Dim iIntegTimeYAct			As UInt32		'Y軸動作 ON積算時間[秒]
	Dim iIntegTimeZAct			As UInt32		'Z軸動作 ON積算時間[秒]

	Dim sExtStartTime			As String		'Gel 外挿法 開始点(秒)
	Dim sExtMiddleTime			As String		'Gel 外挿法 中間点(秒)
	Dim sExtEndTime				As String		'Gel 外挿法 終了点(秒)
	Dim sExtTanSample			As String		'Gel 外挿法 接線生成サンプル数
	Dim sExtTanThinning			As String		'Gel 外挿法 接線生成間引き数
	Dim sExtGelTimeSelect		As String		'Gel 外挿法 ゲルタイム選択(Under、Middle、Over)
	Dim sExtGelTimeUnder		As String		'Gel 外挿法 ゲルタイム(Under)
	Dim sExtGelTimeMiddle		As String		'Gel 外挿法 ゲルタイム(Middle)
	Dim sExtGelTimeOver			As String		'Gel 外挿法 ゲルタイム(Over)
						
	Dim sMelStartTemp			As String		'MELT法 開始温度(℃)
	Dim sMelEndTemp				As String		'MELT法 終了温度(℃)
	Dim sMelWaitTime			As String		'MELT法 待ち時間(Sec)
	Dim sMelSpeed				As String		'MELT法 昇温速度(℃/min)
	Dim sMelTemperatureRslt		As String		'MELT法 結果温度(℃)
	Dim sMelViscosityRslt		As String		'MELT法 結果粘度(PaS)

	Dim sFLCTemp				As String		'FLC 設定温度(℃)
	Dim sProbeDiameter			As String		'FLC プローブ直径(mm)
	Dim sFLCSampleTime			As String		'FLC サンプル取得時間(Sec)
	Dim sFLCSampleAccRate		As String		'FLC サンプル取得率(%)
	Dim sFLCAmplitude			As String		'FLC 振幅(mm)
	Dim sFLCPeriod				As String		'FLC 周期(Sec)
						
	Dim sFLC1AmplitudeStart		As String		'FLC1 開始振幅(mm)
	Dim sFLC1AmplitudeEnd		As String		'FLC1 終了振幅(mm)
	Dim sFLC1AmplitudeStep		As String		'FLC1 Step振幅(mm)
	Dim sFLC1MeasRev			As String		'FLC1 逆順測定有効/無効
						
	Dim sFLC2Repeat				As String		'FLC2 繰返し回数(回)
	Dim sFLC2Ope				As String		'FLC2 動作時間(Sec)
	Dim sFLC2Rest				As String		'FLC2 停止時間(Sec)
						
	Dim sFLC3TempStart			As String		'FLC3 開始温度(℃)
	Dim sFLC3TempEnd			As String		'FLC3 終了温度(℃)
	Dim sFLC3TempStep			As String		'FLC3 Step温度(℃)

	'Dim iState					As Int32		'測定データ状態  1:ファイルから読み込まれて再計算されていない 0:それ以外
End Structure

''' <summary>
''' 粘度計算パラメータ構造体
''' </summary>
Public Structure ViscosityCalcParameter
	Dim dPeriod					As Double										'設定周期(S)
	Dim dAmplitude				As Double										'設定振幅(m)
	Dim dTemp					As Double										'設定温度(℃)
	Dim dMeasGap				As Double										'測定GAP(m)
	Dim dProbeSquare			As Double										'プローブ面積(m^2)
	Dim dProbeRadius			As Double										'プローブ半径(m)
	Dim dMeasStress				As Double										'測定応力(N)
	Dim dVelocityRate			As Double										'せん断速度補正係数

	Dim dShearVelocity			As Double										'せん断速度(1/S)
	Dim dShearStress			As Double										'せん断応力(Pa)
	Dim dViscosity				As Double										'粘度(Pa･S)
	Dim dXPosPhase				As Double										'動粘弾性X
	Dim dSplStrain				As Double										'サンプル歪
	Dim dShearMod				As Double										'せん断弾性率(Pa)
	Dim dXStrgMod				As Double										'X貯蔵弾性率(Pa)
	Dim dXElasMod				As Double										'X損失弾性率(Pa)
	Dim dXLossTan				As Double										'X損失正接
	Dim dHPTemp					As Double										'HP温度(℃)
	Dim dPBTemp					As Double										'PB温度(℃)
End Structure


'▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼
'   API宣言
'▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲

'▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼
'   パブリック変数
'▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲
Public p_FilterPath						As String = Path.GetDirectoryName(Application.ExecutablePath) & "\FilterCoef"			'フィルタ設定保存パス

Public	p_sHeaderItemName1				As String = "ElapseTime(mS),Stress-X,Stress-Y,Stress-Z,Position-X,Position-Y,Position-Z,HP Temperature(PV),HP Temperature(MV),PB Temperature(PV),PB Temperature(MV)"
Public	p_sHeaderItemName2				As String = "Stress(uN),Stress(%),Viscosity(PaS),Phase(deg),Sample strain,Shear modulus(Pa),Shear rate(1/S),Storage modulus(Pa),Elastic modulus(Pa),Tangent"
Public	p_sHeaderItemName3				As String = "Amplitude(mm),Stress(N),Shear rate(1/S),Shear stress(Pa),Viscosity(PaS),Phase(deg),Sample strain,Shear modulus(Pa),Storage modulus(Pa),Elastic modulus(Pa),Tangent,HP Temperature(deg C),PB Temperature(deg C)"
Public	p_sHeaderItemName4				As String = "Preset temperature(deg C),Stress(N),Shear stress(Pa),Viscosity(PaS),Phase(deg),Sample strain,Shear modulus(Pa),Storage modulus(Pa),Elastic modulus(Pa),Tangent,HP Temperature(deg C),PB Temperature(deg C)"
Public	p_sHeaderItemName5				As String = "ElapseTime(S),HP Temperature(PV),HP Temperature(MV),PB Temperature(PV),PB Temperature(MV),Stress(uN),Stress(%),Viscosity(PaS),Phase(deg),Sample strain,Shear modulus(Pa),Shear rate(1/S),Storage modulus(Pa),Elastic modulus(Pa),Tangent"

'測定種類識別コード
Public sMeasindCode()					As String = { "GEL","CLB","FL1","FL2","FL3","LTM","GLA" }
Public Enum eMesCD
	GEL=0
	CLB
	FL1
	FL2
	FL3
	LTM
	GLA
	MAX
End Enum

'現在表示している測定モード GEL/LTM
Public p_CurrReadType					As String = "GEL"
'▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼
'   プライベート変数
'▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲
'ヘッダセクション名
'Private sHeadSection()					As String = { _
'														"[Basic information]", _
'														"[Device SPEC]", _
'														"[Sample SPEC]", _
'														"[Detail setting]", _
'														"[Results]", _
'														"[Extrapolation SPEC]", _
'														"[Melt SPEC]", _
'														"[Flow curve SPEC]", _
'														"[Criteria]" _
'														}

#If 1
Private	m_HeadItem()					As String = { _
													"[Sample SPEC]", _
														"Sample SPEC", _
														"Sample Name", _
														"Lot", _
														"Measurer", _
														"Shape", _
														"Thickness[mm]", _
														"Lamination", _
														"Density[g/ml]", _
														"Weight[g]", _
														"Volume[ml]", _
														"Effective Data Area[Sec]", _
														"GT Standard[Sec]", _
													"[Device SPEC]", _
														"Device SPEC", _
														"HP Position Control", _
														"HP Temperature[deg C]", _
														"PB Temperature[deg C]", _
														"SG Temperature[deg C]", _
														"HP Temp Deviation[deg C]", _
														"PB Temp Deviation[deg C]", _
														"Temperature Profile", _
														"Input Gap[mm]", _
														"Measurement Gap[mm]", _
														"Target Pressure[kPa]", _
														"Z STEP Resolution[um]", _
														"Z TIME Resolution[mSec]", _
														"Probe Diameter[mm]", _
														"Oscillation Trajectory", _
														"Amplitude Control Mode", _
														"Limit Stress[%]", _
														"X Amplitude[mm]", _
														"X Period[Sec]", _
														"Y Amplitude[mm]", _
														"Y Period[Sec]", _
														"Z Amplitude[mm]", _
														"Z Period[Sec]", _
														"Time to Put in[Sec]", _
														"Time to HP Rise[Sec]", _
														"Time to Oscillation[Sec]", _
														"Time to Auto Stop[Sec]", _
														"Stress to Auto Stop[%]", _
													"[Results]", _
														"File Name", _
														"Measurement Date", _
														"Measurement Type", _
														"GT Judgement Method", _
														"Threshold[%]", _
														"Differential Time Constant[Sec]", _
														"GelTime[Sec]", _
														"GelTime Judge", _
														"Melt Temperature[deg C]", _
														"Melt Viscosity[PaS]", _
														"Filter", _
														"Misalignment X[um]", _
														"Misalignment Y[um]", _
														"Misalignment Z[um]", _
														"CPU Comm. Error Count", _
														"Actual measurement Time[Sec]", _
														"After determining Z-Pos[um]", _
														"Real Gap[mm]", _
													"[Basic information]", _
														"PC Software Version",  _
														"Firmware Version",  _
														"SIZUKA S/N", _
														"XY Sensor S/N", _
														"Z Sensor S/N", _
														"Power ON Time[Sec]", _
														"HP Heater ON Time[Sec]", _
														"PB Heater ON Time[Sec]", _
														"SG Heater ON Time[Sec]", _
														"X Axis Action Time[Sec]", _
														"Y Axis Action Time[Sec]", _
														"Z Axis Action Time[Sec]", _
													"[Calibration]", _
														"N Coefficient XY[N/dig]", _
														"Weight Coefficient Z[gf/dig]", _
														"Full Stress", _
														"Zero Stress", _
														"Full Stress Attenuation Rate", _
														"Full Stress Phase X", _
														"Full Stress Phase Y", _
														"Full Stress Phase Z", _
														"GAP Correction[dig/um]", _
														"Z No load sress[gf]", _
														"Z Stress Stabirity[g/10Sec]", _
														"ZeroPos Deviation[um]", _
														"ZeroPos Stability Time[Sec]", _
														"Zero Position[um]", _
														"Center Position X[um]", _
														"Center Position Y[um]", _
														"HP Temp.Cont. P", _
														"HP Temp.Cont. I", _
														"HP Temp.Cont. D", _
														"HP Temp.Cont. Dgain", _
														"HP Temp.Cont. PVBias", _
														"HP Temp.Cont. PVGain", _
														"PB Temp.Cont. P", _
														"PB Temp.Cont. I", _
														"PB Temp.Cont. D", _
														"PB Temp.Cont. Dgain", _
														"PB Temp.Cont. PVBias", _
														"PB Temp.Cont. PVGain", _
														"SG Temp.Cont. P", _
														"SG Temp.Cont. I", _
														"SG Temp.Cont. D", _
														"SG Temp.Cont. Dgain", _
														"SG Temp.Cont. PVBias", _
														"SG Temp.Cont. PVGain", _
													"[Extrapolation SPEC]", _
														"Extrapolation start time[Sec]", _
														"Extrapolation middle time[Sec]", _
														"Extrapolation end time[Sec]", _
														"Extrapolation tangent sample", _
														"Extrapolation tangent thinning", _
														"Extrapolation Geltime selection", _
														"Extrapolation Geltime under[Sec]", _
														"Extrapolation Geltime middle[Sec]", _
														"Extrapolation Geltime over[Sec]", _
													"[Melt SPEC]", _
														"Melt Start Temperature[deg C]", _
														"Melt End Temperature[deg C]", _
														"Melt Waiting time for rize[Sec]", _
														"Melt Heating rate[deg C/min]", _
														"Melt Result Temperature[deg C]", _
														"Melt Result Viscosity[PaS]", _
													"[Flow curve SPEC]", _
														"Flow Curve temperature[deg C]", _
														"Flow Curve Probe diameter[mm]", _
														"Flow Curve Sample get time[Sec]", _
														"Flow Curve Sample get rate[%]", _
														"Flow Curve Amplitude[mm]", _
														"Flow Curve Period[Sec]", _
														"Flow Curve Amp. start[mm]", _
														"Flow Curve Amp. end[mm]", _
														"Flow Curve Amp. step[mm]", _
														"Flow Curve Amp. Reverse", _
														"Flow Curve Cycle Count", _
														"Flow Curve Operation time[Sec]", _
														"Flow Curve Rest time[Sec]", _
														"Flow Curve Start temperature[deg C]", _
														"Flow Curve End temperature[deg C]", _
														"Flow Curve Step temperature[deg C]", _
													"[Criteria]" _
													}
#else
Private	m_HeadItem()					As String = { _
													"[Basic information]", _
														"Tool Version",  _
														"Equip Version",  _
														"File Name", _
														"Measure Date", _
														"Measurement Type" , _
														"Equip ID", _
														"XY Sensor ID", _
														"Z Sensor ID", _
													"[Device SPEC]", _
														"Device SPEC", _
														"HP position control", _
														"Calculation Method", _
														"Stress Threshold[%]", _
														"HP Temperature[deg C]", _
														"PB Temperature[deg C]", _
														"Input Gap[mm]", _
														"Measurement Gap[mm]", _
														"Measure Pressure[kPa]", _
														"Pressure control STEP", _
														"Pressure control delay time[mSec]", _
														"Probe diameter[mm]", _
														"Stress detection mode", _
														"Amplitude mode", _
														"X Amplitude[mm]", _
														"X Period[Sec]", _
														"Y Amplitude[mm]", _
														"Y Period[Sec]", _
														"Z Amplitude[mm]", _
														"Z Period[Sec]", _
														"Z Zero position[um]", _
														"Z No load sress[g]", _
														"Input Wait[Sec]", _
														"HP Lift Wait[Sec]", _
														"XY Action Wait[Sec]", _
														"Measure Time Limit[Sec]", _
														"Measure End Stress[%]", _
														"Temp Profile Name", _
													"[Sample SPEC]", _
														"Sample SPEC", _
														"Sample name", _
														"Lot", _
														"Measurer", _
														"Shape", _
														"Thickness[mm]", _
														"Stacks", _
														"SpecificGravity[g/ml]", _
														"Weight[g]", _
														"Volume[ml]", _
														"Mask Time Range[Sec]", _
														"GelTime Judge Range[Sec]", _
													"[Detail setting]", _
														"XY Weight coefficient[N/dig]", _
														"Z Weight coefficient[g/dig]", _
														"Range of HP temperature[deg C]", _
														"Range of PB temperature[deg C]", _
														"Full Calibration Vlue", _
														"Zero Calibration Vlue", _
														"Full Stress Attenuation Rate", _
														"Full Stress Phase X", _
														"Full Stress Phase Y", _
														"Full Stress Phase Z", _
														"ZeroAdj - HP start position[um]", _
														"ZeroAdj - Stability monitoring time[Sec]", _
														"ZeroAdj - Fluctuation range[um]", _
														"HP Center Position X[um]", _
														"HP Center Position Y[um]", _
														"Filter", _
														"SG Temperature[deg C]", _
														"GAP Correction[dig/um]", _
														"Z Stabirity[g/10Sec]", _
													"[Results]", _
														"GelTime[Sec]", _
														"Melt Temperature[deg C]", _
														"Melt Viscosity[PaS]", _
														"GelTime Judge", _
													"[SLOPE SPEC]", _
														"Slope Differential interval[Sec]", _
													"[Extrapolation SPEC]", _
														"Extrapolation start time[Sec]", _
														"Extrapolation middle time[Sec]", _
														"Extrapolation end time[Sec]", _
														"Extrapolation tangent sample", _
														"Extrapolation tangent thinning", _
														"Extrapolation Geltime selection", _
														"Extrapolation Geltime under[Sec]", _
														"Extrapolation Geltime middle[Sec]", _
														"Extrapolation Geltime over[Sec]", _
													"[Melt SPEC]", _
														"Melt Start Temperature[deg C]", _
														"Melt End Temperature[deg C]", _
														"Melt Waiting time for rize[Sec]", _
														"Melt Heating rate[deg C/min]", _
														"Melt Result Temperature[deg C]", _
														"Melt Result Viscosity[PaS]", _
													"[Flow curve SPEC]", _
														"Flow Curve temperature[deg C]", _
														"Flow Curve Probe diameter[mm]", _
														"Flow Curve Sample get time[Sec]", _
														"Flow Curve Sample get rate[%]", _
														"Flow Curve Amplitude[mm]", _
														"Flow Curve Period[Sec]", _
														"Flow Curve Amp. start[mm]", _
														"Flow Curve Amp. end[mm]", _
														"Flow Curve Amp. step[mm]", _
														"Flow Curve Amp. Reverse", _
														"Flow Curve Cycle Count", _
														"Flow Curve Operation time[Sec]", _
														"Flow Curve Rest time[Sec]", _
														"Flow Curve Start temperature[deg C]", _
														"Flow Curve End temperature[deg C]", _
														"Flow Curve Step temperature[deg C]", _
													"[Criteria]" _
														}

#end if

'履歴表示DGVのカラムヘッダ
#If 1
Private	m_ColumnHeadItem()					As String = { _
														"File Name", _
														"Sample Name", _
														"Sample SPEC", _
														"Lot", _
														"Measurer", _
														"Shape", _
														"Thickness[mm]", _
														"Stacks", _
														"Effective Data Area[Sec]", _
														"GT Standard[Sec]", _
														"Device SPEC", _
														"HP Position Control", _
														"HP Temperature[deg C]", _
														"PB Temperature[deg C]", _
														"SG Temperature[deg C]", _
														"HP Temp Deviation[deg C]", _
														"PB Temp Deviation[deg C]", _
														"Temperature Profile", _
														"Input Gap[mm]", _
														"Measurement Gap[mm]", _
														"Target Pressure[kPa]", _
														"Z STEP Resolution[um]", _
														"Z TIME Resolution[mSec]", _
														"Probe Diameter[mm]", _
														"Oscillation Trajectory", _
														"Amplitude Control Mode", _
														"Limit Stress[%]", _
														"Osc Amplitude[mm]", _
														"Osc Period[Sec]", _
														"Time to Put in[Sec]", _
														"Time to HP Rise[Sec]", _
														"Time to Oscillation[Sec]", _
														"Time to Auto Stop[Sec]", _
														"Stress to Auto Stop[%]", _
														"Measurement Date", _
														"Measurement Type", _
														"GT Judgement Method", _
														"Threshold[%]", _
														"Differential Time Constant[Sec]", _
														"GelTime[Sec]", _
														"GelTime Judge", _
														"Melt Temperature[deg C]", _
														"Melt Viscosity[PaS]", _
														"Filter", _
														"PC Software Version",  _
														"Firmware Version",  _
														"SIZUKA S/N", _
														"XY Sensor S/N", _
														"Z Sensor S/N", _
														"N Coefficient XY[N/dig]", _
														"Weight Coefficient Z[g/dig]", _
														"Full Stress", _
														"Zero Stress", _
														"Full Stress Attenuation Rate", _
														"Full Stress Phase X", _
														"Full Stress Phase Y", _
														"Full Stress Phase Z", _
														"GAP Correction[dig/um]", _
														"Z No load sress[g]", _
														"Z Stress Stabirity[g/10Sec]", _
														"ZeroPos Deviation[um]", _
														"ZeroPos Stability Time[Sec]", _
														"Zero Position[um]", _
														"Z After Position[um]", _
														"Real Meas Gap[mm]", _
														"Center Position X[um]", _
														"Center Position Y[um]" _
														}
#else
Private	m_ColumnHeadItem()					As String = { _
														"File Name", _
														"Measure Date", _
														"Sample name", _
														"Lot", _
														"Measurer", _
														"Shape", _
														"Thickness[mm]", _
														"Stacks", _
														"Mask Time Range[Sec]", _
														"GelTime Judge Range[Sec]", _
														"Filter", _
														"HP position control", _
														"Calculation Method", _
														"Stress Threshold[%]", _
														"HP Temperature[deg C]", _
														"PB Temperature[deg C]", _
														"Measurement Gap[mm]", _
														"Measure Pressure[kPa]", _
														"Probe diameter[mm]", _
														"Stress detection mode", _
														"Amplitude mode", _
														"X Amplitude[mm]", _
														"X Period[Sec]", _
														"Input Wait[Sec]", _
														"HP Lift Wait[Sec]", _
														"XY Action Wait[Sec]", _
														"Measure Time Limit[Sec]", _
														"Measure End Stress[%]", _
														"Temp Profile Name", _
														"GelTime[Sec]", _
														"Melt Temperature[deg C]", _
														"Melt Viscosity[PaS]"
														}
Private	m_ColumnHeadItem()					As String = { _
														"File Name", _
														"Measure Date", _
														"HP position control", _
														"Calculation Method", _
														"Stress Threshold[%]", _
														"HP Temperature[deg C]", _
														"PB Temperature[deg C]", _
														"Measurement Gap[mm]", _
														"Measure Pressure[kPa]", _
														"Probe diameter[mm]", _
														"Stress detection mode", _
														"X Amplitude[mm]", _
														"X Period[Sec]", _
														"Input Wait[Sec]", _
														"HP Lift Wait[Sec]", _
														"XY Action Wait[Sec]", _
														"Measure Time Limit[Sec]", _
														"Measure End Stress[%]", _
														"Temp Profile Name", _
														"Sample name", _
														"Lot", _
														"Measurer", _
														"Shape", _
														"Thickness[mm]", _
														"Stacks", _
														"Mask Time Range[Sec]", _
														"GelTime Judge Range[Sec]", _
														"Filter", _
														"GelTime[Sec]", _
														"Melt Temperature[deg C]", _
														"Melt Viscosity[PaS]"
														}
#end if
Private m_ColCntents()						As String = { _
														"Real data area", _
														"history area" , _
														"Standard area" , _
														"Series color"
														}
Public Property HistColumnHeadItem As String()
	Get
		HistColumnHeadItem = m_ColumnHeadItem
	End Get
	Set(value() As String)
		m_ColumnHeadItem = value
	End Set
End Property
Public Property ColCntents As String()
	Get
		ColCntents = m_ColCntents
	End Get
	Set(value() As String)
		m_ColCntents = value
	End Set
End Property





Protected	m_CurrDataFileName				As String				'現在の測定データファイル名
Protected	m_iDCnt							As UInt64				'測定データ蓄積カウンタ
Private		m_iMesBlockCnt					As Int32 = 0			'測定受信ブロックカウンタ

''' <summary>
''' 測定データパスの設定
''' </summary>
Public Sub InitDataPath()
	DATA_STORE_PATH = Path.Combine( SIZUKA_PATH, p_sEquipName, MESDAT_PATH )
End Sub

'▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼
'   メンバ関数
'▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲
'現在の測定データファイル名を得る
Public Readonly Property GetDataFileName As String
	Get
		If isnothing(m_CurrDataFileName) then
			GetDataFileName = ""
		Else
			SetDataFileName = m_CurrDataFileName
			GetDataFileName = m_CurrDataFileName
		End If
	End Get
End Property
'現在の測定データフルパス名を得る
Public Readonly Property GetDataFullPath(ByVal iCode As Int16) As String
	Get
		If isnothing(m_CurrDataFileName) then
			GetDataFullPath = ""
		Else
			Dim sYMFolder As String
			Select Case iCode
			Case eMesCD.GEL
				sYMFolder = m_CurrDataFileName.Substring(0, 6)
			Case eMesCD.LTM, eMesCD.FL1, eMesCD.FL2, eMesCD.FL3, eMesCD.GLA
				sYMFolder = m_CurrDataFileName.Substring(4, 6)
			Case eMesCD.CLB
				sYMFolder = m_CurrDataFileName.Substring(5, 6)
			Case Else
				sYMFolder = m_CurrDataFileName.Substring(0, 6)
			End Select
			GetDataFullPath = Path.Combine( DATA_STORE_PATH, sMeasindCode(iCode), sYMFolder )
		End If
	End Get
End Property
'測定蓄積データ数のカウント値
Public Property DataCount() As UInt64
	Get
		DataCount = m_iDCnt
	End Get
	Set(value As UInt64)
		m_iDCnt = value
	End Set
End Property
'測定受信ブロックカウンタ
Public Property MesBlockCnt() As UInt64
	Get
		MesBlockCnt = m_iMesBlockCnt
	End Get
	Set(value As UInt64)
		m_iMesBlockCnt = value
	End Set
End Property
'現在の測定データファイル名を設定
'	iType	1:キャリブレーションデータ
public Writeonly Property SetDataFileName() As string
	Set(value As string )
		Dim idx As Int16
		idx = value.IndexOf("#")
		Select Case idx
		Case -1, 0
			m_CurrDataFileName = value
		Case Else
			m_CurrDataFileName = value.Substring(0, idx)
		End Select
		m_CurrDataFileName = m_CurrDataFileName & "#" & m_iMesFileNumber.ToString("000000") & "." & sDataExt
	End Set
End Property
'''' <summary>
'''' 測定データに計算結果も付加するか 0:付加しない 1:付加する
'''' </summary>
'Public m_iResult As Int16 = 1
'Public Property CalcResultEnable() As Int16
'	Get
'		CalcResultEnable = m_iResult
'	End Get
'	Set(value As Int16)
'		m_iResult = value
'	End Set
'End Property
''' <summary>
''' 測定ファイル連番 0-
''' </summary>
Public m_iMesFileNumber As Int32 = 0
Public Property MesFileNumber() As Int32
	Get
		MesFileNumber = m_iMesFileNumber
	End Get
    Set(value As Int32)
		m_iMesFileNumber = value
    End Set
End Property


''' <summary>
''' 差分を算出する区間
''' </summary>
Private m_iSection As int32 = 	250
Public Property SectionCount() As Int32
	Get
		SectionCount = m_iSection
	End Get
	Set(value As Int32)
		m_iSection = value
	End Set
End Property




''' <summary>
''' 測定ファイルパスを決定する
''' </summary>
''' <param name="sFilePath">生成されたファイルフルパス</param>
''' <param name="sPath">オプション 指定フォルダ</param>
''' <param name="sFileName">オプション 指定ファイル</param>
''' <param name="iCalMode">0:ゼロキャリブ 1:フルキャリブ</param>
''' <returns></returns>
Public Function RealTimeMeasDataFilePath(ByRef sFilePath As String,  Optional ByVal sPath As String = Nothing, Optional ByVal sFileName As String = Nothing, Optional ByVal iCalMode As int16 = 0) As Int32
	Dim sLineBuf As String = Nothing
	Dim sYMFolder As String
Try
	If isNothing(sPath) Then
	'決められたファイルパス
		If IsNothing(m_CurrDataFileName)
		'ファイル名が指定されていない
			Return -2
		End If
		Dim shd As String
		shd = m_CurrDataFileName.Substring(0, 3)
		Select Case shd
		Case p_dtFile.sMeasindCode(eMesCD.CLB)
		'キャリブレーションデータ
			sYMFolder = Path.Combine(shd & If(iCalMode=0,"F","Z"), m_CurrDataFileName.Substring(5, 6))

		Case p_dtFile.sMeasindCode(eMesCD.FL1),p_dtFile.sMeasindCode(eMesCD.FL2),p_dtFile.sMeasindCode(eMesCD.FL3)
		'フローカーブ
			sYMFolder = Path.Combine(shd, m_CurrDataFileName.Substring(4, 6))

		Case p_dtFile.sMeasindCode(eMesCD.LTM)
		'間欠測定
			sYMFolder = Path.Combine(shd, m_CurrDataFileName.Substring(4, 6))

		Case p_dtFile.sMeasindCode(eMesCD.GLA)
		'長時間通常測定
			sYMFolder = Path.Combine(p_dtFile.sMeasindCode(eMesCD.GLA), m_CurrDataFileName.Substring(4, 6))

		Case Else
		'通常測定
			sYMFolder = Path.Combine(p_dtFile.sMeasindCode(eMesCD.GEL), m_CurrDataFileName.Substring(0, 6))

		End Select

		sFilePath = Path.Combine( DATA_STORE_PATH,	sYMFolder )
		'年月フォルダ存在確認->作成
		If Not Directory.Exists(sFilePath)
			CreateFolder(sFilePath)
		End If
		If IsNothing(sFileName)
			sFilePath = Path.Combine( sFilePath, m_CurrDataFileName )
		Else
			sFilePath = Path.Combine( sFilePath, sFileName )
		End If
	ElseIf Not IsNothing(sFileName)
		sFilePath = Path.Combine(sPath, sFileName)
	ElseIf IsNothing(sFileName)
		sFilePath = sPath
	Else
	'引数指定されていれば任意のファイルパス
		sFilePath = Path.Combine(sPath, sFileName)
	End If
	Return 0
Catch ex As Exception
	Return -1
End Try

End Function

Private Shared key As Byte()	= {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16}
Private Shared iv As Byte()		= {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 56}

''' <summary>
''' テキストファイルオープン
''' </summary>
''' <param name="iEnc">0:暗号化無し 1:暗号化あり</param>
''' <param name="sFileFullPath">ファイルフルパス</param>
''' <param name="fs">FileStream</param>
''' <param name="cs">CryptoStream</param>
''' <param name="sw">StreamWriter</param>
''' <returns></returns>
Public Shared Function OpenCreateTextFile(ByVal iEnc As int16, ByVal sFileFullPath As String, ByVal iFileMode As integer, ByRef fs As FileStream, ByRef cs As CryptoStream, ByRef sw As StreamWriter ) As int32
Try
	Select Case iEnc
	Case 0
		fs = New FileStream(sFileFullPath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite)
		sw = New StreamWriter(fs, Encoding.UTF8)
	Case 1
		Dim aesAlg  As New AesCryptoServiceProvider()
        Dim encryptor As ICryptoTransform
		aesAlg.Key = key
        aesAlg.IV = iv
		encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV)
		fs = New FileStream(sFileFullPath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite)
		cs = New CryptoStream(fs, encryptor, CryptoStreamMode.Write)
		sw = New StreamWriter(cs)
	End Select
	Return 0
Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Return -1
End Try
End Function



''' <summary>
''' 測定生データをテキストファイルへ保存(text)
''' </summary>
''' <param name="iType">	0:通常測定 1:キャリブレーション 2:粘度-速度依存 3:粘度-時間依存 4:粘度-温度依存</param>
''' <param name="bAppend">	True：測定時の1秒分データを追記
'''							False：測定終了時の補正値更新</param>
''' <param name="cts">		option設定情報</param>
''' <param name="sPath">	Nothing：規則に決められたファイルパスを使用</param>
''' <param name="sFileName">Nothing：規則に決められたファイルパスを使用</param>
''' <param name="iThinning">出力間引き数(iTyp=0,3  ｂAppend=Falseの時のみ有効)</param>
''' <param name="bItem	">出力項目選択(iTyp=0,3  ｂAppend=Falseの時のみ有効)</param>
''' <param name="iCalMode	">キャリブレーションモード 0:ゼロ 1:フル</param>
''' <returns></returns>
Public Function RealTimeMeasDataFileSaveText( _
												ByVal iType					As Int16, _
												ByVal bAppend				As Boolean, _
												ByVal cts					As CToolSettings, _
												Optional ByVal sPath		As String = Nothing, _
												Optional ByVal sFileName	As String = Nothing, _
												Optional ByVal iThinning	As Int32 = 1, _
												Optional ByVal bItem()		As Boolean = Nothing, _
												Optional ByVal iCalMode		As Int16 = 0, _
												Optional ByVal iLTDataPointNum	As Int32 = 0 _
) As Int32
	Dim sFilePath	As String = Nothing
	Dim iRet		As int32
	Dim i			As uInt64
	Dim j			As uInt64
	Dim iElapseTime As uInt64
	Dim sLineBuf	As String = Nothing
	Dim iAllLen		As uInt64
	'Dim lRet		As Int32
Try

	Select Case bAppend
	Case True		'追加(リアルタイム測定中)
		Select Case iType
		Case RealTimeMeas.eMeasMode.Normal, RealTimeMeas.eMeasMode.NormalNoAcum, RealtimeMeas.eMeasMode.FlowCurve_Time 			'粘度関係
			Dim fs As FileStream	= Nothing
			Dim cs As CryptoStream	= Nothing
			Dim sw As Textwriter	= Nothing

			If iType <> RealTimeMeas.eMeasMode.NormalNoAcum
				'書き込んだ結果のファイル行数計算
				RealTimeGetSampleState(iAllLen, j, iElapseTime)
				'リアルタイムグラフ用の蓄積配列拡張
				ResizeMeasArray(iAllLen)
			Else
				j = MesBlockCnt * MEAS_BLOCK_DATA_NUM						'連番生成のための現在のサンプル総数
			End if

			If iType = RealtimeMeas.eMeasMode.FlowCurve_Time
				Array.Resize(dShearStress,	iAllLen)
			End If

			m_iMesFileNumber = j \ cMesFileLineNum							'測定ファイル連番
			SetDataFileName = m_CurrDataFileName							'測定ファイル名セット
			'書き込みパスを決定する
			iRet = RealTimeMeasDataFilePath(sFilePath, sPath)
			If iRet <> 0
				Return iRet
			End If

			'ファイルストリームオープン 追加書込みモード
			fs = New FileStream(sFilePath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite)
			sw = New StreamWriter(fs, Encoding.UTF8)
			'テキストファイル書込み
			If j Mod cMesFileLineNum = 0
				Dim sHead() As String = Nothing
				p_head.sFileName = m_CurrDataFileName
				MeasFileHeaderMake(iType, sHead)				'ヘッダ情報作成
				'ヘッダ情報書込み
				sw.WriteLine(cHeader)
				For i = 0 To sHead.GetUpperBound(0)
					sw.WriteLine(sHead(i))
				Next
				Dim sLine As String = sComnt
				sLine = sLine & p_sHeaderItemName1
				sw.WriteLine(cData)
				sw.WriteLine(sLine)
			End If

			For i = 0 To p_iPwX.GetUpperBound(0)
				'経過時間,応力X,応力Y,応力Z,X軸位置,Y軸位置,Z軸位置,温度,温度操作量,,補正値,補正値微分,粘度,動粘弾性X,サンプル歪,せん断弾性率(Pa),X貯蔵弾性率(Pa),X損失弾性率(Pa),X損失正接
				p_iMSec(i) = DataCount * 2
				sLineBuf = _
							p_iMSec(i).ToString & "," & _
							p_iPwX(i).ToString & "," & _
							p_iPwY(i).ToString & "," & _
							p_iPwZ(i).ToString & "," & _
							p_iPOSX(i).ToString & "," &  _
							p_iPOSY(i).ToString & "," & _
							p_iPOSZ(i).ToString & "," & _
							p_iTemp(i).ToString & "," & _
							p_iMv(i).ToString & "," & _
							p_iTemp2(i).ToString & "," & _
							p_iMv2(i).ToString
				sw.WriteLine(sLineBuf)											'一行書き込み

				If iType <> RealTimeMeas.eMeasMode.NormalNoAcum
					'蓄積配列へ格納
					RealTimeAccumulationArrayCopy(i, j)
				Else
					DataCount += 1												'データカウント更新
				End If
			Next i
			sw.Close()
			sw = nothing
			fs.Close()
			fs = Nothing

		Case RealtimeMeas.eMeasMode.FlowCurve_Speed, RealtimeMeas.eMeasMode.FlowCurve_Temp			'粘度関係
		'フローカーブ速度/温度依存
			'書き込んだ結果のファイル行数計算
			RealTimeGetSampleState(iAllLen, j, iElapseTime)
			'リアルタイムグラフ用の蓄積配列拡張
			ResizeMeasArray(iAllLen)
			'蓄積配列へ追加
			For i = 0 To p_iPwX.GetUpperBound(0)
				'応力X,応力Y,応力Z,X軸位置,Y軸位置,温度,温度操作量,補正値
				RealTimeAccumulationArrayCopy(i, j)
			Next i

		Case RealtimeMeas.eMeasMode.Calib
			Dim fs As FileStream = Nothing
			Dim sw As Textwriter = Nothing
			'書き込んだ結果のファイル行数計算
			RealTimeGetSampleState(iAllLen, j, iElapseTime)
			'リアルタイムグラフ用の蓄積配列拡張
			ResizeMeasArray(iAllLen)

			m_iMesFileNumber = j \ cMesFileLineNum							'測定ファイル連番
			SetDataFileName = m_CurrDataFileName							'測定ファイル名セット
			'書き込みパスを決定する
			iRet = RealTimeMeasDataFilePath(sFilePath, sPath, ,iCalMode)
			If iRet <> 0
				Return iRet
			End If
			If p_bCalibFileOutput
				fs = New FileStream(sFilePath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite)
				sw = New StreamWriter(fs, Encoding.UTF8)
			End If
			'テキストファイル書込み
			If j Mod cMesFileLineNum = 0
#If 0
				Dim sHead() As String = Nothing
				p_head.sFileName = m_CurrDataFileName
				MeasFileHeaderMake(iType, sHead)				'ヘッダ情報作成
				'ヘッダ情報書込み
				If p_bCalibFileOutput
					sw.WriteLine(cHeader)
					For i = 0 To sHead.GetUpperBound(0)
						sw.WriteLine(sHead(i))
					Next
					Dim sLine As String = sComnt
					sLine = sLine & p_sHeaderItemName1
					sw.WriteLine(cData)
					sw.WriteLine(sLine)
				End If
#else
				Dim sLine As String = sComnt
				sLine = sLine & p_sHeaderItemName1
				sw.WriteLine(cData)
				sw.WriteLine(sLine)
#end if
			End If
			For i = 0 To p_iPwX.GetUpperBound(0)
				'経過時間,応力X,応力Y,応力Z,X軸位置,Y軸位置,Z軸位置,温度,温度操作量,,補正値,補正値微分,粘度,動粘弾性X,サンプル歪,せん断弾性率(Pa),X貯蔵弾性率(Pa),X損失弾性率(Pa),X損失正接
				p_iMSec(i) = DataCount * 2
				sLineBuf = _
							p_iMSec(i).ToString & "," & _
							p_iPwX(i).ToString & "," & _
							p_iPwY(i).ToString & "," & _
							p_iPwZ(i).ToString & "," & _
							p_iPOSX(i).ToString & "," &  _
							p_iPOSY(i).ToString & "," & _
							p_iPOSZ(i).ToString & "," & _
							p_iTemp(i).ToString & "," & _
							p_iMv(i).ToString & "," & _
							p_iTemp2(i).ToString & "," & _
							p_iMv2(i).ToString
				If p_bCalibFileOutput
					sw.WriteLine(sLineBuf)											'一行書き込み
				End If

				'蓄積配列へ格納
				RealTimeAccumulationArrayCopy(i, j)
			Next i
			If p_bCalibFileOutput
				sw.Close()
				sw = nothing
				fs.Close()
				fs = Nothing
			End If
		Case RealTimeMeas.eMeasMode.NormalLT
			Dim fs As FileStream	= Nothing
			Dim cs As CryptoStream	= Nothing
			Dim sw As Textwriter	= Nothing

			m_iMesFileNumber = j \ cMesFileLineNum							'測定ファイル連番
			SetDataFileName = m_CurrDataFileName							'測定ファイル名セット
			'書き込みパスを決定する
			iRet = RealTimeMeasDataFilePath(sFilePath, sPath)
			If iRet <> 0
				Return iRet
			End If

			'ファイルストリームオープン 追加書込みモード
			fs = New FileStream(sFilePath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite)
			sw = New StreamWriter(fs, Encoding.UTF8)
			'テキストファイル書込み
			If iLTDataPointNum = 0
				Dim sHead() As String = Nothing
				p_head.sFileName = m_CurrDataFileName
				MeasFileHeaderMake(iType, sHead)				'ヘッダ情報作成
				'ヘッダ情報書込み
				sw.WriteLine(cHeader)
				For i = 0 To sHead.GetUpperBound(0)
					sw.WriteLine(sHead(i))
				Next
				Dim sLine As String = sComnt
				sLine = sLine & p_sHeaderItemName5
				sw.WriteLine(cData)
				sw.WriteLine(sLine)
			End If
			i = iLTDataPointNum
			'経過時間(秒),HP温度,HP温度操作量,PB温度,PB温度操作量,応力,応力%,位置位相,粘度,動粘弾性X,サンプル歪,せん断弾性率(Pa),X貯蔵弾性率(Pa),X損失弾性率(Pa),X損失正接
			sLineBuf = _
						iLT_MSec(i).ToString & "," & _
						iLT_TempBuf(i).ToString & "," & _
						iLT_MvBuf(i).ToString & "," & _
						iLT_Tempbuf2(i).ToString & "," & _
						iLT_Mvbuf2(i).ToString & "," & _
						iLT_Hosei(i).ToString & "," & _
						dLT_Hosei100(i).ToString & "," & _
						dLT_Viscosity(i).ToString & "," & _
						dLT_XPosPhase(i).ToString & "," & _
						dLT_SplStrain(i).ToString & "," & _
						dLT_ShearMod(i).ToString & "," & _
						dLT_ShearVelo(i).ToString & "," & _
						dLT_XStrgMod(i).ToString & "," & _
						dLT_XElasMod(i).ToString & "," & _
						dLT_XLossTan(i).ToString
			sw.WriteLine(sLineBuf)											'一行書き込み

			sw.Close()
			sw = nothing
			fs.Close()
			fs = Nothing

		End Select

	Case False		'上書き(全計算後の)
		Select Case iType
		Case RealtimeMeas.eMeasMode.Normal,RealtimeMeas.eMeasMode.FlowCurve_Time,RealtimeMeas.eMeasMode.NormalNoAcum
			Dim fs As FileStream	= Nothing
			Dim sw As Textwriter	= Nothing

			If IsNothing(iHosei)
				Return -2
			End If
			If IsNothing(bItem)
			'項目選択なし-----------------------------------------------------------------------------------
				Array.Resize(iMSec, iHosei.Length)
				For i = 0 To iHosei.GetUpperBound(0) Step iThinning
					'テキストファイル書込み
					If i Mod cMesFileLineNum = 0
					'ファイル切替わり
						If Not IsNothing(sw)
							sw.Close()
							sw = Nothing
							fs.Close()
							fs = Nothing
						End If
						m_iMesFileNumber = i \ cMesFileLineNum							'測定ファイル連番
						If IsNothing(sPath)
							SetDataFileName = m_CurrDataFileName						'測定ファイル名セット
						ElseIf IsNothing(sFileName)
						Else
							SetDataFileName = sFileName									'測定ファイル名セット
						End If
						'書き込みパスを決定する
						iRet = RealTimeMeasDataFilePath(sFilePath, sPath)
						If iRet <> 0
							Return iRet
						End If
						Dim sFileFullPath As String = Nothing
						If sFilePath.IndexOf("." & sDataExt) < 0
							sFileFullPath = Path.Combine(sFilePath, m_CurrDataFileName)
						Else
							sFileFullPath = sFilePath 
						End If
						fs = New FileStream(sFileFullPath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite)
						sw = New StreamWriter(fs, Encoding.UTF8)
						Dim sHead() As String = Nothing
						p_head.sFileName = IF(m_CurrDataFileName="", sPath, m_CurrDataFileName)
						MeasFileHeaderMake(iType, sHead)				'ヘッダ情報作成
						'ヘッダ情報書込み
						sw.WriteLine(cHeader)
						For j = 0 To sHead.GetUpperBound(0)
							sw.WriteLine(sHead(j))
						Next
						Dim sLine As String = sComnt
						sLine = sLine & p_sHeaderItemName1
						If cts.CalcResultEnable = 1
							sLine = sLine & "," & p_sHeaderItemName2
						End If
						sw.WriteLine(cData)
						sw.WriteLine(sLine)
					End If
					'データ部の書き込み
					'経過時間,応力X,応力Y,応力Z,X軸位置,Y軸位置,温度,温度操作量,
					'補正値(uN),補正値(%),補正値粘度,動粘弾性X,動粘弾性Y,サンプル歪,せん断弾性率(Pa),せん断速度(1/S),X貯蔵弾性率(Pa),X損失弾性率(Pa),X損失正接
					iMSec(i) = i * 2
					sLineBuf = _
							iMSec(i).ToString  & "," &  _
							iOxBuf(i).ToString & "," &  _
							iOyBuf(i).ToString & "," &  _
							iOzBuf(i).ToString & "," &  _
							iPxBuf(i).ToString & "," &  _
							iPyBuf(i).ToString & "," &  _
							iPzBuf(i).ToString & "," &  _
							iTempbuf(i).ToString & "," &  _
							iMvbuf(i).ToString & "," & _
							iTempbuf2(i).ToString & "," &  _
							iMvbuf2(i).ToString
					If cts.CalcResultEnable = 1
						sLineBuf = sLineBuf & "," & _
							iHosei(i).ToString & "," & _
							dHosei100(i).ToString & "," & _
							dViscosity(i).ToString & "," & _
							(dXPosPhase(i)*RadToDegRate).ToString & "," & _
							dSplStrain(i).ToString & "," & _
							dShearMod(i).ToString & "," & _
							dShearVelo(i).ToString & "," & _
							dXStrgMod(i).ToString & "," & _
							dXElasMod(i).ToString & "," & _
							dXLossTan(i).ToString
					End If
					sw.WriteLine(sLineBuf)															'一行書き込み
				Next i
			Else
			'エクスポート時 --------------------------------------------------------------------------------------------
				Array.Resize(iMSec, iHosei.Length)
				For i = 0 To iHosei.GetUpperBound(0) Step iThinning 
					'テキストファイル書込み
					If i = 0
					'初回
						If Not IsNothing(sw)
							sw.Close()
							sw = Nothing
							fs.Close()
							fs = Nothing
						End If
						m_iMesFileNumber = i \ cMesFileLineNum							'測定ファイル連番
						If IsNothing(sPath)
							SetDataFileName = m_CurrDataFileName						'測定ファイル名セット
						ElseIf IsNothing(sFileName)
						Else
							SetDataFileName = sFileName									'測定ファイル名セット
						End If
						'書き込みパスを決定する
						iRet = RealTimeMeasDataFilePath(sFilePath, sPath)
						If iRet <> 0
							Return iRet
						End If
						fs = New FileStream(sFilePath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite)
						sw = New StreamWriter(fs, Encoding.UTF8)
						Dim sHead() As String = Nothing
						p_head.sFileName = IF(m_CurrDataFileName="", sPath, m_CurrDataFileName)
						MeasFileHeaderMake(iType, sHead)				'ヘッダ情報作成
						'ヘッダ情報書込み
						sw.WriteLine(cHeader)
						For j = 0 To sHead.GetUpperBound(0)
							sw.WriteLine(sHead(j))
						Next
						Dim sLine As String = sComnt
						Dim sItem() As String = Nothing
						ItemArrayMake(sItem)
						For j=0 To bItem.GetUpperBound(0)
							If bItem(j)
								sLine = sLine & If(j=0, "", ",") & sItem(j)
							End If
						Next j
						sw.WriteLine(cData)
						sw.WriteLine(sLine)
					End If
					'データ部の書き込み(選択された項目)
					iMSec(i) = i * 2
					sLineBuf = ""
					For j=0 To bItem.GetUpperBound(0)
						If bItem(j)
							Dim s As String = ""
							OneDataMake(j, i, s)													'数値文字列フォーマット
							sLineBuf = sLineBuf & If(j=0, "", ",") & s
						End If
					Next j
					sw.WriteLine(sLineBuf)															'一行書き込み
					If i-1 Mod cMesFileLineNum = 0
					End If
				Next i
				'--------------------------------------------------------------------------------------------------
			End If
			sw.Close()
			sw = Nothing
			fs.Close()
			fs = Nothing



		Case RealtimeMeas.eMeasMode.Calib
			Dim fs As FileStream	= Nothing
			Dim sw As Textwriter	= Nothing
			'項目選択なし-----------------------------------------------------------------------------------
			Array.Resize(iMSec, iHosei.Length)
			For i = 0 To iHosei.GetUpperBound(0) Step iThinning
				'テキストファイル書込み
				If i Mod cMesFileLineNum = 0
				'ファイル切替わり
					If Not IsNothing(sw)
						sw.Close()
						sw = Nothing
						fs.Close()
						fs = Nothing
					End If
					m_iMesFileNumber = i \ cMesFileLineNum							'測定ファイル連番
					If IsNothing(sPath)
						SetDataFileName = m_CurrDataFileName						'測定ファイル名セット
					ElseIf IsNothing(sFileName)
					Else
						SetDataFileName = sFileName									'測定ファイル名セット
					End If
					'書き込みパスを決定する
					iRet = RealTimeMeasDataFilePath(sFilePath, sPath, , iCalMode)
					If iRet <> 0
						Return iRet
					End If
					Dim sFileFullPath As String = Nothing
					If sFilePath.IndexOf("." & sDataExt) < 0
						sFileFullPath = Path.Combine(sFilePath, m_CurrDataFileName)
					Else
						sFileFullPath = sFilePath 
					End If
					If p_bCalibFileOutput
						fs = New FileStream(sFileFullPath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite)
						sw = New StreamWriter(fs, Encoding.UTF8)
					End If
					Dim sHead() As String = Nothing
					p_head.sFileName = IF(m_CurrDataFileName="", sPath, m_CurrDataFileName)

					MeasFileHeaderMake(iType, sHead)				'ヘッダ情報作成
					If p_bCalibFileOutput
						'ヘッダ情報書込み
						sw.WriteLine(cHeader)
						For j = 0 To sHead.GetUpperBound(0)
							sw.WriteLine(sHead(j))
						Next
					End If
					Dim sLine As String = sComnt
					sLine = sLine & p_sHeaderItemName1
					If cts.CalcResultEnable = 1
						sLine = sLine & "," & p_sHeaderItemName2
					End If
					If p_bCalibFileOutput
						sw.WriteLine(cData)
						sw.WriteLine(sLine)
					End If
				End If
				'データ部の書き込み
				'経過時間,応力X,応力Y,応力Z,X軸位置,Y軸位置,温度,温度操作量,
				'補正値(uN),補正値(%),補正値粘度,動粘弾性X,動粘弾性Y,サンプル歪,せん断弾性率(Pa),せん断速度(1/S),X貯蔵弾性率(Pa),X損失弾性率(Pa),X損失正接
				iMSec(i) = i * 2
				sLineBuf = _
						iMSec(i).ToString  & "," &  _
						iOxBuf(i).ToString & "," &  _
						iOyBuf(i).ToString & "," &  _
						iOzBuf(i).ToString & "," &  _
						iPxBuf(i).ToString & "," &  _
						iPyBuf(i).ToString & "," &  _
						iPzBuf(i).ToString & "," &  _
						iTempbuf(i).ToString & "," &  _
						iMvbuf(i).ToString & "," & _
						iTempbuf2(i).ToString & "," &  _
						iMvbuf2(i).ToString
				If cts.CalcResultEnable = 1
					sLineBuf = sLineBuf & "," & _
						iHosei(i).ToString & "," & _
						dHosei100(i).ToString("F4") & "," & _
						dViscosity(i).ToString("F1") & "," & _
						(dXPosPhase(i)*RadToDegRate).ToString("F1") & "," & _
						dSplStrain(i).ToString("F3") & "," & _
						dShearMod(i).ToString("F1") & "," & _
						dShearVelo(i).ToString("F2") & "," & _
						dXStrgMod(i).ToString("F1") & "," & _
						dXElasMod(i).ToString("F1") & "," & _
						dXLossTan(i).ToString("F3")
				End If
				If p_bCalibFileOutput
					sw.WriteLine(sLineBuf)															'一行書き込み
				End If
			Next i
			If p_bCalibFileOutput
				sw.Close()
				sw = Nothing
				fs.Close()
				fs = Nothing
			End If
		Case RealtimeMeas.eMeasMode.FlowCurve_Speed
		'フローカーブ 速度依存
			'書き込みパスを決定する
			iRet = RealTimeMeasDataFilePath(sFilePath, sPath)
			If iRet <> 0
				Return iRet
			End If
			Dim fs As New FileStream(sFilePath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite)
			Dim sw As Textwriter = New StreamWriter(fs, Encoding.UTF8)
			Dim sHead() As String = Nothing
			Dim sClmHead As String = Nothing
			'テキストファイル書込み
			MeasFileHeaderMake(iType, sHead)				'ヘッダ情報作成
			'ヘッダ情報書込み
			sw.WriteLine(cHeader)
			For i = 0 To sHead.GetUpperBound(0)
				sw.WriteLine(sHead(i))
			Next
			sw.WriteLine(cData)		'データ部
			sClmHead = sComnt & p_sHeaderItemName3
			sw.WriteLine( sClmHead )

			For i = 0 To uVis1.GetUpperBound(0)
				sLineBuf = _
							(uVis1(i).dAmplitude*1000).Tostring("F3")	& "," & _				'設定振幅(mm)
							uVis1(i).dMeasStress.Tostring()				& "," & _ 				'測定応力(N)
							uVis1(i).dShearVelocity.Tostring("F1")		& "," & _ 				'せん断速度(1/S)
							uVis1(i).dShearStress.Tostring("F1")		& "," & _				'せん断応力(Pa)
							uVis1(i).dViscosity.Tostring("F1")			& "," & _				'粘度(Pa･S)
							(uVis1(i).dXPosPhase*RadToDegRate).ToString("F1") & "," & _			'動粘弾性X(deg)
							uVis1(i).dSplStrain.ToString("F3")			& "," & _				'サンプル歪(Pa)
							uVis1(i).dShearMod.ToString("F1")			& "," & _				'せん断弾性率(Pa)
							uVis1(i).dXStrgMod.ToString("F1")			& "," & _				'X貯蔵弾性率(Pa)
							uVis1(i).dXElasMod.ToString("F1")			& "," & _				'X損失弾性率(Pa)
							uVis1(i).dXLossTan.ToString("F3")			& "," & _				'X損失正接
							uVis1(i).dHPTemp.Tostring("F1")				& "," & _				'HP温度(℃)
							uVis1(i).dPBTemp.Tostring("F1")		 								'PB温度(℃)
				sw.WriteLine(sLineBuf)
			Next i
			sw.Close()
			sw = Nothing

		Case RealtimeMeas.eMeasMode.FlowCurve_Temp
		'フローカーブ 温度依存
			'書き込みパスを決定する
			iRet = RealTimeMeasDataFilePath(sFilePath, sPath)
			If iRet <> 0
				Return iRet
			End If
			Dim fs As New FileStream(sFilePath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite)
			Dim sw As Textwriter = New StreamWriter(fs, Encoding.UTF8)
			Dim sHead() As String = Nothing
			Dim sClmHead As String = Nothing
			'テキストファイル書込み
			MeasFileHeaderMake(iType, sHead)				'ヘッダ情報作成
			'ヘッダ情報書込み
			sw.WriteLine(cHeader)
			For i = 0 To sHead.GetUpperBound(0)
				sw.WriteLine(sHead(i))
			Next
			sw.WriteLine(cData)		'データ部

			sClmHead = sComnt & p_sHeaderItemName4
			sw.WriteLine( sClmHead )

			For i = 0 To uVis1.GetUpperBound(0)
				sLineBuf = _
							uVis1(i).dTemp.Tostring("F1")		& "," & _						'設定温度(℃)
							uVis1(i).dMeasStress.Tostring()		& ","& _ 						'測定応力(N)
							uVis1(i).dShearStress.Tostring()	& "," & _						'せん断応力(Pa)
							uVis1(i).dViscosity.Tostring("F1")	& "," & _						'粘度(Pa･S)
							(uVis1(i).dXPosPhase*RadToDegRate).ToString("F2") & "," & _			'動粘弾性X(deg)
							uVis1(i).dSplStrain.ToString("F3")	& "," & _						'サンプル歪(Pa)
							uVis1(i).dShearMod.ToString("F1")	& "," & _						'せん断弾性率(Pa)
							uVis1(i).dXStrgMod.ToString("F1")	& "," & _						'X貯蔵弾性率(Pa)
							uVis1(i).dXElasMod.ToString("F1")	& "," & _						'X損失弾性率(Pa)
							uVis1(i).dXLossTan.ToString("F3")	& "," & _						'X損失正接
							uVis1(i).dHPTemp.Tostring("F1")		& "," & _						'HP温度(℃)
							uVis1(i).dPBTemp.Tostring("F1")		 								'PB温度(℃)
				sw.WriteLine(sLineBuf)
			Next i
			sw.Close()
			sw = Nothing

		Case RealTimeMeas.eMeasMode.NormalLT
			Dim fs As FileStream	= Nothing
			Dim cs As CryptoStream	= Nothing
			Dim sw As Textwriter	= Nothing

			m_iMesFileNumber = j \ cMesFileLineNum							'測定ファイル連番
			SetDataFileName = m_CurrDataFileName							'測定ファイル名セット
			'書き込みパスを決定する
			iRet = RealTimeMeasDataFilePath(sFilePath, sPath)
			If iRet <> 0
				Return iRet
			End If

			'ファイルストリームオープン 追加書込みモード
			fs = New FileStream(sFilePath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite)
			sw = New StreamWriter(fs, Encoding.UTF8)
			'テキストファイル書込み
			Dim sHead() As String = Nothing
			p_head.sFileName = m_CurrDataFileName
			MeasFileHeaderMake(iType, sHead)				'ヘッダ情報作成
			'ヘッダ情報書込み
			sw.WriteLine(cHeader)
			For i = 0 To sHead.GetUpperBound(0)
				sw.WriteLine(sHead(i))
			Next
			Dim sLine As String = sComnt
			sLine = sLine & p_sHeaderItemName5
			sw.WriteLine(cData)
			sw.WriteLine(sLine)
			For i = 0 To iLT_MSec.GetUpperBound(0)
				'経過時間(秒),HP温度,HP温度操作量,PB温度,PB温度操作量,応力,応力%,位置位相,粘度,動粘弾性X,サンプル歪,せん断弾性率(Pa),X貯蔵弾性率(Pa),X損失弾性率(Pa),X損失正接
				sLineBuf = _
						iLT_MSec(i).ToString & "," & _
						iLT_TempBuf(i).ToString & "," & _
						iLT_MvBuf(i).ToString & "," & _
						iLT_Tempbuf2(i).ToString & "," & _
						iLT_Mvbuf2(i).ToString & "," & _
						iLT_Hosei(i).ToString & "," & _
						dLT_Hosei100(i).ToString & "," & _
						dLT_Viscosity(i).ToString & "," & _
						dLT_XPosPhase(i).ToString & "," & _
						dLT_SplStrain(i).ToString & "," & _
						dLT_ShearMod(i).ToString & "," & _
						dLT_ShearVelo(i).ToString & "," & _
						dLT_XStrgMod(i).ToString & "," & _
						dLT_XElasMod(i).ToString & "," & _
						dLT_XLossTan(i).ToString
				sw.WriteLine(sLineBuf)											'一行書き込み
			Next
			sw.Close()
			sw = nothing
			fs.Close()
			fs = Nothing
		Case Else
		End Select
	End Select


	Return 0
Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Return -1
End Try
End Function
'Export 出力項目配列を作成する
Public Sub ItemArrayMake( ByRef d() As String )
	Dim sItemName1() As String = p_sHeaderItemName1.Split(",")
	Dim sItemName2() As String = p_sHeaderItemName2.Split(",")
	Array.Resize(d, sItemName1.Length + sItemName2.Length)
	Array.Copy(sItemName1, d, sItemName1.Length)
	Array.Copy(sItemName2, 0, d, sItemName1.Length, sItemName2.Length)
End Sub
'Export 順番より１数値を文字列へフォーマットする
Public Sub OneDataMake( ByVal idx As Int32, ByVal iLine As Int32, ByRef d As String )
	Select Case idx
	Case eDataColmnGel.MSec
		d = iMSec(iLine).ToString

	Case eDataColmnGel.XSrc
		d = iOxBuf(iLine).ToString

	Case eDataColmnGel.YSrc
		d = iOyBuf(iLine).ToString

	Case eDataColmnGel.ZSrc
		'd = dOzBuf(iLine).ToString
		d = iOzBuf(iLine).ToString

	Case eDataColmnGel.XPos
		d = iPxBuf(iLine).ToString

	Case eDataColmnGel.YPos 
		d = iPyBuf(iLine).ToString

	Case eDataColmnGel.ZPos 
		d = iPzBuf(iLine).ToString

	Case eDataColmnGel.Temp
		d = iTempbuf(iLine).ToString

	Case eDataColmnGel.Mv
		d = iMvbuf(iLine).ToString

	Case eDataColmnGel.Temp2
		d = iTempbuf2(iLine).ToString

	Case eDataColmnGel.Mv2
		d = iMvbuf2(iLine).ToString

	Case eDataColmnGel.Hosei
		d = iHosei(iLine).ToString

	Case eDataColmnGel.HoseiP
		d = dHosei100(iLine).ToString

	Case eDataColmnGel.Visco
		d = dViscosity(iLine).ToString


#If 0
	Case eDataColmnGel.XPosPhase
		d = (dXPosPhase(iLine)*RadToDegRate).ToString("F1")

	Case eDataColmnGel.SampleStrain
		d = dSplStrain(iLine).ToString("F3")

	Case eDataColmnGel.ShearMod
		d = dShearMod(iLine).ToString("F1")

	Case eDataColmnGel.ShearVelo
		d = dShearVelo(iLine).ToString("F2")

	Case eDataColmnGel.XStorageMod
		d = dXStrgMod(iLine).ToString("F1")

	Case eDataColmnGel.XElasticMod
		d = dXElasMod(iLine).ToString("F1")

	Case eDataColmnGel.XLossTan
		d = dXLossTan(iLine).ToString("F3")
#else
	Case eDataColmnGel.XPosPhase
		d = (dXPosPhase(iLine)*RadToDegRate).ToString

	Case eDataColmnGel.SampleStrain
		d = dSplStrain(iLine).ToString

	Case eDataColmnGel.ShearMod
		d = dShearMod(iLine).ToString

	Case eDataColmnGel.ShearVelo
		d = dShearVelo(iLine).ToString

	Case eDataColmnGel.XStorageMod
		d = dXStrgMod(iLine).ToString

	Case eDataColmnGel.XElasticMod
		d = dXElasMod(iLine).ToString

	Case eDataColmnGel.XLossTan
		d = dXLossTan(iLine).ToString
#End If
	End Select
End Sub

''' <summary>
'''	書き込んだ結果のファイル行数計算
''' </summary>
''' <param name="iAllLen"></param>
''' <param name="j"></param>
Public Sub RealTimeGetSampleState(ByRef iAllLen As Uint64, ByRef j As Uint64, ByRef iElapseTime As Uint64)
Try
	If IsNothing(iOxBuf)
		iAllLen = 0 + p_iPwX.Length
		j = 0
		iElapseTime = 0
	Else
		iAllLen = iOxBuf.Length + p_iPwX.Length
		j = iOxBuf.Length
		iElapseTime = iOxBuf.Length
	End If
Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub
''' <summary>
''' 受信ブロックデータを蓄積領域へ転送
''' </summary>
''' <param name="i">受信ブロックインデックス</param>
''' <param name="iLastEndIndex">蓄積ブロック終端インデックス</param>
Public Sub RealTimeAccumulationArrayCopy(ByVal i As Int64, ByVal iLastEndIndex As Int64 )
Dim k As Int64
Try
	k = iLastEndIndex + i
	iOxBuf(k)		= p_iPwX(i)
	iOyBuf(k)		= p_iPwY(i)
	iOzBuf(k)		= p_iPwZ(i)
	dOzBuf(k)		= p_dPwZ(i)
	iPxBuf(k)		= p_iPOSX(i)
	iPyBuf(k)		= p_iPOSY(i)
	iPzBuf(k)		= p_iPOSZ(i)
	iTempBuf(k)		= p_iTemp(i)
	iMvBuf(k)		= p_iMv(i)
	iTempBuf2(k)	= p_iTemp2(i)
	iMvBuf2(k)		= p_iMv2(i)
	iHosei(k)		= p_findat(i)
	DataCount += 1							'蓄積データカウント更新

Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub
''' <summary>
''' 分割測定ファイルの読み込み （sFilePath配列の順番で読み込む）
''' </summary>
''' <param name="iArea">0:Real 1:Std1 2:Std2</param>
''' <param name="sFilePath"></param>
''' <param name="bCalc">T:計算データあり F:計算データなし</param>
''' <param name="sHead"></param>
''' <returns>-1:例外発生 -2:項目転送時例外 -3:サンプル時間間隔が2mSec以外</returns>
Public Function RealTimeMeasDataFileReadText(ByVal iArea As Int32, ByVal sFilePath() As String, Byref bCalc As Boolean, Optional ByRef sHead As DataFileHeader = Nothing) As Int32
Dim i			As Int32
Dim j			As Int32
Dim k			As Int32
Dim fi			As int32 = 0
Dim iMode		As Int16 = 0
Dim sLineBuf	As String
Dim sBuf()		As String = Nothing
Dim fs			As FileStream = Nothing
Dim sr			As TextReader = Nothing
Dim iSerial		As Int32
Dim iDCount		As Int32 = 0
Dim iRet		As Int32
Try
	
	bCalc = False
#If 0
	Select Case iArea
	Case 0
	Case 1
		'基準データFIFO処理
		DataArrayFiFoStd()
	End Select
#end If
	Select Case iArea
	Case 0		'Real
		p_head = Nothing
	Case 1, 2	'STD1,2
		p_head_s(iArea-1) = Nothing
	End Select

	
	Array.Sort(sFilePath)		'複数選択時のソート

	'読込処理
	i = 0
	For fi = 0 To sFilePath.GetUpperBound(0)
		Dim sDirName  As String = Path.GetDirectoryName(sFilePath(fi))
		Dim sFileName As String = Path.GetFileName(sFilePath(fi))
		iDCount += GetFileLineCount(sDirName, sFileName)			'データ行カウント
		If iDCount < 0
			Continue For
		Else
			Select Case iArea
			Case 0			'測定データ読込領域確保
				Select Case sFileName.Substring(0, 3)
			    Case sMeasindCode(eMesCD.LTM)
					LTDataArrayResize(iArea, iDCount)
				Case Else
					ResizeMeasArray(iDCount)
				End Select
			Case 1, 2		'基準データ読込領域確保
				Select Case sFileName.Substring(0, 3)
			    Case sMeasindCode(eMesCD.LTM)
					LTDataArrayResize(iArea, iDCount)
				Case Else
					StdReadAreaAlloc(iArea, iDCount)
				End Select
			End Select
		End If
		fs = New FileStream(sFilePath(fi), FileMode.Open, FileAccess.Read,FileShare.ReadWrite)
		sr = New StreamReader(fs, Encoding.UTF8)
		'ファイル連番切り出し
		GetFileSerial(iSerial, sFilePath(fi))
		j = 0
		Do 
			sLineBuf = sr.ReadLine()
			If IsNothing(sLineBuf)
				Exit Do
			End If
			sBuf = sLineBuf.Split(",")
			Select Case sBuf(0)
			Case cHeader
				iMode = 0		'ヘッダ部
				Continue Do
			Case cData
				iMode = 1		'データ部
				Continue Do
			End Select

			Select Case iMode
			Case 0
				If fi = 0
				'一個目のファイル
				'ヘッダ部の読込
					If sLineBuf.Substring(0,1) = "["	'セクション行スキップ
						Continue Do
					End If
					If sLineBuf.Substring(0,1) = "'"	'コメント行スキップ
						Continue Do
					End If
					If sLineBuf.Length=0				'空行スキップ
						Continue Do
					End If
					sBuf = Split(sLineBuf, ",""")		'sLineBuf.Splitでは正常な動作をしない
					For k=0 To m_HeadItem.GetUpperBound(0)
						if m_HeadItem(k) = sBuf(0)
							'項目名が見つかった
							Exit For
						End If
					Next
					If k>m_HeadItem.GetUpperBound(0)
						Continue Do
					End If
					Dim sItm As String = sBuf(1)
					sItm = sItm.Replace("""", "")									'ダブルコーテーション囲いを削除
					Select Case iArea
					Case 0		'Real
						HeaderItemStore(p_head,		sItm, k)						'ヘッダ(DataFileHeader)へ値をセット
					Case 1, 2	'STD1,2
						HeaderItemStore(p_head_s(iArea-1),	sItm, k)				'ヘッダ(DataFileHeader)へ値をセット
					End Select



					j += 1
				End If
			Case 1
			'データ部の読込
				If sLineBuf.Substring(0,1) = "'"
					Continue Do
				End If
				If sLineBuf.Length=0			'空行スキップ
					Continue Do
				End If
				sBuf = sLineBuf.Split(",")
				If iArea = 0
					Select Case p_head.sMeasureType
					'Case "GEL"
					Case sMeasindCode(eMesCD.GEL), sMeasindCode(eMesCD.GLA)
						'測定値読込時
						'サンプル間の時間をチェック2以外はNG
						If i>= 2
							If int64.Parse(sBuf(eDataColmnGel.MSec)) - iMsec(i-1) <> 2
								Return -3
							End If
						End If
						'列長さを検査 補正計算値があれば計算データ含むと判断
						If sBuf.Length > eDataColmnGel.Hosei
							If sBuf(eDataColmnGel.Hosei).Length > 0
								bCalc = True			'計算済データ有り
							End If
						End If
					Case Else
					End Select
				End If
				'値読込領域へ転送
				Select Case iArea
				Case 0		'Real
					iRet = ReadAreaSet(iArea, i, sBuf, p_head, bCalc)
				Case 1, 2	'STD1,2
					iRet = ReadAreaSet(iArea, i, sBuf, p_head_s(iArea-1), bCalc)
				End Select
				'iRet = ReadAreaSet(iArea, i, sBuf, p_head, bCalc)
				If iRet < 0
					Return -2
				End If

				i += 1
			End Select
		Loop Until sLineBuf Is Nothing


		'新規に追加されたヘッダ項目で旧形式ファイルの時の初期値を設定する(最終的には削除)
		Select Case iArea
		Case 0		'Real
			If IsNothing(p_head.sFullStressPhaseZ)
				p_head.sFullStressPhaseZ = p_head.sFullStressPhaseX
			End If
		Case 1, 2	'STD1,2
			If IsNothing(p_head_s(iArea-1).sFullStressPhaseZ)
				p_head_s(iArea-1).sFullStressPhaseZ = p_head_s(iArea-1).sFullStressPhaseX
			End If
		End Select



		sr.Close()
		sr.Dispose()
		fs.Close()
		fs.Dispose()
	Next fi
	Select Case iArea
	Case 0	'Real
		sHead = p_head
	Case 1, 2	'Standard
		sHead = p_head_s(iArea-1)
	End Select
	Return 0
Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Return -1
End Try
End Function
'ファイル連番切り出し
Private Sub	GetFileSerial(ByRef iSerial As Int32,  ByVal sFilePath As String)
Try
	Dim sBuf() As String = Nothing
	Dim sFileName As String = Path.GetFileName(sFilePath)
	sBuf = sFileName.Split("#")
	sBuf = sBuf(1).Split(".")
	iSerial = Int32.Parse(sBuf(0))
Catch ex As Exception
	iSerial = 0
	Exit Sub
End Try
End Sub
''' <summary>
''' 基準値読込領域確保
''' </summary>
''' <param name="iArea">STD1/2</param>
''' <param name="iDCount"></param>
Private Sub StdReadAreaAlloc(ByVal iArea As int32, ByVal iDCount As int32)
Try
	Select Case iArea
	Case 1
		Array.Resize(iOxBuf_s1,		iDCount)
		Array.Resize(iOyBuf_s1,		iDCount)
		Array.Resize(iOzBuf_s1,		iDCount)
		Array.Resize(dOzBuf_s1,		iDCount)
		Array.Resize(iPxBuf_s1,		iDCount)
		Array.Resize(iPyBuf_s1,		iDCount)
		Array.Resize(iPzBuf_s1,		iDCount)
		Array.Resize(iTempBuf_s1,	iDCount)
		Array.Resize(iMvBuf_s1,		iDCount)
		Array.Resize(iTempBuf2_s1,	iDCount)
		Array.Resize(iMvBuf2_s1,	iDCount)

		Array.Resize(iHosei_s1,		iDCount)
		Array.Resize(dHoseiP_s1,	iDCount)
		Array.Resize(dViscosity_s1,	iDCount)
		Array.Resize(dXPosPhase_s1,	iDCount)
		Array.Resize(dSplStrain_s1,	iDCount)
		Array.Resize(dShearMod_s1,	iDCount)
		Array.Resize(dShearVelo_s1,	iDCount)
		Array.Resize(dXStrgMod_s1,	iDCount)
		Array.Resize(dXElasMod_s1,	iDCount)
		Array.Resize(dXLossTan_s1,	iDCount)
		Array.Resize(iSlope_s1,		iDCount)

	Case 2
		Array.Resize(iOxBuf_s2,		iDCount)
		Array.Resize(iOyBuf_s2,		iDCount)
		Array.Resize(iOzBuf_s2,		iDCount)
		Array.Resize(dOzBuf_s2,		iDCount)
		Array.Resize(iPxBuf_s2,		iDCount)
		Array.Resize(iPyBuf_s2,		iDCount)
		Array.Resize(iPzBuf_s2,		iDCount)
		Array.Resize(iTempBuf_s2,	iDCount)
		Array.Resize(iMvBuf_s2,		iDCount)
		Array.Resize(iTempBuf2_s2,	iDCount)
		Array.Resize(iMvBuf2_s2,	iDCount)

		Array.Resize(iHosei_s2,		iDCount)
		Array.Resize(dHoseiP_s2,	iDCount)
		Array.Resize(dViscosity_s2,	iDCount)
		Array.Resize(dXPosPhase_s2,	iDCount)
		Array.Resize(dSplStrain_s2,	iDCount)
		Array.Resize(dShearMod_s2,	iDCount)
		Array.Resize(dShearVelo_s2,	iDCount)
		Array.Resize(dXStrgMod_s2,	iDCount)
		Array.Resize(dXElasMod_s2,	iDCount)
		Array.Resize(dXLossTan_s2,	iDCount)
		Array.Resize(iSlope_s2,		iDCount)

	End Select
Catch ex As Exception
	Exit Sub
End Try
End Sub




''' <summary>
''' 読込値を転送
''' </summary>
''' <param name="iArea">0:Real 1:Std</param>
''' <param name="i"></param>
''' <param name="sBuf"></param>
''' <param name="hd"></param>
''' <param name="bCalc">計算済のデータか False：未計算</param>
''' <returns></returns>
Private Function ReadAreaSet(ByVal iArea As Int32, ByVal i As int32, ByRef sBuf() As String, ByVal hd As DataFileHeader, ByVal bCalc As Boolean) As int32
Try
	Select Case iArea
	Case 0
		Select Case  hd.sMeasureType
		Case sMeasindCode(eMesCD.GEL), sMeasindCode(eMesCD.GLA)
			'経過時間
			iMSec(i)				= Int64.Parse(sBuf(eDataColmnGel.MSec))
			'応力X
			iOxBuf(i)				= Int64.Parse(sBuf(eDataColmnGel.XSrc))
			'応力Y
			iOyBuf(i)				= Int64.Parse(sBuf(eDataColmnGel.YSrc))
#If 1			'Z応力のデジタル
#If 0			'Z応力のデジタル
		'応力Z
		iOzBuf(i)				= Int64.Parse(sBuf(eDataColmnGel.ZSrc))
		'応力Z(g)
		dOzBuf(i)				= iOzBuf(i) * hd.dZWeightCoef
#else
			If sBuf(eDataColmnGel.ZSrc).Contains(".")
				'応力Z(g)
				dOzBuf(i)				= Double.Parse(sBuf(eDataColmnGel.ZSrc))
				'応力Z
				iOzBuf(i)				= dOzBuf(i) / hd.dZWeightCoef
			Else
				'応力Z
				iOzBuf(i)				= Int64.Parse(sBuf(eDataColmnGel.ZSrc))
				'応力Z(g)
				dOzBuf(i)				= iOzBuf(i) * hd.dZWeightCoef
			End If
#end if

#else
	'ファイル側がDouble型の時
		'応力Z(g)
		dOzBuf(i)				= Double.Parse(sBuf(eDataColmnGel.ZSrc))
		'応力Z
		iOzBuf(i)				= dOzBuf(i) / hd.dZWeightCoef
#End if
			'X軸位置
			iPxBuf(i)				= Int64.Parse(sBuf(eDataColmnGel.XPos))
			'Y軸位置
			iPyBuf(i)				= Int64.Parse(sBuf(eDataColmnGel.YPos))
			'Z軸位置
			iPzBuf(i)				= Int64.Parse(sBuf(eDataColmnGel.ZPos))
			'温度
			iTempBuf(i)				= Int64.Parse(sBuf(eDataColmnGel.Temp))
			'操作量
			iMvBuf(i)				= Int64.Parse(sBuf(eDataColmnGel.Mv))
			'温度
			iTempBuf2(i)			= Int64.Parse(sBuf(eDataColmnGel.Temp2))
			'操作量
			iMvBuf2(i)				= Int64.Parse(sBuf(eDataColmnGel.Mv2))
			'If sBuf.Length > eDataColmnGel.Hosei
			If bCalc
				'補正値
				iHosei(i)			= Double.Parse(sBuf(eDataColmnGel.Hosei))
				'補正値
				dHosei100(i)		= Double.Parse(sBuf(eDataColmnGel.HoseiP))
				'粘度
				dViscosity(i)		= Double.Parse(sBuf(eDataColmnGel.Visco))
				'動粘弾性X
				dXPosPhase(i)		= Double.Parse(sBuf(eDataColmnGel.XPosPhase)) * DegToRadRate
				'サンプル歪
				dSplStrain(i)		= Double.Parse(sBuf(eDataColmnGel.SampleStrain))
				'せん断弾性率(Pa)
				dShearMod(i)		= Double.Parse(sBuf(eDataColmnGel.ShearMod))
				'せん断速度(1/S)
				dShearVelo(i)		= Double.Parse(sBuf(eDataColmnGel.ShearVelo))
				'X貯蔵弾性率(Pa)
				dXStrgMod(i)		= Double.Parse(sBuf(eDataColmnGel.XStorageMod))
				'X損失弾性率(Pa)
				dXElasMod(i)		= Double.Parse(sBuf(eDataColmnGel.XElasticMod))
				'X損失正接
				dXLossTan(i)		= Double.Parse(sBuf(eDataColmnGel.XLossTan))
			End If
		Case sMeasindCode(eMesCD.LTM)

			'経過時間
			iLT_MSec(i)			= Int64.Parse(sBuf(eDataColmnLTM.MSec))
			'温度
			iLT_TempBuf(i)			= Int64.Parse(sBuf(eDataColmnLTM.Temp))
			'操作量
			iLT_MvBuf(i)			= Int64.Parse(sBuf(eDataColmnLTM.Mv))
			'温度
			iLT_TempBuf2(i)			= Int64.Parse(sBuf(eDataColmnLTM.Temp2))
			'操作量
			iLT_MvBuf2(i)			= Int64.Parse(sBuf(eDataColmnLTM.Mv2))
			'補正値
			iLT_Hosei(i)			= Double.Parse(sBuf(eDataColmnLTM.Hosei))
			'補正値
			dLT_Hosei100(i)			= Double.Parse(sBuf(eDataColmnLTM.HoseiP))
			'粘度
			dLT_Viscosity(i)		= Double.Parse(sBuf(eDataColmnLTM.Visco))
			'動粘弾性X
			dLT_XPosPhase(i)		= Double.Parse(sBuf(eDataColmnLTM.XPosPhase)) * DegToRadRate
			'サンプル歪
			dLT_SplStrain(i)		= Double.Parse(sBuf(eDataColmnLTM.SampleStrain))
			'せん断弾性率(Pa)
			dLT_ShearMod(i)			= Double.Parse(sBuf(eDataColmnLTM.ShearMod))
			'せん断速度(1/S)
			dLT_ShearVelo(i)		= Double.Parse(sBuf(eDataColmnLTM.ShearVelo))
			'X貯蔵弾性率(Pa)
			dLT_XStrgMod(i)			= Double.Parse(sBuf(eDataColmnLTM.XStorageMod))
			'X損失弾性率(Pa)
			dLT_XElasMod(i)			= Double.Parse(sBuf(eDataColmnLTM.XElasticMod))
			'X損失正接
			dLT_XLossTan(i)			= Double.Parse(sBuf(eDataColmnLTM.XLossTan))
		End Select
	Case 1
		Select Case  hd.sMeasureType
		Case sMeasindCode(eMesCD.GEL), sMeasindCode(eMesCD.GLA)
			'経過時間
			'応力X
			iOxBuf_s1(i)			= Int64.Parse(sBuf(eDataColmnGel.XSrc))
			'応力Y
			iOyBuf_s1(i)			= Int64.Parse(sBuf(eDataColmnGel.YSrc))
			If sBuf(eDataColmnGel.ZSrc).Contains(".")
				'応力Z(g)
				dOzBuf_s1(i)			= Double.Parse(sBuf(eDataColmnGel.ZSrc))
				'応力Z
				iOzBuf_s1(i)			= dOzBuf_s1(i) / hd.dZWeightCoef
			Else
				'応力Z
				iOzBuf_s1(i)			= Int64.Parse(sBuf(eDataColmnGel.ZSrc))
				'応力Z(g)
				dOzBuf_s1(i)			= iOzBuf_s1(i) * hd.dZWeightCoef
			End If
			''応力Z
			'iOzBuf_s1(i)			= Int64.Parse(sBuf(eDataColmnGel.ZSrc))
			''応力Z(g)
			'dOzBuf_s1(i)			= iOzBuf_s1(i) * hd.dZWeightCoef
			'X軸位置
			iPxBuf_s1(i)			= Int64.Parse(sBuf(eDataColmnGel.XPos))
			'Y軸位置
			iPyBuf_s1(i)			= Int64.Parse(sBuf(eDataColmnGel.YPos))
			'Z軸位置
			iPzBuf_s1(i)			= Int64.Parse(sBuf(eDataColmnGel.ZPos))
			'温度
			iTempBuf_s1(i)			= Int64.Parse(sBuf(eDataColmnGel.Temp))
			'操作量
			iMvBuf_s1(i)			= Int64.Parse(sBuf(eDataColmnGel.Mv))
			'温度
			iTempBuf2_s1(i)			= Int64.Parse(sBuf(eDataColmnGel.Temp2))
			'操作量
			iMvBuf2_s1(i)			= Int64.Parse(sBuf(eDataColmnGel.Mv2))
			'If sBuf.Length > eDataColmnGel.Hosei
			If bCalc
				'補正値
				iHosei_s1(i)		= Double.Parse(sBuf(eDataColmnGel.Hosei))
				'補正値
				dHoseiP_s1(i)		= Double.Parse(sBuf(eDataColmnGel.HoseiP))
				'粘度
				dViscosity_s1(i)	= Double.Parse(sBuf(eDataColmnGel.Visco))
				'動粘弾性X
				dXPosPhase_s1(i)	= Double.Parse(sBuf(eDataColmnGel.XPosPhase)) * DegToRadRate
				'サンプル歪
				dSplStrain_s1(i)	= Double.Parse(sBuf(eDataColmnGel.SampleStrain))
				'せん断弾性率(Pa)
				dShearMod_s1(i)		= Double.Parse(sBuf(eDataColmnGel.ShearMod))
				'せん断速度(1/S)
				dShearVelo_s1(i)	= Double.Parse(sBuf(eDataColmnGel.ShearVelo))
				'X貯蔵弾性率(Pa)
				dXStrgMod_s1(i)		= Double.Parse(sBuf(eDataColmnGel.XStorageMod))
				'X損失弾性率(Pa)
				dXElasMod_s1(i)		= Double.Parse(sBuf(eDataColmnGel.XElasticMod))
				'X損失正接
				dXLossTan_s1(i)		= Double.Parse(sBuf(eDataColmnGel.XLossTan))
			End If
		Case sMeasindCode(eMesCD.LTM)
			'経過時間
			iMsec_s1(i)				= Int64.Parse(sBuf(eDataColmnLTM.MSec))
			'温度
			iTempBuf_s1(i)			= Int64.Parse(sBuf(eDataColmnLTM.Temp))
			'操作量
			iMvBuf_s1(i)			= Int64.Parse(sBuf(eDataColmnLTM.Mv))
			'温度
			iTempBuf2_s1(i)			= Int64.Parse(sBuf(eDataColmnLTM.Temp2))
			'操作量
			iMvBuf2_s1(i)			= Int64.Parse(sBuf(eDataColmnLTM.Mv2))
			'補正値
			iHosei_s1(i)			= Double.Parse(sBuf(eDataColmnLTM.Hosei))
			'補正値
			dHoseiP_s1(i)			= Double.Parse(sBuf(eDataColmnLTM.HoseiP))
			'粘度
			dViscosity_s1(i)		= Double.Parse(sBuf(eDataColmnLTM.Visco))
			'動粘弾性X
			dXPosPhase_s1(i)		= Double.Parse(sBuf(eDataColmnLTM.XPosPhase)) * DegToRadRate
			'サンプル歪
			dSplStrain_s1(i)		= Double.Parse(sBuf(eDataColmnLTM.SampleStrain))
			'せん断弾性率(Pa)
			dShearMod_s1(i)			= Double.Parse(sBuf(eDataColmnLTM.ShearMod))
			'せん断速度(1/S)
			dShearVelo_s1(i)		= Double.Parse(sBuf(eDataColmnLTM.ShearVelo))
			'X貯蔵弾性率(Pa)
			dXStrgMod_s1(i)			= Double.Parse(sBuf(eDataColmnLTM.XStorageMod))
			'X損失弾性率(Pa)
			dXElasMod_s1(i)			= Double.Parse(sBuf(eDataColmnLTM.XElasticMod))
			'X損失正接
			dXLossTan_s1(i)			= Double.Parse(sBuf(eDataColmnLTM.XLossTan))
		End Select
	Case 2
		Select Case  hd.sMeasureType
		Case sMeasindCode(eMesCD.GEL), sMeasindCode(eMesCD.GLA)
			'経過時間
			'応力X
			iOxBuf_s2(i)			= Int64.Parse(sBuf(eDataColmnGel.XSrc))
			'応力Y
			iOyBuf_s2(i)			= Int64.Parse(sBuf(eDataColmnGel.YSrc))
			If sBuf(eDataColmnGel.ZSrc).Contains(".")
				'応力Z(g)
				dOzBuf_s2(i)			= Double.Parse(sBuf(eDataColmnGel.ZSrc))
				'応力Z
				iOzBuf_s2(i)			= dOzBuf_s2(i) / hd.dZWeightCoef
			Else
				'応力Z
				iOzBuf_s2(i)			= Int64.Parse(sBuf(eDataColmnGel.ZSrc))
				'応力Z(g)
				dOzBuf_s2(i)			= iOzBuf_s2(i) * hd.dZWeightCoef
			End If
			''応力Z
			'iOzBuf_s2(i)			= Int64.Parse(sBuf(eDataColmnGel.ZSrc))
			''応力Z(g)
			'dOzBuf_s2(i)			= iOzBuf_s2(i) * hd.dZWeightCoef
			'X軸位置
			iPxBuf_s2(i)			= Int64.Parse(sBuf(eDataColmnGel.XPos))
			'Y軸位置
			iPyBuf_s2(i)			= Int64.Parse(sBuf(eDataColmnGel.YPos))
			'Z軸位置
			iPzBuf_s2(i)			= Int64.Parse(sBuf(eDataColmnGel.ZPos))
			'温度
			iTempBuf_s2(i)			= Int64.Parse(sBuf(eDataColmnGel.Temp))
			'操作量
			iMvBuf_s2(i)			= Int64.Parse(sBuf(eDataColmnGel.Mv))
			'温度
			iTempBuf2_s2(i)			= Int64.Parse(sBuf(eDataColmnGel.Temp2))
			'操作量
			iMvBuf2_s2(i)			= Int64.Parse(sBuf(eDataColmnGel.Mv2))
			'If sBuf.Length > eDataColmnGel.Hosei
			If bCalc
				'補正値
				iHosei_s2(i)		= Double.Parse(sBuf(eDataColmnGel.Hosei))
				'補正値
				dHoseiP_s2(i)		= Double.Parse(sBuf(eDataColmnGel.HoseiP))
				'粘度
				dViscosity_s2(i)	= Double.Parse(sBuf(eDataColmnGel.Visco))
				'動粘弾性X
				dXPosPhase_s2(i)	= Double.Parse(sBuf(eDataColmnGel.XPosPhase)) * DegToRadRate
				'サンプル歪
				dSplStrain_s2(i)	= Double.Parse(sBuf(eDataColmnGel.SampleStrain))
				'せん断弾性率(Pa)
				dShearMod_s2(i)		= Double.Parse(sBuf(eDataColmnGel.ShearMod))
				'せん断速度(1/S)
				dShearVelo_s2(i)	= Double.Parse(sBuf(eDataColmnGel.ShearVelo))
				'X貯蔵弾性率(Pa)
				dXStrgMod_s2(i)		= Double.Parse(sBuf(eDataColmnGel.XStorageMod))
				'X損失弾性率(Pa)
				dXElasMod_s2(i)		= Double.Parse(sBuf(eDataColmnGel.XElasticMod))
				'X損失正接
				dXLossTan_s2(i)		= Double.Parse(sBuf(eDataColmnGel.XLossTan))
			End If
		Case sMeasindCode(eMesCD.LTM)
			'経過時間
			iMsec_s2(i)				= Int64.Parse(sBuf(eDataColmnLTM.MSec))
			'温度
			iTempBuf_s2(i)			= Int64.Parse(sBuf(eDataColmnLTM.Temp))
			'操作量
			iMvBuf_s2(i)			= Int64.Parse(sBuf(eDataColmnLTM.Mv))
			'温度
			iTempBuf2_s2(i)			= Int64.Parse(sBuf(eDataColmnLTM.Temp2))
			'操作量
			iMvBuf2_s2(i)			= Int64.Parse(sBuf(eDataColmnLTM.Mv2))
			'補正値
			iHosei_s2(i)			= Double.Parse(sBuf(eDataColmnLTM.Hosei))
			'補正値
			dHoseiP_s2(i)			= Double.Parse(sBuf(eDataColmnLTM.HoseiP))
			'粘度
			dViscosity_s2(i)		= Double.Parse(sBuf(eDataColmnLTM.Visco))
			'動粘弾性X
			dXPosPhase_s2(i)		= Double.Parse(sBuf(eDataColmnLTM.XPosPhase)) * DegToRadRate
			'サンプル歪
			dSplStrain_s2(i)		= Double.Parse(sBuf(eDataColmnLTM.SampleStrain))
			'せん断弾性率(Pa)
			dShearMod_s2(i)			= Double.Parse(sBuf(eDataColmnLTM.ShearMod))
			'せん断速度(1/S)
			dShearVelo_s2(i)		= Double.Parse(sBuf(eDataColmnLTM.ShearVelo))
			'X貯蔵弾性率(Pa)
			dXStrgMod_s2(i)			= Double.Parse(sBuf(eDataColmnLTM.XStorageMod))
			'X損失弾性率(Pa)
			dXElasMod_s2(i)			= Double.Parse(sBuf(eDataColmnLTM.XElasticMod))
			'X損失正接
			dXLossTan_s2(i)			= Double.Parse(sBuf(eDataColmnLTM.XLossTan))
		End Select
	End Select
	Return 0
Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message & ex.StackTrace)
	Return -1
End Try
End Function
''' <summary>
''' ヘッダ項目を判断して値をDataFileHeaderへセットする
''' </summary>
''' <param name="sHead">DataFileHeader</param>
''' <param name="sBuf">値</param>
''' <param name="k"></param>
Private Sub	HeaderItemStore(ByRef sHead As DataFileHeader, ByVal sBuf As String, ByVal k As Int32)
Try
	Select Case k
	Case eHead.ToolVersion
		sHead.sToolVersion			= sBuf
	Case eHead.EquipVersion
		sHead.sEquipVersion			= sBuf
	Case eHead.FileName
		sHead.sFileName				= sBuf
	Case eHead.DateTime
		sHead.sDateTime				= sBuf
	Case eHead.MeasureType
		sHead.sMeasureType			= sBuf
	Case eHead.EquipID 
		sHead.sEquipName			= sBuf				
	Case eHead.XYSensID
		sHead.sXYSensorID			= sBuf				
	Case eHead.ZSensID
		sHead.sZSensorID			= sBuf				

	Case eHead.DeviceSpec
		sHead.sDeviceSpec			= sBuf				
	Case eHead.HPPosControl
		sHead.sHPPosControl			= Int16.Parse(sBuf.Substring(0,1))
	Case eHead.CalculationMethod  
		sHead.sCalcMethod			= sBuf
	Case eHead.StressThreshold
		sHead.sThreshold			= sBuf
	Case eHead.HPTemprature
		sHead.dHPTemp				= Double.Parse(sBuf)
	Case eHead.PBTemprature
		sHead.dPBTemp				= Double.Parse(sBuf)
	Case eHead.InputGap
		sHead.dInputGap				= Double.Parse(sBuf)
	Case eHead.GapValue
		sHead.dGapValue				= Double.Parse(sBuf)
	Case eHead.MeasurePresure
		sHead.dMeasurePress			= Double.Parse(sBuf)
	Case eHead.PressContStep
		sHead.iPressContStep		= Int32.Parse(sBuf)
	Case eHead.PressContDelay
		sHead.iPressContDelay		= Int32.Parse(sBuf)
	Case eHead.PBDiameter
		sHead.dPBDiameter			= Double.Parse(sBuf)
	Case eHead.StressDetectMode
		sHead.sStrsDetectMode		= Int16.Parse(sBuf.Substring(0,1))
	Case eHead.AmplitudeMode
		sHead.sAmpMode				= Int16.Parse(sBuf.Substring(0,1))
	Case eHead.LimitStress
		sHead.dLimitStress			= Double.Parse(sBuf)
	Case eHead.XAmplitude
		sHead.dXAmp					= Double.Parse(sBuf)
	Case eHead.YAmplitude
		sHead.dYAmp					= Double.Parse(sBuf)
	Case eHead.ZAmplitude
		sHead.dZAmp					= Double.Parse(sBuf)
	Case eHead.XPeriod
		sHead.dXPrd					= Double.Parse(sBuf)
	Case eHead.YPeriod
		sHead.dYPrd					= Double.Parse(sBuf)
	Case eHead.ZPeriod
		sHead.dZPrd					= Double.Parse(sBuf)
	Case eHead.ZZeroPosition
		sHead.dZZeroPosition		= Double.Parse(sBuf)
	Case eHead.NoLoadZStress
		sHead.dZNoLoadStress		= Double.Parse(sBuf)
	Case eHead.InputWait
		sHead.dInputWait			= Double.Parse(sBuf)
	Case eHead.HPLiftWait
		sHead.dHPLiftWait			= Double.Parse(sBuf)
	Case eHead.XYActWait
		sHead.dXYActWait			= Double.Parse(sBuf)
	Case eHead.MeasTimeLimit
		sHead.dMeasTimeLimit		= Double.Parse(sBuf)
	Case eHead.MeasEndStress
		sHead.dMeasEndStress		= Double.Parse(sBuf)
	Case eHead.TempProfile
		sHead.sTempProfile			= sBuf

	Case eHead.SampleSpec
		sHead.sSampleSpec			= sBuf
	Case eHead.SampleName
		sHead.sSampleName			= sBuf
	Case eHead.Lot 
		sHead.sLot					= sBuf
	Case eHead.Measurer 
		sHead.sMeasurer				= sBuf
	Case eHead.Shape
		sHead.sShape				= Int16.Parse(sBuf.Substring(0,1))
	Case eHead.Thickness, eHead.SpecificGravity
		If sBuf<>"--" And sBuf<>""
			sHead.sThickness			= sBuf
		End If
	Case eHead.Stacks,eHead.Weight,eHead.Volume
		If sBuf<>"--" And sBuf<>""
			sHead.dStacks				= double.Parse(sBuf)
		End If
	Case eHead.MaskTime
		sHead.sMaskTime				= sBuf
	Case eHead.JudgeRange
		sHead.sJudgeRange			= sBuf


	Case eHead.XYWeightCoef
		sHead.dXYWeightCoef			= Double.Parse(sBuf)
	Case eHead.ZWeightCoef
		sHead.dZWeightCoef			= Double.Parse(sBuf)
	Case eHead.HPTempFlucRange
		sHead.sHPTempFlucRange		= sBuf
	Case eHead.PBTempFlucRange
		sHead.sPBTempFlucRange		= sBuf
	Case eHead.FullCalibrationVlue 
		sHead.sFullCalibValue		= sBuf
	Case eHead.ZeroCalibrationVlue 
		sHead.sZeroCalibValue		= sBuf
	Case eHead.FullStressAttenuation
		sHead.sFullStressAttenuation = sBuf
	Case eHead.FullStressPxPhase
		sHead.sFullStressPhaseX		= sBuf
	Case eHead.FullStressPyPhase
		sHead.sFullStressPhaseY		= sBuf
	Case eHead.FullStressPzPhase
		sHead.sFullStressPhaseZ		= sBuf
	'Case eHead.ZeroAdjStartPos
	'	sHead.sZeroAdjStartPos		= sBuf
	Case eHead.ZeroAdjStabMonTime
		sHead.sZeroAdjStabMonTime	= sBuf
	Case eHead.ZeroAdjFlucRange
		sHead.sZeroAdjFlucRange		= sBuf
	Case eHead.HPCenterX
		sHead.sHPCenterX			= sBuf
	Case eHead.HPCenterY
		sHead.sHPCenterY			= sBuf
	Case eHead.Filter 
		sHead.sFilter				= sBuf
	Case eHead.SGTemp
		sHead.dSGTemp				= Double.Parse(sBuf)
	Case eHead.GAPCORR
		sHead.dGapCorrect			= Double.Parse(sBuf)
		sHead.dGapCorrect			= If(sHead.dGapCorrect=0,1,sHead.dGapCorrect)			'0を回避(ヘッダに無い場合)
	Case eHead.ZSTAB
		sHead.dZStab				= Double.Parse(sBuf)

	Case eHead.TempCnt_P
		sHead.dTempCnt_P				= Double.Parse(sBuf)
	Case eHead.TempCnt_I
		sHead.dTempCnt_I				= Double.Parse(sBuf)
	Case eHead.TempCnt_D
		sHead.dTempCnt_D				= Double.Parse(sBuf)
	Case eHead.TempCnt_Dgain
		sHead.dTempCnt_Dgain			= Double.Parse(sBuf)
	Case eHead.TempCnt_PVBias
		sHead.dTempCnt_PVBias			= Double.Parse(sBuf)
	Case eHead.TempCnt_PVGain
		sHead.dTempCnt_PVGain			= Double.Parse(sBuf)

	Case eHead.TempCnt_P2
		sHead.dTempCnt_P2				= Double.Parse(sBuf)
	Case eHead.TempCnt_I2
		sHead.dTempCnt_I2				= Double.Parse(sBuf)
	Case eHead.TempCnt_D2
		sHead.dTempCnt_D2				= Double.Parse(sBuf)
	Case eHead.TempCnt_Dgain2
		sHead.dTempCnt_Dgain2			= Double.Parse(sBuf)
	Case eHead.TempCnt_PVBias2
		sHead.dTempCnt_PVBias2			= Double.Parse(sBuf)
	Case eHead.TempCnt_PVGain2
		sHead.dTempCnt_PVGain2			= Double.Parse(sBuf)

	Case eHead.TempCnt_P3
		sHead.dTempCnt_P3				= Double.Parse(sBuf)
	Case eHead.TempCnt_I3
		sHead.dTempCnt_I3				= Double.Parse(sBuf)
	Case eHead.TempCnt_D3
		sHead.dTempCnt_D3				= Double.Parse(sBuf)
	Case eHead.TempCnt_Dgain3
		sHead.dTempCnt_Dgain3			= Double.Parse(sBuf)
	Case eHead.TempCnt_PVBias3
		sHead.dTempCnt_PVBias3			= Double.Parse(sBuf)
	Case eHead.TempCnt_PVGain3
		sHead.dTempCnt_PVGain3			= Double.Parse(sBuf)

	Case eHead.GelTime
		sHead.dGelTime				= double.Parse(sBuf)
	Case eHead.GelTimeTemp
		sHead.dGelTimeTemp			= double.Parse(sBuf)
	Case eHead.GelTimeVisco
		sHead.dGelTimeVisco			= double.Parse(sBuf)
	Case eHead.GelTimeJudge
		sHead.sGelTimeJudge			= sBuf

	Case eHead.OOSValX
		sHead.dOosValX				= Double.Parse(sBuf)
	Case eHead.OOSValY
		sHead.dOosValY				= Double.Parse(sBuf)
	Case eHead.OOSValZ
		sHead.dOosValZ				= Double.Parse(sBuf)

	Case eHead.CPUCommErrCnt
		sHead.iCpuCommErrCount		= int32.Parse(sBuf)

	Case eHead.ActMeasTime
		sHead.iMeasActTime			= int64.Parse(sBuf)
	Case eHead.AfterZPos
		sHead.dZAfterPos			= int64.Parse(sBuf)
	Case eHead.RealGap
		sHead.dRealGap				= Double.Parse(sBuf)


	Case eHead.IntegTimePowOn
		sHead.iIntegTimePowOn		= UInt32.Parse(sBuf)
	Case eHead.IntegTimeHeatOn
		sHead.iIntegTimeHeatOn		= UInt32.Parse(sBuf)
	Case eHead.IntegTimePBHeat
		sHead.iIntegTimePBHeat		= UInt32.Parse(sBuf)
	Case eHead.IntegTimeSGHeat
		sHead.iIntegTimeSGHeat		= UInt32.Parse(sBuf)
	Case eHead.IntegTimeXAct
		sHead.iIntegTimeXAct		= UInt32.Parse(sBuf)
	Case eHead.IntegTimeYAct
		sHead.iIntegTimeYAct		= UInt32.Parse(sBuf)
	Case eHead.IntegTimeZAct
		sHead.iIntegTimeZAct		= UInt32.Parse(sBuf)

	Case eHead.SLOPEDiffInterval
		sHead.dDiffInterval			= Double.Parse(sBuf)
		sHead.dDiffInterval			= If(sHead.dDiffInterval=0,10,sHead.dDiffInterval)		'0を回避(ヘッダに無い場合)

	Case eHead.ExtStartTime
		sHead.sExtStartTime			= sBuf
	Case eHead.ExtmiddleTime
		sHead.sExtMiddleTime		= sBuf
	Case eHead.ExtEndTime
		sHead.sExtEndTime			= sBuf
	Case eHead.ExtTanSample
		sHead.sExtTanSample			= sBuf
	Case eHead.ExtTanThinning
		sHead.sExtTanThinning		= sBuf
	Case eHead.ExtGelTimeSelect
		sHead.sExtGelTimeSelect		= sBuf
	Case eHead.ExtGelTimeUnder
		sHead.sExtGelTimeUnder		= sBuf
	Case eHead.ExtGelTimeMiddle
		sHead.sExtGelTimeMiddle		= sBuf
	Case eHead.ExtGelTimeOver
		sHead.sExtGelTimeOver		= sBuf

	Case eHead.MELTStartTemp
		sHead.sMelStartTemp			= sBuf
	Case eHead.MELTEndTemp
		sHead.sMelEndTemp			= sBuf
	Case eHead.MELTWaitTime
		sHead.sMelWaitTime			= sBuf
	Case eHead.MELTSpeed
		sHead.sMelSpeed				= sBuf
	Case eHead.MELTTemperature
		sHead.sMelTemperatureRslt	= sBuf
	Case eHead.MELTViscosity
		sHead.sMelViscosityRslt		= sBuf

	Case eHead.FLCTemp
		sHead.sFLCTemp				= sBuf
	Case eHead.ProbeDiameter
		sHead.sProbeDiameter		= sBuf
	Case eHead.FLCSampleTime
		sHead.sFLCSampleTime		= sBuf
	Case eHead.FLCSampleAccRate
		sHead.sFLCSampleAccRate		= sBuf
	Case eHead.FLCAmplitude
		sHead.sFLCAmplitude			= sBuf
	Case eHead.FLCPeriod
		sHead.sFLCPeriod			= sBuf

	Case eHead.FLC1AmplitudeStart
		sHead.sFLC1AmplitudeStart	= sBuf
	Case eHead.FLC1AmplitudeEnd
		sHead.sFLC1AmplitudeEnd		= sBuf
	Case eHead.FLC1AmplitudeStep
		sHead.sFLC1AmplitudeStep	= sBuf
	Case eHead.FLC1MeasRev
		sHead.sFLC1MeasRev			= sBuf

	Case eHead.FLC2Repeat
		sHead.sFLC2Repeat			= sBuf
	Case eHead.FLC2Ope
		sHead.sFLC2Ope				= sBuf
	Case eHead.FLC2Rest
		sHead.sFLC2Rest				= sBuf

	Case eHead.FLC3TempStart
		sHead.sFLC3TempStart		= sBuf
	Case eHead.FLC3TempEnd
		sHead.sFLC3TempEnd			= sBuf
	Case eHead.FLC3TempStep
		sHead.sFLC3TempStep			= sBuf
	Case Else

	End Select


Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message & ex.StackTrace)
End Try
End Sub

#If 1
''' <summary>
''' データファイルヘッダ情報作成(項目ダブルコーテーション囲いを追加)
''' </summary>
''' <param name="iType">	0:通常測定 1:キャリブレーション 2:粘度-速度依存 3:粘度-時間依存 4:粘度-温度依存</param>
''' <param name="sHead">行配列</param>
Private Sub MeasFileHeaderMake(ByVal iType As Int16, ByRef sHead() As String )
Try
	Select Case iType
	Case RealTimeMeas.eMeasMode.Calib
		Array.Resize(sHead, 9)
		sHead(0)					= m_HeadItem(ehead.XAmplitude)				& "," & """" & p_head.dXAmp.ToString("F3") & """"
		sHead(1)					= m_HeadItem(ehead.XPeriod)					& "," & """" & p_head.dXPrd.ToString("F3") & """"
		sHead(2)					= m_HeadItem(ehead.YAmplitude)				& "," & """" & p_head.dYAmp.ToString("F3") & """"
		sHead(3)					= m_HeadItem(ehead.YPeriod)					& "," & """" & p_head.dYPrd.ToString("F3") & """"
		sHead(4)					= m_HeadItem(ehead.ZAmplitude)				& "," & """" & p_head.dZAmp.ToString("F3") & """"
		sHead(5)					= m_HeadItem(ehead.ZPeriod)					& "," & """" & p_head.dZPrd.ToString("F3") & """"
		sHead(6)					= m_HeadItem(eHead.HPTemprature )			& "," & """" & p_head.dHPTemp.ToString("F1") & """"
		sHead(7)					= m_HeadItem(ehead.GapValue)				& "," & """" & p_head.dGapValue.ToString("F3") & """"
		sHead(8)					= m_HeadItem(ehead.XYActWait)				& "," & """" & p_head.dXYActWait.ToString("F1") & """"
	Case Else
		Array.Resize(sHead, m_HeadItem.Length)
		sHead(ehead.Section1)					= m_HeadItem(ehead.Section1)
		sHead(ehead.ToolVersion)				= m_HeadItem(ehead.ToolVersion)				& "," & """" & p_head.sToolVersion & """"
		sHead(ehead.EquipVersion)				= m_HeadItem(ehead.EquipVersion)			& "," & """" & p_head.sEquipVersion & """"
		sHead(ehead.FileName)					= m_HeadItem(ehead.FileName)				& "," & """" & p_head.sFileName & """"
		sHead(ehead.DateTime )					= m_HeadItem(ehead.DateTime )				& "," & """" & p_head.sDateTime & """"
		sHead(ehead.MeasureType )				= m_HeadItem(ehead.MeasureType )			& "," & """" & p_head.sMeasureType & """"
		sHead(ehead.EquipID)					= m_HeadItem(ehead.EquipID)					& "," & """" & p_head.sEquipName & """"
		sHead(ehead.XYSensID)					= m_HeadItem(ehead.XYSensID)				& "," & """" & p_head.sXYSensorID & """"
		sHead(ehead.ZSensID)					= m_HeadItem(ehead.ZSensID)					& "," & """" & p_head.sZSensorID & """"
		sHead(eHead.IntegTimePowOn)				= m_HeadItem(ehead.IntegTimePowOn)			& "," & """" & p_head.iIntegTimePowOn & """"
		sHead(eHead.IntegTimeHeatOn)			= m_HeadItem(ehead.IntegTimeHeatOn)			& "," & """" & p_head.iIntegTimeHeatOn & """"
		sHead(eHead.IntegTimePBHeat)			= m_HeadItem(ehead.IntegTimePBHeat)			& "," & """" & p_head.iIntegTimePBHeat & """"
		sHead(eHead.IntegTimeSGHeat)			= m_HeadItem(ehead.IntegTimeSGHeat)			& "," & """" & p_head.iIntegTimeSGHeat & """"
		sHead(eHead.IntegTimeXAct)				= m_HeadItem(ehead.IntegTimeXAct)			& "," & """" & p_head.iIntegTimeXAct & """"
		sHead(eHead.IntegTimeYAct)				= m_HeadItem(ehead.IntegTimeYAct)			& "," & """" & p_head.iIntegTimeYAct & """"
		sHead(eHead.IntegTimeZAct)				= m_HeadItem(ehead.IntegTimeZAct)			& "," & """" & p_head.iIntegTimeZAct & """"

		sHead(ehead.Section2)					= m_HeadItem(ehead.Section2)
		sHead(ehead.DeviceSpec )				= m_HeadItem(ehead.DeviceSpec )				& "," & """" & p_head.sDeviceSpec & """"
		sHead(ehead.HPPosControl )				= m_HeadItem(ehead.HPPosControl )			& "," & """" & p_head.sHPPosControl & """"			& ",0:GAP 1:Contact pressure 2:Constant pressure"
		sHead(ehead.CalculationMethod )			= m_HeadItem(eHead.CalculationMethod )		& "," & """" & p_head.sCalcMethod & """"
		sHead(ehead.StressThreshold )			= m_HeadItem(eHead.StressThreshold )		& "," & """" & p_head.sThreshold & """"
		sHead(ehead.HPTemprature )				= m_HeadItem(eHead.HPTemprature )			& "," & """" & p_head.dHPTemp.ToString("F1") & """"
		sHead(ehead.PBTemprature )				= m_HeadItem(eHead.PBTemprature )			& "," & """" & p_head.dPBTemp.ToString("F1") & """"
		sHead(ehead.InputGap)					= m_HeadItem(ehead.InputGap)				& "," & """" & p_head.dInputGap.ToString("F3") & """"
		sHead(ehead.GapValue)					= m_HeadItem(ehead.GapValue)				& "," & """" & p_head.dGapValue.ToString("F3") & """"
		sHead(ehead.MeasurePresure)				= m_HeadItem(ehead.MeasurePresure)			& "," & """" & p_head.dMeasurePress.ToString("F1") & """"
		sHead(ehead.PressContStep)				= m_HeadItem(ehead.PressContStep)			& "," & """" & p_head.iPressContStep.ToString() & """"
		sHead(ehead.PressContDelay)				= m_HeadItem(ehead.PressContDelay)			& "," & """" & p_head.iPressContDelay.ToString() & """"
		sHead(ehead.PBDiameter)					= m_HeadItem(ehead.PBDiameter)				& "," & """" & p_head.dPBDiameter.ToString("F1") & """"
		sHead(ehead.StressDetectMode )			= m_HeadItem(ehead.StressDetectMode )		& "," & """" & p_head.sStrsDetectMode & """"		& ",0:Circle 1:Line 2:Point"
		sHead(ehead.AmplitudeMode )				= m_HeadItem(ehead.AmplitudeMode )			& "," & """" & p_head.sAmpMode & """"				& ",0:Constant amp 1:Constant stress"

		sHead(ehead.LimitStress)				= m_HeadItem(ehead.LimitStress)				& "," & """" & p_head.dLimitStress.ToString("F3") & """"

		sHead(ehead.XAmplitude)					= m_HeadItem(ehead.XAmplitude)				& "," & """" & p_head.dXAmp.ToString("F3") & """"
		sHead(ehead.XPeriod)					= m_HeadItem(ehead.XPeriod)					& "," & """" & p_head.dXPrd.ToString("F3") & """"
		sHead(ehead.YAmplitude)					= m_HeadItem(ehead.YAmplitude)				& "," & """" & p_head.dYAmp.ToString("F3") & """"
		sHead(ehead.YPeriod)					= m_HeadItem(ehead.YPeriod)					& "," & """" & p_head.dYPrd.ToString("F3") & """"
		sHead(ehead.ZAmplitude)					= m_HeadItem(ehead.ZAmplitude)				& "," & """" & p_head.dZAmp.ToString("F3") & """"
		sHead(ehead.ZPeriod)					= m_HeadItem(ehead.ZPeriod)					& "," & """" & p_head.dZPrd.ToString("F3") & """"
		sHead(ehead.ZZeroPosition)				= m_HeadItem(ehead.ZZeroPosition)			& "," & """" & p_head.dZZeroPosition.ToString("F3") & """"
		sHead(ehead.NoLoadZStress)				= m_HeadItem(ehead.NoLoadZStress)			& "," & """" & p_head.dZNoLoadStress.ToString("F1") & """"
		sHead(ehead.InputWait)					= m_HeadItem(ehead.InputWait)				& "," & """" & p_head.dInputWait.ToString("F1") & """"
		sHead(ehead.HPLiftWait)					= m_HeadItem(ehead.HPLiftWait)				& "," & """" & p_head.dHPLiftWait.ToString("F1") & """"
		sHead(ehead.XYActWait)					= m_HeadItem(ehead.XYActWait)				& "," & """" & p_head.dXYActWait.ToString("F1") & """"
		sHead(ehead.MeasTimeLimit)				= m_HeadItem(ehead.MeasTimeLimit)			& "," & """" & p_head.dMeasTimeLimit.ToString("F1") & """"
		sHead(ehead.MeasEndStress)				= m_HeadItem(ehead.MeasEndStress)			& "," & """" & p_head.dMeasEndStress.ToString("F1") & """"
		sHead(eHead.TempProfile)				= m_HeadItem(ehead.TempProfile)				& "," & """" & p_head.sTempProfile & """"

		sHead(ehead.Section3)					= m_HeadItem(ehead.Section3)
		sHead(ehead.SampleSpec )				= m_HeadItem(ehead.SampleSpec )				& "," & """" & p_head.sSampleSpec & """"
		sHead(ehead.SampleName )				= m_HeadItem(ehead.SampleName )				& "," & """" & p_head.sSampleName & """"
		sHead(ehead.Lot )						= m_HeadItem(ehead.Lot )					& "," & """" & p_head.sLot & """"
		sHead(ehead.Measurer )					= m_HeadItem(ehead.Measurer )				& "," & """" & p_head.sMeasurer & """"
		sHead(ehead.Shape )						= m_HeadItem(ehead.Shape )					& "," & """" & p_head.sShape & """"					& ",0:Sheet 1:Powder 2:Liquid 3:RCC"
		Select Case p_head.sShape
		Case eshape.sheet,eshape.Rcc
			sHead(ehead.Thickness )					= m_HeadItem(ehead.Thickness )				& "," & """" & p_head.sThickness & """"
			sHead(ehead.Stacks )					= m_HeadItem(ehead.Stacks )					& "," & """" & p_head.dStacks.ToString("F0") & """"
			sHead(ehead.SpecificGravity )			= m_HeadItem(ehead.SpecificGravity )		& "," & ""
			sHead(ehead.Weight )					= m_HeadItem(ehead.Weight )					& "," & ""
			sHead(ehead.Volume )					= m_HeadItem(ehead.Volume )					& "," & ""
		Case eshape.Powder
			sHead(ehead.Thickness )					= m_HeadItem(ehead.Thickness )				& "," & ""
			sHead(ehead.Stacks )					= m_HeadItem(ehead.Stacks )					& "," & ""
			sHead(ehead.SpecificGravity )			= m_HeadItem(ehead.SpecificGravity )		& "," & """" & p_head.sThickness & """"
			sHead(ehead.Weight )					= m_HeadItem(ehead.Weight )					& "," & """" & p_head.dStacks.ToString("F3") & """"
			sHead(ehead.Volume )					= m_HeadItem(ehead.Volume )					& "," & ""
		Case eshape.Liquid
			sHead(ehead.Thickness )					= m_HeadItem(ehead.Thickness )				& "," & ""
			sHead(ehead.Stacks )					= m_HeadItem(ehead.Stacks )					& "," & ""
			sHead(ehead.SpecificGravity )			= m_HeadItem(ehead.SpecificGravity )		& "," & """" & p_head.sThickness & """"
			sHead(ehead.Weight )					= m_HeadItem(ehead.Weight )					& "," & ""
			sHead(ehead.Volume )					= m_HeadItem(ehead.Volume )					& "," & """" & p_head.dStacks.ToString("F3") & """"
		End Select
		sHead(eHead.MaskTime)					= m_HeadItem(ehead.MaskTime)				& "," & """" & p_head.sMaskTime & """"
		sHead(eHead.JudgeRange)					= m_HeadItem(ehead.JudgeRange)				& "," & """" & p_head.sJudgeRange & """"

		sHead(ehead.Section4)					= m_HeadItem(ehead.Section4)
		sHead(ehead.XYWeightCoef)				= m_HeadItem(ehead.XYWeightCoef)			& "," & """" & p_head.dXYWeightCoef.ToString("E15") & """"
		sHead(ehead.ZWeightCoef)				= m_HeadItem(ehead.ZWeightCoef)				& "," & """" & p_head.dZWeightCoef.ToString("E15") & """"
		sHead(ehead.HPTempFlucRange)			= m_HeadItem(ehead.HPTempFlucRange)			& "," & """" & p_head.sHPTempFlucRange & """"
		sHead(ehead.PBTempFlucRange)			= m_HeadItem(ehead.PBTempFlucRange)			& "," & """" & p_head.sPBTempFlucRange & """"
		sHead(ehead.FullCalibrationVlue)		= m_HeadItem(ehead.FullCalibrationVlue)		& "," & """" & p_head.sFullCalibValue & """"
		sHead(ehead.ZeroCalibrationVlue)		= m_HeadItem(ehead.ZeroCalibrationVlue)		& "," & """" & p_head.sZeroCalibValue & """"
		sHead(ehead.FullStressAttenuation)		= m_HeadItem(ehead.FullStressAttenuation)	& "," & """" & p_head.sFullStressAttenuation & """"
		sHead(ehead.FullStressPxPhase)			= m_HeadItem(ehead.FullStressPxPhase)		& "," & """" & p_head.sFullStressPhaseX & """"
		sHead(ehead.FullStressPyPhase)			= m_HeadItem(ehead.FullStressPyPhase)		& "," & """" & p_head.sFullStressPhaseY & """"
		sHead(ehead.FullStressPzPhase)			= m_HeadItem(ehead.FullStressPzPhase)		& "," & """" & p_head.sFullStressPhaseZ & """"
		'sHead(eHead.ZeroAdjStartPos)			= m_HeadItem(ehead.ZeroAdjStartPos)			& "," & """" & p_head.sZeroAdjStartPos & """"
		sHead(eHead.ZeroAdjStabMonTime)			= m_HeadItem(ehead.ZeroAdjStabMonTime)		& "," & """" & p_head.sZeroAdjStabMonTime & """"
		sHead(eHead.ZeroAdjFlucRange)			= m_HeadItem(ehead.ZeroAdjFlucRange)		& "," & """" & p_head.sZeroAdjFlucRange & """"
		sHead(eHead.HPCenterX)					= m_HeadItem(ehead.HPCenterX)				& "," & """" & p_head.sHPCenterX & """"
		sHead(eHead.HPCenterY)					= m_HeadItem(ehead.HPCenterY)				& "," & """" & p_head.sHPCenterY & """"
		sHead(ehead.Filter)						= m_HeadItem(ehead.Filter)					& "," & """" & p_head.sFilter & """"
		sHead(ehead.SGTemp)						= m_HeadItem(ehead.SGTemp)					& "," & """" & p_head.dSGTemp & """"
		sHead(ehead.GAPCORR)					= m_HeadItem(ehead.GAPCORR)					& "," & """" & p_head.dGapCorrect & """"
		sHead(ehead.ZSTAB)						= m_HeadItem(ehead.ZSTAB)					& "," & """" & p_head.dZStab & """"

		sHead(ehead.TempCnt_P)					= m_HeadItem(ehead.TempCnt_P)				& "," & """" & p_head.dTempCnt_P & """"
		sHead(ehead.TempCnt_I)					= m_HeadItem(ehead.TempCnt_I)				& "," & """" & p_head.dTempCnt_I & """"
		sHead(ehead.TempCnt_D)					= m_HeadItem(ehead.TempCnt_D)				& "," & """" & p_head.dTempCnt_D & """"
		sHead(ehead.TempCnt_Dgain)				= m_HeadItem(ehead.TempCnt_Dgain)			& "," & """" & p_head.dTempCnt_Dgain & """"
		sHead(ehead.TempCnt_PVBias)				= m_HeadItem(ehead.TempCnt_PVBias)			& "," & """" & p_head.dTempCnt_PVBias & """"
		sHead(ehead.TempCnt_PVGain)				= m_HeadItem(ehead.TempCnt_PVGain)			& "," & """" & p_head.dTempCnt_PVGain & """"
		sHead(ehead.TempCnt_P2)					= m_HeadItem(ehead.TempCnt_P2)				& "," & """" & p_head.dTempCnt_P2 & """"
		sHead(ehead.TempCnt_I2)					= m_HeadItem(ehead.TempCnt_I2)				& "," & """" & p_head.dTempCnt_I2 & """"
		sHead(ehead.TempCnt_D2)					= m_HeadItem(ehead.TempCnt_D2)				& "," & """" & p_head.dTempCnt_D2 & """"
		sHead(ehead.TempCnt_Dgain2)				= m_HeadItem(ehead.TempCnt_Dgain2)			& "," & """" & p_head.dTempCnt_Dgain2 & """"
		sHead(ehead.TempCnt_PVBias2)			= m_HeadItem(ehead.TempCnt_PVBias2)			& "," & """" & p_head.dTempCnt_PVBias2 & """"
		sHead(ehead.TempCnt_PVGain2)			= m_HeadItem(ehead.TempCnt_PVGain2)			& "," & """" & p_head.dTempCnt_PVGain2 & """"
		sHead(ehead.TempCnt_P3)					= m_HeadItem(ehead.TempCnt_P3)				& "," & """" & p_head.dTempCnt_P3 & """"
		sHead(ehead.TempCnt_I3)					= m_HeadItem(ehead.TempCnt_I3)				& "," & """" & p_head.dTempCnt_I3 & """"
		sHead(ehead.TempCnt_D3)					= m_HeadItem(ehead.TempCnt_D3)				& "," & """" & p_head.dTempCnt_D3 & """"
		sHead(ehead.TempCnt_Dgain3)				= m_HeadItem(ehead.TempCnt_Dgain3)			& "," & """" & p_head.dTempCnt_Dgain3 & """"
		sHead(ehead.TempCnt_PVBias3)			= m_HeadItem(ehead.TempCnt_PVBias3)			& "," & """" & p_head.dTempCnt_PVBias3 & """"
		sHead(ehead.TempCnt_PVGain3)			= m_HeadItem(ehead.TempCnt_PVGain3)			& "," & """" & p_head.dTempCnt_PVGain3 & """"

		sHead(ehead.Section5)					= m_HeadItem(ehead.Section5)
		sHead(ehead.GelTime)					= m_HeadItem(ehead.GelTime)					& "," & """" & p_head.dGelTime.ToString("F2") & """"
		sHead(ehead.GelTimeTemp)				= m_HeadItem(ehead.GelTimeTemp)				& "," & """" & p_head.dGelTimeTemp.ToString("F1") & """"
		sHead(ehead.GelTimeVisco)				= m_HeadItem(ehead.GelTimeVisco)			& "," & """" & p_head.dGelTimeVisco.ToString("F1") & """"
		sHead(eHead.GelTimeJudge)				= m_HeadItem(ehead.GelTimeJudge)			& "," & """" & p_head.sGelTimeJudge & """"

		'sHead(ehead.Section6)					= m_HeadItem(ehead.Section6)
		sHead(ehead.SLOPEDiffInterval)			= m_HeadItem(ehead.SLOPEDiffInterval)		& "," & """" & p_head.dDiffInterval.ToString("F1") & """"

		sHead(eHead.OOSValX)					= m_HeadItem(ehead.OOSValX)					& "," & """" & p_head.dOosValX & """"
		sHead(eHead.OOSValY)					= m_HeadItem(ehead.OOSValY)					& "," & """" & p_head.dOosValY & """"
		sHead(eHead.OOSValZ)					= m_HeadItem(ehead.OOSValZ)					& "," & """" & p_head.dOosValZ & """"
		sHead(eHead.CPUCommErrCnt)				= m_HeadItem(ehead.CPUCommErrCnt)			& "," & """" & p_head.iCpuCommErrCount & """"
		sHead(eHead.ActMeasTime)				= m_HeadItem(ehead.ActMeasTime)				& "," & """" & p_head.iMeasActTime & """"
		sHead(eHead.AfterZPos)					= m_HeadItem(ehead.AfterZPos)				& "," & """" & p_head.dZAfterPos.ToString("F0") & """"
		sHead(eHead.RealGap)					= m_HeadItem(ehead.RealGap)					& "," & """" & p_head.dRealGap.ToString("F3") & """"

		sHead(ehead.Section7)					= m_HeadItem(ehead.Section7)
		sHead(eHead.ExtStartTime)				= m_HeadItem(ehead.ExtStartTime)			& "," & """" & p_head.sExtStartTime & """"
		sHead(eHead.ExtMiddleTime)				= m_HeadItem(ehead.ExtMiddleTime)			& "," & """" & p_head.sExtMiddleTime & """"
		sHead(eHead.ExtEndTime)					= m_HeadItem(ehead.ExtEndTime)				& "," & """" & p_head.sExtEndTime & """"
		sHead(eHead.ExtTanSample)				= m_HeadItem(ehead.ExtTanSample)			& "," & """" & p_head.sExtTanSample & """"
		sHead(eHead.ExtTanThinning)				= m_HeadItem(ehead.ExtTanThinning)			& "," & """" & p_head.sExtTanThinning & """"
		sHead(eHead.ExtGelTimeSelect)			= m_HeadItem(ehead.ExtGelTimeSelect)		& "," & """" & p_head.sExtGelTimeSelect & """"
		sHead(eHead.ExtGelTimeUnder)			= m_HeadItem(ehead.ExtGelTimeUnder)			& "," & """" & p_head.sExtGelTimeUnder & """"
		sHead(eHead.ExtGelTimeMiddle)			= m_HeadItem(ehead.ExtGelTimeMiddle)		& "," & """" & p_head.sExtGelTimeMiddle & """"
		sHead(eHead.ExtGelTimeOver)				= m_HeadItem(ehead.ExtGelTimeOver)			& "," & """" & p_head.sExtGelTimeOver & """"

		sHead(ehead.Section8)					= m_HeadItem(ehead.Section8)
		sHead(eHead.MELTStartTemp)				= m_HeadItem(ehead.MELTStartTemp)			& "," & """" & p_head.sMelStartTemp & """"
		sHead(eHead.MELTEndTemp)				= m_HeadItem(ehead.MELTEndTemp)				& "," & """" & p_head.sMelEndTemp & """"
		sHead(eHead.MELTWaitTime)				= m_HeadItem(ehead.MELTWaitTime)			& "," & """" & p_head.sMelWaitTime & """"
		sHead(eHead.MELTSpeed)					= m_HeadItem(ehead.MELTSpeed)				& "," & """" & p_head.sMelSpeed & """"
		sHead(eHead.MELTTemperature)			= m_HeadItem(ehead.MELTTemperature)			& "," & """" & p_head.sMelTemperatureRslt & """"
		sHead(eHead.MELTViscosity)				= m_HeadItem(ehead.MELTViscosity)			& "," & """" & p_head.sMelViscosityRslt & """"

		sHead(ehead.Section9)					= m_HeadItem(ehead.Section9)
		sHead(eHead.FLCTemp)					= m_HeadItem(ehead.FLCTemp)					& "," & """" & p_head.sFLCTemp & """"
		sHead(eHead.ProbeDiameter)				= m_HeadItem(ehead.ProbeDiameter)			& "," & """" & p_head.sProbeDiameter & """"
		sHead(eHead.FLCSampleTime)				= m_HeadItem(ehead.FLCSampleTime)			& "," & """" & p_head.sFLCSampleTime & """"
		sHead(eHead.FLCSampleAccRate)			= m_HeadItem(ehead.FLCSampleAccRate)		& "," & """" & p_head.sFLCSampleAccRate & """"
		sHead(eHead.FLCAmplitude)				= m_HeadItem(ehead.FLCAmplitude)			& "," & """" & p_head.sFLCAmplitude & """"
		sHead(eHead.FLCPeriod)					= m_HeadItem(ehead.FLCPeriod)				& "," & """" & p_head.sFLCPeriod & """"

		sHead(eHead.FLC1AmplitudeStart)			= m_HeadItem(ehead.FLC1AmplitudeStart)		& "," & """" & p_head.sFLC1AmplitudeStart & """"
		sHead(eHead.FLC1AmplitudeEnd)			= m_HeadItem(ehead.FLC1AmplitudeEnd)		& "," & """" & p_head.sFLC1AmplitudeEnd & """"
		sHead(eHead.FLC1AmplitudeStep)			= m_HeadItem(ehead.FLC1AmplitudeStep)		& "," & """" & p_head.sFLC1AmplitudeStep & """"
		sHead(eHead.FLC1MeasRev)				= m_HeadItem(ehead.FLC1MeasRev)				& "," & """" & p_head.sFLC1MeasRev & """"

		sHead(eHead.FLC2Repeat)					= m_HeadItem(ehead.FLC2Repeat)				& "," & """" & p_head.sFLC2Repeat & """"
		sHead(eHead.FLC2Ope)					= m_HeadItem(ehead.FLC2Ope)					& "," & """" & p_head.sFLC2Ope & """"
		sHead(eHead.FLC2Rest)					= m_HeadItem(ehead.FLC2Rest)				& "," & """" & p_head.sFLC2Rest & """"
						
		sHead(eHead.FLC3TempStart)				= m_HeadItem(ehead.FLC3TempStart)			& "," & """" & p_head.sFLC3TempStart & """"
		sHead(eHead.FLC3TempEnd)				= m_HeadItem(ehead.FLC3TempEnd)				& "," & """" & p_head.sFLC3TempEnd & """"
		sHead(eHead.FLC3TempStep)				= m_HeadItem(ehead.FLC3TempStep)			& "," & """" & p_head.sFLC3TempStep & """"

		sHead(ehead.Section10)					= m_HeadItem(ehead.Section10)
	End Select

Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub
#Else

#end If
''' <summary>
''' データグリッドビューへヘッダ主要情報表示
''' </summary>
''' <param name="dv"></param>
Public Sub CondDispInitial(ByRef dv As DatagridView, ByVal sHead As DataFile.DataFileHeader )
	With dv
		.Rows.Clear
		.Rows.Add( m_HeadItem(eHead.FileName),			"XXXXXXXXXXXXXXXXXXXX" )
		.Rows.Add( m_HeadItem(eHead.DateTime),			"YYYYMMDD hh:mm:ss" )

		.Rows.Add( m_HeadItem(eHead.HPPosControl),		"" )
		.Rows.Add( m_HeadItem(eHead.CalculationMethod),	"" )
		.Rows.Add( m_HeadItem(eHead.StressThreshold),	"" )
		.Rows.Add( m_HeadItem(eHead.HPTemprature),		"" )
		.Rows.Add( m_HeadItem(eHead.PBTemprature),		"" )
		.Rows.Add( m_HeadItem(eHead.GapValue),			"0.000" )
		.Rows.Add( m_HeadItem(eHead.MeasurePresure),	"0.0" )
		.Rows.Add( m_HeadItem(eHead.PBDiameter),		"0.0" )
		.Rows.Add( m_HeadItem(eHead.StressDetectMode),	"0.0" )
		.Rows.Add( m_HeadItem(eHead.XAmplitude),		"0.000" )
		.Rows.Add( m_HeadItem(eHead.YAmplitude),		"0.000" )
		.Rows.Add( m_HeadItem(eHead.ZAmplitude),		"0.000" )
		.Rows.Add( m_HeadItem(eHead.XPeriod),			"0.0" )
		.Rows.Add( m_HeadItem(eHead.YPeriod),			"0.0" )
		.Rows.Add( m_HeadItem(eHead.ZPeriod),			"0.0" )
		.Rows.Add( m_HeadItem(eHead.TempProfile),		"" )

		.Rows.Add( m_HeadItem(eHead.SampleName),		"" )
		.Rows.Add( m_HeadItem(eHead.Lot),				"" )
		.Rows.Add( m_HeadItem(eHead.Shape),				"" )
		Dim sItem(1) As String
		p_clSpec.CondDispRowsAddForShape(sHead.sShape, m_HeadItem, sItem )
		.Rows.Add( sItem(0),							"" )
		.Rows.Add( sItem(1),							"" )

		.Rows.Add( m_HeadItem(eHead.XYWeightCoef),		"E6" )
		.Rows.Add( m_HeadItem(eHead.ZWeightCoef),		"E6" )
		.Rows.Add( m_HeadItem(eHead.FullCalibrationVlue),"" )
		.Rows.Add( m_HeadItem(eHead.ZeroCalibrationVlue),"" )



		.AllowUserToResizeColumns = False
		.AllowUserToResizeRows = False	
		.ColumnHeadersVisible = False
		.rowHeadersVisible = False
		.Columns(1).DefaultCellStyle.Alignment =     DataGridViewContentAlignment.MiddleRight
		.GridColor = Color.DarkGray
		.BackgroundColor = SystemColors.Control
		.Columns(0).ReadOnly = True
		.Columns(1).ReadOnly = True

		.Columns(0).DefaultCellStyle.Font = New Font(BASE_FONT, 8)
		.Columns(1).DefaultCellStyle.Font = New Font(BASE_FONT, 8)
		.Columns(0).Width = 140
		'.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells)			'列自動調整
		.AutoResizeRows(DataGridViewAutoSizeRowsMode.AllCells)					'行自動調整

		.Height = .Rows(0).height * 29 + 22			'29行の高さ
		.Width	= 380

		
	End With
End Sub

''' <summary>
''' ヘッダ主要情報表示更新
''' </summary>
''' <param name="dv"></param>
''' <param name="sHead"></param>
Public Sub	CondDispUpdate( Byref dv As DatagridView, ByVal sHead As DataFile.DataFileHeader)
	With dv
		.Item(1,0).Value = sHead.sFileName
		.Item(1,1).Value = sHead.sDateTime

'		.Item(1,2).Value = sHead.sHPPosControl
		.Item(1,2).Value = p_clSpec.sHCont(sHead.sHPPosControl)
		.Item(1,3).Value = sHead.sCalcMethod
		.Item(1,4).Value = sHead.sThreshold
		.Item(1,5).Value = sHead.dHPTemp.ToString("F1")
		.Item(1,6).Value = sHead.dPBTemp.ToString("F1")
		.Item(1,7).Value = sHead.dGapValue.ToString("F3")
		.Item(1,8).Value = sHead.dMeasurePress.ToString("F1")
		.Item(1,9).Value = sHead.dPBDiameter.ToString("F1")
		.Item(1,10).Value = p_clSpec.sStrsSens(sHead.sStrsDetectMode)
		.Item(1,11).Value = sHead.dXAmp.ToString("F3")
		.Item(1,12).Value = sHead.dYAmp.ToString("F3")
		.Item(1,13).Value = sHead.dZAmp.ToString("F3")
		.Item(1,14).Value = sHead.dXPrd.ToString("F1")
		.Item(1,15).Value = sHead.dYPrd.ToString("F1")
		.Item(1,16).Value = sHead.dzPrd.ToString("F1")
		.Item(1,17).Value = sHead.sTempProfile

		.Item(1,18).Value = sHead.sSampleName
		.Item(1,19).Value = sHead.sLot
		.Item(1,20).Value = p_clSpec.sShape(sHead.sShape)
		.Item(1,21).Value = sHead.sThickness
		.Item(1,22).Value = sHead.dStacks.ToString("F3")

		.Item(1,23).Value = sHead.dXYWeightCoef.ToString("E6")
		.Item(1,24).Value = sHead.dZWeightCoef.ToString("E6")
		.Item(1,25).Value = sHead.sFullCalibValue
		.Item(1,26).Value = sHead.sZeroCalibValue

	End With
End Sub


''' <summary>
''' 履歴表示データグリッドビューへヘッダ主要情報表示
''' </summary>
''' <param name="dv"></param>
Public Sub HistHeadDispInitial(ByRef dv As DatagridView, ByVal iDir As int16)
Dim i As int32
With dv
	Select Case iDir
	Case 0	'垂直方向へ系列
		.EnableHeadersVisualStyles		= False			'Visualスタイル不適用
		.ReadOnly						= True			'読取専用
		.RowHeadersVisible				= False			'列ヘッダ消去
		.AllowUserToAddRows				= False			'追加用行を消去
		.AllowUserToDeleteRows			= True			'削除可能
		.AllowUserToResizeRows			= False			'行サイズ
		.AllowUserToOrderColumns		= False			'ソート不可
		.CurrentCell					= Nothing
		.Font							= New Font(BASE_FONT, 10)
		.ScrollBars						= ScrollBars.Horizontal	'水平スクロールバーのみ
		.RowTemplate.Height				= 24					'行高さ
		.ColumnHeadersHeightSizeMode	= DataGridViewColumnHeadersHeightSizeMode.EnableResizing
		.ColumnHeadersHeight			= 70			'ヘッダ行高さ
		.ColumnHeadersDefaultCellStyle.BackColor	= Color.LemonChiffon			'ヘッダ行背景色
		.SelectionMode								= DataGridViewSelectionMode.CellSelect
		.MultiSelect								= False

		'カラムヘッダセット
		.Columns.Clear()
		.Columns.Add("Col", "")
		For i = 0 To m_ColumnHeadItem.GetUpperBound(0)
			.Columns.Add(i+1, m_ColumnHeadItem(i))
			.Columns(i+1).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
			.Columns(i+1).SortMode = DataGridViewColumnSortMode.NotSortable
		Next i

		.Columns(2).Frozen		= True			'左3列を固定
		.Columns(0).Width		= 40			'グラフ色カラム幅
		.Columns(1).Width		= 170			'ファイル名カラム幅

		'.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells)			'列自動調整
		'.AutoResizeRows(DataGridViewAutoSizeRowsMode.AllCells)					'行自動調整

	Case 1	'水平方向に系列
		.EnableHeadersVisualStyles		= False					'Visualスタイル不適用
		.ReadOnly						= True					'読取専用
		.RowHeadersVisible				= False					'列ヘッダ
		.AllowUserToAddRows				= False					'追加用行を消去
		.AllowUserToDeleteRows			= True					'削除可能
		.AllowUserToResizeRows			= False					'行サイズ
		.AllowUserToOrderColumns		= False					'ソート不可
		.CurrentCell					= Nothing
		.Font							= New Font(BASE_FONT, 10)
		.ScrollBars						= ScrollBars.Both		'スクロールバー
		.RowTemplate.Height				= 24					'行高さ
		.ColumnHeadersDefaultCellStyle.Alignment	= DataGridViewContentAlignment.MiddleCenter
		.ColumnHeadersHeightSizeMode				= DataGridViewColumnHeadersHeightSizeMode.EnableResizing
		.ColumnHeadersHeight			= 24									'ヘッダ行高さ
		.ColumnHeadersDefaultCellStyle.BackColor	= Color.LightGray			'ヘッダ行背景色
		.SelectionMode								= DataGridViewSelectionMode.CellSelect
		.MultiSelect								= False

		'カラムヘッダセット
		.Rows.Clear()
		.Columns.Clear()
		.Columns.Add("Col", "")
		'Dim iColNum As Int16 = GetMeasDataSeries()
		'For i = 0 To iColNum - 1
		For i = 0 To cHistMax + cStdMax
			Select Case i
			Case 0
				.Columns.Add(i+1, m_ColCntents(0))
			Case 1 To cHistMax
				.Columns.Add(i+1, m_ColCntents(1) & " " & i)
			Case cHistMax+1 To cHistMax + cStdMax
				.Columns.Add(i+1, m_ColCntents(2) & " " & i-cHistMax)
			Case Else
			End Select
			.Columns(i+1).DefaultCellStyle.Alignment	= DataGridViewContentAlignment.MiddleLeft
			.Columns(i+1).SortMode						= DataGridViewColumnSortMode.NotSortable
			.Columns(i+1).Width		= 170				'データ列幅
		Next i
		.Rows.Add(m_ColumnHeadItem.Length + 1)

		.Rows(2).Frozen			= True					'上3行を固定
		.Columns(0).Frozen		= True					'左1列を固定
		.Columns(0).Width		= 200					'条件タイトル列幅

		'.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells)			'列自動調整
		'.AutoResizeRows(DataGridViewAutoSizeRowsMode.AllCells)					'行自動調整
	End Select

End With



#If 0

	With dv
		.Rows.Add( m_HeadItem(eHead.FileName),			"XXXXXXXXXXXXXXXXXXXX" )
		.Rows.Add( m_HeadItem(eHead.DateTime),			"YYYYMMDD hh:mm:ss" )

		.Rows.Add( m_HeadItem(eHead.HPPosControl),		"" )
		.Rows.Add( m_HeadItem(eHead.CalculationMethod),	"" )
		.Rows.Add( m_HeadItem(eHead.StressThreshold),	"" )
		.Rows.Add( m_HeadItem(eHead.HPTemprature),		"" )
		.Rows.Add( m_HeadItem(eHead.PBTemprature),		"" )
		.Rows.Add( m_HeadItem(eHead.GapValue),			"0.000" )
		.Rows.Add( m_HeadItem(eHead.MeasurePresure),	"0.0" )
		.Rows.Add( m_HeadItem(eHead.PBDiameter),		"0.0" )
		.Rows.Add( m_HeadItem(eHead.StressDetectMode),	"0.0" )
		.Rows.Add( m_HeadItem(eHead.XAmplitude),		"0.000" )
		.Rows.Add( m_HeadItem(eHead.YAmplitude),		"0.000" )
		.Rows.Add( m_HeadItem(eHead.ZAmplitude),		"0.000" )
		.Rows.Add( m_HeadItem(eHead.XPeriod),			"0.0" )
		.Rows.Add( m_HeadItem(eHead.YPeriod),			"0.0" )
		.Rows.Add( m_HeadItem(eHead.ZPeriod),			"0.0" )
		.Rows.Add( m_HeadItem(eHead.AValue),			"0.00" )
		.Rows.Add( m_HeadItem(eHead.BValue),			"0.00" )
		.Rows.Add( m_HeadItem(eHead.TempProfile),		"" )

		.Rows.Add( m_HeadItem(eHead.SampleName),		"" )
		.Rows.Add( m_HeadItem(eHead.Lot),				"" )
		.Rows.Add( m_HeadItem(eHead.Shape),				"" )
		Dim sItem(1) As String
		p_clSpec.CondDispRowsAddForShape(sHead.sShape, m_HeadItem, sItem )
		.Rows.Add( sItem(0),							"" )
		.Rows.Add( sItem(1),							"" )

		.Rows.Add( m_HeadItem(eHead.XYWeightCoef),		"E6" )
		.Rows.Add( m_HeadItem(eHead.ZWeightCoef),		"E6" )
		.Rows.Add( m_HeadItem(eHead.FullCalibrationVlue),"" )
		.Rows.Add( m_HeadItem(eHead.ZeroCalibrationVlue),"" )



		.AllowUserToResizeColumns = False
		.AllowUserToResizeRows = False	
		.ColumnHeadersVisible = False
		.rowHeadersVisible = False
		.Columns(1).DefaultCellStyle.Alignment =     DataGridViewContentAlignment.MiddleRight
		.GridColor = Color.DarkGray
		.BackgroundColor = SystemColors.Control
		.Columns(0).ReadOnly = True
		.Columns(1).ReadOnly = True

		.Columns(0).DefaultCellStyle.Font = New Font(BASE_FONT, 8)
		.Columns(1).DefaultCellStyle.Font = New Font(BASE_FONT, 8)
		.Columns(0).Width = 140
		'.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells)			'列自動調整
		.AutoResizeRows(DataGridViewAutoSizeRowsMode.AllCells)					'行自動調整

		.Height = .Rows(0).height * 29 + 22			'29行の高さ
		.Width	= 380

		
	End With


#end if
End Sub
''' <summary>
''' 測定データバッファ内の使用チェック
''' </summary>
''' <returns>	-1:異常終了
'''			その他：使用数</returns>
Public Function GetMeasDataSeries() As Int16
	Dim i As Int16
Try
	'基準値領域をチェック
	If Not isNothing(p_dtFile.p_head_s(0).sDateTime) Or Not isNothing(p_dtFile.p_head_s(1).sDateTime)
	'最後まで埋める
		Return 1 + cHistMax + cStdMax
	ElseIf isNothing(p_dtFile.p_head.sDateTime)
	'測定値領域にないのでゼロ
		Return 0
	Else
	'履歴領域をチェック
		For i=0 To p_dtFile.p_head_h.GetUpperBound(0)
			If isNothing(p_dtFile.p_head_h(i).sDateTime)
				Return i + 1
			End If
		Next
		Return i + 1
	End If
Catch ex As Exception
	Return -1
End Try
End Function

''' <summary>
''' 履歴表示データグリッドビュー表示更新
''' </summary>
''' <param name="dv"></param>
Public Sub HistHeadDispUpdate(ByRef dv As DataGridView, ByVal iDir As int16)
Dim i As int16
Try
	Select Case iDir
	Case 0	'垂直方向へ系列
		dv.Rows.Clear()
		'測定値領域
		dv.Rows.Add()
		If p_head.sFileName <> ""
			HistHeadItemSet(dv, p_head, 0, iDir)
		End If
		'履歴値領域
		For i = 0 To p_head_h.GetUpperBound(0)
			If p_head_h(i).sFileName<>""
				dv.Rows.Add()
				HistHeadItemSet(dv, p_head_h(i), i+1, iDir)
			Else
				dv.Rows.Add()
			End if
		Next i
		'基準値領域
		For i=0 To p_head_s.GetUpperBound(0)
			dv.Rows.Add()
			If p_head_s(i).sFileName <> ""
				HistHeadItemSet(dv, p_head_s(i), cHistMax+1+i, iDir)
			End If
		Next


		'異なるものが有ればマーク(熱板位置制御～温度プロファイル間)
		Dim sOne As String
		'For i = eHistCol.HPPosControl To eHistCol.TempProfile
		For i = eHistCol.Color To eHistCol.Max - 1
			Select Case i
			Case _
				eHistCol.HPPosControl, _
				eHistCol.HPTemprature, _
				eHistCol.PBTemprature , _
				eHistCol.SGTemp, _
				eHistCol.HPTempFlucRange, _
				eHistCol.PBTempFlucRange, _
				eHistCol.TempProfile, _
				eHistCol.InputGap, _
				eHistCol.GapValue, _
				eHistCol.MeasurePresure, _
				eHistCol.PressContStep, _
				eHistCol.PressContDelay, _
				eHistCol.PBDiameter, _
				eHistCol.StressDetectMode, _
				eHistCol.AmplitudeMode, _
				eHistCol.LimitStress, _
				eHistCol.XAmplitude, _
				eHistCol.XPeriod, _
				eHistCol.InputWait, _
				eHistCol.HPLiftWait, _
				eHistCol.XYActWait, _
				eHistCol.MeasTimeLimit, _
				eHistCol.MeasEndStress, _
				eHistCol.CalculationMethod, _
				eHistCol.StressThreshold, _
				eHistCol.SLOPEDiffInterval

				sOne = dv(i, 0).Value
				For j=2 To dv.Rows.Count - 1
					If Not IsNothing(dv(i, j).Value)
						If dv(i, j).Value <> sOne
							For k=0 To dv.Rows.Count - 1
								dv(i, k).Style.BackColor = Color.LightYellow
							Next
							Continue For
						End If
					End If
				Next
			Case Else				
			End Select
		Next



	Case 1	'水平方向へ系列
		dv.Item(0, 0).Value = m_ColCntents(3)
		For i = 0 To m_ColumnHeadItem.GetUpperBound(0)
			dv.Item(0, i+1).Value = m_ColumnHeadItem(i)
		Next
	
		'測定値領域
		If p_head.sFileName <> ""
			HistHeadItemSet(dv, p_head, 0, iDir)
		End If
		'履歴値領域
		For i = 0 To p_head_h.GetUpperBound(0)
			If p_head_h(i).sFileName<>""
				HistHeadItemSet(dv, p_head_h(i), i + 1, iDir)
			End if
		Next i
		'基準値領域
		For i=0 To p_head_s.GetUpperBound(0)
			If p_head_s(i).sFileName <> ""
				HistHeadItemSet(dv, p_head_s(i), cHistMax+1+i, iDir)
			End If
		Next


		'異なるものが有ればマーク(熱板位置制御～温度プロファイル間)
		Dim sOne As String
		'For i = eHistCol.HPPosControl To eHistCol.TempProfile
		For i = eHistCol.Color To eHistCol.Max - 1
			Select Case i
			Case _
				eHistCol.HPPosControl, _
				eHistCol.HPTemprature, _
				eHistCol.PBTemprature , _
				eHistCol.SGTemp, _
				eHistCol.HPTempFlucRange, _
				eHistCol.PBTempFlucRange, _
				eHistCol.TempProfile, _
				eHistCol.InputGap, _
				eHistCol.GapValue, _
				eHistCol.MeasurePresure, _
				eHistCol.PressContStep, _
				eHistCol.PressContDelay, _
				eHistCol.PBDiameter, _
				eHistCol.StressDetectMode, _
				eHistCol.AmplitudeMode, _
				eHistCol.LimitStress, _
				eHistCol.XAmplitude, _
				eHistCol.XPeriod, _
				eHistCol.InputWait, _
				eHistCol.HPLiftWait, _
				eHistCol.XYActWait, _
				eHistCol.MeasTimeLimit, _
				eHistCol.MeasEndStress, _
				eHistCol.CalculationMethod, _
				eHistCol.StressThreshold, _
				eHistCol.SLOPEDiffInterval
					sOne = dv(1, i).Value
					For j=2 To dv.ColumnCount-1
						If Not IsNothing(dv(j, i).Value)
							If dv(j, i).Value <> sOne
								For k=0 To dv.ColumnCount-1
									dv(k, i).Style.BackColor = Color.LightYellow
								Next
								Continue For
							End If
						End If
					Next
			Case Else
			End Select
		Next


	End Select

Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub

''' <summary>
''' 履歴表示データグリッドビューデータセット
''' </summary>
''' <param name="dv"></param>
''' <param name="sHead"></param>
''' <param name="iRows"></param>
''' <param name="iDir"></param>
Private Sub HistHeadItemSet(ByRef dv As DataGridView, ByVal sHead As DataFile.DataFileHeader, ByVal iRows As int16, ByVal iDir As int16)
Dim iCols As int16
Try
	With dv
	Select Case iDir
	Case 0	'垂直方向へ系列
		.Item(eHistCol.Color,				iRows).Style.BackColor	= p_cts.clrY1Default(iRows+1)		'グラフプロット色を設定
		.Item(ehistcol.FileName,			iRows).Value = sHead.sFileName
		.Item(ehistcol.SampleName,			iRows).Value = sHead.sSampleName
		.Item(ehistcol.SampleSpec,			iRows).Value = sHead.sSampleSpec
		.Item(ehistcol.Lot,					iRows).Value = sHead.sLot
		.Item(ehistcol.Measurer,			iRows).Value = sHead.sMeasurer
		.Item(ehistcol.Shape,				iRows).Value = p_clSpec.sShape(sHead.sShape)
		.Item(ehistcol.Thickness,			iRows).Value = sHead.sThickness
		.Item(ehistcol.Stacks,				iRows).Value = sHead.dStacks.ToString("F3")
		.Item(ehistcol.MaskTime,			iRows).Value = sHead.sMaskTime
		.Item(ehistcol.JudgeRange,			iRows).Value = sHead.sJudgeRange

		.Item(ehistcol.DeviceSpec,			iRows).Value	= sHead.sDeviceSpec
		.Item(ehistcol.HPPosControl,		iRows).Value	= p_clSpec.sHCont(sHead.sHPPosControl)
		.Item(ehistcol.HPTemprature,		iRows).Value	= sHead.dHPTemp.ToString("F1")
		.Item(ehistcol.PBTemprature,		iRows).Value	= sHead.dPBTemp.ToString("F1")
		.Item(ehistcol.SGTemp,				iRows).Value	= sHead.dSGTemp.ToString("F1")
		.Item(ehistcol.HPTempFlucRange,		iRows).Value	= sHead.sHPTempFlucRange
		.Item(ehistcol.PBTempFlucRange,		iRows).Value	= sHead.sPbTempFlucRange
		.Item(ehistcol.TempProfile,			iRows).Value	= sHead.sTempProfile
		.Item(ehistcol.InputGap,			iRows).Value	= sHead.dInputGap.ToString("F3")
		.Item(ehistcol.GapValue,			iRows).Value	= sHead.dGapValue.ToString("F3")
		.Item(ehistcol.MeasurePresure,		iRows).Value	= sHead.dMeasurePress.ToString("F1")
		.Item(ehistcol.PressContStep,		iRows).Value	= sHead.iPressContStep
		.Item(ehistcol.PressContDelay,		iRows).Value	= sHead.iPressContDelay
		.Item(ehistcol.PBDiameter,			iRows).Value	= sHead.dPBDiameter.ToString("F1")
		.Item(ehistcol.StressDetectMode,	iRows).Value	= p_clSpec.sStrsSens(sHead.sStrsDetectMode)
		.Item(eHistCol.AmplitudeMode,		iRows).Value	= p_clSpec.sAmpMode(sHead.sAmpMode)
		.Item(ehistcol.LimitStress,			iRows).Value	= sHead.dLimitStress.ToString("F1")

		Select Case sHead.sStrsDetectMode
		Case clSpec.eStrsSens.Circle, clSpec.eStrsSens.Line
			.Item(ehistcol.XAmplitude,			iRows).Value = sHead.dXAmp.ToString("F3")
			.Item(ehistcol.XPeriod,				iRows).Value = sHead.dXPrd.ToString("F1")
		Case clSpec.eStrsSens.Point
			.Item(ehistcol.XAmplitude,			iRows).Value = sHead.dZAmp.ToString("F3")
			.Item(ehistcol.XPeriod,				iRows).Value = sHead.dZPrd.ToString("F1")
		Case Else
			.Item(ehistcol.XAmplitude,			iRows).Value = sHead.dXAmp.ToString("F3")
			.Item(ehistcol.XPeriod,				iRows).Value = sHead.dXPrd.ToString("F1")
		End Select
		.Item(ehistcol.InputWait,			iRows).Value = sHead.dInputWait.ToString("F1")
		.Item(ehistcol.HPLiftWait,			iRows).Value = sHead.dHPLiftWait.ToString("F1")
		.Item(ehistcol.XYActWait,			iRows).Value = sHead.dXYActWait.ToString("F1")
		.Item(ehistcol.MeasTimeLimit,		iRows).Value = sHead.dMeasTimeLimit.ToString("F1")
		.Item(ehistcol.MeasEndStress,		iRows).Value = sHead.dMeasEndStress.ToString("F1")

		.Item(ehistcol.DateTime,			iRows).Value = sHead.sDateTime
		.Item(ehistcol.MeasureType,			iRows).Value = sHead.sMeasureType
		.Item(ehistcol.CalculationMethod,	iRows).Value = sHead.sCalcMethod
		.Item(ehistcol.StressThreshold,		iRows).Value = sHead.sThreshold
		.Item(ehistcol.SLOPEDiffInterval,	iRows).Value = sHead.dDiffInterval.ToString("F1")
		.Item(ehistcol.GelTime,				iRows).Value = sHead.dGelTime.ToString("F2")
		.Item(ehistcol.GelTimeTemp,			iRows).Value = sHead.dGelTimeTemp.ToString("F2")
		.Item(ehistcol.GelTimeVisco,		iRows).Value = sHead.dGelTimeVisco.ToString("F2")
		.Item(ehistcol.Filter,				iRows).Value = sHead.sFilter

		.Item(ehistcol.ToolVersion,				iRows).Value	= sHead.sToolVersion
		.Item(ehistcol.EquipVersion,			iRows).Value	= sHead.sEquipVersion
		.Item(ehistcol.EquipID,					iRows).Value	= sHead.sEquipName
		.Item(ehistcol.XYSensID,				iRows).Value	= sHead.sXYSensorID
		.Item(ehistcol.ZSensID,					iRows).Value	= sHead.sZSensorID
		.Item(ehistcol.XYWeightCoef,			iRows).Value	= sHead.dXYWeightCoef.ToString("E15")
		.Item(ehistcol.ZWeightCoef,				iRows).Value	= sHead.dZWeightCoef.ToString("E15")
		.Item(ehistcol.FullCalibrationVlue,		iRows).Value	= sHead.sFullCalibValue.ToString()
		.Item(ehistcol.ZeroCalibrationVlue,		iRows).Value	= sHead.sZeroCalibValue.ToString()
		.Item(ehistcol.FullStressAttenuation,	iRows).Value	= sHead.sFullStressAttenuation
		.Item(ehistcol.FullStressPxPhase,		iRows).Value	= sHead.sFullStressPhaseX
		.Item(ehistcol.FullStressPyPhase,		iRows).Value	= sHead.sFullStressPhaseY
		.Item(ehistcol.FullStressPzPhase,		iRows).Value	= sHead.sFullStressPhaseZ
		.Item(ehistcol.GAPCORR,					iRows).Value	= sHead.dGapCorrect.ToString()
		.Item(ehistcol.NoLoadZStress,			iRows).Value	= sHead.dZNoLoadStress.ToString()
		.Item(ehistcol.ZSTAB,					iRows).Value	= sHead.dZStab.ToString("F3")
		.Item(ehistcol.ZeroAdjFlucRange,		iRows).Value	= sHead.sZeroAdjFlucRange
		.Item(ehistcol.ZeroAdjStabMonTime,		iRows).Value	= sHead.sZeroAdjStabMonTime
		.Item(ehistcol.ZZeroPosition,			iRows).Value	= sHead.dZZeroPosition.ToString
		.Item(ehistcol.ZAfterPosition,			iRows).Value	= sHead.dZAfterPos.ToString
		.Item(ehistcol.RealGap,					iRows).Value	= sHead.dRealGap.ToString("F3")
		.Item(ehistcol.HPCenterX,				iRows).Value	= sHead.sHPCenterX
		.Item(ehistcol.HPCenterY,				iRows).Value	= sHead.sHPCenterY

	Case 1	'水平方向へ系列
		iCols = iRows + 1
		.Item(iCols, eHistCol.Color).Style.BackColor 	= p_cts.clrY1Default(iCols)		'グラフプロット色を設定
		.Item(iCols,ehistcol.FileName).Value		= sHead.sFileName
		.Item(iCols,ehistcol.SampleName).Value		= sHead.sSampleName
		.Item(iCols,ehistcol.SampleSpec).Value		= sHead.sSampleSpec
		.Item(iCols,ehistcol.Lot).Value				= sHead.sLot
		.Item(iCols,ehistcol.Measurer).Value		= sHead.sMeasurer
		.Item(iCols,ehistcol.Shape).Value			= p_clSpec.sShape(sHead.sShape)
		.Item(iCols,ehistcol.Thickness).Value		= sHead.sThickness
		.Item(iCols,ehistcol.Stacks).Value			= sHead.dStacks.ToString("F3")
		.Item(iCols,ehistcol.MaskTime).Value		= sHead.sMaskTime
		.Item(iCols,ehistcol.JudgeRange).Value		= sHead.sJudgeRange

		.Item(iCols,ehistcol.DeviceSpec).Value		= sHead.sDeviceSpec
		.Item(iCols,ehistcol.HPPosControl).Value	= p_clSpec.sHCont(sHead.sHPPosControl)
		.Item(iCols,ehistcol.HPTemprature).Value	= sHead.dHPTemp.ToString("F1")
		.Item(iCols,ehistcol.PBTemprature).Value	= sHead.dPBTemp.ToString("F1")
		.Item(iCols,ehistcol.SGTemp).Value			= sHead.dSGTemp.ToString("F1")
		.Item(iCols,ehistcol.HPTempFlucRange).Value	= sHead.sHPTempFlucRange
		.Item(iCols,ehistcol.PBTempFlucRange).Value	= sHead.sPbTempFlucRange
		.Item(iCols,ehistcol.TempProfile).Value		= sHead.sTempProfile
		.Item(iCols,ehistcol.InputGap).Value		= sHead.dInputGap.ToString("F3")
		.Item(iCols,ehistcol.GapValue).Value		= sHead.dGapValue.ToString("F3")
		.Item(iCols,ehistcol.MeasurePresure).Value	= sHead.dMeasurePress.ToString("F1")
		.Item(iCols,ehistcol.PressContStep).Value	= sHead.iPressContStep
		.Item(iCols,ehistcol.PressContDelay).Value	= sHead.iPressContDelay
		.Item(iCols,ehistcol.PBDiameter).Value		= sHead.dPBDiameter.ToString("F1")
		.Item(iCols,ehistcol.StressDetectMode).Value= p_clSpec.sStrsSens(sHead.sStrsDetectMode)
		.Item(iCols,eHistCol.AmplitudeMode).Value	= p_clSpec.sAmpMode(sHead.sAmpMode)
		.Item(iCols,ehistcol.LimitStress).Value		= sHead.dLimitStress.ToString("F1")

		Select Case sHead.sStrsDetectMode
		Case clSpec.eStrsSens.Circle, clSpec.eStrsSens.Line
			.Item(iCols,ehistcol.XAmplitude).Value		= sHead.dXAmp.ToString("F3")
			.Item(iCols,ehistcol.XPeriod).Value			= sHead.dXPrd.ToString("F1")
		Case clSpec.eStrsSens.Point
			.Item(iCols,ehistcol.XAmplitude).Value		= sHead.dZAmp.ToString("F3")
			.Item(iCols,ehistcol.XPeriod).Value			= sHead.dZPrd.ToString("F1")
		Case Else
			.Item(iCols,ehistcol.XAmplitude).Value		= sHead.dXAmp.ToString("F3")
			.Item(iCols,ehistcol.XPeriod).Value			= sHead.dXPrd.ToString("F1")
		End Select
		.Item(iCols,ehistcol.InputWait).Value		= sHead.dInputWait.ToString("F1")
		.Item(iCols,ehistcol.HPLiftWait).Value		= sHead.dHPLiftWait.ToString("F1")
		.Item(iCols,ehistcol.XYActWait).Value		= sHead.dXYActWait.ToString("F1")
		.Item(iCols,ehistcol.MeasTimeLimit).Value	= sHead.dMeasTimeLimit.ToString("F1")
		.Item(iCols,ehistcol.MeasEndStress).Value	= sHead.dMeasEndStress.ToString("F1")

		.Item(iCols,ehistcol.DateTime).Value				= sHead.sDateTime
		.Item(iCols,ehistcol.MeasureType).Value				= sHead.sMeasureType
		.Item(iCols,ehistcol.CalculationMethod).Value		= sHead.sCalcMethod
		.Item(iCols,ehistcol.StressThreshold).Value			= sHead.sThreshold
		.Item(iCols,ehistcol.SLOPEDiffInterval).Value		= sHead.dDiffInterval.ToString("F1")
		.Item(iCols,ehistcol.GelTime).Value					= sHead.dGelTime.ToString("F2")
		.Item(iCols,ehistcol.GelTimeTemp).Value				= sHead.dGelTimeTemp.ToString("F2")
		.Item(iCols,ehistcol.GelTimeVisco).Value			= sHead.dGelTimeVisco.ToString("F2")
		.Item(iCols,ehistcol.Filter).Value					= sHead.sFilter

		.Item(iCols,ehistcol.ToolVersion).Value				= sHead.sToolVersion
		.Item(iCols,ehistcol.EquipVersion).Value			= sHead.sEquipVersion
		.Item(iCols,ehistcol.EquipID).Value					= sHead.sEquipName
		.Item(iCols,ehistcol.XYSensID).Value				= sHead.sXYSensorID
		.Item(iCols,ehistcol.ZSensID).Value					= sHead.sZSensorID
		.Item(iCols,ehistcol.XYWeightCoef).Value			= sHead.dXYWeightCoef.ToString("E15")
		.Item(iCols,ehistcol.ZWeightCoef).Value				= sHead.dZWeightCoef.ToString("E15")
		.Item(iCols,ehistcol.FullCalibrationVlue).Value		= sHead.sFullCalibValue.ToString()
		.Item(iCols,ehistcol.ZeroCalibrationVlue).Value		= sHead.sZeroCalibValue.ToString()
		.Item(iCols,ehistcol.FullStressAttenuation).Value	= sHead.sFullStressAttenuation
		.Item(iCols,ehistcol.FullStressPxPhase).Value		= sHead.sFullStressPhaseX
		.Item(iCols,ehistcol.FullStressPyPhase).Value		= sHead.sFullStressPhaseY
		.Item(iCols,ehistcol.FullStressPzPhase).Value		= sHead.sFullStressPhaseZ
		.Item(iCols,ehistcol.GAPCORR).Value					= sHead.dGapCorrect.ToString()
		.Item(iCols,ehistcol.NoLoadZStress).Value			= sHead.dZNoLoadStress.ToString()
		.Item(iCols,ehistcol.ZSTAB).Value					= sHead.dZStab.ToString("F3")
		.Item(iCols,ehistcol.ZeroAdjFlucRange).Value		= sHead.sZeroAdjFlucRange
		.Item(iCols,ehistcol.ZeroAdjStabMonTime).Value		= sHead.sZeroAdjStabMonTime
		.Item(iCols,ehistcol.ZZeroPosition).Value			= sHead.dZZeroPosition.ToString
		.Item(iCols,ehistcol.ZAfterPosition).Value			= sHead.dZAfterPos.ToString
		.Item(iCols,ehistcol.RealGap).Value					= sHead.dRealGap.ToString("F3")
		.Item(iCols,ehistcol.HPCenterX).Value				= sHead.sHPCenterX
		.Item(iCols,ehistcol.HPCenterY).Value				= sHead.sHPCenterY


	End Select
	End With

Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub

''' <summary>
''' 測定データの行数をカウントし必要な配列を確保する
''' </summary>
''' <returns>-1：例外発生、以外：行数</returns>
Public Function GetFileLineCount(Optional ByVal sPath As String = Nothing, Optional ByVal sFileName As String = Nothing) As Int64
Dim sFilePath		As String = Nothing
Dim sLineBuf		As String
Dim iLine			As int64
Dim iRet			As Int32
Try
	'パスを決定する
	iRet = RealTimeMeasDataFilePath(sFilePath, sPath, sFileName)
	If iRet <> 0
		Return iRet
	End If

	Dim fs			As New FileStream(sFilePath, FileMode.Open, FileAccess.Read,FileShare.ReadWrite)
	Dim sr			As TextReader = New StreamReader(fs, Encoding.UTF8)
	Dim iMode		As int16
	Dim sBuf()		As String = Nothing
	iLine = 0
	Do 
		sLineBuf = sr.ReadLine()
		If IsNothing(sLineBuf)
			Exit Do
		End If
		sBuf = sLineBuf.Split(",")
		Select Case sBuf(0)
		Case cHeader
			iMode = 0
			Continue Do
		Case cData
			iMode = 1
			Continue Do
		End Select

		Select Case iMode
		Case 0	'ヘッダ部
		Case 1	'データ部
			iLine += 1
		End Select
	Loop Until IsNothing(sLineBuf)
Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Return -1
End Try
	Return iLine - 1		'カラムヘッダ分を減算
End Function

''' <summary>
''' 測定値関係の配列を確保する
''' </summary>
''' <param name="iLine">サイズ</param>
Public Sub ResizeMeasArray(ByVal iLine As int64)
	'測定値
	Array.Resize(iMSec,			iLine)
	Array.Resize(iOxBuf,		iLine)
	Array.Resize(iOyBuf,		iLine)
	Array.Resize(iOzBuf,		iLine)
	Array.Resize(dOzBuf,		iLine)
	Array.Resize(iPxBuf,		iLine)
	Array.Resize(iPyBuf,		iLine)
	Array.Resize(iPzBuf,		iLine)
	Array.Resize(iTempBuf,		iLine)
	Array.Resize(iMvBuf,		iLine)
	Array.Resize(iTempBuf2,		iLine)
	Array.Resize(iMvBuf2,		iLine)
	'計算値
	Array.Resize(iHosei,		iLine)
	Array.Resize(dHosei100,		iLine)
	Array.Resize(iSlope,		iLine)
	Array.Resize(dViscosity,	iLine)
	Array.Resize(dXPosPhase,	iLine)
	Array.Resize(dYPosPhase,	iLine)
	Array.Resize(dSplStrain,	iLine)
	Array.Resize(dShearMod,		iLine)
	Array.Resize(dShearVelo,	iLine)
	Array.Resize(dShearStress,	iLine)
	Array.Resize(dXStrgMod,		iLine)
	Array.Resize(dXElasMod,		iLine)
	Array.Resize(dXLossTan,		iLine)
	Array.Resize(dYStrgMod,		iLine)
	Array.Resize(dYElasMod,		iLine)
	Array.Resize(dYLossTan,		iLine)
End Sub

''' <summary>
''' 測定生データを読込み(Text) 
'''   読込バッファ配列は確保されていること
''' </summary>
''' <returns></returns>
Public Function RealTimeFileReadText(ByVal bHeadEn As Boolean,Optional ByVal sPath As String = Nothing, Optional ByRef sHead As DataFileHeader = Nothing) As Int32
Dim sFilePath		As String = Nothing
Dim sLineBuf		As String
Dim sBuf()			As String
Dim iRet			As Int32
Dim i				As int64
Dim j				As int64
Dim k				As int64
Try
	'パスを決定する
	iRet = RealTimeMeasDataFilePath(sFilePath, sPath)
	If iRet <> 0
		Return iRet
	End If

	Dim fs As New FileStream(sFilePath, FileMode.Open, FileAccess.Read,FileShare.ReadWrite)
	Dim sr As TextReader = New StreamReader(fs, Encoding.UTF8)
	Dim iMode As Int16 = 0
	i = 0
	j = 0
	Do 
		sLineBuf = sr.ReadLine()
		If IsNothing(sLineBuf)
			Exit Do
		End If
		sBuf = sLineBuf.Split(",")
		Select Case sBuf(0)
		Case cHeader
			iMode = 0		'ヘッダ部
			Continue Do
		Case cData
			iMode = 1		'データ部
			Continue Do
		End Select

		Select Case iMode
		Case 0
		'ヘッダ部の読込
			If bHeadEn
				If sLineBuf.Substring(0,1) = "["	'セクション行スキップ
					Continue Do
				End If
				If sLineBuf.Substring(0,1) = "'"	'コメント行スキップ
					Continue Do
				End If
				If sLineBuf.Length=0				'空行スキップ
					Continue Do
				End If
				sBuf = sLineBuf.Split(",")
				For k=0 To m_HeadItem.GetUpperBound(0)
					if m_HeadItem(k) = sBuf(0)
						'項目名が見つかった
						Exit For
					End If
				Next
				If k>m_HeadItem.GetUpperBound(0)
					Continue Do
				End If

				Dim sItm As String = sBuf(1)
				sItm = sItm.Replace("""", "")			'ダブルコーテーション囲いを削除
				HeaderItemStore(sHead, sItm, k)			'ヘッダ項目セット
				j += 1
			End If
		Case 1
		'データ部の読込
			If sLineBuf.Substring(0,1) = "'"
				Continue Do
			End If
			sBuf = sLineBuf.Split(",")
			'経過時間
				iMsec(i)		= Int64.Parse(sBuf(eDataColmnGel.MSec))
			'応力X
				iOxBuf(i)		= Int64.Parse(sBuf(eDataColmnGel.XSrc))
			'応力Y
				iOyBuf(i)		= Int64.Parse(sBuf(eDataColmnGel.YSrc))
			'応力Z
				iOzBuf(i)		= Int64.Parse(sBuf(eDataColmnGel.ZSrc))
			'応力Z(g)
				dOzBuf(i)		= iOzBuf(i) * sHead.dZWeightCoef
			'X軸位置
				iPxBuf(i)		= Int64.Parse(sBuf(eDataColmnGel.XPos))
			'Y軸位置
				iPyBuf(i)		= Int64.Parse(sBuf(eDataColmnGel.YPos))
			'Z軸位置
				iPzBuf(i)		= Int64.Parse(sBuf(eDataColmnGel.ZPos))
			'温度
				iTempBuf(i)		= Int64.Parse(sBuf(eDataColmnGel.Temp))
			'操作量
				iMvBuf(i)		= Int64.Parse(sBuf(eDataColmnGel.Mv))
			'温度
				iTempBuf2(i)	= Int64.Parse(sBuf(eDataColmnGel.Temp2))
			'操作量
				iMvBuf2(i)		= Int64.Parse(sBuf(eDataColmnGel.Mv2))
			If sBuf.Length > eDataColmnGel.Hosei
			'補正値
				iHosei(i)		= Int64.Parse(sBuf(eDataColmnGel.Hosei))
			'粘度
				dViscosity(i)	= Double.Parse(sBuf(eDataColmnGel.Visco))
			'動粘弾性X
				dXPosPhase(i)	= Double.Parse(sBuf(eDataColmnGel.XPosPhase))
			'サンプル歪
				dSplStrain(i)	= Double.Parse(sBuf(eDataColmnGel.SampleStrain))
			'せん断弾性率(Pa)
				dSplStrain(i)	= Double.Parse(sBuf(eDataColmnGel.ShearMod))
			'X貯蔵弾性率(Pa)
				dXStrgMod(i)	= Double.Parse(sBuf(eDataColmnGel.XStorageMod))
			'X損失弾性率(Pa)
				dXElasMod(i)	= Double.Parse(sBuf(eDataColmnGel.XElasticMod))
			'X損失正接
				dXLossTan(i)	= Double.Parse(sBuf(eDataColmnGel.XLossTan))
			End If
			i += 1
			If i>iOxBuf.GetUpperBound(0) 
				Exit Do
			End If
		End Select
	Loop Until sLineBuf Is Nothing
	sr.Close()
	sr.Dispose()

	Return 0
Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Return -1
End Try
End function

''' <summary>
'''  測定データから各法によるゲルタイムを計算する
'''  PHASE法時はPHASE配列を逆走査で時間を得る
'''  MELT法時はViscosity配列の最小値の時間を得る->その時間の温度を得る
''' </summary>
''' <param name="iMethod">0:THR 1:SLOPE 2:PHASE 3:MELT</param>
''' <param name="iSGItem">現在表示中chartY軸選択</param>
''' <param name="dGelTime">計算されたゲルタイム[秒]</param>
''' <param name="dThr">閾値[%]</param>
''' <param name="dFullScale">フルスケール値</param>
''' <param name="dRangeMin">探索範囲 開始秒</param>
''' <param name="dRangeMax">探索範囲 終了秒</param>
''' <returns>0:正常終了
'''			-1：例外発生
'''			-2：計算不能
'''			-3：未測定
'''			-4：データ数不足
''' </returns>
Public Function CalcGelTime( _
ByVal iMethod As Int16, _
ByVal iSGItem As Int16, _
ByRef dGelTime As double, _
byVal dThr As Double, _
byVal dFullScale As Double, _
byVal dRangeMin As double, _
byVal dRangeMax As double, _
Optional ByVal dWaitTime As Double = 0, _
Optional ByRef dVis As Double = 0, _
Optional Byref dTemp As Double = 0, _
Optional Byref cts As CToolSettings = Nothing _
) As Int32
	Dim iDtCnt	As Int64 = 0
	Dim PerThr	As Double
	Dim iSIdx	As int64
	Dim iEIdx	As int64
	Dim iRet	As int16
	Dim iArr()	As Int64	= Nothing
	Dim dArr()	As Double	= Nothing
	Dim iss		As int32
	Dim iee		As int32
	Dim iWaitSmpNum		As Int32 = 0
	Dim dSampleInterval	As Double = MEAS_TIME_RESOLUTION			'サンプルインターバル(mS)
Try
	Select Case iMethod
	Case eCalcMethod.THR 	'THR
		If Not isNothing(cts)
			Select Case cts.LTModeEnable
			Case False
			'通常モード
				If Not IsNothing(iHosei)
					Array.Resize(dArr, iHosei.Length)
					Select Case iSGItem
					Case iGrpItmN.rbCorrectP
						dArr = DirectCast(dHosei100, Double())
						dFullScale = 100
					Case Else
						dArr = DirectCast(iHosei, Double())
					End Select
				Else
					Return -3
				End If
				dSampleInterval = MEAS_TIME_RESOLUTION
			Case Else
			'長期間欠モード
				If Not IsNothing(iLT_Hosei)
					Array.Resize(dArr, iLT_Hosei.Length)
					Select Case iSGItem
					Case iGrpItmN.rbCorrectP
						dArr = DirectCast(dLT_Hosei100, Double())
						dFullScale = 100
					Case Else
						dArr = DirectCast(iLT_Hosei, Double())
					End Select
				Else
					Return -3
				End If
				dSampleInterval = (iLT_MSec(1) - iLT_MSec(0)) * 1000
			End Select
		Else
			Return -3
		End If
	Case eCalcMethod.SLOPE	'SLOPE
		If Not IsNothing(iSlope)
			Array.Resize(dArr, iSlope.Length)
			dArr = iSlope
		Else
			Return -3
		End If
		'範囲内の最大値を求める
		iRet = GetMaxMin1ArrayRange(iSlope, dRangeMin, dRangeMax, dFullScale,,MEAS_TIME_RESOLUTION)
		If iRet <> 0
			dFullScale = 0
		End If
	Case eCalcMethod.PHASE	'PHASE
		If Not IsNothing(dXPosPhase)
			Array.Resize(dArr, dXPosPhase.Length)
			dArr = DirectCast(dXPosPhase, Double())
		Else
			Return -3
		End If
	Case eCalcMethod.MELT	'MELT
		If Not IsNothing(dViscosity)
			Array.Resize(dArr, dViscosity.Length)
			dArr = DirectCast(dViscosity, Double())
		Else
			Return -3
		End If
	End Select

	'補正済配列内を初めてトルク閾値を超える場所を見つける
	Select Case cts.LTModeEnable
	Case False
		iSIdx = dRangeMin * MEAS_BLOCK_DATA_NUM
		iEIdx = dRangeMax * MEAS_BLOCK_DATA_NUM
		Select Case iMethod
		Case eCalcMethod.THR, eCalcMethod.SLOPE, eCalcMethod.PHASE, eCalcMethod.MELT	'THR,SLOPE,PHASE
			iEIdx = If(iEIdx > dArr.Length - 1,	dArr.Length - 1, iEIdx)			'配列範囲を超えていた
			iEIdx = If(iEIdx = 0, dArr.Length - 1, iEIdx)						'0指定は配列全てを
		End Select
	Case True
		iSIdx = dRangeMin / 60
		iEIdx = dRangeMax / 60
		iEIdx = If(iEIdx > dArr.Length - 1,	dArr.Length - 1, iEIdx)			'配列範囲を超えていた
		iEIdx = If(iEIdx = 0, dArr.Length - 1, iEIdx)						'0指定は配列全てを
	End Select
	Select Case iMethod
	Case eCalcMethod.THR, eCalcMethod.SLOPE							'THR,SLOPE
		For iDtCnt = 0 + iSIdx To iEidx
			If Not Double.IsNaN(dArr(iDtCnt))
				Select Case iSGItem
				Case iGrpItmN.rbCorrectP
					PerThr = (dArr(iDtCnt) / 100) * 100.0
				Case Else
					PerThr = (dArr(iDtCnt) / dFullScale) * 100.0
				End Select
				If PerThr >= dThr 
					Exit For
				End If
			End If
		Next iDtCnt
		iee = iEidx
	Case eCalcMethod.PHASE											'Phase
		For iDtCnt = 0 + iEidx To iSidx Step -1
			If Not Double.IsNaN(dArr(iDtCnt))
				PerThr = (dArr(iDtCnt) / dFullScale) * 100.0
				If PerThr >= dThr 
					Exit For
				End If
			End If
		Next iDtCnt
		iee = iEidx
	Case eCalcMethod.MELT											'MELT
		Dim dArr2() As Double = Nothing
		'有効範囲指定が優先
		If dRangeMax = 0 AndAlso dRangeMin = 0
			iWaitSmpNum	 = (dWaitTime+dRangeMin) * MEAS_BLOCK_DATA_NUM
		Else
			iWaitSmpNum	 = dRangeMin * MEAS_BLOCK_DATA_NUM
		End If
		If dArr.Length - iWaitSmpNum<=0
			Return -4
		End If
		Array.Resize(dArr2, dArr.Length - (iWaitSmpNum+(dArr.Length-iEIdx)))
		Array.Copy(dArr, iWaitSmpNum, dArr2, 0, dArr2.Length)			'最初の待ち時間削除
		
		'粘度Arr2内の最小値を求める
		Dim dMin As Double = Double.PositiveInfinity
		Dim i As UInt64
		For i=0 To dArr2.GetUpperBound(0)
			If Not Double.IsNaN(dArr2(i))
				If dArr2(i) <> 0
					If dMin > dArr2(i)
						dMin = dArr2(i)
					End If
				End If
			End If
		Next

		'PerThr = dArr2.Min()			Double.NaNがあるとNANが返されるので使えない
		PerThr = dMin

		'粘度Arr2内の最小値のインデックスを求める
		'iSIdx = 0
		'iEIdx = dArr2.Length-1
		iss = 0
		iee = dArr2.Length-1
		For iDtCnt = 0 To iee
			If Not Double.IsNaN(dArr2(iDtCnt))
				If dArr2(iDtCnt) <> 0 
					If PerThr >= dArr2(iDtCnt) 
						Exit For
					End If
				End If
			End If
		Next iDtCnt

	End Select

	If iDtCnt > iee
	'交点見つからない
		dGelTime = -1.0
		Return -2
	Else
	'交点が見つかった
		Select Case iMethod
		Case eCalcMethod.THR, eCalcMethod.SLOPE, eCalcMethod.PHASE		'THR,SLOPE,PHASE
			'If Not cts.LTModeEnable
			'	dGelTime = Double.Parse(iDtCnt) * Double.Parse(MEAS_TIME_RESOLUTION) / 1000.0
			'Else
			'	dGelTime = Double.Parse(iDtCnt) * cts.LTMeasureInterval * 60
			'End If
#If 0
			dGelTime = Double.Parse(iDtCnt) * dSampleInterval / 1000
#else
			'2点間を線形補完してゲルタイムを算出する
			dGelTime = fnGelinterpolation(dArr, iDtCnt, dThr, dFullScale, dSampleInterval)
#end if

		Case eCalcMethod.MELT		'MELT
			'iDtCnt += (dWaitTime+dRangeMin) * MEAS_BLOCK_DATA_NUM
			iDtCnt += iWaitSmpNum
			If iDtCnt < iSIdx
			'有効範囲を超えた場合(小)
				iDtCnt = iSIdx
			End If
			If iDtCnt > iEIdx
			'有効範囲を超えた場合(大)
				iDtCnt = iEIdx
			End If
			dGelTime = Double.Parse(iDtCnt) * Double.Parse(MEAS_TIME_RESOLUTION) / 1000.0
			dVis	= PerThr
			If iDtCnt > iTempBuf.GetUpperBound(0)
				dGelTime = -1.0
				Return -2
			Else
				dTemp	= iTempBuf(iDtCnt) / 10.0
			End If
		End Select
	End If
Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Return -1
End Try
	Return 0
End Function
''' <summary>
''' 2点間を線形補完してゲルタイムを算出する
''' </summary>
''' <param name="dArr">測定データ配列</param>
''' <param name="iDtCnt">dArr上のTHR境界インデックス</param>
''' <param name="dThr">THR(%)</param>
''' <param name="dFullScale">dThr100%時の値</param>
''' <param name="dSampleInterval">dArrの時間間隔(mS)</param>
''' <returns>-1:異常終了 その他:ゲルタイム </returns>
Private Function fnGelinterpolation(ByVal dArr() As Double, ByVal iDtCnt As Int32, ByVal dThr As Double, ByVal dFullScale As Double, ByVal dSampleInterval As Double) As Double
Try
	Dim dX(1)	As Double
	Dim dY(1)	As Double
	Dim dRet()	As Double	'線形補完パラメータ 0:b 1:a
	Dim dThrR	As Double
	Dim dGelTime  As Double = -1

	'線形補完座標をセット
	dX(0) = Double.Parse(iDtCnt-1) * Double.Parse(dSampleInterval) / 1000.0
	dX(1) = Double.Parse(iDtCnt+0) * Double.Parse(dSampleInterval) / 1000.0
	dY(0) = dArr(iDtCnt-1)
	dY(1) = dArr(iDtCnt+0)
	'線形補間パラメータ取得
	dRet = datCal.fnLeastSquaresMethod(dX, dY, dX, dY, 2, 1)
	'dThr = dRet(0) + dRet(1) * dGelTime
	dThrR =  (dThr / 100) *  dFullScale				'設定THRの実データ値を換算
	dGelTime = (dThrR - dRet(0)) / dRet(1)			'ゲルタイムを線形計算して取得
	If dGelTime>=dX(0) And dGelTime<=dX(1)
	Else
		dGelTime = dX(1)
	End If
	Return dGelTime
Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Return -1
End Try
End Function

''' <summary>
''' ヘッダ保存されたフィルタパラメータの略式をstFilterInfoタイプのデータへ変換する
''' </summary>
''' <param name="sFilter"></param>
''' <param name="Fltset"></param>
Public Sub FilterConvHeaderToFilset(ByVal sFilter As String, Byref Fltset As stFilterInfo)
	Dim fil As stFilterInfo = Nothing
	Dim s() As String = sFilter.Split("-")					'FIR-1.00-1001-Hamming OR IIR-1.00-3
	If IsNothing(s)
		Fltset.iEnable = 0
	ElseIf s.Length <> 4 AndAlso s.Length <> 3
		Fltset.iEnable = 0
	Else
		Fltset.iEnable = 1
		Select Case s(0)
		Case "FIR"
			Fltset.sKind			= s(0)				        'フィルタ種類 FIR/IIR
			Fltset.dFIRCutOffFreq	= Ctype(s(1), Double)		'FIR カットオフ周波数
			Fltset.iFIRFilLen		= Ctype(s(2), integer)      'FIR パラメータ長
			Fltset.sFIRWinKind		= s(3)						'FIR 窓関数種
		Case Else
			Fltset.sKind			= s(0)				        'フィルタ種類 FIR/IIR
			Fltset.dIIRCutOffFreq	= Ctype(s(1), Double)		'IIR カットオフ周波数
			Fltset.iIIRDeg			= Ctype(s(2), integer)      'IIR フィルタ次数
		End Select
		Fltset.dFIRSmplFreq			= MEAS_SAMPLE_FREQ			'FIR サンプル周波数
	End If
End Sub
''' <summary>
''' 基準値を再計算
''' </summary>
''' <param name="frm"></param>
''' <param name="iSel">1,2</param>
''' <param name="cts"></param>
''' <returns></returns>
Public Function ReCalclationStd(ByRef frm As frmMain, ByVal iSel As Int16, ByVal cts As CToolSettings ) As Int32
Dim ohead	As Datafile.DataFileHeader = p_dtFile.p_head_s(iSel-1)
Dim bEvac	As Boolean = False
Dim FilSet	As Filter.stFilterInfo = Nothing

Try
	If Not IsNothing(p_dtFile.iOxBuf)
	'現在値領域にデータがあるので退避する
		'p_dtFile.TemporaryEvacuration(cts.LTModeEnable)
		p_dtFile.h0ToBCopy(cts.LTModeEnable)
		bEvac = True
	End If
	'読込の基準値データを現在値領域へコピー
	'p_dtFile.StandardToCurrCopy(iSel, cts.LTModeEnable)
	Select Case iSel
	Case 1
		'p_dtFile.BToS1Paste(cts.LTModeEnable)
		p_dtFile.S1Toh0Copy(cts.LTModeEnable)
	Case 2
		'p_dtFile.BToS2Paste(cts.LTModeEnable)
		p_dtFile.S2Toh0Copy(cts.LTModeEnable)
	End Select


	'中間値補正処理
	Dim cp As DataCal.stCalcPara = Nothing
	cp.iMeas			= eHoseiMeasMode.GelTime
	cp.iMode			= eHoseiCalcMode.Batch
	cp.cts				= cts
	cp.cts.WeightCoefZ	= ohead.dZWeightCoef
	cp.cts.GapCorr		= If(ohead.dGapCorrect=0, cp.cts.GapCorr, ohead.dGapCorrect)
	cp.cts.InputGap		= ohead.dInputGap
	cp.clDt				= p_dtFile
	cp.dPeriod			= ohead.dXPrd
	cp.dAmplitude		= ohead.dXAmp
	cp.iSampleStart		= 0
	cp.iSampleNum		= p_dtFile.iOxBuf.Length
	'ヘッダ保存されたフィルタパラメータの略式をstFilterInfoタイプのデータへ変換する
	p_dtFile.FilterConvHeaderToFilset(ohead.sFilter, FilSet)
	cp.fltSet			= FilSet
	cp.dXYWait			= ohead.dHPLiftWait + ohead.dXYActWait
	cp.dZeroStress		= Ctype(ohead.sZeroCalibValue, Double)
	cp.dAttenuation		= Ctype(ohead.sFullStressAttenuation, Double)
	cp.dDiameter		= ohead.dPBDiameter
	cp.dMeasGap			= ohead.dGapValue
	cp.dFullStress		= Ctype(ohead.sFullCalibValue, Double)
	cp.dPxPhase			= Ctype(ohead.sFullStressPhaseX, Double)
	cp.dPyPhase			= Ctype(ohead.sFullStressPhaseY, Double)
	cp.dPzPhase			= Ctype(ohead.sFullStressPhaseZ, Double)
	cp.iZeroPos			= ohead.dZZeroPosition
	cp.iCalcType		= lstCalcMethod.IndexOf(ohead.sCalcMethod)
	cp.dDiffInterval	= ohead.dDiffInterval
'	cp.iStrsSensMode	= Array.IndexOf(p_clSpec.sStrsSens, ohead.sStrsDetectMode)
	cp.iStrsSensMode	= ohead.sStrsDetectMode
	cp.iAmpMode			= ohead.sAmpMode
	cp.iHPPosControl	= ohead.sHPPosControl						'熱板位置制御方法 0,1,2
	cp.dMeasureStress	= ohead.dMeasurePress
	cp.dXYWeightCoef	= ohead.dXYWeightCoef						'XY重量換算
	

	'RCC時の基材厚み
	Select Case ohead.sShape
	Case eShape.Rcc
		cp.dRCCBaseThickness = Ctype(ohead.sThickness, Double) * ohead.dStacks
	Case Else
		cp.dRCCBaseThickness = 0.0
	End Select

	If cp.iStrsSensMode = clSpec.eStrsSens.Circle
		datCal.CorrectRealProcVector( cp )
	Else
		datCal.CorrectRealProcVector( cp )
	End If

	'現在値領域にある計算結果を基準値領域へコピーする
	p_dtFile.CurrToStandardCopy(iSel, cts.LTModeEnable)

	If bEvac
	'現在値領域に一時退避データを戻す
		'p_dtFile.TemporaryToCurrCopy(cts.LTModeEnable)
		p_dtFile.BToh0Paste(cts.LTModeEnable)
	Else
	'現在値領域をクリア
		p_dtFile.ReadBufferClear()
	End If
	Return 0

Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Return -1
Finally
End Try
End Function


'Excelから読み取った測定データ
Dim mesdata(,) as object =nothing
'Excelテストデータをmesdataへ読み込む
Private Sub TestExcelFileRead()
	Dim FilePath As string = "D:\TEST\XXX_1.xlsx"
	Dim oXls As Excel.Application = nothing  ' Excelオブジェクト
	Dim oBook As Excel.Workbook = nothing    ' Excel Workbook
	Dim oSheets As Excel.Sheets = Nothing
	Dim owks As Excel.Worksheet = Nothing
	Dim oRng As Excel.Range = Nothing

	Try
	Thread.Sleep(10)
	oXls = New Excel.Application()
	oBook = oXls.Workbooks.Open(FilePath)
	oSheets = oBook.Worksheets
	owks = oSheets(1)
	orng = owks.Range(owks.Cells(2,3),owks.cells(78002,5))        'X_AD,Y_AD,Z
	mesdata = orng.Value

    

	Catch ex As Exception
	Finally
		If Not oRng Is Nothing Then
			System.Runtime.InteropServices.Marshal.FinalReleaseComObject(oRng)
		oRng = Nothing
		End If

		System.Runtime.InteropServices.Marshal.FinalReleaseComObject(owks)
		System.Runtime.InteropServices.Marshal.FinalReleaseComObject(oSheets)
		System.Runtime.InteropServices.Marshal.FinalReleaseComObject(oBook)
		owks = Nothing
		oSheets = Nothing
		oBook = Nothing

		' Close Excel.
		oXls.Quit()
		System.Runtime.InteropServices.Marshal.FinalReleaseComObject(oXls)
		oXls = Nothing
	End Try
End Sub


Public Function DebugFileOut(ByVal sFileName As String, Byval oArray() As Int64) As Int16 
Try
	Dim fs As New FileStream("D:\Test\" & sFileName & ".txt", FileMode.Create, FileAccess.Write, FileShare.ReadWrite)
	Dim sw As Textwriter = New StreamWriter(fs, Encoding.UTF8)
	Dim sLineBuf As String = nothing
	'テキストファイル書込み
	For i = 0 To oArray.GetUpperBound(0)
		sLineBuf = oArray(i).ToString
		sw.WriteLine(sLineBuf)
	Next i
	sw.Close()
	sw = nothing

Catch ex As Exception
End Try
	Return 0
End Function



'AES方式暗号化サンプル
Public Class TextFileEncryption
    Private Shared key As Byte()	= {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16}
    Private Shared iv As Byte()		= {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 56}

''' <summary>
''' テキストファイルオープン
''' </summary>
''' <param name="iEnc">0:暗号化無し 1:暗号化あり</param>
''' <param name="sFileFullPath">ファイルフルパス</param>
''' <param name="fs">FileStream</param>
''' <param name="cs">CryptoStream</param>
''' <param name="sw">StreamWriter</param>
''' <returns></returns>
Public Shared Function OpenCreateTextFile(ByVal iEnc As int16, ByVal sFileFullPath As String, ByRef fs As FileStream, ByRef cs As CryptoStream, ByRef sw As StreamWriter ) As int32
Try
	Select Case iEnc
	Case 0
		fs = New FileStream(sFileFullPath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite)
		sw = New StreamWriter(fs, Encoding.UTF8)
	Case 1
		Dim aesAlg  As New AesCryptoServiceProvider()
        Dim encryptor As ICryptoTransform
		aesAlg.Key = key
        aesAlg.IV = iv
		encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV)
		fs = New FileStream(sFileFullPath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite)
		cs = New CryptoStream(fs, encryptor, CryptoStreamMode.Write)
		sw = New StreamWriter(cs)
	End Select
	Return 0
Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Return -1
End Try
End Function

    Public Shared Sub EncryptTextToFile(plainText As String, filePath As String)
        Using aesAlg As New AesCryptoServiceProvider()
            aesAlg.Key = key
            aesAlg.IV = iv

            Dim encryptor As ICryptoTransform = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV)

            Using fsEncrypt As New FileStream(filePath, FileMode.Create)
                Using csEncrypt As New CryptoStream(fsEncrypt, encryptor, CryptoStreamMode.Write)
                    Using swEncrypt As New StreamWriter(csEncrypt)
                        swEncrypt.Write(plainText)
                    End Using
                End Using
            End Using
        End Using
    End Sub

    Public Shared Function DecryptTextFromFile(filePath As String) As String
        Using aesAlg As New AesCryptoServiceProvider()
            aesAlg.Key = key
            aesAlg.IV = iv

            Dim decryptor As ICryptoTransform = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV)

            Using fsDecrypt As New FileStream(filePath, FileMode.Open)
                Using csDecrypt As New CryptoStream(fsDecrypt, decryptor, CryptoStreamMode.Read)
                    Using srDecrypt As New StreamReader(csDecrypt)
                        Return srDecrypt.ReadToEnd()
                    End Using
                End Using
            End Using
        End Using
    End Function
End Class




End Class
