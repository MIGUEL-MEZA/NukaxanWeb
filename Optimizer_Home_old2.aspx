<%@ Page Title="" Language="VB" MasterPageFile="~/Master_Optimizer.Master" AutoEventWireup="true" CodeBehind="Optimizer_Home_old2.aspx.vb" Inherits="NukaxanWEB.Optimizer_Home_old2" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Label runat="server" ID="filtroview" Visible="false"></asp:Label>
    <asp:Label runat="server" ID="gvindexpage" Visible="false"></asp:Label>
    <asp:Label runat="server" ID="regPId" Visible="false"></asp:Label>
    <div class="container-fluid w-100 h-100 align-content-center" style="border: solid 0px red;" align="center">
        <table width="100%" runat="server" class="table_noborder" border="0" style="border: 0px;" clientidmode="Static">
            <tr>
                <td colspan="2" align="left">
                    <div style="padding: 3px 0px;">
                        <asp:Label runat="server" ID="TBNombreTitulo" CssClass="cliente_titulo"></asp:Label>
                    </div>
                </td>
            </tr>
            <tr>
                <td width="250px" valign="top">
                    <asp:Repeater ID="rptPlataformas" runat="server">
                        <ItemTemplate>
                            <div style="margin-bottom:25px;" class="menu_formulario">
                                <asp:Label runat="server" ID="CvePlataforma" Text='<%# Eval("CvePlataforma")%>' Visible="false"></asp:Label>
                                <asp:Label runat="server" ID="CvePlataformaP" Text='<%# Eval("CvePlataformaP")%>' Visible="false"></asp:Label>
                                <asp:Label runat="server" ID="Url" Text='<%# Eval("Url")%>' Visible="false"></asp:Label>
                                <asp:Label runat="server" ID="Habilitada" Text='<%# Eval("Habilitada")%>' Visible="false"></asp:Label>
                                <asp:Image ID="ImgP" runat="server" CssClass="menu_form_icono" ImageUrl='<%# "./Content/Image/" + Eval("ImgPlataforma")%>' />
                                <asp:Label runat="server" ID="NomPlataforma"  Text='<%# Eval("NomPlataforma")%>'></asp:Label>                                
                                <div class="menu_section33" style="border-bottom:solid 1px #b6bfc7;padding:0px 0px 15px 0px;">
                                    <%-- 
                                <asp:HyperLink ID="HLP" runat="server" NavigateUrl='<%# Eval("Url").ToString%>' CssClass='<%# If(Eval("Habilitada") = 1, "icono-plataforma", "icono-plataforma-disable")%>'>
                                    
                                </asp:HyperLink>--%>
                                    <asp:Repeater ID="rptMenu" runat="server">
                                        <ItemTemplate>
                                             <asp:Label runat="server" ID="Url" Text='<%# Eval("Url")%>' Visible="false"></asp:Label>
                                            <div runat="server" >
                                                <%--<asp:Label runat="server" ID="Habilitada" Text='<%# Eval("Habilitada")%>' Visible="false"></asp:Label>--%>
                                                <asp:HyperLink ID="HLM" runat="server" NavigateUrl='<%# Eval("Url").ToString%>'  CssClass="menu_formulario_opciones">
                                                    <span class='<%# Eval("Icono") + " navbar-icon"%>'></span>
                                                    <asp:Label runat="server" ID="NomMenu" Text='<%# Eval("NomMenu")%>'></asp:Label>
                                                </asp:HyperLink>
                                            </div>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </div>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </td>
                <td valign="top">
                    <%--<div style="padding: 5px 0px;">
                        <asp:Label runat="server" ID="Menu_Titulo" CssClass="menu_titulo"></asp:Label>
                        <hr class="hr_section" />
                    </div>
                    <div class="divsec-captura">
                        <asp:Label runat="server" ID="SECTitulo1" CssClass="control-label"></asp:Label>
                    </div>--%>
                </td>
            </tr>
        </table>
        <br />
        <br />
    </div>
</asp:Content>
