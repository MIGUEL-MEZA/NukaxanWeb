Public Class ProdAves_Lotes_Registro_CapturaModel
    Public Property CodCliente As String = ""
    Public Property CveLote As Integer = 0
    Public Property CveLoteR As Integer = 0
    Public Property CveRegistro As Integer = 0
    Public Property FecCaptura As String = ""
    Public Property Edad As Integer = 0
    Public Property NumDia As Integer = 0
    Public Property CveCiclo As Integer = 0
    Public Property AvesMuertas As Integer = 0
    Public Property AjusteAves As Integer = 0
    Public Property PesoAve As Double = 0
    Public Property UniforAve As Double = 0
    Public Property CVAve As Double = 0
    Public Property AlimentoServido As Double = 0
    Public Property AguaServida As Double = 0
    Public Property PesoHuevo As Double = 0
    Public Property UniforHuevo As Double = 0
    Public Property TotalHuevos As Integer = 0

    Public Property NomCaseta As String = ""
    Public Property NomCiclo As String = ""
    Public Property CveFase As Integer = 0
    Public Property NomFase As String = ""
    Public Property NomAlimento As String = ""
    Public Property CostoAlimento As Double = 0

    'Calculos
    Public Property AvesAlojadas As Integer = 0
    Public Property AvesInicial As Integer = 0
    Public Property AvesFinal As Integer = 0
    Public Property Mortalidad As Double = 0
    Public Property MortalidadAc As Double = 0
    Public Property Supervivencia As Double = 0
    Public Property ConsumoAlimAD As Double = 0
    Public Property ConsumoAlimAc As Double = 0
    Public Property ConsumoAgua As Double = 0
    Public Property ConsumoAguaAc As Double = 0
    Public Property HuevoAD As Double = 0
    Public Property HuevoAA As Double = 0
    Public Property HuevoAcAD As Double = 0
    Public Property HuevoAcAA As Double = 0
    Public Property ProduccionAD As Double = 0
    Public Property ProduccionAA As Double = 0
    Public Property MasaHuevoAD As Double = 0
    Public Property MasaHuevoAc As Double = 0
    Public Property MasaHuevoAA As Double = 0
    Public Property ConversionAlimAD As Double = 0
    Public Property ConversionAlimAA As Double = 0
    Public Property CostoProdHuevo As Double = 0
    Public Property UtilidadProdHuevo As Double = 0

    'Bitacora
    Public Property CveEstatus As Integer = 0
    Public Property NomEstatus As String = ""
    Public Property FecAct As String = ""
    Public Property UsuAct As String = ""
    Public Property NomUsuAct As String = ""
End Class
