<%@ Page Title="" Language="VB" Async="true" MasterPageFile="~/Master_OptimizerC.Master" AutoEventWireup="true" CodeBehind="OptimizerC_ProgramaA_Frm.aspx.vb" Inherits="NukaxanWEB.OptimizerC_ProgramaA_Frm" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">      
<script src="Scripts/WebForms/Message.js"></script>
    <script>
        //$(function () {
        //    $('.nav-item [id*=tab1]').on('click', function (e) {
        //        e.preventDefault();
        //        $("#MainContent_LB14").show();
        //        $("#MainContent_LB12").show();
        //    });
        //    $('.nav-item [id*=tab2]').on('click', function (e) {
        //        e.preventDefault();
        //        $("#MainContent_LB14").hide();
        //        $("#MainContent_LB12").hide();
        //    });
        //});
        function onDataShown(sender, args) {
            sender._popupBehavior._element.style.zIndex = 1000001;
        }
        $(document).ready(function () {
            $('[id*=DDLCliente]').select2();

        });

        var prm = Sys.WebForms.PageRequestManager.getInstance();
        if (prm != null) {
            prm.add_endRequest(function (sender, e) {
                if (sender._postBackSettings.panelsToUpdate != null) {
                    $("[id*=DDLCliente]").select2({ dropdownAutoWidth: true });

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
        <asp:UpdatePanel runat="server" ID="UPContenido" UpdateMode="Conditional" >
            <ContentTemplate>
                <asp:Label runat="server" ID="filtroview" Visible="false"></asp:Label>
                <asp:Label runat="server" ID="gvindexpage" Visible="false"></asp:Label>
                 <asp:Label runat="server" ID="regPId" Visible="false" ></asp:Label>
                <asp:Label runat="server" ID="CodCliente" Visible="false"></asp:Label>
                <asp:Label runat="server" ID="CveModalidad" Visible="false"></asp:Label>
                <asp:Label runat="server" ID="CvePerfilN" Visible="false"></asp:Label>
                 <asp:Label runat="server" ID="CodALLIXCte" Visible="false"></asp:Label>
                 <asp:Label runat="server" ID="CveEstatus" Visible="false"></asp:Label>  
                 <asp:Label runat="server" ID="Autor" Visible="false"></asp:Label>
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
                        <asp:LinkButton ID="LB16" runat="server" OnClick="MostrarPerfil" CssClass="lnkbtn-action">
                            <i runat="server" id="LB_IMG16" class=""></i>
                            <asp:Label runat="server" ID="LB_LBL16">Ver Perfil</asp:Label>
                        </asp:LinkButton>
                        <asp:LinkButton ID="LB14" runat="server" OnClick="Actualizar" CssClass="lnkbtn-action">
                              <i runat="server" id="LB_IMG14" class=""></i>
                            <asp:Label runat="server" ID="LB_LBL14">Obtener Datos NUFEED</asp:Label>
                        </asp:LinkButton>
                      <asp:LinkButton ID="LB12" runat="server" OnClick="Guardar" CssClass="lnkbtn-action">
                             <i runat="server" id="LB_IMG12" class=""></i>
                            <asp:Label runat="server" ID="LB_LBL12">Calcular</asp:Label>
                        </asp:LinkButton>
                        <asp:LinkButton ID="LB2" runat="server" OnClick="Regresar"  CssClass="lnkbtn-action">
                            <i runat="server" id="LB_IMG2" class=""></i>
                            <asp:Label runat="server" ID="LB_LBL2">Salir</asp:Label>
                        </asp:LinkButton>
                    </div>
                </div>               
                <div id="Tabs" role="tabpanel">
                    <!-- Nav tabs -->
                    <ul class="nav nav-pills" id="myTab" role="tablist">
                        <li class="nav-item active" runat="server">
                            <a class="nav-link" id="tab1" href="#sec1" aria-controls="sec1" role="tab" data-toggle="tab">PARÁMETROS</a>
                        </li>
                        <li class="nav-item" runat="server">
                            <a class="nav-link" id="tab2" href="#sec2" aria-controls="sec2" role="tab" data-toggle="tab">PRESUPUESTO OPTIMIZADO</a>
                        </li>
                         <li class="nav-item" runat="server">
                            <a class="nav-link" id="tab3" href="#sec3" aria-controls="sec3" role="tab" data-toggle="tab"">COMPARATIVO</a>
                        </li>                      
                    </ul>
                    <!-- Tab panes -->
                    <div class="tab-content" >                       
                        <div role="tabpanel" class="tab-pane active" id="sec1" >
                            <div class="divsec-captura">
                                <asp:Label runat="server" ID="SECTitulo1"  CssClass="control-label"></asp:Label>
                            </div>
                            <table class="table-condensed" border="1" style="border: solid 1px gray; width: 100%;">
                                <tr style="visibility: collapse;">
                                    <td width="20%" class="bg-gray semibold"></td>
                                    <td width="15%" class="bg-white"></td>
                                    <td width="18%" class="bg-gray semibold"></td>
                                    <td width="13%" class="bg-white"></td>
                                    <td width="17%" class="bg-gray semibold"></td>
                                    <td width="17%" class="bg-white"></td>
                                </tr>
                                <tr>
                                    <td class="bg-gray semibold">
                                        <asp:Label runat="server" ID="LBLC1"></asp:Label>
                                    </td>
                                    <td class="bg-white">
                                        <asp:Label runat="server" ID="TBID" CssClass="control-label color-red"></asp:Label>
                                    </td>
                                    <td class="bg-gray semibold">
                                        <asp:Label runat="server" ID="LBLC2"></asp:Label>
                                    </td>
                                    <td class="bg-white">
                                        <asp:Label runat="server" ID="TBPNID" CssClass="control-label color-red"></asp:Label>
                                    </td>
                                    <td class="bg-gray semibold">
                                        <asp:Label runat="server" ID="LBLC3"></asp:Label>
                                    </td>
                                    <td class="bg-white">
                                        <asp:Label runat="server" ID="TBNomEstatusD" CssClass="control-label "></asp:Label>
                                    </td>
                                </tr>
                            </table>
                            <table class="table-condensed" border="1" style="border: solid 1px gray; width: 100%;">
                                <tr style="visibility: collapse;">
                                    <td width="20%" class="bg-gray semibold"></td>
                                    <td width="30%" class="bg-white"></td>
                                    <td width="20%" class="bg-gray semibold"></td>
                                    <td width="30%" class="bg-white"></td>
                                </tr>
                                <tr>
                                    <td class="bg-gray semibold">
                                        <asp:Label runat="server" ID="LBLC4"></asp:Label>
                                    </td>
                                    <td class="bg-white" colspan="3">
                                        <asp:TextBox ID="TBTitulo" runat="server" CssClass="form-control ucase" AutoCompleteType="Disabled"></asp:TextBox>
                                        <asp:Label runat="server" ID="TBTituloD" CssClass="control-label"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="bg-gray semibold">
                                        <asp:Label runat="server" ID="LBLC6"></asp:Label>
                                    </td>
                                    <td class="bg-white" colspan="3">
                                        <asp:DropDownList ID="DDLCliente" runat="server" AutoPostBack="true" Width="95%" ClientIDMode="Static">
                                        </asp:DropDownList>
                                        <asp:Label runat="server" ID="TBNomClienteD" CssClass="control-label "></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="bg-gray semibold">
                                        <asp:Label runat="server" ID="LBLC8"></asp:Label>
                                    </td>
                                    <td class="bg-white">
                                        <asp:DropDownList ID="DDLModalidad" runat="server" CssClass="form-control" Width="95%" ClientIDMode="Static">
                                        </asp:DropDownList>
                                        <asp:Label runat="server" ID="TBNomModalidadD" CssClass="control-label "></asp:Label>
                                    </td>
                                    <%--<td class="bg-gray semibold"></td>
                                    <td class="bg-white" ></td>--%>
                                </tr>
                            </table>
                            <br />
                            <div class="divsec-captura">
                                <asp:Label runat="server" ID="SECTitulo2" CssClass="control-label"></asp:Label>
                            </div>
                            <table class="table-condensed" border="1" style="border: solid 1px gray; width: 100%;">
                                <tr>                                   
                                    <td width="20%" class="bg-gray semibold">
                                        <asp:Label runat="server" ID="LBLC10"></asp:Label>
                                    </td>
                                    <td width="80%" class="bg-white">
                                        <asp:DropDownList runat="server" ID="DDLReferencia"   CssClass="form-control"  >
                                        </asp:DropDownList>
                                        <asp:Label runat="server" ID="TBReferenciaD" CssClass="control-label"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="bg-gray semibold">
                                        <asp:Label runat="server" ID="LBLC12"></asp:Label>                                        
                                    </td>
                                    <td class="bg-white">
                                        <asp:DropDownList runat="server" ID="DDLParametro"   CssClass="form-control"  >
                                        </asp:DropDownList>
                                        <asp:Label runat="server" ID="TBParametroD" CssClass="control-label"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                            <br />
                            <div class="divsec-captura">
                                <asp:Label runat="server" ID="SECTitulo3"  CssClass="control-label"></asp:Label>
                            </div>
                            <table class="table-condensed" border="1" style="border: solid 1px gray; width: 100%;">
                                <tr>
                                    <td width="20%" class="bg-gray semibold">
                                        <asp:Label runat="server" ID="LBLC14"></asp:Label>
                                    </td>
                                    <td width="30%" class="bg-white">
                                        <div class="div-unidad">
                                            <asp:TextBox ID="TBPesoDestete" runat="server" CssClass="form-control tb-sm" MaxLength="5" AutoCompleteType="Disabled"></asp:TextBox>
                                            <asp:Label runat="server" ID="TBPesoDesteteD" CssClass="control-label"></asp:Label>
                                            <span runat="server" id="TBPesoDesteteU" class="tb-unidad">Kg</span>
                                            <asp:FilteredTextBoxExtender ID="FTE1" runat="server" FilterType="Custom,Numbers" TargetControlID="TBPesoDestete" ValidChars="." />
                                        </div>
                                    </td>
                                    <td width="20%" class="bg-gray semibold">
                                        <asp:Label runat="server" ID="LBLC16"></asp:Label>
                                    </td>
                                    <td width="30%" class="bg-white">
                                        <div class="div-unidad">
                                            <asp:TextBox ID="TBTemperatura" runat="server" CssClass="form-control tb-sm" MaxLength="5" AutoCompleteType="Disabled"></asp:TextBox>
                                            <asp:Label runat="server" ID="TBTemperaturaD" CssClass="control-label"></asp:Label>
                                            <span runat="server" id="TBTemperaturaU" class="tb-unidad">°C</span>
                                            <asp:Label runat="server" ID="LBLH1933" CssClass="text-muted" Style="padding: 5px 7px;"></asp:Label>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" FilterType="Custom,Numbers" TargetControlID="TBTemperatura" ValidChars="." />
                                        </div>
                                    </td>
                                    <tr>
                                <tr>
                                    <td class="bg-gray semibold">
                                        <asp:Label runat="server" ID="LBLC18"></asp:Label>
                                    </td>
                                    <td class="bg-white">
                                        <div class="div-unidad">
                                            <div class="div-unidad">
                                                <asp:TextBox ID="TBEdadDestete" runat="server" CssClass="form-control tb-sm d-flex" MaxLength="5" AutoCompleteType="Disabled"></asp:TextBox>
                                                <asp:Label runat="server" ID="TBEdadDesteteD" CssClass="control-label"></asp:Label>
                                                <span runat="server" id="TBEdadDesteteU" class="tb-unidad">días</span>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" FilterType="Numbers" TargetControlID="TBEdadDestete" />
                                            </div>
                                        </div>
                                    </td>
                                    <td class="bg-gray semibold">
                                        <asp:Label runat="server" ID="LBLC20"></asp:Label>
                                    </td>
                                    <td class="bg-white">
                                        <asp:DropDownList runat="server" ID="DDLEstado" CssClass="form-control" Width="90%">
                                        </asp:DropDownList>
                                        <asp:Label runat="server" ID="TBEstadoD" CssClass="control-label"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="bg-gray semibold">
                                        <asp:Label runat="server" ID="LBLC22"></asp:Label>
                                    </td>
                                    <td class="bg-white">
                                        <div class="div-unidad">
                                            <asp:TextBox ID="TBEdadSalida" runat="server" CssClass="form-control tb-sm" MaxLength="5" AutoCompleteType="Disabled"></asp:TextBox>
                                            <asp:Label runat="server" ID="TBEdadSalidaD" CssClass="control-label"></asp:Label>
                                            <span runat="server" id="TBEdadSalidaU" class="tb-unidad">días</span>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" FilterType="Numbers" TargetControlID="TBEdadSalida" />
                                        </div>
                                    </td>
                                    <td class="bg-gray semibold">
                                        <asp:Label runat="server" ID="LBLC24"></asp:Label>
                                    </td>
                                    <td class="bg-white">
                                        <div class="div-unidad">
                                            <asp:TextBox ID="TBEspacio" runat="server" CssClass="form-control tb-sm" MaxLength="5" AutoCompleteType="Disabled"></asp:TextBox>
                                            <asp:Label runat="server" ID="TBEspacioD" CssClass="control-label"></asp:Label>
                                            <span runat="server" id="TBEspacioU" class="tb-unidad">m2/Cerdo</span>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender8" runat="server" FilterType="Custom,Numbers" TargetControlID="TBEspacio" ValidChars="." />
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="bg-gray semibold">
                                        <asp:Label runat="server" ID="LBLC26"></asp:Label>
                                    </td>
                                    <td class="bg-white">
                                        <div class="div-unidad">
                                            <asp:TextBox ID="TBEdadVenta" runat="server" CssClass="form-control tb-sm" MaxLength="5" AutoCompleteType="Disabled"></asp:TextBox>
                                            <asp:Label runat="server" ID="TBEdadVentaD" CssClass="control-label"></asp:Label>
                                            <span runat="server" id="TBEdadVentaU" class="tb-unidad">días</span>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" FilterType="Numbers" TargetControlID="TBEdadVenta" />
                                        </div>
                                    </td>
                                    <td class="bg-gray semibold">
                                        <asp:Label runat="server" ID="LBLC28"></asp:Label>
                                    </td>
                                    <td class="bg-white">
                                        <div class="div-unidad">
                                            <asp:TextBox ID="TBDesperdicio" runat="server" CssClass="form-control tb-sm" MaxLength="5" AutoCompleteType="Disabled"></asp:TextBox>
                                            <asp:Label runat="server" ID="TBDesperdicioD" CssClass="control-label"></asp:Label>
                                            <span runat="server" id="TBDesperdicioU" class="tb-unidad">%</span>
                                            <asp:Label runat="server" ID="LBLH27" CssClass="text-muted" Style="padding: 5px 7px;"></asp:Label>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender9" runat="server" FilterType="Custom,Numbers" TargetControlID="TBDesperdicio" ValidChars="." />
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="bg-gray semibold">
                                        <asp:Label runat="server" ID="LBLC30"></asp:Label>
                                    </td>
                                    <td class="bg-white">
                                        <asp:TextBox ID="TBPrecioVenta" runat="server" CssClass="form-control tb-sm" MaxLength="5" AutoCompleteType="Disabled"></asp:TextBox>
                                        <asp:Label runat="server" ID="TBPrecioVentaD" CssClass="control-label"></asp:Label>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" FilterType="Custom,Numbers" TargetControlID="TBPrecioVenta" ValidChars="." />
                                    </td>
                                    <td class="bg-gray semibold"></td>
                                    <td class="bg-white"></td>
                                    <%--<td class="bg-gray">
                                        <asp:Label runat="server" ID="LBLC19"></asp:Label>
                                    </td>
                                    <td class="bg-white">
                                        <div class="div-unidad">
                                            <asp:TextBox ID="TBDiasRAC" runat="server" CssClass="form-control tb-sm" MaxLength="5" AutoCompleteType="Disabled"></asp:TextBox>
                                            <asp:Label runat="server" ID="TBDiasRACD" CssClass="control-label"></asp:Label>
                                            <span runat="server" id="TBDiasRAU" class="tb-unidad">días</span>
                                            <asp:Label runat="server" ID="LBLH17" CssClass="text-muted" Style="padding: 5px 7px;"></asp:Label>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" FilterType="Numbers" TargetControlID="TBDiasRAC" />
                                        </div>
                                    </td>--%>
                                </tr>
                            </table>
                            <br />                           
                            <div class="divsec-captura">
                                <asp:Label runat="server" ID="SECTitulo4"  CssClass="control-label"></asp:Label>
                            </div>
                            <div align="left" style="display: block;">
                                <asp:Repeater ID="rptEtapas" runat="server">
                                    <HeaderTemplate>
                                        <table width='100%' cellpadding='3' border="1" class="table table-condensed  table-sm table-rep">
                                            <thead>
                                                <tr>
                                                    <th width='5%'>
                                                        <asp:Label runat="server" ID="Label13"></asp:Label>
                                                    </th>
                                                    <th width='25%'>
                                                        <asp:Label runat="server" ID="Label1"></asp:Label>
                                                    </th>
                                                    <th width='10%' align="center">
                                                        <asp:Label runat="server" ID="Label3">COSTO $/KG</asp:Label>
                                                    </th>
                                                    <%--<th width='10%' align="center">
                                                        <asp:Label runat="server" ID="Label4">RACTOPAMINA PPM</asp:Label>
                                                    </th>--%>
                                                    <th width='10%' align="center">
                                                        <asp:Label runat="server" ID="Label5">EM (KCAL/KG)</asp:Label>
                                                    </th>
                                                    <th width='10%' align="center">
                                                        <asp:Label runat="server" ID="Label7">EN (KCAL/KG)</asp:Label>
                                                    </th>
                                                    <th width='10%' align="center">
                                                        <asp:Label runat="server" ID="Label8">SID LYS (%)</asp:Label>
                                                    </th>                                                     
                                                    <th width='10%' align="center">
                                                        <asp:Label runat="server" ID="Label9">PRESUPUESTO POR CERDO (KG)</asp:Label>
                                                    </th>
                                                    <th width='10%' align="center">
                                                        <asp:Label runat="server" ID="Label10">DURACIÓN MÍNIMA (DÍAS)</asp:Label>
                                                    </th>
                                                    <th width='10%' align="center">
                                                        <asp:Label runat="server" ID="Label11">DURACIÓN MÁXIMA (DÍAS)</asp:Label>
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
                                                <asp:Label runat="server" ID="CveProducto" Text='<%# Eval("CveProducto")%>' Visible="false"></asp:Label>
                                                <asp:Label runat="server" ID="Posicion" Text='<%# Eval("Posicion")%>' Visible="false"></asp:Label>
                                                <asp:Label runat="server" ID="Aplica" Text='<%# Eval("Aplica")%>' Visible="false"></asp:Label>
                                                <asp:Label runat="server" ID="IsEtapa" Text='<%# Eval("IsEtapa")%>' Visible="false"></asp:Label>
                                                <asp:Label runat="server" ID="IsRactopamina" Text='<%# Eval("IsRactopamina")%>' Visible="false"></asp:Label>
                                                <asp:Label runat="server" ID="IsEM" Text='<%# Eval("IsEM")%>' Visible="false"></asp:Label>
                                                <asp:Label runat="server" ID="IsEN" Text='<%# Eval("IsEN")%>' Visible="false"></asp:Label>
                                                <asp:Label runat="server" ID="IsSID" Text='<%# Eval("IsSID")%>' Visible="false"></asp:Label>
                                                <asp:Label runat="server" ID="IsPresupuesto" Text='<%# Eval("IsPresupuesto")%>' Visible="false"></asp:Label>                                                
                                                <asp:Label runat="server" ID="IsDuracionMin" Text='<%# Eval("IsDuracionMin")%>' Visible="false"></asp:Label>
                                                <asp:Label runat="server" ID="IsDuracionMax" Text='<%# Eval("IsDuracionMax")%>' Visible="false"></asp:Label>                                                
                                                <asp:Label runat="server" ID="CA" Text='<%# Eval("CA")%>' Visible="false"></asp:Label>
                                                <asp:Label runat="server" ID="CodALLIX" Text='<%# Eval("CodALLIX")%>' Visible="false"></asp:Label>
                                                <asp:Label runat="server" ID="TBRactopamina" Text='<%# Eval("Ractopamina")%>' Visible="false"></asp:Label>
                                                <asp:Label runat="server" ID="NomProducto" Text='<%# Eval("NomProducto")%>' CssClass="control-value bold "></asp:Label>
                                            </td>
                                            <td align="center" class="justify-content-center">
                                                <asp:TextBox ID="TBCosto" runat="server" Width="95%" Text='<%#Math.Round(CDbl(Eval("Costo")), 2).ToString%>' CssClass="form-control "></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender10" TargetControlID="TBCosto" runat="server" FilterType="Custom,Numbers" ValidChars="."></asp:FilteredTextBoxExtender>
                                                <asp:Label runat="server" ID="TBCostoD" Text='<%# Eval("Costo")%>' CssClass="control-value bold" Visible="false"></asp:Label>
                                            </td>
                                            <%--<td align="center" class="justify-content-center">
                                                <asp:TextBox ID="TBRactopamina" runat="server" Width="95%" Text='<%# Eval("Ractopamina")%>' CssClass="form-control "></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender11" TargetControlID="TBRactopamina" runat="server" FilterType="Custom,Numbers" ValidChars="."></asp:FilteredTextBoxExtender>
                                                <asp:Label runat="server" ID="TBRactopaminaD" Text='<%# Eval("Ractopamina")%>' CssClass="control-value bold" Visible="false"></asp:Label>
                                            </td>--%>
                                            <td align="center" class="justify-content-center">
                                                <asp:TextBox ID="TBEM" runat="server" Width="95%" Text='<%# Eval("EM")%>' CssClass="form-control "></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender12" TargetControlID="TBEM" runat="server" FilterType="Custom,Numbers" ValidChars="."></asp:FilteredTextBoxExtender>
                                                <asp:Label runat="server" ID="TBEMD" Text='<%# Eval("EM")%>' CssClass="control-value bold" Visible="false"></asp:Label>
                                            </td>
                                            <td align="center" class="justify-content-center">
                                                <asp:TextBox ID="TBEN" runat="server" Width="95%" Text='<%# Eval("EN")%>' CssClass="form-control "></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender13" TargetControlID="TBEN" runat="server" FilterType="Custom,Numbers" ValidChars="."></asp:FilteredTextBoxExtender>
                                                <asp:Label runat="server" ID="TBEND" Text='<%# Eval("EN")%>' CssClass="control-value bold" Visible="false"></asp:Label>
                                            </td>
                                            <td align="center" class="justify-content-center">
                                                <asp:TextBox ID="TBSIDLYS" runat="server" Width="95%" Text='<%# Math.Round(CDbl(Eval("SIDLYS")), 2).ToString%>' CssClass="form-control "></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender14" TargetControlID="TBSIDLYS" runat="server" FilterType="Custom,Numbers" ValidChars="."></asp:FilteredTextBoxExtender>
                                                <asp:Label runat="server" ID="TBSIDLYSD" Text='<%# Eval("SIDLYS")%>' CssClass="control-value bold" Visible="false"></asp:Label>
                                            </td>                                           
                                            <td align="center" class="justify-content-center">
                                                <asp:TextBox ID="TBPresupuesto" runat="server" Width="95%" Text='<%# Eval("Presupuesto")%>' CssClass="form-control "></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender16" TargetControlID="TBPresupuesto" runat="server" FilterType="Custom,Numbers" ValidChars="."></asp:FilteredTextBoxExtender>
                                                <asp:Label runat="server" ID="TBPresupuestoD" Text='<%# Eval("Presupuesto")%>' CssClass="control-value bold" Visible="false"></asp:Label>
                                            </td>
                                            <td align="center" class="justify-content-center">
                                                <asp:TextBox ID="TBDuracionMin" runat="server" Width="95%" Text='<%# Eval("DuracionMin")%>' CssClass="form-control "></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender17" TargetControlID="TBDuracionMin" runat="server" FilterType="Numbers" ></asp:FilteredTextBoxExtender>
                                                <asp:Label runat="server" ID="TBDuracionMinD" Text='<%# Eval("DuracionMin")%>' CssClass="control-value bold" Visible="false"></asp:Label>
                                            </td>
                                             <td align="center" class="justify-content-center">
                                                <asp:TextBox ID="TBDuracionMax" runat="server" Width="95%" Text='<%# Eval("DuracionMax")%>' CssClass="form-control "></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender18" TargetControlID="TBDuracionMax" runat="server" FilterType="Numbers" ></asp:FilteredTextBoxExtender>
                                                <asp:Label runat="server" ID="TBDuracionMaxD" Text='<%# Eval("DuracionMax")%>' CssClass="control-value bold" Visible="false"></asp:Label>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                    <FooterTemplate>                                       
                                        </table>
                                    </FooterTemplate>
                                </asp:Repeater>
                            </div>
                           
                             <div class="divsec-captura">
                                <asp:Label runat="server" ID="SECTitulo5"  CssClass="control-label"></asp:Label>
                            </div>
                            <table class="table-condensed" border="1" style="border: solid 1px gray; width: 100%;">
                                <tr style="visibility: collapse;">
                                    <td width="20%" class="bg-gray semibold"></td>
                                    <td width="80%" class="bg-white"></td>                                    
                                </tr>
                                <tr>
                                    <td class="bg-gray semibold">
                                        <asp:Label runat="server" ID="LBLG32"></asp:Label>
                                    </td>
                                    <td class="bg-white" colspan="3">                                        
                                        <asp:Label runat="server" ID="TBFecAltaD" CssClass="control-label color-blue"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="bg-gray semibold">
                                        <asp:Label runat="server" ID="LBLG33"></asp:Label>
                                    </td>
                                    <td class="bg-white" colspan="3">                                        
                                        <asp:Label runat="server" ID="TBFecActD" CssClass="control-label color-blue"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div role="tabpanel" class="tab-pane" id="sec2" >
                            <div align="left" style="display: block;">
                                <asp:Repeater ID="rptResultado" runat="server">
                                    <HeaderTemplate>
                                        <table width='100%' cellpadding='3' border="1" class="table table-condensed  table-sm table-rep">
                                            <thead>
                                                <tr>
                                                    <th width='18%'>
                                                        <asp:Label runat="server" ID="Label3"></asp:Label>
                                                    </th>
                                                    <th width='10%'>
                                                        <asp:Label runat="server" ID="Label4">COSTO<br />-$/kg-</asp:Label>
                                                    </th>
                                                    <th width='8%'>
                                                        <asp:Label runat="server" ID="Label6">CDA<br>-Kg-</asp:Label>
                                                    </th>
                                                    <th width='8%'>
                                                        <asp:Label runat="server" ID="Label5">PRESUPUESTO POR CERDO<br />-Kg-</asp:Label>
                                                    </th>
                                                    <th width='8%'>
                                                        <asp:Label runat="server" ID="Label7">GDP<br />-Kg-</asp:Label>
                                                    </th>
                                                    <th width='8%'>
                                                        <asp:Label runat="server" ID="Label8">PESO INICIAL<br />-Kg-</asp:Label>
                                                    </th>
                                                    <th width='8%'>
                                                        <asp:Label runat="server" ID="Label9">PESO FINAL<br />-Kg-</asp:Label>
                                                    </th>
                                                    <th width='8%'>
                                                        <asp:Label runat="server" ID="Label10">CA</asp:Label>
                                                    </th>
                                                    <th width='8%'>
                                                        <asp:Label runat="server" ID="Label11">EDAD INICIAL<br />-Días-</asp:Label>
                                                    </th>
                                                    <th width='8%'>
                                                        <asp:Label runat="server" ID="Label12">EDAD FINAL<br />-Días-</asp:Label>
                                                    </th>
                                                    <th width='8%'>
                                                        <asp:Label runat="server" ID="Label13">DURACIÓN ETAPA<br />-Días-</asp:Label>
                                                    </th>
                                                </tr>
                                            </thead>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr>
                                           <td align="left ">
                                                        <asp:Label runat="server" ID="NomProducto" Text='<%# Eval("NomProducto")%>' CssClass="control-value bold "></asp:Label>                                                        
                                                    </td>
                                                    <td align="center">                                                        
                                                        <asp:Label runat="server" ID="Costo" Text='<%#CDbl(Eval("Costo")).ToString("C2")%>'  ></asp:Label>
                                                    </td>
                                                    <td align="center">                                                        
                                                        <asp:Label runat="server" ID="CDA" Text='<%# CDbl(Eval("CDA")).ToString("n2")%>' ></asp:Label>
                                                    </td>
                                                    <td align="center">                                                        
                                                        <asp:Label runat="server" ID="Presupuesto" Text='<%# CDbl(Eval("Presupuesto")).ToString("n1")%>' ></asp:Label>
                                                    </td>                                                   
                                                     <td align="center">                                                        
                                                        <asp:Label runat="server" ID="GDP" Text='<%# CDbl(Eval("GDP")).ToString("n3")%>' ></asp:Label>
                                                    </td>
                                                    <td align="center">                                                       
                                                        <asp:Label runat="server" ID="PesoInicial" Text='<%# CDbl(Eval("PesoInicial")).ToString("n1")%>' ></asp:Label>
                                                    </td>
                                                    <td align="center">                                                        
                                                        <asp:Label runat="server" ID="PesoFinal" Text='<%# CDbl(Eval("PesoFinal")).ToString("n1")%>'></asp:Label>
                                                    </td>
                                                    <td align="center">                                                        
                                                        <asp:Label runat="server" ID="CA" Text='<%# CDbl(Eval("CA")).ToString("n2")%>' ></asp:Label>
                                                    </td>
                                                    <td align="center">                                                        
                                                        <asp:Label runat="server" ID="EdadInicial" Text='<%# Math.Round(CDbl(Eval("EdadInicial")), 0).ToString%>'></asp:Label>
                                                    </td>
                                                     <td align="center">                                                        
                                                        <asp:Label runat="server" ID="EdadFinal" Text='<%# Math.Round(CDbl(Eval("EdadFinal")), 0).ToString%>'></asp:Label>
                                                    </td>
                                                     <td align="center">                                                        
                                                        <asp:Label runat="server" ID="Duracion" Text='<%# Math.Round(CDbl(Eval("Duracion")), 0).ToString("n0")%>'></asp:Label>
                                                    </td>
                                        </tr>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <tfoot>
                                        <tr>
                                            <th>
                                                <asp:Label runat="server" ID="LBLT1"></asp:Label>
                                            </th>
                                            <th>
                                                <asp:Label runat="server" ID="TOTCosto"></asp:Label>
                                            </th>
                                            <th>
                                                <asp:Label runat="server" ID="TOTCDA"></asp:Label>
                                            </th>
                                            <th>
                                                <asp:Label runat="server" ID="TOTPresupuesto"></asp:Label>
                                            </th>
                                            <th>
                                                <asp:Label runat="server" ID="TOTGDP"></asp:Label>
                                            </th>
                                            <th>
                                                <asp:Label runat="server" ID="Label8"></asp:Label>
                                            </th>
                                            <th>
                                                <asp:Label runat="server" ID="Label9"></asp:Label>
                                            </th>
                                            <th>
                                                <asp:Label runat="server" ID="TOTCA">CA</asp:Label>
                                            </th>
                                            <th>
                                                <asp:Label runat="server" ID="Label11"></asp:Label>
                                            </th>
                                            <th>
                                                <asp:Label runat="server" ID="Label12"></asp:Label>
                                            </th>
                                            <th>
                                                <asp:Label runat="server" ID="TOTDuracion"></asp:Label>
                                            </th>
                                        </tr>
                                            </tfoot>
                                        </table>
                                    </FooterTemplate>
                                </asp:Repeater>
                            </div>
                            <br />
                            <table runat="server" id="tbl_resultado" cellpadding='3' border="1" class="table table-condensed  table-sm " style="width:50%!important;">
                                <thead>
                                    <tr>
                                        <th width="60%" style="text-align:left!important;">PRECIO DE VENTA ($/Kg):</th>
                                        <td width="40%"><asp:Label runat="server" ID="Tot_PrecioVenta"></asp:Label></td>
                                    </tr>
                                     <tr>
                                        <th style="text-align:left!important;">PESO DE VENTA (Kg):</th>
                                        <td><asp:Label runat="server" ID="Tot_PesoVenta"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <th style="text-align:left!important;">EDAD DE VENTA (días):</th>
                                        <td><asp:Label runat="server" ID="Tot_EdadVenta"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <th style="text-align:left!important;">ALIMENTO PRESUPUESTADO (Kg):</th>
                                        <td><asp:Label runat="server" ID="Tot_AlimentoPresupuestado"></asp:Label></td>
                                    </tr>
                                     <tr>
                                        <th style="text-align:left!important;">KILOS PRODUCIDOS (Kg):</th>
                                        <td><asp:Label runat="server" ID="Tot_KilosProducidos"></asp:Label></td>
                                    </tr>
                                     <tr>
                                        <th style="text-align:left!important;">GDP (Kg):</th>
                                        <td><asp:Label runat="server" ID="Tot_GDP"></asp:Label></td>
                                    </tr>
                                     <tr>
                                        <th style="text-align:left!important;">CONVERSIÓN ALIMENTICIA:</th>
                                        <td><asp:Label runat="server" ID="Tot_CA"></asp:Label></td>
                                    </tr>
                                     <tr>
                                        <th style="text-align:left!important;">COSTO TOTAL DEL ALIMENTO ($):</th>
                                        <td><asp:Label runat="server" ID="Tot_CostoTotal"></asp:Label></td>
                                    </tr>
                                     <tr>
                                        <th style="text-align:left!important;">COSTO PONDERADO DE ALIMENTO ($):</th>
                                        <td><asp:Label runat="server" ID="Tot_CostoPonderado"></asp:Label></td>
                                    </tr>
                                     <tr>
                                        <th style="text-align:left!important;">COSTO POR KILO PRODUCIDO, ALIMENTO ($):</th>
                                        <td><asp:Label runat="server" ID="Tot_CostoKiloProd"></asp:Label></td>
                                    </tr>
                                     <tr>
                                        <th style="text-align:left!important;">UTILIDAD POR CONCEPTO DE ALIMENTO ($):</th>
                                        <td><asp:Label runat="server" ID="Tot_Utilidad"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <th style="text-align:left!important;">ROI, ALIMENTO (%):</th>
                                        <td><asp:Label runat="server" ID="Tot_ROI"></asp:Label></td>
                                    </tr>
                                </thead>
                            </table>
                        </div>
                        <div role="tabpanel" class="tab-pane" id="sec3" >
                            <div align="left" style="display: block;">
                                <div class="divsec-captura">
                                    <asp:Label runat="server" ID="SECTitulo6" CssClass="control-label"></asp:Label>
                                </div>                                                                
                                <asp:GridView ID="gv2" runat="server" AutoGenerateColumns="false" ShowHeader="true" 
                                     Width="100%" ShowFooter="true" AllowPaging="false" CellPadding="1" CssClass="table table-condensed  table-sm table-rep">                                     
                                     <HeaderStyle /> 
                                     <Columns>
                                     </Columns>
                                     <PagerSettings Visible=" false" />
                                 </asp:GridView>
                             </div>
                            <br />
                             <div align="left" style="display: block;">
                                <div class="divsec-captura">
                                    <asp:Label runat="server" ID="SECTitulo7" CssClass="control-label"></asp:Label>
                                </div>    
                                 <asp:GridView ID="gv" runat="server" AutoGenerateColumns="false" ShowHeader="true" 
                                     Width="100%" ShowFooter="false" AllowPaging="false" CellPadding="1" CssClass="table table-condensed  table-sm table-rep">
                                     <HeaderStyle />
                                     <Columns>
                                     </Columns>
                                     <PagerSettings Visible=" false" />
                                 </asp:GridView>
                             </div>
                            <br />
                        </div>                       
                    </div>                                 
                    <asp:HiddenField ID="TabName" runat="server" ClientIDMode="Static" />
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
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
