<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ServiceTestFilter.ascx.cs" Inherits="UserControls_ServiceTestFilter" %>

<div class="row collapse report-filter-panel">
    <div class="col-md-12">
        <div class="report-filter-default">
            <div class="filter-left">
                <div style="font-size:0">
                    <as:RadioButton onkeypress="javascript:return false;" ID="uxDaily" runat="server"
                        Text="Daily" GroupName="Date" onclick="ChangeDateOption(this)"
                        CssClass="date_item" Value="" meta:resourcekey="uxDailyResource1" />
                    <as:RadioButton onkeypress="javascript:return false;" ID="uxMonthly" runat="server"
                        Text="Monthly" GroupName="Date" onclick="ChangeDateOption(this)"
                        CssClass="date_item" Value="" meta:resourcekey="uxMonthlyResource1" />
                    <as:RadioButton onkeypress="javascript:return false;" ID="uxDateRange" runat="server"
                        Text="Date Range" GroupName="Date" onclick="ChangeDateOption(this)"
                        CssClass="date_item" Value="" meta:resourcekey="uxDateRangeResource1" />
                </div>
                <div>
                    <as:ASRadComboBox ID="uxRCBMerchant" runat="server">
                        <Items>
                            <as:ASRadComboBoxItem Value="MERCHANTNUMBER" Text="Merchant ID(Full)" meta:resourcekey="ASRadComboBoxItemResource2" />
                        </Items>
                    </as:ASRadComboBox>
                </div>
            </div>
            <div class="filter-right">
                <div style="font-size:0">
                    <div id="divDate" runat="server">
                        <as:RadDatePicker ID="uxDate" runat="server" ShowPopupOnFocus="true" meta:resourcekey="uxCalendaPopupResource1">
                            <Calendar FastNavigationStep="12" ShowRowHeaders="false">
                            </Calendar>
                            <DateInput ID="DateInput1" runat="server" onclick="ShowCalendar('1')" />
                        </as:RadDatePicker>
                    </div>
                    <div id="divDateRange" runat="server" class="display-none">
                        <as:RadDatePicker ID="uxDateRangeFrom" runat="server" ShowPopupOnFocus="true" meta:resourcekey="uxCalendaPopupResource1">
                            <Calendar FastNavigationStep="12" ShowRowHeaders="false">
                            </Calendar>
                            <DateInput ID="DateInput2" runat="server" onclick="ShowCalendar('2')" />
                        </as:RadDatePicker>
                        <as:RadDatePicker ID="uxDateRangeTo" runat="server" ShowPopupOnFocus="true" meta:resourcekey="uxCalendaPopupResource1">
                            <Calendar FastNavigationStep="12" ShowRowHeaders="false">
                            </Calendar>
                            <DateInput ID="DateInput3" runat="server" onclick="ShowCalendar('3')" />
                        </as:RadDatePicker>
                    </div>
                </div>
                <div>
                    <asp:TextBox ID="uxSearchValue" CssClass="rf_TextBox" runat="server" MaxLength="16" meta:resourcekey="uxSearchValueResource1"></asp:TextBox>
                    <asp:Button ID="uxSearch" runat="server" CssClass="btn btn-default risk-mgmt-btn" Text="Search"
                        OnClientClick="return ValidateData();" OnClick="uxSearch_Click" meta:resourcekey="uxSearchResource1" />
                </div>
            </div>
        </div>

        <as:HiddenField ID="haveMerchant" Value="1" runat="server" />
        <as:HiddenField ID="haveDate" Value="1" runat="server" />
        <as:Validator ID="uxValidator" runat="server" ValidationFunction="ValidateDate" MessageType="AlertBox" meta:resourcekey="uxValidatorResource1">
            <Items>
                <as:CustomValidationItem ControlToValidateID="uxDate" ResMessage="Resources.MessageManager.Generic_InvalidDateFormat" ClientValidationFunction="StartIsDate" />
                <as:CustomValidationItem ControlToValidateID="uxDate" ResMessage="Resources.MessageManager.ReportFilter_V10" ClientValidationFunction="StartDate_GreaterThanToDay" />

                <as:CustomValidationItem ControlToValidateID="uxDateRangeFrom" ResMessage="Resources.MessageManager.Generic_InvalidDateFormat" ClientValidationFunction="FromIsDate" />
                <as:CustomValidationItem ControlToValidateID="uxDateRangeFrom" ResMessage="Resources.MessageManager.ReportFilter_V10" ClientValidationFunction="FromDate_GreaterThanToDay" />

                <as:CustomValidationItem ControlToValidateID="uxDateRangeTo" ResMessage="Resources.MessageManager.Generic_InvalidDateFormat" ClientValidationFunction="EndIsDate" />
                <as:CustomValidationItem ControlToValidateID="uxDateRangeTo" ResMessage="Resources.MessageManager.ReportFilter_V10" ClientValidationFunction="EndDate_GreaterThanToDay" />
                <as:CustomValidationItem ControlToValidateID="uxDateRangeTo" ResMessage="Resources.MessageManager.Generic_InvalidDateFormat_FromDateGreaterToDate" ClientValidationFunction="EndDate_GreaterThanFromDate" />
            </Items>
        </as:Validator>
        <as:Validator ID="uxValidator2" runat="server" ValidationFunction="ValidateMerchant" MessageType="AlertBox" meta:resourcekey="uxValidator2Resource1">
            <Items>
                <as:BasicValidationItem ControlToValidateID="uxSearchValue" Rule="Required" ResMessage="Resources.MessageManager.ReportFilter_V2" meta:resourcekey="BasicValidator1Resource1Resource1" />
                <as:RegExValidationItem ControlToValidateID="uxSearchValue" RegularExpression="^[0-9]{0,16}$" ResMessage="Resources.MessageManager.ValidationMessages_V7" meta:resourcekey="RegExValidation1Resource1Resource1" />
            </Items>
        </as:Validator>
    </div>
</div>
<div class="row">
    <div class="col-md-12 text-center" data-target=".report-filter-panel" data-toggle="collapse">
        <span class="btn btn-link btn-report-filter">
            <asp:Literal ID="Literal6" runat="server" Text="FILTER" meta:resourcekey="Literal6Resource1" /></span>
    </div>
</div>
<tek:RadCodeBlock ID="uxRadCodeBlock" runat="server">

    <script type="text/javascript">
        var RiskMgmtReporting_uxDaily = "<%=uxDaily.ClientID %>";
        var RiskMgmtReporting_uxMonthly = "<%=uxMonthly.ClientID %>";
        var RiskMgmtReporting_uxDateRange = "<%=uxDateRange.ClientID %>";
        var RiskMgmtReporting_uxDate = "<%=uxDate.ClientID %>";
        var RiskMgmtReporting_uxDateRangeFrom = "<%=uxDateRangeFrom.ClientID %>";
        var RiskMgmtReporting_uxDateRangeTo = "<%=uxDateRangeTo.ClientID %>";
        var RiskMgmtReporting_uxCboMerchant = "<%=uxRCBMerchant.ClientID%>";
        var RiskMgmtReporting_uxSearchValue = "<%=uxSearchValue.ClientID %>";
        var RiskMgmtReporting_divDate = "<%=divDate.ClientID%>";
        var RiskMgmtReporting_divDateRange = "<%=divDateRange.ClientID%>";
        var RiskMgmtReporting_searchBtn = "<%=uxSearch.ClientID%>";
        var RiskhaveMerchant_ClientId = "<%=haveMerchant.ClientID%>";
        var RiskhaveDate_ClientId = "<%=haveDate.ClientID%>";

    </script>
    <script type="text/javascript" src="<%=ResolveUrl("~/") %>res/js/ServiceTestFilter.js"></script>

</tek:RadCodeBlock>
