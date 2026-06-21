<%@ Page Title="" Language="VB" Async="true" AutoEventWireup="true" MasterPageFile="~/Master_ProdAves.Master" CodeBehind="ProdAves_Postura_Lotes_Registro2_Frm.aspx.vb" Inherits="NukaxanWEB.ProdAves_Postura_Lotes_Registro2_Frm" %>

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
                <div style="width: 100%; height: 100%; margin-top: 10px;">
                    <table width="100%" runat="server" class="table_noborder" border="0" style="border: 0px;" clientidmode="Static">
                        <tr>
                            <td colspan="2" align="center">
                                <div style="padding: 3px 0px;">
                                    <asp:Label runat="server" ID="TBNombreTitulo" CssClass="pagina_titulo"></asp:Label>
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
                                <div class="bar-actions" style="display: flex; width: 100%;">
                                    <asp:Label runat="server" ID="LBLCaseta" CssClass="control-label" Style="margin-right: 10px;">Caseta: </asp:Label>
                                    <asp:DropDownList runat="server" ID="DDLLoteRecepcion" AutoPostBack="true" Width="300px" CssClass="form-control semibold">
                                    </asp:DropDownList>
                                    <asp:LinkButton ID="LB45" runat="server" OnClick="AbrirAgregar" CssClass="lnk-action" Style="margin-left: 20px;">
                                        <asp:Image ID="IMG45" runat="server" ImageAlign="AbsMiddle" ImageUrl="Content/Image/icon_file_excel.png" />
                                        <asp:Label runat="server" ID="LBL45"></asp:Label>
                                    </asp:LinkButton>
                                </div>
                                <div id="Tabs" role="tabpanel">
                                    <!-- Nav tabs -->
                                    <ul class="nav nav-pills" id="myTab" role="tablist">
                                        <li class="nav-item active" runat="server">
                                            <a class="nav-link" id="tab1" href="#sec1" aria-controls="sec1" role="tab" data-toggle="tab">MORTALIDAD</a>
                                        </li>
                                        <li class="nav-item " runat="server">
                                            <a class="nav-link" id="tab2" href="#sec1" aria-controls="sec2" role="tab" data-toggle="tab">PESOS</a>
                                        </li>
                                        <li class="nav-item " runat="server">
                                            <a class="nav-link" id="tab3" href="#sec1" aria-controls="sec3" role="tab" data-toggle="tab">CONSUMOS</a>
                                        </li>
                                    </ul>
                                    <!-- Tab panes -->
                                    <div class="tab-content2">
                                        <div role="tabpanel" class="tab-pane2 active" id="sec1">
                                            
                                        </div>
                                        <div role="tabpanel" class="tab-pane active" id="sec2">
                                        </div>
                                    </div>
                                    <asp:HiddenField ID="TabName" runat="server" ClientIDMode="Static" />
                                </div>
                                <div align="left" style="display: block; width: 100%; overflow-y: scroll; height: 90%; height: 300px;">
                                    <asp:Repeater ID="rptRegistros" runat="server">
                                        <HeaderTemplate>
                                            <table width='115%' cellpadding='3' border="0" class="datagrid">
                                                <thead>
                                                    <tr>
                                                        <th width='6%'>
                                                            <asp:Label runat="server" ID="Label3">EDAD</asp:Label>
                                                        </th>
                                                        <th width='5%'>
                                                            <asp:Label runat="server" ID="Label1">DÍA</asp:Label>
                                                        </th>
                                                        <th width='9%'>
                                                            <asp:Label runat="server" ID="Label4">FECHA</asp:Label>
                                                        </th>
                                                        <th width='5%'>
                                                            <asp:Label runat="server" ID="Label6">CICLO</asp:Label>
                                                        </th>
                                                        <th width='8%'>
                                                            <asp:Label runat="server" ID="Label7">MORTALIDAD</asp:Label>
                                                        </th>
                                                        <th width='8%'>
                                                            <asp:Label runat="server" ID="Label5">AVES<br />AJUSTE</asp:Label>
                                                        </th>
                                                        <th width='8%'>
                                                            <asp:Label runat="server" ID="Label8">AVES FINALES</asp:Label>
                                                        </th>
                                                        <th width='8%'>
                                                            <asp:Label runat="server" ID="Label9">PESO AVE</asp:Label>
                                                        </th>
                                                        <th width='8%'>
                                                            <asp:Label runat="server" ID="Label10">UNIF<br />AVE %</asp:Label>
                                                        </th>
                                                        <th width='10%'>
                                                            <asp:Label runat="server" ID="Label12">PRODUCCIÓN<br />TOTAL</asp:Label>
                                                        </th>
                                                        <th width='9%'>
                                                            <asp:Label runat="server" ID="Label13">PESO<br />HUEVO (gr)</asp:Label>
                                                        </th>
                                                        <th width='8%'>
                                                            <asp:Label runat="server" ID="Label14">UNIF<br />HUEVO %</asp:Label>
                                                        </th>
                                                        <th width='10%'>
                                                            <asp:Label runat="server" ID="Label2">ALIM<br />SERV (KG)</asp:Label>
                                                        </th>
                                                        <th width='10%'>
                                                            <asp:Label runat="server" ID="Label11">AGUA<br />SERV(L)</asp:Label>
                                                        </th>
                                                    </tr>
                                                </thead>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <tr>
                                                <td align="center">
                                                    <asp:Label runat="server" ID="CodCliente" Text='<%# Eval("CodCliente")%>' Visible="false"></asp:Label>
                                                    <asp:Label runat="server" ID="CveLote" Text='<%# Eval("CveLote")%>' Visible="false"></asp:Label>
                                                    <asp:Label runat="server" ID="CveLoteR" Text='<%# Eval("CveLoteR")%>' Visible="false"></asp:Label>
                                                    <asp:Label runat="server" ID="CveRegistro" Text='<%# Eval("CveRegistro")%>' Visible="false"></asp:Label>
                                                    <asp:Label runat="server" ID="Edad" Text='<%# Eval("Edad")%>' CssClass="control-value "></asp:Label>
                                                </td>
                                                <td align="center " class="justify-content-center">
                                                    <asp:Label runat="server" ID="NumDia" Text='<%# Eval("NumDia")%>' CssClass="control-value "></asp:Label>
                                                </td>
                                                <td align="center " class="justify-content-center">
                                                    <asp:Label runat="server" ID="FolioR" Text='<%# Convert.ToDateTime(Eval("FecCaptura")).ToString("dd/MM/yyyy")%>' CssClass="control-value bold"></asp:Label>
                                                    <asp:HoverMenuExtender runat="server" ID="HME1" TargetControlID="FolioR" PopupControlID="pnl_action" PopupPosition="Right">
                                                    </asp:HoverMenuExtender>
                                                    <asp:Panel ID="pnl_action" runat="server" CssClass="panel-action">
                                                        <asp:ImageButton runat="server" ID="IB1" OnClick="Editar" CommandArgument='<%# Eval("CveRegistro") %>' ImageUrl="Content/Image/icon_file_jpg.png"
                                                            ImageAlign="AbsMiddle" />
                                                        <asp:ImageButton runat="server" ID="IB2" OnClick="Eliminar" CommandArgument='<%# Eval("CveRegistro") %>' ImageUrl="Content/Image/icon_file_jpg.png"
                                                            ImageAlign="AbsMiddle" />
                                                    </asp:Panel>
                                                </td>
                                                <td align="center">
                                                    <asp:Label runat="server" ID="NomCiclo" Text='<%# Eval("NomCiclo")%>' CssClass="control-value "></asp:Label>
                                                </td>
                                                <td align="center">
                                                    <asp:Label runat="server" ID="AvesMuertas" Text='<%# CDbl(Eval("AvesMuertas")).ToString("N0")%>' CssClass="control-value "></asp:Label>
                                                </td>
                                                <td align="center">
                                                    <asp:Label runat="server" ID="AjusteAves" Text='<%# CDbl(Eval("AjusteAves")).ToString("N0")%>' CssClass="control-value "></asp:Label>
                                                </td>
                                                <td align="center">
                                                    <asp:Label runat="server" ID="AvesFinal" Text='<%# CDbl(Eval("AvesFinal")).ToString("N0")%>' CssClass="control-value "></asp:Label>
                                                </td>
                                                <td align="center">
                                                    <asp:Label runat="server" ID="PesoAve" Text='<%# CDbl(Eval("PesoAve")).ToString("N2")%>' CssClass="control-value "></asp:Label>
                                                </td>
                                                <td align="center">
                                                    <asp:Label runat="server" ID="UniforAve" Text='<%# CDbl(Eval("UniforAve")).ToString("N2")%>' CssClass="control-value "></asp:Label>
                                                </td>
                                                <td align="center">
                                                    <asp:Label runat="server" ID="TotalHuevos" Text='<%# CDbl(Eval("TotalHuevos")).ToString("N0")%>' CssClass="control-value "></asp:Label>
                                                </td>
                                                <td align="center">
                                                    <asp:Label runat="server" ID="PesoHuevo" Text='<%# CDbl(Eval("PesoHuevo")).ToString("N2")%>' CssClass="control-value "></asp:Label>
                                                </td>
                                                <td align="center">
                                                    <asp:Label runat="server" ID="UniforHuevo" Text='<%# CDbl(Eval("UniforHuevo")).ToString("N2")%>' CssClass="control-value "></asp:Label>
                                                </td>
                                                <td align="center">
                                                    <asp:Label runat="server" ID="AlimentoServido" Text='<%# CDbl(Eval("AlimentoServido")).ToString("N2")%>' CssClass="control-value "></asp:Label>
                                                </td>
                                                <td align="center">
                                                    <asp:Label runat="server" ID="AguaServida" Text='<%# CDbl(Eval("AguaServida")).ToString("N2")%>' CssClass="control-value "></asp:Label>
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
                        <table width="100%" class="table table_noborder" border="1" style="border: solid 1px #efefef;">
                            <tr style="visibility: collapse;">
                                <td width="20%"></td>
                                <td width="30%"></td>
                                <td width="20%"></td>
                                <td width="30%"></td>
                            </tr>
                            <tr>
                                <td class="table-td">
                                    <asp:Label runat="server" ID="LBLC3" CssClass="control-label"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="TBFechaE" CssClass="control-value color-red"></asp:Label>
                                    <asp:Label runat="server" ID="TBEdadE" Visible="false"></asp:Label>
                                    <asp:Label runat="server" ID="TBNumDiaE" Visible="false"></asp:Label>
                                </td>
                                <td class="table-td">
                                    <asp:Label runat="server" ID="LBLC4" CssClass="control-label"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList runat="server" ID="DDLCicloE" Width="200px" CssClass="form-control">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="table-td">
                                    <asp:Label runat="server" ID="LBLC5" CssClass="control-label"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="TBAvesMuertasE" runat="server" CssClass="form-control " Width="150px" AutoCompleteType="Disabled"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FTE1" TargetControlID="TBAvesMuertasE" runat="server" FilterType="Numbers"></asp:FilteredTextBoxExtender>
                                </td>
                                <td class="table-td">
                                    <asp:Label runat="server" ID="LBLC6" CssClass="control-label"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="TBAvesAjusteE" runat="server" CssClass="form-control " Width="150px" AutoCompleteType="Disabled"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FTE2" TargetControlID="TBAvesAjusteE" runat="server" FilterType="Numbers"></asp:FilteredTextBoxExtender>
                                </td>
                            </tr>
                            <tr>
                                <td class="table-td">
                                    <asp:Label runat="server" ID="LBLC7" CssClass="control-label"></asp:Label>
                                </td>
                                <td>
                                    <div style="display: flex;">
                                        <asp:TextBox ID="TBAlimServE" runat="server" CssClass="form-control " Width="150px" AutoCompleteType="Disabled"></asp:TextBox>
                                        <span runat="server" id="Span2" class="tb-unidad">Kg</span>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" TargetControlID="TBAlimServE" runat="server" FilterType="Numbers,Custom" ValidChars="."></asp:FilteredTextBoxExtender>
                                    </div>
                                </td>
                                <td class="table-td">
                                    <asp:Label runat="server" ID="LBLC8" CssClass="control-label"></asp:Label>
                                </td>
                                <td>
                                    <div style="display: flex;">
                                        <asp:TextBox ID="TBAguaServE" runat="server" CssClass="form-control " Width="150px" AutoCompleteType="Disabled"></asp:TextBox>
                                        <span runat="server" id="Span3" class="tb-unidad">Lts</span>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" TargetControlID="TBAguaServE" runat="server" FilterType="Numbers,Custom" ValidChars="."></asp:FilteredTextBoxExtender>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td class="table-td">
                                    <asp:Label runat="server" ID="LBLC9" CssClass="control-label"></asp:Label>
                                </td>
                                <td>
                                    <div style="display: flex;">
                                        <asp:TextBox ID="TBPesoAveE" runat="server" CssClass="form-control " Width="150px" AutoCompleteType="Disabled"></asp:TextBox>
                                        <span runat="server" id="TBTemperaturaU" class="tb-unidad">Kg</span>
                                        <asp:FilteredTextBoxExtender ID="FT3" TargetControlID="TBPesoAveE" runat="server" FilterType="Numbers,Custom" ValidChars="."></asp:FilteredTextBoxExtender>
                                    </div>
                                </td>
                                <td class="table-td">
                                    <asp:Label runat="server" ID="LBLC10" CssClass="control-label"></asp:Label>
                                </td>
                                <td>
                                    <div style="display: flex;">
                                        <asp:TextBox ID="TBUnifAveE" runat="server" CssClass="form-control " Width="150px" AutoCompleteType="Disabled"></asp:TextBox>
                                        <span runat="server" id="Span1" class="tb-unidad">%</span>
                                        <asp:FilteredTextBoxExtender ID="FT4" TargetControlID="TBUnifAveE" runat="server" FilterType="Numbers,Custom" ValidChars="."></asp:FilteredTextBoxExtender>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td class="table-td">
                                    <asp:Label runat="server" ID="LBLC11" CssClass="control-label"></asp:Label>
                                </td>
                                <td colspan="3">
                                    <div style="display: flex;">
                                        <asp:TextBox ID="TBCVAveE" runat="server" CssClass="form-control " Width="150px" AutoCompleteType="Disabled"></asp:TextBox>
                                        <span runat="server" id="Span6" class="tb-unidad">%</span>
                                        <asp:FilteredTextBoxExtender ID="FT5" TargetControlID="TBCVAveE" runat="server" FilterType="Numbers,Custom" ValidChars="."></asp:FilteredTextBoxExtender>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td class="table-td">
                                    <asp:Label runat="server" ID="LBLC14" CssClass="control-label"></asp:Label>
                                </td>
                                <td colspan="3" valign="top">
                                    <asp:DataList ID='DLProduccion' runat='server' DataKeyField="CveClasificacionH" RepeatColumns="2" CellSpacing="3" RepeatDirection="Horizontal" Width="100%"
                                        RepeatLayout="Table">
                                        <HeaderTemplate>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <td width="30%">
                                                <asp:Label runat="server" ID="CveClasificacionH" Text='<%# Eval("CveClasificacionH")%>' Visible="false"></asp:Label>
                                                <asp:Label runat="server" ID="PiezasCaja" Text='<%# Eval("PiezasCaja")%>' Visible="false"></asp:Label>
                                                <asp:Label runat="server" ID="ConosCaja" Text='<%# Eval("ConosCaja")%>' Visible="false"></asp:Label>
                                                <asp:Label runat="server" ID="PiezasCono" Text='<%# Eval("PiezasCono")%>' Visible="false"></asp:Label>
                                                <span runat="server" id="Span3" class="tb-unidad" style="font-size: 12px;"><%# Eval("NomClasificacionH")%></span>
                                            </td>
                                            <td width="20%" height="35px">
                                                <asp:TextBox ID="TBTotalE" runat="server" Text='<%# Eval("Total")%>' CssClass="form-control " Width="70px" AutoCompleteType="Disabled"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" TargetControlID="TBTotalE" runat="server" FilterType="Numbers"></asp:FilteredTextBoxExtender>
                                            </td>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                        </FooterTemplate>
                                    </asp:DataList>
                                </td>
                            </tr>
                            <tr>
                                <td class="table-td">
                                    <asp:Label runat="server" ID="LBLC12" CssClass="control-label"></asp:Label>
                                </td>
                                <td>
                                    <div style="display: flex;">
                                        <asp:TextBox ID="TBPesoHuevoE" runat="server" CssClass="form-control " Width="150px" AutoCompleteType="Disabled"></asp:TextBox>
                                        <span runat="server" id="Span4" class="tb-unidad">gr</span>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" TargetControlID="TBPesoHuevoE" runat="server" FilterType="Numbers,Custom" ValidChars="."></asp:FilteredTextBoxExtender>
                                    </div>
                                </td>
                                <td class="table-td">
                                    <asp:Label runat="server" ID="LBLC13" CssClass="control-label"></asp:Label>
                                </td>
                                <td>
                                    <div style="display: flex;">
                                        <asp:TextBox ID="TBUnifHuevoE" runat="server" CssClass="form-control " Width="150px" AutoCompleteType="Disabled"></asp:TextBox>
                                        <span runat="server" id="Span5" class="tb-unidad">%</span>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" TargetControlID="TBUnifHuevoE" runat="server" FilterType="Numbers,Custom" ValidChars="."></asp:FilteredTextBoxExtender>
                                    </div>
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
