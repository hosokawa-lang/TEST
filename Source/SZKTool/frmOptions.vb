'***************************************
' File:   frmOptions.vb
' Name:   SIZUKA Tool 詳細設定画面
' Desc:   詳細設定フォーム。
' Author: Fumihiko Kondo
' Date:   2015/04/16
'***************************************

Imports System
Imports System.IO
Imports System.Linq
Imports System.Numerics
Imports System.Runtime.InteropServices
Imports System.Text
Imports System.Windows
Imports System.Windows.Forms.DataVisualization.Charting

Imports System.Reflection
Imports System.Collections
Imports System.Drawing

Imports SZKTool.DataFile
Imports SZKTool.frmMain


''' <summary>
''' ツールの設定を行うフォーム。
''' </summary>
Public Class frmOptions

'▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼
'   定数
'▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲

''' <summary> 通信機器一覧を示す列定義 </summary>
Private Enum EL_COL As Integer
    USEEQUIP = 0
    IP
    NAME
    XYSENS
    ZSENS
    MCF
    TCF
	MAX
End Enum

''' <summary> RTGSETの項目一覧を示す列定義 </summary>
Private Enum RS_COL As Integer
    LINEAR = 0
    TIME
    TEMPHP
    SPD
	MAX
End Enum

Private Enum eTABOpt As Integer
	TOOL		= 0
	ZEROADJ
	GRAPH
	CALUCLATION
	REMOTE
	SPECS
	TEMPCONT
	TEMPPRO
	MEASURE
	AUTOPRINT
	MAX
End Enum

Public Const DF_OPER_LIST_SEP As String = "\"

'グラフマーカスタイル
Public p_lstMarkerStyle As New List(Of String)(New String() { _
"None", _
"Square", _
"Circle", _
"Diamond", _
"Triangle", _
"Cross", _
"Star4", _
"Star5", _
"Star6", _
"Star10" _
})

'▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼
'   構造体
'▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲

'温度プロファイル変化点
Private Structure tpTempProfPointData
    Dim iLenear As Integer          '線形
    Dim sTime As String             '時間
    Dim sTemphp As String           '温度
    Dim sSpd As String              '速度
End Structure

'温度プロファイル
Private Structure tpTempProfData
    Dim sConfName As String                 '設定名
    Dim uPointData() As tpTempProfPointData '温度プロファイル変化点
End Structure


'▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼
'   API宣言
'▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲


'▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼
'   パブリック変数
'▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲

''' <summary> ダイアログ中の適用ボタンが押された場合Trueとする。 </summary>
Friend p_bDlgApply As Boolean = False

'▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼
'   プライベート変数
'▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲

''' <summary> メインフォームの設定状態 </summary>
Private m_ctsMainFrm	As CToolSettings = Nothing

'''' <summary> データフォルダ。 </summary>
'Private m_strDataFolder As String = ""

''' <summary> FTP通信時のユーザ名。 </summary>
Private m_strUserId()	As String = New String(10) {}

''' <summary> FTP通信時のパスワード。 </summary>
Private m_strPassword() As String = New String(10) {}

'''<summary> デフォルトカラー設定ラベルの配列 </summary>
Private m_lblClr()			As Label
Private m_ClrGrid			As Color = Color.LightGray
'Private m_sPreLmtTime		As String
'Private m_sSpecAbort		As String = ""
Private m_PassKey			As String = ""
'Private m_bCnvTrqUnitEna	As Boolean = False         'dgvSpec_CellValueChanged()動作許可

'''<summary> 本体との接続状態。接続表示で使用。CONNECT_ST列挙体で指定 </summary>
Private m_iConnectStateBak	As Integer

Private iRtgEditSave		As Int16	'温度プロファイル設定状態　0:無編集 1:編集あり 2:保存済

'多言語メッセージ格納
Private m_sStr(eMes.iMAX - 1)		 As String   
Enum eMes
	iMarkerSelAry				'マーカスタイル選択文字列
	iGColorDesc					'グラフ色割当て説明
	iTempProHeader				'温度プロファイル設定表ヘッダ
    iDEL_SPEC					'選択されたSPECを削除してもよろしいですか?
    iSPECNAME_NOTHING			'スペック名が入力されていません。
    iRTG_TIMEERR				'時間の入力が異常です。
    iRTGNAME_NOTHING			'温度プロファイル名が入力されていません。
    iAUTOPRINT_DISABLE			'保存先フォルダが正しくありません。\r\n自動保存を無効にします。
    iEXPORT_COMP				'ファイルのエクスポートが完了しました。
    iEXPORT_FAIL				'ファイルのエクスポートが失敗しました。
    iIMPORT_COMP				'ファイルのインポートが完了しました。
    iIMPORT_FAIL				'ファイルのインポートが失敗しました。
	iTEMPPROF_MAX_OVER			'保存可能な最大件数を #VALUE# 件超過しています。
    iXYMODE_NOTHING             'XYモードが選択されていません。
	INVALID_INPUT_VALUE			'スペックに無効な入力値が含まれています。
	INVALID_INPUT_DIGIT			'(小数桁数)	
	INVALID_INPUT_IP			'IPアドレスが入力されていません。
	GAPCALIB_START				'開始
	GAPCALIB_CANSEL             '中止
	GAPCALIB_CLR_MSG			'Z位置キャリブレーションデータがクリアされます。vbCrlf よろしいですか?
	MSG_YES						'はい
	MSG_NO						'いいえ
	GAPCALIB_OK					'キャリブレーション済
	GAPCALIB_NY					'キャリブレーション未
	FACTORY_CONF				'工場出荷時戻す確認
	FACTORY_SUCCESS				'工場出荷時 成功
	FACTORY_FAILE				'工場出荷時 失敗
	iMAX
End Enum

'■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
'   コンストラクタ／デストラクタ
'■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
'clCtoolSettings.basへ移動
#If 0

''' <summary>
''' ツールの設定状態を保管するクラス。
''' </summary>
Public Class CToolSettings

'温調パラメータ初期値
Public const	cdP			As Double	= 0.20	
Public const	cdI			As Double	= 0.8	
Public const	cdD			As Double	= 3.00		
Public const	cdDgain		As Double	= 0.20	
Public const	cdPVBias	As Double	= 0.0
Public const	cdPVGain	As Double	= 1.0000	
Public const	cdP2		As Double	= 0.50	
Public const	cdI2		As Double	= 1.5	
Public const	cdD2		As Double	= 3.00		
Public const	cdDgain2	As Double	= 0.05	
Public const	cdPVBias2	As Double	= 0.0	
Public const	cdPVGain2	As Double	= 1.0000	
Public const	cdP3		As Double	= 0.50	
Public const	cdI3		As Double	= 1.5	
Public const	cdD3		As Double	= 3.00		
Public const	cdDgain3	As Double	= 0.05	
Public const	cdPVBias3	As Double	= 0.0	
Public const	cdPVGain3	As Double	= 1.0000 

'起歪体応力安定度[g/10s] 初期値
Public const	cStressStab	As Double	= 0.1
'測定GAP補正値 dig/um
Public Const	cGapCorr	As Double	= 100.0

' コピーを作成するメソッド
Public Function Clone() As CToolSettings
	Return CType(MemberwiseClone(), CToolSettings)
End Function

Private iLangValue As Integer = 0
''' <summary> 設定言語。-1以下:未設定, 1:JPN, その他:ENG,  </summary>
Public Property iLang() As Integer
    Get
        Return iLangValue
    End Get
    Set(ByVal value As Integer)
        iLangValue = value
    End Set
End Property
Private iMultiActivate As Integer = 0
''' <summary> 多重起動許可 0:禁止 1:許可  </summary>
Public Property MultiActivate() As Integer
    Get
        Return iMultiActivate
    End Get
    Set(ByVal value As Integer)
        iMultiActivate = value
    End Set
End Property
Private iHideNgtvValue As Integer = 1
''' <summary> 負の値を表示。0:表示, 1:非表示 </summary>
Public Property iHideNgtv() As Integer
    Get
        Return iHideNgtvValue
    End Get
    Set(ByVal value As Integer)
        iHideNgtvValue = value
    End Set
End Property
Private iDetailsValueR As Int32 = 1000
''' <summary> データプロットの間隔(間引き数)Rough </summary>
Public Property iDetailsR() As Int32
    Get
        Return iDetailsValueR
    End Get
    Set(ByVal value As Int32)
        iDetailsValueR = value
    End Set
End Property
Private iDetailsValueD As Int32 = 100
''' <summary> データプロットの間隔(間引き数)Detail </summary>
Public Property iDetailsD() As Int32
    Get
        Return iDetailsValueD
    End Get
    Set(ByVal value As Int32)
        iDetailsValueD = value
    End Set
End Property
Private iARDispValue As Integer = 0
''' <summary> 近似範囲の表示方法。0:塗りつぶし, 1:透明 </summary>
Public Property iARDisp() As Integer
    Get
        Return iARDispValue
    End Get
    Set(ByVal value As Integer)
        iARDispValue = value
    End Set
End Property
'Private iColTagValue As Integer()
'''' <summary> エクスポートする項目の列順。 </summary>
'Public Property iColTag() As Integer()
'    Get
'        Return iColTagValue
'    End Get
'    Set(ByVal value As Integer())
'        iColTagValue = value
'    End Set
'End Property

Private iSelMDKValue As Integer = 0
''' <summary> 通信対象に選択している機器。オプション上の行番号で指定。 </summary>
Public Property iSelMDK As Integer
    Get
        Return iSelMDKValue
    End Get
    Set(ByVal value As Integer)
        iSelMDKValue = value
    End Set
End Property

Private strIPListValue() As String = New String(10) {}
''' <summary> SIZUKA 本体のIPアドレス一覧。 </summary>
Public Property strIP() As String()
    Get
        Return strIPListValue
    End Get
    Set(ByVal value As String())
        strIPListValue = value
    End Set
End Property

Private strEquipNameValue() As String = New String(10) {}
''' <summary> SIZUKA 本体の名称一覧。書式:MDK000 </summary>
Public Property strEquipName() As String()
    Get
        Return strEquipNameValue
    End Get
    Set(ByVal value As String())
        strEquipNameValue = value
    End Set
End Property
Private sXYSensorSerialValue() As String = New String(10) {}
''' <summary> SIZUKA 本体のXYセンサ連番 書式:SZS-00-000 </summary>
Public Property sXYSensorSerial() As String()
    Get
        Return sXYSensorSerialValue
    End Get
    Set(ByVal value As String())
        sXYSensorSerialValue = value
    End Set
End Property
Private sZSensorSerialValue() As String = New String(10) {}
''' <summary> SIZUKA 本体のZセンサ連番 書式:SZS-90-999 </summary>
Public Property sZSensorSerial() As String()
    Get
        Return sZSensorSerialValue
    End Get
    Set(ByVal value As String())
        sZSensorSerialValue = value
    End Set
End Property

Private dCF1Value As Double() = New Double(10) {}
''' <summary> 機器単独動作時の校正係数1乗。 </summary>
Public Property dCF1 As Double()
    Get
        Return dCF1Value
    End Get
    Set(ByVal value As Double())
        dCF1Value = value
    End Set
End Property

Private dCF2Value As Double() = New Double(10) {}
''' <summary> 機器単独動作時の校正係数2乗。 </summary>
Public Property dCF2 As Double()
    Get
        Return dCF2Value
    End Get
    Set(ByVal value As Double())
        dCF2Value = value
    End Set
End Property

Private dCF3Value As Double() = New Double(10) {}
''' <summary> 機器単独動作時の校正係数1乗。 </summary>
Public Property dCF3 As Double()
    Get
        Return dCF3Value
    End Get
    Set(ByVal value As Double())
        dCF3Value = value
    End Set
End Property

Private dTCF6Value As Double() = New Double(10) {}
''' <summary> ツール用校正係数6乗。 </summary>
Public Property dTCF6 As Double()
    Get
        Return dTCF6Value
    End Get
    Set(ByVal value As Double())
        dTCF6Value = value
    End Set
End Property

Private dTCF5Value As Double() = New Double(10) {}
''' <summary> ツール用校正係数5乗。 </summary>
Public Property dTCF5 As Double()
    Get
        Return dTCF5Value
    End Get
    Set(ByVal value As Double())
        dTCF5Value = value
    End Set
End Property

Private dTCF4Value As Double() = New Double(10) {}
''' <summary> ツール用校正係数4乗。 </summary>
Public Property dTCF4 As Double()
    Get
        Return dTCF4Value
    End Get
    Set(ByVal value As Double())
        dTCF4Value = value
    End Set
End Property

Private dTCF3Value As Double() = New Double(10) {}
''' <summary> ツール用校正係数3乗。 </summary>
Public Property dTCF3 As Double()
    Get
        Return dTCF3Value
    End Get
    Set(ByVal value As Double())
        dTCF3Value = value
    End Set
End Property

Private dTCF2Value As Double() = New Double(10) {}
''' <summary> ツール用校正係数2乗。 </summary>
Public Property dTCF2 As Double()
    Get
        Return dTCF2Value
    End Get
    Set(ByVal value As Double())
        dTCF2Value = value
    End Set
End Property

Private dTCF1Value As Double() = New Double(10) {}
''' <summary> ツール用校正係数1乗。 </summary>
Public Property dTCF1 As Double()
    Get
        Return dTCF1Value
    End Get
    Set(ByVal value As Double())
        dTCF1Value = value
    End Set
End Property

Private strPortValue As String = ""
''' <summary> SIZUKA 本体に接続する際のポート。 </summary>
Public Property strPort() As String
    Get
        Return strPortValue
    End Get
    Set(ByVal value As String)
        strPortValue = value
    End Set
End Property

Private iRecentValue As Integer = 0
''' <summary> 最近の測定表示の期間(日数)。 </summary>
Public Property iRecent As Integer
    Get
        Return iRecentValue
    End Get
    Set(ByVal value As Integer)
        iRecentValue = value
    End Set
End Property

Private iAutoStartValue As Integer = -1
''' <summary> 自動開始の待ち時間。負の値で自動開始無効 </summary>
Public Property iAutoStart As Integer
    Get
        Return iAutoStartValue
    End Get
    Set(ByVal value As Integer)
        iAutoStartValue = value
    End Set
End Property

Private penGridsValue As Pen = New Pen(Brushes.LightGray, 1.0F)
''' <summary> グリッド描画用のペン </summary>
Public Property penGrids() As Pen
    Get
        'penGridsValue.Color = p_clrGrid
        Return penGridsValue
    End Get
    Set(ByVal value As Pen)
        penGridsValue = value
    End Set
End Property

Private strServerNameValue As String = ""
''' <summary> データベースサーバ。 </summary>
Public Property strServerName() As String
    Get
        Return strServerNameValue
    End Get
    Set(ByVal value As String)
        strServerNameValue = value
    End Set
End Property

'ADD START M_UNO 20150831 Ver.1.0.3.7 Y1デフォルト、背景色等カラー設定
Private clrY1DefaultVal() As Color = New Color(9) {Color.Black, Color.Brown, _
                                                    Color.Red, Color.DarkOrange, _
                                                    Color.Gold, Color.Green, _
                                                    Color.Blue, Color.Purple, _
                                                    Color.Gray, Color.Cyan}
''' <summary> Y1デフォルトカラー。 </summary>
Public Property clrY1Default() As Color()
    Get
        Return clrY1DefaultVal
    End Get
    Set(ByVal value As Color())
        clrY1DefaultVal = value
    End Set
End Property

Private clrGraphBackVal As Color = Color.White
''' <summary> グラフ背景色。 </summary>
Public Property clrGraphBack() As Color
    Get
        Return clrGraphBackVal
    End Get
    Set(ByVal value As Color)
        clrGraphBackVal = value
    End Set
End Property
Private strPrntMesPlaceValue As String = ""
''' <summary> 印刷設定　測定場所。 </summary>
Public Property strPrntMesPlace() As String
    Get
        Return strPrntMesPlaceValue
    End Get
    Set(ByVal value As String)
        strPrntMesPlaceValue = value
    End Set
End Property

Private strPrntMesPersonValue As String = ""
''' <summary> 印刷設定　測定者。 </summary>
Public Property strPrntMesPerson() As String
    Get
        Return strPrntMesPersonValue
    End Get
    Set(ByVal value As String)
        strPrntMesPersonValue = value
    End Set
End Property

Private strPrntCommentValue As String = ""
''' <summary> 印刷設定　コメント。 </summary>
Public Property strPrntComment() As String
    Get
        Return strPrntCommentValue
    End Get
    Set(ByVal value As String)
        strPrntCommentValue = value
    End Set
End Property
'ADD START M_UNO 20160808  Ver.2.0.1.2  統計グラフ色設定追加
Private strStatsLineClr As String = ""

''' <summary> 統計ラインカラー設定文字列。 </summary>
Public Property StatsLineClr As String
    Get
        Return strStatsLineClr
    End Get
    Set(ByVal value As String)
        strStatsLineClr = value
    End Set
End Property

'Private iEachZeroAdjValue As Integer = False
'''' <summary> 毎回ゼロ調フラグ </summary>
'Public Property iEachZeroAdj() As Integer
'    Get
'        Return iEachZeroAdjValue
'    End Get
'    Set(ByVal value As Integer)
'        iEachZeroAdjValue = value
'    End Set
'End Property

Private iRotaStartLateValue As Integer = 0
''' <summary> インターロック閉後、回転待ち時間 </summary>
Public Property iRotaStartLate() As Integer
    Get
        Return iRotaStartLateValue
    End Get
    Set(ByVal value As Integer)
        iRotaStartLateValue = value
    End Set
End Property

Private iBodySetEnableValue As Integer = eMDK_BODY_TYPE.iOFFLINE
''' <summary> まどか本体設定有効フラグ </summary>
Public Property iBodySetEnable() As Integer
    Get
        Return iBodySetEnableValue
    End Get
    Set(ByVal value As Integer)
        iBodySetEnableValue = value
    End Set
End Property

'Auto Printの有効/無効
Private iAutoPrintEnable As Integer = 0
''' <summary> Auto Printの有効/無効 </summary>
Public Property iAutoPrintEnableValue As Integer
    Get
        Return iAutoPrintEnable
    End Get
    Set(ByVal value As Integer)
        iAutoPrintEnable = value
    End Set
End Property

'Auto Printの保存ファイルタイプ
Private iAutoPrintType As Integer = 0
''' <summary> Auto Printの保存ファイルタイプ </summary>
Public Property iAutoPrintTypeValue As Integer
    Get
        Return iAutoPrintType
    End Get
    Set(ByVal value As Integer)
        iAutoPrintType = value
    End Set
End Property

'Auto Printの保存先
Private strAutoPrintOutPutFolder As String = ""

''' <summary> Auto Printの保存先 </summary>
Public Property strAutoPrintOutPutFolderValue As String
    Get
        Return strAutoPrintOutPutFolder
    End Get
    Set(ByVal value As String)
        strAutoPrintOutPutFolder = value
    End Set
End Property

'Auto Printの測定場所
Private strAutoPrintMesurePlace As String = ""

''' <summary> Auto Printの保存先 </summary>
Public Property strAutoPrintMesurePlaceValue As String
    Get
        Return strAutoPrintMesurePlace
    End Get
    Set(ByVal value As String)
        strAutoPrintMesurePlace = value
    End Set
End Property

'Auto Printの測定場所リスト
Private m_sAutoPrintMesurePlaceList As String = ""

''' <summary> Auto Printの測定場所リスト </summary>
Public Property sAutoPrintMesurePlaceList As String
    Get
        Return m_sAutoPrintMesurePlaceList
    End Get
    Set(ByVal value As String)
        m_sAutoPrintMesurePlaceList = value
    End Set
End Property

'Auto Printの測定者
Private strAutoPrintOperator As String = ""

''' <summary> Auto Printの測定者 </summary>
Public Property strAutoPrintOperatorValue As String
    Get
        Return strAutoPrintOperator
    End Get
    Set(ByVal value As String)
        strAutoPrintOperator = value
    End Set
End Property
'Auto Printの測定者リスト
Private m_sAutoPrintOperatorList As String = ""

''' <summary> Auto Printの測定者リスト </summary>
Public Property sAutoPrintOperatorList As String
    Get
        Return m_sAutoPrintOperatorList
    End Get
    Set(ByVal value As String)
        m_sAutoPrintOperatorList = value
    End Set
End Property

'Auto Printのコメント
Private strAutoPrintComment As String = ""

''' <summary> Auto Printのコメント </summary>
Public Property strAutoPrint_Comment_Value As String
    Get
        Return strAutoPrintComment
    End Get
    Set(ByVal value As String)
        strAutoPrintComment = value
    End Set
End Property

'Auto Printのディスク容量
Private iAutoPrintDiskCapacity As Integer = 10
''' <summary> Auto Printの保存ファイルタイプ </summary>
Public Property iAutoPrintDiskCapacityValue As Integer
    Get
        Return iAutoPrintDiskCapacity
    End Get
    Set(ByVal value As Integer)
        iAutoPrintDiskCapacity = value
    End Set
End Property

Private strDefDspMthdValue As String = ""
''' <summary> デフォルト表示メソッド。 </summary>
Public Property strDefDspMth() As String
    Get
        Return strDefDspMthdValue
    End Get
    Set(ByVal value As String)
        strDefDspMthdValue = value
    End Set
End Property
'Private iStressUnitValue As Integer = eSTRS_UNIT.iDIGITAL
'''' <summary> トルク単位。 </summary>
'Public Property iStrsUnit() As Integer
'    Get
'        Return iStressUnitValue
'    End Get
'    Set(ByVal value As Integer)
'        iStressUnitValue = value
'    End Set
'End Property
Private iCalibPointsValue As Integer = 4
''' <summary> キャリブ点数。 </summary>
Public Property iCalibPoints() As Integer
    Get
        Return iCalibPointsValue
    End Get
    Set(ByVal value As Integer)
        iCalibPointsValue = value
    End Set
End Property

Private iCalibToleranceValue As Integer = 100
''' <summary> キャリブ許容値。 </summary>
Public Property iCalibTolerance() As Integer
    Get
        Return iCalibToleranceValue
    End Get
    Set(ByVal value As Integer)
        iCalibToleranceValue = value
    End Set
End Property

Private sCalibAngleValue As String = ""
''' <summary> キャリブ角度。 </summary>
Public Property sCalibAngle() As String
    Get
        Return sCalibAngleValue
    End Get
    Set(ByVal value As String)
        sCalibAngleValue = value
    End Set
End Property
Private iFixSpecLotManuValue As Integer = 0
''' <summary> 固定スペックロットNo手入力許可 </summary>
Public Property iFixSpecLotManu() As Integer
    Get
        Return iFixSpecLotManuValue
    End Get
    Set(ByVal value As Integer)
        iFixSpecLotManuValue = value
    End Set
End Property
Private m_sSumYstDayValue As String = 0
''' <summary> 年間の開始日"MM/dd" </summary>
Public Property sSumYstDay() As String
    Get
        Return m_sSumYstDayValue
    End Get
    Set(ByVal value As String)
        m_sSumYstDayValue = value
    End Set
End Property

Private m_sSumMstDayValue As String = 0
''' <summary> 月間の開始日"dd" </summary>
Public Property sSumMstDay() As String
    Get
        Return m_sSumMstDayValue
    End Get
    Set(ByVal value As String)
        m_sSumMstDayValue = value
    End Set
End Property

Private m_sSumFreeStValue As String = 0
''' <summary> 自由集計開始日。 </summary>
Public Property sSumFreeSt() As String
    Get
        Return m_sSumFreeStValue
    End Get
    Set(ByVal value As String)
        m_sSumFreeStValue = value
    End Set
End Property

Private m_sSumFreeEdValue As String = 0
''' <summary> 自由集計終了日。 </summary>
Public Property sSumFreeEd() As String
    Get
        Return m_sSumFreeEdValue
    End Get
    Set(ByVal value As String)
        m_sSumFreeEdValue = value
    End Set
End Property

Private m_iSpecHeaderIdxMax As Integer = 0          'Export可能列の最終配列インデックス番号
''' <summary> Export可能列の最終配列インデックス番号。 </summary>
Public Property iSpecHeaderIdxMax() As Integer
    Get
        Return m_iSpecHeaderIdxMax
    End Get
    Set(ByVal value As Integer)
        m_iSpecHeaderIdxMax = value
    End Set
End Property
Private m_sMntTodayChkDate As String = ""           'メンテナンス積算時間超過チェック済み最終日
''' <summary> メンテナンス積算時間超過チェック済み最終日。 </summary>
Public Property sMntTodayChkDate() As String
    Get
        Return m_sMntTodayChkDate
    End Get
    Set(ByVal value As String)
        m_sMntTodayChkDate = value
    End Set
End Property

Private m_iMntChkIntegHourPowerOn As Integer = 0        '本体電源ON時間の超過判定閾値
''' <summary> 本体電源ON時間の超過判定閾値。 </summary>
Public Property iMntChkIntegHourPowerOn() As Integer
    Get
        Return m_iMntChkIntegHourPowerOn
    End Get
    Set(ByVal value As Integer)
        m_iMntChkIntegHourPowerOn = value
    End Set
End Property

Private m_iMntChkIntegHourHeaterOn As Integer = 0       'HPヒータON時間の超過判定閾値
''' <summary> HPヒータON時間の超過判定閾値。 </summary>
Public Property iMntChkIntegHourHeaterOn() As Integer
    Get
        Return m_iMntChkIntegHourHeaterOn
    End Get
    Set(ByVal value As Integer)
        m_iMntChkIntegHourHeaterOn = value
    End Set
End Property

Private m_iMntChkIntegHourXAct As Integer = 0			'X動作時間の超過判定閾値
''' <summary> XY動作時間の超過判定閾値。 </summary>
Public Property iMntChkIntegHourXAct() As Integer
    Get
        Return m_iMntChkIntegHourXAct
    End Get
    Set(ByVal value As Integer)
        m_iMntChkIntegHourXAct = value
    End Set
End Property
Private m_iMntChkIntegHourYAct As Integer = 0			'Y動作時間の超過判定閾値
''' <summary> Y動作時間の超過判定閾値。 </summary>
Public Property iMntChkIntegHourYAct() As Integer
    Get
        Return m_iMntChkIntegHourYAct
    End Get
    Set(ByVal value As Integer)
        m_iMntChkIntegHourYAct = value
    End Set
End Property
Private m_iMntChkIntegHourZAct As Integer = 0			'Z動作時間の超過判定閾値
''' <summary> Z動作時間の超過判定閾値。 </summary>
Public Property iMntChkIntegHourZAct() As Integer
    Get
        Return m_iMntChkIntegHourZAct
    End Get
    Set(ByVal value As Integer)
        m_iMntChkIntegHourZAct = value
    End Set
End Property


Private m_iMntChkIntegHourHeaterOn2 As Integer = 0       'PBヒータON時間の超過判定閾値
''' <summary> PBヒータON時間の超過判定閾値。 </summary>
Public Property iMntChkIntegHourHeaterOn2() As Integer
    Get
        Return m_iMntChkIntegHourHeaterOn2
    End Get
    Set(ByVal value As Integer)
        m_iMntChkIntegHourHeaterOn2 = value
    End Set
End Property

Private m_iMntChkIntegHourHeaterOn3 As Integer = 0       'SGヒータON時間の超過判定閾値
''' <summary> SGヒータON時間の超過判定閾値。 </summary>
Public Property iMntChkIntegHourHeaterOn3() As Integer
    Get
        Return m_iMntChkIntegHourHeaterOn3
    End Get
    Set(ByVal value As Integer)
        m_iMntChkIntegHourHeaterOn3 = value
    End Set
End Property

Private m_iMntChkIntegHourTrqSens As Integer = 0        'トルクセンサ稼動ON時間の超過判定閾値
''' <summary> トルクセンサ稼動ON時間の超過判定閾値。 </summary>
Public Property iMntChkIntegHourTrqSens() As Integer
    Get
        Return m_iMntChkIntegHourTrqSens
    End Get
    Set(ByVal value As Integer)
        m_iMntChkIntegHourTrqSens = value
    End Set
End Property

Private m_iMntChkRstYearPowerOn As Integer = 0        '本体電源ON経過年数の超過判定閾値
''' <summary> 本体電源ON経過日数の超過判定閾値。 </summary>
Public Property iMntChkRstYearPowerOn() As Integer
    Get
        Return m_iMntChkRstYearPowerOn
    End Get
    Set(ByVal value As Integer)
        m_iMntChkRstYearPowerOn = value
    End Set
End Property

Private m_iMntChkRstYearHeaterOn As Integer = 0       'HPヒータON経過年数の超過判定閾値
''' <summary> HPヒータON経過日数の超過判定閾値。 </summary>
Public Property iMntChkRstYearHeaterOn() As Integer
    Get
        Return m_iMntChkRstYearHeaterOn
    End Get
    Set(ByVal value As Integer)
        m_iMntChkRstYearHeaterOn = value
    End Set
End Property

Private m_iMntChkRstYearXAct As Integer = 0			'X動作経過年数の超過判定閾値
''' <summary> X動作 経過日数の超過判定閾値。 </summary>
Public Property iMntChkRstYearXAct() As Integer
    Get
        Return m_iMntChkRstYearXAct
    End Get
    Set(ByVal value As Integer)
        m_iMntChkRstYearXAct = value
    End Set
End Property
Private m_iMntChkRstYearYAct As Integer = 0			'Y動作経過年数の超過判定閾値
''' <summary> Y動作 経過日数の超過判定閾値。 </summary>
Public Property iMntChkRstYearYAct() As Integer
    Get
        Return m_iMntChkRstYearYAct
    End Get
    Set(ByVal value As Integer)
        m_iMntChkRstYearYAct = value
    End Set
End Property
Private m_iMntChkRstYearZAct As Integer = 0			'Z動作経過年数の超過判定閾値
''' <summary> Z動作 経過日数の超過判定閾値。 </summary>
Public Property iMntChkRstYearZAct() As Integer
    Get
        Return m_iMntChkRstYearZAct
    End Get
    Set(ByVal value As Integer)
        m_iMntChkRstYearZAct = value
    End Set
End Property

Private m_iMntChkRstYearHeaterOn2 As Integer = 0       'PBヒータON経過年数の超過判定閾値
''' <summary> PBヒータON経過日数の超過判定閾値。 </summary>
Public Property iMntChkRstYearHeaterOn2() As Integer
    Get
        Return m_iMntChkRstYearHeaterOn2
    End Get
    Set(ByVal value As Integer)
        m_iMntChkRstYearHeaterOn2 = value
    End Set
End Property

Private m_iMntChkRstYearHeaterOn3 As Integer = 0       'SGヒータON経過年数の超過判定閾値
''' <summary> PBヒータON経過日数の超過判定閾値。 </summary>
Public Property iMntChkRstYearHeaterOn3() As Integer
    Get
        Return m_iMntChkRstYearHeaterOn3
    End Get
    Set(ByVal value As Integer)
        m_iMntChkRstYearHeaterOn3 = value
    End Set
End Property

Private m_iMntChkRstYearTrqSens As Integer = 0        'トルクセンサ稼動経過年数の超過判定閾値
''' <summary> トルクセンサ稼動経過日数の超過判定閾値。 </summary>
Public Property iMntChkRstYearTrqSens() As Integer
    Get
        Return m_iMntChkRstYearTrqSens
    End Get
    Set(ByVal value As Integer)
        m_iMntChkRstYearTrqSens = value
    End Set
End Property
'測定結果自動保存の有効/無効
Private m_iAutoSaveMeasDataEnable As Integer = 0
''' <summary> 測定結果自動保存の有効/無効 </summary>
Public Property iAutoSaveMeasDataEnable As Integer
    Get
        Return m_iAutoSaveMeasDataEnable
    End Get
    Set(ByVal value As Integer)
        m_iAutoSaveMeasDataEnable = value
    End Set
End Property

'測定結果自動保存の保存先
Private m_sAutoSaveMeasDataOutPutFolder As String = ""
''' <summary> 測定結果自動保存の保存先 </summary>
Public Property sAutoSaveMeasDataOutPutFolder As String
    Get
        Return m_sAutoSaveMeasDataOutPutFolder
    End Get
    Set(ByVal value As String)
        m_sAutoSaveMeasDataOutPutFolder = value
    End Set
End Property

'測定結果ローカル自動保存の保存先
Private m_sAutoSaveMeasDataFolder As String = ""
''' <summary> データフォルダの絶対パス </summary>
Public Property sAutoSaveMeasDataFolder() As String
    Get
        Return m_sAutoSaveMeasDataFolder
    End Get
    Set(ByVal value As String)
        m_sAutoSaveMeasDataFolder = value
    End Set
End Property
'ADD END   IKEDA Ver3.2.0.0 20180126 測定結果自動保存機能の追加
'******************************************

'******************************************
'ADD START IKEDA Ver3.2.1.0 20180703 SQL Serverエクスポート機能追加
'SQL Serverエクスポート機能の有効／無効
Private m_iSqlSvrExpEnable As Integer = 0
''' <summary> SQL Serverエクスポート機能の有効／無効 </summary>
Public Property iSqlSvrExpEnable As Integer
    Get
        Return m_iSqlSvrExpEnable
    End Get
    Set(ByVal value As Integer)
        m_iSqlSvrExpEnable = value
    End Set
End Property

'SQL Server名
Private m_sSqlSvrExpSvrName As String = ""
''' <summary> SQL Server名 </summary>
Public Property sSqlSvrExpSvrName() As String
    Get
        Return m_sSqlSvrExpSvrName
    End Get
    Set(ByVal value As String)
        m_sSqlSvrExpSvrName = value
    End Set
End Property

'SQL Server認証種別
Private m_iSqlSvrExpAuthType As Integer = 0
''' <summary> SQL Server認証種別 </summary>
Public Property iSqlSvrExpAuthType As Integer
    Get
        Return m_iSqlSvrExpAuthType
    End Get
    Set(ByVal value As Integer)
        m_iSqlSvrExpAuthType = value
    End Set
End Property

'SQL Server認証ログイン名
Private m_sSqlSvrExpUserName As String = ""
''' <summary> SQL Server認証ログイン名 </summary>
Public Property sSqlSvrExpUserName() As String
    Get
        Return m_sSqlSvrExpUserName
    End Get
    Set(ByVal value As String)
        m_sSqlSvrExpUserName = value
    End Set
End Property

'SQL Server認証パスワード
Private m_sSqlSvrExpPassword As String = ""
''' <summary> SQL Server認証パスワード </summary>
Public Property sSqlSvrExpPassword() As String
    Get
        Return m_sSqlSvrExpPassword
    End Get
    Set(ByVal value As String)
        m_sSqlSvrExpPassword = value
    End Set
End Property

'SQL Serverエクスポートデータファイルの出力先パス
Private m_sSqlSvrExpDataPath As String = ""
''' <summary> SQL Serverエクスポートデータファイルの出力先パス </summary>
Public Property sSqlSvrExpDataPath() As String
    Get
        Return m_sSqlSvrExpDataPath
    End Get
    Set(ByVal value As String)
        m_sSqlSvrExpDataPath = value
    End Set
End Property
'SQL Serverエクスポート機能の有効／無効
Private m_iNotChkExpOldVer As Integer = 0
''' <summary> エクスポート時に旧バージョン確認しない </summary>
Public Property iNotChkExpOldVer As Integer
    Get
        Return m_iNotChkExpOldVer
    End Get
    Set(ByVal value As Integer)
        m_iNotChkExpOldVer = value
    End Set
End Property
'熱板 温度制御 P 比例定数
Private m_dP As Double
Public Property TempCnt_P As Double
    Get
        Return m_dP
    End Get
    Set(ByVal value As Double)
        m_dP = value
    End Set
End Property
'熱板 温度制御 I 積分定数
Private m_dI As Double
Public Property TempCnt_I As Double
    Get
        Return m_dI
    End Get
    Set(ByVal value As Double)
        m_dI = value
    End Set
End Property
'熱板 温度制御 D 積分定数(S)
Private m_dD As Double
Public Property TempCnt_D As Double
    Get
        Return m_dD
    End Get
    Set(ByVal value As Double)
        m_dD = value
    End Set
End Property
'熱板 温度制御 Dgain 微分ゲイン
Private m_dDgain As Double
Public Property TempCnt_Dgain As Double
    Get
        Return m_dDgain
    End Get
    Set(ByVal value As Double)
        m_dDgain = value
    End Set
End Property
'熱板 温度制御 PVBias 温度補正(℃)
Private m_dPVBias As Double
Public Property TempCnt_PVBias As Double
    Get
        Return m_dPVBias
    End Get
    Set(ByVal value As Double)
        m_dPVBias = value
    End Set
End Property
'熱板 温度制御 PVGain 温度補正 0 - 6.5535
Private m_dPVGain As Double
Public Property TempCnt_PVGain As Double
    Get
        Return m_dPVGain
    End Get
    Set(ByVal value As Double)
        m_dPVGain = value
    End Set
End Property
'熱板 温度制御 ヒータ上限温度(℃)
Private m_dHUpperLimit As Double
Public Property TempCnt_HUpperLimit As Double
    Get
        Return m_dHUpperLimit
    End Get
    Set(ByVal value As Double)
        m_dHUpperLimit = value
    End Set
End Property

'プローブ 温度制御 P 比例定数
Private m_dP2 As Double
Public Property TempCnt_P2 As Double
    Get
        Return m_dP2
    End Get
    Set(ByVal value As Double)
        m_dP2 = value
    End Set
End Property
'プローブ 温度制御 I 積分定数
Private m_dI2 As Double
Public Property TempCnt_I2 As Double
    Get
        Return m_dI2
    End Get
    Set(ByVal value As Double)
        m_dI2 = value
    End Set
End Property
'プローブ 温度制御 D 積分定数(S)
Private m_dD2 As Double
Public Property TempCnt_D2 As Double
    Get
        Return m_dD2
    End Get
    Set(ByVal value As Double)
        m_dD2 = value
    End Set
End Property
'プローブ 温度制御 Dgain 微分ゲイン
Private m_dDgain2 As Double
Public Property TempCnt_Dgain2 As Double
    Get
        Return m_dDgain2
    End Get
    Set(ByVal value As Double)
        m_dDgain2 = value
    End Set
End Property
'プローブ 温度制御 PVBias 温度補正(℃)
Private m_dPVBias2 As Double
Public Property TempCnt_PVBias2 As Double
    Get
        Return m_dPVBias2
    End Get
    Set(ByVal value As Double)
        m_dPVBias2 = value
    End Set
End Property
'プローブ 温度制御 PVGain 温度補正 0 - 6.5535
Private m_dPVGain2 As Double
Public Property TempCnt_PVGain2 As Double
    Get
        Return m_dPVGain2
    End Get
    Set(ByVal value As Double)
        m_dPVGain2 = value
    End Set
End Property
'プローブ 温度制御 ヒータ上限温度(℃)
Private m_dHUpperLimit2 As Double
Public Property TempCnt_HUpperLimit2 As Double
    Get
        Return m_dHUpperLimit2
    End Get
    Set(ByVal value As Double)
        m_dHUpperLimit2 = value
    End Set
End Property

'起歪体 温度制御 P 比例定数
Private m_dP3 As Double
Public Property TempCnt_P3 As Double
    Get
        Return m_dP3
    End Get
    Set(ByVal value As Double)
        m_dP3 = value
    End Set
End Property
'起歪体 温度制御 I 積分定数
Private m_dI3 As Double
Public Property TempCnt_I3 As Double
    Get
        Return m_dI3
    End Get
    Set(ByVal value As Double)
        m_dI3 = value
    End Set
End Property
'起歪体 温度制御 D 積分定数(S)
Private m_dD3 As Double
Public Property TempCnt_D3 As Double
    Get
        Return m_dD3
    End Get
    Set(ByVal value As Double)
        m_dD3 = value
    End Set
End Property
'起歪体 温度制御 Dgain 微分ゲイン
Private m_dDgain3 As Double
Public Property TempCnt_Dgain3 As Double
    Get
        Return m_dDgain3
    End Get
    Set(ByVal value As Double)
        m_dDgain3 = value
    End Set
End Property
'起歪体 温度制御 PVBias 温度補正(℃)
Private m_dPVBias3 As Double
Public Property TempCnt_PVBias3 As Double
    Get
        Return m_dPVBias3
    End Get
    Set(ByVal value As Double)
        m_dPVBias3 = value
    End Set
End Property
'起歪体 温度制御 PVGain 温度補正 0 - 6.5535
Private m_dPVGain3 As Double
Public Property TempCnt_PVGain3 As Double
    Get
        Return m_dPVGain3
    End Get
    Set(ByVal value As Double)
        m_dPVGain3 = value
    End Set
End Property
'起歪体 温度制御 ヒータ上限温度(℃)
Private m_dHUpperLimit3 As Double
Public Property TempCnt_HUpperLimit3 As Double
    Get
        Return m_dHUpperLimit3
    End Get
    Set(ByVal value As Double)
        m_dHUpperLimit3 = value
    End Set
End Property
'起歪体 温度制御 ヒータ下限温度(℃)
Private m_dHLowerLimit3 As Double
Public Property TempCnt_HLowerLimit3 As Double
    Get
        Return m_dHLowerLimit3
    End Get
    Set(ByVal value As Double)
        m_dHLowerLimit3 = value
    End Set
End Property
'起歪体 温度制御 ヒータ差分設定温度(℃)
Private m_dHDifSet3 As Double
Public Property TempCnt_HDifSetTemp3 As Double
    Get
        Return m_dHDifSet3
    End Get
    Set(ByVal value As Double)
        m_dHDifSet3 = value
    End Set
End Property


'熱板LED切替温度 (℃)
''Private m_dTempLED As Double
''Public Property TempLED As Double
''    Get
''        Return m_dTempLED
''    End Get
''    Set(ByVal value As Double)
''        m_dTempLED = value
''    End Set
''End Property
'熱板LED切替有効
''Private m_bTempLEDEn As Boolean
''Public Property TempLEDEn As Boolean
''    Get
''        Return m_bTempLEDEn
''    End Get
''    Set(ByVal value As boolean)
''        m_bTempLEDEn = value
''    End Set
''End Property
'測定終了ブザー有効
Private m_bEndOfBuzzerEn As Boolean
Public Property EndOfBuzzerEn As Boolean
    Get
        Return m_bEndOfBuzzerEn
    End Get
    Set(ByVal value As boolean)
        m_bEndOfBuzzerEn = value
    End Set
End Property
'測定終了ブザーパターン
Private m_EndOfBuzzerPattern As Int16
Public Property EndOfBuzzerPattern As Int16
    Get
        Return m_EndOfBuzzerPattern
    End Get
    Set(ByVal value As Int16)
        m_EndOfBuzzerPattern = value
    End Set
End Property

'熱板温度範囲ブザー有効
Private m_bBuzzerOkOfHeatEn As Boolean
Public Property BuzzerOkOfHeatEn As Boolean
    Get
        Return m_bBuzzerOkOfHeatEn
    End Get
    Set(ByVal value As boolean)
        m_bBuzzerOkOfHeatEn = value
    End Set
End Property
'カバーIL無効
Private m_bCoverILDis As Boolean
Public Property CoverILDis As Boolean
    Get
        Return m_bCoverILDis
    End Get
    Set(ByVal value As boolean)
        m_bCoverILDis = value
    End Set
End Property
'測定値領域読込許可
Private m_bReadRealAreaEn As Boolean
Public Property ReadRealAreaEn As Boolean
    Get
        Return m_bReadRealAreaEn
    End Get
    Set(ByVal value As boolean)
        m_bReadRealAreaEn = value
    End Set
End Property
'前IL無効
Private m_bAllILDis As Boolean
Public Property AllILDis As Boolean
    Get
        Return m_bAllILDis
    End Get
    Set(ByVal value As boolean)
        m_bAllILDis = value
    End Set
End Property
'スタンバイ時間(Min)
Private m_iStanbyTime As Int16 = 0
Public Property StanbyTime As Int16
    Get
        Return m_iStanbyTime
    End Get
    Set(ByVal value As Int16)
        m_iStanbyTime = value
    End Set
End Property
    
'アイドル中の熱板待機位置(mm)
Private m_dHeatBHomePos As Double
Public Property HeatBHomePos As Double
    Get
        Return m_dHeatBHomePos
    End Get
    Set(ByVal value As Double)
        m_dHeatBHomePos = value
    End Set
End Property
        
    
'応力検知 Max/Min値 取得時間（x10mS）
Private m_iStresDetAcqTime As Uint32 = 20
Public Property StresDetAcqTime As Uint32
    Get
        Return m_iStresDetAcqTime
    End Get
    Set(ByVal value As Uint32)
        m_iStresDetAcqTime = value
    End Set
End Property
'応力検知 A/D値 閾値（x100LSB）
Private m_iStresDetADThreshold As Uint32 = 5000
Public Property StresDetADThreshold As Uint32
    Get
        Return m_iStresDetADThreshold
    End Get
    Set(ByVal value As Uint32)
        m_iStresDetADThreshold = value
    End Set
End Property
'応力検知 継続時間（x10ms）
Private m_iStresDetDuraTime As Uint32 = 3
Public Property StresDetDuraTime As Uint32
    Get
        Return m_iStresDetDuraTime
    End Get
    Set(ByVal value As Uint32)
        m_iStresDetDuraTime = value
    End Set
End Property
        

'重量換算パラメータX 未使用
'Private m_dWeightCoefX As Double = Double.NaN
'Public Property WeightCoefX As Double
'    Get
'        Return m_dWeightCoefX
'    End Get
'    Set(ByVal value As Double)
'        m_dWeightCoefX = value
'    End Set
'End Property
    
'重量換算パラメータY 未使用
'Private m_dWeightCoefY As Double = Double.NaN
'Public Property WeightCoefY As Double
'    Get
'        Return m_dWeightCoefY
'    End Get
'    Set(ByVal value As Double)
'        m_dWeightCoefY = value
'    End Set
'End Property

'重量換算パラメータ AD値->uN
Private m_dWeightCoef As Double = Double.NaN
Public Property WeightCoef As Double
    Get
        Return m_dWeightCoef
    End Get
    Set(ByVal value As Double)
        m_dWeightCoef = value
    End Set
End Property
'重量換算パラメータ AD値->g
Private m_dWeightCoefZ As Double = Double.NaN
Public Property WeightCoefZ As Double
    Get
        Return m_dWeightCoefZ
    End Get
    Set(ByVal value As Double)
        m_dWeightCoefZ = value
    End Set
End Property

''' <summary>
''' 測定データに計算結果も付加するか 0:付加しない 1:付加する
''' </summary>
Private m_iCalcResult As Int32 = 1
Public Property CalcResultEnable() As Int32
	Get
		CalcResultEnable = m_iCalcResult
	End Get
	Set(value As Int32)
		m_iCalcResult = value
	End Set
End Property

Public m_MLinerInfo(2) As frmTempLinearTool.LinearType
Public Property MLinerInfo() As frmTempLinearTool.LinearType()
	Get
		MLinerInfo = m_MLinerInfo
	End Get
	Set(value As frmTempLinearTool.LinearType())
		m_MLinerInfo = value
	End Set
End Property

''' <summary>
''' 7セグLEDの輝度 0-15
''' </summary>
Private m_iLED7SegBrightness As Int32 = 7
Public Property LED7SegBrightness() As Int32
	Get
		LED7SegBrightness = m_iLED7SegBrightness
	End Get
	Set(value As Int32)
		m_iLED7SegBrightness = value
	End Set
End Property

''' <summary>
''' ゼロ調モード 0:X連続 1:Z連続 2:Z一回 3:なし
''' </summary>
Private m_iZeroAdjMode As Int32 = 1
Public Property ZeroAdjMode() As Int32
	Get
		ZeroAdjMode = m_iZeroAdjMode
	End Get
	Set(value As Int32)
		m_iZeroAdjMode = value
	End Set
End Property
''' <summary>
''' 投入待ち時間[秒]
''' </summary>
Private m_iSampleInputWait As Int32 = 0
Public Property SampleInputWait() As Int32
	Get
		SampleInputWait = m_iSampleInputWait
	End Get
	Set(value As Int32)
		m_iSampleInputWait = value
	End Set
End Property
''' <summary>
''' 投入GAP[mm]
''' </summary>
Private m_dInputGap As Double = 0.0
Public Property InputGap() As Double
	Get
		InputGap = m_dInputGap
	End Get
	Set(value As Double)
		m_dInputGap = value
	End Set
End Property

''' <summary>
''' 熱板温度許容範囲[℃]
''' </summary>
Private m_dHPTempRange As Double = 0
Public Property HPTempRange() As Double
	Get
		HPTempRange = m_dHPTempRange
	End Get
	Set(value As Double)
		m_dHPTempRange = value
	End Set
End Property

''' <summary>
''' プローブ温度許容範囲[℃]
''' </summary>
Private m_dPBTempRange As Double = 0
Public Property PBTempRange() As Double
	Get
		PBTempRange = m_dPBTempRange
	End Get
	Set(value As Double)
		m_dPBTempRange = value
	End Set
End Property

''' <summary>
''' 起歪体温度許容範囲[℃]
''' </summary>
Private m_dSGTempRange As Double = 0
Public Property SGTempRange() As Double
	Get
		SGTempRange = m_dSGTempRange
	End Get
	Set(value As Double)
		m_dSGTempRange = value
	End Set
End Property

''' <summary>
''' 起歪体応力安定度[g/10s]
''' </summary>
Private m_dSGStressStab As Double = 0
Public Property SGStressStab() As Double
	Get
		SGStressStab = m_dSGStressStab
	End Get
	Set(value As Double)
		m_dSGStressStab = value
	End Set
End Property

''' <summary>
''' 着圧制御待ち時間[mS]
''' </summary>
Private m_iPressContDelayTime As Int32 = 0
Public Property PressContDelayTime() As Int32
	Get
		PressContDelayTime = m_iPressContDelayTime
	End Get
	Set(value As Int32)
		m_iPressContDelayTime = value
	End Set
End Property

'測定メイングラフの固定時の軸スパン設定
Private m_dMainGraphXMin() As Double = {0.0,	100.0}
Private m_dMainGraphXMax() As Double = {100.0,	200.0}
Private m_dMainGraphYMin() As Double = { _
	-2000000, _
	-2000000, _
	0, _
	0, _
	0, _
	50, _
	0, _
	50, _
	0, _
	0, _
	0, _
	0, _
	0, _
	0, _
	0, _
	0, _
	0, _
	0 _
	}
Private m_dMainGraphYMax() As Double = { _
	+2000000, _
	+2000000, _
	5000, _
	5000, _
	6000000, _
	200, _
	100, _
	200, _
	100, _
	30000, _
	90, _
	1, _
	200000, _
	5, _
	200000, _
	20000, _
	5, _
	100 _
	}
'ENUM定義 (modChart.vb)
''Public eNum iGrpItmN
''	rbXStress		= 0
''	rbYStress		
''	rbZStress
''	rbCorrect
''	rbHPTemp		= 4
''	rbHPMV
''	rbPBTemp
''	rbPBMV
''	rbViscosity
''	rbPosPhase		= 9
''	rbSampleStrain
''	rbShearMod
''	rbShearVelo
''	rbStorageMod
''	rbElasticMod
''	rbLossTangent	= 15
''	rbCorrectP
''End eNum
Public Property MainGraphXMin(ByVal i As int16) As Double
	Get
		MainGraphXMin = m_dMainGraphXMin(i)
	End Get
	Set(value As Double)
		m_dMainGraphXMin(i) = value
	End Set
End Property
Public Property MainGraphXMax(ByVal i As int16) As Double
	Get
		MainGraphXMax = m_dMainGraphXMax(i)
	End Get
	Set(value As Double)
		m_dMainGraphXMax(i) = value
	End Set
End Property
Public Property MainGraphYMin(ByVal i As int16) As Double
	Get
		MainGraphYMin = m_dMainGraphYMin(i)
	End Get
	Set(value As Double)
		m_dMainGraphYMin(i) = value
	End Set
End Property
Public Property MainGraphYMax(ByVal i As int16) As Double
	Get
		MainGraphYMax = m_dMainGraphYMax(i)
	End Get
	Set(value As Double)
		m_dMainGraphYMax(i) = value
	End Set
End Property
'測定メイングラフの固定時の目盛り分割数
Private m_dMainGraphXDiv() As Double = {10,10}
Private m_dMainGraphYDiv() As Double = {10,10,10,10,10,10,10,10,10,10,6,10,10,10,10,10,10,10}
Public Property MainGraphXDiv(ByVal i As int16) As Double
	Get
		MainGraphXDiv = m_dMainGraphXDiv(i)
	End Get
	Set(value As Double)
		m_dMainGraphXDiv(i) = value
	End Set
End Property
Public Property MainGraphYDiv(ByVal i As int16) As Double
	Get
		MainGraphYDiv = m_dMainGraphYDiv(i)
	End Get
	Set(value As Double)
		m_dMainGraphYDiv(i) = value
	End Set
End Property
'粘度計算時の測定GAP補正値 dig/um
Private m_iGapCorr			As int32
Public Property GapCorr() As Int32
	Get
		GapCorr = m_iGapCorr
	End Get
	Set(value As int32)
		m_iGapCorr = value
	End Set
End Property
    
'Z軸 上限リミット位置設定[um]
Private m_iZUpperLimitPos	As Int32 = 50000
Public Property ZUpperLimitPos() As Int32
	Get
		ZUpperLimitPos = m_iZUpperLimitPos
	End Get
	Set(value As int32)
		m_iZUpperLimitPos = value
	End Set
End Property

'管理者権限モード移行パスワード
Private m_sAdminPass	As String  = "MadokaSizuka"
Public Property AdminPass() As String
	Get
		AdminPass = m_sAdminPass
	End Get
	Set(Val As String)
		m_sAdminPass = Val
	End Set
End Property
'管理者権限モード : True
Private m_bAdminEna		As Boolean = False
Public Property AdminEnable() As Boolean
	Get
		AdminEnable = m_bAdminEna
	End Get
	Set(Val As Boolean)
		m_bAdminEna = Val
	End Set
End Property
'管理者権限モード使用有無 : 1
Private m_iAdminUse		As int16 = 1
Public Property AdminUse() As Int16
	Get
		AdminUse = m_iAdminUse
	End Get
	Set(Val As int16)
		m_iAdminUse = Val
	End Set
End Property
'ロック解除ボタン確認２重化
Private m_iDoublePBUnlockConfirm As Int16
Public Property DoublePBUnlockConfirm() As Boolean
	Get
		DoublePBUnlockConfirm = IF(m_iDoublePBUnlockConfirm=1, True, False)
	End Get
	Set(Val As Boolean)
		m_iDoublePBUnlockConfirm = IF(Val, 1, 0)
	End Set
End Property

'測定中のパトライト緑点滅
Private m_iGreenFlashInMeas As Int16
Public Property GreenFlashInMeas() As Boolean
	Get
		GreenFlashInMeas = IF(m_iGreenFlashInMeas=1, True, False)
	End Get
	Set(Val As Boolean)
		m_iGreenFlashInMeas = IF(Val, 1, 0)
	End Set
End Property

'長時間測定モード設定
Private m_iLongTermMode			As Int16 = 0				'長時間測定制御 有効/無効
Public Property LTModeEnable() As Boolean
	Get
		LTModeEnable = IF(m_iLongTermMode=1, True, False)
	End Get
	Set(Val As Boolean)
		m_iLongTermMode = IF(Val, 1, 0)
	End Set
End Property
Private m_iMeasureInterval		As Int32 = 60				'長時間測定サンプリング間隔時間[分]
Public Property LTMeasureInterval() As Int32
	Get
		LTMeasureInterval = m_iMeasureInterval
	End Get
	Set(value As int32)
		m_iMeasureInterval = value
	End Set
End Property
Private m_iAdoptRate			As Int16 = 2				'長時間測定平均化サンプル採用周期数[回]
Public Property LTAdoptRate() As Int16
	Get
		LTAdoptRate = m_iAdoptRate
	End Get
	Set(value As Int16)
		m_iAdoptRate = value
	End Set
End Property

'QRコード 運用設定
Private m_iQRmode				As Int16 = 0				'QR使用  有効/無効
Public Property QRModeEnable() As Boolean
	Get
		QRModeEnable = IF(m_iQRmode=1, True, False)
	End Get
	Set(Val As Boolean)
		m_iQRmode = IF(Val, 1, 0)
	End Set
End Property
Private m_sQRECC				As String = "M"				'QRコード 誤り訂正レベル
Public Property QREcc() As String
	Get
		QREcc = m_sQRECC
	End Get
	Set(Val As String)
		m_sQRECC = Val
	End Set
End Property
Private m_iQRLogoCells			As Int16 = 9				'QRコード ロゴに占めるセル数
Public Property QRLogoCells() As Int16
	Get
		QRLogoCells = m_iQRLogoCells
	End Get
	Set(value As Int16)
		m_iQRLogoCells = value
	End Set
End Property

Private m_iQRPixelInCel			As Int16 = 9				'QRコード 1セル当たりのピクセル数
Public Property QRPixelInCel() As Int16
	Get
		QRPixelInCel = m_iQRPixelInCel
	End Get
	Set(value As Int16)
		m_iQRPixelInCel = value
	End Set
End Property


'サンプルSPECメニュー選択リスト蓄積配列
Public p_sSampleAcum()		As String = {"Sample name"}
Public p_sLotAcum()			As String = {"Lot"}
Public p_sMeasurerAcum()	As String = {"Measurer"}

'データベース自動登録機能有効 : 1
Private m_iDBAutoReg		As int16 = 1
Public Property DBAutoReg() As Int16
	Get
		DBAutoReg = m_iDBAutoReg
	End Get
	Set(Val As int16)
		m_iDBAutoReg = Val
	End Set
End Property
'SPC機能有効 : 1
Private m_iSPC		As int16 = 1
Public Property SPCEn() As Int16
	Get
		SPCEn = m_iSPC
	End Get
	Set(Val As int16)
		m_iSPC = Val
	End Set
End Property
'SPC 計算サンプル数
Private m_iSPCCalcSample	As int16 = 30
Public Property SPCCalcSample() As Int16
	Get
		SPCCalcSample = m_iSPCCalcSample
	End Get
	Set(Val As int16)
		m_iSPCCalcSample = Val
	End Set
End Property
'コンストラスタ
Public Sub New()
End Sub

''' <summary> データ削除と移動の許可。負の値で禁止 </summary>
Public iCanRemoveData As Integer = -1
''' <summary> ツール上で適用する校正係数の変更を許可。負の値で禁止 </summary>
Public iCanModifyCF As Integer = -1
''' <summary> ゲル時間計算条件の変更を許可。負の値で禁止 </summary>
Public iCanModifyRecalc As Integer = -1
''' <summary> 通信に使用するSPECの変更を許可。負の値で禁止 </summary>
Public iCanModifySpec As Integer = -1
''' <summary> 項目追加時のY1デフォルト色のパターン。 </summary>
Public iColorY1 As Integer = 0

''' <summary> リアルタイム測定時のX軸設定。0:自動, 1:手動 </summary>
Public iRTXAxis		As Integer = 1
''' <summary> リアルタイム測定時のY軸設定。0:自動, 1:手動 </summary>
Public iRTYAxis		As Integer = 1
''' <summary> リアルタイム測定時のX軸スクロール幅(秒) </summary>
Public iScrlWidth	As Int64 = 100

''' <summary> マーカスタイル </summary>
Public iMarkerStyle		As Int32 = MarkerStyle.None
''' <summary> マーカサイズ </summary>
Public iMarkerSize		As Int32 = 3

''' <summary> 測定チャートタイプX軸 0:時間 1:温度 </summary>
Public iChartType	As Int16 = 0



''' <summary> ゼロ調条件 熱板開始位置[mm] </summary>
Public dZeroStaPosHP As Double = 0.0

''' <summary> ゼロ調終了判定 安定度の変動範囲[um] </summary>
Public iStaFlucRange As Int64 = 0

''' <summary> ゼロ調終了判定 安定度の監視時間[秒] </summary>
Public iStaMonTime As Int32 = 0

''' <summary> センサ中央出し 熱板開始位置[mm] </summary>
Public dCenterStaPosHP As Double = 0.0

''' <summary> センサ中央出し 結果位置X[um] </summary>
Public iCenterResultsX As Int32 = 0

''' <summary> センサ中央出し 結果位置Y[um] </summary>
Public iCenterResultsY As Int32 = 0

''' <summary> フィルタ有効無効 </summary>
Public iFilterEnable    As Int16 = 0
''' <summary> フィルタ 種 FIR IIR </summary>
Public sFilterKind      As String
''' <summary> フィルタ 窓関数種 FIR </summary>
Public sFilterWinKind      As String

''' <summary> フィルタ FIR カットオフ周波数 </summary>
Public dFIRCutOffFreq      As Double
''' <summary> フィルタ FIR フィルタ長 </summary>
Public iFilterLen          As Int32

''' <summary> フィルタ IIR カットオフ周波数 </summary>
Public dIIRCutOffFreq      As Double
''' <summary> フィルタ IIR フィルタ次数 </summary>
Public iIIRDeg              As Int16
''' <summary> フィルタ係数ファイル名 </summary>
Public sFilterFileName  As String


'ADD START M_UNO 20151006 Ver.1.0.3.8 ツールバー初期表示
Public iDispToolVer As Integer = 0
'ADD START M_UNO 20160809  Ver.2.0.1.2  時間表示形式の設定
Public iTimeDspFormat As Integer = eTIME_DISP.iSECONDS
'ADD START IKEDA Ver3.2.0.0 20180411 ツールバージョンアップタイミングの検知
Public p_sLastToolVer As String = ""            '前回終了時のツールバージョン

Public p_ZCalibTempPro As Int16 = -1			'温度プロファイルZ位置キャリブレーション動作モード -1:補正値計算 その他:補正値作成


    

''' <summary> FTP通信時のユーザ名。 </summary>
Friend strFTPUser() As String = New String(10) {}
''' <summary> FTP通信時のパスワード。 </summary>
Friend strFTPPass() As String = New String(10) {}

''' <summary>
''' デバイス設定状態をINIファイルに保存する。
''' </summary>
Public Sub SaveAsDeviceListINI()
Try
    IO.Directory.SetCurrentDirectory(My.Application.Info.DirectoryPath.ToString())
	Dim strINIPath As String = Path.Combine(My.Application.Info.DirectoryPath.ToString(), DevListFile)
    ' ファイルが存在しない場合、Unicodeのファイルを新規作成
    Dim fileExists As Boolean
    fileExists = My.Computer.FileSystem.FileExists(strINIPath)
    If Not fileExists Then
        My.Computer.FileSystem.WriteAllText(strINIPath, String.Empty, False, System.Text.Encoding.Unicode)
    End If
    ' 設定を保存
    WritePrivateProfileString("Tool",			"Language",				CStr(Me.iLang),					strINIPath)
	'Lock状態保存
    WritePrivateProfileString("Tool",			"Lock",				If(Me.m_bAdminEna, "1", "0"),		strINIPath)
	'AdminPassword
    WritePrivateProfileString("Tool",			"Password",			Me.m_sAdminPass,					strINIPath)
	'Adminモード使用有無
    WritePrivateProfileString("Tool",			"AdminUse",			Me.m_iAdminUse.ToString,			strINIPath)

    WritePrivateProfileString("Remote",			"MultiAct",				CStr(Me.iMultiActivate),		strINIPath)
    WritePrivateProfileString("Remote",			"SelectedEquipment",	Me.iSelMDK,						strINIPath)
    For i As Integer = 0 To 9
        WritePrivateProfileString("Remote",		"IPAddress" + CStr(i),		Me.strIP(i),				strINIPath)
        WritePrivateProfileString("Remote",		"EquipName" + CStr(i),		Me.strEquipName(i),			strINIPath)
        WritePrivateProfileString("Remote",		"XYSensorSerial" + CStr(i),	Me.sXYSensorSerial(i),		strINIPath)
        WritePrivateProfileString("Remote",		"ZSensorSerial" + CStr(i),	Me.sZSensorSerial(i),		strINIPath)
    Next i

Catch ex As Exception
    Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
    Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub
''' <summary>
''' 装置ID毎の設定状態をINIファイルに保存する。
''' 注意：１セクションに属する行はまとめて記述すること->キーワードが重複してしまう
''' </summary>
Public Sub SaveAsDeviceSettingINI(ByVal sDeviceID As String)
Try
	If sDeviceID=""
		Exit Sub
	End If
	If sDeviceID.Substring(0,3)<>"SZK"
		Exit Sub
	End If

	Dim iRet	As int32
	Dim s		As String
	'Zセンサ安定判定しきい値の初期値チェック
	iRet = CheckSGStabInitialVal()
	Select Case iRet
	Case 1, -1
	'初期値になっている
		Call WriteLog("Zセンサ安定判定しきい値が初期値", eLOG_LEVEL.iINFO)
		s = frmMain.p_sMes(efrmMainSTR.iSGStabIni)
		MsgBox(s & vbCrLf & frmMain.p_sMes(efrmMainSTR.iMakerHelp), MsgBoxStyle.Critical + MsgBoxStyle.OkOnly,  System.Windows.Forms.Application.ProductName)
	Case Else
	End Select
	'Z荷重とたわみの比例定数の初期値チェック
	iRet = CheckGapCorrInitialVal()
	Select Case iRet
	Case 1, -1
	'初期値になっている
		Call WriteLog("Z荷重とたわみの比例定数が初期値", eLOG_LEVEL.iINFO)
		s = frmMain.p_sMes(efrmMainSTR.iGapCorrIni)
		MsgBox(s & vbCrLf & frmMain.p_sMes(efrmMainSTR.iMakerHelp), MsgBoxStyle.Critical + MsgBoxStyle.OkOnly,  System.Windows.Forms.Application.ProductName)
	Case Else
	End Select

	'装置ID毎のパスを設定
    Dim strINIPath As String = Path.Combine(SIZUKA_PATH, sDeviceID, DevSettingFile)
    ' ファイルが存在しない場合、Unicodeのファイルを新規作成
    Dim fileExists As Boolean
    fileExists = My.Computer.FileSystem.FileExists(strINIPath)
    If Not fileExists
        My.Computer.FileSystem.WriteAllText(strINIPath, String.Empty, False, System.Text.Encoding.Unicode)
    End If
    ' 設定を保存
    WritePrivateProfileString("Tool",			"StandbyTime",			Me.StanbyTime.ToString(),		strINIPath)
    WritePrivateProfileString("Tool",			"Port",					Me.strPort, strINIPath)
    WritePrivateProfileString("Tool",			"RecentData",				CStr(Me.iRecent),			strINIPath)
    WritePrivateProfileString("Tool",			"CanRemoveData",			CStr(Me.iCanRemoveData),	strINIPath)
    WritePrivateProfileString("Tool",			"CanModifyCF",				CStr(Me.iCanModifyCF),		strINIPath)
    WritePrivateProfileString("Tool",			"CanModifyRecalc",			CStr(Me.iCanModifyRecalc),	strINIPath)
    WritePrivateProfileString("Tool",			"CanModifySpec",			CStr(Me.iCanModifySpec),	strINIPath)
    WritePrivateProfileString("Tool",			"TimeDispFormat",			CStr(Me.iTimeDspFormat),	strINIPath)
    'ツールバー表示設定
    WritePrivateProfileString("Tool",			"DispToolVer",		CStr(Me.iDispToolVer),				strINIPath)
    '印刷設定　測定場所
    WritePrivateProfileString("Tool",			"PrintMesPlace",	Me.strPrntMesPlace,					strINIPath)
    '印刷設定　測定者
    WritePrivateProfileString("Tool",			"PrintMesPerson",	Me.strPrntMesPerson,				strINIPath)
    '印刷設定　コメント
    WritePrivateProfileString("Tool",			"PrintComment",		Me.strPrntComment,					strINIPath)
	'測定データへ計算結果も記録
    WritePrivateProfileString("Tool",			"IncludeCalc",		CStr(Me.m_iCalcResult),				strINIPath)
	'７セグLED輝度
    WritePrivateProfileString("Tool",			"7SegBrightness",	CStr(Me.m_iLED7SegBrightness),		strINIPath)
    'ADD START IKEDA Ver3.2.0.0 20180411 ツールバージョンアップタイミングの検知
    WritePrivateProfileString("Tool",			"LastToolVersion",	My.Application.Info.Version.ToString,	strINIPath)
    'ADD START M_UNO 20170124 Ver.--- 列フィルタ機能
    WritePrivateProfileString("Tool",			"DefDspMethod",				strDefDspMth,				strINIPath)
    WritePrivateProfileString("Tool",			"FixSpecLotManualInput",	iFixSpecLotManu,			strINIPath)

#If 0
    WritePrivateProfileString("Remote",			"SelectedEquipment",	Me.iSelMDK,						strINIPath)
    For i As Integer = 0 To 9
        WritePrivateProfileString("Remote",		"IPAddress" + CStr(i),		Me.strIP(i),				strINIPath)
        WritePrivateProfileString("Remote",		"EquipName" + CStr(i),		Me.strEquipName(i),			strINIPath)
        WritePrivateProfileString("Remote",		"XYSensorSerial" + CStr(i),	Me.sXYSensorSerial(i),		strINIPath)
        WritePrivateProfileString("Remote",		"ZSensorSerial" + CStr(i),	Me.sZSensorSerial(i),		strINIPath)
    Next i
#end if

    WritePrivateProfileString("ZeroAdj",		"ZeroAdjStartPosition", CStr(Me.dZeroStaPosHP),			strINIPath)
    WritePrivateProfileString("ZeroAdj",		"ZeroAdjStaMonTime",	CStr(Me.iStaMonTime),			strINIPath)
    WritePrivateProfileString("ZeroAdj",		"ZeroAdjStaFlucRange",	CStr(Me.iStaFlucRange),			strINIPath)
    WritePrivateProfileString("ZeroAdj",		"ZeroAdjMode",			CStr(Me.m_iZeroAdjMode),		strINIPath)

	'Z軸上限リミット位置
    WritePrivateProfileString("ZeroAdj",		"ZAxisLimitPosition",	CStr(Me.m_iZUpperLimitPos),		strINIPath)

    WritePrivateProfileString("Centering",		"CenterStartPosition",	CStr(Me.dCenterStaPosHP),		strINIPath)
    WritePrivateProfileString("Centering",		"CenterResultX",		CStr(Me.iCenterResultsX),		strINIPath)
    WritePrivateProfileString("Centering",		"CenterResultY",		CStr(Me.iCenterResultsY),		strINIPath)


    WritePrivateProfileString("Graph",			"GridLines",			CStr(Me.penGrids.DashStyle),	strINIPath)
    WritePrivateProfileString("Graph",			"Details(R)",			CStr(Me.iDetailsR),				strINIPath)
    WritePrivateProfileString("Graph",			"Details(D)",			CStr(Me.iDetailsD),				strINIPath)
    WritePrivateProfileString("Graph",			"RTXAxis",				CStr(Me.iRTXAxis),				strINIPath)
    WritePrivateProfileString("Graph",			"RTYAxis",				CStr(Me.iRTYAxis),				strINIPath)
    WritePrivateProfileString("Graph",			"ColorY1",				CStr(Me.iColorY1),				strINIPath)
    WritePrivateProfileString("Graph",			"ChartType",			CStr(Me.iChartType),			strINIPath)
    WritePrivateProfileString("Graph",			"ScrollWidth",			CStr(Me.iScrlWidth),			strINIPath)
    WritePrivateProfileString("Graph",			"MarkerStyle",			CStr(Me.iMarkerStyle),			strINIPath)
    WritePrivateProfileString("Graph",			"MarkerSize",			CStr(Me.iMarkerSize),			strINIPath)

	'グラフSPAN設定
	'X軸
    WritePrivateProfileString("Graph",			"X Time-Min",			CStr(m_dMainGraphXMin(0)),			strINIPath)
    WritePrivateProfileString("Graph",			"X Time-Max",			CStr(m_dMainGraphXMax(0)),			strINIPath)
    WritePrivateProfileString("Graph",			"X Temp-Min",			CStr(m_dMainGraphXMin(1)),			strINIPath)
    WritePrivateProfileString("Graph",			"X Temp-Max",			CStr(m_dMainGraphXMax(1)),			strINIPath)
    WritePrivateProfileString("Graph",			"X Time-Div",			CStr(m_dMainGraphXDiv(0)),			strINIPath)
    WritePrivateProfileString("Graph",			"X Temp-Div",			CStr(m_dMainGraphXDiv(1)),			strINIPath)
	'Y軸
	For i=0 to m_dMainGraphYMin.GetUpperBound(0)
		WritePrivateProfileString("Graph",			"Y " & sGraphSelectionArry(i).Substring(2) & "-Min",			CStr(m_dMainGraphYMin(i)),			strINIPath)
		WritePrivateProfileString("Graph",			"Y " & sGraphSelectionArry(i).Substring(2) & "-Max",			CStr(m_dMainGraphYMax(i)),			strINIPath)
		WritePrivateProfileString("Graph",			"Y " & sGraphSelectionArry(i).Substring(2) & "-Div",			CStr(m_dMainGraphYDiv(i)),			strINIPath)
	Next



    'ADD START M_UNO 20150831 Ver.1.0.3.7 Y1デフォルト、背景色等カラーを設定
    For i = 0 To clrY1DefaultVal.Length - 1
        WritePrivateProfileString("Graph",		"Y1Color_" + CStr(i), _
                                    CStr(Drawing.ColorTranslator.ToOle(Me.clrY1Default(i))), strINIPath)
    Next
    WritePrivateProfileString("Graph",			"GraphBackColor", CStr(Drawing.ColorTranslator.ToOle(Me.clrGraphBack)), strINIPath)
    WritePrivateProfileString("Graph",			"GraphGridColor", CStr(Drawing.ColorTranslator.ToOle(Me.penGridsValue.Color)), strINIPath)
'            WritePrivateProfileString("Graph",			"Y2Opposite", bY2OppositeVal, strINIPath)

    WritePrivateProfileString("Calculation",	"FilterEnable",				Cstr(Me.iFilterEnable),		strINIPath)
    WritePrivateProfileString("Calculation",	"FilterKind",				Me.sFilterKind,				strINIPath)
    WritePrivateProfileString("Calculation",	"FilterFIRLength",			Me.iFilterLen,				strINIPath)
    WritePrivateProfileString("Calculation",	"FilterFIRCutOffFrequency", Me.dFIRCutOffFreq.ToString("F2"), strINIPath)
    WritePrivateProfileString("Calculation",	"FilterFIRWindowFunction",	Me.sFilterWinKind,			strINIPath)
    WritePrivateProfileString("Calculation",	"FilterIIRCutOffFrequency", Me.dIIRCutOffFreq,			strINIPath)
    WritePrivateProfileString("Calculation",	"FilterIIRDegree",			Me.iIIRDeg,					strINIPath)

    WritePrivateProfileString("Calculation",	"GapCorrection",			Me.m_iGapCorr,				strINIPath)
    WritePrivateProfileString("Calculation",	"DBAutoRegistration",		Me.m_iDBAutoReg,			strINIPath)
    WritePrivateProfileString("Calculation",	"SPC function",				Me.m_iSPC,					strINIPath)
    WritePrivateProfileString("Calculation",	"SPC CalcSample",			Me.m_iSPCCalcSample,		strINIPath)

    'Auto Print 有効/無効
    WritePrivateProfileString("AutoPrint",		"Arrival",			iAutoPrintEnable,					strINIPath)
    '保存タイプ(0:JPEG,1:PDF)
    WritePrivateProfileString("AutoPrint",		"Type",				iAutoPrintType,						strINIPath)
    '保存先フォルダ
    WritePrivateProfileString("AutoPrint",		"OutPutFolder",		strAutoPrintOutPutFolder,			strINIPath)
    '測定場所
    WritePrivateProfileString("AutoPrint",		"MesurePlace",		strAutoPrintMesurePlace,			strINIPath)
    '測定者
    WritePrivateProfileString("AutoPrint",		"Operator",			strAutoPrintOperator,				strINIPath)
    '測定場所リスト		
    WritePrivateProfileString("AutoPrint",		"MesurePlaceList",	Me.m_sAutoPrintMesurePlaceList,		strINIPath)
    '測定者リスト
    WritePrivateProfileString("AutoPrint",		"OperatorList",		Me.m_sAutoPrintOperatorList,		strINIPath)
    'コメント
    WritePrivateProfileString("AutoPrint",		"Comment",			strAutoPrintComment,				strINIPath)
    '******************************************
    'Auto Printのディスク容量の警告閾値[%]
    '******************************************
    WritePrivateProfileString("AutoPrint",		"Disk_Capacity_Item",		CStr(iAutoPrintDiskCapacity), strINIPath)

    '年間の開始日
    WritePrivateProfileString("Summary",		"YearlyStartDay",			Me.m_sSumYstDayValue,		strINIPath)
    '月間の開始日
    WritePrivateProfileString("Summary",		"MonthlyStartDay",			Me.m_sSumMstDayValue,		strINIPath)
    '期間選択の集計範囲
    WritePrivateProfileString("Summary",		"FreeRangeStartDate",		Me.m_sSumFreeStValue,		strINIPath)
    WritePrivateProfileString("Summary",		"FreeRangeEndDate",			Me.m_sSumFreeEdValue,		strINIPath)

    'メンテナンス積算時間超過チェック済み最終日
    WritePrivateProfileString("Maintenance",	"TodayChecked",				Me.m_sMntTodayChkDate,					strINIPath)
    '本体電源ON時間の超過判定閾値
    WritePrivateProfileString("Maintenance",	"Threshold_PowerOn",		CStr(Me.m_iMntChkIntegHourPowerOn),		strINIPath)
    'HPヒータON時間の超過判定閾値
    WritePrivateProfileString("Maintenance",	"Threshold_HeaterOn",		CStr(Me.m_iMntChkIntegHourHeaterOn),	strINIPath)
    'X動作時間の超過判定閾値
    WritePrivateProfileString("Maintenance",	"Threshold_XAct",			CStr(Me.m_iMntChkIntegHourXAct),		strINIPath)
    'Y動作時間の超過判定閾値
    WritePrivateProfileString("Maintenance",	"Threshold_YAct",			CStr(Me.m_iMntChkIntegHourYAct),		strINIPath)
    'Z動作時間の超過判定閾値
    WritePrivateProfileString("Maintenance",	"Threshold_ZAct",			CStr(Me.m_iMntChkIntegHourZAct),		strINIPath)
    'PBヒータON時間の超過判定閾値
    WritePrivateProfileString("Maintenance",	"Threshold_HeaterOn2",		CStr(Me.m_iMntChkIntegHourHeaterOn2),	strINIPath)
    'SGヒータON時間の超過判定閾値
    WritePrivateProfileString("Maintenance",	"Threshold_HeaterOn3",		CStr(Me.m_iMntChkIntegHourHeaterOn3),	strINIPath)

    'トルクセンサ稼動ON時間の超過判定閾値
    'WritePrivateProfileString("Maintenance", "Threshold_TrqSensOpe", CStr(Me.m_iMntChkIntegHourTrqSens), strINIPath)

    '本体電源ON経過年数の超過判定閾値
    WritePrivateProfileString("Maintenance",	"RstYear_PowerOn",			CStr(Me.m_iMntChkRstYearPowerOn),		strINIPath)
    'HPヒータON経過年数の超過判定閾値
    WritePrivateProfileString("Maintenance",	"RstYear_HeaterOn",			CStr(Me.m_iMntChkRstYearHeaterOn),		strINIPath)
    'X動作経過年数の超過判定閾値
    WritePrivateProfileString("Maintenance",	"RstYear_XAct",				CStr(Me.m_iMntChkRstYearXAct),			strINIPath)
    'Y動作経過年数の超過判定閾値
    WritePrivateProfileString("Maintenance",	"RstYear_YAct",				CStr(Me.m_iMntChkRstYearYAct),			strINIPath)
    'Z動作経過年数の超過判定閾値
    WritePrivateProfileString("Maintenance",	"RstYear_ZAct",				CStr(Me.m_iMntChkRstYearZAct),			strINIPath)
    'PBヒータON経過年数の超過判定閾値
    WritePrivateProfileString("Maintenance",	"RstYear_HeaterOn2",		CStr(Me.m_iMntChkRstYearHeaterOn2),		strINIPath)
    'SGヒータON経過年数の超過判定閾値
    WritePrivateProfileString("Maintenance",	"RstYear_HeaterOn3",		CStr(Me.m_iMntChkRstYearHeaterOn3),		strINIPath)

    'トルクセンサ稼動経過年数の超過判定閾値
    'WritePrivateProfileString("Maintenance", "RstYear_TrqSensOpe", CStr(Me.m_iMntChkRstYearTrqSens), strINIPath)
    '測定結果自動保存の有効/無効
    WritePrivateProfileString("AutoSaveMeasData",	"Arrival",				m_iAutoSaveMeasDataEnable,				strINIPath)
    '測定結果自動保存の保存先フォルダ
    WritePrivateProfileString("AutoSaveMeasData",	"OutPutFolder",			m_sAutoSaveMeasDataOutPutFolder,		strINIPath)

    '温度コントロール（温度パラメータはデータ損失リスクのため２重保存）
    WritePrivateProfileString("TemperatureControl", "P HP",			m_dP.ToString,			strINIPath)
    WritePrivateProfileString("TemperatureControl", "I HP",			m_dI.ToString,			strINIPath)
    WritePrivateProfileString("TemperatureControl", "D HP",			m_dD.ToString,			strINIPath)
    WritePrivateProfileString("TemperatureControl", "Dgain HP",		m_dDgain.ToString,		strINIPath)
    WritePrivateProfileString("TemperatureControl", "PVBias HP",	m_dPVBias.ToString,		strINIPath)
    WritePrivateProfileString("TemperatureControl", "PVGain HP",	m_dPVGain.ToString,		strINIPath)

    WritePrivateProfileString("TemperatureControl", "P Probe",		m_dP2.ToString,			strINIPath)
    WritePrivateProfileString("TemperatureControl", "I Probe",		m_dI2.ToString,			strINIPath)
    WritePrivateProfileString("TemperatureControl", "D Probe",		m_dD2.ToString,			strINIPath)
    WritePrivateProfileString("TemperatureControl", "Dgain Probe",	m_dDgain2.ToString,		strINIPath)
    WritePrivateProfileString("TemperatureControl", "PVBias Probe", m_dPVBias2.ToString,	strINIPath)
    WritePrivateProfileString("TemperatureControl", "PVGain Probe", m_dPVGain2.ToString,	strINIPath)

    WritePrivateProfileString("TemperatureControl", "P SG",			m_dP3.ToString,			strINIPath)
    WritePrivateProfileString("TemperatureControl", "I SG",			m_dI3.ToString,			strINIPath)
    WritePrivateProfileString("TemperatureControl", "D SG",			m_dD3.ToString,			strINIPath)
    WritePrivateProfileString("TemperatureControl", "Dgain SG",		m_dDgain3.ToString,		strINIPath)
    WritePrivateProfileString("TemperatureControl", "PVBias SG",	m_dPVBias3.ToString,	strINIPath)
    WritePrivateProfileString("TemperatureControl", "PVGain SG",	m_dPVGain3.ToString,	strINIPath)

    WritePrivateProfileString("TemperatureControl", "HeaterUpperLimit HP",		m_dHUpperLimit.ToString,	strINIPath)
    WritePrivateProfileString("TemperatureControl", "HeaterUpperLimit Probe",	m_dHUpperLimit2.ToString,	strINIPath)
    WritePrivateProfileString("TemperatureControl", "HeaterUpperLimit SG",		m_dHUpperLimit3.ToString,	strINIPath)
    WritePrivateProfileString("TemperatureControl", "HeaterLowerLimit SG",		m_dHLowerLimit3.ToString,	strINIPath)
    WritePrivateProfileString("TemperatureControl", "HeaterDifSetting SG",		m_dHDifSet3.ToString,		strINIPath)
    ''WritePrivateProfileString("TemperatureControl", "TempLED",					m_dTempLED.ToString,		strINIPath)
    ''WritePrivateProfileString("TemperatureControl", "TempLEDEnable",			m_bTempLEDEn.ToString,		strINIPath)

    'ブザーコントロール
    WritePrivateProfileString("BuzzerControl",		"End_Of_Buzzer",	m_bEndOfBuzzerEn.ToString,		strINIPath)
    WritePrivateProfileString("BuzzerControl",		"EndOfPattern",		m_EndOfBuzzerPattern.ToString, strINIPath)
    WritePrivateProfileString("BuzzerControl",		"Ok_Of_Heater",		m_bBuzzerOkOfHeatEn.ToString,	strINIPath)

    'モータコントロール
    WritePrivateProfileString("MotorControl",		"StressDetectAcquisitionTime",	m_iStresDetAcqTime.ToString,		strINIPath)
    WritePrivateProfileString("MotorControl",		"StressDetADThreshold",			m_iStresDetADThreshold.ToString,	strINIPath)
    WritePrivateProfileString("MotorControl",		"StressDetDuraTime",			m_iStresDetDuraTime.ToString,		strINIPath)

	'測定制御
    WritePrivateProfileString("MeasureControl",		"SampleInputWait",				m_iSampleInputWait.ToString,		strINIPath)
    WritePrivateProfileString("MeasureControl",		"InputGap",						m_dInputGap.ToString,				strINIPath)
    WritePrivateProfileString("MeasureControl",		"HPTemperatureRange",			m_dHPTempRange.ToString,			strINIPath)
    WritePrivateProfileString("MeasureControl",		"PBTemperatureRange",			m_dPBTempRange.ToString,			strINIPath)
    WritePrivateProfileString("MeasureControl",		"SGTemperatureRange",			m_dSGTempRange.ToString,			strINIPath)
    WritePrivateProfileString("MeasureControl",		"SGStressStability",			m_dSGStressStab.ToString,			strINIPath)
    WritePrivateProfileString("MeasureControl",		"PressControlDelayTime",		m_iPressContDelayTime.ToString,		strINIPath)
	WritePrivateProfileString("MeasureControl",		"DoubleProbeUnlockConfirm",		m_iDoublePBUnlockConfirm.ToString,	strINIPath)
	WritePrivateProfileString("MeasureControl",		"PTGreenFlashInMeasure",		m_iGreenFlashInMeas.ToString,		strINIPath)


	WritePrivateProfileString("MeasureControl",		"Long-termModeEnable",			m_iLongTermMode.ToString,			strINIPath)
	WritePrivateProfileString("MeasureControl",		"Long-termModeInterval",		m_iMeasureInterval.ToString,		strINIPath)
	WritePrivateProfileString("MeasureControl",		"Long-termAdoptRate",			m_iAdoptRate.ToString,				strINIPath)

	WritePrivateProfileString("MeasureControl",		"QRCode Enable",				m_iQRmode.ToString,					strINIPath)
	WritePrivateProfileString("MeasureControl",		"QRCode Error Correction",		m_sQRECC.ToString,					strINIPath)
	WritePrivateProfileString("MeasureControl",		"QRCode Cells in Logo",			m_iQRLogoCells.ToString,			strINIPath)
	WritePrivateProfileString("MeasureControl",		"QRCode Pixels in Cell",		m_iQRPixelInCel.ToString,			strINIPath)


    'その他
    WritePrivateProfileString("Other",				"CoverILDisable",			m_bCoverILDis.ToString,					strINIPath)
    WritePrivateProfileString("Other",				"AllILDisable",				m_bAllILDis.ToString,					strINIPath)
    WritePrivateProfileString("Other",				"HeatBoard_HomePosition",	m_dHeatBHomePos.ToString("F3"),			strINIPath)
    '測定値領域への読込許可
    WritePrivateProfileString("Other",				"ReadRealAreaEnable",		m_bReadRealAreaEn,						strINIPath)


Catch ex As Exception
    Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
    Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub

''' <summary>
''' 温調パラメータをINIファイルに保存する
''' </summary>
''' <param name="sDeviceID">装置名</param>
''' <returns>0:成功 1:初期値書込みスキップ -1:例外発生 -2:装置名が不正</returns>
Public Function SaveAsTempParaINI(ByVal sDeviceID As String) As int32
Dim s As String = Nothing
Try
	If sDeviceID=""
		Return -2
	End If
	If sDeviceID.Substring(0,3)<>"SZK"
		Return -2
	End If
	'温度パラメータ初期値チェック
	Dim iRet As int32
	iRet = CheckTempParaInitialVal()
	Select Case iRet
	Case 1
	'P,I,D,Dgainが初期値以外である
	    Call WriteLog("温度パラメータ(P,I,D,DGain)が変更されている", eLOG_LEVEL.iINFO)
		s = frmMain.p_sMes(efrmMainSTR.iTempParaIni1)
		MsgBox(s, MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, System.Windows.Forms.Application.ProductName)
		If MsgBox(frmMain.p_sMes(efrmMainSTR.iTempParaWrt), MsgBoxStyle.Exclamation + MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton2,  System.Windows.Forms.Application.ProductName) _
			= MsgBoxResult.No
		    Call WriteLog("温度パラメータファイルを初期値で書き換え" & ":NO", eLOG_LEVEL.iINFO)
			''Return 1
		Else
		    Call WriteLog("温度パラメータファイルを初期値で書き換え" & ":Yes", eLOG_LEVEL.iINFO)
			m_dP		= cdP
			m_dI		= cdI
			m_dD		= cdD
			m_dDgain	= cdDgain
			m_dP2		= cdP2
			m_dI2		= cdI2
			m_dD2		= cdD2
			m_dDgain2	= cdDgain2
			m_dP3		= cdP3
			m_dI3		= cdI3
			m_dD3		= cdD3
			m_dDgain3	= cdDgain3
		End If
	Case 2
	'PVBias,PVGainが初期値になっている
	    Call WriteLog("温度パラメータ(PVBias,PVGain)が初期値", eLOG_LEVEL.iINFO)
		s = frmMain.p_sMes(efrmMainSTR.iTempParaIni)
		MsgBox(s, MsgBoxStyle.Critical + MsgBoxStyle.OkOnly,  System.Windows.Forms.Application.ProductName)
	Case -1
	'例外発生
	Case Else
	End Select


	'装置ID毎のパスを設定
    Dim strINIPath As String = Path.Combine(SIZUKA_PATH, sDeviceID, "TempPara.ini")
    ' ファイルが存在しない場合、Unicodeのファイルを新規作成
    Dim fileExists As Boolean
    fileExists = My.Computer.FileSystem.FileExists(strINIPath)
    If Not fileExists Then
        My.Computer.FileSystem.WriteAllText(strINIPath, String.Empty, False, System.Text.Encoding.Unicode)
    End If
    ' 設定を保存
    '温度コントロール
    WritePrivateProfileString("Hot Plate",	"P",		m_dP.ToString, strINIPath)
    WritePrivateProfileString("Hot Plate",	"I",		m_dI.ToString, strINIPath)
    WritePrivateProfileString("Hot Plate",	"D",		m_dD.ToString, strINIPath)
    WritePrivateProfileString("Hot Plate",	"Dgain",	m_dDgain.ToString, strINIPath)
    WritePrivateProfileString("Hot Plate",	"PVBias",	m_dPVBias.ToString, strINIPath)
    WritePrivateProfileString("Hot Plate",	"PVGain",	m_dPVGain.ToString, strINIPath)

    WritePrivateProfileString("Probe",		"P",		m_dP2.ToString, strINIPath)
    WritePrivateProfileString("Probe",		"I",		m_dI2.ToString, strINIPath)
    WritePrivateProfileString("Probe",		"D",		m_dD2.ToString, strINIPath)
    WritePrivateProfileString("Probe",		"Dgain",	m_dDgain2.ToString, strINIPath)
    WritePrivateProfileString("Probe",		"PVBias",	m_dPVBias2.ToString, strINIPath)
    WritePrivateProfileString("Probe",		"PVGain",	m_dPVGain2.ToString, strINIPath)

    WritePrivateProfileString("Strain Guage","P",		m_dP3.ToString, strINIPath)
    WritePrivateProfileString("Strain Guage","I",		m_dI3.ToString, strINIPath)
    WritePrivateProfileString("Strain Guage","D",		m_dD3.ToString, strINIPath)
    WritePrivateProfileString("Strain Guage","Dgain",	m_dDgain3.ToString, strINIPath)
    WritePrivateProfileString("Strain Guage","PVBias",	m_dPVBias3.ToString, strINIPath)
    WritePrivateProfileString("Strain Guage","PVGain",	m_dPVGain3.ToString, strINIPath)

Catch ex As Exception
    Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
    Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Return -1
End Try
	Return 0
End Function
''' <summary>
''' 温度パラメータが全て初期値になっていないかチェックする
''' P,I,D,Dgainは初期値であること
''' </summary>
''' <returns>0:正常状態 1:P,I,D,DGAINが初期値ではない 2:PVBias,PVGainが初期値である </returns>
Public Function CheckTempParaInitialVal() As Int32
Dim iRet As Int32 = 0
Try
	If _
		(m_dPVBias	= cdPVBias	AndAlso _
		m_dPVGain	= cdPVGain)	OrElse _
		(m_dPVBias2	= cdPVBias2	AndAlso _
		m_dPVGain2	= cdPVGain2)
		'HP,PBのPVBias,PVGainが初期値である
		iRet = 2
	End If
	If _
		m_dP		= cdP		AndAlso _
		m_dI		= cdI		AndAlso _
		m_dD		= cdD		AndAlso _
		m_dDgain	= cdDgain	AndAlso _
		m_dP2		= cdP2		AndAlso _
		m_dI2		= cdI2		AndAlso _
		m_dD2		= cdD2		AndAlso _
		m_dDgain2	= cdDGain2	AndAlso _
		m_dP3		= cdP3		AndAlso _
		m_dI3		= cdI3		AndAlso _
		m_dD3		= cdD3		AndAlso _
		m_dDgain3	= cdDgain3
	Else
	'P,I,D,DGAINが初期値ではない
		iRet = 1
	End If
	Return iRet

Catch ex As Exception
    Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Return -1
End Try
End Function
''' <summary>
''' 起歪体応力安定度[g/10s]が 初期値になっていないかチェックする
''' </summary>
''' <returns>0:初期値以外 1:初期値である </returns>
Public Function CheckSGStabInitialVal() As Int32
Dim iRet As Int32 = 0
Try
	iRet = If(m_dSGStressStab = cStressStab, 1, 0)
	Return iRet
Catch ex As Exception
    Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Return -1
End Try
End Function
''' <summary>
''' Z荷重たわみ係数(測定GAP補正値)[dig/um]が 初期値になっていないかチェックする
''' </summary>
''' <returns>0:初期値以外 1:初期値である </returns>
Public Function CheckGapCorrInitialVal() As Int32
Dim iRet As Int32 = 0
Try
	iRet = If(m_iGapCorr = cGapCorr, 1, 0)
	Return iRet
Catch ex As Exception
    Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Return -1
End Try
End Function

#If 0
''' <summary> 特定の項目のみをApp.iniに保存する。 </summary>
Public Shared Sub WriteSingleItemToINI(ByVal strSection As String, ByVal strKey As String, ByVal strVal As String)
    Try
        ' App.iniの場所は絶対パスで指定する
        IO.Directory.SetCurrentDirectory(My.Application.Info.DirectoryPath.ToString())
        Dim strINIPath As String = My.Application.Info.DirectoryPath.ToString() + "/App.ini"
        ' ファイルが存在しない場合、Unicodeのファイルを新規作成
        Dim fileExists As Boolean
        fileExists = My.Computer.FileSystem.FileExists(strINIPath)
        If Not fileExists Then
            My.Computer.FileSystem.WriteAllText(strINIPath, String.Empty, False, System.Text.Encoding.Unicode)
        End If
        ' 設定を保存
        WritePrivateProfileString(strSection, strKey, strVal, strINIPath)

    Catch ex As Exception
        Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
        Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
    End Try
End Sub
#end if
''' <summary>
''' 設定をINIファイルから読み込む。
''' </summary>
Public Sub LoadDeviceListINI()
Try
    IO.Directory.SetCurrentDirectory(My.Application.Info.DirectoryPath.ToString())
    Dim strINIPath As String = Path.Combine(My.Application.Info.DirectoryPath.ToString(), DevListFile)
        ' 格納用バッファ
    Dim strSb As System.Text.StringBuilder = Nothing
    strSb = New System.Text.StringBuilder(1024)
    Me.iLang			= GetPrivateProfileInt(	"Tool",			"Language",				-1,		strINIPath)
	Dim iEna As Int16
	iEna = GetPrivateProfileInt("Tool",	"Lock",			0,		strINIPath)
    Me.m_bAdminEna		= If(iEna = 1, True, False)
    GetPrivateProfileString("Tool",		"Password",		"MadokaSizuka",	strSb, strSb.Capacity, strINIPath)
    Me.m_sAdminPass		= strSb.ToString()
	'Adminモード使用有無
    Me.m_iAdminUse		= GetPrivateProfileInt("Tool",			"AdminUse",				1,			strINIPath)



    Me.iMultiActivate	= GetPrivateProfileInt(	"Remote",		"MultiAct",				0,		strINIPath)
	Me.iSelMDK	= GetPrivateProfileInt(	"Remote",		"SelectedEquipment",	0,					strINIPath)
    GetPrivateProfileString(			"Remote",		"IPAddress0",			"127.0.0.1",		strSb, strSb.Capacity, strINIPath)
    Me.strIP(0) = strSb.ToString()
    GetPrivateProfileString(			"Remote",		"EquipName0",			"SZK999",			strSb, strSb.Capacity, strINIPath)
    Me.strEquipName(0) = strSb.ToString()
    GetPrivateProfileString(			"Remote",		"XYSensorSerial0",		"SZS-00-001",		strSb, strSb.Capacity, strINIPath)
    Me.sXYSensorSerial(0) = strSb.ToString()
    GetPrivateProfileString(			"Remote",		"ZSensorSerial0",		"SZS-90-001",		strSb, strSb.Capacity, strINIPath)
    Me.sZSensorSerial(0) = strSb.ToString()
    For i As Integer = 1 To 9
        GetPrivateProfileString("Remote", "IPAddress" + CStr(i), "", strSb, strSb.Capacity, strINIPath)
        Me.strIP(i) = strSb.ToString()
        GetPrivateProfileString("Remote", "EquipName" + CStr(i), "", strSb, strSb.Capacity, strINIPath)
        Me.strEquipName(i) = strSb.ToString()
        GetPrivateProfileString("Remote", "XYSensorSerial" + CStr(i), "", strSb, strSb.Capacity, strINIPath)
        Me.sXYSensorSerial(i) = strSb.ToString()
        GetPrivateProfileString("Remote", "ZSensorSerial" + CStr(i), "", strSb, strSb.Capacity, strINIPath)
        Me.sZSensorSerial(i) = strSb.ToString()
    Next i

Catch ex As Exception
    Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub
''' <summary>
''' 設定をINIファイルから読み込む。
''' </summary>
Public Sub LoadDeviceSettingINI(ByVal sDeviceID As String, Optional bMsg As Boolean = True)
    Try
		If sDeviceID.Substring(0,3)<>"SZK"
			Exit Sub
		End If
        Dim strINIPath As String = Path.Combine(SIZUKA_PATH, sDeviceID, DevSettingFile)
        ' 格納用バッファ
        Dim strSb As System.Text.StringBuilder = Nothing
        strSb = New System.Text.StringBuilder(1024)
        ' 設定を読込、読み込めない場合は初期値を代入
        Me.StanbyTime			= GetPrivateProfileInt(		"Tool",			"StandbyTime",			"0",		strINIPath)
        'ゼロ調正
        Me.dZeroStaPosHP		= GetPrivateProfilefloat(	"ZeroAdj",		"ZeroAdjStartPosition",	"30.500",	strINIPath)
        Me.iStaMonTime			= GetPrivateProfileInt(		"ZeroAdj",		"ZeroAdjStaMonTime",	"10",		strINIPath)
        Me.iStaFlucRange		= GetPrivateProfileInt(		"ZeroAdj",		"ZeroAdjStaFlucRange",	"5",		strINIPath)
        Me.m_iZeroAdjMode		= GetPrivateProfileInt(		"ZeroAdj",		"ZeroAdjMode",			"1",		strINIPath)
		'Z軸上限リミット位置
        Me.m_iZUpperLimitPos	= GetPrivateProfileInt(		"ZeroAdj",		"ZAxisLimitPosition",	"50000",	strINIPath)

        '中央出し
        '熱板位置
        Me.dCenterStaPosHP		= GetPrivateProfilefloat(	"Centering",	"CenterStartPosition",	"0",		strINIPath)
        '最新処理結果 X位置
        Me.iCenterResultsX		= GetPrivateProfileInt(		"Centering",	"CenterResultX",		"0",		strINIPath)
        '最新処理結果 Y位置
        Me.iCenterResultsY		= GetPrivateProfileInt(		"Centering",	"CenterResultY",		"0",		strINIPath)

        Me.penGrids.DashStyle	= GetPrivateProfileInt("Graph", "GridLines",	0,		strINIPath)
        Me.iDetailsR			= GetPrivateProfileInt("Graph", "Details(R)",	100,	strINIPath)
        Me.iDetailsD			= GetPrivateProfileInt("Graph", "Details(D)",	10,		strINIPath)
        Me.iRTXAxis				= GetPrivateProfileInt("Graph", "RTXAxis",		1,		strINIPath)
        Me.iRTYAxis				= GetPrivateProfileInt("Graph", "RTYAxis",		1,		strINIPath)
        Me.iColorY1				= GetPrivateProfileInt("Graph", "ColorY1",		1,		strINIPath)
        Me.iChartType			= GetPrivateProfileInt("Graph", "ChartType",	0,		strINIPath)
        Me.iScrlWidth			= GetPrivateProfileInt("Graph", "ScrollWidth",	100,	strINIPath)
        Me.iMarkerStyle			= GetPrivateProfileInt("Graph", "MarkerStyle",	0,		strINIPath)
        Me.iMarkerSize			= GetPrivateProfileInt("Graph", "MarKerSize",	0,		strINIPath)


		'グラフSPAN設定
		'X軸
        Me.m_dMainGraphXMin(0)	= GetPrivateProfilefloat(	"Graph",		"X Time-Min",			"0",		strINIPath)
        Me.m_dMainGraphXMax(0)	= GetPrivateProfilefloat(	"Graph",		"X Time-Max",			"100",		strINIPath)
        Me.m_dMainGraphXMin(1)	= GetPrivateProfilefloat(	"Graph",		"X Temp-Min",			"0",		strINIPath)
        Me.m_dMainGraphXMax(1)	= GetPrivateProfilefloat(	"Graph",		"X Temp-Max",			"300",		strINIPath)
        Me.m_dMainGraphXDiv(0)	= GetPrivateProfilefloat(	"Graph",		"X Time-Div",			"10",		strINIPath)
        Me.m_dMainGraphXDiv(1)	= GetPrivateProfilefloat(	"Graph",		"X Temp-Div",			"10",		strINIPath)
		'Y軸
		For i=0 to m_dMainGraphYMin.GetUpperBound(0)
			Me.m_dMainGraphYMin(i)	= GetPrivateProfilefloat(	"Graph", "Y " & sGraphSelectionArry(i).Substring(2) & "-Min",		m_dMainGraphYMin(i),	strINIPath)
			Me.m_dMainGraphYMax(i)	= GetPrivateProfilefloat(	"Graph", "Y " & sGraphSelectionArry(i).Substring(2) & "-Max",		m_dMainGraphYMax(i),	strINIPath)
			Me.m_dMainGraphYDiv(i)	= GetPrivateProfilefloat(	"Graph", "Y " & sGraphSelectionArry(i).Substring(2) & "-Div",		m_dMainGraphYDiv(i),	strINIPath)
		Next

		'計算関係
        'フィルタ有効無効
		Me.iFilterEnable		= GetPrivateProfileInt("Calculation", "FilterEnable",		0,		strINIPath)
        GetPrivateProfileString("Calculation",		"FilterKind",		"FIR",	strSb, strSb.Capacity, strINIPath)
        sFilterKind = strSb.ToString

		'FIRフィルタ条件
        Me.iFilterLen			= GetPrivateProfileInt(		"Calculation", "FilterFIRLength",			1001,	strINIPath)
        Me.dFirCutOffFreq		= GetPrivateProfilefloat(	"Calculation", "FilterFIRCutOffFrequency",	1,		strINIPath)
        GetPrivateProfileString(							"Calculation", "FilterFIRWindowFunction",	"Hamming", strSb, strSb.Capacity, strINIPath)
        sFilterWinKind = strSb.ToString
        Me.dIirCutOffFreq		= GetPrivateProfilefloat(	"Calculation", "FilterIIRCutOffFrequency",	1, strINIPath)
        Me.iIIRDeg				= GetPrivateProfileInt(		"Calculation", "FilterIIRDegree",			3, strINIPath)
        GetPrivateProfileString(							"Calculation", "FilterFile",				"TestFilter", strSb, strSb.Capacity, strINIPath)
        sFilterFileName = strSb.ToString

        Me.m_iGapCorr			= GetPrivateProfileInt(		"Calculation", "GapCorrection",				100,	strINIPath)
        Me.m_iDBAutoReg			= GetPrivateProfileInt(		"Calculation", "DBAutoRegistration",		1,		strINIPath)
        Me.m_iSPC				= GetPrivateProfileInt(		"Calculation", "SPC function",				0,		strINIPath)
        Me.m_iSPCCalcSample		= GetPrivateProfileInt(		"Calculation", "SPC CalcSample",			30,		strINIPath)

        Me.iRecent			= GetPrivateProfileInt("Tool", "RecentData",		9999, strINIPath)
        Me.iCanRemoveData	= GetPrivateProfileInt("Tool", "CanRemoveData ",	0, strINIPath)
        Me.iCanModifyCF		= GetPrivateProfileInt("Tool", "CanModifyCF ",		0, strINIPath)
        Me.iCanModifyRecalc = GetPrivateProfileInt("Tool", "CanModifyRecalc ",	0, strINIPath)
        Me.iCanModifySpec	= GetPrivateProfileInt("Tool", "CanModifySpec ",	0, strINIPath)
        Me.iTimeDspFormat	= GetPrivateProfileInt("Tool", "TimeDispFormat ",	eTIME_DISP.iSECONDS, strINIPath)
        GetPrivateProfileString(				   "Tool", "Port",				cPORT_NO, strSb, strSb.Capacity, strINIPath)
        Me.strPort = strSb.ToString()
        If Me.strPort = "0" Then
            Me.strPort = cPORT_NO
        End If
		'７セグLED輝度
		Me.m_iLED7SegBrightness = GetPrivateProfileInt("Tool", "7SegBrightness",		7, strINIPath)

        'グラフ
        For i = 0 To clrY1DefaultVal.Length - 1
            GetPrivateProfileString("Graph", "Y1Color_" + CStr(i), "", strSb, strSb.Capacity, strINIPath)
            '数値に変換できる場合、色を設定
            If IsNumeric(strSb.ToString()) = True Then
                Me.clrY1Default(i) = Drawing.ColorTranslator.FromOle(strSb.ToString())
            End If
        Next
        GetPrivateProfileString("Graph", "GraphBackColor", "", strSb, strSb.Capacity, strINIPath)
        If IsNumeric(strSb.ToString()) = True Then
            clrGraphBack = Drawing.ColorTranslator.FromOle(strSb.ToString())
        End If
        GetPrivateProfileString("Graph", "GraphGridColor", "", strSb, strSb.Capacity, strINIPath)
        If IsNumeric(strSb.ToString()) = True Then
            penGrids.Color = Drawing.ColorTranslator.FromOle(strSb.ToString())
        End If

        'ツールバー表示設定
        Me.iDispToolVer		= GetPrivateProfileInt("Tool", "DispToolVer", 0, strINIPath)
        '印刷設定　測定場所
        GetPrivateProfileString("Tool", "PrintMesPlace", "", strSb, strSb.Capacity, strINIPath)
        Me.strPrntMesPlace = strSb.ToString()
        '印刷設定　測定者
        GetPrivateProfileString("Tool", "PrintMesPerson", "", strSb, strSb.Capacity, strINIPath)
        Me.strPrntMesPerson = strSb.ToString()
        '印刷設定　コメント
        GetPrivateProfileString("Tool", "PrintComment", "", strSb, strSb.Capacity, strINIPath)
        Me.strPrntComment = strSb.ToString()
        Me.m_iCalcResult = GetPrivateProfileInt("Tool", "IncludeCalc", 0, strINIPath)
			
		'Auto Print有効/無効
        Me.iAutoPrintEnable = GetPrivateProfileInt("AutoPrint", "Arrival ", 0, strINIPath)
        '保存タイプ(0:JPEG,1:PDF)
        Me.iAutoPrintType = GetPrivateProfileInt("AutoPrint", "Type ", 0, strINIPath)
        '保存先フォルダ
        GetPrivateProfileString("AutoPrint", "OutPutFolder", "C:\SZKTool", strSb, strSb.Capacity, strINIPath)
        Me.strAutoPrintOutPutFolder = strSb.ToString()
        '測定場所
        GetPrivateProfileString("AutoPrint", "MesurePlace", "", strSb, strSb.Capacity, strINIPath)
        Me.strAutoPrintMesurePlace = strSb.ToString()
        '測定者
        GetPrivateProfileString("AutoPrint", "Operator", "", strSb, strSb.Capacity, strINIPath)
        Me.strAutoPrintOperator = strSb.ToString()
        'コメント
        GetPrivateProfileString("AutoPrint", "Comment", "", strSb, strSb.Capacity, strINIPath)
        Me.strAutoPrintComment = strSb.ToString()
        'Auto Printのディスク容量の警告閾値[%]
        GetPrivateProfileString("AutoPrint", "Disk_Capacity_Item", "10", strSb, strSb.Capacity, strINIPath)
        Me.iAutoPrintDiskCapacity = CInt(strSb.ToString())
        '測定場所リスト
        GetPrivateProfileString("AutoPrint", "MesurePlaceList", "", strSb, strSb.Capacity, strINIPath)
        Me.m_sAutoPrintMesurePlaceList = strSb.ToString()
        '測定者リスト
        GetPrivateProfileString("AutoPrint", "OperatorList", "", strSb, strSb.Capacity, strINIPath)
        Me.sAutoPrintOperatorList = strSb.ToString()
        GetPrivateProfileString("Tool", "DefDspMethod", "", strSb, strSb.Capacity, strINIPath)
        strDefDspMth = strSb.ToString()

        '年間の集計開始日
        GetPrivateProfileString("Summary", "YearlyStartDay", "", strSb, strSb.Capacity, strINIPath)
        Me.m_sSumYstDayValue = strSb.ToString()
        '月間の集計開始日
        GetPrivateProfileString("Summary", "MonthlyStartDay", "", strSb, strSb.Capacity, strINIPath)
        Me.m_sSumMstDayValue = strSb.ToString()
        '期間選択の集計開始日
        GetPrivateProfileString("Summary", "FreeRangeStartDate", "", strSb, strSb.Capacity, strINIPath)
        Me.m_sSumFreeStValue = strSb.ToString()
        '期間選択の集計終了日
        GetPrivateProfileString("Summary", "FreeRangeEndDate", "", strSb, strSb.Capacity, strINIPath)
        Me.m_sSumFreeEdValue = strSb.ToString()
        'メンテナンス積算時間超過チェック済み最終日
        GetPrivateProfileString("Maintenance", "TodayChecked", "", strSb, strSb.Capacity, strINIPath)
        Me.m_sMntTodayChkDate = strSb.ToString()

        '本体電源ON時間の超過判定閾値
        Me.m_iMntChkIntegHourPowerOn	= GetPrivateProfileInt("Maintenance", "Threshold_PowerOn ", 8000, strINIPath)
        'HPヒータON時間の超過判定閾値
        Me.m_iMntChkIntegHourHeaterOn	= GetPrivateProfileInt("Maintenance", "Threshold_HeaterOn ", 6000, strINIPath)
        'X動作時間の超過判定閾値
        Me.m_iMntChkIntegHourXAct		= GetPrivateProfileInt("Maintenance", "Threshold_XAct ", 5000, strINIPath)
        'Y動作時間の超過判定閾値
        Me.m_iMntChkIntegHourYAct		= GetPrivateProfileInt("Maintenance", "Threshold_YAct ", 5000, strINIPath)
        'Z動作時間の超過判定閾値
        Me.m_iMntChkIntegHourZAct		= GetPrivateProfileInt("Maintenance", "Threshold_ZAct ", 5000, strINIPath)
        'PBヒータ動作時間の超過判定閾値
        Me.m_iMntChkIntegHourHeaterOn2	= GetPrivateProfileInt("Maintenance", "Threshold_HeaterOn2 ", 7000, strINIPath)
        'SGヒータ動作時間の超過判定閾値
        Me.m_iMntChkIntegHourHeaterOn3	= GetPrivateProfileInt("Maintenance", "Threshold_HeaterOn3 ", 7000, strINIPath)

        '本体電源ON経過年数の超過判定閾値
        Me.m_iMntChkRstYearPowerOn		= GetPrivateProfileInt("Maintenance", "RstYear_PowerOn ", 2, strINIPath)
        'HPヒータON経過年数の超過判定閾値
        Me.m_iMntChkRstYearHeaterOn		= GetPrivateProfileInt("Maintenance", "RstYear_HeaterOn ", 2, strINIPath)
        'X動作経過年数の超過判定閾値
        Me.m_iMntChkRstYearXAct			= GetPrivateProfileInt("Maintenance", "RstYear_XAct ", 2, strINIPath)
        'Y動作経過年数の超過判定閾値
        Me.m_iMntChkRstYearYAct			= GetPrivateProfileInt("Maintenance", "RstYear_YAct ", 2, strINIPath)
        'Z動作経過年数の超過判定閾値
        Me.m_iMntChkRstYearZAct			= GetPrivateProfileInt("Maintenance", "RstYear_ZAct ", 2, strINIPath)
        'PBヒータON経過年数の超過判定閾値
        Me.m_iMntChkRstYearHeaterOn2	= GetPrivateProfileInt("Maintenance", "RstYear_HeaterOn2 ", 2, strINIPath)
        'SGヒータON経過年数の超過判定閾値
        Me.m_iMntChkRstYearHeaterOn3	= GetPrivateProfileInt("Maintenance", "RstYear_HeaterOn3 ", 2, strINIPath)

        '測定結果自動保存の有効/無効
        Me.m_iAutoSaveMeasDataEnable = GetPrivateProfileInt("AutoSaveMeasData", "Arrival ", 0, strINIPath)
        '測定結果自動保存の保存先フォルダ
        GetPrivateProfileString("AutoSaveMeasData", "OutPutFolder", "", strSb, strSb.Capacity, strINIPath)
        Me.m_sAutoSaveMeasDataOutPutFolder = strSb.ToString()
        GetPrivateProfileString("Tool", "LastToolVersion", "", strSb, strSb.Capacity, strINIPath)
        Me.p_sLastToolVer = strSb.ToString()

        '温度コントロール
        Me.m_dHUpperLimit	= GetPrivateProfilefloat("TemperatureControl", "HeaterUpperLimit HP",		"240", strINIPath)
        Me.m_dHUpperLimit2	= GetPrivateProfilefloat("TemperatureControl", "HeaterUpperLimit Probe",	"240", strINIPath)
        Me.m_dHUpperLimit3	= GetPrivateProfilefloat("TemperatureControl", "HeaterUpperLimit SG",		"70", strINIPath)
        Me.m_dHLowerLimit3	= GetPrivateProfilefloat("TemperatureControl", "HeaterLowerLimit SG",		"20",  strINIPath)
        Me.m_dHDifSet3		= GetPrivateProfilefloat("TemperatureControl", "HeaterDifSetting SG",		"10", strINIPath)

        'ブザーコントロール
        GetPrivateProfileString("BuzzerControl", "End_Of_Buzzer", "", strSb, strSb.Capacity, strINIPath)
        Boolean.TryParse(  strSb.ToString, Me.m_bEndOfBuzzerEn )
        Me.m_EndOfBuzzerPattern	= GetPrivateProfileInt("BuzzerControl", "EndOfPattern", 0, strINIPath)
        GetPrivateProfileString("BuzzerControl", "Ok_Of_Heater", "", strSb, strSb.Capacity, strINIPath)
        Boolean.TryParse(  strSb.ToString, Me.m_bBuzzerOkOfHeatEn )

        'モータコントロール
        Me.m_iStresDetAcqTime		= GetPrivateProfileInt("MotorControl",	"StressDetectAcquisitionTime",	"20",	strINIPath)
        Me.m_iStresDetADThreshold	= GetPrivateProfileInt("MotorControl",	"StressDetADThreshold",			"5000", strINIPath)
        Me.m_iStresDetDuraTime		= GetPrivateProfileInt("MotorControl",	"StressDetDuraTime",			"3",	strINIPath)

		'測定制御
        Me.m_iSampleInputWait		= GetPrivateProfileInt(		"MeasureControl", "SampleInputWait",		"10",	strINIPath)
        Me.m_dInputGap				= GetPrivateProfilefloat(	"MeasureControl", "InputGap",				"20.0", strINIPath)
        Me.m_dHPTempRange			= GetPrivateProfilefloat(	"MeasureControl", "HPTemperatureRange",		"1.0",	strINIPath)
        Me.m_dPBTempRange			= GetPrivateProfilefloat(	"MeasureControl", "PBTemperatureRange",		"1.0",	strINIPath)
        Me.m_dSGTempRange			= GetPrivateProfilefloat(	"MeasureControl", "SGTemperatureRange",		"1.0",	strINIPath)
        Me.m_dSGStressStab			= GetPrivateProfilefloat(	"MeasureControl", "SGStressStability",		"0.100",strINIPath)
        Me.m_iPressContDelayTime	= GetPrivateProfileInt(		"MeasureControl", "PressControlDelayTime",	"100",	strINIPath)
		Me.m_iDoublePBUnlockConfirm	= GetPrivateProfileInt(		"MeasureControl", "DoubleProbeUnlockConfirm",	"0",strINIPath)
		Me.m_iGreenFlashInMeas		= GetPrivateProfileInt(		"MeasureControl", "PTGreenFlashInMeasure",		"0",strINIPath)

		Me.m_iLongTermMode			= GetPrivateProfileInt(		"MeasureControl", "Long-termModeEnable",	"0",	strINIPath)
		Me.m_iMeasureInterval		= GetPrivateProfileInt(		"MeasureControl", "Long-termModeInterval",	"10",	strINIPath)
        Me.m_iAdoptRate				= GetPrivateProfileInt(		"MeasureControl", "Long-termAdoptRate",		"2",	strINIPath)

		Me.m_iQRmode				= GetPrivateProfileInt(		"MeasureControl", "QRCode Enable",			"0",	strINIPath)
		GetPrivateProfileString(	"MeasureControl", "QRCode Error Correction","M",	strSb, strSb.Capacity, strINIPath)
		Me.m_sQRECC					= strSb.ToString()
		Me.m_iQRLogoCells			= GetPrivateProfileInt(		"MeasureControl", "QRCode Cells in Logo",	"9",	strINIPath)
		Me.m_iQRPixelInCel			= GetPrivateProfileInt(		"MeasureControl", "QRCode Pixels in Cell",	"20",	strINIPath)

        'IL無効設定
        GetPrivateProfileString("Other", "CoverILDisable", "", strSb, strSb.Capacity, strINIPath)
        Boolean.TryParse(  strSb.ToString, Me.m_bCoverILDis )
        GetPrivateProfileString("Other", "AllILDisable", "", strSb, strSb.Capacity, strINIPath)
        Boolean.TryParse(  strSb.ToString, Me.m_bAllILDis )
        Me.m_dHeatBHomePos			= GetPrivateProfilefloat(	"Other",			"HeatBoard_HomePosition", "0",	strINIPath)

        '測定値領域への読込許可
        GetPrivateProfileString("Other", "ReadRealAreaEnable", "", strSb, strSb.Capacity, strINIPath)
        Boolean.TryParse(  strSb.ToString, Me.m_bReadRealAreaEn )

	If bMsg
		Dim iRet	As int32
		Dim s		As String
		'Zセンサ安定判定しきい値の初期値チェック
		iRet = CheckSGStabInitialVal()
		Select Case iRet
		Case 1, -1
		'初期値になっている
			Call WriteLog("Zセンサ安定判定しきい値が初期値", eLOG_LEVEL.iINFO)
			s = frmMain.p_sMes(efrmMainSTR.iSGStabIni)
			MsgBox(s & vbCrLf & frmMain.p_sMes(efrmMainSTR.iMakerHelp), MsgBoxStyle.Critical + MsgBoxStyle.OkOnly,  System.Windows.Forms.Application.ProductName)
		Case Else
		End Select
		'Z荷重とたわみの比例定数の初期値チェック
		iRet = CheckGapCorrInitialVal()
		Select Case iRet
		Case 1, -1
		'初期値になっている
			Call WriteLog("Z荷重とたわみの比例定数が初期値", eLOG_LEVEL.iINFO)
			s = frmMain.p_sMes(efrmMainSTR.iGapCorrIni)
			MsgBox(s & vbCrLf & frmMain.p_sMes(efrmMainSTR.iMakerHelp), MsgBoxStyle.Critical + MsgBoxStyle.OkOnly,  System.Windows.Forms.Application.ProductName)
		Case Else
		End Select
	End If

    Catch ex As Exception
        Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
        Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
    End Try
End Sub
''' <summary>
''' 温調パラメータ設定をINIファイルから読み込む
''' </summary>
''' <param name="sDeviceID">装置名</param>
''' <returns>0:成功 -2:装置名が不正 1:PID温度パラメータ(P,I,D,DGain)が初期値では無い 2:PID温度パラメータ(PVBias,PVGain)が初期値   -1:例外発生</returns>
Public Function LoadTempParaINI(ByVal sDeviceID As String, Optional bMsg As Boolean = True) As Int32
Try
 	If sDeviceID.Substring(0,3)<>"SZK"
		Return -2
	End If
	Dim strINIPath As String = Path.Combine(SIZUKA_PATH, sDeviceID, "TempPara.ini")
    '設定を読込、読み込めない場合は初期値を代入
    '温度コントロール
    Me.m_dP			= GetPrivateProfilefloat("Hot Plate",		"P",		cdP,		strINIPath)
    Me.m_dI			= GetPrivateProfilefloat("Hot Plate",		"I",		cdI,		strINIPath)
    Me.m_dD			= GetPrivateProfilefloat("Hot Plate",		"D",		cdD,		strINIPath)
    Me.m_dDgain		= GetPrivateProfilefloat("Hot Plate",		"Dgain",	cdDgain,	strINIPath)
    Me.m_dPVBias	= GetPrivateProfilefloat("Hot Plate",		"PVBias",	cdPVBias,	strINIPath)
    Me.m_dPVGain	= GetPrivateProfilefloat("Hot Plate",		"PVGain",	cdPVGain,	strINIPath)

    Me.m_dP2		= GetPrivateProfilefloat("Probe",			"P",		cdP2,		strINIPath)
    Me.m_dI2		= GetPrivateProfilefloat("Probe",			"I",		cdI2,		strINIPath)
    Me.m_dD2		= GetPrivateProfilefloat("Probe",			"D",		cdD2,		strINIPath)
    Me.m_dDgain2	= GetPrivateProfilefloat("Probe",			"Dgain",	cdDgain2,	strINIPath)
    Me.m_dPVBias2	= GetPrivateProfilefloat("Probe",			"PVBias",	cdPVBias2,	strINIPath)
    Me.m_dPVGain2	= GetPrivateProfilefloat("Probe",			"PVGain",	cdPVGain2,	strINIPath)

    Me.m_dP3		= GetPrivateProfilefloat("Strain Guage",	"P",		cdP3,		strINIPath)
    Me.m_dI3		= GetPrivateProfilefloat("Strain Guage",	"I",		cdI3,		strINIPath)
    Me.m_dD3		= GetPrivateProfilefloat("Strain Guage",	"D",		cdD3,		strINIPath)
    Me.m_dDgain3	= GetPrivateProfilefloat("Strain Guage",	"Dgain",	cdDgain3,	strINIPath)
    Me.m_dPVBias3	= GetPrivateProfilefloat("Strain Guage",	"PVBias",	cdPVBias3,	strINIPath)
    Me.m_dPVGain3	= GetPrivateProfilefloat("Strain Guage",	"PVGain",	cdPVGain3,	strINIPath)

	Dim iRet	As int32 = 0
	If bMsg
		'温度パラメータ初期値チェック
		Dim s		As String
		iRet = CheckTempParaInitialVal()
		Select Case iRet
		Case 1
		'P,I,D,Dgainが初期値以外である
			Call WriteLog("温度パラメータ(P,I,D,DGain)が変更されている", eLOG_LEVEL.iINFO)
			s = frmMain.p_sMes(efrmMainSTR.iTempParaIni1)
			MsgBox(s & vbCrLf & frmMain.p_sMes(efrmMainSTR.iMakerHelp), MsgBoxStyle.Critical + MsgBoxStyle.OkOnly,  System.Windows.Forms.Application.ProductName)
		Case 2
		'PVBias,PVGainが初期値になっている
			Call WriteLog("温度パラメータ(PVBias,PVGain)が初期値", eLOG_LEVEL.iINFO)
			s = frmMain.p_sMes(efrmMainSTR.iTempParaIni)
			MsgBox(s, MsgBoxStyle.Critical + MsgBoxStyle.OkOnly,  System.Windows.Forms.Application.ProductName)
		Case -1
		'例外発生
		Case Else
		End Select
	End If
	Return iRet
Catch ex As Exception
    Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
    Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Return -1
End Try
End Function

''' <summary>
''' 入力選択項目の候補登録ファイルをセーブする
''' </summary>
''' <param name="sDeviceID">装置名</param>
''' <returns></returns>
Public Function InputSelectionFileSave(ByVal sDeviceID As String) As Int32
Dim fs	As FileStream = Nothing
Dim sw	As StreamWriter = Nothing
Dim sBuf As String = Nothing
Try
 	If sDeviceID.Substring(0,3)<>"SZK"
		Return 0
	End If
    Dim strINIPath As String = Path.Combine(SIZUKA_PATH, sDeviceID, SPEC_PATH, InpSelectFile)
	fs = New FileStream(strINIPath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite)
	sw = New StreamWriter(fs, Encoding.UTF8)
		sBuf = String.Join(",", p_sSampleAcum)			'サンプル名
		sw.WriteLine(sBuf)
		sBuf = String.Join(",", p_sMeasurerAcum)		'測定者
		sw.WriteLine(sBuf)
	sw.Close
	fs.Close
	Return 0	
Catch ex As Exception
    Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Return -1
End Try
End Function
''' <summary>
''' 入力選択項目の候補登録ファイルを読込む
''' </summary>
''' <param name="sDeviceID">装置名</param>
''' <returns></returns>
Public Function InputSelectionFileRead(ByVal sDeviceID As String) As Int32
Dim fs	As FileStream = Nothing
Dim sr	As StreamReader = Nothing
Dim sBuf As String = Nothing
Dim ss() As String = Nothing
Try
 	If sDeviceID.Substring(0,3)<>"SZK"
		Return 0
	End If
    Dim strINIPath As String = Path.Combine(SIZUKA_PATH, sDeviceID, SPEC_PATH, InpSelectFile)
	fs = New FileStream(strINIPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)
	sr = New StreamReader(fs, Encoding.UTF8)
		sBuf = sr.ReadLine()
		If Not IsNothing(sbuf)
			p_sSampleAcum = sBuf.Split(",")		'サンプル名
		End If
		sBuf = sr.ReadLine()
		If Not IsNothing(sbuf)
			p_sMeasurerAcum = sBuf.Split(",")	'測定者
		End If
	sr.Close
	fs.Close
	Return 0	
Catch ex As Exception
    Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Return -1
End Try
End Function


End Class

#end if




''' <summary>
''' タブページ拡張
''' </summary>
Public Class TabPageManager
    Private Class TabPageInfo
        Public TabPage As TabPage
        Public Visible As Boolean

        Public Sub New(ByVal page As TabPage, ByVal v As Boolean)
            TabPage = page
            Visible = v
        End Sub
    End Class

    Private _tabPageInfos As TabPageInfo() = Nothing
    Private _tabControl As TabControl = Nothing

    ''' <summary>
    ''' TabPageManagerクラスのインスタンスを作成する
    ''' </summary>
    ''' <param name="crl">基になるTabControlオブジェクト</param>
    Public Sub New(ByVal crl As TabControl)
        _tabControl = crl
        _tabPageInfos = _
            New TabPageInfo(_tabControl.TabPages.Count - 1) {}
        Dim i As Integer
        For i = 0 To _tabControl.TabPages.Count - 1
            _tabPageInfos(i) = _
                New TabPageInfo(_tabControl.TabPages(i), True)
        Next i
    End Sub

    ''' <summary>
    ''' TabPageの表示・非表示を変更する
    ''' </summary>
    ''' <param name="index">変更するTabPageのIndex番号</param>
    ''' <param name="v">表示するときはTrue。
    ''' 非表示にするときはFalse。</param>
    Public Sub ChangeTabPageVisible( ByVal index As Integer, ByVal v As Boolean)
        If _tabPageInfos(index).Visible = v Then
            Return
        End If
        _tabPageInfos(index).Visible = v
        _tabControl.SuspendLayout()
        _tabControl.TabPages.Clear()
        Dim i As Integer
        For i = 0 To _tabPageInfos.Length - 1
            If _tabPageInfos(i).Visible Then
                _tabControl.TabPages.Add(_tabPageInfos(i).TabPage)
            End If
        Next i
        _tabControl.ResumeLayout()
    End Sub
End Class




''' <summary> コンストラクタ。メンバ変数に引数を受け取る。 </summary>
Public Sub New(ByRef cts As CToolSettings)
    InitializeComponent()
    'm_mssEditing = New MotorSpeedSetting()
    ' 反映用の変数(m_ctsMainFrm)に参照を代入
    m_ctsMainFrm = cts

End Sub
'測定モードに変更があったか
Public p_LTModeChange As Boolean = False


'■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
'   フォームのロード／アンロード／アクティブ
'■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
''' <summary> 設定フォームの初期状態を設定する。 </summary>
Private Sub frmOptions_Load(sender As Object, e As EventArgs) Handles MyBase.Load
    FormLoad
	original = CType(DeepCopyObject(m_ctsMainFrm), cToolSettings)		'変更前のcToolSettingsクラスの新しいインスタンスのコピーを作成する
End Sub
Dim sArry() As String
Private Sub FormLoad()
Try
#If Not NEW_SPEC
    ' ツールチップを設定
    sbDispSpecToolTip()
#end If
    '言語により表示を変更
    Call SetFormLanguage(Me, p_iLanguage,,m_sStr)
	p_lstMarkerStyle = m_sStr(eMes.iMarkerSelAry).Split(",").ToList()	'マーカスタイル選択文字列
	'AutoPrint,Specs Tabを非表示にする
	Dim clTabm As New TabPageManager(TabControl1)
	clTabm.ChangeTabPageVisible(eTABOpt.AUTOPRINT,	False)
	clTabm.ChangeTabPageVisible(eTABOpt.SPECS,		False)

    'm_strDataFolder = Path.Combine(SIZUKA_PATH, p_sEquipName, MESDAT_PATH)
    '本体との接続状態を退避
    m_iConnectStateBak = p_RTM(0).p_iConnectState

	'CToolSettingsクラス->フォームコントロール
	MemToScr()

Catch ex As Exception
    Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
    Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try

End Sub
''' <summary>
''' CToolSettingsクラス->フォームコントロール値へ転送
''' </summary>
Private Sub MemToScr()
Try
    '### Toolタブ #####
    'Language
    Select Case m_ctsMainFrm.iLang
    Case 1
        rbJPN.Checked = True
    Case 4
        rbTChinese.Checked = True
    Case 3
        rbKorean.Checked = True
    Case 2
        rbSChinese.Checked = True
    Case Else
        rbENG.Checked = True
    End Select
    'Administrator Option --------------
    If m_ctsMainFrm.iCanRemoveData >= 0 Then
        cbCanDelete.Checked = True
    Else
        cbCanDelete.Checked = False
    End If
    If m_ctsMainFrm.iCanModifyCF >= 0 Then
        cbModifyCF.Checked = True
    Else
        cbModifyCF.Checked = False
    End If
    If m_ctsMainFrm.iCanModifyRecalc >= 0 Then
        cbModifyRecalc.Checked = True
    Else
        cbModifyRecalc.Checked = False
    End If
    If m_ctsMainFrm.iCanModifySpec >= 0 Then
        cbModifySpec.Checked = True
    Else
        cbModifySpec.Checked = False
    End If
    '--------------------------------------
#If 1
	SafeExecContinue(MethodBase.GetCurrentMethod.Name, "メンテナンス超過判定",
		Sub() nudStandbyTime.Value = m_ctsMainFrm.StanbyTime,                                 'スタンバイ時間
		Sub() nudMainteThr1.Value = m_ctsMainFrm.iMntChkIntegHourPowerOn,                     '本体電源ON時間の超過判定閾値
		Sub() nudMainteThr2.Value = m_ctsMainFrm.iMntChkIntegHourHeaterOn,                    'HPヒータON時間の超過判定閾値
		Sub() nudMainteThr3.Value = m_ctsMainFrm.iMntChkIntegHourXAct,                        'X動作時間の超過判定閾値
		Sub() nudMainteThr4.Value = m_ctsMainFrm.iMntChkIntegHourHeaterOn2,                   'PBヒータON時間の超過判定閾値
		Sub() nudMainteThr5.Value = m_ctsMainFrm.iMntChkIntegHourHeaterOn3                    'SGヒータON時間の超過判定閾値
	)

#else
	SafeExecContinue(MethodBase.GetCurrentMethod.Name, "メンテナンス超過判定",
    'SIZUKA Setting
     Sub() nudStandbyTime.Value =  m_ctsMainFrm.StanbyTime,         'スタンバイ時間
	'本体電源ON時間の超過判定閾値
	nudMainteThr1.Value = m_ctsMainFrm.iMntChkIntegHourPowerOn,
	'HPヒータON時間の超過判定閾値
	nudMainteThr2.Value = m_ctsMainFrm.iMntChkIntegHourHeaterOn

	'X動作時間の超過判定閾値
	nudMainteThr3.Value = m_ctsMainFrm.iMntChkIntegHourXAct

	'PBヒータON時間の超過判定閾値
	nudMainteThr4.Value = m_ctsMainFrm.iMntChkIntegHourHeaterOn2

	'SGヒータON時間の超過判定閾値
	nudMainteThr5.Value = m_ctsMainFrm.iMntChkIntegHourHeaterOn3
	End Sub,"メンテナンス超過判定")
#end If

	'測定データに計算結果も含めるか
	cbDatafileCalcEnable.Checked = If(m_ctsMainFrm.CalcResultEnable=1, True, False)

	'サンプル名選択リスト
	cbxSampleName.Items.Clear
	cbxSampleName.Items.AddRange(m_ctsMainFrm.p_sSampleAcum)
	cbxSampleName.SelectedIndex = 0
	'測定者選択リスト
	cbxMeasurer.Items.Clear
	cbxMeasurer.Items.AddRange(m_ctsMainFrm.p_sMeasurerAcum)
	cbxMeasurer.SelectedIndex = 0



    '### Graphタブ #####
    'GridLine
    Select Case m_ctsMainFrm.penGrids.DashStyle
    Case Drawing2D.DashStyle.Solid
        rbSolid.Checked = True
    Case Drawing2D.DashStyle.Dash
        rbDash.Checked = True
    Case Drawing2D.DashStyle.Dot
        rbDotGrids.Checked = True
    Case Else
        rbNoGrids.Checked = True
    End Select
    Select Case m_ctsMainFrm.iHideNgtv
    Case 0
        rbDisplayNgtv.Checked = True
    Case 1
        rbHideNgtv.Checked = True
    Case Else
        rbHideNgtv.Checked = True
    End Select

	nudThinningValueR.Value = m_ctsMainFrm.iDetailsR
	nudThinningValueD.Value = m_ctsMainFrm.iDetailsD

    Select Case m_ctsMainFrm.iARDisp
    Case 0
        rbARDFill.Checked = True
    Case Else
        rbARDTransparent.Checked = True
    End Select
    Select Case m_ctsMainFrm.iRTXAxis
    Case 0
        rbRXauto.Checked = True
    Case Else
        rbRXFix.Checked = True
    End Select
    Select Case m_ctsMainFrm.iRTYAxis
    Case 0
        rbRYauto.Checked = True
    Case Else
        rbRYFix.Checked = True
    End Select
    Select Case m_ctsMainFrm.iRTXAxis
    Case 0
        rbRXauto.Checked = True
    Case Else
        rbRXFix.Checked = True
    End Select
	nudScrolWidth.Value = m_ctsMainFrm.iScrlWidth			'スクロール幅(秒)

	cbxMarkerStyle.Items.Clear()
	cbxMarkerStyle.Items.AddRange(p_lstMarkerStyle.ToArray)
	cbxMarkerStyle.SelectedIndex = m_ctsMainFrm.iMarkerStyle		'マーカスタイル
	nudMarkerSize.Value = m_ctsMainFrm.iMarkerSize					'マーカサイズ

    'カラーの設定
    With m_ctsMainFrm
        ReDim m_lblClr(.clrY1Default.Length - 1 + 2)
        For iCnt As Integer = 0 To m_lblClr.Length - 1
            '最後から2番目は背景色、1番目は目盛り線
            Select Case iCnt
            Case m_lblClr.Length - 2
                m_lblClr(iCnt) = lblClrB
                m_lblClr(iCnt).BackColor = .clrGraphBack
            Case m_lblClr.Length - 1
                m_lblClr(iCnt) = lblClrL
                m_lblClr(iCnt).BackColor = .penGrids.Color
            Case Else
                m_lblClr(iCnt) = CType(grpDefClr.Controls("lblClr" & CStr(iCnt)), Label)
                m_lblClr(iCnt).BackColor = .clrY1Default(iCnt)
            End Select

            m_lblClr(iCnt).ForeColor = GetForeColor(m_lblClr(iCnt).BackColor)

            AddHandler m_lblClr(iCnt).Click, AddressOf sblblClr_Click
        Next

        ''Y2のカラー反転設定
        'If .bY2Opposite = False Then
        '    cbY2Opp.Checked = False
        'Else
        '    cbY2Opp.Checked = True
        'End If
    End With
    'デフォルトカラー変更表示
    grpDefClr.Visible = True

	lblColorAssignDesc.Text = m_sStr(eMes.iGColorDesc)			'色割当て説明



    '##################
        
    '### Calclation タブ ########
    'Degree for Curve THR and Slope Method
    cbxFilterEnable.Checked = If(m_ctsMainFrm.iFilterEnable=0, False, True)
    Select Case  m_ctsMainFrm.sFilterKind
    Case "FIR"
        rbFIR.Checked = True
        gbxFilterFir.Enabled = True
        gbxFilterIIR.Enabled = False
    Case "IIR"
        rbIIR.Checked = True
        gbxFilterFir.Enabled = False
        gbxFilterIIR.Enabled = True
    End Select
    'FIR
    cbxWinKind.Items.Clear()
    cbxWinKind.Items.Add("Hamming")
    cbxWinKind.Items.Add("Hann")
    cbxWinKind.Items.Add("Blackman")
    cbxWinKind.Text			= m_ctsMainFrm.sFilterWinKind       '窓関数種類
    nudFIRCoffFreq.Value	= m_ctsMainFrm.dFIRCutOffFreq		'カットオフ周波数
    nudFilterLen.Value		= m_ctsMainFrm.iFilterLen			'フィルタ

    'IIR

    nudIIRCoffFreq.Value	= m_ctsMainFrm.dIIRCutOffFreq		'カットオフ周波数
    nudIIRDeg.Value			= m_ctsMainFrm.iIIRDeg              'フィルタ次数

	If cbxFilterEnable.Checked
		If rbFIR.Checked
			gbxFilterFIR.BackColor = Color.Yellow
			gbxFilterIIR.BackColor = Control.DefaultBackColor
		else
			gbxFilterIIR.BackColor = Color.Yellow
			gbxFilterFIR.BackColor = Control.DefaultBackColor
		End If
	Else
		gbxFilterIIR.BackColor = Control.DefaultBackColor
		gbxFilterFIR.BackColor = Control.DefaultBackColor
	End If

    ''lblFilterFileName.Text = m_ctsMainFrm.sFilterFileName


    cbxAutoReg.Checked		= If(m_ctsMainFrm.DBAutoReg = 0, False, True)		'DB自動登録機能
    cbxSPC.Checked			= If(m_ctsMainFrm.SPCEn = 0, False, True)			'SPC機能 有効
    nudSPCCalcSample.Value	= m_ctsMainFrm.SPCCalcSample						'SPC機能 計算サンプル数
        
		
	'############################


    '### Remote タブ ########
    'Auto Start
    Select Case m_ctsMainFrm.iAutoStart
    Case 0 To 100
        cbAutoStart.Checked = True
        nudAutoStart.Value = m_ctsMainFrm.iAutoStart
    Case Else
        nudAutoStart.Value = 3
    End Select
    'Equipment List
	dgvEquipList.Rows.Clear
    For i As Integer = 0 To 9
        'Dim strMCF As String = m_ctsMainFrm.dCF3(i).ToString() + " -- " + m_ctsMainFrm.dCF2(i).ToString() + " -- " + m_ctsMainFrm.dCF1(i).ToString()
        'Dim strTCF As String = m_ctsMainFrm.dTCF6(i).ToString() + " -- " + m_ctsMainFrm.dTCF5(i).ToString() + " -- " + m_ctsMainFrm.dTCF4(i).ToString() + " -- " + _
        '    m_ctsMainFrm.dTCF3(i).ToString() + " -- " + m_ctsMainFrm.dTCF2(i).ToString() + " -- " + m_ctsMainFrm.dTCF1(i).ToString()
        'dgvEquipList.Rows.Add(My.Resources.Uncheckbox, m_ctsMainFrm.strIP(i), m_ctsMainFrm.strEquipName(i), strMCF, strTCF, "Set")
        dgvEquipList.Rows.Add(My.Resources.Uncheckbox, m_ctsMainFrm.strIP(i), m_ctsMainFrm.strEquipName(i), m_ctsMainFrm.sXYSensorSerial(i), m_ctsMainFrm.sZSensorSerial(i), "", "", "Set")
        dgvEquipList.Item(EL_COL.USEEQUIP, i).Tag = False

        ' FTP通信用設定をメンバ変数に代入
        m_strUserId(i) = m_ctsMainFrm.strFTPUser(i)
        m_strPassword(i) = m_ctsMainFrm.strFTPPass(i)
    Next i
	dgvEquipList.Columns(EL_COL.NAME).ReadOnly		= True		'本体名
	dgvEquipList.Columns(EL_COL.XYSENS).ReadOnly	= True		'XYセンサID
	dgvEquipList.Columns(EL_COL.ZSENS).ReadOnly		= True		'ZセンサID

    If m_ctsMainFrm.iSelMDK < 10 Then
        dgvEquipList.Item(EL_COL.USEEQUIP, m_ctsMainFrm.iSelMDK).Value = My.Resources.Checkbox
        dgvEquipList.Item(EL_COL.USEEQUIP, m_ctsMainFrm.iSelMDK).Tag = True
    End If
    txtPort.Text = m_ctsMainFrm.strPort     'TCPポートNo
    'ブザーコントロール
    cbxEndOfMesure.Checked	= m_ctsMainFrm.EndOfBuzzerEn
	nudBZPattern.Value		= m_ctsMainFrm.EndOfBuzzerPattern
    cbxOkOfHeat.Checked		= m_ctsMainFrm.BuzzerOkOfHeatEn
    '日時設定 初期設定
    sbNowDateSet()

	'隠し設定
	gpxHiddenSetting.Visible = False
	cbxDebugFileEn.Checked			= p_bDebugFileOutEnable
	cbxInputEnMeasureing.Checked	= p_bInputEnMeasureing
	cbxCalibValChangeEn.Checked		= p_bCalibValChangeEn
	cbxZStabDis.Checked				= p_bZStrStabDis
	cbxPcLifeChkDis.Checked			= p_bPcLifeCheckDis
	cbxMultActEn.Checked			= m_ctsMainFrm.MultiActivate
	cbxMeasTimeEn.Checked			= p_bMeasTimeEn
	cbxScanTimeEn.Checked			= p_bScanTimeEn
	cbxCalcResultFOutEn.Checked		= p_bCalcResultFOutEn
	cbxTempNoLimitEn.Checked		= p_bTempNoLimitEn














    '############################


    '### SPECS タブ #####################################################

    fcDisplayRTGSETList()


	'### Tool2 タブ ######################################################

	nudStrsDetAcqTime.Value     = p_RTM(0).p_mltcp.StressDetectAcquisitionTime
	nudStrsDetADThreshold.value = p_RTM(0).p_mltcp.StressDetectADThreshold
	nudStresDetDuraTime.Value   = p_RTM(0).p_mltcp.StressDetectDurationTime




    '### ゼロ調タブ #########################
	'ゼロ調モード
	Select Case m_ctsMainFrm.ZeroAdjMode
	Case 0
		rbZeroXcont.Checked = True
	Case 1
		rbZeroZcont.Checked = True
	End Select
    'ゼロ調整終了条件の値
    nudZeroStartPosHP.Text		= m_ctsMainFrm.dZeroStaPosHP.ToString("F3")
    nudStaFlacRange.Text		= m_ctsMainFrm.iStaFlucRange.ToString()
    nudStaMonTime.Text			= m_ctsMainFrm.iStaMonTime.ToString()

    '中央出し関係
    nudCentStartPosHP.Text		= m_ctsMainFrm.dCenterStaPosHP.ToString("F3")
    lblCenterX.Text				= m_ctsMainFrm.iCenterResultsX.ToString()
    lblCenterY.Text				= m_ctsMainFrm.iCenterResultsY.ToString()

    '手動駆動関係
    nudHeatPos.Value			= 0.0

	''cboXYMode.Items.Clear()
	''cboXYMode.Items.AddRange(lstXYMode.ToArray)
''      cboXYMode.SelectedIndex		= p_RTM(0).p_mltcp.ActMode

    nudXAmp.Value				= 0
    nudYAmp.Value				= 0
    nudZAmp.Value				= 0
    nudXPeriod.Value			= 1
    nudYPeriod.Value			= 1
    nudZPeriod.Value			= 1

    '中心座標手動登録
    nudCenterX.Value			= m_ctsMainFrm.iCenterResultsX
    nudCenterY.Value			= m_ctsMainFrm.iCenterResultsY

		'Z軸上限リミット位置
	nudZUpperLimit.Value		= Ctype(m_ctsMainFrm.ZUpperLimitPos, Double) / 1000.0


    '######################################


        



    '### AutoPrintタブ ###
    'Auto Print 有効/無効
    If m_ctsMainFrm.iAutoPrintEnableValue = 1 Then
        cbxAutPrnt.Checked = True
    Else
        cbxAutPrnt.Checked = False
    End If
    '保存タイプ(0:JPEG,1:PDF)
    If p_bValid_OfficeVer_Install = True Then
        rbtnPdf.Enabled = True
        rbtnJpg.Enabled = True
        Select Case m_ctsMainFrm.iAutoPrintTypeValue
        Case 0
            rbtnJpg.Checked = True
            rbtnPdf.Checked = False
        Case 1
            rbtnJpg.Checked = False
            rbtnPdf.Checked = True
        End Select
    Else
        rbtnJpg.Checked = True
        rbtnPdf.Checked = False
        rbtnPdf.Enabled = False
    End If
    '保存先フォルダ
    txtStrgFld.Text = m_ctsMainFrm.strAutoPrintOutPutFolderValue
    '測定場所
    '過去に入力した値を複数 DF_OPER_LIST_SEP 区切りで格納
    sArry = Split(m_ctsMainFrm.sAutoPrintMesurePlaceList, DF_OPER_LIST_SEP)
    cbxMeasPlace.Items.Clear()
    For i As Integer = 0 To sArry.Length - 1
        If sArry(i).Length > 0 Then
            cbxMeasPlace.Items.Add(sArry(i))
        End If
    Next i
    cbxMeasPlace.Text = m_ctsMainFrm.strAutoPrintMesurePlaceValue
    '測定者
    sArry = Split(m_ctsMainFrm.sAutoPrintOperatorList, DF_OPER_LIST_SEP)
    cbxMeasOperator.Items.Clear()
    For i As Integer = 0 To sArry.Length - 1
        If sArry(i).Length > 0 Then
            cbxMeasOperator.Items.Add(sArry(i))
        End If
    Next i
    cbxMeasOperator.Text = m_ctsMainFrm.strAutoPrintOperatorValue

    'Auto Printのディスク容量閾値
    nudDskSpc.Value = m_ctsMainFrm.iAutoPrintDiskCapacityValue

    '測定結果自動保存の有効/無効
    chkAutSaveMeasData.Checked = False
    chkAutSaveMeasData.Enabled = False

        ''測定結果自動エクスポート有効化ライセンスのチェック (測定結果自動エクスポート)
        ''If p_uaMngOpt(LIC_OPT_TYP.AUTO_EXP).iNo >= LIC_OPTNO_AUTO_EXP.STANDARD Then
        ''    '一致するライセンスオプションがあった
        ''    chkAutSaveMeasData.Enabled = True
        ''    If m_ctsMainFrm.iAutoSaveMeasDataEnable = 1 Then
        ''        chkAutSaveMeasData.Checked = True
        ''    Else
        ''        chkAutSaveMeasData.Checked = False
        ''    End If
        ''End If

    '測定結果自動保存の保存先フォルダ
    txtStrgFldMeasData.Text = m_ctsMainFrm.sAutoSaveMeasDataOutPutFolder
    timMain.Interval = 100
    timMain.Enabled = True
    lvSpec.MultiSelect = True
    lvRtgset.MultiSelect = True
    '##################



    '### Temprature Cotrol タブ #########
    '温度コントロール
    Dim sC As String = "Default: "
    'HP パラメータ
    nudP.Value      = m_ctsMainFrm.TempCnt_P
    nudI.Value      = m_ctsMainFrm.TempCnt_I
    nudD.Value      = m_ctsMainFrm.TempCnt_D
    nudDgain.Value  = m_ctsMainFrm.TempCnt_Dgain
    nudPVBias.Value = m_ctsMainFrm.TempCnt_PVBias
    nudPVGain.Value = m_ctsMainFrm.TempCnt_PVGain
    nudHUpperTemp.Value = m_ctsMainFrm.TempCnt_HUpperLimit

	ToolTipOption.SetToolTip(nudP, sC & Ctoolsettings.cdP)
	ToolTipOption.SetToolTip(nudI, sC & Ctoolsettings.cdI)
	ToolTipOption.SetToolTip(nudD, sC & Ctoolsettings.cdD)
	ToolTipOption.SetToolTip(nudDgain, sC & Ctoolsettings.cdDgain)
	ToolTipOption.SetToolTip(nudPVBias, sC & Ctoolsettings.cdPVBias)
	ToolTipOption.SetToolTip(nudPVgain, sC & Ctoolsettings.cdPVgain)


    'Probe パラメータ
    nudP2.Value     = m_ctsMainFrm.TempCnt_P2
    nudI2.Value     = m_ctsMainFrm.TempCnt_I2
    nudD2.Value     = m_ctsMainFrm.TempCnt_D2
    nudDgain2.Value = m_ctsMainFrm.TempCnt_Dgain2
    nudPVBias2.Value = m_ctsMainFrm.TempCnt_PVBias2
    nudPVGain2.Value = m_ctsMainFrm.TempCnt_PVGain2
    nudHUpperTemp2.Value = m_ctsMainFrm.TempCnt_HUpperLimit2

	ToolTipOption.SetToolTip(nudP2, sC & Ctoolsettings.cdP2)
	ToolTipOption.SetToolTip(nudI2, sC & Ctoolsettings.cdI2)
	ToolTipOption.SetToolTip(nudD2, sC & Ctoolsettings.cdD2)
	ToolTipOption.SetToolTip(nudDgain2, sC & Ctoolsettings.cdDgain2)
	ToolTipOption.SetToolTip(nudPVBias2, sC & Ctoolsettings.cdPVBias2)
	ToolTipOption.SetToolTip(nudPVgain2, sC & Ctoolsettings.cdPVgain2)

    'Strain guage パラメータ
    nudP3.Value     = m_ctsMainFrm.TempCnt_P3
    nudI3.Value     = m_ctsMainFrm.TempCnt_I3
    nudD3.Value     = m_ctsMainFrm.TempCnt_D3
    nudDgain3.Value = m_ctsMainFrm.TempCnt_Dgain3
    nudPVBias3.Value = m_ctsMainFrm.TempCnt_PVBias3
    nudPVGain3.Value = m_ctsMainFrm.TempCnt_PVGain3
    nudHUpperTemp3.Value	= m_ctsMainFrm.TempCnt_HUpperLimit3
    nudHLowerTemp3.Value	= m_ctsMainFrm.TempCnt_HLowerLimit3
    nudHDifSetTemp3.Value	= m_ctsMainFrm.TempCnt_HDifSetTemp3

	ToolTipOption.SetToolTip(nudP3, sC & Ctoolsettings.cdP3)
	ToolTipOption.SetToolTip(nudI3, sC & Ctoolsettings.cdI3)
	ToolTipOption.SetToolTip(nudD3, sC & Ctoolsettings.cdD3)
	ToolTipOption.SetToolTip(nudDgain3, sC & Ctoolsettings.cdDgain3)
	ToolTipOption.SetToolTip(nudPVBias3, sC & Ctoolsettings.cdPVBias3)
	ToolTipOption.SetToolTip(nudPVgain3, sC & Ctoolsettings.cdPVgain3)


    'LED切替閾値、有効無効
    ''nudLEDTempObsrv.Value = m_ctsMainFrm.TempLED
    ''cbxTempLEDEn.Checked = m_ctsMainFrm.TempLEDEn
    'IL無効設定
    cbxCoverILDis.Checked	= m_ctsMainFrm.CoverILDis
    cbxMultActEn.Checked	= m_ctsMainFrm.MultiActivate = 1
	'測定値領域への読込許可
    cbxReadRealAreaEn.Checked	= m_ctsMainFrm.ReadRealAreaEn

	'温度設定フィールド範囲設定
	nudHUpperTemp.Maximum	= HEATER_UPLMT_NUD
	nudHUpperTemp.Minimum	= HEATER_LWLMT_NUD
	nudHUpperTemp2.Maximum	= HEATER_UPLMT_NUD
	nudHUpperTemp2.Minimum	= HEATER_LWLMT_NUD

    '##################

    '### Measure Cotrol タブ #########
	nudSampleInputWait.Value		= m_ctsMainFrm.SampleInputWait
	nudInputGap.Value				= m_ctsMainFrm.InputGap
	nudHPTempRange.Value			= m_ctsMainFrm.HPTempRange
	nudPBTempRange.Value			= m_ctsMainFrm.PBTempRange
	nudSGTempRange.Value			= m_ctsMainFrm.SGTempRange
	nudStrainStab.Value				= m_ctsMainFrm.SGStressStab
	ToolTipOption.SetToolTip(nudStrainStab, sC & CToolSettings.cStressStab)
	nudPresContDeyayTime.Value		= m_ctsMainFrm.PressContDelayTime
    nudGapCorr.Value		= m_ctsMainFrm.GapCorr				'GAP補正値 (dig/um)
	ToolTipOption.SetToolTip(nudGapCorr, sC & Ctoolsettings.cGapCorr)
	ckbDoublePBUnlockConfirm.Checked	= m_ctsMainFrm.DoublePBUnlockConfirm
	ckbGreenFlashInMeas.Checked			= m_ctsMainFrm.GreenFlashInMeas

	cbxLongMeasMode.Checked			= m_ctsMainFrm.LTModeEnable
    nudLTMeasInterval.Value			= m_ctsMainFrm.LTMeasureInterval
    nudLTAdoptRate.Value			= m_ctsMainFrm.LTAdoptRate
	gbxLongMeasMode.BackColor = If(cbxLongMeasMode.Checked, Color.Yellow, Control.DefaultBackColor)


	cbxQRCodeEnable.Checked			= m_ctsMainFrm.QRModeEnable
	Dim sECC() As String = {"L","M","Q","H"}
	cboECC.Items.Clear
	cboECC.Items.AddRange(sECC)
    cboECC.Text						= m_ctsMainFrm.QREcc
    nudCellsInLogo.Value			= m_ctsMainFrm.QRLogoCells
    nudPixelInCell.Value			= m_ctsMainFrm.QRPixelInCel
	gbxQRCode.BackColor = If(cbxQRCodeEnable.Checked, Color.Yellow, Control.DefaultBackColor)

    '##################


	'管理者モード 許可
	If m_ctsMainFrm.AdminEnable

	Else
	'管理者モードでない
		For i=0 To TabControl1.TabPages.Count-1
			TabControl1.TabPages(i).Enabled = False
		Next
		TabControl1.TabPages(5).Enabled = True			'温度パラメータ
		TabControl1.TabPages(6).Enabled = True			'温度プロファイル
		btnPIDMonitor.Enabled			= False			'温度モニタ
		nudP.Enabled					= False
		nudP2.Enabled					= False
		nudP3.Enabled					= False
		nudI.Enabled					= False
		nudI2.Enabled					= False
		nudI3.Enabled					= False
		nudD.Enabled					= False
		nudD2.Enabled					= False
		nudD3.Enabled					= False
		nudDgain.Enabled				= False
		nudDgain2.Enabled				= False
		nudDgain3.Enabled				= False
		nudHUpperTemp.Enabled			= False
		nudHUpperTemp2.Enabled			= False
		nudHUpperTemp3.Enabled			= False
		nudPVBias3.Enabled				= False
		nudPVGain3.Enabled				= False
		nudHLowerTemp3.Enabled			= False
		nudHDifSetTemp3.Enabled			= False
	End If
	dgvEquipList.ColumnHeadersDefaultCellStyle.BackColor	= If(p_bEquipListEn,System.Drawing.SystemColors.Window,System.Drawing.SystemColors.Control)
	dgvEquipList.DefaultCellStyle.BackColor					= If(p_bEquipListEn,System.Drawing.SystemColors.Window,System.Drawing.SystemColors.Control)

	gbxLongMeasMode.Visible = True
Catch ex As Exception
    Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub

'■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
'   タイマーイベント
'■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
'常時タイマー
Private Sub timMain_Tick(sender As Object, e As EventArgs) Handles timMain.Tick
    timMain.Enabled = False
    Try
        'メイン画面の操作無効化状態に合わす
        'Me.Enabled = m_fM.Enabled
        '手動駆動ボタン有効無効
        ''if m_fM.p_RTM(0).p_mltcp.PosOrder = 0 then
        ''    btnExecCenter.Enabled =True
        ''    btnXYDrive.Enabled =True
        ''    btnHotPlateDrive.Enabled =True
        ''    btnXAxisDrive.Enabled =True
        ''    btnYAxisDrive.Enabled =True
        ''else
        ''    btnExecCenter.Enabled =False
        ''    btnXYDrive.Enabled =False
        ''    btnHotPlateDrive.Enabled =False
        ''    btnXAxisDrive.Enabled =False
        ''    btnYAxisDrive.Enabled =False
        ''End If

        '本体との接続状態が変化
        If m_iConnectStateBak <> p_RTM(0).p_iConnectState Then
            If p_RTM(0).p_iConnectState = CONNECT_ST.ONLINE Or _
                p_RTM(0).p_iConnectState = CONNECT_ST.OFFLINE _
            Then
                '本体との接続状態を退避
                m_iConnectStateBak = p_RTM(0).p_iConnectState

                If m_iConnectStateBak = CONNECT_ST.ONLINE Then
                    'オンラインの場合
                    gbxMdkSet.Enabled = True            'インターロック有効
                    gbxZeroAdj.Enabled = True           'ゼロ調整設定
                    gbxCentering.Enabled = True             'キャリブ機能
                Else
                    'オフラインの場合
                    gbxMdkSet.Enabled = False           'インターロック有効
                    gbxZeroAdj.Enabled = False          'ゼロ調整設定
                    gbxCentering.Enabled = False            'キャリブ機能
                End If
            End If
        End If
        '中心点出しの時のボタン状態表示
        Select case  p_RTM(0).p_iCenterState 
        Case 0
            ButtonFlash(btnExecCenter, Color.Yellow, Color.Transparent)
        Case 1, -1
            btnExecCenter.BackColor = System.Drawing.SystemColors.Control
        Case 2
            btnExecCenter.BackColor = Color.Red
        End Select
        lblCenterX.Text = p_RTM(0).p_mltcp.CenterPosX
        lblCenterY.Text = p_RTM(0).p_mltcp.CenterPosY

        '手動駆動 ボタン状態表示
        if (p_RTM(0).p_mltcp.PosOrder And MemLinkTCP.eWPosOrderBit.iX) <> 0 Then
            ButtonFlash(btnXAxisDrive, Color.Yellow, Color.Transparent)
        Else
            btnXAxisDrive.BackColor = Color.Transparent
        End If
        if (p_RTM(0).p_mltcp.PosOrder And MemLinkTCP.eWPosOrderBit.iY) <> 0 Then
            ButtonFlash(btnYAxisDrive, Color.Yellow, Color.Transparent)
        Else
            btnYAxisDrive.BackColor = Color.Transparent
        End If
        if (p_RTM(0).p_mltcp.PosOrder And MemLinkTCP.eWPosOrderBit.iZ) <> 0 Then
            ButtonFlash(btnHotPlateDrive, Color.Yellow, Color.Transparent)
        Else
            btnHotPlateDrive.BackColor = Color.Transparent
        End If


        '手動駆動 XYモード時のボタン状態表示
        if (p_RTM(0).p_mltcp.PosOrder And MemLinkTCP.eWPosOrderBit.iXY_MODE) <> 0 Then
            ButtonFlash(btnXYDrive, Color.Yellow, Color.Transparent)
        Else
            btnXYDrive.BackColor = Color.Transparent
        End If

        '原点復帰時のボタン状態表示
        if ( p_RTM(0).p_mltcp.IsPosOrderOrigin ) <> 0 Then
            ButtonFlash(btnOrigin, Color.Yellow, Color.Transparent)
        Else
            btnOrigin.BackColor = Color.Transparent
        End If
        'メンテナンス情報の表示
        Dim s As String = nothing
        lblDevPowOnTime.Text				= (p_RTM(0).p_mltcp.IntegTimePowOn / 3600).ToString("F0")           '本体電源積算ON時間
        lblDevHeatOnTime.Text				= (p_RTM(0).p_mltcp.IntegTimeHeatOn / 3600).ToString("F0")          'HPヒーター積算ON時間
        lblXActTime.Text					= (p_RTM(0).p_mltcp.IntegTimeXAct / 3600).ToString("F0")			'X動作積算ON時間
        lblYActTime.Text					= (p_RTM(0).p_mltcp.IntegTimeYAct / 3600).ToString("F0")			'Y動作積算ON時間
        lblZActTime.Text					= (p_RTM(0).p_mltcp.IntegTimeZAct / 3600).ToString("F0")			'Z動作積算ON時間
        lblProbeHeatOnTime.Text				= (p_RTM(0).p_mltcp.IntegTimePBHeat / 3600).ToString("F0")          'PBヒーター積算ON時間
        lblSGHeatOnTime.Text				= (p_RTM(0).p_mltcp.IntegTimeSGHeat / 3600).ToString("F0")          'SGヒーター積算ON時間

        lblPowOnResetDate.Text				= IntToDateStringConv(p_RTM(0).p_mltcp.IntegDatePowOn)				'本体電源積算時間リセット日
        lblHeaterOnResetDate.Text			= IntToDateStringConv(p_RTM(0).p_mltcp.IntegDateHeatOn)				'HPヒータ積算時間リセット日
        lblXYActResetDate.Text				= IntToDateStringConv(p_RTM(0).p_mltcp.IntegDateXAct)				'X動作積算時間リセット日
        lblProbeHeaterOnResetDate.Text		= IntToDateStringConv(p_RTM(0).p_mltcp.IntegDatePbHeat)				'HPヒータ積算時間リセット日
        lblSGHeaterOnResetDate.Text			= IntToDateStringConv(p_RTM(0).p_mltcp.IntegDateSGHeat)				'SGヒータ積算時間リセット日

		'XYZ上下限リミット検知表示
		nudHeatPos.BackColor				= If(p_RTM(0).p_mltcp.isZUpperDet Or p_RTM(0).p_mltcp.isZLowerDet, Color.LightYellow, Color.White)
		nudXAxisPos.BackColor				= If(p_RTM(0).p_mltcp.isXUpperDet Or p_RTM(0).p_mltcp.isXLowerDet, Color.LightYellow, Color.White)
		nudYAxisPos.BackColor				= If(p_RTM(0).p_mltcp.isYUpperDet Or p_RTM(0).p_mltcp.isYLowerDet, Color.LightYellow, Color.White)


		'測定GAPキャリブレーション関係の表示
		Select Case p_cts.p_ZCalibTempPro
		Case -1
			btnZPosCalib.BackColor	= Control.DefaultBackColor
			btnZPosCalib.Text		= m_sStr(eMes.GAPCALIB_START)
			lblRemTimeVal.Text = "--"
			lblTempPreVal.Text = "--"
			lblPosDeviVal.Text = "--"
			'開始ボタン有効無効
			If (p_RTM(0).p_iHeatRdy = 3) And (p_RTM(0).p_mltcp.isStatHeaterOn = True) And (p_RTM(0).p_mltcp.isStatHeaterOn2 = True) And iRtgEditSave <> 1
				btnZPosCalib.Enabled = True
			Else
				btnZPosCalib.Enabled = False
			End If
			'操作禁止コントロール
			gbxTempProfile.Enabled	= True
			btnOK.Enabled			= True
			btnCancel.Enabled		= True
			btnApply.Enabled		= True

			'制御モード 1->-1変更 キャリブレーション終了
			If iPreZCalibTempPro <> p_cts.p_ZCalibTempPro
				If lvRtgset.SelectedItems.Count<>0
					Dim bCalibOK As Boolean
					fcDisplayRTGSETContents(lvRtgset.SelectedItems(0).Text, bCalibOK)		'ファイル読込後内容表示
					sbDispCalibOK( bCalibOK )												'キャリブ済表示
				End If
			End If


		Case Else
			btnZPosCalib.BackColor = Color.Yellow
			btnZPosCalib.Text		= m_sStr(eMes.GAPCALIB_CANSEL)
			'残り時間
			lblRemTimeVal.Text = p_RTM(0).p_rtgset.dTTotal - p_RTM(0).p_rtgset.iElaspeTime
			'現在温度設定
			lblTempPreVal.Text = p_RTM(0).p_rtgset.dCurrPreTemp.ToString("F1")
			'現在位置差
			lblPosDeviVal.Text = p_RTM(0).p_rtgset.dCurrPosDevi.ToString()
			'中止ボタン有効
			btnZPosCalib.Enabled = True
			'操作禁止コントロール
			gbxTempProfile.Enabled	= False
			btnOK.Enabled			= False
			btnCancel.Enabled		= False
			btnApply.Enabled		= False

		End Select

		iPreZCalibTempPro = p_cts.p_ZCalibTempPro

    Catch ex As Exception
        Call FrmDebugLog(System.Reflection.MethodBase.GetCurrentMethod.Name & ex.Message)
        Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
    End Try

    timMain.Enabled = True
End Sub
Dim iPreZCalibTempPro As Int16		'温度プロファイル制御モード前回値
''' <summary>
''' ボタン背景色点滅 500mS
''' </summary>
''' <param name="oBtn">ボタン名</param>
''' <param name="cOnColor">オン時色</param>
''' <param name="cOffColor">オフ時色</param>
Private sub ButtonFlash(byref oBtn As Button, byval cOnColor As Color, ByVal cOffColor As Color)
    If Fix((Now.Millisecond / 1000) + 0.5) >= 1 Then
        oBtn.BackColor = cOnColor
    Else
        oBtn.BackColor = cOffColor
    End If
End sub
'■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
'   ボタンイベント
'■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■

'詳細設定のOK、適用まとめ
Private Sub btnApply_Click(sender As Object, e As EventArgs) Handles btnApply.Click, btnOK.Click
Try
    if ApplySettings() = 0
	    p_bDlgApply = True
		If sender.Name = "btnOK"
			Me.DialogResult = DialogResult.OK

		End If
	Else
	    Call WriteLog("詳細設定の適用でエラー発生", eLOG_LEVEL.iFATAL)
    End If
Catch ex As Exception
    Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
    Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub
#If Not NEW_SPEC
' 固定SPEC保存ボタン
Private Sub btnSaveSpec_Click(sender As Object, e As EventArgs) Handles btnSaveSpec.Click
    Try
        sbCheckAndSaveFixSpec(True)

        If lvSpec.Items.Count > 0 AndAlso lvSpec.Items.ContainsKey(txtSpecName.Text) = True Then
            lvSpec.Select()
            lvSpec.Items(txtSpecName.Text).Selected = True
        End If

    Catch ex As Exception
        Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
        Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
    End Try
End Sub

' 選択された固定SPECを削除する。
Private Sub btnDeleteSpec_Click(sender As Object, e As EventArgs) Handles btnDeleteSpec.Click
    Try
        If lvSpec.SelectedItems.Count > 0 Then
            If lvSpec.SelectedItems(0).Text <> "" Then
                If MsgBox(m_sStr(eMes.iDEL_SPEC), _
                            MsgBoxStyle.OkCancel Or MsgBoxStyle.Question, "SPEC DELETE") = MsgBoxResult.Cancel Then
                    Exit Sub
                End If

                For i As Integer = 0 To lvSpec.SelectedItems.Count - 1
                    fcDeleteSPEC(lvSpec.SelectedItems(i).Text)
                Next i
                txtSpecName.Text = ""
                fcDisplaySPECList()
            End If
        End If

    Catch ex As Exception
        Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
        Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
    End Try
End Sub
#end if

' 選択されたRTGSETを削除する。
Private Sub btnDeleteRtgs_Click(sender As Object, e As EventArgs) Handles btnDeleteRtgs.Click
    Try
		If ShowAlartCalibClear()		'Z位置キャリブレーションクリア確認選択
			If lvRtgset.SelectedItems.Count > 0 Then
				For i As Integer = 0 To lvRtgset.SelectedItems.Count - 1
					If lvRtgset.SelectedItems(i).Text <> "" Then
						fcDeleteRTGSET(lvRtgset.SelectedItems(i).Text)
						Call WriteLog("温度プロファイル 削除されました。- " & lvRtgset.SelectedItems(i).Text, eLOG_LEVEL.iINFO)
					End If
				Next i

				txtRtgsetName.Text = ""
				fcDisplayRTGSETList()
			End If
		End If
    Catch ex As Exception
        Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
        Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
    End Try
End Sub

' RTGSET保存ボタン
Private Sub btnSaveRtgs_Click(sender As Object, e As EventArgs) Handles btnSaveRtgs.Click
Dim sName As String
Try
    '一行も設定されていないときはエラー
    If "" = GetStr(dgvRtgSetting.Item(RS_COL.TIME, 0).Value)
		Exit Sub
    End If
	If ShowAlartCalibClear()				'確認メッセージ選択
		sName = txtRtgsetName.Text
		'保存前にキャリブデータ部を削除
		fcSaveRTGSETContents(txtRtgsetName.Text, 4)

		sbCheckAndSaveRTGSet(True)
		If iRtgEditSave = 1 
		'編集済
			iRtgEditSave = 2		'保存完了
			btnSaveRtgs.Enabled = False
		End If
		txtRtgsetName.Text = sName
		Call WriteLog("温度プロファイル 保存されました。- " & txtRtgsetName.Text, eLOG_LEVEL.iINFO)
	End If

Catch ex As Exception
    Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
    Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub
''' <summary>
''' Z位置キャリブレーションクリア確認メッセージ選択
''' </summary>
''' <returns></returns>
Private Function ShowAlartCalibClear() As Boolean
	Dim bRet As Boolean
	Dim fMsg As New frmMSG(m_sStr(eMes.GAPCALIB_CLR_MSG), {m_sStr(eMes.MSG_YES),m_sStr(eMes.MSG_NO)}, "Operation guide", Me, 1)
	fMsg.ShowDialog()
	Select Case fMsg.p_iRes
	Case 1
		bRet = True
	Case Else
		bRet = False
	End Select
	fMsg.Dispose()
	Return bRet
End Function
' 温度波形のプレビューグラフ画面を表示。
Private Sub btnTempGraph_Click(sender As Object, e As EventArgs) Handles btnTempGraph.Click
    Dim dTIMEx As Double() = New Double(m_iRTGSETMaxRow * 2 + 2) {}
    Dim dTEMPy As Double() = New Double(m_iRTGSETMaxRow * 2 + 2) {}
    Dim dTEMPpy As Double = 0.0R
    Dim dTEMPcy As Double = 0.0R

    Dim dNextx As Double
    Dim dPrevx As Double = 0.0R
    Dim dVal As Double = 0.0R

    Try
        Dim iCnt As Integer = 0
        Dim iRow As Integer = 0
        '温度の初期値を設定
        Double.TryParse(dgvRtgSetting.Item(RS_COL.TEMPHP, iRow).Value, dVal)
        dTEMPpy = dVal
 
        While iRow < dgvRtgSetting.RowCount
            FixDGVNothing(iRow)
            Double.TryParse(dgvRtgSetting.Item(RS_COL.TIME, iRow).Value, dNextx)
            If dNextx <= dPrevx Then
                Exit While
            End If
            dPrevx = dNextx
            If dgvRtgSetting.Item(RS_COL.LINEAR, iRow).Tag = True AndAlso iCnt <> 0 Then
                ' 線形
                Double.TryParse(dgvRtgSetting.Item(RS_COL.TIME, iRow).Value, dTIMEx(iCnt))
                Double.TryParse(dgvRtgSetting.Item(RS_COL.TEMPHP, iRow).Value, dVal)
                dTEMPy(iCnt) = dVal
                dTEMPcy = dTEMPy(iCnt)
                dTEMPpy = dTEMPcy

                iCnt += 1
            Else
                ' 非線形
                Double.TryParse(dgvRtgSetting.Item(RS_COL.TIME, iRow).Value, dTIMEx(iCnt))
                dTEMPy(iCnt) = dTEMPpy
                iCnt += 1

                Double.TryParse(dgvRtgSetting.Item(RS_COL.TIME, iRow).Value, dTIMEx(iCnt))
                Double.TryParse(dgvRtgSetting.Item(RS_COL.TEMPHP, iRow).Value, dVal)
                dTEMPy(iCnt) = dVal
                dTEMPcy = dTEMPy(iCnt)
                dTEMPpy = dTEMPcy

                iCnt += 1
            End If
            iRow += 1
        End While

        If iRow <= 0 Then
            Exit Sub
        End If

		'温度プロファイルイメージ表示
        Dim ftg As frmTempGraph = New frmTempGraph() With{ .Size = New Drawing.Size(600,600) }		'Windowサイズ指定
		ftg.picBoxSize = New Drawing.Size(600, 500)													'PictureBoxサイズ指定
        'ftg.DispLine(txtRtgsetName.Text, dTIMEx.Take(iCnt).ToArray(), dTEMPy.Take(iCnt).ToArray())
#If 1
		'0秒からのプロファイルイメージを表示する
		'X軸データを編集(先頭追加）
		Dim x0() As Double = Nothing
		Dim X1() As Double = Nothing
		Array.Resize(x0, 1+iCnt)
		x0(0) = 0						'0秒
		Array.Resize(x1, iCnt)
		Array.Copy(dTIMEx.Take(iCnt).ToArray(),X1,X1.Length)
		Array.Copy(x1, 0, x0, 1, iCnt)

		'Y軸データを編集(先頭追加,先頭温度コピー）
		Dim y0() As Double = Nothing
		Dim y1() As Double = Nothing
		Array.Resize(y0, 1+iCnt)
		Array.Resize(y1, iCnt)
		y0(0) = dTEMPy(0)					'0秒時の温度
		Array.Copy(dTEMPy.Take(iCnt).ToArray(),y1,y1.Length)
		Array.Copy(dTEMPy.Take(iCnt).ToArray(),0,y0,1,y1.Length)
        ftg.DispLine(txtRtgsetName.Text, x0, y0)			'イメージ描画
#end if
        ftg.ShowDialog()									'ウィンドウ表示

        ftg.Dispose()


    Catch ex As Exception
        Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
        Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
    End Try
End Sub

' 温度設定表に定期変化設定を自動で追加。
Private Sub btnTempRepeat_Click(sender As Object, e As EventArgs) Handles btnTempRepeat.Click
    Try
        Dim dNew As Double = -1
        Dim dNewPre As Double = -1
        Dim dNew2 As Double = -1
        Dim dNewPre2 As Double = -1

        Dim iCnt As Integer = 1
        Dim iRow As Integer = 0

        For iCnt = dgvRtgSetting.RowCount - 1 To 0 Step -1
            '最新値を取得
            If dNew < 0 Then
                '時間入力が数値の場合
                If IsNumeric(dgvRtgSetting(RS_COL.TIME, iCnt).Value) = True Then
                    dNew = dgvRtgSetting(RS_COL.TIME, iCnt).Value
                    '温度を取得
                    If IsNumeric(dgvRtgSetting(RS_COL.TEMPHP, iCnt).Value) = True Then
                        dNew2 = dgvRtgSetting(RS_COL.TEMPHP, iCnt).Value
                    End If

                    '最新行の保持
                    iRow = iCnt

                    '最大行を超える場合、リピート不可
                    If iRow >= m_iRTGSETMaxRow - 1 Then
                        Exit Sub
                    End If
                End If
            Else
                '最新値の前の時間を取得
                If IsNumeric(dgvRtgSetting(RS_COL.TIME, iCnt).Value) = True Then
                    dNewPre = dgvRtgSetting(RS_COL.TIME, iCnt).Value
                    '温度を取得
                    If IsNumeric(dgvRtgSetting(RS_COL.TEMPHP, iCnt).Value) = True Then
                        dNewPre2 = dgvRtgSetting(RS_COL.TEMPHP, iCnt).Value
                    End If

                    'For文を抜ける
                    Exit For
                End If
            End If
        Next

        '時間の差が取得できた
        If dNew >= 0 AndAlso dNewPre >= 0 AndAlso dNew > dNewPre Then
            '次の時間を更新
            dNew = dNew + (dNew - dNewPre)

            If dNew2 >= 0 AndAlso dNewPre2 >= 0 Then
                '次の温度を更新
                dNew2 = dNew2 + (dNew2 - dNewPre2)

                '温度の最大、最小値を取得
                Dim dMinMax(1) As Double
                ''GetParamMinMaxDigit(DefSPEC.TEMPERATURE, TRQ_FACT_PERCENT, dMinMax)
				dMinMax(0) = 0.0
				dMinMax(1) = 300.0

                '範囲外の場合、上限下限にあわせる
                If dNew2 < dMinMax(0) Then
                    dNew2 = dMinMax(0)
                ElseIf dNew2 > dMinMax(1) Then
                    dNew2 = dMinMax(1)
                End If
            End If
        End If


        '行を挿入
        Dim newRow As DataGridViewRow = New DataGridViewRow()
        newRow.CreateCells(dgvRtgSetting)
        newRow.Cells(RS_COL.LINEAR).Value = dgvRtgSetting.Item(RS_COL.LINEAR, iRow).Value
        newRow.Cells(RS_COL.TIME).Value = dNew
        newRow.Cells(RS_COL.TEMPHP).Value = dNew2
        newRow.Cells(RS_COL.SPD).Value = dgvRtgSetting.Item(RS_COL.SPD, iRow).Value
        dgvRtgSetting.Rows.Insert(iRow + 1, newRow)

    Catch ex As Exception
        Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
        Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
    End Try
End Sub


'固定スペックフィルター種別選択ボタンクリックイベント
'Private Sub btnFixFilter_Click(sender As Object, e As EventArgs) Handles btnFixFilter.Click
'    Try
'        'フィルター種別の変更
'        btnFixFilter.Tag = btnFixFilter.Tag + 1

'        '最大値を超える場合、初期値に戻す
'        If btnFixFilter.Tag >= eFIX_FILTER.iMAX Then
'            btnFixFilter.Tag = 0
'        End If

'        '表示の変更
'        Select Case btnFixFilter.Tag
'            Case eFIX_FILTER.iPREFIX
'                btnFixFilter.Text = "A->"
'            Case eFIX_FILTER.iPARTIAL
'                btnFixFilter.Text = "<-A->"
'        End Select

'        'フィルター適用
'        sbApplyFixSpecFilter()

'    Catch ex As Exception
'        Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
'        Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
'    End Try
'End Sub

'Private Sub btnSelStrgFld_Click(sender As Object, e As EventArgs) Handles btnSelStrgFld.Click
'    Try
'        Dim Result As DialogResult
'        Dim sFld As String
'        Dim fldDlg As New FolderBrowserDialog

'        fldDlg.SelectedPath = ""

'        Me.Cursor = Cursors.WaitCursor

'        'ﾀﾞｲｱﾛｸﾞの表示(前回選択値がある時)
'        If txtStrgFld.Text.Length > 0 Then
'            fldDlg.SelectedPath = txtStrgFld.Text
'        End If

'        'ﾀﾞｲｱﾛｸﾞの表示
'        Result = fldDlg.ShowDialog()
'        sFld = fldDlg.SelectedPath

'        If Result = DialogResult.OK Then
'            'ﾊﾟｽの表示
'            txtStrgFld.Text = sFld
'        End If

'        Me.Cursor = Cursors.Default

'    Catch ex As Exception
'        Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
'        Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
'    End Try
'End Sub

'Private Sub btnSelStrgFldMeasData_Click(sender As Object, e As EventArgs) Handles btnSelStrgFldMeasData.Click
'    Try
'        Dim Result As DialogResult
'        Dim sFld As String
'        Dim fldDlg As New FolderBrowserDialog

'        fldDlg.SelectedPath = ""

'        Me.Cursor = Cursors.WaitCursor

'        'ﾀﾞｲｱﾛｸﾞの表示(前回選択値がある時)
'        If txtStrgFldMeasData.Text.Length > 0 Then
'            fldDlg.SelectedPath = txtStrgFldMeasData.Text
'        End If

'        'ﾀﾞｲｱﾛｸﾞの表示
'        Result = fldDlg.ShowDialog()
'        sFld = fldDlg.SelectedPath

'        If Result = DialogResult.OK Then
'            'ﾊﾟｽの表示
'            txtStrgFldMeasData.Text = sFld
'        End If

'        Me.Cursor = Cursors.Default

'    Catch ex As Exception
'        Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
'        Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
'    End Try
'End Sub
'温度プロファイル[Import]ボタンクリックイベント
Private Sub btnRtgsImp_Click(sender As Object, e As EventArgs) Handles btnRtgsImp.Click
    Static Dim sRtgsPath As String
    Try
        'インポート元ファイルパス取得
        Dim sPath As String
        sPath = OpenFileDialog(sRtgsPath, "CSV File(*.csv)|*.csv")

        'ファイルパスが指定された場合
        If sPath.Length > 0 Then
            sRtgsPath = sPath

            '温度プロファイルデータを取得
            Dim uTempProfile() As tpTempProfData
            uTempProfile = sbImportTempProf(sPath)

            'スペックデータが取得できた場合
            If uTempProfile.Length > 0 Then
                'データをIniファイルに書き込み
                If fcSaveTempProfData(uTempProfile) = True Then
                    MsgBox(m_sStr(eMes.iIMPORT_COMP), MsgBoxStyle.OkOnly Or MsgBoxStyle.Information, "Import")

                    Dim iSelIndex As Integer = 0
                    If lvRtgset.SelectedItems.Count > 0 Then
                        iSelIndex = lvRtgset.SelectedItems(0).Index
                    End If

                    fcDisplayRTGSETList()

                    lvRtgset.Select()
                    lvRtgset.Items(iSelIndex).Selected = True
                End If
            End If
        End If

    Catch ex As Exception
        Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
        Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
        MsgBox(m_sStr(eMes.iEXPORT_FAIL) & " : " & ex.Message, MsgBoxStyle.OkOnly Or MsgBoxStyle.Exclamation, "Export")
    End Try
End Sub

'温度プロファイル[Export]ボタンクリックイベント
Private Sub btnRtgsExp_Click(sender As Object, e As EventArgs) Handles btnRtgsExp.Click
    Static Dim sRtgsPath As String
    Try
        '出力先ファイルパス取得
        Dim sPath As String
        sPath = SaveFileDialog(sRtgsPath, "CSV File(*.csv)|*.csv")

        'ファイルパスが指定された場合
        If sPath.Length > 0 Then
            sRtgsPath = sPath

            '温度プロファイルデータを取得
            Dim uTempProfile() As tpTempProfData
            uTempProfile = fnGetIniTempProfData()

            'スペックデータが取得できた場合
            If uTempProfile.Length > 0 Then
                'データ書き込み
                If sbExportTempProf(sPath, uTempProfile) = True Then
                    MsgBox(m_sStr(eMes.iEXPORT_COMP), MsgBoxStyle.OkOnly Or MsgBoxStyle.Information, "Export")
                Else
                    MsgBox(m_sStr(eMes.iEXPORT_FAIL), MsgBoxStyle.OkOnly Or MsgBoxStyle.Exclamation, "Export")
                End If
            End If
        End If

    Catch ex As Exception
        Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
        Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
        MsgBox(m_sStr(eMes.iEXPORT_FAIL), MsgBoxStyle.OkOnly Or MsgBoxStyle.Exclamation, "Export")
    End Try
End Sub
'温度プロファイル[Clear]ボタンクリックイベント
Private Sub btnTempClear_Click(sender As Object, e As EventArgs) Handles btnTempClear.Click
    Try
        dgvRtgSetting.Rows.Clear()
		iRtgEditSave = 1				'編集済
		sbDispCalibOK(False)			'キャリブレーション済み消す
    Catch ex As Exception
        Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
        Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
    End Try
End Sub
''' <summary>
''' リニア条件を入力した作成支援
'''   開始温度、終了温度、待ち時間、昇温速度(℃/S)
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub btnRTGSpeed_Click(sender As Object, e As EventArgs) Handles btnRTGLinearTool.Click
Dim iRet As int32 = 0
Try
	If dgvRtgSetting.Rows.Count >= 2
		m_ctsMainFrm.m_MLinerInfo(0).iLinear	= If(dgvRtgSetting.Rows(0).Cells(0).Tag, 1, 0)
		m_ctsMainFrm.m_MLinerInfo(0).dTime		= dgvRtgSetting.Rows(0).Cells(1).Value
		m_ctsMainFrm.m_MLinerInfo(0).dTemp		= dgvRtgSetting.Rows(0).Cells(2).Value
		m_ctsMainFrm.m_MLinerInfo(1).iLinear	= If(dgvRtgSetting.Rows(1).Cells(0).Tag, 1, 0)
		m_ctsMainFrm.m_MLinerInfo(1).dTime		= dgvRtgSetting.Rows(1).Cells(1).Value
		m_ctsMainFrm.m_MLinerInfo(1).dTemp		= dgvRtgSetting.Rows(1).Cells(2).Value
	Else
		m_ctsMainFrm.m_MLinerInfo(0).iLinear	= 0
		m_ctsMainFrm.m_MLinerInfo(0).dTime		= 0
		m_ctsMainFrm.m_MLinerInfo(0).dTemp		= 0
		m_ctsMainFrm.m_MLinerInfo(1).iLinear	= 1
		m_ctsMainFrm.m_MLinerInfo(1).dTime		= 0
		m_ctsMainFrm.m_MLinerInfo(1).dTemp		= 0
	End If
	iRet = frmTempLinearTool.ShowDialog()				'リニア昇温入力支援
	Select Case iRet
	Case DialogResult.OK
		dgvRtgSetting.Rows.Clear
		dgvRtgSetting.Rows.Add
		dgvRtgSetting.Rows.Add
		dgvRtgSetting.Rows(0).Cells(0).Tag		= If(m_ctsMainFrm.m_MLinerInfo(0).iLinear=1, True, False)
        dgvRtgSetting.Rows(0).Cells(0).Value	= If(m_ctsMainFrm.m_MLinerInfo(0).iLinear=1, My.Resources.Checkbox, My.Resources.Uncheckbox)
		dgvRtgSetting.Rows(0).Cells(1).Value	= m_ctsMainFrm.m_MLinerInfo(0).dTime
		dgvRtgSetting.Rows(0).Cells(2).Value	= m_ctsMainFrm.m_MLinerInfo(0).dTemp
		dgvRtgSetting.Rows(1).Cells(0).Tag		= If(m_ctsMainFrm.m_MLinerInfo(1).iLinear=1, True, False)
        dgvRtgSetting.Rows(1).Cells(0).Value	= If(m_ctsMainFrm.m_MLinerInfo(1).iLinear=1, My.Resources.Checkbox, My.Resources.Uncheckbox)
		dgvRtgSetting.Rows(1).Cells(1).Value	= m_ctsMainFrm.m_MLinerInfo(1).dTime
		dgvRtgSetting.Rows(1).Cells(2).Value	= m_ctsMainFrm.m_MLinerInfo(1).dTemp
	Case DialogResult.Cancel
	End Select


Catch ex As Exception
    Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
    Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub


''' <summary>
''' 中心出し処理開始ボタン
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub btnOpnCalib_Click(sender As Object, e As EventArgs) Handles btnExecCenter.Click
Try
    Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name, eLOG_LEVEL.iINFO)
	If (p_RTM(0).p_mltcp.PosOrder And MemLinkTCP.eWPosOrderBit.iCENTER) <> 0 Then
		'動作中
		p_RTM(0).p_iBGWOrder = BGW_ORDER.CENTER_PROCESS_STOP_REP
	Else
		'停止中
		If p_RTM(0).p_mltcp.isILState()　Or p_RTM(0).p_mltcp.isILDisable			'IL(扉閉) OKか? IL無効も見る
			p_RTM(0).p_iBGWOrder = BGW_ORDER.CENTER_PROCESS_START_REP
		End if
	End If
Catch ex As Exception
    Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
    Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub
''' <summary>
''' 中心座標手動登録ボタン
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub btnCenterPosReg_Click(sender As Object, e As EventArgs) Handles btnCenterPosReg.Click
Try
    Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name, eLOG_LEVEL.iINFO)
    If p_RTM(0).p_iBGWOrder = BGW_ORDER.NO_ORDER
        '座標セット
		p_RTM(0).p_CenterXReg = nudCenterX.Value							
		p_RTM(0).p_CenterYReg = nudCenterY.Value
        '登録
        p_RTM(0).p_iBGWOrder = BGW_ORDER.CENTER_REGIST_START_REP
    End If
Catch ex As Exception
    Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
    Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub
'サンプル名リスト、測定者リストの削除ボタン
Private Sub btnMeasPlaceDel_Click(sender As Object, e As EventArgs) Handles btnMeasPlaceDel.Click, btnMeasOperDel.Click
    Try
        Dim cbx As ComboBox

        If sender Is btnMeasPlaceDel Then
            cbx = cbxMeasPlace
        Else
            cbx = cbxMeasOperator
        End If

        For i As Integer = 0 To cbx.Items.Count - 1
            If cbx.Items(i) = cbx.Text Then
                cbx.Items.RemoveAt(i)
                cbx.Text = ""
                Exit For
            End If
        Next i

    Catch ex As Exception
        Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
        Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
    End Try
End Sub

'■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
'   コントロールイベント
'■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■

Private Sub dgvEquipList_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvEquipList.CellClick
    Try
        If e.RowIndex >= 0 Then
            If e.ColumnIndex = 0 Then
                For i As Integer = 0 To 9
                    ' 通信対象チェックボックスの切り替え
                    If e.RowIndex = i Then
                        dgvEquipList.Item(EL_COL.USEEQUIP, i).Value = My.Resources.Checkbox
                        dgvEquipList.Item(EL_COL.USEEQUIP, i).Tag = True
                    Else
                        dgvEquipList.Item(EL_COL.USEEQUIP, i).Value = My.Resources.Uncheckbox
                        dgvEquipList.Item(EL_COL.USEEQUIP, i).Tag = False
                    End If
                Next i
            End If
            ''If e.ColumnIndex = 5 Then
            ''    For i As Integer = 0 To 9
            ''        If e.RowIndex = i Then
            ''            ' パスワード入力画面を表示
            ''            Dim dlgPass As frmFTPPass = New frmFTPPass()
            ''            dlgPass.UsernameTextBox.Text = m_strUserId(i)
            ''            dlgPass.PasswordTextBox.Text = m_strPassword(i)
            ''            dlgPass.ShowDialog()
            ''            ' 入力値を受け取る
            ''            m_strUserId(i) = Trim(dlgPass.UsernameTextBox.Text)
            ''            m_strPassword(i) = Trim(dlgPass.PasswordTextBox.Text)
            ''            dlgPass.Dispose()
            ''        End If
            ''    Next i
            ''End If
        End If

    Catch ex As Exception
        Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
        Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
    End Try
End Sub

Private Sub dgvEquipList_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvEquipList.CellDoubleClick
    Dim dCF() As Double = New Double(6) {}
    Dim dMCF() As Double = New Double(3) {}

    Try
        If e.RowIndex >= 0 Then
            ' 校正係数入力フォームを開く
            If e.ColumnIndex = EL_COL.MCF OrElse e.ColumnIndex = EL_COL.TCF Then
                ''dMCF = SplitMeasCalcFac(dgvEquipList.Item(EL_COL.MCF, e.RowIndex).Value.ToString())
                ''dCF = SplitCalcFac(dgvEquipList.Item(EL_COL.TCF, e.RowIndex).Value.ToString())
                ''Dim scdlg As New frmSetCalFac(dCF, dMCF)
                ''scdlg.AllowChangeMCF()
                ''If scdlg.ShowDialog() = Windows.Forms.DialogResult.OK Then
                ''    ' 複数選択されている場合、選択行全てにに同じ値を代入する
                ''    dMCF = scdlg.GetMCF()
                ''    dCF = scdlg.GetCF()
                ''    For Each cel As DataGridViewCell In dgvEquipList.SelectedCells
                ''        dgvEquipList.Item(3, cel.RowIndex).Value = CStr(dMCF(3)) + " -- " + CStr(dMCF(2)) + " -- " + CStr(dMCF(1))
                ''        dgvEquipList.Item(4, cel.RowIndex).Value = CStr(dCF(6)) + " -- " + CStr(dCF(5)) + " -- " + _
                ''            CStr(dCF(4)) + " -- " + CStr(dCF(3)) + " -- " + CStr(dCF(2)) + " -- " + CStr(dCF(1))
                ''    Next cel
                ''    scdlg.Dispose()
                ''End If
            End If
        End If

    Catch ex As Exception
        Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
        Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
    End Try
End Sub

' 指定された計算手法で必要な項目のみを表示する
Private Sub cboMethod_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboMethod.SelectedIndexChanged
    Dim dgvTemp As DataGridView = Nothing

    Try
        'For iTrq As Integer = 0 To eTRQ_UNIT.iMAX - 1
        '    Select Case iTrq
        '        Case eTRQ_UNIT.iPERCENT
        '            dgvTemp = dgvCalcOpt
        '        Case eTRQ_UNIT.iGFCM
        '        Case Else
        '            dgvTemp = dgvCalcOpt
        '    End Select

        '    With dgvTemp
        '        .Rows(eCALC_ROW.iMASK_RANGE).Visible = True
        '        .Rows(eCALC_ROW.iGEL_RANGE).Visible = True
        '        .Rows(eCALC_ROW.iRTG).Visible = True

        '        'スクロールを可視行の先頭に設定
        '        For iCnt As Integer = 0 To .RowCount - 1
        '            If .Rows(iCnt).Visible = True Then
        '                .FirstDisplayedScrollingRowIndex = iCnt
        '                Exit For
        '            End If
        '        Next
        '    End With
        'Next

    Catch ex As Exception
        Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
        Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
    End Try
End Sub
''' <summary>
''' 温度プロファイルリストアイテム変更
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub lvRtgset_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lvRtgset.SelectedIndexChanged
Dim bCalibOK As Boolean
Try
    If lvRtgset.SelectedItems.Count = 1 Then
        If lvRtgset.SelectedItems(0).Text <> "" Then
            fcDisplayRTGSETContents(lvRtgset.SelectedItems(0).Text, bCalibOK)		'ファイル読込後内容表示
			sbDispCalibOK( bCalibOK )												'キャリブ済表示
        End If
    End If
	iRtgEditSave = 0		'ファイル保存状態クリア
	btnSaveRtgs.Enabled = False
Catch ex As Exception
    Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
    Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub
Private Sub	sbDispCalibOK( ByVal bCalibOK As Boolean )
	Select Case bCalibOK
	Case True
		lblCalibOK.Text			= m_sStr(eMes.GAPCALIB_OK)
		lblCalibOK.BackColor	= Color.GreenYellow
	Case Else
		lblCalibOK.Text			= m_sStr(eMes.GAPCALIB_NY)
		lblCalibOK.BackColor = Color.Yellow
	End Select
End Sub

'編集モード終了イベント
Private Sub dgvRtgSetting_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs) Handles dgvRtgSetting.CellEndEdit
Try
	Select Case e.ColumnIndex
    Case RS_COL.TIME
    '列が時間の場合
        Dim dPrevTime As Double = -1
        Dim dCurTime As Double = 0.0R
        For iRow As Integer = 0 To dgvRtgSetting.RowCount - 1
            '空セルは空文字に置き換え
            FixDGVNothing(iRow)

            '数値に変換できる場合
            If IsNumeric(dgvRtgSetting.Item(RS_COL.TIME, iRow).Value) = True Then
                '数値に変換
                Double.TryParse(dgvRtgSetting.Item(RS_COL.TIME, iRow).Value, dCurTime)

                '前回時間より時間が遅れている場合(正の数)
                If dPrevTime >= dCurTime OrElse dCurTime <= 0.0R Then
                    dgvRtgSetting.Item(RS_COL.TIME, e.RowIndex).Style.BackColor = Color.Yellow
                Else
                    dgvRtgSetting.Item(RS_COL.TIME, e.RowIndex).Style.BackColor = Color.White
                End If
                dPrevTime = dCurTime
            End If
        Next iRow
	Case RS_COL.TEMPHP
	'温度 上下限チェック
		If dgvRtgSetting.Item(RS_COL.TEMPHP, e.RowIndex).Value >= 0 And dgvRtgSetting.Item(RS_COL.TEMPHP, e.RowIndex).Value <= p_cts.TempCnt_HUpperLimit
		Else
			dgvRtgSetting.Item(RS_COL.TEMPHP, e.RowIndex).Value = m_sPreTrmp
		End If
    End Select
	iRtgEditSave = 1		'編集済
	btnSaveRtgs.Enabled = True
	sbDispCalibOK(False)
Catch ex As Exception
    Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
    Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub
Dim m_sPreTrmp As String = ""			'温度設定前回値退避
Private Sub dgvRtgSetting_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvRtgSetting.CellClick
    Try
        If e.RowIndex >= 0 Then
			Select Case e.ColumnIndex
			Case RS_COL.LINEAR
                If dgvRtgSetting.Item(RS_COL.LINEAR, e.RowIndex).Tag = False Then
                    dgvRtgSetting.Item(RS_COL.LINEAR, e.RowIndex).Value = My.Resources.Checkbox
                    dgvRtgSetting.Item(RS_COL.LINEAR, e.RowIndex).Tag = True
                Else
                    dgvRtgSetting.Item(RS_COL.LINEAR, e.RowIndex).Value = My.Resources.Uncheckbox
                    dgvRtgSetting.Item(RS_COL.LINEAR, e.RowIndex).Tag = False
                End If
			Case RS_COL.TEMPHP
				m_sPreTrmp = dgvRtgSetting.Item(RS_COL.TEMPHP, e.RowIndex).Value
			End Select
        End If

    Catch ex As Exception
        Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
        Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
    End Try
End Sub

Private Sub txtPort_KeyDown(sender As Object, e As KeyEventArgs) Handles txtPort.KeyDown
    Dim kPass As Keys

    Try
        Select Case m_PassKey.Length
            Case 0, 1
                kPass = Keys.Up
            Case 2, 3
                kPass = Keys.Down
            Case 4, 6
                kPass = Keys.Left
            Case 5, 7
                kPass = Keys.Right
            Case 8
                kPass = Keys.B
            Case 9
                kPass = Keys.A
            Case Else
                m_PassKey = ""
                Exit Sub
        End Select

        If e.KeyCode = kPass Then
            m_PassKey = m_PassKey & "*"
            e.SuppressKeyPress = True
        Else
            m_PassKey = ""
        End If

        ''If m_PassKey.Length = CStr("↑↑↓↓←→←→BA").Length Then
        ''    m_PassKey = ""
        ''    frmDebug.Show()

        ''    '[Cancel]ボタンをクリック
        ''    btnCancel.PerformClick()
        ''End If

    Catch ex As Exception
        Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
        Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
    End Try
End Sub

'通信設定DataGrid入力検証
Private Sub dgvEquipList_CellValidating(sender As Object, e As DataGridViewCellValidatingEventArgs) Handles dgvEquipList.CellValidating
Try
	Select Case e.ColumnIndex
	Case EL_COL.IP
		'IPアドレスの入力をチェック
		If Not System.Net.IPAddress.TryParse(e.FormattedValue, Nothing)
			If e.FormattedValue.ToString = ""
				'空文字は削除したとみなしてクリア
				Dim iRow As Integer = e.RowIndex
				dgvEquipList.Item(EL_COL.NAME, iRow).Value		= ""
				dgvEquipList.Item(EL_COL.XYSENS, iRow).Value	= ""
				dgvEquipList.Item(EL_COL.ZSENS, iRow).Value		= ""

				dgvEquipList.Item(EL_COL.MCF, iRow).Value		= "0 -- 0 -- 1"
				dgvEquipList.Item(EL_COL.TCF, iRow).Value =		"0 -- 0 -- 0 -- 0 -- 0 -- 1"
			Else
				'IPアドレスでない場合、書き戻す
				dgvEquipList.CancelEdit()
			End If
		End If
	End Select
Catch ex As Exception
    Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
    Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub
Dim sPreEditVal(2) As String
Private Sub dgvEquipList_CellBeginEdit(sender As Object, e As DataGridViewCellCancelEventArgs) Handles dgvEquipList.CellBeginEdit
Try
	Select Case e.ColumnIndex
	Case EL_COL.IP
	Case EL_COL.NAME
		sPreEditVal(0) = dgvEquipList.Item(EL_COL.NAME, e.RowIndex).Value
	Case EL_COL.XYSENS
		sPreEditVal(1) = dgvEquipList.Item(EL_COL.XYSENS, e.RowIndex).Value
	Case EL_COL.ZSENS
		sPreEditVal(2) = dgvEquipList.Item(EL_COL.ZSENS, e.RowIndex).Value
	End Select
Catch ex As Exception
    Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
    Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub
Private Sub dgvEquipList_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs) Handles dgvEquipList.CellEndEdit
Try
	Select Case e.ColumnIndex
	Case EL_COL.IP
	Case EL_COL.NAME
		If Not CheckMcnID( dgvEquipList.Item(EL_COL.NAME, e.RowIndex).Value )
			dgvEquipList.Item(EL_COL.NAME, e.RowIndex).Value = sPreEditVal(0)
		End If
	Case EL_COL.XYSENS
		If Not CheckSensor( 0, dgvEquipList.Item(EL_COL.XYSENS, e.RowIndex).Value )
			dgvEquipList.Item(EL_COL.XYSENS, e.RowIndex).Value = sPreEditVal(1)
		End If
	Case EL_COL.ZSENS
		If Not CheckSensor( 1, dgvEquipList.Item(EL_COL.ZSENS, e.RowIndex).Value )
			dgvEquipList.Item(EL_COL.ZSENS, e.RowIndex).Value = sPreEditVal(2)
		End If
	End Select
Catch ex As Exception
    Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
    Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub
''' <summary>
''' 本体番号の入力チェック
''' </summary>
''' <param name="sMcn">チェックする本体ID文字列</param>
''' <returns>True:OK
'''			 False:NG
''' </returns>
Private Function CheckMcnID(ByVal sMcn As String) As Boolean
	Dim bRet As Boolean = False
Try
	If Not IsNothing(sMcn)
		If sMcn.Length <> 6
		'文字列長
			bRet = False
		ElseIf sMcn.Substring(0,3) = "SZK"
		'識別子
			Dim iMcn As Integer = 0
			If Integer.TryParse(sMcn.Substring(3), iMcn)
			'連番部
				If iMcn>0 And iMcn<=999
					bRet = True
				End If
			End If
		End If
	End If
	Return bRet
Catch ex As Exception
    Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
    Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Return bRet
End Try
End Function
''' <summary>
''' センサー番号の入力チェック
''' </summary>
''' <param name="iType"></param>
''' <param name="sSens"></param>
''' <returns>True:OK
'''			 False:NG
''' </returns>
Private Function CheckSensor(ByVal iType As Int16, ByVal sSens As String) As Boolean
	Dim bRet As Boolean = False
Try
	If Not IsNothing(sSens)
		Select Case iType
		Case 0		'XYセンサ
			If sSens.Length <> 10
			'文字列長
				bRet = False
			ElseIf sSens.Substring(0,3) = "SZS" And sSens.Substring(3,1) = "-" And sSens.Substring(6,1) = "-" 
			'識別子
				Dim iSer0 As Integer = 0
				If Integer.TryParse(sSens.Substring(4,2), iSer0)
					'第1連番部
					If iSer0>=0 And iSer0<=89
						bRet = True
					Else
						bRet = False
						Exit Select
					End If
				End If
				Dim iSer1 As Integer = 0
				If Integer.TryParse(sSens.Substring(7), iSer1)
					'第2連番部
					If iSer1>=0 And iSer1<=999
						bRet = True
					Else
						bRet = False
						Exit Select
					End If
				End If
			End If
		Case 1		'Zセンサ
			If sSens.Length <> 10
			'文字列長
				bRet = False
			ElseIf sSens.Substring(0,3) = "SZS" And sSens.Substring(3,1) = "-" And sSens.Substring(6,1) = "-" 
			'識別子
				Dim iSer0 As Integer = 0
				If Integer.TryParse(sSens.Substring(4,2), iSer0)
					'第1連番部
					If iSer0>=90 And iSer0<=99
						bRet = True
					Else
						bRet = False
						Exit Select
					End If
				End If
				Dim iSer1 As Integer = 0
				If Integer.TryParse(sSens.Substring(7), iSer1)
					'第2連番部
					If iSer1>=0 And iSer1<=999
						bRet = True
					Else
						bRet = False
						Exit Select
					End If
				End If
			End If
		End Select
	End If
	Return bRet
Catch ex As Exception
    Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
    Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Return bRet
End Try
End Function
'ポート番号入力変更イベント
Private Sub txtPort_Leave(sender As Object, e As EventArgs) Handles txtPort.Leave
    Try
        'ポート番号が1024～49151の場合のみ有効
        If txtPort.Text.Length = 0 OrElse _
            IsNumeric(txtPort.Text) = False OrElse _
            CInt(txtPort.Text) < 1024 OrElse _
            CInt(txtPort.Text) > 49151 Then
            txtPort.Text = m_ctsMainFrm.strPort
        End If

    Catch ex As Exception
        Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
        Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
    End Try
End Sub
'MOD START M_UNO 20170104 Ver.--- 温度プロファイル判定処理追加
Private Sub dgvRtgSetting_CellValidating(sender As Object, e As DataGridViewCellValidatingEventArgs) Handles dgvRtgSetting.CellValidating
    Try
        '時間、温度は数値変換できる数値のみ受付可能
        If e.ColumnIndex = RS_COL.TIME OrElse e.ColumnIndex = RS_COL.TEMPHP Then
            If IsNothing(e.FormattedValue) = False AndAlso e.FormattedValue.ToString.Length > 0 Then
                If IsNumeric(e.FormattedValue) = False Then
                    dgvRtgSetting.CancelEdit()
                End If
            End If
        End If

    Catch ex As Exception
        Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
        Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
    End Try
End Sub
'ADD START M_UNO 20170105 Ver.--- SPEC編集にフィルタ機能追加
Private p_sAllFixSpecName As New List(Of String)

'フィルターテキスト変更イベント
Private Sub txtFixFilter_TextChanged(sender As Object, e As EventArgs) Handles txtFixFilter.TextChanged
    Try
        'フィルター適用
        sbApplyFixSpecFilter()

    Catch ex As Exception
        Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
        Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
    End Try
End Sub

'CellFormattingイベント
Private Sub dgvRtgSetting_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles dgvRtgSetting.CellFormatting
    Try
        Dim dgv As DataGridView = CType(sender, DataGridView)

        '"Image"列のセルか確認する
        If e.ColumnIndex = 0 AndAlso e.RowIndex >= 0 Then
            '"Column1"列のセルの値によって表示する画像を決定する

            If e.RowIndex >= 20 Then
                Console.WriteLine("")
            End If

            If IsNothing((dgv("Column1", e.RowIndex).Tag)) = True Then
                e.Value = My.Resources.Uncheckbox
                e.FormattingApplied = True
            End If
        End If

    Catch ex As Exception
        Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
        Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
    End Try
End Sub

'[Enable Auto Print]チェックボックスのチェック状態変更
Private Sub cbxAutPrnt_CheckedChanged(sender As Object, e As EventArgs) Handles cbxAutPrnt.CheckedChanged
    Try
        Dim blEna As Boolean = cbxAutPrnt.Checked

        '関連コントロールの有効／無効
        GroupBox20.Enabled = blEna          'File type
        txtStrgFld.Enabled = blEna          'Storage folder
        btnSelStrgFld.Enabled = blEna       'Storage folder選択ボタン

    Catch ex As Exception
        Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
        Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
    End Try
End Sub

'[Enable Auto Save of meas. data]チェックボックスのチェック状態変更
Private Sub chkAutSaveMeasData_CheckedChanged(sender As Object, e As EventArgs) Handles chkAutSaveMeasData.CheckedChanged
    Try
        Dim blEna As Boolean = chkAutSaveMeasData.Checked

        '関連コントロールの有効／無効
        txtStrgFldMeasData.Enabled = blEna          'Storage folder
        btnSelStrgFldMeasData.Enabled = blEna       'Storage folder選択ボタン

    Catch ex As Exception
        Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
        Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
    End Try
End Sub

'■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
'   パブリック関数
'■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■

''' <summary>
''' 詳細設定の内容を反映させる。
''' </summary>
Public Function ApplySettings() As int32
    'Dim sArry()		As String
	Dim sErrMsg		As String = Nothing
Try
	ScrToMem(sErrMsg)			'フォームコントロール値でクラス内容へ反映する
#If 0
    ' 表示言語
    If rbJPN.Checked Then
        m_ctsMainFrm.iLang = 1
    ElseIf rbTChinese.Checked Then
        m_ctsMainFrm.iLang = 4
    ElseIf rbKorean.Checked Then
        m_ctsMainFrm.iLang = 3
    ElseIf rbSChinese.Checked Then
        m_ctsMainFrm.iLang = 2
    Else
        m_ctsMainFrm.iLang = 0
    End If
    ' ポート番号
    m_ctsMainFrm.strPort = txtPort.Text.Replace(" ", "")
    ' グリッド用ペン
    m_ctsMainFrm.penGrids.Width = 1.0F
    If rbSolid.Checked Then
        m_ctsMainFrm.penGrids.DashStyle = Drawing2D.DashStyle.Solid
    ElseIf rbDash.Checked Then
        m_ctsMainFrm.penGrids.DashStyle = Drawing2D.DashStyle.Dash
    ElseIf rbDotGrids.Checked Then
        m_ctsMainFrm.penGrids.DashStyle = Drawing2D.DashStyle.Dot
    ElseIf rbNoGrids.Checked Then
        m_ctsMainFrm.penGrids.DashStyle = Drawing2D.DashStyle.Custom
    Else
        m_ctsMainFrm.penGrids.DashStyle = Drawing2D.DashStyle.Solid
    End If
    ' 負の値の表示
    If rbDisplayNgtv.Checked Then
        m_ctsMainFrm.iHideNgtv = 0
    ElseIf rbHideNgtv.Checked Then
        m_ctsMainFrm.iHideNgtv = 1
    Else
        m_ctsMainFrm.iHideNgtv = 0
    End If

	'時間間引き数
	m_ctsMainFrm.iDetailsR = nudThinningValueR.Value
	m_ctsMainFrm.iDetailsD = nudThinningValueD.Value

	'時間スクロール時の幅(秒)
	m_ctsMainFrm.iScrlWidth = nudScrolWidth.Value

	'マーカスタイル
	m_ctsMainFrm.iMarkerStyle = cbxMarkerStyle.SelectedIndex
	'マーカーサイズ
	m_ctsMainFrm.iMarkerSize = nudMarkerSize.Value

    ' 近似範囲表示方法
    If rbARDFill.Checked Then
        m_ctsMainFrm.iARDisp = 0
    Else
        m_ctsMainFrm.iARDisp = 1
    End If
    ' リアルタイムグラフX軸
    If rbRXauto.Checked Then
        m_ctsMainFrm.iRTXAxis = 0
    Else
        m_ctsMainFrm.iRTXAxis = 1
    End If
    ' リアルタイムグラフY軸
    If rbRYauto.Checked Then
        m_ctsMainFrm.iRTYAxis = 0
    Else
        m_ctsMainFrm.iRTYAxis = 1
    End If


    'フィルタパラメータ
    m_ctsMainFrm.iFilterEnable = iif(cbxFilterEnable.Checked, 1, 0)
    Select Case rbFIR.Checked
    Case True
        m_ctsMainFrm.sFilterKind = "FIR"
    Case False
        m_ctsMainFrm.sFilterKind = "IIR"
    End Select
    m_ctsMainFrm.iFilterLen		= nudFilterLen.Value
    m_ctsMainFrm.dFirCutOffFreq = nudFIRCoffFreq.Value
    m_ctsMainFrm.sFilterWinKind = cbxWinKind.Text

    m_ctsMainFrm.dIIRCutOffFreq = nudIIRCoffFreq.Value
    m_ctsMainFrm.iIIRDeg = nudIIRdeg.Value

    m_ctsMainFrm.sFilterFileName = lblFilterFileName.Text
    m_ctsMainFrm.DBAutoReg		= iif(cbxAutoReg.Checked, 1, 0)
    m_ctsMainFrm.SPCEn			= iif(cbxSPC.Checked, 1, 0)
    m_ctsMainFrm.SPCCalcSample	= nudSPCCalcSample.Value						'SPC機能 計算サンプル数


	'GAP補正値
    m_ctsMainFrm.GapCorr		= nudGapCorr.Value

    ' 測定自動開始
    If cbAutoStart.Checked Then
        m_ctsMainFrm.iAutoStart = nudAutoStart.Value
    Else
        m_ctsMainFrm.iAutoStart = -1
    End If
    ' 管理者用オプション
    If cbCanDelete.Checked Then
        m_ctsMainFrm.iCanRemoveData = 0
    Else
        m_ctsMainFrm.iCanRemoveData = -1
    End If
    If cbModifyCF.Checked Then
        m_ctsMainFrm.iCanModifyCF = 0
    Else
        m_ctsMainFrm.iCanModifyCF = -1
    End If
    If cbModifyRecalc.Checked Then
        m_ctsMainFrm.iCanModifyRecalc = 0
    Else
        m_ctsMainFrm.iCanModifyRecalc = -1
    End If
    If cbModifySpec.Checked Then
        m_ctsMainFrm.iCanModifySpec = 0
    Else
        m_ctsMainFrm.iCanModifySpec = -1
    End If


    ' 通信一覧の記入状態を取得
    For iRow As Integer = 0 To dgvEquipList.RowCount - 1
        ' 通信機器の選択状態
        If dgvEquipList.Item(EL_COL.USEEQUIP, iRow).Tag = True Then
            m_ctsMainFrm.iSelMDK = iRow
        End If
        ' 機器毎のIPアドレスと名称、校正係数
        If dgvEquipList.Item(EL_COL.IP, iRow).Value Is Nothing Then
            m_ctsMainFrm.strIP(iRow) = ""
        Else
            m_ctsMainFrm.strIP(iRow) = dgvEquipList.Item(EL_COL.IP, iRow).Value.ToString.Replace(" ", "")
	        If m_ctsMainFrm.iSelMDK = iRow And m_ctsMainFrm.strIP(iRow) = ""
				'チェックされている機器にIPアドレスが入力されていない
				sErrMsg = m_sStr(eMes.INVALID_INPUT_IP)
	        End If
        End If
        m_ctsMainFrm.strEquipName(iRow)		= dgvEquipList.Item(EL_COL.NAME, iRow).Value
        m_ctsMainFrm.sXYSensorSerial(iRow)	= dgvEquipList.Item(EL_COL.XYSENS, iRow).Value
        m_ctsMainFrm.sZSensorSerial(iRow)	= dgvEquipList.Item(EL_COL.ZSENS, iRow).Value

        Dim dMCF() As Double = New Double(3) {}
        Dim dTCF() As Double = New Double(6) {}
    Next iRow


    ' FTPパスワード
    m_ctsMainFrm.strFTPUser = m_strUserId
    m_ctsMainFrm.strFTPPass = m_strPassword

    'ゼロ調
	'ゼロ調モード
	If rbZeroXcont.Checked
		m_ctsMainFrm.ZeroAdjMode = 0
	End If
	If rbZeroZcont.Checked
		m_ctsMainFrm.ZeroAdjMode = 1
	End If
    'ゼロ調条件
    Double.TryParse(nudZeroStartPosHP.Text, m_ctsMainFrm.dZeroStaPosHP)    '熱板開始位置(mm)
    int16.TryParse(nudStaMonTime.Text, m_ctsMainFrm.iStaMonTime)           '安定度監視時間(S)
    int16.TryParse(nudStaFlacRange.Text, m_ctsMainFrm.iStaFlucRange)       '安定度範囲(um)
    '中央出し条件
    Double.TryParse(nudCentStartPosHP.Text, m_ctsMainFrm.dCenterStaPosHP)  '中央出し熱板開始位置(mm)
    Int32.TryParse(lblCenterX.Text, m_ctsMainFrm.iCenterResultsX)          '中央出し結果X位置(um)
    Int32.TryParse(lblCenterY.Text, m_ctsMainFrm.iCenterResultsY)          '中央出し結果Y位置(um)

	'Z軸上限リミット位置
	m_ctsMainFrm.ZUpperLimitPos = cType(nudZUpperLimit.Value * 1000.0, int32)


    'カラー更新
    For iCnt As Integer = 0 To m_lblClr.Length - 1
        Select Case iCnt
            Case m_lblClr.Length - 2
                m_ctsMainFrm.clrGraphBack = m_lblClr(iCnt).BackColor
            Case m_lblClr.Length - 1
                m_ctsMainFrm.penGrids.Color = m_lblClr(iCnt).BackColor
            Case Else
                m_ctsMainFrm.clrY1Default(iCnt) = m_lblClr(iCnt).BackColor
        End Select
    Next
    'If cbY2Opp.Checked = False Then
    '    m_ctsMainFrm.bY2Opposite = False
    'Else
    '    m_ctsMainFrm.bY2Opposite = True
    'End If

    'Auto Printの有効/無効
    If cbxAutPrnt.Checked = True Then
        m_ctsMainFrm.iAutoPrintEnableValue = 1

        '保存先フォルダが作成できない場合
        If CreateFolder(txtStrgFld.Text) = False Then
            MsgBox(m_sStr(eMes.iAUTOPRINT_DISABLE), MsgBoxStyle.Exclamation Or MsgBoxStyle.OkOnly, "Auto Print")
            cbxAutPrnt.Checked = False
            m_ctsMainFrm.iAutoPrintEnableValue = 0
        Else
            m_ctsMainFrm.strAutoPrintOutPutFolderValue = txtStrgFld.Text
        End If
    Else
        m_ctsMainFrm.iAutoPrintEnableValue = 0
        m_ctsMainFrm.strAutoPrintOutPutFolderValue = txtStrgFld.Text
    End If

    '出力ファイルタイプ
    Select Case True
        Case rbtnJpg.Checked        'JPEG
            m_ctsMainFrm.iAutoPrintTypeValue = eAUTOPRINT_FILE.iJPG
        Case rbtnPdf.Checked        'PDF
            m_ctsMainFrm.iAutoPrintTypeValue = eAUTOPRINT_FILE.iPDF
    End Select

    '測定場所
    m_ctsMainFrm.strAutoPrintMesurePlaceValue = cbxMeasPlace.Text

    '測定場所リスト
    sArry = GetSortComboList(cbxMeasPlace, cbxMeasPlace.Text)
    m_ctsMainFrm.sAutoPrintMesurePlaceList = String.Join(DF_OPER_LIST_SEP, sArry)        '配列内のデータをすべて DF_OPER_LIST_SEP 区切りで連結

    '測定者
    m_ctsMainFrm.strAutoPrintOperatorValue = cbxMeasOperator.Text

    '測定者リスト
    sArry = GetSortComboList(cbxMeasOperator, cbxMeasOperator.Text)
    m_ctsMainFrm.sAutoPrintOperatorList = String.Join(DF_OPER_LIST_SEP, sArry)           '配列内のデータをすべて DF_OPER_LIST_SEP 区切りで連結

    'Auto Printのディスク容量
    m_ctsMainFrm.iAutoPrintDiskCapacityValue = nudDskSpc.Value

    '測定結果自動保存の有効/無効
    '測定結果自動保存の保存先フォルダ
    If chkAutSaveMeasData.Checked = True Then
        m_ctsMainFrm.iAutoSaveMeasDataEnable = 1

        '保存先フォルダが作成できない場合
        If CreateFolder(txtStrgFldMeasData.Text) = False Then
            MsgBox(m_sStr(eMes.iAUTOPRINT_DISABLE), MsgBoxStyle.Exclamation Or MsgBoxStyle.OkOnly, "Auto Print")
            chkAutSaveMeasData.Checked = False
            m_ctsMainFrm.iAutoSaveMeasDataEnable = 0
        Else
            m_ctsMainFrm.sAutoSaveMeasDataOutPutFolder = txtStrgFldMeasData.Text
        End If
    Else
        m_ctsMainFrm.iAutoSaveMeasDataEnable = 0
        m_ctsMainFrm.sAutoSaveMeasDataOutPutFolder = txtStrgFldMeasData.Text
    End If
    m_ctsMainFrm.iStaMonTime = nudStaMonTime.Value


    '固定スペックロット名手動入力
    Select Case cbxLotManu.Checked
        Case True
            m_ctsMainFrm.iFixSpecLotManu = 1
        Case Else
            m_ctsMainFrm.iFixSpecLotManu = 0
    End Select
    '本体電源ON時間の超過判定閾値
    m_ctsMainFrm.iMntChkIntegHourPowerOn	= nudMainteThr1.Value
    '熱版ヒータON時間の超過判定閾値
    m_ctsMainFrm.iMntChkIntegHourHeaterOn	= nudMainteThr2.Value
    'XYZ動作ON時間の超過判定閾値
    m_ctsMainFrm.iMntChkIntegHourXAct		= nudMainteThr3.Value
    'PBヒータON時間の超過判定閾値
    m_ctsMainFrm.iMntChkIntegHourHeaterOn2	= nudMainteThr4.Value
    'SGヒータON時間の超過判定閾値
    m_ctsMainFrm.iMntChkIntegHourHeaterOn3	= nudMainteThr5.Value

    '温度制御
    m_ctsMainFrm.TempCnt_P          = nudP.Value
    m_ctsMainFrm.TempCnt_I          = nudI.Value
    m_ctsMainFrm.TempCnt_D          = nudD.Value
    m_ctsMainFrm.TempCnt_Dgain      = nudDgain.Value
    m_ctsMainFrm.TempCnt_PVBias     = nudPVBias.Value
    m_ctsMainFrm.TempCnt_PVGain     = nudPVGain.Value
    m_ctsMainFrm.TempCnt_HUpperLimit= nudHUpperTemp.Value

    m_ctsMainFrm.TempCnt_P2          = nudP2.Value
    m_ctsMainFrm.TempCnt_I2          = nudI2.Value
    m_ctsMainFrm.TempCnt_D2          = nudD2.Value
    m_ctsMainFrm.TempCnt_Dgain2      = nudDgain2.Value
    m_ctsMainFrm.TempCnt_PVBias2     = nudPVBias2.Value
    m_ctsMainFrm.TempCnt_PVGain2     = nudPVGain2.Value
    m_ctsMainFrm.TempCnt_HUpperLimit2= nudHUpperTemp2.Value

    m_ctsMainFrm.TempCnt_P3          = nudP3.Value
    m_ctsMainFrm.TempCnt_I3          = nudI3.Value
    m_ctsMainFrm.TempCnt_D3          = nudD3.Value
    m_ctsMainFrm.TempCnt_Dgain3      = nudDgain3.Value
    m_ctsMainFrm.TempCnt_PVBias3     = nudPVBias3.Value
    m_ctsMainFrm.TempCnt_PVGain3     = nudPVGain3.Value
    m_ctsMainFrm.TempCnt_HUpperLimit3= nudHUpperTemp3.Value
    m_ctsMainFrm.TempCnt_HLowerLimit3= nudHLowerTemp3.Value
    m_ctsMainFrm.TempCnt_HDifSetTemp3= nudHDifSetTemp3.Value

    ''m_ctsMainFrm.TempLED = nudLEDTempObsrv.Value
    ''m_ctsMainFrm.TempLEDen = cbxTempLEDEn.Checked

    '測定制御
    m_ctsMainFrm.SampleInputWait		= nudSampleInputWait.Value
    m_ctsMainFrm.InputGap				= nudInputGap.Value
    m_ctsMainFrm.HPTempRange	        = nudHPTempRange.Value
    m_ctsMainFrm.PBTempRange	        = nudPBTempRange.Value
    m_ctsMainFrm.SGTempRange	        = nudSGTempRange.Value
    m_ctsMainFrm.SGStressStab	        = nudStrainStab.Value
    m_ctsMainFrm.PressContDelayTime		= nudPresContDeyayTime.Value
	m_ctsMainFrm.DoublePBUnlockConfirm	= ckbDoublePBUnlockConfirm.Checked
	m_ctsMainFrm.GreenFlashInMeas		= ckbGreenFlashInMeas.Checked


	'長時間間欠測定設定
	p_LTModeChange						= If(m_ctsMainFrm.LTModeEnable<>cbxLongMeasMode.Checked, True, False)
	m_ctsMainFrm.LTModeEnable			= cbxLongMeasMode.Checked			
    m_ctsMainFrm.LTMeasureInterval		= nudLTMeasInterval.Value
    m_ctsMainFrm.LTAdoptRate			= nudLTAdoptRate.Value


	'QRコード運用設定
	m_ctsMainFrm.QRModeEnable			= cbxQRCodeEnable.Checked			
    m_ctsMainFrm.QREcc					= cboECC.Text
    m_ctsMainFrm.QRLogoCells			= nudCellsInLogo.Value
    m_ctsMainFrm.QRPixelInCel			= nudPixelInCell.Value




    'ブザー制御
    m_ctsMainFrm.EndOfBuzzerEn		= cbxEndOfMesure.checked
	m_ctsMainFrm.EndOfBuzzerPattern = nudBZPattern.Value
    m_ctsMainFrm.BuzzerOkOfHeatEn	= cbxOkOfHeat.checked

    'IL無効
    m_ctsMainFrm.CoverILDis = cbxCoverILDis.checked

    '測定値領域読込許可
    m_ctsMainFrm.ReadRealAreaEn = cbxReadRealAreaEn.checked

    'スタンバイ時間
    m_ctsMainFrm.StanbyTime = nudStandbyTime.Value

	'多重起動有効無効
	m_ctsMainFrm.MultiActivate = If(cbxMultActEn.Checked, 1, 0)


    Dim sErr As String = ""
    ' スペックが編集中であれば保存(保存時警告なし)
#If Not NEW_SPEC
    sErr = sbCheckAndSaveFixSpec(False)
#end if
    'sErr = sErr & sbCheckAndSaveRTGSet(False)


    '応力検知関係
    m_ctsMainFrm.StresDetAcqTime		= nudStrsDetAcqTime.Value
    m_ctsMainFrm.StresDetADThreshold	= nudStrsDetADThreshold.Value
    m_ctsMainFrm.StresDetDuraTime		= nudStresDetDuraTime.Value

	'測定データに計算結果も含めるか
	m_ctsMainFrm.CalcResultEnable		= If(cbDatafileCalcEnable.Checked, 1, 0)


	'温度制限なしの場合、最大にする
	If p_bTempNoLimitEn
		HEATER_UPLMT_DEV = 399.0		'装置制限
		HEATER_LWLMT_DEV = 0.0
		HEATER_UPLMT_NUD = 390.0		'入力制限
		HEATER_LWLMT_NUD = 0.0
	Else
		HEATER_UPLMT_DEV = 250.0		'装置制限
		HEATER_LWLMT_DEV = 0.0
		HEATER_UPLMT_NUD = 240.0		'入力制限
		HEATER_LWLMT_NUD = 0.0
	End If
	'温度設定フィールド範囲設定
	nudHUpperTemp.Maximum	= HEATER_UPLMT_NUD
	nudHUpperTemp.Minimum	= HEATER_LWLMT_NUD
	nudHUpperTemp2.Maximum	= HEATER_UPLMT_NUD
	nudHUpperTemp2.Minimum	= HEATER_LWLMT_NUD
#End If

	If Not IsNothing(sErrMsg)
        MsgBox(sErrMsg, MsgBoxStyle.OkOnly Or MsgBoxStyle.Exclamation,  System.Windows.Forms.Application.ProductName)
		Return -1
	Else
		Dim sEquip		As String = m_ctsMainFrm.strEquipName(m_ctsMainFrm.iSelMDK)		'新
		Dim sPreEquip	As String = original.strEquipName(original.iSelMDK)				'旧
		If sEquip<> sPreEquip AndAlso File.Exists(Path.Combine(SIZUKA_PATH, sEquip, DevSettingFile))
		'装置IDが変更され、変更された装置の設定が存在するとき
			p_sEquipName = sEquip
			m_ctsMainFrm.LoadDeviceSettingINI(p_sEquipName, False)	'初期化ファイルを読込む
			m_ctsMainFrm.LoadTempParaINI(p_sEquipName, False)		'温調パラメータを読込む
			MemToScr()												'クラス内容をフォームコントロール値へ反映する
		Else
			'設定内容をINIへ保存
			m_ctsMainFrm.SaveAsDeviceListINI()					'装置ID設定
			p_sEquipName = sEquip
			current = m_ctsMainFrm
			'If Not AreObjectsEqual(original, current)
			Dim diffs As List(Of String) = CompareObjectsAll(original, current, "")
			If diffs.Count <> 0
			'バックアップ作成
				DeviceSettingHistryBackup()
				For Each DiffItem As String In diffs
				    Call WriteLog("詳細設定 変更 " & DiffItem , eLOG_LEVEL.iINFO )
				Next
				original = CType(DeepCopyObject(m_ctsMainFrm), cToolSettings)
			End If
			m_ctsMainFrm.SaveAsDeviceSettingINI(p_sEquipName)	'各種設定
			m_ctsMainFrm.SaveAsTempParaINI(p_sEquipName)		'温調パラメータを保存
			m_ctsMainFrm.InputSelectionFileSave(p_sEquipName)	'入力選択リスト保存(サンプル名、測定者)												'
		End If
	End If
Catch ex As Exception
    Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
    Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Return -1
End Try
	Return 0
End Function
''' <summary>
''' フォームコントロール値でクラス内容へ反映する
''' </summary>
Private Sub	ScrToMem(ByRef sErrMsg As String)
Try
    ' 表示言語
    If rbJPN.Checked Then
        m_ctsMainFrm.iLang = 1
    ElseIf rbTChinese.Checked Then
        m_ctsMainFrm.iLang = 4
    ElseIf rbKorean.Checked Then
        m_ctsMainFrm.iLang = 3
    ElseIf rbSChinese.Checked Then
        m_ctsMainFrm.iLang = 2
    Else
        m_ctsMainFrm.iLang = 0
    End If
    ' ポート番号
    m_ctsMainFrm.strPort = txtPort.Text.Replace(" ", "")
    ' グリッド用ペン
    m_ctsMainFrm.penGrids.Width = 1.0F
    If rbSolid.Checked Then
        m_ctsMainFrm.penGrids.DashStyle = Drawing2D.DashStyle.Solid
    ElseIf rbDash.Checked Then
        m_ctsMainFrm.penGrids.DashStyle = Drawing2D.DashStyle.Dash
    ElseIf rbDotGrids.Checked Then
        m_ctsMainFrm.penGrids.DashStyle = Drawing2D.DashStyle.Dot
    ElseIf rbNoGrids.Checked Then
        m_ctsMainFrm.penGrids.DashStyle = Drawing2D.DashStyle.Custom
    Else
        m_ctsMainFrm.penGrids.DashStyle = Drawing2D.DashStyle.Solid
    End If
    ' 負の値の表示
    If rbDisplayNgtv.Checked Then
        m_ctsMainFrm.iHideNgtv = 0
    ElseIf rbHideNgtv.Checked Then
        m_ctsMainFrm.iHideNgtv = 1
    Else
        m_ctsMainFrm.iHideNgtv = 0
    End If

	'時間間引き数
	m_ctsMainFrm.iDetailsR = nudThinningValueR.Value
	m_ctsMainFrm.iDetailsD = nudThinningValueD.Value

	'時間スクロール時の幅(秒)
	m_ctsMainFrm.iScrlWidth = nudScrolWidth.Value

	'マーカスタイル
	m_ctsMainFrm.iMarkerStyle = cbxMarkerStyle.SelectedIndex
	'マーカーサイズ
	m_ctsMainFrm.iMarkerSize = nudMarkerSize.Value

    ' 近似範囲表示方法
    If rbARDFill.Checked Then
        m_ctsMainFrm.iARDisp = 0
    Else
        m_ctsMainFrm.iARDisp = 1
    End If
    ' リアルタイムグラフX軸
    If rbRXauto.Checked Then
        m_ctsMainFrm.iRTXAxis = 0
    Else
        m_ctsMainFrm.iRTXAxis = 1
    End If
    ' リアルタイムグラフY軸
    If rbRYauto.Checked Then
        m_ctsMainFrm.iRTYAxis = 0
    Else
        m_ctsMainFrm.iRTYAxis = 1
    End If


    'フィルタパラメータ
    m_ctsMainFrm.iFilterEnable = iif(cbxFilterEnable.Checked, 1, 0)
    Select Case rbFIR.Checked
    Case True
        m_ctsMainFrm.sFilterKind = "FIR"
    Case False
        m_ctsMainFrm.sFilterKind = "IIR"
    End Select
    m_ctsMainFrm.iFilterLen		= nudFilterLen.Value
    m_ctsMainFrm.dFirCutOffFreq = nudFIRCoffFreq.Value
    m_ctsMainFrm.sFilterWinKind = cbxWinKind.Text

    m_ctsMainFrm.dIIRCutOffFreq = nudIIRCoffFreq.Value
    m_ctsMainFrm.iIIRDeg = nudIIRdeg.Value

    m_ctsMainFrm.sFilterFileName = lblFilterFileName.Text
    m_ctsMainFrm.DBAutoReg		= iif(cbxAutoReg.Checked, 1, 0)
    m_ctsMainFrm.SPCEn			= iif(cbxSPC.Checked, 1, 0)
    m_ctsMainFrm.SPCCalcSample	= nudSPCCalcSample.Value						'SPC機能 計算サンプル数


	'GAP補正値
    m_ctsMainFrm.GapCorr		= nudGapCorr.Value

    ' 測定自動開始
    If cbAutoStart.Checked Then
        m_ctsMainFrm.iAutoStart = nudAutoStart.Value
    Else
        m_ctsMainFrm.iAutoStart = -1
    End If
    ' 管理者用オプション
    If cbCanDelete.Checked Then
        m_ctsMainFrm.iCanRemoveData = 0
    Else
        m_ctsMainFrm.iCanRemoveData = -1
    End If
    If cbModifyCF.Checked Then
        m_ctsMainFrm.iCanModifyCF = 0
    Else
        m_ctsMainFrm.iCanModifyCF = -1
    End If
    If cbModifyRecalc.Checked Then
        m_ctsMainFrm.iCanModifyRecalc = 0
    Else
        m_ctsMainFrm.iCanModifyRecalc = -1
    End If
    If cbModifySpec.Checked Then
        m_ctsMainFrm.iCanModifySpec = 0
    Else
        m_ctsMainFrm.iCanModifySpec = -1
    End If


    ' 通信一覧の記入状態を取得
    For iRow As Integer = 0 To dgvEquipList.RowCount - 1
        ' 通信機器の選択状態
        If dgvEquipList.Item(EL_COL.USEEQUIP, iRow).Tag = True Then
            m_ctsMainFrm.iSelMDK = iRow
        End If
        ' 機器毎のIPアドレスと名称、校正係数
        If dgvEquipList.Item(EL_COL.IP, iRow).Value Is Nothing Then
            m_ctsMainFrm.strIP(iRow) = ""
        Else
            m_ctsMainFrm.strIP(iRow) = dgvEquipList.Item(EL_COL.IP, iRow).Value.ToString.Replace(" ", "")
	        If m_ctsMainFrm.iSelMDK = iRow And m_ctsMainFrm.strIP(iRow) = ""
				'チェックされている機器にIPアドレスが入力されていない
				sErrMsg = m_sStr(eMes.INVALID_INPUT_IP)
	        End If
        End If
        m_ctsMainFrm.strEquipName(iRow)		= dgvEquipList.Item(EL_COL.NAME, iRow).Value
        m_ctsMainFrm.sXYSensorSerial(iRow)	= dgvEquipList.Item(EL_COL.XYSENS, iRow).Value
        m_ctsMainFrm.sZSensorSerial(iRow)	= dgvEquipList.Item(EL_COL.ZSENS, iRow).Value

        Dim dMCF() As Double = New Double(3) {}
        Dim dTCF() As Double = New Double(6) {}
    Next iRow


    ' FTPパスワード
    m_ctsMainFrm.strFTPUser = m_strUserId
    m_ctsMainFrm.strFTPPass = m_strPassword

    'ゼロ調
	'ゼロ調モード
	If rbZeroXcont.Checked
		m_ctsMainFrm.ZeroAdjMode = 0
	End If
	If rbZeroZcont.Checked
		m_ctsMainFrm.ZeroAdjMode = 1
	End If
    'ゼロ調条件
    Double.TryParse(nudZeroStartPosHP.Text, m_ctsMainFrm.dZeroStaPosHP)    '熱板開始位置(mm)
    int16.TryParse(nudStaMonTime.Text, m_ctsMainFrm.iStaMonTime)           '安定度監視時間(S)
    int16.TryParse(nudStaFlacRange.Text, m_ctsMainFrm.iStaFlucRange)       '安定度範囲(um)
    '中央出し条件
    Double.TryParse(nudCentStartPosHP.Text, m_ctsMainFrm.dCenterStaPosHP)  '中央出し熱板開始位置(mm)
    Int32.TryParse(lblCenterX.Text, m_ctsMainFrm.iCenterResultsX)          '中央出し結果X位置(um)
    Int32.TryParse(lblCenterY.Text, m_ctsMainFrm.iCenterResultsY)          '中央出し結果Y位置(um)

	'Z軸上限リミット位置
	m_ctsMainFrm.ZUpperLimitPos = cType(nudZUpperLimit.Value * 1000.0, int32)


    'カラー更新
    For iCnt As Integer = 0 To m_lblClr.Length - 1
        Select Case iCnt
            Case m_lblClr.Length - 2
                m_ctsMainFrm.clrGraphBack = m_lblClr(iCnt).BackColor
            Case m_lblClr.Length - 1
                m_ctsMainFrm.penGrids.Color = m_lblClr(iCnt).BackColor
            Case Else
                m_ctsMainFrm.clrY1Default(iCnt) = m_lblClr(iCnt).BackColor
        End Select
    Next
    'If cbY2Opp.Checked = False Then
    '    m_ctsMainFrm.bY2Opposite = False
    'Else
    '    m_ctsMainFrm.bY2Opposite = True
    'End If

    'Auto Printの有効/無効
    If cbxAutPrnt.Checked = True Then
        m_ctsMainFrm.iAutoPrintEnableValue = 1

        '保存先フォルダが作成できない場合
        If CreateFolder(txtStrgFld.Text) = False Then
            MsgBox(m_sStr(eMes.iAUTOPRINT_DISABLE), MsgBoxStyle.Exclamation Or MsgBoxStyle.OkOnly, "Auto Print")
            cbxAutPrnt.Checked = False
            m_ctsMainFrm.iAutoPrintEnableValue = 0
        Else
            m_ctsMainFrm.strAutoPrintOutPutFolderValue = txtStrgFld.Text
        End If
    Else
        m_ctsMainFrm.iAutoPrintEnableValue = 0
        m_ctsMainFrm.strAutoPrintOutPutFolderValue = txtStrgFld.Text
    End If

    '出力ファイルタイプ
    Select Case True
        Case rbtnJpg.Checked        'JPEG
            m_ctsMainFrm.iAutoPrintTypeValue = eAUTOPRINT_FILE.iJPG
        Case rbtnPdf.Checked        'PDF
            m_ctsMainFrm.iAutoPrintTypeValue = eAUTOPRINT_FILE.iPDF
    End Select

    '測定場所
    m_ctsMainFrm.strAutoPrintMesurePlaceValue = cbxMeasPlace.Text

    '測定場所リスト
    Dim sArry()		As String
    sArry = GetSortComboList(cbxMeasPlace, cbxMeasPlace.Text)
    m_ctsMainFrm.sAutoPrintMesurePlaceList = String.Join(DF_OPER_LIST_SEP, sArry)        '配列内のデータをすべて DF_OPER_LIST_SEP 区切りで連結

    '測定者
    m_ctsMainFrm.strAutoPrintOperatorValue = cbxMeasOperator.Text

    '測定者リスト
    sArry = GetSortComboList(cbxMeasOperator, cbxMeasOperator.Text)
    m_ctsMainFrm.sAutoPrintOperatorList = String.Join(DF_OPER_LIST_SEP, sArry)           '配列内のデータをすべて DF_OPER_LIST_SEP 区切りで連結

    'Auto Printのディスク容量
    m_ctsMainFrm.iAutoPrintDiskCapacityValue = nudDskSpc.Value

    '測定結果自動保存の有効/無効
    '測定結果自動保存の保存先フォルダ
    If chkAutSaveMeasData.Checked = True Then
        m_ctsMainFrm.iAutoSaveMeasDataEnable = 1

        '保存先フォルダが作成できない場合
        If CreateFolder(txtStrgFldMeasData.Text) = False Then
            MsgBox(m_sStr(eMes.iAUTOPRINT_DISABLE), MsgBoxStyle.Exclamation Or MsgBoxStyle.OkOnly, "Auto Print")
            chkAutSaveMeasData.Checked = False
            m_ctsMainFrm.iAutoSaveMeasDataEnable = 0
        Else
            m_ctsMainFrm.sAutoSaveMeasDataOutPutFolder = txtStrgFldMeasData.Text
        End If
    Else
        m_ctsMainFrm.iAutoSaveMeasDataEnable = 0
        m_ctsMainFrm.sAutoSaveMeasDataOutPutFolder = txtStrgFldMeasData.Text
    End If
    m_ctsMainFrm.iStaMonTime = nudStaMonTime.Value


    '固定スペックロット名手動入力
    Select Case cbxLotManu.Checked
        Case True
            m_ctsMainFrm.iFixSpecLotManu = 1
        Case Else
            m_ctsMainFrm.iFixSpecLotManu = 0
    End Select
    '本体電源ON時間の超過判定閾値
    m_ctsMainFrm.iMntChkIntegHourPowerOn	= nudMainteThr1.Value
    '熱版ヒータON時間の超過判定閾値
    m_ctsMainFrm.iMntChkIntegHourHeaterOn	= nudMainteThr2.Value
    'XYZ動作ON時間の超過判定閾値
    m_ctsMainFrm.iMntChkIntegHourXAct		= nudMainteThr3.Value
    'PBヒータON時間の超過判定閾値
    m_ctsMainFrm.iMntChkIntegHourHeaterOn2	= nudMainteThr4.Value
    'SGヒータON時間の超過判定閾値
    m_ctsMainFrm.iMntChkIntegHourHeaterOn3	= nudMainteThr5.Value

    '温度制御
    m_ctsMainFrm.TempCnt_P          = nudP.Value
    m_ctsMainFrm.TempCnt_I          = nudI.Value
    m_ctsMainFrm.TempCnt_D          = nudD.Value
    m_ctsMainFrm.TempCnt_Dgain      = nudDgain.Value
    m_ctsMainFrm.TempCnt_PVBias     = nudPVBias.Value
    m_ctsMainFrm.TempCnt_PVGain     = nudPVGain.Value
    m_ctsMainFrm.TempCnt_HUpperLimit= nudHUpperTemp.Value

    m_ctsMainFrm.TempCnt_P2          = nudP2.Value
    m_ctsMainFrm.TempCnt_I2          = nudI2.Value
    m_ctsMainFrm.TempCnt_D2          = nudD2.Value
    m_ctsMainFrm.TempCnt_Dgain2      = nudDgain2.Value
    m_ctsMainFrm.TempCnt_PVBias2     = nudPVBias2.Value
    m_ctsMainFrm.TempCnt_PVGain2     = nudPVGain2.Value
    m_ctsMainFrm.TempCnt_HUpperLimit2= nudHUpperTemp2.Value

    m_ctsMainFrm.TempCnt_P3          = nudP3.Value
    m_ctsMainFrm.TempCnt_I3          = nudI3.Value
    m_ctsMainFrm.TempCnt_D3          = nudD3.Value
    m_ctsMainFrm.TempCnt_Dgain3      = nudDgain3.Value
    m_ctsMainFrm.TempCnt_PVBias3     = nudPVBias3.Value
    m_ctsMainFrm.TempCnt_PVGain3     = nudPVGain3.Value
    m_ctsMainFrm.TempCnt_HUpperLimit3= nudHUpperTemp3.Value
    m_ctsMainFrm.TempCnt_HLowerLimit3= nudHLowerTemp3.Value
    m_ctsMainFrm.TempCnt_HDifSetTemp3= nudHDifSetTemp3.Value

    ''m_ctsMainFrm.TempLED = nudLEDTempObsrv.Value
    ''m_ctsMainFrm.TempLEDen = cbxTempLEDEn.Checked

    '測定制御
    m_ctsMainFrm.SampleInputWait		= nudSampleInputWait.Value
    m_ctsMainFrm.InputGap				= nudInputGap.Value
    m_ctsMainFrm.HPTempRange	        = nudHPTempRange.Value
    m_ctsMainFrm.PBTempRange	        = nudPBTempRange.Value
    m_ctsMainFrm.SGTempRange	        = nudSGTempRange.Value
    m_ctsMainFrm.SGStressStab	        = nudStrainStab.Value
    m_ctsMainFrm.PressContDelayTime		= nudPresContDeyayTime.Value
	m_ctsMainFrm.DoublePBUnlockConfirm	= ckbDoublePBUnlockConfirm.Checked
	m_ctsMainFrm.GreenFlashInMeas		= ckbGreenFlashInMeas.Checked


	'長時間間欠測定設定
	p_LTModeChange						= If(m_ctsMainFrm.LTModeEnable<>cbxLongMeasMode.Checked, True, False)
	m_ctsMainFrm.LTModeEnable			= cbxLongMeasMode.Checked			
    m_ctsMainFrm.LTMeasureInterval		= nudLTMeasInterval.Value
    m_ctsMainFrm.LTAdoptRate			= nudLTAdoptRate.Value


	'QRコード運用設定
	m_ctsMainFrm.QRModeEnable			= cbxQRCodeEnable.Checked			
    m_ctsMainFrm.QREcc					= cboECC.Text
    m_ctsMainFrm.QRLogoCells			= nudCellsInLogo.Value
    m_ctsMainFrm.QRPixelInCel			= nudPixelInCell.Value




    'ブザー制御
    m_ctsMainFrm.EndOfBuzzerEn		= cbxEndOfMesure.checked
	m_ctsMainFrm.EndOfBuzzerPattern = nudBZPattern.Value
    m_ctsMainFrm.BuzzerOkOfHeatEn	= cbxOkOfHeat.checked

    'IL無効
    m_ctsMainFrm.CoverILDis = cbxCoverILDis.checked

    '測定値領域読込許可
    m_ctsMainFrm.ReadRealAreaEn = cbxReadRealAreaEn.checked

    'スタンバイ時間
    m_ctsMainFrm.StanbyTime = nudStandbyTime.Value

	'多重起動有効無効
	m_ctsMainFrm.MultiActivate = If(cbxMultActEn.Checked, 1, 0)


    Dim sErr As String = ""
    ' スペックが編集中であれば保存(保存時警告なし)
#If Not NEW_SPEC
    sErr = sbCheckAndSaveFixSpec(False)
#end if
    'sErr = sErr & sbCheckAndSaveRTGSet(False)


    '応力検知関係
    m_ctsMainFrm.StresDetAcqTime		= nudStrsDetAcqTime.Value
    m_ctsMainFrm.StresDetADThreshold	= nudStrsDetADThreshold.Value
    m_ctsMainFrm.StresDetDuraTime		= nudStresDetDuraTime.Value

	'測定データに計算結果も含めるか
	m_ctsMainFrm.CalcResultEnable		= If(cbDatafileCalcEnable.Checked, 1, 0)


	'温度制限なしの場合、最大にする
	If p_bTempNoLimitEn
		HEATER_UPLMT_DEV = 400.0		'装置制限
		HEATER_LWLMT_DEV = 0.0
		HEATER_UPLMT_NUD = 390.0		'入力制限
		HEATER_LWLMT_NUD = 0.0
	Else
		HEATER_UPLMT_DEV = 400.0		'装置制限
		HEATER_LWLMT_DEV = 0.0
		HEATER_UPLMT_NUD = 390.0		'入力制限
		HEATER_LWLMT_NUD = 0.0
	End If
	'温度設定フィールド範囲設定
	nudHUpperTemp.Maximum	= HEATER_UPLMT_NUD
	nudHUpperTemp.Minimum	= HEATER_LWLMT_NUD
	nudHUpperTemp2.Maximum	= HEATER_UPLMT_NUD
	nudHUpperTemp2.Minimum	= HEATER_LWLMT_NUD

Catch ex As Exception
    Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	sErrMsg = "There was an error in the settings. The settings were not saved."
End Try
End Sub

''' <summary>
''' DeviceSettings初期化ファイル履歴作成
''' </summary>
Public Sub DeviceSettingHistryBackup()
Try
	Dim sDistPath = Path.Combine(SIZUKA_PATH, p_sEquipName, "DeviceSettingHistory")
	Dim sSrcePath = Path.Combine(SIZUKA_PATH, p_sEquipName, DevSettingFile)
	If Not Directory.Exists(sDistPath)
		Directory.CreateDirectory(sDistPath)
	End If
	sDistPath = Path.Combine(sDistPath, Now.ToString("yyyyMMddHHmmss") & "_" & DevSettingFile)
	File.Copy(sSrcePath, sDistPath, True)
    Call WriteLog("DeviceSetting初期化ファイルをバックアップしました->" & sDistPath, eLOG_LEVEL.iINFO)

Catch ex As Exception
    Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub


'クラス変更判定関係 -----
Dim original	As New cToolSettings()		'変更前詳細設定
Dim current		As New cToolSettings()		'変更後詳細設定


Private Function CompareObjectsAll(obj1 As Object, obj2 As Object, memberPath As String) As List(Of String)
Try
    Dim diffs As New List(Of String)
    ' ===== Nothingチェック =====
    If obj1 Is Nothing AndAlso obj2 Is Nothing Then
        Return diffs
    End If
    If obj1 Is Nothing OrElse obj2 Is Nothing Then
        diffs.Add($"{memberPath} (Old={ValueToString(obj1)}, New={ValueToString(obj2)})")
        Return diffs
    End If
    Dim t As Type = obj1.GetType()
    If t IsNot obj2.GetType() Then
        diffs.Add($"{memberPath} (型不一致 Old={t.Name}, New={obj2.GetType().Name})")
        Return diffs
    End If
    ' ===== Pen =====
    If t Is GetType(System.Drawing.Pen) Then
        Dim p1 = CType(obj1, System.Drawing.Pen)
        Dim p2 = CType(obj2, System.Drawing.Pen)
        If p1.Color.ToArgb() <> p2.Color.ToArgb() Then
            diffs.Add($"{memberPath}.Color (Old={ColorToString(p1.Color)}, New={ColorToString(p2.Color)})")
        End If
        If p1.Width <> p2.Width Then
            diffs.Add($"{memberPath}.Width (Old={p1.Width}, New={p2.Width})")
        End If
        If p1.DashStyle <> p2.DashStyle Then
            diffs.Add($"{memberPath}.DashStyle (Old={p1.DashStyle}, New={p2.DashStyle})")
        End If
        Return diffs
    End If
    ' ===== Color =====
    If t Is GetType(System.Drawing.Color) Then
        Dim c1 = CType(obj1, System.Drawing.Color)
        Dim c2 = CType(obj2, System.Drawing.Color)
        If c1.ToArgb() <> c2.ToArgb() Then
            diffs.Add($"{memberPath} (Old={ColorToString(c1)}, New={ColorToString(c2)})")
        End If
        Return diffs
    End If
    ' ===== 値型・文字列 =====
    If t.IsValueType OrElse t Is GetType(String) Then
        If Not obj1.Equals(obj2) Then
            diffs.Add($"{memberPath} (Old={ValueToString(obj1)}, New={ValueToString(obj2)})")
        End If
        Return diffs
    End If
    ' ===== 配列 =====
    If t.IsArray Then
        Dim arr1 = CType(obj1, Array)
        Dim arr2 = CType(obj2, Array)
        If arr1.Length <> arr2.Length Then
            diffs.Add($"{memberPath} (配列長不一致 Old={arr1.Length}, New={arr2.Length})")
        End If
        Dim minLen As Integer = Math.Min(arr1.Length, arr2.Length)
        For i As Integer = 0 To minLen - 1
            Dim childDiffs = CompareObjectsAll(
                arr1.GetValue(i),
                arr2.GetValue(i),
                $"{memberPath}({i})")
            diffs.AddRange(childDiffs)
        Next
        Return diffs
    End If
    ' ===== クラス（再帰） =====
    For Each f In t.GetFields(
        Reflection.BindingFlags.Instance Or
        Reflection.BindingFlags.Public Or
        Reflection.BindingFlags.NonPublic)
        Dim v1 = f.GetValue(obj1)
        Dim v2 = f.GetValue(obj2)
        Dim newPath As String =
            If(memberPath = "", f.Name, $"{memberPath}.{f.Name}")
        Dim childDiffs = CompareObjectsAll(v1, v2, newPath)
        diffs.AddRange(childDiffs)
    Next
    Return diffs
Catch ex As Exception
    Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
    Return Nothing
End Try
End Function
Private Function ValueToString(obj As Object) As String
    If obj Is Nothing Then Return "Nothing"
    Return obj.ToString()
End Function
Private Function ColorToString(c As System.Drawing.Color) As String
    If c.IsNamedColor Then
        Return c.Name
    Else
        Return $"ARGB({c.A},{c.R},{c.G},{c.B})"
    End If
End Function
#if 0
''' <summary>
''' クラス比較判定処理
'''		Drawing.Penは対象外
''' </summary>
''' <param name="obj1">変更前</param>
''' <param name="obj2">変更後</param>
''' <returns></returns>
Public Function AreObjectsEqual(obj1 As Object, obj2 As Object) As Boolean
Try
    If obj1 Is Nothing AndAlso obj2 Is Nothing	Then Return True
    If obj1 Is Nothing OrElse obj2 Is Nothing	Then Return False
    If obj1.GetType() IsNot obj2.GetType()		Then Return False
    Dim t As Type = obj1.GetType()
    ' ===== 値型・文字列 =====
    If t.IsValueType OrElse t Is GetType(String)
        Return Object.Equals(obj1, obj2)
    End If
    ' ===== Pen =====
    If t Is GetType(System.Drawing.Pen)
		Dim p1 = CType(obj1, System.Drawing.Pen)
		Dim p2 = CType(obj2, System.Drawing.Pen)

		Return p1.Color.ToArgb() = p2.Color.ToArgb() AndAlso
			   p1.Width = p2.Width AndAlso
			   p1.DashStyle = p2.DashStyle    
	End If
    ' ===== 配列専用処理 =====
    If t.IsArray
        Dim arr1 As Array = CType(obj1, Array)
        Dim arr2 As Array = CType(obj2, Array)
        ' ★ 長さチェック（重要）
        If arr1.Length <> arr2.Length
			Return False
		End If
        For i As Integer = 0 To arr1.Length - 1
            If Not AreObjectsEqual(arr1.GetValue(i), arr2.GetValue(i))
                Return False
            End If
        Next
        Return True
    End If
    ' ===== IEnumerable（Listなど）=====
    If GetType(IEnumerable).IsAssignableFrom(t) AndAlso t IsNot GetType(String)
        Dim list1 As IEnumerable = CType(obj1, IEnumerable)
        Dim list2 As IEnumerable = CType(obj2, IEnumerable)
        Dim l1 As New List(Of Object)
        Dim l2 As New List(Of Object)
        For Each item In list1
            l1.Add(item)
        Next
        For Each item In list2
            l2.Add(item)
        Next
        ' ★ Count比較（重要）
        If l1.Count <> l2.Count
			Return False
		End If
        For i As Integer = 0 To l1.Count - 1
            If Not AreObjectsEqual(l1(i), l2(i))
                Return False
            End If
        Next
        Return True
    End If
    ' ===== クラスのプロパティ再帰比較 =====
    For Each prop In t.GetProperties(BindingFlags.Public Or BindingFlags.NonPublic Or BindingFlags.Instance)
        If prop.GetIndexParameters().Length > 0 Then Continue For
        If Not prop.CanRead Then Continue For
        Dim v1 = prop.GetValue(obj1, Nothing)
        Dim v2 = prop.GetValue(obj2, Nothing)
        If Not AreObjectsEqual(v1, v2)
            Return False
        End If
    Next
	' ===== フィールドも比較する =====
	For Each field In t.GetFields(BindingFlags.Public Or BindingFlags.NonPublic Or BindingFlags.Instance)
		Dim v1 = field.GetValue(obj1)
		Dim v2 = field.GetValue(obj2)
		If Not AreObjectsEqual(v1, v2)
			Return False
		End If
	Next
    Return True
Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
    Return True
End Try
End Function
#end if
''' <summary>
''' クラスオブジェクトのコピーを作成
''' </summary>
''' <param name="obj">ソースオブジェクト</param>
''' <returns></returns>
Public Function DeepCopyObject(obj As Object) As Object
Try
    If obj Is Nothing Then Return Nothing
    Dim t As Type = obj.GetType()
    ' 値型・文字列
    If t.IsValueType OrElse t Is GetType(String)
        Return obj
    End If
    ' Pen
	If obj.GetType() Is GetType(System.Drawing.Pen)
		Return CType(obj, System.Drawing.Pen).Clone()
	End If
    ' 配列
    If t.IsArray
        Dim arr		As Array = CType(obj, Array)
        Dim newArr	As Array = Array.CreateInstance(t.GetElementType(), arr.Length)
        For i As Integer = 0 To arr.Length - 1
            newArr.SetValue(DeepCopyObject(arr.GetValue(i)), i)
        Next
        Return newArr
    End If
    ' IEnumerable（Listなど）
    If GetType(IEnumerable).IsAssignableFrom(t) AndAlso t IsNot GetType(String)
        Dim listType = t
        Dim newList = Activator.CreateInstance(listType)
        For Each item In CType(obj, IEnumerable)
            listType.GetMethod("Add").Invoke(newList, {DeepCopyObject(item)})
        Next
        Return newList
    End If
    ' クラス
    Dim newObj = Activator.CreateInstance(t)
    ' プロパティコピー
    For Each prop In t.GetProperties(BindingFlags.Public Or BindingFlags.NonPublic Or BindingFlags.Instance)
        If Not prop.CanRead OrElse Not prop.CanWrite Then Continue For
        If prop.GetIndexParameters().Length > 0 Then Continue For
        Dim value = prop.GetValue(obj, Nothing)
        prop.SetValue(newObj, DeepCopyObject(value), Nothing)
    Next
    ' フィールドコピー
    For Each field In t.GetFields(BindingFlags.Public Or BindingFlags.NonPublic Or BindingFlags.Instance)
        Dim value = field.GetValue(obj)
        field.SetValue(newObj, DeepCopyObject(value))
    Next
    Return newObj
Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
    Return obj
End Try
End Function
'クラス変更判定関係 -----



''' <summary> 指定コンボボックスにRTGSETの一覧を表示する。</summary>
''' <param name="cbo"></param>
Public Shared Function DisplayRTGSETCBO(ByRef cbo As ComboBox)
    Try
        Dim strPrevSpecRtg As String = cbo.Text
		Dim sAry() As String = Nothing
		Dim iCnt As Int32
        cbo.Items.Clear()
        cbo.Items.Add("")
        Dim dinfoNum As New IO.DirectoryInfo(Path.Combine(SIZUKA_PATH, p_sEquipName, RTGSET_PATH))
		iCnt = 0
        For Each diSampFile As IO.FileInfo In dinfoNum.GetFiles("*.ini")
            Dim strINIPath As String = diSampFile.FullName
            ' 格納用バッファ
            Dim strSb As System.Text.StringBuilder = Nothing
            strSb = New System.Text.StringBuilder(1024)
            GetPrivateProfileString("RTG", "Name", "", strSb, strSb.Capacity, strINIPath)
            If strSb.ToString <> "" Then
				Array.Resize(sAry, iCnt+1)
				sAry(iCnt) = strSb.ToString()
				iCnt += 1
            End If
        Next
		Array.Sort(sAry)
	    cbo.Items.AddRange(sAry)

        ' 直前のコンボボックス選択値を復元
        cbo.Text = strPrevSpecRtg

    Catch ex As Exception
        Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
        Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
    End Try

    Return True
End Function



''' <summary> RTGSETの内容をRtgset構造体に読込する。 </summary>
''' <param name="strRtgsetName">読込する設定名。</param>
''' <param name="rtgset">代入先のRtgset構造体配列。</param>
''' <param name="bZPosEnable">Z位置温度キャリブレーション使用可能か</param>
''' <returns>読み込みに成功した場合True。失敗した場合False。</returns>
Public Shared Function LoadRTGSET(ByVal strRtgsetName As String, ByVal sEquipName As String, ByRef rtgset As RealTimeMeas.RtgSetF, Optional ByRef bZPosEnable As Boolean = False) As Boolean
rtgset = New RealTimeMeas.RtgSetF("")
Dim bReadFlg As Boolean = False
Dim dinfoNum As New IO.DirectoryInfo(Path.Combine(SIZUKA_PATH, If(isNothing(sEquipName),"",sEquipName), RTGSET_PATH))
Dim dTotalTime	As Double = 0

Try
    For Each diSampFile As IO.FileInfo In dinfoNum.GetFiles("*.ini")
        ' App.iniの場所は絶対パスで指定する
        Dim strINIPath As String = diSampFile.FullName
        ' 格納用バッファ
        Dim strSb As System.Text.StringBuilder = Nothing
        strSb = New System.Text.StringBuilder(1024)

        Dim dPrevTime	As Double = 0.0R
        Dim dCurTime	As Double = 0.0R
        ' INIより設定値を読込
        GetPrivateProfileString("RTG", "Name", "", strSb, strSb.Capacity, strINIPath)
        If strSb.ToString = strRtgsetName Then
            rtgset.strName = strSb.ToString
            For iRow As Integer = 0 To m_iRTGSETMaxRow - 1
                GetPrivateProfileString("RTG", "Linear" + CStr(iRow), "0", strSb, strSb.Capacity, strINIPath)
                Integer.TryParse(strSb.ToString(), rtgset.iLinearFlag(iRow))
                GetPrivateProfileString("RTG", "TIME" + CStr(iRow), "0", strSb, strSb.Capacity, strINIPath)
                Double.TryParse(strSb.ToString(), rtgset.dTiming(iRow))
                dCurTime = rtgset.dTiming(iRow)
				If dCurTime<>0
					dTotalTime = dCurTime
				End If
                If dPrevTime >= dCurTime Then
                    'rtgset.mdbr(iRow).dHPTemp = 0.0R
                    rtgset.mdbr(iRow).dHPTemp = Double.MinValue
                Else
                    dPrevTime = dCurTime
                    GetPrivateProfileString("RTG", "TEMP" + CStr(iRow), "0", strSb, strSb.Capacity, strINIPath)
                    Double.TryParse(strSb.ToString(), rtgset.mdbr(iRow).dHPTemp)
                End If
            Next iRow

			Array.Resize(rtgset.dZCalibTemp, dTotalTime)
			Array.Resize(rtgset.dZCalibPos, dTotalTime)
			Dim sBuf()	As String

#If 0
			'初期化ファイル形式のリード
			For i = 0 To dTotalTime - 1
	            GetPrivateProfileString("ZPOS CAL", i.ToString, "0", strSb, strSb.Capacity, strINIPath)
				sBuf = strSb.ToString.Split(",")			'温度, Z補正値増分(um)
				If sBuf.Length = 2
					rtgset.dZCalibTemp(i)	= Ctype(sBuf(0), Double)
					rtgset.dZCalibPos(i)	= Ctype(sBuf(1), Double)
				End If
			Next
#else
			'補正値高速リード
			Dim fs As FileStream	= New FileStream(strINIPath, FileMode.Open, FileAccess.Read,FileShare.ReadWrite)
			Dim sr As StreamReader	= New StreamReader(fs, Encoding.UTF8)
			Dim sLineBuf As String
			Dim iMode	As Int16 = -1
			Dim sBuf1()	As String
			Dim idx		As Int32 = 0
			Do 
				sLineBuf = sr.ReadLine()
				IF sr.EndOfStream
					Exit Do
				ElseIf IsNothing(sLineBuf)
					Continue Do
				End If
				If sLineBuf = "[ZPOS CAL]"
					iMode = 1
					idx = 0
					Continue Do
				End If
				Select Case iMode
				Case 1
					sBuf = sLineBuf.Split("=")			'Keyを削除
					sBuf1 = sBuf(1).Split(",")			'温度, Z補正値増分(um)
					If idx <= rtgset.dZCalibTemp.GetUpperBound(0)
						rtgset.dZCalibTemp(idx) = Ctype(sBuf1(0), Double)
						rtgset.dZCalibPos(idx)	= Ctype(sBuf1(1), Double)
						idx += 1
					Else
					End If
				Case Else
				End Select
			Loop
			sr.Close()
			fs.Close()
#end if
			'ZPOS キャリブレーションデータの確認
			GetPrivateProfileString("RTG",  "Created Date", "NONE", strSb, strSb.Capacity, strINIPath)
			Dim s1 As String = strSb.ToString
			GetPrivateProfileString("ZPOS CAL",  (dTotalTime-1), "NONE", strSb, strSb.Capacity, strINIPath)
			Dim s2 As String = strSb.ToString
			bZPosEnable = If(s1="NONE" Or s2="NONE", False, True)


#If 0
			Dim fs As FileStream	= New FileStream(strINIPath, FileMode.Open, FileAccess.Read,FileShare.ReadWrite)
			Dim sr As StreamReader	= New StreamReader(fs, Encoding.UTF8)
			Dim sLineBuf As String
			Dim iMode	As Int16 = -1
			Dim sBuf()	As String
			Dim idx		As Int32 = 0
			Do 
				sLineBuf = sr.ReadLine()
				If IsNothing(sLineBuf)
					Exit Do
				End If
				If sLineBuf = "[ZPOS CAL]"
					iMode = 1		'Z補正行
					Array.Resize(rtgset.dZCalibTemp, 0)
					Array.Resize(rtgset.dZCalibPos, 0)
					Continue Do
				End If
				Select Case iMode
				Case 1
					sBuf = sLineBuf.Split(",")			'温度, Z補正値増分(um)
					Array.Resize(rtgset.dZCalibTemp, idx+1)
					Array.Resize(rtgset.dZCalibPos, idx+1)
					idx = rtgset.dZCalibTemp.GetUpperBound(0)
					rtgset.dZCalibTemp(idx) = Ctype(sBuf(0), Double)
					rtgset.dZCalibPos(idx)	= Ctype(sBuf(1), Double)
					idx += 1
				Case Else
				End Select
			Loop
			If iMode = 1
				bZPosEnable = True
			Else
				bZPosEnable = False
			End If

#end if
            bReadFlg = True
            Exit For
        End If
    Next
	'rtgset.dTTotal = rtgset.dTiming.Sum()			'所要時間をセット
	rtgset.dTTotal = dTotalTime						'所要時間をセット
	If rtgset.strName=""
		bReadFlg = False
	End If

Catch ex As Exception
    Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
    Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try

Return bReadFlg
End Function

'コンボボックスの選択リストに新しい値が含まれていなければ追加したソートしたリストを取得する
Public Shared Function GetSortComboList(ByVal cbx As ComboBox, ByVal sNewVal As String) As String()
    Dim sTemp As String = sNewVal.Trim
    Dim sArry() As String = {sTemp}
    Dim iIdx As Integer = 0

    Try
        If cbx.Items.Count > 0 Then
            For i As Integer = 0 To cbx.Items.Count - 1
                '既に登録済みの値は重複登録しない
                If sTemp <> cbx.Items(i) Then
                    iIdx += 1
                    Array.Resize(sArry, iIdx + 1)
                    sArry(iIdx) = cbx.Items(i)
                End If
            Next i

            'ソート
            Array.Sort(sArry)
        End If

    Catch ex As Exception
        Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
        Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
    End Try

    Return sArry
End Function

'■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
'   プライベート関数
'■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■

''' <summary> INIを読みこみRTGSETの一覧を表示する。 </summary>
Private Function fcDisplayRTGSETList(Optional ByVal sSelPrf As String = "") As Boolean
    Dim iSelIndex As Integer = -1
    Try
        If lvRtgset.SelectedItems.Count > 0 Then
            iSelIndex = lvRtgset.SelectedItems(0).Index
        End If

        lvRtgset.Items.Clear()
        Dim strPrevSpecRtg As String = cboSpecRtgset.Text
        cboSpecRtgset.Items.Clear()
        cboSpecRtgset.Items.Add("")
	    Dim dinfoNum As New IO.DirectoryInfo(Path.Combine(SIZUKA_PATH, p_sEquipName, RTGSET_PATH))
        For Each diSampFile As IO.FileInfo In dinfoNum.GetFiles("*.ini")
            ' App.iniの場所は絶対パスで指定する
            Dim strINIPath As String = diSampFile.FullName
            ' 格納用バッファ
            Dim strSb As System.Text.StringBuilder = Nothing
            strSb = New System.Text.StringBuilder(1024)
            'Me.iLang = GetPrivateProfileInt("Tool", "Language", -1, strINIPath)
            GetPrivateProfileString("RTG", "Name", "", strSb, strSb.Capacity, strINIPath)
            If strSb.ToString <> "" Then
                ' 一覧リストに追加
                lvRtgset.Items.Add(strSb.ToString(), strSb.ToString(), strSb.ToString())

                ' 固定Specページのコンボボックスに追加
                cboSpecRtgset.Items.Add(strSb.ToString())
            End If
        Next
		If lvRtgset.Items.Count > 0		'削除して登録数が０になったときは以降処理はパス
			lvRtgset.Sorting = SortOrder.Ascending
			lvRtgset.Sort()
			Dim sSortTopItem As String = Nothing
			sSortTopItem = lvSortReturnTop(lvRtgset)	'ソート後の先頭アイテム文字列を得る

			' 固定Specページのコンボボックス選択値を復元
			cboSpecRtgset.Text = strPrevSpecRtg

			If iSelIndex >= 0 AndAlso iSelIndex < lvRtgset.Items.Count Then
				lvRtgset.Select()
				lvRtgset.Items(iSelIndex).Selected = True
			Else
				If lvRtgset.Items.Count > 0 Then
					lvRtgset.Select()
					If IsNothing(sSortTopItem)
						lvRtgset.Items(0).Selected = True
					Else
						lvRtgset.Items(sSortTopItem).Selected = True
					End If
				End If
			End If

			If sSelPrf.Length > 0 Then
				lvRtgset.Select()
				lvRtgset.Items(sSelPrf).Selected = True
			End If
		End If
        Return True

    Catch ex As Exception
        Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
        Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
        Return False
    End Try

End Function
Private Function lvSortReturnTop(ByVal lv As ListView) As String
	Dim sAry() As String = Nothing
Try
	Array.Resize(sAry, lv.Items.Count)
	For i=0 To lv.Items.Count - 1
		sAry(i) = lv.Items(i).Text
	Next
	Array.Sort(sAry)

Catch ex As Exception
End Try
	Return sAry(0)
End Function


''' <summary> INIを読みこみRTGSETの内容をDGVに表示する。 </summary>
Private Function fcDisplayRTGSETContents(ByVal strRtgsetName As String, ByRef bCalibOK As Boolean) As Boolean
Try
	Dim dinfoNum As New IO.DirectoryInfo(Path.Combine(SIZUKA_PATH, p_sEquipName, RTGSET_PATH))
    For Each diSampFile As IO.FileInfo In dinfoNum.GetFiles("*.ini")
        ' App.iniの場所は絶対パスで指定する
        Dim strINIPath As String = diSampFile.FullName
        ' 格納用バッファ
        Dim strSb As System.Text.StringBuilder = Nothing
        strSb = New System.Text.StringBuilder(1024)

        Dim dPrevTime As Double = 0.0R
        Dim dCurTime As Double = 0.0R
        Dim iTmp As Integer
        Dim sTemp As String = ""

        GetPrivateProfileString("RTG", "Name", "", strSb, strSb.Capacity, strINIPath)
        If strSb.ToString = strRtgsetName Then
            dgvRtgSetting.Rows.Clear()
			'行ヘッダ文字列
			Dim s() As String = m_sStr(eMes.iTempProHeader).split(",")
			dgvRtgSetting.Columns(0).HeaderCell.Value = s(0)
			dgvRtgSetting.Columns(1).HeaderCell.Value = s(1)
			dgvRtgSetting.Columns(2).HeaderCell.Value = s(2)

            txtRtgsetName.Text = strRtgsetName
            For iRow As Integer = 0 To m_iRTGSETMaxRow - 1
                sTemp = GetIniStrData(strINIPath, "RTG", "TIME" + CStr(iRow))
                If sTemp.Length > 0 Then
                    dgvRtgSetting.Rows.Add()
                Else
                    Exit For
                End If

                GetPrivateProfileString("RTG", "Linear" + CStr(iRow), "0", strSb, strSb.Capacity, strINIPath)
                Integer.TryParse(strSb.ToString(), iTmp)
                If iTmp = 1 Then
                    dgvRtgSetting.Item(RS_COL.LINEAR, iRow).Value = My.Resources.Checkbox
                    dgvRtgSetting.Item(RS_COL.LINEAR, iRow).Tag = True
                Else
                    dgvRtgSetting.Item(RS_COL.LINEAR, iRow).Value = My.Resources.Uncheckbox
                    dgvRtgSetting.Item(RS_COL.LINEAR, iRow).Tag = False
                End If
                GetPrivateProfileString("RTG", "TIME" + CStr(iRow), "0", strSb, strSb.Capacity, strINIPath)
                Double.TryParse(strSb.ToString(), dCurTime)
                If dPrevTime >= dCurTime Then
                    dgvRtgSetting.Item(RS_COL.TIME, iRow).Value = ""
                    dgvRtgSetting.Item(RS_COL.TEMPHP, iRow).Value = ""
                    dgvRtgSetting.Item(RS_COL.SPD, iRow).Value = ""
                Else
                    dPrevTime = dCurTime
                    dgvRtgSetting.Item(RS_COL.TIME, iRow).Value = strSb.ToString()
                    GetPrivateProfileString("RTG", "TEMP" + CStr(iRow), "0", strSb, strSb.Capacity, strINIPath)
                    dgvRtgSetting.Item(RS_COL.TEMPHP, iRow).Value = strSb.ToString()
                    GetPrivateProfileString("RTG", "SENSSPD" + CStr(iRow), "0", strSb, strSb.Capacity, strINIPath)
                    dgvRtgSetting.Item(RS_COL.SPD, iRow).Value = strSb.ToString()
                End If
            Next iRow

			GetPrivateProfileString("RTG",  "Calib", "DIS", strSb, strSb.Capacity, strINIPath)
			Dim s0 As String = strSb.ToString
			'ZPOS キャリブレーションデータの確認
            GetPrivateProfileString("RTG",  "Created Date", "NONE", strSb, strSb.Capacity, strINIPath)
			Dim s1 As String = strSb.ToString
            GetPrivateProfileString("ZPOS CAL",  (dCurTime-1), "NONE", strSb, strSb.Capacity, strINIPath)
			Dim s2 As String = strSb.ToString
			bCalibOK = If(s0="DIS" Or s1="NONE" Or s2="NONE", False, True)
			If Not bCalibOK
				WritePrivateProfileString("RTG", "Calib", "DIS", strINIPath)
			End If
            Exit For
        End If
    Next

Catch ex As Exception
    Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
    Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try

Return True
End Function
''' <summary>
''' Z位置 温度キャリブレーションが正常かチェックする（作成日時存在有無）
''' </summary>
''' <param name="strRtgsetName"></param>
''' <returns></returns>
Public Shared Function fcCheckRTGCalibData(ByVal strRtgsetName As String) As Boolean
Dim sFilePath As String = Nothing
Dim strSb As System.Text.StringBuilder = Nothing
strSb = New System.Text.StringBuilder(1024)
Try
	fcSaveRTGSETContentsRTGPathGet(strRtgsetName, sFilePath)

	'ZPOSキャリブレーション実行が必要か確認
	GetPrivateProfileString("RTG",  "Calib", "DIS", strSb, strSb.Capacity, sFilePath)
	Dim s0 As String = strSb.ToString

	'ZPOS キャリブレーションデータの確認
    GetPrivateProfileString("RTG",  "Created Date", "NONE", strSb, strSb.Capacity, sFilePath)
	Dim s1 As String = strSb.ToString

	Return If(s0="DIS" Or s1="NONE", False, True)

	Return True
Catch ex As Exception
    Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Return False
End Try
End Function

''' <summary> RTGSETの内容をiniに保存する。 </summary>
Public Function fcSaveRTGSETContents(ByVal strRtgsetName As String, Optional ByVal iMode As Int16 = -1, Optional ByVal dTemp As Double = Double.NaN, Optional ByVal dZPos As Double = Double.NaN, Optional ByVal iSec As int32 = 0) As Boolean
Dim bRet As Boolean
Dim sFilePath As String = ""
Try
	Select Case iMode
	Case -1
	'プロファイル部分のみ(上書き)
		'温度プロファイル名から設定ファイルパスを取得する
		fcSaveRTGSETContentsRTGPathGet(strRtgsetName, sFilePath)
		bRet = fcSaveRTGSETContentsRTG(strRtgsetName, sFilePath)
	Case 0, 3
	'Z位置キャリブレーション 1行追加モード
        Dim sWriteLine As String = Nothing
		fcSaveRTGSETContentsRTGPathGet(strRtgsetName, sFilePath)
        Using writer As New IO.StreamWriter(sFilePath, True, System.Text.Encoding.UTF8)
			Select Case iMode
			Case 3
				sWriteLine = "[ZPOS CAL]"
			Case 0
				sWriteLine = iSec.ToString & "=" & dTemp.ToString("F1") & "," & dZPos.ToString()
			End Select
           writer.WriteLine(sWriteLine)
		End Using

'		WritePrivateProfileString("ZPOS CAL", iSec.ToString(), dtemp.ToString("F1") & "," & dZPos.ToString(), sFilePath)
		bRet = true
	Case 1
	'作成日時を追加
		fcSaveRTGSETContentsRTGPathGet(strRtgsetName, sFilePath)
		WritePrivateProfileString("RTG", "Created Date", Now.ToString("yyyy/MM/dd HH:mm:ss"), sFilePath)
	Case 2
	'キャリブレーション完了を保存
		fcSaveRTGSETContentsRTGPathGet(strRtgsetName, sFilePath)
		WritePrivateProfileString("RTG", "Calib", "ENA", sFilePath)
	Case 4
	'ファイル内を全消去後、温度プロファイルのみ書込み
		fcSaveRTGSETContentsRTGPathGet(strRtgsetName, sFilePath)
        Using writer As New IO.StreamWriter(sFilePath, False, System.Text.Encoding.UTF8)
		End Using
		bRet = fcSaveRTGSETContentsRTG(strRtgsetName, sFilePath)

	End Select

Catch ex As Exception
    Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
    Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	bRet = False
End Try
Return bRet
End Function

''' <summary>
''' 温度プロファイル ファイル保存(上書き)
''' </summary>
''' <param name="strRtgsetName"></param>
''' <param name="strFile"></param>
''' <returns></returns>
Private Function fcSaveRTGSETContentsRTG(ByVal strRtgsetName As String, ByVal strFile As String) As Boolean
Try

	Dim strINIPath As String = strFile
	txtRtgsetName.Text = strRtgsetName
	WritePrivateProfileString("RTG", "Name", strRtgsetName, strINIPath)
	Dim iRow As Integer = 0
	For iRow = 0 To dgvRtgSetting.RowCount - 1
		If dgvRtgSetting.Item(RS_COL.LINEAR, iRow).Tag = True Then
			WritePrivateProfileString("RTG", "Linear" + CStr(iRow), "1", strINIPath)
		Else
			WritePrivateProfileString("RTG", "Linear" + CStr(iRow), "0", strINIPath)
		End If

		If IsNothing(dgvRtgSetting.Item(RS_COL.TIME, iRow).Value) = False Then
			WritePrivateProfileString("RTG", "TIME" + CStr(iRow), dgvRtgSetting.Item(RS_COL.TIME, iRow).Value.ToString(), strINIPath)
		Else
			WritePrivateProfileString("RTG", "TIME" + CStr(iRow), "", strINIPath)
		End If

		If IsNothing(dgvRtgSetting.Item(RS_COL.TEMPHP, iRow).Value) = False Then
			WritePrivateProfileString("RTG", "TEMP" + CStr(iRow), dgvRtgSetting.Item(RS_COL.TEMPHP, iRow).Value.ToString(), strINIPath)
		Else
			WritePrivateProfileString("RTG", "TEMP" + CStr(iRow), "", strINIPath)
		End If

		'If IsNothing(dgvRtgSetting.Item(RS_COL.SPD, iRow).Value) = False Then
		'	WritePrivateProfileString("RTG", "SENSSPD" + CStr(iRow), dgvRtgSetting.Item(RS_COL.SPD, iRow).Value.ToString(), strINIPath)
		'Else
		'	WritePrivateProfileString("RTG", "SENSSPD" + CStr(iRow), "", strINIPath)
		'End If
	Next iRow

	If dgvRtgSetting.AllowUserToAddRows = True Then
		iRow = iRow - 1
	End If
	For iRow = iRow To m_iRTGSETMaxRow - 1
		WritePrivateProfileString("RTG", "Linear" + CStr(iRow), "-1", strINIPath)
		WritePrivateProfileString("RTG", "TIME" + CStr(iRow), "", strINIPath)
		WritePrivateProfileString("RTG", "TEMP" + CStr(iRow), "", strINIPath)
		WritePrivateProfileString("RTG", "SENSSPD" + CStr(iRow), "", strINIPath)
	Next


	'WritePrivateProfileString("RTG", "Calib", "ENA", strINIPath)
	WritePrivateProfileString("RTG", "Calib", "DIS", strINIPath)

	Return True
Catch ex As Exception
    Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Return False
End Try
End Function
''' <summary>
''' 温度プロファイル名から設定ファイルパスを取得する
''' </summary>
''' <param name="strRtgsetName">温度プロファイル名</param>
''' <param name="sFilePath">設定ファイルパス</param>
''' <returns></returns>
Public Shared Function fcSaveRTGSETContentsRTGPathGet(ByVal strRtgsetName As String, ByRef sFilePath As String) As Boolean
Try
	Dim dinfoNum As New IO.DirectoryInfo(Path.Combine(SIZUKA_PATH, p_sEquipName, RTGSET_PATH))
	Dim iLastFile As Integer = 1
	' 格納用バッファ
	Dim strSb As System.Text.StringBuilder = Nothing
	strSb = New System.Text.StringBuilder(1024)
	For Each diSampFile As IO.FileInfo In dinfoNum.GetFiles("*.ini")
		' App.iniの場所は絶対パスで指定する
		Dim strCheckingINI As String = diSampFile.FullName
		GetPrivateProfileString("RTG", "Name", "", strSb, strSb.Capacity, strCheckingINI)
		If strSb.ToString = strRtgsetName Then
			sFilePath = strCheckingINI
			Exit For
		End If
		Dim strb As System.Text.StringBuilder = New System.Text.StringBuilder()
		For Each ch As Char In diSampFile.Name
			If "1234567890".IndexOf(ch) >= 0 Then
				strb.Append(ch)
			End If
		Next
		Dim iNum As Integer = 0
		If Integer.TryParse(strb.ToString, iNum) Then
			If iNum >= iLastFile Then
				iLastFile = iNum + 1
			End If
		End If

	Next
	If sFilePath = "" Then
		sFilePath = Path.Combine(SIZUKA_PATH, p_sEquipName, RTGSET_PATH, iLastFile.ToString("00000000")&".ini")
	End If

Catch ex As Exception
    Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Return False
End Try
Return True
End Function

''' <summary> 指定されたRTGSETを削除する。 </summary>
Private Function fcDeleteRTGSET(ByVal strRtgsetName As String) As Boolean
Try
	Dim dinfoNum As New IO.DirectoryInfo(Path.Combine(SIZUKA_PATH, p_sEquipName, RTGSET_PATH))
    Dim strFile As String = ""
    ' 格納用バッファ
    Dim strSb As System.Text.StringBuilder = Nothing
    strSb = New System.Text.StringBuilder(1024)
    For Each diSampFile As IO.FileInfo In dinfoNum.GetFiles("*.ini")
        ' App.iniの場所は絶対パスで指定する
        Dim strCheckingINI As String = diSampFile.FullName
        'Me.iLang = GetPrivateProfileInt("Tool", "Language", -1, strINIPath)
        GetPrivateProfileString("RTG", "Name", "", strSb, strSb.Capacity, strCheckingINI)
        If strSb.ToString = strRtgsetName Then
            strFile = strCheckingINI
            Exit For
        End If
    Next
    If strFile <> "" Then
        My.Computer.FileSystem.DeleteFile(strFile)
    End If

Catch ex As Exception
    Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
    Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try

Return True
End Function

Private Function sbCheckAndSaveRTGSet(ByVal bWarn As Boolean) As String
Dim sRtn As String = ""
Try
    Dim sTemp As String = ""
    '時間が入力されていない行があれば削除
    For iCnt As Integer = dgvRtgSetting.RowCount - 1 To 0 Step -1
        sTemp = GetStr(dgvRtgSetting(RS_COL.TIME, iCnt).Value)
        If sTemp.Length = 0 AndAlso dgvRtgSetting.Rows(iCnt).IsNewRow = False Then
            dgvRtgSetting.Rows.RemoveAt(iCnt)
        End If
    Next

    '一行も設定されていないときはエラー
    sTemp = GetStr(dgvRtgSetting.Item(RS_COL.TIME, 0).Value)

    If dgvRtgSetting.Rows(0).IsNewRow = True AndAlso _
        sTemp.Length = 0 Then
        Return ""
    End If
    '最大保存件数チェック
    Dim iRowNum As Integer = dgvRtgSetting.RowCount
    If dgvRtgSetting.Rows(dgvRtgSetting.RowCount - 1).IsNewRow = True Then
        '最終行が新規行の場合はカウントしない
        iRowNum -= 1
    End If
    If iRowNum > m_iRTGSETMaxRow Then
        Dim sMsg As String = m_sStr(eMes.iTEMPPROF_MAX_OVER)
        sMsg = Replace(sMsg, "#VALUE#", CStr(iRowNum - m_iRTGSETMaxRow))

        Call MsgBox(sMsg, MsgBoxStyle.OkOnly Or MsgBoxStyle.Exclamation, "Temperature Profile")
        Return ""
    End If

    ' 最初のセルが空欄の場合、値を1とする。
    FixDGVNothing(0)
    If dgvRtgSetting.Item(RS_COL.TIME, 0).Value.ToString = "" Then
        dgvRtgSetting.Item(RS_COL.TIME, 0).Value = "1"
    End If

    Dim bCheck As Boolean = False
    '時間、温度が入力されていない行は詰める
    For iRow = dgvRtgSetting.RowCount - 1 To 0 Step -1
        '空セルは空文字に置き換え
        FixDGVNothing(iRow)

        '時間が入力されている場合、0を表示
        If IsNumeric(dgvRtgSetting.Item(RS_COL.TIME, iRow).Value) = True AndAlso _
            IsNumeric(dgvRtgSetting.Item(RS_COL.TEMPHP, iRow).Value) = False Then
            dgvRtgSetting.Item(RS_COL.TEMPHP, iRow).Value = 0
        End If

        'チェック開始OFF
        If bCheck = False Then
            '温度または時間が入力されている場合
            If IsNumeric(dgvRtgSetting.Item(RS_COL.TIME, iRow).Value) = True OrElse _
                IsNumeric(dgvRtgSetting.Item(RS_COL.TEMPHP, iRow).Value) = True Then
                bCheck = True
            End If
        Else
            'チェック開始ON
            '温度、時間共に入力されていない
            If IsNumeric(dgvRtgSetting.Item(RS_COL.TIME, iRow).Value) = False AndAlso _
                IsNumeric(dgvRtgSetting.Item(RS_COL.TEMPHP, iRow).Value) = False Then
                dgvRtgSetting.Rows.RemoveAt(iRow)
                dgvRtgSetting.Rows.Add(My.Resources.Uncheckbox, "", "", "")
            End If
        End If

    Next

    ' 温度値が正常値であるかを確認。時間値が上行以下の場合は確認処理を抜ける
    Dim bValid As Boolean = True
    Dim dPrevTime As Double = -1
    Dim dCurTime As Double = 0.0R
    For iRow As Integer = 0 To dgvRtgSetting.RowCount - 1
        FixDGVNothing(iRow)
        Double.TryParse(dgvRtgSetting.Item(RS_COL.TIME, iRow).Value, dCurTime)
        If IsNumeric(dgvRtgSetting.Item(RS_COL.TIME, iRow).Value) = True AndAlso _
            (dPrevTime >= dCurTime OrElse dCurTime <= 0) Then

            bValid = False
            sRtn = m_sStr(eMes.INVALID_INPUT_VALUE)
            sRtn = sRtn & vbCrLf & " -Row(" & CStr(iRow + 1) & "):" & m_sStr(eMes.iRTG_TIMEERR) & vbCrLf
            Exit For
        End If
        dPrevTime = dCurTime
    Next iRow
    '' 英数字と一部の文字のみ名称に指定可能とする
    If txtRtgsetName.Text.Length = 0 Then
        sRtn = sRtn & m_sStr(eMes.iRTGNAME_NOTHING) & vbCrLf
    End If
    ' 入力値が適切な場合、RTGSETを保存する
    If txtRtgsetName.Text <> "" AndAlso bValid = True Then
        fcSaveRTGSETContents(txtRtgsetName.Text)
        fcDisplayRTGSETList(txtRtgsetName.Text)
    ElseIf bWarn = True Then
        MessageBox.Show(sRtn, frmMain.MesBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning)
    End If

Catch ex As Exception
    Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
    Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try

Return sRtn
End Function

'----------------------------------------------------------------------------------------------------------------

''' <summary> dgvRtgSettingのセル値がNothingの場合、空白文字とする。 </summary>
''' <param name="iRow">修正対象の行。</param>
Private Sub FixDGVNothing(ByVal iRow As Integer)
Try
    If IsNothing(dgvRtgSetting.Item(RS_COL.TIME, iRow).Value) Then
        dgvRtgSetting.Item(RS_COL.TIME, iRow).Value = ""
    End If
    If IsNothing(dgvRtgSetting.Item(RS_COL.TEMPHP, iRow).Value) Then
        dgvRtgSetting.Item(RS_COL.TEMPHP, iRow).Value = ""
    End If
    If IsNothing(dgvRtgSetting.Item(RS_COL.SPD, iRow).Value) Then
        dgvRtgSetting.Item(RS_COL.SPD, iRow).Value = ""
    End If

Catch ex As Exception
    Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
    Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub

'色選択クリックイベント
Private Sub sblblClr_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    Dim iIndex As Integer
    Dim iColor As Integer
    Dim iTemp As Integer

    Try
        'クリックされた配列のIndexを取得
        iIndex = Array.IndexOf(m_lblClr, sender)

        '色を数値に変換
        iColor = System.Drawing.ColorTranslator.ToOle(m_lblClr(iIndex).BackColor)

        'カラーダイアログを表示し、色を選択
        iTemp = OpenColorDialog(iColor)
        '色が選択されていない場合、戻す
        If iTemp < 0 Then
            iTemp = iColor
        End If

    Catch ex As Exception
        Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
        Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
        iTemp = iColor
    End Try

    '表示色の更新
    m_lblClr(iIndex).BackColor = System.Drawing.ColorTranslator.FromOle(iTemp)
    m_lblClr(iIndex).ForeColor = GetForeColor(m_lblClr(iIndex).BackColor)
End Sub

'スペックフィルター適用
Private Sub sbApplyFixSpecFilter()
    Try
        'スペックが登録されている場合
        If p_sAllFixSpecName.Count > 0 Then
            '固定スペック名選択リストビューをクリア
            lvSpec.Items.Clear()

            'フィルタ種別の選択
            Dim sResSpec As List(Of String)
            If txtFixFilter.Text.Length > 0 Then
                Select Case btnFixFilter.Tag
                    '前方一致
                    Case eFIX_FILTER.iPREFIX
                        sResSpec = From SpecName In p_sAllFixSpecName _
                                    Where SpecName.ToUpper.StartsWith(txtFixFilter.Text.ToUpper) _
                                    Order By SpecName Ascending.ToList

                        '部分一致
                    Case Else
                        sResSpec = From SpecName In p_sAllFixSpecName _
                                    Where SpecName.ToUpper.Contains(txtFixFilter.Text.ToUpper) _
                                    Order By SpecName Ascending.ToList
                End Select
            Else
                '検索条件なし
                sResSpec = From SpecName In p_sAllFixSpecName _
                            Order By SpecName.ToUpper Ascending.ToList
            End If

            'リストビューの再設定
            For Each sName As String In sResSpec
                lvSpec.Items.Add(sName, sName, sName)
            Next
        End If

    Catch ex As Exception
        Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
        Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
    End Try
End Sub
'数値かどうかを判定(数値でない場合-99を返す)
Private Function fnIsNumeric(ByVal sNum As String, Optional ByVal sDelm As String = "") As Integer
    Dim iRtn As Integer = 0

    Try
        '区切り文字が指定されている
        If sDelm.Length > 0 Then
            '区切り文字で分割
            Dim sArry() As String = Split(sNum, sDelm)

            '数値でない場合-99を返す
            For Each sStr As String In sArry
                If IsNumeric(Trim(sStr)) = False Then
                    iRtn = -99
                    Exit For
                End If
            Next
        Else
            '数値でない場合-99を返す
            If IsNumeric(sNum) = False Then
                iRtn = -99
            End If
        End If

    Catch ex As Exception
        Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
        Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
        iRtn = -99
    End Try

    Return iRtn
End Function

'本体情報による表示/非表示
Private Sub sbSetVisEna(ByVal iBodySet As Integer, ByVal iBodyVer As Integer, ByVal iOrgSens As Integer, ByVal iPrtcType As Integer)
    Try
        '全て表示、有効
        gbxMdkSet.Visible = True            'インターロック表示
        nudStaMonTime.Visible = True        'ゼロ調強制頻度
        gbxMdkSet.Enabled = True            'インターロック有効
        gbxZeroAdj.Enabled = True           'ゼロ調整設定
        cbxEnaIntLock.Checked = False       'インターロックON
        gbxCentering.Visible = True             'キャリブ機能
        gbxCentering.Enabled = True             'キャリブ機能

        Select Case iBodySet
            Case eMDK_BODY_TYPE.iNOT_NEW        '新メモリ非対応
                'インターロック、毎回ゼロ調非表示
                gbxMdkSet.Visible = False
                nudStaMonTime.Visible = False
            Case eMDK_BODY_TYPE.iOFFLINE        'OFFLIINE
                'インターロック、毎回ゼロ調不可
                gbxMdkSet.Enabled = False
                gbxZeroAdj.Enabled = False      'ゼロ調整設定
            Case eMDK_BODY_TYPE.iINTERLOCK_ON   'インターロックON
                '全て有効
                cbxEnaIntLock.Checked = True    'インターロックON
            Case eMDK_BODY_TYPE.iINTERLOCK_OFF  'インターロックOFF
                cbxEnaIntLock.Checked = False
        End Select

        'OFFLINEでないとき
        If iBodySet <> eMDK_BODY_TYPE.iOFFLINE Then
            '原点センサが無効の場合
            If iOrgSens <= 0 Then
                gbxCentering.Visible = False
            End If
        Else
            gbxCentering.Enabled = False
        End If
    Catch ex As Exception
        Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
        Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
    End Try
End Sub
''' <summary> INIを読みこみ温度プロファイルの内容を取得する。 </summary>
Private Function fnGetIniTempProfData() As tpTempProfData()
    Dim uRtn As New List(Of tpTempProfData)
    Dim uTmpData As tpTempProfData = Nothing

    Try
        Dim sTemp As String
        Dim sPath As String
        Dim sItem As String
        Dim bNum As Boolean

        '温度プロファイルリストのファイル一覧を取得
        ''Dim dinfoNum As New IO.DirectoryInfo(My.Application.Info.DirectoryPath.ToString() + "\rtgset\")
        Dim dinfoNum As New IO.DirectoryInfo(Path.Combine(SIZUKA_PATH, p_sEquipName, RTGSET_PATH))
        Dim fiFileLst As IO.FileInfo() = dinfoNum.GetFiles("*.ini")

        'リストから設定名取得
        For Each lviRtgsName As ListViewItem In lvRtgset.Items
            sPath = ""
            'フォルダ内のIniファイル探索
            For Each fiFile As IO.FileInfo In fiFileLst
                'Iniファイルから設定名取得
                sTemp = GetIniStrData(fiFile.FullName, "RTG", "Name", "")

                '設定名が一致
                If sTemp = lviRtgsName.Text Then
                    uTmpData.sConfName = sTemp
                    sPath = fiFile.FullName
                    Exit For
                End If
            Next

            '一致フラグON
            If sPath.Length > 0 Then
                '構造体にデータを格納
                With uTmpData
                    ReDim .uPointData(0 To m_iRTGSETMaxRow - 1)

                    '変化点データを取得
                    For iRow As Integer = 0 To m_iRTGSETMaxRow - 1
                        With .uPointData(iRow)
                            sItem = "Linear" & iRow.ToString()
                            sTemp = GetIniStrData(sPath, "RTG", sItem, "", bNum)
                            Integer.TryParse(sTemp, .iLenear)

                            sItem = "TIME" & iRow.ToString()
                            .sTime = GetIniStrData(sPath, "RTG", sItem, "", bNum)

                            sItem = "TEMP" & iRow.ToString()
                            .sTemphp = GetIniStrData(sPath, "RTG", sItem, "", bNum)

                            sItem = "SENSSPD" & iRow.ToString()
                            .sSpd = GetIniStrData(sPath, "RTG", sItem, "", bNum)
                        End With
                    Next iRow
                End With

                uRtn.Add(uTmpData)
            End If
        Next

    Catch ex As Exception
        Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
        Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
        MsgBox(m_sStr(eMes.iEXPORT_FAIL), MsgBoxStyle.OkOnly Or MsgBoxStyle.Exclamation, "Export")
        uRtn.Clear()
    End Try

    Return uRtn.ToArray
End Function

'温度プロファイルデータをファイルに出力
Private Function sbExportTempProf(ByVal sPath As String, ByVal uTempProfData() As tpTempProfData) As Boolean
    Try

        Dim sWriteLine As String
        Using writer As New IO.StreamWriter(sPath, False, System.Text.Encoding.UTF8)
            '温度プロファイルの出力
            For iCnt As Integer = 0 To uTempProfData.Length - 1
                With uTempProfData(iCnt)
                    'プロファイル名出力
                    sWriteLine = Replace(lblTempProfileName.Text, ":", "") & "," & .sConfName
                    writer.WriteLine(sWriteLine)

                    '列名出力
                    sWriteLine = "," & _
                                    dgvRtgSetting.Columns(RS_COL.LINEAR).HeaderText & "," & _
                                    dgvRtgSetting.Columns(RS_COL.TIME).HeaderText & "," & _
                                    dgvRtgSetting.Columns(RS_COL.TEMPHP).HeaderText & ","
                    writer.WriteLine(sWriteLine)

                    'データ出力
                    For iRow As Integer = 0 To m_iRTGSETMaxRow - 1
                        With .uPointData(iRow)
                            If .sTime.Length > 0 Then
                                sWriteLine = CStr(iRow + 1) & "," & _
                                                .iLenear.ToString() & "," & _
                                                .sTime & "," & _
                                                .sTemphp & ","

                                writer.WriteLine(sWriteLine)
                            Else
                                writer.WriteLine(CStr(iRow + 1) & ",0,,,")
                            End If
                        End With
                    Next iRow
                    writer.WriteLine("")
                End With

            Next
        End Using
        Return True

    Catch ex As Exception
        Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
        Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
        Return False
    End Try
End Function

'温度プロファイルデータをファイルから取得
Private Function sbImportTempProf(ByVal sPath As String) As tpTempProfData()
    Dim uRtn As New List(Of tpTempProfData)
    Dim uTmpData As tpTempProfData = Nothing
    Try
        'テキストファイルを開く
        Dim bs As Byte() = System.IO.File.ReadAllBytes(sPath)

        '文字コードを判別する
        Dim enc As System.Text.Encoding = DetectEncodingFromBOM(bs)

        If enc Is Nothing Then
            'BOMが見つからない場合はSJISとして開く
            enc = System.Text.Encoding.UTF8
        Else
            'BOMが見つかった場合はBOMが付加したUnicodeかUTF-8として開く
        End If

        Dim sRowData() As String
        Using parser As New FileIO.TextFieldParser(sPath, enc)
            parser.TextFieldType = FileIO.FieldType.Delimited
            parser.SetDelimiters(",") ' 区切り文字は","
            parser.TrimWhiteSpace = True

            Dim sName As String = ""
            Dim iDataCnt As Integer
            Dim iPreDataCnt As Integer = -1
            While Not parser.EndOfData
                'CSVファイルから一行読み込み
                sRowData = parser.ReadFields()
                'データが1つ以上分割可能
                If sRowData.Length > 0 Then
                    '数値変換可能な場合(プロファイルデータ行)
                    If IsNumeric(sRowData(0)) = True AndAlso sRowData.Length >= 4 Then
                        'プロファイル名が取得できている場合
                        If sName.Length > 0 Then
                            'データ番号取得
                            iDataCnt = CInt(sRowData(0))
                            '初回番号が取得できた場合
                            If iDataCnt = 1 Then
                                'プロファイル名格納
                                uTmpData.sConfName = sName
                                'データ配列確保
                                ReDim uTmpData.uPointData(0 To m_iRTGSETMaxRow - 1)
                                '前回データ番号初期化
                                iPreDataCnt = 0
                            End If

                            'データ番号が1ずつ増加している場合、データを格納
                            If iDataCnt <= m_iRTGSETMaxRow AndAlso iDataCnt = iPreDataCnt + 1 Then
                                With uTmpData.uPointData(iPreDataCnt)
                                    '数値に変換できない列があれば格納終了
                                    For iCnt As Integer = 1 To 3
                                        If IsNumeric(sRowData(iCnt)) = False Then
                                            sName = ""
                                            Exit For
                                        End If
                                    Next

                                    '入力に問題なければ、データ格納
                                    If sName.Length >= 0 Then
                                        .iLenear = CInt(sRowData(1))
                                        .sTime = sRowData(2)
                                        .sTemphp = sRowData(3)
                                    End If
                                End With

                                '前回データ番号の更新
                                iPreDataCnt = iDataCnt
                            End If
                        End If
                    Else
                        'プロファイル名取得
                        If sRowData(0).Length > 0 AndAlso IsNumeric(sRowData(0)) = False AndAlso _
                            sRowData.Length > 1 AndAlso sRowData(1).Length > 0 Then
                            sName = sRowData(1)

                            'データ格納
                            If iPreDataCnt > 0 Then
                                'データを格納
                                uRtn.Add(uTmpData)
                            End If
                        End If
                    End If
                End If
            End While

            'データを格納
            uRtn.Add(uTmpData)
        End Using

    Catch ex As Exception
        Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
        Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
        uRtn.Clear()
    End Try

    Return uRtn.ToArray
End Function

'温度プロファイルデータをIniファイルに保存
Private Function fcSaveTempProfData(ByVal uTempProfData() As tpTempProfData) As Boolean
    Dim sPath As String = ""
    Dim sPathTmp As String = ""
    Dim sTemp As String
    ''Dim sDir As String = My.Application.Info.DirectoryPath.ToString() + "\rtgset\"
    Dim sDir As String = Path.Combine(SIZUKA_PATH, p_sEquipName, RTGSET_PATH)
    Dim dinfoNum As IO.DirectoryInfo
    Dim iFileNum As Integer = 1
    Dim sItem As String
    Dim sErrItem As String = ""

    Try
        For Each uProfTmp As tpTempProfData In uTempProfData
            With uProfTmp
                sPath = ""
                dinfoNum = New IO.DirectoryInfo(sDir)
                For Each fiFile As IO.FileInfo In dinfoNum.GetFiles("*.ini")
                    'Iniファイルから設定名取得
                    sTemp = GetIniStrData(fiFile.FullName, "RTG", "Name", "")

                    '設定名が一致
                    If sTemp = .sConfName Then
                        sPath = fiFile.FullName
                        Exit For
                    End If
                Next

                'ファイルパスが取得できなかった場合、新規パスを設定
                If sPath.Length = 0 Then
                    While iFileNum < 99999999
                        sPath = Path.Combine(sDir, iFileNum.ToString("00000000") & ".ini")
                        If System.IO.File.Exists(sPath) = False Then
                            Exit While
                        End If
                        sPath = ""
                        iFileNum = iFileNum + 1
                    End While
                End If

                'ファイルパスが取得できた場合
                If sPath.Length > 0 Then
                    sPathTmp = sPath
                    sPath = sPathTmp & ".bak"

                    '設定名を書き込み
                    sItem = "Name"
                    SetIniData(sPath, "RTG", sItem, .sConfName)

                    '変化点データを書き込み
                    For iRow As Integer = 0 To m_iRTGSETMaxRow - 1
                        With .uPointData(iRow)
                            '線形
                            sItem = "Linear" & iRow.ToString()
                            SetIniData(sPath, "RTG", sItem, .iLenear.ToString())
                            '時間
                            sItem = "TIME" & iRow.ToString()
                            SetIniData(sPath, "RTG", sItem, .sTime)
                            '温度
                            sItem = "TEMP" & iRow.ToString()
                            SetIniData(sPath, "RTG", sItem, .sTemphp)
                            '速度
                            sItem = "SENSSPD" & iRow.ToString()
                            SetIniData(sPath, "RTG", sItem, .sSpd)
                        End With
                    Next iRow

                    'SetIniData(sPath, "RTG", "SensWave", m_mssEditing.iWavekind.ToString())
                    'SetIniData(sPath, "RTG", "SensPeriod", m_mssEditing.dPeriod.ToString())
                    'SetIniData(sPath, "RTG", "SensAmp", m_mssEditing.dAmp.ToString())

                    '設定に問題ない場合、ファイルを上書き
                    If sErrItem.Length = 0 Then
                        '既存のファイルを削除し、「.bak」ファイルをリネーム
                        If DeleteFile(sPathTmp) = False OrElse _
                            RenameFile(sPath, sPathTmp, True) = False _
                        Then
                            sErrItem = "File Copy Error.(" & sPathTmp & ")"
                        End If
                    Else
                        sErrItem = Strings.Left(sErrItem, sErrItem.Length - 1)
                        sErrItem = "Error Item:" & sErrItem
                    End If
                Else
                    sErrItem = "number of files upper limit error."
                End If

                '正常にファイルが作成できなかった
                If sErrItem.Length > 0 Then
                    sErrItem = sErrItem & vbCrLf

                    MsgBox("Name:" & .sConfName & vbCrLf & sErrItem & vbCrLf & m_sStr(eMes.iIMPORT_FAIL), MsgBoxStyle.OkOnly Or MsgBoxStyle.Exclamation, "Import")
                    Call DeleteFile(sPath)
                    Return False
                End If
            End With
        Next

        Return True

    Catch ex As Exception
        Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
        Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
        MsgBox(m_sStr(eMes.iIMPORT_FAIL), MsgBoxStyle.OkOnly Or MsgBoxStyle.Exclamation, "Import")
        Return False
    End Try
End Function

'日時設定 NOWボタン
Private Sub btnNow_Click(sender As Object, e As EventArgs) Handles btnNow.Click
        sbNowDateSet()
End Sub
'現在日時をコントロールへセット
Private sub sbNowDateSet()
    Dim dDate As Date = Now()
    nudYear.Value = dDate.Year
    nudMonth.Value = dDate.Month
    nudDay.Value = dDate.Day

    nudHour.Value = dDate.Hour
    nudMinute.Value = dDate.Minute
    nudSecond.Value = dDate.Second
End sub
'日時設定 変更ボタン
Private Sub btnDateAply_Click(sender As Object, e As EventArgs) Handles btnDateAply.Click

    Try
	If p_RTM(0).p_iBGWOrder = BGW_ORDER.NO_ORDER
        Dim d As String = cint(nudYear.Value).ToString("D4") & cint(nudMonth.Value).ToString("D2") & cint(nudDay.Value).tostring("D2") & _
            cint(nudHour.Value).ToString("D2") & cint(nudMinute.Value).ToString("D2") & cint(nudSecond.Value).tostring("D2")
        Dim f As string = "yyyyMMddHHmmss"
        Dim dDate As Date = DateTime.ParseExact(  d, f, nothing)
        p_RTM(0).p_dDevDateSet = dDate
        p_RTM(0).p_iBGWOrder = BGW_ORDER.SETDATE_REP
	End If
    Catch ex As Exception
        Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
        Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
    End Try
End Sub
'原点復帰ボタン
Private Sub btnOrigin_Click(sender As Object, e As EventArgs) Handles btnOrigin.Click
Try
    Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name, eLOG_LEVEL.iINFO)

    if Not p_RTM(0).p_mltcp.isStatMeasuringTempo()
'		If p_RTM(0).p_mltcp.isILState()　Or p_RTM(0).p_mltcp.isILDisable			'IL(扉閉) OKか? IL無効も見る
       		'if p_RTM(0).p_mltcp.IsPosOrderOrigin()
			''OFFする
			'    p_RTM(0).p_mltcp.PosOrderOrigin(False)									    '原点駆動指令BIT OFF
			'Else
			'ONする
           		if p_RTM(0).p_iBGWOrder = BGW_ORDER.NO_ORDER then
					p_RTM(0).p_iBGWOrder = BGW_ORDER.ORG_BUTTON_REP
				End If
       		'End If
'	  	End If
  	End If

Catch ex As Exception
    Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
    Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub
'手動駆動ボタン
Private Sub btnHotPlateDrive_Click(sender As Object, e As EventArgs) Handles btnHotPlateDrive.Click, btnXAxisDrive.Click, btnYAxisDrive.Click
Try
    if Not p_RTM(0).p_mltcp.isStatMeasuringTempo()
'		If p_RTM(0).p_mltcp.isILState()　Or p_RTM(0).p_mltcp.isILDisable			'IL(扉閉) OKか? IL無効も見る
			select case sender.name
			Case "btnHotPlateDrive"
       			if p_RTM(0).p_mltcp.IsPosOrderZ()
				'OFFする
					p_RTM(0).p_mltcp.PosOrderZ(False)									'Z駆動指令BIT OFF
				Else
				'ONする
           			if p_RTM(0).p_iBGWOrder = BGW_ORDER.NO_ORDER then
						p_RTM(0).p_iHotPlatePos = nudHeatPos.Value * 1000
						p_RTM(0).p_iBGWOrder = BGW_ORDER.HOT_PLATE_DRV_REP
					End If
       			End If

			Case "btnXAxisDrive"
       			if p_RTM(0).p_mltcp.IsPosOrderX()
				'OFFする
					p_RTM(0).p_mltcp.PosOrderX(False)									'X駆動指令BIT OFF
				Else
				'ONする
           			if p_RTM(0).p_iBGWOrder = BGW_ORDER.NO_ORDER then
						p_RTM(0).p_iXAxisPos = nudXAxisPos.Value
						p_RTM(0).p_iBGWOrder = BGW_ORDER.X_AXIS_DRV_REP
					End If
       			End If
			Case "btnYAxisDrive"
       			if p_RTM(0).p_mltcp.IsPosOrderY()
				'OFFする
					p_RTM(0).p_mltcp.PosOrderY(False)									'X駆動指令BIT OFF
				Else
				'ONする
           			if p_RTM(0).p_iBGWOrder = BGW_ORDER.NO_ORDER then
						p_RTM(0).p_iYAxisPos = nudYAxisPos.Value
						p_RTM(0).p_iBGWOrder = BGW_ORDER.Y_AXIS_DRV_REP
					End If
       			End If
			End Select
'	    End If
    End If
Catch ex As Exception
    Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
    Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub
''' <summary>
''' XY動作モード駆動開始/停止ボタン
''' </summary>
Private Sub btnXYDrive_Click(sender As Object, e As EventArgs) Handles btnXYDrive.Click, btnXYStop.Click
Try
    if Not p_RTM(0).p_mltcp.isStatMeasuringTempo()
		If p_RTM(0).p_mltcp.isILState()　Or p_RTM(0).p_mltcp.isILDisable			'IL(扉閉) OKか? IL無効も見る
			if p_RTM(0).p_iBGWOrder = BGW_ORDER.NO_ORDER then
				Select Case sender.name
				Case "btnXYDrive"
					p_RTM(0).p_iMAmplitudeX =  nudXAmp.Value
					p_RTM(0).p_iMAmplitudeY =  nudYAmp.Value
					p_RTM(0).p_iMAmplitudeZ =  nudZAmp.Value
					p_RTM(0).p_iMPeriodX    =  nudXPeriod.Value
					p_RTM(0).p_iMPeriodY    =  nudYPeriod.Value
					p_RTM(0).p_iMPeriodZ    =  nudZPeriod.Value
					p_RTM(0).p_iBGWOrder	= BGW_ORDER.XYMODE_DRIVE_START
					btnOK.Enabled		= False
					btnApply.Enabled	= False
					btnCancel.Enabled	= False
				Case "btnXYStop"
					p_RTM(0).p_iBGWOrder = BGW_ORDER.XYMODE_DRIVE_STOP
					btnOK.Enabled		= True
					btnApply.Enabled	= True
					btnCancel.Enabled	= True
				End Select
				p_RTM(0).p_iModeForManualXY = 2
			End If
	    End If
    End If
Catch ex As Exception
    Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
    Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub
''' <summary>
''' 本体装置番号の書込み
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub btnDeviceSerWite_Click(sender As Object, e As EventArgs) Handles btnDeviceSerWite.Click
Try
    if p_RTM(0).p_iBGWOrder = BGW_ORDER.NO_ORDER then
        For i As Integer = 0 To 9
            If dgvEquipList.Item(EL_COL.USEEQUIP, i).Tag
				p_RTM(0).p_DevSerialWrite = dgvEquipList.Item(EL_COL.NAME, i).Value.substring(3)
				p_RTM(0).p_iBGWOrder = BGW_ORDER.SETPRO_REP

				'dgvEquipList.Item(EL_COL.NAME, i).ReadOnly = True
				btnDeviceSerWite.Visible = False

				Call WriteLog("詳細設定 装置情報登録ボタン 成功", eLOG_LEVEL.iINFO)

				MsgBox("Device ID regist OK!",vbInformation+vbOKOnly)
				Exit For
            End If
        Next i
    End If
Catch ex As Exception
    Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
    Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	MsgBox("Device ID regist Failed!",vbInformation+vbOKOnly)
End Try
End Sub

''' <summary>
''' 100％応力較正画面表示
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub btnFullCalib_Click(sender As Object, e As EventArgs) Handles btnFullCalib.Click
	Dim frmC As New frmCalibFull(0)
    frmC.Showdialog()
	frmC.Dispose()
End Sub
''' <summary>
''' ゼロ応力較正画面表示
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub btnZeroCalib_Click(sender As Object, e As EventArgs) Handles btnZeroCalib.Click
	Dim frmC As New frmCalibFull(1)
    frmC.Showdialog()
	frmC.Dispose()
End Sub

''' <summary>
''' 重量変換校正画面表示
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub Button1_Click(sender As Object, e As EventArgs) Handles btnWeightCalib.Click
    frmCalibWeight3.Showdialog
End Sub

''' <summary>
''' フィルタパラメータファイル選択ボタン
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub btnFilterSelect_Click(sender As Object, e As EventArgs) Handles btnFilterSelect.Click
    With OpenFileDialog1
        .InitialDirectory = p_dtFile.p_FilterPath       'フィルタファイル場所
        .DefaultExt = "txt"
        .FileName = ""
        If .ShowDialog() = DialogResult.OK
            If .CheckFileExists 
                lblFilterFileName.Text = Path.GetFileNameWithoutExtension(.FileName)
            Else
            End If
        End if
    End With
End Sub

''' <summary>
''' フィルタ有効無効
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub cbFilterEnable_CheckedChanged(sender As Object, e As EventArgs)  Handles cbxFilterEnable.CheckedChanged
Try
    m_ctsMainFrm.iFilterEnable = iif(cbxFilterEnable.Checked, 1, 0)
    gbxFilterFIR.Enabled =  cbxFilterEnable.Checked
    gbxFilterIIR.Enabled =  cbxFilterEnable.Checked
    rbFIR.Enabled = cbxFilterEnable.Checked
    rbIIR.Enabled = cbxFilterEnable.Checked
    If cbxFilterEnable.Checked
        rbFIR_CheckedChanged(sender, e)
    Else
        gbxFilterFIR.BackColor = Control.DefaultBackColor
        gbxFilterIIR.BackColor = Control.DefaultBackColor
    End If
Catch ex As Exception
End Try
End Sub

Private Sub rbFIR_CheckedChanged(sender As Object, e As EventArgs) Handles rbFIR.CheckedChanged
    gbxFilterFIR.Enabled = rbFIR.Checked
    gbxFilterIIR.Enabled = rbIIR.Checked
	If cbxFilterEnable.Checked
		If rbFIR.Checked
			gbxFilterFIR.BackColor = Color.Yellow
			gbxFilterIIR.BackColor = Control.DefaultBackColor
		else
			gbxFilterIIR.BackColor = Color.Yellow
			gbxFilterFIR.BackColor = Control.DefaultBackColor
		End If
	Else
		gbxFilterIIR.BackColor = Control.DefaultBackColor
		gbxFilterFIR.BackColor = Control.DefaultBackColor
	End If
End Sub
'水平確認処理画面表示ボタン
Private Sub btnHorizonConfirm_Click(sender As Object, e As EventArgs) Handles btnHorizonConfirm.Click
	frmHorConf.Show()
End Sub
'本体電源ON積算時間リセット
Private Sub btnPowOnTimeReset_Click(sender As Object, e As EventArgs) Handles btnPowOnTimeReset.Click
    p_RTM(0).p_iBGWOrder = BGW_ORDER.RESET_INTEG_POWON
End Sub
'HPヒータON積算時間リセット
Private Sub btnHeatOnTimeReset_Click(sender As Object, e As EventArgs) Handles btnHeatOnTimeReset.Click
    p_RTM(0).p_iBGWOrder = BGW_ORDER.RESET_INTEG_HEATON
End Sub
'XYZ動作積算時間リセット
Private Sub btnXYActTimeReset_Click(sender As Object, e As EventArgs) Handles btnXYActTimeReset.Click
    p_RTM(0).p_iBGWOrder = BGW_ORDER.RESET_INTEG_XYACT
End Sub
'PBヒータON積算時間リセット
Private Sub btnProbeHeaterTimeReset_Click(sender As Object, e As EventArgs) Handles btnProbeHeaterTimeReset.Click
    p_RTM(0).p_iBGWOrder = BGW_ORDER.RESET_INTEG_HEATON2
End Sub
'SGヒータON積算時間リセット
Private Sub btnSGHeaterTimeReset_Click(sender As Object, e As EventArgs) Handles btnSGHeaterTimeReset.Click
    p_RTM(0).p_iBGWOrder = BGW_ORDER.RESET_INTEG_HEATON3
End Sub
'FIX->FREE設定コピー
Private Sub DgvFixToFreeCopy()
#If Not NEW_SPEC
	Dim dgv As DataGridView = Nothing
	Dim cmb As ComboBox = Nothing
	Dim tbx As TextBox	= Nothing
Try
	If txtSpecName.Text = vbNullString
		Return
	End If
	cmb = frmMain.cboMeasNameItem()
	cmb.Text				= txtSpecName.Text					                                            '樹脂名
	cmb = frmMain.cboSmplNameItem()
	cmb.Text				= Now.ToString("yyyyMMdd") 				                                        'ロット
	'cmb = frmMain.cbxMeasureItem()
	'cmb.Text				= ""                					                                        '測定者
	cmb = frmMain.cboMethodItem()
	cmb.Text				= Me.cboMethod.Text                           								    '計算方法

	dgv = frmMain.dgvSpecItem
	dgv.Item(1, eSPEC_ROW.iTEMPR).Value			= dgvSpec.Item( 1, eSPECO_ROW.iTEMPR ).Value			'HP温度
	dgv.Item(1, eSPEC_ROW.iTEMPR2).Value		= dgvSpec.Item( 1, eSPECO_ROW.iTEMPR2 ).Value			'PB温度
	dgv.Item(1, eSPEC_ROW.iTRQ).Value			= dgvSpec.Item( 1, eSPECO_ROW.iTRQ ).Value				'判定閾値
	dgv.Item(1, eSPEC_ROW.iINP_GAP).Value		= dgvSpec.Item( 1, eSPECO_ROW.iINP_GAP ).Value			'入力GAP
	dgv.Item(1, eSPEC_ROW.iGAP).Value			= dgvSpec.Item( 1, eSPECO_ROW.iGAP ).Value				'測定GAP
	dgv.Item(1, eSPEC_ROW.iXAmp).Value			= dgvSpec.Item( 1, eSPECO_ROW.iXAmp ).Value				'X振幅
	dgv.Item(1, eSPEC_ROW.iYAmp).Value			= dgvSpec.Item( 1, eSPECO_ROW.iYAmp ).Value				'Y振幅
	dgv.Item(1, eSPEC_ROW.iXPeriod).Value		= dgvSpec.Item( 1, eSPECO_ROW.iXPeriod ).Value			'X周期
	dgv.Item(1, eSPEC_ROW.iYPeriod).Value		= dgvSpec.Item( 1, eSPECO_ROW.iYPeriod ).Value			'Y周期
	dgv.Item(1, eSPEC_ROW.iWAIT).Value			= dgvSpec.Item( 1, eSPECO_ROW.iWAIT ).Value				'HP上昇待ち時間
	dgv.Item(1, eSPEC_ROW.iCLEAR).Value			= dgvSpec.Item( 1, eSPECO_ROW.iCLEAR ).Value			'投入待ち時間
	dgv.Item(1, eSPEC_ROW.iXYWAIT).Value		= dgvSpec.Item( 1, eSPECO_ROW.iXYWAIT ).Value			'XY動作待ち時間
	dgv.Item(1, eSPEC_ROW.iY_MAX).Value			= dgvSpec.Item( 1, eSPECO_ROW.iY_MAX ).Value			'Y軸表示範囲
	dgv.Item(1, eSPEC_ROW.iEND_COND).Value		= dgvSpec.Item( 1, eSPECO_ROW.iEND_COND ).Value			'終了条件-変化率
	dgv.Item(1, eSPEC_ROW.iEND_STRS).Value		= dgvSpec.Item( 1, eSPECO_ROW.iEND_STRS ).Value			'終了条件-応力
	dgv.Item(1, eSPEC_ROW.iEND_TIME).Value		= dgvSpec.Item( 1, eSPECO_ROW.iEND_TIME ).Value			'終了条件-時間
	dgv.Item(1, eSPEC_ROW.iTEMPR_TOLER).Value	= dgvSpec.Item( 1, eSPECO_ROW.iTEMPR_TOLER ).Value		'HP温度有効範囲

	tbx = frmMain.txtStirRodItem()
	tbx.Text									= dgvSpec.Item( 1, eSPECO_ROW.iPROBE ).Value			'センサヘッド(直径)
	tbx = frmMain.txtAmountItem()
	tbx.Text									= dgvSpec.Item( 1, eSPECO_ROW.iAppendix ).Value			'付加情報

	dgv = frmMain.dgvCalcOptItem
	dgv.Item(1, eCALC_ROW.iMASK_RANGE).Value	= dgvCalcOpt.Item( 1, eCALC_ROW.iMASK_RANGE ).Value	'有効範囲
	dgv.Item(1, eCALC_ROW.iGEL_RANGE).Value	    = dgvCalcOpt.Item( 1, eCALC_ROW.iGEL_RANGE ).Value	'判定範囲
    dgv.Item(1, eCALC_ROW.iRTG).Value           = cboSpecRtgset.Text                                '温度プロファイル
Catch ex As Exception
    Call FrmDebugLog(System.Reflection.MethodBase.GetCurrentMethod.Name & ex.Message)
    Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
#end if
End Sub
#If Not NEW_SPEC
'FREE->FIX設定コピー
Private Sub DgvFreeToFixCopy()
	Dim dgv As DataGridView = Nothing
	Dim cmb As ComboBox = Nothing
	Dim tbx As TextBox	= Nothing
Try
	''Me.cboXYModeOp.Text     = 2					                                                    '動作モード(XYモード固定)
	cmb = frmMain.cboMethodItem()
	cboMethod.Text			= cmb.Text		                             							'計算方法

	dgv = frmMain.dgvSpecItem
	dgvSpec.Item( 1, eSPECO_ROW.iTEMPR ).Value				= dgv.Item(1, eSPEC_ROW.iTEMPR).Value  			'HP温度
	dgvSpec.Item( 1, eSPECO_ROW.iTEMPR2 ).Value				= dgv.Item(1, eSPEC_ROW.iTEMPR2).Value  		'PB温度
	dgvSpec.Item( 1, eSPECO_ROW.iTRQ ).Value				= dgv.Item(1, eSPEC_ROW.iTRQ).Value             '判定閾値
	dgvSpec.Item( 1, eSPECO_ROW.iINP_GAP ).Value			= dgv.Item(1, eSPEC_ROW.iINP_GAP).Value 		'入力GAP
	dgvSpec.Item( 1, eSPECO_ROW.iGAP ).Value				= dgv.Item(1, eSPEC_ROW.iGAP).Value				'測定GAP
	dgvSpec.Item( 1, eSPECO_ROW.iXAmp ).Value				= dgv.Item(1, eSPEC_ROW.iXAmp).Value			'X振幅
	dgvSpec.Item( 1, eSPECO_ROW.iYAmp ).Value				= dgv.Item(1, eSPEC_ROW.iYAmp).Value			'Y振幅
	dgvSpec.Item( 1, eSPECO_ROW.iXPeriod ).Value			= dgv.Item(1, eSPEC_ROW.iXPeriod).Value			'X周期
	dgvSpec.Item( 1, eSPECO_ROW.iYPeriod ).Value			= dgv.Item(1, eSPEC_ROW.iYPeriod).Value			'Y周期
	dgvSpec.Item( 1, eSPECO_ROW.iWAIT ).Value				= dgv.Item(1, eSPEC_ROW.iWAIT).Value			'HP上昇待ち時間
	dgvSpec.Item( 1, eSPECO_ROW.iCLEAR ).Value				= dgv.Item(1, eSPEC_ROW.iCLEAR).Value  			'投入待ち時間
	dgvSpec.Item( 1, eSPECO_ROW.iXYWAIT ).Value				= dgv.Item(1, eSPEC_ROW.iXYWAIT).Value 			'XY動作待ち時間
	dgvSpec.Item( 1, eSPECO_ROW.iY_MAX ).Value				= dgv.Item(1, eSPEC_ROW.iY_MAX).Value      		'Y軸表示範囲
	dgvSpec.Item( 1, eSPECO_ROW.iEND_COND ).Value			= dgv.Item(1, eSPEC_ROW.iEND_COND).Value 		'終了条件-変化率
	dgvSpec.Item( 1, eSPECO_ROW.iEND_STRS ).Value			= dgv.Item(1, eSPEC_ROW.iEND_STRS).Value		'終了条件-応力
	dgvSpec.Item( 1, eSPECO_ROW.iEND_TIME ).Value			= dgv.Item(1, eSPEC_ROW.iEND_TIME).Value 		'終了条件-時間
	dgvSpec.Item( 1, eSPECO_ROW.iTEMPR_TOLER ).Value		= dgv.Item(1, eSPEC_ROW.iTEMPR_TOLER).Value		'HP温度有効範囲
	dgvSpec.Item( 1, eSPECO_ROW.iTEMPR2_TOLER ).Value		= dgv.Item(1, eSPEC_ROW.iTEMPR2_TOLER).Value	'PB温度有効範囲

	tbx = frmMain.txtStirRodItem()
	dgvSpec.Item( 1, eSPECO_ROW.iPROBE ).Value   = tbx.Text                           						'センサヘッド
	tbx = frmMain.txtAmountItem()
	dgvSpec.Item( 1, eSPECO_ROW.iAppendix ).Value = tbx.Text                                				'付加情報

	dgv = frmMain.dgvCalcOptItem
	dgvCalcOpt.Item( 1, eCALC_ROW.iMASK_RANGE ).Value   = dgv.Item(1, eCALC_ROW.iMASK_RANGE).Value 	'有効範囲
	dgvCalcOpt.Item( 1, eCALC_ROW.iGEL_RANGE ).Value    = dgv.Item(1, eCALC_ROW.iGEL_RANGE).Value 	'判定範囲
    cboSpecRtgset.Text                                  = dgv.Item(1, eCALC_ROW.iRTG).Value         '温度プロファイル
Catch ex As Exception
    Call FrmDebugLog(System.Reflection.MethodBase.GetCurrentMethod.Name & ex.Message)
    Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub
'FIX->FREE設定コピー
Private Sub btnSpecCopyFree_Click(sender As Object, e As EventArgs) Handles btnSpecCopyFree.Click
    DgvFixToFreeCopy()
End Sub
'FREE->FIX設定コピー
Private Sub Button2_Click(sender As Object, e As EventArgs) Handles btnSpecCopyFix.Click
    DgvFreeToFixCopy()
End Sub
#end if
'PID操作量モニタ
Private Sub Button3_Click(sender As Object, e As EventArgs) Handles btnPIDMonitor.Click
	Call WriteLog("温度パラメータ 温度モニタオープン", eLOG_LEVEL.iINFO)

    frmTempAdjMon.ShowDialog()
	'PIDパラメータの表示更新
	nudP.Value = m_ctsMainFrm.TempCnt_P
	nudI.Value = m_ctsMainFrm.TempCnt_I
	nudD.Value = m_ctsMainFrm.TempCnt_D
	nudDgain.Value = m_ctsMainFrm.TempCnt_Dgain
	nudPVBias.Value = m_ctsMainFrm.TempCnt_PVBias
	nudPVGain.Value = m_ctsMainFrm.TempCnt_PVGain
	
	nudP2.Value = m_ctsMainFrm.TempCnt_P2
	nudI2.Value = m_ctsMainFrm.TempCnt_I2
	nudD2.Value = m_ctsMainFrm.TempCnt_D2
	nudDgain2.Value = m_ctsMainFrm.TempCnt_Dgain2
	nudPVBias2.Value = m_ctsMainFrm.TempCnt_PVBias2
	nudPVGain2.Value = m_ctsMainFrm.TempCnt_PVGain2

	nudP3.Value = m_ctsMainFrm.TempCnt_P3
	nudI3.Value = m_ctsMainFrm.TempCnt_I3
	nudD3.Value = m_ctsMainFrm.TempCnt_D3
	nudDgain3.Value = m_ctsMainFrm.TempCnt_Dgain3
	nudPVBias3.Value = m_ctsMainFrm.TempCnt_PVBias3
	nudPVGain3.Value = m_ctsMainFrm.TempCnt_PVGain3
	Me.Invalidate()
	Call WriteLog("温度パラメータ 温度モニタクローズ", eLOG_LEVEL.iINFO)
End Sub

''' <summary>
''' 隠しキーコマンド
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub frmOptions_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
	If (e.Modifiers And Keys.Control) = Keys.Control Then
		Select Case e.KeyCode
		Case Keys.M					
		'装置番号書き込みボタン表示 Ctrl+M
			dgvEquipList.Columns(EL_COL.NAME).ReadOnly		= Not dgvEquipList.Columns(EL_COL.NAME).ReadOnly
			dgvEquipList.Columns(EL_COL.XYSENS).ReadOnly	= Not dgvEquipList.Columns(EL_COL.XYSENS).ReadOnly
			dgvEquipList.Columns(EL_COL.ZSENS).ReadOnly		= Not dgvEquipList.Columns(EL_COL.ZSENS).ReadOnly
			btnDeviceSerWite.Visible						= Not btnDeviceSerWite.Visible

			'隠し設定表示/非表示
			gpxHiddenSetting.Visible						= Not gpxHiddenSetting.Visible

			Call WriteLog("詳細設定 隠し画面表示/非表示", eLOG_LEVEL.iINFO)

		Case Keys.K
		'CTRL＋K デバッグ画面
			frmDebug.Show()

		End Select
	End If
End Sub
'出荷時のデバッグファイル出力許可
Private Sub cbxDebugFileEn_CheckedChanged(sender As Object, e As EventArgs) Handles cbxDebugFileEn.CheckedChanged
	p_bDebugFileOutEnable = cbxDebugFileEn.Checked
End Sub
'測定中の値変更許可
Private Sub cbxInputEnMeasureing_CheckedChanged(sender As Object, e As EventArgs) Handles cbxInputEnMeasureing.CheckedChanged
	p_bInputEnMeasureing = cbxInputEnMeasureing.Checked
End Sub
'キャリブレーション値のキー入力許可
Private Sub cbxCalibValChangeEn_CheckedChanged(sender As Object, e As EventArgs) Handles cbxCalibValChangeEn.CheckedChanged
	p_bCalibValChangeEn = cbxCalibValChangeEn.Checked
End Sub
'Z安定度チェックキャンセル
Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles cbxZStabDis.CheckedChanged
	p_bZStrStabDis = cbxZStabDis.Checked
End Sub
'PC生存確認チェック有効無効
Private Sub cbxPcLifeChkDis_CheckedChanged(sender As Object, e As EventArgs) Handles cbxPcLifeChkDis.CheckedChanged
	p_bPcLifeCheckDis = cbxPcLifeChkDis.Checked
End Sub
'測定時間の制限を外す
Private Sub cbxMeasTimeEn_CheckedChanged(sender As Object, e As EventArgs) Handles cbxMeasTimeEn.CheckedChanged
	p_bMeasTimeEn = cbxMeasTimeEn.Checked
End Sub
'温度制限を外す
Private Sub cbxTempNoLimitEn_CheckedChanged(sender As Object, e As EventArgs) Handles cbxTempNoLimitEn.CheckedChanged
	p_bTempNoLimitEn = cbxTempNoLimitEn.Checked
End Sub
'通信スキャン時間表示を許可
Private Sub cbxScanTimeEn_CheckedChanged(sender As Object, e As EventArgs) Handles cbxScanTimeEn.CheckedChanged
	p_bScanTimeEn = cbxScanTimeEn.Checked
End Sub
'装置リスト編集を許可
Private Sub cbxEquipListEn_CheckedChanged(sender As Object, e As EventArgs) Handles cbxEquipListEn.CheckedChanged
	p_bEquipListEn = cbxEquipListEn.Checked
	GroupBox10.Enabled = Not GroupBox10.Enabled
	dgvEquipList.EnableHeadersVisualStyles = False
	dgvEquipList.ColumnHeadersDefaultCellStyle.BackColor	= If(p_bEquipListEn,System.Drawing.SystemColors.Window,System.Drawing.SystemColors.Control)
	dgvEquipList.DefaultCellStyle.BackColor					= If(p_bEquipListEn,System.Drawing.SystemColors.Window,System.Drawing.SystemColors.Control)
	dgvEquipList.EnableHeadersVisualStyles = True

End Sub
'バッチ計算後の計算結果のファイル出力を有効
Private Sub cbxCalcResultFOutEn_CheckedChanged(sender As Object, e As EventArgs) Handles cbxCalcResultFOutEn.CheckedChanged
	p_bCalcResultFOutEn = cbxCalcResultFOutEn.Checked
End Sub
''' <summary>
''' 隠しコマンド 現在の設定を工場出荷時へバックアップする
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub btnCreateFactorySetting_Click(sender As Object, e As EventArgs) Handles btnCreateFactorySetting.Click
Dim iRet As Int32 = 0
Try
	Call WriteLog("工場出荷時設定へバックアップがクリックされました。", eLOG_LEVEL.iINFO)
	iRet = BackupFactorySettings()
	Select Case iRet
	Case 0
		MsgBox("Backup successful." , MsgBoxStyle.Information + MsgBoxStyle.OkOnly,  System.Windows.Forms.Application.ProductName)
	Case Else
		MsgBox("Backup failed." , MsgBoxStyle.Critical + MsgBoxStyle.OkOnly,  System.Windows.Forms.Application.ProductName)
	End Select
Catch ex As Exception
    Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub
''' <summary>
''' 工場出荷時設定を作成する 
''' FactorySettingフォルダ内のファイルを上書きコピー
''' </summary>
''' <returns></returns>
Private Function BackupFactorySettings() As Int32
Dim sFactPath As String = Path.Combine(SIZUKA_PATH, p_sEquipName, FACTORY_PATH)
Try
	If p_sEquipName.Substring(0,3)<>"SZK"
		Return -2
	End If
#if 0
	'設定ファイル確認
	'フル応力設定ファイル
	If Not File.Exists(Path.Combine(sFactPath, CALDAT_PATH, p_sFullStoreFileName))
		Return -2
	End If
	'ゼロ応力設定ファイル
	If Not File.Exists(Path.Combine(sFactPath, CALDAT_PATH, p_sZeroStoreFileName))
		Return -2
	End If
	'重量設定ファイル
	If Not File.Exists(Path.Combine(sFactPath, CALDAT_PATH, p_sStoreFileNameWeight))
		Return -2
	End If
	'デバイス設定初期化ファイル
	If Not File.Exists(Path.Combine(sFactPath, DevSettingFile))
		Return -2
	End If
#If 0
	'温度制御パラメータ初期化ファイル
	If Not File.Exists(Path.Combine(sFactPath, TempParaFile))
		Return -2
	End If
#end if
#end if
	'工場出荷時フォルダ無ければ作成
	If Not Directory.Exists(sFactPath)
		Directory.CreateDirectory(sFactPath)
	End If
	Dim sSrcPath As String
	Dim sDstPath As String
	'キャリブレーションファイルコピー
	sSrcPath = Path.Combine(SIZUKA_PATH, p_sEquipName, CALDAT_PATH)
	sDstPath = 	Path.Combine(sFactPath, CALDAT_PATH)
	My.Computer.FileSystem.CopyDirectory(sSrcPath, sDstPath, True)

	'デバイス設定初期化ファイルコピー
	sSrcPath = Path.Combine(SIZUKA_PATH, p_sEquipName, DevSettingFile)
	sDstPath = Path.Combine(sFactPath, DevSettingFile)
	File.Copy(sSrcPath, sDstPath, True)
#If 1
	'温度制御パラメータ初期化ファイルコピー
	sSrcPath = Path.Combine(SIZUKA_PATH, p_sEquipName, TempParaFile)
	sDstPath = Path.Combine(sFactPath, TempParaFile)
	File.Copy(sSrcPath, sDstPath, True)
#end if

	Return 0
Catch ex As Exception
    Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Return -1
End Try
End Function





''' <summary>
''' 温度プロファイル Z位置更正マップ作成
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Public Sub btnZPosCalib_Click(sender As Object, e As EventArgs) Handles btnZPosCalib.Click
Dim bEnable As Boolean = False
Dim lRet As int32
	If p_cts.p_ZCalibTempPro = -1

		Call WriteLog("温度プロファイル キャリブレーション開始 - " & txtRtgsetName.Text, eLOG_LEVEL.iINFO)
		'温度プロファイル Z位置キャリブレーション用にスレッド開始
		If frmMain.bgwInterrupt.IsBusy = False Then
			frmMain.bgwInterrupt.RunWorkerAsync()							'温度プロファイルスレッド開始
		End If

		'選択されている温度プロファイルを読込む
		p_RTM(0).p_strRTGSetName = txtRtgsetName.Text		
		'温度プロファイルファイルを初期化
		fcSaveRTGSETContents(txtRtgsetName.Text, 4)

		' ゼロ調完了時に移動 p_RTM(0).sbLoadRTGSetting(p_RTM(0).p_strRTGSetName, bEnable)		'温度プロファイル開始

        'ゼロ調開始
		lRet = frmMain.ZeroAdj()
		If lRet = 0
			p_cts.p_ZCalibTempPro = 1
		End If
	Else
		p_RTM(0).p_iBGWOrder = BGW_ORDER.ZERO_BUTTON
		p_cts.p_ZCalibTempPro = -1
	End If
End Sub

'温度プロファイル名変更制御 -----
Dim sProNameGotFocus As String = ""
Private Sub txtRtgsetName_GotFocus(sender As Object, e As EventArgs) Handles txtRtgsetName.GotFocus
	sProNameGotFocus = txtRtgsetName.Text
End Sub
Private Sub txtRtgsetName_LostFocus(sender As Object, e As EventArgs) Handles txtRtgsetName.LostFocus
	If sProNameGotFocus <> txtRtgsetName.Text
		If txtRtgsetName.Text.Contains(",")
			txtRtgsetName.Text = txtRtgsetName.Text.Replace(",", "_")
		End If
		iRtgEditSave = 1
		btnSaveRtgs.Enabled = True
		sbDispCalibOK(False)				'キャリブレーション済み消す
	Else
		iRtgEditSave = 0
		btnSaveRtgs.Enabled = False
	End If
End Sub
Private Sub txtRtgsetName_KeyDown(sender As Object, e As KeyEventArgs) Handles txtRtgsetName.KeyDown
	iRtgEditSave = 1
	btnSaveRtgs.Enabled = True
	If e.KeyCode = Keys.Enter
		If Not IsNothing(txtRtgsetName)
			If txtRtgsetName.Text.Contains(",")
				txtRtgsetName.Text = txtRtgsetName.Text.Replace(",", "_")
			End If
		End If
	End If
End Sub

	'--------------------------------
''' <summary>
''' 工場出荷時設定ボタン
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub btnFactorySetting_Click(sender As Object, e As EventArgs) Handles btnFactorySetting.Click
Dim iRet As int32
Try
	Call WriteLog("工場出荷時設定がクリックされました。", eLOG_LEVEL.iINFO)
	Dim dlgF As New frmMSG(m_sStr(eMes.FACTORY_CONF), {m_sStr(eMes.MSG_YES), m_sStr(eMes.MSG_NO)},  System.Windows.Forms.Application.ProductName, Me, 1)
	dlgF.ShowDialog()
	Select Case dlgF.p_iRes
	Case 1	'OK
		iRet = RestoreFactorySettings()
		Select Case iRet
		Case 0
			Call WriteLog("工場出荷時設定が実行成功しました。", eLOG_LEVEL.iINFO)
			Dim dlgM As New frmMSG(m_sStr(eMes.FACTORY_SUCCESS), {"OK"},  System.Windows.Forms.Application.ProductName, Me)
			dlgM.ShowDialog()
			dlgM.Dispose()
			'適用ボタンが押されたとして詳細設定自動クローズ
			p_bDlgApply = True		
			Me.Close()
		Case Else
			Call WriteLog("工場出荷時設定が実行失敗しました。", eLOG_LEVEL.iINFO)
			Dim dlgM As New frmMSG(m_sStr(eMes.FACTORY_FAILE), {"OK"},  System.Windows.Forms.Application.ProductName, Me)
			dlgM.ShowDialog()
			dlgM.Dispose()
		End Select
	Case Else
	End Select
	dlgF.Dispose()
Catch ex As Exception
    Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub
''' <summary>
''' 工場出荷時に設定を戻す 
''' FactorySettingフォルダ内のファイルを上書きコピー
''' それらを全て読込む
''' </summary>
''' <returns></returns>
Private Function RestoreFactorySettings() As Int32
Dim sFactPath As String = Path.Combine(SIZUKA_PATH, p_sEquipName, FACTORY_PATH)
Try
	If p_sEquipName.Substring(0,3)<>"SZK"
		Return 0
	End If
	'設定ファイル確認
	'フル応力設定ファイル
	If Not File.Exists(Path.Combine(sFactPath, CALDAT_PATH, p_sFullStoreFileName))
		Return -2
	End If
	'ゼロ応力設定ファイル
	If Not File.Exists(Path.Combine(sFactPath, CALDAT_PATH, p_sZeroStoreFileName))
		Return -2
	End If
	'重量設定ファイル
	If Not File.Exists(Path.Combine(sFactPath, CALDAT_PATH, p_sStoreFileNameWeight))
		Return -2
	End If
	'デバイス設定初期化ファイル
	If Not File.Exists(Path.Combine(sFactPath, DevSettingFile))
		Return -2
	End If
#If 0
	'温度制御パラメータ初期化ファイル
	If Not File.Exists(Path.Combine(sFactPath, TempParaFile))
		Return -2
	End If
#end if

	Dim sSrcPath As String
	Dim sDstPath As String
	'キャリブレーションファイルコピー
	sSrcPath = Path.Combine(sFactPath, CALDAT_PATH)
	sDstPath = Path.Combine(SIZUKA_PATH, p_sEquipName, CALDAT_PATH)
	My.Computer.FileSystem.CopyDirectory(sSrcPath, sDstPath, True)

	'デバイス設定初期化ファイルコピー
	sSrcPath = Path.Combine(sFactPath, DevSettingFile)
	sDstPath = Path.Combine(SIZUKA_PATH, p_sEquipName, DevSettingFile)
	File.Copy(sSrcPath, sDstPath, True)
#If 0
	'温度制御パラメータ初期化ファイルコピー
	sSrcPath = Path.Combine(sFactPath, TempParaFile)
	sDstPath = Path.Combine(SIZUKA_PATH, p_sEquipName, TempParaFile)
	File.Copy(sSrcPath, sDstPath, True)
#end if

	'全てのファイルを再読み込み
	Call p_cts.LoadDeviceSettingINI(p_sEquipName)				'装置毎設定読込
#If 0
	Call p_cts.LoadTempParaINI(p_sEquipName)					'温調パラメータ読込
#end if
	Call frmMain.CalibFileRead()								'キャリブレーションファイル読込


	Return 0
Catch ex As Exception
    Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Return -1
End Try
End Function

''' <summary>
''' サンプル名・測定者リスト登録ボタン
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub btnSamplenameResist_Click(sender As Object, e As EventArgs) Handles btnSamplenameResist.Click, btnMeasurerResist.Click
Dim sVal As String = Nothing
Dim iCase As Int16
Try
	iCase = If(sender.Name.contains("Sample"), 0, 1)
	Select Case iCase
	Case 0
		sVal = cbxSampleName.Text
	Case 1
		sVal = cbxMeasurer.Text
	End Select
	If sVal.Contains(",")
		sVal = sVal.Replace(",","_")
	End If
	If Not sVal.Contains("'") AndAlso Not sVal.Contains("\") AndAlso Not sVal.Contains("""")
		If sVal.Length>0
			Select Case iCase
			Case 0
				If Array.IndexOf(p_cts.p_sSampleAcum, sVal) < 0
				'既にある場合は追加しない
					Array.Resize(p_cts.p_sSampleAcum, p_cts.p_sSampleAcum.Length+1)
					p_cts.p_sSampleAcum(p_cts.p_sSampleAcum.GetUpperBound(0)) = sVal
					cbxSampleName.Items.Clear
					cbxSampleName.Items.AddRange(p_cts.p_sSampleAcum)
					cbxSampleName.Text = sVal
				End If
				cbxSampleName.BackColor = Color.White
			Case 1
				If Array.IndexOf(p_cts.p_sMeasurerAcum, sVal) < 0
				'既にある場合は追加しない
					Array.Resize(p_cts.p_sMeasurerAcum, p_cts.p_sMeasurerAcum.Length+1)
					p_cts.p_sMeasurerAcum(p_cts.p_sMeasurerAcum.GetUpperBound(0)) = sVal
					cbxMeasurer.Items.Clear
					cbxMeasurer.Items.AddRange(p_cts.p_sMeasurerAcum)
					cbxMeasurer.Text = sVal
				End If
				cbxMeasurer.BackColor = Color.White
			End Select
		End If
	Else
		Select Case iCase
		Case 0
			cbxSampleName.BackColor = Color.Yellow
		Case 1
			cbxMeasurer.BackColor = Color.Yellow
		End Select
	End If

	p_cts.InputSelectionFileSave(p_sEquipName)

Catch ex As Exception
    Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub

Private Sub cbxSampleName_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbxSampleName.SelectedIndexChanged
	sender.BackColor = Color.White
End Sub

Private Sub cbxMeasurer_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbxMeasurer.SelectedIndexChanged
	sender.BackColor = Color.White
End Sub

''' <summary>
''' サンプル名・測定者リスト削除ボタン
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Private Sub btnSamplenameDelete_Click(sender As Object, e As EventArgs) Handles btnSamplenameDelete.Click, btnMeasurerDelete.Click
Dim sVal As String = Nothing
Dim iCase As Int16
Try
	iCase = If(sender.Name.contains("Sample"), 0, 1)
	Select Case iCase
	Case 0
		sVal = cbxSampleName.Text
	Case 1
		sVal = cbxMeasurer.Text
	End Select
	If sVal.Length>0
		Select Case iCase
		Case 0
			p_cts.p_sSampleAcum = p_cts.p_sSampleAcum.Where(Function(value) value <> sVal).ToArray()
			cbxSampleName.Items.Clear
			cbxSampleName.Items.AddRange(p_cts.p_sSampleAcum)
			cbxSampleName.SelectedIndex = 0
		Case 1
			p_cts.p_sMeasurerAcum = p_cts.p_sMeasurerAcum.Where(Function(value) value <> sVal).ToArray()
			cbxMeasurer.Items.Clear
			cbxMeasurer.Items.AddRange(p_cts.p_sMeasurerAcum)
			cbxMeasurer.SelectedIndex = 0
		End Select
	End If

	p_cts.InputSelectionFileSave(p_sEquipName)

Catch ex As Exception
    Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub

Private Sub cbxLongMeasMode_CheckedChanged(sender As Object, e As EventArgs) Handles cbxLongMeasMode.CheckedChanged
	gbxLongMeasMode.BackColor = If(cbxLongMeasMode.Checked, Color.Yellow, Control.DefaultBackColor)
End Sub

Private Sub cbxQRCodeEnable_CheckedChanged(sender As Object, e As EventArgs) Handles cbxQRCodeEnable.CheckedChanged
	gbxQrCode.BackColor = If(cbxQRCodeEnable.Checked, Color.Yellow, Control.DefaultBackColor)
End Sub

Private Sub cbxAutoReg_CheckedChanged(sender As Object, e As EventArgs) Handles cbxAutoReg.CheckedChanged
	gbxAutoReg.BackColor = If(cbxAutoReg.Checked, Color.Yellow, Control.DefaultBackColor)
End Sub

Private Sub cbxSPC_CheckedChanged(sender As Object, e As EventArgs) Handles cbxSPC.CheckedChanged
	gbxSPC.BackColor = If(cbxSPC.Checked, Color.Yellow, Control.DefaultBackColor)
End Sub

'初期値からの変更監視（P,I,D,Dgain）
Private Sub nudP_ValueChanged(sender As Object, e As EventArgs) Handles nudP.ValueChanged
	nudP.BackColor = If(nudP.Value <> cToolSettings.cdP, Color.Yellow, Color.White)
End Sub
Private Sub nudI_ValueChanged(sender As Object, e As EventArgs) Handles nudI.ValueChanged
	nudI.BackColor = If(nudI.Value <> cToolSettings.cdI, Color.Yellow, Color.White)
End Sub
Private Sub nudD_ValueChanged(sender As Object, e As EventArgs) Handles nudD.ValueChanged
	nudD.BackColor = If(nudD.Value <> cToolSettings.cdD, Color.Yellow, Color.White)
End Sub
Private Sub nudDgain_ValueChanged(sender As Object, e As EventArgs) Handles nudDgain.ValueChanged
	nudDgain.BackColor = If(nudDgain.Value <> cToolSettings.cdDgain, Color.Yellow, Color.White)
End Sub
Private Sub nudP2_ValueChanged(sender As Object, e As EventArgs) Handles nudP2.ValueChanged
	nudP2.BackColor = If(nudP2.Value <> cToolSettings.cdP2, Color.Yellow, Color.White)
End Sub
Private Sub nudI2_ValueChanged(sender As Object, e As EventArgs) Handles nudI2.ValueChanged
	nudI2.BackColor = If(nudI2.Value <> cToolSettings.cdI2, Color.Yellow, Color.White)
End Sub
Private Sub nudD2_ValueChanged(sender As Object, e As EventArgs) Handles nudD2.ValueChanged
	nudD2.BackColor = If(nudD2.Value <> cToolSettings.cdD2, Color.Yellow, Color.White)
End Sub
Private Sub nudDgain2_ValueChanged(sender As Object, e As EventArgs) Handles nudDgain2.ValueChanged
	nudDgain2.BackColor = If(nudDgain2.Value <> cToolSettings.cdDgain2, Color.Yellow, Color.White)
End Sub
Private Sub nudP3_ValueChanged(sender As Object, e As EventArgs) Handles nudP3.ValueChanged
	nudP3.BackColor = If(nudP3.Value <> cToolSettings.cdP3, Color.Yellow, Color.White)
End Sub
Private Sub nudI3_ValueChanged(sender As Object, e As EventArgs) Handles nudI3.ValueChanged
	nudI3.BackColor = If(nudI3.Value <> cToolSettings.cdI3, Color.Yellow, Color.White)
End Sub
Private Sub nudD3_ValueChanged(sender As Object, e As EventArgs) Handles nudD3.ValueChanged
	nudD3.BackColor = If(nudD3.Value <> cToolSettings.cdD3, Color.Yellow, Color.White)
End Sub
Private Sub nudDgain3_ValueChanged(sender As Object, e As EventArgs) Handles nudDgain3.ValueChanged
	nudDgain3.BackColor = If(nudDgain3.Value <> cToolSettings.cdDgain3, Color.Yellow, Color.White)
End Sub

'初期値への変更監視(PVBias,PVGain)
Private Sub nudPVBias_ValueChanged(sender As Object, e As EventArgs) Handles nudPVBias.ValueChanged
	nudPVBias.BackColor = If(nudPVBias.Value = cToolSettings.cdPVBias, Color.Yellow, Color.White)
End Sub
Private Sub nudPVGain_ValueChanged(sender As Object, e As EventArgs) Handles nudPVGain.ValueChanged
	nudPVGain.BackColor = If(nudPVGain.Value = cToolSettings.cdPVGain, Color.Yellow, Color.White)
End Sub
Private Sub nudPVBias2_ValueChanged(sender As Object, e As EventArgs) Handles nudPVBias2.ValueChanged
	nudPVBias2.BackColor = If(nudPVBias2.Value = cToolSettings.cdPVBias2, Color.Yellow, Color.White)
End Sub
Private Sub nudPVGain2_ValueChanged(sender As Object, e As EventArgs) Handles nudPVGain2.ValueChanged
	nudPVGain2.BackColor = If(nudPVGain2.Value = cToolSettings.cdPVGain2, Color.Yellow, Color.White)
End Sub

Private Sub nudStrainStab_ValueChanged(sender As Object, e As EventArgs) Handles nudStrainStab.ValueChanged
	nudStrainStab.BackColor = If(nudStrainStab.Value = cToolSettings.cStressStab , Color.Yellow, Color.White)
End Sub
Private Sub nudGapCorr_ValueChanged(sender As Object, e As EventArgs) Handles nudGapCorr.ValueChanged
	nudGapCorr.BackColor = If(nudGapCorr.Value = cToolSettings.cGapCorr , Color.Yellow, Color.White)
End Sub
End Class