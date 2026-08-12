Imports System.Web.DynamicData
Imports Newtonsoft.Json
Imports System.Configuration
Imports NukaxanWEB.Libreria
Imports NukaxanWEB.OptimizerC_PerfilN
Imports AjaxControlToolkit
Imports System.IO
Imports System.Xml.Schema

Public Class Nufeed_Format_Carga
    Inherits Page
    Protected WithEvents LB15 As LinkButton
    Protected WithEvents LB_IMG15 As HtmlGenericControl
    Protected WithEvents LB_LBL15 As Label
    Protected WithEvents LBLReferencia As Label
    Protected WithEvents LBLCliente As Label
    Public ObjUser As UsuarioModel
    Private Plataforma As String = "3"
    Private menu As String = "21"
    'Variables Generales
    Public defaultoption As String = ""
    Public msg As String = ""
    Private lstMensajes As List(Of MensajesModel)
    Private lstErrores As List(Of MensajesModel)
    Private lstEtiquetas As List(Of EtiquetasModel)
    Private lstAcciones As List(Of Controles_AccionesModel)
    Private lstControles As List(Of Controles_CapturaModel)
    Private iList As New List(Of DatosGrid)
    Private ClienteEditable As Boolean = False
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Me.ObjUser = DirectCast(Session("UsuarioLogin"), UsuarioModel)
        If Me.Page.User.Identity.IsAuthenticated = False Or ObjUser Is Nothing Then Response.Redirect("logout.aspx", True)
        lstMensajes = New Mensajes().FindlstAll("0," + menu, 1, 0)
        lstErrores = New Mensajes().FindlstAll("0," + menu, 2, 0)
        lstEtiquetas = New Etiquetas().FindlstAll("1," + Plataforma, "0," + menu, 0)
        lstAcciones = New Controles_Acciones().FindlstAll("1,2", 0)
        lstControles = New Controles_Captura().FindlstAll(CInt(Plataforma), CInt(menu), 0)
        ClienteEditable = New Acceso().ClienteEditable(CInt(Plataforma), ObjUser.CodUsuario, 1)

        If Not Page.IsPostBack Then
            Session("ArchivoStream") = Nothing
            Session("ArchivoNombre") = ""
            DatosLoad()
        End If


        Response.ContentEncoding = System.Text.Encoding.UTF8
        Response.Charset = "utf-8"


    End Sub

    Sub DatosLoad()
        'pnlPopup.Style.Value = "display:none;"
        regPId.Text = "0"
        filtroview.Text = ""
        gvindexpage.Text = "0"
        If Not Request.QueryString("Id") Is Nothing Then regPId.Text = DeCodif(Request.QueryString("Id"))
        If Not Request.QueryString("filtro") Is Nothing Then filtroview.Text = DeCodif(Request.QueryString("filtro"))
        If Not Request.QueryString("pageIndex") Is Nothing Then gvindexpage.Text = Request.QueryString("pageIndex")
        mpe_op.Text = ""
        Etiquetas()
        LlenaDDL()
        LlenaRegistro()
        ValidarControles()
    End Sub
    Sub Etiquetas()
        'General
        defaultoption = lstEtiquetas.Find(Function(p) p.CvePlataforma = 1 And p.CveMenu = 0 And p.CveEtiqueta = 1).NomEtiqueta
        Dim obligatorio As String = "<label class='control-label color-red'>*</label>"

        'Titulo
        Dim lstMenu As MenuModel = New Menu().FindById(ObjUser.CveRol, CInt(Plataforma), -1, menu)
        PageTitulo.Text = "Carga de Fórmulas"

        '--Acciones--
        For Each a As Controles_AccionesModel In lstAcciones
            Select Case a.CveTipo
                Case 1  'Buttons
                    Dim BTN As Button = TryCast(UPContenido.FindControl("BTN" + a.CveTipo.ToString + a.CveAccion.ToString), Button)
                    If Not BTN Is Nothing Then
                        BTN.ToolTip = a.ToolTip
                        BTN.Text = a.NomAccion
                        If a.ValidaMensaje = "S" Then BTN.OnClientClick = "return confirm('" + a.ValidaMensaje + "');"
                    End If

                    Dim BTNP As Button = TryCast(UPContenido.FindControl("BTNP" + a.CveTipo.ToString + a.CveAccion.ToString), Button)
                    If Not BTNP Is Nothing Then
                        BTNP.ToolTip = a.ToolTip
                        BTNP.Text = a.NomAccion
                        If a.ValidaMensaje = "S" Then BTNP.OnClientClick = "return confirm('" + a.ValidaMensaje + "');"
                    End If

                Case 2  'LinkButton
                    Dim LB As LinkButton = TryCast(UPContenido.FindControl("LB" + a.CveAccion.ToString), LinkButton)
                    Dim LB_IMG As HtmlGenericControl = TryCast(UPContenido.FindControl("LB_IMG" + a.CveAccion.ToString), HtmlGenericControl)
                    Dim LB_LBL As Label = TryCast(UPContenido.FindControl("LB_LBL" + a.CveAccion.ToString), Label)
                    'Dim IMGA As System.Web.UI.WebControls.Image = TryCast(UPContenido.FindControl("IMGA" + a.CveAccion.ToString), System.Web.UI.WebControls.Image)
                    If Not LB Is Nothing Then
                        LB_IMG.Attributes("class") = a.Icono
                        LB_IMG.Style("font-size") = a.IconoSize + "!important"
                        LB.ToolTip = a.ToolTip
                        LB_LBL.Text = a.NomAccion
                        If a.ValidaMensaje = "S" Then LB.OnClientClick = "return confirm('" + a.ValidaMensaje + "');"
                    End If

                Case 3  'ImageButton
                    Dim IB As ImageButton = TryCast(UPContenido.FindControl("IB" + a.CveAccion.ToString), ImageButton)
                    IB.ToolTip = a.ToolTip
                    If a.ValidaMensaje = "S" Then IB.OnClientClick = "return confirm('" + a.ValidaMensaje + "');"
            End Select
        Next

        'SECCIONES
        'For Each a As EtiquetasModel In lstEtiquetas.Where(Function(p) p.CveTipo = "4")
        '    Dim LBLSEC As Label = CType(UPContenido.FindControl("SECTitulo" + a.CveEtiqueta.ToString), Label)
        '    If Not LBLSEC Is Nothing Then LBLSEC.Text = a.NomEtiqueta
        'Next

        'CONTROLES CAPTURA
        For Each a As Controles_CapturaModel In lstControles
            If a.CveEtapa = 0 Then
                Dim LBLG As Label = UPContenido.FindControl("LBLG" + a.CveControl.ToString)
                If Not LBLG Is Nothing Then LBLG.Text = a.Etiqueta.Replace("*", obligatorio)
            Else
                Dim LBL As Label = UPContenido.FindControl("LBLC" + a.CveControl.ToString)
                If Not LBL Is Nothing Then
                    LBL.Text = a.Etiqueta.Replace("*", obligatorio)
                    If a.ValidaRango <> "" Then
                        Dim LBLH As Label = UPContenido.FindControl("LBLH" + a.CveControl.ToString)
                        If Not LBLH Is Nothing Then LBLH.Text = "(" + New Parametros().FindById(CInt(Plataforma), CInt(a.ValidaRango)).Valor + ")"
                    End If
                End If
            End If

        Next

    End Sub
    Sub Acciones(op As Boolean, op2 As Boolean, arrAction As String)
        Dim lb As New LinkButton
        Dim arr2() As String = arrAction.Split(",")
        Dim arr(1) As String
        arr(0) = "LB2"
        'arr(1) = "LB11"
        'arr(2) = "LB7"
        'arr(3) = "LB17"
        'arr(4) = "LB18"
        'arr(5) = "LB15"
        For i = 0 To UBound(arr)
            For j = 0 To UBound(arr2)
                If i = CInt(arr2(j)) Then
                    lb = UPContenido.FindControl(arr(i))
                    lb.Visible = op
                    lb.Enabled = op2
                    lb.CssClass = If(op2 = True, "lnkbtn-action", "lnkbtn-action_disabled")
                    If op2 = True Then lb.Attributes.Add("style", "cursor: pointer;")
                End If
            Next
        Next
    End Sub
    Sub SeguridadLoad()
        Dim IsAdm As Boolean = If(New ArrayList({"1", "2"}).IndexOf(ObjUser.CveRol.ToString) >= 0, True, False)
        Dim IsEstatus As Boolean = If(New ArrayList({"1"}).IndexOf(CveEstatus.Text) >= 0, True, False)
        Dim IsAutor = If(Autor.Text = ObjUser.CodUsuario, True, False)
        Acciones(False, False, "0")
        Acciones(True, True, "0")


    End Sub
    Sub LlenaDDL()
        Call New Catalogos().Nufeed_CargaFormula_Plataforma(DDLPlataforma)
        Call New Catalogos().Nufeed_CargaFormula_Clientes(DDLCliente, DDLPlataforma.SelectedValue.ToString)
        LlenaPerfil()

    End Sub
    Protected Async Sub LlenaPerfil()
        Try
            DDLPerfil.Items.Clear()
            If String.IsNullOrEmpty(DDLCliente.SelectedValue) Then
                DDLPerfil.Items.Add(New ListItem("--Seleccione una opción--", ""))
                DDLPerfil.SelectedIndex = 0
                Return
            End If
            If DDLCliente.SelectedValue.ToString = "" Then
                DDLPerfil.Items.Insert(0, New ListItem("--Seleccione una opción--", ""))
                DDLPerfil.SelectedIndex = 0
            Else
                Dim Obj As New Interfaz_Optimizer()
                Dim resultado As WSOptimizerC_ApiResultModel(Of WSOptimizerC_Formulas_ClienteDataModel) = Await Obj.ConsultaFormulasCliente(DDLCliente.SelectedValue)

                If resultado IsNot Nothing AndAlso resultado.Data IsNot Nothing Then

                    DDLPerfil.DataSource = resultado.Data.Perfiles
                    DDLPerfil.DataTextField = "Folio"
                    DDLPerfil.DataValueField = "IdPerfil"
                    DDLPerfil.DataBind()
                    If resultado.Data.Perfiles.Count = 1 Then
                        DDLPerfil.SelectedIndex = 0
                        DDLPerfil.Enabled = False
                    ElseIf resultado.Data.Perfiles.Count <> 1 Then
                        'DDLPerfil.Items.Insert(0, New ListItem("--Seleccione una opción--", ""))
                        DDLPerfil.Enabled = True
                    End If
                End If
            End If


        Catch ex As Exception
            Alertas("", CleanSpecialCharacter(ex.Message), False, 4)
        End Try
    End Sub
    Protected Sub DDLPlataforma_SelectedIndexChanged(sender As Object, e As EventArgs) Handles DDLPlataforma.SelectedIndexChanged
        Call New Catalogos().Nufeed_CargaFormula_Clientes(DDLCliente, DDLPlataforma.SelectedValue.ToString)
        LlenaPerfil()
    End Sub
    Protected Sub DDLCliente_SelectedIndexChanged(sender As Object, e As EventArgs) Handles DDLCliente.SelectedIndexChanged
        LlenaPerfil()
    End Sub
    Protected Sub DDLPerfil_SelectedIndexChanged(sender As Object, e As EventArgs) Handles DDLPerfil.SelectedIndexChanged
        'ValidarControles()
    End Sub
    Sub LlenaRegistro()
        Try
            'btnEnviar.Enabled = False
            'afuArchivo.Enabled = False
            'afuArchivo.Attributes("disabled") = "disabled"

            SeguridadLoad()

        Catch ex As Exception
            Alertas("", CleanSpecialCharacter(ex.Message), False, 4)
        End Try

    End Sub

    '--Acciones---
    Sub Regresar()
        Response.Redirect(New RedirectPaginas().FindById(Plataforma + "-" + menu + "-0").PaginaURL.Replace("@filtro", Codif(filtroview.Text)).Replace("@pageIndex", gvindexpage.Text), True)
    End Sub
    Sub Refrescar()
        Response.Redirect(New RedirectPaginas().FindById(Plataforma + "-" + menu + "-1").PaginaURL.Replace("@Id", Codif(regPId.Text)).Replace("@filtro", Codif(filtroview.Text)).Replace("@pageIndex", gvindexpage.Text), True)
    End Sub
    Private Sub ValidarControles()
        Dim habilitar As Boolean =
        Not String.IsNullOrEmpty(DDLCliente.SelectedValue) AndAlso
        Not String.IsNullOrEmpty(DDLPerfil.SelectedValue)
        pnlCarga.Visible = habilitar
        'pnlCarga.Enabled = habilitar
        'btnEnviar.Enabled = habilitar
        'afuArchivo.Enabled = habilitar
        'If habilitar Then
        '    afuArchivo.Attributes.Remove("disabled")
        'Else
        '    afuArchivo.Attributes("disabled") = "disabled"
        'End If

    End Sub
    Protected Async Sub Enviar(sender As Object, e As EventArgs)
        Try
            If String.IsNullOrEmpty(DDLPlataforma.SelectedValue) Then
                Alertas("", "Debes seleccionar una plataforma.", False, 4)
                Exit Sub
            End If
            If String.IsNullOrEmpty(DDLCliente.SelectedValue) Then
                Alertas("", "Debes seleccionar un cliente.", False, 4)
                Exit Sub
            End If
            If String.IsNullOrEmpty(DDLPerfil.SelectedValue) Then
                Alertas("", "Debes seleccionar un perfil.", False, 4)
                Exit Sub
            End If
            If FileUpload.HasFile = False Then
                Alertas("", "Debes seleccionar un archivo.", False, 4)
                Exit Sub
            End If
            Dim FileExt As String = Path.GetExtension(FileUpload.PostedFile.FileName).ToLower

            If FileExt <> ".exp" Then
                Alertas("", "La extensión del archivo es invalida", False, 4)
                Exit Sub
            End If
            Alertas("", "Si selecciono archivo", False, 4)
            Exit Sub
            Dim Obj As New Interfaz_Optimizer()

            'Dim clienteId As Int64 = Convert.ToInt64(DDLCliente.SelectedValue)
            Dim perfilId As Int64 = Convert.ToInt64(DDLPerfil.SelectedValue)

            Dim nombreArchivo As String = Session("ArchivoNombre").ToString()
            Dim usuarioActual As String = ObjUser.CodUsuario

            Using archivoStream As Stream = CType(Session("ArchivoStream"), Stream)
                Dim resultado = Await Obj.CargaFormulas(perfilId, archivoStream, nombreArchivo, usuarioActual)
                If resultado IsNot Nothing AndAlso resultado.Code = 0 Then
                    lblMensaje.Text = "Archivo cargado correctamente."
                Else
                    lblMensaje.Text = "Error: " & If(resultado?.Message, "No se pudo procesar.")
                End If
            End Using
        Catch ex As Exception
            Alertas("", CleanSpecialCharacter(ex.Message), False, 4)
        End Try
    End Sub

    '--MODAL---
    Sub Alertas(Titulo As String, Mensaje As String, Refrescar As Boolean, Tipo As Integer)
        ModalAlert(MPEAlerta, MPEBody, BAlertOK, BAlertCancel, Titulo, If(IsNumeric(Mensaje), New Mensajes().FindById("0", 0, CInt(Mensaje)).NomMensaje, Mensaje), Refrescar, Tipo)
    End Sub
    Sub mpe_action(ByVal sender As Object, ByVal e As EventArgs)
        Dim btn As Button = sender
        Dim op As String = btn.CommandArgument

        Select Case op
            Case "alert_close" : MPEAlerta.Hide()
            Case "alert_refresh"
                MPEAlerta.Hide()
                Refrescar()
                'Case "action_close" : MPECaptura.Hide()
        End Select
    End Sub
    Public Overrides Sub VerifyRenderingInServerForm(control As Control)
        ' Verifies that the control is rendered
    End Sub
End Class



