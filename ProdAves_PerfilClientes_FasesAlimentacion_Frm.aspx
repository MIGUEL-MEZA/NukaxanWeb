<%@ Page Title="" Language="VB" Async="true" AutoEventWireup="true" MasterPageFile="~/Master_ProdAves.Master" CodeBehind="ProdAves_PerfilClientes_FasesAlimentacion_Frm.aspx.vb" Inherits="NukaxanWEB.ProdAves_PerfilClientes_FasesAlimentacion_Frm" %>

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
                    <div class="navbar-default " style="margin-bottom: 3px; height: 40px;">
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
                    <div class="navbar-center">
                            <hr class="hr_section" />
                        </div>  
                    <div style="width: 100%; margin-top: 10px;">
                        <table width="100%" runat="server" class="table_noborder" border="0" style="border: 0px;" clientidmode="Static">
                            <tr>
                                <td colspan="2" align="center">
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
                                   <%-- <div style="padding: 5px 0px;">
                                        <asp:Label runat="server" ID="Menu_Titulo" CssClass="menu_titulo"></asp:Label>
                                        <hr class="hr_section" />
                                    </div>--%>
                                    <div class="bar-actions" style="display: inline-block;">
                                        <asp:LinkButton ID="LB45" runat="server" OnClick="AbrirAgregar" CssClass="lnk-action">
                                            <asp:Image ID="IMG45" runat="server" ImageAlign="AbsMiddle" ImageUrl="Content/Image/icon_file_excel.png" />
                                            <asp:Label runat="server" ID="LBL45"></asp:Label>
                                        </asp:LinkButton>
                                    </div>
                                    <div align="left" style="display: block; width: 100%; overflow-y: scroll; height: 300px;">
                                        <asp:Repeater ID="rptRegistros" runat="server">
                                            <HeaderTemplate>
                                                <table width='100%' cellpadding='3' border="1" class="datagrid">
                                                    <thead>
                                                        <tr>                                                            
                                                            <th width='15%'>
                                                                <asp:Label runat="server" ID="Label1">ETAPA</asp:Label>
                                                            </th>
                                                            <th width='7%'>
                                                                <asp:Label runat="server" ID="Label3">ID</asp:Label>
                                                            </th>
                                                            <th width='25%'>
                                                                <asp:Label runat="server" ID="Label2">FASE</asp:Label>
                                                            </th>
                                                            <th width='10%'>
                                                                <asp:Label runat="server" ID="Label7">SEMANA INICIO</asp:Label>
                                                            </th>
                                                            <th width='10%'>
                                                                <asp:Label runat="server" ID="Label8">SEMANA<br />FIN</asp:Label>
                                                            </th>
                                                             <th width='15%'>
                                                                <asp:Label runat="server" ID="Label5">ESTATUS</asp:Label>
                                                            </th>
                                                            <th width='13%'>
                                                                <asp:Label runat="server" ID="Label4">FECHA ACTUALIZACIÓN</asp:Label>
                                                            </th>
                                                             <th width='20%'>
                                                                <asp:Label runat="server" ID="Label6">USUARIO</asp:Label>
                                                            </th>                                                           
                                                        </tr>
                                                    </thead>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <tr>
                                                    <td align="center">
                                                        <asp:Label runat="server" ID="NomEtapa" Text='<%# Eval("NomEtapa")%>' CssClass="control-value "></asp:Label>
                                                    </td>
                                                    <td align="center">
                                                        <asp:Label runat="server" ID="CveFase" Text='<%# Eval("CveFase")%>' CssClass="control-value bold"></asp:Label>
                                                    </td>
                                                    <td align="left">
                                                        <asp:Label runat="server" ID="FolioR" Text='<%# Eval("NomFase")%>' CssClass="control-value bold"></asp:Label>                                                        
                                                        <asp:HoverMenuExtender runat="server" ID="HME1" TargetControlID="FolioR" PopupControlID="pnl_action" PopupPosition="Right">
                                                        </asp:HoverMenuExtender>
                                                        <asp:Panel ID="pnl_action" runat="server" CssClass="panel-action">
                                                            <asp:ImageButton runat="server" ID="IB1" OnClick="Editar" CommandArgument='<%# Eval("CveEtapa").ToString + "-" + Eval("CveFase").ToString %>' ImageUrl="Content/Image/icon_file_jpg.png"
                                                            ImageAlign="AbsMiddle" />
                                                        <asp:ImageButton runat="server" ID="IB2" OnClick="Eliminar" CommandArgument='<%# Eval("CveEtapa").ToString + "-" + Eval("CveFase").ToString %>' ImageUrl="Content/Image/icon_file_jpg.png"
                                                            ImageAlign="AbsMiddle" />
                                                        </asp:Panel>
                                                    </td>
                                                     <td align="center">
                                                        <asp:Label runat="server" ID="EdadIni" Text='<%#Eval("EdadIni")%>' CssClass="control-value "></asp:Label>
                                                    </td>
                                                    <td align="center">
                                                        <asp:Label runat="server" ID="EdadFin" Text='<%#Eval("EdadFin")%>' CssClass="control-value "></asp:Label>
                                                    </td>
                                                     <td align="center">
                                                         <asp:Label runat="server" ID="Dependencias" Text='<%# Eval("Dependencias")%>' Visible="false"></asp:Label>
                                                        <asp:Label runat="server" ID="CveEstatus" Text='<%# Eval("CveEstatus")%>' Visible="false"></asp:Label>
                                                        <asp:Label runat="server" ID="NomEstatus" Text='<%# Eval("NomEstatus")%>' CssClass="control-value "></asp:Label>
                                                    </td>
                                                    <td align="center">
                                                        <asp:Label runat="server" ID="FecAct" Text='<%#CDate(Eval("FecAct")).ToString("dd/MM/yyyy")%>' CssClass="control-value "></asp:Label>
                                                    </td>
                                                    <td align="center">
                                                        <asp:Label runat="server" ID="NomUsuAct" Text='<%# Eval("NomUsuAct")%>' CssClass="control-value "></asp:Label>
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
                <asp:Label runat="server" ID="mpe_regId2" Visible="false"></asp:Label>
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
                                <td width="30%"></td>
                                <td width="70%"></td>
                            </tr>
                            <tr>
                                <td class="table-td">
                                    <asp:Label runat="server" ID="LBLC1" CssClass="control-label"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="TBIDE" CssClass="control-label semibold color-red"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="table-td">
                                    <asp:Label runat="server" ID="LBLC2" CssClass="control-label"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList runat="server" ID="DDLEtapaE" Width="95%" CssClass="form-control">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="table-td">
                                    <asp:Label runat="server" ID="LBLC3" CssClass="control-label"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="TBNombreE" runat="server" CssClass="form-control text-uppercase" AutoCompleteType="Disabled"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="table-td">
                                    <asp:Label runat="server" ID="LBLC4" CssClass="control-label"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="TBEdadIniE" runat="server" CssClass="form-control text-uppercase" Width="150px" AutoCompleteType="Disabled"></asp:TextBox>                                    
                                    <asp:FilteredTextBoxExtender ID="FTE1" TargetControlID="TBEdadIniE" runat="server" FilterType="Numbers"></asp:FilteredTextBoxExtender>
                                </td>
                            </tr>
                             <tr>
                                <td class="table-td">
                                    <asp:Label runat="server" ID="LBLC5" CssClass="control-label"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="TBEdadFinE" runat="server" CssClass="form-control text-uppercase" Width="150px" AutoCompleteType="Disabled"></asp:TextBox>                                    
                                    <asp:FilteredTextBoxExtender ID="FTE2" TargetControlID="TBEdadFinE" runat="server" FilterType="Numbers"></asp:FilteredTextBoxExtender>
                                </td>
                            </tr>
                            <tr>
                                <td class="table-td">
                                    <asp:Label runat="server" ID="LBLC6" CssClass="control-label"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList runat="server" ID="DDLEstatusE" Width="95%" CssClass="form-control">
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
