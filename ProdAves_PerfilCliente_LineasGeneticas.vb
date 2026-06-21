Imports System.Data
Imports Microsoft.Ajax.Utilities
Imports NukaxanWEB
Imports NukaxanWEB.DataBase
Imports NukaxanWEB.Libreria
Public Class ProdAves_PerfilCliente_LineasGeneticas
    Public strError As String = ""
    Public Folio As String = ""
    Public Function GetSQL(CodCliente As String) As String
        Dim sb As New StringBuilder

        sb.Append(" DECLARE @CodCliente varchar(50)='" + CodCliente + "'")
        sb.Append(" DECLARE @Estatus int=0")
        sb.Append(" DECLARE @Mensaje varchar(250)=''")
        sb.Append(" EXEC spc_ProdAves_PerfilCliente_LineasGeneticas @CodCliente,@Estatus Output,@Mensaje Output")

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
    Public Function FindlstAll(CodCliente As String) As List(Of ProdAves_PerfilCliente_LineasGeneticasModel)
        Dim dt As DataTable
        Dim lst As New List(Of ProdAves_PerfilCliente_LineasGeneticasModel)
        Try
            dt = FindAll(CodCliente)
            For Each dr As DataRow In dt.Rows
                Dim ObjM As ProdAves_PerfilCliente_LineasGeneticasModel = FillModel(dr)
                lst.Add(ObjM)
            Next

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        Finally
            dt = Nothing
        End Try

        Return lst
    End Function
    Public Function FindById(CodCliente As String) As ProdAves_PerfilCliente_LineasGeneticasModel
        Dim ObjM As New ProdAves_PerfilCliente_LineasGeneticasModel
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
    Public Function FillModel(dr As DataRow) As ProdAves_PerfilCliente_LineasGeneticasModel
        Dim ObjModel As New ProdAves_PerfilCliente_LineasGeneticasModel
        ObjModel.CveLineaG = dr("CveLineaG")
        ObjModel.NomLineaG = dr("NomLineaG")
        ObjModel.CodeLineaG = dr("CodeLineaG")
        ObjModel.Dependencias = dr("Dependencias")
        ObjModel.Habilitada = dr("Habilitada")

        'Bitacora
        ObjModel.CveEstatus = dr("CveEstatus")
        ObjModel.NomEstatus = dr("NomEstatus")
        ObjModel.FecAct = CDate(dr("FecAct")).ToString("dd/MM/yyyy HH:mm")
        ObjModel.UsuAct = dr("UsuAct")
        ObjModel.NomUsuAct = dr("NomUsuAct")

        Return ObjModel
    End Function
    Public Function SaveModel(CodCliente As String, Valores As String, UsuAct As String) As Boolean
        Dim sb As New StringBuilder
        Dim dt As DataTable
        Dim IsResult As Boolean = False
        Folio = "0"
        Try
            sb.Append(" DECLARE @CodCliente varchar(50)='" + CodCliente + "'")
            sb.Append(" DECLARE @Valores varchar(MAX)='" + Valores + "'")
            sb.Append(" DECLARE @UsuAct varchar(50)='" + UsuAct + "'")
            sb.Append(" DECLARE @Estatus int=0")
            sb.Append(" DECLARE @Mensaje varchar(250)=''")
            sb.Append(" DECLARE @Id int=0")

            sb.Append(" EXEC spiud_ProdAve_PerfilCliente_LineasGeneticas @CodCliente,@Valores,@UsuAct,@Estatus Output,@Mensaje Output")

            dt = execQuery(sb.ToString)
            If Not dt Is Nothing And dt.Rows.Count > 0 Then
                If dt(0)("Estatus").ToString = "0" Then
                    IsResult = True
                Else
                    Throw New Exception(dt(0)("Mensaje").ToString)
                End If
            End If

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        Finally
            dt = Nothing
        End Try
        Return IsResult
    End Function

End Class
