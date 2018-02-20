Imports CSim.Components
Imports CSim.Components.Addons.Gate


Public Class frmMain
    Public Property Circuit As Circuit
    Private Sub frmMain_Load(sender As Object, e As EventArgs) Handles Me.Load

        Me.Circuit = New Circuit(Me.Board, 60)


        '// Create half-adder example

        Dim sw1 As Component = Me.Circuit.Create(New Addons.Switch, Me.Board.NodeAt(1, 1))
        Dim sw2 As Component = Me.Circuit.Create(New Addons.Switch, Me.Board.NodeAt(3, 1))

        Dim gate1 As Component = Me.Circuit.Create(New Addons.Gate(GateType.Xor), Me.Board.NodeAt(1, 6))
        Dim led1 As Component = Me.Circuit.Create(New Addons.Led, Me.Board.NodeAt(1, 9))

        Dim gate2 As Component = Me.Circuit.Create(New Addons.Gate(GateType.And), Me.Board.NodeAt(3, 6))
        Dim led2 As Component = Me.Circuit.Create(New Addons.Led, Me.Board.NodeAt(3, 9))

        Me.Circuit.Connect(sw1, gate1)
        Me.Circuit.Connect(sw2, gate1)

        Me.Circuit.Connect(gate1, led1)

        Me.Circuit.Connect(sw1, gate2)
        Me.Circuit.Connect(sw2, gate2)

        Me.Circuit.Connect(gate2, led2)

        '// Create not gate with timer
        Dim t1 As Component = Me.Circuit.Create(New Addons.Timer(1), Me.Board.NodeAt(6, 1))

        Dim gate3 As Component = Me.Circuit.Create(New Addons.Gate(GateType.Not), Me.Board.NodeAt(6, 6))

        Dim led3 As Component = Me.Circuit.Create(New Addons.Led, Me.Board.NodeAt(6, 9))

        Me.Circuit.Connect(t1, gate3)
        Me.Circuit.Connect(gate3, led3)


        Me.Circuit.Run()

        '// Removing components
        'Me.Circuit.Detach(sw1)

    End Sub
    Private Sub frmMain_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        Me.Circuit.Dispose()
    End Sub
End Class
