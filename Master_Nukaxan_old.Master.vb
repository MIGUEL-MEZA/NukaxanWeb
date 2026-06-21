Imports System.IO
Imports Microsoft.Ajax.Utilities
Imports NukaxanWEB
Public Class Master_Nukaxan_old
    Inherits MasterPage
    Private ObjUser As UsuarioModel
    Private Plataforma As Integer = 1
    Private lstMenu As List(Of MenuModel)
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Me.ObjUser = DirectCast(Session("UsuarioLogin"), UsuarioModel)
        'Seguridad de cache
        Response.Buffer = True
        Response.ExpiresAbsolute = DateTime.Now.AddDays(-1D)
        Response.Expires = -1500
        Response.CacheControl = "no-cache"
        Response.AddHeader("Pragma", "no-cache")
        If Me.Page.User.Identity.IsAuthenticated = False Or ObjUser Is Nothing Then Response.Redirect("logout.aspx", True)
        If Not Page.IsPostBack Then
            DatosLoad()
        End If
    End Sub
    Sub DatosLoad()
        lstMenu = New Menu().FindlstAll(ObjUser.CveRol, Plataforma, -1, "S", "")

        ' Dim lstM = lst.Where(Function(p) p.CvePlataforma = 1) _
        '.ToList().OrderBy(Function(o) o.Posicion).ToList()
        Dim lstM As List(Of MenuModel) = lstMenu.Where(Function(x) x.CvePadre = 0).ToList
        LlenaMenu(lstM, 0, Nothing)
    End Sub
    Function ObtenDatos(parentMenuId As Integer) As List(Of MenuModel)
        Dim lstM As New List(Of MenuModel)
        Try
            lstM = lstMenu.Where(Function(x) x.CvePadre = parentMenuId).ToList
        Catch ex As Exception

        End Try
        Return lstM
    End Function
    Sub LlenaMenu(lst As List(Of MenuModel), parentMenuId As Integer, parentMenuItem As MenuItem)
        Dim dt As DataTable
        Try
            Dim currentPage As String = Path.GetFileName(Request.Url.AbsolutePath)
            lst.ForEach(Sub(p)
                            Dim menuItem As New MenuItem()
                            menuItem.Value = p.CveMenu.ToString
                            menuItem.Text = p.NomMenu
                            menuItem.NavigateUrl = If(p.Url = "", "javascript:;", p.Url)
                            menuItem.ImageUrl = "~/" + p.Icono
                            menuItem.Selected = p.Url.EndsWith(currentPage, StringComparison.CurrentCultureIgnoreCase)

                            If parentMenuId = 0 Then
                                Menu.Items.Add(menuItem)
                                Dim lstH As List(Of MenuModel) = ObtenDatos(p.CveMenu)
                                LlenaMenu(lstH, Integer.Parse(menuItem.Value), menuItem)
                            Else
                                parentMenuItem.ChildItems.Add(menuItem)
                            End If

                        End Sub)

        Catch ex As Exception

        Finally
            lst = Nothing
        End Try

    End Sub

End Class