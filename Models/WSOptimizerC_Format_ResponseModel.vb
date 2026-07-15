Public Class WSOptimizerC_Format_ResponseModel
    Public Property mensaje As String
    Public Property archivo As String
    Public Property ruta As String
    Public Property correoEnviado As Boolean
    Public Property idOperacion As String
    Public Property errorCorreo As String
    Public Property formulas As New List(Of WSOptimizerC_Format_FormulaResponseModel)
End Class
