Imports System.IO
Imports NukaxanWEB
Public Class Master_OptimizerP
    Inherits MasterPage
    Private ObjUser As UsuarioModel
    Private Plataforma As Integer = 42
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
        lstMenu = New Menu().FindlstAll(ObjUser.CveRol, Plataforma, -1, 0, "S", "")
        Dim lstM As List(Of MenuModel) = lstMenu.Where(Function(x) x.CvePadre = 0).OrderBy(Function(c) c.Posicion).ToList
        LlenaPlataformas()
        LlenaMenu(lstM, 0, Nothing)
        Etiquetas()
    End Sub
    Sub Etiquetas()
        'Titulo
        'LBLEslogan.Text = New Plataforma().FindById(ObjUser.CveRol, 4, 0).Descripcion
        'LBLTitulo.Text = New Plataforma().FindById(ObjUser.CveRol, 41, 4).NomPlataforma

    End Sub
    Sub LlenaPlataformas()
        Try
            Dim lstP As List(Of PlataformaModel) = New Acceso().FindlstAllPlataformas(ObjUser.CveRol, 0, 0) _
            .Where(Function(p) p.CvePlataforma <> 4).ToList()
            lstP.Find(Function(p) p.CvePlataforma = 1).ImgPlataforma = "icono-N.svg"
            lstP.Find(Function(p) p.CvePlataforma = 2).ImgPlataforma = "icono-nireo.svg"
            lstP.Find(Function(p) p.CvePlataforma = 3).ImgPlataforma = "icono-nufeed.svg"
            'lstP.Find(Function(p) p.CvePlataforma = 4).ImgPlataforma = "icono-optimizer.svg"
            lstP.Find(Function(p) p.CvePlataforma = 5).ImgPlataforma = "icono-nulink.svg"

            rptPlataformas.DataSource = lstP
            rptPlataformas.DataBind()

            rptPlataformas.Items.Cast(Of RepeaterItem)().ToList.ForEach(Sub(r)
                                                                            'Dim IB As ImageButton = r.FindControl("IBP")
                                                                            Dim HLP As HyperLink = r.FindControl("HLP")
                                                                            Dim Habilitada As String = TryCast(r.FindControl("Habilitada"), Label).Text
                                                                            HLP.Enabled = If(Habilitada = "1", True, False)

                                                                        End Sub)

        Catch ex As Exception
            rptPlataformas.DataSource = Nothing
            rptPlataformas.DataBind()
        End Try
    End Sub
    Sub LlenaMenu(lst As List(Of MenuModel), parentMenuId As Integer, parentMenuItem As MenuItem)
        Dim dt As DataTable
        Try
            Dim currentPage As String = Path.GetFileName(Request.Url.AbsolutePath)
            lst.ForEach(Sub(p)
                            Dim menuItem As New MenuItem()
                            menuItem.Value = p.CveMenu.ToString
                            menuItem.Text = "<span class='" + p.Icono + " navbar-icon'></span>" + p.NomMenu
                            menuItem.NavigateUrl = If(p.Url = "", "javascript:;", p.Url)
                            'menuItem.ImageUrl = "~/" + p.Icono
                            menuItem.Selected = p.Url.Replace("_View", "").Replace("_Frm", "").EndsWith(currentPage.Replace("_View", "").Replace("_Frm", ""), StringComparison.CurrentCultureIgnoreCase)

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
    Function ObtenDatos(parentMenuId As Integer) As List(Of MenuModel)
        Dim lstM As New List(Of MenuModel)
        Try
            lstM = lstMenu.Where(Function(x) x.CvePadre = parentMenuId).OrderBy(Function(c) c.Posicion).ToList
        Catch ex As Exception

        End Try
        Return lstM
    End Function


End Class