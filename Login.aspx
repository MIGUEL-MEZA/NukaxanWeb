<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Login.aspx.vb" Inherits="NukaxanWEB.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>NUKAXAN</title>
    <link href="./Content/Image/icono-nukaxan.ico" rel="shortcut icon" type="image/x-icon" />
    <style>
        #FrmLogin {
            width: 400px!important;
            margin:auto!important;
            height:100vh!important;
            border:solid 0px red;
        }

        .form-control {
            color: white !important;
            background-color: rgb(255,255,255,0.1) !important;
            box-shadow: none;
            transition: all 0.3s ease 0s;
            border-radius: 8px !important;
            width:80%!important;
        }

        .form-horizontal .form-control:focus {
            color: white !important;
            background-color: rgb(255,255,255,0.2) !important;
            box-shadow: none !important;
            outline: 0 none !important;
        }

        .form-control:-ms-input-placeholder {
            color: rgb(255,255,255,0.7) !important;
            opacity: 0.1 !important;
        }
        .form-control::-webkit-input-placeholder {
            color: rgb(255,255,255,0.7) !important;
        }
        .btn {
            width: 80% !important;
            border-radius: 8px !important;
        }        
    </style>
    <link href="Content/Site.css" rel="stylesheet" />
    <link href="Content/bootstrap.css" rel="stylesheet" />
    <script src="Scripts/jquery-3.4.1.min.js"></script>
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.10.5/font/bootstrap-icons.css">
     <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.2/css/bootstrap.min.css" />
    <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.9.1/jquery.min.js"></script>
    <script type="text/javascript" src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.2/js/bootstrap.min.js"></script>  
</head>    
<body >
    <div align="center" class="bgNukaxan" style="width:100%;height:100%; border:solid red 0px">
        <form id="FrmLogin" runat="server" autocomplete="off">
            <div style="margin-top:40px;">
                <img src="./Content/Image/logo_NUKAXAN.png" class="logoNukaxan" />
            </div>
            <br /><br />
            <div class="form-group mb-3 " align="center">
                <asp:TextBox ID="TBUserName" runat="server" CssClass="form-control" placeholder="Usuario"
                    required />
            </div>
            <div class="form-group mb-3" style="margin-top:20px;" align="center">
                <asp:TextBox ID="TBPassword" TextMode="Password" runat="server" CssClass="form-control " placeholder="Contraseña"
                    required />
            </div>
            <div class="form-group mb-3" align="center">
                <label runat="server" id="LblRecordarPassword" class="control-label text-white fw-normal">Olvide mi contraseña</label>
            </div>
            <asp:Button ID="btnLogin" Text="ENTRAR" runat="server" OnClick="ValidaLogin" Class="btn btn-primary"
                data-toggle="tooltip333" data-placement="bottom" x-placement="top" title="Da click para ingresar a la plataforma" />
            <br />           
            <div id="dvMessage" runat="server" visible="false" class="alert alert-danger">
                <strong>Error!</strong>
                <asp:Label ID="lblMessage" runat="server" />
            </div>
            <div style="margin-top:100px;">
                <img src="./Content/Image/logo-grupo-nutec.svg" class="logoGN " />
            </div>
            <div align="center" style="margin-top:30px;">
                <label runat="server" id="Label1" class="control-label text-white fw-normal">Copyright &copy;2024 | GRUPO NUTEC&#174;</label>                
            </div>           
        </form>
    </div>   
</body>
</html>
