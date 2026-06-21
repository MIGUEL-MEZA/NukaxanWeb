<%@ Page Title="" Language="VB" Async="true" MasterPageFile="~/Master_Nireo.Master" AutoEventWireup="true" CodeBehind="Nufeed_ActualizaNutrientes_Frm.aspx.vb" Inherits="NukaxanWEB.Nufeed_ActualizaNutrientes_Frm" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagPrefix="asp" Namespace="Saplin.Controls" Assembly="DropDownCheckBoxes" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
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
    <script>
        function onDataShown(sender, args) {
            sender._popupBehavior._element.style.zIndex = 1000001;
        }
        $(document).ready(function () {
            $('[id*=DDLFiltroCliente]').select2();
            $('[id*=DDLFiltroCategoriaP]').select2();
            $('[id*=DDLFiltroProducto]').select2();
            $('[id*=DDLFiltroOrigen]').select2();
            $('[id*=DDLFiltroProveedor]').select2();
                       
        });

        var prm = Sys.WebForms.PageRequestManager.getInstance();
        if (prm != null) {
            prm.add_endRequest(function (sender, e) {
                if (sender._postBackSettings.panelsToUpdate != null) {
                    $("[id*=DDLFiltroCliente]").select2({ dropdownAutoWidth: true });
                    $("[id*=DDLFiltroCategoriaP]").select2({ dropdownAutoWidth: true });
                    $("[id*=DDLFiltroProducto]").select2({ dropdownAutoWidth: true });
                    $("[id*=DDLFiltroOrigen]").select2({ dropdownAutoWidth: true });
                    $("[id*=DDLFiltroProveedor]").select2({ dropdownAutoWidth: true });
                   
                }
            });
        };
    </script>   
    <div class="container-fluid w-100 h-100">
        <asp:UpdatePanel runat="server" ID="UPContenido" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:Label runat="server" ID="regPId" Visible="false"></asp:Label>
                <asp:Label runat="server" ID="op" Visible="false"></asp:Label>
                <asp:Label runat="server" ID="filtroview" Visible="false"></asp:Label>
                <asp:Label runat="server" ID="gvindexpage" Visible="false"></asp:Label>
                <asp:Label ID="lblMsg" runat="server" Visible="false"></asp:Label>
                <asp:Label ID="CveEstatus" runat="server" Visible="false"></asp:Label>
                 <div class="page-sec-title" align="center">
                    <asp:Label runat="server" ID="PageTitle" CssClass="page-title"></asp:Label>
                </div>
                <div class="panel-group" id="accordion">
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            <h4 class="panel-title" data-toggle="collapse" data-target="#collapseOne">Criterios de Búsqueda:</h4>
                        </div>
                        <div id="collapseOne" class="panel-collapse collapse in">
                            <div class="panel-body">
                                <div class="row2 align-items-center d-flex">
                                    <div class="col-md-2">
                                        <asp:Label runat="server" ID="LBLF1" CssClass="semibold"></asp:Label>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:DropDownList ID="DDLFiltroCliente" runat="server" AutoPostBack="true" Width="95%" ClientIDMode="Static">
                                        </asp:DropDownList>
                                        <asp:Label runat="server" ID="TBNomCliente" CssClass="label-cliente"></asp:Label>
                                    </div>
                                    <div class="col-md-2">
                                        <asp:Label runat="server" ID="LBLF3" CssClass="semibold"></asp:Label>
                                    </div>
                                    <div class="col-md-4" style="display: flex!important;">
                                        <asp:TextBox ID="TBFiltroFecIni" runat="server" CssClass="form-control" Width="110px"></asp:TextBox>
                                        <asp:Label runat="server" ID="Label1" CssClass="semibold" Style="padding: 0px 10px 0px 10px!important;">-</asp:Label>
                                        <asp:TextBox ID="TBFiltroFecFin" runat="server" CssClass="form-control" Width="110px"></asp:TextBox>
                                        <asp:CalendarExtender runat="server" ID="CE1" Animated="true" Format="dd/MM/yyyy" TargetControlID="TBFiltroFecIni"></asp:CalendarExtender>
                                        <asp:CalendarExtender runat="server" ID="CE2" Animated="true" Format="dd/MM/yyyy" TargetControlID="TBFiltroFecFin"></asp:CalendarExtender>
                                    </div>
                                </div>
                                <div class="row2 align-items-center d-flex margin-top-20">                                                                       
                                    <div class="col-md-2">
                                        <asp:Label runat="server" ID="LBLF5" CssClass="semibold"></asp:Label>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:DropDownList ID="DDLFiltroProducto" runat="server" Width="95%" ClientIDMode="Static">
                                        </asp:DropDownList>
                                    </div>
                                     <div class="col-md-2">
                                        <asp:Label runat="server" ID="LBLF6" CssClass="semibold"></asp:Label>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:TextBox ID="TBFiltroReferencia" runat="server" CssClass="form-control" Width="95%"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="row2 align-items-center d-flex margin-top-20">
                                     <div class="col-md-2">
                                        <asp:Label runat="server" ID="LBLF7" CssClass="semibold"></asp:Label>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:TextBox ID="TBFiltroLote" runat="server" CssClass="form-control" Width="95%"></asp:TextBox>
                                    </div>
                                    <div class="col-md-2">
                                        <asp:Label runat="server" ID="LBLF8" CssClass="semibold"></asp:Label>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:TextBox ID="TBFiltroIdentifica" runat="server" CssClass="form-control" Width="95%"></asp:TextBox>
                                    </div>
                                   
                                </div>
                                <div class="row2 align-items-center d-flex margin-top-20">                                   
                                    <div class="col-md-2">
                                        <asp:Label runat="server" ID="LBLF9" CssClass="semibold"></asp:Label>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:DropDownList ID="DDLFiltroOrigen" runat="server" Width="95%" ClientIDMode="Static">
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-md-2">
                                        <asp:Label runat="server" ID="LBLF10" CssClass="semibold"></asp:Label>
                                    </div>
                                    <div class="col-md-4">
                                         <asp:DropDownList ID="DDLFiltroProveedor" runat="server" Width="95%" ClientIDMode="Static">
                                        </asp:DropDownList>
                                    </div>
                                </div>                               
                                <div class="row2 align-items-center d-flex margin-top-20">
                                    <div class="col-md-2 ">
                                        
                                    </div>
                                    <div class="col-md-4 align-items-center d-flex" >
                                        
                                    </div>
                                     <div class="col-md-6 align-items-center">                                        
                                         <asp:Button runat="server" ID="BTNP18" CssClass="btn-action" OnClick="Buscar" Width="80px" UseSubmitBehavior="false"  />
                                    </div>
                                </div>                              
                            </div>
                        </div>
                    </div>
                </div>
                <div class="bar-actions" style="display: inline-block;">
                    <div align="left">
                        <asp:Label runat="server" ID="Mensaje" CssClass="semibold" ></asp:Label>
                        <asp:LinkButton ID="LB46" runat="server" OnClick="Actualizar" CssClass="lnk-action" >
                            <asp:Image ID="IMG46" runat="server" ImageAlign="AbsMiddle" ImageUrl="Content/Image/action_cambios_off.png" />
                            <asp:Label runat="server" ID="LBL46"></asp:Label>
                        </asp:LinkButton>
                    </div>
                </div>                 
                <div style="overflow-y: hidden; height: 100%; width: 70%; ">
                    <%--Grid--%>
                    <asp:GridView ID="gv1" runat="server" AutoGenerateColumns="false" ShowHeader="true"
                        Width="100%" ShowFooter="false" AllowPaging="false" CssClass="datagrid" CellSpacing="0">
                        <HeaderStyle />
                        <Columns>
                            <asp:BoundField DataField="NomParametro" HeaderText="" HeaderStyle-Width="30%" />
                            <asp:BoundField DataField="Cantidad" HeaderText="" HeaderStyle-Width="10%" />
                            <asp:BoundField DataField="ValorMin" HeaderText="" HeaderStyle-Width="10%" />
                            <asp:BoundField DataField="ValorMax" HeaderText="" HeaderStyle-Width="10%" />
                            <asp:BoundField DataField="ValorEsperado" HeaderText="" HeaderStyle-Width="10%" />
                            <asp:BoundField DataField="ValorResultado" HeaderText="" HeaderStyle-Width="10%" />
                             <asp:TemplateField HeaderText="" HeaderStyle-Width="20%">
                                <ItemTemplate>
                                    <asp:Label ID="ValorMin" runat="server" Text='<%# Eval("ValorMin") %>' Visible="false"></asp:Label>
                                    <asp:Label ID="ValorMax" runat="server" Text='<%# Eval("ValorMax") %>' Visible="false"></asp:Label>
                                    <asp:Label ID="CodALLIXPa" runat="server" Text='<%# Eval("CodALLIXPa") %>' Visible="false"></asp:Label>
                                    <asp:TextBox ID="TBValorUsar" runat="server" Text='<%# Eval("ValorResultado")%>' CssClass="form-control tb-sm" MaxLength="5" Width="90px" ></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FTE1" runat="server" FilterType="Custom,Numbers" TargetControlID="TBValorUsar" ValidChars="." />
                                    <asp:RangeValidator ID="RV1" runat="server" ControlToValidate="TBValorUsar" MaximumValue='<%# Eval("ValorMax") %>' MinimumValue='<%# Eval("ValorMin") %>'
                                    ValidationGroup="valid" ForeColor="Red" Type="Double" ErrorMessage="Valor fuera de rango." />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <PagerSettings Visible=" false" />
                    </asp:GridView>
                </div>
                <br /><br />
                <%--SHOW POPUP--%>
                <asp:Label runat="server" ID="mpe_regId" Visible="false"></asp:Label>
                <asp:Label runat="server" ID="mpe_op" Visible="false"></asp:Label>
                <asp:LinkButton Text="" ID="lnkshowdata" runat="server" />
                <asp:ModalPopupExtender ID="mpe_showdata" runat="server" PopupControlID="pnlPopup" TargetControlID="lnkshowdata"
                    BackgroundCssClass="modalBackground">
                </asp:ModalPopupExtender>
                <asp:Panel ID="pnlPopup" runat="server" CssClass="modalPopup" Style="display: none;">
                    <div class="modalPopup_header">
                        <asp:Label runat="server" ID="lblMPE_title"></asp:Label>
                    </div>
                    <div id="MPEbody_Obs" runat="server" class="modalPopup_body" align="left">
                        <table width="100%" cellpadding="2" border="0" style="margin-left: 3px;">
                            <tr>
                                <td valign="top" width="100%">
                                    <asp:TextBox ID="TBNotasE" TextMode="MultiLine" Rows="5" runat="server" CssClass="control-textbox ucase" Width="95%"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                        <br />
                    </div>
                    <div id="MPEbody_Notification" runat="server" class="modalPopup_body" align="left">
                        <table width='100%' cellpadding='3' border="0">
                            <tr>
                                <td valign='top' align='center' width='40px'>
                                    <asp:Image ID="img_notification" runat="server" ImageAlign="Middle" ImageUrl="App_Design/Images/message_confirm.png" Height='32' Width='32' />
                                </td>
                                <td width='310px'>
                                    <asp:Label runat="server" ID="lbl_notification" CssClass="control-label bold"></asp:Label>
                                </td>
                            </tr>
                        </table>

                    </div>
                    <div class="modalPopup_footer" align="center">
                        <asp:Button runat="server" ID="BTNP16" CssClass="btn-action" OnClick="mpe_action" CommandArgument="action_save" Width="80px" UseSubmitBehavior="false" />
                        <asp:Button runat="server" ID="BTNP15" CssClass="btn-action" OnClick="mpe_close" Style="margin-left: 5px;" CommandArgument="action_close" Width="80px" UseSubmitBehavior="false" />
                    </div>
                </asp:Panel>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="LB46" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
</asp:Content>
