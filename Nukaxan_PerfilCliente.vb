Imports System.Data
Imports Microsoft.Ajax.Utilities
Imports NukaxanWEB
Imports NukaxanWEB.DataBase
Imports NukaxanWEB.Libreria
Public Class Nukaxan_PerfilCliente
    Public strError As String = ""
    Public Folio As String = ""
    Public Function GetSQL(CvePlataforma As Integer, CodUsuario As String, CodCliente As String) As String
        Dim sb As New StringBuilder

        sb.Append(" DECLARE @CvePlataforma int=" + CvePlataforma.ToString)
        sb.Append(" DECLARE @CodUsuario varchar(50) = '" + CodUsuario + "'")
        sb.Append(" DECLARE @CodCliente varchar(50) = '" + CodCliente + "'")
        sb.Append(" DECLARE @Estatus int=0")
        sb.Append(" DECLARE @Mensaje varchar(250)=''")

        sb.Append(" EXEC spc_Nukaxan_PerfilCliente @CvePlataforma,@CodUsuario,@CodCliente,@Estatus Output,@Mensaje Output")

        Return sb.ToString
    End Function
    Public Function FindAll(CvePlataforma As Integer, CodUsuario As String, CodCliente As String) As DataTable
        Dim dt As DataTable
        Try
            dt = execQuery(GetSQL(CvePlataforma, CodUsuario, CodCliente))
        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        End Try

        Return dt
    End Function
    Public Function FindlstAll(CvePlataforma As Integer, CodUsuario As String, CodCliente As String) As List(Of Nukaxan_PerfilClienteModel)
        Dim dt As DataTable
        Dim lst As New List(Of Nukaxan_PerfilClienteModel)
        Try
            dt = FindAll(CvePlataforma, CodUsuario, CodCliente)
            For Each dr As DataRow In dt.Rows
                Dim ObjM As Nukaxan_PerfilClienteModel = FillModel(dr)
                lst.Add(ObjM)
            Next

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        Finally
            dt = Nothing
        End Try

        Return lst
    End Function
    Public Function FindById(CvePlataforma As Integer, CodUsuario As String, CodCliente As String) As Nukaxan_PerfilClienteModel
        Dim ObjM As New Nukaxan_PerfilClienteModel
        Dim dt As DataTable
        Try
            dt = FindAll(CvePlataforma, CodUsuario, CodCliente)
            If Not dt Is Nothing Then ObjM = FillModel(dt.Rows(0))

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        Finally
            dt = Nothing
        End Try
        Return ObjM
    End Function
    Public Function FillModel(dr As DataRow) As Nukaxan_PerfilClienteModel
        Dim ObjModel As New Nukaxan_PerfilClienteModel
        ObjModel.CodCliente = dr("CodCliente")
        ObjModel.NomCliente = dr("NomCliente")
        ObjModel.NomClienteA = dr("NomClienteA")
        ObjModel.Pais = dr("Pais")
        ObjModel.CodALLIX = dr("CodALLIX")
        ObjModel.CveOrigen = dr("CveOrigen")
        ObjModel.NomOrigen = dr("NomOrigen")
        ObjModel.NomClienteR = dr("NomClienteR")
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
