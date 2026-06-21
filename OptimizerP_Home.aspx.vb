Public Class OptimizerP_Home
    Inherits Page
    Private ObjUser As UsuarioModel
    Private Plataforma As Integer = 42
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Me.ObjUser = DirectCast(Session("UsuarioLogin"), UsuarioModel)
        If Me.Page.User.Identity.IsAuthenticated = False Or ObjUser Is Nothing Then Response.Redirect("logout.aspx", True)
        If Not Page.IsPostBack Then
            DatosLoad()
        End If
    End Sub
    Sub DatosLoad()
        Etiquetas()
        LlenaDDL()
        LlenaRegistro()
    End Sub
    Sub Etiquetas()
        'Titulo
        'LBLTitulo.Text = "Parvadas Activas"
        'LBLCliente.Text = "CLIENTE:"
        'Dim Titulo As Label = DirectCast(Master.FindControl("LBLTitulo"), Label)
        'Titulo.Text = "Menu Principal" 'lstEtiquetas.Find(Function(p) p.CvePlataforma = CInt(Plataforma) And p.CveMenu = CInt(Menu) And p.CveEtiqueta = 1).NomEtiqueta

    End Sub
    Sub LlenaDDL()

    End Sub

    Sub LlenaRegistro()
        'If ObjUser.TotalRelCte = 1 Then
        '    DDLCliente.Visible = False
        '    TBNomClienteD.Visible = True

        '    DDLCliente.SelectedIndex = 1
        '    TBNomClienteD.Text = DDLCliente.SelectedItem.Text

        'Else
        '    DDLCliente.Visible = True
        '    TBNomClienteD.Visible = False
        'End If

    End Sub

End Class