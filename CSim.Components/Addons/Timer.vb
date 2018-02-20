Imports CSim.Components
Imports System.Drawing

Namespace Addons
    Public Class Timer
        Inherits Component
        Private Property Rate As Integer
        Private Property Elapsed As Integer
        Sub New(rate As Integer)
            Me.Rate = rate
            Me.Color = Color.DarkOrange
        End Sub
        Public Overrides Sub Clock(ci As Circuit, node As Node, gfx As Graphics)
            If ((Environment.TickCount - Me.Elapsed) >= (1000 / Me.Rate)) Then
                Me.Output = If(Me.Output = Current.Off, Current.On, Current.Off)
                Me.Elapsed = Environment.TickCount
            End If
            MyBase.Clock(ci, node, gfx)
        End Sub
        Public Overrides Sub After(ci As Circuit, node As Node, gfx As Graphics)
            If (Me.Output = Current.On) Then
                gfx.FillEllipse(Brushes.Red, node.Center.X - 3, node.Center.Y - 3, 6, 6)
                gfx.DrawEllipse(Pens.Black, node.Center.X - 3, node.Center.Y - 3, 6, 6)
            Else
                gfx.FillEllipse(Brushes.LightYellow, node.Center.X - 3, node.Center.Y - 3, 6, 6)
                gfx.DrawEllipse(Pens.Black, node.Center.X - 3, node.Center.Y - 3, 6, 6)
            End If
            MyBase.After(ci, node, gfx)
        End Sub
        Public Overrides ReadOnly Property Name As String
            Get
                Return "Timer"
            End Get
        End Property
    End Class
End Namespace