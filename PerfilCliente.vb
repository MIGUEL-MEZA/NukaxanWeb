Imports System.Data
Imports Microsoft.Ajax.Utilities
Imports NukaxanWEB
Imports NukaxanWEB.DataBase
Imports NukaxanWEB.Libreria
Public Class PerfilCliente
    Public strError As String = ""
    Public Folio As String = ""
    Public Function GetSQL(CodCliente As String) As String
        Dim sb As New StringBuilder

        sb.Append(" DECLARE @CodCliente varchar(60) = '" + CodCliente + "'")
        sb.Append(" DECLARE @Estatus int=0")
        sb.Append(" DECLARE @Mensaje varchar(250)=''")

        sb.Append(" EXEC spc_PerfilCliente @CodCliente,@Estatus Output,@Mensaje Output")

        Return sb.ToString
    End Function
    Public Function FindAll(CodCliente As String) As DataTable
        Dim dt As DataTable
        Try
            dt = execQuery(GetSQL(CodCliente))
        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        End Try

        Return dt
    End Function
    Public Function FindlstAll(CodCliente As String) As List(Of PerfilClienteModel)
        Dim dt As DataTable
        Dim lst As New List(Of PerfilClienteModel)
        Try
            dt = FindAll(CodCliente)
            For Each dr As DataRow In dt.Rows
                Dim ObjM As PerfilClienteModel = FillModel(dr)
                lst.Add(ObjM)
            Next

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        Finally
            dt = Nothing
        End Try

        Return lst
    End Function
    Public Function FindById(CodCliente As String) As PerfilClienteModel
        Dim ObjM As New PerfilClienteModel
        Dim dt As DataTable
        Try
            dt = FindAll(CodCliente)
            If Not dt Is Nothing Then ObjM = FillModel(dt.Rows(0))

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        Finally
            dt = Nothing
        End Try
        Return ObjM
    End Function
    Public Function FillModel(dr As DataRow) As PerfilClienteModel
        Dim ObjModel As New PerfilClienteModel
        ObjModel.CodCliente = dr("CodCliente")
        ObjModel.NomCliente = dr("NomCliente")
        ObjModel.Pais = dr("Pais")
        ObjModel.CodALLIX = dr("CodALLIX")
        ObjModel.CveOrigen = dr("CveOrigen")
        ObjModel.NomOrigen = dr("NomOrigen")
        ObjModel.Dependencias = dr("Dependencias")

        'Bitacora
        ObjModel.CveEstatus = dr("CveEstatus")
        ObjModel.NomEstatus = dr("NomEstatus")
        ObjModel.FecAct = CDate(dr("FecAct")).ToString("dd/MM/yyyy HH:mm")
        ObjModel.UsuAct = dr("UsuAct")
        ObjModel.NomUsuAct = dr("NomUsuAct")

        Return ObjModel
    End Function

End Class
