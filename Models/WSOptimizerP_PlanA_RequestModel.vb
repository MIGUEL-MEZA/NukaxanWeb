Public Class WSOptimizerP_PlanA_RequestModel
    Public Property CvePlan() As Integer
    Public Property UsuAct As String
    Public Property CveReferencia() As Integer
    Public Property CveParametro() As Integer
    Public Property EdadVenta() As Integer = 0
    Public Property DiasPigmento() As Integer = 0
    Public Property PrecioVenta() As Double
    Public Property Desperdicio() As Double
    Public Property Productos() As List(Of WSOptimizerP_PlanA_RequestEtapasModel)
End Class
