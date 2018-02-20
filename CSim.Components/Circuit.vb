Imports System.Threading
Imports System.Windows.Forms
Imports System.Drawing
Imports System.Drawing.Drawing2D

Public Class Circuit
    Implements IDisposable
    Public Property Board As Board
    Public Property Running As Boolean
    Public Property WaitHandle As ManualResetEvent
    Public Property Devices As Dictionary(Of Node, Component)
    Private Property Rate As Integer
    Private Property Elapsed As Integer
    Sub New(Board As Board, Optional Rate As Integer = 30)
        Me.Rate = Rate
        Me.Board = Board
        Me.Running = False
        Me.WaitHandle = New ManualResetEvent(False)
        Me.Devices = New Dictionary(Of Node, Component)
        AddHandler Me.Board.NodeRender, AddressOf Me.Clock
        AddHandler Me.Board.NodeRenderAfter, AddressOf Me.UpdateAfter
        AddHandler Me.Board.NodeRenderBefore, AddressOf Me.UpdateBefore
        AddHandler Me.Board.MouseUp, AddressOf Me.EventMouseClick
    End Sub
    Public Sub Run()
        Call New Thread(AddressOf Me.Clock) With {.IsBackground = True}.Start()
    End Sub
    Public Sub Abort()
        Me.Running = False
    End Sub
    Public Function Create(device As Component, mount As Node) As Component
        If (mount IsNot Nothing AndAlso Not Me.Devices.ContainsKey(mount)) Then
            SyncLock Me.Devices
                Me.Devices.Add(device.Initialize(mount), device)
                Return Me.Devices(mount)
            End SyncLock
        End If
        Return Nothing
    End Function
    Public Function Connect(dev1 As Component, dev2 As Component) As Boolean
        If (Me.Devices.ContainsKey(dev1.Mount) AndAlso Me.Devices.ContainsKey(dev2.Mount)) Then
            dev1.Wires.Add(New Wire(dev1, dev2))
            Return True
        End If
        Return False
    End Function
    Public Function Detach(dev As Component) As Boolean
        If (Me.Devices.ContainsKey(dev.Mount)) Then
            SyncLock Me.Devices
                dev.Detach()
                Me.Devices.Remove(dev.Mount)
                Return True
            End SyncLock
        End If
        Return False
    End Function
    Private Sub Clock()
        Try
            If (Me.Running) Then
                Me.Running = False
                Me.WaitHandle.WaitOne()
            End If
            Me.Running = True
            Do
                If ((Environment.TickCount - Me.Elapsed) >= (1000 / Me.Rate)) Then
                    Using bm As New Bitmap(Me.Board.ClientRectangle.Width, Me.Board.ClientRectangle.Height)
                        Using g As Graphics = Graphics.FromImage(bm)
                            g.DrawRectangle(Pens.Black, Me.Board.BoardRectangle)
                            g.SmoothingMode = SmoothingMode.HighQuality
                            g.InterpolationMode = InterpolationMode.NearestNeighbor
                            g.PixelOffsetMode = PixelOffsetMode.HighSpeed
                            Me.Board.Draw(g)
                            g.DrawString(String.Format("FPS {0}", Me.Rate), Me.Board.Font, Brushes.White, 5, 5)
                            If (Me.Board.Selected IsNot Nothing AndAlso Me.Devices.ContainsKey(Me.Board.Selected)) Then
                                Dim d As Component = Me.Devices(Me.Board.Selected)
                                g.DrawString(String.Format("SELECTED {0} WIRES {1} LISTENERS {2} OUTPUT {3}",
                                                           d.Name, d.Wires.Count, d.Listeners.Count, d.Output.ToString), Me.Board.Font, Brushes.White, 5, 15)
                            End If
                        End Using
                        Me.Board.BackgroundImage = CType(bm.Clone, Image)
                    End Using
                    Me.Elapsed = Environment.TickCount
                End If
            Loop While Me.Running
        Catch ex As Exception
            Me.Running = False
        Finally
            Me.WaitHandle.Set()
            Me.WaitHandle.Reset()
        End Try
    End Sub
    Private Sub EventMouseClick(sender As Object, e As MouseEventArgs)
        If (Me.Board.Selected IsNot Nothing AndAlso Me.Devices.ContainsKey(Me.Board.Selected)) Then
            Me.Devices(Me.Board.Selected).EventMouseUp(e)
        End If
    End Sub
    Private Sub Clock(node As Node, gfx As Graphics)
        If (Me.Devices.ContainsKey(node)) Then
            Me.Devices(node).Clock(Me, node, gfx)
        End If
    End Sub
    Private Sub UpdateBefore(node As Node, gfx As Graphics)
        If (Me.Devices.ContainsKey(node)) Then
            Me.Devices(node).Before(Me, node, gfx)
        End If
    End Sub
    Private Sub UpdateAfter(node As Node, gfx As Graphics)
        If (Me.Devices.ContainsKey(node)) Then
            Me.Devices(node).After(Me, node, gfx)
        End If
    End Sub
    Private disposedValue As Boolean
    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                Me.Running = False
                Me.Devices.Clear()
                RemoveHandler Me.Board.NodeRender, AddressOf Me.Clock
                RemoveHandler Me.Board.NodeRenderAfter, AddressOf Me.UpdateAfter
                RemoveHandler Me.Board.NodeRenderBefore, AddressOf Me.UpdateBefore
                RemoveHandler Me.Board.MouseUp, AddressOf Me.EventMouseClick
            End If
        End If
        Me.disposedValue = True
    End Sub
    Public Sub Dispose() Implements IDisposable.Dispose
        Me.Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
End Class
