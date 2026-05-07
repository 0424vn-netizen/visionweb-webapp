<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Risk_G2MoneyLaundering_Filter.ascx.cs" Inherits="UserControls_Risk_G2MoneyLaundering_Filter" %>
<div class="row collapse report-filter-panel">
    <div class="col-md-12 report-filter risk-mgmt-report-filter">
        <div class="filter-block">
            <div class="filter-block">
                <table>
                    <as:Panel ID="uxPnlDateCriteria" runat="server">
                        <tr>
                            <td class="text-right">
                                <div class="filter-item">
                                    <label class="filter-item">
                                        <asp:Literal ID="Literal2" runat="server" Text="Date Range:" meta:resourcekey="uxDateRangeResource1" />
                                    </label>
                                </div>
                            </td>
                            <td class="text-left">
                                <div id="divDateRange" runat="server">
                                    <div class="filter-item">
                                        <as:RadDatePicker ID="uxDateRangeFrom" runat="server" ShowPopupOnFocus="true" meta:resourcekey="uxCalendaPopupResource1">
                                            <Calendar FastNavigationStep="12" ShowRowHeaders="false">
                                            </Calendar>
                                            <DateInput ID="DateInput2" runat="server" onclick="ShowCalendar('1')" />
                                        </as:RadDatePicker>
                                    </div>
                                    <div class="filter-item">
                                        <as:RadDatePicker ID="uxDateRangeTo" runat="server" ShowPopupOnFocus="true" meta:resourcekey="uxCalendaPopupResource1">
                                            <Calendar FastNavigationStep="12" ShowRowHeaders="false">
                                            </Calendar>
                                            <DateInput ID="DateInput3" runat="server" onclick="ShowCalendar('2')" />
                                        </as:RadDatePicker>
                                    </div>
                                </div>
                            </td>
                            <td class="text-center">
                                <div class="filter-item">
                                    <asp:Button ID="uxSearch" runat="server" CssClass="btn btn-default risk-mgmt-btn" Text="Create Report"
                                        OnClientClick="return ValidateData();" OnClick="uxSearch_Click" meta:resourcekey="uxSearchResource1"/>
                                </div>
                            </td>
                        </tr>
                    </as:Panel>
                </table>
            </div>
            <as:Validator ID="uxValidator" runat="server" ValidationFunction="ValidateDate" MessageType="AlertBox" meta:resourcekey="uxValidatorResource1">
                <Items>
                    <as:CustomValidationItem ControlToValidateID="uxDateRangeFrom" ResMessage="Resources.MessageManager.Generic_InvalidDateFormat" ClientValidationFunction="FromIsDate" />
                    <as:CustomValidationItem ControlToValidateID="uxDateRangeFrom" ResMessage="Resources.MessageManager.ReportFilter_V10" ClientValidationFunction="FromDate_GreaterThanToDay" />
                    <as:CustomValidationItem ControlToValidateID="uxDateRangeTo" ResMessage="Resources.MessageManager.Generic_InvalidDateFormat" ClientValidationFunction="EndIsDate" />
                    <as:CustomValidationItem ControlToValidateID="uxDateRangeTo" ResMessage="Resources.MessageManager.ReportFilter_V10" ClientValidationFunction="EndDate_GreaterThanToDay" />
                    <as:CustomValidationItem ControlToValidateID="uxDateRangeTo" ResMessage="Resources.MessageManager.Generic_InvalidDateFormat_FromDateGreaterToDate" ClientValidationFunction="EndDate_GreaterThanFromDate" />
                </Items>
            </as:Validator>
        </div>
    </div>
</div>
<div class="row">
    <div class="col-md-12 text-center" data-target=".report-filter-panel" data-toggle="collapse">
        <span class="btn btn-link btn-report-filter">
            <asp:Literal ID="Literal6" runat="server" Text="FILTER" meta:resourcekey="LiteralFilterResource" /></span>
    </div>
</div>
<tek:RadCodeBlock ID="uxRadCodeBlock" runat="server">
    <script type="text/javascript">
        var RiskMgmtReporting_uxDateRangeFrom = "<%=uxDateRangeFrom.ClientID %>";
        var RiskMgmtReporting_uxDateRangeTo = "<%=uxDateRangeTo.ClientID %>";
        var RiskMgmtReporting_searchBtn = "<%=uxSearch.ClientID%>";  
        var funcValidate = 'ValidateData';
    </script>
    <script type="text/javascript" src="<%=ResolveUrl("~/") %>res/js/risk/RiskG2MoneyLaundering.js"></script>
</tek:RadCodeBlock>

