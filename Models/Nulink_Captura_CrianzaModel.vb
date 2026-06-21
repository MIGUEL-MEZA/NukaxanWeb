Public Class Nulink_Captura_CrianzaModel
    Public Property CodCliente As String = ""
    Public Property CveParvada As Integer = 0
    Public Property CveCaseta As Integer = 0
    Public Property FecCaptura As String = ""
    Public Property CveCiclo As Integer = 0
    Public Property Semana As Integer = 0
    Public Property Dia As Integer = 0
    Public Property PoblacionInicial As Integer = 0
    Public Property PoblacionFinal As Integer = 0
    Public Property AvesMuertas As Integer = 0
    Public Property PesoAve As Double = 0
    Public Property Uniformidad As Double = 0
    Public Property AlimentoServido As Double = 0
    Public Property Cal_AlimentoServidoA As Double = 0
    Public Property Cal_Mortalidad As Double = 0
    Public Property Cal_MortalidadA As Double = 0
    Public Property Cal_Supervivencia As Double = 0
    Public Property Cal_ConsumoAS As Double = 0
    Public Property Cal_ConsumoAcumulado As Double = 0
    Public Property Cal_ConsumoAD As Double = 0
    Public Property Cal_GSP As Double = 0
    Public Property Cal_ConversionA As Double = 0

    Public Property CodParvada As String = ""
    Public Property CodCaseta As String = ""
    Public Property NomCiclo As String = ""

    'Bitacora
    Public Property CveEstatus As Integer = 0
    Public Property NomEstatus As String = ""
    Public Property FecAct As String = ""
    Public Property UsuAct As Int64 = 0
    Public Property NomUsuAct As String = ""
End Class
