'============================================
'File:   MemLinkTCP.vb
'Name:   SIZUKA Tool 通信クラス
'Desc:   通信処理を行うクラス。
'============================================

Imports System.Net.Sockets
Imports System.IO
Imports System.Threading
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.Runtime.InteropServices
'''<summary>
'''TCPIPによるメモリリンク通信を行うクラス。
'''</summary>
Public Class MemLinkTCP

'▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼
'   定数
'▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲
Private Const TCP_HEAD_OFS		As Integer = 10
Private Const cCONST_TLG_NUM	As Integer = 13

'リンクマップ ブロック情報
Public Const W_TOP_LNK_ADR		As Int32 = 1000         '書込領域先頭アドレス
Public Const W_END_LNK_ADR		As Int32 = 1100         '書込領域終了アドレス

'リンクマップ ブロック情報
Public Const R_TOP_LNK_ADR		As Int32 = 1100         '読込領域先頭アドレス

Public Const R_MNO_LNK_ADR		As Int32 = 1130         '測定No

Public Const R_MES_LNK_TOP		As Int32 = 1200			'測定データTOP
Public Const R_PWX_LNK_ADR		As Int32 = 1200         '応力X
Public Const R_PWY_LNK_ADR		As Int32 = 2200         '応力Y

Public Const R_PWZ_LNK_ADR		As Int32 = 3200         '応力Z
Public Const R_ASX_LNK_ADR		As Int32 = 4200         '軸位置X
Public Const R_ASY_LNK_ADR		As Int32 = 4700         '軸位置Y
Public Const R_TMP_LNK_ADR		As Int32 = 5200         '熱板温度
Public Const R_TMV_LNK_ADR		As Int32 = 5700         '熱板温度操作量
Public Const R_TMP2_LNK_ADR		As Int32 = 6200         'プローブ温度
Public Const R_TMV2_LNK_ADR		As Int32 = 6700         'プローブ温度操作量
Public Const R_ASZ_LNK_ADR		As Int32 = 7200         '軸位置Z
Public Const R_DUP_LNK_ADR		As Int32 = 7700         '更新要求
Public Const R_MES_LNK_END		As Int32 = 7701         '測定データEND



'メモリリンクアドレス
Public Enum eMEMADR
	iTEMP_PRS				= 1000				'熱板温度設定値(℃)
	iGAP_PRS									'ギャップ値(mm)
	iMES_WAIT									'測定待ち時間(s) 熱板上昇待ち
	iCLR_TIME									'クリア時間(s)   投入待ち
	iTMP_RNG									'熱板 温度許容範囲(℃)
	iMAIN_UNIT_SERIAL		= 1005				'本体製造番号
''	iHEAT_LED_TEMP								'熱板LED切替え温度(℃)
	iLED_LIGHT									'LED照明 0:消灯 1:点灯
	iHEAT_P_PID									'熱板PID制御 P 比例定数
	iHEAT_I_PID									'熱板PID制御 I 積分定数(s)
	iHEAT_D_PID									'熱板PID制御 D 微分定数(s)
	iHEAT_DGAIN				= 1010				'熱板PID制御 微分ゲイン
	iHEAT_PVBIAS								'熱板PID制御 温度補正(℃)
	iSTANBY_TIME								'スタンバイ時間(s)				自動電源OFF制御 (未使用)
	iIN_OPERATION								'本体操作中	0:操作なし 1:操作中 自動電源OFF制御 (未使用)
	iCONFIG1				= 1014					'本体動作設定 ->
												'	b0:終了時ブザー発報
												'	b1:ヒータレンジ内動作OK　ブザー発報
												'	b2:未使用
												'	b3:カバーIL無効
												'	b4:測定終了ガイド音bit0
												'	b5:測定終了ガイド音bit1
												'	B6:
												'	B7:
	iSTART_TEMP_CONT		= 1015				'温度制御開始 1:熱板 2:先端 4:起歪体
	iSTART_SW				= 1016				'測定開始 b0:ゼロ調 b1:測定
	iERR_RESET              = 1017		        'エラーリセット
	iHEATER_LIMIT           = 1018		        '熱板ヒーター上限温度(℃)

	iINP_GAP	            = 1020		        '投入ギャップ(mm) 0.0-100.0
	iMES_PRE_WAIT	        = 1021		        '圧着後の待機時間時間(s)  上昇後XY動作開始するまでの時間
	iACT_MODE							        'XY動作モード 0:X動作(矩形波) 1:X動作(正弦波) 2:XY動作(正弦波) ->1固定
	iAMPLITUDE_X			= 1023				'X軸振幅(um)
	iAMPLITUDE_Y								'Y軸振幅(um)
	iPERIOD_X				= 1025				'X軸周期(s)
	iPERIOD_Y				 					'Y軸周期(s)

	'iPOS_HEATB_HOME_L		= 1027				'熱板待機位置(L) (um)
	'iPOS_HEATB_HOME_H		= 1028				'熱板待機位置(H) (um)

	iPOS_HEATB_L			= 1030 				'Z軸目標位置(L) (um)
	iPOS_HEATB_H				 				'Z軸目標位置(H) (um)
	iPOS_X					= 1032 				'X軸目標位置(um)
	iPOS_Y					= 1033 				'Y軸目標位置(um)
	iPOS_ORDER				= 1034 				'移動指令 b0:Z軸 b1:X軸 b2:Y軸 b3:XY動作 b4:センター出し要求 b5:原点要求 b6:センタ位置書込み要求 b7:脱調確認要求
	iZERO_START_POS_L		= 1035 				'ゼロ点位置 理論値(L)(mm)
	iZERO_START_POS_H			 				'ゼロ点位置 理論値(H)(mm)
	iCENTER_START_POS_L		= 1037 				'中央出し開始位置(L)(mm)
	iCENTER_START_POS_H			 				'中央出し開始位置(H)(mm)

	iTIME_SET_1				= 1040 				'時計設定(年月) ex. 2001 2020年1月
	iTIME_SET_2					 				'時計設定(日時) ex. 1517 15日17時
	iTIME_SET_3					 				'時計設定(分秒) ex. 1200 12分00秒
	iTIME_SET_ORDER			= 1043 				'設定指令
												'bit0:時計設定
												'bit1:製造番号設定
												'bit2:本体電源ON積算時間リセット
												'bit3:熱板ヒーターON積算時間リセット
												'bit4:XY動作積算時間リセット
												'bit5:先端部ヒーターON積算時間リセット
												'bit6:Z無負荷応力設定要求
												'bit7:起歪体ヒーターON積算時間リセット

	iXPOS					= 1044 				'X軸相対位置 ゼロ調
	iYPOS					= 1045 				'Y軸相対位置 ゼロ調

	iSTRESS_DET_ACQUIS_TIME	= 1046 				'デバッグ用 応力検知 Max/Min 取得時間
	iSTRESS_DET_AD_THRESHOLD= 1047 				'デバッグ用 応力検知 A/D閾値
	iSTRESS_DET_DURA_TIME	= 1048 				'デバッグ用 応力検知 継続時間
	iDEBUG_SET				= 1049 				'デバッグ用 Bit0:測定データ保存(1:測定中でなくても測定データを更新する)  20211025
												'			Bit1:PC通信エラー無効


	iPC_ALIVE				= 1050				'PC死活信号
	iTEMP2_PRS				= 1051				'プローブ温度設定値(℃)
	iTMP2_RNG									'プローブ温度許容範囲(℃)
	iHEAT2_P_PID								'プローブPID制御 P 比例定数
	iHEAT2_I_PID								'プローブPID制御 I 積分定数(s)
	iHEAT2_D_PID								'プローブPID制御 D 微分定数(s)
	iHEAT2_DGAIN			= 1056				'プローブPID制御 微分ゲイン
	iHEAT2_PVBIAS								'プローブPID制御 温度補正(℃)
	iHEATER2_LIMIT          = 1058		        'プローブヒーター上限温度(℃)

	iCENT_POS_X_W	        = 1059		        '中央 X軸位置 書込み用
	iCENT_POS_Y_W	        = 1060		        '中央 Y軸位置 書込み用
	iPB_TEMP_AVETIME        = 1061		        'PB温度制御 温度移動平均時間(未使用)

	iHP_PVGAIN		        = 1062		        '熱板		PVGain
	iPB_PVGAIN		        = 1063		        'プローブ	PVGain

	iZERO_COMP_PRESS_L      = 1065		        '測定時圧力 AD値 0-8388607(下位)
	iZERO_COMP_PRESS_H      = 1066		        '測定時圧力 AD値 0-8388607(上位)
	i7SEG_BRIGHTNESS        = 1067		        '7SEG 明るさ 0-15
	iMEASURE_MODE	        = 1068		        '測定モード (0:GAP固定 1:着液応力一定制御 2:応力一定連続制御)

	iAMPLITUDE_Z			= 1069				'Z軸振幅(um)
	iPERIOD_Z				= 1070				'Z軸周期(s)
	iAMPMODE				= 1071				'振幅測定モード 0:振幅一定 1:応力一定(測定モード=0:GAP固定 1:着液応力一定制御のときのみ)
	iPRESSCONT_DELAY		= 1072				'着圧制御待ち時間[mS] * 10
	iPRESSCONT_STEP			= 1073				'着圧制御ステップ数
	iAMPMODE_AMP			= 1074				'応力一定モード時の応力振幅設定P-P x1000

	iZAmpStep				= 1075				'Ｚ振動　振幅ステップ数[um]

	iSG_TEMP_PRS			= 1077				'起歪体 温度設定値（℃）
	iSG_TEMP_RNG			= 1078				'起歪体 温度許容範囲（℃）
	iSG_P_PID				= 1079				'起歪体 PID制御 P 比例定数
	iSG_I_PID				= 1080				'起歪体 PID制御 I 積分定数(s)
	iSG_D_PID				= 1081				'起歪体 PID制御 D 微分定数(s)
	iSG_DGAIN				= 1082				'起歪体 PID制御 微分ゲイン
	iSG_PVBIAS				= 1083				'起歪体 PID制御 温度補正(℃)
	iSG_PVGAIN				= 1084		        '起歪体 PID制御 PVゲイン
	iSG_LIMIT				= 1085		        '起歪体 ヒーター上限温度(℃)

	iZ_UPPER_LIMIT_L		= 1086				'Z軸上限リミット(下位)
	iZ_UPPER_LIMIT_H		= 1087				'Z軸上限リミット(上位) 


	iDEV_ALIVE				= 1100 				'本体CPU死活信号
	iDEV_ERR1				= 1101 				'本体エラー状態1
												'b0:X応力測定レンジ外, 
												'b1:Y応力測定レンジ外,
												'b2:上昇中応力検知,
												'b3:下降中応力検知,
												'b4:EMG STOP
												'b5:HP Over Temp
												'b6:熱板ヒータ昇温異常
												'b7:Up Down Warn
												'b8:Up Down Err
												'b9:Up Down OrgTimeover
												'b10:Up Down No Touch
												'b11:Up Down Pos Err
												'b14:Up Down OrgNoRockStp
												'b15:Max Temp TimeOver

	iDEV_ERR2				= 1102 				'本体エラー状態2
												'b0:No Close 投入位置からのX,Y,Z移動タイミングで扉開
												'b1:EMG STOP(TempOver) -> 熱板過昇温異常(可変抵抗VR1で温度設定)
												'b2:Cover Open -> モータ動作中に扉開
												'b3:Ｙ軸モーター異常, 
												'b4:Ｙ軸移動タイムアウト,
												'b5:Ｙ軸原点センサ異常,
												'b6:FRAMデータ異常,
												'b7:X軸移動中原点検知,
												'b8:Y軸移動中原点検知,
												'b9:下降中原点検知,
												'b10:ﾌﾟﾛｰﾌﾞ 熱電対断線検知, 
												'b11:ﾌﾟﾛｰﾌﾞ ヒーター昇温異常, 
												'b12:ﾌﾟﾛｰﾌﾞ ヒーター高温検知, 
												'b13:X応力センサ通信異常, 
												'b14:Y応力センサ通信異常,
												'b15:Z応力センサ通信異常

	iHEAT_TEMP_ACT			= 1103 				'熱板現在温度(℃)
	iHEAT_TEMP_OPE			= 1104				'熱板温度操作量(％)
	iHEATB_POS_L_ACT		= 1105	 			'熱板位置 下位(um)
	iHEATB_POS_H_ACT		= 1106	 			'熱板位置 上位(um)
	iTOOL_LINK_STS			= 1107				'Tool連携用ｽﾃｰﾀｽ
												'bit0:ｲﾝﾀｰﾛｯｸにより動作不能（ｲﾝﾀｰﾛｯｸ有効時にｶﾊﾞｰ開で移動しようとすると1、ｶﾊﾞｰ閉で0）,
												'bit1:原点復帰未完了により動作不能 （原点復帰未完了時に移動しようとすると1、原点復帰完了で0)

	iDEV_RES				= 1108 				'指令応答ビット
												'b0:測定開始
												'b1:ゼロ調開始
												'b2:熱板移動
												'b3:X軸移動
												'b4:Y軸移動
												'b5:時計設定
												'b6:エラーリセット
												'b7センター出し完了,
												'b8:製造番号設定,
												'b9:本体電源ON積算時間リセット,
												'b10:熱盤ﾋｰﾀON積算時間リセット,
												'b11:センター位置書き込み完了,
												'b12:XY動作積算時間リセット,
												'b13:ﾌﾟﾛｰﾌﾞﾋｰﾀON積算時間リセット,
												'b14:起歪体ﾋｰﾀON積算時間リセット,

	iDEV_STAT1				= 1109 				'本体状態1
												'b0:測定中(STARTﾎﾞﾀﾝ押下後、ｸﾘｱ時間経過後に1になる),
												'b1:測定準備,
												'b2:測定中(STARTﾎﾞﾀﾝ押下後、直ちに1になる),
												'b3:原点復帰 SW状態,
												'b4:エラー解除 SW状態,
												'b5:Z無負荷応力設定応答
												'b6:零調中,
												'b7:,
												'b8:,
												'b9:起歪体ヒータON中,
												'b10:XY動作中,
												'b11:昇降中,
												'b12:ﾌﾟﾛｰﾌﾞ ﾋｰﾀON中,
												'b13:熱盤 ﾋｰﾀON中,
												'b14:熱盤検知中,
												'b15:原点出し完了


	iDEV_TEMP_NRM			= 1110 				'熱板温度範囲内OK
	iDEV_SOFT_VER			= 1111 				'制御基板ソフトバージョン
	iDEV_SEREAL				= 1112 				'本体製造番号
	iDEV_IL					= 1113 				'本体状態2
												'b1:インターロック無効
												'b2:インターロック解除中(ｲﾝﾀｰﾛｯｸ無効設定に関係なく、ｶﾊﾞｰ閉で1)

	iINTEGTIME_POWERON_L	= 1114				'本体電源ON積算時間[秒]（下位）
	iINTEGTIME_POWERON_H						'本体電源ON積算時間[秒]（上位）
	iINTEGTIME_HEATERON_L						'熱板ﾋｰﾀON積算時間[秒]（下位）
	iINTEGTIME_HEATERON_H						'熱板ﾋｰﾀON積算時間[秒]（上位）

	iINTEGRSTDATE_POWERON_L = 1118				'本体電源ON積算時間リセット日[西暦年2桁+月2桁+日2桁]（下位）
	iINTEGRSTDATE_POWERON_H						'本体電源ON積算時間リセット日[西暦年2桁+月2桁+日2桁]（上位）
	iINTEGRSTDATE_HEATERON_L					'熱板ﾋｰﾀON積算時間リセット日[西暦年2桁+月2桁+日2桁]（下位）
	iINTEGRSTDATE_HEATERON_H					'熱板ﾋｰﾀON積算時間リセット日[西暦年2桁+月2桁+日2桁]（上位）

	iCENT_POS_X				= 1122				'中央 X軸位置
	iCENT_POS_Y				= 1123				'中央 Y軸位置

	iCOM_SOFT_VER			= 1124 				'通信基板ソフトバージョン

	iDMV_HP					= 1125				'HP PID制御量 dmv
	iPN_HP					= 1126				'HP PID制御量 pn
	iIN_HP					= 1127				'HP PID制御量 in
	iDN_HP					= 1128				'HP PID制御量 dn

	iDEV_ERR3				= 1129 				'本体エラー状態3
												'b0:X軸 エンコーダ異常
												'b1:Y軸 エンコーダ異常
												'b2:Z軸 エンコーダ異常
												'b3:外部インターロック検知
												'b4:Z応力測定レンジ外
												'b5:プローブ過昇温異常（可変抵抗VR2で温度設定）
												'b6:Z軸 補正 応力センサ測定範囲外
												'b7:Z軸 補正 応力センサ通信異常
												'b8:
												'b9:着液前Z振動開始検知
												'b10:CPU間通信異常
												'b11:SG過昇温異常（可変抵抗VR2で温度設定）
												'b12:起歪体 熱電対断線検知
												'b13:起歪体 ヒータ昇温しない
												'b14:起歪体 ヒータ高温検知


	iMEAS_NO_L				= 1130				'測定№（下位）
	iMEAS_NO_H				= 1131				'測定№（上位）

	iCURR_STRS_X_L			= 1132				'現在 応力X（下位）
	iCURR_STRS_X_H								'現在 応力X（上位）
	iCURR_STRS_Y_L			= 1134				'現在 応力Y（下位）
	iCURR_STRS_Y_H								'現在 応力Y（上位）

	iCURR_POS_X				= 1136				'現在 X軸位置
	iCURR_POS_Y									'現在 Y軸位置

	iMAIN_SPI_OK			= 1138				'メイン基板SPI通信OKカウンタ
	iMAIN_SPI_NG			= 1139				'メイン基板SPI通信NGカウンタ

	iCURR_DATE_1			= 1140				'現在 日時 YYMM
	iCURR_DATE_2			= 1141				'現在 日時 DDHH
	iCURR_DATE_3			= 1142				'現在 日時 MMSS

	iDMV_PB					= 1143				'PB PID制御量 dmv
	iPN_PB					= 1144				'PB PID制御量 pn
	iIN_PB					= 1145				'PB PID制御量 in
	iDN_PB					= 1146				'PB PID制御量 dn

	iCOM_SPI_OK				= 1148				'通信基板SPI通信OKカウンタ
	iCOM_SPI_NG				= 1149				'通信基板SPI通信NGカウンタ

	iHEAT_TEMP2_ACT			= 1150 				'プローブ 現在温度(℃)
	iHEAT_TEMP2_OPE			= 1151				'プローブ 温度操作量(℃)
	iDEV_TEMP2_NRM			= 1152              'プローブ 温度範囲内OK

	iCURR_STRS_Z_L			= 1153              '現在 応力Z補正前（下位）
	iCURR_STRS_Z_H			= 1154              '現在 応力Z補正前（上位）

	iINTEGTIME_XACT_L		= 1155				'X動作積算時間[秒]（下位）
	iINTEGTIME_XACT_H							'X動作積算時間[秒]（上位）
	iINTEGTIME_PBHEATON_L						'PBﾋｰﾀON積算時間[秒]（下位）
	iINTEGTIME_PBHEATON_H						'PBﾋｰﾀON積算時間[秒]（上位）
	iINTEGTIME_YACT_L		= 1159				'Y動作積算時間[秒]（下位）
	iINTEGTIME_YACT_H							'Y動作積算時間[秒]（上位）
	iINTEGTIME_ZACT_L		= 1161				'Z動作積算時間[秒]（下位）
	iINTEGTIME_ZACT_H							'Z動作積算時間[秒]（上位）

	iINTEGRSTDATE_XACT_L	= 1170				'X動作積算時間リセット日[西暦年2桁+月2桁+日2桁]（下位）
	iINTEGRSTDATE_XACT_H						'X動作積算時間リセット日[西暦年2桁+月2桁+日2桁]（上位）
	iINTEGRSTDATE_PBHEATON_L					'PBﾋｰﾀON積算時間リセット日[西暦年2桁+月2桁+日2桁]（下位）
	iINTEGRSTDATE_PBHEATON_H					'PBﾋｰﾀON積算時間リセット日[西暦年2桁+月2桁+日2桁]（上位）


	DBG_X_STRS_OVER_CNT		= 1160				'デバッグ X軸応力センサ範囲外エラーカウント最大
	DBG_Y_STRS_OVER_CNT		= 1161				'デバッグ Y軸応力センサ範囲外エラーカウント最大
	DBG_X_STRS_COME_CNT		= 1162				'デバッグ X軸応力センサ通信エラーカウント最大
	'DBG_Y_STRS_COME_CNT		= 1163				'デバッグ X軸応力センサ通信エラーカウント最大
	CPU_COM_ERR				= 1163				'CPU間通信異常カウント

	Z_CONFIRM_POS_L			= 1168				'着圧決定 Z位置(0.1um) 下位
	Z_CONFIRM_POS_H			= 1169				'着圧決定 Z位置(0.1um) 上位

	iINTEGRSTDATE_YACT_L	= 1174				'Y動作積算時間リセット日[西暦年2桁+月2桁+日2桁]（下位）
	iINTEGRSTDATE_YACT_H						'Y動作積算時間リセット日[西暦年2桁+月2桁+日2桁]（上位）
	iOOSVal_X				= 1176				'脱調誤差 X *0.1um
	iOOSVal_Y				= 1177				'脱調誤差 Y *0.1um
	iOOSVal_Z				= 1178				'脱調誤差 Z *0.1um
	iCURR_STRS_Z_CORR_L		= 1179              '現在 補正用応力Z（下位）
	iCURR_STRS_Z_CORR_H		= 1180              '現在 補正用応力Z（上位）
	iCURR_STRS_Z_CORRA_L	= 1181				'現在 応力Z補正後（下位）
	iCURR_STRS_Z_CORRA_H	= 1182				'現在 応力Z補正後（上位）
	iZERO_POS_Z_L			= 1183				'ゼロ点 Z位置(ゼロ調時更新値) (下位) um
	iZERO_POS_Z_H			= 1184				'ゼロ点 Z位置(ゼロ調時更新値) (上位) um
	iZERO_STRS_Z_L			= 1185				'ゼロ調時無負荷Z応力(下位)
	iZERO_STRS_Z_H			= 1186				'ゼロ調時無負荷Z応力(上位)

	iHEAT_TEMP3_ACT			= 1187 				'起歪体 現在温度(℃)
	iHEAT_TEMP3_OPE			= 1188				'起歪体 温度操作量(℃)
	iDEV_TEMP3_NRM			= 1189              '起歪体 温度範囲内OK

	iINTEGTIME_SGHEATON_L	= 1190				'起歪体ﾋｰﾀON積算時間[秒]（下位）
	iINTEGTIME_SGHEATON_H						'起歪体ﾋｰﾀON積算時間[秒]（上位）
	iINTEGRSTDATE_SGHEATON_L					'起歪体ﾋｰﾀON積算時間リセット日[西暦年2桁+月2桁+日2桁]（下位）
	iINTEGRSTDATE_SGHEATON_H					'起歪体ﾋｰﾀON積算時間リセット日[西暦年2桁+月2桁+日2桁]（上位）

	iDMV_SG					= 1194				'起歪体 PID制御量 dmv
	iPN_SG					= 1195				'起歪体 PID制御量 pn
	iIN_SG					= 1196				'起歪体 PID制御量 in
	iDN_SG					= 1197				'起歪体 PID制御量 dn

	iINTEGRSTDATE_ZACT_L	= 1198				'Z動作積算時間リセット日[西暦年2桁+月2桁+日2桁]（下位）
	iINTEGRSTDATE_ZACT_H						'Z動作積算時間リセット日[西暦年2桁+月2桁+日2桁]（上位）

End Enum
Private CliSndTimeOut	As Int32 = 1000			'クライアント送信タイムアウト(mS)
Private CliRcvTimeOut	As Int32 = 1000			'クライアント受信タイムアウト(mS)

Private cSwitchAmpXYPos	As Int32 = cXYAmpSwitch	'XY位置の振幅設定による解像度切替え閾値(um)


'▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼
'   構造体
'▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲
'Public Structure stMeas
'	Dim iTime		As Int64			'経過時間(mS)
'	Dim iPwX		As UInt32           '応力X(AD値 0-16777215)
'	Dim iPwY		As UInt32           '応力Y(AD値 0-16777215)
'	Dim iPosX		As Int32			'X軸位置
'	Dim iPosY		As Int32			'Y軸位置
'	Dim iHosei		As Int64			'応力補正値
'End Structure

'▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼
'   API宣言
'▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲

'▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼
'   パブリック変数
'▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲
'本体のIPアドレス
Public p_strEquipIP As String

'''<summary> 本体のポート番号。 </summary>
Public p_strEquipPort As String = "1024"

Friend p_strErrState As String = ""

Public m_iRAry(255)											As Int32		'一括読込配列 常時監視(1050-1099)
Public m_iRAryM(8191)										As Int32		'一括読込配列 
Public m_iWAry(W_END_LNK_ADR - W_TOP_LNK_ADR - 1)			As Int32		'一括書込配列


''' <summary>本体との接続状態 0:接続解除 1:接続中</summary>
Public p_iCon							As	Int32 = 0
''' <summary>本体とのLink状態 0:Link解除 1:Link中</summary>
Public p_iLink							As	Int32 = 0

'現在値を仮ゼロにするオフセット値
Public P_StressZeroEnable				As Boolean = False
Public P_XStressZero					As Int32 = 0
Public P_YStressZero					As Int32 = 0
Public P_ZStressZero					As Int32 = 0

'▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼
'   プライベート変数
'▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲

'メンバ変数
Private m_client		As TcpClient = Nothing
Private m_ns			As NetworkStream
Private m_blInitConnect As Boolean = True           '初回接続フラグ

Private m_CurrADMax		As Double					'現在設定のAD最大値

''' <summary>本体リンク状態
''' 0:正常  1:受信側異常 2;送信側異常 3:送受信共に異常 </summary>
Private m_iLinkErrState	As Int16 = 0
Public ReadOnly Property  IsLinkError() As Int16
	Get
		Return m_iLinkErrState
	End Get
End Property

'============================================
'  書込み用変数
’整数で保持。実数で使用する場合はプロパティ
’で引数modeを1にしてアクセスする
'============================================

Private m_iTempPreset As int32						
''' <summary>[1000]熱板温度設定値[℃] 0.0-300.0</summary>
Public Property TempPreset(mode As int32) As Double
	Get
		TempPreset = IIf(mode=0,m_iTempPreset,m_iTempPreset / 10.0)
	End Get
	Set(Value As Double)
		m_iTempPreset = IIf(mode=0,Value,Value*10.0)
	End Set
End Property

Private m_iGapPreset As Int32						
''' <summary>[1001]測定GAP値[um] 0.00-10.00</summary>
Public Property GapPreset(mode As int32) As Double
	Get
		GapPreset = IIf(mode=0,m_iGapPreset,m_iGapPreset / 1000.0)
	End Get
	Set(Value As Double)
		m_iGapPreset = IIf(mode=0,Value,Value*1000.0)
	End Set
End Property


Private m_iMeasWaitTime As int32						
''' <summary>[1002]測定待ち時間[秒] 0.0-100.0</summary>
Public Property MeasWaitTime(mode As int32) As Double
	Get
''		MeasWaitTime = IIf(mode=0,(m_iMeasWaitTime + m_iClearTime),(m_iMeasWaitTime+m_iClearTime) / 10.0)			'新SPEC入力 [投入待ち時間]を足す
		MeasWaitTime = IIf(mode=0,(m_iMeasWaitTime ),(m_iMeasWaitTime) / 10.0)			'ファイルから読み込んだ時の待ち時間と合わせるため、不要な投入待ち時間を足さない
	End Get
	Set(Value As Double)
		m_iMeasWaitTime = IIf(mode=0,Value,Value*10.0)
	End Set
End Property


Private m_iClearTime As int32						
''' <summary>[1003]投入待ち（クリア）時間[秒] 0.0-100.0</summary>
Public Property ClearTime(mode As int32) As Double
	Get
		ClearTime = IIf(mode=0,m_iClearTime,m_iClearTime / 10.0)
	End Get
	Set(Value As Double)
		m_iClearTime = IIf(mode=0,Value,Value*10.0)
	End Set
End Property

Private m_iTempOkRange As int32						
''' <summary>[1004]熱板温度許容範囲[℃] 0.0-10.0</summary>
Public Property TempOkRange(mode As int32) As Double
	Get
		TempOkRange = IIf(mode=0,m_iTempOkRange,m_iTempOkRange / 10.0)
	End Get
	Set(Value As Double)
		m_iTempOkRange = IIf(mode=0,Value,Value*10.0)
	End Set
End Property

Private m_iDevSereal As int32						
''' <summary>[1005]本体製造番号</summary>
Public Property DevSereal() As Int32
	Get
		DevSereal = m_iDevSereal
	End Get
	Set(Value As Int32)
		m_iDevSereal = Value
	End Set
End Property
#If 0
Private m_iHeatLedTemp As int32						
''' <summary>[1006]熱板LED切替温度[℃] 3.0-30.0</summary>
Public Property HeatLedTemp(mode As int32) As Double
	Get
		HeatLedTemp = IIf(mode=0,m_iHeatLedTemp,m_iHeatLedTemp / 10.0)
	End Get
	Set(Value As Double)
		m_iHeatLedTemp = IIf(mode=0,Value,Value*10.0)
	End Set
End Property
#end if
Private m_iLedOnOff As int32						
''' <summary>[1006]照明LED点灯 0:消灯 1:点灯</summary>
Public Property LedOnOff() As int32
	Get
		LedOnOff = m_iLedOnOff
	End Get
	Set(Value As int32)
		m_iLedOnOff = Value
	End Set
End Property

Private m_iP_PID As int32						
''' <summary>[1007]比例定数 P 0.00-60.00</summary>
Public Property P_PID(mode As int32) As Double
	Get
		P_PID = IIf(mode=0,m_iP_PID,m_iP_PID / 100.0)
	End Get
	Set(Value As Double)
		m_iP_PID = IIf(mode=0,Value,Value*100.0)
	End Set
End Property

Private m_iI_PID As int32						
''' <summary>[1008]積分定数 I (秒) 1.0-1000.0</summary>
Public Property I_PID(mode As int32) As Double
	Get
		I_PID = IIf(mode=0,m_iI_PID,m_iI_PID / 10.0)
	End Get
	Set(Value As Double)
		m_iI_PID = IIf(mode=0,Value,Value*10.0)
	End Set
End Property

Private m_iD_PID As int32						
''' <summary>[1009]微分定数 D (秒) 0.0-300.0</summary>
Public Property D_PID(mode As int32) As Double
	Get
		D_PID = IIf(mode=0,m_iD_PID,m_iD_PID / 100.0)
	End Get
	Set(Value As Double)
		m_iD_PID = IIf(mode=0,Value,Value*100.0)
	End Set
End Property

Private m_iDGain As int32						
''' <summary>[1010]微分ゲイン 0.0-300.0</summary>
Public Property DGain(mode As int32) As Double
	Get
		DGain = IIf(mode=0,m_iDGain,m_iDGain / 100.0)
	End Get
	Set(Value As Double)
		m_iDGain = IIf(mode=0,Value,Value*100.0)
	End Set
End Property

Private m_iPVBias As int32						
''' <summary>[1011]温度補正(℃) -10.0-10.0</summary>
Public Property PVBias(mode As int32) As Double
	Get
		PVBias = IIf(mode=0,m_iPVBias,m_iPVBias / 10.0)
	End Get
	Set(Value As Double)
		m_iPVBias = IIf(mode=0,Value,Value*10.0)
	End Set
End Property
	
Private m_iStanbyTime As int32						
''' <summary>(未使用)[1012]スタンバイ時間(秒) 0-60</summary>
Public Property StanbyTime() As Double
	Get
		StanbyTime = m_iStanbyTime
	End Get
	Set(Value As Double)
		m_iStanbyTime = Value
	End Set
End Property
	
Private m_iInOperation As int32						
''' <summary>(未使用)[1013]装置操作中 0-1</summary>
Public Property InOperation() As Double
	Get
		InOperation = m_iInOperation
	End Get
	Set(Value As Double)
		m_iInOperation = Value
	End Set
End Property
	
Private m_iConfig1 As int32						
''' <summary>[1014]
''' 本体動作設定 bit情報
'''	b0:終了時ブザー発報
'''	b1:ヒータレンジ内動作OK　ブザー発報
'''	b2:未使用
'''	b3:カバーIL無効
'''	b4:測定終了ガイド音bit0
'''	b5:測定終了ガイド音bit1
'''	B6:測定中パトライト緑点滅(仮)
'''	B7:
''' </summary>
Public Enum eWConfigBit
	iEndBzz			= &h0001
	iHeatOK			= &h0002
	iCoverILDis		= &h0004
	iILAllStop		= &h0008
	iEndOfPattern0	= &h0000
	iEndOfPattern1	= &h0010
	iEndOfPattern2	= &h0020
	iEndOfPattern3	= &h0030
	iGreenFlash		= &h0040
	iTempLEDEn		= &h0100
End Enum
Public Property Config1() As Int32
	Get
		Config1 =  m_iConfig1
	End Get
	Set(Value As Int32)
		m_iConfig1 = Value
	End Set
End Property
Public Sub AfterMeasureBuzzerSet(ByVal t As Boolean)
	If t
		m_iConfig1 = m_iConfig1 Or eWConfigBit.iEndBzz
	Else
		m_iConfig1 = m_iConfig1 And Not eWConfigBit.iEndBzz
	End If
End Sub
Public Sub TempOKBuzzerSet(ByVal t As Boolean)
	If t
		m_iConfig1 = m_iConfig1 Or eWConfigBit.iHeatOK
	Else
		m_iConfig1 = m_iConfig1 And Not eWConfigBit.iHeatOK
	End If
End Sub
Public Sub InterLockSet(ByVal t As Boolean)
	If t
		m_iConfig1 = m_iConfig1 Or eWConfigBit.iILAllStop
	Else
		m_iConfig1 = m_iConfig1 And Not eWConfigBit.iILAllStop
	End If
End Sub
Public Sub EndOfPattern(byVal iVal As Int16)
	m_iConfig1 = m_iConfig1 And Not iVal
	m_iConfig1 = m_iConfig1 Or iVal
End Sub
'''	B6:測定中パトライト緑点滅(仮)
Public Sub GreenFlashSet(ByVal t As Boolean)
	If t
		m_iConfig1 = m_iConfig1 Or eWConfigBit.iGreenFlash
	Else
		m_iConfig1 = m_iConfig1 And Not eWConfigBit.iGreenFlash
	End If
End Sub

'[1015]温度制御開始 
	'b0:熱板
	'b1:プローブ
	'b2:起歪体
Public Enum eWTempBit
	iHP			= &h0001
	iProbe		= &h0002
	iSG			= &h0004
End Enum
Private m_iStartTempCont As int32						
Public Property StartTempCont() As Int32
	Get
		StartTempCont =  m_iStartTempCont
	End Get
	Set(Value As Int32)
		m_iStartTempCont = Value
	End Set
End Property

'[1016]測定開始 
	'b0:ゼロ調
	'b1:測定
Public Enum eWStartBit
	iZero			= &h0001
	iMeas			= &h0002
End Enum
Private m_iStartSW As int32						
Public Property StartSW() As Int32
	Get
		StartSW =  m_iStartSW
	End Get
	Set(Value As Int32)
		m_iStartSW = Value
	End Set
End Property
Public function IsZeroStartSW() As Boolean
	Return IIf((m_iStartSW And eWStartBit.iZero)<>0, True, False)
End function
Public function IsMeasStartSW() As Boolean
	Return IIf((m_iStartSW And eWStartBit.iMeas)<>0, True, False)
End function
Public sub ZeroSW(ByVal t As Boolean)
	If t
		StartSW = m_iStartSW Or eWStartBit.iZero
	Else
		StartSW = m_iStartSW And Not eWStartBit.iZero
	End If
End sub
Public sub MeasSW(ByVal t As Boolean)
	If t
		StartSW = m_iStartSW Or eWStartBit.iMeas
	Else
		StartSW = m_iStartSW And Not eWStartBit.iMeas
	End If
End sub
'[1017]エラーリセット 
Private m_iErrReset As int32						
Public Property ErrReset() As Int32
	Get
		ErrReset =  m_iErrReset
	End Get
	Set(Value As Int32)
		m_iErrReset = Value
	End Set
End Property
		
'[1018]熱板ヒーター上限温度 0.0-399.0 
Private m_iHeaterUpper As int32						
Public Property HeaterUpper(mode As int32) As Double
	Get
		HeaterUpper = IIf( mode=0, m_iHeaterUpper, m_iHeaterUpper / 10.0) 
	End Get
	Set(Value As Double)
		m_iHeaterUpper = iif( mode=0, Value, Value * 10.0 )
	End Set
End Property


'[1020]投入GAP(mm) 0.0-100.0 
Private m_iInpGap As int32						
Public Property InpGap(mode As int32) As Double
	Get
		InpGap = IIf( mode=0, m_iInpGap, m_iInpGap / 1000.0) 
	End Get
	Set(Value As Double)
		m_iInpGap = iif( mode=0, Value, Value * 1000.0 )
	End Set
End Property
		
''' <summary>[1021]動作待機時間(秒) 0.0-100.0</summary>
Private m_iMesPreWait As int32						
Public Property MesPreWait(mode As int32) As Double
	Get
		MesPreWait = IIf( mode=0, m_iMesPreWait, m_iMesPreWait / 10.0) 
	End Get
	Set(Value As Double)
		m_iMesPreWait = iif( mode=0, Value, Value * 10.0 )
	End Set
End Property

'[1022] XY動作モード 0:X動作(矩形波) 1:X動作(正弦波) 2:XY動作(正弦波)
Public Enum eWActMode
	iRECT			= 0
	iSINE			= 1
	iCIRC			= 2
End Enum
Private m_iActMode As int32						
Public Property ActMode() As Int32
	Get
		ActMode =  m_iActMode
	End Get
	Set(Value As Int32)
		m_iActMode = Value
	End Set
End Property

'[1023]X軸 振幅(um)	0-5000
Private m_iAmplitudeX As int32						
Public Property AmplitudeX() As Int32
	Get
		AmplitudeX = m_iAmplitudeX
	End Get
	Set(Value As Int32)
		m_iAmplitudeX = Value
	End Set
End Property

'[1024]Y軸 振幅(um)	0-5000
Private m_iAmplitudeY As int32						
Public Property AmplitudeY() As Int32
	Get
		AmplitudeY = m_iAmplitudeY
	End Get
	Set(Value As Int32)
		m_iAmplitudeY = Value
	End Set
End Property
		
'[1025]X軸 移動周期(秒) 0.1-10.0
Private m_iPeriodX As int32						
Public Property PeriodX(mode As int32) As Double
	Get
		PeriodX = IIf( mode=0, m_iPeriodX, m_iPeriodX / 10.0) 
	End Get
	Set(Value As Double)
		m_iPeriodX = IIF( mode = 0, Value, Value * 10.0 )
	End Set
End Property

'[1026] Y軸 移動周期(秒) 0.1-10.0
Private m_iPeriodY As int32						
Public Property PeriodY(mode As int32) As Double
	Get
		PeriodY = IIf( mode=0, m_iPeriodY, m_iPeriodY / 10.0) 
	End Get
	Set(Value As Double)
		m_iPeriodY = IIF( mode = 0, Value, Value * 10.0 )
	End Set
End Property

'[1030-1031] Z軸 目標位置(um)	0 - 4294967295
Private m_iPosHeatB As int64						
Public Property PosHeatB() As Int64
	Get
		PosHeatB = m_iPosHeatB
	End Get
	Set(Value As Int64)
		m_iPosHeatB = Value
	End Set
End Property

'[1032] X軸 目標位置(um)	-32767 - 32768
Private m_iPosX As Int32						
Public Property PosX() As Int32
	Get
		PosX = m_iPosX
	End Get
	Set(Value As Int32)
		m_iPosX = Value
	End Set
End Property

'[1033] Y軸 目標位置(um)	-32767 - 32768
Private m_iPosY As Int32						
Public Property PosY() As Int32
	Get
		PosY = m_iPosY
	End Get
	Set(Value As Int32)
		m_iPosY = Value
	End Set
End Property

'[1034] 移動指令
Public Enum eWPosOrderBit
	iZ				= &h0001	'Z軸						b0
	iX				= &h0002	'X軸						b1
	iY				= &h0004	'Y軸						b2
	iXY_MODE		= &h0008	'XYモードに従った動作		b3
	iCENTER			= &h0010	'センター調整指令			b4
	iORIGIN			= &h0020	'原点復帰指令				b5
	iCENTER_W		= &h0040	'センター座標書込み指令		b6
	iOosConf		= &h0080	'脱調確認指令				b7
End Enum
Private m_iPosOrder As Int32						
Public Property PosOrder() As Int32
	Get
		PosOrder = m_iPosOrder
	End Get
	Set(Value As Int32)
		m_iPosOrder = Value
	End Set
End Property
Public function IsPosOrderZ() As Boolean
	Return IIf(m_iPosOrder And eWPosOrderBit.iZ, True, False)
End function
Public function IsPosOrderX() As Boolean
	Return IIf(m_iPosOrder And eWPosOrderBit.iX, True, False)
End function
Public function IsPosOrderY() As Boolean
	Return IIf(m_iPosOrder And eWPosOrderBit.iY, True, False)
End function
Public function IsPosOrderXY() As Boolean
	Return IIf(m_iPosOrder And eWPosOrderBit.iXY_MODE, True, False)
End function
Public function IsPosOrderCenter() As Boolean
	Return IIf(m_iPosOrder And eWPosOrderBit.iCENTER, True, False)
End function
Public function IsPosOrderOrigin() As Boolean
	Return IIf(m_iPosOrder And eWPosOrderBit.iORIGIN, True, False)
End function
Public function IsCenterPosWrite() As Boolean
	Return IIf(m_iPosOrder And eWPosOrderBit.iCENTER_W, True, False)
End function
Public function IsOutOfStepConf() As Boolean
	Return IIf(m_iPosOrder And eWPosOrderBit.iOosConf, True, False)
End function
Public Sub PosOrderZ(ByVal t As Boolean)
	If t
		m_iPosOrder = m_iPosOrder Or eWPosOrderBit.iZ
	Else
		m_iPosOrder = m_iPosOrder And Not eWPosOrderBit.iZ
	End If
End Sub
Public Sub PosOrderX(ByVal t As Boolean)
	If t
		m_iPosOrder = m_iPosOrder Or eWPosOrderBit.iX
	Else
		m_iPosOrder = m_iPosOrder And Not eWPosOrderBit.iX
	End If
End Sub
Public Sub PosOrderY(ByVal t As Boolean)
	If t
		m_iPosOrder = m_iPosOrder Or eWPosOrderBit.iY
	Else
		m_iPosOrder = m_iPosOrder And Not eWPosOrderBit.iY
	End If
End Sub
Public Sub PosOrderXY(ByVal t As Boolean)
	If t
		m_iPosOrder = m_iPosOrder Or eWPosOrderBit.iXY_MODE
	Else
		m_iPosOrder = m_iPosOrder And Not eWPosOrderBit.iXY_MODE
	End If
End Sub
Public Sub PosOrderCenter(ByVal t As Boolean)
	If t
		m_iPosOrder = m_iPosOrder Or eWPosOrderBit.iCENTER
	Else
		m_iPosOrder = m_iPosOrder And Not eWPosOrderBit.iCENTER
	End If
End Sub
Public Sub PosOrderOrigin(ByVal t As Boolean)
	If t
		m_iPosOrder = m_iPosOrder Or eWPosOrderBit.iORIGIN
	Else
		m_iPosOrder = m_iPosOrder And Not eWPosOrderBit.iORIGIN
	End If
End Sub
Public Sub PosOrderCenterWrite(ByVal t As Boolean)
	If t
		m_iPosOrder = m_iPosOrder Or eWPosOrderBit.iCENTER_W
	Else
		m_iPosOrder = m_iPosOrder And Not eWPosOrderBit.iCENTER_W
	End If
End Sub
Public Sub PosOrderOutOfStepConf(ByVal t As Boolean)
	If t
		m_iPosOrder = m_iPosOrder Or eWPosOrderBit.iOosConf
	Else
		m_iPosOrder = m_iPosOrder And Not eWPosOrderBit.iOosConf
	End If
End Sub

'[1035] Z軸 Zero点位置(um)	0 - 4294967295
Private m_iZeroStartPos As int64						
Public Property ZeroStartPos() As Int64
	Get
		ZeroStartPos = m_iZeroStartPos
	End Get
	Set(Value As Int64)
		m_iZeroStartPos = Value
	End Set
End Property

'[1037] Z軸 中央出し開始位置(um)	0 - 4294967295
Private m_iCenterStartPos As int64						
Public Property CenterStartPos() As Int64
	Get
		CenterStartPos = m_iCenterStartPos
	End Get
	Set(Value As Int64)
		m_iCenterStartPos = Value
	End Set
End Property



'[1040-1042] 日時設定
Private m_iTimeSet1 As UInt32						
Private m_iTimeSet2 As UInt32						
Private m_iTimeSet3 As UInt32						
Public Property TimeSet() As UInt32()
	Get
		Dim iTimeSet(2) As UInt32
		iTimeSet(0) = m_iTimeSet1
		iTimeSet(1) = m_iTimeSet2
		iTimeSet(2) = m_iTimeSet3
		TimeSet = iTimeSet
	End Get
	Set(Value() As UInt32)
		m_iTimeSet1 = Value(0)
		m_iTimeSet2 = Value(1)
		m_iTimeSet3 = Value(2)
	End Set
End Property

'[1043]日時設定指令,メンテナンスカウンタリセット指令 
Public Enum eWDateOrderBit
	iDATE_SET					= &h0001	'時計設定
	iDEVICE_SERIAL_WRITE		= &h0002	'本体製造番号書込み指令
	iDEVICE_POWON_TIME_RESET	= &h0004	'本体電源ON積算時間リセット
	iDEVICE_HETON_TIME_RESET	= &h0008	'HPヒータON積算時間リセット
	iDEVICE_XACT_TIME_RESET		= &h0010	'X動作積算時間リセット
	iDEVICE_PBHON_TIME_RESET	= &h0020	'PBヒータON積算時間リセット
	iDEVICE_ZNOLOAD_STRS_SET	= &h0040	'Z無負荷応力設定要求
	iDEVICE_SGHON_TIME_RESET	= &h0080	'起歪体ヒータON積算時間リセット
	iDEVICE_YACT_TIME_RESET		= &h0100	'Y動作積算時間リセット
	iDEVICE_ZACT_TIME_RESET		= &h0200	'Z動作積算時間リセット
End Enum
Private m_iDateSetOrder As Int32						
Public Property DateSetOrder() As Int32
	Get
		DateSetOrder = m_iDateSetOrder
	End Get
	Set(Value As Int32)
		m_iDateSetOrder = Value
	End Set
End Property
Public sub DateSet(ByVal t As Boolean)
	If t
		m_iDateSetOrder = m_iDateSetOrder Or eWDateOrderBit.iDATE_SET
	Else
		m_iDateSetOrder = m_iDateSetOrder And Not eWDateOrderBit.iDATE_SET
	End If
End sub
Public sub SerialWrite(ByVal t As Boolean)
	If t
		m_iDateSetOrder = m_iDateSetOrder Or eWDateOrderBit.iDEVICE_SERIAL_WRITE
	Else
		m_iDateSetOrder = m_iDateSetOrder And Not eWDateOrderBit.iDEVICE_SERIAL_WRITE
	End If
End sub
Public sub PowOnTimeReset(ByVal t As Boolean)
	If t
		m_iDateSetOrder = m_iDateSetOrder Or eWDateOrderBit.iDEVICE_POWON_TIME_RESET
	Else
		m_iDateSetOrder = m_iDateSetOrder And Not eWDateOrderBit.iDEVICE_POWON_TIME_RESET
	End If
End sub
Public sub HeaterOnTimeReset(ByVal t As Boolean)
	If t
		m_iDateSetOrder = m_iDateSetOrder Or eWDateOrderBit.iDEVICE_HETON_TIME_RESET
	Else
		m_iDateSetOrder = m_iDateSetOrder And Not eWDateOrderBit.iDEVICE_HETON_TIME_RESET
	End If
End sub
Public sub XActTimeReset(ByVal t As Boolean)
	If t
		m_iDateSetOrder = m_iDateSetOrder Or eWDateOrderBit.iDEVICE_XACT_TIME_RESET
	Else
		m_iDateSetOrder = m_iDateSetOrder And Not eWDateOrderBit.iDEVICE_XACT_TIME_RESET
	End If
End sub
Public sub YActTimeReset(ByVal t As Boolean)
	If t
		m_iDateSetOrder = m_iDateSetOrder Or eWDateOrderBit.iDEVICE_YACT_TIME_RESET
	Else
		m_iDateSetOrder = m_iDateSetOrder And Not eWDateOrderBit.iDEVICE_YACT_TIME_RESET
	End If
End sub
Public sub ZActTimeReset(ByVal t As Boolean)
	If t
		m_iDateSetOrder = m_iDateSetOrder Or eWDateOrderBit.iDEVICE_ZACT_TIME_RESET
	Else
		m_iDateSetOrder = m_iDateSetOrder And Not eWDateOrderBit.iDEVICE_ZACT_TIME_RESET
	End If
End sub
Public sub PBHOnTimeReset(ByVal t As Boolean)
	If t
		m_iDateSetOrder = m_iDateSetOrder Or eWDateOrderBit.iDEVICE_PBHON_TIME_RESET
	Else
		m_iDateSetOrder = m_iDateSetOrder And Not eWDateOrderBit.iDEVICE_PBHON_TIME_RESET
	End If
End sub
Public sub ZNoLoadStrsSet(ByVal t As Boolean)
	If t
		m_iDateSetOrder = m_iDateSetOrder Or eWDateOrderBit.iDEVICE_ZNOLOAD_STRS_SET
	Else
		m_iDateSetOrder = m_iDateSetOrder And Not eWDateOrderBit.iDEVICE_ZNOLOAD_STRS_SET
	End If
End sub
Public sub SGHOnTimeReset(ByVal t As Boolean)
	If t
		m_iDateSetOrder = m_iDateSetOrder Or eWDateOrderBit.iDEVICE_SGHON_TIME_RESET
	Else
		m_iDateSetOrder = m_iDateSetOrder And Not eWDateOrderBit.iDEVICE_SGHON_TIME_RESET
	End If
End sub

'[1044] X軸 Zero調開始位置(um)	-5000 - +5000
Private m_iZeroAdjPosX As Int64
Public Property ZeroAdjPosX() As Int64
	Get
		ZeroAdjPosX = m_iZeroAdjPosX
	End Get
	Set(Value As Int64)
		m_iZeroAdjPosX = Value
	End Set
End Property
'[1045] Y軸 Zero調開始位置(um)	-5000 - +5000
Private m_iZeroAdjPosY As Int64
Public Property ZeroAdjPosY() As Int64
	Get
		ZeroAdjPosY = m_iZeroAdjPosY
	End Get
	Set(Value As Int64)
		m_iZeroAdjPosY = Value
	End Set
End Property
'[1046] 応力検知 Min/Max 取得時間[x10ms] 0 - 100
Private m_iStressDetectAcquisitionTime As UInt32
Public Property StressDetectAcquisitionTime() As UInt32
	Get
		StressDetectAcquisitionTime = m_iStressDetectAcquisitionTime
	End Get
	Set(Value As UInt32)
		m_iStressDetectAcquisitionTime = Value
	End Set
End Property
'[1047] 応力検知 AD閾値 [x1000LSB] 0 - 16777
Private m_iStressDetectADThreshold As UInt32
Public Property StressDetectADThreshold() As UInt32
	Get
		StressDetectADThreshold = m_iStressDetectADThreshold
	End Get
	Set(Value As UInt32)
		m_iStressDetectADThreshold = Value
	End Set
End Property
'[1048] 応力検知 継続時間 [x10mS] 0 - 100
Private m_iStressDetectDurationTime As UInt32
Public Property StressDetectDurationTime() As UInt32
	Get
		StressDetectDurationTime = m_iStressDetectDurationTime
	End Get
	Set(Value As UInt32)
		m_iStressDetectDurationTime = Value
	End Set
End Property
'[1049] デバッグセット
Public Enum eWDebugSetBit
	iAlwayMeasRecv				= &h0001	'常時測定データ受信
	iPCComErrorNoUse			= &h0002	'PC通信エラー無効
End Enum
Private m_iDebugSet As Int32
Public Property DebugSet() As Int32
	Get
		DebugSet = m_iDebugSet
	End Get
	Set(Value As Int32)
		m_iDebugSet = Value
	End Set
End Property
Public sub AlWaysMeasDataRecv(ByVal t As Boolean)
	If t
		m_iDebugSet = m_iDebugSet Or eWDebugSetBit.iAlwayMeasRecv
	Else
		m_iDebugSet = m_iDebugSet And Not eWDebugSetBit.iAlwayMeasRecv
	End If
End sub
Public sub PCComErrorNoUse(ByVal t As Boolean)
	If t
		m_iDebugSet = m_iDebugSet Or eWDebugSetBit.iPCComErrorNoUse
	Else
		m_iDebugSet = m_iDebugSet And Not eWDebugSetBit.iPCComErrorNoUse
	End If
End sub

'[1050] PC死活信号 
Private m_iPcAlive As Int32						
Public WriteOnly Property PCAlive() As Int32
	Set(Value As int32)
		m_iPcAlive = Value
	End Set
End Property
Private m_iTemp2Preset As int32						
''' <summary>[1051]プローブ温度設定値[℃] 0.0-300.0</summary>
Public Property Temp2Preset(mode As int32) As Double
	Get
		Temp2Preset = IIf(mode=0,m_iTemp2Preset,m_iTemp2Preset / 10.0)
	End Get
	Set(Value As Double)
		m_iTemp2Preset = IIf(mode=0,Value,Value*10.0)
	End Set
End Property
Private m_iTemp2OkRange As int32						
''' <summary>[1052]プローブ温度許容範囲[℃] 0.0-10.0</summary>
Public Property Temp2OkRange(mode As int32) As Double
	Get
		Temp2OkRange = IIf(mode=0,m_iTemp2OkRange,m_iTempOkRange / 10.0)
	End Get
	Set(Value As Double)
		m_iTemp2OkRange = IIf(mode=0,Value,Value*10.0)
	End Set
End Property
Private m_iP_PID2 As int32						
''' <summary>[1053]プローブ比例定数 P 0.00-60.00</summary>
Public Property P_PID2(mode As int32) As Double
	Get
		P_PID2 = IIf(mode=0,m_iP_PID2,m_iP_PID2 / 100.0)
	End Get
	Set(Value As Double)
		m_iP_PID2 = IIf(mode=0,Value,Value*100.0)
	End Set
End Property

Private m_iI_PID2 As int32						
''' <summary>[1054]プローブ積分定数 I (秒) 1.0-1000.0</summary>
Public Property I_PID2(mode As int32) As Double
	Get
		I_PID2 = IIf(mode=0,m_iI_PID2,m_iI_PID2 / 10.0)
	End Get
	Set(Value As Double)
		m_iI_PID2 = IIf(mode=0,Value,Value*10.0)
	End Set
End Property

Private m_iD_PID2 As int32						
''' <summary>[1055]プローブ微分定数 D (秒) 0.0-300.0</summary>
Public Property D_PID2(mode As int32) As Double
	Get
		D_PID2 = IIf(mode=0,m_iD_PID2,m_iD_PID2 / 100.0)
	End Get
	Set(Value As Double)
		m_iD_PID2 = IIf(mode=0,Value,Value*100.0)
	End Set
End Property

Private m_iDGain2 As int32						
''' <summary>[1056]プローブ微分ゲイン 0.0-300.0</summary>
Public Property DGain2(mode As int32) As Double
	Get
		DGain2 = IIf(mode=0,m_iDGain2,m_iDGain2 / 100.0)
	End Get
	Set(Value As Double)
		m_iDGain2 = IIf(mode=0,Value,Value*100.0)
	End Set
End Property

Private m_iPVBias2 As int32						
''' <summary>[1057]プローブ温度補正(℃) -10.0-10.0</summary>
Public Property PVBias2(mode As int32) As Double
	Get
		PVBias2 = IIf(mode=0,m_iPVBias2,m_iPVBias2 / 10.0)
	End Get
	Set(Value As Double)
		m_iPVBias2 = IIf(mode=0,Value,Value*10.0)
	End Set
End Property

'[1058]プローブ ヒーター上限温度 0.0-399.0 
Private m_iHeaterUpper2 As int32						
Public Property HeaterUpper2(mode As int32) As Double
	Get
		HeaterUpper2 = IIf( mode=0, m_iHeaterUpper2, m_iHeaterUpper2 / 10.0) 
	End Get
	Set(Value As Double)
		m_iHeaterUpper2 = iif( mode=0, Value, Value * 10.0 )
	End Set
End Property

'[1059] センター位置X 書込み用 -32766-32767um
Private m_iCentPosX_W As int32						
Public Property CentPosX_W() As Int32
	Get
		CentPosX_W = m_iCentPosX_W
	End Get
	Set(Value As Int32)
		m_iCentPosX_W = Value
	End Set
End Property
'[1060] センター位置Y 書込み用 -32766-32767um
Private m_iCentPosY_W As int32						
Public Property CentPosY_W() As Int32
	Get
		CentPosY_W = m_iCentPosY_W
	End Get
	Set(Value As Int32)
		m_iCentPosY_W = Value
	End Set
End Property
''[1061] PB温度制御 温度移動平均時間(Sec) 0.1-100.0
'Private m_dPbTempAveTime As Double						
'Public Property PbTempAveTime(mode As int32) As Double
'	Get
'		PbTempAveTime = Iif(mode=0, m_dPbTempAveTime, m_dPbTempAveTime/10.0)
'	End Get
'	Set(Value As Double)
'		m_dPbTempAveTime = Iif(mode=0, Value, Value*10.0)
'	End Set
'End Property

'[1062] 熱板PVGain 0 - 6.5532
Private m_dPVGain As Double						
Public Property PVGain(mode As int32) As Double
	Get
		PVGain = Iif(mode=0, m_dPVGain, m_dPVGain/10000.0)
	End Get
	Set(Value As Double)
		m_dPVGain = Iif(mode=0, Value, Value*10000.0)
	End Set
End Property

'[1063] プローブPVGain 0 - 6.5532
Private m_dPVGain2 As Double						
Public Property PVGain2(mode As int32) As Double
	Get
		PVGain2 = Iif(mode=0, m_dPVGain2, m_dPVGain2/10000.0)
	End Get
	Set(Value As Double)
		m_dPVGain2 = Iif(mode=0, Value, Value*10000.0)
	End Set
End Property

'[1064] ゼロ調モード
''Public Enum eZeroModeOrderBit
''	iXCONT					= &h0001	'X連続
''	iZCONT					= &h0002	'Z連続
''	iZSING					= &h0004	'Z単発
''	iZNONE					= &h0008	'ゼロ調なし
''End Enum
''Private m_iZeroAdjMode As Int32
''Public Property ZeroAdjMode() As Int32
''	Get
''		ZeroAdjMode = m_iZeroAdjMode
''	End Get
''	Set(Value As Int32)
''		m_iZeroAdjMode = Value
''	End Set
''End Property

'[1065-66] 測定時圧力(Pa) AD値(dig) 0 - 8388607
Private m_iMeasureStress As Int64
Public Property MeasureStress() As Int64
	Get
		MeasureStress = m_iMeasureStress
	End Get
	Set(Value As Int64)
		m_iMeasureStress = Value
	End Set
End Property

'[1067] 7SEGLEDの明るさ 0 - 15(明るい) 
Private m_i7SegBrightness As Int32
Public Property SSegBrightness() As Int32
	Get
		SSegBrightness = m_i7SegBrightness
	End Get
	Set(Value As Int32)
		m_i7SegBrightness = Value
	End Set
End Property
'[1068] 測定モード (0:GAP固定 1:着液応力一定制御 2:応力一定連続制御)
Private m_iMeasureMode As Int64
Public Property MeasureMode() As Int64
	Get
		MeasureMode = m_iMeasureMode
	End Get
	Set(Value As Int64)
		m_iMeasureMode = Value
	End Set
End Property

'[1069]Z軸 振幅(um)	0-10000
Private m_iAmplitudeZ As int32						
Public Property AmplitudeZ() As Int32
	Get
		AmplitudeZ = m_iAmplitudeZ
	End Get
	Set(Value As Int32)
		m_iAmplitudeZ = Value
	End Set
End Property

'[1070]Z軸 移動周期(秒) 0.2-10.0
Private m_iPeriodZ As int32						
Public Property PeriodZ(mode As int32) As Double
	Get
		PeriodZ = IIf( mode=0, m_iPeriodZ, m_iPeriodZ / 10.0) 
	End Get
	Set(Value As Double)
		m_iPeriodZ = IIF( mode = 0, Value, Value * 10.0 )
	End Set
End Property

'[1071]振幅測定モード 0:振幅一定 1:応力一定
Private m_iAmpMode As int32 = 0
Public Property AmpMode As Int32
	Get
		AmpMode = m_iAmpMode
	End Get
	Set(Value As Int32)
		m_iAmpMode = Value
	End Set
End Property

'[1072]着圧制御待ち時間[mS] 0 - 655350
Private m_iPressContDelay As int32						
Public Property PressContDelay() As int32
	Get
		PressContDelay = m_iPressContDelay
	End Get
	Set(Value As int32)
		m_iPressContDelay = Value / 10.0
	End Set
End Property

'[1073]着圧制御ステップ数 0 - 65535
Private m_iPressContStep As int32						
Public Property PressContStep() As int32
	Get
		PressContStep = m_iPressContStep
	End Get
	Set(Value As int32)
		m_iPressContStep = Value
	End Set
End Property

'[1074]振動測定 応力振幅設定 p-p値 x1000
Private m_iAmpModeAmplitude	As Int32 = 0
Public Property AmpModeAmplitude() As int32
	Get
		AmpModeAmplitude = m_iAmpModeAmplitude * 1000
	End Get
	Set(Value As int32)
		m_iAmpModeAmplitude = Value \ 1000
	End Set
End Property

'[1075]Ｚ振動　振幅ステップ数 um
Private m_iZAmpStep	As Int32 = 1
Public Property ZAmpStep() As int32
	Get
		ZAmpStep = m_iZAmpStep
	End Get
	Set(Value As int32)
		m_iZAmpStep = Value
	End Set
End Property

Private m_iTemp3Preset As int32						
''' <summary>[1077]起歪体 温度設定値[℃] 0.0-300.0</summary>
Public Property Temp3Preset(mode As int32) As Double
	Get
		Temp3Preset = IIf(mode=0,m_iTemp3Preset,m_iTemp3Preset / 10.0)
	End Get
	Set(Value As Double)
		m_iTemp3Preset = IIf(mode=0,Value,Value*10.0)
	End Set
End Property
Private m_iTemp3OkRange As int32						
''' <summary>[1078]起歪体 温度許容範囲[℃] 0.0-10.0</summary>
Public Property Temp3OkRange(mode As int32) As Double
	Get
		Temp3OkRange = IIf(mode=0,m_iTemp3OkRange,m_iTemp3OkRange / 10.0)
	End Get
	Set(Value As Double)
		m_iTemp3OkRange = IIf(mode=0,Value,Value*10.0)
	End Set
End Property
Private m_iP_PID3 As int32						
''' <summary>[1079]起歪体 比例定数 P 0.00-60.00</summary>
Public Property P_PID3(mode As int32) As Double
	Get
		P_PID3 = IIf(mode=0,m_iP_PID3,m_iP_PID3 / 100.0)
	End Get
	Set(Value As Double)
		m_iP_PID3 = IIf(mode=0,Value,Value*100.0)
	End Set
End Property

Private m_iI_PID3 As int32						
''' <summary>[1080]起歪体 積分定数 I (秒) 1.0-1000.0</summary>
Public Property I_PID3(mode As int32) As Double
	Get
		I_PID3 = IIf(mode=0,m_iI_PID3,m_iI_PID3 / 10.0)
	End Get
	Set(Value As Double)
		m_iI_PID3 = IIf(mode=0,Value,Value*10.0)
	End Set
End Property

Private m_iD_PID3 As int32						
''' <summary>[1081]起歪体 微分定数 D (秒) 0.0-300.0</summary>
Public Property D_PID3(mode As int32) As Double
	Get
		D_PID3 = IIf(mode=0,m_iD_PID3,m_iD_PID3 / 100.0)
	End Get
	Set(Value As Double)
		m_iD_PID3 = IIf(mode=0,Value,Value*100.0)
	End Set
End Property

Private m_iDGain3 As int32						
''' <summary>[1082]起歪体 微分ゲイン 0.0-300.0</summary>
Public Property DGain3(mode As int32) As Double
	Get
		DGain3 = IIf(mode=0,m_iDGain3,m_iDGain3 / 100.0)
	End Get
	Set(Value As Double)
		m_iDGain3 = IIf(mode=0,Value,Value*100.0)
	End Set
End Property

Private m_iPVBias3 As int32						
''' <summary>[1083]起歪体 温度補正(℃) -10.0-10.0</summary>
Public Property PVBias3(mode As int32) As Double
	Get
		PVBias3 = IIf(mode=0,m_iPVBias3,m_iPVBias3 / 10.0)
	End Get
	Set(Value As Double)
		m_iPVBias3 = IIf(mode=0,Value,Value*10.0)
	End Set
End Property

Private m_dPVGain3 As Double						
''' <summary>[1084]起歪体 PVGain 0 - 6.5532</summary>
Public Property PVGain3(mode As int32) As Double
	Get
		PVGain3 = Iif(mode=0, m_dPVGain3, m_dPVGain3/10000.0)
	End Get
	Set(Value As Double)
		m_dPVGain3 = Iif(mode=0, Value, Value*10000.0)
	End Set
End Property

Private m_iHeaterUpper3 As int32						
''' <summary>[1085] 起歪体 ヒーター上限温度 0.0-399.0</summary>
Public Property HeaterUpper3(mode As int32) As Double
	Get
		HeaterUpper3 = IIf( mode=0, m_iHeaterUpper3, m_iHeaterUpper3 / 10.0) 
	End Get
	Set(Value As Double)
		m_iHeaterUpper3 = iif( mode=0, Value, Value * 10.0 )
	End Set
End Property

'[1086-87]Z軸 上限リミット[um]
Private m_iZUpperLimit As int64						
Public Property ZUpperLimit() As int64
	Get
		ZUpperLimit = m_iZUpperLimit
	End Get
	Set(Value As int64)
		m_iZUpperLimit = Value
	End Set
End Property


'============================================
' 読込み用変数
’整数で保持。実数で使用する場合はプロパティ
’で引数modeを1にしてアクセスする
'============================================

'[1100] 本体CPU死活信号 
Private m_iDevAlive As Int32						
Public readonly Property DevAlive() As Int32
	Get
		DevAlive = m_iDevAlive
	End Get
End Property

'[1101]本体エラー状態1
	'b0:X stress range over		X応力レンジ外
	'b1:Y stress range over		Y応力レンジ外
	'b2:Stress detect UP		応力検知 上昇中
	'b3:Stress detect DOWN		応力検知 下降中

	'b4:EMG STOP				非常停止SW
	'b5:HP Thermo discon		熱板 熱電対断線
	'b6:HP No Heat Up			熱板 ヒータ昇温異常
	'b7:Y Up Limit				Y軸 上限リミット検知

	'b8:Z Motor Alarm			Zモータ異常
	'b9:Z Move TimeOut			Z移動タイムアウト
	'b10:X Up Limit				X軸 上限リミット検知
	'b11:X Motor Alarm			Xモータ異常

	'b12:X Move Time out		X移動タイムアウト
	'b13:X No Origin			X軸原点センサ異常
	'b14:Z No Origin			Z軸原点センサ異常
	'b15:Heater High Detect		熱板 ヒータ高温検知
Private m_iDevErr1 As Int32
Public Enum eRErr1Bit
	iXStressROver		= &h0001
	iYStressROver		= &h0002
	iStressDetectUp		= &h0004
	iStressDetectDown	= &h0008

	iEmgStp				= &h0010
	iThermoDiscon		= &h0020
	iNoHeatUp			= &h0040
	iYUpLimit			= &h0080

	iZMotorAlm			= &h0100
	iZMoveTimeout		= &h0200
	iXUpLimit			= &h0400
	iXMotorAlm			= &h0800

	iXMoveTimeout		= &h1000
	iXNoOrigin			= &h2000
	iZNoOrigin			= &h4000
	iHeaterHighDetect	= &h8000
End Enum
Public ReadOnly Property DevErr1() As Int32
	Get
		DevErr1 = m_iDevErr1
	End Get
End Property
Public function isErrXStressOver() As Boolean
	Return iif((m_iDevErr1 And eRErr1Bit.iXStressROver) <> 0, True, False ) 
End function
Public function isErrYStressOver() As Boolean
	Return iif((m_iDevErr1 And eRErr1Bit.iYStressROver) <> 0, True, False ) 
End function
Public function isErrStressDetectUp() As Boolean
	Return iif((m_iDevErr1 And eRErr1Bit.iStressDetectUp) <> 0, True, False ) 
End function
Public function isErrStressDetectDown() As Boolean
	Return iif((m_iDevErr1 And eRErr1Bit.iStressDetectDown) <> 0, True, False ) 
End function

Public function isErrEmgStp() As Boolean
	Return iif((m_iDevErr1 And eRErr1Bit.iEmgStp) <> 0, True, False ) 
End function
Public function isErrThermoDiscon() As Boolean
	Return iif((m_iDevErr1 And eRErr1Bit.iThermoDiscon) <> 0, True, False ) 
End function
Public function isErrNoHeatUp() As Boolean
	Return iif((m_iDevErr1 And eRErr1Bit.iNoHeatUp) <> 0, True, False ) 
End function
Public function isErrYUpLimit() As Boolean
	Return iif((m_iDevErr1 And eRErr1Bit.iYUpLimit) <> 0, True, False ) 
End function

Public function isErrZMotorAlm() As Boolean
	Return iif((m_iDevErr1 And eRErr1Bit.iZMotorAlm) <> 0, True, False ) 
End function
Public function isErrZMoveTimeout() As Boolean
	Return iif((m_iDevErr1 And eRErr1Bit.iZMoveTimeout) <> 0, True, False ) 
End function
Public function isErrXUpLimit() As Boolean
	Return iif((m_iDevErr1 And eRErr1Bit.iXUpLimit) <> 0, True, False ) 
End function
Public function isErrXMotorAlm() As Boolean
	Return iif((m_iDevErr1 And eRErr1Bit.iXMotorAlm) <> 0, True, False ) 
End function

Public function isErrXMoveTimeout() As Boolean
	Return iif((m_iDevErr1 And eRErr1Bit.iXMoveTimeout) <> 0, True, False ) 
End function
Public function isErrXNoOrigin() As Boolean
	Return iif((m_iDevErr1 And eRErr1Bit.iXNoOrigin) <> 0, True, False ) 
End function
Public function isErrZNoOrigin() As Boolean
	Return iif((m_iDevErr1 And eRErr1Bit.iZNoOrigin) <> 0, True, False ) 
End function
Public function isErrHeaterHighDetect() As Boolean
	Return iif((m_iDevErr1 And eRErr1Bit.iHeaterHighDetect) <> 0, True, False ) 
End function

'[1102]本体エラー状態2
	'b0:No Close					熱板上昇開始タイミングで扉開
	'b1:HP Overheat					熱板過昇温異常
	'b2:Cover Open					測定中に扉開
	'b3:Y Motor Alarm				Y軸モータ異常

	'b4:Y Move TimeOut				Y軸移動タイムアウト
	'b5:Y No Origin					Y軸原点センサ異常
	'b6:BU Area erased				バックアップ領域クリア
	'b7:X Move Alarm				X軸移動中原点検知(移動中原点検知)

	'b8:Y Move Alarm				Y軸移動中原点検知(移動中原点検知)
	'b9:Z Move Alarm				Z軸移動中原点検知(下降中原点検知)
	'b10:PB Thermo discon			プローブ 熱電対断線
	'b11:PB No Heat Up				プローブ ヒータ断線

	'b12:PB HEATER High	　			プローブ ヒータ高温検知
	'b13:X Stress Com				X応力センサとの通信異常
	'b14:Y Stress Com				Y応力センサとの通信異常
	'b15:Z Stress Com				Z応力センサとの通信異常

Private m_iDevErr2 As Int32						
Public Enum eRErr2Bit
	iNoClose			= &h0001
	iHPOverheat			= &h0002
	iCoverOpen			= &h0004
	iYMotorAlm			= &h0008		

	iYMoveTimeout		= &h0010
	iYNoOrigin			= &h0020
	iBUAreaErased		= &h0040
	iXMoveAlarm			= &h0080

	iYMoveAlarm			= &h0100
	iZMoveAlarm			= &h0200
	iThermoDiscon2		= &h0400
	iNoHeatUp2			= &h0800

	iHeaterHighDetect2	= &h1000
	iXStressCom			= &h2000
	iYStressCom			= &h4000
	iZStressCom			= &h8000

End Enum
Public ReadOnly Property DevErr2() As Int32
	Get
		DevErr2 = m_iDevErr2
	End Get
End Property
Public function isErrNoClose() As Boolean
	Return iif((m_iDevErr2 And eRErr2Bit.iNoClose) <> 0, True, False ) 
End function
Public function isErrHPOverheat() As Boolean
	Return iif((m_iDevErr2 And eRErr2Bit.iHPOverheat) <> 0, True, False ) 
End function
Public function isErrCoverOpen() As Boolean
	Return iif((m_iDevErr2 And eRErr2Bit.iCoverOpen) <> 0, True, False ) 
End function
Public function isErrYMotorAlm() As Boolean
	Return iif((m_iDevErr2 And eRErr2Bit.iYMotorAlm) <> 0, True, False ) 
End function
Public function isErrYMoveTimeout() As Boolean
	Return iif((m_iDevErr2 And eRErr2Bit.iYMoveTimeout) <> 0, True, False ) 
End function
Public function isErrYNoOrigin() As Boolean
	Return iif((m_iDevErr2 And eRErr2Bit.iYNoOrigin) <> 0, True, False ) 
End function
Public function isErrBUAreaErased() As Boolean
	Return iif((m_iDevErr2 And eRErr2Bit.iBUAreaErased) <> 0, True, False ) 
End function

Public function isErrXMoveAlarm() As Boolean
	Return iif((m_iDevErr2 And eRErr2Bit.iXMoveAlarm) <> 0, True, False ) 
End function
Public function isErrYMoveAlarm() As Boolean
	Return iif((m_iDevErr2 And eRErr2Bit.iYMoveAlarm) <> 0, True, False ) 
End function
Public function isErrZMoveAlarm() As Boolean
	Return iif((m_iDevErr2 And eRErr2Bit.iZMoveAlarm) <> 0, True, False ) 
End function
Public function isErrTermoDiscon2() As Boolean
	Return iif((m_iDevErr2 And eRErr2Bit.iThermoDiscon2) <> 0, True, False ) 
End function
Public function isErrNoHeatUp2() As Boolean
	Return iif((m_iDevErr2 And eRErr2Bit.iNoHeatUp2) <> 0, True, False ) 
End function
Public function isErrHeaterHighDetect2() As Boolean
	Return iif((m_iDevErr2 And eRErr2Bit.iHeaterHighDetect2) <> 0, True, False ) 
End function
Public function isErrXStressCom() As Boolean
	Return iif((m_iDevErr2 And eRErr2Bit.iXStressCom) <> 0, True, False ) 
End function
Public function isErrYStressCom() As Boolean
	Return iif((m_iDevErr2 And eRErr2Bit.iYStressCom) <> 0, True, False ) 
End function
Public function isErrZStressCom() As Boolean
	Return iif((m_iDevErr2 And eRErr2Bit.iZStressCom) <> 0, True, False ) 
End function

'熱板現在温度(℃) [1103]
Private m_iHeatTempAct As Int32						
Public ReadOnly Property HeatTempAct() As Double
	Get
		HeatTempAct = m_iHeatTempAct / 10.0
	End Get
End Property

'熱板温度操作量(%) [1104]
Private m_iHeatTempOpe As Int32						
Public ReadOnly Property HeatTempOpe() As Double
	Get
		HeatTempOpe = m_iHeatTempOpe / 10.0
	End Get
End Property

'1055 熱板現在位置(um) [1105]
Private m_iHeatBPos As Int64						
Public ReadOnly Property HeatBPos() As Int64
	Get
		HeatBPos = m_iHeatBPos
	End Get
End Property

'Tool連携用ステータス [1107]
Private m_iToolLinkStatus As Int32						
'ビット [1107]
	'b0:インターロックにより動作不能
	'b1:原点復帰未完了のため動作不能
	'b2:XYZ振動数設定変更中(負論理)
Public Enum eToolSts
	iItLock				= &h0001
	iNoOrigin			= &h0002
	iFreqUpdate			= &h0004
End Enum
Public ReadOnly Property ToolLinkStatus() As Int32
	Get
		ToolLinkStatus = m_iToolLinkStatus
	End Get
End Property
Public ReadOnly Property isItLock() As Boolean
	Get
		isItLock = If((m_iToolLinkStatus And eToolSts.iitlock) <> 0, True, False)
	End Get
End property
Public ReadOnly Property isNoOrigin() As Boolean
	Get
		isNoOrigin = If((m_iToolLinkStatus And eToolSts.iNoOrigin) <> 0, True, False)
	End Get
End property
Public ReadOnly Property isFreqUpdate() As Boolean
	Get
		isFreqUpdate = If((m_iToolLinkStatus And eToolSts.iFreqUpdate) <> 0, False, True)
	End Get
End property

'指令応答ビット [1108]
	'b0:測定開始
	'b1:ゼロ調開始
	'b2:熱板移動
	'b3:X軸移動
	'b4:Y軸移動
	'b5:時計設定
	'b6:エラーリセット
	'b7:センター出し
	'b8:製造番号書込み
	'b9:本体電源ON積算時間リセット
	'b10:本体ヒータON積算時間リセット
	'b11:センター位置書込み完了
	'b12:X動作積算時間リセット
	'b13:PBヒータON積算時間リセット
	'b14:起歪体 ヒータON積算時間リセット
	'b15:Y動作積算時間リセット
Private m_iDevRes As Int32						
Public Enum eRRespBit
	iMeasStart			= &h0001
	iZeroStart			= &h0002
	iHeatMove			= &h0004
	iXAxsisMove			= &h0008
	iYAxsisMove			= &h0010
	iDateSet			= &h0020
	iErrReset			= &h0040
	iCenterPos			= &h0080
	iDevSerial			= &h0100
	iPowOnTimeReset		= &h0200
	iHetOnTimeReset		= &h0400
	iCentPosWrite		= &h0800
	iXActTimeReset		= &h1000
	iPbHeatTimeReset	= &h2000
	iSGHeatTimeReset	= &h4000
	iYActTimeReset		= &h8000
End Enum
Public ReadOnly Property DevRes() As Int32
	Get
		DevRes = m_iDevRes
	End Get
End Property
Public function isRespMeasStart() As Boolean
	Return iif((m_iDevRes And eRRespBit.iMeasStart) <> 0, True, False ) 
End function
Public function isRespZeroStart() As Boolean
	Return iif((m_iDevRes And eRRespBit.iZeroStart) <> 0, True, False ) 
End function
Public function isRespHeatMove() As Boolean
	Return iif((m_iDevRes And eRRespBit.iHeatMove) <> 0, True, False ) 
End function
Public function isRespXAxsisMove() As Boolean
	Return iif((m_iDevRes And eRRespBit.iXAxsisMove) <> 0, True, False ) 
End function
Public function isRespYAxsisMove() As Boolean
	Return iif((m_iDevRes And eRRespBit.iYAxsisMove) <> 0, True, False ) 
End function
Public function isRespDateSet() As Boolean
	Return iif((m_iDevRes And eRRespBit.iDateSet) <> 0, True, False ) 
End function
Public function isRespErrReset() As Boolean
	Return iif((m_iDevRes And eRRespBit.iErrReset) <> 0, True, False ) 
End function
Public function isRespCenterPos() As Boolean
	Return iif((m_iDevRes And eRRespBit.iCenterPos) <> 0, True, False ) 
End function
Public function isDevSerial() As Boolean
	Return iif((m_iDevRes And eRRespBit.iDevSerial) <> 0, True, False ) 
End function
Public function isPowOnTimeReset() As Boolean
	Return iif((m_iDevRes And eRRespBit.iPowOnTimeReset) <> 0, True, False ) 
End function
Public function isHetOnTimeReset() As Boolean
	Return iif((m_iDevRes And eRRespBit.iHetOnTimeReset) <> 0, True, False ) 
End function
Public function isCentPosWrite() As Boolean
	Return iif((m_iDevRes And eRRespBit.iCentPosWrite) <> 0, True, False ) 
End function
Public function isXActTimeReset() As Boolean
	Return iif((m_iDevRes And eRRespBit.iXActTimeReset) <> 0, True, False ) 
End function
Public function isYActTimeReset() As Boolean
	Return iif((m_iDevRes And eRRespBit.iYActTimeReset) <> 0, True, False ) 
End function
Public function isPbHeatOnTimeReset() As Boolean
	Return iif((m_iDevRes And eRRespBit.iPbHeatTimeReset) <> 0, True, False ) 
End function
Public function isSGHeatOnTimeReset() As Boolean
	Return iif((m_iDevRes And eRRespBit.iSGHeatTimeReset) <> 0, True, False ) 
End function

'本体状態1 [1109]
	'b0:測定中
	'b1:測定準備
	'b2:測定中(スタートボタン押下後直ちに１)
	'b3:原点復帰	SW状態
	'b4:エラー解除	SW状態
	'b5:Z無負荷応力設定応答
	'b6:ゼロ調中
	'b7:脱調確認応答
	'b8:Z動作積算時間リセット
	'b9:ヒータON中 起歪体 
	'b10:XY動作中
	'b11:昇降中
	'b12:ヒータON中 Probe
	'b13:ヒータON中 HP
	'b14:熱板検知中
	'b15:原点復帰完了
Private m_iDevStat1 As Int32						
Public Enum eRStat1Bit
	iMeasuring			= &h0001
	iPreMeasure			= &h0002
	iMeasuringTempo		= &h0004
	iOriginSWState		= &h0008
	iResetSWState		= &h0010
	iZNoLoadStress		= &h0020
	iZeroAdjusting		= &h0040
	iOosConf			= &h0080
	iZActTimeReset		= &h0100
	iHeaterOn3			= &h0200
	iXYAction			= &h0400
	iGoingUpDown		= &h0800
	iHeaterOn2			= &h1000
	iHeaterOn			= &h2000
	iHeatPanelDetectComp= &h4000
	iOriginComp			= &h8000
End Enum
Public ReadOnly Property DevStat1() As Int32
	Get
		DevStat1 = m_iDevStat1
	End Get
End Property
Public function isStatMeasuring() As Boolean
	Return iif((m_iDevStat1 And eRStat1Bit.iMeasuring) <> 0, True, False ) 
End function
Public function isStatPreMeasure() As Boolean
	Return iif((m_iDevStat1 And eRStat1Bit.iPreMeasure) <> 0, True, False ) 
End function
Public function isStatOriginSW() As Boolean
	Return iif((m_iDevStat1 And eRStat1Bit.iOriginSWState) <> 0, True, False ) 
End function
Public function isStatResetSW() As Boolean
	Return iif((m_iDevStat1 And eRStat1Bit.iResetSWState) <> 0, True, False ) 
End function
Public function isStatMeasuringTempo() As Boolean
	Return iif((m_iDevStat1 And eRStat1Bit.iMeasuringTempo) <> 0, True, False ) 
End function

Public function isStatZNoLoadStress() As Boolean
	Return iif((m_iDevStat1 And eRStat1Bit.iZNoLoadStress) <> 0, True, False ) 
End function
Public function isStatZeroAdjusting() As Boolean
	Return iif((m_iDevStat1 And eRStat1Bit.iZeroAdjusting) <> 0, True, False ) 
End function
Public function isOosConf() As Boolean
	Return iif((m_iDevStat1 And eRStat1Bit.iOosConf) <> 0, True, False ) 
End function
Public function isZActTimeReset() As Boolean
	Return iif((m_iDevStat1 And eRStat1Bit.iZActTimeReset) <> 0, True, False ) 
End function

Public function isStatHeaterOn3() As Boolean
	Return iif((m_iDevStat1 And eRStat1Bit.iHeaterOn3) <> 0, True, False ) 
End function
Public function isStatXYAction() As Boolean
	Return iif((m_iDevStat1 And eRStat1Bit.iXYAction) <> 0, True, False ) 
End function
Public function isStatGoingUpDown() As Boolean
	Return iif((m_iDevStat1 And eRStat1Bit.iGoingUpDown) <> 0, True, False ) 
End function
Public function isStatHeaterOn2() As Boolean
	Return iif((m_iDevStat1 And eRStat1Bit.iHeaterOn2) <> 0, True, False ) 
End function
Public function isStatHeaterOn() As Boolean
	Return iif((m_iDevStat1 And eRStat1Bit.iHeaterOn) <> 0, True, False ) 
End function
Public function isStatHeatPanelDetectComp() As Boolean
	Return iif((m_iDevStat1 And eRStat1Bit.iHeatPanelDetectComp) <> 0, True, False ) 
End function
Public function isStatOriginComp() As Boolean
	Return iif((m_iDevStat1 And eRStat1Bit.iOriginComp) <> 0, True, False ) 
End function

'[1110]熱板温度範囲内OK 
Private m_iDevTempNorm As Int32						
Public ReadOnly Property DevTempNorm() As Int32
	Get
		DevTempNorm = m_iDevTempNorm
	End Get
End Property

'[1111]制御基板ソフトバージョン
Private m_iDevSoftVer As Int32						
Public ReadOnly Property DevSoftVer() As Double
	Get
		DevSoftVer = m_iDevSoftVer / 100
	End Get
End Property

'[1112]本体製造番号
Private m_iDevSerealRead As Int32						
Public ReadOnly Property DevSerealRead() As Int32
	Get
		DevSerealRead = m_iDevSerealRead
	End Get
End Property

'[1113]本体状態2
	'b0: Not use
	'b1:インターロック無効
	'b2:インターロック状態(扉開閉状態 閉:1）
	'b3:X軸下限検知
	'b4:X軸上限検知
	'b5:Y軸下限検知
	'b6:Y軸上限検知
	'b7:Z軸下限検知
	'b8:Z軸上限検知
Public Enum eRStat2Bit
	iILDisable			= &h0002
	iILState			= &h0004
	iXLowerDet			= &h0008
	iXUpperDet			= &h0010
	iYLowerDet			= &h0020
	iYUpperDet			= &h0040
	iZLowerDet			= &h0080
	iZUpperDet			= &h0100
End Enum
Private m_iDevInt As Int32						
Public ReadOnly Property DevInt() As Int32
	Get
		DevInt = m_iDevInt
	End Get
End Property
Public function isILDisable() As Boolean
	Return iif((m_iDevInt And eRStat2Bit.iILDisable) <> 0, True, False ) 
End function
Public function isILState() As Boolean
	Return iif((m_iDevInt And eRStat2Bit.iILState) <> 0, True, False ) 
End function
Public function isXLowerDet() As Boolean
	Return iif((m_iDevInt And eRStat2Bit.iXLowerDet) <> 0, True, False ) 
End function
Public function isXUpperDet() As Boolean
	Return iif((m_iDevInt And eRStat2Bit.iXUpperDet) <> 0, True, False ) 
End function
Public function isYLowerDet() As Boolean
	Return iif((m_iDevInt And eRStat2Bit.iYLowerDet) <> 0, True, False ) 
End function
Public function isYUpperDet() As Boolean
	Return iif((m_iDevInt And eRStat2Bit.iYUpperDet) <> 0, True, False ) 
End function
Public function isZLowerDet() As Boolean
	Return iif((m_iDevInt And eRStat2Bit.iZLowerDet) <> 0, True, False ) 
End function
Public function isZUpperDet() As Boolean
	Return iif((m_iDevInt And eRStat2Bit.iZUpperDet) <> 0, True, False ) 
End function




'[1114]本体電源ON積算時間[秒]
Private m_iIntegTimePowOn	As Int64						
Public ReadOnly Property IntegTimePowOn() As Int64
	Get
		IntegTimePowOn = m_iIntegTimePowOn
	End Get
End Property

'[1116]熱板ﾋｰﾀON積算時間[秒]
Private m_iIntegTimeHeatOn	As Int64						
Public ReadOnly Property IntegTimeHeatOn() As Int64
	Get
		IntegTimeHeatOn = m_iIntegTimeHeatOn
	End Get
End Property

'[1118]本体電源ON積算時間リセット日[西暦年2桁+月2桁+日2桁]
Private m_iIntegDatePowOn	As Int64						
Public ReadOnly Property IntegDatePowOn() As Int64
	Get
		IntegDatePowOn = m_iIntegDatePowOn
	End Get
End Property

'[1120]熱板ﾋｰﾀON積算時間リセット日[西暦年2桁+月2桁+日2桁]
Private m_iIntegDateHeatOn	As Int64						
Public ReadOnly Property IntegDateHeatOn() As Int64
	Get
		IntegDateHeatOn = m_iIntegDateHeatOn
	End Get
End Property

'[1122]中央 X位置 (um)	
Private m_iCenterPosX	As Int32						
Public ReadOnly Property CenterPosX() As Int32
	Get
		CenterPosX = m_iCenterPosX
	End Get
End Property

'[1123]中央 Y位置 (um)	
Private m_iCenterPosY	As Int32						
Public ReadOnly Property CenterPosY() As Int32
	Get
		CenterPosY = m_iCenterPosY
	End Get
End Property

'[1124]通信基板ソフトバージョン
Private m_iComSoftVer As Int32 = 0						
Public ReadOnly Property ComSoftVer() As Double
	Get
		ComSoftVer = m_iComSoftVer / 100
	End Get
End Property

'[1125]HP PID制御量 dmv
Private m_idmv_hp As Int32						
Public ReadOnly Property Hp_dmv() As Int32
	Get
		Hp_dmv = m_idmv_hp
	End Get
End Property
'[1126]HP PID制御量 pn
Private m_ipn_hp As Int32						
Public ReadOnly Property Hp_pn() As Int32
	Get
		Hp_pn = m_ipn_hp
	End Get
End Property
'[1127]HP PID制御量 in
Private m_iin_hp As Int32						
Public ReadOnly Property Hp_in() As Int32
	Get
		Hp_in = m_iin_hp
	End Get
End Property
'[1128]HP PID制御量 dn
Private m_idn_hp As Int32						
Public ReadOnly Property Hp_dn() As Int32
	Get
		Hp_dn = m_idn_hp
	End Get
End Property

'[1129]本体エラー状態3
	'b0:X軸 脱調検知
	'b1:Y軸 脱調検知
	'b2:Z軸 脱調検知
	'b3:外部インターロック検知

	'b4:Z応力レンジ外
	'b5:プローブ 過昇温異常
	'b6:Z軸 補正 応力センサ測定範囲外
	'b7:Z軸 補正 応力センサ通信異常

	'b8:熱板FAN停止
	'b9:着液前Z振動開始検知
	'b10:CPU間異常
	'b11:起歪体 過昇温異常

	'b12:起歪体 熱電対断線検知
	'b13:起歪体 ヒータ昇温しない
	'b14:起歪体 ヒータ高温検知
	'b15:Z軸駆動中上限検知


Private m_iDevErr3 As Int32						
Public Enum eRErr3Bit
	iXOOSAlm			= &h0001
	iYOOSAlm			= &h0002
	iZOOSAlm			= &h0004
	iExternalInt		= &h0008
	iZStressOver		= &h0010
	iPBOverheat			= &h0020
	iZCorrFail1			= &h0040
	iZCorrFail2			= &h0080
	iHPFANSTOP			= &h0100
	iZVibeDetect		= &h0200
	iCPUCommErr			= &h0400
	iSGOverheat			= &h0800
	iSGTCDis			= &h1000
	iSGTNoHeatUp		= &h2000
	iSGTHighDetect		= &h4000
	iZUPPERDET			= &h8000
End Enum
Public ReadOnly Property DevErr3() As Int32
	Get
		DevErr3 = m_iDevErr3
	End Get
End Property
Public function isErrXOos() As Boolean
	Return iif((m_iDevErr3 And eRErr3Bit.iXOOSAlm) <> 0, True, False ) 
End function
Public function isErrYOos() As Boolean
	Return iif((m_iDevErr3 And eRErr3Bit.iYOOSAlm) <> 0, True, False ) 
End function
Public function isErrZOos() As Boolean
	Return iif((m_iDevErr3 And eRErr3Bit.iZOOSAlm) <> 0, True, False ) 
End function
Public function isErrExternalInt() As Boolean
	Return iif((m_iDevErr3 And eRErr3Bit.iExternalInt) <> 0, True, False ) 
End function
Public function isErrZStressOver() As Boolean
	Return iif((m_iDevErr3 And eRErr3Bit.iZStressOver) <> 0, True, False ) 
End function
Public function isErrPBOverheat() As Boolean
	Return iif((m_iDevErr3 And eRErr3Bit.iPBOverheat) <> 0, True, False ) 
End function
Public function isErrZCorrFail1() As Boolean
	Return iif((m_iDevErr3 And eRErr3Bit.iZCorrFail1) <> 0, True, False ) 
End function
Public function isErrZCorrFail2() As Boolean
	Return iif((m_iDevErr3 And eRErr3Bit.iZCorrFail2) <> 0, True, False ) 
End function
Public function isErrHPFanStop() As Boolean
	Return iif((m_iDevErr3 And eRErr3Bit.iHPFANSTOP) <> 0, True, False ) 
End function
Public function isErrZVibeDetect() As Boolean
	Return iif((m_iDevErr3 And eRErr3Bit.iZVibeDetect) <> 0, True, False ) 
End function
Public function isErrCPUCommErr() As Boolean
	Return iif((m_iDevErr3 And eRErr3Bit.iCPUCommErr) <> 0, True, False ) 
End function


Public function isErrSGOverheat() As Boolean
	Return iif((m_iDevErr3 And eRErr3Bit.iSGOverheat) <> 0, True, False ) 
End function
Public function isErrSGTCDis() As Boolean
	Return iif((m_iDevErr3 And eRErr3Bit.iSGTCDis) <> 0, True, False ) 
End function
Public function isErrSGTNoHeatUp() As Boolean
	Return iif((m_iDevErr3 And eRErr3Bit.iSGTNoHeatUp) <> 0, True, False ) 
End function
Public function isErrSGTHighDetect() As Boolean
	Return iif((m_iDevErr3 And eRErr3Bit.iSGTHighDetect) <> 0, True, False ) 
End function
Public function isErrZUpperDetect() As Boolean
	Return iif((m_iDevErr3 And eRErr3Bit.iZUPPERDET) <> 0, True, False ) 
End function



'[1130]測定№ 
Private m_iMesNo	As Int64						
Public Property MesNo() As Int64
	Get
		MesNo = m_iMesNo
	End Get
	Set (value As Int64)
		m_iMesNo = Value
	End Set
End Property

'[1132]現在応力 X 
Private m_iCurrStrsX	As Int64
Public ReadOnly Property CurrStrsX() As int64
	Get
		CurrStrsX = m_iCurrStrsX
	End Get
End Property
'[1132]現在応力X ゼロ無し
Private m_iCurrStrsLiveX	As Int64
Public ReadOnly Property CurrStrsLiveX() As int64
	Get
		CurrStrsLiveX = m_iCurrStrsLiveX
	End Get
End Property

'[1134]現在応力 Y 
Private m_iCurrStrsY	As Int64						
Public ReadOnly Property CurrStrsY() As int64
	Get
		CurrStrsY = m_iCurrStrsY
	End Get
End Property
'[1134]現在応力Y ゼロ無し 
Private m_iCurrStrsLiveY	As Int64
Public ReadOnly Property CurrStrsLiveY() As int64
	Get
		CurrStrsLiveY = m_iCurrStrsLiveY
	End Get
End Property


'[1136]現在 X位置 
Private m_iCurrPosX	As Int32						
Public ReadOnly Property CurrPosX() As Int32
	Get
		CurrPosX = m_iCurrPosX
	End Get
End Property

'[1137]現在 Y位置 
Private m_iCurrPosY	As Int32						
Public ReadOnly Property CurrPosY() As Int32
	Get
		CurrPosY = m_iCurrPosY
	End Get
End Property

'[1138]メイン基板 SPI通信 受信OKカウンタ
Private m_iMainSpiOK	As UInt16						
Public ReadOnly Property MainSpiOK() As UInt16
	Get
		MainSpiOK = m_iMainSpiOK
	End Get
End Property

'[1140]現在 日時
Private m_iCurrDate1	As Int32						
Private m_iCurrDate2	As Int32						
Private m_iCurrDate3	As Int32						
Public ReadOnly Property CurrDate1() As Int32 
	Get
		CurrDate1 = m_iCurrDate1
	End Get
End Property
Public ReadOnly Property CurrDate2() As Int32 
	Get
		CurrDate2 = m_iCurrDate2
	End Get
End Property
Public ReadOnly Property CurrDate3() As Int32 
	Get
		CurrDate3 = m_iCurrDate3
	End Get
End Property

'[1143]PB PID制御量 dmv
Private m_idmv_pb As Int32						
Public ReadOnly Property Pb_dmv() As Int32
	Get
		Pb_dmv = m_idmv_pb
	End Get
End Property
'[1144]PB PID制御量 pn
Private m_ipn_pb As Int32						
Public ReadOnly Property Pb_pn() As Int32
	Get
		Pb_pn = m_ipn_pb
	End Get
End Property
'[1145]PB PID制御量 in
Private m_iin_pb As Int32						
Public ReadOnly Property Pb_in() As Int32
	Get
		Pb_in = m_iin_pb
	End Get
End Property
'[1146]PB PID制御量 dn
Private m_idn_pb As Int32						
Public ReadOnly Property Pb_dn() As Int32
	Get
		Pb_dn = m_idn_pb
	End Get
End Property

'[1148]通信基板 SPI通信 受信OKカウンタ
Private m_iComSpiOK	As UInt16						
Public ReadOnly Property ComSpiOK() As UInt16
	Get
		ComSpiOK = m_iComSpiOK
	End Get
End Property


'[1150]プローブ 現在温度(℃) 
Private m_iHeatTemp2Act As Int32						
Public ReadOnly Property HeatTemp2Act() As Double
	Get
		HeatTemp2Act = m_iHeatTemp2Act / 10.0
	End Get
End Property

'[1151]プローブ 温度操作量(%) 
Private m_iHeatTemp2Ope As Int32						
Public ReadOnly Property HeatTemp2Ope() As Double
	Get
		HeatTemp2Ope = m_iHeatTemp2Ope / 10.0
	End Get
End Property

'[1152]プローブ 温度範囲内OK 
Private m_iDevTemp2Norm As Int32						
Public ReadOnly Property DevTemp2Norm() As Int32
	Get
		DevTemp2Norm = m_iDevTemp2Norm
	End Get
End Property

'[1153]現在応力 Z 
Private m_iCurrStrsZ	As Int64						
Public ReadOnly Property CurrStrsZ() As int64
	Get
		CurrStrsZ = Ctype(m_iCurrStrsZ, Long)
	End Get
End Property
'現在応力 Z gram換算
Private m_dCurrStrsZ	As Double
Public ReadOnly Property CurrStrsZgram() As Double
	Get
		CurrStrsZgram = Ctype(m_dCurrStrsZ, Double)
	End Get
End Property

'[1155]X動作積算時間[秒]
Private m_iIntegTimeXAct	As Int64						
Public ReadOnly Property IntegTimeXAct() As Int64
	Get
		IntegTimeXAct = m_iIntegTimeXAct
	End Get
End Property
'[1157]PBヒータ積算時間[秒]
Private m_iIntegTimePBHeat	As Int64						
Public ReadOnly Property IntegTimePBHeat() As Int64
	Get
		IntegTimePBHeat = m_iIntegTimePBHeat
	End Get
End Property

'[1159]Y動作積算時間[秒]
Private m_iIntegTimeYAct	As Int64						
Public ReadOnly Property IntegTimeYAct() As Int64
	Get
		IntegTimeYAct = m_iIntegTimeYAct
	End Get
End Property
'[1161]Z動作積算時間[秒]
Private m_iIntegTimeZAct	As Int64						
Public ReadOnly Property IntegTimeZAct() As Int64
	Get
		IntegTimeZAct = m_iIntegTimeZAct
	End Get
End Property
'[1163]CPU間通信異常カウント
Private m_iCPUCommErrCount	As Int64						
Public ReadOnly Property CPUCommErrCount() As Int64
	Get
		CPUCommErrCount = m_iCPUCommErrCount
	End Get
End Property

'[1168]Z位置 着圧決定後(0.1um)
Private m_iZConfirmPos	As Int64						
Public ReadOnly Property ZConfirmPos() As Int64
	Get
		If m_iDevSoftVer>=200
			ZConfirmPos = m_iZConfirmPos
		Else
			ZConfirmPos = 0
		End if
	End Get
End Property






'[1170]X動作積算時間リセット日[西暦年2桁+月2桁+日2桁]
Private m_iIntegDateXAct	As Int64						
Public ReadOnly Property IntegDateXAct() As Int64
	Get
		IntegDateXAct = m_iIntegDateXAct
	End Get
End Property

'[1172]積算時間リセット日[西暦年2桁+月2桁+日2桁]
Private m_iIntegDatePbHeat	As Int64						
Public ReadOnly Property IntegDatePbHeat() As Int64
	Get
		IntegDatePbHeat = m_iIntegDatePbHeat
	End Get
End Property


'[1174]Y動作積算時間リセット日[西暦年2桁+月2桁+日2桁]
Private m_iIntegDateYAct	As Int64						
Public ReadOnly Property IntegDateYAct() As Int64
	Get
		IntegDateYAct = m_iIntegDateYAct
	End Get
End Property


'[1176]脱調誤差X *0.1um
Private m_iOosValX	As Int64
Public ReadOnly Property OosValX() As Double
	Get
		OosValX = m_iOosValX / 10.0
	End Get
End Property
'[1177]脱調誤差Y *0.1um
Private m_iOosValY	As Int64
Public ReadOnly Property OosValY() As Double
	Get
		OosValY = m_iOosValY / 10.0
	End Get
End Property
'[1178]脱調誤差Z *0.1um
Private m_iOosValZ	As Int64
Public ReadOnly Property OosValZ() As Double
	Get
		OosValZ = m_iOosValZ / 10.0
	End Get
End Property

'[1179]現在応力 Z 24bitAD (予備)
Private m_iCurrStrsZCorr As Int64
Public ReadOnly Property CurrStrsZCorr() As Int64
	Get
		CurrStrsZCorr = m_iCurrStrsZCorr
	End Get
End Property
'[1181]現在応力 Z 24bitAD (予備)
Private m_iCurrStrsZCorrA As Int64
Public ReadOnly Property CurrStrsZCorrA() As Int64
	Get
		CurrStrsZCorrA = m_iCurrStrsZCorrA
	End Get
End Property
''' <summary>
''' 現在応力 Z 補正後 24bitAD
''' </summary>
Private m_iCurrStrsZCorrADig As Int64
Public ReadOnly Property　CurrStrsZCorrADig() As Int64
	Get
		CurrStrsZCorrADig = m_iCurrStrsZCorrADig
	End Get
End Property
''' <summary>
''' [1183-1184]ゼロ点位置（ゼロ調時更新値）AD[um]
''' </summary>
Private m_iZeroPos_Z As Int64
Public ReadOnly Property　ZeroPos_Z() As Int64
	Get
		ZeroPos_Z = m_iZeroPos_Z
	End Get
End Property
''' <summary>
''' [1185-1186]ゼロ調時無負荷Ｚ応力AD
''' </summary>
Private m_iZeroStrs_Z As Int64
Public ReadOnly Property　ZeroStrs_Z() As Int64
	Get
		ZeroStrs_Z = m_iZeroStrs_Z
	End Get
End Property

''' <summary>[1187]起歪体 現在温度(℃) </summary>
Private m_iHeatTemp3Act As Int32						
Public ReadOnly Property HeatTemp3Act() As Double
	Get
		HeatTemp3Act = m_iHeatTemp3Act / 10.0
	End Get
End Property

''' <summary>[1188]起歪体 温度操作量(%) </summary>
Private m_iHeatTemp3Ope As Int32						
Public ReadOnly Property HeatTemp3Ope() As Double
	Get
		HeatTemp3Ope = m_iHeatTemp3Ope / 10.0
	End Get
End Property

''' <summary>[1189]起歪体 温度範囲内OK </summary>
Private m_iDevTemp3Norm As Int32						
Public ReadOnly Property DevTemp3Norm() As Int32
	Get
		DevTemp3Norm = m_iDevTemp3Norm
	End Get
End Property

'[1190]起歪体 ヒータ積算時間[秒]
Private m_iIntegTimeSGHeat	As Int64						
Public ReadOnly Property IntegTimeSGHeat() As Int64
	Get
		IntegTimeSGHeat = m_iIntegTimeSGHeat
	End Get
End Property
'[1192]起歪体 積算時間リセット日[西暦年2桁+月2桁+日2桁]
Private m_iIntegDateSGHeat	As Int64						
Public ReadOnly Property IntegDateSGHeat() As Int64
	Get
		IntegDateSGHeat = m_iIntegDateSGHeat
	End Get
End Property

'[1194]起歪体 PID制御量 dmv
Private m_idmv_sg As Int32						
Public ReadOnly Property Sg_dmv() As Int32
	Get
		Sg_dmv = m_idmv_sg
	End Get
End Property
'[1194]起歪体 PID制御量 pn
Private m_ipn_sg As Int32						
Public ReadOnly Property Sg_pn() As Int32
	Get
		Sg_pn = m_ipn_sg
	End Get
End Property
'[1194]起歪体 PID制御量 in
Private m_iin_sg As Int32						
Public ReadOnly Property Sg_in() As Int32
	Get
		Sg_in = m_iin_sg
	End Get
End Property
'[1194]起歪体 PID制御量 dn
Private m_idn_sg As Int32						
Public ReadOnly Property Sg_dn() As Int32
	Get
		Sg_dn = m_idn_sg
	End Get
End Property
'[1198]Z動作積算時間リセット日[西暦年2桁+月2桁+日2桁]
Private m_iIntegDateZAct	As Int64						
Public ReadOnly Property IntegDateZAct() As Int64
	Get
		IntegDateZAct = m_iIntegDateZAct
	End Get
End Property







Public bMesDataEnable As Boolean = False		'測定データリンク領域有効

'■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
'   パブリック関数
'■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
Private iPreMesNo As Int64 = -1																'前回測定№
'前回測定№クリア
Public Sub PreMesClear() 
	iPreMesNo = -1
End Sub
Dim iMnoAdr		As UInt32	= eMEMADR.iMEAS_NO_L - R_TOP_LNK_ADR							'測定№アドレスを計算
'測定No監視用
Dim SW_Ovs			As New Stopwatch
Dim iOvsMesNum		As int64 = -1
Dim bStatMeasuring	As Boolean = False
Dim m_bDebug		As Boolean
Public ReadOnly Property bComDebug() As Boolean
	Get
		bComDebug = m_bDebug
	End Get
End Property
'================================================
'リンク読込領域一括読込
'引数	読込バイト配列
'戻り値
'================================================
''' <summary>
''' リンク読込領域一括読込み
''' </summary>
''' <param name="clDt">		測定データファイル制御クラスのインスタンス</param>
''' <returns>	0			正常終了
'''				-1			例外発生
'''				-2			予定バイト数と不一致なバイト数が受信された
''' </returns>
Public Function ReadBatch(byref clDt As DataFile) As Int32
	Dim iRet	As Int32 = 0
	Dim ms As New IO.MemoryStream
	Dim byteDat() As Byte = Nothing     '書き込み用バイト
	Dim byteReadBuffer(4095) As Byte	'受信バッファバイト
	Dim iReadLength As Int32            '受信文字数
	Dim i As Int32						'j Index補助
	Dim j As Int32						'm_iRAryM_D Index
	Dim k As Int32						'byteReadBuffer Index
	Dim x As Int32 = 0					'p_dTMP,p_iPOSX,p_iPOSY Index
	Dim iRByte as Int32					'受信バイト数
	Dim iMes as Int64
	Dim iDataZ	As Int64

	Try
		j = 0       'm_iRary Index
		MakeReadTelegram(byteDat, R_TOP_LNK_ADR, R_PWX_LNK_ADR - R_TOP_LNK_ADR )		'受信用電文の生成(ﾏｲｺﾝ死活信号1100から1199までをリード)
		TCP_Write(byteDat, byteDat.Length)                                              'TCP送信 読込みコマンド
		iRByte = (R_PWX_LNK_ADR - R_TOP_LNK_ADR) * 2 + TCP_HEAD_OFS
		iRet = ReadBuffer(iRByte, ms, iReadLength)										'全て受信まで蓄積

		If iRet<0 Or iReadLength <> iRByte Then											'50*2+10
			m_iLinkErrState = m_iLinkErrState Or &h0001									'リンクエラー状態設定
			Console.WriteLine("ReadLength Error" & iReadLength)
			iRet = -2
		Else
			m_iLinkErrState = m_iLinkErrState And Not(&h0001)							'リンクエラー状態設定
			byteReadBuffer = ms.GetBuffer()                                             'I/Oストリームバッファから取り込み
			'読込みグローバル（INT32）へ格納
			For k = TCP_HEAD_OFS To iReadLength + 2 Step 2
				m_iRAry(j) = WordFromBytes(byteReadBuffer(k), byteReadBuffer(k + 1))   '2バイトをInt型変換
				j += 1
			Next k
			'読込み情報をプライベート変数へ保持
			RInfoStore()
			'--ここまで常時読み込み  これ以降は測定時の測定データ ----------

			iMes = DWordFromWords(m_iRAry(iMnoAdr + 1), m_iRAry(iMnoAdr + 0))					'測定№ 2WordをUInt64型変換

'If iMes<>0
'Console.WriteLine("iMes={0:D}", iMes)
'End if

			'デバッグ時の測定No制御を自動判別する --------------
			m_bDebug = If(p_strEquipIP.Substring(0,3) = "127",True,False)			'シミュレータIPアドレス？
			Dim bDataUse As Boolean
			If m_bDebug 
				bDataUse = iMes<>0
			Else
#If 1
				bDataUse = iMes <> 0 'AndAlso iPreMesNo<>iMes

				'測定連番の停滞監視 -------------------------------------------------------------
				If isStatMeasuring() And Not bStatMeasuring
				'測定ステータスの立上りで監視タイマー開始
					SW_Ovs.Restart					'測定No変化監視タイマー開始
					iOvsMesNum = 0					'前回値を初期値
				Else
					bStatMeasuring = isStatMeasuring()
				End If

				If isStatMeasuring()
				'測定中ステータスがONの時に監視
					If iMes <> iOvsMesNum
					'測定No変化あり
						iOvsMesNum = iMes			'前回値へ退避
						SW_Ovs.Restart()
					ElseIf SW_Ovs.Elapsed.TotalSeconds > 2
					'測定連番が２秒以上経過しても同じ値の場合はログ出力
						Console.WriteLine("Receive Block No change! more 2Sec Elapsed iMes = {0:D}", iMes)
						Call WriteLog("Receive Block No change! more 2Sec Elapsed iMes = " & iMes & "," & SW_Ovs.Elapsed.TotalSeconds, eLOG_LEVEL.iWARNING)
						SW_Ovs.Restart()
					End if
				End If
				'---------------------------------------------------------------------------------
#ELse
				If m_iStartSW And eWStartBit.iMeas AndAlso	iPreMesNo<>-1		'測定中か？
					Dim iMissingBlockNum As Int16 = iMes - iPreMesNo - 1
					If iMissingBlockNum > 0
					'データブロック抜けがある
						Dim iAllLen		As UInt64
						Dim iSIdx		As Uint64
						Dim iLoop		As Uint64
						Dim iElapseTime As Uint64
						Dim iMiss		As Int16
						For iMiss = 0 To iMissingBlockNum
							'書き込んだ結果のファイル行数計算
							p_dtFile.RealTimeGetSampleState(iAllLen, iSIdx, iElapseTime)
							'リアルタイムグラフ用の蓄積配列拡張
							p_dtFile.ResizeMeasArray(iAllLen)
							'蓄積配列へ追加
							For iLoop = 0 To p_dtFile.p_iPwX.GetUpperBound(0)
								'応力X,応力Y,応力Z,X軸位置,Y軸位置,温度,温度操作量,補正値
								p_dtFile.RealTimeAccumulationArrayCopy(iLoop, iSIdx)
							Next iLoop
						Next iMiss
					End If
					bDataUse = iMes <> 0 AndAlso iPreMesNo<>iMes
				Else
					bDataUse = False
				End If
#End If
			End If
			'---------------------------------------------------
			If bDataUse

'バッファ連番連続性監視 ---------------------------------------------------
If iMes-iPreMesNo > 1 AndAlso iPreMesNo>=0
'連番で無くなったらログ出力
	Call WriteLog("Receive Block missed! iMes-iPreMesNo=" & iMes-iPreMesNo & ",iMes=" & iMes & ",iPreMesNo=" & iPreMesNo, eLOG_LEVEL.iWARNING)
End If
'Console.WriteLine("{0:D}", iMes)
'--------------------------------------------------------------------------

				MakeReadTelegram(byteDat, R_PWX_LNK_ADR, R_MES_LNK_END - R_PWX_LNK_ADR )		'受信用電文の生成(測定データ1100から4100までをリード)
				TCP_Write(byteDat, byteDat.Length)                                              'TCP送信 読込みコマンド
				iRByte = (R_MES_LNK_END - R_PWX_LNK_ADR) * 2 + TCP_HEAD_OFS						'読込みバイト数計算
				ReadBuffer(iRByte, ms, iReadLength)												'全て受信まで蓄積
				If iReadLength <> iRByte Then													'50*2+10
					m_iLinkErrState = m_iLinkErrState Or (&h0001)								'リンクエラー状態設定
					iRet = -2
					bMesDataEnable = False		'測定データ無効
				Else
					m_iLinkErrState = m_iLinkErrState And Not(&h0001)							'リンクエラー状態設定
					byteReadBuffer = ms.GetBuffer()                                             'I/Oストリームバッファから取り込み
					j = 0
					'読込みグローバル（INT32）へ格納, double型変換 -> p_iPwX,p_iPwY
					For k = TCP_HEAD_OFS To iReadLength + 2 Step 2
						m_iRAryM(j) = WordFromBytes(byteReadBuffer(k), byteReadBuffer(k + 1))       '2バイトをInt型変換
						i = j + 1
						Select Case i
						Case R_PWX_LNK_ADR - R_MES_LNK_TOP + 1 To R_PWY_LNK_ADR - R_MES_LNK_TOP			'応力X
							If i = R_PWX_LNK_ADR - R_MES_LNK_TOP + 1 Then
								x = 0
							End If
							If i Mod 2 = 0 Then
								clDt.p_iPwX(x) = DWordFromWords(m_iRAryM(j), m_iRAryM(j - 1)) - P_XStressZero
								x += 1
							End If
						Case R_PWY_LNK_ADR - R_MES_LNK_TOP + 1 To R_PWZ_LNK_ADR - R_MES_LNK_TOP			'応力Y
							If i = R_PWY_LNK_ADR - R_MES_LNK_TOP + 1  Then
								x = 0
							End If
							If i Mod 2 = 0 Then
								clDt.p_iPwY(x) = DWordFromWords(m_iRAryM(j), m_iRAryM(j - 1)) - P_YStressZero
								x += 1
							End If
						Case R_PWZ_LNK_ADR - R_MES_LNK_TOP + 1 To R_ASX_LNK_ADR - R_MES_LNK_TOP			'応力Z
							If i = R_PWZ_LNK_ADR - R_MES_LNK_TOP + 1  Then
								x = 0
							End If
							If i Mod 2 = 0 Then
								'clDt.p_iPwZ(x) = DWordFromWords(m_iRAryM(j), m_iRAryM(j - 1)) - P_ZStressZero
								'iDataZ = DWordFromWords(m_iRAryM(j), m_iRAryM(j - 1)) - P_ZStressZero
								iDataZ = DWordFromWords(m_iRAryM(j), m_iRAryM(j - 1)) - m_iZeroStrs_Z
								clDt.p_iPwZ(x)	= iDataZ						'AD値
								clDt.p_dPwZ(x)	= iDataZ * p_cts.WeightCoefZ	'ｇ値
								x += 1
							End If
						Case R_ASX_LNK_ADR - R_MES_LNK_TOP + 1 To R_ASY_LNK_ADR - R_MES_LNK_TOP			'X軸位置
							If i = R_ASX_LNK_ADR - R_MES_LNK_TOP + 1 Then
								x = 0
							End If
							'振幅規定値以下の場合は最小単位 0.1um
							If m_iAmplitudeX <= cSwitchAmpXYPos
								clDt.p_iPOSX(x) = CType(m_iRAryM(j), Double) * 1
							Else
								clDt.p_iPOSX(x) = CType(m_iRAryM(j), Double) * 1
							End if

							x += 1
						Case R_ASY_LNK_ADR - R_MES_LNK_TOP + 1 To R_TMP_LNK_ADR - R_MES_LNK_TOP			'Y軸位置
							If i = R_ASY_LNK_ADR - R_MES_LNK_TOP + 1 Then
								x = 0
							End If

							'振幅規定値以下の場合は最小単位 0.1um
							If m_iAmplitudeY <= cSwitchAmpXYPos
								clDt.p_iPOSY(x) = CType(m_iRAryM(j), Double) * 1
							Else
								clDt.p_iPOSY(x) = CType(m_iRAryM(j), Double) * 1
							End if
							x += 1
						Case R_TMP_LNK_ADR - R_MES_LNK_TOP + 1 To R_TMV_LNK_ADR - R_MES_LNK_TOP			'温度(HP)
							If i = R_TMP_LNK_ADR - R_MES_LNK_TOP + 1 Then
								x = 0
							End If
							clDt.p_iTemp(x) = CType(m_iRAryM(j), Double)
							x += 1
						Case R_TMV_LNK_ADR - R_MES_LNK_TOP + 1 To R_TMP2_LNK_ADR - R_MES_LNK_TOP		'温度操作量(HP)
							If i = R_TMV_LNK_ADR - R_MES_LNK_TOP + 1 Then
								x = 0
							End If
							clDt.p_iMv(x) = CType(m_iRAryM(j), Double)
							x += 1
						Case R_TMP2_LNK_ADR - R_MES_LNK_TOP + 1 To R_TMV2_LNK_ADR - R_MES_LNK_TOP		'温度(PROBE)
							If i = R_TMP2_LNK_ADR - R_MES_LNK_TOP + 1 Then
								x = 0
							End If
							clDt.p_iTemp2(x) = CType(m_iRAryM(j), Double)
							x += 1
						Case R_TMV2_LNK_ADR - R_MES_LNK_TOP + 1 To R_ASZ_LNK_ADR - R_MES_LNK_TOP		'温度操作量(POBE)
							If i = R_TMV2_LNK_ADR - R_MES_LNK_TOP + 1 Then
								x = 0
							End If
							clDt.p_iMv2(x) = CType(m_iRAryM(j), Double)
							x += 1
						Case R_ASZ_LNK_ADR - R_MES_LNK_TOP + 1 To R_DUP_LNK_ADR - R_MES_LNK_TOP			'Z軸位置 + 更新要求
							'm_iRAryM(j) = UWordFromBytes(byteReadBuffer(k), byteReadBuffer(k + 1))       '2バイトを符号なしInt型変換
							m_iRAryM(j) = SWordFromBytes(byteReadBuffer(k), byteReadBuffer(k + 1))       '2バイトを符号付Int32型変換(Z位置測定データを着圧決定前は投入位置を基準とした[um]単位、着圧決定後は着圧位置を基準とした[0.1um]単位に変更)
							If i = R_ASZ_LNK_ADR - R_MES_LNK_TOP + 1 Then
								x = 0
							End If
							clDt.p_iPOSZ(x) = CType(m_iRAryM(j), Double)
#If 0
Debug.Print("clDt.p_iPOSX(x) = " & clDt.p_iPOSX(x))
Debug.Print("clDt.p_iPOSY(x) = " & clDt.p_iPOSY(x))
Debug.Print("clDt.p_iPOSZ(x) = " & clDt.p_iPOSZ(x))
#end if
							x += 1
						Case Else
						End Select
						j += 1
					Next k
					bMesDataEnable = True		'測定データ有効
				End If
			Else
				bMesDataEnable = False		'測定データ無効
			End If
			If iMes<>0
				iPreMesNo = iMes
			End If
		End If
	Catch ex As Exception
		Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
		iRet = -1
	End Try
	ms.Close()
	ms.Dispose()
	Return iRet
End Function
''' <summary>
''' TCP受信バッファ蓄積
''' </summary>
''' <param name="nByte">		指定読込みバイト数</param>
''' <param name="ms">			I/Oメモリストリーム</param>
''' <param name="iReadLength">	実際に読み込んだバイト数</param>
''' <returns></returns>
Private Function ReadBuffer(byval nByte as int32, ByRef ms As IO.MemoryStream, ByRef iReadLength As Int32) As Int32
	Dim i				As	Int32		'TCPリード回数
	Dim iByteCount		As	Int32		'一回に読込んだバイト数
	Dim btReadBuf(1023)	As	Byte		'一回に読込んだバイト列
	Try
		i = 0
		iReadLength = 0
		ms.Seek(0,SeekOrigin.begin)		'ストリーム先頭へ位置づけ
		Do
			iByteCount = TCP_Read(btReadBuf, btReadBuf.Length)      'TCP受信 読込み値
			If iByteCount = 0 Then
				Exit Do												'サーバーストップ？
			ElseIf iByteCount < 0
				Return -1											'サーバー応答なし
			End If
			ms.Write(btReadBuf, 0, iByteCount)						'メモリストリームへ書込み
			iReadLength += iByteCount								'読込みバイト数を積算
			i += 1
		Loop While iReadLength < nByte			'指定バイト数まで蓄積
		Return 0
	Catch ex As Exception
		Return -1
	End Try
End Function
''' <summary>
''' リンク書込領域一括書込み
''' </summary>
''' <returns>	0	正常
'''				-1	例外発生
'''				-2	レスポンス異常
''' </returns>
Public Function WriteBatch() As Int32
	Dim byteDat() As Byte = Nothing     '書き込み用バイト
	Dim byteReadBuffer(511) As Byte
	Dim strBuf(W_END_LNK_ADR - W_TOP_LNK_ADR - 1) As String                '送信用文字列
	Dim iReadLength As Int32                '受信文字数
	Dim i As Int32
	Try
		'書込み保持配列へセット
		WInfoStore()
		For i = 0 To W_END_LNK_ADR - W_TOP_LNK_ADR - 1
			strBuf(i) = String.Format("{0,4:D}", m_iWAry(i))
		Next i
		MakeWriteTelegram(byteDat, W_TOP_LNK_ADR, strBuf)
		TCP_Write(byteDat, byteDat.Length)
		iReadLength = TCP_Read(byteReadBuffer, byteReadBuffer.Length)
		If iReadLength < 0 Then
			m_iLinkErrState = m_iLinkErrState Or &h0002							'リンクエラー状態設定
			Return -2
		Else
			m_iLinkErrState = m_iLinkErrState AND Not(&h0002)					'リンクエラー状態設定
		End If
		Return 0
	Catch ex As Exception
		Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
		Return -1
	End Try
End Function
'''<summary>
'''メモリリンク用の受信用電文を作成する。
'''</summary>
'''<param name="byteDat">作成した電文を代入する配列。</param>
'''<param name="iStAddr">読込開始アドレス。</param>
'''<param name="iLen">読込長。</param>
Public Shared Sub MakeReadTelegram(ByRef byteDat As Byte(), ByVal iStAddr As Integer, ByVal iLen As Integer)
	Dim bAddr(1) As Byte
	Dim bLen(1) As Byte

	Try
		'開始アドレスと読込み長をByteに変換
		If ConvIntToByteArry(iStAddr, bAddr, bAddr.Length) = True And ConvIntToByteArry(iLen, bLen, bLen.Length) = True Then
			'受信用電文の生成
			byteDat = New Byte(cCONST_TLG_NUM) {}
			byteDat(0) = Asc("B"c)          'CONST
			byteDat(1) = 0                  'CONST
			byteDat(2) = 0                  'CONST
			byteDat(3) = 0                  'CONST
			byteDat(4) = 0                  'CONST
			byteDat(5) = 0                  'CONST
			byteDat(6) = 0                  'CONST
			byteDat(7) = 6                  'ソケットサイズ（Readのため固定）
			byteDat(8) = &H1B               'ESCAPE
			byteDat(9) = Asc("R"c)          'Readモード
			byteDat(10) = bAddr(1)          '開始アドレス上位バイト
			byteDat(11) = bAddr(0)          '開始アドレス下位バイト
			byteDat(12) = bLen(1)           'データ数上位バイト
			byteDat(13) = bLen(0)           'データ数下位バイト
		End If
	Catch ex As Exception
		Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	End Try
End Sub
'''<summary>
'''メモリリンク用の送信用電文を作成する。
'''<param name="byteDat">作成した電文を代入する配列。</param>
'''<param name="iStAddr">書込開始アドレス。</param>
'''<param name="strWrite">書込文字列</param>
'''</summary>
Public Shared Sub MakeWriteTelegram(ByRef byteDat As Byte(), ByVal iStAddr As Integer, ByVal strWrite() As String)
	Dim iLen As Integer = strWrite.Count
	Dim bAddr(1) As Byte
	Dim bLen(1) As Byte

	Try
		'開始アドレスと書込み長をByteに変換
		If ConvIntToByteArry(iStAddr, bAddr, bAddr.Length) = True And
		ConvIntToByteArry(iLen, bLen, bLen.Length) = True Then
			byteDat = New Byte(cCONST_TLG_NUM + iLen * 2) {}

			byteDat(0) = Asc("B"c)              'CONST
			byteDat(1) = 0                      'CONST
			byteDat(2) = 0                      'CONST
			byteDat(3) = 0                      'CONST
			byteDat(4) = 0                      'CONST
			byteDat(5) = 0                      'CONST
			byteDat(6) = 0                      'CONST
			byteDat(7) = 6 + (iLen * 2)         'ソケットサイズ
			byteDat(8) = &H1B                   'ESCAPE
			byteDat(9) = Asc("W"c)              'Writeモード
			byteDat(10) = bAddr(1)              '開始アドレス上位バイト
			byteDat(11) = bAddr(0)              '開始アドレス下位バイト
			byteDat(12) = bLen(1)               'データ数上位バイト
			byteDat(13) = bLen(0)               'データ数下位バイト
			'書込み値の設定
			Dim iVal As Integer
			For i As Integer = 0 To iLen - 1
				Integer.TryParse(strWrite(i), iVal)
				byteDat(14 + i * 2) = (iVal And &HFF00) >> 8        'MSB
				byteDat(15 + i * 2) = iVal And &HFF                 'LSB
			Next i
		End If
	Catch ex As Exception
		Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	End Try
End Sub

'''<summary> 通信により取得した2バイトを符号つきワードに変換する。 </summary>
'''<param name="iM">上位バイト。</param>
'''<param name="iL">下位バイト。</param>
Public Shared Function WordFromBytes(ByVal iM As Byte, iL As Byte) As Int16
	Try
		Dim iResult As int32 = ((Uint16.parse(iM) << 8) Or iL)
		If iResult > &h7fff Then
			iResult -= &h10000
		End If
		Return iResult
	Catch ex As Exception
		Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
		Return -1
	End Try
End Function

'''<summary> 通信により取得した2バイトを符号つき32bitデータに変換する。 </summary>
'''<param name="iM">上位バイト。</param>
'''<param name="iL">下位バイト。</param>
Public Shared Function SWordFromBytes(ByVal iM As Byte, iL As Byte) As Int32
	Try
		Dim iResult As int32 = ((Uint16.parse(iM) << 8) Or iL)
		If iResult > &h7fff Then
			iResult -= &h10000
		End If
		Return iResult
	Catch ex As Exception
		Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
		Return -1
	End Try
End Function

'''<summary> 通信により取得した2バイトを符号なしワードに変換する。 </summary>
'''<param name="iM">上位バイト。</param>
'''<param name="iL">下位バイト。</param>
Public Shared Function UWordFromBytes(ByVal iM As Integer, iL As Integer) As Double
	Dim dResult As Double = ((iM << 8) Or iL)
	Return dResult
End Function


''' <summary>
''' 符号付16bitデータを符号付32ビットデータへ変換
''' </summary>
''' <param name="iH">符号付上位ワード</param>
''' <param name="iL">符号付下位ワード</param>
''' <returns>符号付32bitデータ</returns>
Public Shared Function DWordFromWords(iH As Int16, iL As Int16) As Int32
    Dim ret As Int32
    Dim u16H As UInt16
    Dim u16L As UInt16
    Dim lng As Long
Try
    u16H = int2uint(iH)
    u16L = int2uint(iL)

    lng = (CUInt(u16H) << 16) Or u16L
    If (lng >= 2 ^ 31) Then
        lng -= 2 ^ 32
    End If
    ret = CInt(lng)
Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
    Return ret
End Function
''' <summary>
''' 符号付16bitデータを符号なし16bitへ変換
''' </summary>
''' <param name="i16">符号付16bitデータ</param>
''' <returns>符号なし16bitデータ</returns>
Private Shared Function int2uint(i16 As Int16) As UInt16
    If (i16 >= 0) Then
        Return CUInt(i16)
    Else
        Return CUInt(2 ^ 16 + i16)
    End If
End Function

'''<summary>
'''接続処理を行う。
'''</summary>
Public Function TCP_Connect(ByVal sHost As String, ByVal sPort As String, ByRef sErrMsg As String) As Boolean
	Dim bDat(1024) As Byte
	Try
		If IsNothing(sHost)
		'IPアドレスでない
			Return False
		ElseIf sHost.Length < 7
		'IPアドレスでない
			Return False
		'既に接続されている場合、何もしない
		ElseIf TCP_IsConnected() = True Then
			Return True
		End If
		If m_client Is Nothing Then
			m_client = New TcpClient
			m_client.ReceiveTimeout = CliRcvTimeOut
			m_client.SendTimeout = CliSndTimeOut
		ElseIf m_client.Connected = False Then
			m_client = New TcpClient
			m_client.ReceiveTimeout = CliRcvTimeOut
			m_client.SendTimeout = CliSndTimeOut
		End If
		Dim iPort As Integer
		Integer.TryParse(sPort, iPort)
		m_client.Connect(sHost, iPort)
		m_ns = m_client.GetStream()
		p_iCon = 1
		m_iLinkErrState = 0
		'Console.WriteLine("Connected")
	Catch ex As Exception
		If (m_blInitConnect = True) Or (TCP_IsConnected() = True) Then            '初回接続か接続状態から未接続に移行
			m_blInitConnect = False
			Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
		End If
		'接続できなかった場合
		sErrMsg = ex.Message
		p_strErrState = ex.Message
		p_iCon = 0
		Return False
	End Try
	Return True
End Function

'''<summary>
'''切断処理を行う。
'''</summary>
Public Sub TCP_DisConnect()
	Try
		If Not (m_client Is Nothing) Then
			If m_client.Connected Then
				m_ns.Close()
				m_client.Close()
				p_iCon = 0
			End If
		End If
	Catch ex As Exception
		Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
		p_strErrState = ex.Message
	End Try
End Sub

'''<summary>
'''接続状態を確認する。
'''</summary>
Public Function TCP_IsConnected() As Boolean
	Try
		If m_client Is Nothing Then
			Return False
		End If
		Return m_client.Connected
	Catch ex As Exception
		Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
		Return False
	End Try
	Return True
End Function

'''<summary>
'''書き込み処理を行う。
'''</summary>
Public Function TCP_Write(data() As Byte, len As Integer) As Boolean
	Try
		If Not IsNothing(m_ns)
		'割り込みフラグON
		If p_bInterrupt = True Then
			'割り込み書込み開始
			m_ns.Write(p_bytWInterrupt, 0, p_bytWInterrupt.Length)
			WriteLog("Temperature profile value write : OK", eLOG_LEVEL.iINFO)

			If TCP_Read(p_bytRInterrupt, p_bytRInterrupt.Length) < 0 Then
				Return False
			End If
			WriteLog("Temperature profile value read : OK", eLOG_LEVEL.iINFO)

			p_bInterrupt = False
		End If
		If len >= 0 Then
			'引数のデータを書込み
			m_ns.Write(data, 0, len)
		End If
		End If
	Catch ex As Exception
		Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & "(" & New StackFrame(1).GetMethod.Name & ")" & " : " & ex.Message)
		p_strErrState = ex.Message
		Return False
	End Try
	Return True
End Function

'''<summary>
'''読み込み処理を行う。
'''</summary>
Public Function TCP_Read(data() As Byte, len As Integer) As Int32
	Dim iBytes As Int32
	Try
		If Not IsNothing(m_ns)
		'応答バイトを読み込む
		iBytes = m_ns.Read(data, 0, len)
		'Console.WriteLine("Received")
		End If
	Catch ex As Exception
		Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
		p_strErrState = ex.Message
		Return -1
	End Try
	Return iBytes
End Function


'''<summary> 現在の接続をテストして確認する </summary>
'''<returns> 通信に成功した場合True, 失敗した場合False </returns>
Public Function TCP_CheckConnect() As Boolean
	Try
		Dim tmp(0) As Byte
		m_ns.Write(tmp, 0, 0)
	Catch ex As Exception
		Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
		p_strErrState = ex.Message
		Return False
	End Try
	Return True
End Function



'■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
'   プライベート関数
'■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
'書込み情報をプライベート変数へ保持
Private Function WInfoStore() As Int32
Dim sidx As Int32 = W_TOP_LNK_ADR	
	Try
	m_iWAry( eMEMADR.iTEMP_PRS - sidx )						= m_iTempPreset
	m_iWAry( eMEMADR.iGAP_PRS - sidx )						= m_iGapPreset
	m_iWAry( eMEMADR.iMES_WAIT - sidx )						= m_iMeasWaitTime
	m_iWAry( eMEMADR.iCLR_TIME - sidx )						= m_iClearTime
	m_iWAry( eMEMADR.iTMP_RNG - sidx )						= m_iTempOkRange
	m_iWAry( eMEMADR.iMAIN_UNIT_SERIAL - sidx )				= m_iDevSereal
	m_iWAry( eMEMADR.iLED_LIGHT - sidx )					= m_iLedOnOff
	m_iWAry( eMEMADR.iHEAT_P_PID - sidx )					= m_iP_PID
	m_iWAry( eMEMADR.iHEAT_I_PID - sidx )					= m_iI_PID
	m_iWAry( eMEMADR.iHEAT_D_PID - sidx )					= m_iD_PID
	m_iWAry( eMEMADR.iHEAT_DGAIN - sidx )					= m_iDGain
	m_iWAry( eMEMADR.iHEAT_PVBIAS - sidx )					= m_iPVBias
	m_iWAry( eMEMADR.iSTANBY_TIME - sidx )					= m_iStanbyTime
	m_iWAry( eMEMADR.iIN_OPERATION - sidx )					= m_iInOperation
	m_iWAry( eMEMADR.iCONFIG1 - sidx )						= m_iConfig1
	m_iWAry( eMEMADR.iSTART_TEMP_CONT - sidx )				= m_iStartTempCont
	m_iWAry( eMEMADR.iSTART_SW - sidx )						= m_iStartSW
	m_iWAry( eMEMADR.iERR_RESET - sidx )					= m_iErrReset
	m_iWAry( eMEMADR.iHEATER_LIMIT - sidx )					= m_iHeaterUpper
	m_iWAry( eMEMADR.iINP_GAP - sidx )						= m_iInpGap
	m_iWAry( eMEMADR.iMES_PRE_WAIT - sidx )					= m_iMesPreWait
	m_iWAry( eMEMADR.iACT_MODE - sidx )						= m_iActMode
	m_iWAry( eMEMADR.iAMPLITUDE_X - sidx )					= m_iAmplitudeX
	m_iWAry( eMEMADR.iAMPLITUDE_Y - sidx )					= m_iAmplitudeY
	m_iWAry( eMEMADR.iPERIOD_X - sidx )						= m_iPeriodX
	m_iWAry( eMEMADR.iPERIOD_Y - sidx )						= m_iPeriodY

	m_iWAry( eMEMADR.iPOS_HEATB_L - sidx )					= (m_iPosHeatB And &H0000FFFF) 
	m_iWAry( eMEMADR.iPOS_HEATB_H - sidx )					= (m_iPosHeatB And &HFFFF0000) >> 16

	m_iWAry( eMEMADR.iPOS_X - sidx )						= m_iPosX
	m_iWAry( eMEMADR.iPOS_Y - sidx )						= m_iPosY

	m_iWAry( eMEMADR.iPOS_ORDER - sidx )					= m_iPosOrder

	m_iWAry( eMEMADR.iZERO_START_POS_L - sidx )				= (m_iZeroStartPos And &H0000FFFF) 
	m_iWAry( eMEMADR.iZERO_START_POS_H - sidx )				= (m_iZeroStartPos And &HFFFF0000) >> 16

	m_iWAry( eMEMADR.iCENTER_START_POS_L - sidx )			= (m_iCenterStartPos And &H0000FFFF) 
	m_iWAry( eMEMADR.iCENTER_START_POS_H - sidx )			= (m_iCenterStartPos And &HFFFF0000) >> 16

	m_iWAry( eMEMADR.iTIME_SET_1 - sidx )					= m_iTimeSet1
	m_iWAry( eMEMADR.iTIME_SET_2 - sidx )					= m_iTimeSet2
	m_iWAry( eMEMADR.iTIME_SET_3 - sidx )					= m_iTimeSet3
	m_iWAry( eMEMADR.iTIME_SET_ORDER - sidx )				= m_iDateSetOrder
	
	m_iWAry( eMEMADR.iXPOS - sidx )							= m_iZeroAdjPosX
	m_iWAry( eMEMADR.iYPOS - sidx )							= m_iZeroAdjPosY

	m_iWAry( eMEMADR.iSTRESS_DET_ACQUIS_TIME - sidx )		= m_iStressDetectAcquisitionTime
	m_iWAry( eMEMADR.iSTRESS_DET_AD_THRESHOLD - sidx )		= m_iStressDetectADThreshold
	m_iWAry( eMEMADR.iSTRESS_DET_DURA_TIME - sidx )			= m_iStressDetectDurationTime
	m_iWAry( eMEMADR.iDEBUG_SET - sidx )					= m_iDebugSet

	m_iWAry( eMEMADR.iPC_ALIVE - sidx )						= m_iPcAlive
	m_iWAry( eMEMADR.iTEMP2_PRS - sidx )					= m_iTemp2Preset
	m_iWAry( eMEMADR.iTMP2_RNG - sidx )						= m_iTemp2OkRange
	m_iWAry( eMEMADR.iHEAT2_P_PID - sidx )					= m_iP_PID2
	m_iWAry( eMEMADR.iHEAT2_I_PID - sidx )					= m_iI_PID2
	m_iWAry( eMEMADR.iHEAT2_D_PID - sidx )					= m_iD_PID2
	m_iWAry( eMEMADR.iHEAT2_DGAIN - sidx )					= m_iDGain2
	m_iWAry( eMEMADR.iHEAT2_PVBIAS - sidx )					= m_iPVBias2

	m_iWAry( eMEMADR.iHEATER2_LIMIT - sidx )				= m_iHeaterUpper2

	m_iWAry( eMEMADR.iCENT_POS_X_W - sidx )					= m_iCentPosX_W
	m_iWAry( eMEMADR.iCENT_POS_Y_W - sidx )					= m_iCentPosY_W

	m_iWAry( eMEMADR.iHP_PVGAIN - sidx )					= m_dPVGain
	m_iWAry( eMEMADR.iPB_PVGAIN - sidx )					= m_dPVGain2

	m_iWAry( eMEMADR.iZERO_COMP_PRESS_L - sidx )			= (m_iMeasureStress And &H0000FFFF) 
	m_iWAry( eMEMADR.iZERO_COMP_PRESS_H - sidx )			= (m_iMeasureStress And &HFFFF0000) >> 16

	m_iWAry( eMEMADR.i7SEG_BRIGHTNESS - sidx )				= m_i7SegBrightness
	m_iWAry( eMEMADR.iMEASURE_MODE - sidx )					= m_iMeasureMode
	m_iWAry( eMEMADR.iAMPLITUDE_Z - sidx )					= m_iAmplitudeZ
	m_iWAry( eMEMADR.iPERIOD_Z - sidx )						= m_iPeriodZ

	m_iWAry( eMEMADR.iAMPMODE - sidx )						= m_iAmpMode

	m_iWAry( eMEMADR.iPRESSCONT_DELAY - sidx )				= m_iPressContDelay
	m_iWAry( eMEMADR.iPRESSCONT_STEP - sidx )				= m_iPressContStep

	m_iWAry( eMEMADR.iAMPMODE_AMP - sidx )					= m_iAmpModeAmplitude * 2			'p-p値のため2倍

	m_iWAry( eMEMADR.iZAmpStep - sidx )						= m_iZAmpStep

	m_iWAry( eMEMADR.iSG_TEMP_PRS - sidx )					= m_iTemp3Preset
	m_iWAry( eMEMADR.iSG_TEMP_RNG - sidx )					= m_iTemp3OkRange
	m_iWAry( eMEMADR.iSG_P_PID - sidx )						= m_iP_PID3
	m_iWAry( eMEMADR.iSG_I_PID - sidx )						= m_iI_PID3
	m_iWAry( eMEMADR.isg_D_PID - sidx )						= m_iD_PID3
	m_iWAry( eMEMADR.iSG_DGAIN - sidx )						= m_iDGain3
	m_iWAry( eMEMADR.iSG_PVBIAS - sidx )					= m_iPVBias3
	m_iWAry( eMEMADR.iSG_PVGAIN - sidx )					= m_dPVGain3
	m_iWAry( eMEMADR.iSG_LIMIT - sidx )						= m_iHeaterUpper3 + 100				'起歪体温度上限値 設定を+10℃で本体へ書込み

	'Z軸上限リミット位置
	m_iWAry( eMEMADR.iZ_UPPER_LIMIT_L - sidx )				= (m_iZUpperLimit And &H0000FFFF) 
	m_iWAry( eMEMADR.iZ_UPPER_LIMIT_H - sidx )				= (m_iZUpperLimit And &HFFFF0000) >> 16

	Return 0
	Catch ex As Exception
		Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	End Try
	Return -1
End Function


'読込み情報をプライベート変数へ保持
Private Function RInfoStore() As Int32
Dim sidx As Int32 = R_TOP_LNK_ADR	
Dim iData As Int64
	Try
	m_iDevAlive = m_iRAry(eMEMADR.iDEV_ALIVE - sidx)								'本体CPU死活信号 
	m_iDevErr1 = m_iRAry(eMEMADR.iDEV_ERR1 - sidx)									'本体エラー状態1
	m_iDevErr2 = m_iRAry(eMEMADR.iDEV_ERR2 - sidx)									'本体エラー状態2
	m_iHeatTempAct = m_iRAry(eMEMADR.iHEAT_TEMP_ACT - sidx)							'熱板 現在温度(℃)
	m_iHeatTempOpe = m_iRAry(eMEMADR.iHEAT_TEMP_OPE - sidx)							'熱板 温度操作量(℃)

	iData = DWordFromWords( m_iRAry(eMEMADR.iHEATB_POS_H_ACT - sidx), m_iRAry(eMEMADR.iHEATB_POS_L_ACT - sidx))
	m_iHeatBPos = iData																'熱板現在位置(um)
	m_iToolLinkStatus = m_iRAry(eMEMADR.iTOOL_LINK_STS - sidx)						'Tool連携用ｽﾃｰﾀｽ
	m_iDevRes = m_iRAry(eMEMADR.iDEV_RES - sidx)									'指令応答ビット
	m_iDevStat1 = m_iRAry(eMEMADR.iDEV_STAT1 - sidx)								'本体状態1
	m_iDevTempNorm = m_iRAry(eMEMADR.iDEV_TEMP_NRM - sidx)							'熱板 温度範囲内OK
	m_iDevSoftVer = m_iRAry(eMEMADR.iDEV_SOFT_VER - sidx)							'制御基板ソフトバージョン
	m_iDevSerealRead = m_iRAry(eMEMADR.iDEV_SEREAL - sidx)							'本体製造番号
	m_iDevInt = m_iRAry(eMEMADR.iDEV_IL - sidx)										'本体状態2

	iData = DWordFromWords( m_iRAry(eMEMADR.iINTEGTIME_POWERON_H - sidx), m_iRAry(eMEMADR.iINTEGTIME_POWERON_L - sidx))
	m_iIntegTimePowOn = iData														'本体電源ON積算時間[秒]

	iData = DWordFromWords( m_iRAry(eMEMADR.iINTEGTIME_HEATERON_H - sidx), m_iRAry(eMEMADR.iINTEGTIME_HEATERON_L - sidx))
	m_iIntegTimeHeatOn = iData														'HPﾋｰﾀON積算時間[秒]

	iData = DWordFromWords( m_iRAry(eMEMADR.iINTEGTIME_XACT_H - sidx), m_iRAry(eMEMADR.iINTEGTIME_XACT_L - sidx))
	m_iIntegTimeXAct = iData														'X動作積算時間[秒]
	iData = DWordFromWords( m_iRAry(eMEMADR.iINTEGTIME_YACT_H - sidx), m_iRAry(eMEMADR.iINTEGTIME_YACT_L - sidx))
	m_iIntegTimeYAct = iData														'Y動作積算時間[秒]
	iData = DWordFromWords( m_iRAry(eMEMADR.iINTEGTIME_ZACT_H - sidx), m_iRAry(eMEMADR.iINTEGTIME_ZACT_L - sidx))
	m_iIntegTimeZAct = iData														'Z動作積算時間[秒]

	iData = DWordFromWords( m_iRAry(eMEMADR.iINTEGTIME_PBHEATON_H - sidx), m_iRAry(eMEMADR.iINTEGTIME_PBHEATON_L - sidx))
	m_iIntegTimePbHeat = iData														'PBヒータON動作積算時間[秒]

	iData = DWordFromWords( m_iRAry(eMEMADR.iINTEGRSTDATE_POWERON_H - sidx), m_iRAry(eMEMADR.iINTEGRSTDATE_POWERON_L - sidx))
	m_iIntegDatePowOn = iData														'本体電源ON積算時間リセット日[西暦年2桁+月2桁+日2桁]

	iData = DWordFromWords( m_iRAry(eMEMADR.iINTEGRSTDATE_HEATERON_H - sidx), m_iRAry(eMEMADR.iINTEGRSTDATE_HEATERON_L - sidx))
	m_iIntegDateHeatOn = iData														'HPﾋｰﾀON積算時間リセット日[西暦年2桁+月2桁+日2桁]

	iData = DWordFromWords( m_iRAry(eMEMADR.Z_CONFIRM_POS_H - sidx), m_iRAry(eMEMADR.Z_CONFIRM_POS_L - sidx))
	m_iZConfirmPos	= iData															'Z位置 着圧決定後(0.1um)

	iData = DWordFromWords( m_iRAry(eMEMADR.iINTEGRSTDATE_XACT_H - sidx), m_iRAry(eMEMADR.iINTEGRSTDATE_XACT_L - sidx))
	m_iIntegDateXAct = iData														'X動作積算時間リセット日[西暦年2桁+月2桁+日2桁]
	iData = DWordFromWords( m_iRAry(eMEMADR.iINTEGRSTDATE_YACT_H - sidx), m_iRAry(eMEMADR.iINTEGRSTDATE_YACT_L - sidx))
	m_iIntegDateYAct = iData														'Y動作積算時間リセット日[西暦年2桁+月2桁+日2桁]
	iData = DWordFromWords( m_iRAry(eMEMADR.iINTEGRSTDATE_ZACT_H - sidx), m_iRAry(eMEMADR.iINTEGRSTDATE_ZACT_L - sidx))
	m_iIntegDateZAct = iData														'Z動作積算時間リセット日[西暦年2桁+月2桁+日2桁]

	iData = DWordFromWords( m_iRAry(eMEMADR.iINTEGRSTDATE_PBHEATON_H - sidx), m_iRAry(eMEMADR.iINTEGRSTDATE_PBHEATON_L - sidx))
	m_iIntegDatePbHeat = iData														'PBﾋｰﾀON積算時間リセット日[西暦年2桁+月2桁+日2桁]

	m_iCenterPosX = m_iRAry(eMEMADR.iCENT_POS_X - sidx)								'中央 X位置 
	m_iCenterPosY = m_iRAry(eMEMADR.iCENT_POS_Y - sidx)								'中央 Y位置 

	m_iComSoftVer = m_iRAry(eMEMADR.iCOM_SOFT_VER - sidx)							'通信基板ソフトバージョン

	m_idmv_hp		= m_iRAry(eMEMADR.iDMV_HP - sidx)								'HP PID制御量 dmv
	m_ipn_hp		= m_iRAry(eMEMADR.iPN_HP - sidx)								'HP PID制御量 pn
	m_iin_hp		= m_iRAry(eMEMADR.iIN_HP - sidx)								'HP PID制御量 in
	m_idn_hp		= m_iRAry(eMEMADR.iDN_HP - sidx)								'HP PID制御量 dn

	m_iDevErr3 = m_iRAry(eMEMADR.iDEV_ERR3 - sidx)									'本体エラー状態3

	iData = DWordFromWords( m_iRAry(eMEMADR.iMEAS_NO_H - sidx), m_iRAry(eMEMADR.iMEAS_NO_L - sidx))

	If m_bDebug
		m_iMesNo += 1																'測定№ 
	Else
		m_iMesNo = iData															'測定№ 
	End If

	iData = DWordFromWords( m_iRAry(eMEMADR.iCURR_STRS_X_H - sidx), m_iRAry(eMEMADR.iCURR_STRS_X_L - sidx))
	m_iCurrStrsX		= iData - P_XStressZero										'現在応力 X
	m_iCurrStrsLiveX	= iData														'現在応力 X

	iData = DWordFromWords( m_iRAry(eMEMADR.iCURR_STRS_Y_H - sidx), m_iRAry(eMEMADR.iCURR_STRS_Y_L - sidx))
	m_iCurrStrsY		= iData - P_YStressZero										'現在応力 Y 
	m_iCurrStrsLiveY	= iData														'現在応力 Y 

	iData = DWordFromWords( m_iRAry(eMEMADR.iCURR_STRS_Z_H - sidx), m_iRAry(eMEMADR.iCURR_STRS_Z_L - sidx))
	m_iCurrStrsZ = iData - P_ZStressZero                                            '現在応力 Z(補正前)


	m_iCurrPosX		= m_iRAry(eMEMADR.iCURR_POS_X - sidx)                           '現在 X位置 
	m_iCurrPosY		= m_iRAry(eMEMADR.iCURR_POS_Y - sidx)							'現在 Y位置 
	m_iMainSpiOK	= m_iRAry(eMEMADR.iMAIN_SPI_OK - sidx)							'メイン基板 SPI通信 OKカウンタ
	m_iComSpiOK		= m_iRAry(eMEMADR.iCOM_SPI_OK - sidx)							'通信基板	SPI通信 OKカウンタ
	m_iCurrDate1	= m_iRAry(eMEMADR.iCURR_DATE_1 - sidx)							'現在 日時 YYMM
	m_iCurrDate2	= m_iRAry(eMEMADR.iCURR_DATE_2 - sidx)							'現在 日時 DDHH
	m_iCurrDate3	= m_iRAry(eMEMADR.iCURR_DATE_3 - sidx)							'現在 日時 MMSS

	m_idmv_pb		= m_iRAry(eMEMADR.iDMV_PB - sidx)								'PB PID制御量 dmv
	m_ipn_pb		= m_iRAry(eMEMADR.iPN_PB - sidx)								'PB PID制御量 pn
	m_iin_pb		= m_iRAry(eMEMADR.iIN_PB - sidx)								'PB PID制御量 in
	m_idn_pb		= m_iRAry(eMEMADR.iDN_PB - sidx)								'PB PID制御量 dn

	m_iHeatTemp2Act = m_iRAry(eMEMADR.iHEAT_TEMP2_ACT - sidx)						'プローブ 現在温度(℃)
	m_iHeatTemp2Ope = m_iRAry(eMEMADR.iHEAT_TEMP2_OPE - sidx)						'プローブ 温度操作量(℃)
	m_iDevTemp2Norm = m_iRAry(eMEMADR.iDEV_TEMP2_NRM - sidx)						'プローブ 温度範囲内OK

	m_iOosValX		= m_iRAry(eMEMADR.iOOSVal_X - sidx)								'脱調誤差X
	m_iOosValY		= m_iRAry(eMEMADR.iOOSVal_Y - sidx)								'脱調誤差Y
	m_iOosValZ		= m_iRAry(eMEMADR.iOOSVal_Z - sidx)								'脱調誤差Z

	'ゼロ調時無負荷Z応力（補正後）AD
	iData = DWordFromWords(m_iRAry(eMEMADR.iZERO_STRS_Z_H - sidx), m_iRAry(eMEMADR.iZERO_STRS_Z_L - sidx))
	m_iZeroStrs_Z	= iData

	'現在応力Z(補正後)
	iData = DWordFromWords(m_iRAry(eMEMADR.iCURR_STRS_Z_CORRA_H - sidx), m_iRAry(eMEMADR.iCURR_STRS_Z_CORRA_L - sidx))
	m_iCurrStrsZCorrA	= iData
	m_iCurrStrsZCorrAdig= iData  - m_iZeroStrs_Z                                    '現在応力 Z補正後
	m_dCurrStrsZ		= m_iCurrStrsZCorrAdig * p_cts.WeightCoefZ					'現在応力 Z補正後(gram換算) 

	'ゼロ点位置(ゼロ調時更新値) AD
	iData = DWordFromWords(m_iRAry(eMEMADR.iZERO_POS_Z_H - sidx), m_iRAry(eMEMADR.iZERO_POS_Z_L - sidx))
	m_iZeroPos_Z	= iData




	'現在応力Z(補正用)
	iData = DWordFromWords(m_iRAry(eMEMADR.iCURR_STRS_Z_CORR_H - sidx), m_iRAry(eMEMADR.iCURR_STRS_Z_CORR_L - sidx))
	m_iCurrStrsZCorr	= iData

	m_iHeatTemp3Act = m_iRAry(eMEMADR.iHEAT_TEMP3_ACT - sidx)						'起歪体 現在温度(℃)
	m_iHeatTemp3Ope = m_iRAry(eMEMADR.iHEAT_TEMP3_OPE - sidx)						'起歪体 温度操作量(℃)
	m_iDevTemp3Norm = m_iRAry(eMEMADR.iDEV_TEMP3_NRM - sidx)						'起歪体 温度範囲内OK

	iData = DWordFromWords( m_iRAry(eMEMADR.iINTEGTIME_SGHEATON_H - sidx), m_iRAry(eMEMADR.iINTEGTIME_SGHEATON_L - sidx))
	m_iIntegTimeSGHeat = iData														'起歪体ヒータON動作積算時間[秒]
	iData = DWordFromWords( m_iRAry(eMEMADR.iINTEGRSTDATE_SGHEATON_H - sidx), m_iRAry(eMEMADR.iINTEGRSTDATE_SGHEATON_L - sidx))
	m_iIntegDateSGHeat = iData														'起歪体ﾋｰﾀON積算時間リセット日[西暦年2桁+月2桁+日2桁]

	m_idmv_sg		= m_iRAry(eMEMADR.iDMV_SG - sidx)								'SG PID制御量 dmv
	m_ipn_sg		= m_iRAry(eMEMADR.iPN_SG - sidx)								'SG PID制御量 pn
	m_iin_sg		= m_iRAry(eMEMADR.iIN_SG - sidx)								'SG PID制御量 in
	m_idn_sg		= m_iRAry(eMEMADR.iDN_SG - sidx)								'SG PID制御量 dn

	m_iCPUCommErrCount	= m_iRAry(eMEMADR.CPU_COM_ERR - sidx)						'CPU 通信エラーカウント


	Return 0
	Catch ex As Exception
		Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	End Try
	Return -1
End Function

''' <summary>
''' 日時設定用メンバー変数へ指定DATE型変数から変換
''' </summary>
''' <param name="dNowDate"></param>
Public sub sbTimeSetForm(ByVal dNowDate As date)
	Dim	iYear	As UInt16 = dNowDate.Year
	Dim	iMont	As UInt16 = dNowDate.Month
	Dim	iDay	As UInt16 = dNowDate.Day
	Dim	iHour	As UInt16 = dNowDate.Hour
	Dim	iMin	As UInt16 = dNowDate.Minute
	Dim	iSec	As UInt16 = dNowDate.Second
	Try
		m_iTimeSet1	= (iYear - 2000) * 100 + iMont
		m_iTimeSet2	= iday * 100 + iHour
		m_iTimeSet3	= iMin * 100 + iSec
	Catch ex As Exception
		Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	End Try
End sub

End Class