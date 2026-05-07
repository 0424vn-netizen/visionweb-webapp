<%@ Control Language="C#" AutoEventWireup="true" CodeFile="rm_MCF_DQDistinctReport.ascx.cs"
    Inherits="UserControls_rm_MCF_DQDistinctReport" %>
<%@ Register TagPrefix="uc" TagName="UxExport" Src="~/UserControls/rm_MCF_UxDetectionQueueExport.ascx" %>

<as:RadAjaxManagerProxy ID="RadAjaxManagerProxy1" runat="server">
    <AjaxSettings>
        <tek:AjaxSetting AjaxControlID="uxReportDistinctGrid">
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="uxReportDistinctGrid" LoadingPanelID="uxLoadingPanelCustom" />
                <tek:AjaxUpdatedControl ControlID="optGrid" UpdatePanelRenderMode="Inline" LoadingPanelID="uxInvisiblePanel" />
                <tek:AjaxUpdatedControl ControlID="optCard" UpdatePanelRenderMode="Inline" LoadingPanelID="uxInvisiblePanel" />
            </UpdatedControls>
        </tek:AjaxSetting>
        <tek:AjaxSetting AjaxControlID="optGrid">
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="uxReportDistinctGrid" LoadingPanelID="uxLoadingPanelCustom" />
                <tek:AjaxUpdatedControl ControlID="optGrid" UpdatePanelRenderMode="Inline" LoadingPanelID="uxInvisiblePanel" />
                <tek:AjaxUpdatedControl ControlID="optCard" UpdatePanelRenderMode="Inline" LoadingPanelID="uxInvisiblePanel" />
                <tek:AjaxUpdatedControl ControlID="chkHeaderCV" UpdatePanelRenderMode="Inline" LoadingPanelID="uxInvisiblePanel" />
                <tek:AjaxUpdatedControl ControlID="uxExporterAggregateTop" UpdatePanelRenderMode="Inline" LoadingPanelID="uxInvisiblePanel" />
            </UpdatedControls>
        </tek:AjaxSetting>
        <tek:AjaxSetting AjaxControlID="optCard">
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="uxReportDistinctGrid" LoadingPanelID="uxLoadingPanelCustom" />
                <tek:AjaxUpdatedControl ControlID="optGrid" UpdatePanelRenderMode="Inline" LoadingPanelID="uxInvisiblePanel" />
                <tek:AjaxUpdatedControl ControlID="optCard" UpdatePanelRenderMode="Inline" LoadingPanelID="uxInvisiblePanel" />
                <tek:AjaxUpdatedControl ControlID="chkHeaderCV" UpdatePanelRenderMode="Inline" LoadingPanelID="uxInvisiblePanel" />
                <tek:AjaxUpdatedControl ControlID="uxExporterAggregateTop" UpdatePanelRenderMode="Inline" LoadingPanelID="uxInvisiblePanel" />
            </UpdatedControls>
        </tek:AjaxSetting>
    </AjaxSettings>
</as:RadAjaxManagerProxy>


<div class="height-14"></div>
<uc:UxExport ID="uxExporterDistinctTop" IsOnTop="true" runat="server" GridID="uxReportDistinctGrid"
    OnNeedExportConfig="uxExport_OnNeedExportConfig" />
<div class="list-type-view">
    <div class="row">
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
<as:ASGrid ID="uxReportDistinctGrid" runat="server" AllowFilteringByColumn="false" CssClass="freeze-table"
    AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" AllowSortFilterWhenExport="true"
    ASPagingMethod="SPASingleMethod" GridName="Risk Management - Risk Analysis - Detection Queue Distinct Report"
    OnNeedDataSource="uxReportGrid_NeedDataSource" OnItemDataBound="uxReportGrid_ItemDataBound"
    OnDataSourceReady="uxReportGrid_DataSourceReady" PageSize="100" Width="100%"
    ClientSettings-EnableAlternatingItems="true" OnPreRender="uxReportDistinctGrid_PreRender"
    meta:resourcekey="uxReportDistinctGridResource1" OnItemCommand="uxReportDistinctGrid_ItemCommand"
    OnSortCommand="uxReportDistinctGrid_SortCommand">
    <MasterTableView TableLayout="Fixed" Width="100%">
        <Columns>
            <as:ASGridTemplateColumn HeaderText="RQ" UniqueName="RQCheckbox" HeaderStyle-HorizontalAlign="Center"
                ItemStyle-BackColor="White"
                Visible="false" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="44px" ItemStyle-Width="44px"
                HeaderTooltip="Check All Merchants (Checkbox to Assign)" meta:resourcekey="ASGridTemplateColumnResource1">
                <HeaderTemplate>
                    RQ
                    <input type="checkbox" id="chkHeader" runat="server" disabled="disabled" />
                </HeaderTemplate>
                <ItemTemplate>
                    <input type="checkbox" id="chkItem" runat="server" title="Checkbox to assign" />
                </ItemTemplate>

                <HeaderStyle HorizontalAlign="Center" Width="44px"></HeaderStyle>

                <ItemStyle HorizontalAlign="Center" BackColor="White" Width="44px"></ItemStyle>
            </as:ASGridTemplateColumn>
            <as:ASGridBoundColumn DataField="MerchantNumber" UniqueName="MerchantNumber" HeaderText="Merchant Number"
                HeaderTooltip="Merchant Number" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Wrap="false"
                HeaderStyle-Width="150px" ItemStyle-Width="150px" ItemStyle-Wrap="false"
                ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource1">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center" Wrap="False" Width="150px"></HeaderStyle>

                <ItemStyle Wrap="False" Width="150px"></ItemStyle>
            </as:ASGridBoundColumn>
            <as:ASGridBoundColumn DataField="MerchantName" UniqueName="MerchantName" HeaderText="Merchant Name"
                HeaderTooltip="Merchant Name" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="left"
                HeaderStyle-Width="270px" ItemStyle-Width="270px" HeaderStyle-Wrap="false" ItemStyle-Wrap="true"
                ASFormat="DynamicString" meta:resourcekey="ASGridBoundColumnResource2">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center" Wrap="False" Width="250px"></HeaderStyle>

                <ItemStyle HorizontalAlign="Left" Wrap="True" Width="250px"></ItemStyle>
            </as:ASGridBoundColumn>
            <as:ASGridBoundColumn DataField="Worked" UniqueName="Worked" HeaderText="#WKD 30"
                HeaderTooltip="Times worked in 30 days" HeaderStyle-Wrap="false" ItemStyle-Wrap="false"
                HeaderStyle-Width="70px" ItemStyle-Width="70px" ASFormat="Integer" meta:resourcekey="ASGridBoundColumnResource3">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center" Wrap="False" Width="70px"></HeaderStyle>

                <ItemStyle Wrap="False" Width="70px"></ItemStyle>
            </as:ASGridBoundColumn>
            <as:ASGridBoundColumn DataField="DaysActive" UniqueName="DaysActive" HeaderText="DA"
                HeaderTooltip="Days Active" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" HeaderStyle-Width="70px"
                ItemStyle-Width="70px" ASFormat="Integer" meta:resourcekey="ASGridBoundColumnResource4">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center" Wrap="False" Width="70px"></HeaderStyle>

                <ItemStyle Wrap="False" Width="70px"></ItemStyle>
            </as:ASGridBoundColumn>
            <as:ASGridBoundColumn DataField="DaysProcessedLast90" UniqueName="DaysProcessedLast90"
                HeaderText="DP" HeaderTooltip="Days Processed Last 90" HeaderStyle-Wrap="false"
                ItemStyle-Wrap="false" HeaderStyle-Width="70px" ItemStyle-Width="70px" ASFormat="Integer" meta:resourcekey="ASGridBoundColumnResource5">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center" Wrap="False" Width="70px"></HeaderStyle>

                <ItemStyle Wrap="False" Width="70px"></ItemStyle>
            </as:ASGridBoundColumn>
            <as:ASGridBoundColumn DataField="TransactionCountLast60" UniqueName="TransactionCountLast60"
                HeaderText="# Trans" HeaderTooltip="Transaction Count Last 60" HeaderStyle-Wrap="false"
                ItemStyle-Wrap="false" HeaderStyle-Width="70px" ItemStyle-Width="70px" ASFormat="Integer" meta:resourcekey="ASGridBoundColumnResource6">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center" Wrap="False" Width="70px"></HeaderStyle>

                <ItemStyle Wrap="False" Width="70px"></ItemStyle>
            </as:ASGridBoundColumn>
            <as:ASGridBoundColumn DataField="ForcedViolation" UniqueName="ForcedViolation" HeaderText="FV"
                HeaderTooltip="Forced Violation" HeaderStyle-Wrap="false" ItemStyle-Wrap="false"
                HeaderStyle-Width="50px" ItemStyle-Width="50px" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource7">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center" Wrap="False" Width="50px"></HeaderStyle>

                <ItemStyle Wrap="False" Width="50px"></ItemStyle>
            </as:ASGridBoundColumn>
            <as:ASGridBoundColumn DataField="TodayUnmatchedCreditsVolume" UniqueName="TodayUnmatchedCreditsVolume"
                HeaderText="UC" HeaderTooltip="Today’s Unmatched Credits Volume" ItemStyle-Wrap="false"
                HeaderStyle-Wrap="false" HeaderStyle-Width="90px" ItemStyle-Width="90px" ASFormat="Currency" meta:resourcekey="ASGridBoundColumnResource8">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center" Wrap="False" Width="90px"></HeaderStyle>

                <ItemStyle Wrap="False" Width="90px"></ItemStyle>
            </as:ASGridBoundColumn>
            <as:ASGridBoundColumn DataField="TodayHighestTicket" UniqueName="TodayHighestTicket"
                HeaderText="HT" HeaderTooltip="Today's Highest Ticket" HeaderStyle-Wrap="false"
                ItemStyle-Wrap="false" HeaderStyle-Width="90px" ItemStyle-Width="90px" ASFormat="Currency" meta:resourcekey="ASGridBoundColumnResource9">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center" Wrap="False" Width="90px"></HeaderStyle>

                <ItemStyle Wrap="False" Width="90px"></ItemStyle>
            </as:ASGridBoundColumn>
            <as:ASGridBoundColumn DataField="ExcessAboveContractHighest" UniqueName="ExcessAboveContractHighest"
                HeaderText="CH" HeaderTooltip="Excess Above Contract Highest" HeaderStyle-Wrap="false"
                ItemStyle-Wrap="false" HeaderStyle-Width="90px" ItemStyle-Width="90px" ASFormat="Currency" meta:resourcekey="ASGridBoundColumnResource10">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center" Wrap="False" Width="90px"></HeaderStyle>

                <ItemStyle Wrap="False" Width="90px"></ItemStyle>
            </as:ASGridBoundColumn>
            <as:ASGridBoundColumn DataField="TodayAverageTicket" UniqueName="TodayAverageTicket"
                HeaderText="AT" HeaderTooltip="Today's Average Ticket" HeaderStyle-Wrap="false"
                ItemStyle-Wrap="false" HeaderStyle-Width="90px" ItemStyle-Width="90px" ASFormat="Currency" meta:resourcekey="ASGridBoundColumnResource11">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center" Wrap="False" Width="90px"></HeaderStyle>

                <ItemStyle Wrap="False" Width="90px"></ItemStyle>
            </as:ASGridBoundColumn>
            <as:ASGridBoundColumn DataField="ExcessAboveContractAverage" UniqueName="ExcessAboveContractAverage"
                HeaderText="CA" HeaderTooltip="Excess Above Contract Average" HeaderStyle-Wrap="false"
                ItemStyle-Wrap="false" HeaderStyle-Width="90px" ItemStyle-Width="90px" ASFormat="Currency" meta:resourcekey="ASGridBoundColumnResource12">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center" Wrap="False" Width="90px"></HeaderStyle>

                <ItemStyle Wrap="False" Width="90px"></ItemStyle>
            </as:ASGridBoundColumn>
            <as:ASGridBoundColumn DataField="KeyedPercent" UniqueName="KeyedPercent" HeaderText="K%"
                HeaderTooltip="Keyed Percent" HeaderStyle-Wrap="false" ItemStyle-Wrap="false"
                HeaderStyle-Width="60px" ItemStyle-Width="60px" ASFormat="Percentage" meta:resourcekey="ASGridBoundColumnResource13">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center" Wrap="False" Width="60px"></HeaderStyle>

                <ItemStyle Wrap="False" Width="60px"></ItemStyle>
            </as:ASGridBoundColumn>
            <as:ASGridBoundColumn DataField="ExcessAboveKeyedPercent" UniqueName="ExcessAboveKeyedPercent"
                HeaderText="EK%" HeaderTooltip="Excess Above Keyed Percent" HeaderStyle-Wrap="false"
                ItemStyle-Wrap="false" HeaderStyle-Width="60px" ItemStyle-Width="60px" ASFormat="Percentage" meta:resourcekey="ASGridBoundColumnResource14">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center" Wrap="False" Width="60px"></HeaderStyle>

                <ItemStyle Wrap="False" Width="60px"></ItemStyle>
            </as:ASGridBoundColumn>
            <as:ASGridBoundColumn DataField="TodayTotalVolume" UniqueName="TodayTotalVolume"
                HeaderText="TV" HeaderTooltip="Today's Total Volume" HeaderStyle-Wrap="false"
                ItemStyle-Wrap="false" HeaderStyle-Width="90px" ItemStyle-Width="90px" ASFormat="Currency" meta:resourcekey="ASGridBoundColumnResource15">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center" Wrap="False" Width="90px"></HeaderStyle>

                <ItemStyle Wrap="False" Width="90px"></ItemStyle>
            </as:ASGridBoundColumn>
            <as:ASGridBoundColumn DataField="TodayBankCardSales" UniqueName="TodayBankCardSales"
                HeaderText="BS" HeaderTooltip="Today's Bank Card Sales" HeaderStyle-Wrap="false"
                ItemStyle-Wrap="false" HeaderStyle-Width="70px" ItemStyle-Width="70px" ASFormat="Integer" meta:resourcekey="ASGridBoundColumnResource16">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center" Wrap="False" Width="70px"></HeaderStyle>

                <ItemStyle Wrap="False" Width="70px"></ItemStyle>
            </as:ASGridBoundColumn>
            <as:ASGridBoundColumn DataField="ExcessAboveDailyContractVolume" UniqueName="ExcessAboveDailyContractVolume"
                HeaderText="DC" HeaderTooltip="Excess Above Daily Contract Volume (Annual / 365)"
                HeaderStyle-Wrap="false" ItemStyle-Wrap="false" HeaderStyle-Width="90px" ItemStyle-Width="90px"
                ASFormat="Currency" meta:resourcekey="ASGridBoundColumnResource17">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center" Wrap="False" Width="90px"></HeaderStyle>

                <ItemStyle Wrap="False" Width="90px"></ItemStyle>
            </as:ASGridBoundColumn>
            <as:ASGridBoundColumn DataField="P4VolumeIncreasePercent" UniqueName="P4VolumeIncreasePercent"
                HeaderText="P4 V%" HeaderTooltip="P4 Volume Increase Percent (90-day calculation)"
                HeaderStyle-Wrap="false" ItemStyle-Wrap="false" HeaderStyle-Width="70px" ItemStyle-Width="70px"
                ASFormat="Percentage" meta:resourcekey="ASGridBoundColumnResource18">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center" Wrap="False" Width="70px"></HeaderStyle>

                <ItemStyle Wrap="False" Width="70px"></ItemStyle>
            </as:ASGridBoundColumn>
            <as:ASGridBoundColumn DataField="MV1" UniqueName="MV1" HeaderText="MV1" HeaderTooltip="MV1 (Monthly volume previous month 1)"
                HeaderStyle-Wrap="false" ItemStyle-Wrap="false" HeaderStyle-Width="90px" ItemStyle-Width="90px"
                ASFormat="Currency" meta:resourcekey="ASGridBoundColumnResource19">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center" Wrap="False" Width="90px"></HeaderStyle>

                <ItemStyle Wrap="False" Width="90px"></ItemStyle>
            </as:ASGridBoundColumn>
            <as:ASGridBoundColumn DataField="MV2" UniqueName="MV2" HeaderText="MV2" HeaderTooltip="MV2 (Monthly volume previous month 2)"
                HeaderStyle-Wrap="false" ItemStyle-Wrap="false" HeaderStyle-Width="90px" ItemStyle-Width="90px"
                ASFormat="Currency" meta:resourcekey="ASGridBoundColumnResource20">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center" Wrap="False" Width="90px"></HeaderStyle>

                <ItemStyle Wrap="False" Width="90px"></ItemStyle>
            </as:ASGridBoundColumn>
            <as:ASGridBoundColumn DataField="MV3" UniqueName="MV3" HeaderText="MV3" HeaderTooltip="MV3 (Monthly volume previous month 3)"
                HeaderStyle-Wrap="false" ItemStyle-Wrap="false" HeaderStyle-Width="90px" ItemStyle-Width="90px"
                ASFormat="Currency" meta:resourcekey="ASGridBoundColumnResource21">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center" Wrap="False" Width="90px"></HeaderStyle>

                <ItemStyle Wrap="False" Width="90px"></ItemStyle>
            </as:ASGridBoundColumn>
            <as:ASGridBoundColumn DataField="Zip3" UniqueName="Zip3" HeaderText="Zip-3" HeaderTooltip="Zip-3"
                HeaderStyle-Wrap="false" ItemStyle-Wrap="false" HeaderStyle-Width="60px" ItemStyle-Width="60px"
                ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource22">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center" Wrap="False" Width="60px"></HeaderStyle>

                <ItemStyle Wrap="False" Width="60px"></ItemStyle>
            </as:ASGridBoundColumn>
            <as:ASGridBoundColumn DataField="State" UniqueName="State" HeaderText="State" HeaderTooltip="State"
                HeaderStyle-Wrap="false" ItemStyle-Wrap="false" HeaderStyle-Width="60px" ItemStyle-Width="60px"
                ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource23">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center" Wrap="False" Width="60px"></HeaderStyle>

                <ItemStyle Wrap="False" Width="60px"></ItemStyle>
            </as:ASGridBoundColumn>
            <as:ASGridBoundColumn DataField="SaleGroup" UniqueName="SaleGroup" HeaderText="SG"
                HeaderTooltip="Sale Group" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" HeaderStyle-Width="80px" ItemStyle-Width="80px"
                ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource24">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center" Wrap="False" Width="80px"></HeaderStyle>

                <ItemStyle Wrap="False" Width="80px"></ItemStyle>
            </as:ASGridBoundColumn>
            <as:ASGridBoundColumn DataField="ProfileDescription" UniqueName="ProfileDescription"
                HeaderText="Profile" HeaderTooltip="Profile" HeaderStyle-Wrap="false" ItemStyle-Wrap="true"
                HeaderStyle-Width="120px" ItemStyle-Width="120px" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource25">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center" Wrap="False" Width="90px"></HeaderStyle>

                <ItemStyle Wrap="False" Width="90px"></ItemStyle>
            </as:ASGridBoundColumn>
            <as:ASGridBoundColumn DataField="SICCode" UniqueName="SICCode" HeaderText="SIC" HeaderTooltip="SIC Code"
                HeaderStyle-Wrap="false" ItemStyle-Wrap="false" HeaderStyle-Width="60px" ItemStyle-Width="60px"
                ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource26">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center" Wrap="False" Width="60px"></HeaderStyle>

                <ItemStyle Wrap="False" Width="60px"></ItemStyle>
            </as:ASGridBoundColumn>
            <as:ASGridBoundColumn DataField="RulesViolatedToday" UniqueName="RulesViolatedToday" ItemStyle-CssClass="ellipsis"
                HeaderText="PV" HeaderTooltip="Parameters Violated" HeaderStyle-Width="86px" ItemStyle-Width="86px"
                HeaderStyle-Wrap="false" ItemStyle-Wrap="false" ASFormat="DynamicString" meta:resourcekey="ASGridBoundColumnResource27">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center" Wrap="False" Width="86px"></HeaderStyle>

                <ItemStyle Wrap="False" Width="86px"></ItemStyle>
            </as:ASGridBoundColumn>
            <as:ASGridBoundColumn DataField="WQAssignmentName" UniqueName="WQAssigned" HeaderText="WQ Assigned"
                HeaderTooltip="WQ Assigned" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="left"
                HeaderStyle-Width="200px" HeaderStyle-Wrap="false" ItemStyle-Wrap="true" ItemStyle-Width="200px"
                ASFormat="DynamicString" meta:resourcekey="ASGridBoundColumnResource28">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center" Wrap="False" Width="200px"></HeaderStyle>

                <ItemStyle HorizontalAlign="Left" Wrap="True" Width="200px"></ItemStyle>
            </as:ASGridBoundColumn>
            <as:ASGridTemplateColumn DataField="CardView" UniqueName="CardView" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-CssClass="td-card-view">
                <ItemTemplate>
                    <div class="row card-view">
                        <div class="col-xs-3">
                            <div class="row w-min-180 mb-4x">
                                <div class="col-xs-6 rdHeader" id="colRQColumn" runat="server">
                                    <asp:Label ID="Label2" CssClass="rdHeader" runat="server">RQ</asp:Label>
                                    <div id="bgChkItemCV" runat="server">
                                        <input type="checkbox" id="chkItemCV" runat="server" title="Checkbox to assign" />
                                    </div>
                                </div>
                                <div class="col-xs-6" id="colMerchantName" runat="server">
                                    <asp:LinkButton ID="lbtSort_MerchantName" CssClass="rdHeader" runat="server" Text="Merchant Name" CommandName="Sort_MerchantName" meta:resourcekey="ASGridBoundColumnCardResource1"></asp:LinkButton>
                                    <span class="icon-sort"></span>
                                    <div runat="server" id="merchantName"><%# Eval("MerchantName") %></div>
                                </div>
                            </div>
                            <div class="row w-min-180 mb-4x">
                                <div class="col-xs-6">
                                    <asp:LinkButton ID="lbtSort_MerchantNumber" CssClass="rdHeader" runat="server" Text="Merchant Number" CommandName="Sort_MerchantNumber" meta:resourcekey="ASGridBoundColumnCardResource0"></asp:LinkButton>
                                    <div runat="server" id="BusinessAge"><%# Eval("MerchantNumber") %></div>
                                </div>
                                <div class="col-xs-6">
                                    <asp:LinkButton ID="lbtSort_SICCode" CssClass="rdHeader" runat="server" CommandName="Sort_SICCode" ToolTip="SIC" Text="SIC" meta:resourcekey="ASGridBoundColumnCardResource29"></asp:LinkButton>
                                    <span class="icon-sort"></span>
                                    <div runat="server" id="sicCode"><%# Eval("SICCode") %></div>
                                </div>
                            </div>
                            <div class="row w-min-180 mb-4x">
                                <div class="col-xs-6">
                                    <asp:LinkButton ID="lbtSort_ProfileDescription" CssClass="rdHeader" runat="server" CommandName="Sort_ProfileDescription" ToolTip="Profile" Text="Profile" meta:resourcekey="ASGridBoundColumnCardResource23"></asp:LinkButton>
                                    <span class="icon-sort"></span>
                                    <div id="Div22" runat="server"><%# Eval("ProfileDescription") %></div>
                                </div>
                            </div>
                        </div>
                        <div class="col-xs-9">
                            <div class="auto-grid">
                                <div class="item ">
                                    <asp:LinkButton ID="lbtSort_Worked" CssClass="rdHeader" runat="server" CommandName="Sort_Worked" Text="#WKD 30" ToolTip="Times worked in 30 days" meta:resourcekey="ASGridBoundColumnCardResource3"></asp:LinkButton>
                                    <span class="icon-sort"></span>
                                    <div id="volumePercent" runat="server"><%# Eval("Worked") %></div>
                                </div>
                                <div class="item ">
                                    <asp:LinkButton ID="lbtSort_DaysActive" CssClass="rdHeader" runat="server" CommandName="Sort_DaysActive" Text="DA" meta:resourcekey="ASGridBoundColumnCardResource2"></asp:LinkButton>
                                    <span class="icon-sort"></span>
                                    <div id="Div1" runat="server"><%# Eval("DaysActive") %></div>
                                </div>
                                <div class="item ">
                                    <asp:LinkButton ID="lbtSort_DaysProcessedLast90" CssClass="rdHeader" runat="server" CommandName="Sort_DaysProcessedLast90" ToolTip="Days Processed Last 90" Text="DP" meta:resourcekey="ASGridBoundColumnCardResource4"></asp:LinkButton>
                                    <span class="icon-sort"></span>
                                    <div id="Div2" runat="server"><%# Eval("DaysProcessedLast90") %></div>
                                </div>
                                <div class="item ">
                                    <asp:LinkButton ID="lbtSort_TransactionCountLast60" CssClass="rdHeader" runat="server" CommandName="Sort_TransactionCountLast60" Text="# Trans" ToolTip="Transaction Count Last 60" meta:resourcekey="ASGridBoundColumnCardResource5"></asp:LinkButton>
                                    <span class="icon-sort"></span>
                                    <div id="Div3" runat="server"><%# FormatInteger(Eval("TransactionCountLast60")) %></div>
                                </div>
                                <div class="item ">
                                    <asp:LinkButton ID="lbtSort_ForcedViolation" CssClass="rdHeader" runat="server" CommandName="Sort_ForcedViolation" Text="FV" ToolTip="Forced Violation" meta:resourcekey="ASGridBoundColumnCardResource6"></asp:LinkButton>
                                    <span class="icon-sort"></span>
                                    <div id="Div4" runat="server"><%# Eval("ForcedViolation") %></div>
                                </div>
                                <div class="item ">
                                    <asp:LinkButton ID="lbtSort_TodayUnmatchedCreditsVolume" CssClass="rdHeader" runat="server" CommandName="Sort_TodayUnmatchedCreditsVolume" Text="UC" ToolTip="Today’s Unmatched Credits Volume" meta:resourcekey="ASGridBoundColumnCardResource7"></asp:LinkButton>
                                    <span class="icon-sort"></span>
                                    <div id="Div5" runat="server"><%# FormatCurrency(Eval("TodayUnmatchedCreditsVolume")) %></div>
                                </div>
                                <div class="item ">
                                    <asp:LinkButton ID="lbtSort_TodayHighestTicket" CssClass="rdHeader" runat="server" CommandName="Sort_TodayHighestTicket" Text="HT" ToolTip="Today's Highest Ticket" meta:resourcekey="ASGridBoundColumnCardResource8"></asp:LinkButton>
                                    <span class="icon-sort"></span>
                                    <div id="Div6" runat="server"><%# FormatCurrency(Eval("TodayHighestTicket")) %></div>
                                </div>
                                <div class="item ">
                                    <asp:LinkButton ID="lbtSort_ExcessAboveContractHighest" CssClass="rdHeader" runat="server" CommandName="Sort_ExcessAboveContractHighest" Text="CH" ToolTip="Excess Above Contract Highest" meta:resourcekey="ASGridBoundColumnCardResource27"></asp:LinkButton>
                                    <span class="icon-sort"></span>
                                    <div id="Div7" runat="server"><%# FormatCurrency(Eval("ExcessAboveContractHighest")) %></div>
                                </div>
                                <div class="item ">
                                    <asp:LinkButton ID="lbtSort_TodayAverageTicket" CssClass="rdHeader" runat="server" CommandName="Sort_TodayAverageTicket" Text="AT" ToolTip="Today's Average Ticket" meta:resourcekey="ASGridBoundColumnCardResource9"></asp:LinkButton>
                                    <span class="icon-sort"></span>
                                    <div id="Div8" runat="server"><%# FormatCurrency(Eval("TodayAverageTicket")) %></div>
                                </div>
                                <div class="item ">
                                    <asp:LinkButton ID="lbtSor_ExcessAboveContractAverage" CssClass="rdHeader" runat="server" CommandName="Sort_ExcessAboveContractAverage" Text="CA" meta:resourcekey="ASGridBoundColumnCardResource10"></asp:LinkButton>
                                    <span class="icon-sort"></span>
                                    <div id="Div9" runat="server"><%# FormatCurrency(Eval("ExcessAboveContractAverage")) %></div>
                                </div>
                                <div class="item ">
                                    <asp:LinkButton ID="lbtSort_KeyedPercent" CssClass="rdHeader" runat="server" CommandName="Sort_KeyedPercent" Text="K%" ToolTip="Keyed Percent" meta:resourcekey="ASGridBoundColumnCardResource11"></asp:LinkButton>
                                    <span class="icon-sort"></span>
                                    <div id="Div10" runat="server"><%# FormatPercent(Eval("KeyedPercent")) %></div>
                                </div>
                                <div class="item ">
                                    <asp:LinkButton ID="lbtSort_ExcessAboveKeyedPercent" CssClass="rdHeader" runat="server" CommandName="Sort_ExcessAboveKeyedPercent" Text="EK%" ToolTip="Excess Above Keyed Percent" meta:resourcekey="ASGridBoundColumnCardResource12"></asp:LinkButton>
                                    <span class="icon-sort"></span>
                                    <div id="Div11" runat="server"><%# FormatPercent(Eval("ExcessAboveKeyedPercent")) %></div>
                                </div>
                                <div class="item ">
                                    <asp:LinkButton ID="lbtSort_TodayTotalVolume" CssClass="rdHeader" runat="server" CommandName="Sort_TodayTotalVolume" Text="TV" ToolTip="Today's Total Volume" meta:resourcekey="ASGridBoundColumnCardResource13"></asp:LinkButton>
                                    <span class="icon-sort"></span>
                                    <div id="Div12" runat="server"><%# FormatCurrency(Eval("TodayTotalVolume")) %></div>
                                </div>
                                <div class="item ">
                                    <asp:LinkButton ID="lbtSort_TodayBankCardSales" CssClass="rdHeader" runat="server" CommandName="Sort_TodayBankCardSales" ToolTip="Today's Bank Card Sales" Text="BS" meta:resourcekey="ASGridBoundColumnCardResource14"></asp:LinkButton>
                                    <span class="icon-sort"></span>
                                    <div id="Div13" runat="server"><%# FormatInteger(Eval("TodayBankCardSales")) %></div>
                                </div>
                                <div class="item ">
                                    <asp:LinkButton ID="lbtSort_ExcessAboveDailyContractVolume" CssClass="rdHeader" runat="server" CommandName="Sort_ExcessAboveDailyContractVolume" ToolTip="Excess Above Daily Contract Volume (Annual / 365)" Text="DC" meta:resourcekey="ASGridBoundColumnCardResource15"></asp:LinkButton>
                                    <span class="icon-sort"></span>
                                    <div id="Div14" runat="server"><%# FormatCurrency(Eval("ExcessAboveDailyContractVolume")) %></div>
                                </div>
                                <div class="item ">
                                    <asp:LinkButton ID="lbtSort_P4VolumeIncreasePercent" CssClass="rdHeader" runat="server" CommandName="Sort_P4VolumeIncreasePercent" ToolTip="P4 Volume Increase Percent (90-day calculation)" Text="P4 V%" meta:resourcekey="ASGridBoundColumnCardResource16"></asp:LinkButton>
                                    <span class="icon-sort"></span>
                                    <div id="Div15" runat="server"><%# FormatPercent(Eval("P4VolumeIncreasePercent")) %></div>
                                </div>
                                <div class="item ">
                                    <asp:LinkButton ID="lbtSort_MV1" CssClass="rdHeader" runat="server" CommandName="Sort_MV1" ToolTip="MV1 (Monthly volume previous month 1)" Text="MV1" meta:resourcekey="ASGridBoundColumnCardResource17"></asp:LinkButton>
                                    <span class="icon-sort"></span>
                                    <div id="Div16" runat="server"><%# FormatCurrency(Eval("MV1")) %></div>
                                </div>
                                <div class="item ">
                                    <asp:LinkButton ID="lbtSort_MV2" CssClass="rdHeader" runat="server" CommandName="Sort_MV2" ToolTip="MV2 (Monthly volume previous month 2)" Text="MV2" meta:resourcekey="ASGridBoundColumnCardResource18"></asp:LinkButton>
                                    <span class="icon-sort"></span>
                                    <div id="Div17" runat="server"><%# FormatCurrency(Eval("MV2")) %></div>
                                </div>
                                <div class="item ">
                                    <asp:LinkButton ID="lbtSort_MV3" CssClass="rdHeader" runat="server" CommandName="Sort_MV3" ToolTip="MV3 (Monthly volume previous month 3)" Text="MV3" meta:resourcekey="ASGridBoundColumnCardResource19"></asp:LinkButton>
                                    <span class="icon-sort"></span>
                                    <div id="Div18" runat="server"><%# FormatCurrency(Eval("MV3")) %></div>
                                </div>
                                <div class="item ">
                                    <asp:LinkButton ID="lbtSort_Zip3" CssClass="rdHeader" runat="server" CommandName="Sort_Zip3" ToolTip="Zip-3" Text="Zip-3" meta:resourcekey="ASGridBoundColumnCardResource20"></asp:LinkButton>
                                    <span class="icon-sort"></span>
                                    <div id="Div19" runat="server"><%# Eval("Zip3") %></div>
                                </div>
                                <div class="item ">
                                    <asp:LinkButton ID="lbtSort_State" CssClass="rdHeader" runat="server" CommandName="Sort_State" ToolTip="State" Text="State" meta:resourcekey="ASGridBoundColumnCardResource21"></asp:LinkButton>
                                    <span class="icon-sort"></span>
                                    <div id="Div20" runat="server"><%# Eval("State") %></div>
                                </div>
                                <div class="item ">
                                    <asp:LinkButton ID="lbtSort_SaleGroup" CssClass="rdHeader" runat="server" CommandName="Sort_SaleGroup" ToolTip="Sale Group" Text="SG" meta:resourcekey="ASGridBoundColumnCardResource22"></asp:LinkButton>
                                    <span class="icon-sort"></span>
                                    <div id="Div21" runat="server"><%# Eval("SaleGroup") %></div>
                                </div>
                                <div class="item ">
                                    <asp:LinkButton ID="lbtSort_RulesViolatedToday" CssClass="rdHeader" runat="server" CommandName="Sort_RulesViolatedToday" ToolTip="Parameters Violated" Text="PV" meta:resourcekey="ASGridBoundColumnCardResource25"></asp:LinkButton>
                                    <span class="icon-sort"></span>
                                    <div id="Div24" runat="server"><%# Eval("RulesViolatedToday") %></div>
                                </div>
                                <div class="item ">
                                    <asp:LinkButton ID="lbtSort_WQAssignmentName" CssClass="rdHeader" runat="server" CommandName="Sort_WQAssignmentName" ToolTip="WQ Assigned" Text="WQ Assigned" meta:resourcekey="ASGridBoundColumnCardResource26"></asp:LinkButton>
                                    <span class="icon-sort"></span>
                                    <div id="Div25" runat="server"><%# Eval("WQAssignmentName") %></div>
                                </div>

                            </div>
                        </div>
                    </div>
                </ItemTemplate>
            </as:ASGridTemplateColumn>
        </Columns>
    </MasterTableView>
</as:ASGrid>


<as:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <script type="text/javascript" src="<%=ResolveUrl("~") %>res/js/risk_MCF/rm_MCF_DQDistinctReport.js"></script>
</as:RadCodeBlock>
