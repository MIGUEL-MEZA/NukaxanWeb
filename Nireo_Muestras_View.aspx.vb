Imports System.Drawing
Imports System.Globalization
Imports System.IO
Imports System.Web.DynamicData
Imports NukaxanWEB.Libreria
Imports WebGrease.Css

Public Class Nireo_Muestras_View
    Inherits Page
    Private ObjUser As UsuarioModel
    Private Plataforma As String = "2"
    Private menu As String = "2"
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
    Private ClienteEditable As Boolean = False
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Me.ObjUser = DirectCast(Session("UsuarioLogin"), UsuarioModel)
        If Me.Page.User.Identity.IsAuthenticated = False Or ObjUser Is Nothing Then Response.Redirect("logout.aspx", True)
        lstMensajes = New Mensajes().FindlstAll("0," + menu, 0, 0)
        lstEtiquetas = New Etiquetas().FindlstAll("1," + Plataforma, "0," + menu, 0)
        lstControles = New Controles_Captura().FindlstAll(CInt(Plataforma), CInt(menu), 0)
        lstAcciones = New Controles_Acciones().FindlstAll("1,3,4", 0)
        lstFiltros = New Consultas().Filtroslst(CInt(Plataforma), CInt(menu))
        ClienteEditable = New Acceso().ClienteEditable(Plataforma, ObjUser.CodUsuario, 1)
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
        DefineGV()
        Etiquetas()
        LlenaRegistro()
        SeguridadLoad()
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
        Call New Catalogos().LlenaPerfilCliente_CategoriaProductos(DDLFiltroCategoriaP, DDLFiltroCliente.SelectedValue.ToString)
        Call New Catalogos().LlenaPerfilCliente_Nireo_Productos(DDLFiltroProducto, DDLFiltroCliente.SelectedValue.ToString, DDLFiltroCategoriaP.SelectedValue.ToString)
        Call New Catalogos().LlenaNireo_Origen(DDLFiltroOrigen)
        Call New Catalogos().LlenaPerfilCliente_Nireo_Proveedores(DDLFiltroProveedor, DDLFiltroCliente.SelectedValue.ToString)
    End Sub
    Protected Sub DDLFiltroCliente_SelectedIndexChanged(sender As Object, e As EventArgs) Handles DDLFiltroCliente.SelectedIndexChanged
        Call New Catalogos().LlenaPerfilCliente_CategoriaProductos(DDLFiltroCategoriaP, DDLFiltroCliente.SelectedValue.ToString)
        Call New Catalogos().LlenaPerfilCliente_Nireo_Productos(DDLFiltroProducto, DDLFiltroCliente.SelectedValue.ToString, DDLFiltroCategoriaP.SelectedValue.ToString)
        Call New Catalogos().LlenaPerfilCliente_Nireo_Proveedores(DDLFiltroProveedor, DDLFiltroCliente.SelectedValue.ToString)
    End Sub
    Protected Sub DDLFiltroCategoriaP_SelectedIndexChanged(sender As Object, e As EventArgs) Handles DDLFiltroCategoriaP.SelectedIndexChanged
        Call New Catalogos().LlenaPerfilCliente_Nireo_Productos(DDLFiltroProducto, DDLFiltroCliente.SelectedValue.ToString, DDLFiltroCategoriaP.SelectedValue.ToString)
    End Sub
    Sub SeguridadLoad()
        Dim IsCteSelected As Boolean = If(DDLFiltroCliente.SelectedValue.ToString <> "", True, False)
        Dim IsRow As Boolean = If(gv1.Rows.Count > 0, True, False)

        DDLFiltroCliente.Visible = ClienteEditable
        TBNomCliente.Visible = Not ClienteEditable
    End Sub
    Sub LlenaRegistro()
        If ClienteEditable Then
            DDLFiltroCliente.SelectedValue = ""
            TBNomCliente.Text = ""
        Else
            DDLFiltroCliente.SelectedIndex = 1
            TBNomCliente.Text = DDLFiltroCliente.SelectedItem.Text
            Call New Catalogos().LlenaPerfilCliente_CategoriaProductos(DDLFiltroCategoriaP, DDLFiltroCliente.SelectedValue.ToString)
            Call New Catalogos().LlenaPerfilCliente_Nireo_Productos(DDLFiltroProducto, DDLFiltroCliente.SelectedValue.ToString, DDLFiltroCategoriaP.SelectedValue.ToString)
            Call New Catalogos().LlenaPerfilCliente_Nireo_Proveedores(DDLFiltroProveedor, DDLFiltroCliente.SelectedValue.ToString)
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
            gv1.Columns.Clear()

            'dtCol = New Consultas().Columnas(CInt(Plataforma), CInt(menu))
            'dtCol.AsEnumerable.ToList.ForEach(Sub(r)
            '                                      Dim bfield As BoundField = New BoundField()
            '                                      bfield.HeaderText = r("Titulo")
            '                                      bfield.DataField = r("Campo")
            '                                      bfield.HeaderStyle.Width = Unit.Percentage(r("Ancho"))
            '                                      bfield.ItemStyle.Width = Unit.Percentage(r("Ancho"))
            '                                      bfield.ItemStyle.HorizontalAlign = r("Alineado")
            '                                      bfield.DataFormatString = r("Formato")
            '                                      gv1.Columns.Add(bfield)
            '                                      wgv += r("Ancho")
            '                                  End Sub)
            'gv1.Width = Unit.Percentage(wgv)

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
    Protected Sub GVDataBound(ByVal sender As Object, ByVal e As EventArgs) Handles gv1.DataBound
        gv = gv1
        pageLabel.Text = If(ObjUser.CveLenguaje = 2, "Page ", "Página ") & gv.PageIndex + 1.ToString() & If(ObjUser.CveLenguaje = 2, " of " & gv.PageCount.ToString(), " de " & gv.PageCount.ToString())
    End Sub
    Protected Sub GVPaging(sender As Object, e As EventArgs)
        Dim btn = DirectCast(sender, Button)

        Select Case btn.CommandArgument
            Case "First"
                gv1.PageIndex = 0
            Case "Next"
                If gv1.PageIndex < gv1.PageCount - 1 Then
                    gv1.PageIndex = gv1.PageIndex + 1
                End If
            Case "Prev"
                If gv1.PageIndex >= 1 Then
                    gv1.PageIndex = gv1.PageIndex - 1
                End If
            Case "Last"
                gv1.PageIndex = gv1.PageCount - 1
        End Select
        LlenaGV(FiltraGV())
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
                                   If c.CveTipo = 31 Then c.Valor = TryCast(UPContenido.FindControl(c.Control), DropDownList).SelectedValue
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
    Sub Buscar()
        Dim filtros As String = ""
        Dim dt As DataTable
        Try
            LlenaFiltros()
            If Valida() = False Then Exit Sub
            filtros = String.Join("|", (lstFiltros.Select(Function(a) a.Valor).ToList()))
            dt = New Consultas().Datos_Muestras(CInt(Plataforma), CInt(menu), filtros)
            gv1.PageIndex = 0
            Session("DatosGV") = dt
            LlenaGV(dt)
            'initDropDowns()
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
            If dt.Rows.Count > 0 Then
                rowactual = gv1.Rows.Count
                pageactual = gv1.PageIndex + 1
                totrecords = dt.Rows.Count
                tblpaging.Visible = True

            Else
                tblpaging.Visible = False
            End If
            If totrecords <= gvsize Then
                rowview = rowactual
            Else
                rowview = ((pageactual - 1) * gvsize) + rowactual
            End If
            totreg.Text = "Registros " + rowview.ToString + " de " + dt.Rows.Count.ToString + "&nbsp;&nbsp;"


            If totrecords <= gvsize Then
                BTNP11.Visible = False
                BTNP12.Visible = False
                BTNP13.Visible = False
                BTNP14.Visible = False
            Else
                If gv1.PageIndex = 0 Then
                    BTNP11.Visible = False
                    BTNP12.Visible = False
                Else
                    BTNP11.Visible = True
                    BTNP12.Visible = True
                End If
                If gv1.PageIndex = (gv1.PageCount - 1) Then
                    BTNP13.Visible = False
                    BTNP14.Visible = False
                Else
                    BTNP13.Visible = True
                    BTNP14.Visible = True
                End If

            End If

        Catch ex As Exception
            gv1.DataSource = Nothing
            gv1.DataBind()
            tblpaging.Visible = False
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
    Sub Exportar()
        Dim NombreArchivo As String = lstEtiquetas.Find(Function(p) p.CvePlataforma.ToString = Plataforma And p.CveMenu = menu And p.CveEtiqueta = 1).NomEtiqueta + ".xls"
        Dim Titulo As String = lstEtiquetas.Find(Function(p) p.CvePlataforma.ToString = Plataforma And p.CveMenu = menu And p.CveEtiqueta = 1).NomEtiqueta

        Dim dtgv As New DataTable
        Try
            dtgv = FiltraGV()
            If gv1 Is Nothing Or gv1.Rows.Count = 0 Then
                mpe_notifications("No hay registros para exportar", 3)
                Exit Sub
            ElseIf dtgv Is Nothing Or dtgv.Rows.Count = 0 Then
                mpe_notifications("No hay registros para exportar", 3)
                Exit Sub
            End If
            'Filtros------------------
            Dim lstFiltroEtiquetas As New List(Of String)
            Dim lstFiltroValores As New List(Of String)
            'LlenaFiltros()
            'lstFiltros.ForEach(Sub(p)
            '                       If p.CveControl > 0 And p.Valor <> "" Then
            '                           lstFiltroEtiquetas.Add(p.Etiqueta)
            '                           lstFiltroValores.Add(p.ValorTexto)
            '                       End If
            '                   End Sub)

            'Columnas-----------------
            Dim lstColumnas As List(Of String) = (From col In gv1.Columns.Cast(Of DataControlField) Select Left(col.ItemStyle.HorizontalAlign.ToString, 1) + "|" + col.HeaderText).ToList

            'Valores------------------
            Dim lstValores As New List(Of List(Of String))
            Dim dtCol As DataTable
            dtCol = New Consultas().Columnas(CInt(Plataforma), CInt(menu), "")
            dtgv.AsEnumerable.ToList.ForEach(Sub(r)
                                                 Dim lst As New List(Of String)
                                                 dtCol.AsEnumerable.ToList.ForEach(Sub(p)
                                                                                       If New ArrayList({"0"}).IndexOf(p("CveControl").ToString) >= 0 Then
                                                                                           lst.Add(String.Format("&nbsp;{0}", r(p("Campo"))))

                                                                                       ElseIf New ArrayList({"1"}).IndexOf(p("CveControl").ToString) >= 0 Then
                                                                                           If IsDBNull(r(p("Campo"))) Then
                                                                                               lst.Add("")
                                                                                           Else
                                                                                               lst.Add(CDate(r(p("Campo"))).ToString("dd/MM/yyyy"))
                                                                                           End If
                                                                                       Else
                                                                                           lst.Add(r(p("Campo")))
                                                                                       End If
                                                                                   End Sub)
                                                 lstValores.Add(lst)
                                             End Sub)

            Dim ObjE As New General_ExcelModel With {
            .Titulo = Titulo,
            .lstFiltroEtiquetas = lstFiltroEtiquetas,
            .lstFiltroValores = lstFiltroValores,
            .lstColumnas = lstColumnas,
            .lstValores = lstValores,
            .RowEncabezado = New List(Of String)({"0"}),
            .ColEmpiezaCero = New List(Of String)({"0"}),
            .ColTotales = New List(Of String)({"0"}),
            .Totales = New List(Of String)({"0"})
            }

            Dim sbExcel As StringBuilder = New General_Excel().GenerarArchivo(ObjE)

            If sbExcel.ToString = "" Then
                mpe_notifications("Error: Nose pudo generar el archivo", 3)
                Exit Sub
            End If

            Response.Clear()
            Response.Charset = ""
            Response.ContentType = "application/vnd.ms-excel"
            Response.AddHeader("Content-disposition", "attachment; filename=" + NombreArchivo)
            Response.Charset = "UTF-8"
            Response.ContentEncoding = Encoding.Default
            Dim style As String = "<style>.textmode{mso-number-format:\@;}</style>"
            Response.Write(style)
            Response.Write(Server.HtmlDecode(sbExcel.ToString))
            Response.End()

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Validation", "alert('" + ex.Message.Replace("'", "") + "');", True)
        Finally
            dtgv = Nothing
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