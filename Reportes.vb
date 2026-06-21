Imports System.Data
Imports NukaxanWEB
Imports NukaxanWEB.DataBase
Imports NukaxanWEB.Libreria
Public Class Reportes
    Public strError As String = ""

    Public Function GetSQLFiltros(CvePlataforma As Integer, CveMenu As Integer) As String
        Dim sb As New StringBuilder

        sb.Append(" DECLARE @CvePlataforma int=" + CvePlataforma.ToString)
        sb.Append(" DECLARE @CveMenu int=" + CveMenu.ToString)
        sb.Append(" DECLARE @Estatus int=0")
        sb.Append(" DECLARE @Mensaje varchar(250)=''")

        sb.Append(" EXEC spp_Reportes_Filtros @CvePlataforma,@CveMenu,@Estatus Output,@Mensaje Output")

        Return sb.ToString
    End Function
    Public Function GetSQLColumnas(CvePlataforma As Integer, CveMenu As Integer, id As Int64) As String
        Dim sb As New StringBuilder

        sb.Append(" DECLARE @CvePlataforma int=" + CvePlataforma.ToString)
        sb.Append(" DECLARE @CveMenu int=" + CveMenu.ToString)
        sb.Append(" DECLARE @Id bigint=" + id.ToString)
        sb.Append(" DECLARE @Estatus int=0")
        sb.Append(" DECLARE @Mensaje varchar(250)=''")

        sb.Append(" EXEC spp_Reportes_Columnas @CvePlataforma,@CveMenu,@Id,@Estatus Output,@Mensaje Output")

        Return sb.ToString
    End Function
    Public Function GetSQLDatos(CvePlataforma As Integer, CveMenu As Integer, Filtros As String) As String
        Dim sb As New StringBuilder

        sb.Append(" DECLARE @CvePlataforma int=" + CvePlataforma.ToString)
        sb.Append(" DECLARE @CveMenu int=" + CveMenu.ToString)
        sb.Append(" DECLARE @Filtros varchar(MAX) ='" + Filtros + "'")
        sb.Append(" DECLARE @Estatus int=0")
        sb.Append(" DECLARE @Mensaje varchar(250)=''")

        sb.Append(" EXEC spp_Reportes_Datos @CvePlataforma,@CveMenu,@Filtros,@Estatus Output,@Mensaje Output")

        Return sb.ToString
    End Function

    Public Function Filtros(CvePlataforma As Integer, CveMenu As Integer) As DataTable
        strError = ""
        Dim dt As DataTable
        Try
            dt = execQuery(GetSQLFiltros(CvePlataforma, CveMenu))

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        End Try

        Return dt
    End Function
    Public Function Columnas(CvePlataforma As Integer, CveMenu As Integer, Id As Int64) As DataTable
        strError = ""
        Dim dt As DataTable
        Dim sb As New StringBuilder
        Try
            dt = execQuery(GetSQLColumnas(CvePlataforma, CveMenu, Id))

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        End Try

        Return dt
    End Function
    Public Function Datos(CvePlataforma As Integer, CveMenu As Integer, Filtros As String) As DataTable
        strError = ""
        Dim dt As DataTable
        Dim sb As New StringBuilder
        Try
            dt = execQuery(GetSQLDatos(CvePlataforma, CveMenu, Filtros))

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        End Try

        Return dt
    End Function

    Public Function Filtroslst(CvePlataforma As Integer, CveMenu As Integer) As List(Of Reportes_FiltrosModel)
        Dim dt As DataTable
        Dim lst As New List(Of Reportes_FiltrosModel)
        Try
            dt = Filtros(CvePlataforma, CveMenu)
            For Each dr As DataRow In dt.Rows
                Dim ObjM As Reportes_FiltrosModel = FillFiltrosModel(dr)
                lst.Add(ObjM)
            Next

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        Finally
            dt = Nothing
        End Try

        Return lst
    End Function
    Public Function Columnaslst(CvePlataforma As Integer, CveMenu As Integer, Id As Int64) As List(Of Reportes_ColumnasModel)
        Dim dt As DataTable
        Dim lst As New List(Of Reportes_ColumnasModel)
        Try
            dt = Columnas(CvePlataforma, CveMenu, Id)
            For Each dr As DataRow In dt.Rows
                Dim ObjM As Reportes_ColumnasModel = FillColumnasModel(dr)
                lst.Add(ObjM)
            Next

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        Finally
            dt = Nothing
        End Try

        Return lst
    End Function

    Public Function FillFiltrosModel(dr As DataRow) As Reportes_FiltrosModel
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
    Public Function FillColumnasModel(dr As DataRow) As Reportes_ColumnasModel
        Dim ObjModel As New Reportes_ColumnasModel
        ObjModel.CvePlataforma = dr("CvePlataforma")
        ObjModel.CveMenu = dr("CveMenu")
        ObjModel.CveControl = dr("CveControl")
        ObjModel.Campo = dr("Campo")
        ObjModel.Titulo = dr("Titulo")
        ObjModel.Ancho = dr("Ancho")
        ObjModel.Alineado = dr("Alineado")
        ObjModel.Formato = dr("Formato")

        Return ObjModel
    End Function

End Class
