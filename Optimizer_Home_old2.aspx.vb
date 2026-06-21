Imports System.IO
Imports NukaxanWEB.Libreria
Public Class Optimizer_Home_old2
    Inherits Page
    Private ObjUser As UsuarioModel
    Private Plataforma As Integer = 4
    Private menu As String = "0"
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
            rptPlataformas.DataSource = lstP
            rptPlataformas.DataBind()

            Dim uri = New Uri(Request.Url.AbsoluteUri)
            Dim filename = Path.GetFileName(uri.LocalPath)
            Dim pathURL As String = "~/menuURL?Id=" + Codif(regPId.Text) + "&filter=" + Codif(filtroview.Text)

            rptPlataformas.Items.Cast(Of RepeaterItem)().ToList.ForEach(Sub(p)
                                                                            'Dim IB As ImageButton = r.FindControl("IBP")
                                                                            Dim CvePlataforma As String = TryCast(p.FindControl("CvePlataforma"), Label).Text
                                                                            Dim CvePlataformaP As String = TryCast(p.FindControl("CvePlataformaP"), Label).Text
                                                                            Dim NomPlataforma As Label = TryCast(p.FindControl("NomPlataforma"), Label)
                                                                            Dim plataforma_activa As Boolean = False
                                                                            'Dim HLP As HyperLink = p.FindControl("HLP")
                                                                            'Dim Habilitada As String = TryCast(p.FindControl("Habilitada"), Label).Text
                                                                            'HLP.Enabled = If(Habilitada = "1", True, False)

                                                                            Dim rptMenu As Repeater = TryCast(p.FindControl("rptMenu"), Repeater)
                                                                            Try
                                                                                Dim lstM As List(Of Menu_FormularioModel) = New Menu_Formulario().FindlstAll(CInt(CvePlataforma), 0, "")
                                                                                rptMenu.DataSource = lstM
                                                                                rptMenu.DataBind()

                                                                                rptMenu.Items.Cast(Of RepeaterItem)().ToList.ForEach(Sub(r)
                                                                                                                                         Dim HLM As HyperLink = r.FindControl("HLM")
                                                                                                                                         If filename = TryCast(r.FindControl("Url"), Label).Text Then
                                                                                                                                             plataforma_activa = True
                                                                                                                                             HLM.CssClass = "menu_formulario_opciones_active"
                                                                                                                                         Else
                                                                                                                                             HLM.CssClass = "menu_formulario_opciones"
                                                                                                                                         End If
                                                                                                                                     End Sub)
                                                                                NomPlataforma.CssClass = If(plataforma_activa, "menu_formulario_plataforma_active", "menu_formulario_plataforma")
                                                                            Catch ex As Exception
                                                                                ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Validation", "alert('" + ex.Message.Replace("'", "") + "');", True)
                                                                                rptMenu.DataSource = Nothing
                                                                                rptMenu.DataBind()
                                                                            End Try
                                                                        End Sub)

            'DTLPlataformas.DataSource = lstP
            'DTLPlataformas.DataBind()

            'DTLPlataformas.Items.Cast(Of DataListItem)().ToList.ForEach(Sub(r)
            '                                                                Dim HLP As HyperLink = r.FindControl("HLP")
            '                                                                Dim Habilitada As String = TryCast(r.FindControl("Habilitada"), Label).Text
            '                                                                HLP.Enabled = If(Habilitada = "1", True, False)

            '                                                                Dim cardP As HtmlGenericControl = TryCast(r.FindControl("cardP"), HtmlGenericControl)
            '                                                                cardP.Attributes.Add("class", If(Habilitada = "1", "card", "card-disable"))
            '                                                            End Sub)

        Catch ex As Exception
            rptPlataformas.DataSource = Nothing
            rptPlataformas.DataBind()
        End Try
    End Sub

End Class