<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmDebug
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
		Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
		Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
		Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
		Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
		Me.dgvArea = New System.Windows.Forms.DataGridView()
		Me.clmMemory = New System.Windows.Forms.DataGridViewTextBoxColumn()
		Me.clmItem = New System.Windows.Forms.DataGridViewTextBoxColumn()
		Me.clmVal = New System.Windows.Forms.DataGridViewTextBoxColumn()
		Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
		Me.btnHex = New System.Windows.Forms.Button()
		Me.cbTopMost = New System.Windows.Forms.CheckBox()
		Me.Label1 = New System.Windows.Forms.Label()
		Me.lblBGWOrder = New System.Windows.Forms.Label()
		Me.GroupBox1 = New System.Windows.Forms.GroupBox()
		Me.lblCalJob = New System.Windows.Forms.Label()
		Me.lblMeasureState = New System.Windows.Forms.Label()
		Me.Label4 = New System.Windows.Forms.Label()
		Me.lblBGWOrderNext = New System.Windows.Forms.Label()
		Me.Label2 = New System.Windows.Forms.Label()
		Me.Label3 = New System.Windows.Forms.Label()
		Me.lblDevErr = New System.Windows.Forms.Label()
		Me.Label5 = New System.Windows.Forms.Label()
		CType(Me.dgvArea,System.ComponentModel.ISupportInitialize).BeginInit
		Me.GroupBox1.SuspendLayout
		Me.SuspendLayout
		'
		'dgvArea
		'
		Me.dgvArea.AllowUserToAddRows = false
		Me.dgvArea.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom)  _
            Or System.Windows.Forms.AnchorStyles.Left)  _
            Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
		DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
		DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control
		DataGridViewCellStyle1.Font = New System.Drawing.Font("Arial", 9!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
		DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText
		DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
		DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
		DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
		Me.dgvArea.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
		Me.dgvArea.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
		Me.dgvArea.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.clmMemory, Me.clmItem, Me.clmVal})
		Me.dgvArea.Location = New System.Drawing.Point(7, 25)
		Me.dgvArea.MultiSelect = false
		Me.dgvArea.Name = "dgvArea"
		Me.dgvArea.RowHeadersVisible = false
		Me.dgvArea.RowHeadersWidth = 51
		Me.dgvArea.RowTemplate.Height = 21
		Me.dgvArea.Size = New System.Drawing.Size(416, 800)
		Me.dgvArea.TabIndex = 26
		'
		'clmMemory
		'
		Me.clmMemory.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
		DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
		DataGridViewCellStyle2.BackColor = System.Drawing.Color.White
		DataGridViewCellStyle2.ForeColor = System.Drawing.Color.Black
		DataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.White
		DataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.Black
		Me.clmMemory.DefaultCellStyle = DataGridViewCellStyle2
		Me.clmMemory.HeaderText = "Address"
		Me.clmMemory.MinimumWidth = 6
		Me.clmMemory.Name = "clmMemory"
		Me.clmMemory.ReadOnly = true
		Me.clmMemory.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
		Me.clmMemory.Width = 59
		'
		'clmItem
		'
		Me.clmItem.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
		DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
		DataGridViewCellStyle3.BackColor = System.Drawing.Color.White
		DataGridViewCellStyle3.ForeColor = System.Drawing.Color.Black
		DataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.White
		DataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.Black
		Me.clmItem.DefaultCellStyle = DataGridViewCellStyle3
		Me.clmItem.HeaderText = "Item"
		Me.clmItem.MinimumWidth = 6
		Me.clmItem.Name = "clmItem"
		Me.clmItem.ReadOnly = true
		Me.clmItem.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
		'
		'clmVal
		'
		Me.clmVal.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
		DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
		Me.clmVal.DefaultCellStyle = DataGridViewCellStyle4
		Me.clmVal.HeaderText = "Value"
		Me.clmVal.MinimumWidth = 6
		Me.clmVal.Name = "clmVal"
		Me.clmVal.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
		Me.clmVal.Width = 43
		'
		'Timer1
		'
		'
		'btnHex
		'
		Me.btnHex.Location = New System.Drawing.Point(348, 1)
		Me.btnHex.Name = "btnHex"
		Me.btnHex.Size = New System.Drawing.Size(75, 18)
		Me.btnHex.TabIndex = 27
		Me.btnHex.Text = "Dec/Hex"
		Me.btnHex.UseVisualStyleBackColor = true
		'
		'cbTopMost
		'
		Me.cbTopMost.AutoSize = true
		Me.cbTopMost.Location = New System.Drawing.Point(7, 3)
		Me.cbTopMost.Name = "cbTopMost"
		Me.cbTopMost.Size = New System.Drawing.Size(68, 16)
		Me.cbTopMost.TabIndex = 28
		Me.cbTopMost.Text = "TopMost"
		Me.cbTopMost.UseVisualStyleBackColor = true
		'
		'Label1
		'
		Me.Label1.AutoSize = true
		Me.Label1.Location = New System.Drawing.Point(8, 18)
		Me.Label1.Name = "Label1"
		Me.Label1.Size = New System.Drawing.Size(71, 12)
		Me.Label1.TabIndex = 29
		Me.Label1.Text = "p_iBGWOrder"
		'
		'lblBGWOrder
		'
		Me.lblBGWOrder.BackColor = System.Drawing.SystemColors.ControlLightLight
		Me.lblBGWOrder.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		Me.lblBGWOrder.Location = New System.Drawing.Point(121, 15)
		Me.lblBGWOrder.Name = "lblBGWOrder"
		Me.lblBGWOrder.Size = New System.Drawing.Size(57, 18)
		Me.lblBGWOrder.TabIndex = 29
		Me.lblBGWOrder.Text = "0"
		Me.lblBGWOrder.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		'
		'GroupBox1
		'
		Me.GroupBox1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
		Me.GroupBox1.Controls.Add(Me.lblDevErr)
		Me.GroupBox1.Controls.Add(Me.lblCalJob)
		Me.GroupBox1.Controls.Add(Me.lblMeasureState)
		Me.GroupBox1.Controls.Add(Me.Label5)
		Me.GroupBox1.Controls.Add(Me.Label4)
		Me.GroupBox1.Controls.Add(Me.lblBGWOrderNext)
		Me.GroupBox1.Controls.Add(Me.Label2)
		Me.GroupBox1.Controls.Add(Me.lblBGWOrder)
		Me.GroupBox1.Controls.Add(Me.Label3)
		Me.GroupBox1.Controls.Add(Me.Label1)
		Me.GroupBox1.Location = New System.Drawing.Point(429, 21)
		Me.GroupBox1.Name = "GroupBox1"
		Me.GroupBox1.Size = New System.Drawing.Size(191, 261)
		Me.GroupBox1.TabIndex = 30
		Me.GroupBox1.TabStop = false
		'
		'lblCalJob
		'
		Me.lblCalJob.BackColor = System.Drawing.SystemColors.ControlLightLight
		Me.lblCalJob.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		Me.lblCalJob.Location = New System.Drawing.Point(121, 73)
		Me.lblCalJob.Name = "lblCalJob"
		Me.lblCalJob.Size = New System.Drawing.Size(57, 18)
		Me.lblCalJob.TabIndex = 29
		Me.lblCalJob.Text = "0"
		Me.lblCalJob.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		'
		'lblMeasureState
		'
		Me.lblMeasureState.BackColor = System.Drawing.SystemColors.ControlLightLight
		Me.lblMeasureState.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		Me.lblMeasureState.Location = New System.Drawing.Point(121, 54)
		Me.lblMeasureState.Name = "lblMeasureState"
		Me.lblMeasureState.Size = New System.Drawing.Size(57, 18)
		Me.lblMeasureState.TabIndex = 29
		Me.lblMeasureState.Text = "0"
		Me.lblMeasureState.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		'
		'Label4
		'
		Me.Label4.AutoSize = true
		Me.Label4.Location = New System.Drawing.Point(8, 77)
		Me.Label4.Name = "Label4"
		Me.Label4.Size = New System.Drawing.Size(57, 12)
		Me.Label4.TabIndex = 29
		Me.Label4.Text = "m_iCalJob"
		'
		'lblBGWOrderNext
		'
		Me.lblBGWOrderNext.BackColor = System.Drawing.SystemColors.ControlLightLight
		Me.lblBGWOrderNext.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		Me.lblBGWOrderNext.Location = New System.Drawing.Point(121, 35)
		Me.lblBGWOrderNext.Name = "lblBGWOrderNext"
		Me.lblBGWOrderNext.Size = New System.Drawing.Size(57, 18)
		Me.lblBGWOrderNext.TabIndex = 29
		Me.lblBGWOrderNext.Text = "0"
		Me.lblBGWOrderNext.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		'
		'Label2
		'
		Me.Label2.AutoSize = true
		Me.Label2.Location = New System.Drawing.Point(8, 58)
		Me.Label2.Name = "Label2"
		Me.Label2.Size = New System.Drawing.Size(85, 12)
		Me.Label2.TabIndex = 29
		Me.Label2.Text = "p_MeasureState"
		'
		'Label3
		'
		Me.Label3.AutoSize = true
		Me.Label3.Location = New System.Drawing.Point(8, 39)
		Me.Label3.Name = "Label3"
		Me.Label3.Size = New System.Drawing.Size(95, 12)
		Me.Label3.TabIndex = 29
		Me.Label3.Text = "p_iBGWOrderNext"
		'
		'lblDevErr
		'
		Me.lblDevErr.BackColor = System.Drawing.SystemColors.ControlLightLight
		Me.lblDevErr.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		Me.lblDevErr.Location = New System.Drawing.Point(121, 122)
		Me.lblDevErr.Name = "lblDevErr"
		Me.lblDevErr.Size = New System.Drawing.Size(57, 18)
		Me.lblDevErr.TabIndex = 29
		Me.lblDevErr.Text = "0"
		Me.lblDevErr.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		'
		'Label5
		'
		Me.Label5.AutoSize = true
		Me.Label5.Location = New System.Drawing.Point(8, 125)
		Me.Label5.Name = "Label5"
		Me.Label5.Size = New System.Drawing.Size(63, 12)
		Me.Label5.TabIndex = 29
		Me.Label5.Text = "p_iDevError"
		'
		'frmDebug
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6!, 12!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(626, 845)
		Me.Controls.Add(Me.GroupBox1)
		Me.Controls.Add(Me.cbTopMost)
		Me.Controls.Add(Me.btnHex)
		Me.Controls.Add(Me.dgvArea)
		Me.Name = "frmDebug"
		Me.Text = "Debug Information"
		CType(Me.dgvArea,System.ComponentModel.ISupportInitialize).EndInit
		Me.GroupBox1.ResumeLayout(false)
		Me.GroupBox1.PerformLayout
		Me.ResumeLayout(false)
		Me.PerformLayout

End Sub

	Friend WithEvents dgvArea As DataGridView
	Friend WithEvents clmMemory As DataGridViewTextBoxColumn
	Friend WithEvents clmItem As DataGridViewTextBoxColumn
	Friend WithEvents clmVal As DataGridViewTextBoxColumn
	Friend WithEvents Timer1 As Timer
	Friend WithEvents btnHex As Button
	Friend WithEvents cbTopMost As CheckBox
	Friend WithEvents Label1 As Label
	Friend WithEvents lblBGWOrder As Label
	Friend WithEvents GroupBox1 As GroupBox
	Friend WithEvents lblBGWOrderNext As Label
	Friend WithEvents Label3 As Label
	Friend WithEvents lblMeasureState As Label
	Friend WithEvents Label2 As Label
	Friend WithEvents lblCalJob As Label
	Friend WithEvents Label4 As Label
	Friend WithEvents lblDevErr As Label
	Friend WithEvents Label5 As Label
End Class
