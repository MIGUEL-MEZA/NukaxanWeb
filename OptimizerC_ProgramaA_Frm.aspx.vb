Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Runtime.Remoting
Imports System.Web.DynamicData
Imports Microsoft.Ajax.Utilities
Imports Newtonsoft.Json
Imports NukaxanWEB.Libreria

Public Class OptimizerC_ProgramaA_Frm
    Inherits Page
    Private ObjUser As UsuarioModel
    Private Plataforma As String = "41"
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

        iList.Add(New DatosGrid() With {.Tipo = 0, .Campo = "CveProducto", .Valida = "N", .ValidaCeros = "N"})
        iList.Add(New DatosGrid() With {.Tipo = 0, .Campo = "Aplica", .Valida = "N", .ValidaCeros = "N"})
        iList.Add(New DatosGrid() With {.Tipo = 1, .Campo = "TBCosto", .Valida = "S", .ValidaCeros = "S"})
        'iList.Add(New DatosGrid() With {.Tipo = 0, .Campo = "CA", .Valida = "N", .ValidaCeros = "N"})
        iList.Add(New DatosGrid() With {.Tipo = 0, .Campo = "TBRactopamina", .Valida = "N", .ValidaCeros = "N"})
        iList.Add(New DatosGrid() With {.Tipo = 1, .Campo = "TBEM", .Valida = "N", .ValidaCeros = "S"})
        iList.Add(New DatosGrid() With {.Tipo = 1, .Campo = "TBEN", .Valida = "N", .ValidaCeros = "S"})
        iList.Add(New DatosGrid() With {.Tipo = 1, .Campo = "TBSIDLYS", .Valida = "N", .ValidaCeros = "S"})
        iList.Add(New DatosGrid() With {.Tipo = 1, .Campo = "TBPresupuesto", .Valida = "N", .ValidaCeros = "N"})
        iList.Add(New DatosGrid() With {.Tipo = 1, .Campo = "TBDuracionMin", .Valida = "N", .ValidaCeros = "N"})
        iList.Add(New DatosGrid() With {.Tipo = 1, .Campo = "TBDuracionMax", .Valida = "N", .ValidaCeros = "N"})

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
        LlenaDDL()
        LlenaRegistro()
        gv.PageSize = 40
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

    End Sub
    Sub Controles(op As Boolean)
        Try
            Dim excepciones As String = "41-3-1,41-3-2,41-3-3"
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
        Call New Catalogos().LlenaOptimizer_Referencia(DDLReferencia)
        Call New Catalogos().LlenaOptimizer_Parametros(DDLParametro)
        Call New Catalogos().LlenaOptimizer_EstadoSanitario(DDLEstado)
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
        lstc.Add(New FrmControlModel With {.Nombre = "TBRactopamina", .Tipo = 2})
        lstc.Add(New FrmControlModel With {.Nombre = "TBEM", .Tipo = 2})
        lstc.Add(New FrmControlModel With {.Nombre = "TBEN", .Tipo = 2})
        lstc.Add(New FrmControlModel With {.Nombre = "TBSIDLYS", .Tipo = 2})
        lstc.Add(New FrmControlModel With {.Nombre = "TBPresupuesto", .Tipo = 2})
        lstc.Add(New FrmControlModel With {.Nombre = "TBDuracionMin", .Tipo = 2})
        lstc.Add(New FrmControlModel With {.Nombre = "TBDuracionMax", .Tipo = 2})

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
            'SeguridadRPT(False, rptEtapas, lstc)
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
                TBNomEstatusD.Text = "EN EDICI�N"
                TBFecAltaD.Text = Now.ToString("dd/MM/yyyy") + " | " + ObjUser.NomUsuario
                TBFecActD.Text = Now.ToString("dd/MM/yyyy") + " | " + ObjUser.NomUsuario

                Dim ObjPN As OptimizerC_PerfilNModel = New OptimizerC_PerfilN().FindById(Convert.ToInt64(CvePerfilN.Text), "")
                TBTitulo.Text = ObjPN.Titulo
                DDLCliente.SelectedValue = ObjPN.CodCliente
                TBNomClienteD.Text = ObjPN.NomCliente
                CodCliente.Text = ""
                DDLModalidad.SelectedValue = ObjPN.CveModalidad
                TBNomModalidadD.Text = ObjPN.NomModalidad
                DDLReferencia.SelectedValue = ObjPN.CveReferencia
                TBTemperatura.Text = ObjPN.Temperatura.ToString
                DDLEstado.SelectedValue = 1
                TBEspacio.Text = ObjPN.EspacioCorral.ToString

            Else
                Dim ObjM As OptimizerC_ProgramaAModel = New OptimizerC_ProgramaA().FindById(Convert.ToInt64(regPId.Text), 0, "")
                Autor.Text = ObjM.UsuAlta
                CvePerfilN.Text = ObjM.CvePerfilN.ToString
                CveEstatus.Text = ObjM.CveEstatus.ToString
                CodCliente.Text = ObjM.CodCliente
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
                CodALLIXCte.Text = New Clientes().FindById(CodCliente.Text).CodALLIX
            End If

            LlenaRPT_Etapas()
            MostrarPrograma()
            SeguridadLoad()

        Catch ex As Exception
            Alertas("", CleanSpecialCharacter(ex.Message), False, 4)
        End Try

    End Sub
    Sub LlenaRPT_Etapas()
        Try
            Dim lstP As List(Of OptimizerC_ProgramaA_EtapasModel) = New OptimizerC_ProgramaA_Etapas().FindlstAll(CodCliente.Text, CInt(regPId.Text))
            rptEtapas.DataSource = lstP
            rptEtapas.DataBind()

            If regPId.Text = "0" Then
                Dim lstPN As OptimizerC_PerfilNModel = New OptimizerC_PerfilN().FindById(Convert.ToInt64(CvePerfilN.Text), "")
                Dim lstE As List(Of OptimizerC_PerfilN_EtapasModel) = New OptimizerC_PerfilN_Etapas().FindlstAll(CodCliente.Text, Convert.ToInt64(CvePerfilN.Text))
                Dim ObjR As ResponseModel = JsonConvert.DeserializeObject(Of ResponseModel)(New OptimizerC_PerfilN_Resultado().FindById(Convert.ToInt64(CvePerfilN.Text)).Response)
                rptEtapas.Items.Cast(Of RepeaterItem)().ToList.ForEach(Sub(p)
                                                                           Dim Aplica As String = TryCast(p.FindControl("Aplica"), Label).Text
                                                                           Dim CveProducto As String = TryCast(p.FindControl("CveProducto"), Label).Text
                                                                           If CInt(CveProducto) >= 5 Then
                                                                               Dim chk As CheckBox = TryCast(p.FindControl("chk"), CheckBox)
                                                                               'Dim TBRactopamina As TextBox = TryCast(p.FindControl("TBRactopamina"), TextBox)
                                                                               Dim TBRactopamina As Label = TryCast(p.FindControl("TBRactopamina"), Label)
                                                                               Dim TBEM As TextBox = TryCast(p.FindControl("TBEM"), TextBox)
                                                                               Dim TBEN As TextBox = TryCast(p.FindControl("TBEN"), TextBox)
                                                                               Dim TBSIDLYS As TextBox = TryCast(p.FindControl("TBSIDLYS"), TextBox)
                                                                               Dim lst2 As OptimizerC_PerfilN_EtapasModel = lstE.Find(Function(c) c.CveEtapa + 4 = CInt(CveProducto))
                                                                               chk.Checked = If(lst2.Aplica = "S", True, False)
                                                                               Call OnCheckedChanged(chk, Nothing)

                                                                               If lst2.Aplica = "S" Then
                                                                                   TBEN.Text = lst2.ENAlimento.ToString
                                                                                   TBEM.Text = Math.Round(lst2.ENAlimento / CDbl(New Parametros().FindById(Plataforma, 6).Valor), 2)
                                                                                   Dim SIDLys As Double = CDbl(ObjR.Variables.Find(Function(s) s.NoVariable = 26).Etapas.Find(Function(c) CInt(c.Clave) = CInt(CveProducto) - 4).Valor)
                                                                                   TBSIDLYS.Text = Math.Round(SIDLys, 2).ToString
                                                                                   TBRactopamina.Text = If(lst2.IsRAC = "S", lstPN.RAC.ToString, "0")

                                                                               End If
                                                                           End If
                                                                       End Sub)
            End If

        Catch ex As Exception
            rptEtapas.DataSource = Nothing
            rptEtapas.DataBind()
            Alertas("", CleanSpecialCharacter(ex.Message), False, 4)
        End Try
    End Sub
    Sub LlenaRPT_Resultado()
        Try
            Dim ObjM As WSOptimizer_Presupuesto_OptimizadoModel = New Optimizer_Presupuesto_Optimizado().FindById(1, Convert.ToInt64(regPId.Text))
            Dim ObjRList As WSOptimizerC_ResponsePlan = JsonConvert.DeserializeObject(Of WSOptimizerC_ResponsePlan)(ObjM.Response)
            If ObjRList Is Nothing Then Exit Sub
            Dim tmp As String = ""
            Dim lineas As String = ""
            Dim row As Integer = 0
            rptEtapas.Items.Cast(Of RepeaterItem)().ToList.ForEach(Sub(p)
                                                                       row += 1
                                                                       Dim Aplica As String = TryCast(p.FindControl("Aplica"), Label).Text
                                                                       If Aplica = "S" Then
                                                                           lineas += row.ToString + ","
                                                                       End If

                                                                   End Sub)
            If lineas <> "" Then lineas = Left(lineas, Len(lineas) - 1)
            Dim ObjR As WSOptimizerC_ResponseModel = ObjRList.ResponseParametro.Find(Function(p) p.CveParametro = DDLParametro.SelectedValue)

            ObjR.Data.ForEach(Sub(p)
                                  If New ArrayList(lineas.Split(",")).IndexOf(p.Identificador) >= 0 Then
                                      tmp += p.Identificador + "#" + Math.Round(p.Costo, 2).ToString + "#" + Math.Round(p.CDA_Kg, 3).ToString + "#" + Math.Round(p.PresupuestoCerdo, 2).ToString +
                                 "#" + Math.Round(p.GDP, 3).ToString + "#" + Math.Round(p.Peso_Inicial, 2).ToString + "#" + Math.Round(p.Peso_Final, 2).ToString + "#" + Math.Round(p.CA, 2).ToString +
                                 "#" + Math.Round(p.Edad_Inicial, 0).ToString + "#" + Math.Round(p.Edad_Final, 0).ToString + "#" + Math.Round(Math.Round(p.Edad_Final, 0) - Math.Round(p.Edad_Inicial, 0), 0).ToString + "|"
                                      '"#" + Math.Round(p.Edad_Inicial, 2).ToString + "#" + Math.Round(p.Edad_Final, 2).ToString + "#" + Math.Round(p.Duracion_Etapa, 2).ToString + "|"
                                  End If
                              End Sub)
            If tmp <> "" Then tmp = Left(tmp, Len(tmp) - 1)

            Dim dt As DataTable = New Optimizer_Presupuesto_Resultado().FindAll(1, tmp)
            rptResultado.DataSource = dt
            rptResultado.DataBind()

            Dim TOTCosto As Double = ObjR.Resultado.Costo 'dt.Select.Sum(Function(p) CDbl(p("Costo")))
            Dim Presupuesto As Double = ObjR.Resultado.Presupuesto 'dt.Select.Sum(Function(p) CDbl(p("Presupuesto")))
            Dim Duracion As Double = ObjR.Resultado.DuracionTotal 'dt.Select.Sum(Function(p) p("Duracion"))
            Dim CA As Double = Math.Round(ObjR.Resultado.Ca, 2) ' Math.Round(Presupuesto / ObjR.Optimizer.Find(Function(p) p.Orden = 0).Valor, 2)
            Dim GDP As Double = Math.Round(ObjR.Resultado.Gdp, 2) 'Math.Round(ObjR.Optimizer.Find(Function(p) p.Orden = 0).Valor / Duracion, 2)
            Dim CDA As Double = Math.Round(ObjR.Resultado.Cda, 2) 'Math.Round(Presupuesto / Duracion, 2)


            'TryCast(rptResultado.Controls(rptResultado.Controls.Count - 1).Controls(0).FindControl("TOTCosto"), Label).Text = TOTCosto.ToString("C2")
            TryCast(rptResultado.Controls(rptResultado.Controls.Count - 1).Controls(0).FindControl("TOTCDA"), Label).Text = CDA.ToString("N2")
            TryCast(rptResultado.Controls(rptResultado.Controls.Count - 1).Controls(0).FindControl("TOTPresupuesto"), Label).Text = Presupuesto.ToString("N1")
            TryCast(rptResultado.Controls(rptResultado.Controls.Count - 1).Controls(0).FindControl("TOTGDP"), Label).Text = GDP.ToString("N3")
            TryCast(rptResultado.Controls(rptResultado.Controls.Count - 1).Controls(0).FindControl("TOTCA"), Label).Text = CA.ToString("N2")
            'TryCast(rptResultado.Controls(rptResultado.Controls.Count - 1).Controls(0).FindControl("TOTDuracion"), Label).Text = Duracion.ToString("N0")

            Tot_PrecioVenta.Text = ObjR.Resultado.PrecioVenta.ToString("C2") 'TBPrecioVenta.Text
            Tot_PesoVenta.Text = ObjR.Resultado.PesoVenta.ToString("N2") 'ObjR.Data.Find(Function(p) p.Identificador = 10).Peso_Final.ToString("N2")
            Tot_EdadVenta.Text = Math.Round(ObjR.Resultado.EdadVenta, 2).ToString("N0") 'ObjR.Data.Find(Function(p) p.Identificador = 10).Edad_Final.ToString("N0")
            Tot_AlimentoPresupuestado.Text = ObjR.Resultado.Presupuesto.ToString("N1") 'Presupuesto.ToString("N1")
            Tot_KilosProducidos.Text = Math.Round(ObjR.Resultado.KilosProducidos, 2).ToString("N2") 'Math.Round(ObjR.Data.Find(Function(p) p.Identificador = 10).Peso_Final - ObjR.Data.Find(Function(p) p.Identificador = 1).Peso_Inicial, 2)
            Tot_GDP.Text = Math.Round(ObjR.Resultado.Gdp, 2).ToString("N2") 'GDP.ToString("N3")
            Tot_CA.Text = Math.Round(ObjR.Resultado.Ca, 2).ToString("N2") 'CA.ToString("N2")
            Tot_CostoTotal.Text = Math.Round(ObjR.Resultado.CostoTotalAlimento, 2).ToString("C2") 'Math.Round(ObjR.Data.Sum(Function(p) p.PresupuestoCerdo * p.Costo), 2).ToString("C2")
            Tot_CostoPonderado.Text = Math.Round(ObjR.Resultado.CostoPonderado, 2).ToString("C2") 'Math.Round(CDbl(Tot_CostoTotal.Text) / Presupuesto, 2).ToString("C2")
            Tot_CostoKiloProd.Text = ObjR.Resultado.CostokiloProducido.ToString("C2") 'ObjR.Optimizer.Find(Function(p) p.Orden = 4).Valor.ToString("C2")
            Tot_Utilidad.Text = ObjR.Resultado.Utilidad.ToString("C2") 'ObjR.Optimizer.Find(Function(p) p.Orden = 5).Valor.ToString("C2")
            Tot_ROI.Text = ObjR.Resultado.Roi.ToString("N2") 'ObjR.Optimizer.Find(Function(p) p.Orden = 6).Valor.ToString("N2")

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
            Dim IsEtapa As String = (TryCast(item.FindControl("IsEtapa"), Label)).Text
            Dim IsRactopamina As String = (TryCast(item.FindControl("IsRactopamina"), Label)).Text
            Dim IsEM As String = (TryCast(item.FindControl("IsEM"), Label)).Text
            Dim IsEN As String = (TryCast(item.FindControl("IsEN"), Label)).Text
            Dim IsSID As String = (TryCast(item.FindControl("IsSID"), Label)).Text
            Dim IsPresupuesto As String = (TryCast(item.FindControl("IsPresupuesto"), Label)).Text
            'Dim IsPesoFinal As String = (TryCast(item.FindControl("IsPesoFinal"), Label)).Text
            Dim IsDuracionMin As String = (TryCast(item.FindControl("IsDuracionMin"), Label)).Text
            Dim IsDuracionMax As String = (TryCast(item.FindControl("IsDuracionMax"), Label)).Text
            Dim CA As String = (TryCast(item.FindControl("CA"), Label)).Text
            Dim CodALLIX As String = (TryCast(item.FindControl("CodALLIX"), Label)).Text

            Dim CveProducto As String = (TryCast(item.FindControl("CveProducto"), Label)).Text
            Dim TBCosto As TextBox = (TryCast(item.FindControl("TBCosto"), TextBox))
            Dim Costo As Label = (TryCast(item.FindControl("Costo"), Label))
            Dim TBRactopamina As Label = (TryCast(item.FindControl("TBRactopamina"), Label))
            'Dim TBRactopamina As TextBox = (TryCast(item.FindControl("TBRactopamina"), TextBox))
            'Dim Ractopamina As Label = (TryCast(item.FindControl("Ractopamina"), Label))
            Dim TBEM As TextBox = (TryCast(item.FindControl("TBEM"), TextBox))
            Dim EM As Label = (TryCast(item.FindControl("EM"), Label))
            Dim TBEN As TextBox = (TryCast(item.FindControl("TBEN"), TextBox))
            Dim EN As Label = (TryCast(item.FindControl("EN"), Label))
            Dim TBSIDLYS As TextBox = (TryCast(item.FindControl("TBSIDLYS"), TextBox))
            Dim SIDLYS As Label = (TryCast(item.FindControl("SIDLYS"), Label))
            Dim TBPresupuesto As TextBox = (TryCast(item.FindControl("TBPresupuesto"), TextBox))
            Dim Presupuesto As Label = (TryCast(item.FindControl("Presupuesto"), Label))
            Dim TBDuracionMin As TextBox = (TryCast(item.FindControl("TBDuracionMin"), TextBox))
            Dim DuracionMin As Label = (TryCast(item.FindControl("DuracionMin"), Label))
            Dim TBDuracionMax As TextBox = (TryCast(item.FindControl("TBDuracionMax"), TextBox))
            Dim DuracionMax As Label = (TryCast(item.FindControl("DuracionMax"), Label))

            'TBRactopamina.Visible = If(IsRactopamina = "S", True, False)
            TBEM.Visible = If(IsEM = "S", True, False)
            TBEN.Visible = If(IsEN = "S", True, False)
            TBSIDLYS.Visible = If(IsSID = "S", True, False)
            TBPresupuesto.Visible = If(IsPresupuesto = "S", True, False)
            TBDuracionMin.Visible = If(IsDuracionMin = "S", True, False)
            TBDuracionMax.Visible = If(IsDuracionMax = "S", True, False)

            chk.Checked = If(Aplica = "S", True, False)
            TBCosto.Enabled = chk.Checked
            'TBRactopamina.Enabled = chk.Checked
            TBEM.Enabled = chk.Checked
            TBEN.Enabled = chk.Checked
            TBSIDLYS.Enabled = chk.Checked
            TBPresupuesto.Enabled = chk.Checked
            TBDuracionMin.Enabled = chk.Checked
            TBDuracionMax.Enabled = chk.Checked

            'If IsEtapa = "S" Then chk.Enabled = False

            'TBCosto.Text = "0"
            'TBRactopamina.Text = "0"
            'TBEM.Text = "0"
            'TBEN.Text = "0"
            'TBSIDLYS.Text = "0"
            'TBGDP.Text = "0"
            'TBPresupuesto.Text = "0"
            'TBDuracionMin.Text = "0"
            'TBDuracionMax.Text = "0"

        End If
    End Sub

    Sub DefineGV()
        Dim dtCol As DataTable
        Dim wgv As Integer = 0
        Try
            gv.Columns.Clear()
            gv2.Columns.Clear()

            dtCol = New Reportes().Columnas(CInt(Plataforma), CInt(menu), CInt(regPId.Text))
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
            dt = New Reportes().Datos(CInt(Plataforma), CInt(menu), regPId.Text)
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
    Protected Sub GVRowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gv.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            ' Conta.Text += e.Row.Cells(0).Text + " | "
            'e.Row.Cells(0).Style.Add("cursor", "pointer")
            e.Row.Cells(0).Font.Bold = True
            e.Row.Cells(1).Font.Bold = True
            For i = 1 To 6
                If New ArrayList({0, 7, 8, 9, 10}).IndexOf(e.Row.RowIndex) >= 0 Then e.Row.Cells(i).Text = CDbl(e.Row.Cells(i).Text).ToString("C2")
                If New ArrayList({1, 2, 3, 4, 5, 6, 11}).IndexOf(e.Row.RowIndex) >= 0 Then e.Row.Cells(i).Text = CDbl(e.Row.Cells(i).Text).ToString("N2")
            Next

        End If
    End Sub
    Protected Sub GVRowDataBound2(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gv2.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            ' Conta.Text += e.Row.Cells(0).Text + " | "
            'e.Row.Cells(0).Style.Add("cursor", "pointer")
            e.Row.Cells(0).Font.Bold = True
            e.Row.Cells(1).Font.Bold = True
            If rptEtapas.Items.Cast(Of RepeaterItem)().Where(Function(p) TryCast(p.FindControl("CveProducto"), Label).Text = (e.Row.RowIndex + 1).ToString And TryCast(p.FindControl("Aplica"), Label).Text = "S").Count = 0 Then
                e.Row.Visible = False

            End If

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
            'Valida d�as raptopamina
            'If msg = "" And rptEtapas.Items.Cast(Of RepeaterItem)().Where(Function(p) TryCast(p.FindControl("CveProducto"), Label).Text = "10" _
            'And TryCast(p.FindControl("chk"), CheckBox).Checked).Count > 0 And TBDiasRAC.Text = "0" Then
            '    msg = New Mensajes().FindById("0", 0, 40).NomMensaje
            'ElseIf msg = "" And rptEtapas.Items.Cast(Of RepeaterItem)().Where(Function(p) TryCast(p.FindControl("CveProducto"), Label).Text = "10" _
            'And TryCast(p.FindControl("chk"), CheckBox).Checked = False).Count > 0 And TBDiasRAC.Text <> "0" Then
            'msg = New Mensajes().FindById("0", 0, 41).NomMensaje

            'Valida Grid Captura
            If msg = "" Then
                If ValidRPTChkCaptura(rptEtapas, "chk", iList) Then msg = New Mensajes().FindById("0", 0, 43).NomMensaje
            End If
            If msg = "" And rptEtapas.Items.Cast(Of RepeaterItem)().Where(Function(p) TryCast(p.FindControl("CveProducto"), Label).Text <> "10" And TryCast(p.FindControl("IsEtapa"), Label).Text = "S" _
            And TryCast(p.FindControl("chk"), CheckBox).Checked And TryCast(p.FindControl("IsDuracionMin"), Label).Text = "S" _
            And CInt(TryCast(p.FindControl("TBDuracionMin"), TextBox).Text) < CInt(New Parametros().FindById(CInt(Plataforma), 5).Valor)).Count > 0 Then
                msg = New Mensajes().FindById("0", 0, 44).NomMensaje.Replace("#valor", New Parametros().FindById(CInt(Plataforma), 5).Valor)

            ElseIf msg = "" And rptEtapas.Items.Cast(Of RepeaterItem)().Where(Function(p) TryCast(p.FindControl("CveProducto"), Label).Text = "10" And TryCast(p.FindControl("IsEtapa"), Label).Text = "S" _
            And TryCast(p.FindControl("chk"), CheckBox).Checked And TryCast(p.FindControl("IsDuracionMin"), Label).Text = "S" _
            And CInt(TryCast(p.FindControl("TBDuracionMin"), TextBox).Text) < CInt(New Parametros().FindById(CInt(Plataforma), 5).Valor)).Count > 0 Then
                msg = New Mensajes().FindById("0", 0, 55).NomMensaje.Replace("#valor", New Parametros().FindById(CInt(Plataforma), 7).Valor)
            End If

            If msg = "" Then
                Dim dur_min As Double = rptEtapas.Items.Cast(Of RepeaterItem)().Where(Function(p) TryCast(p.FindControl("chk"), CheckBox).Checked).Sum(Function(p) TryCast(p.FindControl("TBDuracionMin"), TextBox).Text) '+ CInt(TBDiasRAC.Text)
                Dim dur_max As Double = rptEtapas.Items.Cast(Of RepeaterItem)().Where(Function(p) TryCast(p.FindControl("chk"), CheckBox).Checked).Sum(Function(p) TryCast(p.FindControl("TBDuracionMax"), TextBox).Text) '+ CInt(TBDiasRAC.Text)
                Dim dur_valida As Integer = CInt(TBEdadVenta.Text) - CInt(TBEdadSalida.Text)
                If Not (dur_valida >= dur_min And dur_valida <= dur_max) Then msg = New Mensajes().FindById("0", 0, 42).NomMensaje

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
            Dim ObjM As New OptimizerC_ProgramaA()
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
        Dim lst As New List(Of WSPlanA_RequestProductoModel)
        Try
            rptEtapas.Items.Cast(Of RepeaterItem)().ToList.ForEach(Sub(r)
                                                                       Dim chk As CheckBox = TryCast(r.FindControl("chk"), CheckBox)
                                                                       Dim CveProducto As Int64 = CInt(TryCast(r.FindControl("CveProducto"), Label).Text)
                                                                       Dim nomProducto As String = TryCast(r.FindControl("NomProducto"), Label).Text
                                                                       Dim posicion As Integer = CInt(TryCast(r.FindControl("Posicion"), Label).Text)
                                                                       Dim CA As Double = CDbl(TryCast(r.FindControl("CA"), Label).Text)
                                                                       Dim costo As Double = If(chk.Checked, CDbl(TryCast(r.FindControl("TBCosto"), TextBox).Text), 0)
                                                                       'Dim ractopamina As Double = If(chk.Checked, CDbl(TryCast(r.FindControl("TBRactopamina"), TextBox).Text), 0)
                                                                       Dim ractopamina As Double = If(chk.Checked, CDbl(TryCast(r.FindControl("TBRactopamina"), Label).Text), 0)
                                                                       Dim EM As Double = If(chk.Checked, CDbl(TryCast(r.FindControl("TBEM"), TextBox).Text), CDbl(0))
                                                                       Dim EN As Double = If(chk.Checked, CDbl(TryCast(r.FindControl("TBEN"), TextBox).Text), CDbl(0))
                                                                       Dim SID As Double = If(chk.Checked, CDbl(TryCast(r.FindControl("TBSIDLYS"), TextBox).Text), CDbl(0))
                                                                       Dim isEtapa As String = TryCast(r.FindControl("IsEtapa"), Label).Text
                                                                       Dim presupuesto As Double = If(chk.Checked, CDbl(TryCast(r.FindControl("TBPresupuesto"), TextBox).Text), CDbl(0))
                                                                       Dim pesoFinal As Double = 0
                                                                       Dim duracionMin As Double = If(chk.Checked, CDbl(TryCast(r.FindControl("TBDuracionMin"), TextBox).Text), CDbl(0))
                                                                       Dim duracionMax As Double = If(chk.Checked, CDbl(TryCast(r.FindControl("TBDuracionMax"), TextBox).Text), CDbl(0))

                                                                       'If CveProducto = 10 Then
                                                                       '    duracionMin = 21 'CDbl(TBDiasRAC.Text)
                                                                       '    duracionMax = 35 'CDbl(TBDiasRAC.Text)
                                                                       'End If

                                                                       lst.Add(New WSPlanA_RequestProductoModel With {.CveProducto = CveProducto, .nomProducto = nomProducto, .posicion = posicion, .CA = CA, .costo = costo,
                                                                               .ractopamina = ractopamina, .EM = EM, .EN = EN, .SID = SID, .isEtapa = isEtapa, .presupuesto = presupuesto, .pesoFinal = pesoFinal,
                                                                               .duracionMin = duracionMin, .duracionMax = duracionMax})

                                                                   End Sub)

            Dim req As New WSPlanA_RequestModel
            req.CvePlan = CInt(regPId.Text)
            req.UsuAct = ObjUser.CodUsuario
            req.CveReferencia = DDLReferencia.SelectedValue
            req.CveParametro = DDLParametro.SelectedValue
            req.PrecioVenta = CDbl(TBPrecioVenta.Text)
            req.Desperdicio = CDbl(TBDesperdicio.Text)
            req.PesoPromedio = CDbl(TBPesoDestete.Text)
            req.EdadDestete = CDbl(TBEdadDestete.Text)
            req.EdadSalida = CDbl(TBEdadSalida.Text)
            req.EdadVenta = CDbl(TBEdadVenta.Text)
            'req.DiasRactopamina = CDbl(TBDiasRAC.Text)
            req.CveEstado = DDLEstado.SelectedValue
            req.Temperatura = CDbl(TBTemperatura.Text)
            If CDbl(TBTemperatura.Text) < 17 Then req.Temperatura = 17
            If CDbl(TBTemperatura.Text) > 29 Then req.Temperatura = 29
            req.metrosCerdos = CDbl(TBEspacio.Text)
            req.Productos = lst
            Dim Obj As New Interfaz_Optimizer()
            Dim ObjR As WSPlanA_ResponseModel = Await Obj.GeneraPlan(req)
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
        Dim filtro As String = ""
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
        'Dim TBRactopamina As TextBox = TryCast(item.FindControl("TBRactopamina"), TextBox)
        Dim TBRactopamina As Label = TryCast(item.FindControl("TBRactopamina"), Label)
        Dim IsRactopamina As String = TryCast(item.FindControl("IsRactopamina"), Label).Text
        Dim TBEM As TextBox = TryCast(item.FindControl("TBEM"), TextBox)
        Dim TBEN As TextBox = TryCast(item.FindControl("TBEN"), TextBox)
        Dim TBSIDLYS As TextBox = TryCast(item.FindControl("TBSIDLYS"), TextBox)
        'Dim TBGDP As TextBox = TryCast(item.FindControl("TBGDP"), TextBox)
        Dim TBPresupuesto As TextBox = TryCast(item.FindControl("TBPresupuesto"), TextBox)
        Dim TBDuracionMin As TextBox = TryCast(item.FindControl("TBDuracionMin"), TextBox)
        Dim TBDuracionMax As TextBox = TryCast(item.FindControl("TBDuracionMax"), TextBox)

        TBCosto.Enabled = chk.Checked
        'TBRactopamina.Enabled = chk.Checked
        TBEM.Enabled = chk.Checked
        TBEN.Enabled = chk.Checked
        TBSIDLYS.Enabled = chk.Checked
        'TBGDP.Enabled = chk.Checked
        TBPresupuesto.Enabled = chk.Checked
        TBDuracionMin.Enabled = chk.Checked
        TBDuracionMax.Enabled = chk.Checked

        TBCosto.Text = "0"
        TBRactopamina.Text = "0"
        TBEM.Text = "0"
        TBEN.Text = "0"
        TBSIDLYS.Text = "0"
        'TBGDP.Text = "0"
        TBPresupuesto.Text = "0"
        TBDuracionMin.Text = "0"
        TBDuracionMax.Text = "0"

        If IsRactopamina = "S" And chk.Checked Then
            Dim lstPN As OptimizerC_PerfilNModel = New OptimizerC_PerfilN().FindById(Convert.ToInt64(CvePerfilN.Text), "")
            TBRactopamina.Text = lstPN.RAC.ToString
        End If

    End Sub
    Public Overrides Sub VerifyRenderingInServerForm(control As Control)
        ' Verifies that the control is rendered
    End Sub

End Class