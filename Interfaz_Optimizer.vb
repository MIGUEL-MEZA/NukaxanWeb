Imports System.Net
Imports System.Net.Http
Imports System.Net.Http.Headers
Imports System.Globalization
Imports System.IO
Imports System.Text
Imports System.Threading.Tasks
Imports Newtonsoft.Json
Imports NukaxanWEB
Imports NukaxanWEB.Libreria

Public Class Interfaz_Optimizer
    Public client = New HttpClient()
    Public path As String
    Public content_type As String = "application/json"
    Public Shared strError As String = ""
    Public WSEstatus As Boolean = False

    'Private WsUsuario As New WSUsuarioModel
    Public Sub New()
        path = ConfigurationManager.AppSettings("WSOptimizer")
        'WsUsuario.email = "mbravo@gponutec.com"
        'WsUsuario.password = "Nukaxan#23"
    End Sub

    Public Async Function GeneraPerfil(ReqBody As WSPerfilN_RequestModel) As Task(Of ResponseModel)
        Dim lst As ResponseModel
        Dim urlPeticion As String = path + "data"
        Dim httpContent = New StringContent(JsonConvert.SerializeObject(ReqBody), Encoding.UTF8, content_type)

        Try
            client.timeout = New TimeSpan(0, 0, 30)
            Dim response As HttpResponseMessage = Await client.PostAsync(urlPeticion, httpContent)
            If (response.StatusCode = System.Net.HttpStatusCode.OK) Then
                WSEstatus = True
                lst = Await response.Content.ReadAsAsync(Of ResponseModel)
            Else
                Dim errorWS As WSErrorModel = Await response.Content.ReadAsAsync(Of WSErrorModel)
                strError = CleanSpecialCharacter(errorWS.Message)
            End If
        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        End Try

        Return lst
    End Function
    Public Async Function GeneraPlan(ReqBody As WSPlanA_RequestModel) As Task(Of WSPlanA_ResponseModel)
        Dim lst As WSPlanA_ResponseModel
        Dim urlPeticion As String = path + "optimizado"
        Dim httpContent = New StringContent(JsonConvert.SerializeObject(ReqBody), Encoding.UTF8, content_type)

        Try
            client.timeout = New TimeSpan(0, 0, 60)
            Dim response As HttpResponseMessage = Await client.PostAsync(urlPeticion, httpContent)
            If (response.StatusCode = System.Net.HttpStatusCode.OK) Then
                WSEstatus = True
                lst = Await response.Content.ReadAsAsync(Of WSPlanA_ResponseModel)
            Else
                'Dim errorWS As WSErrorModel = Await response.Content.ReadAsAsync(Of WSErrorModel)
                strError = CleanSpecialCharacter(response.StatusCode)

            End If
        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        End Try

        Return lst
    End Function

    Public Async Function GeneraFormat(ReqBody As WSOptimizerC_Format_RequestModel) As Task(Of WSOptimizerC_Format_ResponseModel)
        Dim result As WSOptimizerC_Format_ResponseModel = Nothing

        Try
            If String.IsNullOrWhiteSpace(path) Then Throw New Exception("No se encontrÃ³ la configuraciÃ³n del servicio WSOptimizer.")

            strError = ""
            WSEstatus = False

            Dim urlPeticion As String = path.TrimEnd("/"c) + "/template"
            Dim httpContent = New StringContent(JsonConvert.SerializeObject(ReqBody), Encoding.UTF8, content_type)

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 Or SecurityProtocolType.Tls11 Or SecurityProtocolType.Tls
            client.Timeout = New TimeSpan(0, 2, 0)

            Dim response As HttpResponseMessage = Await client.PostAsync(urlPeticion, httpContent)
            Dim responseText As String = Await response.Content.ReadAsStringAsync()

            If response.StatusCode = System.Net.HttpStatusCode.OK Then
                WSEstatus = True
                result = JsonConvert.DeserializeObject(Of WSOptimizerC_Format_ResponseModel)(responseText)
            Else
                strError = CleanSpecialCharacter(GetErrorMessage(responseText, response.StatusCode.ToString()))
            End If
        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        End Try

        Return result
    End Function

    Public Async Function ConsultaFormulasCliente(CodCliente As String, Optional CveEstatus As Integer = 2) As Task(Of WSOptimizerC_ApiResultModel(Of WSOptimizerC_Formulas_ClienteDataModel))
        Dim cliente As String = Uri.EscapeDataString(If(CodCliente, String.Empty).Trim())
        Return Await SendJsonAsync(Of WSOptimizerC_Formulas_ClienteDataModel)(
            HttpMethod.Get,
            "formulas/cliente/" + cliente + "?estatus=" + CveEstatus.ToString(CultureInfo.InvariantCulture))
    End Function

    Public Async Function CargaFormulas(CvePerfilN As Int64, Archivo As Stream, NombreArchivo As String, UsuAct As String) As Task(Of WSOptimizerC_ApiResultModel(Of WSOptimizerC_Formulas_CargaDataModel))
        Dim result As WSOptimizerC_ApiResultModel(Of WSOptimizerC_Formulas_CargaDataModel) = Nothing

        Try
            If Archivo Is Nothing Then Throw New ArgumentNullException(NameOf(Archivo))
            If String.IsNullOrWhiteSpace(NombreArchivo) Then Throw New ArgumentException("Debe indicar el nombre del archivo.", NameOf(NombreArchivo))

            ResetServiceState()
            PrepareClient(New TimeSpan(0, 5, 0))

            Using form As New MultipartFormDataContent()
                Dim fileContent As New StreamContent(Archivo)
                fileContent.Headers.ContentType = New MediaTypeHeaderValue("application/octet-stream")
                form.Add(fileContent, "Archivo", System.IO.Path.GetFileName(NombreArchivo))
                form.Add(New StringContent(If(UsuAct, String.Empty), Encoding.UTF8), "UsuAct")

                Using response As HttpResponseMessage = Await client.PostAsync(BuildUrl("formulas/perfil/" + CvePerfilN.ToString(CultureInfo.InvariantCulture) + "/carga"), form)
                    Dim responseText As String = Await response.Content.ReadAsStringAsync()
                    result = DeserializeApiResult(Of WSOptimizerC_Formulas_CargaDataModel)(responseText)
                    SetServiceResult(response, responseText, If(result Is Nothing, Nothing, result.Message), If(result Is Nothing, -1, result.Code))
                End Using
            End Using
        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        End Try

        Return result
    End Function

    Public Async Function ConsultaReportesFormulas(Filtros As WSOptimizerC_Formulas_ReportesFiltroModel) As Task(Of WSOptimizerC_ApiResultModel(Of WSOptimizerC_Formulas_ReportesDataModel))
        If Filtros Is Nothing Then Filtros = New WSOptimizerC_Formulas_ReportesFiltroModel()

        Dim query As New List(Of String)
        AddQueryValue(query, "codCliente", Filtros.CodCliente)
        AddQueryValue(query, "idPerfil", Filtros.IdPerfil)
        AddQueryValue(query, "fechaInicio", If(Filtros.FechaInicio.HasValue, Filtros.FechaInicio.Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture), Nothing))
        AddQueryValue(query, "fechaFin", If(Filtros.FechaFin.HasValue, Filtros.FechaFin.Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture), Nothing))
        AddQueryValue(query, "usuario", Filtros.Usuario)
        AddQueryValue(query, "estatus", Filtros.Estatus)
        AddQueryValue(query, "pagina", Math.Max(1, Filtros.Pagina))
        AddQueryValue(query, "tamanoPagina", Math.Max(1, Filtros.TamanoPagina))

        Return Await SendJsonAsync(Of WSOptimizerC_Formulas_ReportesDataModel)(HttpMethod.Get, "formulas/reportes?" + String.Join("&", query))
    End Function

    Public Async Function ConsultaReporteFormulas(CvePerfilN As Int64, Optional NumeroProceso As Nullable(Of Integer) = Nothing) As Task(Of WSOptimizerC_ApiResultModel(Of WSOptimizerC_Formulas_ReporteDetalleModel))
        Dim relativeUrl As String = "formulas/perfil/" + CvePerfilN.ToString(CultureInfo.InvariantCulture) + "/reporte" + BuildNumeroProcesoQuery(NumeroProceso)
        Return Await SendJsonAsync(Of WSOptimizerC_Formulas_ReporteDetalleModel)(HttpMethod.Get, relativeUrl)
    End Function

    Public Async Function GeneraHtmlFormula(CvePerfilN As Int64, CodFormula As String, Optional NumeroProceso As Nullable(Of Integer) = Nothing) As Task(Of String)
        Dim result As String = Nothing

        Try
            ResetServiceState()
            PrepareClient(New TimeSpan(0, 2, 0))
            Dim relativeUrl As String = "formulas/perfil/" + CvePerfilN.ToString(CultureInfo.InvariantCulture) + "/formula/" + Uri.EscapeDataString(If(CodFormula, String.Empty).Trim()) + "/html" + BuildNumeroProcesoQuery(NumeroProceso)

            Using response As HttpResponseMessage = Await client.GetAsync(BuildUrl(relativeUrl))
                Dim responseText As String = Await response.Content.ReadAsStringAsync()
                If response.IsSuccessStatusCode Then
                    WSEstatus = True
                    result = responseText
                Else
                    strError = CleanSpecialCharacter(GetErrorMessage(responseText, response.StatusCode.ToString()))
                End If
            End Using
        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        End Try

        Return result
    End Function

    Public Async Function DescargaPdfFormula(CvePerfilN As Int64, CodFormula As String, Optional NumeroProceso As Nullable(Of Integer) = Nothing) As Task(Of WSOptimizerC_ArchivoServicioModel)
        Dim relativeUrl As String = "formulas/perfil/" + CvePerfilN.ToString(CultureInfo.InvariantCulture) + "/formula/" + Uri.EscapeDataString(If(CodFormula, String.Empty).Trim()) + "/pdf" + BuildNumeroProcesoQuery(NumeroProceso)
        Return Await DescargaArchivo(relativeUrl)
    End Function

    Public Async Function DescargaPdfPerfil(CvePerfilN As Int64, Optional NumeroProceso As Nullable(Of Integer) = Nothing) As Task(Of WSOptimizerC_ArchivoServicioModel)
        Dim relativeUrl As String = "formulas/perfil/" + CvePerfilN.ToString(CultureInfo.InvariantCulture) + "/pdf" + BuildNumeroProcesoQuery(NumeroProceso)
        Return Await DescargaArchivo(relativeUrl)
    End Function

    Public Async Function GuardaDictamen(ReqBody As WSOptimizerC_Format_RequestModel) As Task(Of WSOptimizerC_ApiResultModel(Of WSOptimizerC_Formulas_AccionDataModel))
        Return Await SendJsonAsync(Of WSOptimizerC_Formulas_AccionDataModel)(
            HttpMethod.Post,
            "formulas/dictamen",
            ReqBody)
    End Function

    Public Async Function IniciaReproceso(ReqBody As WSOptimizerC_Format_RequestModel) As Task(Of WSOptimizerC_ApiResultModel(Of WSOptimizerC_Formulas_AccionDataModel))
        Return Await SendJsonAsync(Of WSOptimizerC_Formulas_AccionDataModel)(
            HttpMethod.Post,
            "formulas/reproceso",
            ReqBody)
    End Function

    Private Async Function SendJsonAsync(Of T)(Method As HttpMethod, RelativeUrl As String, Optional RequestBody As Object = Nothing) As Task(Of WSOptimizerC_ApiResultModel(Of T))
        Dim result As WSOptimizerC_ApiResultModel(Of T) = Nothing

        Try
            ResetServiceState()
            PrepareClient(New TimeSpan(0, 2, 0))

            Using request As New HttpRequestMessage(Method, BuildUrl(RelativeUrl))
                If RequestBody IsNot Nothing Then
                    request.Content = New StringContent(JsonConvert.SerializeObject(RequestBody), Encoding.UTF8, content_type)
                End If

                Using response As HttpResponseMessage = Await client.SendAsync(request)
                    Dim responseText As String = Await response.Content.ReadAsStringAsync()
                    result = DeserializeApiResult(Of T)(responseText)
                    SetServiceResult(response, responseText, If(result Is Nothing, Nothing, result.Message), If(result Is Nothing, -1, result.Code))
                End Using
            End Using
        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        End Try

        Return result
    End Function

    Private Async Function DescargaArchivo(RelativeUrl As String) As Task(Of WSOptimizerC_ArchivoServicioModel)
        Dim result As WSOptimizerC_ArchivoServicioModel = Nothing

        Try
            ResetServiceState()
            PrepareClient(New TimeSpan(0, 5, 0))

            Using response As HttpResponseMessage = Await client.GetAsync(BuildUrl(RelativeUrl))
                If response.IsSuccessStatusCode Then
                    Dim disposition = response.Content.Headers.ContentDisposition
                    Dim fileName As String = If(disposition Is Nothing, String.Empty, If(disposition.FileNameStar, disposition.FileName))
                    result = New WSOptimizerC_ArchivoServicioModel With {
                        .Contenido = Await response.Content.ReadAsByteArrayAsync(),
                        .NombreArchivo = If(fileName, String.Empty).Trim(ChrW(34)),
                        .TipoContenido = If(response.Content.Headers.ContentType Is Nothing, "application/pdf", response.Content.Headers.ContentType.MediaType)
                    }
                    WSEstatus = True
                Else
                    Dim responseText As String = Await response.Content.ReadAsStringAsync()
                    strError = CleanSpecialCharacter(GetErrorMessage(responseText, response.StatusCode.ToString()))
                End If
            End Using
        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        End Try

        Return result
    End Function

    Private Sub ResetServiceState()
        If String.IsNullOrWhiteSpace(path) Then Throw New Exception("No se encontrÃ³ la configuraciÃ³n del servicio WSOptimizer.")
        strError = String.Empty
        WSEstatus = False
    End Sub

    Private Sub PrepareClient(Timeout As TimeSpan)
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 Or SecurityProtocolType.Tls11 Or SecurityProtocolType.Tls
        client.Timeout = Timeout
    End Sub

    Private Function BuildUrl(RelativeUrl As String) As String
        Return path.TrimEnd("/"c) + "/" + RelativeUrl.TrimStart("/"c)
    End Function

    Private Shared Function DeserializeApiResult(Of T)(ResponseText As String) As WSOptimizerC_ApiResultModel(Of T)
        If String.IsNullOrWhiteSpace(ResponseText) Then Return Nothing
        Return JsonConvert.DeserializeObject(Of WSOptimizerC_ApiResultModel(Of T))(ResponseText)
    End Function

    Private Sub SetServiceResult(Response As HttpResponseMessage, ResponseText As String, Message As String, Code As Integer)
        If Response.IsSuccessStatusCode AndAlso Code = 0 Then
            WSEstatus = True
        Else
            strError = CleanSpecialCharacter(If(String.IsNullOrWhiteSpace(Message), GetErrorMessage(ResponseText, Response.StatusCode.ToString()), Message))
        End If
    End Sub

    Private Shared Sub AddQueryValue(Query As List(Of String), Name As String, Value As Object)
        If Value Is Nothing Then Return
        Dim textValue As String = Convert.ToString(Value, CultureInfo.InvariantCulture)
        If String.IsNullOrWhiteSpace(textValue) Then Return
        Query.Add(Uri.EscapeDataString(Name) + "=" + Uri.EscapeDataString(textValue))
    End Sub

    Private Shared Function BuildNumeroProcesoQuery(NumeroProceso As Nullable(Of Integer)) As String
        If Not NumeroProceso.HasValue Then Return String.Empty
        Return "?numeroProceso=" + NumeroProceso.Value.ToString(CultureInfo.InvariantCulture)
    End Function

    Private Function GetErrorMessage(responseText As String, fallback As String) As String
        If String.IsNullOrWhiteSpace(responseText) Then Return fallback

        Try
            Dim apiError As WSOptimizerC_ApiResultModel(Of Object) = JsonConvert.DeserializeObject(Of WSOptimizerC_ApiResultModel(Of Object))(responseText)
            If apiError IsNot Nothing AndAlso Not String.IsNullOrWhiteSpace(apiError.Message) Then Return apiError.Message
        Catch
        End Try

        Try
            Dim textMessage As String = JsonConvert.DeserializeObject(Of String)(responseText)
            If Not String.IsNullOrWhiteSpace(textMessage) Then Return textMessage
        Catch
        End Try

        Try
            Dim errorResponse As WSOptimizerC_Format_ResponseModel = JsonConvert.DeserializeObject(Of WSOptimizerC_Format_ResponseModel)(responseText)
            If errorResponse IsNot Nothing Then
                If Not String.IsNullOrWhiteSpace(errorResponse.mensaje) Then Return errorResponse.mensaje
                If Not String.IsNullOrWhiteSpace(errorResponse.errorCorreo) Then Return errorResponse.errorCorreo
            End If
        Catch
        End Try

        Return responseText
    End Function

End Class
