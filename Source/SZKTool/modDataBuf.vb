Imports System.IO
Imports System.Text
Imports System.Threading
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.Runtime.InteropServices
Imports Microsoft.Office.Interop
Imports SZKTool.DataFile

''' <summary>
''' DataFileクラスのデータバッファ基底クラス
''' </summary>
Public Class DataBuf
'--- 定数定義 ------------------------------------------------
Public Const cAllBufMax As Int32	= 1 + cHistMax + cStdMax		'全バッファ数
Public Const cHistMax	As Int32	= 5								'履歴バッファ数(1-5 グラフインデックス)
Public Const cStdMax	As Int32	= 2								'基準バッファ数(6-7 グラフインデックス)

'重ね書き用履歴データ配列クリア
Public	DATA_STORE_PATH				As String	= Nothing

'1トランザクションの測定値バッファ
Public p_iMSec(MEAS_BLOCK_DATA_NUM - 1)		As Int64				'経過時間(mSec)
Public p_iPwX(MEAS_BLOCK_DATA_NUM - 1)		As Int64				'応力X
Public p_iPwY(MEAS_BLOCK_DATA_NUM - 1)		As Int64				'応力Y
Public p_iPwZ(MEAS_BLOCK_DATA_NUM - 1)		As Int64				'応力Z(AD)
Public p_dPwZ(MEAS_BLOCK_DATA_NUM - 1)		As Double				'応力Z
Public p_iPOSX(MEAS_BLOCK_DATA_NUM - 1)		As Int64				'X軸位置
Public p_iPOSY(MEAS_BLOCK_DATA_NUM - 1)		As Int64				'Y軸位置
Public p_iPOSZ(MEAS_BLOCK_DATA_NUM - 1)		As Int64				'Z軸位置
public p_findat(MEAS_BLOCK_DATA_NUM)		As Int64				'補正後
public p_slope(MEAS_BLOCK_DATA_NUM)			As Int64				'補正後微分
public p_iTemp(MEAS_BLOCK_DATA_NUM - 1)		As Int64				'温度(HP)
public p_iMv(MEAS_BLOCK_DATA_NUM - 1)		As Int64				'温度操作量(HP)
public p_iTemp2(MEAS_BLOCK_DATA_NUM - 1)	As Int64				'温度(PROBE)
public p_iMv2(MEAS_BLOCK_DATA_NUM - 1)		As Int64				'温度操作量(PROBE)
public p_dShearStr(MEAS_BLOCK_DATA_NUM - 1)	As Double				'せん断応力
public p_dVisco(MEAS_BLOCK_DATA_NUM - 1)	As Double				'粘度
public p_dXPhase(MEAS_BLOCK_DATA_NUM - 1)	As Double				'動粘弾性X
public p_dYPhase(MEAS_BLOCK_DATA_NUM - 1)	As Double				'動粘弾性Y
Public p_dSplStr(MEAS_BLOCK_DATA_NUM - 1)	As Double				'サンプル歪
Public p_dShearMod(MEAS_BLOCK_DATA_NUM - 1)	As Double				'せん断弾性率(Pa)
Public p_dShearVelo(MEAS_BLOCK_DATA_NUM - 1)As Double				'せん断速度(1/S)
Public p_dXStrgMod(MEAS_BLOCK_DATA_NUM - 1)	As Double				'X貯蔵弾性率(Pa)
Public p_dXElasMod(MEAS_BLOCK_DATA_NUM - 1)	As Double				'X損失弾性率(Pa)
Public p_dXLossTan(MEAS_BLOCK_DATA_NUM - 1)	As Double				'X損失正接
Public p_dYStrgMod(MEAS_BLOCK_DATA_NUM - 1)	As Double				'Y貯蔵弾性率(Pa)
Public p_dYElasMod(MEAS_BLOCK_DATA_NUM - 1)	As Double				'Y損失弾性率(Pa)
Public p_dYLossTan(MEAS_BLOCK_DATA_NUM - 1)	As Double				'Y損失正接

'ファイル保存・読込用のバッファ（グラフ表示、中間値補正等に使用）(RealTimeFileSaveText/GetFileLineCount関数で拡張される)
Public iMSec()							As Int64 = nothing
Public iOxBuf()							As Int64 = nothing			'応力X(dig)
Public iOyBuf()							As Int64 = nothing			'応力Y(dig)
Public iOzBuf()							As Int64 = nothing			'応力Z(dig)
Public dOzBuf()							As Double = nothing			'応力Z(gram)
Public iPxBuf()							As Int64 = nothing			'X軸位置(um)
Public iPyBuf()							As Int64 = nothing			'Y軸位置(um)
Public iPzBuf()							As Int64 = nothing			'Z軸位置(um)
Public iHosei()							As Double = nothing			'合成後応力(uN)
Public dHosei100()						As Double = nothing			'合成後応力(%)
Public iTempBuf()						As Int64 = nothing			'HP温度
Public iMvBuf()							As Int64 = nothing			'HP温度操作量
Public iTempBuf2()						As Int64 = nothing			'PB温度
Public iMvBuf2()						As Int64 = nothing			'PB温度操作量
Public iSlope()							As Double = nothing			'スロープ法(応力変化率)
Public dXPosPhase()						As Double = nothing			'動粘弾性X
Public dYPosPhase()						As Double = nothing			'動粘弾性Y

Public dSplStrain()						As Double = nothing			'サンプル歪
Public dShearMod()						As Double = nothing			'せん断弾性率(Pa)
Public dShearVelo()						As Double = nothing			'せん断速度(1/S)
Public dXStrgMod()						As Double = nothing			'X貯蔵弾性率(Pa)
Public dXElasMod()						As Double = nothing			'X損失弾性率(Pa)
Public dXLossTan()						As Double = nothing			'X損失正接
Public dYStrgMod()						As Double = nothing			'Y貯蔵弾性率(Pa)
Public dYElasMod()						As Double = nothing			'Y損失弾性率(Pa)
Public dYLossTan()						As Double = nothing			'Y損失正接

'長期間欠モードバッファ
Public iLT_MSec()						As Int64 = nothing
'Public iLT_OxBuf()						As Int64 = nothing			'応力X(dig)
'Public iLT_OyBuf()						As Int64 = nothing			'応力Y(dig)
'Public iLT_OzBuf()						As Int64 = nothing			'応力Z(dig)
'Public dLT_OzBuf()						As Double = nothing			'応力Z(gram)
'Public iLT_PxBuf()						As Int64 = nothing			'X軸位置(um)
'Public iLT_PyBuf()						As Int64 = nothing			'Y軸位置(um)
'Public iLT_PzBuf()						As Int64 = nothing			'Z軸位置(um)
Public iLT_Hosei()						As Double = nothing			'合成後応力(uN)
Public dLT_Hosei100()					As Double = nothing			'合成後応力(%)
Public iLT_TempBuf()					As Int64 = nothing			'HP温度
Public iLT_MvBuf()						As Int64 = nothing			'HP温度操作量
Public iLT_TempBuf2()					As Int64 = nothing			'PB温度
Public iLT_MvBuf2()						As Int64 = nothing			'PB温度操作量
Public iLT_Slope()						As Double = nothing			'スロープ法(応力変化率)
Public dLT_XPosPhase()					As Double = nothing			'動粘弾性X
Public dLT_YPosPhase()					As Double = nothing			'動粘弾性Y

Public dLT_Viscosity()					As Double					'粘度(PaS)
Public dLT_SplStrain()					As Double = nothing			'サンプル歪
Public dLT_ShearMod()					As Double = nothing			'せん断弾性率(Pa)
Public dLT_ShearVelo()					As Double = nothing			'せん断速度(1/S)
Public dLT_XStrgMod()					As Double = nothing			'X貯蔵弾性率(Pa)
Public dLT_XElasMod()					As Double = nothing			'X損失弾性率(Pa)
Public dLT_XLossTan()					As Double = nothing			'X損失正接
Public dLT_YStrgMod()					As Double = nothing			'Y貯蔵弾性率(Pa)
Public dLT_YElasMod()					As Double = nothing			'Y損失弾性率(Pa)
Public dLT_YLossTan()					As Double = nothing			'Y損失正接



'粘度測定用バッファ
Public dShearVelocity()					As Double					'せん断速度(1/S)
Public dShearStress()					As Double					'せん断応力(Pa)
Public dViscosity()						As Double					'粘度(PaS)
Public uVis1()							As ViscosityCalcParameter	'フローカーブ 速度依存

Public iHoseiOrg()						As Int64 = nothing			'測定直後のiHoseiを退避(再計算用)

'再計算時の一時退避領域（使用後削除される）
Public iMSec_B()							As Int64 = nothing
Public iOxBuf_B()							As Int64 = nothing			'応力X(dig)
Public iOyBuf_B()							As Int64 = nothing			'応力Y(dig)
Public iOzBuf_B()							As Int64 = nothing			'応力Z(dig)
Public dOzBuf_B()							As Double = nothing			'応力Z(gram)
Public iPxBuf_B()							As Int64 = nothing			'X軸位置(um)
Public iPyBuf_B()							As Int64 = nothing			'Y軸位置(um)
Public iPzBuf_B()							As Int64 = nothing			'Z軸位置(um)
Public iHosei_B()							As Double = nothing			'合成後応力(uN)
Public iSlope_B()							As Double = nothing			'スロープ法(応力変化率)
Public dHosei100_B()						As Double = nothing			'合成後応力(%)
Public iTempBuf_B()							As Int64 = nothing			'HP温度
Public iMvBuf_B()							As Int64 = nothing			'HP温度操作量
Public iTempBuf2_B()						As Int64 = nothing			'PB温度
Public iMvBuf2_B()							As Int64 = nothing			'PB温度操作量
Public dXPosPhase_B()						As Double = nothing			'動粘弾性X
Public dYPosPhase_B()						As Double = nothing			'動粘弾性Y
Public dViscosity_B()						As Double					'粘度(PaS)
Public dSplStrain_B()						As Double = nothing			'サンプル歪
Public dShearMod_B()						As Double = nothing			'せん断弾性率(Pa)
Public dShearVelo_B()						As Double = nothing			'せん断速度(1/S)
Public dXStrgMod_B()						As Double = nothing			'X貯蔵弾性率(Pa)
Public dXElasMod_B()						As Double = nothing			'X損失弾性率(Pa)
Public dXLossTan_B()						As Double = nothing			'X損失正接
Public dYStrgMod_B()						As Double = nothing			'Y貯蔵弾性率(Pa)
Public dYElasMod_B()						As Double = nothing			'Y損失弾性率(Pa)
Public dYLossTan_B()						As Double = nothing			'Y損失正接

Public p_head_B							As DataFileHeader			'ヘッダ情報(一時退避)
Public p_head							As DataFileHeader			'ヘッダ情報(測定値領域)
Public p_head_h(0 To cHistMax-1)		As DataFileHeader			'ヘッダ情報(履歴)

'グラフ重書き用配列 ５回
Public iMSec_h1()						As Int64 = nothing
Public iOxBuf_h1()						As Int64 = nothing
Public iOyBuf_h1()						As Int64 = nothing
Public iOZBuf_h1()						As Int64 = nothing
Public dOzBuf_h1()						As Double = nothing
Public iPxBuf_h1()						As Int64 = nothing
Public iPyBuf_h1()						As Int64 = nothing
Public iPzBuf_h1()						As Int64 = nothing
Public iHosei_h1()						As Double = nothing
Public iSlope_h1()						As Double = nothing
Public dHoseiP_h1()						As Double = nothing
Public iTempBuf_h1()					As Int64 = nothing
Public iMvBuf_h1()						As Int64 = nothing
Public iTempBuf2_h1()					As Int64 = nothing
Public iMvBuf2_h1()						As Int64 = nothing
Public dViscosity_h1()					As Double = Nothing
Public dXPosPhase_h1()					As Double = Nothing
Public dYPosPhase_h1()					As Double = Nothing
Public dSplStrain_h1()					As Double = Nothing
Public dShearMod_h1()					As Double = Nothing
Public dShearVelo_h1()					As Double = Nothing
Public dXStrgMod_h1()					As Double = Nothing
Public dXElasMod_h1()					As Double = Nothing
Public dXLossTan_h1()					As Double = Nothing

Public iMSec_h2()						As Int64 = nothing
Public iOxBuf_h2()						As Int64 = nothing
Public iOyBuf_h2()						As Int64 = nothing
Public iOzBuf_h2()						As Int64 = nothing
Public dOzBuf_h2()						As Double = nothing
Public iPxBuf_h2()						As Int64 = nothing
Public iPyBuf_h2()						As Int64 = nothing
Public iPzBuf_h2()						As Int64 = nothing
Public iHosei_h2()						As Double = nothing
Public iSlope_h2()						As Double = nothing
Public dHoseiP_h2()						As Double = nothing
Public iTempBuf_h2()					As Int64 = nothing
Public iMvBuf_h2()						As Int64 = nothing
Public iTempBuf2_h2()					As Int64 = nothing
Public iMvBuf2_h2()						As Int64 = nothing
Public dViscosity_h2()					As Double = Nothing
Public dXPosPhase_h2()					As Double = Nothing
Public dYPosPhase_h2()					As Double = Nothing
Public dSplStrain_h2()					As Double = Nothing
Public dShearMod_h2()					As Double = Nothing
Public dShearVelo_h2()					As Double = Nothing
Public dXStrgMod_h2()					As Double = Nothing
Public dXElasMod_h2()					As Double = Nothing
Public dXLossTan_h2()					As Double = Nothing

Public iMSec_h3()						As Int64 = nothing
Public iOxBuf_h3()						As Int64 = nothing
Public iOyBuf_h3()						As Int64 = nothing
Public iOzBuf_h3()						As Int64 = nothing
Public dOzBuf_h3()						As Double = nothing
Public iPxBuf_h3()						As Int64 = nothing
Public iPyBuf_h3()						As Int64 = nothing
Public iPzBuf_h3()						As Int64 = nothing
Public iHosei_h3()						As Double = nothing
Public iSlope_h3()						As Double = nothing
Public dHoseiP_h3()						As Double = nothing
Public iTempBuf_h3()					As Int64 = nothing
Public iMvBuf_h3()						As Int64 = nothing
Public iTempBuf2_h3()					As Int64 = nothing
Public iMvBuf2_h3()						As Int64 = nothing
Public dViscosity_h3()					As Double = Nothing
Public dXPosPhase_h3()					As Double = Nothing
Public dYPosPhase_h3()					As Double = Nothing
Public dSplStrain_h3()					As Double = Nothing
Public dShearMod_h3()					As Double = Nothing
Public dShearVelo_h3()					As Double = Nothing
Public dXStrgMod_h3()					As Double = Nothing
Public dXElasMod_h3()					As Double = Nothing
Public dXLossTan_h3()					As Double = Nothing

Public iMSec_h4()						As Int64 = nothing
Public iOxBuf_h4()						As Int64 = nothing
Public iOyBuf_h4()						As Int64 = nothing
Public iOzBuf_h4()						As Int64 = nothing
Public dOzBuf_h4()						As Double = nothing
Public iPxBuf_h4()						As Int64 = nothing
Public iPyBuf_h4()						As Int64 = nothing
Public iPzBuf_h4()						As Int64 = nothing
Public iHosei_h4()						As Double = nothing
Public iSlope_h4()						As Double = nothing
Public dHoseiP_h4()						As Double = nothing
Public iTempBuf_h4()					As Int64 = nothing
Public iMvBuf_h4()						As Int64 = nothing
Public iTempBuf2_h4()					As Int64 = nothing
Public iMvBuf2_h4()						As Int64 = nothing
Public dViscosity_h4()					As Double = Nothing
Public dXPosPhase_h4()					As Double = Nothing
Public dYPosPhase_h4()					As Double = Nothing
Public dSplStrain_h4()					As Double = Nothing
Public dShearMod_h4()					As Double = Nothing
Public dShearVelo_h4()					As Double = Nothing
Public dXStrgMod_h4()					As Double = Nothing
Public dXElasMod_h4()					As Double = Nothing
Public dXLossTan_h4()					As Double = Nothing

Public iMSec_h5()						As Int64 = nothing
Public iOxBuf_h5()						As Int64 = nothing
Public iOyBuf_h5()						As Int64 = nothing
Public iOzBuf_h5()						As Int64 = nothing
Public dOzBuf_h5()						As Double = nothing
Public iPxBuf_h5()						As Int64 = nothing
Public iPyBuf_h5()						As Int64 = nothing
Public iPzBuf_h5()						As Int64 = nothing
Public iHosei_h5()						As Double = nothing
Public iSlope_h5()						As Double = nothing
Public dHoseiP_h5()						As Double = nothing
Public iTempBuf_h5()					As Int64 = nothing
Public iMvBuf_h5()						As Int64 = nothing
Public iTempBuf2_h5()					As Int64 = nothing
Public iMvBuf2_h5()						As Int64 = nothing
Public dViscosity_h5()					As Double = Nothing
Public dXPosPhase_h5()					As Double = Nothing
Public dYPosPhase_h5()					As Double = Nothing
Public dSplStrain_h5()					As Double = Nothing
Public dShearMod_h5()					As Double = Nothing
Public dShearVelo_h5()					As Double = Nothing
Public dXStrgMod_h5()					As Double = Nothing
Public dXElasMod_h5()					As Double = Nothing
Public dXLossTan_h5()					As Double = Nothing

'読込基準データ
Public p_head_s(0 To cStdMax - 1)		As DataFileHeader			'ヘッダ情報

Public iMsec_s1()						As Int64 = nothing
Public iOxBuf_s1()						As Int64 = nothing
Public iOyBuf_s1()						As Int64 = nothing
Public iOzBuf_s1()						As Int64 = nothing
Public dOzBuf_s1()						As Double = nothing
Public iPxBuf_s1()						As Int64 = nothing			'X軸位置(um)
Public iPyBuf_s1()						As Int64 = nothing			'Y軸位置(um)
Public iPzBuf_s1()						As Int64 = nothing			'Z軸位置(um)
Public iHosei_s1()						As Double = nothing
Public iSlope_s1()						As Double = nothing
Public dHoseiP_s1()						As Double = nothing
Public iTempBuf_s1()					As Int64 = nothing
Public iMvBuf_s1()						As Int64 = nothing
Public iTempBuf2_s1()					As Int64 = nothing
Public iMvBuf2_s1()						As Int64 = nothing
Public dViscosity_s1()					As Double = Nothing
Public dXPosPhase_s1()					As Double = Nothing
Public dYPosPhase_s1()					As Double = Nothing
Public dSplStrain_s1()					As Double = Nothing
Public dShearMod_s1()					As Double = Nothing
Public dShearVelo_s1()					As Double = Nothing
Public dXStrgMod_s1()					As Double = Nothing
Public dXElasMod_s1()					As Double = Nothing
Public dXLossTan_s1()					As Double = Nothing

Public iMsec_s2()						As Int64 = nothing
Public iOxBuf_s2()						As Int64 = nothing
Public iOyBuf_s2()						As Int64 = nothing
Public iOzBuf_s2()						As Int64 = nothing
Public dOzBuf_s2()						As Double = nothing
Public iPxBuf_s2()						As Int64 = nothing			'X軸位置(um)
Public iPyBuf_s2()						As Int64 = nothing			'Y軸位置(um)
Public iPzBuf_s2()						As Int64 = nothing			'Z軸位置(um)
Public iHosei_s2()						As Double = nothing
Public iSlope_s2()						As Double = nothing
Public dHoseiP_s2()						As Double = nothing
Public iTempBuf_s2()					As Int64 = nothing
Public iMvBuf_s2()						As Int64 = nothing
Public iTempBuf2_s2()					As Int64 = nothing
Public iMvBuf2_s2()						As Int64 = nothing
Public dViscosity_s2()					As Double = Nothing
Public dXPosPhase_s2()					As Double = Nothing
Public dYPosPhase_s2()					As Double = Nothing
Public dSplStrain_s2()					As Double = Nothing
Public dShearMod_s2()					As Double = Nothing
Public dShearVelo_s2()					As Double = Nothing
Public dXStrgMod_s2()					As Double = Nothing
Public dXElasMod_s2()					As Double = Nothing
Public dXLossTan_s2()					As Double = Nothing

Public p_ListCutting					As Int16 = -1

'モニタ表示用
Public	p_curStress						As int64					'応力値
Public	p_curStressP					As Double					'応力値%
Public	p_curViscosity					As Double					'粘度
Public	p_curPhase						As Double					'X位相差
Public	p_curSplStrain					As Double					'サンプルに与えたひずみ
Public	p_curShearMod					As Double					'ずり弾性率
Public	p_curShearVelo					As Double					'剪断速度
Public	p_curStrgMod					As Double					'X貯蔵弾性率
Public	p_curElasMod					As Double					'X損失弾性率
Public	p_curLosTan						As Double					'X損失正接

''' <summary>
''' 測定データ、履歴データから指定データを削除する
''' </summary>
''' <param name="iSel">	0:		測定データ領域 
'''						1-5:	履歴データ	
'''						6-7:	基準データ	
''' </param>
Public Sub OneDataDelete(ByVal iSel As Int16, ByVal bLTMode As Boolean)
	Select Case iSel
	Case 0
		MClear()
		h1Toh0Copy(bLTMode)
		h2Toh1Copy(bLTMode)
		h3Toh2Copy(bLTMode)
		h4Toh3Copy(bLTMode)
		h5Toh4Copy(bLTMode)
		h5Clear()
	Case 1
		h2Toh1Copy(bLTMode)
		h3Toh2Copy(bLTMode)
		h4Toh3Copy(bLTMode)
		h5Toh4Copy(bLTMode)
		h5Clear()
	Case 2
		h3Toh2Copy(bLTMode)
		h4Toh3Copy(bLTMode)
		h5Toh4Copy(bLTMode)
		h5Clear()
	Case 3
		h4Toh3Copy(bLTMode)
		h5Toh4Copy(bLTMode)
		h5Clear()
	Case 4
		h5Toh4Copy(bLTMode)
		h5Clear()
	Case 5
		h5Clear()
	Case 6
		ClearStandard1()
	Case 7
		ClearStandard2()
	End Select
End Sub
''' <summary>
''' 測定データ、履歴データから指定データを切り抜きする
''' </summary>
''' <param name="iSel">	0:		測定データ領域 
'''						1-5:	履歴データ	
'''						6-7:	基準データ	
''' </param>
Public Sub OneDataCut(ByVal iSel As Int16, ByVal bLTMode As Boolean)
	Select Case iSel
	Case 0
		h0ToBCopy(bLTMode)
		OneDataDelete(iSel, bLTMode)
	Case 1
		h1ToBCopy(bLTMode)
		OneDataDelete(iSel, bLTMode)
	Case 2
		h2ToBCopy(bLTMode)
		OneDataDelete(iSel, bLTMode)
	Case 3
		h3ToBCopy(bLTMode)
		OneDataDelete(iSel, bLTMode)
	Case 4
		h4ToBCopy(bLTMode)
		OneDataDelete(iSel, bLTMode)
	Case 5
		h5ToBCopy(bLTMode)
		OneDataDelete(iSel, bLTMode)
	Case 6
		s1ToBCopy(bLTMode)
		OneDataDelete(iSel, bLTMode)
	Case 7
		s2ToBCopy(bLTMode)
		OneDataDelete(iSel, bLTMode)
	Case Else
		p_ListCutting = -1		'切取り状態
	End Select
	p_ListCutting = iSel		'切取り状態
End Sub
''' <summary>
''' 測定データ、履歴データから指定データをコピーする
''' </summary>
''' <param name="iSel">	0:		測定データ領域 
'''						1-5:	履歴データ	
'''						6-7:	基準データ	
''' </param>
Public Sub OneDataCopy(ByVal iSel As Int16, ByVal bLTMode As Boolean)
	Select Case iSel
	Case 0
		h0ToBCopy(bLTMode)
	Case 1
		h1ToBCopy(bLTMode)
	Case 2
		h2ToBCopy(bLTMode)
	Case 3
		h3ToBCopy(bLTMode)
	Case 4
		h4ToBCopy(bLTMode)
	Case 5
		h5ToBCopy(bLTMode)
	Case 6
		s1ToBCopy(bLTMode)
	Case 7
		s2ToBCopy(bLTMode)
	Case Else
		p_ListCutting = -1		'切取り状態
	End Select
	p_ListCutting = iSel		'切取り状態
End Sub
''' <summary>
''' 測定データ、履歴データへ指定データをペーストする
''' </summary>
''' <param name="iSel">	0:		測定データ領域 
'''						1-5:	履歴データ	
'''						6-7:	基準データ	
''' </param>
''' <param name="bLTMode"></param>
Public Sub OneDataPaste(ByVal iSel As Int16, ByVal bLTMode As Boolean)
	If p_ListCutting = -1
		Exit Sub
	End If
	Select Case iSel
	Case 0
		h4Toh5Copy(bLTMode)
		h3Toh4Copy(bLTMode)
		h2Toh3Copy(bLTMode)
		h1Toh2Copy(bLTMode)
		h0Toh1Copy(bLTMode)
		BToh0Paste(bLTMode)
	Case 1
		h4Toh5Copy(bLTMode)
		h3Toh4Copy(bLTMode)
		h2Toh3Copy(bLTMode)
		h1Toh2Copy(bLTMode)
		BToh1Paste(bLTMode)
	Case 2
		h4Toh5Copy(bLTMode)
		h3Toh4Copy(bLTMode)
		h2Toh3Copy(bLTMode)
		BToh2Paste(bLTMode)
	Case 3
		h4Toh5Copy(bLTMode)
		h3Toh4Copy(bLTMode)
		BToh3Paste(bLTMode)
	Case 4
		h4Toh5Copy(bLTMode)
		BToh4Paste(bLTMode)
	Case 5
		BToh5Paste(bLTMode)
	Case 6
		BToS1Paste(bLTMode)
	Case 7
		BToS2Paste(bLTMode)
	Case Else
	End Select
	p_ListCutting = -1		'切取り状態
End Sub
''' <summary>
''' 重ね書き用履歴データ配列FIFO処理
''' </summary>
''' <param name="bLTMode">True:長期間欠モード</param>
Public Sub DataArrayFiFo(ByVal bLTMode As Boolean)
	If Not isNothing(iTempBuf_h4)
		h4Toh5Copy(bLTMode)
		h3Toh4Copy(bLTMode)
		h2Toh3Copy(bLTMode)
		h1Toh2Copy(bLTMode)
	ElseIf Not IsNothing(iTempBuf_h3)
		h3Toh4Copy(bLTMode)
		h2Toh3Copy(bLTMode)
		h1Toh2Copy(bLTMode)
	ElseIf Not IsNothing(iTempBuf_h2)
		h2Toh3Copy(bLTMode)
		h1Toh2Copy(bLTMode)
	ElseIf Not IsNothing(iTempBuf_h1)
		h1Toh2Copy(bLTMode)
	End If
	Select Case bLTMode
	Case True
		If Not IsNothing(iLT_TempBuf)
			h0Toh1Copy(bLTMode)
		End If
	Case Else
		If Not IsNothing(iTempBuf)
			h0Toh1Copy(bLTMode)
		End If
	End Select
End Sub
'履歴バッファ使用数を検査
Public Function CheckHist() As Int16
	If Not isNothing(iTempBuf_h5)
		Return cHistMax
	ElseIf Not isNothing(iTempBuf_h4)
		Return cHistMax - 1
	ElseIf Not IsNothing(iTempBuf_h3)
		Return cHistMax - 2
	ElseIf Not IsNothing(iTempBuf_h2)
		Return cHistMax - 3
	ElseIf Not IsNothing(iTempBuf_h1)
		Return cHistMax - 4
	Else
		Return cHistMax - 5
	End If
End Function
'履歴バッファクリア
Public Sub DataArrayFiFoClear()
	h1Clear()
	h2Clear()
	h3Clear()
	h4Clear()
	h5Clear()
End Sub
'重ね書き用基準データ配列FIFO処理
Public Sub DataArrayFiFoStd()
	If Not isNothing(iTempBuf_s1)
		s1Tos2Copy()
	End If
End Sub
'基準バッファ使用数を検査
Public Function CheckStd() As Int16
	If Not isNothing(iTempBuf_s2)
		Return 2
	ElseIf Not isNothing(iTempBuf_S1)
		Return 1
	Else
		Return 0
	End If
End Function
'測定領域クリア
Public Sub MClear()
	p_head		=Nothing
	iOxBuf		=Nothing
	iOyBuf		=Nothing
	iOzBuf		=Nothing
	dOzBuf		=Nothing
	iPxBuf		=Nothing
	iPyBuf		=Nothing
	iPzBuf		=Nothing
	iHosei		=Nothing
	iSlope		=Nothing
	dHosei100	=Nothing
	iTempBuf	=Nothing
	iMvBuf		=Nothing
	iTempBuf2	=Nothing
	iMvBuf2		=Nothing
	dViscosity	=Nothing
	dXPosPhase	=Nothing
	dYPosPhase	=Nothing
	dSplStrain	=Nothing
	dShearMod	=Nothing
	dShearVelo	=Nothing
	dXStrgMod	=Nothing
	dXElasMod	=Nothing
	dXLossTan	=Nothing

	iLT_MSec		=Nothing
	iLT_Hosei		=Nothing
	dLT_Hosei100	=Nothing
	iLT_TempBuf		=Nothing
	iLT_MvBuf		=Nothing
	iLT_TempBuf2	=Nothing
	iLT_MvBuf2		=Nothing
	dLT_Viscosity	=Nothing
	dLT_XPosPhase	=Nothing
	dLT_YPosPhase	=Nothing
	dLT_SplStrain	=Nothing
	dLT_ShearMod	=Nothing
	dLT_ShearVelo	=Nothing
	dLT_XStrgMod	=Nothing
	dLT_XElasMod	=Nothing
	dLT_XLossTan	=Nothing
End Sub
'基準バッファクリア
Public Sub ClearStandard()
	ClearStandard1()
	ClearStandard2()
End Sub
Public Sub ClearStandard1()
	p_head_s(0)		=Nothing
	iOxBuf_s1		=Nothing
	iOyBuf_s1		=Nothing
	iOzBuf_s1		=Nothing
	dOzBuf_s1		=Nothing
	iPxBuf_s1		=Nothing
	iPyBuf_s1		=Nothing
	iPzBuf_s1		=Nothing
	iHosei_s1		=Nothing
	iSlope_s1		=Nothing
	dHoseiP_s1		=Nothing
	iTempBuf_s1		=Nothing
	iMvBuf_s1		=Nothing
	iTempBuf2_s1	=Nothing
	iMvBuf2_s1		=Nothing
	dViscosity_s1	=Nothing
	dXPosPhase_s1	=Nothing
	dYPosPhase_s1	=Nothing
	dSplStrain_s1	=Nothing
	dShearMod_s1	=Nothing
	dShearVelo_s1	=Nothing
	dXStrgMod_s1	=Nothing
	dXElasMod_s1	=Nothing
	dXLossTan_s1	=Nothing

	iMSec_s1		=Nothing
End Sub
Public Sub ClearStandard2()
	p_head_s(1)		=Nothing
	iOxBuf_s2		=Nothing
	iOyBuf_s2		=Nothing
	iOzBuf_s2		=Nothing
	dOzBuf_s2		=Nothing
	iPxBuf_s2		=Nothing
	iPyBuf_s2		=Nothing
	iPzBuf_s2		=Nothing
	iHosei_s2		=Nothing
	iSlope_s2		=Nothing
	dHoseiP_s2		=Nothing
	iTempBuf_s2		=Nothing
	iMvBuf_s2		=Nothing
	iTempBuf2_s2	=Nothing
	iMvBuf2_s2		=Nothing
	dViscosity_s2	=Nothing
	dXPosPhase_s2	=Nothing
	dYPosPhase_s2	=Nothing
	dSplStrain_s2	=Nothing
	dShearMod_s2	=Nothing
	dShearVelo_s2	=Nothing
	dXStrgMod_s2	=Nothing
	dXElasMod_s2	=Nothing
	dXLossTan_s2	=Nothing

	iMSec_s2		=Nothing
End Sub
Public Sub h1Clear()
	p_dtFile.p_head_h(0)		=Nothing

	iMSec_h1		=Nothing
	iOxBuf_h1		=Nothing
	iOyBuf_h1		=Nothing
	iOzBuf_h1		=Nothing
	dOzBuf_h1		=Nothing
	iPxBuf_h1		=Nothing
	iPyBuf_h1		=Nothing
	iPzBuf_h1		=Nothing
	iHosei_h1		=Nothing
	iSlope_h1		=Nothing
	dHoseiP_h1		=Nothing
	iTempBuf_h1		=Nothing
	iMvBuf_h1		=Nothing
	iTempBuf2_h1	=Nothing
	iMvBuf2_h1		=Nothing
	dViscosity_h1	=Nothing
	dXPosPhase_h1	=Nothing
	dYPosPhase_h1	=Nothing
	dSplStrain_h1	=Nothing
	dShearMod_h1	=Nothing
	dShearVelo_h1	=Nothing
	dXStrgMod_h1	=Nothing
	dXElasMod_h1	=Nothing
	dXLossTan_h1	=Nothing
End Sub
Public Sub h2Clear()
	p_head_h(1)		=Nothing

	iMSec_h2		=Nothing
	iOxBuf_h2		=Nothing
	iOyBuf_h2		=Nothing
	iOzBuf_h2		=Nothing
	dOzBuf_h2		=Nothing
	iPxBuf_h2		=Nothing
	iPyBuf_h2		=Nothing
	iPzBuf_h2		=Nothing
	iHosei_h2		=Nothing
	iSlope_h2		=Nothing
	dHoseiP_h2		=Nothing
	iTempBuf_h2		=Nothing
	iMvBuf_h2		=Nothing
	iTempBuf2_h2	=Nothing
	iMvBuf2_h2		=Nothing
	dViscosity_h2	=Nothing
	dXPosPhase_h2	=Nothing
	dYPosPhase_h2	=Nothing
	dSplStrain_h2	=Nothing
	dShearMod_h2	=Nothing
	dShearVelo_h2	=Nothing
	dXStrgMod_h2	=Nothing
	dXElasMod_h2	=Nothing
	dXLossTan_h2	=Nothing
End Sub
Public Sub h3Clear()
	p_head_h(2)		=Nothing

	iMSec_h3		=Nothing
	iOxBuf_h3		=Nothing
	iOyBuf_h3		=Nothing
	iOzBuf_h3		=Nothing
	dOzBuf_h3		=Nothing
	iPxBuf_h3		=Nothing
	iPyBuf_h3		=Nothing
	iPzBuf_h3		=Nothing
	iHosei_h3		=Nothing
	iSlope_h3		=Nothing
	dHoseiP_h3		=Nothing
	iTempBuf_h3		=Nothing
	iMvBuf_h3		=Nothing
	iTempBuf2_h3	=Nothing
	iMvBuf2_h3		=Nothing
	dViscosity_h3	=Nothing
	dXPosPhase_h3	=Nothing
	dYPosPhase_h3	=Nothing
	dSplStrain_h3	=Nothing
	dShearMod_h3	=Nothing
	dShearVelo_h3	=Nothing
	dXStrgMod_h3	=Nothing
	dXElasMod_h3	=Nothing
	dXLossTan_h3	=Nothing
End Sub
Public Sub h4Clear()
	p_head_h(3)		=Nothing

	iMSec_h4		=Nothing
	iOxBuf_h4		=Nothing
	iOyBuf_h4		=Nothing
	iOzBuf_h4		=Nothing
	dOzBuf_h4		=Nothing
	iPxBuf_h4		=Nothing
	iPyBuf_h4		=Nothing
	iPzBuf_h4		=Nothing
	iHosei_h4		=Nothing
	iSlope_h4		=Nothing
	dHoseiP_h4		=Nothing
	iTempBuf_h4		=Nothing
	iMvBuf_h4		=Nothing
	iTempBuf2_h4	=Nothing
	iMvBuf2_h4		=Nothing
	dViscosity_h4	=Nothing
	dXPosPhase_h4	=Nothing
	dYPosPhase_h4	=Nothing
	dSplStrain_h4	=Nothing
	dShearMod_h4	=Nothing
	dShearVelo_h4	=Nothing
	dXStrgMod_h4	=Nothing
	dXElasMod_h4	=Nothing
	dXLossTan_h4	=Nothing
End Sub
Public Sub h5Clear()
	p_head_h(4)		=Nothing

	iMSec_h5		=Nothing
	iOxBuf_h5		=Nothing
	iOyBuf_h5		=Nothing
	iOzBuf_h5		=Nothing
	dOzBuf_h5		=Nothing
	iPxBuf_h5		=Nothing
	iPyBuf_h5		=Nothing
	iPzBuf_h5		=Nothing
	iHosei_h5		=Nothing
	iSlope_h5		=Nothing
	dHoseiP_h5		=Nothing
	iTempBuf_h5		=Nothing
	iMvBuf_h5		=Nothing
	iTempBuf2_h5	=Nothing
	iMvBuf2_h5		=Nothing
	dViscosity_h5	=Nothing
	dXPosPhase_h5	=Nothing
	dYPosPhase_h5	=Nothing
	dSplStrain_h5	=Nothing
	dShearMod_h5	=Nothing
	dShearVelo_h5	=Nothing
	dXStrgMod_h5	=Nothing
	dXElasMod_h5	=Nothing
	dXLossTan_h5	=Nothing
End Sub

Private sub s1Tos2Copy()
Try
	Array.Resize(iOxBuf_s2,		iOxBuf_s1.Length)
	Array.Resize(iOyBuf_s2,		iOyBuf_s1.Length)
	Array.Resize(iOzBuf_s2,		iOzBuf_s1.Length)
	Array.Resize(dOzBuf_s2,		dOzBuf_s1.Length)
	Array.Resize(iPxBuf_s2,		iPxBuf_s1.Length)
	Array.Resize(iPyBuf_s2,		iPyBuf_s1.Length)
	Array.Resize(iPzBuf_s2,		iPzBuf_s1.Length)
	Array.Resize(iTempBuf_s2,	iTempBuf_s1.Length)
	Array.Resize(iMvBuf_s2,		iMvBuf_s1.Length)
	Array.Resize(iTempBuf2_s2,	iTempBuf2_s1.Length)
	Array.Resize(iMvBuf2_s2,	iMvBuf2_s1.Length)
	Array.Resize(iHosei_s2,		iHosei_s1.Length)
	Array.Resize(iSlope_s2,		iSlope_s1.Length)
	Array.Resize(dHoseiP_s2,	dHoseiP_s1.Length)
	Array.Resize(dViscosity_s2,	dViscosity_s1.Length)
	Array.Resize(dXPosPhase_s2,	dXPosPhase_s1.Length)
	Array.Resize(dYPosPhase_s2,	dYPosPhase_s1.Length)
	Array.Resize(dSplStrain_s2,	dSplStrain_s1.Length)
	Array.Resize(dShearMod_s2,	dShearMod_s1.Length)
	Array.Resize(dShearVelo_s2,	dShearVelo_s1.Length)
	Array.Resize(dXStrgMod_s2,	dXStrgMod_s1.Length)
	Array.Resize(dXElasMod_s2,	dXElasMod_s1.Length)
	Array.Resize(dXLossTan_s2,	dXLossTan_s1.Length)

	Array.Copy(iOxBuf_s1,		iOxBuf_s2,		iOxBuf_s1.Length)
	Array.Copy(iOyBuf_s1,		iOyBuf_s2,		iOyBuf_s1.Length)
	Array.Copy(iOzBuf_s1,		iOzBuf_s2,		iOzBuf_s1.Length)
	Array.Copy(dOzBuf_s1,		dOzBuf_s2,		dOzBuf_s1.Length)
	Array.Copy(iPxBuf_s1,		iPxBuf_s2,		iPxBuf_s1.Length)
	Array.Copy(iPyBuf_s1,		iPyBuf_s2,		iPyBuf_s1.Length)
	Array.Copy(iPzBuf_s1,		iPzBuf_s2,		iPzBuf_s1.Length)
	Array.Copy(iTempBuf_s1,		iTempBuf_s2,	iTempBuf_s1.Length)
	Array.Copy(iMvBuf_s1,		iMvBuf_s2,		iMvBuf_s1.Length)
	Array.Copy(iTempBuf2_s1,	iTempBuf2_s2,	iTempBuf2_s1.Length)
	Array.Copy(iMvBuf2_s1,		iMvBuf2_s2,		iMvBuf2_s1.Length)
	Array.Copy(iHosei_s1,		iHosei_s2,		iHosei_s1.Length)
	Array.Copy(iSlope_s1,		iSlope_s2,		iSlope_s1.Length)
	Array.Copy(dHoseiP_s1,		dHoseiP_s2,		dHoseiP_s1.Length)
	Array.Copy(dViscosity_s1,	dViscosity_s2,	dViscosity_s1.Length)
	Array.Copy(dXPosPhase_s1,	dXPosPhase_s2,	dXPosPhase_s1.Length)
	Array.Copy(dYPosPhase_s1,	dYPosPhase_s2,	dYPosPhase_s1.Length)
	Array.Copy(dSplStrain_s1,	dSplStrain_s2,	dSplStrain_s1.Length)
	Array.Copy(dShearMod_s1,	dShearMod_s2,	dShearMod_s1.Length)
	Array.Copy(dShearVelo_s1,	dShearVelo_s2,	dShearVelo_s1.Length)
	Array.Copy(dXStrgMod_s1,	dXStrgMod_s2,	dXStrgMod_s1.Length)
	Array.Copy(dXElasMod_s1,	dXElasMod_s2,	dXElasMod_s1.Length)
	Array.Copy(dXLossTan_s1,	dXLossTan_s2,	dXLossTan_s1.Length)
	p_head_s(1) = p_head_s(0)

Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End sub

Private sub h4Toh5Copy(ByVal bLTMode As Boolean)
Try
	Select Case bLTMode
	Case True
		If IsNothing(iTempBuf_h4)
			Exit Sub
		End If
		Array.Resize(iMsec_h5,		iMsec_h4.Length)
		Array.Resize(iTempBuf_h5,	iTempBuf_h4.Length)
		Array.Resize(iMvBuf_h5,		iMvBuf_h4.Length)
		Array.Resize(iTempBuf2_h5,	iTempBuf2_h4.Length)
		Array.Resize(iMvBuf2_h5,	iMvBuf2_h4.Length)
		Array.Resize(iHosei_h5,		iHosei_h4.Length)
		Array.Resize(dHoseiP_h5,	dHoseiP_h4.Length)
		Array.Resize(dViscosity_h5,	dViscosity_h4.Length)
		Array.Resize(dXPosPhase_h5,	dXPosPhase_h4.Length)
		Array.Resize(dYPosPhase_h5,	dYPosPhase_h4.Length)
		Array.Resize(dSplStrain_h5,	dSplStrain_h4.Length)
		Array.Resize(dShearMod_h5,	dShearMod_h4.Length)
		Array.Resize(dShearVelo_h5,	dShearVelo_h4.Length)
		Array.Resize(dXStrgMod_h5,	dXStrgMod_h4.Length)
		Array.Resize(dXElasMod_h5,	dXElasMod_h4.Length)
		Array.Resize(dXLossTan_h5,	dXLossTan_h4.Length)

		Array.Copy(iMsec_h4,		iMsec_h5,		iMsec_h4.Length)
		Array.Copy(iTempBuf_h4,		iTempBuf_h5,	iTempBuf_h4.Length)
		Array.Copy(iMvBuf_h4,		iMvBuf_h5,		iMvBuf_h4.Length)
		Array.Copy(iTempBuf2_h4,	iTempBuf2_h5,	iTempBuf2_h4.Length)
		Array.Copy(iMvBuf2_h4,		iMvBuf2_h5,		iMvBuf2_h4.Length)
		Array.Copy(iHosei_h4,		iHosei_h5,		iHosei_h4.Length)
		Array.Copy(dHoseiP_h4,		dHoseiP_h5,		dHoseiP_h4.Length)
		Array.Copy(dViscosity_h4,	dViscosity_h5,	dViscosity_h4.Length)
		Array.Copy(dXPosPhase_h4,	dXPosPhase_h5,	dXPosPhase_h4.Length)
		Array.Copy(dYPosPhase_h4,	dYPosPhase_h5,	dYPosPhase_h4.Length)
		Array.Copy(dSplStrain_h4,	dSplStrain_h5,	dSplStrain_h4.Length)
		Array.Copy(dShearMod_h4,	dShearMod_h5,	dShearMod_h4.Length)
		Array.Copy(dShearVelo_h4,	dShearVelo_h5,	dShearVelo_h4.Length)
		Array.Copy(dXStrgMod_h4,	dXStrgMod_h5,	dXStrgMod_h4.Length)
		Array.Copy(dXElasMod_h4,	dXElasMod_h5,	dXElasMod_h4.Length)
		Array.Copy(dXLossTan_h4,	dXLossTan_h5,	dXLossTan_h4.Length)
	Case Else
		If IsNothing(iTempBuf_h4)
			Exit Sub
		End If
		Array.Resize(iMsec_h5,		iMsec_h4.Length)
		Array.Resize(iOxBuf_h5,		iOxBuf_h4.Length)
		Array.Resize(iOyBuf_h5,		iOyBuf_h4.Length)
		Array.Resize(iOzBuf_h5,		iOzBuf_h4.Length)
		Array.Resize(dOzBuf_h5,		dOzBuf_h4.Length)
		Array.Resize(iPxBuf_h5,		iPxBuf_h4.Length)
		Array.Resize(iPyBuf_h5,		iPyBuf_h4.Length)
		Array.Resize(iPzBuf_h5,		iPzBuf_h4.Length)
		Array.Resize(iTempBuf_h5,	iTempBuf_h4.Length)
		Array.Resize(iMvBuf_h5,		iMvBuf_h4.Length)
		Array.Resize(iTempBuf2_h5,	iTempBuf2_h4.Length)
		Array.Resize(iMvBuf2_h5,	iMvBuf2_h4.Length)
		Array.Resize(iHosei_h5,		iHosei_h4.Length)
		Array.Resize(iSlope_h5,		iSlope_h4.Length)
		Array.Resize(dHoseiP_h5,	dHoseiP_h4.Length)
		Array.Resize(dViscosity_h5,	dViscosity_h4.Length)
		Array.Resize(dXPosPhase_h5,	dXPosPhase_h4.Length)
		Array.Resize(dYPosPhase_h5,	dYPosPhase_h4.Length)
		Array.Resize(dSplStrain_h5,	dSplStrain_h4.Length)
		Array.Resize(dShearMod_h5,	dShearMod_h4.Length)
		Array.Resize(dShearVelo_h5,	dShearVelo_h4.Length)
		Array.Resize(dXStrgMod_h5,	dXStrgMod_h4.Length)
		Array.Resize(dXElasMod_h5,	dXElasMod_h4.Length)
		Array.Resize(dXLossTan_h5,	dXLossTan_h4.Length)

		Array.Copy(iMsec_h4,		iMsec_h5,		iMsec_h4.Length)
		Array.Copy(iOxBuf_h4,		iOxBuf_h5,		iOxBuf_h4.Length)
		Array.Copy(iOyBuf_h4,		iOyBuf_h5,		iOyBuf_h4.Length)
		Array.Copy(iOzBuf_h4,		iOzBuf_h5,		iOzBuf_h4.Length)
		Array.Copy(dOzBuf_h4,		dOzBuf_h5,		dOzBuf_h4.Length)
		Array.Copy(iPxBuf_h4,		iPxBuf_h5,		iPxBuf_h4.Length)
		Array.Copy(iPyBuf_h4,		iPyBuf_h5,		iPyBuf_h4.Length)
		Array.Copy(iPzBuf_h4,		iPzBuf_h5,		iPzBuf_h4.Length)
		Array.Copy(iTempBuf_h4,		iTempBuf_h5,	iTempBuf_h4.Length)
		Array.Copy(iMvBuf_h4,		iMvBuf_h5,		iMvBuf_h4.Length)
		Array.Copy(iTempBuf2_h4,	iTempBuf2_h5,	iTempBuf2_h4.Length)
		Array.Copy(iMvBuf2_h4,		iMvBuf2_h5,		iMvBuf2_h4.Length)
		Array.Copy(iHosei_h4,		iHosei_h5,		iHosei_h4.Length)
		Array.Copy(iSlope_h4,		iSlope_h5,		iSlope_h4.Length)
		Array.Copy(dHoseiP_h4,		dHoseiP_h5,		dHoseiP_h4.Length)
		Array.Copy(dViscosity_h4,	dViscosity_h5,	dViscosity_h4.Length)
		Array.Copy(dXPosPhase_h4,	dXPosPhase_h5,	dXPosPhase_h4.Length)
		Array.Copy(dYPosPhase_h4,	dYPosPhase_h5,	dYPosPhase_h4.Length)
		Array.Copy(dSplStrain_h4,	dSplStrain_h5,	dSplStrain_h4.Length)
		Array.Copy(dShearMod_h4,	dShearMod_h5,	dShearMod_h4.Length)
		Array.Copy(dShearVelo_h4,	dShearVelo_h5,	dShearVelo_h4.Length)
		Array.Copy(dXStrgMod_h4,	dXStrgMod_h5,	dXStrgMod_h4.Length)
		Array.Copy(dXElasMod_h4,	dXElasMod_h5,	dXElasMod_h4.Length)
		Array.Copy(dXLossTan_h4,	dXLossTan_h5,	dXLossTan_h4.Length)
	End Select
	p_head_h(4) = p_head_h(3)
Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End sub
Private sub h3Toh4Copy(ByVal bLTMode As Boolean)
Try
	Select Case bLTMode
	Case True
		If IsNothing(iTempBuf_h3)
			Exit Sub
		End If
		Array.Resize(iMsec_h4,		iMsec_h3.Length)
		Array.Resize(iTempBuf_h4,	iTempBuf_h3.Length)
		Array.Resize(iMvBuf_h4,		iMvBuf_h3.Length)
		Array.Resize(iTempBuf2_h4,	iTempBuf2_h3.Length)
		Array.Resize(iMvBuf2_h4,	iMvBuf2_h3.Length)
		Array.Resize(iHosei_h4,		iHosei_h3.Length)
		Array.Resize(dHoseiP_h4,	dHoseiP_h3.Length)
		Array.Resize(dViscosity_h4,	dViscosity_h3.Length)
		Array.Resize(dXPosPhase_h4,	dXPosPhase_h3.Length)
		Array.Resize(dYPosPhase_h4,	dYPosPhase_h3.Length)
		Array.Resize(dSplStrain_h4,	dSplStrain_h3.Length)
		Array.Resize(dShearMod_h4,	dShearMod_h3.Length)
		Array.Resize(dShearVelo_h4,	dShearVelo_h3.Length)
		Array.Resize(dXStrgMod_h4,	dXStrgMod_h3.Length)
		Array.Resize(dXElasMod_h4,	dXElasMod_h3.Length)
		Array.Resize(dXLossTan_h4,	dXLossTan_h3.Length)

		Array.Copy(iMsec_h3,		iMsec_h4,		iMsec_h3.Length)
		Array.Copy(iTempBuf_h3,		iTempBuf_h4,	iTempBuf_h3.Length)
		Array.Copy(iMvBuf_h3,		iMvBuf_h4,		iMvBuf_h3.Length)
		Array.Copy(iTempBuf2_h3,	iTempBuf2_h4,	iTempBuf2_h3.Length)
		Array.Copy(iMvBuf2_h3,		iMvBuf2_h4,		iMvBuf2_h3.Length)
		Array.Copy(iHosei_h3,		iHosei_h4,		iHosei_h3.Length)
		Array.Copy(dHoseiP_h3,		dHoseiP_h4,		dHoseiP_h3.Length)
		Array.Copy(dViscosity_h3,	dViscosity_h4,	dViscosity_h3.Length)
		Array.Copy(dXPosPhase_h3,	dXPosPhase_h4,	dXPosPhase_h3.Length)
		Array.Copy(dYPosPhase_h3,	dYPosPhase_h4,	dYPosPhase_h3.Length)
		Array.Copy(dSplStrain_h3,	dSplStrain_h4,	dSplStrain_h3.Length)
		Array.Copy(dShearMod_h3,	dShearMod_h4,	dShearMod_h3.Length)
		Array.Copy(dShearVelo_h3,	dShearVelo_h4,	dShearVelo_h3.Length)
		Array.Copy(dXStrgMod_h3,	dXStrgMod_h4,	dXStrgMod_h3.Length)
		Array.Copy(dXElasMod_h3,	dXElasMod_h4,	dXElasMod_h3.Length)
		Array.Copy(dXLossTan_h3,	dXLossTan_h4,	dXLossTan_h3.Length)
	Case Else
		If IsNothing(iTempBuf_h3)
			Exit Sub
		End If
		Array.Resize(iMsec_h4,		iMsec_h3.Length)
		Array.Resize(iOxBuf_h4,		iOxBuf_h3.Length)
		Array.Resize(iOyBuf_h4,		iOyBuf_h3.Length)
		Array.Resize(iOzBuf_h4,		iOzBuf_h3.Length)
		Array.Resize(dOzBuf_h4,		dOzBuf_h3.Length)
		Array.Resize(iPxBuf_h4,		iPxBuf_h3.Length)
		Array.Resize(iPyBuf_h4,		iPyBuf_h3.Length)
		Array.Resize(iPzBuf_h4,		iPzBuf_h3.Length)
		Array.Resize(iTempBuf_h4,	iTempBuf_h3.Length)
		Array.Resize(iMvBuf_h4,		iMvBuf_h3.Length)
		Array.Resize(iTempBuf2_h4,	iTempBuf2_h3.Length)
		Array.Resize(iMvBuf2_h4,	iMvBuf2_h3.Length)
		Array.Resize(iHosei_h4,		iHosei_h3.Length)
		Array.Resize(iSlope_h4,		iSlope_h3.Length)
		Array.Resize(dHoseiP_h4,	dHoseiP_h3.Length)
		Array.Resize(dViscosity_h4,	dViscosity_h3.Length)
		Array.Resize(dXPosPhase_h4,	dXPosPhase_h3.Length)
		Array.Resize(dYPosPhase_h4,	dYPosPhase_h3.Length)
		Array.Resize(dSplStrain_h4,	dSplStrain_h3.Length)
		Array.Resize(dShearMod_h4,	dShearMod_h3.Length)
		Array.Resize(dShearVelo_h4,	dShearVelo_h3.Length)
		Array.Resize(dXStrgMod_h4,	dXStrgMod_h3.Length)
		Array.Resize(dXElasMod_h4,	dXElasMod_h3.Length)
		Array.Resize(dXLossTan_h4,	dXLossTan_h3.Length)

		Array.Copy(iMsec_h3,		iMsec_h4,		iMsec_h3.Length)
		Array.Copy(iOxBuf_h3,		iOxBuf_h4,		iOxBuf_h3.Length)
		Array.Copy(iOyBuf_h3,		iOyBuf_h4,		iOyBuf_h3.Length)
		Array.Copy(iOzBuf_h3,		iOzBuf_h4,		iOzBuf_h3.Length)
		Array.Copy(dOzBuf_h3,		dOzBuf_h4,		dOzBuf_h3.Length)
		Array.Copy(iPxBuf_h3,		iPxBuf_h4,		iPxBuf_h3.Length)
		Array.Copy(iPyBuf_h3,		iPyBuf_h4,		iPyBuf_h3.Length)
		Array.Copy(iPzBuf_h3,		iPzBuf_h4,		iPzBuf_h3.Length)
		Array.Copy(iTempBuf_h3,		iTempBuf_h4,	iTempBuf_h3.Length)
		Array.Copy(iMvBuf_h3,		iMvBuf_h4,		iMvBuf_h3.Length)
		Array.Copy(iTempBuf2_h3,	iTempBuf2_h4,	iTempBuf2_h3.Length)
		Array.Copy(iMvBuf2_h3,		iMvBuf2_h4,		iMvBuf2_h3.Length)
		Array.Copy(iHosei_h3,		iHosei_h4,		iHosei_h3.Length)
		Array.Copy(iSlope_h3,		iSlope_h4,		iSlope_h3.Length)
		Array.Copy(dHoseiP_h3,		dHoseiP_h4,		dHoseiP_h3.Length)
		Array.Copy(dViscosity_h3,	dViscosity_h4,	dViscosity_h3.Length)
		Array.Copy(dXPosPhase_h3,	dXPosPhase_h4,	dXPosPhase_h3.Length)
		Array.Copy(dYPosPhase_h3,	dYPosPhase_h4,	dYPosPhase_h3.Length)
		Array.Copy(dSplStrain_h3,	dSplStrain_h4,	dSplStrain_h3.Length)
		Array.Copy(dShearMod_h3,	dShearMod_h4,	dShearMod_h3.Length)
		Array.Copy(dShearVelo_h3,	dShearVelo_h4,	dShearVelo_h3.Length)
		Array.Copy(dXStrgMod_h3,	dXStrgMod_h4,	dXStrgMod_h3.Length)
		Array.Copy(dXElasMod_h3,	dXElasMod_h4,	dXElasMod_h3.Length)
		Array.Copy(dXLossTan_h3,	dXLossTan_h4,	dXLossTan_h3.Length)
	End Select
	p_head_h(3) = p_head_h(2)
Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End sub
Private sub h2Toh3Copy(ByVal bLTMode As Boolean)
Try
	Select Case bLTMode
	Case True
		If IsNothing(iTempBuf_h2)
			Exit Sub
		End If
		Array.Resize(iMsec_h3,		iMsec_h2.Length)
		Array.Resize(iTempBuf_h3,	iTempBuf_h2.Length)
		Array.Resize(iMvBuf_h3,		iMvBuf_h2.Length)
		Array.Resize(iTempBuf2_h3,	iTempBuf2_h2.Length)
		Array.Resize(iMvBuf2_h3,	iMvBuf2_h2.Length)
		Array.Resize(iHosei_h3,		iHosei_h2.Length)
		Array.Resize(dHoseiP_h3,	dHoseiP_h2.Length)
		Array.Resize(dViscosity_h3,	dViscosity_h2.Length)
		Array.Resize(dXPosPhase_h3,	dXPosPhase_h2.Length)
		Array.Resize(dYPosPhase_h3,	dYPosPhase_h2.Length)
		Array.Resize(dSplStrain_h3,	dSplStrain_h2.Length)
		Array.Resize(dShearMod_h3,	dShearMod_h2.Length)
		Array.Resize(dShearVelo_h3,	dShearVelo_h2.Length)
		Array.Resize(dXStrgMod_h3,	dXStrgMod_h2.Length)
		Array.Resize(dXElasMod_h3,	dXElasMod_h2.Length)
		Array.Resize(dXLossTan_h3,	dXLossTan_h2.Length)

		Array.Copy(iMsec_h2,		iMsec_h3,		iMsec_h2.Length)
		Array.Copy(iTempBuf_h2,		iTempBuf_h3,	iTempBuf_h2.Length)
		Array.Copy(iMvBuf_h2,		iMvBuf_h3,		iMvBuf_h2.Length)
		Array.Copy(iTempBuf2_h2,	iTempBuf2_h3,	iTempBuf2_h2.Length)
		Array.Copy(iMvBuf2_h2,		iMvBuf2_h3,		iMvBuf2_h2.Length)
		Array.Copy(iHosei_h2,		iHosei_h3,		iHosei_h2.Length)
		Array.Copy(dHoseiP_h2,		dHoseiP_h3,		dHoseiP_h2.Length)
		Array.Copy(dViscosity_h2,	dViscosity_h3,	dViscosity_h2.Length)
		Array.Copy(dXPosPhase_h2,	dXPosPhase_h3,	dXPosPhase_h2.Length)
		Array.Copy(dYPosPhase_h2,	dYPosPhase_h3,	dYPosPhase_h2.Length)
		Array.Copy(dSplStrain_h2,	dSplStrain_h3,	dSplStrain_h2.Length)
		Array.Copy(dShearMod_h2,	dShearMod_h3,	dShearMod_h2.Length)
		Array.Copy(dShearVelo_h2,	dShearVelo_h3,	dShearVelo_h2.Length)
		Array.Copy(dXStrgMod_h2,	dXStrgMod_h3,	dXStrgMod_h2.Length)
		Array.Copy(dXElasMod_h2,	dXElasMod_h3,	dXElasMod_h2.Length)
		Array.Copy(dXLossTan_h2,	dXLossTan_h3,	dXLossTan_h2.Length)
	Case Else
		If IsNothing(iTempBuf_h2)
			Exit Sub
		End If
		Array.Resize(iMsec_h3,		iMsec_h2.Length)
		Array.Resize(iOxBuf_h3,		iOxBuf_h2.Length)
		Array.Resize(iOyBuf_h3,		iOyBuf_h2.Length)
		Array.Resize(iOzBuf_h3,		iOzBuf_h2.Length)
		Array.Resize(dOzBuf_h3,		dOzBuf_h2.Length)
		Array.Resize(iPxBuf_h3,		iPxBuf_h2.Length)
		Array.Resize(iPyBuf_h3,		iPyBuf_h2.Length)
		Array.Resize(iPzBuf_h3,		iPzBuf_h2.Length)
		Array.Resize(iTempBuf_h3,	iTempBuf_h2.Length)
		Array.Resize(iMvBuf_h3,		iMvBuf_h2.Length)
		Array.Resize(iTempBuf2_h3,	iTempBuf2_h2.Length)
		Array.Resize(iMvBuf2_h3,	iMvBuf2_h2.Length)
		Array.Resize(iHosei_h3,		iHosei_h2.Length)
		Array.Resize(iSlope_h3,		iSlope_h2.Length)
		Array.Resize(dHoseiP_h3,	dHoseiP_h2.Length)
		Array.Resize(dViscosity_h3,	dViscosity_h2.Length)
		Array.Resize(dXPosPhase_h3,	dXPosPhase_h2.Length)
		Array.Resize(dYPosPhase_h3,	dYPosPhase_h2.Length)
		Array.Resize(dSplStrain_h3,	dSplStrain_h2.Length)
		Array.Resize(dShearMod_h3,	dShearMod_h2.Length)
		Array.Resize(dShearVelo_h3,	dShearVelo_h2.Length)
		Array.Resize(dXStrgMod_h3,	dXStrgMod_h2.Length)
		Array.Resize(dXElasMod_h3,	dXElasMod_h2.Length)
		Array.Resize(dXLossTan_h3,	dXLossTan_h2.Length)

		Array.Copy(iMsec_h2,		iMsec_h3,		iMsec_h2.Length)
		Array.Copy(iOxBuf_h2,		iOxBuf_h3,		iOxBuf_h2.Length)
		Array.Copy(iOyBuf_h2,		iOyBuf_h3,		iOyBuf_h2.Length)
		Array.Copy(iOzBuf_h2,		iOzBuf_h3,		iOzBuf_h2.Length)
		Array.Copy(dOzBuf_h2,		dOzBuf_h3,		dOzBuf_h2.Length)
		Array.Copy(iPxBuf_h2,		iPxBuf_h3,		iPxBuf_h2.Length)
		Array.Copy(iPyBuf_h2,		iPyBuf_h3,		iPyBuf_h2.Length)
		Array.Copy(iPzBuf_h2,		iPzBuf_h3,		iPzBuf_h2.Length)
		Array.Copy(iTempBuf_h2,		iTempBuf_h3,	iTempBuf_h2.Length)
		Array.Copy(iMvBuf_h2,		iMvBuf_h3,		iMvBuf_h2.Length)
		Array.Copy(iTempBuf2_h2,	iTempBuf2_h3,	iTempBuf2_h2.Length)
		Array.Copy(iMvBuf2_h2,		iMvBuf2_h3,		iMvBuf2_h2.Length)
		Array.Copy(iHosei_h2,		iHosei_h3,		iHosei_h2.Length)
		Array.Copy(iSlope_h2,		iSlope_h3,		iSlope_h2.Length)
		Array.Copy(dHoseiP_h2,		dHoseiP_h3,		dHoseiP_h2.Length)
		Array.Copy(dViscosity_h2,	dViscosity_h3,	dViscosity_h2.Length)
		Array.Copy(dXPosPhase_h2,	dXPosPhase_h3,	dXPosPhase_h2.Length)
		Array.Copy(dYPosPhase_h2,	dYPosPhase_h3,	dYPosPhase_h2.Length)
		Array.Copy(dSplStrain_h2,	dSplStrain_h3,	dSplStrain_h2.Length)
		Array.Copy(dShearMod_h2,	dShearMod_h3,	dShearMod_h2.Length)
		Array.Copy(dShearVelo_h2,	dShearVelo_h3,	dShearVelo_h2.Length)
		Array.Copy(dXStrgMod_h2,	dXStrgMod_h3,	dXStrgMod_h2.Length)
		Array.Copy(dXElasMod_h2,	dXElasMod_h3,	dXElasMod_h2.Length)
		Array.Copy(dXLossTan_h2,	dXLossTan_h3,	dXLossTan_h2.Length)
	End Select
	p_head_h(2) = p_head_h(1)
Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End sub
Private sub h1Toh2Copy(ByVal bLTMode As Boolean)
Try
	Select Case bLTMode
	Case True
		If IsNothing(iTempBuf_h1)
			Exit Sub
		End If
		Array.Resize(iMsec_h2,		iMsec_h1.Length)
		Array.Resize(iTempBuf_h2,	iTempBuf_h1.Length)
		Array.Resize(iMvBuf_h2,		iMvBuf_h1.Length)
		Array.Resize(iTempBuf2_h2,	iTempBuf2_h1.Length)
		Array.Resize(iMvBuf2_h2,	iMvBuf2_h1.Length)
		Array.Resize(iHosei_h2,		iHosei_h1.Length)
		Array.Resize(dHoseiP_h2,	dHoseiP_h1.Length)
		Array.Resize(dViscosity_h2,	dViscosity_h1.Length)
		Array.Resize(dXPosPhase_h2,	dXPosPhase_h1.Length)
		Array.Resize(dYPosPhase_h2,	dYPosPhase_h1.Length)
		Array.Resize(dSplStrain_h2,	dSplStrain_h1.Length)
		Array.Resize(dShearMod_h2,	dShearMod_h1.Length)
		Array.Resize(dShearVelo_h2,	dShearVelo_h1.Length)
		Array.Resize(dXStrgMod_h2,	dXStrgMod_h1.Length)
		Array.Resize(dXElasMod_h2,	dXElasMod_h1.Length)
		Array.Resize(dXLossTan_h2,	dXLossTan_h1.Length)

		Array.Copy(iMsec_h1,		iMsec_h2,		iMsec_h1.Length)
		Array.Copy(iTempBuf_h1,		iTempBuf_h2,	iTempBuf_h1.Length)
		Array.Copy(iMvBuf_h1,		iMvBuf_h2,		iMvBuf_h1.Length)
		Array.Copy(iTempBuf2_h1,	iTempBuf2_h2,	iTempBuf2_h1.Length)
		Array.Copy(iMvBuf2_h1,		iMvBuf2_h2,		iMvBuf2_h1.Length)
		Array.Copy(iHosei_h1,		iHosei_h2,		iHosei_h1.Length)
		Array.Copy(dHoseiP_h1,		dHoseiP_h2,		dHoseiP_h1.Length)
		Array.Copy(dViscosity_h1,	dViscosity_h2,	dViscosity_h1.Length)
		Array.Copy(dXPosPhase_h1,	dXPosPhase_h2,	dXPosPhase_h1.Length)
		Array.Copy(dYPosPhase_h1,	dYPosPhase_h2,	dYPosPhase_h1.Length)
		Array.Copy(dSplStrain_h1,	dSplStrain_h2,	dSplStrain_h1.Length)
		Array.Copy(dShearMod_h1,	dShearMod_h2,	dShearMod_h1.Length)
		Array.Copy(dShearVelo_h1,	dShearVelo_h2,	dShearVelo_h1.Length)
		Array.Copy(dXStrgMod_h1,	dXStrgMod_h2,	dXStrgMod_h1.Length)
		Array.Copy(dXElasMod_h1,	dXElasMod_h2,	dXElasMod_h1.Length)
		Array.Copy(dXLossTan_h1,	dXLossTan_h2,	dXLossTan_h1.Length)
	Case Else
		If IsNothing(iTempBuf_h1)
			Exit Sub
		End If
		Array.Resize(iMsec_h2,		iMsec_h1.Length)
		Array.Resize(iOxBuf_h2,		iOxBuf_h1.Length)
		Array.Resize(iOyBuf_h2,		iOyBuf_h1.Length)
		Array.Resize(iOzBuf_h2,		iOzBuf_h1.Length)
		Array.Resize(dOzBuf_h2,		dOzBuf_h1.Length)
		Array.Resize(iPxBuf_h2,		iPxBuf_h1.Length)
		Array.Resize(iPyBuf_h2,		iPyBuf_h1.Length)
		Array.Resize(iPzBuf_h2,		iPzBuf_h1.Length)
		Array.Resize(iTempBuf_h2,	iTempBuf_h1.Length)
		Array.Resize(iMvBuf_h2,		iMvBuf_h1.Length)
		Array.Resize(iTempBuf2_h2,	iTempBuf2_h1.Length)
		Array.Resize(iMvBuf2_h2,	iMvBuf2_h1.Length)
		Array.Resize(iHosei_h2,		iHosei_h1.Length)
		Array.Resize(iSlope_h2,		iSlope_h1.Length)
		Array.Resize(dHoseiP_h2,	dHoseiP_h1.Length)
		Array.Resize(dViscosity_h2,	dViscosity_h1.Length)
		Array.Resize(dXPosPhase_h2,	dXPosPhase_h1.Length)
		Array.Resize(dYPosPhase_h2,	dYPosPhase_h1.Length)
		Array.Resize(dSplStrain_h2,	dSplStrain_h1.Length)
		Array.Resize(dShearMod_h2,	dShearMod_h1.Length)
		Array.Resize(dShearVelo_h2,	dShearVelo_h1.Length)
		Array.Resize(dXStrgMod_h2,	dXStrgMod_h1.Length)
		Array.Resize(dXElasMod_h2,	dXElasMod_h1.Length)
		Array.Resize(dXLossTan_h2,	dXLossTan_h1.Length)

		Array.Copy(iMsec_h1,		iMsec_h2,		iMsec_h1.Length)
		Array.Copy(iOxBuf_h1,		iOxBuf_h2,		iOxBuf_h1.Length)
		Array.Copy(iOyBuf_h1,		iOyBuf_h2,		iOyBuf_h1.Length)
		Array.Copy(iOzBuf_h1,		iOzBuf_h2,		iOzBuf_h1.Length)
		Array.Copy(dOzBuf_h1,		dOzBuf_h2,		dOzBuf_h1.Length)
		Array.Copy(iPxBuf_h1,		iPxBuf_h2,		iPxBuf_h1.Length)
		Array.Copy(iPyBuf_h1,		iPyBuf_h2,		iPyBuf_h1.Length)
		Array.Copy(iPzBuf_h1,		iPzBuf_h2,		iPzBuf_h1.Length)
		Array.Copy(iTempBuf_h1,		iTempBuf_h2,	iTempBuf_h1.Length)
		Array.Copy(iMvBuf_h1,		iMvBuf_h2,		iMvBuf_h1.Length)
		Array.Copy(iTempBuf2_h1,	iTempBuf2_h2,	iTempBuf2_h1.Length)
		Array.Copy(iMvBuf2_h1,		iMvBuf2_h2,		iMvBuf2_h1.Length)
		Array.Copy(iHosei_h1,		iHosei_h2,		iHosei_h1.Length)
		Array.Copy(iSlope_h1,		iSlope_h2,		iSlope_h1.Length)
		Array.Copy(dHoseiP_h1,		dHoseiP_h2,		dHoseiP_h1.Length)
		Array.Copy(dViscosity_h1,	dViscosity_h2,	dViscosity_h1.Length)
		Array.Copy(dXPosPhase_h1,	dXPosPhase_h2,	dXPosPhase_h1.Length)
		Array.Copy(dYPosPhase_h1,	dYPosPhase_h2,	dYPosPhase_h1.Length)
		Array.Copy(dSplStrain_h1,	dSplStrain_h2,	dSplStrain_h1.Length)
		Array.Copy(dShearMod_h1,	dShearMod_h2,	dShearMod_h1.Length)
		Array.Copy(dShearVelo_h1,	dShearVelo_h2,	dShearVelo_h1.Length)
		Array.Copy(dXStrgMod_h1,	dXStrgMod_h2,	dXStrgMod_h1.Length)
		Array.Copy(dXElasMod_h1,	dXElasMod_h2,	dXElasMod_h1.Length)
		Array.Copy(dXLossTan_h1,	dXLossTan_h2,	dXLossTan_h1.Length)
	End Select
	p_head_h(1) = p_head_h(0)
Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End sub

Private sub h0Toh1Copy(ByVal bLTMode As Boolean)
Try
	Select Case bLTMode
	Case True
		If IsNothing(iLT_MSec)
			Exit Sub
		End If
		Array.Resize(iMSec_h1,		iLT_MSec.Length)
		Array.Resize(iTempBuf_h1,	iLT_TempBuf.Length)
		Array.Resize(iMvBuf_h1,		iLT_MvBuf.Length)
		Array.Resize(iTempBuf2_h1,	iLT_TempBuf2.Length)
		Array.Resize(iMvBuf2_h1,	iLT_MvBuf2.Length)
		Array.Resize(iHosei_h1,		iLT_Hosei.Length)
		Array.Resize(dHoseiP_h1,	dLT_Hosei100.Length)
		Array.Resize(dViscosity_h1,	dLT_Viscosity.Length)
		Array.Resize(dXPosPhase_h1,	dLT_XPosPhase.Length)
		Array.Resize(dYPosPhase_h1,	dLT_YPosPhase.Length)
		Array.Resize(dSplStrain_h1,	dLT_SplStrain.Length)
		Array.Resize(dShearMod_h1,	dLT_ShearMod.Length)
		Array.Resize(dShearVelo_h1,	dLT_ShearVelo.Length)
		Array.Resize(dXStrgMod_h1,	dLT_XStrgMod.Length)
		Array.Resize(dXElasMod_h1,	dLT_XElasMod.Length)
		Array.Resize(dXLossTan_h1,	dLT_XLossTan.Length)

		Array.Copy(iLT_Msec,		iMsec_h1,		iLT_MSec.Length)
		Array.Copy(iLT_TempBuf,		iTempBuf_h1,	iLT_TempBuf.Length)
		Array.Copy(iLT_MvBuf,		iMvBuf_h1,		iLT_MvBuf.Length)
		Array.Copy(iLT_TempBuf2,	iTempBuf2_h1,	iLT_TempBuf2.Length)
		Array.Copy(iLT_MvBuf2,		iMvBuf2_h1,		iLT_MvBuf2.Length)
		Array.Copy(iLT_Hosei,		iHosei_h1,		iLT_Hosei.Length)
		Array.Copy(dLT_Hosei100,	dHoseiP_h1,		dLT_Hosei100.Length)
		Array.Copy(dLT_Viscosity,	dViscosity_h1,	dLT_Viscosity.Length)
		Array.Copy(dLT_XPosPhase,	dXPosPhase_h1,	dLT_XPosPhase.Length)
		Array.Copy(dLT_YPosPhase,	dYPosPhase_h1,	dLT_YPosPhase.Length)
		Array.Copy(dLT_SplStrain,	dSplStrain_h1,	dLT_SplStrain.Length)
		Array.Copy(dLT_ShearMod,	dShearMod_h1,	dLT_ShearMod.Length)
		Array.Copy(dLT_ShearVelo,	dShearVelo_h1,	dLT_ShearVelo.Length)
		Array.Copy(dLT_XStrgMod,	dXStrgMod_h1,	dLT_XStrgMod.Length)
		Array.Copy(dLT_XElasMod,	dXElasMod_h1,	dLT_XElasMod.Length)
		Array.Copy(dLT_XLossTan,	dXLossTan_h1,	dLT_XLossTan.Length)
	Case Else
		If IsNothing(iOxBuf)
			Exit Sub
		End If
		Array.Resize(iMSec_h1,		iMSec.Length)
		Array.Resize(iOxBuf_h1,		iOxBuf.Length)
		Array.Resize(iOyBuf_h1,		iOyBuf.Length)
		Array.Resize(iOzBuf_h1,		iOzBuf.Length)
		Array.Resize(dOzBuf_h1,		dOzBuf.Length)
		Array.Resize(iPxBuf_h1,		iPxBuf.Length)
		Array.Resize(iPyBuf_h1,		iPyBuf.Length)
		Array.Resize(iPzBuf_h1,		iPzBuf.Length)
		Array.Resize(iTempBuf_h1,	iTempBuf.Length)
		Array.Resize(iMvBuf_h1,		iMvBuf.Length)
		Array.Resize(iTempBuf2_h1,	iTempBuf2.Length)
		Array.Resize(iMvBuf2_h1,	iMvBuf2.Length)
		Array.Resize(iHosei_h1,		iHosei.Length)
		Array.Resize(iSlope_h1,		iSlope.Length)
		Array.Resize(dHoseiP_h1,	dHosei100.Length)
		Array.Resize(dViscosity_h1,	dViscosity.Length)
		Array.Resize(dXPosPhase_h1,	dXPosPhase.Length)
		Array.Resize(dYPosPhase_h1,	dYPosPhase.Length)
		Array.Resize(dSplStrain_h1,	dSplStrain.Length)
		Array.Resize(dShearMod_h1,	dShearMod.Length)
		Array.Resize(dShearVelo_h1,	dShearVelo.Length)
		Array.Resize(dXStrgMod_h1,	dXStrgMod.Length)
		Array.Resize(dXElasMod_h1,	dXElasMod.Length)
		Array.Resize(dXLossTan_h1,	dXLossTan.Length)

		Array.Copy(iMsec,		iMsec_h1,		iMSec.Length)
		Array.Copy(iOxBuf,		iOxBuf_h1,		iOxBuf.Length)
		Array.Copy(iOyBuf,		iOyBuf_h1,		iOyBuf.Length)
		Array.Copy(iOzBuf,		iOzBuf_h1,		iOzBuf.Length)
		Array.Copy(dOzBuf,		dOzBuf_h1,		dOzBuf.Length)
		Array.Copy(iPxBuf,		iPxBuf_h1,		iPxBuf.Length)
		Array.Copy(iPyBuf,		iPyBuf_h1,		iPyBuf.Length)
		Array.Copy(iPzBuf,		iPzBuf_h1,		iPzBuf.Length)
		Array.Copy(iTempBuf,	iTempBuf_h1,	iTempBuf.Length)
		Array.Copy(iMvBuf,		iMvBuf_h1,		iMvBuf.Length)
		Array.Copy(iTempBuf2,	iTempBuf2_h1,	iTempBuf2.Length)
		Array.Copy(iMvBuf2,		iMvBuf2_h1,		iMvBuf2.Length)
		Array.Copy(iHosei,		iHosei_h1,		iHosei.Length)
		Array.Copy(iSlope,		iSlope_h1,		iSlope.Length)
		Array.Copy(dHosei100,	dHoseiP_h1,		dHosei100.Length)
		Array.Copy(dViscosity,	dViscosity_h1,	dViscosity.Length)
		Array.Copy(dXPosPhase,	dXPosPhase_h1,	dXPosPhase.Length)
		Array.Copy(dYPosPhase,	dYPosPhase_h1,	dYPosPhase.Length)
		Array.Copy(dSplStrain,	dSplStrain_h1,	dSplStrain.Length)
		Array.Copy(dShearMod,	dShearMod_h1,	dShearMod.Length)
		Array.Copy(dShearVelo,	dShearVelo_h1,	dShearVelo.Length)
		Array.Copy(dXStrgMod,	dXStrgMod_h1,	dXStrgMod.Length)
		Array.Copy(dXElasMod,	dXElasMod_h1,	dXElasMod.Length)
		Array.Copy(dXLossTan,	dXLossTan_h1,	dXLossTan.Length)
	End Select
	p_head_h(0) = p_head
Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End sub




#If 0
PubLic sub DataTrim()
Try
	If IsNothing(iOxBuf)
		Return
	End If
	if iOxBuf.Length - MEAS_BLOCK_DATA_NUM > 0
		Array.Resize(iOxBuf,		iOxBuf.Length		- MEAS_BLOCK_DATA_NUM)
		Array.Resize(iOyBuf,		iOyBuf.Length		- MEAS_BLOCK_DATA_NUM)
		Array.Resize(iOzBuf,		iOzBuf.Length		- MEAS_BLOCK_DATA_NUM)
		Array.Resize(dOzBuf,		dOzBuf.Length		- MEAS_BLOCK_DATA_NUM)
		Array.Resize(iPxBuf,		iPxBuf.Length		- MEAS_BLOCK_DATA_NUM)
		Array.Resize(iPyBuf,		iPyBuf.Length		- MEAS_BLOCK_DATA_NUM)
		Array.Resize(iPzBuf,		iPzBuf.Length		- MEAS_BLOCK_DATA_NUM)
		Array.Resize(iTempBuf,		iTempBuf.Length		- MEAS_BLOCK_DATA_NUM)
		Array.Resize(iMvBuf,		iMvBuf.Length		- MEAS_BLOCK_DATA_NUM)
		Array.Resize(iTempBuf2,		iTempBuf2.Length	- MEAS_BLOCK_DATA_NUM)
		Array.Resize(iMvBuf2,		iMvBuf2.Length		- MEAS_BLOCK_DATA_NUM)
		Array.Resize(iHosei,		iHosei.Length		- MEAS_BLOCK_DATA_NUM)
		Array.Resize(iSlope,		iSlope.Length		- MEAS_BLOCK_DATA_NUM)
		Array.Resize(dHosei100,		dHosei100.Length	- MEAS_BLOCK_DATA_NUM)
		Array.Resize(dViscosity,	dViscosity.Length	- MEAS_BLOCK_DATA_NUM)
		Array.Resize(dXPosPhase,	dXPosPhase.Length	- MEAS_BLOCK_DATA_NUM)
		Array.Resize(dYPosPhase,	dYPosPhase.Length	- MEAS_BLOCK_DATA_NUM)
		Array.Resize(dSplStrain,	dSplStrain.Length	- MEAS_BLOCK_DATA_NUM)
		Array.Resize(dShearMod,		dShearMod.Length	- MEAS_BLOCK_DATA_NUM)
		Array.Resize(dShearVelo,	dShearVelo.Length	- MEAS_BLOCK_DATA_NUM)
		Array.Resize(dXStrgMod,		dXStrgMod.Length	- MEAS_BLOCK_DATA_NUM)
		Array.Resize(dXElasMod,		dXElasMod.Length	- MEAS_BLOCK_DATA_NUM)
		Array.Resize(dXLossTan,		dXLossTan.Length	- MEAS_BLOCK_DATA_NUM)

		Array.Resize(dYPosPhase,	dYPosPhase.Length	- MEAS_BLOCK_DATA_NUM)		'Y位相差
		Array.Resize(dYStrgMod,		dYStrgMod.Length	- MEAS_BLOCK_DATA_NUM)		'Y貯蔵弾性率
		Array.Resize(dYElasMod,		dYElasMod.Length	- MEAS_BLOCK_DATA_NUM)		'Y損失弾性率
		Array.Resize(dYLossTan,		dYLossTan.Length	- MEAS_BLOCK_DATA_NUM)		'Y損失正接

	End If
Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End sub
#end if


''' <summary>
''' 配列の末尾N要素がすべて0の場合、最後の非0値で埋める
''' </summary>
''' <typeparam name="T">Double または Integer</typeparam>
''' <param name="iKind">0-6</param>
''' <param name="arr">対象配列</param>
''' <param name="tailCount">末尾チェック数（例：500）</param>
Public Sub FillLastZeroTail(Of T As Structure)(ByVal iKind As Int16, arr As T(), tailCount As Integer)
Try
    If arr Is Nothing OrElse arr.Length < tailCount Then Exit Sub
    Dim zero As T = Activator.CreateInstance(Of T)()

    ' ① 末尾tailCountが全て0か確認
    For i As Integer = arr.Length - tailCount To arr.Length - 1
        If Not arr(i).Equals(zero) Then
            Exit Sub   ' 0以外が含まれていれば何もしない
        End If
    Next

    ' ② 最後の非0値を探す
    Dim lastValue As T = zero
    Dim found As Boolean = False
    For i As Integer = arr.Length - tailCount - 1 To 0 Step -1
        If Not arr(i).Equals(zero) Then
            lastValue = arr(i)
            found = True
            Exit For
        End If
    Next
    If Not found Then Exit Sub   ' 有値が見つからない場合

    ' ③ 末尾tailCountを埋める
    For i As Integer = arr.Length - tailCount To arr.Length - 1
        arr(i) = lastValue
    Next
	Dim s() As String = {"OxBuf","OyBuf","OzBuf","PxBuf","PyBuf","PzBuf"}
	Call WriteLog($"{s(iKind)}のバッファで{tailCount}ポイントの連続0を検知し最終値で埋めた。", eLOG_LEVEL.iWARNING)
	Exit Sub
Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub



Private sub h5Toh4Copy(ByVal bLTMode As Boolean)
Try
	Select Case bLTMode
	Case True
		If IsNothing(iTempBuf_h5)
			h4Clear()
			p_head_h(3) = Nothing
			Exit Sub
		End If
		Array.Resize(iMSec_h4,		iMSec_h5.Length)
		Array.Resize(iTempBuf_h4,	iTempBuf_h5.Length)
		Array.Resize(iMvBuf_h4,		iMvBuf_h5.Length)
		Array.Resize(iTempBuf2_h4,	iTempBuf2_h5.Length)
		Array.Resize(iMvBuf2_h4,	iMvBuf2_h5.Length)
		Array.Resize(iHosei_h4,		iHosei_h5.Length)
		Array.Resize(dHoseiP_h4,	dHoseiP_h5.Length)
		Array.Resize(dViscosity_h4,	dViscosity_h5.Length)
		Array.Resize(dXPosPhase_h4,	dXPosPhase_h5.Length)
		Array.Resize(dYPosPhase_h4,	dYPosPhase_h5.Length)
		Array.Resize(dSplStrain_h4,	dSplStrain_h5.Length)
		Array.Resize(dShearMod_h4,	dShearMod_h5.Length)
		Array.Resize(dShearVelo_h4,	dShearVelo_h5.Length)
		Array.Resize(dXStrgMod_h4,	dXStrgMod_h5.Length)
		Array.Resize(dXElasMod_h4,	dXElasMod_h5.Length)
		Array.Resize(dXLossTan_h4,	dXLossTan_h5.Length)

		Array.Copy(iMSec_h5,		iMSec_h4,		iMSec_h5.Length)
		Array.Copy(iTempBuf_h5,		iTempBuf_h4,	iTempBuf_h5.Length)
		Array.Copy(iMvBuf_h5,		iMvBuf_h4,		iMvBuf_h5.Length)
		Array.Copy(iTempBuf2_h5,	iTempBuf2_h4,	iTempBuf2_h5.Length)
		Array.Copy(iMvBuf2_h5,		iMvBuf2_h4,		iMvBuf2_h5.Length)
		Array.Copy(iHosei_h5,		iHosei_h4,		iHosei_h5.Length)
		Array.Copy(dHoseiP_h5,		dHoseiP_h4,		dHoseiP_h5.Length)
		Array.Copy(dViscosity_h5,	dViscosity_h4,	dViscosity_h5.Length)
		Array.Copy(dXPosPhase_h5,	dXPosPhase_h4,	dXPosPhase_h5.Length)
		Array.Copy(dYPosPhase_h5,	dYPosPhase_h4,	dYPosPhase_h5.Length)
		Array.Copy(dSplStrain_h5,	dSplStrain_h4,	dSplStrain_h5.Length)
		Array.Copy(dShearMod_h5,	dShearMod_h4,	dShearMod_h5.Length)
		Array.Copy(dShearVelo_h5,	dShearVelo_h4,	dShearVelo_h5.Length)
		Array.Copy(dXStrgMod_h5,	dXStrgMod_h4,	dXStrgMod_h5.Length)
		Array.Copy(dXElasMod_h5,	dXElasMod_h4,	dXElasMod_h5.Length)
		Array.Copy(dXLossTan_h5,	dXLossTan_h4,	dXLossTan_h5.Length)
	Case Else
		If IsNothing(iOxBuf_h5)
			h4Clear()
			p_head_h(3) = Nothing
			Exit Sub
		End If
		Array.Resize(iOxBuf_h4,		iOxBuf_h5.Length)
		Array.Resize(iOyBuf_h4,		iOyBuf_h5.Length)
		Array.Resize(iOzBuf_h4,		iOzBuf_h5.Length)
		Array.Resize(dOzBuf_h4,		dOzBuf_h5.Length)
		Array.Resize(iPxBuf_h4,		iPxBuf_h5.Length)
		Array.Resize(iPyBuf_h4,		iPyBuf_h5.Length)
		Array.Resize(iPzBuf_h4,		iPzBuf_h5.Length)
		Array.Resize(iTempBuf_h4,	iTempBuf_h5.Length)
		Array.Resize(iMvBuf_h4,		iMvBuf_h5.Length)
		Array.Resize(iTempBuf2_h4,	iTempBuf2_h5.Length)
		Array.Resize(iMvBuf2_h4,	iMvBuf2_h5.Length)
		Array.Resize(iHosei_h4,		iHosei_h5.Length)
		Array.Resize(iSlope_h4,		iSlope_h5.Length)
		Array.Resize(dHoseiP_h4,	dHoseiP_h5.Length)
		Array.Resize(dViscosity_h4,	dViscosity_h5.Length)
		Array.Resize(dXPosPhase_h4,	dXPosPhase_h5.Length)
		Array.Resize(dYPosPhase_h4,	dYPosPhase_h5.Length)
		Array.Resize(dSplStrain_h4,	dSplStrain_h5.Length)
		Array.Resize(dShearMod_h4,	dShearMod_h5.Length)
		Array.Resize(dShearVelo_h4,	dShearVelo_h5.Length)
		Array.Resize(dXStrgMod_h4,	dXStrgMod_h5.Length)
		Array.Resize(dXElasMod_h4,	dXElasMod_h5.Length)
		Array.Resize(dXLossTan_h4,	dXLossTan_h5.Length)

		Array.Copy(iOxBuf_h5,		iOxBuf_h4,		iOxBuf_h5.Length)
		Array.Copy(iOyBuf_h5,		iOyBuf_h4,		iOyBuf_h5.Length)
		Array.Copy(iOzBuf_h5,		iOzBuf_h4,		iOzBuf_h5.Length)
		Array.Copy(dOzBuf_h5,		dOzBuf_h4,		dOzBuf_h5.Length)
		Array.Copy(iPxBuf_h5,		iPxBuf_h4,		iPxBuf_h5.Length)
		Array.Copy(iPyBuf_h5,		iPyBuf_h4,		iPyBuf_h5.Length)
		Array.Copy(iPzBuf_h5,		iPzBuf_h4,		iPzBuf_h5.Length)
		Array.Copy(iTempBuf_h5,		iTempBuf_h4,	iTempBuf_h5.Length)
		Array.Copy(iMvBuf_h5,		iMvBuf_h4,		iMvBuf_h5.Length)
		Array.Copy(iTempBuf2_h5,	iTempBuf2_h4,	iTempBuf2_h5.Length)
		Array.Copy(iMvBuf2_h5,		iMvBuf2_h4,		iMvBuf2_h5.Length)
		Array.Copy(iHosei_h5,		iHosei_h4,		iHosei_h5.Length)
		Array.Copy(iSlope_h5,		iSlope_h4,		iSlope_h5.Length)
		Array.Copy(dHoseiP_h5,		dHoseiP_h4,		dHoseiP_h5.Length)
		Array.Copy(dViscosity_h5,	dViscosity_h4,	dViscosity_h5.Length)
		Array.Copy(dXPosPhase_h5,	dXPosPhase_h4,	dXPosPhase_h5.Length)
		Array.Copy(dYPosPhase_h5,	dYPosPhase_h4,	dYPosPhase_h5.Length)
		Array.Copy(dSplStrain_h5,	dSplStrain_h4,	dSplStrain_h5.Length)
		Array.Copy(dShearMod_h5,	dShearMod_h4,	dShearMod_h5.Length)
		Array.Copy(dShearVelo_h5,	dShearVelo_h4,	dShearVelo_h5.Length)
		Array.Copy(dXStrgMod_h5,	dXStrgMod_h4,	dXStrgMod_h5.Length)
		Array.Copy(dXElasMod_h5,	dXElasMod_h4,	dXElasMod_h5.Length)
		Array.Copy(dXLossTan_h5,	dXLossTan_h4,	dXLossTan_h5.Length)
	End Select
	p_head_h(3) = p_head_h(4)
Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End sub
Private sub h4Toh3Copy(ByVal bLTMode As Boolean)
Try
	Select Case bLTMode
	Case True
		If IsNothing(iTempBuf_h4)
			h3Clear()
			p_head_h(2) = Nothing
			Exit Sub
		End If
		Array.Resize(iMSec_h3,		iMSec_h4.Length)
		Array.Resize(iTempBuf_h3,	iTempBuf_h4.Length)
		Array.Resize(iMvBuf_h3,		iMvBuf_h4.Length)
		Array.Resize(iTempBuf2_h3,	iTempBuf2_h4.Length)
		Array.Resize(iMvBuf2_h3,	iMvBuf2_h4.Length)
		Array.Resize(iHosei_h3,		iHosei_h4.Length)
		Array.Resize(dHoseiP_h3,	dHoseiP_h4.Length)
		Array.Resize(dViscosity_h3,	dViscosity_h4.Length)
		Array.Resize(dXPosPhase_h3,	dXPosPhase_h4.Length)
		Array.Resize(dYPosPhase_h3,	dYPosPhase_h4.Length)
		Array.Resize(dSplStrain_h3,	dSplStrain_h4.Length)
		Array.Resize(dShearMod_h3,	dShearMod_h4.Length)
		Array.Resize(dShearVelo_h3,	dShearVelo_h4.Length)
		Array.Resize(dXStrgMod_h3,	dXStrgMod_h4.Length)
		Array.Resize(dXElasMod_h3,	dXElasMod_h4.Length)
		Array.Resize(dXLossTan_h3,	dXLossTan_h4.Length)

		Array.Copy(iMSec_h4,		iMSec_h3,		iMSec_h4.Length)
		Array.Copy(iTempBuf_h4,		iTempBuf_h3,	iTempBuf_h4.Length)
		Array.Copy(iMvBuf_h4,		iMvBuf_h3,		iMvBuf_h4.Length)
		Array.Copy(iTempBuf2_h4,	iTempBuf2_h3,	iTempBuf2_h4.Length)
		Array.Copy(iMvBuf2_h4,		iMvBuf2_h3,		iMvBuf2_h4.Length)
		Array.Copy(iHosei_h4,		iHosei_h3,		iHosei_h4.Length)
		Array.Copy(dHoseiP_h4,		dHoseiP_h3,		dHoseiP_h4.Length)
		Array.Copy(dViscosity_h4,	dViscosity_h3,	dViscosity_h4.Length)
		Array.Copy(dXPosPhase_h4,	dXPosPhase_h3,	dXPosPhase_h4.Length)
		Array.Copy(dYPosPhase_h4,	dYPosPhase_h3,	dYPosPhase_h4.Length)
		Array.Copy(dSplStrain_h4,	dSplStrain_h3,	dSplStrain_h4.Length)
		Array.Copy(dShearMod_h4,	dShearMod_h3,	dShearMod_h4.Length)
		Array.Copy(dShearVelo_h4,	dShearVelo_h3,	dShearVelo_h4.Length)
		Array.Copy(dXStrgMod_h4,	dXStrgMod_h3,	dXStrgMod_h4.Length)
		Array.Copy(dXElasMod_h4,	dXElasMod_h3,	dXElasMod_h4.Length)
		Array.Copy(dXLossTan_h4,	dXLossTan_h3,	dXLossTan_h4.Length)
	Case Else
		If IsNothing(iOxBuf_h4)
			h3Clear()
			p_head_h(2) = Nothing
			Exit Sub
		End If
		Array.Resize(iOxBuf_h3,		iOxBuf_h4.Length)
		Array.Resize(iOyBuf_h3,		iOyBuf_h4.Length)
		Array.Resize(iOzBuf_h3,		iOzBuf_h4.Length)
		Array.Resize(dOzBuf_h3,		dOzBuf_h4.Length)
		Array.Resize(iPxBuf_h3,		iPxBuf_h4.Length)
		Array.Resize(iPyBuf_h3,		iPyBuf_h4.Length)
		Array.Resize(iPzBuf_h3,		iPzBuf_h4.Length)
		Array.Resize(iTempBuf_h3,	iTempBuf_h4.Length)
		Array.Resize(iMvBuf_h3,		iMvBuf_h4.Length)
		Array.Resize(iTempBuf2_h3,	iTempBuf2_h4.Length)
		Array.Resize(iMvBuf2_h3,	iMvBuf2_h4.Length)
		Array.Resize(iHosei_h3,		iHosei_h4.Length)
		Array.Resize(iSlope_h3,		iSlope_h4.Length)
		Array.Resize(dHoseiP_h3,	dHoseiP_h4.Length)
		Array.Resize(dViscosity_h3,	dViscosity_h4.Length)
		Array.Resize(dXPosPhase_h3,	dXPosPhase_h4.Length)
		Array.Resize(dYPosPhase_h3,	dYPosPhase_h4.Length)
		Array.Resize(dSplStrain_h3,	dSplStrain_h4.Length)
		Array.Resize(dShearMod_h3,	dShearMod_h4.Length)
		Array.Resize(dShearVelo_h3,	dShearVelo_h4.Length)
		Array.Resize(dXStrgMod_h3,	dXStrgMod_h4.Length)
		Array.Resize(dXElasMod_h3,	dXElasMod_h4.Length)
		Array.Resize(dXLossTan_h3,	dXLossTan_h4.Length)

		Array.Copy(iOxBuf_h4,		iOxBuf_h3,		iOxBuf_h4.Length)
		Array.Copy(iOyBuf_h4,		iOyBuf_h3,		iOyBuf_h4.Length)
		Array.Copy(iOzBuf_h4,		iOzBuf_h3,		iOzBuf_h4.Length)
		Array.Copy(dOzBuf_h4,		dOzBuf_h3,		dOzBuf_h4.Length)
		Array.Copy(iPxBuf_h4,		iPxBuf_h3,		iPxBuf_h4.Length)
		Array.Copy(iPyBuf_h4,		iPyBuf_h3,		iPyBuf_h4.Length)
		Array.Copy(iPzBuf_h4,		iPzBuf_h3,		iPzBuf_h4.Length)
		Array.Copy(iTempBuf_h4,		iTempBuf_h3,	iTempBuf_h4.Length)
		Array.Copy(iMvBuf_h4,		iMvBuf_h3,		iMvBuf_h4.Length)
		Array.Copy(iTempBuf2_h4,	iTempBuf2_h3,	iTempBuf2_h4.Length)
		Array.Copy(iMvBuf2_h4,		iMvBuf2_h3,		iMvBuf2_h4.Length)
		Array.Copy(iHosei_h4,		iHosei_h3,		iHosei_h4.Length)
		Array.Copy(iSlope_h4,		iSlope_h3,		iSlope_h4.Length)
		Array.Copy(dHoseiP_h4,		dHoseiP_h3,		dHoseiP_h4.Length)
		Array.Copy(dViscosity_h4,	dViscosity_h3,	dViscosity_h4.Length)
		Array.Copy(dXPosPhase_h4,	dXPosPhase_h3,	dXPosPhase_h4.Length)
		Array.Copy(dYPosPhase_h4,	dYPosPhase_h3,	dYPosPhase_h4.Length)
		Array.Copy(dSplStrain_h4,	dSplStrain_h3,	dSplStrain_h4.Length)
		Array.Copy(dShearMod_h4,	dShearMod_h3,	dShearMod_h4.Length)
		Array.Copy(dShearVelo_h4,	dShearVelo_h3,	dShearVelo_h4.Length)
		Array.Copy(dXStrgMod_h4,	dXStrgMod_h3,	dXStrgMod_h4.Length)
		Array.Copy(dXElasMod_h4,	dXElasMod_h3,	dXElasMod_h4.Length)
		Array.Copy(dXLossTan_h4,	dXLossTan_h3,	dXLossTan_h4.Length)
	End Select
	p_head_h(2) = p_head_h(3)
Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End sub
Private sub h3Toh2Copy(ByVal bLTMode As Boolean)
Try
	Select Case bLTMode
	Case True
		If IsNothing(iTempBuf_h3)
			h2Clear()
			p_head_h(1) = Nothing
			Exit Sub
		End If
		Array.Resize(iMSec_h2,		iMSec_h3.Length)
		Array.Resize(iTempBuf_h2,	iTempBuf_h3.Length)
		Array.Resize(iMvBuf_h2,		iMvBuf_h3.Length)
		Array.Resize(iTempBuf2_h2,	iTempBuf2_h3.Length)
		Array.Resize(iMvBuf2_h2,	iMvBuf2_h3.Length)
		Array.Resize(iHosei_h2,		iHosei_h3.Length)
		Array.Resize(dHoseiP_h2,	dHoseiP_h3.Length)
		Array.Resize(dViscosity_h2,	dViscosity_h3.Length)
		Array.Resize(dXPosPhase_h2,	dXPosPhase_h3.Length)
		Array.Resize(dYPosPhase_h2,	dYPosPhase_h3.Length)
		Array.Resize(dSplStrain_h2,	dSplStrain_h3.Length)
		Array.Resize(dShearMod_h2,	dShearMod_h3.Length)
		Array.Resize(dShearVelo_h2,	dShearVelo_h3.Length)
		Array.Resize(dXStrgMod_h2,	dXStrgMod_h3.Length)
		Array.Resize(dXElasMod_h2,	dXElasMod_h3.Length)
		Array.Resize(dXLossTan_h2,	dXLossTan_h3.Length)

		Array.Copy(iMsec_h3,		iMsec_h2,		iMsec_h3.Length)
		Array.Copy(iTempBuf_h3,		iTempBuf_h2,	iTempBuf_h3.Length)
		Array.Copy(iMvBuf_h3,		iMvBuf_h2,		iMvBuf_h3.Length)
		Array.Copy(iTempBuf2_h3,	iTempBuf2_h2,	iTempBuf2_h3.Length)
		Array.Copy(iMvBuf2_h3,		iMvBuf2_h2,		iMvBuf2_h3.Length)
		Array.Copy(iHosei_h3,		iHosei_h2,		iHosei_h3.Length)
		Array.Copy(dHoseiP_h3,		dHoseiP_h2,		dHoseiP_h3.Length)
		Array.Copy(dViscosity_h3,	dViscosity_h2,	dViscosity_h3.Length)
		Array.Copy(dXPosPhase_h3,	dXPosPhase_h2,	dXPosPhase_h3.Length)
		Array.Copy(dYPosPhase_h3,	dYPosPhase_h2,	dYPosPhase_h3.Length)
		Array.Copy(dSplStrain_h3,	dSplStrain_h2,	dSplStrain_h3.Length)
		Array.Copy(dShearMod_h3,	dShearMod_h2,	dShearMod_h3.Length)
		Array.Copy(dShearVelo_h3,	dShearVelo_h2,	dShearVelo_h3.Length)
		Array.Copy(dXStrgMod_h3,	dXStrgMod_h2,	dXStrgMod_h3.Length)
		Array.Copy(dXElasMod_h3,	dXElasMod_h2,	dXElasMod_h3.Length)
		Array.Copy(dXLossTan_h3,	dXLossTan_h2,	dXLossTan_h3.Length)
	Case Else
		If IsNothing(iOxBuf_h3)
			h2Clear()
			p_head_h(1) = Nothing
			Exit Sub
		End If
		Array.Resize(iOxBuf_h2,		iOxBuf_h3.Length)
		Array.Resize(iOyBuf_h2,		iOyBuf_h3.Length)
		Array.Resize(iOzBuf_h2,		iOzBuf_h3.Length)
		Array.Resize(dOzBuf_h2,		dOzBuf_h3.Length)
		Array.Resize(iPxBuf_h2,		iPxBuf_h3.Length)
		Array.Resize(iPyBuf_h2,		iPyBuf_h3.Length)
		Array.Resize(iPzBuf_h2,		iPzBuf_h3.Length)
		Array.Resize(iTempBuf_h2,	iTempBuf_h3.Length)
		Array.Resize(iMvBuf_h2,		iMvBuf_h3.Length)
		Array.Resize(iTempBuf2_h2,	iTempBuf2_h3.Length)
		Array.Resize(iMvBuf2_h2,	iMvBuf2_h3.Length)
		Array.Resize(iHosei_h2,		iHosei_h3.Length)
		Array.Resize(iSlope_h2,		iSlope_h3.Length)
		Array.Resize(dHoseiP_h2,	dHoseiP_h3.Length)
		Array.Resize(dViscosity_h2,	dViscosity_h3.Length)
		Array.Resize(dXPosPhase_h2,	dXPosPhase_h3.Length)
		Array.Resize(dYPosPhase_h2,	dYPosPhase_h3.Length)
		Array.Resize(dSplStrain_h2,	dSplStrain_h3.Length)
		Array.Resize(dShearMod_h2,	dShearMod_h3.Length)
		Array.Resize(dShearVelo_h2,	dShearVelo_h3.Length)
		Array.Resize(dXStrgMod_h2,	dXStrgMod_h3.Length)
		Array.Resize(dXElasMod_h2,	dXElasMod_h3.Length)
		Array.Resize(dXLossTan_h2,	dXLossTan_h3.Length)

		Array.Copy(iOxBuf_h3,		iOxBuf_h2,		iOxBuf_h3.Length)
		Array.Copy(iOyBuf_h3,		iOyBuf_h2,		iOyBuf_h3.Length)
		Array.Copy(iOzBuf_h3,		iOzBuf_h2,		iOzBuf_h3.Length)
		Array.Copy(dOzBuf_h3,		dOzBuf_h2,		dOzBuf_h3.Length)
		Array.Copy(iPxBuf_h3,		iPxBuf_h2,		iPxBuf_h3.Length)
		Array.Copy(iPyBuf_h3,		iPyBuf_h2,		iPyBuf_h3.Length)
		Array.Copy(iPzBuf_h3,		iPzBuf_h2,		iPzBuf_h3.Length)
		Array.Copy(iTempBuf_h3,		iTempBuf_h2,	iTempBuf_h3.Length)
		Array.Copy(iMvBuf_h3,		iMvBuf_h2,		iMvBuf_h3.Length)
		Array.Copy(iTempBuf2_h3,	iTempBuf2_h2,	iTempBuf2_h3.Length)
		Array.Copy(iMvBuf2_h3,		iMvBuf2_h2,		iMvBuf2_h3.Length)
		Array.Copy(iHosei_h3,		iHosei_h2,		iHosei_h3.Length)
		Array.Copy(iSlope_h3,		iSlope_h2,		iSlope_h3.Length)
		Array.Copy(dHoseiP_h3,		dHoseiP_h2,		dHoseiP_h3.Length)
		Array.Copy(dViscosity_h3,	dViscosity_h2,	dViscosity_h3.Length)
		Array.Copy(dXPosPhase_h3,	dXPosPhase_h2,	dXPosPhase_h3.Length)
		Array.Copy(dYPosPhase_h3,	dYPosPhase_h2,	dYPosPhase_h3.Length)
		Array.Copy(dSplStrain_h3,	dSplStrain_h2,	dSplStrain_h3.Length)
		Array.Copy(dShearMod_h3,	dShearMod_h2,	dShearMod_h3.Length)
		Array.Copy(dShearVelo_h3,	dShearVelo_h2,	dShearVelo_h3.Length)
		Array.Copy(dXStrgMod_h3,	dXStrgMod_h2,	dXStrgMod_h3.Length)
		Array.Copy(dXElasMod_h3,	dXElasMod_h2,	dXElasMod_h3.Length)
		Array.Copy(dXLossTan_h3,	dXLossTan_h2,	dXLossTan_h3.Length)
	End Select
	p_head_h(1) = p_head_h(2)
Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End sub
Private sub h2Toh1Copy(ByVal bLTMode As Boolean)
Try
	Select Case bLTMode
	Case True
		If IsNothing(iTempBuf_h2)
			h1Clear()
			p_head_h(0) = Nothing
			Exit Sub
		End If
		Array.Resize(iMsec_h1,		iMsec_h2.Length)
		Array.Resize(iTempBuf_h1,	iTempBuf_h2.Length)
		Array.Resize(iMvBuf_h1,		iMvBuf_h2.Length)
		Array.Resize(iTempBuf2_h1,	iTempBuf2_h2.Length)
		Array.Resize(iMvBuf2_h1,	iMvBuf2_h2.Length)
		Array.Resize(iHosei_h1,		iHosei_h2.Length)
		Array.Resize(dHoseiP_h1,	dHoseiP_h2.Length)
		Array.Resize(dViscosity_h1,	dViscosity_h2.Length)
		Array.Resize(dXPosPhase_h1,	dXPosPhase_h2.Length)
		Array.Resize(dYPosPhase_h1,	dYPosPhase_h2.Length)
		Array.Resize(dSplStrain_h1,	dSplStrain_h2.Length)
		Array.Resize(dShearMod_h1,	dShearMod_h2.Length)
		Array.Resize(dShearVelo_h1,	dShearVelo_h2.Length)
		Array.Resize(dXStrgMod_h1,	dXStrgMod_h2.Length)
		Array.Resize(dXElasMod_h1,	dXElasMod_h2.Length)
		Array.Resize(dXLossTan_h1,	dXLossTan_h2.Length)

		Array.Copy(iMsec_h2,		iMsec_h1,		iMsec_h2.Length)
		Array.Copy(iTempBuf_h2,		iTempBuf_h1,	iTempBuf_h2.Length)
		Array.Copy(iMvBuf_h2,		iMvBuf_h1,		iMvBuf_h2.Length)
		Array.Copy(iTempBuf2_h2,	iTempBuf2_h1,	iTempBuf2_h2.Length)
		Array.Copy(iMvBuf2_h2,		iMvBuf2_h1,		iMvBuf2_h2.Length)
		Array.Copy(iHosei_h2,		iHosei_h1,		iHosei_h2.Length)
		Array.Copy(dHoseiP_h2,		dHoseiP_h1,		dHoseiP_h2.Length)
		Array.Copy(dViscosity_h2,	dViscosity_h1,	dViscosity_h2.Length)
		Array.Copy(dXPosPhase_h2,	dXPosPhase_h1,	dXPosPhase_h2.Length)
		Array.Copy(dYPosPhase_h2,	dYPosPhase_h1,	dYPosPhase_h2.Length)
		Array.Copy(dSplStrain_h2,	dSplStrain_h1,	dSplStrain_h2.Length)
		Array.Copy(dShearMod_h2,	dShearMod_h1,	dShearMod_h2.Length)
		Array.Copy(dShearVelo_h2,	dShearVelo_h1,	dShearVelo_h2.Length)
		Array.Copy(dXStrgMod_h2,	dXStrgMod_h1,	dXStrgMod_h2.Length)
		Array.Copy(dXElasMod_h2,	dXElasMod_h1,	dXElasMod_h2.Length)
		Array.Copy(dXLossTan_h2,	dXLossTan_h1,	dXLossTan_h2.Length)
	Case Else
		If IsNothing(iOxBuf_h2)
			h1Clear()
			p_head_h(0) = Nothing
			Exit Sub
		End If
		Array.Resize(iOxBuf_h1,		iOxBuf_h2.Length)
		Array.Resize(iOyBuf_h1,		iOyBuf_h2.Length)
		Array.Resize(iOzBuf_h1,		iOzBuf_h2.Length)
		Array.Resize(dOzBuf_h1,		dOzBuf_h2.Length)
		Array.Resize(iPxBuf_h1,		iPxBuf_h2.Length)
		Array.Resize(iPyBuf_h1,		iPyBuf_h2.Length)
		Array.Resize(iPzBuf_h1,		iPzBuf_h2.Length)
		Array.Resize(iTempBuf_h1,	iTempBuf_h2.Length)
		Array.Resize(iMvBuf_h1,		iMvBuf_h2.Length)
		Array.Resize(iTempBuf2_h1,	iTempBuf2_h2.Length)
		Array.Resize(iMvBuf2_h1,	iMvBuf2_h2.Length)
		Array.Resize(iHosei_h1,		iHosei_h2.Length)
		Array.Resize(iSlope_h1,		iSlope_h2.Length)
		Array.Resize(dHoseiP_h1,	dHoseiP_h2.Length)
		Array.Resize(dViscosity_h1,	dViscosity_h2.Length)
		Array.Resize(dXPosPhase_h1,	dXPosPhase_h2.Length)
		Array.Resize(dYPosPhase_h1,	dYPosPhase_h2.Length)
		Array.Resize(dSplStrain_h1,	dSplStrain_h2.Length)
		Array.Resize(dShearMod_h1,	dShearMod_h2.Length)
		Array.Resize(dShearVelo_h1,	dShearVelo_h2.Length)
		Array.Resize(dXStrgMod_h1,	dXStrgMod_h2.Length)
		Array.Resize(dXElasMod_h1,	dXElasMod_h2.Length)
		Array.Resize(dXLossTan_h1,	dXLossTan_h2.Length)

		Array.Copy(iOxBuf_h2,		iOxBuf_h1,		iOxBuf_h2.Length)
		Array.Copy(iOyBuf_h2,		iOyBuf_h1,		iOyBuf_h2.Length)
		Array.Copy(iOzBuf_h2,		iOzBuf_h1,		iOzBuf_h2.Length)
		Array.Copy(dOzBuf_h2,		dOzBuf_h1,		dOzBuf_h2.Length)
		Array.Copy(iPxBuf_h2,		iPxBuf_h1,		iPxBuf_h2.Length)
		Array.Copy(iPyBuf_h2,		iPyBuf_h1,		iPyBuf_h2.Length)
		Array.Copy(iPzBuf_h2,		iPzBuf_h1,		iPzBuf_h2.Length)
		Array.Copy(iTempBuf_h2,		iTempBuf_h1,	iTempBuf_h2.Length)
		Array.Copy(iMvBuf_h2,		iMvBuf_h1,		iMvBuf_h2.Length)
		Array.Copy(iTempBuf2_h2,	iTempBuf2_h1,	iTempBuf2_h2.Length)
		Array.Copy(iMvBuf2_h2,		iMvBuf2_h1,		iMvBuf2_h2.Length)
		Array.Copy(iHosei_h2,		iHosei_h1,		iHosei_h2.Length)
		Array.Copy(iSlope_h2,		iSlope_h1,		iSlope_h2.Length)
		Array.Copy(dHoseiP_h2,		dHoseiP_h1,		dHoseiP_h2.Length)
		Array.Copy(dViscosity_h2,	dViscosity_h1,	dViscosity_h2.Length)
		Array.Copy(dXPosPhase_h2,	dXPosPhase_h1,	dXPosPhase_h2.Length)
		Array.Copy(dYPosPhase_h2,	dYPosPhase_h1,	dYPosPhase_h2.Length)
		Array.Copy(dSplStrain_h2,	dSplStrain_h1,	dSplStrain_h2.Length)
		Array.Copy(dShearMod_h2,	dShearMod_h1,	dShearMod_h2.Length)
		Array.Copy(dShearVelo_h2,	dShearVelo_h1,	dShearVelo_h2.Length)
		Array.Copy(dXStrgMod_h2,	dXStrgMod_h1,	dXStrgMod_h2.Length)
		Array.Copy(dXElasMod_h2,	dXElasMod_h1,	dXElasMod_h2.Length)
		Array.Copy(dXLossTan_h2,	dXLossTan_h1,	dXLossTan_h2.Length)
	End Select
	p_head_h(0) = p_head_h(1)
Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End sub

Public sub h1Toh0Copy(ByVal bLTMode As Boolean)
Try
	Select Case bLTMode
	Case True
		If IsNothing(iTempBuf_h1)
			Exit Sub
		End If
		Array.Resize(iMsec,			iMsec_h1.Length)
		Array.Resize(iTempBuf,		iTempBuf_h1.Length)
		Array.Resize(iMvBuf,		iMvBuf_h1.Length)
		Array.Resize(iTempBuf2,		iTempBuf2_h1.Length)
		Array.Resize(iMvBuf2,		iMvBuf2_h1.Length)
		Array.Resize(iHosei,		iHosei_h1.Length)
		Array.Resize(dHosei100,		dHoseiP_h1.Length)
		Array.Resize(dViscosity,	dViscosity_h1.Length)
		Array.Resize(dXPosPhase,	dXPosPhase_h1.Length)
		Array.Resize(dYPosPhase,	dYPosPhase_h1.Length)
		Array.Resize(dSplStrain,	dSplStrain_h1.Length)
		Array.Resize(dShearMod,		dShearMod_h1.Length)
		Array.Resize(dShearVelo,	dShearVelo_h1.Length)
		Array.Resize(dXStrgMod,		dXStrgMod_h1.Length)
		Array.Resize(dXElasMod,		dXElasMod_h1.Length)
		Array.Resize(dXLossTan,		dXLossTan_h1.Length)

		Array.Copy(iMsec_h1,		iMsec,			iMsec_h1.Length)
		Array.Copy(iTempBuf_h1,		iTempBuf,		iTempBuf_h1.Length)
		Array.Copy(iMvBuf_h1,		iMvBuf,			iMvBuf_h1.Length)
		Array.Copy(iTempBuf2_h1,	iTempBuf2,		iTempBuf2_h1.Length)
		Array.Copy(iMvBuf2_h1,		iMvBuf2,		iMvBuf2_h1.Length)
		Array.Copy(iHosei_h1,		iHosei,			iHosei_h1.Length)
		Array.Copy(dHoseiP_h1,		dHosei100,		dHoseiP_h1.Length)
		Array.Copy(dViscosity_h1,	dViscosity,		dViscosity_h1.Length)
		Array.Copy(dXPosPhase_h1,	dXPosPhase,		dXPosPhase_h1.Length)
		Array.Copy(dYPosPhase_h1,	dYPosPhase,		dYPosPhase_h1.Length)
		Array.Copy(dSplStrain_h1,	dSplStrain,		dSplStrain_h1.Length)
		Array.Copy(dShearMod_h1,	dShearMod,		dShearMod_h1.Length)
		Array.Copy(dShearVelo_h1,	dShearVelo,		dShearVelo_h1.Length)
		Array.Copy(dXStrgMod_h1,	dXStrgMod,		dXStrgMod_h1.Length)
		Array.Copy(dXElasMod_h1,	dXElasMod,		dXElasMod_h1.Length)
		Array.Copy(dXLossTan_h1,	dXLossTan,		dXLossTan_h1.Length)
	Case Else
		If IsNothing(iOxBuf_h1)
			Exit Sub
		End If
		Array.Resize(iOxBuf,		iOxBuf_h1.Length)
		Array.Resize(iOyBuf,		iOyBuf_h1.Length)
		Array.Resize(iOzBuf,		iOzBuf_h1.Length)
		Array.Resize(dOzBuf,		dOzBuf_h1.Length)
		Array.Resize(iPxBuf,		iPxBuf_h1.Length)
		Array.Resize(iPyBuf,		iPyBuf_h1.Length)
		Array.Resize(iPzBuf,		iPzBuf_h1.Length)
		Array.Resize(iTempBuf,		iTempBuf_h1.Length)
		Array.Resize(iMvBuf,		iMvBuf_h1.Length)
		Array.Resize(iTempBuf2,		iTempBuf2_h1.Length)
		Array.Resize(iMvBuf2,		iMvBuf2_h1.Length)
		Array.Resize(iHosei,		iHosei_h1.Length)
		Array.Resize(iSlope,		iSlope_h1.Length)
		Array.Resize(dHosei100,		dHoseiP_h1.Length)
		Array.Resize(dViscosity,	dViscosity_h1.Length)
		Array.Resize(dXPosPhase,	dXPosPhase_h1.Length)
		Array.Resize(dYPosPhase,	dYPosPhase_h1.Length)
		Array.Resize(dSplStrain,	dSplStrain_h1.Length)
		Array.Resize(dShearMod,		dShearMod_h1.Length)
		Array.Resize(dShearVelo,	dShearVelo_h1.Length)
		Array.Resize(dXStrgMod,		dXStrgMod_h1.Length)
		Array.Resize(dXElasMod,		dXElasMod_h1.Length)
		Array.Resize(dXLossTan,		dXLossTan_h1.Length)

		Array.Copy(iOxBuf_h1,		iOxBuf,			iOxBuf_h1.Length)
		Array.Copy(iOyBuf_h1,		iOyBuf,			iOyBuf_h1.Length)
		Array.Copy(iOzBuf_h1,		iOzBuf,			iOzBuf_h1.Length)
		Array.Copy(dOzBuf_h1,		dOzBuf,			dOzBuf_h1.Length)
		Array.Copy(iPxBuf_h1,		iPxBuf,			iPxBuf_h1.Length)
		Array.Copy(iPyBuf_h1,		iPyBuf,			iPyBuf_h1.Length)
		Array.Copy(iPzBuf_h1,		iPzBuf,			iPzBuf_h1.Length)
		Array.Copy(iTempBuf_h1,		iTempBuf,		iTempBuf_h1.Length)
		Array.Copy(iMvBuf_h1,		iMvBuf,			iMvBuf_h1.Length)
		Array.Copy(iTempBuf2_h1,	iTempBuf2,		iTempBuf2_h1.Length)
		Array.Copy(iMvBuf2_h1,		iMvBuf2,		iMvBuf2_h1.Length)
		Array.Copy(iHosei_h1,		iHosei,			iHosei_h1.Length)
		Array.Copy(iSlope_h1,		iSlope,			iSlope_h1.Length)
		Array.Copy(dHoseiP_h1,		dHosei100,		dHoseiP_h1.Length)
		Array.Copy(dViscosity_h1,	dViscosity,		dViscosity_h1.Length)
		Array.Copy(dXPosPhase_h1,	dXPosPhase,		dXPosPhase_h1.Length)
		Array.Copy(dYPosPhase_h1,	dYPosPhase,		dYPosPhase_h1.Length)
		Array.Copy(dSplStrain_h1,	dSplStrain,		dSplStrain_h1.Length)
		Array.Copy(dShearMod_h1,	dShearMod,		dShearMod_h1.Length)
		Array.Copy(dShearVelo_h1,	dShearVelo,		dShearVelo_h1.Length)
		Array.Copy(dXStrgMod_h1,	dXStrgMod,		dXStrgMod_h1.Length)
		Array.Copy(dXElasMod_h1,	dXElasMod,		dXElasMod_h1.Length)
		Array.Copy(dXLossTan_h1,	dXLossTan,		dXLossTan_h1.Length)
	End Select
	p_head = p_head_h(0)

Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End sub
Public sub h2Toh0Copy(ByVal bLTMode As Boolean)
Try
	Select Case bLTMode
	Case True
		If IsNothing(iMsec_h2)
			Exit Sub
		End If
		Array.Resize(iLT_Msec,			iMsec_h2.Length)
		Array.Resize(iLT_TempBuf,		iTempBuf_h2.Length)
		Array.Resize(iLT_MvBuf,			iMvBuf_h2.Length)
		Array.Resize(iLT_TempBuf2,		iTempBuf2_h2.Length)
		Array.Resize(iLT_MvBuf2,		iMvBuf2_h2.Length)
		Array.Resize(iLT_Hosei,			iHosei_h2.Length)
		Array.Resize(dLT_Hosei100,		dHoseiP_h2.Length)
		Array.Resize(dLT_Viscosity,		dViscosity_h2.Length)
		Array.Resize(dLT_XPosPhase,		dXPosPhase_h2.Length)
		Array.Resize(dLT_YPosPhase,		dYPosPhase_h2.Length)
		Array.Resize(dLT_SplStrain,		dSplStrain_h2.Length)
		Array.Resize(dLT_ShearMod,		dShearMod_h2.Length)
		Array.Resize(dLT_ShearVelo,		dShearVelo_h2.Length)
		Array.Resize(dLT_XStrgMod,		dXStrgMod_h2.Length)
		Array.Resize(dLT_XElasMod,		dXElasMod_h2.Length)
		Array.Resize(dLT_XLossTan,		dXLossTan_h2.Length)

		Array.Copy(iMsec_h2,		iLT_Msec,			iMsec_h2.Length)
		Array.Copy(iTempBuf_h2,		iLT_TempBuf,		iTempBuf_h2.Length)
		Array.Copy(iMvBuf_h2,		iLT_MvBuf,			iMvBuf_h2.Length)
		Array.Copy(iTempBuf2_h2,	iLT_TempBuf2,		iTempBuf2_h2.Length)
		Array.Copy(iMvBuf2_h2,		iLT_MvBuf2,			iMvBuf2_h2.Length)
		Array.Copy(iHosei_h2,		iLT_Hosei,			iHosei_h2.Length)
		Array.Copy(dHoseiP_h2,		dLT_Hosei100,		dHoseiP_h2.Length)
		Array.Copy(dViscosity_h2,	dLT_Viscosity,		dViscosity_h2.Length)
		Array.Copy(dXPosPhase_h2,	dLT_XPosPhase,		dXPosPhase_h2.Length)
		Array.Copy(dYPosPhase_h2,	dLT_YPosPhase,		dYPosPhase_h2.Length)
		Array.Copy(dSplStrain_h2,	dLT_SplStrain,		dSplStrain_h2.Length)
		Array.Copy(dShearMod_h2,	dLT_ShearMod,		dShearMod_h2.Length)
		Array.Copy(dShearVelo_h2,	dLT_ShearVelo,		dShearVelo_h2.Length)
		Array.Copy(dXStrgMod_h2,	dLT_XStrgMod,		dXStrgMod_h2.Length)
		Array.Copy(dXElasMod_h2,	dLT_XElasMod,		dXElasMod_h2.Length)
		Array.Copy(dXLossTan_h2,	dLT_XLossTan,		dXLossTan_h2.Length)
	Case Else
		If IsNothing(iOxBuf_h2)
			Exit Sub
		End If
		Array.Resize(iOxBuf,		iOxBuf_h2.Length)
		Array.Resize(iOyBuf,		iOyBuf_h2.Length)
		Array.Resize(iOzBuf,		iOzBuf_h2.Length)
		Array.Resize(dOzBuf,		dOzBuf_h2.Length)
		Array.Resize(iPxBuf,		iPxBuf_h2.Length)
		Array.Resize(iPyBuf,		iPyBuf_h2.Length)
		Array.Resize(iPzBuf,		iPzBuf_h2.Length)
		Array.Resize(iTempBuf,		iTempBuf_h2.Length)
		Array.Resize(iMvBuf,		iMvBuf_h2.Length)
		Array.Resize(iTempBuf2,		iTempBuf2_h2.Length)
		Array.Resize(iMvBuf2,		iMvBuf2_h2.Length)
		Array.Resize(iHosei,		iHosei_h2.Length)
		Array.Resize(iSlope,		iSlope_h2.Length)
		Array.Resize(dHosei100,		dHoseiP_h2.Length)
		Array.Resize(dViscosity,	dViscosity_h2.Length)
		Array.Resize(dXPosPhase,	dXPosPhase_h2.Length)
		Array.Resize(dYPosPhase,	dYPosPhase_h2.Length)
		Array.Resize(dSplStrain,	dSplStrain_h2.Length)
		Array.Resize(dShearMod,		dShearMod_h2.Length)
		Array.Resize(dShearVelo,	dShearVelo_h2.Length)
		Array.Resize(dXStrgMod,		dXStrgMod_h2.Length)
		Array.Resize(dXElasMod,		dXElasMod_h2.Length)
		Array.Resize(dXLossTan,		dXLossTan_h2.Length)

		Array.Copy(iOxBuf_h2,		iOxBuf,			iOxBuf_h2.Length)
		Array.Copy(iOyBuf_h2,		iOyBuf,			iOyBuf_h2.Length)
		Array.Copy(iOzBuf_h2,		iOzBuf,			iOzBuf_h2.Length)
		Array.Copy(dOzBuf_h2,		dOzBuf,			dOzBuf_h2.Length)
		Array.Copy(iPxBuf_h2,		iPxBuf,			iPxBuf_h2.Length)
		Array.Copy(iPyBuf_h2,		iPyBuf,			iPyBuf_h2.Length)
		Array.Copy(iPzBuf_h2,		iPzBuf,			iPzBuf_h2.Length)
		Array.Copy(iTempBuf_h2,		iTempBuf,		iTempBuf_h2.Length)
		Array.Copy(iMvBuf_h2,		iMvBuf,			iMvBuf_h2.Length)
		Array.Copy(iTempBuf2_h2,	iTempBuf2,		iTempBuf2_h2.Length)
		Array.Copy(iMvBuf2_h2,		iMvBuf2,		iMvBuf2_h2.Length)
		Array.Copy(iHosei_h2,		iHosei,			iHosei_h2.Length)
		Array.Copy(iSlope_h2,		iSlope,			iSlope_h2.Length)
		Array.Copy(dHoseiP_h2,		dHosei100,		dHoseiP_h2.Length)
		Array.Copy(dViscosity_h2,	dViscosity,		dViscosity_h2.Length)
		Array.Copy(dXPosPhase_h2,	dXPosPhase,		dXPosPhase_h2.Length)
		Array.Copy(dYPosPhase_h2,	dYPosPhase,		dYPosPhase_h2.Length)
		Array.Copy(dSplStrain_h2,	dSplStrain,		dSplStrain_h2.Length)
		Array.Copy(dShearMod_h2,	dShearMod,		dShearMod_h2.Length)
		Array.Copy(dShearVelo_h2,	dShearVelo,		dShearVelo_h2.Length)
		Array.Copy(dXStrgMod_h2,	dXStrgMod,		dXStrgMod_h2.Length)
		Array.Copy(dXElasMod_h2,	dXElasMod,		dXElasMod_h2.Length)
		Array.Copy(dXLossTan_h2,	dXLossTan,		dXLossTan_h2.Length)
	End Select
	p_head = p_head_h(1)

Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End sub
Public sub h3Toh0Copy(ByVal bLTMode As Boolean)
Try
	Select Case bLTMode
	Case True
		If IsNothing(iMsec_h3)
			Exit Sub
		End If
		Array.Resize(iLT_Msec,			iMsec_h3.Length)
		Array.Resize(iLT_TempBuf,		iTempBuf_h3.Length)
		Array.Resize(iLT_MvBuf,			iMvBuf_h3.Length)
		Array.Resize(iLT_TempBuf2,		iTempBuf2_h3.Length)
		Array.Resize(iLT_MvBuf2,		iMvBuf2_h3.Length)
		Array.Resize(iLT_Hosei,			iHosei_h3.Length)
		Array.Resize(dLT_Hosei100,		dHoseiP_h3.Length)
		Array.Resize(dLT_Viscosity,		dViscosity_h3.Length)
		Array.Resize(dLT_XPosPhase,		dXPosPhase_h3.Length)
		Array.Resize(dLT_YPosPhase,		dYPosPhase_h3.Length)
		Array.Resize(dLT_SplStrain,		dSplStrain_h3.Length)
		Array.Resize(dLT_ShearMod,		dShearMod_h3.Length)
		Array.Resize(dLT_ShearVelo,		dShearVelo_h3.Length)
		Array.Resize(dLT_XStrgMod,		dXStrgMod_h3.Length)
		Array.Resize(dLT_XElasMod,		dXElasMod_h3.Length)
		Array.Resize(dLT_XLossTan,		dXLossTan_h3.Length)

		Array.Copy(iMsec_h3,		iLT_Msec,			iMsec_h3.Length)
		Array.Copy(iTempBuf_h3,		iLT_TempBuf,		iTempBuf_h3.Length)
		Array.Copy(iMvBuf_h3,		iLT_MvBuf,			iMvBuf_h3.Length)
		Array.Copy(iTempBuf2_h3,	iLT_TempBuf2,		iTempBuf2_h3.Length)
		Array.Copy(iMvBuf2_h3,		iLT_MvBuf2,			iMvBuf2_h3.Length)
		Array.Copy(iHosei_h3,		iLT_Hosei,			iHosei_h3.Length)
		Array.Copy(dHoseiP_h3,		dLT_Hosei100,		dHoseiP_h3.Length)
		Array.Copy(dViscosity_h3,	dLT_Viscosity,		dViscosity_h3.Length)
		Array.Copy(dXPosPhase_h3,	dLT_XPosPhase,		dXPosPhase_h3.Length)
		Array.Copy(dYPosPhase_h3,	dLT_YPosPhase,		dYPosPhase_h3.Length)
		Array.Copy(dSplStrain_h3,	dLT_SplStrain,		dSplStrain_h3.Length)
		Array.Copy(dShearMod_h3,	dLT_ShearMod,		dShearMod_h3.Length)
		Array.Copy(dShearVelo_h3,	dLT_ShearVelo,		dShearVelo_h3.Length)
		Array.Copy(dXStrgMod_h3,	dLT_XStrgMod,		dXStrgMod_h3.Length)
		Array.Copy(dXElasMod_h3,	dLT_XElasMod,		dXElasMod_h3.Length)
		Array.Copy(dXLossTan_h3,	dLT_XLossTan,		dXLossTan_h3.Length)
	Case Else
		If IsNothing(iOxBuf_h3)
			Exit Sub
		End If
		Array.Resize(iOxBuf,		iOxBuf_h3.Length)
		Array.Resize(iOyBuf,		iOyBuf_h3.Length)
		Array.Resize(iOzBuf,		iOzBuf_h3.Length)
		Array.Resize(dOzBuf,		dOzBuf_h3.Length)
		Array.Resize(iPxBuf,		iPxBuf_h3.Length)
		Array.Resize(iPyBuf,		iPyBuf_h3.Length)
		Array.Resize(iPzBuf,		iPzBuf_h3.Length)
		Array.Resize(iTempBuf,		iTempBuf_h3.Length)
		Array.Resize(iMvBuf,		iMvBuf_h3.Length)
		Array.Resize(iTempBuf2,		iTempBuf2_h3.Length)
		Array.Resize(iMvBuf2,		iMvBuf2_h3.Length)
		Array.Resize(iHosei,		iHosei_h3.Length)
		Array.Resize(iSlope,		iSlope_h3.Length)
		Array.Resize(dHosei100,		dHoseiP_h3.Length)
		Array.Resize(dViscosity,	dViscosity_h3.Length)
		Array.Resize(dXPosPhase,	dXPosPhase_h3.Length)
		Array.Resize(dYPosPhase,	dYPosPhase_h3.Length)
		Array.Resize(dSplStrain,	dSplStrain_h3.Length)
		Array.Resize(dShearMod,		dShearMod_h3.Length)
		Array.Resize(dShearVelo,	dShearVelo_h3.Length)
		Array.Resize(dXStrgMod,		dXStrgMod_h3.Length)
		Array.Resize(dXElasMod,		dXElasMod_h3.Length)
		Array.Resize(dXLossTan,		dXLossTan_h3.Length)

		Array.Copy(iOxBuf_h3,		iOxBuf,			iOxBuf_h3.Length)
		Array.Copy(iOyBuf_h3,		iOyBuf,			iOyBuf_h3.Length)
		Array.Copy(iOzBuf_h3,		iOzBuf,			iOzBuf_h3.Length)
		Array.Copy(dOzBuf_h3,		dOzBuf,			dOzBuf_h3.Length)
		Array.Copy(iPxBuf_h3,		iPxBuf,			iPxBuf_h3.Length)
		Array.Copy(iPyBuf_h3,		iPyBuf,			iPyBuf_h3.Length)
		Array.Copy(iPzBuf_h3,		iPzBuf,			iPzBuf_h3.Length)
		Array.Copy(iTempBuf_h3,		iTempBuf,		iTempBuf_h3.Length)
		Array.Copy(iMvBuf_h3,		iMvBuf,			iMvBuf_h3.Length)
		Array.Copy(iTempBuf2_h3,	iTempBuf2,		iTempBuf2_h3.Length)
		Array.Copy(iMvBuf2_h3,		iMvBuf2,		iMvBuf2_h3.Length)
		Array.Copy(iHosei_h3,		iHosei,			iHosei_h3.Length)
		Array.Copy(iSlope_h3,		iSlope,			iSlope_h3.Length)
		Array.Copy(dHoseiP_h3,		dHosei100,		dHoseiP_h3.Length)
		Array.Copy(dViscosity_h3,	dViscosity,		dViscosity_h3.Length)
		Array.Copy(dXPosPhase_h3,	dXPosPhase,		dXPosPhase_h3.Length)
		Array.Copy(dYPosPhase_h3,	dYPosPhase,		dYPosPhase_h3.Length)
		Array.Copy(dSplStrain_h3,	dSplStrain,		dSplStrain_h3.Length)
		Array.Copy(dShearMod_h3,	dShearMod,		dShearMod_h3.Length)
		Array.Copy(dShearVelo_h3,	dShearVelo,		dShearVelo_h3.Length)
		Array.Copy(dXStrgMod_h3,	dXStrgMod,		dXStrgMod_h3.Length)
		Array.Copy(dXElasMod_h3,	dXElasMod,		dXElasMod_h3.Length)
		Array.Copy(dXLossTan_h3,	dXLossTan,		dXLossTan_h3.Length)
	End Select
	p_head = p_head_h(2)

Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End sub
Public sub h4Toh0Copy(ByVal bLTMode As Boolean)
Try
	Select Case bLTMode
	Case True
		If IsNothing(iMsec_h4)
			Exit Sub
		End If
		Array.Resize(iLT_Msec,			iMsec_h4.Length)
		Array.Resize(iLT_TempBuf,		iTempBuf_h4.Length)
		Array.Resize(iLT_MvBuf,			iMvBuf_h4.Length)
		Array.Resize(iLT_TempBuf2,		iTempBuf2_h4.Length)
		Array.Resize(iLT_MvBuf2,		iMvBuf2_h4.Length)
		Array.Resize(iLT_Hosei,			iHosei_h4.Length)
		Array.Resize(dLT_Hosei100,		dHoseiP_h4.Length)
		Array.Resize(dLT_Viscosity,		dViscosity_h4.Length)
		Array.Resize(dLT_XPosPhase,		dXPosPhase_h4.Length)
		Array.Resize(dLT_YPosPhase,		dYPosPhase_h4.Length)
		Array.Resize(dLT_SplStrain,		dSplStrain_h4.Length)
		Array.Resize(dLT_ShearMod,		dShearMod_h4.Length)
		Array.Resize(dLT_ShearVelo,		dShearVelo_h4.Length)
		Array.Resize(dLT_XStrgMod,		dXStrgMod_h4.Length)
		Array.Resize(dLT_XElasMod,		dXElasMod_h4.Length)
		Array.Resize(dLT_XLossTan,		dXLossTan_h4.Length)

		Array.Copy(iMsec_h4,		iLT_Msec,			iMsec_h4.Length)
		Array.Copy(iTempBuf_h4,		iLT_TempBuf,		iTempBuf_h4.Length)
		Array.Copy(iMvBuf_h4,		iLT_MvBuf,			iMvBuf_h4.Length)
		Array.Copy(iTempBuf2_h4,	iLT_TempBuf2,		iTempBuf2_h4.Length)
		Array.Copy(iMvBuf2_h4,		iLT_MvBuf2,			iMvBuf2_h4.Length)
		Array.Copy(iHosei_h4,		iLT_Hosei,			iHosei_h4.Length)
		Array.Copy(dHoseiP_h4,		dLT_Hosei100,		dHoseiP_h4.Length)
		Array.Copy(dViscosity_h4,	dLT_Viscosity,		dViscosity_h4.Length)
		Array.Copy(dXPosPhase_h4,	dLT_XPosPhase,		dXPosPhase_h4.Length)
		Array.Copy(dYPosPhase_h4,	dLT_YPosPhase,		dYPosPhase_h4.Length)
		Array.Copy(dSplStrain_h4,	dLT_SplStrain,		dSplStrain_h4.Length)
		Array.Copy(dShearMod_h4,	dLT_ShearMod,		dShearMod_h4.Length)
		Array.Copy(dShearVelo_h4,	dLT_ShearVelo,		dShearVelo_h4.Length)
		Array.Copy(dXStrgMod_h4,	dLT_XStrgMod,		dXStrgMod_h4.Length)
		Array.Copy(dXElasMod_h4,	dLT_XElasMod,		dXElasMod_h4.Length)
		Array.Copy(dXLossTan_h4,	dLT_XLossTan,		dXLossTan_h4.Length)
	Case Else
		If IsNothing(iOxBuf_h4)
			Exit Sub
		End If
		Array.Resize(iOxBuf,		iOxBuf_h4.Length)
		Array.Resize(iOyBuf,		iOyBuf_h4.Length)
		Array.Resize(iOzBuf,		iOzBuf_h4.Length)
		Array.Resize(dOzBuf,		dOzBuf_h4.Length)
		Array.Resize(iPxBuf,		iPxBuf_h4.Length)
		Array.Resize(iPyBuf,		iPyBuf_h4.Length)
		Array.Resize(iPzBuf,		iPzBuf_h4.Length)
		Array.Resize(iTempBuf,		iTempBuf_h4.Length)
		Array.Resize(iMvBuf,		iMvBuf_h4.Length)
		Array.Resize(iTempBuf2,		iTempBuf2_h4.Length)
		Array.Resize(iMvBuf2,		iMvBuf2_h4.Length)
		Array.Resize(iHosei,		iHosei_h4.Length)
		Array.Resize(iSlope,		iSlope_h4.Length)
		Array.Resize(dHosei100,		dHoseiP_h4.Length)
		Array.Resize(dViscosity,	dViscosity_h4.Length)
		Array.Resize(dXPosPhase,	dXPosPhase_h4.Length)
		Array.Resize(dYPosPhase,	dYPosPhase_h4.Length)
		Array.Resize(dSplStrain,	dSplStrain_h4.Length)
		Array.Resize(dShearMod,		dShearMod_h4.Length)
		Array.Resize(dShearVelo,	dShearVelo_h4.Length)
		Array.Resize(dXStrgMod,		dXStrgMod_h4.Length)
		Array.Resize(dXElasMod,		dXElasMod_h4.Length)
		Array.Resize(dXLossTan,		dXLossTan_h4.Length)

		Array.Copy(iOxBuf_h4,		iOxBuf,			iOxBuf_h4.Length)
		Array.Copy(iOyBuf_h4,		iOyBuf,			iOyBuf_h4.Length)
		Array.Copy(iOzBuf_h4,		iOzBuf,			iOzBuf_h4.Length)
		Array.Copy(dOzBuf_h4,		dOzBuf,			dOzBuf_h4.Length)
		Array.Copy(iPxBuf_h4,		iPxBuf,			iPxBuf_h4.Length)
		Array.Copy(iPyBuf_h4,		iPyBuf,			iPyBuf_h4.Length)
		Array.Copy(iPzBuf_h4,		iPzBuf,			iPzBuf_h4.Length)
		Array.Copy(iTempBuf_h4,		iTempBuf,		iTempBuf_h4.Length)
		Array.Copy(iMvBuf_h4,		iMvBuf,			iMvBuf_h4.Length)
		Array.Copy(iTempBuf2_h4,	iTempBuf2,		iTempBuf2_h4.Length)
		Array.Copy(iMvBuf2_h4,		iMvBuf2,		iMvBuf2_h4.Length)
		Array.Copy(iHosei_h4,		iHosei,			iHosei_h4.Length)
		Array.Copy(iSlope_h4,		iSlope,			iSlope_h4.Length)
		Array.Copy(dHoseiP_h4,		dHosei100,		dHoseiP_h4.Length)
		Array.Copy(dViscosity_h4,	dViscosity,		dViscosity_h4.Length)
		Array.Copy(dXPosPhase_h4,	dXPosPhase,		dXPosPhase_h4.Length)
		Array.Copy(dYPosPhase_h4,	dYPosPhase,		dYPosPhase_h4.Length)
		Array.Copy(dSplStrain_h4,	dSplStrain,		dSplStrain_h4.Length)
		Array.Copy(dShearMod_h4,	dShearMod,		dShearMod_h4.Length)
		Array.Copy(dShearVelo_h4,	dShearVelo,		dShearVelo_h4.Length)
		Array.Copy(dXStrgMod_h4,	dXStrgMod,		dXStrgMod_h4.Length)
		Array.Copy(dXElasMod_h4,	dXElasMod,		dXElasMod_h4.Length)
		Array.Copy(dXLossTan_h4,	dXLossTan,		dXLossTan_h4.Length)
	End Select
	p_head = p_head_h(3)

Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End sub

Public sub h5Toh0Copy(ByVal bLTMode As Boolean)
Try
	Select Case bLTMode
	Case True
		If IsNothing(iMsec_h5)
			Exit Sub
		End If
		Array.Resize(iLT_Msec,			iMsec_h5.Length)
		Array.Resize(iLT_TempBuf,		iTempBuf_h5.Length)
		Array.Resize(iLT_MvBuf,			iMvBuf_h5.Length)
		Array.Resize(iLT_TempBuf2,		iTempBuf2_h5.Length)
		Array.Resize(iLT_MvBuf2,		iMvBuf2_h5.Length)
		Array.Resize(iLT_Hosei,			iHosei_h5.Length)
		Array.Resize(dLT_Hosei100,		dHoseiP_h5.Length)
		Array.Resize(dLT_Viscosity,		dViscosity_h5.Length)
		Array.Resize(dLT_XPosPhase,		dXPosPhase_h5.Length)
		Array.Resize(dLT_YPosPhase,		dYPosPhase_h5.Length)
		Array.Resize(dLT_SplStrain,		dSplStrain_h5.Length)
		Array.Resize(dLT_ShearMod,		dShearMod_h5.Length)
		Array.Resize(dLT_ShearVelo,		dShearVelo_h5.Length)
		Array.Resize(dLT_XStrgMod,		dXStrgMod_h5.Length)
		Array.Resize(dLT_XElasMod,		dXElasMod_h5.Length)
		Array.Resize(dLT_XLossTan,		dXLossTan_h5.Length)

		Array.Copy(iMsec_h5,		iLT_Msec,			iMsec_h5.Length)
		Array.Copy(iTempBuf_h5,		iLT_TempBuf,		iTempBuf_h5.Length)
		Array.Copy(iMvBuf_h5,		iLT_MvBuf,			iMvBuf_h5.Length)
		Array.Copy(iTempBuf2_h5,	iLT_TempBuf2,		iTempBuf2_h5.Length)
		Array.Copy(iMvBuf2_h5,		iLT_MvBuf2,			iMvBuf2_h5.Length)
		Array.Copy(iHosei_h5,		iLT_Hosei,			iHosei_h5.Length)
		Array.Copy(dHoseiP_h5,		dLT_Hosei100,		dHoseiP_h5.Length)
		Array.Copy(dViscosity_h5,	dLT_Viscosity,		dViscosity_h5.Length)
		Array.Copy(dXPosPhase_h5,	dLT_XPosPhase,		dXPosPhase_h5.Length)
		Array.Copy(dYPosPhase_h5,	dLT_YPosPhase,		dYPosPhase_h5.Length)
		Array.Copy(dSplStrain_h5,	dLT_SplStrain,		dSplStrain_h5.Length)
		Array.Copy(dShearMod_h5,	dLT_ShearMod,		dShearMod_h5.Length)
		Array.Copy(dShearVelo_h5,	dLT_ShearVelo,		dShearVelo_h5.Length)
		Array.Copy(dXStrgMod_h5,	dLT_XStrgMod,		dXStrgMod_h5.Length)
		Array.Copy(dXElasMod_h5,	dLT_XElasMod,		dXElasMod_h5.Length)
		Array.Copy(dXLossTan_h5,	dLT_XLossTan,		dXLossTan_h5.Length)
	Case Else
		If IsNothing(iOxBuf_h5)
			Exit Sub
		End If
		Array.Resize(iOxBuf,		iOxBuf_h5.Length)
		Array.Resize(iOyBuf,		iOyBuf_h5.Length)
		Array.Resize(iOzBuf,		iOzBuf_h5.Length)
		Array.Resize(dOzBuf,		dOzBuf_h5.Length)
		Array.Resize(iPxBuf,		iPxBuf_h5.Length)
		Array.Resize(iPyBuf,		iPyBuf_h5.Length)
		Array.Resize(iPzBuf,		iPzBuf_h5.Length)
		Array.Resize(iTempBuf,		iTempBuf_h5.Length)
		Array.Resize(iMvBuf,		iMvBuf_h5.Length)
		Array.Resize(iTempBuf2,		iTempBuf2_h5.Length)
		Array.Resize(iMvBuf2,		iMvBuf2_h5.Length)
		Array.Resize(iHosei,		iHosei_h5.Length)
		Array.Resize(iSlope,		iSlope_h5.Length)
		Array.Resize(dHosei100,		dHoseiP_h5.Length)
		Array.Resize(dViscosity,	dViscosity_h5.Length)
		Array.Resize(dXPosPhase,	dXPosPhase_h5.Length)
		Array.Resize(dYPosPhase,	dYPosPhase_h5.Length)
		Array.Resize(dSplStrain,	dSplStrain_h5.Length)
		Array.Resize(dShearMod,		dShearMod_h5.Length)
		Array.Resize(dShearVelo,	dShearVelo_h5.Length)
		Array.Resize(dXStrgMod,		dXStrgMod_h5.Length)
		Array.Resize(dXElasMod,		dXElasMod_h5.Length)
		Array.Resize(dXLossTan,		dXLossTan_h5.Length)

		Array.Copy(iOxBuf_h5,		iOxBuf,			iOxBuf_h5.Length)
		Array.Copy(iOyBuf_h5,		iOyBuf,			iOyBuf_h5.Length)
		Array.Copy(iOzBuf_h5,		iOzBuf,			iOzBuf_h5.Length)
		Array.Copy(dOzBuf_h5,		dOzBuf,			dOzBuf_h5.Length)
		Array.Copy(iPxBuf_h5,		iPxBuf,			iPxBuf_h5.Length)
		Array.Copy(iPyBuf_h5,		iPyBuf,			iPyBuf_h5.Length)
		Array.Copy(iPzBuf_h5,		iPzBuf,			iPzBuf_h5.Length)
		Array.Copy(iTempBuf_h5,		iTempBuf,		iTempBuf_h5.Length)
		Array.Copy(iMvBuf_h5,		iMvBuf,			iMvBuf_h5.Length)
		Array.Copy(iTempBuf2_h5,	iTempBuf2,		iTempBuf2_h5.Length)
		Array.Copy(iMvBuf2_h5,		iMvBuf2,		iMvBuf2_h5.Length)
		Array.Copy(iHosei_h5,		iHosei,			iHosei_h5.Length)
		Array.Copy(iSlope_h5,		iSlope,			iSlope_h5.Length)
		Array.Copy(dHoseiP_h5,		dHosei100,		dHoseiP_h5.Length)
		Array.Copy(dViscosity_h5,	dViscosity,		dViscosity_h5.Length)
		Array.Copy(dXPosPhase_h5,	dXPosPhase,		dXPosPhase_h5.Length)
		Array.Copy(dYPosPhase_h5,	dYPosPhase,		dYPosPhase_h5.Length)
		Array.Copy(dSplStrain_h5,	dSplStrain,		dSplStrain_h5.Length)
		Array.Copy(dShearMod_h5,	dShearMod,		dShearMod_h5.Length)
		Array.Copy(dShearVelo_h5,	dShearVelo,		dShearVelo_h5.Length)
		Array.Copy(dXStrgMod_h5,	dXStrgMod,		dXStrgMod_h5.Length)
		Array.Copy(dXElasMod_h5,	dXElasMod,		dXElasMod_h5.Length)
		Array.Copy(dXLossTan_h5,	dXLossTan,		dXLossTan_h5.Length)
	End Select
	p_head = p_head_h(4)

Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End sub

Public Sub h0ToBCopy(ByVal bLTMode As Boolean)
Try
	Dim iSize As Int32 = 0
	Select Case bLTMode
	Case True
		If IsNothing(iLT_Msec)
			Exit Sub
		Else
			iSize = iLT_Msec.Length
		End If

		Array.Resize(iMsec_B,		iSize)
		Array.Resize(iHosei_B,		iSize)
		Array.Resize(dHosei100_B,	iSize)
		Array.Resize(iTempBuf_B,	iSize)
		Array.Resize(iMvBuf_B,		iSize)
		Array.Resize(iTempBuf2_B,	iSize)
		Array.Resize(iMvBuf2_B,		iSize)
		Array.Resize(dXPosPhase_B,	iSize)
		Array.Resize(dYPosPhase_B,	iSize)
		Array.Resize(dViscosity_B,	iSize)
		Array.Resize(dSplStrain_B,	iSize)
		Array.Resize(dShearMod_B,	iSize)
		Array.Resize(dShearVelo_B,	iSize)
		Array.Resize(dXStrgMod_B,	iSize)
		Array.Resize(dXElasMod_B,	iSize)
		Array.Resize(dXLossTan_B,	iSize)

		Array.Copy(iLT_Msec,			iMsec_B,		iSize)
		Array.Copy(iLT_Hosei,			iHosei_B,		iSize)
		Array.Copy(dLT_Hosei100,		dHosei100_B,	iSize)
		Array.Copy(iLT_TempBuf,			iTempBuf_B,		iSize)
		Array.Copy(iLT_MvBuf,			iMvBuf_B,		iSize)
		Array.Copy(iLT_TempBuf2,		iTempBuf2_B,	iSize)
		Array.Copy(iLT_MvBuf2,			iMvBuf2_B,		iSize)
		Array.Copy(dLT_XPosPhase,		dXPosPhase_B,	iSize)
		Array.Copy(dLT_YPosPhase,		dYPosPhase_B,	iSize)
		Array.Copy(dLT_Viscosity,		dViscosity_B,	iSize)
		Array.Copy(dLT_SplStrain,		dSplStrain_B,	iSize)
		Array.Copy(dLT_ShearMod,		dShearMod_B,	iSize)
		Array.Copy(dLT_ShearVelo,		dShearVelo_B,	iSize)
		Array.Copy(dLT_XStrgMod,		dXStrgMod_B,	iSize)
		Array.Copy(dLT_XElasMod,		dXElasMod_B,	iSize)
		Array.Copy(dLT_XLossTan,		dXLossTan_B,	iSize)
	Case Else
		If IsNothing(iOxBuf)
			Exit Sub
		Else
			iSize = iOxBuf.Length
		End If

		Array.Resize(iOxBuf_B,		iSize)
		Array.Resize(iOyBuf_B,		iSize)
		Array.Resize(iOzBuf_B,		iSize)
		Array.Resize(dOzBuf_B,		iSize)
		Array.Resize(iPxBuf_B,		iSize)
		Array.Resize(iPyBuf_B,		iSize)
		Array.Resize(iPzBuf_B,		iSize)
		Array.Resize(iHosei_B,		iSize)
		Array.Resize(dHosei100_B,	iSize)
		Array.Resize(iTempBuf_B,	iSize)
		Array.Resize(iMvBuf_B,		iSize)
		Array.Resize(iTempBuf2_B,	iSize)
		Array.Resize(iMvBuf2_B,		iSize)
		Array.Resize(iSlope_B,		iSize)
		Array.Resize(dXPosPhase_B,	iSize)
		Array.Resize(dYPosPhase_B,	iSize)
		Array.Resize(dViscosity_B,	iSize)
		Array.Resize(dSplStrain_B,	iSize)
		Array.Resize(dShearMod_B,	iSize)
		Array.Resize(dShearVelo_B,	iSize)
		Array.Resize(dXStrgMod_B,	iSize)
		Array.Resize(dXElasMod_B,	iSize)
		Array.Resize(dXLossTan_B,	iSize)

		Array.Copy(iOxBuf,			iOxBuf_B,		iSize)
		Array.Copy(iOyBuf,			iOyBuf_B,		iSize)
		Array.Copy(iOzBuf,			iOzBuf_B,		iSize)
		Array.Copy(dOzBuf,			dOzBuf_B,		iSize)
		Array.Copy(iPxBuf,			iPxBuf_B,		iSize)
		Array.Copy(iPyBuf,			iPyBuf_B,		iSize)
		Array.Copy(iPzBuf,			iPzBuf_B,		iSize)
		Array.Copy(iHosei,			iHosei_B,		iSize)
		Array.Copy(dHosei100,		dHosei100_B,	iSize)
		Array.Copy(iTempBuf,		iTempBuf_B,		iSize)
		Array.Copy(iMvBuf,			iMvBuf_B,		iSize)
		Array.Copy(iTempBuf2,		iTempBuf2_B,	iSize)
		Array.Copy(iMvBuf2,			iMvBuf2_B,		iSize)
		Array.Copy(iSlope,			iSlope_B,		iSize)
		Array.Copy(dXPosPhase,		dXPosPhase_B,	iSize)
		Array.Copy(dYPosPhase,		dYPosPhase_B,	iSize)
		Array.Copy(dViscosity,		dViscosity_B,	iSize)
		Array.Copy(dSplStrain,		dSplStrain_B,	iSize)
		Array.Copy(dShearMod,		dShearMod_B,	iSize)
		Array.Copy(dShearVelo,		dShearVelo_B,	iSize)
		Array.Copy(dXStrgMod,		dXStrgMod_B,	iSize)
		Array.Copy(dXElasMod,		dXElasMod_B,	iSize)
		Array.Copy(dXLossTan,		dXLossTan_B,	iSize)
	End Select
	p_head_B = p_head

Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub
Public Sub h5ToBCopy(ByVal bLTMode As Boolean)
Try
	Dim iSize As Int32 = 0
	Select Case bLTMode
	Case True
		If IsNothing(iMsec_h5)
			Exit Sub
		Else
			iSize = iMsec_h5.Length
		End If

		Array.Resize(iMsec_B,		iSize)
		Array.Resize(iHosei_B,		iSize)
		Array.Resize(dHosei100_B,	iSize)
		Array.Resize(iTempBuf_B,	iSize)
		Array.Resize(iMvBuf_B,		iSize)
		Array.Resize(iTempBuf2_B,	iSize)
		Array.Resize(iMvBuf2_B,		iSize)
		Array.Resize(dXPosPhase_B,	iSize)
		Array.Resize(dYPosPhase_B,	iSize)
		Array.Resize(dViscosity_B,	iSize)
		Array.Resize(dSplStrain_B,	iSize)
		Array.Resize(dShearMod_B,	iSize)
		Array.Resize(dShearVelo_B,	iSize)
		Array.Resize(dXStrgMod_B,	iSize)
		Array.Resize(dXElasMod_B,	iSize)
		Array.Resize(dXLossTan_B,	iSize)

		Array.Copy(iMsec_h5,			iMsec_B,		iSize)
		Array.Copy(iHosei_h5,			iHosei_B,		iSize)
		Array.Copy(dHoseiP_h5,			dHosei100_B,	iSize)
		Array.Copy(iTempBuf_h5,			iTempBuf_B,		iSize)
		Array.Copy(iMvBuf_h5,			iMvBuf_B,		iSize)
		Array.Copy(iTempBuf2_h5,		iTempBuf2_B,	iSize)
		Array.Copy(iMvBuf2_h5,			iMvBuf2_B,		iSize)
		Array.Copy(dXPosPhase_h5,		dXPosPhase_B,	iSize)
		Array.Copy(dYPosPhase_h5,		dYPosPhase_B,	iSize)
		Array.Copy(dViscosity_h5,		dViscosity_B,	iSize)
		Array.Copy(dSplStrain_h5,		dSplStrain_B,	iSize)
		Array.Copy(dShearMod_h5,		dShearMod_B,	iSize)
		Array.Copy(dShearVelo_h5,		dShearVelo_B,	iSize)
		Array.Copy(dXStrgMod_h5,		dXStrgMod_B,	iSize)
		Array.Copy(dXElasMod_h5,		dXElasMod_B,	iSize)
		Array.Copy(dXLossTan_h5,		dXLossTan_B,	iSize)
	Case Else
		If IsNothing(iOxBuf_h5)
			Exit Sub
		Else
			iSize = iOxBuf_h5.Length
		End If

		Array.Resize(iOxBuf_B,		iSize)
		Array.Resize(iOyBuf_B,		iSize)
		Array.Resize(iOzBuf_B,		iSize)
		Array.Resize(dOzBuf_B,		iSize)
		Array.Resize(iPxBuf_B,		iSize)
		Array.Resize(iPyBuf_B,		iSize)
		Array.Resize(iPzBuf_B,		iSize)
		Array.Resize(iHosei_B,		iSize)
		Array.Resize(dHosei100_B,	iSize)
		Array.Resize(iTempBuf_B,	iSize)
		Array.Resize(iMvBuf_B,		iSize)
		Array.Resize(iTempBuf2_B,	iSize)
		Array.Resize(iMvBuf2_B,		iSize)
		Array.Resize(iSlope_B,		iSize)
		Array.Resize(dXPosPhase_B,	iSize)
		Array.Resize(dYPosPhase_B,	iSize)
		Array.Resize(dViscosity_B,	iSize)
		Array.Resize(dSplStrain_B,	iSize)
		Array.Resize(dShearMod_B,	iSize)
		Array.Resize(dShearVelo_B,	iSize)
		Array.Resize(dXStrgMod_B,	iSize)
		Array.Resize(dXElasMod_B,	iSize)
		Array.Resize(dXLossTan_B,	iSize)

		Array.Copy(iOxBuf_h5,			iOxBuf_B,		iSize)
		Array.Copy(iOyBuf_h5,			iOyBuf_B,		iSize)
		Array.Copy(iOzBuf_h5,			iOzBuf_B,		iSize)
		Array.Copy(dOzBuf_h5,			dOzBuf_B,		iSize)
		Array.Copy(iPxBuf_h5,			iPxBuf_B,		iSize)
		Array.Copy(iPyBuf_h5,			iPyBuf_B,		iSize)
		Array.Copy(iPzBuf_h5,			iPzBuf_B,		iSize)
		Array.Copy(iHosei_h5,			iHosei_B,		iSize)
		Array.Copy(dHoseiP_h5,			dHosei100_B,	iSize)
		Array.Copy(iTempBuf_h5,			iTempBuf_B,		iSize)
		Array.Copy(iMvBuf_h5,			iMvBuf_B,		iSize)
		Array.Copy(iTempBuf2_h5,		iTempBuf2_B,	iSize)
		Array.Copy(iMvBuf2_h5,			iMvBuf2_B,		iSize)
		Array.Copy(iSlope_h5,			iSlope_B,		iSize)
		Array.Copy(dXPosPhase_h5,		dXPosPhase_B,	iSize)
		Array.Copy(dYPosPhase_h5,		dYPosPhase_B,	iSize)
		Array.Copy(dViscosity_h5,		dViscosity_B,	iSize)
		Array.Copy(dSplStrain_h5,		dSplStrain_B,	iSize)
		Array.Copy(dShearMod_h5,		dShearMod_B,	iSize)
		Array.Copy(dShearVelo_h5,		dShearVelo_B,	iSize)
		Array.Copy(dXStrgMod_h5,		dXStrgMod_B,	iSize)
		Array.Copy(dXElasMod_h5,		dXElasMod_B,	iSize)
		Array.Copy(dXLossTan_h5,		dXLossTan_B,	iSize)
	End Select
	p_head_B = p_head_h(4)

Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub
Public Sub h4ToBCopy(ByVal bLTMode As Boolean)
Try
	Dim iSize As Int32 = 0
	Select Case bLTMode
	Case True
		If IsNothing(iMsec_h4)
			Exit Sub
		Else
			iSize = iMsec_h4.Length
		End If

		Array.Resize(iMsec_B,		iSize)
		Array.Resize(iHosei_B,		iSize)
		Array.Resize(dHosei100_B,	iSize)
		Array.Resize(iTempBuf_B,	iSize)
		Array.Resize(iMvBuf_B,		iSize)
		Array.Resize(iTempBuf2_B,	iSize)
		Array.Resize(iMvBuf2_B,		iSize)
		Array.Resize(dXPosPhase_B,	iSize)
		Array.Resize(dYPosPhase_B,	iSize)
		Array.Resize(dViscosity_B,	iSize)
		Array.Resize(dSplStrain_B,	iSize)
		Array.Resize(dShearMod_B,	iSize)
		Array.Resize(dShearVelo_B,	iSize)
		Array.Resize(dXStrgMod_B,	iSize)
		Array.Resize(dXElasMod_B,	iSize)
		Array.Resize(dXLossTan_B,	iSize)

		Array.Copy(iMsec_h4,			iMsec_B,		iSize)
		Array.Copy(iHosei_h4,			iHosei_B,		iSize)
		Array.Copy(dHoseiP_h4,			dHosei100_B,	iSize)
		Array.Copy(iTempBuf_h4,			iTempBuf_B,		iSize)
		Array.Copy(iMvBuf_h4,			iMvBuf_B,		iSize)
		Array.Copy(iTempBuf2_h4,		iTempBuf2_B,	iSize)
		Array.Copy(iMvBuf2_h4,			iMvBuf2_B,		iSize)
		Array.Copy(dXPosPhase_h4,		dXPosPhase_B,	iSize)
		Array.Copy(dYPosPhase_h4,		dYPosPhase_B,	iSize)
		Array.Copy(dViscosity_h4,		dViscosity_B,	iSize)
		Array.Copy(dSplStrain_h4,		dSplStrain_B,	iSize)
		Array.Copy(dShearMod_h4,		dShearMod_B,	iSize)
		Array.Copy(dShearVelo_h4,		dShearVelo_B,	iSize)
		Array.Copy(dXStrgMod_h4,		dXStrgMod_B,	iSize)
		Array.Copy(dXElasMod_h4,		dXElasMod_B,	iSize)
		Array.Copy(dXLossTan_h4,		dXLossTan_B,	iSize)
	Case Else
		If IsNothing(iOxBuf_h4)
			Exit Sub
		Else
			iSize = iOxBuf_h4.Length
		End If

		Array.Resize(iOxBuf_B,		iSize)
		Array.Resize(iOyBuf_B,		iSize)
		Array.Resize(iOzBuf_B,		iSize)
		Array.Resize(dOzBuf_B,		iSize)
		Array.Resize(iPxBuf_B,		iSize)
		Array.Resize(iPyBuf_B,		iSize)
		Array.Resize(iPzBuf_B,		iSize)
		Array.Resize(iHosei_B,		iSize)
		Array.Resize(dHosei100_B,	iSize)
		Array.Resize(iTempBuf_B,	iSize)
		Array.Resize(iMvBuf_B,		iSize)
		Array.Resize(iTempBuf2_B,	iSize)
		Array.Resize(iMvBuf2_B,		iSize)
		Array.Resize(iSlope_B,		iSize)
		Array.Resize(dXPosPhase_B,	iSize)
		Array.Resize(dYPosPhase_B,	iSize)
		Array.Resize(dViscosity_B,	iSize)
		Array.Resize(dSplStrain_B,	iSize)
		Array.Resize(dShearMod_B,	iSize)
		Array.Resize(dShearVelo_B,	iSize)
		Array.Resize(dXStrgMod_B,	iSize)
		Array.Resize(dXElasMod_B,	iSize)
		Array.Resize(dXLossTan_B,	iSize)

		Array.Copy(iOxBuf_h4,			iOxBuf_B,		iSize)
		Array.Copy(iOyBuf_h4,			iOyBuf_B,		iSize)
		Array.Copy(iOzBuf_h4,			iOzBuf_B,		iSize)
		Array.Copy(dOzBuf_h4,			dOzBuf_B,		iSize)
		Array.Copy(iPxBuf_h4,			iPxBuf_B,		iSize)
		Array.Copy(iPyBuf_h4,			iPyBuf_B,		iSize)
		Array.Copy(iPzBuf_h4,			iPzBuf_B,		iSize)
		Array.Copy(iHosei_h4,			iHosei_B,		iSize)
		Array.Copy(dHoseiP_h4,			dHosei100_B,	iSize)
		Array.Copy(iTempBuf_h4,			iTempBuf_B,		iSize)
		Array.Copy(iMvBuf_h4,			iMvBuf_B,		iSize)
		Array.Copy(iTempBuf2_h4,		iTempBuf2_B,	iSize)
		Array.Copy(iMvBuf2_h4,			iMvBuf2_B,		iSize)
		Array.Copy(iSlope_h4,			iSlope_B,		iSize)
		Array.Copy(dXPosPhase_h4,		dXPosPhase_B,	iSize)
		Array.Copy(dYPosPhase_h4,		dYPosPhase_B,	iSize)
		Array.Copy(dViscosity_h4,		dViscosity_B,	iSize)
		Array.Copy(dSplStrain_h4,		dSplStrain_B,	iSize)
		Array.Copy(dShearMod_h4,		dShearMod_B,	iSize)
		Array.Copy(dShearVelo_h4,		dShearVelo_B,	iSize)
		Array.Copy(dXStrgMod_h4,		dXStrgMod_B,	iSize)
		Array.Copy(dXElasMod_h4,		dXElasMod_B,	iSize)
		Array.Copy(dXLossTan_h4,		dXLossTan_B,	iSize)
	End Select
	p_head_B = p_head_h(3)

Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub
Public Sub h3ToBCopy(ByVal bLTMode As Boolean)
Try
	Dim iSize As Int32 = 0
	Select Case bLTMode
	Case True
		If IsNothing(iMsec_h3)
			Exit Sub
		Else
			iSize = iMsec_h3.Length
		End If

		Array.Resize(iMsec_B,		iSize)
		Array.Resize(iHosei_B,		iSize)
		Array.Resize(dHosei100_B,	iSize)
		Array.Resize(iTempBuf_B,	iSize)
		Array.Resize(iMvBuf_B,		iSize)
		Array.Resize(iTempBuf2_B,	iSize)
		Array.Resize(iMvBuf2_B,		iSize)
		Array.Resize(dXPosPhase_B,	iSize)
		Array.Resize(dYPosPhase_B,	iSize)
		Array.Resize(dViscosity_B,	iSize)
		Array.Resize(dSplStrain_B,	iSize)
		Array.Resize(dShearMod_B,	iSize)
		Array.Resize(dShearVelo_B,	iSize)
		Array.Resize(dXStrgMod_B,	iSize)
		Array.Resize(dXElasMod_B,	iSize)
		Array.Resize(dXLossTan_B,	iSize)

		Array.Copy(iMsec_h3,			iMsec_B,		iSize)
		Array.Copy(iHosei_h3,			iHosei_B,		iSize)
		Array.Copy(dHoseiP_h3,			dHosei100_B,	iSize)
		Array.Copy(iTempBuf_h3,			iTempBuf_B,		iSize)
		Array.Copy(iMvBuf_h3,			iMvBuf_B,		iSize)
		Array.Copy(iTempBuf2_h3,		iTempBuf2_B,	iSize)
		Array.Copy(iMvBuf2_h3,			iMvBuf2_B,		iSize)
		Array.Copy(dXPosPhase_h3,		dXPosPhase_B,	iSize)
		Array.Copy(dYPosPhase_h3,		dYPosPhase_B,	iSize)
		Array.Copy(dViscosity_h3,		dViscosity_B,	iSize)
		Array.Copy(dSplStrain_h3,		dSplStrain_B,	iSize)
		Array.Copy(dShearMod_h3,		dShearMod_B,	iSize)
		Array.Copy(dShearVelo_h3,		dShearVelo_B,	iSize)
		Array.Copy(dXStrgMod_h3,		dXStrgMod_B,	iSize)
		Array.Copy(dXElasMod_h3,		dXElasMod_B,	iSize)
		Array.Copy(dXLossTan_h3,		dXLossTan_B,	iSize)
	Case Else
		If IsNothing(iOxBuf_h3)
			Exit Sub
		Else
			iSize = iOxBuf_h3.Length
		End If

		Array.Resize(iOxBuf_B,		iSize)
		Array.Resize(iOyBuf_B,		iSize)
		Array.Resize(iOzBuf_B,		iSize)
		Array.Resize(dOzBuf_B,		iSize)
		Array.Resize(iPxBuf_B,		iSize)
		Array.Resize(iPyBuf_B,		iSize)
		Array.Resize(iPzBuf_B,		iSize)
		Array.Resize(iHosei_B,		iSize)
		Array.Resize(dHosei100_B,	iSize)
		Array.Resize(iTempBuf_B,	iSize)
		Array.Resize(iMvBuf_B,		iSize)
		Array.Resize(iTempBuf2_B,	iSize)
		Array.Resize(iMvBuf2_B,		iSize)
		Array.Resize(iSlope_B,		iSize)
		Array.Resize(dXPosPhase_B,	iSize)
		Array.Resize(dYPosPhase_B,	iSize)
		Array.Resize(dViscosity_B,	iSize)
		Array.Resize(dSplStrain_B,	iSize)
		Array.Resize(dShearMod_B,	iSize)
		Array.Resize(dShearVelo_B,	iSize)
		Array.Resize(dXStrgMod_B,	iSize)
		Array.Resize(dXElasMod_B,	iSize)
		Array.Resize(dXLossTan_B,	iSize)

		Array.Copy(iOxBuf_h3,			iOxBuf_B,		iSize)
		Array.Copy(iOyBuf_h3,			iOyBuf_B,		iSize)
		Array.Copy(iOzBuf_h3,			iOzBuf_B,		iSize)
		Array.Copy(dOzBuf_h3,			dOzBuf_B,		iSize)
		Array.Copy(iPxBuf_h3,			iPxBuf_B,		iSize)
		Array.Copy(iPyBuf_h3,			iPyBuf_B,		iSize)
		Array.Copy(iPzBuf_h3,			iPzBuf_B,		iSize)
		Array.Copy(iHosei_h3,			iHosei_B,		iSize)
		Array.Copy(dHoseiP_h3,			dHosei100_B,	iSize)
		Array.Copy(iTempBuf_h3,			iTempBuf_B,		iSize)
		Array.Copy(iMvBuf_h3,			iMvBuf_B,		iSize)
		Array.Copy(iTempBuf2_h3,		iTempBuf2_B,	iSize)
		Array.Copy(iMvBuf2_h3,			iMvBuf2_B,		iSize)
		Array.Copy(iSlope_h3,			iSlope_B,		iSize)
		Array.Copy(dXPosPhase_h3,		dXPosPhase_B,	iSize)
		Array.Copy(dYPosPhase_h3,		dYPosPhase_B,	iSize)
		Array.Copy(dViscosity_h3,		dViscosity_B,	iSize)
		Array.Copy(dSplStrain_h3,		dSplStrain_B,	iSize)
		Array.Copy(dShearMod_h3,		dShearMod_B,	iSize)
		Array.Copy(dShearVelo_h3,		dShearVelo_B,	iSize)
		Array.Copy(dXStrgMod_h3,		dXStrgMod_B,	iSize)
		Array.Copy(dXElasMod_h3,		dXElasMod_B,	iSize)
		Array.Copy(dXLossTan_h3,		dXLossTan_B,	iSize)
	End Select
	p_head_B = p_head_h(2)

Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub
Public Sub h2ToBCopy(ByVal bLTMode As Boolean)
Try
	Dim iSize As Int32 = 0
	Select Case bLTMode
	Case True
		If IsNothing(iMsec_h2)
			Exit Sub
		Else
			iSize = iMsec_h2.Length
		End If

		Array.Resize(iMsec_B,		iSize)
		Array.Resize(iHosei_B,		iSize)
		Array.Resize(dHosei100_B,	iSize)
		Array.Resize(iTempBuf_B,	iSize)
		Array.Resize(iMvBuf_B,		iSize)
		Array.Resize(iTempBuf2_B,	iSize)
		Array.Resize(iMvBuf2_B,		iSize)
		Array.Resize(dXPosPhase_B,	iSize)
		Array.Resize(dYPosPhase_B,	iSize)
		Array.Resize(dViscosity_B,	iSize)
		Array.Resize(dSplStrain_B,	iSize)
		Array.Resize(dShearMod_B,	iSize)
		Array.Resize(dShearVelo_B,	iSize)
		Array.Resize(dXStrgMod_B,	iSize)
		Array.Resize(dXElasMod_B,	iSize)
		Array.Resize(dXLossTan_B,	iSize)

		Array.Copy(iMsec_h2,			iMsec_B,		iSize)
		Array.Copy(iHosei_h2,			iHosei_B,		iSize)
		Array.Copy(dHoseiP_h2,			dHosei100_B,	iSize)
		Array.Copy(iTempBuf_h2,			iTempBuf_B,		iSize)
		Array.Copy(iMvBuf_h2,			iMvBuf_B,		iSize)
		Array.Copy(iTempBuf2_h2,		iTempBuf2_B,	iSize)
		Array.Copy(iMvBuf2_h2,			iMvBuf2_B,		iSize)
		Array.Copy(dXPosPhase_h2,		dXPosPhase_B,	iSize)
		Array.Copy(dYPosPhase_h2,		dYPosPhase_B,	iSize)
		Array.Copy(dViscosity_h2,		dViscosity_B,	iSize)
		Array.Copy(dSplStrain_h2,		dSplStrain_B,	iSize)
		Array.Copy(dShearMod_h2,		dShearMod_B,	iSize)
		Array.Copy(dShearVelo_h2,		dShearVelo_B,	iSize)
		Array.Copy(dXStrgMod_h2,		dXStrgMod_B,	iSize)
		Array.Copy(dXElasMod_h2,		dXElasMod_B,	iSize)
		Array.Copy(dXLossTan_h2,		dXLossTan_B,	iSize)
	Case Else
		If IsNothing(iOxBuf_h2)
			Exit Sub
		Else
			iSize = iOxBuf_h2.Length
		End If

		Array.Resize(iOxBuf_B,		iSize)
		Array.Resize(iOyBuf_B,		iSize)
		Array.Resize(iOzBuf_B,		iSize)
		Array.Resize(dOzBuf_B,		iSize)
		Array.Resize(iPxBuf_B,		iSize)
		Array.Resize(iPyBuf_B,		iSize)
		Array.Resize(iPzBuf_B,		iSize)
		Array.Resize(iHosei_B,		iSize)
		Array.Resize(dHosei100_B,	iSize)
		Array.Resize(iTempBuf_B,	iSize)
		Array.Resize(iMvBuf_B,		iSize)
		Array.Resize(iTempBuf2_B,	iSize)
		Array.Resize(iMvBuf2_B,		iSize)
		Array.Resize(iSlope_B,		iSize)
		Array.Resize(dXPosPhase_B,	iSize)
		Array.Resize(dYPosPhase_B,	iSize)
		Array.Resize(dViscosity_B,	iSize)
		Array.Resize(dSplStrain_B,	iSize)
		Array.Resize(dShearMod_B,	iSize)
		Array.Resize(dShearVelo_B,	iSize)
		Array.Resize(dXStrgMod_B,	iSize)
		Array.Resize(dXElasMod_B,	iSize)
		Array.Resize(dXLossTan_B,	iSize)

		Array.Copy(iOxBuf_h2,			iOxBuf_B,		iSize)
		Array.Copy(iOyBuf_h2,			iOyBuf_B,		iSize)
		Array.Copy(iOzBuf_h2,			iOzBuf_B,		iSize)
		Array.Copy(dOzBuf_h2,			dOzBuf_B,		iSize)
		Array.Copy(iPxBuf_h2,			iPxBuf_B,		iSize)
		Array.Copy(iPyBuf_h2,			iPyBuf_B,		iSize)
		Array.Copy(iPzBuf_h2,			iPzBuf_B,		iSize)
		Array.Copy(iHosei_h2,			iHosei_B,		iSize)
		Array.Copy(dHoseiP_h2,			dHosei100_B,	iSize)
		Array.Copy(iTempBuf_h2,			iTempBuf_B,		iSize)
		Array.Copy(iMvBuf_h2,			iMvBuf_B,		iSize)
		Array.Copy(iTempBuf2_h2,		iTempBuf2_B,	iSize)
		Array.Copy(iMvBuf2_h2,			iMvBuf2_B,		iSize)
		Array.Copy(iSlope_h2,			iSlope_B,		iSize)
		Array.Copy(dXPosPhase_h2,		dXPosPhase_B,	iSize)
		Array.Copy(dYPosPhase_h2,		dYPosPhase_B,	iSize)
		Array.Copy(dViscosity_h2,		dViscosity_B,	iSize)
		Array.Copy(dSplStrain_h2,		dSplStrain_B,	iSize)
		Array.Copy(dShearMod_h2,		dShearMod_B,	iSize)
		Array.Copy(dShearVelo_h2,		dShearVelo_B,	iSize)
		Array.Copy(dXStrgMod_h2,		dXStrgMod_B,	iSize)
		Array.Copy(dXElasMod_h2,		dXElasMod_B,	iSize)
		Array.Copy(dXLossTan_h2,		dXLossTan_B,	iSize)
	End Select
	p_head_B = p_head_h(1)

Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub
Public Sub h1ToBCopy(ByVal bLTMode As Boolean)
Try
	Dim iSize As Int32 = 0
	Select Case bLTMode
	Case True
		If IsNothing(iMsec_h1)
			Exit Sub
		Else
			iSize = iMsec_h1.Length
		End If

		Array.Resize(iMsec_B,		iSize)
		Array.Resize(iHosei_B,		iSize)
		Array.Resize(dHosei100_B,	iSize)
		Array.Resize(iTempBuf_B,	iSize)
		Array.Resize(iMvBuf_B,		iSize)
		Array.Resize(iTempBuf2_B,	iSize)
		Array.Resize(iMvBuf2_B,		iSize)
		Array.Resize(dXPosPhase_B,	iSize)
		Array.Resize(dYPosPhase_B,	iSize)
		Array.Resize(dViscosity_B,	iSize)
		Array.Resize(dSplStrain_B,	iSize)
		Array.Resize(dShearMod_B,	iSize)
		Array.Resize(dShearVelo_B,	iSize)
		Array.Resize(dXStrgMod_B,	iSize)
		Array.Resize(dXElasMod_B,	iSize)
		Array.Resize(dXLossTan_B,	iSize)

		Array.Copy(iMsec_h1,			iMsec_B,		iSize)
		Array.Copy(iHosei_h1,			iHosei_B,		iSize)
		Array.Copy(dHoseiP_h1,			dHosei100_B,	iSize)
		Array.Copy(iTempBuf_h1,			iTempBuf_B,		iSize)
		Array.Copy(iMvBuf_h1,			iMvBuf_B,		iSize)
		Array.Copy(iTempBuf2_h1,		iTempBuf2_B,	iSize)
		Array.Copy(iMvBuf2_h1,			iMvBuf2_B,		iSize)
		Array.Copy(dXPosPhase_h1,		dXPosPhase_B,	iSize)
		Array.Copy(dYPosPhase_h1,		dYPosPhase_B,	iSize)
		Array.Copy(dViscosity_h1,		dViscosity_B,	iSize)
		Array.Copy(dSplStrain_h1,		dSplStrain_B,	iSize)
		Array.Copy(dShearMod_h1,		dShearMod_B,	iSize)
		Array.Copy(dShearVelo_h1,		dShearVelo_B,	iSize)
		Array.Copy(dXStrgMod_h1,		dXStrgMod_B,	iSize)
		Array.Copy(dXElasMod_h1,		dXElasMod_B,	iSize)
		Array.Copy(dXLossTan_h1,		dXLossTan_B,	iSize)
	Case Else
		If IsNothing(iOxBuf_h1)
			Exit Sub
		Else
			iSize = iOxBuf_h1.Length
		End If

		Array.Resize(iOxBuf_B,		iSize)
		Array.Resize(iOyBuf_B,		iSize)
		Array.Resize(iOzBuf_B,		iSize)
		Array.Resize(dOzBuf_B,		iSize)
		Array.Resize(iPxBuf_B,		iSize)
		Array.Resize(iPyBuf_B,		iSize)
		Array.Resize(iPzBuf_B,		iSize)
		Array.Resize(iHosei_B,		iSize)
		Array.Resize(dHosei100_B,	iSize)
		Array.Resize(iTempBuf_B,	iSize)
		Array.Resize(iMvBuf_B,		iSize)
		Array.Resize(iTempBuf2_B,	iSize)
		Array.Resize(iMvBuf2_B,		iSize)
		Array.Resize(iSlope_B,		iSize)
		Array.Resize(dXPosPhase_B,	iSize)
		Array.Resize(dYPosPhase_B,	iSize)
		Array.Resize(dViscosity_B,	iSize)
		Array.Resize(dSplStrain_B,	iSize)
		Array.Resize(dShearMod_B,	iSize)
		Array.Resize(dShearVelo_B,	iSize)
		Array.Resize(dXStrgMod_B,	iSize)
		Array.Resize(dXElasMod_B,	iSize)
		Array.Resize(dXLossTan_B,	iSize)

		Array.Copy(iOxBuf_h1,			iOxBuf_B,		iSize)
		Array.Copy(iOyBuf_h1,			iOyBuf_B,		iSize)
		Array.Copy(iOzBuf_h1,			iOzBuf_B,		iSize)
		Array.Copy(dOzBuf_h1,			dOzBuf_B,		iSize)
		Array.Copy(iPxBuf_h1,			iPxBuf_B,		iSize)
		Array.Copy(iPyBuf_h1,			iPyBuf_B,		iSize)
		Array.Copy(iPzBuf_h1,			iPzBuf_B,		iSize)
		Array.Copy(iHosei_h1,			iHosei_B,		iSize)
		Array.Copy(dHoseiP_h1,			dHosei100_B,	iSize)
		Array.Copy(iTempBuf_h1,			iTempBuf_B,		iSize)
		Array.Copy(iMvBuf_h1,			iMvBuf_B,		iSize)
		Array.Copy(iTempBuf2_h1,		iTempBuf2_B,	iSize)
		Array.Copy(iMvBuf2_h1,			iMvBuf2_B,		iSize)
		Array.Copy(iSlope_h1,			iSlope_B,		iSize)
		Array.Copy(dXPosPhase_h1,		dXPosPhase_B,	iSize)
		Array.Copy(dYPosPhase_h1,		dYPosPhase_B,	iSize)
		Array.Copy(dViscosity_h1,		dViscosity_B,	iSize)
		Array.Copy(dSplStrain_h1,		dSplStrain_B,	iSize)
		Array.Copy(dShearMod_h1,		dShearMod_B,	iSize)
		Array.Copy(dShearVelo_h1,		dShearVelo_B,	iSize)
		Array.Copy(dXStrgMod_h1,		dXStrgMod_B,	iSize)
		Array.Copy(dXElasMod_h1,		dXElasMod_B,	iSize)
		Array.Copy(dXLossTan_h1,		dXLossTan_B,	iSize)
	End Select
	p_head_B = p_head_h(0)

Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub
Public Sub s1ToBCopy(ByVal bLTMode As Boolean)
Try
	Dim iSize As Int32 = 0
	Select Case bLTMode
	Case True
		If IsNothing(iMsec_s1)
			Exit Sub
		Else
			iSize = iMsec_s1.Length
		End If

		Array.Resize(iMsec_B,		iSize)
		Array.Resize(iHosei_B,		iSize)
		Array.Resize(dHosei100_B,	iSize)
		Array.Resize(iTempBuf_B,	iSize)
		Array.Resize(iMvBuf_B,		iSize)
		Array.Resize(iTempBuf2_B,	iSize)
		Array.Resize(iMvBuf2_B,		iSize)
		Array.Resize(dXPosPhase_B,	iSize)
		Array.Resize(dYPosPhase_B,	iSize)
		Array.Resize(dViscosity_B,	iSize)
		Array.Resize(dSplStrain_B,	iSize)
		Array.Resize(dShearMod_B,	iSize)
		Array.Resize(dShearVelo_B,	iSize)
		Array.Resize(dXStrgMod_B,	iSize)
		Array.Resize(dXElasMod_B,	iSize)
		Array.Resize(dXLossTan_B,	iSize)

		Array.Copy(iMsec_s1,			iMsec_B,		iSize)
		Array.Copy(iHosei_s1,			iHosei_B,		iSize)
		Array.Copy(dHoseiP_s1,			dHosei100_B,	iSize)
		Array.Copy(iTempBuf_s1,			iTempBuf_B,		iSize)
		Array.Copy(iMvBuf_s1,			iMvBuf_B,		iSize)
		Array.Copy(iTempBuf2_s1,		iTempBuf2_B,	iSize)
		Array.Copy(iMvBuf2_s1,			iMvBuf2_B,		iSize)
		Array.Copy(dXPosPhase_s1,		dXPosPhase_B,	iSize)
		Array.Copy(dYPosPhase_s1,		dYPosPhase_B,	iSize)
		Array.Copy(dViscosity_s1,		dViscosity_B,	iSize)
		Array.Copy(dSplStrain_s1,		dSplStrain_B,	iSize)
		Array.Copy(dShearMod_s1,		dShearMod_B,	iSize)
		Array.Copy(dShearVelo_s1,		dShearVelo_B,	iSize)
		Array.Copy(dXStrgMod_s1,		dXStrgMod_B,	iSize)
		Array.Copy(dXElasMod_s1,		dXElasMod_B,	iSize)
		Array.Copy(dXLossTan_s1,		dXLossTan_B,	iSize)
	Case Else
		If IsNothing(iOxBuf_s1)
			Exit Sub
		Else
			iSize = iOxBuf_s1.Length
		End If

		Array.Resize(iOxBuf_B,		iSize)
		Array.Resize(iOyBuf_B,		iSize)
		Array.Resize(iOzBuf_B,		iSize)
		Array.Resize(dOzBuf_B,		iSize)
		Array.Resize(iPxBuf_B,		iSize)
		Array.Resize(iPyBuf_B,		iSize)
		Array.Resize(iPzBuf_B,		iSize)
		Array.Resize(iHosei_B,		iSize)
		Array.Resize(dHosei100_B,	iSize)
		Array.Resize(iTempBuf_B,	iSize)
		Array.Resize(iMvBuf_B,		iSize)
		Array.Resize(iTempBuf2_B,	iSize)
		Array.Resize(iMvBuf2_B,		iSize)
		Array.Resize(iSlope_B,		iSize)
		Array.Resize(dXPosPhase_B,	iSize)
		Array.Resize(dYPosPhase_B,	iSize)
		Array.Resize(dViscosity_B,	iSize)
		Array.Resize(dSplStrain_B,	iSize)
		Array.Resize(dShearMod_B,	iSize)
		Array.Resize(dShearVelo_B,	iSize)
		Array.Resize(dXStrgMod_B,	iSize)
		Array.Resize(dXElasMod_B,	iSize)
		Array.Resize(dXLossTan_B,	iSize)

		Array.Copy(iOxBuf_s1,			iOxBuf_B,		iSize)
		Array.Copy(iOyBuf_s1,			iOyBuf_B,		iSize)
		Array.Copy(iOzBuf_s1,			iOzBuf_B,		iSize)
		Array.Copy(dOzBuf_s1,			dOzBuf_B,		iSize)
		Array.Copy(iPxBuf_s1,			iPxBuf_B,		iSize)
		Array.Copy(iPyBuf_s1,			iPyBuf_B,		iSize)
		Array.Copy(iPzBuf_s1,			iPzBuf_B,		iSize)
		Array.Copy(iHosei_s1,			iHosei_B,		iSize)
		Array.Copy(dHoseiP_s1,			dHosei100_B,	iSize)
		Array.Copy(iTempBuf_s1,			iTempBuf_B,		iSize)
		Array.Copy(iMvBuf_s1,			iMvBuf_B,		iSize)
		Array.Copy(iTempBuf2_s1,		iTempBuf2_B,	iSize)
		Array.Copy(iMvBuf2_s1,			iMvBuf2_B,		iSize)
		Array.Copy(iSlope_s1,			iSlope_B,		iSize)
		Array.Copy(dXPosPhase_s1,		dXPosPhase_B,	iSize)
		Array.Copy(dYPosPhase_s1,		dYPosPhase_B,	iSize)
		Array.Copy(dViscosity_s1,		dViscosity_B,	iSize)
		Array.Copy(dSplStrain_s1,		dSplStrain_B,	iSize)
		Array.Copy(dShearMod_s1,		dShearMod_B,	iSize)
		Array.Copy(dShearVelo_s1,		dShearVelo_B,	iSize)
		Array.Copy(dXStrgMod_s1,		dXStrgMod_B,	iSize)
		Array.Copy(dXElasMod_s1,		dXElasMod_B,	iSize)
		Array.Copy(dXLossTan_s1,		dXLossTan_B,	iSize)
	End Select
	p_head_B = p_head_s(0)

Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub
Public Sub s2ToBCopy(ByVal bLTMode As Boolean)
Try
	Dim iSize As Int32 = 0
	Select Case bLTMode
	Case True
		If IsNothing(iMsec_s2)
			Exit Sub
		Else
			iSize = iMsec_s2.Length
		End If

		Array.Resize(iMsec_B,		iSize)
		Array.Resize(iHosei_B,		iSize)
		Array.Resize(dHosei100_B,	iSize)
		Array.Resize(iTempBuf_B,	iSize)
		Array.Resize(iMvBuf_B,		iSize)
		Array.Resize(iTempBuf2_B,	iSize)
		Array.Resize(iMvBuf2_B,		iSize)
		Array.Resize(dXPosPhase_B,	iSize)
		Array.Resize(dYPosPhase_B,	iSize)
		Array.Resize(dViscosity_B,	iSize)
		Array.Resize(dSplStrain_B,	iSize)
		Array.Resize(dShearMod_B,	iSize)
		Array.Resize(dShearVelo_B,	iSize)
		Array.Resize(dXStrgMod_B,	iSize)
		Array.Resize(dXElasMod_B,	iSize)
		Array.Resize(dXLossTan_B,	iSize)

		Array.Copy(iMsec_s2,			iMsec_B,		iSize)
		Array.Copy(iHosei_s2,			iHosei_B,		iSize)
		Array.Copy(dHoseiP_s2,			dHosei100_B,	iSize)
		Array.Copy(iTempBuf_s2,			iTempBuf_B,		iSize)
		Array.Copy(iMvBuf_s2,			iMvBuf_B,		iSize)
		Array.Copy(iTempBuf2_s2,		iTempBuf2_B,	iSize)
		Array.Copy(iMvBuf2_s2,			iMvBuf2_B,		iSize)
		Array.Copy(dXPosPhase_s2,		dXPosPhase_B,	iSize)
		Array.Copy(dYPosPhase_s2,		dYPosPhase_B,	iSize)
		Array.Copy(dViscosity_s2,		dViscosity_B,	iSize)
		Array.Copy(dSplStrain_s2,		dSplStrain_B,	iSize)
		Array.Copy(dShearMod_s2,		dShearMod_B,	iSize)
		Array.Copy(dShearVelo_s2,		dShearVelo_B,	iSize)
		Array.Copy(dXStrgMod_s2,		dXStrgMod_B,	iSize)
		Array.Copy(dXElasMod_s2,		dXElasMod_B,	iSize)
		Array.Copy(dXLossTan_s2,		dXLossTan_B,	iSize)
	Case Else
		If IsNothing(iOxBuf_s2)
			Exit Sub
		Else
			iSize = iOxBuf_s2.Length
		End If

		Array.Resize(iOxBuf_B,		iSize)
		Array.Resize(iOyBuf_B,		iSize)
		Array.Resize(iOzBuf_B,		iSize)
		Array.Resize(dOzBuf_B,		iSize)
		Array.Resize(iPxBuf_B,		iSize)
		Array.Resize(iPyBuf_B,		iSize)
		Array.Resize(iPzBuf_B,		iSize)
		Array.Resize(iHosei_B,		iSize)
		Array.Resize(dHosei100_B,	iSize)
		Array.Resize(iTempBuf_B,	iSize)
		Array.Resize(iMvBuf_B,		iSize)
		Array.Resize(iTempBuf2_B,	iSize)
		Array.Resize(iMvBuf2_B,		iSize)
		Array.Resize(iSlope_B,		iSize)
		Array.Resize(dXPosPhase_B,	iSize)
		Array.Resize(dYPosPhase_B,	iSize)
		Array.Resize(dViscosity_B,	iSize)
		Array.Resize(dSplStrain_B,	iSize)
		Array.Resize(dShearMod_B,	iSize)
		Array.Resize(dShearVelo_B,	iSize)
		Array.Resize(dXStrgMod_B,	iSize)
		Array.Resize(dXElasMod_B,	iSize)
		Array.Resize(dXLossTan_B,	iSize)

		Array.Copy(iOxBuf_s2,			iOxBuf_B,		iSize)
		Array.Copy(iOyBuf_s2,			iOyBuf_B,		iSize)
		Array.Copy(iOzBuf_s2,			iOzBuf_B,		iSize)
		Array.Copy(dOzBuf_s2,			dOzBuf_B,		iSize)
		Array.Copy(iPxBuf_s2,			iPxBuf_B,		iSize)
		Array.Copy(iPyBuf_s2,			iPyBuf_B,		iSize)
		Array.Copy(iPzBuf_s2,			iPzBuf_B,		iSize)
		Array.Copy(iHosei_s2,			iHosei_B,		iSize)
		Array.Copy(dHoseiP_s2,			dHosei100_B,	iSize)
		Array.Copy(iTempBuf_s2,			iTempBuf_B,		iSize)
		Array.Copy(iMvBuf_s2,			iMvBuf_B,		iSize)
		Array.Copy(iTempBuf2_s2,		iTempBuf2_B,	iSize)
		Array.Copy(iMvBuf2_s2,			iMvBuf2_B,		iSize)
		Array.Copy(iSlope_s2,			iSlope_B,		iSize)
		Array.Copy(dXPosPhase_s2,		dXPosPhase_B,	iSize)
		Array.Copy(dYPosPhase_s2,		dYPosPhase_B,	iSize)
		Array.Copy(dViscosity_s2,		dViscosity_B,	iSize)
		Array.Copy(dSplStrain_s2,		dSplStrain_B,	iSize)
		Array.Copy(dShearMod_s2,		dShearMod_B,	iSize)
		Array.Copy(dShearVelo_s2,		dShearVelo_B,	iSize)
		Array.Copy(dXStrgMod_s2,		dXStrgMod_B,	iSize)
		Array.Copy(dXElasMod_s2,		dXElasMod_B,	iSize)
		Array.Copy(dXLossTan_s2,		dXLossTan_B,	iSize)
	End Select
	p_head_B = p_head_s(1)

Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub



Public sub S1Toh0Copy(ByVal bLTMode As Boolean)
Try
	Select Case bLTMode
	Case True
		If IsNothing(iMsec_S1)
			Exit Sub
		End If
		Array.Resize(iLT_MSec,			iMsec_S1.Length)
		Array.Resize(iLT_TempBuf,		iTempBuf_S1.Length)
		Array.Resize(iLT_MvBuf,			iMvBuf_S1.Length)
		Array.Resize(iLT_TempBuf2,		iTempBuf2_S1.Length)
		Array.Resize(iLT_MvBuf2,		iMvBuf2_S1.Length)
		Array.Resize(iLT_Hosei,			iHosei_S1.Length)
		Array.Resize(dLT_Hosei100,		dHoseiP_S1.Length)
		Array.Resize(dLT_Viscosity,		dViscosity_S1.Length)
		Array.Resize(dLT_XPosPhase,		dXPosPhase_S1.Length)
		Array.Resize(dLT_YPosPhase,		dYPosPhase_S1.Length)
		Array.Resize(dLT_SplStrain,		dSplStrain_S1.Length)
		Array.Resize(dLT_ShearMod,		dShearMod_S1.Length)
		Array.Resize(dLT_ShearVelo,		dShearVelo_S1.Length)
		Array.Resize(dLT_XStrgMod,		dXStrgMod_S1.Length)
		Array.Resize(dLT_XElasMod,		dXElasMod_S1.Length)
		Array.Resize(dLT_XLossTan,		dXLossTan_S1.Length)

		'領域だけ確保
		Array.Resize(dLT_YStrgMod,		dXStrgMod_S1.Length)
		Array.Resize(dLT_YElasMod,		dXElasMod_S1.Length)
		Array.Resize(dLT_YLossTan,		dXLossTan_S1.Length)

		Array.Copy(iMsec_S1,		iLT_Msec,			iMsec_S1.Length)
		Array.Copy(iTempBuf_S1,		iLT_TempBuf,		iTempBuf_S1.Length)
		Array.Copy(iMvBuf_S1,		iLT_MvBuf,			iMvBuf_S1.Length)
		Array.Copy(iTempBuf2_S1,	iLT_TempBuf2,		iTempBuf2_S1.Length)
		Array.Copy(iMvBuf2_S1,		iLT_MvBuf2,			iMvBuf2_S1.Length)
		Array.Copy(iHosei_S1,		iLT_Hosei,			iHosei_S1.Length)
		Array.Copy(dHoseiP_S1,		dLT_Hosei100,		dHoseiP_S1.Length)
		Array.Copy(dViscosity_S1,	dLT_Viscosity,		dViscosity_S1.Length)
		Array.Copy(dXPosPhase_S1,	dLT_XPosPhase,		dXPosPhase_S1.Length)
		Array.Copy(dYPosPhase_S1,	dLT_YPosPhase,		dYPosPhase_S1.Length)
		Array.Copy(dSplStrain_S1,	dLT_SplStrain,		dSplStrain_S1.Length)
		Array.Copy(dShearMod_S1,	dLT_ShearMod,		dShearMod_S1.Length)
		Array.Copy(dShearVelo_S1,	dLT_ShearVelo,		dShearVelo_S1.Length)
		Array.Copy(dXStrgMod_S1,	dLT_XStrgMod,		dXStrgMod_S1.Length)
		Array.Copy(dXElasMod_S1,	dLT_XElasMod,		dXElasMod_S1.Length)
		Array.Copy(dXLossTan_S1,	dLT_XLossTan,		dXLossTan_S1.Length)
	Case Else
		If IsNothing(iOxBuf_s1)
			Exit Sub
		End If
		Array.Resize(iOxBuf,		iOxBuf_S1.Length)
		Array.Resize(iOyBuf,		iOyBuf_S1.Length)
		Array.Resize(iOzBuf,		iOzBuf_S1.Length)
		Array.Resize(dOzBuf,		dOzBuf_S1.Length)
		Array.Resize(iPxBuf,		iPxBuf_S1.Length)
		Array.Resize(iPyBuf,		iPyBuf_S1.Length)
		Array.Resize(iPzBuf,		iPzBuf_S1.Length)
		Array.Resize(iTempBuf,		iTempBuf_S1.Length)
		Array.Resize(iMvBuf,		iMvBuf_S1.Length)
		Array.Resize(iTempBuf2,		iTempBuf2_S1.Length)
		Array.Resize(iMvBuf2,		iMvBuf2_S1.Length)
		Array.Resize(iHosei,		iHosei_S1.Length)
		Array.Resize(iSlope,		iSlope_S1.Length)
		Array.Resize(dHosei100,		dHoseiP_S1.Length)
		Array.Resize(dViscosity,	dViscosity_S1.Length)
		Array.Resize(dXPosPhase,	dXPosPhase_S1.Length)
		Array.Resize(dYPosPhase,	dXPosPhase_S1.Length)
		Array.Resize(dSplStrain,	dSplStrain_S1.Length)
		Array.Resize(dShearMod,		dShearMod_S1.Length)
		Array.Resize(dShearVelo,	dShearVelo_S1.Length)
		Array.Resize(dXStrgMod,		dXStrgMod_S1.Length)
		Array.Resize(dXElasMod,		dXElasMod_S1.Length)
		Array.Resize(dXLossTan,		dXLossTan_S1.Length)

		'領域だけ確保
		Array.Resize(dYStrgMod,		dXStrgMod_S1.Length)
		Array.Resize(dYElasMod,		dXElasMod_S1.Length)
		Array.Resize(dYLossTan,		dXLossTan_S1.Length)

		Array.Copy(iOxBuf_S1,		iOxBuf,			iOxBuf_S1.Length)
		Array.Copy(iOyBuf_S1,		iOyBuf,			iOyBuf_S1.Length)
		Array.Copy(iOzBuf_S1,		iOzBuf,			iOzBuf_S1.Length)
		Array.Copy(dOzBuf_S1,		dOzBuf,			dOzBuf_S1.Length)
		Array.Copy(iPxBuf_S1,		iPxBuf,			iPxBuf_S1.Length)
		Array.Copy(iPyBuf_S1,		iPyBuf,			iPyBuf_S1.Length)
		Array.Copy(iPzBuf_S1,		iPzBuf,			iPzBuf_S1.Length)
		Array.Copy(iTempBuf_S1,		iTempBuf,		iTempBuf_S1.Length)
		Array.Copy(iMvBuf_S1,		iMvBuf,			iMvBuf_S1.Length)
		Array.Copy(iTempBuf2_S1,	iTempBuf2,		iTempBuf2_S1.Length)
		Array.Copy(iMvBuf2_S1,		iMvBuf2,		iMvBuf2_S1.Length)
		Array.Copy(iHosei_S1,		iHosei,			iHosei_S1.Length)
		Array.Copy(iSlope_S1,		iSlope,			iSlope_S1.Length)
		Array.Copy(dHoseiP_S1,		dHosei100,		dHoseiP_S1.Length)
		Array.Copy(dViscosity_S1,	dViscosity,		dViscosity_S1.Length)
		Array.Copy(dXPosPhase_S1,	dXPosPhase,		dXPosPhase_S1.Length)
		Array.Copy(dXPosPhase_S1,	dYPosPhase,		dXPosPhase_S1.Length)
		Array.Copy(dSplStrain_S1,	dSplStrain,		dSplStrain_S1.Length)
		Array.Copy(dShearMod_S1,	dShearMod,		dShearMod_S1.Length)
		Array.Copy(dShearVelo_S1,	dShearVelo,		dShearVelo_S1.Length)
		Array.Copy(dXStrgMod_S1,	dXStrgMod,		dXStrgMod_S1.Length)
		Array.Copy(dXElasMod_S1,	dXElasMod,		dXElasMod_S1.Length)
		Array.Copy(dXLossTan_S1,	dXLossTan,		dXLossTan_S1.Length)
	End Select
	p_head = p_head_s(0)

Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End sub

Public sub S2Toh0Copy(ByVal bLTMode As Boolean)
Try
	Select Case bLTMode
	Case True
		If IsNothing(iMsec_S2)
			Exit Sub
		End If
		Array.Resize(iLT_MSec,			iMsec_S2.Length)
		Array.Resize(iLT_TempBuf,		iTempBuf_S2.Length)
		Array.Resize(iLT_MvBuf,			iMvBuf_S2.Length)
		Array.Resize(iLT_TempBuf2,		iTempBuf2_S2.Length)
		Array.Resize(iLT_MvBuf2,		iMvBuf2_S2.Length)
		Array.Resize(iLT_Hosei,			iHosei_S2.Length)
		Array.Resize(dLT_Hosei100,		dHoseiP_S2.Length)
		Array.Resize(dLT_Viscosity,		dViscosity_S2.Length)
		Array.Resize(dLT_XPosPhase,		dXPosPhase_S2.Length)
		Array.Resize(dLT_YPosPhase,		dXPosPhase_S2.Length)
		Array.Resize(dLT_SplStrain,		dSplStrain_S2.Length)
		Array.Resize(dLT_ShearMod,		dShearMod_S2.Length)
		Array.Resize(dLT_ShearVelo,		dShearVelo_S2.Length)
		Array.Resize(dLT_XStrgMod,		dXStrgMod_S2.Length)
		Array.Resize(dLT_XElasMod,		dXElasMod_S2.Length)
		Array.Resize(dLT_XLossTan,		dXLossTan_S2.Length)

		'領域だけ確保
		Array.Resize(dLT_YStrgMod,		dXStrgMod_S2.Length)
		Array.Resize(dLT_YElasMod,		dXElasMod_S2.Length)
		Array.Resize(dLT_YLossTan,		dXLossTan_S2.Length)

		Array.Copy(iMsec_S2,		iLT_Msec,			iMsec_S2.Length)
		Array.Copy(iTempBuf_S2,		iLT_TempBuf,		iTempBuf_S2.Length)
		Array.Copy(iMvBuf_S2,		iLT_MvBuf,			iMvBuf_S2.Length)
		Array.Copy(iTempBuf2_S2,	iLT_TempBuf2,		iTempBuf2_S2.Length)
		Array.Copy(iMvBuf2_S2,		iLT_MvBuf2,			iMvBuf2_S2.Length)
		Array.Copy(iHosei_S2,		iLT_Hosei,			iHosei_S2.Length)
		Array.Copy(dHoseiP_S2,		dLT_Hosei100,		dHoseiP_S2.Length)
		Array.Copy(dViscosity_S2,	dLT_Viscosity,		dViscosity_S2.Length)
		Array.Copy(dXPosPhase_S2,	dLT_XPosPhase,		dXPosPhase_S2.Length)
		Array.Copy(dXPosPhase_S2,	dLT_YPosPhase,		dXPosPhase_S2.Length)
		Array.Copy(dSplStrain_S2,	dLT_SplStrain,		dSplStrain_S2.Length)
		Array.Copy(dShearMod_S2,	dLT_ShearMod,		dShearMod_S2.Length)
		Array.Copy(dShearVelo_S2,	dLT_ShearVelo,		dShearVelo_S2.Length)
		Array.Copy(dXStrgMod_S2,	dLT_XStrgMod,		dXStrgMod_S2.Length)
		Array.Copy(dXElasMod_S2,	dLT_XElasMod,		dXElasMod_S2.Length)
		Array.Copy(dXLossTan_S2,	dLT_XLossTan,		dXLossTan_S2.Length)
	Case Else
		If IsNothing(iOxBuf_S2)
			Exit Sub
		End If
		Array.Resize(iOxBuf,		iOxBuf_S2.Length)
		Array.Resize(iOyBuf,		iOyBuf_S2.Length)
		Array.Resize(iOzBuf,		iOzBuf_S2.Length)
		Array.Resize(dOzBuf,		dOzBuf_S2.Length)
		Array.Resize(iPxBuf,		iPxBuf_S2.Length)
		Array.Resize(iPyBuf,		iPyBuf_S2.Length)
		Array.Resize(iPzBuf,		iPzBuf_S2.Length)
		Array.Resize(iTempBuf,		iTempBuf_S2.Length)
		Array.Resize(iMvBuf,		iMvBuf_S2.Length)
		Array.Resize(iTempBuf2,		iTempBuf2_S2.Length)
		Array.Resize(iMvBuf2,		iMvBuf2_S2.Length)
		Array.Resize(iHosei,		iHosei_S2.Length)
		Array.Resize(iSlope,		iSlope_S2.Length)
		Array.Resize(dHosei100,		dHoseiP_S2.Length)
		Array.Resize(dViscosity,	dViscosity_S2.Length)
		Array.Resize(dXPosPhase,	dXPosPhase_S2.Length)
		Array.Resize(dSplStrain,	dSplStrain_S2.Length)
		Array.Resize(dShearMod,		dShearMod_S2.Length)
		Array.Resize(dShearVelo,	dShearVelo_S2.Length)
		Array.Resize(dXStrgMod,		dXStrgMod_S2.Length)
		Array.Resize(dXElasMod,		dXElasMod_S2.Length)
		Array.Resize(dXLossTan,		dXLossTan_S2.Length)

		'領域だけ確保
		Array.Resize(dYPosPhase,	dXPosPhase_S2.Length)
		Array.Resize(dYStrgMod,		dXStrgMod_S2.Length)
		Array.Resize(dYElasMod,		dXElasMod_S2.Length)
		Array.Resize(dYLossTan,		dXLossTan_S2.Length)


		Array.Copy(iOxBuf_S2,		iOxBuf,			iOxBuf_S2.Length)
		Array.Copy(iOyBuf_S2,		iOyBuf,			iOyBuf_S2.Length)
		Array.Copy(iOzBuf_S2,		iOzBuf,			iOzBuf_S2.Length)
		Array.Copy(dOzBuf_S2,		dOzBuf,			dOzBuf_S2.Length)
		Array.Copy(iPxBuf_S2,		iPxBuf,			iPxBuf_S2.Length)
		Array.Copy(iPyBuf_S2,		iPyBuf,			iPyBuf_S2.Length)
		Array.Copy(iPzBuf_S2,		iPzBuf,			iPzBuf_S2.Length)
		Array.Copy(iTempBuf_S2,		iTempBuf,		iTempBuf_S2.Length)
		Array.Copy(iMvBuf_S2,		iMvBuf,			iMvBuf_S2.Length)
		Array.Copy(iTempBuf2_S2,	iTempBuf2,		iTempBuf2_S2.Length)
		Array.Copy(iMvBuf2_S2,		iMvBuf2,		iMvBuf2_S2.Length)
		Array.Copy(iHosei_S2,		iHosei,			iHosei_S2.Length)
		Array.Copy(iSlope_S2,		iSlope,			iSlope_S2.Length)
		Array.Copy(dHoseiP_S2,		dHosei100,		dHoseiP_S2.Length)
		Array.Copy(dViscosity_S2,	dViscosity,		dViscosity_S2.Length)
		Array.Copy(dXPosPhase_S2,	dXPosPhase,		dXPosPhase_S2.Length)
		'Array.Copy(dYPosPhase_S2,	dYPosPhase,		dYPosPhase_S2.Length)
		Array.Copy(dSplStrain_S2,	dSplStrain,		dSplStrain_S2.Length)
		Array.Copy(dShearMod_S2,	dShearMod,		dShearMod_S2.Length)
		Array.Copy(dShearVelo_S2,	dShearVelo,		dShearVelo_S2.Length)
		Array.Copy(dXStrgMod_S2,	dXStrgMod,		dXStrgMod_S2.Length)
		Array.Copy(dXElasMod_S2,	dXElasMod,		dXElasMod_S2.Length)
		Array.Copy(dXLossTan_S2,	dXLossTan,		dXLossTan_S2.Length)
	End Select
	p_head = p_head_s(0)

Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End sub



Public Sub BToh0Paste(ByVal bLTMode As Boolean)
Try
	Dim iSize As Int32 = 0
	Select Case bLTMode
	Case True
		If IsNothing(iMSec_B)
			Exit Sub
		Else
			iSize = iMSec_B.Length
		End If

		Array.Resize(iLT_Msec,			iSize)
		Array.Resize(iLT_Hosei,			iSize)
		Array.Resize(dLT_Hosei100,		iSize)
		Array.Resize(iLT_TempBuf,		iSize)
		Array.Resize(iLT_MvBuf,			iSize)
		Array.Resize(iLT_TempBuf2,		iSize)
		Array.Resize(iLT_MvBuf2,		iSize)
		Array.Resize(dLT_XPosPhase,		iSize)
		Array.Resize(dLT_YPosPhase,		iSize)
		Array.Resize(dLT_Viscosity,		iSize)
		Array.Resize(dLT_SplStrain,		iSize)
		Array.Resize(dLT_ShearMod,		iSize)
		Array.Resize(dLT_ShearVelo,		iSize)
		Array.Resize(dLT_XStrgMod,		iSize)
		Array.Resize(dLT_XElasMod,		iSize)
		Array.Resize(dLT_XLossTan,		iSize)

		Array.Copy(iMSec_B,				iLT_MSec,		iSize)
		Array.Copy(iHosei_B,			iLT_Hosei,		iSize)
		Array.Copy(dHosei100_B,			dLT_Hosei100,	iSize)
		Array.Copy(iTempBuf_B,			iLT_TempBuf,	iSize)
		Array.Copy(iMvBuf_B,			iLT_MvBuf,		iSize)
		Array.Copy(iTempBuf2_B,			iLT_TempBuf2,	iSize)
		Array.Copy(iMvBuf2_B,			iLT_MvBuf2,		iSize)
		Array.Copy(dXPosPhase_B,		dLT_XPosPhase,	iSize)
		Array.Copy(dYPosPhase_B,		dLT_YPosPhase,	iSize)
		Array.Copy(dViscosity_B,		dLT_Viscosity,	iSize)
		Array.Copy(dSplStrain_B,		dLT_SplStrain,	iSize)
		Array.Copy(dShearMod_B,			dLT_ShearMod,	iSize)
		Array.Copy(dShearVelo_B,		dLT_ShearVelo,	iSize)
		Array.Copy(dXStrgMod_B,			dLT_XStrgMod,	iSize)
		Array.Copy(dXElasMod_B,			dLT_XElasMod,	iSize)
		Array.Copy(dXLossTan_B,			dLT_XLossTan,	iSize)
	Case Else
		If IsNothing(iOxBuf_B)
			Exit Sub
		Else
			iSize = iOxBuf_B.Length
		End If

		Array.Resize(iOxBuf,		iSize)
		Array.Resize(iOyBuf,		iSize)
		Array.Resize(iOzBuf,		iSize)
		Array.Resize(dOzBuf,		iSize)
		Array.Resize(iPxBuf,		iSize)
		Array.Resize(iPyBuf,		iSize)
		Array.Resize(iPzBuf,		iSize)
		Array.Resize(iHosei,		iSize)
		Array.Resize(dHosei100,		iSize)
		Array.Resize(iTempBuf,		iSize)
		Array.Resize(iMvBuf,		iSize)
		Array.Resize(iTempBuf2,		iSize)
		Array.Resize(iMvBuf2,		iSize)
		Array.Resize(iSlope,		iSize)
		Array.Resize(dXPosPhase,	iSize)
		Array.Resize(dYPosPhase,	iSize)
		Array.Resize(dViscosity,	iSize)
		Array.Resize(dSplStrain,	iSize)
		Array.Resize(dShearMod,		iSize)
		Array.Resize(dShearVelo,	iSize)
		Array.Resize(dXStrgMod,		iSize)
		Array.Resize(dXElasMod,		iSize)
		Array.Resize(dXLossTan,		iSize)

		Array.Copy(iOxBuf_B,			iOxBuf,		iSize)
		Array.Copy(iOyBuf_B,			iOyBuf,		iSize)
		Array.Copy(iOzBuf_B,			iOzBuf,		iSize)
		Array.Copy(dOzBuf_B,			dOzBuf,		iSize)
		Array.Copy(iPxBuf_B,			iPxBuf,		iSize)
		Array.Copy(iPyBuf_B,			iPyBuf,		iSize)
		Array.Copy(iPzBuf_B,			iPzBuf,		iSize)
		Array.Copy(iHosei_B,			iHosei,		iSize)
		Array.Copy(dHosei100_B,			dHosei100,	iSize)
		Array.Copy(iTempBuf_B,			iTempBuf,	iSize)
		Array.Copy(iMvBuf_B,			iMvBuf,		iSize)
		Array.Copy(iTempBuf2_B,			iTempBuf2,	iSize)
		Array.Copy(iMvBuf2_B,			iMvBuf2,	iSize)
		Array.Copy(iSlope_B,			iSlope,		iSize)
		Array.Copy(dXPosPhase_B,		dXPosPhase,	iSize)
		Array.Copy(dYPosPhase_B,		dYPosPhase,	iSize)
		Array.Copy(dViscosity_B,		dViscosity,	iSize)
		Array.Copy(dSplStrain_B,		dSplStrain,	iSize)
		Array.Copy(dShearMod_B,			dShearMod,	iSize)
		Array.Copy(dShearVelo_B,		dShearVelo,	iSize)
		Array.Copy(dXStrgMod_B,			dXStrgMod,	iSize)
		Array.Copy(dXElasMod_B,			dXElasMod,	iSize)
		Array.Copy(dXLossTan_B,			dXLossTan,	iSize)
	End Select
	p_head = p_head_B

Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub
Private Sub BToh1Paste(ByVal bLTMode As Boolean)
Try
	Dim iSize As Int32 = 0
	Select Case bLTMode
	Case True
		If IsNothing(iMSec_B)
			Exit Sub
		Else
			iSize = iMSec_B.Length
		End If

		Array.Resize(iMSec_h1,		iSize)
		Array.Resize(iHosei_h1,		iSize)
		Array.Resize(dHoseiP_h1,	iSize)
		Array.Resize(iTempBuf_h1,	iSize)
		Array.Resize(iMvBuf_h1,		iSize)
		Array.Resize(iTempBuf2_h1,	iSize)
		Array.Resize(iMvBuf2_h1,	iSize)
		Array.Resize(dXPosPhase_h1,	iSize)
		Array.Resize(dYPosPhase_h1,	iSize)
		Array.Resize(dViscosity_h1,	iSize)
		Array.Resize(dSplStrain_h1,	iSize)
		Array.Resize(dShearMod_h1,	iSize)
		Array.Resize(dShearVelo_h1,	iSize)
		Array.Resize(dXStrgMod_h1,	iSize)
		Array.Resize(dXElasMod_h1,	iSize)
		Array.Resize(dXLossTan_h1,	iSize)

		Array.Copy(iMSec_B,				iMSec_h1,		iSize)
		Array.Copy(iHosei_B,			iHosei_h1,		iSize)
		Array.Copy(dHosei100_B,			dHoseiP_h1,		iSize)
		Array.Copy(iTempBuf_B,			iTempBuf_h1,	iSize)
		Array.Copy(iMvBuf_B,			iMvBuf_h1,		iSize)
		Array.Copy(iTempBuf2_B,			iTempBuf2_h1,	iSize)
		Array.Copy(iMvBuf2_B,			iMvBuf2_h1,		iSize)
		Array.Copy(dXPosPhase_B,		dXPosPhase_h1,	iSize)
		Array.Copy(dYPosPhase_B,		dYPosPhase_h1,	iSize)
		Array.Copy(dViscosity_B,		dViscosity_h1,	iSize)
		Array.Copy(dSplStrain_B,		dSplStrain_h1,	iSize)
		Array.Copy(dShearMod_B,			dShearMod_h1,	iSize)
		Array.Copy(dShearVelo_B,		dShearVelo_h1,	iSize)
		Array.Copy(dXStrgMod_B,			dXStrgMod_h1,	iSize)
		Array.Copy(dXElasMod_B,			dXElasMod_h1,	iSize)
		Array.Copy(dXLossTan_B,			dXLossTan_h1,	iSize)
	Case Else
		If IsNothing(iOxBuf_B)
			Exit Sub
		Else
			iSize = iOxBuf_B.Length
		End If

		Array.Resize(iOxBuf_h1,		iSize)
		Array.Resize(iOyBuf_h1,		iSize)
		Array.Resize(iOzBuf_h1,		iSize)
		Array.Resize(dOzBuf_h1,		iSize)
		Array.Resize(iPxBuf_h1,		iSize)
		Array.Resize(iPyBuf_h1,		iSize)
		Array.Resize(iPzBuf_h1,		iSize)
		Array.Resize(iHosei_h1,		iSize)
		Array.Resize(dHoseiP_h1,	iSize)
		Array.Resize(iTempBuf_h1,	iSize)
		Array.Resize(iMvBuf_h1,		iSize)
		Array.Resize(iTempBuf2_h1,	iSize)
		Array.Resize(iMvBuf2_h1,	iSize)
		Array.Resize(iSlope_h1,		iSize)
		Array.Resize(dXPosPhase_h1,	iSize)
		Array.Resize(dYPosPhase_h1,	iSize)
		Array.Resize(dViscosity_h1,	iSize)
		Array.Resize(dSplStrain_h1,	iSize)
		Array.Resize(dShearMod_h1,	iSize)
		Array.Resize(dShearVelo_h1,	iSize)
		Array.Resize(dXStrgMod_h1,	iSize)
		Array.Resize(dXElasMod_h1,	iSize)
		Array.Resize(dXLossTan_h1,	iSize)

		Array.Copy(iOxBuf_B,			iOxBuf_h1,		iSize)
		Array.Copy(iOyBuf_B,			iOyBuf_h1,		iSize)
		Array.Copy(iOzBuf_B,			iOzBuf_h1,		iSize)
		Array.Copy(dOzBuf_B,			dOzBuf_h1,		iSize)
		Array.Copy(iPxBuf_B,			iPxBuf_h1,		iSize)
		Array.Copy(iPyBuf_B,			iPyBuf_h1,		iSize)
		Array.Copy(iPzBuf_B,			iPzBuf_h1,		iSize)
		Array.Copy(iHosei_B,			iHosei_h1,		iSize)
		Array.Copy(dHosei100_B,			dHoseiP_h1,		iSize)
		Array.Copy(iTempBuf_B,			iTempBuf_h1,	iSize)
		Array.Copy(iMvBuf_B,			iMvBuf_h1,		iSize)
		Array.Copy(iTempBuf2_B,			iTempBuf2_h1,	iSize)
		Array.Copy(iMvBuf2_B,			iMvBuf2_h1,		iSize)
		Array.Copy(iSlope_B,			iSlope_h1,		iSize)
		Array.Copy(dXPosPhase_B,		dXPosPhase_h1,	iSize)
		Array.Copy(dYPosPhase_B,		dYPosPhase_h1,	iSize)
		Array.Copy(dViscosity_B,		dViscosity_h1,	iSize)
		Array.Copy(dSplStrain_B,		dSplStrain_h1,	iSize)
		Array.Copy(dShearMod_B,			dShearMod_h1,	iSize)
		Array.Copy(dShearVelo_B,		dShearVelo_h1,	iSize)
		Array.Copy(dXStrgMod_B,			dXStrgMod_h1,	iSize)
		Array.Copy(dXElasMod_B,			dXElasMod_h1,	iSize)
		Array.Copy(dXLossTan_B,			dXLossTan_h1,	iSize)
	End Select
	p_head_h(0) = p_head_B

Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub
Private Sub BToh2Paste(ByVal bLTMode As Boolean)
Try
	Dim iSize As Int32 = 0
	Select Case bLTMode
	Case True
		If IsNothing(iMSec_B)
			Exit Sub
		Else
			iSize = iMSec_B.Length
		End If

		Array.Resize(iMSec_h2,		iSize)
		Array.Resize(iHosei_h2,		iSize)
		Array.Resize(dHoseiP_h2,	iSize)
		Array.Resize(iTempBuf_h2,	iSize)
		Array.Resize(iMvBuf_h2,		iSize)
		Array.Resize(iTempBuf2_h2,	iSize)
		Array.Resize(iMvBuf2_h2,	iSize)
		Array.Resize(dXPosPhase_h2,	iSize)
		Array.Resize(dYPosPhase_h2,	iSize)
		Array.Resize(dViscosity_h2,	iSize)
		Array.Resize(dSplStrain_h2,	iSize)
		Array.Resize(dShearMod_h2,	iSize)
		Array.Resize(dShearVelo_h2,	iSize)
		Array.Resize(dXStrgMod_h2,	iSize)
		Array.Resize(dXElasMod_h2,	iSize)
		Array.Resize(dXLossTan_h2,	iSize)

		Array.Copy(iMSec_B,				iMSec_h2,		iSize)
		Array.Copy(iHosei_B,			iHosei_h2,		iSize)
		Array.Copy(dHosei100_B,			dHoseiP_h2,		iSize)
		Array.Copy(iTempBuf_B,			iTempBuf_h2,	iSize)
		Array.Copy(iMvBuf_B,			iMvBuf_h2,		iSize)
		Array.Copy(iTempBuf2_B,			iTempBuf2_h2,	iSize)
		Array.Copy(iMvBuf2_B,			iMvBuf2_h2,		iSize)
		Array.Copy(dXPosPhase_B,		dXPosPhase_h2,	iSize)
		Array.Copy(dYPosPhase_B,		dYPosPhase_h2,	iSize)
		Array.Copy(dViscosity_B,		dViscosity_h2,	iSize)
		Array.Copy(dSplStrain_B,		dSplStrain_h2,	iSize)
		Array.Copy(dShearMod_B,			dShearMod_h2,	iSize)
		Array.Copy(dShearVelo_B,		dShearVelo_h2,	iSize)
		Array.Copy(dXStrgMod_B,			dXStrgMod_h2,	iSize)
		Array.Copy(dXElasMod_B,			dXElasMod_h2,	iSize)
		Array.Copy(dXLossTan_B,			dXLossTan_h2,	iSize)
	Case Else
		If IsNothing(iOxBuf_B)
			Exit Sub
		Else
			iSize = iOxBuf_B.Length
		End If

		Array.Resize(iOxBuf_h2,		iSize)
		Array.Resize(iOyBuf_h2,		iSize)
		Array.Resize(iOzBuf_h2,		iSize)
		Array.Resize(dOzBuf_h2,		iSize)
		Array.Resize(iPxBuf_h2,		iSize)
		Array.Resize(iPyBuf_h2,		iSize)
		Array.Resize(iPzBuf_h2,		iSize)
		Array.Resize(iHosei_h2,		iSize)
		Array.Resize(dHoseiP_h2,	iSize)
		Array.Resize(iTempBuf_h2,	iSize)
		Array.Resize(iMvBuf_h2,		iSize)
		Array.Resize(iTempBuf2_h2,	iSize)
		Array.Resize(iMvBuf2_h2,	iSize)
		Array.Resize(iSlope_h2,		iSize)
		Array.Resize(dXPosPhase_h2,	iSize)
		Array.Resize(dYPosPhase_h2,	iSize)
		Array.Resize(dViscosity_h2,	iSize)
		Array.Resize(dSplStrain_h2,	iSize)
		Array.Resize(dShearMod_h2,	iSize)
		Array.Resize(dShearVelo_h2,	iSize)
		Array.Resize(dXStrgMod_h2,	iSize)
		Array.Resize(dXElasMod_h2,	iSize)
		Array.Resize(dXLossTan_h2,	iSize)

		Array.Copy(iOxBuf_B,			iOxBuf_h2,		iSize)
		Array.Copy(iOyBuf_B,			iOyBuf_h2,		iSize)
		Array.Copy(iOzBuf_B,			iOzBuf_h2,		iSize)
		Array.Copy(dOzBuf_B,			dOzBuf_h2,		iSize)
		Array.Copy(iPxBuf_B,			iPxBuf_h2,		iSize)
		Array.Copy(iPyBuf_B,			iPyBuf_h2,		iSize)
		Array.Copy(iPzBuf_B,			iPzBuf_h2,		iSize)
		Array.Copy(iHosei_B,			iHosei_h2,		iSize)
		Array.Copy(dHosei100_B,			dHoseiP_h2,		iSize)
		Array.Copy(iTempBuf_B,			iTempBuf_h2,	iSize)
		Array.Copy(iMvBuf_B,			iMvBuf_h2,		iSize)
		Array.Copy(iTempBuf2_B,			iTempBuf2_h2,	iSize)
		Array.Copy(iMvBuf2_B,			iMvBuf2_h2,		iSize)
		Array.Copy(iSlope_B,			iSlope_h2,		iSize)
		Array.Copy(dXPosPhase_B,		dXPosPhase_h2,	iSize)
		Array.Copy(dYPosPhase_B,		dYPosPhase_h2,	iSize)
		Array.Copy(dViscosity_B,		dViscosity_h2,	iSize)
		Array.Copy(dSplStrain_B,		dSplStrain_h2,	iSize)
		Array.Copy(dShearMod_B,			dShearMod_h2,	iSize)
		Array.Copy(dShearVelo_B,		dShearVelo_h2,	iSize)
		Array.Copy(dXStrgMod_B,			dXStrgMod_h2,	iSize)
		Array.Copy(dXElasMod_B,			dXElasMod_h2,	iSize)
		Array.Copy(dXLossTan_B,			dXLossTan_h2,	iSize)
	End Select
	p_head_h(1) = p_head_B

Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub

Private Sub BToh3Paste(ByVal bLTMode As Boolean)
Try
	Dim iSize As Int32 = 0
	Select Case bLTMode
	Case True
		If IsNothing(iMSec_B)
			Exit Sub
		Else
			iSize = iMSec_B.Length
		End If

		Array.Resize(iMSec_h3,		iSize)
		Array.Resize(iHosei_h3,		iSize)
		Array.Resize(dHoseiP_h3,	iSize)
		Array.Resize(iTempBuf_h3,	iSize)
		Array.Resize(iMvBuf_h3,		iSize)
		Array.Resize(iTempBuf2_h3,	iSize)
		Array.Resize(iMvBuf2_h3,	iSize)
		Array.Resize(dXPosPhase_h3,	iSize)
		Array.Resize(dYPosPhase_h3,	iSize)
		Array.Resize(dViscosity_h3,	iSize)
		Array.Resize(dSplStrain_h3,	iSize)
		Array.Resize(dShearMod_h3,	iSize)
		Array.Resize(dShearVelo_h3,	iSize)
		Array.Resize(dXStrgMod_h3,	iSize)
		Array.Resize(dXElasMod_h3,	iSize)
		Array.Resize(dXLossTan_h3,	iSize)

		Array.Copy(iMSec_B,				iMSec_h3,		iSize)
		Array.Copy(iHosei_B,			iHosei_h3,		iSize)
		Array.Copy(dHosei100_B,			dHoseiP_h3,		iSize)
		Array.Copy(iTempBuf_B,			iTempBuf_h3,	iSize)
		Array.Copy(iMvBuf_B,			iMvBuf_h3,		iSize)
		Array.Copy(iTempBuf2_B,			iTempBuf2_h3,	iSize)
		Array.Copy(iMvBuf2_B,			iMvBuf2_h3,		iSize)
		Array.Copy(dXPosPhase_B,		dXPosPhase_h3,	iSize)
		Array.Copy(dYPosPhase_B,		dYPosPhase_h3,	iSize)
		Array.Copy(dViscosity_B,		dViscosity_h3,	iSize)
		Array.Copy(dSplStrain_B,		dSplStrain_h3,	iSize)
		Array.Copy(dShearMod_B,			dShearMod_h3,	iSize)
		Array.Copy(dShearVelo_B,		dShearVelo_h3,	iSize)
		Array.Copy(dXStrgMod_B,			dXStrgMod_h3,	iSize)
		Array.Copy(dXElasMod_B,			dXElasMod_h3,	iSize)
		Array.Copy(dXLossTan_B,			dXLossTan_h3,	iSize)
	Case Else
		If IsNothing(iOxBuf_B)
			Exit Sub
		Else
			iSize = iOxBuf_B.Length
		End If

		Array.Resize(iOxBuf_h3,		iSize)
		Array.Resize(iOyBuf_h3,		iSize)
		Array.Resize(iOzBuf_h3,		iSize)
		Array.Resize(dOzBuf_h3,		iSize)
		Array.Resize(iPxBuf_h3,		iSize)
		Array.Resize(iPyBuf_h3,		iSize)
		Array.Resize(iPzBuf_h3,		iSize)
		Array.Resize(iHosei_h3,		iSize)
		Array.Resize(dHoseiP_h3,	iSize)
		Array.Resize(iTempBuf_h3,	iSize)
		Array.Resize(iMvBuf_h3,		iSize)
		Array.Resize(iTempBuf2_h3,	iSize)
		Array.Resize(iMvBuf2_h3,	iSize)
		Array.Resize(iSlope_h3,		iSize)
		Array.Resize(dXPosPhase_h3,	iSize)
		Array.Resize(dYPosPhase_h3,	iSize)
		Array.Resize(dViscosity_h3,	iSize)
		Array.Resize(dSplStrain_h3,	iSize)
		Array.Resize(dShearMod_h3,	iSize)
		Array.Resize(dShearVelo_h3,	iSize)
		Array.Resize(dXStrgMod_h3,	iSize)
		Array.Resize(dXElasMod_h3,	iSize)
		Array.Resize(dXLossTan_h3,	iSize)

		Array.Copy(iOxBuf_B,			iOxBuf_h3,		iSize)
		Array.Copy(iOyBuf_B,			iOyBuf_h3,		iSize)
		Array.Copy(iOzBuf_B,			iOzBuf_h3,		iSize)
		Array.Copy(dOzBuf_B,			dOzBuf_h3,		iSize)
		Array.Copy(iPxBuf_B,			iPxBuf_h3,		iSize)
		Array.Copy(iPyBuf_B,			iPyBuf_h3,		iSize)
		Array.Copy(iPzBuf_B,			iPzBuf_h3,		iSize)
		Array.Copy(iHosei_B,			iHosei_h3,		iSize)
		Array.Copy(dHosei100_B,			dHoseiP_h3,		iSize)
		Array.Copy(iTempBuf_B,			iTempBuf_h3,	iSize)
		Array.Copy(iMvBuf_B,			iMvBuf_h3,		iSize)
		Array.Copy(iTempBuf2_B,			iTempBuf2_h3,	iSize)
		Array.Copy(iMvBuf2_B,			iMvBuf2_h3,		iSize)
		Array.Copy(iSlope_B,			iSlope_h3,		iSize)
		Array.Copy(dXPosPhase_B,		dXPosPhase_h3,	iSize)
		Array.Copy(dYPosPhase_B,		dYPosPhase_h3,	iSize)
		Array.Copy(dViscosity_B,		dViscosity_h3,	iSize)
		Array.Copy(dSplStrain_B,		dSplStrain_h3,	iSize)
		Array.Copy(dShearMod_B,			dShearMod_h3,	iSize)
		Array.Copy(dShearVelo_B,		dShearVelo_h3,	iSize)
		Array.Copy(dXStrgMod_B,			dXStrgMod_h3,	iSize)
		Array.Copy(dXElasMod_B,			dXElasMod_h3,	iSize)
		Array.Copy(dXLossTan_B,			dXLossTan_h3,	iSize)
	End Select
	p_head_h(2) = p_head_B

Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub
Private Sub BToh4Paste(ByVal bLTMode As Boolean)
Try
	Dim iSize As Int32 = 0
	Select Case bLTMode
	Case True
		If IsNothing(iMSec_B)
			Exit Sub
		Else
			iSize = iMSec_B.Length
		End If

		Array.Resize(iMSec_h4,		iSize)
		Array.Resize(iHosei_h4,		iSize)
		Array.Resize(dHoseiP_h4,	iSize)
		Array.Resize(iTempBuf_h4,	iSize)
		Array.Resize(iMvBuf_h4,		iSize)
		Array.Resize(iTempBuf2_h4,	iSize)
		Array.Resize(iMvBuf2_h4,	iSize)
		Array.Resize(dXPosPhase_h4,	iSize)
		Array.Resize(dYPosPhase_h4,	iSize)
		Array.Resize(dViscosity_h4,	iSize)
		Array.Resize(dSplStrain_h4,	iSize)
		Array.Resize(dShearMod_h4,	iSize)
		Array.Resize(dShearVelo_h4,	iSize)
		Array.Resize(dXStrgMod_h4,	iSize)
		Array.Resize(dXElasMod_h4,	iSize)
		Array.Resize(dXLossTan_h4,	iSize)

		Array.Copy(iMSec_B,				iMSec_h4,		iSize)
		Array.Copy(iHosei_B,			iHosei_h4,		iSize)
		Array.Copy(dHosei100_B,			dHoseiP_h4,		iSize)
		Array.Copy(iTempBuf_B,			iTempBuf_h4,	iSize)
		Array.Copy(iMvBuf_B,			iMvBuf_h4,		iSize)
		Array.Copy(iTempBuf2_B,			iTempBuf2_h4,	iSize)
		Array.Copy(iMvBuf2_B,			iMvBuf2_h4,		iSize)
		Array.Copy(dXPosPhase_B,		dXPosPhase_h4,	iSize)
		Array.Copy(dYPosPhase_B,		dYPosPhase_h4,	iSize)
		Array.Copy(dViscosity_B,		dViscosity_h4,	iSize)
		Array.Copy(dSplStrain_B,		dSplStrain_h4,	iSize)
		Array.Copy(dShearMod_B,			dShearMod_h4,	iSize)
		Array.Copy(dShearVelo_B,		dShearVelo_h4,	iSize)
		Array.Copy(dXStrgMod_B,			dXStrgMod_h4,	iSize)
		Array.Copy(dXElasMod_B,			dXElasMod_h4,	iSize)
		Array.Copy(dXLossTan_B,			dXLossTan_h4,	iSize)
	Case Else
		If IsNothing(iOxBuf_B)
			Exit Sub
		Else
			iSize = iOxBuf_B.Length
		End If

		Array.Resize(iOxBuf_h4,		iSize)
		Array.Resize(iOyBuf_h4,		iSize)
		Array.Resize(iOzBuf_h4,		iSize)
		Array.Resize(dOzBuf_h4,		iSize)
		Array.Resize(iPxBuf_h4,		iSize)
		Array.Resize(iPyBuf_h4,		iSize)
		Array.Resize(iPzBuf_h4,		iSize)
		Array.Resize(iHosei_h4,		iSize)
		Array.Resize(dHoseiP_h4,	iSize)
		Array.Resize(iTempBuf_h4,	iSize)
		Array.Resize(iMvBuf_h4,		iSize)
		Array.Resize(iTempBuf2_h4,	iSize)
		Array.Resize(iMvBuf2_h4,	iSize)
		Array.Resize(iSlope_h4,		iSize)
		Array.Resize(dXPosPhase_h4,	iSize)
		Array.Resize(dYPosPhase_h4,	iSize)
		Array.Resize(dViscosity_h4,	iSize)
		Array.Resize(dSplStrain_h4,	iSize)
		Array.Resize(dShearMod_h4,	iSize)
		Array.Resize(dShearVelo_h4,	iSize)
		Array.Resize(dXStrgMod_h4,	iSize)
		Array.Resize(dXElasMod_h4,	iSize)
		Array.Resize(dXLossTan_h4,	iSize)

		Array.Copy(iOxBuf_B,			iOxBuf_h4,		iSize)
		Array.Copy(iOyBuf_B,			iOyBuf_h4,		iSize)
		Array.Copy(iOzBuf_B,			iOzBuf_h4,		iSize)
		Array.Copy(dOzBuf_B,			dOzBuf_h4,		iSize)
		Array.Copy(iPxBuf_B,			iPxBuf_h4,		iSize)
		Array.Copy(iPyBuf_B,			iPyBuf_h4,		iSize)
		Array.Copy(iPzBuf_B,			iPzBuf_h4,		iSize)
		Array.Copy(iHosei_B,			iHosei_h4,		iSize)
		Array.Copy(dHosei100_B,			dHoseiP_h4,		iSize)
		Array.Copy(iTempBuf_B,			iTempBuf_h4,	iSize)
		Array.Copy(iMvBuf_B,			iMvBuf_h4,		iSize)
		Array.Copy(iTempBuf2_B,			iTempBuf2_h4,	iSize)
		Array.Copy(iMvBuf2_B,			iMvBuf2_h4,		iSize)
		Array.Copy(iSlope_B,			iSlope_h4,		iSize)
		Array.Copy(dXPosPhase_B,		dXPosPhase_h4,	iSize)
		Array.Copy(dYPosPhase_B,		dYPosPhase_h4,	iSize)
		Array.Copy(dViscosity_B,		dViscosity_h4,	iSize)
		Array.Copy(dSplStrain_B,		dSplStrain_h4,	iSize)
		Array.Copy(dShearMod_B,			dShearMod_h4,	iSize)
		Array.Copy(dShearVelo_B,		dShearVelo_h4,	iSize)
		Array.Copy(dXStrgMod_B,			dXStrgMod_h4,	iSize)
		Array.Copy(dXElasMod_B,			dXElasMod_h4,	iSize)
		Array.Copy(dXLossTan_B,			dXLossTan_h4,	iSize)
	End Select
	p_head_h(3) = p_head_B

Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub
Private Sub BToh5Paste(ByVal bLTMode As Boolean)
Try
	Dim iSize As Int32 = 0
	Select Case bLTMode
	Case True
		If IsNothing(iMSec_B)
			Exit Sub
		Else
			iSize = iMSec_B.Length
		End If

		Array.Resize(iMSec_h5,		iSize)
		Array.Resize(iHosei_h5,		iSize)
		Array.Resize(dHoseiP_h5,	iSize)
		Array.Resize(iTempBuf_h5,	iSize)
		Array.Resize(iMvBuf_h5,		iSize)
		Array.Resize(iTempBuf2_h5,	iSize)
		Array.Resize(iMvBuf2_h5,	iSize)
		Array.Resize(dXPosPhase_h5,	iSize)
		Array.Resize(dYPosPhase_h5,	iSize)
		Array.Resize(dViscosity_h5,	iSize)
		Array.Resize(dSplStrain_h5,	iSize)
		Array.Resize(dShearMod_h5,	iSize)
		Array.Resize(dShearVelo_h5,	iSize)
		Array.Resize(dXStrgMod_h5,	iSize)
		Array.Resize(dXElasMod_h5,	iSize)
		Array.Resize(dXLossTan_h5,	iSize)

		Array.Copy(iMSec_B,				iMSec_h5,		iSize)
		Array.Copy(iHosei_B,			iHosei_h5,		iSize)
		Array.Copy(dHosei100_B,			dHoseiP_h5,		iSize)
		Array.Copy(iTempBuf_B,			iTempBuf_h5,	iSize)
		Array.Copy(iMvBuf_B,			iMvBuf_h5,		iSize)
		Array.Copy(iTempBuf2_B,			iTempBuf2_h5,	iSize)
		Array.Copy(iMvBuf2_B,			iMvBuf2_h5,		iSize)
		Array.Copy(dXPosPhase_B,		dXPosPhase_h5,	iSize)
		Array.Copy(dYPosPhase_B,		dYPosPhase_h5,	iSize)
		Array.Copy(dViscosity_B,		dViscosity_h5,	iSize)
		Array.Copy(dSplStrain_B,		dSplStrain_h5,	iSize)
		Array.Copy(dShearMod_B,			dShearMod_h5,	iSize)
		Array.Copy(dShearVelo_B,		dShearVelo_h5,	iSize)
		Array.Copy(dXStrgMod_B,			dXStrgMod_h5,	iSize)
		Array.Copy(dXElasMod_B,			dXElasMod_h5,	iSize)
		Array.Copy(dXLossTan_B,			dXLossTan_h5,	iSize)
	Case Else
		If IsNothing(iOxBuf_B)
			Exit Sub
		Else
			iSize = iOxBuf_B.Length
		End If

		Array.Resize(iOxBuf_h5,		iSize)
		Array.Resize(iOyBuf_h5,		iSize)
		Array.Resize(iOzBuf_h5,		iSize)
		Array.Resize(dOzBuf_h5,		iSize)
		Array.Resize(iPxBuf_h5,		iSize)
		Array.Resize(iPyBuf_h5,		iSize)
		Array.Resize(iPzBuf_h5,		iSize)
		Array.Resize(iHosei_h5,		iSize)
		Array.Resize(dHoseiP_h5,	iSize)
		Array.Resize(iTempBuf_h5,	iSize)
		Array.Resize(iMvBuf_h5,		iSize)
		Array.Resize(iTempBuf2_h5,	iSize)
		Array.Resize(iMvBuf2_h5,	iSize)
		Array.Resize(iSlope_h5,		iSize)
		Array.Resize(dXPosPhase_h5,	iSize)
		Array.Resize(dYPosPhase_h5,	iSize)
		Array.Resize(dViscosity_h5,	iSize)
		Array.Resize(dSplStrain_h5,	iSize)
		Array.Resize(dShearMod_h5,	iSize)
		Array.Resize(dShearVelo_h5,	iSize)
		Array.Resize(dXStrgMod_h5,	iSize)
		Array.Resize(dXElasMod_h5,	iSize)
		Array.Resize(dXLossTan_h5,	iSize)

		Array.Copy(iOxBuf_B,			iOxBuf_h5,		iSize)
		Array.Copy(iOyBuf_B,			iOyBuf_h5,		iSize)
		Array.Copy(iOzBuf_B,			iOzBuf_h5,		iSize)
		Array.Copy(dOzBuf_B,			dOzBuf_h5,		iSize)
		Array.Copy(iPxBuf_B,			iPxBuf_h5,		iSize)
		Array.Copy(iPyBuf_B,			iPyBuf_h5,		iSize)
		Array.Copy(iPzBuf_B,			iPzBuf_h5,		iSize)
		Array.Copy(iHosei_B,			iHosei_h5,		iSize)
		Array.Copy(dHosei100_B,			dHoseiP_h5,		iSize)
		Array.Copy(iTempBuf_B,			iTempBuf_h5,	iSize)
		Array.Copy(iMvBuf_B,			iMvBuf_h5,		iSize)
		Array.Copy(iTempBuf2_B,			iTempBuf2_h5,	iSize)
		Array.Copy(iMvBuf2_B,			iMvBuf2_h5,		iSize)
		Array.Copy(iSlope_B,			iSlope_h5,		iSize)
		Array.Copy(dXPosPhase_B,		dXPosPhase_h5,	iSize)
		Array.Copy(dYPosPhase_B,		dYPosPhase_h5,	iSize)
		Array.Copy(dViscosity_B,		dViscosity_h5,	iSize)
		Array.Copy(dSplStrain_B,		dSplStrain_h5,	iSize)
		Array.Copy(dShearMod_B,			dShearMod_h5,	iSize)
		Array.Copy(dShearVelo_B,		dShearVelo_h5,	iSize)
		Array.Copy(dXStrgMod_B,			dXStrgMod_h5,	iSize)
		Array.Copy(dXElasMod_B,			dXElasMod_h5,	iSize)
		Array.Copy(dXLossTan_B,			dXLossTan_h5,	iSize)
	End Select
	p_head_h(4) = p_head_B

Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub
Public Sub BToS1Paste(ByVal bLTMode As Boolean)
Try
	Dim iSize As Int32 = 0
	Select Case bLTMode
	Case True
		If IsNothing(iMSec_B)
			Exit Sub
		Else
			iSize = iMSec_B.Length
		End If

		Array.Resize(iMSec_s1,		iSize)
		Array.Resize(iHosei_s1,		iSize)
		Array.Resize(dHoseiP_s1,	iSize)
		Array.Resize(iTempBuf_s1,	iSize)
		Array.Resize(iMvBuf_s1,		iSize)
		Array.Resize(iTempBuf2_s1,	iSize)
		Array.Resize(iMvBuf2_s1,	iSize)
		Array.Resize(dXPosPhase_s1,	iSize)
		Array.Resize(dYPosPhase_s1,	iSize)
		Array.Resize(dViscosity_s1,	iSize)
		Array.Resize(dSplStrain_s1,	iSize)
		Array.Resize(dShearMod_s1,	iSize)
		Array.Resize(dShearVelo_s1,	iSize)
		Array.Resize(dXStrgMod_s1,	iSize)
		Array.Resize(dXElasMod_s1,	iSize)
		Array.Resize(dXLossTan_s1,	iSize)

		Array.Copy(iMSec_B,				iMSec_s1,		iSize)
		Array.Copy(iHosei_B,			iHosei_s1,		iSize)
		Array.Copy(dHosei100_B,			dHoseiP_s1,		iSize)
		Array.Copy(iTempBuf_B,			iTempBuf_s1,	iSize)
		Array.Copy(iMvBuf_B,			iMvBuf_s1,		iSize)
		Array.Copy(iTempBuf2_B,			iTempBuf2_s1,	iSize)
		Array.Copy(iMvBuf2_B,			iMvBuf2_s1,		iSize)
		Array.Copy(dXPosPhase_B,		dXPosPhase_s1,	iSize)
		Array.Copy(dYPosPhase_B,		dYPosPhase_s1,	iSize)
		Array.Copy(dViscosity_B,		dViscosity_s1,	iSize)
		Array.Copy(dSplStrain_B,		dSplStrain_s1,	iSize)
		Array.Copy(dShearMod_B,			dShearMod_s1,	iSize)
		Array.Copy(dShearVelo_B,		dShearVelo_s1,	iSize)
		Array.Copy(dXStrgMod_B,			dXStrgMod_s1,	iSize)
		Array.Copy(dXElasMod_B,			dXElasMod_s1,	iSize)
		Array.Copy(dXLossTan_B,			dXLossTan_s1,	iSize)
	Case Else
		If IsNothing(iOxBuf_B)
			Exit Sub
		Else
			iSize = iOxBuf_B.Length
		End If

		Array.Resize(iOxBuf_S1,		iSize)
		Array.Resize(iOyBuf_s1,		iSize)
		Array.Resize(iOzBuf_s1,		iSize)
		Array.Resize(dOzBuf_s1,		iSize)
		Array.Resize(iPxBuf_s1,		iSize)
		Array.Resize(iPyBuf_s1,		iSize)
		Array.Resize(iPzBuf_s1,		iSize)
		Array.Resize(iHosei_s1,		iSize)
		Array.Resize(dHoseiP_s1,	iSize)
		Array.Resize(iTempBuf_s1,	iSize)
		Array.Resize(iMvBuf_s1,		iSize)
		Array.Resize(iTempBuf2_s1,	iSize)
		Array.Resize(iMvBuf2_s1,	iSize)
		Array.Resize(iSlope_s1,		iSize)
		Array.Resize(dXPosPhase_s1,	iSize)
		Array.Resize(dYPosPhase_s1,	iSize)
		Array.Resize(dViscosity_s1,	iSize)
		Array.Resize(dSplStrain_s1,	iSize)
		Array.Resize(dShearMod_s1,	iSize)
		Array.Resize(dShearVelo_s1,	iSize)
		Array.Resize(dXStrgMod_s1,	iSize)
		Array.Resize(dXElasMod_s1,	iSize)
		Array.Resize(dXLossTan_s1,	iSize)

		Array.Copy(iOxBuf_B,			iOxBuf_s1,		iSize)
		Array.Copy(iOyBuf_B,			iOyBuf_s1,		iSize)
		Array.Copy(iOzBuf_B,			iOzBuf_s1,		iSize)
		Array.Copy(dOzBuf_B,			dOzBuf_s1,		iSize)
		Array.Copy(iPxBuf_B,			iPxBuf_s1,		iSize)
		Array.Copy(iPyBuf_B,			iPyBuf_s1,		iSize)
		Array.Copy(iPzBuf_B,			iPzBuf_s1,		iSize)
		Array.Copy(iHosei_B,			iHosei_s1,		iSize)
		Array.Copy(dHosei100_B,			dHoseiP_s1,		iSize)
		Array.Copy(iTempBuf_B,			iTempBuf_s1,	iSize)
		Array.Copy(iMvBuf_B,			iMvBuf_s1,		iSize)
		Array.Copy(iTempBuf2_B,			iTempBuf2_s1,	iSize)
		Array.Copy(iMvBuf2_B,			iMvBuf2_s1,		iSize)
		Array.Copy(iSlope_B,			iSlope_s1,		iSize)
		Array.Copy(dXPosPhase_B,		dXPosPhase_s1,	iSize)
		Array.Copy(dYPosPhase_B,		dYPosPhase_s1,	iSize)
		Array.Copy(dViscosity_B,		dViscosity_s1,	iSize)
		Array.Copy(dSplStrain_B,		dSplStrain_s1,	iSize)
		Array.Copy(dShearMod_B,			dShearMod_s1,	iSize)
		Array.Copy(dShearVelo_B,		dShearVelo_s1,	iSize)
		Array.Copy(dXStrgMod_B,			dXStrgMod_s1,	iSize)
		Array.Copy(dXElasMod_B,			dXElasMod_s1,	iSize)
		Array.Copy(dXLossTan_B,			dXLossTan_s1,	iSize)
	End Select
	p_head_s(0) = p_head_B

Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub
Public Sub BToS2Paste(ByVal bLTMode As Boolean)
Try
	Dim iSize As Int32 = 0
	Select Case bLTMode
	Case True
		If IsNothing(iMSec_B)
			Exit Sub
		Else
			iSize = iMSec_B.Length
		End If

		Array.Resize(iMSec_s2,		iSize)
		Array.Resize(iHosei_s2,		iSize)
		Array.Resize(dHoseiP_s2,	iSize)
		Array.Resize(iTempBuf_s2,	iSize)
		Array.Resize(iMvBuf_s2,		iSize)
		Array.Resize(iTempBuf2_s2,	iSize)
		Array.Resize(iMvBuf2_s2,	iSize)
		Array.Resize(dXPosPhase_s2,	iSize)
		Array.Resize(dYPosPhase_s2,	iSize)
		Array.Resize(dViscosity_s2,	iSize)
		Array.Resize(dSplStrain_s2,	iSize)
		Array.Resize(dShearMod_s2,	iSize)
		Array.Resize(dShearVelo_s2,	iSize)
		Array.Resize(dXStrgMod_s2,	iSize)
		Array.Resize(dXElasMod_s2,	iSize)
		Array.Resize(dXLossTan_s2,	iSize)

		Array.Copy(iMSec_B,				iMSec_s2,		iSize)
		Array.Copy(iHosei_B,			iHosei_s2,		iSize)
		Array.Copy(dHosei100_B,			dHoseiP_s2,		iSize)
		Array.Copy(iTempBuf_B,			iTempBuf_s2,	iSize)
		Array.Copy(iMvBuf_B,			iMvBuf_s2,		iSize)
		Array.Copy(iTempBuf2_B,			iTempBuf2_s2,	iSize)
		Array.Copy(iMvBuf2_B,			iMvBuf2_s2,		iSize)
		Array.Copy(dXPosPhase_B,		dXPosPhase_s2,	iSize)
		Array.Copy(dYPosPhase_B,		dYPosPhase_s2,	iSize)
		Array.Copy(dViscosity_B,		dViscosity_s2,	iSize)
		Array.Copy(dSplStrain_B,		dSplStrain_s2,	iSize)
		Array.Copy(dShearMod_B,			dShearMod_s2,	iSize)
		Array.Copy(dShearVelo_B,		dShearVelo_s2,	iSize)
		Array.Copy(dXStrgMod_B,			dXStrgMod_s2,	iSize)
		Array.Copy(dXElasMod_B,			dXElasMod_s2,	iSize)
		Array.Copy(dXLossTan_B,			dXLossTan_s2,	iSize)
	Case Else
		If IsNothing(iOxBuf_B)
			Exit Sub
		Else
			iSize = iOxBuf_B.Length
		End If

		Array.Resize(iOxBuf_s2,		iSize)
		Array.Resize(iOyBuf_s2,		iSize)
		Array.Resize(iOzBuf_s2,		iSize)
		Array.Resize(dOzBuf_s2,		iSize)
		Array.Resize(iPxBuf_s2,		iSize)
		Array.Resize(iPyBuf_s2,		iSize)
		Array.Resize(iPzBuf_s2,		iSize)
		Array.Resize(iHosei_s2,		iSize)
		Array.Resize(dHoseiP_s2,	iSize)
		Array.Resize(iTempBuf_s2,	iSize)
		Array.Resize(iMvBuf_s2,		iSize)
		Array.Resize(iTempBuf2_s2,	iSize)
		Array.Resize(iMvBuf2_s2,	iSize)
		Array.Resize(iSlope_s2,		iSize)
		Array.Resize(dXPosPhase_s2,	iSize)
		Array.Resize(dYPosPhase_s2,	iSize)
		Array.Resize(dViscosity_s2,	iSize)
		Array.Resize(dSplStrain_s2,	iSize)
		Array.Resize(dShearMod_s2,	iSize)
		Array.Resize(dShearVelo_s2,	iSize)
		Array.Resize(dXStrgMod_s2,	iSize)
		Array.Resize(dXElasMod_s2,	iSize)
		Array.Resize(dXLossTan_s2,	iSize)

		Array.Copy(iOxBuf_B,			iOxBuf_s2,		iSize)
		Array.Copy(iOyBuf_B,			iOyBuf_s2,		iSize)
		Array.Copy(iOzBuf_B,			iOzBuf_s2,		iSize)
		Array.Copy(dOzBuf_B,			dOzBuf_s2,		iSize)
		Array.Copy(iPxBuf_B,			iPxBuf_s2,		iSize)
		Array.Copy(iPyBuf_B,			iPyBuf_s2,		iSize)
		Array.Copy(iPzBuf_B,			iPzBuf_s2,		iSize)
		Array.Copy(iHosei_B,			iHosei_s2,		iSize)
		Array.Copy(dHosei100_B,			dHoseiP_s2,		iSize)
		Array.Copy(iTempBuf_B,			iTempBuf_s2,	iSize)
		Array.Copy(iMvBuf_B,			iMvBuf_s2,		iSize)
		Array.Copy(iTempBuf2_B,			iTempBuf2_s2,	iSize)
		Array.Copy(iMvBuf2_B,			iMvBuf2_s2,		iSize)
		Array.Copy(iSlope_B,			iSlope_s2,		iSize)
		Array.Copy(dXPosPhase_B,		dXPosPhase_s2,	iSize)
		Array.Copy(dYPosPhase_B,		dYPosPhase_s2,	iSize)
		Array.Copy(dViscosity_B,		dViscosity_s2,	iSize)
		Array.Copy(dSplStrain_B,		dSplStrain_s2,	iSize)
		Array.Copy(dShearMod_B,			dShearMod_s2,	iSize)
		Array.Copy(dShearVelo_B,		dShearVelo_s2,	iSize)
		Array.Copy(dXStrgMod_B,			dXStrgMod_s2,	iSize)
		Array.Copy(dXElasMod_B,			dXElasMod_s2,	iSize)
		Array.Copy(dXLossTan_B,			dXLossTan_s2,	iSize)
	End Select
	p_head_s(1) = p_head_B

Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub

''' <summary>
''' 蓄積配列バッファ消去
''' </summary>
Public sub ReadBufferClear(ByVal Optional bHeaderCls As Boolean = True)
	If bHeaderCls
		p_head		= Nothing
	End if
	iMSec		= Nothing
	iOxBuf		= Nothing
	iOyBuf		= Nothing
	iOzBuf		= Nothing
	dOzBuf		= Nothing
	iPxBuf		= Nothing
	iPyBuf		= Nothing
	iPzBuf		= Nothing
	iHosei		= Nothing
	dHosei100	= Nothing
	iSlope		= Nothing
	iTempBuf	= Nothing
	iMvBuf		= Nothing
	iTempBuf2	= Nothing
	iMvBuf2		= Nothing
	dViscosity	= Nothing
	dXPosPhase	= Nothing
	dYPosPhase	= Nothing
	dSplStrain	= Nothing
	dShearMod	= Nothing
	dShearVelo	= Nothing
	dShearStress= Nothing
	dXStrgMod	= Nothing
	dXElasMod	= Nothing
	dXLossTan	= Nothing
	dYStrgMod	= Nothing
	dYElasMod	= Nothing
	dYLossTan	= Nothing
	datCal.tempXp_QM    = Nothing
	datCal.tempYp_QM    = Nothing

	ViscosityBufferClear()
End sub

''' <summary>
''' 蓄積配列バッファ消去 長期間欠モード
''' </summary>
Public sub LTReadBufferClear(ByVal Optional bHeaderCls As Boolean = True)
	If bHeaderCls
		p_head		= Nothing
	End if
	iLT_MSec		= Nothing
	iLT_Hosei		= Nothing
	dLT_Hosei100	= Nothing
	iLT_Slope		= Nothing
	iLT_TempBuf	= Nothing
	iLT_MvBuf		= Nothing
	iLT_TempBuf2	= Nothing
	iLT_MvBuf2		= Nothing
	dLT_Viscosity	= Nothing
	dLT_XPosPhase	= Nothing
	dLT_YPosPhase	= Nothing
	dLT_SplStrain	= Nothing
	dLT_ShearMod	= Nothing
	dLT_ShearVelo	= Nothing
	dLT_XStrgMod	= Nothing
	dLT_XElasMod	= Nothing
	dLT_XLossTan	= Nothing
	dLT_YStrgMod	= Nothing
	dLT_YElasMod	= Nothing
	dLT_YLossTan	= Nothing
End sub
''' <summary>
''' 粘度測定結果バッファクリア
''' </summary>
Public Sub ViscosityBufferClear
	dShearVelocity	= Nothing								'せん断速度(1/S)
	dShearStress	= Nothing								'せん断応力(Pa)
	dViscosity		= Nothing								'粘度(PaS)
End Sub
''' <summary>
''' 粘度測定結果バッファ確保
''' </summary>
Public Sub ViscosityBufferAllocate(ByVal iLen As Int32)
	Array.Resize(dShearVelocity, iLen )						'せん断速度(1/S)
	Array.Resize(dShearStress, iLen )						'せん断応力(Pa)
	Array.Resize(dViscosity, iLen )							'粘度(PaS)
End Sub


#If 0
'測定値領域を一時退避領域へコピー
Public Sub TemporaryEvacuration(ByVal bLTMode As Boolean)
	Select Case bLTMode
	Case True
		Dim iSize As Int32 = iLT_MSec.Length

		Array.Resize(iMSec_B,		iSize)
		Array.Resize(iHosei_B,		iSize)
		Array.Resize(dHosei100_B,	iSize)
		Array.Resize(iTempBuf_B,	iSize)
		Array.Resize(iMvBuf_B,		iSize)
		Array.Resize(iTempBuf2_B,	iSize)
		Array.Resize(iMvBuf2_B,		iSize)
		Array.Resize(dXPosPhase_B,	iSize)
		Array.Resize(dYPosPhase_B,	iSize)

		Array.Resize(dViscosity_B,	iSize)
		Array.Resize(dSplStrain_B,	iSize)
		Array.Resize(dShearMod_B,	iSize)
		Array.Resize(dShearVelo_B,	iSize)
		Array.Resize(dXStrgMod_B,	iSize)
		Array.Resize(dXElasMod_B,	iSize)
		Array.Resize(dXLossTan_B,	iSize)


		Array.Copy(iLT_MSec,			iMSec_B,		iSize)
		Array.Copy(iLT_Hosei,			iHosei_B,		iSize)
		Array.Copy(dLT_Hosei100,		dHosei100_B,	iSize)
		Array.Copy(iLT_TempBuf,			iTempBuf_B,		iSize)
		Array.Copy(iLT_MvBuf,			iMvBuf_B,		iSize)
		Array.Copy(iLT_TempBuf2,		iTempBuf2_B,	iSize)
		Array.Copy(iLT_MvBuf2,			iMvBuf2_B,		iSize)
		Array.Copy(dLT_XPosPhase,		dXPosPhase_B,	iSize)
		Array.Copy(dLT_YPosPhase,		dYPosPhase_B,	iSize)

		Array.Copy(dLT_Viscosity,		dViscosity_B,	iSize)
		Array.Copy(dLT_SplStrain,		dSplStrain_B,	iSize)
		Array.Copy(dLT_ShearMod,		dShearMod_B,	iSize)
		Array.Copy(dLT_ShearVelo,		dShearVelo_B,	iSize)
		Array.Copy(dLT_XStrgMod,		dXStrgMod_B,	iSize)
		Array.Copy(dLT_XElasMod,		dXElasMod_B,	iSize)
		Array.Copy(dLT_XLossTan,		dXLossTan_B,	iSize)
	Case Else
		Dim iSize As Int32 = iOxBuf.Length

		Array.Resize(iOxBuf_B,		iSize)
		Array.Resize(iOyBuf_B,		iSize)
		Array.Resize(iOzBuf_B,		iSize)
		Array.Resize(dOzBuf_B,		iSize)
		Array.Resize(iPxBuf_B,		iSize)
		Array.Resize(iPyBuf_B,		iSize)
		Array.Resize(iPzBuf_B,		iSize)
		Array.Resize(iHosei_B,		iSize)
		Array.Resize(dHosei100_B,	iSize)
		Array.Resize(iTempBuf_B,	iSize)
		Array.Resize(iMvBuf_B,		iSize)
		Array.Resize(iTempBuf2_B,	iSize)
		Array.Resize(iMvBuf2_B,		iSize)
		Array.Resize(iSlope_B,		iSize)
		Array.Resize(dXPosPhase_B,	iSize)
		Array.Resize(dYPosPhase_B,	iSize)

		Array.Resize(dViscosity_B,	iSize)
		Array.Resize(dSplStrain_B,	iSize)
		Array.Resize(dShearMod_B,	iSize)
		Array.Resize(dShearVelo_B,	iSize)
		Array.Resize(dXStrgMod_B,	iSize)
		Array.Resize(dXElasMod_B,	iSize)
		Array.Resize(dXLossTan_B,	iSize)
		Array.Resize(dYStrgMod_B,	iSize)
		Array.Resize(dYElasMod_B,	iSize)
		Array.Resize(dYLossTan_B,	iSize)


		Array.Copy(iOxBuf,			iOxBuf_B,		iSize)
		Array.Copy(iOyBuf,			iOyBuf_B,		iSize)
		Array.Copy(iOzBuf,			iOZBuf_B,		iSize)
		Array.Copy(dOzBuf,			dOzBuf_B,		iSize)
		Array.Copy(iPxBuf,			iPxBuf_B,		iSize)
		Array.Copy(iPyBuf,			iPyBuf_B,		iSize)
		Array.Copy(iPzBuf,			iPzBuf_B,		iSize)
		Array.Copy(iHosei,			iHosei_B,		iSize)
		Array.Copy(dHosei100,		dHosei100_B,	iSize)
		Array.Copy(iTempBuf,		iTempBuf_B,		iSize)
		Array.Copy(iMvBuf,			iMvBuf_B,		iSize)
		Array.Copy(iTempBuf2,		iTempBuf2_B,	iSize)
		Array.Copy(iMvBuf2,			iMvBuf2_B,		iSize)
		Array.Copy(iSlope,			iSlope_B,		iSize)
		Array.Copy(dXPosPhase,		dXPosPhase_B,	iSize)
		Array.Copy(dYPosPhase,		dYPosPhase_B,	iSize)

		Array.Copy(dViscosity,		dViscosity_B,	iSize)
		Array.Copy(dSplStrain,		dSplStrain_B,	iSize)
		Array.Copy(dShearMod,		dShearMod_B,	iSize)
		Array.Copy(dShearVelo,		dShearVelo_B,	iSize)
		Array.Copy(dXStrgMod,		dXStrgMod_B,	iSize)
		Array.Copy(dXElasMod,		dXElasMod_B,	iSize)
		Array.Copy(dXLossTan,		dXLossTan_B,	iSize)
		Array.Copy(dYStrgMod,		dYStrgMod_B,	iSize)
		Array.Copy(dYElasMod,		dYElasMod_B,	iSize)
		Array.Copy(dYLossTan,		dYLossTan_B,	iSize)
	End Select
	p_head_B = p_head


End Sub
#end if
'現在値領域にある計算結果を基準値領域へコピーする
Public Sub	CurrToStandardCopy(ByVal iSel As int32, ByVal bLTMode As Boolean)
	Select Case bLTMode
	Case True
		Dim iSize As Int32 = iOxBuf.Length
		Select Case iSel
		Case 1
			Array.Resize(iMsec_s1,		iSize)
			Array.Resize(iHosei_s1,		iSize)
			Array.Resize(dHoseiP_s1,	iSize)
			Array.Resize(iTempBuf_s1,	iSize)
			Array.Resize(iMvBuf_s1,		iSize)
			Array.Resize(iTempBuf2_s1,	iSize)
			Array.Resize(iMvBuf2_s1,	iSize)
			Array.Resize(dXPosPhase_s1,	iSize)
			Array.Resize(dYPosPhase_s1,	iSize)
			Array.Resize(dViscosity_s1,	iSize)
			Array.Resize(dSplStrain_s1,	iSize)
			Array.Resize(dShearMod_s1,	iSize)
			Array.Resize(dShearVelo_s1,	iSize)
			Array.Resize(dXStrgMod_s1,	iSize)
			Array.Resize(dXElasMod_s1,	iSize)
			Array.Resize(dXLossTan_s1,	iSize)

			Array.Copy(iLT_MSec,			iMSec_s1,		iSize)
			Array.Copy(iLT_Hosei,			iHosei_s1,		iSize)
			Array.Copy(dLT_Hosei100,		dHoseiP_s1,		iSize)
			Array.Copy(iLT_TempBuf,			iTempBuf_s1,	iSize)
			Array.Copy(iLT_MvBuf,			iMvBuf_s1,		iSize)
			Array.Copy(iLT_TempBuf2,		iTempBuf2_s1,	iSize)
			Array.Copy(iLT_MvBuf2,			iMvBuf2_s1,		iSize)
			Array.Copy(dLT_XPosPhase,		dXPosPhase_s1,	iSize)
			Array.Copy(dLT_YPosPhase,		dYPosPhase_s1,	iSize)

			Array.Copy(dLT_Viscosity,		dViscosity_s1,	iSize)
			Array.Copy(dLT_SplStrain,		dSplStrain_s1,	iSize)
			Array.Copy(dLT_ShearMod,		dShearMod_s1,	iSize)
			Array.Copy(dLT_ShearVelo,		dShearVelo_s1,	iSize)
			Array.Copy(dLT_XStrgMod,		dXStrgMod_s1,	iSize)
			Array.Copy(dLT_XElasMod,		dXElasMod_s1,	iSize)
			Array.Copy(dLT_XLossTan,		dXLossTan_s1,	iSize)
		Case 2
			Array.Resize(iMsec_s2,		iSize)
			Array.Resize(iHosei_s2,		iSize)
			Array.Resize(dHoseiP_s2,	iSize)
			Array.Resize(iTempBuf_s2,	iSize)
			Array.Resize(iMvBuf_s2,		iSize)
			Array.Resize(iTempBuf2_s2,	iSize)
			Array.Resize(iMvBuf2_s2,	iSize)
			Array.Resize(dXPosPhase_s2,	iSize)
			Array.Resize(dYPosPhase_s2,	iSize)
			Array.Resize(dViscosity_s2,	iSize)
			Array.Resize(dSplStrain_s2,	iSize)
			Array.Resize(dShearMod_s2,	iSize)
			Array.Resize(dShearVelo_s2,	iSize)
			Array.Resize(dXStrgMod_s2,	iSize)
			Array.Resize(dXElasMod_s2,	iSize)
			Array.Resize(dXLossTan_s2,	iSize)

			Array.Copy(iLT_MSec,			iMSec_s2,		iSize)
			Array.Copy(iLT_Hosei,			iHosei_s2,		iSize)
			Array.Copy(dLT_Hosei100,		dHoseiP_s2,		iSize)
			Array.Copy(iLT_TempBuf,			iTempBuf_s2,	iSize)
			Array.Copy(iLT_MvBuf,			iMvBuf_s2,		iSize)
			Array.Copy(iLT_TempBuf2,		iTempBuf2_s2,	iSize)
			Array.Copy(iLT_MvBuf2,			iMvBuf2_s2,		iSize)
			Array.Copy(dLT_XPosPhase,		dXPosPhase_s2,	iSize)
			Array.Copy(dLT_YPosPhase,		dYPosPhase_s2,	iSize)

			Array.Copy(dLT_Viscosity,		dViscosity_s2,	iSize)
			Array.Copy(dLT_SplStrain,		dSplStrain_s2,	iSize)
			Array.Copy(dLT_ShearMod,		dShearMod_s2,	iSize)
			Array.Copy(dLT_ShearVelo,		dShearVelo_s2,	iSize)
			Array.Copy(dLT_XStrgMod,		dXStrgMod_s2,	iSize)
			Array.Copy(dLT_XElasMod,		dXElasMod_s2,	iSize)
			Array.Copy(dLT_XLossTan,		dXLossTan_s2,	iSize)
		End Select
	Case Else
		Dim iSize As Int32 = iOxBuf.Length
		Select Case iSel
		Case 1
			Array.Resize(iOxBuf_s1,		iSize)
			Array.Resize(iOyBuf_s1,		iSize)
			Array.Resize(iOzBuf_s1,		iSize)
			Array.Resize(dOzBuf_s1,		iSize)
			Array.Resize(iPxBuf_s1,		iSize)
			Array.Resize(iPyBuf_s1,		iSize)
			Array.Resize(iPzBuf_s1,		iSize)
			Array.Resize(iHosei_s1,		iSize)
			Array.Resize(dHoseiP_s1,	iSize)
			Array.Resize(iTempBuf_s1,	iSize)
			Array.Resize(iMvBuf_s1,		iSize)
			Array.Resize(iTempBuf2_s1,	iSize)
			Array.Resize(iMvBuf2_s1,	iSize)
			Array.Resize(iSlope_s1,		iSize)
			Array.Resize(dXPosPhase_s1,	iSize)
			Array.Resize(dYPosPhase_s1,	iSize)
			Array.Resize(dViscosity_s1,	iSize)
			Array.Resize(dSplStrain_s1,	iSize)
			Array.Resize(dShearMod_s1,	iSize)
			Array.Resize(dShearVelo_s1,	iSize)
			Array.Resize(dXStrgMod_s1,	iSize)
			Array.Resize(dXElasMod_s1,	iSize)
			Array.Resize(dXLossTan_s1,	iSize)

			Array.Copy(iOxBuf,			iOxBuf_s1,		iSize)
			Array.Copy(iOyBuf,			iOyBuf_s1,		iSize)
			Array.Copy(iOzBuf,			iOzBuf_s1,		iSize)
			Array.Copy(dOzBuf,			dOzBuf_s1,		iSize)
			Array.Copy(iPxBuf,			iPxBuf_s1,		iSize)
			Array.Copy(iPyBuf,			iPyBuf_s1,		iSize)
			Array.Copy(iPzBuf,			iPzBuf_s1,		iSize)
			Array.Copy(iHosei,			iHosei_s1,		iSize)
			Array.Copy(dHosei100,		dHoseiP_s1,		iSize)
			Array.Copy(iSlope,			iSlope_s1,		iSize)
			Array.Copy(iTempBuf,		iTempBuf_s1,	iSize)
			Array.Copy(iMvBuf,			iMvBuf_s1,		iSize)
			Array.Copy(iTempBuf2,		iTempBuf2_s1,	iSize)
			Array.Copy(iMvBuf2,			iMvBuf2_s1,		iSize)
			Array.Copy(dXPosPhase,		dXPosPhase_s1,	iSize)
			Array.Copy(dYPosPhase,		dYPosPhase_s1,	iSize)

			Array.Copy(dViscosity,		dViscosity_s1,	iSize)
			Array.Copy(dSplStrain,		dSplStrain_s1,	iSize)
			Array.Copy(dShearMod,		dShearMod_s1,	iSize)
			Array.Copy(dShearVelo,		dShearVelo_s1,	iSize)
			Array.Copy(dXStrgMod,		dXStrgMod_s1,	iSize)
			Array.Copy(dXElasMod,		dXElasMod_s1,	iSize)
			Array.Copy(dXLossTan,		dXLossTan_s1,	iSize)
		Case 2
			Array.Resize(iOxBuf_s2,		iSize)
			Array.Resize(iOyBuf_s2,		iSize)
			Array.Resize(iOzBuf_s2,		iSize)
			Array.Resize(dOzBuf_s2,		iSize)
			Array.Resize(iPxBuf_s2,		iSize)
			Array.Resize(iPyBuf_s2,		iSize)
			Array.Resize(iPzBuf_s2,		iSize)
			Array.Resize(iHosei_s2,		iSize)
			Array.Resize(dHoseiP_s2,	iSize)
			Array.Resize(iTempBuf_s2,	iSize)
			Array.Resize(iMvBuf_s2,		iSize)
			Array.Resize(iTempBuf2_s2,	iSize)
			Array.Resize(iMvBuf2_s2,	iSize)
			Array.Resize(iSlope_s2,		iSize)
			Array.Resize(dXPosPhase_s2,	iSize)
			Array.Resize(dYPosPhase_s2,	iSize)
			Array.Resize(dViscosity_s2,	iSize)
			Array.Resize(dSplStrain_s2,	iSize)
			Array.Resize(dShearMod_s2,	iSize)
			Array.Resize(dShearVelo_s2,	iSize)
			Array.Resize(dXStrgMod_s2,	iSize)
			Array.Resize(dXElasMod_s2,	iSize)
			Array.Resize(dXLossTan_s2,	iSize)

			Array.Copy(iOxBuf,			iOxBuf_s2,		iSize)
			Array.Copy(iOyBuf,			iOyBuf_s2,		iSize)
			Array.Copy(iOzBuf,			iOzBuf_s2,		iSize)
			Array.Copy(dOzBuf,			dOzBuf_s2,		iSize)
			Array.Copy(iPxBuf,			iPxBuf_s2,		iSize)
			Array.Copy(iPyBuf,			iPyBuf_s2,		iSize)
			Array.Copy(iPzBuf,			iPzBuf_s2,		iSize)
			Array.Copy(iHosei,			iHosei_s2,		iSize)
			Array.Copy(dHosei100,		dHoseiP_s2,		iSize)
			Array.Copy(iSlope,			iSlope_s2,		iSize)
			Array.Copy(iTempBuf,		iTempBuf_s2,	iSize)
			Array.Copy(iMvBuf,			iMvBuf_s2,		iSize)
			Array.Copy(iTempBuf2,		iTempBuf2_s2,	iSize)
			Array.Copy(iMvBuf2,			iMvBuf2_s2,		iSize)
			Array.Copy(dXPosPhase,		dXPosPhase_s2,	iSize)
			Array.Copy(dYPosPhase,		dYPosPhase_s2,	iSize)

			Array.Copy(dViscosity,		dViscosity_s2,	iSize)
			Array.Copy(dSplStrain,		dSplStrain_s2,	iSize)
			Array.Copy(dShearMod,		dShearMod_s2,	iSize)
			Array.Copy(dShearVelo,		dShearVelo_s2,	iSize)
			Array.Copy(dXStrgMod,		dXStrgMod_s2,	iSize)
			Array.Copy(dXElasMod,		dXElasMod_s2,	iSize)
			Array.Copy(dXLossTan,		dXLossTan_s2,	iSize)
		End Select
	End Select
End Sub
#If 0
'一時退避を現在値領域にコピーする
Public Sub	TemporaryToCurrCopy(ByVal bLTMode As Boolean)

	Select Case bLTMode
	Case True
		Dim iSize As Int32 = iMsec_B.Length
		Array.Resize(iLT_Msec,			iSize)
		Array.Resize(iLT_Hosei,			iSize)
		Array.Resize(dLT_Hosei100,		iSize)
		Array.Resize(iLT_TempBuf,		iSize)
		Array.Resize(iLT_MvBuf,			iSize)
		Array.Resize(iLT_TempBuf2,		iSize)
		Array.Resize(iLT_MvBuf2,		iSize)
		Array.Resize(dLT_XPosPhase,		iSize)
		Array.Resize(dLT_YPosPhase,		iSize)
		Array.Resize(dLT_Viscosity,		iSize)
		Array.Resize(dLT_SplStrain,		iSize)
		Array.Resize(dLT_ShearMod,		iSize)
		Array.Resize(dLT_ShearVelo,		iSize)
		Array.Resize(dLT_XStrgMod,		iSize)
		Array.Resize(dLT_XElasMod,		iSize)
		Array.Resize(dLT_XLossTan,		iSize)

		Array.Copy(iMsec_B,				iLT_MSec,		iSize)
		Array.Copy(iHosei_B,			iLT_Hosei,		iSize)
		Array.Copy(dHosei100_B,			dLT_Hosei100,	iSize)
		Array.Copy(iTempBuf_B,			iLT_TempBuf,	iSize)
		Array.Copy(iMvBuf_B,			iLT_MvBuf,		iSize)
		Array.Copy(iTempBuf2_B,			iLT_TempBuf2,	iSize)
		Array.Copy(iMvBuf2_B,			iLT_MvBuf2,		iSize)
		Array.Copy(dXPosPhase_B,		dLT_XPosPhase,	iSize)
		Array.Copy(dYPosPhase_B,		dLT_YPosPhase,	iSize)
		Array.Copy(dYPosPhase_B,		dLT_YPosPhase,	iSize)
		Array.Copy(dViscosity_B,		dLT_Viscosity,	iSize)
		Array.Copy(dSplStrain_B,		dLT_SplStrain,	iSize)
		Array.Copy(dShearMod_B,			dLT_ShearMod,	iSize)
		Array.Copy(dShearVelo_B,		dLT_ShearVelo,	iSize)
		Array.Copy(dXStrgMod_B,			dLT_XStrgMod,	iSize)
		Array.Copy(dXElasMod_B,			dLT_XElasMod,	iSize)
		Array.Copy(dXLossTan_B,			dLT_XLossTan,	iSize)
	Case Else
		Dim iSize As Int32 = iOxBuf_B.Length
		Array.Resize(iOxBuf,		iSize)
		Array.Resize(iOyBuf,		iSize)
		Array.Resize(iOzBuf,		iSize)
		Array.Resize(dOzBuf,		iSize)
		Array.Resize(iPxBuf,		iSize)
		Array.Resize(iPyBuf,		iSize)
		Array.Resize(iPzBuf,		iSize)
		Array.Resize(iHosei,		iSize)
		Array.Resize(dHosei100,		iSize)
		Array.Resize(iTempBuf,		iSize)
		Array.Resize(iMvBuf,		iSize)
		Array.Resize(iTempBuf2,		iSize)
		Array.Resize(iMvBuf2,		iSize)
		Array.Resize(iSlope,		iSize)
		Array.Resize(dXPosPhase,	iSize)
		Array.Resize(dYPosPhase,	iSize)
		Array.Resize(dViscosity,	iSize)
		Array.Resize(dSplStrain,	iSize)
		Array.Resize(dShearMod,		iSize)
		Array.Resize(dShearVelo,	iSize)
		Array.Resize(dXStrgMod,		iSize)
		Array.Resize(dXElasMod,		iSize)
		Array.Resize(dXLossTan,		iSize)
		Array.Resize(dYStrgMod,		iSize)
		Array.Resize(dYElasMod,		iSize)
		Array.Resize(dYLossTan,		iSize)

		Array.Copy(iOxBuf_B,			iOxBuf,		iSize)
		Array.Copy(iOyBuf_B,			iOyBuf,		iSize)
		Array.Copy(iOzBuf_B,			iOzBuf,		iSize)
		Array.Copy(dOzBuf_B,			dOzBuf,		iSize)
		Array.Copy(iPxBuf_B,			iPxBuf,		iSize)
		Array.Copy(iPyBuf_B,			iPyBuf,		iSize)
		Array.Copy(iPzBuf_B,			iPzBuf,		iSize)
		Array.Copy(iHosei_B,			iHosei,		iSize)
		Array.Copy(dHosei100_B,			dHosei100,	iSize)
		Array.Copy(iTempBuf_B,			iTempBuf,	iSize)
		Array.Copy(iMvBuf_B,			iMvBuf,		iSize)
		Array.Copy(iTempBuf2_B,			iTempBuf2,	iSize)
		Array.Copy(iMvBuf2_B,			iMvBuf2,	iSize)
		Array.Copy(iSlope_B,			iSlope,		iSize)
		Array.Copy(dXPosPhase_B,		dXPosPhase,	iSize)
		Array.Copy(dYPosPhase_B,		dYPosPhase,	iSize)

		Array.Copy(dViscosity_B,		dViscosity,	iSize)
		Array.Copy(dSplStrain_B,		dSplStrain,	iSize)
		Array.Copy(dShearMod_B,			dShearMod,	iSize)
		Array.Copy(dShearVelo_B,		dShearVelo,	iSize)
		Array.Copy(dXStrgMod_B,			dXStrgMod,	iSize)
		Array.Copy(dXElasMod_B,			dXElasMod,	iSize)
		Array.Copy(dXLossTan_B,			dXLossTan,	iSize)
		Array.Copy(dYStrgMod_B,			dYStrgMod,	iSize)
		Array.Copy(dYElasMod_B,			dYElasMod,	iSize)
		Array.Copy(dYLossTan_B,			dYLossTan,	iSize)
	End Select
	p_head = p_head_B
End Sub
'一時退避を現在値領域にコピーする
Public Sub	StandardToCurrCopy(ByVal iSel As Int32, ByVal bLTMode As Boolean)
	Dim iSize As Int32
	Select Case bLTMode
	Case True
		Select Case iSel
		Case 1
			If Not IsNothing(iMsec_s1)
				iSize = iMsec_s1.Length
				Array.Resize(iLT_MSec,		iSize)
				Array.Resize(iLT_Hosei,		iSize)
				Array.Resize(dLT_Hosei100,	iSize)
				Array.Resize(iLT_TempBuf,	iSize)
				Array.Resize(iLT_MvBuf,		iSize)
				Array.Resize(iLT_TempBuf2,	iSize)
				Array.Resize(iLT_MvBuf2,	iSize)
				Array.Resize(dLT_XPosPhase,	iSize)
				Array.Resize(dLT_YPosPhase,	iSize)

				Array.Resize(dLT_Viscosity,	iSize)
				Array.Resize(dLT_SplStrain,	iSize)
				Array.Resize(dLT_ShearMod,	iSize)
				Array.Resize(dLT_ShearVelo,	iSize)
				Array.Resize(dLT_XStrgMod,	iSize)
				Array.Resize(dLT_XElasMod,	iSize)
				Array.Resize(dLT_XLossTan,	iSize)


				Array.Copy(iMsec_s1,			iLT_MSec,		iSize)
				Array.Copy(iHosei_s1,			iLT_Hosei,		iSize)
				Array.Copy(dHoseiP_s1,			dLT_Hosei100,	iSize)
				Array.Copy(iTempBuf_s1,			iLT_TempBuf,	iSize)
				Array.Copy(iMvBuf_s1,			iLT_MvBuf,		iSize)
				Array.Copy(iTempBuf2_s1,		iLT_TempBuf2,	iSize)
				Array.Copy(iMvBuf2_s1,			iLT_MvBuf2,		iSize)
				Array.Copy(dXPosPhase_s1,		dLT_XPosPhase,	iSize)
				Array.Copy(dYPosPhase_s1,		dLT_YPosPhase,	iSize)

				Array.Copy(dViscosity_s1,		dLT_Viscosity,	iSize)
				Array.Copy(dSplStrain_s1,		dLT_SplStrain,	iSize)
				Array.Copy(dShearMod_s1,		dLT_ShearMod,	iSize)
				Array.Copy(dShearVelo_s1,		dLT_ShearVelo,	iSize)
				Array.Copy(dXStrgMod_s1,		dLT_XStrgMod,	iSize)
				Array.Copy(dXElasMod_s1,		dLT_XElasMod,	iSize)
				Array.Copy(dXLossTan_s1,		dLT_XLossTan,	iSize)
			End If
		Case 2
			If Not IsNothing(iMsec_s2)
				iSize = iMsec_s2.Length
				Array.Resize(iLT_MSec,		iSize)
				Array.Resize(iLT_Hosei,		iSize)
				Array.Resize(dLT_Hosei100,	iSize)
				Array.Resize(iLT_TempBuf,	iSize)
				Array.Resize(iLT_MvBuf,		iSize)
				Array.Resize(iLT_TempBuf2,	iSize)
				Array.Resize(iLT_MvBuf2,	iSize)
				Array.Resize(dLT_XPosPhase,	iSize)
				Array.Resize(dLT_YPosPhase,	iSize)

				Array.Resize(dLT_Viscosity,	iSize)
				Array.Resize(dLT_SplStrain,	iSize)
				Array.Resize(dLT_ShearMod,	iSize)
				Array.Resize(dLT_ShearVelo,	iSize)
				Array.Resize(dLT_XStrgMod,	iSize)
				Array.Resize(dLT_XElasMod,	iSize)
				Array.Resize(dLT_XLossTan,	iSize)


				Array.Copy(iMsec_s2,			iLT_MSec,		iSize)
				Array.Copy(iHosei_s2,			iLT_Hosei,		iSize)
				Array.Copy(dHoseiP_s2,			dLT_Hosei100,	iSize)
				Array.Copy(iTempBuf_s2,			iLT_TempBuf,	iSize)
				Array.Copy(iMvBuf_s2,			iLT_MvBuf,		iSize)
				Array.Copy(iTempBuf2_s2,		iLT_TempBuf2,	iSize)
				Array.Copy(iMvBuf2_s2,			iLT_MvBuf2,		iSize)
				Array.Copy(dXPosPhase_s2,		dLT_XPosPhase,	iSize)
				Array.Copy(dYPosPhase_s2,		dLT_YPosPhase,	iSize)

				Array.Copy(dViscosity_s2,		dLT_Viscosity,	iSize)
				Array.Copy(dSplStrain_s2,		dLT_SplStrain,	iSize)
				Array.Copy(dShearMod_s2,		dLT_ShearMod,	iSize)
				Array.Copy(dShearVelo_s2,		dLT_ShearVelo,	iSize)
				Array.Copy(dXStrgMod_s2,		dLT_XStrgMod,	iSize)
				Array.Copy(dXElasMod_s2,		dLT_XElasMod,	iSize)
				Array.Copy(dXLossTan_s2,		dLT_XLossTan,	iSize)
			End If
		End Select
	Case Else
		Select Case iSel
		Case 1
			If Not IsNothing(iOxBuf_s1)
				iSize = iOxBuf_s1.Length
				Array.Resize(iOxBuf,	iSize)
				Array.Resize(iOyBuf,	iSize)
				Array.Resize(iOzBuf,	iSize)
				Array.Resize(dOzBuf,	iSize)
				Array.Resize(iPxBuf,	iSize)
				Array.Resize(iPyBuf,	iSize)
				Array.Resize(iPzBuf,	iSize)
				Array.Resize(iHosei,	iSize)
				Array.Resize(dHosei100,	iSize)
				Array.Resize(iTempBuf,	iSize)
				Array.Resize(iMvBuf,	iSize)
				Array.Resize(iTempBuf2,	iSize)
				Array.Resize(iMvBuf2,	iSize)
				Array.Resize(iSlope,	iSize)
				Array.Resize(dXPosPhase,iSize)
				Array.Resize(dYPosPhase,iSize)

				Array.Resize(dViscosity,iSize)
				Array.Resize(dSplStrain,iSize)
				Array.Resize(dShearMod,	iSize)
				Array.Resize(dShearVelo,iSize)
				Array.Resize(dXStrgMod,	iSize)
				Array.Resize(dXElasMod,	iSize)
				Array.Resize(dXLossTan,	iSize)
				Array.Resize(dYStrgMod,	iSize)
				Array.Resize(dYElasMod,	iSize)
				Array.Resize(dYLossTan,	iSize)


				Array.Copy(iOxBuf_s1,			iOxBuf,		iSize)
				Array.Copy(iOyBuf_s1,			iOyBuf,		iSize)
				Array.Copy(iOzBuf_s1,			iOzBuf,		iSize)
				Array.Copy(dOzBuf_s1,			dOzBuf,		iSize)
				Array.Copy(iPxBuf_s1,			iPxBuf,		iSize)
				Array.Copy(iPyBuf_s1,			iPyBuf,		iSize)
				Array.Copy(iPzBuf_s1,			iPzBuf,		iSize)
				Array.Copy(iHosei_s1,			iHosei,		iSize)
				Array.Copy(dHoseiP_s1,			dHosei100,	iSize)
				Array.Copy(iTempBuf_s1,			iTempBuf,	iSize)
				Array.Copy(iMvBuf_s1,			iMvBuf,		iSize)
				Array.Copy(iTempBuf2_s1,		iTempBuf2,	iSize)
				Array.Copy(iMvBuf2_s1,			iMvBuf2,	iSize)
				Array.Copy(iSlope_s1,			iSlope,		iSize)
				Array.Copy(dXPosPhase_s1,		dXPosPhase,	iSize)
				Array.Copy(dYPosPhase_s1,		dYPosPhase,	iSize)

				Array.Copy(dViscosity_s1,		dViscosity,	iSize)
				Array.Copy(dSplStrain_s1,		dSplStrain,	iSize)
				Array.Copy(dShearMod_s1,		dShearMod,	iSize)
				Array.Copy(dShearVelo_s1,		dShearVelo,	iSize)
				Array.Copy(dXStrgMod_s1,		dXStrgMod,	iSize)
				Array.Copy(dXElasMod_s1,		dXElasMod,	iSize)
				Array.Copy(dXLossTan_s1,		dXLossTan,	iSize)
			End If
		Case 2
			If Not IsNothing(iOxBuf_s2)
				iSize = iOxBuf_s2.Length
				Array.Resize(iOxBuf,	iSize)
				Array.Resize(iOyBuf,	iSize)
				Array.Resize(iOzBuf,	iSize)
				Array.Resize(dOzBuf,	iSize)
				Array.Resize(iPxBuf,	iSize)
				Array.Resize(iPyBuf,	iSize)
				Array.Resize(iPzBuf,	iSize)
				Array.Resize(iHosei,	iSize)
				Array.Resize(dHosei100,	iSize)
				Array.Resize(iTempBuf,	iSize)
				Array.Resize(iMvBuf,	iSize)
				Array.Resize(iTempBuf2,	iSize)
				Array.Resize(iMvBuf2,	iSize)
				Array.Resize(iSlope,	iSize)
				Array.Resize(dXPosPhase,iSize)
				Array.Resize(dYPosPhase,iSize)

				Array.Resize(dViscosity,iSize)
				Array.Resize(dSplStrain,iSize)
				Array.Resize(dShearMod,	iSize)
				Array.Resize(dShearVelo,iSize)
				Array.Resize(dXStrgMod,	iSize)
				Array.Resize(dXElasMod,	iSize)
				Array.Resize(dXLossTan,	iSize)
				Array.Resize(dYStrgMod,	iSize)
				Array.Resize(dYElasMod,	iSize)
				Array.Resize(dYLossTan,	iSize)


				Array.Copy(iOxBuf_s2,			iOxBuf,		iSize)
				Array.Copy(iOyBuf_s2,			iOyBuf,		iSize)
				Array.Copy(iOzBuf_s2,			iOzBuf,		iSize)
				Array.Copy(dOzBuf_s2,			dOzBuf,		iSize)
				Array.Copy(iPxBuf_s2,			iPxBuf,		iSize)
				Array.Copy(iPyBuf_s2,			iPyBuf,		iSize)
				Array.Copy(iPzBuf_s2,			iPzBuf,		iSize)
				Array.Copy(iHosei_s2,			iHosei,		iSize)
				Array.Copy(dHoseiP_s2,			dHosei100,	iSize)
				Array.Copy(iTempBuf_s2,			iTempBuf,	iSize)
				Array.Copy(iMvBuf_s2,			iMvBuf,		iSize)
				Array.Copy(iTempBuf2_s2,		iTempBuf2,	iSize)
				Array.Copy(iMvBuf2_s2,			iMvBuf2,	iSize)
				Array.Copy(iSlope_s2,			iSlope,		iSize)
				Array.Copy(dXPosPhase_s2,		dXPosPhase,	iSize)
				Array.Copy(dYPosPhase_s2,		dYPosPhase,	iSize)

				Array.Copy(dViscosity_s2,		dViscosity,	iSize)
				Array.Copy(dSplStrain_s2,		dSplStrain,	iSize)
				Array.Copy(dShearMod_s2,		dShearMod,	iSize)
				Array.Copy(dShearVelo_s2,		dShearVelo,	iSize)
				Array.Copy(dXStrgMod_s2,		dXStrgMod,	iSize)
				Array.Copy(dXElasMod_s2,		dXElasMod,	iSize)
				Array.Copy(dXLossTan_s2,		dXLossTan,	iSize)
			End If
		End Select
	End Select
	p_head = p_head_s(iSel-1)
End Sub
#end if
''' <summary>
''' デバッグ用ファイル出力 汎用
''' </summary>
''' <returns></returns>
Public Function DebugTextOut() As int32
	'デバッグ用ファイル出力
	Dim fs As New FileStream("C:\Test\" & p_sEquipName &  "_Debug"  & ".csv", FileMode.Append, FileAccess.Write, FileShare.ReadWrite)
	Dim sw As Textwriter = New StreamWriter(fs, Encoding.UTF8)
	Dim sLineBuf As String = nothing
Try
#If 1
	'テキストファイル(CSV)書込み
	'測定用AD
	'補正用AD
	'マイコン算出補正後AD
	'無負荷Z　AD
	sLineBuf = Now.ToString("yyyy/MM/dd HH:mm:ss.fff") & "," & _
				p_RTM(0).p_mltcp.HeatBPos & "," & _
				p_RTM(0).p_mltcp.Hp_dmv & "," & _
				p_RTM(0).p_mltcp.Hp_pn & "," & _
				p_RTM(0).p_mltcp.Hp_in & "," & _
				p_RTM(0).p_mltcp.Hp_dn & "," & _
				p_RTM(0).p_mltcp.HeatTempAct & "," & _
				p_RTM(0).p_mltcp.HeatTempOpe & "," & _
				p_RTM(0).p_mltcp.pb_dmv & "," & _
				p_RTM(0).p_mltcp.pb_pn & "," & _
				p_RTM(0).p_mltcp.pb_in & "," & _
				p_RTM(0).p_mltcp.pb_dn & "," & _
				p_RTM(0).p_mltcp.HeatTemp2Act & "," & _
				p_RTM(0).p_mltcp.HeatTemp2Ope & "," & _
				p_RTM(0).p_mltcp.Sg_dmv & "," & _
				p_RTM(0).p_mltcp.Sg_pn & "," & _
				p_RTM(0).p_mltcp.Sg_in & "," & _
				p_RTM(0).p_mltcp.Sg_dn & "," & _
				p_RTM(0).p_mltcp.HeatTemp3Act & "," & _
				p_RTM(0).p_mltcp.HeatTemp3Ope & "," & _
				p_RTM(0).p_mltcp.CurrStrsX & "," & _
				p_RTM(0).p_mltcp.CurrStrsY & "," & _
				p_RTM(0).p_mltcp.CurrStrsZCorrA & "," & _
				p_RTM(0).p_mltcp.ZeroStrs_Z & "," & _
				frmMain.dZStability


	sw.WriteLine(sLineBuf)
	sw.Close()
	sw = nothing
#else
	'テキストファイル(CSV)書込み
	'現在温度
	'MV
	'[1194]起歪体 PID制御量 dmv
	'[1194]起歪体 PID制御量 pn
	'[1194]起歪体 PID制御量 in
	'[1194]起歪体 PID制御量 dn
	sLineBuf = Now.ToString("yyyy/MM/dd HH:mm:ss.fff") & "," & _
				p_RTM(0).p_mltcp.HeatTemp3Act & "," & _
				p_RTM(0).p_mltcp.HeatTemp3Ope & "," & _
				p_RTM(0).p_mltcp.Sg_dmv & "," & _
				p_RTM(0).p_mltcp.Sg_pn & "," & _
				p_RTM(0).p_mltcp.Sg_in & "," & _
				p_RTM(0).p_mltcp.Sg_dn 


	sw.WriteLine(sLineBuf)
	sw.Close()
	sw = nothing



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


Dim dx(0 To 49)		As Double						'直線近似用 X配列50点
Dim dy(0 To 49)		As Double						'直線近似用 Y配列50点
Dim da(0 To 99)		As Double						'直線傾き配列 100点
Dim dc(0 To 99)		As Double						'直線Y切配列 100点
Dim iCnta			As Int32 = 0					'サンプルカウントa  規定配列分までのカウント
Dim iCntaa			As Int32 = 0					'サンプルカウントaa 規定配列分までのカウント
Dim dAAA			As Double						'係数A
Dim dCCC			As Double						'係数C
Public	dAwarp		As Double
Public	dCwarp		As Double
''' <summary>
''' Z補正用 A係数計算処理
'''   タイマーインターバル200ｍS毎に過去50点の1179(X),1153(Y)として直線近似->a
'''   aが100点溜まったら平均計算->A
''' </summary>
''' <returns></returns>
Public Function ACoefficientCalculation(ByVal dInp1 As Double, ByVal dInp2 As Double, ByRef dAA As Double, ByRef dCC As Double, ByRef dAwarp As Double, ByRef dCwarp As Double) As Int32
Dim nX()		As Double = Nothing
Dim nY()		As Double = Nothing
Dim dPara()		As Double = Nothing
Try
	dAA = Double.NaN
	If iCnta > dx.GetUpperBound(0)
	'直線近似用配列溜まったら直線近似でaを算出
		Array.Copy(dx, 1, dx, 0, dx.Length-1)			'先頭を廃棄
		Array.Copy(dy, 1, dy, 0, dy.Length-1)			'先頭を廃棄
		dx(iCnta-1) = dInp1								'新データをセット
		dy(iCnta-1) = dInp2								'新データをセット

		dPara = datCal.fnLeastSquaresMethod(dx, dY, nX, nY, dx.Length, 1)		'最小二乗法 一次式
		If iCntaa > da.GetUpperBound(0)
		'直線傾き配列が溜まったら平均
			Array.Copy(da, 1, da, 0, da.Length-1)			'先頭を廃棄
			Array.Copy(dc, 1, dc, 0, dc.Length-1)			'先頭を廃棄
			da(iCntaa-1) = dPara(1)							'新データをセット
			dc(iCntaa-1) = dPara(0)							'新データをセット
			dAA = da.Average()								'平均
			dCC = dc.Average()								'平均
			dAAA = daa
			dCCC = dcc
			dAwarp = dPara(1)
			dCwarp = dPara(0)
		Else
		'必要カウント溜まるまで
			da(iCntaa) = dPara(1)		'傾きをセット
			iCntaa += 1
		End if
	Else
	'必要カウント溜まるまで
		dx(iCnta) = dInp1
		dy(iCnta) = dInp2
		iCnta += 1
	End If

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
''' 長期間欠モード測定データ領域拡張
''' </summary>
''' <param name="iArea">記憶領域</param>
''' <param name="iSize">配列数</param>
Public Sub LTDataArrayResize(ByVal iArea As int16, ByVal iSize As Int32)
Try
	Select Case iArea
	Case 0
		Array.Resize(iLT_MSec,		iSize)
		Array.Resize(iLT_TempBuf,	iSize)
		Array.Resize(iLT_TempBuf2,	iSize)
		Array.Resize(iLT_MvBuf,		iSize)
		Array.Resize(iLT_MvBuf2,	iSize)
		Array.Resize(iLT_Hosei,		iSize)
		Array.Resize(dLT_Hosei100,	iSize)
		Array.Resize(dLT_XPosPhase,	iSize)
		Array.Resize(dLT_YPosPhase,	iSize)
		Array.Resize(dLT_Viscosity,	iSize)
		Array.Resize(dLT_SplStrain,	iSize)
		Array.Resize(dLT_ShearMod,	iSize)
		Array.Resize(dLT_ShearVelo,	iSize)
		Array.Resize(dLT_XStrgMod,	iSize)
		Array.Resize(dLT_XElasMod,	iSize)
		Array.Resize(dLT_XLossTan,	iSize)
	Case 1
		Array.Resize(iMSec_s1,		iSize)
		Array.Resize(iTempBuf_s1,	iSize)
		Array.Resize(iTempBuf2_s1,	iSize)
		Array.Resize(iMvBuf_s1,		iSize)
		Array.Resize(iMvBuf2_s1,	iSize)
		Array.Resize(iHosei_s1,		iSize)
		Array.Resize(dHoseiP_s1,	iSize)
		Array.Resize(dXPosPhase_s1,	iSize)
		Array.Resize(dYPosPhase_s1,	iSize)
		Array.Resize(dViscosity_s1,	iSize)
		Array.Resize(dSplStrain_s1,	iSize)
		Array.Resize(dShearMod_s1,	iSize)
		Array.Resize(dShearVelo_s1,	iSize)
		Array.Resize(dXStrgMod_s1,	iSize)
		Array.Resize(dXElasMod_s1,	iSize)
		Array.Resize(dXLossTan_s1,	iSize)
	Case 2
		Array.Resize(iMSec_s2,		iSize)
		Array.Resize(iTempBuf_s2,	iSize)
		Array.Resize(iTempBuf2_s2,	iSize)
		Array.Resize(iMvBuf_s2,		iSize)
		Array.Resize(iMvBuf2_s2,	iSize)
		Array.Resize(iHosei_s2,		iSize)
		Array.Resize(dHoseiP_s2,	iSize)
		Array.Resize(dXPosPhase_s2,	iSize)
		Array.Resize(dYPosPhase_s2,	iSize)
		Array.Resize(dViscosity_s2,	iSize)
		Array.Resize(dSplStrain_s2,	iSize)
		Array.Resize(dShearMod_s2,	iSize)
		Array.Resize(dShearVelo_s2,	iSize)
		Array.Resize(dXStrgMod_s2,	iSize)
		Array.Resize(dXElasMod_s2,	iSize)
		Array.Resize(dXLossTan_s2,	iSize)
	End Select
Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
End Try
End Sub




End Class
