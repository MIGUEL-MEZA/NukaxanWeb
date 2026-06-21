Imports System.Data
Imports NukaxanWEB
Imports NukaxanWEB.DataBase
Imports NukaxanWEB.Libreria
Public Class Reportes_Filtros
    Public strError As String = ""
    Public Function GetSQL(CvePlataforma As Integer, CveMenu As Integer) As String
        Dim sb As New StringBuilder

        sb.Append(" DECLARE @CvePlataforma int=" + CvePlataforma.ToString)
        sb.Append(" DECLARE @CveMenu int=" + CveMenu.ToString)
        sb.Append(" DECLARE @Estatus int=0")
        sb.Append(" DECLARE @Mensaje varchar(250)=''")

        sb.Append(" EXEC spp_Reportes_Filtros @CvePlataforma,@CveMenu,@Estatus Output,@Mensaje Output")

        Return sb.ToString
    End Function
    Public Function FindAll(CvePlataforma As Integer, CveMenu As Integer) As DataTable
        Dim dt As DataTable
        Try
            dt = execQuery(GetSQL(CvePlataforma, CveMenu))
        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        End Try

        Return dt
    End Function
    Public Function FindlstAll(CvePlataforma As Integer, CveMenu As Integer) As List(Of Reportes_FiltrosModel)
        Dim dt As DataTable
        Dim lst As New List(Of Reportes_FiltrosModel)
        Try
            dt = FindAll(CvePlataforma, CveMenu)
            For Each dr As DataRow In dt.Rows
                Dim ObjM As Reportes_FiltrosModel = FillModel(dr)
                lst.Add(ObjM)
            Next

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        Finally
            dt = Nothing
        End Try

        Return lst
    End Function
    Public Function FillModel(dr As DataRow) As Reportes_FiltrosModel
        Dim ObjModel As New Reportes_FiltrosModel
        ObjModel.CvePlataforma = dr("CvePlataforma")
        ObjModel.CveMenu = dr("CveMenu")
        ObjModel.CveControl = dr("CveControl")
        ObjModel.Control = dr("Control")
        ObjModel.CveTipo = dr("CveTipo")
        ObjModel.Campo = dr("Campo")
        ObjModel.Etiqueta = dr("Etiqueta")
        ObjModel.Obligatorio = dr("Obligatorio")
        ObjModel.Mensaje = dr("Mensaje")
        ObjModel.NomTipo = dr("NomTipo")
        ObjModel.Valor = dr("Valor")
        ObjModel.ValorTexto = dr("ValorTexto")
        Return ObjModel
    End Function


End Class
