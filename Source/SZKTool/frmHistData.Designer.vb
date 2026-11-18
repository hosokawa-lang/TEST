<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmHistData
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
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmHistData))
		Me.gbxHistdata = New System.Windows.Forms.GroupBox()
		Me.dgvHistHead = New System.Windows.Forms.DataGridView()
		Me.cmsEdit = New System.Windows.Forms.ContextMenuStrip(Me.components)
		Me.tsmiDelete = New System.Windows.Forms.ToolStripMenuItem()
		Me.tsmiReadReal = New System.Windows.Forms.ToolStripMenuItem()
		Me.tsmiReadStd = New System.Windows.Forms.ToolStripMenuItem()
		Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
		Me.tsmiMoveToStandard = New System.Windows.Forms.ToolStripMenuItem()
		Me.tsmiMoveStdArea1 = New System.Windows.Forms.ToolStripMenuItem()
		Me.tsmiMoveStdArea2 = New System.Windows.Forms.ToolStripMenuItem()
		Me.tsmiCopyToStandard = New System.Windows.Forms.ToolStripMenuItem()
		Me.tsmiCopyStdArea1 = New System.Windows.Forms.ToolStripMenuItem()
		Me.tsmiCopyStdArea2 = New System.Windows.Forms.ToolStripMenuItem()
		Me.mnsDisp = New System.Windows.Forms.MenuStrip()
		Me.tsmiMnsFile = New System.Windows.Forms.ToolStripMenuItem()
		Me.tsmiFileReadReal = New System.Windows.Forms.ToolStripMenuItem()
		Me.tsmiFileReadStd1 = New System.Windows.Forms.ToolStripMenuItem()
		Me.tsmiFileReadStd2 = New System.Windows.Forms.ToolStripMenuItem()
		Me.tsmiMsnClose = New System.Windows.Forms.ToolStripMenuItem()
		Me.tsmiDisp = New System.Windows.Forms.ToolStripMenuItem()
		Me.tsmiMatrixSwap = New System.Windows.Forms.ToolStripMenuItem()
		Me.tsmiVerticlSeries = New System.Windows.Forms.ToolStripMenuItem()
		Me.tsmiHorizontalSeries = New System.Windows.Forms.ToolStripMenuItem()
		Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
		Me.tsmiExport = New System.Windows.Forms.ToolStripMenuItem()
		Me.gbxHistdata.SuspendLayout
		CType(Me.dgvHistHead,System.ComponentModel.ISupportInitialize).BeginInit
		Me.cmsEdit.SuspendLayout
		Me.mnsDisp.SuspendLayout
		Me.SuspendLayout
		'
		'gbxHistdata
		'
		Me.gbxHistdata.Controls.Add(Me.dgvHistHead)
		Me.gbxHistdata.Dock = System.Windows.Forms.DockStyle.Fill
		Me.gbxHistdata.Font = New System.Drawing.Font("Meiryo UI", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.gbxHistdata.Location = New System.Drawing.Point(0, 0)
		Me.gbxHistdata.Name = "gbxHistdata"
		Me.gbxHistdata.Size = New System.Drawing.Size(664, 368)
		Me.gbxHistdata.TabIndex = 84
		Me.gbxHistdata.TabStop = false
		'
		'dgvHistHead
		'
		Me.dgvHistHead.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
		Me.dgvHistHead.Dock = System.Windows.Forms.DockStyle.Fill
		Me.dgvHistHead.Location = New System.Drawing.Point(3, 23)
		Me.dgvHistHead.Name = "dgvHistHead"
		Me.dgvHistHead.RowTemplate.Height = 21
		Me.dgvHistHead.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.dgvHistHead.Size = New System.Drawing.Size(658, 342)
		Me.dgvHistHead.TabIndex = 0
		'
		'cmsEdit
		'
		Me.cmsEdit.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsmiDelete, Me.tsmiReadReal, Me.tsmiReadStd, Me.ToolStripSeparator1, Me.tsmiMoveToStandard, Me.tsmiCopyToStandard, Me.ToolStripSeparator2, Me.tsmiExport})
		Me.cmsEdit.Name = "cmsEdit"
		Me.cmsEdit.Size = New System.Drawing.Size(181, 170)
		'
		'tsmiDelete
		'
		Me.tsmiDelete.Name = "tsmiDelete"
		Me.tsmiDelete.Size = New System.Drawing.Size(180, 22)
		Me.tsmiDelete.Text = "&Delete"
		'
		'tsmiReadReal
		'
		Me.tsmiReadReal.Name = "tsmiReadReal"
		Me.tsmiReadReal.Size = New System.Drawing.Size(180, 22)
		Me.tsmiReadReal.Text = "Read &Real"
		'
		'tsmiReadStd
		'
		Me.tsmiReadStd.Name = "tsmiReadStd"
		Me.tsmiReadStd.Size = New System.Drawing.Size(180, 22)
		Me.tsmiReadStd.Text = "Read &Standard"
		'
		'ToolStripSeparator1
		'
		Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
		Me.ToolStripSeparator1.Size = New System.Drawing.Size(177, 6)
		'
		'tsmiMoveToStandard
		'
		Me.tsmiMoveToStandard.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsmiMoveStdArea1, Me.tsmiMoveStdArea2})
		Me.tsmiMoveToStandard.Name = "tsmiMoveToStandard"
		Me.tsmiMoveToStandard.Size = New System.Drawing.Size(180, 22)
		Me.tsmiMoveToStandard.Text = "&Move to standard"
		'
		'tsmiMoveStdArea1
		'
		Me.tsmiMoveStdArea1.Name = "tsmiMoveStdArea1"
		Me.tsmiMoveStdArea1.Size = New System.Drawing.Size(107, 22)
		Me.tsmiMoveStdArea1.Text = "Area &1"
		'
		'tsmiMoveStdArea2
		'
		Me.tsmiMoveStdArea2.Name = "tsmiMoveStdArea2"
		Me.tsmiMoveStdArea2.Size = New System.Drawing.Size(107, 22)
		Me.tsmiMoveStdArea2.Text = "Area &2"
		'
		'tsmiCopyToStandard
		'
		Me.tsmiCopyToStandard.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsmiCopyStdArea1, Me.tsmiCopyStdArea2})
		Me.tsmiCopyToStandard.Name = "tsmiCopyToStandard"
		Me.tsmiCopyToStandard.Size = New System.Drawing.Size(180, 22)
		Me.tsmiCopyToStandard.Text = "&Copy to standard"
		'
		'tsmiCopyStdArea1
		'
		Me.tsmiCopyStdArea1.Name = "tsmiCopyStdArea1"
		Me.tsmiCopyStdArea1.Size = New System.Drawing.Size(107, 22)
		Me.tsmiCopyStdArea1.Text = "Area &1"
		'
		'tsmiCopyStdArea2
		'
		Me.tsmiCopyStdArea2.Name = "tsmiCopyStdArea2"
		Me.tsmiCopyStdArea2.Size = New System.Drawing.Size(107, 22)
		Me.tsmiCopyStdArea2.Text = "Area &2"
		'
		'mnsDisp
		'
		Me.mnsDisp.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsmiMnsFile, Me.tsmiDisp})
		Me.mnsDisp.Location = New System.Drawing.Point(0, 0)
		Me.mnsDisp.Name = "mnsDisp"
		Me.mnsDisp.Size = New System.Drawing.Size(664, 24)
		Me.mnsDisp.TabIndex = 85
		Me.mnsDisp.Text = "MenuStrip1"
		'
		'tsmiMnsFile
		'
		Me.tsmiMnsFile.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsmiFileReadReal, Me.tsmiFileReadStd1, Me.tsmiFileReadStd2, Me.tsmiMsnClose})
		Me.tsmiMnsFile.Name = "tsmiMnsFile"
		Me.tsmiMnsFile.Size = New System.Drawing.Size(37, 20)
		Me.tsmiMnsFile.Text = "&File"
		'
		'tsmiFileReadReal
		'
		Me.tsmiFileReadReal.Name = "tsmiFileReadReal"
		Me.tsmiFileReadReal.Size = New System.Drawing.Size(181, 22)
		Me.tsmiFileReadReal.Text = "Read &Real area"
		'
		'tsmiFileReadStd1
		'
		Me.tsmiFileReadStd1.Name = "tsmiFileReadStd1"
		Me.tsmiFileReadStd1.Size = New System.Drawing.Size(181, 22)
		Me.tsmiFileReadStd1.Text = "Read Standard area&1"
		'
		'tsmiFileReadStd2
		'
		Me.tsmiFileReadStd2.Name = "tsmiFileReadStd2"
		Me.tsmiFileReadStd2.Size = New System.Drawing.Size(181, 22)
		Me.tsmiFileReadStd2.Text = "Read Standard area&2"
		'
		'tsmiMsnClose
		'
		Me.tsmiMsnClose.Name = "tsmiMsnClose"
		Me.tsmiMsnClose.Size = New System.Drawing.Size(181, 22)
		Me.tsmiMsnClose.Text = "&Close"
		'
		'tsmiDisp
		'
		Me.tsmiDisp.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsmiMatrixSwap})
		Me.tsmiDisp.Name = "tsmiDisp"
		Me.tsmiDisp.Size = New System.Drawing.Size(56, 20)
		Me.tsmiDisp.Text = "&Setting"
		'
		'tsmiMatrixSwap
		'
		Me.tsmiMatrixSwap.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsmiVerticlSeries, Me.tsmiHorizontalSeries})
		Me.tsmiMatrixSwap.Name = "tsmiMatrixSwap"
		Me.tsmiMatrixSwap.Size = New System.Drawing.Size(139, 22)
		Me.tsmiMatrixSwap.Text = "Matrix Swap"
		'
		'tsmiVerticlSeries
		'
		Me.tsmiVerticlSeries.Name = "tsmiVerticlSeries"
		Me.tsmiVerticlSeries.Size = New System.Drawing.Size(161, 22)
		Me.tsmiVerticlSeries.Text = "&Verticl series"
		'
		'tsmiHorizontalSeries
		'
		Me.tsmiHorizontalSeries.Name = "tsmiHorizontalSeries"
		Me.tsmiHorizontalSeries.Size = New System.Drawing.Size(161, 22)
		Me.tsmiHorizontalSeries.Text = "&Horizontal series"
		'
		'ToolStripSeparator2
		'
		Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
		Me.ToolStripSeparator2.Size = New System.Drawing.Size(177, 6)
		'
		'tsmiExport
		'
		Me.tsmiExport.Name = "tsmiExport"
		Me.tsmiExport.Size = New System.Drawing.Size(180, 22)
		Me.tsmiExport.Text = "&Export"
		'
		'frmHistData
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6!, 12!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(664, 368)
		Me.Controls.Add(Me.mnsDisp)
		Me.Controls.Add(Me.gbxHistdata)
		Me.Icon = CType(resources.GetObject("$this.Icon"),System.Drawing.Icon)
		Me.MinimizeBox = false
		Me.Name = "frmHistData"
		Me.Text = "Measurement history information"
		Me.gbxHistdata.ResumeLayout(false)
		CType(Me.dgvHistHead,System.ComponentModel.ISupportInitialize).EndInit
		Me.cmsEdit.ResumeLayout(false)
		Me.mnsDisp.ResumeLayout(false)
		Me.mnsDisp.PerformLayout
		Me.ResumeLayout(false)
		Me.PerformLayout

End Sub

	Friend WithEvents gbxHistdata As GroupBox
	Friend WithEvents dgvHistHead As DataGridView
	Friend WithEvents cmsEdit As ContextMenuStrip
	Friend WithEvents tsmiDelete As ToolStripMenuItem
	Friend WithEvents mnsDisp As MenuStrip
	Friend WithEvents tsmiDisp As ToolStripMenuItem
	Friend WithEvents tsmiMatrixSwap As ToolStripMenuItem
	Friend WithEvents tsmiVerticlSeries As ToolStripMenuItem
	Friend WithEvents tsmiHorizontalSeries As ToolStripMenuItem
	Friend WithEvents tsmiReadStd As ToolStripMenuItem
	Friend WithEvents tsmiReadReal As ToolStripMenuItem
	Friend WithEvents tsmiMnsFile As ToolStripMenuItem
	Friend WithEvents tsmiFileReadReal As ToolStripMenuItem
	Friend WithEvents tsmiFileReadStd1 As ToolStripMenuItem
	Friend WithEvents tsmiFileReadStd2 As ToolStripMenuItem
	Friend WithEvents tsmiMsnClose As ToolStripMenuItem
	Friend WithEvents tsmiMoveToStandard As ToolStripMenuItem
	Friend WithEvents tsmiMoveStdArea1 As ToolStripMenuItem
	Friend WithEvents tsmiMoveStdArea2 As ToolStripMenuItem
	Friend WithEvents ToolStripSeparator1 As ToolStripSeparator
	Friend WithEvents tsmiCopyToStandard As ToolStripMenuItem
	Friend WithEvents tsmiCopyStdArea1 As ToolStripMenuItem
	Friend WithEvents tsmiCopyStdArea2 As ToolStripMenuItem
	Friend WithEvents ToolStripSeparator2 As ToolStripSeparator
	Friend WithEvents tsmiExport As ToolStripMenuItem
End Class
