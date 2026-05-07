<%@ Page Language="C#" MasterPageFile="~/MasterPagePopup.master" AutoEventWireup="true"
    CodeFile="Message_PastMessage.aspx.cs" Inherits="gen_Message_PastMessage"
    Title="Past Message" meta:resourcekey="PageResource1" %>

<%@ Register TagName="MessageGrid" Src="~/UserControls/MessageGrid.ascx" TagPrefix="uc" %>
<%@ Register TagName="PageTitle" Src="~/UserControls/PageTitle.ascx" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="Server">
    <div style="padding: 0px 20px 0px 20px; width: 95%">
        <uc:PageTitle ID="uxPageTitle" ReportTitle="Past Messages" runat="server" meta:resourcekey="uxPageTitleResource1" />
        <table>
            <tr>

                <td style="padding: 0px 2px 0px 0px" align="left">

                    <as:Container runat="server" ID="uxFilteringTable" Width="100%" HeaderText="Past Messages" FooterControlID="" FooterText="" HeaderControlID="" TemplateName="ascontainer_greyborder.tpl" meta:resourcekey="uxFilteringTableResource1">
                        <table cellpadding="0" cellspacing="0" width="100%">
                            <tr style="height: 35px; vertical-align: text-top;">
                                <td style="width: 30px"></td>
                                <td colspan="5" align="left">
                                    <i>
                                        <as:Literal ID="ltSelectSomething" runat="server" Text="Select a date range using the calendar dropdowns,then press search button to view
                                        messages received between those dates."
                                            meta:resourcekey="ltSelectSomethingResource1"></as:Literal></i>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 30px"></td>
                                <td width="120px" align="left">
                                    <as:Literal ID="ltStartDate" runat="server" Text="Start Date:" meta:resourcekey="ltStartDateResource1"></as:Literal>
                                </td>
                                <td>
                                    <div style="width: 182px; float: left;">
                                        <as:RadDatePicker ID="uxStarting" Style="vertical-align: middle;" runat="server"
                                            Width="182px" meta:resourcekey="uxStartingResource1">
                                            <Calendar ID="Calendar1" FastNavigationStep="12" runat="server">
                                            </Calendar>
                                            <DateInput ID="cidDateFrom" runat="server" onclick="ShowCalendar('1')" onkeypress="return DefaultEnterOnTextBox(event);">
                                                <EmptyMessageStyle Resize="None"></EmptyMessageStyle>

                                                <ReadOnlyStyle Resize="None"></ReadOnlyStyle>

                                                <FocusedStyle Resize="None"></FocusedStyle>

                                                <DisabledStyle Resize="None"></DisabledStyle>

                                                <InvalidStyle Resize="None"></InvalidStyle>

                                                <HoveredStyle Resize="None"></HoveredStyle>

                                                <EnabledStyle Resize="None"></EnabledStyle>
                                            </DateInput>

                                            <DatePopupButton CssClass="" ImageUrl="" HoverImageUrl=""></DatePopupButton>
                                        </as:RadDatePicker>
                                    </div>
                                </td>
                                <td width="120px">
                                    <as:Literal ID="Literal1" runat="server" Text="EndDate:" meta:resourcekey="Literal1Resource1"></as:Literal>
                                </td>
                                <td>
                                    <div style="width: 182px; display: inline; float: left;">
                                        <as:RadDatePicker ID="uxEnding" Style="vertical-align: middle;" runat="server" Width="182px" meta:resourcekey="uxEndingResource1">
                                            <Calendar ID="Calendar2" FastNavigationStep="12" runat="server">
                                            </Calendar>
                                            <DateInput ID="cidDateTo" runat="server" onclick="ShowCalendar('2')" onkeypress="return DefaultEnterOnTextBox(event);">
                                                <EmptyMessageStyle Resize="None"></EmptyMessageStyle>

                                                <ReadOnlyStyle Resize="None"></ReadOnlyStyle>

                                                <FocusedStyle Resize="None"></FocusedStyle>

                                                <DisabledStyle Resize="None"></DisabledStyle>

                                                <InvalidStyle Resize="None"></InvalidStyle>

                                                <HoveredStyle Resize="None"></HoveredStyle>

                                                <EnabledStyle Resize="None"></EnabledStyle>
                                            </DateInput>

                                            <DatePopupButton CssClass="" ImageUrl="" HoverImageUrl=""></DatePopupButton>
                                        </as:RadDatePicker>
                                    </div>
                                </td>
                                <td width="500px" align="left">
                                    <as:Button ID="uxBtnSearch" runat="server" Text="Search" OnClientClick="return validate(); "
                                        OnClick="uxBtnSearch_Click" meta:resourcekey="uxBtnSearchResource1" />
                                </td>
                            </tr>
                        </table>
                    </as:Container>
                </td>
            </tr>
            <tr>
                <td>
                    <table cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                            <td>
                                <uc:MessageGrid ID="uxMessageGrid" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <as:Button ID="uxbtnClose" runat="Server" Text="Close" Width="90px" OnClientClick=" window.close();" IsStandardButton="False" meta:resourcekey="uxbtnCloseResource1" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    <as:RadAjaxManagerProxy runat="server" ID="uxRadAjaxManager">
        <AjaxSettings>
            <tek:AjaxSetting AjaxControlID="uxBtnSearch">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="uxBtnSearch" />
                    <tek:AjaxUpdatedControl ControlID="uxMessageGrid" />
                </UpdatedControls>
            </tek:AjaxSetting>
        </AjaxSettings>
    </as:RadAjaxManagerProxy>
    <as:RadCodeBlock runat="server" ID="RadCodeBlock">

        <script type="text/javascript" language="javascript">
            function ajaxRequestStart(sender, args) {
                UxExporter_OnRequestStart(sender, args);
            }
            function ajaxOnResponseEnd(sender, args) {
                UxExporter_OnResponseEnd(sender, args);
            }

            function ShowCalendar(type) {
                if (type == "1")
                    $find("<%=uxStarting.ClientID %>").showPopup();
                if (type == "2")
                    $find("<%=uxEnding.ClientID %>").showPopup();
            }
            function DefaultEnterOnTextBox(e) {
                var isEnter = false;
                var isIE = /MSIE/.test(navigator.userAgent);
                var keyCode = isIE ? e.keyCode : e.which;
                isEnter = (keyCode == 13);
                if (isEnter) {
                    document.getElementById("<%=uxBtnSearch.ClientID%>").focus();
                    document.getElementById("<%=uxBtnSearch.ClientID%>").click();
                    return false;
                }
            }
            try {
                shortcut("enter", function() { document.getElementById("<%=uxBtnSearch.ClientID%>").click(); });
            } catch (ex) { };

            function validate() {
                var uxStarting = document.getElementById("<%=uxStarting.ClientID %>");
                var uxEnding = document.getElementById("<%=uxEnding.ClientID %>");
                //var pickerbeginDate = $find("<%=uxStarting.ClientID %>").get_selectedDate();
                begindate = uxStarting.value;
                enddate = uxEnding.value;
                if (uxStarting.value == "" || uxEnding.value == "") {
                    alert("<%=Resources.MessageManager.Generic_RequireDate%>");
                    return false;
                }
                var today = new Date();
                if (begindate > enddate) {
                    alert("<%=Resources.MessageManager.Generic_BeginDateGreaterEndDate%>");
                    var fromDateID = "<%=uxStarting.ClientID %>" + "_dateInput_text";
                    document.getElementById(fromDateID).style.color = "#e95005";
                    document.getElementById(fromDateID).style.border = "1px solid #e95005";
                    return false;
                }
                /*
                if (pickerbeginDate > today) {
                alert("<%=Resources.MessageManager.Generic_BeginDateGreaterNowDate%>");
                return false;
            }
            */

            if (begindate > today) {
                alert("<%=Resources.MessageManager.Generic_BeginDateGreaterNowDate%>");
                    return false;
                }

                __doPostBack('<%=uxBtnSearch.UniqueID %>');
            return true;

            //                if (Page_ClientValidate()) {
            //                    
            //                    //WebForm_DoPostBackWithOptions(new WebForm_PostBackOptions('ctl00$ContentPage$uxBtnSearch', '', true, '', '', false, true));
            //                    //return true;
            //                }
            //                return false;
            }
        </script>

    </as:RadCodeBlock>
</asp:Content>
