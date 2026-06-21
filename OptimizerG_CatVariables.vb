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
        ObjModel.CveVariable = dr("CveVariable")
        ObjModel.CodALLIX = dr("CodALLIX")
        ObjModel.NomVariable = dr("NomVariable")
        ObjModel.Posicion = dr("Posicion")
        ObjModel.Nutriente = dr("Nutriente")
        ObjModel.Decimales = dr("Decimales")
        ObjModel.MostrarCliente = dr("MostrarCliente")
        ObjModel.CveCategoria = dr("CveCategoria")
        ObjModel.EditarAjuste = dr("EditarAjuste")
        ObjModel.ReporteInterno = dr("ReporteInterno")
        ObjModel.ReporteExterno = dr("ReporteExterno")
        ObjModel.MostrarValores = dr("MostrarValores")
        ObjModel.EnvioFlujo = dr("EnvioFlujo")
        ObjModel.NomCategoria = dr("NomCategoria")

        'Bitacora
        ObjModel.FecAct = CDate(dr("FecAct")).ToString("dd/MM/yyyy HH:mm")
        ObjModel.UsuAct = dr("UsuAct")

        Return ObjModel
    End Function


End Class
