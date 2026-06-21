Public Class WSOptimizerG_PlanA_RequestModel
    Public Property CvePlan() As Integer
    Public Property UsuAct As String
    Public Property CveReferencia() As Integer
    Public Property CveParametro() As Integer
    Public Property NoGallinas() As Integer = 0
    Public Property NoPollitas() As Integer = 0
    Public Property PrecioVenta() As Double = 0
    Public Property MasaHuevoTotal() As Double = 0
    Public Property ConsumoAlimento() As Double = 0
    Public Property Productos() As List(Of WSOptimizerG_PlanA_RequestEtapasModel)
End Class
