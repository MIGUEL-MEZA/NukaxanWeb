Public Class Nulink_Home
    Inherits Page
    Private ObjUser As UsuarioModel
    Private Plataforma As Integer = 5
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Me.ObjUser = DirectCast(Session("UsuarioLogin"), UsuarioModel)
        If Me.Page.User.Identity.IsAuthenticated = False Or ObjUser Is Nothing Then Response.Redirect("logout.aspx", True)
        If Not Page.IsPostBack Then
            DatosLoad()
        End If
    End Sub
    Sub DatosLoad()
        Etiquetas()
        LlenaPlataformas()
    End Sub
    Sub Etiquetas()
        'Titulo
        'Dim Titulo As Label = DirectCast(Master.FindControl("LBLTitulo"), Label)
        'Titulo.Text = "Menu Principal" 'lstEtiquetas.Find(Function(p) p.CvePlataforma = CInt(Plataforma) And p.CveMenu = CInt(Menu) And p.CveEtiqueta = 1).NomEtiqueta

    End Sub
    Sub LlenaPlataformas()
        Try
            Dim lstP As List(Of PlataformaModel) = New Acceso().FindlstAllPlataformas(ObjUser.CveRol, 0, Plataforma)
            DTLPlataformas.DataSource = lstP
            DTLPlataformas.DataBind()

            DTLPlataformas.Items.Cast(Of DataListItem)().ToList.ForEach(Sub(r)
                                                                            Dim HLP As HyperLink = r.FindControl("HLP")
                                                                            Dim Habilitada As String = TryCast(r.FindControl("Habilitada"), Label).Text
                                                                            HLP.Enabled = If(Habilitada = "1", True, False)
                                                                            HLP.Enabled = False
                                                                            Dim cardP As HtmlGenericControl = TryCast(r.FindControl("cardP"), HtmlGenericControl)
                                                                            cardP.Attributes.Add("class", If(Habilitada = "1", "card", "card-disable"))
                                                                            cardP.Attributes.Add("class", "card-disable")
                                                                        End Sub)

        Catch ex As Exception
            DTLPlataformas.DataSource = Nothing
            DTLPlataformas.DataBind()
        End Try
    End Sub

End Class