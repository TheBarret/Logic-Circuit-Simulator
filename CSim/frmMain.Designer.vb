<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMain
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
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

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.Board = New CSim.Components.Board()
        Me.SuspendLayout()
        '
        'Board
        '
        Me.Board.BackColor = System.Drawing.Color.FromArgb(CType(CType(65, Byte), Integer), CType(CType(105, Byte), Integer), CType(CType(225, Byte), Integer))
        Me.Board.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Board.Font = New System.Drawing.Font("Consolas", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Board.ForeColor = System.Drawing.Color.White
        Me.Board.Location = New System.Drawing.Point(0, 0)
        Me.Board.Name = "Board"
        Me.Board.Neighbour = CSim.Components.Neighbours.All
        Me.Board.NodeHeight = 32
        Me.Board.NodesX = 20
        Me.Board.NodesY = 12
        Me.Board.NodeWidth = 31
        Me.Board.NodeXmin = 32
        Me.Board.NodeYmin = 38
        Me.Board.Selected = Nothing
        Me.Board.Size = New System.Drawing.Size(684, 461)
        Me.Board.TabIndex = 0
        '
        'frmMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(684, 461)
        Me.Controls.Add(Me.Board)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmMain"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "CSim"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Board As CSim.Components.Board
End Class
