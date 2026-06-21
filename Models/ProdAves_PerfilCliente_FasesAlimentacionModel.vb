Public Class ProdAves_PerfilCliente_FasesAlimentacionModel
    Public Property CodCliente As String = ""
    Public Property CveEtapa As Integer = 0
    Public Property CveFase As Integer = 0
    Public Property NomFase As String = ""
    Public Property EdadIni As Integer = 0
    Public Property EdadFin As Integer = 0
    Public Property NomEtapa As String = ""
    Public Property Dependencias As String = ""

    'Bitacora
    Public Property CveEstatus As Integer = 0
    Public Property NomEstatus As String = ""
    Public Property FecAct As String = ""
    Public Property UsuAct As String = ""
    Public Property NomUsuAct As String = ""
End Class
