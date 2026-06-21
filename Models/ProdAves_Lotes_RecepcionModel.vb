Public Class ProdAves_Lotes_RecepcionModel
    Public Property CodCliente As String = ""
    Public Property CveLote As Integer = 0
    Public Property CveLoteR As String = ""
    Public Property CveGranja As Integer = 0
    Public Property CveCaseta As Integer = 0
    Public Property FecRecepcion As String = ""
    Public Property FecCierre As String = ""
    Public Property AvesAlojadas As Integer = 0
    Public Property AvesFinales As Integer = 0
    Public Property PesoInicial As Double = 0
    Public Property Edad As Integer = 0
    Public Property NumDia As Integer = 0
    Public Property NomCaseta As String = ""
    Public Property Dependencias As String = ""

    'Bitacora
    Public Property CveEstatus As Integer = 0
    Public Property NomEstatus As String = ""
    Public Property FecAct As String = ""
    Public Property UsuAct As String = ""
    Public Property NomUsuAct As String = ""
End Class
