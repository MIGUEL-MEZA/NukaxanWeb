Imports System.Data
Imports Microsoft.Ajax.Utilities
Imports NukaxanWEB
Imports NukaxanWEB.DataBase
Imports NukaxanWEB.Libreria
Public Class ProdAves_Lotes
    Public strError As String = ""
    Public Folio As String = ""
    Public Function GetSQL(CodCliente As String, CveLote As Integer, CveEtapa As Integer, CveEstatus As Integer) As String
        Dim sb As New StringBuilder

        sb.Append(" DECLARE @CodCliente varchar(50)='" + CodCliente + "'")
        sb.Append(" DECLARE @CveLote int=" + CveLote.ToString)
        sb.Append(" DECLARE @CveEtapa int=" + CveEtapa.ToString)
        sb.Append(" DECLARE @CveEstatus int=" + CveEstatus.ToString)
        sb.Append(" DECLARE @Estatus int=0")
        sb.Append(" DECLARE @Mensaje varchar(250)=''")

        sb.Append(" EXEC spc_ProdAves_Lotes @CodCliente,@CveLote,@CveEtapa,@CveEstatus,@Estatus Output,@Mensaje Output")

        Return sb.ToString
    End Function
    Public Function FindAll(CodCliente As String, CveLote As Integer, CveEtapa As Integer, CveEstatus As Integer) As DataTable
        Dim dt As DataTable
        Try
            dt = execQuery(GetSQL(CodCliente, CveLote, CveEtapa, CveEstatus))
        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        End Try

        Return dt
    End Function
    Public Function FindlstAll(CodCliente As String, CveLote As Integer, CveEtapa As Integer, CveEstatus As Integer) As List(Of ProdAves_LotesModel)
        Dim dt As DataTable
        Dim lst As New List(Of ProdAves_LotesModel)
        Try
            dt = FindAll(CodCliente, CveLote, CveEtapa, CveEstatus)
            For Each dr As DataRow In dt.Rows
                Dim ObjM As ProdAves_LotesModel = FillModel(dr)
                lst.Add(ObjM)
            Next

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        Finally
            dt = Nothing
        End Try

        Return lst
    End Function
    Public Function FindById(CodCliente As String, CveLote As Integer, CveEtapa As Integer, CveEstatus As Integer) As ProdAves_LotesModel
        Dim ObjM As New ProdAves_LotesModel
        Dim dt As DataTable
        Try
            dt = FindAll(CodCliente, CveLote, CveEtapa, CveEstatus)
            If Not dt Is Nothing Then ObjM = FillModel(dt.Rows(0))

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        Finally
            dt = Nothing
        End Try
        Return ObjM
    End Function
    Public Function FillModel(dr As DataRow) As ProdAves_LotesModel
        Dim ObjModel As New ProdAves_LotesModel
        ObjModel.CodCliente = dr("CodCliente")
        ObjModel.CveLote = dr("CveLote")
        ObjModel.CveEtapa = dr("CveEtapa")
        ObjModel.CodLote = dr("CodLote")
        ObjModel.CveLoteO = dr("CveLoteO")
        ObjModel.CveGranja = dr("CveGranja")
        ObjModel.CveLineaG = dr("CveLineaG")
        ObjModel.CveGuia = dr("CveGuia")
        ObjModel.FecNacimiento = If(IsDBNull(dr("FecNacimiento")), "", CDate(dr("FecNacimiento")).ToString("dd/MM/yyyy"))
        ObjModel.FecRecepcion = If(IsDBNull(dr("FecRecepcion")), "", CDate(dr("FecRecepcion")).ToString("dd/MM/yyyy"))
        ObjModel.FecCierre = If(IsDBNull(dr("FecCierre")), "", CDate(dr("FecCierre")).ToString("dd/MM/yyyy"))
        ObjModel.AvesIniciales = dr("AvesIniciales")
        ObjModel.AvesFinales = dr("AvesFinales")
        ObjModel.Edad = dr("Edad")
        ObjModel.NumCasetas = dr("NumCasetas")
        ObjModel.NomCliente = dr("NomCliente")
        ObjModel.NomEtapa = dr("NomEtapa")
        ObjModel.CodLoteO = dr("CodLoteO")
        ObjModel.NomGranja = dr("NomGranja")
        ObjModel.NomLineaG = dr("NomLineaG")
        ObjModel.NomGuia = dr("NomGuia")
        ObjModel.Dependencias = dr("Dependencias")

        'Bitacora
        ObjModel.CveEstatus = dr("CveEstatus")
        ObjModel.NomEstatus = dr("NomEstatus")
        ObjModel.FecAct = CDate(dr("FecAct")).ToString("dd/MM/yyyy HH:mm")
        ObjModel.UsuAct = dr("UsuAct")
        ObjModel.NomUsuAct = dr("NomUsuAct")

        Return ObjModel
    End Function

    Public Function SaveModel(Valores As String, UsuAct As String) As Boolean
        Dim sb As New StringBuilder
        Dim dt As DataTable
        Dim IsResult As Boolean = False
        Folio = "0"
        Try
            sb.Append(" DECLARE @Opcion int =0")
            sb.Append(" DECLARE @Valores varchar(MAX)='" + Valores + "'")
            sb.Append(" DECLARE @UsuAct varchar(50)='" + UsuAct + "'")
            sb.Append(" DECLARE @Estatus int=0")
            sb.Append(" DECLARE @Mensaje varchar(250)=''")
            sb.Append(" DECLARE @Id int=0")

            sb.Append(" EXEC spiud_ProdAves_Lotes @Opcion,@Valores,@UsuAct,@Estatus Output,@Mensaje Output,@Id Output")

            dt = execQuery(sb.ToString)
            If Not dt Is Nothing And dt.Rows.Count > 0 Then
                If dt(0)("Estatus").ToString = "0" Then
                    Folio = dt(0)("Id").ToString
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
    Public Function DeleteModel(CodCliente As String, CveLote As Integer) As Boolean
        Dim sb As New StringBuilder
        Dim dt As DataTable
        Dim IsResult As Boolean = False
        Folio = "0"
        Try
            sb.Append(" DECLARE @Opcion int =2")
            sb.Append(" DECLARE @Valores varchar(MAX)=''")
            sb.Append(" DECLARE @UsuAct varchar(50)=''")
            sb.Append(" DECLARE @Estatus int=0")
            sb.Append(" DECLARE @Mensaje varchar(250)=''")
            sb.Append(" DECLARE @Id int=0")

            sb.Append(" EXEC spiud_ProdAves_Lotes @Opcion,@Valores,@UsuAct,@Estatus Output,@Mensaje Output,@Id Output")

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
