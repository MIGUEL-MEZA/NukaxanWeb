Public Class WSOptimizerPModel
    Public Sub New(nombre As String, orden As Short, valor As Double)
        Me.Nombre = nombre
        Me.Orden = orden
        Me.Valor = valor
    End Sub

    Public Property Nombre() As String
    Public Property Orden() As Int16
    Public Property Valor() As Double
End Class
