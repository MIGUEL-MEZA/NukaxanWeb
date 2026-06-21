Imports System.Data
Imports Microsoft.Ajax.Utilities
Imports NukaxanWEB
Imports NukaxanWEB.DataBase
Imports NukaxanWEB.Libreria
Public Class Controles_Captura
    Dim strSQLExe As String = ""
    Public strError As String = ""
    Public Folio As String = 0
    Public Function getSQL(CvePlataforma As Integer, CveMenu As Integer, CveControl As Integer) As String
        Dim sb As New StringBuilder
        sb.Append(" DECLARE @CvePlataforma int=" + CvePlataforma.ToString)
        sb.Append(" DECLARE @CveMenu int=" + CveMenu.ToString)
        sb.Append(" DECLARE @CveControl int=" + CveControl.ToString)
        sb.Append(" DECLARE @Estatus int=0")
        sb.Append(" DECLARE @Mensaje varchar(250)=''")
        sb.Append(" EXEC spc_ControlesCaptura @CvePlataforma,@CveMenu,@CveControl,@Estatus Output,@Mensaje Output")

        Return sb.ToString
    End Function
    Public Function FindAll(CvePlataforma As Integer, CveMenu As Integer, CveControl As Integer) As DataTable
        strError = ""
        Dim dt As DataTable
        Try
            strSQLExe = getSQL(CvePlataforma, CveMenu, CveControl)
            dt = execQuery(strSQLExe)

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        End Try

        Return dt
    End Function
    Public Function FindlstAll(CvePlataforma As Integer, CveMenu As Integer, CveControl As Integer) As List(Of Controles_CapturaModel)
        Dim dt As DataTable
        Dim lst As New List(Of Controles_CapturaModel)
        Try
            dt = FindAll(CvePlataforma, CveMenu, CveControl)
            For Each dr As DataRow In dt.Rows
                Dim ObjM As Controles_CapturaModel = FillModel(dr)
                lst.Add(ObjM)
            Next

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        Finally
            dt = Nothing
        End Try

        Return lst
    End Function
    Public Function FindById(CvePlataforma As Integer, CveMenu As Integer, CveControl As Integer) As Controles_CapturaModel
        Dim dt As DataTable
        Dim ObjM As Controles_CapturaModel
        Try
            dt = FindAll(CvePlataforma, CveMenu, CveControl)
            If Not dt Is Nothing Then ObjM = FillModel(dt.Rows(0))

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        Finally
            dt = Nothing
        End Try

        Return ObjM
    End Function
    Public Function FillModel(dr As DataRow) As Controles_CapturaModel
        Dim ObjModel As New Controles_CapturaModel
        ObjModel.CvePlataforma = dr("CvePlataforma")
        ObjModel.CveMenu = dr("CveMenu")
        ObjModel.CveControl = dr("CveControl")
        ObjModel.CveMSC = dr("CveMSC")
        ObjModel.CveEtapa = dr("CveEtapa")
        ObjModel.Control = dr("Control")
        ObjModel.CveTipo = dr("CveTipo")
        ObjModel.Campo = dr("Campo")
        ObjModel.Etiqueta = dr("Etiqueta")
        ObjModel.Mensaje = dr("Mensaje")
        ObjModel.Editable = dr("Editable")
        ObjModel.Obligatorio = dr("Obligatorio")
        ObjModel.ValidaRango = dr("ValidaRango")
        ObjModel.NomTipo = dr("NomTipo")

        Return ObjModel
    End Function


End Class