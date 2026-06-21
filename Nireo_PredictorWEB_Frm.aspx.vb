Imports System.Globalization
Imports System.IO
Imports System.Net.Http
Imports System.Text.Json
Imports System.Text.Json.Serialization
Imports Microsoft.Ajax.Utilities
Imports NukaxanWEB.Libreria

''' <summary>
''' This class represents the spectre data of the response from the PredictorAPI
''' filtered by the SpectreResult class.
''' </summary>
''' <remarks>
''' This class is used to deserialize the spectre data from the JSON response.
''' Used to deserialize the JSON response from the PredictorAPI and show the results in
''' the GridViewPredictions (GridView in the aspx file) and export the results to a CSV file.
''' </remarks>
Public Class SpectreResult
    '' Date: 2024-06-24
    '' Time: 13:41:45
    '' Spectrum File: 01A332D0005-240621.txt
    '' Models: Dictionary<string, double> (Model, Plane) **List of models with their planes**
    '' Spectre: [double] **List of 201 values**
    Public Property SampleDate As String
    Public Property SampleTime As String
    Public Property SpectrumFile As String
    Public Property Models As List(Of Dictionary(Of String, Double))
    Public Property Spectre As List(Of Double)
End Class
Public Class PredictorFileName
    Public Property ModelFileName As String
    Public Property SpectreFileName As String
    Public Property KeyFileName As String
End Class

''' <summary>
''' Nireo_PredictorWEB_Frm class represents the code behind of the Nireo_PredictorWEB_Frm page
''' </summary>
Public Class Nireo_PredictorWEB_Frm
    Inherits Page
    Private UserModel As UsuarioModel
    ' TODO: Add this url to the web.config file or take it from a database
    Private ReadOnly predictorUrl As String = "https://nukaxan.gponutec.com:8093/ForecastEngineAPI/Forecast/"
    ' TODO: Take this token from the logged user session when available
    Private ReadOnly bearerToken As String = System.Configuration.ConfigurationManager.AppSettings("ForecastEngineBearerToken")
    Public errorMessages As String = ""
    Public spectres As New List(Of SPECTREModel)()
    Public predictorFileNames As New List(Of PredictorFileName)()

    ''' <summary>
    ''' Check if the user is logged in, if not, redirect to the login page
    ''' </summary>
    ''' <param name="sender"> Not used </param>
    ''' <param name="e"> Not used </param>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Me.UserModel = DirectCast(Session("UsuarioLogin"), UsuarioModel)
        If Me.Page.User.Identity.IsAuthenticated = False Or
            UserModel Is Nothing Then Response.Redirect("logout.aspx", True)
    End Sub

    ''' <summary>
    ''' Event handler for the PredictButton click event
    ''' </summary>
    ''' <param name="sender"> Not used </param>
    ''' <param name="e"> Not used </param>
    Protected Sub ButtonPredictClicked(sender As Object, e As EventArgs) Handles ButtonPredict.Click
        Dim trxId As Long
        Dim spectreIds As New List(Of Long)()
        ' TODO: Add a maxAttempts and pauseTime to the web.config file or take it from a database
        Dim maxAttempts As Integer = 20
        Dim pauseTime As Integer = 500
        Try
            trxId = If(CallPredictorAPI("GetTransaction", HttpMethod.Get)?.
                Content.ReadAsAsync(Of Long)().Result, 0)
            If trxId = 0 Then Throw New Exception("Transaction has not been correctly created (Transaction = 0)")
            If Not FileUploadSpectre.HasFiles Or Not FileUploadModel.HasFiles Then
                Throw New Exception("No files uploaded")
            End If

            predictorFileNames.Clear()
            spectres.Clear()
            Dim dictionaryDateTime As DateTime = DateTime.Now
            For Each spectrePostedFile As HttpPostedFile In FileUploadSpectre.PostedFiles
                For Each modelPostedFile As HttpPostedFile In FileUploadModel.PostedFiles
                    predictorFileNames.Add(New PredictorFileName With {
                        .ModelFileName = modelPostedFile.FileName,
                        .SpectreFileName = spectrePostedFile.FileName,
                        .KeyFileName = trxId.ToString() + "_" +
                        dictionaryDateTime.ToString("yyMMddHHmmss") + ".unsb"
                    })
                    dictionaryDateTime = dictionaryDateTime.AddSeconds(1)
                Next
                Dim bodySpectres As List(Of SPECTREModel) =
                    GetBodySpectres(trxId, spectrePostedFile, FileUploadModel.PostedFiles)
                spectres.AddRange(bodySpectres)
            Next
#If DEBUG Then
            Debug.WriteLine("FileUploadSpectre names:")
            For Each predictorFileName In predictorFileNames
                Debug.WriteLine("  ModelFileName: " & predictorFileName.ModelFileName)
                Debug.WriteLine("  SpectreFileName: " & predictorFileName.SpectreFileName)
                Debug.WriteLine("  KeyFileName: " & predictorFileName.KeyFileName)
                Debug.WriteLine("--------------------------------")
            Next
#End If
            Dim apiSaveResponse As List(Of Object) =
                CallPredictorAPI("SaveSpectres", HttpMethod.Post, JsonSerializer.Serialize(spectres))?.
                Content.ReadAsAsync(Of List(Of Object))().Result
            If apiSaveResponse Is Nothing Then Throw New Exception("The Spectres could not be saved")
            For Each idScope In apiSaveResponse
                If TypeOf idScope Is Integer Then
                    spectreIds.Add(CLng(idScope))
                ElseIf TypeOf idScope Is Long Then
                    spectreIds.Add(CType(idScope, Long))
                Else
                    Throw New Exception("Unsupported type found in SaveSpectres response")
                End If
            Next
            If Not spectreIds.Any() Then
                Throw New Exception("No bodySpectre saved")
            End If
            ' TODO: Get Number of attempts to get the predictions and pause time from a database or configuration file
            For predictionAttempt As Integer = 1 To maxAttempts
                Dim apiGetResponse As String =
                    CallPredictorAPI("GetPredictions?id_trx=" + trxId.ToString(), HttpMethod.Get)?.
                    Content.ReadAsStringAsync().Result
                'outputSpectres = JsonSerializer.Deserialize(Of List(Of SPECTREModel))(apiGetResponse)
                If apiGetResponse Is Nothing Then
                    If predictionAttempt >= maxAttempts Then
                        Throw New Exception("Predictions not found")
                    Else
                        System.Threading.Thread.Sleep(pauseTime * predictionAttempt)
                        Continue For
                    End If
                End If
#If DEBUG Then
                Debug.WriteLine("JSON Response from the API:")
                Debug.WriteLine(apiGetResponse)
#End If
                Using jsonDoc As JsonDocument = JsonDocument.Parse(apiGetResponse)
                    Dim root As JsonElement = jsonDoc.RootElement
                    If root.ValueKind = JsonValueKind.Array Then
                        Dim elementPosition As Integer = 0
                        For Each element As JsonElement In root.EnumerateArray()
                            spectres(elementPosition).IdStatus = element.GetProperty("id_status").GetInt32()
                            spectres(elementPosition).PlanesJson = element.GetProperty("planes_json").GetString()
                            spectres(elementPosition).SpectreJson =
                                JsonSerializer.Deserialize(Of SpectreData)(element.GetProperty("spectre_json").GetString())
                            spectres(elementPosition).CreatedAt = element.GetProperty("created_at").GetDateTime()
                            spectres(elementPosition).UpdatedAt = element.GetProperty("updated_at").GetDateTime()
                            elementPosition += 1
                        Next
                        Exit For
                    Else
                        Throw New Exception("Predictions not found")
                    End If
                End Using
            Next
#If DEBUG Then
            Debug.WriteLine("Spectres retreived from the API:")
            For Each spectre In spectres
                Debug.WriteLine("Spectre:")
                Debug.WriteLine("    Id: " & spectre.Id)
                Debug.WriteLine("    IdTrx: " & spectre.IdTrx)
                Debug.WriteLine("    IdPriority: " & spectre.IdPriority)
                Debug.WriteLine("    IdStatus: " & spectre.IdStatus)
                Debug.WriteLine("    IdModelType: " & spectre.IdModelType)
                Debug.WriteLine("    FileName: " & spectre.FileName)
                Debug.WriteLine("    PlanesJson: " & spectre.PlanesJson)
                Debug.WriteLine("    SpectreJson: " & JsonSerializer.Serialize(spectre.SpectreJson))
                Debug.WriteLine("    CreatedAt: " & spectre.CreatedAt)
                Debug.WriteLine("    UpdatedAt: " & spectre.UpdatedAt)
            Next
            Debug.WriteLine("End of Spectres\n")
#End If
            '' Fill GridViewPredictions (GridView in the aspx)
            If spectres Is Nothing Then
                Throw New Exception("No predictions found")
            End If
#If DEBUG Then
            Debug.WriteLine("-- Spectres are not null --\n")
#End If
            '' Fill a list of SpectreResult to show the results in the GridViewPredictions
            Dim spectreResults As New List(Of SpectreResult)()
            For Each spectre In spectres
#If DEBUG Then
                Debug.WriteLine("Looping through spectres:" + spectre.FileName)
#End If
                Dim spectreFileName As String =
                    predictorFileNames.FirstOrDefault(Function(x) x.KeyFileName = spectre.FileName).SpectreFileName
                Dim modelDictionary As New Dictionary(Of String, Double) From
                {
                    {   '' NOTE: 
                        spectre.SpectreJson.ModelNames.FirstOrDefault,
                        CType(JsonSerializer.Deserialize(
                            spectre.PlanesJson, GetType(List(Of Double))), List(Of Double)).Last() +
                        Convert.ToDouble(TextBoxBias.Text)
                    }
                }
                If spectreResults.Any(
                    Function(spectreResult) spectreResult.SpectrumFile = spectreFileName) Then
                    spectreResults.First(Function(spectreResult) spectreResult.SpectrumFile = spectreFileName).
                        Models.Add(modelDictionary)
                Else
                    Dim spectreResult As New SpectreResult With {
                        .SampleDate = spectre.CreatedAt.ToString("yyyy-MM-dd"),
                        .SampleTime = spectre.CreatedAt.ToString("HH:mm:ss"),
                        .SpectrumFile = spectreFileName,
                        .Models = New List(Of Dictionary(Of String, Double)) From {modelDictionary},
                        .Spectre = spectre.SpectreJson.Spectre.First()
                    }
                    spectreResults.Add(spectreResult)
                End If
#If DEBUG Then
                Debug.WriteLine("  SpectreResults content now:")
                Debug.WriteLine(JsonDocument.Parse(JsonSerializer.Serialize(spectreResults)).RootElement.ToString())
#End If
            Next
#If DEBUG Then
            Debug.WriteLine("-- SpectreResults --\n")
#End If
            GridViewPredictions.Columns.Clear()

            Dim dataSource As New DataTable()
            Dim columnType As Type = GetType(String)
            dataSource.Columns.Add(New DataColumn With {.ColumnName = "Fecha", .DataType = columnType})
            dataSource.Columns.Add(New DataColumn With {.ColumnName = "Hora", .DataType = columnType})
            dataSource.Columns.Add(New DataColumn With {.ColumnName = "ArchivoEspectro", .DataType = columnType})
            For index As Integer = 1 To spectreResults.First().Models.Count
                dataSource.Columns.Add(New DataColumn With {.ColumnName = "Modelo" + index.ToString(), .DataType = columnType})
                dataSource.Columns.Add(New DataColumn With {.ColumnName = "Prediccion" + index.ToString(), .DataType = columnType})
            Next
            For waveLenghtIndex As Integer = 1550 To 1950 Step 2
                dataSource.Columns.Add(New DataColumn With {.ColumnName = waveLenghtIndex.ToString() + "nm", .DataType = columnType})
            Next
            For Each spectreResult In spectreResults
                Dim row As DataRow = dataSource.NewRow()
                row("Fecha") = spectreResult.SampleDate
                row("Hora") = spectreResult.SampleTime
                row("ArchivoEspectro") = spectreResult.SpectrumFile
                For modelIndex As Integer = 1 To spectreResult.Models.Count
                    row("Modelo" & modelIndex.ToString()) = spectreResult.Models(modelIndex - 1).Keys.First()
                    row("Prediccion" & modelIndex.ToString()) = spectreResult.Models(modelIndex - 1).Values.First()
                Next
                For waveLenghtIndex As Integer = 0 To spectreResult.Spectre.Count - 1
                    row((1550 + waveLenghtIndex * 2).ToString() & "nm") = spectreResult.Spectre(waveLenghtIndex)
                Next
                dataSource.Rows.Add(row)
#If DEBUG Then
                Debug.WriteLine("Row added:")
                Debug.WriteLine("  Fecha: " & row("Fecha"))
                Debug.WriteLine("  Hora: " & row("Hora"))
                Debug.WriteLine("  ArchivoEspectro: " & row("ArchivoEspectro"))
#End If
            Next
            GridViewPredictions.DataSource = dataSource
            Dim regex As New System.Text.RegularExpressions.Regex("^\d{4}nm$")
            GridViewPredictions.DataBind()
            For Each column As TableCell In GridViewPredictions.HeaderRow.Cells
                If regex.IsMatch(column.Text) Then
                    column.Visible = False
                End If
            Next
            For Each row As GridViewRow In GridViewPredictions.Rows
                For indexCell As Integer = 0 To row.Cells.Count - 1
                    If indexCell >= row.Cells.Count - 201 Then
                        row.Cells(indexCell).Visible = False
                    End If
                Next
            Next
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Validation", "alert('" + ex.Message.Replace("'", "") + "');", True)
        End Try
    End Sub

    ''' <summary>
    ''' Event handler for the ExportButton click event
    ''' </summary>
    ''' <param name="sender"> Not used </param>
    ''' <param name="e"> Not used </param>
    Protected Sub LinkButtonExportClicked(sender As Object, e As EventArgs) Handles LinkButtonExport.Click
        Try
            Dim csv As New StringBuilder()

            Dim columnLine As String = ""
            For Each column As TableCell In GridViewPredictions.HeaderRow.Cells
                columnLine += column.Text & ","
            Next
            csv.AppendLine(columnLine.Substring(0, columnLine.Length - 1))
            For Each row As GridViewRow In GridViewPredictions.Rows
                If row.RowType = DataControlRowType.DataRow Then
                    Dim line As String = ""
                    For Each cell As TableCell In row.Cells
                        line += cell.Text.Replace(",", "").Replace(vbCrLf, "").Replace(vbLf, "").Replace(vbCr, "") & ","
                    Next
                    csv.AppendLine(line.Substring(0, line.Length - 1))
                End If
            Next
            GridViewPredictions.DataSource = Nothing
            GridViewPredictions.DataBind()
            Response.Clear()
            Response.Charset = ""
            Response.ContentType = "text/csv" ' Cambiar el ContentType a "text/csv"
            Response.AddHeader("content-disposition", "attachment;filename=predictions.csv")
            Response.Charset = "UTF-8"
            Response.ContentEncoding = Encoding.Default
            Response.Write(csv.ToString())
            Response.End()
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Validation", "alert('" + ex.Message.Replace("'", "") + "');", True)
        End Try
    End Sub

    ' TODO: Implement the FillGridView method if it is necessary or remove it if it is not used
    ''' <summary>
    ''' Fill the GridViewPredictions with the spectres data
    ''' </summary>
    ''' <param name="spectres"> List of SPECTREModel to fill the GridViewPredictions </param>
    ''' <remarks>
    ''' The GridViewPredictions is a GridView in the aspx file.
    ''' NOTE: NOT IMPLEMENTED/PROVED YET (Its a suggestion to implement the FillGridView method).
    ''' This method have a notes in spanish, to help the developer to understand the code.
    ''' </remarks>
    Public Sub FillGridView(ByRef spectres As List(Of SPECTREModel))
        ' Definir el rango de longitud de onda para las columnas del espectro (1550nm a 1948nm en pasos de 2nm)
        Dim startWavelength As Integer = 1550
        Dim increment As Integer = 2

        Try
            ' Limpiar columnas anteriores del GridView
            GridViewPredictions.Columns.Clear()

            ' Agregar las columnas fijas de FileNameModel, FileNameSpectre, y Result
            Dim fileNameModelColumn As New BoundField With {
                .HeaderText = "Archivo de modelo",
                .DataField = "FileNameModel"
            }
            GridViewPredictions.Columns.Add(fileNameModelColumn)

            Dim fileNameSpectreColumn As New BoundField With {
                .HeaderText = "Archivo de espectro",
                .DataField = "FileNameSpectre"
            }
            GridViewPredictions.Columns.Add(fileNameSpectreColumn)

            Dim resultColumn As New BoundField With {
                .HeaderText = "Resultado",
                .DataField = "Result"
            }
            GridViewPredictions.Columns.Add(resultColumn)

            ' Agregar las 201 columnas de espectro (de 1550nm hasta 1948nm en pasos de 2nm)
            For i As Integer = 0 To 200
                Dim wavelength As Integer = startWavelength + (i * increment)
                Dim boundField As New BoundField With {
                    .HeaderText = wavelength.ToString() & "nm",
                    .DataField = "SpectreValue" & (i + 1)
                }
                boundField.ItemStyle.Width = Unit.Pixel(100) ' Ajustar ancho de las columnas
                boundField.ItemStyle.Wrap = False ' Deshabilitar el ajuste de texto
                GridViewPredictions.Columns.Add(boundField)
            Next

            ' Crear una lista que contendrá todos los datos a mostrar en el GridView
            Dim dataSource As New List(Of Object)

            ' Recorrer cada elemento de la lista de espectros y crear los renglones
            For Each spectre In spectres
                ' Deserializar los valores del espectro
                Dim spectreValues As List(Of Double) = spectre.SpectreJson.Spectre.First()

                ' Verificar que se tienen exactamente 201 valores en el espectro
                If spectreValues.Count <> 201 Then
                    Throw New Exception("El espectro no contiene exactamente 201 valores.")
                End If

                ' Crear el objeto con las columnas fijas y los valores del espectro
                Dim data = New With {
                .FileNameModel = spectre.FileName.Substring(
                    spectre.FileName.IndexOf("-") + 1,
                    spectre.FileName.Length - spectre.FileName.IndexOf("-") - 1),
                    .FileNameSpectre = spectre.FileName.Substring(0, spectre.FileName.IndexOf("-")),
                    .Result = CType(JsonSerializer.Deserialize(
                        spectre.PlanesJson, GetType(List(Of Double))), List(Of Double)).Last()
                }

                ' Crear un diccionario dinámico para asignar los valores de espectro
                Dim row = New Dictionary(Of String, Object) From {
                    {"FileNameModel", data.FileNameModel},
                    {"FileNameSpectre", data.FileNameSpectre},
                    {"Result", data.Result}
                }

                ' Asignar los 201 valores del espectro a sus respectivas columnas
                For i As Integer = 0 To 200
                    row.Add("SpectreValue" & (i + 1), spectreValues(i))
                Next

                ' Agregar el renglon a la lista de datasource
                dataSource.Add(row)
            Next

            ' Asignar el data source al GridView y realizar el data bind
            GridViewPredictions.DataSource = dataSource
            GridViewPredictions.DataBind()
        Catch ex As Exception
            ' Manejar cualquier error y mostrar el mensaje
            errorMessages = ex.Message
        End Try
    End Sub

    ''' <summary>
    ''' Call the PredictorAPI with the specified endpoint and method
    ''' </summary>
    ''' <param name="endpoint">Endpoint of the PredictorAPI</param>
    ''' <param name="method">HTTP method to use</param>
    ''' <param name="jsonBody">Body of the request</param>
    ''' <returns>HttpResponseMessage with the response of the PredictorAPI</returns>
    Private Function CallPredictorAPI(
        ByRef endpoint As String,
        ByVal method As HttpMethod,
        Optional ByRef jsonBody As String = Nothing) As HttpResponseMessage
        Dim response As HttpResponseMessage = Nothing
        Try
            Using _httpClient As New HttpClient()
                _httpClient.DefaultRequestHeaders.Add("Authorization", bearerToken)
                Select Case method
                    Case HttpMethod.Get
                        response = _httpClient.GetAsync(predictorUrl + endpoint).Result
                    Case HttpMethod.Post
                        Dim content As New StringContent(jsonBody, Encoding.UTF8, "application/json")
                        response = _httpClient.PostAsync(predictorUrl + endpoint, content).Result
                End Select
            End Using
            If response Is Nothing OrElse Not response.IsSuccessStatusCode Then
                Return Nothing
            End If
            Return response
        Catch
            Throw
        End Try
    End Function

    ''' <summary>
    ''' Get the body bodySpectre from the spectre file and the model files
    ''' </summary>
    ''' <param name="trxId"> Transaction Id </param>
    ''' <param name="spectreFile"> Txt spectre file, to get the matrix </param>
    ''' <param name="modelFile"> UNSB model file, to get the bytes (only to save in server) </param>
    ''' <returns> List of BodySpectre (Body of the request to the PredictorAPI) </returns>
    ''' <exception cref="Exception"> If the spectre file is not valid </exception>
    ''' <remarks>
    ''' *** The file content must be like (Unscrambler/Nirone format): ***
    ''' IntTime:21/6/2024 13:41:45,SerialNro: 01A332D0005,Points: 201,LightSourceIntensity: 100,ScanAverage: 1,Steps: 2,LightSourceControlMode: Automatic, AutomaticDark: On,SubtractDark: On,NumMuestreos: 1,SampleIdentification: A, SessionReference:  -240621
    ''' Time, Date, 0.0000, 0.0000,... (201 values)
    ''' ... (more calibration spectre lines)
    ''' 13:41:45,21.06.2024,1.06194000,... (201 values)
    ''' ... (more spectre lines)
    ''' </remarks>
    Private Function GetBodySpectres(
        ByVal trxId As Long,
        ByVal spectreFile As HttpPostedFile,
        ByRef modelFiles As IList(Of HttpPostedFile)
        ) As List(Of SPECTREModel)
        Dim spectre As SPECTREModel
        Dim spectres As New List(Of SPECTREModel)()
        Try
            spectre = New SPECTREModel()
            Dim memoryStream As New MemoryStream()
            spectreFile.InputStream.CopyTo(memoryStream)
            memoryStream.Position = 0
            ' Use only a memory stream to read the file content (not read from the file again)
            Dim lines As New List(Of String)()
            Using reader As New StreamReader(memoryStream)
                Dim fileContent As String = reader.ReadToEnd()
                lines.AddRange(fileContent.Split(New String() {vbCrLf, vbLf, vbCr}, StringSplitOptions.RemoveEmptyEntries))
                memoryStream.Position = 0
                memoryStream.Seek(0, SeekOrigin.Begin)
                memoryStream.Close()
                reader.Close()
            End Using
            Dim spectreValues As New List(Of List(Of Double))()
            For Each line As String In lines
                ' If the line does not start with a timestamp, skip it
                If Not Regex.IsMatch(line, "^\d{2}:\d{2}:\d{2},\d{2}\.\d{2}\.\d{4},") Then
                    Continue For
                End If
                Dim stringValues() As String = line.Split(","c)
                ' Quit a first two elements (TIME and DATE)
                stringValues = stringValues.Skip(2).ToArray()
                Dim floatValues As New List(Of Double)()
                For Each value As String In stringValues
                    Dim floatValue As Double
                    If Double.TryParse(value, floatValue) Then
                        floatValues.Add(floatValue)
                    End If
                Next
                spectreValues.Add(floatValues)
            Next
            ' TODO: Implement a DigestMode Using a Database info (settings table)
            Dim digestData As New List(Of Double)()
            ' Implemented digestData = Average for each column
            ' TODO: Implement all digest modes (raw, average, median)
            For column As Integer = 0 To spectreValues(0).Count - 1
                Dim sum As Double = 0
                For row As Integer = 0 To spectreValues.Count - 1
                    sum += spectreValues(row)(column)
                Next
                digestData.Add(sum / spectreValues.Count)
            Next
            For Each modelFile In modelFiles
                Dim index As Integer = 0
                Dim spectreData As New SpectreData() With {
                    .Spectre = New List(Of List(Of Double)) From {digestData}
                }
                Dim predictorFileName As String = predictorFileNames.FirstOrDefault(
                Function(x)
                    Return x.SpectreFileName = spectreFile.FileName And
                    x.ModelFileName = modelFile.FileName
                End Function).KeyFileName
                spectre = New SPECTREModel With {
                    .IdTrx = trxId,
                    .IdPriority = 3, ' TODO: Implement a Priority Using a Database info (settings table)
                    .SpectreJson = spectreData,
                    .FileName = predictorFileName,
                    .IdModelType = 1 ' TODO: Implement a ModelType (RawLogic) Using a Database info (settings table)
                }
                index += 1
                Using stream As New MemoryStream()
                    Dim fileStream = modelFile.InputStream()
                    fileStream.CopyTo(stream)
                    spectre.ModelFile = stream.ToArray()
                    fileStream.Seek(0, SeekOrigin.Begin)
                End Using
                spectres.Add(spectre)
            Next
        Catch ex As Exception
            Throw
        End Try
        Return spectres
    End Function

End Class
