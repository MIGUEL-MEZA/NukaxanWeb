<%@ Page Title="" Language="VB" Async="true" MasterPageFile="~/Master_Nireo.Master" AutoEventWireup="true" CodeBehind="Nireo_CatEquipos_View.aspx.vb" Inherits="NukaxanWEB.Nireo_CatEquipos_View" %>

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
    </script>
    <div class="container-fluid w-100 h-100">
        
        <asp:UpdatePanel runat="server" ID="UPContenido" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:Label runat="server" ID="regPId" Visible="false"></asp:Label>
                <asp:Label runat="server" ID="op" Visible="false"></asp:Label>
                <asp:Label runat="server" ID="CveEstatus" Visible="false"></asp:Label>
                <asp:Label runat="server" ID="filtroview" Visible="false"></asp:Label>
                <asp:Label runat="server" ID="gvindexpage" Visible="false"></asp:Label>
                <asp:Label ID="lblMsg" runat="server" Visible="false"></asp:Label>
                <div class="page-sec-title" align="center">
                    <asp:Label runat="server" ID="PageTitle" CssClass="page-title"></asp:Label>
                </div>
                <div class="bar-actions" style="display: inline-block;">
                    <div align="left">
                        <asp:LinkButton ID="LB44" runat="server" OnClick="Exportar" CssClass="lnk-action">
                            <asp:Image ID="IMG44" runat="server" ImageAlign="AbsMiddle" ImageUrl="Content/Image/icon_file_excel.png" />
                            <asp:Label runat="server" ID="LBL44"></asp:Label>
                        </asp:LinkButton>
                        <asp:LinkButton ID="LB45" runat="server" OnClick="Agregar" CssClass="lnk-action">
                            <asp:Image ID="IMG45" runat="server" ImageAlign="AbsMiddle" ImageUrl="Content/Image/icon_file_excel.png" />
                            <asp:Label runat="server" ID="LBL45"></asp:Label>
                        </asp:LinkButton>
                    </div>
                </div>
                <asp:Panel ID="Panel1" runat="server" DefaultButton="btnInvisible" Style="display: inline-block; float: right;margin-left:15px;">
                    <asp:LinkButton ID="LB41" Font-Underline="false" runat="server" CssClass="lnk-action" OnClick="FiltrosAbrir">
                        <asp:Image ID="IMG41" runat="server" ImageAlign="AbsMiddle" Width="24px" Height="24px" ImageUrl="Content/Image/action_filter.png" />
                        <asp:Label runat="server" ID="LBL41" CssClass="lnk_action"></asp:Label>
                    </asp:LinkButton>
                    <asp:LinkButton ID="LB42" Font-Underline="false" runat="server" CssClass="lnk-action" OnClick="FiltrosModificar" Visible="false">
                        <asp:Image ID="IMG42" runat="server" ImageAlign="AbsMiddle" Width="24px" Height="24px" ImageUrl="Content/Image/action_filter_edit.png" />
                        <asp:Label runat="server" ID="LBL42" CssClass="lnk_action"></asp:Label>
                    </asp:LinkButton>
                    <asp:LinkButton ID="LB43" Font-Underline="false" runat="server" CssClass="lnk-action" OnClick="FiltrosCancelar" Visible="false">
                        <asp:Image ID="IMG43" runat="server" ImageAlign="AbsMiddle" Width="24px" Height="24px" ImageUrl="Content/Image/action_filter_cancel.png" />
                        <asp:Label runat="server" ID="LBL43" CssClass="lnk_action"></asp:Label>
                    </asp:LinkButton>
                    &nbsp&nbsp
                   <asp:TextBox ID="TBFiltroGen" placeholder="Buscar" Width="320px" runat="server" CssClass="form-control" AutoComplete="off" Style="display: inline-block!important;"></asp:TextBox>
                    <asp:Button ID="btnInvisible" runat="server" Style="display: none" OnClick="FiltrosAplicar" CommandArgument="filtro_gen" />
                </asp:Panel>

                <div style="overflow-y: scroll; height: 59vh; width: 100%; border: solid 0px red;">
                    <%--Grid--%>
                    <asp:GridView ID="gv1" runat="server" AutoGenerateColumns="false" DataKeyNames="CveEquipo" ShowHeader="true"
                        Width="110%" ShowFooter="false" AllowPaging="True" CellPadding="1"
                        CssClass="datagrid" >
                        <HeaderStyle />
                        <Columns>
                            <asp:BoundField DataField="CveEquipo" HeaderText="" HeaderStyle-Width="5%" />
                            <asp:BoundField DataField="NomEquipo" HeaderText="" HeaderStyle-Width="13%" />
                            <asp:TemplateField HeaderText="" HeaderStyle-Width="10%">
                                <ItemTemplate>
                                    <asp:Label ID="FolioR" runat="server" Text='<%# Eval("Serie") %>'></asp:Label>
                                    <asp:HoverMenuExtender runat="server" ID="HME1"  TargetControlID="FolioR" PopupControlID="pnl_action" PopupPosition="Right">
                                    </asp:HoverMenuExtender>
                                    <asp:Panel ID="pnl_action" runat="server" CssClass="panel-action">
                                        <asp:ImageButton runat="server" ID="IB1" OnClick="Editar" CommandArgument='<%# Eval("CveEquipo") %>' ImageUrl="Content/Image/icon_file_jpg.png" 
                                            ImageAlign="AbsMiddle" />                                       
                                        <%--<asp:ImageButton runat="server" ID="IB2" OnClick="Eliminar" CommandArgument='<%# Eval("CveEquipo") %>' ImageUrl="Content/Image/icon_file_jpg.png" 
                                            ImageAlign="AbsMiddle"  />--%>
                                    </asp:Panel>
                                </ItemTemplate>
                            </asp:TemplateField>                            
                            <asp:BoundField DataField="Marca" HeaderText="" HeaderStyle-Width="8%" />                            
                            <asp:BoundField DataField="Modelo" HeaderText="" HeaderStyle-Width="8%" />
                            <asp:BoundField DataField="NomProveedor" HeaderText="" HeaderStyle-Width="13%" />
                            <asp:BoundField DataField="FechaCompra" HeaderText="" HeaderStyle-Width="8%" DataFormatString="{0:dd/MM/yyyy}"/>
                            <asp:BoundField DataField="Costo" HeaderText="" HeaderStyle-Width="7%" DataFormatString="{0:N2}"/>
                            <asp:BoundField DataField="NomMoneda" HeaderText="" HeaderStyle-Width="7%" />
                            <asp:BoundField DataField="NomEstatus" HeaderText="" HeaderStyle-Width="8%" />
                            <asp:BoundField DataField="FecAct" HeaderText="" HeaderStyle-Width="8%" DataFormatString="{0:dd/MM/yyyy}" />
                            <asp:BoundField DataField="NomUsuAct" HeaderText="" HeaderStyle-Width="15%" />
                        </Columns>
                        <PagerSettings Visible=" false" />
                    </asp:GridView>
                </div>
                <div runat="server" class="datagrid-footer" id="tblpaging">
                    <div class="row align-items-center d-flex">
                        <div class="col-md-3">
                             <asp:Label runat="server" ID="totreg" Visible="true"></asp:Label>
                        </div>
                        <div class="col-md-6" align="center">
                             <asp:Button runat="server" ID="BTNP11" OnClick="GVPaging" CommandArgument="First" CssClass="button-paging" />
                                <asp:Button runat="server" ID="BTNP12" OnClick="GVPaging" CommandArgument="Prev" CssClass="button-paging" />
                                &nbsp;<asp:Label ID="pageLabel" runat="server"> </asp:Label>&nbsp;
                            <asp:Button runat="server" ID="BTNP13" OnClick="GVPaging" CommandArgument="Next" CssClass="button-paging" />
                                <asp:Button runat="server" ID="BTNP14" OnClick="GVPaging" CommandArgument="Last" CssClass="button-paging" />
                        </div>
                        <div class="col-md-3">
                             <asp:Label runat="server" ID="Label2" Visible="true"></asp:Label>
                        </div>
                    </div>
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
                    <div id="MPEbody_Filtros" runat="server" class="modalPopup_body" align="left">
                        <table width='100%' class="table table_noborder" cellpadding='3' border="0" >
                            <tr style="visibility: collapse;">
                                <td width="25%"></td>
                                <td width="75%"></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label runat="server" ID="LBLF1" class="semibold"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList runat="server" ID="DDLFiltroEstatus" Width="95%" CssClass="form-control">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label runat="server" ID="LBLF2" class="semibold"></asp:Label>
                                </td>
                                <td>
                                   <asp:TextBox ID="TBFiltroProveedor"  runat="server" CssClass="form-control ucase" Width="100%"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label runat="server" ID="LBLF3" class="semibold"></asp:Label>
                                </td>
                                <td>
                                   <asp:TextBox ID="TBFiltroMarca"  runat="server" CssClass="form-control ucase" Width="100%"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label runat="server" ID="LBLF4" class="semibold"></asp:Label>
                                </td>
                                <td>
                                   <asp:TextBox ID="TBFiltroSerie"  runat="server" CssClass="form-control ucase" Width="100%"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                      
                    </div>
                    <div id="MPEBody_Captura" runat="server" class="modalPopup_body" align="left" style="overflow-y: scroll; height: 450px;">
                    <table width="100%" class="table table_noborder" border="0">
                        <tr style="visibility: collapse;">
                            <td width="25%"></td>
                            <td width="75%"></td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label runat="server" ID="LBLC1" CssClass="control-label"></asp:Label>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="TBIDE" CssClass="control-label semibold"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top">
                                <asp:Label runat="server" ID="LBLC2" CssClass="control-label"></asp:Label>
                            </td>
                            <td valign="top">
                                <asp:TextBox ID="TBNombreE"  runat="server" CssClass="form-control ucase" Width="100%"></asp:TextBox>
                            </td>
                        </tr>
                         <tr>
                            <td valign="top">
                                <asp:Label runat="server" ID="LBLC3" CssClass="control-label"></asp:Label>
                            </td>
                            <td valign="top">
                                <asp:TextBox ID="TBSerieE"  runat="server" CssClass="form-control ucase" Width="100%"></asp:TextBox>
                            </td>
                        </tr>
                         <tr>
                            <td valign="top">
                                <asp:Label runat="server" ID="LBLC4" CssClass="control-label"></asp:Label>
                            </td>
                            <td valign="top">
                                <asp:TextBox ID="TBMarcaE"  runat="server" CssClass="form-control ucase" Width="100%"></asp:TextBox>
                            </td>
                        </tr>
                         <tr>
                            <td valign="top">
                                <asp:Label runat="server" ID="LBLC5" CssClass="control-label"></asp:Label>
                            </td>
                            <td valign="top">
                                <asp:TextBox ID="TBModeloE"  runat="server" CssClass="form-control ucase" Width="100%"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top">
                                <asp:Label runat="server" ID="LBLC6" CssClass="control-label"></asp:Label>
                            </td>
                            <td valign="top">
                                <asp:TextBox ID="TBProveedorE"  runat="server" CssClass="form-control ucase" Width="100%"></asp:TextBox>
                            </td>
                        </tr>
                         <tr>
                            <td valign="top">
                                <asp:Label runat="server" ID="LBLC7" CssClass="control-label"></asp:Label>
                            </td>
                            <td valign="top">
                                <asp:TextBox ID="TBFecCompraE"  runat="server" CssClass="form-control " Width="120px"></asp:TextBox>
                                <asp:CalendarExtender runat="server" ID="CalendarExtender9" Animated="true" Format="dd/MM/yyyy" TargetControlID="TBFecCompraE"></asp:CalendarExtender>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top">
                                <asp:Label runat="server" ID="LBLC8" CssClass="control-label"></asp:Label>
                            </td>
                            <td valign="top">
                                <asp:TextBox ID="TBCostoE"  runat="server" CssClass="form-control " Width="120px"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FTE1" runat="server" FilterType="Numbers,Custom" TargetControlID="TBCostoE" ValidChars="." />
                            </td>
                        </tr>
                        <tr>
                            <td valign="top">
                                <asp:Label runat="server" ID="LBLC9" CssClass="control-label"></asp:Label>
                            </td>
                            <td valign="top">
                                 <asp:DropDownList runat="server" ID="DDLMonedaE" Width="95%" CssClass="form-control">
                                    </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top">
                                <asp:Label runat="server" ID="LBLC10" CssClass="control-label"></asp:Label>
                            </td>
                            <td valign="top">
                                 <asp:DropDownList runat="server" ID="DDLEstatusE" Width="95%" CssClass="form-control">
                                    </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                        <br />
                </div>
                    <div class="modalPopup_footer" align="center">
                        <asp:Button runat="server" ID="BTNP16" CssClass="btn-action" OnClick="mpe_action" CommandArgument="action_save" Width="80px" UseSubmitBehavior="false" />
                        <asp:Button runat="server" ID="BTNP15" CssClass="btn-action" OnClick="mpe_close"  style="margin-left:5px;" CommandArgument="action_close" Width="80px" UseSubmitBehavior="false" />
                    </div>
                </asp:Panel>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="LB44" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
</asp:Content>
