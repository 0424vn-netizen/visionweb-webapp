<%@ Page Title="CASE MANAGEMENT" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="CaseSearch.aspx.cs" Inherits="CaseSearch" meta:resourcekey="PageResource1" %>

<%@ Register TagName="PageTile" Src="~/UserControls/PageTitle.ascx" TagPrefix="uc" %>
<%@ Register TagName="Export" Src="~/UserControls/UxExport.ascx" TagPrefix="uc" %>
<%--<%@ Register TagName="SiteID_Selector" Src="~/UserControls/SiteID_Selector.ascx" TagPrefix="uc" %>--%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="Server">
    <tek:RadAjaxManagerProxy ID="uxRadAjaxManagerProxy" runat="server">
        <AjaxSettings>
            <tek:AjaxSetting AjaxControlID="uxReportGrid">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="uxReportGrid" />
                </UpdatedControls>
            </tek:AjaxSetting>
            <tek:AjaxSetting AjaxControlID="uxProxyButton">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="uxReportGrid" />
                    <tek:AjaxUpdatedControl ControlID="uxExporterTop" />
                    <tek:AjaxUpdatedControl ControlID="uxExporterBottom" />
                </UpdatedControls>
            </tek:AjaxSetting>
            <tek:AjaxSetting AjaxControlID="uxOpenNewTicket">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="hdRebind" />
                </UpdatedControls>
            </tek:AjaxSetting>
        </AjaxSettings>
    </tek:RadAjaxManagerProxy>
    <%-- ReportTitle --%>
    <uc:PageTile ID="uxPageTitle" runat="server" ReportTitle="CASE MANAGEMENT" meta:resourcekey="uxPageTitleResource1" />
    <table width="100%" cellpadding="0" cellspacing="0">
        <tr>
            <%--<td>
                &nbsp;<uc:SiteID_Selector runat="server" ID="uxSiteIDSelector"/>
            </td>--%>
            <td class="PanelGreyPadding" align="right">
                <as:Button runat="server" ID="uxIssueMaintenance" Text="Issue Maintenance" OnClientClick="goTo('IssueManagement.aspx');return false;" IsStandardButton="False" meta:resourcekey="uxIssueMaintenanceResource1" />
            </td>
        </tr>
    </table>
    <br />
    <%-- Filtering Options --%>
    <as:Container runat="server" ID="AsContainer1" Width="100%" HeaderText="Filtering Options" FooterControlID="" FooterText="" HeaderControlID="" TemplateName="ascontainer_greyborder.tpl" meta:resourcekey="AsContainer1Resource1">
        <div id="Div1" class="filter_center" style="overflow: hidden;" runat="server">
            <table width="100%" cellpadding="0" cellspacing="0" border="0">
                <tr>
                    <td>
                        <table cellspacing="0" border="0" cellspacing="0">
                            <tr valign="top">
                                <td style="padding-left: 6px">
                                    <as:Literal ID="ltAssignedTo" runat="server" Text="Assigned To:" meta:resourcekey="ltAssignedToResource1"></as:Literal>
                                    <a href="#" onclick="SelectAllOrNone('<%= uxFilterAssignedToList.ClientID %>', true); return false;">
                                        <as:Literal ID="ltAll" runat="server" Text="All" meta:resourcekey="ltAllResource1"></as:Literal></a> | <a href="#" onclick="SelectAllOrNone('<%= uxFilterAssignedToList.ClientID %>', false); return false;">
                                            <as:Literal ID="ltNone" runat="server" Text="None" meta:resourcekey="ltNoneResource1"></as:Literal></a>
                                    <br />
                                    <tek:RadListBox ID="uxFilterAssignedToList" runat="server" CheckBoxes="true" Width="200"
                                        Height="103" DataKeyField="KeyName" DataTextField="KeyValue" meta:resourcekey="uxFilterAssignedToListResource1">
                                        <ButtonSettings TransferButtons="All"></ButtonSettings>
                                    </tek:RadListBox>
                                </td>
                                <td style="padding-left: 5px">
                                    <as:Literal ID="ltStatus" runat="server" Text="Status:" meta:resourcekey="ltStatusResource1"></as:Literal>
                                    <a href="#" onclick="SelectAllOrNone('<%= uxFilterStatusList.ClientID %>', true); return false;">
                                        <as:Literal ID="ltStatusAll" runat="server" Text="All" meta:resourcekey="ltStatusAllResource1"></as:Literal></a> | <a href="#" onclick="SelectAllOrNone('<%= uxFilterStatusList.ClientID %>', false); return false;">
                                            <as:Literal ID="ltStatusNone" runat="server" Text="None" meta:resourcekey="ltStatusNoneResource1"></as:Literal></a>
                                    <br />
                                    <tek:RadListBox ID="uxFilterStatusList" runat="server" CheckBoxes="true" Width="200"
                                        Height="103" DataKeyField="KeyName" DataTextField="KeyValue" meta:resourcekey="uxFilterStatusListResource1">
                                        <ButtonSettings TransferButtons="All"></ButtonSettings>
                                    </tek:RadListBox>
                                </td>
                                <td style="padding-left: 5px">
                                    <as:Literal ID="ltResolution" runat="server" Text="Resolution:" meta:resourcekey="ltResolutionResource1"></as:Literal>
                                    <a href="#" onclick="SelectAllOrNone('<%= uxFilterResolutionList.ClientID %>', true); return false;">
                                        <as:Literal ID="ltResolutionAll" runat="server" Text="All" meta:resourcekey="ltResolutionAllResource1"></as:Literal></a> | <a href="#" onclick="SelectAllOrNone('<%= uxFilterResolutionList.ClientID %>', false); return false;">
                                            <as:Literal ID="ltResolutionNone" runat="server" Text="None" meta:resourcekey="ltResolutionNoneResource1"></as:Literal></a>
                                    <br />
                                    <tek:RadListBox ID="uxFilterResolutionList" runat="server" CheckBoxes="true" Width="200"
                                        Height="103" DataKeyField="KeyName" DataTextField="KeyValue" meta:resourcekey="uxFilterResolutionListResource1">
                                        <ButtonSettings TransferButtons="All"></ButtonSettings>
                                    </tek:RadListBox>
                                </td>
                                <td style="padding-left: 5px;">
                                    <br />
                                    <table cellpadding="0" cellspacing="0" border="0">
                                        <tr>
                                            <td>
                                                <tek:RadComboBox ID="uxFilterOpenClosed" runat="server" EnableEmbeddedSkins="false"
                                                    EnableEmbeddedBaseStylesheet="false" OnClientSelectedIndexChanged="uxFilterOpenClosed_OnClientSelectedIndexChanged"
                                                    xValuesReqFromToDates="[OpenBetween][ClosedBetween]" DataValueField="KeyValue" DataTextField="KeyName" meta:resourcekey="uxFilterOpenClosedResource1">
                                                </tek:RadComboBox>
                                            </td>
                                            <td width="10">&nbsp;
                                            </td>
                                            <td valign="top">
                                                <div id="uxOpenCloseDateRange" style="height: 100%; visibility: hidden;">
                                                    <as:RadDatePicker ID="uxOpenCloseFromDate" runat="server" Width="100px" ShowPopupOnFocus="true" meta:resourcekey="uxOpenCloseFromDateResource1">
                                                        <Calendar ID="Calendar1" FastNavigationStep="12" ShowRowHeaders="false" runat="server">
                                                        </Calendar>
                                                        <DateInput ID="DateInput1" DateFormat="MM/dd/yyyy" runat="Server" onkeypress="return DefaultEnterOnTextBox(event);">
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
                                                    <as:Literal ID="ltAnd" runat="server" Text="And" meta:resourcekey="ltAndResource1"></as:Literal>
                                                    <as:RadDatePicker ID="uxOpenCloseToDate" runat="server" Width="100px" ShowPopupOnFocus="true" meta:resourcekey="uxOpenCloseToDateResource1">
                                                        <Calendar ID="Calendar2" FastNavigationStep="12" ShowRowHeaders="false" runat="server">
                                                        </Calendar>
                                                        <DateInput ID="DateInput2" DateFormat="MM/dd/yyyy" runat="Server" onkeypress="return DefaultEnterOnTextBox(event);">
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
                                        </tr>
                                    </table>
                                    <table cellpadding="0" cellspacing="0" border="0">
                                        <tr>
                                            <td id="uxFilterContainer" runat="server">
                                                <as:RadioButton ID="uxFilterNone" runat="server" Text="None" GroupName="uxFilterSearch"
                                                    onclick="uxFilterSearchValue_Checked()" Checked="true" xKeyType="" meta:resourcekey="uxFilterNoneResource1" />
                                                <as:RadioButton ID="uxFilterTicketNumber" runat="server" Text="Ticket Number" GroupName="uxFilterSearch"
                                                    onclick="uxFilterSearchValue_Checked()" xKeyType="TNO" meta:resourcekey="uxFilterTicketNumberResource1" />
                                                <as:RadioButton ID="uxFilterMerchantNumber" runat="server" Text="Merchant ID"
                                                    GroupName="uxFilterSearch" onclick="uxFilterSearchValue_Checked()" xKeyType="MNO" meta:resourcekey="uxFilterMerchantNumberResource1" />
                                                <as:RadioButton ID="uxFilterMerchantName" runat="server" Text="Merchant Name" GroupName="uxFilterSearch"
                                                    onclick="uxFilterSearchValue_Checked()" xKeyType="MNAME" meta:resourcekey="uxFilterMerchantNameResource1" />
                                            </td>
                                            <td width="10">&nbsp;
                                            </td>
                                            <td>
                                                <div id="uxFilterSearchKey" style="height: 30px; visibility: hidden; padding-top: 10px;">
                                                    <as:TextBox ID="uxFilterSearchKeyText" runat="server" Width="120" onkeypress="return DefaultEnterOnTextBox(event);" meta:resourcekey="uxFilterSearchKeyTextResource1" />
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                    <br />
                                    <table>
                                        <tr>
                                            <td>
                                                <as:Button ID="uxSearchButton" runat="server" Text="Search" OnClientClick="return ValidateData();" meta:resourcekey="uxSearchButtonResource1" />
                                                <as:Button ID="uxProxyButton" runat="server" IsStandardButton="true" OnClick="uxSearchButton_OnClick" Style="visibility: hidden; display: none;" meta:resourcekey="uxProxyButtonResource1" />
                                            </td>
                                            <td style="padding-left: 10px;">
                                                <as:Button runat="server" ID="uxOpenNewTicket" Text="Open A New Ticket" OnClick="uxOpenNewTicket_Click" meta:resourcekey="uxOpenNewTicketResource1" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
    </as:Container>
    <br />
    <uc:Export ID="uxExporterTop" runat="server" GridID="uxReportGrid" />
        <as:ASGrid ID="uxReportGrid" runat="server" GridLines="None" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False"
            ASPagingMethod="SPASingleMethod" AllowSortFilterWhenExport="true" CssClass="in" meta:resourcekey="uxReportGridResource1">
            <MasterTableView>
                <Columns>
                    <as:ASGridBoundColumn HeaderText="Ticket" DataField="TicketNumber" SortExpression="TicketNumber"
                        UniqueName="TicketNumber" ASFormat="StaticString" HeaderTooltip="Ticket Number" meta:resourcekey="ASGridBoundColumnResource1">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Merchant ID" DataField="MerchantNumber" SortExpression="MerchantNumber"
                        UniqueName="MerchantNumber" ASFormat="StaticString" HeaderTooltip="Merchant ID" meta:resourcekey="ASGridBoundColumnResource2">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Merchant Name" DataField="MerchantName" SortExpression="MerchantName"
                        UniqueName="MerchantName" HeaderTooltip="Merchant Name" ASFormat="DynamicString" AllowEncodeOnExporting="true" meta:resourcekey="ASGridBoundColumnResource3">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Current Status" DataField="CurrentStatus" SortExpression="CurrentStatus"
                        UniqueName="CurrentStatus" HeaderTooltip="Current Status" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource4">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Resolution" DataField="Resolution" SortExpression="Resolution"
                        UniqueName="Resolution" HeaderTooltip="Resolution" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource5">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Assigned To" DataField="AssignedTo" SortExpression="AssignedTo"
                        UniqueName="AssignedTo" ASFormat="StaticString" HeaderTooltip="Assigned To" AllowEncodeOnExporting="true" meta:resourcekey="ASGridBoundColumnResource6">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Open Date" DataField="OpenedDate" SortExpression="OpenedDate"
                        UniqueName="OpenedDate" ASFormat="DateAndTime12Hours" HeaderTooltip="Open Date" meta:resourcekey="ASGridBoundColumnResource7">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Closed Date" DataField="ClosedDate" SortExpression="ClosedDate"
                        UniqueName="ClosedDate" ASFormat="DateAndTime12Hours" HeaderTooltip="Closed Date" meta:resourcekey="ASGridBoundColumnResource8">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Last Update" DataField="LastUpdatedDate" SortExpression="LastUpdatedDate"
                        UniqueName="LastUpdatedDate" ASFormat="DateAndTime12Hours" HeaderTooltip="Last Update" meta:resourcekey="ASGridBoundColumnResource9">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                </Columns>
            </MasterTableView>
        </as:ASGrid>
    <uc:Export ID="uxExporterBottom" runat="server" IsBottom="true" GridID="uxReportGrid" />
    <as:HiddenField ID="hdRebind" Value="0" runat="server" />

    <tek:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <script language="javascript" type="text/javascript">
            function goTo(url) {
                window.location = url;
            }

            //Search when user focus on textbox
            function DefaultEnterOnTextBox(e) {
                var isEnter = false;
                var isIE = /MSIE/.test(navigator.userAgent);
                var keyCode = isIE ? e.keyCode : e.which;
                isEnter = (keyCode == 13);
                uxSearchButton = document.getElementById('<%=uxSearchButton.ClientID %>');
                if (isEnter) {
                    if (uxSearchButton) {
                        uxSearchButton.focus();
                        if (isIE)
                            setTimeout("uxSearchButton.click()", 200);
                        else
                            uxSearchButton.click();
                    }
                    return false;
                }
            }

            function LimitText(fieldObj, maxChars) {
                var result = true;
                var text = document.getElementById(fieldObj);
                if (text.value.length >= maxChars) { result = false; }
                if (window.event) { window.event.returnValue = result; return result; }
            }
            function uxFilterOpenClosed_OnClientSelectedIndexChanged(sender, args) {
                //var ocFilterValue = args.get_item().get_value();
                var filterOpenClosedValue = $find('<%= uxFilterOpenClosed.ClientID %>').get_value();

                $get("uxOpenCloseDateRange").style.visibility = (filterOpenClosedValue == "OpenedBetween"
                || filterOpenClosedValue == "ClosedBetween" ? "visible" : "hidden");
                if ($find('<%=uxOpenCloseFromDate.ClientID %>') && resetvalue)
                    $find('<%=uxOpenCloseFromDate.ClientID %>').set_selectedDate(new Date());
                if ($find('<%=uxOpenCloseToDate.ClientID %>') && resetvalue)
                    $find('<%=uxOpenCloseToDate.ClientID %>').set_selectedDate(new Date());
            }

            function uxFilterSearchValue_Checked() {
                var noneValue = $get('<%= uxFilterNone.ClientID %>').checked;
                var ticketNumberValue = $get('<%= uxFilterTicketNumber.ClientID %>').checked;
                var merchantNumberValue = $get('<%= uxFilterMerchantNumber.ClientID %>').checked;
                var merchantNameValue = $get('<%= uxFilterMerchantName.ClientID %>').checked;

                if (ticketNumberValue)
                    document.getElementById('<%= uxFilterSearchKeyText.ClientID %>').maxLength = 9;
                else if (merchantNumberValue)
                    document.getElementById('<%= uxFilterSearchKeyText.ClientID %>').maxLength = 16;
                else if (merchantNameValue)
                    document.getElementById('<%= uxFilterSearchKeyText.ClientID %>').maxLength = 100;
        $get("uxFilterSearchKey").style.visibility = (!noneValue && (ticketNumberValue || merchantNumberValue || merchantNameValue) ? "visible" : "hidden");
        if (document.getElementById('<%= uxFilterSearchKeyText.ClientID %>') && resetvalue && !noneValue) {
            document.getElementById('<%= uxFilterSearchKeyText.ClientID %>').value = '';
            document.getElementById('<%= uxFilterSearchKeyText.ClientID %>').focus();
        }
        resetvalue = true;
    }

    function SelectAllOrNone(id, checked) {
        var listBox = $find(id);

        listBox.trackChanges();

        for (var idx = 0; idx < listBox.get_items().get_count() ; idx++) {
            var item = listBox.get_items().getItem(idx);
            item.set_checked(checked);
        }

        listBox.commitChanges();
    }


    function rf_ValidateFormat(input, validHexValue) {
        if (input.search(validHexValue) == -1) {
            return false;
        }
        return true;
    }
    function ValidateData() {
        var filterOpenClosedValue = $find('<%= uxFilterOpenClosed.ClientID %>').get_value();
                var ticketNumberValue = $get('<%= uxFilterTicketNumber.ClientID %>').checked;
                var merchantNumberValue = $get('<%= uxFilterMerchantNumber.ClientID %>').checked;
                var merchantNameValue = $get('<%= uxFilterMerchantName.ClientID %>').checked;
                var searchKeyTextValue = document.getElementById('<%= uxFilterSearchKeyText.ClientID %>').value.trim();
                var openCloseFromDate = $find('<%= uxOpenCloseFromDate.ClientID %>').get_selectedDate();
                var openCloseToDate = $find('<%= uxOpenCloseToDate.ClientID %>').get_selectedDate();
                var numberOnlyReg = /^\s*\d+\s*$/;
                if (filterOpenClosedValue == "OpenBetween" || filterOpenClosedValue == "ClosedBetween") {
                    var dF = new Date(openCloseFromDate);
                    var dT = new Date(openCloseToDate);

                    if (dF > new Date() || dT > new Date()) {
                        alert('<%= GetLocalResourceObject("CaseSearch_aspx_DateMustBeGreaterThanToday").ToString() %>');
                        return false;
                    }
                    else if (dF > dT) {
                        alert('<%= GetLocalResourceObject("CaseSearch_aspx_DateMustBeGreaterThanBegin").ToString() %>');
                            return false;
                        }
                }

                if (ticketNumberValue && !rf_ValidateFormat(searchKeyTextValue, numberOnlyReg)) {
                    alert('\'' + '<%= GetLocalResourceObject("uxFilterTicketNumberResource1.Text").ToString() %>' + '\' ' + '<%= GetLocalResourceObject("CaseSearch_aspx_MustBeEnteredAndNumber").ToString() %>');
                    document.getElementById('<%= uxFilterSearchKeyText.ClientID %>').focus();
                    return false;
                }
                else if (merchantNumberValue && !rf_ValidateFormat(searchKeyTextValue, numberOnlyReg)) {
                    alert('\'' + '<%= GetLocalResourceObject("uxFilterMerchantNumberResource1.Text").ToString() %>' + '\' ' + '<%= GetLocalResourceObject("CaseSearch_aspx_MustBeEnteredAndNumber").ToString() %>');
                    document.getElementById('<%= uxFilterSearchKeyText.ClientID %>').focus();
                    return false;
                }

                else if (merchantNameValue && searchKeyTextValue.length == 0) {
                    alert('\'' + '<%= GetLocalResourceObject("uxFilterMerchantNameResource1.Text").ToString() %>' + '\' ' + '<%= GetLocalResourceObject("CaseSearch_aspx_MustBeEntered").ToString() %>');
                    document.getElementById('<%= uxFilterSearchKeyText.ClientID %>').focus();
                    return false;
                }

        document.getElementById('<%=uxProxyButton.ClientID %>').click();
                return false;
            }
            var resetvalue = false;
            setTimeout("uxFilterOpenClosed_OnClientSelectedIndexChanged();uxFilterSearchValue_Checked();", 500);
            function ajaxRequestStart(sender, args) {
                if (args.get_eventTarget().indexOf('uxExporterTop') != -1 || args.get_eventTarget().indexOf('uxExporterBottom') != -1) {
                    args.set_enableAjax(false);
                }
            }
        </script>
    </tek:RadCodeBlock>

</asp:Content>
