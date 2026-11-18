Imports System
Imports System.IO
Imports System.Numerics
Imports System.Runtime.InteropServices
Imports System.Text
Imports System.Windows
Imports System.Windows.Forms.DataVisualization.Charting
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
Private iAutoStartValue As Integer = 0
''' <summary> 自動開始の待ち時間。未使用 </summary>
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
Private m_bAdminEna		As Boolean = True
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
'データベース選択  0:SQLServer 1:SQLite
Private m_iDBSel		As int16 = 1
Public Property DBSel() As Int16
	Get
		DBSel = m_iDBSel
	End Get
	Set(Val As int16)
		m_iDBSel = Val
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
    WritePrivateProfileString("Calculation",	"DB Selection",				Me.m_iDBSel,				strINIPath)
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
	iEna = GetPrivateProfileInt("Tool",	"Lock",			1,		strINIPath)
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
        Me.m_iDBSel				= GetPrivateProfileInt(		"Calculation", "DB Selection",				1,		strINIPath)
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
