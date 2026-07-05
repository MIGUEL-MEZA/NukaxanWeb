Imports System.Configuration
Imports System.Drawing
Imports Newtonsoft.Json
Imports NukaxanWEB.Libreria

Public Class OptimizerC_ProgramaA_ReporteFrm
    Inherits Page

    Private ObjUser As UsuarioModel
    Private Plataforma As String = "41"
    Private menu As String = "62"
    Private productosAplicados As List(Of String)

    Public defaultoption As String = ""
    Public msg As String = ""
    Private lstMensajes As List(Of MensajesModel)
    Private lstErrores As List(Of MensajesModel)
    Private lstEtiquetas As List(Of EtiquetasModel)
    Private lstAcciones As List(Of Controles_AccionesModel)
    Private lstControles As List(Of Controles_CapturaModel)
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

        RegistrarDescargaDirecta()

        If Not Page.IsPostBack Then
            DatosLoad()
        End If

        Response.ContentEncoding = System.Text.Encoding.UTF8
        Response.Charset = "utf-8"
    End Sub

    Private Sub RegistrarDescargaDirecta()
        RegistrarControlDescarga("LB20")
        RegistrarControlDescarga("LB21")
    End Sub

    Private Sub RegistrarControlDescarga(controlId As String)
        Dim scriptManager = System.Web.UI.ScriptManager.GetCurrent(Page)
        If scriptManager Is Nothing Then Exit Sub

        Dim control = BuscarControlRecursivo(Me, controlId)
        If Not control Is Nothing Then
            scriptManager.RegisterPostBackControl(control)
        End If
    End Sub

    Private Function BuscarControlRecursivo(parent As System.Web.UI.Control, controlId As String) As System.Web.UI.Control
        If parent Is Nothing Then Return Nothing

        Dim control = parent.FindControl(controlId)
        If Not control Is Nothing Then Return control

        For Each child As System.Web.UI.Control In parent.Controls
            control = BuscarControlRecursivo(child, controlId)
            If Not control Is Nothing Then Return control
        Next

        Return Nothing
    End Function

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
        Dim etiquetaDefault = lstEtiquetas.Find(Function(p) p.CvePlataforma = 1 And p.CveMenu = 0 And p.CveEtiqueta = 1)
        If Not etiquetaDefault Is Nothing Then defaultoption = etiquetaDefault.NomEtiqueta

        Dim lstMenu As MenuModel = New Menu().FindById(ObjUser.CveRol, CInt(Plataforma), -1, menu)
        If Not lstMenu Is Nothing Then
            PageTitulo.Text = lstMenu.NomMenu
        Else
            PageTitulo.Text = "Programa de Alimentación - Reporte"
        End If

        For Each a As Controles_AccionesModel In lstAcciones
            Select Case a.CveTipo
                Case 1
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

                Case 2
                    Dim LB As LinkButton = TryCast(UPContenido.FindControl("LB" + a.CveAccion.ToString), LinkButton)
                    Dim LB_IMG As HtmlGenericControl = TryCast(UPContenido.FindControl("LB_IMG" + a.CveAccion.ToString), HtmlGenericControl)
                    Dim LB_LBL As Label = TryCast(UPContenido.FindControl("LB_LBL" + a.CveAccion.ToString), Label)
                    If Not LB Is Nothing Then
                        If Not LB_IMG Is Nothing Then
                            LB_IMG.Attributes("class") = a.Icono
                            LB_IMG.Style("font-size") = a.IconoSize + "!important"
                        End If
                        LB.ToolTip = a.ToolTip
                        If Not LB_LBL Is Nothing Then LB_LBL.Text = a.NomAccion
                        If a.ValidaMensaje = "S" Then LB.OnClientClick = "return confirm('" + a.ValidaMensaje + "');"
                    End If

                Case 3
                    Dim IB As ImageButton = TryCast(UPContenido.FindControl("IB" + a.CveAccion.ToString), ImageButton)
                    If Not IB Is Nothing Then
                        IB.ToolTip = a.ToolTip
                        If a.ValidaMensaje = "S" Then IB.OnClientClick = "return confirm('" + a.ValidaMensaje + "');"
                    End If
            End Select
        Next

        For Each a As EtiquetasModel In lstEtiquetas.Where(Function(p) p.CveTipo = "4")
            Dim LBLSEC As Label = CType(UPContenido.FindControl("SECTitulo" + a.CveEtiqueta.ToString), Label)
            If Not LBLSEC Is Nothing Then LBLSEC.Text = a.NomEtiqueta
        Next
    End Sub

    Sub Acciones(op As Boolean, op2 As Boolean, arrAction As String)
        Dim lb As LinkButton
        Dim arr2() As String = arrAction.Split(",")
        Dim arr() As String = {"LB2", "LB16"}

        For i = 0 To UBound(arr)
            For j = 0 To UBound(arr2)
                If i = CInt(arr2(j)) Then
                    lb = TryCast(UPContenido.FindControl(arr(i)), LinkButton)
                    If Not lb Is Nothing Then
                        lb.Visible = op
                        lb.Enabled = op2
                        lb.CssClass = If(op2 = True, "lnkbtn-action", "lnkbtn-action_disabled")
                        If op2 = True Then lb.Attributes.Add("style", "cursor: pointer;")
                    End If
                End If
            Next
        Next
    End Sub

    Sub SeguridadLoad()
        Acciones(True, True, "0,1")
    End Sub

    Sub LlenaRegistro()
        Try
            Dim ObjM As OptimizerC_ProgramaAModel = New OptimizerC_ProgramaA().FindById(Convert.ToInt64(regPId.Text), 0, "")
            If ObjM Is Nothing Then Throw New Exception("No se encontró el programa de alimentación.")

            CodCliente.Text = ObjM.CodCliente
            CvePerfilN.Text = ObjM.CvePerfilN.ToString
            CveEstatus.Text = ObjM.CveEstatus.ToString
            Autor.Text = ObjM.UsuAlta
            LBLReferencia.Text = "FOLIO: " + ObjM.FolioR + " | " + ObjM.NomReferencia
            LBLCliente.Text = ObjM.NomCliente

            MostrarPrograma()
            SeguridadLoad()
        Catch ex As Exception
            Alertas("", CleanSpecialCharacter(ex.Message), False, 4)
        End Try
    End Sub

    Sub LlenaRPT_Resultado()
        Try
            Dim ObjReg As OptimizerC_ProgramaAModel = New OptimizerC_ProgramaA().FindById(Convert.ToInt64(regPId.Text), 0, "")
            Dim ObjM As WSOptimizer_Presupuesto_OptimizadoModel = New Optimizer_Presupuesto_Optimizado().FindById(1, Convert.ToInt64(regPId.Text))
            Dim ObjRList As WSOptimizerC_ResponsePlan = JsonConvert.DeserializeObject(Of WSOptimizerC_ResponsePlan)(ObjM.Response)
            If ObjRList Is Nothing OrElse ObjRList.ResponseParametro Is Nothing Then Exit Sub

            Dim tmp As String = ""
            Dim lineas As String = ""
            Dim row As Integer = 0
            Dim lstP As List(Of OptimizerC_ProgramaA_EtapasModel) = New OptimizerC_ProgramaA_Etapas().FindlstAll(ObjReg.CodCliente, Convert.ToInt64(regPId.Text))
            lstP.ForEach(Sub(p)
                             row += 1
                             If p.Aplica = "S" Then lineas += row.ToString + ","
                         End Sub)
            If lineas <> "" Then lineas = Left(lineas, Len(lineas) - 1)

            Dim ObjR As WSOptimizerC_ResponseModel = ObjRList.ResponseParametro.Find(Function(p) p.CveParametro = ObjReg.CveParametro)
            If ObjR Is Nothing Then Exit Sub

            ObjR.Data.ForEach(Sub(p)
                                  If New ArrayList(lineas.Split(",")).IndexOf(p.Identificador) >= 0 Then
                                      tmp += p.Identificador + "#" + Math.Round(p.Costo, 2).ToString + "#" + Math.Round(p.CDA_Kg, 3).ToString + "#" + Math.Round(p.PresupuestoCerdo, 2).ToString +
                                         "#" + Math.Round(p.GDP, 3).ToString + "#" + Math.Round(p.Peso_Inicial, 2).ToString + "#" + Math.Round(p.Peso_Final, 2).ToString + "#" + Math.Round(p.CA, 2).ToString +
                                         "#" + Math.Round(p.Edad_Inicial, 0).ToString + "#" + Math.Round(p.Edad_Final, 0).ToString + "#" + Math.Round(Math.Round(p.Edad_Final, 0) - Math.Round(p.Edad_Inicial, 0), 0).ToString + "|"
                                  End If
                              End Sub)
            If tmp <> "" Then tmp = Left(tmp, Len(tmp) - 1)

            Dim dt As DataTable = New Optimizer_Presupuesto_Resultado().FindAll(1, tmp)
            rptResultado.DataSource = dt
            rptResultado.DataBind()

            TryCast(rptResultado.Controls(rptResultado.Controls.Count - 1).Controls(0).FindControl("TOTCDA"), Label).Text = Math.Round(ObjR.Resultado.Cda, 2).ToString("N2")
            TryCast(rptResultado.Controls(rptResultado.Controls.Count - 1).Controls(0).FindControl("TOTPresupuesto"), Label).Text = ObjR.Resultado.Presupuesto.ToString("N1")
            TryCast(rptResultado.Controls(rptResultado.Controls.Count - 1).Controls(0).FindControl("TOTGDP"), Label).Text = Math.Round(ObjR.Resultado.Gdp, 2).ToString("N3")
            TryCast(rptResultado.Controls(rptResultado.Controls.Count - 1).Controls(0).FindControl("TOTCA"), Label).Text = Math.Round(ObjR.Resultado.Ca, 2).ToString("N2")

            Tot_PrecioVenta.Text = ObjR.Resultado.PrecioVenta.ToString("C2")
            Tot_PesoVenta.Text = ObjR.Resultado.PesoVenta.ToString("N2")
            Tot_EdadVenta.Text = Math.Round(ObjR.Resultado.EdadVenta, 2).ToString("N0")
            Tot_AlimentoPresupuestado.Text = ObjR.Resultado.Presupuesto.ToString("N1")
            Tot_KilosProducidos.Text = Math.Round(ObjR.Resultado.KilosProducidos, 2).ToString("N2")
            Tot_GDP.Text = Math.Round(ObjR.Resultado.Gdp, 2).ToString("N2")
            Tot_CA.Text = Math.Round(ObjR.Resultado.Ca, 2).ToString("N2")
            Tot_CostoTotal.Text = Math.Round(ObjR.Resultado.CostoTotalAlimento, 2).ToString("C2")
            Tot_CostoPonderado.Text = Math.Round(ObjR.Resultado.CostoPonderado, 2).ToString("C2")
            Tot_CostoKiloProd.Text = ObjR.Resultado.CostokiloProducido.ToString("C2")
            Tot_Utilidad.Text = ObjR.Resultado.Utilidad.ToString("C2")
            Tot_ROI.Text = ObjR.Resultado.Roi.ToString("N2")
        Catch ex As Exception
            rptResultado.DataSource = Nothing
            rptResultado.DataBind()
            Alertas("", CleanSpecialCharacter(ex.Message), False, 4)
        End Try
    End Sub

    Private Function ObtenerProductosAplicados() As List(Of String)
        If productosAplicados Is Nothing Then
            productosAplicados = New OptimizerC_ProgramaA_Etapas().FindlstAll(CodCliente.Text, Convert.ToInt64(regPId.Text)) _
                .Where(Function(p) p.Aplica = "S") _
                .Select(Function(p) p.CveProducto.ToString()) _
                .ToList()
        End If

        Return productosAplicados
    End Function

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
                                                  bfield.HeaderText = If(r("Posicion") = 1, "", "PRESUPUESTO POR CERDO (Kg) - " + r("Titulo"))
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

            If gv2.FooterRow IsNot Nothing Then
                Dim total(gv2.Columns.Count - 1) As Double
                For i = 1 To gv2.Columns.Count - 1
                    For Each row As GridViewRow In gv2.Rows
                        If row.RowType = DataControlRowType.DataRow Then total(i) += CDbl(row.Cells(i).Text)
                    Next
                    gv2.FooterRow.Cells(i).Text = total(i).ToString("N2")
                Next
            End If
        Catch ex As Exception
            gv.DataSource = Nothing
            gv.DataBind()
            gv2.DataSource = Nothing
            gv2.DataBind()
            Alertas("", CleanSpecialCharacter(ex.Message), False, 4)
        End Try
    End Sub

    Protected Sub GVRowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs) Handles gv.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Cells(0).Font.Bold = True
            e.Row.Cells(1).Font.Bold = True
            Dim maxCol As Integer = Math.Min(6, e.Row.Cells.Count - 1)
            For i = 1 To maxCol
                If New ArrayList({0, 7, 8, 9, 10}).IndexOf(e.Row.RowIndex) >= 0 Then e.Row.Cells(i).Text = CDbl(e.Row.Cells(i).Text).ToString("C2")
                If New ArrayList({1, 2, 3, 4, 5, 6, 11}).IndexOf(e.Row.RowIndex) >= 0 Then e.Row.Cells(i).Text = CDbl(e.Row.Cells(i).Text).ToString("N2")
            Next
        End If
    End Sub

    Protected Sub GVRowDataBound2(ByVal sender As Object, ByVal e As GridViewRowEventArgs) Handles gv2.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Cells(0).Font.Bold = True
            If e.Row.Cells.Count > 1 Then e.Row.Cells(1).Font.Bold = True
            If ObtenerProductosAplicados().IndexOf((e.Row.RowIndex + 1).ToString()) < 0 Then
                e.Row.Visible = False
            End If
        End If
    End Sub

    Protected Sub GVPreRender(sender As Object, e As EventArgs) Handles gv.PreRender
        If ((gv.ShowHeader = True And gv.Rows.Count > 0) Or gv.ShowHeaderWhenEmpty = True) AndAlso gv.HeaderRow IsNot Nothing Then
            gv.HeaderRow.TableSection = TableRowSection.TableHeader
        End If
    End Sub

    Protected Sub GVPreRender2(sender As Object, e As EventArgs) Handles gv2.PreRender
        If ((gv2.ShowHeader = True And gv2.Rows.Count > 0) Or gv2.ShowHeaderWhenEmpty = True) AndAlso gv2.HeaderRow IsNot Nothing Then
            gv2.HeaderRow.TableSection = TableRowSection.TableHeader
        End If
        If gv2.ShowFooter = True And gv2.Rows.Count > 0 AndAlso gv2.FooterRow IsNot Nothing Then
            gv2.FooterRow.TableSection = TableRowSection.TableFooter
        End If
    End Sub

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
        Dim ObjM As OptimizerC_ProgramaAModel = New OptimizerC_ProgramaA().FindById(Convert.ToInt64(regPId.Text), 0, "")
        Dim filtro As String = ObjM.CodCliente + "|1|" + regPId.Text
        Response.Redirect(New RedirectPaginas().FindById(Plataforma + "-61-1").PaginaURL.Replace("@Id", Codif(ObjM.CvePerfilN.ToString)).Replace("@filtro", Codif(filtro)).Replace("@pageIndex", gvindexpage.Text), True)
    End Sub

    Sub DescargarExcel()
        DescargarArchivoReporte("excel", ConfigurationManager.AppSettings("WSOptimizer"))
    End Sub

    Sub DescargarPdf()
        DescargarArchivoReporte("pdf", ConfigurationManager.AppSettings("WSOptimizer"))
    End Sub

    Private Sub DescargarArchivoReporte(formato As String, baseApiUrl As String)
        Try
            If regPId.Text = "0" Then Throw New Exception("No se encontró el identificador del perfil para generar el archivo.")
            Dim seccion As String = GetSeccionReporteSeleccionada()
            Dim prefijo As String = If(seccion = "comparativo", "ProgramaAlimentacion_Comparativo", "ProgramaAlimentacion")
            OptimizerReporteDescarga.Descargar(Me, baseApiUrl, Convert.ToInt64(regPId.Text), formato, 0, prefijo, "programaalimentacion", seccion)
        Catch ex As Exception
            Alertas("", CleanSpecialCharacter(ex.Message), False, 4)
        End Try
    End Sub

    Private Function GetSeccionReporteSeleccionada() As String
        Dim tabActual As String = TabName.Value.Trim().ToLowerInvariant()
        If tabActual = "comparativo" Then Return "comparativo"
        Return "presupuesto"
    End Function

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
        End Select
    End Sub

    Public Overrides Sub VerifyRenderingInServerForm(control As Control)
    End Sub
End Class