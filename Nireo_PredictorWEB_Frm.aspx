<%@ Page Title="" Language="VB" Async="true" MasterPageFile="~/Master_Nireo.Master" AutoEventWireup="true" CodeBehind="Nireo_PredictorWEB_Frm.aspx.vb" Inherits="NukaxanWEB.Nireo_PredictorWEB_Frm" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagPrefix="asp" Namespace="Saplin.Controls" Assembly="DropDownCheckBoxes" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdateProgress ID="ProgressProcessing" DisplayAfter="2" runat="server" AssociatedUpdatePanelID="PanelPredictorInput">
        <ProgressTemplate>
            <div class="divWaiting" style="background-color: white;">
                <div style="margin-top: 300px;">
                    <%-- TODO: All labels should be replaced by a TOKEN to be replaced by the corresponding text for IA language engine (Kastiyah) --%>
                    <asp:Label ID="LabelProcessing" runat="server" Font-Bold="true" Font-Size="25px" ForeColor="#003F7C" Text=" Procesando... "></asp:Label>
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <div class="container-fluid w-100 h-100">
        <asp:UpdatePanel runat="server" ID="PanelPredictorInput" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="container-fluid w-100 h-100">
                    <div class="page_title_div">
                        <asp:Label ID="LabelPageTitle" runat="server" Text=" ForecastEngine " CssClass="page_title_label"></asp:Label>
                    </div>
                </div>
                <div class="panel-group" id="panelInput">
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            <h4 class="panel-title" data-toggle="collapse" data-target="#collapseOne">Datos a predecir:</h4>
                        </div>
                        <div id="collapseOne" class="panel-collapse collapse in">
                            <div class="panel-body">
                                <div class="row align-items-center mb-3">
                                    <div class="col-12 col-md-2">
                                        <asp:Label runat="server" ID="LabelSpectreFiles" CssClass="semibold">Archivos de espectro:</asp:Label>
                                    </div>
                                    <div class="col-12 col-md-4 mb-2 mb-md-0">                                        
                                        <asp:FileUpload ID="FileUploadSpectre" runat="server" CssClass="form-control" 
                                            Multiple="true" Accept=".txt" 
                                            onchange="updateFileNames(this.id, 'spectreFileNames');" />
                                    </div>
                                </div>
                                <div class="row align-items-center mb-3">
                                    <div class="col-12 col-md-2">
                                        <asp:Label runat="server" ID="LabelModelFiles" CssClass="semibold">Archivos de modelo:</asp:Label>
                                    </div>
                                    <div class="col-12 col-md-4 mb-2 mb-md-0">
                                        <asp:FileUpload ID="FileUploadModel" runat="server" CssClass="form-control" 
                                            Multiple="true" Accept=".unsb"
                                            onchange="updateFileNames(this.id, 'modelFileNames');" />
                                    </div>
                                </div>
                                <div class="row align-items-center mb-3">
                                    <div class="col-12 col-md-2">
                                        <asp:Label runat="server" ID="LabelBias" CssClass="semibold">BIAS:             </asp:Label>
                                    </div>
                                    <div class="col-12 col-md-4">
                                        <asp:TextBox runat="server" ID="TextBoxBias" CssClass="form-control" Text="0.0"
                                            pattern="^-?(100(\.0{1,6})?|[1-9]?\d(\.\d{1,6})?)$"
                                            oninput="this.value = this.value.replace(/[^0-9.-]/g, '').replace(/(\..*?)(\..*)/g, '$1')"
                                            title="Debe ser un número entre -100 y 100, con hasta 6 decimales." />
                                    </div>
                                </div>
                                <div style="height: 20px;"></div>
                                <div class="row align-items-center mb-3">
                                    <div class="col-12 col-md-2">
                                        <asp:Label runat="server" ID="LabelSpectreFileNames" CssClass="semibold">Espectros:</asp:Label>
                                    </div>
                                    <div class="col-12 col-md-10">
                                        <span id="spectreFileNames" class="file-names border p-2 d-block"></span>
                                    </div>
                                </div>
                                <div style="height: 20px;"></div>
                                <div class="row align-items-center mb-3">
                                    <div class="col-12 col-md-2">
                                        <asp:Label runat="server" ID="LabelModelFileNames" CssClass="semibold">Modelos:</asp:Label>
                                    </div>
                                    <div class="col-12 col-md-10">
                                        <span id="modelFileNames" class="file-names border p-2 d-block"></span>
                                    </div>
                                </div>
                                <div style="height: 50px;"></div>
                                <div class="row2 align-items-center d-flex margin-top-20">
                                    <div class="col-md-1 "></div>
                                    <div class="col-md-4 align-items-center d-flex"></div>
                                    <div class="col-md-7 align-items-center">
                                        <asp:Button runat="server" ID="ButtonPredict" CssClass="btn btn-primary" OnClientClick="return validateInputs();" OnClick="ButtonPredictClicked" Text=" Predecir... " />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="bar-actions" style="display: inline-block;">
                    <div align="left">
                        <asp:LinkButton ID="LinkButtonExport" runat="server" OnClick="LinkButtonExportClicked" CssClass="lnk-action" OnClientClick="return validateGridViewPredictions();">
                            <asp:Image ID="ImageExcel" runat="server" ImageAlign="AbsMiddle" ImageUrl="Content/Image/icon_file_excel.png" />
                            <asp:Label ID="LabelExport" runat="server"></asp:Label>
                        </asp:LinkButton>
                    </div>
                </div>
                <div style="overflow-y: scroll; overflow-x: scroll; height: 57vh; width: 100%; white-space:nowrap">
                    <asp:GridView ID="GridViewPredictions" runat="server" AutoGenerateColumns="true" ShowHeader="true"
                        Width="100%" ShowFooter="false" AllowPaging="false" CellSpacing="0"
                        Style="table-layout: auto; min-width: 100%; white-space: nowrap;" CssClass="datagrid">
                        <PagerSettings Visible="true" />
                    </asp:GridView>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="ButtonPredict" />
                <asp:PostBackTrigger ControlID="LinkButtonExport" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
    <script>
        function showProgress() {
            document.getElementById('<%= ProgressProcessing.ClientID %>').style.display = 'block';
        }
        function hideProgress() {
            document.getElementById('<%= ProgressProcessing.ClientID %>').style.display = 'none';
        }
        function validateInputs() {
            showProgress();
            const spectreFiles = document.getElementById('<%= FileUploadSpectre.ClientID %>').files;
            const modelFiles = document.getElementById('<%= FileUploadModel.ClientID %>').files;
            if (spectreFiles.length === 0) {
                alert('Debe seleccionar al menos un archivo de espectro.');
                hideProgress();
                return false;
            } else if (Array.from(spectreFiles).some(file => file.name.length > 120)) {
                alert('El nombre del archivo debe de ser de menos de 120 caracteres'); 
                hideProgress();
                return false;
            }
            if (modelFiles.length === 0) {
                alert('Debe seleccionar al menos un archivo de modelo.');
                hideProgress();
                return false;
            } else if (Array.from(modelFiles).some(file => file.name.length > 120)) {
                alert('El nombre del archivo debe de ser de menos de 120 caracteres');
                hideProgress();
                return false;
            }
            return true;
        }
        function updateFileNames(fileId, spanId) {
            const files = document.getElementById(fileId).files;
            const names = [];
            for (const file of files) {
                names.push(file.name);
            }
            document.getElementById(spanId).innerText = names.join(', ');
        }
        function validateGridViewPredictions() {
            const grid = document.getElementById('<%= GridViewPredictions.ClientID %>');
            if (!grid || !grid.rows || grid.rows.length <= 1) {
                alert('No hay datos para exportar.');
                return false;
            }
            return true;
        }
    </script>
</asp:Content>