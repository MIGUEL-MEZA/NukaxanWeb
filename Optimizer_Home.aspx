<%@ Page Title="" Language="VB" MasterPageFile="~/Master_Optimizer.Master" AutoEventWireup="true" CodeBehind="Optimizer_Home.aspx.vb" Inherits="NukaxanWEB.Optimizer_Home" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container-fluid w-100 h-100 align-content-center" style="border: solid 0px red;" align="center">
        <asp:DataList ID="DTLPlataformas" runat="server" RepeatColumns="3" CellSpacing="3" RepeatDirection="Horizontal"
            RepeatLayout="Table">
            <HeaderTemplate>
            </HeaderTemplate>
            <ItemTemplate>
                <div style="padding: 10px;">
                    <asp:HyperLink ID="HLP" runat="server" NavigateUrl='<%# Eval("Url").ToString%>' CssClass='<%# If(Eval("Habilitada") = 1, "icono-plataforma", "icono-plataforma-disable")%>' >
                        <div class="container-fluid" style="padding: 20px; border: solid 0px blue; min-width: 200px;">
                            <div runat="server" id="cardP" class="card" >
                                <asp:Label runat="server" ID="Habilitada" Text='<%# Eval("Habilitada")%>' Visible="false"></asp:Label>
                                <div align="center">
                                    <asp:Image ID="ImgP" runat="server" CssClass="img-responsive" Height="54px" ImageUrl='<%# "./Content/Image/" + Eval("ImgPlataforma")%>'  />
                                </div>
                                <div class="card-title" align="center">
                                    <br />
                                    <span><%#Eval("NomPlataforma")%></span>
                                </div>
                            </div>
                        </div>
                    </asp:HyperLink>
                </div>
            </ItemTemplate>
            <FooterTemplate>
            </FooterTemplate>
        </asp:DataList>
    </div>
</asp:Content>
