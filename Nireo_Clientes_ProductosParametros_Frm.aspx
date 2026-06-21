<%@ Page Title="" Language="VB" Async="true" AutoEventWireup="true" MasterPageFile="~/Master_Nireo.Master" CodeBehind="Nireo_Clientes_ProductosParametros_Frm.aspx.vb" Inherits="NukaxanWEB.Nireo_Clientes_ProductosParametros_Frm" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <script>
        function onDataShown(sender, args) {
            sender._popupBehavior._element.style.zIndex = 1000001;
        }
        $(document).ready(function () {
            $('[id*=DDLEquipoPE]').select2();
        });
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        if (prm != null) {
            prm.add_endRequest(function (sender, e) {
                if (sender._postBackSettings.panelsToUpdate != null) {
                    $("[id*=DDLEquipoPE]").select2({ dropdownAutoWidth: true, dropdownParent: $('#MainContent_pnlCaptura') });
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
                <asp:Panel runat="server" ID="panelcontenido" Visible="true">
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
                    <div style="width: 100%; margin-top: 10px;">
                        <table width="100%" runat="server" class="table_noborder" border="0" style="border: 0px;" clientidmode="Static">
                            <tr>
                                <td colspan="2" align="left">
                                    <div style="padding: 3px 0px;">
                                        <asp:Label runat="server" ID="TBNombreTitulo" CssClass="cliente_titulo"></asp:Label>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td width="250px" valign="top">
                                    <div class="menu_section">
                                        <asp:Menu ID="MenuF" runat="server" Orientation="Vertical" RenderingMode="List"
                                            IncludeStyleBlock="true" CssClass="menu_formulario" StaticMenuStyle-CssClass="menu_formulario">
                                        </asp:Menu>
                                    </div>
                                </td>
                                <td valign="top">
                                    <div style="padding: 5px 0px;">
                                        <asp:Label runat="server" ID="Menu_Titulo" CssClass="menu_titulo"></asp:Label>
                                        <hr class="hr_section" />
                                    </div>
                                    <div align="left" style="display: block; width: 99%; overflow-y: scroll; height: 320px;">
                                        <asp:Repeater ID="rptProductos" runat="server">
                                            <HeaderTemplate>
                                                <table width='100%' cellpadding='3' border="1" class="datagrid">
                                                    <thead>
                                                        <tr>
                                                            <th width='4%'>
                                                                <asp:Label runat="server" ID="Label1"></asp:Label>
                                                            </th>
                                                            <th width='20%'>
                                                                <asp:Label runat="server" ID="Label3">PRODUCTO</asp:Label>
                                                            </th>
                                                            <th width='15%'>
                                                                <asp:Label runat="server" ID="Label2">PARÁMETRO</asp:Label>
                                                            </th>
                                                             <th width='15%'>
                                                                <asp:Label runat="server" ID="Label18">CURVA CALIBRACIÓN</asp:Label>
                                                            </th>
                                                            <th width='10%'>
                                                                <asp:Label runat="server" ID="Label4">VALOR ESPERADO</asp:Label>
                                                            </th>
                                                            <th width='10%'>
                                                                <asp:Label runat="server" ID="Label6">VALOR MÍNIMO ESPERADO</asp:Label>
                                                            </th>
                                                            <th width='10%'>
                                                                <asp:Label runat="server" ID="Label7">VALOR MÁXIMO ESPERADO</asp:Label>
                                                            </th>
                                                            <th width='10%'>
                                                                <asp:Label runat="server" ID="Label5">ESTATUS</asp:Label>
                                                            </th>
                                                            <th width='10%'>
                                                                <asp:Label runat="server" ID="Label8">ACTUALIZACIÓN</asp:Label>
                                                            </th>
                                                        </tr>
                                                    </thead>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <tr>
                                                    <td align="center">
                                                        <asp:ImageButton runat="server" ID="IB3" OnClick="ExpandirColapsar" CommandArgument="E" ImageUrl="Content/Image/icono_expandir.png"
                                                            ImageAlign="AbsMiddle" />
                                                        <asp:ImageButton runat="server" ID="IB4" OnClick="ExpandirColapsar" CommandArgument="C" ImageUrl="Content/Image/icono_colapsar.png" Visible="false"
                                                            ImageAlign="AbsMiddle" />
                                                    </td>
                                                    <td align="left" colspan="2">
                                                        <asp:Label runat="server" ID="CveProducto" Text='<%# Eval("CveProducto")%>' Visible="false"></asp:Label>
                                                        <asp:Label runat="server" ID="Dependencias" Text='<%# Eval("Dependencias")%>' Visible="false"></asp:Label>
                                                        <asp:Label runat="server" ID="NomProducto" Text='<%# Eval("NomProducto")%>' CssClass="control-value bold pointer"></asp:Label>
                                                        <asp:ImageButton runat="server" ID="IB1" OnClick="Editar" CommandArgument='<%# Eval("CveProducto") %>' ImageUrl="Content/Image/icon_file_jpg.png" CssClass="image-buttom"
                                                        ImageAlign="AbsMiddle" />
                                                    </td>
                                                    <td colspan="5"></td>
                                                </tr>
                                                <asp:Panel ID="pnlParametros" runat="server" Visible="false">
                                                    <asp:Repeater ID="rptParametros" runat="server">
                                                        <ItemTemplate>
                                                            <tr>
                                                                <td colspan="2"></td>
                                                                <td align="center">
                                                                    <asp:Label runat="server" ID="CveProducto" Text='<%# Eval("CveProducto")%>' Visible="false"></asp:Label>
                                                                    <asp:Label runat="server" ID="CveParametro" Text='<%# Eval("CveParametro")%>' Visible="false"></asp:Label>
                                                                    <asp:Label runat="server" ID="Dependencias" Text='<%# Eval("Dependencias")%>' Visible="false"></asp:Label>
                                                                    <asp:Label runat="server" ID="CveEstatus" Text='<%# Eval("CveEstatus")%>' Visible="false"></asp:Label>
                                                                    <asp:Label runat="server" ID="NomParametro" Text='<%# Eval("NomParametro")%>' CssClass="control-value bold pointer"></asp:Label>                                                                    
                                                                </td>
                                                                 <td align="center">
                                                                   <asp:Label runat="server" ID="Archivo" Text='<%# Eval("Archivo")%>' CssClass="control-value "></asp:Label>
                                                                </td>
                                                                <td align="center">
                                                                    <asp:Label runat="server" ID="ValorEsperado" Text='<%# Eval("ValorEsperado")%>' CssClass="control-value "></asp:Label>
                                                                </td>
                                                                <td align="center">
                                                                    <asp:Label runat="server" ID="ValorMin" Text='<%# Eval("ValorMin")%>' CssClass="control-value "></asp:Label>
                                                                </td>
                                                                <td align="center">
                                                                    <asp:Label runat="server" ID="ValorMax" Text='<%# Eval("ValorMax")%>' CssClass="control-value "></asp:Label>
                                                                </td>
                                                                <td align="center">
                                                                   <asp:Label runat="server" ID="NomEstatus" Text='<%# Eval("NomEstatus")%>' CssClass="control-value "></asp:Label>
                                                                </td>
                                                                <td align="center">
                                                                    <asp:Label runat="server" ID="FecAct" Text='<%#CDate(Eval("FecAct")).ToString("dd/MM/yyyy HH:mm")%>' CssClass="control-value "></asp:Label>
                                                                </td>
                                                            </tr>
                                                        </ItemTemplate>
                                                    </asp:Repeater>
                                                </asp:Panel>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                </table>
                                            </FooterTemplate>
                                        </asp:Repeater>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <br />
                </asp:Panel>

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
                        <asp:Label runat="server" ID="TBProductoD" CssClass="control-label semibold "></asp:Label>
                        <div align="left" style="display: block; width: 99%; overflow-y: scroll; height: 450px;">
                            <asp:Repeater ID="rptParametrosE" runat="server">
                                <HeaderTemplate>
                                    <table width='100%' cellpadding='3' border="1" class="datagrid">
                                        <thead>
                                            <tr>
                                                <th width='8%'>
                                                    <asp:Label runat="server" ID="Label7">
                                                        <%--<asp:CheckBox ID="chkAll" runat="server" AutoPostBack="true" CssClass="control-chk semibold"  />--%>
                                                    </asp:Label>
                                                </th>
                                                <th width=' 62%'>
                                                    <asp:Label runat="server" ID="Label1">NOMBRE</asp:Label>
                                                </th>
                                                <th width='30%'>
                                                    <asp:Label runat="server" ID="Label2">ESTATUS</asp:Label>
                                                </th>
                                            </tr>
                                        </thead>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <td align="center ">
                                            <asp:CheckBox ID="chk" runat="server"  />
                                        </td>
                                        <td align="left " class="fw-bold">                                            
                                            <asp:Label runat="server" ID="CveParametro" Text='<%# Eval("CveParametro")%>' Visible="false"></asp:Label>
                                            <asp:Label runat="server" ID="Aplica" Text='<%# Eval("Aplica")%>' Visible="false"></asp:Label>
                                            <asp:Label runat="server" ID="Dependencias" Text='<%# Eval("Dependencias")%>' Visible="false"></asp:Label>
                                            <asp:Label runat="server" ID="CveEstatus" Text='<%# Eval("CveEstatus")%>' Visible="false"></asp:Label>
                                            <asp:Label runat="server" ID="NomParametro" Text='<%# Eval("NomParametro")%>' CssClass="control-value bold pointer"></asp:Label>
                                        </td>
                                        <td align="center" class="justify-content-center">
                                            <asp:DropDownList runat="server" ID="DDLEstatusE" Width="120px" CssClass="form-control">
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
