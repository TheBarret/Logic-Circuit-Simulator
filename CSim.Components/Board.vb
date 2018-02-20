Imports System.Drawing
Imports System.Windows.Forms
Imports System.ComponentModel

Public Class Board
    Inherits Panel
    Public Event NodeHover(node As Node)
    Public Event NodeSelected(node As Node)
    Public Event NodeRender(node As Node, gfx As Graphics)
    Public Event NodeRenderBefore(node As Node, gfx As Graphics)
    Public Event NodeRenderAfter(node As Node, gfx As Graphics)
    Sub New()
        Me.m_nodesx = 10
        Me.m_nodesy = 10
        Me.DoubleBuffered = True
        Me.Dock = DockStyle.Fill
        Me.ForeColor = Color.White
        Me.BackColor = Color.FromArgb(65, 105, 225)
        Me.Neighbour = Neighbours.Few
        Me.Reinitialize()
    End Sub
    Protected Overrides Sub OnPaint(pe As PaintEventArgs)
        If (Me.DesignMode) Then Me.Draw(pe.Graphics)
        MyBase.OnPaint(pe)
    End Sub
    Protected Overrides Sub OnResize(e As EventArgs)
        Me.Reinitialize()
        MyBase.OnResize(e)
    End Sub
    Protected Overrides Sub OnMouseMove(e As MouseEventArgs)
        Me.UpdateHoverNode(e)
        MyBase.OnMouseMove(e)
    End Sub
    Protected Overrides Sub OnMouseClick(e As MouseEventArgs)
        Me.UpdateSelectedNode(e)
        MyBase.OnMouseClick(e)
    End Sub
    Public Sub Reinitialize()
        Me.NodeWidth = Me.ClientRectangle.Width \ (Me.m_nodesx + 2)
        Me.NodeHeight = Me.ClientRectangle.Height \ (Me.m_nodesy + 2)
        Me.NodeXmin = (Me.ClientRectangle.Width - Me.m_nodesx * Me.NodeWidth) \ 2
        Me.NodeYmin = (Me.ClientRectangle.Height - Me.m_nodesy * Me.NodeHeight) \ 2
        Me.m_nodes = Me.CreateNodes(Me.m_nodesx, Me.m_nodesy)
        Me.Hover = Me.m_nodes(0, 0)
    End Sub
    Public Sub Draw(gfx As Graphics)
        If (Me.InvokeRequired) Then
            Me.Invoke(Sub() Me.Draw(gfx))
        Else
            If (Me.m_nodes.Length > 0) Then
                For row As Integer = 0 To Me.m_nodes.GetUpperBound(0)
                    For column As Integer = 0 To Me.m_nodes.GetUpperBound(1)
                        gfx.FillRectangle(New SolidBrush(Me.m_nodes(row, column).BackColor), Me.m_nodes(row, column).Rectangle)
                    Next
                Next
                If (Not Me.DesignMode) Then
                    Me.UpdateNodeBefore(gfx)
                    Me.UpdateNode(gfx)
                    Me.UpdateNodeAfter(gfx)
                End If
            End If
        End If
    End Sub
    Public Sub Reset()
        For row As Integer = 0 To Me.m_nodes.GetUpperBound(0)
            For column As Integer = 0 To Me.m_nodes.GetUpperBound(1)
                Me.m_nodes(row, column).Reset()
            Next
        Next
    End Sub
    Public Function NodeAt(index As Integer) As Node
        For row As Integer = 0 To Me.m_nodes.GetUpperBound(0)
            For column As Integer = 0 To Me.m_nodes.GetUpperBound(1)
                If (Me.m_nodes(row, column).Index = index) Then
                    Return Me.m_nodes(row, column)
                End If
            Next
        Next
        Return Nothing
    End Function
    Public Function NodeAt(row As Integer, column As Integer) As Node
        If (row >= 0 AndAlso row <= Me.m_nodes.GetUpperBound(0)) Then
            If (column >= 0 AndAlso column <= Me.m_nodes.GetUpperBound(1)) Then
                Return Me.m_nodes(row, column)
            End If
        End If
        Return Nothing
    End Function
    Public Function ContainsNodeWith(position As Point) As Boolean
        Return Me.ContainsNodeWith(position.X, position.Y)
    End Function
    Public Function ContainsNodeWith(x As Integer, y As Integer) As Boolean
        Return Me.NodeWith(x, y) IsNot Nothing
    End Function
    Public Function NodeWith(position As Point) As Node
        Return Me.NodeWith(position.X, position.Y)
    End Function
    Public Function NodeWith(x As Integer, y As Integer) As Node
        Dim value As New Point(x, y)
        For row As Integer = 0 To Me.m_nodes.GetUpperBound(0)
            For column As Integer = 0 To Me.m_nodes.GetUpperBound(1)
                If (Me.m_nodes(row, column).Rectangle.Contains(value)) Then
                    Return Me.m_nodes(row, column)
                End If
            Next
        Next
        Return Nothing
    End Function
    Private Sub UpdateHoverNode(e As MouseEventArgs)
        For row As Integer = 0 To Me.m_nodes.GetUpperBound(0)
            For column As Integer = 0 To Me.m_nodes.GetUpperBound(1)
                If (Me.m_nodes(row, column).Rectangle.Contains(e.Location)) Then
                    If (Me.Hover IsNot Me.m_nodes(row, column)) Then
                        Me.Hover = Me.m_nodes(row, column)
                        Me.Refresh()
                        RaiseEvent NodeHover(Me.Hover)
                        Return
                    End If
                End If
            Next
        Next
    End Sub
    Private Sub UpdateSelectedNode(e As MouseEventArgs)
        If (e.Button = Windows.Forms.MouseButtons.Left) Then
            If (Me.Hover IsNot Nothing AndAlso Me.Selected IsNot Me.Hover) Then
                Me.Selected = Me.Hover
                Me.Refresh()
                RaiseEvent NodeSelected(Me.Hover)
                Return
            End If
        End If
    End Sub
    Private Sub UpdateNodeBefore(gfx As Graphics)
        For row As Integer = 0 To Me.m_nodes.GetUpperBound(0)
            For column As Integer = 0 To Me.m_nodes.GetUpperBound(1)
                RaiseEvent NodeRenderBefore(Me.m_nodes(row, column), gfx)
            Next
        Next
    End Sub
    Private Sub UpdateNode(gfx As Graphics)
        For row As Integer = 0 To Me.m_nodes.GetUpperBound(0)
            For column As Integer = 0 To Me.m_nodes.GetUpperBound(1)
                RaiseEvent NodeRender(Me.m_nodes(row, column), gfx)
            Next
        Next
    End Sub
    Private Sub UpdateNodeAfter(gfx As Graphics)
        For row As Integer = 0 To Me.m_nodes.GetUpperBound(0)
            For column As Integer = 0 To Me.m_nodes.GetUpperBound(1)
                RaiseEvent NodeRenderAfter(Me.m_nodes(row, column), gfx)
            Next
        Next
    End Sub
    Private Function CreateNodes(w As Integer, h As Integer) As Node(,)
        Dim nodes As Node(,) = New Node(h - 1, w - 1) {}, y, x As Integer, index As Integer = 0
        For row As Integer = 0 To h - 1
            y = Me.NodeYmin + Me.NodeHeight * row
            For column As Integer = 0 To w - 1
                x = Me.NodeXmin + Me.NodeWidth * column
                nodes(row, column) = New Node(Me, index, row, column, x, y, Me.NodeWidth, Me.NodeHeight)
                index += 1
            Next
        Next
        For row As Integer = 0 To h - 1
            For column As Integer = 0 To w - 1
                If (row > 0) Then
                    nodes(row, column).Neighbours.Add(Direction.North, nodes(row - 1, column))
                End If
                If (row < h - 1) Then
                    nodes(row, column).Neighbours.Add(Direction.South, nodes(row + 1, column))
                End If
                If (column > 0) Then
                    nodes(row, column).Neighbours.Add(Direction.West, nodes(row, column - 1))
                End If
                If (column < w - 1) Then
                    nodes(row, column).Neighbours.Add(Direction.East, nodes(row, column + 1))
                End If
                If (Me.Neighbour = Neighbours.All) Then
                    If (row > 0 And column < w - 1) Then
                        nodes(row, column).Neighbours.Add(Direction.NorthEast, nodes(row - 1, column + 1))
                    End If
                    If (row > 0 And column > 0) Then
                        nodes(row, column).Neighbours.Add(Direction.NorthWest, nodes(row - 1, column - 1))
                    End If
                    If (row < h - 1 And column < w - 1) Then
                        nodes(row, column).Neighbours.Add(Direction.SouthEast, nodes(row + 1, column + 1))
                    End If
                    If (row < h - 1 And column > 0) Then
                        nodes(row, column).Neighbours.Add(Direction.SouthWest, nodes(row + 1, column - 1))
                    End If
                End If
            Next
        Next
        Return nodes
    End Function
#Region "Properties"
    Private m_nodes(,) As Node
    Private m_nodesx As Integer
    Private m_nodesy As Integer
    Private m_neighbours As Neighbours
    <Browsable(False)> Public Property Hover As Node
    <Browsable(False)> Public Property Selected As Node
    <Browsable(False)> Public Property NodeXmin As Integer
    <Browsable(False)> Public Property NodeYmin As Integer
    <Browsable(False)> Public Property NodeWidth As Integer
    <Browsable(False)> Public Property NodeHeight As Integer
    '// Remark: IDE cannot handle 2d arrays in the editor
    <Browsable(False)> Private Property Nodes As Node(,)
        Get
            Return Me.m_nodes
        End Get
        Set(value As Node(,))
            Me.m_nodes = value
            Me.Reinitialize()
            Me.Invalidate()
        End Set
    End Property
    Public ReadOnly Property BoardRectangle As Rectangle
        Get
            Return New Rectangle(Me.NodeXmin, Me.NodeYmin, Me.NodeWidth * Me.m_nodesx, Me.NodeHeight * Me.m_nodesy)
        End Get
    End Property
    <Browsable(False)> Public ReadOnly Property Count As Integer
        Get
            Return Me.m_nodesx * Me.m_nodesy
        End Get
    End Property
    Public Property NodesX As Integer
        Get
            Return Me.m_nodesx
        End Get
        Set(value As Integer)
            Me.m_nodesx = value
            Me.Reinitialize()
            Me.Invalidate()
        End Set
    End Property
    Public Property NodesY As Integer
        Get
            Return Me.m_nodesy
        End Get
        Set(value As Integer)
            Me.m_nodesy = value
            Me.Reinitialize()
            Me.Invalidate()
        End Set
    End Property
    Public Property Neighbour As Neighbours
        Get
            Return Me.m_neighbours
        End Get
        Set(value As Neighbours)
            Me.m_neighbours = value
            Me.Reinitialize()
            Me.Invalidate()
        End Set
    End Property
#End Region
End Class
