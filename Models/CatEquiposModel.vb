Public Class CatEquiposModel
    Public Property CveEquipo As Integer = 0
    Public Property NomEquipo As String = ""
    Public Property Marca As String = ""
    Public Property Serie As String = ""
    Public Property Modelo As String = ""
    Public Property NoParte As String = ""
    Public Property CveCategoria As Integer = 0
    Public Property CveProveedor As Integer = 0
    Public Property FechaCompra As String = ""
    Public Property Costo As Double = 0
    Public Property CveMoneda As Integer = 0

    Public Property NomCategoria As String = ""
    Public Property NomProveedor As String = ""
    Public Property NomMoneda As String = ""


    'Bitacora
    Public Property CveEstatus As Integer = 0
    Public Property NomEstatus As String = ""
    Public Property FecAct As String = ""
    Public Property UsuAct As Int64 = 0
    Public Property NomUsuAct As String = ""

End Class
