Imports System.ComponentModel
Imports System.Web.Script.Services
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports Newtonsoft.Json
Imports NukaxanWEB.OptimizerP_PerfilN

' Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente.
' <System.Web.Script.Services.ScriptService()> _

<WebService(Namespace:="http://tempuri.org/")>
<WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)>
<ScriptService()>
Public Class OptimizerPerfilServices
    Inherits System.Web.Services.WebService
    <WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Public Function GuardarJSONAjuste(modelo As List(Of PNCapturaModel), modulo As String, id As Long, usuario As String) As String
        Try
            Dim result As Boolean = False

            If modulo = "P" Then
                Dim negocio As New OptimizerP_PerfilN()
                Dim modeloEditable As OptimizerP_ResponseEditableModel = negocio.ObtenerModeloEditable(id)
                modeloEditable = negocio.AplicarAjustes(modeloEditable, modelo)
                Dim jsonNuevo = JsonConvert.SerializeObject(modeloEditable)
                result = negocio.ActualizaEditable(3, id, jsonNuevo, usuario)
            ElseIf modulo = "G" Then
                Dim jsonNuevo = JsonConvert.SerializeObject(modelo)
                result = New OptimizerG_PerfilN().ActualizaEditable(3, id, jsonNuevo, usuario)
            Else
                Throw New Exception("Módulo no soportado.")
            End If

            If Not result Then
                Throw New Exception("No fue posible guardar los ajustes.")
            End If

            Return "OK"

        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try

    End Function


End Class
