Public Class OptimizerP_ResponseDataEditableModel
    Public Property NoVariable As Integer
    Public Property Variable As String = ""
    Public Property Posicion As Integer
    Public Property MostrarCliente As String = ""
    Public Property Etapas As New List(Of OptimizerP_EtapaEditableModel)
End Class
