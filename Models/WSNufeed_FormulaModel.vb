Public Class WSNufeed_FormulaModel
    Public Property customerName As String
    Public Property formulaCode As String
    Public Property productionNo As Integer
    Public Property optimizationNo As Integer
    Public Property batchSize As Integer
    Public Property comment As String
    Public Property price As Double? = Nothing
    Public Property currency As String
    Public Property dietDate As String
    Public Property rawMaterials As List(Of WSNufeed_Formula_RawMaterialModel)
    Public Property rawMaterialSums As List(Of WSNufeed_Formula_RawMaterialSumsModel)
    Public Property nutrients As List(Of WSNufeed_Formula_NutrientModel)
    Public Property nutrientRatios As List(Of WSNufeed_Formula_NutrientRatiosModel)

End Class
