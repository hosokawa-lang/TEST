Imports System.IO
Imports System.Windows.Forms
Imports System.Windows.Forms.DataVisualization.Charting
Imports Microsoft.VisualBasic.FileIO
Imports System.Numerics
Imports SZKTool.DataFile
Imports SZKTool.frmOptions

Module modChart
Public p_iScrlStart		As Int64 = 0		'チャートスクロール開始(秒)
'測定表示チャートの選択項目番号
Public Enum eChartItem 
	OxBuf			= 0
	OyBuf
	OzBuf
	OzPos
	Hosei			= 5
	TempBuf
	MvBuf
	TempBuf2
	MvBuf2
	Viscosity
	XPosPhase
	SplStrain
	ShearMod
	ShearVelo
	XStrgMod
	XElasMod
	XLossTan
	HoseiP
	MAX
End Enum
'ゲルタイムチャート軸タイトル用
Public sChartItemName() As String = Nothing
Public sChartItemUnit() As String = Nothing
'ゲルタイムチャート用データテーブル列名
Public sChartItemStr() As String = {"X Stress","Y Stress","Z Stress","Z Position","","Correction","HP Temprature","HP MV","PB Temprature","PB MV","Viscosity","PosPhase","SampStrain","ShearMod","ShearVelo","StorageMod","ElasticMod","LossTan","CorrectionP"}
'グラフ選択文字列配列定義(eChartItemと連携)
Public sGraphSelectionArry() As String = {"rbXStress","rbYStress","rbZStress","rbZPosition","rbCorrect","rbHPTemp","rbHPMV","rbPBTemp","rbPBMV","rbViscosity","rbPosPhase","rbSampleStrain","rbShearMod","rbShearVelo","rbStorageMod","rbElasticMod","rbLossTangent","rbCorrectP"}
Public eNum iGrpItmN
	rbXStress		= 0
	rbYStress		
	rbZStress
	rbZPosition
	rbCorrect		= 4
	rbHPTemp		= 5
	rbHPMV
	rbPBTemp
	rbPBMV
	rbViscosity
	rbPosPhase		= 10
	rbSampleStrain
	rbShearMod
	rbShearVelo
	rbStorageMod
	rbElasticMod
	rbLossTangent
	rbCorrectP
End eNum
''' <summary>
''' RealTimeDisp呼び出しの引数構造体
''' </summary>
Public Structure ChartArg
	Dim	iThinningType		As Int16			'0:間引きなし 1:既定の間引き
	Dim sSelectGItem		As String       	'グラフ選択
	Dim iSelectGItem		As int16       		'グラフ選択番号
    Dim bHist				As Boolean      	'履歴描画 有効無効
    Dim bStd	            As Boolean      	'基準描画 有効無効
    Dim dYSpanPer			As Double       	'Y軸拡大率
    Dim dLimitTime			As Double       	'測定時間制限
    Dim sCalcMethod			As String       	'計算方法
	Dim iGelTimeJudge		As int16        	'ゲルタイム判定
    Dim dJudgeRangeMin		As Double       	'ゲルタイム判定範囲
    Dim dJudgeRangeMax		As Double       	'ゲルタイム判定範囲
    Dim dAprxRangeMin		As Double       	'有効範囲
    Dim dAprxRangeMax		As Double       	'有効範囲
	Dim dGelTime			As Double       	'ゲルタイム 測定値
	Dim dToqueThreshold		As Double       	'ゲルタイム 応力閾値
	Dim dFullStressADVal	As Double       	'ゲルタイム フル応力
	Dim dMeltTemp			As Double       	'MELT法 温度測定値
	Dim dMeltVisco			As Double       	'MELT法 粘度測定値
	Dim dTempMin			As Double       	'横軸温度 温度範囲
	Dim dTempMax			As Double       	'横軸温度 温度範囲
End Structure

'ヘッダ情報
Private header	As DataFileHeader = Nothing
Public Writeonly Property HeaderInfo() As DataFileHeader
    Set(value As DataFileHeader)
        header = value
    End Set
End Property

'リアルタイムトレンドチャート描画 サンプル間引き数
Private m_iRealTimeThinning_R         As Int32	'粗い 応力以外の表示間引き(温度、補正など)
Private m_iRealTimeThinning_D		  As Int32	'精細 
Public Property RealTimeThinning(ByVal Sel As int32) As Int32
	Get
        Dim Ret As int32
        Select Case Sel
        Case 0
            Ret = m_iRealTimeThinning_R
        Case 1
            Ret = m_iRealTimeThinning_D
        End Select
        Return Ret
	End Get
    Set(value As Int32)
        Select Case Sel
        Case 0
            m_iRealTimeThinning_R = value
        Case 1
            m_iRealTimeThinning_D = value
        End Select
    End Set
End Property
Public Property RealTimeThininingValueR As Int32
	Get 
		RealTimeThininingValueR = m_iRealTimeThinning_R
	End Get
    Set(value As Int32)
		m_iRealTimeThinning_R = value
    End Set
End Property
Public Property RealTimeThininingValueD As Int32
	Get 
		RealTimeThininingValueD = m_iRealTimeThinning_D
	End Get
    Set(value As Int32)
		m_iRealTimeThinning_D = value
    End Set
End Property
'リアルタイムトレンドチャートX軸掛け率 1個 / 2mS * 1000ms / 間引き数
Private m_dRealXaxisRatio   As Double = 1 / MEAS_TIME_RESOLUTION  * 1000.0 / m_iRealTimeThinning_R
'Private m_dRealXaxisRatio_M As Double = 1 / MEAS_TIME_RESOLUTION  * 1000.0 / m_iRealTimeThinning_M
'Private m_dRealXaxisRatio_S As Double = 1 / MEAS_TIME_RESOLUTION  * 1000.0 / m_iRealTimeThinning_D
Public ReadOnly Property RealXaxisRatio(ByVal Sel As int16) As Double
	Get
        Select Case Sel
        Case 0
            RealXaxisRatio = 1 / MEAS_TIME_RESOLUTION  * 1000.0 / m_iRealTimeThinning_R
        Case 1
            RealXaxisRatio = 1 / MEAS_TIME_RESOLUTION  * 1000.0 / m_iRealTimeThinning_D
		Case Else
            RealXaxisRatio = 1 / MEAS_TIME_RESOLUTION  * 1000.0 / m_iRealTimeThinning_D
        End Select
	End Get
End Property

'トレンドチャート描画 サンプル間引き数
Private m_iThinning As Int16 = 1                                                         
Public Property Thinning() As Int16
	Get
        Return m_iThinning
	End Get
    Set(value As Int16)
        m_iThinning = value
    End Set
End Property
'トレンドチャートX軸掛け率 1個 / 2mS * 1000ms / 間引き数
Private m_dXaxisRatio As Double = 1 / MEAS_TIME_RESOLUTION  * 1000.0 / m_iThinning        
Public ReadOnly Property XaxisRatio() As Double
	Get
        m_dXaxisRatio = 1 / MEAS_TIME_RESOLUTION  * 1000.0 / m_iThinning
        Return m_dXaxisRatio
	End Get
End Property

''' <summary> frmMAin Language File 文字列格納場所 </summary>
Private m_sMes(efrmMainSTR.iMAX - 1) As String
Public Sub ChartItemSet(ByVal s() As string)
	If Not IsNothing(s)
		Array.Copy(s, m_sMes, s.Length)
	End If
End Sub 
Private m_sSelectGItem		As String    '測定データ選択項目(ラジオボタン名)
Private complexData()		As Complex
Private m_Disp              As Integer = -1
Private Const cInitXSec		As Integer = 100	'初期画面のX軸時間表示スパン(秒)

'系列の表示非表示
'Private bSeriesDisp(cAllBufMax-1)		As Boolean = {True,}



#If 1
Dim ds		As DataSet		= New DataSet()
Dim dt		As DataTable	= New DataTable()

''' <summary>
''' リアルタイム測定時のトレンド表示（frmMain タイマーイベントからコールされる）
''' </summary>
''' <param name="arg">共通引数構造体
'''		iThinningType			0:間引きなし 1:既定の間引き
''' 	sSelectGItem			グラフ選択
''' 	bHist					履歴描画 有効無効
''' 	bStd					基準描画 有効無効
''' 	dYSpanPer				Y軸拡大率
''' 	dLimitTime				測定時間制限
''' 	sCalcMethod				計算方法
''' 	iGelTimeJudge			ゲルタイム判定
''' 	dJudgeRangeMin			ゲルタイム判定範囲
''' 	dJudgeRangeMax			ゲルタイム判定範囲
''' 	dAprxRangeMin			有効範囲
''' 	dAprxRangeMax			有効範囲
''' 	dGelTime				ゲルタイム 測定値
''' 	dToqueThreshold			ゲルタイム 応力閾値
''' 	dFullStressADVal		ゲルタイム フル応力
''' 	dMeltTemp				MELT法 温度測定値
''' 	dMeltVisco				MELT法 粘度測定値
''' 	dTempMin				横軸温度 温度範囲
''' 	dTempMax				横軸温度 温度範囲</param>
''' <param name="dFil">データファイルクラス</param>
''' <param name="Chart1">チャートコントロール</param>
''' <param name="clExt">外挿法クラス</param>
''' <param name="cts">オプション設定クラス</param>
''' <param name="bRealTrendUpdate">初期設定（チャート設定、データテーブル定義など）行うか True：行わない False：行う</param>
Public Sub RealChartDisp1( _
	ByRef arg		As ChartArg, _
	ByRef dFil		As DataFile, _
	ByRef Chart1	As Chart, _
	ByRef clExt		As Extra, _
	ByRef cts		As CToolSettings, _
	ByRef bRealTrendUpdate As Boolean _
)
Dim dTemp()		As Double = Nothing
Dim dMv()		As Double = Nothing
Dim dTemp2()	As Double = Nothing
Dim dMv2()		As Double = Nothing
Dim strColumnName		As String = String.Empty
Dim intCsvRowCounter	As Integer = 1
Dim iHis		As Int16 = 0            '履歴バッファ使用数
Dim iStd		As Int16 = 0            '基準バッファ使用数
Dim ChartGrid   As Integer
Dim iThinTyp	As int16 
Try
	iHis = If(arg.bHist,	dFil.CheckHist(), 0)		'履歴バッファ使用数検査
	iStd = If(arg.bStd,		dFil.CheckStd(), 0)			'基準バッファ使用数検査

    Select Case cts.penGrids.DashStyle				'グリッドスタイルPen形式からChartへ変換
    Case Drawing2D.DashStyle.Solid
        ChartGrid = ChartDashStyle.Solid
    Case Drawing2D.DashStyle.Dash
        ChartGrid = ChartDashStyle.Dash
    Case Drawing2D.DashStyle.Dot
        ChartGrid = ChartDashStyle.Dot
    Case Else
        ChartGrid = ChartDashStyle.NotSet
    End Select

    'グラフサンプル数計算 ------------------
	If arg.iThinningType = 1
	'規定間引き(リアルタイム)
		Select Case arg.sSelectGItem
		Case "rbXStress","rbYStress","rbZStress"
			iThinTyp = 1
		Case Else
			iThinTyp = 0
		End Select
	Else
	'間引きなし(最終描画)
		iThinTyp = 1
	End If

	Dim iSampleNumOnGraph	As Int64
	Dim iScrolSampleNum		As int64
    If Not isNothing(dFil.iOxBuf)
	'最新測定データがある ----------------------------------------------------------
        If cts.iRTXAxis = 0 AndAlso cts.iChartType = 0
		'自動モード
			iScrolSampleNum = ((cts.iScrlWidth+1) * 1000) / MEAS_TIME_RESOLUTION
			If dFil.iOxBuf.GetUpperBound(0) > iScrolSampleNum AndAlso arg.iThinningType = 1
			'スクロールモード 間引き表示モード含む
	            iSampleNumOnGraph = dFil.iOxBuf.GetUpperBound(0)
				p_iScrlStart = dFil.iOxBuf.Length - iScrolSampleNum 
			Else
	            iSampleNumOnGraph = dFil.iOxBuf.GetUpperBound(0)
				p_iScrlStart = 0
			End If
        Else
		'固定モード
            'iSampleNumOnGraph = ( arg.dLimitTime * 1000.0 ) / MEAS_TIME_RESOLUTION
			Select Case cts.iChartType
			Case 0
				iSampleNumOnGraph = ( cts.MainGraphXMax(cts.iChartType) * 1000.0 ) / MEAS_TIME_RESOLUTION
			Case 1
				If Not IsNothing(dFil.iTempBuf)
					iSampleNumOnGraph = dFil.iTempBuf.GetUpperBound(0)
				Else
					iSampleNumOnGraph = cts.MainGraphXMax(cts.iChartType) * 1000.0
				End If
			End Select
			p_iScrlStart = 0
        End If
	ElseIf  iHis<>0 Or iStd<>0
	'最新測定データはなく、履歴または基準データがある ------------------------------
		Dim iMax As Int32 = 0
		iMax = GetArrayMaxInHisStd(iHis, iStd, dFil)
        If cts.iRTXAxis = 0  AndAlso cts.iChartType = 0
		'自動モード
			iScrolSampleNum = ((cts.iScrlWidth+1) * 1000) / MEAS_TIME_RESOLUTION
			If iMax-1 > iScrolSampleNum AndAlso arg.iThinningType = 1
			'スクロールモード 間引き表示モード含む
	            iSampleNumOnGraph = iMax-1
				p_iScrlStart = iMax - iScrolSampleNum 
			Else
	            iSampleNumOnGraph = iMax-1
				p_iScrlStart = 0
			End If
        Else
		'固定モード
            'iSampleNumOnGraph = ( arg.dLimitTime * 1000.0 ) / MEAS_TIME_RESOLUTION
            iSampleNumOnGraph = ( cts.MainGraphXMax(cts.iChartType) * 1000.0 ) / MEAS_TIME_RESOLUTION
			p_iScrlStart = 0
        End If
	Else
	'未測定画面の表示(初期表示) -------------------------------------------------------
        'iSampleNumOnGraph = ( cInitXSec * 1000.0 ) / MEAS_TIME_RESOLUTION			'100秒分のデータ
        'iSampleNumOnGraph = ( cts.MainGraphXMax(cts.iChartType) * 1000.0 ) / MEAS_TIME_RESOLUTION			'100秒分のデータ
		Select Case cts.iChartType
		Case 0 '時間
			If cts.iRTXAxis = 0
			'X軸自動
				iSampleNumOnGraph = ( cInitXSec * 1000.0 ) / MEAS_TIME_RESOLUTION		'100秒分のデータ
			Else
			'X軸固定
				iSampleNumOnGraph = ( cts.MainGraphXMax(cts.iChartType) * 1000.0 ) / MEAS_TIME_RESOLUTION
			End If
		Case 1 '温度
			If Not IsNothing(dFil.iTempBuf)
				iSampleNumOnGraph = dFil.iTempBuf.GetUpperBound(0)
			Else
				If cts.iRTXAxis = 0
				'X軸自動
					iSampleNumOnGraph = 300		'0-300℃分のデータ
				Else
				'X軸固定
					iSampleNumOnGraph = cts.MainGraphXMax(cts.iChartType)
				End If
			End If
		End Select
		
	End If
	'---------------------------------------

	'温度系の選択時のワーク配列確保
	Select case arg.sSelectGItem
	Case "rbHPTemp"
		Array.Resize(dTemp,		iSampleNumOnGraph)
	Case "rbHPMV"
		Array.Resize(dMv,		iSampleNumOnGraph)
	Case "rbPBTemp"
		Array.Resize(dTemp2,	iSampleNumOnGraph)
	Case "rbPBMV"
		Array.Resize(dMv2,		iSampleNumOnGraph)
	Case Else
	End Select

	'データテーブルの設定
	If Not bRealTrendUpdate
		RealTimeChartDataTableDefine(cts.iChartType, ds, dt, arg.sSelectGItem, iHis, iStd, arg.sCalcMethod)
	End If
	'データテーブルへのデータ追加 20210210
	ds.Tables(0).Rows.Clear()
	RealTimeDataTableUpdate( _
		arg, _
		dFil, _
		cts, _
		ds.Tables(0), _
		iHis, _
		iStd, _
		iSampleNumOnGraph, _
		iThinTyp _
		)

	If Not bRealTrendUpdate
		'チャートコントロールの設定
		RealTimeChartSetting(cts, Chart1, ds.Tables(0), iHis, iStd, arg.sSelectGItem, arg.sCalcMethod)
		bRealTrendUpdate = True
	End If

#If 1
	'データプロット
	Select Case cts.iChartType
	Case 0
	'横軸 時間
		Chart1.DataBind()														'チャートコントロールにデータバインド
	Case 1
	'横軸 温度
		DataPlotXTemp1(Chart1, dFil, iHis, iStd, arg.sSelectGItem, iThinTyp)		'データ点プロット
	End Select
#end if

    '-----------------------------------------------------------------------------------------------------
    '以下、Ｘ軸とＹ軸の設定
    ChartXYAxisSetting( _
			arg, _
            Chart1, _
            dFil, _
            clExt, _
            cts, _
            iHis, _
            iStd, _
            ChartGrid, _
            ds, _
            dTemp, _
            dMv, _ 
            iThinTyp _ 
    )

	'閾値・ゲルタイムライン表示 (補正データ表示のみ)
	ChartUpdateGelTimeLine( 
			arg, _
            Chart1, _
            clExt, _
            arg.sSelectGItem, _
            cts, _
            iThinTyp _ 
	)
#If 0
	Select Case cts.iChartType
	Case 0
	'横軸 時間
		Chart1.DataBind()														'チャートコントロールにデータバインド
	Case 1
	'横軸 温度
		DataPlotXTemp1(Chart1, dFil, iHis, iStd, arg.sSelectGItem, iThinTyp)		'データ点プロット
	End Select
#end if
	'ChartAreaのサイズ位置調整
	Chart1.Location = New Point(-2, 43)
	Chart1.Size		= New Size(1245, 900)
	Chart1.ChartAreas(0).Position.Auto					= False
	Chart1.ChartAreas(0).Position.X						= 0
	Chart1.ChartAreas(0).Position.Y						= 9
	Chart1.ChartAreas(0).Position.height				= 88
	Chart1.ChartAreas(0).Position.Width					= 97

	'Chart1.Serializer.Save("C:\Test\Chart1.xml")
	'Chart1.Update()

Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
    Console.Write("error mes:" & ex.Message.ToString)
End Try
End Sub
''' <summary>
''' X軸温度散布図 プロット
''' </summary>
''' <param name="oChart">チャート</param>
''' <param name="dFil">データファイルクラス</param>
''' <param name="iHis">履歴数</param>
''' <param name="iStd">基準数</param>
''' <param name="sSelectGItem">選択系列名</param>
''' <param name="iThinTyp">表示間引きタイプ</param>
Private Sub	DataPlotXTemp1(ByRef oChart As Chart, ByVal dFil As DataFile, ByVal iHis As int16, ByVal iStd As int16, ByVal sSelectGItem As String, ByVal iThinTyp As Int32)
Dim i As Int64
Dim j As Int16
Dim dt As DataTable = ds.Tables(0)
Dim strColumnName As String = dt.Columns(1).ColumnName
Dim tArray() As Long = Nothing
Try
	With oChart
	Dim x As Double
	Dim y As Double

	'基準データ2のプロット
	Dim iIdx As Int16 = -1
	Dim cIdx As Int16 = -1
	If Not IsNothing(dFil.iTempBuf_s2)
		Array.Resize(tArray, dFil.iTempBuf_s2.Length)
		tArray = dFil.iTempBuf_s2
		iIdx = cHistMax+cStdMax
		For j = 0 To dt.Columns.Count-1
			strColumnName = dt.Columns(j).ColumnName							'系列名を取得
			If strColumnName.Contains("7")
				cIdx = j
				Exit for
			End If
		Next
		'基準アイテムの描画
		strColumnName = dt.Columns(cIdx).ColumnName						'系列名を取得
		For i = 0 To tArray.GetUpperBound(0) Step RealTimeThinning(iThinTyp)
			x = tArray(i) / 10.0
			HistYSetForTemp(dFil, iIdx, i, sSelectGItem, y)
			If Not (x = 0 Or y = 0)
				.Series(strColumnName).Points.AddXY(x, y)				'データを設定
			End If
		Next i
		tArray = Nothing
		iIdx = -1
	End If
	'基準データ1のプロット
	If Not IsNothing(dFil.iTempBuf_s1)
		Array.Resize(tArray, dFil.iTempBuf_s1.Length)
		tArray = dFil.iTempBuf_s1
		iIdx = cHistMax+cStdMax-1
		For j = 0 To dt.Columns.Count-1
			strColumnName = dt.Columns(j).ColumnName							'系列名を取得
			If strColumnName.Contains("6")
				cIdx = j
				Exit for
			End If
		Next
		'基準アイテムの描画
		strColumnName = dt.Columns(cIdx).ColumnName						'系列名を取得
		For i = 0 To tArray.GetUpperBound(0) Step RealTimeThinning(iThinTyp)
			x = tArray(i) / 10.0
			HistYSetForTemp(dFil, iIdx, i, sSelectGItem, y)
			If Not (x = 0 Or y = 0)
				.Series(strColumnName).Points.AddXY(x, y)				'データを設定
			End If
		Next i
		tArray = Nothing
		iIdx = -1
	End If

	'測定データ、履歴データのプロット
	For j = cHistMax To 0 Step -1
		Select Case j
		Case cHistMax
			If Not IsNothing(dFil.iTempBuf_h5)
				Array.Resize(tArray, dFil.iTempBuf_h5.Length)
				tArray = dFil.iTempBuf_h5
			End If
		Case cHistMax-1
			If Not IsNothing(dFil.iTempBuf_h4)
				Array.Resize(tArray, dFil.iTempBuf_h4.Length)
				tArray = dFil.iTempBuf_h4
			End If
		Case cHistMax-2
			If Not IsNothing(dFil.iTempBuf_h3)
				Array.Resize(tArray, dFil.iTempBuf_h3.Length)
				tArray = dFil.iTempBuf_h3
			End If
		Case cHistMax-3
			If Not IsNothing(dFil.iTempBuf_h2)
				Array.Resize(tArray, dFil.iTempBuf_h2.Length)
				tArray = dFil.iTempBuf_h2
			End If
		Case cHistMax-4
			If Not IsNothing(dFil.iTempBuf_h1)
				Array.Resize(tArray, dFil.iTempBuf_h1.Length)
				tArray = dFil.iTempBuf_h1
			End If
		Case cHistMax-5
			'If Not IsNothing(dFil.iHosei)
			If Not IsNothing(dFil.iTempBuf)
				Array.Resize(tArray, dFil.iTempBuf.Length)
				tArray = dFil.iTempBuf
			End If
#If 0
			If Not IsNothing(dFil.iHosei)
				For i = 0 To dt.Rows.Count - 1 Step 1
				'行ループ
					x = dt.Rows(i).Item(0)
					'列ループ
					y = If( IsDBNull(dt.Rows(i).Item(1)), 0, dt.Rows(i).Item(1))
					strColumnName = dt.Columns(1).ColumnName						'測定中系列名を取得
					If Not (x = 0 Or y = 0)
						.Series(strColumnName).Points.AddXY(x, y)					'データを設定
					End If
				Next i
			Else
				.Series(strColumnName).Points.AddXY(-1, -1)
			End If
			Continue For
#end if

		End Select
		'履歴アイテムの描画
		If Not IsNothing(tArray)
			strColumnName = dt.Columns(j+1).ColumnName						'系列名を取得
			For i = 0 To tArray.GetUpperBound(0) Step RealTimeThinning(iThinTyp)
				x = tArray(i) / 10.0
				HistYSetForTemp(dFil, j, i, sSelectGItem, y)
				If Not (x = 0 Or y = 0)
					.Series(strColumnName).Points.AddXY(x, y)				'データを設定
				End If
			Next i
			tArray = Nothing
		Else
			'系列データない場合
			.Series(strColumnName).Points.AddXY(-1, -1)
		End If
	Next

	End With
Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub
Private Sub HistYSetForTemp(ByVal dFil As DataFile, ByVal iHisIdx As Int16, ByVal iRow As Long, ByVal sSelectGItem As String, ByRef dVal As Double, Optional ByVal iMesType As int16 = 0)
	Select Case iHisIdx
	Case 7
		Select Case sSelectGItem
		Case "rbXStress"
			dVal = dFil.iOxBuf_s2(iRow)
		Case "rbYStress"
			dVal = dFil.iOyBuf_s2(iRow)
		Case "rbZStress"
			dVal = dFil.dOzBuf_s2(iRow) * (cGraviAcc / 1000.0)
		Case "rbZPosition"
			dVal = dFil.iPzBuf_s2(iRow)
		Case "rbCorrect"
			dVal = dFil.iHosei_s2(iRow)
		Case "rbHPTemp"
			dVal = dFil.iTempBuf_s2(iRow) / 10
		Case "rbHPMV"
			dVal = dFil.iMvBuf_s2(iRow) / 10
		Case "rbPBTemp"
			dVal = dFil.iTempBuf2_s2(iRow) / 10
		Case "rbPBMV"
			dVal = dFil.iMvBuf2_s2(iRow) / 10
		Case "rbViscosity"
			dVal = dFil.dViscosity_s2(iRow)
		Case "rbPosPhase"
			dVal = dFil.dXPosPhase_s2(iRow) * RadToDegRate
		Case "rbSampleStrain"
			dVal = dFil.dSplStrain_s2(iRow)
		Case "rbShearMod"
			dVal = dFil.dShearMod_s2(iRow)
		Case "rbShearVelo"
			dVal = dFil.dShearVelo_s2(iRow)
		Case "rbStorageMod"
			dVal = dFil.dXStrgMod_s2(iRow)
		Case "rbElasticMod"
			dVal = dFil.dXElasMod_s2(iRow)
		Case "rbLossTangent"
			dVal = dFil.dXLossTan_s2(iRow)
		Case "rbCorrectP"
			dVal = dFil.dHoseiP_s2(iRow)
		End Select
	Case 6
		Select Case sSelectGItem
		Case "rbXStress"
			dVal = dFil.iOxBuf_s1(iRow)
		Case "rbYStress"
			dVal = dFil.iOyBuf_s1(iRow)
		Case "rbZStress"
			dVal = dFil.dOzBuf_s1(iRow) * (cGraviAcc / 1000.0)
		Case "rbZPosition"
			dVal = dFil.iPzBuf_s1(iRow)
		Case "rbCorrect"
			dVal = dFil.iHosei_s1(iRow)
		Case "rbHPTemp"
			dVal = dFil.iTempBuf_s1(iRow) / 10
		Case "rbHPMV"
			dVal = dFil.iMvBuf_s1(iRow) / 10
		Case "rbPBTemp"
			dVal = dFil.iTempBuf2_s1(iRow) / 10
		Case "rbPBMV"
			dVal = dFil.iMvBuf2_s1(iRow) / 10
		Case "rbViscosity"
			dVal = dFil.dViscosity_s1(iRow)
		Case "rbPosPhase"
			dVal = dFil.dXPosPhase_s1(iRow) * RadToDegRate
		Case "rbSampleStrain"
			dVal = dFil.dSplStrain_s1(iRow)
		Case "rbShearMod"
			dVal = dFil.dShearMod_s1(iRow)
		Case "rbShearVelo"
			dVal = dFil.dShearVelo_s1(iRow)
		Case "rbStorageMod"
			dVal = dFil.dXStrgMod_s1(iRow)
		Case "rbElasticMod"
			dVal = dFil.dXElasMod_s1(iRow)
		Case "rbLossTangent"
			dVal = dFil.dXLossTan_s1(iRow)
		Case "rbCorrectP"
			dVal = dFil.dHoseiP_s1(iRow)
		End Select
	Case 5
		Select Case sSelectGItem
		Case "rbXStress"
			dVal = dFil.iOxBuf_h5(iRow)
		Case "rbYStress"
			dVal = dFil.iOyBuf_h5(iRow)
		Case "rbZStress"
			dVal = dFil.dOzBuf_h5(iRow) * (cGraviAcc / 1000.0)
		Case "rbZPosition"
			dVal = dFil.iPzBuf_h5(iRow)
		Case "rbCorrect"
			dVal = dFil.iHosei_h5(iRow)
		Case "rbHPTemp"
			dVal = dFil.iTempBuf_h5(iRow) / 10
		Case "rbHPMV"
			dVal = dFil.iMvBuf_h5(iRow) / 10
		Case "rbPBTemp"
			dVal = dFil.iTempBuf2_h5(iRow) / 10
		Case "rbPBMV"
			dVal = dFil.iMvBuf2_h5(iRow) / 10
		Case "rbViscosity"
			dVal = dFil.dViscosity_h5(iRow)
		Case "rbPosPhase"
			dVal = dFil.dXPosPhase_h5(iRow) * RadToDegRate
		Case "rbSampleStrain"
			dVal = dFil.dSplStrain_h5(iRow)
		Case "rbShearMod"
			dVal = dFil.dShearMod_h5(iRow)
		Case "rbShearVelo"
			dVal = dFil.dShearVelo_h5(iRow)
		Case "rbStorageMod"
			dVal = dFil.dXStrgMod_h5(iRow)
		Case "rbElasticMod"
			dVal = dFil.dXElasMod_h5(iRow)
		Case "rbLossTangent"
			dVal = dFil.dXLossTan_h5(iRow)
		Case "rbCorrectP"
			dVal = dFil.dHoseiP_h5(iRow)
		End Select
	Case 4
		Select Case sSelectGItem
		Case "rbXStress"
			dVal = dFil.iOxBuf_h4(iRow)
		Case "rbYStress"
			dVal = dFil.iOyBuf_h4(iRow)
		Case "rbZStress"
			dVal = dFil.dOzBuf_h4(iRow) * (cGraviAcc / 1000.0)
		Case "rbZPosition"
			dVal = dFil.iPzBuf_h4(iRow)
		Case "rbCorrect"
			dVal = dFil.iHosei_h4(iRow)
		Case "rbHPTemp"
			dVal = dFil.iTempBuf_h4(iRow) / 10
		Case "rbHPMV"
			dVal = dFil.iMvBuf_h4(iRow) / 10
		Case "rbPBTemp"
			dVal = dFil.iTempBuf2_h4(iRow) / 10
		Case "rbPBMV"
			dVal = dFil.iMvBuf2_h4(iRow) / 10
		Case "rbViscosity"
			dVal = dFil.dViscosity_h4(iRow)
		Case "rbPosPhase"
			dVal = dFil.dXPosPhase_h4(iRow) * RadToDegRate
		Case "rbSampleStrain"
			dVal = dFil.dSplStrain_h4(iRow)
		Case "rbShearMod"
			dVal = dFil.dShearMod_h4(iRow)
		Case "rbShearVelo"
			dVal = dFil.dShearVelo_h4(iRow)
		Case "rbStorageMod"
			dVal = dFil.dXStrgMod_h4(iRow)
		Case "rbElasticMod"
			dVal = dFil.dXElasMod_h4(iRow)
		Case "rbLossTangent"
			dVal = dFil.dXLossTan_h4(iRow)
		Case "rbCorrectP"
			dVal = dFil.dHoseiP_h4(iRow)
		End Select
	Case 3
		Select Case sSelectGItem
		Case "rbXStress"
			dVal = dFil.iOxBuf_h3(iRow)
		Case "rbYStress"
			dVal = dFil.iOyBuf_h3(iRow)
		Case "rbZStress"
			dVal = dFil.dOzBuf_h3(iRow) * (cGraviAcc / 1000.0)
		Case "rbZPosition"
			dVal = dFil.iPzBuf_h3(iRow)
		Case "rbCorrect"
			dVal = dFil.iHosei_h3(iRow)
		Case "rbHPTemp"
			dVal = dFil.iTempBuf_h3(iRow) / 10
		Case "rbHPMV"
			dVal = dFil.iMvBuf_h3(iRow) / 10
		Case "rbPBTemp"
			dVal = dFil.iTempBuf2_h3(iRow) / 10
		Case "rbPBMV"
			dVal = dFil.iMvBuf2_h3(iRow) / 10
		Case "rbViscosity"
			dVal = dFil.dViscosity_h3(iRow)
		Case "rbPosPhase"
			dVal = dFil.dXPosPhase_h3(iRow) * RadToDegRate
		Case "rbSampleStrain"
			dVal = dFil.dSplStrain_h3(iRow)
		Case "rbShearMod"
			dVal = dFil.dShearMod_h3(iRow)
		Case "rbShearVelo"
			dVal = dFil.dShearVelo_h3(iRow)
		Case "rbStorageMod"
			dVal = dFil.dXStrgMod_h3(iRow)
		Case "rbElasticMod"
			dVal = dFil.dXElasMod_h3(iRow)
		Case "rbLossTangent"
			dVal = dFil.dXLossTan_h3(iRow)
		Case "rbCorrectP"
			dVal = dFil.dHoseiP_h3(iRow)
		End Select
	Case 2
		Select Case sSelectGItem
		Case "rbXStress"
			dVal = dFil.iOxBuf_h2(iRow)
		Case "rbYStress"
			dVal = dFil.iOyBuf_h2(iRow)
		Case "rbZStress"
			dVal = dFil.dOzBuf_h2(iRow) * (cGraviAcc / 1000.0)
		Case "rbZPosition"
			dVal = dFil.iPzBuf_h2(iRow)
		Case "rbCorrect"
			dVal = dFil.iHosei_h2(iRow)
		Case "rbHPTemp"
			dVal = dFil.iTempBuf_h2(iRow) / 10
		Case "rbHPMV"
			dVal = dFil.iMvBuf_h2(iRow) / 10
		Case "rbPBTemp"
			dVal = dFil.iTempBuf2_h2(iRow) / 10
		Case "rbPBMV"
			dVal = dFil.iMvBuf2_h2(iRow) / 10
		Case "rbViscosity"
			dVal = dFil.dViscosity_h2(iRow)
		Case "rbPosPhase"
			dVal = dFil.dXPosPhase_h2(iRow) * RadToDegRate
		Case "rbSampleStrain"
			dVal = dFil.dSplStrain_h2(iRow)
		Case "rbShearMod"
			dVal = dFil.dShearMod_h2(iRow)
		Case "rbShearVelo"
			dVal = dFil.dShearVelo_h2(iRow)
		Case "rbStorageMod"
			dVal = dFil.dXStrgMod_h2(iRow)
		Case "rbElasticMod"
			dVal = dFil.dXElasMod_h2(iRow)
		Case "rbLossTangent"
			dVal = dFil.dXLossTan_h2(iRow)
		Case "rbCorrectP"
			dVal = dFil.dHoseiP_h2(iRow)
		End Select
	Case 1
		Select Case sSelectGItem
		Case "rbXStress"
			dVal = dFil.iOxBuf_h1(iRow)
		Case "rbYStress"
			dVal = dFil.iOyBuf_h1(iRow)
		Case "rbZStress"
			dVal = dFil.dOzBuf_h1(iRow) * (cGraviAcc / 1000.0)
		Case "rbZPosition"
			dVal = dFil.iPzBuf_h1(iRow)
		Case "rbCorrect"
			dVal = dFil.iHosei_h1(iRow)
		Case "rbHPTemp"
			dVal = dFil.iTempBuf_h1(iRow) / 10
		Case "rbHPMV"
			dVal = dFil.iMvBuf_h1(iRow) / 10
		Case "rbPBTemp"
			dVal = dFil.iTempBuf2_h1(iRow) / 10
		Case "rbPBMV"
			dVal = dFil.iMvBuf2_h1(iRow) / 10
		Case "rbViscosity"
			dVal = dFil.dViscosity_h1(iRow)
		Case "rbPosPhase"
			dVal = dFil.dXPosPhase_h1(iRow) * RadToDegRate
		Case "rbSampleStrain"
			dVal = dFil.dSplStrain_h1(iRow)
		Case "rbShearMod"
			dVal = dFil.dShearMod_h1(iRow)
		Case "rbShearVelo"
			dVal = dFil.dShearVelo_h1(iRow)
		Case "rbStorageMod"
			dVal = dFil.dXStrgMod_h1(iRow)
		Case "rbElasticMod"
			dVal = dFil.dXElasMod_h1(iRow)
		Case "rbLossTangent"
			dVal = dFil.dXLossTan_h1(iRow)
		Case "rbCorrectP"
			dVal = dFil.dHoseiP_h1(iRow)
		End Select
	Case 0
		Select Case iMesType
		Case 0
			Select Case sSelectGItem
			Case "rbXStress"
				dVal = dFil.iOxBuf(iRow)
			Case "rbYStress"
				dVal = dFil.iOyBuf(iRow)
			Case "rbZStress"
				dVal = dFil.dOzBuf(iRow) * (cGraviAcc / 1000.0)
			Case "rbZPosition"
				dVal = dFil.iPzBuf(iRow)
			Case "rbCorrect"
				dVal = dFil.iHosei(iRow)
			Case "rbHPTemp"
				dVal = dFil.iTempBuf(iRow) / 10.0
			Case "rbHPMV"
				dVal = dFil.iMvBuf(iRow) / 10
			Case "rbPBTemp"
				dVal = dFil.iTempBuf2(iRow) / 10.0
			Case "rbPBMV"
				dVal = dFil.iMvBuf2(iRow) / 10
			Case "rbViscosity"
				dVal = dFil.dViscosity(iRow)
			Case "rbPosPhase"
				dVal = dFil.dXPosPhase(iRow) * RadToDegRate
			Case "rbSampleStrain"
				dVal = dFil.dSplStrain(iRow)
			Case "rbShearMod"
				dVal = dFil.dShearMod(iRow)
			Case "rbShearVelo"
				dVal = dFil.dShearVelo(iRow)
			Case "rbStorageMod"
				dVal = dFil.dXStrgMod(iRow)
			Case "rbElasticMod"
				dVal = dFil.dXElasMod(iRow)
			Case "rbLossTangent"
				dVal = dFil.dXLossTan(iRow)
			Case "rbCorrectP"
				dVal = dFil.dHosei100(iRow)
			End Select
		Case 1
			Select Case sSelectGItem
			Case "rbCorrect"
				dVal = dFil.iLT_Hosei(iRow)
			Case "rbHPTemp"
				dVal = dFil.iLT_TempBuf(iRow) / 10.0
			Case "rbHPMV"
				dVal = dFil.iLT_MvBuf(iRow) / 10
			Case "rbPBTemp"
				dVal = dFil.iLT_TempBuf2(iRow) / 10.0
			Case "rbPBMV"
				dVal = dFil.iLT_MvBuf2(iRow) / 10
			Case "rbViscosity"
				dVal = dFil.dLT_Viscosity(iRow)
			Case "rbPosPhase"
				dVal = dFil.dLT_XPosPhase(iRow) * RadToDegRate
			Case "rbSampleStrain"
				dVal = dFil.dLT_SplStrain(iRow)
			Case "rbShearMod"
				dVal = dFil.dLT_ShearMod(iRow)
			Case "rbShearVelo"
				dVal = dFil.dLT_ShearVelo(iRow)
			Case "rbStorageMod"
				dVal = dFil.dLT_XStrgMod(iRow)
			Case "rbElasticMod"
				dVal = dFil.dLT_XElasMod(iRow)
			Case "rbLossTangent"
				dVal = dFil.dLT_XLossTan(iRow)
			Case "rbCorrectP"
				dVal = dFil.dLT_Hosei100(iRow)
			End Select
		End Select
	End Select
End Sub

#end if
''' <summary>
''' 選択された系列のデータテーブルを定義する
''' </summary>
''' <param name="iType"> 0:時間 1:温度 </param>
''' <param name="ds"></param>
''' <param name="dt"></param>
''' <param name="sSelectGItem"></param>
''' <param name="iHis"></param>
Private Sub	RealTimeChartDataTableDefine(ByVal iType As Int16, ByRef ds As DataSet, ByRef dt As DataTable, ByVal sSelectGItem As String, ByVal iHis As Int16, ByVal iStd As Int16, ByVal sCalcMethod As String)
Dim iStdBase As Int32 = cHistMax+1
Try
	ds.Tables.Clear()
	dt.Columns.Clear()

    dt.Columns.Add(If(iType=0, "Time", "Temp"))
	Select case sSelectGItem
	Case "rbXStress"
        dt.Columns.Add(sChartItemStr(eChartItem.OxBuf))
        If iHis<>0
            For i=1 To iHis
                dt.Columns.Add(sChartItemStr(eChartItem.OxBuf) & i.ToString)
            Next
        End if
        If iStd<>0
            For i = iStdBase To iStdBase + iStd - 1
                dt.Columns.Add(sChartItemStr(eChartItem.OxBuf) & i.ToString)
            Next
        End if
	Case "rbYStress"
        dt.Columns.Add(sChartItemStr(eChartItem.OyBuf))
        If iHis<>0
            For i=1 To iHis
                dt.Columns.Add(sChartItemStr(eChartItem.OyBuf) & i.ToString)
            Next
        End if
        If iStd<>0
            For i = iStdBase To iStdBase + iStd - 1
                dt.Columns.Add(sChartItemStr(eChartItem.OyBuf) & i.ToString)
            Next
        End if
	Case "rbZStress"
        dt.Columns.Add(sChartItemStr(eChartItem.OzBuf))
        If iHis<>0
            For i=1 To iHis
                dt.Columns.Add(sChartItemStr(eChartItem.OzBuf) & i.ToString)
            Next
        End if
        If iStd<>0
            For i = iStdBase To iStdBase + iStd - 1
                dt.Columns.Add(sChartItemStr(eChartItem.OzBuf) & i.ToString)
            Next
        End if
	Case "rbZPosition"
        dt.Columns.Add(sChartItemStr(eChartItem.OzPos))
        If iHis<>0
            For i=1 To iHis
                dt.Columns.Add(sChartItemStr(eChartItem.OzPos) & i.ToString)
            Next
        End if
        If iStd<>0
            For i = iStdBase To iStdBase + iStd - 1
                dt.Columns.Add(sChartItemStr(eChartItem.OzPos) & i.ToString)
            Next
        End if
	Case "rbCorrect"
        dt.Columns.Add(sChartItemStr(eChartItem.Hosei))
        If iHis<>0
            For i=1 To iHis
				dt.Columns.Add(sChartItemStr(eChartItem.Hosei) & i.ToString)
            Next
        End if
        If iStd<>0
            For i = iStdBase To iStdBase + iStd - 1
                dt.Columns.Add(sChartItemStr(eChartItem.Hosei) & i.ToString)
            Next
        End if
        '補助グラフSLOPE
        Select Case sCalcMethod
        Case lstCalcMethod(eCalcMethod.SLOPE)
            dt.Columns.Add(lstCalcMethod(eCalcMethod.SLOPE))
        End Select
	Case "rbHPTemp"
        dt.Columns.Add(sChartItemStr(eChartItem.TempBuf))
        If iHis<>0
            For i=1 To iHis
                dt.Columns.Add(sChartItemStr(eChartItem.TempBuf) & i.ToString)
            Next
        End If
        If iStd<>0
            For i = iStdBase To iStdBase + iStd - 1
                dt.Columns.Add(sChartItemStr(eChartItem.TempBuf) & i.ToString)
            Next
        End if
	Case "rbHPMV"
        dt.Columns.Add(sChartItemStr(eChartItem.MvBuf))
        If iHis<>0
            For i=1 To iHis
                dt.Columns.Add(sChartItemStr(eChartItem.MvBuf) & i.ToString)
            Next
        End If
        If iStd<>0
            For i = iStdBase To iStdBase + iStd - 1
                dt.Columns.Add(sChartItemStr(eChartItem.MvBuf) & i.ToString)
            Next
        End if
	Case "rbPBTemp"
        dt.Columns.Add(sChartItemStr(eChartItem.TempBuf2))
        If iHis<>0
            For i=1 To iHis
                dt.Columns.Add(sChartItemStr(eChartItem.TempBuf2) & i.ToString)
            Next
        End If
        If iStd<>0
            For i = iStdBase To iStdBase + iStd - 1
                dt.Columns.Add(sChartItemStr(eChartItem.TempBuf2) & i.ToString)
            Next
        End if
	Case "rbPBMV"
        dt.Columns.Add(sChartItemStr(eChartItem.MvBuf2))
        If iHis<>0
            For i=1 To iHis
                dt.Columns.Add(sChartItemStr(eChartItem.MvBuf2) & i.ToString)
            Next
        End If
        If iStd<>0
            For i = iStdBase To iStdBase + iStd - 1
                dt.Columns.Add(sChartItemStr(eChartItem.MvBuf2) & i.ToString)
            Next
        End if
	Case "rbViscosity"
        dt.Columns.Add(sChartItemStr(eChartItem.Viscosity))
        If iHis<>0
            For i=1 To iHis
                dt.Columns.Add(sChartItemStr(eChartItem.Viscosity) & i.ToString)
            Next
        End If
        If iStd<>0
            For i = iStdBase To iStdBase + iStd - 1
                dt.Columns.Add(sChartItemStr(eChartItem.Viscosity) & i.ToString)
            Next
        End if
	Case "rbPosPhase"
        dt.Columns.Add(sChartItemStr(eChartItem.XPosPhase))
        If iHis<>0
            For i=1 To iHis
                dt.Columns.Add(sChartItemStr(eChartItem.XPosPhase) & i.ToString)
            Next
        End If
        If iStd<>0
            For i = iStdBase To iStdBase + iStd - 1
                dt.Columns.Add(sChartItemStr(eChartItem.XPosPhase) & i.ToString)
            Next
        End if
	Case "rbSampleStrain"
        dt.Columns.Add(sChartItemStr(eChartItem.SplStrain))
        If iHis<>0
            For i=1 To iHis
                dt.Columns.Add(sChartItemStr(eChartItem.SplStrain) & i.ToString)
            Next
        End If
        If iStd<>0
            For i = iStdBase To iStdBase + iStd - 1
                dt.Columns.Add(sChartItemStr(eChartItem.SplStrain) & i.ToString)
            Next
        End if
	Case "rbShearMod"
        dt.Columns.Add(sChartItemStr(eChartItem.ShearMod))
        If iHis<>0
            For i=1 To iHis
                dt.Columns.Add(sChartItemStr(eChartItem.ShearMod) & i.ToString)
            Next
        End If
        If iStd<>0
            For i = iStdBase To iStdBase + iStd - 1
                dt.Columns.Add(sChartItemStr(eChartItem.ShearMod) & i.ToString)
            Next
        End if
	Case "rbShearVelo"
        dt.Columns.Add(sChartItemStr(eChartItem.ShearVelo))
        If iHis<>0
            For i=1 To iHis
                dt.Columns.Add(sChartItemStr(eChartItem.ShearVelo) & i.ToString)
            Next
        End If
        If iStd<>0
            For i = iStdBase To iStdBase + iStd - 1
                dt.Columns.Add(sChartItemStr(eChartItem.ShearVelo) & i.ToString)
            Next
        End if
	Case "rbStorageMod"
        dt.Columns.Add(sChartItemStr(eChartItem.XStrgMod))
        If iHis<>0
            For i=1 To iHis
                dt.Columns.Add(sChartItemStr(eChartItem.XStrgMod) & i.ToString)
            Next
        End If
        If iStd<>0
            For i = iStdBase To iStdBase + iStd - 1
                dt.Columns.Add(sChartItemStr(eChartItem.XStrgMod) & i.ToString)
            Next
        End if
	Case "rbElasticMod"
        dt.Columns.Add(sChartItemStr(eChartItem.XElasMod))
        If iHis<>0
            For i=1 To iHis
                dt.Columns.Add(sChartItemStr(eChartItem.XElasMod) & i.ToString)
            Next
        End If
        If iStd<>0
            For i = iStdBase To iStdBase + iStd - 1
                dt.Columns.Add(sChartItemStr(eChartItem.XElasMod) & i.ToString)
            Next
        End if
	Case "rbLossTangent"
        dt.Columns.Add(sChartItemStr(eChartItem.XLossTan))
        If iHis<>0
            For i=1 To iHis
                dt.Columns.Add(sChartItemStr(eChartItem.XLossTan) & i.ToString)
            Next
        End If
        If iStd<>0
            For i = iStdBase To iStdBase + iStd - 1
                dt.Columns.Add(sChartItemStr(eChartItem.XLossTan) & i.ToString)
            Next
        End if
	Case "rbCorrectP"
        dt.Columns.Add(sChartItemStr(eChartItem.HoseiP))
        If iHis<>0
            For i=1 To iHis
                dt.Columns.Add(sChartItemStr(eChartItem.HoseiP) & i.ToString)
            Next
        End If
        If iStd<>0
            For i = iStdBase To iStdBase + iStd - 1
                dt.Columns.Add(sChartItemStr(eChartItem.HoseiP) & i.ToString)
            Next
        End if
    Case Else
	End Select
    ds.Tables.Add(dt)
	Exit Sub
    '----------------------------------------------------------------------------------------------------
Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
    Console.Write("error mes:" & ex.Message.ToString)
End Try
End Sub

''' <summary>
''' データテーブルへのデータ追加
''' </summary>
''' <param name="arg"></param>
'''		"sSelectGItem"
'''		"dLimitTime"
'''		"sCalcMethod"
'''		"dFullStressADVal"
''' <param name="dFil"></param>
''' <param name="cts"></param>
''' <param name="dt"></param>
''' <param name="iHis"></param>
''' <param name="iStd"></param>
''' <param name="iSampleNumOnGraph"></param>
''' <param name="iThinTyp"></param>v
Public Sub RealTimeDataTableUpdate( _
	ByRef arg		As ChartArg, _
	ByRef dFil		As DataFile, _
	ByRef cts		As CToolSettings, _
	ByRef dt		As DataTable, _
	ByVal iHis              As Int16, _
	ByVal iStd              As Int16, _
	ByVal iSampleNumOnGraph	As Int64, _
	ByVal iThinTyp			As Int16
	)

Dim i			As int32
Dim decTime		As Double
Dim dData		As Double
Dim rw			As DataRow = Nothing
Dim dTemp()		As Double = Nothing
Dim dMv()		As Double = Nothing
Dim dTemp2()	As Double = Nothing
Dim dMv2()		As Double = Nothing
Dim Ary()		As Int64 = Nothing
Dim Ary2()		As Int64 = Nothing
Dim dAry()		As Double = Nothing
Dim dAry2()		As Double = Nothing
Dim sIdx		As Int16 = 0
Dim iLimit		As Int64

Try
	Array.Resize(dTemp,		iSampleNumOnGraph)
	Array.Resize(dTemp2,	iSampleNumOnGraph)
	Array.Resize(dMv,		iSampleNumOnGraph)
	Array.Resize(dMv2,		iSampleNumOnGraph)

	
	Dim iStep As Int32 = RealTimeThinning(iThinTyp)
'    For i = p_iScrlStart To iSampleNumOnGraph   Step iStep
    For i = p_iScrlStart + iStep To iSampleNumOnGraph Step iStep
        'データテーブルに必要データを加える
        rw = dt.NewRow
		Select Case cts.iChartType
		Case 0
		'時間
		    decTime	= CType(i * MEAS_TIME_RESOLUTION, Double) / 1000.0										'秒換算
	        rw("Time") = decTime
		Case 1
		'温度
			If Not IsNothing(dFil.iTempBuf)
				If i<=dFil.iTempBuf.GetUpperBound(0)
					decTime	= CType(dFil.iTempBuf(i), Double) / 10.0					'温度換算
				Else
					decTime	= vbNull
				End If
			Else
				decTime	= CType(i * 0.01, Double) / 10.0
			End If
			rw("Temp") = decTime
		End Select
        Ary		= Nothing
        Ary2	= Nothing
        sIdx    = 0
        Select case arg.sSelectGItem
        Case "rbXStress"
            If isNothing(dFil.iOxBuf)
                Array.Resize(Ary,iSampleNumOnGraph)
            Else
                Ary = dFil.iOxBuf
            End If
            sIdx = eChartItem.OxBuf
        Case "rbYStress"
            If isNothing(dFil.iOyBuf)
                Array.Resize(Ary,iSampleNumOnGraph)
            Else
                Ary = dFil.iOyBuf
            End If
            sIdx = eChartItem.OyBuf
        Case "rbZStress"
            If isNothing(dFil.dOzBuf)
                Array.Resize(dAry,iSampleNumOnGraph)
            Else
                dAry = dFil.dOzBuf
            End If
            sIdx = eChartItem.OzBuf
        Case "rbZPosition"
            If isNothing(dFil.iPzBuf)
                Array.Resize(Ary,iSampleNumOnGraph)
            Else
                Ary = dFil.iPzBuf
            End If
            sIdx = eChartItem.OzPos
        Case "rbCorrect"
            If isNothing(dFil.iHosei)
                Array.Resize(dAry,iSampleNumOnGraph)
                Array.Resize(dAry2,iSampleNumOnGraph)
            Else
                dAry = dFil.iHosei
                dAry2 = dFil.iSlope
            End If
            sIdx = eChartItem.Hosei
        Case "rbHPTemp"
            If isNothing(dFil.iTempBuf)
                Array.Resize(Ary,iSampleNumOnGraph)
            Else
                Ary = dFil.iTempBuf
            End If
            sIdx = eChartItem.TempBuf
        Case "rbHPMV"
            If isNothing(dFil.iMvBuf)
                Array.Resize(Ary,iSampleNumOnGraph)
            Else
                Ary = dFil.iMvBuf
            End If
            sIdx = eChartItem.MvBuf
        Case "rbPBTemp"
            If isNothing(dFil.iTempBuf2)
                Array.Resize(Ary,iSampleNumOnGraph)
            Else
                Ary = dFil.iTempBuf2
            End If
            sIdx = eChartItem.TempBuf2
        Case "rbPBMV"
            If isNothing(dFil.iMvBuf2)
                Array.Resize(Ary,iSampleNumOnGraph)
            Else
                Ary = dFil.iMvBuf2
            End If
            sIdx = eChartItem.MvBuf2
        Case "rbViscosity"
            If isNothing(dFil.dViscosity)
                Array.Resize(dAry,iSampleNumOnGraph)
            Else
                dAry = dFil.dViscosity
            End If
            sIdx = eChartItem.Viscosity
		Case "rbPosPhase"
            If isNothing(dFil.dXPosPhase)
                Array.Resize(dAry,iSampleNumOnGraph)
            Else
                dAry = dFil.dXPosPhase
            End If
            sIdx = eChartItem.XPosPhase
		Case "rbSampleStrain"
            If isNothing(dFil.dSplStrain)
                Array.Resize(dAry,iSampleNumOnGraph)
            Else
                dAry = dFil.dSplStrain
            End If
            sIdx = eChartItem.SplStrain
		Case "rbShearMod"
            If isNothing(dFil.dShearMod)
                Array.Resize(dAry,iSampleNumOnGraph)
            Else
                dAry = dFil.dShearMod
            End If
            sIdx = eChartItem.ShearMod
		Case "rbShearVelo"
            If isNothing(dFil.dShearVelo)
                Array.Resize(dAry,iSampleNumOnGraph)
            Else
                dAry = dFil.dShearVelo
            End If
            sIdx = eChartItem.ShearVelo
		Case "rbStorageMod"
            If isNothing(dFil.dXStrgMod)
                Array.Resize(dAry,iSampleNumOnGraph)
            Else
                dAry = dFil.dXStrgMod
            End If
            sIdx = eChartItem.XStrgMod
		Case "rbElasticMod"
            If isNothing(dFil.dXElasMod)
                Array.Resize(dAry,iSampleNumOnGraph)
            Else
                dAry = dFil.dXElasMod
            End If
            sIdx = eChartItem.XElasMod
		Case "rbLossTangent"
            If isNothing(dFil.dXLossTan)
                Array.Resize(dAry,iSampleNumOnGraph)
            Else
                dAry = dFil.dXLossTan
            End If
            sIdx = eChartItem.XLossTan
		Case "rbCorrectP"
            If isNothing(dFil.dHosei100)
                Array.Resize(dAry,iSampleNumOnGraph)
            Else
                dAry = dFil.dHosei100
            End If
            sIdx = eChartItem.HoseiP
		Case Else
        End Select
		'X軸制限値を取得
		Select Case arg.sSelectGItem
        Case "rbViscosity", "rbPosPhase", "rbSampleStrain", "rbShearMod", "rbShearVelo", "rbStorageMod", "rbElasticMod", "rbLossTangent","rbCorrect","rbCorrectP","rbZStress"
			if dAry.Length > iSampleNumOnGraph
				iLimit = iSampleNumOnGraph - 1
			Else
				iLimit = dAry.GetUpperBound(0)
			End If
		Case Else
			if Ary.Length > iSampleNumOnGraph
				iLimit = iSampleNumOnGraph - 1
			Else
				iLimit = Ary.GetUpperBound(0)
			End If
		End Select
        If i <= iLimit
            Select Case arg.sSelectGItem
            Case "rbHPTemp"
                dData	= CType(Ary(i) / 10, Double)
                dTemp(i) = dData
            Case "rbHPMV"
                dData	= CType(Ary(i) / 10, Double)
                dMv(i) = dData
            Case "rbPBTemp"
                dData	= CType(Ary(i) / 10, Double)
                dTemp2(i) = dData
            Case "rbPBMV"
                dData	= CType(Ary(i) / 10, Double)
                dMv2(i) = dData
            Case "rbViscosity", "rbSampleStrain", "rbShearMod", "rbShearVelo", "rbStorageMod", "rbElasticMod", "rbLossTangent"
                dData	= dAry(i)
				If dData = 0
					rw(sChartItemStr(sIdx)) = Double.NaN
				ElseIf dData < 0
'Debug.Print("")
'If  Double.IsNaN(dData)
'Debug.Print("")
'End If
				Else
					rw(sChartItemStr(sIdx)) = dData
				End If
            Case "rbZStress"		'ｇ->N変換
                dData	= dAry(i)
				If dData = 0
					rw(sChartItemStr(sIdx)) = Double.NaN
				Else
					dData = dData * (cGraviAcc / 1000.0)
					rw(sChartItemStr(sIdx)) = dData
				End If
            Case "rbPosPhase"		'Rad->Deg変換
                dData	= dAry(i) * RadToDegRate
				If dData = 0
					rw(sChartItemStr(sIdx)) = Double.NaN
				Else
					rw(sChartItemStr(sIdx)) = dData
				End If
            Case "rbCorrectP"
                dData	= dAry(i)
				If dData = 0
					rw(sChartItemStr(sIdx)) = Double.NaN
				Else
					rw(sChartItemStr(sIdx)) = dData
				End If
                HistDataRwSet(dfil, iHis, iStd, cts, i, sIdx, rw, 100)
            Case "rbCorrect"
                dData	= CType(dAry(i), Double)
				If dData = 0
					rw(sChartItemStr(sIdx)) = Double.NaN
				Else
					rw(sChartItemStr(sIdx)) = dData
				End If
                HistDataRwSet(dfil, iHis, iStd, cts, i, sIdx, rw, arg.dFullStressADVal)

                '補助グラフデータ追加 第２軸
                Select Case arg.sCalcMethod
                Case lstCalcMethod(eCalcMethod.SLOPE)
                    dData	= CType(dAry2(i), Double)
				    If dData = 0
					    rw(lstCalcMethod(eCalcMethod.SLOPE)) = Double.NaN
				    Else
					    rw(lstCalcMethod(eCalcMethod.SLOPE)) = dData
				    End If
                End Select 
                dt.Rows.Add(rw)
                Continue For

            Case Else
                dData	= CType(Ary(i), Double)
            End Select
			If dData = 0
				rw(sChartItemStr(sIdx)) = Double.NaN
			Else
				rw(sChartItemStr(sIdx)) = dData
			End If
		Else
			rw(sChartItemStr(sIdx)) = Double.NaN
        End If
        HistDataRwSet(dfil, iHis, iStd, cts, i, sIdx, rw, arg.dFullStressADVal)
        dt.Rows.Add(rw)
    Next i
Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub
''' <summary>
''' 履歴データの行データをセット
''' </summary>
''' <param name="dFil">データファイルクラス参照</param>
''' <param name="iHis">履歴データカウント</param>
''' <param name="cts">オプション設定</param>
''' <param name="i">データインデックス</param>
''' <param name="iKind">系統種</param>
''' <param name="rw">行データ</param>
''' <param name="dFullStressADVal">フルAD値</param>
Private Sub HistDataRwSet( ByVal dFil As DataFile, _
                            ByVal iHis As Int16, _
                            ByVal iStd As Int16, _
                            ByRef cts As CToolSettings, _
                            ByVal i As Int32, _
                            ByVal iKind As int16, _
                            byRef rw As DataRow, _
                            ByVal dFullStressADVal As Double)
Dim j       As int32
Dim decX    As Decimal
Dim decY    As Decimal
Dim decZ    As Double
Dim decZP   As Decimal
Dim dCR     As Double
Dim decT    As Double
Dim decM    As Double
Dim Ary()   As int64	= Nothing
Dim dAry()  As Double	= Nothing
Dim iStdBase As Int32   = cHistMax+1

Try
    Select Case iKind
    Case eChartItem.OxBuf      'X Stress
		If iHis  <> 0
			For j = 1 To iHis
				Select Case j
				Case 1
					Ary = dFil.iOxBuf_h1
				Case 2
					Ary = dFil.iOxBuf_h2
				Case 3
					Ary = dFil.iOxBuf_h3
				Case 4
					Ary = dFil.iOxBuf_h4
				Case 5
					Ary = dFil.iOxBuf_h5
				End Select
				If i > Ary.GetUpperBound(0)
					decX	= 0
				Else
					decX	= CType(Ary(i), Decimal)
				End If
				rw(sChartItemStr(iKind) & j.ToString) = decX
			Next
		End If
		If iStd <> 0
			For j = iStdBase To iStdBase + iStd - 1
				Select Case j
				Case 6
					Ary = dFil.iOxBuf_S1
				Case 7
					Ary = dFil.iOxBuf_S2
				End Select
				If Not IsNothing(Ary)
					If i > Ary.GetUpperBound(0)
						decX	= 0
					Else
						decX	= CType(Ary(i), Decimal)
					End If
				Else
					decX	= 0
				End If
				rw(sChartItemStr(iKind) & j.ToString) = decX
			Next
		End If

    Case eChartItem.OyBuf      'Y Stress
		If iHis  <> 0
			For j = 1 To iHis
				Select Case j
				Case 1
					Ary = dFil.iOyBuf_h1
				Case 2
					Ary = dFil.iOyBuf_h2
				Case 3
					Ary = dFil.iOyBuf_h3
				Case 4
					Ary = dFil.iOyBuf_h4
				Case 5
					Ary = dFil.iOyBuf_h5
				End Select
				If i > Ary.GetUpperBound(0)
					decY	= 0
				Else
					decY	= CType(Ary(i), Decimal)
				End if
				rw(sChartItemStr(iKind) & j.ToString) = decY
			Next
		End If
		If iStd <> 0
			For j = iStdBase To iStdBase + iStd - 1
				Select Case j
				Case 6
					Ary = dFil.iOyBuf_S1
				Case 7
					Ary = dFil.iOyBuf_S2
				End Select
				If Not IsNothing(Ary)
					If i > Ary.GetUpperBound(0)
						decY	= 0
					Else
						decY	= CType(Ary(i), Decimal)
					End If
				Else
					decY	= 0
				End If
				rw(sChartItemStr(iKind) & j.ToString) = decY
			Next
		End If


    Case eChartItem.OzBuf      'Z Stress
		If iHis  <> 0
			For j = 1 To iHis
				Select Case j
				Case 1
					dAry = dFil.dOzBuf_h1
				Case 2
					dAry = dFil.dOzBuf_h2
				Case 3
					dAry = dFil.dOzBuf_h3
				Case 4
					dAry = dFil.dOzBuf_h4
				Case 5
					dAry = dFil.dOzBuf_h5
				End Select
				If i > dAry.GetUpperBound(0)
					decZ	= Double.NaN
				Else
					decZ	= CType(dAry(i)* (cGraviAcc / 1000.0), Double)
				End if
				rw(sChartItemStr(iKind) & j.ToString) = decZ
			Next
		End If
		If iStd <> 0
			For j = iStdBase To iStdBase + iStd - 1
				Select Case j
				Case 6
					dAry = dFil.dOzBuf_S1
				Case 7
					dAry = dFil.dOzBuf_S2
				End Select
				If Not IsNothing(dAry)
					If i > dAry.GetUpperBound(0)
						decZ	= Double.NaN
					Else
						decZ	= CType(dAry(i)* (cGraviAcc / 1000.0), Double)
					End If
				Else
					decZ	= Double.NaN
				End If
				rw(sChartItemStr(iKind) & j.ToString) = decZ
			Next
		End If


    Case eChartItem.OzPos      'Z Position
		If iHis  <> 0
			For j = 1 To iHis
				Select Case j
				Case 1
					Ary = dFil.iPzBuf_h1
				Case 2
					Ary = dFil.iPzBuf_h2
				Case 3
					Ary = dFil.iPzBuf_h3
				Case 4
					Ary = dFil.iPzBuf_h4
				Case 5
					Ary = dFil.iPzBuf_h5
				End Select
				If i > Ary.GetUpperBound(0)
					decZP	= 0
				Else
					decZp	= CType(Ary(i), Decimal)
				End if
				rw(sChartItemStr(iKind) & j.ToString) = decZp
			Next
		End If
		If iStd <> 0
			For j = iStdBase To iStdBase + iStd - 1
				Select Case j
				Case 6
					Ary = dFil.iPzBuf_S1
				Case 7
					Ary = dFil.iPzBuf_S2
				End Select
				If Not IsNothing(Ary)
					If i > Ary.GetUpperBound(0)
						decZp	= 0
					Else
						decZp	= CType(Ary(i), Decimal)
					End If
				Else
					decZp	= 0
				End If
				rw(sChartItemStr(iKind) & j.ToString) = decZP
			Next
		End If


    Case eChartItem.Hosei      'Correct
		If iHis  <> 0
			For j = 1 To iHis
			   Select Case j
				Case 1
					dAry = dFil.iHosei_h1
				Case 2
					dAry = dFil.iHosei_h2
				Case 3
					dAry = dFil.iHosei_h3
				Case 4
					dAry = dFil.iHosei_h4
				Case 5
					dAry = dFil.iHosei_h5
				End Select
				if IsNothing(dAry)
					dCR	= Double.NaN
				Else
					If i > dAry.GetUpperBound(0)
						dCR	= Double.NaN
					Else
						dCR	= CType(dAry(i), Double)
					End if
				End if
				rw(sChartItemStr(iKind) & j.ToString) = dCR
			Next
		End If
		If iStd <> 0
			For j = iStdBase To iStdBase + iStd - 1
				Select Case j
				Case 6
					dAry = dFil.iHosei_S1
				Case 7
					dAry = dFil.iHosei_S2
				End Select
				if IsNothing(dAry)
					dCR	= Double.NaN
				Else
					If i > dAry.GetUpperBound(0)
						dCR	= Double.NaN
					Else
						dCR	= CType(dAry(i), Double)
					End If
				End If
				rw(sChartItemStr(iKind) & j.ToString) = dCR
			Next
		End If
    Case eChartItem.HoseiP      'Correct100
		If iHis  <> 0
			For j = 1 To iHis
			   Select Case j
				Case 1
					dAry = dFil.dHoseiP_h1
				Case 2
					dAry = dFil.dHoseiP_h2
				Case 3
					dAry = dFil.dHoseiP_h3
				Case 4
					dAry = dFil.dHoseiP_h4
				Case 5
					dAry = dFil.dHoseiP_h5
				End Select
				if IsNothing(dAry)
					dCR	= Double.NaN
				Else
					If i > dAry.GetUpperBound(0)
						dCR	= Double.NaN
					Else
						dCR	= CType(dAry(i), Double)
					End if
				End if
				rw(sChartItemStr(iKind) & j.ToString) = dCR
			Next
		End If
		If iStd <> 0
			For j = iStdBase To iStdBase + iStd - 1
				Select Case j
				Case iStdBase
					dAry = dFil.dHoseiP_S1
				Case iStdBase+1
					dAry = dFil.dHoseiP_S2
				End Select
				if IsNothing(dAry)
					dCR	= Double.NaN
				Else
					If i > dAry.GetUpperBound(0)
						dCR	= Double.NaN
					Else
						dCR	= CType(dAry(i), Double)
					End If
				End If
				rw(sChartItemStr(iKind) & j.ToString) = dCR
			Next
		End If

    Case eChartItem.TempBuf      'HP Temp
		If iHis  <> 0
			For j = 1 To iHis
				Select Case j
				Case 1
					Ary = dFil.iTempBuf_h1
				Case 2
					Ary = dFil.iTempBuf_h2
				Case 3
					Ary = dFil.iTempBuf_h3
				Case 4
					Ary = dFil.iTempBuf_h4
				Case 5
					Ary = dFil.iTempBuf_h5
				End Select
				if IsNothing(Ary)
					decT	= Double.NaN
				Else
					If i > Ary.GetUpperBound(0)
						decT	= Double.NaN
					Else
						decT	= CType(Ary(i) / 10, Double)
					End if
				End if
				rw(sChartItemStr(iKind) & j.ToString) = decT
			Next
		End If
		If iStd <> 0
			For j = iStdBase To iStdBase + iStd - 1
				Select Case j
				Case 6
					Ary = dFil.iTempBuf_S1
				Case 7
					Ary = dFil.iTempBuf_S2
				End Select
				if IsNothing(Ary)
					decT	= Double.NaN
				Else
					If i > Ary.GetUpperBound(0)
						decT	= Double.NaN
					Else
						decT	= CType(Ary(i) / 10, Double)
					End If
				End If
				rw(sChartItemStr(iKind) & j.ToString) = decT
			Next
		End If
    Case eChartItem.MvBuf      'HP MV
		If iHis  <> 0
		   For j = 1 To iHis
				Select Case j
				Case 1
					Ary = dFil.iMvBuf_h1
				Case 2
					Ary = dFil.iMvBuf_h2
				Case 3
					Ary = dFil.iMvBuf_h3
				Case 4
					Ary = dFil.iMvBuf_h4
				Case 5
					Ary = dFil.iMvBuf_h5
				End Select
				 if IsNothing(Ary)
					decM	= Double.NaN
				Else
					If i > Ary.GetUpperBound(0)
						decM	= Double.NaN
					Else
						decM	= CType(Ary(i) / 10, Double)
					End if
				End if
				rw(sChartItemStr(iKind) & j.ToString) = decM
			Next
		End If
		If iStd <> 0
			For j = iStdBase To iStdBase + iStd - 1
				Select Case j
				Case 6
					Ary = dFil.iMvBuf_S1
				Case 7
					Ary = dFil.iMvBuf_S2
				End Select
				 if IsNothing(Ary)
					decM	= Double.NaN
				Else
					If i > Ary.GetUpperBound(0)
						decM	= Double.NaN
					Else
						decM	= CType(Ary(i) / 10, Double)
					End If
				End If
				rw(sChartItemStr(iKind) & j.ToString) = decM
			Next
		End If
   Case eChartItem.TempBuf2      'PB Temp
		If iHis  <> 0
			For j = 1 To iHis
				Select Case j
				Case 1
					Ary = dFil.iTempBuf2_h1
				Case 2
					Ary = dFil.iTempBuf2_h2
				Case 3
					Ary = dFil.iTempBuf2_h3
				Case 4
					Ary = dFil.iTempBuf2_h4
				Case 5
					Ary = dFil.iTempBuf2_h5
				End Select
				if IsNothing(Ary)
					decT	= Double.NaN
				Else
					If i > Ary.GetUpperBound(0)
						decT	= Double.NaN
					Else
						decT	= CType(Ary(i) / 10, Double)
					End if
				End if
				rw(sChartItemStr(iKind) & j.ToString) = decT
			Next
		End If
		If iStd <> 0
			For j = iStdBase To iStdBase + iStd - 1
				Select Case j
				Case 6
					Ary = dFil.iTempBuf2_S1
				Case 7
					Ary = dFil.iTempBuf2_S2
				End Select
				if IsNothing(Ary)
					decT	= Double.NaN
				Else
					If i > Ary.GetUpperBound(0)
						decT	= Double.NaN
					Else
						decT	= CType(Ary(i) / 10, Double)
					End If
				End If
				rw(sChartItemStr(iKind) & j.ToString) = decT
			Next
		End If
    Case eChartItem.MvBuf2      'PB MV
		If iHis <> 0
			For j = 1 To iHis
				Select Case j
				Case 1
					Ary = dFil.iMvBuf2_h1
				Case 2
					Ary = dFil.iMvBuf2_h2
				Case 3
					Ary = dFil.iMvBuf2_h3
				Case 4
					Ary = dFil.iMvBuf2_h4
				Case 5
					Ary = dFil.iMvBuf2_h5
				End Select
				if IsNothing(Ary)
					decM	= Double.NaN
				Else
					If i > Ary.GetUpperBound(0)
						decM	= Double.NaN
					Else
						decM	= CType(Ary(i) / 10, Double)
					End if
				End if
				rw(sChartItemStr(iKind) & j.ToString) = decM
			Next
		End If
		If iStd <> 0
			For j = iStdBase To iStdBase + iStd - 1
				Select Case j
				Case 6
					Ary = dFil.iMvBuf2_S1
				Case 7
					Ary = dFil.iMvBuf2_S2
				End Select
				if IsNothing(Ary)
					decM	= Double.NaN
				Else
					If i > Ary.GetUpperBound(0)
						decM	= Double.NaN
					Else
						decM	= CType(Ary(i) / 10, Double)
					End if
					If decM < 0.0
						decM = Double.Nan
					End If
				End if
				rw(sChartItemStr(iKind) & j.ToString) = decM
			Next
		End If
    Case eChartItem.Viscosity      'Viscosity
		If iHis <> 0
			For j = 1 To iHis
				Select Case j
				Case 1
					dAry = dFil.dViscosity_h1
				Case 2
					dAry = dFil.dViscosity_h2
				Case 3
					dAry = dFil.dViscosity_h3
				Case 4
					dAry = dFil.dViscosity_h4
				Case 5
					dAry = dFil.dViscosity_h5
				End Select
				if IsNothing(dAry)
					decM	= Double.NaN
				Else
					If i > dAry.GetUpperBound(0)
						decM	= Double.NaN
					Else
						decM	= dAry(i)
					End if
				End if
				rw(sChartItemStr(iKind) & j.ToString) = decM
			Next
		End If
		If iStd <> 0
			For j = iStdBase To iStdBase + iStd - 1
				Select Case j
				Case 6
					dAry = dFil.dViscosity_s1
				Case 7
					dAry = dFil.dViscosity_s2
				End Select
				if IsNothing(dAry)
					decM	= Double.NaN
				Else
					If i > dAry.GetUpperBound(0)
						decM	= Double.NaN
					Else
						decM	= dAry(i)
					End if
				End if
				rw(sChartItemStr(iKind) & j.ToString) = decM
			Next
		End If
    Case eChartItem.XPosPhase      'Position Phase
		If iHis <> 0
			For j = 1 To iHis
				Select Case j
				Case 1
					dAry = dFil.dXPosPhase_h1
				Case 2
					dAry = dFil.dXPosPhase_h2
				Case 3
					dAry = dFil.dXPosPhase_h3
				Case 4
					dAry = dFil.dXPosPhase_h4
				Case 5
					dAry = dFil.dXPosPhase_h5
				End Select
				if IsNothing(dAry)
					decM	= Double.NaN
				Else
					If i > dAry.GetUpperBound(0)
						decM	= Double.NaN
					Else
						decM	= dAry(i)
					End if
				End if
				rw(sChartItemStr(iKind) & j.ToString) = decM * RadToDegRate
			Next
		End If
		If iStd <> 0
			For j = iStdBase To iStdBase + iStd - 1
				Select Case j
				Case 6
					dAry = dFil.dXPosPhase_s1
				Case 7
					dAry = dFil.dXPosPhase_s2
				End Select
				if IsNothing(dAry)
					decM	= Double.NaN
				Else
					If i > dAry.GetUpperBound(0)
						decM	= Double.NaN
					Else
						decM	= dAry(i)
					End if
				End if
				rw(sChartItemStr(iKind) & j.ToString) = decM * RadToDegRate
			Next
		End If
    Case eChartItem.SplStrain      'Sample strain
		If iHis <> 0
			For j = 1 To iHis
				Select Case j
				Case 1
					dAry = dFil.dSplStrain_h1
				Case 2
					dAry = dFil.dSplStrain_h2
				Case 3
					dAry = dFil.dSplStrain_h3
				Case 4
					dAry = dFil.dSplStrain_h4
				Case 5
					dAry = dFil.dSplStrain_h5
				End Select
				 if IsNothing(dAry)
					decM	= Double.NaN
				Else
					If i > dAry.GetUpperBound(0)
						decM	= Double.NaN
					Else
						decM	= dAry(i)
					End if
				End if
				rw(sChartItemStr(iKind) & j.ToString) = decM
			Next
		End If
		If iStd <> 0
			For j = iStdBase To iStdBase + iStd - 1
				Select Case j
				Case 6
					dAry = dFil.dSplStrain_s1
				Case 7
					dAry = dFil.dSplStrain_s2
				End Select
				 if IsNothing(dAry)
					decM	= Double.NaN
				Else
					If i > dAry.GetUpperBound(0)
						decM	= Double.NaN
					Else
						decM	= dAry(i)
					End if
				End if
				rw(sChartItemStr(iKind) & j.ToString) = decM
			Next
		End If
    Case eChartItem.ShearMod      'Shear modulus
		If iHis <> 0
			For j = 1 To iHis
				Select Case j
				Case 1
					dAry = dFil.dShearMod_h1
				Case 2
					dAry = dFil.dShearMod_h2
				Case 3
					dAry = dFil.dShearMod_h3
				Case 4
					dAry = dFil.dShearMod_h4
				Case 5
					dAry = dFil.dShearMod_h5
				End Select
				if IsNothing(dAry)
					decM	= Double.NaN
				Else
					If i > dAry.GetUpperBound(0)
						decM	= Double.NaN
					Else
						decM	= dAry(i)
					End if
				End if
				rw(sChartItemStr(iKind) & j.ToString) = decM
			Next
		End If
		If iStd <> 0
			For j = iStdBase To iStdBase + iStd - 1
				Select Case j
				Case 6
					dAry = dFil.dShearMod_s1
				Case 7
					dAry = dFil.dShearMod_s2
				End Select
				if IsNothing(dAry)
					decM	= Double.NaN
				Else
					If i > dAry.GetUpperBound(0)
						decM	= Double.NaN
					Else
						decM	= dAry(i)
					End if
				End if
				rw(sChartItemStr(iKind) & j.ToString) = decM
			Next
		End If
    Case eChartItem.ShearVelo      'Shear Velosity
		If iHis <> 0
			For j = 1 To iHis
				Select Case j
				Case 1
					dAry = dFil.dShearVelo_h1
				Case 2
					dAry = dFil.dShearVelo_h2
				Case 3
					dAry = dFil.dShearVelo_h3
				Case 4
					dAry = dFil.dShearVelo_h4
				Case 5
					dAry = dFil.dShearVelo_h5
				End Select
				if IsNothing(dAry)
					decM	= Double.NaN
				Else
					If i > dAry.GetUpperBound(0)
						decM	= Double.NaN
					Else
						decM	= dAry(i)
					End if
				End if
				rw(sChartItemStr(iKind) & j.ToString) = decM
			Next
		End If
		If iStd <> 0
			For j = iStdBase To iStdBase + iStd - 1
				Select Case j
				Case 6
					dAry = dFil.dShearVelo_s1
				Case 7
					dAry = dFil.dShearVelo_s2
				End Select
				if IsNothing(dAry)
					decM	= Double.NaN
				Else
					If i > dAry.GetUpperBound(0)
						decM	= Double.NaN
					Else
						decM	= dAry(i)
					End if
				End if
				rw(sChartItemStr(iKind) & j.ToString) = decM
			Next
		End If
    Case eChartItem.XStrgMod      'Storage modulus
		If iHis <> 0
			For j = 1 To iHis
				Select Case j
				Case 1
					dAry = dFil.dXStrgMod_h1
				Case 2
					dAry = dFil.dXStrgMod_h2
				Case 3
					dAry = dFil.dXStrgMod_h3
				Case 4
					dAry = dFil.dXStrgMod_h4
				Case 5
					dAry = dFil.dXStrgMod_h5
				End Select
				 if IsNothing(dAry)
					decM	= Double.NaN
				Else
					If i > dAry.GetUpperBound(0)
						decM	= Double.NaN
					Else
						decM	= dAry(i)
					End if
				End if
				rw(sChartItemStr(iKind) & j.ToString) = decM
			Next
		End If
		If iStd <> 0
			For j = iStdBase To iStdBase + iStd - 1
				Select Case j
				Case 6
					dAry = dFil.dXStrgMod_s1
				Case 7
					dAry = dFil.dXStrgMod_s2
				End Select
				 if IsNothing(dAry)
					decM	= Double.NaN
				Else
					If i > dAry.GetUpperBound(0)
						decM	= Double.NaN
					Else
						decM	= dAry(i)
					End if
				End if
				rw(sChartItemStr(iKind) & j.ToString) = decM
			Next
		End If
    Case eChartItem.XElasMod      'Elastic modulus
		If iHis <> 0
			For j = 1 To iHis
				Select Case j
				Case 1
					dAry = dFil.dXElasMod_h1
				Case 2
					dAry = dFil.dXElasMod_h2
				Case 3
					dAry = dFil.dXElasMod_h3
				Case 4
					dAry = dFil.dXElasMod_h4
				Case 5
					dAry = dFil.dXElasMod_h5
				End Select
				if IsNothing(dAry)
					decM	= Double.NaN
				Else
					If i > dAry.GetUpperBound(0)
						decM	= Double.NaN
					Else
						decM	= dAry(i)
					End if
				End if
				rw(sChartItemStr(iKind) & j.ToString) = decM
			Next
		End If
		If iStd <> 0
			For j = iStdBase To iStdBase + iStd - 1
				Select Case j
				Case 6
					dAry = dFil.dXElasMod_s1
				Case 7
					dAry = dFil.dXElasMod_s2
				End Select
				if IsNothing(dAry)
					decM	= Double.NaN
				Else
					If i > dAry.GetUpperBound(0)
						decM	= Double.NaN
					Else
						decM	= dAry(i)
					End if
				End if
				rw(sChartItemStr(iKind) & j.ToString) = decM
			Next
		End If
    Case eChartItem.XLossTan      'Loss Tangent
		If iHis <> 0
			For j = 1 To iHis
				Select Case j
				Case 1
					dAry = dFil.dXLossTan_h1
				Case 2
					dAry = dFil.dXLossTan_h2
				Case 3
					dAry = dFil.dXLossTan_h3
				Case 4
					dAry = dFil.dXLossTan_h4
				Case 5
					dAry = dFil.dXLossTan_h5
				End Select
				if IsNothing(dAry)
					decM	= Double.NaN
				Else
					If i > dAry.GetUpperBound(0)
						decM	= Double.NaN
					Else
						decM	= dAry(i)
					End if
				End if
				rw(sChartItemStr(iKind) & j.ToString) = decM
			Next
		End If
		If iStd <> 0
			For j = iStdBase To iStdBase + iStd - 1
				Select Case j
				Case 6
					dAry = dFil.dXLossTan_s1
				Case 7
					dAry = dFil.dXLossTan_s2
				End Select
				if IsNothing(dAry)
					decM	= Double.NaN
				Else
					If i > dAry.GetUpperBound(0)
						decM	= Double.NaN
					Else
						decM	= dAry(i)
					End if
				End if
				rw(sChartItemStr(iKind) & j.ToString) = decM
			Next
		End If
    End Select

Catch ex As Exception
    Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
    Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub
''' <summary>
''' チャートコントロールの設定
''' </summary>
''' <param name="cts"> CToolSettings参照 </param>
''' <param name="Chart1"></param>
''' <param name="dt"></param>
''' <param name="iHis"></param>
''' <param name="iStd"></param>
''' <param name="sSelectGItem"></param>
''' <param name="sCalcMethod"></param>
'''Private Sub	RealTimeChartSetting(ByVal iType As Int16, ByRef Chart1 As Chart, ByRef dt As DataTable, ByVal iHis As int16, ByVal iStd As int16, ByVal sSelectGItem As String, ByVal sCalcMethod As String, ByVal iColor() As Color)
Private Sub	RealTimeChartSetting(ByVal cts As CToolSettings, ByRef Chart1 As Chart, ByRef dt As DataTable, ByVal iHis As int16, ByVal iStd As int16, ByVal sSelectGItem As String, ByVal sCalcMethod As String)
Dim j As int32
Dim strColumnName	As String
Dim iType			As Int16 = cts.iChartType
Dim iColor()		As Color = cts.clrY1Default

Try
	With Chart1
		'初期化
		.Series.Clear()
		.Legends.Clear()
		.Titles.Clear()
		.ChartAreas.Clear()

		'TITLE 作成
		Dim title1 As Title = new Title("")
		.Titles.Add(title1)
		.Titles.Item(0).Font = New Font(BASE_FONT, 16, FontStyle.Bold)
		'ChartArea作成
		.ChartAreas.Add("ChartArea1")

		'Seriesの作成
		If iHis <> 0    '履歴がある場合は表示順を測定中系列を最前面へ配置するため先に履歴を追加しておく
						'Series(0)->Series(1)->Series(2)...Series(n)[最前面]
''			For j = 1 To iHis
			For j = iHis To 1 Step -1
				strColumnName							= dt.Columns(1+j).ColumnName		'履歴系列名を取得
				.Series.Add(strColumnName)													'履歴系列追加
				.Series(strColumnName).Color			= iColor(1+j)						'色をセット
				Select Case iType
				Case 0	'トレンド
					If cts.iMarkerStyle = MarkerStyle.None
						.Series(strColumnName).ChartType	= SeriesChartType.FastLine		'グラフの種類設定
					Else
						.Series(strColumnName).ChartType	= SeriesChartType.Line			'グラフの種類設定
					End If
					.Series(strColumnName).YValueMembers	= strColumnName					'履歴の系列設定
				Case 1	'散布図
					.Series(strColumnName).ChartType		= SeriesChartType.FastPoint		'グラフの種類設定
					.Series(strColumnName).MarkerSize		= If(cts.iMarkerSize = 0, 1, cts.iMarkerSize)						'マーカサイズ
					.Series(strColumnName).MarkerStyle		= if(cts.iMarkerStyle=0,MarkerStyle.Circle,cts.iMarkerStyle)		'マーカスタイル
				End Select
			Next
		End If
		If iStd <> 0	'基準データがある場合
			Dim iStdBase As Int32 = iHis + 2
			For j = iStd - 1 To 0  Step -1
				strColumnName							= dt.Columns(iStdBase+j).ColumnName		'履歴系列名を取得
				.Series.Add(strColumnName)														'履歴系列追加
				.Series(strColumnName).Color			= iColor(cHistMax+2+j)					'色をセット
				Select Case iType
				Case 0
					If cts.iMarkerStyle = MarkerStyle.None
						.Series(strColumnName).ChartType	= SeriesChartType.FastLine		'グラフの種類設定
					Else
						.Series(strColumnName).ChartType	= SeriesChartType.Line			'グラフの種類設定
					End If
					.Series(strColumnName).YValueMembers	= strColumnName					'履歴の系列設定
				Case 1
					.Series(strColumnName).ChartType		= SeriesChartType.FastPoint		'グラフの種類設定
					.Series(strColumnName).MarkerSize		= If(cts.iMarkerSize = 0, 1, cts.iMarkerSize)						'マーカサイズ
					.Series(strColumnName).MarkerStyle		= if(cts.iMarkerStyle=0,MarkerStyle.Circle,cts.iMarkerStyle)		'マーカスタイル
				End Select
			Next
		End If

		strColumnName									= dt.Columns(1).ColumnName		'測定中系列名を取得
		.Series.Add(strColumnName)														'測定中系列追加
		.Series(strColumnName).Color					= iColor(1)						'色をセット
		Select Case iType
		Case 0		'トレンド
			.DataSource = dt
			If cts.iMarkerStyle = MarkerStyle.None
				.Series(strColumnName).ChartType	= SeriesChartType.FastLine		'グラフの種類設定
			Else
				.Series(strColumnName).ChartType	= SeriesChartType.Line			'グラフの種類設定
			End If
			.Series(strColumnName).XValueMember		= dt.Columns(0).ColumnName		'X軸 時間(秒)/温度(deg C)
			.Series(strColumnName).YValueMembers	= dt.Columns(1).ColumnName		'Y軸 測定中の系列
			.Series(strColumnName).YAxisType		= AxisType.Primary
			.Series(strColumnName).MarkerSize		= cts.iMarkerSize				'マーカサイズ
			.Series(strColumnName).MarkerStyle		= cts.iMarkerStyle	 			'マーカスタイル
		Case 1		'散布図
			.Series(strColumnName).ChartType		= SeriesChartType.FastPoint		'グラフの種類設定
			.Series(strColumnName).MarkerSize		= If(cts.iMarkerSize = 0, 1, cts.iMarkerSize)						'マーカサイズ
			.Series(strColumnName).MarkerStyle		= if(cts.iMarkerStyle=0,MarkerStyle.Circle,cts.iMarkerStyle)		'マーカスタイル
		End Select
#If 1
		'補助グラフ
		Select Case sSelectGItem
		Case "rbCorrect"
			Select Case sCalcMethod
			Case lstCalcMethod(eCalcMethod.SLOPE)
				.Series.Add(lstCalcMethod(eCalcMethod.SLOPE))
				.Series(lstCalcMethod(eCalcMethod.SLOPE)).Color			= Color.Black							'色をセット
				Select Case iType
				Case 0
					.Series(lstCalcMethod(eCalcMethod.SLOPE)).ChartType		= SeriesChartType.FastLine			'グラフの種類設定
				Case 1
					.Series(lstCalcMethod(eCalcMethod.SLOPE)).ChartType		= SeriesChartType.Point				'グラフの種類設定
					.Series(lstCalcMethod(eCalcMethod.SLOPE)).MarkerSize	= 10								'マーカサイズ
				End Select
				.Series(lstCalcMethod(eCalcMethod.SLOPE)).YValueMembers = dt.Columns(lstCalcMethod(eCalcMethod.SLOPE)).ColumnName   'Y軸 測定中の系列
				.Series(lstCalcMethod(eCalcMethod.SLOPE)).YAxisType		= AxisType.Secondary
			End Select
		End Select
#end if
		'Legendの作成
		dim leg As Legend = new Legend With {
			.DockedToChartArea = "ChartArea1",
			.Alignment = StringAlignment.Near
		}
		'作ったSeriesをchartコントロールに追加する
		.Legends.Add(leg)
		.Legends(0).Enabled = False
	End With
Catch ex As Exception
    Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
    Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub

''' <summary>
''' X,Y軸関係の設定処理
''' </summary>
''' <param name="arg"></param>
''' <param name="Chart1"></param>
''' <param name="dFil"></param>
''' <param name="clExt"></param>
''' <param name="cts"></param>
''' <param name="iHis"></param>
''' <param name="iStd"></param>
''' <param name="ChartGrid"></param>
''' <param name="ds"></param>
''' <param name="dTemp"></param>
''' <param name="dMv"></param>
''' <param name="iThinTyp"></param>
Private Sub ChartXYAxisSetting( _
        ByVal arg	        As ChartArg, _
        ByRef Chart1        As Chart, _
        ByRef dFil          As DataFile, _
        ByRef clExt         As Extra, _
        ByVal cts           As CToolSettings, _
        ByVal iHis          As int16, _
        ByVal iStd          As int16, _
        ByVal ChartGrid     As Integer, _
        ByVal ds            As dataset, _
        ByVal dTemp()       As Double, _
        ByVal dMv()	        As Double, _ 
        ByVal iThinTyp      As Int16 _
)
Dim dInterval   As Double
Dim iInterval   As Int32
Dim GridColor   As Color    = cts.penGrids.Color
Dim dMax		As Double	= 100
Dim dMin		As Double	= 0

Try
	If arg.dTempMin=0 And arg.dTempMax=0
		arg.dTempMax = 300
	End If
    With Chart1
		.ChartAreas(0).Position.Auto = True
        .ChartAreas(0).BackColor = cts.clrGraphBack                                 'プロット領域の背景色
        .ChartAreas(0).BorderColor = Color.DarkGray                                 'プロット領域の境界色
        .ChartAreas(0).BorderDashStyle = ChartDashStyle.Solid                       'プロット領域の境界線スタイル
        .ChartAreas(0).BorderWidth = 2                                              'プロット領域の境界線幅

        'X軸
        .ChartAreas(0).AxisX.Title                      = If(cts.iChartType = 0, m_sMes(efrmMainSTR.iCHART_X_TIME) , m_sMes(efrmMainSTR.iCHART_X_TEMP))
        .ChartAreas(0).AxisX.TitleFont                  = New Font(BASE_FONT, 16, FontStyle.Regular)
        .ChartAreas(0).AxisX.Crossing                   = Double.NaN
	    .ChartAreas(0).AxisX.IntervalType               = DateTimeIntervalType.NotSet
		.ChartAreas(0).AxisX.MajorGrid.Enabled          = True
		.ChartAreas(0).AxisX.MajorGrid.LineDashStyle    = ChartGrid
        .ChartAreas(0).AxisX.MajorGrid.LineColor        = GridColor
        .ChartAreas(0).AxisX.LabelStyle.Format			= "F1"
		''.ChartAreas(0).AxisX.LabelStyle.Font			=  New Font(BASE_FONT, 12, FontStyle.Regular)

        'Dim a As Double
        If IsNothing(dFil.iHosei) And iHis=0 And iStd=0
		'初期画面時
			Select Case cts.iChartType 
			Case 0	'時間
				''dMax = 5000
				''dMin = 1
				''iInterval = dMax \ 10									'初期画面 100秒スパン
				'dMax = 500
				'dMin = 1
				If cts.iRTXAxis = 0
				'自動
					dMax = cInitXSec * RealXaxisRatio(iThinTyp)
				Else
				'固定
					dMax = cts.MainGraphXMax(cts.iChartType) * RealXaxisRatio(iThinTyp)
				End If
				dMin = cts.MainGraphXMin(cts.iChartType) * RealXaxisRatio(iThinTyp)
				dInterval = (dMax-(dMin-1)) \ cts.MainGraphXDiv(cts.iChartType)			'目盛り数値
			Case 1  '温度
				'If arg.dTempMax<arg.dTempMin
				'	dMax = arg.dTempMin
				'	dMin = arg.dTempMax
				'Else
				'	dMax = arg.dTempMax
				'	dMin = arg.dTempMin
				'End If
				'iInterval = 0		
				If arg.dTempMax<arg.dTempMin
					dMax = cts.MainGraphXMin(cts.iChartType)
					dMin = cts.MainGraphXMax(cts.iChartType)
				Else
					If cts.iRTXAxis = 0
					'自動
						dMax = arg.dTempMax
						dMin = arg.dTempMin
						dInterval = (dMax-dMin) / cts.MainGraphXDiv(cts.iChartType)		
					Else
						dMax = cts.MainGraphXMax(cts.iChartType)
						dMin = cts.MainGraphXMin(cts.iChartType)
						dInterval = (dMax-dMin) / cts.MainGraphXDiv(cts.iChartType)		
					End if
				End If
			End Select
            .ChartAreas(0).AxisX.Maximum	= dMax
            .ChartAreas(0).AxisX.Minimum	= dMin
	        .ChartAreas(0).AxisX.Interval	= dInterval
		Else
		'表示データある時
            If cts.iRTXAxis = 0
            'Auto
				Select Case cts.iChartType 
				Case 0	'時間
					If Not IsNothing(dFil.iHosei)
						If p_iScrlStart > 0
							'dMax = dMin + (dFil.iHosei.Length-p_iScrlStart) / RealTimeThinning(iThinTyp) + 10
							dMax = dMin + (dFil.iHosei.Length-p_iScrlStart) / RealTimeThinning(iThinTyp) 
						Else
							dMax = dFil.iHosei.Length / RealTimeThinning(iThinTyp)
						End If
					Else
						Dim iMax As int64 = 0
						iMax = GetArrayMaxInHisStd(iHis, iStd, dFil) - 500
						dMax = iMax / RealTimeThinning(iThinTyp)
					End If
					'dMin = 1
					dMin = 0
					dMax \= 10
					dMax *= 10
					iInterval = (dMax-dMin) \ 100
					iInterval *= 10

					.ChartAreas(0).AxisX.Interval = iInterval

					.ChartAreas(0).AxisX.Maximum = dMax
					.ChartAreas(0).AxisX.Minimum = dMin
				Case 1	'温度
					If Not IsNothing(dFil.iTempBuf)
					'測定値データがある
						dMax = dFil.iTempBuf.Max/10
						dMin = Math.Ceiling(dFil.iTempBuf.Min/10/10 * 10)
						Dim dMaxH As Double = 0.0
						Select Case dMax
						Case < 50
							dMaxH = 50
						Case < 80
							dMaxH = 80
						Case < 100
							dMaxH = 100
						Case < 120
							dMaxH = 120
						Case < 140
							dMaxH = 140
						Case < 160
							dMaxH = 160
						Case < 180
							dMaxH = 180
						Case Else
							dMaxH = Math.Ceiling(dMax/10) * 10
						End Select
						dMax = dMaxH
					Else
					'測定値データがない
						'履歴・基準データから温度の最大最小を得る
						Dim iMax As Int32 = -99999
						Dim iMin As Int32 = 99999
						GetTempMaxInHisStd(iMax, iMin, dFil)
						dMax = iMax / 10
						dMin = iMin / 10
					End If
					If dMax = dMin Or Double.IsNaN(dMin) 
						.ChartAreas(0).AxisX.Maximum = dMax
						.ChartAreas(0).AxisX.Minimum = 0
					ElseIf dMax = 0 
						.ChartAreas(0).AxisX.Maximum = 250
						.ChartAreas(0).AxisX.Minimum = 0
					ElseIf dMax - dMin <> 0
						.ChartAreas(0).AxisX.Maximum = dMax
						.ChartAreas(0).AxisX.Minimum = dMin
					End If

				End Select
            Else
            'Fixed
				Select Case cts.iChartType
				Case 0	'時間
					dMax = cts.MainGraphXMax(cts.iChartType) * RealXaxisRatio(iThinTyp)
					dMin = cts.MainGraphXMin(cts.iChartType) * RealXaxisRatio(iThinTyp)
					iInterval = (dMax-(dMin-1)) \ cts.MainGraphXDiv(cts.iChartType)		'目盛りインターバル数値
					.ChartAreas(0).AxisX.Maximum = dMax
					.ChartAreas(0).AxisX.Minimum = dMin
					.ChartAreas(0).AxisX.Interval = iInterval

				Case 1	'温度
					dMax = cts.MainGraphXMax(cts.iChartType)
					dMin = cts.MainGraphXMin(cts.iChartType)
					dInterval = (dMax-(dmin-1)) \ cts.MainGraphXDiv(cts.iChartType)			'目盛りインターバル数値
					.ChartAreas(0).AxisX.Maximum = dMax
					.ChartAreas(0).AxisX.Minimum = dMin
					.ChartAreas(0).AxisX.Interval = dInterval

				End Select
            End If
			.ChartAreas(0).AxisX.LabelStyle.Format			= "F1"
			'.ChartAreas(0).AxisX.LabelStyle.Font    =  New Font(BASE_FONT, 12, FontStyle.Regular)
        End If

		'フォント自動設定
		With Chart1.ChartAreas(0).AxisX
			.IsLabelAutoFit = True
			.LabelAutoFitMinFontSize = 5
			.LabelAutoFitMaxFontSize = 12
		End With
		.ChartAreas(0).AxisX.ScaleView.Zoomable = False


        'Y軸
		'Y軸スパン計算
        AxisMaxMinSetting( _
			arg, _
			Chart1, _
			dFil, _
			cts, _
			iThinTyp, _
			iHis, _
			iStd, _
			dMax, _
			dMin
		)
Try
		If Double.IsInfinity(dMax) Or Double.IsInfinity(dMin)
			dMax = 1000
			dMin = 0
		End If
		.ChartAreas(0).AxisY.Maximum			= dMax
		.ChartAreas(0).AxisY.Minimum			= dMin
Catch ex As System.OverflowException
		Call WriteLog("オーバーフローエラー グラフ表示スパン MAX:" & dMax & " - MIN:" & dMin,eLOG_LEVEL.iFATAL)
End Try		

		Select Case arg.sSelectGItem
		Case "rbXStress"
	        .ChartAreas(0).AxisY.Title          = sChartItemName(eChartItem.OxBuf) & sChartItemUnit(eChartItem.OxBuf)
		Case "rbYStress"
	        .ChartAreas(0).AxisY.Title          = sChartItemName(eChartItem.OyBuf) & sChartItemUnit(eChartItem.OyBuf)
		Case "rbZStress"
	        .ChartAreas(0).AxisY.Title          = sChartItemName(eChartItem.OzBuf) & sChartItemUnit(eChartItem.OzBuf)
		Case "rbZPosition"
	        .ChartAreas(0).AxisY.Title          = sChartItemName(eChartItem.OzPos) & sChartItemUnit(eChartItem.OzPos)
		Case "rbHPTemp"
	        .ChartAreas(0).AxisY.Title          = sChartItemName(eChartItem.TempBuf) & sChartItemUnit(eChartItem.TempBuf)
		Case "rbPBTemp"
	        .ChartAreas(0).AxisY.Title          = sChartItemName(eChartItem.TempBuf2) & sChartItemUnit(eChartItem.TempBuf2)
		Case "rbHPMV"
	        .ChartAreas(0).AxisY.Title          = sChartItemName(eChartItem.MvBuf) & sChartItemUnit(eChartItem.MvBuf)
		Case "rbPBMV"
	        .ChartAreas(0).AxisY.Title          = sChartItemName(eChartItem.MvBuf2) & sChartItemUnit(eChartItem.MvBuf2)
        Case "rbCorrect"
	        .ChartAreas(0).AxisY.Title          = sChartItemName(eChartItem.Hosei) & sChartItemUnit(eChartItem.Hosei)
        Case "rbCorrectP"
	        .ChartAreas(0).AxisY.Title          = sChartItemName(eChartItem.HoseiP) & sChartItemUnit(eChartItem.HoseiP)
        Case "rbViscosity"
	        .ChartAreas(0).AxisY.Title          = sChartItemName(eChartItem.Viscosity) & sChartItemUnit(eChartItem.Viscosity)
        Case "rbPosPhase"
	        .ChartAreas(0).AxisY.Title          = sChartItemName(eChartItem.XPosPhase) & sChartItemUnit(eChartItem.XPosPhase)
        Case "rbSampleStrain"
	        .ChartAreas(0).AxisY.Title          = sChartItemName(eChartItem.SplStrain) & sChartItemUnit(eChartItem.SplStrain)
        Case "rbShearMod"
	        .ChartAreas(0).AxisY.Title          = sChartItemName(eChartItem.ShearMod) & sChartItemUnit(eChartItem.ShearMod)
        Case "rbShearVelo"
	        .ChartAreas(0).AxisY.Title          = sChartItemName(eChartItem.ShearVelo) & sChartItemUnit(eChartItem.ShearVelo)
        Case "rbStorageMod"
	        .ChartAreas(0).AxisY.Title          = sChartItemName(eChartItem.XStrgMod) & sChartItemUnit(eChartItem.XStrgMod)
        Case "rbElasticMod"
	        .ChartAreas(0).AxisY.Title          = sChartItemName(eChartItem.XElasMod) & sChartItemUnit(eChartItem.XElasMod)
        Case "rbLossTangent"
	        .ChartAreas(0).AxisY.Title          = sChartItemName(eChartItem.XLossTan) & sChartItemUnit(eChartItem.XLossTan)
		Case Else
	        .ChartAreas(0).AxisY.Title          = "N/A"
		End Select
        .ChartAreas(0).AxisY.TitleFont          =  New Font(BASE_FONT, 16, FontStyle.Regular)
        .ChartAreas(0).AxisY.TextOrientation    = TextOrientation.Rotated270       'テキストの向き
		.ChartAreas(0).AxisY.Crossing           = Double.NaN
		.ChartAreas(0).AxisY.LabelStyle.Font    =  New Font(BASE_FONT, 12, FontStyle.Regular)

		If cts.iRTYAxis = 0
		'自動
			.ChartAreas(0).AxisY.IntervalAutoMode   = IntervalAutoMode.FixedCount
		Else
		'固定
			dInterval								= (.ChartAreas(0).AxisY.Maximum-.ChartAreas(0).AxisY.Minimum) / cts.MainGraphYDiv(arg.iSelectGItem)
			Dim sInterval As String = dInterval.ToString()
			Dim idx As Int16 = sInterval.IndexOf(".")
			Dim i As int32
			Select Case idx
			Case <=1	'1以下
				For i=idx+1 To sInterval.Length-1
					If sInterval.Substring(i,1)<>"0"
						Exit for
					End If
				Next
				.ChartAreas(0).AxisY.LabelStyle.Format	= "F" & i.ToString
			Case Else
				.ChartAreas(0).AxisY.LabelStyle.Format	= "F0"
			End Select
			.ChartAreas(0).AxisY.Interval			= dInterval

		End If
		.ChartAreas(0).AxisY.Intervaltype       = DateTimeIntervalType.Number

		'第二Y軸がある場合は第二側のグリッド線を引く
        Select Case arg.sCalcMethod
        Case lstCalcMethod(eCalcMethod.THR), lstCalcMethod(eCalcMethod.EXTRA)
			.ChartAreas(0).AxisY.MajorGrid.Enabled          = True
			.ChartAreas(0).AxisY.MajorGrid.LineDashStyle    = ChartGrid
			.ChartAreas(0).AxisY.MajorGrid.LineColor        = GridColor
			.ChartAreas(0).AxisY2.MajorGrid.Enabled         = False

        Case lstCalcMethod(eCalcMethod.SLOPE)
			If arg.sSelectGItem = "rbCorrect"
			'合成応力シリーズの時のみ
				.ChartAreas(0).AxisY.MajorGrid.Enabled          = True
				.ChartAreas(0).AxisY.MajorGrid.LineDashStyle    = ChartDashStyle.NotSet
				.ChartAreas(0).AxisY.MajorGrid.LineColor        = Chart1.ChartAreas(0).BackColor

				.ChartAreas(0).AxisY2.MajorGrid.Enabled         = True
				.ChartAreas(0).AxisY2.MajorGrid.LineDashStyle   = ChartGrid
				.ChartAreas(0).AxisY2.MajorGrid.LineColor       = GridColor
			Else
				.ChartAreas(0).AxisY.MajorGrid.Enabled          = True
				.ChartAreas(0).AxisY.MajorGrid.LineDashStyle    = ChartGrid
				.ChartAreas(0).AxisY.MajorGrid.LineColor        = GridColor
				.ChartAreas(0).AxisY2.MajorGrid.Enabled         = False
			End If
			.ChartAreas(0).AxisY2.LabelStyle.Font    =  New Font(BASE_FONT, 12, FontStyle.Regular)
		Case Else
			.ChartAreas(0).AxisY.MajorGrid.Enabled          = True
			.ChartAreas(0).AxisY.MajorGrid.LineDashStyle    = ChartGrid
			.ChartAreas(0).AxisY.MajorGrid.LineColor        = GridColor
			.ChartAreas(0).AxisY2.MajorGrid.Enabled         = False
			.ChartAreas(0).AxisY2.LabelStyle.Font    =  New Font(BASE_FONT, 12, FontStyle.Regular)
		End Select
		.ChartAreas(0).AxisY.ScaleView.Zoomable         = False

 'Debug.print( "AxisY.Maximum = " & Chart1.ChartAreas(0).AxisY.Maximum )
		End With
Catch ex As Exception
    Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Call WriteLog("Graph display span MAX:" & dMax & " - MIN:" & dMin,eLOG_LEVEL.iFATAL)
    Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub
''' <summary>
''' 履歴・基準データの配列最大を得る
''' </summary>
''' <param name="iHis"></param>
''' <param name="iStd"></param>
''' <param name="dFil"></param>
''' <returns></returns>
Private Function GetArrayMaxInHisStd(ByVal iHis As int32, ByVal iStd As Int32, ByRef dFil As DataFile) As Int32
Dim iMax As Int32
Try
	If iHis<>0
		For i=0 To iHis - 1
			Select Case i
			Case 0
				If Not IsNothing(dFil.iTempBuf_h1)
					If dFil.iTempBuf_h1.Length > iMax
						iMax = dFil.iTempBuf_h1.Length
					End If
				Else
					iMax = 100
				End If
			Case 1
				If Not IsNothing(dFil.iTempBuf_h2)
					If dFil.iTempBuf_h2.Length > iMax
						iMax = dFil.iTempBuf_h2.Length
					End If
				Else
					iMax = 100
				End If
			Case 2
				If Not IsNothing(dFil.iTempBuf_h3)
					If dFil.iTempBuf_h3.Length > iMax
						iMax = dFil.iTempBuf_h3.Length
					End If
				Else
					iMax = 100
				End If
			Case 3
				If Not IsNothing(dFil.iTempBuf_h4)
					If dFil.iTempBuf_h4.Length > iMax
						iMax = dFil.iTempBuf_h4.Length
					End If
				Else
					iMax = 100
				End If
			Case 4
				If Not IsNothing(dFil.iTempBuf_h5)
					If dFil.iTempBuf_h5.Length > iMax
						iMax = dFil.iTempBuf_h5.Length
					End If
				Else
					iMax = 100
				End If
			End Select
		Next
	End If
	If iStd<>0
		For i=0 To iStd - 1
			Select Case i
			Case 0
				If Not IsNothing(dFil.iTempBuf_s1)
					If dFil.iTempBuf_s1.Length > iMax
						iMax = dFil.iTempBuf_s1.Length
					End If
				Else
					iMax = 100
				End If
			Case 1
				If Not IsNothing(dFil.iTempBuf_s2)
					If dFil.iTempBuf_s2.Length > iMax
						iMax = dFil.iTempBuf_s2.Length
					End If
				Else
					iMax = 100
				End If
			End Select
		Next
	End If
	Return iMax
Catch ex As Exception
	Return -1
End Try
End Function
''' <summary>
''' 履歴・基準から温度データの最大最小を得る
''' </summary>
''' <param name="iMax"></param>
''' <param name="iMin"></param>
''' <param name="dFil"></param>
Private Sub GetTempMaxInHisStd(ByRef iMax As Int32, ByRef iMin As Int32, ByRef dFil As DataFile)
Try
	If	Not   IsNothing(dFil.iTempBuf_h1)
		iMax = dFil.iTempBuf_h1.Max
		iMin = dFil.iTempBuf_h1.Min
	ElseIf Not IsNothing(dFil.iTempBuf_h2)
		If iMax < dFil.iTempBuf_h2.Max
			iMax = dFil.iTempBuf_h2.Max
		End If
		If iMin > dFil.iTempBuf_h2.Min
			iMin = dFil.iTempBuf_h2.Min
		End If
	ElseIf Not IsNothing(dFil.iTempBuf_h3)
		If iMax < dFil.iTempBuf_h3.Max
			iMax = dFil.iTempBuf_h3.Max
		End If
		If iMin > dFil.iTempBuf_h3.Min
			iMin = dFil.iTempBuf_h3.Min
		End If
	ElseIf Not IsNothing(dFil.iTempBuf_h4)
		If iMax < dFil.iTempBuf_h4.Max
			iMax = dFil.iTempBuf_h4.Max
		End If
		If iMin > dFil.iTempBuf_h4.Min
			iMin = dFil.iTempBuf_h4.Min
		End If
	ElseIf Not IsNothing(dFil.iTempBuf_h5)
		If iMax < dFil.iTempBuf_h5.Max
			iMax = dFil.iTempBuf_h5.Max
		End If
		If iMin > dFil.iTempBuf_h5.Min
			iMin = dFil.iTempBuf_h5.Min
		End If
	ElseIf Not IsNothing(dFil.iTempBuf_s1)
		If iMax < dFil.iTempBuf_s1.Max
			iMax = dFil.iTempBuf_s1.Max
		End If
		If iMin > dFil.iTempBuf_s1.Min
			iMin = dFil.iTempBuf_s1.Min
		End If
	ElseIf Not IsNothing(dFil.iTempBuf_s2)
		If iMax < dFil.iTempBuf_s2.Max
			iMax = dFil.iTempBuf_s2.Max
		End If
		If iMin > dFil.iTempBuf_s2.Min
			iMin = dFil.iTempBuf_s2.Min
		End If
	End If
Catch ex As Exception
    Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub
Private Function GetXIntervalD(ByVal iDiv As Int32, ByVal dSpan As Double, ByVal iKetaDown As Int32) As Double
Dim dInterval As Double
Try
	dInterval = dSpan / iDiv
	Return Math.Round(dInterval, iKetaDown)

Catch ex As Exception
    Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Return -1
End Try
End Function
Private Function GetXInterval(ByVal iDiv As Int32, ByVal dSpan As Double, ByVal iKetaDown As Int32) As Int32
Dim iKeta As Int32
Dim iInterval As Int32
Try
	iInterval = dSpan \ iDiv
    iKeta = iInterval.ToString.Length - iKetaDown
    if iKeta >= 0
        iInterval /= 10^(iKeta)
        iInterval *= 10^(iKeta)
    End If
	Return iInterval
Catch ex As Exception
    Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Return -1
End Try
End Function

''' <summary>
''' Y軸のスパン情報の設定
''' </summary>
''' <param name="arg"></param>
''' <param name="Chart1"></param>
''' <param name="dFil"></param>
''' <param name="cts"></param>
''' <param name="iThinTyp"></param>
''' <param name="iHis"></param>
''' <param name="iStd"></param>
Private Sub AxisMaxMinSetting( _
								ByVal arg			As ChartArg, _
                                ByRef Chart1        As Chart, _
                                ByRef dFil          As DataFile, _
                                ByVal cts           As CToolSettings, _
								ByVal iThinTyp		As Int16, _ 
                                ByVal iHis          As int16, _
                                ByVal iStd          As int16, _
								ByRef dGMax			As Double, _
								ByRef dGMin			As Double _
                                )
Dim iAryMax(8) As int64				'最大値配列 全て(測定中＋履歴)
Dim iAryMin(8) As int64				'最小値配列 全て(測定中＋履歴)
Dim dAryMax(8) As Double			'最大値配列 全て(測定中＋履歴)
Dim dAryMin(8) As Double			'最小値配列 全て(測定中＋履歴)
Dim dHisMax As Double                'Y軸AutoFitのための最大値
Dim dHisMin As Double                'Y軸AutoFitのための最小値
Dim dHisMax2 As Double               'Y軸2AutoFitのための最大値
Dim dHisMin2 As Double               'Y軸2AutoFitのための最小値
Dim iRet	As Int16
Dim iStdBase As Int32   = cHistMax+1
Dim iInterval As int32 

Try
	For i As Int32 = 0 To iAryMax.GetUpperBound(0)
		iAryMax(i) = Int64.MinValue
		iAryMin(i) = Int64.MaxValue
		dAryMax(i) = Double.MinValue
		dAryMin(i) = Double.MaxValue
	Next
    With Chart1
	'Y軸の最大最小設定 -----------------------
    Select case arg.sSelectGItem
	Case "rbXStress"
        If cts.iRTYAxis = 1
	    'Y軸スパン 固定
			dHisMax = cts.MainGraphYMax(arg.iSelectGItem) * (arg.dYSpanPer / 100.0)
            dHisMin = cts.MainGraphYMin(arg.iSelectGItem)
		Else
		   If Not IsNothing(dFil.iOxBuf)
 				iAryMax(0) = dFil.iOxBuf.Max
				iAryMin(0) = dFil.iOxBuf.Min
			Else
 				iAryMax(0) = 10
				iAryMin(0) = -10
			End If
			For j = 1 To iHis
				Select Case j
				Case 1
        			iAryMax(j) = dFil.iOxBuf_h1.Max
        			iAryMin(j) = dFil.iOxBuf_h1.Min
				Case 2
        			iAryMax(j) = dFil.iOxBuf_h2.Max
        			iAryMin(j) = dFil.iOxBuf_h2.Min
				Case 3
        			iAryMax(j) = dFil.iOxBuf_h3.Max
        			iAryMin(j) = dFil.iOxBuf_h3.Min
				Case 4
        			iAryMax(j) = dFil.iOxBuf_h4.Max
        			iAryMin(j) = dFil.iOxBuf_h4.Min
				Case 5
        			iAryMax(j) = dFil.iOxBuf_h5.Max
        			iAryMin(j) = dFil.iOxBuf_h5.Min
				End Select
			Next
			For j = iStdBase To iStdBase - 1 + iStd
				Select Case j
				Case 6
					If Not IsNothing(dFil.iOxBuf_S1)
        				iAryMax(j) = dFil.iOxBuf_S1.Max
        				iAryMin(j) = dFil.iOxBuf_S1.Min
					End If
				Case 7
					If Not IsNothing(dFil.iOxBuf_S2)
        				iAryMax(j) = dFil.iOxBuf_S2.Max
        				iAryMin(j) = dFil.iOxBuf_S2.Min
					End If
				End Select
			Next
			dHisMax = iAryMax.Max
			dHisMin = iAryMin.Min
		End if
		If dHisMax - dHisMin <> 0
			dGMax = dHisMax
			dGMin = dHisMin
		End If

	Case "rbYStress"
         If cts.iRTYAxis = 1
	    'Y軸スパン 固定
			dHisMax = cts.MainGraphYMax(arg.iSelectGItem) * (arg.dYSpanPer / 100.0)
            dHisMin = cts.MainGraphYMin(arg.iSelectGItem)
		Else
		   If Not IsNothing(dFil.iOyBuf)
				iAryMax(0) = dFil.iOyBuf.Max
				iAryMin(0) = dFil.iOyBuf.Min
			Else
 				iAryMax(0) = 10
				iAryMin(0) = -10
			End If
			For j = 1 To iHis
				Select Case j
				Case 1
        			iAryMax(j) = dFil.iOyBuf_h1.Max
        			iAryMin(j) = dFil.iOyBuf_h1.Min
				Case 2
        			iAryMax(j) = dFil.iOyBuf_h2.Max
        			iAryMin(j) = dFil.iOyBuf_h2.Min
				Case 3
        			iAryMax(j) = dFil.iOyBuf_h3.Max
        			iAryMin(j) = dFil.iOyBuf_h3.Min
				Case 4
        			iAryMax(j) = dFil.iOyBuf_h4.Max
        			iAryMin(j) = dFil.iOyBuf_h4.Min
				Case 5
        			iAryMax(j) = dFil.iOyBuf_h5.Max
        			iAryMin(j) = dFil.iOyBuf_h5.Min
				End Select
			Next
			For j = iStdBase To iStdBase - 1 + iStd
				Select Case j
				Case 6
					If Not IsNothing(dFil.iOyBuf_s1)
        				iAryMax(j) = dFil.iOyBuf_s1.Max
        				iAryMin(j) = dFil.iOyBuf_s1.Min
					End if
				Case 7
					If Not IsNothing(dFil.iOyBuf_s2)
        				iAryMax(j) = dFil.iOyBuf_s2.Max
        				iAryMin(j) = dFil.iOyBuf_s2.Min
					End If
				End Select
			Next
			dHisMax = iAryMax.Max
			dHisMin = iAryMin.Min
		End If
		If dHisMax - dHisMin <> 0
			dGMax = dHisMax
			dGMin = dHisMin
		End If
	Case "rbZStress"		'g->N換算して使用
         If cts.iRTYAxis = 1
	    'Y軸スパン 固定
			dHisMax = cts.MainGraphYMax(arg.iSelectGItem) * (arg.dYSpanPer / 100.0)
            dHisMin = cts.MainGraphYMin(arg.iSelectGItem)
		Else
			If Not IsNothing(dFil.dOzBuf)
				dAryMax(0) = dFil.dOzBuf.Max * (cGraviAcc / 1000.0)
				dAryMin(0) = dFil.dOzBuf.Min * (cGraviAcc / 1000.0)
			Else
 				dAryMax(0) = 50
				dAryMin(0) = 0
			End If
			For j = 1 To iHis
				Select Case j
				Case 1
        			dAryMax(j) = dFil.dOzBuf_h1.Max * (cGraviAcc / 1000.0)
        			dAryMin(j) = dFil.dOzBuf_h1.Min * (cGraviAcc / 1000.0)
				Case 2
        			dAryMax(j) = dFil.dOzBuf_h2.Max * (cGraviAcc / 1000.0)
        			dAryMin(j) = dFil.dOzBuf_h2.Min * (cGraviAcc / 1000.0)
				Case 3
        			dAryMax(j) = dFil.dOzBuf_h3.Max * (cGraviAcc / 1000.0)
        			dAryMin(j) = dFil.dOzBuf_h3.Min * (cGraviAcc / 1000.0)
				Case 4
        			dAryMax(j) = dFil.dOzBuf_h4.Max * (cGraviAcc / 1000.0)
        			dAryMin(j) = dFil.dOzBuf_h4.Min * (cGraviAcc / 1000.0)
				Case 5
        			dAryMax(j) = dFil.dOzBuf_h5.Max * (cGraviAcc / 1000.0)
        			dAryMin(j) = dFil.dOzBuf_h5.Min * (cGraviAcc / 1000.0)
				End Select
			Next
			For j = iStdBase To iStdBase - 1 + iStd
				Select Case j
				Case 6
					If Not IsNothing(dFil.dOzBuf_s1)
        				dAryMax(j) = dFil.dOzBuf_s1.Max * (cGraviAcc / 1000.0)
        				dAryMin(j) = dFil.dOzBuf_s1.Min * (cGraviAcc / 1000.0)
					End If
				Case 7
					If Not IsNothing(dFil.dOzBuf_s2)
        				dAryMax(j) = dFil.dOzBuf_s2.Max * (cGraviAcc / 1000.0)
        				dAryMin(j) = dFil.dOzBuf_s2.Min * (cGraviAcc / 1000.0)
					End If
				End Select
			Next
			dHisMax = dAryMax.Max
			dHisMin = dAryMin.Min
		End If
		If dHisMax - dHisMin <> 0
			dGMax = dHisMax
			dGMin = dHisMin
		End If
		.ChartAreas(0).AxisY.LabelStyle.Format			= "F2"

	Case "rbZPosition"
         If cts.iRTYAxis = 1
	    'Y軸スパン 固定
			dHisMax = cts.MainGraphYMax(arg.iSelectGItem) * (arg.dYSpanPer / 100.0)
            dHisMin = cts.MainGraphYMin(arg.iSelectGItem)
		Else
		   If Not IsNothing(dFil.iPzBuf)
				iAryMax(0) = dFil.iPzBuf.Max
				iAryMin(0) = dFil.iPzBuf.Min
			Else
 				iAryMax(0) = 10
				iAryMin(0) = -10
			End If
			For j = 1 To iHis
				Select Case j
				Case 1
        			iAryMax(j) = dFil.iPzBuf_h1.Max
        			iAryMin(j) = dFil.iPzBuf_h1.Min
				Case 2
        			iAryMax(j) = dFil.iPzBuf_h2.Max
        			iAryMin(j) = dFil.iPzBuf_h2.Min
				Case 3
        			iAryMax(j) = dFil.iPzBuf_h3.Max
        			iAryMin(j) = dFil.iPzBuf_h3.Min
				Case 4
        			iAryMax(j) = dFil.iPzBuf_h4.Max
        			iAryMin(j) = dFil.iPzBuf_h4.Min
				Case 5
        			iAryMax(j) = dFil.iPzBuf_h5.Max
        			iAryMin(j) = dFil.iPzBuf_h5.Min
				End Select
			Next
			For j = iStdBase To iStdBase - 1 + iStd
				Select Case j
				Case 6
					If Not IsNothing(dFil.iPzBuf_s1)
        				iAryMax(j) = dFil.iPzBuf_s1.Max
        				iAryMin(j) = dFil.iPzBuf_s1.Min
					End if
				Case 7
					If Not IsNothing(dFil.iPzBuf_s2)
        				iAryMax(j) = dFil.iPzBuf_s2.Max
        				iAryMin(j) = dFil.iPzBuf_s2.Min
					End If
				End Select
			Next
			dHisMax = iAryMax.Max
			dHisMin = iAryMin.Min
		End If
		If dHisMax - dHisMin <> 0
			dGMax = dHisMax
			dGMin = dHisMin
		End If

	Case "rbCorrect"
        If cts.iRTYAxis = 1
        'Y軸スパン 固定
			'Select Case arg.dFullStressADVal * (arg.dYSpanPer / 100.0)
			'Case < 1000
			'	dHisMax = 1000
			'Case < 10000
			'	dHisMax = 10000
			'Case < 100000
			'	dHisMax = 100000
			'Case < 1000000
			'	dHisMax = 1000000
			'Case Else
			'	dHisMax = Math.Ceiling( arg.dFullStressADVal * (arg.dYSpanPer / 100.0) / 10000) * 10000
			'End Select
            'dHisMin = 0
			dHisMax = cts.MainGraphYMax(arg.iSelectGItem) * (arg.dYSpanPer / 100.0)
            dHisMin = cts.MainGraphYMin(arg.iSelectGItem)
        Else
            If Not IsNothing(dFil.iHosei)
		        dAryMax(0) = dFil.iHosei.Max
		        dAryMin(0) = dFil.iHosei.Min
            Else
		        dAryMax(0) = 1000
		        dAryMin(0) = 0
            End If
            For j = 1 To iHis
                Select Case j
                Case 1
        		    dAryMax(j) = dFil.iHosei_h1.Max
        		    dAryMin(j) = dFil.iHosei_h1.Min
                Case 2
        		    dAryMax(j) = dFil.iHosei_h2.Max
        		    dAryMin(j) = dFil.iHosei_h2.Min
                Case 3
        		    dAryMax(j) = dFil.iHosei_h3.Max
        		    dAryMin(j) = dFil.iHosei_h3.Min
                Case 4
        		    dAryMax(j) = dFil.iHosei_h4.Max
        		    dAryMin(j) = dFil.iHosei_h4.Min
                Case 5
        		    dAryMax(j) = dFil.iHosei_h5.Max
        		    dAryMin(j) = dFil.iHosei_h5.Min
                End Select
            Next
			For j = iStdBase To iStdBase - 1 + iStd
				Select Case j
                Case 6
					If Not IsNothing(dFil.iHosei_s1)
        				dAryMax(j) = dFil.iHosei_s1.Max
        				dAryMin(j) = dFil.iHosei_s1.Min
					End If
                Case 7
					If Not IsNothing(dFil.iHosei_s2)
        				dAryMax(j) = dFil.iHosei_s2.Max
        				dAryMin(j) = dFil.iHosei_s2.Min
					End If
				End Select
			Next
			dHisMax = dAryMax.Max
			dHisMin = dAryMin.Min
			Dim dMaxH As Double = 0.0
			Select Case dHisMax
			Case < 100
				dMaxH = 100
			Case < 1000
				dMaxH = 1000
			Case < 10000
				dMaxH = 10000
			Case Else
				dMaxH = Math.Ceiling(dHisMax/10000) * 10000
			End Select
			dHisMax = dMaxH
        End if
		If dHisMax = dHisMin Or Double.IsNaN(dHisMin)
			dGMax = dHisMax
			dGMin = 0
		ElseIf dHisMax = 0 
			dGMax = 1000
			dGMin = 0
		ElseIf dHisMax - dHisMin <> 0
			dGMax = dHisMax
			dGMin = dHisMin
		End If
		If cts.iRTYAxis = 1
			.ChartAreas(0).AxisY.Interval = dHisMax  / 10
		Else
			.ChartAreas(0).AxisY.Interval = 0
		End If
		.ChartAreas(0).AxisY.LabelStyle.Format			= "F0"

        '補助グラフ用第2軸
		Select Case arg.sCalcMethod
		Case lstCalcMethod(eCalcMethod.SLOPE)
			'有効範囲内の最大最小を得る
			iRet = GetMaxMin1ArrayRange(dFil.iSlope, arg.dAprxRangeMin, arg.dAprxRangeMax, dHisMax2, dHisMin2)
			If iRet <> 0 Or dHisMax2 = 0
				dHisMax2 = 1000
			End If
			dHisMin2 = 0
			Dim dMaxH2 As Double
			Select Case dHisMax2
			Case < 100
				dMaxH2 = Math.Ceiling( dHisMax2 * (arg.dYSpanPer / 100.0) / 10) * 10
			Case < 1000
				dMaxH2 = Math.Ceiling( dHisMax2 * (arg.dYSpanPer / 100.0) / 100) * 100
			Case < 10000
				dMaxH2 = Math.Ceiling( dHisMax2 * (arg.dYSpanPer / 100.0) / 1000) * 1000
			Case Else
				dMaxH2 = Math.Ceiling( dHisMax2 * (arg.dYSpanPer / 100.0) / 10000) * 10000
			End Select
			dHisMax2 = dMaxH2
			If dHisMax2 = dHisMin2 Or Double.IsNaN(dHisMin2) 
				.ChartAreas(0).AxisY2.Maximum = dHisMax2
				.ChartAreas(0).AxisY2.Minimum = 0
			ElseIf Double.IsNaN(dHisMax2)
				.ChartAreas(0).AxisY2.Maximum = 1000
				.ChartAreas(0).AxisY2.Minimum = 0
			ElseIf dHisMax2 = 0 
				.ChartAreas(0).AxisY2.Maximum = 1000
				.ChartAreas(0).AxisY2.Minimum = 0
			ElseIf dHisMax - dHisMin <> 0
				.ChartAreas(0).AxisY2.Maximum = dHisMax2
				.ChartAreas(0).AxisY2.Minimum = 0
			End If
			.ChartAreas(0).AxisY2.LabelStyle.Format			= "F0"
        End Select

	Case "rbCorrectP"
        If cts.iRTYAxis = 1
        'Y軸スパン 固定
         '   dHisMax = 100 * (arg.dYSpanPer / 100.0)
	        'dHisMin = 0
			dHisMax = cts.MainGraphYMax(arg.iSelectGItem) * (arg.dYSpanPer / 100.0)
            dHisMin = cts.MainGraphYMin(arg.iSelectGItem)
        Else
            If Not IsNothing(dFil.dHosei100)
		        dAryMax(0) = dFil.dHosei100.Max
		        dAryMin(0) = dFil.dHosei100.Min
            Else
		        dAryMax(0) = 100
		        dAryMin(0) = 0
            End If
            For j = 1 To iHis
                Select Case j
                Case 1
        		    dAryMax(j) = dFil.dHoseiP_h1.Max
        		    dAryMin(j) = dFil.dHoseiP_h1.Min
                Case 2
        		    dAryMax(j) = dFil.dHoseiP_h2.Max
        		    dAryMin(j) = dFil.dHoseiP_h2.Min
                Case 3
        		    dAryMax(j) = dFil.dHoseiP_h3.Max
        		    dAryMin(j) = dFil.dHoseiP_h3.Min
                Case 4
        		    dAryMax(j) = dFil.dHoseiP_h4.Max
        		    dAryMin(j) = dFil.dHoseiP_h4.Min
                Case 5
        		    dAryMax(j) = dFil.dHoseiP_h5.Max
        		    dAryMin(j) = dFil.dHoseiP_h5.Min
                End Select
            Next
			For j = iStdBase To iStdBase - 1 + iStd
				Select Case j
                Case 6
					If Not IsNothing(dFil.dHoseiP_s1)
        				dAryMax(j) = dFil.dHoseiP_s1.Max
        				dAryMin(j) = dFil.dHoseiP_s1.Min
					End If
                Case 7
					If Not IsNothing(dFil.dHoseiP_s2)
        				dAryMax(j) = dFil.dHoseiP_s2.Max
        				dAryMin(j) = dFil.dHoseiP_s2.Min
					End If
				End Select
			Next
			dHisMax = dAryMax.Max
			dHisMin = If(dAryMin.Min<0,0,dAryMin.Min)
 			Dim dMaxH As Double = 0.0
			Select Case dHisMax
			Case <0.1
				dMaxH = 0.1
			Case <1
				dMaxH = 1
			Case <10
				dMaxH = 10
			Case Else
				dMaxH = Math.Ceiling(dHisMax/10) * 10
			End Select
			dHisMax = dMaxH
		End if
		If dHisMax = dHisMin Or Double.IsNaN(dHisMin)
			dGMax = dHisMax
			dGMin = 0
		ElseIf dHisMax = 0 
			dGMax = 0.1
			dGMin = 0
		ElseIf dHisMax - dHisMin <> 0
			dGMax = dHisMax
			dGMin = dHisMin
		ElseIf Double.IsInfinity(dHisMax)
			dGMax = dHisMax
			dGMin = dHisMin
		End If
		If cts.iRTYAxis = 1
			.ChartAreas(0).AxisY.Interval = 10.0 * (arg.dYSpanPer / 100.0)
		Else
			.ChartAreas(0).AxisY.Interval = 0
		End If
		.ChartAreas(0).AxisY.LabelStyle.Format			= "F2"

	Case "rbViscosity"
        If cts.iRTYAxis = 1
        'Y軸スパン 固定
			'dHisMax = 10000 * (arg.dYSpanPer / 100.0)
			'dHisMin = 0
			dHisMax = cts.MainGraphYMax(arg.iSelectGItem) * (arg.dYSpanPer / 100.0)
			dHisMin = cts.MainGraphYMin(arg.iSelectGItem)
        Else
            If Not IsNothing(dFil.dViscosity)
		        dAryMax(0) = dFil.dViscosity.Max
		        dAryMin(0) = dFil.dViscosity.Min
            Else
		        dAryMax(0) = 10000
		        dAryMin(0) = 0
            End If
			For j = 1 To iHis
				Select Case j
				Case 1
        			dAryMax(j) = dFil.dViscosity_h1.Max
        			dAryMin(j) = dFil.dViscosity_h1.Min
				Case 2
        			dAryMax(j) = dFil.dViscosity_h2.Max
        			dAryMin(j) = dFil.dViscosity_h2.Min
				Case 3
        			dAryMax(j) = dFil.dViscosity_h3.Max
        			dAryMin(j) = dFil.dViscosity_h3.Min
				Case 4
        			dAryMax(j) = dFil.dViscosity_h4.Max
        			dAryMin(j) = dFil.dViscosity_h4.Min
				Case 5
        			dAryMax(j) = dFil.dViscosity_h5.Max
        			dAryMin(j) = dFil.dViscosity_h5.Min
				End Select
			Next
			For j = iStdBase To iStdBase - 1 + iStd
				Select Case j
				Case 6
					If Not IsNothing(dFil.dViscosity_s1)
        				dAryMax(j) = dFil.dViscosity_s1.Max
        				dAryMin(j) = dFil.dViscosity_s1.Min
					End If
				Case 7
					If Not IsNothing(dFil.dViscosity_s2)
        				dAryMax(j) = dFil.dViscosity_s2.Max
        				dAryMin(j) = dFil.dViscosity_s2.Min
					End If
				End Select
			Next
			dHisMax = If(dAryMax.Max = 0, 10000, dAryMax.Max)
			dHisMin = If(dAryMin.Min<0,0,dAryMin.Min)
 			Dim dMaxH As Double = 0.0
			Select Case dHisMax
			Case <1
				dMaxH = 1
			Case <10
				dMaxH = 10
			Case <100
				dMaxH = 100
			Case Else
				dMaxH = Math.Ceiling(dHisMax/100) * 100
			End Select
			dHisMax = dMaxH
		End if
		If dHisMax = dHisMin Or Double.IsNaN(dHisMin)
			dGMax = dHisMax
			dGMin = 0
		ElseIf dHisMax = 0 
			dGMax = 1
			dGMin = 0
		ElseIf dHisMax - dHisMin <> 0
			dGMax = dHisMax
			dGMin = dHisMin
		End If

		If cts.iRTYAxis = 1
	    	.ChartAreas(0).AxisY.Interval = 1000.0 * (arg.dYSpanPer / 100.0)
		Else
    		.ChartAreas(0).AxisY.Interval = 0
		End If
		Select Case dHisMax
		Case <100
			.ChartAreas(0).AxisY.LabelStyle.Format			= "F1"
		Case Else
			.ChartAreas(0).AxisY.LabelStyle.Format			= "F0"
		End Select

	Case "rbPosPhase"
        If cts.iRTYAxis = 1
        'Y軸スパン 固定
            'dHisMax = 90.0 * (arg.dYSpanPer / 100.0)
            'dHisMin = 0
			dHisMax = cts.MainGraphYMax(arg.iSelectGItem) * (arg.dYSpanPer / 100.0)
			dHisMin = cts.MainGraphYMin(arg.iSelectGItem)
        Else
            If Not IsNothing(dFil.dXPosPhase)
		        dAryMax(0) = dFil.dXPosPhase.Max * RadToDegRate
		        ''dAryMin(0) = dFil.dXPosPhase.Min
		        dAryMin(0) = 0
            Else
		        dAryMax(0) = 90.0
		        dAryMin(0) = 0
            End If
			For j = 1 To iHis
				Select Case j
				Case 1
        			dAryMax(j) = dFil.dXPosPhase_h1.Max * RadToDegRate
        			dAryMin(j) = dFil.dXPosPhase_h1.Min * RadToDegRate
				Case 2
        			dAryMax(j) = dFil.dXPosPhase_h2.Max * RadToDegRate
        			dAryMin(j) = dFil.dXPosPhase_h2.Min * RadToDegRate
				Case 3
        			dAryMax(j) = dFil.dXPosPhase_h3.Max * RadToDegRate
        			dAryMin(j) = dFil.dXPosPhase_h3.Min * RadToDegRate
				Case 4
        			dAryMax(j) = dFil.dXPosPhase_h4.Max * RadToDegRate
        			dAryMin(j) = dFil.dXPosPhase_h4.Min * RadToDegRate
				Case 5
        			dAryMax(j) = dFil.dXPosPhase_h5.Max * RadToDegRate
        			dAryMin(j) = dFil.dXPosPhase_h5.Min * RadToDegRate
				End Select
			Next
			For j = iStdBase To iStdBase - 1 + iStd
				Select Case j
				Case 6
 					If Not IsNothing(dFil.dXPosPhase_s1)
		   				dAryMax(j) = dFil.dXPosPhase_s1.Max * RadToDegRate
        				dAryMin(j) = dFil.dXPosPhase_s1.Min * RadToDegRate
					End If
				Case 7
 					If Not IsNothing(dFil.dXPosPhase_s2)
	       				dAryMax(j) = dFil.dXPosPhase_s2.Max * RadToDegRate
        				dAryMin(j) = dFil.dXPosPhase_s2.Min * RadToDegRate
					End If
				End Select
			Next
			dHisMax = dAryMax.Max
			dHisMin = If(dAryMin.Min<0,0,dAryMin.Min)

 			Dim dMaxH As Double = 0.0
			If dHisMax < 1
				dMaxH = 1
			Else
				dMaxH = Math.Ceiling(dHisMax/10) * 10
			End If
			dHisMax = dMaxH
		End if
		If dHisMax = dHisMin Or Double.IsNaN(dHisMin)
			dGMax = dHisMax
			dGMin = 0
		ElseIf dHisMax = 0 
			dGMax = 1
			dGMin = 0
		ElseIf dHisMax - dHisMin <> 0
			dGMax = dHisMax
			dGMin = dHisMin
		End If
		If cts.iRTYAxis = 1
	    	.ChartAreas(0).AxisY.Interval = 10 * (arg.dYSpanPer / 100.0)
		Else
    		.ChartAreas(0).AxisY.Interval = If(iInterval<>-1, iInterval, 0)
		End If
		Select Case dHisMax
		Case <1
			.ChartAreas(0).AxisY.LabelStyle.Format			= "F2"
		Case <10
			.ChartAreas(0).AxisY.LabelStyle.Format			= "F1"
		Case Else
			.ChartAreas(0).AxisY.LabelStyle.Format			= "F0"
		End Select

	Case "rbSampleStrain"
        If cts.iRTYAxis = 1
        'Y軸スパン 固定
            'dHisMax = 10.0 * (arg.dYSpanPer / 100.0)
            'dHisMin = 0
			dHisMax = cts.MainGraphYMax(arg.iSelectGItem) * (arg.dYSpanPer / 100.0)
			dHisMin = cts.MainGraphYMin(arg.iSelectGItem)
        Else
            If Not IsNothing(dFil.dSplStrain)
		        dAryMax(0) = dFil.dSplStrain.Max
		        dAryMin(0) = dFil.dSplStrain.Min
            Else
		        dAryMax(0) = 0.5
		        dAryMin(0) = 0
            End If
			For j = 1 To iHis
				Select Case j
				Case 1
        			dAryMax(j) = dFil.dSplStrain_h1.Max
        			dAryMin(j) = dFil.dSplStrain_h1.Min
				Case 2
        			dAryMax(j) = dFil.dSplStrain_h2.Max
        			dAryMin(j) = dFil.dSplStrain_h2.Min
				Case 3
        			dAryMax(j) = dFil.dSplStrain_h3.Max
        			dAryMin(j) = dFil.dSplStrain_h3.Min
				Case 4
        			dAryMax(j) = dFil.dSplStrain_h4.Max
        			dAryMin(j) = dFil.dSplStrain_h4.Min
				Case 5
        			dAryMax(j) = dFil.dSplStrain_h5.Max
        			dAryMin(j) = dFil.dSplStrain_h5.Min
				End Select
			Next
			For j = iStdBase To iStdBase - 1 + iStd
				Select Case j
				Case 6
  					If Not IsNothing(dFil.dSplStrain_s1)
	       				dAryMax(j) = dFil.dSplStrain_s1.Max
        				dAryMin(j) = dFil.dSplStrain_s1.Min
					End If
				Case 7
 					If Not IsNothing(dFil.dSplStrain_s2)
        				dAryMax(j) = dFil.dSplStrain_s2.Max
        				dAryMin(j) = dFil.dSplStrain_s2.Min
					End If
				End Select
			Next
			dHisMax = dAryMax.Max
			dHisMin = If(dAryMin.Min<0,0,dAryMin.Min)
 			Dim dMaxH As Double = 0.0
			Select Case dHisMax
			Case < 0.01
				dMaxH = 0.01
			Case < 0.1
				dMaxH = 0.1
			Case < 1
				dMaxH = Math.Ceiling(dHisMax/0.1) * 0.1
			Case Else
				dMaxH = Math.Ceiling(dHisMax/2) * 2
			End Select
			dHisMax = dMaxH
		End if
		If dHisMax = dHisMin Or Double.IsNaN(dHisMin)
			dGMax = dHisMax
			dGMin = 0
		ElseIf dHisMax = 0 
			dGMax = 1
			dGMin = 0
		ElseIf dHisMax - dHisMin <> 0
			dGMax = dHisMax
			dGMin = dHisMin
		End If

		If cts.iRTYAxis = 1
	    	.ChartAreas(0).AxisY.Interval = 1 * (arg.dYSpanPer / 100.0)
		Else
    		.ChartAreas(0).AxisY.Interval = 0
		End If
		Select Case dHisMax
		Case <= 0.01
			.ChartAreas(0).AxisY.LabelStyle.Format			= "F3"
		Case <= 0.1
			.ChartAreas(0).AxisY.LabelStyle.Format			= "F2"
		Case Else
			.ChartAreas(0).AxisY.LabelStyle.Format			= "F1"
		End Select

	Case "rbShearMod"
        If cts.iRTYAxis = 1
        'Y軸スパン 固定
            'dHisMax = 100000 * (arg.dYSpanPer / 100.0)
            'dHisMin = 0
			dHisMax = cts.MainGraphYMax(arg.iSelectGItem) * (arg.dYSpanPer / 100.0)
			dHisMin = cts.MainGraphYMin(arg.iSelectGItem)
        Else
            If Not IsNothing(dFil.dShearMod)
		        dAryMax(0) = dFil.dShearMod.Max
		        dAryMin(0) = dFil.dShearMod.Min
            Else
		        dAryMax(0) = 10000
		        dAryMin(0) = 0
            End If
			For j = 1 To iHis
				Select Case j
				Case 1
        			dAryMax(j) = dFil.dShearMod_h1.Max
        			dAryMin(j) = dFil.dShearMod_h1.Min
				Case 2
        			dAryMax(j) = dFil.dShearMod_h2.Max
        			dAryMin(j) = dFil.dShearMod_h2.Min
				Case 3
        			dAryMax(j) = dFil.dShearMod_h3.Max
        			dAryMin(j) = dFil.dShearMod_h3.Min
				Case 4
        			dAryMax(j) = dFil.dShearMod_h4.Max
        			dAryMin(j) = dFil.dShearMod_h4.Min
				Case 5
        			dAryMax(j) = dFil.dShearMod_h5.Max
        			dAryMin(j) = dFil.dShearMod_h5.Min
				End Select
			Next
			For j = iStdBase To iStdBase - 1 + iStd
				Select Case j
				Case 6
  					If Not IsNothing(dFil.dShearMod_s1)
        				dAryMax(j) = dFil.dShearMod_s1.Max
        				dAryMin(j) = dFil.dShearMod_s1.Min
					End If
				Case 7
  					If Not IsNothing(dFil.dShearMod_s2)
        				dAryMax(j) = dFil.dShearMod_s2.Max
        				dAryMin(j) = dFil.dShearMod_s2.Min
					End If
				End Select
			Next
			dHisMax = dAryMax.Max
			dHisMin = If(dAryMin.Min<0,0,dAryMin.Min)
 			Dim dMaxH As Double = 0.0
			Select Case dHisMax
			Case < 100
				dMaxH = 100
			Case < 1000
				dMaxH = 1000
			Case < 10000
				dMaxH = 10000
			Case Else
				dMaxH = Math.Ceiling(dHisMax/10000) * 10000
			End Select
			dHisMax = dMaxH
		End if
		If dHisMax = dHisMin Or Double.IsNaN(dHisMin)
			dGMax = dHisMax
			dGMin = 0
		ElseIf dHisMax = 0 
			dGMax = 1
			dGMin = 0
		ElseIf dHisMax - dHisMin <> 0
			dGMax = dHisMax
			dGMin = dHisMin
		End If

		If cts.iRTYAxis = 1
	    	.ChartAreas(0).AxisY.Interval = 10000 * (arg.dYSpanPer / 100.0)
		Else
    		.ChartAreas(0).AxisY.Interval = 0
		End If
		Select Case dHisMax
		Case <= 100
			.ChartAreas(0).AxisY.LabelStyle.Format			= "F2"
		Case <= 1000
			.ChartAreas(0).AxisY.LabelStyle.Format			= "F1"
		Case Else
			.ChartAreas(0).AxisY.LabelStyle.Format			= "F0"
		End Select

	Case "rbShearVelo"
        If cts.iRTYAxis = 1
        'Y軸スパン 固定
            'dHisMax = 100 * (arg.dYSpanPer / 100.0)
            'dHisMin = 0
			dHisMax = cts.MainGraphYMax(arg.iSelectGItem) * (arg.dYSpanPer / 100.0)
			dHisMin = cts.MainGraphYMin(arg.iSelectGItem)
        Else
            If Not IsNothing(dFil.dShearVelo)
		        dAryMax(0) = dFil.dShearVelo.Max
		        dAryMin(0) = dFil.dShearVelo.Min
            Else
		        dAryMax(0) = 10
		        dAryMin(0) = 0
            End If
			For j = 1 To iHis
				Select Case j
				Case 1
        			dAryMax(j) = dFil.dShearVelo_h1.Max
        			dAryMin(j) = dFil.dShearVelo_h1.Min
				Case 2
        			dAryMax(j) = dFil.dShearVelo_h2.Max
        			dAryMin(j) = dFil.dShearVelo_h2.Min
				Case 3
        			dAryMax(j) = dFil.dShearVelo_h3.Max
        			dAryMin(j) = dFil.dShearVelo_h3.Min
				Case 4
        			dAryMax(j) = dFil.dShearVelo_h4.Max
        			dAryMin(j) = dFil.dShearVelo_h4.Min
				Case 5
        			dAryMax(j) = dFil.dShearVelo_h5.Max
        			dAryMin(j) = dFil.dShearVelo_h5.Min
				End Select
			Next
			For j = iStdBase To iStdBase - 1 + iStd
				Select Case j
				Case 6
  					If Not IsNothing(dFil.dShearVelo_s1)
        				dAryMax(j) = dFil.dShearVelo_s1.Max
        				dAryMin(j) = dFil.dShearVelo_s1.Min
					End If
				Case 7
   					If Not IsNothing(dFil.dShearVelo_s2)
       					dAryMax(j) = dFil.dShearVelo_s2.Max
        				dAryMin(j) = dFil.dShearVelo_s2.Min
					End If
				End Select
			Next
			dHisMax = dAryMax.Max
			dHisMin = If(dAryMin.Min<0,0,dAryMin.Min)
 			Dim dMaxH As Double = 0.0
			Select Case dHisMax
			Case < 1
				dMaxH = 1
			Case < 10
				dMaxH = 10
			Case < 100
				dMaxH = Math.Ceiling(dHisMax/10) * 10
			Case Else
				dMaxH = Math.Ceiling(dHisMax/100) * 100
			End Select
			dHisMax = dMaxH
		End if
		If dHisMax = dHisMin Or Double.IsNaN(dHisMin)
			dGMax = dHisMax
			dGMin = 0
		ElseIf dHisMax = 0 
			dGMax = 1
			dGMin = 0
		ElseIf dHisMax - dHisMin <> 0
			dGMax = dHisMax
			dGMin = dHisMin
		End If

		If cts.iRTYAxis = 1
	    	.ChartAreas(0).AxisY.Interval = 10 * (arg.dYSpanPer / 100.0)
		Else
    		.ChartAreas(0).AxisY.Interval = 0
		End If
		Select Case dHisMax
		Case <= 100
			.ChartAreas(0).AxisY.LabelStyle.Format			= "F2"
		Case <= 1000
			.ChartAreas(0).AxisY.LabelStyle.Format			= "F1"
		Case Else
			.ChartAreas(0).AxisY.LabelStyle.Format			= "F0"
		End Select

	Case "rbStorageMod"
        If cts.iRTYAxis = 1
        'Y軸スパン 固定
            'dHisMax = 100000 * (arg.dYSpanPer / 100.0)
            'dHisMin = 0
			dHisMax = cts.MainGraphYMax(arg.iSelectGItem) * (arg.dYSpanPer / 100.0)
			dHisMin = cts.MainGraphYMin(arg.iSelectGItem)
        Else
            If Not IsNothing(dFil.dXStrgMod)
		        dAryMax(0) = dFil.dXStrgMod.Max
		        dAryMin(0) = dFil.dXStrgMod.Min
            Else
		        dAryMax(0) = 10000
		        dAryMin(0) = 0
            End If
			For j = 1 To iHis
				Select Case j
				Case 1
        			dAryMax(j) = dFil.dXStrgMod_h1.Max
        			dAryMin(j) = dFil.dXStrgMod_h1.Min
				Case 2
        			dAryMax(j) = dFil.dXStrgMod_h2.Max
        			dAryMin(j) = dFil.dXStrgMod_h2.Min
				Case 3
        			dAryMax(j) = dFil.dXStrgMod_h3.Max
        			dAryMin(j) = dFil.dXStrgMod_h3.Min
				Case 4
        			dAryMax(j) = dFil.dXStrgMod_h4.Max
        			dAryMin(j) = dFil.dXStrgMod_h4.Min
				Case 5
        			dAryMax(j) = dFil.dXStrgMod_h5.Max
        			dAryMin(j) = dFil.dXStrgMod_h5.Min
				End Select
			Next
			For j = iStdBase To iStdBase - 1 + iStd
				Select Case j
				Case 6
  					If Not IsNothing(dFil.dXStrgMod_s1)
        				dAryMax(j) = dFil.dXStrgMod_s1.Max
        				dAryMin(j) = dFil.dXStrgMod_s1.Min
					End If
				Case 7
  					If Not IsNothing(dFil.dXStrgMod_s2)
        				dAryMax(j) = dFil.dXStrgMod_s2.Max
        				dAryMin(j) = dFil.dXStrgMod_s2.Min
					End If
				End Select
			Next
			dHisMax = dAryMax.Max
			dHisMin = If(dAryMin.Min<0,0,dAryMin.Min)
 			Dim dMaxH As Double = 0.0
			Select Case dHisMax
			Case < 100
				dMaxH = 100
			Case < 1000
				dMaxH = 1000
			Case < 10000
				dMaxH = 10000
			Case Else
				dMaxH = Math.Ceiling(dHisMax/10000) * 10000
			End Select
			dHisMax = dMaxH
		End if
		If dHisMax = dHisMin Or Double.IsNaN(dHisMin)
			dGMax = dHisMax
			dGMin = 0
		ElseIf dHisMax = 0 
			dGMax = 1
			dGMin = 0
		ElseIf dHisMax - dHisMin <> 0
			dGMax = dHisMax
			dGMin = dHisMin
		End If

		If cts.iRTYAxis = 1
	    	.ChartAreas(0).AxisY.Interval = 10000 * (arg.dYSpanPer / 100.0)
		Else
    		.ChartAreas(0).AxisY.Interval = 0
		End If
		Select Case dHisMax
		Case <= 100
			.ChartAreas(0).AxisY.LabelStyle.Format			= "F2"
		Case <= 1000
			.ChartAreas(0).AxisY.LabelStyle.Format			= "F1"
		Case Else
			.ChartAreas(0).AxisY.LabelStyle.Format			= "F0"
		End Select

	Case "rbElasticMod"
        If cts.iRTYAxis = 1
        'Y軸スパン 固定
            'dHisMax = 100000 * (arg.dYSpanPer / 100.0)
            'dHisMin = 0
			dHisMax = cts.MainGraphYMax(arg.iSelectGItem) * (arg.dYSpanPer / 100.0)
			dHisMin = cts.MainGraphYMin(arg.iSelectGItem)
        Else
            If Not IsNothing(dFil.dXElasMod)
		        dAryMax(0) = dFil.dXElasMod.Max
		        dAryMin(0) = dFil.dXElasMod.Min
            Else
		        dAryMax(0) = 5000
		        dAryMin(0) = 0
            End If
			For j = 1 To iHis
				Select Case j
				Case 1
        			dAryMax(j) = dFil.dXElasMod_h1.Max
        			dAryMin(j) = dFil.dXElasMod_h1.Min
				Case 2
        			dAryMax(j) = dFil.dXElasMod_h2.Max
        			dAryMin(j) = dFil.dXElasMod_h2.Min
				Case 3
        			dAryMax(j) = dFil.dXElasMod_h3.Max
        			dAryMin(j) = dFil.dXElasMod_h3.Min
				Case 4
        			dAryMax(j) = dFil.dXElasMod_h4.Max
        			dAryMin(j) = dFil.dXElasMod_h4.Min
				Case 5
        			dAryMax(j) = dFil.dXElasMod_h5.Max
        			dAryMin(j) = dFil.dXElasMod_h5.Min
				End Select
			Next
			For j = iStdBase To iStdBase - 1 + iStd
				Select Case j
				Case 6
  					If Not IsNothing(dFil.dXElasMod_s1)
        				dAryMax(j) = dFil.dXElasMod_s1.Max
        				dAryMin(j) = dFil.dXElasMod_s1.Min
					End If
				Case 7
   					If Not IsNothing(dFil.dXElasMod_s2)
       					dAryMax(j) = dFil.dXElasMod_s2.Max
        				dAryMin(j) = dFil.dXElasMod_s2.Min
					End If
				End Select
			Next
			dHisMax = dAryMax.Max
			dHisMin = If(dAryMin.Min<0,0,dAryMin.Min)
 			Dim dMaxH As Double = 0.0
			Select Case dHisMax
			Case < 100
				dMaxH = 100
			Case < 1000
				dMaxH = 1000
			Case < 10000
				dMaxH = 10000
			Case Else
				dMaxH = Math.Ceiling(dHisMax/10000) * 10000
			End Select
			dHisMax = dMaxH
		End if
		If dHisMax = dHisMin Or Double.IsNaN(dHisMin)
			dGMax = dHisMax
			dGMin = 0
		ElseIf dHisMax = 0 
			dGMax = 1
			dGMin = 0
		ElseIf dHisMax - dHisMin <> 0
			dGMax = dHisMax
			dGMin = dHisMin
		End If

		If cts.iRTYAxis = 1
	    	.ChartAreas(0).AxisY.Interval = 10000 * (arg.dYSpanPer / 100.0)
		Else
    		.ChartAreas(0).AxisY.Interval = 0
		End If
		Select Case dHisMax
		Case <= 100
			.ChartAreas(0).AxisY.LabelStyle.Format			= "F2"
		Case <= 1000
			.ChartAreas(0).AxisY.LabelStyle.Format			= "F1"
		Case Else
			.ChartAreas(0).AxisY.LabelStyle.Format			= "F0"
		End Select

	Case "rbLossTangent"
        If cts.iRTYAxis = 1
        'Y軸スパン 固定
            'dHisMax = 100 * (arg.dYSpanPer / 100.0)
            'dHisMin = 0
			dHisMax = cts.MainGraphYMax(arg.iSelectGItem) * (arg.dYSpanPer / 100.0)
			dHisMin = cts.MainGraphYMin(arg.iSelectGItem)
        Else
            If Not IsNothing(dFil.dXLossTan)
		        dAryMax(0) = dFil.dXLossTan.Max
		        dAryMin(0) = dFil.dXLossTan.Min
            Else
		        dAryMax(0) = 10
		        dAryMin(0) = 0
            End If
			For j = 1 To iHis
				Select Case j
				Case 1
        			dAryMax(j) = dFil.dXLossTan_h1.Max
        			dAryMin(j) = dFil.dXLossTan_h1.Min
				Case 2
        			dAryMax(j) = dFil.dXLossTan_h2.Max
        			dAryMin(j) = dFil.dXLossTan_h2.Min
				Case 3
        			dAryMax(j) = dFil.dXLossTan_h3.Max
        			dAryMin(j) = dFil.dXLossTan_h3.Min
				Case 4
        			dAryMax(j) = dFil.dXLossTan_h4.Max
        			dAryMin(j) = dFil.dXLossTan_h4.Min
				Case 5
        			dAryMax(j) = dFil.dXLossTan_h5.Max
        			dAryMin(j) = dFil.dXLossTan_h5.Min
				End Select
			Next
			For j = iStdBase To iStdBase - 1 + iStd
				Select Case j
				Case 6
  					If Not IsNothing(dFil.dXLossTan_s1)
        				dAryMax(j) = dFil.dXLossTan_s1.Max
        				dAryMin(j) = dFil.dXLossTan_s1.Min
					End If
				Case 7
   					If Not IsNothing(dFil.dXLossTan_s2)
       					dAryMax(j) = dFil.dXLossTan_s2.Max
        				dAryMin(j) = dFil.dXLossTan_s2.Min
					End If
				End Select
			Next
			dHisMax = dAryMax.Max
			dHisMin = If(dAryMin.Min<0,0,dAryMin.Min)
 			Dim dMaxH As Double = 0.0
			Select Case dHisMax
			Case < 1
				dMaxH = 1
			Case < 10
				dMaxH = 10
			Case < 100
				dMaxH = Math.Ceiling(dHisMax/10) * 10
			Case Else
				dMaxH = Math.Ceiling(dHisMax/100) * 100
			End Select
			dHisMax = dMaxH
		End if
		If dHisMax = dHisMin Or Double.IsNaN(dHisMin)
			dGMax = dHisMax
			dGMin = 0
		ElseIf dHisMax = 0 
			dGMax = 1
			dGMin = 0
		ElseIf dHisMax - dHisMin <> 0
			dGMax = dHisMax
			dGMin = dHisMin
		End If

		If cts.iRTYAxis = 1
	    	.ChartAreas(0).AxisY.Interval = 10 * (arg.dYSpanPer / 100.0)
		Else
    		.ChartAreas(0).AxisY.Interval = 0
		End If
		Select Case dHisMax
		Case <= 100
			.ChartAreas(0).AxisY.LabelStyle.Format			= "F2"
		Case <= 1000
			.ChartAreas(0).AxisY.LabelStyle.Format			= "F1"
		Case Else
			.ChartAreas(0).AxisY.LabelStyle.Format			= "F0"
		End Select

	Case "rbHPTemp"
         If cts.iRTYAxis = 1
        'Y軸スパン 固定
            'dHisMax = 2000 * (arg.dYSpanPer / 100.0)
            'dHisMin = 0
			dHisMax = cts.MainGraphYMax(arg.iSelectGItem) * 10 * (arg.dYSpanPer / 100.0)
			dHisMin = cts.MainGraphYMin(arg.iSelectGItem) * 10
        Else
		   If Not IsNothing(dFil.iTempBuf)
				iAryMax(0) = dFil.iTempBuf.Max
				iAryMin(0) = dFil.iTempBuf.Min
			Else
				iAryMax(0) = 300.0
				iAryMin(0) = 0.0
			End If
			For j = 1 To iHis
				Select Case j
				Case 1
        			iAryMax(j) = dFil.iTempBuf_h1.Max
        			iAryMin(j) = dFil.iTempBuf_h1.Min
				Case 2
        			iAryMax(j) = dFil.iTempBuf_h2.Max
        			iAryMin(j) = dFil.iTempBuf_h2.Min
				Case 3
        			iAryMax(j) = dFil.iTempBuf_h3.Max
        			iAryMin(j) = dFil.iTempBuf_h3.Min
				Case 4
        			iAryMax(j) = dFil.iTempBuf_h4.Max
        			iAryMin(j) = dFil.iTempBuf_h4.Min
				Case 5
        			iAryMax(j) = dFil.iTempBuf_h5.Max
        			iAryMin(j) = dFil.iTempBuf_h5.Min
				End Select
			Next
			For j = iStdBase To iStdBase - 1 + iStd
				Select Case j
				Case 6
  					If Not IsNothing(dFil.iTempBuf_s1)
        				iAryMax(j) = dFil.iTempBuf_s1.Max
        				iAryMin(j) = dFil.iTempBuf_s1.Min
					End If
				Case 7
   					If Not IsNothing(dFil.iTempBuf_s2)
       					iAryMax(j) = dFil.iTempBuf_s2.Max
        				iAryMin(j) = dFil.iTempBuf_s2.Min
					End If
				End Select
			Next
			dHisMax = iAryMax.Max
			dHisMin = iAryMin.Min
 			Dim dMaxH As Double = 0.0
 			Dim dMinH As Double = 0.0
			dMaxH = Math.Ceiling(dHisMax/100) * 100
			dMinH = Math.Floor(dHisMin/100) * 100
			dHisMax = dMaxH
			dHisMin = dMinH
		End if
		If dHisMax = dHisMin Or Double.IsNaN(dHisMin)
			dGMax = dHisMax / 10.0
			dGMin = 0
		ElseIf dHisMax = 0 
			dGMax = 200
			dGMin = 0
		ElseIf dHisMax - dHisMin <> 0
			dGMax = dHisMax / 10.0
			dGMin = dHisMin / 10.0
		End If
		.ChartAreas(0).AxisY.LabelStyle.Format			= "F1"

	Case "rbPBTemp"
          If cts.iRTYAxis = 1
        'Y軸スパン 固定
            'dHisMax = 2000 * (arg.dYSpanPer / 100.0)
            'dHisMin = 0
			dHisMax = cts.MainGraphYMax(arg.iSelectGItem) * 10 * (arg.dYSpanPer / 100.0)
			dHisMin = cts.MainGraphYMin(arg.iSelectGItem) * 10
        Else
		   If Not IsNothing(dFil.iTempBuf2)
				iAryMax(0) = dFil.iTempBuf2.Max
				iAryMin(0) = dFil.iTempBuf2.Min
			Else
				iAryMax(0) = 300.0
				iAryMin(0) = 0.0
			End If
			For j = 1 To iHis
				Select Case j
				Case 1
        			iAryMax(j) = dFil.iTempBuf2_h1.Max
        			iAryMin(j) = dFil.iTempBuf2_h1.Min
				Case 2
        			iAryMax(j) = dFil.iTempBuf2_h2.Max
        			iAryMin(j) = dFil.iTempBuf2_h2.Min
				Case 3
        			iAryMax(j) = dFil.iTempBuf2_h3.Max
        			iAryMin(j) = dFil.iTempBuf2_h3.Min
				Case 4
        			iAryMax(j) = dFil.iTempBuf2_h4.Max
        			iAryMin(j) = dFil.iTempBuf2_h4.Min
				Case 5
        			iAryMax(j) = dFil.iTempBuf2_h5.Max
        			iAryMin(j) = dFil.iTempBuf2_h5.Min
				End Select
			Next
			For j = iStdBase To iStdBase - 1 + iStd
				Select Case j
				Case 6
  					If Not IsNothing(dFil.iTempBuf2_s1)
        				iAryMax(j) = dFil.iTempBuf2_s1.Max
        				iAryMin(j) = dFil.iTempBuf2_s1.Min
					End If
				Case 7
  					If Not IsNothing(dFil.iTempBuf2_s2)
        				iAryMax(j) = dFil.iTempBuf2_s2.Max
        				iAryMin(j) = dFil.iTempBuf2_s2.Min
					End If
				End Select
			Next
			dHisMax = iAryMax.Max
			dHisMin = iAryMin.Min
 			Dim dMaxH As Double = 0.0
 			Dim dMinH As Double = 0.0
			dMaxH = Math.Ceiling(dHisMax/100) * 100
			dMinH = Math.Floor(dHisMin/100) * 100
			dHisMax = dMaxH
			dHisMin = dMinH
		End if
		If dHisMax = dHisMin Or Double.IsNaN(dHisMin)
			dGMax = dHisMax / 10.0
			dGMin = 0
		ElseIf dHisMax = 0 
			dGMax = 200
			dGMin = 0
		ElseIf dHisMax - dHisMin <> 0
			dGMax = dHisMax / 10.0
			dGMin = dHisMin / 10.0
		End If
		.ChartAreas(0).AxisY.LabelStyle.Format			= "F1"

	Case "rbHPMV"
         If cts.iRTYAxis = 1
        'Y軸スパン 固定
            'dHisMax = 1000 * (arg.dYSpanPer / 100.0)
            'dHisMin = 0
			dHisMax = cts.MainGraphYMax(arg.iSelectGItem) * 10 * (arg.dYSpanPer / 100.0)
			dHisMin = cts.MainGraphYMin(arg.iSelectGItem) * 10
        Else
			If Not IsNothing(dFil.iMvBuf)
				iAryMax(0) = dFil.iMvBuf.Max
				iAryMin(0) = dFil.iMvBuf.Min
			Else
				iAryMax(0) = 1000
				iAryMin(0) = 0
			End If
			For j = 1 To iHis
				Select Case j
				Case 1
        			iAryMax(j) = dFil.iMvBuf_h1.Max
        			iAryMin(j) = dFil.iMvBuf_h1.Min
				Case 2
        			iAryMax(j) = dFil.iMvBuf_h2.Max
        			iAryMin(j) = dFil.iMvBuf_h2.Min
				Case 3
        			iAryMax(j) = dFil.iMvBuf_h3.Max
        			iAryMin(j) = dFil.iMvBuf_h3.Min
				Case 4
        			iAryMax(j) = dFil.iMvBuf_h4.Max
        			iAryMin(j) = dFil.iMvBuf_h4.Min
				Case 5
        			iAryMax(j) = dFil.iMvBuf_h5.Max
        			iAryMin(j) = dFil.iMvBuf_h5.Min
				End Select
			Next
			For j = iStdBase To iStdBase - 1 + iStd
				Select Case j
				Case 6
  					If Not IsNothing(dFil.iMvBuf_s1)
        				iAryMax(j) = dFil.iMvBuf_s1.Max
        				iAryMin(j) = dFil.iMvBuf_s1.Min
					End If
				Case 7
  					If Not IsNothing(dFil.iMvBuf_s2)
        				iAryMax(j) = dFil.iMvBuf_s2.Max
        				iAryMin(j) = dFil.iMvBuf_s2.Min
					End If
				End Select
			Next
			dHisMax = iAryMax.Max
			dHisMin = iAryMin.Min
 			Dim dMaxH As Double = 0.0
 			Dim dMinH As Double = 0.0
			dMaxH = Math.Ceiling(dHisMax/10) * 10
			dMinH = Math.Floor(dHisMin/10) * 10
			dHisMax = dMaxH
			dHisMin = dMinH
		End if
		If dHisMax = dHisMin Or Double.IsNaN(dHisMin)
			dGMax = dHisMax / 10.0
			dGMin = 0
		ElseIf dHisMax = 0 
			dGMax = 200
			dGMin = 0
		ElseIf dHisMax - dHisMin <> 0
			dGMax = dHisMax / 10.0
			dGMin = dHisMin / 10.0
		End If
		.ChartAreas(0).AxisY.LabelStyle.Format			= "F1"

	Case "rbPBMV"
         If cts.iRTYAxis = 1
        'Y軸スパン 固定
            'dHisMax = 1000 * (arg.dYSpanPer / 100.0)
            'dHisMin = 0
			dHisMax = cts.MainGraphYMax(arg.iSelectGItem) * 10 * (arg.dYSpanPer / 100.0)
			dHisMin = cts.MainGraphYMin(arg.iSelectGItem) * 10
        Else
			If Not IsNothing(dFil.iMvBuf2)
				iAryMax(0) = dFil.iMvBuf2.Max
				iAryMin(0) = dFil.iMvBuf2.Min
			Else
				iAryMax(0) = 100
				iAryMin(0) = 0
			End If
			For j = 1 To iHis
				Select Case j
				Case 1
        			iAryMax(j) = dFil.iMvBuf2_h1.Max
        			iAryMin(j) = dFil.iMvBuf2_h1.Min
				Case 2
        			iAryMax(j) = dFil.iMvBuf2_h2.Max
        			iAryMin(j) = dFil.iMvBuf2_h2.Min
				Case 3
        			iAryMax(j) = dFil.iMvBuf2_h3.Max
        			iAryMin(j) = dFil.iMvBuf2_h3.Min
				Case 4
        			iAryMax(j) = dFil.iMvBuf2_h4.Max
        			iAryMin(j) = dFil.iMvBuf2_h4.Min
				Case 5
        			iAryMax(j) = dFil.iMvBuf2_h5.Max
        			iAryMin(j) = dFil.iMvBuf2_h5.Min
				End Select
			Next
			For j = iStdBase To iStdBase - 1 + iStd
				Select Case j
				Case 6
  					If Not IsNothing(dFil.iMvBuf2_s1)
        				iAryMax(j) = dFil.iMvBuf2_s1.Max
        				iAryMin(j) = dFil.iMvBuf2_s1.Min
					End If
				Case 7
  					If Not IsNothing(dFil.iMvBuf2_s2)
        				iAryMax(j) = dFil.iMvBuf2_s2.Max
        				iAryMin(j) = dFil.iMvBuf2_s2.Min
					End If
				End Select
			Next
			dHisMax = iAryMax.Max
			dHisMin = iAryMin.Min
 			Dim dMaxH As Double = 0.0
 			Dim dMinH As Double = 0.0
			dMaxH = Math.Ceiling(dHisMax/10) * 10
			dMinH = Math.Floor(dHisMin/10) * 10
			dHisMax = dMaxH
			dHisMin = dMinH
		End if
		If dHisMax = dHisMin Or Double.IsNaN(dHisMin)
			dGMax = dHisMax / 10.0
			dGMin = 0
		ElseIf dHisMax = 0 
			dGMax = 200
			dGMin = 0
		ElseIf dHisMax - dHisMin <> 0
			dGMax = dHisMax / 10.0
			dGMin = dHisMin / 10.0
		End If

		.ChartAreas(0).AxisY.LabelStyle.Format			= "F1"
	
	End Select
    End With
Catch ex As Exception
    Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
    Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub
''' <summary>
''' 指定範囲(秒)内測定データ配列の最大最小を求める
''' </summary>
''' <param name="dArray">測定データ配列 Double型</param>
''' <param name="iSRange">範囲開始(秒)</param>
''' <param name="iERange">範囲終了(秒)</param>
''' <param name="dGetMax">最大値</param>
''' <param name="dGetMin">最小値</param>
''' <param name="iSampint">サンプルインターバル[mS]</param>
''' <returns></returns>
Public Function GetMaxMin1ArrayRange(ByRef dArray() As Double, ByVal iSRange As Int32, ByVal iERange As Int32, ByRef dGetMax As Double, Optional ByRef dGetMin As Double = 0, Optional ByRef iSampint As Int32 = 2 ) As Int32
Dim iArrayRange() As Double = Nothing
Dim iStIdx	As Int64
Dim iEdIdx	As Int64
Dim iDNum	As Int32 = 1000 / iSampint
Try
	If IsNothing(dArray)
		Return -1
	ElseIf iSRange = 0 And iERange = 0
		dGetMax = CType(dArray.Max, Double)
		dGetMin = CType(dArray.Min, Double)
		Return 0
	ElseIf iSRange >= iERange
		Return -1
	Else
		'iStIdx = iSRange * MEAS_BLOCK_DATA_NUM
		'iEdIdx = If((iERange * MEAS_BLOCK_DATA_NUM)>dArray.Length, dArray.Length, iERange * MEAS_BLOCK_DATA_NUM)
		iStIdx = iSRange * iDNum
		iEdIdx = If((iERange * iDNum)>dArray.Length, dArray.Length, iERange * iDNum)
		If iStIdx >= iEdIdx
			Return -1
		Else
			Array.Resize(iArrayRange, (iEdIdx - iStIdx))
			Array.Copy(dArray, iStIdx, iArrayRange, 0, iArrayRange.Length)
			dGetMax = CType(iArrayRange.Max, Double)
			dGetMin = If( Double.IsNaN(CType(iArrayRange.Min, Double)), 0 , CType(iArrayRange.Min, Double))
		End If
	End If
	Return 0
Catch ex As Exception
	Return -1
End Try
End Function
''' <summary>
''' Threshold変更に伴うゲルタイム表示その他更新
''' </summary>
''' <param name="arg"></param>
''' <param name="Chart1"></param>
''' <param name="clExt"></param>
''' <param name="sSelectGItem"></param>
''' <param name="cts"></param>
''' <param name="iThinTyp"></param>
Public Sub ChartUpdateGelTimeLine( _
		ByVal arg			As ChartArg, _
        ByRef Chart1        As Chart, _
        ByRef clExt         As Extra, _
        ByVal sSelectGItem  As String, _
        ByVal cts           As CToolSettings, _
        ByVal iThinTyp      As Int16 _
)

Try
    With Chart1
		.Annotations.Clear							'全ての注釈ラインを消去
		'If IsNothing(p_dtFile.iPxBuf)
		If IsNothing(p_dtFile.iOxBuf)
			Exit Sub
		End If
		'補正済みデータ表示
        Select Case arg.sCalcMethod
        Case lstCalcMethod(eCalcMethod.THR) , lstCalcMethod(eCalcMethod.SLOPE), lstCalcMethod(eCalcMethod.PHASE), lstCalcMethod(eCalcMethod.MELT)
        'THRまたはSLOPE法,Phase法
            'If dGelTime > 0
			    'ゲルタイム 垂直補助線追加
			    .ChartAreas(0).AxisX.StripLines.Clear()
			    Dim Time_THR As New StripLine()
			    Dim dGelTimeG	As Double = arg.dGelTime / MEAS_TIME_RESOLUTION * 1000.0 / RealTimeThinning(iThinTyp)
			    Dim dTempG		As Double = arg.dMeltTemp
		        Time_THR.Font           = New Font(BASE_FONT, 12, FontStyle.Regular)
				
				Select Case arg.sCalcMethod
				Case lstCalcMethod(eCalcMethod.THR) , lstCalcMethod(eCalcMethod.SLOPE), lstCalcMethod(eCalcMethod.PHASE)
					Select Case arg.iGelTimeJudge
					Case 0      'NG
    					'Time_THR.Text           = $"NG: {dGelTime:F2} Sec"
    					Time_THR.Text           = m_sMes(efrmMainSTR.iGEL_NG).Replace("#GEL#", arg.dGelTime.ToString("F2"))
						Time_THR.ForeColor      = Color.Red
						'Chart1.BackColor        = Color.lightpink
					Case 1      'OK
    					'Time_THR.Text           = $"OK: {dGelTime:F2} Sec"
    					Time_THR.Text           = m_sMes(efrmMainSTR.iGEL_OK).Replace("#GEL#", arg.dGelTime.ToString("F2"))
						Time_THR.ForeColor      = Color.Blue
						'Chart1.BackColor        = Color.lightgreen
					Case else   'NA
    					'Time_THR.Text           = $"GelTime: {dGelTime:F2} Sec"
    					Time_THR.Text           = m_sMes(efrmMainSTR.iGEL_TIME).Replace("#GEL#", arg.dGelTime.ToString("F2"))
						Time_THR.ForeColor      = Color.Blue
						'Chart1.BackColor        = Color.White
					End Select
				    Time_THR.IntervalOffset = dGelTimeG

					'ゲルタイム判定範囲の境界線
					Dim Time_JudgeS As New StripLine()
					Dim Time_JudgeE As New StripLine()
					Dim dJudgeTimeS As Double = arg.dJudgeRangeMin / MEAS_TIME_RESOLUTION * 1000.0 / RealTimeThinning(iThinTyp)
					Dim dJudgeTimeE As Double = arg.dJudgeRangeMax / MEAS_TIME_RESOLUTION * 1000.0 / RealTimeThinning(iThinTyp)

					Time_JudgeS.Text           = "lower"
					Time_JudgeS.TextAlignment  = StringAlignment.Far
					Time_JudgeS.Font           = New Font(BASE_FONT, 12, FontStyle.Regular)
					Time_JudgeS.ForeColor      = Color.DarkCyan
					Time_JudgeS.Interval       = 0
					Time_JudgeS.IntervalOffset = dJudgeTimeS
					Time_JudgeS.BorderWidth    = 1
					Time_JudgeS.BorderDashStyle = ChartDashStyle.Solid
					Time_JudgeS.BorderColor    = Color.DarkCyan
					.ChartAreas(0).AxisX.StripLines.Add(Time_JudgeS)

					Time_JudgeE.Text           = "upper"
					Time_JudgeE.TextAlignment  = StringAlignment.Near
					Time_JudgeE.Font           = New Font(BASE_FONT, 12, FontStyle.Regular)
					Time_JudgeE.ForeColor      = Color.DarkCyan
					Time_JudgeE.Interval       = 0
					Time_JudgeE.IntervalOffset = dJudgeTimeE
					Time_JudgeE.BorderWidth    = 1
					Time_JudgeE.BorderDashStyle = ChartDashStyle.Solid
					Time_JudgeE.BorderColor    = Color.DarkCyan
					.ChartAreas(0).AxisX.StripLines.Add(Time_JudgeE)

					'データ有効範囲の境界線
					If arg.dAprxRangeMax<>0
						Dim Time_AprxS As New StripLine()
						Dim Time_AprxE As New StripLine()
						Dim dAprxRangeS As Double = arg.dAprxRangeMin / MEAS_TIME_RESOLUTION * 1000.0 / RealTimeThinning(iThinTyp)
						Dim dAprxRangeE As Double = arg.dAprxRangeMax / MEAS_TIME_RESOLUTION * 1000.0 / RealTimeThinning(iThinTyp)
						Time_AprxS.Interval       = 0
						Time_AprxS.IntervalOffset = dAprxRangeS
						Time_AprxS.BorderWidth    = 2
						Time_AprxS.BorderDashStyle = ChartDashStyle.Solid
						Time_AprxS.BorderColor    = Color.DarkGray
						.ChartAreas(0).AxisX.StripLines.Add(Time_AprxS)
						Time_AprxE.Interval       = 0
						Time_AprxE.IntervalOffset = dAprxRangeE
						Time_AprxE.BorderWidth    = 2
						Time_AprxE.BorderDashStyle = ChartDashStyle.Solid
						Time_AprxE.BorderColor    = Color.DarkGray
						.ChartAreas(0).AxisX.StripLines.Add(Time_AprxE)
					End If

				Case lstCalcMethod(eCalcMethod.MELT)
					If cts.iChartType = 1
    					Time_THR.Text           = $"{dTempG:F1} deg C"
						Time_THR.ForeColor      = Color.Blue
						'Chart1.BackColor        = Color.White
						Time_THR.IntervalOffset = dTempG
					Else
    					Time_THR.Text           = $"{arg.dGelTime:F2} Sec"
						Time_THR.ForeColor      = Color.Blue
						'Chart1.BackColor        = Color.White
						Time_THR.IntervalOffset = dGelTimeG
						'データ有効範囲の境界線
						If arg.dAprxRangeMax<>0
							Dim Time_AprxS As New StripLine()
							Dim Time_AprxE As New StripLine()
							Dim dAprxRangeS As Double = arg.dAprxRangeMin / MEAS_TIME_RESOLUTION * 1000.0 / RealTimeThinning(iThinTyp)
							Dim dAprxRangeE As Double = arg.dAprxRangeMax / MEAS_TIME_RESOLUTION * 1000.0 / RealTimeThinning(iThinTyp)
							Time_AprxS.Interval       = 0
							Time_AprxS.IntervalOffset = dAprxRangeS
							Time_AprxS.BorderWidth    = 2
							Time_AprxS.BorderDashStyle = ChartDashStyle.Solid
							Time_AprxS.BorderColor    = Color.DarkGray
							.ChartAreas(0).AxisX.StripLines.Add(Time_AprxS)
							Time_AprxE.Interval       = 0
							Time_AprxE.IntervalOffset = dAprxRangeE
							Time_AprxE.BorderWidth    = 2
							Time_AprxE.BorderDashStyle = ChartDashStyle.Solid
							Time_AprxE.BorderColor    = Color.DarkGray
							.ChartAreas(0).AxisX.StripLines.Add(Time_AprxE)
						End If
					End If
				End Select

				Time_THR.TextAlignment  = StringAlignment.Far
				Time_THR.Interval       = 0
				Time_THR.BorderWidth    = 1
				Time_THR.BorderDashStyle = ChartDashStyle.DashDot
				Time_THR.BorderColor    = Color.Red
	            If arg.dGelTime > 0
					.ChartAreas(0).AxisX.StripLines.Add(Time_THR)
				End If
            'Else
            '    Chart1.BackColor        = Color.White
            'End if

        Case lstCalcMethod(eCalcMethod.EXTRA)
        End Select


    'Y軸
	'補正済みデータ表示
        Select Case arg.sCalcMethod
        Case lstCalcMethod(eCalcMethod.THR), lstCalcMethod(eCalcMethod.PHASE)
			If (sSelectGItem="rbCorrect" And arg.sCalcMethod = lstCalcMethod(eCalcMethod.THR)) Or _
				 (sSelectGItem="rbCorrectP" And arg.sCalcMethod = lstCalcMethod(eCalcMethod.THR)) Or _
				 (sSelectGItem="rbPosPhase" And arg.sCalcMethod = lstCalcMethod(eCalcMethod.PHASE))
				'THR法
				'ゲルタイム 水平補助線追加 
				.ChartAreas(0).AxisY.StripLines.Clear()
				.ChartAreas(0).AxisY2.StripLines.Clear()
				Dim Stress_THR  As New StripLine()
				Dim dThresholdG As Double
				Select Case sSelectGItem
				Case "rbCorrect"
   					dThresholdG             = arg.dFullStressADVal * (arg.dToqueThreshold / 100.0)
				Case "rbCorrectP"
   					dThresholdG             = arg.dToqueThreshold
				Case "rbPosPhase"
   					dThresholdG             = 90.0 * (arg.dToqueThreshold / 100.0)
				End Select
				
				'Stress_THR.Text             = $"Threshold: {dToqueThreshold} % "
				Stress_THR.Text             = m_sMes(efrmMainSTR.CHART_THRESHOLD) & arg.dToqueThreshold.ToString("F1") & " % "
				If arg.sCalcMethod = lstCalcMethod(eCalcMethod.PHASE)
					Stress_THR.Text             &= " -> " & dThresholdG.ToString("F1") & "[deg]"
				ElseIf sSelectGItem = "rbCorrectP"
				Else
					Stress_THR.Text             &= " -> " & dThresholdG.ToString("F1") & "[uN]"
				End If
				Stress_THR.Font             = New Font(BASE_FONT, 12, FontStyle.Regular)
				Stress_THR.ForeColor        = Color.Blue
				Stress_THR.Interval         = 0
				Stress_THR.TextAlignment	= StringAlignment.Near         'Near 左端
				Stress_THR.TextLineAlignment= StringAlignment.Far          'Far  上側
				Stress_THR.IntervalOffset   = dThresholdG
				Stress_THR.BorderWidth      = 1
				Stress_THR.BorderDashStyle  = ChartDashStyle.DashDot
				Stress_THR.BorderColor      = Color.DarkGreen
				.ChartAreas(0).AxisY.StripLines.Add(Stress_THR)
			End If

        Case lstCalcMethod(eCalcMethod.SLOPE)
        'SLOPE法
			'ゲルタイム 水平補助線追加 
			.ChartAreas(0).AxisY.StripLines.Clear()
			.ChartAreas(0).AxisY2.StripLines.Clear()
			Dim Stress_THR  As New StripLine()
			Dim dThresholdG As Double
			If IsNothing(p_dtFile.iSlope)
	    		dThresholdG					= .ChartAreas(0).AxisY2.Maximum * (arg.dToqueThreshold / 100.0)
			ElseIf arg.dAprxRangeMax<>0
				Dim dMax As Double
				GetMaxMin1ArrayRange(p_dtFile.iSlope, arg.dAprxRangeMin, arg.dAprxRangeMax, dMax)
	    		dThresholdG					= dMax * (arg.dToqueThreshold / 100.0)
			Else
	    		dThresholdG					= p_dtFile.iSlope.Max * (arg.dToqueThreshold / 100.0)
			End If
			Stress_THR.Text             = $"Threshold: {arg.dToqueThreshold:F1} %"
			Stress_THR.Font             = New Font(BASE_FONT, 12, FontStyle.Regular)
			Stress_THR.ForeColor        = Color.Blue
			Stress_THR.Interval         = 0
			Stress_THR.TextAlignment	= StringAlignment.Near				'Near 左端
			Stress_THR.TextLineAlignment= StringAlignment.Far				'Far  上側
			Stress_THR.IntervalOffset   = dThresholdG
			Stress_THR.BorderWidth      = 1
			Stress_THR.BorderDashStyle  = ChartDashStyle.DashDot
			Stress_THR.BorderColor      = Color.DarkGreen
			.ChartAreas(0).AxisY2.StripLines.Add(Stress_THR)

			'補助グラフ SLOPEのグラフ設定
            .ChartAreas(0).Axisy2.Enabled					= AxisEnabled.Auto
            .ChartAreas(0).AxisY2.Title						= frmMain.p_sMes(efrmMainSTR.iSTRESS_CHG_RATE)
            .ChartAreas(0).AxisY2.TitleFont					= New Font(BASE_FONT, 14, FontStyle.Regular)
            .ChartAreas(0).AxisY2.IntervalType				= DateTimeIntervalType.NotSet
            .ChartAreas(0).AxisY2.Crossing					= Double.NaN
            .ChartAreas(0).AxisY2.MajorGrid.Enabled			= True
            .ChartAreas(0).AxisY2.MajorGrid.LineDashStyle	= ChartDashStyle.Solid
            .ChartAreas(0).AxisY2.MajorGrid.LineColor		= Color.LightGray
            .ChartAreas(0).AxisY2.TitleAlignment			= StringAlignment.Center
        Case lstCalcMethod(eCalcMethod.EXTRA)
        '外挿法
			If sSelectGItem="rbCorrect" 
				clExt.ExtrapoCursorAllDisp(Chart1)	'カーソル表示
				clExt.StartLineDisp(Chart1)			'開始点(下ベースライン)の接線を引く
				clExt.MiddleLineDisp(Chart1)		'中間点(中間ライン)の接線を引く
				clExt.EndLineDisp(Chart1)			'終了点(上ベースライン)の接線を引く
			End If
            clExt.GelTimeCrossLineDisp(Chart1)		'ゲルタイム表示
        Case lstCalcMethod(eCalcMethod.MELT)
			'ゲルタイム 水平補助線追加 
			.ChartAreas(0).AxisY.StripLines.Clear()
			.ChartAreas(0).AxisY2.StripLines.Clear()

			Select Case sSelectGItem
			Case "rbViscosity"
				If arg.dGelTime > 0
					Dim Stress_THR  As New StripLine()
					Dim dViscoG As Double
   					dViscoG					= arg.dMeltVisco

					Stress_THR.Text             = $"Viscosity: {dViscoG:F1} [PaS] "
					Stress_THR.Font             = New Font(BASE_FONT, 12, FontStyle.Regular)
					Stress_THR.ForeColor        = Color.Blue
					Stress_THR.Interval         = 0
					Stress_THR.TextAlignment	= StringAlignment.Near         'Near 左端
					Stress_THR.TextLineAlignment= StringAlignment.Far          'Far  上側
					Stress_THR.IntervalOffset   = dViscoG
					Stress_THR.BorderWidth      = 1
					Stress_THR.BorderDashStyle  = ChartDashStyle.DashDot
					Stress_THR.BorderColor      = Color.DarkGreen
					.ChartAreas(0).AxisY.StripLines.Add(Stress_THR)
				End If
			Case Else
			End Select
        End Select
'Debug.print( "AxisY.Maximum = " & Chart1.ChartAreas(0).AxisY.Maximum )

	End With
Catch ex As Exception
    Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
    Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub














































''' <summary>
''' 長期間欠モードリアルタイム測定時のトレンド表示（frmMain タイマーイベントからコールされる）
''' </summary>
''' <param name="arg">共通引数構造体
''' 	sSelectGItem			グラフ選択
''' 	bHist					履歴描画 有効無効
''' 	bStd					基準描画 有効無効
''' 	dYSpanPer				Y軸拡大率
''' 	dLimitTime				測定時間制限
''' 	sCalcMethod				計算方法
''' 	iGelTimeJudge			ゲルタイム判定
''' 	dJudgeRangeMin			ゲルタイム判定範囲
''' 	dJudgeRangeMax			ゲルタイム判定範囲
''' 	dAprxRangeMin			有効範囲
''' 	dAprxRangeMax			有効範囲
''' 	dGelTime				ゲルタイム 測定値
''' 	dToqueThreshold			ゲルタイム 応力閾値
''' 	dFullStressADVal		ゲルタイム フル応力
''' 	dMeltTemp				MELT法 温度測定値
''' 	dMeltVisco				MELT法 粘度測定値
''' 	dTempMin				横軸温度 温度範囲
''' 	dTempMax				横軸温度 温度範囲</param>
''' <param name="dFil">データファイルクラス</param>
''' <param name="Chart1">チャートコントロール</param>
''' <param name="clExt">外挿法クラス</param>
''' <param name="cts">オプション設定クラス</param>
''' <param name="bRealTrendUpdate">初期設定（チャート設定、データテーブル定義など）行うか True：行わない False：行う</param>
Public Sub LTRealChartDisp1( _
	ByRef arg		As ChartArg, _
	ByRef dFil		As DataFile, _
	ByRef Chart1	As Chart, _
	ByRef clExt		As Extra, _
	ByRef cts		As CToolSettings, _
	ByRef bRealTrendUpdate As Boolean _
)
Dim dTemp()		As Double = Nothing
Dim dMv()		As Double = Nothing
Dim dTemp2()	As Double = Nothing
Dim dMv2()		As Double = Nothing
Dim strColumnName		As String = String.Empty
Dim intCsvRowCounter	As Integer = 1
Dim iMes		As Int16 = 0            '測定バッファ使用
Dim iHis		As Int16 = 0            '履歴バッファ使用数
Dim iStd		As Int16 = 0            '基準バッファ使用数
Dim ChartGrid   As Integer
Dim iThinTyp	As int16 
Try
	iHis = If(arg.bHist,	dFil.CheckHist(), 0)		'履歴バッファ使用数検査
	iStd = If(arg.bStd,		dFil.CheckStd(), 0)			'基準バッファ使用数検査

    Select Case cts.penGrids.DashStyle				'グリッドスタイルPen形式からChartへ変換
    Case Drawing2D.DashStyle.Solid
        ChartGrid = ChartDashStyle.Solid
    Case Drawing2D.DashStyle.Dash
        ChartGrid = ChartDashStyle.Dash
    Case Drawing2D.DashStyle.Dot
        ChartGrid = ChartDashStyle.Dot
    Case Else
        ChartGrid = ChartDashStyle.NotSet
    End Select

    'グラフサンプル数計算 ------------------
	Dim iSampleNumOnGraph	As Int64
	Dim iScrolSampleNum		As int64
	Dim iInterValSec		As int32
	Dim iArrayCnt			As int32
	'測定インターバル秒,配列数を得る
	If Not IsNothing(dFil.iLT_MSec)
		iInterValSec	= If( dFil.iLT_MSec.Length>=2, dFil.iLT_MSec(1)-dFil.iLT_MSec(0), cts.LTMeasureInterval * 60 )
		iArrayCnt		= dFil.iLT_MSec.Length
	ElseIf Not IsNothing(dFil.iMSec_s1)
		iInterValSec	= If( dFil.iMSec_s1.Length>=2, dFil.iMSec_s1(1)-dFil.iMSec_s1(0), cts.LTMeasureInterval * 60 )
		iArrayCnt		= dFil.iMSec_s1.Length
	ElseIf Not IsNothing(dFil.iMSec_s2)
		iInterValSec	= If( dFil.iMSec_s2.Length>=2, dFil.iMSec_s2(1)-dFil.iMSec_s2(0), cts.LTMeasureInterval * 60 )
		iArrayCnt		= dFil.iMSec_s2.Length
	Else
		iInterValSec	= cts.LTMeasureInterval * 60
		iArrayCnt		= 0
	End If
    If Not isNothing(dFil.iLT_TempBuf) Or Not isNothing(dFil.iTempBuf_s1) Or Not isNothing(dFil.iTempBuf_s2) Or _
		Not IsNothing(dFil.iTempBuf_h1) Or _
		Not IsNothing(dFil.iTempBuf_h2) Or _
		Not IsNothing(dFil.iTempBuf_h3) Or _
		Not IsNothing(dFil.iTempBuf_h4) Or _
		Not IsNothing(dFil.iTempBuf_h5)
	'最新測定データがある ----------------------------------------------------------
        If cts.iRTXAxis = 0 AndAlso cts.iChartType = 0
		'自動モード
			iScrolSampleNum		= cts.iScrlWidth / (iInterValSec/60)			'インターバル分で割る
            iSampleNumOnGraph	= iArrayCnt - 1
			If iArrayCnt - 1 > iScrolSampleNum
				If dFil.p_head.dGelTime = -9
				'測定中 スクロールモード
					p_iScrlStart = iArrayCnt - iScrolSampleNum - 1
				Else
					p_iScrlStart = 0
				End If
			Else
				p_iScrlStart = 0
			End If
        Else
		'固定モード
			Select Case cts.iChartType
			Case 0	'Time
				If Not IsNothing(dFil.iLT_MSec)
					iSampleNumOnGraph = iArrayCnt - 1
				Else
					iSampleNumOnGraph = cts.MainGraphXMax(cts.iChartType)
				End If
			Case 1	'Temp
				If Not IsNothing(dFil.iLT_MSec)
					iSampleNumOnGraph = iArrayCnt - 1
				Else
					iSampleNumOnGraph = cts.MainGraphXMax(cts.iChartType) * 1000.0
				End If
			End Select
			p_iScrlStart = 0
        End If
	ElseIf  iHis<>0 Or iStd<>0
	'最新測定データはなく、履歴または基準データがある ------------------------------
		Dim iMax As Int32 = 0
		iMax = GetArrayMaxInHisStd(iHis, iStd, dFil)
        If cts.iRTXAxis = 0  AndAlso cts.iChartType = 0
		'自動モード
			iScrolSampleNum = cts.iScrlWidth / (iInterValSec/60)				'インターバル分で割る
			If iMax-1 > iScrolSampleNum
			'スクロールモード
	            iSampleNumOnGraph = iMax-1
				p_iScrlStart = iMax - iScrolSampleNum 
			Else
	            iSampleNumOnGraph = iMax-1
				p_iScrlStart = 0
			End If
        Else
		'固定モード
            iSampleNumOnGraph = cts.MainGraphXMax(cts.iChartType)
			p_iScrlStart = 0
        End If
	Else
	'未測定画面の表示(初期表示) -------------------------------------------------------
		Select Case cts.iChartType
		Case 0 '時間
			If cts.iRTXAxis = 0
			'X軸自動
				iSampleNumOnGraph = cInitXSec																'データ
			Else
			'X軸固定
				iSampleNumOnGraph = cts.MainGraphXMax(cts.iChartType)
			End If
		Case 1 '温度
			If Not IsNothing(dFil.iTempBuf)
				iSampleNumOnGraph = dFil.iTempBuf.GetUpperBound(0)
			Else
				If cts.iRTXAxis = 0
				'X軸自動
					iSampleNumOnGraph = 300		'0-300℃分のデータ
				Else
				'X軸固定
					iSampleNumOnGraph = cts.MainGraphXMax(cts.iChartType)
				End If
			End If
		End Select
		
	End If
	'---------------------------------------

	'温度系の選択時のワーク配列確保
	Select case arg.sSelectGItem
	Case "rbHPTemp"
		Array.Resize(dTemp,		iSampleNumOnGraph)
	Case "rbHPMV"
		Array.Resize(dMv,		iSampleNumOnGraph)
	Case "rbPBTemp"
		Array.Resize(dTemp2,	iSampleNumOnGraph)
	Case "rbPBMV"
		Array.Resize(dMv2,		iSampleNumOnGraph)
	Case Else
	End Select

	'データテーブルの設定
	If Not bRealTrendUpdate
		LTRealTimeChartDataTableDefine(cts.iChartType, ds, dt, arg.sSelectGItem, iHis, iStd, arg.sCalcMethod)
	End If
	'データテーブルへのデータ追加 20210210
	ds.Tables(0).Rows.Clear()
	LTRealTimeDataTableUpdate( _
		arg, _
		dFil, _
		cts, _
		ds.Tables(0), _
		iHis, _
		iStd, _
		iSampleNumOnGraph, _
		iThinTyp _
		)

	If Not bRealTrendUpdate
		'チャートコントロールの設定
		LTRealTimeChartSetting(cts, Chart1, ds.Tables(0), iHis, iStd, arg.sSelectGItem, arg.sCalcMethod)
		bRealTrendUpdate = True
	End If

    '-----------------------------------------------------------------------------------------------------
    '以下、Ｘ軸とＹ軸の設定
    LTChartXYAxisSetting( _
			arg, _
            Chart1, _
            dFil, _
            clExt, _
            cts, _
            iHis, _
            iStd, _
            ChartGrid, _
            ds, _
            dTemp, _
            dMv, _ 
            iThinTyp _ 
    )

	'閾値・ゲルタイムライン表示 (補正データ表示のみ)
		LTChartUpdateGelTimeLine( 
				arg, _
				Chart1, _
				clExt, _
				arg.sSelectGItem, _
				cts, _
				iThinTyp _ 
		)
	LTDataPlot(cts.iChartType, Chart1, dFil, iHis, iStd, arg.sSelectGItem, iThinTyp)		'データ点プロット

	'ChartAreaのサイズ位置調整
	Chart1.Location = New Point(-2, 43)
	Chart1.Size		= New Size(1245, 900)
	Chart1.ChartAreas(0).Position.Auto					= False
	Chart1.ChartAreas(0).Position.X						= 0
	Chart1.ChartAreas(0).Position.Y						= 9
	Chart1.ChartAreas(0).Position.height				= 88
	Chart1.ChartAreas(0).Position.Width					= 97

	'Chart1.Serializer.Save("C:\Test\Chart1.xml")
	'Chart1.Update()

Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
    Console.Write("error mes:" & ex.Message.ToString)
End Try
End Sub
''' <summary>
''' 選択された系列のデータテーブルを定義する 長期間欠モード
''' </summary>
''' <param name="iType"> 0:時間 1:温度 </param>
''' <param name="ds"></param>
''' <param name="dt"></param>
''' <param name="sSelectGItem"></param>
''' <param name="iHis"></param>
Private Sub	LTRealTimeChartDataTableDefine(ByVal iType As Int16, ByRef ds As DataSet, ByRef dt As DataTable, ByVal sSelectGItem As String, ByVal iHis As Int16, ByVal iStd As Int16, ByVal sCalcMethod As String)
Dim iStdBase As Int32 = cHistMax+1
Try
	ds.Tables.Clear()
	dt.Columns.Clear()

    dt.Columns.Add(If(iType=0, "Time", "Temp"))
	Select case sSelectGItem
	Case "rbCorrect"
        dt.Columns.Add(sChartItemStr(eChartItem.Hosei))
        If iHis<>0
            For i=1 To iHis
				dt.Columns.Add(sChartItemStr(eChartItem.Hosei) & i.ToString)
            Next
        End if
        If iStd<>0
            For i = iStdBase To iStdBase + iStd - 1
                dt.Columns.Add(sChartItemStr(eChartItem.Hosei) & i.ToString)
            Next
        End if
	Case "rbHPTemp"
        dt.Columns.Add(sChartItemStr(eChartItem.TempBuf))
        If iHis<>0
            For i=1 To iHis
                dt.Columns.Add(sChartItemStr(eChartItem.TempBuf) & i.ToString)
            Next
        End If
        If iStd<>0
            For i = iStdBase To iStdBase + iStd - 1
                dt.Columns.Add(sChartItemStr(eChartItem.TempBuf) & i.ToString)
            Next
        End if
	Case "rbHPMV"
        dt.Columns.Add(sChartItemStr(eChartItem.MvBuf))
        If iHis<>0
            For i=1 To iHis
                dt.Columns.Add(sChartItemStr(eChartItem.MvBuf) & i.ToString)
            Next
        End If
        If iStd<>0
            For i = iStdBase To iStdBase + iStd - 1
                dt.Columns.Add(sChartItemStr(eChartItem.MvBuf) & i.ToString)
            Next
        End if
	Case "rbPBTemp"
        dt.Columns.Add(sChartItemStr(eChartItem.TempBuf2))
        If iHis<>0
            For i=1 To iHis
                dt.Columns.Add(sChartItemStr(eChartItem.TempBuf2) & i.ToString)
            Next
        End If
        If iStd<>0
            For i = iStdBase To iStdBase + iStd - 1
                dt.Columns.Add(sChartItemStr(eChartItem.TempBuf2) & i.ToString)
            Next
        End if
	Case "rbPBMV"
        dt.Columns.Add(sChartItemStr(eChartItem.MvBuf2))
        If iHis<>0
            For i=1 To iHis
                dt.Columns.Add(sChartItemStr(eChartItem.MvBuf2) & i.ToString)
            Next
        End If
        If iStd<>0
            For i = iStdBase To iStdBase + iStd - 1
                dt.Columns.Add(sChartItemStr(eChartItem.MvBuf2) & i.ToString)
            Next
        End if
	Case "rbViscosity"
        dt.Columns.Add(sChartItemStr(eChartItem.Viscosity))
        If iHis<>0
            For i=1 To iHis
                dt.Columns.Add(sChartItemStr(eChartItem.Viscosity) & i.ToString)
            Next
        End If
        If iStd<>0
            For i = iStdBase To iStdBase + iStd - 1
                dt.Columns.Add(sChartItemStr(eChartItem.Viscosity) & i.ToString)
            Next
        End if
	Case "rbPosPhase"
        dt.Columns.Add(sChartItemStr(eChartItem.XPosPhase))
        If iHis<>0
            For i=1 To iHis
                dt.Columns.Add(sChartItemStr(eChartItem.XPosPhase) & i.ToString)
            Next
        End If
        If iStd<>0
            For i = iStdBase To iStdBase + iStd - 1
                dt.Columns.Add(sChartItemStr(eChartItem.XPosPhase) & i.ToString)
            Next
        End if
	Case "rbSampleStrain"
        dt.Columns.Add(sChartItemStr(eChartItem.SplStrain))
        If iHis<>0
            For i=1 To iHis
                dt.Columns.Add(sChartItemStr(eChartItem.SplStrain) & i.ToString)
            Next
        End If
        If iStd<>0
            For i = iStdBase To iStdBase + iStd - 1
                dt.Columns.Add(sChartItemStr(eChartItem.SplStrain) & i.ToString)
            Next
        End if
	Case "rbShearMod"
        dt.Columns.Add(sChartItemStr(eChartItem.ShearMod))
        If iHis<>0
            For i=1 To iHis
                dt.Columns.Add(sChartItemStr(eChartItem.ShearMod) & i.ToString)
            Next
        End If
        If iStd<>0
            For i = iStdBase To iStdBase + iStd - 1
                dt.Columns.Add(sChartItemStr(eChartItem.ShearMod) & i.ToString)
            Next
        End if
	Case "rbShearVelo"
        dt.Columns.Add(sChartItemStr(eChartItem.ShearVelo))
        If iHis<>0
            For i=1 To iHis
                dt.Columns.Add(sChartItemStr(eChartItem.ShearVelo) & i.ToString)
            Next
        End If
        If iStd<>0
            For i = iStdBase To iStdBase + iStd - 1
                dt.Columns.Add(sChartItemStr(eChartItem.ShearVelo) & i.ToString)
            Next
        End if
	Case "rbStorageMod"
        dt.Columns.Add(sChartItemStr(eChartItem.XStrgMod))
        If iHis<>0
            For i=1 To iHis
                dt.Columns.Add(sChartItemStr(eChartItem.XStrgMod) & i.ToString)
            Next
        End If
        If iStd<>0
            For i = iStdBase To iStdBase + iStd - 1
                dt.Columns.Add(sChartItemStr(eChartItem.XStrgMod) & i.ToString)
            Next
        End if
	Case "rbElasticMod"
        dt.Columns.Add(sChartItemStr(eChartItem.XElasMod))
        If iHis<>0
            For i=1 To iHis
                dt.Columns.Add(sChartItemStr(eChartItem.XElasMod) & i.ToString)
            Next
        End If
        If iStd<>0
            For i = iStdBase To iStdBase + iStd - 1
                dt.Columns.Add(sChartItemStr(eChartItem.XElasMod) & i.ToString)
            Next
        End if
	Case "rbLossTangent"
        dt.Columns.Add(sChartItemStr(eChartItem.XLossTan))
        If iHis<>0
            For i=1 To iHis
                dt.Columns.Add(sChartItemStr(eChartItem.XLossTan) & i.ToString)
            Next
        End If
        If iStd<>0
            For i = iStdBase To iStdBase + iStd - 1
                dt.Columns.Add(sChartItemStr(eChartItem.XLossTan) & i.ToString)
            Next
        End if
	Case "rbCorrectP"
        dt.Columns.Add(sChartItemStr(eChartItem.HoseiP))
        If iHis<>0
            For i=1 To iHis
                dt.Columns.Add(sChartItemStr(eChartItem.HoseiP) & i.ToString)
            Next
        End If
        If iStd<>0
            For i = iStdBase To iStdBase + iStd - 1
                dt.Columns.Add(sChartItemStr(eChartItem.HoseiP) & i.ToString)
            Next
        End if
    Case Else
	End Select
    ds.Tables.Add(dt)
	Exit Sub
    '----------------------------------------------------------------------------------------------------
Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
    Console.Write("error mes:" & ex.Message.ToString)
End Try
End Sub
''' <summary>
''' データテーブルへのデータ追加 長期間欠モード
''' </summary>
''' <param name="arg"></param>
'''		"sSelectGItem"
'''		"dLimitTime"
'''		"sCalcMethod"
'''		"dFullStressADVal"
''' <param name="dFil"></param>
''' <param name="cts"></param>
''' <param name="dt"></param>
''' <param name="iHis"></param>
''' <param name="iStd"></param>
''' <param name="iSampleNumOnGraph"></param>
''' <param name="iThinTyp"></param>v
Public Sub LTRealTimeDataTableUpdate( _
	ByRef arg		As ChartArg, _
	ByRef dFil		As DataFile, _
	ByRef cts		As CToolSettings, _
	ByRef dt		As DataTable, _
	ByVal iHis              As Int16, _
	ByVal iStd              As Int16, _
	ByVal iSampleNumOnGraph	As Int64, _
	ByVal iThinTyp			As Int16
	)

Dim i			As int32
Dim j			As int32
Dim decTime		As Double
Dim dData		As Double
Dim rw			As DataRow = Nothing
Dim dTemp()		As Double = Nothing
Dim dMv()		As Double = Nothing
Dim dTemp2()	As Double = Nothing
Dim dMv2()		As Double = Nothing
Dim Ary()		As Int64 = Nothing
Dim Ary2()		As Int64 = Nothing
Dim dAry()		As Double = Nothing
Dim dAry2()		As Double = Nothing
Dim sIdx		As Int16 = 0
Dim iLimit		As Int64

Try
	Array.Resize(dTemp,		iSampleNumOnGraph)
	Array.Resize(dTemp2,	iSampleNumOnGraph)
	Array.Resize(dMv,		iSampleNumOnGraph)
	Array.Resize(dMv2,		iSampleNumOnGraph)

	
	Dim iStep As Int32 = 1
    For j = p_iScrlStart + iStep To iSampleNumOnGraph Step iStep
        'データテーブルに必要データを加える
        rw = dt.NewRow
		i = j-1
		Select Case cts.iChartType
		Case 0
		'時間
			If Not IsNothing(dFil.iLT_MSec)
				decTime	= dFil.iLT_MSec(i)				'秒
			Else
				decTime	= cts.LTMeasureInterval * 60 * i
			End If
			rw("Time") = decTime
		Case 1
		'温度
			If Not IsNothing(dFil.iLT_TempBuf)
				If i<=dFil.iLT_TempBuf.GetUpperBound(0)
					decTime	= CType(dFil.iLT_TempBuf(i), Double) / 10.0					'温度換算
				Else
					decTime	= vbNull
				End If
			Else
				decTime	= CType(i * 0.01, Double) / 10.0
			End If
			rw("Temp") = decTime
		End Select
        Ary		= Nothing
        Ary2	= Nothing
        sIdx    = 0
        Select case arg.sSelectGItem
        Case "rbCorrect"
            If isNothing(dFil.iLT_Hosei)
                Array.Resize(dAry,iSampleNumOnGraph)
                Array.Resize(dAry2,iSampleNumOnGraph)
            Else
                dAry = dFil.iLT_Hosei
                dAry2 = dFil.iLT_Hosei
            End If
            sIdx = eChartItem.Hosei
        Case "rbHPTemp"
            If isNothing(dFil.iLT_TempBuf)
                Array.Resize(Ary,iSampleNumOnGraph)
            Else
                Ary = dFil.iLT_TempBuf
            End If
            sIdx = eChartItem.TempBuf
        Case "rbHPMV"
            If isNothing(dFil.iLT_MvBuf)
                Array.Resize(Ary,iSampleNumOnGraph)
            Else
                Ary = dFil.iLT_MvBuf
            End If
            sIdx = eChartItem.MvBuf
        Case "rbPBTemp"
            If isNothing(dFil.iLT_TempBuf2)
                Array.Resize(Ary,iSampleNumOnGraph)
            Else
                Ary = dFil.iLT_TempBuf2
            End If
            sIdx = eChartItem.TempBuf2
        Case "rbPBMV"
            If isNothing(dFil.iLT_MvBuf2)
                Array.Resize(Ary,iSampleNumOnGraph)
            Else
                Ary = dFil.iLT_MvBuf2
            End If
            sIdx = eChartItem.MvBuf2
        Case "rbViscosity"
            If isNothing(dFil.dLT_Viscosity)
                Array.Resize(dAry,iSampleNumOnGraph)
            Else
                dAry = dFil.dLT_Viscosity
            End If
            sIdx = eChartItem.Viscosity
		Case "rbPosPhase"
            If isNothing(dFil.dLT_XPosPhase)
                Array.Resize(dAry,iSampleNumOnGraph)
            Else
                dAry = dFil.dLT_XPosPhase
            End If
            sIdx = eChartItem.XPosPhase
		Case "rbSampleStrain"
            If isNothing(dFil.dLT_SplStrain)
                Array.Resize(dAry,iSampleNumOnGraph)
            Else
                dAry = dFil.dLT_SplStrain
            End If
            sIdx = eChartItem.SplStrain
		Case "rbShearMod"
            If isNothing(dFil.dLT_ShearMod)
                Array.Resize(dAry,iSampleNumOnGraph)
            Else
                dAry = dFil.dLT_ShearMod
            End If
            sIdx = eChartItem.ShearMod
		Case "rbShearVelo"
            If isNothing(dFil.dLT_ShearVelo)
                Array.Resize(dAry,iSampleNumOnGraph)
            Else
                dAry = dFil.dLT_ShearVelo
            End If
            sIdx = eChartItem.ShearVelo
		Case "rbStorageMod"
            If isNothing(dFil.dLT_XStrgMod)
                Array.Resize(dAry,iSampleNumOnGraph)
            Else
                dAry = dFil.dLT_XStrgMod
            End If
            sIdx = eChartItem.XStrgMod
		Case "rbElasticMod"
            If isNothing(dFil.dLT_XElasMod)
                Array.Resize(dAry,iSampleNumOnGraph)
            Else
                dAry = dFil.dLT_XElasMod
            End If
            sIdx = eChartItem.XElasMod
		Case "rbLossTangent"
            If isNothing(dFil.dLT_XLossTan)
                Array.Resize(dAry,iSampleNumOnGraph)
            Else
                dAry = dFil.dLT_XLossTan
            End If
            sIdx = eChartItem.XLossTan
		Case "rbCorrectP"
            If isNothing(dFil.dLT_Hosei100)
                Array.Resize(dAry,iSampleNumOnGraph)
            Else
                dAry = dFil.dLT_Hosei100
            End If
            sIdx = eChartItem.HoseiP
		Case Else
        End Select
		'X軸制限値を取得
		Select Case arg.sSelectGItem
        Case "rbViscosity", "rbPosPhase", "rbSampleStrain", "rbShearMod", "rbShearVelo", "rbStorageMod", "rbElasticMod", "rbLossTangent","rbCorrect","rbCorrectP","rbZStress"
			if dAry.Length > iSampleNumOnGraph
				iLimit = iSampleNumOnGraph - 1
			Else
				iLimit = dAry.GetUpperBound(0)
			End If
		Case Else
			if Ary.Length > iSampleNumOnGraph
				iLimit = iSampleNumOnGraph - 1
			Else
				iLimit = Ary.GetUpperBound(0)
			End If
		End Select
        If i <= iLimit
            Select Case arg.sSelectGItem
            Case "rbHPTemp"
                dData	= CType(Ary(i) / 10, Double)
                dTemp(i) = dData
            Case "rbHPMV"
                dData	= CType(Ary(i) / 10, Double)
                dMv(i) = dData
            Case "rbPBTemp"
                dData	= CType(Ary(i) / 10, Double)
                dTemp2(i) = dData
            Case "rbPBMV"
                dData	= CType(Ary(i) / 10, Double)
                dMv2(i) = dData
            Case "rbViscosity", "rbSampleStrain", "rbShearMod", "rbShearVelo", "rbStorageMod", "rbElasticMod", "rbLossTangent"
                dData	= dAry(i)
				If dData = 0
					rw(sChartItemStr(sIdx)) = Double.NaN
				ElseIf dData < 0
				Else
					rw(sChartItemStr(sIdx)) = dData
				End If
            Case "rbZStress"		'ｇ->N変換
                dData	= dAry(i)
				If dData = 0
					rw(sChartItemStr(sIdx)) = Double.NaN
				Else
					dData = dData * (cGraviAcc / 1000.0)
					rw(sChartItemStr(sIdx)) = dData
				End If
            Case "rbPosPhase"		'Rad->Deg変換
                dData	= dAry(i) * RadToDegRate
				If dData = 0
					rw(sChartItemStr(sIdx)) = Double.NaN
				Else
					rw(sChartItemStr(sIdx)) = dData
				End If
            Case "rbCorrectP"
                dData	= dAry(i)
				If dData = 0
					rw(sChartItemStr(sIdx)) = Double.NaN
				Else
					rw(sChartItemStr(sIdx)) = dData
				End If
                HistDataRwSet(dfil, iHis, iStd, cts, i, sIdx, rw, 100)
            Case "rbCorrect"
                dData	= CType(dAry(i), Double)
				'If dData = 0
				'	rw(sChartItemStr(sIdx)) = Double.NaN
				'Else
					rw(sChartItemStr(sIdx)) = dData
				'End If
                HistDataRwSet(dfil, iHis, iStd, cts, i, sIdx, rw, arg.dFullStressADVal)

            Case Else
                dData	= CType(Ary(i), Double)
            End Select
			If dData = 0
				rw(sChartItemStr(sIdx)) = Double.NaN
			Else
				rw(sChartItemStr(sIdx)) = dData
			End If
'rw(sChartItemStr(sIdx)) = dData
		Else
			rw(sChartItemStr(sIdx)) = 0
        End If
        HistDataRwSet(dfil, iHis, iStd, cts, i, sIdx, rw, arg.dFullStressADVal)
        dt.Rows.Add(rw)
    Next j
Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub
''' <summary>
''' チャートコントロールの設定 長期間欠モード
''' </summary>
''' <param name="cts"> CToolSettings参照 </param>
''' <param name="Chart1"></param>
''' <param name="dt"></param>
''' <param name="iHis"></param>
''' <param name="iStd"></param>
''' <param name="sSelectGItem"></param>
''' <param name="sCalcMethod"></param>
Private Sub	LTRealTimeChartSetting(ByVal cts As CToolSettings, ByRef Chart1 As Chart, ByRef dt As DataTable, ByVal iHis As int16, ByVal iStd As int16, ByVal sSelectGItem As String, ByVal sCalcMethod As String)
Dim j As int32
Dim strColumnName	As String
Dim iType			As Int16 = cts.iChartType
Dim iColor()		As Color = cts.clrY1Default

Try
	With Chart1
		'初期化
		.Series.Clear()
		.Legends.Clear()
		.Titles.Clear()
		.ChartAreas.Clear()

		'TITLE 作成
		Dim title1 As Title = new Title("")
		.Titles.Add(title1)
		.Titles.Item(0).Font = New Font(BASE_FONT, 16, FontStyle.Bold)
		'ChartArea作成
		.ChartAreas.Add("ChartArea1")

		'Seriesの作成
		If iHis <> 0    '履歴がある場合は表示順を測定中系列を最前面へ配置するため先に履歴を追加しておく
						'Series(0)->Series(1)->Series(2)...Series(n)[最前面]
''			For j = 1 To iHis
			For j = iHis To 1 Step -1
				strColumnName							= dt.Columns(1+j).ColumnName		'履歴系列名を取得
				.Series.Add(strColumnName)													'履歴系列追加
				.Series(strColumnName).Color			= iColor(1+j)						'色をセット
				Select Case iType
				Case 0	'時間
					.Series(strColumnName).ChartType		= SeriesChartType.Line			'グラフの種類設定
					.Series(strColumnName).MarkerSize		= If(cts.iMarkerSize = 0, 1, cts.iMarkerSize)						'マーカサイズ
					.Series(strColumnName).MarkerStyle		= if(cts.iMarkerStyle=0,MarkerStyle.Circle,cts.iMarkerStyle)		'マーカスタイル
					.Series(strColumnName).YValueMembers	= strColumnName					'履歴の系列設定
				Case 1	'温度
					.Series(strColumnName).ChartType		= SeriesChartType.FastPoint		'グラフの種類設定
					.Series(strColumnName).MarkerSize		= If(cts.iMarkerSize = 0, 1, cts.iMarkerSize)						'マーカサイズ
					.Series(strColumnName).MarkerStyle		= if(cts.iMarkerStyle=0,MarkerStyle.Circle,cts.iMarkerStyle)		'マーカスタイル
					'.Series(strColumnName).YValueMembers	= strColumnName					'履歴の系列設定
				End Select
			Next
		End If
		If iStd <> 0	'基準データがある場合
			Dim iStdBase As Int32 = iHis + 2
			For j = iStd - 1 To 0  Step -1
				strColumnName							= dt.Columns(iStdBase+j).ColumnName		'履歴系列名を取得
				.Series.Add(strColumnName)														'履歴系列追加
				.Series(strColumnName).Color			= iColor(cHistMax+2+j)					'色をセット
				Select Case iType
				Case 0
					.Series(strColumnName).ChartType		= SeriesChartType.Line			'グラフの種類設定
					.Series(strColumnName).MarkerSize		= If(cts.iMarkerSize = 0, 1, cts.iMarkerSize)						'マーカサイズ
					.Series(strColumnName).MarkerStyle		= if(cts.iMarkerStyle=0,MarkerStyle.Circle,cts.iMarkerStyle)		'マーカスタイル
					.Series(strColumnName).YValueMembers	= strColumnName					'系列設定
				Case 1	'温度
					.Series(strColumnName).ChartType		= SeriesChartType.FastPoint		'グラフの種類設定
					.Series(strColumnName).MarkerSize		= If(cts.iMarkerSize = 0, 1, cts.iMarkerSize)						'マーカサイズ
					.Series(strColumnName).MarkerStyle		= if(cts.iMarkerStyle=0,MarkerStyle.Circle,cts.iMarkerStyle)		'マーカスタイル
					'.Series(strColumnName).YValueMembers	= strColumnName					'系列設定
				End Select
			Next
		End If

		strColumnName									= dt.Columns(1).ColumnName		'測定中系列名を取得
		Chart1.Series.Add(strColumnName)												'測定中系列追加
		Chart1.Series(strColumnName).Color				= iColor(1)						'色をセット
		Select Case iType
		Case 0		'時間
			Chart1.Series(strColumnName).ChartType		= SeriesChartType.Line			'グラフの種類設定
			Chart1.Series(strColumnName).MarkerSize		= If(cts.iMarkerSize = 0, 1, cts.iMarkerSize)						'マーカサイズ
			Chart1.Series(strColumnName).MarkerStyle	= if(cts.iMarkerStyle=0,MarkerStyle.Circle,cts.iMarkerStyle)		'マーカスタイル
			Chart1.Series(strColumnName).YValueMembers	= strColumnName					'系列設定
		Case 1		'温度
			Chart1.Series(strColumnName).ChartType		= SeriesChartType.FastPoint		'グラフの種類設定
			Chart1.Series(strColumnName).MarkerSize		= If(cts.iMarkerSize = 0, 1, cts.iMarkerSize)						'マーカサイズ
			Chart1.Series(strColumnName).MarkerStyle	= if(cts.iMarkerStyle=0,MarkerStyle.Circle,cts.iMarkerStyle)		'マーカスタイル
			'Chart1.Series(strColumnName).YValueMembers	= strColumnName					'系列設定
		End Select
		'Legendの作成
		dim leg As Legend = new Legend With {
			.DockedToChartArea = "ChartArea1",
			.Alignment = StringAlignment.Near
		}
		'作ったSeriesをchartコントロールに追加する
		Chart1.Legends.Add(leg)
		Chart1.Legends(0).Enabled = False
	End With
Catch ex As Exception
    Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
    Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub
''' <summary>
''' 散布図 プロット 長期間欠モード
''' </summary>
''' <param name="iXtype">X軸 0:時間 1:温度</param>
''' <param name="oChart">チャート</param>
''' <param name="dFil">データファイルクラス</param>
''' <param name="iHis">履歴数</param>
''' <param name="iStd">基準数</param>
''' <param name="sSelectGItem">選択系列名</param>
''' <param name="iThinTyp">表示間引きタイプ</param>
Private Sub	LTDataPlot(ByVal iXtype As Int16,  oChart As Chart, ByVal dFil As DataFile, ByVal iHis As int16, ByVal iStd As int16, ByVal sSelectGItem As String, ByVal iThinTyp As Int32)
Dim i As Int64
Dim j As Int16
Dim dt As DataTable = ds.Tables(0)
Dim strColumnName As String = dt.Columns(1).ColumnName
Dim tArray()	As Long = Nothing
Try
	With oChart
	Dim x As Double
	Dim y As Double
	'基準データ2のプロット
	Dim iIdx As Int16 = -1
	Dim cIdx As Int16 = -1
	If Not IsNothing(dFil.iTempBuf_s2)
		Select Case iXtype
		Case 0	'時間
			If Not IsNothing(dFil.iMSec_s2)
				Array.Resize(tArray, dFil.iMSec_s2.Length)
				Array.Copy(dFil.iMSec_s2, tArray, dFil.iMSec_s2.Length)
				'tArray = dFil.iMSec_s2
			End If
		Case 1	'温度
			If Not IsNothing(dFil.iTempBuf_s2)
				Array.Resize(tArray, dFil.iTempBuf_s2.Length)
				Array.Copy(dFil.iTempBuf_s2, tArray, dFil.iTempBuf_s2.Length)
				'tArray = dFil.iTempBuf_s2
			End If
		End Select

		iIdx = cHistMax+cStdMax
		For j = 0 To dt.Columns.Count-1
			strColumnName = dt.Columns(j).ColumnName							'系列名を取得
			If strColumnName.Contains(iIdx.ToString)
				cIdx = j
				Exit for
			End If
		Next
		'基準アイテムの描画
		strColumnName = dt.Columns(cIdx).ColumnName						'系列名を取得
		For i = 0 To tArray.GetUpperBound(0) Step 1
			Select Case iXtype
			Case 0	'時間(分)
				x = tArray(i) / 60
			Case 1	'温度
				x = tArray(i) / 10.0
			End Select
			HistYSetForTemp(dFil, iIdx, i, sSelectGItem, y, 1)
			'If Not (x = 0 Or y = 0)
				.Series(strColumnName).Points.AddXY(x, y)				'データを設定
'Debug.Print("x=" & x & Space(2) & "y=" & y)
			'End If
		Next i
		tArray = Nothing
		iIdx = -1
	End If
	'基準データ1のプロット
	If Not IsNothing(dFil.iTempBuf_s1)
		Select Case iXtype
		Case 0	'時間
			If Not IsNothing(dFil.iMSec_s1)
				Array.Resize(tArray, dFil.iMSec_s1.Length)
				Array.Copy(dFil.iMSec_s1, tArray, dFil.iMSec_s1.Length)
				'tArray = dFil.iMSec_s1
			End If
		Case 1	'温度
			If Not IsNothing(dFil.iTempBuf_s1)
				Array.Resize(tArray, dFil.iTempBuf_s1.Length)
				Array.Copy(dFil.iTempBuf_s1, tArray, dFil.iTempBuf_s1.Length)
				'tArray = dFil.iTempBuf_s1
			End If
		End Select

		iIdx = cHistMax+cStdMax-1
		For j = 0 To dt.Columns.Count-1
			strColumnName = dt.Columns(j).ColumnName							'系列名を取得
			If strColumnName.Contains(iIdx.ToString)
				cIdx = j
				Exit for
			End If
		Next
		'基準アイテムの描画
		strColumnName = dt.Columns(cIdx).ColumnName						'系列名を取得
		For i = 0 To tArray.GetUpperBound(0) Step 1
			Select Case iXtype
			Case 0	'時間(分)
				x = tArray(i) / 60
			Case 1	'温度
				x = tArray(i) / 10.0
			End Select
			HistYSetForTemp(dFil, iIdx, i, sSelectGItem, y, 1)
			'If Not (x = 0 Or y = 0)
				.Series(strColumnName).Points.AddXY(x, y)				'データを設定
'Debug.Print("x=" & x & Space(2) & "y=" & y)
			'End If
		Next i
		tArray = Nothing
		iIdx = -1
	End If

	'測定データ、履歴データのプロット
	For j = cHistMax To 0 Step -1
		Select Case j
		Case cHistMax
			Select Case iXtype
			Case 0	'時間
				If Not IsNothing(dFil.iMSec_h5)
					Array.Resize(tArray, dFil.iMSec_h5.Length)
					Array.Copy(dFil.iMSec_h5, tArray, dFil.iMSec_h5.Length)
					'tArray = dFil.iMSec_h5
				End If
			Case 1	'温度
				If Not IsNothing(dFil.iTempBuf_h5)
					Array.Resize(tArray, dFil.iTempBuf_h5.Length)
					Array.Copy(dFil.iTempBuf_h5, tArray, dFil.iTempBuf_h5.Length)
					'tArray = dFil.iTempBuf_h5
				End If
			End Select
		Case cHistMax-1
			Select Case iXtype
			Case 0	'時間
				If Not IsNothing(dFil.iMSec_h4)
					Array.Resize(tArray, dFil.iMSec_h4.Length)
					Array.Copy(dFil.iMSec_h4, tArray, dFil.iMSec_h4.Length)
					'tArray = dFil.iMSec_h4
				End If
			Case 1	'温度
				If Not IsNothing(dFil.iTempBuf_h4)
					Array.Resize(tArray, dFil.iTempBuf_h4.Length)
					Array.Copy(dFil.iTempBuf_h4, tArray, dFil.iTempBuf_h4.Length)
					'tArray = dFil.iTempBuf_h4
				End If
			End Select
		Case cHistMax-2
			Select Case iXtype
			Case 0	'時間
				If Not IsNothing(dFil.iMSec_h3)
					Array.Resize(tArray, dFil.iMSec_h3.Length)
					Array.Copy(dFil.iMSec_h3, tArray, dFil.iMSec_h3.Length)
					'tArray = dFil.iMSec_h3
				End If
			Case 1	'温度
				If Not IsNothing(dFil.iTempBuf_h3)
					Array.Resize(tArray, dFil.iTempBuf_h2.Length)
					Array.Copy(dFil.iTempBuf_h3, tArray, dFil.iTempBuf_h3.Length)
					'tArray = dFil.iTempBuf_h3
				End If
			End Select
		Case cHistMax-3
			Select Case iXtype
			Case 0	'時間
				If Not IsNothing(dFil.iMSec_h2)
					Array.Resize(tArray, dFil.iMSec_h2.Length)
					Array.Copy(dFil.iMSec_h2, tArray, dFil.iMSec_h2.Length)
					'tArray = dFil.iMSec_h2
				End If
			Case 1	'温度
				If Not IsNothing(dFil.iTempBuf_h2)
					Array.Resize(tArray, dFil.iTempBuf_h2.Length)
					Array.Copy(dFil.iTempBuf_h2, tArray, dFil.iTempBuf_h2.Length)
					'tArray = dFil.iTempBuf_h2
				End If
			End Select
		Case cHistMax-4
			Select Case iXtype
			Case 0	'時間
				If Not IsNothing(dFil.iMSec_h1)
					Array.Resize(tArray, dFil.iMSec_h1.Length)
					Array.Copy(dFil.iMSec_h1, tArray, dFil.iMSec_h1.Length)
					'tArray = dFil.iMSec_h1
				End If
			Case 1	'温度
				If Not IsNothing(dFil.iTempBuf_h1)
					Array.Resize(tArray, dFil.iTempBuf_h1.Length)
					Array.Copy(dFil.iTempBuf_h1, tArray, dFil.iTempBuf_h1.Length)
					'tArray = dFil.iTempBuf_h1
				End If
			End Select
		Case cHistMax-5
			Select Case iXtype
			Case 0	'時間
				If Not IsNothing(dFil.iLT_MSec)
					Array.Resize(tArray, dFil.iLT_MSec.Length)
					Array.Copy(dFil.iLT_MSec, tArray, dFil.iLT_MSec.Length)
					'tArray = dFil.iLT_MSec
				End If
			Case 1	'温度
				If Not IsNothing(dFil.iLT_TempBuf)
					Array.Resize(tArray, dFil.iLT_TempBuf.Length)
					Array.Copy(dFil.iLT_TempBuf, tArray, dFil.iLT_TempBuf.Length)
					'tArray = dFil.iLT_TempBuf
				End If
			End Select

		End Select
		'履歴アイテムの描画
		If Not IsNothing(tArray)
			strColumnName = dt.Columns(j+1).ColumnName						'系列名を取得
			For i = 0 To tArray.GetUpperBound(0) Step 1
				Select Case iXtype
				Case 0	'時間(分)
					x = tArray(i) / 60 
				Case 1	'温度
					x = tArray(i) / 10.0
				End Select
				HistYSetForTemp(dFil, j, i, sSelectGItem, y, 1)
				'If Not (x = 0 Or y = 0)
					.Series(strColumnName).Points.AddXY(x, y)				'データを設定
#If 1
Debug.Print("x=" & x & Space(2) & "y=" & y)
#end if
				'End If
			Next i
			tArray = Nothing
		Else
			'系列データない場合
			.Series(strColumnName).Points.AddXY(-1, -1)
		End If
	Next

	End With
Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub
Private Sub LTHistYSetForTemp(ByVal dFil As DataFile, ByVal iHisIdx As Int16, ByVal iRow As Long, ByVal sSelectGItem As String, ByRef dVal As Double)
	Select Case iHisIdx
#If 0
	Case 7
		Select Case sSelectGItem
		Case "rbCorrect"
			dVal = dFil.iHosei_s2(iRow)
		Case "rbHPTemp"
			dVal = dFil.iTempBuf_s2(iRow) / 10
		Case "rbHPMV"
			dVal = dFil.iMvBuf_s2(iRow) / 10
		Case "rbPBTemp"
			dVal = dFil.iTempBuf2_s2(iRow) / 10
		Case "rbPBMV"
			dVal = dFil.iMvBuf2_s2(iRow) / 10
		Case "rbViscosity"
			dVal = dFil.dViscosity_s2(iRow)
		Case "rbPosPhase"
			dVal = dFil.dXPosPhase_s2(iRow) * RadToDegRate
		Case "rbSampleStrain"
			dVal = dFil.dSplStrain_s2(iRow)
		Case "rbShearMod"
			dVal = dFil.dShearMod_s2(iRow)
		Case "rbShearVelo"
			dVal = dFil.dShearVelo_s2(iRow)
		Case "rbStorageMod"
			dVal = dFil.dXStrgMod_s2(iRow)
		Case "rbElasticMod"
			dVal = dFil.dXElasMod_s2(iRow)
		Case "rbLossTangent"
			dVal = dFil.dXLossTan_s2(iRow)
		Case "rbCorrectP"
			dVal = dFil.dHoseiP_s2(iRow)
		End Select
	Case 6
		Select Case sSelectGItem
		Case "rbCorrect"
			dVal = dFil.iHosei_s1(iRow)
		Case "rbHPTemp"
			dVal = dFil.iTempBuf_s1(iRow) / 10
		Case "rbHPMV"
			dVal = dFil.iMvBuf_s1(iRow) / 10
		Case "rbPBTemp"
			dVal = dFil.iTempBuf2_s1(iRow) / 10
		Case "rbPBMV"
			dVal = dFil.iMvBuf2_s1(iRow) / 10
		Case "rbViscosity"
			dVal = dFil.dViscosity_s1(iRow)
		Case "rbPosPhase"
			dVal = dFil.dXPosPhase_s1(iRow) * RadToDegRate
		Case "rbSampleStrain"
			dVal = dFil.dSplStrain_s1(iRow)
		Case "rbShearMod"
			dVal = dFil.dShearMod_s1(iRow)
		Case "rbShearVelo"
			dVal = dFil.dShearVelo_s1(iRow)
		Case "rbStorageMod"
			dVal = dFil.dXStrgMod_s1(iRow)
		Case "rbElasticMod"
			dVal = dFil.dXElasMod_s1(iRow)
		Case "rbLossTangent"
			dVal = dFil.dXLossTan_s1(iRow)
		Case "rbCorrectP"
			dVal = dFil.dHoseiP_s1(iRow)
		End Select
	Case 5
		Select Case sSelectGItem
		Case "rbCorrect"
			dVal = dFil.iHosei_h5(iRow)
		Case "rbHPTemp"
			dVal = dFil.iTempBuf_h5(iRow) / 10
		Case "rbHPMV"
			dVal = dFil.iMvBuf_h5(iRow) / 10
		Case "rbPBTemp"
			dVal = dFil.iTempBuf2_h5(iRow) / 10
		Case "rbPBMV"
			dVal = dFil.iMvBuf2_h5(iRow) / 10
		Case "rbViscosity"
			dVal = dFil.dViscosity_h5(iRow)
		Case "rbPosPhase"
			dVal = dFil.dXPosPhase_h5(iRow) * RadToDegRate
		Case "rbSampleStrain"
			dVal = dFil.dSplStrain_h5(iRow)
		Case "rbShearMod"
			dVal = dFil.dShearMod_h5(iRow)
		Case "rbShearVelo"
			dVal = dFil.dShearVelo_h5(iRow)
		Case "rbStorageMod"
			dVal = dFil.dXStrgMod_h5(iRow)
		Case "rbElasticMod"
			dVal = dFil.dXElasMod_h5(iRow)
		Case "rbLossTangent"
			dVal = dFil.dXLossTan_h5(iRow)
		Case "rbCorrectP"
			dVal = dFil.dHoseiP_h5(iRow)
		End Select
	Case 4
		Select Case sSelectGItem
		Case "rbCorrect"
			dVal = dFil.iHosei_h4(iRow)
		Case "rbHPTemp"
			dVal = dFil.iTempBuf_h4(iRow) / 10
		Case "rbHPMV"
			dVal = dFil.iMvBuf_h4(iRow) / 10
		Case "rbPBTemp"
			dVal = dFil.iTempBuf2_h4(iRow) / 10
		Case "rbPBMV"
			dVal = dFil.iMvBuf2_h4(iRow) / 10
		Case "rbViscosity"
			dVal = dFil.dViscosity_h4(iRow)
		Case "rbPosPhase"
			dVal = dFil.dXPosPhase_h4(iRow) * RadToDegRate
		Case "rbSampleStrain"
			dVal = dFil.dSplStrain_h4(iRow)
		Case "rbShearMod"
			dVal = dFil.dShearMod_h4(iRow)
		Case "rbShearVelo"
			dVal = dFil.dShearVelo_h4(iRow)
		Case "rbStorageMod"
			dVal = dFil.dXStrgMod_h4(iRow)
		Case "rbElasticMod"
			dVal = dFil.dXElasMod_h4(iRow)
		Case "rbLossTangent"
			dVal = dFil.dXLossTan_h4(iRow)
		Case "rbCorrectP"
			dVal = dFil.dHoseiP_h4(iRow)
		End Select
	Case 3
		Select Case sSelectGItem
		Case "rbCorrect"
			dVal = dFil.iHosei_h3(iRow)
		Case "rbHPTemp"
			dVal = dFil.iTempBuf_h3(iRow) / 10
		Case "rbHPMV"
			dVal = dFil.iMvBuf_h3(iRow) / 10
		Case "rbPBTemp"
			dVal = dFil.iTempBuf2_h3(iRow) / 10
		Case "rbPBMV"
			dVal = dFil.iMvBuf2_h3(iRow) / 10
		Case "rbViscosity"
			dVal = dFil.dViscosity_h3(iRow)
		Case "rbPosPhase"
			dVal = dFil.dXPosPhase_h3(iRow) * RadToDegRate
		Case "rbSampleStrain"
			dVal = dFil.dSplStrain_h3(iRow)
		Case "rbShearMod"
			dVal = dFil.dShearMod_h3(iRow)
		Case "rbShearVelo"
			dVal = dFil.dShearVelo_h3(iRow)
		Case "rbStorageMod"
			dVal = dFil.dXStrgMod_h3(iRow)
		Case "rbElasticMod"
			dVal = dFil.dXElasMod_h3(iRow)
		Case "rbLossTangent"
			dVal = dFil.dXLossTan_h3(iRow)
		Case "rbCorrectP"
			dVal = dFil.dHoseiP_h3(iRow)
		End Select
	Case 2
		Select Case sSelectGItem
		Case "rbCorrect"
			dVal = dFil.iHosei_h2(iRow)
		Case "rbHPTemp"
			dVal = dFil.iTempBuf_h2(iRow) / 10
		Case "rbHPMV"
			dVal = dFil.iMvBuf_h2(iRow) / 10
		Case "rbPBTemp"
			dVal = dFil.iTempBuf2_h2(iRow) / 10
		Case "rbPBMV"
			dVal = dFil.iMvBuf2_h2(iRow) / 10
		Case "rbViscosity"
			dVal = dFil.dViscosity_h2(iRow)
		Case "rbPosPhase"
			dVal = dFil.dXPosPhase_h2(iRow) * RadToDegRate
		Case "rbSampleStrain"
			dVal = dFil.dSplStrain_h2(iRow)
		Case "rbShearMod"
			dVal = dFil.dShearMod_h2(iRow)
		Case "rbShearVelo"
			dVal = dFil.dShearVelo_h2(iRow)
		Case "rbStorageMod"
			dVal = dFil.dXStrgMod_h2(iRow)
		Case "rbElasticMod"
			dVal = dFil.dXElasMod_h2(iRow)
		Case "rbLossTangent"
			dVal = dFil.dXLossTan_h2(iRow)
		Case "rbCorrectP"
			dVal = dFil.dHoseiP_h2(iRow)
		End Select
	Case 1
		Select Case sSelectGItem
		Case "rbCorrect"
			dVal = dFil.iHosei_h1(iRow)
		Case "rbHPTemp"
			dVal = dFil.iTempBuf_h1(iRow) / 10
		Case "rbHPMV"
			dVal = dFil.iMvBuf_h1(iRow) / 10
		Case "rbPBTemp"
			dVal = dFil.iTempBuf2_h1(iRow) / 10
		Case "rbPBMV"
			dVal = dFil.iMvBuf2_h1(iRow) / 10
		Case "rbViscosity"
			dVal = dFil.dViscosity_h1(iRow)
		Case "rbPosPhase"
			dVal = dFil.dXPosPhase_h1(iRow) * RadToDegRate
		Case "rbSampleStrain"
			dVal = dFil.dSplStrain_h1(iRow)
		Case "rbShearMod"
			dVal = dFil.dShearMod_h1(iRow)
		Case "rbShearVelo"
			dVal = dFil.dShearVelo_h1(iRow)
		Case "rbStorageMod"
			dVal = dFil.dXStrgMod_h1(iRow)
		Case "rbElasticMod"
			dVal = dFil.dXElasMod_h1(iRow)
		Case "rbLossTangent"
			dVal = dFil.dXLossTan_h1(iRow)
		Case "rbCorrectP"
			dVal = dFil.dHoseiP_h1(iRow)
		End Select
#end if
	Case 0
		Select Case sSelectGItem
		Case "rbCorrect"
			dVal = dFil.iLT_Hosei(iRow)
		Case "rbHPTemp"
			dVal = dFil.iLT_TempBuf(iRow) / 10.0
		Case "rbHPMV"
			dVal = dFil.iLT_MvBuf(iRow) / 10
		Case "rbPBTemp"
			dVal = dFil.iLT_TempBuf2(iRow) / 10.0
		Case "rbPBMV"
			dVal = dFil.iLT_MvBuf2(iRow) / 10
		Case "rbViscosity"
			dVal = dFil.dLT_Viscosity(iRow)
		Case "rbPosPhase"
			dVal = dFil.dLT_XPosPhase(iRow) * RadToDegRate
		Case "rbSampleStrain"
			dVal = dFil.dLT_SplStrain(iRow)
		Case "rbShearMod"
			dVal = dFil.dLT_ShearMod(iRow)
		Case "rbShearVelo"
			dVal = dFil.dLT_ShearVelo(iRow)
		Case "rbStorageMod"
			dVal = dFil.dLT_XStrgMod(iRow)
		Case "rbElasticMod"
			dVal = dFil.dLT_XElasMod(iRow)
		Case "rbLossTangent"
			dVal = dFil.dLT_XLossTan(iRow)
		Case "rbCorrectP"
			dVal = dFil.dLT_Hosei100(iRow)
		End Select
	End Select
End Sub
''' <summary>
''' X,Y軸関係の設定処理 長期間欠モード
''' </summary>
''' <param name="arg"></param>
''' <param name="Chart1"></param>
''' <param name="dFil"></param>
''' <param name="clExt"></param>
''' <param name="cts"></param>
''' <param name="iHis"></param>
''' <param name="iStd"></param>
''' <param name="ChartGrid"></param>
''' <param name="ds"></param>
''' <param name="dTemp"></param>
''' <param name="dMv"></param>
''' <param name="iThinTyp"></param>
Private Sub LTChartXYAxisSetting( _
        ByVal arg	        As ChartArg, _
        ByRef Chart1        As Chart, _
        ByRef dFil          As DataFile, _
        ByRef clExt         As Extra, _
        ByVal cts           As CToolSettings, _
        ByVal iHis          As int16, _
        ByVal iStd          As int16, _
        ByVal ChartGrid     As Integer, _
        ByVal ds            As dataset, _
        ByVal dTemp()       As Double, _
        ByVal dMv()	        As Double, _ 
        ByVal iThinTyp      As Int16 _
)
Dim dInterval   As Double
Dim iInterval   As Int32
Dim GridColor   As Color    = cts.penGrids.Color
Dim dMax		As Double	= 100
Dim dMin		As Double	= 0

Try
	If arg.dTempMin=0 And arg.dTempMax=0
		arg.dTempMax = 300
	End If
    With Chart1
		.ChartAreas(0).Position.Auto = True
        .ChartAreas(0).BackColor = cts.clrGraphBack                                 'プロット領域の背景色
        .ChartAreas(0).BorderColor = Color.DarkGray                                 'プロット領域の境界色
        .ChartAreas(0).BorderDashStyle = ChartDashStyle.Solid                       'プロット領域の境界線スタイル
        .ChartAreas(0).BorderWidth = 2                                              'プロット領域の境界線幅

        'X軸
        .ChartAreas(0).AxisX.Title                      = If(cts.iChartType = 0, m_sMes(efrmMainSTR.iCHART_X_TIME_LT) , m_sMes(efrmMainSTR.iCHART_X_TEMP))
        .ChartAreas(0).AxisX.TitleFont                  = New Font(BASE_FONT, 16, FontStyle.Regular)
        .ChartAreas(0).AxisX.Crossing                   = Double.NaN
	    .ChartAreas(0).AxisX.IntervalType               = DateTimeIntervalType.NotSet
		.ChartAreas(0).AxisX.MajorGrid.Enabled          = True
		.ChartAreas(0).AxisX.MajorGrid.LineDashStyle    = ChartGrid
        .ChartAreas(0).AxisX.MajorGrid.LineColor        = GridColor
        .ChartAreas(0).AxisX.LabelStyle.Format			= "F1"

        'Dim a As Double
        If IsNothing(dFil.iLT_Hosei) And iHis=0 And iStd=0
		'初期画面時
			Select Case cts.iChartType 
			Case 0	'時間
				If cts.iRTXAxis = 0
				'自動
					dMax = cInitXSec
				Else
				'固定
					dMax = cts.MainGraphXMax(cts.iChartType)
				End If
				dMin = cts.MainGraphXMin(cts.iChartType)
				dInterval = (dMax-(dMin-1)) \ cts.MainGraphXDiv(cts.iChartType)			'目盛り数値
			Case 1  '温度
				If arg.dTempMax<arg.dTempMin
					dMax = cts.MainGraphXMin(cts.iChartType)
					dMin = cts.MainGraphXMax(cts.iChartType)
				Else
					If cts.iRTXAxis = 0
					'自動
						dMax = arg.dTempMax
						dMin = arg.dTempMin
						dInterval = (dMax-dMin) / cts.MainGraphXDiv(cts.iChartType)		
					Else
						dMax = cts.MainGraphXMax(cts.iChartType)
						dMin = cts.MainGraphXMin(cts.iChartType)
						dInterval = (dMax-dMin) / cts.MainGraphXDiv(cts.iChartType)		
					End if
				End If
			End Select
            .ChartAreas(0).AxisX.Maximum	= dMax
            .ChartAreas(0).AxisX.Minimum	= dMin
	        .ChartAreas(0).AxisX.Interval	= dInterval
		Else
		'表示データある時
            If cts.iRTXAxis = 0
            'Auto
				Select Case cts.iChartType 
				Case 0	'時間
					If Not IsNothing(dFil.iLT_MSec)
						If p_iScrlStart > 0 And p_iScrlStart < dFil.iLT_MSec.Length
							dMin = dFil.iLT_MSec(p_iScrlStart) / 60
							dMax = dMin + (dFil.iLT_MSec.Max-dFil.iLT_MSec(p_iScrlStart)) / 60
						Else
							dMin = 0
							dMax = dFil.iLT_MSec.Max / 60
						End If
					Else
						Dim iMax As int64 = 0
						iMax = GetArrayMaxInHisStd(iHis, iStd, dFil)
						dMax = iMax
					End If
					'dMax \= 10
					'dMax *= 10
					'iInterval = (dMax-dMin) \ 100
					'iInterval *= 10
					iInterval = (dMax-dMin) / 10

					.ChartAreas(0).AxisX.Interval = iInterval

					.ChartAreas(0).AxisX.Maximum = dMax
					.ChartAreas(0).AxisX.Minimum = dMin
				Case 1	'温度
					If Not IsNothing(dFil.iLT_TempBuf)
					'測定値データがある
						dMax = dFil.iLT_TempBuf.Max/10
						dMin = Math.Ceiling(dFil.iLT_TempBuf.Min/10/10 * 10)
						Dim dMaxH As Double = 0.0
						Select Case dMax
						Case < 50
							dMaxH = 50
						Case < 80
							dMaxH = 80
						Case < 100
							dMaxH = 100
						Case < 120
							dMaxH = 120
						Case < 140
							dMaxH = 140
						Case < 160
							dMaxH = 160
						Case < 180
							dMaxH = 180
						Case Else
							dMaxH = Math.Ceiling(dMax/10) * 10
						End Select
						dMax = dMaxH
					Else
					'測定値データがない
						'履歴・基準データから温度の最大最小を得る
						Dim iMax As Int32 = -99999
						Dim iMin As Int32 = 99999
						GetTempMaxInHisStd(iMax, iMin, dFil)
						dMax = iMax / 10
						dMin = iMin / 10
					End If
					If dMax = dMin Or Double.IsNaN(dMin) 
						.ChartAreas(0).AxisX.Maximum = dMax
						.ChartAreas(0).AxisX.Minimum = 0
					ElseIf dMax = 0 
						.ChartAreas(0).AxisX.Maximum = 250
						.ChartAreas(0).AxisX.Minimum = 0
					ElseIf dMax - dMin <> 0
						.ChartAreas(0).AxisX.Maximum = dMax
						.ChartAreas(0).AxisX.Minimum = dMin
					End If

				End Select
            Else
            'Fixed
				dMax = cts.MainGraphXMax(cts.iChartType)
				dMin = cts.MainGraphXMin(cts.iChartType)
				iInterval = (dMax-(dMin-1)) \ cts.MainGraphXDiv(cts.iChartType)		'目盛りインターバル数値
				.ChartAreas(0).AxisX.Maximum = dMax
				.ChartAreas(0).AxisX.Minimum = dMin
				.ChartAreas(0).AxisX.Interval = iInterval
            End If
			.ChartAreas(0).AxisX.LabelStyle.Format			= "F0"
			.ChartAreas(0).AxisX.LabelStyle.Font    =  New Font(BASE_FONT, 12, FontStyle.Regular)
        End If

		.ChartAreas(0).AxisX.ScaleView.Zoomable = False


        'Y軸
		'Y軸スパン計算
        LTAxisMaxMinSetting( _
			arg, _
			Chart1, _
			dFil, _
			cts, _
			iThinTyp, _
			iHis, _
			iStd, _
			dMax, _
			dMin
		)
Try
		If Double.IsInfinity(dMax) Or Double.IsInfinity(dMin) Or dMax = dMin
			dMax = 1000
			dMin = 0
		End If
		.ChartAreas(0).AxisY.Maximum			= dMax
		.ChartAreas(0).AxisY.Minimum			= dMin
Catch ex As System.OverflowException
		Call WriteLog("オーバーフローエラー グラフ表示スパン MAX:" & dMax & " - MIN:" & dMin,eLOG_LEVEL.iFATAL)
End Try		

		Select Case arg.sSelectGItem
		Case "rbXStress"
	        .ChartAreas(0).AxisY.Title          = sChartItemName(eChartItem.OxBuf) & sChartItemUnit(eChartItem.OxBuf)
		Case "rbYStress"
	        .ChartAreas(0).AxisY.Title          = sChartItemName(eChartItem.OyBuf) & sChartItemUnit(eChartItem.OyBuf)
		Case "rbZStress"
	        .ChartAreas(0).AxisY.Title          = sChartItemName(eChartItem.OzBuf) & sChartItemUnit(eChartItem.OzBuf)
		Case "rbZPosition"
	        .ChartAreas(0).AxisY.Title          = sChartItemName(eChartItem.OzPos) & sChartItemUnit(eChartItem.OzPos)
		Case "rbHPTemp"
	        .ChartAreas(0).AxisY.Title          = sChartItemName(eChartItem.TempBuf) & sChartItemUnit(eChartItem.TempBuf)
		Case "rbPBTemp"
	        .ChartAreas(0).AxisY.Title          = sChartItemName(eChartItem.TempBuf2) & sChartItemUnit(eChartItem.TempBuf2)
		Case "rbHPMV"
	        .ChartAreas(0).AxisY.Title          = sChartItemName(eChartItem.MvBuf) & sChartItemUnit(eChartItem.MvBuf)
		Case "rbPBMV"
	        .ChartAreas(0).AxisY.Title          = sChartItemName(eChartItem.MvBuf2) & sChartItemUnit(eChartItem.MvBuf2)
        Case "rbCorrect"
	        .ChartAreas(0).AxisY.Title          = sChartItemName(eChartItem.Hosei) & sChartItemUnit(eChartItem.Hosei)
        Case "rbCorrectP"
	        .ChartAreas(0).AxisY.Title          = sChartItemName(eChartItem.HoseiP) & sChartItemUnit(eChartItem.HoseiP)
        Case "rbViscosity"
	        .ChartAreas(0).AxisY.Title          = sChartItemName(eChartItem.Viscosity) & sChartItemUnit(eChartItem.Viscosity)
        Case "rbPosPhase"
	        .ChartAreas(0).AxisY.Title          = sChartItemName(eChartItem.XPosPhase) & sChartItemUnit(eChartItem.XPosPhase)
        Case "rbSampleStrain"
	        .ChartAreas(0).AxisY.Title          = sChartItemName(eChartItem.SplStrain) & sChartItemUnit(eChartItem.SplStrain)
        Case "rbShearMod"
	        .ChartAreas(0).AxisY.Title          = sChartItemName(eChartItem.ShearMod) & sChartItemUnit(eChartItem.ShearMod)
        Case "rbShearVelo"
	        .ChartAreas(0).AxisY.Title          = sChartItemName(eChartItem.ShearVelo) & sChartItemUnit(eChartItem.ShearVelo)
        Case "rbStorageMod"
	        .ChartAreas(0).AxisY.Title          = sChartItemName(eChartItem.XStrgMod) & sChartItemUnit(eChartItem.XStrgMod)
        Case "rbElasticMod"
	        .ChartAreas(0).AxisY.Title          = sChartItemName(eChartItem.XElasMod) & sChartItemUnit(eChartItem.XElasMod)
        Case "rbLossTangent"
	        .ChartAreas(0).AxisY.Title          = sChartItemName(eChartItem.XLossTan) & sChartItemUnit(eChartItem.XLossTan)
		Case Else
	        .ChartAreas(0).AxisY.Title          = "N/A"
		End Select
        .ChartAreas(0).AxisY.TitleFont          =  New Font(BASE_FONT, 16, FontStyle.Regular)
        .ChartAreas(0).AxisY.TextOrientation    = TextOrientation.Rotated270       'テキストの向き
		.ChartAreas(0).AxisY.Crossing           = Double.NaN
		.ChartAreas(0).AxisY.LabelStyle.Font    =  New Font(BASE_FONT, 12, FontStyle.Regular)

		If cts.iRTYAxis = 0
		'自動
			.ChartAreas(0).AxisY.IntervalAutoMode   = IntervalAutoMode.FixedCount
		Else
		'固定
			dInterval								= (.ChartAreas(0).AxisY.Maximum-.ChartAreas(0).AxisY.Minimum) / cts.MainGraphYDiv(arg.iSelectGItem)
			Dim sInterval As String = dInterval.ToString()
			Dim idx As Int16 = sInterval.IndexOf(".")
			Dim i As int32
			Select Case idx
			Case <=1	'1以下
				For i=idx+1 To sInterval.Length-1
					If sInterval.Substring(i,1)<>"0"
						Exit for
					End If
				Next
				.ChartAreas(0).AxisY.LabelStyle.Format	= "F" & i.ToString
			Case Else
				.ChartAreas(0).AxisY.LabelStyle.Format	= "F0"
			End Select
			.ChartAreas(0).AxisY.Interval			= dInterval

		End If
		.ChartAreas(0).AxisY.Intervaltype       = DateTimeIntervalType.Number
#If 1
		'第二Y軸がある場合は第二側のグリッド線を引く
        Select Case arg.sCalcMethod
        Case lstCalcMethod(eCalcMethod.THR), lstCalcMethod(eCalcMethod.EXTRA)
			.ChartAreas(0).AxisY.MajorGrid.Enabled          = True
			.ChartAreas(0).AxisY.MajorGrid.LineDashStyle    = ChartGrid
			.ChartAreas(0).AxisY.MajorGrid.LineColor        = GridColor
			.ChartAreas(0).AxisY2.MajorGrid.Enabled         = False

        Case lstCalcMethod(eCalcMethod.SLOPE)
			If arg.sSelectGItem = "rbCorrect"
			'合成応力シリーズの時のみ
				.ChartAreas(0).AxisY.MajorGrid.Enabled          = True
				.ChartAreas(0).AxisY.MajorGrid.LineDashStyle    = ChartDashStyle.NotSet
				.ChartAreas(0).AxisY.MajorGrid.LineColor        = Chart1.ChartAreas(0).BackColor

				.ChartAreas(0).AxisY2.MajorGrid.Enabled         = True
				.ChartAreas(0).AxisY2.MajorGrid.LineDashStyle   = ChartGrid
				.ChartAreas(0).AxisY2.MajorGrid.LineColor       = GridColor
			Else
				.ChartAreas(0).AxisY.MajorGrid.Enabled          = True
				.ChartAreas(0).AxisY.MajorGrid.LineDashStyle    = ChartGrid
				.ChartAreas(0).AxisY.MajorGrid.LineColor        = GridColor
				.ChartAreas(0).AxisY2.MajorGrid.Enabled         = False
			End If
			.ChartAreas(0).AxisY2.LabelStyle.Font    =  New Font(BASE_FONT, 12, FontStyle.Regular)
		Case Else
			.ChartAreas(0).AxisY.MajorGrid.Enabled          = True
			.ChartAreas(0).AxisY.MajorGrid.LineDashStyle    = ChartGrid
			.ChartAreas(0).AxisY.MajorGrid.LineColor        = GridColor
			.ChartAreas(0).AxisY2.MajorGrid.Enabled         = False
			.ChartAreas(0).AxisY2.LabelStyle.Font    =  New Font(BASE_FONT, 12, FontStyle.Regular)
		End Select
#end if
		.ChartAreas(0).AxisY.ScaleView.Zoomable         = True




 'Debug.print( "AxisY.Maximum = " & Chart1.ChartAreas(0).AxisY.Maximum )
		End With
Catch ex As Exception
    Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Call WriteLog("Graph display span MAX:" & dMax & " - MIN:" & dMin,eLOG_LEVEL.iFATAL)
    Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub

''' <summary>
''' Y軸のスパン情報の設定 LTM
''' </summary>
''' <param name="arg"></param>
''' <param name="Chart1"></param>
''' <param name="dFil"></param>
''' <param name="cts"></param>
''' <param name="iThinTyp"></param>
''' <param name="iHis"></param>
''' <param name="iStd"></param>
Private Sub LTAxisMaxMinSetting( _
								ByVal arg			As ChartArg, _
                                ByRef Chart1        As Chart, _
                                ByRef dFil          As DataFile, _
                                ByVal cts           As CToolSettings, _
								ByVal iThinTyp		As Int16, _ 
                                ByVal iHis          As int16, _
                                ByVal iStd          As int16, _
								ByRef dGMax			As Double, _
								ByRef dGMin			As Double _
                                )
Dim iAryMax(8) As int64				'最大値配列 全て(測定中＋履歴)
Dim iAryMin(8) As int64				'最小値配列 全て(測定中＋履歴)
Dim dAryMax(8) As Double			'最大値配列 全て(測定中＋履歴)
Dim dAryMin(8) As Double			'最小値配列 全て(測定中＋履歴)
Dim dHisMax As Double                'Y軸AutoFitのための最大値
Dim dHisMin As Double                'Y軸AutoFitのための最小値
Dim iStdBase As Int32   = cHistMax+1
Dim iInterval As int32 

Try
	For i As Int32 = 0 To iAryMax.GetUpperBound(0)
		iAryMax(i) = Int64.MinValue
		iAryMin(i) = Int64.MaxValue
		dAryMax(i) = Double.MinValue
		dAryMin(i) = Double.MaxValue
	Next
    With Chart1
	'Y軸の最大最小設定 -----------------------
    Select case arg.sSelectGItem
	Case "rbCorrect"
        If cts.iRTYAxis = 1
        'Y軸スパン 固定
			dHisMax = cts.MainGraphYMax(arg.iSelectGItem) * (arg.dYSpanPer / 100.0)
            dHisMin = cts.MainGraphYMin(arg.iSelectGItem)
        Else
            If Not IsNothing(dFil.iLT_Hosei)
		        dAryMax(0) = dFil.iLT_Hosei.Max
		        dAryMin(0) = dFil.iLT_Hosei.Min
            Else
		        dAryMax(0) = 1000
		        dAryMin(0) = 0
            End If
            For j = 1 To iHis
                Select Case j
                Case 1
        		    dAryMax(j) = dFil.iHosei_h1.Max
        		    dAryMin(j) = dFil.iHosei_h1.Min
                Case 2
        		    dAryMax(j) = dFil.iHosei_h2.Max
        		    dAryMin(j) = dFil.iHosei_h2.Min
                Case 3
        		    dAryMax(j) = dFil.iHosei_h3.Max
        		    dAryMin(j) = dFil.iHosei_h3.Min
                Case 4
        		    dAryMax(j) = dFil.iHosei_h4.Max
        		    dAryMin(j) = dFil.iHosei_h4.Min
                Case 5
        		    dAryMax(j) = dFil.iHosei_h5.Max
        		    dAryMin(j) = dFil.iHosei_h5.Min
                End Select
            Next
			For j = iStdBase To iStdBase - 1 + iStd
				Select Case j
                Case 6
					If Not IsNothing(dFil.iHosei_s1)
        				dAryMax(j) = dFil.iHosei_s1.Max
        				dAryMin(j) = dFil.iHosei_s1.Min
					End If
                Case 7
					If Not IsNothing(dFil.iHosei_s2)
        				dAryMax(j) = dFil.iHosei_s2.Max
        				dAryMin(j) = dFil.iHosei_s2.Min
					End If
				End Select
			Next
			dHisMax = dAryMax.Max
			dHisMin = dAryMin.Min
			Dim dMaxH As Double = 0.0
			Select Case dHisMax
			Case < 100
				dMaxH = 100
			Case < 1000
				dMaxH = 1000
			Case < 10000
				dMaxH = 10000
			Case Else
				dMaxH = Math.Ceiling(dHisMax/10000) * 10000
			End Select
			dHisMax = dMaxH
        End if
		If dHisMax = dHisMin Or Double.IsNaN(dHisMin)
			dGMax = dHisMax
			dGMin = 0
		ElseIf dHisMax = 0 
			dGMax = 1000
			dGMin = 0
		ElseIf dHisMax - dHisMin <> 0
			dGMax = dHisMax
			dGMin = dHisMin
		End If
		If cts.iRTYAxis = 1
			.ChartAreas(0).AxisY.Interval = dHisMax  / 10
		Else
			.ChartAreas(0).AxisY.Interval = 0
		End If
		.ChartAreas(0).AxisY.LabelStyle.Format			= "F0"


	Case "rbCorrectP"
        If cts.iRTYAxis = 1
        'Y軸スパン 固定
         '   dHisMax = 100 * (arg.dYSpanPer / 100.0)
	        'dHisMin = 0
			dHisMax = cts.MainGraphYMax(arg.iSelectGItem) * (arg.dYSpanPer / 100.0)
            dHisMin = cts.MainGraphYMin(arg.iSelectGItem)
        Else
            If Not IsNothing(dFil.dLT_Hosei100)
		        dAryMax(0) = dFil.dLT_Hosei100.Max
		        dAryMin(0) = dFil.dLT_Hosei100.Min
            Else
		        dAryMax(0) = 100
		        dAryMin(0) = 0
            End If
            For j = 1 To iHis
                Select Case j
                Case 1
        		    dAryMax(j) = dFil.dHoseiP_h1.Max
        		    dAryMin(j) = dFil.dHoseiP_h1.Min
                Case 2
        		    dAryMax(j) = dFil.dHoseiP_h2.Max
        		    dAryMin(j) = dFil.dHoseiP_h2.Min
                Case 3
        		    dAryMax(j) = dFil.dHoseiP_h3.Max
        		    dAryMin(j) = dFil.dHoseiP_h3.Min
                Case 4
        		    dAryMax(j) = dFil.dHoseiP_h4.Max
        		    dAryMin(j) = dFil.dHoseiP_h4.Min
                Case 5
        		    dAryMax(j) = dFil.dHoseiP_h5.Max
        		    dAryMin(j) = dFil.dHoseiP_h5.Min
                End Select
            Next
			For j = iStdBase To iStdBase - 1 + iStd
				Select Case j
                Case 6
					If Not IsNothing(dFil.dHoseiP_s1)
        				dAryMax(j) = dFil.dHoseiP_s1.Max
        				dAryMin(j) = dFil.dHoseiP_s1.Min
					End If
                Case 7
					If Not IsNothing(dFil.dHoseiP_s2)
        				dAryMax(j) = dFil.dHoseiP_s2.Max
        				dAryMin(j) = dFil.dHoseiP_s2.Min
					End If
				End Select
			Next
			dHisMax = dAryMax.Max
			dHisMin = If(dAryMin.Min<0,0,dAryMin.Min)
 			Dim dMaxH As Double = 0.0
			Select Case dHisMax
			Case <0.1
				dMaxH = 0.1
			Case <1
				dMaxH = 1
			Case <10
				dMaxH = 10
			Case Else
				dMaxH = Math.Ceiling(dHisMax/10) * 10
			End Select
			dHisMax = dMaxH
		End if
		If dHisMax = dHisMin Or Double.IsNaN(dHisMin)
			dGMax = dHisMax
			dGMin = 0
		ElseIf dHisMax = 0 
			dGMax = 0.1
			dGMin = 0
		ElseIf dHisMax - dHisMin <> 0
			dGMax = dHisMax
			dGMin = dHisMin
		ElseIf Double.IsInfinity(dHisMax)
			dGMax = dHisMax
			dGMin = dHisMin
		End If
		If cts.iRTYAxis = 1
			.ChartAreas(0).AxisY.Interval = 10.0 * (arg.dYSpanPer / 100.0)
		Else
			.ChartAreas(0).AxisY.Interval = 0
		End If
		.ChartAreas(0).AxisY.LabelStyle.Format			= "F2"

	Case "rbViscosity"
        If cts.iRTYAxis = 1
        'Y軸スパン 固定
			'dHisMax = 10000 * (arg.dYSpanPer / 100.0)
			'dHisMin = 0
			dHisMax = cts.MainGraphYMax(arg.iSelectGItem) * (arg.dYSpanPer / 100.0)
			dHisMin = cts.MainGraphYMin(arg.iSelectGItem)
        Else
            If Not IsNothing(dFil.dLT_Viscosity)
		        dAryMax(0) = dFil.dLT_Viscosity.Max
		        dAryMin(0) = dFil.dLT_Viscosity.Min
            Else
		        dAryMax(0) = 10000
		        dAryMin(0) = 0
            End If
			For j = 1 To iHis
				Select Case j
				Case 1
        			dAryMax(j) = dFil.dViscosity_h1.Max
        			dAryMin(j) = dFil.dViscosity_h1.Min
				Case 2
        			dAryMax(j) = dFil.dViscosity_h2.Max
        			dAryMin(j) = dFil.dViscosity_h2.Min
				Case 3
        			dAryMax(j) = dFil.dViscosity_h3.Max
        			dAryMin(j) = dFil.dViscosity_h3.Min
				Case 4
        			dAryMax(j) = dFil.dViscosity_h4.Max
        			dAryMin(j) = dFil.dViscosity_h4.Min
				Case 5
        			dAryMax(j) = dFil.dViscosity_h5.Max
        			dAryMin(j) = dFil.dViscosity_h5.Min
				End Select
			Next
			For j = iStdBase To iStdBase - 1 + iStd
				Select Case j
				Case 6
					If Not IsNothing(dFil.dViscosity_s1)
        				dAryMax(j) = dFil.dViscosity_s1.Max
        				dAryMin(j) = dFil.dViscosity_s1.Min
					End If
				Case 7
					If Not IsNothing(dFil.dViscosity_s2)
        				dAryMax(j) = dFil.dViscosity_s2.Max
        				dAryMin(j) = dFil.dViscosity_s2.Min
					End If
				End Select
			Next
			dHisMax = If(dAryMax.Max = 0, 10000, dAryMax.Max)
			dHisMin = If(dAryMin.Min<0,0,dAryMin.Min)
 			Dim dMaxH As Double = 0.0
			Select Case dHisMax
			Case <1
				dMaxH = 1
			Case <10
				dMaxH = 10
			Case <100
				dMaxH = 100
			Case Else
				dMaxH = Math.Ceiling(dHisMax/100) * 100
			End Select
			dHisMax = dMaxH
		End if
		If dHisMax = dHisMin Or Double.IsNaN(dHisMin)
			dGMax = dHisMax
			dGMin = 0
		ElseIf dHisMax = 0 
			dGMax = 1
			dGMin = 0
		ElseIf dHisMax - dHisMin <> 0
			dGMax = dHisMax
			dGMin = dHisMin
		End If

		If cts.iRTYAxis = 1
	    	.ChartAreas(0).AxisY.Interval = 1000.0 * (arg.dYSpanPer / 100.0)
		Else
    		.ChartAreas(0).AxisY.Interval = 0
		End If
		Select Case dHisMax
		Case <100
			.ChartAreas(0).AxisY.LabelStyle.Format			= "F1"
		Case Else
			.ChartAreas(0).AxisY.LabelStyle.Format			= "F0"
		End Select

	Case "rbPosPhase"
        If cts.iRTYAxis = 1
        'Y軸スパン 固定
            'dHisMax = 90.0 * (arg.dYSpanPer / 100.0)
            'dHisMin = 0
			dHisMax = cts.MainGraphYMax(arg.iSelectGItem) * (arg.dYSpanPer / 100.0)
			dHisMin = cts.MainGraphYMin(arg.iSelectGItem)
        Else
            If Not IsNothing(dFil.dLT_XPosPhase)
		        dAryMax(0) = dFil.dLT_XPosPhase.Max * RadToDegRate
		        dAryMin(0) = 0
            Else
		        dAryMax(0) = 90.0
		        dAryMin(0) = 0
            End If
			For j = 1 To iHis
				Select Case j
				Case 1
        			dAryMax(j) = dFil.dXPosPhase_h1.Max * RadToDegRate
        			dAryMin(j) = dFil.dXPosPhase_h1.Min * RadToDegRate
				Case 2
        			dAryMax(j) = dFil.dXPosPhase_h2.Max * RadToDegRate
        			dAryMin(j) = dFil.dXPosPhase_h2.Min * RadToDegRate
				Case 3
        			dAryMax(j) = dFil.dXPosPhase_h3.Max * RadToDegRate
        			dAryMin(j) = dFil.dXPosPhase_h3.Min * RadToDegRate
				Case 4
        			dAryMax(j) = dFil.dXPosPhase_h4.Max * RadToDegRate
        			dAryMin(j) = dFil.dXPosPhase_h4.Min * RadToDegRate
				Case 5
        			dAryMax(j) = dFil.dXPosPhase_h5.Max * RadToDegRate
        			dAryMin(j) = dFil.dXPosPhase_h5.Min * RadToDegRate
				End Select
			Next
			For j = iStdBase To iStdBase - 1 + iStd
				Select Case j
				Case 6
 					If Not IsNothing(dFil.dXPosPhase_s1)
		   				dAryMax(j) = dFil.dXPosPhase_s1.Max * RadToDegRate
        				dAryMin(j) = dFil.dXPosPhase_s1.Min * RadToDegRate
					End If
				Case 7
 					If Not IsNothing(dFil.dXPosPhase_s2)
	       				dAryMax(j) = dFil.dXPosPhase_s2.Max * RadToDegRate
        				dAryMin(j) = dFil.dXPosPhase_s2.Min * RadToDegRate
					End If
				End Select
			Next
			dHisMax = dAryMax.Max
			dHisMin = If(dAryMin.Min<0,0,dAryMin.Min)

 			Dim dMaxH As Double = 0.0
			If dHisMax < 1
				dMaxH = 1
			Else
				dMaxH = Math.Ceiling(dHisMax/10) * 10
			End If
			dHisMax = dMaxH
		End if
		If dHisMax = dHisMin Or Double.IsNaN(dHisMin)
			dGMax = dHisMax
			dGMin = 0
		ElseIf dHisMax = 0 
			dGMax = 1
			dGMin = 0
		ElseIf dHisMax - dHisMin <> 0
			dGMax = dHisMax
			dGMin = dHisMin
		End If
		If cts.iRTYAxis = 1
	    	.ChartAreas(0).AxisY.Interval = 10 * (arg.dYSpanPer / 100.0)
		Else
    		.ChartAreas(0).AxisY.Interval = If(iInterval<>-1, iInterval, 0)
		End If
		Select Case dHisMax
		Case <1
			.ChartAreas(0).AxisY.LabelStyle.Format			= "F2"
		Case <10
			.ChartAreas(0).AxisY.LabelStyle.Format			= "F1"
		Case Else
			.ChartAreas(0).AxisY.LabelStyle.Format			= "F0"
		End Select

	Case "rbSampleStrain"
        If cts.iRTYAxis = 1
        'Y軸スパン 固定
            'dHisMax = 10.0 * (arg.dYSpanPer / 100.0)
            'dHisMin = 0
			dHisMax = cts.MainGraphYMax(arg.iSelectGItem) * (arg.dYSpanPer / 100.0)
			dHisMin = cts.MainGraphYMin(arg.iSelectGItem)
        Else
            If Not IsNothing(dFil.dLT_SplStrain)
		        dAryMax(0) = dFil.dLT_SplStrain.Max
		        dAryMin(0) = dFil.dLT_SplStrain.Min
            Else
		        dAryMax(0) = 0.5
		        dAryMin(0) = 0
            End If
			For j = 1 To iHis
				Select Case j
				Case 1
        			dAryMax(j) = dFil.dSplStrain_h1.Max
        			dAryMin(j) = dFil.dSplStrain_h1.Min
				Case 2
        			dAryMax(j) = dFil.dSplStrain_h2.Max
        			dAryMin(j) = dFil.dSplStrain_h2.Min
				Case 3
        			dAryMax(j) = dFil.dSplStrain_h3.Max
        			dAryMin(j) = dFil.dSplStrain_h3.Min
				Case 4
        			dAryMax(j) = dFil.dSplStrain_h4.Max
        			dAryMin(j) = dFil.dSplStrain_h4.Min
				Case 5
        			dAryMax(j) = dFil.dSplStrain_h5.Max
        			dAryMin(j) = dFil.dSplStrain_h5.Min
				End Select
			Next
			For j = iStdBase To iStdBase - 1 + iStd
				Select Case j
				Case 6
  					If Not IsNothing(dFil.dSplStrain_s1)
	       				dAryMax(j) = dFil.dSplStrain_s1.Max
        				dAryMin(j) = dFil.dSplStrain_s1.Min
					End If
				Case 7
 					If Not IsNothing(dFil.dSplStrain_s2)
        				dAryMax(j) = dFil.dSplStrain_s2.Max
        				dAryMin(j) = dFil.dSplStrain_s2.Min
					End If
				End Select
			Next
			dHisMax = dAryMax.Max
			dHisMin = If(dAryMin.Min<0,0,dAryMin.Min)
 			Dim dMaxH As Double = 0.0
			Select Case dHisMax
			Case < 0.01
				dMaxH = 0.01
			Case < 0.1
				dMaxH = 0.1
			Case < 1
				dMaxH = Math.Ceiling(dHisMax/0.1) * 0.1
			Case Else
				dMaxH = Math.Ceiling(dHisMax/2) * 2
			End Select
			dHisMax = dMaxH
		End if
		If dHisMax = dHisMin Or Double.IsNaN(dHisMin)
			dGMax = dHisMax
			dGMin = 0
		ElseIf dHisMax = 0 
			dGMax = 1
			dGMin = 0
		ElseIf dHisMax - dHisMin <> 0
			dGMax = dHisMax
			dGMin = dHisMin
		End If

		If cts.iRTYAxis = 1
	    	.ChartAreas(0).AxisY.Interval = 1 * (arg.dYSpanPer / 100.0)
		Else
    		.ChartAreas(0).AxisY.Interval = 0
		End If
		Select Case dHisMax
		Case <= 0.01
			.ChartAreas(0).AxisY.LabelStyle.Format			= "F3"
		Case <= 0.1
			.ChartAreas(0).AxisY.LabelStyle.Format			= "F2"
		Case Else
			.ChartAreas(0).AxisY.LabelStyle.Format			= "F1"
		End Select

	Case "rbShearMod"
        If cts.iRTYAxis = 1
        'Y軸スパン 固定
            'dHisMax = 100000 * (arg.dYSpanPer / 100.0)
            'dHisMin = 0
			dHisMax = cts.MainGraphYMax(arg.iSelectGItem) * (arg.dYSpanPer / 100.0)
			dHisMin = cts.MainGraphYMin(arg.iSelectGItem)
        Else
            If Not IsNothing(dFil.dLT_ShearMod)
		        dAryMax(0) = dFil.dLT_ShearMod.Max
		        dAryMin(0) = dFil.dLT_ShearMod.Min
            Else
		        dAryMax(0) = 10000
		        dAryMin(0) = 0
            End If
			For j = 1 To iHis
				Select Case j
				Case 1
        			dAryMax(j) = dFil.dShearMod_h1.Max
        			dAryMin(j) = dFil.dShearMod_h1.Min
				Case 2
        			dAryMax(j) = dFil.dShearMod_h2.Max
        			dAryMin(j) = dFil.dShearMod_h2.Min
				Case 3
        			dAryMax(j) = dFil.dShearMod_h3.Max
        			dAryMin(j) = dFil.dShearMod_h3.Min
				Case 4
        			dAryMax(j) = dFil.dShearMod_h4.Max
        			dAryMin(j) = dFil.dShearMod_h4.Min
				Case 5
        			dAryMax(j) = dFil.dShearMod_h5.Max
        			dAryMin(j) = dFil.dShearMod_h5.Min
				End Select
			Next
			For j = iStdBase To iStdBase - 1 + iStd
				Select Case j
				Case 6
  					If Not IsNothing(dFil.dShearMod_s1)
        				dAryMax(j) = dFil.dShearMod_s1.Max
        				dAryMin(j) = dFil.dShearMod_s1.Min
					End If
				Case 7
  					If Not IsNothing(dFil.dShearMod_s2)
        				dAryMax(j) = dFil.dShearMod_s2.Max
        				dAryMin(j) = dFil.dShearMod_s2.Min
					End If
				End Select
			Next
			dHisMax = dAryMax.Max
			dHisMin = If(dAryMin.Min<0,0,dAryMin.Min)
 			Dim dMaxH As Double = 0.0
			Select Case dHisMax
			Case < 100
				dMaxH = 100
			Case < 1000
				dMaxH = 1000
			Case < 10000
				dMaxH = 10000
			Case Else
				dMaxH = Math.Ceiling(dHisMax/10000) * 10000
			End Select
			dHisMax = dMaxH
		End if
		If dHisMax = dHisMin Or Double.IsNaN(dHisMin)
			dGMax = dHisMax
			dGMin = 0
		ElseIf dHisMax = 0 
			dGMax = 1
			dGMin = 0
		ElseIf dHisMax - dHisMin <> 0
			dGMax = dHisMax
			dGMin = dHisMin
		End If

		If cts.iRTYAxis = 1
	    	.ChartAreas(0).AxisY.Interval = 10000 * (arg.dYSpanPer / 100.0)
		Else
    		.ChartAreas(0).AxisY.Interval = 0
		End If
		Select Case dHisMax
		Case <= 100
			.ChartAreas(0).AxisY.LabelStyle.Format			= "F2"
		Case <= 1000
			.ChartAreas(0).AxisY.LabelStyle.Format			= "F1"
		Case Else
			.ChartAreas(0).AxisY.LabelStyle.Format			= "F0"
		End Select

	Case "rbShearVelo"
        If cts.iRTYAxis = 1
        'Y軸スパン 固定
            'dHisMax = 100 * (arg.dYSpanPer / 100.0)
            'dHisMin = 0
			dHisMax = cts.MainGraphYMax(arg.iSelectGItem) * (arg.dYSpanPer / 100.0)
			dHisMin = cts.MainGraphYMin(arg.iSelectGItem)
        Else
            If Not IsNothing(dFil.dLT_ShearVelo)
		        dAryMax(0) = dFil.dLT_ShearVelo.Max
		        dAryMin(0) = dFil.dLT_ShearVelo.Min
            Else
		        dAryMax(0) = 10
		        dAryMin(0) = 0
            End If
			For j = 1 To iHis
				Select Case j
				Case 1
        			dAryMax(j) = dFil.dShearVelo_h1.Max
        			dAryMin(j) = dFil.dShearVelo_h1.Min
				Case 2
        			dAryMax(j) = dFil.dShearVelo_h2.Max
        			dAryMin(j) = dFil.dShearVelo_h2.Min
				Case 3
        			dAryMax(j) = dFil.dShearVelo_h3.Max
        			dAryMin(j) = dFil.dShearVelo_h3.Min
				Case 4
        			dAryMax(j) = dFil.dShearVelo_h4.Max
        			dAryMin(j) = dFil.dShearVelo_h4.Min
				Case 5
        			dAryMax(j) = dFil.dShearVelo_h5.Max
        			dAryMin(j) = dFil.dShearVelo_h5.Min
				End Select
			Next
			For j = iStdBase To iStdBase - 1 + iStd
				Select Case j
				Case 6
  					If Not IsNothing(dFil.dShearVelo_s1)
        				dAryMax(j) = dFil.dShearVelo_s1.Max
        				dAryMin(j) = dFil.dShearVelo_s1.Min
					End If
				Case 7
   					If Not IsNothing(dFil.dShearVelo_s2)
       					dAryMax(j) = dFil.dShearVelo_s2.Max
        				dAryMin(j) = dFil.dShearVelo_s2.Min
					End If
				End Select
			Next
			dHisMax = dAryMax.Max
			dHisMin = If(dAryMin.Min<0,0,dAryMin.Min)
 			Dim dMaxH As Double = 0.0
			Select Case dHisMax
			Case < 1
				dMaxH = 1
			Case < 10
				dMaxH = 10
			Case < 100
				dMaxH = Math.Ceiling(dHisMax/10) * 10
			Case Else
				dMaxH = Math.Ceiling(dHisMax/100) * 100
			End Select
			dHisMax = dMaxH
		End if
		If dHisMax = dHisMin Or Double.IsNaN(dHisMin)
			dGMax = dHisMax
			dGMin = 0
		ElseIf dHisMax = 0 
			dGMax = 1
			dGMin = 0
		ElseIf dHisMax - dHisMin <> 0
			dGMax = dHisMax
			dGMin = dHisMin
		End If

		If cts.iRTYAxis = 1
	    	.ChartAreas(0).AxisY.Interval = 10 * (arg.dYSpanPer / 100.0)
		Else
    		.ChartAreas(0).AxisY.Interval = 0
		End If
		Select Case dHisMax
		Case <= 100
			.ChartAreas(0).AxisY.LabelStyle.Format			= "F2"
		Case <= 1000
			.ChartAreas(0).AxisY.LabelStyle.Format			= "F1"
		Case Else
			.ChartAreas(0).AxisY.LabelStyle.Format			= "F0"
		End Select

	Case "rbStorageMod"
        If cts.iRTYAxis = 1
        'Y軸スパン 固定
            'dHisMax = 100000 * (arg.dYSpanPer / 100.0)
            'dHisMin = 0
			dHisMax = cts.MainGraphYMax(arg.iSelectGItem) * (arg.dYSpanPer / 100.0)
			dHisMin = cts.MainGraphYMin(arg.iSelectGItem)
        Else
            If Not IsNothing(dFil.dLT_XStrgMod)
		        dAryMax(0) = dFil.dLT_XStrgMod.Max
		        dAryMin(0) = dFil.dLT_XStrgMod.Min
            Else
		        dAryMax(0) = 10000
		        dAryMin(0) = 0
            End If
			For j = 1 To iHis
				Select Case j
				Case 1
        			dAryMax(j) = dFil.dXStrgMod_h1.Max
        			dAryMin(j) = dFil.dXStrgMod_h1.Min
				Case 2
        			dAryMax(j) = dFil.dXStrgMod_h2.Max
        			dAryMin(j) = dFil.dXStrgMod_h2.Min
				Case 3
        			dAryMax(j) = dFil.dXStrgMod_h3.Max
        			dAryMin(j) = dFil.dXStrgMod_h3.Min
				Case 4
        			dAryMax(j) = dFil.dXStrgMod_h4.Max
        			dAryMin(j) = dFil.dXStrgMod_h4.Min
				Case 5
        			dAryMax(j) = dFil.dXStrgMod_h5.Max
        			dAryMin(j) = dFil.dXStrgMod_h5.Min
				End Select
			Next
			For j = iStdBase To iStdBase - 1 + iStd
				Select Case j
				Case 6
  					If Not IsNothing(dFil.dXStrgMod_s1)
        				dAryMax(j) = dFil.dXStrgMod_s1.Max
        				dAryMin(j) = dFil.dXStrgMod_s1.Min
					End If
				Case 7
  					If Not IsNothing(dFil.dXStrgMod_s2)
        				dAryMax(j) = dFil.dXStrgMod_s2.Max
        				dAryMin(j) = dFil.dXStrgMod_s2.Min
					End If
				End Select
			Next
			dHisMax = dAryMax.Max
			dHisMin = If(dAryMin.Min<0,0,dAryMin.Min)
 			Dim dMaxH As Double = 0.0
			Select Case dHisMax
			Case < 100
				dMaxH = 100
			Case < 1000
				dMaxH = 1000
			Case < 10000
				dMaxH = 10000
			Case Else
				dMaxH = Math.Ceiling(dHisMax/10000) * 10000
			End Select
			dHisMax = dMaxH
		End if
		If dHisMax = dHisMin Or Double.IsNaN(dHisMin)
			dGMax = dHisMax
			dGMin = 0
		ElseIf dHisMax = 0 
			dGMax = 1
			dGMin = 0
		ElseIf dHisMax - dHisMin <> 0
			dGMax = dHisMax
			dGMin = dHisMin
		End If

		If cts.iRTYAxis = 1
	    	.ChartAreas(0).AxisY.Interval = 10000 * (arg.dYSpanPer / 100.0)
		Else
    		.ChartAreas(0).AxisY.Interval = 0
		End If
		Select Case dHisMax
		Case <= 100
			.ChartAreas(0).AxisY.LabelStyle.Format			= "F2"
		Case <= 1000
			.ChartAreas(0).AxisY.LabelStyle.Format			= "F1"
		Case Else
			.ChartAreas(0).AxisY.LabelStyle.Format			= "F0"
		End Select

	Case "rbElasticMod"
        If cts.iRTYAxis = 1
        'Y軸スパン 固定
            'dHisMax = 100000 * (arg.dYSpanPer / 100.0)
            'dHisMin = 0
			dHisMax = cts.MainGraphYMax(arg.iSelectGItem) * (arg.dYSpanPer / 100.0)
			dHisMin = cts.MainGraphYMin(arg.iSelectGItem)
        Else
            If Not IsNothing(dFil.dLT_XElasMod)
		        dAryMax(0) = dFil.dLT_XElasMod.Max
		        dAryMin(0) = dFil.dLT_XElasMod.Min
            Else
		        dAryMax(0) = 5000
		        dAryMin(0) = 0
            End If
			For j = 1 To iHis
				Select Case j
				Case 1
        			dAryMax(j) = dFil.dXElasMod_h1.Max
        			dAryMin(j) = dFil.dXElasMod_h1.Min
				Case 2
        			dAryMax(j) = dFil.dXElasMod_h2.Max
        			dAryMin(j) = dFil.dXElasMod_h2.Min
				Case 3
        			dAryMax(j) = dFil.dXElasMod_h3.Max
        			dAryMin(j) = dFil.dXElasMod_h3.Min
				Case 4
        			dAryMax(j) = dFil.dXElasMod_h4.Max
        			dAryMin(j) = dFil.dXElasMod_h4.Min
				Case 5
        			dAryMax(j) = dFil.dXElasMod_h5.Max
        			dAryMin(j) = dFil.dXElasMod_h5.Min
				End Select
			Next
			For j = iStdBase To iStdBase - 1 + iStd
				Select Case j
				Case 6
  					If Not IsNothing(dFil.dXElasMod_s1)
        				dAryMax(j) = dFil.dXElasMod_s1.Max
        				dAryMin(j) = dFil.dXElasMod_s1.Min
					End If
				Case 7
   					If Not IsNothing(dFil.dXElasMod_s2)
       					dAryMax(j) = dFil.dXElasMod_s2.Max
        				dAryMin(j) = dFil.dXElasMod_s2.Min
					End If
				End Select
			Next
			dHisMax = dAryMax.Max
			dHisMin = If(dAryMin.Min<0,0,dAryMin.Min)
 			Dim dMaxH As Double = 0.0
			Select Case dHisMax
			Case < 100
				dMaxH = 100
			Case < 1000
				dMaxH = 1000
			Case < 10000
				dMaxH = 10000
			Case Else
				dMaxH = Math.Ceiling(dHisMax/10000) * 10000
			End Select
			dHisMax = dMaxH
		End if
		If dHisMax = dHisMin Or Double.IsNaN(dHisMin)
			dGMax = dHisMax
			dGMin = 0
		ElseIf dHisMax = 0 
			dGMax = 1
			dGMin = 0
		ElseIf dHisMax - dHisMin <> 0
			dGMax = dHisMax
			dGMin = dHisMin
		End If

		If cts.iRTYAxis = 1
	    	.ChartAreas(0).AxisY.Interval = 10000 * (arg.dYSpanPer / 100.0)
		Else
    		.ChartAreas(0).AxisY.Interval = 0
		End If
		Select Case dHisMax
		Case <= 100
			.ChartAreas(0).AxisY.LabelStyle.Format			= "F2"
		Case <= 1000
			.ChartAreas(0).AxisY.LabelStyle.Format			= "F1"
		Case Else
			.ChartAreas(0).AxisY.LabelStyle.Format			= "F0"
		End Select

	Case "rbLossTangent"
        If cts.iRTYAxis = 1
        'Y軸スパン 固定
            'dHisMax = 100 * (arg.dYSpanPer / 100.0)
            'dHisMin = 0
			dHisMax = cts.MainGraphYMax(arg.iSelectGItem) * (arg.dYSpanPer / 100.0)
			dHisMin = cts.MainGraphYMin(arg.iSelectGItem)
        Else
            If Not IsNothing(dFil.dLT_XLossTan)
		        dAryMax(0) = dFil.dLT_XLossTan.Max
		        dAryMin(0) = dFil.dLT_XLossTan.Min
            Else
		        dAryMax(0) = 10
		        dAryMin(0) = 0
            End If
			For j = 1 To iHis
				Select Case j
				Case 1
        			dAryMax(j) = dFil.dXLossTan_h1.Max
        			dAryMin(j) = dFil.dXLossTan_h1.Min
				Case 2
        			dAryMax(j) = dFil.dXLossTan_h2.Max
        			dAryMin(j) = dFil.dXLossTan_h2.Min
				Case 3
        			dAryMax(j) = dFil.dXLossTan_h3.Max
        			dAryMin(j) = dFil.dXLossTan_h3.Min
				Case 4
        			dAryMax(j) = dFil.dXLossTan_h4.Max
        			dAryMin(j) = dFil.dXLossTan_h4.Min
				Case 5
        			dAryMax(j) = dFil.dXLossTan_h5.Max
        			dAryMin(j) = dFil.dXLossTan_h5.Min
				End Select
			Next
			For j = iStdBase To iStdBase - 1 + iStd
				Select Case j
				Case 6
  					If Not IsNothing(dFil.dXLossTan_s1)
        				dAryMax(j) = dFil.dXLossTan_s1.Max
        				dAryMin(j) = dFil.dXLossTan_s1.Min
					End If
				Case 7
   					If Not IsNothing(dFil.dXLossTan_s2)
       					dAryMax(j) = dFil.dXLossTan_s2.Max
        				dAryMin(j) = dFil.dXLossTan_s2.Min
					End If
				End Select
			Next
			dHisMax = dAryMax.Max
			dHisMin = If(dAryMin.Min<0,0,dAryMin.Min)
 			Dim dMaxH As Double = 0.0
			Select Case dHisMax
			Case < 1
				dMaxH = 1
			Case < 10
				dMaxH = 10
			Case < 100
				dMaxH = Math.Ceiling(dHisMax/10) * 10
			Case Else
				dMaxH = Math.Ceiling(dHisMax/100) * 100
			End Select
			dHisMax = dMaxH
		End if
		If dHisMax = dHisMin Or Double.IsNaN(dHisMin)
			dGMax = dHisMax
			dGMin = 0
		ElseIf dHisMax = 0 
			dGMax = 1
			dGMin = 0
		ElseIf dHisMax - dHisMin <> 0
			dGMax = dHisMax
			dGMin = dHisMin
		End If

		If cts.iRTYAxis = 1
	    	.ChartAreas(0).AxisY.Interval = 10 * (arg.dYSpanPer / 100.0)
		Else
    		.ChartAreas(0).AxisY.Interval = 0
		End If
		Select Case dHisMax
		Case <= 100
			.ChartAreas(0).AxisY.LabelStyle.Format			= "F2"
		Case <= 1000
			.ChartAreas(0).AxisY.LabelStyle.Format			= "F1"
		Case Else
			.ChartAreas(0).AxisY.LabelStyle.Format			= "F0"
		End Select

	Case "rbHPTemp"
         If cts.iRTYAxis = 1
        'Y軸スパン 固定
            'dHisMax = 2000 * (arg.dYSpanPer / 100.0)
            'dHisMin = 0
			dHisMax = cts.MainGraphYMax(arg.iSelectGItem) * 10 * (arg.dYSpanPer / 100.0)
			dHisMin = cts.MainGraphYMin(arg.iSelectGItem) * 10
        Else
		   If Not IsNothing(dFil.iLT_TempBuf)
				iAryMax(0) = dFil.iLT_TempBuf.Max
				iAryMin(0) = dFil.iLT_TempBuf.Min
			Else
				iAryMax(0) = 300.0
				iAryMin(0) = 0.0
			End If
			For j = 1 To iHis
				Select Case j
				Case 1
        			iAryMax(j) = dFil.iTempBuf_h1.Max
        			iAryMin(j) = dFil.iTempBuf_h1.Min
				Case 2
        			iAryMax(j) = dFil.iTempBuf_h2.Max
        			iAryMin(j) = dFil.iTempBuf_h2.Min
				Case 3
        			iAryMax(j) = dFil.iTempBuf_h3.Max
        			iAryMin(j) = dFil.iTempBuf_h3.Min
				Case 4
        			iAryMax(j) = dFil.iTempBuf_h4.Max
        			iAryMin(j) = dFil.iTempBuf_h4.Min
				Case 5
        			iAryMax(j) = dFil.iTempBuf_h5.Max
        			iAryMin(j) = dFil.iTempBuf_h5.Min
				End Select
			Next
			For j = iStdBase To iStdBase - 1 + iStd
				Select Case j
				Case 6
  					If Not IsNothing(dFil.iTempBuf_s1)
        				iAryMax(j) = dFil.iTempBuf_s1.Max
        				iAryMin(j) = dFil.iTempBuf_s1.Min
					End If
				Case 7
   					If Not IsNothing(dFil.iTempBuf_s2)
       					iAryMax(j) = dFil.iTempBuf_s2.Max
        				iAryMin(j) = dFil.iTempBuf_s2.Min
					End If
				End Select
			Next
			dHisMax = iAryMax.Max
			dHisMin = iAryMin.Min
 			Dim dMaxH As Double = 0.0
 			Dim dMinH As Double = 0.0
			dMaxH = Math.Ceiling(dHisMax/100) * 100
			dMinH = Math.Floor(dHisMin/100) * 100
			dHisMax = dMaxH
			dHisMin = dMinH
		End if
		If dHisMax = dHisMin Or Double.IsNaN(dHisMin)
			dGMax = dHisMax / 10.0
			dGMin = 0
		ElseIf dHisMax = 0 
			dGMax = 200
			dGMin = 0
		ElseIf dHisMax - dHisMin <> 0
			dGMax = dHisMax / 10.0
			dGMin = dHisMin / 10.0
		End If
		.ChartAreas(0).AxisY.LabelStyle.Format			= "F1"

	Case "rbPBTemp"
          If cts.iRTYAxis = 1
        'Y軸スパン 固定
            'dHisMax = 2000 * (arg.dYSpanPer / 100.0)
            'dHisMin = 0
			dHisMax = cts.MainGraphYMax(arg.iSelectGItem) * 10 * (arg.dYSpanPer / 100.0)
			dHisMin = cts.MainGraphYMin(arg.iSelectGItem) * 10
        Else
		   If Not IsNothing(dFil.iLT_TempBuf2)
				iAryMax(0) = dFil.iLT_TempBuf2.Max
				iAryMin(0) = dFil.iLT_TempBuf2.Min
			Else
				iAryMax(0) = 300.0
				iAryMin(0) = 0.0
			End If
			For j = 1 To iHis
				Select Case j
				Case 1
        			iAryMax(j) = dFil.iTempBuf2_h1.Max
        			iAryMin(j) = dFil.iTempBuf2_h1.Min
				Case 2
        			iAryMax(j) = dFil.iTempBuf2_h2.Max
        			iAryMin(j) = dFil.iTempBuf2_h2.Min
				Case 3
        			iAryMax(j) = dFil.iTempBuf2_h3.Max
        			iAryMin(j) = dFil.iTempBuf2_h3.Min
				Case 4
        			iAryMax(j) = dFil.iTempBuf2_h4.Max
        			iAryMin(j) = dFil.iTempBuf2_h4.Min
				Case 5
        			iAryMax(j) = dFil.iTempBuf2_h5.Max
        			iAryMin(j) = dFil.iTempBuf2_h5.Min
				End Select
			Next
			For j = iStdBase To iStdBase - 1 + iStd
				Select Case j
				Case 6
  					If Not IsNothing(dFil.iTempBuf2_s1)
        				iAryMax(j) = dFil.iTempBuf2_s1.Max
        				iAryMin(j) = dFil.iTempBuf2_s1.Min
					End If
				Case 7
  					If Not IsNothing(dFil.iTempBuf2_s2)
        				iAryMax(j) = dFil.iTempBuf2_s2.Max
        				iAryMin(j) = dFil.iTempBuf2_s2.Min
					End If
				End Select
			Next
			dHisMax = iAryMax.Max
			dHisMin = iAryMin.Min
 			Dim dMaxH As Double = 0.0
 			Dim dMinH As Double = 0.0
			dMaxH = Math.Ceiling(dHisMax/100) * 100
			dMinH = Math.Floor(dHisMin/100) * 100
			dHisMax = dMaxH
			dHisMin = dMinH
		End if
		If dHisMax = dHisMin Or Double.IsNaN(dHisMin)
			dGMax = dHisMax / 10.0
			dGMin = 0
		ElseIf dHisMax = 0 
			dGMax = 200
			dGMin = 0
		ElseIf dHisMax - dHisMin <> 0
			dGMax = dHisMax / 10.0
			dGMin = dHisMin / 10.0
		End If
		.ChartAreas(0).AxisY.LabelStyle.Format			= "F1"

	Case "rbHPMV"
         If cts.iRTYAxis = 1
        'Y軸スパン 固定
            'dHisMax = 1000 * (arg.dYSpanPer / 100.0)
            'dHisMin = 0
			dHisMax = cts.MainGraphYMax(arg.iSelectGItem) * 10 * (arg.dYSpanPer / 100.0)
			dHisMin = cts.MainGraphYMin(arg.iSelectGItem) * 10
        Else
			If Not IsNothing(dFil.iLT_MvBuf)
				iAryMax(0) = dFil.iLT_MvBuf.Max
				iAryMin(0) = dFil.iLT_MvBuf.Min
			Else
				iAryMax(0) = 1000
				iAryMin(0) = 0
			End If
			For j = 1 To iHis
				Select Case j
				Case 1
        			iAryMax(j) = dFil.iMvBuf_h1.Max
        			iAryMin(j) = dFil.iMvBuf_h1.Min
				Case 2
        			iAryMax(j) = dFil.iMvBuf_h2.Max
        			iAryMin(j) = dFil.iMvBuf_h2.Min
				Case 3
        			iAryMax(j) = dFil.iMvBuf_h3.Max
        			iAryMin(j) = dFil.iMvBuf_h3.Min
				Case 4
        			iAryMax(j) = dFil.iMvBuf_h4.Max
        			iAryMin(j) = dFil.iMvBuf_h4.Min
				Case 5
        			iAryMax(j) = dFil.iMvBuf_h5.Max
        			iAryMin(j) = dFil.iMvBuf_h5.Min
				End Select
			Next
			For j = iStdBase To iStdBase - 1 + iStd
				Select Case j
				Case 6
  					If Not IsNothing(dFil.iMvBuf_s1)
        				iAryMax(j) = dFil.iMvBuf_s1.Max
        				iAryMin(j) = dFil.iMvBuf_s1.Min
					End If
				Case 7
  					If Not IsNothing(dFil.iMvBuf_s2)
        				iAryMax(j) = dFil.iMvBuf_s2.Max
        				iAryMin(j) = dFil.iMvBuf_s2.Min
					End If
				End Select
			Next
			dHisMax = iAryMax.Max
			dHisMin = iAryMin.Min
 			Dim dMaxH As Double = 0.0
 			Dim dMinH As Double = 0.0
			dMaxH = Math.Ceiling(dHisMax/10) * 10
			dMinH = Math.Floor(dHisMin/10) * 10
			dHisMax = dMaxH
			dHisMin = dMinH
		End if
		If dHisMax = dHisMin Or Double.IsNaN(dHisMin)
			dGMax = dHisMax / 10.0
			dGMin = 0
		ElseIf dHisMax = 0 
			dGMax = 200
			dGMin = 0
		ElseIf dHisMax - dHisMin <> 0
			dGMax = dHisMax / 10.0
			dGMin = dHisMin / 10.0
		End If
		.ChartAreas(0).AxisY.LabelStyle.Format			= "F1"

	Case "rbPBMV"
         If cts.iRTYAxis = 1
        'Y軸スパン 固定
            'dHisMax = 1000 * (arg.dYSpanPer / 100.0)
            'dHisMin = 0
			dHisMax = cts.MainGraphYMax(arg.iSelectGItem) * 10 * (arg.dYSpanPer / 100.0)
			dHisMin = cts.MainGraphYMin(arg.iSelectGItem) * 10
        Else
			If Not IsNothing(dFil.iLT_MvBuf2)
				iAryMax(0) = dFil.iLT_MvBuf2.Max
				iAryMin(0) = dFil.iLT_MvBuf2.Min
			Else
				iAryMax(0) = 100
				iAryMin(0) = 0
			End If
			For j = 1 To iHis
				Select Case j
				Case 1
        			iAryMax(j) = dFil.iMvBuf2_h1.Max
        			iAryMin(j) = dFil.iMvBuf2_h1.Min
				Case 2
        			iAryMax(j) = dFil.iMvBuf2_h2.Max
        			iAryMin(j) = dFil.iMvBuf2_h2.Min
				Case 3
        			iAryMax(j) = dFil.iMvBuf2_h3.Max
        			iAryMin(j) = dFil.iMvBuf2_h3.Min
				Case 4
        			iAryMax(j) = dFil.iMvBuf2_h4.Max
        			iAryMin(j) = dFil.iMvBuf2_h4.Min
				Case 5
        			iAryMax(j) = dFil.iMvBuf2_h5.Max
        			iAryMin(j) = dFil.iMvBuf2_h5.Min
				End Select
			Next
			For j = iStdBase To iStdBase - 1 + iStd
				Select Case j
				Case 6
  					If Not IsNothing(dFil.iMvBuf2_s1)
        				iAryMax(j) = dFil.iMvBuf2_s1.Max
        				iAryMin(j) = dFil.iMvBuf2_s1.Min
					End If
				Case 7
  					If Not IsNothing(dFil.iMvBuf2_s2)
        				iAryMax(j) = dFil.iMvBuf2_s2.Max
        				iAryMin(j) = dFil.iMvBuf2_s2.Min
					End If
				End Select
			Next
			dHisMax = iAryMax.Max
			dHisMin = iAryMin.Min
 			Dim dMaxH As Double = 0.0
 			Dim dMinH As Double = 0.0
			dMaxH = Math.Ceiling(dHisMax/10) * 10
			dMinH = Math.Floor(dHisMin/10) * 10
			dHisMax = dMaxH
			dHisMin = dMinH
		End if
		If dHisMax = dHisMin Or Double.IsNaN(dHisMin)
			dGMax = dHisMax / 10.0
			dGMin = 0
		ElseIf dHisMax = 0 
			dGMax = 200
			dGMin = 0
		ElseIf dHisMax - dHisMin <> 0
			dGMax = dHisMax / 10.0
			dGMin = dHisMin / 10.0
		End If

		.ChartAreas(0).AxisY.LabelStyle.Format			= "F1"
	
	End Select
    End With
Catch ex As Exception
    Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
    Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub
''' <summary>
''' Threshold変更に伴うゲルタイム表示その他更新 長期間欠モード
''' </summary>
''' <param name="arg"></param>
''' <param name="Chart1"></param>
''' <param name="clExt"></param>
''' <param name="sSelectGItem"></param>
''' <param name="cts"></param>
''' <param name="iThinTyp"></param>
Public Sub LTChartUpdateGelTimeLine( _
		ByVal arg			As ChartArg, _
        ByRef Chart1        As Chart, _
        ByRef clExt         As Extra, _
        ByVal sSelectGItem  As String, _
        ByVal cts           As CToolSettings, _
        ByVal iThinTyp      As Int16 _
)

Try
    With Chart1
		Chart1.Annotations.Clear							'全ての注釈ラインを消去
		If IsNothing(p_dtFile.iLT_MSec)
			Exit Sub
		End If
		'補正済みデータ表示
        Select Case arg.sCalcMethod
        Case lstCalcMethod(eCalcMethod.THR) , lstCalcMethod(eCalcMethod.PHASE), lstCalcMethod(eCalcMethod.MELT)
        'THRまたはSLOPE法,Phase法
			    'ゲルタイム 垂直補助線追加
			    Chart1.ChartAreas(0).AxisX.StripLines.Clear()
			    Dim Time_THR As New StripLine()
			    Dim dGelTimeG	As Double = arg.dGelTime / 60
			    Dim dTempG		As Double = arg.dMeltTemp
		        Time_THR.Font           = New Font(BASE_FONT, 12, FontStyle.Regular)
				
				Select Case arg.sCalcMethod
				Case lstCalcMethod(eCalcMethod.THR) , lstCalcMethod(eCalcMethod.SLOPE), lstCalcMethod(eCalcMethod.PHASE)
					Select Case arg.iGelTimeJudge
					Case 0      'NG
    					'Time_THR.Text           = $"NG: {dGelTime:F2} Sec"
    					Time_THR.Text           = m_sMes(efrmMainSTR.iGEL_NG).Replace("#GEL# Sec", dGelTimeG.ToString("F2") & " min")
						Time_THR.ForeColor      = Color.Red
						'Chart1.BackColor        = Color.lightpink
					Case 1      'OK
    					'Time_THR.Text           = $"OK: {dGelTime:F2} Sec"
    					Time_THR.Text           = m_sMes(efrmMainSTR.iGEL_OK).Replace("#GEL# Sec", dGelTimeG.ToString("F2") & " min")
						Time_THR.ForeColor      = Color.Blue
						'Chart1.BackColor        = Color.lightgreen
					Case else   'NA
    					'Time_THR.Text           = $"GelTime: {dGelTime:F2} Sec"
    					Time_THR.Text           = m_sMes(efrmMainSTR.iGEL_TIME).Replace("#GEL# Sec", dGelTimeG.ToString("F2") & " min")
						Time_THR.ForeColor      = Color.Blue
						'Chart1.BackColor        = Color.White
					End Select
				    Time_THR.IntervalOffset = dGelTimeG + 1		'+1はスムース調整

					'ゲルタイム判定範囲の境界線
					Dim Time_JudgeS As New StripLine()
					Dim Time_JudgeE As New StripLine()
					Dim dJudgeTimeS As Double = arg.dJudgeRangeMin / 60
					Dim dJudgeTimeE As Double = arg.dJudgeRangeMax / 60

					Time_JudgeS.Text           = "lower"
					Time_JudgeS.TextAlignment  = StringAlignment.Far
					Time_JudgeS.Font           = New Font(BASE_FONT, 12, FontStyle.Regular)
					Time_JudgeS.ForeColor      = Color.DarkCyan
					Time_JudgeS.Interval       = 0
					Time_JudgeS.IntervalOffset = dJudgeTimeS
					Time_JudgeS.BorderWidth    = 1
					Time_JudgeS.BorderDashStyle = ChartDashStyle.Solid
					Time_JudgeS.BorderColor    = Color.DarkCyan
					.ChartAreas(0).AxisX.StripLines.Add(Time_JudgeS)

					Time_JudgeE.Text           = "upper"
					Time_JudgeE.TextAlignment  = StringAlignment.Near
					Time_JudgeE.Font           = New Font(BASE_FONT, 12, FontStyle.Regular)
					Time_JudgeE.ForeColor      = Color.DarkCyan
					Time_JudgeE.Interval       = 0
					Time_JudgeE.IntervalOffset = dJudgeTimeE
					Time_JudgeE.BorderWidth    = 1
					Time_JudgeE.BorderDashStyle = ChartDashStyle.Solid
					Time_JudgeE.BorderColor    = Color.DarkCyan
					.ChartAreas(0).AxisX.StripLines.Add(Time_JudgeE)

					'データ有効範囲の境界線
					If arg.dAprxRangeMax<>0
						Dim Time_AprxS As New StripLine()
						Dim Time_AprxE As New StripLine()
						Dim dAprxRangeS As Double = arg.dAprxRangeMin / 60
						Dim dAprxRangeE As Double = arg.dAprxRangeMax / 60
						Time_AprxS.Interval       = 0
						Time_AprxS.IntervalOffset = dAprxRangeS
						Time_AprxS.BorderWidth    = 2
						Time_AprxS.BorderDashStyle = ChartDashStyle.Solid
						Time_AprxS.BorderColor    = Color.DarkGray
						.ChartAreas(0).AxisX.StripLines.Add(Time_AprxS)
						Time_AprxE.Interval       = 0
						Time_AprxE.IntervalOffset = dAprxRangeE
						Time_AprxE.BorderWidth    = 2
						Time_AprxE.BorderDashStyle = ChartDashStyle.Solid
						Time_AprxE.BorderColor    = Color.DarkGray
						.ChartAreas(0).AxisX.StripLines.Add(Time_AprxE)
					End If

				Case lstCalcMethod(eCalcMethod.MELT)
					If cts.iChartType = 1
    					Time_THR.Text           = $"{dTempG:F1} deg C"
						Time_THR.ForeColor      = Color.Blue
						'Chart1.BackColor        = Color.White
						Time_THR.IntervalOffset = dTempG
					Else
    					Time_THR.Text           = $"{arg.dGelTime:F0} Sec"
						Time_THR.ForeColor      = Color.Blue
						'Chart1.BackColor        = Color.White
						Time_THR.IntervalOffset = dGelTimeG
						'データ有効範囲の境界線
						If arg.dAprxRangeMax<>0
							Dim Time_AprxS As New StripLine()
							Dim Time_AprxE As New StripLine()
							Dim dAprxRangeS As Double = arg.dAprxRangeMin / 60
							Dim dAprxRangeE As Double = arg.dAprxRangeMax / 60
							Time_AprxS.Interval       = 0
							Time_AprxS.IntervalOffset = dAprxRangeS
							Time_AprxS.BorderWidth    = 2
							Time_AprxS.BorderDashStyle = ChartDashStyle.Solid
							Time_AprxS.BorderColor    = Color.DarkGray
							.ChartAreas(0).AxisX.StripLines.Add(Time_AprxS)
							Time_AprxE.Interval       = 0
							Time_AprxE.IntervalOffset = dAprxRangeE
							Time_AprxE.BorderWidth    = 2
							Time_AprxE.BorderDashStyle = ChartDashStyle.Solid
							Time_AprxE.BorderColor    = Color.DarkGray
							.ChartAreas(0).AxisX.StripLines.Add(Time_AprxE)
						End If
					End If
				End Select

				Time_THR.TextAlignment  = StringAlignment.Far
				Time_THR.Interval       = 0
				Time_THR.BorderWidth    = 1
				Time_THR.BorderDashStyle = ChartDashStyle.DashDot
				Time_THR.BorderColor    = Color.Red
	            If arg.dGelTime > 0
					.ChartAreas(0).AxisX.StripLines.Add(Time_THR)
				End If

        Case lstCalcMethod(eCalcMethod.EXTRA)
        End Select


    'Y軸
	'補正済みデータ表示
        Select Case arg.sCalcMethod
        Case lstCalcMethod(eCalcMethod.THR), lstCalcMethod(eCalcMethod.PHASE)
			If (sSelectGItem="rbCorrect" And arg.sCalcMethod = lstCalcMethod(eCalcMethod.THR)) Or _
				 (sSelectGItem="rbCorrectP" And arg.sCalcMethod = lstCalcMethod(eCalcMethod.THR)) Or _
				 (sSelectGItem="rbPosPhase" And arg.sCalcMethod = lstCalcMethod(eCalcMethod.PHASE))
				'THR法
				'ゲルタイム 水平補助線追加 
				.ChartAreas(0).AxisY.StripLines.Clear()
				.ChartAreas(0).AxisY2.StripLines.Clear()
				Dim Stress_THR  As New StripLine()
				Dim dThresholdG As Double
				Select Case sSelectGItem
				Case "rbCorrect"
   					dThresholdG             = arg.dFullStressADVal * (arg.dToqueThreshold / 100.0)
				Case "rbCorrectP"
   					dThresholdG             = arg.dToqueThreshold
				Case "rbPosPhase"
   					dThresholdG             = 90.0 * (arg.dToqueThreshold / 100.0)
				End Select
				
				'Stress_THR.Text             = $"Threshold: {dToqueThreshold} % "
				Stress_THR.Text             = m_sMes(efrmMainSTR.CHART_THRESHOLD) & arg.dToqueThreshold.ToString("F1") & " % "
				If arg.sCalcMethod = lstCalcMethod(eCalcMethod.PHASE)
					Stress_THR.Text             &= " -> " & dThresholdG.ToString("F1") & "[deg]"
				ElseIf sSelectGItem = "rbCorrectP"
				Else
					Stress_THR.Text             &= " -> " & dThresholdG.ToString("F1") & "[uN]"
				End If
				Stress_THR.Font             = New Font(BASE_FONT, 12, FontStyle.Regular)
				Stress_THR.ForeColor        = Color.Blue
				Stress_THR.Interval         = 0
				Stress_THR.TextAlignment	= StringAlignment.Near         'Near 左端
				Stress_THR.TextLineAlignment= StringAlignment.Far          'Far  上側
				Stress_THR.IntervalOffset   = dThresholdG
				Stress_THR.BorderWidth      = 1
				Stress_THR.BorderDashStyle  = ChartDashStyle.DashDot
				Stress_THR.BorderColor      = Color.DarkGreen
				.ChartAreas(0).AxisY.StripLines.Add(Stress_THR)
			End If

        Case lstCalcMethod(eCalcMethod.MELT)
			'ゲルタイム 水平補助線追加 
			.ChartAreas(0).AxisY.StripLines.Clear()
			.ChartAreas(0).AxisY2.StripLines.Clear()

			Select Case sSelectGItem
			Case "rbViscosity"
				If arg.dGelTime > 0
					Dim Stress_THR  As New StripLine()
					Dim dViscoG As Double
   					dViscoG					= arg.dMeltVisco

					Stress_THR.Text             = $"Viscosity: {dViscoG:F1} [PaS] "
					Stress_THR.Font             = New Font(BASE_FONT, 12, FontStyle.Regular)
					Stress_THR.ForeColor        = Color.Blue
					Stress_THR.Interval         = 0
					Stress_THR.TextAlignment	= StringAlignment.Near         'Near 左端
					Stress_THR.TextLineAlignment= StringAlignment.Far          'Far  上側
					Stress_THR.IntervalOffset   = dViscoG
					Stress_THR.BorderWidth      = 1
					Stress_THR.BorderDashStyle  = ChartDashStyle.DashDot
					Stress_THR.BorderColor      = Color.DarkGreen
					.ChartAreas(0).AxisY.StripLines.Add(Stress_THR)
				End If
			Case Else
			End Select
        End Select
'Debug.print( "AxisY.Maximum = " & Chart1.ChartAreas(0).AxisY.Maximum )

	End With
Catch ex As Exception
    Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
    Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub
End Module
