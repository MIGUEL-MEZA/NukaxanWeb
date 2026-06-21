Imports System.Data
Imports NukaxanWEB
Imports NukaxanWEB.DataBase
Imports NukaxanWEB.Libreria
Public Class Mensajes
    Public strError As String = ""
    Public Function GetSQL(CveMenu As String, CveTipo As Integer, CveMensaje As Integer) As String
        Dim sb As New StringBuilder

        sb.Append(" DECLARE @CveMenu varchar(MAX) ='" + CveMenu + "'")
        sb.Append(" DECLARE @CveTipo int =" + CveTipo.ToString)
        sb.Append(" DECLARE @CveMensaje int =" + CveMensaje.ToString)
        sb.Append(" DECLARE @Estatus int=0")
        sb.Append(" DECLARE @Mensaje varchar(250)=''")

        sb.Append(" EXEC spc_Mensajes @CveMenu,@CveTipo,@CveMensaje,@Estatus Output,@Mensaje Output")

        Return sb.ToString
    End Function
    Public Function FindAll(CveMenu As String, CveTipo As Integer, CveMensaje As Integer) As DataTable
        Dim dt As DataTable
        Try
            dt = execQuery(GetSQL(CveMenu, CveTipo, CveMensaje))
        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        End Try

        Return dt
    End Function
    Public Function FindlstAll(CveMenu As String, CveTipo As Integer, CveMensaje As Integer) As List(Of MensajesModel)
        Dim dt As DataTable
        Dim lst As New List(Of MensajesModel)
        Try
            dt = FindAll(CveMenu, CveTipo, CveMensaje)
            For Each dr As DataRow In dt.Rows
                Dim ObjM As MensajesModel = FillModel(dr)
                lst.Add(ObjM)
            Next

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        Finally
            dt = Nothing
        End Try

        Return lst
    End Function
    Public Function FindById(CveMenu As String, CveTipo As Integer, CveMensaje As Integer) As MensajesModel
        Dim ObjM As New MensajesModel
        Dim dt As DataTable
        Try
            dt = FindAll(CveMenu, CveTipo, CveMensaje)
            If Not dt Is Nothing Then ObjM = FillModel(dt.Rows(0))

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        Finally
            dt = Nothing
        End Try
        Return ObjM
    End Function
    Public Function FillModel(dr As DataRow) As MensajesModel
        Dim ObjModel As New MensajesModel
        ObjModel.CveMenu = dr("CveMenu")
        ObjModel.CveTipo = dr("CveTipo")
        ObjModel.CveMensaje = dr("CveMensaje")
        ObjModel.NomMensaje = dr("NomMensaje")
        ObjModel.NomTipo = dr("NomTipo")
        ObjModel.Clase = dr("Clase")
        ObjModel.Icono = dr("Icono")
        Return ObjModel
    End Function
End Class
