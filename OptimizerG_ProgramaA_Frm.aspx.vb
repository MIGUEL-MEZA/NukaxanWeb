Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Runtime.Remoting
Imports System.Web.DynamicData
Imports Microsoft.Ajax.Utilities
Imports Newtonsoft.Json
Imports NukaxanWEB.Libreria
Imports NukaxanWEB.OptimizerG_ProgramaA

Public Class OptimizerG_ProgramaA_Frm
    Inherits Page
    Private ObjUser As UsuarioModel
    Private Plataforma As String = "43"
    Private menu As String = "3"
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
        PageTitulo.Text = lstMenu.NomMenu

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
    Sub Controles(op As Boolean)
        Try
            Dim excepciones As String = "43-3-1,43-3-2,43-3-3"
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
        Call New Catalogos().LlenaOptimizerG_Referencia(DDLReferencia)
        'Call New Catalogos().LlenaOptimizerP_Parametros(DDLParametro)
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
        Controles(False)

        'IsAdm = False
        'IsAutor = False

        Dim lstc As New List(Of FrmControlModel)
        lstc.Add(New FrmControlModel With {.Nombre = "chk", .Tipo = 0})
        lstc.Add(New FrmControlModel With {.Nombre = "TBCosto", .Tipo = 2})
        lstc.Add(New FrmControlModel With {.Nombre = "TBEM", .Tipo = 2})
        lstc.Add(New FrmControlModel With {.Nombre = "TBDuracionMin", .Tipo = 2})
        lstc.Add(New FrmControlModel With {.Nombre = "TBDuracionMax", .Tipo = 2})
        lstc.Add(New FrmControlModel With {.Nombre = "TBSIDLYS", .Tipo = 2})

        If regPId.Text = "0" Then 'Nuevo
            Acciones(True, True, "0,1")
            Controles(True)
            DDLCliente.Visible = False
            TBNomClienteD.Visible = True
            DDLModalidad.Visible = False
            TBNomModalidadD.Visible = True
            'SeguridadRPT(True, rptEtapas, lstc)
        ElseIf regPId.Text <> "0" And IsEstatus And (IsAutor Or IsAdm) Then
            Acciones(True, True, "0,1,2,3")
            Controles(True)
            DDLCliente.Visible = False
            TBNomClienteD.Visible = True
            DDLModalidad.Visible = False
            TBNomModalidadD.Visible = True
            'SeguridadRPT(True, rptEtapas, lstc)
            LB14.Visible = If(CodALLIXCte.Text <> "" And DDLModalidad.SelectedValue.ToString = "1", True, False)
        ElseIf regPId.Text <> "0" And Not IsEstatus And (IsAutor Or IsAdm) Then
            Acciones(True, True, "0,3")
            Controles(False)
            SeguridadRPT(False, rptEtapas, lstc)
        Else
            Acciones(True, True, "0,3")
            Controles(False)
            'SeguridadRPT(False, rptEtapas, lstc)
        End If

    End Sub
    Sub LlenaRegistro()
        Try
            If regPId.Text = "0" Then
                'CvePerfilN.Text = "0"
                CveEstatus.Text = "1"
                CodALLIXCte.Text = ""
                TBID.Text = "POR ASIGNAR"
                TBPNID.Text = CvePerfilN.Text
                TBNomEstatusD.Text = "EN EDICIÓN"
                TBFecAltaD.Text = Now.ToString("dd/MM/yyyy") + " | " + ObjUser.NomUsuario
                TBFecActD.Text = Now.ToString("dd/MM/yyyy") + " | " + ObjUser.NomUsuario

                Dim ObjPN As OptimizerG_PerfilNModel = New OptimizerG_PerfilN().FindById(Convert.ToInt64(CvePerfilN.Text), "")
                TBTitulo.Text = ObjPN.Titulo
                DDLCliente.SelectedValue = ObjPN.CodCliente
                TBNomClienteD.Text = ObjPN.NomCliente
                CodCliente.Text = ""
                DDLModalidad.SelectedValue = ObjPN.CveModalidad
                TBNomModalidadD.Text = ObjPN.NomModalidad
                DDLReferencia.SelectedValue = ObjPN.CveReferencia

            Else
                Dim ObjM As OptimizerG_ProgramaAModel = New OptimizerG_ProgramaA().FindById(Convert.ToInt64(regPId.Text), 0, "")
                Autor.Text = ObjM.UsuAlta
                CvePerfilN.Text = ObjM.CvePerfilN.ToString
                CveEstatus.Text = ObjM.CveEstatus.ToString
                CodCliente.Text = ObjM.CodCliente
                Dim excepciones As String = ""
                lstControles.FindAll(Function(p) p.CveEtapa > 0 And New ArrayList(excepciones.Split(",")).IndexOf(p.CveMSC.ToString) < 0) _
                    .ForEach(Sub(c)
                                 If c.CveTipo = 1 Then CType(UPContenido.FindControl(c.Control), Label).Text = ObjM.GetType.GetProperty(c.Campo).GetValue(ObjM).ToString
                                 If c.CveTipo = 2 Then CType(UPContenido.FindControl(c.Control), TextBox).Text = ObjM.GetType.GetProperty(c.Campo).GetValue(ObjM).ToString
                                 If c.CveTipo = 3 Then CType(UPContenido.FindControl(c.Control), DropDownList).SelectedValue = If(ObjM.GetType.GetProperty(c.Campo).GetValue(ObjM).ToString = "0", "", ObjM.GetType.GetProperty(c.Campo).GetValue(ObjM))
                                 If c.CveTipo = 4 Then CType(UPContenido.FindControl(c.Control), AjaxControlToolkit.ComboBox).SelectedValue = If(ObjM.GetType.GetProperty(c.Campo).GetValue(ObjM).ToString = "0", "", ObjM.GetType.GetProperty(c.Campo).GetValue(ObjM))
                             End Sub)
                TBFecAltaD.Text = ObjM.FecAlta + " | " + ObjM.NomUsuAlta
                TBFecActD.Text = ObjM.FecAct + " | " + ObjM.NomUsuAct
                CodALLIXCte.Text = New Clientes().FindById(CodCliente.Text).CodALLIX
            End If
            LlenaRPT_Etapas()
            If regPId.Text <> "0" Then MostrarPrograma()
            SeguridadLoad()

        Catch ex As Exception
            Alertas("", CleanSpecialCharacter(ex.Message), False, 4)
        End Try

    End Sub
    Sub LlenaRPT_Etapas()
        Try
            Dim lstP As List(Of OptimizerG_ProgramaA_EtapasModel) = New OptimizerG_ProgramaA_Etapas().FindlstAll(CodCliente.Text, Convert.ToInt64(regPId.Text), Convert.ToInt64(CvePerfilN.Text))
            rptEtapas.DataSource = lstP
            rptEtapas.DataBind()

            Dim lstPN As OptimizerG_PerfilNModel = New OptimizerG_PerfilN().FindById(Convert.ToInt64(CvePerfilN.Text), "")
            Dim lstE As List(Of OptimizerG_PerfilN_EtapasModel) = New OptimizerG_PerfilN_Etapas().FindlstAll(CodCliente.Text, Convert.ToInt64(CvePerfilN.Text))
            Dim ObjR As ResponseModel = JsonConvert.DeserializeObject(Of ResponseModel)(New OptimizerG_PerfilN_Resultado().FindById(Convert.ToInt64(CvePerfilN.Text)).Response)
            rptEtapas.Items.Cast(Of RepeaterItem)().ToList.ForEach(Sub(p)
                                                                       Dim Aplica As String = TryCast(p.FindControl("Aplica"), Label).Text
                                                                       Dim CveEtapa As String = TryCast(p.FindControl("CveEtapa"), Label).Text
                                                                       Dim chk As CheckBox = TryCast(p.FindControl("chk"), CheckBox)
                                                                       Dim TBCosto As TextBox = TryCast(p.FindControl("TBCosto"), TextBox)
                                                                       Dim EdadIni As Label = TryCast(p.FindControl("EdadIni"), Label)
                                                                       Dim EdadFin As Label = TryCast(p.FindControl("EdadFin"), Label)
                                                                       Dim Mortalidad As Label = TryCast(p.FindControl("Mortalidad"), Label)
                                                                       Dim ConsumoAlimento As Label = TryCast(p.FindControl("ConsumoAlimento"), Label)
                                                                       Dim PesoHuevo As Label = TryCast(p.FindControl("PesoHuevo"), Label)
                                                                       Dim Produccion As Label = TryCast(p.FindControl("Produccion"), Label)

                                                                       Dim lst2 As OptimizerG_PerfilN_EtapasModel = lstE.Find(Function(c) c.CveEtapa = CInt(CveEtapa))
                                                                       chk.Checked = If(lst2.Aplica = "S", True, False)
                                                                       Call OnCheckedChanged(chk, Nothing)

                                                                       If lst2.Aplica = "S" Then
                                                                           EdadIni.Text = ObjR.Variables.Find(Function(s) s.NoVariable = 1).Etapas.Find(Function(c) CInt(c.Clave) = CInt(CveEtapa)).Valor.ToString
                                                                           EdadFin.Text = ObjR.Variables.Find(Function(s) s.NoVariable = 2).Etapas.Find(Function(c) CInt(c.Clave) = CInt(CveEtapa)).Valor.ToString
                                                                           Mortalidad.Text = Math.Round(ObjR.Variables.Find(Function(s) s.NoVariable = 23).Etapas.Find(Function(c) CInt(c.Clave) = CInt(CveEtapa)).Valor, 2).ToString
                                                                           ConsumoAlimento.Text = Math.Round(ObjR.Variables.Find(Function(s) s.NoVariable = 35).Etapas.Find(Function(c) CInt(c.Clave) = CInt(CveEtapa)).Valor, 2).ToString
                                                                           PesoHuevo.Text = Math.Round(ObjR.Variables.Find(Function(s) s.NoVariable = 15).Etapas.Find(Function(c) CInt(c.Clave) = CInt(CveEtapa)).Valor, 2).ToString
                                                                           Produccion.Text = Math.Round(ObjR.Variables.Find(Function(s) s.NoVariable = 12).Etapas.Find(Function(c) CInt(c.Clave) = CInt(CveEtapa)).Valor, 2).ToString
                                                                       End If
                                                                   End Sub)
        Catch ex As Exception
            rptEtapas.DataSource = Nothing
            rptEtapas.DataBind()
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
            TBPrecioVentaHuevo.Text = TBPrecioVentaH.Text
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
    Protected Sub RPTOnItemDataBound(ByVal sender As Object, ByVal e As RepeaterItemEventArgs) Handles rptEtapas.ItemDataBound
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            'Reference the Repeater Item.
            Dim item As RepeaterItem = e.Item
            Dim chk As CheckBox = (TryCast(item.FindControl("chk"), CheckBox))
            Dim Aplica As String = (TryCast(item.FindControl("Aplica"), Label)).Text
            Dim Fija As String = (TryCast(item.FindControl("Fija"), Label)).Text
            Dim CodALLIX As String = (TryCast(item.FindControl("CodALLIX"), Label)).Text

            Dim CveEtapa As String = (TryCast(item.FindControl("CveEtapa"), Label)).Text
            Dim TBCosto As TextBox = (TryCast(item.FindControl("TBCosto"), TextBox))
            Dim Costo As Label = (TryCast(item.FindControl("Costo"), Label))

            chk.Checked = If(Aplica = "S", True, False)
            chk.Enabled = If(Fija = "S", False, True)

            TBCosto.Enabled = chk.Checked
        End If
    End Sub


    '--Acciones---
    Sub Regresar()
        Response.Redirect(New RedirectPaginas().FindById(Plataforma + "-" + menu + "-0").PaginaURL.Replace("@filtro", Codif(filtroview.Text)).Replace("@pageIndex", gvindexpage.Text), True)
    End Sub
    Sub Refrescar()
        Response.Redirect(New RedirectPaginas().FindById(Plataforma + "-" + menu + "-1").PaginaURL.Replace("@Id", Codif(regPId.Text)).Replace("@CvePN", Codif(CvePerfilN.Text)).Replace("@filtro", Codif(filtroview.Text)).Replace("@pageIndex", gvindexpage.Text), True)
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
                                 'If c.CveMSC = "41-3-19" And c.Valor = "0" Then msg = ""
                             End Sub)
            End If
            'Valida Grid Captura
            If msg = "" Then
                If ValidRPTChkCaptura(rptEtapas, "chk", iList) Then msg = New Mensajes().FindById("0", 0, 43).NomMensaje
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

            Dim Valores As String = String.Join("|", (lstControles.FindAll(Function(p) p.Editable = "S").Select(Function(a) a.Valor).ToList())) + "|1"
            Dim valoresE As String = GetRPTChkCapturaAll(rptEtapas, "chk", iList)
            Dim ObjM As New OptimizerG_ProgramaA()

            IsResult = ObjM.SaveModel(Convert.ToInt64(regPId.Text), Convert.ToInt64(CvePerfilN.Text), Valores, valoresE, ObjUser.CodUsuario)
            If IsResult Then
                'ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Validation", "alert('" + "Los datos han sido guardados." + "');", True)
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
        Dim IsResult As Boolean = False
        Dim lst As New List(Of WSOptimizerG_PlanA_RequestEtapasModel)
        Try
            rptEtapas.Items.Cast(Of RepeaterItem)().ToList.ForEach(Sub(r)
                                                                       Dim CveEtapa As Int64 = CInt(TryCast(r.FindControl("CveEtapa"), Label).Text)
                                                                       Dim CveTipo As Integer = CInt(TryCast(r.FindControl("CveTipo"), Label).Text)
                                                                       Dim chk As CheckBox = TryCast(r.FindControl("chk"), CheckBox)
                                                                       Dim NomEtapa As String = TryCast(r.FindControl("NomEtapa"), Label).Text
                                                                       Dim Costo As Double = If(chk.Checked, CDbl(TryCast(r.FindControl("TBCosto"), TextBox).Text), 0)
                                                                       Dim EdadIni As Integer = If(chk.Checked, CDbl(TryCast(r.FindControl("EdadIni"), Label).Text), CDbl(0))
                                                                       Dim EdadFin As Integer = If(chk.Checked, CDbl(TryCast(r.FindControl("EdadFin"), Label).Text), CDbl(0))
                                                                       Dim Mortalidad As Double = If(chk.Checked, CDbl(TryCast(r.FindControl("Mortalidad"), Label).Text), CDbl(0))
                                                                       Dim ConsumoAlimento As Double = If(chk.Checked, CDbl(TryCast(r.FindControl("ConsumoAlimento"), Label).Text), CDbl(0))
                                                                       Dim PesoHuevo As Double = If(chk.Checked, CDbl(TryCast(r.FindControl("PesoHuevo"), Label).Text), CDbl(0))
                                                                       Dim Produccion As Double = If(chk.Checked, CDbl(TryCast(r.FindControl("Produccion"), Label).Text), CDbl(0))

                                                                       lst.Add(New WSOptimizerG_PlanA_RequestEtapasModel With {.CveProducto = CveEtapa, .Posicion = CveEtapa, .TipoEtapa = CveTipo _
                                                                       , .Costo = Costo, .EdadInicial = EdadIni, .EdadFinal = EdadFin, .Mortalidad = Mortalidad, .ConsumoAlimento = ConsumoAlimento _
                                                                       , .PesoHuevo = PesoHuevo, .Produccion = Produccion})

                                                                   End Sub)
            Dim req As New WSOptimizerG_PlanA_RequestModel
            req.CvePlan = CInt(regPId.Text)
            req.UsuAct = ObjUser.CodUsuario
            req.CveParametro = 1
            req.CveReferencia = DDLReferencia.SelectedValue
            req.NoGallinas = CInt(TBTotalGallinas.Text)
            req.NoPollitas = CInt(TBTotalPollitas.Text)
            req.PrecioVenta = CDbl(TBPrecioVentaH.Text)
            req.MasaHuevoTotal = 0
            req.ConsumoAlimento = 0
            req.Productos = lst
            Dim Obj As New Interfaz_OptimizerG()
            Dim ObjR As WSOptimizerG_PlanA_ResponseModel = Await Obj.GeneraPlan(req)
            If Obj.WSEstatus Then
                Alertas("", "36", True, 2)
                MostrarPrograma()
            Else
                Alertas("", "37", True, 3)
                LimpiaOptimizado()
            End If
        Catch ex As Exception
            Alertas("", CleanSpecialCharacter(ex.Message), False, 4)
            LimpiaOptimizado()
        End Try

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
    Async Sub Actualizar()
        Dim lst As New List(Of WSNufeed_FormulaModel)
        Dim Isresult As Boolean = False
        Try
            Dim Obj As New Interfaz_Nufeed()
            lst = Await Obj.GetFormulas(CodALLIXCte.Text)
            If lst Is Nothing Then
                'ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Validation", "alert('" + Obj.strError + "');", True)
                Alertas("", "45", False, 4)
                Exit Sub
            End If
            lst.ForEach(Sub(p)
                            rptEtapas.Items.Cast(Of RepeaterItem)().ToList.ForEach(Sub(c)
                                                                                       Dim chk As CheckBox = TryCast(c.FindControl("chk"), CheckBox)
                                                                                       Dim codallix As String = TryCast(c.FindControl("CodALLIX"), Label).Text
                                                                                       Dim TBCosto As TextBox = TryCast(c.FindControl("TBCosto"), TextBox)
                                                                                       Dim TBEM As TextBox = TryCast(c.FindControl("TBEM"), TextBox)
                                                                                       Dim TBEN As TextBox = TryCast(c.FindControl("TBEN"), TextBox)
                                                                                       Dim TBSIDLYS As TextBox = TryCast(c.FindControl("TBSIDLYS"), TextBox)

                                                                                       If chk.Checked And codallix = p.formulaCode Then
                                                                                           Dim precio As Double = p.price / 1000
                                                                                           Dim EM As Double = p.nutrients.Find(Function(c1) c1.nutrientCode = "0110").value
                                                                                           Dim EN As Double = p.nutrients.Find(Function(c1) c1.nutrientCode = "0130").value
                                                                                           Dim Lisina As Double = p.nutrients.Find(Function(c1) c1.nutrientCode = "1200").value

                                                                                           TBCosto.Text = Math.Round(precio, 2).ToString
                                                                                           TBEM.Text = Math.Round(EM, 2).ToString
                                                                                           TBEN.Text = Math.Round(EN, 2).ToString
                                                                                           TBSIDLYS.Text = Math.Round(Lisina, 2).ToString
                                                                                       End If
                                                                                   End Sub)
                        End Sub)
            'Guardar()
            'Generar()
            Alertas("", "46", False, 2)
        Catch ex As Exception
            Alertas("", CleanSpecialCharacter(ex.Message), False, 4)
        End Try

    End Sub
    Sub MostrarPerfil()
        Dim filtro As String = CodCliente.Text + "|1|" + regPId.Text
        Response.Redirect(New RedirectPaginas().FindById(Plataforma + "-2-1").PaginaURL.Replace("@Id", Codif(Convert.ToInt64(CvePerfilN.Text))).Replace("@filtro", Codif(filtro)).Replace("@pageIndex", gvindexpage.Text), True)
    End Sub
    Function RPTCaptura(campos(,) As String) As List(Of String)
        Dim lstEtapas As New List(Of String)
        Try
            rptEtapas.Items.Cast(Of RepeaterItem)().ToList.ForEach(Sub(p)
                                                                       Dim tmp As String = ""
                                                                       For i = 0 To UBound(campos, 1)
                                                                           If campos(i, 1) = "0" Then tmp += TryCast(p.FindControl(campos(i, 0)), Label).Text + "-"
                                                                           If campos(i, 1) = "1" Then tmp += TryCast(p.FindControl(campos(i, 0)), TextBox).Text + "-"
                                                                       Next
                                                                       tmp = If(tmp <> "", Left(tmp, Len(tmp) - 1), "")
                                                                       lstEtapas.Add(tmp)
                                                                   End Sub)
        Catch ex As Exception
            Alertas("", CleanSpecialCharacter(ex.Message), False, 4)
        End Try
        Return lstEtapas
    End Function

    Sub DescargarExcel()
        DescargarArchivoReporte("excel", 2, ConfigurationManager.AppSettings("WSOptimizerPollos"))
    End Sub
    Sub DescargarPdf()
        DescargarArchivoReporte("pdf", 2, ConfigurationManager.AppSettings("WSOptimizerPollos"))
    End Sub
    Private Sub DescargarArchivoReporte(formato As String, versionReporte As Integer, baseApiUrl As String)
        Try
            If regPId.Text = "0" Then Throw New Exception("No se encontró el identificador del perfil para generar el archivo.")
            OptimizerReporteDescarga.Descargar(Me, baseApiUrl, Convert.ToInt64(regPId.Text), formato, versionReporte, "PerfilNutricional")
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