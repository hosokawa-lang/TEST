<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmWelcome
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
		Me.btnSave = New System.Windows.Forms.Button()
		Me.lblPort = New System.Windows.Forms.Label()
		Me.txtPort = New System.Windows.Forms.TextBox()
		Me.Label3 = New System.Windows.Forms.Label()
		Me.nudIP = New System.Windows.Forms.NumericUpDown()
		Me.gbxIP = New System.Windows.Forms.GroupBox()
		Me.Label1 = New System.Windows.Forms.Label()
		CType(Me.nudIP,System.ComponentModel.ISupportInitialize).BeginInit
		Me.gbxIP.SuspendLayout
		Me.SuspendLayout
		'
		'btnSave
		'
		Me.btnSave.Font = New System.Drawing.Font("Meiryo UI", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.btnSave.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.btnSave.Location = New System.Drawing.Point(12, 88)
		Me.btnSave.Name = "btnSave"
		Me.btnSave.Size = New System.Drawing.Size(314, 31)
		Me.btnSave.TabIndex = 5
		Me.btnSave.Text = "OK"
		Me.btnSave.UseVisualStyleBackColor = true
		'
		'lblPort
		'
		Me.lblPort.AutoSize = true
		Me.lblPort.Font = New System.Drawing.Font("Meiryo UI", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.lblPort.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.lblPort.Location = New System.Drawing.Point(12, 126)
		Me.lblPort.Name = "lblPort"
		Me.lblPort.Size = New System.Drawing.Size(117, 20)
		Me.lblPort.TabIndex = 7
		Me.lblPort.Text = "Port Number:"
		Me.lblPort.Visible = false
		'
		'txtPort
		'
		Me.txtPort.Font = New System.Drawing.Font("Meiryo UI", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.txtPort.Location = New System.Drawing.Point(286, 123)
		Me.txtPort.Name = "txtPort"
		Me.txtPort.Size = New System.Drawing.Size(62, 28)
		Me.txtPort.TabIndex = 9
		Me.txtPort.Text = "1024"
		Me.txtPort.Visible = false
		'
		'Label3
		'
		Me.Label3.AutoSize = true
		Me.Label3.Font = New System.Drawing.Font("Meiryo UI", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.Label3.ImeMode = System.Windows.Forms.ImeMode.NoControl
		Me.Label3.Location = New System.Drawing.Point(19, 30)
		Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(137, 26)
        Me.Label3.TabIndex = 10
        Me.Label3.Text = "192.168.10."
        '
        'nudIP
        '
        Me.nudIP.Font = New System.Drawing.Font("Meiryo UI", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.nudIP.Location = New System.Drawing.Point(159, 28)
        Me.nudIP.Maximum = New Decimal(New Integer() {16, 0, 0, 0})
        Me.nudIP.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.nudIP.Name = "nudIP"
        Me.nudIP.Size = New System.Drawing.Size(63, 34)
        Me.nudIP.TabIndex = 11
        Me.nudIP.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.nudIP.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'gbxIP
        '
        Me.gbxIP.Controls.Add(Me.Label1)
        Me.gbxIP.Controls.Add(Me.Label3)
        Me.gbxIP.Controls.Add(Me.nudIP)
        Me.gbxIP.Font = New System.Drawing.Font("Meiryo UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.gbxIP.Location = New System.Drawing.Point(12, 12)
        Me.gbxIP.Name = "gbxIP"
        Me.gbxIP.Size = New System.Drawing.Size(317, 70)
        Me.gbxIP.TabIndex = 12
        Me.gbxIP.TabStop = False
        Me.gbxIP.Text = "SIZUKA IP Address"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Meiryo UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Label1.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Label1.Location = New System.Drawing.Point(226, 35)
        Me.Label1.Name = "Label1"
		Me.Label1.Size = New System.Drawing.Size(70, 20)
		Me.Label1.TabIndex = 12
		Me.Label1.Text = "[1 - 16]"
		'
		'frmWelcome
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(8!, 17!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(338, 124)
		Me.Controls.Add(Me.gbxIP)
		Me.Controls.Add(Me.btnSave)
		Me.Controls.Add(Me.txtPort)
		Me.Controls.Add(Me.lblPort)
		Me.Font = New System.Drawing.Font("Meiryo UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.Margin = New System.Windows.Forms.Padding(4)
		Me.MaximizeBox = false
		Me.MinimizeBox = false
		Me.Name = "frmWelcome"
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
		Me.Text = "Connection setting for SIZUKA"
		CType(Me.nudIP,System.ComponentModel.ISupportInitialize).EndInit
		Me.gbxIP.ResumeLayout(false)
		Me.gbxIP.PerformLayout
		Me.ResumeLayout(false)
		Me.PerformLayout

End Sub

	Friend WithEvents btnSave As Button
	Friend WithEvents lblPort As Label
	Friend WithEvents txtPort As TextBox
	Friend WithEvents Label3 As Label
	Friend WithEvents nudIP As NumericUpDown
	Friend WithEvents gbxIP As GroupBox
	Friend WithEvents Label1 As Label
End Class
