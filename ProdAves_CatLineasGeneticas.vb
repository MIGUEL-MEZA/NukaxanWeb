Imports System.Data
Imports Microsoft.Ajax.Utilities
Imports NukaxanWEB
Imports NukaxanWEB.DataBase
Imports NukaxanWEB.Libreria
Public Class ProdAves_CatLineasGeneticas
    Public strError As String = ""
    Public Folio As String = ""
    Public Function GetSQL(CveLineaG As Integer) As String
        Dim sb As New StringBuilder

        sb.Append(" DECLARE @CveLineaG int=" + CveLineaG.ToString)
        sb.Append(" DECLARE @Estatus int=0")
        sb.Append(" DECLARE @Mensaje varchar(250)=''")

        sb.Append(" EXEC spc_ProdAves_CatLineasGeneticas @CveLineaG,@Estatus Output,@Mensaje Output")

        Return sb.ToString
    End Function
    Public Function FindAll(CveLineaG As Integer) As DataTable
        Dim dt As DataTable
        Try
            dt = execQuery(GetSQL(CveLineaG))
        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        End Try

        Return dt
    End Function
    Public Function FindlstAll(CveLineaG As Integer) As List(Of ProdAves_CatLineasGeneticasModel)
        Dim dt As DataTable
        Dim lst As New List(Of ProdAves_CatLineasGeneticasModel)
        Try
            dt = FindAll(CveLineaG)
            For Each dr As DataRow In dt.Rows
                Dim ObjM As ProdAves_CatLineasGeneticasModel = FillModel(dr)
                lst.Add(ObjM)
            Next

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        Finally
            dt = Nothing
        End Try

        Return lst
    End Function
    Public Function FindById(CveLineaG As Integer) As ProdAves_CatLineasGeneticasModel
        Dim ObjM As New ProdAves_CatLineasGeneticasModel
        Dim dt As DataTable
        Try
            dt = FindAll(CveLineaG)
            If Not dt Is Nothing Then ObjM = FillModel(dt.Rows(0))

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        Finally
            dt = Nothing
        End Try
        Return ObjM
    End Function
    Public Function FillModel(dr As DataRow) As ProdAves_CatLineasGeneticasModel
        Dim ObjModel As New ProdAves_CatLineasGeneticasModel
        ObjModel.CveLineaG = dr("CveLineaG")
        ObjModel.NomLineaG = dr("NomLineaG")
        ObjModel.CodeLineaG = dr("CodeLineaG")
        ObjModel.EdadIni_Crianza = dr("EdadIni_Crianza")
        ObjModel.EdadFin_Crianza = dr("EdadFin_Crianza")
        ObjModel.EdadIni_Postura = dr("EdadIni_Postura")
        ObjModel.EdadFin_Postura = dr("EdadFin_Postura")
        ObjModel.Dependencias = dr("Dependencias")

        'Bitacora
        ObjModel.CveEstatus = dr("CveEstatus")
        ObjModel.NomEstatus = dr("NomEstatus")
        ObjModel.FecAct = CDate(dr("FecAct")).ToString("dd/MM/yyyy HH:mm")
        ObjModel.UsuAct = dr("UsuAct")
        ObjModel.NomUsuAct = dr("NomUsuAct")

        Return ObjModel
    End Function

    Public Function SaveModel(CveLineaG As Integer, Valores As String, UsuAct As Int64) As Boolean
        Dim sb As New StringBuilder
        Dim dt As DataTable
        Dim IsResult As Boolean = False
        Folio = "0"
        Try
            sb.Append(" DECLARE @Opcion int =0")
            sb.Append(" DECLARE @CveLineaG int =" + CveLineaG.ToString)
            sb.Append(" DECLARE @Valores varchar(MAX)='" + Valores + "'")
            sb.Append(" DECLARE @UsuAct bigint=" + UsuAct.ToString)
            sb.Append(" DECLARE @Estatus int=0")
            sb.Append(" DECLARE @Mensaje varchar(250)=''")

            sb.Append(" EXEC spiud_ProdAves_CatLineasGeneticas @Opcion,@CveLineaG,@Valores,@UsuAct,@Estatus Output,@Mensaje Output")

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
    Public Function DeleteModel(CveLineaG As Integer) As Boolean
        Dim sb As New StringBuilder
        Dim dt As DataTable
        Dim IsResult As Boolean = False
        Folio = "0"
        Try
            sb.Append(" DECLARE @Opcion int =2")
            sb.Append(" DECLARE @CveLineaG int =" + CveLineaG.ToString)
            sb.Append(" DECLARE @Valores varchar(MAX)=''")
            sb.Append(" DECLARE @UsuAct bigint=0")
            sb.Append(" DECLARE @Estatus int=0")
            sb.Append(" DECLARE @Mensaje varchar(250)=''")

            sb.Append(" EXEC spiud_ProdAves_CatLineasGeneticas @Opcion,@CveLineaG,@Valores,@UsuAct,@Estatus Output,@Mensaje Output")

            dt = execQuery(sb.ToString)
            If Not dt Is Nothing And dt.Rows.Count > 0 Then
                If dt(0)("Estatus").ToString = "0" Then
                    IsResult = True
                ElseIf dt(0)("Estatus").ToString = "-2" Then
                    Throw New Exception(New Mensajes().FindById("0", 0, 19).NomMensaje)
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
