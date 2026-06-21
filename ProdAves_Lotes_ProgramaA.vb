Imports System.Data
Imports Microsoft.Ajax.Utilities
Imports NukaxanWEB
Imports NukaxanWEB.DataBase
Imports NukaxanWEB.Libreria
Public Class ProdAves_Lotes_ProgramaA
    Public strError As String = ""
    Public Folio As String = ""
    Public Function GetSQL(CodCliente As String, CveLote As Integer, CveProgramaA As Integer) As String
        Dim sb As New StringBuilder

        sb.Append(" DECLARE @CodCliente varchar(50)='" + CodCliente + "'")
        sb.Append(" DECLARE @CveLote int=" + CveLote.ToString)
        sb.Append(" DECLARE @CveProgramaA int=" + CveProgramaA.ToString)
        sb.Append(" DECLARE @Estatus int=0")
        sb.Append(" DECLARE @Mensaje varchar(250)=''")

        sb.Append(" EXEC spc_ProdAves_Lotes_ProgramaA @CodCliente,@CveLote,@CveProgramaA,@Estatus Output,@Mensaje Output")

        Return sb.ToString
    End Function
    Public Function FindAll(CodCliente As String, CveLote As Integer, CveProgramaA As Integer) As DataTable
        Dim dt As DataTable
        Try
            dt = execQuery(GetSQL(CodCliente, CveLote, CveProgramaA))
        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        End Try

        Return dt
    End Function
    Public Function FindlstAll(CodCliente As String, CveLote As Integer, CveProgramaA As Integer) As List(Of ProdAves_Lotes_ProgramaAModel)
        Dim dt As DataTable
        Dim lst As New List(Of ProdAves_Lotes_ProgramaAModel)
        Try
            dt = FindAll(CodCliente, CveLote, CveProgramaA)
            For Each dr As DataRow In dt.Rows
                Dim ObjM As ProdAves_Lotes_ProgramaAModel = FillModel(dr)
                lst.Add(ObjM)
            Next

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        Finally
            dt = Nothing
        End Try

        Return lst
    End Function
    Public Function FindById(CodCliente As String, CveLote As Integer, CveProgramaA As Integer) As ProdAves_Lotes_ProgramaAModel
        Dim ObjM As New ProdAves_Lotes_ProgramaAModel
        Dim dt As DataTable
        Try
            dt = FindAll(CodCliente, CveLote, CveProgramaA)
            If Not dt Is Nothing Then ObjM = FillModel(dt.Rows(0))

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        Finally
            dt = Nothing
        End Try
        Return ObjM
    End Function
    Public Function FillModel(dr As DataRow) As ProdAves_Lotes_ProgramaAModel
        Dim ObjModel As New ProdAves_Lotes_ProgramaAModel
        ObjModel.CodCliente = dr("CodCliente")
        ObjModel.CveLote = dr("CveLote")
        ObjModel.CveProgramaA = dr("CveProgramaA")
        ObjModel.CveEtapa = dr("CveEtapa")
        ObjModel.CveFase = dr("CveFase")
        ObjModel.EdadIni = dr("EdadIni")
        ObjModel.EdadFin = dr("EdadFin")
        ObjModel.NomAlimento = dr("NomAlimento")
        ObjModel.Costo = dr("Costo")
        ObjModel.NomEtapa = dr("NomEtapa")
        ObjModel.NomFase = dr("NomFase")

        'Bitacora
        ObjModel.CveEstatus = dr("CveEstatus")
        ObjModel.NomEstatus = dr("NomEstatus")
        ObjModel.FecAct = CDate(dr("FecAct")).ToString("dd/MM/yyyy HH:mm")
        ObjModel.UsuAct = dr("UsuAct")
        ObjModel.NomUsuAct = dr("NomUsuAct")

        Return ObjModel
    End Function

    Public Function SaveModel(Opcion As Integer, Valores As String, UsuAct As String) As Boolean
        Dim sb As New StringBuilder
        Dim dt As DataTable
        Dim IsResult As Boolean = False
        Folio = "0"
        Try
            sb.Append(" DECLARE @Opcion int =" + Opcion.ToString)
            sb.Append(" DECLARE @Valores varchar(MAX)='" + Valores + "'")
            sb.Append(" DECLARE @UsuAct varchar(50)='" + UsuAct + "'")
            sb.Append(" DECLARE @Estatus int=0")
            sb.Append(" DECLARE @Mensaje varchar(250)=''")
            sb.Append(" DECLARE @Id int=0")

            sb.Append(" EXEC spiud_ProdAves_Lotes_ProgramaA @Opcion,@Valores,@UsuAct,@Estatus Output,@Mensaje Output,@Id Output")

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

            sb.Append(" EXEC spiud_ProdAves_Lotes_ProgramaA @Opcion,@Valores,@UsuAct,@Estatus Output,@Mensaje Output,@Id Output")

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
