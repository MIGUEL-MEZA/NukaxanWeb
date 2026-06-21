Public Class WSOptimizerP_ResponseModel
    Public Property CveParametro As Integer
    Public Property Parametro As String
    Public Property Optimizer() As List(Of WSOptimizerModel)
    Public Property Data() As New List(Of WSOptimizerP_TablaModel)
    Public Property Resultado() As WSOptimizerP_ResultadoModel
End Class
