<%@ Page Title="" Language="VB" Async="true" AutoEventWireup="true" MasterPageFile="~/Master_OptimizerC.Master" CodeBehind="Nufeed_Format_Carga.aspx.vb" Inherits="NukaxanWEB.Nufeed_Format_Carga" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <script src="Scripts/WebForms/Message.js"></script>
    <script>       

        function onDataShown(sender, args) {
            sender._popupBehavior._element.style.zIndex = 1000001;
        }
        $(document).ready(function () {
            $('[id*=DDLPlataforma]').select2();
            $('[id*=DDLCliente]').select2();
            $('[id*=DDLPerfil]').select2();
            //$("#MainContent_LB4").hide();
            //$("#MainContent_LB11").show();
        });
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        if (prm != null) {
            prm.add_endRequest(function (sender, e) {
                if (sender._postBackSettings.panelsToUpdate != null) {
                    $("[id*=DDLPlataforma]").select2({ dropdownAutoWidth: true });
                    $("[id*=DDLCliente]").select2({ dropdownAutoWidth: true });
                    $("[id*=DDLPerfil]").select2({ dropdownAutoWidth: true });
                }
            });
        };
      
    </script>
    <style>
        .header {
            background-color: #0b2e57;
            color: white;
            padding: 10px 25px 10px 15px;
            border-radius: 10px;
            display: flex;
            justify-content: space-between;
            align-items: flex-start; /* ?? Alinea todo arriba */
        }

        .header-left {
            display: flex;
            align-items: flex-start; /* ?? clave */
            gap: 5px;
        }

        .logo-left {
            width: 85px; /* ?? m�s grande */
            margin-right: 8px; /* ?? m�s espacio */
        }

        .header-text h1 {
            margin: 0;
            font-size: 26px;
            letter-spacing: 1px;
        }

        .subtitulo {
            color: #18a4ff;
            font-size: 13px;
            margin-top: 3px;
        }

        .cliente {
            font-size: 12px;
            margin-top: 4px;
        }

        .divider {
            width: 2px;
            height: 65px;
            background-color: #0f4d8a;
            margin: 0 25px;
        }

        .header-right {
            text-align: right;
        }

        .logo-right {
            width: 290px;
            margin-bottom: 8px;
        }

        .fecha {
            font-size: 12px;
            color: #cfd8e3;
        }

        thead tr:nth-child(1) th {
            height: 35px;
        }

        thead tr:nth-child(1) th {
            position: sticky;
            top: 0;
            background: #0b2e57 !important;
            color: #ffffff !important;
            font-weight: normal !important;
            z-index: 3;
        }

        th {
            /* border:  1px solid #ccc;*/
            padding: 5px;
        }

        td {
            /*border:  1px solid #ccc;*/
            padding: 5px;
        }

        .table-rep {
            border-collapse: separate;
            border-spacing: 0;
        }


            .table-rep th {
                border-right: 1px solid #0f2f52;
            }

            .table-rep thead tr th:last-child {
                border-right: none; /* ?? evita el borde cuadrado */
            }

            .table-rep thead tr th:first-child {
                border-top-left-radius: 10px;
            }

            .table-rep thead tr th:last-child {
                border-top-right-radius: 10px;
            }

            .table-rep tfoot th {
                background: #0b2e57 !important;
                color: #ffffff !important;
                font-weight: normal !important;
            }

        .categoria {
            background-color: #dce9f5;
        }
    </style>
    <asp:UpdateProgress ID="UpdateProgress1" DisplayAfter="10" runat="server" AssociatedUpdatePanelID="UPContenido">
        <ProgressTemplate>
            <div class="divWaiting" style="background-color: white;">
                <div style="margin-top: 300px;">
                    <%--<asp:Image ID="imgWait" runat="server" ImageAlign="Middle" ImageUrl="Content/Images/euro-lab-online.png" Width="367" Height="54px" />
                    <br />--%>
                    <asp:Label ID="lblWait" runat="server" Font-Bold="true" Font-Size="25px" ForeColor="#003F7C" Text=" Procesando... " />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <div class="container-fluid w-100 h-100">
        <asp:UpdatePanel runat="server" ID="UPContenido" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:Label runat="server" ID="filtroview" Visible="false"></asp:Label>
                <asp:Label runat="server" ID="gvindexpage" Visible="false"></asp:Label>
                <asp:Label runat="server" ID="regPId" Visible="false"></asp:Label>
                <asp:Label runat="server" ID="CodCliente" Visible="false"></asp:Label>
                <asp:Label runat="server" ID="CodALLIX" Visible="false"></asp:Label>
                <asp:Label runat="server" ID="CveModalidad" Visible="false"></asp:Label>
                <asp:Label runat="server" ID="CveEstatus" Visible="false"></asp:Label>
                <asp:Label runat="server" ID="Autor" Visible="false"></asp:Label>
                <asp:Label runat="server" ID="CvePlan" Visible="false"></asp:Label>

                <div class="navbar-default " style="margin-bottom: 10px; height: 40px;">
                    <div class="navbar-left">
                        <asp:Label runat="server" ID="PageTitulo" CssClass="page-title"></asp:Label>
                        <img src="./Content/Image/icono_title_edit.png" style="margin-left: 10px; width: 30px; vertical-align: top;" />
                    </div>
                    <button type="button" class="navbar-toggle collapsed" data-toggle="collapse" data-target="#bar-action"
                        aria-expanded="false" style="margin-left: 10px;">
                        <span class="sr-only">Toggle navigation</span> <span class="icon-bar"></span><span
                            class="icon-bar"></span><span class="icon-bar"></span>
                    </button>
                    <div class="collapse navbar-collapse navbar-right navbar-right" id="bar-action">
                        <asp:LinkButton ID="LB2" runat="server" OnClick="Regresar" CssClass="lnkbtn-action">
                            <i runat="server" id="LB_IMG2" class=""></i>
                            <asp:Label runat="server" ID="LB_LBL2">Salir</asp:Label>
                        </asp:LinkButton>
                    </div>
                </div>
                <table class="table-condensed" border="1" style="border: solid 1px gray; width: 700px; margin-left: auto; margin-right: auto;">
                    <tr style="visibility: collapse;">
                        <td width="40%" class="bg-gray semibold"></td>
                        <td width="60%" class="bg-white"></td>
                    </tr>
                    <tr>
                        <td class="bg-gray semibold">
                            <asp:Label runat="server" ID="LBLC1"></asp:Label>
                        </td>
                        <td class="bg-white" colspan="3">
                            <asp:DropDownList ID="DDLPlataforma" runat="server" AutoPostBack="true" Width="95%" ClientIDMode="Static">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="bg-gray semibold">
                            <asp:Label runat="server" ID="LBLC2"></asp:Label>
                        </td>
                        <td class="bg-white" colspan="3">
                            <asp:DropDownList ID="DDLCliente" runat="server" AutoPostBack="true" Width="95%" ClientIDMode="Static">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="bg-gray semibold">
                            <asp:Label runat="server" ID="LBLC3"></asp:Label>
                        </td>
                        <td class="bg-white" colspan="3">
                            <asp:DropDownList ID="DDLPerfil" AutoPostBack="true" runat="server" Width="95%" ClientIDMode="Static">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="center">
                            <asp:UpdatePanel ID="pnlCarga" runat="server">
                                <ContentTemplate>
                                    <asp:FileUpload ID="FileUpload" runat="server" CssClass="file-upload" Width="370px" accept=".exp"/>
                                   <%-- <ajaxToolkit:AsyncFileUpload ID="afuArchivo" runat="server" ClientIDMode="Static" AllowedFileTypes="EXP,exp"
                                   OnUploadedComplete="afuArchivo_UploadedComplete" />--%>
                                    <asp:Button ID="btnEnviar" runat="server" Text="Enviar" OnClick="Enviar" ClientIDMode="Static" />
                                </ContentTemplate>
                                <Triggers>
                                    <asp:PostBackTrigger ControlID="btnEnviar" />
                                </Triggers>
                            </asp:UpdatePanel>                           
                            <asp:Label ID="lblMensaje" runat="server" ClientIDMode="Static" />
                        </td>
                    </tr>
                </table>

                <%--SHOW POPUP--%>
                <asp:Label runat="server" ID="mpe_regId" Visible="false"></asp:Label>
                <asp:Label runat="server" ID="mpe_op" Visible="false"></asp:Label>
                <asp:LinkButton Text="" ID="lnkshowdata" runat="server" />
                <asp:LinkButton Text="" ID="lnkshowdata2" runat="server" />
                <asp:ModalPopupExtender ID="MPEAlerta" runat="server" PopupControlID="pnlAlerta" TargetControlID="lnkshowdata"
                    BackgroundCssClass="modalBackground">
                </asp:ModalPopupExtender>
                <asp:Panel ID="pnlAlerta" runat="server" CssClass="alert-panel" Style="display: none;">
                    <div class='alert-content'>
                        <asp:Literal ID="MPEBody" runat="server"></asp:Literal>
                        <div class="modal-footer" align="center">
                            <asp:Button runat="server" ID="BAlertOK" CssClass="btn-action" OnClick="mpe_action" CommandArgument="alert_refresh" Width="80px" UseSubmitBehavior="false" />
                            <asp:Button runat="server" ID="BAlertCancel" CssClass="btn-action" OnClick="mpe_action" Style="margin-left: 5px;" CommandArgument="alert_close" Width="80px" />
                        </div>
                    </div>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>


