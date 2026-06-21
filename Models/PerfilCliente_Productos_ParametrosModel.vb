Public Class PerfilCliente_Productos_ParametrosModel
    Public Property CodCliente As String = ""
    Public Property CveProducto As Integer = 0
    Public Property CveParametro As Integer = 0
    Public Property ValorMin As Double = 0
    Public Property ValorMax As Double = 0
    Public Property ValorEsperado As Double = 0
    Public Property CodParametro As String = ""
    Public Property CodALLIXPa As String = ""
    Public Property NomParametro As String = ""


    Public Property CveTipoP As Integer = 0
    Public Property CveCategoriaP As Integer = 0
    Public Property NomCategoriaP As String = ""
    Public Property NomProducto As String = ""
    Public Property NomTipoP As String = ""


    'Bitacora
    Public Property CveEstatus As Integer = 0
    Public Property NomEstatus As String = ""
    Public Property FecAct As String = ""
    Public Property UsuAct As Int64 = 0
    Public Property NomUsuAct As String = ""
End Class
