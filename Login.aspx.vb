Imports NukaxanWEB
Imports NukaxanWEB.Libreria
Public Class Login
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Seguridad de cache
        Response.Buffer = True
        Response.ExpiresAbsolute = Now.AddMinutes(-1)
        Response.Expires = -1
        Response.CacheControl = "no-cache"
        Response.AddHeader("Pragma", "no-cache")

        If HttpContext.Current.User.Identity.IsAuthenticated Then
            Response.Redirect("Logout.aspx", True)
        End If
        If Not Page.IsPostBack Then
            DatosLoad()
        End If
    End Sub
    Sub DatosLoad()
        Etiquetas()

    End Sub
    Sub Etiquetas()


    End Sub
    Sub ValidaLogin()
        dvMessage.Visible = False
        lblMessage.Text = ""
        Try
            Dim sec_usu As String = TBUserName.Text.Replace("'", "")
            Dim sec_password As String = TBPassword.Text.Replace("'", "")
            'If sec_usu = "" Or sec_password = "" Then Throw New Exception("El usuario y/o password son incorrectos")
            Dim Obj As New Usuario()
            If Obj.ValidaLogin(sec_usu, sec_password) Then
                Dim ObjU As UsuarioModel = New Usuario().FindById(0, 0, Obj.CodUsuario)
                'If ObjU.CveTipo = "I" Then
                '    dvMessage.Visible = True
                '    lblMessage.Text = "<b>SITIO WEB EN MANTENIMIENTO</b><br>La Plataforma esta actualmente bajo mantenimiento programado.<br>Estaremos de vuelta en breve."
                '    Exit Sub
                'End If
                Session("UsuarioLogin") = ObjU
                System.Web.Security.FormsAuthentication.RedirectFromLoginPage(sec_usu, False)
                System.Web.Security.FormsAuthentication.SetAuthCookie(sec_usu, False)

                Response.Redirect(FormsAuthentication.DefaultUrl)
                'If ObjU.CveRol = 222 Then
                '    Response.Redirect("OptimizerC_Home.aspx", True)
                'ElseIf ObjU.CveRol = 4 Then
                '    Response.Redirect("Nufeed_Home.aspx", True)
                'Else
                '    Response.Redirect(FormsAuthentication.DefaultUrl)
                'End If


                ' Response.Redirect((New RedirectPaginas().FindById(nav.Text)).PaginaURL.Replace("@Id", Codif(regPId.Text)), True)

            Else
                dvMessage.Visible = True
                lblMessage.Text = Obj.strError
            End If

        Catch ex As Exception
            dvMessage.Visible = True
            lblMessage.Text = CleanSpecialCharacter(ex.Message)
        End Try
    End Sub
    'General
    Protected Sub Page_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender

    End Sub
End Class