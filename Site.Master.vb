Imports NukaxanWEB
Public Class SiteMaster
    Inherits MasterPage
    Private ObjUser As UsuarioModel
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Me.ObjUser = DirectCast(Session("UsuarioLogin"), UsuarioModel)
        'Seguridad de cache
        Response.Buffer = True
        Response.ExpiresAbsolute = DateTime.Now.AddDays(-1D)
        Response.Expires = -1500
        Response.CacheControl = "no-cache"
        Response.AddHeader("Pragma", "no-cache")
        If Not Page.IsPostBack Then
            If Web.HttpContext.Current.User.Identity.IsAuthenticated = False Or ObjUser Is Nothing Then
                Response.Redirect("logout.aspx", True)
            Else
                DatosLoad()
            End If
        End If
    End Sub
    Sub DatosLoad()
        LlenaMenu()
    End Sub
    Sub LlenaMenu()
        Dim dt As DataTable
        Try
            'Dim lst As List(Of MenuModel) = New Menu().FindlstAll(ObjUser.CveRol, 0, "S", "")

            'Dim lstP = lst.Where(Function(p) p.CvePlataforma <> 1 And p.CvePadreP = 0) _
            '                            .GroupBy(Function(g) New With {Key .CvePlataforma = g.CvePlataforma, Key .NomPlataforma = g.NomPlataforma, Key .Posicion = g.PosicionP, Key .ImgPlataforma = g.ImgPlataforma}) _
            '                            .Select(Function(c) New With {c.Key.CvePlataforma, c.Key.NomPlataforma, c.Key.Posicion, c.Key.ImgPlataforma}).OrderBy(Function(p) p.Posicion).ToList()
            'rptPlataforma.DataSource = lstP
            'rptPlataforma.DataBind()

            'rptPlataforma.Items.Cast(Of RepeaterItem)().ToList.ForEach(Sub(pl)
            '                                                               Dim rpt As Repeater = TryCast(pl.FindControl("rptMenu"), Repeater)
            '                                                               Dim CvePlataforma As Integer = CInt(TryCast(pl.FindControl("CvePlataforma"), Label).Text)
            '                                                               Dim lstM = lst.Where(Function(p) p.CvePlataforma = CvePlataforma And p.CvePadre = 0) _
            '                                                               .GroupBy(Function(g) New With {Key .CveMenu = g.CveMenu, Key .NomMenu = g.NomMenu, Key .Posicion = g.Posicion, Key .Icono = g.Icono, Key .Url = g.Url}) _
            '                                                               .Select(Function(c) New With {c.Key.CveMenu, c.Key.NomMenu, c.Key.Posicion, c.Key.Icono, c.Key.Url}).OrderBy(Function(p) p.Posicion).ToList()
            '                                                               Try
            '                                                                   rpt.DataSource = lstM
            '                                                                   rpt.DataBind()
            '                                                               Catch ex As Exception
            '                                                                   rpt.DataSource = lstM
            '                                                                   rpt.DataBind()
            '                                                               End Try
            '                                                           End Sub)

        Catch ex As Exception
            rptPlataforma.DataSource = Nothing
            rptPlataforma.DataBind()
        Finally
            dt = Nothing
        End Try

    End Sub
    'Protected Sub rptPlataformaOnItemDataBound(ByVal sender As Object, ByVal e As RepeaterItemEventArgs) Handles rptPlataforma.ItemDataBound
    '    If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
    '        'Reference the Repeater Item.
    '        Try
    '            Dim rpt As Repeater = TryCast(e.Item.FindControl("rptMenu"), Repeater)
    '            Call LlenaRPT_Objetivos(rpt, CveTipo, CveMeta)

    '        Catch ex As Exception

    '        End Try

    '    End If
    'End Sub

End Class