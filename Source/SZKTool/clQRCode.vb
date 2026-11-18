Imports System.Net.NetworkInformation
Imports QRCoder

Public Class clQRCode
Private qrImage As Bitmap

Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
	Me.Text = Application.ProductName
    cmbECC.Items.AddRange(New String() {"L", "M", "Q", "H"})
    cmbECC.SelectedItem = "H"
    numLogo.Value = 9
End Sub

Private Sub btnGenerate_Click(sender As Object, e As EventArgs) Handles btnGenerate.Click
Try
    Dim qrText As String = txtText.Text.Trim()
    If qrText = "" Then
        MessageBox.Show("QRコードにする文字列を入力してください。")
        Return
    End If

    Dim logoPath As String = ""
    Using ofd As New OpenFileDialog()
        ofd.Title = "ロゴ画像を選択"
        ofd.Filter = "画像ファイル|*.png;*.jpg;*.jpeg;*.bmp"
        If ofd.ShowDialog() = DialogResult.OK Then
            logoPath = ofd.FileName
        Else
            Return
        End If
    End Using

    Dim logoPercent As Integer = CInt(numLogo.Value)
    Dim eccLevelChar As String = cmbECC.SelectedItem.ToString()
    Dim eccLevel As QRCodeGenerator.ECCLevel = [Enum].Parse(GetType(QRCodeGenerator.ECCLevel), eccLevelChar)

	'レベル40までセル境界に収まる様にロゴに占めるセル数でロゴを配置しデコードテストを行い成功したものを採用する
	Dim success As Boolean = False
	Dim versionUsed As Integer = 0
	Dim qrData As QRCodeData = Nothing
	For version As Integer = 0 To 40
		Try
			Dim qrGenerator As New QRCodeGenerator()
			' version=0 の場合は自動選択、それ以外は強制指定
			If version = 0 Then
				qrData = qrGenerator.CreateQrCode(qrText, eccLevel)
			Else
				qrData = qrGenerator.CreateQrCode(qrText, eccLevel, True, False, QRCodeGenerator.EciMode.Default, version)
			End If

			' qrData が生成されなかった場合は次へ
			If qrData Is Nothing OrElse qrData.ModuleMatrix Is Nothing Then Continue For

			Dim qrCode As New QRCode(qrData)
			Dim baseQR As Bitmap = qrCode.GetGraphic(20, Color.Black, Color.White, True)

		    ' ロゴ合成
#If 0
    Dim logo As Bitmap = CType(Image.FromFile(logoPath), Bitmap)
    qrImage = New Bitmap(baseQR.Width, baseQR.Height)
    Using g As Graphics = Graphics.FromImage(qrImage)
        g.DrawImage(baseQR, 0, 0, baseQR.Width, baseQR.Height)
        Dim logoSize As Integer = baseQR.Width * logoPercent \ 100
        Dim logoX As Integer = (baseQR.Width - logoSize) \ 2
        Dim logoY As Integer = (baseQR.Height - logoSize) \ 2
        g.DrawImage(logo, New Rectangle(logoX, logoY, logoSize, logoSize))
    End Using
#else
			' === QR生成済み（qrData, baseQRあり） ===
			Dim cellCount As Integer = qrData.ModuleMatrix.Count		' セル数
			Dim cellSize As Integer = baseQR.Width / cellCount			' 1セル当たりピクセル数
			Dim logoCellCount As Integer = numLogo.Value				' ロゴを占めるセル数（例）
			Dim logoPixelSize As Integer = cellSize * logoCellCount

			Dim logo As Bitmap = CType(Image.FromFile(logoPath), Bitmap)
			qrImage = New Bitmap(baseQR.Width, baseQR.Height)

			Using g As Graphics = Graphics.FromImage(qrImage)
				g.DrawImage(baseQR, 0, 0, baseQR.Width, baseQR.Height)
				' 中央セル境界に合わせてロゴを配置
				Dim logoX As Integer = (baseQR.Width - logoPixelSize) \ 2
				Dim logoY As Integer = (baseQR.Height - logoPixelSize) \ 2
				g.DrawImage(logo, New Rectangle(logoX, logoY, logoPixelSize, logoPixelSize))
			End Using

#end if

			' デコードテスト
			Dim reader As New ZXing.BarcodeReader()
			Dim result = reader.Decode(qrImage)

			If result IsNot Nothing AndAlso result.Text = qrText Then
				success = True
				versionUsed = qrData.Version
				Exit For
			End If

		Catch ex As Exception
			' 生成に失敗した場合は次のバージョンへ進む
			Continue For
		End Try
	Next

	picQR.Image = qrImage

	If success AndAlso qrData IsNot Nothing Then
		Dim matrixSize As Integer = qrData.ModuleMatrix.Count
		lblInfo.Text =
			$"読み取り成功！" & vbCrLf &
			$"・使用バージョン：{versionUsed}" & vbCrLf &
			$"・セル数：{matrixSize}×{matrixSize}" & vbCrLf &
			$"・誤り訂正レベル：{eccLevel}" & vbCrLf &
			$"・ロゴサイズ：{logoPercent}%"
	Else
		lblInfo.Text = "※読み取りできませんでした（最大バージョンまで試行）"
	End If

Catch ex As Exception
    MessageBox.Show("エラー: " & ex.Message)
End Try
End Sub

Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
    If qrImage Is Nothing Then
        MessageBox.Show("QRコードを生成してください。")
        Return
    End If

    Using sfd As New SaveFileDialog()
        sfd.Title = "QRコード保存"
        sfd.Filter = "PNGファイル|*.png"
        sfd.FileName = "QR_with_logo.png"
        If sfd.ShowDialog() = DialogResult.OK Then
            qrImage.Save(sfd.FileName, Imaging.ImageFormat.Png)
            MessageBox.Show("保存しました: " & sfd.FileName)
        End If
    End Using
End Sub

End Class
