Public Class frmLightPrint
'▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼
'   定数
'▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲

'▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼
'   構造体
'▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲

'▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼
'   パブリック変数
'▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲

'▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼
'   プライベート変数
'▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲

Private m_psSize() As System.Drawing.Printing.PaperSize             '用紙サイズ選択用配列
'多言語メッセージ格納
Private m_sStr(eMes.iMAX - 1)		As String   
Enum eMes
	iMAX
End Enum


'■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
'   フォームのロード／アンロード／アクティブ
'■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■

'================================================
'フォームLoad
'================================================
Private Sub frmLightPrint_Load(sender As Object, e As EventArgs) Handles MyBase.Load
	Dim iCnt As Integer

	Try
        '言語により表示を変更
        Call SetFormLanguage(Me, p_iLanguage)

		btnExec.Enabled		= False
		btnPreview.Enabled	= False
        'プリンタ選択コンボボックスを設定
		cmbPrinter.Items.Clear()
		For iCnt = 0 To Printing.PrinterSettings.InstalledPrinters.Count - 1
			cmbPrinter.Items.Add(Printing.PrinterSettings.InstalledPrinters.Item(iCnt))
		Next
		'選択されたことがない場合、初期値にデフォルトを指定
		If p_sPrinterName.Length = 0 Then
			Dim pd As New System.Drawing.Printing.PrintDocument
			cmbPrinter.Text = pd.PrinterSettings.PrinterName

			'用紙サイズA3に固定する A3サイズを探して設定 ----------------
			For i As Int32 = 0 to pd.PrinterSettings.PaperSizes.Count - 1
				If pd.PrinterSettings.PaperSizes.Item(i).papername.Contains("A3")
					cmbPaper.Text	= pd.PrinterSettings.PaperSizes.Item(i).PaperName
					btnExec.Enabled		= True
					btnPreview.Enabled	= True
					Exit For
				End If
			Next
			’------------------------------------------
		Else
			'前回選択プリンタを設定
			cmbPrinter.Text = p_sPrinterName
			btnExec.Enabled		= True
			btnPreview.Enabled	= True
		End If

		txtMemo.Text = p_sLtPrintMemo

		ckbCate1.Checked = True
		ckbCate2.Checked = True
		ckbCate3.Checked = True
		ckbCate4.Checked = True

		cmbPaper.Enabled = False

    Catch ex As Exception
        Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
        Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
    End Try
End Sub

'■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
'   ボタンイベント
'■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■

'================================================
'[PRINT]ボタンクリックイベント
'================================================
Private Sub btnExec_Click(sender As Object, e As EventArgs) Handles btnExec.Click, btnPreview.Click
    Try
		If Not ckbCate1.Checked And Not ckbCate2.Checked And Not ckbCate3.Checked And Not ckbCate4.Checked
			Exit Sub
		End If
        'プリンタ名、メモ、用紙サイズの取得
        p_sPrinterName	= cmbPrinter.Text
        p_sLtPrintMemo	= txtMemo.Text
        p_sPaperSize	= cmbPaper.Text

        With frmMain.p_pdocPrint
            'ドキュメント名の設定
            .DocumentName = "SizukaTool_" & Format(Now, "yyyyMMddHHmm")
            'プリンタ、用紙サイズの設定
            .PrinterSettings.PrinterName = p_sPrinterName
            .DefaultPageSettings.PaperSize = m_psSize(cmbPaper.SelectedIndex)
                
            Try
                'ページ設定
                With .DefaultPageSettings
                    '用紙の向き
                    '.Landscape = False      '縦
                    .Landscape = True       '横

                    '余白
                    With .Margins
                        .Top	= 0
                        .Bottom = 0
                        .Left	= 0
                        .Right	= 0
                    End With
                End With

				'印刷カテゴリ選択
				p_bReportCat(0) = ckbCate1.Checked
				p_bReportCat(1) = ckbCate2.Checked
				p_bReportCat(2) = ckbCate3.Checked
				p_bReportCat(3) = ckbCate4.Checked

                'プレビューボタンが押された
                If sender.Equals(btnPreview) = True Then

					Dim ppd As New PrintPreviewDialog
					ppd.StartPosition = FormStartPosition.CenterScreen
					ppd.Width		= 700
					ppd.Height		= 500
					ppd.Document	= frmMain.p_pdocPrint

					'印刷プレビューダイアログを表示する
					ppd.ShowDialog()
                Else
                    '印刷実行
                    .Print()
                End If

            Catch ex As System.ComponentModel.Win32Exception
                Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)

            Catch ex As Exception
                Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)

            End Try
        End With

        Me.Close()

    Catch ex As Exception
        Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
        Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
    End Try
End Sub

'■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
'   コントロールイベント
'■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■

'================================================
'プリンタ選択コンボボックス変更イベント
'================================================
Private Sub cmbPrinter_TextChanged(sender As Object, e As EventArgs) Handles cmbPrinter.TextChanged
    Dim iCnt As Integer

    Try
		btnExec.Enabled		= False
		btnPreview.Enabled	= False

        With frmMain.p_pdocPrint.PrinterSettings
            '選択されたプリンタを設定
            .PrinterName = cmbPrinter.Text

            '用紙選択コンボボックスをクリア
            cmbPaper.Items.Clear()

            'プリンタの用紙の選択設定
            '配列拡張
            ReDim m_psSize(.PaperSizes.Count - 1)

            '配列コンボボックスに追加
            For iCnt = 0 To m_psSize.Count - 1
                m_psSize(iCnt) = .PaperSizes(iCnt)
                cmbPaper.Items.Add(m_psSize(iCnt).PaperName)
            Next

			'初期値を選択
			cmbPaper.Text = p_sPaperSize
		
			'用紙サイズA3に固定する A3サイズを探して設定 ----------------
			For i As Int32 = 0 to .PaperSizes.Count - 1
				If .PaperSizes.Item(i).papername.Contains("A3")
					cmbPaper.Text	= .PaperSizes.Item(i).PaperName
					btnExec.Enabled		= True
					btnPreview.Enabled	= True
					Exit For
				End If
			Next
			’------------------------------------------
        End With
    Catch ex As Exception
        Call WriteLog(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
        Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
    End Try
End Sub

'■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
'   パブリック関数
'■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■

'■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
'   プライベート関数
'■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■

End Class