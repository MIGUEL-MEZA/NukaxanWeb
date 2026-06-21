Imports System.Data
Imports NukaxanWEB
Imports NukaxanWEB.DataBase
Imports NukaxanWEB.Libreria
Public Class PerfilN_Variables
    Public strError As String = ""
    Public Function GetSQL(TipoNutriente As String) As String
        Dim sb As New StringBuilder

        sb.Append(" DECLARE @TipoNutriente varchar(MAX)='" + TipoNutriente + "'")
        sb.Append(" DECLARE @Estatus int=0")
        sb.Append(" DECLARE @Mensaje varchar(250)=''")

        sb.Append(" EXEC spc_OptimizerC_PerfilNutricional_Variables @TipoNutriente,@Estatus Output,@Mensaje Output")

        Return sb.ToString
    End Function
    Public Function FindAll(TipoNutriente As String) As DataTable
        Dim dt As DataTable
        Try
            dt = execQuery(GetSQL(TipoNutriente))
        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        End Try

        Return dt
    End Function
    Public Function FindlstAll(TipoNutriente As String) As List(Of PerfilN_VariablesModel)
        Dim dt As DataTable
        Dim lst As New List(Of PerfilN_VariablesModel)
        Try
            dt = FindAll(TipoNutriente)
            For Each dr As DataRow In dt.Rows
                Dim ObjM As PerfilN_VariablesModel = FillModel(dr)
                lst.Add(ObjM)
            Next

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        Finally
            dt = Nothing
        End Try

        Return lst
    End Function
    Public Function FindById(TipoNutriente As String) As PerfilN_VariablesModel
        Dim ObjM As New PerfilN_VariablesModel
        Dim dt As DataTable
        Try
            dt = FindAll(TipoNutriente)
            If Not dt Is Nothing Then ObjM = FillModel(dt.Rows(0))

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        Finally
            dt = Nothing
        End Try
        Return ObjM
    End Function
    Public Function FillModel(dr As DataRow) As PerfilN_VariablesModel
        Dim ObjModel As New PerfilN_VariablesModel
        ObjModel.CveVariable = dr("CveVariable")
        ObjModel.CodALLIX = dr("CodALLIX")
        ObjModel.NomVariable = dr("NomVariable")
        ObjModel.Posicion = dr("Posicion")
        ObjModel.TipoNutriente = dr("TipoNutriente")
        ObjModel.FecAct = CDate(dr("FecAct")).ToString("dd-MM-yyyy")
        ObjModel.UsuAct = dr("UsuAct")

        Return ObjModel
    End Function
End Class
