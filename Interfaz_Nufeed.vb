Imports System.Net.Http
Imports System.Threading.Tasks
Imports Microsoft.Ajax.Utilities
Imports Newtonsoft.Json
Imports System.Data
Imports NukaxanWEB
Imports NukaxanWEB.DataBase
Imports NukaxanWEB.Libreria

Public Class Interfaz_Nufeed
    Public client = New HttpClient()
    Public path As String
    Public content_type As String = "application/json"
    Public Shared strError As String = ""
    Private WsUsuario As New WSUsuarioModel
    Public Sub New()
        path = ConfigurationManager.AppSettings("WSALLIX")
        WsUsuario.email = "mbravo@gponutec.com"
        WsUsuario.password = "Nukaxan#23"
    End Sub

    Public Async Function GetToken() As Task(Of WSTokenModel)
        Dim result As WSTokenModel
        Dim urlPeticion As String = path + "authentication/authenticate"
        Dim httpContent = New StringContent(JsonConvert.SerializeObject(WsUsuario), Encoding.UTF8, content_type)
        Dim response As HttpResponseMessage = Await client.PostAsync(urlPeticion, httpContent)

        If (response.StatusCode = System.Net.HttpStatusCode.OK) Then
            result = Await response.Content.ReadAsAsync(Of WSTokenModel)()
        Else
            Dim errorWS As WSErrorModel = Await response.Content.ReadAsAsync(Of WSErrorModel)
            strError = CleanSpecialCharacter(errorWS.ExceptionMessage)
        End If
        Return result
    End Function
    Public Async Function UpdateParametros(CodCliente As String, CodeProducto As String, ReqBody As List(Of WSParametrosModel)) As Task(Of Boolean)
        Dim result As Boolean = False
        Dim tmp As WSTokenModel = Await GetToken()
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {tmp.token}")

        Try
            Dim urlPeticion As String = path + "rawmaterial/updateanalysis?customername=" + CodCliente + "&rawmaterialcode=" + CodeProducto
            Dim httpContent = New StringContent(JsonConvert.SerializeObject(ReqBody), Encoding.UTF8, content_type)
            Dim response As HttpResponseMessage = Await client.PostAsync(urlPeticion, httpContent)

            If (response.StatusCode = System.Net.HttpStatusCode.OK) Then
                result = True 'Await response.Content.ReadAsStringAsync()
            Else
                Dim errorWS As WSErrorModel = Await response.Content.ReadAsAsync(Of WSErrorModel)
                strError = CleanSpecialCharacter(errorWS.Message)
            End If
        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        End Try

        Return result
    End Function
    Public Async Function UpdateNutrientes(tmp As WSTokenModel, CodCliente As String, formulaCode As String, ReqBody As WSPerfilN_NutrientesRequestModel) As Task(Of Boolean)
        Dim result As Boolean = False
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {tmp.token}")
        Try
            '    Dim settings = New JsonSerializerSettings With {
            '    .NullValueHandling = NullValueHandling.Ignore,
            '    .DefaultValueHandling = DefaultValueHandling.Ignore,
            '    .Formatting = Formatting.Indented
            '}
            Dim urlPeticion As String = path + "formula/updateconstraints?customername=" + CodCliente + "&formulaCode=" + formulaCode
            Dim httpContent = New StringContent(JsonConvert.SerializeObject(ReqBody), Encoding.UTF8, content_type)
            ' client.timeout = New TimeSpan(0, 0, 120)
            Dim response As HttpResponseMessage = Await client.PostAsync(urlPeticion, httpContent)

            If (response.StatusCode = System.Net.HttpStatusCode.OK) Then
                result = True 'Await response.Content.ReadAsStringAsync()
            Else
                strError = CleanSpecialCharacter(response.StatusCode)
            End If
        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        End Try

        Return result
    End Function
    Public Async Function GetPerfilMP(CodCliente As String, CodALLIXP As String) As Task(Of List(Of WSNufeed_PerfilMP_RMModel))
        Dim result As Boolean = False
        Dim tmp As WSTokenModel = Await GetToken()
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {tmp.token}")
        Dim lstResult As New List(Of WSNufeed_PerfilMP_RMModel)
        Try
            Dim urlPeticion As String = path + "rawmaterial/getanalysis?customerName=" + CodCliente
            If CodALLIXP <> "" Then urlPeticion += "&rawMaterialCode=" + CodALLIXP
            Dim response As HttpResponseMessage = Await client.GetAsync(urlPeticion)

            If (response.StatusCode = System.Net.HttpStatusCode.OK) Then
                Dim content As String = Await response.Content.ReadAsStringAsync()
                lstResult = Await response.Content.ReadAsAsync(Of List(Of WSNufeed_PerfilMP_RMModel))

                result = True 'Await response.Content.ReadAsStringAsync()
            Else
                Dim errorWS As WSErrorModel = Await response.Content.ReadAsAsync(Of WSErrorModel)
                strError = CleanSpecialCharacter(errorWS.Message)
            End If
        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        End Try

        Return lstResult
    End Function

    Public Async Function GetFormulas(CodCliente As String) As Task(Of List(Of WSNufeed_FormulaModel))
        Dim result As Boolean = False
        Dim tmp As WSTokenModel = Await GetToken()
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {tmp.token}")
        Dim lstResult As New List(Of WSNufeed_FormulaModel)
        Try
            Dim urlPeticion As String = path + "diet/production?customerName=" + CodCliente
            Dim response As HttpResponseMessage = Await client.GetAsync(urlPeticion)

            If (response.StatusCode = System.Net.HttpStatusCode.OK) Then
                Dim content As String = Await response.Content.ReadAsStringAsync()
                lstResult = Await response.Content.ReadAsAsync(Of List(Of WSNufeed_FormulaModel))
                result = True 'Await response.Content.ReadAsStringAsync()
            Else
                Dim errorWS As WSErrorModel = Await response.Content.ReadAsAsync(Of WSErrorModel)
                strError = CleanSpecialCharacter(errorWS.Message)
            End If
        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        End Try

        Return lstResult
    End Function
    Public Function UPdatePerfilMP(Valores As String, UsuAct As Int64) As Boolean
        Dim sb As New StringBuilder
        Dim dt As DataTable
        Dim IsResult As Boolean = False
        Try
            sb.Append(" DECLARE @Valores varchar(MAX)='" + Valores + "'")
            sb.Append(" DECLARE @UsuAct bigint=" + UsuAct.ToString)
            sb.Append(" DECLARE @Estatus int=0")
            sb.Append(" DECLARE @Mensaje varchar(250)=''")

            sb.Append(" EXEC spu_ActualizaEsperados @Valores,@UsuAct,@Estatus Output,@Mensaje Output")

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


    'Public Async Function GetPerfilMP(CodCliente As String, CodALLIXP As String) As Task(Of List(Of WSNufeed_PerfilMP_RMModel))
    '    Dim result As Boolean = False
    '    Dim tmp As WSTokenModel = Await GetToken()
    '    client.DefaultRequestHeaders.Add("Authorization", $"Bearer {tmp.token}")
    '    Dim lstResult As New List(Of WSNufeed_PerfilMP_RMModel)
    '    Try
    '        Dim urlPeticion As String = path + "rawmaterial/getanalysis?customerName=" + CodCliente
    '        If CodALLIXP <> "" Then urlPeticion += "&rawMaterialCode=" + CodALLIXP
    '        Dim response As HttpResponseMessage = Await client.GetAsync(urlPeticion)

    '        If (response.StatusCode = System.Net.HttpStatusCode.OK) Then
    '            Dim content As String = Await response.Content.ReadAsStringAsync()
    '            lstResult = Await response.Content.ReadAsAsync(Of List(Of WSNufeed_PerfilMP_RMModel))

    '            result = True 'Await response.Content.ReadAsStringAsync()
    '        Else
    '            Dim errorWS As WSErrorModel = Await response.Content.ReadAsAsync(Of WSErrorModel)
    '            strError = CleanSpecialCharacter(errorWS.Message)
    '        End If
    '    Catch ex As Exception
    '        strError = CleanSpecialCharacter(ex.Message)
    '    End Try

    '    Return lstResult
    'End Function

End Class
