Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Runtime.Remoting
Imports System.Web.DynamicData
Imports Microsoft.Ajax.Utilities
Imports Newtonsoft.Json
Imports NukaxanWEB.Libreria
Imports NukaxanWEB.OptimizerG_ProgramaA

Public Class OptimizerG_ProgramaA_ReporteFrm
    Inherits Page
    Private ObjUser As UsuarioModel
    Private Plataforma As String = "43"
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

        iList.Add(New DatosGrid() With {.Tipo = 0, .Campo = "CveEtapa", .Valida = "N", .ValidaCeros = "N"})
        iList.Add(New DatosGrid() With {.Tipo = 0, .Campo = "Aplica", .Valida = "N", .ValidaCeros = "N"})
        iList.Add(New DatosGrid() With {.Tipo = 1, .Campo = "TBCosto", .Valida = "S", .ValidaCeros = "N"})
        iList.Add(New DatosGrid() With {.Tipo = 0, .Campo = "EdadIni", .Valida = "N", .ValidaCeros = "N"})
        iList.Add(New DatosGrid() With {.Tipo = 0, .Campo = "EdadFin", .Valida = "N", .ValidaCeros = "N"})
        iList.Add(New DatosGrid() With {.Tipo = 0, .Campo = "Mortalidad", .Valida = "N", .ValidaCeros = "N"})
        iList.Add(New DatosGrid() With {.Tipo = 0, .Campo = "ConsumoAlimento", .Valida = "N", .ValidaCeros = "N"})
        iList.Add(New DatosGrid() With {.Tipo = 0, .Campo = "PesoHuevo", .Valida = "N", .ValidaCeros = "N"})
        iList.Add(New DatosGrid() With {.Tipo = 0, .Campo = "Produccion", .Valida = "N", .ValidaCeros = "N"})

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
        LlenaDDL()
        LlenaRegistro()

    End Sub
    Sub Etiquetas()
        'General
        defaultoption = lstEtiquetas.Find(Function(p) p.CvePlataforma = 1 And p.CveMenu = 0 And p.CveEtiqueta = 1).NomEtiqueta
        Dim obligatorio As String = "<label class='control-label color-red'>*</label>"

        'Titulo
        Dim lstMenu As MenuModel = New Menu().FindById(ObjUser.CveRol, CInt(Plataforma), -1, menu)
        'PageTitulo.Text = "Perfil Nutricional - Reporte"

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
        'SECCIONES TOTALES OPTIMIZADO
        'SECCIONES TOTALES OPTIMIZADO
        LBLCostoPonderadoAlimTotal.Text = "COSTO PONDERADO DEL ALIMENTO TOTAL, $"
        'LBAEPT2.Text = "CONSUMO TOTAL DE ALIMENTO, KG/AVE"
        LBLCostoKGProducido.Text = "COSTO POR KG PRODUCIDO, $/KG HUEVO"
        'LBAEPT4.Text = "CONVERSIÓN ALIMENTICIA (CRIANZA + POSTURA)"
        'LBAEPT5.Text = "MASA DE HUEVO TOTAL, KG/AVE"
        LBLCostoProgramaAlimTotal.Text = "COSTO PROGAMA DE ALIMENTACIÓN TOTAL, $/AVE"
        'LBAEPT7.Text = "CONSUMO TOTAL DE ALIMENTO, KG/PARVADA"
        'LBAEPT8.Text = "COSTO PROGAMA DE ALIMENTACIÓN TOTAL, $/PARVADA"
        LBLPrecioVentaHuevo.Text = "PRECIO VENTA ($/Kg huevo)"
        LBLMasaHuevoKGParvada.Text = "MASA DE HUEVO, KG/PARVADA"
        LBLIngresoxVentaHuevo.Text = "INGRESO POR VENTA DE HUEVO, $/PARVADA"
        LBLUtilidadBruta.Text = "UTILIDAD POR CONCEPTO DE ALIMENTACIÓN, $/PARVADA"
        LBLROI.Text = "ROI, %"

        LBAEPC1.Text = "COSTO PONDERADO DEL ALIMENTO CRIANZA, $"
        LBAEPC2.Text = "CONSUMO DE ALIMENTO CRIANZA, KG/AVE"
        LBAEPC3.Text = "COSTO PROGAMA DE ALIMENTACIÓN CRIANZA, $/POLLITA"

        LBAEPP1.Text = "COSTO PONDERADO DEL ALIMENTO POSTURA, $/KG"
        LBAEPP2.Text = "CONSUMO TOTAL DE ALIMENTO POSTURA, KG/AVE"
        LBAEPP3.Text = "COSTO POR KG PRODUCIDO, $/KG HUEVO"
        LBAEPP4.Text = "CONVERSIÓN ALIMENTICIA"
        LBAEPP5.Text = "COSTO PROGAMA DE ALIMENTACIÓN POSTURA, $/AVE"
        LBAEPP6.Text = "MASA DE HUEVO TOTAL, KG/AVE"
        LBAEPP7.Text = "INGRESO POR VENTA DE HUEVO, $/AVE"
        LBAEPP8.Text = "UTILIDAD POR CONCEPTO DE ALIMENTACIÓN, $"
        LBAEPP9.Text = "ROI, %"

    End Sub
    Sub LlenaDDL()

    End Sub
    Sub Acciones(op As Boolean, op2 As Boolean, arrAction As String)
        Dim lb As New LinkButton
        Dim arr2() As String = arrAction.Split(",")
        Dim arr(4) As String
        arr(0) = "LB2"
        arr(1) = "LB12"
        arr(2) = "LB14"
        arr(3) = "LB16"

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
        Acciones(False, False, "0,1,2,3")

    End Sub
    Sub LlenaRegistro()
        Try

            Dim ObjM As OptimizerG_ProgramaAModel = New OptimizerG_ProgramaA().FindById(Convert.ToInt64(regPId.Text), 0, "")

            LBLReferencia.Text = "FOLIO: " + ObjM.FolioR + " | " + ObjM.NomReferencia
            LBLCliente.Text = ObjM.NomCliente
            LlenaRPT_Resultado()
        Catch ex As Exception
            Alertas("", CleanSpecialCharacter(ex.Message), False, 4)
        End Try

    End Sub
    Sub LlenaRPT_Resultado()
        Try
            'Dim ObjM As OptimizerG_ProgramaAModel = New OptimizerG_ProgramaA().FindById(Convert.ToInt64(regPId.Text), 0, "")
            Dim lstRPA As List(Of ResultadoPAModel) = New OptimizerG_ProgramaA().ObtenResultadoPA(Convert.ToInt64(regPId.Text))
            rptResultado.DataSource = lstRPA
            rptResultado.DataBind()
            rptResultado.Items.Cast(Of RepeaterItem)().ToList.ForEach(Sub(p)
                                                                          Dim CveEtapa As String = TryCast(p.FindControl("CveEtapa"), Label).Text
                                                                          Dim PesoHuevo As Label = TryCast(p.FindControl("PesoHuevo"), Label)
                                                                          Dim Produccion As Label = TryCast(p.FindControl("Produccion"), Label)
                                                                          Dim MasaHuevo As Label = TryCast(p.FindControl("MasaHuevo"), Label)
                                                                          Dim ConversionAlimenticia As Label = TryCast(p.FindControl("ConversionAlimenticia"), Label)
                                                                          'Dim HuevoProducido As Label = TryCast(p.FindControl("HuevoProducido"), Label)

                                                                          Dim IsOculta As Boolean = If(New ArrayList({"1", "2", "3", "4", "5"}).IndexOf(CveEtapa) >= 0, False, True)

                                                                          PesoHuevo.Visible = IsOculta
                                                                          Produccion.Visible = IsOculta
                                                                          MasaHuevo.Visible = IsOculta
                                                                          ConversionAlimenticia.Visible = IsOculta
                                                                          'HuevoProducido.Visible = IsOculta

                                                                      End Sub)

            Dim lstTot As List(Of String) = New OptimizerG_ProgramaA().ObtenTotales(Convert.ToInt64(regPId.Text))

            TryCast(rptResultado.Controls(rptResultado.Controls.Count - 1).Controls(0).FindControl("TOTConsumoAlimento"), Label).Text = CDbl(lstTot(1)).ToString("N2")
            TryCast(rptResultado.Controls(rptResultado.Controls.Count - 1).Controls(0).FindControl("TOTMasaHUevo"), Label).Text = CDbl(lstTot(4)).ToString("N2")
            TryCast(rptResultado.Controls(rptResultado.Controls.Count - 1).Controls(0).FindControl("TOTCA"), Label).Text = CDbl(lstTot(3)).ToString("N2")

            TBCostoPonderadoAlimTotal.Text = lstTot(0)
            'TBAEPT2.Text = lstTot(1)
            TBCostoKGProducido.Text = lstTot(2)
            'TBAEPT4.Text = lstTot(3)
            'TBAEPT5.Text = lstTot(4)
            TBCostoProgramaAlimTotal.Text = lstTot(5)
            'TBAEPT7.Text = lstTot(6)
            'TBAEPT8.Text = lstTot(7)
            TBMasaHuevoKGParvada.Text = lstTot(8)
            'TBPrecioVentaHuevo.Text = TBPrecioVentaH.Text
            TBIngresoxVentaHuevo.Text = lstTot(9)
            TBUtilidadBruta.Text = lstTot(10)
            TBROI.Text = lstTot(11)

            TBAEPC1.Text = lstTot(12)
            TBAEPC2.Text = lstTot(13)
            TBAEPC3.Text = lstTot(14)

            TBAEPP1.Text = lstTot(15)
            TBAEPP2.Text = lstTot(16)
            TBAEPP3.Text = lstTot(17)
            TBAEPP4.Text = lstTot(18)
            TBAEPP5.Text = lstTot(19)
            TBAEPP6.Text = lstTot(20)
            TBAEPP7.Text = lstTot(21)
            TBAEPP8.Text = lstTot(22)
            TBAEPP9.Text = lstTot(23)



        Catch ex As Exception
            rptResultado.DataSource = Nothing
            rptResultado.DataBind()
            Alertas("", CleanSpecialCharacter(ex.Message), False, 4)
        End Try
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
        'tbl_resultado.Visible = True
    End Sub
    Sub LimpiaOptimizado()
        rptResultado.DataSource = Nothing
        rptResultado.DataBind()
        'tbl_resultado.Visible = False

    End Sub
    Sub MostrarPerfil()
        Dim ObjM As OptimizerG_ProgramaAModel = New OptimizerG_ProgramaA().FindById(Convert.ToInt64(regPId.Text), 0, "")
        Dim filtro As String = ObjM.CodCliente + "|1|" + regPId.Text
        Response.Redirect(New RedirectPaginas().FindById(Plataforma + "-61-1").PaginaURL.Replace("@Id", Codif(ObjM.CvePerfilN.ToString)).Replace("@filtro", Codif(filtro)).Replace("@pageIndex", gvindexpage.Text), True)
    End Sub

    Sub DescargarExcel()
        DescargarArchivoReporte("excel", ConfigurationManager.AppSettings("WSOptimizerGallinas"))
    End Sub
    Sub DescargarPdf()
        DescargarArchivoReporte("pdf", ConfigurationManager.AppSettings("WSOptimizerGallinas"))
    End Sub
    Private Sub DescargarArchivoReporte(formato As String, baseApiUrl As String)
        Try
            If regPId.Text = "0" Then Throw New Exception("No se encontró el identificador del perfil para generar el archivo.")
            OptimizerReporteDescarga.Descargar(Me, baseApiUrl, Convert.ToInt64(regPId.Text), formato, 0, "ProgramaAlimentacion", "programaalimentacion", "presupuesto")
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
    Protected Sub OnCheckedChanged(sender As Object, e As EventArgs)
        Dim chk As CheckBox = TryCast(sender, CheckBox)
        Dim item As RepeaterItem = CType(chk.NamingContainer, RepeaterItem)
        Dim TBCosto As TextBox = TryCast(item.FindControl("TBCosto"), TextBox)

        TBCosto.Enabled = chk.Checked
        TBCosto.Text = If(chk.Checked = False, "0", TBCosto.Text)

    End Sub
    Public Overrides Sub VerifyRenderingInServerForm(control As Control)
        ' Verifies that the control is rendered
    End Sub

End Class
