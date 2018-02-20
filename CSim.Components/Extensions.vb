Imports System.Drawing
Imports System.Drawing.Drawing2D

Public Module Extensions
    <Runtime.CompilerServices.Extension()>
    Public Function Resize(bm As Bitmap, size As Size) As Bitmap
        Return Extensions.Resize(bm, size.Width, size.Height)
    End Function
    <Runtime.CompilerServices.Extension()>
    Public Function Resize(bm As Bitmap, width As Integer, height As Integer) As Bitmap
        Return New Bitmap(bm, width, height)
    End Function
    <Runtime.CompilerServices.Extension()>
    Public Function Attach(Start As Node, Destination As Node, ByRef Result As List(Of Node)) As Boolean
        Dim open As New List(Of Node) From {Start}
        Dim closed As New List(Of Node)
        Dim current As Node = Start
        While open.Any AndAlso Not closed.Any(Function(x) x Is Destination)
            current = open.First
            open.Remove(current)
            closed.Add(current)
            For Each item As KeyValuePair(Of Direction, Node) In current.Neighbours
                If (Not closed.Contains(item.Value) AndAlso item.Value.Open) Then
                    If (Not open.Contains(item.Value)) Then
                        item.Value.Path = current
                        item.Value.Cost = 1 + item.Value.Path.Cost
                        open.Add(item.Value)
                        open = open.OrderBy(Function(x) x.F(Destination)).ToList
                    End If
                End If
            Next
        End While
        If (Not closed.Any(Function(x) x Is Destination)) Then
            Return False
        Else
            Result = New List(Of Node)
            Result.AddRange(closed.ToPath(current, Start))
            Start.Grid.Reset()
            Return True
        End If
    End Function
    <Runtime.CompilerServices.Extension()>
    Public Sub ToClosed(src As List(Of Node))
        If (src.Count >= 3) Then
            For i As Integer = 1 To src.Count - 2
                src(i).Open = False
            Next
        End If
    End Sub
    <Runtime.CompilerServices.Extension()>
    Public Function ToPath(src As List(Of Node), current As Node, start As Node) As List(Of Node)
        Dim node As Node = src(src.IndexOf(current)), path As New Stack(Of Node)
        Do While node.Path IsNot Nothing
            path.Push(node)
            node = node.Path
        Loop
        path.Push(start)
        Return path.ToList
    End Function
End Module
