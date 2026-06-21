Public Class UsuarioModel
    Public Property userId As Int64 = 0
    Public Property CodUsuario As String = ""
    Public Property NomUsuario As String = ""
    Public Property Email As String = ""
    Public Property NomPuesto As String = ""
    Public Property NomUbicacion As String = ""
    Public Property NomArea As String = ""
    Public Property NomLider As String = ""
    Public Property CveTipo As String = "I"
    Public Property CvePuesto As Int64 = 0
    Public Property CveUbicacion As Int64 = 0
    Public Property CveArea As Int64 = 0
    Public Property CveLider As Int64 = 0
    Public Property CveLenguaje As Integer = 0
    Public Property CveRol As Integer = 0
    Public Property SegUsuario As String = ""
    Public Property SegPassword As String = ""

    Public Property NomTipo As String = ""
    Public Property NomRol As String = ""

    Public Property TotalRelCte As Integer = 0
    Public Property Dependencias As String = "N"
    Public Property NomCliente As String = "N"

    'Bitacora
    Public Property CveEstatus As Integer = 0
    Public Property NomEstatus As String = ""
    Public Property FecAlta As String = ""
    Public Property UsuAlta As String = ""
    Public Property NomUsuAlta As String = ""
    Public Property FecAct As String = ""
    Public Property UsuAct As String = ""
    Public Property NomUsuAct As String = ""
End Class
