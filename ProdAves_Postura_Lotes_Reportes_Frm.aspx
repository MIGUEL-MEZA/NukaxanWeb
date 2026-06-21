<%@ Page Title="" Language="VB" Async="true" AutoEventWireup="true" MasterPageFile="~/Master_ProdAves.Master" CodeBehind="ProdAves_Postura_Lotes_Reportes_Frm.aspx.vb" Inherits="NukaxanWEB.ProdAves_Postura_Lotes_Reportes_Frm" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <script>
        function onDataShown(sender, args) {
            sender._popupBehavior._element.style.zIndex = 1000001;
        }
        $(document).ready(function () {
            /* $('[id*=DDLRolE]').select2();*/
        });
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        if (prm != null) {
            prm.add_endRequest(function (sender, e) {
                if (sender._postBackSettings.panelsToUpdate != null) {
                    /* $("[id*=DDLRolE]").select2({ dropdownAutoWidth: true });*/
                }
            });
        };

    </script>
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
                <asp:Label runat="server" ID="CveEstatus" Visible="false"></asp:Label>
                <asp:Label runat="server" ID="Autor" Visible="false"></asp:Label>
                <asp:Label runat="server" ID="CodCliente" Visible="false"></asp:Label>
                <asp:Label runat="server" ID="CveGranja" Visible="false"></asp:Label>
                <div class="navbar-default " style="margin-bottom: 10px; height: 40px;">
                    <div class="navbar-left">
                        <img src="./Content/Image/icono_title_edit.png" style="margin-left: 10px; width: 30px; vertical-align: top;" />
                        <asp:Label runat="server" ID="PageTitulo" CssClass="page-title"></asp:Label>
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
                <div>
                    <hr class="hr_section" />
                </div>
                <div style="width: 100%; height: 100%; margin-top: 10px;">
                    <table width="100%" runat="server" class="table_noborder" border="0" style="border: 0px;" clientidmode="Static">                        
                        <tr>
                            <td width="250px" valign="top">
                                <div class="menu_section">
                                    <asp:Menu ID="MenuF" runat="server" Orientation="Vertical" RenderingMode="List"
                                        IncludeStyleBlock="true" CssClass="menu_formulario" StaticMenuStyle-CssClass="menu_formulario">
                                    </asp:Menu>
                                </div>
                            </td>
                            <td valign="top">
                                <div class="panel-group" id="accordion">
                                    <div class="panel panel-default">
                                        <div class="panel-heading">
                                            <h4 class="panel-title" data-toggle="collapse" data-target="#collapseOne">Criterios de B�squeda:</h4>
                                        </div>
                                        <div id="collapseOne" class="panel-collapse collapse in">
                                            <div class="panel-body">
                                                <div class="row2 align-items-center d-flex">
                                                    <div class="col-md-2">
                                                        <asp:Label runat="server" ID="LBLF1" CssClass="semibold"></asp:Label>
                                                    </div>
                                                    <div class="col-md-3">
                                                        <asp:DropDownList runat="server" ID="DDLFiltroModalidad" AutoPostBack="True" Width="95%" CssClass="form-control semibold">
                                                        </asp:DropDownList>
                                                    </div>
                                                    <div class="col-md-2">
                                                        <asp:Label runat="server" ID="LBLF2" CssClass="semibold"></asp:Label>
                                                    </div>
                                                    <div class="col-md-3">
                                                        <asp:DropDownList runat="server" ID="DDLFiltroCaseta" Width="95%" CssClass="form-control semibold">
                                                        </asp:DropDownList>
                                                    </div>
                                                    <div class="col-md-2">
                                                        <asp:Button runat="server" ID="BTNP18" CssClass="btn-action" OnClick="Buscar" Width="80px" UseSubmitBehavior="false" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <table border="0" width="100%" cellpadding="1" runat="server" class="reporte_footer"  id="tblpaging">
                                    <tr >
                                        <td width="30%" class="reporte_footer">
                                            <asp:Label runat="server" ID="totreg" Visible="true" CssClass="control-value color-blue-medium "></asp:Label>
                                            <asp:LinkButton ID="LB44" runat="server" OnClick="Exportar" CssClass="lnk-action">
                                                <asp:Image ID="IMG44" runat="server" ImageAlign="AbsMiddle" ImageUrl="Content/Image/icon_file_excel.png" />
                                                <asp:Label runat="server" ID="LBL44"></asp:Label>
                                            </asp:LinkButton>
                                        </td>
                                        <td width="70%" align="right" class="reporte_footer">
                                            <div style="margin-right: 10px;">
                                                <asp:Button runat="server" ID="BTNP11" OnClick="GVPaging" CommandArgument="First" CssClass="paging_button" />
                                                <asp:Button runat="server" ID="BTNP12" OnClick="GVPaging" CommandArgument="Prev" CssClass="paging_button" />
                                                &nbsp;<asp:Label ID="pageLabel" runat="server" CssClass="control-value color-blue-medium "> </asp:Label>&nbsp;
                                                <asp:Button runat="server" ID="BTNP13" OnClick="GVPaging" CommandArgument="Next" CssClass="paging_button" />
                                                <asp:Button runat="server" ID="BTNP14" OnClick="GVPaging" CommandArgument="Last" CssClass="paging_button" />
                                            </div>
                                        </td>

                                    </tr>
                                </table>
                                
                                <div style="overflow-x: auto; overflow-y: AUTO; height: 70vh; width: 100%; border: solid 0px red;">
                                    <%--Grid--%>
                                    <asp:GridView ID="gv1" runat="server" AutoGenerateColumns="false" ShowHeader="true"
                                        Width="100%" ShowFooter="false" AllowPaging="true" CellPadding="1" CssClass="datagrid">
                                        <HeaderStyle />
                                        <Columns>
                                        </Columns>
                                        <PagerSettings Visible=" false" />
                                    </asp:GridView>
                                </div>
                            </td>
                        </tr>
                    </table>
                    <br />
                    <br />
                </div>

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
                <asp:ModalPopupExtender ID="MPECaptura" runat="server" PopupControlID="pnlCaptura" TargetControlID="lnkshowdata2"
                    BackgroundCssClass="modalBackground">
                </asp:ModalPopupExtender>
                <asp:Panel ID="pnlCaptura" runat="server" CssClass="modalPopup" Style="display: none;">
                    <div class="modalPopup_header">
                        <asp:Label runat="server" ID="lblMPE_title"></asp:Label>
                    </div>
                    <div id="MPEBody_Captura" runat="server" class="modalPopup_body" align="left" style="height: 85%; overflow-y: scroll;">
                    </div>
                    <div class="modalPopup_footer" align="center">
                        <asp:Button runat="server" ID="BTNP16" CssClass="btn-action" OnClick="mpe_action" CommandArgument="action_save" Width="80px" UseSubmitBehavior="false" />
                        <asp:Button runat="server" ID="BTNP15" CssClass="btn-action" OnClick="mpe_action" Style="margin-left: 5px;" CommandArgument="action_close" Width="80px" UseSubmitBehavior="false" />
                    </div>
                </asp:Panel>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="LB44" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
</asp:Content>
