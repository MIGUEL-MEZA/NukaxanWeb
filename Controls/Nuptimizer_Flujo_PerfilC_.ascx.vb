Public Class Nuptimizer_Flujo_PerfilC_
    Inherits System.Web.UI.UserControl
    Public Property PerfilId As Long
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Public Sub Mostrar()

        CargarDatos()

        ScriptManager.RegisterStartupScript(
        Me,
        Me.GetType(),
        "show_modal_acciones",
        "$('#mdlPerfilAcciones').modal('show');",
        True)

    End Sub

    Private Sub CargarDatos()

        Dim usuario As UsuarioModel =
        CType(Session("UsuarioLogin"),
              UsuarioModel)

        Dim objPerfil =
        New OptimizerC_PerfilN().
        FindById(PerfilId, "")

        Dim etapas =
        New OptimizerC_PerfilN_Etapas().
        FindlstAll(
            objPerfil.CodCliente,
            CInt(PerfilId))

        rptAcciones.DataSource = etapas
        rptAcciones.DataBind()

    End Sub

End Class