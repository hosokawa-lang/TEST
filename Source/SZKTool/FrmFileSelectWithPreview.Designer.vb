<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmFileSelectWithPreview
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
		Me.btnOpen = New System.Windows.Forms.Button()
		Me.btnCancel = New System.Windows.Forms.Button()
		Me.tvFolders = New System.Windows.Forms.TreeView()
		Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
		Me.lvFiles = New System.Windows.Forms.ListView()
		Me.lvPreview = New System.Windows.Forms.ListView()
		Me.Panel2 = New System.Windows.Forms.Panel()
		Me.ckbMarkOnlyDisp = New System.Windows.Forms.CheckBox()
		Me.btnMarkClear = New System.Windows.Forms.Button()
		Me.SplitContainer2 = New System.Windows.Forms.SplitContainer()
		CType(Me.SplitContainer1,System.ComponentModel.ISupportInitialize).BeginInit
		Me.SplitContainer1.Panel1.SuspendLayout
		Me.SplitContainer1.Panel2.SuspendLayout
		Me.SplitContainer1.SuspendLayout
		Me.Panel2.SuspendLayout
		CType(Me.SplitContainer2,System.ComponentModel.ISupportInitialize).BeginInit
		Me.SplitContainer2.Panel1.SuspendLayout
		Me.SplitContainer2.Panel2.SuspendLayout
		Me.SplitContainer2.SuspendLayout
		Me.SuspendLayout
		'
		'btnOpen
		'
		Me.btnOpen.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
		Me.btnOpen.Location = New System.Drawing.Point(973, 9)
		Me.btnOpen.Margin = New System.Windows.Forms.Padding(4)
		Me.btnOpen.Name = "btnOpen"
		Me.btnOpen.Size = New System.Drawing.Size(149, 33)
		Me.btnOpen.TabIndex = 2
		Me.btnOpen.Text = "&Open"
		Me.btnOpen.UseVisualStyleBackColor = true
		'
		'btnCancel
		'
		Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
		Me.btnCancel.Location = New System.Drawing.Point(1130, 9)
		Me.btnCancel.Margin = New System.Windows.Forms.Padding(4)
		Me.btnCancel.Name = "btnCancel"
		Me.btnCancel.Size = New System.Drawing.Size(135, 33)
		Me.btnCancel.TabIndex = 2
		Me.btnCancel.Text = "&Cancel"
		Me.btnCancel.UseVisualStyleBackColor = true
		'
		'tvFolders
		'
		Me.tvFolders.Dock = System.Windows.Forms.DockStyle.Fill
		Me.tvFolders.Location = New System.Drawing.Point(0, 0)
		Me.tvFolders.Margin = New System.Windows.Forms.Padding(4)
		Me.tvFolders.Name = "tvFolders"
		Me.tvFolders.Size = New System.Drawing.Size(224, 770)
		Me.tvFolders.TabIndex = 3
		'
		'SplitContainer1
		'
		Me.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill
		Me.SplitContainer1.Location = New System.Drawing.Point(0, 0)
		Me.SplitContainer1.Margin = New System.Windows.Forms.Padding(4)
		Me.SplitContainer1.Name = "SplitContainer1"
		'
		'SplitContainer1.Panel1
		'
		Me.SplitContainer1.Panel1.Controls.Add(Me.lvFiles)
		'
		'SplitContainer1.Panel2
		'
		Me.SplitContainer1.Panel2.Controls.Add(Me.lvPreview)
		Me.SplitContainer1.Size = New System.Drawing.Size(1046, 770)
		Me.SplitContainer1.SplitterDistance = 458
		Me.SplitContainer1.SplitterWidth = 5
		Me.SplitContainer1.TabIndex = 4
		'
		'lvFiles
		'
		Me.lvFiles.CheckBoxes = true
		Me.lvFiles.Dock = System.Windows.Forms.DockStyle.Fill
		Me.lvFiles.HideSelection = false
		Me.lvFiles.Location = New System.Drawing.Point(0, 0)
		Me.lvFiles.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
		Me.lvFiles.Name = "lvFiles"
		Me.lvFiles.Size = New System.Drawing.Size(458, 770)
		Me.lvFiles.TabIndex = 7
		Me.lvFiles.UseCompatibleStateImageBehavior = false
		'
		'lvPreview
		'
		Me.lvPreview.Dock = System.Windows.Forms.DockStyle.Fill
		Me.lvPreview.HideSelection = false
		Me.lvPreview.Location = New System.Drawing.Point(0, 0)
		Me.lvPreview.Name = "lvPreview"
		Me.lvPreview.Size = New System.Drawing.Size(583, 770)
		Me.lvPreview.TabIndex = 0
		Me.lvPreview.UseCompatibleStateImageBehavior = false
		'
		'Panel2
		'
		Me.Panel2.Controls.Add(Me.ckbMarkOnlyDisp)
		Me.Panel2.Controls.Add(Me.btnMarkClear)
		Me.Panel2.Controls.Add(Me.btnOpen)
		Me.Panel2.Controls.Add(Me.btnCancel)
		Me.Panel2.Dock = System.Windows.Forms.DockStyle.Bottom
		Me.Panel2.Location = New System.Drawing.Point(0, 770)
		Me.Panel2.Name = "Panel2"
		Me.Panel2.Size = New System.Drawing.Size(1274, 50)
		Me.Panel2.TabIndex = 6
		'
		'ckbMarkOnlyDisp
		'
		Me.ckbMarkOnlyDisp.AutoSize = true
		Me.ckbMarkOnlyDisp.Location = New System.Drawing.Point(12, 16)
		Me.ckbMarkOnlyDisp.Name = "ckbMarkOnlyDisp"
		Me.ckbMarkOnlyDisp.Size = New System.Drawing.Size(119, 21)
		Me.ckbMarkOnlyDisp.TabIndex = 3
		Me.ckbMarkOnlyDisp.Text = "マーク行のみ表示"
		Me.ckbMarkOnlyDisp.UseVisualStyleBackColor = true
		'
		'btnMarkClear
		'
		Me.btnMarkClear.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
		Me.btnMarkClear.Location = New System.Drawing.Point(258, 9)
		Me.btnMarkClear.Margin = New System.Windows.Forms.Padding(4)
		Me.btnMarkClear.Name = "btnMarkClear"
		Me.btnMarkClear.Size = New System.Drawing.Size(149, 33)
		Me.btnMarkClear.TabIndex = 2
		Me.btnMarkClear.Text = "マーククリア"
		Me.btnMarkClear.UseVisualStyleBackColor = true
		'
		'SplitContainer2
		'
		Me.SplitContainer2.Dock = System.Windows.Forms.DockStyle.Fill
		Me.SplitContainer2.Location = New System.Drawing.Point(0, 0)
		Me.SplitContainer2.Margin = New System.Windows.Forms.Padding(5)
		Me.SplitContainer2.Name = "SplitContainer2"
		'
		'SplitContainer2.Panel1
		'
		Me.SplitContainer2.Panel1.Controls.Add(Me.tvFolders)
		'
		'SplitContainer2.Panel2
		'
		Me.SplitContainer2.Panel2.Controls.Add(Me.SplitContainer1)
		Me.SplitContainer2.Size = New System.Drawing.Size(1274, 770)
		Me.SplitContainer2.SplitterDistance = 224
		Me.SplitContainer2.TabIndex = 7
		'
		'FrmFileSelectWithPreview
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(8!, 17!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(1274, 820)
		Me.Controls.Add(Me.SplitContainer2)
		Me.Controls.Add(Me.Panel2)
		Me.Font = New System.Drawing.Font("Meiryo UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.Margin = New System.Windows.Forms.Padding(4)
		Me.Name = "FrmFileSelectWithPreview"
		Me.Text = "FrmFileSelectWithPreview"
		Me.SplitContainer1.Panel1.ResumeLayout(false)
		Me.SplitContainer1.Panel2.ResumeLayout(false)
		CType(Me.SplitContainer1,System.ComponentModel.ISupportInitialize).EndInit
		Me.SplitContainer1.ResumeLayout(false)
		Me.Panel2.ResumeLayout(false)
		Me.Panel2.PerformLayout
		Me.SplitContainer2.Panel1.ResumeLayout(false)
		Me.SplitContainer2.Panel2.ResumeLayout(false)
		CType(Me.SplitContainer2,System.ComponentModel.ISupportInitialize).EndInit
		Me.SplitContainer2.ResumeLayout(false)
		Me.ResumeLayout(false)

End Sub
	Friend WithEvents btnOpen As Button
	Friend WithEvents btnCancel As Button
	Friend WithEvents tvFolders As TreeView
	Friend WithEvents SplitContainer1 As SplitContainer
	Friend WithEvents Panel2 As Panel
	Friend WithEvents lvFiles As ListView
	Friend WithEvents SplitContainer2 As SplitContainer
	Friend WithEvents lvPreview As ListView
	Friend WithEvents ckbMarkOnlyDisp As CheckBox
	Friend WithEvents btnMarkClear As Button
End Class
