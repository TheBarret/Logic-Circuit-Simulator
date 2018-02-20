Imports System.Drawing

Namespace Addons
    Public Class Switch
        Inherits Component
        Public Overrides Sub EventMouseUp(e As Windows.Forms.MouseEventArgs)
            If (Me.Output = Current.Off) Then
                Me.Output = Current.On
            Else
                Me.Output = Current.Off
            End If
            MyBase.EventMouseUp(e)
        End Sub
        Public Overrides ReadOnly Property Texture As Bitmap
            Get
                If (Me.Output = Current.On) Then
                    Return My.Resources.switch_on
                Else
                    Return My.Resources.switch_off
                End If
            End Get
        End Property
        Public Overrides ReadOnly Property Name As String
            Get
                Return "Switch"
            End Get
        End Property
    End Class
End Namespace