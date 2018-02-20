Imports CSim.Components
Imports System.Drawing

Namespace Addons
    Public Class Led
        Inherits Component
        Sub New()
            Me.Color = Color.LawnGreen
        End Sub
        Public Overrides Sub After(ci As Circuit, node As Node, gfx As Graphics)
            If (Me.Listeners.Any(Function(x) x.Value = Current.On)) Then
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
                Return "Led"
            End Get
        End Property
    End Class
End Namespace