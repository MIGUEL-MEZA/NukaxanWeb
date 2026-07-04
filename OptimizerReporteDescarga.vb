Imports System.Collections.Generic
Imports System.IO
Imports System.Net
Imports System.Text
Imports System.Web
Imports System.Web.UI

Public Class OptimizerReporteDescarga
    Public Shared Sub Descargar(page As Page, baseApiUrl As String, cvePerfilN As Long, formato As String, versionReporte As Integer, prefijoArchivo As String)
        Descargar(page, baseApiUrl, cvePerfilN, formato, versionReporte, prefijoArchivo, "perfilnutricional", Nothing)
    End Sub

    Public Shared Sub Descargar(page As Page, baseApiUrl As String, cvePerfilN As Long, formato As String, versionReporte As Integer, prefijoArchivo As String, rutaReporte As String, seccion As String)
        If page Is Nothing Then Throw New ArgumentNullException(NameOf(page))
        If String.IsNullOrWhiteSpace(baseApiUrl) Then Throw New Exception("No se encontró la configuración del servicio para descargar el reporte.")

        Dim formatoNormalizado As String = formato.Trim().ToLowerInvariant()
        If formatoNormalizado <> "excel" AndAlso formatoNormalizado <> "pdf" Then Throw New Exception("El formato solicitado no es válido.")

        Dim extension As String = If(formatoNormalizado = "excel", "xlsx", "pdf")
        Dim contentType As String = If(formatoNormalizado = "excel",
                                       "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                                       "application/pdf")
        Dim rutaNormalizada As String = rutaReporte.Trim().Trim("/"c)
        Dim queryParts As New List(Of String)
        If versionReporte > 0 Then queryParts.Add("versionReporte=" + versionReporte.ToString())
        If Not String.IsNullOrWhiteSpace(seccion) Then queryParts.Add("seccion=" + HttpUtility.UrlEncode(seccion))

        Dim queryString As String = If(queryParts.Count > 0, "?" + String.Join("&", queryParts), "")
        Dim url As String = baseApiUrl.TrimEnd("/"c) + "/reportes/" + rutaNormalizada + "/" + cvePerfilN.ToString() + "/" + formatoNormalizado + queryString

        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 Or SecurityProtocolType.Tls11 Or SecurityProtocolType.Tls

        Try
            Using client As New WebClient()
                client.Encoding = Encoding.UTF8
                Dim bytes As Byte() = client.DownloadData(url)
                If bytes Is Nothing OrElse bytes.Length = 0 Then Throw New Exception("El servicio no devolvió información para el archivo solicitado.")

                Dim response = page.Response
                response.Clear()
                response.Buffer = True
                response.ContentType = contentType
                response.AddHeader("Content-Disposition", "attachment; filename=" + prefijoArchivo + "_" + cvePerfilN.ToString() + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + "." + extension)
                response.BinaryWrite(bytes)
                response.Flush()
                HttpContext.Current.ApplicationInstance.CompleteRequest()
            End Using
        Catch ex As WebException
            Dim mensaje As String = "No fue posible descargar el archivo solicitado."

            If ex.Response IsNot Nothing Then
                Using responseStream = ex.Response.GetResponseStream()
                    If responseStream IsNot Nothing Then
                        Using reader As New StreamReader(responseStream)
                            Dim detalle = reader.ReadToEnd()
                            If Not String.IsNullOrWhiteSpace(detalle) Then
                                mensaje = detalle
                            End If
                        End Using
                    End If
                End Using
            ElseIf Not String.IsNullOrWhiteSpace(ex.Message) Then
                mensaje = ex.Message
            End If

            Throw New Exception(mensaje, ex)
        End Try
    End Sub
End Class
