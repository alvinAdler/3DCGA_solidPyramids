<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.pbox_screen = New System.Windows.Forms.PictureBox()
        Me.btn_left = New System.Windows.Forms.Button()
        Me.btn_right = New System.Windows.Forms.Button()
        Me.btn_up = New System.Windows.Forms.Button()
        Me.btn_down = New System.Windows.Forms.Button()
        Me.btn_out = New System.Windows.Forms.Button()
        Me.btn_in = New System.Windows.Forms.Button()
        Me.radbut_translate = New System.Windows.Forms.RadioButton()
        Me.radbut_rotate = New System.Windows.Forms.RadioButton()
        Me.gbox_action = New System.Windows.Forms.GroupBox()
        Me.radbut_tri = New System.Windows.Forms.RadioButton()
        Me.radbut_square = New System.Windows.Forms.RadioButton()
        Me.gbox_object = New System.Windows.Forms.GroupBox()
        Me.lbox_vertri_scs = New System.Windows.Forms.ListBox()
        Me.lbox_vertri_vcs = New System.Windows.Forms.ListBox()
        Me.gbox_polytri = New System.Windows.Forms.GroupBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.tes_label1 = New System.Windows.Forms.Label()
        Me.tes_label2 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.lbox_versquare_scs = New System.Windows.Forms.ListBox()
        Me.lbox_versquare_vcs = New System.Windows.Forms.ListBox()
        CType(Me.pbox_screen, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.gbox_action.SuspendLayout()
        Me.gbox_object.SuspendLayout()
        Me.gbox_polytri.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'pbox_screen
        '
        Me.pbox_screen.BackColor = System.Drawing.Color.White
        Me.pbox_screen.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pbox_screen.Location = New System.Drawing.Point(12, 12)
        Me.pbox_screen.Name = "pbox_screen"
        Me.pbox_screen.Size = New System.Drawing.Size(400, 400)
        Me.pbox_screen.TabIndex = 0
        Me.pbox_screen.TabStop = False
        '
        'btn_left
        '
        Me.btn_left.Location = New System.Drawing.Point(74, 418)
        Me.btn_left.Name = "btn_left"
        Me.btn_left.Size = New System.Drawing.Size(59, 96)
        Me.btn_left.TabIndex = 1
        Me.btn_left.Text = "Left (A)"
        Me.btn_left.UseVisualStyleBackColor = True
        '
        'btn_right
        '
        Me.btn_right.Location = New System.Drawing.Point(261, 418)
        Me.btn_right.Name = "btn_right"
        Me.btn_right.Size = New System.Drawing.Size(59, 96)
        Me.btn_right.TabIndex = 2
        Me.btn_right.Text = "Right (D)"
        Me.btn_right.UseVisualStyleBackColor = True
        '
        'btn_up
        '
        Me.btn_up.Location = New System.Drawing.Point(139, 418)
        Me.btn_up.Name = "btn_up"
        Me.btn_up.Size = New System.Drawing.Size(116, 25)
        Me.btn_up.TabIndex = 3
        Me.btn_up.Text = "Up(W)"
        Me.btn_up.UseVisualStyleBackColor = True
        '
        'btn_down
        '
        Me.btn_down.Location = New System.Drawing.Point(139, 489)
        Me.btn_down.Name = "btn_down"
        Me.btn_down.Size = New System.Drawing.Size(116, 25)
        Me.btn_down.TabIndex = 4
        Me.btn_down.Text = "Down(S)"
        Me.btn_down.UseVisualStyleBackColor = True
        '
        'btn_out
        '
        Me.btn_out.Location = New System.Drawing.Point(139, 444)
        Me.btn_out.Name = "btn_out"
        Me.btn_out.Size = New System.Drawing.Size(53, 44)
        Me.btn_out.TabIndex = 5
        Me.btn_out.Text = "Out(Q)"
        Me.btn_out.UseVisualStyleBackColor = True
        '
        'btn_in
        '
        Me.btn_in.Location = New System.Drawing.Point(198, 444)
        Me.btn_in.Name = "btn_in"
        Me.btn_in.Size = New System.Drawing.Size(57, 44)
        Me.btn_in.TabIndex = 6
        Me.btn_in.Text = "In(E)"
        Me.btn_in.UseVisualStyleBackColor = True
        '
        'radbut_translate
        '
        Me.radbut_translate.AutoSize = True
        Me.radbut_translate.Location = New System.Drawing.Point(11, 26)
        Me.radbut_translate.Name = "radbut_translate"
        Me.radbut_translate.Size = New System.Drawing.Size(69, 17)
        Me.radbut_translate.TabIndex = 7
        Me.radbut_translate.TabStop = True
        Me.radbut_translate.Text = "Translate"
        Me.radbut_translate.UseVisualStyleBackColor = True
        '
        'radbut_rotate
        '
        Me.radbut_rotate.AutoSize = True
        Me.radbut_rotate.Location = New System.Drawing.Point(11, 49)
        Me.radbut_rotate.Name = "radbut_rotate"
        Me.radbut_rotate.Size = New System.Drawing.Size(57, 17)
        Me.radbut_rotate.TabIndex = 8
        Me.radbut_rotate.TabStop = True
        Me.radbut_rotate.Text = "Rotate"
        Me.radbut_rotate.UseVisualStyleBackColor = True
        '
        'gbox_action
        '
        Me.gbox_action.Controls.Add(Me.radbut_rotate)
        Me.gbox_action.Controls.Add(Me.radbut_translate)
        Me.gbox_action.Location = New System.Drawing.Point(418, 12)
        Me.gbox_action.Name = "gbox_action"
        Me.gbox_action.Size = New System.Drawing.Size(97, 72)
        Me.gbox_action.TabIndex = 9
        Me.gbox_action.TabStop = False
        Me.gbox_action.Text = "Action"
        '
        'radbut_tri
        '
        Me.radbut_tri.AutoSize = True
        Me.radbut_tri.Location = New System.Drawing.Point(11, 19)
        Me.radbut_tri.Name = "radbut_tri"
        Me.radbut_tri.Size = New System.Drawing.Size(112, 17)
        Me.radbut_tri.TabIndex = 10
        Me.radbut_tri.TabStop = True
        Me.radbut_tri.Text = "Triangular Pyramid"
        Me.radbut_tri.UseVisualStyleBackColor = True
        '
        'radbut_square
        '
        Me.radbut_square.AutoSize = True
        Me.radbut_square.Location = New System.Drawing.Point(11, 42)
        Me.radbut_square.Name = "radbut_square"
        Me.radbut_square.Size = New System.Drawing.Size(99, 17)
        Me.radbut_square.TabIndex = 11
        Me.radbut_square.TabStop = True
        Me.radbut_square.Text = "Square Pyramid"
        Me.radbut_square.UseVisualStyleBackColor = True
        '
        'gbox_object
        '
        Me.gbox_object.Controls.Add(Me.radbut_square)
        Me.gbox_object.Controls.Add(Me.radbut_tri)
        Me.gbox_object.Location = New System.Drawing.Point(418, 105)
        Me.gbox_object.Name = "gbox_object"
        Me.gbox_object.Size = New System.Drawing.Size(139, 70)
        Me.gbox_object.TabIndex = 12
        Me.gbox_object.TabStop = False
        Me.gbox_object.Text = "Object"
        '
        'lbox_vertri_scs
        '
        Me.lbox_vertri_scs.FormattingEnabled = True
        Me.lbox_vertri_scs.Location = New System.Drawing.Point(229, 36)
        Me.lbox_vertri_scs.Name = "lbox_vertri_scs"
        Me.lbox_vertri_scs.Size = New System.Drawing.Size(206, 82)
        Me.lbox_vertri_scs.TabIndex = 13
        '
        'lbox_vertri_vcs
        '
        Me.lbox_vertri_vcs.FormattingEnabled = True
        Me.lbox_vertri_vcs.Location = New System.Drawing.Point(17, 36)
        Me.lbox_vertri_vcs.Name = "lbox_vertri_vcs"
        Me.lbox_vertri_vcs.Size = New System.Drawing.Size(206, 82)
        Me.lbox_vertri_vcs.TabIndex = 14
        '
        'gbox_polytri
        '
        Me.gbox_polytri.Controls.Add(Me.Label2)
        Me.gbox_polytri.Controls.Add(Me.Label1)
        Me.gbox_polytri.Controls.Add(Me.lbox_vertri_scs)
        Me.gbox_polytri.Controls.Add(Me.lbox_vertri_vcs)
        Me.gbox_polytri.Location = New System.Drawing.Point(418, 197)
        Me.gbox_polytri.Name = "gbox_polytri"
        Me.gbox_polytri.Size = New System.Drawing.Size(450, 124)
        Me.gbox_polytri.TabIndex = 15
        Me.gbox_polytri.TabStop = False
        Me.gbox_polytri.Text = "Triangular Pyramid"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(14, 20)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(121, 13)
        Me.Label2.TabIndex = 17
        Me.Label2.Text = "View Coordinate System"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(226, 20)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(132, 13)
        Me.Label1.TabIndex = 16
        Me.Label1.Text = "Screen Coordinate System"
        '
        'tes_label1
        '
        Me.tes_label1.AutoSize = True
        Me.tes_label1.Location = New System.Drawing.Point(769, 38)
        Me.tes_label1.Name = "tes_label1"
        Me.tes_label1.Size = New System.Drawing.Size(39, 13)
        Me.tes_label1.TabIndex = 16
        Me.tes_label1.Text = "Label3"
        '
        'tes_label2
        '
        Me.tes_label2.AutoSize = True
        Me.tes_label2.Location = New System.Drawing.Point(769, 80)
        Me.tes_label2.Name = "tes_label2"
        Me.tes_label2.Size = New System.Drawing.Size(39, 13)
        Me.tes_label2.TabIndex = 17
        Me.tes_label2.Text = "Label4"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(724, 38)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(20, 13)
        Me.Label5.TabIndex = 18
        Me.Label5.Text = "S1"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(724, 80)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(20, 13)
        Me.Label6.TabIndex = 19
        Me.Label6.Text = "S2"
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.Label3)
        Me.GroupBox1.Controls.Add(Me.Label4)
        Me.GroupBox1.Controls.Add(Me.lbox_versquare_scs)
        Me.GroupBox1.Controls.Add(Me.lbox_versquare_vcs)
        Me.GroupBox1.Location = New System.Drawing.Point(418, 327)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(450, 124)
        Me.GroupBox1.TabIndex = 18
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Square Pyramid"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(14, 20)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(121, 13)
        Me.Label3.TabIndex = 17
        Me.Label3.Text = "View Coordinate System"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(226, 20)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(132, 13)
        Me.Label4.TabIndex = 16
        Me.Label4.Text = "Screen Coordinate System"
        '
        'lbox_versquare_scs
        '
        Me.lbox_versquare_scs.FormattingEnabled = True
        Me.lbox_versquare_scs.Location = New System.Drawing.Point(229, 36)
        Me.lbox_versquare_scs.Name = "lbox_versquare_scs"
        Me.lbox_versquare_scs.Size = New System.Drawing.Size(206, 82)
        Me.lbox_versquare_scs.TabIndex = 13
        '
        'lbox_versquare_vcs
        '
        Me.lbox_versquare_vcs.FormattingEnabled = True
        Me.lbox_versquare_vcs.Location = New System.Drawing.Point(17, 36)
        Me.lbox_versquare_vcs.Name = "lbox_versquare_vcs"
        Me.lbox_versquare_vcs.Size = New System.Drawing.Size(206, 82)
        Me.lbox_versquare_vcs.TabIndex = 14
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(878, 526)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.tes_label2)
        Me.Controls.Add(Me.tes_label1)
        Me.Controls.Add(Me.gbox_polytri)
        Me.Controls.Add(Me.gbox_object)
        Me.Controls.Add(Me.gbox_action)
        Me.Controls.Add(Me.btn_in)
        Me.Controls.Add(Me.btn_out)
        Me.Controls.Add(Me.btn_down)
        Me.Controls.Add(Me.btn_up)
        Me.Controls.Add(Me.btn_right)
        Me.Controls.Add(Me.btn_left)
        Me.Controls.Add(Me.pbox_screen)
        Me.KeyPreview = True
        Me.Name = "Form1"
        Me.Text = "Form1"
        CType(Me.pbox_screen, System.ComponentModel.ISupportInitialize).EndInit()
        Me.gbox_action.ResumeLayout(False)
        Me.gbox_action.PerformLayout()
        Me.gbox_object.ResumeLayout(False)
        Me.gbox_object.PerformLayout()
        Me.gbox_polytri.ResumeLayout(False)
        Me.gbox_polytri.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents pbox_screen As PictureBox
    Friend WithEvents btn_left As Button
    Friend WithEvents btn_right As Button
    Friend WithEvents btn_up As Button
    Friend WithEvents btn_down As Button
    Friend WithEvents btn_out As Button
    Friend WithEvents btn_in As Button
    Friend WithEvents radbut_translate As RadioButton
    Friend WithEvents radbut_rotate As RadioButton
    Friend WithEvents gbox_action As GroupBox
    Friend WithEvents radbut_tri As RadioButton
    Friend WithEvents radbut_square As RadioButton
    Friend WithEvents gbox_object As GroupBox
    Friend WithEvents lbox_vertri_scs As ListBox
    Friend WithEvents lbox_vertri_vcs As ListBox
    Friend WithEvents gbox_polytri As GroupBox
    Friend WithEvents Label2 As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents tes_label1 As Label
    Friend WithEvents tes_label2 As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents Label6 As Label
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents Label3 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents lbox_versquare_scs As ListBox
    Friend WithEvents lbox_versquare_vcs As ListBox
End Class
