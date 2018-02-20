Imports CSim.Components
Imports System.Drawing

Namespace Addons
    Public Class Terminal
        Inherits Component
        Public Overrides Sub Clock(ci As Circuit, node As Node, gfx As Graphics)
            Me.Output = If(Me.Listeners.Any(Function(x) x.Value = Current.On), Current.On, Current.Off)
            MyBase.Clock(ci, node, gfx)
        End Sub
        Public Overrides Sub After(ci As Circuit, node As Node, gfx As Graphics)
            gfx.FillRectangle(Brushes.White, node.Center.X - 3, node.Center.Y - 3, 6, 6)
            gfx.DrawRectangle(Pens.Black, node.Center.X - 3, node.Center.Y - 3, 6, 6)
            MyBase.After(ci, node, gfx)
        End Sub
        Public Overrides ReadOnly Property Texture As Bitmap
            Get
                Return My.Resources.Empty
            End Get
        End Property
        Public Overrides ReadOnly Property Name As String
            Get
                Return "Terminal"
            End Get
        End Property
    End Class
End Namespace