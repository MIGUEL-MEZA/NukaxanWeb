Public Class Clientes_ProductosModel
    Public Property CodCliente As String = ""
    Public Property CveProducto As Integer = 0
    Public Property NomProducto As String = ""
    Public Property CodProducto As String = ""
    Public Property CveCategoriaP As Integer = 0
    Public Property CodALLIX As String = "N"

    Public Property NomCategoriaP As String = "N"
    Public Property Dependencias As String = "N"
    Public Property DependenciasM As String = "N"

    'Bitacora
    Public Property CveEstatus As Integer = 0
    Public Property NomEstatus As String = ""
    Public Property FecAct As String = ""
    Public Property UsuAct As Int64 = 0
    Public Property NomUsuAct As String = ""

End Class
