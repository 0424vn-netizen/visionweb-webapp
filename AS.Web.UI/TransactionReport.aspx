<%@ Page Title="Daily/Monthly Transaction Report" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="TransactionReport.aspx.cs" Inherits="TransactionReport" meta:resourcekey="PageResource1" %>

<%@ Register Src="~/UserControls/PageTitle.ascx" TagName="PageTitle" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/ReportFiltering.ascx" TagName="ReportFiltering"
    TagPrefix="uc" %>
<%@ Register Src="~/UserControls/UxExport.ascx" TagName="UxExport" TagPrefix="uc" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="Server">
    <uc:PageTitle ID="uxPageTitle" runat="server" ReportTitle="DAILY/MONTHLY TRANSACTION REPORT" meta:resourcekey="uxPageTitleResource1" />

    <as:Container ID="uxContainer" runat="server" Width="100%" HeaderText="Filtering Options" FooterControlID="" FooterText="" HeaderControlID="" TemplateName="ascontainer_greyborder.tpl" meta:resourcekey="uxContainerResource1">
        <table style="width: 100%;" cellspacing="5">
            <tr style="vertical-align: bottom">
                <td style="width: 150px">
                    <div>
                        <as:RadioButton ID="uxDaily" runat="server" Text="Daily" GroupName="Date" Checked="true" onclick="ChangeDateOption(this)"
                            onkeypress="javascript:return false;" meta:resourcekey="uxDailyResource1" />
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <as:RadioButton ID="uxMonthly" runat="server" Text="Monthly" GroupName="Date" onclick="ChangeDateOption(this)"
                            onkeypress="javascript:return false;" meta:resourcekey="uxMonthlyResource1" />
                    </div>
                </td>
                <td style="width: 150px">
                    <div class="item" id="divDate">
                        <as:RadDatePicker ID="uxDate" Width="130" runat="server" Skin="Default" meta:resourcekey="uxDateResource1">
                            <Calendar FastNavigationStep="12" ShowRowHeaders="false" />
                            <DateInput ID="DateInput1" runat="server" onclick="ShowCalendar('1')" onkeypress="return SearchEnterOnTextbox(event);">
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

                <td align="left">
                    <as:Button ID="uxSearch" runat="server" Text="Search" OnClientClick="return ValidateData();"
                        OnClick="uxSearch_Click" meta:resourcekey="uxSearchResource1" />
                </td>
            </tr>
        </table>
    </as:Container>
    <br />
        <uc:UxExport ID="uxExporter" runat="server" GridID="uxReportGrid" />
        <as:ASGrid ID="uxReportGrid" runat="server" DDSPagingMethod="SPASingleMethod" AllowSortFilterWhenExport="true"
            Width="100%" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False"
            GridName="Transaction Report" VisiblePageTotal="false" CssClass="in" meta:resourcekey="uxReportGridResource1">
            <MasterTableView>
                <Columns>
                    <as:ASGridBoundColumn HeaderText="Merchant ID" DataField="MerchantNumber" SortExpression="MerchantNumber"
                        UniqueName="MerchantNumber" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource1">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Store Code" DataField="StoreCode" SortExpression="StoreCode"
                        UniqueName="StoreCode" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource2">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Report Date" DataField="BatchDate" SortExpression="BatchDate"
                        UniqueName="BatchDate" ASFormat="Date" meta:resourcekey="ASGridBoundColumnResource3">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="MC Cnt" DataField="MCCount" SortExpression="MCCount"
                        UniqueName="MCCount" HeaderTooltip="Total count of MC trans" ASFormat="Integer"
                        ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource4">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="MC Amt" DataField="MCAmount" SortExpression="MCAmount"
                        UniqueName="MCAmount" HeaderTooltip="Total net sales amount of MC trans" ASFormat="Currency"
                        ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource5">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Visa Cnt" DataField="VisaCount" SortExpression="VisaCount"
                        UniqueName="VisaCount" HeaderTooltip="Total count of Visa trans" ASFormat="Integer"
                        ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource6">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Visa Amt" DataField="VisaAmount" SortExpression="VisaAmount"
                        UniqueName="VisaAmount" HeaderTooltip="Total net sales amount of Visa trans"
                        ASFormat="Currency" ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource7">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Discover Cnt" DataField="DiscoverCount" SortExpression="DiscoverCount"
                        UniqueName="DiscoverCount" HeaderTooltip="Total count of Discover trans" ASFormat="Integer"
                        ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource8">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Discover Amt" DataField="DiscoverAmount" SortExpression="DiscoverAmount"
                        UniqueName="DiscoverAmount" HeaderTooltip="Total net sales amount of Discover trans"
                        ASFormat="Currency" ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource9">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="AMEX Cnt" DataField="AMEXCount" SortExpression="AMEXCount"
                        UniqueName="AMEXCount" HeaderTooltip="Total count of AMEX trans" ASFormat="Integer"
                        ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource10">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="AMEX Amt" DataField="AMEXAmount" SortExpression="AMEXAmount"
                        UniqueName="AMEXAmount" HeaderTooltip="Total net sales amount of AMEX trans"
                        ASFormat="Currency" ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource11">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Pin Debit Cnt" DataField="PinDebitCount" SortExpression="PinDebitCount"
                        UniqueName="PinDebitCount" HeaderTooltip="Total count of Pin Debit trans" ASFormat="Integer"
                        ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource12">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Pin Debit Amt" DataField="PinDebitAmount" SortExpression="PinDebitAmount"
                        UniqueName="PinDebitAmount" HeaderTooltip="Total net sales amount of Pin Debit trans"
                        ASFormat="Currency" ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource13">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Total Cnt" DataField="TotalCount" SortExpression="TotalCount"
                        UniqueName="TotalCount" HeaderTooltip="Total count of Total trans" ASFormat="Integer"
                        ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource14">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Total Amt" DataField="TotalAmount" SortExpression="TotalAmount"
                        UniqueName="TotalAmount" HeaderTooltip="Total net sales amount of Total trans"
                        ASFormat="Currency" ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource15">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                </Columns>
            </MasterTableView>
        </as:ASGrid>
    <uc:UxExport ID="uxExportBottom" runat="server" GridID="uxReportGrid" />

    <tek:RadCodeBlock ID="uxRadCodeBlock" runat="server">
        <script type="text/javascript">
            function ChangeDateOption(chk) {
                var now = new Date();
                var dateOpt = document.getElementById("divDate");
                dateOpt.style.display = "inline";
                var picker = $find("<%=uxDate.ClientID%>");
                if (picker.get_selectedDate() == null) {
                    picker.set_selectedDate(now);
                }
            }

            function CompareToday(datePicker, type) {
                var currentDate = new Date();
                var datePickerValue = datePicker.get_textBox().value;
                var isValid = isDate(datePickerValue);
                if (!isValid) {
                    alert("<%=Resources.MessageManager.ReportFilter_V1%>");
                    return false;
                }
                var selectedDate = new Date(datePickerValue);
                if (selectedDate > currentDate) {
                    var object;
                    switch (type) {
                        case 0: object = '<%= GetLocalResourceObject("uxDailyResource1.Text").ToString() %>'; break;
                        case 1: object = '<%= GetLocalResourceObject("uxMonthlyResource1.Text").ToString() %>'; break;
                    }
                    alert(String.format("<%=Resources.MessageManager.ReportFilter_V9%>", object));
                    return false;
                }
                return true;
            }

            function CheckDate() {

                var uxDate = $find("<%= uxDate.ClientID %>");

                //RadioButton           
                var uxDaily = document.getElementById("<%=uxDaily.ClientID %>");
                var uxMonthly = document.getElementById("<%=uxMonthly.ClientID %>");

                if (uxDaily && uxDaily.checked) {
                    if (uxDate.get_textBox().value != "") {
                        if (!CompareToday(uxDate, 0))
                            return false;
                    }
                    else {
                        alert('<%= GetLocalResourceObject("uxDailyResource1.Text").ToString() %>' + ": " + "<%=Resources.MessageManager.ReportFilter_ReportDate_InvaidDate %>");
                        return false;
                    }
                }
                else if (uxMonthly && uxMonthly.checked) {
                    if (uxDate.get_textBox().value != "") {
                        if (!CompareToday(uxDate, 1))
                            return false;
                    }
                    else {
                        alert('<%= GetLocalResourceObject("uxMonthlyResource1.Text").ToString() %>' + ": " + "<%=Resources.MessageManager.ReportFilter_ReportDate_InvaidDate %>");
                        return false;
                    }
                }
                else if (uxDateRange && uxDateRange.checked) {

                    if (!CompareToday(fromDate, 2))
                        return false;
                    if (!CompareToday(toDate, 2))
                        return false;

                    if (fromDate.get_selectedDate() > toDate.get_selectedDate()) {
                        alert("<%=Resources.MessageManager.ReportFilter_V3%>");
                        return false;
                    }
                }
        return true;
    }

    //Show calendar when user click on date text
    function ShowCalendar(type) {
        if (type == '1')
            $find("<%=uxDate.ClientID %>").showPopup();

    }
    function SearchEnterOnTextbox(e) {
        var isEnter = false;
        var isIE = /MSIE/.test(navigator.userAgent);
        var keyCode = isIE ? e.keyCode : e.which;
        isEnter = (keyCode == 13);
        if (isEnter) {
            document.getElementById("<%=uxSearch.ClientID%>").focus();
                    setTimeout('doClick()', 100);
                    return false;
                }
            }

            function doClick() {
                document.getElementById("<%=uxSearch.ClientID%>").click();
            }

            function ValidateData() {
                if (CheckDate())
                    return true;
                return false;
            }

            function master_closeModalEvent() {
                setTimeout('doClick()', 100);
            }
            var funcValidate = 'ValidateData';
        </script>
    </tek:RadCodeBlock>
</asp:Content>
