Imports System.Data
Imports Microsoft.Ajax.Utilities
Imports NukaxanWEB
Imports NukaxanWEB.DataBase
Imports NukaxanWEB.Libreria
Public Class CatCategoriaProductos
    Public strError As String = ""
    Public Folio As String = ""
    Public Function GetSQL(CveCategoriaP As Integer) As String
        Dim sb As New StringBuilder

        sb.Append(" DECLARE @CveCategoriaP  int=" + CveCategoriaP.ToString)
        sb.Append(" DECLARE @Estatus int=0")
        sb.Append(" DECLARE @Mensaje varchar(250)=''")

        sb.Append(" EXEC spc_CatProductos_Categorias @CveCategoriaP,@Estatus Output,@Mensaje Output")

        Return sb.ToString
    End Function
    Public Function FindAll(CveCategoriaP As Integer) As DataTable
        Dim dt As DataTable
        Try
            dt = execQuery(GetSQL(CveCategoriaP))
        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        End Try

        Return dt
    End Function
    Public Function FindlstAll(CveCategoriaP As Integer) As List(Of CatCategoriaProductosModel)
        Dim dt As DataTable
        Dim lst As New List(Of CatCategoriaProductosModel)
        Try
            dt = FindAll(CveCategoriaP)
            For Each dr As DataRow In dt.Rows
                Dim ObjM As CatCategoriaProductosModel = FillModel(dr)
                lst.Add(ObjM)
            Next

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        Finally
            dt = Nothing
        End Try

        Return lst
    End Function
    Public Function FindById(CveCategoriaP As Integer) As CatCategoriaProductosModel
        Dim ObjM As New CatCategoriaProductosModel
        Dim dt As DataTable
        Try
            dt = FindAll(CveCategoriaP)
            If Not dt Is Nothing Then ObjM = FillModel(dt.Rows(0))

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        Finally
            dt = Nothing
        End Try
        Return ObjM
    End Function
    Public Function FillModel(dr As DataRow) As CatCategoriaProductosModel
        Dim ObjModel As New CatCategoriaProductosModel
        ObjModel.CveCategoriaP = dr("CveCategoriaP")
        ObjModel.CodCategoriaP = dr("CodCategoriaP")
        ObjModel.NomCategoriaP = dr("NomCategoriaP")
        ObjModel.CveTipoP = dr("CveTipoP")
        ObjModel.ImgProducto = dr("ImgProducto")
        ObjModel.NomTipoP = dr("NomTipoP")

        'Bitacora
        ObjModel.CveEstatus = dr("CveEstatus")
        ObjModel.NomEstatus = dr("NomEstatus")
        ObjModel.FecAct = CDate(dr("FecAct")).ToString("dd/MM/yyyy HH:mm")
        ObjModel.UsuAct = dr("UsuAct")
        ObjModel.NomUsuAct = dr("NomUsuAct")

        Return ObjModel
    End Function

    Public Function SaveModel(CveCategoriaP As Integer, Valores As String, UsuAct As Int64) As Boolean
        Dim sb As New StringBuilder
        Dim dt As DataTable
        Dim IsResult As Boolean = False
        Folio = "0"
        Try
            sb.Append(" DECLARE @Opcion int=0")
            sb.Append(" DECLARE @CveCategoriaP int=" + CveCategoriaP.ToString)
            sb.Append(" DECLARE @Valores varchar(MAX)='" + Valores + "'")
            sb.Append(" DECLARE @UsuAct bigint=" + UsuAct.ToString)
            sb.Append(" DECLARE @Estatus int=0")
            sb.Append(" DECLARE @Mensaje varchar(250)=''")
            sb.Append(" DECLARE @Id int=0")

            sb.Append(" EXEC spiu_CatProductos_Categorias @Opcion,@CveCategoriaP,@Valores,@UsuAct,@Estatus Output,@Mensaje Output,@Id Output")

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
    Public Function DeleteModel(CveCategoriaP As Integer) As Boolean
        Dim sb As New StringBuilder
        Dim dt As DataTable
        Dim IsResult As Boolean = False
        Folio = "0"
        Try
            sb.Append(" DECLARE @Opcion int=2")
            sb.Append(" DECLARE @CveCategoriaP int=" + CveCategoriaP.ToString)
            sb.Append(" DECLARE @Valores varchar(MAX)=''")
            sb.Append(" DECLARE @UsuAct bigint=0")
            sb.Append(" DECLARE @Estatus int=0")
            sb.Append(" DECLARE @Mensaje varchar(250)=''")
            sb.Append(" DECLARE @Id int=0")

            sb.Append(" EXEC spiu_CatProductos_Categorias @Opcion,@CveCategoriaP,@Valores,@UsuAct,@Estatus Output,@Mensaje Output,@Id Output")

            dt = execQuery(sb.ToString)
            If Not dt Is Nothing And dt.Rows.Count > 0 Then
                If dt(0)("Estatus").ToString = "0" Then
                    Folio = dt(0)("Id").ToString
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

    Public Sub LlenaCategoriaP(DDLControl As Control)
        Dim dt As DataTable

        Try
            dt = FindAll(0)
            Call subControl_fill(DDLControl, dt, "CveCategoriaP", "NomCategoriaP", True)

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        Finally
            dt = Nothing
        End Try
    End Sub

End Class
