'============================================
'File:   modSZKConst.vb
'Name:   SIZUKA Tool 定数定義
'Desc:   グローバル定数を定義する。
'Author: Fumihiko Kondo
'Date:   2015/04/16
'============================================

Module modSZKConst

#Const ONERR = False            'OnErrorGgotoを切り替えるデバッグ定義

'▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼
'   定数
'▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲

'''<summary> RTGSETの最大行数 </summary>
Public Const m_iRTGSETMaxRow	As Integer = 100
'Public Const cMTD_None As String = "(None)"
'PDF保存用テンプレートファイル
Public Const m_sPDFTemplate_1	As String = "Template1.doc"
Public Const m_sPDFTemplate_2	As String = "Template2.doc"
'PDF保存用テンプレートファイル
Public Const m_sImg2PdfPath		As String = "Img2Pdf\Img2Pdf.exe"
'ProtectKit組込み
Public Const cLOCK_WORD			As String = "Msek86nuyCtG"          'ロックワード
Public Const cLOCK_WORD_VIEW	As String = "Msek86nuyCtGV"			'ロックワード(閲覧用)
Public Const cPROC_INFO_NAME	As String = "DLL_INFO"				'プロテクト情報ファイル
Public Const cPROC_SYS_NAME		As String = "DLL_SYS"               'プロテクトシステムファイル
Public Const cPROC_FILE_PATH	As String = "C:\windows\fSq62W"		'プロテクト状態ファイル保存フォルダ
Public Const cPROC_DEVINFO_NAME As String = "DLL_DEV_INFO"			'開発者用プロテクト情報ファイル
'ProtectKit組込み
'本体画面番号
Public Const cPORT_NO			As String = "1024"                  '本体のポート番号
'日時フォーマット文字列 年月日時分
Public Const cDateFormYMDHm		As String = "yyyy/MM/dd HH:mm"


'Public Enum eSPEC_TAB
'	iFREE = 0		'自由入力
'	iFIX			'固定選択
'	iMAX			'要素数
'End Enum

Enum eLANGUAGE
	iENG = 0		'米英語
	iJPN			'日本語
	iHNS			'中国語(簡体字)
	iKRA			'韓国語
	iHNT			'中国語(繁体字)
	iMAX			'要素数
End Enum
'Public Enum eCALC_ROW
'	iMASK_RANGE = 0 'マスク範囲
'	iGEL_RANGE      'ゲル判定範囲
'	iRTG            'リアルタイムグラフ(熱板温度変更)指定
'	iMAX            '要素数
'End Enum

''''<summary> 測定エラーを示す定数 </summary>
'Public Enum RTG_ERR As Integer
'	OK = 0      'エラーなし
'	DOUBLECNT   '特定のX値について複数のY値がある
'End Enum

'''<summary> 測定エラーを示す定数 </summary>
Public Enum CONNECT_ST As Integer
    OFFLINE = 0     '未接続
    ONLINE          '接続中
    CANNOTCONNECT   '接続不可
    CONNECTING      '接続処理中
End Enum

'''<summary> BackgroundWorkerへの通信要求一覧の定数 </summary>
Public Enum BGW_ORDER As Integer
    NO_ORDER = 0                '指示なし
    CONNECT_LINK                '接続->LINK
    CONNECT_LINK_1              '接続->LINK
    DISCONNECT_LINK             'LINK解除->切断
    DISCONNECT_LINK_1           'LINK解除->切断
    CONNECT                     '接続
    DISCONNECT                  '切断
    START_RTG                   '同期処理開始
    CANCEL_RTG                  '同期処理中止
    START_BUTTON                '測定開始受付
    START_BUTTON_CNF            '測定応答確認
    MEAS_PREPARE_1              '測定準備1
    MEAS_PREPARE_2              '測定準備2
    MEAS_PROCESS                '測定中
	WAIT_LOCKOUT				'測定終了 プローブロック外し待ち
	WAIT_LOCKOUT1				'測定終了 プローブロック外し待ち
    MEAS_PRO_DATA               '測定データ終了処理
    MEAS_PRO_FINISH             '測定終了処理
    MEAS_PRO_FINISH_1           '測定終了処理(MAIN Timerと同期をとるため)
    MEAS_PRO_FINISH_2           '測定終了処理(MAIN Timerと同期をとるため)
	MEAS_BEFORE_CANCEL			'測定開始前に中止
 	MEAS_ERROR_CANCEL			'装置エラーによる測定中止
	CANCEL_BUTTON               '測定中止
    CANCEL_BUTTON_1             '測定中止
    ZERO_BUTTON                 'ゼロ調開始
    ZERO_BUTTON_CNF             'ゼロ調開始
    ZERO_BUTTON_N               'ゼロ調開始
    ZERO_BUTTON_0               'ゼロ調開始
    ZERO_BUTTON_1               'ゼロ調開始
    ZERO_BUTTON_2               'ゼロ調開始
    HORIZON_BUTTON_START        '水平確認
    HORIZON_BUTTON_CNF          '水平確認
    HORIZON_BUTTON_0            '水平確認
    HORIZON_BUTTON_1            '水平確認
    HORIZON_BUTTON_1_1          '水平確認
    HORIZON_BUTTON_2            '水平確認
    HORIZON_BUTTON_3            '水平確認
    HORIZON_BUTTON_4            '水平確認
    HORIZON_BUTTON_STOP         '水平確認
    HORIZON_BUTTON_STOP_1       '水平確認
    TEMP_BUTTON_REP             '熱板&プローブ温度制御開始
    TEMP_HP_BUTTON_REP          '熱板温度制御開始
    TEMP_PB_BUTTON_REP          'プローブ温度制御開始
    TEMP_SG_BUTTON_REP          '起歪体温度制御開始
    ORG_BUTTON_REP              '原点復帰開始
    ORG_BUTTON_REP_1            '原点復帰完了待ち
    ORG_BUTTON_REP_2            '原点復帰完了待ち
    RESET_ERROR_REP             'エラーリセット受付
    RESET_ERROR_CNF             'エラーリセット確認
    CENTER_PROCESS_START_REP    '中心出し処理開始
    CENTER_PROCESS_START_CNF    '中心出し完了応答
    CENTER_PROCESS_STOP_REP     '中心出し処理中止
    CENTER_REGIST_START_REP     '中心座標登録開始
    CENTER_REGIST_START_CNF     '中心座標登録応答
    XYMODE_DRIVE_START          '手動XY動作モード駆動の開始
    XYMODE_DRIVE_STOP           '手動XY動作モード駆動の停止

	ZNOLOAD_STRESS_SET_REP		'Z無負荷応力設定要求
	ZNOLOAD_STRESS_SET_CNF		'Z無負荷応力設定応答

    SET_BODY                    '詳細オプション本体設定
    CALIB_TEMP                  '較正 温度ON
    CALIB_ZERO                  '較正 ゼロ調
    CALIB_START_0               '較正開始
    CALIB_START_1               '較正開始
    CALIB_START_2               '較正開始
    CALIB_START_X               '較正X軸開始
    CALIB_START_C               '較正XY軸開始
    CALIB_START_P               '較正Z軸開始
    CALIB_START_CNF             '較正処理確定
    CALIB_PROCESSING            '較正処理中
    CALIB_STOP                  '較正処理 中止
    CALIB_STOP_1                '較正処理 中止
    CALIB_STOP_2                '較正処理 中止

    AUTO_PEELING                '自動剥がし
    AUTO_PEELING_0              '自動剥がし
    AUTO_PEELING_1              '自動剥がし
    AUTO_PEELING_2              '自動剥がし
    AUTO_PEELING_3              '自動剥がし
    AUTO_PEELING_4              '自動剥がし
    AUTO_PEELING_5              '自動剥がし
    AUTO_PEELING_6              '自動剥がし
    AUTO_PEELING_STOP           '自動剥がし停止

    MANU_PEELING                '手動剥がし
    MANU_PEELING_0              '手動剥がし
    MANU_PEELING_1              '手動剥がし
    MANU_PEELING_2              '手動剥がし
    MANU_PEELING_3              '手動剥がし
    MANU_PEELING_STOP           '手動剥がし停止

	FLCV_START					'フローカーブ(速度) 開始
	FLCV_START_0				'フローカーブ(速度)
	FLCV_START_1				'フローカーブ(速度)
	FLCV_START_2				'フローカーブ(速度)
	FLCV_START_CNF				'フローカーブ(速度)
	FLCV_PROCESSING				'フローカーブ(速度)
	FLCV_STOP					'フローカーブ(速度)
	FLCV_STOP_1					'フローカーブ(速度)

    FLCV_START_BUTTON           'フローカーブ(時間) 測定開始受付
    FLCV_START_BUTTON_CNF       'フローカーブ(時間) 測定応答確認
    FLCV_MEAS_PREPARE_1         'フローカーブ(時間) 測定準備1
    FLCV_MEAS_PREPARE_2         'フローカーブ(時間) 測定準備2
    FLCV_MEAS_PROCESS           'フローカーブ(時間) 測定中
    FLCV_MEAS_PRO_DATA          'フローカーブ(時間) 測定データ終了処理
    FLCV_MEAS_PRO_FINISH        'フローカーブ(時間) 測定終了処理
    FLCV_MEAS_PRO_FINISH_1      'フローカーブ(時間) 測定終了処理(MAIN Timerと同期をとるため)
    FLCV_CANCEL_BUTTON          'フローカーブ(時間) 測定中止
    FLCV_CANCEL_BUTTON_1        'フローカーブ(時間) 測定中止

    LT_START_BUTTON				'長時間(間欠) 測定開始受付
    LT_START_BUTTON_CNF			'長時間(間欠) 測定応答確認
    LT_MEAS_PREPARE_1			'長時間(間欠) 測定準備1
    LT_MEAS_PREPARE_2			'長時間(間欠) 測定準備2
	LT_MEAS_PROCESS_WAIT		'長時間(間欠) 休止中
	LT_MEAS_STAB_WAIT			'長時間(間欠) 動作後安定待ち
	LT_MEAS_PROCESS_ZPOS		'長時間(間欠) Z位置ラッチ
	LT_MEAS_PROCESS				'長時間(間欠) 測定中
	LT_DATACAL					'長時間(間欠) 計算および平均化
	LT_DATACAL_DISP				'長時間(間欠) 表示更新
	LT_WAIT_LOCKOUT				'長時間(間欠) 測定終了 プローブロック外し待ち
	LT_WAIT_LOCKOUT1			'長時間(間欠) 測定終了 プローブロック外し待ち
    LT_MEAS_PRO_FINISH			'長時間(間欠) 測定終了処理
    LT_MEAS_PRO_FINISH_1		'長時間(間欠) 測定終了処理(MAIN Timerと同期をとるため)
    LT_MEAS_PRO_FINISH_2		'長時間(間欠) 測定終了処理(MAIN Timerと同期をとるため)
    LT_CANCEL_BUTTON			'長時間(間欠) 測定中止
    LT_CANCEL_BUTTON_1			'長時間(間欠) 測定中止
	LT_MEAS_BEFORE_CANCEL		'長時間(間欠) 測定中になる前に測定中止
	LT_MEAS_ERROR_CANCEL		'長時間(間欠) 装置エラーによる測定中止

    RESET_INTEG_POWON           '本体電源ON積算時間リセット
    RESET_INTEG_POWON_CNF       '本体電源ON積算時間リセット
    RESET_INTEG_HEATON          'HPヒータON積算時間リセット
    RESET_INTEG_HEATON_CNF      'HPヒータON積算時間リセット
    RESET_INTEG_XYACT			'XY動作積算時間リセット
    RESET_INTEG_XYACT_CNF		'XY動作積算時間リセット
    RESET_INTEG_HEATON2			'PBヒータON積算時間リセット
    RESET_INTEG_HEATON2_CNF		'PBヒータON積算時間リセット
    RESET_INTEG_HEATON3			'SGヒータON積算時間リセット
    RESET_INTEG_HEATON3_CNF		'SGヒータON積算時間リセット

    LINK                        'バッチ通信開始
    LINK_1                      '本体シリアル読込
    NOLINK                      'バッチ通信中止
	SETDATE_REP                 '時計設定受付
	SETDATE_CNF                 '時計設定確認
	SETPRO_REP					'装置名設定受付
	SETPRO_CNF					'装置名設定確認
    HOT_PLATE_DRV_REP           '熱板駆動要求
    HOT_PLATE_DRV_CNF           '熱板駆動確認
    HOT_PLATE_DRV_CNF1          '熱板駆動確認
    X_AXIS_DRV_REP              'X軸駆動要求
    X_AXIS_DRV_CNF              'X軸駆動確認
    X_AXIS_DRV_CNF1             'X軸駆動確認
    Y_AXIS_DRV_REP              'Y軸駆動要求
    Y_AXIS_DRV_CNF              'Y軸駆動確認
    Y_AXIS_DRV_CNF1             'Y軸駆動確認
    OutOfStepConf_REP           '脱調確認要求
    OutOfStepConf_CNF           '脱調確認確認
    OutOfStepConf_CNF1          '脱調確認確認

    FINISH_BGW                  'バックグラウンド動作終了
End Enum

'''<summary> BackgroundWorkerが行う作業一覧の定数 </summary>
Public Enum BGW_WORK As Integer
    NO_ORDER = 0        '指示なし
    AUTO_CONNECT        '自動接続
    START_RTG           '同期処理開始
    CANCEL_RTG          '同期処理中止
    FINISH_BGW          'バックグラウンド動作終了
End Enum

'Public Enum eSPEC_ROW    
'    iTEMPR = 0          '熱板温度
'    iTEMPR2             'プローブ温度
'    iTRQ                'トルク判定値
'    iINP_GAP            '投入GAP
'    iGAP                'ギャップ値
'    iXAmp               'X軸振幅
'    iYAmp               'Y軸振幅
'    iXPeriod            'X軸周期
'    iYPeriod            'Y軸周期
'    iWAIT               '熱板上昇待ち時間
'    iCLEAR              'クリア時間(投入待ち時間)
'    iXYWAIT             'XY動作待機時間
'    iY_MAX              'Y軸表示範囲
'    iEND_COND           '測定終了条件
'    iEND_STRS           '測定終了応力
'    iEND_TIME           '測定終了時間
'    iTEMPR_TOLER        '熱板温度範囲
'    iTEMPR2_TOLER       'プローブ温度範囲

'    iPROBE              'プローブ
'    iAPPENDIX           '付加情報
'    iMETHOD             '算出法
'    iEFFECTIVE          '有効範囲
'    iJUDGE              '判定範囲
'    iXYMODE             'XY動作モード
'    iTEMPPROFILE        '温度プロファイル

'    iMAX                '要素数
'End Enum
'Public Enum eSPECF_ROW    
'    iXYMode = 0         'XY動作モード
'    iTEMPR = 1          '熱板温度
'    iTEMPR2             'プローブ温度
'    iTRQ                'トルク判定値
'    iINP_GAP            '投入GAP
'    iGAP                'ギャップ値
'    iXAmp               'X軸振幅
'    iYAmp               'Y軸振幅
'    iXPeriod            'X軸周期
'    iYPeriod            'Y軸周期
'    iWAIT               '熱板上昇待ち時間
'    iCLEAR              'クリア時間(投入待ち時間)
'    iXYWAIT             'XY動作待機時間
'    iY_MAX              'Y軸表示範囲
'    iEND_COND           '測定終了条件
'    iEND_TIME           '測定終了時間
'    iEND_STRS           '測定終了応力
'    iTEMPR_TOLER        '熱板温度範囲
'    iTEMPR2_TOLER       'プローブ温度範囲
'    iSTIRRING_ROD       'センサヘッド
'    iSAMPLE_AMMOUNT     'サンプル量
'    iMETHOD             '算出法
'    iMASKTIME           'マスク範囲（時間）
'    iJUDGE              '判定範囲
'    iTEMPPROFILE        '温度プロファイル
'    iMAX                '要素数
'End Enum
'Public Enum eSPECO_ROW    
'    iTEMPR = 0          '熱板温度
'    iTEMPR2             'プローブ温度
'    iTRQ                'トルク判定値
'    iINP_GAP            '投入GAP
'    iGAP                'ギャップ値
'    iXAmp               'X軸振幅
'    iYAmp               'Y軸振幅
'    iXPeriod            'X軸周期
'    iYPeriod            'Y軸周期
'    iWAIT               '熱板上昇待ち時間
'    iCLEAR              'クリア時間(投入待ち時間)
'    iXYWAIT             'XY動作待機時間
'    iY_MAX              'Y軸表示範囲
'    iEND_COND           '測定終了条件
'    iEND_STRS           '測定終了応力
'    iEND_TIME           '測定終了時間
'    iTEMPR_TOLER        '熱板温度範囲
'    iTEMPR2_TOLER       'プローブ温度範囲
'    iPROBE			    'プローブ
'    iAppendix		    '付加情報
'    iMAX                '要素数
'End Enum

'============================================
'固定値
'============================================
'未決定追加・変更機能
'Public Const cNOT_DECIDE	As Boolean = False
'Public Const cMAX_USBFILE	As Integer = 20

'グラフの余白設定
'Public cMARGIN_TOP			As Integer = 10
'Public Const cMARGIN_LEFT	As Integer = 70
'Public Const cMARGIN_RIGHT	As Integer = 70
'Public Const cMARGIN_BOTTOM As Integer = 70

Public p_sLOG_LEVEL() As String = {"DEBUG", "INFO", "WARNING", "FATAL"}
Enum eLOG_LEVEL
    iDEBUG = 0          'デバッグ出力
    iINFO               'Information
    iWARNING            '警告
    iFATAL              '異常
End Enum
Enum efrmMainSTR
    iINTERLOCK_ZERO = 0			'
    iINPUT_TIMELIMIT_ERR		'
    iDBCON_ERROR				'
    Open3						'
    Open4						'
    Open5						'
    Open6						'
    Open7						'
    iOUTOF_DISKSPACE			'自動保存先ドライブ(#DRIVE#) の空き容量が #VALUE#[%] 以下です。\r\n自動保存を行わずに測定を実施しますか?
    Open9						'
    Open10						'
    Open11						'
    iDoorOpen					'扉が開いています。
    iErrorNotContinue			'エラー発生中のため継続できません。
    iRETUN_ORIGIN				'原点復帰が行われていません。
    iTEMP_CONT_START			'温度制御が開始されました。
    iZERO_CONT_START			'ゼロ調整制御が開始されました。
    iACCUM_SAMP					'蓄積サンプル数:#NUM#
	iMEAS_END_CALC				'測定終了-計算中です...
	iRESULT1					'ゲルタイム(秒)
	iRESULT2					'温度(℃)
	iRESULT3					'粘度(PaS)
	iMEAS_END_GEL				'測定が終了しました。 ゲルタイム(秒)=#GELTIME#
	iDIAG_GEL_END				'測定が終了しました。\r\nプローブのロックを解除しOKボタンをクリックしてください。
	iDIAG_FCLB_END				'フル応力キャリブレーションが終了しました。\r\n自動剥しを実行しますか?
	iDIAG_ZCLB_END				'ゼロ応力キャリブレーションが終了しました。
    iNO_CALIB_DATA				'指定条件のキャリブレーションデータがありません。
    iRECALC_MEAS_TIME           '測定データが #VALUE# 件あります。\r\n測定時間の再計算を行いますか?\r\n(時間がかかります)
    iZOVS_TITLE					'Z位置監視モニタ
    iZMON_TIME					'時間[秒]
    iZMON_POS					'Z位置[mm]
    iGEL_NG						'NG: #GEL# 秒
    iGEL_OK						'OK: #GEL# 秒
    iGEL_TIME					'ゲルタイム: #GEL# 秒
    iFILE_OVERWRITE_TITLE       'ファイルの上書き確認
    iFILE_OVERWRITE_MSG         '次のファイルが既に存在します。上書きしてもよろしいですか？\r\n#PATH#
    iYES                        'はい(&Y)
    iYES_TO_ALL                 'すべてはい(&A)
    iNO                         'いいえ(&N)
    iOK                         'OK(&O)
    iCANCEL=40                  'キャンセル(&C)
    iFILE_RENAME_CHAR_TITLE     'ファイル名の置換確認
    iTITLE_OPEN_LICENSE_FILE    'ライセンスファイルのインポート
    iFILTER_OPEN_LICENSE_FILE   'ライセンスファイル|*.lic
    iLARGE_LOEWER_LIMIT         '下限に上限より大きい値が入力されています。
    iAUTH_WINDOWS               'Windows 認証
    iAUTH_SQLSVR                'SQL Server 認証
    iSQLSVR_CONN_SUCCESS        '接続は成功しました。
    iSQLSVR_CONN_FAILED         '接続に失敗しました。
    iSQLSVR_SELECT_GUIDE        'サーバーを選択してください。
    iNotUse50					'
    iRMTSVR_SHARE_ACCESS_DENIED 'リモートサーバーの共有フォルダにアクセスできません。\r\n測定を中止します。
    iOPE_GUIDE_TITLE			'操作ガイド ウィンドウタイトル
    iRECALC_FILE				'再計算後、ファイル名:
    iRECALC_PROC				'再計算中です...
    iCHART_X_TIME				'時間[秒]
    iCHART_X_TEMP				'温度[℃]
    iMNT_CNT_OVER	            'メンテナンスカウンタの積算値が閾値を超過しています。\r\n確認してください。
    iRESERVE1			        'SPEC 項目名
    Open59						'
    PRINTPAGE_STRINGS           '
    PRINT_TOPSTR                '
    CHART_DISP_ITEM             'ゲルタイムチャート表示項目リスト(X応力,Y応力,Z応力,,応力,HP温度,HP MV,PB温度,PB MV,粘度,位相,サンプル歪,剪断弾性率,貯蔵弾性率,弾性率,損失正接,応力%)
    CHART_DISP_UNIT             'ゲルタイムチャート表示項目単位リスト(,,,,[uN],[℃],[%],[℃],[%],[PaS],[度],,[Pa],[Pa],[Pa],,[%])
    CHART_THRESHOLD				'閾値
    EXT_GEL_CBX					'"下限側","中間","上限側"
    iFILE_OVERWRITE_MSG1        '上書きしてもよろしいですか？\r\n上書きする場合は元のデータは履歴へ移動します。
	iDATA_CLEAR					'クリアしました。
	ROW_APRX_RANGE				'有効範囲	
	ROW_VALID_RANGE				'判定範囲
	RTG_SPEC_SETTING			'温度プロファイル
	FILE_OPENED					'規定スペックファイルオープン
	FAILED_OPEN_DATA			'規定スペックファイルのオープンに失敗	
	READY						'レディ
	INVALID_INPUT_VALUE			'スペックに無効な入力値が含まれています。
	INVALID_INPUT_DIGIT			'(小数桁数)	
	LANGUAGE_CHANGE_WHEN_RESTARTED	'言語設定は、アプリケーションの再起動時に反映されます。	
	COULD_NOT_CONNECT			'接続に失敗しました	
	BUTTON_MEAS					'測定
	BUTTON_STOP					'停止
	NOT_WEIGHT_CONV				'重量換算係数が取得できませんでした。
	NOT_ZERO_CALIB				'ゼロキャリブレーション値が取得できませんでした。
	NOT_FULL_CALIB				'フルキャリブレーション値が取得できませんでした。
	INVALID_IP					'入力されたIPアドレスに接続できませんでした。本体設定を確認してください。
	CONNECTING					'接続中...
	APPLI_CLOSING				'アプリケーションを終了中...
	iDIAG_FCLB_END1				'フル応力キャリブレーションが終了しました。\r\nプローブのロックを解除しOKボタンをクリックしてください。
	iSTRESS_CHG_RATE			'応力変化率[uN/s]

	iDevItem					'デバイスSPEC基本項目名
	iDevUnit					'デバイスSPEC基本単位
	iHCont						'デバイスSPEC 熱板制御方法
	iStrsSens					'デバイスSPEC 応力検知方法
	iAmpMode					'デバイスSPEC 振幅測定モード
	iDevSpecPBInpMode0_Item		'"振幅", "周期"
	iDevSpecPBInpMode1_Item		'"せん断速度 x 3.14", "サンプルひずみ"
	iDevSpecPBInpMode0_Unit		'"mm", "秒"

	iSamItem					'サンプルSPEC基本項目名
	iSamUnit					'サンプルSPEC基本単位
	iShape						'サンプルSPEC 形状
	iSamSpecShape0_Item			'"厚み", "積層枚数"
	iSamSpecShape1_Item			'"密度", "重量"
	iSamSpecShape2_Item			'"密度", "体積"
	iSamSpecShape0_Unit			'"mm", "枚"
	iUnderMsg1					'不正な文字が入力されました。", "設定範囲外の値が入力されました。
	iDIAG_FCLB_END2				'熱板温度を剥し設定温度まで上げますか？
	iInvalidFileFormat			'ファイルフォーマットが不正です。読込に失敗しました。
	iGraphImageSave				'現在表示されているグラフをファイルへ保存します。
	iGraphImageSaveFilter		'イメージファイル(*.jpg;*.bmp;*.png;*.tiff)|*.jpg;*.bmp;*.png;*.tiff|全てのファイル(*.*)|*.*
	iDeviceTimeOut				'本体からの応答が無いので中止しました。
	iZeroAdjComp				'ゼロ調整が終了しました。
	iPrintRepListItem			'測定データレポート リストアイテム文字列
	iMeasStartConf				'測定開始確認
	iMeasStartConf1				'装置エラー発生中のため、測定開始できません
	iMeasStartConf2				'デバイスSPECの入力エラーを確認してください
	iMeasStartConf3				'サンプルSPECの入力エラーを確認してください
	iMeasStartConf4				'動作軌跡が円または線の場合は、応力一定を選択できません
	iMeasStartConf5				'動作軌跡が点の場合は、常時圧力を選択できません
	iMeasStartConf6				'フル応力キャリブレーション設定内に０値が見つかったので測定開始できません。フル応力キャリブレーション設定を確認してください。
	iAdminMode1					'ロックを解除しました
	iAdminMode2					'ロックを設定しました
	iAdminMode3					'パスワードが違います
	iReading					'読込み中です...
	iCalucurating				'計算中です...
	iDevHint					'デバイスSPECヒント
	iSamHint					'サンプルSPECヒント
	iShapeHint					'形状,形状,形状,形状:[厚み]x[積層枚数]がサンプル厚として計算に使用されます。
	iSamSpecShape0_Hint			'厚み,積層枚数
	iSamSpecShape1_Hint			'密度,重量
	iSamSpecShape2_Hint			'密度,体積

	iSpecOverWrite_MSG			'現在のSPEC内容を読込んだ内容で書き換えますか？
	BUTTON_MEASLT				'間欠測定
	BUTTON_STOPLT				'間欠停止
	iMeasStartConf7				'長期間欠測定モードではTHR以外は選択できません
	iMeasStartConf8				'長期間欠測定モードのインターバル時間と動作周期の設定が不正です。
    iCHART_X_TIME_LT			'時間[分]
    iMesMode_err1				' 測定モードが異なります。
	iGelTimeLblSec				'ゲルタイム(秒)
	iGelTimeLblMin				'ゲルタイム(分)
	iScrolWidthSec				'スクロール幅(秒)
	iScrolWidthMin				'スクロール幅(分)
	iXScaleFixedSec				'X軸 固定(秒)
	iXScaleFixedMin				'X軸 固定(分)
	iXScaleFixedDegC			'X軸 固定(℃)
	iTempParaIni				'温度パラメータ(PVBias,PVGain)が初期値になっています。\r\n修復が必要な可能性がありますので、工場出荷時設定へ戻すか、メーカへ問い合わせてください。
	iTempParaWrt				'温度パラメータファイルを初期値で書き換えます。\r\nよろしいですか?
	iTempParaWrtFail			'温度パラメータファイルの書込みに失敗しました。\r\n修復が必要な可能性がありますので、工場出荷時設定へ戻すか、メーカへ問い合わせてください。
	iTempParaIni1				'温度パラメータ(P,I,D,DGain)が変更されています。
	iSGStabIni					'Zセンサ安定判定しきい値[g/10S]が初期値になっています。
	iGapCorrIni					'Z荷重とたわみの比例定数[dig/um]が初期値になっています。
	iMakerHelp					'修復が必要な可能性がありますので、メーカへ問い合わせてください。
	iGrpRangeTooSmall			'指定された範囲は小さ過ぎます。  
	iGrpYAxisRangeOrder			'Y軸範囲を  XXXX  以上に設定してください。
	iGrpXAxisRangeOrder			'X軸範囲を  XXXX  以上に設定してください。
	iQRCodePrepare				'QRコード準備中...
	iQRCodeReceipt				'QRコード受付中...
	iQRCodeComplete				'QRコード受付完了
	iQRCodeSaveTitle			'QRコード画像保存
	iQRCodeSaveFileKind			'PNGファイル|*.png
	iQRCodeSaveFinished			'ファイルが保存されました:
	iDIAG_END_LOCK				'再度、プローブロック解除を確認してOKボタンをクリックしてください。
	iCALIBRATION_NOT_CONFIRMED	'キャリブレーション値が確定されていないので正確な測定ができません。\r\n工場出荷時設定へ戻すかメーカへ問い合わせてください。
	iCONTINUE_CONFIRM			'続行しますか? いいえを選択するとアプリケーションを終了します。

	iMAX
End Enum
'固定フィルタ種別
Enum eFIX_FILTER
    iPARTIAL = 0            '部分一致
    iPREFIX                 '前方一致

    iMAX                    '要素数
End Enum
'Enum eTREE
'    iLV0_EQUIP = 0
'    iLV1_RESIN
'    iLV2_LOT
'    iLV3_NUM

'    iMAX
'End Enum
Enum eAUTOPRINT_FILE
    iJPG = 0                'JPEG
    iPDF                    'PDF

    iMAX                    '要素数
End Enum
'グラフX軸種別
'Enum eX_TYPE
'    iTIME                   '時間
'    iTEMP                   '温度

'    iMAX                    '要素数
'End Enum
'応力単位
'Enum eSTRS_UNIT
'    iDIGITAL                'Dig
'    iPERCENT                '%
'    iNEWTON                 'N

'    iMAX
'End Enum
Enum eTIME_DISP
    iSECONDS = 0
    iDATE

    iMAX
End Enum

'まどか本体状態
Enum eMDK_BODY_TYPE
    iNOT_NEW = -1   '新メモリでない
    iOFFLINE        'オフライン状態
    iINTERLOCK_ON   'インターロック有効
    iINTERLOCK_OFF  'インターロック無効
End Enum

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

'■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
'   パブリック関数
'■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■

'■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
'   プライベート関数
'■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■

End Module
