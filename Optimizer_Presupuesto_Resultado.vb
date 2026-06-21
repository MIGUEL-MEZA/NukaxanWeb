Imports System.Data
Imports NukaxanWEB
Imports NukaxanWEB.DataBase
Imports NukaxanWEB.Libreria
Public Class Optimizer_Presupuesto_Resultado
    Public strError As String = ""
    Public Function getSQL(Opcion As Integer, Valores As String) As String
        Dim sb As New StringBuilder
        sb.Append(" DECLARE @Opcion  int=" + Opcion.ToString)
        sb.Append(" DECLARE @Valores varchar(MAX)='" + Valores + "'")
        sb.Append(" DECLARE @Estatus int=0")
        sb.Append(" DECLARE @Mensaje varchar(250)=''")
        sb.Append(" EXEC spc_Optimizer_Presupuesto_Resultado @Opcion,@Valores,@Estatus Output,@Mensaje Output")

        Return sb.ToString
    End Function
    Public Function FindAll(Opcion As Integer, Valores As String) As DataTable
        Dim dt As DataTable
        Try
            dt = execQuery(getSQL(Opcion, Valores))

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        End Try

        Return dt
    End Function

End Class
