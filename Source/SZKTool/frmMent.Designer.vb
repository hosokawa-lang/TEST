<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMent
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
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMent))
		Me.cboDateList = New System.Windows.Forms.ComboBox()
		Me.lblDate = New System.Windows.Forms.Label()
		Me.TabPage3 = New System.Windows.Forms.TabPage()
		Me.txtMnt = New System.Windows.Forms.TextBox()
		Me.TabPage2 = New System.Windows.Forms.TabPage()
		Me.txtVer = New System.Windows.Forms.TextBox()
		Me.TabPage1 = New System.Windows.Forms.TabPage()
		Me.txtPC = New System.Windows.Forms.TextBox()
		Me.TabControl1 = New System.Windows.Forms.TabControl()
		Me.TabPage3.SuspendLayout
		Me.TabPage2.SuspendLayout
		Me.TabPage1.SuspendLayout
		Me.TabControl1.SuspendLayout
		Me.SuspendLayout
		'
		'cboDateList
		'
		Me.cboDateList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.cboDateList.FormattingEnabled = true
		Me.cboDateList.Location = New System.Drawing.Point(122, 13)
		Me.cboDateList.Name = "cboDateList"
		Me.cboDateList.Size = New System.Drawing.Size(287, 25)
		Me.cboDateList.TabIndex = 0
		'
		'lblDate
		'
		Me.lblDate.AutoSize = true
		Me.lblDate.Location = New System.Drawing.Point(12, 16)
		Me.lblDate.Name = "lblDate"
		Me.lblDate.Size = New System.Drawing.Size(66, 17)
		Me.lblDate.TabIndex = 2
		Me.lblDate.Text = "日付選択:"
		'
		'TabPage3
		'
		Me.TabPage3.Controls.Add(Me.txtMnt)
		Me.TabPage3.Location = New System.Drawing.Point(4, 26)
		Me.TabPage3.Name = "TabPage3"
		Me.TabPage3.Padding = New System.Windows.Forms.Padding(3)
		Me.TabPage3.Size = New System.Drawing.Size(783, 565)
		Me.TabPage3.TabIndex = 2
		Me.TabPage3.Text = "調整情報"
		Me.TabPage3.UseVisualStyleBackColor = true
		'
		'txtMnt
		'
		Me.txtMnt.Dock = System.Windows.Forms.DockStyle.Fill
		Me.txtMnt.Location = New System.Drawing.Point(3, 3)
		Me.txtMnt.Multiline = true
		Me.txtMnt.Name = "txtMnt"
		Me.txtMnt.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
		Me.txtMnt.Size = New System.Drawing.Size(777, 559)
		Me.txtMnt.TabIndex = 0
		'
		'TabPage2
		'
		Me.TabPage2.Controls.Add(Me.txtVer)
		Me.TabPage2.Location = New System.Drawing.Point(4, 26)
		Me.TabPage2.Name = "TabPage2"
		Me.TabPage2.Padding = New System.Windows.Forms.Padding(3)
		Me.TabPage2.Size = New System.Drawing.Size(783, 565)
		Me.TabPage2.TabIndex = 1
		Me.TabPage2.Text = "バージョン情報"
		Me.TabPage2.UseVisualStyleBackColor = true
		'
		'txtVer
		'
		Me.txtVer.Dock = System.Windows.Forms.DockStyle.Fill
		Me.txtVer.Location = New System.Drawing.Point(3, 3)
		Me.txtVer.Multiline = true
		Me.txtVer.Name = "txtVer"
		Me.txtVer.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
		Me.txtVer.Size = New System.Drawing.Size(777, 559)
		Me.txtVer.TabIndex = 1
		'
		'TabPage1
		'
		Me.TabPage1.Controls.Add(Me.txtPC)
		Me.TabPage1.Location = New System.Drawing.Point(4, 26)
		Me.TabPage1.Name = "TabPage1"
		Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
		Me.TabPage1.Size = New System.Drawing.Size(783, 565)
		Me.TabPage1.TabIndex = 0
		Me.TabPage1.Text = "PC情報"
		Me.TabPage1.UseVisualStyleBackColor = true
		'
		'txtPC
		'
		Me.txtPC.Dock = System.Windows.Forms.DockStyle.Fill
		Me.txtPC.Location = New System.Drawing.Point(3, 3)
		Me.txtPC.Multiline = true
		Me.txtPC.Name = "txtPC"
		Me.txtPC.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
		Me.txtPC.Size = New System.Drawing.Size(777, 559)
		Me.txtPC.TabIndex = 0
		'
		'TabControl1
		'
		Me.TabControl1.Controls.Add(Me.TabPage1)
		Me.TabControl1.Controls.Add(Me.TabPage2)
		Me.TabControl1.Controls.Add(Me.TabPage3)
		Me.TabControl1.Dock = System.Windows.Forms.DockStyle.Bottom
		Me.TabControl1.Location = New System.Drawing.Point(0, 43)
		Me.TabControl1.Name = "TabControl1"
		Me.TabControl1.SelectedIndex = 0
		Me.TabControl1.Size = New System.Drawing.Size(791, 595)
		Me.TabControl1.TabIndex = 1
		'
		'frmMent
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(8!, 17!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(791, 638)
		Me.Controls.Add(Me.lblDate)
		Me.Controls.Add(Me.TabControl1)
		Me.Controls.Add(Me.cboDateList)
		Me.Font = New System.Drawing.Font("Meiryo UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
		Me.Icon = CType(resources.GetObject("$this.Icon"),System.Drawing.Icon)
		Me.Margin = New System.Windows.Forms.Padding(4)
		Me.MaximizeBox = false
		Me.Name = "frmMent"
		Me.Text = "Maintenance information"
		Me.TopMost = true
		Me.TabPage3.ResumeLayout(false)
		Me.TabPage3.PerformLayout
		Me.TabPage2.ResumeLayout(false)
		Me.TabPage2.PerformLayout
		Me.TabPage1.ResumeLayout(false)
		Me.TabPage1.PerformLayout
		Me.TabControl1.ResumeLayout(false)
		Me.ResumeLayout(false)
		Me.PerformLayout

End Sub

	Friend WithEvents cboDateList As ComboBox
	Friend WithEvents lblDate As Label
	Friend WithEvents TabPage3 As TabPage
	Friend WithEvents txtMnt As TextBox
	Friend WithEvents TabPage2 As TabPage
	Friend WithEvents txtVer As TextBox
	Friend WithEvents TabPage1 As TabPage
	Friend WithEvents txtPC As TextBox
	Friend WithEvents TabControl1 As TabControl
End Class
