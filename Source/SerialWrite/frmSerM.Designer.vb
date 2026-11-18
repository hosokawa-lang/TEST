<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmSerM
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
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmSerM))
		Me.Label1 = New System.Windows.Forms.Label()
		Me.Label2 = New System.Windows.Forms.Label()
		Me.nudNode = New System.Windows.Forms.NumericUpDown()
		Me.Label3 = New System.Windows.Forms.Label()
		Me.nudSerial = New System.Windows.Forms.NumericUpDown()
		Me.btnWriteVeri = New System.Windows.Forms.Button()
		Me.lblConnOK = New System.Windows.Forms.Label()
		Me.lblWriteVeriOK = New System.Windows.Forms.Label()
		Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
		Me.CheckBox1 = New System.Windows.Forms.CheckBox()
		Me.tbxIP = New System.Windows.Forms.TextBox()
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		CType(Me.nudNode,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.nudSerial,System.ComponentModel.ISupportInitialize).BeginInit
		Me.SuspendLayout
		'
		'Label1
		'
		Me.Label1.AutoSize = true
		Me.Label1.Location = New System.Drawing.Point(29, 27)
		Me.Label1.Margin = New System.Windows.Forms.Padding(5, 0, 5, 0)
		Me.Label1.Name = "Label1"
		Me.Label1.Size = New System.Drawing.Size(158, 20)
		Me.Label1.TabIndex = 0
		Me.Label1.Text = "SIZUKA IP Address"
		'
		'Label2
		'
		Me.Label2.AutoSize = true
		Me.Label2.Location = New System.Drawing.Point(225, 27)
		Me.Label2.Margin = New System.Windows.Forms.Padding(5, 0, 5, 0)
		Me.Label2.Name = "Label2"
		Me.Label2.Size = New System.Drawing.Size(107, 20)
		Me.Label2.TabIndex = 1
		Me.Label2.Text = "192.168.11."
		'
		'nudNode
		'
		Me.nudNode.Location = New System.Drawing.Point(330, 25)
		Me.nudNode.Maximum = New Decimal(New Integer() {16, 0, 0, 0})
		Me.nudNode.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
		Me.nudNode.Name = "nudNode"
		Me.nudNode.Size = New System.Drawing.Size(72, 28)
		Me.nudNode.TabIndex = 2
		Me.nudNode.Value = New Decimal(New Integer() {1, 0, 0, 0})
		'
		'Label3
		'
		Me.Label3.AutoSize = true
		Me.Label3.Location = New System.Drawing.Point(29, 64)
		Me.Label3.Margin = New System.Windows.Forms.Padding(5, 0, 5, 0)
		Me.Label3.Name = "Label3"
		Me.Label3.Size = New System.Drawing.Size(183, 20)
		Me.Label3.TabIndex = 3
		Me.Label3.Text = "SIZUKA Serial number"
		'
		'nudSerial
		'
		Me.nudSerial.Location = New System.Drawing.Point(330, 62)
		Me.nudSerial.Maximum = New Decimal(New Integer() {999, 0, 0, 0})
		Me.nudSerial.Name = "nudSerial"
		Me.nudSerial.Size = New System.Drawing.Size(72, 28)
		Me.nudSerial.TabIndex = 4
		Me.nudSerial.Value = New Decimal(New Integer() {1, 0, 0, 0})
		'
		'btnWriteVeri
		'
		Me.btnWriteVeri.Location = New System.Drawing.Point(33, 111)
		Me.btnWriteVeri.Name = "btnWriteVeri"
		Me.btnWriteVeri.Size = New System.Drawing.Size(562, 42)
		Me.btnWriteVeri.TabIndex = 5
		Me.btnWriteVeri.Text = "Write and Verify"
		Me.btnWriteVeri.UseVisualStyleBackColor = true
		'
		'lblConnOK
		'
		Me.lblConnOK.AutoSize = true
		Me.lblConnOK.Location = New System.Drawing.Point(425, 27)
		Me.lblConnOK.Name = "lblConnOK"
		Me.lblConnOK.Size = New System.Drawing.Size(0, 20)
		Me.lblConnOK.TabIndex = 6
		'
		'lblWriteVeriOK
		'
		Me.lblWriteVeriOK.AutoSize = true
		Me.lblWriteVeriOK.Location = New System.Drawing.Point(425, 66)
		Me.lblWriteVeriOK.Name = "lblWriteVeriOK"
		Me.lblWriteVeriOK.Size = New System.Drawing.Size(0, 20)
		Me.lblWriteVeriOK.TabIndex = 7
		'
		'Timer1
		'
		'
		'CheckBox1
		'
		Me.CheckBox1.AutoSize = true
		Me.CheckBox1.Location = New System.Drawing.Point(207, 33)
		Me.CheckBox1.Name = "CheckBox1"
		Me.CheckBox1.Size = New System.Drawing.Size(15, 14)
		Me.CheckBox1.TabIndex = 9
		Me.ToolTip1.SetToolTip(Me.CheckBox1, "Fixed /Free change switch")
		Me.CheckBox1.UseVisualStyleBackColor = true
		'
		'tbxIP
		'
		Me.tbxIP.Location = New System.Drawing.Point(229, 24)
		Me.tbxIP.MaxLength = 16
		Me.tbxIP.Name = "tbxIP"
		Me.tbxIP.Size = New System.Drawing.Size(173, 28)
		Me.tbxIP.TabIndex = 10
		Me.tbxIP.Visible = false
		'
		'frmSerM
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(10!, 20!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(634, 160)
		Me.Controls.Add(Me.tbxIP)
		Me.Controls.Add(Me.CheckBox1)
		Me.Controls.Add(Me.lblWriteVeriOK)
		Me.Controls.Add(Me.lblConnOK)
		Me.Controls.Add(Me.btnWriteVeri)
		Me.Controls.Add(Me.nudSerial)
		Me.Controls.Add(Me.Label3)
		Me.Controls.Add(Me.nudNode)
		Me.Controls.Add(Me.Label2)
		Me.Controls.Add(Me.Label1)
		Me.Font = New System.Drawing.Font("Meiryo UI", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.Icon = CType(resources.GetObject("$this.Icon"),System.Drawing.Icon)
		Me.Margin = New System.Windows.Forms.Padding(5)
		Me.MaximizeBox = false
		Me.MinimizeBox = false
		Me.Name = "frmSerM"
		Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
		Me.Text = "SIZUKA Device number WRITE&VERIFY"
		CType(Me.nudNode,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.nudSerial,System.ComponentModel.ISupportInitialize).EndInit
		Me.ResumeLayout(false)
		Me.PerformLayout

End Sub

	Friend WithEvents Label1 As Label
	Friend WithEvents Label2 As Label
	Friend WithEvents nudNode As NumericUpDown
	Friend WithEvents Label3 As Label
	Friend WithEvents nudSerial As NumericUpDown
	Friend WithEvents btnWriteVeri As Button
	Friend WithEvents lblConnOK As Label
	Friend WithEvents lblWriteVeriOK As Label
	Friend WithEvents Timer1 As Timer
	Friend WithEvents CheckBox1 As CheckBox
	Friend WithEvents tbxIP As TextBox
	Friend WithEvents ToolTip1 As ToolTip
End Class
