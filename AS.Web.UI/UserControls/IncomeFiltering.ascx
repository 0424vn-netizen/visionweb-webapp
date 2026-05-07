<%@ Control Language="C#" AutoEventWireup="true" CodeFile="IncomeFiltering.ascx.cs"
    Inherits="UserControls_IncomeFiltering" %>

<div class="row collapse report-filter-panel">
    <div class="col-md-12 income-filter">
        <div id="divContainer" class="report-filter-default" onkeypress="rf_SubmitOnEnter(event);">
            <div class="filter-left">
                <span class="date_item">
                    <asp:RadioButton ID="uxradIEYearToDate" onclick="SelectDateChange();" runat="server"
                        Text="Year To Date" GroupName="DateFilterOption" meta:resourcekey="uxradIEYearToDateResource1" />
                </span>
                <span class="date_item">
                    <asp:RadioButton ID="uxradTwelveMonth" onclick="SelectDateChange();" runat="server"
                        Text="Trailing Twelve Months" GroupName="DateFilterOption" meta:resourcekey="uxradTwelveMonthResource1" />
                </span>
                <span class="date_item">
                    <asp:RadioButton ID="uxradDateRange" onclick="SelectDateChange();" Checked="True"
                        runat="server" Text="Date Range" GroupName="DateFilterOption" meta:resourcekey="uxradDateRangeResource1" />
                </span>
            </div>
            <div class="filter-right">
                <div id="pnlDateRangecontrols" class="line-height-zero">
                    <span id="pnlDateRangecontrolsfrom" runat="server" class="date_item_from">
                        <as:RadDatePicker ID="uxIEDateFrom" runat="server">
                        </as:RadDatePicker>
                    </span><span id="pnlDateRangecontrolsto" runat="server" class="date_item_to">
                        <as:RadDatePicker ID="uxIEDateTo" runat="server">
                        </as:RadDatePicker>
                    </span>
                </div>
            </div>
        </div>
        <as:MPSReportFilter ID="uxReportFilter" runat="server" DateOptionVisible="false" OnReportFilterAction="uxReportFilter_ReportFilterAction" ButtonSubmitCss="display-none" />
        <div class="report-filter-default" onkeypress="rf_SubmitOnEnter(event);">
            <div class="filter-left">
                    <label class="filter-label"><asp:Literal ID="Literal1" runat="server"  Text="Net Profit:" meta:resourcekey="LiteralResource1" /></label>
                    <as:RadComboBox runat="server" ID="uxNetProfit" Width="250px" OnClientSelectedIndexChanged="uxNetProfitOnClientSelectedIndexChanged">
                        <Items>
                            <as:RadComboBoxItem Value="All" Text="All Values"  meta:resourcekey="IncomeFilteringASPX_Text_uxNetProfitResource1" />
                            <as:RadComboBoxItem Value="GREATERTHAN" Text="Greater Than" meta:resourcekey="IncomeFilteringASPX_Text_uxNetProfitResource2" />
                            <as:RadComboBoxItem Value="LESSTHAN" Text="Less Than" meta:resourcekey="IncomeFilteringASPX_Text_uxNetProfitResource3"/>
                            <as:RadComboBoxItem Value="BETWEEN" Text="Between" meta:resourcekey="IncomeFilteringASPX_Text_uxNetProfitResource4" />
                        </Items>
                    </as:RadComboBox>
            </div>
            <div class="filter-right">
                <span id="pnlProfitFrom" class="display-none" runat="server">
                    <as:RadNumericTextBox runat="server" CssClass="to-textbox" ID="uxNetProfitFrom" Width="112px" Type="Currency"></as:RadNumericTextBox>
                </span>
                <span id="pnlProfitTo" runat="server" class="display-none">
                    <label class="filter-label">
                        <asp:Literal ID="Literal2" runat="server" Text="AND" meta:resourcekey="Literal1Resource1" />
                    </label>
                    <as:RadNumericTextBox runat="server" CssClass="from-textbox" ID="uxNetProfitTo" Width="112px" Type="Currency"></as:RadNumericTextBox>
                </span>
                
            </div>
            <as:PlaceHolder ID="uxPanelProfile" runat="server">
                <div class="filter-left">
                    <label class="filter-label"><asp:Literal ID="Literal3" runat="server"  Text="Profile:" meta:resourcekey="LiteralResource2" /></label>
                    <as:RadComboBox ID="uxProfile" runat="server" Width="150" EnableEmbeddedSkins="false"
                                        EnableEmbeddedBaseStylesheet="false" MaxHeight="250px" meta:resourcekey="uxProfileResource1" />
                </div>
                <div class="filter-right"></div>
            </as:PlaceHolder>
            <div class="filter-left">
            <as:Button ID="uxFakeSearchButton" runat="server" Text="Search" CssClass="btn btn-default" OnClientClick="return SubmitFilter();" IsStandardButton="False" meta:resourcekey="uxFakeSearchButtonResource1" />
                </div>
            <div class="filter-right"></div>
        </div>
    </div>
</div>
<div class="row">
    <div class="col-md-12 text-center" data-target=".report-filter-panel" data-toggle="collapse">
        <span class="btn btn-link btn-report-filter"><asp:Literal ID="Literal4" runat="server"  Text="FILTER" meta:resourcekey="LiteralResource3" /></span>
    </div>
</div>

<script type="text/javascript">
    var uxradDateRangeID = '<%= uxradDateRange.ClientID %>';
    var uxradIEYearToDateID = '<%= uxradIEYearToDate.ClientID %>';
    var uxradTwelveMonthID = '<%= uxradTwelveMonth.ClientID %>';
    var uxIEDateFromID = '<%= uxIEDateFrom.ClientID %>';
    var uxIEDateToID = '<%= uxIEDateTo.ClientID %>';
    var pnlDateRangecontrolsfromID = '<%= pnlDateRangecontrolsfrom.ClientID %>';
    var pnlDateRangecontrolstoID = '<%= pnlDateRangecontrolsto.ClientID %>';
    var pnlProfitFromID = '<%= pnlProfitFrom.ClientID %>';
    var pnlProfitToID = '<%= pnlProfitTo.ClientID %>';
    var uxNetProfitFromID = '<%= uxNetProfitFrom.ClientID %>';
    var uxNetProfitToID = '<%= uxNetProfitTo.ClientID %>';
    var uxNetProfitID = '<%= uxNetProfit.ClientID %>';
    var uxReportFilterID = '<%= uxReportFilter.ClientID %>';

    var defaultFromDate = new Date('<%= DefaultFromDate %>');
    var uxFakeSearchButtonID = '<% = uxFakeSearchButton.ClientID%>';

    var Text_DateFormatInvalid = '<%= GetLocalResourceObject("IncomeFilteringJS_Text_DateFormatInvalid").ToString()%>';
    var Text_EndGreaterBeginDate = '<%= GetLocalResourceObject("IncomeFilteringJS_Text_EndGreaterBeginDate").ToString()%>';
    var Text_EndGreaterToday = "<%= GetLocalResourceObject("IncomeFilteringJS_Text_EndGreaterToday").ToString()%>";
    var Text_NetProfitFromRequired = '<%= GetLocalResourceObject("IncomeFilteringJS_Text_NetProfitFromRequired").ToString()%>';
    var Text_NetProfitToRequired = '<%= GetLocalResourceObject("IncomeFilteringJS_Text_NetProfitToRequired").ToString()%>';
    var Text_ToGreaterFrom = '<%= GetLocalResourceObject("IncomeFilteringJS_Text_ToGreaterFrom").ToString()%>';
    var Msg_Date_24Month = "<%=Resources.MessageManager.ValidationMessages_V10%>";
</script>
<script type="text/javascript" src="<% =ResolveUrl("~") %>res/js/IncomeFiltering.js"></script>

