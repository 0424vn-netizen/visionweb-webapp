<%@ Control Language="C#" AutoEventWireup="true" CodeFile="rm_MCF_MgmtReport_ReportFilter.ascx.cs" Inherits="UserControls_rm_MCF_MgmtReport_ReportFilter" %>

<div class="row collapse report-filter-panel">
    <div class="col-md-12 report-filter risk-mgmt-report-filter">
        <div class="filter-block">
            <div class="filter-block">
                <table>
                    <tr>
                        <td></td>
                        <td class="text-left">
                            <div class="filter-item risk-mgmt-date-item default-width text-right">
                                <as:RadioButton onkeypress="javascript:return false;" ID="optDaily" runat="server"
                                    Text="Daily" GroupName="Date" onclick="ChangeDateOption(this)"
                                    CssClass="date-item first" meta:resourcekey="optDailyResource1" Value="" />
                                <as:RadioButton onkeypress="javascript:return false;" ID="optWeekly" runat="server"
                                    Text="Weekly" GroupName="Date" onclick="ChangeDateOption(this)"
                                    CssClass="date-item" meta:resourcekey="optWeeklyResource1" Value=""/>
                                <as:RadioButton onkeypress="javascript:return false;" ID="optMonthly" runat="server"
                                    Text="Monthly" GroupName="Date" onclick="ChangeDateOption(this)" 
                                    CssClass="date-item" meta:resourcekey="optMonthlyResource1" Value="" />
                                <as:RadioButton onkeypress="javascript:return false;" ID="optDateRange" runat="server" Visible="False"
                                    Text="Date Range" GroupName="Date" onclick="ChangeDateOption(this)"
                                    CssClass="date-item" meta:resourcekey="optDateRangeResource1" Value="" />
                            </div><!--
                            --><div id="pnlDaily" runat="server" class="filter-item">
                                <as:RadDatePicker ID="uxDaily" runat="server" ShowPopupOnFocus="true" meta:resourcekey="uxCalendaPopupResource1">
                                    <Calendar FastNavigationStep="12" ShowRowHeaders="false">
                                    </Calendar>
                                    <DateInput ID="DateInput1" runat="server" onclick="ShowCalendar('1')" onkeypress="return SearchEnterOnTextbox(event);" />
                                </as:RadDatePicker>
                            </div><!--
                            --><div id="pnlWeekly" runat="server" class="filter-item">
                                <as:RadDatePicker ID="uxWeekly" ShowPopupOnFocus="true" meta:resourcekey="uxCalendaPopupResource1"
                                    runat="server" Skin="Default">
                                    <Calendar FastNavigationStep="12" ShowRowHeaders="false">
                                    </Calendar>
                                    <DateInput ID="DateInput4" runat="server" onclick="ShowCalendar('5')" onkeypress="return SearchEnterOnTextbox(event);" />
                                </as:RadDatePicker>
                            </div><!--
                            --><div id="pnlMonthly" runat="server" class="filter-item" >
                                <as:RadDatePicker ID="uxMonthly" runat="server" Skin="Default" ShowPopupOnFocus="true" meta:resourcekey="uxCalendaPopupResource1">
                                    <Calendar FastNavigationStep="12" ShowRowHeaders="false">
                                    </Calendar>
                                    <DateInput ID="DateInput5" runat="server" onclick="ShowCalendar('2')" onkeypress="return SearchEnterOnTextbox(event);" />
                                </as:RadDatePicker>
                            </div><!--
                            --><div id="pnlDateRange" runat="server" class="filter-item">
                                <as:RadDatePicker ID="uxDateRangeFrom" runat="server">
                                    <Calendar FastNavigationStep="12" ShowRowHeaders="false">
                                    </Calendar>
                                    <DateInput ID="DateInput2" runat="server" onclick="ShowCalendar('3')" onkeypress="return SearchEnterOnTextbox(event);" />
                                </as:RadDatePicker>
                                <as:RadDatePicker ID="uxDateRangeTo" runat="server">
                                    <Calendar FastNavigationStep="12" ShowRowHeaders="false">
                                    </Calendar>
                                    <DateInput ID="DateInput3" runat="server" onclick="ShowCalendar('4')" onkeypress="return SearchEnterOnTextbox(event);" />
                                </as:RadDatePicker>
                            </div>
                        </td>
                    </tr>
                    <as:PlaceHolder ID="uxUserGroupPanel" runat="server" Visible="False">
                        <tr>
                            <td></td>
                            <td class="text-left">
                                <div class="filter-item default-width text-right">
                                    <as:ASRadComboBox ID="uxOption" runat="server" OnClientSelectedIndexChanged="doUserGroupSelectdIndexChanged" Width="220px" meta:resourcekey="uxOptionResource1">
                                        <Items>
                                            <as:ASRadComboBoxItem Value="optAgent" Text="Agent" Selected="true" meta:resourcekey="ASRadComboBoxItemResource1" />
                                            <as:ASRadComboBoxItem Value="optGroup" Text="Group" meta:resourcekey="ASRadComboBoxItemResource2" />
                                        </Items>
                                    </as:ASRadComboBox>
                                </div><asp:Panel ID="uxAgentPanel" runat="server" CssClass="AgentPanel filter-item risk-mgmt-small-chosen" meta:resourcekey="uxAgentPanelResource1">
                                    <as:MultiChooser ID="uxAgentList" runat="server" Placeholder=" " meta:resourcekey="uxAgentListResource1">
                                    </as:MultiChooser>
                                </asp:Panel><asp:Panel ID="uxGroupPanel" runat="server" CssClass="GroupPanel filter-item risk-mgmt-small-chosen" meta:resourcekey="uxGroupPanelResource1">
                                    <as:MultiChooser ID="uxGroupList" runat="server" Placeholder=" " meta:resourcekey="uxGroupListResource1">
                                    </as:MultiChooser>
                                </asp:Panel>
                            </td>
                        </tr>
                    </as:PlaceHolder>
                    <as:PlaceHolder ID="uxMerchantPanel" runat="server" Visible="False">
                        <tr>
                            <td class="text-right">
                                <label class="filter-label"><asp:Literal ID="Literal1" runat="server" Text="Merchant Number:" meta:resourcekey="LiteralResource1" /></label></td>
                            <td class="text-left">
                                <div class="filter-item">
                                    <as:PlaceHolder ID="divMerchant" runat="server">
                                        <as:TextBox ID="uxMerchantNumber" runat="server" Text="" Width="500px" CssClass="rf_TextBox" MaxLength="8500" onkeypress="return SearchEnterOnTextbox(event);" meta:resourcekey="uxMerchantNumberResource1"></as:TextBox>
                                    </as:PlaceHolder>
                                </div>
                                <span class="filter-text mr-9x-neg">
                                    <a href="#" onclick="return ShowPopupModal('rm_MCF_MgmtReport_MerchantFilter.aspx', 'auto'); return false;"><asp:Literal ID="Literal2" runat="server" Text="Find" meta:resourcekey="LiteralResource2" /></a>
                                </span>
                            </td>
                        </tr>
                        <tr>
                            <td class="text-right">
                                <label class="filter-label"><asp:Literal ID="Literal3" runat="server" Text="Merchant Name:" meta:resourcekey="LiteralResource6" /></label></td>
                            <td class="text-left">
                                <div class="filter-item">
                                    <as:TextBox ID="uxMerchantName" runat="server" MaxLength="60" Width="500px" CssClass="rf_TextBox" onkeypress="return SearchEnterOnTextbox(event);" meta:resourcekey="uxMerchantNameResource1"></as:TextBox>
                                </div>
                            </td>
                        </tr>
                    </as:PlaceHolder>
                    <as:PlaceHolder ID="uxAssignmentPanel" runat="server" Visible="False">
                        <tr>
                            <td class="text-right valign-top">
                                <label class="filter-label"><asp:Literal ID="Literal4" runat="server" Text="Assignment:" meta:resourcekey="LiteralResource3" /></label></td>
                            <td class="text-left">
                                <div class="filter-item risk-mgmt-big-chosen">
                                    <as:MultiChooser ID="uxAssignmentList" runat="server" Width="100%" Placeholder=" " CssClass="rf_TextBox" meta:resourcekey="uxAssignmentListResource1">
                                    </as:MultiChooser>
                                </div>
                            </td>
                        </tr>
                    </as:PlaceHolder>
                    <as:PlaceHolder ID="uxParameterPanel" runat="server" Visible="False">
                        <tr>
                            <td class="text-right valign-top">
                                <label class="filter-label"><asp:Literal ID="Literal5" runat="server" Text="Parameter:" meta:resourcekey="LiteralResource4" /></label></td>
                            <td class="text-left">
                                <div class="filter-item risk-mgmt-big-chosen">
                                    <as:MultiChooser ID="uxParameterList" runat="server" Width="100%" Placeholder=" " meta:resourcekey="uxParameterListResource1">
                                    </as:MultiChooser>
                                </div>
                            </td>
                        </tr>
                    </as:PlaceHolder>
                </table>
            </div><div class="filter-block valign-bottom">
                <div class="filter-item">
                    <asp:Button ID="uxSearch" runat="server" CssClass="btn btn-default risk-mgmt-btn" Text="Search"
                        OnClientClick="return ValidateData();" OnClick="uxSearch_Click" meta:resourcekey="uxSearchResource1" />
                </div>
            </div>
        </div>
    </div>
</div>
<div class="row">
    <div class="col-md-12 text-center" data-target=".report-filter-panel" data-toggle="collapse">
        <span class="btn btn-link btn-report-filter"><asp:Literal ID="Literal6" runat="server" Text="FILTER" meta:resourcekey="LiteralResource5" /></span>
    </div>
</div>
<as:Button ID="uxRefresh" runat="server" Style="display: none;" OnClick="uxRefresh_Click" IsStandardButton="True" meta:resourcekey="uxRefreshResource1" />
<asp:HiddenField ID="hddMerchantList" runat="server" />
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
    var MgmtReport_ReportFilter_optDaily = "<%=optDaily.ClientID %>";
    var MgmtReport_ReportFilter_optWeekly = "<%=optWeekly.ClientID %>";
    var MgmtReport_ReportFilter_optMonthly = "<%=optMonthly.ClientID %>";
    var MgmtReport_ReportFilter_optDateRange = "<%=optDateRange.ClientID %>";

    //Panel DataPicker
    var MgmtReport_ReportFilter_pnlDaily = "<%=pnlDaily.ClientID %>";
    var MgmtReport_ReportFilter_pnlWeekly = "<%=pnlWeekly.ClientID %>";
    var MgmtReport_ReportFilter_pnlMonthly = "<%=pnlMonthly.ClientID %>";
    var MgmtReport_ReportFilter_pnlDateRange = "<%=pnlDateRange.ClientID %>";

    //DatePicker
    var MgmtReport_ReportFilter_uxDaily = "<%=uxDaily.ClientID %>";
    var MgmtReport_ReportFilter_uxWeekly = "<%=uxWeekly.ClientID %>";
    var MgmtReport_ReportFilter_uxMonthly = "<%=uxMonthly.ClientID %>";
    var MgmtReport_ReportFilter_uxDateRangeFrom = "<%=uxDateRangeFrom.ClientID %>";
    var MgmtReport_ReportFilter_uxDateRangeTo = "<%=uxDateRangeTo.ClientID %>";

    var MgmtReport_ReportFilter_uxSearch = "<%=uxSearch.ClientID%>";


    var MgmtReport_ReportFilter_uxAgentPanel = "<%=uxAgentPanel.ClientID %>";
    var MgmtReport_ReportFilter_uxGroupPanel = "<%=uxGroupPanel.ClientID %>";

    var MgmtReport_ReportFilter_uxMerchantNumber = "<%=uxMerchantNumber.ClientID %>";
    var MgmtReport_ReportFilter_uxMerchantName = "<%=uxMerchantName.ClientID %>";

    var MgmtReport_ReportFilter_ReportFilter_V2 = "<%=Resources.MessageManager.ReportFilter_V2 %>";
    var MgmtReport_ReportFilter_ReportFilter_V9 = "<%=Resources.MessageManager.ReportFilter_V9 %>";
    var MgmtReport_ReportFilter_ReportFilter_V3 = "<%=Resources.MessageManager.ReportFilter_V3 %>";
    var MgmtReport_ReportFilter_ReportFilter_V7 = "<%=Resources.MessageManager.ReportFilter_V7 %>";
    var MgmtReport_ReportFilter_ReportFilter_V6 = "<%=Resources.MessageManager.ReportFilter_V6 %>";
    var MgmtReport_ReportFilter_ReportFilter_V11 = "<%=Resources.MessageManager.ReportFilter_V11 %>";
    var MgmtReport_ReportFilter_ReportDate_InvaidDate = "<%=Resources.MessageManager.ReportFilter_ReportDate_InvaidDate %>";
    var MgmtReport_ReportFilter_hddMerchantList = "<%=hddMerchantList.ClientID%>";
    var MgmtReport_ReportFilter_uxRefresh = "<%=uxRefresh.ClientID%>";

    var Text_MerchantName = '<%=GetLocalResourceObject("LiteralResource6.Text").ToString()%>';
    var Text_Daily = '<%=GetLocalResourceObject("MgmtReport_ReportFilterJS_Text_Daily").ToString()%>';
    var Text_Monthly = '<%=GetLocalResourceObject("MgmtReport_ReportFilterJS_Text_Monthly").ToString()%>';
    var Text_Weekly = '<%=GetLocalResourceObject("MgmtReport_ReportFilterJS_Text_Weekly").ToString()%>';
    var Text_DateRange = '<%=GetLocalResourceObject("MgmtReport_ReportFilterJS_Text_DateRange").ToString()%>';

    var mgmtReport_ReportFilter_js_MerchantNumber = '<%= GetLocalResourceObject("LiteralResource1.Text").ToString()%>';
    var mgmtReport_ReportFilter_js_MerchantName = '<%= GetLocalResourceObject("LiteralResource6.Text").ToString()%>';
    var funcValidate = 'ValidateData';
    
</script>
<script src="<%=ResolveUrl("~") %>res/js/risk_MCF/rm_MCF_MgmtReport_ReportFilter.js"></script>