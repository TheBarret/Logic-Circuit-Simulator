Imports System.Drawing
Imports System.Windows.Forms

Public Class Node
    Public Property Index As Integer
    Public Property Row As Integer
    Public Property Column As Integer
    Public Property Cost As Double
    Public Property Open As Boolean
    Public Property Grid As Board
    Public Property Path As Node
    Public Property BackColor As Color
    Public Property ForeColor As Color
    Public Property Rectangle As Rectangle
    Public Property Neighbours As Dictionary(Of Direction, Node)
    Sub New(Grid As Board, index As Integer, row As Integer, column As Integer, x As Integer, y As Integer, w As Integer, h As Integer)
        Me.Open = True
        Me.Cost = 1
        Me.Row = row
        Me.Column = column
        Me.Index = index
        Me.Grid = Grid
        Me.ForeColor = Grid.ForeColor
        Me.BackColor = Color.DarkGreen
        Me.Rectangle = New Rectangle(x, y, w, h)
        Me.Neighbours = New Dictionary(Of Direction, Node)
    End Sub
    Public Function GetDistance(other As Node) As Double
        Return Math.Sqrt(Math.Pow(Math.Abs(Me.Center.X - other.Center.X), 2) + Math.Pow(Math.Abs(Me.Center.Y - other.Center.Y), 2))
    End Function
    Public Sub Reset()
        Me.Cost = 1
        Me.Path = Nothing
    End Sub
    Public Function F(other As Node) As Double
        Dim distance As Double = Me.GetDistance(other)
        If distance <> -1 AndAlso Cost <> -1 Then Return distance + Me.Cost Else Return -1
    End Function
    Public ReadOnly Property Center As Point
        Get
            Return New Point(Me.Rectangle.Left + Me.Rectangle.Width \ 2, Me.Rectangle.Top + Me.Rectangle.Height \ 2)
        End Get
    End Property
    Public ReadOnly Property Size As Size
        Get
            Return Me.Rectangle.Size
        End Get
    End Property
    Public ReadOnly Property Location As Point
        Get
            Return Me.Rectangle.Location
        End Get
    End Property
    Public ReadOnly Property Font As Font
        Get
            Return Me.Grid.Font
        End Get
    End Property
    Public Overrides Function ToString() As String
        If (Me.Path IsNot Nothing) Then
            Return String.Format("[{0} -> {1} ({2})]", Me.Index, Me.Path.ToString, Me.Cost)
        End If
        Return String.Format("{0} [{1}]", Me.Index, Me.Cost)
    End Function
End Class
