<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMSG
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
		Me.lblMsg = New System.Windows.Forms.Label()
		Me.btn1 = New System.Windows.Forms.Button()
		Me.btn2 = New System.Windows.Forms.Button()
		Me.btn3 = New System.Windows.Forms.Button()
		Me.btn4 = New System.Windows.Forms.Button()
		Me.btn5 = New System.Windows.Forms.Button()
		Me.btn6 = New System.Windows.Forms.Button()
		Me.btn7 = New System.Windows.Forms.Button()
		Me.Panel1 = New System.Windows.Forms.Panel()
		Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
		Me.Panel1.SuspendLayout
		Me.SuspendLayout
		'
		'lblMsg
		'
		Me.lblMsg.Dock = System.Windows.Forms.DockStyle.Top
		Me.lblMsg.Font = New System.Drawing.Font("Meiryo UI", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.lblMsg.Location = New System.Drawing.Point(0, 0)
		Me.lblMsg.Name = "lblMsg"
		Me.lblMsg.Size = New System.Drawing.Size(433, 108)
		Me.lblMsg.TabIndex = 0
		Me.lblMsg.Text = "Label1"
		Me.lblMsg.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		'
		'btn1
		'
		Me.btn1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
		Me.btn1.Font = New System.Drawing.Font("Meiryo UI", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.btn1.Location = New System.Drawing.Point(174, 14)
		Me.btn1.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
		Me.btn1.Name = "btn1"
		Me.btn1.Size = New System.Drawing.Size(90, 32)
		Me.btn1.TabIndex = 1
		Me.btn1.Text = "Button1"
		Me.btn1.UseVisualStyleBackColor = true
		'
		'btn2
		'
		Me.btn2.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
		Me.btn2.Font = New System.Drawing.Font("Meiryo UI", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.btn2.Location = New System.Drawing.Point(120, 14)
		Me.btn2.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
		Me.btn2.Name = "btn2"
		Me.btn2.Size = New System.Drawing.Size(90, 32)
		Me.btn2.TabIndex = 2
		Me.btn2.Text = "Button2"
		Me.btn2.UseVisualStyleBackColor = true
		'
		'btn3
		'
		Me.btn3.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
		Me.btn3.Font = New System.Drawing.Font("Meiryo UI", 9!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.btn3.Location = New System.Drawing.Point(226, 14)
		Me.btn3.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
		Me.btn3.Name = "btn3"
		Me.btn3.Size = New System.Drawing.Size(90, 29)
		Me.btn3.TabIndex = 3
		Me.btn3.Text = "Button3"
		Me.btn3.UseVisualStyleBackColor = true
		'
		'btn4
		'
		Me.btn4.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
		Me.btn4.Font = New System.Drawing.Font("Meiryo UI", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.btn4.Location = New System.Drawing.Point(20, 14)
		Me.btn4.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
		Me.btn4.Name = "btn4"
		Me.btn4.Size = New System.Drawing.Size(90, 32)
		Me.btn4.TabIndex = 4
		Me.btn4.Text = "Button4"
		Me.btn4.UseVisualStyleBackColor = true
		'
		'btn5
		'
		Me.btn5.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
		Me.btn5.Font = New System.Drawing.Font("Meiryo UI", 9!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.btn5.Location = New System.Drawing.Point(122, 14)
		Me.btn5.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
		Me.btn5.Name = "btn5"
		Me.btn5.Size = New System.Drawing.Size(90, 29)
		Me.btn5.TabIndex = 5
		Me.btn5.Text = "Button5"
		Me.btn5.UseVisualStyleBackColor = true
		'
		'btn6
		'
		Me.btn6.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
		Me.btn6.Font = New System.Drawing.Font("Meiryo UI", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.btn6.Location = New System.Drawing.Point(225, 14)
		Me.btn6.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
		Me.btn6.Name = "btn6"
		Me.btn6.Size = New System.Drawing.Size(90, 32)
		Me.btn6.TabIndex = 5
		Me.btn6.Text = "Button6"
		Me.btn6.UseVisualStyleBackColor = true
		'
		'btn7
		'
		Me.btn7.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
		Me.btn7.Font = New System.Drawing.Font("Meiryo UI", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.btn7.Location = New System.Drawing.Point(328, 14)
		Me.btn7.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
		Me.btn7.Name = "btn7"
		Me.btn7.Size = New System.Drawing.Size(90, 32)
		Me.btn7.TabIndex = 6
		Me.btn7.Text = "Button7"
		Me.btn7.UseVisualStyleBackColor = true
		'
		'Panel1
		'
		Me.Panel1.Controls.Add(Me.btn7)
		Me.Panel1.Controls.Add(Me.btn1)
		Me.Panel1.Controls.Add(Me.btn6)
		Me.Panel1.Controls.Add(Me.btn2)
		Me.Panel1.Controls.Add(Me.btn5)
		Me.Panel1.Controls.Add(Me.btn3)
		Me.Panel1.Controls.Add(Me.btn4)
		Me.Panel1.Dock = System.Windows.Forms.DockStyle.Bottom
		Me.Panel1.Location = New System.Drawing.Point(0, 98)
		Me.Panel1.Name = "Panel1"
		Me.Panel1.Size = New System.Drawing.Size(433, 57)
		Me.Panel1.TabIndex = 7
		'
		'Timer1
		'
		'
		'frmMSG
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(7!, 15!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(433, 155)
		Me.Controls.Add(Me.Panel1)
		Me.Controls.Add(Me.lblMsg)
		Me.Font = New System.Drawing.Font("Meiryo UI", 9!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
		Me.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
		Me.MaximizeBox = false
		Me.MinimizeBox = false
		Me.Name = "frmMSG"
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
		Me.Text = "frmMSG"
		Me.Panel1.ResumeLayout(false)
		Me.ResumeLayout(false)

End Sub
    Friend WithEvents lblMsg As System.Windows.Forms.Label
    Friend WithEvents btn1 As System.Windows.Forms.Button
    Friend WithEvents btn2 As System.Windows.Forms.Button
    Friend WithEvents btn3 As System.Windows.Forms.Button
    Friend WithEvents btn4 As System.Windows.Forms.Button
    Friend WithEvents btn5 As System.Windows.Forms.Button
    Friend WithEvents btn6 As System.Windows.Forms.Button
    Friend WithEvents btn7 As System.Windows.Forms.Button
	Friend WithEvents Panel1 As Panel
	Friend WithEvents Timer1 As Timer
End Class
