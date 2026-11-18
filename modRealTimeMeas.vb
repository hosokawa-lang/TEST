'============================================
'File:   RealTimeMeas.vb
'Name:   SIZUKA Tool RTGクラス
'Desc:   測定処理を行うクラス。
'Author: Fumihiko Kondo
'Date:   2015/04/01
'============================================

Imports System.ComponentModel
Imports System.IO
Imports System.Threading
Imports SZKTool.clSpec
Imports SZKTool.DataFile
Imports SZKTool.Filter
Imports SZKTool.frmCalibFull
Imports SZKTool.frmHorConf
Imports SZKTool.frmMain
Imports SZKTool.frmOptions
Imports SZKTool.frmViscosity
Imports SZKTool.My.Mycomputer

'''<summary>
'''リアルタイム測定を行うクラス。
'''</summary>
Public Class RealTimeMeas

	'▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼
	'   定数
	'▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲
	Public Const cOriginSearchTimeLimit	As Double = 30.0		'原点復帰タイムアウト値(秒)

	Private Const cTCP_WAIT As Double = 0.5

	Private Const cCompStatus As Integer = &H102    '完了状態ステータス
	Private Const cCompTimeout As Integer = 200     '完了状態監視タイムアウト設定(msec) 処理時間は除く

	Private Const cResponseTimeOut		As Int16 = 5		'応答ビットタイムアウト時間(秒)
	Private Const cResponseTimeOutDrive As Int16 = 30		'応答ビットタイムアウト時間(秒) 手動駆動

	'測定状態
	Public Enum eMeasStat             
		CANCEL = -5					'測定中止 
		NOT_START = -1				'未開始
		RESERV						'予備
		PREPAREING_1				'準備中(投入GAP下降～クリア時間) 
		PREPAREING_2				'準備中(測定位置上昇～動作待機時間) 
		DATA_TRANS					'測定中(データ取得)
		LOCKOUT_OK					'プローブロック外しOK
		FINISHING					'終了中	
	End Enum

	'較正中状態
	Public Enum eCalibState
		NOT_START = -1				'未開始
		ZERO_EXEC = 1				'ZERO調実行中
		ZERO_COMP					'ZERO調完了
		RESERV						'予備
		PREPAREING					'準備中
		COND_CONF					'条件確定
		DATA_TRANS					'測定中(データ取得)
		DispUpWait					'表示更新中
		AUTO_PEELING				'自動剥がし
		AUTO_PEELING_1				'自動剥がし
		MANU_PEELING				'手動剥がし
		MANU_PEELING_1				'手動剥がし
		FINISHING					'終了中
	End Enum
	

	'測定モード
	Public Enum eMeasMode
		Normal = 0					'通常測定
		Calib						'キャリブレーション
		FlowCurve_Speed				'粘度 速度依存
		FlowCurve_Time				'粘度 時間依存
		FlowCurve_Temp				'粘度 温度依存
		NormalLT					'長時間間欠測定
		NormalNoAcum				'通常測定 配列蓄積なし
	End Enum
	'▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼
	'   構造体
	'▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲
	'連続較正中情報
	Public Structure CalibDgvInfo
		Dim sMode As String			'X:X直線動作, C:XY円動作,  P:Z直線動作
		Dim iCol As Int16			'セルカラムインデックス
		Dim iRaw As Int16			'セルロウインデックス
		Dim iState As Int16			'状態 eCalibState
	End Structure

	'''<summary> リアルタイム測定設定ファイルを保持する構造体 modDBStrcから移植</summary>
	'''<remarks> 配列がそれぞれ設定画面の表での1行に対応する。 </remarks>
	Public Structure RtgSetF
		''<summary> 設定ファイル名称。 </summary>
		Public strName As String
		'''<summary> 線形変更フラグ配列。0:非線形,1:線形 </summary>
		Public iLinearFlag As Integer()
		'''<summary> 測定条件変更タイミング配列。(単位:秒) </summary>
		Public dTiming As Double()
		''' <summary> 実行時間合計(秒) </summary>
		Public dTTotal As Double
		'''<summary> 変更タイミングごとの構造体配列。 </summary>
		Public mdbr As DataFile.DataFileHeader()
		'''<summary> 温度Z位置補正値 i秒毎(um) </summary>
		Public dZCalibPos	As Double()
		'''<summary> 温度 １秒毎(℃) </summary>
		Public dZCalibTemp	As Double()

		Public iElaspeTime	As Int32			'実行経過時間(秒)
		Public dCurrPreTemp	As Double			'現在設定温度(℃)
		Public dCurrPosDevi	As Double			'現在位置偏差(um)

		'''<summary>コンストラクタ。名称を代入する。</summary>
		Public Sub New(ByVal str As String)
			Me.strName          = str
			Me.iLinearFlag      = New Integer(m_iRTGSETMaxRow) {}
			Me.dTiming          = New Double(m_iRTGSETMaxRow) {}
			Me.mdbr             = New DataFile.DataFileHeader(m_iRTGSETMaxRow) {}
			Me.dZCalibPos       = Nothing
			Me.dZCalibTemp      = Nothing
			Me.iElaspeTime		= 0		
		End Sub
	End Structure

	'▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼
	'   API宣言
	'▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲

	'▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼
	'   パブリック変数
	'▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲

	Public p_strEquipIP				As String					'本体のIPアドレス
	Public p_strEquipPort			As String = "1024"			'本体のポート番号
	Public p_sEquipName				As String					'本体識別
	Public p_sXYSensorID			As String					'XYセンサID
	Public p_sZSensorID				As String					'ZセンサID
	Public p_strDataFolder			As String = Nothing			'データフォルダ

	Public p_mltcp					As MemLinkTCP				'メモリリンク通信を行うクラス
	Public p_MainLink				As Int16 = 0				'1:frmMainのdgvからの通信リンク(書込み側)を切る

	'''<summary> 自動開始の待ち時間。負の値で自動開始無効 </summary>
	Public p_iAutoStart As Integer = 3

	Public p_iZeroHPStart				As Int64 = 0				'ゼロ調開始位置[um]
	Public p_iStaFlucRange				As Int64 = 0				'ゼロ調終了判定 安定度の変動範囲[um]
	Public p_iStaMonTime				As Int32 = 0				'ゼロ調終了判定 安定度の監視時間[秒]
	Public p_iZeroAdjOk					As Int16 = -1				'ゼロ調終了判定 結果
	Public p_iZeroAdjOkWithOutContinue	As Int16 = -1				'ゼロ調範囲内で安定時間待ち：1  未使用
	Public p_iCenterState				As Int16 = -1				'センター出し状況
	Public p_FltSetting					As stFilterInfo				'フィルタ動作設定
	Public p_dZstressCR					As Double					'z軸安定度[g/10S]記憶 ゼロ調開始時更新

	'''<summary> 通信処理のエラー状態。。 </summary>
	Public ReadOnly Property p_ro_strErr() As String
		Get
			Return m_ro_strErrValue
		End Get
	End Property

	Public p_ConnectAfterProc	As Int32 = 0						'コネクト後の処理をメインフォームへ通知する

	'''<summary> リアルタイム測定データ保存パス 未使用</summary>
	Public p_sRealDataPath As String

    '''<summary> リアルタイム測定時に書き換えを行うSpecの名称。RTG開始前に名称を代入して使用。 </summary>
    Public p_strRTGSetName As String = ""

    '''<summary> リアルタイム測定時設定変更用構造体。 </summary>
    Public p_rtgset As RtgSetF = New RtgSetF("")

    '''<summary> リアルタイム処理の状況。RTG_ST列挙体で指定 </summary>
    Public p_iRTGState As Integer = 0

    '''<summary> リアルタイム処理についてのエラーコード。未使用 </summary>
    Public p_iRTGErr As Integer = 0

    '''<summary> リアルタイム処理の正確なデータ点数推測値。未使用 </summary>
    Public p_iRTGEstimatePtNum As Integer = 0

	Public p_bZCalibEnable As Boolean = True				'Z位置温度キャリブレーション使用可能か True:可能 False:不可



	'''<summary> 本体との接続状態。接続表示で使用。CONNECT_ST列挙体で指定 </summary>
	Public p_iConnectState As Integer = 0

	Public p_iBGWWork		As Integer = 0				'メインスレッド作業№
	Public p_iBGWOrder		As Integer = 0				'メインスレッドからBackgroundWorkerへの指示、BGW_ORDER列挙体で指定
	Public p_iBGWOrderNext	As Integer = 0				'メインスレッドからBackgroundWorkerへの指示予約。BGW_ORDER列挙体で指定
	Public p_iBGWOrderNext1	As Integer = 0				'メインスレッドからBackgroundWorkerへの指示予約。BGW_ORDER列挙体で指定
	Public p_iBGWOrderPrev	As Integer = 0				'メインスレッドからBackgroundWorkerへの指示。BGW_ORDER列挙体で指定
	Public p_iHeatingHP		As Integer = -1				'本体のヒータ状態表示に使用。-1で未取得、0でFalse、1でTrue
	Public p_iHeatingPB		As Integer = -1				'本体のヒータ状態表示に使用。-1で未取得、0でFalse、1でTrue
	Public p_iHeatingSG		As Integer = -1				'本体のヒータ状態表示に使用。-1で未取得、0でFalse、1でTrue
	Public p_iAdjustedZero	As Integer = -1				'本体のゼロ調整状態表示に使用。-1で未取得、0でFalse、1でTrue
	Public p_iEndBuzzer		As Integer = -1				'本体の測定終了時ブザー。-1で未取得、0でFalse、1でTrue 未使用
	Public p_iHeaterRangeBuzzer As Integer = -1			'本体の熱板温度設定値到達時ブザー。-1で未取得、0でFalse、1でTrue 未使用
	Public p_iHeatRdy		As Integer = -1				'温度の状況を表示
	Public p_sActualTemp	As String = ""				'現在熱板温度 未使用
	Public p_sActualPos		As String = ""				'現在位置 未使用

	'状態ステータス取得対応
	Public p_sPresetTemp As String = ""					'本体設定熱板温度  未使用
	Public p_sPresetGap As String = ""					'本体設定GAP  未使用
	''''<summary> エラー状態 </summary>
	'Public p_iErrStts1 As String = ""
	'Public p_iErrStts2 As String = ""
	'Public p_iScreenNo As Integer = -1                  '本体画面番号

	'Public p_bZeroInterLock As Boolean = False          'ゼロ調インターロック有効フラグ
	'Public p_bZeroStartInterLock As Boolean = False     '原点復帰インターロック有効フラグ
	'Public p_iEachZeroAdj As Integer = 0
	'Public p_iRotaStartLate As Integer = 0
	'Public p_iDriveStatus() As Integer = Nothing
	'Public p_iHistPulse() As Integer
	'Public p_iPrePulse As Integer
	'Public p_iPreAngle As Integer
	'Public p_iUnitAngle() As Integer
	'Public p_iUnitAnglIndex As Integer = -1
	'Public p_iOrgSensor As Integer = -1

	Public p_ulIntegTimePowerOn			As ULong = 0        '本体電源ON積算時間[秒]
	Public p_ulIntegTimeHeaterOn		As ULong = 0        '熱板ﾋｰﾀON積算時間[秒]
	Public p_ulIntegTimeHeater2On		As ULong = 0        'プローブﾋｰﾀON積算時間[秒]
	Public p_ulIntegTimeHeater3On		As ULong = 0        '起歪体ﾋｰﾀON積算時間[秒]
	Public p_ulIntegTimeXOn				As ULong = 0		'X動作積算時間[秒]
	Public p_ulIntegTimeYOn				As ULong = 0		'Y動作積算時間[秒]
	Public p_ulIntegTimeZOn				As ULong = 0		'Z動作積算時間[秒]

	Public p_sIntegResetDatePowerOn		As String = ""      '本体電源ON積算時間リセット日[西暦年4桁/月2桁/日2桁]
	Public p_sIntegResetDateHeaterOn	As String = ""      '熱板ﾋｰﾀON積算時間リセット日[西暦年4桁/月2桁/日2桁]
	Public p_sIntegResetDateHeater2On	As String = ""      'プローブﾋｰﾀON積算時間リセット日[西暦年4桁/月2桁/日2桁]
	Public p_sIntegResetDateHeater3On	As String = ""      '起歪体ﾋｰﾀON積算時間リセット日[西暦年4桁/月2桁/日2桁]
	Public p_sIntegResetDateXOn			As String = ""      'X動作ON積算時間リセット日[西暦年4桁/月2桁/日2桁]
	Public p_sIntegResetDateYOn			As String = ""      'Y動作ON積算時間リセット日[西暦年4桁/月2桁/日2桁]
	Public p_sIntegResetDateZOn			As String = ""      'Z動作ON積算時間リセット日[西暦年4桁/月2桁/日2桁]

	Public p_blIntegTimePowerOn			As Boolean = False  '本体電源ON積算時間超過
	Public p_blIntegTimeHeaterOn		As Boolean = False  '熱板ﾋｰﾀON積算時間超過
	Public p_blIntegTimeHeater2On		As Boolean = False  'プローブﾋｰﾀON積算時間超過
	Public p_blIntegTimeHeater3On		As Boolean = False  '起歪体ﾋｰﾀON積算時間超過
	Public p_blIntegTimeXOn				As Boolean = False  'X動作ON積算時間超過
	Public p_blIntegTimeYOn				As Boolean = False  'Y動作ON積算時間超過
	Public p_blIntegTimeZOn				As Boolean = False  'Z動作ON積算時間超過

	Public p_blIntegResetDatePowerOn	As Boolean = False  '本体電源ON積算時間リセット日からの経過年数超過
	Public p_blIntegResetDateHeaterOn	As Boolean = False  '熱板ﾋｰﾀON積算時間リセット日からの経過年数超過
	Public p_blIntegResetDateHeater2On	As Boolean = False  'プローブﾋｰﾀON積算時間リセット日からの経過年数超過
	Public p_blIntegResetDateHeater3On	As Boolean = False  '起歪体ﾋｰﾀON積算時間リセット日からの経過年数超過
	Public p_blIntegResetDateXOn		As Boolean = False  'X動作ON積算時間リセット日からの経過年数超過
	Public p_blIntegResetDateYOn		As Boolean = False  'Y動作ON積算時間リセット日からの経過年数超過
	Public p_blIntegResetDateZOn		As Boolean = False  'Z動作ON積算時間リセット日からの経過年数超過


	'Public p_iZeroAdjAct As Integer = 0                     'ゼロ調強制頻度(検出)





	Public p_sResinName				As String = ""          'サンプル名
	Public p_sLot					As String = ""          'LOT
	Public p_sMeasPlace				As String = ""			'測定場所
	Public p_sMeasurer				As String = ""			'測定者
	'Public p_sSensor				As String = ""			'センサー
	Public p_sCalcMethod			As String = ""			'ゲルタイム計算方法
	Public p_dDiffInterval			As Double = 0			'SLOPE法 微分区間



	Public p_dToqueThreshold		As double = 0.0			'測定応力閾値(%)
	Public p_dStressLimit			As double = 0.0			'測定終了応力(%)
	Public p_dMeasTimeLimit			As double = 500.0		'測定リミット時間
	Public p_dPressContDelayTime	As double = 100.0		'着圧制御ディレイ時間[mS]
	Public p_dYDispArea				As Double = 100.0		'Y軸表示範囲(%)
	Public p_dFullStressADVal		As Double = 0.0			'測定 100%応力較正値(uN)
	Public p_dFullStressAttenuation	As Double = 0.0			'測定 100%応力較正値(uN) 2秒周期基準のフィルタ減衰率
	'Public p_dFullStressRx			As Double = 0.0			'測定 100%応力比率
	'Public p_dFullStressRy			As Double = 0.0			'測定 100%応力比率
	Public p_dFullStressAlpha		As Double = 0.0			'測定 100%応力位相角
	Public p_dFullStressPxPhase		As Double = 0.0			'測定 100%応力X位置位相差
	Public p_dFullStressPyPhase		As Double = 0.0			'測定 100%応力Y位置位相差
	Public p_dFullStressPzPhase		As Double = 0.0			'測定 100%応力Y位置位相差
	Public p_dFullStressZero		As Double = 0.0			'測定 100%応力ゼロ応力(uN)
	Public p_dZeroStressPxPhase		As Double = 0.0			'測定 100%応力ゼロ応力位置位相差
	Public p_dZeroStressPyPhase		As Double = 0.0			'測定 100%応力ゼロ応力位置位相差
	Public p_dZeroStressPzPhase		As Double = 0.0			'測定 100%応力ゼロ応力位置位相差
	Public p_dWeightCoef			As Double = Double.NaN	'測定 dig->ｇ変換係数
	Public p_iZPosition				As Int64  = 0			'測定 熱板ゼロ位置(um)
	Public p_dGelTime				As Double = -1.0		'測定結果から算出されたゲルタイム
	Public p_iGelTimeJudge			As Int16  = 2			'ゲルタイム判定 1：OK  0：NG 2:無効
	Public p_dJudgeRangeMin			As Double = 0			'GelTime判定範囲
	Public p_dJudgeRangeMax			As Double = 0			'GelTime判定範囲
	Public p_dAprxRangeMin			As Double = 0			'データ有効範囲
	Public p_dAprxRangeMax			As Double = 0			'データ有効範囲
	Public p_dTempStubRange			As Double = 0			'HP温度安定許容範囲
	Public p_dTempStubRange2		As Double = 0			'PB温度安定許容範囲
	Public p_iHPPosCont				As Int16 = 0			'測定 熱板制御方法 0,1,2,3
	Public p_dMeasureStress			As Double = 0.0			'測定 熱板設定圧力 (kPa)
	Public p_dZStability			As Double = 0.0			'Z荷重安定度[g/10Sec] リアルタイム値
	Public p_dMesGap				As Double = 0.0			'計算処理により算出される測定中のGAP(mm) リアルタイム値 モニタ使用


	'Public p_sDeviceSpecName		As String = ""			'デバイスSPEC名
	'Public p_sSampleSpecName		As String = ""			'サンプルSPEC名
	Public p_dDiameter				As Double = 20.0		'プローブ直径(mm)
	Public p_sShape					As String = ""			'形状	
	Public p_sThickness				As Double = 0.000		'厚み(mm)
	Public p_sStacks				As Double = 0.000		'積層枚数
	Public p_sSpecGravity			As Double = 0.000		'密度
	Public p_sWeight				As Double = 0.000		'重量
	Public p_sVolume				As Double = 0.000		'体積


	Public p_dMeltVisco				As Double = -1.0		'MELT法測定結果から算出された最小粘度
	Public p_dMeltTemp				As Double = -1.0		'MELT法測定結果から算出された温度



	Public p_dDevDateSet			As Date					'本体日時設定データ
	Public p_lElapseTime			As Int32				'BGW1スキャン時間(デバッグ用)

	'手動駆動目標値
	Public p_iHotPlatePos			As Int64                '熱板目標位置(um)
	Public p_iXAxisPos				As Int32                'X軸目標位置(um)
	Public p_iYAxisPos				As Int32                'Y軸目標位置(um)
	Public p_iMAmplitudeX			As Int32				'X軸振幅
	Public p_iMAmplitudeY			As Int32				'Y軸振幅
	Public p_iMAmplitudeZ			As Int32				'Z軸振幅
	Public p_iMPeriodX				As Double				'X軸周期
	Public p_iMPeriodY				As Double				'Y軸周期
	Public p_iMPeriodZ				As Double				'Z軸周期
	Public p_iModeForManualXY		As Int32				'手動駆動 XY駆動モード

	Public p_MeasMode				As Int16				'0:GelTime 1:Flowcurve

	Public p_MeasureState As Int32 = eMeasStat.NOT_START	'測定状態 
														'	-5:測定中止 
														'	-1:未開始 
														'	0:予備 
														'	1:準備中(投入GAP下降+クリア時間) 
														'	2: 
														'	3:データ転送中
														'	5:終了中

	'Public p_StabilityForMeasEnd	As Double = 99.999		'測定終了条件 区間変化量安定度(%) 測定データスパンからの変化率
	'Public p_SampleNumForPeriod		As Int64				'変化量を算出する指定周期による区間内サンプル数

	Public p_CaliblationState		As CalibDgvInfo			'較正状態 -5:較正中止 -1:未開始 0:予備 1:準備中 2:条件インデックス確定 3:データ転送中 5:終了中

	Public p_iCalibMode				As int16				'0:Full Stress 1:Zero Stress
	Public p_frmCalibObj			As frmCalibFull			'キャリブレーションフォーム参照
	Public p_ClbMode				As String = Nothing		'A：X,C一括実行
	Public p_ClbXIdx				As int32				'較正 X動作  現在測定インデックス
	Public p_ClbCIdx				As int32				'較正 円動作 現在測定インデックス
	Public p_ClbPIdx				As int32				'較正 円動作 現在測定インデックス
	Public p_ClbX()					As stPoint				'較正 X動作  測定条件・結果
	Public p_ClbC()					As stPoint				'較正 円動作 測定条件・結果
	Public p_ClbZ()					As stPoint				'較正 Z動作 測定条件・結果
	Public p_iDataCount				As Int64				'蓄積データ数
	'Public p_ClbAveSizeRate			As Double = 20.0		'平均対象とするサンプルデータの後半割合(%)
	Public p_AutoPeelingTemp		As Double = 220.0		'自動剥がし時の設定温度
	Public p_CalibPreTemp			As Double = 40.0		'較正中設定温度
	Public p_CalibMesGap			As Double = 0.01		'較正中GAP
	Public p_CalibInpGap			As Double = 10.0		'較正中入力GAP
	'Public p_CalibWaitXYAct		As Double = 120			'較正中XY動作待ち
	'Public p_CalibWaitInpResin		As Double = 5			'較正中クリア時間
	'Public p_CalibWaitRisingHP		As Double = 3			'較正中熱板上昇待ち時間

	Public p_FlcvMeasMode			As Int16  = 0			'FLCV 測定モード
	Public p_FlcvActMode			As Int16  = 0			'FLCV 動作モード 0:円 1:X方向
	Public p_FlcvPreTemp			As Double = 25.0		'FLCV 設定温度
	Public p_FlcvPeriod				As Double				'FLCV 設定 動作周期(S)
	Public p_FlcvAmplitude			As Double				'FLCV 設定 動作振幅(mm)<温度依存>
	Public p_FlcvChgVal()			As Double = Nothing		'FLCV 設定 測定振幅列(mm)<速度依存>/測定温度列(℃)<温度依存>
	Public p_FlcvStart				As Double = Nothing		'FLCV 設定 測定振幅列(mm)<速度依存>/測定温度列(℃)<温度依存>
	Public p_FlcvEnd				As Double = Nothing     'FLCV 設定 測定振幅列(mm)<速度依存>/測定温度列(℃)<温度依存>
	Public p_FlcvStep				As Double = Nothing     'FLCV 設定 測定振幅列(mm)<速度依存>/測定温度列(℃)<温度依存>

	Public p_FlcvPreshearAmplitude	As Double               'FLCV 設定 前処理 動作振幅(mm)<時間依存>
	Public p_FlcvRecoveryAmplitude	As Double               'FLCV 設定 回復処理 動作振幅(mm)<時間依存>
	Public p_FlcvLow1Amplitude		As Double               'FLCV 設定 初期化低速 動作振幅(mm)<時間依存>
	Public p_FlcvLow2Amplitude		As Double               'FLCV 設定 経過観察低速 動作振幅(mm)<時間依存>
	Public p_FlcvHighAmplitude		As Double               'FLCV 設定 構造破壊高速 動作振幅(mm)<時間依存>
	Public p_FlcvPreshearTime		As Double               'FLCV 設定 前処理 動作時間(秒)<時間依存>
	Public p_FlcvRecoveryTime		As Double               'FLCV 設定 回復処理 動作時間(秒)<時間依存>
	Public p_FlcvLow1Time			As Double               'FLCV 設定 初期化低速 動作時間(秒)<時間依存>
	Public p_FlcvLow2Time			As Double               'FLCV 設定 経過観察低速 動作時間(秒)<時間依存>
	Public p_FlcvHighTime			As Double               'FLCV 設定 構造破壊高速 動作時間(秒)<時間依存>

	Public p_FlcvThinning			As Int32				'FLCV 時間依存グラフ　間引き

	Public p_FlcvZero()				As Double = Nothing		'FLCV 設定 Zero応力(uN)
	Public p_FlcvSample				As Int32				'FLCV 設定 サンプル数
	Public p_FlcvAveSizeRate		As Double = 50.0		'FLCV 設定 平均対象とするサンプルデータの後半割合(%)
	Public p_FlcvPbDiameter			As Double = 20.0		'FLCV プローブ直径(mm)
	Public p_FlcvReverse			As Boolean				'FLCV 逆行測定 有効無効

	Public p_FlcvOpeTime			As Double				'FLCV 動作時間(S)
	Public p_FlcvRstTime			As Double				'FLCV 停止時間(S)
	Public p_FlcvCycle				As Int32				'FLCV サイクル回数設定

	Public p_FlcvCycleOnCnt			As Int32 = 0			'FLCV サイクル回数(現在値)
	Public p_FlcvCycleOfCnt			As Int32 = 0			'FLCV サイクル回数(現在値)
	Public p_FlcvArrayUpdate		As Int16 = 0			'FLCV 結果データ更新(1:更新中 0:更新中でない)
	'
	Public p_uVp					As DataFile.ViscosityCalcParameter




	Public p_dPosHeatBHome			As Double				'熱板待機位置 アイドル中の熱板位置[mm]  未使用
	'Public p_NoFilterTime			As Double = -1.0		'計測されたフィルタ無効時間(秒)

	Public p_Horizon()				As stZeroMem			'水平確認
	Public p_HrznIdx				As Int16 = -1			'水平確認配列インデックス
	Public p_iHorizonConfOperation	As Int16 = -1			'水平確認操作状態 -1:未設定 0:操作待ち 1:次の測定点へ 2:測定終了
	Public p_dZeroStaPosHP			As Double				'ゼロ調開始位置退避  未使用

	Public p_ElapsedMeasTime		As Double = -1			'測定経過時間(秒)

	Public p_dZPosAfter				As Double = -1			'着圧決定後Z位置(um)

	Public p_ZeroAdjBuf()			As Double
	Public p_ZeroBufIdx				As int32
	'Public p_ZeroPointConf			As int32				'Zero点位置確定 ゼロ調開始で0 ゼロ点位置のゼロを確認->ゼロ以外で1 測定開始条件へ

	Public p_CenterXReg				As Int32	= 0			'センター座標X 手動登録値
	Public p_CenterYReg				As Int32	= 0			'センター座標Y 手動登録値

	Public p_iError					As Int16	= 0			'1:エラー発生中
	Public p_bDevInternalComError	As Boolean	= False		'装置内部通信エラー
	Public p_bDevError				As Boolean	= False		'本体エラー有無
	Public p_iDevErrorCount			As UInt32				'未使用

	'▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼
	'   プライベート変数
	'▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲
	Private m_ro_strErrValue As String = ""
	'''<summary> 通信処理を行う。 </summary>
	Private WithEvents m_bgwTCP As BackgroundWorker
	Private m_blBgwAlive_Tcp As Boolean = False             'スレッド生存中

    '''<remarks> 線形変更の場合は定期的に値を変更する。変更間隔は1秒とする。 </remarks>
    Private m_mdbrRTGSetNext As Datafile.DataFileHeader = New Datafile.DataFileHeader()


	Public ReadOnly Property isBGWAlive() As Boolean
		Get
			Return m_blBgwAlive_Tcp
		End Get
	End Property
	Private m_blMntTodayChcked As Boolean = False           '本日のメンテナンス積算時間超過表示済み
	Private iPreZPos		As Int64						'ゼロ調整時の熱板位置監視用 前回値退避

	Private iCalibFirst		As Int64						'100%応力測定回数（制御処理開始を判断）
	Private	m_HorizonZPos	As Int64						'XY原点のZ位置を記憶
	Private	m_iCorrRet		As Int32						'リアルタイム中間値補正 処理結果
	Private m_iStartIdx		As Int64						'リアルタイム中間値補正 開始インデックスの退避
	Private m_bRealCorrect	As Boolean						'リアルタイム中間値補正 処理有効
	Private m_iCorrectCount	As Int64						'リアルタイム中間値補正 処理回数
	Public ReadOnly Property GetCorrCount() As Int32
		Get
			Return m_iCorrectCount
		End Get
	End Property
	
	Private m_iPeriodSample	As Int32						'リアルタイム中間値補正 1周期分のサンプル個数
	Private m_cts			As CToolSettings				'オプション設定参照の生成
	Private m_bFirstConnect	As Boolean = False				'最初のコネクション時 True

	''' <summary>
	''' 本体エラー発生中か
	''' </summary>
	''' <returns>TRUE:4発生中</returns>
	Public Function ErrorDetect() As Boolean
#if 0
		'装置内SPI通信エラー
		Const cObCount As Int32 = 100
		If p_mltcp.MainSpiOK<cObCount OrElse p_mltcp.ComSpiOK<cObCount OrElse 65535-cObCount < p_mltcp.MainSpiOK OrElse 65535-cObCount < p_mltcp.ComSpiOK
		'16bitオーバーフローを考慮し不感帯を作成
			p_bDevInternalComError = False
		Else
			Dim iTemp1 As Int32 = p_mltcp.MainSpiOK
			Dim iTemp2 As Int32 = p_mltcp.ComSpiOK
			p_iDevErrorCount = Math.Abs(iTemp1 - iTemp2)
			p_bDevInternalComError = p_iDevErrorCount > cObCount
		End If
#else
		'p_bDevInternalComError = False
#End If
		'Return (p_mltcp.DevErr1 And &HFFFC) <> 0 Or
		'		p_mltcp.DevErr2 <> 0 Or
		'		(p_mltcp.DevErr3 And &HFBEF) <> 0 Or 
		'		p_bDevInternalComError
		Return (p_mltcp.DevErr1 And &HFFFC) <> 0 Or
				p_mltcp.DevErr2 <> 0 Or
				(p_mltcp.DevErr3 And &HF9EF) <> 0 Or 
				p_bDevInternalComError
	End Function

	Private m_iLTSampleNum	As Int32						'長期間欠モード 平均化に使用するサンプル個数

	Private m_frmM			As frmMain						'frmMain参照

	'■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
	'   コンストラクタ／デストラクタ
	'■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
	'''<summary> コンストラクタ。初期化を行う。 </summary>
	Public Sub New(ByRef frmM As frmMain)
		p_mltcp					= New MemLinkTCP()
		p_mltcp.p_strEquipIP	= p_strEquipIP
		p_mltcp.p_strEquipPort	= p_strEquipPort
		p_iBGWOrder				= BGW_ORDER.NO_ORDER
		m_cts					= p_cts					'オプション設定の参照をセット
		m_frmM					= frmM					'frmMain参照
	End Sub

	'■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
	'   スレッドイベント
	'■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
	'================================================
	'バックグラウンド動作開始
	'引数	なし
	'戻り値 なし
	'================================================
	'''<summary> バックグラウンド動作を開始する。 </summary>
	Public Sub StartBGW()
		Try
			If IsNothing(m_bgwTCP) Then
				m_bgwTCP = New BackgroundWorker()
				m_bgwTCP.WorkerReportsProgress = True
			End If
			m_bgwTCP.RunWorkerAsync()
		Catch ex As InvalidOperationException
			'Console.WriteLine(ex.Message)
			Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
		End Try
	End Sub

	'================================================
	'バックグラウンド動作キャンセル
	'引数	なし
	'戻り値 なし
	'================================================
	'''<summary> バックグラウンド動作をキャンセルする。 </summary>
	Public Sub CancelAndDisposeBGW()
		m_bgwTCP.WorkerSupportsCancellation = True
		m_bgwTCP.CancelAsync()
		Call WaitBgwEnd("m_bgwTCP", m_blBgwAlive_Tcp)
		m_bgwTCP.Dispose()
	End Sub
Dim bPreOriginState As Boolean = False				'原点SW状態のスキャン前回値
Dim bPreResetState As Boolean = False				'リセットSW状態のスキャン前回値
'''<summary>
'''マルチスレッド処理で通信操作を行う。
'''</summary>
Private Sub BGWWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles m_bgwTCP.DoWork
	'現在の作業を表す変数。BGW_WORK列挙体で指定
	Dim sw0 As New Stopwatch()      'スキャン時間計測用
	Dim sw1 As New Stopwatch()      'タイムアウト計測用
	Dim sw2 As New Stopwatch()      '応力較正制御用
	Dim sw3 As New Stopwatch()      'ゲルタイム測定制御用
	Dim sw4 As New Stopwatch()      '測定中からXY動作開始までの時間計測用
	Dim sw5 As New Stopwatch()      'ゼロ調安定度
	Dim sw6 As New Stopwatch()      '測定経過時間(秒)
	Dim sw7 As New Stopwatch()      '水平確認
	Dim sw8 As New Stopwatch()      'PIDモニタ
	Dim sErr As String = New String("")

	m_blBgwAlive_Tcp = True         'スレッド生存中

	'本体通信系スレッド
	While p_iBGWWork >= 0
		sw0.Start()                 'ストップウオッチ0開始(スキャンタイム計測用)
		'常時読込み 停止
		If m_bgwTCP.CancellationPending Then
			e.Cancel = True
			Exit While
		End If

		Try

			'指示に応じた通信処理を行う
			Select Case p_iBGWWork
				Case BGW_WORK.AUTO_CONNECT

				'自動接続制御  3秒ごとに接続を試みる
					If p_mltcp.TCP_IsConnected() = False Or	p_iConnectState = CONNECT_ST.OFFLINE Or p_mltcp.IsLinkError() <> 0
						sErr = Nothing
						If p_mltcp.TCP_Connect(p_strEquipIP, p_strEquipPort, sErr) Then
							sErr = Nothing

							p_mltcp.p_strEquipIP	= p_strEquipIP
							p_mltcp.p_strEquipPort	= p_strEquipPort
							If p_mltcp.ReadBatch(p_dtFile) = 0															'[[[[一括読込(通常)]]]]

								Call WriteLog("接続完了 リンク開始", eLOG_LEVEL.iINFO)

								p_sEquipName			= p_mltcp.DevSerealRead												'機器シリアル番号を取得
								p_cts.strEquipName(p_cts.iSelMDK) = "SZK" & CInt(p_sEquipName).ToString("000")				'本体IDをセット
								modPublic.p_sEquipName	= p_cts.strEquipName(p_cts.iSelMDK)
								p_sEquipName			= p_cts.strEquipName(p_cts.iSelMDK)
								p_ConnectAfterProc = 1

								p_iConnectState			= CONNECT_ST.CONNECTING                 'LINK中へ
								p_iBGWWork				= BGW_WORK.NO_ORDER
								m_bFirstConnect			= True
								modFrmMain.SetForderCreate()						'データ管理フォルダ環境生成
								'初期化ファイルを読込む
								p_cts.LoadDeviceSettingINI(modPublic.p_sEquipName, False)			'装置毎設定読込
								p_cts.LoadTempParaINI(modPublic.p_sEquipName, False)				'温調パラメータ読込

								'p_iBGWOrderNext			= p_iBGWOrder
							End If
						Else
							p_iConnectState = CONNECT_ST.OFFLINE
							m_ro_strErrValue = p_mltcp.p_strErrState
							p_mltcp.TCP_DisConnect()
							'------- 指令ビットを全てOFF -------
							p_mltcp.StartTempCont	= 0
							p_mltcp.StartSW			= 0
							p_mltcp.PosOrder		= 0
							p_mltcp.DateSetOrder	= 0
							p_iZeroAdjOk			= 0                                    'ゼロ調正完了無効
							'-----------------------------------
						End If
						System.Threading.Thread.Sleep(500)							'スリープ
					Else
						p_iBGWWork = BGW_WORK.NO_ORDER
						Call WriteLog("再接続制御するが接続済", eLOG_LEVEL.iINFO)
					End If

				Case BGW_WORK.NO_ORDER                                          '現在、処理を実行中でない
					'接続、リンク監視
					' --- 本体リンク読込処理 -------------------------------------
'#################################################################################################################
					p_mltcp.ReadBatch(p_dtFile)                                         '[[[[一括読込(通常)]]]]
'#################################################################################################################
					System.Threading.Thread.Sleep(30)                                   'スリープ
					p_mltcp.PCAlive() = p_mltcp.DevAlive()								'本体死活信号をPC死活信号として書き戻し
#If 0 '### 本番注意  #########################################
					'PC通信エラー有効/無効
					''p_mltcp.PCComErrorNoUse(FALSE)				'PC通信エラー有効
					If p_ConnectAfterProc = 2
						p_ConnectAfterProc = 0
					End If
#else
					'p_mltcp.PCComErrorNoUse(TRUE)				'PC通信エラー無効
					p_mltcp.PCComErrorNoUse(p_bPcLifeCheckDis)	'PC生存チェック有効無効
					'リンク直後、ヒータONの時はONの状態とする
					If p_ConnectAfterProc = 1
						If p_mltcp.isStatHeaterOn3()
							'温度制御開始Bit ON HP,PB,SG
							p_mltcp.StartTempCont = p_mltcp.StartTempCont Or MemLinkTCP.eWTempBit.iHP 
						End If
						If p_mltcp.isStatHeaterOn2()
							'温度制御開始Bit ON HP,PB,SG
							p_mltcp.StartTempCont = p_mltcp.StartTempCont Or MemLinkTCP.eWTempBit.iProbe
						End If
						If p_mltcp.isStatHeaterOn()
							'温度制御開始Bit ON HP,PB,SG
							p_mltcp.StartTempCont = p_mltcp.StartTempCont Or MemLinkTCP.eWTempBit.iSG
						End If
						p_ConnectAfterProc = 2
					End If
#end if

					' --- 本体リンク書込処理 -------------------------------------
'#################################################################################################################
					p_mltcp.WriteBatch()                                                '[[[[一括書込み]]]]
'#################################################################################################################
					If p_mltcp.IsLinkError() <> 0 Then
						'リンク異常発生
						p_iConnectState			= CONNECT_ST.OFFLINE
						p_iBGWWork				= BGW_WORK.AUTO_CONNECT                                  '自動接続制御へ
						Call WriteLog("リンク異常発生", eLOG_LEVEL.iINFO)
					Else
						p_iHeatingHP			= IIf(p_mltcp.isErrNoHeatUp(), 1, 0)				'1051 bit6  No HeatUp
						p_iHeatingPB			= IIf(p_mltcp.isErrNoHeatUp2(), 1, 0)				'1051 bit11 No HeatUp
						p_iHeatingSG			= IIf(p_mltcp.isErrNoHeatUp2(), 1, 0)				'1051 bit11 No HeatUp
						'1060 熱板温度範囲内 + 1052 プローブ温度範囲内 + 1189 起歪体温度範囲内
						p_iHeatRdy				= p_mltcp.DevTempNorm + p_mltcp.DevTemp2Norm + p_mltcp.DevTemp3Norm		
						''p_mltcp.PosHeatBHome	= Int64.Parse(p_dPosHeatBHome * 1000.0)				'熱板退避位置[mm]
						p_sActualTemp			= p_mltcp.HeatTempAct.ToString("0.0")				'現在温度
						p_sActualPos			= p_mltcp.HeatBPos									'現在位置
						p_sPresetTemp			= p_mltcp.TempPreset(1).ToString("0.0")				'設定温度
						p_sPresetGap			= p_mltcp.GapPreset(1).ToString("0.00")				'設定GAP

						p_ulIntegTimePowerOn	= p_mltcp.IntegTimePowOn							'本体電源ON時間
						p_ulIntegTimeHeaterOn	= p_mltcp.IntegTimeHeatOn							'熱板ヒータON時間
						p_ulIntegTimeHeater2On	= p_mltcp.IntegTimePBHeat							'プローブヒータON時間
						p_ulIntegTimeXOn		= p_mltcp.IntegTimeXAct								'X動作ON時間
						p_ulIntegTimeYOn		= p_mltcp.IntegTimeYAct								'Y動作ON時間
						p_ulIntegTimeZOn		= p_mltcp.IntegTimeZAct								'Z動作ON時間

						p_sIntegResetDatePowerOn = IntToDateStringConv(p_mltcp.IntegDatePowOn)		'本体電源ON積算時間リセット日付
						p_sIntegResetDateHeaterOn = IntToDateStringConv(p_mltcp.IntegDateHeatOn)	'熱板ヒータON積算時間リセット日付
						p_sIntegResetDateHeater2On	= IntToDateStringConv(p_mltcp.IntegDatePbHeat)	'ブローブヒータON積算時間リセット日付
						p_sIntegResetDateXOn		= IntToDateStringConv(p_mltcp.IntegDateXAct)	'X動作ON積算時間リセット日付
						p_sIntegResetDateYOn		= IntToDateStringConv(p_mltcp.IntegDateYAct)	'Y動作ON積算時間リセット日付
						p_sIntegResetDateZOn		= IntToDateStringConv(p_mltcp.IntegDateZAct)	'Z動作ON積算時間リセット日付

						p_DevSerialRead			= p_mltcp.DevSerealRead								'読込み本体装置番号


						p_bDevError = ErrorDetect()					'本体エラーセット
If p_bDevError
	'Debug.Print("")
End If


						If m_bFirstConnect
						'接続直後は日付時刻設定
							'p_iBGWOrder = BGW_ORDER.SETDATE_REP    '日付時刻設定制御へ
						End If


						'本体エラーリセットSW確認 ON
						If p_iBGWOrder = BGW_ORDER.NO_ORDER And p_iBGWOrderNext = BGW_ORDER.NO_ORDER
							If (p_mltcp.isStatResetSW()<>bPreResetState) And Not bPreResetState
								p_iBGWOrder = BGW_ORDER.RESET_ERROR_REP
								p_iError = 0
							End If
						End If
						bPreResetState = p_mltcp.isStatResetSW()

						'本体原点復帰SW確認 ON
						If p_iBGWOrder = BGW_ORDER.NO_ORDER And p_iBGWOrderNext = BGW_ORDER.NO_ORDER
							If p_mltcp.isILState()　Or p_mltcp.isILDisable			'IL(扉閉) OKか? IL無効も見る
								If (p_mltcp.isStatOriginSW()<>bPreOriginState) And Not bPreOriginState
									p_iBGWOrder = BGW_ORDER.ORG_BUTTON_REP
								End If
							End If
						End If
						bPreOriginState = p_mltcp.isStatOriginSW()


'メインスレッドからの指示を処理
'#################################################################################################################
'#################################################################################################################
						If Not p_cts.LTModeEnable
						'通常モード
							GelTimeMeasureProc(sw1,sw2,sw3,sw6)		'<<<<ゲルタイム測定制御>>>>
						Else
						'長時間モード(間欠測定)
							GelTimeMeasureProcLT(sw1,sw2,sw3,sw6)	'<<<<ゲルタイム測定制御 長時間制御>>>>
						End If
						CaliblationProc(sw1,sw2)				'<<<<FULL/ZERO応力較正制御>>>>
						Select Case p_FlcvMeasMode
						Case eViscoMeaure.SpeedDep, eViscoMeaure.TempDep
							FlowCurve1Proc(sw1,sw2)				'<<<<フローカーブ(速度,温度依存)測定制御>>>>
						Case eViscoMeaure.TimeDep
							''FlowCurve2Proc(sw1,sw2,sw3,sw6)		'<<<<フローカーブ(時間依存)測定制御>>>>
							FlowCurve3Proc(sw1,sw2,sw3,sw6)		'<<<<フローカーブ(時間依存)測定制御 3ステップチキソトロピー試験>>>>
						End Select
						ZeroAdjustmentProc(sw1,sw5)				'<<<<ゼロ調正制御>>>>
						HorizontalConfirmProc(sw1,sw7)			'<<<<水平確認制御>>>>
						CenterSearchProc(sw1)					'<<<<中心探索制御>>>>
						ManualDriveProc(sw1)					'<<<<手動駆動制御>>>>
						ReturnOriginProc(sw1)					'<<<<原点復帰制御>>>> 
						TimeSetProc(sw1)						'<<<<日時設定制御>>>> 
						ProductSetProc(sw1)						'<<<<装置名設定制御>>>> 
						HeaterProc(sw1)							'<<<<ヒータ制御>>>> 
						ErrorReSetProc(sw1)						'<<<<エラーリセット制御>>>>
						MenteResetProc(sw1)						'<<<<メンテナンス情報リセット制御>>>>
						ZNoLoadSetProc(sw1)						'<<<<Z無負荷応力設定制御>>>> 

'#################################################################################################################
'#################################################################################################################
						'指示予約がある場合
						If p_iBGWOrderNext <> BGW_ORDER.NO_ORDER Then
							p_iBGWOrder		= p_iBGWOrderNext
							p_iBGWOrderNext = BGW_ORDER.NO_ORDER
						Else
							p_iBGWOrder		= BGW_ORDER.NO_ORDER
						End If
						System.Threading.Thread.Sleep(0)                           'スリープ
						p_iBGWOrderPrev		= p_iBGWOrder                          '今回指示を退避

					End If
				'装置変更のため切断を行う。
				Case BGW_ORDER.DISCONNECT
					p_mltcp.TCP_DisConnect()
					p_iConnectState = CONNECT_ST.OFFLINE
					p_iBGWWork = BGW_WORK.AUTO_CONNECT

			End Select
#If 0
			'指示予約がある場合
			If p_iBGWOrderNext <> BGW_ORDER.NO_ORDER Then
				p_iBGWOrder		= p_iBGWOrderNext
				p_iBGWOrderNext = BGW_ORDER.NO_ORDER
			Else
				p_iBGWOrder		= BGW_ORDER.NO_ORDER
			End If
			System.Threading.Thread.Sleep(0)                           'スリープ
			p_iBGWOrderPrev		= p_iBGWOrder                          '今回指示を退避
#end if

			sw0.Stop()
			p_lElapseTime = sw0.Elapsed.Seconds * 1000 + sw0.Elapsed.Milliseconds
			sw0.Reset()

		Catch ex As Exception
			Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
		End Try
	End While

	m_blBgwAlive_Tcp = False           'スレッド終了
End Sub
Dim m_bPcLifeCheckDisBack As Boolean = False
''' <summary>
''' 測定制御
''' </summary>
''' <param name="sw1"></param>
''' <param name="sw2"></param>
''' <param name="sw3"></param>
''' <param name="sw6"></param>
Private Sub GelTimeMeasureProc(byRef sw1 As Stopwatch, byRef sw2 As Stopwatch, byRef sw3 As Stopwatch, byRef sw6 As Stopwatch)
Try
	Select Case p_iBGWOrder
	Case BGW_ORDER.START_BUTTON
		If p_iConnectState = CONNECT_ST.CONNECTING Then                                     'リンク中のみ
			If p_mltcp.isStatMeasuring()
				p_mltcp.MeasSW(False)													    '測定開始指令Bit OFF
				p_MeasureState = eMeasStat.NOT_START                                        '未測定中
			Else
				p_mltcp.MeasSW(True)											            '測定開始指令Bit ON
				p_MeasureState = eMeasStat.PREPAREING_1                                     '測定準備中
				p_iBGWOrderNext = BGW_ORDER.START_BUTTON_CNF                                '応答確認へ
				sw1.Restart                                                                 '応答Bit タイマースタート
			End If
			p_CaliblationState.iState = eCalibState.NOT_START								'較正用ステータス->未実行(ZERO調後のクリア)
		End If

	Case BGW_ORDER.START_BUTTON_CNF
		'測定SW応答確認
		If p_mltcp.isRespMeasStart() AndAlso p_mltcp.isStatMeasuringTempo()					'応答bit[1108.00]、測定中ステータス[1109.02]

			'ファイル名
			If p_bMeasTimeEn
				p_dtFile.SetDataFileName = p_dtFile.sMeasindCode(eMeasMode.NormalNoAcum) & "-" & Format(Now, "yyyyMMddHHmmss")
			End If
			MeasureHeaderInfoSet(If(p_bMeasTimeEn,  eMeasMode.NormalNoAcum, eMeasMode.Normal))	'測定開始時のファイルヘッダ情報のセット

			p_dGelTime = -1.0                                                               'ゲルタイム値クリア
			p_mltcp.PreMesClear()															'前回測定№クリア

			'区間変化量計算に必要なサンプル数計算 必要周期(=10)分 周期 * 区間数(10) / サンプルインターバル
			'p_SampleNumForPeriod = Ctype(p_mltcp.PeriodX(1) * 1000.0 * 10 / MEAS_TIME_RESOLUTION, Int64)
			System.Threading.Thread.Sleep(500)
			p_MeasureState = eMeasStat.PREPAREING_1
			p_iBGWOrderNext = BGW_ORDER.MEAS_PREPARE_1	                                    '測定準備1へ
			sw3.Restart()																	'測定制限時間SW開始
		Else
			If sw1.Elapsed.TotalSeconds > cResponseTimeOut Then
				'応答BIT未確認タイムオーバー	5秒
				p_mltcp.MeasSW(False)													    '測定開始指令Bit OFF
				Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & ":Confirm Bit Response Time Over (START_BUTTON_CNF)")
				sw1.Restart()
				p_iBGWOrderNext = BGW_ORDER.MEAS_PRO_FINISH									'測定終了へ
			Else
				p_iBGWOrderNext = BGW_ORDER.START_BUTTON_CNF                                '応答確認継続
			End If
		End If
		'測定キャンセル確認
		If p_MeasureState < eMeasStat.RESERV
			p_mltcp.MeasSW(False)															'測定開始指令Bit OFF
			p_iBGWOrderNext = BGW_ORDER.NO_ORDER
		End If
		'本体エラー発生時
		If p_bDevError
			p_iBGWOrderNext = BGW_ORDER.MEAS_ERROR_CANCEL
		End If

	Case BGW_ORDER.MEAS_PREPARE_1
		p_dMesGap = 0
		If (p_mltcp.isRespMeasStart() = False) Or (p_mltcp.isStatMeasuringTempo()=False)	'応答bit[1108.00]、測定中ステータス[1109.02]
			'本体側が測定終了した
			p_mltcp.MeasSW(False)															'測定開始指令Bit OFF
			sw1.Restart()
			p_iBGWOrderNext = BGW_ORDER.MEAS_PRO_FINISH										'測定終了へ
		Elseif p_mltcp.isStatMeasuring() = True
		'測定中(クリア後)がON-> ゲルタイム ０秒 ここから測定データ取得開始
			m_iCorrRet = 0
			m_iStartIdx = 0
			m_bRealCorrect = False
			m_iCorrectCount = 0
			m_iPeriodSample = p_mltcp.PeriodX(1) * 1000 / MEAS_TIME_RESOLUTION			'1周期分のサンプル数計算
			sbLoadRTGSetting(p_strRTGSetName)											'温度プロファイル読込
			p_mltcp.P_XStressZero = p_mltcp.P_XStressZero + p_mltcp.CurrStrsX()			'X応力
			p_mltcp.P_YStressZero = p_mltcp.P_YStressZero + p_mltcp.CurrStrsY()			'Y応力
''			p_mltcp.P_ZStressZero = p_mltcp.P_ZStressZero + p_mltcp.CurrStrsZ()			'Z応力
''			p_mltcp.P_ZStressZero = p_mltcp.P_ZStressZero + p_mltcp.CurrStrsZCorrA()	'Z応力(補正後AD直)


			sw6.Restart()
			p_ElapsedMeasTime = sw6.Elapsed.TotalSeconds
			p_MeasureState = eMeasStat.DATA_TRANS
			p_iBGWOrderNext = BGW_ORDER.MEAS_PROCESS	                                '測定終了監視へ
		Else
			p_iBGWOrderNext = BGW_ORDER.MEAS_PREPARE_1	                                '継続
		End If
		'本体エラー発生時
		If p_bDevError
			p_iBGWOrderNext = BGW_ORDER.MEAS_ERROR_CANCEL
		End If
'測定手動終了中止制御  -------------------------------------------------------------------------------------
	Case BGW_ORDER.CANCEL_BUTTON

		'If p_MeasureState = eMeasStat.DATA_TRANS
		'	p_iBGWOrderNext = BGW_ORDER.WAIT_LOCKOUT									'終了データ処理へ
		'Else
		'	p_iBGWOrderNext = BGW_ORDER.MEAS_PRO_FINISH									'測定終了へ
		'	sw1.Restart()
		'End If
		If p_dtFile.MesBlockCnt <> 0
		'一度でもデータ受信した
			p_iBGWOrderNext = BGW_ORDER.WAIT_LOCKOUT									'終了データ処理へ
		Else
		'未だデータ受信していない->測定開始をキャンセルしてゼロ調開始待機状態
			p_ElapsedMeasTime = -1
			p_iZeroAdjOk = -1                                                                   'ZERO調未実施へ
			p_mltcp.ZeroSW(False)                                                               'ZERO調制御開始Bit OFF
			p_mltcp.MeasSW(False)                                                               '測定開始指令Bit OFF
			p_MeasureState =  eMeasStat.NOT_START
			p_iBGWOrderNext = BGW_ORDER.MEAS_BEFORE_CANCEL
		End If

	Case BGW_ORDER.MEAS_BEFORE_CANCEL
		Thread.Sleep(500)
		p_iBGWOrderNext = BGW_ORDER.NO_ORDER

	Case BGW_ORDER.MEAS_ERROR_CANCEL
		Thread.Sleep(500)
		p_iBGWOrderNext = BGW_ORDER.WAIT_LOCKOUT


'-----------------------------------------------------------------------------------------------------------
'測定データ取得制御  -------------------------------------------------------------------------------------

	Case BGW_ORDER.MEAS_PROCESS
		'測定終了監視
		If (p_mltcp.isRespMeasStart() = False) Or (p_mltcp.isStatMeasuringTempo() = False)  '応答bit[1108.00]、測定中ステータス[1109.02]
			'本体側が測定終了した
			Call WriteLog("測定終了 (装置終了)", eLOG_LEVEL.iINFO)
			p_iBGWOrderNext = BGW_ORDER.WAIT_LOCKOUT                                       '測定したところまでで測定終了しデータ処理
		ElseIf p_mltcp.isItLock()
			'扉オープンされた
			p_iBGWOrderNext = BGW_ORDER.WAIT_LOCKOUT                                       '測定したところまでで測定終了しデータ処理
		Else
			p_ElapsedMeasTime = sw6.Elapsed.TotalSeconds
			'測定実施中
			If p_mltcp.bMesDataEnable Then
			'測定データ有効

				'測定データ蓄積セーブ -------------------
				If p_bMeasTimeEn
					p_dtFile.RealTimeMeasDataFileSaveText(eMeasMode.NormalNoAcum, True, m_cts)             '測定値累積保存 追加モード(蓄積配列なし)
				Else
					p_dtFile.RealTimeMeasDataFileSaveText(eMeasMode.Normal, True, m_cts)                   '測定値累積保存 追加モード
				End If


#If 1
'リアルタイム計算処理
			'Z決定位置をセット
			If p_mltcp.ZConfirmPos<>0
				p_dtFile.p_head.dZAfterPos = Ctype(p_mltcp.ZConfirmPos, Double) / 10.0
			End If
			If p_bMeasTimeEn
			Else
			'リアルタイム中間値補正処理 ----------------------
'				If p_dtFile.DataCount > m_iPeriodSample 
				'If p_dtFile.DataCount-m_iStartIdx >= m_iPeriodSample 
				'1周期分のデータ取得まで待つ
					if m_iCorrectCount = 0
					'初回は0インデックスから
						m_iStartIdx = 0
						Select Case p_clSpec.spi.iStressSensMode
						Case eStrsSens.Point
							datCal.AveTempClear(p_mltcp.PeriodZ(1))			'クリア
						Case Else
							datCal.AveTempClear(p_mltcp.PeriodX(1))			'クリア
						End Select
					Else
						'm_iStartIdx = p_dtFile.DataCount - (m_iPeriodSample + MEAS_BLOCK_DATA_NUM)	'開始インデックスの退避
						'm_iStartIdx = p_dtFile.DataCount - MEAS_BLOCK_DATA_NUM	'開始インデックスの退避
					End If
					'リアルタイム中間値補正
					Dim cp As DataCal.stCalcPara = Nothing
					cp.iMeas		= eHoseiMeasMode.GelTime
					cp.iMode		= eHoseiCalcMode.RealTime
					cp.cts			= m_cts
					cp.clDt			= p_dtFile
					Select Case p_clSpec.spi.iStressSensMode
					Case eStrsSens.Point
						cp.dAmplitude	= p_mltcp.AmplitudeZ / 1000.0
						cp.dPeriod		= p_mltcp.PeriodZ(1)
					Case Else
						cp.dAmplitude	= p_mltcp.AmplitudeX / 1000.0
						cp.dPeriod		= p_mltcp.PeriodX(1)
					End Select
					cp.iSampleStart	= m_iStartIdx
					cp.iSampleNum	= p_dtFile.iHosei.Length - m_iStartIdx
					cp.fltSet		= p_FltSetting
					cp.dXYWait		= p_mltcp.MesPreWait(1) + p_mltcp.MeasWaitTime(1)
					cp.dZeroStress	= p_dFullStressZero
					cp.dZeroPxPhase	= p_dZeroStressPxPhase
					cp.dZeroPyPhase	= p_dZeroStressPyPhase
					cp.dZeroPzPhase	= p_dZeroStressPzPhase
					cp.dAttenuation	= p_dFullStressAttenuation
					cp.dDiameter	= p_dDiameter
					cp.dMeasGap		= p_mltcp.GapPreset(1)
					cp.dFullStress	= p_dFullStressADVal
					cp.dPxPhase		= p_dFullStressPxPhase
					cp.dPyPhase		= p_dFullStressPyPhase
					cp.dPzPhase		= p_dFullStressPzPhase
					cp.iZeroPos		= p_iZPosition
					cp.iHPPosControl	= p_clSpec.spi.iHPPosCont
					cp.dMeasureStress	= p_clSpec.spi.dMeasureStress
					cp.iCalcType		= p_clSpec.spi.iGelTimeMethod
					cp.dDiffInterval	= p_clSpec.spi.dDiffInterval
					cp.iStrsSensMode	= p_clSpec.spi.iStressSensMode			'応力検知モード0,1,2
					cp.iAmpMode			= p_clSpec.spi.iAmpMode					'Z振動モード 0,1
					cp.dMesGap			= 0.0									'測定GAP計算値 モニタ用
					cp.dXYWeightCoef	= p_dNConv								'XY重量換算

					'RCC時の基材厚み
					Select Case p_clSpec.spi.iShape
					Case eShape.Rcc
						cp.dRCCBaseThickness = Ctype(p_clSpec.spi.sThickness, Double) * p_clSpec.spi.dStack
					Case Else
						cp.dRCCBaseThickness = 0.0
					End Select

					p_cp = cp															'計算パラメータを退避
					If cp.iStrsSensMode=clSpec.eStrsSens.Circle
						m_iCorrRet = datCal.CorrectRealProcVector( cp )
					Else
						m_iCorrRet = datCal.CorrectRealProcVector( cp )
					End If
					p_dMesGap = cp.dMesGap		'測定GAP計算値 モニタ用
					p_dtFile.p_head.dRealGap = cp.dMesGap								'実測定GAPをヘッダへセット
					m_iStartIdx += p_dtFile.iHosei.Length - m_iStartIdx					'開始インデックスの更新
					m_iCorrectCount += 1
				'End If
			'--------------------------------------------------
			End If
#End If
				p_mltcp.ZeroSW(False)                                                   'ZERO調制御開始Bit OFF（測定中ゼロ調指令はOFFにする）

				p_dtFile.MesBlockCnt += 1                                               '受信ブロックカウンタ更新
				
				If p_strRTGSetName.Length = 0 Then
				'温度プロファイル中は自動終了しない
					'測定終了判定
					'応力制限判定 -----------------
					if p_dStressLimit <> 0
						If p_bMeasTimeEn
						'長時間モードは応力制限終了できない
						Else
							If p_dtFile.iHosei.Max * 100 / p_dFullStressADVal > p_dStressLimit
								sw6.Stop
								frmMain.StopTempInterrupt()								'温度プロファイル制御スレッドを停止
								Call WriteLog("測定終了 (応力制限)", eLOG_LEVEL.iINFO)
								p_iBGWOrderNext = BGW_ORDER.WAIT_LOCKOUT                           '終了データ処理へ
							End If
						End If
					End if
					'測定時間制限判定 -----------------
					If p_dMeasTimeLimit <> 0
						If p_dtFile.DataCount * MEAS_TIME_RESOLUTION / 1000.0 > p_dMeasTimeLimit
							'測定時間が制限超えた
							sw6.Stop
							frmMain.StopTempInterrupt()								'温度プロファイル制御スレッドを停止
							Call WriteLog("測定終了 (時間制限)", eLOG_LEVEL.iINFO)
							p_iBGWOrderNext = BGW_ORDER.WAIT_LOCKOUT                           '終了データ処理へ
						End If
					Else
					'0の場合、時間制限なし
					End If
				End If

				'処理時間が長くなった時の中止ボタン監視
				If p_iBGWOrder = BGW_ORDER.CANCEL_BUTTON
					p_iBGWOrderNext = BGW_ORDER.CANCEL_BUTTON							'測定中止
				ElseIf p_iBGWOrderNext = BGW_ORDER.NO_ORDER
					p_iBGWOrderNext = BGW_ORDER.MEAS_PROCESS							'継続
				End if
			Else
				p_iBGWOrderNext = BGW_ORDER.MEAS_PROCESS								'継続
			End If
		End If
'-----------------------------------------------------------------------------------------------------------
'測定終了 プローブロック外し待ち
	Case BGW_ORDER.WAIT_LOCKOUT
		p_mltcp.ZeroSW(False)                                                   'ZERO調制御開始Bit OFF
		p_mltcp.MeasSW(False)                                                   '測定開始指令Bit OFF
		If p_MeasureState =  eMeasStat.LOCKOUT_OK								'測定状態をロック解除済
			m_bPcLifeCheckDisBack = p_bPcLifeCheckDis								
			p_bPcLifeCheckDis = True												'PC生存チェック有効無効
			Sw1.Restart()
			p_iBGWOrderNext = BGW_ORDER.WAIT_LOCKOUT1                           '終了データ処理へ
		Else
			p_iBGWOrderNext = BGW_ORDER.WAIT_LOCKOUT							'継続
		End If

	Case BGW_ORDER.WAIT_LOCKOUT1
		If sw1.Elapsed.TotalSeconds>1
			p_iBGWOrderNext = BGW_ORDER.MEAS_PRO_DATA                           '終了データ処理へ
		Else
			p_iBGWOrderNext = BGW_ORDER.WAIT_LOCKOUT1							'継続
		End If


'測定終了データ処理(ゲルタイム計算)  -------------------------------------------------------------------------------------
	Case BGW_ORDER.MEAS_PRO_DATA
		Dim iRet As int32
		p_ElapsedMeasTime = sw6.Elapsed.TotalSeconds

		If p_bMeasTimeEn
		'規定時間制限許可のときはメモリ蓄積がないのでファイルからリード
			'読込ファイル名配列の生成
			Dim sFilePathAry() As String = Nothing
			Dim bCalc As Boolean = False
			Dim i As int32
			Dim sTemp As String
			Array.Resize(sFilePathAry, p_dtFile.MesFileNumber()+1)	
			For i = 0 To p_dtFile.MesFileNumber()
				sTemp = p_dtFile.p_head.sFileName.Substring(0,18)
				p_dtFile.p_head.sFileName = sTemp & "#" & i.ToString("D6") & "." & sDataExt
				p_dtFile.RealTimeMeasDataFilePath(sFilePathAry(i), , p_dtFile.p_head.sFileName)
			Next
			'ファイルリード
			iRet = p_dtFile.RealTimeMeasDataFileReadText(0, sFilePathAry, bCalc)		'ファイル読み込んで測定値領域へ転送
		End If


		If Not IsNothing(p_dtFile.iOxBuf)
			Select Case p_clSpec.spi.iStressSensMode
			Case eStrsSens.Point
				datCal.AveTempClear(p_mltcp.PeriodZ(1))			'クリア
			Case Else
				datCal.AveTempClear(p_mltcp.PeriodX(1))			'クリア
			End Select
			'中間値補正処理
			Dim cp As DataCal.stCalcPara = Nothing
			cp.iMeas		= eHoseiMeasMode.GelTime
			cp.iMode		= eHoseiCalcMode.Batch
			cp.cts			= m_cts
			cp.clDt			= p_dtFile
			Select Case p_clSpec.spi.iStressSensMode
			Case eStrsSens.Point
				cp.dAmplitude	= p_mltcp.AmplitudeZ / 1000.0
				cp.dPeriod		= p_mltcp.PeriodZ(1)
			Case Else
				cp.dAmplitude	= p_mltcp.AmplitudeX / 1000.0
				cp.dPeriod		= p_mltcp.PeriodX(1)
			End Select
			cp.iSampleStart	= 0

			'周期2秒でi=30499まで処理した場合、リアルタイム処理の方はi=30000までしか実行しない
			'バッチ処理では受信データ長で処理するためi=30499まで処理しようとする
			'データの整合性をとるためバッチ処理時に端数となるデータは切り捨てる
			'下記の処理で2秒周期で30500のデータは30000となる
			'同様に1.2秒や1.7秒などの中途半端な周期では2秒に切り上げて処理する
			'cp.iSampleNum	= (p_dtFile.iHosei.Length \ _
			'					(Math.Ceiling(p_mltcp.PeriodX(1)) * MEAS_BLOCK_DATA_NUM)) * _
			'					(Math.Ceiling(p_mltcp.PeriodX(1)) * MEAS_BLOCK_DATA_NUM)
			cp.iSampleNum	= p_dtFile.iHosei.Length
			cp.fltSet		= p_FltSetting
			cp.dXYWait		= p_mltcp.MesPreWait(1) + p_mltcp.MeasWaitTime(1)
			cp.dZeroStress	= p_dFullStressZero
			cp.dZeroPxPhase	= p_dZeroStressPxPhase
			cp.dZeroPyPhase	= p_dZeroStressPyPhase
			cp.dZeroPzPhase	= p_dZeroStressPzPhase
			cp.dAttenuation	= p_dFullStressAttenuation
			cp.dDiameter	= p_dDiameter
			cp.dMeasGap		= p_mltcp.GapPreset(1)
			cp.dFullStress	= p_dFullStressADVal
			cp.dPxPhase		= p_dFullStressPxPhase
			cp.dPyPhase		= p_dFullStressPyPhase
			cp.dPzPhase		= p_dFullStressPzPhase
			cp.iZeroPos		= p_iZPosition
			cp.iHPPosControl	= p_clSpec.spi.iHPPosCont
			cp.dMeasureStress	= p_clSpec.spi.dMeasureStress
			cp.iCalcType		= p_clSpec.spi.iGelTimeMethod
			cp.dDiffInterval	= p_clSpec.spi.dDiffInterval
			cp.iStrsSensMode	= p_clSpec.spi.iStressSensMode			'応力検知モード0,1,2
			cp.iAmpMode			= p_clSpec.spi.iAmpMode					'Z振動モード 0,1

			If cp.iStrsSensMode=clSpec.eStrsSens.Point AndAlso cp.iAmpMode=clSpec.eAmpMode.StsConst
			'点モードかつ応力一定の時
				'指定周期のFull応力値とFullPhase値を取得
				'FULLキャリブレーション結果読込
				Dim frmF As New frmCalibFull(0)
				Dim i As int32
				Dim j As int32
				iRet = frmF.sbFullStressCondRead(0, p_FClbX, p_FClbC, p_FClbP)
				frmF.Dispose()
				Array.Resize(cp.dCalibAmp,			0)
				Array.Resize(cp.dCalibFullStress,	0)
				Array.Resize(cp.dCalibFullPhase,	0)
				For i = 0 To p_FClbP.GetUpperBound(0)
					If p_FClbP(i).dPeriodZ = cp.dPeriod
						j = cp.dCalibAmp.Length+1	
						Array.Resize(cp.dCalibAmp,			j)
						Array.Resize(cp.dCalibFullStress,	j)
						Array.Resize(cp.dCalibFullPhase,	j)
						cp.dCalibAmp(j-1)			= p_FClbP(i).dAmplitudeZ
						cp.dCalibFullStress(j-1)	= p_FClbP(i).iFullVal
						cp.dCalibFullPhase(j-1)		= p_FClbP(i).dPPhaseZ
					End If
				Next
			Else
				cp.dCalibAmp			= Nothing
				cp.dCalibFullStress		= Nothing
				cp.dCalibFullPhase		= Nothing
			End If
			cp.dXYWeightCoef		= p_dNConv								'重量換算

			'RCC時の基材厚み
			Select Case p_clSpec.spi.iShape
			Case eShape.Rcc
				cp.dRCCBaseThickness = Ctype(p_clSpec.spi.sThickness, Double) * p_clSpec.spi.dStack
			Case Else
				cp.dRCCBaseThickness = 0.0
			End Select

			p_cp = cp															'計算パラメータを退避
			If cp.iStrsSensMode=clSpec.eStrsSens.Circle
				m_iCorrRet = datCal.CorrectRealProcVector( cp )
			Else
				m_iCorrRet = datCal.CorrectRealProcVector( cp )
			End If
			p_dMesGap = cp.dMesGap		'測定GAP計算値 モニタ用
			p_dtFile.p_head.dRealGap = cp.dMesGap								'実測定GAPをヘッダへセット

			''p_dtFile.DataTrim()															'余分に取得した受信ブロック分配列を削除

			'受信バッファチェック 最終１秒間の受信配列に全て０になっていないかチェックしなっていれば有値で埋める
			p_dtFile.FillLastZeroTail(0, p_dtFile.iOxBuf, MEAS_BLOCK_DATA_NUM)
			p_dtFile.FillLastZeroTail(1, p_dtFile.iOyBuf, MEAS_BLOCK_DATA_NUM)
			p_dtFile.FillLastZeroTail(2, p_dtFile.iOzBuf, MEAS_BLOCK_DATA_NUM)
			p_dtFile.FillLastZeroTail(3, p_dtFile.iPxBuf, MEAS_BLOCK_DATA_NUM)
			p_dtFile.FillLastZeroTail(4, p_dtFile.iPyBuf, MEAS_BLOCK_DATA_NUM)
			p_dtFile.FillLastZeroTail(5, p_dtFile.iPzBuf, MEAS_BLOCK_DATA_NUM)

			'== THR ゲルタイム計算処理 ==
            Select Case p_sCalcMethod
            Case lstCalcMethod(eCalcMethod.THR)
				iRet = p_dtFile.CalcGelTime(eCalcMethod.THR,	frmMain.p_iSelectGItem, p_dGelTime, p_dToqueThreshold, p_dFullStressADVal, p_dAprxRangeMin, p_dAprxRangeMax,,,,p_cts)
            Case lstCalcMethod(eCalcMethod.SLOPE)
				iRet = p_dtFile.CalcGelTime(eCalcMethod.SLOPE,	frmMain.p_iSelectGItem, p_dGelTime, p_dToqueThreshold, p_dFullStressADVal, p_dAprxRangeMin, p_dAprxRangeMax,,,,p_cts)
            Case lstCalcMethod(eCalcMethod.EXTRA)
            Case lstCalcMethod(eCalcMethod.PHASE)
				iRet = p_dtFile.CalcGelTime(eCalcMethod.PHASE,	frmMain.p_iSelectGItem, p_dGelTime, p_dToqueThreshold, 90.0 * Math.PI /180, p_dAprxRangeMin, p_dAprxRangeMax,,,,p_cts)
			Case lstCalcMethod(eCalcMethod.MELT)
				If p_dtFile.p_head.sTempProfile=""
					'定温MELT
						p_dtFile.p_head.sMelStartTemp	= p_clSpec.spi.dHPSetTemp			'初期温度
						p_dtFile.p_head.sMelEndTemp		= p_clSpec.spi.dHPSetTemp			'終了温度
						p_dtFile.p_head.sMelWaitTime	= datCal.iLatTimeForZPos / 1000 + 3 * p_clSpec.spi.dXPeriod		'待ち時間
						p_dtFile.p_head.sMelSpeed		= "0.0"								'上昇速度(℃/min)
						'最低粘度とその時の温度
						'iRet = p_dtFile.CalcGelTime(eCalcMethod.MELT,	frmMain.p_iSelectGItem, p_dGelTime, 0, 0, p_dAprxRangeMin, p_dAprxRangeMax, _
						'	p_clSpec.spi.dHPUpToXYActWait + p_clSpec.spi.dInputToHPUpWait + p_clSpec.spi.dXPeriod * 3 + 3, _
						'	p_dMeltVisco, p_dMeltTemp,p_cts)
						iRet = p_dtFile.CalcGelTime(eCalcMethod.MELT,	frmMain.p_iSelectGItem, p_dGelTime, 0, 0, p_dAprxRangeMin, p_dAprxRangeMax, _
							datCal.iLatTimeForZPos / 1000 + 3 * p_clSpec.spi.dXPeriod, _
							p_dMeltVisco, p_dMeltTemp,p_cts)
				Else
					'温度プロファイル読込
					If frmOptions.LoadRTGSET(p_dtFile.p_head.sTempProfile, p_sEquipName, p_rtgset)
					'温度プロファイル 読込OK
						p_dtFile.p_head.sMelStartTemp	= p_rtgset.mdbr(0).dHPTemp			'初期温度
						p_dtFile.p_head.sMelEndTemp		= p_rtgset.mdbr(1).dHPTemp			'終了温度
						p_dtFile.p_head.sMelWaitTime	= p_rtgset.dTiming(0)				'待ち時間
						Dim dRizeTime As Double			= p_rtgset.dTiming(1)				'上昇時間
						Dim dSpeed	As Double			= (p_rtgset.mdbr(1).dHPTemp - p_rtgset.mdbr(0).dHPTemp) / (dRizeTime/60.0)
						p_dtFile.p_head.sMelSpeed		= dSpeed.ToString("F1")					'上昇速度(℃/min)
						'最低粘度とその時の温度
						iRet = p_dtFile.CalcGelTime(eCalcMethod.MELT,	frmMain.p_iSelectGItem, p_dGelTime, 0, 0, p_dAprxRangeMin, p_dAprxRangeMax, p_rtgset.dTiming(0), p_dMeltVisco, p_dMeltTemp,p_cts)
					End If
				End If
#If 0
				'温度プロファイル読込
				If frmOptions.LoadRTGSET(p_dtFile.p_head.sTempProfile, p_rtgset)
				'温度プロファイル 読込OK
					p_dtFile.p_head.sMelStartTemp	= p_rtgset.mdbr(0).dHPTemp			'初期温度
					p_dtFile.p_head.sMelEndTemp		= p_rtgset.mdbr(1).dHPTemp			'終了温度
					p_dtFile.p_head.sMelWaitTime	= p_rtgset.dTiming(0)					'待ち時間
					Dim dRizeTime As Double			= p_rtgset.dTiming(1)					'上昇時間
					Dim dSpeed	As Double			= (p_rtgset.mdbr(1).dHPTemp - p_rtgset.mdbr(0).dHPTemp) / (dRizeTime/60.0)
					p_dtFile.p_head.sMelSpeed		= dSpeed.ToString("F1")					'上昇速度(℃/min)
					'最低粘度とその時の温度
					iRet = p_dtFile.CalcGelTime(eCalcMethod.MELT,	frmMain.p_iSelectGItem, p_dGelTime, 0, 0, p_dAprxRangeMin, p_dAprxRangeMax, p_rtgset.dTiming(0), p_dMeltVisco, p_dMeltTemp,p_cts)
				End If
#end if
			End Select
			Select Case iRet
			Case 0		'ゲルタイム計算OK
				Select Case p_sCalcMethod
				Case lstCalcMethod(eCalcMethod.THR), lstCalcMethod(eCalcMethod.SLOPE), lstCalcMethod(eCalcMethod.PHASE)
					p_dtfile.p_head.dGelTime = p_dGelTime										'データファイルヘッダ用ゲルタイム
					Select Case p_dGelTime
					Case p_dJudgeRangeMin To p_dJudgeRangeMax
						p_iGelTimeJudge = 1			'OK
						p_dtFile.p_head.sGelTimeJudge = "OK"
					Case Else
						If (p_dJudgeRangeMax - p_dJudgeRangeMin) <> 0
							p_iGelTimeJudge = 0			'NG
							p_dtFile.p_head.sGelTimeJudge = "NG"
						Else
							p_iGelTimeJudge = 2		'NA
							p_dtFile.p_head.sGelTimeJudge = "NA"
						End If
					End Select
				Case lstCalcMethod(eCalcMethod.MELT)
					p_iGelTimeJudge = 2
					p_dtFile.p_head.sGelTimeJudge	= "NA"
					p_dtfile.p_head.dGelTime		= p_dGelTime					'データファイルヘッダ用ゲルタイム
					p_dtfile.p_head.dGelTimeTemp	= p_dMeltTemp.ToString("F1")
					p_dtfile.p_head.dGelTimeVisco	= p_dMeltVisco.ToString("F1")
				End Select

			Case -2		'ゲルタイム計算不可
				p_iGelTimeJudge = 2
				p_dtFile.p_head.sGelTimeJudge = "NA"
			case else	'例外エラー発生
				p_iGelTimeJudge = 2
				p_dtFile.p_head.sGelTimeJudge = "NA"
			End Select
			'補正データ込みで上書き更新
			'実測定時間をセット
			p_dtFile.p_head.iMeasActTime =	p_dtFile.DataCount * MEAS_TIME_RESOLUTION / 1000.0
			p_dtFile.RealTimeMeasDataFileSaveText(If(p_bMeasTimeEn,  eMeasMode.NormalNoAcum, eMeasMode.Normal), False, m_cts)
		End If
		sw1.Restart()
		p_iBGWOrderNext = BGW_ORDER.MEAS_PRO_FINISH											'測定終了へ

'測定終了制御  -------------------------------------------------------------------------------------
	Case BGW_ORDER.MEAS_PRO_FINISH
		p_ElapsedMeasTime = -1
		p_iZeroAdjOk = -1                                                                   'ZERO調未実施へ
		p_mltcp.ZeroSW(False)                                                               'ZERO調制御開始Bit OFF
		p_mltcp.MeasSW(False)                                                               '測定開始指令Bit OFF

		If sw1.Elapsed.TotalSeconds > 1 
			sw1.Restart()
			p_iBGWOrderNext = BGW_ORDER.MEAS_PRO_FINISH_1									'測定終了制御_1へ
		Else
			Dim iLine As Int64
			iLine = p_dtFile.GetFileLineCount()												'ファイル行数を確認
			If iLine < p_dtFile.DataCount()
				p_iBGWOrderNext = BGW_ORDER.MEAS_PRO_FINISH									'継続
			Else
				sw1.Restart()
				p_iBGWOrderNext = BGW_ORDER.MEAS_PRO_FINISH_1								'測定終了制御_1へ
			End If
		End If

'測定終了制御_1  -------------------------------------------------------------------------------------
	Case BGW_ORDER.MEAS_PRO_FINISH_1
		'If sw1.Elapsed.TotalSeconds > 1
		''測定終了確認
		'	Dim style = MsgBoxStyle.SystemModal Or MsgBoxStyle.OkCancel Or MsgBoxStyle.DefaultButton2 Or MsgBoxStyle.Exclamation
		'	Dim title = "操作ガイド"
		'	if vbOK = MsgBox("測定が終了しました。" & vbCrLf & "センサーヘッドの固定を外してからOKボタンをクリックしてください。", style, title)
		'		'熱板原点移動
		'		p_iHotPlatePos = 0
		'		p_iBGWOrderNext = BGW_ORDER.HOT_PLATE_DRV_REP								'Z軸手動移動
		'	End If
		'	p_MeasureState =  eMeasStat.NOT_START
		'Else
			'p_strRTGSetName = ""															'温度プロファイル無効->リンク有効
		'End If
		p_MeasureState =  eMeasStat.NOT_START												'測定状態を未開始
		If sw1.Elapsed.TotalSeconds > 1
			sw1.Restart()
			p_iBGWOrderNext = BGW_ORDER.MEAS_PRO_FINISH_2										'測定終了制御_2へ
		Else
			p_iBGWOrderNext = BGW_ORDER.MEAS_PRO_FINISH_1										'継続
		End If

'測定終了制御_2  -------------------------------------------------------------------------------------
	Case BGW_ORDER.MEAS_PRO_FINISH_2
		If sw1.Elapsed.TotalSeconds > 2
			p_bPcLifeCheckDis = m_bPcLifeCheckDisBack						
			p_iBGWOrderNext = BGW_ORDER.NO_ORDER
			'DB自動登録処理
			If p_cts.DBAutoReg() = 1
				Dim sFileName()	As String = p_dtFile.GetDataFileName().Split("#")
				sFileName(0) = sFileName(0) & "#000000.dat"				'複数ファイル生成時の正規化
				If p_cts.DBSel = 0
					'SZKAutoRegistを起動
					m_frmM.AutoRejistExec(If(p_cts.SPCEn = 1, True, False), p_dtFile.p_head.sMeasureType, sFileName(0))
				Else 
					'SQLite版 1行追加
					If p_clDB.InsertResultsRec(p_dtFile.p_head) = 0
						Call WriteLog("DB登録成功しました。" & p_dtFile.p_head.sFileName , eLOG_LEVEL.iINFO)
					Else
						Call WriteLog("DB登録失敗しました。" & p_dtFile.p_head.sFileName , eLOG_LEVEL.iFATAL)
					End If
					'dSPCValuesのみ取得
					m_frmM.btnSPCClick_SQLite(1)
				End if

			End If 
		Else
			p_iBGWOrderNext = BGW_ORDER.MEAS_PRO_FINISH_2										'継続
		End If

	End Select

Catch ex As Exception
    Call FrmDebugLog(System.Reflection.MethodBase.GetCurrentMethod.Name & ex.Message)
    Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub


Public p_LTIntervalSecond	As Double		'動作インターバル時間(秒)
Public p_LTStopSecond		As Double		'動作停止時間
Public p_LTStopElaspSecond	As Int64		'通算動作停止時間		
Public p_LTDataPointNum		As Int32		'現在のデータ点数
Private	m_DataPointTime		As Int64		'データ点経過時間(秒)	
Private m_iZposUpdMode		As Int16 = 0	'Z位置をラッチするため計算処理を動作させる期間=1
''' <summary>
''' 測定制御 長期間欠モード
''' </summary>
''' <param name="sw1"></param>
''' <param name="sw2"></param>
''' <param name="sw3"></param>
''' <param name="sw6"></param>
Private Sub GelTimeMeasureProcLT(byRef sw1 As Stopwatch, byRef sw2 As Stopwatch, byRef sw3 As Stopwatch, byRef sw6 As Stopwatch)
Dim iRet As int32 = 0
Try
	Select Case p_iBGWOrder
	Case BGW_ORDER.LT_START_BUTTON
		If p_iConnectState = CONNECT_ST.CONNECTING Then                                     'リンク中のみ
			If p_mltcp.isStatMeasuring()
				p_mltcp.MeasSW(False)													    '測定開始指令Bit OFF
				p_MeasureState = eMeasStat.NOT_START                                        '未測定中
			Else
				p_mltcp.MeasSW(True)											            '測定開始指令Bit ON
				p_MeasureState = eMeasStat.PREPAREING_1                                     '測定準備中
				p_iBGWOrderNext = BGW_ORDER.LT_START_BUTTON_CNF                             '応答確認へ
				sw1.Restart                                                                 '応答Bit タイマースタート
			End If
			p_CaliblationState.iState = eCalibState.NOT_START								'較正用ステータス->未実行(ZERO調後のクリア)
		End If

	Case BGW_ORDER.LT_START_BUTTON_CNF
		'測定SW応答確認
		If p_mltcp.isRespMeasStart() AndAlso p_mltcp.isStatMeasuringTempo()					'応答bit[1108.00]、測定中ステータス[1109.02]

			p_LTDataPointNum		= 0
			'データファイル名の決定
			p_dtFile.MesFileNumber	= 0														'ファイル連番クリア
			'長期間欠ファイル名
			p_dtFile.SetDataFileName = p_dtFile.sMeasindCode(eMeasMode.NormalLT) & "-" & Format(Now, "yyyyMMddHHmmss")
			MeasureHeaderInfoSet(eMeasMode.NormalLT)										'測定開始時のファイルヘッダ情報のセット
			'長期間欠モード測定データ配列確保
			Dim iPt As Int32 = p_LTDataPointNum
			p_dtFile.LTDataArrayResize(0, 1)
		'一次元Double配列の交尾X周期の平均値を算出し長期間欠モード測定データ配列へセット
			p_dtFile.iLT_MSec(iPt)		= 0
			p_dtFile.iLT_TempBuf(iPt)	= 0			'HP温度
			p_dtFile.iLT_TempBuf2(iPt)	= 0			'PB温度
			p_dtFile.iLT_MvBuf(iPt)		= 0			'HP MV
			p_dtFile.iLT_MvBuf2(iPt)	= 0			'PB MV
			p_dtFile.iLT_Hosei(iPt)		= 0			'応力
			p_dtFile.dLT_Hosei100(iPt)	= 0			'応力%
			p_dtFile.dLT_XPosPhase(iPt)	= 0			'x位置位相差
			p_dtFile.dLT_Viscosity(iPt)	= 0			'粘度
			p_dtFile.dLT_SplStrain(iPt)	= 0			'サンプル歪
			p_dtFile.dLT_ShearMod(iPt)	= 0			'せん断弾性率
			p_dtFile.dLT_ShearVelo(iPt)	= 0			'せん断速度
			p_dtFile.dLT_XStrgMod(iPt)	= 0			'貯蔵弾性率
			p_dtFile.dLT_XElasMod(iPt)	= 0			'損失弾性率
			p_dtFile.dLT_XLossTan(iPt)	= 0			'損失正接

			'長期間欠モード測定データをファイルへ一行追加(初回はヘッダ出力)
			p_dtFile.RealTimeMeasDataFileSaveText(eMeasMode.NormalLT, True, m_cts,,,,,,p_LTDataPointNum)			'測定値保存 追加モード
			p_LTDataPointNum	+= 1

			p_MeasureState = eMeasStat.PREPAREING_1
			p_iBGWOrderNext = BGW_ORDER.LT_MEAS_PREPARE_1	                                '測定準備1へ
			sw3.Restart()																	'測定制限時間SW開始
		Else
			If sw1.Elapsed.TotalSeconds > cResponseTimeOut Then
				'応答BIT未確認タイムオーバー	5秒
				p_mltcp.MeasSW(False)													    '測定開始指令Bit OFF
				Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & ":Confirm Bit Response Time Over (START_BUTTON_CNF)")
				sw1.Restart()
				p_iBGWOrderNext = BGW_ORDER.LT_MEAS_PRO_FINISH							'測定終了へ
			Else
				p_iBGWOrderNext = BGW_ORDER.LT_START_BUTTON_CNF                           '応答確認継続
			End If
		End If
		'測定キャンセル確認
		If p_MeasureState < eMeasStat.RESERV
			p_mltcp.MeasSW(False)															'測定開始指令Bit OFF
			p_iBGWOrderNext = BGW_ORDER.NO_ORDER
		End If
		'本体エラー発生時
		If p_bDevError
			p_iBGWOrderNext = BGW_ORDER.LT_MEAS_ERROR_CANCEL
		End If

	Case BGW_ORDER.LT_MEAS_PREPARE_1
		If (p_mltcp.isRespMeasStart() = False) Or (p_mltcp.isStatMeasuringTempo()=False)	'応答bit[1058.00]、測定中ステータス[1059.02]
			'本体側が測定終了した
			p_mltcp.MeasSW(False)															'測定開始指令Bit OFF
			sw1.Restart()
			p_iBGWOrderNext = BGW_ORDER.LT_MEAS_PRO_FINISH								'測定終了へ
		Elseif p_mltcp.isStatMeasuring()
		'測定中(クリア後)がON-> ゲルタイム ０秒 ここから測定データ取得開始
			m_iCorrRet = 0
			m_iStartIdx = 0
			m_bRealCorrect = False
			m_iCorrectCount = 0
			dWaitAmpChange	= 3															'振幅変更時の最低安定待ち時間(秒)
			m_iPeriodSample = p_mltcp.PeriodX(1) * 1000 / MEAS_TIME_RESOLUTION			'1周期分のサンプル数計算
			m_iLTSampleNum	= m_iPeriodSample * 10										'10周期分のサンプル数計算
			p_mltcp.MesNo	= 0
			sw6.Restart()
			p_ElapsedMeasTime = sw6.Elapsed.TotalSeconds
			p_MeasureState = eMeasStat.DATA_TRANS

			p_mltcp.AmplitudeX	= 0														'X軸振幅
			p_mltcp.AmplitudeY	= 0														'Y軸振幅
			p_mltcp.AmplitudeZ  = 0														'Z軸振幅
			p_LTIntervalSecond	= p_cts.LTMeasureInterval * 60							'動作インターバル時間(秒)
			p_LTStopSecond		= p_LTIntervalSecond									'動作停止時間
			p_iDataCount		= 0														'データカウントクリア
			m_DataPointTime		= 0														'データ点経過時間(秒)
			datCal.iLatSign		= 0
			datCal.iLatPrePzBuf	= 0

			p_iBGWOrderNext = BGW_ORDER.LT_MEAS_PROCESS_ZPOS                            '位置更新用ラッチ制御へ >>>>
		Else
			p_iBGWOrderNext = BGW_ORDER.LT_MEAS_PREPARE_1								'<<継続>>
		End If
		'本体エラー発生時
		If p_bDevError
			p_iBGWOrderNext = BGW_ORDER.LT_MEAS_ERROR_CANCEL
		End If

'測定手動終了中止制御  -------------------------------------------------------------------------------------
	Case BGW_ORDER.LT_CANCEL_BUTTON
		If p_mltcp.isStatMeasuring()
		'測定中ステータスON
			p_iBGWOrderNext = BGW_ORDER.LT_WAIT_LOCKOUT											'終了データ処理へ
		Else
			p_ElapsedMeasTime = -1
			p_iZeroAdjOk = -1                                                                   'ZERO調未実施へ
			p_mltcp.ZeroSW(False)                                                               'ZERO調制御開始Bit OFF
			p_mltcp.MeasSW(False)                                                               '測定開始指令Bit OFF
			p_MeasureState =  eMeasStat.NOT_START
			p_iBGWOrderNext = BGW_ORDER.LT_MEAS_BEFORE_CANCEL
		End If

	Case BGW_ORDER.LT_MEAS_BEFORE_CANCEL
		Thread.Sleep(500)
		p_iBGWOrderNext = BGW_ORDER.NO_ORDER

	Case BGW_ORDER.LT_MEAS_ERROR_CANCEL
		Thread.Sleep(500)
		p_iBGWOrderNext = BGW_ORDER.LT_WAIT_LOCKOUT


'測定間欠動作中止中  ----------------------------------------------------------------------------------------
	Case BGW_ORDER.LT_MEAS_PROCESS_WAIT
		'測定終了監視
		If (p_mltcp.isRespMeasStart() = False) Or (p_mltcp.isStatMeasuringTempo() = False)  '応答bit[1108.00]、測定中ステータス[1109.02]
			'本体側が測定終了した
			p_iBGWOrderNext = BGW_ORDER.LT_WAIT_LOCKOUT                                  '測定したところまでで測定終了しデータ処理
		ElseIf p_mltcp.isItLock()
			'扉オープンされた
			p_iBGWOrderNext = BGW_ORDER.LT_WAIT_LOCKOUT                                  '測定したところまでで測定終了しデータ処理
		Else
			p_ElapsedMeasTime	= sw6.Elapsed.TotalSeconds								'総合経過時間
			'休止時間のチェック
			p_LTStopElaspSecond = p_LTStopSecond * p_LTDataPointNum						''通算動作停止時間更新
			If p_ElapsedMeasTime >= p_LTStopElaspSecond
				m_DataPointTime		= p_ElapsedMeasTime									'データ取り始めの相対時間(秒)をセット
				'設定振幅・周期をセットし動作開始
				Select Case p_clSpec.spi.iStressSensMode
				Case eStrsSens.Line		'線
					p_mltcp.AmplitudeX	= p_clSpec.spi.dXAmplitude * 1000.0
					p_mltcp.AmplitudeY	= 0
					p_mltcp.Amplitudez	= 0
				Case eStrsSens.Circle	'円
					p_mltcp.AmplitudeX	= p_clSpec.spi.dXAmplitude * 1000.0
					p_mltcp.AmplitudeY	= p_clSpec.spi.dYAmplitude * 1000.0
					p_mltcp.Amplitudez	= 0
				Case eStrsSens.Point	'点
					p_mltcp.AmplitudeX	= 0
					p_mltcp.AmplitudeY	= 0
					p_mltcp.Amplitudez	= p_clSpec.spi.dZAmplitude * 1000.0
				End Select
				p_mltcp.PeriodX(1)	= p_clSpec.spi.dXPeriod
				p_mltcp.PeriodY(1)	= p_clSpec.spi.dYPeriod
				p_mltcp.PeriodZ(1)	= p_clSpec.spi.dZPeriod
				p_DispUpdate	= 0
				m_iCorrectCount = 0															'リアルタイム計算カウンタ(受信ブロック処理) クリア
				p_iDataCount	= 0															'データサンプル処理数 クリア
				p_dtFile.ReadBufferClear(False)												'計算ワーク配列 初期化(ヘッダ以外)
				sw2.Restart()																'安定待ちタイマ開始
				p_iBGWOrderNext = BGW_ORDER.LT_MEAS_STAB_WAIT								'動作開始時の本体安定待ちへ>>>>
			Else
				'測定時間制限判定 -----------------
				If p_dMeasTimeLimit <> 0
					If m_DataPointTime >= p_dMeasTimeLimit
						'測定時間が制限超えた
						sw6.Stop
						Call WriteLog("測定終了 (時間制限)", eLOG_LEVEL.iINFO)
						p_iBGWOrderNext = BGW_ORDER.LT_WAIT_LOCKOUT                           '測定終了 プローブロック外し待ち>>>>>>>
					End If
				Else
				'0の場合、時間制限なし
				End If
				If p_iBGWOrder = BGW_ORDER.LT_CANCEL_BUTTON
					p_iBGWOrderNext = BGW_ORDER.LT_CANCEL_BUTTON							'測定手動終了中止制御 >>>>
				ElseIf p_iBGWOrderNext = BGW_ORDER.NO_ORDER
					p_iBGWOrderNext = BGW_ORDER.LT_MEAS_PROCESS_WAIT						'<<継続>>
				End if
			End If
		End If

'動作開始時の本体安定待ち  ----------------------------------------------------------------------------------
	Case BGW_ORDER.LT_MEAS_STAB_WAIT
		p_ElapsedMeasTime	= sw6.Elapsed.TotalSeconds									'総合経過時間
		If sw2.Elapsed.TotalSeconds >= dWaitAmpChange
			If p_mltcp.bMesDataEnable Andalso p_mltcp.isStatXYAction
				If Not p_mltcp.isFreqUpdate()
				'XYZ振動設定変更中でないこと
					p_iBGWOrderNext = BGW_ORDER.LT_MEAS_PROCESS								'測定データ取得制御>>>>
				Else
					p_iBGWOrderNext = BGW_ORDER.LT_MEAS_STAB_WAIT							'<<継続>>
				End If
			Else
				p_iBGWOrderNext = BGW_ORDER.LT_MEAS_STAB_WAIT							'<<継続>>
			End If
		Else
			p_iBGWOrderNext = BGW_ORDER.LT_MEAS_STAB_WAIT							'<<継続>>
		End If
		'本体エラー発生時
		If p_bDevError
			p_iBGWOrderNext = BGW_ORDER.LT_MEAS_ERROR_CANCEL
		End If

'Z位置更新用ラッチ制御  -------------------------------------------------------------------------------------
	Case BGW_ORDER.LT_MEAS_PROCESS_ZPOS
		p_ElapsedMeasTime	= sw6.Elapsed.TotalSeconds									'総合経過時間
		If p_mltcp.bMesDataEnable		'測定データ有効
		'補正処理(リアルタイム) ----------------------
		'1周期分のデータ取得まで待つ
			if m_iCorrectCount = 0
			'初回は0インデックスから
				m_iStartIdx = 0
				Select Case p_clSpec.spi.iStressSensMode
				Case eStrsSens.Point
					datCal.AveTempClear(p_mltcp.PeriodZ(1), 0)			'クリア
				Case Else
					datCal.AveTempClear(p_mltcp.PeriodX(1), 0)			'クリア
				End Select
			End If
			'蓄積配列拡張
			Dim idx As int32 = If(IsNothing(p_dtFile.iOxBuf), 0, p_dtFile.iOxBuf.Length)
			p_dtFile.ResizeMeasArray(MEAS_BLOCK_DATA_NUM + idx)
			'蓄積配列へ格納
			For i=0 To MEAS_BLOCK_DATA_NUM-1
				p_dtFile.RealTimeAccumulationArrayCopy(i, idx)
			Next
			'リアルタイム中間値補正
			Dim cp As DataCal.stCalcPara = Nothing
			cp.iMeas		= eHoseiMeasMode.GelTime
			cp.iMode		= eHoseiCalcMode.LTRealTime
			cp.cts			= m_cts
			cp.clDt			= p_dtFile
			Select Case p_clSpec.spi.iStressSensMode
			Case eStrsSens.Point
				cp.dAmplitude	= p_mltcp.AmplitudeZ / 1000.0
				cp.dPeriod		= p_mltcp.PeriodZ(1)
			Case Else
				cp.dAmplitude	= p_mltcp.AmplitudeX / 1000.0
				cp.dPeriod		= p_mltcp.PeriodX(1)
			End Select
			cp.iSampleStart	= m_iStartIdx
			cp.iSampleNum	= p_dtFile.iHosei.Length - m_iStartIdx
			cp.fltSet		= p_FltSetting
			cp.dXYWait		= 0								'間欠測定なので動作待ちはゼロ
			cp.dZeroStress	= p_dFullStressZero
			cp.dAttenuation	= p_dFullStressAttenuation
			cp.dDiameter	= p_dDiameter
			cp.dMeasGap		= p_mltcp.GapPreset(1)
			cp.dFullStress	= p_dFullStressADVal
			cp.dPxPhase		= p_dFullStressPxPhase
			cp.dPyPhase		= p_dFullStressPyPhase
			cp.dPzPhase		= p_dFullStressPzPhase
			cp.iZeroPos		= p_iZPosition
			cp.iHPPosControl	= p_clSpec.spi.iHPPosCont
			cp.dMeasureStress	= p_clSpec.spi.dMeasureStress
			cp.iCalcType		= p_clSpec.spi.iGelTimeMethod
			cp.dDiffInterval	= p_clSpec.spi.dDiffInterval
			cp.iStrsSensMode	= p_clSpec.spi.iStressSensMode					'応力検知モード0,1,2
			cp.iAmpMode			= p_clSpec.spi.iAmpMode							'Z振動モード 0,1
			cp.dRCCBaseThickness = 0.0
			cp.dXYWeightCoef	= p_dNConv										'重量換算
			cp.dHPPosition		= p_mltcp.HeatBPos								'熱板位置(um)
			p_cp = cp															'計算パラメータを退避
			Select Case cp.iStrsSensMode
			Case clSpec.eStrsSens.Circle	'円モード
				m_iCorrRet = datCal.CorrectRealProcVector( cp )
			Case Else						'直線モード
				m_iCorrRet = datCal.CorrectRealProcVector( cp )
			End Select
			p_dMesGap = cp.dMesGap		'測定GAP計算値 モニタ用
			p_dtFile.p_head.dRealGap = cp.dMesGap								'実測定GAPをヘッダへセット
			m_iStartIdx += p_dtFile.iHosei.Length - m_iStartIdx					'開始インデックスの更新
			m_iCorrectCount += 1

			If datCal.iLatSign = 1	Or p_mltcp.bComDebug		
			'Z位置のラッチ完了
				'設定振幅・周期をセットし動作停止
				p_mltcp.AmplitudeX	= 0												'X軸振幅
				p_mltcp.AmplitudeY	= 0												'Y軸振幅
				p_mltcp.AmplitudeZ  = 0												'Z軸振幅
				p_iBGWOrderNext = BGW_ORDER.LT_MEAS_PROCESS_WAIT				'測定間欠動作中止中へ>>>>
				p_dtFile.ResizeMeasArray(0)										'蓄積配列クリア
				p_dtFile.DataCount	= 0											'最初に戻す
				m_iCorrectCount		= 0											'最初に戻す
				m_iStartIdx			= 0
			Else
				p_iBGWOrderNext = BGW_ORDER.LT_MEAS_PROCESS_ZPOS						'<<<<継続>>>>
			End If
		Else
			p_iBGWOrderNext = BGW_ORDER.LT_MEAS_PROCESS_ZPOS						'<<<<継続>>>>
		End if
		'本体エラー発生時
		If p_bDevError
			p_iBGWOrderNext = BGW_ORDER.LT_MEAS_ERROR_CANCEL
		End If

'測定データ取得制御( 10周期分動作ON )  -------------------------------------------------------------------------------------
	Case BGW_ORDER.LT_MEAS_PROCESS
		p_ElapsedMeasTime	= sw6.Elapsed.TotalSeconds									'総合経過時間
		If p_mltcp.bMesDataEnable Andalso p_mltcp.isStatXYAction Andalso (Not p_mltcp.isFreqUpdate) Then
		'測定データ有効
		'補正処理(リアルタイム) ----------------------
		'1周期分のデータ取得まで待つ
			if m_iCorrectCount = 0
			'初回は0インデックスから
				m_iStartIdx = 0
				Select Case p_clSpec.spi.iStressSensMode
				Case eStrsSens.Point
					datCal.AveTempClear(p_mltcp.PeriodZ(1), 0)			'クリア
				Case Else
					datCal.AveTempClear(p_mltcp.PeriodX(1), 0)			'クリア
				End Select
			End If
			'蓄積配列拡張
			Dim idx As int32 = If(IsNothing(p_dtFile.iOxBuf), 0, p_dtFile.iOxBuf.Length)
			p_dtFile.ResizeMeasArray(MEAS_BLOCK_DATA_NUM + idx)
			'蓄積配列へ格納
			For i=0 To MEAS_BLOCK_DATA_NUM-1
				p_dtFile.RealTimeAccumulationArrayCopy(i, idx)
			Next
			'リアルタイム中間値補正
			'Z決定位置をセット
			If p_mltcp.ZConfirmPos<>0
				p_dtFile.p_head.dZAfterPos = Ctype(p_mltcp.ZConfirmPos, Double) / 10.0
			End If
			Dim cp As DataCal.stCalcPara = Nothing
			cp.iMeas		= eHoseiMeasMode.GelTime
			cp.iMode		= eHoseiCalcMode.LTRealTime
			cp.cts			= m_cts
			cp.clDt			= p_dtFile
			Select Case p_clSpec.spi.iStressSensMode
			Case eStrsSens.Point
				cp.dAmplitude	= p_mltcp.AmplitudeZ / 1000.0
				cp.dPeriod		= p_mltcp.PeriodZ(1)
			Case Else
				cp.dAmplitude	= p_mltcp.AmplitudeX / 1000.0
				cp.dPeriod		= p_mltcp.PeriodX(1)
			End Select
			cp.iSampleStart	= m_iStartIdx
			cp.iSampleNum	= p_dtFile.iHosei.Length - m_iStartIdx
			cp.fltSet		= p_FltSetting
			cp.dXYWait		= 0								'間欠測定なので動作待ちはゼロ
			cp.dZeroStress	= p_dFullStressZero
			cp.dAttenuation	= p_dFullStressAttenuation
			cp.dDiameter	= p_dDiameter
			cp.dMeasGap		= p_mltcp.GapPreset(1)
			cp.dFullStress	= p_dFullStressADVal
			cp.dPxPhase		= p_dFullStressPxPhase
			cp.dPyPhase		= p_dFullStressPyPhase
			cp.dPzPhase		= p_dFullStressPzPhase
			cp.iZeroPos		= p_iZPosition
			cp.iHPPosControl	= p_clSpec.spi.iHPPosCont
			cp.dMeasureStress	= p_clSpec.spi.dMeasureStress
			cp.iCalcType		= p_clSpec.spi.iGelTimeMethod
			cp.dDiffInterval	= p_clSpec.spi.dDiffInterval
			cp.iStrsSensMode	= p_clSpec.spi.iStressSensMode					'応力検知モード0,1,2
			cp.iAmpMode			= p_clSpec.spi.iAmpMode							'Z振動モード 0,1
			cp.dRCCBaseThickness = 0.0
			cp.dXYWeightCoef	= p_dNConv										'重量換算
			cp.dHPPosition		= p_mltcp.HeatBPos								'熱板位置(um)
			p_cp = cp															'計算パラメータを退避
			Select Case cp.iStrsSensMode
			Case clSpec.eStrsSens.Circle	'円モード
				m_iCorrRet = datCal.CorrectRealProcVector( cp )
			Case Else						'直線モード
				m_iCorrRet = datCal.CorrectRealProcVector( cp )
			End Select
			p_dMesGap = cp.dMesGap		'測定GAP計算値 モニタ用
			p_dtFile.p_head.dRealGap = cp.dMesGap								'実測定GAPをヘッダへセット
			m_iStartIdx += p_dtFile.iHosei.Length - m_iStartIdx					'開始インデックスの更新
			m_iCorrectCount += 1
		'--------------------------------------------------
			p_dtFile.MesBlockCnt += 1                                           '受信ブロックカウンタ更新
			p_iDataCount += MEAS_BLOCK_DATA_NUM									'累積サンプル数更新
			'累積サンプル数が規定ピーク数を含んだサンプル数を超えたか確認 '1周期余分にとる
			If p_iDataCount >= m_iLTSampleNum '+ (MEAS_BLOCK_DATA_NUM * cp.dPeriod)	
				'設定振幅・周期をセットし動作停止
				p_mltcp.AmplitudeX	= 0												'X軸振幅
				p_mltcp.AmplitudeY	= 0												'Y軸振幅
				p_mltcp.AmplitudeZ  = 0												'Z軸振幅
				p_iBGWOrderNext = BGW_ORDER.LT_DATACAL							'計算・平均化処理へ>>>>
			Else
				p_iBGWOrderNext = BGW_ORDER.LT_MEAS_PROCESS						'<<<<継続>>>>
			End If
		Else
			p_iBGWOrderNext = BGW_ORDER.LT_MEAS_PROCESS						'<<<<継続>>>>
		End if
		'本体エラー発生時
		If p_bDevError
			p_iBGWOrderNext = BGW_ORDER.LT_MEAS_ERROR_CANCEL
		End If

'計算・平均化処理 ------------------------------------------------------------------------------------------------
	Case BGW_ORDER.LT_DATACAL 
		p_ElapsedMeasTime	= sw6.Elapsed.TotalSeconds									'総合経過時間
		Select Case p_clSpec.spi.iStressSensMode
		Case eStrsSens.Point
			datCal.AveTempClear(p_mltcp.PeriodZ(1))			'クリア
		Case Else
			datCal.AveTempClear(p_mltcp.PeriodX(1))			'クリア
		End Select
		'中間値補正処理
		Dim cp As DataCal.stCalcPara = Nothing
		cp.iMeas		= eHoseiMeasMode.GelTime
		cp.iMode		= eHoseiCalcMode.LTBatch
		cp.cts			= m_cts
		cp.clDt			= p_dtFile
		Select Case p_clSpec.spi.iStressSensMode
		Case eStrsSens.Point
			cp.dAmplitude	= p_mltcp.AmplitudeZ / 1000.0
			cp.dPeriod		= p_mltcp.PeriodZ(1)
		Case Else
			cp.dAmplitude	= p_mltcp.AmplitudeX / 1000.0
			cp.dPeriod		= p_mltcp.PeriodX(1)
		End Select
		cp.iSampleStart	= 0

		'周期2秒でi=30499まで処理した場合、リアルタイム処理の方はi=30000までしか実行しない
		'バッチ処理では受信データ長で処理するためi=30499まで処理しようとする
		'データの整合性をとるためバッチ処理時に端数となるデータは切り捨てる
		'下記の処理で2秒周期で30500のデータは30000となる
		'同様に1.2秒や1.7秒などの中途半端な周期では2秒に切り上げて処理する
		'cp.iSampleNum	= (p_dtFile.iHosei.Length \ _
		'					(Math.Ceiling(p_mltcp.PeriodX(1)) * MEAS_BLOCK_DATA_NUM)) * _
		'					(Math.Ceiling(p_mltcp.PeriodX(1)) * MEAS_BLOCK_DATA_NUM)
		cp.iSampleNum	= p_dtFile.iHosei.Length
		cp.fltSet		= p_FltSetting
		cp.dXYWait		= 0
		cp.dZeroStress	= p_dFullStressZero
		cp.dZeroPxPhase	= p_dZeroStressPxPhase
		cp.dZeroPyPhase	= p_dZeroStressPyPhase
		cp.dZeroPzPhase	= p_dZeroStressPzPhase
		cp.dAttenuation	= p_dFullStressAttenuation
		cp.dDiameter	= p_dDiameter
		cp.dMeasGap		= p_mltcp.GapPreset(1)
		cp.dFullStress	= p_dFullStressADVal
		cp.dPxPhase		= p_dFullStressPxPhase
		cp.dPyPhase		= p_dFullStressPyPhase
		cp.dPzPhase		= p_dFullStressPzPhase
		cp.iZeroPos		= p_iZPosition
		cp.iHPPosControl	= p_clSpec.spi.iHPPosCont
		cp.dMeasureStress	= p_clSpec.spi.dMeasureStress
		cp.iCalcType		= p_clSpec.spi.iGelTimeMethod
		cp.dDiffInterval	= p_clSpec.spi.dDiffInterval
		cp.iStrsSensMode	= p_clSpec.spi.iStressSensMode			'応力検知モード0,1,2
		cp.iAmpMode			= p_clSpec.spi.iAmpMode					'Z振動モード 0,1

		If cp.iStrsSensMode=clSpec.eStrsSens.Point AndAlso cp.iAmpMode=clSpec.eAmpMode.StsConst
		'点モードかつ応力一定の時
			'指定周期のFull応力値とFullPhase値を取得
			'FULLキャリブレーション結果読込
			Dim frmF As New frmCalibFull(0)
			Dim i As int32
			iRet = frmF.sbFullStressCondRead(0, p_FClbX, p_FClbC, p_FClbP)
			frmF.Dispose()
			Array.Resize(cp.dCalibAmp,			0)
			Array.Resize(cp.dCalibFullStress,	0)
			Array.Resize(cp.dCalibFullPhase,	0)
			For i = 0 To p_FClbP.GetUpperBound(0)
				If p_FClbP(i).dPeriodZ = cp.dPeriod
					Dim idx As int32
					idx = cp.dCalibAmp.Length+1	
					Array.Resize(cp.dCalibAmp,			idx)
					Array.Resize(cp.dCalibFullStress,	idx)
					Array.Resize(cp.dCalibFullPhase,	idx)
					cp.dCalibAmp(idx-1)				= p_FClbP(i).dAmplitudeZ
					cp.dCalibFullStress(idx-1)		= p_FClbP(i).iFullVal
					cp.dCalibFullPhase(idx-1)		= p_FClbP(i).dPPhaseZ
				End If
			Next
		Else
			cp.dCalibAmp			= Nothing
			cp.dCalibFullStress		= Nothing
			cp.dCalibFullPhase		= Nothing
		End If
		cp.dXYWeightCoef		= p_dNConv								'重量換算
		cp.dHPPosition			= p_mltcp.HeatBPos						'熱板位置(um)

		'RCC時の基材厚み
		Select Case p_clSpec.spi.iShape
		Case eShape.Rcc
			cp.dRCCBaseThickness = Ctype(p_clSpec.spi.sThickness, Double) * p_clSpec.spi.dStack
		Case Else
			cp.dRCCBaseThickness = 0.0
		End Select

		p_cp = cp															'計算パラメータを退避
		Select Case cp.iStrsSensMode
		Case clSpec.eStrsSens.Circle	'円モード
			m_iCorrRet = datCal.CorrectRealProcVector( cp )
		Case Else						'直線モード
			m_iCorrRet = datCal.CorrectRealProcVector( cp )
		End Select
		p_dMesGap = cp.dMesGap		'測定GAP計算値 モニタ用
		p_dtFile.p_head.dRealGap = cp.dMesGap								'実測定GAPをヘッダへセット

		'長期間欠モード測定データ配列確保
		Dim iPt As Int32 = p_LTDataPointNum
		Dim j As int32 = p_dtFile.iLT_MSec.Length+1
		p_dtFile.LTDataArrayResize(0, j)

		'一次元Double配列の交尾X周期の平均値を算出し長期間欠モード測定データ配列へセット
		p_dtFile.iLT_MSec(iPt)	= m_DataPointTime
		sbCalcInbtegerAverage2Period(p_dtFile.iTempBuf,		cp.dPeriod,		p_dtFile.iLT_TempBuf(iPt),	p_cts.LTAdoptRate)			'HP温度
		sbCalcInbtegerAverage2Period(p_dtFile.iTempBuf2,	cp.dPeriod,		p_dtFile.iLT_TempBuf2(iPt),	p_cts.LTAdoptRate)			'PB温度
		sbCalcInbtegerAverage2Period(p_dtFile.iMvBuf,		cp.dPeriod,		p_dtFile.iLT_MvBuf(iPt),	p_cts.LTAdoptRate)			'HP MV
		sbCalcInbtegerAverage2Period(p_dtFile.iMvBuf2,		cp.dPeriod,		p_dtFile.iLT_MvBuf2(iPt),	p_cts.LTAdoptRate)			'PB MV
		sbCalcDoubleAverage2Period(	p_dtFile.iHosei,		cp.dPeriod,		p_dtFile.iLT_Hosei(iPt),	p_cts.LTAdoptRate)			'応力
		sbCalcDoubleAverage2Period(	p_dtFile.dHosei100,		cp.dPeriod,		p_dtFile.dLT_Hosei100(iPt),	p_cts.LTAdoptRate)			'応力%
		sbCalcDoubleAverage2Period( p_dtFile.dXPosPhase,	cp.dPeriod,		p_dtFile.dLT_XPosPhase(iPt),p_cts.LTAdoptRate)			'x位置位相差
		sbCalcDoubleAverage2Period(	p_dtFile.dViscosity,	cp.dPeriod,		p_dtFile.dLT_Viscosity(iPt),p_cts.LTAdoptRate)			'粘度
		sbCalcDoubleAverage2Period( p_dtFile.dSplStrain,	cp.dPeriod,		p_dtFile.dLT_SplStrain(iPt),p_cts.LTAdoptRate)			'サンプル歪
		sbCalcDoubleAverage2Period( p_dtFile.dShearMod,		cp.dPeriod,		p_dtFile.dLT_ShearMod(iPt),	p_cts.LTAdoptRate)			'せん断弾性率
		sbCalcDoubleAverage2Period( p_dtFile.dShearVelo,	cp.dPeriod,		p_dtFile.dLT_ShearVelo(iPt),p_cts.LTAdoptRate)			'せん断速度
		sbCalcDoubleAverage2Period( p_dtFile.dXStrgMod,		cp.dPeriod,		p_dtFile.dLT_XStrgMod(iPt),	p_cts.LTAdoptRate)			'貯蔵弾性率
		sbCalcDoubleAverage2Period( p_dtFile.dXElasMod,		cp.dPeriod,		p_dtFile.dLT_XElasMod(iPt),	p_cts.LTAdoptRate)			'損失弾性率
		sbCalcDoubleAverage2Period( p_dtFile.dXLossTan,		cp.dPeriod,		p_dtFile.dLT_XLossTan(iPt),	p_cts.LTAdoptRate)			'損失正接

#If 0
		'平均前の測定データファイル出力(応力、粘度、せん断速度)
		Dim fs As FileStream	= New FileStream("C:\Debug\BeforeAverage.csv", FileMode.Append, FileAccess.Write, FileShare.ReadWrite)
		Dim sw As StreamWriter	= New StreamWriter(fs, System.text.Encoding.UTF8)
		Dim cnt As Int32
		Dim sBuf As String = ""
		sBuf = "Stress,Viscosity,ShearRate,Visco*SRate,Stress/Area"
		sw.WriteLine(sBuf)
		For cnt = 0 To p_dtFile.iHosei.Length-1
			sBuf = p_dtFile.iHosei(cnt) & "," & p_dtFile.dViscosity(cnt) & "," & p_dtFile.dShearVelo(cnt) & "," & _
			p_dtFile.dViscosity(cnt) * p_dtFile.dShearVelo(cnt) & "," & p_dtFile.iHosei(cnt)/((p_cp.dDiameter/2)^2*Math.pi)
			sw.WriteLine(sBuf)
		Next
		sw.Close
		fs.Close
#end if

'If p_dtFile.iLT_TempBuf(iPt)<0
	'Debug.Print("")
'End If
		'長期間欠モード測定データをファイルへ一行追加(初回はヘッダ出力)
		p_dtFile.RealTimeMeasDataFileSaveText(eMeasMode.NormalLT, True, m_cts,,,,,,p_LTDataPointNum)			'測定値保存 追加モード

		'メイン画面計算値モニタ用
		p_dtFile.p_curStress		= p_dtFile.iLT_Hosei(iPt)				'応力
		p_dtFile.p_curStressP		= p_dtFile.dLT_Hosei100(iPt)			'応力%
		p_dtFile.p_curViscosity		= p_dtFile.dLT_XPosPhase(iPt)			'粘度
		p_dtFile.p_curPhase			= p_dtFile.dLT_XPosPhase(iPt)			'位相
		p_dtFile.p_curSplStrain		= p_dtFile.dLT_SplStrain(iPt)			'サンプル歪

		p_LTDataPointNum += 1												'長期間欠モード測定データポイント更新
		p_DispUpdate = 1

		'設定振幅・周期をセットし動作停止
		p_mltcp.AmplitudeX	= 0												'X軸振幅
		p_mltcp.AmplitudeY	= 0												'Y軸振幅
		p_mltcp.AmplitudeZ  = 0												'Z軸振幅

		sw3.Restart()														'表示更新タイマー
		'自動終了判定
		If p_strRTGSetName.Length = 0 Then
		'温度プロファイル中は自動終了しない
			'測定終了判定
			'応力制限判定 -----------------
			if p_dStressLimit <> 0
				If p_dtFile.iLT_Hosei.Max * 100 / p_dFullStressADVal > p_dStressLimit
					sw6.Stop
					Call WriteLog("測定終了 (応力制限)", eLOG_LEVEL.iINFO)
					p_iBGWOrderNext = BGW_ORDER.LT_WAIT_LOCKOUT                         '測定終了 プローブロック外し待ち>>>>>>
				Else
					p_iBGWOrderNext = BGW_ORDER.LT_DATACAL_DISP							'<<<<測定間欠動作中止中制御へ
				End If
			Else
				p_iBGWOrderNext = BGW_ORDER.LT_DATACAL_DISP								'<<<<測定間欠動作中止中制御へ
			End if
			'測定時間制限判定 -----------------
			If p_dMeasTimeLimit <> 0
				If m_DataPointTime >= p_dMeasTimeLimit
					'測定時間が制限超えた
					sw6.Stop
					Call WriteLog("測定終了 (時間制限)", eLOG_LEVEL.iINFO)
					p_iBGWOrderNext = BGW_ORDER.LT_WAIT_LOCKOUT                           '測定終了 プローブロック外し待ち>>>>>>>
				End If
			Else
				p_iBGWOrderNext = BGW_ORDER.LT_DATACAL_DISP							'<<<<測定間欠動作中止中制御へ
			End If
		Else
			p_iBGWOrderNext = BGW_ORDER.LT_DATACAL_DISP									'<<<<測定間欠動作中止中制御へ
		End If
		'本体エラー発生時
		If p_bDevError
			p_iBGWOrderNext = BGW_ORDER.LT_MEAS_ERROR_CANCEL
		End If

'-----------------------------------------------------------------------------------------------------------
'リアルタイムグラフ表示更新
	Case BGW_ORDER.LT_DATACAL_DISP
		If sw3.Elapsed.TotalSeconds > 5
			'表示更新タイムアウト
			p_iBGWOrderNext = BGW_ORDER.LT_MEAS_PROCESS_WAIT							'<<<<測定間欠動作中止中制御へ
		Else
			If p_RTM(0).p_DispUpdate = 0
			'表示更新完了
				p_iBGWOrderNext = BGW_ORDER.LT_MEAS_PROCESS_WAIT							'<<<<測定間欠動作中止中制御へ
			Else
				p_iBGWOrderNext = BGW_ORDER.LT_DATACAL_DISP									'<<<<継続
			End If
		End If

'-----------------------------------------------------------------------------------------------------------
'測定終了 プローブロック外し待ち
	Case BGW_ORDER.LT_WAIT_LOCKOUT
		p_mltcp.ZeroSW(False)                                                   'ZERO調制御開始Bit OFF
		p_mltcp.MeasSW(False)                                                   '測定開始指令Bit OFF
		If p_MeasureState =  eMeasStat.LOCKOUT_OK								'測定状態をロック解除済
			m_bPcLifeCheckDisBack = p_bPcLifeCheckDis								
			p_bPcLifeCheckDis = True												'PC生存チェック有効無効
			Sw1.Restart()
			p_iBGWOrderNext = BGW_ORDER.LT_WAIT_LOCKOUT1                           '終了データ処理へ
		Else
			p_iBGWOrderNext = BGW_ORDER.LT_WAIT_LOCKOUT							'継続
		End If

	Case BGW_ORDER.LT_WAIT_LOCKOUT1
		If sw1.Elapsed.TotalSeconds>1
			p_iBGWOrderNext = BGW_ORDER.LT_MEAS_PRO_FINISH                           '終了データ処理へ
		Else
			p_iBGWOrderNext = BGW_ORDER.LT_WAIT_LOCKOUT1							'継続
		End If

'測定終了制御  -------------------------------------------------------------------------------------
	Case BGW_ORDER.LT_MEAS_PRO_FINISH
		p_ElapsedMeasTime = -1		'経過時間 未実施
		p_iZeroAdjOk = -1			'ZERO調未実施へ
		p_mltcp.ZeroSW(False)		'ZERO調制御開始Bit OFF
		p_mltcp.MeasSW(False)		'測定開始指令Bit OFF

#If 1
		'ゲルタイム計算
        Select Case p_sCalcMethod
        Case lstCalcMethod(eCalcMethod.THR)
			iRet = p_dtFile.CalcGelTime(eCalcMethod.THR,	frmMain.p_iSelectGItem, p_dGelTime, p_dToqueThreshold, p_dFullStressADVal, p_dAprxRangeMin, p_dAprxRangeMax,,,,p_cts)
		End Select
		Select Case iRet
		Case 0		'ゲルタイム計算OK
			Select Case p_sCalcMethod
			Case lstCalcMethod(eCalcMethod.THR), lstCalcMethod(eCalcMethod.SLOPE), lstCalcMethod(eCalcMethod.PHASE)
				p_dtfile.p_head.dGelTime = p_dGelTime										'データファイルヘッダ用ゲルタイム
				Select Case p_dGelTime
				Case p_dJudgeRangeMin To p_dJudgeRangeMax
					p_iGelTimeJudge = 1			'OK
					p_dtFile.p_head.sGelTimeJudge = "OK"
				Case Else
					If (p_dJudgeRangeMax - p_dJudgeRangeMin) <> 0
						p_iGelTimeJudge = 0			'NG
						p_dtFile.p_head.sGelTimeJudge = "NG"
					Else
						p_iGelTimeJudge = 2		'NA
						p_dtFile.p_head.sGelTimeJudge = "NA"
					End If
				End Select
			Case lstCalcMethod(eCalcMethod.MELT)
				p_iGelTimeJudge = 2
				p_dtFile.p_head.sGelTimeJudge = "NA"
				p_dtfile.p_head.dGelTime = p_dGelTime					'データファイルヘッダ用ゲルタイム
				p_dtfile.p_head.dGelTimeTemp		= p_dMeltTemp.ToString("F1")
				p_dtfile.p_head.dGelTimeVisco		= p_dMeltVisco.ToString("F1")
			End Select

		Case -2		'ゲルタイム計算不可
			p_iGelTimeJudge = 2
			p_dtFile.p_head.sGelTimeJudge = "NA"
		case else	'例外エラー発生
			p_iGelTimeJudge = 2
			p_dtFile.p_head.sGelTimeJudge = "NA"
		End Select

		'補正データ込みで上書き更新(ゲルタイム、判定)
		'実測定時間をセット
		p_dtFile.p_head.iMeasActTime =	m_DataPointTime
		p_dtFile.RealTimeMeasDataFileSaveText(eMeasMode.NormalLT, False, m_cts)

#end If
		If sw1.Elapsed.TotalSeconds > 1 
			sw1.Restart()
			p_iBGWOrderNext = BGW_ORDER.LT_MEAS_PRO_FINISH_1									'測定終了制御_1へ
		Else
			Dim iLine As Int64
			iLine = p_dtFile.GetFileLineCount()												'ファイル行数を確認
			If iLine < p_dtFile.DataCount()
				p_iBGWOrderNext = BGW_ORDER.LT_MEAS_PRO_FINISH								'継続
			Else
				sw1.Restart()
				p_iBGWOrderNext = BGW_ORDER.LT_MEAS_PRO_FINISH_1							'測定終了制御_1へ
			End If
		End If

'測定終了制御_1  -------------------------------------------------------------------------------------
	Case BGW_ORDER.LT_MEAS_PRO_FINISH_1
		p_MeasureState =  eMeasStat.NOT_START												'測定状態を未開始
		If sw1.Elapsed.TotalSeconds > 1
			sw1.Restart()
			p_iBGWOrderNext = BGW_ORDER.LT_MEAS_PRO_FINISH_2								'測定終了制御_2へ
		Else
			p_iBGWOrderNext = BGW_ORDER.LT_MEAS_PRO_FINISH_1								'継続
		End If

'測定終了制御_2  -------------------------------------------------------------------------------------
	Case BGW_ORDER.LT_MEAS_PRO_FINISH_2
		'If sw1.Elapsed.TotalSeconds > 2
		'	p_bPcLifeCheckDis = m_bPcLifeCheckDisBack						
		'	p_iBGWOrderNext = BGW_ORDER.NO_ORDER
		'Else
		'	p_iBGWOrderNext = BGW_ORDER.LT_MEAS_PRO_FINISH_2								'継続
		'End If
		If sw1.Elapsed.TotalSeconds > 2
			p_bPcLifeCheckDis = m_bPcLifeCheckDisBack						
			p_iBGWOrderNext = BGW_ORDER.NO_ORDER
			'DB自動登録処理
			If p_cts.DBAutoReg() = 1
				If p_cts.DBSel = 0
					'SZKAutoRegistを起動
					m_frmM.AutoRejistExec(If(p_cts.SPCEn = 1, True, False), p_dtFile.p_head.sMeasureType, p_dtFile.GetDataFileName())
				Else
					'SQLite版 1行追加
					If p_clDB.InsertResultsRec(p_dtFile.p_head) = 0
						Call WriteLog("DB登録成功しました。" & p_dtFile.p_head.sFileName , eLOG_LEVEL.iINFO)
					Else
						Call WriteLog("DB登録失敗しました。" & p_dtFile.p_head.sFileName , eLOG_LEVEL.iFATAL)
					End If

					'dSPCValuesのみ取得
					m_frmM.btnSPCClick_SQLite(1)
				End if
			End If 
		Else
			p_iBGWOrderNext = BGW_ORDER.LT_MEAS_PRO_FINISH_2										'継続
		End If

	End Select



Catch ex As Exception
    Call FrmDebugLog(System.Reflection.MethodBase.GetCurrentMethod.Name & ex.Message)
    Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub

''' <summary>
''' 応力較正制御
''' </summary>
''' <param name="sw1"></param>
''' <param name="sw2"></param>
Private Sub CaliblationProc(byRef sw1 As Stopwatch, byRef sw2 As Stopwatch)
Dim iRet As Int32
Dim iMode As Int32
Dim iFirstIdx As int32
Try
	Select Case p_iBGWOrder
	case BGW_ORDER.CALIB_TEMP
		p_mltcp.TempPreset(1)	= p_CalibPreTemp											'更正処理　温度
		p_mltcp.Temp2Preset(1)	= p_CalibPreTemp											'更正処理　温度
		p_mltcp.Temp3Preset(1)	= p_clSpec.SGTempGet(p_mltcp.TempPreset(1))
		p_mltcp.StartTempCont	= 3															'温度制御開始Bit ON
		p_iHeatingHP = IIf(p_mltcp.isStatHeaterOn(), 1, 0)
		p_iHeatingPB = IIf(p_mltcp.isStatHeaterOn2(), 1, 0)
		p_iHeatingSG = IIf(p_mltcp.isStatHeaterOn3(), 1, 0)

	Case BGW_ORDER.CALIB_ZERO
		p_mltcp.ZeroSW(True)																'ZERO調制御開始Bit ON
		p_iZeroAdjOk = 0
		p_CaliblationState.iState = eCalibState.ZERO_EXEC                                   '較正用ステータス->ゼロ調実行中
		sw1.Restart                                                                         '応答Bit タイマースタート
		p_iBGWOrderNext = BGW_ORDER.ZERO_BUTTON_CNF                                         '応答確認へ

	Case BGW_ORDER.CALIB_START_0
		p_iBGWOrderNext = BGW_ORDER.CALIB_START_X                                    

	Case BGW_ORDER.CALIB_START_1
		p_iBGWOrderNext = BGW_ORDER.CALIB_START_C                                    

	Case BGW_ORDER.CALIB_START_2
		p_iBGWOrderNext = BGW_ORDER.CALIB_START_P                                    

	Case BGW_ORDER.CALIB_START_X, BGW_ORDER.CALIB_START_C, BGW_ORDER.CALIB_START_P
		If p_iConnectState = CONNECT_ST.CONNECTING                                          'リンク中のみ
			'If p_mltcp.IsMeasStartSW() = False
			'未測定中であること


				'測定選択された最初のIndexを探す
				Select Case p_iBGWOrder
				Case BGW_ORDER.CALIB_START_X
					iMode = 0
				Case BGW_ORDER.CALIB_START_C
					iMode = 1
				Case BGW_ORDER.CALIB_START_P
					iMode = 2
				End Select
				iRet = FirstIndexSerchInCalibArray(iMode, iFirstIdx)
				If iRet = 0
					Dim clb() As stPoint = Nothing
					'XY動作モード
					Select Case iMode
					Case 0
						p_CaliblationState.sMode = "X"                                          'X動作
						clb= p_clbX
					Case 1
						p_CaliblationState.sMode = "C"                                          '円動作
						clb= p_clbC
					Case 2
						p_CaliblationState.sMode = "P"                                          '点動作
						clb= p_clbZ
					End Select
					'X軸振幅
					p_mltcp.AmplitudeX = clb(iFirstIdx).dAmplitudeX * 1000.0
					'Y軸振幅
					p_mltcp.AmplitudeY = clb(iFirstIdx).dAmplitudeY * 1000.0
					'Z軸振幅
					p_mltcp.AmplitudeZ = clb(iFirstIdx).dAmplitudeZ * 1000.0
					'X軸周期
					p_mltcp.PeriodX(1) = clb(iFirstIdx).dPeriodX
					'Y軸周期
					p_mltcp.PeriodY(1) = clb(iFirstIdx).dPeriodY
					'Z軸周期
					p_mltcp.PeriodZ(1) = clb(iFirstIdx).dPeriodZ

					p_CaliblationState.iState = eCalibState.PREPAREING                          '較正準備中
					p_CaliblationState.iCol = -1
					p_CaliblationState.iRaw = -1



					p_mltcp.MeasSW(True)											            '測定開始指令Bit ON
					p_MeasureState = eMeasStat.PREPAREING_1                                     '測定準備中
					p_iBGWOrderNext = BGW_ORDER.CALIB_START_CNF									'応答確認へ
					sw1.Restart                                                                 '応答Bit タイマースタート
				Else
				'測定選択されていない
					Select Case p_iBGWOrder
					Case BGW_ORDER.CALIB_START_X
						p_iBGWOrderNext = BGW_ORDER.CALIB_START_C				'円モード測定へ                                  
					Case BGW_ORDER.CALIB_START_C
						p_iBGWOrderNext = BGW_ORDER.CALIB_START_P               '点モード測定へ      
					Case BGW_ORDER.CALIB_START_P
						p_iBGWOrderNext = BGW_ORDER.CALIB_STOP					'終了処理へ
					End Select
				End If

			'End If
		End If
			
	Case BGW_ORDER.CALIB_START_CNF
		'測定SW応答確認
		If p_mltcp.isRespMeasStart() AndAlso p_mltcp.isStatMeasuringTempo()					'応答bit[1108.00]、測定中ステータス[1109.02]
			p_dtFile.DataCount = 0															'蓄積データカウンタクリア
			p_dtFile.ReadBufferClear()														'蓄積エリアのクリア
			p_dtFile.MesFileNumber = 0														'ファイル連番クリア
			p_dtFile.SetDataFileName() = Format(Now, "yyyyMMddHHmmss")						'データファイル名


			System.Threading.Thread.Sleep(500)
			p_iBGWOrderNext = BGW_ORDER.CALIB_PROCESSING                                    '測定終了監視へ
		Else
			If sw1.Elapsed.TotalSeconds > cResponseTimeOut Then
				'応答BIT未確認タイムオーバー	5秒
				Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & ":Confirm Bit Response Time Over (START_BUTTON_CNF)")
				p_iBGWOrderNext = BGW_ORDER.CALIB_STOP										'終了処理
			Else
				p_iBGWOrderNext = BGW_ORDER.CALIB_START_CNF									'応答確認継続
			End If
		End If


	Case BGW_ORDER.CALIB_PROCESSING
		If p_bDevError
		'装置エラー発生したら先にアラーム表示を出す
			'p_iBGWOrderNext = BGW_ORDER.CALIB_PROCESSING                    '継続
			p_iBGWOrderNext = BGW_ORDER.CALIB_STOP											'終了処理
		ElseIf (p_mltcp.isRespMeasStart() = False) Or (p_mltcp.isStatMeasuringTempo()=False)	'応答bit[1108.00]、測定中ステータス[1109.02]
			'本体側が測定終了した
			p_iBGWOrderNext = BGW_ORDER.CALIB_STOP											'終了処理
		ElseIf p_mltcp.isItLock()
			'扉オープンされた
			p_iBGWOrderNext = BGW_ORDER.CALIB_STOP											'終了処理
		Else
			'測定実施中
			':::: 応力較正制御処理 :::::::::::::::::::::::::::::::::::::::::::::::::::
			If iCalibFirst = 0 Then
				m_iCalibTrig = 1
			End If
			Select Case p_CaliblationState.sMode
			Case "X"
				CalibControl("X", p_ClbX, sw2, p_dtFile)						'[[[[[ 応力較正制御処理 ]]]]]
			Case "C"
				CalibControl("C", p_ClbC, sw2, p_dtFile)						'[[[[[ 応力較正制御処理 ]]]]]
			Case "P"
				CalibControl("P", p_ClbZ, sw2, p_dtFile)						'[[[[[ 応力較正制御処理 ]]]]]
			End Select

			If m_iCalJob = eClbJob.FINISH                                       '100％応力測定モード終了確認
				Select Case p_CaliblationState.sMode
				Case "X"
					p_iBGWOrderNext = BGW_ORDER.CALIB_START_1					'Cモード開始へ
				Case "C"
					p_iBGWOrderNext = BGW_ORDER.CALIB_START_2					'Pモード開始へ
				Case "P"
					iCalibFirst = 0
					sw1.Restart
					p_iBGWOrderNext = BGW_ORDER.CALIB_STOP						'終了処理へ
				End Select
			Else
				p_iBGWOrderNext = BGW_ORDER.CALIB_PROCESSING                    '継続
			End If
			'::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
		End If
								

'応力較正終了制御  -------------------------------------------------------------------------------------
	Case BGW_ORDER.CALIB_STOP
		p_mltcp.MeasSW(False)															'測定開始指令Bit OFF
		p_mltcp.ZeroSW(False)															'ZERO調制御開始Bit OFF
		m_iCalJob = eClbJob.FINISH
		p_CaliblationState.iState = eCalibState.FINISHING
		p_MainLink = 0																	'frmMainでの通信リンク(書込み)を許可
		p_iBGWOrderNext = BGW_ORDER.CALIB_STOP_1										'
		sw1.Restart

	Case BGW_ORDER.CALIB_STOP_1																'熱板下降確認
		If sw1.Elapsed.TotalSeconds > 1 Then
			iCalibFirst = 0
			m_idx = 0
			m_iCalJob = -1
			p_ClbXIdx = 0
			p_ClbCIdx = 0
			p_ClbPIdx = 0
			p_iZeroAdjOk = -1																'ゼロ調正未完了

			p_MeasureState = eCalibState.NOT_START
			p_CaliblationState.iState = eCalibState.NOT_START
			p_iBGWOrderNext = BGW_ORDER.NO_ORDER
			Call WriteLog("Calib 測定終了 " & System.Reflection.MethodBase.GetCurrentMethod.Name, eLOG_LEVEL.iINFO)

		Else
			p_iBGWOrderNext = BGW_ORDER.CALIB_STOP_1											'継続(熱板下降確認待ち)
		End If

	Case BGW_ORDER.CALIB_STOP_2																'剥しシーケンス確認
		p_iBGWOrderNext = BGW_ORDER.CALIB_STOP_2											'継続(剥しシーケンス確認待ち)
#If 0
'==========================================================================================================================================================
'自動剥がし制御制御  -------------------------------------------------------------------------------------
	Case BGW_ORDER.AUTO_PEELING
		p_MainLink = 1																	'frmMainでの通信リンク(書込み)を一時的に遮断
		p_CaliblationState.iState = eCalibState.AUTO_PEELING
		p_iBGWOrderNext = BGW_ORDER.AUTO_PEELING_1										'温度監視へ

	Case BGW_ORDER.AUTO_PEELING_1
		'温度設定、ヒータON 設定温度
		p_mltcp.TempPreset(1) = p_AutoPeelingTemp
		p_mltcp.StartTempCont = 3														'HP,PB温度制御開始Bit ON
		p_iHeatingHP = IIf(p_mltcp.isStatHeaterOn(), 1, 0)
		sw1.Restart()
		p_iBGWOrderNext = BGW_ORDER.AUTO_PEELING_2										'温度監視へ

	Case BGW_ORDER.AUTO_PEELING_2
		'温度監視 設定に達したら剥がす
		If sw1.Elapsed.TotalSeconds > 300
		'300秒タイムアップで何もしないで終了（ロックを緩めて手動でZ位置を０へ）
			Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & ":AutoPeeling Temperature TimeOver")
			p_MainLink = 0																'frmMainでの通信リンク(書込み)を許可
			p_iBGWOrderNext = BGW_ORDER.AUTO_PEELING_3									'Z軸手動移動
		Else
			p_CaliblationState.iState = eCalibState.AUTO_PEELING
''			If Math.abs(p_mltcp.HeatTempAct - p_mltcp.TempPreset(1)) <= 5
''			If Math.abs(p_mltcp.HeatTempAct - p_mltcp.TempPreset(1)) <= 0
			If p_mltcp.HeatTempAct >= p_mltcp.TempPreset(1)								'設定温度以上になったら終了
				p_iHotPlatePos = 0
				p_iBGWOrderNext = BGW_ORDER.AUTO_PEELING_3								'Z軸手動移動
			Else
				p_iBGWOrderNext = BGW_ORDER.AUTO_PEELING_2								'継続
			End If
			If p_mltcp.DevErr1 <> 0 Or p_mltcp.DevErr2 <> 0
				p_iBGWOrderNext = BGW_ORDER.AUTO_PEELING_6				'終了処理へ
			End If
		End If
'熱板 手動駆動制御 
	Case BGW_ORDER.AUTO_PEELING_3
		p_mltcp.PosHeatB = p_iHotPlatePos                           '目標位置セット
		p_mltcp.PosOrderZ(True)										'Z駆動指令BIT ON
		p_iBGWOrderNext = BGW_ORDER.AUTO_PEELING_4		            '手動駆動応答確認へ
		sw1.Restart                                                 '応答Bit タイマースタート
'熱板 手動駆動確認
	Case BGW_ORDER.AUTO_PEELING_4
		If p_mltcp.isRespHeatMove()
			sw1.Restart                                             '応答Bit タイマースタート
			p_iBGWOrderNext = BGW_ORDER.AUTO_PEELING_5				'手動駆動応答確認へ
		Else
			If sw1.Elapsed.TotalSeconds > cResponseTimeOutDrive Then
				'応答BIT未確認タイムオーバー
				WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & ":Confirm Bit Response Time Over (HOT_PLATE_DRV_CNF)")
				p_iBGWOrderNext = BGW_ORDER.AUTO_PEELING_6          '終了処理へ
			Else
				p_iBGWOrderNext = BGW_ORDER.AUTO_PEELING_4          '手動駆動応答確認へ
			End If
		End If
	Case BGW_ORDER.AUTO_PEELING_5
		'If Not p_mltcp.isRespHeatMove()
			p_iBGWOrderNext = BGW_ORDER.AUTO_PEELING_6				'終了処理へ
		'Else
		'	If sw1.Elapsed.TotalSeconds > cResponseTimeOutDrive Then
		'		'応答BIT未確認タイムオーバー
		'		WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & ":Confirm Bit Response Time Over (HOT_PLATE_DRV_CNF)")
		'		p_iBGWOrderNext = BGW_ORDER.AUTO_PEELING_6			'終了処理へ
		'	Else
		'		p_iBGWOrderNext = BGW_ORDER.AUTO_PEELING_5          '手動駆動応答確認へ
		'	End If
		'End If
	Case BGW_ORDER.AUTO_PEELING_6
		p_mltcp.PosOrderZ(False)									'Z駆動指令BIT OFF
		p_MainLink = 0												'frmMainでの通信リンク(書込み)を許可
		p_mltcp.TempPreset(1) = p_CalibPreTemp						'更正処理　温度
		p_mltcp.StartTempCont = 0									'温度制御開始Bit OFF
		p_CaliblationState.iState = eCalibState.NOT_START

'自動剥がし停止
	Case BGW_ORDER.AUTO_PEELING_STOP					
		p_iBGWOrderNext = BGW_ORDER.AUTO_PEELING_6				'終了処理へ
#end if
'==========================================================================================================================================================
'手動剥がし制御制御  -------------------------------------------------------------------------------------
	Case BGW_ORDER.MANU_PEELING
		p_MainLink = 1												'frmMainでの通信リンク(書込み)を一時的に遮断
		p_CaliblationState.iState = eCalibState.MANU_PEELING
		p_mltcp.PosHeatB = p_iHotPlatePos                           '目標位置セット
		p_mltcp.PosOrderZ(True)										'Z駆動指令BIT ON
		p_iBGWOrderNext = BGW_ORDER.MANU_PEELING_1					'温度設定へ

	Case BGW_ORDER.MANU_PEELING_1
		'温度設定、ヒータON 設定温度
		p_mltcp.TempPreset(1) = p_AutoPeelingTemp
		p_mltcp.StartTempCont = 1									'HP温度制御開始Bit ON
		p_iHeatingHP = IIf(p_mltcp.isStatHeaterOn(), 1, 0)
		sw1.Restart()
		p_iBGWOrderNext = BGW_ORDER.MANU_PEELING_2					'温度監視へ

	Case BGW_ORDER.MANU_PEELING_2
		'温度監視 設定に達したら剥がす
		If sw1.Elapsed.TotalSeconds > 300
		'300秒タイムアップで何もしないで終了
			Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & ":AutoPeeling Temperature TimeOver")
			p_MainLink = 0											'frmMainでの通信リンク(書込み)を許可
			p_iBGWOrderNext = BGW_ORDER.MANU_PEELING_3				'終了
		Else
			If p_mltcp.HeatTempAct >= p_mltcp.TempPreset(1)			'設定温度以上になったら終了
				p_iHotPlatePos = 0
				p_iBGWOrderNext = BGW_ORDER.MANU_PEELING_3			'終了
			Else
				p_iBGWOrderNext = BGW_ORDER.MANU_PEELING_2			'継続
			End If
			If p_mltcp.DevErr1 <> 0 Or p_mltcp.DevErr2 <> 0
				p_iBGWOrderNext = BGW_ORDER.MANU_PEELING_3			'終了処理へ
			End If
		End If

	Case BGW_ORDER.MANU_PEELING_3
		if p_mltcp.isStatGoingUpDown()								'動作中?
			p_iBGWOrderNext = BGW_ORDER.MANU_PEELING_3				'継続
		Else
			p_mltcp.PosOrderZ(False)									'Z駆動指令BIT OFF
			p_MainLink = 0												'frmMainでの通信リンク(書込み)を許可
			p_mltcp.TempPreset(1) = p_CalibPreTemp						'更正処理　温度
			p_mltcp.StartTempCont = 0									'温度制御開始Bit OFF
			p_CaliblationState.iState = eCalibState.NOT_START
		End If

'手動剥がし停止
	Case BGW_ORDER.MANU_PEELING_STOP					
		p_iBGWOrderNext = BGW_ORDER.MANU_PEELING_3				'終了処理へ


	End Select
Catch ex As Exception
    Call FrmDebugLog(System.Reflection.MethodBase.GetCurrentMethod.Name & ex.Message)
    Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try

End Sub
''' <summary>
''' フローカーブ（速度・温度依存）測定制御
''' </summary>
''' <param name="sw1"></param>
''' <param name="sw2"></param>
Private Sub FlowCurve1Proc(byRef sw1 As Stopwatch, byRef sw2 As Stopwatch)
Try
	Select Case p_iBGWOrder
	case BGW_ORDER.FLCV_START
		p_mltcp.TempPreset(1) = p_FlcvPreTemp												'更正処理　温度
		p_mltcp.StartTempCont = 7															'温度制御開始Bit ON (HP,PB)
		p_iHeatingHP = IIf(p_mltcp.isStatHeaterOn(), 1, 0)
		p_iHeatingPB = IIf(p_mltcp.isStatHeaterOn2(), 1, 0)
		p_iHeatingSG = IIf(p_mltcp.isStatHeaterOn3(), 1, 0)
		p_iBGWOrderNext = BGW_ORDER.FLCV_START_0  

	Case BGW_ORDER.FLCV_START_0
		p_iBGWOrderNext = BGW_ORDER.FLCV_START_1                                    

	Case BGW_ORDER.FLCV_START_1
		p_iBGWOrderNext = BGW_ORDER.FLCV_START_2                                    

	Case BGW_ORDER.FLCV_START_2
		If p_iConnectState = CONNECT_ST.CONNECTING                                      'リンク中のみ
			Select Case p_FlcvMeasMode
			Case eViscoMeaure.SpeedDep
			'速度依存
				p_mltcp.AmplitudeX = p_FlcvChgVal(0) * 1000.0							'X軸振幅(初期)
				p_mltcp.AmplitudeY = If(p_FlcvActMode=0, p_FlcvChgVal(0) * 1000.0, 0)	'Y軸振幅(初期)
				p_mltcp.AmplitudeZ = 0
				p_mltcp.TempPreset(1) = p_FlcvPreTemp									'温度指定
				p_mltcp.Temp2Preset(1) = p_FlcvPreTemp									'温度指定
				p_mltcp.Temp3Preset(1) = p_clSpec.SGTempGet(p_mltcp.TempPreset(1))
			Case eViscoMeaure.TempDep
			'温度依存
				p_mltcp.AmplitudeX = p_FlcvAmplitude * 1000.0							'X軸振幅指定
				p_mltcp.AmplitudeY = If(p_FlcvActMode=0, p_FlcvAmplitude * 1000.0, 0)	'Y軸振幅指定
				p_mltcp.AmplitudeZ = 0
				p_mltcp.TempPreset(1)	= p_FlcvChgVal(0)								'HP温度(初期)
				p_mltcp.Temp2Preset(1)	= p_FlcvChgVal(0)								'PB温度(初期)
				p_mltcp.Temp3Preset(1)	= p_clSpec.SGTempGet(p_mltcp.TempPreset(1))
			End Select
			p_mltcp.PeriodX(1) = p_FlcvPeriod											'X軸周期
			p_mltcp.PeriodY(1) = p_FlcvPeriod											'Y軸周期
			p_mltcp.MeasSW(True)											            '測定開始指令Bit ON
			p_iBGWOrderNext = BGW_ORDER.FLCV_START_CNF									'応答確認へ
			sw1.Restart                                                                 '応答Bit タイマースタート

		End If
			
	Case BGW_ORDER.FLCV_START_CNF
		'測定SW応答確認
		If p_mltcp.isRespMeasStart() AndAlso p_mltcp.isStatMeasuringTempo()					'応答bit[1108.00]、測定中ステータス[1109.02]
			p_dtFile.DataCount = 0															'蓄積データカウンタクリア
			p_dtFile.ReadBufferClear()														'蓄積エリアのクリア
			p_dtFile.MesFileNumber = 0														'ファイル連番クリア
			p_dtFile.SetDataFileName = Format(Now, "yyyyMMddHHmmss")						'データファイル名

			System.Threading.Thread.Sleep(500)
			p_iBGWOrderNext = BGW_ORDER.FLCV_PROCESSING                                    '測定終了監視へ
		Else
			If sw1.Elapsed.TotalSeconds > cResponseTimeOut Then
				'応答BIT未確認タイムオーバー	5秒
				Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & ":Confirm Bit Response Time Over (START_BUTTON_CNF)")
				p_iBGWOrderNext = BGW_ORDER.FLCV_STOP										'終了処理
			Else
				p_iBGWOrderNext = BGW_ORDER.FLCV_START_CNF									'応答確認継続
			End If
		End If


	Case BGW_ORDER.FLCV_PROCESSING
		If (p_mltcp.isRespMeasStart() = False) Or (p_mltcp.isStatMeasuringTempo()=False)	'応答bit[1108.00]、測定中ステータス[1109.02]
			'本体側が測定終了した
			p_iBGWOrderNext = BGW_ORDER.FLCV_STOP											'終了処理
		ElseIf p_mltcp.isItLock()
			'扉オープンされた
			p_iBGWOrderNext = BGW_ORDER.FLCV_STOP											'終了処理
		Else
			'測定実施中
			':::: FLCV制御処理 :::::::::::::::::::::::::::::::::::::::::::::::::::
			If iCalibFirst = 0 Then
				m_iCalibTrig = 1
			End If

			FlcvControl1(sw2, p_dtFile)											'[[[[[ フローカーブ測定制御処理 ]]]]]

			If m_iCalJob = eClbJob.FINISH                                       'FLCV測定終了確認
				iCalibFirst = 0
				p_mltcp.StartTempCont = 0										'温度制御開始Bit OFF(HP+PB)
				sw1.Restart
				p_iBGWOrderNext = BGW_ORDER.FLCV_STOP							'終了処理へ
			Else
				p_iBGWOrderNext = BGW_ORDER.FLCV_PROCESSING		                '継続
			End If
			'::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
		End If
								

'FLCV終了制御  -------------------------------------------------------------------------------------
	Case BGW_ORDER.FLCV_STOP
		If p_iConnectState = CONNECT_ST.CONNECTING Then                                     'リンク中のみ
			p_mltcp.MeasSW(False)															'測定開始指令Bit OFF
			p_mltcp.ZeroSW(False)															'ZERO調制御開始Bit OFF
			m_iCalJob = eClbJob.FINISH
			p_CaliblationState.iState	= eCalibState.FINISHING
			iViscoState					= eViscoState.NOT_START
			p_MainLink = 0																	'frmMainでの通信リンク(書込み)を許可
			p_iBGWOrderNext = BGW_ORDER.FLCV_STOP_1										'
			sw1.Restart
		End If

	Case BGW_ORDER.FLCV_STOP_1
		If sw1.Elapsed.TotalSeconds > 1 Then
			iCalibFirst = 0
			m_idx = 0
			m_iCalJob = -1
			p_iZeroAdjOk = -1																'ゼロ調正未完了
			Call WriteLog("フローカーブ 測定終了 " & System.Reflection.MethodBase.GetCurrentMethod.Name, eLOG_LEVEL.iINFO)


		Else
			p_iBGWOrderNext = BGW_ORDER.FLCV_STOP_1										'継続
		End If
	End Select
Catch ex As Exception
    Call FrmDebugLog(System.Reflection.MethodBase.GetCurrentMethod.Name & ex.Message)
    Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try

End Sub
''' <summary>
''' フローカーブ（時間依存）測定制御
''' </summary>
''' <param name="sw1"></param>
''' <param name="sw2"></param>
Private Sub FlowCurve2Proc(byRef sw1 As Stopwatch, byRef sw2 As Stopwatch, byRef sw3 As Stopwatch, byRef sw6 As Stopwatch)
Dim i As Int32 = 0
Try
	Select Case p_iBGWOrder
	Case BGW_ORDER.FLCV_START_BUTTON
		If p_iConnectState = CONNECT_ST.CONNECTING Then                                     'リンク中のみ
			p_mltcp.MeasSW(True)												            '測定開始指令Bit ON
			p_MeasureState = eMeasStat.PREPAREING_1							                '測定準備中
			p_iBGWOrderNext = BGW_ORDER.FLCV_START_BUTTON_CNF					            '応答確認へ
			sw1.Restart																        '応答Bit タイマースタート
			p_CaliblationState.iState = eCalibState.NOT_START								'較正用ステータス->未実行(ZERO調後のクリア)
		End If

	Case BGW_ORDER.FLCV_START_BUTTON_CNF
		'測定SW応答確認
		If p_mltcp.isRespMeasStart() AndAlso p_mltcp.isStatMeasuringTempo()					'応答bit[1108.00]、測定中ステータス[1109.02]

			'データファイル名の決定
			p_dtFile.MesFileNumber = 0														'ファイル連番クリア
			p_dtFile.SetDataFileName = p_dtFile.sMeasindCode(eMesCD.FL2) & "-" & Format(Now, "yyyyMMddHHmmss")			'フローカーブ（時間依存）
			MeasureHeaderInfoSet(eMeasMode.FlowCurve_Time)									'測定開始時のファイルヘッダ情報のセット
			p_mltcp.AmplitudeX	= p_FlcvChgVal(0) * 1000.0									'X軸振幅
			p_mltcp.AmplitudeY	= If(p_FlcvActMode=0, p_FlcvChgVal(0) * 1000.0, 0)			'Y軸振幅
			p_mltcp.AmplitudeZ = 0
			p_mltcp.PeriodX(1)	= p_FlcvPeriod												'X軸周期
			p_mltcp.PeriodY(1)	= p_FlcvPeriod												'Y軸周期
			p_mltcp.PeriodZ(1)	= 0

			'区間変化量計算に必要なサンプル数計算 必要周期(=10)分 周期 * 区間数(10) / サンプルインターバル
			'p_SampleNumForPeriod = Ctype(p_mltcp.PeriodX(1) * 1000.0 * 10 / MEAS_TIME_RESOLUTION, Int64)
			'System.Threading.Thread.Sleep(500)
			p_MeasureState = eMeasStat.PREPAREING_1
			p_iBGWOrderNext = BGW_ORDER.FLCV_MEAS_PREPARE_1	                                '測定準備1へ
			sw3.Restart()																	'測定制限時間SW開始
		Else
			If sw1.Elapsed.TotalSeconds > cResponseTimeOut Then
				'応答BIT未確認タイムオーバー	5秒
				p_mltcp.MeasSW(False)													    '測定開始指令Bit OFF
				Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & ":Confirm Bit Response Time Over (START_BUTTON_CNF)")
				sw1.Restart()
				p_iBGWOrderNext = BGW_ORDER.FLCV_MEAS_PRO_FINISH							'測定終了へ
			Else
				p_iBGWOrderNext = BGW_ORDER.FLCV_START_BUTTON_CNF                           '応答確認継続
			End If
		End If
		'測定キャンセル確認
		If p_MeasureState < eMeasStat.RESERV
			p_mltcp.MeasSW(False)															'測定開始指令Bit OFF
			p_iBGWOrderNext = BGW_ORDER.NO_ORDER
		End If

	Case BGW_ORDER.FLCV_MEAS_PREPARE_1
		If (p_mltcp.isRespMeasStart() = False) Or (p_mltcp.isStatMeasuringTempo()=False)	'応答bit[1108.00]、測定中ステータス[1109.02]
			'本体側が測定終了した
			p_mltcp.MeasSW(False)															'測定開始指令Bit OFF
			sw1.Restart()
			p_iBGWOrderNext = BGW_ORDER.FLCV_MEAS_PRO_FINISH								'測定終了へ
		Elseif p_mltcp.isStatMeasuring() = True 'Andalso p_mltcp.isStatXYAction Then
		'測定中(クリア後)がON-> ゲルタイム ０秒 ここから測定データ取得開始
			m_iCorrRet = 0
			m_iStartIdx = 0
			m_bRealCorrect = False
			m_iCorrectCount = 0
			m_iPeriodSample = p_mltcp.PeriodX(1) * 1000 / MEAS_TIME_RESOLUTION			'1周期分のサンプル数計算
			'sbLoadRTGSetting(p_strRTGSetName)

			sw6.Restart()
			p_FlcvCycleOnCnt = 0
			p_FlcvCycleOfCnt = 0
			p_ElapsedMeasTime = sw6.Elapsed.TotalSeconds
			p_MeasureState = eMeasStat.DATA_TRANS
			p_iBGWOrderNext = BGW_ORDER.FLCV_MEAS_PROCESS                               '測定終了監視へ
		Else
			p_iBGWOrderNext = BGW_ORDER.FLCV_MEAS_PREPARE_1                             '継続
		End If
'測定手動終了中止制御  -------------------------------------------------------------------------------------
	Case BGW_ORDER.FLCV_CANCEL_BUTTON
		If p_MeasureState = eMeasStat.DATA_TRANS
			p_iBGWOrderNext = BGW_ORDER.FLCV_MEAS_PRO_DATA									'終了データ処理へ
		Else
			p_iBGWOrderNext = BGW_ORDER.FLCV_MEAS_PRO_FINISH								'測定終了へ
			sw1.Restart()
		End If
'-----------------------------------------------------------------------------------------------------------
'測定データ取得制御  -------------------------------------------------------------------------------------

	Case BGW_ORDER.FLCV_MEAS_PROCESS
		'測定終了監視
		If (p_mltcp.isRespMeasStart() = False) Or (p_mltcp.isStatMeasuringTempo() = False)  '応答bit[1108.00]、測定中ステータス[1109.02]
			'本体側が測定終了した
			p_iBGWOrderNext = BGW_ORDER.FLCV_MEAS_PRO_DATA                                  '測定したところまでで測定終了しデータ処理
		ElseIf p_mltcp.isItLock()
			'扉オープンされた
			p_iBGWOrderNext = BGW_ORDER.FLCV_MEAS_PRO_DATA                                  '測定したところまでで測定終了しデータ処理
		Else
			p_ElapsedMeasTime = sw6.Elapsed.TotalSeconds
			'測定実施中
			If p_mltcp.bMesDataEnable Then
			'測定データ有効
				'測定データ蓄積セーブ -------------------
				p_dtFile.RealTimeMeasDataFileSaveText(eMeasMode.FlowCurve_Time, True, m_cts)               '測定値累積保存 追加モード


			'リアルタイム中間値補正処理 ----------------------
				If p_dtFile.DataCount-m_iStartIdx >= m_iPeriodSample 
				'1周期分のデータ取得まで待つ
					if m_iCorrectCount = 0
					'初回は0インデックスから
						m_iStartIdx = 0
						datCal.AveTempClear(p_mltcp.PeriodX(1), 0)			'領域クリア
					Else
					End If
					'リアルタイム中間値補正
					'Z決定位置をセット
					If p_mltcp.ZConfirmPos<>0
						p_dtFile.p_head.dZAfterPos = Ctype(p_mltcp.ZConfirmPos, Double) / 10.0
					End If
					Dim cp As DataCal.stCalcPara = Nothing
					cp.iMeas		= eHoseiMeasMode.FlowCurve
					cp.iMode		= eHoseiCalcMode.RealTime
					cp.cts			= m_cts
					cp.clDt			= p_dtFile
					cp.dPeriod		= p_mltcp.PeriodX(1)
					'cp.dAmplitude	= p_FlcvChgVal(0)
					cp.dAmplitude	= p_mltcp.AmplitudeX/1000
					cp.iSampleStart	= m_iStartIdx
					cp.iSampleNum	= p_dtFile.iHosei.Length - m_iStartIdx
					cp.fltSet		= p_FltSetting
					cp.dXYWait		= p_mltcp.MesPreWait(1) + p_mltcp.MeasWaitTime(1)
					cp.dZeroStress	= p_dFullStressZero
					cp.dAttenuation	= p_dFullStressAttenuation
					cp.dDiameter	= p_dDiameter
					cp.dMeasGap		= p_mltcp.GapPreset(1)
					cp.dFullStress	= p_dFullStressADVal
					cp.dPxPhase		= p_dFullStressPxPhase
					cp.dPyPhase		= p_dFullStressPyPhase
					cp.dPzPhase		= p_dFullStressPzPhase
					cp.iZeroPos		= p_iZPosition
					cp.iHPPosControl	= eHCont.Gap
					cp.dMeasureStress	= p_clSpec.spi.dMeasureStress
					cp.iCalcType		= eCalcMethod.THR
					cp.dDiffInterval	= p_clSpec.spi.dDiffInterval
					cp.iStrsSensMode	= p_FlcvActMode									'応力検知モード0,1,2
					cp.iAmpMode			= eAmpMode.AmpConst								'Z振動モード 0,1
					cp.dRCCBaseThickness = 0.0
					cp.dXYWeightCoef	= p_dNConv										'重量換算
					p_cp = cp															'計算パラメータを退避
					Select Case p_FlcvActMode
				    Case 0	'円モード
						m_iCorrRet = datCal.CorrectRealProcVector( cp )
				    Case 1	'Xモード
						m_iCorrRet = datCal.CorrectRealProcVector( cp )
					End Select
					p_dMesGap = cp.dMesGap		'測定GAP計算値 モニタ用
					p_dtFile.p_head.dRealGap = cp.dMesGap								'実測定GAPをヘッダへセット

					#If 0
					'粘度関係計算
					Dim uVp As Datafile.ViscosityCalcParameter
					uVp.dPeriod			= p_FlcvPeriod									'設定周期
					uVp.dAmplitude		= p_FlcvChgVal(0) / 10^3						'設定振幅(m)
					uVp.dMeasGap		= p_mltcp.GapPreset(1) / 10^3					'測定GAP(m)
					uVp.dProbeSquare	= Math.PI * ((p_FlcvPbDiameter/10^3) / 2)^2		'プローブ面積(m2)
					uVp.dVelocityRate	= 1.0											'せん断速度補正係数

					'粘度計算処理
					p_FlcvArrayUpdate = 1
					Array.Resize(p_dtFile.dViscosity,		p_dtFile.iHosei.Length)
					Array.Resize(p_dtFile.dShearStress,		p_dtFile.iHosei.Length)
					Array.Resize(p_dtFile.dShearVelocity,	p_dtFile.iHosei.Length)
					For i = 0 To p_dtFile.iHosei.GetUpperBound(0)
						p_dtFile.dViscosity(i) = Double.NaN
						uVp.dMeasStress		= p_dtFile.iHosei(i) / 10^6					'測定応力(N)
						datCal.ViscosityCalc( uVp )										'粘度計算
						p_dtFile.dViscosity(i)		= uVp.dViscosity					'粘度
						p_dtFile.dShearStress(i)	= uVp.dShearStress					'せん断応力
						p_dtFile.dShearVelocity(i)	= uVp.dShearVelocity				'せん断速度
					Next i
#end if
					p_FlcvArrayUpdate = 0

					m_iStartIdx += p_dtFile.iHosei.Length - m_iStartIdx					'開始インデックスの更新
					m_iCorrectCount += 1
				End If
			'--------------------------------------------------

				p_dtFile.MesBlockCnt += 1                                               '受信ブロックカウンタ更新

				p_mltcp.ZeroSW(False)                                                   'ZERO調制御開始Bit OFF（測定中ゼロ調指令はOFFにする）

#If 1

		If p_mltcp.isStatXYAction
			Dim dTotalSeconds	As Double = p_dtFile.DataCount * MEAS_TIME_RESOLUTION / 1000.0									'現在経過時間
			If dTotalSeconds = dTime1 + 120
				p_mltcp.AmplitudeX = 4000							'X軸振幅
				p_mltcp.AmplitudeY = 4000							'Y軸振幅

			ElseIf dTotalSeconds = dTime1 + dTime2 + 120
				p_mltcp.AmplitudeX = 16							'X軸振幅
				p_mltcp.AmplitudeY = 16							'Y軸振幅

			ElseIf dTotalSeconds = dTime1 + dTime2 + dtime3 + 120

				sw6.Stop
				p_iBGWOrderNext = BGW_ORDER.FLCV_MEAS_PRO_DATA                      '終了データ処理へ

			End If
		End If












#else
				'測定終了判定
				Dim dTotalSeconds	As Double = p_dtFile.DataCount * MEAS_TIME_RESOLUTION / 1000.0									'現在経過時間
				Dim dOnChkCompareSeconds As Double = p_FlcvRstTime * (p_FlcvCycleOnCnt+1) + (p_FlcvOpeTime * p_FlcvCycleOfCnt)		'OFF->ONチェック時間
				Dim dOfChkCompareSeconds As Double = p_FlcvOpeTime * (p_FlcvCycleOfCnt+1) + (p_FlcvRstTime * p_FlcvCycleOnCnt)		'ON->OFFチェック時間
				'測定時間制限判定 -----------------
				If dTotalSeconds >= (p_FlcvOpeTime + p_FlcvRstTime) * p_FlcvCycle
					'測定時間が制限超えた
					sw6.Stop
					p_iBGWOrderNext = BGW_ORDER.FLCV_MEAS_PRO_DATA                      '終了データ処理へ
				'動作ON/OFF切替え/終了確認
				ElseIf dTotalSeconds >= dOnChkCompareSeconds AndAlso p_mltcp.AmplitudeX = 0
					p_FlcvCycleOnCnt += 1
					p_mltcp.AmplitudeX = p_FlcvChgVal(0) * 1000.0		'X軸振幅
					p_mltcp.AmplitudeY = p_FlcvChgVal(0) * 1000.0		'Y軸振幅
Debug.Print("")
				ElseIf dTotalSeconds >= dOfChkCompareSeconds AndAlso p_mltcp.AmplitudeX <> 0
					p_FlcvCycleOfCnt += 1
					p_mltcp.AmplitudeX = 0							'X軸振幅
					p_mltcp.AmplitudeY = 0							'Y軸振幅
Debug.Print("")

				End If
#end if
				If p_iBGWOrderNext = BGW_ORDER.NO_ORDER
					p_iBGWOrderNext = BGW_ORDER.FLCV_MEAS_PROCESS						'継続
				End if

			Else
				p_iBGWOrderNext = BGW_ORDER.FLCV_MEAS_PROCESS							'継続
			End If
		End If
'-----------------------------------------------------------------------------------------------------------

'測定終了データ処理(粘度計算)  -------------------------------------------------------------------------------------
	Case BGW_ORDER.FLCV_MEAS_PRO_DATA
		p_ElapsedMeasTime = sw6.Elapsed.TotalSeconds
		If Not IsNothing(p_dtFile.iOxBuf)
			datCal.AveTempClear(p_mltcp.PeriodX(1), 1)			'領域クリア
			'中間値補正処理
			Dim cp As DataCal.stCalcPara = Nothing
			cp.iMeas		= eHoseiMeasMode.FlowCurve
			cp.iMode		= eHoseiCalcMode.Batch
			cp.cts			= m_cts
			cp.clDt			= p_dtFile
			cp.dPeriod		= p_mltcp.PeriodX(1)
			'cp.dAmplitude	= p_FlcvChgVal(0)
			cp.dAmplitude	= p_mltcp.AmplitudeX
			cp.iSampleStart	= 0
			cp.iSampleNum	= p_dtFile.iHosei.Length
			cp.fltSet		= p_FltSetting
			cp.dXYWait		= p_mltcp.MesPreWait(1) + p_mltcp.MeasWaitTime(1)
			cp.dZeroStress	= p_dFullStressZero
			cp.dAttenuation	= p_dFullStressAttenuation
			cp.dDiameter	= p_dDiameter
			cp.dMeasGap		= p_mltcp.GapPreset(1)
			cp.dFullStress	= p_dFullStressADVal
			cp.dPxPhase		= p_dFullStressPxPhase
			cp.dPyPhase		= p_dFullStressPyPhase
			cp.dPzPhase		= p_dFullStressPzPhase
			cp.iZeroPos		= p_iZPosition
			cp.iHPPosControl	= eHCont.Gap
			cp.dMeasureStress	= p_clSpec.spi.dMeasureStress
			cp.iCalcType		= eCalcMethod.THR
			cp.dDiffInterval	= p_clSpec.spi.dDiffInterval
			cp.iStrsSensMode	= p_FlcvActMode									'応力検知モード0,1,2
			cp.iAmpMode			= eAmpMode.AmpConst								'Z振動モード 0,1
			cp.dRCCBaseThickness = 0.0
			cp.dXYWeightCoef	= p_dNConv										'重量換算
			p_cp = cp															'計算パラメータを退避
			Select Case p_FlcvActMode
			Case 0	
				m_iCorrRet = datCal.CorrectRealProcVector( cp )
			Case 1
				m_iCorrRet = datCal.CorrectRealProcVector( cp )
			End Select
			p_dMesGap = cp.dMesGap		'測定GAP計算値 モニタ用
			p_dtFile.p_head.dRealGap = cp.dMesGap								'実測定GAPをヘッダへセット
#If 0
			'粘度関係計算
			Dim uVp As Datafile.ViscosityCalcParameter
			uVp.dPeriod			= p_FlcvPeriod									'設定周期
			uVp.dAmplitude		= p_FlcvChgVal(0) / 10^3							'設定振幅(m)
			uVp.dMeasGap		= p_mltcp.GapPreset(1) / 10^3					'測定GAP(m)
			uVp.dProbeSquare	= Math.PI * ((p_FlcvPbDiameter/10^3) / 2)^2		'プローブ面積(m2)
			uVp.dVelocityRate	= 1.0											'せん断速度補正係数

			'粘度計算処理
			Array.Resize(p_dtFile.dShearStress,		p_dtFile.iHosei.Length)
			Array.Resize(p_dtFile.dViscosity,		p_dtFile.iHosei.Length)
			Array.Resize(p_dtFile.dShearVelocity,	p_dtFile.iHosei.Length)
			For i = 0 To p_dtFile.iHosei.GetUpperBound(0)
				uVp.dMeasStress		= p_dtFile.iHosei(i) / 10^6					'測定応力(N)
				datCal.ViscosityCalc( uVp )										'粘度・せん断応力計算
				p_dtFile.dShearStress(i)	= uVp.dShearStress					'せん断応力
				p_dtFile.dViscosity(i)		= uVp.dViscosity					'粘度
				p_dtFile.dShearVelocity(i)	= uVp.dShearVelocity				'せん断速度
			Next i
#end if
			'補正データ込みで上書き更新
			p_dtFile.RealTimeMeasDataFileSaveText(eMeasMode.FlowCurve_Time, False, m_cts)
		End If
		sw1.Restart()
		p_iBGWOrderNext = BGW_ORDER.FLCV_MEAS_PRO_FINISH						'測定終了へ

'測定終了制御  -------------------------------------------------------------------------------------
	Case BGW_ORDER.FLCV_MEAS_PRO_FINISH
		p_ElapsedMeasTime = -1
		p_iZeroAdjOk = -1                                                                   'ZERO調未実施へ
		p_mltcp.ZeroSW(False)                                                               'ZERO調制御開始Bit OFF
		p_mltcp.MeasSW(False)                                                               '測定開始指令Bit OFF
		p_mltcp.StartTempCont = 0															'温度制御開始Bit OFF(HP+PB)

		If sw1.Elapsed.TotalSeconds > 1 
			sw1.Restart()
			p_iBGWOrderNext = BGW_ORDER.FLCV_MEAS_PRO_FINISH_1								'測定終了制御_1へ
		Else
			Dim iLine As Int64
			iLine = p_dtFile.GetFileLineCount()												'ファイル行数を確認
			If iLine < p_dtFile.DataCount()
				p_iBGWOrderNext = BGW_ORDER.FLCV_MEAS_PRO_FINISH							'継続
			Else
				sw1.Restart()
				p_iBGWOrderNext = BGW_ORDER.FLCV_MEAS_PRO_FINISH_1							'測定終了制御_1へ
			End If
		End If

'測定終了制御_1  -------------------------------------------------------------------------------------
	Case BGW_ORDER.FLCV_MEAS_PRO_FINISH_1
		p_MeasureState =  eMeasStat.NOT_START
		iViscoState		= eViscoState.NOT_START
		p_iBGWOrderNext = BGW_ORDER.FLCV_MEAS_PRO_FINISH_1									'継続
	End Select



Catch ex As Exception
    Call FrmDebugLog(System.Reflection.MethodBase.GetCurrentMethod.Name & ex.Message)
    Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try

End Sub


Dim dTime1 As Int32 = 0         '初期化用 高Shear保持時間
Dim dtime2 As Int32 = 0         '初期化用 回復保持時間
Dim dtime3 As Int32 = 0         '低Shear 保持時間
Dim dtime4 As Int32 = 0			'高Shear 保持時間
Dim dtime5 As Int32 = 0			'低Shear 保持時間

'時間依存 測定制御状態
Public enum eFl3State
	NOT_START	= -101
	START_SEND	= 100
	START_CONF
	START_DATA
	DATA_PRO_dTime1
	DATA_PRO_dTime2
	DATA_PRO_dTime3
	DATA_PRO_dTime4
	DATA_PRO_dTime5
	BATCH_CALC
	FINISH
End Enum

''' <summary>
''' フローカーブ（時間依存）測定制御 3STEP チキソトロピー試験 
''' </summary>
''' <param name="sw1"></param>
''' <param name="sw2"></param>
Private Sub FlowCurve3Proc(byRef sw1 As Stopwatch, byRef sw2 As Stopwatch, byRef sw3 As Stopwatch, byRef sw6 As Stopwatch)
Dim i As Int32 = 0
Try
	Select Case p_iBGWOrder
	Case BGW_ORDER.FLCV_START_BUTTON
		If p_iConnectState = CONNECT_ST.CONNECTING Then                                     'リンク中のみ
			p_mltcp.MeasSW(True)												            '測定開始指令Bit ON
			p_MeasureState = eMeasStat.PREPAREING_1							                '測定準備中
			p_iBGWOrderNext = BGW_ORDER.FLCV_START_BUTTON_CNF					            '応答確認へ
			sw1.Restart																        '応答Bit タイマースタート
			iViscoState = eFl3State.START_SEND
		End If

	Case BGW_ORDER.FLCV_START_BUTTON_CNF
		'測定SW応答確認
		If p_mltcp.isRespMeasStart() AndAlso p_mltcp.isStatMeasuringTempo()					'応答bit[1108.00]、測定中ステータス[1109.02]

			'データファイル名の決定
			p_dtFile.MesFileNumber = 0																		'ファイル連番クリア
			p_dtFile.SetDataFileName = p_dtFile.sMeasindCode(eMesCD.FL2) & "-" & Format(Now, "yyyyMMddHHmmss")			'フローカーブ（時間依存）
			MeasureHeaderInfoSet(eMeasMode.FlowCurve_Time)													'測定開始時のファイルヘッダ情報のセット

			dTime1 = p_mltcp.MesPreWait(1) + p_mltcp.MeasWaitTime(1) + p_FlcvPreshearTime	'初期化用 高Shear保持時間
			dTime2 = dTime1 + p_FlcvRecoveryTime											'初期化用 回復保持時間
			dTime3 = dTime2 + p_FlcvLow1Time												'低Shear 保持時間
			dTime4 = dTime3 + p_FlcvHighTime												'高Shear 保持時間	
			dTime5 = dTime4 + p_FlcvLow2Time												'低Shear 保持時間

			iViscoState = eFl3State.START_CONF

			'プリシア動作の設定
			'p_mltcp.AmplitudeX	= p_FlcvPreshearAmplitude * 1000.0											'X軸振幅
			'p_mltcp.AmplitudeY	= If(p_FlcvActMode=0, p_FlcvPreshearAmplitude * 1000.0, 0)			'Y軸振幅
			'p_mltcp.AmplitudeZ = 0
			'p_mltcp.PeriodX(1)	= p_FlcvPeriod												'X軸周期
			'p_mltcp.PeriodY(1)	= p_FlcvPeriod												'Y軸周期
			'p_mltcp.PeriodZ(1)	= 0

			'Thread.Sleep(1000)
			'条件のゼロストレスを取得
			Dim dAlpha As Double
			If FullStressCalibValueGet(2, p_ZClbX, p_ZClbC, p_ZClbP, p_FClbX, p_FClbC, p_FClbP, p_dFullStressZero, p_dFullStressAttenuation, _
										dAlpha, p_dFullStressPxPhase, p_dFullStressPyPhase, p_dFullStressPzPhase, p_FlcvActMode, p_FlcvPreshearAmplitude, p_FlcvPeriod ) <> 0
			End If
			'条件のフルストレスを取得
			If FullStressCalibValueGet(0, p_ZClbX, p_ZClbC, p_ZClbP, p_FClbX, p_FClbC, p_FClbP, p_dFullStressADVal, p_dFullStressAttenuation, _
										dAlpha, p_dFullStressPxPhase, p_dFullStressPyPhase, p_dFullStressPzPhase, p_FlcvActMode, p_FlcvPreshearAmplitude, p_FlcvPeriod ) <> 0
			End If
			p_MeasureState = eMeasStat.PREPAREING_1
			p_iBGWOrderNext = BGW_ORDER.FLCV_MEAS_PREPARE_1	                                '測定準備1へ
			sw3.Restart()																	'測定制限時間SW開始
		Else
			If sw1.Elapsed.TotalSeconds > cResponseTimeOut Then
				'応答BIT未確認タイムオーバー	5秒
				p_mltcp.MeasSW(False)													    '測定開始指令Bit OFF
				Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & ":Confirm Bit Response Time Over (START_BUTTON_CNF)")
				sw1.Restart()
				p_iBGWOrderNext = BGW_ORDER.FLCV_MEAS_PRO_FINISH							'測定終了へ
			Else
				p_iBGWOrderNext = BGW_ORDER.FLCV_START_BUTTON_CNF                           '応答確認継続
			End If
		End If
		'測定キャンセル確認
		If p_MeasureState < eMeasStat.RESERV
			p_mltcp.MeasSW(False)															'測定開始指令Bit OFF
			p_iBGWOrderNext = BGW_ORDER.NO_ORDER
		End If

	Case BGW_ORDER.FLCV_MEAS_PREPARE_1
		If (p_mltcp.isRespMeasStart() = False) Or (p_mltcp.isStatMeasuringTempo()=False)	'応答bit[1108.00]、測定中ステータス[1109.02]
			'本体側が測定終了した
			p_mltcp.MeasSW(False)															'測定開始指令Bit OFF
			sw1.Restart()
			p_iBGWOrderNext = BGW_ORDER.FLCV_MEAS_PRO_FINISH								'測定終了へ
		Elseif p_mltcp.isStatMeasuring() = True
		'測定中(クリア後)がON-> ゲルタイム ０秒 ここから測定データ取得開始
			m_iCorrRet = 0
			m_iStartIdx = 0
			m_bRealCorrect = False
			m_iCorrectCount = 0
			m_iPeriodSample = p_mltcp.PeriodX(1) * 1000 / MEAS_TIME_RESOLUTION			'1周期分のサンプル数計算

			sw6.Restart()
			p_ElapsedMeasTime = sw6.Elapsed.TotalSeconds
			p_MeasureState = eMeasStat.DATA_TRANS
			p_iBGWOrderNext = BGW_ORDER.FLCV_MEAS_PROCESS                               '測定データ取得制御へ
		Else
			p_iBGWOrderNext = BGW_ORDER.FLCV_MEAS_PREPARE_1                             '継続
		End If
		iViscoState = eFl3State.START_DATA
		iViscoState = eFl3State.DATA_PRO_dTime1
'測定手動終了中止制御  -------------------------------------------------------------------------------------
	Case BGW_ORDER.FLCV_CANCEL_BUTTON
		If p_MeasureState = eMeasStat.DATA_TRANS
			p_iBGWOrderNext = BGW_ORDER.FLCV_MEAS_PRO_DATA									'終了データ処理へ
		Else
			p_iBGWOrderNext = BGW_ORDER.FLCV_MEAS_PRO_FINISH								'測定終了へ
			sw1.Restart()
		End If
'-----------------------------------------------------------------------------------------------------------
'測定データ取得制御  -------------------------------------------------------------------------------------

	Case BGW_ORDER.FLCV_MEAS_PROCESS
		'測定終了監視
		If (p_mltcp.isRespMeasStart() = False) Or (p_mltcp.isStatMeasuringTempo() = False)  '応答bit[1108.00]、測定中ステータス[1109.02]
			'本体側が測定終了した
			p_iBGWOrderNext = BGW_ORDER.FLCV_MEAS_PRO_DATA                                  '測定したところまでで測定終了しデータ処理
		ElseIf p_mltcp.isItLock()
			'扉オープンされた
			p_iBGWOrderNext = BGW_ORDER.FLCV_MEAS_PRO_DATA                                  '測定したところまでで測定終了しデータ処理
		Else
			p_ElapsedMeasTime = sw6.Elapsed.TotalSeconds
			'測定実施中
			If p_mltcp.bMesDataEnable Then
			'測定データ有効
				'測定データ蓄積セーブ -------------------
				p_dtFile.RealTimeMeasDataFileSaveText(eMeasMode.FlowCurve_Time, True, m_cts)               '測定値累積保存 追加モード


			'リアルタイム中間値補正処理 ----------------------
				If p_dtFile.DataCount-m_iStartIdx >= m_iPeriodSample 
				'1周期分のデータ取得まで待つ
					if m_iCorrectCount = 0
					'初回は0インデックスから
						m_iStartIdx = 0
						datCal.AveTempClear(p_mltcp.PeriodX(1), 0)			'領域クリア
					Else
					End If
					'リアルタイム中間値補正
					'Z決定位置をセット
					If p_mltcp.ZConfirmPos<>0
						p_dtFile.p_head.dZAfterPos = Ctype(p_mltcp.ZConfirmPos, Double) / 10.0
					End If
					Dim cp As DataCal.stCalcPara = Nothing
					cp.iMeas		= eHoseiMeasMode.FlowCurve
					cp.iMode		= eHoseiCalcMode.RealTime
					cp.cts			= m_cts
					cp.clDt			= p_dtFile
					cp.dPeriod		= p_mltcp.PeriodX(p_FlcvThinning)
					cp.dAmplitude	= p_mltcp.AmplitudeX/1000
					cp.iSampleStart	= m_iStartIdx
					cp.iSampleNum	= p_dtFile.iHosei.Length - m_iStartIdx
					cp.fltSet		= p_FltSetting
					cp.dXYWait		= p_mltcp.MesPreWait(1) + p_mltcp.MeasWaitTime(1)
					cp.dZeroStress	= p_dFullStressZero
					cp.dAttenuation	= p_dFullStressAttenuation
					cp.dDiameter	= p_dDiameter
					cp.dMeasGap		= p_mltcp.GapPreset(1)
					cp.dFullStress	= p_dFullStressADVal
					cp.dPxPhase		= p_dFullStressPxPhase
					cp.dPyPhase		= p_dFullStressPyPhase
					cp.dPzPhase		= p_dFullStressPzPhase
					cp.iZeroPos		= p_iZPosition
					cp.iHPPosControl	= eHCont.Gap
					cp.dMeasureStress	= p_clSpec.spi.dMeasureStress
					cp.iCalcType		= eCalcMethod.THR
					cp.dDiffInterval	= p_clSpec.spi.dDiffInterval
					cp.iStrsSensMode	= p_FlcvActMode									'応力検知モード0,1,2
					cp.iAmpMode			= eAmpMode.AmpConst								'Z振動モード 0,1
					cp.dRCCBaseThickness = 0.0
					cp.dXYWeightCoef	= p_dNConv										'重量換算
					p_cp = cp															'計算パラメータを退避
					Select Case p_FlcvActMode
				    Case 0	'円モード
						m_iCorrRet = datCal.CorrectRealProcVector( cp )
				    Case 1	'Xモード
						m_iCorrRet = datCal.CorrectRealProcVector( cp )
					End Select
					p_dMesGap = cp.dMesGap		'測定GAP計算値 モニタ用
					p_dtFile.p_head.dRealGap = cp.dMesGap								'実測定GAPをヘッダへセット

					p_FlcvArrayUpdate = 0

					m_iStartIdx += p_dtFile.iHosei.Length - m_iStartIdx					'開始インデックスの更新
					m_iCorrectCount += 1
				End If
			'--------------------------------------------------

				p_dtFile.MesBlockCnt += 1                                               '受信ブロックカウンタ更新
				p_mltcp.ZeroSW(False)                                                   'ZERO調制御開始Bit OFF（測定中ゼロ調指令はOFFにする）


			' 条件更新タイミングでの振幅変更処理 1秒以内にスキャン前提----------------------
				Dim dTotalSeconds	As Double = p_dtFile.DataCount * MEAS_TIME_RESOLUTION / 1000.0      '現在経過時間 サンプル数で換算
				Select Case dTotalSeconds
				Case dTime1				'初期化用 高Shear保持時間経過->回復へ
					'条件のゼロストレスを取得
					Dim dAlpha As Double
					FullStressCalibValueGet(2, p_ZClbX, p_ZClbC, p_ZClbP, p_FClbX, p_FClbC, p_FClbP, p_dFullStressZero, p_dFullStressAttenuation, _
												dAlpha, p_dFullStressPxPhase, p_dFullStressPyPhase, p_dFullStressPzPhase, p_FlcvActMode, p_FlcvRecoveryAmplitude, p_FlcvPeriod )
					'条件のフルストレスを取得
					FullStressCalibValueGet(0, p_ZClbX, p_ZClbC, p_ZClbP, p_FClbX, p_FClbC, p_FClbP, p_dFullStressADVal, p_dFullStressAttenuation, _
												dAlpha, p_dFullStressPxPhase, p_dFullStressPyPhase, p_dFullStressPzPhase, p_FlcvActMode, p_FlcvRecoveryAmplitude, p_FlcvPeriod )
					p_mltcp.AmplitudeX = p_FlcvRecoveryAmplitude*1000									'X軸振幅
					p_mltcp.AmplitudeY = If(p_FlcvActMode=0, p_FlcvRecoveryAmplitude * 1000.0, 0)		'Y軸振幅
					iViscoState = eFl3State.DATA_PRO_dTime2
				Case dtime2				'初期化用 回復保持時間経過->低Shearへ
					'条件のゼロストレスを取得
					Dim dAlpha As Double
					FullStressCalibValueGet(2, p_ZClbX, p_ZClbC, p_ZClbP, p_FClbX, p_FClbC, p_FClbP, p_dFullStressZero, p_dFullStressAttenuation, _
												dAlpha, p_dFullStressPxPhase, p_dFullStressPyPhase, p_dFullStressPzPhase, p_FlcvActMode, p_FlcvLow1Amplitude, p_FlcvPeriod )
					'条件のフルストレスを取得
					FullStressCalibValueGet(0, p_ZClbX, p_ZClbC, p_ZClbP, p_FClbX, p_FClbC, p_FClbP, p_dFullStressADVal, p_dFullStressAttenuation, _
												dAlpha, p_dFullStressPxPhase, p_dFullStressPyPhase, p_dFullStressPzPhase, p_FlcvActMode, p_FlcvLow1Amplitude, p_FlcvPeriod )
					p_mltcp.AmplitudeX = p_FlcvLow1Amplitude*1000											'X軸振幅	
					p_mltcp.AmplitudeY = If(p_FlcvActMode=0, p_FlcvLow1Amplitude * 1000.0, 0)			'Y軸振幅
					iViscoState = eFl3State.DATA_PRO_dTime3
				Case dtime3				'初期化用 低Shear保持時間経過->高Shearへ
					'条件のゼロストレスを取得
					Dim dAlpha As Double
					FullStressCalibValueGet(2, p_ZClbX, p_ZClbC, p_ZClbP, p_FClbX, p_FClbC, p_FClbP, p_dFullStressZero, p_dFullStressAttenuation, _
												dAlpha, p_dFullStressPxPhase, p_dFullStressPyPhase, p_dFullStressPzPhase, p_FlcvActMode, p_FlcvHighAmplitude, p_FlcvPeriod )
					'条件のフルストレスを取得
					FullStressCalibValueGet(0, p_ZClbX, p_ZClbC, p_ZClbP, p_FClbX, p_FClbC, p_FClbP, p_dFullStressADVal, p_dFullStressAttenuation, _
												dAlpha, p_dFullStressPxPhase, p_dFullStressPyPhase, p_dFullStressPzPhase, p_FlcvActMode, p_FlcvHighAmplitude, p_FlcvPeriod )
					p_mltcp.AmplitudeX = p_FlcvHighAmplitude*1000											'X軸振幅	
					p_mltcp.AmplitudeY = If(p_FlcvActMode=0, p_FlcvHighAmplitude * 1000.0, 0)			'Y軸振幅
					iViscoState = eFl3State.DATA_PRO_dTime4
				Case dtime4				'初期化用 高Shear保持時間経過->低Shearへ
					'条件のゼロストレスを取得
					Dim dAlpha As Double
					FullStressCalibValueGet(2, p_ZClbX, p_ZClbC, p_ZClbP, p_FClbX, p_FClbC, p_FClbP, p_dFullStressZero, p_dFullStressAttenuation, _
												dAlpha, p_dFullStressPxPhase, p_dFullStressPyPhase, p_dFullStressPzPhase, p_FlcvActMode, p_FlcvLow2Amplitude, p_FlcvPeriod )
					'条件のフルストレスを取得
					FullStressCalibValueGet(0, p_ZClbX, p_ZClbC, p_ZClbP, p_FClbX, p_FClbC, p_FClbP, p_dFullStressADVal, p_dFullStressAttenuation, _
												dAlpha, p_dFullStressPxPhase, p_dFullStressPyPhase, p_dFullStressPzPhase, p_FlcvActMode, p_FlcvLow2Amplitude, p_FlcvPeriod )
					p_mltcp.AmplitudeX = p_FlcvLow2Amplitude*1000											'X軸振幅	
					p_mltcp.AmplitudeY = If(p_FlcvActMode=0, p_FlcvLow2Amplitude * 1000.0, 0)			'Y軸振幅
					iViscoState = eFl3State.DATA_PRO_dTime5
				Case dtime5				'初期化用 低Shear保持時間経過->終了へ
					p_mltcp.AmplitudeX = 0																'X軸振幅	
					p_mltcp.AmplitudeY = 0																'Y軸振幅
					sw6.Stop()
					p_iBGWOrderNext = BGW_ORDER.FLCV_MEAS_PRO_DATA                      '終了データ処理へ
					iViscoState = eFl3State.BATCH_CALC
				End Select

				If p_iBGWOrderNext = BGW_ORDER.NO_ORDER
					p_iBGWOrderNext = BGW_ORDER.FLCV_MEAS_PROCESS						'継続
				End if

			Else
				p_iBGWOrderNext = BGW_ORDER.FLCV_MEAS_PROCESS							'継続
			End If
		End If
'-----------------------------------------------------------------------------------------------------------

'測定終了データ処理(粘度計算)  -------------------------------------------------------------------------------------
	Case BGW_ORDER.FLCV_MEAS_PRO_DATA
		iViscoState = eFl3State.BATCH_CALC
		p_ElapsedMeasTime = sw6.Elapsed.TotalSeconds
#If 0
		If Not IsNothing(p_dtFile.iOxBuf)
			datCal.AveTempClear(p_mltcp.PeriodX(1), 1)			'領域クリア
			'中間値補正処理
			Dim cp As DataCal.stCalcPara = Nothing
			cp.iMeas		= eHoseiMeasMode.FlowCurve
			cp.iMode		= eHoseiCalcMode.Batch
			cp.cts			= m_cts
			cp.clDt			= p_dtFile
			cp.dPeriod		= p_mltcp.PeriodX(1)
			cp.dAmplitude	= p_mltcp.AmplitudeX
			cp.iSampleStart	= 0
			cp.iSampleNum	= p_dtFile.iHosei.Length
			cp.fltSet		= p_FltSetting
			cp.dXYWait		= p_mltcp.MesPreWait(1) + p_mltcp.MeasWaitTime(1)
			cp.dZeroStress	= p_dFullStressZero
			cp.dAttenuation	= p_dFullStressAttenuation
			cp.dDiameter	= p_dDiameter
			cp.dMeasGap		= p_mltcp.GapPreset(1)
			cp.dFullStress	= p_dFullStressADVal
			cp.dPxPhase		= p_dFullStressPxPhase
			cp.dPyPhase		= p_dFullStressPyPhase
			cp.dPzPhase		= p_dFullStressPzPhase
			cp.iZeroPos		= p_iZPosition
			cp.iHPPosControl	= eHCont.Gap
			cp.dMeasureStress	= p_clSpec.spi.dMeasureStress
			cp.iCalcType		= eCalcMethod.THR
			cp.dDiffInterval	= p_clSpec.spi.dDiffInterval
			cp.iStrsSensMode	= p_FlcvActMode									'応力検知モード0,1,2
			cp.iAmpMode			= eAmpMode.AmpConst								'Z振動モード 0,1
			cp.dRCCBaseThickness = 0.0
			cp.dXYWeightCoef	= p_dNConv										'重量換算
			p_cp = cp															'計算パラメータを退避
			Select Case p_FlcvActMode
			Case 0	
				m_iCorrRet = datCal.CorrectRealProcVector( cp )
			Case 1
				m_iCorrRet = datCal.CorrectRealProcVector( cp )
			End Select
			p_dMesGap = cp.dMesGap		'測定GAP計算値 モニタ用
			p_dtFile.p_head.dRealGap = cp.dMesGap								'実測定GAPをヘッダへセット

			'補正データ込みで上書き更新
			p_dtFile.RealTimeMeasDataFileSaveText(eMeasMode.FlowCurve_Time, False, m_cts)
		End If
#else
			p_dtFile.p_head.dRealGap		= p_mltcp.GapPreset(1)				'実測定GAPをヘッダへセット(設定値)
			p_dtFile.p_head.iMeasActTime	= p_ElapsedMeasTime					'実測定時間(秒)
			p_dtFile.p_head.dZAfterPos		= p_mltcp.ZConfirmPos / 10.0		'実測定Z位置(um)
			'補正データ込みで上書き更新
			p_dtFile.RealTimeMeasDataFileSaveText(eMeasMode.FlowCurve_Time, False, m_cts)
#end if
		sw1.Restart()
		p_iBGWOrderNext = BGW_ORDER.FLCV_MEAS_PRO_FINISH						'測定終了へ

'測定終了制御  -------------------------------------------------------------------------------------
	Case BGW_ORDER.FLCV_MEAS_PRO_FINISH
		iViscoState = eFl3State.FINISH
		p_ElapsedMeasTime = -1
		p_iZeroAdjOk = -1                                                                   'ZERO調未実施へ
		p_mltcp.ZeroSW(False)                                                               'ZERO調制御開始Bit OFF
		p_mltcp.MeasSW(False)                                                               '測定開始指令Bit OFF
		p_mltcp.StartTempCont = 0															'温度制御開始Bit OFF(HP+PB)

		If sw1.Elapsed.TotalSeconds > 1 
			sw1.Restart()
			p_iBGWOrderNext = BGW_ORDER.FLCV_MEAS_PRO_FINISH_1								'測定終了制御_1へ
		Else
			Dim iLine As Int64
			iLine = p_dtFile.GetFileLineCount()												'ファイル行数を確認
			If iLine < p_dtFile.DataCount()
				p_iBGWOrderNext = BGW_ORDER.FLCV_MEAS_PRO_FINISH							'継続
			Else
				sw1.Restart()
				p_iBGWOrderNext = BGW_ORDER.FLCV_MEAS_PRO_FINISH_1							'測定終了制御_1へ
			End If
		End If

'測定終了制御_1  -------------------------------------------------------------------------------------
	Case BGW_ORDER.FLCV_MEAS_PRO_FINISH_1
		p_MeasureState =  eMeasStat.NOT_START
		iViscoState		= eViscoState.NOT_START
		p_iBGWOrderNext = BGW_ORDER.FLCV_MEAS_PRO_FINISH_1									'継続
	End Select



Catch ex As Exception
    Call FrmDebugLog(System.Reflection.MethodBase.GetCurrentMethod.Name & ex.Message)
    Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try

End Sub





''' <summary>
''' ゼロ調整制御
''' </summary>
''' <param name="sw1"></param>
''' <param name="sw5"></param>
Private Sub ZeroAdjustmentProc(byRef sw1 As Stopwatch, byRef sw5 As Stopwatch)
Dim i As int32
Try
	Select Case p_iBGWOrder
	Case BGW_ORDER.ZERO_BUTTON
		If p_iConnectState = CONNECT_ST.CONNECTING Then                                     'リンク中のみ
			If p_mltcp.IsZeroStartSW()
				p_mltcp.ZeroSW(False)														'ZERO調制御開始Bit OFF
				p_iZeroAdjOk = -1                                                           'ゼロ調正未完了
				p_CaliblationState.iState = eCalibState.NOT_START                           '較正用ステータス->未実行
			Else
				p_mltcp.ZeroSW(True)														'ZERO調制御開始Bit ON
				p_iBGWOrderNext = BGW_ORDER.ZERO_BUTTON_CNF                                 '応答確認へ
				p_iZeroAdjOk = 0
				iViscoState = eViscoState.ZERO_EXEC											'フローカーブ用
'				p_CaliblationState.iState = eCalibState.ZERO_EXEC							'較正用ステータス->ゼロ調実行中
				sw1.Restart                                                                 '応答Bit タイマースタート
			End If
		End If
	Case BGW_ORDER.ZERO_BUTTON_CNF
		'ZERO調SW応答確認
		If p_mltcp.isRespZeroStart() 
			p_iBGWOrderNext = BGW_ORDER.ZERO_BUTTON_N                                       'ゼロ点位置のチェックへ
			iPreZPos = p_mltcp.HeatBPos                                                     '前回値退避
			sw1.Restart                                                                     '監視タイマースタート
			sw5.Restart                                                                     '監視タイマースタート
		Else
			If sw1.Elapsed.TotalSeconds > cResponseTimeOut Then
				'応答BIT未確認タイムオーバー	5秒
				p_mltcp.ZeroSW(False)														'ZERO調制御開始Bit OFF
				p_CaliblationState.iState = eCalibState.NOT_START							'較正用ステータス->未実行
				Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & ":Confirm Bit Response Time Over (ZERO_BUTTON_CNF)")
				If p_iZeroAdjOk <> -10
					p_iZeroAdjOk ＝ -9
					p_iBGWOrderNext = BGW_ORDER.ZERO_BUTTON_CNF								'継続
				End If
			Else
				p_iBGWOrderNext = BGW_ORDER.ZERO_BUTTON_CNF                                 '応答確認継続
			End If
		End If

	Case BGW_ORDER.ZERO_BUTTON_N
		If sw1.Elapsed.TotalSeconds < 5 Then
			'ゼロ点位置ゼロを確認
			If p_mltcp.ZeroPos_Z = 0
				sw1.Restart                                                                     '監視タイマースタート
				p_iBGWOrderNext = BGW_ORDER.ZERO_BUTTON_0										'監視バッファ準備へ
			Else
				p_iBGWOrderNext = BGW_ORDER.ZERO_BUTTON_N										'継続
			End If
		Else
		'ゼロ点位置クリアタイムオーバー
			p_mltcp.ZeroSW(False)														'ZERO調制御開始Bit OFF
			p_CaliblationState.iState = eCalibState.NOT_START							'較正用ステータス->未実行
			Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & ":Zero Point Clear Time Over (ZERO_BUTTON_N)")
			If p_iZeroAdjOk <> -10
				p_iZeroAdjOk ＝ -9
				p_iBGWOrderNext = BGW_ORDER.ZERO_BUTTON_N										'継続
			End If
		End If

	Case BGW_ORDER.ZERO_BUTTON_0
		If sw1.Elapsed.TotalSeconds >= 1 Then
			sw5.Restart                                                                     '監視タイマースタート
			p_ZeroBufIdx = 0																'リングバッファインデックス準備
			Array.Resize(p_ZeroAdjBuf, p_iStaMonTime)										'リングバッファ準備
			FillDoubleArr(p_ZeroAdjBuf, Double.NaN)											'NaNで埋める
			p_iBGWOrderNext = BGW_ORDER.ZERO_BUTTON_1										'チェック開始へ
		Else
			p_iBGWOrderNext = BGW_ORDER.ZERO_BUTTON_0										'継続
		End If

	Case BGW_ORDER.ZERO_BUTTON_1
		If p_iError = 1
			p_mltcp.ZeroSW(False)														'ZERO調制御開始Bit OFF
			p_CaliblationState.iState = eCalibState.NOT_START							'較正用ステータス->未実行
		ElseIf sw5.Elapsed.TotalMilliseconds >= 950
		'1秒毎にバッファリング
			p_ZeroAdjBuf(p_ZeroBufIdx) = p_mltcp.HeatBPos
			'安定監視時間を超えた
			Dim dAve As Double = 0
			Dim dAveC As Double = 0
			For i=0 To  p_ZeroAdjBuf.GetUpperBound(0)
				If Not Double.IsNaN(p_ZeroAdjBuf(i))
					dAve += p_ZeroAdjBuf(i)
					dAveC += 1
				End If
			Next
			dAve = dAve / dAveC
			Dim dUpper As Double = dAve + p_iStaFlucRange / 2
			Dim dLower As Double = dAve - p_iStaFlucRange / 2
			'配列内チェック
			For i = 0 to p_ZeroAdjBuf.GetUpperBound(0)
				If Not Double.IsNaN(p_ZeroAdjBuf(i))
					if p_ZeroAdjBuf(i) > dUpper
						Exit For
					End If
					if p_ZeroAdjBuf(i) < dLower
						Exit For
					End If
				Else
					Exit for
				End If
			Next i
			''If i > p_ZeroAdjBuf.GetUpperBound(0) And p_mltcp.ZeroStrs_Z<>0
			If i > p_ZeroAdjBuf.GetUpperBound(0) AndAlso p_mltcp.ZeroPos_Z <> 0
			'安定範囲に入っている かつ ゼロ点位置確定しているので ゼロ調完了
				
				p_iZeroAdjOk = 1                                                            'ゼロ調正完了
				If p_CaliblationState.iState = eCalibState.ZERO_EXEC
					p_CaliblationState.iState = eCalibState.ZERO_COMP						'較正用ステータス->ゼロ調完了
				End if
				iViscoState = eViscoState.ZERO_COMP											'フローカーブ用
				p_iBGWOrderNext = BGW_ORDER.ZERO_BUTTON_2									'継続
				sw1.Restart

				'Z位置補正データ作成時、温度プロファイル開始
				If p_cts.p_ZCalibTempPro = 1
					If p_RTM(0).p_strRTGSetName<>""
						p_RTM(0).sbLoadRTGSetting(p_RTM(0).p_strRTGSetName, p_bZCalibEnable)		'温度プロファイル開始
					End If
				End If
			Else
			'安定範囲に入っていない
				p_ZeroBufIdx += 1															'リングバッファインデックスを+1
				If p_ZeroBufIdx > p_ZeroAdjBuf.GetUpperBound(0)
					p_ZeroBufIdx = 0
				End If
				sw5.Restart
				p_iBGWOrderNext = BGW_ORDER.ZERO_BUTTON_1									'継続
			End If
		Else
			p_iBGWOrderNext = BGW_ORDER.ZERO_BUTTON_1									'継続
		End If

	Case BGW_ORDER.ZERO_BUTTON_2
		'ゼロ調完了 表示更新待ち
		If sw1.Elapsed.TotalSeconds<1
			p_iBGWOrderNext = BGW_ORDER.ZERO_BUTTON_2									'継続
		End If
	Case Else
	End Select
Catch ex As Exception
    Call FrmDebugLog(System.Reflection.MethodBase.GetCurrentMethod.Name & ex.Message)
    Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub
''' <summary>
''' 水平確認制御
''' </summary>
''' <param name="sw1"></param>
''' <param name="sw7"></param>
Private Sub HorizontalConfirmProc(byRef sw1 As Stopwatch, byRef sw7 As Stopwatch)
Dim i As int32
Try
	Select Case p_iBGWOrder
	Case BGW_ORDER.HORIZON_BUTTON_START
		If p_iConnectState = CONNECT_ST.CONNECTING Then                                     'リンク中のみ
			If p_mltcp.IsZeroStartSW()
				p_mltcp.ZeroSW(False)														'ZERO調制御開始Bit OFF
				p_CaliblationState.iState = eCalibState.NOT_START							'較正用ステータス->未実行
				p_iHorizonConfOperation  = -1
			Else
				p_mltcp.ZeroSW(True)														'ZERO調制御開始Bit ON
				p_iBGWOrderNext = BGW_ORDER.HORIZON_BUTTON_CNF                              '応答確認へ
				sw1.Restart                                                                 '応答Bit タイマースタート
			End If
		End If
	Case BGW_ORDER.HORIZON_BUTTON_CNF
		'ZERO調SW応答確認
		If p_mltcp.isRespZeroStart() 
			p_HrznIdx = -1
			iPreZPos = p_mltcp.HeatBPos
			sw1.Restart                                                                     '監視タイマースタート
			sw7.Restart                                                                     '監視タイマースタート
			p_iBGWOrderNext = BGW_ORDER.HORIZON_BUTTON_0                                    'シーケンス監視へ
		Else
			If sw1.Elapsed.TotalSeconds > cResponseTimeOut Then
				'応答BIT未確認タイムオーバー	5秒
				p_mltcp.ZeroSW(False)														'ZERO調制御開始Bit OFF
				p_CaliblationState.iState = eCalibState.NOT_START							'較正用ステータス->未実行
				Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & ":ZeroSw Confirm Bit Response Time Over (HORIZON_BUTTON_CNF)")
			Else
				p_iBGWOrderNext = BGW_ORDER.HORIZON_BUTTON_CNF                              '応答確認継続
			End If
		End If

	Case BGW_ORDER.HORIZON_BUTTON_0
	'監視待機 経過
		''If sw1.Elapsed.TotalSeconds > 1 And p_mltcp.isStatXYAction() Then
		If sw1.Elapsed.TotalSeconds >= 1
			sw1.Restart                                                                     '監視タイマースタート
			p_ZeroBufIdx = 0																'リングバッファインデックス準備
			Array.Resize(p_ZeroAdjBuf, p_iStaMonTime)										'リングバッファ準備
			FillDoubleArr(p_ZeroAdjBuf, Double.NaN)											'NaNで埋める
			p_iBGWOrderNext = BGW_ORDER.HORIZON_BUTTON_1                                    'シーケンス監視へ
		Else
			p_iBGWOrderNext = BGW_ORDER.HORIZON_BUTTON_0									'応答確認継続
		End If

#If 0
	Case BGW_ORDER.HORIZON_BUTTON_1
		'監視指定時間内に指定量の変動がないことを確認して終了
		If Math.Abs(iPreZPos - p_mltcp.HeatBPos) >= p_iStaFlucRange Then
			sw1.Restart
			sw7.Restart
			iPreZPos = p_mltcp.HeatBPos
			p_iBGWOrderNext = BGW_ORDER.HORIZON_BUTTON_0	                                'Z位置変化待ちへ
		Else
			If sw7.Elapsed.TotalSeconds >= p_iStaMonTime Then
			'ZERO調終了
				if p_HrznIdx = -1					
					'初回は原点位置
					For i=0 To p_Horizon.GetUpperBound(0)
						if p_Horizon(i).iXPos = 0 And p_Horizon(i).iYPos = 0
							p_Horizon(i).iZPos = p_mltcp.HeatBPos.ToString
							p_Horizon(i).iDiff = 0
							m_HorizonZPos = p_mltcp.HeatBPos								'XY原点のZ位置を記憶
							Exit for
						End If
					Next
					p_HrznIdx = 0
				Else
					If p_Horizon(p_HrznIdx).iXPos =0 And p_Horizon(p_HrznIdx).iYPos = 0
					'原点はスキップ
					Else
						p_Horizon(p_HrznIdx).iZPos = p_mltcp.HeatBPos.ToString				'測定結果
						p_Horizon(p_HrznIdx).iDiff = p_mltcp.HeatBPos - m_HorizonZPos
					End If
				End If
				p_iBGWOrderNext = BGW_ORDER.HORIZON_BUTTON_2								'位置移動へ
			Else
				sw1.Restart()
				p_iBGWOrderNext = BGW_ORDER.HORIZON_BUTTON_0                                'Z位置変化待ちへ
			End If
		End If
#else

	Case BGW_ORDER.HORIZON_BUTTON_1
		If sw1.Elapsed.TotalMilliseconds >= 950
		'1秒毎にバッファリング
			p_ZeroAdjBuf(p_ZeroBufIdx) = p_mltcp.HeatBPos
			'安定監視時間を超えた
			Dim dAve As Double = 0
			Dim dAveC As Double = 0
			For i=0 To  p_ZeroAdjBuf.GetUpperBound(0)
				If Not Double.IsNaN(p_ZeroAdjBuf(i))
					dAve += p_ZeroAdjBuf(i)
					dAveC += 1
				End If
			Next
			dAve = dAve / dAveC
			Dim dUpper As Double = dAve + p_iStaFlucRange / 2
			Dim dLower As Double = dAve - p_iStaFlucRange / 2
			'配列内チェック
			For i = 0 to p_ZeroAdjBuf.GetUpperBound(0)
				If Not Double.IsNaN(p_ZeroAdjBuf(i))
					if p_ZeroAdjBuf(i) > dUpper
						Exit For
					End If
					if p_ZeroAdjBuf(i) < dLower
						Exit For
					End If
				Else
					Exit for
				End If
			Next i
			''If i > p_ZeroAdjBuf.GetUpperBound(0) And p_mltcp.ZeroStrs_Z<>0
			If i > p_ZeroAdjBuf.GetUpperBound(0)
			'安定範囲に入っているのでゼロ調完了
				
				'p_iZeroAdjOk = 1                                                            'ゼロ調正完了
				p_mltcp.ZeroSW(False)														'ZERO調制御開始Bit OFF
				sw1.Restart

				p_iBGWOrderNext = BGW_ORDER.HORIZON_BUTTON_1_1								'位置移動へ

			Else
			'安定範囲に入っていない
				p_ZeroBufIdx += 1															'リングバッファインデックスを+1
				If p_ZeroBufIdx > p_ZeroAdjBuf.GetUpperBound(0)
					p_ZeroBufIdx = 0
				End If
				sw1.Restart
				p_iBGWOrderNext = BGW_ORDER.HORIZON_BUTTON_1									'継続
			End If
		Else
			p_iBGWOrderNext = BGW_ORDER.HORIZON_BUTTON_1									'継続
		End If
#end If


	Case BGW_ORDER.HORIZON_BUTTON_1_1
	'ゼロ調終了待ち、データ格納
'		If (Not p_mltcp.isStatZeroAdjusting() And  Not p_mltcp.isStatGoingUpDown()) Or sw1.Elapsed.TotalMilliseconds >= 2000
		If (Not p_mltcp.isStatZeroAdjusting() And  Not p_mltcp.isStatGoingUpDown()) 
			if p_HrznIdx = -1					
				'初回は原点位置
				For i=0 To p_Horizon.GetUpperBound(0)
					if p_Horizon(i).iXPos = 0 And p_Horizon(i).iYPos = 0
						p_Horizon(i).iZPos = p_mltcp.ZeroPos_Z.ToString					'ゼロ点記録(ADR1183-1184)
						p_Horizon(i).iDiff = 0
						m_HorizonZPos = p_mltcp.ZeroPos_Z								'XY原点(中心)のZ位置を記憶
						Exit for
					End If
				Next
				p_HrznIdx = 0
			Else
				If p_Horizon(p_HrznIdx).iXPos =0 And p_Horizon(p_HrznIdx).iYPos = 0
				'原点はスキップ
				Else
					p_Horizon(p_HrznIdx).iZPos = p_mltcp.ZeroPos_Z.ToString				'測定結果
					p_Horizon(p_HrznIdx).iDiff = p_mltcp.ZeroPos_Z - m_HorizonZPos		'中心点との差分を記録
				End If
				p_HrznIdx += 1															'次の位置インデックス
			End If
			p_iBGWOrderNext = BGW_ORDER.HORIZON_BUTTON_2									'位置移動へ
		Else
			p_iBGWOrderNext = BGW_ORDER.HORIZON_BUTTON_1_1									'継続
		End If

	Case BGW_ORDER.HORIZON_BUTTON_2
		'p_HrznIdx += 1															'次の位置インデックス
		'位置移動
		If p_HrznIdx > p_Horizon.GetUpperBound(0)
		'水平確認終了
			p_HrznIdx = -1															'最後の位置インデックス
		Else
		'選択されているか確認
			If p_Horizon(p_HrznIdx).bMeasEn 
			Else
			'選択されていない場合は次のインデックスへ進める
				p_HrznIdx += 1														'次の位置インデックス
				'終了確認
				'最後のインデックスが有効かを確認
				If p_HrznIdx >= p_Horizon.GetUpperBound(0)
					If p_HrznIdx <= p_Horizon.GetUpperBound(0)
						If Not p_Horizon(p_HrznIdx).bMeasEn							'最後のインデックスが有効か
							p_HrznIdx = -1											'最後の位置インデックス
						End If
					End If
				End If
			End If
		End if
		if	p_HrznIdx <> -1																'最後の位置インデックス
			If p_HrznIdx <= p_Horizon.GetUpperBound(0)
				If p_Horizon(p_HrznIdx).bMeasEn
					p_mltcp.ZeroAdjPosX = p_Horizon(p_HrznIdx).iXPos							'X軸位置
					p_mltcp.ZeroAdjPosY = p_Horizon(p_HrznIdx).iYPos							'Y軸位置
					sw1.Restart
					p_iBGWOrderNext = BGW_ORDER.HORIZON_BUTTON_3
				ElseIf p_HrznIdx >= p_Horizon.GetUpperBound(0)
					p_iBGWOrderNext = BGW_ORDER.HORIZON_BUTTON_3
				Else
					p_iBGWOrderNext = BGW_ORDER.HORIZON_BUTTON_2									'位置移動へ
				End If
			Else
				p_HrznIdx = -1													'最後の位置インデックス
			'原点へ戻す
			p_mltcp.ZeroAdjPosX = 0
			p_mltcp.ZeroAdjPosY = 0
			p_iBGWOrderNext = BGW_ORDER.HORIZON_BUTTON_STOP
			End If
		Else
			sw1.Restart
			'原点へ戻す
			p_mltcp.ZeroAdjPosX = 0
			p_mltcp.ZeroAdjPosY = 0
			p_iBGWOrderNext = BGW_ORDER.HORIZON_BUTTON_STOP
		End If

	Case BGW_ORDER.HORIZON_BUTTON_3
		'次の測定設定
		If sw1.Elapsed.TotalSeconds > 3 Then
			iPreZPos = p_mltcp.HeatBPos                                             '前回値退避
#If 0
			p_mltcp.ZeroSW(True)													'ZERO調制御開始Bit ON
			sw1.Restart                                                             '監視タイマースタート
			sw7.Restart                                                             '監視タイマースタート
			p_iBGWOrderNext = BGW_ORDER.HORIZON_BUTTON_1							'熱板位置安定監視へ
#end If
			p_iHorizonConfOperation = 0
			p_iBGWOrderNext = BGW_ORDER.HORIZON_BUTTON_4							'操作待ちへ
		Else
			p_iBGWOrderNext = BGW_ORDER.HORIZON_BUTTON_3
		End If

	Case BGW_ORDER.HORIZON_BUTTON_4
		'１ブロック終了 操作待ち
		Select Case p_iHorizonConfOperation
		Case 0		'操作待ち
			p_iBGWOrderNext = BGW_ORDER.HORIZON_BUTTON_4							'継続
		Case 1		'測定へGo
			p_mltcp.ZeroSW(True)													'ZERO調制御開始Bit ON
			sw1.Restart                                                             '監視タイマースタート
			sw7.Restart                                                             '監視タイマースタート
			p_iBGWOrderNext = BGW_ORDER.HORIZON_BUTTON_1							'熱板位置安定監視へ
		Case 2		'測定終了
			sw1.Restart
			'原点へ戻す
			p_mltcp.ZeroAdjPosX = 0
			p_mltcp.ZeroAdjPosY = 0
			p_iBGWOrderNext = BGW_ORDER.HORIZON_BUTTON_STOP
		End Select

	Case BGW_ORDER.HORIZON_BUTTON_STOP
		'終了・中止処理
		If sw1.Elapsed.TotalSeconds > 1
			p_mltcp.ZeroSW(False)													'ZERO調制御開始Bit OFF
			sw1.Restart                                                             '監視タイマースタート
			p_iBGWOrderNext = BGW_ORDER.HORIZON_BUTTON_STOP_1
		Else
			p_iBGWOrderNext = BGW_ORDER.HORIZON_BUTTON_STOP
		End if
	Case BGW_ORDER.HORIZON_BUTTON_STOP_1
		If sw1.Elapsed.TotalSeconds > 1
		Else
			p_iBGWOrderNext = BGW_ORDER.HORIZON_BUTTON_STOP_1
		End If
	End Select

Catch ex As Exception
    Call FrmDebugLog(System.Reflection.MethodBase.GetCurrentMethod.Name & ex.Message)
    Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub
''' <summary>
''' 中心探索制御,手動登録(隠し)
''' </summary>
''' <param name="sw1"></param>
Private Sub CenterSearchProc(byRef sw1 As Stopwatch)
Try
	Select Case p_iBGWOrder
	'中心出し制御
	Case BGW_ORDER.CENTER_PROCESS_START_REP
		If p_iConnectState = CONNECT_ST.CONNECTING				        'リンク中のみ
			p_mltcp.PosOrderCenter(True)								'Bit ON
			p_iCenterState = 0
			p_iBGWOrderNext = BGW_ORDER.CENTER_PROCESS_START_CNF        '応答確認へ
			sw1.Restart                                                 '応答Bit タイマースタート
		End If

	Case BGW_ORDER.CENTER_PROCESS_START_CNF
		'完了応答確認
		If p_mltcp.isRespCenterPos()
			'完了応答OK
			p_iCenterState = 1
			p_iBGWOrderNext = BGW_ORDER.CENTER_PROCESS_STOP_REP			'終了処理へ
		Else
			If sw1.Elapsed.TotalSeconds > 120 Then
				'完了応答BIT未確認タイムオーバー	120
				p_mltcp.PosOrderCenter(False)							'Bit OFF
				p_iCenterState = 2
				Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & ":Confirm Bit Response Time Over (CENTER_PROCESS_START_CNF)")
			Else
				p_iBGWOrderNext = BGW_ORDER.CENTER_PROCESS_START_CNF                '応答確認を継続
			End If
		End If

	Case BGW_ORDER.CENTER_PROCESS_STOP_REP
		'中心出し中止
		p_mltcp.PosOrderCenter(False)							'Bit OFF
		p_iCenterState = -1

	'中心座標手動登録
	Case BGW_ORDER.CENTER_REGIST_START_REP
		If p_iConnectState = CONNECT_ST.CONNECTING				        'リンク中のみ
			'通信領域へセット
			p_mltcp.CentPosX_W = p_CenterXReg							
			p_mltcp.CentPosY_W = p_CenterYReg
			p_mltcp.PosOrderCenterWrite(True)							'Bit ON
			p_iBGWOrderNext = BGW_ORDER.CENTER_REGIST_START_CNF			'応答確認へ
			sw1.Restart                                                 '応答Bit タイマースタート
		End If

	Case BGW_ORDER.CENTER_REGIST_START_CNF
		'完了応答確認
		If p_mltcp.isCentPosWrite()
			'完了応答OK
			p_mltcp.PosOrderCenterWrite(False)							'Bit OFF
		Else
			If sw1.Elapsed.TotalSeconds > 5 Then
				'完了応答BIT未確認タイムオーバー	5
				p_mltcp.PosOrderCenter(False)							'Bit OFF
				Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & ":Confirm Bit Response Time Over (CENTER_REGIST_START_CNF)")
			Else
				p_iBGWOrderNext = BGW_ORDER.CENTER_REGIST_START_CNF     '応答確認を継続
			End If
		End If
	End Select



Catch ex As Exception
    Call FrmDebugLog(System.Reflection.MethodBase.GetCurrentMethod.Name & ex.Message)
    Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub

''' <summary>
''' 手動駆動制御
''' </summary>
''' <param name="sw1"></param>
Private Sub ManualDriveProc(byRef sw1 As Stopwatch)
Try
	Select Case p_iBGWOrder
'熱板 手動駆動制御 
	Case BGW_ORDER.HOT_PLATE_DRV_REP
		If p_iConnectState = CONNECT_ST.CONNECTING                      'リンク中のみ
			If p_mltcp.IsPosOrderZ
				p_mltcp.PosOrderZ(False)									'Z駆動指令BIT OFF
			Else
				p_mltcp.PosHeatB = p_iHotPlatePos                           '目標位置セット
				p_mltcp.PosOrderZ(True)										'Z駆動指令BIT ON
				p_iBGWOrderNext = BGW_ORDER.HOT_PLATE_DRV_CNF              '手動駆動応答確認へ
				sw1.Restart                                                 '応答Bit タイマースタート
			End If 
		End If
	Case BGW_ORDER.HOT_PLATE_DRV_CNF
		If p_mltcp.isRespHeatMove()
			p_iBGWOrderNext = BGW_ORDER.HOT_PLATE_DRV_CNF1              '手動駆動応答確認へ
		Else
			If p_mltcp.isNoOrigin()
			'原点復帰完了していない
				p_iBGWOrderNext = BGW_ORDER.HOT_PLATE_DRV_CNF1		           '終了
			ElseIf p_mltcp.isItLock()
			'インターロックで動作できない
				p_iBGWOrderNext = BGW_ORDER.HOT_PLATE_DRV_CNF1		           '終了
			ElseIf sw1.Elapsed.TotalSeconds > cResponseTimeOutDrive Then
				'応答BIT未確認タイムオーバー
				p_mltcp.PosOrderZ(False)                                'Z駆動指令BIT OFF
				WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & ":Confirm Bit Response Time Over (HOT_PLATE_DRV_CNF)")
			ElseIf p_iError = 0
				p_iBGWOrderNext = BGW_ORDER.HOT_PLATE_DRV_CNF          '手動駆動応答確認へ
			Else
				p_iBGWOrderNext = BGW_ORDER.NO_ORDER		           'エラー発生中は抜ける
			End If
		End If
	Case BGW_ORDER.HOT_PLATE_DRV_CNF1
		p_mltcp.PosOrderZ(False)									'Z駆動指令BIT OFF

'X軸 手動駆動指令受付
	Case BGW_ORDER.X_AXIS_DRV_REP
		If p_iConnectState = CONNECT_ST.CONNECTING                      'リンク中のみ
			p_mltcp.PosX = p_iXAxisPos                                  '目標位置セット
			If p_mltcp.IsPosOrderX 
				p_mltcp.PosOrderX(False)								'X駆動指令BIT OFF
			Else
				p_mltcp.PosOrderX(True)									'X駆動指令BIT ON
				p_iBGWOrderNext = BGW_ORDER.X_AXIS_DRV_CNF              '手動駆動応答確認へ
				sw1.Restart                                             '応答Bit タイマースタート
			End If
		End If
'X軸 手動駆動確認
	Case BGW_ORDER.X_AXIS_DRV_CNF
		If p_mltcp.isRespXAxsisMove()
			p_mltcp.PosOrderX(False)									'X駆動指令BIT OFF
		Else
			If p_mltcp.isNoOrigin()
			'原点復帰完了していない
				p_iBGWOrderNext = BGW_ORDER.X_AXIS_DRV_CNF1		           '終了
			ElseIf p_mltcp.isItLock()
			'インターロックで動作できない
				p_iBGWOrderNext = BGW_ORDER.X_AXIS_DRV_CNF1		           '終了
			ElseIf sw1.Elapsed.TotalSeconds > cResponseTimeOutDrive Then
				'応答BIT未確認タイムオーバー
				p_mltcp.PosOrderX(False)								'X駆動指令BIT OFF
				WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & ":Confirm Bit Response Time Over (X_AXIS_DRV_CNF)")
			ElseIf p_iError = 0
				p_iBGWOrderNext = BGW_ORDER.X_AXIS_DRV_CNF				'手動駆動応答確認へ
			Else
				p_iBGWOrderNext = BGW_ORDER.NO_ORDER		           'エラー発生中は抜ける
			End If
		End If
	Case BGW_ORDER.X_AXIS_DRV_CNF1
			p_mltcp.PosOrderX(False)									'X駆動指令BIT OFF

'Y軸 手動駆動指令受付
	Case BGW_ORDER.Y_AXIS_DRV_REP
		If p_iConnectState = CONNECT_ST.CONNECTING                      'リンク中のみ
			p_mltcp.PosY = p_iYAxisPos                                  '目標位置セット
			If p_mltcp.IsPosOrderY 
				p_mltcp.PosOrderY(False)								'Y駆動指令BIT OFF
			Else
				p_mltcp.PosOrderY(True)									'Y駆動指令BIT ON
				p_iBGWOrderNext = BGW_ORDER.Y_AXIS_DRV_CNF              '手動駆動応答確認へ
				sw1.Restart                                             '応答Bit タイマースタート
			End If

		End If
'Y軸 手動駆動確認
	Case BGW_ORDER.Y_AXIS_DRV_CNF
		If p_mltcp.isRespYAxsisMove()
			p_mltcp.PosOrderY(False)									'Y駆動指令BIT OFF
		Else
			If p_mltcp.isNoOrigin()
			'原点復帰完了していない
				p_iBGWOrderNext = BGW_ORDER.Y_AXIS_DRV_CNF1		           '終了
			ElseIf p_mltcp.isItLock()
			'インターロックで動作できない
				p_iBGWOrderNext = BGW_ORDER.Y_AXIS_DRV_CNF1		           '終了
			ElseIf sw1.Elapsed.TotalSeconds > cResponseTimeOutDrive Then
				'応答BIT未確認タイムオーバー
				p_mltcp.PosOrderY(False)								'Y駆動指令BIT OFF
				WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & ":Confirm Bit Response Time Over (Y_AXIS_DRV_CNF)")
			ElseIf p_iError = 0
				p_iBGWOrderNext = BGW_ORDER.Y_AXIS_DRV_CNF				'手動駆動応答確認へ
			Else
				p_iBGWOrderNext = BGW_ORDER.NO_ORDER		           'エラー発生中は抜ける
			End If
		End If
	Case BGW_ORDER.Y_AXIS_DRV_CNF1
			p_mltcp.PosOrderY(False)									'Y駆動指令BIT OFF

'XYモード駆動開始
	Case BGW_ORDER.XYMODE_DRIVE_START
		If p_iConnectState = CONNECT_ST.CONNECTING Then					'リンク中のみ
			If p_mltcp.IsPosOrderXY
				p_mltcp.PosOrderXY(False)                                   'XYモード駆動指令BIT OFF
			Else
				p_MainLink = 1												'frmMainでの通信リンク(書込み)を一時的に遮断
				p_mltcp.AmplitudeX	= p_iMAmplitudeX
				p_mltcp.AmplitudeY	= p_iMAmplitudeY
				p_mltcp.AmplitudeZ	= p_iMAmplitudeZ
				p_mltcp.PeriodX(1)	= p_iMPeriodX
				p_mltcp.PeriodY(1)	= p_iMPeriodY
				p_mltcp.PeriodZ(1)	= p_iMPeriodZ
				p_mltcp.ActMode		= p_iModeForManualXY
				p_mltcp.PosOrderXY(True)                                    'XYモード駆動指令BIT ON
			End If
		End If

'XYモード駆動停止
	Case BGW_ORDER.XYMODE_DRIVE_STOP
		If p_iConnectState = CONNECT_ST.CONNECTING Then                     'リンク中のみ
			p_mltcp.PosOrderXY(False)                                   'XYモード駆動指令BIT OFF
		End If
		p_MainLink = 0													'frmMainでの通信リンク(書込み)を一時的に遮断


'脱調確認指令受付
	Case BGW_ORDER.OutOfStepConf_REP
		If p_iConnectState = CONNECT_ST.CONNECTING                      'リンク中のみ
			If p_mltcp.IsOutOfStepConf
				p_mltcp.PosOrderOutOfStepConf(False)					'脱調確認指令BIT OFF
			Else
				p_mltcp.PosOrderOutOfStepConf(True)						'脱調確認指令BIT ON
				p_iBGWOrderNext = BGW_ORDER.OutOfStepConf_CNF           '応答確認へ
				sw1.Restart                                             '応答Bit タイマースタート
			End If

		End If
'脱調確認
	Case BGW_ORDER.OutOfStepConf_CNF
		If p_mltcp.isOosConf()
			p_mltcp.PosOrderOutOfStepConf(False)						'脱調確認指令BIT OFF
			p_iBGWOrderNext = BGW_ORDER.OutOfStepConf_CNF1				'脱調誤差取得・測定データ更新へ
			sw1.Restart													'脱調誤差確認 タイマースタート
		Else
			If sw1.Elapsed.TotalSeconds > cResponseTimeOutDrive Then
				'応答BIT未確認タイムオーバー
				p_mltcp.PosOrderOutOfStepConf(False)					'脱調確認指令BIT OFF
				WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & ":Confirm Bit Response Time Over (OutOfStepConf_CNF)")
			ElseIf p_iError = 0
				p_iBGWOrderNext = BGW_ORDER.OutOfStepConf_CNF			'応答確認へ
			Else
				p_iBGWOrderNext = BGW_ORDER.OutOfStepConf_CNF1			'エラー発生中は抜ける
			End If
		End If
'脱調誤差取得・測定データ更新
	Case BGW_ORDER.OutOfStepConf_CNF1
		If sw1.Elapsed.TotalSeconds > 1		'取得までのウェイト
			'取得した脱調誤差更新
			p_dtFile.p_head.dOosValX = p_mltcp.OosValX
			p_dtFile.p_head.dOosValY = p_mltcp.OosValY
			p_dtFile.p_head.dOosValZ = p_mltcp.OosValZ

			'CPU間通信異常カウントセット
			p_dtFile.p_head.iCpuCommErrCount = p_mltcp.CPUCommErrCount

			'補正データ込みで上書き更新
			Select Case p_MeasMode
			Case 0			'ゲルタイム測定
				p_dtFile.RealTimeMeasDataFileSaveText(eMeasMode.Normal, False, m_cts)
			Case 1			'フローカーブ測定
				Select Case p_FlcvMeasMode
				Case eViscoMeaure.SpeedDep
				'速度依存
					p_dtFile.RealTimeMeasDataFileSaveText( eMeasMode.FlowCurve_Speed, False, m_cts )	'ファイル上書き保存(粘度-速度依存)
				Case eViscoMeaure.TempDep
				'温度依存
					p_dtFile.RealTimeMeasDataFileSaveText( eMeasMode.FlowCurve_Temp, False, m_cts )		'ファイル上書き保存(粘度-温度依存)
				Case eViscoMeaure.TimeDep
				'時間依存
					p_dtFile.RealTimeMeasDataFileSaveText(eMeasMode.FlowCurve_Time, False, m_cts)
				End Select
			End Select

			'CPU間通信異常確認(Trueならエラー画面を出す)
			p_bDevInternalComError = p_mltcp.isErrCPUCommErr()
			p_iBGWOrderNext = BGW_ORDER.NO_ORDER						'終了
		Else
			p_iBGWOrderNext = BGW_ORDER.OutOfStepConf_CNF1				'継続
		End If

	End Select
Catch ex As Exception
    Call FrmDebugLog(System.Reflection.MethodBase.GetCurrentMethod.Name & ex.Message)
    Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub
''' <summary>
''' 原点復帰制御
''' </summary>
''' <param name="sw1"></param>
Private Sub ReturnOriginProc(ByRef sw1 As Stopwatch)
Try
	Select Case p_iBGWOrder
	Case BGW_ORDER.ORG_BUTTON_REP
		If p_iConnectState = CONNECT_ST.CONNECTING Then                 'リンク中のみ
			'If p_mltcp.IsPosOrderOrigin()
			'	p_mltcp.PosOrderOrigin(False)							'原点復帰制御開始Bit OFF

			'Else
				p_mltcp.PosOrderOrigin(True)							'原点復帰制御開始Bit ON
				p_iBGWOrderNext = BGW_ORDER.ORG_BUTTON_REP_1            '完了待ちへ
				sw1.Restart

			'End If
		End If

	Case BGW_ORDER.ORG_BUTTON_REP_1
		Thread.Sleep(200)
		p_iBGWOrderNext = BGW_ORDER.ORG_BUTTON_REP_2					'完了待ちへ


	Case BGW_ORDER.ORG_BUTTON_REP_2
		If sw1.Elapsed.TotalSeconds > cOriginSearchTimeLimit Then
			’原点復帰タイムオーバー
			Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & ":原点復帰タイムオーバー")
			p_mltcp.PosOrderOrigin(False)								'原点復帰制御開始Bit OFF

		ElseIf p_bDevError							
		'何らかのエラー発生
			p_mltcp.PosOrderOrigin(False)								'原点復帰制御開始Bit OFF

		ElseIf p_mltcp.isItLock()
		'インターロックで動作できない
			p_mltcp.PosOrderOrigin(False)								'原点復帰制御開始Bit OFF

		ElseIf p_mltcp.isStatOriginComp()
		'原点復帰完了がONになった
			'原点復帰完了
			p_mltcp.PosOrderOrigin(False)							'原点復帰制御開始Bit OFF

		'ElseIf Not p_mltcp.IsPosOrderOrigin()
		''原点復帰指令がOFFになった
		'	p_mltcp.PosOrderOrigin(False)							'原点復帰制御開始Bit OFF
			p_iBGWOrderNext = BGW_ORDER.NO_ORDER
		Else
			p_iBGWOrderNext = BGW_ORDER.ORG_BUTTON_REP_2            '完了待ち継続
		End If
	End Select
Catch ex As Exception
    Call FrmDebugLog(System.Reflection.MethodBase.GetCurrentMethod.Name & ex.Message)
    Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub
''' <summary>
''' 日時設定制御
''' </summary>
''' <param name="sw1"></param>
Private Sub TimeSetProc(byref sw1 As Stopwatch)
Try
	Select Case p_iBGWOrder
	Case BGW_ORDER.SETDATE_REP
		If p_iConnectState = CONNECT_ST.CONNECTING                      'リンク中のみ
			If IsNothing(p_dDevDateSet) Or m_bFirstConnect
				p_dDevDateSet = Now()                                   '指示無ければ現在日時
			End If
			p_mltcp.sbTimeSetForm(p_dDevDateSet)                        '時刻設定用変数セット
			p_mltcp.DateSet(True)	                                    '時計設定BIT ON
			p_iBGWOrderNext = BGW_ORDER.SETDATE_CNF                     '時計設定応答確認へ
			sw1.Restart                                                 '応答Bit タイマースタート
			m_bFirstConnect = False										'初回接続クリア
		Else

		End If
	Case BGW_ORDER.SETDATE_CNF
		'時刻設定応答確認
		If p_mltcp.isRespDateSet()
			p_mltcp.DateSet(False)	                                    '時計設定BIT OFF
			WriteLog("Date setting was completed.", eLOG_LEVEL.iINFO)
		Else
			If sw1.Elapsed.TotalSeconds > cResponseTimeOut Then
				'応答BIT未確認タイムオーバー	5秒
				p_mltcp.DateSet(False)	                                '時計設定BIT OFF
				WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & "Confirm Bit Response Time Over (SETDATE_CNF)")
			Else
				p_iBGWOrderNext = BGW_ORDER.SETDATE_CNF                 '時計設定応答確認へ
			End If
		End If
	End Select
Catch ex As Exception
    Call FrmDebugLog(System.Reflection.MethodBase.GetCurrentMethod.Name & ex.Message)
    Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub
Public p_DevSerialWrite As Int32
Public p_DevSerialRead	As Int32
''' <summary>
''' 装置名設定制御
''' </summary>
''' <param name="sw1"></param>
Private Sub ProductSetProc(byref sw1 As Stopwatch)
Try
	Select Case p_iBGWOrder
	Case BGW_ORDER.SETPRO_REP
		If p_iConnectState = CONNECT_ST.CONNECTING                      'リンク中のみ
			p_mltcp.DevSereal = p_DevSerialWrite						'装置名設定用変数セット
			p_mltcp.SerialWrite(True)	                                '装置名設定BIT ON
			p_iBGWOrderNext = BGW_ORDER.SETPRO_CNF						'装置名設定応答確認へ
			sw1.Restart                                                 '応答Bit タイマースタート
		Else

		End If
	Case BGW_ORDER.SETPRO_CNF
		'装置名設定応答確認
		If p_mltcp.isDevSerial()
			p_mltcp.SerialWrite(False)	                                '装置名設定BIT OFF
		Else
			If sw1.Elapsed.TotalSeconds > cResponseTimeOut Then
				'応答BIT未確認タイムオーバー	5秒
				p_mltcp.SerialWrite(False)	                                '装置名設定BIT OFF
				WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & "Confirm Bit Response Time Over (SETPRO_CNF)")
			Else
				p_iBGWOrderNext = BGW_ORDER.SETPRO_CNF                 '装置名設定応答確認へ
			End If
		End If
	End Select
Catch ex As Exception
    Call FrmDebugLog(System.Reflection.MethodBase.GetCurrentMethod.Name & ex.Message)
    Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub

''' <summary>
''' 熱板温度制御開始/終了
''' </summary>
''' <param name="sw1"></param>
Private Sub HeaterProc(ByRef sw1 As Stopwatch)
Try
	If p_iConnectState = CONNECT_ST.CONNECTING Then                 'リンク中のみ
		Select Case p_iBGWOrder
		Case BGW_ORDER.TEMP_BUTTON_REP
		'HP/PB/SG温度制御
			if p_mltcp.isStatHeaterOn() Or p_mltcp.isStatHeaterOn2() Or p_mltcp.StartTempCont<>0
				'温度制御開始Bit OFF
				p_mltcp.StartTempCont = 0                                   
			Else
				'温度制御開始Bit ON HP,PB,SG
				p_mltcp.StartTempCont = MemLinkTCP.eWTempBit.iHP Or MemLinkTCP.eWTempBit.iProbe Or MemLinkTCP.eWTempBit.iSG
			End If
			p_iHeatingHP = IIf(p_mltcp.isStatHeaterOn() And p_mltcp.isStatHeaterOn2(), 1, 0)
			p_iHeatingPB = p_iHeatingHP
			p_iHeatingSG = IIf(p_mltcp.isStatHeaterOn3(), 1, 0)

		Case BGW_ORDER.TEMP_HP_BUTTON_REP
		'HP温度制御
			if p_mltcp.isStatHeaterOn()
				'温度制御開始Bit OFF
				p_mltcp.StartTempCont = p_mltcp.StartTempCont And Not(MemLinkTCP.eWTempBit.iHP)
			Else
				'温度制御開始Bit ON
				p_mltcp.StartTempCont = p_mltcp.StartTempCont Or MemLinkTCP.eWTempBit.iHP
			End If
			p_iHeatingHP = IIf(p_mltcp.isStatHeaterOn(), 1, 0)

		Case BGW_ORDER.TEMP_PB_BUTTON_REP
		'PB温度制御
			if p_mltcp.isStatHeaterOn2()
				'温度制御開始Bit OFF
				p_mltcp.StartTempCont = p_mltcp.StartTempCont And Not(MemLinkTCP.eWTempBit.iProbe)
			Else
				'温度制御開始Bit ON
				p_mltcp.StartTempCont = p_mltcp.StartTempCont Or MemLinkTCP.eWTempBit.iProbe
			End If
			p_iHeatingPB = IIf(p_mltcp.isStatHeaterOn2(), 1, 0)

		Case BGW_ORDER.TEMP_SG_BUTTON_REP
		'起歪体温度制御
			if p_mltcp.isStatHeaterOn3()
				'温度制御開始Bit OFF
				p_mltcp.StartTempCont = p_mltcp.StartTempCont And Not(MemLinkTCP.eWTempBit.iSG)
			Else
				'温度制御開始Bit ON
				p_mltcp.StartTempCont = p_mltcp.StartTempCont Or MemLinkTCP.eWTempBit.iSG
			End If
			p_iHeatingSG = IIf(p_mltcp.isStatHeaterOn3(), 1, 0)
		End Select
	End If
Catch ex As Exception
    Call FrmDebugLog(System.Reflection.MethodBase.GetCurrentMethod.Name & ex.Message)
    Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub
Public iResetGo As Int32 = 0
''' <summary>
''' エラーリセット制御
''' </summary>
''' <param name="sw1"></param>
Private Sub ErrorReSetProc(ByRef sw1 As Stopwatch)
Try
	Select Case p_iBGWOrder
	Case BGW_ORDER.RESET_ERROR_REP
		If p_iConnectState = CONNECT_ST.CONNECTING                      'リンク中のみ
			p_mltcp.ErrReset = 1                                        'エラーリセットBit ON
			'指令ビットを全てOFF
			p_mltcp.StartSW = 0
			p_mltcp.PosOrder = 0										
			If (p_mltcp.DevErr1 And &h8070) Or _
				(p_mltcp.DevErr2 And &h1C02) Or _
				(p_mltcp.DevErr3 And &h7820)
				p_mltcp.StartTempCont = 0		'温度制御OFF
				WriteLog("温度制御OFF",eLOG_LEVEL.iINFO)
			End If
			p_iZeroAdjOk = 0											'ゼロ調正完了無効
			p_iBGWOrderNext = BGW_ORDER.RESET_ERROR_CNF                 'エラーリセット応答確認へ
			sw1.Restart                                                 '応答Bit タイマースタート
		End If

	Case BGW_ORDER.RESET_ERROR_CNF
		'エラーリセット応答確認
		If p_mltcp.isRespErrReset()
			p_mltcp.ErrReset = 0                                        'エラーリセットBIT OFF
			p_bDevInternalComError = False								'CPU間通信エラー内部フラグリセット
			iResetGo  = 1												'リセット発行済

		Else
			If sw1.Elapsed.TotalSeconds > cResponseTimeOut Then
				'応答BIT未確認タイムオーバー	5秒
				p_mltcp.ErrReset = 0                                    'エラーリセットBIT OFF
				iResetGo  = 1												'リセット発行済
				WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & ":Confirm Bit Response Time Over (RESET_ERROR_CNF)")
			Else
				p_iBGWOrderNext = BGW_ORDER.RESET_ERROR_CNF             'エラーリセット応答確認へ
			End If
		End If
	End Select
Catch ex As Exception
    Call FrmDebugLog(System.Reflection.MethodBase.GetCurrentMethod.Name & ex.Message)
    Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub
''' <summary>
''' メンテナンス情報リセット制御
''' </summary>
''' <param name="sw1"></param>
Private Sub MenteResetProc(ByRef sw1 As Stopwatch)
Try
	Select Case p_iBGWOrder
'本体電源ON時間 リセット処理
	Case BGW_ORDER.RESET_INTEG_POWON
		If p_iConnectState = CONNECT_ST.CONNECTING                      'リンク中のみ
			p_mltcp.PowOnTimeReset(True)                                '本体電源ON時間リセットBit ON
			p_iBGWOrderNext = BGW_ORDER.RESET_INTEG_POWON_CNF           '応答確認へ
			sw1.Restart                                                 '応答Bit タイマースタート
		End If

	Case BGW_ORDER.RESET_INTEG_POWON_CNF
		'エラーリセット応答確認
		If p_mltcp.isPowOnTimeReset()
			p_mltcp.PowOnTimeReset(False)                               '本体電源ON時間リセットBit OFF
		Else
			If sw1.Elapsed.TotalSeconds > cResponseTimeOut Then
				'応答BIT未確認タイムオーバー	5秒
				p_mltcp.PowOnTimeReset(False)                           '本体電源ON時間リセットBit OFF
				WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & ":Confirm Bit Response Time Over (RESET_INTEG_POWON_CNF)")
			Else
				p_iBGWOrderNext = BGW_ORDER.RESET_INTEG_POWON_CNF       '継続
			End If
		End If

'HPヒータON時間 リセット処理
	Case BGW_ORDER.RESET_INTEG_HEATON
		If p_iConnectState = CONNECT_ST.CONNECTING                      'リンク中のみ
			p_mltcp.HeaterOnTimeReset(True)                             'ヒータON時間リセットBit ON
			p_iBGWOrderNext = BGW_ORDER.RESET_INTEG_HEATON_CNF           '応答確認へ
			sw1.Restart                                                 '応答Bit タイマースタート
		End If

	Case BGW_ORDER.RESET_INTEG_HEATON_CNF
		'エラーリセット応答確認
		If p_mltcp.isHetOnTimeReset()
			p_mltcp.HeaterOnTimeReset(False)							'ヒータON時間リセットBit OFF
		Else
			If sw1.Elapsed.TotalSeconds > cResponseTimeOut Then
				'応答BIT未確認タイムオーバー	5秒
				p_mltcp.HeaterOnTimeReset(False)							'ヒータON時間リセットBit OFF
				WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & ":Confirm Bit Response Time Over (RESET_INTEG_HEATON_CNF)")
			Else
				p_iBGWOrderNext = BGW_ORDER.RESET_INTEG_HEATON_CNF       '継続
			End If
		End If

'XYZ動作時間 リセット処理
	Case BGW_ORDER.RESET_INTEG_XYACT
		If p_iConnectState = CONNECT_ST.CONNECTING                      'リンク中のみ
			p_mltcp.XActTimeReset(True)									'X動作時間リセットBit ON
			p_mltcp.YActTimeReset(True)									'Y動作時間リセットBit ON
			p_mltcp.ZActTimeReset(True)									'Z動作時間リセットBit ON
			p_iBGWOrderNext = BGW_ORDER.RESET_INTEG_XYACT_CNF           '応答確認へ
			sw1.Restart                                                 '応答Bit タイマースタート
		End If

	Case BGW_ORDER.RESET_INTEG_XYACT_CNF
		'エラーリセット応答確認
		If p_mltcp.isXActTimeReset() AndAlso p_mltcp.isYActTimeReset() AndAlso p_mltcp.isZActTimeReset()
			p_mltcp.XActTimeReset(False)								'X動作時間リセットBit OFF
			p_mltcp.YActTimeReset(False)								'Y動作時間リセットBit OFF
			p_mltcp.ZActTimeReset(False)								'Z動作時間リセットBit OFF
		Else
			If sw1.Elapsed.TotalSeconds > cResponseTimeOut Then
				'応答BIT未確認タイムオーバー	5秒
				p_mltcp.XActTimeReset(False)								'X動作時間リセットBit OFF
				p_mltcp.YActTimeReset(False)								'Y動作時間リセットBit OFF
				p_mltcp.ZActTimeReset(False)								'Z動作時間リセットBit OFF
				WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & ":Confirm Bit Response Time Over (RESET_INTEG_XYACT_CNF)")
			Else
				p_iBGWOrderNext = BGW_ORDER.RESET_INTEG_XYACT_CNF       '継続
			End If
		End If

'PBヒータON時間 リセット処理
	Case BGW_ORDER.RESET_INTEG_HEATON2
		If p_iConnectState = CONNECT_ST.CONNECTING                      'リンク中のみ
			p_mltcp.PBHOnTimeReset(True)								'ヒータON時間リセットBit ON
			p_iBGWOrderNext = BGW_ORDER.RESET_INTEG_HEATON2_CNF         '応答確認へ
			sw1.Restart                                                 '応答Bit タイマースタート
		End If

	Case BGW_ORDER.RESET_INTEG_HEATON2_CNF
		'エラーリセット応答確認
		If p_mltcp.isPbHeatOnTimeReset()
			p_mltcp.PBHOnTimeReset(False)								'ヒータON時間リセットBit OFF
		Else
			If sw1.Elapsed.TotalSeconds > cResponseTimeOut Then
				'応答BIT未確認タイムオーバー	5秒
				p_mltcp.PBHOnTimeReset(False)							'ヒータON時間リセットBit OFF
				WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & ":Confirm Bit Response Time Over (RESET_INTEG_HEATON2_CNF)")
			Else
				p_iBGWOrderNext = BGW_ORDER.RESET_INTEG_HEATON2_CNF       '継続
			End If
		End If

'SGヒータON時間 リセット処理
	Case BGW_ORDER.RESET_INTEG_HEATON3
		If p_iConnectState = CONNECT_ST.CONNECTING                      'リンク中のみ
			p_mltcp.SGHOnTimeReset(True)								'ヒータON時間リセットBit ON
			p_iBGWOrderNext = BGW_ORDER.RESET_INTEG_HEATON3_CNF         '応答確認へ
			sw1.Restart                                                 '応答Bit タイマースタート
		End If

	Case BGW_ORDER.RESET_INTEG_HEATON2_CNF
		'エラーリセット応答確認
		If p_mltcp.isSGHeatOnTimeReset()
			p_mltcp.SGHOnTimeReset(False)								'ヒータON時間リセットBit OFF
		Else
			If sw1.Elapsed.TotalSeconds > cResponseTimeOut Then
				'応答BIT未確認タイムオーバー	5秒
				p_mltcp.PBHOnTimeReset(False)							'ヒータON時間リセットBit OFF
				WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & ":Confirm Bit Response Time Over (RESET_INTEG_HEATON3_CNF)")
			Else
				p_iBGWOrderNext = BGW_ORDER.RESET_INTEG_HEATON3_CNF       '継続
			End If
		End If
	End Select
Catch ex As Exception
    Call FrmDebugLog(System.Reflection.MethodBase.GetCurrentMethod.Name & ex.Message)
    Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub
''' <summary>
''' Z無負荷応力設定制御
''' </summary>
''' <param name="sw1"></param>
Private Sub ZNoLoadSetProc(ByRef sw1 As Stopwatch)
Try
	Select Case p_iBGWOrder
	Case BGW_ORDER.ZNOLOAD_STRESS_SET_REP
		If p_iConnectState = CONNECT_ST.CONNECTING                      'リンク中のみ
			p_mltcp.ZNoLoadStrsSet(True)								'指令BIT ON
			p_iBGWOrderNext = BGW_ORDER.ZNOLOAD_STRESS_SET_CNF          '応答確認へ
			sw1.Restart                                                 '応答Bit タイマースタート
		End If

	Case BGW_ORDER.ZNOLOAD_STRESS_SET_CNF
		'応答確認
		If p_mltcp.isStatZNoLoadStress()
			p_mltcp.ZNoLoadStrsSet(False)								'指令BIT OFF
		Else
			If sw1.Elapsed.TotalSeconds > cResponseTimeOut Then
				'応答BIT未確認タイムオーバー	5秒
				p_mltcp.ZNoLoadStrsSet(False)								'指令BIT OFF
				WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & ":Confirm Bit Response Time Over (ZNOLOAD_STRESS_SET_CNF)")
			Else
				p_iBGWOrderNext = BGW_ORDER.ZNOLOAD_STRESS_SET_CNF      '応答確認へ
			End If
		End If
	End Select
Catch ex As Exception
    Call FrmDebugLog(System.Reflection.MethodBase.GetCurrentMethod.Name & ex.Message)
    Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub



'■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
'   パブリック関数
'■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■

'================================================
'通信を強制切断
'引数	なし
'戻り値 なし
'================================================
'''<summary> 通信を強制切断する。 </summary>
Public Sub DisconnectML()
	If p_mltcp.TCP_IsConnected() = True Then
		p_mltcp.TCP_DisConnect()
	End If
End Sub
'メンテナンス積算時間の超過チェック
'   引数：ulIntegTime      現在の積算時間[sec]
'       ：iThreshold       超過判定閾値[hour]
'   戻値：True..超過、False..閾値範囲内
Public Function ChkMntIntegTimeOver(ByVal ulIntegTime As ULong, ByVal iThreshold As Integer) As Boolean
	Dim bChk As Boolean = False

	Try
		'超過判定閾値をsecに換算
		Dim ulSec As ULong = iThreshold * 3600

		If ulSec < ulIntegTime Then
			bChk = True         '超過判定閾値を超過
		End If

	Catch ex As Exception
		Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	End Try

	Return bChk
End Function

'メンテナンスリセット日からの経過年数の超過チェック
'   引数：dtRstDate        前回のリセット日
'       ：iThreshold       超過判定閾値[year]
'   戻値：True..超過、False..閾値範囲内
Public Function ChkMntRstDayOver(ByVal sRstDate As String, ByVal iThreshold As Integer) As Boolean
	Dim bChk As Boolean = False

	Try
		'前回のリセット日がない間はチェック不可
		If sRstDate = "" Then
			Return False
		End If

		'前回のリセット日から超過判定閾値年数経過後の日を算出
		Dim dtTemp As Date = CDate(sRstDate).AddYears(iThreshold)

		If dtTemp < Today Then
			bChk = True         '本日が算出日を超過
		End If

	Catch ex As Exception
		Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	End Try

	Return bChk
End Function
Public  p_DispUpdate	As Int16 = 0			'UI表示画面更新確認
Private	m_iCalJob		As Int16 = -1			'応力較正制御フロー変数
Public Property CalJob() As Int32
	Get
		CalJob = m_iCalJob
	End Get
	Set(Value As Int32)
		m_iCalJob = Value
	End Set
End Property
Private m_iCalibTrig	As Int16 = 0			'応力較正制御フロー開始トリガ
Private m_idx			As Int16				'応力較正情報配列インデックス
Private m_iStres()		As Int64				'採取中間補正応力
Private m_dViscosity()	As Double				'粘度
Private m_dXPosPhase()	As Double				'動粘弾性X
Private m_dSplStrain()	As Double				'サンプル歪
Private m_dShearMod()	As Double				'せん断弾性率(Pa)
Private m_dXStrgMod()	As Double				'X貯蔵弾性率(Pa)
Private m_dXElasMod()	As Double				'X損失弾性率(Pa)
Private	m_dXLossTan()	As Double				'X損失正接

Private m_dAlpha()		As Double				'位相差
Private m_iResultAve	As Int64                '100%応力平均結果
Private m_dAlphaAve		As Double               '位相差平均結果
Private m_dBRxAve		As Double               '比率x平均結果
Private m_dBRyAve		As Double               '比率y平均結果
Private m_dPxPhaseAve	As Double               'x位置位相差平均結果
Private m_dPyPhaseAve	As Double               'y位置位相差平均結果

Private m_iSensMode		As Int16				'0,1,2


'100%応力較正ジョブ定義
Public Enum eClbJob						
	IDLE		= -1					'開始待ち
	MEAS_SEL	= 0						'測定条件選択
	VISC_TBL_CL_WAIT					'テーブル行クリア待ち(粘度測定)
	XYMODE_SEL							'XY動作設定
	STAB_WAIT							'本体安定待ち
	XYACT_VERIFICATION					'XY動作確認
	DATAGET								'データ取得
	DATACAL								'データ補正・平均化
	DispUpDateWait						'表示更新待ち
	FINISHING							'終了処理中
	FINISH								'終了処理
End Enum
Dim dWaitAmpChange As double	= 3		'振幅変更後の監視までのウェイト(秒)
''' <summary>
''' 100％応力測定(キャリブレーション)を行う制御
''' </summary>
''' <param name="sw2">制御に使用する時間計測</param>
''' <param name="Dtf">測定データクラス</param>
''' <returns></returns>
Public Function CalibControl(ByVal sMode As String, ByRef clb() As stPoint, ByRef sw2 As Stopwatch, ByRef Dtf As DataFile) As Int32
	Dim i As int32
	Try
		Select Case m_iCalJob
		Case eClbJob.IDLE 'アイドル中
			If m_iCalibTrig = 1
				m_idx = 0
				p_iDataCount = 0											'データカウントクリア
				m_iCalJob = eClbJob.MEAS_SEL
			End If

		Case eClbJob.MEAS_SEL  '測定選択確認
			For i = m_idx To clb.GetUpperBound(0)
				If clb(i).bSelected								'測定選択されている
					p_iDataCount = 0                                'データカウントクリア
					m_iStres = Nothing                              '配列初期化
					m_idx = i
					Select Case sMode
					Case "X"
						p_ClbXIdx = i
					Case "C"
						p_ClbCIdx = i
					Case "P"
						p_ClbPIdx = i
					End Select
					p_CaliblationState.iState = eCalibState.COND_CONF		'条件インデックス確定
					p_CaliblationState.iCol = clb(m_idx).iCol
					p_CaliblationState.iRaw = clb(m_idx).iRow
					m_iCalJob = eClbJob.XYMODE_SEL							'>>> XY動作設定
					Exit for
				End If
			Next
			If i > clb.GetUpperBound(0)
				'配列終了
				p_CaliblationState.iState = eCalibState.FINISHING	'終了中
				sw2.Restart()
				m_iCalJob = eClbJob.FINISHING						'>>> 終了中処理へ
			End If

		Case eClbJob.XYMODE_SEL 'XY動作設定
			Select Case sMode
			Case "X"
				'X軸振幅
				p_mltcp.AmplitudeX = clb(m_idx).dAmplitudeX * 1000.0
				'Y軸振幅
				p_mltcp.AmplitudeY = 0
				'Z軸振幅
				p_mltcp.AmplitudeZ = 0
				'X軸周期
				p_mltcp.PeriodX(1) = clb(m_idx).dPeriodX
				'Y軸周期
				p_mltcp.PeriodY(1) = 1
				'Z軸周期
				p_mltcp.PeriodZ(1) = 1
				m_iSensMode = clSpec.eStrsSens.Line
				dWaitAmpChange = clb(m_idx).dPeriodX
			Case "C"
				'X軸振幅
				p_mltcp.AmplitudeX = clb(m_idx).dAmplitudeX * 1000.0
				'Y軸振幅
				p_mltcp.AmplitudeY = clb(m_idx).dAmplitudeY * 1000.0
				'Z軸振幅
				p_mltcp.AmplitudeZ = 0
				'X軸周期
				p_mltcp.PeriodX(1) = clb(m_idx).dPeriodX
				'Y軸周期
				p_mltcp.PeriodY(1) = clb(m_idx).dPeriodY
				'Z軸周期
				p_mltcp.PeriodZ(1) = 1
				m_iSensMode = clSpec.eStrsSens.Circle
				dWaitAmpChange = clb(m_idx).dPeriodX
			Case "P"
				'X軸振幅
				p_mltcp.AmplitudeX = 0
				'Y軸振幅
				p_mltcp.AmplitudeY = 0

				'Z軸振幅増ステップ(um)
				Dim dDif As Double = Math.Abs((clb(m_idx).dAmplitudeZ * 1000.0) - p_mltcp.AmplitudeZ) * clb(m_idx).dPeriodZ
				If dDif < 0
				'最大振幅から0振幅に移るとき=周期変更時
					p_mltcp.ZAmpStep = 1
				Else
					Select Case dDif
					Case 0 To 10
						p_mltcp.ZAmpStep = 1
					Case Else
						p_mltcp.ZAmpStep = dDif / 10
					End Select
				End If

				'Z軸振幅
				p_mltcp.AmplitudeZ = clb(m_idx).dAmplitudeZ * 1000.0
'Debug.Print(clb(m_idx).dAmplitudeZ.ToString)
				'X軸周期
				p_mltcp.PeriodX(1) = 1
				'Y軸周期
				p_mltcp.PeriodY(1) = 1
				'Z軸周期
				p_mltcp.PeriodZ(1) = clb(m_idx).dPeriodZ
				m_iSensMode = clSpec.eStrsSens.Point
				dWaitAmpChange = clb(m_idx).dPeriodZ
			End Select
			Array.Resize(m_iStres, 0)								'配列準備
			Dtf.ReadBufferClear()
			sw2.Restart()
			m_iCalJob = eClbJob.STAB_WAIT                           '>>> 本体の安定待ち

			'If p_mltcp.bMesDataEnable Andalso p_mltcp.isStatXYAction Then
			''If p_mltcp.bMesDataEnable Andalso Not p_mltcp.isFreqUpdate Then
			'	sw2.Restart()
			'	m_iCalJob = eClbJob.STAB_WAIT                           '>>> 本体の安定待ち
			'End If

		Case eClbJob.STAB_WAIT '本体安定待ち
			'If sw2.Elapsed.TotalSeconds > 3
			If sw2.Elapsed.TotalSeconds > dWaitAmpChange
				If p_mltcp.bMesDataEnable Andalso p_mltcp.isStatXYAction Then
					If Not p_mltcp.isFreqUpdate()
					'XYZ振動設定変更中でないこと
						Array.Resize(m_iStres, m_iStres.Length + MEAS_BLOCK_DATA_NUM)   '配列確保
						'p_CaliblationState.iState = eCalibState.DATA_TRANS				'データ取得
						m_iCalJob = eClbJob.XYACT_VERIFICATION							'>>> XY動作確認
					End If'
				End If
			End If

		Case eClbJob.XYACT_VERIFICATION								'XY動作確認
			'Dim dAlpha		As Double
			'Dim dPxPhase	As Double
			'Dim dPyPhase	As Double
			'Dim dPzPhase	As Double
			'if p_mltcp.isStatXYAction() = True									'XY動作中がON
				'データファイル名の決定
				Dtf.MesFileNumber = 0											'ファイル連番クリア
#If 0
				'Dtf.SetDataFileName = Dtf.sMeasindCode(eMesCD.CLB) & sMode & "-" & Format(Now, "yyyyMMddHHmmss") & "-" & m_idx.ToString("000")
#Else
				Select Case m_iSensMode
				Case eStrsSens.Point
					Dtf.SetDataFileName() = Dtf.sMeasindCode(eMesCD.CLB) & sMode & "-" & Format(Now, "yyyyMMddHHmmss") & "-" & p_mltcp.PeriodZ(1).ToString("F1") & "$" & (p_mltcp.AmplitudeZ / 1000.0).ToString("F3")
				Case Else
					Dtf.SetDataFileName() = Dtf.sMeasindCode(eMesCD.CLB) & sMode & "-" & Format(Now, "yyyyMMddHHmmss") & "-" & p_mltcp.PeriodX(1).ToString("F1") & "$" & (p_mltcp.AmplitudeX / 1000.0).ToString("F3")
				End Select
#end if
				''条件の減衰比を取得
				'If 0 <> FullStressCalibValueGet(0, p_ZClbX, p_ZClbC, p_ZClbP, p_FClbX, p_FClbC, p_FClbP, p_dFullStressZero, p_dFullStressAttenuation, dAlpha, dPxPhase, dPyPhase, dPzPhase, m_iSensMode, clb(m_idx).dAmplitudeX, clb(m_idx).dPeriodX )
				'	p_dFullStressAttenuation = 1.0
				'End If
				p_dFullStressZero = 0
				p_dFullStressAttenuation = 1.0
				m_iCorrRet = 0
				m_iStartIdx = 0
				m_iCorrectCount = 0
				Select Case m_iSensMode
				Case eStrsSens.Point
					m_iPeriodSample = p_mltcp.PeriodZ(1) * 1000 / MEAS_TIME_RESOLUTION			'1周期分のサンプル数計算
				Case Else
					m_iPeriodSample = p_mltcp.PeriodX(1) * 1000 / MEAS_TIME_RESOLUTION			'1周期分のサンプル数計算
				End Select
				p_dtFile.DataCount = 0														'蓄積データカウンタクリア

				MeasureHeaderInfoSet(eMeasMode.Calib)							'測定開始時のファイルヘッダ情報のセット
				m_iCalJob = eClbJob.DATAGET										'>>> データ取得・結果セット
			'End If


		Case eClbJob.DATAGET 'データ取得
			If p_mltcp.bMesDataEnable Andalso p_mltcp.isStatXYAction Andalso (Not p_mltcp.isFreqUpdate) Then
			'If p_mltcp.bMesDataEnable Andalso Not p_mltcp.isFreqUpdate Then
			'測定データ有効
				Dtf.RealTimeMeasDataFileSaveText(eMeasMode.Calib, True, m_cts, , , , , p_iCalibMode)	'測定データ蓄積ファイルセーブ
				p_CaliblationState.iState = eCalibState.DATA_TRANS					'データ取得

			'補正処理(リアルタイム) ----------------------
'				If p_dtFile.DataCount-m_iStartIdx >= m_iPeriodSample 
				'1周期分のデータ取得まで待つ
					if m_iCorrectCount = 0
					'初回は0インデックスから
						m_iStartIdx = 0
						Select Case m_iSensMode
						Case eStrsSens.Point
							datCal.AveTempClear(p_mltcp.PeriodZ(1), 0)			'クリア
						Case Else
							datCal.AveTempClear(p_mltcp.PeriodX(1), 0)			'クリア
						End Select
					End If
					'リアルタイム中間値補正
					'Z決定位置をセット
					If p_mltcp.ZConfirmPos<>0
						p_dtFile.p_head.dZAfterPos = Ctype(p_mltcp.ZConfirmPos, Double) / 10.0
					End If
					Dim cp As DataCal.stCalcPara = Nothing
					cp.iMeas		= If( p_iCalibMode = 0, eHoseiMeasMode.FullCalib, eHoseiMeasMode.ZeroCalib )
					cp.iMode		= eHoseiCalcMode.RealTime
					cp.cts			= m_cts
					cp.clDt			= Dtf
					'Select Case p_clSpec.spi.iStressSensMode
					Select Case m_iSensMode
					Case eStrsSens.Point
						cp.dAmplitude	= p_mltcp.AmplitudeZ / 1000.0
						cp.dPeriod		= p_mltcp.PeriodZ(1)
					Case Else
						cp.dAmplitude	= p_mltcp.AmplitudeX / 1000.0
						cp.dPeriod		= p_mltcp.PeriodX(1)
					End Select
					'cp.dPeriod		= p_mltcp.PeriodX(1)
					'cp.dAmplitude	= p_mltcp.AmplitudeX / 1000.0
					cp.iSampleStart	= m_iStartIdx
					cp.iSampleNum	= Dtf.iHosei.Length - m_iStartIdx
					cp.fltSet		= p_FltSetting
					cp.dXYWait		= p_mltcp.MesPreWait(1) + p_mltcp.MeasWaitTime(1)
					cp.dZeroStress	= p_dFullStressZero
					cp.dAttenuation	= p_dFullStressAttenuation
					cp.dDiameter	= p_dDiameter
					cp.dMeasGap		= p_mltcp.GapPreset(1)
					cp.dFullStress	= p_dFullStressADVal
					cp.dPxPhase		= p_dFullStressPxPhase
					cp.dPyPhase		= p_dFullStressPyPhase
					cp.dPzPhase		= p_dFullStressPzPhase
					cp.iZeroPos		= p_iZPosition
					cp.iHPPosControl	= p_clSpec.spi.iHPPosCont
					cp.dMeasureStress	= p_clSpec.spi.dMeasureStress
					cp.iCalcType		= p_clSpec.spi.iGelTimeMethod
					cp.dDiffInterval	= p_clSpec.spi.dDiffInterval
					cp.iStrsSensMode	= m_iSensMode									'応力検知モード0,1,2
					cp.iAmpMode			= p_clSpec.spi.iAmpMode							'Z振動モード 0,1
					cp.dRCCBaseThickness = 0.0
					cp.dXYWeightCoef	= p_dNConv										'重量換算
					p_cp = cp															'計算パラメータを退避
					Select Case cp.iStrsSensMode
				    Case clSpec.eStrsSens.Circle	'円モード
						m_iCorrRet = datCal.CorrectRealProcVector( cp )
				    Case Else						'直線モード
						m_iCorrRet = datCal.CorrectRealProcVector( cp )
					End Select
					p_dMesGap = cp.dMesGap		'測定GAP計算値 モニタ用
					p_dtFile.p_head.dRealGap = cp.dMesGap								'実測定GAPをヘッダへセット

					m_iStartIdx += p_dtFile.iHosei.Length - m_iStartIdx					'開始インデックスの更新
					m_iCorrectCount += 1
				'End If
			'--------------------------------------------------
				p_dtFile.MesBlockCnt += 1                                               '受信ブロックカウンタ更新



				p_iDataCount += MEAS_BLOCK_DATA_NUM								'累積サンプル数更新
				'累積サンプル数が規定ピーク数を含んだサンプル数を超えたか確認 1周期余分にとる
				If p_iDataCount >= clb(m_idx).iSample + (MEAS_BLOCK_DATA_NUM*cp.dPeriod)	
					m_iCalJob = eClbJob.DATACAL									'>>> 補正・平均化処理へ
				End If
			End if

		Case eClbJob.DATACAL '補正・平均化処理
			'メモリ内にデータ有るので読む必要なし Dtf.RealTimeFileReadText(True)										'測定ファイルリード

			'補正処理
			'Dim dAlpha		As Double
			'Dim dPxPhase	As Double
			'Dim dPyPhase	As Double
			'Dim dPzPhase	As Double

			''条件の減衰比を取得
			'If 0 <> FullStressCalibValueGet(0, p_ZClbX, p_ZClbC, p_ZClbP, p_FClbX, p_FClbC, p_FClbP, p_dFullStressZero, p_dFullStressAttenuation, dAlpha, dPxPhase, dPyPhase, dPzPhase, m_iSensMode, clb(m_idx).dAmplitudeX, clb(m_idx).dPeriodX )
			'	p_dFullStressAttenuation = 1.0
			'End If
			Select Case m_iSensMode
			Case eStrsSens.Point
				datCal.AveTempClear(p_mltcp.PeriodZ(1), 1)			'クリア
			Case Else
				datCal.AveTempClear(p_mltcp.PeriodX(1), 1)			'クリア
			End Select
			p_dFullStressZero = 0
			p_dFullStressAttenuation = 1.0
			Dim cp As DataCal.stCalcPara = Nothing
			cp.iMeas		= If( p_iCalibMode = 0, eHoseiMeasMode.FullCalib, eHoseiMeasMode.ZeroCalib )
			cp.iMode		= eHoseiCalcMode.Batch
			cp.cts			= m_cts
			cp.clDt			= Dtf
			'Select Case p_clSpec.spi.iStressSensMode
			Select Case m_iSensMode
			Case eStrsSens.Point
				cp.dAmplitude	= p_mltcp.AmplitudeZ / 1000.0
				cp.dPeriod		= p_mltcp.PeriodZ(1)
			Case Else
				cp.dAmplitude	= p_mltcp.AmplitudeX / 1000.0
				cp.dPeriod		= p_mltcp.PeriodX(1)
			End Select
			'cp.dPeriod		= p_mltcp.PeriodX(1)
			'cp.dAmplitude	= p_mltcp.AmplitudeX / 1000.0
			cp.iSampleStart	= 0
			cp.iSampleNum	= Dtf.iHosei.Length
			cp.fltSet		= p_FltSetting
			cp.dXYWait		= p_mltcp.MesPreWait(1) + p_mltcp.MeasWaitTime(1)
			cp.dZeroStress	= p_dFullStressZero
			cp.dAttenuation	= p_dFullStressAttenuation
			cp.dDiameter	= p_dDiameter
			cp.dMeasGap		= p_mltcp.GapPreset(1)
			cp.dFullStress	= p_dFullStressADVal
			cp.dPxPhase		= p_dFullStressPxPhase
			cp.dPyPhase		= p_dFullStressPyPhase
			cp.dPzPhase		= p_dFullStressPzPhase
			cp.iZeroPos		= p_iZPosition
			cp.iCalcType		= lstCalcMethod.IndexOf(p_sCalcMethod)
			cp.dDiffInterval	= p_dDiffInterval
			cp.iStrsSensMode	= m_iSensMode
			cp.iAmpMode			= p_mltcp.AmpMode								'Z振動モード 0,1
			cp.dRCCBaseThickness = 0.0
			cp.dXYWeightCoef	= p_dNConv										'重量換算
			p_cp = cp															'計算パラメータを退避
			Select Case cp.iStrsSensMode
			Case clSpec.eStrsSens.Circle	'円モード
				m_iCorrRet = datCal.CorrectRealProcVector( cp )
			Case Else						'直線モード
				m_iCorrRet = datCal.CorrectRealProcVector( cp )
			End Select
			p_dMesGap = cp.dMesGap		'測定GAP計算値 モニタ用
			p_dtFile.p_head.dRealGap = cp.dMesGap								'実測定GAPをヘッダへセット

			Select Case m_iSensMode
			Case eStrsSens.Circle
				p_dtFile.p_head.dXAmp		=	p_mltcp.AmplitudeX / 1000.0
				p_dtFile.p_head.dXPrd		=	p_mltcp.PeriodX(1)
				p_dtFile.p_head.dYAmp		=	p_mltcp.AmplitudeY / 1000.0
				p_dtFile.p_head.dYPrd		=	p_mltcp.PeriodY(1)
				p_dtFile.p_head.dZAmp		=	0
				p_dtFile.p_head.dZPrd		=	0
			Case eStrsSens.Line
				p_dtFile.p_head.dXAmp		=	p_mltcp.AmplitudeX / 1000.0
				p_dtFile.p_head.dXPrd		=	p_mltcp.PeriodX(1)
				p_dtFile.p_head.dYAmp		=	0
				p_dtFile.p_head.dYPrd		=	0
				p_dtFile.p_head.dZAmp		=	0
				p_dtFile.p_head.dZPrd		=	0
			Case eStrsSens.Point
				p_dtFile.p_head.dXAmp		=	0
				p_dtFile.p_head.dXPrd		=	0
				p_dtFile.p_head.dYAmp		=	0
				p_dtFile.p_head.dYPrd		=	0
				p_dtFile.p_head.dZAmp		=	p_mltcp.AmplitudeZ / 1000.0
				p_dtFile.p_head.dZPrd		=	p_mltcp.PeriodZ(1)
			End Select
			p_dtFile.p_head.dHPTemp		=	p_mltcp.TempPreset(1)		'設定温度
			p_dtFile.p_head.dGapValue	=	p_mltcp.GapPreset(1)		'測定GAP
			p_dtFile.p_head.dXYActWait	=	p_mltcp.MesPreWait(1)		'XY動作待ち
			Dtf.RealTimeMeasDataFileSaveText(eMeasMode.Calib, False, m_cts, , , , , p_iCalibMode)			'補正データで上書き更新

			'一次元Double配列の交尾X％の平均値を算出
			'sbCalcDoubleAveragePercent(	Dtf.iHosei,		p_ClbAveSizeRate, m_iResultAve )				'補正値
			'clb(m_idx).iFullVal = m_iResultAve															'応力平均化結果をセット
			Select Case cp.iStrsSensMode
			Case clSpec.eStrsSens.Circle	'円モード
				sbCalcDoubleAverage2Period(	Dtf.iHosei,		clb(m_idx).dPeriodX, m_iResultAve)				'補正値
				clb(m_idx).iFullVal = m_iResultAve															'応力平均化結果をセット
				clb(m_idx).dMeasDate= Now()																	'測定日時をセット
				If p_iCalibMode = 0
					p_FMeasDateC = Format(clb(m_idx).dMeasDate, "yyyy/MM/dd HH:mm")
				Else
					p_ZMeasDateC = Format(clb(m_idx).dMeasDate, "yyyy/MM/dd HH:mm")
				End If
				sbCalcDoubleAverage2Period( Dtf.dXPosPhase,	clb(m_idx).dPeriodX, m_dPxPhaseAve )			'x位置位相差
				sbCalcDoubleAverage2Period( Dtf.dYPosPhase,	clb(m_idx).dPeriodX, m_dPyPhaseAve )			'y位置位相差
				clb(m_idx).dPPhaseCX	= m_dPxPhaseAve														'X位置位相差平均化結果をセット
				clb(m_idx).dPPhaseCY	= m_dPyPhaseAve														'Y位置位相差平均化結果をセット

			Case clSpec.eStrsSens.Line						'X直線モード
				sbCalcDoubleAverage2Period(	Dtf.iHosei,		clb(m_idx).dPeriodX, m_iResultAve)				'補正値
				clb(m_idx).iFullVal = m_iResultAve															'応力平均化結果をセット
				clb(m_idx).dMeasDate= Now()																	'測定日時をセット
				If p_iCalibMode = 0
					p_FMeasDateX = Format(clb(m_idx).dMeasDate, "yyyy/MM/dd HH:mm")
				Else
					p_ZMeasDateX = Format(clb(m_idx).dMeasDate, "yyyy/MM/dd HH:mm")
				End If
				sbCalcDoubleAverage2Period( Dtf.dXPosPhase,	clb(m_idx).dPeriodX, m_dPxPhaseAve )			'x位置位相差
				clb(m_idx).dPPhaseX		= m_dPxPhaseAve														'X位置位相差平均化結果をセット

			Case clSpec.eStrsSens.Point						'Z直線モード
				sbCalcDoubleAverage2Period(	Dtf.iHosei,		clb(m_idx).dPeriodZ, m_iResultAve)				'補正値
				clb(m_idx).iFullVal = m_iResultAve															'応力平均化結果をセット
				clb(m_idx).dMeasDate= Now()																	'測定日時をセット
				If p_iCalibMode = 0
					p_FMeasDateP = Format(clb(m_idx).dMeasDate, "yyyy/MM/dd HH:mm")
				Else
					p_ZMeasDateP = Format(clb(m_idx).dMeasDate, "yyyy/MM/dd HH:mm")
				End If
				sbCalcDoubleAverage2Period( Dtf.dXPosPhase,	clb(m_idx).dPeriodZ, m_dPxPhaseAve )			'z位置位相差
				clb(m_idx).dPPhaseZ		= m_dPxPhaseAve														'z位置位相差平均化結果をセット
			End Select


			m_idx += 1															'次の条件インデックス
			If m_idx > clb.GetUpperBound(0)
				'配列終了
				'm_iCalJob = eClbJob.FINISHING               '>>> 終了処理へ
				p_DispUpdate = 0
				m_iCalJob = eClbJob.DispUpDateWait			'>>> 次のTABへ
				sw2.Restart()
			Else
				p_DispUpdate = 0
				m_iCalJob = eClbJob.DispUpDateWait			'>>> 次の測定点へ
				sw2.Restart()
			End If

		Case eClbJob.DispUpDateWait
			If p_DispUpdate >= 2
				m_iCalJob = eClbJob.MEAS_SEL                '>>> 次の測定点へ
			ElseIf sw2.Elapsed.TotalSeconds > 10
			'ここで留まってしまう現象の回避
				m_iCalJob = eClbJob.MEAS_SEL                '>>> 次の測定点へ
			End if

		Case eClbJob.FINISHING '終了処理中
			If sw2.Elapsed.TotalSeconds > 1
				p_CaliblationState.iState = eCalibState.FINISHING           '終了中
				m_iCalJob = eClbJob.FINISH					'>>> 終了処理へ
			End If

		Case eClbJob.FINISH '終了処理
			m_iCalibTrig = 0
			m_iCalJob = eClbJob.IDLE                            '>>> アイドル中へ
		Case Else
			m_iCalJob = eClbJob.IDLE                            '>>> アイドル中へ
		End Select
		Return 0
	Catch ex As Exception
		Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
		Return -1
	End Try
End Function
Public Sub sbCalcIntAveragePercent( ByVal iSrc() As Int64, ByVal dRate As Double, ByRef iAve As Int64 )
	'平均対応配列作成
	Dim iAveSize	 As Int32 = iSrc.Length * dRate / 100.0
	Dim iAveStartidx As Int32 = iSrc.Length - iAveSize
	Dim iTemp()		 As int64 = Nothing
	'平均値計算
	Array.Resize( iTemp, iAveSize )											'平均算出用配列拡張
	Array.Copy(iSrc, iAveStartidx, iTemp, 0, iTemp.Length)					'開始インデックスから末尾までコピー
	iAve	= iTemp.Average()												'平均値
End Sub
''' <summary>
''' 配列後ろからdRateで切取り平均化
''' </summary>
''' <param name="dSrc"></param>
''' <param name="dRate"></param>
''' <param name="iAve"></param>
Public Sub sbCalcDoubleAveragePercent( ByVal dSrc() As Double, ByVal dRate As Double, ByRef iAve As Double )
	'平均対応配列作成
	'Dim iAveSize	 As Int32 = dSrc.Length * dRate / 100.0
	'Dim iAveStartidx As Int32 = dSrc.Length - iAveSize
	Dim iAveSize	 As Int32 = p_FlcvSample * dRate / 100.0
	Dim iAveStartidx As Int32 = dSrc.Length - iAveSize
	Dim dTemp()		 As Double = Nothing
	'平均値計算
	Array.Resize( dTemp, iAveSize )											'平均算出用配列拡張
	Array.Copy(dSrc, iAveStartidx, dTemp, 0, dTemp.Length)					'開始インデックスから末尾までコピー
	iAve	= dTemp.Average()												'平均値
End Sub

''' <summary>
''' 配列後ろから2周期を切取り平均化
''' </summary>
''' <param name="dSrc">データ配列</param>
''' <param name="dPeriod">周期</param>
''' <param name="iAve">平均値</param>
''' <param name="iPeriodNum">任意周期数</param>
Public Sub sbCalcDoubleAverage2Period( ByVal dSrc() As Double, ByVal dPeriod As Double, ByRef iAve As Double, Optional ByVal iPeriodNum As Int16 = cCalibCalPeriodNum )
Try
	'平均対応配列作成
	Dim iAveSize	 As Int32 = If(dPeriod=0, iPeriodNum * MEAS_BLOCK_DATA_NUM, dPeriod * iPeriodNum * MEAS_BLOCK_DATA_NUM)
	Dim iAveStartidx As Int32 = dSrc.Length - iAveSize - MEAS_BLOCK_DATA_NUM	'最終ブロックを除いて指定周期の開始インデックスを得る
	Dim dTemp()		 As Double = Nothing

	If iAveSize > dSrc.Length+MEAS_BLOCK_DATA_NUM
		iAveStartidx = 0
	End If
	'平均値計算
	Array.Resize( dTemp, iAveSize )											'平均算出用配列拡張
	Array.Copy(dSrc, iAveStartidx, dTemp, 0, dTemp.Length)					'開始インデックスからコピー
	'NAN 確認
	Dim dPre As Double
	For i=0 To dTemp.GetUpperBound(0)
		If Double.IsNan(dTemp(i))
			dTemp(i) = dPre
		Else
			dPre = dTemp(i)
		End If
	Next
	iAve	= dTemp.Average()												'平均値

Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub
''' <summary>
''' 配列後ろから2周期を切取り平均化
''' </summary>
''' <param name="iSrc">データ配列</param>
''' <param name="dPeriod">周期</param>
''' <param name="iAve">平均値</param>
''' <param name="iPeriodNum">任意周期数</param>
Public Sub sbCalcInbtegerAverage2Period( ByVal iSrc() As Int64, ByVal dPeriod As Double, ByRef iAve As Int64, Optional ByVal iPeriodNum As Int16 = cCalibCalPeriodNum )
Try
	'平均対応配列作成
	Dim iAveSize	 As Int32 = If(dPeriod=0, iPeriodNum * MEAS_BLOCK_DATA_NUM, dPeriod * iPeriodNum * MEAS_BLOCK_DATA_NUM)
	Dim iAveStartidx As Int32 = iSrc.Length - iAveSize - MEAS_BLOCK_DATA_NUM	'最終ブロックを除いて指定周期の開始インデックスを得る
	Dim dTemp()		 As Double = Nothing
	'平均値計算
	Array.Resize( dTemp, iAveSize )											'平均算出用配列拡張
	Array.Copy(iSrc, iAveStartidx, dTemp, 0, dTemp.Length)					'開始インデックスからコピー
	'NAN 確認
	Dim dPre As Double
	For i=0 To dTemp.GetUpperBound(0)
		If Double.IsNan(dTemp(i))
			dTemp(i) = dPre
		Else
			dPre = dTemp(i)
		End If
	Next
	iAve	= dTemp.Average()												'平均値

Catch ex As Exception

End Try
End Sub






Public P_ViscoCont As int32 = -1
Public Enum enflcvContRol
	NO_WORK			= -1
	VISC_TBL_CLEAR	= 0
	VISC_TBL_ADD
End Enum
Public p_iFlcvMeasIdx			As Int32  = 0				'測定データインデックス
Public iViscoState				As Int16  = 0

''' <summary>
''' フローカーブ測定(速度・温度依存)を行う制御
''' </summary>
''' <param name="sw2">制御に使用する時間計測</param>
''' <param name="Dtf">測定データクラス</param>
''' <returns></returns>
Public Function FlcvControl1(ByRef sw2 As Stopwatch, ByRef Dtf As DataFile) As Int32
	Try
		Select Case m_iCalJob
		Case eClbJob.IDLE 'アイドル中
			If m_iCalibTrig = 1
				m_idx = 0
				Dtf.ReadBufferClear()
				Dtf.ViscosityBufferClear()							'測定結果バッファクリア
				Dtf.ViscosityBufferAllocate(p_FlcvChgVal.Length)		'測定結果バッファ確保
				p_iDataCount = 0
				Dtf.uVis1 = Nothing
				'データファイル名の決定
				Select Case p_FlcvMeasMode
				Case eViscoMeaure.SpeedDep
				'速度依存
					Dtf.MesFileNumber = 0													'ファイル連番クリア
					Dtf.SetDataFileName = Dtf.sMeasindCode(eMesCD.FL1) & "-" & Format(Now, "yyyyMMddHHmmss")		'フローカーブ（速度依存）
					MeasureHeaderInfoSet(eMeasMode.FlowCurve_Speed)							'測定開始時のファイルヘッダ情報のセット
				Case eViscoMeaure.TempDep
				'速度依存
					Dtf.MesFileNumber = 0													'ファイル連番クリア
					Dtf.SetDataFileName = Dtf.sMeasindCode(eMesCD.FL3) & "-" & Format(Now, "yyyyMMddHHmmss")		'フローカーブ（温度依存）
					MeasureHeaderInfoSet(eMeasMode.FlowCurve_Temp)							'測定開始時のファイルヘッダ情報のセット
				End Select
				m_iCalJob = eClbJob.VISC_TBL_CL_WAIT
				iViscoState = eViscoState.COND_CONF
			End If

		Case eClbJob.VISC_TBL_CL_WAIT  '待ち
			if P_ViscoCont = enflcvContRol.NO_WORK
				P_ViscoCont = enflcvContRol.VISC_TBL_CLEAR
				m_iCalJob = eClbJob.MEAS_SEL
			End If

		Case eClbJob.MEAS_SEL  '測定選択確認
			If m_idx > p_FlcvChgVal.GetUpperBound(0)
				'配列終了
				sw2.Restart()
				m_iCalJob = eClbJob.FINISHING						'>>> 終了中処理へ
			Else
				p_iFlcvMeasIdx = m_idx								'現在の測定データインデックスをUIへ伝える
				p_iDataCount = 0
				m_iCalJob = eClbJob.XYMODE_SEL
			End If

		Case eClbJob.XYMODE_SEL '動作設定
			Select Case p_FlcvMeasMode
			Case eViscoMeaure.SpeedDep
			'速度依存
				p_mltcp.AmplitudeX		= p_FlcvChgVal(m_idx) * 1000.0							'X軸振幅
				p_mltcp.AmplitudeY		= If(p_FlcvActMode=0, p_FlcvChgVal(m_idx) * 1000.0, 0)	'Y軸振幅指定
			Case eViscoMeaure.TempDep
			'温度依存
				p_mltcp.TempPreset(1)	= p_FlcvChgVal(m_idx)									'HP温度
				p_mltcp.Temp2Preset(1)	= p_FlcvChgVal(m_idx)									'PB温度
				p_mltcp.Temp3Preset(1)	= p_clSpec.SGTempGet(p_mltcp.TempPreset(1))
				p_mltcp.AmplitudeX		= p_FlcvAmplitude * 1000.0								'X軸振幅
				p_mltcp.AmplitudeY		= If(p_FlcvActMode=0, p_FlcvAmplitude * 1000.0, 0)		'Y軸振幅
			End Select
			p_mltcp.PeriodX(1) = p_FlcvPeriod													'X軸周期
			p_mltcp.PeriodY(1) = p_FlcvPeriod													'Y軸周期

			Array.Resize(m_iStres, 0)								'配列準備
			'Dtf.ReadBufferClear()
			'If p_mltcp.bMesDataEnable Andalso p_mltcp.isStatXYAction Then
			If p_mltcp.bMesDataEnable  Then
				sw2.Restart()
				m_iCalJob = eClbJob.STAB_WAIT                           '>>> 本体の安定待ち
				iViscoState = eViscoState.STAB_WAIT
			End If

		Case eClbJob.STAB_WAIT '本体安定待ち
			If m_idx = 0
				Array.Resize(m_iStres, m_iStres.Length + MEAS_BLOCK_DATA_NUM)   '配列確保
				p_CaliblationState.iState = eCalibState.DATA_TRANS				'データ取得
				m_iCalJob = eClbJob.XYACT_VERIFICATION							'>>> XY動作確認
				iViscoState = eViscoState.DATA_TRANS

			ElseIf p_mltcp.isStatXYAction
				Select Case p_FlcvMeasMode
				Case eViscoMeaure.SpeedDep
				'速度依存
					'If sw2.Elapsed.TotalSeconds > 3		'振幅安定3秒待ち
					If p_mltcp.isStatXYAction		'振幅安定3秒待ち
						Array.Resize(m_iStres, m_iStres.Length + MEAS_BLOCK_DATA_NUM)   '配列確保
						p_CaliblationState.iState = eCalibState.DATA_TRANS				'データ取得
						m_iCalJob = eClbJob.XYACT_VERIFICATION							'>>> XY動作確認
						iViscoState = eViscoState.DATA_TRANS
					End If
				Case eViscoMeaure.TempDep
				'温度依存（±0.5℃範囲待ち）
					if	p_mltcp.HeatTempAct		>= p_FlcvChgVal(m_idx) - 0.2 AndAlso p_mltcp.HeatTempAct	<= p_FlcvChgVal(m_idx) + 0.2 AndAlso _
						p_mltcp.HeatTemp2Act	>= p_FlcvChgVal(m_idx) - 0.2 AndAlso p_mltcp.HeatTemp2Act	<= p_FlcvChgVal(m_idx) + 0.2
						p_CaliblationState.iState = eCalibState.DATA_TRANS				'データ取得
						m_iCalJob = eClbJob.XYACT_VERIFICATION							'>>> XY動作確認
						iViscoState = eViscoState.DATA_TRANS
					End If 
				End Select
			End If


		Case eClbJob.XYACT_VERIFICATION								'XY動作確認
			Dim dAlpha		As Double
			Select Case p_FlcvMeasMode
			Case eViscoMeaure.SpeedDep
			'速度依存
				'条件のゼロストレスを取得
				If FullStressCalibValueGet(2, p_ZClbX, p_ZClbC, p_ZClbP, p_FClbX, p_FClbC, p_FClbP, p_dFullStressZero, p_dFullStressAttenuation, _
											dAlpha, p_dFullStressPxPhase, p_dFullStressPyPhase, p_dFullStressPzPhase, p_FlcvActMode, p_FlcvChgVal(m_idx), p_FlcvPeriod ) <> 0
				End If

				'条件のフルストレスを取得
				If FullStressCalibValueGet(0, p_ZClbX, p_ZClbC, p_ZClbP, p_FClbX, p_FClbC, p_FClbP, p_dFullStressADVal, p_dFullStressAttenuation, _
											dAlpha, p_dFullStressPxPhase, p_dFullStressPyPhase, p_dFullStressPzPhase, p_FlcvActMode, p_FlcvChgVal(m_idx), p_FlcvPeriod ) <> 0
				End If
			Case eViscoMeaure.TempDep
			'温度依存
				'条件のゼロストレスを取得
				If FullStressCalibValueGet(2, p_ZClbX, p_ZClbC, p_ZClbP, p_FClbX, p_FClbC, p_FClbP, p_dFullStressZero, p_dFullStressAttenuation, _
											dAlpha, p_dFullStressPxPhase, p_dFullStressPyPhase, p_dFullStressPzPhase, p_FlcvActMode, p_FlcvAmplitude, p_FlcvPeriod ) <> 0
				End If

				'条件のフルストレスを取得
				If FullStressCalibValueGet(0, p_ZClbX, p_ZClbC, p_ZClbP, p_FClbX, p_FClbC, p_FClbP, p_dFullStressADVal, p_dFullStressAttenuation, _
											dAlpha, p_dFullStressPxPhase, p_dFullStressPyPhase, p_dFullStressPzPhase, p_FlcvActMode, p_FlcvAmplitude, p_FlcvPeriod ) <> 0
				End If
			End Select
			m_iCorrRet = 0
			m_iStartIdx = 0
			m_iCorrectCount = 0
			m_iPeriodSample = p_mltcp.PeriodX(1) * 1000 / MEAS_TIME_RESOLUTION			'1周期分のサンプル数計算
			p_dtFile.DataCount = 0														'蓄積データカウンタクリア

			
			
			
			
			m_iCalJob = eClbJob.DATAGET								'>>> データ取得・結果セット


		Case eClbJob.DATAGET 'データ取得
			'If p_mltcp.bMesDataEnable Andalso p_mltcp.isStatXYAction Then
			If p_mltcp.bMesDataEnable 
			'測定データ有効
				Dtf.RealTimeMeasDataFileSaveText(eMeasMode.FlowCurve_Speed, True, m_cts)		'測定データ蓄積ファイルセーブはしない カウンタ更新だけ

			'補正処理(リアルタイム) ----------------------
				'If p_dtFile.DataCount-m_iStartIdx >= m_iPeriodSample 
				'If p_dtFile.iHosei.Length-m_iStartIdx >= m_iPeriodSample
				'If p_dtFile.DataCount >= m_iPeriodSample
				'1周期分のデータ取得まで待つ
					if m_iCorrectCount = 0
					'初回は0インデックスから
						m_iStartIdx = 0
						datCal.AveTempClear(p_mltcp.PeriodX(1), 0, m_idx)			'前回Temp_3の末尾退避をクリア
					Else
					End If
					'リアルタイム中間値補正
					'Z決定位置をセット
					If p_mltcp.ZConfirmPos<>0
						p_dtFile.p_head.dZAfterPos = Ctype(p_mltcp.ZConfirmPos, Double) / 10.0
					End If
					Dim cp As DataCal.stCalcPara = Nothing
					cp.iMeas		= eHoseiMeasMode.FlowCurve
					cp.iMode		= eHoseiCalcMode.RealTime
					cp.cts			= m_cts
					cp.clDt			= Dtf
					cp.dPeriod		= p_mltcp.PeriodX(1)
					'cp.dPeriod		= p_mltcp.PeriodX(1)
					cp.dAmplitude	= p_FlcvChgVal(m_idx)
					cp.iSampleStart	= m_iStartIdx
					cp.iSampleNum	= Dtf.iHosei.Length - m_iStartIdx
					cp.fltSet		= p_FltSetting
					cp.dXYWait		= p_mltcp.MesPreWait(1) + p_mltcp.MeasWaitTime(1)
					cp.dZeroStress	= p_dFullStressZero
					cp.dAttenuation	= p_dFullStressAttenuation
					cp.dDiameter	= p_FlcvPbDiameter
					cp.dMeasGap		= p_mltcp.GapPreset(1)
					cp.dFullStress	= 0
					cp.dPxPhase		= p_dFullStressPxPhase
					cp.dPyPhase		= p_dFullStressPyPhase
					cp.dPzPhase		= p_dFullStressPzPhase
					cp.iZeroPos		= p_iZPosition
					cp.iHPPosControl	= eHCont.Gap
					cp.dMeasureStress	= p_clSpec.spi.dMeasureStress
					cp.iCalcType		= eCalcMethod.THR
					cp.dDiffInterval	= p_dDiffInterval
					cp.iStrsSensMode	= p_FlcvActMode									'応力検知モード0,1,2
					cp.iAmpMode			= eAmpMode.AmpConst								'Z振動モード 0,1
					cp.dRCCBaseThickness = 0.0
					cp.dXYWeightCoef	= p_dNConv										'重量換算
					p_cp = cp															'計算パラメータを退避
					Select Case p_FlcvActMode
				    Case 0	'円モード
						m_iCorrRet = datCal.CorrectRealProcVector( cp )
				    Case 1	'Xモード
						m_iCorrRet = datCal.CorrectRealProcVector( cp )
					End Select
					p_dMesGap = cp.dMesGap		'測定GAP計算値 モニタ用
					p_dtFile.p_head.dRealGap = cp.dMesGap								'実測定GAPをヘッダへセット

					p_FlcvArrayUpdate = 1
#If 0
					'粘度関係計算
					Dim uVp As Datafile.ViscosityCalcParameter
					uVp.dPeriod			= p_FlcvPeriod									'設定周期
					uVp.dAmplitude		= p_FlcvChgVal(0) / 10^3						'設定振幅(m)
					uVp.dMeasGap		= p_mltcp.GapPreset(1) / 10^3					'測定GAP(m)
					uVp.dProbeSquare	= Math.PI * ((p_FlcvPbDiameter/10^3) / 2)^2		'プローブ面積(m2)
					uVp.dVelocityRate	= 1.0											'せん断速度補正係数

					'粘度計算処理
					Array.Resize(p_dtFile.dViscosity,		p_dtFile.iHosei.Length)
					Array.Resize(p_dtFile.dShearStress,		p_dtFile.iHosei.Length)
					Array.Resize(p_dtFile.dShearVelocity,	p_dtFile.iHosei.Length)
					For i = 0 To p_dtFile.iHosei.GetUpperBound(0)
						p_dtFile.dViscosity(i)		= Double.NaN
						uVp.dMeasStress				= p_dtFile.iHosei(i) / 10^6			'測定応力(N)
						modDataCal.ViscosityCalc( uVp )								'粘度計算
						p_dtFile.dViscosity(i)		= uVp.dViscosity					'粘度
						p_dtFile.dShearStress(i)	= uVp.dShearStress					'せん断応力
						p_dtFile.dShearVelocity(i)	= uVp.dShearVelocity				'せん断速度
					Next i
#Else
					Array.Resize(p_dtFile.dShearVelocity,	p_dtFile.iHosei.Length)
					Array.Copy(p_dtFile.dShearVelo, p_dtFile.dShearVelocity, p_dtFile.iHosei.Length)
#End If
					p_FlcvArrayUpdate = 0

					m_iStartIdx += p_dtFile.iHosei.Length - m_iStartIdx					'開始インデックスの更新
					'm_iStartIdx = 0					'開始インデックスの更新
					m_iCorrectCount += 1

'Debug.Print(datCal.m_dLatBaseZPos)
				'End If
'debug.Print(p_dtFile.iHosei.Length & Space(8) & p_iDataCount & Space(8) & p_dtFile.DataCount & Space(8) & m_iStartIdx) 

			'--------------------------------------------------
				p_dtFile.MesBlockCnt += 1                                               '受信ブロックカウンタ更新

				p_iDataCount += MEAS_BLOCK_DATA_NUM								'累積サンプル数更新
				Dim iFlcvSample As int32 = 0
				If m_idx = 0
					iFlcvSample = p_FlcvSample + (p_mltcp.MesPreWait(1) + p_mltcp.MeasWaitTime(1))*MEAS_BLOCK_DATA_NUM
				Else
					iFlcvSample = p_FlcvSample 
				End If
				If p_iDataCount >= iFlcvSample									'累積サンプル数が規定サンプル数を超えたか確認
					m_iCalJob = eClbJob.DATACAL									'>>> 補正・平均化処理へ
				End If
'Debug.Print("p_iDataCount=" & p_iDataCount & "  iFlcvSample=" & iFlcvSample)
			End if

		Case eClbJob.DATACAL '補正・平均化処理
			'補正処理(バッチ)
			datCal.AveTempClear(p_mltcp.PeriodX(1), 1, m_idx)			'前回Temp_3の末尾退避をクリア
			Dim cp As DataCal.stCalcPara = Nothing
			cp.iMeas		= eHoseiMeasMode.FlowCurve
			cp.iMode		= eHoseiCalcMode.Batch
			cp.cts			= m_cts
			cp.clDt			= Dtf
			cp.dPeriod		= p_mltcp.PeriodX(1)
			cp.dAmplitude	= p_mltcp.AmplitudeX / 1000.0
			cp.iSampleStart	= 0
			cp.iSampleNum	= Dtf.iHosei.Length
			cp.fltSet		= p_FltSetting
			cp.dXYWait		= p_mltcp.MesPreWait(1) + p_mltcp.MeasWaitTime(1)
			cp.dZeroStress	= p_dFullStressZero
			cp.dAttenuation	= p_dFullStressAttenuation
			cp.dDiameter	= p_FlcvPbDiameter
			cp.dMeasGap		= p_mltcp.GapPreset(1)
			cp.dFullStress	= 0
			cp.dPxPhase		= p_dFullStressPxPhase
			cp.dPyPhase		= p_dFullStressPyPhase
			cp.dPzPhase		= p_dFullStressPzPhase
			cp.iZeroPos		= p_iZPosition
			cp.iHPPosControl	= eHCont.Gap
			cp.dMeasureStress	= p_clSpec.spi.dMeasureStress
			cp.iCalcType		= eCalcMethod.THR
			cp.dDiffInterval	= p_clSpec.spi.dDiffInterval
			cp.iAmpMode			= eAmpMode.AmpConst								'Z振動モード 0,1
			cp.iStrsSensMode	= p_FlcvActMode									'応力検知モード0,1,2
			cp.dRCCBaseThickness = 0.0
			cp.dXYWeightCoef	= p_dNConv										'重量換算
			p_cp = cp															'計算パラメータを退避
			Select Case p_FlcvActMode
			Case 0	'円モード
				m_iCorrRet = datCal.CorrectRealProcVector( cp )
			Case 1	'Xモード
				m_iCorrRet = datCal.CorrectRealProcVector( cp )
			End Select
			''Dtf.RealTimeMeasDataFileSaveText(eMeasMode.Viscosity, False)				'補正データで上書き更新
			''Dtf.RealTimeMeasDataFileSaveText(eMeasMode.FlowCurve_Speed, False, m_cts, , , , , )	'測定データ蓄積ファイルセーブ
			p_dMesGap = cp.dMesGap		'測定GAP計算値 モニタ用
			p_dtFile.p_head.dRealGap = cp.dMesGap								'実測定GAPをヘッダへセット

			Dim uVp As Datafile.ViscosityCalcParameter
			sbCalcDoubleAveragePercent( Dtf.iHosei,		p_FlcvAveSizeRate, uVp.dMeasStress )	'応力補正値(uN)
			sbCalcDoubleAveragePercent( Dtf.dViscosity,	p_FlcvAveSizeRate, uVp.dViscosity )		'粘度
			sbCalcDoubleAveragePercent( Dtf.dXPosPhase,	p_FlcvAveSizeRate, uVp.dXPosPhase )		'位相角
			sbCalcDoubleAveragePercent( Dtf.dSplStrain,	p_FlcvAveSizeRate, uVp.dSplStrain )		'サンプル歪
			sbCalcDoubleAveragePercent( Dtf.dShearMod,	p_FlcvAveSizeRate, uVp.dShearMod )		'せん断弾性率(Pa)
			sbCalcDoubleAveragePercent( Dtf.dShearVelo,	p_FlcvAveSizeRate, uVp.dShearVelocity )	'せん断速度(1/S)
			sbCalcDoubleAveragePercent( Dtf.dXStrgMod,	p_FlcvAveSizeRate, uVp.dXStrgMod )		'X貯蔵弾性率(Pa)
			sbCalcDoubleAveragePercent( Dtf.dXElasMod,	p_FlcvAveSizeRate, uVp.dXElasMod )		'X損失弾性率(Pa)
			sbCalcDoubleAveragePercent( Dtf.dXLossTan,	p_FlcvAveSizeRate, uVp.dXLossTan )		'X損失正接
			sbCalcIntAveragePercent( Dtf.iTempBuf,		p_FlcvAveSizeRate, uVp.dHPTemp )		'HP温度
			sbCalcIntAveragePercent( Dtf.iTempBuf2,		p_FlcvAveSizeRate, uVp.dPBTemp )		'PB温度
			uVp.dHPTemp /= 10
			uVp.dPBTemp /= 10
			uVp.dPeriod			= p_FlcvPeriod									'設定周期
			Select Case p_FlcvMeasMode
			Case eViscoMeaure.SpeedDep
			'速度依存
				uVp.dAmplitude	= p_FlcvChgVal(m_idx) / 10^3					'設定振幅(m)
			Case eViscoMeaure.TempDep
			'温度依存
				uVp.dAmplitude	= p_FlcvAmplitude / 10^3						'設定振幅(m)
				uVp.dTemp		= p_FlcvChgVal(m_idx)							'設定温度(℃)
			End Select
			uVp.dMeasGap		= p_mltcp.GapPreset(1) / 10^3					'測定GAP(m)
			uVp.dMeasStress		/= 10^6											'測定応力(N)
			uVp.dProbeSquare	= Math.PI * ((p_FlcvPbDiameter/10^3) / 2)^2		'プローブ面積(m2)
			uVp.dVelocityRate	= 1.0											'せん断速度補正係数
#If 0
			'粘度計算処理
			modDataCal.ViscosityCalc( uVp )
#End If
			p_uVp = uVp															'UIへ渡す
			'ファイルデータ列をセット
			if isNothing(Dtf.uVis1)
				Array.Resize(Dtf.uVis1, 1)
			Else
				Array.Resize(Dtf.uVis1, Dtf.uVis1.Length + 1)
			End If
			Dtf.uVis1(m_idx) = uVp
			
			Select Case p_FlcvMeasMode
			Case eViscoMeaure.SpeedDep
			'速度依存
				Dtf.RealTimeMeasDataFileSaveText( eMeasMode.FlowCurve_Speed, False, m_cts )	'ファイル上書き保存(粘度-速度依存)
			Case eViscoMeaure.TempDep
			'温度依存
				Dtf.RealTimeMeasDataFileSaveText( eMeasMode.FlowCurve_Temp, False, m_cts )	'ファイル上書き保存(粘度-温度依存)
			End Select
			P_ViscoCont = enflcvContRol.VISC_TBL_ADD							'データ行追加
			m_iCalJob	= eClbJob.DispUpDateWait								'表示更新へ

		Case eClbJob.DispUpDateWait
			If P_ViscoCont = enflcvContRol.NO_WORK
			'表示更新完了
				m_idx += 1														'次の条件インデックス
				If m_idx > p_FlcvChgVal.GetUpperBound(0)
					'配列終了
					p_CaliblationState.iState = eCalibState.FINISHING           '終了中
					m_iCalJob = eClbJob.FINISHING               '>>> 終了処理へ
					sw2.Restart()
				Else
					Dtf.ResizeMeasArray(0)						'測定値蓄積バッファクリア
					iViscoState = eViscoState.COND_CONF
					m_iCalJob = eClbJob.MEAS_SEL                '>>> 次の測定点へ
				End If
			End If

		Case eClbJob.FINISHING '終了処理中
			If sw2.Elapsed.TotalSeconds > 1
				m_iCalJob = eClbJob.FINISH					'>>> 終了処理へ
				iViscoState = eViscoState.FINISHING
				p_RTM(0).p_iZeroAdjOk = 0
			End If

		Case eClbJob.FINISH '終了処理
			m_iCalibTrig = 0
			m_iCalJob = eClbJob.IDLE                            '>>> アイドル中へ

		Case Else
			m_iCalJob = eClbJob.IDLE                            '>>> アイドル中へ
		End Select
		Return 0

	Catch ex As Exception
		Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
		Return -1
	End Try
End Function




'■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
'   プライベート関数
'■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
''' <summary>
''' 100%応力AD値測定の計算処理
''' </summary>
''' <param name="m_iStres">対象配列</param>
''' <param name="iResult">計算結果</param>
''' <returns>0：正常終了 -1:例外発生 -2：ゼロ除算</returns>
Private Function stressAverage(ByRef m_iStres() As Int64, ByRef iResult As Int64) As Int32
	Try
		Dim i As Int32
		Dim iTotal As Int64 = 0
		Dim iStandard As Int64 = 0
		Dim iD As Int64
		'先頭1000個の平均を基準にする
		For i = 0 To 999
			iTotal += m_iStres(i)
		Next i
		iStandard = iTotal / 1000
		iTotal = 0
		For i = 0 To m_iStres.GetUpperBound(0)
			If m_iStres(i) <> 0
				iTotal += m_iStres(i) - iStandard
			'Else
			'	Exit For
			End If
		Next i
		If i <> 0 
			iD = iTotal / (i - 1)
		Else
			Return -2		'ゼロ除算
		End if
		iResult = iStandard + iD
		Return 0
	Catch ex As Exception
		Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
		Return -1
	End Try
End Function
#If 0
''' <summary>
''' 10周期分の指定区間の変化量を計算し安定度判定を行う
''' </summary>
''' <param name="iObservSectionSpanNum">指定区間サンプル数</param>
''' <param name="dPeriod">測定周期[秒]</param>
''' <param name="dStability">設定安定度[%]</param>
''' <returns>True:安定を確認した False:安定確認できていない</returns>
Private Function fnCheckMeasureEndForChangeValue(byVal iObservSectionSpanNum As Int16, byVal dPeriod As Double, ByVal dStability As Double) As Boolean
	Dim i As int32
	Dim j As int32
	Dim k As int32
	Dim iMax As Int64 = p_dtFile.iHoseiDif.Max
	Dim iMin As Int64 = p_dtFile.iHoseiDif.Min
	Dim iSpan As Int64 = Math.Abs(iMax - iMin)
	Dim iAveArray() As Int64 = Nothing
	Dim dAveOfPeriod() As Double = Nothing
	Dim dAveDif() As Double = Nothing

	Try
		'確認可能なサンプル数が存在するか
		if p_dtFile.iHoseiDif.GetUpperBound(0) <= p_SampleNumForPeriod
			Return False
		End If
		Array.Resize(iAveArray, p_SampleNumForPeriod)           '安定度を見る配列確保
		Array.Resize(dAveOfPeriod, iObservSectionSpanNum)
		Array.Resize(dAveDif, iObservSectionSpanNum - 1)
		For k = dAveOfPeriod.GetUpperBound(0) To dAveOfPeriod.GetLowerBound(0) Step -1
			If p_dtFile.iHoseiDif.GetUpperBound(0) - (p_SampleNumForPeriod * k) > 0
				If k > 0
					j = 0
					For i = p_dtFile.iHoseiDif.GetUpperBound(0) - (p_SampleNumForPeriod * k) To p_dtFile.iHoseiDif.GetUpperBound(0) - (p_SampleNumForPeriod * (k - 1)) - 1
						iAveArray(j) = p_dtFile.iHoseiDif(i) - iMin
						j += 1
					Next i
					dAveOfPeriod(k) = iAveArray.Average                 '10周期区間平均値
				Else
					dAveOfPeriod(k) = dAveOfPeriod(k + 1)
				End If
			Else
				Return False
			end if
		Next k
		'指定周期*n間の差分をセット
		i = 0
		For k = 0 To dAveDif.GetUpperBound(0)
			dAveDif(k) = math.Abs(dAveOfPeriod(i) - dAveOfPeriod(i + 1))
			i += 1
			'If i > dAveOfPeriod.GetUpperBound(0)
			'	Exit For
			'End If
		Next
		'差分の平均値が設定以下なら安定確認
		Dim a As Double = math.abs(dAveDif.Max - dAveDif.Min) / iSpan * 100
		p_StabilityForMeasEnd = a
		If a < dStability
			Return True
		End If
	Catch ex As Exception
		Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	End Try
	Return False
End Function
#end if

''' <summary>
''' メモリ内に残っている測定データを元に中間値補正-フィルタリング-ゲルタイム計算を行う
''' ->測定ファイルの末尾に連番付きで新規保存
''' </summary>
''' <param name="iUpdate">データファイルアクセス 0:新規 1:上書き 2:なし</param>
''' <param name="cp">stCalcPara</param>
Public sub ReCalcProc(ByVal iUpdate As Int16, ByVal cp As DataCal.stCalcPara)
	Dim iRet As Int32
	Dim i As int32
	Dim j As int32
	Dim sFile As String
	Dim sFileOrg As String
	Dim sFileNew As String
	Dim sBuf() As String
	Dim dVis	As Double = 0
	Dim dTemp	As Double = 0

	'測定データ メモリ内存在？
	If IsNothing( p_dtFile.iOxBuf )
		Return
	End If
	'中間値補正処理
	If cp.iStrsSensMode=eStrsSens.Circle
		m_iCorrRet = datCal.CorrectRealProcVector( cp )
	Else
		m_iCorrRet = datCal.CorrectRealProcVector( cp )
	End If
	p_dMesGap = cp.dMesGap		'測定GAP計算値 モニタ用
	p_dtFile.p_head.dRealGap = cp.dMesGap								'実測定GAPをヘッダへセット


	'== ゲルタイム計算処理 ==
	Select Case p_sCalcMethod
	Case lstCalcMethod(eCalcMethod.THR)
		iRet = p_dtFile.CalcGelTime(eCalcMethod.THR,	frmMain.p_iSelectGItem, p_dGelTime, p_dToqueThreshold, p_dFullStressADVal, p_dAprxRangeMin, p_dAprxRangeMax,,,,p_cts)

	Case lstCalcMethod(eCalcMethod.SLOPE)
		iRet = p_dtFile.CalcGelTime(eCalcMethod.SLOPE,	frmMain.p_iSelectGItem, p_dGelTime, p_dToqueThreshold, p_dFullStressADVal, p_dAprxRangeMin, p_dAprxRangeMax,,,,p_cts)

	Case lstCalcMethod(eCalcMethod.EXTRA)
	Case lstCalcMethod(eCalcMethod.PHASE)
		iRet = p_dtFile.CalcGelTime(eCalcMethod.PHASE,	frmMain.p_iSelectGItem, p_dGelTime, p_dToqueThreshold, 90.0 * Math.PI / 180, p_dAprxRangeMin, p_dAprxRangeMax,,,,p_cts)
	Case lstCalcMethod(eCalcMethod.MELT)
		If p_dtFile.p_head.sTempProfile=""
		'定温MELT
			'最低粘度とその時の温度
			iRet = p_dtFile.CalcGelTime(eCalcMethod.MELT,	frmMain.p_iSelectGItem, p_dGelTime, 0, 0, p_dAprxRangeMin, p_dAprxRangeMax, p_dtFile.p_head.sMelWaitTime+3*p_dtFile.p_head.dXPrd, p_dMeltVisco, p_dMeltTemp,p_cts)
		Else
			'温度プロファイル読込
			If frmOptions.LoadRTGSET(p_dtFile.p_head.sTempProfile, p_sEquipName, p_rtgset)
			'温度プロファイル 読込OK
				p_dtFile.p_head.sMelStartTemp	= p_rtgset.mdbr(0).dHPTemp			'初期温度
				p_dtFile.p_head.sMelEndTemp		= p_rtgset.mdbr(1).dHPTemp			'終了温度
				p_dtFile.p_head.sMelWaitTime	= p_rtgset.dTiming(0)				'待ち時間
				Dim dRizeTime As Double			= p_rtgset.dTiming(1)				'上昇時間
				Dim dSpeed	As Double			= (p_rtgset.mdbr(1).dHPTemp - p_rtgset.mdbr(0).dHPTemp) / (dRizeTime/60.0)
				p_dtFile.p_head.sMelSpeed		= dSpeed.ToString("F1")				'上昇速度(℃/min)
				'最低粘度とその時の温度
				iRet = p_dtFile.CalcGelTime(eCalcMethod.MELT,	frmMain.p_iSelectGItem, p_dGelTime, 0, 0, p_dAprxRangeMin, p_dAprxRangeMax, p_rtgset.dTiming(0), p_dMeltVisco, p_dMeltTemp,p_cts)
			End If
		End If

#if 0
		'温度プロファイル読込
		If frmOptions.LoadRTGSET(p_dtFile.p_head.sTempProfile, p_rtgset)
		'温度プロファイル 読込OK
			p_dtFile.p_head.sMelStartTemp	= p_rtgset.mdbr(0).dHPTemp			'初期温度
			p_dtFile.p_head.sMelEndTemp		= p_rtgset.mdbr(1).dHPTemp			'終了温度
			p_dtFile.p_head.sMelWaitTime	= p_rtgset.dTiming(0)				'待ち時間
			Dim dRizeTime As Double			= p_rtgset.dTiming(1)				'上昇時間
			Dim dSpeed	As Double			= (p_rtgset.mdbr(1).dHPTemp - p_rtgset.mdbr(0).dHPTemp) / (dRizeTime/60.0)
			p_dtFile.p_head.sMelSpeed		= dSpeed.ToString("F1")				'上昇速度(℃/min)
			'最低粘度とその時の温度
			iRet = p_dtFile.CalcGelTime(eCalcMethod.MELT,	frmMain.p_iSelectGItem, p_dGelTime, 0, 0, p_dAprxRangeMin, p_dAprxRangeMax, p_rtgset.dTiming(0), p_dMeltVisco, p_dMeltTemp,p_cts)
		End If
#end if
	End Select
	p_iGelTimeJudge = 2				'判定結果を無効
	Select Case iRet
	Case 0		'ゲルタイム計算OK
		Select Case p_sCalcMethod
		Case lstCalcMethod(eCalcMethod.THR), lstCalcMethod(eCalcMethod.SLOPE), lstCalcMethod(eCalcMethod.EXTRA), lstCalcMethod(eCalcMethod.PHASE)
			p_dtfile.p_head.dGelTime = p_dGelTime					'データファイルヘッダ用ゲルタイム
			Select Case p_dGelTime
			Case p_dJudgeRangeMin To p_dJudgeRangeMax
				p_iGelTimeJudge = 1			'OK
				p_dtFile.p_head.sGelTimeJudge = "OK"
			Case Else
				If (p_dJudgeRangeMax - p_dJudgeRangeMin) <> 0
					p_iGelTimeJudge = 0		'NG
					p_dtFile.p_head.sGelTimeJudge = "NG"
				Else
					p_iGelTimeJudge = 2		'NA
					p_dtFile.p_head.sGelTimeJudge = "NA"
				End If
			End Select
		Case lstCalcMethod(eCalcMethod.MELT)
			p_iGelTimeJudge = 2		'NA
			p_dtFile.p_head.sGelTimeJudge = "NA"
			p_dtfile.p_head.dGelTime = p_dGelTime					'データファイルヘッダ用ゲルタイム
			p_dtfile.p_head.sMelTemperatureRslt		= p_dMeltTemp.ToString("F1")
			p_dtfile.p_head.sMelViscosityRslt		= p_dMeltVisco.ToString("F1")
		End Select
	Case -2		'ゲルタイム計算不可
		p_dtfile.p_head.dGelTime		= -1.0					'ゲルタイム
	case else	'例外エラー発生
		p_dtfile.p_head.dGelTime		= -1.0					'ゲルタイム
	End Select



	If iUpdate <> 2
		'補正データ込みで上書き更新
		sFile = Path.GetFileNameWithoutExtension( p_dtFile.GetDataFileName() )

			i = sFile.IndexOf("_")
			If i <> -1 
				sBuf = sFile.Split("_")
				sFileOrg = sBuf(0)
				j = int32.Parse(sBuf(1))
				j += 1
				sFileNew = sFileOrg & "_" & j.ToString("D3")
			Else
				sFileOrg = sFile
				sFileNew = sFileOrg & "_" & (1).ToString("D3")
			End If

		If iUpdate = 0
			p_dtFile.SetDataFileName = sFileNew
		Else
			p_dtFile.SetDataFileName = sFileOrg
		End if
		'データファイルヘッダ情報の変更
		With p_dtFile
		.p_head.sFileName	= p_dtFile.GetDataFileName							'ファイル名
		.p_head.sThreshold	= p_clSpec.spi.dThreshold							'応力判定値
		.p_head.sCalcMethod = lstCalcMethod(p_clSpec.spi.iGelTimeMethod)		'ゲルタイム計算方法
		FilterCondFormatMake()													'フィルタ条件
		'.p_head.sFullCalibValue = p_dFullStressADVal            'キャリブレーション値
		'.p_head.sDeviceSpec		= p_clSpec.spi.sDeviceSpecName  'デバイスSPEC名
		'.p_head.sSampleSpec		= p_clSpec.spi.sSampleSpecName  'サンプルSPEC名
		.p_head.sSampleName		= p_clSpec.spi.sSampleName						'サンプル名
		.p_head.sLot			= p_clSpec.spi.sLot								'ロット
		.p_head.sMeasurer		= p_clSpec.spi.sMeasurer						'測定者
		.p_head.dPBDiameter		= p_clSpec.spi.dProbDiameter					'プローブ直径
		.p_head.sJudgeRange		= p_clSpec.spi.dJudgeRangeMin & " - " & p_clSpec.spi.dJudgeRangeMax		'判定範囲
		.p_head.sMaskTime		= p_clSpec.spi.dEffectRangeMin & " - " & p_clSpec.spi.dEffectRangeMax	'マスク範囲

		.p_head.sShape = p_clSpec.spi.iShape		'形状
		Select Case p_clSpec.spi.iShape
		Case eShape.Sheet, eShape.Rcc
			.p_head.sthickness	= p_sThickness   '厚み(mm)
			.p_head.dStacks		= p_sStacks      '積層枚数
		Case eShape.Powder
			.p_head.sthickness	= p_sSpecGravity '密度
			.p_head.dStacks		= p_sWeight      '重量
		Case eShape.Liquid
			.p_head.sthickness	= p_sSpecGravity '密度
			.p_head.dStacks		= p_sVolume      '体積
		End Select
		End With
		p_dtFile.RealTimeMeasDataFileSaveText(eMeasMode.Normal, False, m_cts)
	End If

End sub
Public Function GelTimeSeach(ByVal iSGItem As int16) As Int32
	Dim iRet As Int32
	'== THR ゲルタイム計算処理 ==
	p_iGelTimeJudge = 2		'NA
	Select Case If(p_sCalcMethod="",p_dtFile.p_head.sCalcMethod,p_sCalcMethod)
    Case lstCalcMethod(eCalcMethod.THR)
		iRet = p_dtFile.CalcGelTime(eCalcMethod.THR,	iSGItem, p_dGelTime, p_dToqueThreshold, p_dFullStressADVal, p_dAprxRangeMin, p_dAprxRangeMax,,,,p_cts)

    Case lstCalcMethod(eCalcMethod.PHASE)
		iRet = p_dtFile.CalcGelTime(eCalcMethod.PHASE,	iSGItem, p_dGelTime, p_dToqueThreshold, 90.0 * Math.PI / 180, p_dAprxRangeMin, p_dAprxRangeMax,,,,p_cts)

    Case lstCalcMethod(eCalcMethod.SLOPE)
		iRet = p_dtFile.CalcGelTime(eCalcMethod.SLOPE,	iSGItem, p_dGelTime, p_dToqueThreshold, p_dFullStressADVal, p_dAprxRangeMin, p_dAprxRangeMax,,,,p_cts)

    Case lstCalcMethod(eCalcMethod.MELT)
		If p_dtFile.p_head.sMelWaitTime <> ""
			iRet = p_dtFile.CalcGelTime(eCalcMethod.MELT,	iSGItem, p_dGelTime, p_dToqueThreshold, p_dFullStressADVal, p_dAprxRangeMin, p_dAprxRangeMax, _
			p_dtFile.p_head.sMelWaitTime+3*p_dtFile.p_head.dXPrd, p_dMeltVisco, p_dMeltTemp, p_cts)
		Else
			iRet = p_dtFile.CalcGelTime(eCalcMethod.MELT,	iSGItem, p_dGelTime, p_dToqueThreshold, p_dFullStressADVal, p_dAprxRangeMin, p_dAprxRangeMax, _
			0, p_dMeltVisco, p_dMeltTemp, p_cts)
		End if

    Case lstCalcMethod(eCalcMethod.EXTRA)
	End Select
	Select Case iRet
	Case 0		'ゲルタイム計算OK
		p_dtfile.p_head.dGelTime = p_dGelTime					'データファイルヘッダ用ゲルタイム
		Select Case p_dGelTime
		Case p_dJudgeRangeMin To p_dJudgeRangeMax
			p_iGelTimeJudge = 1			'OK
			p_dtFile.p_head.sGelTimeJudge = "OK"
		Case Else
			If (p_dJudgeRangeMax - p_dJudgeRangeMin) <> 0
				p_iGelTimeJudge = 0			'NG
				p_dtFile.p_head.sGelTimeJudge = "NG"
			Else
				p_iGelTimeJudge = 2		'NA
				p_dtFile.p_head.sGelTimeJudge = "NA"
			End If
		End Select
	Case -2		'ゲルタイム計算不可
		p_dtfile.p_head.dGelTime		= -1.0					'ゲルタイム
	case else	'例外エラー発生
		p_dtfile.p_head.dGelTime		= -1.0					'ゲルタイム
	End Select
	Return iRet
End Function
'''<summary> リアルタイム測定時の条件変更設定を読込 </summary>
'''<param name="strName">設定名称。</param>
Public Function sbLoadRTGSetting(ByVal strName As String, Optional ByRef bZPosCalbEnable As Boolean = False) As Boolean
	Dim bRet As Boolean = True
	If strName <> "" Then
        '読込して構造体に値を代入
		If p_sEquipName=""
			'一度も接続されていない時、接続設定されている装置名をセットする
			p_sEquipName = p_cts.strEquipName(p_cts.iSelMDK)
		End If
        bRet = frmOptions.LoadRTGSET(strName, p_sEquipName, p_rtgset, bZPosCalbEnable)

        '処理時最初の参照構造体
        m_mdbrRTGSetNext.dHPTemp	= p_rtgset.mdbr(0).dHPTemp
    Else
        'SPEC途中変更をしない場合の設定
        p_rtgset.dTiming = New Double(1) {}
        p_rtgset.dTiming(0) = -1.0R
    End If
	Return bRet
End Function




''' <summary>
''' 測定開始時のファイルヘッダ情報のセット
''' </summary>
Private Sub MeasureHeaderInfoSet(ByVal iMode As int16)
Try
	With p_dtFile
	.p_head = Nothing
	.DataCount = 0															'蓄積データカウンタクリア
	.ReadBufferClear()														'蓄積エリアのクリア
	.MesFileNumber = 0														'ファイル連番クリア

	If iMode = eMeasMode.Normal
		.SetDataFileName = Format(Now, "yyyyMMddHHmmss")				'データファイル名
	End If
	.SectionCount = 100														'区間変化量を算出するときの区間サンプル数
	.MesBlockCnt = 0														'受信ブロックカウンタクリア

	'データファイルヘッダ部セット ------
	.p_head.sToolVersion			= Application.ProductVersion						'本アプリバージョン
																						'本体バージョン
	.p_head.sEquipVersion			= p_RTM(0).p_mltcp.DevSoftVer.ToString("F2") & "/" & p_RTM(0).p_mltcp.ComSoftVer.ToString("F2") 
	.p_head.sFileName				= p_dtFile.GetDataFileName()						'自ファイル名
	.p_head.sDateTime				= Format(Now, "yyyy/MM/dd HH:mm:ss")				'測定開始日時
	.p_head.sMeasureType			= p_dtFile.sMeasindCode(iMode)						'測定種類
	.p_head.sEquipName				= p_sEquipName										'本体識別
	.p_head.sXYSensorID				= p_sXYSensorID										'XYセンサID
	.p_head.sZSensorID				= p_sZSensorID										'ZセンサID
	.p_head.iIntegTimePowOn			= p_mltcp.IntegTimePowOn							'本体電源ON積算時間[秒]
	.p_head.iIntegTimeHeatOn		= p_mltcp.IntegTimeHeatOn							'熱板ヒータON積算時間[秒]
	.p_head.iIntegTimePBHeat		= p_mltcp.IntegTimePBHeat							'プローブヒータON積算時間[秒]
	.p_head.iIntegTimeSGHeat		= p_mltcp.IntegTimeSGHeat							'起歪体ヒータON積算時間[秒]
	.p_head.iIntegTimeXAct			= p_mltcp.IntegTimeXAct								'X軸動作積算時間[秒]
	.p_head.iIntegTimeYAct			= p_mltcp.IntegTimeYAct								'Y軸動作積算時間[秒]
	.p_head.iIntegTimeZAct			= p_mltcp.IntegTimeZAct								'Z軸動作積算時間[秒]

	.p_head.sDeviceSpec				= p_clSpec.spi.sDeviceSpecName						'デバイスSPEC名
'	.p_head.sHPPosControl			= p_clSpec.sHCont(p_clSpec.spi.iHPPosCont)			'熱板位置制御
	.p_head.sHPPosControl			= p_clSpec.spi.iHPPosCont							'熱板位置制御

	.p_head.dHPTemp					= p_clSpec.spi.dHPSetTemp							'熱板温度設定(℃)
	.p_head.dPBTemp					= p_clSpec.spi.dPBSetTemp							'プローブ温度設定(℃)
	.p_head.dInputGap				= p_clSpec.spi.dSampleGap							'投入GAP
	.p_head.dGapValue				= p_clSpec.spi.dMeasureGap							'測定GAP
	.p_head.dMeasurePress			= p_clSpec.spi.dMeasureStress						'測定圧力設定
	.p_head.iPressContStep			= p_clSpec.spi.iPressContStep						'圧力制御STEP
	.p_head.iPressContDelay			= p_cts.PressContDelayTime							'圧力制御Delay time
	.p_head.dPBDiameter				= p_clSpec.spi.dProbDiameter						'プローブ直径
'	.p_head.sStrsDetectMode			= p_clSpec.sStrsSens(p_clSpec.spi.iStressSensMode)	'応力検知モード
	.p_head.sStrsDetectMode			= p_clSpec.spi.iStressSensMode						'応力検知モード
	.p_head.sAmpMode				= p_clSpec.spi.iAmpMode								'振幅測定モード
	.p_head.dLimitStress			= p_clSpec.spi.dAmpModeStress						'上限応力(%)
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
	.p_head.dHPLiftWait				= p_clSpec.spi.dInputToHPUpWait						'投入から熱板上昇までの待ち時間
	.p_head.dInputWait				= p_clSpec.spi.dSampleInputWait						'投入待ち時間
	.p_head.dXYActWait				= p_clSpec.spi.dHPUpToXYActWait						'XY動作待ち時間
	.p_head.dMeasTimeLimit			= p_clSpec.spi.dMeasureTime							'測定時間リミット
	.p_head.dMeasEndStress			= p_clSpec.spi.dMeasureEndStress					'測定終了応力
	.p_head.sTempProfile			= p_clSpec.spi.sTempProfileName						'温度プロファイル名
	.p_head.dDiffInterval			= p_clSpec.spi.dDiffInterval						'SLOPE微分区間

	

	.p_head.sSampleSpec				= p_clSpec.spi.sSampleSpecName						'サンプルSPEC名
	.p_head.sSampleName				= p_clSpec.spi.sSampleName							'サンプル名
	.p_head.sLot					= p_clSpec.spi.sLot									'ロット
	.p_head.sMeasurer				= p_clSpec.spi.sMeasurer							'測定者
	.p_head.sShape					= p_clSpec.spi.iShape								'形状
	.p_head.sThickness				= p_clSpec.spi.sThickness							'厚み/密度
	.p_head.dStacks					= p_clSpec.spi.dStack								'積層数/重量/体積
	.p_head.sMaskTime				= p_clSpec.spi.dEffectRangeMin.ToString("F1") & " - " & p_clSpec.spi.dEffectRangeMax.ToString("F1")	'マスク範囲
	.p_head.sJudgeRange				= p_clSpec.spi.dJudgeRangeMin.ToString("F1") & " - " & p_clSpec.spi.dJudgeRangeMax.ToString("F1")	'判定範囲


	.p_head.dXYWeightCoef			= p_cts.WeightCoef									'dig->N変換係数 XY
	.p_head.dZWeightCoef			= P_cts.WeightCoefZ									'dig->g変換係数 Z
	.p_head.sHPTempFlucRange		= p_dTempStubRange.ToString("F1")					'HP温度変動許容範囲
	.p_head.sPBTempFlucRange		= p_dTempStubRange2.ToString("F1")					'PB温度変動許容範囲
	.p_head.sFullCalibValue			= p_dFullStressADVal.ToString("F0")					'フルキャリブレーション値
	.p_head.sZeroCalibValue			= p_dFullStressZero.ToString("F0")					'ゼロキャリブレーション値



	.p_head.sFullStressAttenuation	= p_dFullStressAttenuation.ToString("F6")			'フルストレス減衰比
	.p_head.sFullStressPhaseX		= p_dFullStressPxPhase								'フルストレス位置 位相X
	.p_head.sFullStressPhaseY		= p_dFullStressPyPhase								'フルストレス位置 位相Y
	.p_head.sFullStressPhaseZ		= p_dFullStressPzPhase								'フルストレス位置 位相Z
	'.p_head.sZeroAdjStartPos		= p_cts.dZeroStaPosHP.ToString("F3")				'ゼロ調 HP開始位置
	.p_head.sZeroAdjStabMonTime		= p_cts.iStaMonTime									'ゼロ調 安定監視時間
	.p_head.sZeroAdjFlucRange		= p_cts.iStaFlucRange								'ゼロ調 変動許容範囲
	.p_head.sHPCenterX				= p_mltcp.CenterPosX()								'HP 中心位置X
	.p_head.sHPCenterY				= p_mltcp.CenterPosY()								'HP 中心位置Y
	FilterCondFormatMake()																'フィルタ条件
	.p_head.dSGTemp					= p_mltcp.Temp3Preset(1)							'起歪体設定温度
	.p_head.dGapCorrect				= p_cts.GapCorr										'GAP補正値
	.p_head.dZStab					= p_dZstressCR										'Z荷重安定度(ゼロ調開始時のラッチ値)

	'温度パラメータ
	.p_head.dTempCnt_P				= p_cts.TempCnt_P
	.p_head.dTempCnt_I				= p_cts.TempCnt_I
	.p_head.dTempCnt_D				= p_cts.TempCnt_D
	.p_head.dTempCnt_DGain			= p_cts.TempCnt_Dgain
	.p_head.dTempCnt_PVBias			= p_cts.TempCnt_PVBias
	.p_head.dTempCnt_PVGain			= p_cts.TempCnt_PVGain
	.p_head.dTempCnt_P2				= p_cts.TempCnt_P2
	.p_head.dTempCnt_I2				= p_cts.TempCnt_I2
	.p_head.dTempCnt_D2				= p_cts.TempCnt_D2
	.p_head.dTempCnt_DGain2			= p_cts.TempCnt_Dgain2
	.p_head.dTempCnt_PVBias2		= p_cts.TempCnt_PVBias2
	.p_head.dTempCnt_PVGain2		= p_cts.TempCnt_PVGain2
	.p_head.dTempCnt_P3				= p_cts.TempCnt_P3
	.p_head.dTempCnt_I3				= p_cts.TempCnt_I3
	.p_head.dTempCnt_D3				= p_cts.TempCnt_D3
	.p_head.dTempCnt_DGain3			= p_cts.TempCnt_Dgain3
	.p_head.dTempCnt_PVBias3		= p_cts.TempCnt_PVBias3
	.p_head.dTempCnt_PVGain3		= p_cts.TempCnt_PVGain3


	Select case iMode
	Case eMeasMode.Normal,eMeasMode.NormalLT,eMeasMode.NormalNoAcum
		.p_head.sThreshold		= p_dToqueThreshold.ToString("F1")					'応力判定値
		.p_head.dMeasEndStress	= p_dStressLimit									'測定終了応力
		.p_head.dMeasTimeLimit	= p_dMeasTimeLimit									'測定時間リミット
		.p_head.sCalcMethod		= p_sCalcMethod										'ゲルタイム計算方法
		.p_head.dGelTime		= -9.0												'ゲルタイム(-9:測定中)
		.p_head.sGelTimeJudge	= "NA"												'ゲルタイム判定
		.p_head.sMaskTime		= p_clSpec.spi.dEffectRangeMin.ToString("F1") & " - " & p_clSpec.spi.dEffectRangeMax.ToString("F1")	'マスク範囲
		.p_head.sJudgeRange		= p_clSpec.spi.dJudgeRangeMin.ToString("F1") & " - " & p_clSpec.spi.dJudgeRangeMax.ToString("F1")	'判定範囲
		.p_head.sTempProfile	= p_clSpec.spi.sTempProfileName						'温度プロファイル名

	Case eMeasMode.Calib  'キャリブレーション

	Case eMeasMode.FlowCurve_Speed  'フローカーブ 速度
		.p_head.sFLCTemp			= p_FlcvPreTemp.ToString("F1")				'FLC 設定温度(℃)
		.p_head.sProbeDiameter		= p_FlcvPbDiameter.ToString("F3")			'FLC プローブ直径(mm)
		.p_head.sFLCSampleTime		= p_FlcvSample.ToString("F1")				'FLC サンプル取得時間(Sec)
		.p_head.sFLCSampleAccRate	= p_FlcvAveSizeRate.Tostring("F1")			'FLC サンプル取得率(%)
		.p_head.sFLCAmplitude		= p_FlcvAmplitude.ToString("F3")	 		'FLC 振幅(mm)
		.p_head.sFLCPeriod			= p_FlcvPeriod.ToString("F3")		 		'FLC 周期(Sec)
						
		.p_head.sFLC1AmplitudeStart	= p_FlcvStart.ToString("F3")		 		'FLC1 開始振幅(mm)
		.p_head.sFLC1AmplitudeEnd	= p_FlcvEnd.ToString("F3")	 				'FLC1 終了振幅(mm)
		.p_head.sFLC1AmplitudeStep	= p_FlcvStep.ToString("F3")					'FLC1 Step振幅(mm)
		.p_head.sFLC1MeasRev		= p_FlcvReverse.ToString()		 			'FLC1 逆順測定有効/無効

	Case eMeasMode.FlowCurve_Time  'フローカーブ 時間
		.p_head.sFLCTemp			= p_FlcvPreTemp.ToString("F1")				'FLC 設定温度(℃)
		.p_head.sProbeDiameter		= p_FlcvPbDiameter.ToString("F3")			'FLC プローブ直径(mm)
		.p_head.sFLCSampleTime		= p_FlcvSample.ToString("F1")				'FLC サンプル取得時間(Sec)
		.p_head.sFLCSampleAccRate	= p_FlcvAveSizeRate.Tostring("F1")			'FLC サンプル取得率(%)
		.p_head.sFLCAmplitude		= p_FlcvAmplitude.ToString("F3")	 		'FLC 振幅(mm)
		.p_head.sFLCPeriod			= p_FlcvPeriod.ToString("F3")		 		'FLC 周期(Sec)

		.p_head.sFLC2Repeat			= p_FlcvCycle.ToString						'FLC2 繰返し回数(回)
		.p_head.sFLC2Ope			= p_FlcvOpeTime.ToString("F1")				'FLC2 動作時間(Sec)
		.p_head.sFLC2Rest			= p_FlcvRstTime.ToString("F1")				'FLC2 停止時間(Sec)
						
	Case eMeasMode.FlowCurve_Temp  'フローカーブ 温度
		.p_head.sProbeDiameter		= p_FlcvPbDiameter.ToString("F3")			'FLC プローブ直径(mm)
		.p_head.sFLCSampleTime		= p_FlcvSample.ToString("F1")				'FLC サンプル取得時間(Sec)
		.p_head.sFLCSampleAccRate	= p_FlcvAveSizeRate.Tostring("F1")			'FLC サンプル取得率(%)
		.p_head.sFLCAmplitude		= p_FlcvAmplitude.ToString("F3")	 		'FLC 振幅(mm)
		.p_head.sFLCPeriod			= p_FlcvPeriod.ToString("F3")		 		'FLC 周期(Sec)
						
		.p_head.sFLC3TempStart		= p_FlcvStart.ToString("F1")		 		'FLC3 開始温度(℃)
		.p_head.sFLC3TempEnd		= p_FlcvEnd.ToString("F1")	 				'FLC3 終了温度(℃)
		.p_head.sFLC3TempStep		= p_FlcvStep.ToString("F1")					'FLC3 Step温度(℃)

	End Select

	End With

Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub
'フィルタ条件フォーマット作成
Public Sub FilterCondFormatMake()
	With p_dtFile
	If p_FltSetting.iEnable <> 0
		.p_head.sFilter		= p_FltSetting.sKind & "-"
		Select case p_FltSetting.sKind
		Case "FIR"
			.p_head.sFilter	= .p_head.sFilter & p_FltSetting.dFIRCutOffFreq.ToString("F2") & "-" & _
								p_FltSetting.iFIRFilLen & "-" & _
								p_FltSetting.sFIRWinKind
		Case "IIR"
			.p_head.sFilter	= .p_head.sFilter & p_FltSetting.dIIRCutOffFreq.ToString("F2") & "-" & _
								p_FltSetting.iIIRDeg
		End Select
	Else
		.p_head.sFilter		= "NONE"
	End If
	End With
End Sub
''' <summary>
''' キャリブレーション開始時の最初の測定選択インデックスを探す
''' </summary>
''' <param name="iMode">0：X 1：C 2:P</param>
''' <param name="iFirstIdx">インデックス</param>
''' <returns>0：インデックス検索OK -1：選択されていないまたは異常終了</returns>
Public Function FirstIndexSerchInCalibArray(ByRef iMode As Int32, ByRef iFirstIdx As int32) As Int32
	Dim j As int32
	Dim clb() As stPoint = Nothing
Try
	Select Case iMode
	Case 0
		clb = p_ClbX
	Case 1
		clb = p_ClbC
	Case 2
		clb = p_ClbZ
	End Select
	For j = 0 To clb.GetUpperBound(0)
		If clb(j).bSelected								'測定選択されている
			iFirstIdx = j
			Exit for
		End If
	Next
	If j > clb.GetUpperBound(0)
	'測定選択が無い
		Return -1
	Else
		Return 0
	End If

Catch ex As Exception
	Return -1
End Try
	Return 0
End Function

#If 0
'温度PIDモニタ用変数
Public	p_TempMonEn		As Boolean = False		'ロギング有効
Public	p_MonReset		As int16 = 0			'リセットタイミング
Dim		iJob			As int32 = -1			'シーケンス変数
Public	dsTemp			As New Dataset
Public	dtHP			As New DataTable		'HP PID データテーブル
Public	dtHPT			As New DataTable		'HP 温度 データテーブル

Public	dtPB			As New DataTable		'PB PID データテーブル
Public	dtPBT			As New DataTable		'PB 温度 データテーブル
Dim		GetCount		As UInt32 = 0			'データ行カウント

'温度PID制御量モニタ
Public Function	TempAdjMonProc(ByRef sw8 As Stopwatch) As int32
Dim dtRowHp		As DataRow
Dim dtRowHpT	As DataRow
Dim dtRowPb		As DataRow
Dim dtRowPbT	As DataRow

Try
	'ゼロ調中のＺ位置モニタ
	Select Case iJob
	Case -1
	'データテーブル初期化
		If p_TempMonEn 'And (p_mltcp.isStatHeaterOn2 Or p_mltcp.isStatHeaterOn)
			'列の作成
			dtHP.Columns.Add("200mS Count", Type.GetType("System.UInt32"))
			dtHP.Columns.Add("dmv", Type.GetType("System.Int32"))
			dtHP.Columns.Add("pn", Type.GetType("System.Int32"))
			dtHP.Columns.Add("in", Type.GetType("System.Int32"))
			dtHP.Columns.Add("dn", Type.GetType("System.Int32"))
			dtHP.TableName = "hpPIDTbl"
			dsTemp.Tables.Add(dtHP)

			dtHPT.Columns.Add("200mS Count", Type.GetType("System.UInt32"))
			dtHPT.Columns.Add("Temp", Type.GetType("System.Double"))
			dtHPT.Columns.Add("Mv", Type.GetType("System.Double"))
			dtHPT.TableName = "hpTempTbl"
			dsTemp.Tables.Add(dtHPT)

			dtPB.Columns.Add("200mS Count", Type.GetType("System.UInt32"))
			dtPB.Columns.Add("dmv", Type.GetType("System.Int32"))
			dtPB.Columns.Add("pn", Type.GetType("System.Int32"))
			dtPB.Columns.Add("in", Type.GetType("System.Int32"))
			dtPB.Columns.Add("dn", Type.GetType("System.Int32"))
			dtPB.TableName = "pbPIDTbl"
			dsTemp.Tables.Add(dtPB)

			dtPBT.Columns.Add("200mS Count", Type.GetType("System.UInt32"))
			dtPBT.Columns.Add("Temp", Type.GetType("System.Double"))
			dtPBT.Columns.Add("Mv", Type.GetType("System.Double"))
			dtPBT.TableName = "pbTempTbl"
			dsTemp.Tables.Add(dtPBT)

			iJob = 0
		End if

	Case 0
		If p_TempMonEn 'And (p_mltcp.isStatHeaterOn2 Or p_mltcp.isStatHeaterOn)
			GetCount = 0
			iJob = 1
			sw8.reStart()
		End if

	Case 1
	'モニタ中の処理
		If p_TempMonEn 'And (p_mltcp.isStatHeaterOn2 Or p_mltcp.isStatHeaterOn)
			'データの追加
			If sw8.Elapsed.TotalMilliseconds >= 200
				GetCount += 1
				dtRowHp = dtHP.NewRow
				dtRowHp(0) = GetCount
				dtRowHp(1) = p_mltcp.Hp_dmv
				dtRowHp(2) = p_mltcp.Hp_pn
				dtRowHp(3) = p_mltcp.Hp_in
				dtRowHp(4) = p_mltcp.Hp_dn
				dtHP.Rows.Add(dtRowHp)

				dtRowHPT = dtHPT.NewRow
				dtRowHPT(0) = GetCount
				dtRowHPT(1) = p_mltcp.HeatTempAct()
				dtRowHPT(2) = p_mltcp.HeatTempOpe()
				dtHPT.Rows.Add(dtRowHpT)

				dtRowPb = dtPB.NewRow
				dtRowPb(0) = GetCount
				dtRowPb(1) = p_mltcp.Pb_dmv
				dtRowPb(2) = p_mltcp.Pb_pn
				dtRowPb(3) = p_mltcp.Pb_in
				dtRowPb(4) = p_mltcp.Pb_dn
				dtPB.Rows.Add(dtRowPb)

				dtRowPbT = dtPBT.NewRow
				dtRowPbT(0) = GetCount
				dtRowPbT(1) = p_mltcp.HeatTemp2Act()
				dtRowPbT(2) = p_mltcp.HeatTemp2Ope()
				dtPBT.Rows.Add(dtRowPbT)

				sw8.Restart()
			End if

			if p_MonReset <> 0
				iJob = 8
			End if
		Else
			iJob = 9
		End if
	Case 8
	'グラフリセット処理
		dtHP.Clear
		dtPB.Clear
		dtPBT.Clear
		dtHPT.Clear
		dsTemp.Clear
		iJob = 0
		p_MonReset = 0

	Case 9
	'終了処理
		iJob = 0
		p_MonReset = 0
		p_TempMonEn = False
		dtHP.Clear
		dtPB.Clear
		dtPBT.Clear
		dtHPT.Clear
		dsTemp.Clear
	End Select
Catch ex As Exception
	Return -1
End Try
	Return 0
End Function
#end if


End Class
