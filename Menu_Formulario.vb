Imports System.Data
Imports NukaxanWEB
Imports NukaxanWEB.DataBase
Imports NukaxanWEB.Libreria
Public Class Menu_Formulario
    Dim strSQLExe As String = ""
    Public strError As String = ""

    Public Function getSQL(Plataforma As Integer, CveMenuF As Integer, CveMenu As String) As String
        Dim sb As New StringBuilder
        sb.Append(" DECLARE @CvePlataforma int=" + Plataforma.ToString)
        sb.Append(" DECLARE @CveMenuF int=" + CveMenuF.ToString)
        sb.Append(" DECLARE @CveMenu varchar(MAX)='" + CveMenu + "'")
        sb.Append(" DECLARE @Estatus int=0")
        sb.Append(" DECLARE @Mensaje varchar(250)=''")
        sb.Append(" EXEC Menus_Formularios @CvePlataforma,@CveMenuF,@CveMenu,@Estatus Output,@Mensaje Output")

        Return sb.ToString
    End Function
    Public Function FindAll(Plataforma As Integer, CveMenuF As Integer, CveMenu As String) As DataTable
        strError = ""
        Dim dt As DataTable
        Try
            strSQLExe = getSQL(Plataforma, CveMenuF, CveMenu)
            dt = execQuery(strSQLExe)

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        End Try

        Return dt
    End Function
    Public Function FindlstAll(Plataforma As Integer, CveMenuF As Integer, CveMenu As String) As List(Of Menu_FormularioModel)
        Dim dt As DataTable
        Dim lst As New List(Of Menu_FormularioModel)
        Try
            dt = FindAll(Plataforma, CveMenuF, CveMenu)
            For Each dr As DataRow In dt.Rows
                Dim ObjM As Menu_FormularioModel = FillModel(dr)
                lst.Add(ObjM)
            Next

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        Finally
            dt = Nothing
        End Try

        Return lst
    End Function
    Public Function FindById(Plataforma As Integer, CveMenuF As Integer, CveMenu As String) As Menu_FormularioModel
        Dim dt As DataTable
        Dim ObjM As Menu_FormularioModel
        Try
            dt = FindAll(Plataforma, CveMenuF, CveMenu)
            If Not dt Is Nothing Then ObjM = FillModel(dt.Rows(0))

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        Finally
            dt = Nothing
        End Try

        Return ObjM
    End Function
    Public Function FillModel(dr As DataRow) As Menu_FormularioModel
        Dim ObjModel As New Menu_FormularioModel
        ObjModel.CvePlataforma = dr("CvePlataforma")
        ObjModel.CveMenuF = dr("CveMenuF")
        ObjModel.CveMenu = dr("CveMenu")
        ObjModel.Posicion = dr("Posicion")
        ObjModel.NomMenu = dr("NomMenu")
        ObjModel.Icono = dr("Icono")
        ObjModel.Url = dr("Url")
        ObjModel.Tooltip = dr("Tooltip")

        Return ObjModel
    End Function

    'Funciones
    Public Sub LlenaMenu(MenuP As WebControls.Menu, Plataforma As Integer, CveMenuF As Integer, CveMenu As String, fileName As String, pathURL As String, Optional separador As Integer = 0)
        Dim MenuItem As MenuItem
        Dim menu_separator As String = "App_Design/Images/separador_menu.png"
        strError = ""
        Dim lst As List(Of Menu_FormularioModel)
        Try
            lst = FindlstAll(Plataforma, CveMenuF, CveMenu)
            lst.ForEach(Sub(p)
                            MenuItem = New MenuItem
                            MenuItem.Value = p.CveMenu.ToString
                            MenuItem.Text = "<span class='" + p.Icono + " navbar-icon'></span>" + p.NomMenu
                            'menuItem.ImageUrl = "~/" + p.Icono
                            MenuItem.NavigateUrl = pathURL.Replace("menuURL", p.Url)
                            MenuItem.ToolTip = p.Tooltip
                            MenuItem.SeparatorImageUrl = If(separador = 0, "", If(MenuItem.Value = 1, "", menu_separator))
                            If fileName = p.Url Then
                                MenuItem.Selected = True
                                MenuItem.SeparatorImageUrl = ""
                            End If
                            MenuP.Items.Add(MenuItem)
                        End Sub)

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        Finally
            lst = Nothing
        End Try

    End Sub

End Class