Imports System.IO
Imports System.Web.DynamicData
Imports Newtonsoft.Json
Imports NukaxanWEB.Libreria
Imports Saplin
Imports WebGrease.Css.Ast.Selectors

Public Class ProdAves_Postura_Lotes_Graficas_Frm
    Inherits Page
    Private ObjUser As UsuarioModel
    Private Plataforma As String = "54"
    Private menu As String = "33"
    Private menuC As String = "335"
    Private Etapa As Integer = 2
    'Variables Generales
    Public defaultoption As String = ""
    Public msg As String = ""
    'Variables de GridView
    Public gv As GridView
    Public gvsize As Integer = 50
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
        lstAcciones = New Controles_Acciones().FindlstAll("1,2,3,4", 0)
        lstFiltros = New Consultas().Filtroslst(CInt(Plataforma), CInt(menuC))

        If Not Page.IsPostBack Then
            DatosLoad()
        End If
    End Sub
    Sub DatosLoad()
        'pnlPopup.Style.Value = "display:none;"
        regPId.Text = "0"
        filtroview.Text = ""
        gvindexpage.Text = "0"
        mpe_op.Text = ""
        Dim tmpId As String = ""
        If Not Request.QueryString("Id") Is Nothing Then
            tmpId = DeCodif(Request.QueryString("Id"))
            CodCliente.Text = tmpId.Split("|")(0)
            regPId.Text = tmpId.Split("|")(1)
        End If
        If Not Request.QueryString("filtro") Is Nothing Then filtroview.Text = DeCodif(Request.QueryString("filtro"))
        If Not Request.QueryString("pageIndex") Is Nothing Then gvindexpage.Text = Request.QueryString("pageIndex")

        LlenaMenu()
        Etiquetas()
        LlenaRegistro()
        LlenaDDL()


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
        'CONTROLES FILTRO
        For Each a As Consultas_FiltrosModel In lstFiltros
            Dim LBLF As Label = UPContenido.FindControl("LBLF" + a.CveControl.ToString)
            If Not LBLF Is Nothing Then LBLF.Text = a.Etiqueta.Replace("*", obligatorio)
        Next
    End Sub
    Sub LlenaDDL()
        Call New Catalogos().LlenaReportes_Modalidad(DDLFiltroModalidad, CInt(Plataforma), CInt(menuC))
        Call New Catalogos().LlenaReportes_Indicadores(DDLFiltroIndicador, CInt(Plataforma), CInt(menuC))

        Call New Catalogos().Llena_ProdAvesLoteRecepcion(DDLFiltroCaseta, CodCliente.Text, CInt(regPId.Text))
        DDLFiltroModalidad.SelectedIndex = 1
        'LlenaRPT()
    End Sub
    Protected Sub DDLFiltroModalidad_SelectedIndexChanged(sender As Object, e As EventArgs) Handles DDLFiltroModalidad.SelectedIndexChanged
        DDLFiltroCaseta.SelectedValue = ""
        Dim IsCaseta As Boolean = If(New ArrayList({"2", "4"}).IndexOf(DDLFiltroModalidad.SelectedValue.ToString) >= 0, True, False)
        DDLFiltroCaseta.Enabled = If(IsCaseta, True, False)
    End Sub
    Sub SeguridadLoad()
        Dim IsAdm As Boolean = If(New ArrayList({"1", "2"}).IndexOf(ObjUser.CveRol.ToString) >= 0, True, False)
        Dim IsEditable As Boolean = If(New ArrayList({"1"}).IndexOf(CveEstatus.Text) >= 0, True, False)
        Dim IsAutor = If(Autor.Text = ObjUser.CodUsuario, True, False)

        Dim IsCaseta As Boolean = If(New ArrayList({"2", "4"}).IndexOf(DDLFiltroModalidad.SelectedValue.ToString) >= 0, True, False)
        DDLFiltroCaseta.Enabled = If(IsCaseta, True, False)
    End Sub
    Sub LlenaRegistro()
        Try
            Dim Obj As ProdAves_LotesModel = New ProdAves_Lotes().FindById(CodCliente.Text, CInt(regPId.Text), 0, 0)
            CveEstatus.Text = Obj.CveEstatus.ToString
            CveGranja.Text = Obj.CveGranja.ToString
            PageTitulo.Text = "LOTE: " + Obj.CodLote
            'TBNombreTitulo.Text = "LOTE: " + Obj.CodLote 'New Clientes().FindById(CodCliente.Text).NomClienteR

            SeguridadLoad()
        Catch ex As Exception
            Alertas("", CleanSpecialCharacter(ex.Message), False, 4)
        End Try

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

                                   'If c.CveTipo = 2 And (c.CveControl = 3 Or c.CveControl = 4) And c.Valor <> "" Then
                                   '    c.Valor = DateTime.ParseExact(TryCast(UPContenido.FindControl(c.Control), TextBox).Text, "dd/MM/yyyy", Nothing).ToString("yyyy-MM-dd")

                                   'End If

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

        Dim titulo As String = "INDICADOR - " + DDLFiltroIndicador.SelectedItem.Text
        Dim ejextitulo As String = "EDAD"
        Dim ejeytitulo As String = ""
        Dim xValores As String = ""
        Dim series_nombre As String = "'Lote','Estándar'"
        Dim lstLote As List(Of ProdAves_Lotes_Reportes_LoteModel)
        Dim lstCaseta As New List(Of ProdAves_Lotes_Reportes_CasetaModel)
        Dim lstDatos As New List(Of String)
        Dim lstEstandares As List(Of ProdAves_CatGuias_EstandaresModel)
        Dim EdadMin As Integer = 0
        Dim EdadMax As Integer = 0
        'Dim lstC As New List(Of CasetaModel)
        'Dim lstColores() As String = ObtenReporteSerie_Colores()



        Try
            LlenaFiltros()
            If Valida() = False Then Exit Sub
            filtros = Etapa.ToString + "|" + regPId.Text + "|" + String.Join("|", (lstFiltros.Select(Function(a) a.Valor).ToList())) + "|0"

            'lstLote = New ProdAves_Lotes_Reportes().FindlstAll_ResumenLote(CodCliente.Text, filtros)
            'xValores = String.Join(",", lstLote.Where(Function(p) p.Edad >= 18).Select(Function(p) p.Edad).Distinct.ToArray)

            'lstEstandares = New ProdAves_CatGuias_Estandares().FindlstAll(lstLote(0).CveGuia, Etapa, 0)

            'EdadMin = lstEstandares.Select(Function(p) p.Edad).Min
            'EdadMax = lstLote.Select(Function(p) p.Edad).Max





            'Select Case DDLFiltroModalidad.SelectedValue
            '    Case 1
            '        dt = New ProdAves_Lotes_Reportes().FindAll_ResumenLote(CodCliente.Text, filtros)
            '    Case 2
            '        dt = New ProdAves_Lotes_Reportes().GetDatos_ResumenCaseta(CodCliente.Text, filtros)
            '    Case 3
            '        dt = New ProdAves_Lotes_Reportes().GetDatos_ResumenHuevoLote(CodCliente.Text, filtros)
            '    Case 4
            '        dt = New ProdAves_Lotes_Reportes().GetDatos_ResumenHuevoCaseta(CodCliente.Text, filtros)
            'End Select

            'Dim lstLote As List(Of ProdAves_Lotes_Reportes_LoteModel) = New ProdAves_Lotes_Reportes().FindlstAll_ResumenLote(CodCliente.Text, filtros)
            'Dim lstE = lstLote.Select(Function(p) p.Edad).ToList
            'Dim lstM = lstLote.Select(Function(p) p.MortalidadAc).ToList
            'Dim lstM_Est = lstLote.Select(Function(p) p.Est_MortalidadAc).ToList

            ''const datos = [{x:17,y0:25,y1:50,y2:5},{x:18,y0:15,y1:45,y2:25},{x:19,y0:17,y1:35,y2:47},{x:19,y0:35,y1:55,y2:65}];

            'Dim lstDatos As New List(Of String)
            'lstLote.ForEach(Sub(p)
            '                    'Dim lstCaseta As List(Of ProdAves_Lotes_Reportes_LoteModel) = New ProdAves_Lotes_Reportes().FindlstAll_ResumenCaseta(CodCliente.Text, filtros)
            '                    lstDatos.Add("{x:" + p.Edad.ToString + ",y0:" + p.MortalidadAc.ToString("N2") + ",y1:" + p.Est_MortalidadAc.ToString("N2") + "}")
            '                End Sub)



            LScriptGrafico.Text = ChartJSLineChart("Gr�fico Indicadores", "Edad", "%Mortalidad Acum", "'Lote','Est�ndar'", String.Join(",", lstDatos.ToArray()))
            Dim m = "ss"
            'Dim x As String() = New String(lstE.Count - 1) {}
            'Dim y As Decimal() = New Decimal(lstE.Count - 1) {}
            'Dim z As Decimal() = New Decimal(lstE.Count - 1) {}

            'For i As Integer = 0 To lstE.Count - 1
            '    x(i) = lstE(i)
            '    y(i) = CDbl(lstM(i))
            '    z(i) = CDbl(lstM_Est(i))
            'Next

            'LineChart1.Series.Add(New AjaxControlToolkit.LineChartSeries() With {.Name = "Mortalidad", .Data = y})
            'LineChart1.Series.Add(New AjaxControlToolkit.LineChartSeries() With {.Name = "Mortalidad_Est", .Data = z})
            'LineChart1.CategoriesAxis = String.Join(",", x)

            'LineChart1.ChartTitle = "Pruebas"
            'LineChart1.ToolTip = "Y Value: #VALY"
            ''.Series(0).ToolTip = "Y Value: #VALY"
            'LineChart1.Visible = True

        Catch ex As Exception


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

    '--Acciones---
    Sub Regresar()
        menu = If(CveEstatus.Text = "2", "32", "31")
        Response.Redirect(New RedirectPaginas().FindById(Plataforma + "-" + menu + "-0").PaginaURL.Replace("@filtro", Codif(filtroview.Text)).Replace("@pageIndex", gvindexpage.Text), True)
    End Sub
    Sub Refrescar()
        Response.Redirect(New RedirectPaginas().FindById(Plataforma + "-" + menu + "-1").PaginaURL.Replace("@Id", Codif(regPId.Text)).Replace("@filtro", Codif(filtroview.Text)).Replace("@pageIndex", gvindexpage.Text), True)
    End Sub


    '--MODAL---
    Sub Alertas(Titulo As String, Mensaje As String, Refrescar As Boolean, Tipo As Integer)
        ModalAlert(MPEAlerta, MPEBody, BAlertOK, BAlertCancel, Titulo, If(IsNumeric(Mensaje), New Mensajes().FindById("0", 0, CInt(Mensaje)).NomMensaje, Mensaje), Refrescar, Tipo)
    End Sub
    Sub mpe_close()
        MPECaptura.Hide()
    End Sub
    Sub mpe_clean()
        mpe_regId.Text = "0"
        ''DDLEstatusE.SelectedValue = ""
        'Select Case mpe_op.Text
        '    Case "action_add", "action_edit"
        '        ControlesAcciones(0)
        '        DDLCicloE.SelectedValue = 1
        'End Select
    End Sub
    Sub mpe_open()
        MPEBody_Captura.Visible = False
        BTNP15.Visible = True
        BTNP16.Visible = False


        MPECaptura.Show()
    End Sub
    Sub mpe_action(ByVal sender As Object, ByVal e As EventArgs)
        Dim btn As Button = sender
        Dim op As String = btn.CommandArgument

        Select Case op
            Case "alert_close" : MPEAlerta.Hide()
            Case "alert_refresh"
                MPEAlerta.Hide()
                Refrescar()
            Case "action_close" : MPECaptura.Hide()

            Case Else
                MPEAlerta.Hide()
                MPECaptura.Hide()
        End Select
    End Sub

    Public Overrides Sub VerifyRenderingInServerForm(control As Control)
        ' Verifies that the control is rendered
    End Sub
End Class