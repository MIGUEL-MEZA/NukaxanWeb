Imports System.Data
Imports Microsoft.Ajax.Utilities
Imports NukaxanWEB
Imports NukaxanWEB.DataBase
Imports NukaxanWEB.Libreria
Public Class OptimizerC_PerfilN_Resultado
    Dim strSQLExe As String = ""
    Public strError As String = ""
    Public Folio As String = 0
    Public Function getSQL(CvePerfilN As Int64) As String
        Dim sb As New StringBuilder
        sb.Append(" DECLARE @CvePerfilN bigint =" + CvePerfilN.ToString)
        sb.Append(" DECLARE @Estatus int=0")
        sb.Append(" DECLARE @Mensaje varchar(250)=''")
        sb.Append(" EXEC spc_OptimizerC_PerfilNutricional_Resultado @CvePerfilN,@Estatus Output,@Mensaje Output")

        Return sb.ToString
    End Function
    Public Function FindAll(CvePerfilN As Int64) As DataTable
        strError = ""
        Dim dt As DataTable
        Try
            strSQLExe = getSQL(CvePerfilN)
            dt = execQuery(strSQLExe)

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        End Try

        Return dt
    End Function
    Public Function FindlstAll(CvePerfilN As Int64) As List(Of OptimizerC_PerfilN_ResultadoModel)
        Dim dt As DataTable
        Dim lst As New List(Of OptimizerC_PerfilN_ResultadoModel)
        Try
            dt = FindAll(CvePerfilN)
            For Each dr As DataRow In dt.Rows
                Dim ObjM As OptimizerC_PerfilN_ResultadoModel = FillModel(dr)
                lst.Add(ObjM)
            Next

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        Finally
            dt = Nothing
        End Try

        Return lst
    End Function
    Public Function FindById(CvePerfilN As Int64) As OptimizerC_PerfilN_ResultadoModel
        Dim dt As DataTable
        Dim ObjM As OptimizerC_PerfilN_ResultadoModel
        Try
            dt = FindAll(CvePerfilN)
            If Not dt Is Nothing Then ObjM = FillModel(dt.Rows(0))

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        Finally
            dt = Nothing
        End Try

        Return ObjM
    End Function
    Public Function FillModel(dr As DataRow) As OptimizerC_PerfilN_ResultadoModel
        Dim ObjModel As New OptimizerC_PerfilN_ResultadoModel
        ObjModel.CvePerfilN = dr("CvePerfilN")
        ObjModel.Request = dr("Request")
        ObjModel.Response = dr("Response")
        ObjModel.FecAct = CDate(dr("FecAct")).ToString("dd/MM/yyyy HH:mm")
        ObjModel.UsuAct = dr("UsuAct")

        Return ObjModel
    End Function
End Class