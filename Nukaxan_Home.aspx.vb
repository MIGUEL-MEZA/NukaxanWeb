Imports WebGrease.Css

Public Class Nukaxan_Home
    Inherits Page
    Private ObjUser As UsuarioModel
    Private Plataforma As String = "1"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Me.ObjUser = DirectCast(Session("UsuarioLogin"), UsuarioModel)
        If Me.Page.User.Identity.IsAuthenticated = False Or ObjUser Is Nothing Then Response.Redirect("logout.aspx", True)
        If Not Page.IsPostBack Then
            DatosLoad()
        End If
    End Sub
    Sub DatosLoad()
        LlenaRPT()
    End Sub
    Sub LlenaRPT()
        Try
            Dim lstP As List(Of PlataformaModel) = New Plataforma().FindlstAll(ObjUser.CveRol, 0, 0) _
            .Where(Function(p) p.CvePlataforma <> 1).ToList()

            If lstP.Count = 1 Then
                Response.Redirect(New RedirectPaginas().FindById(lstP(0).CvePlataforma.ToString + "-1").PaginaURL, True)
            End If
            lstP.ForEach(Sub(p)
                             If p.CvePlataforma = 2 Then p.ImgPlataforma = "logo-nireo.svg"
                             If p.CvePlataforma = 3 Then p.ImgPlataforma = "logo-nufeed.svg"
                             If p.CvePlataforma = 4 Then p.ImgPlataforma = "logo-optimizer.svg"
                             If p.CvePlataforma = 5 Then p.ImgPlataforma = "logo-nulink.svg"
                         End Sub)

            rptPlataformas.DataSource = lstP
            rptPlataformas.DataBind()

            'rptPlataformas.Items.Cast(Of RepeaterItem)().ToList.ForEach(Sub(r)
            '                                                                'Dim IB As ImageButton = r.FindControl("IBP")
            '                                                                Dim HLP As HyperLink = r.FindControl("HLP")
            '                                                                Dim Habilitada As String = TryCast(r.FindControl("Habilitada"), Label).Text
            '                                                                HLP.Enabled = If(Habilitada = "1", True, False)

            '                                                            End Sub)

        Catch ex As Exception
            rptPlataformas.DataSource = Nothing
            rptPlataformas.DataBind()
        End Try
    End Sub
End Class