Public Class PerfilClienteModel
    Public Property CodCliente As String = ""
    Public Property NomCliente As String = ""
    Public Property Pais As String = ""
    Public Property CodALLIX As String = ""
    Public Property CveOrigen As Integer = 0
    Public Property NomOrigen As String = ""
    Public Property Dependencias As String = "N"

    'Bitacora
    Public Property CveEstatus As Integer = 0
    Public Property NomEstatus As String = ""
    Public Property FecAct As String = ""
    Public Property UsuAct As Int64 = 0
    Public Property NomUsuAct As String = ""
End Class
