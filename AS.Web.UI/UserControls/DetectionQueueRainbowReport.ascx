<%@ Control Language="C#" AutoEventWireup="true" CodeFile="DetectionQueueRainbowReport.ascx.cs"
    Inherits="UserControls_DetectionQueueRainbowReport" %>

<%@ Register TagPrefix="uc" TagName="UxExport" Src="~/UserControls/UxDetectionQueueExport.ascx" %>

<as:RadAjaxManagerProxy ID="RadAjaxManager" runat="server">
    <AjaxSettings>
        <tek:AjaxSetting AjaxControlID="uxReportGrid">
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="uxReportGrid" />
            </UpdatedControls>
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="optGrid" UpdatePanelRenderMode="Inline" LoadingPanelID="uxInvisiblePanel" />
            </UpdatedControls>
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="optCard" UpdatePanelRenderMode="Inline" LoadingPanelID="uxInvisiblePanel" />
            </UpdatedControls>
        </tek:AjaxSetting>
        <tek:AjaxSetting AjaxControlID="optGrid">
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="uxReportGrid" LoadingPanelID="uxLoadingPanelCustom" />
            </UpdatedControls>
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="optGrid" UpdatePanelRenderMode="Inline" LoadingPanelID="uxInvisiblePanel" />
            </UpdatedControls>
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="optCard" UpdatePanelRenderMode="Inline" LoadingPanelID="uxInvisiblePanel" />
            </UpdatedControls>
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="chkHeaderCV" UpdatePanelRenderMode="Inline" LoadingPanelID="uxInvisiblePanel" />
            </UpdatedControls>

            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="uxExporterTop" UpdatePanelRenderMode="Inline" LoadingPanelID="uxInvisiblePanel" />
            </UpdatedControls>



        </tek:AjaxSetting>
        <tek:AjaxSetting AjaxControlID="optCard">
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="uxReportGrid" LoadingPanelID="uxLoadingPanelCustom" />
            </UpdatedControls>
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="optGrid" UpdatePanelRenderMode="Inline" LoadingPanelID="uxInvisiblePanel" />
            </UpdatedControls>
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="optCard" UpdatePanelRenderMode="Inline" LoadingPanelID="uxInvisiblePanel" />
            </UpdatedControls>
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="chkHeaderCV" UpdatePanelRenderMode="Inline" LoadingPanelID="uxInvisiblePanel" />
            </UpdatedControls>
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="uxExporterTop" UpdatePanelRenderMode="Inline" LoadingPanelID="uxInvisiblePanel" />
            </UpdatedControls>
        </tek:AjaxSetting>

        <tek:AjaxSetting AjaxControlID="btnRebind">
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="uxReportGrid" />
            </UpdatedControls>
        </tek:AjaxSetting>
    </AjaxSettings>
</as:RadAjaxManagerProxy>
<div class="height-14"></div>
<uc:UxExport ID="uxExporterTop" IsOnTop="true" runat="server" GridID="uxReportGrid"
    OnNeedExportConfig="uxExport_OnNeedExportConfig" />

<div style="display: none">
    <asp:Button runat="server" ID="btnRebind" OnClick="btnRebind_Click" />
</div>

<div class="list-type-view">
    <div class="row ">
        <div class="col-md-12">
            <as:CheckBox runat="server" ID="chkHeaderCV" Text="Re-queue All" CssClass="control-inline text-requeue-all" meta:resourcekey="ASCheckBoxResource1" />
        </div>
    </div>

    <div class="btn-group  pull-right view-mode-options" role="group" aria-label="Default btn-group">
        <div id="GridView" runat="server" class="btn btn-default pos-relative mr-m-1 btn-option btn-option-gridview">
            <as:RadioButton ID="optGrid" runat="server" GroupName="GroupView" Text="GRIDS" OnCheckedChanged="ChangeView" AutoPostBack="True" meta:resourcekey="ASGroupViewGrids" />
        </div>
        <div id="CardView" runat="server" class="btn btn-default pos-relative pull-right btn-option btn-option-cardview">
            <as:RadioButton ID="optCard" runat="server" GroupName="GroupView" Text="CARDS" OnCheckedChanged="ChangeView" AutoPostBack="True" meta:resourcekey="ASGroupViewCards" />
        </div>
    </div>
</div>
<as:ASGrid ID="uxReportGrid" runat="server" AllowFilteringByColumn="false" AllowPaging="True"
    AllowSorting="True" AutoGenerateColumns="False" AllowSortFilterWhenExport="true"
    ASPagingMethod="SPASingleMethod1" GridName="Risk Management - Risk Analysis - Barometer Report"
    OnItemCommand="uxReportGrid_ItemCommand" IsAutoExportTemplate="true" IsCacheTemplateFile="false"
    OnSortCommand="uxReportGrid_SortCommand" OnNeedDataSource="uxReportGrid_NeedDataSource"
    OnItemDataBound="uxReportGrid_ItemDataBound" OnDataSourceReady="uxReportGrid_DataSourceReady" OnPreRender="uxReportGrid_PreRender"
    OnInit="uxReportGrid_Init" ClientSettings-EnableAlternatingItems="true" ClientSettings-ClientEvents-OnDataBound="ReloadHover()"
    HeaderStyle-Width="60px" CssClass="in" meta:resourcekey="uxReportGridResource1">
    <MasterTableView>
        <Columns>
            <as:ASGridTemplateColumn HeaderText="Wk" UniqueName="CheckBoxColumn" HeaderStyle-HorizontalAlign="Center"
                ItemStyle-HorizontalAlign="Center" meta:resourcekey="CheckBoxColumn">
                <ItemTemplate>
                    <span class="checkbox-middle">
                        <input type="checkbox" id="cBox" runat="server" onclick="ChangeMerchantWorked(this)" />
                        &nbsp;</span>
                </ItemTemplate>

                <HeaderStyle HorizontalAlign="Center" Width="40px"></HeaderStyle>

                <ItemStyle HorizontalAlign="Center" Width="40px"></ItemStyle>
            </as:ASGridTemplateColumn>
            <as:ASGridTemplateColumn HeaderText="RQ" UniqueName="RQCheckbox" HeaderStyle-HorizontalAlign="Center" Visible="false"
                ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="40px" ItemStyle-Width="40px"
                HeaderTooltip="Check All Merchants (Checkbox to Assign)" meta:resourcekey="RQCheckbox">
                <HeaderTemplate>
                    RQ<br />
                    <input type="checkbox" id="chkHeader" runat="server" disabled="disabled" />
                </HeaderTemplate>
                <ItemTemplate>
                    <input type="checkbox" id="chkItem" runat="server" title="Checkbox to assign" />
                </ItemTemplate>

                <HeaderStyle HorizontalAlign="Center" Wrap="true" Width="40px"></HeaderStyle>

                <ItemStyle HorizontalAlign="Center" Width="40px"></ItemStyle>
            </as:ASGridTemplateColumn>
            <as:ASGridBoundColumn AllowSorting="true" HeaderText="Merchant Name(*)" DataField="MerchantName"
                UniqueName="MerchantName" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="left"
                Visible="true" HeaderStyle-Wrap="false" ItemStyle-Wrap="true" SortExpression="MerchantName"
                HeaderTooltip="Merchant Name" ItemStyle-CssClass="merchantName" HeaderStyle-Width="250px" meta:resourcekey="MerchantName">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>
                <HeaderStyle HorizontalAlign="Center" Wrap="False" Width="250px"></HeaderStyle>
                <ItemStyle HorizontalAlign="Left" Wrap="True" CssClass="merchantName" Width="250px"></ItemStyle>
            </as:ASGridBoundColumn>
            <tek:GridBoundColumn HeaderText="Merchant ID" DataField="MerchantNumber" UniqueName="MerchantNumber"
                Visible="false" HeaderStyle-Width="200px" meta:resourcekey="MerchantNumber">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>
            </tek:GridBoundColumn>
            <as:ASGridBoundColumn HeaderText="#WKD 30" DataField="TotalOfWorked" UniqueName="TotalOfWorked"
                HeaderStyle-HorizontalAlign="Center" HeaderStyle-Wrap="true" ItemStyle-Wrap="true" ASFormat="Integer"
                SortExpression="TotalOfWorked" HeaderTooltip="Times worked in 30 days" meta:resourcekey="TotalOfWorked">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center" Wrap="True"></HeaderStyle>

                <ItemStyle Wrap="True"></ItemStyle>
            </as:ASGridBoundColumn>
            <tek:GridBoundColumn HeaderText="BA" DataField="BusinessAge" UniqueName="BusinessAge" HeaderTooltip="Business Age"
                HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Wrap="true"
                ItemStyle-Wrap="false" HeaderStyle-Width="80px" meta:resourcekey="BusinessAge">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>
                <HeaderStyle HorizontalAlign="Center" Wrap="True"></HeaderStyle>
                <ItemStyle Wrap="False"></ItemStyle>
            </tek:GridBoundColumn>
            <as:ASGridBoundColumn HeaderText="RS" DataField="RiskScore" UniqueName="RiskScore"
                HeaderStyle-HorizontalAlign="Center" HeaderStyle-Wrap="true" ItemStyle-Wrap="true" HeaderStyle-Width="70px"
                SortExpression="RiskScore" HeaderTooltip="Risk Score" ASFormat="Integer" meta:resourcekey="RiskScore">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center" Wrap="True"></HeaderStyle>

                <ItemStyle Wrap="True"></ItemStyle>
            </as:ASGridBoundColumn>
            <as:ASGridBoundColumn HeaderText="V%" DataField="VolumePercent" UniqueName="VolumePercent"
                HeaderStyle-HorizontalAlign="Center" HeaderStyle-Wrap="true" ItemStyle-Wrap="false"
                SortExpression="VolumePercent" HeaderTooltip="Volume %" ASFormat="Percentage0Digits" meta:resourcekey="VolumePercent">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center" Wrap="True"></HeaderStyle>

                <ItemStyle Wrap="False"></ItemStyle>
            </as:ASGridBoundColumn>

            <as:ASGridBoundColumn HeaderText="CV%" DataField="ContractualVolume" UniqueName="ContractualVolume"
                HeaderStyle-HorizontalAlign="Center" HeaderStyle-Wrap="true" ItemStyle-Wrap="false" ASDefaultNullValue="—"
                SortExpression="ContractualVolume" HeaderTooltip="Contractual Volume %" ASFormat="Percentage" meta:resourcekey="ContractualVolume">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>
                <HeaderStyle HorizontalAlign="Center" Wrap="True" Width="105px"></HeaderStyle>
                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
            </as:ASGridBoundColumn>

            <as:ASGridBoundColumn HeaderText="AT%" DataField="AverageTicketPercent" UniqueName="AverageTicketPercent"
                HeaderStyle-HorizontalAlign="Center" HeaderStyle-Wrap="true" ItemStyle-Wrap="false"
                SortExpression="AverageTicketPercent" HeaderTooltip="Average Ticket %" ASFormat="Percentage0Digits" meta:resourcekey="AverageTicketPercent">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center" Wrap="True"></HeaderStyle>

                <ItemStyle Wrap="False"></ItemStyle>
            </as:ASGridBoundColumn>
            <as:ASGridBoundColumn HeaderText="A%" DataField="AuthorizationPercent" UniqueName="AuthorizationPercent"
                HeaderStyle-HorizontalAlign="Center" HeaderStyle-Wrap="true" ItemStyle-Wrap="false"
                SortExpression="AuthorizationPercent" HeaderTooltip="Authorization %" ASFormat="Percentage0Digits" meta:resourcekey="AuthorizationPercent">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center" Wrap="True"></HeaderStyle>

                <ItemStyle Wrap="False"></ItemStyle>
            </as:ASGridBoundColumn>
            <as:ASGridBoundColumn HeaderText="DA%" DataField="DeclinedAuthorizationPercent" UniqueName="DeclinedAuthorizationPercent"
                HeaderStyle-HorizontalAlign="Center" HeaderStyle-Wrap="true" ItemStyle-Wrap="true"
                SortExpression="DeclinedAuthorizationPercent" HeaderTooltip="Declined Authorization %"
                ASFormat="Percentage0Digits" meta:resourcekey="DeclinedAuthorizationPercent">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center" Wrap="True"></HeaderStyle>

                <ItemStyle Wrap="True"></ItemStyle>
            </as:ASGridBoundColumn>
            <as:ASGridBoundColumn HeaderText="#RA" DataField="RepeatAuthorizationCount" UniqueName="RepeatAuthorizationCount"
                HeaderStyle-HorizontalAlign="Center" HeaderStyle-Wrap="true" ItemStyle-Wrap="true" ASFormat="Integer"
                SortExpression="RepeatAuthorizationCount" HeaderTooltip="Repeat Authorization Count" meta:resourcekey="RepeatAuthorizationCount">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center" Wrap="True"></HeaderStyle>

                <ItemStyle Wrap="True"></ItemStyle>
            </as:ASGridBoundColumn>
            <as:ASGridBoundColumn HeaderText="#FC" DataField="TodayForeignCardCount" UniqueName="TodayForeignCardCount"
                HeaderStyle-HorizontalAlign="Center" HeaderStyle-Wrap="true" ItemStyle-Wrap="true" ASFormat="Integer"
                SortExpression="TodayForeignCardCount" HeaderTooltip="Today's Foreign Card Count" meta:resourcekey="TodayForeignCardCount">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center" Wrap="True"></HeaderStyle>

                <ItemStyle Wrap="True"></ItemStyle>
            </as:ASGridBoundColumn>
            <as:ASGridBoundColumn HeaderText="K%" DataField="KeyPercent" UniqueName="KeyPercent"
                HeaderStyle-HorizontalAlign="Center" HeaderStyle-Wrap="true" ItemStyle-Wrap="true"
                SortExpression="KeyPercent" HeaderTooltip="Keyed Percent" ASFormat="Percentage0Digits" meta:resourcekey="KeyPercent">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center" Wrap="True"></HeaderStyle>

                <ItemStyle Wrap="True"></ItemStyle>
            </as:ASGridBoundColumn>
            <as:ASGridBoundColumn HeaderText="=%" DataField="EvenDollarTransactionPercent" UniqueName="EvenDollarTransactionPercent"
                HeaderStyle-HorizontalAlign="Center" HeaderStyle-Wrap="true" ItemStyle-Wrap="true"
                SortExpression="EvenDollarTransactionPercent" HeaderTooltip="Even Dollar Transaction %"
                ASFormat="Percentage0Digits" meta:resourcekey="EvenDollarTransactionPercent">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center" Wrap="True"></HeaderStyle>

                <ItemStyle Wrap="True"></ItemStyle>
            </as:ASGridBoundColumn>
            <as:ASGridBoundColumn HeaderText="DD%" DataField="DuplicateDollarTransactionPercent"
                UniqueName="DuplicateDollarTransactionPercent" HeaderStyle-HorizontalAlign="Center"
                HeaderStyle-Wrap="true" ItemStyle-Wrap="true" SortExpression="DuplicateDollarTransactionPercent"
                HeaderTooltip="Duplicate Dollar Transaction %" ASFormat="Percentage0Digits" meta:resourcekey="DuplicateDollarTransactionPercent">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center" Wrap="True"></HeaderStyle>

                <ItemStyle Wrap="True"></ItemStyle>
            </as:ASGridBoundColumn>
            <as:ASGridBoundColumn HeaderText="DB" DataField="DuplicateBin" UniqueName="DuplicateBin"
                HeaderStyle-HorizontalAlign="Center" HeaderStyle-Wrap="true" ItemStyle-Wrap="true" ASFormat="Integer"
                SortExpression="DuplicateBin" HeaderTooltip="Duplicate Bin" meta:resourcekey="DuplicateBin">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center" Wrap="True"></HeaderStyle>

                <ItemStyle Wrap="True"></ItemStyle>
            </as:ASGridBoundColumn>
            <as:ASGridBoundColumn HeaderText="#-" DataField="NegativeBatchCount" UniqueName="NegativeBatchCount"
                HeaderStyle-HorizontalAlign="Center" HeaderStyle-Wrap="true" ItemStyle-Wrap="true" ASFormat="Integer"
                SortExpression="NegativeBatchCount" HeaderTooltip="Negative Batch Count" meta:resourcekey="NegativeBatchCount">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center" Wrap="True"></HeaderStyle>

                <ItemStyle Wrap="True"></ItemStyle>
            </as:ASGridBoundColumn>
            <as:ASGridBoundColumn HeaderText="#0" DataField="ZeroBatchCount" UniqueName="ZeroBatchCount" ASFormat="Integer"
                HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right" HeaderStyle-Wrap="true"
                ItemStyle-Wrap="true" SortExpression="ZeroBatchCount" HeaderTooltip="Zero Batch Count" meta:resourcekey="ZeroBatchCount">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center" Wrap="True"></HeaderStyle>

                <ItemStyle HorizontalAlign="Right" Wrap="True"></ItemStyle>
            </as:ASGridBoundColumn>
            <as:ASGridBoundColumn HeaderText="TV" DataField="TodayVolume" UniqueName="TodayVolume"
                HeaderStyle-HorizontalAlign="Center" HeaderStyle-Wrap="true" ItemStyle-Wrap="true" HeaderStyle-Width="95px"
                SortExpression="TodayVolume" HeaderTooltip="Today's Total Volume" ASFormat="Currency" meta:resourcekey="TodayVolume">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center" Wrap="True"></HeaderStyle>

                <ItemStyle Wrap="True"></ItemStyle>
            </as:ASGridBoundColumn>
            <as:ASGridBoundColumn HeaderText="RV$" DataField="TodayFirstTimeRetrievalVolume" UniqueName="TodayFirstTimeRetrievalVolume"
                HeaderStyle-HorizontalAlign="Center" HeaderStyle-Wrap="true" ItemStyle-Wrap="true"
                SortExpression="TodayFirstTimeRetrievalVolume" HeaderTooltip="Today's First Time Retrieval Volume"
                ASFormat="Currency" HeaderStyle-Width="85px" meta:resourcekey="TodayFirstTimeRetrievalVolume">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center" Wrap="True" Width="85px"></HeaderStyle>

                <ItemStyle Wrap="True" Width="85px"></ItemStyle>
            </as:ASGridBoundColumn>
            <as:ASGridBoundColumn HeaderText="CB" DataField="TodayChargebackVolume" UniqueName="TodayChargebackVolume"
                HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right" HeaderStyle-Wrap="true"
                ItemStyle-Wrap="true" SortExpression="TodayChargebackVolume" HeaderTooltip="Today's Chargeback Volume"
                ASFormat="Currency" HeaderStyle-Width="100px" meta:resourcekey="TodayChargebackVolume">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center" Wrap="True" Width="100px"></HeaderStyle>

                <ItemStyle HorizontalAlign="Right" Wrap="True"></ItemStyle>
            </as:ASGridBoundColumn>
            <as:ASGridBoundColumn HeaderText="R%" DataField="ReturnPercent" UniqueName="ReturnPercent"
                HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right" HeaderStyle-Wrap="true"
                ItemStyle-Wrap="false" SortExpression="ReturnPercent" HeaderTooltip="Return %"
                ASFormat="Percentage0Digits" meta:resourcekey="ReturnPercent">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center" Wrap="True"></HeaderStyle>

                <ItemStyle HorizontalAlign="Right" Wrap="False"></ItemStyle>
            </as:ASGridBoundColumn>
            <as:ASGridBoundColumn HeaderText="MT$" DataField="TodayHighestTransactionAmount"
                UniqueName="TodayHighestTransactionAmount" HeaderStyle-HorizontalAlign="Center"
                ItemStyle-HorizontalAlign="Right" HeaderStyle-Wrap="true" ItemStyle-Wrap="true"
                SortExpression="TodayHighestTransactionAmount" HeaderTooltip="Today's Highest Transaction Amount"
                ASFormat="Currency" HeaderStyle-Width="95px" meta:resourcekey="TodayHighestTransactionAmount">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center" Wrap="True" Width="95px"></HeaderStyle>

                <ItemStyle HorizontalAlign="Right" Wrap="True"></ItemStyle>
            </as:ASGridBoundColumn>
            <as:ASGridBoundColumn HeaderText="#T" DataField="TodayTransactionCount" UniqueName="TodayTransactionCount"
                HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right" HeaderStyle-Wrap="true" ASFormat="Integer"
                ItemStyle-Wrap="true" SortExpression="TodayTransactionCount" HeaderTooltip="Today's Transaction Count" meta:resourcekey="TodayTransactionCount">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center" Wrap="True"></HeaderStyle>

                <ItemStyle HorizontalAlign="Right" Wrap="True"></ItemStyle>
            </as:ASGridBoundColumn>
            <as:ASGridBoundColumn HeaderText="#B" DataField="TodayBatchCount" UniqueName="TodayBatchCount" ASFormat="Integer"
                HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right" HeaderStyle-Wrap="true"
                ItemStyle-Wrap="true" SortExpression="TodayBatchCount" HeaderTooltip="Today's Batch Count" meta:resourcekey="TodayBatchCount">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center" Wrap="True"></HeaderStyle>

                <ItemStyle HorizontalAlign="Right" Wrap="True"></ItemStyle>
            </as:ASGridBoundColumn>
            <as:ASGridBoundColumn HeaderText="SC" DataField="SingleCardTransToday" UniqueName="SingleCardTransToday"
                HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right" HeaderStyle-Wrap="true" ASFormat="Integer"
                ItemStyle-Wrap="true" SortExpression="SingleCardTransToday" HeaderTooltip="Similar Card Transactions Today" meta:resourcekey="SingleCardTransToday">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center" Wrap="True"></HeaderStyle>

                <ItemStyle HorizontalAlign="Right" Wrap="True"></ItemStyle>
            </as:ASGridBoundColumn>
            <as:ASGridBoundColumn HeaderText="SIC" DataField="SICCode" UniqueName="SICCode" HeaderStyle-HorizontalAlign="Center"
                ItemStyle-HorizontalAlign="Right" HeaderStyle-Wrap="true" ItemStyle-Wrap="true"
                SortExpression="SICCode" HeaderTooltip="Standard Industry Code" ASFormat="StaticString" meta:resourcekey="SICCode">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center" Wrap="True"></HeaderStyle>

                <ItemStyle HorizontalAlign="Right" Wrap="True"></ItemStyle>
            </as:ASGridBoundColumn>
            <as:ASGridBoundColumn HeaderText="Profile" DataField="ProfileDescription" UniqueName="ProfileDescription"
                HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="left" Visible="true"
                HeaderStyle-Wrap="true" ItemStyle-Wrap="true" SortExpression="ProfileDescription"
                HeaderTooltip="Profile Description" HeaderStyle-Width="80px" meta:resourcekey="ProfileDescription">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center" Wrap="True" Width="80px"></HeaderStyle>

                <ItemStyle HorizontalAlign="Left" Wrap="True"></ItemStyle>
            </as:ASGridBoundColumn>
            <as:ASGridBoundColumn HeaderText="WQ" DataField="WorkQueueName" UniqueName="WorkQueueName"
                HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" Visible="true"
                HeaderStyle-Wrap="true" ItemStyle-Wrap="true" SortExpression="WorkQueueName"
                HeaderTooltip="WQ Assigned" HeaderStyle-Width="80px" meta:resourcekey="WorkQueueName">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center" Wrap="True" Width="80px"></HeaderStyle>

                <ItemStyle HorizontalAlign="Center" Wrap="True"></ItemStyle>
            </as:ASGridBoundColumn>
            <as:ASGridBoundColumn HeaderText="PV" DataField="RulesViolated" UniqueName="RulesViolated"
                HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="left" Visible="true" ItemStyle-Width="64px"
                HeaderStyle-Wrap="true" ItemStyle-Wrap="true" SortExpression="RulesViolated" ItemStyle-CssClass="ellipsis"
                HeaderTooltip="Parameters Violated" HeaderStyle-Width="64px" meta:resourcekey="RulesViolated">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center" Wrap="True" Width="64px"></HeaderStyle>

                <ItemStyle HorizontalAlign="Left" Wrap="True" Width="64px"></ItemStyle>
            </as:ASGridBoundColumn>
            <as:ASGridTemplateColumn DataField="CardView" UniqueName="CardView" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-CssClass="td-card-view">
                <ItemTemplate>
                    <div class="row card-view">
                        <div class="col-xs-3">
                            <div class="row w-min-180 mb-4x">
                                <div class="col-xs-2 rdHeader">
                                    <asp:Label ID="Label1" CssClass="rdHeader" runat="server" meta:resourcekey="ASGridTemplateColumnCardResource1">Wk</asp:Label>
                                    <div class="checkbox-middle">
                                        <input type="checkbox" id="cBoxCV" runat="server" onclick="ChangeMerchantWorked(this)" />
                                    </div>
                                </div>
                                <div class="col-xs-2 rdHeader" id="colRQColumn" runat="server">
                                    <asp:Label ID="Label2" CssClass="rdHeader" runat="server">RQ</asp:Label>
                                    <div id="bgChkItemCV" runat="server">
                                        <input type="checkbox" id="chkItemCV" runat="server" title="Checkbox to assign" />
                                    </div>
                                </div>
                                <div class="col-xs-8" id="colMerchantName" runat="server">
                                    <asp:LinkButton ID="lbtSort_MerchantName" CssClass="rdHeader" runat="server" Text="Merchant Name" CommandName="Sort_MerchantName" meta:resourcekey="ASGridBoundColumnCardResource1"></asp:LinkButton>
                                    <span class="icon-sort"></span>
                                    <div runat="server" id="merchantName"><%# Eval("MerchantName") %></div>
                                </div>
                            </div>
                            <div class="row w-min-180 mb-4x">
                                <div class="col-xs-4">
                                    <asp:LinkButton ID="lbtSort_BusinessAge" CssClass="rdHeader" runat="server" Text="BA" CommandName="Sort_BusinessAge" meta:resourcekey="ASGridBoundColumnCardResource0"></asp:LinkButton>
                                    <div runat="server" id="BusinessAge"><%# Eval("BusinessAge") %></div>
                                </div>
                                <div class="col-xs-4">
                                    <asp:LinkButton ID="lbtSort_SICCode" CssClass="rdHeader" runat="server" Text="SIC" CommandName="Sort_SICCode" meta:resourcekey="ASGridBoundColumnCardResource23"></asp:LinkButton>
                                    <span class="icon-sort"></span>
                                    <div runat="server" id="sicCode"><%# Eval("SICCode") %></div>
                                </div>
                                <div class="col-xs-4">
                                    <asp:LinkButton ID="lbtSort_ProfileDescription" CssClass="rdHeader" runat="server" Text="Profile" CommandName="Sort_ProfileDescription" meta:resourcekey="ASGridBoundColumnCardResource24"></asp:LinkButton>
                                    <span class="icon-sort"></span>
                                    <div runat="server" id="profileDes"><%# Eval("ProfileDescription") %></div>
                                </div>
                            </div>
                            <div class="row w-min-180 mb-4x">
                                <div class="col-xs-4">
                                    <asp:LinkButton ID="lbtSort_RiskScore" CssClass="rdHeader" runat="server" Text="RS" CommandName="Sort_RiskScore" meta:resourcekey="ASGridBoundColumnCardResource2"></asp:LinkButton>
                                    <span class="icon-sort"></span>
                                    <div runat="server" id="riskScore"><%# Eval("RiskScore") %></div>
                                </div>
                            </div>
                        </div>
                        <div class="col-xs-9">
                            <div class="auto-grid">
                                <div class="item ">
                                    <asp:LinkButton ID="lbtSort_TotalOfWorked" CssClass="rdHeader" runat="server" CommandName="Sort_TotalOfWorked" Text="#WKD 30" meta:resourcekey="ASGridBoundColumnCardResource25"></asp:LinkButton>
                                    <span class="icon-sort"></span>
                                    <div id="totalOfWorked" runat="server"><%# Eval("TotalOfWorked") %></div>
                                </div>
                                <div class="item ">
                                    <asp:LinkButton ID="lbtSort_VolumePercent" CssClass="rdHeader" runat="server" CommandName="Sort_VolumePercent" Text="V%" meta:resourcekey="ASGridBoundColumnCardResource3"></asp:LinkButton>
                                    <span class="icon-sort"></span>
                                    <div id="volumePercent" runat="server"><%# Eval("VolumePercent") %></div>
                                </div>

                                <div class="item ">
                                    <asp:LinkButton ID="LinkButton1" CssClass="rdHeader" runat="server" CommandName="Sort_ContractualVolume" Text="CV%" meta:resourcekey="ASGridBoundColumnCardContractualVolume"></asp:LinkButton>
                                    <span class="icon-sort"></span>
                                    <div id="contractualVolume" runat="server"><%# Eval("ContractualVolume") != DBNull.Value ? Eval("ContractualVolume") : WebSiteConstants.HTML_EM_DASH_ENCODE  %></div>
                                </div>


                                <div class="item ">
                                    <asp:LinkButton ID="lbtSort_AverageTicketPercent" CssClass="rdHeader" runat="server" CommandName="Sort_AverageTicketPercent" Text="AT%" meta:resourcekey="ASGridBoundColumnCardResource4"></asp:LinkButton>
                                    <span class="icon-sort"></span>
                                    <div id="avgTicket" runat="server"><%# Eval("AverageTicketPercent") %></div>
                                </div>
                                <div class="item ">
                                    <asp:LinkButton ID="lbtSort_AuthorizationPercent" CssClass="rdHeader" runat="server" CommandName="Sort_AuthorizationPercent" Text="A%" meta:resourcekey="ASGridBoundColumnCardResource5"></asp:LinkButton>
                                    <span class="icon-sort"></span>
                                    <div id="authPercent" runat="server"><%# Eval("AuthorizationPercent") %></div>
                                </div>
                                <div class="item ">
                                    <asp:LinkButton ID="lbtSort_DeclinedAuthorizationPercent" CssClass="rdHeader" runat="server" CommandName="Sort_DeclinedAuthorizationPercent" Text="DA%" meta:resourcekey="ASGridBoundColumnCardResource6"></asp:LinkButton>
                                    <span class="icon-sort"></span>
                                    <div id="decPercent" runat="server"><%# Eval("DeclinedAuthorizationPercent") %></div>
                                </div>
                                <div class="item ">
                                    <asp:LinkButton ID="lbtSort_RepeatAuthorizationCount" CssClass="rdHeader" runat="server" CommandName="Sort_RepeatAuthorizationCount" Text="#RA" meta:resourcekey="ASGridBoundColumnCardResource7"></asp:LinkButton>
                                    <span class="icon-sort"></span>
                                    <div id="rptAuth" runat="server"><%# Eval("RepeatAuthorizationCount") %></div>
                                </div>
                                <div class="item ">
                                    <asp:LinkButton ID="lbtSort_KeyPercent" CssClass="rdHeader" runat="server" CommandName="Sort_KeyPercent" Text="K%" meta:resourcekey="ASGridBoundColumnCardResource9"></asp:LinkButton>
                                    <span class="icon-sort"></span>
                                    <div id="keypercent" runat="server"><%# Eval("KeyPercent") %></div>

                                </div>
                                <div class="item ">
                                    <asp:LinkButton ID="lbtSort_TodayForeignCardCount" CssClass="rdHeader" runat="server" CommandName="Sort_TodayForeignCardCount" Text="#FC" meta:resourcekey="ASGridBoundColumnCardResource8"></asp:LinkButton>
                                    <span class="icon-sort"></span>
                                    <div id="fCardCout" runat="server"><%# Eval("TodayForeignCardCount") %></div>
                                </div>
                                <div class="item ">
                                    <asp:LinkButton ID="lbtSort_EvenDollarTransactionPercent" CssClass="rdHeader" runat="server" CommandName="Sort_EvenDollarTransactionPercent" Text="=%" meta:resourcekey="ASGridBoundColumnCardResource10"></asp:LinkButton>
                                    <span class="icon-sort"></span>
                                    <div id="evenPercent" runat="server"><%# Eval("EvenDollarTransactionPercent") %></div>
                                </div>

                                <div class="item ">
                                    <asp:LinkButton ID="lbtSort_DuplicateDollarTransactionPercent" CssClass="rdHeader" runat="server" CommandName="Sort_DuplicateDollarTransactionPercent" Text="DD%" meta:resourcekey="ASGridBoundColumnCardResource11"></asp:LinkButton>
                                    <span class="icon-sort"></span>
                                    <div id="dupPercent" runat="server"><%# Eval("DuplicateDollarTransactionPercent") %></div>
                                </div>
                                <div class="item ">
                                    <asp:LinkButton ID="lbtSort_DuplicateBin" CssClass="rdHeader" runat="server" CommandName="Sort_DuplicateBin" Text="DB" meta:resourcekey="ASGridBoundColumnCardResource12"></asp:LinkButton>
                                    <span class="icon-sort"></span>
                                    <div id="dupBin" runat="server"><%# Eval("DuplicateBin") %></div>
                                </div>
                                <div class="item ">
                                    <asp:LinkButton ID="lbtSort_TodayVolume" CssClass="rdHeader" runat="server" CommandName="Sort_TodayVolume" Text="TV" meta:resourcekey="ASGridBoundColumnCardResource15"></asp:LinkButton>
                                    <span class="icon-sort"></span>
                                    <div id="todayVolume" runat="server"><%# Eval("TodayVolume") %></div>
                                </div>
                                <div class="item ">
                                    <asp:LinkButton ID="lbtSort_TodayHighestTransactionAmount" CssClass="rdHeader" runat="server" CommandName="Sort_TodayHighestTransactionAmount" Text="MT$" meta:resourcekey="ASGridBoundColumnCardResource19"></asp:LinkButton>
                                    <span class="icon-sort"></span>
                                    <div id="maxTkt" runat="server"><%# Eval("TodayHighestTransactionAmount") %></div>
                                </div>
                                <div class="item ">
                                    <asp:LinkButton ID="lbtSort_TodayTransactionCount" CssClass="rdHeader" runat="server" CommandName="Sort_TodayTransactionCount" Text="#T" meta:resourcekey="ASGridBoundColumnCardResource20"></asp:LinkButton>
                                    <span class="icon-sort"></span>
                                    <div id="todayTC" runat="server"><%# Eval("TodayTransactionCount") %></div>
                                </div>
                                <div class="item ">
                                    <asp:LinkButton ID="lbtSort_TodayBatchCount" CssClass="rdHeader" runat="server" CommandName="Sort_TodayBatchCount" Text="#B" meta:resourcekey="ASGridBoundColumnCardResource21"></asp:LinkButton>
                                    <span class="icon-sort"></span>
                                    <div id="todayBC" runat="server"><%# Eval("TodayBatchCount") %></div>
                                </div>
                                <div class="item ">
                                    <asp:LinkButton ID="lbtSort_TodayFirstTimeRetrievalVolume" CssClass="rdHeader" runat="server" CommandName="Sort_TodayFirstTimeRetrievalVolume" Text="RV" meta:resourcekey="ASGridBoundColumnCardResource16"></asp:LinkButton>
                                    <span class="icon-sort"></span>
                                    <div id="todayRV" runat="server"><%# Eval("TodayFirstTimeRetrievalVolume") %></div>
                                </div>
                                <div class="item ">
                                    <asp:LinkButton ID="lbtSort_TodayChargebackVolume" CssClass="rdHeader" runat="server" CommandName="Sort_TodayChargebackVolume" Text="CB" meta:resourcekey="ASGridBoundColumnCardResource17"></asp:LinkButton>
                                    <span class="icon-sort"></span>
                                    <div id="todayCB" runat="server"><%# Eval("TodayChargebackVolume") %></div>
                                </div>

                                <div class="item ">
                                    <asp:LinkButton ID="lbtSort_NegativeBatchCount" CssClass="rdHeader" runat="server" CommandName="Sort_NegativeBatchCount" Text="#-" meta:resourcekey="ASGridBoundColumnCardResource13"></asp:LinkButton>
                                    <span class="icon-sort"></span>
                                    <div id="negativeBC" runat="server"><%# Eval("NegativeBatchCount") %></div>
                                </div>
                                <div class="item ">
                                    <asp:LinkButton ID="lbtSort_ZeroBatchCount" CssClass="rdHeader" runat="server" CommandName="Sort_ZeroBatchCount" Text="#0" meta:resourcekey="ASGridBoundColumnCardResource14"></asp:LinkButton>
                                    <span class="icon-sort"></span>
                                    <div id="zeroBC" runat="server"><%# Eval("ZeroBatchCount") %></div>
                                </div>
                                <div class="item ">
                                    <asp:LinkButton ID="lbtSort_SingleCardTransToday" CssClass="rdHeader" runat="server" CommandName="Sort_SingleCardTransToday" Text="SC" meta:resourcekey="ASGridBoundColumnCardResource22"></asp:LinkButton>
                                    <span class="icon-sort"></span>
                                    <div id="scTransToday" runat="server"><%# Eval("SingleCardTransToday") %></div>
                                </div>
                                <div class="item ">
                                    <asp:LinkButton ID="lbtSort_ReturnPercent" CssClass="rdHeader" runat="server" CommandName="Sort_ReturnPercent" Text="R%" meta:resourcekey="ASGridBoundColumnCardResource18"></asp:LinkButton>
                                    <span class="icon-sort"></span>
                                    <div id="rtnPercent" runat="server"><%# Eval("ReturnPercent") %></div>
                                </div>
                                <div class="item ">
                                    <asp:LinkButton ID="lbtSort_WorkQueueName" CssClass="rdHeader" runat="server" CommandName="Sort_WorkQueueName" Text="WQ" meta:resourcekey="ASGridBoundColumnCardResource26"></asp:LinkButton>
                                    <span class="icon-sort"></span>
                                    <div id="workQueueName" runat="server"><%# Eval("WorkQueueName") %></div>
                                </div>
                                <div class="item ">
                                    <asp:LinkButton ID="lbtSort_RulesViolated" CssClass="rdHeader" runat="server" CommandName="Sort_RulesViolated" Text="Parameters Violated" meta:resourcekey="ASGridBoundColumnCardResource27"></asp:LinkButton>
                                    <span class="icon-sort"></span>
                                    <div id="rulesViolated" title='<%# Eval("RulesViolated") %>' runat="server"><%# Eval("RulesViolated").ToSafeString().Length > 35 ?  (Eval("RulesViolated").ToSafeString().Substring(0, 35) + "...") : Eval("RulesViolated") %></div>
                                </div>
                            </div>
                        </div>
                    </div>
                </ItemTemplate>
            </as:ASGridTemplateColumn>
        </Columns>
    </MasterTableView>
    <HeaderStyle Width="60px"></HeaderStyle>
</as:ASGrid>
<as:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <script type="text/javascript">
        var detectionQueueRainbowReport_uxReportGrid = "<%=uxReportGrid.ClientID%>";
        var rm_Button_Rebind_ClientID = "<% = btnRebind.ClientID%>";
        function doOpenNewPopup(encodeURL) {
            parent.master_closeModalEvent = function () {
                document.getElementById(rm_Button_Rebind_ClientID).click();
                // parent.location = parent.location;
            }
            return parent.ShowPopupModal(encodeURL, 'auto');
        }
    </script>
    <script type="text/javascript" src="<%=ResolveUrl("~") %>res/js/risk/DetectionQueueRainbowReport.js">             
    </script>
</as:RadCodeBlock>
