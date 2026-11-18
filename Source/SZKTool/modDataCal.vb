Imports System.Diagnostics.Eventing.Reader
Imports System.IO
Imports System.Numerics
Imports System.Runtime.InteropServices
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.Security.Cryptography
Imports System.Text
Imports System.Threading
Imports System.Windows
Imports MathNet.Numerics.Statistics
Imports Microsoft.Office.Core
Imports Microsoft.Office.Interop
Imports SZKTool.frmOptions
''' <summary>
''' 測定データ計算モジュール
''' </summary>
Public Class DataCal
#If 1
'計算条件パラメータ
Public Structure stCalcPara
	Dim iMeas			As Int16					'測定種別 0:ゲルタイム　1:フルキャリブレーション　2:ゼロキャリブレーション　3:フローカーブ
	Dim iMode			As Int16					'0:最終データ 1:測定中 2:キャリブレーション
	Dim iCalcType		As int16					'計算方法 0:THR 1:SLOPE 2:EXTRAPOLATION 3:PHASE 4:MELT
	Dim	iHPPosControl	As Int16					'測定時熱板制御 0:GAP 1:着圧 2:常時
	Dim cts				As CToolSettings			'詳細設定クラス
	Dim clDt			As DataFile					'データファイルクラス
	Dim dPeriod			As Double					'周期(秒)
	Dim dAmplitude		As Double					'振幅(mm)
	Dim iSampleStart	As Int64					'測定データ開始インデックス
	Dim iSampleNum		As Int64					'サンプル数
	Dim fltSet			As Filter.stFilterInfo		'フィルタクラス
	Dim dXYWait			As Double					'XY動作待ち時間(秒)<-測定開始からXYZ動作するまでの時間
	Dim dZeroStress		As Double					'ゼロストレス値
	Dim dZeroPxPhase	As Double					'ゼロストレスx位置位相差
	Dim dZeroPyPhase	As Double					'ゼロストレスy位置位相差
	Dim dZeroPzPhase	As Double					'ゼロストレスz位置位相差
	Dim dAttenuation	As Double					'2秒周期基準のフィルタ減衰率
	Dim dDiameter		As Double					'プローブ直径(mm)
	Dim dMeasGap		As Double					'測定GAP(mm)
	Dim dFullStress		As Double					'フルストレス値
	Dim dPxPhase		As Double					'x位置位相差
	Dim dPyPhase		As Double					'y位置位相差
	Dim dPzPhase		As Double					'z位置位相差
	Dim iZeroPos		As Int32					'zero調確定位置
	Dim dMeasureStress	As Double					'測定時設定圧力(kPa)
	Dim dDiffInterval	As Double					'SLOPE計算時の微分区間(秒)
	Dim iStrsSensMode	As Int16					'応力検知モード 0:線 1:円 2:点
	Dim iAmpMode		As Int16					'Z振動モード 0:振幅一定 1:応力一定
	Dim dCalibAmp()			As Double				'キャリブレーション 振幅テーブル(点モード時のみ)
	Dim dCalibFullStress()	As Double				'キャリブレーション フル応力値(点モードバッチ時のみ)
	Dim dCalibFullPhase()	As Double				'キャリブレーション フル位相値(点モードバッチ時のみ)
	Dim dRCCBaseThickness	As Double				'サンプル：RCCの時の基材厚み 積層数で計算済
	Dim dMesGap				As Double				'測定GAP計算値(mm) リアルタイム モニタ使用
	Dim dXYWeightCoef		As Double				'XY重量N換算係数 (Dig->N)
	Dim dHPPosition			As Double				'熱板現在位置(um)
End Structure
'データ定義
Private m_iOXBuf_R_pt	As Int64 = 0								'リアルタイム補正のための入力バッファ読み取りポインタ
Private m_TempX_1_R_pt	As Int64 = 0								'リアルタイム補正のための微分列バッファ読み取りポインタ
Private m_TempX_A_R_pt	As Int64 = 0								'リアルタイム補正のための1次積分列バッファ読み取りポインタ
Private m_iSumAry1_W_pt	As Int64 = 0								'1回目積分配列書込みポインタ記憶
Private m_iSumAryA_W_pt	As Int64 = 0								'2回目積分配列書込みポインタ記憶
Private m_iMoveAve_W_pt	As Int64 = 0								'移動平均配列書込みポインタ記憶
Private m_iWaveBuf_W_pt As Int64 = 0								'波形モニタ用配列書込みポインタ記憶

Dim		TempX_1()		As Double = Nothing							'微分列
Dim		TempY_1()		As Double = Nothing							'微分列
Dim		TempXp_1()		As Double = Nothing							'位置微分列
Dim		TempYp_1()		As Double = Nothing							'位置微分列

Dim		TempX_A()		As Double = Nothing							'1次積分列
Dim		TempY_A()		As Double = Nothing							'1次積分列

Public	TempXp_A()		As Double = Nothing							'位置 2次積分列
Public	TempYp_A()		As Double = Nothing							'位置 2次積分列
Public	TempXp_QM()		As Double = Nothing							'X位置 2次積分列(FC波形モニタ使用)
Public	TempYp_QM()		As Double = Nothing							'Y位置 2次積分列

Public	TempX_QM()		As Double = Nothing							'波形モニタ用 (2次積分列)
Public	TempY_QM()		As Double = Nothing							'波形モニタ用 (2次積分列)

Dim	Temp_J100()			As Double = Nothing							'XY合成配列
Dim	Temp_JAve()			As Double = Nothing							'XY合成配列

Dim	Temp_SL()			As Double = Nothing							'Slope演算
								
Dim	TempXp_Rad()		As Double = Nothing							'X動的粘弾性(Rad)
Dim	TempYp_Rad()		As Double = Nothing							'Y動的粘弾性(Rad)
Dim	TempXY_Rad()		As Double = Nothing							'XY動的粘弾性(Rad)

Dim	Temp_V()			As Double = Nothing										'粘度(PaS)
Dim	Temp_SS()			As Double = Nothing										'せん断応力
Dim	Temp_SV()			As Double = Nothing										'せん断速度

Dim	TempXp_G1()			As Double = Nothing										'X貯蔵弾性率(Pa)
Dim	TempXp_G2()			As Double = Nothing										'X損失弾性率(Pa)
Dim	TempXp_Tn()			As Double = Nothing										'X損失正接(deg)

Dim	TempYp_G1()			As Double = Nothing										'Y貯蔵弾性率(Pa)
Dim	TempYp_G2()			As Double = Nothing										'Y損失弾性率(Pa)
Dim	TempYp_Tn()			As Double = Nothing										'Y損失正接(deg)

Dim Temp_Stn()			As Double = Nothing										'サンプルに与えたひずみ量
Dim Temp_SM()			As Double = Nothing										'ずり弾性率

Dim	Temp_J()			As Double
Dim	Temp_T1Aly()		As Double
Dim	TempXp_T3Aly()		As Double
Dim	TempYp_T3Aly()		As Double
Dim	TempXY_T3Aly()		As Double
Dim	Temp_JAly()			As Double
Dim	TempXp_BAly()		As Double
Dim	Temp_GAly()			As Double

Dim TempX_QFB()			As Double	'FCモニタ用X応力コピーワーク
Dim TempY_QFB()			As Double	'FCモニタ用Y応力コピーワーク
Dim TempXp_QFB()		As Double	'FCモニタ用X位置コピーワーク
Dim TempYp_QFB()		As Double	'FCモニタ用Y位置コピーワーク

'今回値記録用2回目積分結果ワーク
Dim TempX_QF	As Double = 0
Dim TempY_QF	As Double = 0
Dim TempXp_QF	As Double = 0
Dim TempYp_QF	As Double = 0

'前回値記録用2回目積分結果ワーク
Dim TempX_QFP	As Double = 0
Dim TempY_QFP	As Double = 0
Dim TempXp_QFP	As Double = 0
Dim TempYp_QFP	As Double = 0

'Z位置移動平均用バッファ
Dim iZposAveBuf() As Int64 = Nothing

'温度プロファイル使用時の基準Z位置[um]記憶
Dim m_dLatBaseZPos		As Double = 0.0

'計算中？
Dim bBusy			As Boolean = False
Public ReadOnly Property Busy() As Boolean
	Get
		Busy = bBusy
	End Get
End Property
'計算ループカウント
Dim iCalCount		As Int64
Public ReadOnly Property CalCount() As Int64
	Get
		CalCount = iCalCount
	End Get
End Property
'計算ループ総合カウント
Dim iCalCountTotal		As Int64
Public ReadOnly Property CalCountTotal() As Int64
	Get
		CalCountTotal = iCalCountTotal
	End Get
End Property

'リアルタイム補正制御領域の初期化
Public sub AveTempClear(Optional ByVal dPeriod As Double = 1.0, 
	Optional ByVal iBatch	As Int16 = 0, 
	Optional ByVal iFidx	As Int16 = 0)
	iCalCount		= 0
	iCalCountTotal	= 0

	m_iOXBuf_R_pt	= 0
	m_TempX_1_R_pt	= 0
	m_TempX_A_R_pt	= 0
	m_iSumAry1_W_pt	= 0
	m_iSumAryA_W_pt	= 0
	m_iMoveAve_W_pt	= 0
	m_iWaveBuf_W_pt	= 0

	TempX_QF = 0
	TempY_QF = 0
	TempXp_QF = 0
	TempYp_QF = 0
	TempX_QFP = 0
	TempY_QFP = 0
	TempXp_QFP = 0
	TempYp_QFP = 0

	'微分格納領域確保
	TempX_1		= Nothing
	TempXp_1	= Nothing
	TempY_1		= Nothing
	TempYp_1	= Nothing
	Array.Resize(TempX_1,	MEAS_BLOCK_DATA_NUM * (dPeriod/2))
	Array.Resize(TempY_1,	MEAS_BLOCK_DATA_NUM * (dPeriod/2))
	Array.Resize(TempXp_1,	MEAS_BLOCK_DATA_NUM * (dPeriod/2))
	Array.Resize(TempYp_1,	MEAS_BLOCK_DATA_NUM * (dPeriod/2))

	'1回目積分格納領域確保
	TempX_A		= Nothing
	TempXp_A	= Nothing
	TempY_A		= Nothing
	TempYp_A	= Nothing
	Array.Resize(TempX_A,	MEAS_BLOCK_DATA_NUM * (dPeriod/2) - 1)
	Array.Resize(TempY_A,	MEAS_BLOCK_DATA_NUM * (dPeriod/2) - 1)
	Array.Resize(TempXp_A,	MEAS_BLOCK_DATA_NUM * (dPeriod/2) - 1)
	Array.Resize(TempYp_A,	MEAS_BLOCK_DATA_NUM * (dPeriod/2) - 1)

	If iBatch = 0
		Array.Resize(TempX_QM, 0)							'2次積分列
		Array.Resize(TempY_QM, 0)							'2次積分列
		Array.Resize(TempXp_QM, 0)							'位置 2次積分列
		Array.Resize(TempYp_QM, 0)							'位置 2次積分列
	end if
	TempX_QFB	= Nothing
	TempY_QFB	= Nothing
	TempXp_QFB	= Nothing
	TempYp_QFB	= Nothing
	Array.Resize(TempX_QFB, MEAS_BLOCK_DATA_NUM )
	Array.Resize(TempY_QFB, MEAS_BLOCK_DATA_NUM )
	Array.Resize(TempXp_QFB, MEAS_BLOCK_DATA_NUM )
	Array.Resize(TempYp_QFB, MEAS_BLOCK_DATA_NUM )

	Temp_J		= Nothing
	Temp_T1Aly	= Nothing
	TempXp_T3Aly= Nothing
	TempYp_T3Aly= Nothing
	TempXY_T3Aly= Nothing
	Temp_JAly	= Nothing
	TempXp_BAly	= Nothing
	Temp_GAly	= Nothing
	Array.Resize(Temp_J,MEAS_BLOCK_DATA_NUM * dPeriod )
	Array.Resize(Temp_T1Aly,MEAS_BLOCK_DATA_NUM * dPeriod )
	Array.Resize(Temp_JAly,MEAS_BLOCK_DATA_NUM * dPeriod )
	Array.Resize(Temp_GAly,MEAS_BLOCK_DATA_NUM * dPeriod )
	Array.Resize(TempXp_T3Aly,MEAS_BLOCK_DATA_NUM * dPeriod )
	Array.Resize(TempYp_T3Aly,MEAS_BLOCK_DATA_NUM * dPeriod )
	Array.Resize(TempXY_T3Aly,MEAS_BLOCK_DATA_NUM * dPeriod )
	Array.Resize(TempXp_BAly,MEAS_BLOCK_DATA_NUM * dPeriod )

	Temp_J100	= Nothing
	Temp_JAve	= Nothing
	Temp_SL		= Nothing

	TempXp_Rad	= Nothing
	TempYp_Rad	= Nothing
	TempXY_Rad	= Nothing

	Temp_V		= Nothing										'粘度(PaS)
	Temp_SS		= Nothing										'せん断応力
	Temp_SV		= Nothing										'せん断速度

	TempXp_G1	= Nothing										'X貯蔵弾性率(Pa)
	TempXp_G2	= Nothing										'X損失弾性率(Pa)
	TempXp_Tn	= Nothing										'X損失正接(deg)

	TempYp_G1	= Nothing										'Y貯蔵弾性率(Pa)
	TempYp_G2	= Nothing										'Y損失弾性率(Pa)
	TempYp_Tn	= Nothing										'Y損失正接(deg)

	Temp_Stn	= Nothing										'サンプルに与えたひずみ量(Pa)
	Temp_SM		= Nothing										'ずり弾性率(deg)

	iZposAveBuf = Nothing
	Array.Resize(iZposAveBuf,MEAS_BLOCK_DATA_NUM * dPeriod )
	If Not p_cts.LTModeEnable
		If iFidx = 0 And iBatch = 0
			iLatPrePzBuf	= 0
			iLatSign		= 0
			m_dLatBaseZPos	= 0.0
		End if
	End if
	iLatSampleNum	= 0
	iCalCount		= 0
	iLatShiftOneTime = 0


End sub
Public iLatPrePzBuf		As int64 = 0
Public iLatSign			As Int16 = 0
Dim iLatSampleNum		As Int64 = 0
Dim dInputPos			As Int64 
Public iLatTimeForZPos	As int64 = 0		'Z位置決定時の経過時間(mS)
Dim iLatShiftOneTime	As Int32 = 0		'1/4周期シフト 実行フラグ

Private iSwitchAmpXYPos	As Int32 = cXYAmpSwitch	'XY位置の振幅設定による解像度切替え閾値(um)

'現行補正計算
''' <summary>
''' 各種結果パラメータ計算処理（リアルタイム・バッチ）
''' </summary>
''' <returns>0:正常終了 -1:例外発生 1:サンプル数が満たない</returns>
Public Function CorrectRealProcVector( ByRef cp As stCalcPara ) As Int32
Try
	'周期0の時は合成できないので仮に周期1秒として計算する
	If cp.dPeriod = 0
		cp.dPeriod = 1.0
	End If
	Dim	i								As Int32
	Dim iAllPriod						As Int32	= (cp.dPeriod * 1000 / 2 ) - 1			'測定周期サンプル数
	Dim iHalfPriod						As Int32	= (cp.dPeriod * 1000 / 2 / 2) - 1		'測定半周期サンプル数
	Dim iQuotPriod						As Int32	= (cp.dPeriod * 1000 / 2 / 2 / 2) - 1	'測定1/4周期サンプル数
	Dim dFreq							As Double	= 1.0 / cp.dPeriod						'測定周波数
	Dim dSampleTime						As Double	= 1.0 / MEAS_SAMPLE_FREQ			'測定サンプリング時間(S)
	Dim iArraySize						As Int64	= cp.iSampleNum - 1					'配列サイズ = 処理サンプル数(引数)

	'比率・内積・外積・位相角を計算しないサンプル数
	Dim iIgnoreCount					As Int64	= (MEAS_BLOCK_DATA_NUM * cp.dXYWait) + (iHalfPriod * 3)

	'AD値->mN変換係数
	Dim dDigToNX						As Double	= 1.0
	Dim dDigToNY						As Double	= 1.0

	Dim SumAryX()						As Double = Nothing								'積分法 積分値マージ用
	Dim SumAryY()						As Double = Nothing								'積分法 積分値マージ用

	Dim SumAryPX()						As Double = Nothing								'積分法 積分値マージ用(位置)
	Dim SumAryPY()						As Double = Nothing								'積分法 積分値マージ用(位置)

	Dim XYvector(iArraySize)			As Vector

	Dim iSmpEnd							As int64
	Dim fil								As New Filter									'フィルタクラス生成
	Dim	Coef()							As Double = Nothing								'FIRフィルタパラメータ(窓関数から生成)
	Dim dK								As Double = Math.PI * dSampleTime / cp.dPeriod	'PI * サンプリング時間 / 周期
	Dim dSumX							As Double 										'和を計算
	Dim dSumY							As Double 										'和を計算
	Dim dSumPX							As Double 										'和を計算(位置)
	Dim dSumPY							As Double 										'和を計算(位置)
	'Dim dNConv							As Double = p_dNConv * 1E+6						'AD値->uN変換係数
	Dim dNConv							As Double = cp.dXYWeightCoef * 1E+6				'AD値->uN変換係数
	Dim iBranch							As int16
	Dim Sw								As New Stopwatch

	Dim Xvect		As Vector
	Dim Yvect		As Vector
	Dim Xpvect		As Vector
	Dim Ypvect		As Vector
	Dim XYvect		As Vector
	Dim XYpvect		As Vector
	Dim dXYCos		As Double
	Dim dXYSin		As Double

	Dim dUpperdeg	As Double = Math.PI / 2 + 0.16	'100deg
	Dim dLowerdeg	As Double = Math.PI / 2 - 0.32 	'70deg
	Dim dPPhase		As Double

	Dim iZPosAry()	As int64 = Nothing		'バッチ処理　点モード　Z位置移動平均バッファ


	'粘度関係計算 条件値
	Dim uVp As Datafile.ViscosityCalcParameter
	uVp.dPeriod			= cp.dPeriod										'設定周期(S)
	uVp.dAmplitude		= cp.dAmplitude / 10^3								'設定振幅(m)
	uVp.dMeasGap		= cp.dMeasGap / 10^3								'測定GAP(m)
	uVp.dProbeRadius	= (cp.dDiameter/10^3) / 2							'プローブ半径(m)
	uVp.dProbeSquare	= Math.PI * uVp.dProbeRadius^2						'プローブ面積(m2)



	iSmpEnd = cp.iSampleNum - 1						'iSampleNumはリアルタイムでは常に５００*周期　バッチ処理では最終インデックス

	Sw.ReStart()
	'測定種別による分岐処理
	'測定種別によるXYウェイトタイムの比率、位相角計算無効
	Select Case cp.iMeas
	Case eHoseiMeasMode.GelTime		'ゲルタイム
		Select Case cp.iMode
		Case ehoseiCalcMode.Batch
			iIgnoreCount = (MEAS_BLOCK_DATA_NUM * (cp.dXYWait))
			iLatShiftOneTime = 0
			iBranch = 0
		Case ehoseiCalcMode.RealTime
			iIgnoreCount = (MEAS_BLOCK_DATA_NUM * (cp.dXYWait))
			iBranch = 1
		Case eHoseiCalcMode.LTRealTime
			iIgnoreCount = 0
			iBranch = 1
		Case  eHoseiCalcMode.LTBatch
			iIgnoreCount = 0
			iBranch = 0
		End Select
	Case eHoseiMeasMode.FullCalib		'フルキャリブ
		Select Case cp.iMode
		Case ehoseiCalcMode.Batch
			iIgnoreCount = 0
			iBranch = 0
		Case ehoseiCalcMode.RealTime
			iIgnoreCount = 0
			iBranch = 1
		End Select
	Case eHoseiMeasMode.ZeroCalib		'ゼロキャリブ
		Select Case cp.iMode
		Case ehoseiCalcMode.Batch
			iIgnoreCount = 0
			iBranch = 0
		Case ehoseiCalcMode.RealTime
			iIgnoreCount = 0
			iBranch = 1
		End Select
	Case eHoseiMeasMode.FlowCurve		'フローカーブ
		Select Case cp.iMode
		Case ehoseiCalcMode.Batch
			iIgnoreCount = 0
			iBranch = 0
		Case ehoseiCalcMode.RealTime
			iIgnoreCount = 0
			iBranch = 1
		End Select
	Case Else
		iIgnoreCount = 0
		iBranch = 0
	End Select

	'一括処理の場合は読み取りインデックスをゼロクリア
	If iBranch = 0
		m_iOXBuf_R_pt	= 0
		m_TempX_1_R_pt	= 0
		m_TempX_A_R_pt	= 0
		m_iSumAry1_W_pt	= 0	'微分結果書込みポインタ準備
		m_iSumAryA_W_pt	= 0	'1回目積分書込みポインタ準備
		m_iMoveAve_W_pt	= 0	'移動平均書込みポインタ準備
		m_iWaveBuf_W_pt	= 0	'波形モニタ用書込みポインタ準備

		'点モード時のバッチ処理で使用するZ位置配列(Sin波消去)を生成する
		Select Case cp.iStrsSensMode
		Case clSpec.eStrsSens.Point	'点モード
			'周期区間で移動平均  （区間サンプル数は測定周期*500）　することによりSin波を消す
			Array.Resize(iZPosAry, cp.clDt.iPzBuf.Length)
			BatchMovingAverageInt(cp.clDt.iPzBuf, iZPosAry, cp.dPeriod*MEAS_BLOCK_DATA_NUM)
		End Select

	End If
	'リアルタイムの１回目か一括処理の場合
	'If IsNothing(TempX_1) Or iBranch = 0
	If m_iOXBuf_R_pt = 0 Or iBranch = 0
		Array.Resize(Temp_J100,cp.iSampleNum)
		Array.Resize(Temp_JAve,cp.iSampleNum)
		Array.Resize(Temp_SL,cp.iSampleNum)
		Array.Resize(TempXp_Rad,cp.iSampleNum)
		Array.Resize(TempYp_Rad,cp.iSampleNum)
		Array.Resize(TempXY_Rad,cp.iSampleNum)

		Array.Resize(Temp_V,cp.iSampleNum)
		Array.Resize(Temp_SS,cp.iSampleNum)
		Array.Resize(Temp_SV,cp.iSampleNum)

		Array.Resize(TempXp_G1,cp.iSampleNum)
		Array.Resize(TempXp_G2,cp.iSampleNum)
		Array.Resize(TempXp_Tn,cp.iSampleNum)

		Array.Resize(TempYp_G1,cp.iSampleNum)
		Array.Resize(TempYp_G2,cp.iSampleNum)
		Array.Resize(TempYp_Tn,cp.iSampleNum)

		Array.Resize(Temp_Stn,cp.iSampleNum)
		Array.Resize(Temp_SM,cp.iSampleNum)

	'リアルタイムの２回目以降
	Else
		Array.Resize(Temp_J100,Temp_J100.Length  + cp.iSampleNum)
		Array.Resize(Temp_JAve,Temp_JAve.Length  + cp.iSampleNum)
		Array.Resize(Temp_SL,Temp_SL.Length + cp.iSampleNum)
		Array.Resize(TempXp_Rad,TempXp_Rad.Length  + cp.iSampleNum)
		Array.Resize(TempYp_Rad,TempYp_Rad.Length  + cp.iSampleNum)
		Array.Resize(TempXY_Rad,TempXY_Rad.Length  + cp.iSampleNum)

		Array.Resize(Temp_V,Temp_V.Length  + cp.iSampleNum)
		Array.Resize(Temp_SS,Temp_SS.Length  + cp.iSampleNum)
		Array.Resize(Temp_SV,Temp_SV.Length  + cp.iSampleNum)

		Array.Resize(TempXp_G1,TempXp_G1.Length  + cp.iSampleNum)
		Array.Resize(TempXp_G2,TempXp_G2.Length  + cp.iSampleNum)
		Array.Resize(TempXp_Tn,TempXp_Tn.Length  + cp.iSampleNum)

		Array.Resize(TempYp_G1,TempYp_G1.Length  + cp.iSampleNum)
		Array.Resize(TempYp_G2,TempYp_G2.Length  + cp.iSampleNum)
		Array.Resize(TempYp_Tn,TempYp_Tn.Length  + cp.iSampleNum)

		Array.Resize(Temp_Stn,Temp_Stn.Length  + cp.iSampleNum)
		Array.Resize(Temp_SM,Temp_SM.Length  + cp.iSampleNum)
	End If

	Select Case cp.iStrsSensMode
	Case clSpec.eStrsSens.Point
	'X成分領域へZ成分に置き換えて計算
		dPPhase = cp.dPzPhase
		dNConv  = cp.cts.WeightCoefZ * p_cuNperGram								'uN/dig
	Case Else
		dPPhase = cp.dPxPhase
	End Select

	'Dim dZeroStress as Double
	'dZeroStress = 98 * ((2 * math.PI / cp.dPeriod)^2 ) * (cp.dAmplitude / 2) * cp.dAttenuation			'遠心力から求めた仮想のゼロストレスキャリブ値
	'dZeroStress = dZeroStress / Math.Sqrt(2)															'振幅の最大値


Dim preMesGap As double = 999
Dim preAmplitude As Double = 999
Dim iRangeNum As int64 = math.Ceiling(cp.dDiffInterval / (MEAS_TIME_RESOLUTION / 1000.0))
Dim iRoundNum As int64 = math.Ceiling(iRangeNum / 2)


Dim Temp_T1F		As Double = 0	
Dim TempXp_T2F		As Double = 0
Dim TempYp_T2F		As Double = 0
Dim TempXY_T2F		As Double = 0
Dim TempXp_BF		As Double = 0
Dim Temp_T3AveF		As Double = 0
Dim TempXp_T3AveF	As Double = 0
Dim TempYp_T3AveF	As Double = 0
Dim TempXY_T3AveF	As Double = 0
Dim TempXp_BAveF	As Double = 0
Dim Temp_NF			As Double = 0
Dim Temp_MF			As Double = 0
Dim Temp_LF			As Double = 0
Dim Temp_GF			As Double = 0
Dim dRTemp_J100F	As Double = 0

Dim k As Int32 = m_iSumAry1_W_pt		'微分結果書込みポインタ準備
Dim m As Int32 = m_iSumAryA_W_pt		'1回目積分書込みポインタ準備
Dim n As Int32 = m_iMoveAve_W_pt		'移動平均書込みポインタ準備
Dim p As Int32 = m_iWaveBuf_W_pt		'波形モニタ用書込みポインタ準備

#Const DEBUGFILEOUT = 1
#If DEBUGFILEOUT
'デバッグ用ファイル出力
	Dim fs			As FileStream	= Nothing
	Dim swr			As Textwriter	= Nothing
	Dim sLineBuf	As String		= Nothing
	If p_bCalcResultFOutEn		'隠しコマンドで有効か
		Dim sFilePath	As String = "C:\Test\debugCorrect_" & If(cp.iMode=ehoseiCalcMode.Batch,"B_", "R_") & cp.clDt.p_head.sFileName & ".csv"
		If m_iOXBuf_R_pt = 0
		'初回はファイル内空にする
			fs = New FileStream(sFilePath, FileMode.Create, FileAccess.Write, FileShare.Read)
			fs.Close()
		End if
		'追加書込みモードで開く
		fs	= New FileStream(sFilePath, FileMode.Append, FileAccess.Write, FileShare.Read)
		swr = New StreamWriter(fs, Encoding.UTF8)
		If m_iOXBuf_R_pt = 0
			'ヘッダ行
			sLineBuf = "i,iOxBuf,iOyBuf,iOzBuf,iPxBuf,iPyBuf,iPzBuf,,"
			sLineBuf &= "k,TempX_1,TempY_1,TempXp_1,TempYp_1,,"
			sLineBuf &= "m,TempX_A,TempY_A,TempXp_A,TempYp_A,,"
			sLineBuf &= "TempX_QF,TempY_QF,TempXp_QF,TempYp_QF,,"
			sLineBuf &= "TempX_QFP,TempY_QFP,TempXp_QFP,TempYp_QFP,,"
			sLineBuf &= "Temp_T1F,TempXp_T2F,TempYp_T2F,TempXY_T2F,TempXp_BF,,"
			sLineBuf &= "n,Temp_T1Aly,TempXp_T3Aly,TempYp_T3Aly,TempXY_T3Aly,TempXp_BAly,,"
			sLineBuf &= "Temp_T3AveF,TempXp_T3AveF,TempYp_T3AveF,TempXY_T3AveF,TempXp_BAveF,,"
			sLineBuf &= "Temp_LF,Temp_MF,Temp_NF,Temp_GF,,"
			sLineBuf &= "Temp_J,Temp_JAve,Temp_J100,,"
			sLineBuf &= "TempXp_Rad,TempYp_Rad,TempXY_Rad,"
			swr.WriteLine(sLineBuf)
		End if
	End if
#end if
dInputPos = cp.iZeroPos - (cp.cts.InputGap * 1000)		'サンプル投入位置を割り出す
Dim iLatPrePos		As Int64 = -1
Dim iLatPrePosIdx	As Int64 = -1
iLatSampleNum += cp.iSampleNum				'1周期分のデータが追加される
'==================================================================================
'==================================================================================
	bBusy = True
	iCalCountTotal = m_iOXBuf_R_pt + cp.iSampleNum - 1
	For i = m_iOXBuf_R_pt To iCalCountTotal
		iCalCount = i
		Select Case cp.iMeas
		Case eHoseiMeasMode.GelTime, eHoseiMeasMode.FlowCurve	'ゲルタイム,フローカーブ
			Select Case cp.iMode
			Case ehoseiCalcMode.RealTime
				PzBufUpdate(cp, i)	'Z位置測定データ更新

			Case ehoseiCalcMode.Batch
				If p_bMeasTimeEn
				'長時間連続モード
					PzBufUpdate(cp, i)			'Z位置測定データ更新
				End If
#If 1			
				If i > (cp.clDt.p_head.dHPLiftWait+1) * MEAS_BLOCK_DATA_NUM
				'熱板上昇待ち時間中は処理しない
					If m_dLatBaseZPos = 0
						If cp.clDt.p_head.dZAfterPos=0
						'着圧後Z位置が無い場合
							Dim iPosMax As Int64 = cp.clDt.iPzBuf.Max
							Select Case cp.iStrsSensMode
							Case clSpec.eStrsSens.Point
							'点モード
								'温度プロファイル使用無し
								'点モード時は位置配列の最大値-振幅/2で算出する
								m_dLatBaseZPos	=iPosMax - (cp.dAmplitude * 1E3 / 2)				'Z位置の記憶

							Case Else
							'円・直線モード
								If cp.clDt.p_head.sTempProfile=""
								'温度プロファイル使用無し
									'円、直線モードは位置配列の最大値	
									m_dLatBaseZPos	=iPosMax													'Z位置の記憶
								Else
								'温度プロファイル使用あり
									'500mS区間で位置変化なし検知
									If cp.clDt.iPzBuf(i) <> iLatPrePos
										Dim iSameCnt As Int64 = MEAS_BLOCK_DATA_NUM * 500 / 1000
										If i - iLatPrePosIdx > iSameCnt
										'500mS毎にチェック
											iLatPrePos		= cp.clDt.iPzBuf(i - iSameCnt)
											iLatPrePosIdx	= i
										Else
											iLatPrePos		= cp.clDt.iPzBuf(0)
											iLatPrePosIdx	= 0
										End If
									Else
										If m_dLatBaseZPos = 0
											m_dLatBaseZPos	  = cp.clDt.iPzBuf(i)						'Z位置の記憶
										End if
									End If

								End If
							End Select
						Else
						'着圧後Z位置がある場合
							m_dLatBaseZPos	=cp.clDt.p_head.dZAfterPos											'Z位置の記憶
						End If
					End If
				End If

#else
#If 1
					Select Case cp.iStrsSensMode
					Case clSpec.eStrsSens.Point
					'点モードの時は移動平均後配列(Sin波消去済)　iZPosAry　を使用する
						If i > (cp.dXYWait + cp.clDt.p_head.dHPLiftWait) * MEAS_BLOCK_DATA_NUM
						'XYZ動作待ち時間+熱板上昇待ち時間中は処理しない
							If iZPosAry(i) <> iLatPrePos
								If i > 5
								'10mS毎に比較
									iLatPrePos = iZPosAry(i - 5)		
								Else
									iLatPrePos = iZPosAry(0)
								End If
							Else
								If m_dLatBaseZPos = 0
								'最初の検知のみセット
									If p_iGuideXYbeforeGap = 1
									'警報発生時は
										m_dLatBaseZPos	  = m_dLatBaseZPosBak				'リアルタイム時のZ位置の記憶
									Else
										m_dLatBaseZPos	  = iZPosAry(i)						'Z位置の記憶
									End If
								End if
							End If
						End If
					Case Else
					'円、直線モード時
						If cp.clDt.iPzBuf(i) <> iLatPrePos
							Select Case cp.iHPPosControl
							Case 1,2	'着圧、常時
								'着圧制御待ち時間[mS]最低40ｍS間のサンプル数+5 で比較
								Dim iCompSamp As Int64 = If(cp.clDt.p_head.iPressContDelay<40,40,cp.clDt.p_head.iPressContDelay) / 2 + 5
								If i > iCompSamp
									iLatPrePos = cp.clDt.iPzBuf(i - iCompSamp)			
								Else
									iLatPrePos = cp.clDt.iPzBuf(0)
								End If
							Case Else	'GAP
								If i > 5
								'10mS毎に比較
									iLatPrePos = cp.clDt.iPzBuf(i - 5)		
								Else
									iLatPrePos = cp.clDt.iPzBuf(0)
								End If
							End Select
						Else
							If m_dLatBaseZPos = 0
							'最初の検知のみセット
								m_dLatBaseZPos	  = cp.clDt.iPzBuf(i)						'Z位置の記憶
							End if
						End If
					End Select
				End If
#else
				If cp.clDt.iPzBuf(i) > dInputPos
					If cp.clDt.iPzBuf(i) <> iLatPrePos
						If i > cp.clDt.p_head.iPressContDelay * 2
							iLatPrePos = cp.clDt.iPzBuf(i - cp.clDt.p_head.iPressContDelay * 2)			'着圧制御待ち時間[mS]*2 で 階段4つ分
						Else
							iLatPrePos = cp.clDt.iPzBuf(0)
						End If
					Else
						If m_dLatBaseZPos = 0
						'最初の検知のみセット
							m_dLatBaseZPos	  = cp.clDt.iPzBuf(i)							'Z位置の記憶
						End if
					End If
				End If
#end if
#end if
			Case ehoseiCalcMode.LTRealTime
			'長期間欠モード時
				'cp.clDt.iPzBuf(i) = cp.clDt.iPzBuf(i) / 10	+ cp.dHPPosition		'Gain 1/10 に
				PzBufUpdate(cp, i)			'Z位置測定データ更新

			Case eHoseiCalcMode.LTBatch
				If m_dLatBaseZPos = 0
				'最初の検知のみセット
					m_dLatBaseZPos	  = cp.clDt.iPzBuf(i)							'Z位置の記憶
				End if
			End Select

			
		End Select

		'微分を行う TempX_1, TempY_1---------------------------------------------------
		If i >  0		'最初の一回はやらない
			'演算式 ((X2 - X1) / (T2 - T1)) / (2 * Math.pi * dfreq )
			Select Case cp.iStrsSensMode
			Case clSpec.eStrsSens.Point
				TempX_1(k) = DifferentialProc(cp.clDt.iOzBuf(i-1), cp.clDt.iOzBuf(i), 0, dSampleTime, dFreq ) 
				TempXp_1(k) = DifferentialProc(cp.clDt.iPzBuf(i-1), cp.clDt.iPzBuf(i), 0, dSampleTime, dFreq )
			Case Else
				TempX_1(k) = DifferentialProc(cp.clDt.iOxBuf(i-1), cp.clDt.iOxBuf(i), 0, dSampleTime, dFreq ) 
				TempXp_1(k) = DifferentialProc(cp.clDt.iPxBuf(i-1), cp.clDt.iPxBuf(i), 0, dSampleTime, dFreq )
			End Select
			TempY_1(k) = DifferentialProc(cp.clDt.iOyBuf(i-1), cp.clDt.iOyBuf(i), 0, dSampleTime, dFreq )
			TempYp_1(k) = DifferentialProc(cp.clDt.iPyBuf(i-1), cp.clDt.iPyBuf(i), 0, dSampleTime, dFreq )

		end if

		'一回目の積分
		If i - iHalfPriod >= 0
		'半周期先頭読み出しポイント
			dSumX = TempX_1.Sum()                             '和を計算
			dSumY = TempY_1.Sum()                             '和を計算
			dSumPX = TempXp_1.Sum()                            '和を計算
			dSumPY = TempYp_1.Sum()                            '和を計算

			TempX_A( m ) = dSumX * dK									'和 * PI * サンプリング時間 / 周期
			TempY_A( m ) = dSumY * dK									'和 * PI * サンプリング時間 / 周期
			TempXp_A( m ) = dSumPX * dK									'和 * PI * サンプリング時間 / 周期
			TempYp_A( m ) = dSumPY * dK									'和 * PI * サンプリング時間 / 周期
		Else
			'無効データ 半周期積分元のデータが無いため
			TempX_A( m ) = 0
			TempY_A( m ) = 0
			TempXp_A( m ) = 0
			TempYp_A( m ) = 0
		End if

		'2回目 積分を行う 区間は半周期で1点ずつ移動して和を計算  TempX_Q, TempY_Q
		If i - iAllPriod >= 0
		'半周期先頭読み出しポイント
			dSumX = TempX_A.Sum()                             '和を計算
			dSumY = TempY_A.Sum()                             '和を計算
			dSumPX = TempXp_A.Sum()                            '和を計算
			dSumPY = TempYp_A.Sum()                            '和を計算

			TempX_QFP = TempX_QF
			TempY_QFP = TempY_QF
			TempXp_QFP = TempXp_QF
			TempYp_QFP = TempYp_QF

			TempX_QF	= dSumX * dK								'和 * PI * サンプリング時間 / 周期
			TempY_QF	= dSumY * dK								'和 * PI * サンプリング時間 / 周期
			TempXp_QF	= dSumPX * dK								'和 * PI * サンプリング時間 / 周期
			TempYp_QF	= dSumPY * dK								'和 * PI * サンプリング時間 / 周期

			If p <= TempX_QFB.GetUpperBound(0)
				TempX_QFB(p)	= TempX_QF
				TempY_QFB(p)	= TempY_QF
				TempXp_QFB(p)	= TempXp_QF
			End If


		Else
			'無効データ 半周期積分元のデータが無いため
			TempX_QF	= 0
			TempY_QF	= 0
			TempXp_QF	= 0
			TempYp_QF	= 0
			If p <= TempX_QFB.GetUpperBound(0)
				TempX_QFB(p)	= 0
				TempY_QFB(p)	= 0
				TempXp_QFB(p)	= 0
			End If
		End if

		Xvect	= New Vector(TempX_A(m),	TempX_QFP)
		Yvect	= New Vector(TempY_A(m),	TempY_QFP)
		Xpvect	= New Vector(TempXp_A(m),	TempXp_QFP)
		Ypvect	= New Vector(TempYp_A(m),	TempYp_QFP)
		XYvect	= New Vector(TempX_A(m),	TempY_A(m))
		XYpvect = New Vector(TempXp_A(m),	TempYp_A(m))

		If i > iIgnoreCount + iAllPriod 'i < iAllPriod					' + iAllPriod			'Double.isNaN(TempX_Q(i)) 
			Temp_T1F	= Vector.AngleBetween(Xvect, Yvect)		* Math.PI/180
			TempXp_T2F	= Vector.AngleBetween(Xpvect, Xvect)	* Math.PI/180
			TempYp_T2F	= Vector.AngleBetween(Ypvect, Yvect)	* Math.PI/180
			TempXY_T2F	= Vector.AngleBetween(XYvect, XYpvect)	* Math.PI/180
			TempXp_BF	= Xpvect.Length

			If Temp_T1F < 0
				Temp_T1F = -1 * Temp_T1F
			End If

		Else
			Temp_T1F = 0				'振幅停止中はNan	
			TempXp_T2F = 0
			TempYp_T2F = 0
			TempXY_T2F = 0
			TempXp_BF = 0
		End if
		Temp_T1Aly( n )		= Temp_T1F
		TempXp_T3Aly( n )	= TempXp_T2F
		TempYp_T3Aly( n )	= TempYp_T2F
		TempXY_T3Aly( n )	= TempXY_T2F
		TempXp_BAly( n )	= TempXp_BF

		Temp_T3AveF			= Temp_T1Aly.Average()
		TempXp_T3AveF		= TempXp_T3Aly.Average()
		TempYp_T3AveF		= TempYp_T3Aly.Average()
		TempXY_T3AveF		= TempXY_T3Aly.Average()
		TempXp_BAveF		= TempXp_BAly.Average()


		'位相角 範囲制限
		if Temp_T3AveF		> dUpperdeg									'95deg
			Temp_T3AveF = dUpperdeg
		Elseif Temp_T3AveF	<= dLowerdeg								'85deg
			Temp_T3AveF = dLowerdeg
		End If
		'位相差補正
		Temp_LF	= Math.Sin(Temp_T3AveF) * TempY_A(m)						'位相差補正 Y方向ベクトルのY成分（Xに直交する成分）配列
		Temp_MF	= Math.Cos(Temp_T3AveF) * TempY_A(m)						'位相差補正 Y方向ベクトルのX成分（Xに平行成分）配列
		Temp_NF	= TempX_A(m) - Temp_MF									'位相差補正 Y方向ベクトルのX成分で補正したX方向ベクトル

		'XYを合成 --------------------------------------------------
		Select Case cp.iStrsSensMode
		Case clSpec.eStrsSens.Circle
			If Temp_T3AveF <> 0
				Temp_GF = Math.Sqrt(Temp_NF^2 + Temp_LF^2)/ Math.Sin(Temp_T3AveF)
			Else
				Temp_GF = 0
			End If
		Case clSpec.eStrsSens.Point,clSpec.eStrsSens.Line
			Temp_GF = Math.Sqrt(Xvect.Length^2 + Yvect.Length^2)
		End Select

		'ニュートン変換
		Select Case cp.iMeas
		Case eHoseiMeasMode.FullCalib, eHoseiMeasMode.ZeroCalib
			Temp_J(n) = Temp_GF * dNConv 									'μN変換　Temp_T1範囲0～180度で表現
		Case Else						'μN変換
			Temp_J(n) = ((Temp_GF * dNConv ) - cp.dZeroStress) * cp.dAttenuation  	'μN変換
		End Select


		'移動平均
		If i < iIgnoreCount + iAllPriod+1	'<-498
			Temp_JAve(i) = 0
		Else					'周期１秒の時499からデータあり->
			Temp_JAve(i) = Temp_J.Average()
		End If

		'負数はゼロにする
		Temp_JAve(i) = if( Temp_JAve(i)<0, 0, Temp_JAve(i) )					

		'％応力
		If cp.dFullStress <> 0
			Temp_J100(i) = Temp_JAve(i) / (cp.dFullStress)
		Else
			Temp_J100(i) = 0
		End If
		If uVp.dAmplitude <> 0
			Select Case cp.iMeas
			Case eHoseiMeasMode.GelTime, eHoseiMeasMode.FlowCurve		'ゲルタイム,フローカーブ
				TempXp_Rad(i) = TempXp_T3AveF - dPPhase										'動粘弾位相オフセット補正		-90度～+90度で表現
				TempYp_Rad(i) = TempYp_T3AveF - cp.dPyPhase									'動粘弾位相オフセット補正		
				TempXY_Rad(i) = TempXY_T3AveF	-((dPPhase+cp.dPyPhase) / 2)				'動粘弾位相オフセット補正		

				TempXp_Rad(i) = If(TempXp_Rad(i) < 0, 0, TempXp_Rad(i))											'負数は０
				TempYp_Rad(i) = If(TempYp_Rad(i) < 0, 0, TempYp_Rad(i))											'負数は０
				TempXY_Rad(i) = If(TempXY_Rad(i) < 0 ,0, TempXY_Rad(i))											'負数は０

				TempXp_Rad(i) = If(TempXp_Rad(i) > Math.pi/2 , Math.pi/2, TempXp_Rad(i))
				TempYp_Rad(i) = If(TempYp_Rad(i) > Math.pi/2 , Math.pi/2, TempYp_Rad(i))
				TempXY_Rad(i) = If(TempXY_Rad(i) > Math.pi/2 , Math.pi/2, TempXY_Rad(i))

				If i < iIgnoreCount + 2 * (iAllPriod+1)
					TempXp_Rad(i) = 0
					TempYp_Rad(i) = 0
					TempXY_Rad(i) = 0
				End If


			Case eHoseiMeasMode.FullCalib, eHoseiMeasMode.ZeroCalib							'キャリブレーション
				'動的粘弾性	'振幅0時はゼロ
				'TempXp_Rad(i) = If(uVp.dAmplitude <> 0 Or TempXp_T3AveF > 0, TempXp_T3AveF, 2)'Xモードで使用するcp.dPxPhaseになる
				'TempYp_Rad(i) = If(uVp.dAmplitude <> 0 Or TempYp_T3AveF > 0, TempYp_T3AveF, 2)'XYモードで使用するcp.dPyPhaseになる
				TempXp_Rad(i) = TempXp_T3AveF					'Xモードで使用するcp.dPxPhaseになる
				TempYp_Rad(i) = TempYp_T3AveF					'XYモードで使用するcp.dPyPhaseになる
			End Select

		Else
			TempXp_Rad(i) = 0
			TempYp_Rad(i) = 0
			TempXY_Rad(i) = 0
		End If


		Select Case cp.iStrsSensMode
		Case clSpec.eStrsSens.Circle
			dXYSin = Math.Sin(TempXY_Rad(i))										'XY応力とXY位置の位相差の位相オフセット補正
			dXYCos = Math.Cos(TempXY_Rad(i))
		Case clSpec.eStrsSens.Point,clSpec.eStrsSens.Line
			dXYSin = Math.Sin(TempXp_Rad(i))										'XY応力とXY位置の位相差の位相オフセット補正
			dXYCos = Math.Cos(TempXp_Rad(i))
		End Select

		dRTemp_J100F = 1 - (Temp_J100(i) * dXYCos)									'貯蔵弾性率成分で振幅が小さくなる

		'dRTemp_J100Fを0-1へ制限する (粘度ハンチング防止として)
		If dRTemp_J100F < 0
			dRTemp_J100F = 0
		ElseIf dRTemp_J100F > 1
			dRTemp_J100F = 0
		End If

		Temp_J100(i) *= 100.0

		Dim dRealMeasGap As Double
		Select Case cp.iStrsSensMode
		Case clSpec.eStrsSens.Point
		'点モードでの粘度計算
			Temp_V(i) = 0
			Temp_SS(i) = 0
			Temp_SV(i) = 0
			Temp_Stn(i) = 0
			Temp_SM(i) = 0
			TempXp_G1(i) = 0
			TempYp_G1(i) = 0
			TempXp_G2(i) = 0
			TempYp_G2(i) = 0

			Select Case cp.iHPPosControl
			Case 1,2		'熱板着圧制御,常時圧力
				Dim dZStrsAD As Int64 = 0
				dZStrsAD = CalcMeasureStrsToADValue(cp.dMeasureStress, cp.dDiameter, cp.cts.WeightCoefZ)
				uVp.dMeasGap		= (cp.iZeroPos - cp.dRCCBaseThickness*1E3 - cp.clDt.iPzBuf(i) + (1/cp.cts.GapCorr) * dZStrsAD) / 10^6
				dRealMeasGap		= (cp.iZeroPos - cp.dRCCBaseThickness*1E3 - m_dLatBaseZPos + (1/cp.cts.GapCorr) * dZStrsAD) / 10^3
			Case Else
				uVp.dMeasGap		= (cp.iZeroPos - cp.dRCCBaseThickness*1E3 - cp.cldt.iPzBuf(i)) / 10^6					'測定GAP(m)
				dRealMeasGap		= (cp.iZeroPos - cp.dRCCBaseThickness*1E3 - m_dLatBaseZPos) / 10^3		'測定GAP(mm)
			End Select
			If m_dLatBaseZPos <> 0
				cp.dMesGap = dRealMeasGap												'モニタ用測定GAP(mm)
			End If

			uVp.dMeasGap		= uVp.dMeasGap / 10^3
			uVp.dMeasStress		= Temp_JAve(i) / 10^6									'測定応力(N)
			uVp.dAmplitude		= Math.Round(TempXp_BAveF) / 10^6						'設定振幅(m)
			ViscosityCalcZ( uVp )														'粘度計算処理
			Temp_V(i) = uVp.dViscosity
			Temp_SS(i) = uVp.dShearStress												'せん断応力
			Temp_SV(i) = uVp.dShearVelocity												'せん断速度


		Case Else
		'円、直線モードでの粘度計算
			If i > iIgnoreCount + 2 * (iAllPriod+1)
				uVp.dMeasStress		= Temp_JAve(i) / 10^6									'測定応力(N)
				uVp.dVelocityRate	= dRTemp_J100F											'せん断速度補正　貯蔵弾性率に応じて可変する
				If cp.clDt.p_head.sTempProfile=""
				'温度プロファイル使用無し
					Select Case cp.iHPPosControl
					Case 1		'熱板着圧制御
						Dim dZStrsAD As Int64 = 0
						dZStrsAD = CalcMeasureStrsToADValue(cp.dMeasureStress, cp.dDiameter, cp.cts.WeightCoefZ)
						uVp.dMeasGap		= (cp.iZeroPos - cp.dRCCBaseThickness*1E3 - m_dLatBaseZPos + (1/cp.cts.GapCorr) * dZStrsAD) / 10^6			'測定GAP(m)
					Case 2		'常時圧力
						Dim dZStrsAD As Int64 = 0
						dZStrsAD = CalcMeasureStrsToADValue(cp.dMeasureStress, cp.dDiameter, cp.cts.WeightCoefZ)
						uVp.dMeasGap		= (cp.iZeroPos - cp.dRCCBaseThickness*1E3 - cp.clDt.iPzBuf(i) + (1/cp.cts.GapCorr) * dZStrsAD) / 10^6			'測定GAP(m)
					Case Else
						uVp.dMeasGap		= (cp.iZeroPos - cp.dRCCBaseThickness*1E3 - m_dLatBaseZPos) / 10^6			'測定GAP(m)
					End Select
				Else
				'温度プロファイル使用
					Select Case cp.iHPPosControl
					Case 1		'熱板着圧制御
						Dim dZStrsAD As Int64 = 0
						dZStrsAD = CalcMeasureStrsToADValue(cp.dMeasureStress, cp.dDiameter, cp.cts.WeightCoefZ)
						uVp.dMeasGap		= (cp.iZeroPos - cp.dRCCBaseThickness*1E3 - m_dLatBaseZPos + (1/cp.cts.GapCorr) * dZStrsAD) / 10^6			'測定GAP(m)
					Case 2		'常時圧力
						Dim dZStrsAD As Int64 = 0
						dZStrsAD = CalcMeasureStrsToADValue(cp.dMeasureStress, cp.dDiameter, cp.cts.WeightCoefZ)
						uVp.dMeasGap		= (cp.iZeroPos - cp.dRCCBaseThickness*1E3 - cp.clDt.iPzBuf(i) + (1/cp.cts.GapCorr) * dZStrsAD) / 10^6			'測定GAP(m)
					Case Else
						uVp.dMeasGap		= (cp.iZeroPos - cp.dRCCBaseThickness*1E3 - m_dLatBaseZPos) / 10^6			'測定GAP(m)
					End Select
					'If m_dLatBaseZPos = 0
					'	uVp.dMeasGap		= 0
					'End If
				End If
				'測定GAPの共通0制限（着圧決定するまでは０とする）
				If m_dLatBaseZPos = 0
					uVp.dMeasGap		= 0
				End If
				cp.dMesGap = uVp.dMeasGap * 10^3											'モニタ用測定GAP(mm)
				IF TempXp_BAveF <= 0
					uVp.dAmplitude		= 0													'設定振幅(m)
				Else
					Select Case cp.dAmplitude * 1000.0
					Case <= iSwitchAmpXYPos
					'0.1um解像度
						uVp.dAmplitude		= Math.Round(TempXp_BAveF * 2 / 10, 0) / 10^6		'設定振幅(m)
					Case Else
					'1um解像度
						uVp.dAmplitude		= TempXp_BAveF * 2 / 10^6							'設定振幅(m)
					End Select
				End If

'Debug.Print("MesGAP=" & uVp.dMeasGap & "    Amp=" & uVp.dAmplitude ) 

				ViscosityCalc( uVp )														'粘度計算処理
				Temp_V(i) = uVp.dViscosity
				Temp_SS(i) = uVp.dShearStress												'せん断応力
				Temp_SV(i) = uVp.dShearVelocity												'せん断速度
'Debug.Print("vp.dShearVelocity=" & uVp.dShearVelocity & "    MesGAP=" & uVp.dMeasGap & "    Amp=" & uVp.dAmplitude )
If i>=1
'Debug.Print(uvp.dAmplitude &  "  uVp.dShearVelocity=" & uVp.dShearVelocity)
End if

				'動的粘弾性'振幅0時はゼロ
				If uVp.dMeasGap > 0 And uVp.dAmplitude <> 0
					Temp_Stn(i)	 = ((uVp.dAmplitude * dRTemp_J100F) / 2) / uVp.dMeasGap		'サンプルに与えたひずみ
					Temp_SM(i)	 = Temp_SS(i) / Temp_Stn(i)									'ずり弾性率G*
					TempXp_G1(i) = Temp_SM(i) * dXYCos					'X貯蔵弾性率
					TempXp_G2(i) = Temp_SM(i) * dXYSin					'X損失弾性率
					TempYp_G1(i) = Temp_SM(i) * dXYCos					'Y貯蔵弾性率
					TempYp_G2(i) = Temp_SM(i) * dXYSin					'Y損失弾性率
				Else
					Temp_Stn(i)	 = 0
					Temp_SM(i)	 = 0	
					TempXp_G1(i) = 0
					TempXp_G2(i) = 0
					TempYp_G1(i) = 0
					TempYp_G2(i) = 0
				End If
			Else
				Temp_V(i)	 = 0
				Temp_SS(i)	 = 0
				Temp_SV(i)	 = 0
				Temp_Stn(i)	 = 0
				Temp_SM(i)	 = 0	
				TempXp_G1(i) = 0
				TempXp_G2(i) = 0
				TempYp_G1(i) = 0
				TempYp_G2(i) = 0
			end if
		End Select

		If (i > iIgnoreCount + 2 * (iAllPriod+1)) And uVp.dAmplitude <> 0
			Select Case cp.iStrsSensMode
			Case clSpec.eStrsSens.Circle
				If TempXY_Rad(i) >= Math.PI / 2
					TempXp_Tn(i) = 0
					TempYp_Tn(i) = 0
				Else
					TempXp_Tn(i) = Math.Tan(TempXY_Rad(i))
					TempYp_Tn(i) = Math.Tan(TempXY_Rad(i))	
				End If
			Case clSpec.eStrsSens.Point,clSpec.eStrsSens.Line
				If TempXp_Rad(i) >= Math.PI / 2
					TempXp_Tn(i) = 0
				Else
					TempXp_Tn(i) = Math.Tan(TempXp_Rad(i))
				End If

				If TempYp_Rad(i) >= Math.PI / 2
					TempYp_Tn(i) = 0
				Else
					TempYp_Tn(i) = Math.Tan(TempYp_Rad(i))
				End If
			End Select

			TempXp_Tn(i) = If(TempXp_Tn(i)< 0, 0, TempXp_Tn(i))								'X損失正接(負数は０)
			TempYp_Tn(i) = If(TempYp_Tn(i)< 0, 0, TempYp_Tn(i))								'Y損失正接(負数は０)	'%変換
		Else
			TempXp_Tn(i) = 0
			TempYp_Tn(i) = 0
		End if

		Dim iLen As Int64 = 0
		Dim iSrt As Int64 = 0
		Dim iErt As Int64 = 0


		Dim dDeltaX	As double = iRangeNum * (MEAS_TIME_RESOLUTION / 1000.0)

		' (i - iRangeNum >=  iQuotPriod+1) Temp_JAveのデータ数が微分可能になるまで　iQuotPriod+1以上必要
		' (i - iRoundNum >= iIgnoreCount + iQuotPriod+1)　微分後のデータがiIgnoreCount+iQuotPriod+1よりも後になる必要がある
		If (i - iRangeNum >=  iQuotPriod+1) And (i - iRoundNum >= iIgnoreCount + iQuotPriod+1)
			if cp.dDiffInterval <> 0
				Temp_SL(i) = (Temp_JAve(i) - Temp_JAve(i -iRangeNum)) / dDeltaX
			Else
				Temp_SL(i) = 0
			End If
		End If

		if i = (m_iOXBuf_R_pt + cp.iSampleNum - 1)'受信ブロックの最後のデータを処理するとき
			Select Case cp.iMeas
			Case eHoseiMeasMode.GelTime		'ゲルタイム
#If 1
				If Not p_cts.LTModeEnable
					'1/4周期が1秒を超えるときの対応 20251211 ------
					If cp.clDt.iOzBuf.Length <= (iQuotPriod + 1) '(1/4周期)のデータがまだないとき
						iSrt = m_iOXBuf_R_pt
						iErt = m_iOXBuf_R_pt
						iLen = cp.iSampleNum
						iLatShiftOneTime = 0

					Else '(1/4周期)以上のデータがあるとき
						If iLatShiftOneTime = 0
							'1/4周期溜まった後の1回目
							iLatShiftOneTime = 1
							iSrt = iQuotPriod + 1
							iErt = 0
							iLen = m_iOXBuf_R_pt + cp.iSampleNum - iSrt
						Else
							'1/4周期溜まった後の2回目
							iSrt = m_iOXBuf_R_pt
							iErt = m_iOXBuf_R_pt  - (iQuotPriod	+ 1)
							iLen = cp.iSampleNum
						End If
					End If
					'1/4周期が1秒を超えるときの対応 20251211 ------
				Else
					iSrt = m_iOXBuf_R_pt								'LTモード測定の時は1/4周期シフトしない
					iErt = m_iOXBuf_R_pt
					iLen = cp.iSampleNum
				End If
#else
				If cp.clDt.iOzBuf.Length <= cp.iSampleNum
					iSrt = m_iOXBuf_R_pt + (iQuotPriod + 1)				'1回目の受信処理（積分による遅れ分を(1/4周期)前シフト）
					iErt = cp.clDt.dXPosPhase.Length-cp.iSampleNum
					iLen = cp.iSampleNum - iSrt
				Else
					iSrt = m_iOXBuf_R_pt								'2回目の受信処理
					iErt = m_iOXBuf_R_pt  - (iQuotPriod	+ 1)
					iLen = cp.iSampleNum
				End If
#end if
			Case Else
				iSrt = m_iOXBuf_R_pt								'キャリブレーションとフロー測定の時は1/4周期シフトしない
				iErt = m_iOXBuf_R_pt
				iLen = cp.iSampleNum
			End Select
			Select Case cp.iMeas		
			Case eHoseiMeasMode.GelTime		'ゲルタイム
				'処理済 補正データをセット （積分による遅れ分を(1/4周期)前シフト） ---------------------------------------------
				Select Case cp.iStrsSensMode
				Case clSpec.eStrsSens.Circle
					Array.Copy(TempXY_Rad,	iSrt, cp.clDt.dXPosPhase, iErt,	iLen)		'X位相差
					Array.Copy(TempXY_Rad,	iSrt, cp.clDt.dYPosPhase, iErt,	iLen)		'Y位相差
				Case clSpec.eStrsSens.Point,clSpec.eStrsSens.Line
					Array.Copy(TempXp_Rad,	iSrt, cp.clDt.dXPosPhase, iErt,	iLen)		'X位相差
					Array.Copy(TempXp_Rad,	iSrt, cp.clDt.dYPosPhase, iErt,	iLen)		'Y位相差
				End Select

			Case Else
				iSrt = m_iOXBuf_R_pt
				Array.Copy(TempXp_Rad,	iSrt, cp.clDt.dXPosPhase, iErt,	iLen)		'X位相差
				Array.Copy(TempYp_Rad,	iSrt, cp.clDt.dYPosPhase, iErt,	iLen)		'Y位相差
			End select
				
			Array.Copy(Temp_JAve,	iSrt, cp.clDt.iHosei,		iErt,	iLen)		'応力値
			Array.Copy(Temp_J100,	iSrt, cp.clDt.dHosei100,	iErt,	iLen)		'応力値%
			Array.Copy(Temp_V,		iSrt, cp.clDt.dViscosity,	iErt,	iLen)		'粘度
			Array.Copy(TempXp_G1,	iSrt, cp.clDt.dXStrgMod,	iErt,	iLen)		'X貯蔵弾性率
			Array.Copy(TempXp_G2,	iSrt, cp.clDt.dXElasMod,	iErt,	iLen)		'X損失弾性率
			Array.Copy(TempXp_Tn,	iSrt, cp.clDt.dXLossTan,	iErt,	iLen)		'X損失正接
			Array.Copy(TempYp_G1,	iSrt, cp.clDt.dYStrgMod,	iErt,	iLen)		'Y貯蔵弾性率
			Array.Copy(TempYp_G2,	iSrt, cp.clDt.dYElasMod,	iErt,	iLen)		'Y損失弾性率
			Array.Copy(TempYp_Tn,	iSrt, cp.clDt.dYLossTan,	iErt,	iLen)		'Y損失正接
			Array.Copy(Temp_Stn,	iSrt, cp.clDt.dSplStrain,	iErt,	iLen)		'サンプルに与えたひずみ
			Array.Copy(Temp_SM,		iSrt, cp.clDt.dShearMod,	iErt,	iLen)		'せん断弾性率
			Array.Copy(Temp_SV,		iSrt, cp.clDt.dShearVelo,	iErt,	iLen)		'せん断速度
			Array.Copy(Temp_SS,		iSrt, cp.clDt.dShearStress,	iErt,	iLen)		'せん断応力

			Select Case cp.iMeas		
			Case eHoseiMeasMode.GelTime
				'1回目のslope処理
				If cp.clDt.iOzBuf.Length <= cp.iSampleNum
					Dim iSrt1 As Int64
					iSrt1 = iRoundNum + iQuotPriod +1
					If iSrt1 <= Temp_SL.Length
						Array.Copy(Temp_SL,	iSrt1,	cp.clDt.iSlope,		iErt,	cp.iSampleNum - iSrt1)
					End If

				'2回目以降のslope処理
				Else
					Dim iErt1 As Int64
					If iErt  >= iRoundNum								
						iErt1 = iErt - iRoundNum
						Array.Copy(Temp_SL,	m_iOXBuf_R_pt, cp.clDt.iSlope,	iErt1,	cp.iSampleNum)
					End If
				End If
			End select
#If 1
			Select Case cp.iMeas
			Case eHoseiMeasMode.GelTime		'ゲルタイム
				If cp.clDt.iHosei.Length - iQuotPriod > 0		'インデックス負数を制限
					Dim j As Int64 = 0
					'前シフトした分最終部分を埋める（NAN）
					For j = cp.clDt.iHosei.Length - (iQuotPriod	+ 1) To cp.clDt.iHosei.GetUpperBound(0)
#If 0
						cp.clDt.iHosei(j)		= cp.clDt.iHosei(j-1)
						cp.clDt.dHosei100(j)	= cp.clDt.dHosei100(j-1)
						cp.clDt.dViscosity(j)	= cp.clDt.dViscosity(j-1)
						cp.clDt.dXPosPhase(j)	= cp.clDt.dXPosPhase(j-1)
						cp.clDt.dXStrgMod(j)	= cp.clDt.dXStrgMod(j-1)
						cp.clDt.dXElasMod(j)	= cp.clDt.dXElasMod(j-1)
						cp.clDt.dXLossTan(j)	= cp.clDt.dXLossTan(j-1)
						cp.clDt.dYPosPhase(j)	= cp.clDt.dYPosPhase(j-1)
						cp.clDt.dYStrgMod(j)	= cp.clDt.dYStrgMod(j-1)
						cp.clDt.dYElasMod(j)	= cp.clDt.dYElasMod(j-1)
						cp.clDt.dYLossTan(j)	= cp.clDt.dYLossTan(j-1)
						cp.clDt.dSplStrain(j)	= cp.clDt.dSplStrain(j-1)
						cp.clDt.dShearMod(j)	= cp.clDt.dShearMod(j-1)
						cp.clDt.dShearVelo(j)	= cp.clDt.dShearVelo(j-1)
						cp.clDt.iSlope(j)		= cp.clDt.iSlope(j-1)
#else
						cp.clDt.iHosei(j)		= Double.NaN
						cp.clDt.dHosei100(j)	= Double.NaN
						cp.clDt.dViscosity(j)	= Double.NaN
						cp.clDt.dXPosPhase(j)	= Double.NaN
						cp.clDt.dXStrgMod(j)	= Double.NaN
						cp.clDt.dXElasMod(j)	= Double.NaN
						cp.clDt.dXLossTan(j)	= Double.NaN
						cp.clDt.dYPosPhase(j)	= Double.NaN
						cp.clDt.dYStrgMod(j)	= Double.NaN
						cp.clDt.dYElasMod(j)	= Double.NaN
						cp.clDt.dYLossTan(j)	= Double.NaN
						cp.clDt.dSplStrain(j)	= Double.NaN
						cp.clDt.dShearMod(j)	= Double.NaN
						cp.clDt.dShearVelo(j)	= Double.NaN
						cp.clDt.dShearStress(j)	= Double.NaN
						cp.clDt.iSlope(j)		= Double.NaN
#end if
					Next
				End if
			Case eHoseiMeasMode.FlowCurve		'フローカーブ
				
			End Select
#end if
		End If

#If DEBUGFILEOUT
'If i=500
'Debug.Print("")
'End If
	If p_bCalcResultFOutEn		'隠しコマンドで有効か
		sLineBuf = i.ToString & "," & _
		cp.clDt.iOxBuf(i).ToString & "," & _
		cp.clDt.iOyBuf(i).ToString & "," & _
		cp.clDt.iOzBuf(i).ToString & "," & _
		p_dtFile.iPxBuf(i).ToString & "," & _
		p_dtFile.iPyBuf(i).ToString & "," & _
		p_dtFile.iPzBuf(i).ToString & "," & _
		"," & _
		k.ToString & "," & _
		TempX_1(k).ToString & "," & _
		TempY_1(k).ToString & "," & _
		TempXp_1(k).ToString & "," & _
		TempYp_1(k).ToString & "," & _
		"," & _
		(m).ToString & "," & _
		TempX_A(m).ToString & "," & _
		TempY_A(m).ToString & "," & _
		TempXp_A(m).ToString & "," & _
		TempYp_A(m).ToString & "," & _
		"," & _
		TempX_QF.ToString & "," & _
		TempY_QF.ToString & "," & _
		TempXp_QF.ToString & "," & _
		TempYp_QF.ToString & "," & _
		"," & _
		TempX_QFP.ToString & "," & _
		TempY_QFP.ToString & "," & _
		TempXp_QFP.ToString & "," & _
		TempYp_QFP.ToString & "," & _
		"," & _
		Temp_T1F.ToString & "," & _
		TempXp_T2F.ToString & "," & _
		TempYp_T2F.ToString & "," & _
		TempXY_T2F.ToString & "," & _
		TempXp_BF.ToString & "," & _
		"," & _
		n.ToString & "," & _
		Temp_T1Aly(n).ToString & "," & _
		TempXp_T3Aly(n).ToString & "," & _
		TempYp_T3Aly(n).ToString & "," & _
		TempXY_T3Aly(n).ToString & "," & _
		TempXp_BAly(n).ToString & "," & _
		"," & _
		Temp_T3AveF.ToString & "," & _
		TempXp_T3AveF.ToString & "," & _
		TempYp_T3AveF.ToString & "," & _
		TempXY_T3AveF.ToString & "," & _
		TempXp_BAveF.ToString & "," & _
		"," & _
		Temp_LF.ToString & "," & _
		Temp_MF.ToString & "," & _
		Temp_NF.ToString & "," & _
		Temp_GF.ToString & "," & _
		"," & _
		Temp_J(n).ToString & "," & _
		Temp_JAve(i).ToString & "," & _
		Temp_J100(i).ToString & "," & _
		"," & _
		TempXp_Rad(i).ToString & "," & _
		TempYp_Rad(i).ToString & "," & _
		TempXY_Rad(i).ToString & ","

		swr.WriteLine(sLineBuf)
	End If
#End If
		'微分結果書込みポインタ更新
		If k >= TempX_1.GetUpperBound(0)
			k = 0
		Else
			k += 1
		End If
		'1回目積分書込みポインタ更新
		If m >= TempX_A.GetUpperBound(0)
			m = 0
		Else
			m += 1
		End If
		'移動平均書込みポインタ更新
		If n >= iAllPriod
			n = 0
		Else
			n += 1
		End If
		'波形モニタ用バッファ書込みポインタ更新
		If p >= TempX_QFB.GetUpperBound(0)
			p = 0
		Else
			p += 1
		End If

	Next i
'==================================================================================
'==================================================================================
#If DEBUGFILEOUT
	If p_bCalcResultFOutEn		'隠しコマンドで有効か
		swr.Close()
		swr = Nothing
	End If
#End If
	'バッチ処理の最後に1/4周期シフト実行フラグクリア
	If iBranch = 0
		iLatShiftOneTime = 0
	End If

	'リアルタイム数値モニタ表示用(frmMain) 
	Dim iMonIndex As Int64 = (cp.clDt.iOxBuf.Length - 1) - (iQuotPriod + 1)
	iMonIndex = If(iMonIndex<0, 0, iMonIndex)					'インデックス負数を制限

	cp.clDt.p_head.dZAfterPos = m_dLatBaseZPos					'ヘッダへ着圧後Z位置をセット
#If 1
	'1/4シフトの後ろ空きNANに対応
	Select Case cp.iStrsSensMode
	Case clSpec.eStrsSens.Point
		cp.clDt.p_curStress		= cp.clDt.iHosei(iMonIndex)		'応力値
		cp.clDt.p_curStressP	= cp.clDt.dHosei100(iMonIndex)	'応力値%
		cp.clDt.p_curViscosity	= cp.clDt.dViscosity(iMonIndex)	'粘度
		cp.clDt.p_curPhase		= cp.clDt.dXPosPhase(iMonIndex)	'X位相差
		cp.clDt.p_curSplStrain = 0                              'サンプルに与えたひずみ
		cp.clDt.p_curShearMod = 0                               'ずり弾性率
		cp.clDt.p_curShearVelo = 0                              '剪断速度
		cp.clDt.p_curStrgMod = 0                                'X貯蔵弾性率
		cp.clDt.p_curElasMod = 0                                'X損失弾性率

	Case Else
		cp.clDt.p_curStress		= cp.clDt.iHosei(iMonIndex)		'応力値
		cp.clDt.p_curStressP	= cp.clDt.dHosei100(iMonIndex)	'応力値%
		cp.clDt.p_curViscosity	= cp.clDt.dViscosity(iMonIndex)	'粘度
		cp.clDt.p_curPhase		= cp.clDt.dXPosPhase(iMonIndex)	'X位相差
		cp.clDt.p_curSplStrain	= cp.clDt.dSplStrain(iMonIndex)	'サンプルに与えたひずみ
		cp.clDt.p_curShearMod	= cp.clDt.dShearMod(iMonIndex)	'ずり弾性率
		cp.clDt.p_curShearVelo	= cp.clDt.dShearVelo(iMonIndex)	'剪断速度
		cp.clDt.p_curStrgMod	= cp.clDt.dXStrgMod(iMonIndex)	'X貯蔵弾性率
		cp.clDt.p_curElasMod	= cp.clDt.dXElasMod(iMonIndex)	'X損失弾性率
		cp.clDt.p_curLosTan		= cp.clDt.dXLossTan(iMonIndex)  'X損失正接
	End Select
#else
	Select Case cp.iStrsSensMode
	Case clSpec.eStrsSens.Point
		cp.clDt.p_curStress		= cp.clDt.iHosei(cp.clDt.iHosei.Length-cp.iSampleNum)								'応力値
		cp.clDt.p_curStressP	= cp.clDt.dHosei100(cp.clDt.dHosei100.Length-cp.iSampleNum)							'応力値%
		cp.clDt.p_curViscosity	= cp.clDt.dViscosity(cp.clDt.dViscosity.Length-cp.iSampleNum)						'粘度
		cp.clDt.p_curPhase		= cp.clDt.dXPosPhase(cp.clDt.dXPosPhase.Length-cp.iSampleNum)						'X位相差
		cp.clDt.p_curSplStrain = 0                              'サンプルに与えたひずみ
		cp.clDt.p_curShearMod = 0                               'ずり弾性率
		cp.clDt.p_curShearVelo = 0                              '剪断速度
		cp.clDt.p_curStrgMod = 0                                'X貯蔵弾性率
		cp.clDt.p_curElasMod = 0                                'X損失弾性率

	Case Else
		cp.clDt.p_curStress		= cp.clDt.iHosei(cp.clDt.iHosei.Length-cp.iSampleNum)								'応力値
		cp.clDt.p_curStressP	= cp.clDt.dHosei100(cp.clDt.dHosei100.Length-cp.iSampleNum)							'応力値%
		cp.clDt.p_curViscosity	= cp.clDt.dViscosity(cp.clDt.dViscosity.Length-cp.iSampleNum)						'粘度
		cp.clDt.p_curPhase		= cp.clDt.dXPosPhase(cp.clDt.dXPosPhase.Length-cp.iSampleNum)						'X位相差
		cp.clDt.p_curSplStrain	= cp.clDt.dSplStrain(cp.clDt.dSplStrain.Length-cp.iSampleNum)						'サンプルに与えたひずみ
		cp.clDt.p_curShearMod	= cp.clDt.dShearMod(cp.clDt.dShearMod.Length-cp.iSampleNum)							'ずり弾性率
		cp.clDt.p_curShearVelo	= cp.clDt.dShearVelo(cp.clDt.dShearVelo.Length-cp.iSampleNum)						'剪断速度
		cp.clDt.p_curStrgMod	= cp.clDt.dXStrgMod(cp.clDt.dXStrgMod.Length-cp.iSampleNum)							'X貯蔵弾性率
		cp.clDt.p_curElasMod	= cp.clDt.dXElasMod(cp.clDt.dXElasMod.Length-cp.iSampleNum)							'X損失弾性率
		cp.clDt.p_curLosTan		= cp.clDt.dXLossTan(cp.clDt.dXLossTan.Length-cp.iSampleNum)                         'X損失正接
	End Select
#end if
	'リアルタイム時のみ波形モニタ用バッファ作成
	If iBranch = 1
		If TempX_QM.Length < MEAS_BLOCK_DATA_NUM * 12 * cp.dPeriod
			Array.Resize(TempX_QM, TempX_QM.Length + TempX_QFB.Length)
			Array.Resize(TempY_QM, TempY_QM.Length + TempY_QFB.Length)
			If Not IsNothing(TempXp_QM)
				Array.Resize(TempXp_QM, TempXp_QM.Length + TempXp_QFB.Length)
				Array.Resize(TempYp_QM, TempYp_QM.Length + TempYp_QFB.Length)
			End If
			Array.Copy(TempX_QFB, 0, TempX_QM, TempX_QM.GetUpperBound(0)-TempX_QFB.GetUpperBound(0), TempX_QFB.Length)		'ワークをコピー
			Array.Copy(TempY_QFB, 0, TempY_QM, TempY_QM.GetUpperBound(0)-TempY_QFB.GetUpperBound(0), TempY_QFB.Length)		'ワークをコピー

			If Not IsNothing(TempXp_QM)
				Array.Copy(TempXp_QFB, 0, TempXp_QM, TempXp_QM.GetUpperBound(0)-TempXp_QFB.GetUpperBound(0), TempXp_QFB.Length)	'ワークをコピー
				Array.Copy(TempYp_QFB, 0, TempYp_QM, TempYp_QM.GetUpperBound(0)-TempYp_QFB.GetUpperBound(0), TempYp_QFB.Length)	'ワークをコピー
			End If
		Else
			Array.Copy(TempX_QM, TempX_QFB.GetUpperBound(0)+1, TempX_QM, 0, TempX_QM.Length-TempX_QFB.Length)				'1受信分を左シフト
			Array.Copy(TempY_QM, TempY_QFB.GetUpperBound(0)+1, TempY_QM, 0, TempY_QM.Length-TempY_QFB.Length)				'1受信分を左シフト
			Array.Copy(TempXp_QM, TempXp_QFB.GetUpperBound(0)+1, TempXp_QM, 0, TempXp_QM.Length-TempXp_QFB.Length)				'1受信分を左シフト
			Array.Copy(TempYp_QM, TempYp_QFB.GetUpperBound(0)+1, TempYp_QM, 0, TempYp_QM.Length-TempYp_QFB.Length)				'1受信分を左シフト

			Array.Copy(TempX_QFB, 0, TempX_QM, TempX_QM.GetUpperBound(0)-TempX_QFB.GetUpperBound(0), TempX_QFB.Length)		'ワークをコピー
			Array.Copy(TempY_QFB, 0, TempY_QM, TempY_QM.GetUpperBound(0)-TempY_QFB.GetUpperBound(0), TempY_QFB.Length)				'ワークをコピー
			Array.Copy(TempXp_QFB, 0, TempXp_QM, TempXp_QM.GetUpperBound(0)-TempXp_QFB.GetUpperBound(0), TempXp_QFB.Length)				'ワークをコピー
			Array.Copy(TempYp_QFB, 0, TempYp_QM, TempYp_QM.GetUpperBound(0)-TempYp_QFB.GetUpperBound(0), TempYp_QFB.Length)				'ワークをコピー
		End If
	End If


	m_iOXBuf_R_pt += cp.iSampleNum										'TempX_1の読み出しポイント更新
	m_iSumAry1_W_pt = k
	m_iSumAryA_W_pt = m
	m_iMoveAve_W_pt = n
	m_iWaveBuf_W_pt = p
		
	'IIRフィルタまたはスキップ
	Select Case iBranch
	Case 0		'キャリブレーションまたは最終測定のとき	
		Select Case cp.iMeas
		Case eHoseiMeasMode.GelTime		'ゲルタイム
			If cp.fltSet.iEnable = 1
				PostFilterProc(cp)		'計算後デジタルフィルタ処理
			Else
			'スキップ
			End If
		End Select

	Case Else
	End Select


Sw.Stop()
#If 1
'使用メモリ監視
' 合計物理メモリ
Dim totalPhysicalMemoryByte As Decimal = CDec(My.Computer.Info.TotalPhysicalMemory)
Dim totalPhysicalMemoryKB As Decimal = totalPhysicalMemoryByte / 1024
Dim totalPhysicalMemoryMB As Decimal = totalPhysicalMemoryByte / 1024 / 1024
Dim totalPhysicalMemoryGB As Decimal = totalPhysicalMemoryByte / 1024 / 1024 / 1024

' 利用可能な物理メモリ
Dim availablePhysicalMemoryByte As Decimal = CDec(My.Computer.Info.AvailablePhysicalMemory)
Dim availablePhysicalMemoryKB As Decimal = availablePhysicalMemoryByte / 1024
Dim availablePhysicalMemoryMB As Decimal = availablePhysicalMemoryByte / 1024 / 1024
Dim availablePhysicalMemoryGB As Decimal = availablePhysicalMemoryByte / 1024 / 1024 / 1024

' メモリ使用量を計算
Dim usedPhysicalMemoryByte As Decimal = totalPhysicalMemoryByte - availablePhysicalMemoryByte
Dim usedPhysicalMemoryKB As Decimal = usedPhysicalMemoryByte / 1024
Dim usedPhysicalMemoryMB As Decimal = usedPhysicalMemoryByte / 1024 / 1024
Dim usedPhysicalMemoryGB As Decimal = usedPhysicalMemoryByte / 1024 / 1024 / 1024

'Debug.Print("Calc Elapsed(mS),{0:F0},T,{1:F0},A,{2:F0},U,{3:F0}", Sw.Elapsed.TotalMilliseconds,totalPhysicalMemoryMB,availablePhysicalMemoryMB,usedPhysicalMemoryMB)
#end if

	Return 0

Catch ex As OverflowException
	'Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message & ex.StackTrace)
	Return -1

Catch ex As Exception
	'Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message & ex.StackTrace)
	Return -1
Finally
	bBusy = False
End Try
End Function
Dim bPreXYZAct As Boolean = False
''' <summary>
''' Z位置追跡ラッチ処理
''' 最終測定位置見つけてZ位置測定データを更新する
''' </summary>
''' <param name="cp"></param>
''' <param name="i"></param>
Private Sub PzBufUpdate(ByRef cp As stCalcPara, ByVal i As int32)
Dim iMeasGap	As int32
Try
	'iPzBufは投入位置にきたとき以降 0um（1um精度）になり以降は10倍され0.1um精度となる
	If cp.clDt.iPzBuf(i) = 0 AndAlso iLatPrePzBuf <> 0 AndAlso iLatSign = 0
	'Z位置が着圧位置基準で0になったことを検知した
		iLatSign		= 1
		iLatPrePzBuf	+= dInputPos
	End If
	If iLatSign = 1
	'着圧によりZ位置が決定して以降cp.clDt.iPzBuf(i)はゼロになる、点モードの時は0起点に振幅する
		cp.clDt.iPzBuf(i) = (cp.clDt.iPzBuf(i) / 10) + iLatPrePzBuf			'Gain 1/10 にして 落差を足す
		If m_dLatBaseZPos = 0
		'最初の検知のみセット
			m_dLatBaseZPos		= cp.clDt.iPzBuf(i)							'Z位置の記憶
			iLatTimeForZPos		= i * 2										'経過時間(mS)
			iMeasGap			= (cp.iZeroPos - cp.cldt.iPzBuf(i))			'測定GAP(um)
			Select Case cp.iHPPosControl
			Case  clSpec.eHCont.OrderP	'着圧制御
				Select Case cp.iStrsSensMode
				Case clSpec.eStrsSens.Point	'点は監視無し
				Case Else				'円、直線
					If cp.dMeasGap*1000 <= iMeasGap
					'警告
						p_iFailedOrderP = 1
					End If
				End Select
			Case Else
			End Select
			If p_RTM(0).p_mltcp.isStatXYAction
			'メッセージON
				p_iGuideXYbeforeGap = 1
			End If
		End if
	Else
	'着圧調整中のみ
		iLatPrePzBuf = cp.clDt.iPzBuf(i)
		cp.clDt.iPzBuf(i) = cp.clDt.iPzBuf(i) + dInputPos					'投入位置を足す
	End If
Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub
				

''' <summary>
''' SLOPE法の計算(微分)
''' </summary>
''' <param name="cp">データファイルクラス</param>
''' <returns></returns>
Public Function MakeHoseiToSlope(ByRef cp As stCalcPara) As Int32
	Dim i As Int32
	Dim iSampleNum			As int32 = cp.clDt.iHosei.Length			'配列サイズ
	Dim iArraySize			As Int32 = iSampleNum - 1					'配列サイズ
	Dim iSmpEnd				As int32
	Dim	Temp_K(iArraySize)	As Double												'XY合成微分配列
	Dim iRangeSec			As Double = cp.dDiffInterval							'微分区間(秒)
	Dim iStartidx			As int32 = 10 / (MEAS_TIME_RESOLUTION / 1000.0)			'XY動作待ち分の処理をスキップする
	Dim iRangeNum			As Int32 = Math.Ceiling(iRangeSec / (MEAS_TIME_RESOLUTION / 1000.0))	'微分区間のサンプル数 
	Dim iRoundNum			As Int32 = Math.Ceiling(iRangeNum / 2)
	Dim dWait				As Double = cp.clDt.p_head.dXYActWait + cp.clDt.p_head.dHPLiftWait
	Dim dQuadPeriod			As Double = (cp.dPeriod * 1000 / 2 / 2 / 2)				'1/4周期のサンプル数
Try
	iSmpEnd = iSampleNum - 1						'処理最終インデックス
	If dWait <> 0
		iStartidx = (dWait+iRangeSec) / (MEAS_TIME_RESOLUTION / 1000.0)				'要検討
	End If 
	Array.Clear(Temp_K, 0 ,Temp_K.Length)


	'処理済 補正微分データを追加 ---------------------------------------------------------------------------------------
	For i = iRangeNum To iSmpEnd
		'If i >= iStartidx							'処理最終インデックス
		If i - iRoundNum >= (MEAS_BLOCK_DATA_NUM * cp.dXYWait) And i <= iSmpEnd - dQuadPeriod AndAlso cp.dDiffInterval <>0   		'iStartidx							'処理最終インデックス
			Dim dDeltaY As Double = cp.cldt.iHosei(i) - cp.cldt.iHosei(i - iRangeNum)
			Dim dDeltaX As Double = (i - (i - iRangeNum)) * (MEAS_TIME_RESOLUTION / 1000.0) 
			Temp_K(i - iRoundNum) = (dDeltaY / dDeltaX) /  dDeltaX
		Else
			Temp_K(i - iRoundNum) = 0
		End if
	Next i

	Array.Clear(cp.clDt.iSlope, 0 ,cp.clDt.iSlope.Length)
	Array.Copy(Temp_K, 0, cp.clDt.iSlope, 0, cp.clDt.iSlope.Length)
#If 0
	'補正微分データにIIRフィルタ処理
	Dim fil				As New Filter								'フィルタクラス生成
	fltSet.dIIRCutOffFreq = 1 / dPeriod	/ 16						'カットオフ周波数=測定周波数/16でフィルタ処理
	fltSet.iIIRDeg = 3
	fil.IirFilterButterworth(clDt.iSlope, fltSet, clDt.iSlope)		'IIRバタワース
#End If 
#If 0
	If fltSet.iEnable = 1
		'補正微分データにFIRフィルタ処理
		Dim fil				As New Filter								'フィルタクラス生成
		Dim	Coef()			As Double = Nothing							'FIRフィルタパラメータ(窓関数から生成)
		fltSet.dFIRCutOffFreq = 1 / dPeriod	/ 16						'カットオフ周波数=測定周波数/16でフィルタ処理
		fltSet.dFIRSmplFreq = 500
		fltSet.iFIRFilLen= 4001
		'fltSet.iFIRFilLen= 32767
		fltSet.sFIRWinKind = "Hamming"
		fltSet.sKind = "FIR"
		fil.WindowFunction(fltSet,	Coef)								'FIRフィルタパラメータ作成
		fil.FirFilterD(clDt.iSlope, coef, clDt.iSlope)					'FIR
		'遅延分をずらす
		Dim iFillen As Int32 = fltSet.iFIRFilLen\2
		Array.Copy(clDt.iSlope, iFillen, clDt.iSlope, 0, clDt.iSlope.Length - iFillen )
		'遅延分をNAN
		For i=clDt.iSlope.Length - iFillen To clDt.iSlope.getupperbound(0)
			clDt.iSlope(i) = Double.NaN
		Next
	End If
#End If 
	Return 0
Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Return -1
End Try
End Function


''' <summary>
''' 汎用バッチ式移動平均 最初の区間は前回値で埋める
''' </summary>
''' <param name="dSource">元サンプル配列</param>
''' <param name="dDestArry">移動平均化された配列</param>
''' <param name="iPriod">移動平均区間</param>
Public Sub	BatchMovingAverage(ByRef dSource() As Double, ByRef dDestArry() As Double, ByVal iPriod As Int32 )
Dim i As int32
Dim j As int32
Dim AveArray() As Double = Nothing
Try
	Array.Resize(AveArray, iPriod + 1)										'平均算出用バッファ確保
	j = iPriod + 1
	'最後の区間終了が最大インデックスになるようサンプル配列インデックスでループ 
	For i = 0 To dSource.GetUpperBound(0) - (iPriod + 1)
		Array.Copy( dSource, i, AveArray, 0, AveArray.Length)					'区間サンプルを平均計算用バッファへコピー
		dDestArry(j) = AveArray.Average()										'区間平均値計算して移動平均後のデータ配列へセット
		j +=1																	'移動平均後のデータ配列インデックス更新
	Next i
	For i = 0 To iPriod															'先頭半周期を前回値で埋める
		dDestArry(i) = dDestArry(iPriod + 1)
	Next i
Catch ex As Exception
End Try
End Sub
''' <summary>
''' 上限下限を超えるデータを指定値で固定する
''' </summary>
''' <param name="dData">データ</param>
''' <param name="dLower">下限値</param>
''' <param name="dUpper">上限値</param>
''' <param name="dLimLo">下限固定値</param>
''' <param name="dLimUp">上限固定値</param>
Private Sub	TrigoRangeLimit(ByRef dData As Double, ByVal dLower As Double, ByVal dUpper As Double, ByVal dLimLo As Double, ByVal dLimUp As Double)
	if dData		< dLower
		dData = dLimLo
	End If
	if dData		> dUpper
		dData = dLimUp
	End If
End Sub

''' <summary>
''' 粘度関係計算処理 (フローカーブ測定で使用)
''' </summary>
''' <param name="vp">粘度計算パラメータ構造体</param>
''' <returns></returns>
Public Function ViscosityCalc(ByRef vp As DataFile.ViscosityCalcParameter ) As Int32
	Dim dStrain As Double
	Dim dSpeed	As Double
Try
	If vp.dAmplitude = 0 Or vp.dPeriod = 0 Or vp.dVelocityRate = 0 Or vp.dMeasGap = 0
		dStrain				= 0
		dSpeed				= 0
		vp.dShearVelocity	= 0
		vp.dShearStress		= 0
		vp.dViscosity		= 0
	Else
		dStrain				= ( (vp.dAmplitude / 2) / vp.dMeasGap ) * 100		'ひずみ量
		dSpeed				= ( vp.dAmplitude * Math.PI ) / vp.dPeriod			'速度
		vp.dShearVelocity	= (dSpeed / vp.dMeasGap) * vp.dVelocityRate			'せん断速度
		vp.dShearStress		= vp.dMeasStress / vp.dProbeSquare					'せん断応力
		vp.dViscosity		= vp.dShearStress / vp.dShearVelocity				'粘度


'Debug.Print(vp.dAmplitude &  "  Vp.dShearVelocity=" & Vp.dShearVelocity)

	End If
Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Return -1
End Try
	Return 0
End Function

''' <summary>
''' 粘度関係計算処理 (点モード用)
''' </summary>
''' <param name="vp">粘度計算パラメータ構造体</param>
''' <returns></returns>
Public Function ViscosityCalcZ(ByRef vp As DataFile.ViscosityCalcParameter ) As Int32
	Dim dSpeed	As Double
Try
	dSpeed				= vp.dAmplitude * 2 * Math.PI / vp.dPeriod			'速度(m/s)
	vp.dViscosity		= (2 * vp.dMeasStress * vp.dMeasGap^3) / (3 * Math.PI * vp.dProbeRadius^4 * dSpeed )
	vp.dViscosity		= If(Double.IsInfinity(vp.dViscosity), 0, vp.dViscosity)
Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Return -1
End Try
	Return 0
End Function

''' <summary>
''' 計算後フィルタ処理
''' </summary>
''' <param name="cp"></param>
Private Sub	PostFilterProc(ByRef cp As stCalcPara)     'ByRef cldt As DataFile, ByRef fltSet As stFilterInfo)
Try
	Dim fil				As New Filter									'フィルタクラス生成
	Dim	Coef()			As Double = Nothing								'FIRフィルタパラメータ(窓関数から生成)
	Dim i				As Int64

	Dim iHosei()		As Double	= Nothing
	Dim iSlope()		As Double	= Nothing
	Dim dHosei100()		As Double	= Nothing
	Dim dVisco()		As Double	= Nothing
	Dim dXPosPhase()	As Double	= Nothing
	Dim dXStrgMod()		As Double	= Nothing
	Dim dXElasMod()		As Double	= Nothing
	Dim dXLossTan()		As Double	= Nothing
	Dim dSplStrain()	As Double	= Nothing
	Dim dShearMod()		As Double	= Nothing
	Dim dShearVelo()	As Double	= Nothing

	Array.Resize(iHosei,		cp.cldt.iHosei.Length)						'フィルタ用補正データ領域確保
	Array.Resize(iSlope,		cp.cldt.iSlope.Length)						'フィルタ用補正データ領域確保
	Array.Resize(dHosei100,		cp.cldt.dHosei100.Length)					'フィルタ用補正データ領域確保
	Array.Resize(dVisco,		cp.cldt.dViscosity.Length)					'フィルタ用補正データ領域確保
	Array.Resize(dXPosPhase,	cp.cldt.dXPosPhase.Length)					'フィルタ用補正データ領域確保
	Array.Resize(dXStrgMod,		cp.cldt.dXStrgMod.Length)					'フィルタ用補正データ領域確保
	Array.Resize(dXElasMod,		cp.cldt.dXElasMod.Length)					'フィルタ用補正データ領域確保
	Array.Resize(dXLossTan,		cp.cldt.dXLossTan.Length)					'フィルタ用補正データ領域確保
	Array.Resize(dSplStrain,	cp.cldt.dSplStrain.Length)					'フィルタ用補正データ領域確保
	Array.Resize(dShearMod,		cp.cldt.dShearMod.Length)					'フィルタ用補正データ領域確保
	Array.Resize(dShearVelo,	cp.cldt.dShearVelo.Length)					'フィルタ用補正データ領域確保
	Select Case cp.fltSet.sKind
	Case "FIR"
		fil.WindowFunction(cp.fltSet,	Coef)			'FIRフィルタパラメータ作成
		fil.FirFilterD(cp.cldt.iHosei,		Coef, iHosei)					'FIRフィルタ
		fil.FirFilterD(cp.cldt.iSlope,		Coef, iSlope)					'FIRフィルタ
		fil.FirFilterD(cp.cldt.dHosei100,	Coef, dHosei100)				'FIRフィルタ
		fil.FirFilterD(cp.cldt.dViscosity, Coef, dVisco)					'FIRフィルタ
		fil.FirFilterD(cp.cldt.dXPosPhase, Coef, dXPosPhase)				'FIRフィルタ
		fil.FirFilterD(cp.cldt.dXStrgMod,	Coef, dXStrgMod)				'FIRフィルタ
		fil.FirFilterD(cp.cldt.dXElasMod,	Coef, dXElasMod)				'FIRフィルタ
		fil.FirFilterD(cp.cldt.dXLossTan,	Coef, dXLossTan)				'FIRフィルタ
		fil.FirFilterD(cp.cldt.dSplStrain, Coef, dSplStrain)				'FIRフィルタ
		fil.FirFilterD(cp.cldt.dShearMod,	Coef, dShearMod)				'FIRフィルタ
		fil.FirFilterD(cp.cldt.dShearVelo,	Coef, dShearVelo)				'FIRフィルタ
#If 1
		'遅延分をずらす
		Dim filLen As Int32 = cp.fltSet.iFIRFilLen/2
		Array.Copy(iHosei,		filLen, iHosei,		0, iHosei.Length-filLen)
		Array.Copy(iSlope,		filLen, iSlope,		0, iSlope.Length-filLen)
		Array.Copy(dHosei100,	filLen, dHosei100,	0, dHosei100.Length - filLen )
		Array.Copy(dVisco,		filLen, dVisco,		0, dVisco.Length - filLen )
		Array.Copy(dXPosPhase,	filLen, dXPosPhase,	0, dXPosPhase.Length - filLen )
		Array.Copy(dXStrgMod,	filLen, dXStrgMod,	0, dXStrgMod.Length - filLen )
		Array.Copy(dXElasMod,	filLen, dXElasMod,	0, dXElasMod.Length - filLen )
		Array.Copy(dXLossTan,	filLen, dXLossTan,	0, dXLossTan.Length - filLen )
		Array.Copy(dSplStrain,	filLen, dSplStrain,	0, dSplStrain.Length - filLen )
		Array.Copy(dShearMod,	filLen, dShearMod,	0, dHosei100.Length - filLen )
		Array.Copy(dShearVelo,	filLen, dShearVelo,	0, dHosei100.Length - filLen )
		'遅延分をNAN
		For i=cp.clDt.iHosei.Length - filLen To cp.clDt.iHosei.getupperbound(0)
			iHosei(i)				= 0
			iSlope(i)				= 0
			dHosei100(i)			= 0
			dVisco(i)				= 0
			dXStrgMod(i)			= 0
			dXElasMod(i)			= 0
			dXLossTan(i)			= 0
			dSplStrain(i)			= 0
			dShearMod(i)			= 0
			dShearVelo(i)			= 0
		Next
#end if
	Case "IIR"
		fil.IirFilterButterworthD(cp.cldt.iHosei,		cp.fltSet, iHosei)		'IIRバタワース
		fil.IirFilterButterworthD(cp.cldt.iSlope,		cp.fltSet, iSlope)		'IIRバタワース
		fil.IirFilterButterworthD(cp.cldt.dHosei100,	cp.fltSet, dHosei100)	'IIRバタワース
		For i=0 To cp.cldt.iHosei.GetUpperBound(0)
			If (cp.cldt.dViscosity(i) = 0)
				cp.cldt.dXPosPhase(i)	= 0
				cp.cldt.dXStrgMod(i)	= 0
				cp.cldt.dXElasMod(i)	= 0
				cp.cldt.dXLossTan(i)	= 0
				cp.cldt.dSplStrain(i)	= 0
				cp.cldt.dShearMod(i)	= 0
				cp.cldt.dShearVelo(i)	= 0
				cp.cldt.dXStrgMod(i)	= 0
			End If
		Next i
		fil.IirFilterButterworthD(cp.cldt.dViscosity,	cp.fltSet, dVisco)		'IIRバタワース
		fil.IirFilterButterworthD(cp.cldt.dXPosPhase,	cp.fltSet, dXPosPhase)	'IIRバタワース
		fil.IirFilterButterworthD(cp.cldt.dXStrgMod,	cp.fltSet, dXStrgMod)	'IIRバタワース
		fil.IirFilterButterworthD(cp.cldt.dXElasMod,	cp.fltSet, dXElasMod)	'IIRバタワース
		fil.IirFilterButterworthD(cp.cldt.dXLossTan,	cp.fltSet, dXLossTan)	'IIRバタワース
		fil.IirFilterButterworthD(cp.cldt.dSplStrain,	cp.fltSet, dSplStrain)	'IIRバタワース
		fil.IirFilterButterworthD(cp.cldt.dShearMod,	cp.fltSet, dShearMod)	'IIRバタワース
		fil.IirFilterButterworthD(cp.cldt.dShearVelo,	cp.fltSet, dShearVelo)	'IIRバタワース
	End Select

	Array.Copy(iHosei,		cp.cldt.iHosei,		iHosei.Length)			'最終補正データをフィルタ済補正データで上書き
	Array.Copy(iSlope,		cp.cldt.iSlope,		iSlope.Length)			'最終補正データをフィルタ済補正データで上書き
	Array.Copy(dHosei100,	cp.cldt.dHosei100,	dHosei100.Length)		'最終補正データをフィルタ済補正データで上書き
	Array.Copy(dVisco,		cp.cldt.dViscosity,	dVisco.Length)			'最終補正データをフィルタ済補正データで上書き
	Array.Copy(dXPosPhase,	cp.cldt.dXPosPhase,	dXPosPhase.Length)		'最終補正データをフィルタ済補正データで上書き
	Array.Copy(dXStrgMod,	cp.cldt.dXStrgMod,	dXStrgMod.Length)		'最終補正データをフィルタ済補正データで上書き
	Array.Copy(dXElasMod,	cp.cldt.dXElasMod,	dXElasMod.Length)		'最終補正データをフィルタ済補正データで上書き
	Array.Copy(dXLossTan,	cp.cldt.dXLossTan,	dXLossTan.Length)		'最終補正データをフィルタ済補正データで上書き
	Array.Copy(dSplStrain,	cp.cldt.dSplStrain,	dSplStrain.Length)		'最終補正データをフィルタ済補正データで上書き
	Array.Copy(dShearMod,	cp.cldt.dShearMod,	dShearMod.Length)		'最終補正データをフィルタ済補正データで上書き
	Array.Copy(dShearVelo,	cp.cldt.dShearVelo,	dShearVelo.Length)		'最終補正データをフィルタ済補正データで上書き

Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub
''' <summary>
''' 微分関数
''' </summary>
''' <param name="X1">データn-1</param>
''' <param name="X2">データn</param>
''' <param name="T1">時刻n-1</param>
''' <param name="T2">時刻n</param>
''' <param name="dFreq">移動周波数Hz</param>
''' <returns></returns>
Private Function DifferentialProc(byval X1 As Double, byval X2 As Double, byval T1 As Double, byval T2 As Double, byVal dFreq As Double ) As Double
Dim dRet As Double
Try
	dRet = ((X2 - X1) / (T2 - T1)) / (2 * Math.pi * dfreq )
Catch ex As Exception
End Try
	Return dRet
End Function
''' <summary>
''' 汎用バッチ式移動平均 最初の区間は前回値で埋める(2並列版 サンプル数は同一であること)
''' </summary>
''' <param name="dSource1"></param>
''' <param name="dDestArry1"></param>
''' <param name="dSource2"></param>
''' <param name="dDestArry2"></param>
''' <param name="iPriod"></param>
Public Sub	BatchMovingAverage2(ByRef dSource1() As Double, ByRef dDestArry1() As Double, ByRef dSource2() As Double, ByRef dDestArry2() As Double, ByVal iPriod As Int32 )
Dim i As int32
Dim j As int32
Dim AveArray1() As Double = Nothing
Dim AveArray2() As Double = Nothing
Try
	Array.Resize(AveArray1, iPriod + 1)										'平均算出用バッファ確保
	Array.Resize(AveArray2, iPriod + 1)										'平均算出用バッファ確保
	j = iPriod + 1
	'最後の区間終了が最大インデックスになるようサンプル配列インデックスでループ 
	For i = 0 To dSource1.GetUpperBound(0) - (iPriod + 1)
		Array.Copy( dSource1, i, AveArray1, 0, AveArray1.Length)				'区間サンプルを平均計算用バッファへコピー
		dDestArry1(j) = AveArray1.Average()										'区間平均値計算して移動平均後のデータ配列へセット
		Array.Copy( dSource2, i, AveArray2, 0, AveArray2.Length)				'区間サンプルを平均計算用バッファへコピー
		dDestArry2(j) = AveArray2.Average()										'区間平均値計算して移動平均後のデータ配列へセット
		j +=1																	'移動平均後のデータ配列インデックス更新
	Next i
	For i = 0 To iPriod															'先頭半周期を前回値で埋める
		dDestArry1(i) = dDestArry1(iPriod + 1)
		dDestArry2(i) = dDestArry2(iPriod + 1)
	Next i
Catch ex As Exception
End Try
End Sub
''' <summary>
''' 汎用バッチ式移動平均 最初の区間は前回値で埋める Int64版
''' </summary>
''' <param name="iSource">元サンプル配列</param>
''' <param name="iDestArry">移動平均化された配列</param>
''' <param name="iPriod">移動平均区間</param>
Public Sub	BatchMovingAverageInt(ByRef iSource() As Int64, ByRef iDestArry() As Int64, ByVal iPriod As Int32 )
Dim i As int32
Dim j As int32
Dim AveArray() As Int64 = Nothing
Try
	Array.Resize(AveArray, iPriod )										'平均算出用バッファ確保
	j = iPriod + 1
	'最後の区間終了が最大インデックスになるようサンプル配列インデックスでループ 
	For i = 0 To iSource.GetUpperBound(0) - (iPriod )
		Array.Copy( iSource, i, AveArray, 0, AveArray.Length)					'区間サンプルを平均計算用バッファへコピー
		iDestArry(j) = AveArray.Average()										'区間平均値計算して移動平均後のデータ配列へセット
		j +=1																	'移動平均後のデータ配列インデックス更新
	Next i
	For i = 0 To iPriod															'先頭半周期を前回値で埋める
		iDestArry(i) = iDestArry(iPriod)
	Next i
Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub
''' <summary>
''' 汎用バッチ式移動平均 最初の区間は前回値で埋める(3並列版 サンプル数は同一であること)
''' </summary>
''' <param name="dSource1"></param>
''' <param name="dDestArry1"></param>
''' <param name="dSource2"></param>
''' <param name="dDestArry2"></param>
''' <param name="iPriod"></param>
Public Sub	BatchMovingAverage3(ByRef dSource1() As Double, ByRef dDestArry1() As Double, ByRef dSource2() As Double, ByRef dDestArry2() As Double, ByRef dSource3() As Double, ByRef dDestArry3() As Double, ByVal iPriod As Int32 )
Dim i As int32
Dim j As int32
Dim AveArray1() As Double = Nothing
Dim AveArray2() As Double = Nothing
Dim AveArray3() As Double = Nothing
Try
	Array.Resize(AveArray1, iPriod + 1)										'平均算出用バッファ確保
	Array.Resize(AveArray2, iPriod + 1)										'平均算出用バッファ確保
	Array.Resize(AveArray3, iPriod + 1)										'平均算出用バッファ確保
	j = iPriod + 1
	'最後の区間終了が最大インデックスになるようサンプル配列インデックスでループ 
	For i = 0 To dSource1.GetUpperBound(0) - (iPriod + 1)
		Array.Copy( dSource1, i, AveArray1, 0, AveArray1.Length)				'区間サンプルを平均計算用バッファへコピー
		dDestArry1(j) = AveArray1.Average()										'区間平均値計算して移動平均後のデータ配列へセット
		Array.Copy( dSource2, i, AveArray2, 0, AveArray2.Length)				'区間サンプルを平均計算用バッファへコピー
		dDestArry2(j) = AveArray2.Average()										'区間平均値計算して移動平均後のデータ配列へセット
		Array.Copy( dSource3, i, AveArray3, 0, AveArray3.Length)				'区間サンプルを平均計算用バッファへコピー
		dDestArry3(j) = AveArray3.Average()										'区間平均値計算して移動平均後のデータ配列へセット
		j +=1																	'移動平均後のデータ配列インデックス更新
	Next i
	For i = 0 To iPriod															'先頭半周期を前回値で埋める
		dDestArry1(i) = dDestArry1(iPriod + 1)
		dDestArry2(i) = dDestArry2(iPriod + 1)
		dDestArry3(i) = dDestArry3(iPriod + 1)
	Next i
Catch ex As Exception
End Try
End Sub
''' <summary>
''' 最小二乗法による多項近似
''' </summary>
''' <param name="dX">X軸配列</param>
''' <param name="dY">Y軸配列</param>
''' <param name="dNewX">変換後X軸配列</param>
''' <param name="dNewY">変換後Y軸配列</param>
''' <param name="iNum">データ数</param>
''' <param name="iM">近似次数</param>
''' <returns>数式パラメータ配列  一次式例) y = ax + b -> b:0 a:1</returns>
Public Function fnLeastSquaresMethod(ByVal dX() As Double, ByVal dY() As Double, _
                                        ByRef dNewX() As Double, ByRef dNewY() As Double, _
										ByVal iNum As Integer, ByVal iM As Integer) As Double()
Try
Dim dA(iM, iM + 1)	As Double
Dim dS(2 * iM)		As Double
Dim dT(iM)			As Double

    '配列の初期化
    For i As Integer = 0 To 2 * iM
        dS(i) = 0
    Next i
    For i As Integer = 0 To iM
        dT(i) = 0
    Next i

    'X軸平均値を取得
    Dim XAve As Double = dX.Average()

    '
    For i As Integer = 0 To iNum - 1
        For j As Integer = 0 To 2 * iM
            dS(j) = dS(j) + dX(i) ^ j
        Next j
        For j As Integer = 0 To iM
            dT(j) = dT(j) + dX(i) ^ j * dY(i)
        Next j
    Next i

    For i As Integer = 0 To iM
        For j As Integer = 0 To iM
            dA(i, j) = dS(i + j)
        Next j
        dA(i, iM + 1) = dT(i)
    Next i

    For k As Integer = 0 To iM
        Dim p As Double = dA(k, k)
        For j As Integer = k To iM + 1
            dA(k, j) = dA(k, j) / p
        Next j
        For i As Integer = 0 To iM
            If i <> k Then
                Dim d As Double = dA(i, k)
                For j As Integer = k To iM + 1
                    dA(i, j) = dA(i, j) - d * dA(k, j)
                Next j
            End If
        Next i
    Next k

    '数式を代入
    Dim dRet As Double() = New Double(iM) {}
    Dim str As String
    str = CStr(dA(0, iM + 1))
    dRet(0) = dA(0, iM + 1)
    For i As Integer = 1 To iM
        str += " + " + CStr(dA(i, iM + 1)) + "x^" + CStr(i)
        dRet(i) = dA(i, iM + 1)
    Next i
    'Console.WriteLine(str)

    '引数の配列に代入
    If dNewX IsNot Nothing AndAlso dNewY IsNot Nothing Then
        Dim iLen As Integer = dNewX.Length - 1
        For iCnt As Integer = 0 To iLen
            dNewX(iCnt) = dX(iCnt) 'CDbl(iCnt) * 0.1R
            dNewY(iCnt) = dA(0, iM + 1)

            For i As Integer = 1 To iM
                dNewY(iCnt) += dA(i, iM + 1) * Math.Pow(dNewX(iCnt), CDbl(i))
            Next i
        Next iCnt
    End If
    Return dRet
Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
    Return Nothing
End Try
End Function
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
	dADVal = dWeight / (dWeightCoef * DataFile.cGraviAcc/1000)
	Return dADVal
Catch ex As Exception
	Return Double.NaN
End Try
End Function
'TempXp_BAve(振幅 um 0-p)からキャリブレーションテーブルをサーチしてフル応力値とフル位相値を得る
Public Function CalibSearchPointStressConstMode(ByVal cp As stCalcPara, ByVal dTempXp As Double,  ByRef dCalibFullStress As Double, ByRef dCalibFullPhase As Double) As Int32
Dim i		As int32
Dim x(1)	As Double
Dim y(1)	As Double
Dim nx(1)	As Double
Dim ny(1)	As Double
Dim dRet(1)	As Double
Dim a		As Double
Dim b		As Double
Try
	For i = cp.dCalibAmp.GetUpperBound(0) To 0 Step  -1
		If i-1 >= 0
			If dTempXp <= cp.dCalibAmp(i) And dTempXp >= cp.dCalibAmp(i-1)
			'2点間の直線近似
				'フル応力を計算
				x(0)=cp.dCalibAmp(i-1)
				y(0)=cp.dCalibFullStress(i-1)
				x(1)=cp.dCalibAmp(i)
				y(1)=cp.dCalibFullStress(i)
				dRet = fnLeastSquaresMethod(x, y, nx, ny, 2, 1)
				a = dRet(1)		'傾きをセット
				b = dRet(0)		'Y切片をセット
				dCalibFullStress = a * dTempXp + b
				'フル位相を計算
				x(0)=cp.dCalibAmp(i-1)
				y(0)=cp.dCalibFullPhase(i-1)
				x(1)=cp.dCalibAmp(i)
				y(1)=cp.dCalibFullPhase(i)
				dRet = fnLeastSquaresMethod(x, y, nx, ny, 2, 1)
				a = dRet(1)		'傾きをセット
				b = dRet(0)		'Y切片をセット
				dCalibFullPhase = a * dTempXp + b
				Return 0

			ElseIf (dTempXp > cp.dCalibAmp(i)) And (i = cp.dCalibAmp.GetUpperBound(0))
			'定義最大振幅を超えている
				Dim idx As Int32 = cp.dCalibAmp.GetUpperBound(0)
				dCalibFullStress	= cp.dCalibFullStress(idx)
				dCalibFullPhase		= cp.dCalibFullPhase(idx)
				Return 1
			End If
		Else
		'定義最小振幅を下回っている dTempXpが負数
			dCalibFullStress	= cp.dCalibFullStress(0)
			dCalibFullPhase		= cp.dCalibFullPhase(0)
			Return 1
		End If
	Next
	Return 0
Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Return -1
End Try
End Function

Public Function Debug_ResultFileOut(ByVal iSw As Int16, ByRef cp As stCalcPara) As Int32
Try
	'デバッグ用ファイル出力
	Dim fs As New FileStream("C:\Test\debugCorrect_" & If(iSw=0,"Real",If(iSw=1,"Batch","Batch2"))& Now().ToString("MMddhhmmss") & ".csv", FileMode.Create, FileAccess.Write, FileShare.ReadWrite)
	Dim swr As Textwriter = New StreamWriter(fs, Encoding.UTF8)
	Dim sLineBuf As String = nothing
	'テキストファイル(CSV)書込み
	'ヘッダ行
	sLineBuf = "Time,iOxBuf,iOyBuf,iOzBuf,iPxBuf,iPyBuf,iPzBuf,,"
	sLineBuf = sLineBuf & "TempX_1,TempX_A,TempX_Q,TempXp_1,TempXp_A,TempXp_Q,TempXp_B,TempXp_BAve,,"
	sLineBuf = sLineBuf & "Temp_G,Temp_J,Temp_JAve,Temp_J100,Temp_L,Temp_M,Temp_N,Temp_T1,Temp_T3Ave,TempXp_T2,TempXp_T3Ave,TempXp_Rad,TempXY_T2,TempXY_Rad,,"
	sLineBuf = sLineBuf & "Slope,dViscosity,iHosei,dXPosPhase,dYPosPhase,dSplStrain,dShearMod,dShearVelo,dXStrgMod,dXElasMod,dXLossTan,dYStrgMod,dYElasMod,dYLossTan,,"
	sLineBuf = sLineBuf & "dRTemp_J100"
	'sLineBuf = sLineBuf & '"XYVectorX,XYVectorY"				'"TempXY_Dir,TempXYp_Dir,TempX_Dir,TempY_Dir,TempX_VStress(i),TempY_VStress(i),XYVectorX,XYVectorY"

	swr.WriteLine(sLineBuf)
	Dim dTime As Double = 0.0

	For i = 0 To cp.clDt.iHosei.GetUpperBound(0)
		sLineBuf = dTime.ToString("F3") & "," & _
		cp.clDt.iOxBuf(i).ToString & "," & _
		cp.clDt.iOyBuf(i).ToString & "," & _
		cp.clDt.iOzBuf(i).ToString & "," & _
		p_dtFile.iPxBuf(i).ToString & "," & _
		p_dtFile.iPyBuf(i).ToString & "," & _
		p_dtFile.iPzBuf(i).ToString & ",,"

		sLineBuf = sLineBuf & _
		Temp_JAve(i).ToString & "," & _
		Temp_J100(i).ToString & "," & _
		TempXp_Rad(i).ToString & "," & _
		TempXY_Rad(i).ToString & ",,"

		sLineBuf = sLineBuf & _
		cp.clDt.iSlope(i).ToString & "," & _
		cp.clDt.dViscosity(i).Tostring & "," & _
		cp.clDt.iHosei(i).ToString & "," & _
		cp.clDt.dXPosPhase(i).ToString & "," & _
		cp.clDt.dYPosPhase(i).ToString & "," & _
		cp.clDt.dSplStrain(i).ToString & "," & _
		cp.clDt.dShearMod(i).ToString & "," & _
		cp.clDt.dShearVelo(i).ToString & "," & _
		cp.clDt.dXStrgMod(i).ToString & "," & _
		cp.clDt.dXElasMod(i).ToString & "," & _
		cp.clDt.dXLossTan(i).ToString & "," & _
		cp.clDt.dYStrgMod(i).ToString & "," & _
		cp.clDt.dYElasMod(i).ToString & "," & _
		cp.clDt.dYLossTan(i).ToString & ",,"
 		swr.WriteLine(sLineBuf)
		dTime += 0.002
	Next i
	swr.Close()
	swr = nothing
	Return 0
Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Return -1
End Try
End Function



#Else
'計算条件パラメータ
Public Structure stCalcPara
	Dim iMeas			As Int16					'測定種別 0:ゲルタイム　1:フルキャリブレーション　2:ゼロキャリブレーション　3:フローカーブ
	Dim iMode			As Int16					'0:最終データ 1:測定中 2:キャリブレーション
	Dim iCalcType		As int16					'計算方法 0:THR 1:SLOPE 2:EXTRAPOLATION 3:PHASE 4:MELT
	Dim	iHPPosControl	As Int16					'測定時熱板制御 0:GAP 1:着圧 2:常時
	Dim cts				As CToolSettings			'詳細設定クラス
	Dim clDt			As DataFile					'データファイルクラス
	Dim dPeriod			As Double					'周期(秒)
	Dim dAmplitude		As Double					'振幅(mm)
	Dim iSampleStart	As Int64					'測定データ開始インデックス
	Dim iSampleNum		As Int64					'サンプル数
	Dim fltSet			As Filter.stFilterInfo		'フィルタクラス
	Dim dXYWait			As Double					'XY動作待ち時間(秒)<-測定開始からXYZ動作するまでの時間
	Dim dZeroStress		As Double					'ゼロストレス値
	Dim dAttenuation	As Double					'2秒周期基準のフィルタ減衰率
	Dim dDiameter		As Double					'プローブ直径(mm)
	Dim dMeasGap		As Double					'測定GAP(mm)
	Dim dFullStress		As Double					'フルストレス値
	Dim dPxPhase		As Double					'x位置位相差
	Dim dPyPhase		As Double					'y位置位相差
	Dim dPzPhase		As Double					'z位置位相差
	Dim iZeroPos		As Int32					'zero調確定位置
	Dim dMeasureStress	As Double					'測定時設定圧力(kPa)
	Dim dDiffInterval	As Double					'SLOPE計算時の微分区間(秒)
	Dim iStrsSensMode	As Int16					'応力検知モード 0:線 1:円 2:点
	Dim iAmpMode		As Int16					'Z振動モード 0:振幅一定 1:応力一定
	Dim dCalibAmp()			As Double				'キャリブレーション 振幅テーブル(点モード時のみ)
	Dim dCalibFullStress()	As Double				'キャリブレーション フル応力値(点モードバッチ時のみ)
	Dim dCalibFullPhase()	As Double				'キャリブレーション フル位相値(点モードバッチ時のみ)
	Dim dRCCBaseThickness	As Double				'サンプル：RCCの時の基材厚み 積層数で計算済
	Dim dMesGap				As Double				'測定GAP計算値(mm) リアルタイム モニタ使用
End Structure
'データ定義
Private m_iOXBuf_R_pt	As Int64 = 0								'リアルタイム補正のための入力バッファ読み取りポインタ
Private m_TempX_1_R_pt	As Int64 = 0								'リアルタイム補正のための微分列バッファ読み取りポインタ
Private m_TempX_A_R_pt	As Int64 = 0								'リアルタイム補正のための1次積分列バッファ読み取りポインタ

Dim		TempX_0()		As Double = Nothing							'応力AD値 X 
Dim		TempY_0()		As Double = Nothing							'応力AD値 Y
Dim		TempXp_0()		As Double = Nothing							'位置 X 
Dim		TempYp_0()		As Double = Nothing							'位置 Y
Dim		TempX_1()		As Double = Nothing							'微分列
Dim		TempY_1()		As Double = Nothing							'微分列
Dim		TempXp_1()		As Double = Nothing							'位置微分列
Dim		TempYp_1()		As Double = Nothing							'位置微分列
Dim		TempX_A()		As Double = Nothing							'1次積分列
'Dim		TempX_AR()		As Double = Nothing							'1次積分列(比率補正済)
Dim		TempY_A()		As Double = Nothing							'1次積分列
'Dim		TempY_AR()		As Double = Nothing							'1次積分列(比率補正済)
Public	TempXp_A()		As Double = Nothing							'位置 2次積分列
Public	TempYp_A()		As Double = Nothing							'位置 2次積分列
Public	TempX_Q()		As Double = Nothing							'2次積分列
'Dim		TempX_QR()		As Double = Nothing							'2次積分列(比率補正済)
Public	TempY_Q()		As Double = Nothing							'2次積分列
'Dim		TempY_QR()		As Double = Nothing							'2次積分列(比率補正済)
Public	TempXp_Q()		As Double = Nothing							'位置 2次積分列
Public	TempYp_Q()		As Double = Nothing							'位置 2次積分列

Public	TempX_QM()		As Double = Nothing							'波形モニタ用 (2次積分列)
Public	TempY_QM()		As Double = Nothing							'波形モニタ用 (2次積分列)

'Dim		Temp_BR()		As Double = Nothing							'XY比率列 (未使用 20210309)

Public	Temp_T1()		As Double = Nothing							'ACos列(位相差)
'Public	Temp_BRx()		As Double = Nothing							'振幅比率計算 X
'Public	Temp_BRy()		As Double = Nothing							'振幅比率計算 Y

'進捗状態 %
Public	dProgPer()	As Double


'リアルタイム補正制御領域の初期化
Public sub AveTempClear()
	m_iOXBuf_R_pt	= 0
	m_TempX_1_R_pt	= 0
	m_TempX_A_R_pt	= 0
	TempX_0		= Nothing								'重量変換
	TempY_0		= Nothing								'重量変換
	TempX_1		= Nothing								'微分列
	TempY_1		= Nothing								'微分列
	TempXp_0	= Nothing								'位置
	TempYp_0	= Nothing								'位置
	TempXp_1	= Nothing								'位置微分列
	TempYp_1	= Nothing								'位置微分列

	TempX_A		= Nothing								'1次積分列
	'TempX_AR	= Nothing								'1次積分列
	TempY_A		= Nothing								'1次積分列
	'TempY_AR	= Nothing								'1次積分列
	TempXp_A	= Nothing								'1次積分列
	TempYp_A	= Nothing								'1次積分列

	TempX_Q		= Nothing								'2次積分列
	'TempX_QR	= Nothing								'2次積分列
	TempY_Q		= Nothing								'2次積分列
	'TempY_QR	= Nothing								'2次積分列
	TempXp_Q	= Nothing								'位置 2次積分列
	TempYp_Q	= Nothing								'位置 2次積分列

	'Temp_BR		= Nothing								'XY比率列	(未使用 20210309)
	Temp_T1		= Nothing								'ACos列(位相差)
	'Temp_BRx	= Nothing								'振幅比率計算 X
	'Temp_BRy	= Nothing								'振幅比率計算 Y

	iLatPrePzBuf  = 0
	iLatSign      = 0
	iLatSampleNum = 0

End sub
Dim iLatPrePzBuf	As int64 = 0
Dim iLatSign		As Int16 = 0
Dim iLatSampleNum	As Int64 = 0
Dim dInputPos As Int64 '= cp.iZeroPos - (cp.cts.InputGap * 1000)

Private iSwitchAmpXYPos	As Int32 = cXYAmpSwitch	'XY位置の振幅設定による解像度切替え閾値(um)

'現行補正計算
''' <summary>
''' 各種結果パラメータ計算処理（リアルタイム・バッチ）
''' </summary>
''' <returns>0:正常終了 -1:例外発生 1:サンプル数が満たない</returns>
Public Function CorrectRealProcVector( ByRef cp As stCalcPara ) As Int32
	'周期0の時は合成できないので仮に周期1秒として計算する
	If cp.dPeriod = 0
		cp.dPeriod = 1.0
	End If
	Dim	i								As int32
	Dim	j								As int32
	Dim idx								As int32
	Dim iAllPriod						As int32	= (cp.dPeriod * 1000 / 2 ) - 1			'測定周期サンプル数
	Dim iHalfPriod						As int32	= (cp.dPeriod * 1000 / 2 / 2) - 1		'測定半周期サンプル数
	Dim iQuotPriod						As int32	= (cp.dPeriod * 1000 / 2 / 2 / 2) - 1	'測定1/4周期サンプル数
	Dim dFreq							As Double	= 1.0 / cp.dPeriod						'測定周波数
	Dim dSampleTime						As Double	= 1.0 / MEAS_SAMPLE_FREQ			'測定サンプリング時間(S)
	Dim iArraySize						As Int64	= cp.iSampleNum - 1					'配列サイズ = 処理サンプル数(引数)

	'比率・内積・外積・位相角を計算しないサンプル数
	Dim iIgnoreCount					As Int64	= (MEAS_BLOCK_DATA_NUM * cp.dXYWait) + (iHalfPriod * 3)

	'AD値->mN変換係数
	Dim dDigToNX						As Double	= 1.0
	Dim dDigToNY						As Double	= 1.0

	Dim	TimeAry(iArraySize)				As Double										'時間配列

	Dim SumAryX()						As Double = Nothing								'積分法 積分値マージ用
	Dim SumAryY()						As Double = Nothing								'積分法 積分値マージ用

	Dim SumAryPX()						As Double = Nothing								'積分法 積分値マージ用(位置)
	Dim SumAryPY()						As Double = Nothing								'積分法 積分値マージ用(位置)

	Dim	TempX_B(iArraySize)				As Double										'振幅列
	Dim	TempY_B(iArraySize)				As Double										'振幅列
	'Dim	TempX_BR(iArraySize)			As Double										'振幅列(比率補正済)
	'Dim	TempY_BR(iArraySize)			As Double										'振幅列(比率補正済)


	Dim	TempXp_B(iArraySize)			As Double										'振幅列(位置)
	Dim	TempYp_B(iArraySize)			As Double										'振幅列(位置)

	'XY中間値比率方法のためのエリア ----
	Dim	Temp_BCenter(iArraySize)		As Double										'XY中間値
	'Dim	Temp_BRxAve(iArraySize)			As Double										'振幅比率計算 X（半周期移動平均後）
	'Dim	Temp_BRyAve(iArraySize)			As Double										'振幅比率計算 Y（半周期移動平均後）
	Dim TempX_BAve(iArraySize)			As Double
	Dim TempY_BAve(iArraySize)			As Double
	Dim TempXY_BAve(iArraySize)			As Double
	Dim TempXY_B(iArraySize)			As Double
'----

	Dim	Temp_D(iArraySize)				As Double										'内積列
	Dim	Temp_C(iArraySize)				As Double										'外積列
	Dim	TempXY_D(iArraySize)			As Double										'内積列
	Dim	TempXY_C(iArraySize)			As Double										'外積列

	Dim	TempXp_D(iArraySize)			As Double										'内積列(位置)
	Dim	TempXp_C(iArraySize)			As Double										'外積列(位置)
	Dim	TempYp_D(iArraySize)			As Double										'内積列(位置)
	Dim	TempYp_C(iArraySize)			As Double										'外積列(位置)

	'移動平均
	Dim Temp_DAve(iArraySize)			As Double										'内積列
	Dim Temp_CAve(iArraySize)			As Double										'外積列
	Dim	TempXp_DAve(iArraySize)			As Double										'内積列(位置)
	Dim	TempXp_CAve(iArraySize)			As Double										'外積列(位置)
	Dim	TempYp_DAve(iArraySize)			As Double										'内積列(位置)
	Dim	TempYp_CAve(iArraySize)			As Double										'外積列(位置)
	Dim TempXp_BAve(iArraySize)			As Double										'X位置の振幅
	Dim TempYp_BAve(iArraySize)			As Double										'Y位置の振幅
	Dim TempXYp_BAve(iArraySize)		As Double										'位置の振幅
	Dim TempXYp_B(iArraySize)			As Double										'位置の振幅

	Dim	Temp_Ts(iArraySize)				As Double										'Sin列
	Dim	Temp_Tc(iArraySize)				As Double										'Cos列
	Dim	Temp_T2(iArraySize)				As Double										'ASin列
	Dim	Temp_T3(iArraySize)				As Double										'ACos列
	Dim	Temp_T3Ave(iArraySize)			As Double										'ACos列(半周期移動平均)

	Dim TempXY_DAve(iArraySize)			As Double										'内積列
	Dim TempXY_CAve(iArraySize)			As Double										'外積列
	Dim	TempXY_Ts(iArraySize)			As Double										'Sin列
	Dim	TempXY_Tc(iArraySize)			As Double										'Cos列
	Dim	TempXY_T2(iArraySize)			As Double										'ASin列
	Dim	TempXY_T1(iArraySize)			As Double										'ASin列

	'位置用
	Dim	TempXp_Ts(iArraySize)			As Double										'Sin列
	Dim	TempXp_Tc(iArraySize)			As Double										'Cos列
	Dim	TempXp_T1(iArraySize)			As Double										'ASin列
	Dim	TempXp_T2(iArraySize)			As Double										'ASin列(位相差)(位置)
	Dim	TempXp_T3(iArraySize)			As Double										'ACos列
	Dim	TempXp_T3Ave(iArraySize)		As Double										'ASin列(半周期移動平均)
	Dim	TempYp_Ts(iArraySize)			As Double										'Sin列
	Dim	TempYp_Tc(iArraySize)			As Double										'Cos列
	Dim	TempYp_T1(iArraySize)			As Double										'ASin列
	Dim	TempYp_T2(iArraySize)			As Double										'ASin列(位相差)(位置)
	Dim	TempYp_T3(iArraySize)			As Double										'ACos列
	Dim	TempYp_T3Ave(iArraySize)		As Double										'ASin列(半周期移動平均)
	Dim	TempXYp_T2(iArraySize)			As Double										'ASin列(位相差)(位置)
	Dim	TempXYp_T3Ave(iArraySize)		As Double										'位相差の平均値
	Dim	TempXY_T3Ave(iArraySize)		As Double										'XY応力とXY位置の位相差

	Dim	Temp_J(iArraySize)				As Double										'XY合成配列
	Dim	Temp_G(iArraySize)				As Double										'XY合成配列前段
	Dim	Temp_GAve(iArraySize)			As Double										'XY合成配列前段移動平均
	Dim	Temp_H(iArraySize)				As int64										'XY合成配列

	Dim	Temp_K(iArraySize)				As int64										'XY合成微分配列(SLOPE用)
	Dim	Temp_L(iArraySize)				As Double										'位相差補正 Y方向ベクトルのY成分（Xに直交する成分）配列
	Dim	Temp_M(iArraySize)				As Double										'位相差補正 Y方向ベクトルのX成分（Xに平行成分）配列
	Dim	Temp_N(iArraySize)				As Double										'位相差補正 Y方向ベクトルのX成分で補正したX方向ベクトル

	Dim	Temp_J100(iArraySize)			As Double										'XY合成配列100%換算値
	Dim Temp_Stn(iArraySize)			As Double										'サンプルに与えたひずみ量
	Dim Temp_SM(iArraySize)				As Double										'ずり弾性率

	Dim	Temp_V(iArraySize)				As Double										'粘度(PaS)
	Dim	Temp_SS(iArraySize)				As Double										'せん断応力
	Dim	Temp_SV(iArraySize)				As Double										'せん断速度

	Dim	TempXp_Rad(iArraySize)			As Double										'X動的粘弾性(Rad)
	Dim	TempXp_G1(iArraySize)			As Double										'X貯蔵弾性率(Pa)
	Dim	TempXp_G2(iArraySize)			As Double										'X損失弾性率(Pa)
	Dim	TempXp_Tn(iArraySize)			As Double										'X損失正接(deg)

	Dim	TempYp_Rad(iArraySize)			As Double										'Y動的粘弾性(Rad)
	Dim	TempYp_G1(iArraySize)			As Double										'Y貯蔵弾性率(Pa)
	Dim	TempYp_G2(iArraySize)			As Double										'Y損失弾性率(Pa)
	Dim	TempYp_Tn(iArraySize)			As Double										'Y損失正接(deg)

	Dim	TempXY_Rad(iArraySize)			As Double										'XY動的粘弾性(Rad)
		

	Dim AveAry()						As Double = Nothing								'平均算出用配列

	Dim iSmpEnd							As int64
	Dim fil								As New Filter									'フィルタクラス生成
	Dim	Coef()							As Double = Nothing								'FIRフィルタパラメータ(窓関数から生成)
	Dim dK								As Double = Math.PI * dSampleTime / cp.dPeriod	'PI * サンプリング時間 / 周期
	Dim dSumX							As Double 										'和を計算
	Dim dSumY							As Double 										'和を計算
	Dim dSumPX							As Double 										'和を計算(位置)
	Dim dSumPY							As Double 										'和を計算(位置)
	Dim dNConv							As Double = p_dNConv * 1E+6						'AD値->uN変換係数
	Dim iBranch							As int16
	Dim Sw								As New Stopwatch

	Dim dUpperR As Double = 10
	Dim dLowerR As Double = 0.1
Try
	Sw.ReStart()
	'測定種別による分岐処理
	'測定種別によるXYウェイトタイムの比率、位相角計算無効
	Select Case cp.iMeas
	Case eHoseiMeasMode.GelTime		'ゲルタイム
		Select Case cp.iMode
		Case ehoseiCalcMode.Batch
			'iIgnoreCount = (MEAS_BLOCK_DATA_NUM * (cp.dXYWait+3))
			iIgnoreCount = (MEAS_BLOCK_DATA_NUM * (cp.dXYWait))
			iBranch = 0
		Case ehoseiCalcMode.RealTime
			'iIgnoreCount = (MEAS_BLOCK_DATA_NUM * (cp.dXYWait+3))
			iIgnoreCount = (MEAS_BLOCK_DATA_NUM * (cp.dXYWait))
			iBranch = 1
		End Select
	Case eHoseiMeasMode.FullCalib		'フルキャリブ
		Select Case cp.iMode
		Case ehoseiCalcMode.Batch
			iIgnoreCount = -1
			iBranch = 0
		Case ehoseiCalcMode.RealTime
			iIgnoreCount = -1
			iBranch = 1
		End Select
	Case eHoseiMeasMode.ZeroCalib		'ゼロキャリブ
		Select Case cp.iMode
		Case ehoseiCalcMode.Batch
			iIgnoreCount = -1
			iBranch = 0
		Case ehoseiCalcMode.RealTime
			iIgnoreCount = -1
			iBranch = 1
		End Select
	Case eHoseiMeasMode.FlowCurve		'フローカーブ
		Select Case cp.iMode
		Case ehoseiCalcMode.Batch
			iIgnoreCount = -1
			iBranch = 0
		Case ehoseiCalcMode.RealTime
			iIgnoreCount = -1
			iBranch = 1
		End Select
	Case Else
		iIgnoreCount = -1
		iBranch = 0
	End Select

	iSmpEnd = cp.iSampleNum - 1						'処理最終インデックス
	Array.Resize(dProgPer, 7)						'進捗度配列確保
	dInputPos = cp.iZeroPos - (cp.cts.InputGap * 1000)

	iLatSampleNum += cp.iSampleNum
	For i = iLatSampleNum - cp.iSampleNum To iLatSampleNum - 1
		'ゲルタイム測定・点モード・バッチ処理のみ
		'Z軸位置データを変換する 振動位置に達したらゲイン1/10 
		Select Case cp.iMeas
		Case eHoseiMeasMode.GelTime		'ゲルタイム
			Select Case cp.iMode
			Case ehoseiCalcMode.RealTime
				'iPzBufは投入位置で一旦０になり１０倍精度で着圧決定位置を知らせる
				If cp.clDt.iPzBuf(i) = 0 AndAlso iLatPrePzBuf > 0 AndAlso iLatSign = 0
					iLatSign = 1
					iLatPrePzBuf += dInputPos
				End If
				If iLatSign = 1
				'着圧決定の振動位置を見つけた
					cp.clDt.iPzBuf(i) = (cp.clDt.iPzBuf(i) / 10) + iLatPrePzBuf			'Gain 1/10 にして 落差を足す
				Else
					iLatPrePzBuf = cp.clDt.iPzBuf(i)
					cp.clDt.iPzBuf(i) = cp.clDt.iPzBuf(i) + dInputPos					'投入位置を足す
				End If  
			Case Else
			End Select
		Case Else
		End Select
	Next


	'1周期分のサンプル数が無い場合は何もしない
	If cp.iSampleNum <= iAllPriod 
		Return 1
	End If
	'時刻配列の作成
	For i = 0 To iSmpEnd
		TimeAry(i) = i * MEAS_TIME_RESOLUTION / MEAS_TIME_BLOCK
	Next
	'一括処理の場合は読み取りインデックスをゼロクリア
	If iBranch = 0
		m_iOXBuf_R_pt	= 0
		m_TempX_1_R_pt	= 0
		m_TempX_A_R_pt	= 0
	End If
	

	'微分を行う TempX_1, TempY_1---------------------------------------------------
	If IsNothing(TempX_1) Or iBranch = 0
		Array.Resize(TempX_1, cp.iSampleNum)							'配列拡張
		Array.Resize(TempY_1, cp.iSampleNum)							'配列拡張
		Array.Resize(TempXp_1, cp.iSampleNum)							'配列拡張
		Array.Resize(TempYp_1, cp.iSampleNum)							'配列拡張
	Else
		Array.Resize(TempX_1, TempX_1.Length + cp.iSampleNum)			'配列拡張
		Array.Resize(TempY_1, TempY_1.Length + cp.iSampleNum)			'配列拡張
		Array.Resize(TempXp_1, TempX_1.Length + cp.iSampleNum)			'配列拡張
		Array.Resize(TempYp_1, TempY_1.Length + cp.iSampleNum)			'配列拡張
	End If

	'AD値->uN変換止めて、AD値のまま)
	Array.Resize(TempXp_0, cp.clDt.iPxBuf.Length)							'配列確保
	Array.Resize(TempYp_0, cp.clDt.iPyBuf.Length)							'配列確保
	Array.Copy(cp.clDt.iPxBuf, TempXp_0, cp.clDt.iPxBuf.Length)
	Array.Copy(cp.clDt.iPyBuf, TempYp_0, cp.clDt.iPyBuf.Length)

	Array.Resize(TempX_0, cp.clDt.iOxBuf.Length)							'配列確保
	Array.Resize(TempY_0, cp.clDt.iOyBuf.Length)							'配列確保
	Array.Copy(cp.clDt.iOxBuf, TempX_0, cp.clDt.iOxBuf.Length)
	Array.Copy(cp.clDt.iOyBuf, TempY_0, cp.clDt.iOyBuf.Length)

	'微分
	idx = 1
	For i = m_iOXBuf_R_pt + 1 To m_iOXBuf_R_pt + cp.iSampleNum - 1
		'演算式 ((X2 - X1) / (T2 - T1)) / (2 * Math.pi * dfreq )
		TempX_1(i) = DifferentialProc(TempX_0(i - 1), TempX_0(i), TimeAry(idx - 1), TimeAry(idx), dFreq ) 
		TempY_1(i) = DifferentialProc(TempY_0(i - 1), TempY_0(i), TimeAry(idx - 1), TimeAry(idx), dFreq )
		TempXp_1(i) = DifferentialProc(TempXp_0(i - 1), TempXp_0(i), TimeAry(idx - 1), TimeAry(idx), dFreq )
		TempYp_1(i) = DifferentialProc(TempYp_0(i - 1), TempYp_0(i), TimeAry(idx - 1), TimeAry(idx), dFreq )
		idx += 1
		dProgPer(0) = idx * 100 / (m_iOXBuf_R_pt + cp.iSampleNum - m_iOXBuf_R_pt)
'IF i=17209
'Debug.Print("")
'End If
	Next

	m_iOXBuf_R_pt += cp.iSampleNum										'TempX_1の読み出しポイント更新


	'一回目 積分を行う 区間は半周期で1点ずつ移動して和を計算  TempX_A, TempY_A ---------------------------------------------------
	If IsNothing(TempX_A) Or iBranch = 0
		Array.Resize(TempX_A, cp.iSampleNum)							'配列拡張
		Array.Resize(TempY_A, cp.iSampleNum)							'配列拡張
		'Array.Resize(TempX_AR, cp.iSampleNum)							'配列拡張
		'Array.Resize(TempY_AR, cp.iSampleNum)							'配列拡張
		Array.Resize(TempXp_A, cp.iSampleNum)							'配列拡張
		Array.Resize(TempYp_A, cp.iSampleNum)							'配列拡張
	Else
		Array.Resize(TempX_A, TempX_A.Length + cp.iSampleNum)			'配列拡張
		Array.Resize(TempY_A, TempY_A.Length + cp.iSampleNum)		 	'配列拡張
		'Array.Resize(TempX_AR, TempX_AR.Length + cp.iSampleNum)		'配列拡張
		'Array.Resize(TempY_AR, TempY_AR.Length + cp.iSampleNum)		'配列拡張
		Array.Resize(TempXp_A, TempXp_Q.Length + cp.iSampleNum)			'配列拡張
		Array.Resize(TempYp_A, TempYp_Q.Length + cp.iSampleNum)			'配列拡張
	End if
	'微分配列 半周期分の合計
	SumAryX = Nothing
	SumAryY = Nothing
	SumAryPX = Nothing
	SumAryPY = Nothing
	Array.Resize(SumAryX, iHalfPriod + 1)				'半周期分の加算エリア確保
	Array.Resize(SumAryY, iHalfPriod + 1)				'半周期分の加算エリア確保
	Array.Resize(SumAryPX, iHalfPriod + 1)				'半周期分の加算エリア確保
	Array.Resize(SumAryPY, iHalfPriod + 1)				'半周期分の加算エリア確保

	For idx = m_TempX_1_R_pt To m_TempX_1_R_pt + (cp.iSampleNum - 1)
		If idx - iHalfPriod >= 0
		'半周期先頭読み出しポイント
			Array.Copy(TempX_1, idx - iHalfPriod, SumAryX, 0, SumAryX.Length)			'SUM用配列へデータコピー
			Array.Copy(TempY_1, idx - iHalfPriod, SumAryY, 0, SumAryY.Length)			'SUM用配列へデータコピー
			dSumX = SumAryX.Sum()                             '和を計算
			dSumY = SumAryY.Sum()                             '和を計算

			Array.Copy(TempXp_1, idx - iHalfPriod, SumAryPX, 0, SumAryPX.Length)			'SUM用配列へデータコピー
			Array.Copy(TempYp_1, idx - iHalfPriod, SumAryPY, 0, SumAryPY.Length)			'SUM用配列へデータコピー
			dSumPX = SumAryPX.Sum()                            '和を計算
			dSumPY = SumAryPY.Sum()                            '和を計算



			TempX_A( idx ) = dSumX * dK									'和 * PI * サンプリング時間 / 周期
			TempY_A( idx ) = dSumY * dK									'和 * PI * サンプリング時間 / 周期

			
			TempXp_A( idx ) = dSumPX * dK									'和 * PI * サンプリング時間 / 周期
			TempYp_A( idx ) = dSumPY * dK									'和 * PI * サンプリング時間 / 周期
		Else
			'無効データ 半周期積分元のデータが無いため
			TempX_A( idx ) = Double.NaN
			TempY_A( idx ) = Double.NaN
		End if
		dProgPer(1) = idx * 100 / (m_TempX_1_R_pt + cp.iSampleNum - m_TempX_1_R_pt)
	Next idx
	m_TempX_1_R_pt += cp.iSampleNum										'TempX_1の読み出しポイント更新

	'2回目 積分を行う 区間は半周期で1点ずつ移動して和を計算  TempX_Q, TempY_Q ---------------------------------------------------
	If IsNothing(TempX_Q) Or iBranch = 0
		Array.Resize(TempX_Q, cp.iSampleNum)							'配列拡張
		Array.Resize(TempY_Q, cp.iSampleNum)							'配列拡張
		'Array.Resize(TempX_QR, cp.iSampleNum)							'配列拡張
		'Array.Resize(TempY_QR, cp.iSampleNum)							'配列拡張
		Array.Resize(TempX_QM, cp.iSampleNum)							'配列拡張
		Array.Resize(TempY_QM, cp.iSampleNum)							'配列拡張
		Array.Resize(TempXp_Q, cp.iSampleNum)							'配列拡張
		Array.Resize(TempYp_Q, cp.iSampleNum)							'配列拡張
	Else
		Array.Resize(TempX_Q, TempX_Q.Length + cp.iSampleNum)			'配列拡張
		Array.Resize(TempY_Q, TempY_Q.Length + cp.iSampleNum)			'配列拡張
		'Array.Resize(TempX_QR, TempY_QR.Length + cp.iSampleNum)			'配列拡張
		'Array.Resize(TempY_QR, TempY_QR.Length + cp.iSampleNum)			'配列拡張
		Array.Resize(TempX_QM, TempY_QM.Length + cp.iSampleNum)			'配列拡張
		Array.Resize(TempY_QM, TempY_QM.Length + cp.iSampleNum)			'配列拡張
		Array.Resize(TempXp_Q, TempXp_Q.Length + cp.iSampleNum)			'配列拡張
		Array.Resize(TempYp_Q, TempYp_Q.Length + cp.iSampleNum)			'配列拡張
	End if
	'微分配列 半周期分の合計
	SumAryX = Nothing
	SumAryY = Nothing
	SumAryPX = Nothing
	SumAryPY = Nothing
	Array.Resize(SumAryX,	iHalfPriod + 1)				'半周期分の加算エリア確保
	Array.Resize(SumAryY,	iHalfPriod + 1)				'半周期分の加算エリア確保
	Array.Resize(SumAryPX,	iHalfPriod + 1)				'半周期分の加算エリア確保
	Array.Resize(SumAryPY,	iHalfPriod + 1)				'半周期分の加算エリア確保

	For idx = m_TempX_A_R_pt To m_TempX_A_R_pt + (cp.iSampleNum - 1)
		If idx - iAllPriod >= 0
		'半周期先頭読み出しポイント
			Array.Copy(TempX_A, idx - iHalfPriod, SumAryX, 0, SumAryX.Length - 1)			'SUM用配列へデータコピー
			Array.Copy(TempY_A, idx - iHalfPriod, SumAryY, 0, SumAryY.Length - 1)			'SUM用配列へデータコピー
			dSumX = SumAryX.Sum()                             '和を計算
			dSumY = SumAryY.Sum()                             '和を計算

			Array.Copy(TempXp_A, idx - iHalfPriod, SumAryPX, 0, SumAryPX.Length - 1)		'SUM用配列へデータコピー
			Array.Copy(TempYp_A, idx - iHalfPriod, SumAryPY, 0, SumAryPY.Length - 1)		'SUM用配列へデータコピー
			dSumPX = SumAryPX.Sum()                            '和を計算
			dSumPY = SumAryPY.Sum()                            '和を計算

			TempX_Q( idx ) = dSumX * dK									'和 * PI * サンプリング時間 / 周期
			TempY_Q( idx ) = dSumY * dK									'和 * PI * サンプリング時間 / 周期

			TempXp_Q( idx ) = dSumPX * dK								'和 * PI * サンプリング時間 / 周期
			TempYp_Q( idx ) = dSumPY * dK								'和 * PI * サンプリング時間 / 周期

		Else
			'無効データ 半周期積分元のデータが無いため
			TempX_Q( idx )	= Double.NaN
			TempY_Q( idx )	= Double.NaN
			TempXp_Q( idx ) = Double.NaN
			TempYp_Q( idx ) = Double.NaN
		End if
		dProgPer(2) = idx * 100 / (m_TempX_A_R_pt + cp.iSampleNum - m_TempX_A_R_pt)
	Next idx

	'波形モニタ用
	Array.Copy(TempX_Q, TempX_QM, TempX_Q.Length)
	Array.Copy(TempY_Q, TempY_QM, TempY_Q.Length)

	'粘度関係計算 条件値
	Dim uVp As Datafile.ViscosityCalcParameter
	uVp.dPeriod			= cp.dPeriod										'設定周期(S)
	uVp.dAmplitude		= cp.dAmplitude / 10^3								'設定振幅(m)
	uVp.dMeasGap		= cp.dMeasGap / 10^3								'測定GAP(m)
	uVp.dProbeSquare	= Math.PI * ((cp.dDiameter/10^3) / 2)^2				'プローブ面積(m2)
	'-------------------------------------------------------------------------------------------------
	
	
	Select Case iBranch
	Case 0 			'GelTime最終測定、キャリブレーション
		'Array.Resize(Temp_BRx,iArraySize+1)
		'Array.Resize(Temp_BRy,iArraySize+1)

		For i = 0  To iSmpEnd
			'振幅を計算 TempX_B, TempY_B---------------------------------------------------
			TempX_B(i)	= Math.Sqrt( TempX_A(i)^2 + TempX_Q(i)^2 )
			TempY_B(i)	= Math.Sqrt( TempY_A(i)^2 + TempY_Q(i)^2 )
			TempXp_B(i) = Math.Sqrt( TempXp_A(i)^2 + TempXp_Q(i)^2 )
			TempYp_B(i) = Math.Sqrt( TempYp_A(i)^2 + TempYp_Q(i)^2 )
			dProgPer(3) = i * 100 / iSmpEnd
		Next i

		'振幅の半周期移動平均
		BatchMovingAverage(TempX_B,		TempX_BAve,		iAllPriod)
		BatchMovingAverage(TempY_B,		TempY_BAve,		iAllPriod)
		BatchMovingAverage(TempXp_B,	TempXp_BAve,		iAllPriod)
		BatchMovingAverage(TempYp_B,	TempYp_BAve,		iAllPriod)

		'BatchMovingAverage2(TempX_B, TempX_BAve, TempY_B, TempY_BAve, iHalfPriod)
		'Dim dUpperR As Double = 10
		'Dim dLowerR As Double = 0.1
		'比率を考慮して対象を再計算
		For i = 0  To iSmpEnd
			''If i > iIgnoreCount
				'If Double.IsNaN(TempX_B(i)) Or Double.IsNaN(TempY_B(i))
				'	Temp_BRx(i) =  1													'振幅比率計算 X
				'	Temp_BRy(i) =  1													'振幅比率計算 Y
				'Else
				'Temp_BCenterは大きい方をセット
				'	if TempX_BAve(i) > TempY_BAve(i)
				'		Temp_BCenter(i) = TempX_BAve(i)
				'	Else
				'		Temp_BCenter(i) = TempY_BAve(i)
				'	End If

					'Temp_BCenter(i) = (TempX_B(i) + TempY_B(i)) / 2
				'	Temp_BRx(i) = Temp_BCenter(i) / TempX_BAve(i)	'振幅比率計算 X
				'	Temp_BRy(i) = Temp_BCenter(i) / TempY_BAve(i)	'振幅比率計算 Y

				'End If
			'Else
			'XY動作するまでは計算は行わない
			'	Temp_BRx(i) = 1
			'	Temp_BRy(i) = 1
			'End If
			'TrigoRangeLimit(Temp_BRx(i), dLowerR, dUpperR, 1, 1)
			'TrigoRangeLimit(Temp_BRy(i), dLowerR, dUpperR, 1, 1)

			'Temp_BRx(i) = 1
			'Temp_BRy(i) = 1
			'TempX_AR(i) = TempX_A(i) * Temp_BRx(i)								'1次積分X配列を補正
			'TempY_AR(i) = TempY_A(i) * Temp_BRy(i)								'1次積分Y配列を補正
			'TempX_QR(i) = TempX_Q(i) * Temp_BRx(i)								'2次積分X配列を補正
			'TempY_QR(i) = TempY_Q(i) * Temp_BRy(i)								'2次積分Y配列を補正
#If OLD
			TempXY_BAve(i)	= (TempX_BAve(i)	+ TempY_BAve(i))  / 2
			TempXYp_BAve(i) = (TempXp_BAve(i)	+ TempYp_BAve(i)) / 2
#else
			TempXY_B(i)	= (TempX_B(i)	+ TempY_B(i))  / 2
			TempXYp_B(i) = (TempXp_B(i)	+ TempYp_B(i)) / 2
#End If
			
			If i > iIgnoreCount
			'XY動作するまでは計算は行わない
				'TempX_BR(i) = Math.Sqrt( TempX_AR(i)^2 + TempX_QR(i)^2 )
				'TempY_BR(i) = Math.Sqrt( TempY_AR(i)^2 + TempY_QR(i)^2 )
				'TempX_BR(i) = Math.Sqrt( TempX_A(i)^2 + TempX_Q(i)^2 )
				'TempY_BR(i) = Math.Sqrt( TempY_A(i)^2 + TempY_Q(i)^2 )
			Else
				'TempX_BR(i) = Double.NaN
				'TempY_BR(i) = Double.NaN
				TempX_B(i)		= Double.NaN
				TempY_B(i)		= Double.NaN
				TempX_BAve(i)	= Double.NaN
				TempY_BAve(i)	= Double.NaN
			End If
			dProgPer(4) = i * 100 / iSmpEnd
		Next i


		Dim dUpperdeg As Double = Math.PI / 2 + 0.160	'90deg
		Dim dLowerdeg As Double = Math.PI / 2 - 0.160	'80deg
		Array.Resize(Temp_T1,Temp_T2.Length)
		Array.Resize(TempXp_T2,Temp_T2.Length)
		Array.Resize(TempYp_T2,Temp_T2.Length)
		Array.Resize(TempXY_T1,Temp_T2.Length)
		For i = 0  To iSmpEnd
			'内積/外積を計算 Temp_D, Temp_C --------------------------------------------------
			If i > iIgnoreCount
				If TempX_Q(i) = 0 Or TempY_Q(i) = 0
					Temp_D(i) = 0
					Temp_C(i) = 0
					TempXY_D(i) = 0
					TempXY_C(i) = 0
					TempXp_D(i) = 0
					TempXp_C(i) = 0
					TempYp_D(i) = 0
					TempYp_C(i) = 0
				Else
					'Temp_D(i) = TempX_AR(i) * TempY_AR(i) + TempX_QR(i) * TempY_QR(i)			'内積
					'Temp_C(i) = TempX_AR(i) * TempY_QR(i) - TempY_AR(i) * TempX_QR(i)			'外積

					'TempXY_D(i) = TempX_AR(i) * TempXp_A(i) + TempY_AR(i) * TempYp_A(i)			'内積
					'TempXY_C(i) = TempX_AR(i) * TempYp_A(i) - TempXp_A(i) * TempY_AR(i)			'外積

					'TempXp_D(i) = TempXp_A(i) * TempX_AR(i) + TempXp_Q(i) * TempX_QR(i)			'内積(位置)
					'TempXp_C(i) = TempXp_A(i) * TempX_QR(i) - TempX_AR(i) * TempXp_Q(i)			'外積(位置)
					'TempYp_D(i) = TempYp_A(i) * TempY_AR(i) + TempYp_Q(i) * TempY_QR(i)			'内積(位置)
					'TempYp_C(i) = TempYp_A(i) * TempY_QR(i) - TempY_AR(i) * TempYp_Q(i)			'外積(位置)

					Temp_D(i)	= TempX_A(i) * TempY_A(i) + TempX_Q(i) * TempY_Q(i)				'内積
					Temp_C(i)	= TempX_A(i) * TempY_Q(i) - TempY_A(i) * TempX_Q(i)				'外積

					TempXY_D(i) = TempX_A(i) * TempXp_A(i) + TempY_A(i) * TempYp_A(i)			'内積
					TempXY_C(i) = TempX_A(i) * TempYp_A(i) - TempXp_A(i) * TempY_A(i)			'外積

					TempXp_D(i) = TempXp_A(i) * TempX_A(i) + TempXp_Q(i) * TempX_Q(i)			'内積(位置)
					TempXp_C(i) = TempXp_A(i) * TempX_Q(i) - TempX_A(i) * TempXp_Q(i)			'外積(位置)
					TempYp_D(i) = TempYp_A(i) * TempY_A(i) + TempYp_Q(i) * TempY_Q(i)			'内積(位置)
					TempYp_C(i) = TempYp_A(i) * TempY_Q(i) - TempY_A(i) * TempYp_Q(i)			'外積(位置)
				End If
			Else

			'XY動作するまでは計算は行わない
				Temp_D(i)	= Double.NaN
				Temp_C(i)	= Double.NaN
				TempXY_D(i)	= Double.NaN
				TempXY_C(i)	= Double.NaN
				TempXp_D(i) = Double.NaN
				TempXp_C(i) = Double.NaN
				TempYp_D(i) = Double.NaN
				TempYp_C(i) = Double.NaN
			End If
		Next i

		BatchMovingAverage(Temp_D, Temp_DAve, iAllPriod)
		BatchMovingAverage(Temp_C, Temp_CAve, iAllPriod)
		BatchMovingAverage(TempXY_D, TempXY_DAve, iAllPriod)
		BatchMovingAverage(TempXY_C, TempXY_CAve, iAllPriod)
		BatchMovingAverage(TempXp_D, TempXp_DAve, iAllPriod)
		BatchMovingAverage(TempXp_C, TempXp_CAve, iAllPriod)
		BatchMovingAverage(TempYp_D, TempYp_DAve, iAllPriod)
		BatchMovingAverage(TempYp_C, TempYp_CAve, iAllPriod)

		For i = 0  To iSmpEnd

			'X:Y位相差θを算出 --------------------------------------------------
			'If TempX_BR(i) = 0 Or TempY_BR(i) = 0
			If TempX_B(i) = 0 Or TempY_B(i) = 0
				Temp_Tc(i) = Double.NaN												'Cosを演算
				Temp_Ts(i) = Double.NaN												'Sinを演算
			Else
#If OLD
				Temp_Tc(i) = Temp_DAve(i) / (TempX_BAve(i) * TempY_BAve(i))			'Cosを演算
				Temp_Ts(i) = Temp_CAve(i) / (TempX_BAve(i) * TempY_BAve(i))			'Sinを演算
#else
				Temp_Tc(i) = Temp_D(i) / (TempX_B(i) * TempY_B(i))			'Cosを演算
				Temp_Ts(i) = Temp_C(i) / (TempX_B(i) * TempY_B(i))			'Sinを演算
#end if
				TrigoRangeLimit(Temp_Tc(i), -1, 1, -1, 1)
				TrigoRangeLimit(Temp_Ts(i), -1, 1, -1, 1)
				'TrigoRangeLimit(Temp_Tc(i), -1, 1, Double.NaN, Double.NaN)
				'TrigoRangeLimit(Temp_Ts(i), -1, 1, Double.NaN, Double.NaN)
			End If
			Temp_T1(i) = Math.Acos( Temp_Tc(i) )							'θ算出　Cosの範囲0～180度で表現し90度付近が高精度
#If OLD
			Temp_T2(i) = Math.Asin( Temp_Ts(i) )							'θ算出　Sinの範囲-90度～+90度で表現し0度付近が高精度
#else
			If Temp_T1(i) >= Math.PI / 2
				Temp_T2(i) = Temp_T1(i)											'θ算出(位置)
			Else
				Temp_T2(i) = Math.Asin( Temp_Ts(i) )								'θ算出(位置)
			End If
#end if

			'XY応力とXY位置の位相差θを算出 --------------------------------------------------
			'If TempX_BR(i) = 0 Or TempY_BR(i) = 0
			If TempX_B(i) = 0 Or TempY_B(i) = 0
				TempXY_Tc(i) = Double.NaN											'Cosを演算
				TempXY_Ts(i) = Double.NaN											'Sinを演算
			Else
#If OLD
				TempXY_Tc(i) = TempXY_DAve(i) / (TempXY_BAve(i) * TempXYp_BAve(i))	'Cosを演算
				TempXY_Ts(i) = TempXY_CAve(i) / (TempXY_BAve(i) * TempXYp_BAve(i))	'Sinを演算
#else
				TempXY_Tc(i) = TempXY_D(i) / (TempXY_B(i) * TempXYp_B(i))	'Cosを演算
				TempXY_Ts(i) = TempXY_C(i) / (TempXY_B(i) * TempXYp_B(i))	'Sinを演算
#end if
				TrigoRangeLimit(TempXY_Tc(i), -1, 1, -1, 1)
				TrigoRangeLimit(TempXY_Ts(i), -1, 1, -1, 1)
				'TrigoRangeLimit(TempXY_Tc(i), -1, 1, Double.NaN, Double.NaN)
				'TrigoRangeLimit(TempXY_Ts(i), -1, 1, Double.NaN, Double.NaN)
			End If
			TempXY_T1(i) = Math.Acos( TempXY_Tc(i) )							'θ算出　Cosの範囲0～180度で表現し90度付近が高精度
#If OLD
			TempXY_T2(i) = Math.Asin( TempXY_Ts(i) )							'θ算出　Sinの範囲-90度～+90度で表現し0度付近が高精度
#else
			If TempXY_T1(i) >= Math.PI / 2
				TempXY_T2(i) = TempXY_T1(i)											'θ算出(位置)
			Else
				TempXY_T2(i) = Math.Asin( TempXY_Ts(i) )								'θ算出(位置)
			End If
#end If


			'X:Xpos位相差θを算出（位置） --------------------------------------------------
			'If TempX_BR(i) = 0 Or TempXp_B(i) = 0
			If TempX_B(i) = 0 Or TempXp_B(i) = 0
				TempXp_Tc(i) = Double.NaN											'Cosを演算(位置)
				TempXp_Ts(i) = Double.NaN											'Sinを演算(位置)
			Else
#If OLD
				TempXp_Tc(i) = TempXp_DAve(i) / (TempX_BAve(i) * TempXp_BAve(i))	'Cosを演算
				TempXp_Ts(i) = TempXp_CAve(i) / (TempX_BAve(i) * TempXp_BAve(i))	'Sinを演算
#else
				TempXp_Tc(i) = TempXp_D(i) / (TempX_B(i) * TempXp_B(i))	'Cosを演算
				TempXp_Ts(i) = TempXp_C(i) / (TempX_B(i) * TempXp_B(i))	'Sinを演算
#end if
				TrigoRangeLimit(TempXp_Tc(i), -1, 1, -1, 1)
				TrigoRangeLimit(TempXp_Ts(i), -1, 1, -1, 1)
				'TrigoRangeLimit(TempXp_Tc(i), -1, 1, Double.NaN, Double.NaN)
				'TrigoRangeLimit(TempXp_Ts(i), -1, 1, Double.NaN, Double.NaN)

			End If

			TempXp_T1(i) = Math.Acos( TempXp_Tc(i) )								'θ算出(位置)
#If OLD
			TempXp_T2(i) = Math.Asin( TempXp_Ts(i) )								'θ算出(位置)
#else
			If TempXp_T1(i) >= Math.PI / 2
				TempXp_T2(i) = TempXp_T1(i)											'θ算出(位置)
			Else
				TempXp_T2(i) = Math.Asin( TempXp_Ts(i) )								'θ算出(位置)
			End If
#end if
				
			'Y:Ypos位相差θを算出（位置） --------------------------------------------------
			'If TempY_BR(i) = 0 Or TempYp_B(i) = 0
			If TempY_B(i) = 0 Or TempYp_B(i) = 0
				TempYp_Tc(i) = Double.NaN											'Cosを演算(位置)
				TempYp_Ts(i) = Double.NaN											'Sinを演算(位置)
			Else
#If OLD
				TempYp_Tc(i) = TempYp_DAve(i) / (TempY_BAve(i) * TempYp_BAve(i))	'Cosを演算
				TempYp_Ts(i) = TempYp_CAve(i) / (TempY_BAve(i) * TempYp_BAve(i))	'Sinを演算

#else
				TempYp_Tc(i) = TempYp_D(i) / (TempY_B(i) * TempYp_B(i))	'Cosを演算
				TempYp_Ts(i) = TempYp_C(i) / (TempY_B(i) * TempYp_B(i))	'Sinを演算
#end if
				TrigoRangeLimit(TempYp_Tc(i), -1, 1, -1, 1)
				TrigoRangeLimit(TempYp_Ts(i), -1, 1, -1, 1)
				'TrigoRangeLimit(TempYp_Tc(i), -1, 1, Double.NaN, Double.NaN)
				'TrigoRangeLimit(TempYp_Ts(i), -1, 1, Double.NaN, Double.NaN)

			End If

			TempYp_T1(i) = Math.Acos( TempYp_Tc(i) )							'θ算出(位置)
#If OLD
			TempYp_T2(i) = Math.Asin( TempYp_Ts(i) )							'θ算出(位置)
#else
			If TempYp_T1(i) >= Math.PI / 2
				TempYp_T2(i) = TempYp_T1(i)											'θ算出(位置)
			Else
				TempYp_T2(i) = Math.Asin( TempYp_Ts(i) )							'θ算出(位置)
			End If
#End If

			TempXYp_T2(i) = (TempXp_T2(i) + TempYp_T2(i))/2
			dProgPer(5) = i * 100 / iSmpEnd
		Next i

		'sinとCosのどちらを使うかを選択
		'XY応力の位相差の場合は90度付近でほとんど動かないのでCosが適当
		'応力対位置の位相差は90度～0度の範囲になる。フルキャリブでの位相差は-0.27radぐらい。
		'フルキャリブの位相差がマイナスであれば0をまたぐためsinが適当
#If OLD
		Temp_T3Ave		= Temp_T1								'X応力とY応力の位相差
		TempXp_T3Ave	= TempXp_T2								'X応力とX位置の位相差
		TempYp_T3Ave	= TempYp_T2								'Y応力とY位置の位相差
		TempXYp_T3Ave	= TempXYp_T2							'X位置位相差とY位置位相差の平均値
		TempXY_T3Ave	= TempXY_T2								'XY応力とXY位置の位相差
#Else
		BatchMovingAverage(Temp_T1, Temp_T3Ave, iAllPriod)						'位相角αの半周期移動平均
		BatchMovingAverage(TempXp_T2, TempXp_T3Ave, iAllPriod)					'X位相角の半周期移動平均
		BatchMovingAverage(TempYp_T2, TempYp_T3Ave, iAllPriod)					'Y位相角の半周期移動平均
		BatchMovingAverage(TempXYp_T2, TempXYp_T3Ave, iAllPriod)				'Y位相角の半周期移動平均
		BatchMovingAverage(TempXY_T2, TempXY_T3Ave, iAllPriod)					'Y位相角の半周期移動平均
		'For i=0 To Temp_T3Ave.GetUpperBound(0)
		'	Temp_T3Ave(i) = Math.PI / 2											'位相補正を止めるため90度固定とする
		'Next i
		'BatchMovingAverage3(Temp_T1, Temp_T3Ave, TempXp_T2, TempXp_T3Ave, TempYp_T2, TempYp_T3Ave, iHalfPriod)
#end if




		'キャリブレーション、測定------------------------------------------------------------------------------------------
		Dim dXCos		As Double
		Dim dYCos		As Double
		Dim dXSin		As Double
		Dim dYSin		As Double
		Dim dRTemp_J100 As Double
		Dim dXYCos		As Double
		Dim dXYSin		As Double

		For i = 0  To iSmpEnd
			'位相角 範囲制限
			if Temp_T3Ave(i)	> dUpperdeg									'95deg
				Temp_T3Ave(i) = dUpperdeg
			Elseif Temp_T1(i)	<= dLowerdeg								'85deg
				Temp_T3Ave(i) = dLowerdeg
			End If
			'位相差補正
			'Temp_L(i)	= Math.Sin(Temp_T3Ave(i)) * TempY_AR(i)						'位相差補正 Y方向ベクトルのY成分（Xに直交する成分）配列
			'Temp_M(i)	= Math.Cos(Temp_T3Ave(i)) * TempY_AR(i)						'位相差補正 Y方向ベクトルのX成分（Xに平行成分）配列
			Temp_L(i)	= Math.Sin(Temp_T3Ave(i)) * TempY_A(i)						'位相差補正 Y方向ベクトルのY成分（Xに直交する成分）配列
			Temp_M(i)	= Math.Cos(Temp_T3Ave(i)) * TempY_A(i)						'位相差補正 Y方向ベクトルのX成分（Xに平行成分）配列
			'Temp_N(i)	= TempX_AR(i) - Temp_M(i)									'位相差補正 Y方向ベクトルのX成分で補正したX方向ベクトル
			Temp_N(i)	= TempX_A(i) - Temp_M(i)									'位相差補正 Y方向ベクトルのX成分で補正したX方向ベクトル
			'XYを合成 --------------------------------------------------
			If Not Double.IsNaN(Temp_T3Ave(i))
				Temp_G(i) = Math.Sqrt(Temp_N(i)^2 + Temp_L(i)^2)
			Else
				Temp_G(i) = 0
			End If
		Next i

		BatchMovingAverage(Temp_G, Temp_GAve, iAllPriod)						'合成後の一周期移動平均

		For i = 0  To iSmpEnd
		Temp_H(i) = Ctype(Temp_GAve(i),Int64)

			'ニュートン変換
			If Not Double.IsNaN(Temp_T3Ave(i))
				Select Case cp.iMeas
				Case eHoseiMeasMode.FullCalib, eHoseiMeasMode.ZeroCalib
					Temp_J(i) = Temp_H(i) * dNConv / Math.Sin(Temp_T3Ave(i))										'μN変換
				Case Else
					Temp_J(i) = (Temp_H(i) * dNConv / Math.Sin(Temp_T3Ave(i)) - cp.dZeroStress) * cp.dAttenuation  	'μN変換
					If cp.dFullStress <> 0
						Temp_J100(i) = Temp_J(i) / (cp.dFullStress)
					Else
						Temp_J100(i) = 0
					End If

				End Select
				Temp_J(i) = if( Temp_J(i)<0, 0, Temp_J(i) )									'負数はゼロにする
			Else
				Temp_J100(i) = 0
			End If

			Select Case cp.iMeas
			Case eHoseiMeasMode.GelTime, eHoseiMeasMode.FlowCurve		'ゲルタイム,フローカーブ

				TempXp_Rad(i) = If(Double.IsNaN(TempXp_T3Ave(i)), Double.NaN, TempXp_T3Ave(i) - cp.dPxPhase)	'動粘弾位相オフセット補正		
				TempYp_Rad(i) = If(Double.IsNaN(TempYp_T3Ave(i)), Double.NaN, TempYp_T3Ave(i) - cp.dPyPhase)	'動粘弾位相オフセット補正		
				TempXY_Rad(i) = If(Double.IsNaN(TempXY_T3Ave(i)), Double.NaN, TempXY_T3Ave(i))	-((cp.dPxPhase+cp.dPyPhase) / 2)				'動粘弾位相オフセット補正		
				TempXp_Rad(i) = If(TempXp_Rad(i) < 0, 0, TempXp_Rad(i))										'負数は０
				TempYp_Rad(i) = If(TempYp_Rad(i) < 0, 0, TempYp_Rad(i))										'負数は０
				TempXY_Rad(i) = If(TempXY_Rad(i) < 0, 0, TempXY_Rad(i))										'負数は０
				TempXp_Rad(i) = If(TempXp_Rad(i) > Math.PI/2, Math.PI/2, TempXp_Rad(i))						'90度を超えると90度
				TempYp_Rad(i) = If(TempYp_Rad(i) > Math.PI/2, Math.PI/2, TempYp_Rad(i))						'90度を超えると90度
				TempXY_Rad(i) = If(TempXY_Rad(i) > Math.PI/2, Math.PI/2, TempXY_Rad(i))						'90度を超えると90度
				If i < iIgnoreCount + 2 * iAllPriod
					TempXp_Rad(i) = Double.NaN
					TempYp_Rad(i) = Double.NaN
					TempXY_Rad(i) = Double.NaN
				End If

				dXSin   = Math.Sin(TempXp_Rad(i))
#If Old
				dXCos	= If(dXSin = 1, 0, Math.Cos(TempXp_Rad(i)))
#Else
				If TempXp_Rad(i) >= Math.PI / 2
					dXCos	= Math.Cos(Math.PI / 2)
				Else
					dXCos	= Math.Cos(TempXp_Rad(i))
				End If
#End If

				dYSin   = Math.Sin(TempYp_Rad(i))
#If Old
				dYCos   = If(dYSin = 1, 0, Math.Cos(TempYp_Rad(i)))
#Else
				If TempYp_Rad(i) >= Math.PI / 2
					dYCos	= Math.Cos(Math.PI / 2)
				Else
					dYCos	= Math.Cos(TempYp_Rad(i))
				End If
#End If

				dXYSin = Math.Sin(TempXY_Rad(i))
#If Old
				dXYCos = If(dXSin = 1, 0, Math.Cos(TempXY_Rad(i)))
#Else
				If TempXY_Rad(i) >= Math.PI / 2
					dXYCos	= Math.Cos(Math.PI / 2)
				Else
					dXYCos	= Math.Cos(TempXY_Rad(i))
				End If
#End If

				dRTemp_J100 = 1 - (Temp_J100(i) * dXYCos)									'貯蔵弾性率成分で振幅が小さくなる

				'粘度計算
				uVp.dMeasStress		= Temp_J(i) / 10^6										'測定応力(N)
				uVp.dVelocityRate	= dRTemp_J100											'せん断速度補正　貯蔵弾性率に応じて可変する
				''uVp.dMeasGap		= (cp.iZeroPos-p_dtFile.iPzBuf(i)) / 10^6				'測定GAP(m)
				Select Case cp.iHPPosControl
				Case 1		'熱板着圧制御
					Dim dZStrsAD As Int64 = 0
					dZStrsAD = CalcMeasureStrsToADValue(cp.dMeasureStress, cp.dDiameter, cp.cts.WeightCoefZ)
					uVp.dMeasGap		= (cp.iZeroPos - cp.dRCCBaseThickness - cp.clDt.iPzBuf(i) + (1/cp.cts.GapCorr) * dZStrsAD) / 10^6			'測定GAP(m)
				Case Else
					uVp.dMeasGap		= (cp.iZeroPos - cp.dRCCBaseThickness - cp.cldt.iPzBuf(i)) / 10^6			'測定GAP(m)
				End Select
				cp.dMesGap = uVp.dMeasGap * 10^3											'モニタ用測定GAP(mm)
				IF Double.isNaN(TempXp_BAve(i)) Or TempXp_BAve(i) <= 0
					uVp.dAmplitude		= 0													'設定振幅(m)
				Else
					Select Case cp.dAmplitude * 1000.0
					Case <= iSwitchAmpXYPos
					'0.1um解像度
						uVp.dAmplitude		= Math.Round(TempXp_BAve(i) * 2 / 10, 0) / 10^6		'設定振幅(m)
					Case Else
					'1um解像度
						uVp.dAmplitude		= TempXp_BAve(i) * 2 / 10^6							'設定振幅(m)
					End Select
				End If
				ViscosityCalc( uVp )														'粘度計算処理
				Temp_V(i) = uVp.dViscosity
				Temp_SS(i) = uVp.dShearStress												'せん断応力
				Temp_SV(i) = uVp.dShearVelocity												'せん断速度

				If uVp.dMeasGap > 0 And uVp.dAmplitude <> 0
					Temp_Stn(i) =  ((uVp.dAmplitude * dRTemp_J100) / 2) / uVp.dMeasGap		'サンプルに与えたひずみ
					Temp_SM(i) = Temp_SS(i) / Temp_Stn(i)									'ずり弾性率G*
					'If(i > 100000)
					'		Temp_SM(i) = 1
					'End If
				Else
					Temp_Stn(i) = 0
					Temp_SM(i) = 0	
				End If



				'動的粘弾性'振幅0時はゼロ
				TempXp_G1(i) = If(uVp.dAmplitude <> 0, Temp_SM(i) * dXYCos, 0)					'X貯蔵弾性率
				TempXp_G2(i) = If(uVp.dAmplitude <> 0, Temp_SM(i) * dXYSin, 0)					'X損失弾性率
#If OLD
				TempXp_Tn(i) = Math.Tan(TempXY_Rad(i))											'X損失正接
#Else
				If TempXY_Rad(i) >= Math.PI / 2
					TempXp_Tn(i) = Double.NaN
				Else
					TempXp_Tn(i) = Math.Tan(TempXY_Rad(i))										'X損失正接
				End If
#end If
				TempXp_Tn(i) = If(TempXp_Tn(i)< 0, 0, TempXp_Tn(i))								'X損失正接(負数は０)

				TempYp_G1(i) = If(uVp.dAmplitude <> 0, Temp_SM(i) * dXYCos, 0)					'Y貯蔵弾性率
				TempYp_G2(i) = If(uVp.dAmplitude <> 0, Temp_SM(i) * dXYSin, 0)					'Y損失弾性率
#If OLD
				TempYp_Tn(i) = Math.Tan(TempXY_Rad(i))											'Y損失正接
#Else
				If TempXY_Rad(i) >= Math.PI / 2
					TempYp_Tn(i) = Double.NaN
				Else
					TempYp_Tn(i) = Math.Tan(TempXY_Rad(i))										'X損失正接
				End If
#end If
				TempYp_Tn(i) = If(TempYp_Tn(i)< 0, 0, TempYp_Tn(i))								'Y損失正接(負数は０)


			Case eHoseiMeasMode.FullCalib, eHoseiMeasMode.ZeroCalib							'キャリブレーション
				'動的粘弾性	'振幅0時はゼロ
				TempXp_Rad(i) = If(uVp.dAmplitude <> 0 Or TempXp_T3Ave(i) > 0, TempXp_T3Ave(i), 0)'Xモードで使用するcp.dPxPhaseになる
				TempYp_Rad(i) = If(uVp.dAmplitude <> 0 Or TempYp_T3Ave(i) > 0, TempYp_T3Ave(i), 0)'XYモードで使用するcp.dPyPhaseになる
			End Select
			Temp_J100(i) *= 100.0															'%変換

			dProgPer(6) = i * 100 / iSmpEnd
		Next i

		Dim iLen As Int64 = cp.iSampleNum - iQuotPriod
		Dim iSrt As Int64 = 0
		Select Case cp.iMeas
		Case eHoseiMeasMode.GelTime		'ゲルタイム
			'処理済 補正データをセット （積分による遅れ分を(1/4周期)前シフト） ---------------------------------------------
			iLen = cp.iSampleNum - iQuotPriod
			iSrt = iQuotPriod

			Array.Copy(TempXY_Rad,	iSrt, cp.clDt.dXPosPhase,	0,	iLen)		'X位相差
			Array.Copy(TempXY_Rad,	iSrt, cp.clDt.dYPosPhase,	0,	iLen)		'Y位相差

		Case Else
			iLen = cp.iSampleNum
			iSrt = 0

			Array.Copy(TempXp_Rad,	iSrt, cp.clDt.dXPosPhase,	0,	iLen)		'X位相差
			Array.Copy(TempYp_Rad,	iSrt, cp.clDt.dYPosPhase,	0,	iLen)		'Y位相差

		End Select
		Array.Copy(Temp_J,		iSrt, cp.clDt.iHosei,		0,	iLen)		'応力値
		Array.Copy(Temp_J100,	iSrt, cp.clDt.dHosei100,	0,	iLen)		'応力値%
		Array.Copy(Temp_V,		iSrt, cp.clDt.dViscosity,	0,	iLen)		'粘度
		Array.Copy(TempXp_G1,	iSrt, cp.clDt.dXStrgMod,	0,	iLen)		'X貯蔵弾性率
		Array.Copy(TempXp_G2,	iSrt, cp.clDt.dXElasMod,	0,	iLen)		'X損失弾性率
		Array.Copy(TempXp_Tn,	iSrt, cp.clDt.dXLossTan,	0,	iLen)		'X損失正接
		Array.Copy(TempYp_G1,	iSrt, cp.clDt.dYStrgMod,	0,	iLen)		'Y貯蔵弾性率
		Array.Copy(TempYp_G2,	iSrt, cp.clDt.dYElasMod,	0,	iLen)		'Y損失弾性率
		Array.Copy(TempYp_Tn,	iSrt, cp.clDt.dYLossTan,	0,	iLen)		'Y損失正接
		Array.Copy(Temp_Stn,	iSrt, cp.clDt.dSplStrain,	0,	iLen)		'サンプルに与えたひずみ
		Array.Copy(Temp_SM,		iSrt, cp.clDt.dShearMod,	0,	iLen)		'ずり弾性率
		Array.Copy(Temp_SV,		iSrt, cp.clDt.dShearVelo,	0,	iLen)		'せん断速度

		Select Case cp.iMeas
		Case eHoseiMeasMode.GelTime		'ゲルタイム
			'前シフトした分最終部分を埋める
			For i=cp.clDt.iHosei.Length-iSrt To cp.clDt.iHosei.GetUpperBound(0)
				cp.clDt.iHosei(i)		= cp.clDt.iHosei(i-1)
				cp.clDt.dHosei100(i)	= cp.clDt.dHosei100(i-1)
				cp.clDt.dViscosity(i)	= cp.clDt.dViscosity(i-1)
				cp.clDt.dXPosPhase(i)	= cp.clDt.dXPosPhase(i-1)
				cp.clDt.dXStrgMod(i)	= cp.clDt.dXStrgMod(i-1)
				cp.clDt.dXElasMod(i)	= cp.clDt.dXElasMod(i-1)
				cp.clDt.dXLossTan(i)	= cp.clDt.dXLossTan(i-1)
				cp.clDt.dYPosPhase(i)	= cp.clDt.dYPosPhase(i-1)
				cp.clDt.dYStrgMod(i)	= cp.clDt.dYStrgMod(i-1)
				cp.clDt.dYElasMod(i)	= cp.clDt.dYElasMod(i-1)
				cp.clDt.dYLossTan(i)	= cp.clDt.dYLossTan(i-1)
				cp.clDt.dSplStrain(i)	= cp.clDt.dSplStrain(i-1)
				cp.clDt.dShearMod(i)	= cp.clDt.dShearMod(i-1)
				cp.clDt.dShearVelo(i)	= cp.clDt.dShearVelo(i-1)
			Next
		Case eHoseiMeasMode.FlowCurve		'フローカーブ
#If 0
			For i=0 To cp.clDt.iHosei.GetUpperBound(0)
				cp.clDt.iHosei(i)		= If( Double.IsNaN(cp.clDt.iHosei(i)), 0.0, cp.clDt.iHosei(i))
				cp.clDt.dHosei100(i)	= If( Double.IsNaN(cp.clDt.dHosei100(i)), 0.0, cp.clDt.dHosei100(i))
				cp.clDt.dViscosity(i)	= If( Double.IsNaN(cp.clDt.dViscosity(i)), 0.0, cp.clDt.dViscosity(i))
				cp.clDt.dXPosPhase(i)	= If( Double.IsNaN(cp.clDt.dXPosPhase(i)), 0.0, cp.clDt.dXPosPhase(i))
				cp.clDt.dXStrgMod(i)	= If( Double.IsNaN(cp.clDt.dXStrgMod(i)), 0.0, cp.clDt.dXStrgMod(i))
				cp.clDt.dXElasMod(i)	= If( Double.IsNaN(cp.clDt.dXElasMod(i)), 0.0, cp.clDt.dXElasMod(i))
				cp.clDt.dXLossTan(i)	= If( Double.IsNaN(cp.clDt.dXLossTan(i)), 0.0, cp.clDt.dXLossTan(i))
				cp.clDt.dYPosPhase(i)	= If( Double.IsNaN(cp.clDt.dYPosPhase(i)), 0.0, cp.clDt.dYPosPhase(i))
				cp.clDt.dYStrgMod(i)	= If( Double.IsNaN(cp.clDt.dYStrgMod(i)), 0.0, cp.clDt.dYStrgMod(i))
				cp.clDt.dYElasMod(i)	= If( Double.IsNaN(cp.clDt.dYElasMod(i)), 0.0, cp.clDt.dYElasMod(i))
				cp.clDt.dYLossTan(i)	= If( Double.IsNaN(cp.clDt.dYLossTan(i)), 0.0, cp.clDt.dYLossTan(i))
				cp.clDt.dSplStrain(i)	= If( Double.IsNaN(cp.clDt.dSplStrain(i)), 0.0, cp.clDt.dSplStrain(i))
				cp.clDt.dShearMod(i)	= If( Double.IsNaN(cp.clDt.dShearMod(i)), 0.0, cp.clDt.dShearMod(i))
				cp.clDt.dShearVelo(i)	= If( Double.IsNaN(cp.clDt.dShearVelo(i)), 0.0, cp.clDt.dShearVelo(i))
			Next
#end If
		End Select

	Case 1	'GelTime測定リアルタイム
		j = m_TempX_A_R_pt
		'Array.Resize(Temp_BRx,iSmpEnd+1)
		'Array.Resize(Temp_BRy,iSmpEnd+1)

		'-------------------------------------------------------------------------------------------------
		For i = 0  To iSmpEnd
			'振幅を計算 TempX_B, TempY_B---------------------------------------------------
			TempX_B(i)	= Math.Sqrt( TempX_A(j)^2	+ TempX_Q(j)^2 )
			TempY_B(i)	= Math.Sqrt( TempY_A(j)^2	+ TempY_Q(j)^2 )
			TempXp_B(i) = Math.Sqrt( TempXp_A(j)^2	+ TempXp_Q(j)^2 )
			TempYp_B(i) = Math.Sqrt( TempYp_A(j)^2	+ TempYp_Q(j)^2 )



			'Temp_BCenterは大きい方をセット
			'if TempX_B(i) > TempY_B(i)
			'	Temp_BCenter(i) = TempX_B(i)
			'Else
			'	Temp_BCenter(i) = TempY_B(i)
			'End If

			'If Double.IsNaN(TempX_B(i)) Or Double.IsNaN(TempY_B(i))
			'	Temp_BRx(i) =  1													'振幅比率計算 X
			'	Temp_BRy(i) =  1													'振幅比率計算 Y
			'Else
			'	'Temp_BCenter(i) = (TempX_B(i) + TempY_B(i)) / 2
			'	Temp_BRx(i) = If(TempX_B(i) < 1, 1, Temp_BCenter(i) / TempX_B(i))	'振幅比率計算 X
			'	Temp_BRy(i) = If(TempY_B(i) < 1, 1, Temp_BCenter(i) / TempY_B(i))	'振幅比率計算 Y
			'End If

			'TrigoRangeLimit(Temp_BRx(i), dLowerR, dUpperR, 1, 1)
			'TrigoRangeLimit(Temp_BRy(i), dLowerR, dUpperR, 1, 1)
			'Temp_BRx(i) = 1
			'Temp_BRy(i) = 1

			'TempX_AR(j) = TempX_A(j) * Temp_BRx(i)								'1次積分X配列を補正
			'TempY_AR(j) = TempY_A(j) * Temp_BRy(i)								'1次積分Y配列を補正
			'TempX_QR(j) = TempX_Q(j) * Temp_BRx(i)								'2次積分X配列を補正
			'TempY_QR(j) = TempY_Q(j) * Temp_BRy(i)								'2次積分Y配列を補正

			TempXY_BAve(i)	= (TempX_B(i)	+ TempY_B(i)) / 2
			TempXYp_BAve(i) = (TempXp_B(i)	+ TempYp_B(i)) / 2

			If j > iIgnoreCount
			'内積/外積を計算 Temp_D, Temp_C --------------------------------------------------
				If TempX_Q(j) = 0 Or TempY_Q(j) = 0
					'TempX_BR(i) = 0
					'TempY_BR(i) = 0
					TempX_B(i) = 0
					TempY_B(i) = 0
					Temp_D(i) = 0
					Temp_C(i) = 0
					TempXY_D(i) = 0
					TempXY_C(i) = 0
					TempXp_D(i) = 0
					TempXp_C(i) = 0
					TempYp_D(i) = 0
					TempYp_C(i) = 0
				Else
					'TempX_BR(i) = Math.Sqrt( TempX_AR(j)^2 + TempX_QR(j)^2 )					'比率補正振幅
					'TempY_BR(i) = Math.Sqrt( TempY_AR(j)^2 + TempY_QR(j)^2 )					'比率補正振幅
					'Temp_D(i) = TempX_AR(j) * TempY_AR(j) + TempX_QR(j) * TempY_QR(j)			'内積
					'Temp_C(i) = TempX_AR(j) * TempY_QR(j) - TempY_AR(j) * TempX_QR(j)			'外積
					'TempXY_D(i) = TempX_AR(j) * TempXp_A(j) + TempY_AR(j) * TempYp_A(j)			'内積
					'TempXY_C(i) = TempX_AR(j) * TempYp_A(j) - TempXp_A(j) * TempY_AR(j)			'外積
					'TempXp_D(i) = p_dtFile.iPxbuf(j) * TempX_AR(j) + TempXp_Q(j) * TempX_QR(j)			'内積(位置)
					'TempXp_C(i) = p_dtFile.iPxbuf(j) * TempX_QR(j) - TempX_AR(j) * TempXp_Q(j)			'外積(位置)
					'TempYp_D(i) = p_dtFile.iPybuf(j) * TempY_AR(j) + TempYp_Q(j) * TempY_QR(j)			'内積(位置)
					'TempYp_C(i) = p_dtFile.iPybuf(j) * TempY_QR(j) - TempY_AR(j) * TempYp_Q(j)			'外積(位置)
					TempX_B(i) = Math.Sqrt( TempX_A(j)^2 + TempX_Q(j)^2 )								'比率補正振幅
					TempY_B(i) = Math.Sqrt( TempY_A(j)^2 + TempY_Q(j)^2 )								'比率補正振幅
					Temp_D(i) = TempX_A(j) * TempY_A(j) + TempX_Q(j) * TempY_Q(j)						'内積
					Temp_C(i) = TempX_A(j) * TempY_Q(j) - TempY_A(j) * TempX_Q(j)						'外積
					TempXY_D(i) = TempX_A(j) * TempXp_A(j) + TempY_A(j) * TempYp_A(j)					'内積
					TempXY_C(i) = TempX_A(j) * TempYp_A(j) - TempXp_A(j) * TempY_A(j)					'外積
					TempXp_D(i) = p_dtFile.iPxbuf(j) * TempX_A(j) + TempXp_Q(j) * TempX_Q(j)			'内積(位置)
					TempXp_C(i) = p_dtFile.iPxbuf(j) * TempX_Q(j) - TempX_A(j) * TempXp_Q(j)			'外積(位置)
					TempYp_D(i) = p_dtFile.iPybuf(j) * TempY_A(j) + TempYp_Q(j) * TempY_Q(j)			'内積(位置)
					TempYp_C(i) = p_dtFile.iPybuf(j) * TempY_Q(j) - TempY_A(j) * TempYp_Q(j)			'外積(位置)

				End If
			Else
			'XY動作するまでは計算は行わない
				'TempX_BR(i) = Double.NaN
				'TempY_BR(i) = Double.NaN
				TempX_B(i)	= Double.NaN
				TempY_B(i)	= Double.NaN
				Temp_D(i)	= Double.NaN
				Temp_C(i)	= Double.NaN
				TempXY_D(i)	= Double.NaN
				TempXY_C(i)	= Double.NaN
				TempXp_D(i) = Double.NaN
				TempXp_C(i) = Double.NaN
				TempYp_D(i) = Double.NaN
				TempYp_C(i) = Double.NaN
			End If

			'XY位相差θを算出 --------------------------------------------------
			'If TempX_BR(i) = 0 Or TempY_BR(i) = 0
			If TempX_B(i) = 0 Or TempY_B(i) = 0
				Temp_Tc(i) = Double.NaN												'Cosを演算
				Temp_Ts(i) = Double.NaN												'Sinを演算
			Else
				'Temp_Tc(i) = Temp_D(i) / (TempX_BR(i) * TempY_BR(i))				'Cosを演算
				'Temp_Ts(i) = Temp_C(i) / (TempX_BR(i) * TempY_BR(i))				'Sinを演算
				Temp_Tc(i) = Temp_D(i) / (TempX_B(i) * TempY_B(i))				'Cosを演算
				Temp_Ts(i) = Temp_C(i) / (TempX_B(i) * TempY_B(i))				'Sinを演算
				TrigoRangeLimit(Temp_Tc(i), -1, 1, -1, 1)
				TrigoRangeLimit(Temp_Ts(i), -1, 1, -1, 1)
			End If
			Temp_T3(i) = Math.Acos( Temp_Tc(i) )									'θ算出
			Temp_T2(i) = Math.Asin( Temp_Ts(i) )									'θ算出

			'XY応力とXY位置の位相差θを算出 --------------------------------------------------
			'If TempX_BR(i) = 0 Or TempY_BR(i) = 0
			If TempX_B(i) = 0 Or TempY_B(i) = 0
				TempXY_Tc(i) = Double.NaN												'Cosを演算
				TempXY_Ts(i) = Double.NaN												'Sinを演算
			Else
				TempXY_Tc(i) = TempXY_D(i) / (TempXY_BAve(i) * TempXYp_BAve(i))		'Cosを演算
				TempXY_Ts(i) = TempXY_C(i) / (TempXY_BAve(i) * TempXYp_BAve(i))		'Sinを演算
				TrigoRangeLimit(TempXY_Tc(i), -1, 1, -1, 1)
				TrigoRangeLimit(TempXY_Ts(i), -1, 1, -1, 1)
			End If
			TempXY_T1(i) = Math.Acos( TempXY_Tc(i) )									'θ算出　Cosの範囲0～180度で表現し90度付近が高精度
			TempXY_T2(i) = Math.Asin( TempXY_Ts(i) )									'θ算出　Sinの範囲-90度～+90度で表現し0度付近が高精度
			TempXY_Rad(i) = TempXY_T2(i) - cp.dPyPhase									'XY位相差をキャリブ値でオフセット補正

			'X位相差θを算出（位置） --------------------------------------------------
			'If Double.IsNaN(TempX_BR(i)) Or Double.IsNaN(TempY_BR(i))
			If Double.IsNaN(TempX_B(i)) Or Double.IsNaN(TempY_B(i))
				TempXp_Tc(i) = Double.NaN											'Cosを演算(位置)
				TempXp_Ts(i) = Double.NaN											'Sinを演算(位置)
			Else
				'TempXp_Tc(i) = TempXp_D(i) / (TempX_BR(i) * TempXp_B(i))			'Cosを演算
				'TempXp_Ts(i) = TempXp_C(i) / (TempX_BR(i) * TempXp_B(i))			'Sinを演算
				TempXp_Tc(i) = TempXp_D(i) / (TempX_B(i) * TempXp_B(i))				'Cosを演算
				TempXp_Ts(i) = TempXp_C(i) / (TempX_B(i) * TempXp_B(i))				'Sinを演算
			End If
			TempXp_T1(i) = Math.Acos( TempXp_Tc(i) )								'θ算出(位置)
			TempXp_T2(i) = Math.Asin( TempXp_Ts(i) )								'θ算出(位置)
			TempXp_Rad(i) = TempXp_T2(i) - cp.dPxPhase								'X位相差をキャリブ値でオフセット補正
				
			'Y位相差θを算出（位置） --------------------------------------------------
			'If Double.IsNaN(TempX_BR(i)) Or Double.IsNaN(TempY_BR(i))
			If Double.IsNaN(TempX_B(i)) Or Double.IsNaN(TempY_B(i))
				TempYp_Tc(i) = Double.NaN											'Cosを演算(位置)
				TempYp_Ts(i) = Double.NaN											'Sinを演算(位置)
			Else
				'TempYp_Tc(i) = TempYp_D(i) / (TempY_BR(i) * TempYp_B(i))			'Cosを演算
				'TempYp_Ts(i) = TempYp_C(i) / (TempY_BR(i) * TempYp_B(i))			'Sinを演算
				TempYp_Tc(i) = TempYp_D(i) / (TempY_B(i) * TempYp_B(i))				'Cosを演算
				TempYp_Ts(i) = TempYp_C(i) / (TempY_B(i) * TempYp_B(i))				'Sinを演算
			End If
			TempYp_T1(i) = Math.Acos( TempYp_Tc(i) )								'θ算出(位置)
			TempYp_T2(i) = Math.Asin( TempYp_Ts(i) )								'θ算出(位置)
			TempYp_Rad(i) = TempYp_T2(i) - cp.dPyPhase								'Y位相差をキャリブ値でオフセット補正

			j += 1
		Next i

		Dim Temp_T3_Ave As Double = Temp_T3.Average()								'位相を平均化
		''Dim Temp_T3_Ave As Double = Math.PI / 2									'位相補正を止めるため90度固定とする
		'Dim Temp_T3_Ave() As Double = Temp_T3

		j = m_TempX_A_R_pt
		'-------------------------------------------------------------------------------------------------
		Dim dXCos			As Double 
		Dim dYCos			As Double 
		Dim dXSin			As Double
		Dim dYSin			As Double
		Dim dRTemp_J100 	As Double
		Dim dXYCos			As Double
		Dim dXYSin			As Double

		For i = 0  To iSmpEnd
			dXSin		= Math.Sin(TempXp_Rad(i))
			dXCos		= If(dxSin = 1, 0, Math.Cos(TempXp_Rad(i)))
			dYSin		= Math.Sin(TempYp_Rad(i))
			dYCos		= If(dYSin = 1, 0, Math.Cos(TempYp_Rad(i)))

			dXYSin		= Math.Sin(TempXY_Rad(i))
			dXYCos		= If(dYSin = 1, 0, Math.Cos(TempXY_Rad(i)))

		'位相差補正
			'Temp_L(i)	= Math.Sin(Temp_T3_Ave) * TempY_AR(j)						'位相差補正 Y方向ベクトルのY成分（Xに直交する成分）配列
			'Temp_M(i)	= Math.Cos(Temp_T3_Ave) * TempY_AR(j)						'位相差補正 Y方向ベクトルのX成分（Xに平行成分）配列
			Temp_L(i)	= Math.Sin(Temp_T3_Ave) * TempY_A(j)						'位相差補正 Y方向ベクトルのY成分（Xに直交する成分）配列
			Temp_M(i)	= Math.Cos(Temp_T3_Ave) * TempY_A(j)						'位相差補正 Y方向ベクトルのX成分（Xに平行成分）配列
			'Temp_N(i)	= TempX_AR(j) - Temp_M(i)									'位相差補正 Y方向ベクトルのX成分で補正したX方向ベクトル
			Temp_N(i)	= TempX_A(j) - Temp_M(i)									'位相差補正 Y方向ベクトルのX成分で補正したX方向ベクトル

		'XYを合成 --------------------------------------------------
			If Not Double.IsNaN(Temp_T3_Ave)
				Temp_J(i) = Math.Sqrt(Temp_N(i)^2 + Temp_L(i)^2)
				Select Case cp.iMeas
				Case eHoseiMeasMode.FullCalib, eHoseiMeasMode.ZeroCalib
					Temp_J(i) = Temp_J(i) * dNConv / Math.Sin(Temp_T3_Ave)											'μN変換
				Case Else
					Temp_J(i) = (Temp_J(i) * dNConv / Math.Sin(Temp_T3_Ave) - cp.dZeroStress) * cp.dAttenuation  	'μN変換
					If cp.dFullStress <> 0
						Temp_J100(i) = Temp_J(i) / cp.dFullStress									'フルストレス（100%応力）に対する現在の応力割合
					Else
						Temp_J100(i) = 0									
					End If
				End Select
				Temp_J(i) = if( Temp_J(i)<0.0, 0.0, Temp_J(i) )								'負数はゼロにする
				dRTemp_J100 = 1 - (Temp_J100(i) * dXYCos)
				Temp_J100(i) *= 100.0
			Else
				Temp_J(i) = 0.0
				dRTemp_J100 = 1
				Temp_J100(i) = 0
			End If


		'粘度計算
			uVp.dMeasStress		= Temp_J(i) / 10^6										'測定応力(N)
			uVp.dVelocityRate	= dRTemp_J100											'せん断速度補正
			''uVp.dMeasGap		= (cp.iZeroPos-p_dtFile.iPzBuf(j)) / 10^6				'測定GAP(m)
			Select Case cp.iHPPosControl
			Case 1		'熱板着圧制御
				Dim dZStrsAD As Int64 = 0
				dZStrsAD = CalcMeasureStrsToADValue(cp.dMeasureStress, cp.dDiameter, cp.cts.WeightCoefZ)
				uVp.dMeasGap		= (cp.iZeroPos - cp.dRCCBaseThickness - p_dtFile.iPzBuf(j) + (1/cp.cts.GapCorr) * dZStrsAD) / 10^6			'測定GAP(m)
			Case Else
				uVp.dMeasGap		= (cp.iZeroPos - cp.dRCCBaseThickness - p_dtFile.iPzBuf(j)) / 10^6				'測定GAP(m)
			End Select
			cp.dMesGap = uVp.dMeasGap * 10^3											'モニタ用測定GAP(mm)
			IF Double.isNaN(TempXp_B(i)) Or TempXp_B(i) <= 0
				uVp.dAmplitude		= 0													'設定振幅(m)
			Else
				''uVp.dAmplitude		= Math.Round(TempXp_B(i) * 2 / 10, 0) / 10^6		'設定振幅(m)
				Select Case cp.dAmplitude * 1000.0
				Case <= iSwitchAmpXYPos
				'0.1um解像度
					uVp.dAmplitude		= Math.Round(TempXp_B(i) * 2 / 10, 0) / 10^6		'設定振幅(m)
				Case Else
				'1um解像度
					uVp.dAmplitude		= TempXp_B(i) * 2 / 10^6							'設定振幅(m)
				End Select
			End If
			ViscosityCalc( uVp )														'粘度計算処理
			Temp_V(i) = uVp.dViscosity
			Temp_SS(i) = uVp.dShearStress												'せん断応力
			Temp_SV(i) = uVp.dShearVelocity												'せん断速度

			If uVp.dMeasGap > 0 And uVp.dAmplitude <> 0
				Temp_Stn(i) =  ((uVp.dAmplitude * dRTemp_J100) / 2) / uVp.dMeasGap 		'サンプルに与えたひずみ
				Temp_SM(i) = Temp_SS(i) / Temp_Stn(i)									'ずり弾性率G*
			Else
				Temp_Stn(i) = 0
				Temp_SM(i) = 0	
			End If

			'動的粘弾性'振幅0時はゼロ
			TempXp_G1(i) = If(uVp.dAmplitude <> 0, Temp_SM(i) * dXYCos, 0)					'X貯蔵弾性率
			TempXp_G2(i) = If(uVp.dAmplitude <> 0, Temp_SM(i) * dXYSin, 0)					'X損失弾性率
			TempXp_Tn(i) = Math.Tan(TempXY_Rad(i))											'X損失正接
			TempXp_Tn(i) = If(TempXp_Tn(i)< 0, 0, TempXp_Tn(i))								'X損失正接(負数は０)

			TempYp_G1(i) = If(uVp.dAmplitude <> 0, Temp_SM(i) * dXYCos, 0)					'Y貯蔵弾性率
			TempYp_G2(i) = If(uVp.dAmplitude <> 0, Temp_SM(i) * dXYSin, 0)					'Y損失弾性率
			TempYp_Tn(i) = Math.Tan(TempXY_Rad(i))											'Y損失正接
			TempYp_Tn(i) = If(TempYp_Tn(i)< 0, 0, TempYp_Tn(i))								'Y損失正接(負数は０)
			j += 1
		Next i

		m_TempX_A_R_pt += cp.iSampleNum										'TempX_1の読み出しポイント更新

		'処理済 補正データを追加 ---------------------------------------------------------------------------------------
		Array.Copy(Temp_J,		0, cp.clDt.iHosei,		cp.clDt.iHosei.Length-cp.iSampleNum,	cp.iSampleNum)	'応力値
		Array.Copy(Temp_J100,	0, cp.clDt.dHosei100,	cp.clDt.dHosei100.Length-cp.iSampleNum,	cp.iSampleNum)		'応力値%
		Array.Copy(Temp_V,		0, cp.clDt.dViscosity, cp.clDt.dViscosity.Length-cp.iSampleNum,	cp.iSampleNum)		'粘度
		Array.Copy(TempXp_Rad,	0, cp.clDt.dXPosPhase, cp.clDt.dXPosPhase.Length-cp.iSampleNum,	cp.iSampleNum)		'X位相差
		Array.Copy(TempXp_G1,	0, cp.clDt.dXStrgMod,	cp.clDt.dXStrgMod.Length-cp.iSampleNum,	cp.iSampleNum)		'X貯蔵弾性率
		Array.Copy(TempXp_G2,	0, cp.clDt.dXElasMod,	cp.clDt.dXElasMod.Length-cp.iSampleNum,	cp.iSampleNum)		'X損失弾性率
		Array.Copy(TempXp_Tn,	0, cp.clDt.dXLossTan,	cp.clDt.dXLossTan.Length-cp.iSampleNum,	cp.iSampleNum)		'X損失正接
		Array.Copy(TempYp_Rad,	0, cp.clDt.dYPosPhase, cp.clDt.dYPosPhase.Length-cp.iSampleNum,	cp.iSampleNum)		'Y位相差
		Array.Copy(TempYp_G1,	0, cp.clDt.dYStrgMod,	cp.clDt.dYStrgMod.Length-cp.iSampleNum,	cp.iSampleNum)		'Y貯蔵弾性率
		Array.Copy(TempYp_G2,	0, cp.clDt.dYElasMod,	cp.clDt.dYElasMod.Length-cp.iSampleNum,	cp.iSampleNum)		'Y損失弾性率
		Array.Copy(TempYp_Tn,	0, cp.clDt.dYLossTan,	cp.clDt.dYLossTan.Length-cp.iSampleNum,	cp.iSampleNum)		'Y損失正接
		Array.Copy(Temp_Stn,	0, cp.clDt.dSplStrain, cp.clDt.dSplStrain.Length-cp.iSampleNum,	cp.iSampleNum)		'サンプルに与えたひずみ
		Array.Copy(Temp_SM,		0, cp.clDt.dShearMod,	cp.clDt.dShearMod.Length-cp.iSampleNum,	cp.iSampleNum)		'ずり弾性率
		Array.Copy(Temp_SV,		0, cp.clDt.dShearVelo,	cp.clDt.dShearVelo.Length-cp.iSampleNum,	cp.iSampleNum)	'剪断速度

		'リアルタイム数値モニタ表示用(frmMain)
		cp.clDt.p_curStress	= cp.clDt.iHosei(cp.clDt.iHosei.Length-cp.iSampleNum)								'応力値
		cp.clDt.p_curStressP	= cp.clDt.dHosei100(cp.clDt.dHosei100.Length-cp.iSampleNum)							'応力値%
		cp.clDt.p_curViscosity	= cp.clDt.dViscosity(cp.clDt.dViscosity.Length-cp.iSampleNum)						'粘度
		cp.clDt.p_curPhase		= cp.clDt.dXPosPhase(cp.clDt.dXPosPhase.Length-cp.iSampleNum)						'X位相差
		cp.clDt.p_curSplStrain	= cp.clDt.dSplStrain(cp.clDt.dSplStrain.Length-cp.iSampleNum)						'サンプルに与えたひずみ
		cp.clDt.p_curShearMod	= cp.clDt.dShearMod(cp.clDt.dShearMod.Length-cp.iSampleNum)							'ずり弾性率
		cp.clDt.p_curShearVelo	= cp.clDt.dShearVelo(cp.clDt.dShearVelo.Length-cp.iSampleNum)						'剪断速度
		cp.clDt.p_curStrgMod	= cp.clDt.dXStrgMod(cp.clDt.dXStrgMod.Length-cp.iSampleNum)							'X貯蔵弾性率
		cp.clDt.p_curElasMod	= cp.clDt.dXElasMod(cp.clDt.dXElasMod.Length-cp.iSampleNum)							'X損失弾性率
		cp.clDt.p_curLosTan	= cp.clDt.dXLossTan(cp.clDt.dXLossTan.Length-cp.iSampleNum)							'X損失正接




	End Select


		
	'IIRフィルタまたはスキップ
	Select Case iBranch
	Case 0		'キャリブレーションまたは最終測定のとき	
		'処理済 補正微分データを追加 ---------------------------------------------------------------------------------------
		'If cp.iCalcType = eCalcMethod.SLOPE
			MakeHoseiToSlope(cp)
		'End If
		Select Case cp.iMeas
		Case eHoseiMeasMode.GelTime		'ゲルタイム
			If cp.fltSet.iEnable = 1
				PostFilterProc(cp)		'計算後デジタルフィルタ処理
			Else
			'スキップ
			End If
		End Select

	Case Else
	End Select



#If 1
If p_bCalcResultFOutEn		'隠しコマンドで有効か
If iBranch = 0				'最終一括処理,キャリブレーション時のみ

	'デバッグ用ファイル出力
	Dim fs As New FileStream("C:\Test\debugCorrect_" & Now().ToString("MMddhhmmss") & ".csv", FileMode.Create, FileAccess.Write, FileShare.ReadWrite)
	Dim swr As Textwriter = New StreamWriter(fs, Encoding.UTF8)
	Dim sLineBuf As String = nothing
	'テキストファイル(CSV)書込み
	'ヘッダ行
	sLineBuf = "Time,iOxBuf,iOyBuf,iPxBuf,iPyBuf,TempX_1,TempY_1,TempX_A,TempY_A,TempX_Q,TempY_Q,TempX_BAve,TempY_BAve,Temp_BCenter,"
	sLineBuf = sLineBuf & "Temp_DAve,Temp_C,Temp_Ts,Temp_Tc,Temp_T1,Temp_T2,Temp_T3,Temp_T3Ave,Temp_J,Temp_H,Temp_K,Temp_L,Temp_M,Temp_N,Slope,dViscosity,iHosei,"
	sLineBuf = sLineBuf & "TempXp_A,TempYp_A,TempXp_Q,TempYp_Q,TempXp_BAve,TempYp_BAve,TempXY_D,TempXY_C,TempYp_D,TempYp_C,TempXY_Tc,TempXY_T1,TempXY_Ts,TempXY_T2,TempYp_Tc,TempYp_T1,TempYp_Ts,TempYp_T2,TempXY_Rad,TempYp_T3_Ave,Temp_J100,"
	sLineBuf = sLineBuf & "dXPosPhase,dYPosPhase,dSplStrain,dShearMod,dShearVelo,dXStrgMod,dXElasMod,dXLossTan,dYStrgMod,dYElasMod,dYLossTan,Temp_G, Temp_GAve"

	swr.WriteLine(sLineBuf)
	Dim dTime As Double = 0.0
	For i = 0 To cp.clDt.iOxBuf.GetUpperBound(0)
		sLineBuf = dTime.ToString("F3") & "," & _
		cp.clDt.iOxBuf(i).ToString & "," & _
		cp.clDt.iOyBuf(i).ToString & "," & _
		p_dtFile.iPxBuf(i).ToString & "," & _
		p_dtFile.iPyBuf(i).ToString & "," & _
		TempX_1(i).ToString & "," & _
		TempY_1(i).ToString  & "," & _
		TempX_A(i).ToString & "," & _
		TempY_A(i).ToString  & "," & _
		TempX_Q(i).ToString & "," & _
		TempY_Q(i).ToString  & "," & _
		TempX_BAve(i).ToString & "," & _
		TempY_BAve(i).ToString  & "," & _
		Temp_BCenter(i).ToString & ","


		sLineBuf = sLineBuf & _
		Temp_DAve(i).ToString & "," & _
		Temp_C(i).ToString & "," & _
		Temp_Ts(i).ToString & "," & _
		Temp_Tc(i).ToString & "," & _
		Temp_T1(i).ToString & "," & _
		Temp_T2(i).ToString & "," & _
		Temp_T3(i).ToString & "," & _
		Temp_T3Ave(i).ToString & "," & _
		Temp_J(i).ToString & "," & _
		Temp_H(i).ToString & "," & _
		Temp_K(i).ToString & "," & _
		Temp_L(i).ToString & "," & _
		Temp_M(i).ToString & "," & _
		Temp_N(i).ToString & "," & _
		cp.clDt.iSlope(i).ToString & "," & _
		cp.clDt.dViscosity(i).Tostring & "," & _
		cp.clDt.iHosei(i).ToString & ","

		sLineBuf = sLineBuf & _
		TempXp_A(i).ToString & "," & _
		TempYp_A(i).ToString & "," & _
		TempXp_Q(i).ToString & "," & _
		TempYp_Q(i).ToString & "," & _
		TempXp_BAve(i).ToString & "," & _
		TempYp_BAve(i).ToString & "," & _
		TempXY_D(i).ToString & "," & _
		TempXY_C(i).ToString & "," & _
		TempYp_D(i).ToString & "," & _
		TempYp_C(i).ToString & "," & _
		TempXY_Tc(i).ToString & "," & _
		TempXY_T1(i).ToString & "," & _
		TempXY_Ts(i).ToString & "," & _
		TempXY_T2(i).ToString & "," & _
		TempYp_Tc(i).ToString & "," & _
		TempYp_T1(i).ToString & "," & _
		TempYp_Ts(i).ToString & "," & _
		TempYp_T2(i).ToString & "," & _
		TempXY_Rad(i).ToString & "," & _
		TempYp_T3Ave(i).ToString & "," & _
		Temp_J100(i).ToString & ","


		sLineBuf = sLineBuf & _
		cp.clDt.dXPosPhase(i).ToString & "," & _
		cp.clDt.dYPosPhase(i).ToString & "," & _
		cp.clDt.dSplStrain(i).ToString & "," & _
		cp.clDt.dShearMod(i).ToString & "," & _
		cp.clDt.dShearVelo(i).ToString & "," & _
		cp.clDt.dXStrgMod(i).ToString & "," & _
		cp.clDt.dXElasMod(i).ToString & "," & _
		cp.clDt.dXLossTan(i).ToString & "," & _
		cp.clDt.dYStrgMod(i).ToString & "," & _
		cp.clDt.dYElasMod(i).ToString & "," & _
		cp.clDt.dYLossTan(i).ToString & "," & _
		Temp_G(i).ToString & "," & _
		Temp_GAve(i).ToString

 		swr.WriteLine(sLineBuf)
		dTime += 0.002
	Next i
	swr.Close()
	swr = nothing
End if
End if
#end if
	'debug.Print($"iHosei() = {clDt.iHosei(clDt.iHosei.GetUpperBound(0))}")
Sw.Stop()
#If 1
'使用メモリ監視
' 合計物理メモリ
Dim totalPhysicalMemoryByte As Decimal = CDec(My.Computer.Info.TotalPhysicalMemory)
Dim totalPhysicalMemoryKB As Decimal = totalPhysicalMemoryByte / 1024
Dim totalPhysicalMemoryMB As Decimal = totalPhysicalMemoryByte / 1024 / 1024
Dim totalPhysicalMemoryGB As Decimal = totalPhysicalMemoryByte / 1024 / 1024 / 1024

' 利用可能な物理メモリ
Dim availablePhysicalMemoryByte As Decimal = CDec(My.Computer.Info.AvailablePhysicalMemory)
Dim availablePhysicalMemoryKB As Decimal = availablePhysicalMemoryByte / 1024
Dim availablePhysicalMemoryMB As Decimal = availablePhysicalMemoryByte / 1024 / 1024
Dim availablePhysicalMemoryGB As Decimal = availablePhysicalMemoryByte / 1024 / 1024 / 1024

' メモリ使用量を計算
Dim usedPhysicalMemoryByte As Decimal = totalPhysicalMemoryByte - availablePhysicalMemoryByte
Dim usedPhysicalMemoryKB As Decimal = usedPhysicalMemoryByte / 1024
Dim usedPhysicalMemoryMB As Decimal = usedPhysicalMemoryByte / 1024 / 1024
Dim usedPhysicalMemoryGB As Decimal = usedPhysicalMemoryByte / 1024 / 1024 / 1024

'Debug.Print("Calc Elapsed(mS),{0:F0},T,{1:F0},A,{2:F0},U,{3:F0}", Sw.Elapsed.TotalMilliseconds,totalPhysicalMemoryMB,availablePhysicalMemoryMB,usedPhysicalMemoryMB)
#end if

	Return 0

Catch ex As OverflowException
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Return -1

Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Return -1

End Try
End Function

''' <summary>
''' 各種結果パラメータ計算 XOnly
''' </summary>
''' <returns>0:正常終了 -1:例外発生 1:サンプル数が満たない</returns>
Public Function CorrectRealProcVectorX( ByRef cp As stCalcPara ) As Int32
	'周期0の時は合成できないので仮に周期1秒として計算する
	If cp.dPeriod = 0
		cp.dPeriod = 1.0
	End If
	Dim iRet							As int32
	Dim	i								As int32
	Dim	j								As int32
	Dim idx								As int32
	Dim iAllPriod						As int32	= (cp.dPeriod * 1000 / 2 ) - 1			'測定周期サンプル数
	Dim iHalfPriod						As int32	= (cp.dPeriod * 1000 / 2 / 2) - 1		'測定半周期サンプル数
	Dim iQuotPriod						As int32	= (cp.dPeriod * 1000 / 2 / 2 / 2) - 1	'測定1/4周期サンプル数
	Dim dFreq							As Double	= 1.0 / cp.dPeriod						'測定周波数
	Dim dSampleTime						As Double	= 1.0 / MEAS_SAMPLE_FREQ				'測定サンプリング時間(S)
	Dim iArraySize						As Int64	= cp.iSampleNum - 1						'配列サイズ = 処理サンプル数(引数)

	'比率・内積・外積・位相角を計算しないサンプル数
	Dim iIgnoreCount					As Int64	= (MEAS_BLOCK_DATA_NUM * cp.dXYWait) + (iHalfPriod * 3)

	'AD値->mN変換係数
	Dim dDigToNX						As Double	= 1.0
	Dim dDigToNY						As Double	= 1.0

	Dim	TimeAry(iArraySize)				As Double										'時間配列

	Dim SumAryX()						As Double = Nothing								'積分法 積分値マージ用

	Dim SumAryPX()						As Double = Nothing								'積分法 積分値マージ用(位置)

	Dim	TempX_B(iArraySize)				As Double										'振幅列
	'Dim	TempX_BR(iArraySize)			As Double										'振幅列(比率補正済)
	Dim TempX_BAve(iArraySize)			As Double


	Dim	TempXp_B(iArraySize)			As Double										'振幅列(位置)
	Dim TempXp_BAve(iArraySize)			As Double										'X位置の振幅

	'XY中間値比率方法のためのエリア ----
	'Dim	Temp_BRxAve(iArraySize)			As Double										'振幅比率計算 X（半周期移動平均後）
	'----

	Dim	Temp_D(iArraySize)				As Double										'内積列
	Dim	Temp_C(iArraySize)				As Double										'外積列
	Dim	TempXp_D(iArraySize)			As Double										'内積列(位置)
	Dim	TempXp_C(iArraySize)			As Double										'外積列(位置)
	Dim	TempXp_DAve(iArraySize)			As Double										'内積列(位置)
	Dim	TempXp_CAve(iArraySize)			As Double										'外積列(位置)

	Dim	Temp_Ts(iArraySize)				As Double										'Sin列
	Dim	Temp_Tc(iArraySize)				As Double										'Cos列
	Dim	Temp_T2(iArraySize)				As Double										'ASin列
	Dim	Temp_T3(iArraySize)				As Double										'ACos列
	Dim	Temp_T3Ave(iArraySize)			As Double										'ACos列(半周期移動平均)

	'位置用
	Dim	TempXp_Ts(iArraySize)			As Double										'Sin列
	Dim	TempXp_Tc(iArraySize)			As Double										'Cos列
	Dim	TempXp_T1(iArraySize)			As Double										'ASin列
	Dim	TempXp_T2(iArraySize)			As Double										'ASin列(位相差)(位置)
	Dim	TempXp_T3(iArraySize)			As Double										'ACos列
	Dim	TempXp_T3Ave(iArraySize)		As Double										'ASin列(半周期移動平均)

	Dim	Temp_J(iArraySize)				As int64										'XY合成配列
	Dim	Temp_K(iArraySize)				As int64										'XY合成微分配列(SLOPE用)
	Dim	Temp_L(iArraySize)				As Double										'位相差補正 Y方向ベクトルのY成分（Xに直交する成分）配列
	Dim	Temp_M(iArraySize)				As Double										'位相差補正 Y方向ベクトルのX成分（Xに平行成分）配列
	Dim	Temp_N(iArraySize)				As Double										'位相差補正 Y方向ベクトルのX成分で補正したX方向ベクトル

	Dim	Temp_J100(iArraySize)			As Double										'XY合成配列100%換算値
	Dim Temp_Stn(iArraySize)			As Double										'サンプルに与えたひずみ量
	Dim Temp_SM(iArraySize)				As Double										'ずり弾性率

	Dim	Temp_V(iArraySize)				As Double										'粘度(PaS)
	Dim	Temp_SS(iArraySize)				As Double										'せん断応力
	Dim	Temp_SV(iArraySize)				As Double										'せん断速度

	Dim	TempXp_Rad(iArraySize)			As Double										'X動的粘弾性(Rad)
	Dim	TempXp_G1(iArraySize)			As Double										'X貯蔵弾性率(Pa)
	Dim	TempXp_G2(iArraySize)			As Double										'X損失弾性率(Pa)
	Dim	TempXp_Tn(iArraySize)			As Double										'X損失正接(deg)

	Dim AveAry()						As Double = Nothing								'平均算出用配列

	Dim iSmpEnd							As int64
	Dim fil								As New Filter									'フィルタクラス生成
	Dim	Coef()							As Double = Nothing								'FIRフィルタパラメータ(窓関数から生成)
	Dim dK								As Double = Math.PI * dSampleTime / cp.dPeriod	'PI * サンプリング時間 / 周期
	Dim dSumX							As Double 										'和を計算
	Dim dSumPX							As Double 										'和を計算(位置)
	Dim dNConv							As Double = p_dNConv * 1E+6						'AD値->uN変換係数
	Dim iBranch							As int16
	Dim dPPhase							As Double = 0

	Dim dCalibFullStress(iArraySize)	As Double										'バッチ 点モード時の補正されたフル応力値
	Dim dCalibFullPhase(iArraySize)		As Double										'バッチ 点モード時の補正されたフル位相値

Try
	'測定種別による分岐処理
	'測定種別によるXYウェイトタイムの比率、位相角計算無効
	Select Case cp.iMeas
	Case eHoseiMeasMode.GelTime		'ゲルタイム
		Select Case cp.iMode
		Case ehoseiCalcMode.Batch
			'iIgnoreCount = (MEAS_BLOCK_DATA_NUM * (cp.dXYWait+3))
			iIgnoreCount = (MEAS_BLOCK_DATA_NUM * (cp.dXYWait))
			iBranch = 0
		Case ehoseiCalcMode.RealTime
			'iIgnoreCount = (MEAS_BLOCK_DATA_NUM * (cp.dXYWait+3))
			iIgnoreCount = (MEAS_BLOCK_DATA_NUM * (cp.dXYWait))
			iBranch = 1
		End Select
	Case eHoseiMeasMode.FullCalib		'フルキャリブ
		Select Case cp.iMode
		Case ehoseiCalcMode.Batch
			iIgnoreCount = -1
			iBranch = 0
		Case ehoseiCalcMode.RealTime
			iIgnoreCount = -1
			iBranch = 1
		End Select
	Case eHoseiMeasMode.ZeroCalib		'ゼロキャリブ
		Select Case cp.iMode
		Case ehoseiCalcMode.Batch
			iIgnoreCount = -1
			iBranch = 0
		Case ehoseiCalcMode.RealTime
			iIgnoreCount = -1
			iBranch = 1
		End Select
	Case eHoseiMeasMode.FlowCurve		'フローカーブ
		Select Case cp.iMode
		Case ehoseiCalcMode.Batch
			iIgnoreCount = -1
			iBranch = 0
		Case ehoseiCalcMode.RealTime
			iIgnoreCount = -1
			iBranch = 1
		End Select
	Case Else
		iIgnoreCount = -1
		iBranch = 0
	End Select

	iSmpEnd = cp.iSampleNum - 1						'処理最終インデックス
	dInputPos = cp.iZeroPos - (cp.cts.InputGap * 1000)
	'If cp.iMeas = ehoseiCalcMode.RealTime
	If cp.iMode = ehoseiCalcMode.RealTime
		iLatSampleNum += MEAS_SAMPLE_FREQ
		For i = iLatSampleNum - MEAS_SAMPLE_FREQ To iLatSampleNum - 1
			'ゲルタイム測定・点モード・バッチ処理のみ
			'Z軸位置データを変換する 振動位置に達したらゲイン1/10 
			Select Case cp.iMeas
			Case eHoseiMeasMode.GelTime		'ゲルタイム
				Select Case cp.iMode
				Case ehoseiCalcMode.RealTime
					If cp.clDt.iPzBuf(i) = 0 AndAlso iLatPrePzBuf > 0 AndAlso iLatSign = 0
						iLatSign = 1
						iLatPrePzBuf += dInputPos
					End If
					If iLatSign = 1
						'着圧決定の振動位置を見つけた
						cp.clDt.iPzBuf(i) = (cp.clDt.iPzBuf(i) / 10) + iLatPrePzBuf			'Gain 1/10 にして 落差を足す
					Else
						iLatPrePzBuf = cp.clDt.iPzBuf(i)
						cp.clDt.iPzBuf(i) = cp.clDt.iPzBuf(i) + dInputPos					'投入位置を足す
					End If  
				Case Else
				End Select
			Case Else
			End Select
		Next
	Else
		iLatSampleNum = 0
	End If

	'1周期分のサンプル数が無い場合は何もしない
	If cp.iSampleNum <= iAllPriod 
		Return 1
	End If
	For i = 0 To iSmpEnd
		'時刻配列の作成
		TimeAry(i) = i * MEAS_TIME_RESOLUTION / MEAS_TIME_BLOCK
	Next

	'一括処理の場合は読み取りインデックスをゼロクリア
	If iBranch = 0
		m_iOXBuf_R_pt	= 0
		m_TempX_1_R_pt	= 0
		m_TempX_A_R_pt	= 0
	End If
	

	'微分を行う TempX_1, TempY_1---------------------------------------------------
	If IsNothing(TempX_1) Or iBranch = 0
		Array.Resize(TempX_1, cp.iSampleNum)							'配列拡張
		Array.Resize(TempXp_1, cp.iSampleNum)							'配列拡張
	Else
		Array.Resize(TempX_1, TempX_1.Length + cp.iSampleNum)			'配列拡張
		Array.Resize(TempXp_1, TempXp_1.Length + cp.iSampleNum)			'配列拡張
	End If

	'AD値->uN変換止めて、AD値のまま)
#If 1
	Select Case cp.iStrsSensMode
	Case clSpec.eStrsSens.Point
	'X成分領域へZ成分に置き換えて計算
		Array.Resize(TempX_0, cp.clDt.iOzBuf.Length)							'配列確保
		Array.Copy(cp.clDt.iOzBuf, TempX_0, cp.clDt.iOzBuf.Length)

		Array.Resize(TempXp_0, cp.clDt.iPzBuf.Length)							'配列確保
		Array.Copy(cp.clDt.iPzBuf, TempXp_0, cp.clDt.iPzBuf.Length)
		dPPhase = cp.dPzPhase
		dNConv  = cp.cts.WeightCoefZ * p_cuNperGram								'uN/dig
		'dNConv  = 15.164								'uN/dig
	Case Else
		Array.Resize(TempX_0, cp.clDt.iOxBuf.Length)							'配列確保
		Array.Copy(cp.clDt.iOxBuf, TempX_0, cp.clDt.iOxBuf.Length)

		Array.Resize(TempXp_0, cp.clDt.iPxBuf.Length)							'配列確保
		Array.Copy(cp.clDt.iPxBuf, TempXp_0, cp.clDt.iPxBuf.Length)
		dPPhase = cp.dPxPhase
	End Select
#else
	Select Case cp.iStrsSensMode
	Case clSpec.eStrsSens.Point
		Array.Resize(TempX_0, cp.clDt.iOxBuf.Length)							'配列確保
		Array.Copy(cp.clDt.iOxBuf, TempX_0, cp.clDt.iOxBuf.Length)

		Array.Resize(TempXp_0, cp.clDt.iPxBuf.Length)							'配列確保
		Array.Copy(cp.clDt.iPxBuf, TempXp_0, cp.clDt.iPxBuf.Length)
		dPPhase = cp.dPxPhase
	Case Else


	'X成分領域へZ成分に置き換えて計算
		Array.Resize(TempX_0, cp.clDt.iOzBuf.Length)							'配列確保
		Array.Copy(cp.clDt.iOzBuf, TempX_0, cp.clDt.iOzBuf.Length)

		Array.Resize(TempXp_0, cp.clDt.iPzBuf.Length)							'配列確保
		Array.Copy(cp.clDt.iPzBuf, TempXp_0, cp.clDt.iPzBuf.Length)
		dPPhase = cp.dPzPhase
		dNConv  = cp.cts.WeightCoefZ * p_cuNperGram								'uN/dig
		'dNConv  = 15.164								'uN/dig
	End Select
#End if
	'微分
	idx = 1
	For i = m_iOXBuf_R_pt + 1 To m_iOXBuf_R_pt + cp.iSampleNum - 1
		'演算式 ((X2 - X1) / (T2 - T1)) / (2 * Math.pi * dfreq )
		TempX_1(i) = DifferentialProc(TempX_0(i - 1), TempX_0(i), TimeAry(idx - 1), TimeAry(idx), dFreq ) 
		TempXp_1(i) = DifferentialProc(TempXp_0(i - 1), TempXp_0(i), TimeAry(idx - 1), TimeAry(idx), dFreq ) 
		idx += 1
	Next

	m_iOXBuf_R_pt += cp.iSampleNum										'TempX_1の読み出しポイント更新


	'一回目 積分を行う 区間は半周期で1点ずつ移動して和を計算  TempX_A, TempY_A ---------------------------------------------------
	If IsNothing(TempX_A) Or iBranch = 0
		Array.Resize(TempX_A, cp.iSampleNum)							'配列拡張
		'Array.Resize(TempX_AR, cp.iSampleNum)							'配列拡張
		Array.Resize(TempXp_A, cp.iSampleNum)							'配列拡張
	Else
		Array.Resize(TempX_A, TempX_A.Length + cp.iSampleNum)			'配列拡張
		'Array.Resize(TempX_AR, TempX_AR.Length + cp.iSampleNum)			'配列拡張
		Array.Resize(TempXp_A, TempXp_A.Length + cp.iSampleNum)			'配列拡張

	End if
	'微分配列 半周期分の合計
	SumAryX = Nothing
	Array.Resize(SumAryX, iHalfPriod + 1)				'半周期分の加算エリア確保

	SumAryPX = Nothing
	Array.Resize(SumAryPX, iHalfPriod + 1)				'半周期分の加算エリア確保

	For idx = m_TempX_1_R_pt To m_TempX_1_R_pt + (cp.iSampleNum - 1)
		If idx - iHalfPriod >= 0
		'半周期先頭読み出しポイント
			Array.Copy(TempX_1, idx - iHalfPriod, SumAryX, 0, SumAryX.Length)			'SUM用配列へデータコピー
			dSumX = SumAryX.Sum()                             '和を計算
			TempX_A( idx ) = dSumX * dK									'和 * PI * サンプリング時間 / 周期

			Array.Copy(TempXp_1, idx - iHalfPriod, SumAryPX, 0, SumAryPX.Length)			'SUM用配列へデータコピー
			dSumPX = SumAryPX.Sum()                             '和を計算
			TempXp_A( idx ) = dSumPX * dK									'和 * PI * サンプリング時間 / 周期
		Else
			'無効データ 半周期積分元のデータが無いため
			TempX_A( idx ) = Double.NaN
			TempXp_A( idx ) = Double.NaN
		End if

	Next idx
	m_TempX_1_R_pt += cp.iSampleNum										'TempX_1の読み出しポイント更新

	'2回目 積分を行う 区間は半周期で1点ずつ移動して和を計算  TempX_Q, TempY_Q ---------------------------------------------------
	If IsNothing(TempX_Q) Or iBranch = 0
		Array.Resize(TempX_Q, cp.iSampleNum)							'配列拡張
		'Array.Resize(TempX_QR, cp.iSampleNum)							'配列拡張
		Array.Resize(TempX_QM, cp.iSampleNum)							'配列拡張
		Array.Resize(TempXp_Q, cp.iSampleNum)							'配列拡張
	Else
		Array.Resize(TempX_Q, TempX_Q.Length + cp.iSampleNum)			'配列拡張
		'Array.Resize(TempX_QR, TempX_QR.Length + cp.iSampleNum)		'配列拡張
		Array.Resize(TempX_QM, TempX_QM.Length + cp.iSampleNum)			'配列拡張
		Array.Resize(TempXp_Q, TempXp_Q.Length + cp.iSampleNum)			'配列拡張
	End if
	'微分配列 半周期分の合計
	SumAryX = Nothing
	SumAryPX = Nothing
	Array.Resize(SumAryX, iHalfPriod + 1)				'半周期分の加算エリア確保
	Array.Resize(SumAryPX, iHalfPriod + 1)				'半周期分の加算エリア確保

	For idx = m_TempX_A_R_pt To m_TempX_A_R_pt + (cp.iSampleNum - 1)
		If idx - iAllPriod >= 0
		'半周期先頭読み出しポイント
			Array.Copy(TempX_A, idx - iHalfPriod, SumAryX, 0, SumAryX.Length)			'SUM用配列へデータコピー
			dSumX = SumAryX.Sum()                             '和を計算
			TempX_Q( idx ) = dSumX * dK									'和 * PI * サンプリング時間 / 周期

			Array.Copy(TempXp_A, idx - iHalfPriod, SumAryPX, 0, SumAryPX.Length)			'SUM用配列へデータコピー
			dSumPX = SumAryPX.Sum()                            '和を計算
			TempXp_Q( idx ) = dSumPX * dK								'和 * PI * サンプリング時間 / 周期

		Else
			'無効データ 半周期積分元のデータが無いため
			TempX_Q( idx )	= Double.NaN
			TempXp_Q( idx ) = Double.NaN
		End if
	Next idx

	'波形モニタ用
	Array.Copy(TempX_Q, TempX_QM, TempX_Q.Length)

	'粘度関係計算 条件値
	Dim uVp As Datafile.ViscosityCalcParameter
	uVp.dPeriod			= cp.dPeriod										'設定周期(S)
	uVp.dAmplitude		= cp.dAmplitude / 10^3								'設定振幅(m)
	uVp.dMeasGap		= cp.dMeasGap / 10^3								'測定GAP(m)
	uVp.dProbeSquare	= Math.PI * ((cp.dDiameter/10^3) / 2)^2				'プローブ面積(m2)


	'-------------------------------------------------------------------------------------------------
	Select Case iBranch
	Case 0 			'GelTime最終測定、キャリブレーション
		'Array.Resize(Temp_BRx,iArraySize+1)

		For i = 0  To iSmpEnd
			'振幅を計算 TempX_B, TempY_B---------------------------------------------------
			TempX_B(i)	= Math.Sqrt( TempX_A(i)^2 + TempX_Q(i)^2 )
			TempXp_B(i) = Math.Sqrt( TempXp_A(i)^2 + TempXp_Q(i)^2 )
			'TempX_BR(i) = TempX_B(i)
		Next i





		'BatchMovingAverage(TempX_B,		TempX_BAve,		iHalfPriod)

		Dim dUpperdeg As Double = Math.PI / 2 + 0.160	'90deg
		Dim dLowerdeg As Double = Math.PI / 2 - 0.160	'80deg
		Array.Resize(TempXp_T2,Temp_T2.Length)
		Dim dPreCalibFullStress As Double = cp.dFullStress
		Dim dPreCalibFullPhase	As Double = dPPhase
		For i = 0  To iSmpEnd
			If i > iIgnoreCount + iAllPriod
			'内積/外積を計算 Temp_D, Temp_C --------------------------------------------------
				TempXp_D(i) = If(TempXp_Q(i)=0, 0, TempXp_A(i) * TempX_A(i) + TempXp_Q(i) * TempX_Q(i))			'内積(位置)
				TempXp_C(i) = If(TempXp_Q(i)=0, 0, TempXp_A(i) * TempX_Q(i) - TempX_A(i) * TempXp_Q(i))			'外積(位置)

				'TempXp_BAve(振幅 um 0-p)からキャリブレーションテーブルをサーチしてフル応力値とフル位相値を得る
				Select Case cp.iMeas
				Case eHoseiMeasMode.FullCalib, eHoseiMeasMode.ZeroCalib, eHoseiMeasMode.FlowCurve
				Case Else
					If cp.iStrsSensMode = clSpec.eStrsSens.Point AndAlso cp.iAmpMode = clSpec.eAmpMode.StsConst
					'点モード かつ 応力一定
						Dim dXp_B As Double = TempXp_BAve(i) * 2 / 1000.0 
						'キャリブレーション値を直線補完で再取得
						iRet = CalibSearchPointStressConstMode(cp, dXp_B, dCalibFullStress(i), dCalibFullPhase(i))
						If iRet<0
						'エラー発生
							dCalibFullStress(i)	= cp.dFullStress
							dCalibFullPhase(i)	= dPPhase
						Else
						End If
						If dCalibFullStress(i) = 0
						'フル応力値0を探し当ててしまった場合は最後に使用した値を使用
							dCalibFullStress(i)	= dPreCalibFullStress
						Else
							dPreCalibFullStress = dCalibFullStress(i)			'最後に使用した値を退避
						End If
						If dCalibFullPhase(i) = 0
						'フル位相値0を探し当ててしまった場合は最後に使用した値を使用
							dCalibFullPhase(i)	= dPreCalibFullPhase
						Else
							dPreCalibFullPhase = dCalibFullPhase(i)				'最後に使用した値を退避
						End If
					Else
					'線モード または 点モードかつ振幅一定
						dCalibFullStress(i)	= cp.dFullStress
						dCalibFullPhase(i)	= dPPhase
					End If
				End Select
			Else
			'XY動作するまでは計算は行わない
				TempX_B(i)	= Double.NaN
				TempXp_B(i) = Double.NaN
				TempX_BAve(i) = Double.NaN
				TempXp_D(i) = Double.NaN
				TempXp_C(i) = Double.NaN
			End If

		Next i

		BatchMovingAverage(TempX_B,			TempX_BAve,			iAllPriod)
		BatchMovingAverage(TempXp_B,		TempXp_BAve,		iAllPriod)
		BatchMovingAverage(TempXp_D,		TempXp_DAve,		iAllPriod)
		BatchMovingAverage(TempXp_C,		TempXp_CAve,		iAllPriod)


		For i = 0  To iSmpEnd
			'X:Y位相差はない

			'X:Xpos位相差θを算出（位置）Zも共通 --------------------------------------------------
			'If Double.IsNaN(TempX_BR(i)) Or Double.IsNaN(TempXp_B(i))
			If Double.IsNaN(TempX_B(i)) Or Double.IsNaN(TempXp_B(i))
				TempXp_Tc(i) = Double.NaN											'Cosを演算(位置)
				TempXp_Ts(i) = Double.NaN											'Sinを演算(位置)
			Else
				'TempXp_Tc(i) = TempXp_DAve(i) / (TempX_BAve(i) * TempXp_BAve(i))			'Cosを演算
				'TempXp_Ts(i) = TempXp_CAve(i) / (TempX_BAve(i) * TempXp_BAve(i))			'Sinを演算
				TempXp_Tc(i) = TempXp_D(i) / (TempX_B(i) * TempXp_B(i))			'Cosを演算
				TempXp_Ts(i) = TempXp_C(i) / (TempX_B(i) * TempXp_B(i))			'Sinを演算
			End If
			TempXp_T1(i) = Math.Acos( TempXp_Tc(i) )							'θ算出(位置)
			If TempXp_T1(i) >= Math.PI / 2
				TempXp_T2(i) = TempXp_T1(i)
			Else
				TempXp_T2(i) = Math.Asin( TempXp_Ts(i) )						'θ算出(位置)
			End If

			'Y:Yp位相差はない

		Next i


		'X位相角の半周期移動平均
		BatchMovingAverage(TempXp_T2, TempXp_T3Ave, iAllPriod)

		'キャリブレーション、測定------------------------------------------------------------------------------------------
		Dim dXCos		As Double
		Dim dXSin		As Double
		Dim dRTemp_J100 As Double
		For i = 0  To iSmpEnd

		'XYを合成 --------------------------------------------------
			If Not Double.IsNaN(TempX_BAve(i))
				Temp_J(i) = Ctype(TempX_BAve(i), Int64)
				Select Case cp.iMeas
				Case eHoseiMeasMode.FullCalib, eHoseiMeasMode.ZeroCalib
					Temp_J(i) = Temp_J(i) * dNConv												'μN変換
				Case Else
					Temp_J(i) = ((Temp_J(i) * dNConv) - cp.dZeroStress) * cp.dAttenuation		'μN変換
					''Temp_J100(i) = Temp_J(i) / cp.dFullStress
					If dCalibFullStress(i)<>0
						Temp_J100(i) = Temp_J(i) / dCalibFullStress(i)
					Else
						Temp_J100(i) = 0
					End If
				End Select
			End if
			Temp_J(i) = if( Temp_J(i)<0, 0, Temp_J(i) )									'負数はゼロにする

			Select Case cp.iMeas
			Case eHoseiMeasMode.GelTime, eHoseiMeasMode.FlowCurve		'ゲルタイム,フローカーブ


				'TempXp_T3Aveの移動平均時定数分でひげが発生する　要対策
				''TempXp_Rad(i) = If(Double.IsNaN(TempXp_Ts(i)), Double.NaN, TempXp_T3Ave(i) - dPPhase)		'動粘弾位相オフセット補正		
				TempXp_Rad(i) = If(Double.IsNaN(TempXp_Ts(i)), Double.NaN, TempXp_T3Ave(i) - dCalibFullPhase(i))		'動粘弾位相オフセット補正		
				TempXp_Rad(i) = If(TempXp_Rad(i) < 0, 0, TempXp_Rad(i))										'負数は０

				dXSin   = Math.Sin(TempXp_Rad(i))
				If TempXp_Rad(i) >= Math.PI / 2
					dXCos	= Math.Cos(Math.PI / 2)
				Else
					dXCos	= If(dXSin = 1, 0, Math.Cos(TempXp_Rad(i)))
				End If
				dRTemp_J100 = 1 - (Temp_J100(i) * dXCos)								'貯蔵弾性率成分で振幅が小さくなる


				'粘度計算
				uVp.dMeasStress		= Temp_J(i) / 10^6										'測定応力(N)
				uVp.dVelocityRate	= dRTemp_J100											'せん断速度補正　貯蔵弾性率に応じて可変する

				Select Case cp.iHPPosControl
				Case 1		'熱板着圧制御
					Dim dZStrsAD As Int64 = 0
					dZStrsAD = CalcMeasureStrsToADValue(cp.dMeasureStress, cp.dDiameter, cp.cts.WeightCoefZ)
					uVp.dMeasGap		= (cp.iZeroPos - cp.dRCCBaseThickness - p_dtFile.iPzBuf(i) + (1/cp.cts.GapCorr) * dZStrsAD) / 10^6			'測定GAP(m)
				Case Else
					uVp.dMeasGap		= (cp.iZeroPos - cp.dRCCBaseThickness - p_dtFile.iPzBuf(i)) / 10^6											'測定GAP(m)
				End Select
				cp.dMesGap = uVp.dMeasGap * 10^3											'モニタ用測定GAP(mm)
				IF Double.isNaN(TempXp_BAve(i)) Or TempXp_BAve(i) <= 0
					uVp.dAmplitude		= 0													'設定振幅(m)
				Else
					''uVp.dAmplitude		= Math.Round(TempXp_BAve(i) * 2 / 10, 0) / 10^6		'設定振幅(m)
					Select Case cp.dAmplitude * 1000.0
					Case <= iSwitchAmpXYPos
					'0.1um解像度
						uVp.dAmplitude		= Math.Round(TempXp_BAve(i) * 2 / 10, 0) / 10^6		'設定振幅(m)
					Case Else
					'1um解像度
						uVp.dAmplitude		= TempXp_BAve(i) * 2 / 10^6							'設定振幅(m)
					End Select
				End If

				ViscosityCalc( uVp )														'粘度計算処理
				Temp_V(i) = uVp.dViscosity
				Temp_SS(i) = uVp.dShearStress												'せん断応力
				Temp_SV(i) = uVp.dShearVelocity												'せん断速度

				If uVp.dMeasGap > 0 And uVp.dAmplitude <> 0
					Temp_Stn(i) =  ((uVp.dAmplitude * dRTemp_J100) / 2) / uVp.dMeasGap		'サンプルに与えたひずみ
					Temp_SM(i) = Temp_SS(i) / Temp_Stn(i)									'ずり弾性率G*
				Else
					Temp_Stn(i) = 0
					Temp_SM(i) = 0	
				End If
				'動的粘弾性'振幅0時はゼロ
				TempXp_G1(i) = If(cp.dAmplitude <> 0, Temp_SM(i) * dXCos, 0)					'X貯蔵弾性率
				TempXp_G2(i) = If(cp.dAmplitude <> 0, Temp_SM(i) * dXSin, 0)					'X損失弾性率
				'TempXp_Tn(i) = If(TempXp_G1(i) = 0, _
				'									Double.NaN, TempXp_G2(i) / TempXp_G1(i))		'X損失正接
				If TempXp_Rad(i) >= Math.PI / 2
					TempXp_Tn(i) = Double.NaN
				Else
					TempXp_Tn(i) = Math.Tan(TempXp_Rad(i))										'X損失正接
				End If
				TempXp_Tn(i) = If(TempXp_Tn(i)< 0, 0, TempXp_Tn(i))							'X損失正接(負数は０)

			Case eHoseiMeasMode.FullCalib, eHoseiMeasMode.ZeroCalib							'キャリブレーション
				'動的粘弾性	'振幅0時はゼロ
				TempXp_Rad(i) = If(cp.dAmplitude <> 0, TempXp_T3Ave(i), 0)
			End Select
			Temp_J100(i) *= 100.0
		Next i


		Dim iLen As Int64 = cp.iSampleNum - iQuotPriod
		Dim iSrt As Int64 = 0
		Select Case cp.iMeas
		Case eHoseiMeasMode.GelTime		'ゲルタイム
			'処理済 補正データをセット （積分による遅れ分を(1/4周期)前シフト） ---------------------------------------------
			iLen = cp.iSampleNum - iQuotPriod
			iSrt = iQuotPriod
		Case Else
			iLen = cp.iSampleNum
			iSrt = 0
		End Select

		Array.Copy(Temp_J,		iSrt, cp.clDt.iHosei,		0,	iLen)		'応力値
		Array.Copy(Temp_J100,	iSrt, cp.clDt.dHosei100,	0,	iLen)		'応力値%
		Array.Copy(Temp_V,		iSrt, cp.clDt.dViscosity,	0,	iLen)		'粘度
		Array.Copy(TempXp_Rad,	iSrt, cp.clDt.dXPosPhase,	0,	iLen)		'X位相差
		Array.Copy(TempXp_G1,	iSrt, cp.clDt.dXStrgMod,	0,	iLen)		'X貯蔵弾性率
		Array.Copy(TempXp_G2,	iSrt, cp.clDt.dXElasMod,	0,	iLen)		'X損失弾性率
		Array.Copy(TempXp_Tn,	iSrt, cp.clDt.dXLossTan,	0,	iLen)		'X損失正接
		Array.Copy(Temp_Stn,	iSrt, cp.clDt.dSplStrain,	0,	iLen)		'サンプルに与えたひずみ
		Array.Copy(Temp_SM,		iSrt, cp.clDt.dShearMod,	0,	iLen)		'ずり弾性率
		Array.Copy(Temp_SV,		iSrt, cp.clDt.dShearVelo,	0,	iLen)		'せん断速度

		Select Case cp.iMeas
		Case eHoseiMeasMode.GelTime		'ゲルタイム
			'前シフトした分最終部分を埋める
			For i=cp.clDt.iHosei.Length-iSrt To cp.clDt.iHosei.GetUpperBound(0)
				cp.clDt.iHosei(i)		= cp.clDt.iHosei(i-1)
				cp.clDt.dHosei100(i)	= cp.clDt.dHosei100(i-1)
				cp.clDt.dViscosity(i)	= cp.clDt.dViscosity(i-1)
				cp.clDt.dXPosPhase(i)	= cp.clDt.dXPosPhase(i-1)
				cp.clDt.dXStrgMod(i)	= cp.clDt.dXStrgMod(i-1)
				cp.clDt.dXElasMod(i)	= cp.clDt.dXElasMod(i-1)
				cp.clDt.dXLossTan(i)	= cp.clDt.dXLossTan(i-1)
				cp.clDt.dYPosPhase(i)	= cp.clDt.dYPosPhase(i-1)
				cp.clDt.dYStrgMod(i)	= cp.clDt.dYStrgMod(i-1)
				cp.clDt.dYElasMod(i)	= cp.clDt.dYElasMod(i-1)
				cp.clDt.dYLossTan(i)	= cp.clDt.dYLossTan(i-1)
				cp.clDt.dSplStrain(i)	= cp.clDt.dSplStrain(i-1)
				cp.clDt.dShearMod(i)	= cp.clDt.dShearMod(i-1)
				cp.clDt.dShearVelo(i)	= cp.clDt.dShearVelo(i-1)
			Next
		Case eHoseiMeasMode.FlowCurve		'フローカーブ
		Case Else
		End Select

	Case 1	'GelTime測定リアルタイム
		j = m_TempX_A_R_pt
		'Array.Resize(Temp_BRx,iSmpEnd+1)

		'-------------------------------------------------------------------------------------------------
		For i = 0  To iSmpEnd
			If j > iIgnoreCount
				'振幅を計算 TempX_B, TempY_B---------------------------------------------------
				TempX_B(i)	= Math.Sqrt( TempX_A(j)^2 + TempX_Q(j)^2 )
				TempXp_B(i) = Math.Sqrt( TempXp_A(j)^2 + TempXp_Q(j)^2 )
				'TempX_BR(i) = Math.Sqrt( TempX_A(j)^2 + TempX_Q(j)^2 )


				'内積/外積を計算 Temp_D, Temp_C --------------------------------------------------
				TempXp_D(i) = If(TempXp_Q(j)=0, 0, TempXp_A(j) * TempX_A(j) + TempXp_Q(j) * TempX_Q(j))			'内積(位置)
				TempXp_C(i) = If(TempXp_Q(j)=0, 0, TempXp_A(j) * TempX_Q(j) - TempX_A(j) * TempXp_Q(j))			'外積(位置)


				'X位相差θを算出（位置） --------------------------------------------------
				'If Double.IsNaN(TempX_BR(i)) Or Double.IsNaN(TempXp_B(i))
				If Double.IsNaN(TempX_B(i)) Or Double.IsNaN(TempXp_B(i)) Or cp.dAmplitude = 0
					TempXp_Tc(i) = Double.NaN										'Cosを演算(位置)
					TempXp_Ts(i) = Double.NaN										'Sinを演算(位置)
				Else
					'TempXp_Tc(i) = TempXp_D(i) / (TempX_BR(i) * TempXp_B(i))		'Cosを演算
					'TempXp_Ts(i) = TempXp_C(i) / (TempX_BR(i) * TempXp_B(i))		'Sinを演算
					TempXp_Tc(i) = TempXp_D(i) / (TempX_B(i) * TempXp_B(i))			'Cosを演算
					TempXp_Ts(i) = TempXp_C(i) / (TempX_B(i) * TempXp_B(i))			'Sinを演算
				End If
				TempXp_T1(i) = Math.Acos( TempXp_Tc(i) )							'θ算出(位置)
				TempXp_T2(i) = Math.Asin( TempXp_Ts(i) )							'θ算出(位置)
				TempXp_Rad(i) = TempXp_T2(i) - dPPhase								'位相差をキャリブ値でオフセット補正
			Else
			'XY動作するまでは計算は行わない
				TempX_B(i)	= Double.NaN
				TempXp_B(i) = Double.NaN
				'TempX_BR(i) = Double.NaN
				TempXp_D(i) = Double.NaN
				TempXp_C(i) = Double.NaN
			End If
				
			j += 1
		Next i
		'Dim Temp_T3_Ave As Double = Temp_T3.Average()								'位相を平均化

		j = m_TempX_A_R_pt
		'-------------------------------------------------------------------------------------------------
		Dim dXCos			As Double 
		Dim dXSin			As Double
		Dim dRTemp_J100 	As Double

		For i = 0  To iSmpEnd
			dXSin		= Math.Sin(TempXp_Rad(i))
			dXCos		= If(dXSin = 1, 0, Math.Cos(TempXp_Rad(i)))

		'XYを合成 --------------------------------------------------
			'Temp_J(i) = TempX_BR(i)
			'If Not Double.IsNaN(TempXp_Rad(i))
			If Not Double.IsNaN(TempX_B(i))
				Temp_J(i) = TempX_B(i)
				Select Case cp.iMeas
				Case eHoseiMeasMode.FullCalib, eHoseiMeasMode.ZeroCalib
					Temp_J(i) = Temp_J(i) * dNConv											'μN変換
				Case Else
					Temp_J(i)	= (Temp_J(i) * dNConv - cp.dZeroStress) * cp.dAttenuation 	'μN変換
					If cp.dFullStress<>0
						Temp_J100(i)= Temp_J(i) / cp.dFullStress								'フルストレス（100%応力）に対する現在の応力割合
					Else
						Temp_J100(i)= 0
					End If
				End Select
			End If
			Temp_J(i) = if( Temp_J(i)<0, 0, Temp_J(i) )									'負数はゼロにする
			If Not Double.IsNaN(TempXp_Rad(i))
				dRTemp_J100 = 1 - (Temp_J100(i) * dXCos)
			Else
				dRTemp_J100 = 1
			End If

		'粘度計算
			uVp.dMeasStress		= Temp_J(i) / 10^6									'測定応力(N)
			uVp.dVelocityRate	= dRTemp_J100										'せん断速度補正
			Select Case cp.iHPPosControl
			Case 1		'熱板着圧制御
				Dim dZStrsAD As Int64 = 0
				dZStrsAD = CalcMeasureStrsToADValue(cp.dMeasureStress, cp.dDiameter, cp.cts.WeightCoefZ)
				uVp.dMeasGap		= (cp.iZeroPos - cp.dRCCBaseThickness - p_dtFile.iPzBuf(j) + (1/cp.cts.GapCorr) * dZStrsAD) / 10^6			'測定GAP(m)
			Case Else
				uVp.dMeasGap		= (cp.iZeroPos - cp.dRCCBaseThickness - p_dtFile.iPzBuf(j)) / 10^6				'測定GAP(m)
			End Select
			cp.dMesGap = uVp.dMeasGap * 10^3											'モニタ用測定GAP(mm)
			IF Double.isNaN(TempXp_B(i)) Or TempXp_B(i) <= 0
				uVp.dAmplitude		= 0													'設定振幅(m)
			Else
				''uVp.dAmplitude		= Math.Round(TempXp_B(i) * 2 / 10, 0) / 10^6		'設定振幅(m)
				Select Case cp.dAmplitude * 1000.0
				Case <= iSwitchAmpXYPos
				'0.1um解像度
					uVp.dAmplitude		= Math.Round(TempXp_B(i) * 2 / 10, 0) / 10^6		'設定振幅(m)
				Case Else
				'1um解像度
					uVp.dAmplitude		= TempXp_B(i) * 2 / 10^6							'設定振幅(m)
				End Select
			End If

			ViscosityCalc( uVp )													'粘度計算処理
			Temp_V(i) = uVp.dViscosity
			Temp_SS(i) = uVp.dShearStress											'せん断応力
			Temp_SV(i) = uVp.dShearVelocity											'せん断速度

			If uVp.dMeasGap > 0 And uVp.dAmplitude <> 0
				Temp_Stn(i) =  ((uVp.dAmplitude * dRTemp_J100) / 2) / uVp.dMeasGap 	'サンプルに与えたひずみ
				Temp_SM(i)	= Temp_SS(i) / Temp_Stn(i)								'ずり弾性率G*
			Else
				Temp_Stn(i) = 0
				Temp_SM(i)	= 0	
			End If

			'動的粘弾性'振幅0時はゼロ
			TempXp_G1(i) = If(cp.dAmplitude <> 0, Temp_SM(i) * dXCos, 0)			'X貯蔵弾性率
			TempXp_G2(i) = If(cp.dAmplitude <> 0, Temp_SM(i) * dXSin, 0)			'X損失弾性率
			'TempXp_Tn(i) = If(TempXp_G1(i) = 0, _
			'									Double.NaN, TempXp_G2(i) / TempXp_G1(i))		'X損失正接
			
			TempXp_Tn(i) = Math.Tan(TempXp_Rad(i))										'X損失正接
			TempXp_Tn(i) = If(TempXp_Tn(i)< 0, 0, TempXp_Tn(i))							'X損失正接(負数は０)
			Temp_J100(i) *= 100.0

			j += 1
		Next i

		m_TempX_A_R_pt += cp.iSampleNum										'TempX_1の読み出しポイント更新


	'処理済 補正データを追加 ---------------------------------------------------------------------------------------
		Array.Copy(Temp_J,		0, cp.clDt.iHosei,		cp.clDt.iHosei.Length-cp.iSampleNum,		cp.iSampleNum)	'応力値
		Array.Copy(Temp_J100,	0, cp.clDt.dHosei100,	cp.clDt.dHosei100.Length-cp.iSampleNum,	cp.iSampleNum)		'応力値%
		Array.Copy(Temp_V,		0, cp.clDt.dViscosity, cp.clDt.dViscosity.Length-cp.iSampleNum,	cp.iSampleNum)		'粘度
		Array.Copy(TempXp_Rad,	0, cp.clDt.dXPosPhase, cp.clDt.dXPosPhase.Length-cp.iSampleNum,	cp.iSampleNum)		'X位相差
		Array.Copy(Temp_Stn,	0, cp.clDt.dSplStrain, cp.clDt.dSplStrain.Length-cp.iSampleNum,	cp.iSampleNum)		'サンプルに与えたひずみ
		Array.Copy(Temp_SM,		0, cp.clDt.dShearMod,	cp.clDt.dShearMod.Length-cp.iSampleNum,	cp.iSampleNum)		'ずり弾性率
		Array.Copy(Temp_SV,		0, cp.clDt.dShearVelo,	cp.clDt.dShearVelo.Length-cp.iSampleNum,	cp.iSampleNum)	'剪断速度
		Array.Copy(TempXp_G1,	0, cp.clDt.dXStrgMod,	cp.clDt.dXStrgMod.Length-cp.iSampleNum,	cp.iSampleNum)		'X貯蔵弾性率
		Array.Copy(TempXp_G2,	0, cp.clDt.dXElasMod,	cp.clDt.dXElasMod.Length-cp.iSampleNum,	cp.iSampleNum)		'X損失弾性率
		Array.Copy(TempXp_Tn,	0, cp.clDt.dXLossTan,	cp.clDt.dXLossTan.Length-cp.iSampleNum,	cp.iSampleNum)		'X損失正接

		'リアルタイム数値モニタ表示用(frmMain)
		cp.clDt.p_curStress	= cp.clDt.iHosei(cp.clDt.iHosei.Length-cp.iSampleNum)									'応力値
		cp.clDt.p_curStressP	= cp.clDt.dHosei100(cp.clDt.dHosei100.Length-cp.iSampleNum)							'応力値%
		cp.clDt.p_curViscosity	= cp.clDt.dViscosity(cp.clDt.dViscosity.Length-cp.iSampleNum)						'粘度
		cp.clDt.p_curPhase		= cp.clDt.dXPosPhase(cp.clDt.dXPosPhase.Length-cp.iSampleNum)						'X位相差
		cp.clDt.p_curSplStrain	= cp.clDt.dSplStrain(cp.clDt.dSplStrain.Length-cp.iSampleNum)						'サンプルに与えたひずみ
		cp.clDt.p_curShearMod	= cp.clDt.dShearMod(cp.clDt.dShearMod.Length-cp.iSampleNum)							'ずり弾性率
		cp.clDt.p_curShearVelo	= cp.clDt.dShearVelo(cp.clDt.dShearVelo.Length-cp.iSampleNum)						'剪断速度
		cp.clDt.p_curStrgMod	= cp.clDt.dXStrgMod(cp.clDt.dXStrgMod.Length-cp.iSampleNum)							'X貯蔵弾性率
		cp.clDt.p_curElasMod	= cp.clDt.dXElasMod(cp.clDt.dXElasMod.Length-cp.iSampleNum)							'X損失弾性率
		cp.clDt.p_curLosTan	= cp.clDt.dXLossTan(cp.clDt.dXLossTan.Length-cp.iSampleNum)								'X損失正接
	End Select


	'IIRフィルタまたはスキップ
	Select Case iBranch
	Case 0		'キャリブレーションまたは最終測定のとき	
		'処理済 補正微分データを追加 ---------------------------------------------------------------------------------------
		If cp.iCalcType = eCalcMethod.SLOPE
			MakeHoseiToSlope(cp)
		End If
		Select Case cp.iMeas
		Case eHoseiMeasMode.GelTime		'ゲルタイム
			If cp.fltSet.iEnable = 1
				PostFilterProc(cp)		'計算後デジタルフィルタ処理
			Else
			'スキップ
			End If
		End Select
	Case Else

	End Select



#If 1
If p_bCalcResultFOutEn		'隠しコマンドで有効か
If iBranch = 0				'最終一括処理,キャリブレーション時のみ

	'デバッグ用ファイル出力
	Dim fs As New FileStream("C:\Test\debugCorrectX_" & Now().ToString("MMddhhmmss") & ".csv", FileMode.Create, FileAccess.Write, FileShare.ReadWrite)
	Dim sw As Textwriter = New StreamWriter(fs, Encoding.UTF8)
	Dim sLineBuf As String = nothing
	'テキストファイル(CSV)書込み
	'ヘッダ行
	'ヘッダ行
	sLineBuf = "Time,iOxBuf,iOyBuf,iOzBuf,iPxBuf,iPyBuf,iPzBuf,TempX_1,TempX_A,TempX_B,TempX_Q,"
	sLineBuf = sLineBuf & "Temp_D,Temp_C,Temp_Ts,Temp_Tc,Temp_T2,Temp_T3,Temp_T3Ave,Temp_J,Temp_K,Temp_L,Temp_M,Temp_N,Slope,dViscosity,iHosei,"
	sLineBuf = sLineBuf & "TempXp_B,TempXp_D,TempXp_C,TempXp_Tc,TempXp_T1,TempXp_Ts,TempXp_T2,TempXp_T3_Ave,Temp_J100,"
	sLineBuf = sLineBuf & "dXPosPhase,dYPosPhase,dSplStrain,dShearMod,dShearVelo,dXStrgMod,dXElasMod,dXLossTan,dYStrgMod,dYElasMod,dYLossTan"

	sw.WriteLine(sLineBuf)
	Dim dTime As Double = 0.0
	For i = 0 To cp.clDt.iOxBuf.GetUpperBound(0)
		sLineBuf = dTime.ToString("F3") & "," & _
		cp.clDt.iOxBuf(i).ToString & "," & _
		cp.clDt.iOyBuf(i).ToString & "," & _
		cp.clDt.iOzBuf(i).ToString & "," & _
		cp.clDt.iPxBuf(i).ToString & "," & _
		cp.clDt.iPyBuf(i).ToString & "," & _
		cp.clDt.iPzBuf(i).ToString & "," & _
		If(Double.IsNaN(TempX_1(i)),0,TempX_1(i).ToString) & "," &  _
		If(Double.IsNaN(TempX_A(i)),0,TempX_A(i).ToString) & "," &  _
		If(Double.IsNaN(TempX_B(i)),0,TempX_B(i).ToString) & "," &  _
		If(Double.IsNaN(TempX_Q(i)),0,TempX_Q(i).ToString) & ","

		sLineBuf = sLineBuf & _
		Temp_D(i).ToString & "," & _
		Temp_C(i).ToString & "," & _
		Temp_Ts(i).ToString & "," & _
		Temp_Tc(i).ToString & "," & _
		Temp_T2(i).ToString & "," & _
		Temp_T3(i).ToString & "," & _
		Temp_T3Ave(i).ToString & "," & _
		Temp_J(i).ToString & "," & _
		Temp_K(i).ToString & "," & _
		Temp_L(i).ToString & "," & _
		Temp_M(i).ToString & "," & _
		Temp_N(i).ToString & "," & _
		cp.clDt.iSlope(i).ToString & "," & _
		cp.clDt.dViscosity(i).Tostring & "," & _
		cp.clDt.iHosei(i).ToString

		sLineBuf = sLineBuf & "," & _
		TempXp_B(i).ToString & "," & _
		TempXp_D(i).ToString & "," & _
		TempXp_C(i).ToString & "," & _
		TempXp_Tc(i).ToString & "," & _
		TempXp_T1(i).ToString & "," & _
		TempXp_Ts(i).ToString & "," & _
		TempXp_T2(i).ToString & "," & _
		TempXp_T3Ave(i).ToString & "," & _
		Temp_J100(i).ToString & ","

		sLineBuf = sLineBuf & _
		cp.clDt.dXPosPhase(i).ToString & "," & _
		cp.clDt.dYPosPhase(i).ToString & "," & _
		cp.clDt.dSplStrain(i).ToString & "," & _
		cp.clDt.dShearMod(i).ToString & "," & _
		cp.clDt.dShearVelo(i).ToString & "," & _
		cp.clDt.dXStrgMod(i).ToString & "," & _
		cp.clDt.dXElasMod(i).ToString & "," & _
		cp.clDt.dXLossTan(i).ToString & "," & _
		cp.clDt.dYStrgMod(i).ToString & "," & _
		cp.clDt.dYElasMod(i).ToString & "," & _
		cp.clDt.dYLossTan(i).ToString

 		sw.WriteLine(sLineBuf)
		dTime += 0.002
	Next i
	sw.Close()
	sw = nothing
End if
End if
#end if
	'debug.Print($"iHosei() = {clDt.iHosei(clDt.iHosei.GetUpperBound(0))}")

	'点モード時は粘度関係の計算は無効のため
	Select Case cp.iStrsSensMode
	Case clSpec.eStrsSens.Point
		'結果バッファ内のクリア
		Array.Clear(cp.clDt.dViscosity, 0, cp.clDt.dViscosity.Length)		'粘度
		Array.Clear(cp.clDt.dSplStrain, 0, cp.clDt.dSplStrain.Length)		'サンプルに与えたひずみ
		Array.Clear(cp.clDt.dShearMod,	0, cp.clDt.dShearMod.Length)		'ずり弾性率
		Array.Clear(cp.clDt.dShearVelo,	0, cp.clDt.dShearVelo.Length)		'剪断速度
		Array.Clear(cp.clDt.dXStrgMod,	0, cp.clDt.dXStrgMod.Length)		'X貯蔵弾性率
		Array.Clear(cp.clDt.dXElasMod,	0, cp.clDt.dXElasMod.Length)		'X損失弾性率
		'リアルタイム数値モニタ表示用(frmMain)
		cp.clDt.p_curViscosity	= 0								'粘度
		cp.clDt.p_curSplStrain	= 0								'サンプルに与えたひずみ
		cp.clDt.p_curShearMod	= 0								'ずり弾性率
		cp.clDt.p_curShearVelo	= 0								'剪断速度
		cp.clDt.p_curStrgMod	= 0								'X貯蔵弾性率
		cp.clDt.p_curElasMod	= 0								'X損失弾性率
	Case Else
	End Select

	Return 0

Catch ex As OverflowException
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Return -1

Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Return -1

End Try
End Function


''' <summary>
''' SLOPE法の計算(微分)
''' </summary>
''' <param name="cp">データファイルクラス</param>
''' <returns></returns>
Public Function MakeHoseiToSlope(ByRef cp As stCalcPara) As Int32
	Dim i As Int32
	Dim iSampleNum			As int32 = cp.clDt.iHosei.Length			'配列サイズ
	Dim iArraySize			As Int32 = iSampleNum - 1					'配列サイズ
	Dim iSmpEnd				As int32
	Dim	Temp_K(iArraySize)	As Double												'XY合成微分配列
	Dim iRangeSec			As Double = cp.dDiffInterval							'微分区間(秒)
	Dim iStartidx			As int32 = 10 / (MEAS_TIME_RESOLUTION / 1000.0)			'XY動作待ち分の処理をスキップする
	Dim iRangeNum			As Int32 = iRangeSec / (MEAS_TIME_RESOLUTION / 1000.0)	'微分区間のサンプル数 
	Dim dWait				As Double = cp.clDt.p_head.dXYActWait + cp.clDt.p_head.dHPLiftWait

Try
	iSmpEnd = iSampleNum - 1						'処理最終インデックス
	If dWait <> 0
		iStartidx = (dWait+iRangeSec) / (MEAS_TIME_RESOLUTION / 1000.0)				'要検討
	End If 
	Array.Clear(Temp_K, 0 ,Temp_K.Length)
	'処理済 補正微分データを追加 ---------------------------------------------------------------------------------------
	For i = iRangeNum To iSmpEnd
		'If i >= iStartidx							'処理最終インデックス
		If i - iRangeNum/2 >= iStartidx							'処理最終インデックス
			Dim dDeltaY As Double = cp.cldt.iHosei(i) - cp.cldt.iHosei(i - iRangeNum)
			Dim dDeltaX As Double = (i - (i - iRangeNum)) * (MEAS_TIME_RESOLUTION / 1000.0) 
			'Temp_K(i - iRangeNum/2) = (dDeltaY / dDeltaX) / iRangeSec
			Temp_K(i - iRangeNum/2) = (dDeltaY / dDeltaX)
		Else
			Temp_K(i - iRangeNum/2) = 0
		End if
	Next i

	Array.Clear(cp.clDt.iSlope, 0 ,cp.clDt.iSlope.Length)
	Array.Copy(Temp_K, 0, cp.clDt.iSlope, 0, cp.clDt.iSlope.Length)
#If 0
	'補正微分データにIIRフィルタ処理
	Dim fil				As New Filter								'フィルタクラス生成
	fltSet.dIIRCutOffFreq = 1 / dPeriod	/ 16						'カットオフ周波数=測定周波数/16でフィルタ処理
	fltSet.iIIRDeg = 3
	fil.IirFilterButterworth(clDt.iSlope, fltSet, clDt.iSlope)		'IIRバタワース
#End If 
#If 0
	If fltSet.iEnable = 1
		'補正微分データにFIRフィルタ処理
		Dim fil				As New Filter								'フィルタクラス生成
		Dim	Coef()			As Double = Nothing							'FIRフィルタパラメータ(窓関数から生成)
		fltSet.dFIRCutOffFreq = 1 / dPeriod	/ 16						'カットオフ周波数=測定周波数/16でフィルタ処理
		fltSet.dFIRSmplFreq = 500
		fltSet.iFIRFilLen= 4001
		'fltSet.iFIRFilLen= 32767
		fltSet.sFIRWinKind = "Hamming"
		fltSet.sKind = "FIR"
		fil.WindowFunction(fltSet,	Coef)								'FIRフィルタパラメータ作成
		fil.FirFilterD(clDt.iSlope, coef, clDt.iSlope)					'FIR
		'遅延分をずらす
		Dim iFillen As Int32 = fltSet.iFIRFilLen\2
		Array.Copy(clDt.iSlope, iFillen, clDt.iSlope, 0, clDt.iSlope.Length - iFillen )
		'遅延分をNAN
		For i=clDt.iSlope.Length - iFillen To clDt.iSlope.getupperbound(0)
			clDt.iSlope(i) = Double.NaN
		Next
	End If
#End If 
	Return 0
Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Return -1
End Try
End Function


''' <summary>
''' 汎用バッチ式移動平均 最初の区間は前回値で埋める
''' </summary>
''' <param name="dSource">元サンプル配列</param>
''' <param name="dDestArry">移動平均化された配列</param>
''' <param name="iPriod">移動平均区間</param>
Public Sub	BatchMovingAverage(ByRef dSource() As Double, ByRef dDestArry() As Double, ByVal iPriod As Int32 )
Dim i As int32
Dim j As int32
Dim AveArray() As Double = Nothing
Try
	Array.Resize(AveArray, iPriod + 1)										'平均算出用バッファ確保
	j = iPriod + 1
	'最後の区間終了が最大インデックスになるようサンプル配列インデックスでループ 
	For i = 0 To dSource.GetUpperBound(0) - (iPriod + 1)
		Array.Copy( dSource, i, AveArray, 0, AveArray.Length)					'区間サンプルを平均計算用バッファへコピー
		dDestArry(j) = AveArray.Average()										'区間平均値計算して移動平均後のデータ配列へセット
		j +=1																	'移動平均後のデータ配列インデックス更新
	Next i
	For i = 0 To iPriod															'先頭半周期を前回値で埋める
		dDestArry(i) = dDestArry(iPriod + 1)
	Next i
Catch ex As Exception
End Try
End Sub
''' <summary>
''' 上限下限を超えるデータを指定値で固定する
''' </summary>
''' <param name="dData">データ</param>
''' <param name="dLower">下限値</param>
''' <param name="dUpper">上限値</param>
''' <param name="dLimLo">下限固定値</param>
''' <param name="dLimUp">上限固定値</param>
Private Sub	TrigoRangeLimit(ByRef dData As Double, ByVal dLower As Double, ByVal dUpper As Double, ByVal dLimLo As Double, ByVal dLimUp As Double)
	if dData		< dLower
		dData = dLimLo
	End If
	if dData		> dUpper
		dData = dLimUp
	End If
End Sub

''' <summary>
''' 粘度関係計算処理 (フローカーブ測定で使用)
''' </summary>
''' <param name="vp">粘度計算パラメータ構造体</param>
''' <returns></returns>
Public Function ViscosityCalc(ByRef vp As DataFile.ViscosityCalcParameter ) As Int32
	Dim dStrain As Double
	Dim dSpeed	As Double
Try
	If vp.dAmplitude = 0 Or vp.dPeriod = 0 Or vp.dVelocityRate = 0
		dStrain				= 0
		dSpeed				= 0
		vp.dShearVelocity	= 0
		vp.dShearStress		= 0
		vp.dViscosity		= 0
	Else
		dStrain				= ( (vp.dAmplitude / 2) / vp.dMeasGap ) * 100		'ひずみ量
		dSpeed				= ( vp.dAmplitude * Math.PI ) / vp.dPeriod			'速度
		vp.dShearVelocity	= (dSpeed / vp.dMeasGap) * vp.dVelocityRate			'せん断速度
		vp.dShearStress		= vp.dMeasStress / vp.dProbeSquare					'せん断応力
		vp.dViscosity		= vp.dShearStress / vp.dShearVelocity				'粘度
	End If
Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Return -1
End Try
	Return 0
End Function


''' <summary>
''' 計算後フィルタ処理
''' </summary>
''' <param name="cp"></param>
Private Sub	PostFilterProc(ByRef cp As stCalcPara)     'ByRef cldt As DataFile, ByRef fltSet As stFilterInfo)
Try
	Dim fil				As New Filter									'フィルタクラス生成
	Dim	Coef()			As Double = Nothing								'FIRフィルタパラメータ(窓関数から生成)
	Dim i				As Int64

	Dim iHosei()		As Double	= Nothing
	Dim iSlope()		As Double	= Nothing
	Dim dHosei100()		As Double	= Nothing
	Dim dVisco()		As Double	= Nothing
	Dim dXPosPhase()	As Double	= Nothing
	Dim dXStrgMod()		As Double	= Nothing
	Dim dXElasMod()		As Double	= Nothing
	Dim dXLossTan()		As Double	= Nothing
	Dim dSplStrain()	As Double	= Nothing
	Dim dShearMod()		As Double	= Nothing
	Dim dShearVelo()	As Double	= Nothing

	Array.Resize(iHosei,		cp.cldt.iHosei.Length)						'フィルタ用補正データ領域確保
	Array.Resize(iSlope,		cp.cldt.iSlope.Length)						'フィルタ用補正データ領域確保
	Array.Resize(dHosei100,		cp.cldt.dHosei100.Length)					'フィルタ用補正データ領域確保
	Array.Resize(dVisco,		cp.cldt.dViscosity.Length)					'フィルタ用補正データ領域確保
	Array.Resize(dXPosPhase,	cp.cldt.dXPosPhase.Length)					'フィルタ用補正データ領域確保
	Array.Resize(dXStrgMod,		cp.cldt.dXStrgMod.Length)					'フィルタ用補正データ領域確保
	Array.Resize(dXElasMod,		cp.cldt.dXElasMod.Length)					'フィルタ用補正データ領域確保
	Array.Resize(dXLossTan,		cp.cldt.dXLossTan.Length)					'フィルタ用補正データ領域確保
	Array.Resize(dSplStrain,	cp.cldt.dSplStrain.Length)					'フィルタ用補正データ領域確保
	Array.Resize(dShearMod,		cp.cldt.dShearMod.Length)					'フィルタ用補正データ領域確保
	Array.Resize(dShearVelo,	cp.cldt.dShearVelo.Length)					'フィルタ用補正データ領域確保
	Select Case cp.fltSet.sKind
	Case "FIR"
		fil.WindowFunction(cp.fltSet,	Coef)			'FIRフィルタパラメータ作成
		fil.FirFilterD(cp.cldt.iHosei,		Coef, iHosei)					'FIRフィルタ
		fil.FirFilterD(cp.cldt.iSlope,		Coef, iSlope)					'FIRフィルタ
		fil.FirFilterD(cp.cldt.dHosei100,	Coef, dHosei100)				'FIRフィルタ
		fil.FirFilterD(cp.cldt.dViscosity, Coef, dVisco)					'FIRフィルタ
		fil.FirFilterD(cp.cldt.dXPosPhase, Coef, dXPosPhase)				'FIRフィルタ
		fil.FirFilterD(cp.cldt.dXStrgMod,	Coef, dXStrgMod)				'FIRフィルタ
		fil.FirFilterD(cp.cldt.dXElasMod,	Coef, dXElasMod)				'FIRフィルタ
		fil.FirFilterD(cp.cldt.dXLossTan,	Coef, dXLossTan)				'FIRフィルタ
		fil.FirFilterD(cp.cldt.dSplStrain, Coef, dSplStrain)				'FIRフィルタ
		fil.FirFilterD(cp.cldt.dShearMod,	Coef, dShearMod)				'FIRフィルタ
		fil.FirFilterD(cp.cldt.dShearVelo,	Coef, dShearVelo)				'FIRフィルタ
#If 1
		'遅延分をずらす
		Dim filLen As Int32 = cp.fltSet.iFIRFilLen/2
		Array.Copy(iHosei,		filLen, iHosei,		0, iHosei.Length-filLen)
		Array.Copy(iSlope,		filLen, iSlope,		0, iSlope.Length-filLen)
		Array.Copy(dHosei100,	filLen, dHosei100,	0, dHosei100.Length - filLen )
		Array.Copy(dVisco,		filLen, dVisco,		0, dVisco.Length - filLen )
		Array.Copy(dXPosPhase,	filLen, dXPosPhase,	0, dXPosPhase.Length - filLen )
		Array.Copy(dXStrgMod,	filLen, dXStrgMod,	0, dXStrgMod.Length - filLen )
		Array.Copy(dXElasMod,	filLen, dXElasMod,	0, dXElasMod.Length - filLen )
		Array.Copy(dXLossTan,	filLen, dXLossTan,	0, dXLossTan.Length - filLen )
		Array.Copy(dSplStrain,	filLen, dSplStrain,	0, dSplStrain.Length - filLen )
		Array.Copy(dShearMod,	filLen, dShearMod,	0, dHosei100.Length - filLen )
		Array.Copy(dShearVelo,	filLen, dShearVelo,	0, dHosei100.Length - filLen )
		'遅延分をNAN
		For i=cp.clDt.iHosei.Length - filLen To cp.clDt.iHosei.getupperbound(0)
			iHosei(i)				= Double.NaN
			iSlope(i)				= Double.NaN
			dHosei100(i)			= Double.NaN
			dVisco(i)				= Double.NaN
			dXStrgMod(i)			= Double.NaN
			dXElasMod(i)			= Double.NaN
			dXLossTan(i)			= Double.NaN
			dSplStrain(i)			= Double.NaN
			dShearMod(i)			= Double.NaN
			dShearVelo(i)			= Double.NaN
		Next
#end if
	Case "IIR"
		fil.IirFilterButterworthD(cp.cldt.iHosei,		cp.fltSet, iHosei)		'IIRバタワース
		fil.IirFilterButterworthD(cp.cldt.iSlope,		cp.fltSet, iSlope)		'IIRバタワース
		fil.IirFilterButterworthD(cp.cldt.dHosei100,	cp.fltSet, dHosei100)	'IIRバタワース
		For i=0 To cp.cldt.iHosei.GetUpperBound(0)
			If Double.IsNan(cp.cldt.dViscosity(i))
				cp.cldt.dViscosity(i)	= Double.NaN
				cp.cldt.dXPosPhase(i)	= Double.NaN
				cp.cldt.dXStrgMod(i)	= Double.NaN
				cp.cldt.dXElasMod(i)	= Double.NaN
				cp.cldt.dXLossTan(i)	= Double.NaN
				cp.cldt.dSplStrain(i)	= Double.NaN
				cp.cldt.dShearMod(i)	= Double.NaN
				cp.cldt.dShearVelo(i)	= Double.NaN
				cp.cldt.dXStrgMod(i)	= Double.NaN
			End If
		Next i
		fil.IirFilterButterworthD(cp.cldt.dViscosity,	cp.fltSet, dVisco)		'IIRバタワース
		fil.IirFilterButterworthD(cp.cldt.dXPosPhase,	cp.fltSet, dXPosPhase)	'IIRバタワース
		fil.IirFilterButterworthD(cp.cldt.dXStrgMod,	cp.fltSet, dXStrgMod)	'IIRバタワース
		fil.IirFilterButterworthD(cp.cldt.dXElasMod,	cp.fltSet, dXElasMod)	'IIRバタワース
		fil.IirFilterButterworthD(cp.cldt.dXLossTan,	cp.fltSet, dXLossTan)	'IIRバタワース
		fil.IirFilterButterworthD(cp.cldt.dSplStrain,	cp.fltSet, dSplStrain)	'IIRバタワース
		fil.IirFilterButterworthD(cp.cldt.dShearMod,	cp.fltSet, dShearMod)	'IIRバタワース
		fil.IirFilterButterworthD(cp.cldt.dShearVelo,	cp.fltSet, dShearVelo)	'IIRバタワース
	End Select

	Array.Copy(iHosei,		cp.cldt.iHosei,		iHosei.Length)			'最終補正データをフィルタ済補正データで上書き
	Array.Copy(iSlope,		cp.cldt.iSlope,		iSlope.Length)			'最終補正データをフィルタ済補正データで上書き
	Array.Copy(dHosei100,	cp.cldt.dHosei100,	dHosei100.Length)		'最終補正データをフィルタ済補正データで上書き
	Array.Copy(dVisco,		cp.cldt.dViscosity,	dVisco.Length)			'最終補正データをフィルタ済補正データで上書き
	Array.Copy(dXPosPhase,	cp.cldt.dXPosPhase,	dXPosPhase.Length)		'最終補正データをフィルタ済補正データで上書き
	Array.Copy(dXStrgMod,	cp.cldt.dXStrgMod,	dXStrgMod.Length)		'最終補正データをフィルタ済補正データで上書き
	Array.Copy(dXElasMod,	cp.cldt.dXElasMod,	dXElasMod.Length)		'最終補正データをフィルタ済補正データで上書き
	Array.Copy(dXLossTan,	cp.cldt.dXLossTan,	dXLossTan.Length)		'最終補正データをフィルタ済補正データで上書き
	Array.Copy(dSplStrain,	cp.cldt.dSplStrain,	dSplStrain.Length)		'最終補正データをフィルタ済補正データで上書き
	Array.Copy(dShearMod,	cp.cldt.dShearMod,	dShearMod.Length)		'最終補正データをフィルタ済補正データで上書き
	Array.Copy(dShearVelo,	cp.cldt.dShearVelo,	dShearVelo.Length)		'最終補正データをフィルタ済補正データで上書き

Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub
''' <summary>
''' 微分関数
''' </summary>
''' <param name="X1">データn-1</param>
''' <param name="X2">データn</param>
''' <param name="T1">時刻n-1</param>
''' <param name="T2">時刻n</param>
''' <param name="dFreq">移動周波数Hz</param>
''' <returns></returns>
Private Function DifferentialProc(byval X1 As Double, byval X2 As Double, byval T1 As Double, byval T2 As Double, byVal dFreq As Double ) As Double
Dim dRet As Double
Try
	dRet = ((X2 - X1) / (T2 - T1)) / (2 * Math.pi * dfreq )
Catch ex As Exception
End Try
	Return dRet
End Function
''' <summary>
''' 汎用バッチ式移動平均 最初の区間は前回値で埋める(2並列版 サンプル数は同一であること)
''' </summary>
''' <param name="dSource1"></param>
''' <param name="dDestArry1"></param>
''' <param name="dSource2"></param>
''' <param name="dDestArry2"></param>
''' <param name="iPriod"></param>
Public Sub	BatchMovingAverage2(ByRef dSource1() As Double, ByRef dDestArry1() As Double, ByRef dSource2() As Double, ByRef dDestArry2() As Double, ByVal iPriod As Int32 )
Dim i As int32
Dim j As int32
Dim AveArray1() As Double = Nothing
Dim AveArray2() As Double = Nothing
Try
	Array.Resize(AveArray1, iPriod + 1)										'平均算出用バッファ確保
	Array.Resize(AveArray2, iPriod + 1)										'平均算出用バッファ確保
	j = iPriod + 1
	'最後の区間終了が最大インデックスになるようサンプル配列インデックスでループ 
	For i = 0 To dSource1.GetUpperBound(0) - (iPriod + 1)
		Array.Copy( dSource1, i, AveArray1, 0, AveArray1.Length)				'区間サンプルを平均計算用バッファへコピー
		dDestArry1(j) = AveArray1.Average()										'区間平均値計算して移動平均後のデータ配列へセット
		Array.Copy( dSource2, i, AveArray2, 0, AveArray2.Length)				'区間サンプルを平均計算用バッファへコピー
		dDestArry2(j) = AveArray2.Average()										'区間平均値計算して移動平均後のデータ配列へセット
		j +=1																	'移動平均後のデータ配列インデックス更新
	Next i
	For i = 0 To iPriod															'先頭半周期を前回値で埋める
		dDestArry1(i) = dDestArry1(iPriod + 1)
		dDestArry2(i) = dDestArry2(iPriod + 1)
	Next i
Catch ex As Exception
End Try
End Sub
''' <summary>
''' 汎用バッチ式移動平均 最初の区間は前回値で埋める(3並列版 サンプル数は同一であること)
''' </summary>
''' <param name="dSource1"></param>
''' <param name="dDestArry1"></param>
''' <param name="dSource2"></param>
''' <param name="dDestArry2"></param>
''' <param name="iPriod"></param>
Public Sub	BatchMovingAverage3(ByRef dSource1() As Double, ByRef dDestArry1() As Double, ByRef dSource2() As Double, ByRef dDestArry2() As Double, ByRef dSource3() As Double, ByRef dDestArry3() As Double, ByVal iPriod As Int32 )
Dim i As int32
Dim j As int32
Dim AveArray1() As Double = Nothing
Dim AveArray2() As Double = Nothing
Dim AveArray3() As Double = Nothing
Try
	Array.Resize(AveArray1, iPriod + 1)										'平均算出用バッファ確保
	Array.Resize(AveArray2, iPriod + 1)										'平均算出用バッファ確保
	Array.Resize(AveArray3, iPriod + 1)										'平均算出用バッファ確保
	j = iPriod + 1
	'最後の区間終了が最大インデックスになるようサンプル配列インデックスでループ 
	For i = 0 To dSource1.GetUpperBound(0) - (iPriod + 1)
		Array.Copy( dSource1, i, AveArray1, 0, AveArray1.Length)				'区間サンプルを平均計算用バッファへコピー
		dDestArry1(j) = AveArray1.Average()										'区間平均値計算して移動平均後のデータ配列へセット
		Array.Copy( dSource2, i, AveArray2, 0, AveArray2.Length)				'区間サンプルを平均計算用バッファへコピー
		dDestArry2(j) = AveArray2.Average()										'区間平均値計算して移動平均後のデータ配列へセット
		Array.Copy( dSource3, i, AveArray3, 0, AveArray3.Length)				'区間サンプルを平均計算用バッファへコピー
		dDestArry3(j) = AveArray3.Average()										'区間平均値計算して移動平均後のデータ配列へセット
		j +=1																	'移動平均後のデータ配列インデックス更新
	Next i
	For i = 0 To iPriod															'先頭半周期を前回値で埋める
		dDestArry1(i) = dDestArry1(iPriod + 1)
		dDestArry2(i) = dDestArry2(iPriod + 1)
		dDestArry3(i) = dDestArry3(iPriod + 1)
	Next i
Catch ex As Exception
End Try
End Sub
''' <summary>
''' 最小二乗法による多項近似
''' </summary>
''' <param name="dX">X軸配列</param>
''' <param name="dY">Y軸配列</param>
''' <param name="dNewX">変換後X軸配列</param>
''' <param name="dNewY">変換後Y軸配列</param>
''' <param name="iNum">データ数</param>
''' <param name="iM">近似次数</param>
''' <returns>数式パラメータ配列  一次式例) y = ax + b -> b:0 a:1</returns>
Public Function fnLeastSquaresMethod(ByVal dX() As Double, ByVal dY() As Double, _
                                        ByRef dNewX() As Double, ByRef dNewY() As Double, _
										ByVal iNum As Integer, ByVal iM As Integer) As Double()
Try
Dim dA(iM, iM + 1)	As Double
Dim dS(2 * iM)		As Double
Dim dT(iM)			As Double

    '配列の初期化
    For i As Integer = 0 To 2 * iM
        dS(i) = 0
    Next i
    For i As Integer = 0 To iM
        dT(i) = 0
    Next i

    'X軸平均値を取得
    Dim XAve As Double = dX.Average()

    '
    For i As Integer = 0 To iNum - 1
        For j As Integer = 0 To 2 * iM
            dS(j) = dS(j) + dX(i) ^ j
        Next j
        For j As Integer = 0 To iM
            dT(j) = dT(j) + dX(i) ^ j * dY(i)
        Next j
    Next i

    For i As Integer = 0 To iM
        For j As Integer = 0 To iM
            dA(i, j) = dS(i + j)
        Next j
        dA(i, iM + 1) = dT(i)
    Next i

    For k As Integer = 0 To iM
        Dim p As Double = dA(k, k)
        For j As Integer = k To iM + 1
            dA(k, j) = dA(k, j) / p
        Next j
        For i As Integer = 0 To iM
            If i <> k Then
                Dim d As Double = dA(i, k)
                For j As Integer = k To iM + 1
                    dA(i, j) = dA(i, j) - d * dA(k, j)
                Next j
            End If
        Next i
    Next k

    '数式を代入
    Dim dRet As Double() = New Double(iM) {}
    Dim str As String
    str = CStr(dA(0, iM + 1))
    dRet(0) = dA(0, iM + 1)
    For i As Integer = 1 To iM
        str += " + " + CStr(dA(i, iM + 1)) + "x^" + CStr(i)
        dRet(i) = dA(i, iM + 1)
    Next i
    'Console.WriteLine(str)

    '引数の配列に代入
    If dNewX IsNot Nothing AndAlso dNewY IsNot Nothing Then
        Dim iLen As Integer = dNewX.Length - 1
        For iCnt As Integer = 0 To iLen
            dNewX(iCnt) = dX(iCnt) 'CDbl(iCnt) * 0.1R
            dNewY(iCnt) = dA(0, iM + 1)

            For i As Integer = 1 To iM
                dNewY(iCnt) += dA(i, iM + 1) * Math.Pow(dNewX(iCnt), CDbl(i))
            Next i
        Next iCnt
    End If
    Return dRet
Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
    Return Nothing
End Try
End Function
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
	dADVal = dWeight / (dWeightCoef * DataFile.cGraviAcc/1000)
	Return dADVal
Catch ex As Exception
	Return Double.NaN
End Try
End Function
'TempXp_BAve(振幅 um 0-p)からキャリブレーションテーブルをサーチしてフル応力値とフル位相値を得る
Public Function CalibSearchPointStressConstMode(ByVal cp As stCalcPara, ByVal dTempXp As Double,  ByRef dCalibFullStress As Double, ByRef dCalibFullPhase As Double) As Int32
Dim i		As int32
Dim x(1)	As Double
Dim y(1)	As Double
Dim nx(1)	As Double
Dim ny(1)	As Double
Dim dRet(1)	As Double
Dim a		As Double
Dim b		As Double
Try
	For i = cp.dCalibAmp.GetUpperBound(0) To 0 Step  -1
		If i-1 >= 0
			If dTempXp <= cp.dCalibAmp(i) And dTempXp >= cp.dCalibAmp(i-1)
			'2点間の直線近似
				'フル応力を計算
				x(0)=cp.dCalibAmp(i-1)
				y(0)=cp.dCalibFullStress(i-1)
				x(1)=cp.dCalibAmp(i)
				y(1)=cp.dCalibFullStress(i)
				dRet = fnLeastSquaresMethod(x, y, nx, ny, 2, 1)
				a = dRet(1)		'傾きをセット
				b = dRet(0)		'Y切片をセット
				dCalibFullStress = a * dTempXp + b
				'フル位相を計算
				x(0)=cp.dCalibAmp(i-1)
				y(0)=cp.dCalibFullPhase(i-1)
				x(1)=cp.dCalibAmp(i)
				y(1)=cp.dCalibFullPhase(i)
				dRet = fnLeastSquaresMethod(x, y, nx, ny, 2, 1)
				a = dRet(1)		'傾きをセット
				b = dRet(0)		'Y切片をセット
				dCalibFullPhase = a * dTempXp + b
				Return 0

			ElseIf (dTempXp > cp.dCalibAmp(i)) And (i = cp.dCalibAmp.GetUpperBound(0))
			'定義最大振幅を超えている
				Dim idx As Int32 = cp.dCalibAmp.GetUpperBound(0)
				dCalibFullStress	= cp.dCalibFullStress(idx)
				dCalibFullPhase		= cp.dCalibFullPhase(idx)
				Return 1
			End If
		Else
		'定義最小振幅を下回っている dTempXpが負数
			dCalibFullStress	= cp.dCalibFullStress(0)
			dCalibFullPhase		= cp.dCalibFullPhase(0)
			Return 1
		End If
	Next
	Return 0
Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Return -1
End Try
End Function
#End If

End Class
