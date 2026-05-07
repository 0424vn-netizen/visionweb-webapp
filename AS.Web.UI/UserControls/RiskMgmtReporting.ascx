<%@ Control Language="C#" AutoEventWireup="true" CodeFile="RiskMgmtReporting.ascx.cs" Inherits="UserControls_RiskMgmtReporting" %>

<%--<tek:RadAjaxManagerProxy runat="server" ID="uxRadAjaxManager">
    <AjaxSettings>
        <tek:AjaxSetting AjaxControlID="uxRCBReportType">
            <UpdatedControls>                
                <tek:AjaxUpdatedControl ControlID="uxPnlMerchantCriteria"/>
                <tek:AjaxUpdatedControl ControlID="uxPnlDateCriteria" LoadingPanelID="uxLoadingPanelCustom" />
            </UpdatedControls>
        </tek:AjaxSetting>
        <tek:AjaxSetting AjaxControlID="uxPnlMerchantCriteria">
            <UpdatedControls>                
                <tek:AjaxUpdatedControl ControlID="uxPnlMerchantCriteria"/>
            </UpdatedControls>
        </tek:AjaxSetting>
    </AjaxSettings>
</tek:RadAjaxManagerProxy>--%>

<div class="row collapse report-filter-panel">
    <div class="col-md-12 report-filter risk-mgmt-report-filter">
        <div class="filter-block">
            <div class="filter-block">
                <table>
                    <%--42907 - REORDER FILTER FOR EXTRACTS REPORT--%>
                    <tr>
                        <td class="text-right">
                            <label class="filter-item">
                                <asp:Literal ID="Literal1" runat="server" Text="Report Type:" meta:resourcekey="Literal1Resource1" /></label>
                        </td>
                        <td class="text-left">
                            <div class="filter-item">
                                <as:ASRadComboBox ID="uxRCBReportType" CssClass="mr-0" runat="server" Style="width: 333px" OnClientSelectedIndexChanged="uxRCBReportType_ClientSelectedIndexChanged">
                                </as:ASRadComboBox>
                            </div>
                        </td>
                    </tr>
                    <tr id="uxPnlDateCriteria" runat="server">
                        <td class="text-right">
                            <div class="filter-item">
                                <as:RadioButton onkeypress="javascript:return false;" ID="uxDaily" runat="server"
                                    Text="Daily" GroupName="Date" onclick="ChangeDateOption(this)"
                                    CssClass="date-item first daily-js" Value="" meta:resourcekey="uxDailyResource1" />
                                <as:RadioButton onkeypress="javascript:return false;" ID="uxMonthly" runat="server"
                                    Text="Monthly" GroupName="Date" onclick="ChangeDateOption(this)"
                                    CssClass="date-item month-js" Value="" meta:resourcekey="uxMonthlyResource1" />
                                <as:RadioButton onkeypress="javascript:return false;" ID="uxDateRange" runat="server"
                                    Text="Date Range" GroupName="Date" onclick="ChangeDateOption(this)"
                                    CssClass="date-item daterange-js" Value="" meta:resourcekey="uxDateRangeResource1" />
                            </div>
                        </td>
                        <td class="text-left">
                            <div id="divDate" runat="server">
                                <div class="filter-item">
                                    <%--42907 - REORDER FILTER FOR EXTRACTS REPORT--%>
                                    <as:RadDatePicker ID="uxDate" runat="server" ShowPopupOnFocus="true" meta:resourcekey="uxCalendaPopupResource1" CssClass="filter-date">
                                        <Calendar FastNavigationStep="12" ShowRowHeaders="false">
                                        </Calendar>
                                        <DateInput ID="DateInput1" runat="server" onclick="ShowCalendar('1')" />
                                    </as:RadDatePicker>
                                </div>
                            </div>
                            <div id="divDateRange" runat="server" class="display-none">
                                <div class="filter-item">
                                    <%--42907 - REORDER FILTER FOR EXTRACTS REPORT--%>
                                    <as:RadDatePicker ID="uxDateRangeFrom" runat="server" ShowPopupOnFocus="true" meta:resourcekey="uxCalendaPopupResource1" CssClass="filter-date">
                                        <Calendar FastNavigationStep="12" ShowRowHeaders="false">
                                        </Calendar>
                                        <DateInput ID="DateInput2" runat="server" onclick="ShowCalendar('2')" />
                                    </as:RadDatePicker>
                                </div>
                                <div class="filter-item" style="padding-left:4px">
                                    <%--42907 - REORDER FILTER FOR EXTRACTS REPORT--%>
                                    <as:RadDatePicker ID="uxDateRangeTo" runat="server" ShowPopupOnFocus="true" meta:resourcekey="uxCalendaPopupResource1" CssClass="filter-date">
                                        <Calendar FastNavigationStep="12" ShowRowHeaders="false">
                                        </Calendar>
                                        <DateInput ID="DateInput3" runat="server" onclick="ShowCalendar('3')" />
                                    </as:RadDatePicker>
                                </div>
                            </div>
                        </td>
                    </tr>

                    <tr id="uxPnlMerchantCriteria" runat="server">
                        <td class="text-right">
                            <label class="filter-item">
                                <asp:Literal ID="Literal2" runat="server" Text="Merchant:" meta:resourcekey="Literal2Resource1" /></label>
                        </td>
                        <td class="text-left">
                            <div class="filter-item">
                                <%--42907 - REORDER FILTER FOR EXTRACTS REPORT--%>
                                <as:ASRadComboBox ID="uxRCBMerchant" runat="server" OnClientSelectedIndexChanged="uxCboMerchant_ClientSelectedIndexChanged" Width="160px">
                                    <Items>
                                        <as:ASRadComboBoxItem Value="ALLMERCHANTS" Text="All Merchants" meta:resourcekey="ASRadComboBoxItemResource1" />
                                        <as:ASRadComboBoxItem Value="MERCHANTNUMBER" Text="Merchant ID" meta:resourcekey="ASRadComboBoxItemResource2" />
                                    </Items>
                                </as:ASRadComboBox>
                            </div>
                            <div class="filter-item">
                                <div id="cidSearchValue">
                                    <asp:TextBox ID="uxSearchValue" CssClass="form-control ml-1" runat="server" Width="160px" MaxLength="16" meta:resourcekey="uxSearchValueResource1"></asp:TextBox>
                                </div>
                            </div>


                        </td>
                    </tr>
                    <tr>
                        <td class="text-right"></td>
                        <td class="text-right">
                            <div class="filter-item" style="min-width: 150px">
                                <asp:Button ID="uxSearch" runat="server" CssClass="btn btn-default risk-mgmt-btn" Text="Create Report"
                                    OnClientClick="return ValidateData();" OnClick="uxSearch_Click" meta:resourcekey="uxSearchResource1" />
                            </div>
                        </td>
                    </tr>

                    <as:HiddenField ID="haveMerchant" Value="0" runat="server" />
                    <as:HiddenField ID="haveDate" Value="0" runat="server" />
                    <as:HiddenField ID="optionDate" Value="0" runat="server" />
                </table>
            </div>
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
                    <as:RegExValidationItem ControlToValidateID="uxSearchValue" RegularExpression="^[-a-zA-Z0-9]{0,16}$" ResMessage="Resources.MessageManager.ValidationMessagesMerchantNumber" meta:resourcekey="RegExValidation1Resource1Resource1" />
                </Items>
            </as:Validator>
        </div>
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
        var RiskMgmtReporting_uxCboReportType = "<%=uxRCBReportType.ClientID %>";
        var RiskMgmtReporting_divDate = "<%=divDate.ClientID%>";
        var RiskMgmtReporting_divDateRange = "<%=divDateRange.ClientID%>";
        var RiskMgmtReporting_searchBtn = "<%=uxSearch.ClientID%>";
        var RiskPnlMerchantCriteria_ClientId = "<%=uxPnlMerchantCriteria.ClientID%>";
        var RiskPnlDateCriteria_ClientId = "<%=uxPnlDateCriteria.ClientID%>";
        var RiskhaveMerchant_ClientId = "<%=haveMerchant.ClientID%>";
        var RiskhaveDate_ClientId = "<%=haveDate.ClientID%>";
        var RiskMgmtReporting_optionDate = "<%= optionDate.ClientID%>";

    </script>
    <script type="text/javascript" src="<%=ResolveUrl("~/") %>res/js/risk/RiskMgmtReporting.js"></script>

</tek:RadCodeBlock>
