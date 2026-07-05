<%@ Page Title="" Language="VB" Async="true" MasterPageFile="~/Master_OptimizerG.Master" AutoEventWireup="true" CodeBehind="OptimizerG_ProgramaA_Frm.aspx.vb" Inherits="NukaxanWEB.OptimizerG_ProgramaA_Frm" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <script src="Scripts/WebForms/Message.js"></script>
    <script>
        function onDataShown(sender, args) {
            sender._popupBehavior._element.style.zIndex = 1000001;
        }
        $(document).ready(function () {
            $('[id*=DDLCliente]').select2();
            inicializarTabsPrograma();

        });

        function actualizarBotonesReportePrograma(tabKey) {
            var buttons = $("[id$='LB20'],[id$='LB21']");
            var disabled = tabKey === 'parametros';

            buttons.each(function () {
                var button = $(this);
                button.data('disabled', disabled);
                button.toggleClass('lnkbtn-action_disabled', disabled);
                button.css({
                    'pointer-events': disabled ? 'none' : '',
                    'opacity': disabled ? '0.6' : '',
                    'cursor': disabled ? 'not-allowed' : 'pointer'
                });
            });
        }

        function inicializarTabsPrograma() {
            var tabName = $("#TabName");
            if (!tabName.length) {
                return;
            }

            if (!tabName.val()) {
                tabName.val('parametros');
            }

            $('#myTab a[data-toggle="tab"]').off('shown.bs.tab.programa').on('shown.bs.tab.programa', function (e) {
                var href = $(e.target).attr('href');
                var selectedTab = href === '#sec2' ? 'presupuesto' : 'parametros';
                tabName.val(selectedTab);
                actualizarBotonesReportePrograma(selectedTab);
            });

            if (tabName.val() === 'presupuesto') {
                $('#tab2').tab('show');
            } else {
                $('#tab1').tab('show');
            }

            actualizarBotonesReportePrograma(tabName.val());
        }

        var prm = Sys.WebForms.PageRequestManager.getInstance();
        if (prm != null) {
            prm.add_endRequest(function (sender, e) {
                if (sender._postBackSettings.panelsToUpdate != null) {
                    $("[id*=DDLCliente]").select2({ dropdownAutoWidth: true });
                    inicializarTabsPrograma();

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
        align-items: flex-start; /* 🔥 Alinea todo arriba */
    }

    .header-left {
        display: flex;
        align-items: flex-start; /* 🔥 clave */
        gap: 5px;
    }

    .logo-left {
        width: 85px;             /* 🔥 más grande */
        margin-right: 8px;      /* 🔥 más espacio */        
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
    background: #0b2e57!important;
    color:#ffffff!important;
    font-weight:normal!important;
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
.table-rep{    
    border-collapse: separate;
    border-spacing: 0;
}


.table-rep th {
    border-right: 1px solid #0f2f52;
}

.table-rep thead tr th:last-child {
    border-right: none; /* 🔥 evita el borde cuadrado */
}

.table-rep thead tr th:first-child {
    border-top-left-radius: 10px;
}

.table-rep thead tr th:last-child {
    border-top-right-radius: 10px;
}

.table-rep tbody tr:nth-child(odd) {
    background-color: #f5f5f5;
}

.table-rep tfoot th {
    background: #0b2e57 !important;
    color: #ffffff !important;
    font-weight: normal !important;
}

.categoria{
     background-color: #eef3f8;
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
                        <asp:LinkButton ID="LB20" runat="server" OnClick="DescargarExcel" CssClass="lnkbtn-action">
                            <i runat="server" id="LB_IMG20" class=""></i>
                            <asp:Label runat="server" ID="LB_LBL20">Excel</asp:Label>
                        </asp:LinkButton>
                        <asp:LinkButton ID="LB21" runat="server" OnClick="DescargarPdf" CssClass="lnkbtn-action">
                            <i runat="server" id="LB_IMG21" class=""></i>
                            <asp:Label runat="server" ID="LB_LBL21">PDF</asp:Label>
                        </asp:LinkButton>
                        <asp:LinkButton ID="LB2" runat="server" OnClick="Regresar" CssClass="lnkbtn-action">
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
                    </ul>
                    <!-- Tab panes -->
                    <div class="tab-content">
                        <div role="tabpanel" class="tab-pane active" id="sec1">
                            <div class="divsec-captura">
                                <asp:Label runat="server" ID="SECTitulo1" CssClass="control-label"></asp:Label>
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
                                    <td width="80%" class="bg-white" colspan="3">
                                        <asp:DropDownList runat="server" ID="DDLReferencia" Width="95%" CssClass="form-control">
                                        </asp:DropDownList>
                                        <asp:Label runat="server" ID="TBReferenciaD" CssClass="control-label"></asp:Label>
                                    </td>
                                </tr>
                                <%-- <tr>
                                    <td class="bg-gray semibold">
                                        <asp:Label runat="server" ID="LBLC12"></asp:Label>                                        
                                    </td>
                                    <td class="bg-white">
                                        <asp:DropDownList runat="server" ID="DDLParametro"   CssClass="form-control"  >
                                        </asp:DropDownList>
                                        <asp:Label runat="server" ID="TBParametroD" CssClass="control-label"></asp:Label>
                                    </td>
                                </tr>--%>
                            </table>
                            <br />
                            <div class="divsec-captura">
                                <asp:Label runat="server" ID="SECTitulo3" CssClass="control-label"></asp:Label>
                            </div>
                            <table class="table-condensed" border="1" style="border: solid 1px gray; width: 100%;">
                                <tr>
                                    <td width="20%" class="bg-gray semibold">
                                        <asp:Label runat="server" ID="LBLC12"></asp:Label>
                                    </td>
                                    <td width="30%" class="bg-white">
                                        <asp:TextBox ID="TBTotalGallinas" runat="server" CssClass="form-control tb-sm" AutoCompleteType="Disabled"></asp:TextBox>
                                        <asp:Label runat="server" ID="TBTotalGallinasD" CssClass="control-label"></asp:Label>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" FilterType="Numbers" TargetControlID="TBTotalGallinas" />
                                    </td>
                                    <td class="bg-gray semibold">
                                        <asp:Label runat="server" ID="LBLC16"></asp:Label>
                                    </td>
                                    <td class="bg-white">
                                        <asp:TextBox ID="TBPrecioVentaH" runat="server" CssClass="form-control tb-sm" AutoCompleteType="Disabled"></asp:TextBox>
                                        <asp:Label runat="server" ID="TBPrecioVentaHD" CssClass="control-label"></asp:Label>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" FilterType="Numbers, Custom" TargetControlID="TBPrecioVentaH" ValidChars="." />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="bg-gray semibold">
                                        <asp:Label runat="server" ID="LBLC14"></asp:Label>
                                    </td>
                                    <td class="bg-white">
                                        <asp:TextBox ID="TBTotalPollitas" runat="server" CssClass="form-control tb-sm" AutoCompleteType="Disabled"></asp:TextBox>
                                        <asp:Label runat="server" ID="TBTotalPollitasD" CssClass="control-label"></asp:Label>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" FilterType="Numbers" TargetControlID="TBTotalPollitas" />
                                    </td>
                                    <td class="bg-gray semibold"></td>
                                    <td class="bg-white"></td>
                                </tr>
                            </table>
                            <br />
                            <div class="divsec-captura">
                                <asp:Label runat="server" ID="SECTitulo4" CssClass="control-label"></asp:Label>
                            </div>
                            <div align="left" style="display: block;">
                                <asp:Repeater ID="rptEtapas" runat="server">
                                    <HeaderTemplate>
                                        <table width='60%' style="max-width: 60%" cellpadding='3' border="1" class="table table-condensed  table-sm table-rep">
                                            <thead>
                                                <tr>
                                                    <th width='5%'>
                                                        <asp:Label runat="server" ID="Label13"></asp:Label>
                                                    </th>
                                                    <th width='80%'>
                                                        <asp:Label runat="server" ID="Label1">ETAPA</asp:Label>
                                                    </th>
                                                    <th width='15%' align="center">
                                                        <asp:Label runat="server" ID="Label3">COSTO FORMULA ($/Kg)</asp:Label>
                                                    </th>
                                                </tr>
                                            </thead>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <td align="center ">
                                                <asp:CheckBox ID="chk" runat="server" AutoPostBack="true" OnCheckedChanged="OnCheckedChanged" ClientIDMode="Static" />
                                            </td>
                                            <td align="left " class="fw-bold">
                                                <asp:Label runat="server" ID="CveEtapa" Text='<%# Eval("CveEtapa")%>' Visible="false"></asp:Label>
                                                <asp:Label runat="server" ID="Aplica" Text='<%# Eval("Aplica")%>' Visible="false"></asp:Label>
                                                <asp:Label runat="server" ID="Fija" Text='<%# Eval("Fija")%>' Visible="false"></asp:Label>
                                                <asp:Label runat="server" ID="CveTipo" Text='<%# Eval("CveTipo")%>' Visible="false"></asp:Label>
                                                <asp:Label runat="server" ID="CodALLIX" Text='<%# Eval("CodALLIX")%>' Visible="false"></asp:Label>
                                                <asp:Label runat="server" ID="EdadIni" Text='<%# Eval("EdadIni")%>' Visible="false"></asp:Label>
                                                <asp:Label runat="server" ID="EdadFin" Text='<%# Eval("EdadFin")%>' Visible="false"></asp:Label>
                                                <asp:Label runat="server" ID="Mortalidad" Text='<%# Eval("Mortalidad")%>' Visible="false"></asp:Label>
                                                <asp:Label runat="server" ID="ConsumoAlimento" Text='<%# Eval("ConsumoAlimento")%>' Visible="false"></asp:Label>
                                                <asp:Label runat="server" ID="PesoHuevo" Text='<%# Eval("PesoHuevo")%>' Visible="false"></asp:Label>
                                                <asp:Label runat="server" ID="Produccion" Text='<%# Eval("Produccion")%>' Visible="false"></asp:Label>
                                                <asp:Label runat="server" ID="NomEtapa" Text='<%# Eval("NomEtapa")%>' CssClass="control-value bold "></asp:Label>
                                            </td>
                                            <td align="center" class="justify-content-center">
                                                <asp:TextBox ID="TBCosto" runat="server" Width="90%" Text='<%#Math.Round(CDbl(Eval("Costo")), 2).ToString%>' CssClass="form-control "></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender10" TargetControlID="TBCosto" runat="server" FilterType="Custom,Numbers" ValidChars="."></asp:FilteredTextBoxExtender>
                                                <asp:Label runat="server" ID="TBCostoD" Text='<%# Eval("Costo")%>' CssClass="control-value bold" Visible="false"></asp:Label>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        </table>
                                    </FooterTemplate>
                                </asp:Repeater>
                            </div>

                            <div class="divsec-captura">
                                <asp:Label runat="server" ID="SECTitulo5" CssClass="control-label"></asp:Label>
                            </div>
                            <table class="table-condensed" border="1" style="border: solid 1px gray; width: 100%;">
                                <tr style="visibility: collapse;">
                                    <td width="20%" class="bg-gray semibold"></td>
                                    <td width="80%" class="bg-white"></td>
                                </tr>
                                <tr>
                                    <td class="bg-gray semibold">
                                        <asp:Label runat="server" ID="LBLG18"></asp:Label>
                                    </td>
                                    <td class="bg-white" colspan="3">
                                        <asp:Label runat="server" ID="TBFecAltaD" CssClass="control-label color-blue"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="bg-gray semibold">
                                        <asp:Label runat="server" ID="LBLG19"></asp:Label>
                                    </td>
                                    <td class="bg-white" colspan="3">
                                        <asp:Label runat="server" ID="TBFecActD" CssClass="control-label color-blue"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div role="tabpanel" class="tab-pane" id="sec2">
                            <div align="left" style="display: block;">
                                <asp:Repeater ID="rptResultado" runat="server">
                                    <HeaderTemplate>
                                        <table width='100%' cellpadding='3' border="1" class="table table-condensed  table-sm table-rep">
                                            <thead>
                                                <tr>
                                                    <th width='20%'>
                                                        <asp:Label runat="server" ID="Label3">ETAPA</asp:Label>
                                                    </th>
                                                    <th width='8%'>
                                                        <asp:Label runat="server" ID="Label2">COSTO FÓRMULA ($/Kg)</asp:Label>
                                                    </th>
                                                    <th width='8%'>
                                                        <asp:Label runat="server" ID="Label4">EDAD INICIAL (SEM)</asp:Label>
                                                    </th>
                                                    <th width='8%'>
                                                        <asp:Label runat="server" ID="Label6">EDAD FINAL (SEM)</asp:Label>
                                                    </th>
                                                  <%--  <th width='10%'>
                                                        <asp:Label runat="server" ID="Label5"># SEMANAS</asp:Label>
                                                    </th>
                                                    <th width='10%'>
                                                        <asp:Label runat="server" ID="Label14">#DÍAS/FASE</asp:Label>
                                                    </th>--%>
                                                    <th width='8%'>
                                                        <asp:Label runat="server" ID="Label7">MORTALIDAD ACUMULADA (%)</asp:Label>
                                                    </th>
                                                    <th width='8%'>
                                                        <asp:Label runat="server" ID="Label8">NO. AVES AJUSTADO A MORTALIDAD</asp:Label>
                                                    </th>
                                                    <th width='8%'>
                                                        <asp:Label runat="server" ID="Label9">CONSUMO ALIMENTO (Kg AVE/FASE)</asp:Label>
                                                    </th>
                                                  <%--  <th width='10%'>
                                                        <asp:Label runat="server" ID="Label10">CONSUMO ALIMENTO TOTAL (Kg/#AVES)</asp:Label>
                                                    </th>--%>
                                                    <th width='8%'>
                                                        <asp:Label runat="server" ID="Label11">PESO HUEVO (gr)</asp:Label>
                                                    </th>
                                                    <th width='8%'>
                                                        <asp:Label runat="server" ID="Label12">PRODUCCIÓN (%)</asp:Label>
                                                    </th>
                                                    <th width='8%'>
                                                        <asp:Label runat="server" ID="Label15">MASA DE HUEVO (gr)</asp:Label>
                                                    </th>
                                                    <th width='8%'>
                                                        <asp:Label runat="server" ID="Label16">CONVERSIÓN ALIMENTICIA</asp:Label>
                                                    </th>
                                                   <%-- <th width='10%'>
                                                        <asp:Label runat="server" ID="Label17">HUEVO PRODUCIDO (TON/FASE)</asp:Label>
                                                    </th>--%>
                                                </tr>
                                            </thead>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <td align="left ">
                                                <asp:Label runat="server" ID="CveEtapa" Text='<%#Eval("CveEtapa")%>' Visible="false"></asp:Label>
                                                <asp:Label runat="server" ID="NomEtapa" Text='<%# Eval("NomEtapa")%>' CssClass="control-value bold "></asp:Label>
                                            </td>
                                            <td align="center">
                                                <asp:Label runat="server" ID="Costo" Text='<%#CDbl(Eval("Costo")).ToString("C2")%>'></asp:Label>
                                            </td>
                                            <td align="center">
                                                <asp:Label runat="server" ID="EdadInicial" Text='<%#Eval("EdadInicial").ToString%>'></asp:Label>
                                            </td>
                                            <td align="center">
                                                <asp:Label runat="server" ID="EdadFinal" Text='<%#Eval("EdadFinal").ToString%>'></asp:Label>
                                            </td>
                                          <%--  <td align="center">
                                                <asp:Label runat="server" ID="Semanas" Text='<%#Eval("Semanas").ToString%>'></asp:Label>
                                            </td>
                                            <td align="center">
                                                <asp:Label runat="server" ID="Dias" Text='<%#Eval("Dias").ToString%>'></asp:Label>
                                            </td>--%>
                                            <td align="center">
                                                <asp:Label runat="server" ID="Mortalidad" Text='<%#CDbl(Eval("Mortalidad")).ToString("N2")%>'></asp:Label>
                                            </td>
                                            <td align="center">
                                                <asp:Label runat="server" ID="NoAves" Text='<%#CDbl(Eval("NoAves")).ToString("N0")%>'></asp:Label>
                                            </td>
                                            <td align="center">
                                                <asp:Label runat="server" ID="ConsumoAlimento" Text='<%#CDbl(Eval("ConsumoAlimento")).ToString("N2")%>'></asp:Label>
                                            </td>
                                           <%-- <td align="center">
                                                <asp:Label runat="server" ID="ConsumoAlimentoTotal" Text='<%#CDbl(Eval("ConsumoAlimentoTotal")).ToString("N2")%>'></asp:Label>
                                            </td>--%>
                                            <td align="center">
                                                <asp:Label runat="server" ID="PesoHuevo" Text='<%#CDbl(Eval("PesoHuevo")).ToString("N2")%>'></asp:Label>
                                            </td>
                                            <td align="center">
                                                <asp:Label runat="server" ID="Produccion" Text='<%#CDbl(Eval("Produccion")).ToString("N2")%>'></asp:Label>
                                            </td>
                                            <td align="center">
                                                <asp:Label runat="server" ID="MasaHuevo" Text='<%#CDbl(Eval("MasaHuevo")).ToString("N2")%>'></asp:Label>
                                            </td>
                                            <td align="center">
                                                <asp:Label runat="server" ID="ConversionAlimenticia" Text='<%#CDbl(Eval("ConversionAlimenticia")).ToString("N2")%>'></asp:Label>
                                            </td>
                                        <%--    <td align="center">
                                                <asp:Label runat="server" ID="HuevoProducido" Text='<%#CDbl(Eval("HuevoProducido")).ToString("N2")%>'></asp:Label>
                                            </td>--%>
                                        </tr>
                                    </ItemTemplate>
                                     <FooterTemplate>
                                        <tfoot>
                                            <tr>
                                                <th colspan="6">
                                                    <asp:Label runat="server" ID="LBLT1"></asp:Label>
                                                </th>
                                                <th>
                                                    <asp:Label runat="server" ID="TOTConsumoAlimento"></asp:Label>
                                                </th>                                               
                                                <th colspan="2">
                                                    <asp:Label runat="server" ID="Label5"></asp:Label>
                                                </th>
                                                <th >
                                                    <asp:Label runat="server" ID="TOTMasaHUevo"></asp:Label>
                                                </th>
                                                 <th >
                                                    <asp:Label runat="server" ID="TOTCA"></asp:Label>
                                                </th>
                                            </tr>
                                            </tfoot>
                                        </table>
                                    </FooterTemplate>
                                </asp:Repeater>
                            </div>
                            <br />
                            <table id="tbl_resultado" cellpadding='3' border="0" class="table table-condensed  table-sm table-rep" style="width: 50%!important;">
                                <thead>
                                    <tr>
                                        <th colspan="2">ANÁLISIS ECONONÓMICO TOTAL</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr>
                                       <td style="text-align: left!important;">
                                            <asp:Label runat="server" ID="LBLCostoProgramaAlimTotal" CssClass="semibold"></asp:Label>
                                        </td>
                                        <td >
                                            <asp:Label runat="server" ID="TBCostoProgramaAlimTotal"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="60%" style="text-align: left!important;">
                                            <asp:Label runat="server" ID="LBLCostoPonderadoAlimTotal" CssClass="semibold"></asp:Label>
                                        </td>
                                        <td width="40%">
                                            <asp:Label runat="server" ID="TBCostoPonderadoAlimTotal"></asp:Label>
                                        </td>
                                    </tr>                                  
                                    <tr>
                                       <td style="text-align: left!important;">
                                            <asp:Label runat="server" ID="LBLCostoKGProducido" CssClass="semibold"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label runat="server" ID="TBCostoKGProducido"></asp:Label>
                                        </td>
                                    </tr>                                                                    
                                    <tr>
                                       <td style="text-align: left!important;">
                                            <asp:Label runat="server" ID="LBLMasaHuevoKGParvada" CssClass="semibold"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label runat="server" ID="TBMasaHuevoKGParvada"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                       <td style="text-align: left!important;">
                                            <asp:Label runat="server" ID="LBLPrecioVentaHuevo" CssClass="semibold"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label runat="server" ID="TBPrecioVentaHuevo"></asp:Label>
                                        </td>
                                    </tr> 
                                    <tr>
                                       <td style="text-align: left!important;">
                                            <asp:Label runat="server" ID="LBLIngresoxVentaHuevo" CssClass="semibold"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label runat="server" ID="TBIngresoxVentaHuevo"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                       <td style="text-align: left!important;">
                                            <asp:Label runat="server" ID="LBLUtilidadBruta" CssClass="semibold"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label runat="server" ID="TBUtilidadBruta"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                       <td style="text-align: left!important;">
                                            <asp:Label runat="server" ID="LBLROI" CssClass="semibold"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label runat="server" ID="TBROI"></asp:Label>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                            <br />
                            <table id="tbl_resultado2" cellpadding='3' border="0" class="table table-condensed  table-sm table-rep" style="width: 50%!important;">
                                <thead>
                                    <tr>
                                        <th colspan="2">ANÁLISIS ECONÓMICO PRODUCTIVO CRIANZA</th>
                                    </tr>
                                </thead>
                                <tbody>                                   
                                    <tr>
                                        <td width="60%" style="text-align: left!important;">
                                            <asp:Label runat="server" ID="LBAEPC1" CssClass="semibold"></asp:Label>
                                        </td>
                                        <td width="40%">
                                            <asp:Label runat="server" ID="TBAEPC1"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="60%" style="text-align: left!important;">
                                            <asp:Label runat="server" ID="LBAEPC2" CssClass="semibold"></asp:Label>
                                        </td>
                                        <td width="40%">
                                            <asp:Label runat="server" ID="TBAEPC2"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="60%" style="text-align: left!important;">
                                            <asp:Label runat="server" ID="LBAEPC3" CssClass="semibold"></asp:Label>
                                        </td>
                                        <td width="40%">
                                            <asp:Label runat="server" ID="TBAEPC3"></asp:Label>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                            <br />
                            <table id="tbl_resultado3" cellpadding='3' border="0" class="table table-condensed  table-sm table-rep" style="width: 50%!important;">
                                <thead>
                                    <tr>
                                        <th colspan="2">ANÁLISIS ECONÓMICO PRODUCTIVO POSTURA</th>
                                    </tr>
                                </thead>
                                <tbody>                                   
                                    <tr>
                                        <td width="60%" style="text-align: left!important;">
                                            <asp:Label runat="server" ID="LBAEPP1" CssClass="semibold"></asp:Label>
                                        </td>
                                        <td width="40%">
                                            <asp:Label runat="server" ID="TBAEPP1"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="60%" style="text-align: left!important;">
                                            <asp:Label runat="server" ID="LBAEPP2" CssClass="semibold"></asp:Label>
                                        </td>
                                        <td width="40%">
                                            <asp:Label runat="server" ID="TBAEPP2"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="60%" style="text-align: left!important;">
                                            <asp:Label runat="server" ID="LBAEPP3" CssClass="semibold"></asp:Label>
                                        </td>
                                        <td width="40%">
                                            <asp:Label runat="server" ID="TBAEPP3"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="60%" style="text-align: left!important;">
                                            <asp:Label runat="server" ID="LBAEPP4" CssClass="semibold"></asp:Label>
                                        </td>
                                        <td width="40%">
                                            <asp:Label runat="server" ID="TBAEPP4"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="60%" style="text-align: left!important;">
                                            <asp:Label runat="server" ID="LBAEPP5" CssClass="semibold"></asp:Label>
                                        </td>
                                        <td width="40%">
                                            <asp:Label runat="server" ID="TBAEPP5"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="60%" style="text-align: left!important;">
                                            <asp:Label runat="server" ID="LBAEPP6" CssClass="semibold"></asp:Label>
                                        </td>
                                        <td width="40%" >
                                            <asp:Label runat="server" ID="TBAEPP6"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="60%" style="text-align: left!important;">
                                            <asp:Label runat="server" ID="LBAEPP7" CssClass="semibold"></asp:Label>
                                        </td>
                                        <td width="40%">
                                            <asp:Label runat="server" ID="TBAEPP7"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="60%" style="text-align: left!important;">
                                            <asp:Label runat="server" ID="LBAEPP8" CssClass="semibold"></asp:Label>
                                        </td>
                                        <td width="40%">
                                            <asp:Label runat="server" ID="TBAEPP8"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="60%" style="text-align: left!important;">
                                            <asp:Label runat="server" ID="LBAEPP9" CssClass="semibold"></asp:Label>
                                        </td>
                                        <td width="40%">
                                            <asp:Label runat="server" ID="TBAEPP9"></asp:Label>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
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
             <Triggers>
                <asp:PostBackTrigger ControlID="LB20" />
                <asp:PostBackTrigger ControlID="LB21" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
</asp:Content>
