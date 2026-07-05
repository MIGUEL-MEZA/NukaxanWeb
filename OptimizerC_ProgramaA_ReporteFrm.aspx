<%@ Page Title="" Language="VB" Async="true" MasterPageFile="~/Master_OptimizerC.Master" AutoEventWireup="true" CodeBehind="OptimizerC_ProgramaA_ReporteFrm.aspx.vb" Inherits="NukaxanWEB.OptimizerC_ProgramaA_ReporteFrm" %>

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
            inicializarTabsPrograma();

        });

        function inicializarTabsPrograma() {
            var tabName = $("#TabName");
            if (!tabName.length) {
                return;
            }

            if (!tabName.val()) {
                tabName.val('presupuesto');
            }

            $('#myTab a[data-toggle="tab"]').off('shown.bs.tab.programa').on('shown.bs.tab.programa', function (e) {
                var href = $(e.target).attr('href');
                tabName.val(href === '#sec3' ? 'comparativo' : 'presupuesto');
            });

            if (tabName.val() === 'comparativo') {
                $('#tab3').tab('show');
            } else {
                $('#tab2').tab('show');
            }
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
.table-rep tfoot th,.table-rep tfoot td {
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
                         <asp:LinkButton ID="LB20" runat="server" OnClick="DescargarExcel" CssClass="lnkbtn-action">
                            <i runat="server" id="LB_IMG20" class=""></i>
                            <asp:Label runat="server" ID="LB_LBL20">Excel</asp:Label>
                        </asp:LinkButton>
                        <asp:LinkButton ID="LB21" runat="server" OnClick="DescargarPdf" CssClass="lnkbtn-action">
                            <i runat="server" id="LB_IMG21" class=""></i>
                            <asp:Label runat="server" ID="LB_LBL21">PDF</asp:Label>
                        </asp:LinkButton>
                        <asp:LinkButton ID="LB2" runat="server" OnClick="Regresar"  CssClass="lnkbtn-action">
                            <i runat="server" id="LB_IMG2" class=""></i>
                            <asp:Label runat="server" ID="LB_LBL2">Salir</asp:Label>
                        </asp:LinkButton>
                    </div>
                </div>

                 <div class="header">                    
                    <div class="header-left">
                       <%-- <img src="logo-left.png" class="logo-left" />--%>
                        <img src="Content/Image/Icono-PerfilNutricional.svg" class="logo-left" />
                        <div class="header-text">
                            <h1>PROGRAMA DE ALIMENTACIÓN</h1>
                            <asp:Label runat="server" ID="LBLReferencia" CssClass="subtitulo"></asp:Label><br />
                            <asp:Label runat="server" ID="LBLCliente" CssClass="cliente"></asp:Label>                           
                        </div>
                    </div>                    
                    <div class="header-right">
                        <img src="Content/Image/logo-nuptimizer.svg" class="logo-right" />
                       <%-- <div class="fecha">
                            FECHA EMISIÓN: 18/06/2026
                        </div>--%>
                    </div>
                </div>
                <br />
                                <div id="Tabs" role="tabpanel">
                    <!-- Nav tabs -->
                    <ul class="nav nav-pills" id="myTab" role="tablist">
                        <li class="nav-item active" runat="server">
                            <a class="nav-link" id="tab2" href="#sec2" aria-controls="sec2" role="tab" data-toggle="tab">PRESUPUESTO OPTIMIZADO</a>
                        </li>
                        <li class="nav-item" runat="server">
                            <a class="nav-link" id="tab3" href="#sec3" aria-controls="sec3" role="tab" data-toggle="tab">COMPARATIVO</a>
                        </li>
                    </ul>
                    <!-- Tab panes -->
                    <div class="tab-content">                        <div role="tabpanel" class="tab-pane active" id="sec2" >
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
                    </div>                                                     </div>
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
