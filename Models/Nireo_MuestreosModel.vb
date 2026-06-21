Public Class Nireo_MuestreosModel
    Public Property CveMuestra As Int64 = 0
    Public Property CveSesion As Int64 = 0
    Public Property CveEquipo As Integer = 0
    Public Property CodCliente As String = ""
    Public Property CveProducto As Integer = 0
    Public Property Identificacion As String = ""
    Public Property FecMuestreo As String = ""
    Public Property NumMuestreos As Integer = 0
    Public Property Referencia As String = ""
    Public Property Lote As String = ""
    Public Property CveOrigen As Integer = 0
    Public Property CveProveedor As Integer = 0
    Public Property Nota As String = ""

    Public Property NomCliente As String = ""
    Public Property Serie As String = ""
    Public Property NomProducto As String = ""
    Public Property CveCategoriaP As Integer = 0
    Public Property NomCategoriaP As String = ""
    Public Property NomOrigen As String = ""
    Public Property NomProveedor As String = ""

    'Bitacora
    Public Property CveEstatus As Integer = 0
    Public Property NomEstatus As String = ""
    Public Property FecAlta As String = ""
    Public Property UsuAlta As String = ""
    Public Property NomUsuAlta As String = ""
End Class
