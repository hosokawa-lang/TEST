<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmLightPrint
    Inherits System.Windows.Forms.Form

    'フォームがコンポーネントの一覧をクリーンアップするために dispose をオーバーライドします。
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Windows フォーム デザイナーで必要です。
    Private components As System.ComponentModel.IContainer

    'メモ: 以下のプロシージャは Windows フォーム デザイナーで必要です。
    'Windows フォーム デザイナーを使用して変更できます。  
    'コード エディターを使って変更しないでください。
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
		Me.Label1 = New System.Windows.Forms.Label()
		Me.cmbPrinter = New System.Windows.Forms.ComboBox()
		Me.Label2 = New System.Windows.Forms.Label()
		Me.txtMemo = New System.Windows.Forms.TextBox()
		Me.btnExec = New System.Windows.Forms.Button()
		Me.Label3 = New System.Windows.Forms.Label()
		Me.cmbPaper = New System.Windows.Forms.ComboBox()
		Me.btnPreview = New System.Windows.Forms.Button()
		Me.gbxCategSel = New System.Windows.Forms.GroupBox()
		Me.ckbCate4 = New System.Windows.Forms.CheckBox()
		Me.ckbCate3 = New System.Windows.Forms.CheckBox()
		Me.ckbCate2 = New System.Windows.Forms.CheckBox()
		Me.ckbCate1 = New System.Windows.Forms.CheckBox()
		Me.gbxCategSel.SuspendLayout
		Me.SuspendLayout
		'
		'Label1
		'
		Me.Label1.AutoSize = true
		Me.Label1.Font = New System.Drawing.Font("Meiryo UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.Label1.Location = New System.Drawing.Point(13, 17)
		Me.Label1.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
		Me.Label1.Name = "Label1"
		Me.Label1.Size = New System.Drawing.Size(92, 17)
		Me.Label1.TabIndex = 0
		Me.Label1.Text = "Printer Name"
		'
		'cmbPrinter
		'
		Me.cmbPrinter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.cmbPrinter.FormattingEnabled = true
		Me.cmbPrinter.Location = New System.Drawing.Point(13, 37)
		Me.cmbPrinter.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
		Me.cmbPrinter.Name = "cmbPrinter"
		Me.cmbPrinter.Size = New System.Drawing.Size(315, 25)
		Me.cmbPrinter.Sorted = true
		Me.cmbPrinter.TabIndex = 1
		'
		'Label2
		'
		Me.Label2.AutoSize = true
		Me.Label2.Font = New System.Drawing.Font("Meiryo UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.Label2.Location = New System.Drawing.Point(13, 82)
		Me.Label2.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
		Me.Label2.Name = "Label2"
		Me.Label2.Size = New System.Drawing.Size(47, 17)
		Me.Label2.TabIndex = 2
		Me.Label2.Text = "Memo"
		'
		'txtMemo
		'
		Me.txtMemo.Location = New System.Drawing.Point(13, 102)
		Me.txtMemo.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
		Me.txtMemo.Name = "txtMemo"
		Me.txtMemo.Size = New System.Drawing.Size(499, 24)
		Me.txtMemo.TabIndex = 3
		'
		'btnExec
		'
		Me.btnExec.Font = New System.Drawing.Font("Meiryo UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.btnExec.Location = New System.Drawing.Point(379, 214)
		Me.btnExec.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
		Me.btnExec.Name = "btnExec"
		Me.btnExec.Size = New System.Drawing.Size(135, 40)
		Me.btnExec.TabIndex = 4
		Me.btnExec.Text = "PRINT"
		Me.btnExec.UseVisualStyleBackColor = true
		'
		'Label3
		'
		Me.Label3.AutoSize = true
		Me.Label3.Font = New System.Drawing.Font("Meiryo UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.Label3.Location = New System.Drawing.Point(336, 17)
		Me.Label3.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
		Me.Label3.Name = "Label3"
		Me.Label3.Size = New System.Drawing.Size(75, 17)
		Me.Label3.TabIndex = 5
		Me.Label3.Text = "Paper Size"
		'
		'cmbPaper
		'
		Me.cmbPaper.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.cmbPaper.FormattingEnabled = true
		Me.cmbPaper.Location = New System.Drawing.Point(337, 37)
		Me.cmbPaper.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
		Me.cmbPaper.Name = "cmbPaper"
		Me.cmbPaper.Size = New System.Drawing.Size(172, 25)
		Me.cmbPaper.TabIndex = 6
		'
		'btnPreview
		'
		Me.btnPreview.Font = New System.Drawing.Font("Meiryo UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.btnPreview.Location = New System.Drawing.Point(236, 214)
		Me.btnPreview.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
		Me.btnPreview.Name = "btnPreview"
		Me.btnPreview.Size = New System.Drawing.Size(135, 40)
		Me.btnPreview.TabIndex = 7
		Me.btnPreview.Text = "PREVIEW"
		Me.btnPreview.UseVisualStyleBackColor = true
		'
		'gbxCategSel
		'
		Me.gbxCategSel.Controls.Add(Me.ckbCate4)
		Me.gbxCategSel.Controls.Add(Me.ckbCate3)
		Me.gbxCategSel.Controls.Add(Me.ckbCate2)
		Me.gbxCategSel.Controls.Add(Me.ckbCate1)
		Me.gbxCategSel.Location = New System.Drawing.Point(13, 139)
		Me.gbxCategSel.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
		Me.gbxCategSel.Name = "gbxCategSel"
		Me.gbxCategSel.Padding = New System.Windows.Forms.Padding(4, 4, 4, 4)
		Me.gbxCategSel.Size = New System.Drawing.Size(500, 67)
		Me.gbxCategSel.TabIndex = 8
		Me.gbxCategSel.TabStop = false
		'
		'ckbCate4
		'
		Me.ckbCate4.AutoSize = true
		Me.ckbCate4.Location = New System.Drawing.Point(365, 25)
		Me.ckbCate4.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
		Me.ckbCate4.Name = "ckbCate4"
		Me.ckbCate4.Size = New System.Drawing.Size(97, 21)
		Me.ckbCate4.TabIndex = 1
		Me.ckbCate4.Text = "Category 4"
		Me.ckbCate4.UseVisualStyleBackColor = true
		'
		'ckbCate3
		'
		Me.ckbCate3.AutoSize = true
		Me.ckbCate3.Location = New System.Drawing.Point(249, 25)
		Me.ckbCate3.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
		Me.ckbCate3.Name = "ckbCate3"
		Me.ckbCate3.Size = New System.Drawing.Size(97, 21)
		Me.ckbCate3.TabIndex = 0
		Me.ckbCate3.Text = "Category 3"
		Me.ckbCate3.UseVisualStyleBackColor = true
		'
		'ckbCate2
		'
		Me.ckbCate2.AutoSize = true
		Me.ckbCate2.Location = New System.Drawing.Point(135, 25)
		Me.ckbCate2.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
		Me.ckbCate2.Name = "ckbCate2"
		Me.ckbCate2.Size = New System.Drawing.Size(97, 21)
		Me.ckbCate2.TabIndex = 0
		Me.ckbCate2.Text = "Category 2"
		Me.ckbCate2.UseVisualStyleBackColor = true
		'
		'ckbCate1
		'
		Me.ckbCate1.AutoSize = true
		Me.ckbCate1.Location = New System.Drawing.Point(20, 25)
		Me.ckbCate1.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
		Me.ckbCate1.Name = "ckbCate1"
		Me.ckbCate1.Size = New System.Drawing.Size(97, 21)
		Me.ckbCate1.TabIndex = 0
		Me.ckbCate1.Text = "Category 1"
		Me.ckbCate1.UseVisualStyleBackColor = true
		'
		'frmLightPrint
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(8!, 17!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(529, 266)
		Me.Controls.Add(Me.gbxCategSel)
		Me.Controls.Add(Me.btnPreview)
		Me.Controls.Add(Me.cmbPaper)
		Me.Controls.Add(Me.Label3)
		Me.Controls.Add(Me.btnExec)
		Me.Controls.Add(Me.txtMemo)
		Me.Controls.Add(Me.Label2)
		Me.Controls.Add(Me.cmbPrinter)
		Me.Controls.Add(Me.Label1)
		Me.Font = New System.Drawing.Font("Meiryo UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
		Me.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
		Me.MaximizeBox = false
		Me.MinimizeBox = false
		Me.Name = "frmLightPrint"
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
		Me.Text = "Light Print"
		Me.gbxCategSel.ResumeLayout(false)
		Me.gbxCategSel.PerformLayout
		Me.ResumeLayout(false)
		Me.PerformLayout

End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents cmbPrinter As System.Windows.Forms.ComboBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txtMemo As System.Windows.Forms.TextBox
    Friend WithEvents btnExec As System.Windows.Forms.Button
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents cmbPaper As System.Windows.Forms.ComboBox
    Friend WithEvents btnPreview As System.Windows.Forms.Button
	Friend WithEvents gbxCategSel As GroupBox
	Friend WithEvents ckbCate4 As CheckBox
	Friend WithEvents ckbCate3 As CheckBox
	Friend WithEvents ckbCate2 As CheckBox
	Friend WithEvents ckbCate1 As CheckBox
End Class
