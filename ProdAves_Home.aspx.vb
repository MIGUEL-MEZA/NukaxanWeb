Public Class ProdAves_Home
    Inherits Page
    Private ObjUser As UsuarioModel
    Private Plataforma As Integer = 54
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
        LBLTitulo.Text = "Parvadas Activas"
        LBLCliente.Text = "CLIENTE:"
        'Dim Titulo As Label = DirectCast(Master.FindControl("LBLTitulo"), Label)
        'Titulo.Text = "Menu Principal" 'lstEtiquetas.Find(Function(p) p.CvePlataforma = CInt(Plataforma) And p.CveMenu = CInt(Menu) And p.CveEtiqueta = 1).NomEtiqueta

    End Sub
    Sub LlenaDDL()
        Call New Catalogos().LlenaClientes(DDLCliente, ObjUser.CodUsuario)
    End Sub
    Protected Sub DDLCliente_SelectedIndexChanged(sender As Object, e As EventArgs) Handles DDLCliente.SelectedIndexChanged
        LlenaParvadas()
    End Sub
    Sub LlenaRegistro()
        If ObjUser.TotalRelCte = 1 Then
            DDLCliente.Visible = False
            TBNomClienteD.Visible = True

            DDLCliente.SelectedIndex = 1
            TBNomClienteD.Text = DDLCliente.SelectedItem.Text
            LlenaParvadas()
        Else
            DDLCliente.Visible = True
            TBNomClienteD.Visible = False
        End If

    End Sub
    Sub LlenaParvadas()
        Try
            'Dim lstP As List(Of Nulink_ParvadasModel) = New Nulink_Parvadas().FindlstAll(DDLCliente.SelectedValue.ToString, 0)
            'DTLParvadas.DataSource = lstP
            'DTLParvadas.DataBind()
        Catch ex As Exception
            DTLParvadas.DataSource = Nothing
            DTLParvadas.DataBind()
        End Try
    End Sub
    Protected Sub DTLParvadasOnItemDataBound(ByVal sender As Object, ByVal e As DataListItemEventArgs) Handles DTLParvadas.ItemDataBound
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim item As DataListItem = e.Item
            Dim dtl As DataList = (TryCast(item.FindControl("DTLCasetas"), DataList))
            Dim CodCliente As String = (TryCast(item.FindControl("CodCliente"), Label)).Text
            Dim CveParvada As String = (TryCast(item.FindControl("CveParvada"), Label)).Text

            Try
                Dim lstC As List(Of Nulink_Parvadas_CasetasModel) = New Nulink_CatParvadas_Casetas().FindlstAll(CodCliente, CveParvada, 0)
                dtl.DataSource = lstC
                dtl.DataBind()
            Catch ex As Exception
                dtl.DataSource = Nothing
                dtl.DataBind()
            End Try

        End If
    End Sub
End Class