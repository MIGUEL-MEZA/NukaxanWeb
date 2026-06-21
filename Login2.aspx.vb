Imports System.Data.OleDb
Imports System.Data.SqlClient
Imports NukaxanWEB
Imports NukaxanWEB.Libreria
Public Class Login2
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Validation", "alert('" + CheckConnection().ToString + "');", True)
        End If
    End Sub
    Public Function CheckConnection() As Boolean
        Dim isValid As Boolean = False
        Dim Conn As OleDbConnection
        Try
            Dim cadena As String = ConfigurationManager.AppSettings("ConnBD.ConnectionString")
            Dim Oledbconn1 As New OleDbConnection(cadena)
            Conn = Oledbconn1
            Conn.Open()
            isValid = True
        Catch ex As SqlException
            isValid = False
        Finally
            Conn.Close()
        End Try

        Return isValid
    End Function
    'General
    Protected Sub Page_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender

    End Sub
End Class