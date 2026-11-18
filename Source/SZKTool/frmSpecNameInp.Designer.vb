<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmSpecNameInp
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
		Me.components = New System.ComponentModel.Container()
		Me.tbxSpecName = New System.Windows.Forms.TextBox()
		Me.btnSaveAs = New System.Windows.Forms.Button()
		Me.btnClose = New System.Windows.Forms.Button()
		Me.lbxSpec = New System.Windows.Forms.ListBox()
		Me.btnDelete = New System.Windows.Forms.Button()
		Me.dgvSpecView = New System.Windows.Forms.DataGridView()
		Me.cmsSpecSelect = New System.Windows.Forms.ContextMenuStrip(Me.components)
		Me.ToolStripMenuItem2 = New System.Windows.Forms.ToolStripMenuItem()
		Me.ToolStripMenuItem4 = New System.Windows.Forms.ToolStripMenuItem()
		Me.ToolStripMenuItem3 = New System.Windows.Forms.ToolStripMenuItem()
		CType(Me.dgvSpecView,System.ComponentModel.ISupportInitialize).BeginInit
		Me.cmsSpecSelect.SuspendLayout
		Me.SuspendLayout
		'
		'tbxSpecName
		'
		Me.tbxSpecName.Font = New System.Drawing.Font("Meiryo UI", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.tbxSpecName.Location = New System.Drawing.Point(12, 480)
		Me.tbxSpecName.Name = "tbxSpecName"
		Me.tbxSpecName.Size = New System.Drawing.Size(817, 28)
		Me.tbxSpecName.TabIndex = 0
		'
		'btnSaveAs
		'
		Me.btnSaveAs.Font = New System.Drawing.Font("Meiryo UI", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.btnSaveAs.Location = New System.Drawing.Point(532, 523)
		Me.btnSaveAs.Name = "btnSaveAs"
		Me.btnSaveAs.Size = New System.Drawing.Size(145, 32)
		Me.btnSaveAs.TabIndex = 1
		Me.btnSaveAs.Text = "&Save As"
		Me.btnSaveAs.UseVisualStyleBackColor = true
		'
		'btnClose
		'
		Me.btnClose.Font = New System.Drawing.Font("Meiryo UI", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.btnClose.Location = New System.Drawing.Point(683, 523)
		Me.btnClose.Name = "btnClose"
		Me.btnClose.Size = New System.Drawing.Size(145, 32)
		Me.btnClose.TabIndex = 2
		Me.btnClose.Text = "&Close"
		Me.btnClose.UseVisualStyleBackColor = true
		'
		'lbxSpec
		'
		Me.lbxSpec.Font = New System.Drawing.Font("Meiryo UI", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.lbxSpec.FormattingEnabled = true
		Me.lbxSpec.ItemHeight = 20
		Me.lbxSpec.Location = New System.Drawing.Point(12, 12)
		Me.lbxSpec.Name = "lbxSpec"
		Me.lbxSpec.ScrollAlwaysVisible = true
		Me.lbxSpec.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple
		Me.lbxSpec.Size = New System.Drawing.Size(415, 464)
		Me.lbxSpec.TabIndex = 3
		'
		'btnDelete
		'
		Me.btnDelete.Font = New System.Drawing.Font("Meiryo UI", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.btnDelete.Location = New System.Drawing.Point(12, 523)
		Me.btnDelete.Name = "btnDelete"
		Me.btnDelete.Size = New System.Drawing.Size(145, 32)
		Me.btnDelete.TabIndex = 5
		Me.btnDelete.Text = "&Delete"
		Me.btnDelete.UseVisualStyleBackColor = true
		'
		'dgvSpecView
		'
		Me.dgvSpecView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
		Me.dgvSpecView.Location = New System.Drawing.Point(433, 12)
		Me.dgvSpecView.Name = "dgvSpecView"
		Me.dgvSpecView.RowTemplate.Height = 21
		Me.dgvSpecView.Size = New System.Drawing.Size(394, 462)
		Me.dgvSpecView.TabIndex = 6
		'
		'cmsSpecSelect
		'
		Me.cmsSpecSelect.Font = New System.Drawing.Font("Meiryo UI", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.cmsSpecSelect.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItem2, Me.ToolStripMenuItem3})
		Me.cmsSpecSelect.Name = "cmsSpecSelect"
		Me.cmsSpecSelect.Size = New System.Drawing.Size(236, 52)
		'
		'ToolStripMenuItem2
		'
		Me.ToolStripMenuItem2.Checked = true
		Me.ToolStripMenuItem2.CheckOnClick = true
		Me.ToolStripMenuItem2.CheckState = System.Windows.Forms.CheckState.Checked
		Me.ToolStripMenuItem2.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItem4})
		Me.ToolStripMenuItem2.Name = "ToolStripMenuItem2"
		Me.ToolStripMenuItem2.Size = New System.Drawing.Size(235, 24)
		Me.ToolStripMenuItem2.Text = "ToolStripMenuItem2"
		'
		'ToolStripMenuItem4
		'
		Me.ToolStripMenuItem4.Name = "ToolStripMenuItem4"
		Me.ToolStripMenuItem4.Size = New System.Drawing.Size(235, 24)
		Me.ToolStripMenuItem4.Text = "ToolStripMenuItem4"
		'
		'ToolStripMenuItem3
		'
		Me.ToolStripMenuItem3.Name = "ToolStripMenuItem3"
		Me.ToolStripMenuItem3.Size = New System.Drawing.Size(235, 24)
		Me.ToolStripMenuItem3.Text = "ToolStripMenuItem3"
		'
		'frmSpecNameInp
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6!, 12!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(841, 567)
		Me.ControlBox = false
		Me.Controls.Add(Me.dgvSpecView)
		Me.Controls.Add(Me.btnDelete)
		Me.Controls.Add(Me.lbxSpec)
		Me.Controls.Add(Me.btnClose)
		Me.Controls.Add(Me.btnSaveAs)
		Me.Controls.Add(Me.tbxSpecName)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
		Me.Name = "frmSpecNameInp"
		Me.ShowInTaskbar = false
		Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
		Me.Text = "Save SPEC As "
		CType(Me.dgvSpecView,System.ComponentModel.ISupportInitialize).EndInit
		Me.cmsSpecSelect.ResumeLayout(false)
		Me.ResumeLayout(false)
		Me.PerformLayout

End Sub

	Friend WithEvents tbxSpecName As TextBox
	Friend WithEvents btnSaveAs As Button
	Friend WithEvents btnClose As Button
	Friend WithEvents lbxSpec As ListBox
	Friend WithEvents btnDelete As Button
	Friend WithEvents dgvSpecView As DataGridView
	Friend WithEvents cmsSpecSelect As ContextMenuStrip
	Friend WithEvents ToolStripMenuItem2 As ToolStripMenuItem
	Friend WithEvents ToolStripMenuItem4 As ToolStripMenuItem
	Friend WithEvents ToolStripMenuItem3 As ToolStripMenuItem
End Class
