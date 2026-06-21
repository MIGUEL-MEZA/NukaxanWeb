<%@ Page Title="" Language="VB" Async="true" AutoEventWireup="true" MasterPageFile="~/Master_Nireo.Master" CodeBehind="Nireo_Clientes_Plataformas_Frm.aspx.vb" Inherits="NukaxanWEB.Nireo_Clientes_Plataformas_Frm" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <script>
        function onDataShown(sender, args) {
            sender._popupBehavior._element.style.zIndex = 1000001;
        }
        
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
        <asp:UpdatePanel runat="server" ID="UPContenido" UpdateMode="Always">
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
                                <div align="left" style="display: block;width:70%;">
                                    <asp:Repeater ID="rptPlataformas" runat="server">
                                        <HeaderTemplate>
                                            <table width='100%' cellpadding='3' border="1" class="table table-condensed  table-sm table-rep">
                                                <thead>
                                                    <tr>
                                                        <th width='10%'>
                                                            <asp:Label runat="server" ID="Label7">
                                                                <asp:CheckBox ID="chkAll" runat="server" AutoPostBack="true" CssClass="control-chk semibold" OnCheckedChanged="OnCheckedAllChanged" />
                                                            </asp:Label>
                                                        </th>
                                                        <th width='70%'>
                                                            <asp:Label runat="server" ID="Label1">PLATAFORMA</asp:Label>
                                                        </th>
                                                        <th width='20%'>
                                                            <asp:Label runat="server" ID="Label2">ESTATUS</asp:Label>
                                                        </th> 
                                                    </tr>
                                                </thead>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <tr>
                                                <td align="center ">
                                                    <asp:CheckBox ID="chk" runat="server" AutoPostBack="true" OnCheckedChanged="OnCheckedChanged" />
                                                </td>
                                                <td align="left " class="fw-bold">
                                                    <asp:Label runat="server" ID="CvePlataforma" Text='<%# Eval("CvePlataforma")%>' Visible="false"></asp:Label>
                                                    <asp:Label runat="server" ID="CvePlataformaP" Text='<%# Eval("CvePlataformaP")%>' Visible="false"></asp:Label>
                                                    <asp:Label runat="server" ID="CveEstatus" Text='<%# Eval("CveEstatus")%>' Visible="false"></asp:Label>
                                                    <asp:Label runat="server" ID="Habilitada" Text='<%# Eval("Habilitada")%>' Visible="false"></asp:Label>
                                                    <asp:Label runat="server" ID="Dependencias" Text='<%# Eval("Dependencias")%>' Visible="false"></asp:Label>
                                                    <asp:Label runat="server" ID="NomPlataforma" Text='<%# Eval("NomPlataforma")%>' CssClass="control-value bold" ></asp:Label>
                                                </td>                                                
                                                <td align="center" class="justify-content-center">
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
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
