Imports System.Data
Imports Microsoft.Ajax.Utilities
Imports NukaxanWEB
Imports NukaxanWEB.DataBase
Imports NukaxanWEB.Libreria
Public Class ProdAves_Lotes_Recepcion
    Public strError As String = ""
    Public Folio As String = ""
    Public Function GetSQL(CodCliente As String, CveLote As Integer, CveLoteR As Integer) As String
        Dim sb As New StringBuilder

        sb.Append(" DECLARE @CodCliente varchar(50)='" + CodCliente + "'")
        sb.Append(" DECLARE @CveLote int=" + CveLote.ToString)
        sb.Append(" DECLARE @CveLoteR int=" + CveLoteR.ToString)
        sb.Append(" DECLARE @Estatus int=0")
        sb.Append(" DECLARE @Mensaje varchar(250)=''")

        sb.Append(" EXEC spc_ProdAves_Lotes_Recepcion @CodCliente,@CveLote,@CveLoteR,@Estatus Output,@Mensaje Output")

        Return sb.ToString
    End Function
    Public Function FindAll(CodCliente As String, CveLote As Integer, CveLoteR As Integer) As DataTable
        Dim dt As DataTable
        Try
            dt = execQuery(GetSQL(CodCliente, CveLote, CveLoteR))
        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        End Try

        Return dt
    End Function
    Public Function FindlstAll(CodCliente As String, CveLote As Integer, CveLoteR As Integer) As List(Of ProdAves_Lotes_RecepcionModel)
        Dim dt As DataTable
        Dim lst As New List(Of ProdAves_Lotes_RecepcionModel)
        Try
            dt = FindAll(CodCliente, CveLote, CveLoteR)
            For Each dr As DataRow In dt.Rows
                Dim ObjM As ProdAves_Lotes_RecepcionModel = FillModel(dr)
                lst.Add(ObjM)
            Next

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        Finally
            dt = Nothing
        End Try

        Return lst
    End Function
    Public Function FindById(CodCliente As String, CveLote As Integer, CveLoteR As Integer) As ProdAves_Lotes_RecepcionModel
        Dim ObjM As New ProdAves_Lotes_RecepcionModel
        Dim dt As DataTable
        Try
            dt = FindAll(CodCliente, CveLote, CveLoteR)
            If Not dt Is Nothing Then ObjM = FillModel(dt.Rows(0))

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        Finally
            dt = Nothing
        End Try
        Return ObjM
    End Function
    Public Function FillModel(dr As DataRow) As ProdAves_Lotes_RecepcionModel
        Dim ObjModel As New ProdAves_Lotes_RecepcionModel
        ObjModel.CodCliente = dr("CodCliente")
        ObjModel.CveLote = dr("CveLote")
        ObjModel.CveLoteR = dr("CveLoteR")
        ObjModel.CveGranja = dr("CveGranja")
        ObjModel.CveCaseta = dr("CveCaseta")
        ObjModel.FecRecepcion = If(IsDBNull(dr("FecRecepcion")), "", CDate(dr("FecRecepcion")).ToString("dd/MM/yyyy"))
        ObjModel.FecCierre = If(IsDBNull(dr("FecCierre")), "", CDate(dr("FecCierre")).ToString("dd/MM/yyyy"))
        ObjModel.AvesAlojadas = dr("AvesAlojadas")
        ObjModel.AvesFinales = dr("AvesFinales")
        ObjModel.PesoInicial = dr("PesoInicial")
        ObjModel.Edad = dr("Edad")
        ObjModel.NumDia = dr("NumDia")
        ObjModel.NomCaseta = dr("NomCaseta")
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

            sb.Append(" EXEC spiud_ProdAves_Lotes_Recepcion @Opcion,@Valores,@UsuAct,@Estatus Output,@Mensaje Output,@Id Output")

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
    Public Function DeleteModel(Valores As String) As Boolean
        Dim sb As New StringBuilder
        Dim dt As DataTable
        Dim IsResult As Boolean = False
        Folio = "0"
        Try
            sb.Append(" DECLARE @Opcion int =2")
            sb.Append(" DECLARE @Valores varchar(MAX)='" + Valores + "'")
            sb.Append(" DECLARE @UsuAct varchar(50)=''")
            sb.Append(" DECLARE @Estatus int=0")
            sb.Append(" DECLARE @Mensaje varchar(250)=''")
            sb.Append(" DECLARE @Id int=0")

            sb.Append(" EXEC spiud_ProdAves_Lotes_Recepcion @Opcion,@Valores,@UsuAct,@Estatus Output,@Mensaje Output,@Id Output")

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
