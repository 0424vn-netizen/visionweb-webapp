<%@ Control Language="C#" AutoEventWireup="true" CodeFile="rm_MCF_RiskWorkLog_Filter.ascx.cs" Inherits="UserControls_rm_MCF_RiskWorkLog_Filter" %>

<div class="row collapse report-filter-panel">
    <div class="col-md-12 report-filter">
        <div class="filter-block">
            <div class="filter-block">
                <table>
                    <tr>
                        <td class="text-right">
                            <div class="filter-item text-nowrap">
                                <asp:RadioButton ID="uxDaily" runat="server" Text="Daily" GroupName="Date" onclick="ChangeDateOption(this)"
                                    onkeypress="javascript:return false;" CssClass="date-item" meta:resourcekey="uxDailyResource1" />
                                <asp:RadioButton ID="uxRange" runat="server" Text="Date Range" GroupName="Date" onclick="ChangeDateOption(this)"
                                    onkeypress="javascript:return false;" CssClass="date-item" meta:resourcekey="uxRangeResource1" />
                            </div>
                        </td>
                        <td colspan="2" class="text-left">
                            <div id="divDate">
                                <div class="filter-item">
                                    <as:RadDatePicker ID="uxDate" ShowPopupOnFocus="true" runat="server" Width="128px">
                                        <Calendar ID="Calendar1" FastNavigationStep="12" ShowRowHeaders="false" runat="server" />
                                        <DateInput ID="DateInput1" runat="server" onkeypress="return SearchEnterOnTextbox(event);" />
                                    </as:RadDatePicker>
                                </div>
                            </div>
                            <div id="divDateRange" style="display: none">
                                <div class="filter-item">
                                    <as:RadDatePicker ID="uxFromDate" ShowPopupOnFocus="true" runat="server" Width="128px">
                                        <Calendar ID="Calendar2" FastNavigationStep="12" ShowRowHeaders="false" runat="server" />
                                        <DateInput ID="DateInput2" runat="server" onkeypress="return SearchEnterOnTextbox(event);" />
                                    </as:RadDatePicker>
                                </div>
                                <div class="filter-item">
                                    <as:RadDatePicker ID="uxEndDate" ShowPopupOnFocus="true" runat="server" Width="128px">
                                        <Calendar ID="Calendar3" FastNavigationStep="12" ShowRowHeaders="false" runat="server" />
                                        <DateInput ID="DateInput3" runat="server" onkeypress="return SearchEnterOnTextbox(event);" />
                                    </as:RadDatePicker>
                                </div>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="text-right">
                            <label class="filter-label">
                                <asp:Literal ID="lblMerchantName" runat="server" meta:resourcekey="MerchantNameResource1"> Merchant Name:</asp:Literal></label>
                        </td>
                        <td class="text-left" colspan="2">
                            <div class="filter-item">
                                <div>
                                    <as:RadComboBox ID="uxMerchantName" runat="server" EnableEmbeddedBaseStylesheet="False"
                                        DataValueField="DataKey" DataTextField="DataText" Width="320px"
                                        AppendDataBoundItems="True" EnableLoadOnDemand="True" OnItemsRequested="uxMerchantName_OnItemsRequested"
                                        OnClientTextChange="uxMerchantName_OnClientTextChange"
                                        OnClientSelectedIndexChanged="uxMerchantName_OnClientSelectedIndexChanged"
                                        EmptyMessage="enter at least 3 letters of the merchant names"
                                        MaxLength="100" meta:resourcekey="MerchantNamePlaceHolderResource1">
                                        <Items>
                                            <tek:RadComboBoxItem runat="server" meta:resourcekey="RadComboBoxItemResource1" />
                                        </Items>
                                    </as:RadComboBox>
                                </div>
                            </div>
                        </td>

                    </tr>
                    <tr>
                        <td class="text-right">
                            <label class="filter-label">
                                <asp:Literal ID="Literal3" runat="server" Text="Merchant ID:" meta:resourcekey="MerchantIDResource1" /></label></td>
                        <td class="text-left">
                            <div class="filter-item">
                                <as:RadTextBox ID="uxMerchantID" runat="server" Width="320px" MaxLength="16"
                                    CssClass="rf_TextBox" LabelCssClass="" LabelWidth="64px" meta:resourcekey="uxMerchantNumberResource1" Resize="None">
                                    <EmptyMessageStyle Resize="None" />
                                    <ReadOnlyStyle Resize="None" />
                                    <FocusedStyle Resize="None" />
                                    <DisabledStyle Resize="None" />
                                    <InvalidStyle Resize="None" />
                                    <HoveredStyle Resize="None" />
                                    <EnabledStyle Resize="None" />
                                </as:RadTextBox>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="text-right">
                            <label class="filter-label">
                                <asp:Literal ID="Literal1" runat="server" meta:resourcekey="UserResource1"> User:</asp:Literal></label>
                        </td>
                        <td class="text-left" colspan="2">
                            <div class="filter-item">
                                <div>
                                    <as:RadComboBox ID="uxUser" runat="server" EnableEmbeddedBaseStylesheet="False"
                                        DataValueField="DataKey" DataTextField="DataText" Width="320"
                                        AppendDataBoundItems="True" EnableLoadOnDemand="True" OnItemsRequested="uxUserList_OnItemsRequested"
                                        EmptyMessage="enter at least 3 letters of the user"
                                        MaxLength="100" meta:resourcekey="UserPlaceHolderResource1">
                                        <Items>
                                            <tek:RadComboBoxItem runat="server" meta:resourcekey="RadComboBoxItemResource1" />
                                        </Items>
                                    </as:RadComboBox>
                                </div>
                            </div>
                        </td>

                    </tr>
                    <tr>
                        <td class="text-right">
                            <label class="filter-label">
                                <asp:Literal ID="Literal2" runat="server" meta:resourcekey="DQViewResource1"> DQ View:</asp:Literal></label>
                        </td>
                        <td class="text-left" colspan="2">
                            <div class="filter-item">
                                <as:RadComboBox ID="uxDQView" runat="server" Width="320px" EnableTextSelection="true" Filter="Contains" MarkFirstMatch="true"
                                    MaxHeight="230px" EnableEmbeddedSkins="false" EnableEmbeddedBaseStylesheet="false">
                                </as:RadComboBox>
                            </div>
                        </td>

                    </tr>
                    <tr>
                        <td class="text-right">
                            <label class="filter-label">
                                <asp:Literal ID="Literal4" runat="server" meta:resourcekey="AssignmentNameResource1"> Assignment Name:</asp:Literal></label>
                        </td>
                        <td class="text-left" colspan="2">
                            <div class="filter-item">
                                <as:RadComboBox ID="uxAssignmentName" runat="server" Height="150px" Width="320px" EnableTextSelection="true" Filter="Contains" MarkFirstMatch="true"
                                    MaxHeight="230px" EnableEmbeddedSkins="false" EnableEmbeddedBaseStylesheet="false">
                                </as:RadComboBox>
                            </div>
                        </td>

                    </tr>
                    <tr>
                        <td></td>
                        <td class="text-right" style="width: 250px">
                            <div class="filter-item">
                                <as:Button ID="uxSearch" runat="server" Text="Submit" OnClientClick="return ValidateData();"
                                    OnClick="uxSearch_Click" CssClass="btn btn-default" IsStandardButton="False" meta:resourcekey="uxSearchResource1" />
                            </div>
                        </td>
                        <td></td>

                    </tr>
                </table>
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

<as:RadCodeBlock ID="JavaScript" runat="server">

    <script type="text/javascript">
        var uxFromDate_ClientID = "<%=uxFromDate.ClientID%>";
        var uxEndDate_ClientID = "<%=uxEndDate.ClientID%>";
        var uxRange_ClientID = "<%=uxRange.ClientID %>";
        var uxDate_ClientID = "<%= uxDate.ClientID %>";

        var uxDaily_ClientID = "<%=uxDaily.ClientID %>";
        var uxRange_ClientID = "<%=uxRange.ClientID %>";
        var uxSearch_ClientID = "<%=uxSearch.ClientID%>";
        var uxDaily = document.getElementById(uxDaily_ClientID);
        var uxDateRange = document.getElementById(uxRange_ClientID);
        var rm_MCF_uxMerchantID = '<%= uxMerchantID.ClientID %>';
        var rm_MCF_uxMerchantName = '<%= uxMerchantName.ClientID %>';
        var rm_MCF_uxUser = '<%= uxUser.ClientID %>';
        var merchantName_js_ValidationSpecialCharacter = '<%= GetLocalResourceObject("merchantName_js_ValidationSpecialCharacter").ToString() %>';
        var merchantID_InvalidFormatMerchantID_js = '<%= GetLocalResourceObject("uxInvalidFormatMerchantID.Text").ToString() %>'; 

        var Msg_V1 = "<%=Resources.MessageManager.ReportFilter_V1%>";
        var Msg_V2 = "<%=Resources.MessageManager.ReportFilter_ReportDate_InvaidDate %>";
        var Msg_V3 = "<%=Resources.MessageManager.ReportFilter_V3%>";
        var Msg_V9 = "<%=Resources.MessageManager.ReportFilter_V9%>";
        var Msg_V10 = "<%=Resources.MessageManager.ReportFilter_V10%>";

        var uxDailyResource1_text = '<%= GetLocalResourceObject("uxDailyResource1.Text").ToString()%>';
        var uxRangeResource1_text = '<%= GetLocalResourceObject("uxRangeResource1.Text").ToString()%>';
    </script>
    <script src="<%=ResolveUrl("~") %>res/js/risk_MCF/rm_MCF_RiskWorkLog_Filter.js"></script>
</as:RadCodeBlock>
