Imports System.Drawing
Imports System.Globalization
Imports System.IO
Imports System.Web.DynamicData
Imports NukaxanWEB.Libreria
Imports WebGrease.Css

Public Class Nufeed_ActualizaNutrientes_Frm
    Inherits Page
    Private ObjUser As UsuarioModel
    Private Plataforma As String = "3"
    Private menu As String = "21"
    Public msg As String = ""
    'Variables de GridView
    Public gv As GridView
    Public gvsize As Integer = 30
    Public rowactual As Double = 0
    Public pageactual As Double = 0
    Public rowview As Double = 0
    Public totrecords As Double = 0

    'Variables Generales
    Private lstMensajes As List(Of MensajesModel)
    Private lstEtiquetas As List(Of EtiquetasModel)
    Private lstControles As List(Of Controles_CapturaModel)
    Private lstAcciones As List(Of Controles_AccionesModel)
    Private lstFiltros As New List(Of Consultas_FiltrosModel)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Me.ObjUser = DirectCast(Session("UsuarioLogin"), UsuarioModel)
        If Me.Page.User.Identity.IsAuthenticated = False Or ObjUser Is Nothing Then Response.Redirect("logout.aspx", True)
        lstMensajes = New Mensajes().FindlstAll("0," + menu, 0, 0)

        lstEtiquetas = New Etiquetas().FindlstAll("1," + Plataforma, "0," + menu, 0)
        lstControles = New Controles_Captura().FindlstAll(CInt(Plataforma), CInt(menu), 0)
        lstAcciones = New Controles_Acciones().FindlstAll("1,3,4", 0)
        lstFiltros = New Consultas().Filtroslst(CInt(Plataforma), CInt(menu))

        If Not Page.IsPostBack Then
            DatosLoad()
        End If
    End Sub
    Sub DatosLoad()
        pnlPopup.Style.Value = "display:none;"
        gvindexpage.Text = "0"
        If Not Request.QueryString("pageIndex") Is Nothing Then gvindexpage.Text = Request.QueryString("pageIndex")
        If regPId.Text = "" Then regPId.Text = "0"
        CveEstatus.Text = "1"
        gv1.PageSize = gvsize
        gv1.PageIndex = If(gvindexpage.Text = "", 0, CInt(gvindexpage.Text))

        LlenaDDL()
        LlenaGV(Nothing)
        DefineGV()
        Etiquetas()
        LlenaRegistro()
    End Sub
    Sub Etiquetas()
        'GENERAL
        defaultoption = lstEtiquetas.Find(Function(p) p.CvePlataforma = 1 And p.CveMenu = 0 And p.CveEtiqueta = 1).NomEtiqueta
        Dim obligatorio As String = "<label class='control-label color-red'>*</label>"

        'Titulo
        Dim lstMenu As MenuModel = New Menu().FindById(ObjUser.CveRol, Plataforma, -1, menu)
        'Dim PageTitle As Label = DirectCast(Master.FindControl("PageTitle"), Label)
        PageTitle.Text = lstMenu.NomMenu

        '--ACCIONES--
        For Each a As Controles_AccionesModel In lstAcciones
            Select Case a.CveTipo
                Case 1  'Buttons
                    'Generales
                    Dim BTN As Button = TryCast(UPContenido.FindControl("BTN" + a.CveTipo.ToString + a.CveAccion.ToString), Button)
                    If Not BTN Is Nothing Then
                        If a.IconoSize <> "" Then
                            BTN.Width = New Unit(a.IconoSize)
                            BTN.Height = New Unit(a.IconoSize)
                        End If
                        BTN.ToolTip = a.ToolTip
                        BTN.Text = a.NomAccion
                        If a.ValidaClick = "S" Then BTN.OnClientClick = "return confirm('" + a.ValidaMensaje + "');"
                    End If
                    'Paginación
                    Dim BTNP As Button = TryCast(UPContenido.FindControl("BTNP" + a.CveTipo.ToString + a.CveAccion.ToString), Button)
                    If Not BTNP Is Nothing Then
                        If a.IconoSize <> "" Then
                            BTNP.Width = New Unit(a.IconoSize)
                            BTNP.Height = New Unit(a.IconoSize)
                        End If
                        BTNP.ToolTip = a.ToolTip
                        BTNP.Text = a.NomAccion
                        If a.ValidaClick = "S" Then BTNP.OnClientClick = "return confirm('" + a.ValidaMensaje + "');"
                    End If

                Case 3  'ImageButton
                    Dim IB As ImageButton = TryCast(UPContenido.FindControl("IB" + a.CveAccion.ToString), ImageButton)
                    If Not IB Is Nothing Then
                        IB.Width = New Unit(a.IconoSize)
                        IB.Height = New Unit(a.IconoSize)
                        IB.ToolTip = a.ToolTip
                        If a.ValidaClick = "S" Then IB.OnClientClick = "return confirm('" + a.ValidaMensaje + "');"
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

        'CONTROLES FILTRO
        For Each a As Consultas_FiltrosModel In lstFiltros
            Dim LBLF As Label = UPContenido.FindControl("LBLF" + a.CveControl.ToString)
            If Not LBLF Is Nothing Then LBLF.Text = a.Etiqueta.Replace("*", obligatorio)
        Next
    End Sub
    Sub LlenaDDL()
        Call New Acceso().LlenaClientes(DDLFiltroCliente, Plataforma, ObjUser.CodUsuario, 1)
        'Call New Catalogos().LlenaPerfilCliente_CategoriaProductos(DDLFiltroCategoriaP, DDLFiltroCliente.SelectedValue.ToString)
        Call New Catalogos().LlenaPerfilCliente_Nireo_Productos(DDLFiltroProducto, DDLFiltroCliente.SelectedValue.ToString, "")
        Call New Catalogos().LlenaNireo_Origen(DDLFiltroOrigen)
        Call New Catalogos().LlenaPerfilCliente_Nireo_Proveedores(DDLFiltroProveedor, DDLFiltroCliente.SelectedValue.ToString)
    End Sub
    Protected Sub DDLFiltroCliente_SelectedIndexChanged(sender As Object, e As EventArgs) Handles DDLFiltroCliente.SelectedIndexChanged
        'Call New Catalogos().LlenaPerfilCliente_CategoriaProductos(DDLFiltroCategoriaP, DDLFiltroCliente.SelectedValue.ToString)
        Call New Catalogos().LlenaPerfilCliente_Nireo_Productos(DDLFiltroProducto, DDLFiltroCliente.SelectedValue.ToString, "")
        Call New Catalogos().LlenaPerfilCliente_Nireo_Proveedores(DDLFiltroProveedor, DDLFiltroCliente.SelectedValue.ToString)
    End Sub
    'Protected Sub DDLFiltroCategoriaP_SelectedIndexChanged(sender As Object, e As EventArgs) Handles DDLFiltroCategoriaP.SelectedIndexChanged
    '    Call New Catalogos().LlenaPerfilCliente_Nireo_Productos(DDLFiltroProducto, DDLFiltroCliente.SelectedValue.ToString, DDLFiltroCategoriaP.SelectedValue.ToString)
    'End Sub
    Sub SeguridadLoad()
        LB46.Visible = False
    End Sub
    Sub LlenaRegistro()
        If ObjUser.TotalRelCte = 1 Then
            DDLFiltroCliente.Visible = False
            TBNomCliente.Visible = True

            DDLFiltroCliente.SelectedIndex = 1
            TBNomCliente.Text = DDLFiltroCliente.SelectedItem.Text
            'Call New Catalogos().LlenaPerfilCliente_CategoriaProductos(DDLFiltroCategoriaP, DDLFiltroCliente.SelectedValue.ToString)
            Call New Catalogos().LlenaPerfilCliente_Nireo_Productos(DDLFiltroProducto, DDLFiltroCliente.SelectedValue.ToString, "")
            Call New Catalogos().LlenaPerfilCliente_Nireo_Proveedores(DDLFiltroProveedor, DDLFiltroCliente.SelectedValue.ToString)
        Else
            DDLFiltroCliente.Visible = True
            TBNomCliente.Visible = False
        End If
        TBFiltroFecIni.Text = Now.AddMonths(-1).ToString("01/MM/yyyy")
        TBFiltroFecFin.Text = Now.ToString("dd/MM/yyyy")
        'Buscar()
        LlenaGV(Nothing)
        SeguridadLoad()
    End Sub

    'DATAGRID
    Sub DefineGV()
        gv = gv1
        Dim dtCol As DataTable
        Dim wgv As Integer = 0
        Try
            dtCol = New Consultas().Columnas(CInt(Plataforma), CInt(menu), "")
            Dim c As Integer = 0
            dtCol.AsEnumerable.ToList.ForEach(Sub(r)
                                                  'gv1.HeaderRow.Cells(c).Text = r("Titulo")
                                                  gv1.Columns(c).HeaderText = r("Titulo")
                                                  c += 1
                                              End Sub)
            For i = 0 To gv.Columns.Count - 1
                If i <> 0 Then
                    gv.Columns(i).ItemStyle.HorizontalAlign = HorizontalAlign.Center
                End If

            Next
            gv.HeaderStyle.HorizontalAlign = HorizontalAlign.Center
        Catch ex As Exception

        Finally
            dtCol = Nothing
        End Try

        gv.HeaderStyle.HorizontalAlign = HorizontalAlign.Center
    End Sub
    Protected Sub GVItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gv1.RowCreated
        '  Deal with the alternating items here  
        If e.Row.RowState = DataControlRowState.Alternate Then
            e.Row.CssClass = "AlternatingRowStyle"
            e.Row.Attributes.Add("onmouseout", "this.className='AlternatingRowStyle';")
        Else
            e.Row.CssClass = "RowStyle"
            e.Row.Attributes.Add("onmouseout", "this.className='RowStyle';")
        End If
        'e.Row.Attributes.Add("onmouseover", "this.className='HighlightRowStyle';")

    End Sub
    Protected Sub GVRowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gv1.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Cells(0).Font.Bold = True
            'Dim ValorMin As String = TryCast(e.Row.FindControl("ValorMin"), Label).Text
            'Dim ValorMax As String = TryCast(e.Row.FindControl("ValorMax"), Label).Text
            'Dim TBValorUsar As TextBox = TryCast(e.Row.FindControl("TBValorUsar"), TextBox)
            'TBValorUsar.Attributes.Add("onkeyup", "javascript:alert(this.value);")
            'TBValorUsar.Attributes.Add("onkeypress", "javascript:if(this.value<" + ValorMin + " || this.value>" + ValorMax + "){Return False;};")
        End If
    End Sub
    Protected Sub GVPreRender(sender As Object, e As EventArgs) Handles gv1.PreRender
        If (gv1.ShowHeader = True And gv1.Rows.Count > 0) Or (gv1.ShowHeaderWhenEmpty = True) Then
            gv1.HeaderRow.TableSection = TableRowSection.TableHeader
        End If

    End Sub

    'LLENA DATAGRID
    Sub LlenaFiltros()
        Try
            lstFiltros.ForEach(Sub(c)
                                   If c.CveTipo = 1 Then c.Valor = TryCast(UPContenido.FindControl(c.Control), Label).Text.ToLower.Trim()
                                   If c.CveTipo = 2 Then c.Valor = TryCast(UPContenido.FindControl(c.Control), TextBox).Text
                                   If c.CveTipo = 3 Then c.Valor = TryCast(UPContenido.FindControl(c.Control), DropDownList).SelectedValue
                                   If c.CveTipo = 4 Then c.Valor = TryCast(UPContenido.FindControl(c.Control), AjaxControlToolkit.ComboBox).SelectedValue
                                   If c.CveTipo = 5 Then c.Valor = TryCast(UPContenido.FindControl(c.Control), Saplin.Controls.DropDownCheckBoxes).SelectedValue


                                   If c.CveTipo = 2 And (c.CveControl = 3 Or c.CveControl = 4) And c.Valor <> "" Then
                                       c.Valor = DateTime.ParseExact(TryCast(UPContenido.FindControl(c.Control), TextBox).Text, "dd/MM/yyyy", Nothing).ToString("yyyy-MM-dd")

                                   End If


                               End Sub)

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Validation", "alert('" + ex.Message.Replace("'", "") + "');", True)
        End Try

    End Sub
    Function Valida() As Boolean
        Dim IsResult As Boolean = False
        Dim lst As Consultas_FiltrosModel
        Try
            lst = lstFiltros.FindAll(Function(c) c.Obligatorio = "S" And c.Valor = "").FirstOrDefault
            If Not lst Is Nothing Then msg = lst.Mensaje

            If msg <> "" Then
                ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Validation", "alert('" + msg + "');", True)
                'Alertas("Campos Obligatorios", msg, BootstrapAlertType.Warning)
                'mpe_notifications(msg, 3)
            Else
                IsResult = True
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Validation", "alert('" + CleanSpecialCharacter(ex.Message) + "');", True)
        End Try

        Return IsResult
    End Function
    Async Sub Buscar()
        Dim filtros As String = ""
        Dim dt As DataTable
        Dim Isresult As Boolean = False
        Try
            LlenaFiltros()
            If Valida() = False Then Exit Sub
            filtros = String.Join("|", (lstFiltros.Select(Function(a) a.Valor).ToList()))
            'Isresult = Await Actualiza_Esperados()
            ' If Isresult = False Then Throw New System.Exception("An exception has occurred.")
            dt = New Consultas().Datos_Muestras(CInt(Plataforma), CInt(menu), filtros)
            gv1.PageIndex = 0
            Session("DatosGV") = dt
            LlenaGV(dt)
            'initDropDowns()
            LB46.Visible = True
        Catch ex As Exception
            gv1.DataSource = Nothing
            gv1.DataBind()

        Finally
            dt = Nothing
        End Try
    End Sub
    Sub initDropDowns()
        Dim tmp As String = "
           $(function () {
                    $('[id*=DDLFiltroCliente]').select2();
                    $('[id*=DDLFiltroProducto]').select2();
                    $('[id*=DDLFiltroOrigen]').select2();
                    $('[id*=DDLFiltroProveedor]').select2();
                });"
        Dim tmp2 As String = "
            $(document).ready(function () {
            $('#<%=DDLFiltroCliente.ClientID%>').select2();
            $('#<%=DDLFiltroProducto.ClientID%>').select2();
            $('#<%=DDLFiltroOrigen.ClientID%>').select2();
            $('#<%=DDLFiltroProveedor.ClientID%>').select2();
           
        });"
        ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Validation", tmp, True)
    End Sub
    Sub LlenaGV(dt As DataTable)
        Try
            If dt Is Nothing Then Throw New Exception("ND")
            gv1.DataSource = dt
            gv1.DataBind()

        Catch ex As Exception
            gv1.DataSource = Nothing
            gv1.DataBind()
            If ex.GetBaseException.Message <> "ND" Then ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Validation", "alert('" + ex.Message.Replace("'", "") + "');", True)
        Finally
            dt = Nothing
        End Try

    End Sub
    Function FiltraGV() As DataTable
        Dim dt1 As DataTable = DirectCast(Session("DatosGV"), DataTable)
        Try
            Dim dv As DataView = dt1.DefaultView
            dt1 = If(dv.Count > 0, dv.ToTable, Nothing)

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Validation", "alert('" + ex.Message.Replace("'", "") + "');", True)
        End Try
        Return dt1

    End Function

    'ACCIONES
    Async Function Actualiza_Esperados() As Threading.Tasks.Task(Of Boolean)
        Dim Obj As New Interfaz_Nufeed()
        Dim lstPerfil As New List(Of WSNufeed_PerfilMP_RMModel)
        Dim Isresult As Boolean = False
        Dim lstValores As New List(Of String)
        Try
            Dim CodALLIXCte As String = New Clientes().FindById(DDLFiltroCliente.SelectedValue).CodALLIX

            Dim ObjPro As PerfilCliente_ProductosModel = New PerfilCliente_Productos().FindById(DDLFiltroCliente.SelectedValue, DDLFiltroProducto.SelectedValue)
            Dim CodALLIXP As String = ObjPro.CodALLIX
            Dim CveTipoP As Integer = ObjPro.CveTipoP
            Dim CodCategoriaP As String = ObjPro.CodCategoriaP

            Dim ObjPara As List(Of PerfilCliente_Productos_ParametrosModel) = New PerfilCliente_Productos_Parametros().FindlstAll(DDLFiltroCliente.SelectedValue, DDLFiltroProducto.SelectedValue, "0")

            lstPerfil = Await Obj.GetPerfilMP(CodALLIXCte, CodALLIXP)
            If lstPerfil(0).analysis Is Nothing Then
                Return Isresult
                Exit Function
            End If
            lstPerfil(0).analysis.ForEach(Sub(p)
                                              Dim CodALLIXPa As String = p.code
                                              Dim valor As Double = p.value
                                              If ObjPara.Where(Function(c) c.CodALLIXPa = CodALLIXPa).Count = 0 Then Exit Sub
                                              Dim CodParametro As String = ObjPara.Find(Function(c) c.CodALLIXPa = CodALLIXPa).CodParametro
                                              Dim CveParametro As String = ObjPara.Find(Function(c) c.CodALLIXPa = CodALLIXPa).CveParametro.ToString

                                              lstValores.Add(DDLFiltroCliente.SelectedValue + "#" + DDLFiltroProducto.SelectedValue.ToString + "#" + CveParametro + "#" + CveTipoP.ToString + "#" + CodCategoriaP + "#" + CodALLIXP + "#" + CodParametro + "#" + CodALLIXPa + "#" + Math.Round(valor, 2).ToString)
                                          End Sub)
            If lstValores.Count = 0 Then
                ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Validation", "alert('" + "No se encontraron datos para actualizar." + "');", True)
                Return Isresult
                Exit Function
            End If
            Isresult = Obj.UPdatePerfilMP(String.Join("|", lstValores.ToArray().Distinct), ObjUser.userId)
            If Isresult Then
                'ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Validation", "alert('" + "Los Valores Esperados fueron actualizados." + "');", True)
            Else
                ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Validation", "alert('" + Obj.strError + "');", True)
            End If

            Return Isresult
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Validation", "alert('" + CleanSpecialCharacter(ex.Message) + "');", True)
        End Try

    End Function
    Async Sub Actualizar()
        Dim lst As New List(Of WSParametrosModel)
        Dim Isresult As Boolean = False
        Try
            Dim CodALLIXCte As String = New Clientes().FindById(DDLFiltroCliente.SelectedValue).CodALLIX
            Dim CodALLIXPro As String = New PerfilCliente_Productos().FindById(DDLFiltroCliente.SelectedValue, DDLFiltroProducto.SelectedValue).CodALLIX
            Dim valida As Boolean = True
            gv1.Rows.Cast(Of GridViewRow)().ToList.ForEach(Sub(p)
                                                               Dim CodALLIXPa As String = TryCast(p.FindControl("CodALLIXPa"), Label).Text
                                                               Dim ValorMin As String = TryCast(p.FindControl("ValorMin"), Label).Text
                                                               Dim ValorMax As String = TryCast(p.FindControl("ValorMax"), Label).Text
                                                               Dim TBValorUsar As String = TryCast(p.FindControl("TBValorUsar"), TextBox).Text

                                                               'If CDbl(TBValorUsar) < CDbl(ValorMin) Or CDbl(TBValorUsar) > CDbl(ValorMax) Then
                                                               '    valida = False
                                                               '    Exit Sub
                                                               'Else
                                                               lst.Add(New WSParametrosModel With {.nutrientcode = CodALLIXPa, .nutrientvalue = CDbl(TBValorUsar)})
                                                               'End If

                                                           End Sub)
            If valida = False Then
                ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Validation", "alert('" + lstMensajes.Find(Function(c) c.CveMenu = 0 And c.CveMensaje = 26).NomMensaje + "');", True)
                Exit Sub
            End If
            Dim Obj As New Interfaz_Nufeed()
            Isresult = Await Obj.UpdateParametros(CodALLIXCte, CodALLIXPro, lst)
            If Isresult Then
                Buscar()
                'Alertas("Interfaz NUFEED", "Los parámetros y valores indicados fueron enviados.", BootstrapAlertType.Success)
                ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Validation", "alert('" + "Los parámetros y valores indicados fueron enviados." + "');", True)
            Else
                'Alertas("Interfaz NUFEED", Obj.strError, BootstrapAlertType.Warning)
                ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Validation", "alert('" + Obj.strError + "');", True)
            End If


        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Validation", "alert('" + CleanSpecialCharacter(ex.Message) + "');", True)
        End Try

    End Sub

    'MODAL
    Sub mpe_notifications(msg As String, op As Integer)
        mpe_op.Text = "notification"
        mpe_open()
        Dim img As String = ""
        If op = 0 Then img = "message_confirm.png"
        If op = 1 Then img = "message_confirm.png"
        If op = 2 Then img = "message_info.png"
        If op = 3 Then img = "message_error.png"
        lbl_notification.Text = msg
        lblMPE_title.Text = "NOTIFICACIONES"
        img_notification.ImageUrl = "App_Design/Images/" + img
        img_notification.Width = 40
        img_notification.Height = 40
        pnlPopup.Width = 500
        pnlPopup.Height = 130
        MPEbody_Notification.Visible = True
        BTNP15.Visible = True
        If op = 0 Then
            BTNP15.CommandArgument = "action_closeR"
        Else
            BTNP15.CommandArgument = "action_close"
        End If
    End Sub
    Sub mpe_close()
        mpe_showdata.Hide()
    End Sub
    Sub mpe_clean()

    End Sub
    Sub mpe_open()
        MPEbody_Obs.Visible = False
        MPEbody_Notification.Visible = False
        BTNP15.Visible = True
        BTNP16.Visible = False

        mpe_showdata.Show()
    End Sub
    Sub mpe_action(ByVal sender As Object, ByVal e As EventArgs)
        Dim btn As Button = sender
        Dim op As String = btn.CommandArgument

        Select Case op
            Case "action_close"
                mpe_close()
            Case Else
                mpe_close()
        End Select
    End Sub
    Public Overrides Sub VerifyRenderingInServerForm(control As Control)
        ' Verifies that the control is rendered

    End Sub

End Class