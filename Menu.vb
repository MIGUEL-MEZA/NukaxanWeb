Imports System.Data
Imports NukaxanWEB
Imports NukaxanWEB.DataBase
Imports NukaxanWEB.Libreria
Public Class Menu
    Dim strSQLExe As String = ""
    Public strError As String = ""

    Public Function getSQL(Rol As Integer, Plataforma As Integer, CvePadre As Integer, CveMenu As Integer, Optional IsWEB As String = "", Optional IsAPP As String = "") As String
        Dim sb As New StringBuilder
        sb.Append(" DECLARE @Rol int=" + Rol.ToString)
        sb.Append(" DECLARE @CvePlataforma int=" + Plataforma.ToString)
        sb.Append(" DECLARE @CvePadre int=" + CvePadre.ToString)
        sb.Append(" DECLARE @CveMenu int=" + CveMenu.ToString)
        sb.Append(" DECLARE @IsWEB char(1)='" + IsWEB + "'")
        sb.Append(" DECLARE @IsAPP char(1)='" + IsAPP + "'")
        sb.Append(" DECLARE @Estatus int=0")
        sb.Append(" DECLARE @Mensaje varchar(250)=''")
        sb.Append(" EXEC spc_Menu @Rol,@CvePlataforma,@CvePadre,@CveMenu,@IsWEB,@IsAPP,@Estatus Output,@Mensaje Output")

        Return sb.ToString
    End Function
    Public Function FindAll(Rol As Integer, Plataforma As Integer, CvePadre As Integer, CveMenu As Integer, Optional IsWEB As String = "", Optional IsAPP As String = "") As DataTable
        strError = ""
        Dim dt As DataTable
        Try
            strSQLExe = getSQL(Rol, Plataforma, CvePadre, CveMenu, IsWEB, IsAPP)
            dt = execQuery(strSQLExe)

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        End Try

        Return dt
    End Function
    Public Function FindlstAll(Rol As Integer, Plataforma As Integer, CvePadre As Integer, CveMenu As Integer, Optional IsWEB As String = "", Optional IsAPP As String = "") As List(Of MenuModel)
        Dim dt As DataTable
        Dim lst As New List(Of MenuModel)
        Try
            dt = FindAll(Rol, Plataforma, CvePadre, CveMenu, IsWEB, IsAPP)
            For Each dr As DataRow In dt.Rows
                Dim ObjM As MenuModel = FillModel(dr)
                lst.Add(ObjM)
            Next

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        Finally
            dt = Nothing
        End Try

        Return lst
    End Function
    Public Function FindById(Rol As Integer, Plataforma As Integer, CvePadre As Integer, CveMenu As Integer, Optional IsWEB As String = "", Optional IsAPP As String = "") As MenuModel
        Dim dt As DataTable
        Dim ObjM As MenuModel
        Try
            dt = FindAll(Rol, Plataforma, CvePadre, CveMenu, IsWEB, IsAPP)
            If Not dt Is Nothing Then ObjM = FillModel(dt.Rows(0))

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        Finally
            dt = Nothing
        End Try

        Return ObjM
    End Function
    Public Function FillModel(dr As DataRow) As MenuModel
        Dim ObjModel As New MenuModel
        ObjModel.CvePlataforma = dr("CvePlataforma")
        ObjModel.CveMenu = dr("CveMenu")
        ObjModel.CvePadre = dr("CvePadre")
        ObjModel.CvePrivilegio = dr("CvePrivilegio")
        ObjModel.NomMenu = dr("NomMenu")
        ObjModel.Posicion = dr("Posicion")
        ObjModel.IsWEB = dr("IsWEB")
        ObjModel.IsAPP = dr("IsAPP")
        ObjModel.Icono = dr("Icono")
        ObjModel.Url = dr("Url")
        ObjModel.Tooltip = dr("Tooltip")

        Return ObjModel
    End Function

    'Funciones
    Public Function MenuFormularios(lstfiltros As List(Of String)) As DataTable
        strError = ""
        Dim sb As New StringBuilder
        Dim dt As DataTable
        Try
            sb.Append(" DECLARE @Opcion int =" + lstfiltros(0))
            sb.Append(" DECLARE @CveLenguaje int=" + lstfiltros(1))
            sb.Append(" DECLARE @Estatus int=0")
            sb.Append(" DECLARE @Mensaje varchar(250)=''")
            sb.Append(" EXEC spc_Config_Menus_Formularios @Opcion,@CveLenguaje,@Estatus Output,@Mensaje Output")

            strSQLExe = sb.ToString
            dt = execQuery(strSQLExe)

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        End Try

        Return dt
    End Function
    Public Sub LlenaMenuFormularios(MenuP As WebControls.Menu, Opcion As String, Lenguaje As String, fileName As String, pathURL As String)
        Dim MenuItem As MenuItem
        Dim menu_separator As String = "App_Design/Images/separador_menu.png"
        strError = ""
        Dim dt As DataTable
        Try
            dt = MenuFormularios(New List(Of String)({Opcion, Lenguaje}))
            For Each dr As DataRow In dt.Rows
                MenuItem = New MenuItem
                MenuItem.Value = dr("CveMenu").ToString
                MenuItem.Text = dr("NomMenu").ToString
                MenuItem.NavigateUrl = pathURL.Replace("menuURL", dr("Url").ToString)
                MenuItem.ToolTip = dr("Tooltip").ToString
                MenuItem.SeparatorImageUrl = If(MenuItem.Value = 1, "", menu_separator)
                If fileName = dr("Url").ToString Then
                    MenuItem.Selected = True
                    MenuItem.SeparatorImageUrl = ""
                End If
                MenuP.Items.Add(MenuItem)
            Next

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        Finally
            dt = Nothing
        End Try

    End Sub
End Class