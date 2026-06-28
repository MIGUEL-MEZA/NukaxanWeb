Imports System.Drawing
Imports System.Web.DynamicData
Imports System.Web.Services.Description
Imports NukaxanWEB.Libreria
Imports WebGrease.Css

Public Class OptimizerG_ProgramaA_View
    Inherits Page
    Private ObjUser As UsuarioModel
    Private Plataforma As String = "43"
    Private menu As String = "62"

    'Variables de GridView
    Public gv As GridView
    Public gvsize As Integer = 30
    Public rowactual As Double = 0
    Public pageactual As Double = 0
    Public rowview As Double = 0
    Public totrecords As Double = 0

    'Variables Generales
    Public defaultoption As String = ""
    Public msg As String = ""
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
        pnlCaptura.Style.Value = "display:none;"
        regPId.Text = "0"
        op.Text = ""
        filtroview.Text = ""
        gvindexpage.Text = "0"
        If Not Request.QueryString("Id") Is Nothing Then regPId.Text = DeCodif(Request.QueryString("Id"))
        If Not Request.QueryString("op") Is Nothing Then op.Text = Request.QueryString("op")
        If Not Request.QueryString("filtro") Is Nothing Then filtroview.Text = DeCodif(Request.QueryString("filtro"))
        If Not Request.QueryString("pageIndex") Is Nothing Then gvindexpage.Text = Request.QueryString("pageIndex")

        If regPId.Text = "" Then regPId.Text = "0"
        gv1.PageSize = gvsize
        gv1.PageIndex = If(gvindexpage.Text = "", 0, CInt(gvindexpage.Text))

        LlenaDDL()
        DefineGV()
        ObtenDatos()
        Etiquetas()
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

        'Filtros-------
        TBFiltroGen.Attributes.Add("placeholder", lstFiltros.Find(Function(p) p.CveControl = 0).Etiqueta)
        TBFiltroGen.Font.Italic = True
        For Each a As Consultas_FiltrosModel In lstFiltros.Where(Function(p) p.CveControl > 0)
            Dim LBL As Label = UPContenido.FindControl("LBLF" + a.CveControl.ToString)
            If Not LBL Is Nothing Then LBL.Text = a.Etiqueta
        Next

        'CONTROLES FILTRO
        For Each a As Consultas_FiltrosModel In lstFiltros
            Dim LBLF As Label = UPContenido.FindControl("LBLF" + a.CveControl.ToString)
            If Not LBLF Is Nothing Then LBLF.Text = a.Etiqueta.Replace("*", obligatorio)
        Next

        'COLUMNAS-----------------
        Dim col_title() As String = lstEtiquetas.Find(Function(p) p.CvePlataforma.ToString = Plataforma And p.CveMenu.ToString = menu And p.CveEtiqueta = 2 And p.CveTipo = 5).NomEtiqueta.Split("|")
        If col_title.Count = gv1.Columns.Count And gv1.Rows.Count > 0 Then
            For i = 0 To gv1.Columns.Count - 1
                gv1.HeaderRow.Cells(i).Text = col_title(i)
                gv1.Columns(i).HeaderText = col_title(i)
            Next
        End If

    End Sub
    Sub LlenaControles()
        Try
            lstControles.FindAll(Function(p) p.Editable = "S").ForEach(Sub(c)
                                                                           If c.CveTipo = 1 Then c.Valor = CType(UPContenido.FindControl(c.Control), Label).Text
                                                                           If c.CveTipo = 2 Then c.Valor = CType(UPContenido.FindControl(c.Control), TextBox).Text
                                                                           If c.CveTipo = 3 Then c.Valor = CType(UPContenido.FindControl(c.Control), DropDownList).SelectedValue
                                                                           If c.CveTipo = 4 Then c.Valor = CType(UPContenido.FindControl(c.Control), AjaxControlToolkit.ComboBox).SelectedValue
                                                                       End Sub)

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Validation", "alert('" + ex.Message.Replace("'", "") + "');", True)
        End Try

    End Sub
    Sub LlenaDDL()
        Call New Catalogos().LlenaGeneralClientes(DDLFiltroCliente, Plataforma, ObjUser.CodUsuario)
        Call New Catalogos().LlenaOptimizer_Modalidad(DDLFiltroModalidad)
    End Sub
    Sub SeguridadLoad()
        'sec_mensaje.Visible = If(Not IsRow, True, False)
        'gvbody.Visible = If(IsRow, True, False)

    End Sub

    'DATAGRID
    Sub DefineGV()
        gv = gv1
        For i = 0 To gv.Columns.Count - 1
            If i <> 2 Then
                gv.Columns(i).ItemStyle.HorizontalAlign = HorizontalAlign.Center
            End If

        Next
        gv.HeaderStyle.HorizontalAlign = HorizontalAlign.Center
    End Sub
    Protected Sub GVRowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gv1.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Cells(0).Font.Bold = True
            e.Row.Cells(0).ForeColor = Color.Red
            e.Row.Cells(1).Font.Bold = True
            e.Row.Cells(1).ForeColor = Color.Red
            e.Row.Cells(2).Font.Bold = True
            e.Row.Cells(2).Style.Add("cursor", "pointer")

            For Each a As Controles_AccionesModel In lstAcciones.Where(Function(p) p.CveTipo = 3)
                Dim IB As ImageButton = TryCast(e.Row.FindControl("IB" + a.CveAccion.ToString), ImageButton)
                If Not IB Is Nothing Then
                    IB.ImageUrl = "~/Content/Image/" + a.Icono
                    IB.Attributes.Add("Width", a.IconoSize.Split("x")(0) + "px!important")
                    IB.Attributes.Add("Height", a.IconoSize.Split("x")(1) + "px!important")
                    IB.ToolTip = a.ToolTip
                    If a.ValidaClick = "S" Then IB.OnClientClick = "return confirm('" + a.ValidaMensaje + "');"
                End If

            Next

        End If
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
    Sub ObtenDatos()
        'Llena el control con los datos
        Dim dt As DataTable
        Try
            gv1.PageIndex = 0
            dt = New OptimizerG_ProgramaA().FindAll(0, 0, ObjUser.CodUsuario)
            Session("DatosGV") = dt
            LlenaGV(dt)
        Catch ex As Exception
            gv1.DataSource = Nothing
            gv1.DataBind()

        Finally
            dt = Nothing
        End Try

    End Sub
    Sub LlenaFiltros()
        Try
            lstFiltros.ForEach(Sub(c)
                                   If c.CveTipo = 1 Then c.Valor = CType(UPContenido.FindControl(c.Control), Label).Text.ToLower.Trim()
                                   If c.CveTipo = 1 Then c.ValorTexto = CType(UPContenido.FindControl(c.Control), Label).Text
                                   If c.CveTipo = 2 Then c.Valor = CType(UPContenido.FindControl(c.Control), TextBox).Text
                                   If c.CveTipo = 2 Then c.ValorTexto = CType(UPContenido.FindControl(c.Control), TextBox).Text
                                   If c.CveTipo = 3 Then c.Valor = CType(UPContenido.FindControl(c.Control), DropDownList).SelectedValue
                                   If c.CveTipo = 3 Then c.ValorTexto = CType(UPContenido.FindControl(c.Control), DropDownList).SelectedItem.Text
                                   If c.CveTipo = 4 Then c.Valor = CType(UPContenido.FindControl(c.Control), AjaxControlToolkit.ComboBox).SelectedValue
                                   If c.CveTipo = 4 Then c.ValorTexto = CType(UPContenido.FindControl(c.Control), AjaxControlToolkit.ComboBox).SelectedItem.Text
                                   If c.CveTipo = 31 Then c.Valor = CType(UPContenido.FindControl(c.Control), DropDownList).SelectedValue
                                   If c.CveTipo = 31 Then c.ValorTexto = CType(UPContenido.FindControl(c.Control), DropDownList).SelectedItem.Text
                               End Sub)

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Validation", "alert('" + ex.Message.Replace("'", "") + "');", True)
        End Try

    End Sub
    Sub LlenaGV(dt As DataTable)
        Try
            If dt Is Nothing Then Throw New Exception("ND")
            gv1.DataSource = dt
            gv1.DataBind()
            gv1.Rows.Cast(Of GridViewRow)().ToList.ForEach(Sub(r)
                                                               'For Each a As Etiquetas_AccionesModel In lstAcciones.Where(Function(p) p.CveOpcion = 2)
                                                               '    Dim IB As ImageButton = r.FindControl("IB" + a.CveOpcion.ToString + a.CveAccion.ToString)
                                                               '    If Not IB Is Nothing Then IB.ToolTip = a.ToolTip
                                                               '    'If a.CveCOA = "1-2-2" Then
                                                               '    '    IB.OnClientClick = "return confirm('" + New General_Mensajes().ObtenMensaje("10", ObjUser.CveLenguaje) + "');"
                                                               '    'End If
                                                               'Next
                                                           End Sub)
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
            If ex.GetBaseException.Message <> "ND" Then ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Validation", "alert('" + ex.Message.Replace("'", "") + "');", True)
        Finally
            dt = Nothing
        End Try

    End Sub

    'ACCIONES
    Sub Exportar()
        Dim NombreArchivo As String = PageTitle.Text.Replace(" ", "_") + ".xls" 'lstEtiquetas.Find(Function(p) p.CvePlataforma.ToString = Plataforma And p.CveMenu = menu And p.CveEtiqueta = 1).NomEtiqueta + ".xls"
        Dim Titulo As String = PageTitle.Text 'lstEtiquetas.Find(Function(p) p.CvePlataforma.ToString = Plataforma And p.CveMenu = menu And p.CveEtiqueta = 1).NomEtiqueta


        Dim dtgv As New DataTable
        Try
            dtgv = FiltraGV()
            If gv1 Is Nothing Or gv1.Rows.Count = 0 Then
                'Alertas("", "50", False, 4)
                Exit Sub
            ElseIf dtgv Is Nothing Or dtgv.Rows.Count = 0 Then
                'Alertas("", "50", False, 4)
                Exit Sub
            End If
            'Filtros------------------
            Dim lstFiltroEtiquetas As New List(Of String)
            Dim lstFiltroValores As New List(Of String)
            lstFiltros.ForEach(Sub(p)
                                   If p.CveControl > 0 And p.Valor <> "" Then
                                       lstFiltroEtiquetas.Add(p.Etiqueta)
                                       lstFiltroValores.Add(p.ValorTexto)
                                   End If
                               End Sub)

            'Columnas-----------------
            Dim lstColumnas As List(Of String) = (From col In gv1.Columns.Cast(Of DataControlField) Select Left(col.ItemStyle.HorizontalAlign.ToString, 1) + "|" + col.HeaderText).ToList

            'Valores------------------
            Dim lstValores As New List(Of List(Of String))
            dtgv.AsEnumerable.ToList.ForEach(Sub(r)
                                                 Dim lst As New List(Of String)
                                                 lst.Add(r("CvePerfilN"))
                                                 lst.Add(r("Titulo"))
                                                 lst.Add(r("NomModalidad"))
                                                 lst.Add(r("NomCliente"))
                                                 lst.Add(r("NomReferencia"))
                                                 lst.Add(r("NomParametro"))
                                                 lst.Add(CDate(r("FecAct")).ToString("dd/MM/yyyy"))
                                                 lst.Add(r("NomUsuAct"))
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
                'Alertas("", "51", False, 4)
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
            'Alertas("", CleanSpecialCharacter(ex.Message), False, 4)
        Finally
            dtgv = Nothing
        End Try
    End Sub
    Sub Editar(ByVal sender As Object, ByVal e As EventArgs)
        Dim gvrow As GridViewRow = sender.NamingContainer
        Dim keyId As String = gv1.DataKeys(gvrow.RowIndex).Value.ToString()
        LlenaFiltros()
        Dim filtro As String = DDLFiltroCliente.SelectedValue + "|1|0" 'String.Join("|", (lstFiltros.Select(Function(a) a.Valor).ToList()))
        Response.Redirect(New RedirectPaginas().FindById(Plataforma + "-" + menu + "-1").PaginaURL.Replace("@Id", Codif(keyId)).Replace("@CvePN", Codif("0")).Replace("@filtro", Codif(filtro)).Replace("@pageIndex", gvindexpage.Text), True)
    End Sub
    Sub Eliminar(ByVal sender As Object, ByVal e As EventArgs)
        Dim IsResult As Boolean = False
        Dim gvrow As GridViewRow = sender.NamingContainer
        Dim keyId As String = gv1.DataKeys(gvrow.RowIndex).Value.ToString()
        Try
            Dim ObjM As New Clientes()
            IsResult = ObjM.DeleteModel(keyId)
            If IsResult Then
                ReflejarCambios()
                Alertas("", 17, False, 2)
            Else
                Alertas("", ObjM.strError, False, 4)
            End If

        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Validation", "alert('" + ex.Message + "');", True)
        End Try
    End Sub

    'FILTROS
    Sub ReflejarCambios()
        ObtenDatos()
        mpe_op.Text = ""
        mpe_clean()
        LB41.Visible = True
        LB42.Visible = False
        LB43.Visible = False
    End Sub
    Function FiltraGV() As DataTable
        Dim dt1 As DataTable = DirectCast(Session("DatosGV"), DataTable)
        Dim strSpecial() As String = {"%"}
        Dim filtro As String = ""

        Try
            Dim excepciones As String = ""
            LlenaFiltros()
            lstFiltros.FindAll(Function(p) New ArrayList(excepciones.Split(",")).IndexOf(p.CveControl.ToString) < 0).ForEach(Sub(p)
                                                                                                                                 If p.CveControl = 0 Then
                                                                                                                                     Dim tmp() As String = p.Campo.Split(",")
                                                                                                                                     filtro += "( "
                                                                                                                                     For j = 0 To UBound(tmp)
                                                                                                                                         filtro += " Convert([" + tmp(j) + "], System.String) LIKE '%" + CleanSpecialCharacter(p.Valor) + "%' OR "
                                                                                                                                     Next
                                                                                                                                     filtro = Left(filtro, Len(filtro) - 3) + " )"

                                                                                                                                 ElseIf p.Valor <> "" And New ArrayList({"3", "4"}).IndexOf(p.CveTipo.ToString) >= 0 Then
                                                                                                                                     filtro += " AND " + p.Campo + "=" + p.Valor
                                                                                                                                 ElseIf p.Valor <> "" And New ArrayList({"31", "41"}).IndexOf(p.CveTipo.ToString) >= 0 Then
                                                                                                                                     filtro += " AND " + p.Campo + "='" + p.Valor + "'"
                                                                                                                                 Else
                                                                                                                                     If p.Valor <> "" Then filtro += " AND [" + p.Campo + "] LIKE '%" + CleanSpecialCharacter(p.Valor) + "%'"
                                                                                                                                 End If
                                                                                                                             End Sub)
            Dim dv As DataView = dt1.DefaultView
            dv.RowFilter = filtro
            dt1 = If(dv.Count > 0, dv.ToTable, Nothing)
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Validation", "alert('" + ex.Message.Replace("'", "") + "');", True)
        End Try
        Return dt1

    End Function
    Sub FiltrosAbrir()
        mpe_op.Text = "filter_open"
        mpe_clean()
        mpe_open()
    End Sub
    Sub FiltrosAplicar(ByVal sender As Object, ByVal e As EventArgs)
        If mpe_op.Text = "filter_open" Then
            LB41.Visible = False
            LB42.Visible = True
            LB43.Visible = True
        End If

        gv1.PageIndex = 0
        LlenaGV(FiltraGV())
    End Sub
    Sub FiltrosModificar()
        mpe_op.Text = "filter_edit"
        mpe_open()
    End Sub
    Sub FiltrosCancelar()
        mpe_op.Text = "filter_cancel"
        mpe_clean()
        LB41.Visible = True
        LB42.Visible = False
        LB43.Visible = False

        LlenaGV(FiltraGV())
    End Sub

    'MODAL
    Sub Alertas(Titulo As String, Mensaje As String, Refrescar As Boolean, Tipo As Integer)
        ModalAlert(MPEAlerta, MPEBody, BAlertOK, BAlertCancel, Titulo, If(IsNumeric(Mensaje), New Mensajes().FindById("0", 0, CInt(Mensaje)).NomMensaje, Mensaje), Refrescar, Tipo)
    End Sub
    Sub mpe_close()
        MPECaptura.Hide()
    End Sub
    Sub mpe_clean()
        mpe_regId.Text = "0"
        TBNotasE.Text = ""

        Select Case mpe_op.Text
            Case "filter_open", "filter_edit", "filter_cancel"
                For Each f As Consultas_FiltrosModel In lstFiltros.Where(Function(p) p.CveControl > 0)
                    If f.CveTipo = 2 Then CType(UPContenido.FindControl(f.Control), TextBox).Text = ""
                    If f.CveTipo = 3 Then CType(UPContenido.FindControl(f.Control), DropDownList).SelectedValue = ""
                    If f.CveTipo = 31 Then CType(UPContenido.FindControl(f.Control), DropDownList).SelectedValue = ""
                    If f.CveTipo = 4 Then CType(UPContenido.FindControl(f.Control), AjaxControlToolkit.ComboBox).SelectedValue = ""
                Next

        End Select

    End Sub
    Sub mpe_open()
        MPEbody_Obs.Visible = False
        MPEbody_Filtros.Visible = False
        BTNP15.Visible = True
        BTNP16.Visible = False

        Select Case mpe_op.Text
            Case "filter_open", "filter_edit"
                MPEbody_Filtros.Visible = True
                lblMPE_title.Text = "Filtros"
                pnlCaptura.Width = 650
                pnlCaptura.Height = 220
                BTNP16.Visible = True
                BTNP16.Text = lstAcciones.Find(Function(p) p.CveTipo = 1 And p.CveAccion = 7).NomAccion
                BTNP16.ToolTip = lstAcciones.Find(Function(p) p.CveTipo = 1 And p.CveAccion = 7).ToolTip

        End Select
        MPECaptura.Show()
    End Sub

    Function mpe_valida() As Boolean
        Dim IsResult As Boolean = False
        Dim msg As String = ""
        Try
            Select Case mpe_op.Text
                Case "filter_open", "filter_edit"
                    LlenaFiltros()
                    Dim result = lstFiltros.Any(Function(x) x.Valor <> "")
                    If result = False Then msg = "Debes seleccionar por lo menos un filtro"
            End Select

            If msg <> "" Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Validation", "alert('" + msg + "');", True)
                MPECaptura.Show()
            Else
                IsResult = True
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Validation", "alert('" + ex.Message.Replace("'", "") + "');", True)
            MPECaptura.Show()
        End Try
        Return IsResult
    End Function
    Sub mpe_action(ByVal sender As Object, ByVal e As EventArgs)
        Dim btn As Button = sender
        Dim op As String = btn.CommandArgument

        Select Case op
            Case "alert_close" : MPEAlerta.Hide()
            Case "alert_refresh"
                MPEAlerta.Hide()
                'Refrescar()
            Case "action_close" : MPECaptura.Hide()
            Case "action_save"
                MPECaptura.Hide()
                Select Case mpe_op.Text
                    Case "filter_open", "filter_edit"
                        If mpe_valida() Then FiltrosAplicar(sender, e)
                End Select
            Case Else
                mpe_close()
        End Select
    End Sub
    Public Overrides Sub VerifyRenderingInServerForm(control As Control)
        ' Verifies that the control is rendered
    End Sub
End Class