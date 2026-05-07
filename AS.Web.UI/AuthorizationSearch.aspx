<%@ Page Title="Authorization" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="AuthorizationSearch.aspx.cs" Inherits="AuthorizationSearch" meta:resourcekey="PageResource1" %>

<%@ Register Src="~/UserControls/PageTitle.ascx" TagName="PageTitle" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/UxExport.ascx" TagName="UxExport" TagPrefix="uc" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="Server">
    <style type="text/css">
        .left_col {
            border-right: 1px solid #7f9db9;
            text-align: right;
            padding-right: 7px;
        }

        .inline {
            display: inline;
            height: 25px;
        }

        .red {
            color: Red;
        }

        .padding_top {
            padding-top: 3px;
        }

        .margin_top {
            margin-top: 7px;
        }

        html body .RadInput .riTextBox, html body .RadInputMgr {
            padding: 4px 1px;
        }

        input.normal[type="text"],
        input.normal[type="password"],
        .filteringInputs input[type="text"],
        input.disable[type="text"], input.disable[type="password"] {
            padding-left: 1px;
            padding-right: 1px;
        }
    </style>
    <uc:PageTitle ID="uxPageTitle" runat="server" ReportTitle="Tech Console - Authorization" meta:resourcekey="uxPageTitleResource1" />
    <as:Container ID="uxContainer" runat="server" HeaderText="Filtering Options" Width="100%" meta:resourcekey="uxContainerResource1">
        <table style="width: 100%;" cellspacing="10">
            <colgroup>
                <col width="35%" />
                <col width="18%" />
                <col width="10%" />
                <col />
            </colgroup>
            <tr>
                <td class="left_col">
                    <div>
                        <as:Literal ID="ltAuthDate" runat="server" Text="Auth Date" meta:resourcekey="ltAuthDateResource1"></as:Literal>
                    </div>
                    <div style="display: none;">
                        <as:RadioButton ID="uxDaily" runat="server" Text="Daily" GroupName="Date" onclick="ChangeDateOption(this)"
                            onkeypress="javascript:return false;" meta:resourcekey="uxDailyResource1" />
                        <as:RadioButton ID="uxMonthly" runat="server" Text="Monthly" GroupName="Date" onclick="ChangeDateOption(this)"
                            onkeypress="javascript:return false;" meta:resourcekey="uxMonthlyResource1" />
                        <as:RadioButton ID="uxRange" runat="server" Checked="true" Text="Date range" GroupName="Date" onclick="ChangeDateOption(this)"
                            onkeypress="javascript:return false;" meta:resourcekey="uxRangeResource1" />
                    </div>
                </td>
                <td colspan="3">
                    <div id="divDate" style="width: 130px; display: none; visibility: hidden;" class="inline">
                        <as:RadDatePicker ID="uxDate" Width="130" runat="server" Skin="Default" meta:resourcekey="uxDateResource1">
                            <Calendar FastNavigationStep="12" ShowRowHeaders="false" />
                            <DateInput ID="DateInput1" runat="server" onclick="ShowCalendar('1')">
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
                    <div id="divDateRange">
                        <div class="inline" style="vertical-align: middle;">
                            <as:Literal ID="ltFrom" runat="server" Text="From:" meta:resourcekey="ltFromResource1"></as:Literal>
                        </div>
                        <div class="inline">
                            <as:RadDatePicker ID="uxFromDate" Width="150" runat="server" meta:resourcekey="uxFromDateResource1">
                                <Calendar FastNavigationStep="12" ShowRowHeaders="false" />
                                <DateInput ID="DateInput2" runat="server" onclick="ShowCalendar('2')">
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
                        <div class="inline" style="vertical-align: middle; padding-left: 10px;">
                            <as:Literal ID="ltTo" runat="server" Text="To:" meta:resourcekey="ltToResource1"></as:Literal>
                        </div>
                        <div class="inline">
                            <as:RadDatePicker ID="uxEndDate" Width="150" runat="server" Skin="Default" meta:resourcekey="uxEndDateResource1">
                                <Calendar FastNavigationStep="12" ShowRowHeaders="false" />
                                <DateInput ID="DateInput3" runat="server" onclick="ShowCalendar('3')">
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
                    </div>
                </td>
            </tr>
            <tr>
                <td class="left_col">
                    <div>
                        <as:RadioButton ID="uxRadMerchantNumber" runat="server" Checked="true" Text="Merchant ID" GroupName="SearchOption" onclick="ChangeSearchOption(this,false)"
                            onkeypress="javascript:return false;" meta:resourcekey="uxRadMerchantNumberResource1" />&nbsp;
                        <as:RadioButton ID="uxRadMerchantName" runat="server" Text="Merchant Name" GroupName="SearchOption"
                            onclick="ChangeSearchOption(this,false)" onkeypress="javascript:return false;" meta:resourcekey="uxRadMerchantNameResource1" />&nbsp;
                        <as:RadioButton ID="uxRadAccountNumber" runat="server" Text="Account Number"
                            GroupName="SearchOption" onclick="ChangeSearchOption(this,false)" onkeypress="javascript:return false;" meta:resourcekey="uxRadAccountNumberResource1" />
                    </div>
                </td>
                <td colspan="3">
                    <div class="inline" id="divMerchantNumber">
                        <as:TextBox ID="uxFilterValue" runat="server" Width="200" MaxLength="16" onclick="onKeyUp();"
                            onblur="onKeyUp();" onkeyup="onKeyUp();" onfocus="onKeyUp();" meta:resourcekey="uxFilterValueResource1" />
                    </div>

                    <div class="inline padding_top">
                        <span class="red option">
                            <as:Literal ID="ltRequire" runat="server" Text="*required" meta:resourcekey="ltRequireResource1"></as:Literal></span>
                    </div>
                </td>
            </tr>
            <tr>
                <td class="left_col">
                    <as:Literal ID="ltTerminalID" runat="server" Text="Terminal ID" meta:resourcekey="ltTerminalIDResource1"></as:Literal>
                </td>
                <td>
                    <div class="inline">
                        <as:TextBox ID="uxTerminalID" runat="server" Width="200" MaxLength="20" meta:resourcekey="uxTerminalIDResource1" />
                    </div>
                </td>
                <td class="left_col">
                    <as:Literal ID="ltAuthID" runat="server" Text="Auth ID" meta:resourcekey="ltAuthIDResource1"></as:Literal>
                </td>
                <td>
                    <div class="inline">
                        <as:TextBox ID="uxAuthID" runat="server" Width="200" MaxLength="16" meta:resourcekey="uxAuthIDResource1" />
                    </div>
                </td>
            </tr>
            <tr>
                <td class="left_col">
                    <as:Literal ID="ltPCD" runat="server" Text="Processing Code Description" meta:resourcekey="ltPCDResource1"></as:Literal>
                </td>
                <td>
                    <div class="inline">
                        <as:RadComboBox ID="uxProcCodeDes" runat="server" Width="204" DataValueField="DataKey" DataTextField="DataText" meta:resourcekey="uxProcCodeDesResource1"></as:RadComboBox>
                    </div>
                </td>
                <td class="left_col">
                    <as:Literal ID="ltAuthAmount" runat="server" Text="Auth Amount" meta:resourcekey="ltAuthAmountResource1"></as:Literal>
                </td>
                <td>
                    <div class="inline">
                        <tek:RadNumericTextBox ID="uxAuthAmount" runat="server" Type="Currency" Width="200" NegativeStyle-ForeColor="Red" meta:resourcekey="uxAuthAmountResource1">
                            <NegativeStyle Resize="None" ForeColor="Red"></NegativeStyle>

                            <NumberFormat ZeroPattern="$n"></NumberFormat>

                            <EmptyMessageStyle Resize="None"></EmptyMessageStyle>

                            <ReadOnlyStyle Resize="None"></ReadOnlyStyle>

                            <FocusedStyle Resize="None"></FocusedStyle>

                            <DisabledStyle Resize="None"></DisabledStyle>

                            <InvalidStyle Resize="None"></InvalidStyle>

                            <HoveredStyle Resize="None"></HoveredStyle>

                            <EnabledStyle Resize="None"></EnabledStyle>
                        </tek:RadNumericTextBox>
                    </div>
                </td>
            </tr>
            <tr>
                <td></td>
                <td align="left">
                    <div class="inline">
                        <as:Button ID="uxSearch" runat="server" Text="Search" OnClientClick="return ValidateData();" OnClick="uxSearch_Click" meta:resourcekey="uxSearchResource1" />
                    </div>
                </td>
                <td colspan="2"></td>
            </tr>

        </table>
    </as:Container>
    <br />
    <uc:UxExport ID="uxExporter" runat="server" GridID="uxReportGrid" GridHeader="Authorization Summary" meta:resourcekey="uxExporterResource1" />
    <as:ASGrid ID="uxReportGrid" runat="server" AutoGenerateColumns="false" AllowPaging="true"
        AllowSorting="true" ASPagingMethod="SPASingleMethod" ShowPageTotal="false" meta:resourcekey="uxReportGridResource1">
        <MasterTableView>
            <Columns>
                <as:ASGridBoundColumn HeaderText="Merchant ID" HeaderTooltip="Merchant ID" UniqueName="MerchantNumber"
                    DataField="MerchantNumber" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource1">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn HeaderText="Merchant Name" HeaderTooltip="Merchant Name" UniqueName="MerchantName" DataField="MerchantName" ASFormat="DynamicString" meta:resourcekey="ASGridBoundColumnResource2">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn HeaderText="Account #" HeaderTooltip="Account Number" UniqueName="AccountNumber" DataField="AccountNumber"
                    ASFormat="StaticString" SortExpression="PartialCardNumber" meta:resourcekey="ASGridBoundColumnResource3">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn HeaderText="Account #" HeaderTooltip="Account Number" UniqueName="PartialCardNumber"
                    DataField="PartialCardNumber" ASFormat="StaticString" SortExpression="PartialCardNumber" meta:resourcekey="ASGridBoundColumnResource4">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn HeaderText="Exp Date" HeaderTooltip="Expiration Date" UniqueName="ExpirationDate" DataField="ExpirationDate" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource5">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn HeaderText="Auth Date" HeaderTooltip="Authorization Date" UniqueName="AuthDate" DataField="AuthDate" ASFormat="Date" meta:resourcekey="ASGridBoundColumnResource6">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn HeaderText="Auth Time" HeaderTooltip="Authorization Time" UniqueName="AuthTime" DataField="AuthTime" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource7">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn HeaderText="Auth ID" HeaderTooltip="Authorization ID" UniqueName="AuthID" DataField="AuthID" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource8">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn HeaderText="Auth Amt" HeaderTooltip="Authorization Amount" UniqueName="AuthAmount" DataField="AuthAmount" ASFormat="Currency" ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource9">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn HeaderText="" HeaderTooltip="" UniqueName="CaptureDetail" ItemStyle-HorizontalAlign="Center" meta:resourcekey="ASGridBoundColumnResource10">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                </as:ASGridBoundColumn>

            </Columns>
        </MasterTableView>
    </as:ASGrid>
    <uc:UxExport ID="uxExporterBottom" runat="server" GridID="uxReportGrid" IsBottom="true" />
    <as:HiddenField ID="uxChangeOption" runat="server" />
    <tek:RadCodeBlock ID="uxRadCodeBlock" runat="server">

        <script type="text/javascript" language="javascript">
            function ValidateData() {
                if (!CheckDate()) {
                    return false;
                }

                var radMerchantNumber = document.getElementById('<%= uxRadMerchantNumber.ClientID %>');
                var radMerchantName = document.getElementById('<%= uxRadMerchantName.ClientID %>');
                var radAccountNumber = document.getElementById('<%= uxRadAccountNumber.ClientID %>');

                if (radMerchantNumber.checked) {
                    if (!CheckMerchantNumber()) {
                        return false;
                    }
                }

                if (radMerchantName.checked) {
                    if (!CheckMerchantName()) {
                        return false;
                    }
                }

                if (radAccountNumber.checked) {
                    if (!CheckCardNumber()) {
                        return false;
                    }
                }

                if (!CheckAlphaNumeric(_auth, '<%= GetLocalResourceObject("ltAuthIDResource1.Text").ToString() %>')) {
                    return false;
                }

                if (!CheckAlphaNumeric(_terminal, '<%= GetLocalResourceObject("ltTerminalIDResource1.Text").ToString() %>')) {
                    return false;
                }
              
                return true;
            }


            function onKeyUp(type) {
                if (trim(_filterValue.value) == '') {
                    $('.option').show();
                    return false;
                }
                if (trim(_filterValue.value) != '') {
                    $('.option').hide();
                    return true;
                }
            }
            
            
            function ShowCalendar(type) {
                if (type == '1')
                    $find("<%=uxDate.ClientID %>").showPopup();
                else if (type == '2')
                    $find("<%=uxFromDate.ClientID %>").showPopup();
                else
                    $find("<%=uxEndDate.ClientID %>").showPopup();
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
                    case 0: object = '<%=GetLocalResourceObject("uxDailyResource1.Text").ToString() %>'; break;
                    case 1: object = '<%=GetLocalResourceObject("uxMonthlyResource1.Text").ToString() %>'; break;
                    case 2: object = '<%=GetLocalResourceObject("ltAuthDateResource1.Text").ToString() %>'; break;
                }
                alert(String.format("<%=Resources.MessageManager.ReportFilter_V9%>", object));
                return false;
            }
            return true;
        }

        function CheckDate() {
            var beginDate = document.getElementById("<%=uxFromDate.ClientID %>");
                var endDate = document.getElementById("<%=uxEndDate.ClientID %>");

                var fromDate = $find("<%= uxFromDate.ClientID %>");
                var toDate = $find("<%= uxEndDate.ClientID %>");
                var uxDate = $find("<%= uxDate.ClientID %>");

                //RadioButton           
                var uxDaily = document.getElementById("<%=uxDaily.ClientID %>");
                var uxMonthly = document.getElementById("<%=uxMonthly.ClientID %>");
                var uxDateRange = document.getElementById("<%=uxRange.ClientID %>");

                beginDate.value = fromDate.get_textBox().value;
                endDate.value = toDate.get_textBox().value;

                if (uxDaily && uxDaily.checked) {
                    if (uxDate.get_textBox().value != "") {
                        if (!CompareToday(uxDate, 0))
                            return false;
                    }
                    else {
                        alert('<%=GetLocalResourceObject("uxDailyResource1.Text").ToString() %>' + ': ' + '<%=Resources.MessageManager.ReportFilter_ReportDate_InvaidDate %>');
                        return false;
                    }
                }
                else if (uxMonthly && uxMonthly.checked) {
                    if (uxDate.get_textBox().value != "") {
                        if (!CompareToday(uxDate, 1))
                            return false;
                    }
                    else {
                        alert('<%=GetLocalResourceObject("uxMonthlyResource1.Text").ToString() %>' + ': ' + '<%=Resources.MessageManager.ReportFilter_ReportDate_InvaidDate %>');
                        return false;
                    }
                }
                else if (uxDateRange && uxDateRange.checked) {
                    if (fromDate.get_textBox().value == "" || toDate.get_textBox().value == "") {
                        alert('<%=GetLocalResourceObject("ltAuthDateResource1.Text").ToString() %>' + ': ' + '<%=Resources.MessageManager.ReportFilter_ReportDate_InvaidDate %>');
                        return false;
                    }
                    if (!CompareToday(fromDate, 2))
                        return false;
                    if (!CompareToday(toDate, 2))
                        return false;

                    if (fromDate.get_selectedDate() > toDate.get_selectedDate()) {
                        alert('<%=GetLocalResourceObject("ltAuthDateResource1.Text").ToString() %>' + ': ' + '<%=Resources.MessageManager.ReportFilter_V3%>');
                        return false;
                    }
                }
        return true;
    }

    function ChangeSearchOption(chk, isPostBack) {
        var filterValue = document.getElementById('<%= uxFilterValue.ClientID %>');
        //filterValue.value = '';
        filterValue.focus();
        filterValue.select();
        //clear uxfiltervalue when change filter type
        //var isPostBack = <%= Page.IsPostBack ? "true" : "false" %>; 
        if (!isPostBack) {
            var uxfilterValue = $("#<%=uxFilterValue.ClientID %>");
            uxfilterValue.val("");
        }
                
        if (_option.value == '0') {
            if (chk.value == 'uxRadAccountNumber') {
                filterValue.setAttribute('maxLength', 4);
                filterValue.style.width = 100 + 'px';
            }
            else if (chk.value == 'uxRadMerchantName') {
                filterValue.removeAttribute('maxLength');
                filterValue.style.width = 200 + 'px';
            }
            else {
                filterValue.setAttribute('maxLength', 16);
                filterValue.style.width = 200 + 'px';
            }
        }
        if (_option.value == '1') {
                    
            if (chk.value == 'uxRadAccountNumber') {
                filterValue.setAttribute('maxLength', 16);
            }
            else if (chk.value == 'uxRadMerchantName') {
                filterValue.removeAttribute('maxLength');
            }
            else {
                filterValue.setAttribute('maxLength', 16);
            }
        }
    }
    function KeepDateOption() {
        var uxRad = document.getElementById("<%=uxRadMerchantName.ClientID %>");
                var isPostBack = <%= Page.IsPostBack ? "true" : "false" %>; 
                if (uxRad.checked) {
                    ChangeSearchOption(uxRad, isPostBack);
                }
            }
            
            
            

            var _filterValue;
            var _option;
            var _terminal;
            var _auth;
            var _textObj;
            function InitControl() {
                _option = document.getElementById('<%= uxChangeOption.ClientID %>');
                _filterValue = document.getElementById('<%= uxFilterValue.ClientID %>');
                _auth = document.getElementById('<%= uxAuthID.ClientID %>');
                _terminal = document.getElementById('<%= uxTerminalID.ClientID %>');
                _filterValue.focus();
            }

            function CheckMerchantNumber() {
                _textObj = '<%= GetLocalResourceObject("uxRadMerchantNumberResource1.Text").ToString() %>';
                _filterValue.value=trim(_filterValue.value);
                if (trim(_filterValue.value) == '') {
                    alert(String.format("<%=Resources.MessageManager.ReportFilter_V2%>", _textObj));
                    return false;
                }
                if (trim(_filterValue.value).length > 16) {
                    alert(String.format("<%=Resources.MessageManager.ReportFilter_V4 %>", _textObj, 16));
                    _filterValue.focus();
                    _filterValue.select();
                    return false;
                }
                var reg = /^[0-9]{1,16}$/;
                if (reg.test(trim(_filterValue.value)) == false) {
                    alert(String.format("<%=Resources.MessageManager.ReportFilter_V7 %>", _textObj));
                    _filterValue.focus();
                    _filterValue.select();
                    return false;
                }
                return true;
            }

            function CheckMerchantName() {
                _textObj = '<%= GetLocalResourceObject("uxRadMerchantNameResource1.Text").ToString() %>';
                document.getElementById('<%= uxFilterValue.ClientID %>').value=trim(_filterValue.value);
                if (trim(_filterValue.value) == '') {
                    alert(String.format("<%=Resources.MessageManager.ReportFilter_V2%>", _textObj));
                    return false;
                }

                //var reg = /^[ ,.A-Za-z0-9]*$/;
                var reg = /^((?!(\<|\>))(?!(\&\#)).)*$/g;
                if (reg.test(_filterValue.value) == false) {
                    alert(String.format("<%=Resources.MessageManager.ReportFilter_V6%>", _textObj));
                    _filterValue.focus();
                    _filterValue.select();
                    return false;
                }
                return true;
            }

            function CheckCardNumber() {

                if (_option.value == '1') {
                    _textObj = '<%= GetLocalResourceObject("uxRadAccountNumberResource1.Text").ToString() %>';
                    if (trim(_filterValue.value) == '') {
                        
                        alert(String.format("<%=Resources.MessageManager.ReportFilter_V2%>", _textObj));
                        _filterValue.focus();
                        _filterValue.select();
                        return false;
                    }
                    if (trim(_filterValue.value) != '') {
                        var reg = /^[0-9]*$/;
                        if (reg.test(_filterValue.value) == false) {
                            alert(String.format("<%=Resources.MessageManager.ReportFilter_V7 %>", _textObj));
                            _filterValue.focus();
                            _filterValue.select();
                            return false;
                        }

                        var reg1 = /.{15,16}/;
                        if (reg1.test(_filterValue.value) == false) {
                            alert(_textObj + ': ' + '<%= GetLocalResourceObject("Text_TheLengthFrom15To16").ToString() %>');
                            _filterValue.focus();
                            _filterValue.select();
                            return false;
                        }
                    }
                    return true;
                }
                if (_option.value == '0') {
                    if (trim(_filterValue.value) != '') {
                        _textObj = '<%= GetLocalResourceObject("Text_ANLast4").ToString() %>';
                        if (trim(_filterValue.value).length != 4) {
                            alert(String.format("<%=Resources.MessageManager.ReportFilter_V21 %>", _textObj, 4));
                            _filterValue.focus();
                            _filterValue.select();
                            return false;
                        }
                        var reg = /^[0-9]{4}$/;
                        if (reg.test(_filterValue.value) == false) {
                            alert(String.format("<%=Resources.MessageManager.ReportFilter_V7 %>", _textObj));
                            _filterValue.focus();
                            _filterValue.select();
                            return false;
                        }
                    }
                    if (trim(_filterValue.value) == '') {
                        _textObj = '<%= GetLocalResourceObject("Text_ANLast4").ToString() %>';
                        if (trim(_filterValue.value) == '') {
                            alert(String.format("<%=Resources.MessageManager.ReportFilter_V2%>", _textObj));
                            _filterValue.focus();
                            _filterValue.select();
                            return false;
                        }
                    }
                    return true;
                }
                return false;
            }

            function CheckAlphaNumeric(ctrl, textObj) {
                var reg = /^[a-zA-Z0-9]*$/;
                if (reg.test(ctrl.value) == false) {
                    alert(String.format("<%=Resources.MessageManager.ReportFilter_V6%>", textObj));
                    ctrl.select();
                    return false;
                }
                return true;
            }

            var funcValidate = 'ValidateData';

            $(document).ready(function() {
                InitControl();
                try {
                    KeepDateOption();
                }
                catch (ex) { }
                // Add event click enter from kb
                $('#<%= uxContainer.ClientID %> input[type=text]').keypress(function(e){
                    var code = e.keyCode || e.which;
                    if (code == 13) {
                        $('#<%= uxSearch.ClientID %>').click();
                        return false;
                    }
                });
            });
        </script>
    </tek:RadCodeBlock>
</asp:Content>

