Imports System.Data
Imports Microsoft.Ajax.Utilities
Imports NukaxanWEB
Imports NukaxanWEB.DataBase
Imports NukaxanWEB.Libreria
Public Class OptimizerG_CatVariables
    Public strError As String = ""
    Public Folio As String = ""
    Public Function GetSQL(CveVariable As Integer) As String
        Dim sb As New StringBuilder

        sb.Append(" DECLARE @CveVariable  int=" + CveVariable.ToString)
        sb.Append(" DECLARE @Estatus int=0")
        sb.Append(" DECLARE @Mensaje varchar(250)=''")

        sb.Append(" EXEC spc_OptimizerG_CatVariables @CveVariable,@Estatus Output,@Mensaje Output")

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
    Public Function FindlstAll(CveVariable As Integer) As List(Of OptimizerG_CatVariablesModel)
        Dim dt As DataTable
        Dim lst As New List(Of OptimizerG_CatVariablesModel)
        Try
            dt = FindAll(CveVariable)
            For Each dr As DataRow In dt.Rows
                Dim ObjM As OptimizerG_CatVariablesModel = FillModel(dr)
                lst.Add(ObjM)
            Next

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        Finally
            dt = Nothing
        End Try

        Return lst
    End Function
    Public Function FindById(CveVariable As Integer) As OptimizerG_CatVariablesModel
        Dim ObjM As New OptimizerG_CatVariablesModel
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
    Public Function FillModel(dr As DataRow) As OptimizerG_CatVariablesModel
        Dim ObjModel As New OptimizerG_CatVariablesModel
        ObjModel.CveVariable = GetFieldValue(dr, "CveVariable", 0)
        ObjModel.NomVariable = GetFieldValue(dr, "NomVariable", "")
        ObjModel.Posicion = GetFieldValue(dr, "Posicion", 0)
        ObjModel.CodALLIX = GetFieldValue(dr, "CodALLIX", "")
        ObjModel.CodFormat = GetFieldValue(dr, "CodFormat", "")
        ObjModel.FactorValMax = GetFieldValue(dr, "FactorValMax", 0.0)
        ObjModel.Nutriente = GetFieldValue(dr, "Nutriente", "")
        ObjModel.Decimales = GetFieldValue(dr, "Decimales", 2)
        ObjModel.MostrarCliente = GetFieldValue(dr, "MostrarCliente", "")
        ObjModel.CveCategoria = GetFieldValue(dr, "CveCategoria", 0)
        ObjModel.EditarAjuste = GetFieldValue(dr, "EditarAjuste", "N")
        ObjModel.ReporteInterno = GetFieldValue(dr, "ReporteInterno", "N")
        ObjModel.ReporteExterno = GetFieldValue(dr, "ReporteExterno", "N")
        ObjModel.MostrarValores = GetFieldValue(dr, "MostrarValores", "")
        ObjModel.EnvioFlujo = GetFieldValue(dr, "EnvioFlujo", "N")
        ObjModel.NomCategoria = GetFieldValue(dr, "NomCategoria", "")
        ObjModel.PosicionC = GetFieldValue(dr, "PosicionC", 0)

        'Bitacora
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

        Return CType(Convert.ChangeType(dr(columnName), GetType(T)), T)
    End Function

End Class
