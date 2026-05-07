<%@ Control Language="C#" AutoEventWireup="true" CodeFile="rm_MCF_MerchantAlertHistory_ReportFilter.ascx.cs" Inherits="UserControls_rm_MCF_MerchantAlertHistory_ReportFilter" %>

<style>
    #divDate,
    #divDateRange {
        display: inline-block;
    }

    .RadPicker_Default {
        width: 118px !important;
    }

    .filter-item span.date-item:first-child {
        margin-left: 0;
    }

    .spec-merchant-filter .date-item {
        margin-left: 0;
    }

    .btn-filter-merchant.disabled {
        color: #8B8B8B;
    }
</style>

<div class="row collapse report-filter-panel">
    <div class="col-md-12 report-filter risk-mgmt-report-filter risk-filter-merchant-alert">
        <div class="filter-block">
            <div class="filter-block">
                <table>
                    <tr>
                        <td class="text-right"></td>
                        <td class="text-left">
                            <div class="filter-item text-nowrap">
                                <asp:RadioButton ID="optDaily" runat="server" Text="Daily" GroupName="Date" onclick="ChangeDateOption(this)"
                                    onkeypress="javascript:return false;" CssClass="date-item" meta:resourcekey="optDailyResource1" />
                                <asp:RadioButton ID="optMonthly" runat="server" Text="Monthly" GroupName="Date" onclick="ChangeDateOption(this)"
                                    onkeypress="javascript:return false;" CssClass="date-item" meta:resourcekey="optMonthlyResource1" />
                                <asp:RadioButton ID="optDateRange" runat="server" Text="Date Range" GroupName="Date" onclick="ChangeDateOption(this)"
                                    onkeypress="javascript:return false;" CssClass="date-item" meta:resourcekey="optDateRangeResource1" />
                            </div>
                            <div id="divDate">
                                <div class="filter-item">
                                    <as:RadDatePicker ID="uxDate" ShowPopupOnFocus="true" runat="server" Width="110px">
                                        <Calendar ID="Calendar1" FastNavigationStep="12" ShowRowHeaders="false" runat="server" />
                                        <DateInput ID="DateInput1" runat="server" onkeypress="return SearchEnterOnTextbox(event);" />
                                    </as:RadDatePicker>
                                </div>
                            </div>
                            <div id="divDateRange" style="display: none">
                                <div class="filter-item">
                                    <as:RadDatePicker ID="uxFromDate" ShowPopupOnFocus="true" runat="server" Width="110px">
                                        <Calendar ID="Calendar2" FastNavigationStep="12" ShowRowHeaders="false" runat="server" />
                                        <DateInput ID="DateInput2" runat="server" onkeypress="return SearchEnterOnTextbox(event);" />
                                    </as:RadDatePicker>
                                </div>
                                <div class="filter-item">
                                    <as:RadDatePicker ID="uxEndDate" ShowPopupOnFocus="true" runat="server" Width="110px">
                                        <Calendar ID="Calendar3" FastNavigationStep="12" ShowRowHeaders="false" runat="server" />
                                        <DateInput ID="DateInput3" runat="server" onkeypress="return SearchEnterOnTextbox(event);" />
                                    </as:RadDatePicker>
                                </div>
                            </div>
                        </td>
                    </tr>
                    <as:PlaceHolder ID="uxMerchantPanel" runat="server">
                        <tr>
                            <td class="text-right"></td>
                            <td class="text-left">
                                <div class="filter-item mb-0 spec-merchant-filter">
                                    <asp:RadioButton ID="optSpecMerchant" runat="server" Text="Specific Merchants" GroupName="Merchant" onclick="ChangeMerchantFilterOption(this)"
                                        onkeypress="javascript:return false;" CssClass="date-item" Checked="true" />
                                </div>
                                <div class="filter-item mb-0">
                                    <asp:RadioButton ID="optAllMerchant" runat="server" Text="All Merchants" GroupName="Merchant" onclick="ChangeMerchantFilterOption(this)"
                                        onkeypress="javascript:return false;" CssClass="date-item" />
                                </div>
                            </td>
                        </tr>
                        <tr class="merchant-desc-label">
                            <td class="text-right"></td>
                            <td class="text-left">
                                <div class="filter-item mb-0">
                                    <asp:Label ID="Label1" runat="server" Text="Enter a minimum of 1 Merchant ID or a Merchant Name" meta:resourcekey="LiteralResource9"></asp:Label>
                                </div>
                            </td>
                        </tr>
                        <tr>

                            <td class="text-right">
                                <label class="filter-label">
                                    <asp:Literal ID="Literal1" runat="server" Text="Merchant ID:" meta:resourcekey="LiteralResource1" /></label></td>
                            <td class="text-left">
                                <div class="filter-item">
                                    <as:PlaceHolder ID="divMerchant" runat="server">
                                        <as:TextBox ID="uxMerchantNumber" runat="server" Text="" Width="500px" CssClass="rf_TextBox" MaxLength="8500" onkeypress="return SearchEnterOnTextbox(event);" meta:resourcekey="uxMerchantNumberResource1"></as:TextBox>
                                    </as:PlaceHolder>
                                </div>
                                <span class="filter-text mr-9x-neg">
                                    <a href="#" class="btn-filter-merchant" onclick="return ShowPopupModal('rm_MCF_MgmtReport_MerchantFilter.aspx', 'auto'); return false;">
                                        <asp:Literal ID="Literal2" runat="server" Text="Find" meta:resourcekey="LiteralResource2" /></a>
                                </span>
                            </td>
                        </tr>

                        <tr>
                            <td class="text-right">
                                <label class="filter-label">
                                    <asp:Literal ID="Literal3" runat="server" Text="Merchant Name:" meta:resourcekey="LiteralResource6" /></label></td>
                            <td class="text-left">
                                <div class="filter-item">
                                    <as:TextBox ID="uxMerchantName" runat="server" MaxLength="60" Width="500px" CssClass="rf_TextBox" onkeypress="return SearchEnterOnTextbox(event);" meta:resourcekey="uxMerchantNameResource1"></as:TextBox>
                                </div>
                            </td>
                        </tr>
                    </as:PlaceHolder>
                    <as:PlaceHolder ID="uxAssignmentPanel" runat="server">
                        <tr>
                            <td class="text-right valign-top">
                                <label class="filter-label">
                                    <asp:Literal ID="Literal4" runat="server" Text="Assignment:" meta:resourcekey="LiteralResource3" /></label></td>
                            <td class="text-left">
                                <div class="filter-item risk-mgmt-big-chosen">
                                    <as:MultiChooser ID="uxAssignmentList" runat="server" Width="100%" Placeholder=" " CssClass="rf_TextBox" meta:resourcekey="uxAssignmentListResource1">
                                    </as:MultiChooser>
                                </div>
                            </td>
                        </tr>
                    </as:PlaceHolder>
                    <as:PlaceHolder ID="uxParameterPanel" runat="server">
                        <tr>
                            <td class="text-right valign-top">
                                <label class="filter-label">
                                    <asp:Literal ID="Literal5" runat="server" Text="Parameter:" meta:resourcekey="LiteralResource4" /></label></td>
                            <td class="text-left">
                                <div class="filter-item risk-mgmt-big-chosen">
                                    <as:MultiChooser ID="uxParameterList" runat="server" Width="100%" Placeholder=" " meta:resourcekey="uxParameterListResource1">
                                    </as:MultiChooser>
                                </div>
                            </td>
                        </tr>
                    </as:PlaceHolder>
                    <as:PlaceHolder ID="uxUserName" runat="server">
                        <tr>
                            <td class="text-right valign-top">
                                <label class="filter-label">
                                    <asp:Literal ID="Literal7" runat="server" Text="User Name:" meta:resourcekey="LiteralResource7" /></label></td>
                            <td class="text-left">
                                <div class="filter-item risk-mgmt-big-chosen">
                                    <as:MultiChooser ID="uxUserNameList" runat="server" Width="100%" Placeholder=" " meta:resourcekey="uxUserNameListResource1">
                                    </as:MultiChooser>
                                </div>
                            </td>
                        </tr>
                    </as:PlaceHolder>
                    <as:PlaceHolder ID="uxDisposition" runat="server">
                        <tr>
                            <td class="text-right valign-top">
                                <label class="filter-label">
                                    <asp:Literal ID="Literal8" runat="server" Text="Disposition:" meta:resourcekey="LiteralResource8" /></label></td>
                            <td class="text-left">
                                <div class="filter-item risk-mgmt-big-chosen">
                                    <as:MultiChooser ID="uxDispositionList" runat="server" Width="100%" Placeholder=" " meta:resourcekey="uxDispositionListResource1">
                                    </as:MultiChooser>
                                </div>
                            </td>
                        </tr>
                    </as:PlaceHolder>
                </table>
            </div>
            <div class="filter-block valign-bottom">
                <div class="filter-item ">
                    <asp:Button ID="uxSearch" runat="server" CssClass="btn btn-default" Text="Search"
                        OnClientClick="return ValidateData();" OnClick="uxSearch_Click" meta:resourcekey="uxSearchResource1" />
                </div>
            </div>
        </div>
    </div>
</div>
<div class="row">
    <div class="col-md-12 text-center" data-target=".report-filter-panel" data-toggle="collapse">
        <span class="btn btn-link btn-report-filter">
            <asp:Literal ID="Literal6" runat="server" Text="FILTER" meta:resourcekey="LiteralResource5" /></span>
    </div>
</div>
<as:Button ID="uxRefresh" runat="server" Style="display: none;" OnClick="uxRefresh_Click" IsStandardButton="True" meta:resourcekey="uxRefreshResource1" />
<asp:HiddenField ID="hddMerchantList" runat="server" />
<asp:HiddenField ID="hhdDateFrom" runat="server" />
<asp:HiddenField ID="hhdDateTo" runat="server" />
<tek:RadAjaxManagerProxy runat="server" ID="uxRadAjaxManager">
    <AjaxSettings>
        <tek:AjaxSetting AjaxControlID="uxRefresh">
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="divMerchant" />
            </UpdatedControls>
        </tek:AjaxSetting>
    </AjaxSettings>
</tek:RadAjaxManagerProxy>
<script type="text/javascript">
    var MerchantAlertHistory_ReportFilter_optDaily = "<%=optDaily.ClientID %>";
    var MerchantAlertHistory_ReportFilter_optMonthly = "<%=optMonthly.ClientID %>";
    var MerchantAlertHistory_ReportFilter_optDateRange = "<%=optDateRange.ClientID %>";

    var MerchantAlertHistory_ReportFilter_optSpecMerchant = "<%=optSpecMerchant.ClientID %>";
    var MerchantAlertHistory_ReportFilter_optAllMerchant = "<%=optAllMerchant.ClientID %>";

    //DatePicker
    var MerchantAlertHistory_ReportFilter_uxFromDate = "<%=uxFromDate.ClientID %>";
    var MerchantAlertHistory_ReportFilter_uxEndDate = "<%=uxEndDate.ClientID %>";
    var MerchantAlertHistory_ReportFilter_uxDate = "<%=uxDate.ClientID %>";

    var MerchantAlertHistory_ReportFilter_uxSearch = "<%=uxSearch.ClientID%>";

    var MerchantAlertHistory_ReportFilter_uxMerchantNumber = "<%=uxMerchantNumber.ClientID %>";
    var MerchantAlertHistory_ReportFilter_uxMerchantName = "<%=uxMerchantName.ClientID %>";
    var hhdDateFrom_ClientID = "<%=hhdDateFrom.ClientID %>";
    var hhdDateTo_ClientID = "<%=hhdDateTo.ClientID %>";

    var MerchantAlertHistory_ReportFilter_ReportFilter_V2 = "<%=Resources.MessageManager.ReportFilter_V2 %>";
    var MerchantAlertHistory_ReportFilter_ReportFilter_V9 = "<%=Resources.MessageManager.ReportFilter_V9 %>";
    var MerchantAlertHistory_ReportFilter_ReportFilter_V3 = "<%=Resources.MessageManager.ReportFilter_V3 %>";
    var MerchantAlertHistory_ReportFilter_ReportFilter_V7 = "<%=Resources.MessageManager.ReportFilter_V7 %>";
    var MerchantAlertHistory_ReportFilter_ReportFilter_V6 = "<%=Resources.MessageManager.ReportFilter_V6 %>";
    var MerchantAlertHistory_ReportFilter_ReportFilter_V11 = "<%=Resources.MessageManager.ReportFilter_V11 %>";
    var MerchantAlertHistory_ReportFilter_ReportDate_InvaidDate = "<%=Resources.MessageManager.ReportFilter_ReportDate_InvaidDate %>";
    var MerchantAlertHistory_ReportFilter_hddMerchantList = "<%=hddMerchantList.ClientID%>";
    var MerchantAlertHistory_ReportFilter_uxRefresh = "<%=uxRefresh.ClientID%>";
    var MerchantAlertHistory_ReportFilter_LargeDataSetWarningMsg = '<%= GetLocalResourceObject("LargeDataSetWarningMsg")%>';
    var MerchantAlertHistory_ReportFilter_LimitDaysWarningMsg = <%=LimitReportDayMerchanAlertHistory %>;

    var MerchantAlertHistory_ReportFilter_js_MerchantNumber = '<%= GetLocalResourceObject("LiteralResource1.Text").ToString()%>';
    var MerchantAlertHistory_ReportFilter_js_MerchantName = '<%= GetLocalResourceObject("LiteralResource6.Text").ToString()%>';
    var Text_MerchantName = '<%=GetLocalResourceObject("LiteralResource6.Text").ToString()%>';
    var Text_Daily = '<%=GetLocalResourceObject("optDailyResource1.Text").ToString()%>';
    var Text_Monthly = '<%=GetLocalResourceObject("optMonthlyResource1.Text").ToString()%>';
    var Text_DateRange = '<%=GetLocalResourceObject("optDateRangeResource1.Text").ToString()%>';
    var MerchantAlertHistory_ReportFilter_Limit90Days = '<%= GetLocalResourceObject("Litmit90DaysMsg")%>';
    var MinimumOfMerchantIDOrMerchantNameMsg = '<%= GetLocalResourceObject("MinimumOfMerchantIDOrMerchantNameMsg")%>';
</script>
<script src="<%=ResolveUrl("~") %>res/js/risk_MCF/rm_MCF_MerchantAlertHistory_ReportFilter.js"></script>
