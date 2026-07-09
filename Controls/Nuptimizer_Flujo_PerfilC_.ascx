<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="Nuptimizer_Flujo_PerfilC_.ascx.vb" Inherits="NukaxanWEB.Nuptimizer_Flujo_PerfilC_" %>

<div class="modal fade"
     id="mdlPerfilAcciones"
     tabindex="-1"
     role="dialog">

    <div class="modal-dialog modal-xl">
        <div class="modal-content">

            <div class="modal-header">
                <h4 class="modal-title">
                    Acciones de Fórmulas
                </h4>
            </div>

            <div class="modal-body">

                <asp:Repeater ID="rptAcciones"
                    runat="server">

                    <HeaderTemplate>

                        <table class="table table-condensed table-bordered">
                            <thead>

                                <tr>

                                    <th>ETAPA</th>
                                    <th>NOMBRE</th>
                                    <th>ACCIÓN</th>
                                    <th>COMENTARIO</th>

                                </tr>

                            </thead>
                            <tbody>

                    </HeaderTemplate>

                    <ItemTemplate>

                        <tr>

                            <td>
                                <%# Eval("NomEtapaFlujo") %>
                            </td>

                            <td>
                                <%# Eval("NomEtapa") %>
                            </td>

                            <td>

                                <asp:DropDownList
                                    ID="DDLAccion"
                                    runat="server"
                                    CssClass="form-control">

                                    <asp:ListItem Text="NUEVA"
                                        Value="N" />

                                    <asp:ListItem Text="ACTUALIZAR"
                                        Value="A" />

                                    <asp:ListItem Text="ELIMINAR"
                                        Value="E" />

                                </asp:DropDownList>

                            </td>

                            <td>

                                <asp:TextBox
                                    ID="TBComentario"
                                    runat="server"
                                    CssClass="form-control">
                                </asp:TextBox>

                            </td>

                        </tr>

                    </ItemTemplate>

                    <FooterTemplate>

                            </tbody>
                        </table>

                    </FooterTemplate>

                </asp:Repeater>

            </div>

            <div class="modal-footer">

                <asp:Button
                    ID="BTNGuardar"
                    runat="server"
                    Text="Guardar"
                    CssClass="btn btn-primary" />

                <button
                    type="button"
                    class="btn btn-default"
                    data-dismiss="modal">

                    Cancelar

                </button>

            </div>

        </div>
    </div>
