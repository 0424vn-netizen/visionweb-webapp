<%@ Page Title="Merchant Detail" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="rm_MCF_MgmtReport_MerchantDetail.aspx.cs" Inherits="rm_MCF_MgmtReport_MerchantDetail" meta:resourcekey="PageResource1" %>

<%@ Register Src="~/UserControls/rm_MCF_MgmtReport_ReportFilter.ascx" TagName="ReportFilter"
    TagPrefix="uc" %>
<%@ Register Src="~/UserControls/PageTitle.ascx" TagName="PageTitle" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/UxExport.ascx" TagName="UxExport" TagPrefix="uc" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="Server">
    <tek:RadAjaxManagerProxy runat="server" ID="uxRadAjaxManager">
        <AjaxSettings>
            <tek:AjaxSetting AjaxControlID="uxReportGrid">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="uxReportGrid" LoadingPanelID="uxLoadingPanelCustom" />
                </UpdatedControls>
            </tek:AjaxSetting>
        </AjaxSettings>
    </tek:RadAjaxManagerProxy>
    <as:PlaceHolder ID="sdff" runat="server">
        <uc:ReportFilter ID="uxReportFilter" runat="server" OnSubmitFiltering="OnSearchEvent"
            VisibleAssignmentFilter="true" VisibleParameterFilter="true" VisibleMerchantFilter="true"></uc:ReportFilter>
        <%--<uc:PageTitle ID="uxPageTitle" runat="server" ReportTitle="Risk Management - Reports - Merchant Detail" />--%>

        <uc:UxExport ID="uxExporter" runat="server" GridID="uxReportGrid" ShowPDF="false"
            ShowWord="false" IsOnTop="true" />
        <as:ASGrid ID="uxReportGrid" runat="server" GridLines="None" AutoGenerateColumns="False"  HeaderStyle-Width="80px"
            AllowSorting="True" AllowPaging="True" ASPagingMethod="SPASingleMethod" CssClass="in" 
            meta:resourcekey="uxReportGridResource1" AllowExportAtWebServices="false" IsAutoExportTemplate="true">
            <MasterTableView>
                <Columns>
                    <as:ASGridBoundColumn SortExpression="ReportDate" UniqueName="ReportDate" HeaderText="Report Date"
                        HeaderTooltip="Report Date" DataField="ReportDate" ASFormat="Date" HeaderStyle-Width="100px" meta:resourcekey="ASGridBoundColumnResource1">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center" Width="100px"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn SortExpression="MerchantNumber" UniqueName="MerchantNumberEx"
                        HeaderText="Merchant ID" HeaderTooltip="Merchant ID" DataField="MerchantNumber"
                        ASFormat="StaticString" Visible="false" Display="false"
                        HeaderStyle-Width="140px" meta:resourcekey="ASGridBoundColumnResource2">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center" Width="140px"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridTemplateColumn SortExpression="MerchantNumber" UniqueName="MerchantNumber"
                        HeaderText="Merchant ID" HeaderTooltip="Merchant ID" ItemStyle-HorizontalAlign="Center"
                        HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="140px" meta:resourcekey="ASGridTemplateColumnResource1">
                        <ItemTemplate>
                            <as:LinkButton runat="server" CommandArgument='<%# Eval("MerchantNumber") %>'
                                OnCommand="uxMerchantName_Command" Text='<%# Eval("MerchantNumber") %>' CommandName="MerchantNumberClick" meta:resourcekey="LinkButtonResource1"></as:LinkButton>
                        </ItemTemplate>

                        <HeaderStyle HorizontalAlign="Center" Width="140px"></HeaderStyle>

                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </as:ASGridTemplateColumn>
                    <as:ASGridBoundColumn SortExpression="MerchantName" UniqueName="MerchantNameEx" ASFormat="DynamicString"
                        HeaderText="Merchant Name" HeaderTooltip="Merchant Name" DataField="MerchantName"
                        Visible="false" Display="false" meta:resourcekey="ASGridBoundColumnResource3">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridTemplateColumn SortExpression="MerchantName" UniqueName="MerchantName"  HeaderStyle-Width="150px"
                        HeaderText="Merchant Name" HeaderTooltip="Merchant Name" ItemStyle-CssClass="word-break"
                        HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left" meta:resourcekey="ASGridTemplateColumnResource2">
                        <ItemTemplate>
                            <asp:LinkButton runat="server" CommandArgument='<%# Eval("MerchantNumber") %>'
                                OnCommand="uxMerchantName_Command" Text='<%# Eval("MerchantName") %>' CommandName="MerchantNameClick" meta:resourcekey="LinkButtonResource2"></asp:LinkButton>
                        </ItemTemplate>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                    </as:ASGridTemplateColumn>
                    <as:ASGridBoundColumn SortExpression="AssignmentName" UniqueName="AssignmentName"
                        ASFormat="DynamicString"  HeaderStyle-Width="160px" ItemStyle-CssClass="word-break"
                        HeaderText="Assignment Name" HeaderTooltip="Assignment Name" DataField="AssignmentName" meta:resourcekey="ASGridBoundColumnResource4">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn SortExpression="ParameterKey" UniqueName="ParameterKey" HeaderText="P #"
                        HeaderTooltip="Parameter Key" DataField="ParameterKey" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource5">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn SortExpression="ParameterName" UniqueName="ParameterName"  HeaderStyle-Width="150px"
                        HeaderText="Parameter Name" HeaderTooltip="Parameter Name" ASFormat="DynamicString" ItemStyle-CssClass="word-break"
                        DataField="ParameterName" meta:resourcekey="ASGridBoundColumnResource6">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn SortExpression="ParameterIndicator" UniqueName="ParameterIndicator"
                        HeaderText="P %/#/$" HeaderTooltip="P %/#/$" DataField="ParameterIndicator" meta:resourcekey="ASGridBoundColumnResource7">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <%--[44432]: Bug #37842--%>
                    <as:ASGridBoundColumn SortExpression="ParameterIndicatorText" UniqueName="ParameterIndicatorText"
                        HeaderText="P %/#/$" HeaderTooltip="P %/#/$" ASFormat="StaticString"
                        DataField="ParameterIndicatorText" meta:resourcekey="ASGridBoundColumnResource7"
                        Visible="false">
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn SortExpression="ParameterIndicatorText" UniqueName="ParameterIndicatorTextCSV"
                        HeaderText="P %/#/$" HeaderTooltip="P %/#/$" ASFormat="StaticString"
                        DataField="ParameterIndicatorTextCSV" meta:resourcekey="ASGridBoundColumnResource7"
                        Visible="false">
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn SortExpression="ParameterIndicator_ActualValue" UniqueName="ActualValue"
                        HeaderText="Actual Value" HeaderTooltip="Actual Value" DataField="ParameterIndicator_ActualValue" 
                        meta:resourcekey="ASGridBoundColumnResource8">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <%--[44432]: Bug #37842--%>
                    <as:ASGridBoundColumn SortExpression="ActualValueText" UniqueName="ActualValueText"
                        HeaderText="Actual Value" HeaderTooltip="Actual Value" ASFormat="StaticString"
                        DataField="ActualValueText" meta:resourcekey="ASGridBoundColumnResource8"
                        Visible="false">
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn SortExpression="ActualValueText" UniqueName="ActualValueTextCSV"
                        HeaderText="Actual Value" HeaderTooltip="Actual Value" ASFormat="StaticString"
                        DataField="ActualValueTextCSV" meta:resourcekey="ASGridBoundColumnResource8"
                        Visible="false">
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn SortExpression="ParameterThreshold" UniqueName="ParameterThreshold"
                        ASFormat="StaticString" HeaderText="P TH" HeaderTooltip="Parameter Threshold"
                        DataField="ParameterThreshold" meta:resourcekey="ASGridBoundColumnResource9">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <%--[44432]: Bug #37842--%>
                    <as:ASGridBoundColumn SortExpression="ParameterThresholdText" UniqueName="ParameterThresholdText"
                        HeaderText="P TH" HeaderTooltip="P TH" ASFormat="StaticString"
                        DataField="ParameterThresholdText" meta:resourcekey="ASGridBoundColumnResource9"
                        Visible="false">
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn SortExpression="ParameterThreshold_ActualValue" UniqueName="ActualThreshold"
                        HeaderText="Actual TH" HeaderTooltip="Actual Threshold" ASFormat="StaticString"
                        DataField="ParameterThreshold_ActualValue" meta:resourcekey="ASGridBoundColumnResource10"  HeaderStyle-Width="100px">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <%-- 10/10/2018 |41044 PIVOT New Parameter--%>
                    <as:ASGridBoundColumn SortExpression="ActualThresholdText" UniqueName="ActualThresholdText"
                        HeaderText="Actual TH" HeaderTooltip="Actual Threshold" ASFormat="StaticString"
                        DataField="ActualThresholdText" meta:resourcekey="ASGridBoundColumnResource10"
                        Visible="false">
                    </as:ASGridBoundColumn>

                    <as:ASGridBoundColumn SortExpression="Volume" UniqueName="Volume" ASFormat="Currency"
                        HeaderText="Volume" HeaderTooltip="Volume" DataField="Volume" meta:resourcekey="ASGridBoundColumnResource11">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn SortExpression="AvgTicket" UniqueName="AvgTicket" ASFormat="Currency"
                        HeaderText="Avg Ticket" HeaderTooltip="Average Ticket" DataField="AvgTicket" meta:resourcekey="ASGridBoundColumnResource12">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn SortExpression="HighTransactionAmount" UniqueName="HighTransactionAmount"
                        ASFormat="Currency" HeaderText="High Trans Amount" HeaderTooltip="High Transaction Amount"
                        DataField="HighTransactionAmount" meta:resourcekey="ASGridBoundColumnResource13">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn SortExpression="RiskScore" UniqueName="RiskScore" ASFormat="Integer"
                        HeaderText="Risk Score" HeaderTooltip="Risk Score" DataField="RiskScore" meta:resourcekey="ASGridBoundColumnResource14">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn SortExpression="IsWorked" UniqueName="Worked" ASFormat="StaticString"
                        HeaderText="Wrk" HeaderTooltip="Worked" DataField="IsWorked" meta:resourcekey="ASGridBoundColumnResource15">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn SortExpression="StatusDesc" UniqueName="Status" HeaderText="Status"
                        HeaderTooltip="Status" ASFormat="DynamicString" DataField="StatusDesc" meta:resourcekey="ASGridBoundColumnResource16">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                </Columns>
            </MasterTableView>
        </as:ASGrid>
    </as:PlaceHolder>
</asp:Content>

