Public Class WSOptimizerC_ResponseModel
    Public Property CveParametro As Integer
    Public Property Parametro As String
    Public Property Optimizer() As List(Of WSOptimizerModel)
    Public Property Data() As New List(Of WSOptimizerC_TablaModel)
    Public Property Resultado() As WSOptimizerC_ResultadoModel
End Class
