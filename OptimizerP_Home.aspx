<%@ Page Title="" Language="VB" MasterPageFile="~/Master_OptimizerP.Master" AutoEventWireup="true" CodeBehind="OptimizerP_Home.aspx.vb" Inherits="NukaxanWEB.OptimizerP_Home" %>

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
            
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
