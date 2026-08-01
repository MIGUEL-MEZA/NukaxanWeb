Imports Newtonsoft.Json

Public Class WSOptimizerC_ApiResultModel(Of T)
    Public Property Code As Integer
    Public Property Message As String
    Public Property Data As T
    Public Property TraceId As String
End Class

Public Class WSOptimizerC_Formulas_ClienteDataModel
    Public Property CodCliente As String
    Public Property Perfiles As New List(Of WSOptimizerC_Formulas_PerfilResumenModel)
End Class

Public Class WSOptimizerC_Formulas_PerfilResumenModel
    Public Property IdPerfil As Int64
    Public Property CodCliente As String
    Public Property Cliente As String
    Public Property Folio As String
    Public Property Titulo As String
    Public Property Estatus As Nullable(Of Integer)
    Public Property Fecha As Nullable(Of DateTime)
    Public Property Usuario As Nullable(Of Int64)
    Public Property CantidadFormulas As Integer
End Class

Public Class WSOptimizerC_Formulas_CargaDataModel
    Public Property IdPerfil As Int64
    Public Property NumeroProceso As Integer
    Public Property Formulas As New List(Of WSOptimizerC_Formulas_CargaResumenModel)
End Class

Public Class WSOptimizerC_Formulas_CargaResumenModel
    Public Property CveEtapa As Integer
    Public Property CodFormulaEnviada As String
    Public Property CodFormulaCarga As String
    Public Property Nombre As String
End Class

Public Class WSOptimizerC_Formulas_ReportesFiltroModel
    Public Property CodCliente As String
    Public Property IdPerfil As Nullable(Of Int64)
    Public Property FechaInicio As Nullable(Of DateTime)
    Public Property FechaFin As Nullable(Of DateTime)
    Public Property Usuario As Nullable(Of Int64)
    Public Property Estatus As Nullable(Of Integer)
    Public Property Pagina As Integer = 1
    Public Property TamanoPagina As Integer = 25
End Class

Public Class WSOptimizerC_Formulas_ReportesDataModel
    Public Property Pagina As Integer
    Public Property TamanoPagina As Integer
    Public Property Total As Integer
    Public Property Registros As New List(Of WSOptimizerC_Formulas_PerfilResumenModel)
End Class

Public Class WSOptimizerC_Formulas_ReporteDetalleModel
    Public Property IdPerfil As Int64
    Public Property CodCliente As String
    Public Property Cliente As String
    Public Property Folio As String
    Public Property Titulo As String
    Public Property NumeroProceso As Integer
    Public Property Formulas As New List(Of WSOptimizerC_Formulas_EtapaModel)
End Class

Public Class WSOptimizerC_Formulas_EtapaModel
    Public Property CveEtapa As Integer
    Public Property CodFormulaEnviada As String
    Public Property CodFormulaCarga As String
    Public Property Nombre As String
    Public Property DatosGenerales As New WSOptimizerC_Formulas_DatosGeneralesModel
    Public Property MateriasPrimas As New List(Of WSOptimizerC_Formulas_MateriaPrimaModel)
    Public Property Nutrientes As New List(Of WSOptimizerC_Formulas_NutrienteModel)
    Public Property SeccionesAdicionales As New Dictionary(Of String, Object)
End Class

Public Class WSOptimizerC_Formulas_DatosGeneralesModel
    Public Property Cantidad As Nullable(Of Decimal)
    Public Property Fecha As String
    Public Property Costo As Nullable(Of Decimal)
    Public Property CamposOriginales As New List(Of String)
End Class

Public Class WSOptimizerC_Formulas_MateriaPrimaModel
    Public Property Orden As Integer
    Public Property RmCode As String
    Public Property Descripcion As String
    Public Property Porcentaje As Nullable(Of Decimal)
    Public Property Kilogramos As Nullable(Of Decimal)
    Public Property CamposOriginales As New List(Of String)
    Public Property CamposAdicionales As New Dictionary(Of String, Object)
End Class

Public Class WSOptimizerC_Formulas_NutrienteModel
    Public Property Orden As Integer
    Public Property Descripcion As String
    Public Property Actual As Nullable(Of Decimal)
    Public Property Unidad As String
    Public Property CamposOriginales As New List(Of String)
    Public Property CamposAdicionales As New Dictionary(Of String, Object)
End Class

Public Class WSOptimizerC_Formulas_AccionDataModel
    Public Property IdPerfil As Int64
    Public Property EstatusPerfil As Nullable(Of Integer)
    Public Property IdOperacion As String
    Public Property CorreoEnviado As Nullable(Of Boolean)
    Public Property ErrorCorreo As String
End Class

Public Class WSOptimizerC_ArchivoServicioModel
    Public Property Contenido As Byte()
    Public Property NombreArchivo As String
    Public Property TipoContenido As String
End Class
