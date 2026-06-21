Public Class ProdAves_LotesModel
    Public Property CodCliente As String = ""
    Public Property CveLote As Integer = 0
    Public Property CveEtapa As Integer = 0
    Public Property CodLote As String = ""
    Public Property CveLoteO As Integer = 0
    Public Property CveGranja As Integer = 0
    Public Property CveLineaG As Integer = 0
    Public Property CveGuia As Integer = 0
    Public Property FecNacimiento As String = ""
    Public Property FecRecepcion As String = ""
    Public Property FecCierre As String = ""
    Public Property AvesIniciales As Integer = 0
    Public Property AvesFinales As Integer = 0
    Public Property Edad As Integer = 0
    Public Property NumCasetas As Integer = 0

    Public Property NomCliente As String = ""
    Public Property NomEtapa As String = ""
    Public Property CodLoteO As String = ""
    Public Property NomGranja As String = ""
    Public Property NomLineaG As String = ""
    Public Property NomGuia As String = ""
    Public Property Dependencias As String = ""

    'Bitacora
    Public Property CveEstatus As Integer = 0
    Public Property NomEstatus As String = ""
    Public Property FecAct As String = ""
    Public Property UsuAct As String = ""
    Public Property NomUsuAct As String = ""
End Class
