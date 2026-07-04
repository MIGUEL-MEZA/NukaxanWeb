Imports System.Configuration
Imports System.Web.DynamicData
Imports Microsoft.Ajax.Utilities
Imports Newtonsoft.Json
Imports NukaxanWEB.Libreria

Public Class OptimizerC_PerfilN_Frm
    Inherits Page
    Private ObjUser As UsuarioModel
    Private Plataforma As String = "41"
    Private menu As String = "2"
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
        iList.Add(New DatosGrid() With {.Tipo = 2, .Campo = "DDLEtapaFlujo", .Valida = "S", .ValidaCeros = "N"})
        iList.Add(New DatosGrid() With {.Tipo = 1, .Campo = "TBNomEtapa", .Valida = "N", .ValidaCeros = "N"})
        iList.Add(New DatosGrid() With {.Tipo = 0, .Campo = "CodALLIX", .Valida = "N", .ValidaCeros = "N"})
        iList.Add(New DatosGrid() With {.Tipo = 0, .Campo = "Aplica", .Valida = "N", .ValidaCeros = "N"})
        iList.Add(New DatosGrid() With {.Tipo = 1, .Campo = "TBPesoIni", .Valida = "S", .ValidaCeros = "S"})
        iList.Add(New DatosGrid() With {.Tipo = 1, .Campo = "TBPesoFin", .Valida = "S", .ValidaCeros = "S"})
        iList.Add(New DatosGrid() With {.Tipo = 1, .Campo = "TBENAlimento", .Valida = "S", .ValidaCeros = "S"})
        iList.Add(New DatosGrid() With {.Tipo = 2, .Campo = "DDLAjuste", .Valida = "S", .ValidaCeros = "N"})

        RegistrarDescargaDirecta()

        If Not Page.IsPostBack Then
            DatosLoad()
        End If
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
    End Sub
    Sub Etiquetas()
        'General
        defaultoption = lstEtiquetas.Find(Function(p) p.CvePlataforma = 1 And p.CveMenu = 0 And p.CveEtiqueta = 1).NomEtiqueta
        Dim obligatorio As String = "<label class='control-label color-red'>*</label>"

        'Titulo
        Dim lstMenu As MenuModel = New Menu().FindById(ObjUser.CveRol, CInt(Plataforma), -1, menu)
        PageTitulo.Text = lstMenu.NomMenu

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
            Dim excepciones As String = "41-2-1,41-2-2"
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
    Sub LlenaDDL()
        Call New Catalogos().LlenaGeneralClientes(DDLCliente, Plataforma, ObjUser.CodUsuario)
        Call New Catalogos().LlenaOptimizer_Modalidad(DDLModalidad)
        DDLModalidad.SelectedValue = 2
        Call New Catalogos().LlenaOptimizer_Referencia(DDLReferencia)
    End Sub
    Protected Sub DDLModalidad_SelectedIndexChanged(sender As Object, e As EventArgs) Handles DDLModalidad.SelectedIndexChanged
        SeguridadLoad()
    End Sub
    Sub Acciones(op As Boolean, op2 As Boolean, arrAction As String)
        Dim lb As New LinkButton
        Dim arr2() As String = arrAction.Split(",")
        Dim arr(6) As String
        arr(0) = "LB2"
        arr(1) = "LB11"
        arr(2) = "LB7"
        arr(3) = "LB17"
        arr(4) = "LB18"
        arr(5) = "LB15"
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

        'IsAdm = False
        'IsAutor = False

        Acciones(False, False, "0,1,2,3,4,5")
        Controles(False)

        Dim lstc As New List(Of FrmControlModel)
        lstc.Add(New FrmControlModel With {.Nombre = "chk", .Tipo = 0})
        lstc.Add(New FrmControlModel With {.Nombre = "DDLEtapaFlujo", .Tipo = 3})
        lstc.Add(New FrmControlModel With {.Nombre = "TBNomEtapa", .Tipo = 2})
        lstc.Add(New FrmControlModel With {.Nombre = "TBPesoIni", .Tipo = 2})
        lstc.Add(New FrmControlModel With {.Nombre = "TBPesoFin", .Tipo = 2})
        lstc.Add(New FrmControlModel With {.Nombre = "TBENAlimento", .Tipo = 2})
        lstc.Add(New FrmControlModel With {.Nombre = "DDLAjuste", .Tipo = 3})

        'iList.Add(New DatosGrid() With {.Tipo = 0, .Campo = "LBLGD_PReferencia", .Valida = "N", .ValidaCeros = "N"})
        'iList.Add(New DatosGrid() With {.Tipo = 3, .Campo = "DDLAjuste", .Valida = "S", .ValidaCeros = "N"})
        'iList.Add(New DatosGrid() With {.Tipo = 0, .Campo = "LBLGD_PAjustada", .Valida = "N", .ValidaCeros = "N"})

        If regPId.Text = "0" Then 'Nuevo
            Acciones(True, True, "0,1")
            Controles(True)
            DDLCliente.Visible = True
            TBNomClienteD.Visible = False
            SeguridadRPT(True, rptEtapas, lstc)
        ElseIf regPId.Text <> "0" And IsEstatus And (IsAutor Or IsAdm) Then
            Acciones(True, True, "0,1,4,5")
            Controles(True)
            DDLCliente.Visible = False
            TBNomClienteD.Visible = True
            SeguridadRPT(True, rptEtapas, lstc)
            LB18.Visible = If(CodALLIX.Text <> "" And DDLModalidad.SelectedValue.ToString = "1", True, False)
        ElseIf regPId.Text <> "0" And Not IsEstatus And (IsAutor Or IsAdm) Then
            Acciones(True, True, "0,5")
            Controles(False)
            SeguridadRPT(False, rptEtapas, lstc)
            LB15.Visible = If(CvePlan.Text <> "0", True, False)
        Else
            Acciones(True, True, "0,5")
            Controles(False)
            SeguridadRPT(False, rptEtapas, lstc)
            LB15.Visible = If(CvePlan.Text <> "0", True, False)
        End If
    End Sub
    Sub LlenaRegistro()
        Try
            If regPId.Text = "0" Then
                CvePlan.Text = "0"
                CveEstatus.Text = "1"
                CodCliente.Text = ""
                CodALLIX.Text = ""
                TBID.Text = "POR ASIGNAR"
                TBNomEstatusD.Text = "EN EDICIÓN"
                TBFecAltaD.Text = Now.ToString("dd/MM/yyyy") + " | " + ObjUser.NomUsuario
                TBFecActD.Text = Now.ToString("dd/MM/yyyy") + " | " + ObjUser.NomUsuario
                LlenaRPT_Etapas()
            Else
                Dim ObjM As OptimizerC_PerfilNModel = New OptimizerC_PerfilN().FindById(Convert.ToInt64(regPId.Text), "")
                Autor.Text = ObjM.UsuAlta
                CvePlan.Text = ObjM.CvePlan.ToString
                CveEstatus.Text = ObjM.CveEstatus.ToString
                CodCliente.Text = ObjM.CodCliente
                TBNomClienteD.Text = ObjM.NomCliente
                Dim excepciones As String = ""
                lstControles.FindAll(Function(p) New ArrayList(excepciones.Split(",")).IndexOf(p.CveMSC.ToString) < 0) _
                    .ForEach(Sub(c)
                                 If c.CveTipo = 1 Then CType(UPContenido.FindControl(c.Control), Label).Text = ObjM.GetType.GetProperty(c.Campo).GetValue(ObjM).ToString
                                 If c.CveTipo = 2 Then CType(UPContenido.FindControl(c.Control), TextBox).Text = ObjM.GetType.GetProperty(c.Campo).GetValue(ObjM).ToString
                                 If c.CveTipo = 3 Then CType(UPContenido.FindControl(c.Control), DropDownList).SelectedValue = If(ObjM.GetType.GetProperty(c.Campo).GetValue(ObjM).ToString = "0", "", ObjM.GetType.GetProperty(c.Campo).GetValue(ObjM))
                                 If c.CveTipo = 4 Then CType(UPContenido.FindControl(c.Control), AjaxControlToolkit.ComboBox).SelectedValue = If(ObjM.GetType.GetProperty(c.Campo).GetValue(ObjM).ToString = "0", "", ObjM.GetType.GetProperty(c.Campo).GetValue(ObjM))
                             End Sub)
                TBFecAltaD.Text = ObjM.FecAlta + " | " + ObjM.NomUsuAlta
                TBFecActD.Text = ObjM.FecAct + " | " + ObjM.NomUsuAct
                CodALLIX.Text = New Clientes().FindById(CodCliente.Text).CodALLIX
                LlenaRPT_Etapas()
                MostrarPerfil()
            End If
            SeguridadLoad()

        Catch ex As Exception
            Alertas("", CleanSpecialCharacter(ex.Message), False, 4)
        End Try

    End Sub
    Sub LlenaRPT_Etapas()
        Try
            rptEtapas.DataSource = New OptimizerC_PerfilN_Etapas().FindAll(CodCliente.Text, CInt(regPId.Text))
            rptEtapas.DataBind()
        Catch ex As Exception
            rptEtapas.DataSource = Nothing
            rptEtapas.DataBind()
            Alertas("", CleanSpecialCharacter(ex.Message), False, 4)
        End Try
    End Sub
    Protected Sub RPTOnItemDataBound(ByVal sender As Object, ByVal e As RepeaterItemEventArgs) Handles rptEtapas.ItemDataBound
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim item As RepeaterItem = e.Item
            Dim chk As CheckBox = (TryCast(item.FindControl("chk"), CheckBox))
            Dim CveEtapa As Integer = CInt((TryCast(item.FindControl("CveEtapa"), Label).Text))
            Dim Aplica As String = (TryCast(item.FindControl("Aplica"), Label)).Text
            Dim CveEtapaFlujo As Integer = CInt((TryCast(item.FindControl("CveEtapaFlujo"), Label).Text))
            Dim DDLEtapaFlujo As DropDownList = (TryCast(item.FindControl("DDLEtapaFlujo"), DropDownList))
            Dim TBNomEtapa As TextBox = (TryCast(item.FindControl("TBNomEtapa"), TextBox))
            Dim TBPesoIni As TextBox = (TryCast(item.FindControl("TBPesoIni"), TextBox))
            Dim TBPesoFin As TextBox = (TryCast(item.FindControl("TBPesoFin"), TextBox))
            Dim TBENAlimento As TextBox = (TryCast(item.FindControl("TBENAlimento"), TextBox))
            Dim DDLAjuste As DropDownList = (TryCast(item.FindControl("DDLAjuste"), DropDownList))
            Dim TBAjuste As String = (TryCast(item.FindControl("TBAjuste"), Label).Text)

            Call New Catalogos().LlenaOptimizer_Etapas(DDLEtapaFlujo)
            DDLEtapaFlujo.SelectedValue = CveEtapaFlujo
            Call New Catalogos().LlenaAjusteGDP(DDLAjuste)
            DDLAjuste.SelectedValue = TBAjuste

            chk.Checked = If(Aplica = "S", True, False)
            DDLEtapaFlujo.Enabled = chk.Checked
            TBNomEtapa.Enabled = chk.Checked
            TBPesoIni.Enabled = chk.Checked
            TBPesoFin.Enabled = chk.Checked
            TBENAlimento.Enabled = chk.Checked
            DDLAjuste.Enabled = chk.Checked

            Dim LBLGD_PReferencia As Label = (TryCast(item.FindControl("LBLGD_PReferencia"), Label))
            Dim LBLGD_PAjustada As Label = (TryCast(item.FindControl("LBLGD_PAjustada"), Label))

            If chk.Checked Then
                Dim ObjR As ResponseModel = JsonConvert.DeserializeObject(Of ResponseModel)(New OptimizerC_PerfilN_Resultado().FindById(CInt(regPId.Text)).Response)

                Dim resultado = ObjR.Variables.Where(Function(v) v.NoVariable = 4).SelectMany(Function(v) v.Etapas) _
                .Where(Function(et) et.Clave = CveEtapa).Select(Function(et) New With {.Valor = et.Valor, .ValorReferencia = et.ValorReferencia}) _
                .FirstOrDefault()
                If resultado IsNot Nothing And regPId.Text <> "0" Then
                    LBLGD_PReferencia.Text = Math.Round(resultado.ValorReferencia, 2)
                    LBLGD_PAjustada.Text = Math.Round(resultado.Valor, 2)
                Else
                    LBLGD_PReferencia.Text = ""
                    LBLGD_PAjustada.Text = ""
                End If


            End If



        End If
    End Sub

    '--Acciones---
    Sub Regresar()
        Response.Redirect(New RedirectPaginas().FindById(Plataforma + "-" + menu + "-0").PaginaURL.Replace("@filtro", Codif(filtroview.Text)).Replace("@pageIndex", gvindexpage.Text), True)
    End Sub
    Sub Refrescar()
        Response.Redirect(New RedirectPaginas().FindById(Plataforma + "-" + menu + "-1").PaginaURL.Replace("@Id", Codif(regPId.Text)).Replace("@filtro", Codif(filtroview.Text)).Replace("@pageIndex", gvindexpage.Text), True)
    End Sub
    Sub DescargarExcel()
        DescargarArchivoReporte("excel", 1, ConfigurationManager.AppSettings("WSOptimizer"))
    End Sub

    Sub DescargarPdf()
        DescargarArchivoReporte("pdf", 1, ConfigurationManager.AppSettings("WSOptimizer"))
    End Sub

    Private Sub DescargarArchivoReporte(formato As String, versionReporte As Integer, baseApiUrl As String)
        Try
            If regPId.Text = "0" Then Throw New Exception("Debes guardar el perfil antes de generar el archivo.")
            OptimizerReporteDescarga.Descargar(Me, baseApiUrl, Convert.ToInt64(regPId.Text), formato, versionReporte, "PerfilNutricional")
        Catch ex As Exception
            Alertas("", CleanSpecialCharacter(ex.Message), False, 4)
        End Try
    End Sub
    Function Valida() As Boolean
        Dim IsResult As Boolean = False
        Dim lst As Controles_CapturaModel
        Try
            Dim excepciones As String = ""
            lst = lstControles.FindAll(Function(c) c.Editable = "S" And c.Obligatorio = "S" And New ArrayList(excepciones.Split(",")).IndexOf(c.CveMSC.ToString) < 0 And c.Valor = "").FirstOrDefault
            If Not lst Is Nothing Then msg = lst.Mensaje
            'Valida Rangos
            If msg = "" Then
                lstControles.Where(Function(p) p.ValidaRango <> "").ToList _
                    .ForEach(Sub(c)
                                 If msg <> "" Then Exit Sub
                                 Dim tmp() As String = New Parametros().FindById(CInt(Plataforma), CInt(c.ValidaRango)).Valor.Trim.Split("-")
                                 'c.ValidaRango.Trim.Split("-")
                                 If Not (CDbl(c.Valor) >= CDbl(tmp(0)) And CDbl(c.Valor) <= CDbl(tmp(1))) Then msg = New Mensajes().FindById("0", 0, 38).NomMensaje.Replace("#valor", c.Etiqueta.Replace(": *", ""))
                                 If c.CveMSC = "41-2-15" And c.Valor = "0" Then msg = ""
                             End Sub)
            End If
            'If msg = "" And regPId.Text = "0" Then
            '    Try
            '        Dim ObjM As OptimizerC_PerfilNModel = New OptimizerC_PerfilN().FindById(0, DDLCliente.SelectedValue + "|1")
            '        If Not ObjM Is Nothing Then msg = New Mensajes().FindById("0", 0, 54).NomMensaje
            '    Catch ex As Exception

            '    End Try
            'End If
            'Valida ppm raptopamina
            'If rptEtapas.Items.Cast(Of RepeaterItem)().Where(Function(p) TryCast(p.FindControl("CveEtapa"), Label).Text = "6" _
            'And TryCast(p.FindControl("chk"), CheckBox).Checked).Count > 0 And TBRAC.Text = "0" Then
            '    msg = "Debes capturar un valor de ppm ractopamina dentro del rango."
            'ElseIf rptEtapas.Items.Cast(Of RepeaterItem)().Where(Function(p) TryCast(p.FindControl("CveEtapa"), Label).Text = "6" _
            'And TryCast(p.FindControl("chk"), CheckBox).Checked = False).Count > 0 And TBRAC.Text <> "0" Then
            '    msg = "No puedes tener un valor de ppm ractopamina ya que no tienes activa la etapa de finalizador ractopamina."
            'End If

            'Valida Grid Captura
            If msg = "" Then
                If ValidRPTChkCaptura(rptEtapas, "chk", iList) Then msg = New Mensajes().FindById("0", 0, 39).NomMensaje
            End If

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
            LlenaControles()
            If Valida() = False Then Return False

            Dim Valores As String = String.Join("|", (lstControles.FindAll(Function(p) p.Editable = "S").Select(Function(a) a.Valor).ToList()))
            Valores += "|1"
            Dim valoresE As String = GetRPTChkCapturaAll(rptEtapas, "chk", iList)

            Dim ObjM As New OptimizerC_PerfilN()

            IsResult = ObjM.SaveModel(Convert.ToInt64(regPId.Text), Valores, valoresE, ObjUser.CodUsuario)
            If IsResult Then
                If regPId.Text = "0" Then
                    regPId.Text = ObjM.Folio
                    'mpe_notifications(lstMensajes.Find(Function(p) p.CveMensaje = 1).NomMensaje.Replace("@Id", New Anomalia().FindById(New List(Of String)({regPId.Text})).FolioR), 0)
                End If
            Else
                Alertas("", CleanSpecialCharacter(ObjM.strError), False, 4)
            End If
        Catch ex As Exception
            Alertas("", CleanSpecialCharacter(ex.Message), False, 4)
        End Try
        Return IsResult
    End Function
    Sub Guardar()
        Dim IsResult As Boolean
        Dim lst As New List(Of WSPerfilN_RequestEtapasModel)
        Try
            IsResult = GuardaDatos()
            If IsResult Then
                Generar()
            End If
        Catch ex As Exception
            Alertas("", CleanSpecialCharacter(ex.Message), False, 4)
        End Try

    End Sub
    Async Sub Generar()
        Dim IsResult As Boolean
        Dim lst As New List(Of WSPerfilN_RequestEtapasModel)
        Try
            If DDLReferencia.SelectedValue.ToString = "" Then Exit Sub
            rptEtapas.Items.Cast(Of RepeaterItem)().ToList.ForEach(Sub(r)
                                                                       Dim chk As CheckBox = TryCast(r.FindControl("chk"), CheckBox)
                                                                       Dim CveEtapa As Integer = CInt(TryCast(r.FindControl("CveEtapa"), Label).Text)
                                                                       Dim TBPesoIni As Double = CDbl(TryCast(r.FindControl("TBPesoIni"), TextBox).Text)
                                                                       Dim TBPesoFin As Double = CDbl(TryCast(r.FindControl("TBPesoFin"), TextBox).Text)
                                                                       Dim TBENAlimento As Double = CDbl(TryCast(r.FindControl("TBENAlimento"), TextBox).Text)
                                                                       Dim IsRAC As Boolean = If(TryCast(r.FindControl("IsRAC"), Label).Text = "S", True, False)
                                                                       Dim PorcGDP As Integer = CInt(TryCast(r.FindControl("DDLAjuste"), DropDownList).SelectedValue)
                                                                       If chk.Checked Then lst.Add(New WSPerfilN_RequestEtapasModel With {.Clave = CveEtapa, .PesoInicial = TBPesoIni, .PesoFinal = TBPesoFin, .ENAlimento = TBENAlimento, .PorcGDP = PorcGDP, .IsRactopamina = IsRAC})
                                                                   End Sub)

            Dim req As New WSPerfilN_RequestModel
            req.CvePerfilN = CInt(regPId.Text)
            req.UsuAct = ObjUser.CodUsuario
            req.Referencia = DDLReferencia.SelectedValue
            req.Temperatura = CDbl(TBTemperatura.Text)
            If CDbl(TBTemperatura.Text) < 17 Then req.Temperatura = 17
            If CDbl(TBTemperatura.Text) > 29 Then req.Temperatura = 29
            req.Espacio = CDbl(TBEspacio.Text)
            req.PPMRAC = CDbl(TBRAC.Text)
            req.EtapasModel = lst
            Dim Obj As New Interfaz_Optimizer()
            Dim ObjR As ResponseModel = Await Obj.GeneraPerfil(req)
            If Obj.WSEstatus Then
                MostrarPerfil()
                Alertas("", "33", True, 2)
            Else
                Alertas("", "34", True, 3)
            End If
        Catch ex As Exception
            Alertas("", CleanSpecialCharacter(ex.Message), False, 4)
        End Try

    End Sub
    Async Sub Enviar()
        Dim Isresult As Boolean
        Dim formulasok As String = ""
        Try
            Dim ObjR As OptimizerC_PerfilN_ResultadoModel = New OptimizerC_PerfilN_Resultado().FindById(CInt(regPId.Text))
            Dim ObjR2 As ResponseModel = JsonConvert.DeserializeObject(Of ResponseModel)(ObjR.Response)
            'Dim ObjR2 As List(Of ResponseDataModel) = (JsonConvert.DeserializeObject(Of ResponseModel)(ObjR.Response)).Variables.OrderBy(Function(p) p.Posicion).ToList
            'Etapas
            Dim lstE As List(Of OptimizerC_PerfilN_EtapasModel) = New OptimizerC_PerfilN_Etapas().FindlstAll(CodCliente.Text, CInt(regPId.Text))
            Dim lstVN As List(Of PerfilN_VariablesModel) = New PerfilN_Variables().FindlstAll("N")
            Dim lstVR As List(Of PerfilN_VariablesModel) = New PerfilN_Variables().FindlstAll("R")

            Dim tmptoken As WSTokenModel = Await New Interfaz_Nufeed().GetToken()

            lstE.Where(Function(p) p.Aplica = "S" And p.CodALLIX <> "").ToList() _
                .ForEach(Async Sub(p)
                             Dim lstRN As New List(Of WSPerfilN_NutrientesModel)
                             Dim lstRR As New List(Of WSPerfilN_RelNutrientesModel)
                             lstVN.ForEach(Sub(c)
                                               ObjR2.Variables.Where(Function(c2) c2.NoVariable = c.CveVariable) _
                                               .OrderBy(Function(o) o.NoVariable).ToList _
                                               .ForEach(Sub(c3)
                                                            Dim tmp As Double = 0
                                                            tmp = c3.Etapas.Find(Function(x) x.Clave = p.CveEtapa).Valor
                                                            If c.CodALLIX = "9448" Or c.CodALLIX = "9449" Then
                                                                lstRN.Add(New WSPerfilN_NutrientesModel With {.nutrientcode = c.CodALLIX, .minvalue = Math.Round(tmp / 10000, 6), .maxvalue = Nothing})
                                                            ElseIf c.CodALLIX = "9450" Or c.CodALLIX = "0750" Then
                                                                lstRN.Add(New WSPerfilN_NutrientesModel With {.nutrientcode = c.CodALLIX, .minvalue = Math.Round(tmp, 2), .maxvalue = Math.Round(tmp, 2)})
                                                            Else
                                                                lstRN.Add(New WSPerfilN_NutrientesModel With {.nutrientcode = c.CodALLIX, .minvalue = Math.Round(tmp, 2), .maxvalue = Nothing})
                                                            End If
                                                        End Sub)
                                           End Sub)
                             lstVR.ForEach(Sub(c)
                                               ObjR2.Variables.Where(Function(c2) c2.NoVariable = c.CveVariable) _
                                               .OrderBy(Function(o) o.NoVariable).ToList _
                                               .ForEach(Sub(c3)
                                                            Dim tmp As Double = 0
                                                            tmp = c3.Etapas.Find(Function(x) x.Clave = p.CveEtapa).Valor
                                                            'lstRR.Add(New WSPerfilN_RelNutrientesModel With {.nutrientRatioCode = c.CodALLIX, .minValue = Math.Round(tmp, 2), .maxValue = Nothing})

                                                            If c.CodALLIX = "9448" Or c.CodALLIX = "9449" Then
                                                                lstRR.Add(New WSPerfilN_RelNutrientesModel With {.nutrientRatioCode = c.CodALLIX, .minValue = Math.Round(tmp / 10000, 6), .maxValue = Nothing})
                                                            ElseIf c.CodALLIX = "9450" Or c.CodALLIX = "0750" Then
                                                                lstRR.Add(New WSPerfilN_RelNutrientesModel With {.nutrientRatioCode = c.CodALLIX, .minValue = Math.Round(tmp, 2), .maxValue = Math.Round(tmp, 2)})
                                                            Else
                                                                lstRR.Add(New WSPerfilN_RelNutrientesModel With {.nutrientRatioCode = c.CodALLIX, .minValue = Math.Round(tmp, 2), .maxValue = Nothing})
                                                            End If

                                                        End Sub)
                                           End Sub)
                             Try
                                 Dim req As New WSPerfilN_NutrientesRequestModel
                                 req.nutrients = lstRN
                                 req.nutrientRatios = lstRR
                                 Dim Obj As New Interfaz_Nufeed()
                                 Isresult = Await Obj.UpdateNutrientes(tmptoken, New Clientes().FindById(CodCliente.Text).CodALLIX, p.CodALLIX, req)
                                 If Isresult Then formulasok += p.NomEtapa + "|"
                             Catch ex As Exception
                                 Alertas("", CleanSpecialCharacter(ex.Message), False, 4)
                             End Try

                         End Sub)
            Alertas("", "35", False, 2)
        Catch ex As Exception
            Alertas("", CleanSpecialCharacter(ex.Message), False, 4)
        End Try
    End Sub
    Sub MostrarPrograma()
        Dim filtro As String = filtroview.Text
        Response.Redirect(New RedirectPaginas().FindById(Plataforma + "-3-1").PaginaURL.Replace("@Id", Codif(CvePlan.Text)).Replace("@CvePN", Codif(regPId.Text)).Replace("@filtro", Codif(filtro)).Replace("@pageIndex", gvindexpage.Text), True)
    End Sub
    Sub MostrarPerfil()
        Try
            Dim objPerfil As OptimizerC_PerfilNModel = New OptimizerC_PerfilN().FindById(Convert.ToInt64(regPId.Text), "")
            Dim modeloCaptura As List(Of OptimizerC_PerfilN.PNCapturaModel) = New OptimizerC_PerfilN().ConstruirModeloCaptura(Convert.ToInt64(regPId.Text), objPerfil.CodCliente)
            Dim jsonCaptura = JsonConvert.SerializeObject(modeloCaptura)
            ClientScript.RegisterStartupScript(Me.GetType(), "initModeloCerdos", "var modeloCaptura = " & jsonCaptura & ";", True)

            Dim etapas = modeloCaptura.GroupBy(Function(x) x.Etapa).Select(Function(g) New With {.Etapa = g.Key, .NombreEtapa = g.First().NombreEtapa}).OrderBy(Function(x) x.Etapa).ToList()
            Dim categorias = modeloCaptura.OrderBy(Function(x) x.CveCategoria).ThenBy(Function(x) x.Variable).GroupBy(Function(x) x.CveCategoria).ToList()
            Dim w As String = (350 + (170 * Math.Max(etapas.Count, 1))).ToString() + "px"
            Dim sb As New StringBuilder()

            sb.Append("<style>")
            sb.Append("thead tr:nth-child(1) th {height:35px;position:sticky;top:0;background:#9aa3b2;z-index:3;}")
            sb.Append("th,td{border:1px solid #ccc;padding:5px;}")
            sb.Append("</style>")
            sb.Append("<div style='overflow:auto; height:57vh; width:100%;'>")
            sb.Append("<table align='left' style='min-width:" & w & ";margin-top:10px;' cellpadding='1' border='0' class='datagrid333 table table-condensed table-sm table-rep'>")
            sb.Append("<thead><tr><th width='350px'></th>")
            For Each etapa In etapas
                sb.Append("<th align='center' width='170px'><label style='font-weight:bold!important;'>" & etapa.NombreEtapa & "</label></th>")
            Next
            sb.Append("</tr></thead><tbody>")

            For Each categoria In categorias
                Dim grupoVariables = categoria.GroupBy(Function(x) x.Variable).ToList()
                Dim nombreCategoria = grupoVariables.First().First().NomCategoria

                If Not String.IsNullOrWhiteSpace(nombreCategoria) Then
                    sb.Append("<tr style='background-color:#dce9f5!important;font-weight:bold;'>")
                    sb.Append("<td colspan='" & (etapas.Count + 1).ToString() & "'>" & nombreCategoria & "</td>")
                    sb.Append("</tr>")
                End If

                For Each grupo In grupoVariables
                    sb.Append("<tr>")
                    sb.Append("<td><label style='font-weight:bold!important;'>" & grupo.First().Descripcion & "</label></td>")
                    For Each etapa In etapas
                        Dim item = grupo.FirstOrDefault(Function(x) x.Etapa = etapa.Etapa)
                        If item IsNot Nothing Then
                            Dim valorTexto = Math.Round(item.Ajuste, item.Decimales).ToString(System.Globalization.CultureInfo.InvariantCulture)
                            sb.Append("<td align='center'>" & If(item.Mostrar = "N", "", valorTexto) & "</td>")
                        Else
                            sb.Append("<td></td>")
                        End If
                    Next
                    sb.Append("</tr>")
                Next
            Next

            sb.Append("</tbody></table></div>")
            PerfilN.Text = sb.ToString()
        Catch ex As Exception
            Alertas("", CleanSpecialCharacter(ex.Message), False, 4)
        End Try
    End Sub
    Sub MostrarPerfil2(res As ResponseModel, req As WSPerfilN_RequestModel)
        Try
            Dim style_titulo As String = "style='font-weight: bold!important;'"
            Dim style_valor As String = "" '"style='font-family: Arial; font-size: 12px; font-weight: normal;color:#000000;'"
            Dim sb As StringBuilder = New StringBuilder()
            Dim num_decimales As Integer = 3

            Dim arr = New String(1, 1) {{"TBNomEtapa", "1"}, {"CveEtapa", "0"}}
            Dim lstEtapas As List(Of String) = RPTCaptura(arr)

            Dim w As String = (350 + (170 * lstEtapas.Count)).ToString + "px"
            w = "900px"
            sb.Append("<table align='left' style='width:" + w + ";margin-top:10px;'cellpadding='3' border='1' class='table table-condensed  table-sm '>")
            sb.Append("<thead><tr >")
            sb.Append("<th align='left' width='350px' ><label " + style_titulo + ">&nbsp;</label></th>")


            For Each item In lstEtapas
                sb.Append("<th align='center' width='170px' ><label " + style_titulo + ">" + item.Split("-")(0) + "</label></th>")
            Next

            sb.Append(" </thead></tr>")
            res.Variables.FindAll(Function(p) p.MostrarCliente = "S").OrderBy(Function(p) p.NoVariable).ToList.ForEach(Sub(p)
                                                                                                                           sb.Append("<tr >")
                                                                                                                           sb.Append("<td align='left' ><label " + style_titulo + ">" + p.Variable + "</label></td>")
                                                                                                                           req.EtapasModel.ForEach(Sub(e)
                                                                                                                                                       If p.Etapas.Find(Function(x) x.Clave = e.Clave) Is Nothing Then
                                                                                                                                                           sb.Append("<td align='center' ><label " + style_valor + ">" + "&nbsp;" + "</label></td>")
                                                                                                                                                       Else
                                                                                                                                                           sb.Append("<td align='center' ><label " + style_valor + ">" + p.Etapas.Find(Function(x) x.Clave = e.Clave).Valor.ToString("N2") + "</label></td>")
                                                                                                                                                       End If

                                                                                                                                                   End Sub
                                                                                        )
                                                                                                                       End Sub)
            sb.Append("</table>")

            PerfilN.Text = sb.ToString
            'LB4.Style("display") = "none"
        Catch ex As Exception

        End Try

    End Sub
    Function RPTCaptura(campos(,) As String) As List(Of String)
        Dim lstEtapas As New List(Of String)
        Try
            rptEtapas.Items.Cast(Of RepeaterItem)().ToList.ForEach(Sub(p)
                                                                       Dim chk As CheckBox = TryCast(p.FindControl("chk"), CheckBox)
                                                                       Dim tmp As String = ""
                                                                       For i = 0 To UBound(campos, 1)
                                                                           If campos(i, 1) = "0" Then tmp += TryCast(p.FindControl(campos(i, 0)), Label).Text + "-"
                                                                           If campos(i, 1) = "1" Then tmp += TryCast(p.FindControl(campos(i, 0)), TextBox).Text + "-"
                                                                       Next
                                                                       tmp = If(tmp <> "", Left(tmp, Len(tmp) - 1), "")
                                                                       If chk.Checked Then lstEtapas.Add(tmp)
                                                                   End Sub)
        Catch ex As Exception
            Alertas("", CleanSpecialCharacter(ex.Message), 1, 4)
        End Try
        Return lstEtapas
    End Function

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
        Dim DDLEtapaFlujo As DropDownList = (TryCast(item.FindControl("DDLEtapaFlujo"), DropDownList))
        Dim TBNomEtapa As TextBox = TryCast(item.FindControl("TBNomEtapa"), TextBox)
        Dim TBPesoIni As TextBox = TryCast(item.FindControl("TBPesoIni"), TextBox)
        Dim TBPesoFin As TextBox = TryCast(item.FindControl("TBPesoFin"), TextBox)
        Dim TBENAlimento As TextBox = TryCast(item.FindControl("TBENAlimento"), TextBox)
        Dim DDLAjuste As DropDownList = (TryCast(item.FindControl("DDLAjuste"), DropDownList))

        DDLEtapaFlujo.Enabled = chk.Checked
        TBNomEtapa.Enabled = chk.Checked
        TBPesoIni.Enabled = chk.Checked
        TBPesoFin.Enabled = chk.Checked
        TBENAlimento.Enabled = chk.Checked
        DDLAjuste.Enabled = chk.Checked

        'TBNomEtapa.Text = "0"
        TBPesoIni.Text = "0"
        TBPesoFin.Text = "0"
        TBENAlimento.Text = "0"
        DDLAjuste.SelectedValue = "0"

    End Sub
    Public Overrides Sub VerifyRenderingInServerForm(control As Control)
        ' Verifies that the control is rendered
    End Sub
End Class


