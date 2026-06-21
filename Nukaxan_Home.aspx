<%@ Page Title="" Language="VB" MasterPageFile="~/Master_Nukaxan.Master" AutoEventWireup="true" CodeBehind="Nukaxan_Home.aspx.vb" Inherits="NukaxanWEB.Nukaxan_Home" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container-fluid w-100 h-100">
        <div class="page_title_div">
            <label runat="server" class="page_title_label">PLATAFORMAS DE SERVICIO</label>           
        </div>          
            <ul class="cards">
                <asp:Repeater ID="rptPlataformas" runat="server">
                    <ItemTemplate>
                        <li class="card">
                            <div class="ms-auto w-100">
                                <div class="card-title">
                                    <img class="img-responsive" src='<%# "./Content/image/" + Eval("ImgPlataforma")%>' height="34px" />
                                </div>
                                <div class="card-content">
                                    <p><%#Eval("Descripcion")%></p>
                                </div>
                            </div>
                            <div class="card-link-wrapper">
                              <%--  <asp:Button ID="btnLogin" Text="Accesar" runat="server"  Class="card-link" />--%>
                                <a href='<%#Eval("Url")%>' class="card-link">Accesar</a>
                            </div>
                        </li>
                    </ItemTemplate>
                </asp:Repeater>         
            </ul>
        </div>
</asp:Content>
