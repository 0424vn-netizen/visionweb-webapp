<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserAccessFiltering.ascx.cs"
    Inherits="UserControls_UserAccessFiltering" %>

<div class="row collapse report-filter-panel">
    <div class="col-md-12 report-filter">
        <div class="filter-block" runat="server" id="uxFilteringTable">
            <table>
                <colgroup>
                    <col />
                    <col />
                    <col style="width: 30px;" />
                </colgroup>
                <tr>
                    <td class="text-right">
                        <div class="filter-item">
                            <as:RadioButton runat="server" ID="uxtadDaily" class="date-item" onclick="DateoptionChange();" Text="Daily"
                                GroupName="DateOption" meta:resourcekey="uxtadDailyResource1" Value="" />
                            <as:RadioButton runat="server" ID="uxradMonthly" class="date-item" onclick="DateoptionChange();" Text="Monthly"
                                GroupName="DateOption" meta:resourcekey="uxradMonthlyResource1" Value="" />
                            <as:RadioButton runat="server" ID="uxradDateRange" class="date-item" onclick="DateoptionChange();"
                                Text="Date Range" GroupName="DateOption" Checked="True" meta:resourcekey="uxradDateRangeResource1" Value="" />
                        </div>
                    </td>
                    <td class="text-left" colspan="2">
                        <div id="pnlDateoptiop">
                            <div class="filter-item">
                                <span id="pnlDateRangecontrolsfrom" runat="server" style="visibility: visible">
                                    <as:RadDatePicker ID="uxDateFrom" runat="server">
                                    </as:RadDatePicker>

                                </span>
                            </div>
                            <div class="filter-item">
                                <span id="pnlDateRangecontrolsto" runat="server" style="visibility: hidden">
                                    <as:RadDatePicker ID="uxDateTo" runat="server">
                                    </as:RadDatePicker>
                                </span>
                            </div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="text-right">
                        <div class="filter-item">
                            <as:Literal ID="ltSearchCriteria" runat="server" Text="Search Criteria:" meta:resourcekey="ltSearchCriteriaResource1"></as:Literal>
                        </div>
                    </td>
                    <td class="text-right">
                        <as:RadComboBox runat="server" Width="248px" ID="uxSearchType" CssClass="filter-item" OnClientSelectedIndexChanged="SearchTypeOnClientSelectedIndexChanged">
                            <Items>
                                <as:RadComboBoxItem Value="All" Text="ALL" meta:resourcekey="RadComboBoxItemAll" />
                                <as:RadComboBoxItem Value="USERTYPE" Text="User Type" meta:resourcekey="RadComboBoxItemUserType" />
                                <as:RadComboBoxItem Value="USERROLE" Text="User Role" meta:resourcekey="RadComboBoxItemUserRole" />
                                <as:RadComboBoxItem Value="USERID" Text="User ID" meta:resourcekey="RadComboBoxItemUserID" />
                                <as:RadComboBoxItem Value="FIRSTNAME" Text="First Name" meta:resourcekey="RadComboBoxItemFirstName" />
                                <as:RadComboBoxItem Value="LASTNAME" Text="Last Name" meta:resourcekey="RadComboBoxItemLastName" />
                                <as:RadComboBoxItem Value="EMAIL" Text="Email Address" meta:resourcekey="RadComboBoxItemEmail" />
                            </Items>
                        </as:RadComboBox>
                    </td>
                    <td></td>
                </tr>
                <tr id="uxtr" runat="server">
                    <td class="text-right" colspan="2">
                        <div id="pnlcbbSearch" style="display: none;" runat="server">
                            <as:RadComboBox runat="server" CssClass="filter-item" Width="248px" ID="uxcbbSearchValue" OnClientSelectedIndexChanged="SearchValueOnClientSelectedIndexChanged">
                            </as:RadComboBox>
                        </div>
                        <div id="pnltxtSearchValue" style="display: none;" runat="server">
                            <div class="filter-item">
                                <as:TextBox ID="uxSearchValue" CssClass="rf_TextBox" runat="server" Width="248px" HintCss="hint" meta:resourcekey="uxSearchValueResource1" />
                            </div>
                        </div>
                        <div id="pnlcbbSearchRole" style="display: none;" runat="server">
                            <as:RadComboBox runat="server" CssClass="filter-item" Width="248px" ID="uxcbbUserRole" MaxHeight="210px"></as:RadComboBox>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="text-right" colspan="2">
                        <div id="pnlFilterVal" style="display: none;" runat="server">
                            <div class="filter-item">
                                <as:TextBox ID="uxFilterVal" CssClass="rf_TextBox" MaxLength="100" runat="server" Width="248px" HintCss="hint" meta:resourcekey="uxSearchValueResource1" />
                            </div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td></td>
                    <td class="text-right">
                        <div class="filter-item">
                            <as:Button runat="server" CssClass="btn btn-default" Text="Search" ID="uxbtnSearch" OnClientClick="return validation();" IsStandardButton="False" meta:resourcekey="uxbtnSearchResource1" />
                        </div>
                    </td>
                    <td></td>
                </tr>
            </table>

        </div>
    </div>
</div>
<!-- -->
<div class="row">
    <div class="col-md-12 text-center" data-target=".report-filter-panel" data-toggle="collapse">
        <span class="btn btn-link btn-report-filter">
            <as:Literal ID="Literal1" runat="server" Text="FILTER" meta:resourcekey="Literal1Resource1"></as:Literal></span>
    </div>
</div>

<script type="text/javascript">
    var uxtadDaily_ClientID = '<%= uxtadDaily.ClientID %>';
    var uxradMonthly_ClientID = '<%= uxradMonthly.ClientID %>';
    var uxradDateRange_ClientID = '<%= uxradDateRange.ClientID %>';
    var uxDateFrom_ClientID = '<%= uxDateFrom.ClientID %>';
    var uxDateTo_ClientID = '<%= uxDateTo.ClientID %>';
    var pnlDateRangecontrolsfrom_ClientID = '<%= pnlDateRangecontrolsfrom.ClientID %>';
    var pnlDateRangecontrolsto_ClientID = '<%= pnlDateRangecontrolsto.ClientID %>';
    var FromDate = '<%= FromDate %>';
    var uxtr_ClientID = '<%= uxtr.ClientID %>';
    var uxcbbSearchValue_ClientID = '<%= uxcbbSearchValue.ClientID %>';

    var pnltxtSearchValue_ClientID = "<%= pnltxtSearchValue.ClientID %>";
    var pnlFilterVal_ClientID = "<%= pnlFilterVal.ClientID %>";
    var pnlcbbSearch_ClientID = '<%= pnlcbbSearch.ClientID %>';
    var uxSearchValue_ClientID = '<%= uxSearchValue.ClientID %>';
    var uxradDateRange_ClientID = '<%= uxradDateRange.ClientID %>';

    var uxFilteringTable_ClientID = '<%= uxFilteringTable.ClientID %>';
    var uxbtnSearch_ClientID = '<%= uxbtnSearch.ClientID %>';
    var uxSearchType_ClientID = '<%= uxSearchType.ClientID %>';
    var pnlcbbSearchRole_ClientID = '<%= pnlcbbSearchRole.ClientID %>';

    var UserAccessFiltering_ascx_js_InvalidDate = '<%= GetLocalResourceObject("UserAccessFiltering_ascx_js_InvalidDate").ToString() %>';
    var UserAccessFiltering_ascx_js_GreaterThan = '<%= GetLocalResourceObject("UserAccessFiltering_ascx_js_GreaterThan").ToString() %>';
    var UserAccessFiltering_ascx_js_GreaterThanToday = '<%= GetLocalResourceObject("UserAccessFiltering_ascx_js_GreaterThanToday").ToString() %>';
    var UserAccessFiltering_ascx_js_NotGreaterThanToday = '<%= GetLocalResourceObject("UserAccessFiltering_ascx_js_NotGreaterThanToday").ToString() %>';
    var UserAccessFiltering_ascx_js_RequriedField = '<%= GetLocalResourceObject("UserAccessFiltering_ascx_js_RequriedField").ToString() %>';
    var UserAccessFiltering_ascx_js_Email = '<%= GetLocalResourceObject("UserAccessFiltering_ascx_js_Email").ToString() %>';
    var RadComboBoxItemUserID_Text = '<%= GetLocalResourceObject("RadComboBoxItemUserID.Text").ToString() %>';
    var RadComboBoxItemFirstName_Text = '<%= GetLocalResourceObject("RadComboBoxItemFirstName.Text").ToString() %>';
    var RadComboBoxItemLastName_Text = '<%= GetLocalResourceObject("RadComboBoxItemLastName.Text").ToString() %>';
    var msgNotAllowSpecialCharacter = "<%= GetLocalResourceObject("msgNotAllowSpecialCharacter").ToString() %>";
    var uxFilterVal_ClientID = '<%= uxFilterVal.ClientID %>';
    var default_Text = '<%= Filter_All %>';
    var funcValidate = 'validation';

</script>
<script src="<%= ResolveUrl("~/")%>res/js/usermaintenance/UserAccessFiltering.js"></script>

