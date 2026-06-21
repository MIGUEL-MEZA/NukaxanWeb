Imports System.IO
Imports System.Web.DynamicData
Imports Newtonsoft.Json
Imports NukaxanWEB.Libreria
Imports Saplin

Public Class Nireo_Clientes_Plataformas_Frm
    Inherits Page
    Private ObjUser As UsuarioModel
    Private Plataforma As String = "2"
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
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Me.ObjUser = DirectCast(Session("UsuarioLogin"), UsuarioModel)
        If Me.Page.User.Identity.IsAuthenticated = False Or ObjUser Is Nothing Then Response.Redirect("logout.aspx", True)
        lstMensajes = New Mensajes().FindlstAll("0," + menu, 1, 0)
        lstErrores = New Mensajes().FindlstAll("0," + menu, 2, 0)
        lstEtiquetas = New Etiquetas().FindlstAll("1," + Plataforma, "0," + menu, 0)
        lstAcciones = New Controles_Acciones().FindlstAll("1,2", 0)
        lstControles = New Controles_Captura().FindlstAll(CInt(Plataforma), CInt(menu), 0)

        iList.Add(New DatosGrid() With {.Tipo = 0, .Campo = "CvePlataforma", .Valida = "N", .ValidaCeros = "N"})
        iList.Add(New DatosGrid() With {.Tipo = 0, .Campo = "CvePlataformaP", .Valida = "N", .ValidaCeros = "N"})
        iList.Add(New DatosGrid() With {.Tipo = 2, .Campo = "DDLEstatus", .Valida = "S", .ValidaCeros = "N"})

        If Not Page.IsPostBack Then
            DatosLoad()
        End If
    End Sub
    Sub DatosLoad()
        'pnlPopup.Style.Value = "display:none;"
        regPId.Text = "0"
        filtroview.Text = ""
        gvindexpage.Text = "0"
        If Not Request.QueryString("Id") Is Nothing Then regPId.Text = DeCodif(Request.QueryString("Id"))
        If Not Request.QueryString("filtro") Is Nothing Then filtroview.Text = DeCodif(Request.QueryString("filtro"))
        If Not Request.QueryString("pageIndex") Is Nothing Then gvindexpage.Text = Request.QueryString("pageIndex")
        mpe_op.Text = ""
        LlenaMenu()
        Etiquetas()
        LlenaDDL()
        LlenaRegistro()
    End Sub
    Public Sub LlenaMenu()
        Dim uri = New Uri(Request.Url.AbsoluteUri)
        Dim filename = Path.GetFileName(uri.LocalPath)
        Dim pathURL As String = "~/menuURL?Id=" + Codif(regPId.Text) + "&filter=" + Codif(filtroview.Text)
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
        PageTitulo.Text = New Menu().FindById(ObjUser.CveRol, CInt(Plataforma), -1, menu).NomMenu
        Menu_Titulo.Text = New Menu_Formulario().FindById(Plataforma, menu, 2).NomMenu

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
            Dim LBL As Label = UPContenido.FindControl("LBLC" + a.CveControl.ToString)
            If Not LBL Is Nothing Then
                LBL.Text = a.Etiqueta.Replace("*", obligatorio)
                If a.ValidaRango <> "" Then
                    Dim LBLH As Label = UPContenido.FindControl("LBLH" + a.CveControl.ToString)
                    If Not LBLH Is Nothing Then LBLH.Text = "(" + New Parametros().FindById(CInt(Plataforma), CInt(a.ValidaRango)).Valor + ")"
                End If
            End If
        Next

    End Sub
    Sub LlenaDDL()

    End Sub
    Sub Acciones(op As Boolean, op2 As Boolean, arrAction As String)
        Dim lb As New LinkButton
        Dim arr2() As String = arrAction.Split(",")
        Dim arr(2) As String
        arr(0) = "LB2"
        arr(1) = "LB3"
        For i = 0 To UBound(arr)
            For j = 0 To UBound(arr2)
                If i = CInt(arr2(j)) Then
                    lb = UPContenido.FindControl(arr(i))
                    lb.Visible = op
                    lb.Enabled = op2
                    lb.CssClass = If(op2 = True, "lnkbtn-action", "lnkbtn-action-disabled")
                    If op2 = True Then lb.Attributes.Add("style", "cursor: pointer;")
                    If op2 = True Then lb.Attributes.Add("style", "margin-left: 14px;")
                    If op2 = True And i = 5 Then lb.Attributes.Add("style", "margin-left: 10px;")
                End If
            Next
        Next
    End Sub
    Sub SeguridadLoad()
        Dim IsAdm As Boolean = If(New ArrayList({"1", "2"}).IndexOf(ObjUser.CveRol.ToString) >= 0, True, False)
        Dim IsEditable As Boolean = If(New ArrayList({"1"}).IndexOf(CveEstatus.Text) >= 0, True, False)
        Dim IsAutor = If(Autor.Text = ObjUser.CodUsuario, True, False)
        Acciones(True, True, "0,1")
        'Controles(False)
        'If regPId.Text = "0" And DDLOrigen.SelectedValue.ToString = "" Then 'Blanco
        '    Acciones(True, True, "0")
        '    Controles(True, "1,2")
        'ElseIf regPId.Text = "0" And DDLOrigen.SelectedValue.ToString = "2" Then    'OET
        '    Acciones(True, True, "0,1")
        '    Controles(True)
        'ElseIf regPId.Text = "0" And DDLOrigen.SelectedValue.ToString = "3" Then    'NUKAXAN
        '    Acciones(True, True, "0,1")
        '    Controles(False, "3,4")
        'Else
        '    Acciones(True, True, "0")
        '    Controles(False)
        'End If

    End Sub
    Sub LlenaRegistro()
        Try
            If regPId.Text <> "0" Then
                Dim ObjM As ClientesModel = New Clientes().FindById(regPId.Text)
                TBNombreTitulo.Text = ObjM.NomClienteR
                LlenaRPT_Plataformas()
            End If
            SeguridadLoad()
        Catch ex As Exception
            Alertas("", CleanSpecialCharacter(ex.Message), 1, 4)
        End Try

    End Sub
    Sub LlenaRPT_Plataformas()
        Try
            rptPlataformas.DataSource = New Clientes_Plataformas().FindAll(regPId.Text)
            rptPlataformas.DataBind()
        Catch ex As Exception
            rptPlataformas.DataSource = Nothing
            rptPlataformas.DataBind()
            Alertas("", CleanSpecialCharacter(ex.Message), 1, 4)
        End Try
    End Sub
    Protected Sub RPTOnItemDataBound(ByVal sender As Object, ByVal e As RepeaterItemEventArgs) Handles rptPlataformas.ItemDataBound
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim item As RepeaterItem = e.Item
            Dim IsAdm As Boolean = If(New ArrayList({"1", "2"}).IndexOf(ObjUser.CveRol.ToString) >= 0, True, False)
            Dim CvePlataforma As String = (TryCast(item.FindControl("CvePlataforma"), Label)).Text
            Dim chk As CheckBox = (TryCast(item.FindControl("chk"), CheckBox))
            Dim Habilitada As String = (TryCast(item.FindControl("Habilitada"), Label)).Text
            Dim Dependencias As String = (TryCast(item.FindControl("Dependencias"), Label)).Text
            Dim CveEstatus As String = (TryCast(item.FindControl("CveEstatus"), Label)).Text
            Dim DDLEstatus As DropDownList = (TryCast(item.FindControl("DDLEstatus"), DropDownList))
            Call New Catalogos().LlenaEstatusGeneral(DDLEstatus)
            chk.Checked = If(Habilitada = "1", True, False)
            DDLEstatus.SelectedValue = If(CveEstatus = "0", "", CveEstatus)


            If IsAdm Then
                chk.Enabled = If(Dependencias = "S", False, True)
                DDLEstatus.Enabled = If(Habilitada = "1", True, False)
            ElseIf CvePlataforma = "2" Then
                chk.Enabled = If(Dependencias = "S", False, True)
                DDLEstatus.Enabled = If(Habilitada = "1", True, False)
            Else
                chk.Enabled = False
                DDLEstatus.Enabled = False
            End If

        End If
    End Sub

    '--Acciones---
    Sub Regresar()
        Response.Redirect(New RedirectPaginas().FindById(Plataforma + "-" + menu + "-0").PaginaURL.Replace("@filtro", Codif(filtroview.Text)).Replace("@pageIndex", gvindexpage.Text), True)
    End Sub
    Sub Refrescar()
        Response.Redirect(New RedirectPaginas().FindById(Plataforma + "-" + menu + "-1-2").PaginaURL.Replace("@Id", Codif(regPId.Text)).Replace("@filtro", Codif(filtroview.Text)).Replace("@pageIndex", gvindexpage.Text), True)
    End Sub
    Function Valida() As Boolean
        Dim IsResult As Boolean = False
        Try
            If msg = "" Then
                If ValidRPTChkCaptura(rptPlataformas, "chk", iList) Then msg = New Mensajes().FindById("0", 0, 47).NomMensaje
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
            If Valida() = False Then Return False
            Dim valores As String = GetRPTChkCaptura(rptPlataformas, "chk", iList)

            Dim ObjM As New Clientes_Plataformas()
            IsResult = ObjM.SaveModel(regPId.Text, valores, ObjUser.userId)
            mpe_op.Text = ""
            If IsResult Then
                Alertas("", "6", True, 2)
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
        Try
            IsResult = GuardaDatos()
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
            Case Else
                MPEAlerta.Hide()
        End Select
    End Sub

    Protected Sub OnCheckedChanged(sender As Object, e As EventArgs)
        Dim chk As CheckBox = TryCast(sender, CheckBox)
        Dim item As RepeaterItem = CType(chk.NamingContainer, RepeaterItem)
        Dim DDLEstatus As DropDownList = (TryCast(item.FindControl("DDLEstatus"), DropDownList))
        DDLEstatus.SelectedValue = If(chk.Checked, "1", "")
        DDLEstatus.Enabled = If(chk.Checked, True, False)
    End Sub
    Protected Sub OnCheckedAllChanged(sender As Object, e As EventArgs)
        Dim chkAll As CheckBox = TryCast(sender, CheckBox)
        rptPlataformas.Items.Cast(Of RepeaterItem)().ToList.ForEach(Sub(p)
                                                                        Dim chk As CheckBox = TryCast(p.FindControl("chk"), CheckBox)
                                                                        Dim Dependencias As String = (TryCast(p.FindControl("Dependencias"), Label)).Text
                                                                        Dim dd As DropDownList = TryCast(p.FindControl("DDLEstatus"), DropDownList)
                                                                        If Dependencias = "N" Then
                                                                            chk.Checked = chkAll.Checked
                                                                            dd.SelectedValue = If(chkAll.Checked, "1", "")
                                                                            dd.Enabled = If(chkAll.Checked, True, False)
                                                                        End If
                                                                    End Sub)
    End Sub
    Public Overrides Sub VerifyRenderingInServerForm(control As Control)
        ' Verifies that the control is rendered
    End Sub
End Class