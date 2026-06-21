Imports System.Data
Imports Microsoft.Ajax.Utilities
Imports NukaxanWEB
Imports NukaxanWEB.DataBase
Imports NukaxanWEB.Libreria
Public Class Controles_Acciones
    Dim strSQLExe As String = ""
    Public strError As String = ""
    Public Folio As String = 0
    Public Function getSQL(CveTipo As String, CveAccion As Integer) As String
        Dim sb As New StringBuilder
        sb.Append(" DECLARE @CveTipo varchar(MAX)= '" + CveTipo.ToString + "'")
        sb.Append(" DECLARE @CveAccion int=" + CveAccion.ToString)
        sb.Append(" DECLARE @Estatus int=0")
        sb.Append(" DECLARE @Mensaje varchar(250)=''")
        sb.Append(" EXEC spc_ControlesAcciones @CveTipo,@CveAccion,@Estatus Output,@Mensaje Output")

        Return sb.ToString
    End Function
    Public Function FindAll(CveTipo As String, CveAccion As Integer) As DataTable
        strError = ""
        Dim dt As DataTable
        Try
            strSQLExe = getSQL(CveTipo, CveAccion)
            dt = execQuery(strSQLExe)

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        End Try

        Return dt
    End Function
    Public Function FindlstAll(CveTipo As String, CveAccion As Integer) As List(Of Controles_AccionesModel)
        Dim dt As DataTable
        Dim lst As New List(Of Controles_AccionesModel)
        Try
            dt = FindAll(CveTipo, CveAccion)
            For Each dr As DataRow In dt.Rows
                Dim ObjM As Controles_AccionesModel = FillModel(dr)
                lst.Add(ObjM)
            Next

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        Finally
            dt = Nothing
        End Try

        Return lst
    End Function
    Public Function FindById(CveTipo As String, CveAccion As Integer) As Controles_AccionesModel
        Dim dt As DataTable
        Dim ObjM As Controles_AccionesModel
        Try
            dt = FindAll(CveTipo, CveAccion)
            If Not dt Is Nothing Then ObjM = FillModel(dt.Rows(0))

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        Finally
            dt = Nothing
        End Try

        Return ObjM
    End Function
    Public Function FillModel(dr As DataRow) As Controles_AccionesModel
        Dim ObjModel As New Controles_AccionesModel
        ObjModel.CveTipo = dr("CveTipo")
        ObjModel.CveAccion = dr("CveAccion")
        ObjModel.NomAccion = dr("NomAccion")
        ObjModel.Icono = dr("Icono")
        ObjModel.IconoSize = dr("IconoSize")
        ObjModel.ToolTip = dr("ToolTip")
        ObjModel.NomTipo = dr("NomTipo")
        ObjModel.ValidaClick = dr("ValidaClick")
        ObjModel.ValidaMensaje = dr("ValidaMensaje")

        Return ObjModel
    End Function


End Class