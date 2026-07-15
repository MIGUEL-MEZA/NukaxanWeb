Imports System.Net
Imports System.Net.Http
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

    Private Function GetErrorMessage(responseText As String, fallback As String) As String
        If String.IsNullOrWhiteSpace(responseText) Then Return fallback

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
