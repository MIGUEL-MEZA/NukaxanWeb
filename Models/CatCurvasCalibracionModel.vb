Public Class CatCurvasCalibracionModel
    Public Property CveCategoriaP As Integer = 0
    Public Property CveParametro As Integer = 0
    Public Property Critico As String = ""
    Public Property MinValor As Double = 0
    Public Property MaxValor As Double = 0
    Public Property Archivo As String = ""

    Public Property NomCategoriaP As String = ""
    Public Property NomParametro As String = ""
    Public Property NomCritico As String = ""

    'Bitacora
    Public Property CveEstatus As Integer = 0
    Public Property NomEstatus As String = ""
    Public Property FecAct As String = ""
    Public Property UsuAct As Int64 = 0
    Public Property NomUsuAct As String = ""

End Class
