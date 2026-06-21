Imports System.Data
Imports NukaxanWEB
Imports NukaxanWEB.DataBase
Imports NukaxanWEB.Libreria
Public Class Etiquetas
    Public strError As String = ""
    Public Function GetSQL(CvePlataforma As String, CveMenu As String, CveEtiqueta As Integer) As String
        Dim sb As New StringBuilder

        sb.Append(" DECLARE @CvePlataforma varchar(MAX) ='" + CvePlataforma + "'")
        sb.Append(" DECLARE @CveMenu varchar(MAX) ='" + CveMenu + "'")
        sb.Append(" DECLARE @CveEtiqueta  int=" + CveEtiqueta.ToString)
        sb.Append(" DECLARE @Estatus int=0")
        sb.Append(" DECLARE @Mensaje varchar(250)=''")

        sb.Append(" EXEC spc_Etiquetas @CvePlataforma,@CveMenu,@CveEtiqueta,@Estatus Output,@Mensaje Output")

        Return sb.ToString
    End Function
    Public Function FindAll(CvePlataforma As String, CveMenu As String, CveEtiqueta As Integer) As DataTable
        Dim dt As DataTable
        Try
            dt = execQuery(GetSQL(CvePlataforma, CveMenu, CveEtiqueta))
        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        End Try

        Return dt
    End Function
    Public Function FindlstAll(CvePlataforma As String, CveMenu As String, CveEtiqueta As Integer) As List(Of EtiquetasModel)
        Dim dt As DataTable
        Dim lst As New List(Of EtiquetasModel)
        Try
            dt = FindAll(CvePlataforma, CveMenu, CveEtiqueta)
            For Each dr As DataRow In dt.Rows
                Dim ObjM As EtiquetasModel = FillModel(dr)
                lst.Add(ObjM)
            Next

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        Finally
            dt = Nothing
        End Try

        Return lst
    End Function
    Public Function FindById(CvePlataforma As String, CveMenu As String, CveEtiqueta As Integer) As EtiquetasModel
        Dim ObjM As New EtiquetasModel
        Dim dt As DataTable
        Try
            dt = FindAll(CvePlataforma, CveMenu, CveEtiqueta)
            If Not dt Is Nothing Then ObjM = FillModel(dt.Rows(0))

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        Finally
            dt = Nothing
        End Try
        Return ObjM
    End Function
    Public Function FillModel(dr As DataRow) As EtiquetasModel
        Dim ObjModel As New EtiquetasModel
        ObjModel.CvePlataforma = dr("CvePlataforma")
        ObjModel.CveMenu = dr("CveMenu")
        ObjModel.CveEtiqueta = dr("CveEtiqueta")
        ObjModel.CveTipo = dr("CveTipo")
        ObjModel.NomTipo = dr("NomTipo")
        ObjModel.NomEtiqueta = dr("NomEtiqueta")

        Return ObjModel
    End Function
End Class
