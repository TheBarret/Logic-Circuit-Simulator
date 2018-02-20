Imports System.Drawing
Imports System.Drawing.Drawing2D

Public Class Wire
    Public Property Color As Color
    Public Property Value As Current
    Public Property Receiver As Component
    Public Property Transmitter As Component
    Private Property Nodes As List(Of Node)
    Sub New(source As Component, destination As Component)
        Me.Value = Current.Off
        Me.Color = source.Color
        Me.Transmitter = source
        Me.Receiver = destination
        Me.Receiver.Listeners.Add(Me)
        If (Me.Receiver.Mount.Attach(source.Mount, Me.Nodes)) Then
            Me.Nodes.ToClosed()
        Else
            Throw New Exception(String.Format("Unable to attach {0} to {1}, no access point found", Me.Transmitter, Me.Receiver))
        End If
    End Sub
    Public Sub Update(value As Current)
        Me.Value = value
    End Sub
    Public Sub Detach()
        Me.Receiver.Listeners.Remove(Me)
        For Each n As Node In Me.Nodes
            n.Open = True
        Next
    End Sub
    Public Sub Draw(gfx As Graphics)
        Dim points() As Point = Me.Nodes.Select(Function(n) n.Center).ToArray
        If (Me.Nodes.Any AndAlso Me.Nodes.Count >= 4) Then
            gfx.DrawLines(New Pen(Brushes.Silver, 2), points.Take(2).ToArray)
            gfx.DrawLines(New Pen(Me.Transmitter.WireColor, 2), points.Skip(1).Take(points.Count - 2).ToArray)
            gfx.DrawLines(New Pen(Brushes.Silver, 2), points.Skip(points.Count - 2).Take(2).ToArray)
        ElseIf (Me.Nodes.Any AndAlso Me.Nodes.Count <= 3) Then
            gfx.DrawLines(New Pen(Me.Transmitter.WireColor, 2), points.ToArray)
        End If
    End Sub
    Public ReadOnly Property GetPath As Point()
        Get
            Return Me.Nodes.Select(Function(n) n.Center).ToArray
        End Get
    End Property
    Public Overrides Function ToString() As String
        Return String.Format("{0} -> {1}", Me.Transmitter, Me.Receiver)
    End Function
End Class
