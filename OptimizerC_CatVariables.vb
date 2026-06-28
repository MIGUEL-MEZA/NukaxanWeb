Imports System.Data
Imports Microsoft.Ajax.Utilities
Imports NukaxanWEB
Imports NukaxanWEB.DataBase
Imports NukaxanWEB.Libreria
Public Class OptimizerC_CatVariables
    Public strError As String = ""
    Public Folio As String = ""
    Public Function GetSQL(CveVariable As Integer) As String
        Dim sb As New StringBuilder

        sb.Append(" DECLARE @CveVariable  int=" + CveVariable.ToString)
        sb.Append(" DECLARE @Estatus int=0")
        sb.Append(" DECLARE @Mensaje varchar(250)=''")

        sb.Append(" EXEC spc_OptimizerC_CatVariables @CveVariable,@Estatus Output,@Mensaje Output")

        Return sb.ToString
    End Function
    Public Function FindAll(CveVariable As Integer) As DataTable
        Dim dt As DataTable
        Try
            dt = execQuery(GetSQL(CveVariable))
        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        End Try

        Return dt
    End Function
    Public Function FindlstAll(CveVariable As Integer) As List(Of OptimizerC_CatVariablesModel)
        Dim dt As DataTable
        Dim lst As New List(Of OptimizerC_CatVariablesModel)
        Try
            dt = FindAll(CveVariable)
            For Each dr As DataRow In dt.Rows
                Dim ObjM As OptimizerC_CatVariablesModel = FillModel(dr)
                lst.Add(ObjM)
            Next

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        Finally
            dt = Nothing
        End Try

        Return lst
    End Function
    Public Function FindById(CveVariable As Integer) As OptimizerC_CatVariablesModel
        Dim ObjM As New OptimizerC_CatVariablesModel
        Dim dt As DataTable
        Try
            dt = FindAll(CveVariable)
            If Not dt Is Nothing Then ObjM = FillModel(dt.Rows(0))

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        Finally
            dt = Nothing
        End Try
        Return ObjM
    End Function
    Public Function FillModel(dr As DataRow) As OptimizerC_CatVariablesModel
        Dim ObjModel As New OptimizerC_CatVariablesModel
        ObjModel.CveVariable = GetFieldValue(dr, "CveVariable", 0)
        ObjModel.CodALLIX = GetFieldValue(dr, "CodALLIX", "")
        ObjModel.NomVariable = GetFieldValue(dr, "NomVariable", "")
        ObjModel.Posicion = GetFieldValue(dr, "Posicion", 0)
        ObjModel.TipoNutriente = GetFieldValue(dr, "TipoNutriente", GetFieldValue(dr, "Nutriente", ""))
        ObjModel.Decimales = GetFieldValue(dr, "Decimales", 2)
        ObjModel.MostrarCliente = GetFieldValue(dr, "MostrarCliente", "")
        ObjModel.CveCategoria = GetFieldValue(dr, "CveCategoria", 0)
        ObjModel.EditarAjuste = GetFieldValue(dr, "EditarAjuste", "N")
        ObjModel.ReporteInterno = GetFieldValue(dr, "ReporteInterno", "S")
        ObjModel.ReporteExterno = GetFieldValue(dr, "ReporteExterno", "S")
        ObjModel.MostrarValores = GetFieldValue(dr, "MostrarValores", "")
        ObjModel.EnvioFlujo = GetFieldValue(dr, "EnvioFlujo", "N")
        ObjModel.NomCategoria = GetFieldValue(dr, "NomCategoria", "")

        If dr.Table.Columns.Contains("FecAct") AndAlso Not IsDBNull(dr("FecAct")) Then
            ObjModel.FecAct = CDate(dr("FecAct")).ToString("dd/MM/yyyy HH:mm")
        End If
        ObjModel.UsuAct = GetFieldValue(dr, "UsuAct", 0)

        Return ObjModel
    End Function


    Private Function GetFieldValue(Of T)(dr As DataRow, columnName As String, defaultValue As T) As T
        If dr Is Nothing OrElse dr.Table Is Nothing OrElse Not dr.Table.Columns.Contains(columnName) OrElse IsDBNull(dr(columnName)) Then
            Return defaultValue
        End If

        Dim value As Object = dr(columnName)
        Return CType(Convert.ChangeType(value, GetType(T)), T)
    End Function
End Class
