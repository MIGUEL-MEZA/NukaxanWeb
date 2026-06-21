<%@ Page Title="" Language="VB" Async="true" AutoEventWireup="true" MasterPageFile="~/Master_Nukaxan.Master" CodeBehind="Nukaxan_UsuariosE_Frm.aspx.vb" Inherits="NukaxanWEB.Nukaxan_UsuariosE_Frm" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <script src="Scripts/WebForms/Message.js"></script>
    <script>
        function onDataShown(sender, args) {
            sender._popupBehavior._element.style.zIndex = 1000001;
        }
        $(document).ready(function () {           
            $('[id*=DDLClientePE]').select2();
        });
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        if (prm != null) {
            prm.add_endRequest(function (sender, e) {
                if (sender._postBackSettings.panelsToUpdate != null) {                    
                    $("[id*=DDLClientePE]").select2({ dropdownAutoWidth: true, dropdownParent: $('#MainContent_pnlCaptura') });
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
                <div class="navbar-default " style="margin-bottom: 10px; height: 40px; width: 71%;">
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
                        <asp:LinkButton ID="LB3" runat="server" OnClick="Guardar" CssClass="lnkbtn-action">
                            <i runat="server" id="LB_IMG3" class=""></i>
                            <asp:Label runat="server" ID="LB_LBL3">Guardar</asp:Label>
                        </asp:LinkButton>
                        <asp:LinkButton ID="LB2" runat="server" OnClick="Regresar" CssClass="lnkbtn-action">
                            <i runat="server" id="LB_IMG2" class=""></i>
                            <asp:Label runat="server" ID="LB_LBL2">Salir</asp:Label>
                        </asp:LinkButton>
                    </div>
                </div>
                <div style="width: 70%; margin-top: 10px; margin-left: 5px;">
                    <div class="divsec-captura">
                        <asp:Label runat="server" ID="SECTitulo1" CssClass="control-label"></asp:Label>
                    </div>
                    <table class="table-condensed" border="1" style="border: solid 1px gray; width: 100%;">
                        <tr>
                            <td width="30%" class="bg-gray semibold">
                                <asp:Label runat="server" ID="LBLC1"></asp:Label>
                            </td>
                            <td width="70%" class="bg-white">
                                <asp:Label runat="server" ID="TBCodigoD" CssClass="control-label semibold color-red"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="bg-gray semibold">
                                <asp:Label runat="server" ID="LBLC2"></asp:Label>
                            </td>
                            <td class="bg-white">
                                <asp:TextBox ID="TBNombre" runat="server" CssClass="form-control text-uppercase" AutoCompleteType="Disabled"></asp:TextBox>
                                <asp:Label runat="server" ID="TBNombreD" CssClass="control-label semibold"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="bg-gray semibold">
                                <asp:Label runat="server" ID="LBLC4"></asp:Label>
                            </td>
                            <td class="bg-white">
                                <asp:TextBox ID="TBEmail" runat="server" CssClass="form-control" AutoCompleteType="Disabled"></asp:TextBox>
                                <asp:Label runat="server" ID="TBEmailD" CssClass="control-label"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="bg-gray semibold">
                                <asp:Label runat="server" ID="LBLC6"></asp:Label>
                            </td>
                            <td class="bg-white ">
                                <asp:TextBox ID="TBNomPuesto" runat="server" CssClass="form-control text-uppercase" AutoCompleteType="Disabled"></asp:TextBox>
                                <asp:Label runat="server" ID="TBNomPuestoD"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="bg-gray semibold">
                                <asp:Label runat="server" ID="LBLC8"></asp:Label>
                            </td>
                            <td class="bg-white">
                                <asp:TextBox ID="TBNomUbicacion" runat="server" CssClass="form-control text-uppercase" AutoCompleteType="Disabled"></asp:TextBox>
                                <asp:Label runat="server" ID="TBNomUbicacionD" CssClass="control-label"></asp:Label>

                            </td>
                        </tr>
                        <tr>
                            <td class="bg-gray semibold">
                                <asp:Label runat="server" ID="LBLC10"></asp:Label>
                            </td>
                            <td class="bg-white">
                                <asp:TextBox ID="TBNomArea" runat="server" CssClass="form-control text-uppercase" AutoCompleteType="Disabled"></asp:TextBox>
                                <asp:Label runat="server" ID="TBNomAreaD" CssClass="control-label"></asp:Label>

                            </td>
                        </tr>
                        <tr>
                            <td class="bg-gray semibold">
                                <asp:Label runat="server" ID="LBLC12"></asp:Label>
                            </td>
                            <td class="bg-white">
                                <asp:DropDownList runat="server" ID="DDLEstatus" Width="200px" CssClass="form-control">
                                </asp:DropDownList>
                                <asp:Label runat="server" ID="TBNomEstatusD" CssClass="control-label"></asp:Label>
                            </td>
                        </tr>
                    </table>
                    <br />
                    <div class="divsec-captura">
                        <asp:Label runat="server" ID="SECTitulo2" CssClass="control-label"></asp:Label>
                    </div>
                    <table class="table-condensed" border="1" style="border: solid 1px gray; width: 100%;">
                        <tr>
                            <td width="30%" class="bg-gray semibold">
                                <asp:Label runat="server" ID="LBLC14"></asp:Label>
                            </td>
                            <td width="70%" class="bg-white">
                                <asp:DropDownList runat="server" ID="DDLRol" Width="95%" CssClass="form-control" ClientIDMode="Static">
                                </asp:DropDownList>
                                <asp:Label runat="server" ID="TBNomRolD" CssClass="control-label"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td width="30%" class="bg-gray semibold">
                                <asp:Label runat="server" ID="LBLC16"></asp:Label>
                            </td>
                            <td width="70%" class="bg-white">
                                <asp:TextBox ID="TBSecUsuario" runat="server" CssClass="form-control" Width="200px"></asp:TextBox>
                                <asp:Label runat="server" ID="TBSecUsuarioD" CssClass="control-label "></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td width="30%" class="bg-gray semibold">
                                <asp:Label runat="server" ID="LBLC18"></asp:Label>
                            </td>
                            <td width="70%" class="bg-white">
                                <asp:TextBox ID="TBsecPassword" runat="server" CssClass="form-control" Width="200px"></asp:TextBox>
                                <asp:Label runat="server" ID="TBsecPasswordD" CssClass="control-label"></asp:Label>
                            </td>
                        </tr>
                    </table>
                    <br />
                    <div class="divsec-captura">
                        <asp:Label runat="server" ID="SECTitulo3" CssClass="control-label"></asp:Label>
                    </div>
                    <div runat="server" id="bar_action" class="bar-actions" style="display: inline-block;">
                        <asp:LinkButton ID="LB412" runat="server" OnClick="AbrirAgregar" CssClass="lnk-action">
                            <asp:Image ID="IMG412" runat="server" ImageAlign="AbsMiddle" ImageUrl="Content/Image/icon_file_excel.png" />
                            <asp:Label runat="server" ID="LBL412"></asp:Label>
                        </asp:LinkButton>
                    </div>
                    <div align="left" style="display: block; width: 100%;">
                        <asp:Repeater ID="rptClientes" runat="server">
                            <HeaderTemplate>
                                <table width='100%' cellpadding='3' border="1" class="table table-condensed  table-sm table-rep">
                                    <thead>
                                        <tr>
                                            <th width='10%'>
                                                <asp:Label runat="server" ID="Label1">CÓDIGO</asp:Label>
                                            </th>
                                            <th width='70%'>
                                                <asp:Label runat="server" ID="Label2">NOMBRE</asp:Label>
                                            </th>
                                            <th width='20%'>
                                                <asp:Label runat="server" ID="Label3">ESTATUS</asp:Label>
                                            </th>
                                        </tr>
                                    </thead>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <tr>
                                    <td align="center " class="justify-content-center">
                                        <asp:Label runat="server" ID="Dependencias2" Text='<%# Eval("Dependencias2")%>' Visible="false"></asp:Label>
                                        <asp:Label runat="server" ID="CveEstatus" Text='<%# Eval("CveEstatus")%>' Visible="false"></asp:Label>
                                        <asp:Label runat="server" ID="CodCliente" Text='<%# Eval("CodCliente")%>' CssClass="control-value "></asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:Label runat="server" ID="NomCliente" Text='<%# Eval("NomCliente")%>' CssClass="control-value "></asp:Label>
                                        <asp:ImageButton runat="server" ID="IB2" OnClick="Eliminar" CommandArgument='<%# Eval("CodCliente") %>' ImageUrl="Content/Image/icon_file_jpg.png"
                                            ImageAlign="AbsMiddle" />
                                    </td>
                                    <td align="center">
                                        <asp:DropDownList runat="server" ID="DDLEstatus" Width="95%" CssClass="form-control">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </ItemTemplate>
                            <FooterTemplate>
                                </table>
                            </FooterTemplate>
                        </asp:Repeater>
                    </div>
                    <br />
                    <div class="divsec-captura">
                        <asp:Label runat="server" ID="SECTitulo4" CssClass="control-label"></asp:Label>
                    </div>
                    <table class="table-condensed" border="1" style="border: solid 1px gray; width: 100%;">
                        <tr>
                            <td width="30%" class="bg-gray semibold">
                                <asp:Label runat="server" ID="LBLC0"></asp:Label>
                            </td>
                            <td width="70%" class="bg-white">
                                <asp:Label runat="server" ID="TBActualizaD" CssClass="control-label "></asp:Label>
                            </td>
                        </tr>
                    </table>
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
                    <div id="MPEBody_Captura" runat="server" class="modalPopup_body" align="left">
                        <table width="100%" class="table table_noborder" border="1" style="border: solid 1px #efefef;">
                            <tr style="visibility: collapse;">
                                <td width="20%"></td>
                                <td width="80%"></td>
                            </tr>
                            <tr>
                                <td class="table-td">
                                    <asp:Label runat="server" ID="LBLC20" CssClass="control-label"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList runat="server" ID="DDLClientePE" Width="95%" CssClass="form-control" ClientIDMode="Static">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div class="modalPopup_footer" align="center">
                        <asp:Button runat="server" ID="BTNP16" CssClass="btn-action" OnClick="mpe_action" CommandArgument="action_save" Width="80px" UseSubmitBehavior="false" />
                        <asp:Button runat="server" ID="BTNP15" CssClass="btn-action" OnClick="mpe_action" Style="margin-left: 5px;" CommandArgument="action_close" Width="80px" UseSubmitBehavior="false" />
                    </div>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
