Imports System.Web.DynamicData
Imports Newtonsoft.Json
Imports System.Configuration
Imports NukaxanWEB.Libreria
Imports NukaxanWEB.OptimizerP_PerfilN

Public Class OptimizerG_PerfilN_ReporteFrm
    Inherits Page
    Protected WithEvents LB15 As LinkButton
    Protected WithEvents LB_IMG15 As HtmlGenericControl
    Protected WithEvents LB_LBL15 As Label
    Protected WithEvents LBExcel As LinkButton
    Protected WithEvents LB_LBLExcel As Label
    Protected WithEvents LBPdf As LinkButton
    Protected WithEvents LB_LBLPdf As Label
    Protected WithEvents LBLReferencia As Label
    Protected WithEvents LBLCliente As Label
    Public ObjUser As UsuarioModel
    Private Plataforma As String = "43"
    Private menu As String = "61"
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

        RegistrarDescargaDirecta()

        If Not Page.IsPostBack Then
            DatosLoad()
        End If


        Response.ContentEncoding = System.Text.Encoding.UTF8
        Response.Charset = "utf-8"


    End Sub
    Private Sub RegistrarDescargaDirecta()
        RegistrarControlDescarga("LBExcel")
        RegistrarControlDescarga("LBPdf")
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
        'pnlPopup.Style.Value = "display:none;"
        regPId.Text = "0"
        filtroview.Text = ""
        gvindexpage.Text = "0"
        If Not Request.QueryString("Id") Is Nothing Then regPId.Text = DeCodif(Request.QueryString("Id"))
        If Not Request.QueryString("filtro") Is Nothing Then filtroview.Text = DeCodif(Request.QueryString("filtro"))
        If Not Request.QueryString("pageIndex") Is Nothing Then gvindexpage.Text = Request.QueryString("pageIndex")
        mpe_op.Text = ""
        Etiquetas()
        LlenaRegistro()
    End Sub
    Sub Etiquetas()
        'General
        defaultoption = lstEtiquetas.Find(Function(p) p.CvePlataforma = 1 And p.CveMenu = 0 And p.CveEtiqueta = 1).NomEtiqueta
        Dim obligatorio As String = "<label class='control-label color-red'>*</label>"

        'Titulo
        Dim lstMenu As MenuModel = New Menu().FindById(ObjUser.CveRol, CInt(Plataforma), -1, menu)
        'PageTitulo.Text = "Perfil Nutricional - Reporte"

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
                        If Not LB_IMG Is Nothing Then
                            LB_IMG.Attributes("class") = a.Icono
                            LB_IMG.Style("font-size") = a.IconoSize + "!important"
                        End If
                        LB.ToolTip = a.ToolTip
                        If Not LB_LBL Is Nothing Then
                            LB_LBL.Text = a.NomAccion
                        End If
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
        'For Each a As Controles_CapturaModel In lstControles
        '    If a.CveEtapa = 0 Then
        '        Dim LBLG As Label = UPContenido.FindControl("LBLG" + a.CveControl.ToString)
        '        If Not LBLG Is Nothing Then LBLG.Text = a.Etiqueta.Replace("*", obligatorio)
        '    Else
        '        Dim LBL As Label = UPContenido.FindControl("LBLC" + a.CveControl.ToString)
        '        If Not LBL Is Nothing Then
        '            LBL.Text = a.Etiqueta.Replace("*", obligatorio)
        '            If a.ValidaRango <> "" Then
        '                Dim LBLH As Label = UPContenido.FindControl("LBLH" + a.CveControl.ToString)
        '                If Not LBLH Is Nothing Then LBLH.Text = "(" + New Parametros().FindById(CInt(Plataforma), CInt(a.ValidaRango)).Valor + ")"
        '            End If
        '        End If
        '    End If

        'Next

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

        Dim ObjM As OptimizerG_ProgramaAModel = New OptimizerG_ProgramaA().FindById(0, Convert.ToInt64(regPId.Text), "")
        LB22.Visible = If(ObjM Is Nothing, False, True)

    End Sub
    Sub LlenaRegistro()
        Try
            MostrarPerfil()
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

    Sub DescargarExcel()
        DescargarArchivoReporte("excel", 2, ConfigurationManager.AppSettings("WSOptimizerGallinas"))
    End Sub
    Sub DescargarPdf()
        DescargarArchivoReporte("pdf", 2, ConfigurationManager.AppSettings("WSOptimizerGallinas"))
    End Sub
    Private Sub DescargarArchivoReporte(formato As String, versionReporte As Integer, baseApiUrl As String)
        Try
            If regPId.Text = "0" Then Throw New Exception("No se encontró el identificador del perfil para generar el archivo.")
            OptimizerReporteDescarga.Descargar(Me, baseApiUrl, Convert.ToInt64(regPId.Text), formato, versionReporte, "PerfilNutricional")
        Catch ex As Exception
            Alertas("", CleanSpecialCharacter(ex.Message), False, 4)
        End Try
    End Sub
    Sub MostrarPerfil()
        Dim num_decimales As Integer = 3

        Try
            'Dim ObjR As ResponseModel = JsonConvert.DeserializeObject(Of ResponseModel)(New OptimizerP_PerfilN_Resultado().FindById(CInt(regPId.Text)).Response)
            'Dim lstE As List(Of OptimizerP_PerfilN_EtapasModel) = New OptimizerP_PerfilN_Etapas().FindlstAll(CodCliente.Text, Convert.ToInt64(regPId.Text))
            'Dim lstVariables As List(Of OptimizerP_CatVariablesModel) = New OptimizerP_CatVariables().FindlstAll(0)
            Dim ObjM As OptimizerG_PerfilNModel = New OptimizerG_PerfilN().FindById(Convert.ToInt64(regPId.Text), "")
            LBLReferencia.Text = "FOLIO: " + ObjM.FolioR + " | " + ObjM.NomReferencia
            LBLCliente.Text = ObjM.NomCliente


            Dim objPerfil As OptimizerG_PerfilNModel = New OptimizerG_PerfilN().FindById(Convert.ToInt64(regPId.Text), "")
            Dim modeloCaptura As List(Of PNCapturaModel) = New OptimizerG_PerfilN().ConstruirModeloCaptura(Convert.ToInt64(regPId.Text), objPerfil.CodCliente)
            Dim jsonCaptura = JsonConvert.SerializeObject(modeloCaptura)
            ClientScript.RegisterStartupScript(Me.GetType(), "initModelo", "var modeloCaptura = " & jsonCaptura & ";", True)


            Dim etapas = modeloCaptura.GroupBy(Function(x) x.Etapa).Select(
                Function(g) New With {.Etapa = g.Key, .NombreEtapa = g.First().NombreEtapa}).OrderBy(Function(x) x.Etapa).ToList()

            Dim categorias = modeloCaptura.FindAll(Function(p) p.ReporteInterno = "S").OrderBy(Function(x) x.CveCategoria).GroupBy(Function(x) x.CveCategoria).ToList()

            'Dim variables = modeloCaptura.GroupBy(Function(x) x.Variable).ToList()

            Dim w As String = (350 + (100 * etapas.Count)).ToString + "px"
            Dim sb As StringBuilder = New StringBuilder()
            sb.Append("<div style='overflow:auto; height: 65vh; width: 100%;'>")
            sb.Append("<table align='left' style='max-width:" + w + ";margin-top:10px;'cellpadding='1' border='0' class='datagrid333 table table-condensed  table-sm table-rep'>")
            'sb.Append("<table border='1' style='border-collapse:collapse;width:100%;'>")

            ' HEADER
            sb.Append("<thead>")

            sb.Append("<tr><th width='250px'></th>")
            For Each e In etapas
                sb.Append("<th >" & e.NombreEtapa & "</th>")
            Next
            sb.Append("</tr>")
            sb.Append("</thead><tbody>")

            For Each cat In categorias
                Dim nombreCategoria = cat.First().NomCategoria
                sb.Append("<tr style='background-color:#dce9f5!important;font-weight:bold;'>")
                sb.Append("<td colspan='" & (etapas.Count + 1) & "'>" & nombreCategoria & "</td>")
                sb.Append("</tr>")
                Dim variables = cat.GroupBy(Function(x) x.Variable).ToList()

                For Each grupo In variables
                    If grupo.First().ReporteInterno = "S" Then
                        sb.Append("<tr>")
                        sb.Append("<td>" & grupo.First().Descripcion & "</td>")

                        For Each e In etapas
                            Dim item = grupo.FirstOrDefault(Function(x) x.Etapa = e.Etapa)

                            If item IsNot Nothing Then
                                Dim valorTexto = Math.Round(item.Ajuste, item.Decimales).
                        ToString(System.Globalization.CultureInfo.InvariantCulture)

                                sb.Append("<td align='center'>" & If(item.Mostrar = "N", "", valorTexto) & "</td>")
                            Else
                                sb.Append("<td></td>")
                            End If
                        Next
                        sb.Append("</tr>")
                    End If
                Next
            Next

            'For Each grupo In variables
            '    If grupo.First().ReporteInterno = "S" Then
            '        sb.Append("<tr>")
            '        sb.Append("<td >" & grupo.First().Descripcion & "</td>")
            '        For Each e In etapas
            '            Dim item = grupo.FirstOrDefault(Function(x) x.Etapa = e.Etapa)
            '            If item IsNot Nothing Then
            '                Dim stepValue As String = If(item.Decimales <= 0, "1", "0." & New String("0"c, item.Decimales - 1) & "1")
            '                Dim valorTexto = Math.Round(item.Ajuste, item.Decimales).ToString(System.Globalization.CultureInfo.InvariantCulture)

            '                sb.Append("<td align='center'>" & If(item.Mostrar = "N", "", valorTexto) & "</td>")
            '            Else
            '                sb.Append("<td></td>")
            '            End If

            '        Next
            '        sb.Append("</tr>")
            '    End If

            'Next


            sb.Append("</tbody></table>")
            PerfilN.Text = sb.ToString

        Catch ex As Exception
            Alertas("", CleanSpecialCharacter(ex.Message), False, 4)
        End Try

    End Sub
Sub MostrarPrograma()
        Dim ObjM As OptimizerG_PerfilNModel = New OptimizerG_PerfilN().FindById(Convert.ToInt64(regPId.Text), "")
        Dim filtro As String = filtroview.Text
        Response.Redirect(New RedirectPaginas().FindById(Plataforma + "-62-1").PaginaURL.Replace("@Id", Codif(ObjM.CvePlan)).Replace("@CvePN", Codif(regPId.Text)).Replace("@filtro", Codif(filtro)).Replace("@pageIndex", gvindexpage.Text), True)
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


