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
    Public Sub Render(gfx As Graphics)
        If (Me.Nodes.Any) Then
            Dim points() As Point = Me.GetPath, lp As Point = points.First
            For i As Integer = 1 To points.Count - 1
                If (i <= 1) Then
                    gfx.DrawLine(New Pen(Brushes.Gray, 2), lp, points(i))
                ElseIf (i >= points.Count - 1) Then
                    gfx.DrawLine(New Pen(Brushes.Gray, 2), lp, points(i))
                Else
                    gfx.DrawLine(New Pen(Me.Color, 2), lp, points(i))
                End If
                lp = points(i)
            Next
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