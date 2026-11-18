Imports System.Data.SQLite
Imports System.IO
Imports System.Runtime.CompilerServices
Imports System.Runtime.Remoting.Activation
Imports Microsoft.Office.Interop.Excel

Public Class clSizukaDB_SQLite
Public p_DBCon As New SqLiteConnection
Public p_DBPath				As String = "C:\SZKTool\DB"
Public p_SQLiteDBFileName	As String	="SizukaDB.db"
'データベース接続先設定
Private m_ConnString		As String = "Data Source=" & Path.Combine(p_DBPath, p_SQLiteDBFileName) & ";Version=3;"
'測定データ結果ヘッダテーブル
Public p_tblResults As String = "tblResults"
Public p_sRltName() As String                   'tblResultsテーブルフィールド名
Public p_sRltNameAt() As String                 'tblResultsテーブルフィールド名@付き

'tblResultsテーブルフィールド順番
Public Enum eRst
	'1
	EquipID = 0
	DateTime
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
	FileName
	MeasureType
	CalculationMethod
	StressThreshold
	SLOPEDiffInterval
	GelTime
	GelTimeJudge
	GelTimeTemp
	GelTimeVisco
	Filter
	OOSValX                         '脱調確認結果 X (um)
	OOSValY                         '脱調確認結果 Y (um)
	OOSValZ                         '脱調確認結果 Z (um)
	CPUCommErrCnt                   'CPU間通信エラーカウント
	'4
	ToolVersion
	EquipVersion
	XYSensID
	ZSensID

	IntegTimePowOn                  '本体ON積算時間[秒]
	IntegTimeHeatOn                 'HP ON積算時間[秒]
	IntegTimePBHeat                 'PB ON積算時間[秒]
	IntegTimeSGHeat                 'SG ON積算時間[秒]
	IntegTimeXAct                   'X軸動作 ON積算時間[秒]
	IntegTimeYAct                   'Y軸動作 ON積算時間[秒]
	IntegTimeZAct                   'Z軸動作 ON積算時間[秒]
	'5
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
	MELTStartTemp                   'MELT法 開始温度(℃)
	MELTEndTemp                     'MELT法 終了温度(℃)
	MELTWaitTime                    'MELT法 待ち時間(Sec)
	MELTSpeed                       'MELT法 昇温速度(℃/min)
	MELTTemperature                 'MELT法 結果温度(℃)
	MELTViscosity                   'MELT法 結果粘度(PaS)
	'9
	FLCTemp                         'FLC 設定温度(℃)
	ProbeDiameter                   'FLC プローブ直径(mm)
	FLCSampleTime                   'FLC サンプル取得時間(Sec)
	FLCSampleAccRate                'FLC サンプル取得率(%)
	FLCAmplitude                    'FLC 振幅(mm)
	FLCPeriod                       'FLC 周期(Sec)

	FLC1AmplitudeStart              'FLC1 開始振幅(mm)
	FLC1AmplitudeEnd                'FLC1 終了振幅(mm)
	FLC1AmplitudeStep               'FLC1 Step振幅(mm)
	FLC1MeasRev                     'FLC1 逆順測定有効/無効

	FLC2Repeat                      'FLC2 繰返し回数(回)
	FLC2Ope                         'FLC2 動作時間(Sec)
	FLC2Rest                        'FLC2 停止時間(Sec)

	FLC3TempStart                   'FLC3 開始温度(℃)
	FLC3TempEnd                     'FLC3 終了温度(℃)
	FLC3TempStep                    'FLC3 Step温度(℃)
	'10
	Reserve0						'予備(測定時間で使用)
	Reserve1						'予備
	Reserve2						'予備
	Reserve3						'予備
	Reserve4						'予備
	Max
End Enum
'形状内訳
Public Enum eShape
	Sheet = 0       'シート
	Powder                  '粉体
	Liquid                  '液体
	Rcc                     'RCC
End Enum

''' <summary>検索パラメータ構造体</summary>
Public Structure stParam
	Dim sEquipID		As String
	Dim bDateTimeRange	As Boolean
	Dim sDateTimeFrom	As Date
	Dim sDateTimeTo		As Date
	Dim sSampleName		As String
	Dim sLot			As String
	Dim sMeasurer		As String
	Dim sSampleSpec		As String
	Dim sDeviceSpec		As String
	Dim sCalcMethod		As String
	Dim sHpPosCont		As String
	Dim sTEmpProfile	As String
	Dim sMeasType		As String
	Dim sOSCTrajectry	As String
End Structure
Public Const SearchParamMax As Int32 = 12


Private s_TableResultCreateText As String = _
"CREATE TABLE IF Not EXISTS tblResults (
    EquipID TEXT NOT NULL,
    DateTime TEXT NOT NULL,
    SampleSpec TEXT,
    SampleName TEXT,
    Lot TEXT,
    Measurer TEXT,
    Shape INTEGER,
    Thickness TEXT,
    Stacks TEXT,
    SpecificGravity TEXT,
    Weight TEXT,
    Volume TEXT,
    MaskTime TEXT,
    JudgeRange TEXT,
    DeviceSpec TEXT,
    HPPosControl INTEGER,
    HPTemprature REAL,
    PBTemprature REAL,
    SGTemp REAL,
    HPTempFlucRange REAL,
    PBTempFlucRange REAL,
    TempProfile TEXT,
    InputGap REAL,
    GapValue REAL,
    MeasurePresure REAL,
    PressContStep INTEGER,
    PressContDelay INTEGER,
    PBDiameter REAL,
    StressDetectMode INTEGER,
    AmplitudeMode INTEGER,
    LimitStress REAL,
    XAmplitude REAL,
    XPeriod REAL,
    YAmplitude REAL,
    YPeriod REAL,
    ZAmplitude REAL,
    ZPeriod REAL,
    InputWait REAL,
    HPLiftWait REAL,
    XYActWait REAL,
    MeasTimeLimit REAL,
    MeasEndStress REAL,
    FileName TEXT,
    MeasureType TEXT,
    CalculationMethod TEXT,
    StressThreshold REAL,
    SLOPEDiffInterval REAL,
    GelTime REAL,
    GelTimeJudge TEXT,
    GelTimeTemp REAL,
    GelTimeVisco REAL,
    Filter TEXT,
    OOSValX INTEGER,
    OOSValY INTEGER,
    OOSValZ INTEGER,
    CPUCommErrCnt INTEGER,
    ToolVersion TEXT,
    EquipVersion TEXT,
    XYSensID TEXT,
    ZSensID TEXT,
    IntegTimePowOn INTEGER,
    IntegTimeHeatOn INTEGER,
    IntegTimePBHeat INTEGER,
    IntegTimeSGHeat INTEGER,
    IntegTimeXAct INTEGER,
    IntegTimeYAct INTEGER,
    IntegTimeZAct INTEGER,
    XYWeightCoef REAL,
    ZWeightCoef REAL,
    FullCalibrationVlue INTEGER,
    ZeroCalibrationVlue INTEGER,
    FullStressAttenuation REAL,
    FullStressPxPhase REAL,
    FullStressPyPhase REAL,
    FullStressPzPhase REAL,
    GAPCORR REAL,
    NoLoadZStress REAL,
    ZSTAB REAL,
    ZeroAdjStabMonTime REAL,
    ZeroAdjFlucRange REAL,
    ZZeroPosition REAL,
    HPCenterX INTEGER,
    HPCenterY INTEGER,
    TempCnt_P REAL,
    TempCnt_I REAL,
    TempCnt_D REAL,
    TempCnt_Dgain REAL,
    TempCnt_PVBias REAL,
    TempCnt_PVGain REAL,
    TempCnt_P2 REAL,
    TempCnt_I2 REAL,
    TempCnt_D2 REAL,
    TempCnt_Dgain2 REAL,
    TempCnt_PVBias2 REAL,
    TempCnt_PVGain2 REAL,
    TempCnt_P3 REAL,
    TempCnt_I3 REAL,
    TempCnt_D3 REAL,
    TempCnt_Dgain3 REAL,
    TempCnt_PVBias3 REAL,
    TempCnt_PVGain3 REAL,
    ExtStartTime TEXT,
    ExtMiddleTime TEXT,
    ExtEndTime TEXT,
    ExtTanSample TEXT,
    ExtTanThinning TEXT,
    ExtGelTimeSelect TEXT,
    ExtGelTimeUnder TEXT,
    ExtGelTimeMiddle TEXT,
    ExtGelTimeOver TEXT,
    MELTStartTemp TEXT,
    MELTEndTemp TEXT,
    MELTWaitTime TEXT,
    MELTSpeed TEXT,
    MELTTemperature TEXT,
    MELTViscosity TEXT,
    FLCTemp TEXT,
    ProbeDiameter TEXT,
    FLCSampleTime TEXT,
    FLCSampleAccRate TEXT,
    FLCAmplitude TEXT,
    FLCPeriod TEXT,
    FLC1AmplitudeStart TEXT,
    FLC1AmplitudeEnd TEXT,
    FLC1AmplitudeStep TEXT,
    FLC1MeasRev TEXT,
    FLC2Repeat TEXT,
    FLC2Ope TEXT,
    FLC2Rest TEXT,
    FLC3TempStart TEXT,
    FLC3TempEnd TEXT,
    FLC3TempStep TEXT,
    Reserve0 TEXT,
    Reserve1 TEXT,
    Reserve2 TEXT,
    Reserve3 TEXT,
    Reserve4 TEXT,
    PRIMARY KEY (EquipID, DateTime)
);
"

''' <summary>
''' SizukaDB使用可能か検査（SQLite版）
''' </summary>
Public Function CheckSizukaDB() As Int32
    Try
        Dim dbPath As String = Path.Combine(p_DBPath, p_SQLiteDBFileName)
        '① DBファイル存在確認,無ければ作成する
        If Not IO.File.Exists(dbPath) Then
			SQLiteConnection.CreateFile(dbPath)
        End If
        '② 接続確認
        Try
            Using cn As New SQLiteConnection("Data Source=" & dbPath & ";Version=3;")
                cn.Open()
            End Using
        Catch ex As Exception
            WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
            Return -4   '接続失敗
        End Try
        '③ テーブル存在確認（任意）
        Try
            Using cn As New SQLiteConnection("Data Source=" & dbPath & ";Version=3;")
                cn.Open()
                Using cmd As New SQLiteCommand(cn)
                    cmd.CommandText =
                        "SELECT COUNT(*) FROM sqlite_master WHERE type='table' AND name=@tbl"
                    cmd.Parameters.AddWithValue("@tbl", p_tblResults) '例：メインテーブル名
                    Dim cnt As Integer = Convert.ToInt32(cmd.ExecuteScalar())
                    If cnt = 0 Then
                        Return -5 'テーブルなし（＝不正状態）
                    End If
                End Using
            End Using
        Catch ex As Exception
            WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
            Return -4
        End Try
        Return 0

    Catch ex As Exception
        WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
        Return -1
    End Try
End Function

''' <summary>
''' SQLite DB 初期化（完全自動）
''' </summary>
Public Function InitializeSizukaDB() As Boolean
    Try
        Dim dbPath As String = Path.Combine(p_DBPath, p_SQLiteDBFileName)
        Dim connStr As String = "Data Source=" & dbPath & ";Version=3;"
		Dim dbDir As String = p_DBPath
		If Not IO.Directory.Exists(dbDir) Then
			Try
				IO.Directory.CreateDirectory(dbDir)
			Catch ex As Exception
				WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
				Return -3   'ディレクトリ作成失敗
			End Try
		End If

        '① DBファイル作成
        If Not IO.File.Exists(dbPath) Then
            SQLiteConnection.CreateFile(dbPath)
        End If

        Using cn As New SQLiteConnection(connStr)
            cn.Open()

            Using cmd As New SQLiteCommand(cn)

                '========================================
                '★ 追加：SQLite最適設定（超重要）
                '========================================
                cmd.CommandText =
                "PRAGMA journal_mode=WAL;" &
                "PRAGMA synchronous=NORMAL;" &
                "PRAGMA temp_store=MEMORY;" &
                "PRAGMA foreign_keys=ON;"
                cmd.ExecuteNonQuery()

                '========================================
                '② テーブル作成（存在しなければ）
                '========================================
                cmd.CommandText =
                "CREATE TABLE IF NOT EXISTS T_Sample (" &
                "ID INTEGER PRIMARY KEY AUTOINCREMENT," &
                "Name TEXT," &
                "Value REAL," &
                "CreateDate TEXT" &
                ")"
                cmd.ExecuteNonQuery()

                cmd.CommandText = s_TableResultCreateText
                cmd.ExecuteNonQuery()

                '========================================
                '③ 初期データ投入（存在しなければ）
                '========================================
                cmd.CommandText = "SELECT COUNT(*) FROM T_Sample"
                Dim cnt As Integer = Convert.ToInt32(cmd.ExecuteScalar())

                If cnt = 0 Then
                    cmd.CommandText =
                    "INSERT INTO T_Sample (Name, Value, CreateDate) VALUES " &
                    "('InitData1', 0, datetime('now'))," &
                    "('InitData2', 1, datetime('now'))"
                    cmd.ExecuteNonQuery()
                End If

            End Using
        End Using

        Return True

    Catch ex As Exception
        WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
        Return False
    End Try
End Function



#If 0


''' <summary>
''' SQLite DB 初期化（完全自動）
''' </summary>
Public Function InitializeSizukaDB() As Boolean
    Try
        Dim dbPath As String = Path.Combine(p_DBPath, p_SQLiteDBFileName)
        Dim connStr As String = "Data Source=" & dbPath & ";Version=3;"
        '① DBファイル作成
        If Not IO.File.Exists(dbPath) Then
            SQLiteConnection.CreateFile(dbPath)
        End If
        Using cn As New SQLiteConnection(connStr)
            cn.Open()
            Using cmd As New SQLiteCommand(cn)
                '========================================
                '② テーブル作成（存在しなければ）
                '========================================
                cmd.CommandText =
                "CREATE TABLE IF NOT EXISTS T_Sample (" &
                "ID INTEGER PRIMARY KEY AUTOINCREMENT," &
                "Name TEXT," &
                "Value REAL," &
                "CreateDate TEXT" &
                ")"
                cmd.ExecuteNonQuery()

				cmd.CommandText = s_TableResultCreateText
				cmd.ExecuteNonQuery()

                '========================================
                '③ 初期データ投入（存在しなければ）
                '========================================

                cmd.CommandText = "SELECT COUNT(*) FROM T_Sample"
                Dim cnt As Integer = Convert.ToInt32(cmd.ExecuteScalar())

                If cnt = 0 Then
                    cmd.CommandText =
                    "INSERT INTO T_Sample (Name, Value, CreateDate) VALUES " &
                    "('InitData1', 0, datetime('now'))," &
                    "('InitData2', 1, datetime('now'))"
                    cmd.ExecuteNonQuery()
                End If

            End Using
        End Using
        Return True
    Catch ex As Exception
        WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
        Return False
    End Try
End Function

#end if
''' <summary>
''' SQLite DB接続
''' </summary>
''' <param name="sqlcn"></param>
''' <returns></returns>
Public Function connectDb(ByRef sqlcn As SQLiteConnection) As Int32
	Dim sqlcmd As New SQLiteCommand()
	Try
		sqlcn.ConnectionString = "Data Source=" & p_DBPath & "\SizukaDB.db;Version=3;"
		sqlcmd.Connection = sqlcn
		sqlcmd.CommandType = CommandType.Text
		Try
			sqlcn.Open()
		Catch ex As Exception
			Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
			Return -2
		End Try
		Return 0
	Catch ex As Exception
		Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
		Return -1
	End Try
End Function
''' <summary>
''' テーブルの行数を取得
''' </summary>
''' <param name="sTable"></param>
''' <returns></returns>
Public Function fTableCountCommand(ByVal sTable As String) As Int32
	Dim iCount As Int32 = 0
	Try
	Using conn As New SQLiteConnection(m_ConnString)
		conn.Open()
		Using sqlcmd As New SQLiteCommand("",conn)
			sqlcmd.CommandText = "SELECT COUNT(*) FROM " & sTable
			Using sqldr As SQLiteDataReader = sqlcmd.ExecuteReader()
				sqldr.Read()
				iCount = sqldr(0)
			End Using
		End Using
	End Using
	Return iCount
	Catch ex As Exception
		Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
		Return -1
	Finally
	End Try
End Function
''' <summary>
''' テーブルフィールド名取得
''' </summary>
''' <param name="sTable">テーブル名</param>
''' <param name="sColmns">フィールド一覧</param>
''' <returns></returns>
Private Function TblColumnsGet(ByVal sTable As String, ByRef sColmns() As String, Optional ByRef sColmnsAt() As String = Nothing) As Int32
Dim i As Int32
Try
	Using conn As New SQLiteConnection(m_ConnString)
		conn.Open()
		Using sqlcmd As New SQLiteCommand("",conn)
			'PRAGMA
			sqlcmd.CommandText = "PRAGMA table_info(" & sTable & ")"
			Using sqldr As SQLiteDataReader = sqlcmd.ExecuteReader()
				i = 0
				Array.Resize(sColmns, 0)
				Array.Resize(sColmnsAt, 0)
				While sqldr.Read()
					Array.Resize(sColmns, sColmns.Length + 1)
					sColmns(i) = sqldr("name").ToString
					If sColmnsAt IsNot Nothing Then
						Array.Resize(sColmnsAt, sColmnsAt.Length + 1)
						sColmnsAt(i) = "@" & sqldr("name").ToString
					End If
					i += 1
				End While
			End using
		End Using

	End Using
    Return 0
Catch ex As Exception
    WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
    Return -1
End Try
End Function
''' <summary>
''' tblResults フィールド名一覧取得
''' </summary>
''' <returns></returns>
Public Function TblFieldNameGet() As Int32
	Dim lRet As Int32 = 0
	Try
		lRet = TblColumnsGet(p_tblResults, p_sRltName, p_sRltNameAt)        'Resultテーブルフィールド一覧取得
		If lRet < 0
			'フィールド一覧取得失敗
			Return lRet
		End If
		Return lRet
	Catch ex As Exception
		Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
		Return -1
	End Try
End Function
'''<summary>
'''Resultテーブルへ行追加
'''</summary>
''' <param name="stData">追加するデータ ヘッダ情報構造体</param>
'''<returns>0:成功 -1:例外発生</returns>
Public Function InsertResultsRec(ByVal stData As DataFile.DataFileHeader) As Int32
	Dim sqlTran As SqLiteTransaction = Nothing
	Dim lRet As Int32
Try
	If IsNothing(p_sRltName)
		lRet = TblColumnsGet(p_tblResults, p_sRltName, p_sRltNameAt)        'Resultテーブルフィールド一覧取得
		If lRet < 0
			'フィールド一覧取得失敗
			Return lRet
		End If
	End If
	Dim sFields As String = ""
	Dim sAtFields As String = ""
	For i = 0 To p_sRltName.GetUpperBound(0) - 1
		sFields &= p_sRltName(i) & ","
		sAtFields &= p_sRltNameAt(i) & ","
	Next
	sFields = sFields.Substring(0, sFields.Length - 1) & ")"
	sAtFields = sAtFields.Substring(0, sAtFields.Length - 1) & ")"


	Using conn1 As New SQLiteConnection(m_ConnString)
		conn1.Open()
		Using sqlcmdC As New SqLiteCommand("", conn1)
			'登録しようとするキーがあるかチェック
			sqlcmdC.CommandText = "SELECT " & p_sRltName(eRst.EquipID) & "," & p_sRltName(eRst.DateTime) & _
							 " FROM tblResults WHERE " & _
							 p_sRltName(eRst.EquipID) & " = '" & stData.sEquipName & "' And " & _
							 p_sRltName(eRst.DateTime) & " = '" & stData.sDateTime & "' " & _
							 "ORDER BY EquipID LIMIT 1;"
			Using sqldr As SqLiteDataReader = sqlcmdC.ExecuteReader()
				Try
					If sqldr.Read()
						If sqldr(p_sRltName(eRst.EquipID)) <> ""
							sqldr.Close
							Return -9		'既にあるのでスキップ
						End If
					End If
				Catch ex As Exception
					sqldr.Close
				End Try
			End Using
		End Using
	End Using

	Using conn As New SQLiteConnection(m_ConnString)
		conn.Open()
		Using tran = conn.BeginTransaction()		'トランザクション開始
		Using sqlcmd As New SqLiteCommand("", conn, tran)
			'データの追加
			sqlcmd.CommandText = "INSERT INTO tblResults (" & sFields & _
									" VALUES (" & sAtFields

			AddParam(sqlcmd, p_sRltNameAt(eRst.EquipID), stData.sEquipName)
			AddParam(sqlcmd, p_sRltNameAt(eRst.DateTime), stData.sDateTime)

			AddParam(sqlcmd, p_sRltNameAt(eRst.SampleSpec), stData.sSampleSpec)
			AddParam(sqlcmd, p_sRltNameAt(eRst.SampleName), stData.sSampleName)
			AddParam(sqlcmd, p_sRltNameAt(eRst.Lot), stData.sLot)
			AddParam(sqlcmd, p_sRltNameAt(eRst.Measurer), stData.sMeasurer)
			AddParam(sqlcmd, p_sRltNameAt(eRst.Shape), stData.sShape)

			AddParam(sqlcmd, p_sRltNameAt(eRst.Thickness), stData.sThickness)
			AddParam(sqlcmd, p_sRltNameAt(eRst.Stacks), stData.dStacks)
			AddParam(sqlcmd, p_sRltNameAt(eRst.SpecificGravity), stData.sThickness)
			AddParam(sqlcmd, p_sRltNameAt(eRst.Weight), stData.dStacks)
			AddParam(sqlcmd, p_sRltNameAt(eRst.Volume), stData.dStacks)

			AddParam(sqlcmd, p_sRltNameAt(eRst.MaskTime), stData.sMaskTime)
			AddParam(sqlcmd, p_sRltNameAt(eRst.JudgeRange), stData.sJudgeRange)
			AddParam(sqlcmd, p_sRltNameAt(eRst.DeviceSpec), stData.sDeviceSpec)
			AddParam(sqlcmd, p_sRltNameAt(eRst.HPPosControl), stData.sHPPosControl)

			AddParam(sqlcmd, p_sRltNameAt(eRst.HPTemprature), stData.dHPTemp)
			AddParam(sqlcmd, p_sRltNameAt(eRst.PBTemprature), stData.dPBTemp)
			AddParam(sqlcmd, p_sRltNameAt(eRst.SGTemp), stData.dSGTemp)

			AddParam(sqlcmd, p_sRltNameAt(eRst.HPTempFlucRange), stData.sHPTempFlucRange)
			AddParam(sqlcmd, p_sRltNameAt(eRst.PBTempFlucRange), stData.sPBTempFlucRange)

			AddParam(sqlcmd, p_sRltNameAt(eRst.TempProfile), stData.sTempProfile)
			AddParam(sqlcmd, p_sRltNameAt(eRst.InputGap), stData.dInputGap)
			AddParam(sqlcmd, p_sRltNameAt(eRst.GapValue), stData.dGapValue)
			AddParam(sqlcmd, p_sRltNameAt(eRst.MeasurePresure), stData.dMeasurePress)

			AddParam(sqlcmd, p_sRltNameAt(eRst.PressContStep), stData.iPressContStep)
			AddParam(sqlcmd, p_sRltNameAt(eRst.PressContDelay), stData.iPressContDelay)

			AddParam(sqlcmd, p_sRltNameAt(eRst.PBDiameter), stData.dPBDiameter)
			AddParam(sqlcmd, p_sRltNameAt(eRst.StressDetectMode), stData.sStrsDetectMode)
			AddParam(sqlcmd, p_sRltNameAt(eRst.AmplitudeMode), stData.sAmpMode)
			AddParam(sqlcmd, p_sRltNameAt(eRst.LimitStress), stData.dLimitStress)

			AddParam(sqlcmd, p_sRltNameAt(eRst.XAmplitude), stData.dXAmp)
			AddParam(sqlcmd, p_sRltNameAt(eRst.XPeriod), stData.dXPrd)
			AddParam(sqlcmd, p_sRltNameAt(eRst.YAmplitude), stData.dYAmp)
			AddParam(sqlcmd, p_sRltNameAt(eRst.YPeriod), stData.dYPrd)
			AddParam(sqlcmd, p_sRltNameAt(eRst.ZAmplitude), stData.dZAmp)
			AddParam(sqlcmd, p_sRltNameAt(eRst.ZPeriod), stData.dZPrd)

			AddParam(sqlcmd, p_sRltNameAt(eRst.InputWait), stData.dInputWait)
			AddParam(sqlcmd, p_sRltNameAt(eRst.HPLiftWait), stData.dHPLiftWait)
			AddParam(sqlcmd, p_sRltNameAt(eRst.XYActWait), stData.dXYActWait)

			AddParam(sqlcmd, p_sRltNameAt(eRst.MeasTimeLimit), stData.dMeasTimeLimit)
			AddParam(sqlcmd, p_sRltNameAt(eRst.MeasEndStress), stData.dMeasEndStress)
			AddParam(sqlcmd, p_sRltNameAt(eRst.FileName), stData.sFileName)
			AddParam(sqlcmd, p_sRltNameAt(eRst.MeasureType), stData.sMeasureType)
			AddParam(sqlcmd, p_sRltNameAt(eRst.CalculationMethod), stData.sCalcMethod)
			AddParam(sqlcmd, p_sRltNameAt(eRst.StressThreshold), stData.sThreshold)
			AddParam(sqlcmd, p_sRltNameAt(eRst.SLOPEDiffInterval), stData.dDiffInterval)

			AddParam(sqlcmd,p_sRltNameAt(eRst.GelTime), stData.dGelTime)
			AddParam(sqlcmd,p_sRltNameAt(eRst.GelTimeJudge), stData.sGelTimeJudge)
			AddParam(sqlcmd,p_sRltNameAt(eRst.GelTimeTemp), stData.dGelTimeTemp)
			AddParam(sqlcmd,p_sRltNameAt(eRst.GelTimeVisco), stData.dGelTimeVisco)

			AddParam(sqlcmd,p_sRltNameAt(eRst.Filter), stData.sFilter)
			AddParam(sqlcmd,p_sRltNameAt(eRst.OOSValX), stData.dOosValX)
			AddParam(sqlcmd,p_sRltNameAt(eRst.OOSValY), stData.dOosValY)
			AddParam(sqlcmd,p_sRltNameAt(eRst.OOSValZ), stData.dOosValZ)
			AddParam(sqlcmd,p_sRltNameAt(eRst.CPUCommErrCnt), stData.iCpuCommErrCount)

			AddParam(sqlcmd,p_sRltNameAt(eRst.ToolVersion), stData.sToolVersion)
			AddParam(sqlcmd,p_sRltNameAt(eRst.EquipVersion), stData.sEquipVersion)
			AddParam(sqlcmd,p_sRltNameAt(eRst.XYSensID), stData.sXYSensorID)
			AddParam(sqlcmd,p_sRltNameAt(eRst.ZSensID), stData.sZSensorID)

			AddParam(sqlcmd,p_sRltNameAt(eRst.IntegTimePowOn), stData.iIntegTimePowOn)
			AddParam(sqlcmd,p_sRltNameAt(eRst.IntegTimeHeatOn), stData.iIntegTimeHeatOn)
			AddParam(sqlcmd,p_sRltNameAt(eRst.IntegTimePBHeat), stData.iIntegTimePBHeat)
			AddParam(sqlcmd,p_sRltNameAt(eRst.IntegTimeSGHeat), stData.iIntegTimeSGHeat)
			AddParam(sqlcmd,p_sRltNameAt(eRst.IntegTimeXAct), stData.iIntegTimeXAct)
			AddParam(sqlcmd,p_sRltNameAt(eRst.IntegTimeYAct), stData.iIntegTimeYAct)
			AddParam(sqlcmd,p_sRltNameAt(eRst.IntegTimeZAct), stData.iIntegTimeZAct)

			AddParam(sqlcmd,p_sRltNameAt(eRst.XYWeightCoef), stData.dXYWeightCoef)
			AddParam(sqlcmd,p_sRltNameAt(eRst.ZWeightCoef), stData.dZWeightCoef)
			AddParam(sqlcmd,p_sRltNameAt(eRst.FullCalibrationVlue), Int32.Parse(stData.sFullCalibValue))
			AddParam(sqlcmd,p_sRltNameAt(eRst.ZeroCalibrationVlue), Int32.Parse(stData.sZeroCalibValue))
			AddParam(sqlcmd,p_sRltNameAt(eRst.FullStressAttenuation), Double.Parse(stData.sFullStressAttenuation))
			AddParam(sqlcmd,p_sRltNameAt(eRst.FullStressPxPhase), Double.Parse(stData.sFullStressPhaseX))
			AddParam(sqlcmd,p_sRltNameAt(eRst.FullStressPyPhase), Double.Parse(stData.sFullStressPhaseY))
			AddParam(sqlcmd,p_sRltNameAt(eRst.FullStressPzPhase), Double.Parse(stData.sFullStressPhaseZ))
			AddParam(sqlcmd,p_sRltNameAt(eRst.GAPCORR), stData.dGapCorrect)
			AddParam(sqlcmd,p_sRltNameAt(eRst.NoLoadZStress), stData.dZNoLoadStress)
			AddParam(sqlcmd,p_sRltNameAt(eRst.ZSTAB), stData.dZStab)
			AddParam(sqlcmd,p_sRltNameAt(eRst.ZeroAdjStabMonTime), Double.Parse(stData.sZeroAdjStabMonTime))
			AddParam(sqlcmd,p_sRltNameAt(eRst.ZeroAdjFlucRange), Double.Parse(stData.sZeroAdjFlucRange))
			AddParam(sqlcmd,p_sRltNameAt(eRst.ZZeroPosition), stData.dZZeroPosition)
			AddParam(sqlcmd,p_sRltNameAt(eRst.HPCenterX), stData.sHPCenterX)
			AddParam(sqlcmd,p_sRltNameAt(eRst.HPCenterY), stData.sHPCenterY)

			AddParam(sqlcmd,p_sRltNameAt(eRst.TempCnt_P), stData.dTempCnt_P)
			AddParam(sqlcmd,p_sRltNameAt(eRst.TempCnt_I), stData.dTempCnt_I)
			AddParam(sqlcmd,p_sRltNameAt(eRst.TempCnt_D), stData.dTempCnt_d)
			AddParam(sqlcmd,p_sRltNameAt(eRst.TempCnt_Dgain), stData.dTempCnt_Dgain)
			AddParam(sqlcmd,p_sRltNameAt(eRst.TempCnt_PVBias), stData.dTempCnt_PVBias)
			AddParam(sqlcmd,p_sRltNameAt(eRst.TempCnt_PVGain), stData.dTempCnt_PVGain)

			AddParam(sqlcmd,p_sRltNameAt(eRst.TempCnt_P2), stData.dTempCnt_P2)
			AddParam(sqlcmd,p_sRltNameAt(eRst.TempCnt_I2), stData.dTempCnt_I2)
			AddParam(sqlcmd,p_sRltNameAt(eRst.TempCnt_D2), stData.dTempCnt_d2)
			AddParam(sqlcmd,p_sRltNameAt(eRst.TempCnt_Dgain2), stData.dTempCnt_Dgain2)
			AddParam(sqlcmd,p_sRltNameAt(eRst.TempCnt_PVBias2), stData.dTempCnt_PVBias2)
			AddParam(sqlcmd,p_sRltNameAt(eRst.TempCnt_PVGain2), stData.dTempCnt_PVGain2)

			AddParam(sqlcmd,p_sRltNameAt(eRst.TempCnt_P3), stData.dTempCnt_P3)
			AddParam(sqlcmd,p_sRltNameAt(eRst.TempCnt_I3), stData.dTempCnt_I3)
			AddParam(sqlcmd,p_sRltNameAt(eRst.TempCnt_D3), stData.dTempCnt_d3)
			AddParam(sqlcmd,p_sRltNameAt(eRst.TempCnt_Dgain3), stData.dTempCnt_Dgain3)
			AddParam(sqlcmd,p_sRltNameAt(eRst.TempCnt_PVBias3), stData.dTempCnt_PVBias3)
			AddParam(sqlcmd,p_sRltNameAt(eRst.TempCnt_PVGain3), stData.dTempCnt_PVGain3)

			'If stData.sCalcMethod = "EXTRAPOLATION"
			AddParam(sqlcmd,p_sRltNameAt(eRst.ExtStartTime), stData.sExtStartTime)
			AddParam(sqlcmd,p_sRltNameAt(eRst.ExtMiddleTime), stData.sExtMiddleTime)
			AddParam(sqlcmd,p_sRltNameAt(eRst.ExtEndTime), stData.sExtEndTime)
			AddParam(sqlcmd,p_sRltNameAt(eRst.ExtTanSample), stData.sExtTanSample)
			AddParam(sqlcmd,p_sRltNameAt(eRst.ExtTanThinning), stData.sExtTanThinning)
			AddParam(sqlcmd,p_sRltNameAt(eRst.ExtGelTimeSelect), stData.sExtGelTimeSelect)
			AddParam(sqlcmd,p_sRltNameAt(eRst.ExtGelTimeUnder), stData.sExtGelTimeUnder)
			AddParam(sqlcmd,p_sRltNameAt(eRst.ExtGelTimeMiddle), stData.sExtGelTimeMiddle)
			AddParam(sqlcmd,p_sRltNameAt(eRst.ExtGelTimeOver), stData.sExtGelTimeOver)
			'End If

			'If stData.sCalcMethod = "MELT"
			AddParam(sqlcmd,p_sRltNameAt(eRst.MELTStartTemp), stData.sMelStartTemp)
			AddParam(sqlcmd,p_sRltNameAt(eRst.MELTEndTemp), stData.sMelEndTemp)
			AddParam(sqlcmd,p_sRltNameAt(eRst.MELTWaitTime), stData.sMelWaitTime)
			AddParam(sqlcmd,p_sRltNameAt(eRst.MELTSpeed), stData.sMelSpeed)
			AddParam(sqlcmd,p_sRltNameAt(eRst.MELTTemperature), stData.sMelTemperatureRslt)
			AddParam(sqlcmd,p_sRltNameAt(eRst.MELTViscosity), stData.sMelViscosityRslt)
			'End If

			'If stData.sMeasureType <> "GEL"
			AddParam(sqlcmd,p_sRltNameAt(eRst.FLCTemp), stData.sFLCTemp)
			AddParam(sqlcmd,p_sRltNameAt(eRst.ProbeDiameter), stData.sProbeDiameter)
			AddParam(sqlcmd,p_sRltNameAt(eRst.FLCSampleTime), stData.sFLCSampleTime)
			AddParam(sqlcmd,p_sRltNameAt(eRst.FLCSampleAccRate), stData.sFLCSampleAccRate)
			AddParam(sqlcmd,p_sRltNameAt(eRst.FLCAmplitude), stData.sFLCAmplitude)
			AddParam(sqlcmd,p_sRltNameAt(eRst.FLCPeriod), stData.sFLCPeriod)

			AddParam(sqlcmd,p_sRltNameAt(eRst.FLC1AmplitudeStart), stData.sFLC1AmplitudeStart)
			AddParam(sqlcmd,p_sRltNameAt(eRst.FLC1AmplitudeEnd), stData.sFLC1AmplitudeEnd)
			AddParam(sqlcmd,p_sRltNameAt(eRst.FLC1AmplitudeStep), stData.sFLC1AmplitudeStep)
			AddParam(sqlcmd,p_sRltNameAt(eRst.FLC1MeasRev), stData.sFLC1MeasRev)

			AddParam(sqlcmd,p_sRltNameAt(eRst.FLC2Repeat), stData.sFLC2Repeat)
			AddParam(sqlcmd,p_sRltNameAt(eRst.FLC2Ope), stData.sFLC2Ope)
			AddParam(sqlcmd,p_sRltNameAt(eRst.FLC2Rest), stData.sFLC2Rest)

			AddParam(sqlcmd,p_sRltNameAt(eRst.FLC3TempStart), stData.sFLC3TempStart)
			AddParam(sqlcmd,p_sRltNameAt(eRst.FLC3TempEnd), stData.sFLC3TempEnd)
			AddParam(sqlcmd,p_sRltNameAt(eRst.FLC3TempStep), stData.sFLC3TempStep)
			AddParam(sqlcmd,p_sRltNameAt(eRst.Reserve0), stData.iMeasActTime)
			AddParam(sqlcmd,p_sRltNameAt(eRst.Reserve1), "")
			AddParam(sqlcmd,p_sRltNameAt(eRst.Reserve2), "")
			AddParam(sqlcmd,p_sRltNameAt(eRst.Reserve3), "")
			AddParam(sqlcmd,p_sRltNameAt(eRst.Reserve4), "")

			Try
				sqlcmd.ExecuteNonQuery()
				Tran.Commit()   'トランザクション終了（成功））

			Catch ex As Exception
				Try
					Tran.Rollback()
					Call WriteLog("Rollback Transaction", eLOG_LEVEL.iWARNING)
					Return -9
				Catch ex2 As Exception
					Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
					Call WriteLog("Rollback Transaction failed", eLOG_LEVEL.iFATAL)
					'This catch block will handle any errors that may have occurred
					'on the server that would cause the rollback to fail, such as a closed connection.
					Return -1
				End Try
			Finally
				'sqlcn.Close()
			End Try
		End Using
		End Using
	End Using
	Return 0

Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Return -1
End Try
End Function
Private Sub AddParam(cmd As SQLiteCommand, name As String, val As Object)
    If val Is Nothing Then
        cmd.Parameters.AddWithValue(name, DBNull.Value)
        Return
    End If
    ' 文字列の場合
    If TypeOf val Is String Then
        Dim s As String = DirectCast(val, String).Trim()

        If s = "" OrElse s.ToUpper() = "NA" Then
            cmd.Parameters.AddWithValue(name, DBNull.Value)
        'ElseIf IsNumeric(s) Then
        '    cmd.Parameters.AddWithValue(name, CDbl(s))
        Else
            cmd.Parameters.AddWithValue(name, s)
        End If

        Return
    End If
    ' 数値系
    If TypeOf val Is Double OrElse
       TypeOf val Is Single OrElse
       TypeOf val Is Decimal Then
        cmd.Parameters.AddWithValue(name, CDbl(val))
        Return
    End If
    If TypeOf val Is Integer OrElse
       TypeOf val Is Int32 Then
        cmd.Parameters.AddWithValue(name, CInt(val))
        Return
    End If
    ' DateTime
    If TypeOf val Is DateTime Then
        cmd.Parameters.AddWithValue(name, DirectCast(val, DateTime).ToString("yyyy-MM-dd HH:mm:ss"))
        Return
    End If
    ' その他
    cmd.Parameters.AddWithValue(name, val)

End Sub
#If 0
''' <summary>
''' Resultsテーブル 指定変更
''' </summary>
''' <param name="sKey">主キー</param>
''' <param name="uEi">変更指示</param>
''' <returns></returns>
Public Function UpdateResultsRec(ByVal sKey() As String, ByVal uEi As dlgEditInfo.stEditItem) As Int32
'Dim sqlcmd As New SqLiteCommand()
Dim sSet As String
Try
	sSet = ""
	If uEi.bSample
		sSet = sSet & p_sRltName(eRst.SampleName) & " = N'" & uEi.sSample & "'"
	End If
	If uEi.bLot
		sSet = sSet & If(sSet.Length>0,",","") & p_sRltName(eRst.Lot) & " = N'" & uEi.sLot & "'"
	End If
	If uEi.bMeasurer
		sSet = sSet & If(sSet.Length>0,",","") & p_sRltName(eRst.Measurer) & " = N'" & uEi.sMeasurer & "'"
	End If
	Using sqlcon As New SqLiteConnection(m_ConnString)
		sqlcon.Open
		Using sqlcmd As New SqLiteCommand("", sqlcon)
			sqlcmd.CommandText = _
					"UPDATE tblResults SET " &  sSet & _
					" WHERE " & _
					p_sRltName(eRst.EquipID) & " = '" & sKey(0) & "' AND " & _
					p_sRltName(eRst.DateTime) & " = '" & sKey(1) & "';"
			sqlcmd.ExecuteNonQuery()
		End Using
	End Using
	Return 0
Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Return -1
End Try
End Function
#end If
''' <summary>
''' テーブル 全行削除
''' </summary>
''' <param name="sTable"></param>
''' <returns></returns>
Public Function fTableAllDeleteCommand(ByVal sTable As String) As Int32
	Try
		Using sqlcon As New SqLiteConnection(m_ConnString)
			sqlcon.Open
			Using sqlcmd As New SqLiteCommand("", sqlcon)
				sqlcmd.CommandText = "DELETE FROM " & sTable
				sqlcmd.ExecuteNonQuery()
			End Using
		End Using
		Return 0
	Catch ex As Exception
		Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
		Return -1
	Finally
	End Try
End Function
''' <summary>
''' tblResultsテーブル 選択キー１行削除
''' </summary>
''' <param name="sKey">主キー(EquipID,DateTime)をカンマ区切りで</param>
''' <returns>0:成功 1:行が存在しない</returns>
Public Function ftblResultsOneDeleteCommand(ByVal sKey As String) As Int32
	Dim sKeys() As String
	Dim sWhere	As String
	Dim cnt		As Int32
	Try
		sKeys = sKey.Split(",")
		sWhere = " WHERE "
		sWhere = sWhere & p_sRltName(eRst.EquipID) & " = '" & sKeys(0) & "' AND "
		sWhere = sWhere & p_sRltName(eRst.DateTime) & " = '" & sKeys(1) & "'"

		Using sqlcon As New SqLiteConnection(m_ConnString)
			sqlcon.Open
			Using sqlcmd As New SqLiteCommand("", sqlcon)
				sqlcmd.CommandText = "DELETE FROM " & p_tblResults & sWhere
				cnt = sqlcmd.ExecuteNonQuery()
			End Using
		End Using
		Return If(cnt > 0, 0, 1)
	Catch ex As Exception
		Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
		Return -1
	Finally
	End Try
End Function
''' <summary>
''' 単発コマンド共通関数
''' </summary>
''' <param name="sql">SQL文字列</param>
''' <param name="params"></param>
''' <returns></returns>
Function ExecuteNonQuery(sql As String, params As List(Of SQLiteParameter)) As Integer
    Using conn As New SQLiteConnection(m_ConnString)
        conn.Open()
        Using cmd As New SQLiteCommand(sql, conn)
            cmd.Parameters.AddRange(params.ToArray())
            Return cmd.ExecuteNonQuery()
        End Using
    End Using
End Function
''' <summary>
''' テーブルの全行を取得コマンド発行
''' </summary>
''' <param name="iProcMode">0:装置/測定日時/ファイル名列 取得コマンド 1:全列取得コマンド 2,3:日時範囲選択 取得コマンド</param>
''' <param name="sTable">テーブル名</param>
''' <param name="dt">データテーブル</param>
''' <returns></returns>
Public Function fTableAllReadCommand(ByVal iProcMode As Int16, ByVal sTable As String, ByRef dt As datatable) As Int32
	Try
		Using conn As New SQLiteConnection(m_ConnString)
			conn.Open()
			Using sqlcmd As New SqLiteCommand("", conn)
				Select Case iProcMode
				Case 0
					sqlcmd.CommandText = "SELECT " & p_sRltName(eRst.EquipID) & "," & p_sRltName(eRst.DateTime) & "," & p_sRltName(eRst.FileName) & " FROM " & sTable
				Case 1
					sqlcmd.CommandText = "SELECT * FROM " & sTable
				Case 2
				'今月のみ
					Dim fd As String = Now.Date.Year & "/" & Now.Date.Month & "/01" & " 00:00:00"
					Dim td As String = Now.Date.Year & "/" & Now.Date.Month & "/31" & " 23:59:59"
					sqlcmd.CommandText = "SELECT " & p_sRltName(eRst.EquipID) & "," & p_sRltName(eRst.DateTime) & "," & p_sRltName(eRst.FileName) & " FROM " & sTable & _
											" WHERE " & p_sRltName(eRst.DateTime) & " BETWEEN '" & fd & "' AND '" & td & "';"
				Case 3
				'今日のみ
					Dim fd As String = Now.Date.ToShortDateString & " 00:00:00"
					Dim td As String = Now.Date.ToShortDateString & " 23:59:59"
					sqlcmd.CommandText = "SELECT " & p_sRltName(eRst.EquipID) & "," & p_sRltName(eRst.DateTime) & "," & p_sRltName(eRst.FileName) & " FROM " & sTable & _
											" WHERE " & p_sRltName(eRst.DateTime) & " BETWEEN '" & fd & "' AND '" & td & "';"
				End Select
				Using sqldrr As SQLiteDataReader = sqlcmd.ExecuteReader()
					dt.Load(sqldrr)
				End Using
			End Using
		End Using
		Return 0
	Catch ex As Exception
		Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
		Return -1
	Finally
	End Try
End Function


''' <summary>
''' テーブル１行リード（DataTable版）
''' </summary>
''' <param name="iProcMode"></param>
''' <param name="dt">取得済みDataTable</param>
''' <param name="rowIndex">読み出し行インデックス</param>
''' <param name="stData">ヘッダ情報</param>
''' <returns>0:読込成功 9:読込なし -1:その他の異常</returns>
Public Function fTableAllRead(ByVal iProcMode As Int16, ByVal dt As DataTable, ByVal rowIndex As Integer, ByRef stData As DataFile.DataFileHeader) As Int32
    Try
        ' 行存在チェック
        If dt Is Nothing OrElse dt.Rows.Count <= rowIndex Then
            Return 9
        End If
        Dim row As DataRow = dt.Rows(rowIndex)
        Select Case iProcMode
            Case 1
                stData.sEquipName	= row(eRst.EquipID).ToString()
                stData.sDateTime	= row(eRst.DateTime).ToString()
                stData.sFileName	= row(eRst.FileName).ToString()
                stData.sMeasureType = row(eRst.MeasureType).ToString()

            Case 0, 2
                stData.sEquipName	= row(0).ToString()
                stData.sDateTime	= row(1).ToString()
                stData.sFileName	= row(2).ToString()
                stData.sMeasureType = row(3).ToString()
        End Select

        Return 0

    Catch ex As Exception
        Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
        Return -1
    End Try
End Function

#if 0


''' <summary>
''' テーブル１行リード
''' </summary>
''' <param name="iProcMode"></param>
''' <param name="sqldr">SQlServerリーダー</param>
''' <param name="stData">ヘッダ情報</param>
''' <returns>0:読込成功 9:読込なし -1:その他の異常</returns>
Public Function fTableAllRead(ByVal iProcMode As Int16, ByRef sqldr As SqLiteDataReader, ByRef stData As DataFileHeader) As Int32
	Try
	Using conn As New SQLiteConnection(m_ConnString)
		conn.Open()
		Using sqldrr As SqLiteDataReader = sqldr
			If sqldrr.Read() = True
				Select Case iProcMode
				Case 1
					stData.sEquipName	= sqldrr(eRst.EquipID)
					stData.sDateTime	= sqldrr(eRst.DateTime)
					stData.sFileName	= sqldrr(eRst.FileName)
					stData.sMeasureType	= sqldrr(eRst.MeasureType)
				Case 0, 2
					stData.sEquipName	= sqldrr(0)
					stData.sDateTime	= sqldrr(1)
					stData.sFileName	= sqldrr(2)
					stData.sMeasureType	= sqldrr(3)
				End Select
			Else
				'終了
				sqldrr.Close
				Return 9
			End If
		End Using
	End Using
	Return 0
	Catch ex As Exception
		Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
		Return -1
	Finally
	End Try
End Function
＃end if

''' <summary>
''' 指定フィールド名のグループ化リストを取得
''' </summary>
''' <param name="sField">フィールド名</param>
''' <param name="sTable">テーブル名</param>
''' <param name="sAry">グループ化リスト</param>
''' <returns></returns>
Public Function GetFieldsGroupeList(ByVal sField As String, ByVal sTable As String, ByRef sAry() As String) As Int32
	Try
	Array.Resize(sAry, 0)
	Using conn As New SQLiteConnection(m_ConnString)
		conn.Open()
		Using sqlcmd As New SQLiteCommand("", conn)
			sqlcmd.CommandText = "SELECT " & sField & " FROM " & sTable & " GROUP BY " & sField & " ORDER BY " & sField & " ASC"
			Using sqldr As SqLiteDataReader = sqlcmd.ExecuteReader()
				While sqldr.Read()
					Array.Resize(sAry, sAry.Length + 1)
					IF IsDBNull(sqldr(0))
					Debug.Print("")
					sAry(sAry.GetUpperBound(0)) = ""
					Else
					sAry(sAry.GetUpperBound(0)) = sqldr(0)
					End If
				End While
			End Using
		End Using
	End Using
	Return 0
	Catch ex As Exception
		Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
		Return -1
	Finally
	End Try
End Function
''' <summary>
''' 検索パラメータによるtblResultsを検索リード
''' </summary>
''' <param name="uPrm"></param>
''' <param name="sTable"></param>
''' <param name="sHead"></param>
''' <returns></returns>
Public Function GetParameterSearchList(ByVal uPrm As stParam, ByVal sTable As String, ByRef sHead() As DataFile.DataFileHeader) As Int32
Dim lRet As Int32
Dim sCommand	As String = Nothing
Dim sWhereMake	As String = Nothing
Try
	Array.Resize(sHead, 0)
	Using conn As New SQLiteConnection(m_ConnString)
		conn.Open()
		Using sqlcmd As New SqLiteCommand("", conn)
			lRet = SearchCommandSet(uPrm, sTable, sHead, sCommand, sWhereMake)		'SQLコマンド文字列生成
			If lRet<>0
			'例外発生
				Return lRet
			End If
			lRet = SearchReadToHeader(sqlcmd, sCommand, sHead)			'SQLレスポンスをリードしヘッダ情報へセット
			If lRet<>0
			'例外発生
				Return lRet
			End If
		End Using
	End Using
	Return 0
Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Return -1
Finally
End Try
End Function
''' <summary>
''' 検索パラメータからSQLコマンド文字列作成
''' </summary>
''' <param name="uPrm"></param>
''' <param name="sTable"></param>
''' <param name="sHead"></param>
''' <param name="sCommand"></param>
''' <returns></returns>
Public Function SearchCommandSet(ByVal uPrm As stParam, ByVal sTable As String, ByRef sHead() As DataFile.DataFileHeader, ByRef sCommand As String, ByRef sWhereMake As String ) As Int32
Dim sWhere(SearchParamMax - 1) As String
Try
	'デバイスID
	If uPrm.sEquipID.Length <> 0
		StringParameterWhere(0, eRst.EquipID, uPrm.sEquipID, sWhere(0))
	End If
	'サンプル名
	If uPrm.sSampleName.Length <> 0
		StringParameterWhere(0, eRst.SampleName, uPrm.sSampleName, sWhere(1))
	End If
	'Lot
	If uPrm.sLot.Length <> 0
		StringParameterWhere(0, eRst.Lot, uPrm.sLot, sWhere(2))
	End If
	'測定者
	If uPrm.sMeasurer.Length <> 0
		StringParameterWhere(0, eRst.Measurer, uPrm.sMeasurer, sWhere(3))
	End If
	'サンプルSPEC
	If uPrm.sSampleSpec.Length <> 0
		StringParameterWhere(0, eRst.SampleSpec, uPrm.sSampleSpec, sWhere(4))
	End If
	'デバイスSPEC
	If uPrm.sDeviceSpec.Length <> 0
		StringParameterWhere(0, eRst.DeviceSpec, uPrm.sDeviceSpec, sWhere(5))
	End If

	'GELタイム判定方法
	If uPrm.sCalcMethod.Length <> 0
		StringParameterWhere(0, eRst.CalculationMethod, uPrm.sCalcMethod, sWhere(6))
	End If

	'HP制御方法
	If uPrm.sHpPosCont.Length <> 0
		StringParameterWhere(1, eRst.HPPosControl, uPrm.sHpPosCont, sWhere(7))
	End If

	'温度プロファイル
	If uPrm.sTEmpProfile.Length <> 0
		StringParameterWhere(0, eRst.TempProfile, uPrm.sTEmpProfile, sWhere(8))
	End If

	'測定種別
	If uPrm.sMeasType.Length <> 0
		StringParameterWhere(0, eRst.MeasureType, uPrm.sMeasType, sWhere(8))
	End If

	'動作軌跡
	If uPrm.sOSCTrajectry.Length <> 0
		StringParameterWhere(0, eRst.StressDetectMode, uPrm.sOSCTrajectry, sWhere(8))
	End If

	'測定日時範囲
	If uPrm.bDateTimeRange
		sWhere(6) = p_sRltName(eRst.DateTime) & " BETWEEN '" & uPrm.sDateTimeFrom & "' AND '" & uPrm.sDateTimeTo & "'"
	End If

	'SELECT文の編集
	sCommand = "SELECT * FROM " & sTable & " WHERE "
	sWhereMake = " WHERE "
	For i = 0 To sWhere.GetUpperBound(0)
		sCommand = sCommand & sWhere(i) & If(IsNothing(sWhere(i)), "", " AND ")
		sWhereMake = sWhereMake & sWhere(i) & If(IsNothing(sWhere(i)), "", " AND ")
	Next
	'パラメータが何も無いときはWHERE句を削除
	If sCommand.Substring(sCommand.Length - 7, 7).Contains("WHERE")
		sCommand	= sCommand.Replace("WHERE", "")
		sWhereMake	= sWhereMake.Replace("WHERE", "")
	Else
		'最後のANDを削除
		sCommand	= sCommand.Substring(0, sCommand.Length - 5)
		sWhereMake	= sWhereMake.Substring(0, sWhereMake.Length - 5)
	End If
	Return 0
Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Return -1
End Try
End Function
''' <summary>
''' SQLレスポンスをリードしヘッダ情報を作成
''' </summary>
''' <param name="sqlcmd"></param>
''' <param name="sCommand"></param>
''' <param name="sHead"></param>
''' <returns></returns>
Private Function SearchReadToHeader(ByVal sqlcmd As SqLiteCommand, ByVal sCommand As String, ByRef sHead() As DataFile.DataFileHeader) As int32
Try
	Using conn As New SQLiteConnection(m_ConnString)
		conn.Open()
		Using sqldr As SqLiteDataReader = sqlcmd.ExecuteReader()
			Dim idx As Int32 = 0
			While sqldr.Read()      'SQLReader行リード
				Array.Resize(sHead, sHead.Length + 1)   'ヘッダ情報配列拡張
				idx = sHead.GetUpperBound(0)			'ヘッダ情報インデックス計算
				SetHeadFromSqlDr(sqldr, sHead(idx))		'ヘッダ情報へSQLReader行をセット
			End While
		End Using
	End Using
	Return 0
Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Return -1
End Try
End Function
''' <summary>
''' 検索条件の行数を取得
''' </summary>
''' <param name="sqlcmd"></param>
''' <param name="sWhere">WHERE句文字列</param>
''' <param name="iCount">取得行数</param>
''' <returns></returns>
Public Function ResultRecCountGet(ByVal sqlcmd As SqLiteCommand, ByRef sWhere As String, ByRef iCount As int32) As int32
Try
	Using conn As New SQLiteConnection(m_ConnString)
		conn.Open()
		Using sqlcmdd As New SQLitecommand("", conn)
			sqlcmdd.CommandText = "SELECT COUNT(*) FROM " & p_tblResults & sWhere
			Using sqldr As SqLiteDataReader = sqlcmdd.ExecuteReader()
				sqldr.Read()      'SQLReader行リード
				iCount = sqldr(0)
			End Using
		End Using
	End Using
	Return 0
Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Return -1
End Try
End Function
''' <summary>
''' 検索パラメータ文字列 ワイルドカードWhere句作成
''' </summary>
''' <param name="iType">0:文字列 1:数値</param>
''' <param name="iField"></param>
''' <param name="sField"></param>
''' <param name="sWhere"></param>
Private Sub StringParameterWhere(ByVal iType As Int16, ByVal iField As Int16, ByRef sField As String, ByRef sWhere As String)
	Select Case iType
	Case 0
		If sField.Contains("*")
			sField = sField.Replace("*", "%")
			sWhere = p_sRltName(iField) & " LIKE '" & sField & "'"
		Else
			sWhere = p_sRltName(iField) & " ='" & sField & "'"
		End If
	Case 1
		sWhere = p_sRltName(iField) & " = " & sField & " "
	End Select
	 
End Sub
''' <summary>
''' ヘッダ情報へSQLReader行をセット
''' </summary>
''' <param name="sqldr"></param>
''' <param name="sHead"></param>
Public Sub SetHeadFromSqlDr(ByVal sqldr As SqLiteDataReader, ByRef sHead As Datafile.DataFileHeader)
Try
	sHead.sEquipName = GetStringSafe(sqldr, eRst.EquipID)
	sHead.sDateTime = GetStringSafe(sqldr, eRst.DateTime)
	sHead.sSampleSpec = GetStringSafe(sqldr, eRst.SampleSpec)
	sHead.sSampleName = GetStringSafe(sqldr, eRst.SampleName)
	sHead.sLot = GetStringSafe(sqldr, eRst.Lot)
	sHead.sMeasurer = GetStringSafe(sqldr, eRst.Measurer)

	sHead.sShape = GetIntSafe(sqldr, eRst.Shape)
	sHead.sThickness = GetStringSafe(sqldr, eRst.Thickness)
	sHead.dStacks = GetDoubleSafe(sqldr, eRst.Stacks)

	sHead.sMaskTime = GetStringSafe(sqldr, eRst.MaskTime)
	sHead.sJudgeRange = GetStringSafe(sqldr, eRst.JudgeRange)
	sHead.sDeviceSpec = GetStringSafe(sqldr, eRst.DeviceSpec)
	sHead.sHPPosControl = GetIntSafe(sqldr, eRst.HPPosControl)
	sHead.dHPTemp = GetDoubleSafe(sqldr, eRst.HPTemprature)
	sHead.dPBTemp = GetDoubleSafe(sqldr, eRst.HPTemprature)
	sHead.dSGTemp = GetDoubleSafe(sqldr, eRst.SGTemp)
	sHead.sHPTempFlucRange = GetDoubleSafe(sqldr, eRst.HPTempFlucRange)
	sHead.sPBTempFlucRange = GetDoubleSafe(sqldr, eRst.PBTempFlucRange)
	sHead.sTempProfile = GetStringSafe(sqldr, eRst.TempProfile)
	sHead.dInputGap = GetDoubleSafe(sqldr, eRst.InputGap)
	sHead.dGapValue = GetDoubleSafe(sqldr, eRst.GapValue)
	sHead.dMeasurePress = GetDoubleSafe(sqldr, eRst.MeasurePresure)
	sHead.iPressContStep = GetIntSafe(sqldr, eRst.PressContStep)
	sHead.iPressContDelay = GetIntSafe(sqldr, eRst.PressContDelay)
	sHead.dPBDiameter = GetDoubleSafe(sqldr, eRst.PBDiameter)
	sHead.sStrsDetectMode = GetIntSafe(sqldr, eRst.StressDetectMode)
	sHead.sAmpMode = GetIntSafe(sqldr, eRst.AmplitudeMode)
	sHead.dAmpModeStress = GetDoubleSafe(sqldr, eRst.LimitStress)

	sHead.dXAmp = GetDoubleSafe(sqldr, eRst.XAmplitude)
	sHead.dXPrd = GetDoubleSafe(sqldr, eRst.XPeriod)
	sHead.dYAmp = GetDoubleSafe(sqldr, eRst.YAmplitude)
	sHead.dYPrd = GetDoubleSafe(sqldr, eRst.YPeriod)
	sHead.dZAmp = GetDoubleSafe(sqldr, eRst.ZAmplitude)
	sHead.dZPrd = GetDoubleSafe(sqldr, eRst.ZPeriod)

	sHead.dInputWait = GetDoubleSafe(sqldr, eRst.InputWait)
	sHead.dHPLiftWait = GetDoubleSafe(sqldr, eRst.HPLiftWait)
	sHead.dXYActWait = GetDoubleSafe(sqldr, eRst.XYActWait)

	sHead.dMeasTimeLimit = GetDoubleSafe(sqldr, eRst.MeasTimeLimit)
	sHead.dMeasEndStress = GetDoubleSafe(sqldr, eRst.MeasEndStress)

	sHead.sFileName = GetStringSafe(sqldr, eRst.FileName)

	sHead.sMeasureType = GetStringSafe(sqldr, eRst.MeasureType)
	sHead.sCalcMethod = GetStringSafe(sqldr, eRst.CalculationMethod)
	sHead.sThreshold = GetDoubleSafe(sqldr, eRst.StressThreshold)
	sHead.dDiffInterval = GetDoubleSafe(sqldr, eRst.SLOPEDiffInterval)

	sHead.dGelTime = GetDoubleSafe(sqldr, eRst.GelTime)
	sHead.sGelTimeJudge = GetStringSafe(sqldr, eRst.GelTimeJudge)
	sHead.dGelTimeTemp = GetDoubleSafe(sqldr, eRst.GelTimeTemp)
	sHead.dGelTimeVisco = GetDoubleSafe(sqldr, eRst.GelTimeVisco)

	sHead.sFilter = GetStringSafe(sqldr, eRst.Filter)
	sHead.dOosValX = GetIntSafe(sqldr, eRst.OOSValX)
	sHead.dOosValY = GetIntSafe(sqldr, eRst.OOSValY)
	sHead.dOosValZ = GetIntSafe(sqldr, eRst.OOSValZ)
	sHead.iCpuCommErrCount = GetIntSafe(sqldr, eRst.CPUCommErrCnt)

	sHead.sToolVersion = GetStringSafe(sqldr, eRst.ToolVersion)
	sHead.sEquipVersion = GetStringSafe(sqldr, eRst.EquipVersion)
	sHead.sXYSensorID = GetStringSafe(sqldr, eRst.XYSensID)
	sHead.sZSensorID = GetStringSafe(sqldr, eRst.ZSensID)

	sHead.iIntegTimePowOn = GetIntSafe(sqldr, eRst.IntegTimePowOn)
	sHead.iIntegTimeHeatOn = GetIntSafe(sqldr, eRst.IntegTimeHeatOn)
	sHead.iIntegTimePBHeat = GetIntSafe(sqldr, eRst.IntegTimePBHeat)
	sHead.iIntegTimeSGHeat = GetIntSafe(sqldr, eRst.IntegTimeSGHeat)
	sHead.iIntegTimeXAct = GetIntSafe(sqldr, eRst.IntegTimeXAct)
	sHead.iIntegTimeYAct = GetIntSafe(sqldr, eRst.IntegTimeYAct)
	sHead.iIntegTimeZAct = GetIntSafe(sqldr, eRst.IntegTimeZAct)

	sHead.dXYWeightCoef = GetDoubleSafe(sqldr, eRst.XYWeightCoef)
	sHead.dZWeightCoef = GetDoubleSafe(sqldr, eRst.ZWeightCoef)
	sHead.sFullCalibValue = GetIntSafe(sqldr, eRst.FullCalibrationVlue)
	sHead.sZeroCalibValue = GetIntSafe(sqldr, eRst.ZeroCalibrationVlue)
	sHead.sFullStressAttenuation = GetDoubleSafe(sqldr, eRst.FullStressAttenuation)
	sHead.sFullStressPhaseX = GetDoubleSafe(sqldr, eRst.FullStressPxPhase)
	sHead.sFullStressPhaseY = GetDoubleSafe(sqldr, eRst.FullStressPyPhase)
	sHead.sFullStressPhaseZ = GetDoubleSafe(sqldr, eRst.FullStressPzPhase)
	sHead.dGapCorrect = GetDoubleSafe(sqldr, eRst.GAPCORR)
	sHead.dZNoLoadStress = GetDoubleSafe(sqldr, eRst.NoLoadZStress)
	sHead.dZStab = GetDoubleSafe(sqldr, eRst.ZSTAB)
	sHead.sZeroAdjStabMonTime = GetDoubleSafe(sqldr, eRst.ZeroAdjStabMonTime)
	sHead.sZeroAdjFlucRange = GetDoubleSafe(sqldr, eRst.ZeroAdjFlucRange)
	sHead.dZZeroPosition = GetDoubleSafe(sqldr, eRst.ZZeroPosition)
	sHead.sHPCenterX = GetIntSafe(sqldr, eRst.HPCenterX)
	sHead.sHPCenterY = GetIntSafe(sqldr, eRst.HPCenterY)

	sHead.dTempCnt_P = GetDoubleSafe(sqldr, eRst.TempCnt_P)
	sHead.dTempCnt_I = GetDoubleSafe(sqldr, eRst.TempCnt_I)
	sHead.dTempCnt_d = GetDoubleSafe(sqldr, eRst.TempCnt_D)
	sHead.dTempCnt_Dgain = GetDoubleSafe(sqldr, eRst.TempCnt_Dgain)
	sHead.dTempCnt_PVBias = GetDoubleSafe(sqldr, eRst.TempCnt_PVBias)
	sHead.dTempCnt_PVGain = GetDoubleSafe(sqldr, eRst.TempCnt_PVGain)

	sHead.dTempCnt_P2 = GetDoubleSafe(sqldr, eRst.TempCnt_P2)
	sHead.dTempCnt_I2 = GetDoubleSafe(sqldr, eRst.TempCnt_I2)
	sHead.dTempCnt_d2 = GetDoubleSafe(sqldr, eRst.TempCnt_D2)
	sHead.dTempCnt_Dgain2 = GetDoubleSafe(sqldr, eRst.TempCnt_Dgain2)
	sHead.dTempCnt_PVBias2 = GetDoubleSafe(sqldr, eRst.TempCnt_PVBias2)
	sHead.dTempCnt_PVGain2 = GetDoubleSafe(sqldr, eRst.TempCnt_PVGain2)

	sHead.dTempCnt_P3 = GetDoubleSafe(sqldr, eRst.TempCnt_P3)
	sHead.dTempCnt_I3 = GetDoubleSafe(sqldr, eRst.TempCnt_I3)
	sHead.dTempCnt_d3 = GetDoubleSafe(sqldr, eRst.TempCnt_D3)
	sHead.dTempCnt_Dgain3 = GetDoubleSafe(sqldr, eRst.TempCnt_Dgain3)
	sHead.dTempCnt_PVBias3 = GetDoubleSafe(sqldr, eRst.TempCnt_PVBias3)
	sHead.dTempCnt_PVGain3 = GetDoubleSafe(sqldr, eRst.TempCnt_PVGain3)

	sHead.sExtStartTime = GetStringSafe(sqldr, eRst.ExtStartTime)
	sHead.sExtMiddleTime = GetStringSafe(sqldr, eRst.ExtMiddleTime)
	sHead.sExtEndTime = GetStringSafe(sqldr, eRst.ExtEndTime)
	sHead.sExtTanSample = GetStringSafe(sqldr, eRst.ExtTanSample)
	sHead.sExtTanThinning = GetStringSafe(sqldr, eRst.ExtTanThinning)
	sHead.sExtGelTimeSelect = GetStringSafe(sqldr, eRst.ExtGelTimeSelect)
	sHead.sExtGelTimeUnder = GetStringSafe(sqldr, eRst.ExtGelTimeUnder)
	sHead.sExtGelTimeMiddle = GetStringSafe(sqldr, eRst.ExtGelTimeMiddle)
	sHead.sExtGelTimeOver = GetStringSafe(sqldr, eRst.ExtGelTimeOver)

	sHead.sMelStartTemp = GetStringSafe(sqldr, eRst.MELTStartTemp)
	sHead.sMelEndTemp = GetStringSafe(sqldr, eRst.MELTEndTemp)
	sHead.sMelWaitTime = GetStringSafe(sqldr, eRst.MELTWaitTime)
	sHead.sMelSpeed = GetStringSafe(sqldr, eRst.MELTSpeed)
	sHead.sMelTemperatureRslt = GetStringSafe(sqldr, eRst.MELTTemperature)
	sHead.sMelViscosityRslt = GetStringSafe(sqldr, eRst.MELTViscosity)

	sHead.sFLCTemp = GetStringSafe(sqldr, eRst.FLCTemp)
	sHead.sProbeDiameter = GetStringSafe(sqldr, eRst.ProbeDiameter)
	sHead.sFLCSampleTime = GetStringSafe(sqldr, eRst.FLCSampleTime)
	sHead.sFLCSampleAccRate = GetStringSafe(sqldr, eRst.FLCSampleAccRate)
	sHead.sFLCAmplitude = GetStringSafe(sqldr, eRst.FLCAmplitude)
	sHead.sFLCPeriod = GetStringSafe(sqldr, eRst.FLCPeriod)

	sHead.sFLC1AmplitudeStart = GetStringSafe(sqldr, eRst.FLC1AmplitudeStart)
	sHead.sFLC1AmplitudeEnd = GetStringSafe(sqldr, eRst.FLC1AmplitudeEnd)
	sHead.sFLC1AmplitudeStep = GetStringSafe(sqldr, eRst.FLC1AmplitudeStep)
	sHead.sFLC1MeasRev = GetStringSafe(sqldr, eRst.FLC1MeasRev)

	sHead.sFLC2Repeat = GetStringSafe(sqldr, eRst.FLC2Repeat)
	sHead.sFLC2Ope = GetStringSafe(sqldr, eRst.FLC2Ope)
	sHead.sFLC2Rest = GetStringSafe(sqldr, eRst.FLC2Rest)

	sHead.sFLC3TempStart = GetStringSafe(sqldr, eRst.FLC3TempStart)
	sHead.sFLC3TempEnd = GetStringSafe(sqldr, eRst.FLC1AmplitudeEnd)
	sHead.sFLC3TempStep = If(IsDBNull(GetStringSafe(sqldr, eRst.FLC3TempStep)), "" , GetStringSafe(sqldr, eRst.FLC3TempStep))

	sHead.iMeasActTime		= If(IsDBNull(GetStringSafe(sqldr, eRst.Reserve0)), "" , GetStringSafe(sqldr, eRst.Reserve0))

Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub
'統計用SELECT文  @XXXXX@を置換して使用する
Public p_statisticsSqlBase As String = 
"SELECT 
[GelTime]
,[EquipID]
,[DateTime]
,[SampleSpec]
,[SampleName]
,[Lot]
,[DeviceSpec]
,[HPPosControl]
,[HPTemprature]
,[PBTemprature]
,[TempProfile]
,[GapValue]
,[MeasurePresure]
,[PBDiameter]
,[StressDetectMode]
,[AmplitudeMode]
,[XAmplitude]
,[XPeriod]
,[YAmplitude]
,[YPeriod]
,[ZAmplitude]
,[ZPeriod]
,[HPLiftWait]
,[XYActWait]
,[FileName]
,[MeasureType]
,[CalculationMethod]
,[StressThreshold]
,[SLOPEDiffInterval]
,[Filter]
,[XYSensID]
,[ZSensID]
,[Reserve0]
  FROM [dbo].[tblResults]
  WHERE 
 [MeasureType]		= '@MESTYPE@'		 AND 
[EquipID]			= '@DEVICE@'		 AND 
[SampleSpec]		= '@SAMPLESPEC@'	 AND 
[SampleName]		= '@SAMPLENAME@'	 AND 
[Lot]				= '@LOT@'			 AND 
[DeviceSpec]		= '@DEVICESPEC@'	 AND 
[CalculationMethod]	= '@CALCMETHOD@'	 AND 
[StressThreshold]	= @THRESHOLD@		 AND 
/*[DateTime] >= CONVERT(date, GETDATE()) AND [DateTime] <  CONVERT(date, DATEADD(day, 1, GETDATE())) AND */																/*本日のみ*/
/*[DateTime] >= DATEFROMPARTS(YEAR(GETDATE()), MONTH(GETDATE()), 1) AND [DateTime] <  DATEADD(month, 1, DATEFROMPARTS(YEAR(GETDATE()), MONTH(GETDATE()), 1)) AND */		/*今月のみ*/
/*[DateTime] >= DATEFROMPARTS(YEAR(GETDATE()), 1, 1) AND [DateTime] <  DATEFROMPARTS(YEAR(GETDATE()) + 1, 1, 1) AND */													/*今年のみ*/
/*[DateTime] >= DATEADD(month, -2, DATEFROMPARTS(YEAR(GETDATE()), MONTH(GETDATE()), 1)) AND [DateTime] <  DATEADD(day, 1, CONVERT(date, GETDATE())) AND */				/*過去2ヶ月前の月初〜今日まで*/
[GelTime]			>= 0;
"
'SPC用SELECT文  @XXXXX@を置換して使用する
Public p_SpcSqlBase As String = 
"SELECT 
[GelTime]
  FROM [dbo].[tblResults]
  WHERE 
 [MeasureType]		= '@MESTYPE@'		 AND 
[EquipID]			= '@DEVICE@'		 AND 
[SampleName]		= '@SAMPLENAME@'	 AND 
[Lot]				= '@LOT@'			 AND 
[CalculationMethod]	= '@CALCMETHOD@'	 AND 
[StressThreshold]	= @THRESHOLD@		 AND 
[DateTime] >= CONVERT(datetime, '@STATMIN@') AND 
[DateTime] <  CONVERT(datetime, '@STATMAX@') AND 
[GelTime]			>= 0;
"
'SPC用SELECT文  @XXXXX@を置換して使用する(SQLite版)
Public p_SpcSqlBase_SQLite As String = 
"SELECT 
GelTime
  FROM tblResults
  WHERE 
MeasureType			= '@MESTYPE@'		 AND 
EquipID				= '@DEVICE@'		 AND 
SampleName			= '@SAMPLENAME@'	 AND 
Lot					= '@LOT@'			 AND 
CalculationMethod	= '@CALCMETHOD@'	 AND 
StressThreshold		= @THRESHOLD@		 AND 
DateTime	>= '@STATMIN@' AND 
DateTime	<  '@STATMAX@' AND 
GelTime		>= 0;
"
'tblResultsテーブルフィールド順番
Public Enum eStaRst
	'1
	GelTime = 0
	EquipID
	DateTime
	SampleSpec
	SampleName
	Lot
	DeviceSpec
	HPPosControl
	HPTemprature
	PBTemprature
	TempProfile
	GapValue
	MeasurePresure
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
	HPLiftWait
	XYActWait
	FileName
	MeasureType
	CalculationMethod
	StressThreshold
	SLOPEDiffInterval
	Filter
	XYSensID
	ZSensID
	Reserve0						'予備(測定時間で使用)
	Max
End Enum
''' <summary>
''' 統計用Select文指示パラメータ構造体
''' </summary>
Public Structure stStaticsParameter
	Dim sMeasType					As String		'測定種類
	Dim sEquipID					As String		'デバイスID
	Dim sSampleSpec					As String		'サンプルSPEC名
	Dim sDeviceSpec					As String		'デバイスSPEC名
	Dim sSampleName					As String		'サンプル名
	Dim sLot						As String		'ロット
	Dim sCalculationMethod			As String		'計算手法
	Dim sStressThreshold			As String		'閾値
	Dim sStatMin					As String		'統計範囲 yyyy/MM/dd HH:mm
	Dim sStatMax					As String		'統計範囲 yyyy/MM/dd HH:mm
End Structure

''' <summary>
''' 統計用Select文を編集
''' </summary>
''' <param name="sSql"></param>
''' <param name="uPara"></param>
Public Sub StaticsSqlEdit(ByRef sSql As String, ByVal uPara As stStaticsParameter)
	sSql = sSql.Replace("@MESTYPE@",	uPara.sMeasType)
	sSql = sSql.Replace("@DEVICE@",		uPara.sEquipID)
	sSql = sSql.Replace("@SAMPLESPEC@", uPara.sSampleSpec)
	sSql = sSql.Replace("@SAMPLENAME@", uPara.sSampleName)
	sSql = sSql.Replace("@LOT@",		uPara.sLot)
	sSql = sSql.Replace("@DEVICESPEC@", uPara.sDeviceSpec)
	sSql = sSql.Replace("@CALCMETHOD@", uPara.sCalculationMethod)
	sSql = sSql.Replace("@THRESHOLD@",	uPara.sStressThreshold)
	sSql = sSql.Replace("@STATMIN@",	uPara.sStatMin)
	sSql = sSql.Replace("@STATMAX@",	uPara.sStatMax)
	sSql = sSql.Replace(vbTab,	"")
	sSql = sSql.Replace(vbCrLf,	"")
End Sub

''' <summary>
''' 統計用にSELECT文を実行する
''' </summary>
''' <param name="sSql">SQL文字列</param>
''' <param name="head">DataFileHeader</param>
''' <returns>0:成功 -1:失敗</returns>
Public Function fnStaticsSelectExecute(ByVal sSql As String, ByRef head() As datafile.DataFileHeader) As Int32
Dim sqlcmd	As New SqLiteCommand()
Dim sqldr	As SqLiteDataReader
Dim lRet	As Int32
Dim idx		As int32 = 0
Try
	If p_DBCon.State <> ConnectionState.Open
		lRet = connectDb(p_DBCon)
		If lRet < 0
			'接続失敗
			Return lRet
		End If
	End If
	sqlcmd.Connection = p_DBCon
	sqlcmd.CommandType = CommandType.Text
	sqlcmd.CommandText = sSql
	sqldr = sqlcmd.ExecuteReader()
	Array.Resize(head, idx)
	While sqldr.Read()      'SQLReader行リード
		Array.Resize(head, idx + 1)
		head(idx).dGelTime = Double.Parse(sqldr(eStaRst.GelTime))
		idx += 1
	End While
	sqldr.Close
	p_DBCon.Close
	Return 0

Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	sqlcmd.Dispose()
	p_DBCon.Close()
	Return -1
End Try
End Function
''' <summary>
''' SPC用にSELECT文を実行する
''' </summary>
''' <param name="sSql">SQL文字列</param>
''' <param name="dVals">数値配列</param>
''' <returns>0:成功 -1:失敗</returns>
Public Function fnSpcSelectExecute(ByVal sSql As String, ByRef dVals() As Double) As Int32
Dim idx		As int32 = 0
Try
	Using conn As New SQLiteConnection(m_ConnString)
		conn.Open()
		Using sqlcmd As New SQLitecommand("", conn)
			sqlcmd.CommandText = sSql
			Using sqldr As SqLiteDataReader = sqlcmd.ExecuteReader()
				Array.Resize(dVals, idx)
				While sqldr.Read()      'SQLReader行リード
					Array.Resize(dVals, idx + 1)
					dVals(idx) = Double.Parse(sqldr(eStaRst.GelTime))
					idx += 1
				End While
			End Using
		End Using
	End Using
	Return 0
Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Return -1
End Try
End Function


End Class



