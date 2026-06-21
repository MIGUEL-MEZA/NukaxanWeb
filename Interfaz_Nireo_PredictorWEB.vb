Imports System.Net
Imports System.Net.Http
Imports System.Threading.Tasks
Imports Newtonsoft.Json
Imports NukaxanWEB
Imports NukaxanWEB.Libreria

Public Class Interfaz_Nireo_PredictorWEB
    Public client = New HttpClient()
    Public path As String
    Public content_type As String = "application/json"
    Public Shared strError As String = ""
    Public WSEstatus As Boolean = False

    'Private WsUsuario As New WSUsuarioModel
    Public Sub New()
        path = ConfigurationManager.AppSettings("WSOptimizer")

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

End Class
