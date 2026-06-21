Public Class WSPlanA_RequestModel
    Public Property CvePlan() As Integer
    Public Property UsuAct As String
    Public Property cvePerfil As Integer = 0
    Public Property CveReferencia() As Integer
    Public Property CveParametro() As Integer
    Public Property PrecioVenta() As Double
    Public Property Desperdicio() As Double
    Public Property PesoPromedio() As Double
    Public Property EdadDestete() As Double
    Public Property EdadSalida() As Double
    Public Property EdadVenta() As Double
    Public Property DiasRactopamina() As Double
    Public Property CveEstado() As Integer
    Public Property Temperatura() As Double
    Public Property metrosCerdos() As Double
    Public Property Productos() As List(Of WSPlanA_RequestProductoModel)
End Class
