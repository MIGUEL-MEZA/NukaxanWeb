Imports System.Data
Imports NukaxanWEB
Imports NukaxanWEB.DataBase
Imports NukaxanWEB.Libreria

Public Class RedirectPaginas
    Dim strSQLExe As String = ""
    Public strError As String = ""

    Public Function GetSQL(CodigoNav As String) As String
        Dim sb As New StringBuilder
        sb.Append(" DECLARE @CodeNav varchar(10) ='" + CodigoNav + "'")
        sb.Append(" DECLARE @Estatus int=0")
        sb.Append(" DECLARE @Mensaje varchar(250)=''")

        sb.Append(" EXEC spc_RedirectPaginas @CodeNav,@Estatus Output,@Mensaje Output")

        Return sb.ToString
    End Function
    Public Function FindAll(CodigoNav As String) As DataTable
        Dim dt As DataTable
        Try
            strSQLExe = GetSQL(CodigoNav)
            dt = execQuery(strSQLExe)
        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        End Try

        Return dt
    End Function
    Public Function FindlstAll(CodigoNav As String) As List(Of RedirectPaginasModel)
        Dim dt As DataTable
        Dim lst As New List(Of RedirectPaginasModel)
        Try
            dt = FindAll(CodigoNav)
            For Each dr As DataRow In dt.Rows
                Dim ObjM As RedirectPaginasModel = FillModel(dr)
                lst.Add(ObjM)
            Next

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        Finally
            dt = Nothing
        End Try

        Return lst
    End Function
    Public Function FindById(CodigoNav As String) As RedirectPaginasModel
        Dim ObjM As New RedirectPaginasModel
        Dim dt As DataTable
        Try
            dt = FindAll(CodigoNav)
            If Not dt Is Nothing Then ObjM = FillModel(dt.Rows(0))

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        Finally
            dt = Nothing
        End Try
        Return ObjM
    End Function
    Public Function FillModel(dr As DataRow) As RedirectPaginasModel
        Dim ObjModel As New RedirectPaginasModel
        ObjModel.CodeNav = dr("CodeNav")
        ObjModel.PaginaURL = dr("PaginaURL")

        Return objModel
    End Function

End Class