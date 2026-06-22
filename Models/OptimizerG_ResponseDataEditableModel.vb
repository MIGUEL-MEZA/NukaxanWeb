Public Class OptimizerG_ResponseDataEditableModel
    Public Property NoVariable As Integer
    Public Property Variable As String = ""
    Public Property Posicion As Integer
    Public Property MostrarCliente As String = ""
    Public Property Etapas As New List(Of OptimizerG_EtapaEditableModel)
End Class
