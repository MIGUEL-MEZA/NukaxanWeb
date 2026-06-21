<%@ Page Title="" Language="VB" MasterPageFile="~/Master_ProdAves.Master" AutoEventWireup="true" CodeBehind="ProdAves_Home.aspx.vb" Inherits="NukaxanWEB.ProdAves_Home" %>

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
    <asp:UpdatePanel runat="server" ID="UPContenido" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="sec-form-title">
                <asp:Label runat="server" ID="LBLTitulo" CssClass="form-title"></asp:Label>
            </div>
            <div class="row2 align-items-center d-flex">
                <div class="col-md-12" align="center">
                    <asp:Label runat="server" ID="LBLCliente" CssClass="semibold" Style="font-size: 15px; margin-right: 10px;"></asp:Label>
                    <asp:DropDownList ID="DDLCliente" runat="server" AutoPostBack="true" Width="500px" ClientIDMode="Static">
                    </asp:DropDownList>
                    <asp:Label runat="server" ID="TBNomClienteD" CssClass="label-cliente"></asp:Label>
                </div>
            </div>
            <br />
            <div class="container-fluid w-100 h-100 align-content-center" style="border: solid 0px red;" align="center">
                <asp:DataList ID="DTLParvadas" runat="server" RepeatColumns="3" CellPadding="10" CellSpacing="3" RepeatDirection="Horizontal"
                    RepeatLayout="Table" ItemStyle-VerticalAlign="Top">
                    <HeaderTemplate>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label runat="server" ID="CodCliente" Text='<%#Eval("CodCliente")%>' Visible="false"></asp:Label>
                        <asp:Label runat="server" ID="CveParvada" Text='<%#Eval("CveParvada")%>' Visible="false"></asp:Label>
                        <div class="container-fluid" style="padding: 10px;">
                            <div class="form-panel" style="width: 380px;">
                                <div class="form-panel-header-blue">
                                    <div class="row d-flex">
                                        <div class="col-md-8" align="left">
                                            <asp:Label runat="server" ID="LBLTitulo" CssClass="form-panel-title" Text='<%#Eval("CodParvada")%>'></asp:Label>
                                        </div>
                                        <div class="col-md-4" align="right">
                                            <span class="form-panel-tag"><%#Eval("Edad").ToString + " Semanas"%></span>
                                        </div>
                                    </div>
                                </div>
                                <div class="form-pane-body" align="center">
                                    <div class="row d-flex" style="padding: 10px 0px;">
                                        <div class="col-md-4" align="center">
                                            <asp:Label runat="server" ID="Label3" CssClass="semibold" Text='<%#Eval("NomLineaG")%>'></asp:Label>
                                        </div>
                                        <div class="col-md-4" align="center">
                                            <asp:Label runat="server" ID="Label2" CssClass="semibold" Text='<%#CInt(Eval("AvesActuales")).ToString("N0") + " Aves"%>'></asp:Label>
                                        </div>
                                        <div class="col-md-4" align="center">
                                            <asp:Label runat="server" ID="Label4" CssClass="semibold" Text='<%#Eval("NomEtapa")%>'></asp:Label>
                                        </div>
                                    </div>
                                    <asp:DataList ID="DTLCasetas" runat="server" RepeatColumns="2" CellPadding="5" CellSpacing="3" RepeatDirection="Horizontal"
                                        RepeatLayout="Table" ItemStyle-VerticalAlign="Top">
                                        <HeaderTemplate>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <div class="container-fluid" style="padding: 5px;">
                                                <div class="form-panel" style="width: 170px;">
                                                    <div class="form-panel-header-green">
                                                        <div class="row d-flex">
                                                            <div class="col-md-8" align="left" style="padding: 2px;">
                                                                <asp:Label runat="server" ID="Label1" Font-Size="11px" CssClass="form-panel-title" Text='<%#Eval("CodCaseta")%>'></asp:Label>
                                                            </div>
                                                            <div class="col-md-4" align="right" style="padding: 2px;">
                                                                <span class="form-panel-tag" style="font-size: 10px;"><%#Eval("Edad").ToString + " Sem"%></span>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="form-pane-body">
                                                        <div align="center" width="100%">
                                                            <asp:Label runat="server" ID="Label3" Text='<%#CInt(Eval("AvesActuales")).ToString("N0") + " Aves"%>'></asp:Label>
                                                        </div>
                                                        <div align="center" width="100%">
                                                            <asp:Label runat="server" ID="Label5" Text='<%#"Peso Actual: " + CDbl(Eval("PesoActual")).ToString("N2") + " gr"%>'></asp:Label>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                        </FooterTemplate>
                                    </asp:DataList>
                                </div>
                            </div>
                    </ItemTemplate>
                    <FooterTemplate>
                    </FooterTemplate>
                </asp:DataList>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
