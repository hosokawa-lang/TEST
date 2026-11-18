<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmFileExport
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
		Me.btnClose = New System.Windows.Forms.Button()
		Me.lblOutputPathLabel = New System.Windows.Forms.Label()
		Me.tbxOutputPath = New System.Windows.Forms.TextBox()
		Me.btnBrowse = New System.Windows.Forms.Button()
		Me.lvItem = New System.Windows.Forms.ListView()
		Me.lblPutputItemList = New System.Windows.Forms.Label()
		Me.btnOutput = New System.Windows.Forms.Button()
		Me.btnAllSelect = New System.Windows.Forms.Button()
		Me.btnAllClear = New System.Windows.Forms.Button()
		Me.lblSampleThinLabel = New System.Windows.Forms.Label()
		Me.nudSampleThinning = New System.Windows.Forms.NumericUpDown()
		Me.lblSampleResult = New System.Windows.Forms.Label()
		Me.btnSelect = New System.Windows.Forms.Button()
		Me.btnClear = New System.Windows.Forms.Button()
		Me.svFileDlg = New System.Windows.Forms.SaveFileDialog()
		Me.ssp1 = New System.Windows.Forms.StatusStrip()
		Me.tssl1 = New System.Windows.Forms.ToolStripStatusLabel()
		Me.Panel1 = New System.Windows.Forms.Panel()
		Me.btnMeasSelect = New System.Windows.Forms.Button()
		CType(Me.nudSampleThinning,System.ComponentModel.ISupportInitialize).BeginInit
		Me.ssp1.SuspendLayout
		Me.Panel1.SuspendLayout
		Me.SuspendLayout
		'
		'btnClose
		'
		Me.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel
		Me.btnClose.Font = New System.Drawing.Font("Meiryo UI", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.btnClose.Location = New System.Drawing.Point(3, 497)
		Me.btnClose.Name = "btnClose"
		Me.btnClose.Size = New System.Drawing.Size(142, 36)
		Me.btnClose.TabIndex = 0
		Me.btnClose.Text = "Close"
		Me.btnClose.UseVisualStyleBackColor = true
		'
		'lblOutputPathLabel
		'
		Me.lblOutputPathLabel.AutoSize = true
		Me.lblOutputPathLabel.Font = New System.Drawing.Font("Meiryo UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.lblOutputPathLabel.Location = New System.Drawing.Point(14, 646)
		Me.lblOutputPathLabel.Name = "lblOutputPathLabel"
		Me.lblOutputPathLabel.Size = New System.Drawing.Size(85, 17)
		Me.lblOutputPathLabel.TabIndex = 1
		Me.lblOutputPathLabel.Text = "Output Path"
		'
		'tbxOutputPath
		'
		Me.tbxOutputPath.Font = New System.Drawing.Font("Meiryo UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.tbxOutputPath.Location = New System.Drawing.Point(17, 666)
		Me.tbxOutputPath.Name = "tbxOutputPath"
		Me.tbxOutputPath.Size = New System.Drawing.Size(401, 24)
		Me.tbxOutputPath.TabIndex = 2
		Me.tbxOutputPath.Text = "C:\"
		'
		'btnBrowse
		'
		Me.btnBrowse.Font = New System.Drawing.Font("Meiryo UI", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.btnBrowse.Location = New System.Drawing.Point(427, 662)
		Me.btnBrowse.Name = "btnBrowse"
		Me.btnBrowse.Size = New System.Drawing.Size(142, 32)
		Me.btnBrowse.TabIndex = 0
		Me.btnBrowse.Text = "Export"
		Me.btnBrowse.UseVisualStyleBackColor = true
		'
		'lvItem
		'
		Me.lvItem.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		Me.lvItem.CheckBoxes = true
		Me.lvItem.Font = New System.Drawing.Font("Meiryo UI", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.lvItem.GridLines = true
		Me.lvItem.HideSelection = false
		Me.lvItem.Location = New System.Drawing.Point(17, 21)
		Me.lvItem.MultiSelect = false
		Me.lvItem.Name = "lvItem"
		Me.lvItem.Size = New System.Drawing.Size(401, 536)
		Me.lvItem.TabIndex = 3
		Me.lvItem.UseCompatibleStateImageBehavior = false
		Me.lvItem.View = System.Windows.Forms.View.Details
		'
		'lblPutputItemList
		'
		Me.lblPutputItemList.AutoSize = true
		Me.lblPutputItemList.Font = New System.Drawing.Font("Meiryo UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.lblPutputItemList.Location = New System.Drawing.Point(14, 1)
		Me.lblPutputItemList.Name = "lblPutputItemList"
		Me.lblPutputItemList.Size = New System.Drawing.Size(111, 17)
		Me.lblPutputItemList.TabIndex = 1
		Me.lblPutputItemList.Text = "Output Item List"
		'
		'btnOutput
		'
		Me.btnOutput.Font = New System.Drawing.Font("Meiryo UI", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.btnOutput.Location = New System.Drawing.Point(3, 3)
		Me.btnOutput.Name = "btnOutput"
		Me.btnOutput.Size = New System.Drawing.Size(142, 36)
		Me.btnOutput.TabIndex = 0
		Me.btnOutput.Text = "Export"
		Me.btnOutput.UseVisualStyleBackColor = true
		Me.btnOutput.Visible = false
		'
		'btnAllSelect
		'
		Me.btnAllSelect.Font = New System.Drawing.Font("Meiryo UI", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.btnAllSelect.Location = New System.Drawing.Point(3, 89)
		Me.btnAllSelect.Name = "btnAllSelect"
		Me.btnAllSelect.Size = New System.Drawing.Size(142, 36)
		Me.btnAllSelect.TabIndex = 0
		Me.btnAllSelect.Text = "All Select"
		Me.btnAllSelect.UseVisualStyleBackColor = true
		'
		'btnAllClear
		'
		Me.btnAllClear.Font = New System.Drawing.Font("Meiryo UI", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.btnAllClear.Location = New System.Drawing.Point(3, 131)
		Me.btnAllClear.Name = "btnAllClear"
		Me.btnAllClear.Size = New System.Drawing.Size(142, 36)
		Me.btnAllClear.TabIndex = 0
		Me.btnAllClear.Text = "All Clear"
		Me.btnAllClear.UseVisualStyleBackColor = true
		'
		'lblSampleThinLabel
		'
		Me.lblSampleThinLabel.AutoSize = true
		Me.lblSampleThinLabel.Font = New System.Drawing.Font("Meiryo UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.lblSampleThinLabel.Location = New System.Drawing.Point(14, 598)
		Me.lblSampleThinLabel.Name = "lblSampleThinLabel"
		Me.lblSampleThinLabel.Size = New System.Drawing.Size(113, 17)
		Me.lblSampleThinLabel.TabIndex = 1
		Me.lblSampleThinLabel.Text = "Sample Thinning"
		'
		'nudSampleThinning
		'
		Me.nudSampleThinning.Font = New System.Drawing.Font("Meiryo UI", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.nudSampleThinning.Increment = New Decimal(New Integer() {10, 0, 0, 0})
		Me.nudSampleThinning.Location = New System.Drawing.Point(133, 593)
		Me.nudSampleThinning.Maximum = New Decimal(New Integer() {10000000, 0, 0, 0})
		Me.nudSampleThinning.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
		Me.nudSampleThinning.Name = "nudSampleThinning"
		Me.nudSampleThinning.Size = New System.Drawing.Size(82, 28)
		Me.nudSampleThinning.TabIndex = 4
		Me.nudSampleThinning.Value = New Decimal(New Integer() {1, 0, 0, 0})
		'
		'lblSampleResult
		'
		Me.lblSampleResult.AutoSize = true
		Me.lblSampleResult.Font = New System.Drawing.Font("Meiryo UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.lblSampleResult.Location = New System.Drawing.Point(221, 598)
		Me.lblSampleResult.Name = "lblSampleResult"
		Me.lblSampleResult.Size = New System.Drawing.Size(114, 17)
		Me.lblSampleResult.TabIndex = 1
		Me.lblSampleResult.Text = "99.9%  9999999"
		'
		'btnSelect
		'
		Me.btnSelect.Font = New System.Drawing.Font("Meiryo UI", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.btnSelect.Location = New System.Drawing.Point(3, 5)
		Me.btnSelect.Name = "btnSelect"
		Me.btnSelect.Size = New System.Drawing.Size(142, 36)
		Me.btnSelect.TabIndex = 5
		Me.btnSelect.Text = "Select"
		Me.btnSelect.UseVisualStyleBackColor = true
		'
		'btnClear
		'
		Me.btnClear.Font = New System.Drawing.Font("Meiryo UI", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.btnClear.Location = New System.Drawing.Point(3, 47)
		Me.btnClear.Name = "btnClear"
		Me.btnClear.Size = New System.Drawing.Size(142, 36)
		Me.btnClear.TabIndex = 5
		Me.btnClear.Text = "Clear"
		Me.btnClear.UseVisualStyleBackColor = true
		'
		'ssp1
		'
		Me.ssp1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tssl1})
		Me.ssp1.Location = New System.Drawing.Point(0, 703)
		Me.ssp1.Name = "ssp1"
		Me.ssp1.Size = New System.Drawing.Size(578, 22)
		Me.ssp1.TabIndex = 6
		Me.ssp1.Text = "StatusStrip1"
		'
		'tssl1
		'
		Me.tssl1.Name = "tssl1"
		Me.tssl1.Size = New System.Drawing.Size(0, 17)
		'
		'Panel1
		'
		Me.Panel1.Controls.Add(Me.btnMeasSelect)
		Me.Panel1.Controls.Add(Me.btnOutput)
		Me.Panel1.Controls.Add(Me.btnSelect)
		Me.Panel1.Controls.Add(Me.btnClear)
		Me.Panel1.Controls.Add(Me.btnAllSelect)
		Me.Panel1.Controls.Add(Me.btnAllClear)
		Me.Panel1.Controls.Add(Me.btnClose)
		Me.Panel1.Location = New System.Drawing.Point(424, 21)
		Me.Panel1.Name = "Panel1"
		Me.Panel1.Size = New System.Drawing.Size(149, 536)
		Me.Panel1.TabIndex = 7
		'
		'btnMeasSelect
		'
		Me.btnMeasSelect.Font = New System.Drawing.Font("Meiryo UI", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.btnMeasSelect.Location = New System.Drawing.Point(3, 173)
		Me.btnMeasSelect.Name = "btnMeasSelect"
		Me.btnMeasSelect.Size = New System.Drawing.Size(142, 36)
		Me.btnMeasSelect.TabIndex = 6
		Me.btnMeasSelect.Text = "Select Meas."
		Me.btnMeasSelect.UseVisualStyleBackColor = true
		'
		'frmFileExport
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6!, 12!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.CancelButton = Me.btnClose
		Me.ClientSize = New System.Drawing.Size(578, 725)
		Me.Controls.Add(Me.Panel1)
		Me.Controls.Add(Me.ssp1)
		Me.Controls.Add(Me.nudSampleThinning)
		Me.Controls.Add(Me.lvItem)
		Me.Controls.Add(Me.tbxOutputPath)
		Me.Controls.Add(Me.lblPutputItemList)
		Me.Controls.Add(Me.lblSampleResult)
		Me.Controls.Add(Me.lblSampleThinLabel)
		Me.Controls.Add(Me.lblOutputPathLabel)
		Me.Controls.Add(Me.btnBrowse)
		Me.MaximizeBox = false
		Me.MinimizeBox = false
		Me.Name = "frmFileExport"
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
		Me.Text = "frmFileExport"
		CType(Me.nudSampleThinning,System.ComponentModel.ISupportInitialize).EndInit
		Me.ssp1.ResumeLayout(false)
		Me.ssp1.PerformLayout
		Me.Panel1.ResumeLayout(false)
		Me.ResumeLayout(false)
		Me.PerformLayout

End Sub

	Friend WithEvents btnClose As Button
	Friend WithEvents lblOutputPathLabel As Label
	Friend WithEvents tbxOutputPath As TextBox
	Friend WithEvents btnBrowse As Button
	Friend WithEvents lvItem As ListView
	Friend WithEvents lblPutputItemList As Label
	Friend WithEvents btnOutput As Button
	Friend WithEvents btnAllSelect As Button
	Friend WithEvents btnAllClear As Button
	Friend WithEvents lblSampleThinLabel As Label
	Friend WithEvents nudSampleThinning As NumericUpDown
	Friend WithEvents lblSampleResult As Label
	Friend WithEvents btnSelect As Button
	Friend WithEvents btnClear As Button
	Friend WithEvents svFileDlg As SaveFileDialog
	Friend WithEvents ssp1 As StatusStrip
	Friend WithEvents tssl1 As ToolStripStatusLabel
	Friend WithEvents Panel1 As Panel
	Friend WithEvents btnMeasSelect As Button
End Class
