Imports CSim.Components
Imports System.Drawing

Namespace Addons
    Public Class Gate
        Inherits Component
        Public Enum GateType
            [Not]
            [And]
            [Or]
            [Xor]
        End Enum
        Public Property Type As GateType
        Sub New(Type As GateType)
            Me.Type = Type
            Me.Color = Color.Blue
        End Sub
        Public Overrides Sub Clock(ci As Circuit, node As Node, gfx As Graphics)
            If (Me.Listeners.Count = 2) Then
                Dim left As Wire = Me.Listeners.First
                Dim right As Wire = Me.Listeners.Last
                Select Case Me.Type
                    Case GateType.And
                        If (left.Value = Current.On And right.Value = Current.On) Then
                            Me.Output = Current.On
                        Else
                            Me.Output = Current.Off
                        End If
                    Case GateType.Or
                        If (left.Value = Current.On Or right.Value = Current.On) Then
                            Me.Output = Current.On
                        Else
                            Me.Output = Current.Off
                        End If
                    Case GateType.Xor
                        If (left.Value = Current.On Xor right.Value = Current.On) Then
                            Me.Output = Current.On
                        Else
                            Me.Output = Current.Off
                        End If
                End Select
            ElseIf (Me.Listeners.Count = 1) Then
                Dim left As Wire = Me.Listeners.First
                If (Me.Type = GateType.Not) Then
                    If (left.Value = Current.On) Then
                        Me.Output = Current.Off
                    Else
                        Me.Output = Current.On
                    End If
                End If
            Else
                Me.Output = Current.Off
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
                Return String.Format("Gate[{0}]", Me.Type.ToString)
            End Get
        End Property
    End Class
End Namespace