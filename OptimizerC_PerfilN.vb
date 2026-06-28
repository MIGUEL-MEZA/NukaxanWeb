Imports System.Data
Imports Microsoft.Ajax.Utilities
Imports Newtonsoft.Json
Imports NukaxanWEB
Imports NukaxanWEB.DataBase
Imports NukaxanWEB.Libreria
Public Class OptimizerC_PerfilN
    Public Class PNCapturaModel
        Public Property Etapa As Integer
        Public Property NombreEtapa As String
        Public Property Variable As Integer
        Public Property Descripcion As String
        Public Property Decimales As Integer
        Public Property Referencia As Double
        Public Property Ajuste As Double
        Public Property Comentario As String
        Public Property Mostrar As String
        Public Property CveCategoria As Integer
        Public Property EditarAjuste As String
        Public Property ReporteInterno As String
        Public Property ReporteExterno As String
        Public Property MostrarValores As String = ""
        Public Property EnvioFlujo As String = ""
        Public Property NomCategoria As String = ""
    End Class
    Dim strSQLExe As String = ""
    Public strError As String = ""
    Public Folio As String = 0
    Public Function getSQL(CvePerfilN As Int64, CodUsuario As String) As String
        Dim sb As New StringBuilder
        sb.Append(" DECLARE @CvePerfilN bigint=" + CvePerfilN.ToString)
        sb.Append(" DECLARE @CodUsuario varchar(50)='" + CodUsuario + "'")
        sb.Append(" DECLARE @Estatus int=0")
        sb.Append(" DECLARE @Mensaje varchar(250)=''")
        sb.Append(" EXEC spc_OptimizerC_PerfilNutricional @CvePerfilN,@CodUsuario,@Estatus Output,@Mensaje Output")

        Return sb.ToString
    End Function
    Public Function FindAll(CvePerfilN As Int64, CodUsuario As String) As DataTable
        strError = ""
        Dim dt As DataTable
        Try
            strSQLExe = getSQL(CvePerfilN, CodUsuario)
            dt = execQuery(strSQLExe)

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        End Try

        Return dt
    End Function
    Public Function FindlstAll(CvePerfilN As Int64, CodUsuario As String) As List(Of OptimizerC_PerfilNModel)
        Dim dt As DataTable
        Dim lst As New List(Of OptimizerC_PerfilNModel)
        Try
            dt = FindAll(CvePerfilN, CodUsuario)
            For Each dr As DataRow In dt.Rows
                Dim ObjM As OptimizerC_PerfilNModel = FillModel(dr)
                lst.Add(ObjM)
            Next

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        Finally
            dt = Nothing
        End Try

        Return lst
    End Function
    Public Function FindById(CvePerfilN As Int64, CodUsuario As String) As OptimizerC_PerfilNModel
        Dim dt As DataTable
        Dim ObjM As OptimizerC_PerfilNModel
        Try
            dt = FindAll(CvePerfilN, CodUsuario)
            If Not dt Is Nothing Then ObjM = FillModel(dt.Rows(0))

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        Finally
            dt = Nothing
        End Try

        Return ObjM
    End Function
    Public Function FillModel(dr As DataRow) As OptimizerC_PerfilNModel
        Dim ObjModel As New OptimizerC_PerfilNModel
        ObjModel.CvePerfilN = dr("CvePerfilN")
        ObjModel.CvePlan = dr("CvePlan")
        ObjModel.Folio = dr("Folio")
        ObjModel.FolioR = dr("FolioR")
        ObjModel.CodCliente = dr("CodCliente")
        ObjModel.CveModalidad = dr("CveModalidad")
        ObjModel.Titulo = dr("Titulo")
        ObjModel.CveReferencia = dr("CveReferencia")
        ObjModel.Temperatura = dr("Temperatura")
        ObjModel.EspacioCorral = dr("EspacioCorral")
        ObjModel.RAC = dr("RAC")
        ObjModel.Conclusion = dr("Conclusion")
        ObjModel.NomCliente = dr("NomCliente")
        ObjModel.NomModalidad = dr("NomModalidad")
        ObjModel.NomReferencia = dr("NomReferencia")

        'Bitacora
        ObjModel.CveEstatus = dr("CveEstatus")
        ObjModel.NomEstatus = dr("NomEstatus")
        ObjModel.FecAlta = CDate(dr("FecAlta")).ToString("dd/MM/yyyy HH:mm")
        ObjModel.UsuAlta = dr("UsuAlta")
        ObjModel.NomUsuAlta = dr("NomUsuAlta")
        ObjModel.FecAct = CDate(dr("FecAct")).ToString("dd/MM/yyyy HH:mm")
        ObjModel.UsuAct = dr("UsuAct")
        ObjModel.NomUsuAct = dr("NomUsuAct")

        Return ObjModel
    End Function

    Public Function SaveModel(CvePerfilN As Int64, Valores As String, ValoresE As String, UsuAct As String) As Boolean
        Dim sb As New StringBuilder
        Dim dt As DataTable
        Dim IsResult As Boolean = False
        Folio = "0"
        Try
            sb.Append(" DECLARE @Opcion int=0")
            sb.Append(" DECLARE @CvePerfilN bigint=" + CvePerfilN.ToString)
            sb.Append(" DECLARE @Valores varchar(MAX)='" + Valores + "'")
            sb.Append(" DECLARE @ValoresE varchar(MAX)='" + ValoresE + "'")
            sb.Append(" DECLARE @UsuAct varchar(50)='" + UsuAct + "'")
            sb.Append(" DECLARE @Estatus int=0")
            sb.Append(" DECLARE @Mensaje varchar(250)=''")
            sb.Append(" DECLARE @Id int=0")

            sb.Append(" EXEC spiud_OptimizerC_PerfilNutricional @Opcion,@CvePerfilN,@Valores,@ValoresE,@UsuAct,@Estatus Output,@Mensaje Output,@Id Output")

            dt = execQuery(sb.ToString)
            If Not dt Is Nothing And dt.Rows.Count > 0 Then
                If dt(0)("Estatus").ToString = "0" Then
                    Folio = dt(0)("Id").ToString
                    IsResult = True
                Else
                    Throw New Exception(dt(0)("Mensaje").ToString)
                End If
            End If

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        Finally
            dt = Nothing
        End Try
        Return IsResult
    End Function
    Public Function DeleteModel(CvePerfilN As Int64) As Boolean
        Dim sb As New StringBuilder
        Dim dt As DataTable
        Dim IsResult As Boolean = False

        Try
            sb.Append(" DECLARE @Opcion int=2")
            sb.Append(" DECLARE @CvePerfilN bigint=" + CvePerfilN.ToString)
            sb.Append(" DECLARE @Valores varchar(MAX)=''")
            sb.Append(" DECLARE @ValoresE varchar(MAX)=''")
            sb.Append(" DECLARE @UsuAct varchar(50)=''")
            sb.Append(" DECLARE @Estatus int=0")
            sb.Append(" DECLARE @Mensaje varchar(250)=''")
            sb.Append(" DECLARE @Id int=0")

            sb.Append(" EXEC spiud_OptimizerC_PerfilNutricional @Opcion,@CvePerfilN,@Valores,@ValoresE,@UsuAct,@Estatus Output,@Mensaje Output,@Id Output")


            dt = execQuery(sb.ToString)
            If Not dt Is Nothing And dt.Rows.Count > 0 Then
                If dt(0)("Estatus").ToString = "0" Then
                    IsResult = True
                Else
                    Throw New Exception(dt(0)("Mensaje").ToString)
                End If
            End If

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        Finally
            dt = Nothing
        End Try
        Return IsResult
    End Function



    Public Function ConstruirModeloEditable(Id As Int64) As OptimizerC_ResponseEditableModel
        Dim objResultado As OptimizerC_PerfilN_ResultadoModel = New OptimizerC_PerfilN_Resultado().FindById(Id)
        Return ConstruirModeloEditableDesdeResponseJson(objResultado.Response)
    End Function

    Public Function ObtenerModeloEditable(Id As Int64) As OptimizerC_ResponseEditableModel
        Dim objResultado As OptimizerC_PerfilN_ResultadoModel = New OptimizerC_PerfilN_Resultado().FindById(Id)

        If String.IsNullOrWhiteSpace(objResultado.Response2) Then
            Return ConstruirModeloEditableDesdeResponseJson(objResultado.Response)
        End If

        Try
            Dim modeloEditable As OptimizerC_ResponseEditableModel = JsonConvert.DeserializeObject(Of OptimizerC_ResponseEditableModel)(objResultado.Response2)
            If modeloEditable IsNot Nothing AndAlso modeloEditable.Variables IsNot Nothing AndAlso modeloEditable.Variables.Count > 0 Then
                Return NormalizarModeloEditable(modeloEditable)
            End If
        Catch
        End Try

        Try
            Dim legacyCaptura As List(Of PNCapturaModel) = JsonConvert.DeserializeObject(Of List(Of PNCapturaModel))(objResultado.Response2)
            If legacyCaptura IsNot Nothing AndAlso legacyCaptura.Count > 0 Then
                Dim modeloBase = ConstruirModeloEditableDesdeResponseJson(objResultado.Response)
                Return AplicarAjustes(modeloBase, legacyCaptura)
            End If
        Catch
        End Try

        Return ConstruirModeloEditableDesdeResponseJson(objResultado.Response)
    End Function

    Private Function ConstruirModeloEditableDesdeResponseJson(responseJson As String) As OptimizerC_ResponseEditableModel
        If String.IsNullOrWhiteSpace(responseJson) Then
            Return New OptimizerC_ResponseEditableModel()
        End If

        Dim modelo As OptimizerC_ResponseEditableModel = JsonConvert.DeserializeObject(Of OptimizerC_ResponseEditableModel)(responseJson)
        If modelo Is Nothing Then
            modelo = New OptimizerC_ResponseEditableModel()
        End If

        modelo = NormalizarModeloEditable(modelo)

        For Each variable In modelo.Variables
            For Each etapa In variable.Etapas
                etapa.ValorReferencia = etapa.Valor
                etapa.Motivo = ""
            Next
        Next

        Return modelo
    End Function

    Private Function NormalizarModeloEditable(modelo As OptimizerC_ResponseEditableModel) As OptimizerC_ResponseEditableModel
        If modelo Is Nothing Then modelo = New OptimizerC_ResponseEditableModel()
        If modelo.Variables Is Nothing Then modelo.Variables = New List(Of OptimizerC_ResponseDataEditableModel)()
        modelo.DescripcionTemHum = If(modelo.DescripcionTemHum, "")

        For Each variable In modelo.Variables
            variable.Variable = If(variable.Variable, "")
            variable.MostrarCliente = If(variable.MostrarCliente, "")
            If variable.Etapas Is Nothing Then variable.Etapas = New List(Of OptimizerC_EtapaEditableModel)()
            For Each etapa In variable.Etapas
                etapa.Motivo = If(etapa.Motivo, "")
            Next
        Next

        Return modelo
    End Function

    Public Function AplicarAjustes(modeloBase As OptimizerC_ResponseEditableModel, ajustes As List(Of PNCapturaModel)) As OptimizerC_ResponseEditableModel
        modeloBase = NormalizarModeloEditable(modeloBase)
        If ajustes Is Nothing Then Return modeloBase

        For Each ajuste In ajustes
            Dim variable = modeloBase.Variables.FirstOrDefault(Function(v) v.NoVariable = ajuste.Variable)
            If variable Is Nothing Then Continue For

            Dim etapa = variable.Etapas.FirstOrDefault(Function(e) e.Clave = ajuste.Etapa)
            If etapa Is Nothing Then
                etapa = New OptimizerC_EtapaEditableModel With {
                    .Clave = ajuste.Etapa,
                    .Valor = ajuste.Ajuste,
                    .ValorReferencia = ajuste.Referencia,
                    .Motivo = If(ajuste.Comentario, "")
                }
                variable.Etapas.Add(etapa)
            Else
                etapa.Valor = ajuste.Ajuste
                etapa.ValorReferencia = ajuste.Referencia
                etapa.Motivo = If(ajuste.Comentario, "")
            End If
        Next

        Return modeloBase
    End Function

    Public Function ConstruirModeloCaptura(Id As Int64, CodCliente As String) As List(Of PNCapturaModel)
        Dim modeloEditable As OptimizerC_ResponseEditableModel = ObtenerModeloEditable(Id)
        Return ConstruirModeloCaptura(modeloEditable, Id, CodCliente)
    End Function

    Public Function ConstruirModeloCaptura(modeloEditable As OptimizerC_ResponseEditableModel, Id As Int64, CodCliente As String) As List(Of PNCapturaModel)
        modeloEditable = NormalizarModeloEditable(modeloEditable)

        Dim lstE As List(Of OptimizerC_PerfilN_EtapasModel) = New OptimizerC_PerfilN_Etapas().FindlstAll(CodCliente, Id)
        Dim lstVariables As List(Of OptimizerC_CatVariablesModel) = New OptimizerC_CatVariables().FindlstAll(0)

        Dim resultado As New List(Of PNCapturaModel)
        For Each variable In modeloEditable.Variables.Where(Function(v) v.MostrarCliente = "S").OrderBy(Function(v) v.Posicion)
            Dim infoVar = lstVariables.FirstOrDefault(Function(v) v.CveVariable = variable.NoVariable)
            Dim cveCategoria As Integer = If(infoVar IsNot Nothing, infoVar.CveCategoria, 0)
            Dim nomCategoria As String = If(infoVar IsNot Nothing, infoVar.NomCategoria, "")
            Dim reporteInterno As String = If(infoVar IsNot Nothing, infoVar.ReporteInterno, "N")
            Dim reporteExterno As String = If(infoVar IsNot Nothing, infoVar.ReporteExterno, "N")
            Dim envioFlujo As String = If(infoVar IsNot Nothing, infoVar.EnvioFlujo, "N")
            Dim mostrarValores As String = If(infoVar IsNot Nothing, infoVar.MostrarValores, "")
            Dim nomVariable As String = If(infoVar IsNot Nothing, infoVar.NomVariable, variable.Variable)

            For Each etapa In lstE.Where(Function(e) e.Aplica = "S")
                Dim etapaData = variable.Etapas.FirstOrDefault(Function(x) x.Clave = etapa.CveEtapa)
                Dim valorReferencia As Double = 0
                Dim valorAjuste As Double = 0
                Dim motivo As String = ""

                If etapaData IsNot Nothing Then
                    valorReferencia = etapaData.ValorReferencia
                    valorAjuste = etapaData.Valor
                    motivo = If(etapaData.Motivo, "")
                End If

                resultado.Add(New PNCapturaModel With {
                    .Etapa = etapa.CveEtapa,
                    .NombreEtapa = etapa.NomEtapa,
                    .Variable = variable.NoVariable,
                    .Descripcion = nomVariable,
                    .Decimales = 2,
                    .Referencia = valorReferencia,
                    .Ajuste = valorAjuste,
                    .Comentario = motivo,
                    .Mostrar = variable.MostrarCliente,
                    .CveCategoria = cveCategoria,
                    .EditarAjuste = If(infoVar IsNot Nothing, infoVar.EditarAjuste, "N"),
                    .ReporteInterno = reporteInterno,
                    .ReporteExterno = reporteExterno,
                    .MostrarValores = mostrarValores,
                    .EnvioFlujo = envioFlujo,
                    .NomCategoria = nomCategoria
                })
            Next
        Next

        Return resultado
    End Function
End Class


