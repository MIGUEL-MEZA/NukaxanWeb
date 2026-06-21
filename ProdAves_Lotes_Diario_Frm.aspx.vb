Imports System.IO
Imports System.Web.DynamicData
Imports Newtonsoft.Json
Imports NukaxanWEB.Libreria
Imports Saplin

Public Class ProdAves_Lotes_Diario_Frm
    Inherits Page
    Private ObjUser As UsuarioModel
    Private Plataforma As String = "54"
    Private menu As String = "21"
    Private menuC As String = "214"
    'Variables Generales
    Public defaultoption As String = ""
    Public msg As String = ""
    Private lstMensajes As List(Of MensajesModel)
    Private lstErrores As List(Of MensajesModel)
    Private lstEtiquetas As List(Of EtiquetasModel)
    Private lstAcciones As List(Of Controles_AccionesModel)
    Private lstControles As List(Of Controles_CapturaModel)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Me.ObjUser = DirectCast(Session("UsuarioLogin"), UsuarioModel)
        If Me.Page.User.Identity.IsAuthenticated = False Or ObjUser Is Nothing Then Response.Redirect("logout.aspx", True)
        lstMensajes = New Mensajes().FindlstAll("0," + menu, 1, 0)
        lstErrores = New Mensajes().FindlstAll("0," + menu, 2, 0)
        lstEtiquetas = New Etiquetas().FindlstAll("1," + Plataforma, "0," + menu, 0)
        lstAcciones = New Controles_Acciones().FindlstAll("1,2,3,4", 0)
        lstControles = New Controles_Captura().FindlstAll(CInt(Plataforma), CInt(menuC), 0)

        If Not Page.IsPostBack Then
            DatosLoad()
        End If
    End Sub
    Sub DatosLoad()
        'pnlPopup.Style.Value = "display:none;"
        regPId.Text = "0"
        filtroview.Text = ""
        gvindexpage.Text = "0"
        Dim tmpId As String = ""
        If Not Request.QueryString("Id") Is Nothing Then
            tmpId = DeCodif(Request.QueryString("Id"))
            CodCliente.Text = tmpId.Split("|")(0)
            regPId.Text = tmpId.Split("|")(1)
        End If
        If Not Request.QueryString("filtro") Is Nothing Then filtroview.Text = DeCodif(Request.QueryString("filtro"))
        If Not Request.QueryString("pageIndex") Is Nothing Then gvindexpage.Text = Request.QueryString("pageIndex")
        mpe_op.Text = ""
        LlenaMenu()
        Etiquetas()
        LlenaDDL()
        LlenaRegistro()
    End Sub
    Public Sub LlenaMenu()
        Dim uri = New Uri(Request.Url.AbsoluteUri)
        Dim filename = Path.GetFileName(uri.LocalPath)
        Dim pathURL As String = "~/menuURL?Id=" + Codif(CodCliente.Text + "|" + regPId.Text) + "&filter=" + Codif(filtroview.Text)
        Call New Menu_Formulario().LlenaMenu(MenuF, Plataforma, menu, "", filename, pathURL)

        For Each item As MenuItem In MenuF.Items
            'If regPId.Text = "" Or regPId.Text = "0" Then
            If regPId.Text = "0" And CInt(item.Value) <> 1 Then
                item.Enabled = False
                item.Text = "<span style='color:Gray'>" + item.Text + "</span>"
            End If
            'End If
        Next
    End Sub
    Sub Etiquetas()
        'General
        defaultoption = lstEtiquetas.Find(Function(p) p.CvePlataforma = 1 And p.CveMenu = 0 And p.CveEtiqueta = 1).NomEtiqueta
        Dim obligatorio As String = "<label class='control-label color-red'>*</label>"

        'Titulo
        PageTitulo.Text = "Lote" 'New Menu().FindById(ObjUser.CveRol, CInt(Plataforma), -1, menu).NomMenu
        'Menu_Titulo.Text = New Menu_Formulario().FindById(Plataforma, menu, 1).NomMenu


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
                    If Not IB Is Nothing Then
                        IB.ToolTip = a.ToolTip
                        If a.ValidaMensaje = "S" Then IB.OnClientClick = "return confirm('" + a.ValidaMensaje + "');"
                    End If
                Case 4  'LinkButton
                    Dim LB As LinkButton = TryCast(UPContenido.FindControl("LB" + a.CveTipo.ToString + a.CveAccion.ToString), LinkButton)
                    Dim IMG As System.Web.UI.WebControls.Image = TryCast(UPContenido.FindControl("IMG" + a.CveTipo.ToString + a.CveAccion.ToString), System.Web.UI.WebControls.Image)
                    Dim LBL As Label = TryCast(UPContenido.FindControl("LBL" + a.CveTipo.ToString + a.CveAccion.ToString), Label)
                    If Not LB Is Nothing Then
                        IMG.ImageUrl = "~/Content/Image/" + a.Icono
                        IMG.Width = New Unit(a.IconoSize)
                        IMG.Height = New Unit(a.IconoSize)
                        LB.ToolTip = a.ToolTip
                        LBL.Text = a.NomAccion
                        If a.ValidaClick = "S" Then LB.OnClientClick = "return confirm('" + a.ValidaMensaje + "');"
                    End If
            End Select
        Next
        'SECCIONES
        For Each a As EtiquetasModel In lstEtiquetas.Where(Function(p) p.CveTipo = "4")
            Dim LBLSEC As Label = CType(UPContenido.FindControl("SECTitulo" + a.CveEtiqueta.ToString), Label)
            If Not LBLSEC Is Nothing Then LBLSEC.Text = a.NomEtiqueta
        Next

        'CONTROLES CAPTURA
        For Each a As Controles_CapturaModel In lstControles
            Dim LBL As Label = UPContenido.FindControl("LBLC" + a.CveControl.ToString)
            If Not LBL Is Nothing Then
                LBL.Text = a.Etiqueta.Replace("*", obligatorio)
                If a.ValidaRango <> "" Then
                    Dim LBLH As Label = UPContenido.FindControl("LBLH" + a.CveControl.ToString)
                    If Not LBLH Is Nothing Then LBLH.Text = "(" + New Parametros().FindById(CInt(Plataforma), CInt(a.ValidaRango)).Valor + ")"
                End If
            End If
        Next

    End Sub
    Sub Controles(op As Boolean, Optional controles As String = "")
        Try
            Dim excepciones As String = "54-211-11,54-211-12,54-211-13,54-211-14,54-211-15"
            lstControles.FindAll(Function(p) p.CveEtapa > 0 And New ArrayList(excepciones.Split(",")).IndexOf(p.CveMSC.ToString) < 0) _
                    .ForEach(Sub(c)
                                 Dim ctrl As Control = UPContenido.FindControl(c.Control)
                                 If controles = "" Or New ArrayList(controles.Split(",")).IndexOf(c.CveControl.ToString) >= 0 Then
                                     ctrl.Visible = If(c.Editable = "S", op, Not op)
                                 Else
                                     ctrl.Visible = If(c.Editable = "S", Not op, op)
                                 End If
                             End Sub)
        Catch ex As Exception
            Alertas("", CleanSpecialCharacter(ex.Message), False, 4)
        End Try

    End Sub
    Sub ControlesAcciones(op As Integer, Optional excepciones As String = "")
        Try
            lstControles.FindAll(Function(p) p.CveEtapa > 0 And p.Editable = "S" And New ArrayList(excepciones.Split(",")).IndexOf(p.CveControl.ToString) < 0) _
                .ForEach(Sub(p)
                             Select Case p.CveTipo
                                 Case 1
                                     If op = 0 Then TryCast(UPContenido.FindControl(p.Control), Label).Text = ""
                                     If op = 1 Then p.Valor = TryCast(UPContenido.FindControl(p.Control), Label).Text
                                 Case 2
                                     If op = 0 Then TryCast(UPContenido.FindControl(p.Control), TextBox).Text = ""
                                     If op = 1 Then p.Valor = TryCast(UPContenido.FindControl(p.Control), TextBox).Text
                                 Case 3, 31, 32
                                     If op = 0 Then TryCast(UPContenido.FindControl(p.Control), DropDownList).SelectedValue = ""
                                     If op = 1 Then p.Valor = TryCast(UPContenido.FindControl(p.Control), DropDownList).SelectedValue
                                 Case 4
                                     If op = 0 Then TryCast(UPContenido.FindControl(p.Control), AjaxControlToolkit.ComboBox).SelectedValue = ""
                                     If op = 1 Then p.Valor = TryCast(UPContenido.FindControl(p.Control), AjaxControlToolkit.ComboBox).SelectedValue
                                 Case 5
                                     If op = 0 Then TryCast(UPContenido.FindControl(p.Control), Saplin.Controls.DropDownCheckBoxes).SelectedValue = ""
                                     If op = 1 Then p.Valor = TryCast(UPContenido.FindControl(p.Control), Saplin.Controls.DropDownCheckBoxes).SelectedValue
                             End Select
                         End Sub)

        Catch ex As Exception
            Alertas("", CleanSpecialCharacter(ex.Message), False, 4)
        End Try

    End Sub
    Sub LlenaDDL()


    End Sub
    'Protected Sub DDLOrigen_SelectedIndexChanged(sender As Object, e As EventArgs) Handles DDLOrigen.SelectedIndexChanged
    '    ControlesAcciones(0, "1,2,13,14")
    '    SeguridadLoad()
    'End Sub

    Sub Acciones(op As Boolean, op2 As Boolean, arrAction As String)
        Dim lb As New LinkButton
        Dim arr2() As String = arrAction.Split(",")
        Dim arr(2) As String
        arr(0) = "LB2"
        arr(1) = "LB3"
        For i = 0 To UBound(arr)
            For j = 0 To UBound(arr2)
                If i = CInt(arr2(j)) Then
                    lb = UPContenido.FindControl(arr(i))
                    lb.Visible = op
                    lb.Enabled = op2
                    lb.CssClass = If(op2 = True, "lnkbtn-action", "lnkbtn-action-disabled")
                    If op2 = True Then lb.Attributes.Add("style", "cursor: pointer;")
                    If op2 = True Then lb.Attributes.Add("style", "margin-left: 14px;")
                    If op2 = True And i = 5 Then lb.Attributes.Add("style", "margin-left: 10px;")
                End If
            Next
        Next
    End Sub
    Sub SeguridadLoad()
        Dim IsAdm As Boolean = If(New ArrayList({"1", "2"}).IndexOf(ObjUser.CveRol.ToString) >= 0, True, False)
        Dim IsEditable As Boolean = If(New ArrayList({"1"}).IndexOf(CveEstatus.Text) >= 0, True, False)
        Dim IsAutor = If(Autor.Text = ObjUser.CodUsuario, True, False)
        Acciones(False, False, "0,1")
        Controles(False)
        If regPId.Text = "0" Then 'Blanco
            Acciones(True, True, "0")
            Controles(True, "1,2")

        ElseIf regPId.Text <> "0" Then
            Acciones(True, True, "0,1")
            Controles(True)

        Else
            Acciones(True, True, "0")
            Controles(False)
        End If

        'LB3.Visible = If(CveOrigen.Text = "2", True, False)
    End Sub
    Sub LlenaRegistro()
        Try
            TBNombreTitulo.Text = New Clientes().FindById(CodCliente.Text).NomClienteR
            LlenaRPT()
            SeguridadLoad()
        Catch ex As Exception
            Alertas("", CleanSpecialCharacter(ex.Message), False, 4)
        End Try

    End Sub
    Sub LlenaRPT()
        Try
            'rptRegistros.DataSource = New ProdAves_Lotes_RegistroDiario().FindAll(CodCliente.Text, CInt(regPId.Text))
            rptRegistros.DataBind()
            rptRegistros.Items.Cast(Of RepeaterItem)().ToList.ForEach(Sub(r)
                                                                          'Dim Dependencias As String = TryCast(r.FindControl("Dependencias"), Label).Text
                                                                          ' Dim IB2 As ImageButton = TryCast(r.FindControl("IB2"), ImageButton)

                                                                          'IB2.Visible = If(Dependencias = "S", False, True)

                                                                          For Each a As Controles_AccionesModel In lstAcciones.Where(Function(p) p.CveTipo = 3)
                                                                              Dim IB As ImageButton = TryCast(r.FindControl("IB" + a.CveAccion.ToString), ImageButton)
                                                                              If Not IB Is Nothing Then
                                                                                  IB.ImageUrl = "~/Content/Image/" + a.Icono
                                                                                  'IB.Width = New Unit(a.IconoSize.Split("x")(0) + "px!important")
                                                                                  'IB.Height = New Unit(a.IconoSize.Split("x")(1) + "px!important")
                                                                                  IB.Attributes.Add("Width", a.IconoSize.Split("x")(0) + "px!important")
                                                                                  IB.Attributes.Add("Height", a.IconoSize.Split("x")(1) + "px!important")
                                                                                  IB.ToolTip = a.ToolTip
                                                                                  If a.ValidaClick = "S" Then IB.OnClientClick = "return confirm('" + a.ValidaMensaje + "');"
                                                                              End If
                                                                          Next
                                                                      End Sub)
        Catch ex As Exception
            rptRegistros.DataSource = Nothing
            rptRegistros.DataBind()
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
    Function Valida() As Boolean
        Dim IsResult As Boolean = False
        Dim lst As Controles_CapturaModel
        Try
            Dim excepciones As String = ""
            'If DDLOrigen.SelectedValue.ToString = "3" Then excepciones = "2-61-3"
            lst = lstControles.FindAll(Function(c) c.CveEtapa > 0 And c.Editable = "S" And c.Obligatorio = "S" And New ArrayList(excepciones.Split(",")).IndexOf(c.CveMSC.ToString) < 0 And c.Valor = "").FirstOrDefault
            If Not lst Is Nothing Then msg = lst.Mensaje

            If msg <> "" Then
                Alertas("", msg, False, 4)
            Else
                IsResult = True
            End If
        Catch ex As Exception
            Alertas("", CleanSpecialCharacter(ex.Message), False, 4)
        End Try

        Return IsResult
    End Function
    Function GuardaDatos() As Boolean
        Dim IsResult As Boolean = False
        Try
            'ControlesAcciones(1)
            'If Valida() = False Then Return False
            'Dim Valores As String = String.Join("|", (lstControles.FindAll(Function(p) p.CveEtapa > 0 And p.Editable = "S").Select(Function(a) a.Valor).ToList()))
            'Dim ObjM As New Clientes()
            'IsResult = ObjM.SaveModel(If(regPId.Text = "0", 0, 1), regPId.Text, Valores, ObjUser.userId)
            'mpe_op.Text = ""
            'If IsResult Then
            '    If regPId.Text = "0" Then
            '        regPId.Text = ObjM.Folio
            '        TBCodigoD.Text = ObjM.Folio
            '    End If
            '    Alertas("", "6", True, 2)
            'Else
            '    Alertas("", CleanSpecialCharacter(ObjM.strError), False, 4)
            'End If
        Catch ex As Exception
            Alertas("", CleanSpecialCharacter(ex.Message), False, 4)
        End Try
        Return IsResult
    End Function
    Sub Guardar()
        Dim IsResult As Boolean
        Try
            IsResult = GuardaDatos()
        Catch ex As Exception
            Alertas("", CleanSpecialCharacter(ex.Message), False, 4)
        End Try

    End Sub


    Sub AbrirAgregar()
        'mpe_op.Text = "action_add"
        'mpe_clean()
        'mpe_open()
    End Sub
    Sub Editar(ByVal sender As Object, ByVal e As EventArgs)
        'Dim key As String = sender.CommandArgument
        'mpe_op.Text = "action_edit"
        'mpe_clean()
        'mpe_regId.Text = key
        'mpe_open()
        'mpe_fill()
    End Sub
    Sub Eliminar(ByVal sender As Object, ByVal e As EventArgs)
        'Dim key As String = sender.CommandArgument
        'Dim IsResult As Boolean = False
        'Try
        '    Dim ObjM As New Clientes_Productos()
        '    IsResult = ObjM.DeleteModel(regPId.Text, key, "")
        '    If IsResult Then
        '        LlenaRPT()
        '        mpe_op.Text = ""
        '        Alertas("", "17", False, 2)
        '    Else
        '        Alertas("", CleanSpecialCharacter(ObjM.strError), False, 4)
        '    End If

        'Catch ex As Exception
        '    Alertas("", CleanSpecialCharacter(ex.Message), False, 4)
        'End Try
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
            Case Else
                MPEAlerta.Hide()
        End Select
    End Sub

    Public Overrides Sub VerifyRenderingInServerForm(control As Control)
        ' Verifies that the control is rendered
    End Sub
End Class