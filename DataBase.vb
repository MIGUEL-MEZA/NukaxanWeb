Imports System.Data
Imports System.Data.OleDb
Imports NukaxanWEB
Imports NukaxanWEB.Libreria
Public Class DataBase
    Public Shared strError As String = ""
    Public Shared Function Conectar(Optional ByVal tipoDB As String = "") As OleDbConnection
        Dim cadena As String = ConfigurationManager.AppSettings("ConnBD.ConnectionString")
        Select Case tipoDB.ToLower
            Case ""
#If DEBUG Then
                cadena = ConfigurationManager.AppSettings("ConnBD.ConnectionStringDebug")
#Else
                cadena = ConfigurationManager.AppSettings("ConnBD.ConnectionString")
#End If
            Case "nulink"
                cadena = ConfigurationManager.AppSettings("ConnBD_Nulink.ConnectionString")

        End Select

        Dim Oledbconn1 As New OleDbConnection(cadena)
        Return Oledbconn1
    End Function
    Public Shared Function execQuery(ByVal strSQL As String, Optional ByVal tipoDB As String = "") As DataTable
        Dim Conn As OleDbConnection
        Conn = Conectar(tipoDB)
        Dim dt As New DataTable
        Try
            Conn.Open()
            Dim objCMD As OleDb.OleDbCommand = New OleDb.OleDbCommand(strSQL, Conn)
            objCMD.CommandTimeout = 0
            Dim DataAdapter1 = New OleDb.OleDbDataAdapter(objCMD)
            Dim DataSet1 = New DataSet
            DataAdapter1.Fill(DataSet1)
            dt = DataSet1.Tables(0)
            Return dt
        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        Finally
            Conn.Close()
        End Try
    End Function
    Public Shared Function execNonQuery(ByVal strSQL As String, Optional ByVal tipoDB As String = "") As Boolean
        Dim cmd As OleDb.OleDbCommand
        Dim resp As Boolean = False
        Dim Conn As OleDbConnection
        Conn = Conectar(tipoDB)
        Try
            Conn.Open()
            cmd = New OleDb.OleDbCommand(strSQL, Conn)
            cmd.CommandTimeout = 0
            resp = cmd.ExecuteNonQuery()
        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        Finally
            Conn.Close()
        End Try
        Return resp
    End Function
    Public Shared Function execScalar(ByVal strSQL As String) As Integer
        Dim cmd As OleDb.OleDbCommand
        Dim resp As Boolean = False
        Dim Conn As OleDbConnection
        Dim regid As Integer = 0
        Conn = Conectar()
        Try
            Conn.Open()
            cmd = New OleDb.OleDbCommand(strSQL, Conn)
            cmd.CommandTimeout = 0
            regid = CInt(cmd.ExecuteScalar())
        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        Finally
            Conn.Close()
        End Try
        Return regid
    End Function
End Class
