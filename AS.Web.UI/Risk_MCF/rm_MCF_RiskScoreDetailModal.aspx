<%@ Page Title="RISK SCORE DETAILS" Language="C#" MasterPageFile="~/MasterPagePopup.master"
    AutoEventWireup="true" CodeFile="rm_MCF_RiskScoreDetailModal.aspx.cs" Inherits="rm_MCF_RiskScoreDetailModal" meta:resourcekey="PageResource1" %>

<%@ Register Src="~/UserControls/PageTitle.ascx" TagName="PageTitle" TagPrefix="uc" %>
<%@ Register TagName="UxExport" Src="~/UserControls/UxExport.ascx" TagPrefix="uc" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="Server">

    <as:RadAjaxManagerProxy runat="server" ID="uxRadAjaxManager">
        <AjaxSettings>
            <tek:AjaxSetting AjaxControlID="uxParameterList">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="uxParameterList" />
                </UpdatedControls>
            </tek:AjaxSetting>
            <tek:AjaxSetting AjaxControlID="uxAttributeGrid">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="uxAttributeGrid" />
                </UpdatedControls>
            </tek:AjaxSetting>
        </AjaxSettings>
    </as:RadAjaxManagerProxy>

    <as:ASModalContainer ID="uxModalContainer" runat="server" WidthCssClass="modal-xxl" ContainerCssClass="container" Width="">
        <as:RadCodeBlock runat="server">
            <h2 class="grid-title on-top">
                <as:Literal ID="ltrMerchantInfor" runat="server" meta:resourcekey="ltrMerchantInforResource1"></as:Literal>
                <span class="text-muted">
                    <asp:Label ID="uxReportDate" runat="server" /></span>
            </h2>
            <div id="pnlAttributeScore" runat="server" class="row">

                <div class="col-md-12 RadGrid RadGrid_Default">
                    <table class="rgMasterTable rgClipCells  w-100">
                        <colgroup>
                            <col style="width: 90px" />
                            <col style="width: 35%" />
                            <col style="width: 65px" />
                            <col style="width: 25%" />
                            <col style="width: 100px" />
                            <col style="width: 100px" />
                        </colgroup>
                        <tr>
                            <th class="rgHeader"></th>
                            <th class="rgHeader"></th>
                            <th class="rgHeader"></th>
                            <th class="rgHeader"></th>
                            <th class="rgHeader"></th>
                            <th class="rgHeader"></th>
                        </tr>

                        <tr class="rgRow">
                            <td class="text-default-gray font-12 font-weight-bold">
                                <div class="treeview text-dark-gray font-weight-bold">
                                    <as:Literal ID="Literal3" runat="server" Text="Classification" meta:resourcekey="ltrClassificationResource"></as:Literal>
                                </div>
                            </td>
                            <td class="text-dark-gray treeview text-left">
                                <%= BindValue("MerchantClassificationName")%>
                            </td>
                            <td class="text-default-gray font-12 font-weight-bold">
                                <div class="treeview text-dark-gray font-weight-bold">
                                    <as:Literal ID="Literal5" runat="server" Text="Multiplier" meta:resourcekey="ltrMultiplierResource"></as:Literal>
                                </div>
                            </td>
                            <td class="text-dark-gray treeview text-left">
                                <%= BindValue("Multiplier")%>
                            </td>
                            <td class="text-default-gray font-12 font-weight-bold">
                                <div class="treeview text-dark-gray font-weight-bold">
                                    <as:Literal ID="Literal7" runat="server" Text="Total Risk Score:" meta:resourcekey="ltrTotalRiskScoreResource"></as:Literal>
                                </div>
                            </td>
                            <td class="text-dark-gray treeview text-left">
                                <%= BindValueDecimal("TotalRS")  %>
                            </td>
                        </tr>

                    </table>
                </div>

            </div>
        </as:RadCodeBlock>
        <!--  -->
        <h2 class="grid-title inline-block" data-toggle="collapse" data-target="#ciParameterRiskScore">
            <asp:Literal runat="server" ID="Literal2" Text="Parameter Risk Score" meta:resourcekey="ResourceParamerterRiskScore" />
        </h2>
        <div class="in" id="ciParameterRiskScore">
            <uc:UxExport ID="uxExportTop" IsOnTop="true" runat="server" GridID="uxParameterList" ShowPDF="false" />
            <as:ASGrid ID="uxParameterList" runat="server" AutoGenerateColumns="false" Width="100%" ShowReportTotal="true" IsCacheTemplateFile="false"
                AllowPaging="true" AllowSorting="true" CssClass="in" meta:resourcekey="uxParameterListResource1" IsAutoExportTemplate="true">
                <MasterTableView>
                    <Columns>
                        <as:ASGridBoundColumn HeaderText="Group" DataField="ParameterGroupDescription" UniqueName="ParameterGroupDescription"
                            HeaderTooltip="Group" ASFormat="DynamicString" SortExpression="ParameterGroupDescription" meta:resourcekey="ASGridBoundColumnResourceGroup">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text=""></ModelErrorMessage>
                            </ColumnValidationSettings>

                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        </as:ASGridBoundColumn>

                        <as:ASGridBoundColumn HeaderText="Parameter" DataField="ParameterName" UniqueName="ParameterName"
                            HeaderTooltip="Risk Parameter" ASFormat="DynamicString" SortExpression="ParameterName" meta:resourcekey="ASGridBoundColumnResource1">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text=""></ModelErrorMessage>
                            </ColumnValidationSettings>

                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        </as:ASGridBoundColumn>
                        <as:ASGridBoundColumn HeaderText="Parameter Description" DataField="ParameterDescription" UniqueName="ParameterDescription"
                            HeaderTooltip="Parameter Description" ASFormat="DynamicString" Display="false"  meta:resourcekey="ASGridBoundColumnResource8">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text=""></ModelErrorMessage>
                            </ColumnValidationSettings>

                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        </as:ASGridBoundColumn>
                        <as:ASGridBoundColumn HeaderText="From % / # / $" DataField="ParameterIndicatorFrom" HeaderStyle-Width="100px"
                            SortExpression="ParameterIndicatorFrom" UniqueName="ParameterIndicatorFrom" HeaderStyle-HorizontalAlign="Center"
                            HeaderTooltip="Minimum Range" ASFormat="StaticString" ItemStyle-HorizontalAlign="Right" meta:resourcekey="ASGridBoundColumnResource2">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text=""></ModelErrorMessage>
                            </ColumnValidationSettings>

                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                        </as:ASGridBoundColumn>
                        <as:ASGridBoundColumn HeaderText="To % / # / $" DataField="ParameterIndicatorTo" HeaderStyle-Width="100px"
                            SortExpression="ParameterIndicatorTo" UniqueName="ParameterIndicatorTo" HeaderStyle-HorizontalAlign="Center"
                            HeaderTooltip="Maximum Range" ItemStyle-HorizontalAlign="Right" meta:resourcekey="ASGridBoundColumnResource3">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text=""></ModelErrorMessage>
                            </ColumnValidationSettings>

                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                        </as:ASGridBoundColumn>
                        <as:ASGridBoundColumn Visible="false" UniqueName="ParameterDataType" HeaderText="Type"
                            DataField="ParameterDataType" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right" meta:resourcekey="ASGridBoundColumnResource4">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text=""></ModelErrorMessage>
                            </ColumnValidationSettings>

                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                        </as:ASGridBoundColumn>
                        <as:ASGridBoundColumn HeaderText="Threshold" DataField="ParameterThreshold" HeaderStyle-Width="100px" SortExpression="ParameterThreshold"
                            HeaderStyle-HorizontalAlign="Center" UniqueName="ParameterThreshold" HeaderTooltip="Minimum Dollar"
                            ItemStyle-HorizontalAlign="Right" meta:resourcekey="ASGridBoundColumnResource5">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text=""></ModelErrorMessage>
                            </ColumnValidationSettings>

                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                        </as:ASGridBoundColumn>
                        <as:ASGridBoundColumn UniqueName="ParameterPrecision" DataField="ParameterPrecision"
                            Visible="false" meta:resourcekey="ASGridBoundColumnResource6">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text=""></ModelErrorMessage>
                            </ColumnValidationSettings>

                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        </as:ASGridBoundColumn>
                        <as:ASGridBoundColumn HeaderText="Risk Score" DataField="ParameterScore" HeaderStyle-Width="100px" ASIsTotalColumn="true" SortExpression="ParameterScore"
                            UniqueName="ParameterScore" HeaderStyle-HorizontalAlign="Center" HeaderTooltip="Risk Score"
                            ASFormat="Integer" ItemStyle-HorizontalAlign="Right" meta:resourcekey="ASGridBoundColumnResource7">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text=""></ModelErrorMessage>
                            </ColumnValidationSettings>

                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                            <FooterStyle HorizontalAlign="Center" Font-Bold="true" Font-Italic="true" />
                        </as:ASGridBoundColumn>
                    </Columns>
                </MasterTableView>
            </as:ASGrid>

        </div>

        <asp:PlaceHolder ID="pnlAttributeRiskScoreGrid" runat="server">
            <!--  -->
            <h2 class="grid-title" data-toggle="collapse" data-target="#ciAttributeRiskScore">
                <asp:Literal runat="server" ID="Literal1" Text="Attribute Risk Score" meta:resourcekey="ResourceAttributeRiskScore" />
            </h2>

            <div id="ciAttributeRiskScore" class="in">
                <uc:UxExport ID="uxExportAttributeGrid" runat="server" GridHeader="Attribute Risk Score"
                    meta:resourcekey="uxExportAttributeGridResource"
                    GridID="uxAttributeGrid" OnNeedExportConfig="uxExportAttributeGrid_NeedExportConfig" IsOnTop="true" ShowPDF="false" />
                <as:ASGrid ID="uxAttributeGrid" runat="server" AllowPaging="True" GridLines="None" ShowReportTotal="true" OnNeedDataSource="uxAttributeGrid_NeedDataSource"
                    AllowSorting="True" AutoGenerateColumns="False" CssClass="in" meta:resourcekey="uxParameterListResource1" IsAutoExportTemplate="true">
                    <MasterTableView DataKeyNames="RecordID">
                        <Columns>
                            <as:ASGridBoundColumn UniqueName="AttributeName" HeaderText="Attribute Name" DataField="AttributeName"
                                ASFormat="DynamicString" HeaderTooltip="Attribute Name" meta:resourcekey="ASGridBoundColumnResourceAttributeName">
                                <ColumnValidationSettings>
                                </ColumnValidationSettings>
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            </as:ASGridBoundColumn>
                            <as:ASGridBoundColumn UniqueName="RangeName" HeaderText="Range Name" DataField="RangeName"
                                ASFormat="DynamicString" HeaderTooltip="Range Name" meta:resourcekey="ASGridBoundColumnResourceRangeName">
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            </as:ASGridBoundColumn>

                            <as:ASGridBoundColumn UniqueName="FromValue" HeaderText="From" HeaderStyle-Width="100px"
                                DataField="FromValue" ItemStyle-HorizontalAlign="Right" ASFormat="Integer"
                                HeaderTooltip="From" meta:resourcekey="ASGridBoundColumnResourceFrom">
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                            </as:ASGridBoundColumn>
                            <as:ASGridBoundColumn UniqueName="ToValue" HeaderText="To" HeaderStyle-Width="100px"
                                DataField="ToValue" ItemStyle-HorizontalAlign="Right" ASFormat="Integer"
                                HeaderTooltip="To" meta:resourcekey="ASGridBoundColumnResourceTo">
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                            </as:ASGridBoundColumn>

                            <as:ASGridBoundColumn UniqueName="Operand" HeaderText="Operand" HeaderStyle-Width="80px" ASFormat="DynamicString"
                                DataField="Operand" ItemStyle-HorizontalAlign="Right"
                                HeaderTooltip="Operand" meta:resourcekey="ASGridBoundColumnResourceOperand">
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                            </as:ASGridBoundColumn>
                            <as:ASGridBoundColumn UniqueName="MetricValue" HeaderText="Metric" ASFormat="DynamicString" ItemStyle-CssClass="word-wrapped"
                                DataField="MetricValue" meta:resourcekey="ASGridBoundColumnResourceMetric">
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            </as:ASGridBoundColumn>
                            <as:ASGridBoundColumn UniqueName="Score" HeaderText="Score" ASIsTotalColumn="true" DataField="Score" HeaderStyle-Width="100px"
                                ASFormat="Integer" HeaderTooltip="Score" meta:resourcekey="ASGridBoundColumnResourceScore">
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            </as:ASGridBoundColumn>
                        </Columns>
                    </MasterTableView>
                </as:ASGrid>
            </div>
        </asp:PlaceHolder>
        <div class="height-12"></div>
        <div class="text-right">
            <asp:Button runat="server" CssClass="btn btn-default" Text="Close" meta:resourcekey="btncloseResource" OnClientClick="return parent.HidePopupModal();" />
        </div>
    </as:ASModalContainer>
    <as:RadCodeBlock ID="JavaScript" runat="server">
        <script type="text/javascript">
            AdjustWindowSize(1120);
        </script>
    </as:RadCodeBlock>

</asp:Content>
