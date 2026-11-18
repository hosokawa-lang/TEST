Imports System.Drawing
Imports System.IO
Imports MathNet.Numerics.LinearAlgebra.Factorization
Imports QRCoder
Imports ZXing
Imports Microsoft.VisualBasic.FileIO

'QRコード生成制御クラス
Public Class clQRCodeControl
Public	p_qrText			As String			'QRコードかするテキスト
Public	p_qrImage			As Bitmap			'QRコードイメージ
Public	p_qrImagePath		As String			'QRコードイメージ出力パス
Public	p_picQR				As PictureBox		'作成されたQRコードピクチャボックス
Private	m_PxelInCell		As Int32			'1セル当たりのピクセル数
Private m_ECC				As String			'品質 L,M,Q,H
Private m_LogoVal			As Int32			'ロゴに占めるセル数
Private m_LogoPAth			As String			'ロゴイメージパス

'QRコード状態遷移
Public enum eQRState
	Preparing	= -1		'準備中
	Receipting	= 0			'受付中
	Complete	= 1			'受付完了
End enum

''' <summary>
''' コンストラクタ
''' </summary>
''' <param name="sQ">品質</param>
''' <param name="LogoVal">ロゴに占めるセル数</param>
''' <param name="sLogoPath">ロゴイメージパス</param>
''' <param name="iPxelInCell">1セル当たりのピクセル数</param>
Public Sub New(ByVal sQ As String, ByVal LogoVal As  Int32, ByVal sLogoPath As String, ByVal iPxelInCell As Int32)
	m_ECC			= sQ				'品質
	m_LogoVal		= LogoVal			'ロゴに占めるセル数
	m_LogoPAth		= sLogoPath			'ロゴ参照パス
	m_PxelInCell	= iPxelInCell		'1セル当たりのピクセル数
	'QRコードピクチャボックス初期化
	p_picQR = New PictureBox With
	{
		.SizeMode	= PictureBoxSizeMode.Zoom,
		.Margin		= New Padding(3,4,3,4),
		.BackColor	= Color.White,
		.Size		= New Size(320,320)
	}
	'QRコードデフォルト出力先を準備
	If Not FileSystem.DirectoryExists(Path.Combine(SIZUKA_PATH, QRCODE_PATH))
		FileSystem.CreateDirectory(Path.Combine(SIZUKA_PATH, QRCODE_PATH))
	End If
End Sub
''' <summary>
''' ロゴ付きQRコードイメージを生成してイメージ作成
''' </summary>
''' <returns></returns>
Public Function GenerateQR() As Int32
    Dim logoPercent		As Integer = m_LogoVal
    Dim eccLevelChar	As String = m_ECC
    Dim eccLevel		As QRCodeGenerator.ECCLevel = [Enum].Parse(GetType(QRCodeGenerator.ECCLevel), eccLevelChar)
	Dim success			As Boolean = False
	Dim versionUsed		As Integer = 0
	Dim qrData			As QRCodeData = Nothing
	Dim iRet			As Int32 = 0
Try
	'レベル40までアップさせながら指定のロゴ占有セル数で境界調整して配置しデコード出来る設定を採用する
	For version As Integer = 0 To 40
		Try
			Dim qrGenerator As New QRCodeGenerator()
			' version=0 の場合は自動選択、それ以外は強制指定
			If version = 0 Then
				qrData = qrGenerator.CreateQrCode(p_qrText, eccLevel)
			Else
				qrData = qrGenerator.CreateQrCode(p_qrText, eccLevel, True, False, QRCodeGenerator.EciMode.Default, version)
			End If

			' qrData が生成されなかった場合は次へ
			If qrData Is Nothing OrElse qrData.ModuleMatrix Is Nothing Then Continue For

			Dim qrCode As New QRCode(qrData)
			Dim baseQR As Bitmap = qrCode.GetGraphic(m_PxelInCell, Color.Black, Color.White, True)

			' ロゴ合成
			' === QR生成済み（qrData, baseQRあり） ===

			Dim cellCount As Integer = qrData.ModuleMatrix.Count		' セル数
			Dim cellSize As Integer = baseQR.Width \ cellCount			' 1セル当たりピクセル数
			Dim logoCellCount As Integer = m_LogoVal					' ロゴを占めるセル数
			Dim logoPixelSize As Integer = cellSize * logoCellCount

			Dim logo As Bitmap = CType(Image.FromFile(m_LogoPAth), Bitmap)
			p_qrImage = New Bitmap(baseQR.Width, baseQR.Height)

			Using g As Graphics = Graphics.FromImage(p_qrImage)
				g.DrawImage(baseQR, 0, 0, baseQR.Width, baseQR.Height)

				' 中央セル境界に合わせてロゴを配置
				Dim logoX As Integer = (baseQR.Width - logoPixelSize) \ 2
				Dim logoY As Integer = (baseQR.Height - logoPixelSize) \ 2
				g.DrawImage(logo, New Rectangle(logoX, logoY, logoPixelSize, logoPixelSize))
			End Using

			' デコードテスト
			Dim reader As New ZXing.BarcodeReader()
			Dim result = reader.Decode(p_qrImage)

			If result IsNot Nothing AndAlso result.Text = p_qrText Then
				success = True
				versionUsed = qrData.Version
				Exit For
			End If
		Catch ex As Exception
			' 生成に失敗した場合は次のバージョンへ進む
			Continue For
		End Try
	Next

	p_picQR.Image = p_qrImage

	Dim sText As String = Nothing
	If success AndAlso qrData IsNot Nothing Then
		Dim matrixSize As Integer = qrData.ModuleMatrix.Count
		sText =
			$"・使用バージョン：{versionUsed}" & vbCrLf &
			$"・セル数：{matrixSize}×{matrixSize}" & vbCrLf &
			$"・誤り訂正レベル：{eccLevel}" & vbCrLf &
			$"・占有セル数：{logoPercent}"
		''生成したQRコードをファイルへ保存
		'p_qrImage.Save(p_qrImagePath, Imaging.ImageFormat.Png)
		'Call WriteLog("QRコード生成完了 " & vbCrLf & sText, eLOG_LEVEL.iINFO)
		iRet = 0
	Else
		sText = "※読み取りできませんでした" & vbcrlf & "（最大バージョンまで試行）"
		iRet = -2
	End If
	Console.WriteLine(sText)
	Return iRet
Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Return -1
End Try
End Function
''' <summary>
''' QRコードイメージの名前を付けて保存
''' </summary>
''' <returns></returns>
Public Function SaveImageAsNamed() As Int32
Try
    Using sfd As New SaveFileDialog()
        sfd.Title = "QRコード保存"
        sfd.Filter = "PNGファイル|*.png"
        sfd.FileName = Path.GetFileName(p_qrImagePath)
		sfd.InitialDirectory = Path.GetDirectoryName(p_qrImagePath)
        If sfd.ShowDialog() = DialogResult.OK Then
            p_picQR.Image.Save(sfd.FileName, Imaging.ImageFormat.Png)
            MessageBox.Show("保存しました: " & sfd.FileName)
        End If
    End Using
	Return 0
Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Return -1
End Try
End Function
''' <summary>
''' ピクチャボックス p_picQR のデコードを行う
''' </summary>
''' <param name="sText">デコードテキスト</param>
''' <returns></returns>
Public Function Decord(ByRef sText As String) As Int32
Try
	Dim reader As New ZXing.BarcodeReader()
	Dim result = reader.Decode(p_picQR.Image)
	If Not IsNothing(result)
		sText = result.Text
	Else
		Return -2
	End If
	Return 0
Catch ex As Exception
	Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
	Return -1
End Try
End Function

End Class
