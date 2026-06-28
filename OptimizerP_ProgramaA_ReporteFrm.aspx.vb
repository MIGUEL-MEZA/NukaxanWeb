Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Runtime.Remoting
Imports System.Web.DynamicData
Imports Microsoft.Ajax.Utilities
Imports Newtonsoft.Json
Imports NukaxanWEB.Libreria
Imports NukaxanWEB.OptimizerP_PerfilN

Public Class OptimizerP_ProgramaA_ReporteFrm
    Inherits Page
    Private ObjUser As UsuarioModel
    Private Plataforma As String = "42"
    Private menu As String = "62"
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
            DatosLoad()
        End If
    End Sub
    Sub DatosLoad()
        regPId.Text = "0"
        filtroview.Text = ""
        gvindexpage.Text = "0"
        CvePerfilN.Text = "0"
        If Not Request.QueryString("Id") Is Nothing Then regPId.Text = DeCodif(Request.QueryString("Id"))
        If Not Request.QueryString("filtro") Is Nothing Then filtroview.Text = DeCodif(Request.QueryString("filtro"))
        If Not Request.QueryString("pageIndex") Is Nothing Then gvindexpage.Text = Request.QueryString("pageIndex")
        If Not Request.QueryString("CvePN") Is Nothing Then CvePerfilN.Text = DeCodif(Request.QueryString("CvePN"))
        mpe_op.Text = ""
        Etiquetas()

        LlenaRegistro()
        gv.PageSize = 40
    End Sub
    Sub Etiquetas()
        'General
        defaultoption = lstEtiquetas.Find(Function(p) p.CvePlataforma = 1 And p.CveMenu = 0 And p.CveEtiqueta = 1).NomEtiqueta
        Dim obligatorio As String = "<label class='control-label color-red'>*</label>"

        'Titulo
        Dim lstMenu As MenuModel = New Menu().FindById(ObjUser.CveRol, CInt(Plataforma), -1, menu)
        'PageTitulo.Text = lstMenu.NomMenu

        'ACCIONES

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
        For Each a As EtiquetasModel In lstEtiquetas.Where(Function(p) p.CveTipo = "4")
            Dim LBLSEC As Label = CType(UPContenido.FindControl("SECTitulo" + a.CveEtiqueta.ToString), Label)
            If Not LBLSEC Is Nothing Then LBLSEC.Text = a.NomEtiqueta
        Next

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
    Sub Controles(op As Boolean)
        Try
            Dim excepciones As String = "42-3-1,42-3-2,42-3-3,42-3-20"
            lstControles.FindAll(Function(p) p.CveEtapa > 0 And New ArrayList(excepciones.Split(",")).IndexOf(p.CveMSC.ToString) < 0) _
                    .ForEach(Sub(c)
                                 Dim ctrl As Control = UPContenido.FindControl(c.Control)
                                 Dim ctrl2 As HtmlControl = UPContenido.FindControl(c.Control + "U")
                                 ctrl.Visible = If(c.Editable = "S", op, Not op)

                                 If Not IsNothing(ctrl2) Then
                                     ctrl2.Attributes("Class") = If(c.Editable = "S" And op, "tb-unidad", "control-label")
                                     ctrl2.Attributes("style") = If(c.Editable = "S" And op, "", "margin-left:5px;")
                                 End If
                             End Sub)
        Catch ex As Exception
            Alertas("", CleanSpecialCharacter(ex.Message), False, 4)
        End Try

    End Sub
    Sub LlenaControles()
        Try
            lstControles.FindAll(Function(p) p.CveEtapa > 0 And p.Editable = "S" And p.Obligatorio = "S").ForEach(Sub(c)
                                                                                                                      If c.CveTipo = 1 Then c.Valor = CType(UPContenido.FindControl(c.Control), Label).Text
                                                                                                                      If c.CveTipo = 2 Then c.Valor = CType(UPContenido.FindControl(c.Control), TextBox).Text
                                                                                                                      If c.CveTipo = 3 Then c.Valor = CType(UPContenido.FindControl(c.Control), DropDownList).SelectedValue
                                                                                                                      If c.CveTipo = 4 Then c.Valor = CType(UPContenido.FindControl(c.Control), AjaxControlToolkit.ComboBox).SelectedValue
                                                                                                                  End Sub)

        Catch ex As Exception
            Alertas("", CleanSpecialCharacter(ex.Message), False, 4)
        End Try

    End Sub

    Sub Acciones(op As Boolean, op2 As Boolean, arrAction As String)
        Dim lb As New LinkButton
        Dim arr2() As String = arrAction.Split(",")
        Dim arr(2) As String
        arr(0) = "LB2"
        arr(1) = "LB16"

        For i = 0 To UBound(arr)
            For j = 0 To UBound(arr2)
                If i = CInt(arr2(j)) Then
                    lb = UPContenido.FindControl(arr(i))
                    lb.Visible = op
                    lb.Enabled = op2
                    lb.CssClass = If(op2 = True, "lnkbtn-action", "lnkbtn-action_disabled")
                    If op2 = True Then lb.Attributes.Add("style", "cursor: pointer;")
                    'If op2 = True Then lb.Attributes.Add("style", "margin-left: 14px;")
                    'If op2 = True And i = 5 Then lb.Attributes.Add("style", "margin-left: 10px;")
                End If
            Next
        Next
    End Sub
    Sub SeguridadLoad()
        Dim IsAdm As Boolean = If(New ArrayList({"1", "2"}).IndexOf(ObjUser.CveRol.ToString) >= 0, True, False)
        Dim IsEstatus As Boolean = If(New ArrayList({"1"}).IndexOf(CveEstatus.Text) >= 0, True, False)
        Dim IsAutor = If(Autor.Text = ObjUser.CodUsuario, True, False)
        Acciones(True, False, "0,1")

    End Sub
    Sub LlenaRegistro()
        Try
            Dim ObjM As OptimizerP_ProgramaAModel = New OptimizerP_ProgramaA().FindById(Convert.ToInt64(regPId.Text), 0, "")
            LBLReferencia.Text = "FOLIO: " + ObjM.FolioR + " | " + ObjM.NomReferencia
            LBLCliente.Text = ObjM.NomCliente
            MostrarPrograma()

        Catch ex As Exception
            Alertas("", CleanSpecialCharacter(ex.Message), False, 4)
        End Try

    End Sub
    Sub LlenaRPT_Resultado()
        Try
            Dim ObjReg As OptimizerP_ProgramaAModel = New OptimizerP_ProgramaA().FindById(Convert.ToInt64(regPId.Text), 0, "")

            Dim ObjM As WSOptimizer_Presupuesto_OptimizadoModel = New Optimizer_Presupuesto_Optimizado().FindById(2, Convert.ToInt64(regPId.Text))
            Dim ObjRList As WSOptimizerP_ResponsePlan = JsonConvert.DeserializeObject(Of WSOptimizerP_ResponsePlan)(ObjM.Response)
            If ObjRList Is Nothing Then Exit Sub
            Dim tmp As String = ""
            Dim lineas As String = ""
            Dim row As Integer = 0

            Dim lstP As List(Of OptimizerP_ProgramaA_EtapasModel) = New OptimizerP_ProgramaA_Etapas().FindlstAll(ObjReg.CodCliente, Convert.ToInt64(regPId.Text), Convert.ToInt64(ObjReg.CvePerfilN))
            lstP.ForEach(Sub(p)
                             row += 1
                             If p.Aplica = "S" Then
                                 lineas += row.ToString + ","
                             End If
                         End Sub)
            If lineas <> "" Then lineas = Left(lineas, Len(lineas) - 1)
            Dim ObjR As WSOptimizerP_ResponseModel = ObjRList.ResponseParametro.Find(Function(p) p.CveParametro = ObjReg.CveParametro)

            ObjR.Data.ForEach(Sub(p)
                                  If New ArrayList(lineas.Split(",")).IndexOf(p.Identificador) >= 0 Then
                                      tmp += p.Identificador + "#" + Math.Round(p.EM_Alimento, 2).ToString + "#" + Math.Round(p.Req_EM, 2).ToString + "#" + Math.Round(p.Costo, 3).ToString +
                                 "#" + Math.Round(p.Duracion_Etapa, 2).ToString + "#" + Math.Round(p.Duracion_Minima, 2).ToString + "#" + Math.Round(p.Duracion_Maxima, 2).ToString + "#" + Math.Round(p.Peso_Inicial, 3).ToString +
                                 "#" + Math.Round(p.Peso_Final, 3).ToString + "#" + Math.Round(p.Peso_Medio, 3).ToString + "#" + Math.Round(p.SIDLysGDP, 2).ToString + "#" + Math.Round(p.ConsumoDiario, 3).ToString +
                                  "#" + Math.Round(p.AlimentoOfrecer, 3).ToString + "#" + Math.Round(p.PresupuestoAlimento, 3).ToString + "#" + Math.Round(p.Lisina, 3).ToString + "#" + Math.Round(p.CA, 3).ToString + "|"
                                  End If
                              End Sub)
            If tmp <> "" Then tmp = Left(tmp, Len(tmp) - 1)

            Dim dt As DataTable = New Optimizer_Presupuesto_Resultado().FindAll(2, tmp)
            rptResultado.DataSource = dt
            rptResultado.DataBind()



            Dim Duracion As Double = dt.Select.Sum(Function(p) CDbl(p("Duracion_Etapa")))
            Dim DuracionPigmento As Double = dt.Select.Where(Function(p) p("CveEtapa") > 2).Sum(Function(p) CDbl(p("Duracion_Etapa")))
            TryCast(rptResultado.Controls(rptResultado.Controls.Count - 1).Controls(0).FindControl("TOTDuracionEtapa"), Label).Text = Duracion.ToString("N0")
            'TBDiasPigmentoD.Text = DuracionPigmento.ToString("N0")

            CostoPonderado.Text = ObjR.Resultado.CostoPonderado.ToString("C2")
            CostoTotalAlimento.Text = ObjR.Resultado.CostoTotalAlimento.ToString("C2")
            Ca.Text = ObjR.Resultado.Ca.ToString("N2")
            CostoKiloProducido.Text = ObjR.Resultado.CostokiloProducido.ToString("N2")
            Utilidad.Text = ObjR.Resultado.Utilidad.ToString("N2")

            Roi.Text = ObjR.Resultado.Roi.ToString("N2")
            Gdp.Text = ObjR.Resultado.Gdp.ToString("N3")

        Catch ex As Exception
            rptResultado.DataSource = Nothing
            rptResultado.DataBind()
            Alertas("", CleanSpecialCharacter(ex.Message), False, 4)
        End Try
    End Sub

    Sub DefineGV()
        Dim dtCol As DataTable
        Dim wgv As Integer = 0
        Try
            gv.Columns.Clear()
            gv2.Columns.Clear()

            dtCol = New Reportes().Columnas(CInt(Plataforma), 3, CInt(regPId.Text))
            dtCol.AsEnumerable.ToList.ForEach(Sub(r)
                                                  Dim bfield As BoundField = New BoundField()
                                                  bfield.HeaderText = r("Titulo")
                                                  bfield.DataField = r("Campo")
                                                  bfield.HeaderStyle.Width = Unit.Percentage(r("Ancho"))
                                                  bfield.ItemStyle.Width = Unit.Percentage(r("Ancho"))
                                                  bfield.ItemStyle.HorizontalAlign = r("Alineado")
                                                  bfield.DataFormatString = r("Formato")
                                                  gv.Columns.Add(bfield)
                                                  wgv += r("Ancho")
                                              End Sub)
            gv.Width = Unit.Percentage(wgv)
            wgv = 0
            dtCol.AsEnumerable.ToList.ForEach(Sub(r)
                                                  Dim bfield As BoundField = New BoundField()
                                                  bfield.HeaderText = If(r("Posicion") = 1, "", "PRESUPUESTO - " + r("Titulo"))
                                                  bfield.DataField = r("Campo")
                                                  bfield.HeaderStyle.Width = Unit.Percentage(r("Ancho"))
                                                  bfield.ItemStyle.Width = Unit.Percentage(r("Ancho"))
                                                  bfield.ItemStyle.HorizontalAlign = r("Alineado")
                                                  bfield.DataFormatString = r("Formato")
                                                  gv2.Columns.Add(bfield)
                                                  wgv += r("Ancho")
                                              End Sub)
            gv2.Width = Unit.Percentage(wgv)

        Catch ex As Exception
            Alertas("", CleanSpecialCharacter(ex.Message), False, 4)
        Finally
            dtCol = Nothing
        End Try

        gv.HeaderStyle.HorizontalAlign = HorizontalAlign.Center
    End Sub
    Sub LlenaGV()
        Dim dt As DataTable
        Try
            dt = New Reportes().Datos(CInt(Plataforma), 3, regPId.Text)
            gv.DataSource = dt
            gv.DataBind()

            dt = New Reportes().Datos(CInt(Plataforma), CInt(31), regPId.Text)
            gv2.DataSource = dt
            gv2.DataBind()

            Dim total(gv2.Columns.Count - 1) As Double

            For i = 1 To gv2.Columns.Count - 1
                For Each row As GridViewRow In gv2.Rows
                    If row.RowType = DataControlRowType.DataRow Then total(i) += CDbl(row.Cells(i).Text)
                Next
                gv2.FooterRow.Cells(i).Text = total(i).ToString("N2")
            Next

        Catch ex As Exception
            gv.DataSource = Nothing
            gv.DataBind()
            gv2.DataSource = Nothing
            gv2.DataBind()
            Alertas("", CleanSpecialCharacter(ex.Message), False, 4)
        End Try
    End Sub
    'PRESUPUESTOS
    Protected Sub GVRowDataBound2(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gv2.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            ' Conta.Text += e.Row.Cells(0).Text + " | "
            'e.Row.Cells(0).Style.Add("cursor", "pointer")
            e.Row.Cells(0).Font.Bold = True
            e.Row.Cells(1).Font.Bold = True
            'If rptEtapas.Items.Cast(Of RepeaterItem)().Where(Function(p) TryCast(p.FindControl("CveEtapa"), Label).Text = (e.Row.RowIndex + 1).ToString And TryCast(p.FindControl("Aplica"), Label).Text = "S").Count = 0 Then
            '    e.Row.Visible = False
            'End If
            For i = 1 To 7
                Math.Round(CDbl(e.Row.Cells(i).Text), 3).ToString("N2")
            Next
        End If
    End Sub
    'VARIABLES ECONOMICAS
    Protected Sub GVRowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gv.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            ' Conta.Text += e.Row.Cells(0).Text + " | "
            'e.Row.Cells(0).Style.Add("cursor", "pointer")
            e.Row.Cells(0).Font.Bold = True
            e.Row.Cells(1).Font.Bold = True
            For i = 1 To 7
                If New ArrayList({0, 7, 8, 9, 10}).IndexOf(e.Row.RowIndex) >= 0 Then e.Row.Cells(i).Text = CDbl(e.Row.Cells(i).Text).ToString("C2")
                If New ArrayList({1, 2, 3, 4, 5, 6, 11}).IndexOf(e.Row.RowIndex) >= 0 Then e.Row.Cells(i).Text = Math.Round(CDbl(e.Row.Cells(i).Text), 3).ToString("N2")

            Next
            If e.Row.RowIndex = 2 Then e.Row.Visible = False
        End If
    End Sub
    Protected Sub GVPreRender(sender As Object, e As EventArgs) Handles gv.PreRender
        If (gv.ShowHeader = True And gv.Rows.Count > 0) Or (gv.ShowHeaderWhenEmpty = True) Then
            gv.HeaderRow.TableSection = TableRowSection.TableHeader
        End If

    End Sub
    Protected Sub GVPreRender2(sender As Object, e As EventArgs) Handles gv2.PreRender
        If (gv2.ShowHeader = True And gv2.Rows.Count > 0) Or (gv2.ShowHeaderWhenEmpty = True) Then
            gv2.HeaderRow.TableSection = TableRowSection.TableHeader
        End If
        If (gv2.ShowFooter = True And gv2.Rows.Count > 0) Then
            gv2.FooterRow.TableSection = TableRowSection.TableFooter

        End If
    End Sub


    '--Acciones---
    Sub Regresar()
        Response.Redirect(New RedirectPaginas().FindById(Plataforma + "-" + menu + "-0").PaginaURL.Replace("@filtro", Codif(filtroview.Text)).Replace("@pageIndex", gvindexpage.Text), True)
    End Sub
    Sub Refrescar()
        Response.Redirect(New RedirectPaginas().FindById(Plataforma + "-" + menu + "-1").PaginaURL.Replace("@Id", Codif(regPId.Text)).Replace("@CvePN", Codif(CvePerfilN.Text)).Replace("@filtro", Codif(filtroview.Text)).Replace("@pageIndex", gvindexpage.Text), True)
    End Sub
    Sub MostrarPrograma()
        LlenaRPT_Resultado()
        tbl_resultado.Visible = True
        DefineGV()
        LlenaGV()
    End Sub
    Sub LimpiaOptimizado()
        rptResultado.DataSource = Nothing
        rptResultado.DataBind()
        tbl_resultado.Visible = False

        gv.DataSource = Nothing
        gv.DataBind()
        gv2.DataSource = Nothing
        gv2.DataBind()
    End Sub
    Sub MostrarPerfil()
        Dim ObjM As OptimizerP_ProgramaAModel = New OptimizerP_ProgramaA().FindById(Convert.ToInt64(regPId.Text), 0, "")
        Dim filtro As String = ObjM.CodCliente + "|1|" + regPId.Text
        Response.Redirect(New RedirectPaginas().FindById(Plataforma + "-61-1").PaginaURL.Replace("@Id", Codif(ObjM.CvePerfilN)).Replace("@filtro", Codif(filtro)).Replace("@pageIndex", gvindexpage.Text), True)
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
    Protected Sub OnCheckedChanged(sender As Object, e As EventArgs)
        Dim chk As CheckBox = TryCast(sender, CheckBox)
        Dim item As RepeaterItem = CType(chk.NamingContainer, RepeaterItem)
        Dim TBCosto As TextBox = TryCast(item.FindControl("TBCosto"), TextBox)
        Dim TBEM As TextBox = TryCast(item.FindControl("TBEM"), TextBox)
        Dim TBSIDLYSD As Label = TryCast(item.FindControl("TBSIDLYSD"), Label)
        Dim TBSIDLYS As TextBox = TryCast(item.FindControl("TBSIDLYS"), TextBox)
        Dim TBDuracionMin As TextBox = TryCast(item.FindControl("TBDuracionMin"), TextBox)
        Dim TBDuracionMax As TextBox = TryCast(item.FindControl("TBDuracionMax"), TextBox)

        TBCosto.Enabled = chk.Checked
        TBEM.Enabled = chk.Checked
        TBDuracionMin.Enabled = chk.Checked
        TBDuracionMax.Enabled = chk.Checked
        TBSIDLYS.Enabled = chk.Checked
        TBCosto.Text = "0"
        TBEM.Text = "0"
        TBSIDLYS.Text = 0
        TBSIDLYSD.Text = 0
        TBDuracionMin.Text = "0"
        TBDuracionMax.Text = "0"

    End Sub
    Public Overrides Sub VerifyRenderingInServerForm(control As Control)
        ' Verifies that the control is rendered
    End Sub

End Class