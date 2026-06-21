Imports System
Imports System.Data
Imports System.Drawing
Imports System.Security.Policy
Imports System.Web.Services.Description
Imports AjaxControlToolkit
Imports NukaxanWEB

Public Class Libreria
    Public Shared defaultoption As String = "--Selecciona una opción--"
    Public Shared defaultoption2 As String = "--Todos--"
    Public Shared strError As String = ""
    Public Class DatosGrid
        Public Property Tipo As Integer = 0
        Public Property Campo As String = ""
        Public Property Valida As String = "S"
        Public Property ValidaCeros As String = "N"
        Public Property IsDate As String = "N"
    End Class
    Public Class WSErrorModel
        Public Property Message As String
        Public Property ExceptionMessage As String
        Public Property ExceptionType As String
        Public Property StackTrace As String
    End Class
    Public Class WSUsuarioModel
        Public Property email As String
        Public Property password As String
    End Class
    Public Class WSTokenModel
        Public Property token As String
    End Class
    Public Class WSParametrosModel
        Public Property nutrientcode As String
        Public Property nutrientvalue As Double
    End Class
    Public Class FrmControlModel
        Public Property Nombre As String
        Public Property Tipo As Integer
    End Class
    Public Enum BootstrapAlertType
        Plain
        Information
        Success
        Warning
        Danger
        Primary
        Secundary
    End Enum
    Public Shared Sub ModalAlert(MPEA As ModalPopupExtender, MPEBody As Literal, BAlertOK As Button, BAlertCancel As Button, Titulo As String, Mensaje As String, Refrescar As Boolean, Optional MessageType As BootstrapAlertType = BootstrapAlertType.Plain)
        Dim bestilo As String, bicono As String, bleyenda As String
        BAlertOK.Visible = False
        BAlertCancel.Visible = False
        BAlertOK.CommandArgument = If(Refrescar, "alert_refresh", "alert_close")
        BAlertOK.Text = New Controles_Acciones().FindById(1, 9).NomAccion
        BAlertOK.ToolTip = New Controles_Acciones().FindById(1, 9).ToolTip

        BAlertCancel.CommandArgument = "alert_close"
        BAlertCancel.Text = New Controles_Acciones().FindById(1, 10).NomAccion
        BAlertCancel.ToolTip = New Controles_Acciones().FindById(1, 10).ToolTip
        BAlertCancel.CssClass = "btn btn-danger"

        Select Case MessageType
            Case BootstrapAlertType.Plain
                bestilo = "default"
                bicono = ""
                bleyenda = ""
            Case BootstrapAlertType.Information
                bestilo = "info"
                bicono = "bi bi-info-circle-fill"
                bleyenda = "Notificación"
                BAlertOK.Visible = True
            Case BootstrapAlertType.Success
                bestilo = "success"
                bicono = "bi bi-check-circle-fill"
                bleyenda = "Confirmación"
                BAlertOK.Visible = True
            Case BootstrapAlertType.Warning
                bestilo = "warning"
                bicono = "bi bi-exclamation-triangle-fill"
                bleyenda = "Atención"
                BAlertOK.Visible = True
            Case BootstrapAlertType.Danger
                bestilo = "danger"
                bicono = "bi bi-exclamation-triangle-fill"
                bleyenda = "Error"
                BAlertOK.Visible = True
            Case BootstrapAlertType.Primary
                bestilo = "primary"
                bicono = "bi bi-info-circle-fill"
                bleyenda = ""
                BAlertOK.Visible = True
            Case BootstrapAlertType.Secundary
                bestilo = "primary"
                bicono = "bi bi-question-circle-fill"
                bleyenda = ""

                BAlertOK.Visible = True
                BAlertCancel.Visible = True
                BAlertOK.CommandArgument = "alert_ok"
                BAlertCancel.CommandArgument = "alert_cancel"
                BAlertOK.Text = New Controles_Acciones().FindById(1, 9).NomAccion
                BAlertOK.ToolTip = New Controles_Acciones().FindById(1, 9).ToolTip
                BAlertCancel.Text = New Controles_Acciones().FindById(1, 10).NomAccion
                BAlertCancel.ToolTip = New Controles_Acciones().FindById(1, 10).ToolTip
        End Select
        BAlertOK.CssClass = "btn btn-" + bestilo
        Dim tmp As String = "
                <div class='alert-header alert alert-" + bestilo + "' >                    
                    <i class='" + bicono + " alert-icono' ></i>
                    <span>" + bleyenda + If(Titulo = "", "", " " + Titulo) + "</span>                
               </div>
               <div class='alert-body'>" + Mensaje + "</div>"
        MPEBody.Text = tmp
        MPEA.Show()
    End Sub
    Public Shared Function CleanSpecialCharacter(strtexto As String) As String
        Dim strSpecialCharacters() As String = {"~", "(", ")", "#", "\", "/", "=", ">", "<", "+", "*", "%", "&", "|", "^", "'", """", vbLf, vbCr}
        For Each strSpecialChar As String In strSpecialCharacters
            strtexto = strtexto.Replace(strSpecialChar, "")
        Next
        Return strtexto
    End Function
    Public Shared Function Codif(ByVal strCadena As String) As String
        Try
            Codif = strCadena
            Dim uEncode As New UnicodeEncoding()
            Dim bytSource() As Byte = System.Text.UnicodeEncoding.UTF8.GetBytes(strCadena)

            strCadena = Convert.ToBase64String(bytSource)

            Dim strCadenaNueva As String = ""
            Dim ch As Char
            For Each ch In strCadena.ToCharArray
                strCadenaNueva += ch & Microsoft.VisualBasic.Left(System.Guid.NewGuid().ToString().Replace("-", ""), 1)
            Next
            Codif = strCadenaNueva
        Catch exSecPP As Exception
            Throw exSecPP
        End Try
    End Function
    Public Shared Function DeCodif(ByVal strCadena As String) As String
        DeCodif = strCadena
        Dim ch As Char, copia As Boolean = True
        Dim strCadenaNueva As String = ""
        For Each ch In strCadena.ToCharArray
            If copia Then
                strCadenaNueva += ch
            End If
            copia = Not copia
        Next
        Dim uEncode As New UnicodeEncoding()
        Dim bytSource() As Byte = Convert.FromBase64String(strCadenaNueva)
        strCadenaNueva = System.Text.UTF8Encoding.UTF8.GetChars(bytSource)
        DeCodif = strCadenaNueva
    End Function
    Public Shared Function EvaluateExpression(eqn As String) As Object
        Dim dt As New DataTable()
        Dim result = dt.Compute(eqn, String.Empty)
        Return result
    End Function
    Public Shared Sub subControl_fill(ddl_control As Control, dt As DataTable, fieldkey As String, fieldname As String, optioncero As Boolean)
        Try
            If ddl_control.GetType.Name = "DropDownList" Then
                Dim ddl_control2 As DropDownList = ddl_control
                ddl_control2.SelectedIndex = -1
                ddl_control2.Items.Clear()
                ddl_control2.DataSource = dt
                ddl_control2.DataValueField = fieldkey
                ddl_control2.DataTextField = fieldname
                ddl_control2.DataBind()
                If optioncero Then
                    ddl_control2.Items.Insert(0, New ListItem(defaultoption, ""))
                    ddl_control2.SelectedIndex = 0
                End If
            ElseIf ddl_control.GetType.Name = "DropDownCheckBoxes" Then
                Dim ddl_control2 As Saplin.Controls.DropDownCheckBoxes = ddl_control
                ddl_control2.SelectedIndex = -1
                ddl_control2.Items.Clear()
                ddl_control2.DataSource = dt
                ddl_control2.DataValueField = fieldkey
                ddl_control2.DataTextField = fieldname
                ddl_control2.DataBind()
                If optioncero Then
                    ddl_control2.Texts.SelectBoxCaption = defaultoption
                    'ddl_control2.Items.Insert(0, New ListItem(defaultoption, ""))
                    'ddl_control2.SelectedIndex = 0
                End If
            ElseIf ddl_control.GetType.Name = "ComboBox" Then
                Dim ddl_control2 As AjaxControlToolkit.ComboBox = ddl_control
                ddl_control2.Items.Clear()
                ddl_control2.SelectedIndex = -1
                ddl_control2.DataSource = dt
                ddl_control2.DataValueField = fieldkey
                ddl_control2.DataTextField = fieldname
                ddl_control2.DataBind()

                If optioncero Then
                    ddl_control2.Items.Insert(0, New ListItem(defaultoption, ""))
                    ddl_control2.SelectedIndex = 0
                End If

            ElseIf ddl_control.GetType.Name = "CheckBoxList" Then
                Dim ddl_control2 As CheckBoxList = ddl_control
                ddl_control2.SelectedIndex = -1
                'ddl_control2.Items.Clear()
                ddl_control2.DataSource = dt
                ddl_control2.DataValueField = fieldkey
                ddl_control2.DataTextField = fieldname
                ddl_control2.DataBind()

                If optioncero Then
                    ddl_control2.Items.Insert(0, New ListItem(defaultoption, ""))
                    ddl_control2.SelectedIndex = 0
                End If
            ElseIf ddl_control.GetType.Name = "RadioButtonList" Then
                Dim ddl_control2 As RadioButtonList = ddl_control
                ddl_control2.SelectedIndex = -1
                'ddl_control2.Items.Clear()
                ddl_control2.DataSource = dt
                ddl_control2.DataValueField = fieldkey
                ddl_control2.DataTextField = fieldname
                ddl_control2.DataBind()

                If optioncero Then
                    ddl_control2.Items.Insert(0, New ListItem(defaultoption, ""))
                    ddl_control2.SelectedIndex = 0
                End If
            End If
        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)

        Finally
            dt = Nothing
        End Try
    End Sub
    Shared Function ValidRPTCaptura(Rpt As Repeater, Valores As List(Of DatosGrid)) As Boolean
        Dim valida_row As Boolean = False
        Dim valida_campo As Boolean = False
        Try
            Rpt.Items.Cast(Of RepeaterItem)().ToList.ForEach(Sub(p)
                                                                 If valida_row Then Exit Sub
                                                                 Valores.Where(Function(f) f.Valida = "S").ToList().ForEach(Sub(c)
                                                                                                                                If valida_campo Then Exit Sub
                                                                                                                                Dim tmpvalor As String = ""
                                                                                                                                If c.Tipo = 0 Then tmpvalor = TryCast(p.FindControl(c.Campo), Label).Text
                                                                                                                                If c.Tipo = 1 Then tmpvalor = TryCast(p.FindControl(c.Campo), TextBox).Text
                                                                                                                                If c.Tipo = 2 Then tmpvalor = TryCast(p.FindControl(c.Campo), DropDownList).SelectedValue
                                                                                                                                If c.Tipo = 21 Then tmpvalor = TryCast(p.FindControl(c.Campo), DropDownList).SelectedItem.Text
                                                                                                                                If tmpvalor = "" Then valida_campo = True
                                                                                                                                If tmpvalor = "" Then valida_row = True
                                                                                                                                If tmpvalor = "0" And c.ValidaCeros = "S" Then valida_campo = True
                                                                                                                                If tmpvalor = "0" And c.ValidaCeros = "S" Then valida_row = True
                                                                                                                            End Sub)

                                                             End Sub)

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        End Try
        Return valida_row
    End Function
    Shared Function ValidRPTChkCaptura(Rpt As Repeater, chkName As String, Valores As List(Of DatosGrid)) As Boolean
        Dim IsResult As Boolean = False
        Dim valida_row As Boolean = False
        Dim valida_campo As Boolean = False
        Try
            Rpt.Items.Cast(Of RepeaterItem)().ToList.ForEach(Sub(p)
                                                                 Dim tmp As String = ""
                                                                 Dim chk As CheckBox = TryCast(p.FindControl(chkName), CheckBox)
                                                                 If valida_row Or chk.Checked = False Then Exit Sub
                                                                 Valores.Where(Function(f) f.Valida = "S").ToList().ForEach(Sub(c)
                                                                                                                                If valida_campo Then Exit Sub
                                                                                                                                Dim tmpvalor As String = ""
                                                                                                                                If c.Tipo = 0 Then tmpvalor = TryCast(p.FindControl(c.Campo), Label).Text
                                                                                                                                If c.Tipo = 1 Then tmpvalor = TryCast(p.FindControl(c.Campo), TextBox).Text
                                                                                                                                If c.Tipo = 2 Then tmpvalor = TryCast(p.FindControl(c.Campo), DropDownList).SelectedValue
                                                                                                                                If c.Tipo = 21 Then tmpvalor = TryCast(p.FindControl(c.Campo), DropDownList).SelectedItem.Text
                                                                                                                                If tmpvalor = "" Then valida_campo = True
                                                                                                                                If tmpvalor = "" Then valida_row = True
                                                                                                                                If tmpvalor = "0" And c.ValidaCeros = "S" Then valida_campo = True
                                                                                                                                If tmpvalor = "0" And c.ValidaCeros = "S" Then valida_row = True
                                                                                                                            End Sub)

                                                             End Sub)


        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        End Try
        Return valida_row
    End Function
    Public Shared Function GetRPTCaptura(Rpt As Repeater, Valores As List(Of DatosGrid)) As String
        Dim lstValores As New List(Of String)
        Dim tmp_result As String = ""
        Try
            Rpt.Items.Cast(Of RepeaterItem)().ToList.ForEach(Sub(p)
                                                                 Dim tmp As String = ""
                                                                 Valores.ForEach(Sub(c)
                                                                                     If c.Tipo = 0 Then tmp += TryCast(p.FindControl(c.Campo), Label).Text + "#"
                                                                                     If c.Tipo = 1 And c.IsDate = "N" Then tmp += TryCast(p.FindControl(c.Campo), TextBox).Text + "#"
                                                                                     If c.Tipo = 1 And c.IsDate = "S" Then tmp += CDate(TryCast(p.FindControl(c.Campo), TextBox).Text).ToString("yyyy-MM-dd") + "#"
                                                                                     If c.Tipo = 2 Then tmp += TryCast(p.FindControl(c.Campo), DropDownList).SelectedValue + "#"
                                                                                     If c.Tipo = 21 Then tmp += TryCast(p.FindControl(c.Campo), DropDownList).SelectedItem.Text + "#"

                                                                                 End Sub)
                                                                 tmp = If(tmp <> "", Left(tmp, Len(tmp) - 1), "")
                                                                 lstValores.Add(tmp)
                                                             End Sub)

            tmp_result = String.Join("|", String.Join("|", lstValores.ToArray().Distinct).Split("|").Distinct)

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        End Try
        Return tmp_result
    End Function
    Public Shared Function GetRPTChkCaptura(Rpt As Repeater, chkName As String, Valores As List(Of DatosGrid)) As String
        Dim lstValores As New List(Of String)
        Dim tmp_result As String = ""
        Try
            Rpt.Items.Cast(Of RepeaterItem)().ToList.ForEach(Sub(p)
                                                                 Dim chk As CheckBox = TryCast(p.FindControl(chkName), CheckBox)
                                                                 Dim tmp As String = ""
                                                                 If chk.Checked = False Then Exit Sub
                                                                 Valores.ForEach(Sub(c)
                                                                                     If c.Campo = "Aplica" Then TryCast(p.FindControl(c.Campo), Label).Text = If(chk.Checked, "S", "N")
                                                                                     If c.Tipo = 0 Then tmp += TryCast(p.FindControl(c.Campo), Label).Text + "#"
                                                                                     If c.Tipo = 1 And c.IsDate = "N" Then tmp += TryCast(p.FindControl(c.Campo), TextBox).Text + "#"
                                                                                     If c.Tipo = 1 And c.IsDate = "S" Then tmp += CDate(TryCast(p.FindControl(c.Campo), TextBox).Text).ToString("yyyy-MM-dd") + "#"
                                                                                     If c.Tipo = 2 Then tmp += TryCast(p.FindControl(c.Campo), DropDownList).SelectedValue + "#"
                                                                                     If c.Tipo = 21 Then tmp += TryCast(p.FindControl(c.Campo), DropDownList).SelectedItem.Text + "#"
                                                                                     'If c.Tipo = 3 Then tmp += TryCast(p.FindControl(c.Campo), Label).Text + "#"

                                                                                 End Sub)
                                                                 tmp = If(tmp <> "", Left(tmp, Len(tmp) - 1), "")
                                                                 lstValores.Add(tmp)
                                                             End Sub)

            tmp_result = String.Join("|", lstValores.ToArray())

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        End Try
        Return tmp_result
    End Function
    Public Shared Function GetRPTChkCapturaAll(Rpt As Repeater, chkName As String, Valores As List(Of DatosGrid)) As String
        Dim lstValores As New List(Of String)
        Dim tmp_result As String = ""
        Try
            Rpt.Items.Cast(Of RepeaterItem)().ToList.ForEach(Sub(p)
                                                                 Dim chk As CheckBox = TryCast(p.FindControl(chkName), CheckBox)
                                                                 Dim tmp As String = ""
                                                                 Valores.ForEach(Sub(c)
                                                                                     If c.Campo = "Aplica" Then TryCast(p.FindControl(c.Campo), Label).Text = If(chk.Checked, "S", "N")
                                                                                     If c.Tipo = 0 Then tmp += TryCast(p.FindControl(c.Campo), Label).Text + "#"
                                                                                     If c.Tipo = 1 And c.IsDate = "N" Then tmp += TryCast(p.FindControl(c.Campo), TextBox).Text + "#"
                                                                                     If c.Tipo = 1 And c.IsDate = "S" Then tmp += CDate(TryCast(p.FindControl(c.Campo), TextBox).Text).ToString("yyyy-MM-dd") + "#"
                                                                                     If c.Tipo = 2 Then tmp += TryCast(p.FindControl(c.Campo), DropDownList).SelectedValue + "#"
                                                                                     If c.Tipo = 21 Then tmp += TryCast(p.FindControl(c.Campo), DropDownList).SelectedItem.Text + "#"
                                                                                     'If c.Tipo = 3 Then tmp += TryCast(p.FindControl(c.Campo), Label).Text + "#"

                                                                                 End Sub)
                                                                 tmp = If(tmp <> "", Left(tmp, Len(tmp) - 1), "")
                                                                 lstValores.Add(tmp)
                                                             End Sub)

            tmp_result = String.Join("|", lstValores.ToArray())

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        End Try
        Return tmp_result
    End Function
    Shared Function GetValueDropDownCheckBoxes(DDL As Saplin.Controls.DropDownCheckBoxes, Optional op As Integer = 0) As String
        Dim str_result As String = ""
        Try
            If DDL Is Nothing Then Return ""
            If DDL.Items.Count = 0 Then Return ""

            If op = 0 Then str_result = String.Join(",", (DDL.Items.Cast(Of ListItem)().Where(Function(i) i.Selected).[Select](Function(i) i.Value)).ToList())
            If op = 1 Then str_result = String.Join(",", (DDL.Items.Cast(Of ListItem)().Where(Function(i) i.Selected).[Select](Function(i) i.Value)).ToList())
            If op = 2 Then str_result = "'" + String.Join("','", (DDL.Items.Cast(Of ListItem)().Where(Function(i) i.Selected).[Select](Function(i) i.Value)).ToList()) + "'"
            If op = 3 Then str_result = String.Join(",", (DDL.Items.Cast(Of ListItem)().Where(Function(i) i.Selected).[Select](Function(i) i.Text)).ToList())
            Return str_result
        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
            Return ""
        End Try

    End Function
    Public Shared Function GoogleLineChart(titulo As String, datoschart As String, serieline As String) As String
        Dim sbChart As New StringBuilder("<script type = ""text/javascript"" >")
        sbChart.Append("google.charts.load('current', {'packages':['corechart']});")
        sbChart.Append("google.charts.setOnLoadCallback(drawChart);")
        sbChart.Append("function drawChart() {")
        sbChart.Append("var options = { ")
        sbChart.Append("title:'" + titulo + "',")
        sbChart.Append("vAxis: {title: 'Total Meses'},")
        sbChart.Append("hAxis: {title: 'Periodo'},")
        sbChart.Append("seriesType: 'bars',")
        sbChart.Append("series: {" + serieline + ": {type: 'line'}}")
        sbChart.Append("};")
        sbChart.Append("var data = google.visualization.arrayToDataTable([")
        sbChart.Append(datoschart)
        sbChart.Append("]);")
        sbChart.Append("var chart = new google.visualization.ComboChart(document.getElementById('linechart'));")
        sbChart.Append("chart.draw(data, options);")
        sbChart.Append("}")
        sbChart.Append("</script>")
        sbChart.Append("<div id='linechart' style='width:  900px; height: 500px;'></div>")

        Return sbChart.ToString
    End Function
    Public Shared Sub SeguridadRPT(IsVisible As Boolean, rpt As Repeater, lstcontroles As List(Of FrmControlModel))
        Try
            rpt.Items.Cast(Of RepeaterItem)().ToList.ForEach(Sub(p)
                                                                 Dim chk As CheckBox = TryCast(p.FindControl(lstcontroles.Find(Function(x) x.Tipo = 0).Nombre), CheckBox)
                                                                 Dim Ischecked As Boolean = True
                                                                 If Not IsNothing(chk) Then
                                                                     Ischecked = chk.Checked
                                                                     chk.Visible = True
                                                                     chk.Enabled = IsVisible
                                                                 End If
                                                                 lstcontroles.FindAll(Function(x) x.Tipo > 0).ForEach(Sub(c)
                                                                                                                          Dim obj As Object
                                                                                                                          Dim obj2 As Object

                                                                                                                          If c.Tipo = 1 Then obj = TryCast(p.FindControl(c.Nombre), Label)
                                                                                                                          If c.Tipo = 2 Then obj = TryCast(p.FindControl(c.Nombre), TextBox)
                                                                                                                          If c.Tipo = 2 Then obj2 = TryCast(p.FindControl(c.Nombre + "D"), Label)
                                                                                                                          If c.Tipo = 3 Then obj = TryCast(p.FindControl(c.Nombre), DropDownList)
                                                                                                                          If c.Tipo = 4 Then obj = TryCast(p.FindControl(c.Nombre), ComboBox)
                                                                                                                          If c.Tipo = 5 Then obj = TryCast(p.FindControl(c.Nombre), ImageButton)
                                                                                                                          If c.Tipo = 6 Then obj = TryCast(p.FindControl(c.Nombre), LinkButton)

                                                                                                                          If Not IsNothing(obj) Then obj.Visible = IsVisible
                                                                                                                          If Not IsNothing(obj2) Then obj2.Visible = Not IsVisible

                                                                                                                          If Not IsNothing(obj) Then obj.Enabled = If(Ischecked, IsVisible, Not IsVisible)
                                                                                                                          If Not IsNothing(obj2) Then obj2.Enabled = If(Ischecked, Not IsVisible, IsVisible)
                                                                                                                          If Not IsNothing(obj2) And Not Ischecked Then obj2.Attributes.Add("style", "color:#d5d3d3;")

                                                                                                                      End Sub)
                                                             End Sub)
        Catch ex As Exception

        End Try

    End Sub
    Public Shared Sub SeguridadRPT3(IsVisible As Boolean, rpt As Repeater, lstcontroles As List(Of FrmControlModel))
        Try
            rpt.Items.Cast(Of RepeaterItem)().ToList.ForEach(Sub(p)
                                                                 Dim chk As CheckBox = TryCast(p.FindControl(lstcontroles.Find(Function(x) x.Tipo = 0).Nombre), CheckBox)
                                                                 Dim fija As String = TryCast(p.FindControl(lstcontroles.Find(Function(x) x.Tipo = -1).Nombre), Label).Text
                                                                 Dim Ischecked As Boolean = True
                                                                 If Not IsNothing(chk) Then
                                                                     Ischecked = chk.Checked
                                                                     chk.Visible = True
                                                                     chk.Enabled = If(fija, False, IsVisible)
                                                                 End If
                                                                 lstcontroles.FindAll(Function(x) x.Tipo > 0).ForEach(Sub(c)
                                                                                                                          Dim obj As Object
                                                                                                                          Dim obj2 As Object

                                                                                                                          If c.Tipo = 1 Then obj = TryCast(p.FindControl(c.Nombre), Label)
                                                                                                                          If c.Tipo = 2 Then obj = TryCast(p.FindControl(c.Nombre), TextBox)
                                                                                                                          If c.Tipo = 2 Then obj2 = TryCast(p.FindControl(c.Nombre + "D"), Label)
                                                                                                                          If c.Tipo = 3 Then obj = TryCast(p.FindControl(c.Nombre), DropDownList)
                                                                                                                          If c.Tipo = 4 Then obj = TryCast(p.FindControl(c.Nombre), ComboBox)
                                                                                                                          If c.Tipo = 5 Then obj = TryCast(p.FindControl(c.Nombre), ImageButton)
                                                                                                                          If c.Tipo = 6 Then obj = TryCast(p.FindControl(c.Nombre), LinkButton)

                                                                                                                          If Not IsNothing(obj) Then obj.Visible = IsVisible
                                                                                                                          If Not IsNothing(obj2) Then obj2.Visible = Not IsVisible

                                                                                                                          If Not IsNothing(obj) Then obj.Enabled = If(Ischecked, IsVisible, Not IsVisible)
                                                                                                                          If Not IsNothing(obj2) Then obj2.Enabled = If(Ischecked, Not IsVisible, IsVisible)
                                                                                                                          If Not IsNothing(obj2) And Not Ischecked Then obj2.Attributes.Add("style", "color:#d5d3d3;")

                                                                                                                      End Sub)
                                                             End Sub)
        Catch ex As Exception

        End Try

    End Sub
    Shared Sub SeguridadRPT2(rpt As Repeater, lstAcciones As List(Of String), lstCondicion As List(Of String), lstUsuario As List(Of String), op As Boolean, Optional pnl As String = "")
        rpt.Items.Cast(Of RepeaterItem)().ToList.ForEach(Sub(p)
                                                             For Each item As String In lstAcciones
                                                                 If op = False Then
                                                                     TryCast(p.FindControl(item), ImageButton).Visible = op
                                                                     If pnl <> "" Then TryCast(p.FindControl(pnl), Panel).Visible = op
                                                                 ElseIf op And lstCondicion IsNot Nothing And lstUsuario Is Nothing Then
                                                                     Dim IsCondicion As Boolean = If(TryCast(p.FindControl(lstCondicion(0)), Label).Text = lstCondicion(1), True, False)
                                                                     TryCast(p.FindControl(item), ImageButton).Visible = IsCondicion
                                                                     If pnl <> "" Then TryCast(p.FindControl(pnl), Panel).Visible = op
                                                                 ElseIf op And lstCondicion Is Nothing And lstUsuario IsNot Nothing Then
                                                                     Dim IsAutor As Boolean = If(TryCast(p.FindControl(lstUsuario(0)), Label).Text = lstUsuario(1), True, False)
                                                                     TryCast(p.FindControl(item), ImageButton).Visible = IsAutor
                                                                     If pnl <> "" Then TryCast(p.FindControl(pnl), Panel).Visible = op
                                                                 ElseIf op And lstCondicion IsNot Nothing And lstUsuario IsNot Nothing Then
                                                                     Dim IsCondicion As Boolean = If(TryCast(p.FindControl(lstCondicion(0)), Label).Text = lstCondicion(1), True, False)
                                                                     Dim IsAutor As Boolean = If(TryCast(p.FindControl(lstUsuario(0)), Label).Text = lstUsuario(1), True, False)
                                                                     TryCast(p.FindControl(item), ImageButton).Visible = If(IsCondicion And IsAutor, op, Not op)
                                                                     If pnl <> "" Then TryCast(p.FindControl(pnl), Panel).Visible = op
                                                                 Else
                                                                     TryCast(p.FindControl(item), ImageButton).Visible = op
                                                                     If pnl <> "" Then TryCast(p.FindControl(pnl), Panel).Visible = op
                                                                 End If

                                                             Next

                                                         End Sub)
    End Sub
    Shared Sub SeguridadGV(gv As GridView, lstAcciones As List(Of String), lstCondicion As List(Of String), lstUsuario As List(Of String), op As Boolean)
        If gv Is Nothing Then Exit Sub
        gv.Rows.Cast(Of GridViewRow)().ToList.ForEach(Sub(p)
                                                          For Each item As String In lstAcciones
                                                              If op = False Then
                                                                  TryCast(p.FindControl(item), ImageButton).Visible = op

                                                              ElseIf op And lstCondicion IsNot Nothing And lstUsuario Is Nothing Then
                                                                  Dim IsCondicion As Boolean = If(TryCast(p.FindControl(lstCondicion(0)), Label).Text = lstCondicion(1), True, False)
                                                                  TryCast(p.FindControl(item), ImageButton).Visible = IsCondicion

                                                              ElseIf op And lstCondicion Is Nothing And lstUsuario IsNot Nothing Then
                                                                  Dim IsAutor As Boolean = If(TryCast(p.FindControl(lstUsuario(0)), Label).Text = lstUsuario(1), True, False)
                                                                  TryCast(p.FindControl(item), ImageButton).Visible = IsAutor

                                                              ElseIf op And lstCondicion IsNot Nothing And lstUsuario IsNot Nothing Then
                                                                  Dim IsCondicion As Boolean = If(TryCast(p.FindControl(lstCondicion(0)), Label).Text = lstCondicion(1), True, False)
                                                                  Dim IsAutor As Boolean = If(TryCast(p.FindControl(lstUsuario(0)), Label).Text = lstUsuario(1), True, False)
                                                                  TryCast(p.FindControl(item), ImageButton).Visible = If(IsCondicion And IsAutor, op, Not op)

                                                              Else
                                                                  TryCast(p.FindControl(item), ImageButton).Visible = op

                                                              End If

                                                          Next
                                                      End Sub)
    End Sub

    Shared Function GetCommaSeparatedListDT(Dt As DataTable, campo As String, Optional op As Integer = 0) As String
        Dim str_result As String = ""
        Try
            If Dt Is Nothing Then Return ""
            If Dt.Rows.Count = 0 Then Return ""

            Dim lst = Dt.AsEnumerable().Select(Function(x) x(campo).ToString).Distinct().ToList()
            If op = 0 Then str_result = String.Join(",", String.Join(",", lst.ToArray().Distinct).Split(",").Distinct)
            If op = 1 Then str_result = "'" + String.Join("','", String.Join(",", lst.ToArray().Distinct).Split(",").Distinct) + "'"
            Return str_result
        Catch ex As Exception
            'strError2 = ex.Message.Replace("'", "").Replace(vbLf, "").Replace(vbCr, "")
            Return ""
        End Try

    End Function
    Shared Function GetCommaSeparatedListRPT(Rpt As Repeater, campo As String, Optional op As Integer = 0) As String
        Dim str_result As String = ""
        Try
            If Rpt Is Nothing Then Return ""
            If Rpt.Items.Count = 0 Then Return ""

            If op = 0 Then str_result = String.Join(",", (Rpt.Items.Cast(Of RepeaterItem)().Select(Function(a) TryCast(a.FindControl(campo), Label).Text).ToList().Distinct))
            If op = 1 Then str_result = "'" + String.Join("','", (Rpt.Items.Cast(Of RepeaterItem)().Select(Function(a) TryCast(a.FindControl(campo), Label).Text).ToList().Distinct)) + "'"
            Return str_result
        Catch ex As Exception
            'strError2 = ex.Message.Replace("'", "").Replace(vbLf, "").Replace(vbCr, "")
            Return ""
        End Try

    End Function
    Shared Function GetCommaSeparatedListCHK(ChkList As CheckBoxList, Optional op As Integer = 0) As String
        Dim str_result As String = ""
        Try
            If ChkList Is Nothing Then Return ""
            If ChkList.Items.Count = 0 Then Return ""

            If op = 0 Then str_result = String.Join(",", (ChkList.Items.Cast(Of ListItem)().Where(Function(i) i.Selected).[Select](Function(i) i.Value)).ToList())
            If op = 1 Then str_result = "'" + String.Join("','", (ChkList.Items.Cast(Of ListItem)().Where(Function(i) i.Selected).[Select](Function(i) i.Value)).ToList()) + "'"
            Return str_result
        Catch ex As Exception
            'strError2 = ex.Message.Replace("'", "").Replace(vbLf, "").Replace(vbCr, "")
            Return ""
        End Try

    End Function
    Shared Function GetCommaSeparatedDropDownCheckBoxes(DDL As Saplin.Controls.DropDownCheckBoxes, Optional op As Integer = 0) As String
        Dim str_result As String = ""
        Try
            If DDL Is Nothing Then Return ""
            If DDL.Items.Count = 0 Then Return ""

            If op = 0 Then str_result = String.Join(",", (DDL.Items.Cast(Of ListItem)().Where(Function(i) i.Selected).[Select](Function(i) i.Value)).ToList())
            If op = 1 Then str_result = "'" + String.Join("','", (DDL.Items.Cast(Of ListItem)().Where(Function(i) i.Selected).[Select](Function(i) i.Value)).ToList()) + "'"
            If op = 2 Then str_result = String.Join(",", (DDL.Items.Cast(Of ListItem)().Where(Function(i) i.Selected).[Select](Function(i) i.Text)).ToList())
            Return str_result
        Catch ex As Exception
            'strError2 = ex.Message.Replace("'", "").Replace(vbLf, "").Replace(vbCr, "")
            Return ""
        End Try

    End Function
    Shared Sub SetSelectedDropDownCheckBoxes(DDLControl As Saplin.Controls.DropDownCheckBoxes, Valores As String)
        Try
            Dim tmp() As String = Valores.Split("|")
            For i = 0 To UBound(tmp)
                DDLControl.Items.Cast(Of ListItem)().Where(Function(x) x.Value = tmp(i)).FirstOrDefault.Selected = True
            Next

        Catch ex As Exception
            'strError2 = ex.Message.Replace("'", "").Replace(vbLf, "").Replace(vbCr, "")
        End Try
    End Sub

    Public Shared Function ChartJSLineChart(titulo As String, xtitulo As String, ytitulo As String, series As String, datos As String) As String
        Dim sbChart As New StringBuilder("<script>")
        'sbChart.Append("const xValues = [55,65,70,80,90,100,110,20,130,140,150];")
        'sbChart.Append("const yValues = [7,8,8,9,9,1,10,11,14,14,15];")
        'sbChart.Append("const yValues2 = [17,18,18,19,19,11,10,11,19,19,21];")
        sbChart.Append("const titulo = '" + titulo + "';")
        sbChart.Append("const xtitulo = '" + xtitulo + "';")
        sbChart.Append("const ytitulo = '" + ytitulo + "';")
        'sbChart.Append("const datos = [{x:17,y0:25,y1:50,y2:5},{x:18,y0:15,y1:45,y2:25},{x:19,y0:17,y1:35,y2:47},{x:19,y0:35,y1:55,y2:65}];")
        sbChart.Append("const datos = [" + datos + "];")
        'azul,rojo,verde,amarillo,naranja,morado,violeta,cafe,gris
        sbChart.Append("const puntocolor = ['#0080ff30','#ff004030','#5cd65c30','#ffb84d30','#ff751a30','#4d4dff30','#a64dff30','#c68c5330','#C0C0C030'];")
        sbChart.Append("const linecolor = ['#0080ff','#ff0040','#5cd65c','#ffb84d','#ff751a','#4d4dff','#a64dff','#c68c53','#C0C0C0'];")
        sbChart.Append("const series = [" + series + "];")

        sbChart.Append("new Chart('myChart', {")
        sbChart.Append("type: 'line',")
        sbChart.Append("options: {")
        sbChart.Append("  responsive: true,")
        sbChart.Append("  title: {display: true,text:titulo},")
        sbChart.Append("  legend: {display: true,labels: {usePointStyle: false,boxWidth: 10}")
        sbChart.Append("    ,onHover: function (e) {e.target.style.cursor = 'pointer'}")
        sbChart.Append("    ,onLeave: function (e) {e.target.style.cursor = 'default'}")
        sbChart.Append("  },")
        sbChart.Append("  hover: {onHover: function(e) {var point = this.getElementAtEvent(e);")
        sbChart.Append("    if (point.length) e.target.style.cursor = 'pointer';")
        sbChart.Append("    else e.target.style.cursor = 'default';}")
        sbChart.Append("  },")
        sbChart.Append("  scales: { ")
        sbChart.Append("   xAxes:[{scaleLabel: {display: true,labelString: xtitulo}}],")
        sbChart.Append("   yAxes:[{scaleLabel: {display: true,labelString: ytitulo}}]")
        sbChart.Append("  },")

        sbChart.Append("  tooltips: {mode: 'point',intersect: false,")
        sbChart.Append("    callbacks: {")
        sbChart.Append("    title: function(tooltipItems, data) {return xtitulo+': ' + tooltipItems[0].xLabel;},")
        sbChart.Append("    label: function(tooltipItem, data) {return ' '+data.datasets[tooltipItem.datasetIndex].label+': '+tooltipItem.yLabel;}")
        sbChart.Append("    }")
        sbChart.Append("  }")
        sbChart.Append("},/*options*/")

        sbChart.Append("data: {labels: datos.map(o => o.x)")
        sbChart.Append("  ,datasets: Object.keys(datos[0])")
        sbChart.Append("  .filter(k => k != 'x')")
        sbChart.Append("  .map((k, i) => ({label: series[i],")
        sbChart.Append("    data: datos.map(o => o[k]),")
        sbChart.Append("    backgroundColor:puntocolor[i],")
        sbChart.Append("    borderColor: linecolor[i],")
        sbChart.Append("    showLine: true,fill: false,lineTension: 0,")
        sbChart.Append("   pointStyle: 'circle', pointRadius: 4,pointHoverRadius: 6")
        sbChart.Append("  }))")
        sbChart.Append("}/*data*/")

        'sbChart.Append("data: {")
        'sbChart.Append("  labels: xValues,")
        'sbChart.Append("  datasets: [")
        'sbChart.Append("    {label:'Serie1',fill: false,lineTension: 0,")
        'sbChart.Append("    backgroundColor: 'rgba(0, 0, 255, 1.0)',borderColor: 'rgba(0,0,255,0.1)',")
        'sbChart.Append("    data: yValues},")
        'sbChart.Append("    {label:'Serie2',fill: false,lineTension: 0,")
        'sbChart.Append("    borderColor: 'rgba(255, 0, 0, 0.2)',backgroundColor:'red',")
        'sbChart.Append("    data: yValues2}")
        'sbChart.Append("]}")

        sbChart.Append("});")
        sbChart.Append("</script>")

        Return sbChart.ToString
    End Function
    Public Shared Function GoogleLineChart(titulo As String, datoschart As String) As String
        Dim sbChart As New StringBuilder("<script type = ""text/javascript"" >")
        sbChart.Append("google.charts.load('current', {'packages':['line']});")
        sbChart.Append("google.charts.setOnLoadCallback(drawChart);")
        sbChart.Append("function drawChart() {")
        sbChart.Append("var options = { ")
        sbChart.Append("title:'" + titulo + "',")
        sbChart.Append("vAxis: {title: 'Total Meses'},")
        sbChart.Append("hAxis: {title: 'Edad'},")
        sbChart.Append("pointsVisible: true")
        sbChart.Append("};")

        sbChart.Append("var data = google.visualization.arrayToDataTable([")
        sbChart.Append(datoschart)
        sbChart.Append("]);")

        sbChart.Append("var chart = new google.visualization.LineChart(document.getElementById('line_chart'));")
        sbChart.Append("chart.draw(data, options);")
        sbChart.Append("}")
        sbChart.Append("</script>")
        sbChart.Append("<div id='line_chart' style='width: 700px; height: 500px;'></div>")

        Return sbChart.ToString
    End Function
    Public Shared Function GooglePieChart(titulo As String, datoschart As String) As String
        Dim sbChart As New StringBuilder("<script type = ""text/javascript"" >")
        sbChart.Append("google.charts.load('current', {'packages':['corechart']});")
        sbChart.Append("google.charts.setOnLoadCallback(drawChart);")
        sbChart.Append("function drawChart() {")
        sbChart.Append("var options = { title:'" + titulo + "'};")
        sbChart.Append("var data = google.visualization.arrayToDataTable([")
        sbChart.Append(datoschart)
        sbChart.Append("]);")
        sbChart.Append("var chart = new google.visualization.PieChart(document.getElementById('piechart'));")
        sbChart.Append("chart.draw(data, options);")
        sbChart.Append("}")
        sbChart.Append("</script>")
        sbChart.Append("<div id='piechart' style='width:  900px; height: 500px;'></div>")

        Return sbChart.ToString
    End Function
    Public Shared Function GoogleColumnasMaterialChart(titulo As String, datoschart As String) As String
        Dim sbChart As New StringBuilder("<script type = ""text/javascript"" >")
        sbChart.Append("google.charts.load('current', {'packages':['bar']});")
        sbChart.Append("google.charts.setOnLoadCallback(drawChart);")
        sbChart.Append("function drawChart() {")
        sbChart.Append("var options = { ")
        sbChart.Append("chart: {")
        sbChart.Append("title:'" + titulo + "',")
        sbChart.Append("subtitle: '',")
        sbChart.Append("}};")
        sbChart.Append("var data = google.visualization.arrayToDataTable([")
        sbChart.Append(datoschart)
        sbChart.Append("]);")
        sbChart.Append("var chart = new google.charts.Bar(document.getElementById('columnchart_material'));")
        sbChart.Append("chart.draw(data, google.charts.Bar.convertOptions(options));")
        sbChart.Append("}")
        sbChart.Append("</script>")
        sbChart.Append("<div id='columnchart_material' style='width:  900px; height: 500px;'></div>")

        Return sbChart.ToString
    End Function
    Public Shared Function GoogleComboChart(titulo As String, datoschart As String, serieline As String) As String
        Dim sbChart As New StringBuilder("<script type = ""text/javascript"" >")
        sbChart.Append("google.charts.load('current', {'packages':['corechart']});")
        sbChart.Append("google.charts.setOnLoadCallback(drawChart);")
        sbChart.Append("function drawChart() {")
        sbChart.Append("var options = { ")
        sbChart.Append("title:'" + titulo + "',")
        sbChart.Append("vAxis: {title: 'Total Meses'},")
        sbChart.Append("hAxis: {title: 'Periodo'},")
        sbChart.Append("seriesType: 'bars',")
        sbChart.Append("series: {" + serieline + ": {type: 'line'}}")
        sbChart.Append("};")
        sbChart.Append("var data = google.visualization.arrayToDataTable([")
        sbChart.Append(datoschart)
        sbChart.Append("]);")
        sbChart.Append("var chart = new google.visualization.ComboChart(document.getElementById('combochart'));")
        sbChart.Append("chart.draw(data, options);")
        sbChart.Append("}")
        sbChart.Append("</script>")
        sbChart.Append("<div id='combochart' style='width:  900px; height: 500px;'></div>")

        Return sbChart.ToString
    End Function
End Class
