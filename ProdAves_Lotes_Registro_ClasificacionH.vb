Imports System.Data
Imports Microsoft.Ajax.Utilities
Imports NukaxanWEB
Imports NukaxanWEB.DataBase
Imports NukaxanWEB.Libreria
Public Class ProdAves_Lotes_Registro_ClasificacionH
    Public strError As String = ""
    Public Folio As String = ""
    Public Function GetSQL(CodCliente As String, CveLote As Integer, CveLoteR As Integer, CveRegistro As Integer, CveClasificacionH As Integer) As String
        Dim sb As New StringBuilder

        sb.Append(" DECLARE @CodCliente varchar(50)='" + CodCliente + "'")
        sb.Append(" DECLARE @CveLote int=" + CveLote.ToString)
        sb.Append(" DECLARE @CveLoteR int=" + CveLoteR.ToString)
        sb.Append(" DECLARE @CveRegistro int=" + CveRegistro.ToString)
        sb.Append(" DECLARE @CveClasificacionH int=" + CveClasificacionH.ToString)
        sb.Append(" DECLARE @Estatus int=0")
        sb.Append(" DECLARE @Mensaje varchar(250)=''")

        sb.Append(" EXEC spc_ProdAves_Lotes_Registro_ClasificacionH @CodCliente,@CveLote,@CveLoteR,@CveRegistro,@CveClasificacionH,@Estatus Output,@Mensaje Output")

        Return sb.ToString
    End Function
    Public Function FindAll(CodCliente As String, CveLote As Integer, CveLoteR As Integer, CveRegistro As Integer, CveClasificacionH As Integer) As DataTable
        Dim dt As DataTable
        Try
            dt = execQuery(GetSQL(CodCliente, CveLote, CveLoteR, CveRegistro, CveClasificacionH))
        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        End Try

        Return dt
    End Function
    Public Function FindlstAll(CodCliente As String, CveLote As Integer, CveLoteR As Integer, CveRegistro As Integer, CveClasificacionH As Integer) As List(Of ProdAves_Lotes_Registro_ClasificacionHModel)
        Dim dt As DataTable
        Dim lst As New List(Of ProdAves_Lotes_Registro_ClasificacionHModel)
        Try
            dt = FindAll(CodCliente, CveLote, CveLoteR, CveRegistro, CveClasificacionH)
            For Each dr As DataRow In dt.Rows
                Dim ObjM As ProdAves_Lotes_Registro_ClasificacionHModel = FillModel(dr)
                lst.Add(ObjM)
            Next

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        Finally
            dt = Nothing
        End Try

        Return lst
    End Function
    Public Function FindById(CodCliente As String, CveLote As Integer, CveLoteR As Integer, CveRegistro As Integer, CveClasificacionH As Integer) As ProdAves_Lotes_Registro_ClasificacionHModel
        Dim ObjM As New ProdAves_Lotes_Registro_ClasificacionHModel
        Dim dt As DataTable
        Try
            dt = FindAll(CodCliente, CveLote, CveLoteR, CveRegistro, CveClasificacionH)
            If Not dt Is Nothing Then ObjM = FillModel(dt.Rows(0))

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        Finally
            dt = Nothing
        End Try
        Return ObjM
    End Function
    Public Function FillModel(dr As DataRow) As ProdAves_Lotes_Registro_ClasificacionHModel
        Dim ObjModel As New ProdAves_Lotes_Registro_ClasificacionHModel
        ObjModel.CodCliente = dr("CodCliente")
        ObjModel.CveLote = dr("CveLote")
        ObjModel.CveLoteR = dr("CveLoteR")
        ObjModel.CveRegistro = dr("CveRegistro")
        ObjModel.CveClasificacionH = dr("CveClasificacionH")
        ObjModel.NomClasificacionH = dr("NomClasificacionH")
        ObjModel.Total = dr("Total")
        ObjModel.Porcentaje = dr("Porcentaje")
        ObjModel.Cajas = dr("Cajas")
        ObjModel.Conos = dr("Conos")
        ObjModel.Piezas = dr("Piezas")

        'Bitacora
        ObjModel.FecAct = If(IsDBNull(dr("FecAct")), "", CDate(dr("FecAct")).ToString("dd/MM/yyyy HH:mm"))
        ObjModel.UsuAct = dr("UsuAct")

        Return ObjModel
    End Function

End Class
