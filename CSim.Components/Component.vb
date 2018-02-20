Imports System.Drawing
Imports System.Windows.Forms

Public MustInherit Class Component
    Sub New()
        Me.Color = Color.Cyan
        Me.Output = Current.Off
        Me.Font = New Font("Verdana", 7)
        Me.Wires = New List(Of Wire)
        Me.Listeners = New List(Of Wire)
    End Sub
    Public Sub Detach()
        SyncLock Me.Wires
            For Each wire As Wire In Me.Wires
                wire.Detach()
            Next
        End SyncLock
        For Each wire As Wire In Me.Listeners
            SyncLock wire.Transmitter
                wire.Transmitter.Wires.Remove(wire.Transmitter.Wires.Where(Function(x) x.Receiver Is Me).First)
            End SyncLock
        Next
    End Sub
    Public Overridable Function Initialize(node As Node) As Node
        Me.Mount = node
        Return node
    End Function
    Public Overridable Sub Clock(ci As Circuit, node As Node, gfx As Graphics)
        Me.UpdateWires()
        Me.Render(gfx)
    End Sub
    Public Overridable Sub Before(ci As Circuit, node As Node, gfx As Graphics)
        If (Me.Wires.Any) Then Me.Wires.ForEach(Sub(w) w.Render(gfx))
    End Sub
    Public Overridable Sub After(ci As Circuit, node As Node, gfx As Graphics)

    End Sub
    Public Overridable Sub EventMouseUp(e As MouseEventArgs)

    End Sub
    Public Overridable Sub Reset()
        Me.Output = Current.Off
        Me.Listeners.Clear()
    End Sub
    Public Overridable Sub UpdateWires()
        If (Me.Wires.Any) Then
            For Each w As Wire In Me.Wires
                w.Update(Me.Output)
            Next
        End If
    End Sub
    Private Sub Render(gfx As Graphics)
        Dim label As String = String.Format("{0}", Me.Name)
        Dim offset As Integer = CInt(gfx.MeasureString(label, Me.Font).Width / 2)
        gfx.DrawImage(Me.Texture.Resize(Me.Mount.Rectangle.Size), Me.Mount.Location)
        gfx.DrawString(label, Me.Font, Brushes.White, Me.Mount.Center.X - offset, Me.Mount.Location.Y + Me.Mount.Size.Height)
    End Sub
#Region "Properties"
    Public Property Color As Color
    Public Property Mount As Node
    Public Property Output As Current
    Public Property Wires As List(Of Wire)
    Public Property Listeners As List(Of Wire)
    Public Overridable ReadOnly Property Name As String
        Get
            Return String.Empty
        End Get
    End Property
    Public Overridable ReadOnly Property Texture As Bitmap
        Get
            Return My.Resources.Component
        End Get
    End Property
    Public Overrides Function ToString() As String
        Return String.Format("{0}", Me.Name)
    End Function
    Public Overridable Property Font As Font
#End Region
End Class