<%@ Page Title="" Language="VB" Async="true" AutoEventWireup="true" MasterPageFile="~/Master_OptimizerP.Master" CodeBehind="OptimizerP_PerfilN_Frm.aspx.vb" Inherits="NukaxanWEB.OptimizerP_PerfilN_Frm" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <script src="Scripts/WebForms/Message.js"></script>
    <script>       
        //$(function () {
        //    $('.nav-item [id*=tab1]').click(function (e) {
        //        e.preventDefault();
        //        $("#MainContent_LB4").hide();
        //        $("#MainContent_LB11").show();
        //    });
        //    $('.nav-item [id*=tab2]').click(function (e) {
        //        e.preventDefault();
        //        $("#MainContent_LB4").show();
        //        $("#MainContent_LB11").hide();
        //    });
        //});

        function onDataShown(sender, args) {
            sender._popupBehavior._element.style.zIndex = 1000001;
        }
        $(document).ready(function () {
            $('[id*=DDLCliente]').select2();
            //$("#MainContent_LB4").hide();
            //$("#MainContent_LB11").show();
        });
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        if (prm != null) {
            prm.add_endRequest(function (sender, e) {
                if (sender._postBackSettings.panelsToUpdate != null) {
                    $("[id*=DDLCliente]").select2({ dropdownAutoWidth: true });
                }
            });
        };
        function handleChange(textbox) {
            var $txt = $(textbox);
            var valor = $txt.val();

            // Buscar la fila actual (tr) y luego el otro campo dentro de esa fila
            var $fila = $txt.closest('tr');
            var etapa  = $fila.find('.hdn-etapa').val();
           /* alert(valor + '-' + etapa);*/
            var $items = $('.repeater-item');
           /* alert(parseInt(etapa) + 1)*/
            
            for (var i = parseInt(etapa) ; i < $items.length; i++) {                
                var $nextItem = $($items[i]);
                var chkActivo = $nextItem.find('input[type="checkbox"]');              
                if (chkActivo.prop('checked')) {                    
                    $nextItem.find('.Edad-Ini').val(valor);
                    break;
                }
            }            
        };
        /*AJUSTES*/
        function marcarCambios() {
            document.querySelectorAll(".ajuste").forEach(input => {
                if (input.offsetParent === null) return;
                let ref = parseFloat(input.dataset.referencia);
                let ajuste = parseFloat(input.value);
                let tdAjuste = input.closest("td");
                let tdcomentario = tdAjuste.nextElementSibling;
                                                
                let cambiado = Math.abs(ajuste - ref) > 0.000001;

                if (cambiado) {
                    tdAjuste.style.backgroundColor = "#fff3cd";
                    input.style.fontWeight = "bold";

                    tdcomentario.style.backgroundColor = "#ffeeba";


                } else {
                    tdAjuste.style.backgroundColor = "";
                    input.style.fontWeight = "";

                    tdcomentario.style.backgroundColor = "";
                }
            });
        }
        document.addEventListener("input", function (e) {
            if (e.target.classList.contains("ajuste")) {
                marcarCambios();
            }
        });
        window.onload = function () {
            marcarCambios();
        };
        function normalizarModelo(modelo) {
            modelo.forEach(x => {
                let dec = x.Decimales || 2;
                let ref = parseFloat(x.Referencia.toFixed(dec));
                let aju = parseFloat(x.Ajuste.toFixed(dec));
                // ✅ si son iguales después de redondeo → forzar igualdad
                if (aju === ref) {
                    x.Ajuste = ref;
                }
            });

            return modelo;
        }
        function actualizarModeloDesdeUI() {
            document.querySelectorAll(".ajuste").forEach(input => {
                if (input.offsetParent === null) return;
                let etapa = input.dataset.etapa;
                let variable = input.dataset.variable;

                let item = modeloCaptura.find(x =>
                    x.Etapa == etapa && x.Variable == variable
                );

                if (!item) return;

                let dec = item.Decimales || 2;
                let nuevoAjuste = parseFloat(input.value || 0);
                nuevoAjuste = parseFloat(nuevoAjuste.toFixed(dec));
                let referencia = parseFloat(item.Referencia.toFixed(dec));
                if (nuevoAjuste !== referencia) {
                    item.Ajuste = nuevoAjuste;
                    /*console.log("CAMBIO REAL:", item);*/
                } else {
                    item.Ajuste = referencia;
                }

            });

            return modeloCaptura;
        }
        document.addEventListener("input", function (e) {

            if (e.target.classList.contains("ajuste")) {

                // ✅ Marcar como editado
                e.target.dataset.editado = "1";
            }

            if (e.target.classList.contains("comentario")) {

                let etapa = e.target.dataset.etapa;
                let variable = e.target.dataset.variable;

                let item = modeloCaptura.find(x =>
                    x.Etapa == etapa && x.Variable == variable
                );

                if (item) {
                    item.Comentario = e.target.value.trim();
                }
            }

        });
        function validarModelo(modelo) {

            let errores = false;
            let tolerancia = 0.0001;

            // 🔄 Limpia bordes previos
            document.querySelectorAll(".comentario").forEach(input => {
                input.style.border = "";
            });

            modelo.forEach(item => {

                // ✅ 1. Ignorar los que no se muestran
                if (item.Mostrar === "N" || item.EditarAjuste === "N") return;

                // ✅ 2. Buscar inputs en UI
                let inputAjuste = document.querySelector(
                    `.ajuste[data-etapa='${item.Etapa}'][data-variable='${item.Variable}']`
                );

                let inputComentario = document.querySelector(
                    `.comentario[data-etapa='${item.Etapa}'][data-variable='${item.Variable}']`
                );

                // ✅ 3. Si no existe o está oculto → ignorar
                if (!inputAjuste || inputAjuste.offsetParent === null) return;

                let dec = item.Decimales || 2;

                let ajuste = parseFloat(Number(item.Ajuste).toFixed(dec));
                let referencia = parseFloat(Number(item.Referencia).toFixed(dec));
                let comentario = String(item.Comentario || "").trim();

                // ✅ 4. Detectar cambio real (con tolerancia)
                let cambioReal = Math.abs(ajuste - referencia) > tolerancia;

                // ✅ 5. Validar SOLO si el usuario lo tocó
                let fueEditado = inputAjuste.dataset.editado === "1";

                if (cambioReal && fueEditado && comentario.length === 0) {

                    errores = true;

                    console.log("ERROR VALIDACION:", item);

                    if (inputComentario) {
                        inputComentario.style.border = "2px solid red";
                    }
                }

            });

            return !errores;
        }
        function guardarCambios() {
            let modelo = actualizarModeloDesdeUI();
           modelo = normalizarModelo(modelo);
            if (!validarModelo(modelo)) {
                alert("Debes capturar comentario cuando el ajuste es diferente a la referencia.");
                return;
            }
           
            let id = document.getElementById("JSONId").value;          
            let usuario = document.getElementById("JSONUsuAct").value;
            $.ajax({
                         url: '/OptimizerPerfilServices.asmx/GuardarJSONAjuste',  
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        xhrFields: {
                            withCredentials: true   // ✅ ENVÍA COOKIES DE AUTH
                        },
                        data: JSON.stringify({
                            modelo: modelo,
                            modulo:'P',
                            id: parseInt(id),
                            usuario: usuario
                        }),

                        success: function (res) {
                            alert("Guardado correctamente");
                        },

                        error: function (err) {
                            console.log(err.responseText);
                            alert("Error");
                        }
                    });

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
        <asp:UpdatePanel runat="server" ID="UPContenido" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:Label runat="server" ID="filtroview" Visible="false"></asp:Label>
                <asp:Label runat="server" ID="gvindexpage" Visible="false"></asp:Label>
                <asp:Label runat="server" ID="regPId" Visible="false"></asp:Label>
                <asp:Label runat="server" ID="CodCliente" Visible="false"></asp:Label>
                 <asp:Label runat="server" ID="CodALLIX" Visible="false"></asp:Label>
                <asp:Label runat="server" ID="CveModalidad" Visible="false"></asp:Label>
                <asp:Label runat="server" ID="CveEstatus" Visible="false"></asp:Label>
                <asp:Label runat="server" ID="Autor" Visible="false"></asp:Label>
                <asp:Label runat="server" ID="CvePlan" Visible="false"></asp:Label>                
                <input type="hidden" id="JSONId" value="<%= regPId.Text %>" />
                <input type="hidden" id="JSONUsuAct" value="<%= ObjUser.CodUsuario %>" />
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
                        <asp:LinkButton ID="LB15" runat="server" OnClick="MostrarPrograma" CssClass="lnkbtn-action">
                            <i runat="server" id="LB_IMG15" class=""></i>
                            <asp:Label runat="server" ID="LB_LBL15">Ver Programa</asp:Label>
                        </asp:LinkButton>
                        <asp:LinkButton ID="LB18" runat="server" OnClick="Enviar" CssClass="lnkbtn-action">
                            <i runat="server" id="LB_IMG18" class=""></i>
                            <asp:Label runat="server" ID="LB_LBL18">Enviar Perfil</asp:Label>
                        </asp:LinkButton>
                        <asp:LinkButton ID="LB17" runat="server" OnClick="Guardar" CssClass="lnkbtn-action">
                            <i runat="server" id="LB_IMG17" class=""></i>
                            <asp:Label runat="server" ID="LB_LBL17">Editar</asp:Label>
                        </asp:LinkButton>
                        <asp:LinkButton ID="LB7" runat="server" OnClick="Guardar" CssClass="lnkbtn-action">
                            <i runat="server" id="LB_IMG7" class=""></i>
                            <asp:Label runat="server" ID="LB_LBL7">Cerrar</asp:Label>
                        </asp:LinkButton>
                        <asp:LinkButton ID="LB11" runat="server" OnClick="Guardar" CssClass="lnkbtn-action">
                            <i runat="server" id="LB_IMG11" class=""></i>
                            <asp:Label runat="server" ID="LB_LBL11">Calcular</asp:Label>
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
                        <li class="nav-item active" runat="server" id="li_tab1">
                            <a class="nav-link" id="tab1" href="#sec1" aria-controls="sec1" role="tab" data-toggle="tab">REQUERIMIENTOS</a>
                        </li>
                        <li class="nav-item" runat="server" id="li_tab2">
                            <a class="nav-link" id="tab2" href="#sec2" aria-controls="sec2" role="tab" data-toggle="tab">PERFIL</a>
                        </li>
                        <%-- <li class="nav-item" runat="server">
                            <a class="nav-link" id="tab3" href="#sec3" aria-controls="sec3" role="tab" data-toggle="tab"">BITACORA</a>
                        </li>--%>
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
                                    <td width="30%" class="bg-white"></td>
                                    <td width="20%" class="bg-gray semibold"></td>
                                    <td width="30%" class="bg-white"></td>
                                </tr>
                                 <tr>
                                    <td class="bg-gray semibold">
                                        <asp:Label runat="server" ID="LBLC1"></asp:Label>
                                    </td>
                                    <td class="bg-white" >                                        
                                        <asp:Label runat="server" ID="TBID" CssClass="control-label color-red"></asp:Label>
                                    </td>
                                    <td class="bg-gray semibold">
                                        <asp:Label runat="server" ID="LBLC2"></asp:Label>
                                    </td>
                                    <td class="bg-white">                                       
                                        <asp:Label runat="server" ID="TBNomEstatusD" CssClass="control-label "></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="bg-gray semibold">
                                        <asp:Label runat="server" ID="LBLC3"></asp:Label>
                                    </td>
                                    <td class="bg-white" colspan="3">
                                        <asp:TextBox ID="TBTitulo" runat="server" CssClass="form-control ucase" AutoCompleteType="Disabled"></asp:TextBox>
                                        <asp:Label runat="server" ID="TBTituloD" CssClass="control-label"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="bg-gray semibold">
                                        <asp:Label runat="server" ID="LBLC5"></asp:Label>
                                    </td>
                                    <td class="bg-white" colspan="3">
                                        <asp:DropDownList ID="DDLCliente" runat="server" AutoPostBack="true" Width="95%" ClientIDMode="Static">
                                        </asp:DropDownList>
                                        <asp:Label runat="server" ID="TBNomClienteD" CssClass="control-label "></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="bg-gray semibold">
                                        <asp:Label runat="server" ID="LBLC7"></asp:Label>
                                    </td>
                                    <td class="bg-white">
                                        <asp:DropDownList ID="DDLModalidad" runat="server" AutoPostBack="true" CssClass="form-control" Width="95%" ClientIDMode="Static">
                                        </asp:DropDownList>
                                        <asp:Label runat="server" ID="TBNomModalidadD" CssClass="control-label "></asp:Label>
                                    </td>
                                   
                                </tr>
                            </table>
                            <br />
                            <div class="divsec-captura">
                                <asp:Label runat="server" ID="SECTitulo2"  CssClass="control-label"></asp:Label>
                            </div>
                            <table class="table-condensed" border="1" style="border: solid 1px gray; width: 100%;">
                                <tr style="visibility: collapse;">
                                    <td width="20%" class="bg-gray semibold"></td>
                                    <td width="30%" class="bg-white"></td>
                                    <td width="20%" class="bg-gray semibold"></td>
                                    <td width="30%" class="bg-white"></td>
                                </tr>
                                <tr>
                                    <td class="bg-gray semibold">
                                        <asp:Label runat="server" ID="LBLC9"></asp:Label>
                                    </td>
                                    <td class="bg-white">
                                        <asp:DropDownList runat="server" ID="DDLReferencia" CssClass="form-control" Width="95%" ClientIDMode="Static">
                                        </asp:DropDownList>
                                        <asp:Label runat="server" ID="TBReferenciaD" CssClass="control-label"></asp:Label>

                                    </td>                               
                                    <td class="bg-gray semibold">
                                        <asp:Label runat="server" ID="LBLC11"></asp:Label>
                                    </td>
                                    <td class="bg-white">
                                        <asp:DropDownList runat="server" ID="DDLPreIniNupio" CssClass="form-control" Width="150px">
                                        </asp:DropDownList>
                                        <asp:Label runat="server" ID="TBPreIniNupioD" CssClass="control-label"></asp:Label>

                                    </td>
                                </tr>
                                <tr>
                                    <td class="bg-gray semibold">
                                        <asp:Label runat="server" ID="LBLC13"></asp:Label>
                                    </td>
                                    <td class="bg-white">
                                        <div style="display: flex;">
                                            <asp:TextBox ID="TBTemperatura" runat="server" CssClass="form-control tb-sm" MaxLength="5" AutoCompleteType="Disabled"></asp:TextBox>
                                            <asp:Label runat="server" ID="TBTemperaturaD" CssClass="control-label"></asp:Label>
                                            <span runat="server" id="TBTemperaturaU" class="tb-unidad">�C</span>
                                            <asp:Label runat="server" ID="LBLH5444" CssClass="text-muted" Style="padding: 5px 7px;"></asp:Label>
                                            <asp:FilteredTextBoxExtender ID="FTE1" runat="server" FilterType="Custom,Numbers" TargetControlID="TBTemperatura" ValidChars="." />
                                        </div>
                                    </td>
                                    <td class="bg-gray semibold">
                                        <asp:Label runat="server" ID="LBLC19"></asp:Label>
                                    </td>
                                    <td class="bg-white">
                                        <asp:Label runat="server" ID="TBITHD" CssClass="control-label"></asp:Label>
                                        <asp:Image ID="ImgITH" runat="server" ImageUrl="./Content/Image/circle_gray.png" Style="margin-left: 10px; width: 20px; height: 20px;" />
                                        <asp:Label runat="server" ID="TBNomITHD" CssClass="control-label" Style="margin-left: 5px;"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="bg-gray semibold">
                                        <asp:Label runat="server" ID="LBLC15"></asp:Label></td>
                                    <td class="bg-white">
                                        <div style="display: flex;">
                                            <asp:TextBox ID="TBHumedad" runat="server" CssClass="form-control tb-sm" MaxLength="7" AutoCompleteType="Disabled"></asp:TextBox>
                                            <asp:Label runat="server" ID="TBHumedadD" CssClass="control-label"></asp:Label>
                                            <span runat="server" id="TBHumedadU" class="tb-unidad">%</span>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" FilterType="Custom,Numbers" TargetControlID="TBHumedad" ValidChars="." />
                                        </div>
                                    </td>
                                    <td class="bg-gray semibold">
                                        <asp:Label runat="server" ID="LBLC21"></asp:Label>
                                    </td>
                                    <td class="bg-white">
                                        <asp:Label runat="server" ID="TBProductividadD" CssClass="control-label"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="bg-gray semibold">
                                        <asp:Label runat="server" ID="LBLC17"></asp:Label></td>
                                    <td class="bg-white">
                                        <div style="display: flex;">
                                            <asp:TextBox ID="TBDesperdicio" runat="server" CssClass="form-control tb-sm" MaxLength="7" AutoCompleteType="Disabled"></asp:TextBox>
                                            <asp:Label runat="server" ID="TBDesperdicioD" CssClass="control-label"></asp:Label>
                                            <span runat="server" id="TBDesperdicioU" class="tb-unidad">%</span>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" FilterType="Custom,Numbers" TargetControlID="TBDesperdicio" ValidChars="." />
                                        </div>
                                    </td>
                                </tr>                               
                            </table>
                            <br />
                            <div class="divsec-captura">
                                <asp:Label runat="server" ID="SECTitulo3"  CssClass="control-label"></asp:Label>
                            </div>
                            <div align="left" style="display: block;">
                                <asp:Repeater ID="rptEtapas" runat="server">
                                    <HeaderTemplate>
                                        <table width='100%' cellpadding='3' border="1" class="table table-condensed  table-sm table-rep">
                                            <thead>
                                                <tr>
                                                    <th width='5%'>
                                                        <asp:Label runat="server" ID="Label7"></asp:Label>
                                                    </th>
                                                    <th width='22%'>
                                                        <asp:Label runat="server" ID="Label1">ETAPA</asp:Label>
                                                    </th>
                                                    <th width='22%'>
                                                        <asp:Label runat="server" ID="Label2">NONBRE CLIENTE</asp:Label>
                                                    </th>
                                                    <th width='17%' align="center">
                                                        <asp:Label runat="server" ID="Label3">EDAD INICIAL (d�as)</asp:Label>
                                                    </th>
                                                    <th width='17%' align="center">
                                                        <asp:Label runat="server" ID="Label4">EDAD FINAL (d�as)</asp:Label>
                                                    </th>
                                                    <th width='17%' align="center">
                                                        <asp:Label runat="server" ID="Label5">EM alimento (kcal/kg)</asp:Label>
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
                                                  <asp:Label runat="server" ID="CveEtapa" Text='<%# Eval("CveEtapa")%>' Visible="false"></asp:Label>
                                                <asp:Label runat="server" ID="Aplica" Text='<%# Eval("Aplica")%>' Visible="false"></asp:Label>
                                                <asp:Label runat="server" ID="Fija" Text='<%# Eval("Fija")%>' Visible="false"></asp:Label>
                                                <asp:Label runat="server" ID="CodALLIX" Text='<%# Eval("CodALLIX")%>' Visible="false"></asp:Label>

                                            </td>
                                            <td align="left " class="fw-bold">
                                               
                                                <asp:TextBox ID="TBNomEtapa" runat="server" Width="95%" Text='<%# Eval("NomEtapa")%>' CssClass="form-control "></asp:TextBox>
                                                <asp:Label runat="server" ID="TBNomEtapaD" Text='<%# Eval("NomEtapa")%>' CssClass="control-value bold" Visible="false"></asp:Label>
                                            </td>
                                            <td align="center" class="justify-content-center">
                                                <asp:TextBox ID="TBEdadIni" runat="server" Width="50px" Text='<%# Eval("EdadIni")%>' CssClass="form-control "></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" TargetControlID="TBEdadIni" runat="server" FilterType="Custom,Numbers" ValidChars="."></asp:FilteredTextBoxExtender>
                                                <asp:Label runat="server" ID="TBEdadIniD" Text='<%# Eval("EdadIni")%>' CssClass="control-value bold" Visible="false"></asp:Label>
                                            </td>
                                            <td align="center" class="justify-content-center">
                                                <asp:TextBox ID="TBEdadFin" runat="server" Width="50px" Text='<%# Eval("EdadFin")%>' CssClass="form-control "></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" TargetControlID="TBEdadFin" runat="server" FilterType="Custom,Numbers" ValidChars="."></asp:FilteredTextBoxExtender>
                                                <asp:Label runat="server" ID="TBEdadFinD" Text='<%# Eval("EdadFin")%>' CssClass="control-value bold" Visible="false"></asp:Label>
                                            </td>
                                            <td align="center" class="justify-content-center">
                                                <asp:TextBox ID="TBEMAlimento" runat="server" Width="60px" Text='<%# Eval("EMAlimento")%>' Visible="true" CssClass="form-control "></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" TargetControlID="TBEMAlimento" runat="server" FilterType="Custom,Numbers" ValidChars="."></asp:FilteredTextBoxExtender>
                                                <asp:Label runat="server" ID="TBEMAlimentoD" Text='<%# Eval("EMAlimento")%>' CssClass="control-value bold" Visible="false"></asp:Label>
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
                                <asp:Label runat="server" ID="SECTitulo4"  CssClass="control-label"></asp:Label>
                            </div>
                            <table class="table-condensed" border="1" style="border: solid 1px gray; width: 100%;">
                                <tr style="visibility: collapse;">
                                    <td width="20%" class="bg-gray semibold"></td>
                                    <td width="80%" class="bg-white"></td>                                    
                                </tr>
                                <tr>
                                    <td class="bg-gray semibold">
                                        <asp:Label runat="server" ID="LBLG22"></asp:Label>
                                    </td>
                                    <td class="bg-white" colspan="3">                                        
                                        <asp:Label runat="server" ID="TBFecAltaD" CssClass="control-label color-blue"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="bg-gray semibold">
                                        <asp:Label runat="server" ID="LBLG23"></asp:Label>
                                    </td>
                                    <td class="bg-white" colspan="3">                                        
                                        <asp:Label runat="server" ID="TBFecActD" CssClass="control-label color-blue"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div role="tabpanel" class="tab-pane" id="sec2">
                            <div align="right" style="padding:3px 20px 3px 3px;">
                                <asp:LinkButton ID="LB19" runat="server" OnClientClick="guardarCambios(); return false;" CssClass="lnkbtn-action">
                                    <i runat="server" id="LB_IMG19" class=""></i>
                                    <asp:Label runat="server" ID="LB_LBL19">Guardar Ajuste</asp:Label>
                                </asp:LinkButton>                                
                            </div>
                            <asp:Literal runat="server" ID="PerfilN"></asp:Literal>
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
