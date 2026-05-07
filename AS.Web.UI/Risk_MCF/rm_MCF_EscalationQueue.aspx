<%@ Page Title="Escalation Queue" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="rm_MCF_EscalationQueue.aspx.cs" Inherits="rm_MCF_EscalationQueue" meta:resourcekey="PageResource1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPage" runat="Server">
    <tek:RadAjaxManagerProxy ID="RadAjaxManager1" runat="server">
        <AjaxSettings>
            <tek:AjaxSetting AjaxControlID="uxReportGrid">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="uxReportGrid" LoadingPanelID="uxLoadingPanelCustom" />
                    <tek:AjaxUpdatedControl ControlID="hdRebind" />
                </UpdatedControls>
            </tek:AjaxSetting>
            <tek:AjaxSetting AjaxControlID="uxProxyButton">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="uxReportGrid" LoadingPanelID="uxLoadingPanelCustom" />
                    <tek:AjaxUpdatedControl ControlID="uxExporterTop" />
                    <tek:AjaxUpdatedControl ControlID="hdRebind" />
                </UpdatedControls>
            </tek:AjaxSetting>
        </AjaxSettings>
    </tek:RadAjaxManagerProxy>
    <as:PlaceHolder ID="sdsf" runat="server">
        <!--Filtering Options-->
        <div class="row collapse report-filter-panel" runat="server">
            <div class="col-lg-12 col-md-10 col-xs-8 col-xs-offset-2 col-lg-offset-0 col-md-offset-1 report-filter">
                <div class="filter-block">
                    <div class="filter-row text-left">
                        <div class="filter-item">
                            <asp:Literal ID="Literal1" runat="server" meta:resourcekey="uxLiteralAssignedToResource1"> Assigned To:</asp:Literal>
                            <a href="#" onclick="SelectAllOrNone('<%= uxFilterAssignedToList.ClientID %>', true); return false;">
                                <asp:Literal ID="Literal2" runat="server" meta:resourcekey="uxLiteralAllResource1">All</asp:Literal></a> |
                         <a href="#" onclick="SelectAllOrNone('<%= uxFilterAssignedToList.ClientID %>', false); return false;">
                             <asp:Literal ID="Literal3" runat="server" meta:resourcekey="uxLiteralNoneResource1">None</asp:Literal></a>
                            <br />
                            <tek:RadListBox ID="uxFilterAssignedToList" runat="server" CheckBoxes="true" Width="267px"
                                Height="120px" DataKeyField="DataKey" DataTextField="DataText" meta:resourcekey="uxFilterAssignedToListResource1">
                                <ButtonSettings TransferButtons="All"></ButtonSettings>
                            </tek:RadListBox>
                        </div>
                        <div class="filter-item">
                            <asp:Literal ID="rm_EscalationQueue_aspx_All" runat="server" meta:resourcekey="rm_EscalationQueue_aspx_AllResource1">  Reason:</asp:Literal>
                            <a href="#" onclick="SelectAllOrNone('<%= uxFilterReason.ClientID %>', true); return false;">
                                <asp:Literal ID="Literal9" runat="server" meta:resourcekey="uxLiteralAllResource1">All</asp:Literal></a> |
                         <a href="#" onclick="SelectAllOrNone('<%= uxFilterReason.ClientID %>', false); return false;">
                             <asp:Literal ID="Literal6" runat="server" meta:resourcekey="uxLiteralNoneResource1">None</asp:Literal></a>
                            <br />
                            <tek:RadListBox ID="uxFilterReason" runat="server" CheckBoxes="true" Width="265px"
                                Height="120px" DataKeyField="EscalationReasonID" DataTextField="Reason" meta:resourcekey="uxFilterReasonResource1">
                                <ButtonSettings TransferButtons="All"></ButtonSettings>
                            </tek:RadListBox>
                        </div>
                        <div class="filter-item">
                            <asp:Literal ID="Literal10" runat="server" meta:resourcekey="Literal10Resource1">Status:</asp:Literal>
                            <a href="#" onclick="SelectAllOrNone('<%= uxFilterStatusList.ClientID %>', true); return false;">
                                <asp:Literal ID="Literal4" runat="server" meta:resourcekey="uxLiteralAllResource1">All</asp:Literal></a> |
                        <a href="#" onclick="SelectAllOrNone('<%= uxFilterStatusList.ClientID %>', false); return false;">
                            <asp:Literal ID="Literal7" runat="server" meta:resourcekey="uxLiteralNoneResource1">None</asp:Literal></a>
                            <br />
                            <tek:RadListBox ID="uxFilterStatusList" runat="server" CheckBoxes="true" Width="267px"
                                Height="120px" DataKeyField="EscalationStatusID" DataTextField="Status" meta:resourcekey="uxFilterStatusListResource1">
                                <ButtonSettings TransferButtons="All"></ButtonSettings>
                            </tek:RadListBox>
                        </div>
                        <div class="filter-item">
                            <asp:Literal ID="Literal11" runat="server" meta:resourcekey="Literal11Resource1">Resolution:</asp:Literal>
                            <a href="#" onclick="SelectAllOrNone('<%= uxFilterResolutionList.ClientID %>', true); return false;">
                                <asp:Literal ID="Literal5" runat="server" meta:resourcekey="uxLiteralAllResource1">All</asp:Literal></a> |
                        <a href="#" onclick="SelectAllOrNone('<%= uxFilterResolutionList.ClientID %>', false); return false;">
                            <asp:Literal ID="Literal8" runat="server" meta:resourcekey="uxLiteralNoneResource1">None</asp:Literal></a>
                            <br />
                            <tek:RadListBox ID="uxFilterResolutionList" runat="server" CheckBoxes="true" Width="267px"
                                Height="120px" DataKeyField="ResolutionID" DataTextField="Resolution" meta:resourcekey="uxFilterResolutionListResource1">
                                <ButtonSettings TransferButtons="All"></ButtonSettings>
                            </tek:RadListBox>
                        </div>
                    </div>
                    <div class="filter-row text-left">
                        <div class="filter-item">
                            <as:RadComboBox ID="uxFilterOpenClosed" runat="server" Width="267"
                                OnClientSelectedIndexChanged="uxFilterOpenClosed_OnClientSelectedIndexChanged"
                                xValuesReqFromToDates="[OpenBetween][ClosedBetween]" meta:resourcekey="uxFilterOpenClosedResource1">
                                <Items>
                                    <tek:RadComboBoxItem Value="Open" Text="Open" meta:resourcekey="RadComboBoxItemResource1" />
                                    <tek:RadComboBoxItem Value="Closed" Text="Closed" meta:resourcekey="RadComboBoxItemResource2" />
                                    <tek:RadComboBoxItem Value="All" Text="All" meta:resourcekey="RadComboBoxItemResource3" />
                                    <tek:RadComboBoxItem Value="OpenBetween" Text="Open Between" meta:resourcekey="RadComboBoxItemResource4" />
                                    <tek:RadComboBoxItem Value="ClosedBetween" Text="Closed Between" meta:resourcekey="RadComboBoxItemResource5" />
                                    <tek:RadComboBoxItem Value="SavingLossAmtBetween" Text="Savings/Loss Between" Visible="false" meta:resourcekey="RadComboBoxItemSavingsLoss" />
                                </Items>
                            </as:RadComboBox>
                        </div>
                        <div id="uxOpenCloseDateRange" class="filter-block">
                            <div class="filter-item">
                                <as:ASRadDatePicker ID="uxOpenCloseFromDate" runat="server" Width="128px" ShowPopupOnFocus="true"
                                    onkeypress="OnClientKeyPressing(this, event)" meta:resourcekey="uxOpenCloseFromDateResource1">
                                    <Calendar ID="Calendar1" FastNavigationStep="12" ShowRowHeaders="false" runat="server">
                                    </Calendar>
                                    <DateInput ID="DateInput1" DateFormat="MM/dd/yyyy" runat="Server" LabelWidth="64px" Width="">
                                        <EmptyMessageStyle Resize="None"></EmptyMessageStyle>

                                        <ReadOnlyStyle Resize="None"></ReadOnlyStyle>

                                        <FocusedStyle Resize="None"></FocusedStyle>

                                        <DisabledStyle Resize="None"></DisabledStyle>

                                        <InvalidStyle Resize="None"></InvalidStyle>

                                        <HoveredStyle Resize="None"></HoveredStyle>

                                        <EnabledStyle Resize="None"></EnabledStyle>
                                    </DateInput>

                                    <DatePopupButton ImageUrl="" HoverImageUrl="" CssClass=""></DatePopupButton>
                                </as:ASRadDatePicker>
                            </div>
                            <div class="filter-item">
                                <as:ASRadDatePicker ID="uxOpenCloseToDate" runat="server" Width="128px" ShowPopupOnFocus="true"
                                    onkeypress="OnClientKeyPressing(this, event)" meta:resourcekey="uxOpenCloseToDateResource1">
                                    <Calendar ID="Calendar2" FastNavigationStep="12" ShowRowHeaders="false" runat="server">
                                    </Calendar>
                                    <DateInput ID="DateInput2" DateFormat="MM/dd/yyyy" runat="Server" LabelWidth="64px" Width="">
                                        <EmptyMessageStyle Resize="None"></EmptyMessageStyle>

                                        <ReadOnlyStyle Resize="None"></ReadOnlyStyle>

                                        <FocusedStyle Resize="None"></FocusedStyle>

                                        <DisabledStyle Resize="None"></DisabledStyle>

                                        <InvalidStyle Resize="None"></InvalidStyle>

                                        <HoveredStyle Resize="None"></HoveredStyle>

                                        <EnabledStyle Resize="None"></EnabledStyle>
                                    </DateInput>

                                    <DatePopupButton ImageUrl="" HoverImageUrl="" CssClass=""></DatePopupButton>
                                </as:ASRadDatePicker>
                            </div>
                        </div>
                        <div id="uxSavingsLossRange" style="height: 100%; display: none;">
                            <div class="filter-item">
                                <tek:RadNumericTextBox ID="uxSavingsLossFrom" runat="server" MaxLength="9" Width="100px" Type="Currency" NegativeStyle-ForeColor="Red"></tek:RadNumericTextBox>
                            </div>
                            <div class="filter-item">
                                &nbsp;And&nbsp;
                            </div>
                            <div class="filter-item">
                                <tek:RadNumericTextBox ID="uxSavingsLossTo" runat="server" MaxLength="9" Width="100px" Type="Currency" NegativeStyle-ForeColor="Red"></tek:RadNumericTextBox>
                            </div>
                        </div>
                    </div>
                    <div class="filter-row text-left">
                        <div class="filter-item">
                            <as:RadComboBox ID="uxFilterOption" runat="server" Width="267"
                                OnClientSelectedIndexChanged="uxFilterOption_OnClientSelectedIndexChanged"
                                xValues="[TNO][MNO][MNAME][CNAME][FOLLOWUP]" meta:resourcekey="uxFilterOptionResource1">
                                <Items>
                                    <tek:RadComboBoxItem Value="" Text="None" meta:resourcekey="RadComboBoxItemResource6" />
                                    <tek:RadComboBoxItem Value="TNO" Text="Ticket Number" meta:resourcekey="RadComboBoxItemResource7" />
                                    <tek:RadComboBoxItem Value="MNO" Text="Merchant ID" meta:resourcekey="RadComboBoxItemResource8" />
                                    <tek:RadComboBoxItem Value="MNP" Text="Merchant ID (Partial)" meta:resourcekey="RadComboBoxItemResource24" />
                                    <tek:RadComboBoxItem Value="MNAME" Text="Merchant Name" meta:resourcekey="RadComboBoxItemResource9" />
                                    <tek:RadComboBoxItem Value="CNAME" Text="Corporate Name" meta:resourcekey="RadComboBoxItemResource23" Visible="false" />
                                    <tek:RadComboBoxItem Value="FOLLOWUP" Text="Follow Up" meta:resourcekey="RadComboBoxItemResource10" />
                                </Items>
                            </as:RadComboBox>
                        </div>
                        <div id="uxFilterSearchKey" class="filter-item">
                            <as:RadTextBox ID="uxFilterSearchKeyText" runat="server" Width="267" LabelCssClass="" LabelWidth="64px" meta:resourcekey="uxFilterSearchKeyTextResource1">
                                <EmptyMessageStyle Resize="None"></EmptyMessageStyle>

                                <ReadOnlyStyle Resize="None"></ReadOnlyStyle>

                                <FocusedStyle Resize="None"></FocusedStyle>

                                <DisabledStyle Resize="None"></DisabledStyle>

                                <InvalidStyle Resize="None"></InvalidStyle>

                                <HoveredStyle Resize="None"></HoveredStyle>

                                <EnabledStyle Resize="None"></EnabledStyle>
                            </as:RadTextBox>
                        </div>
                        <div id="pnlFollowUpOption" class="filter-item" style="display: none">
                            <as:RadComboBox ID="uxcbbFollowUp" Width="267px" runat="server" OnClientSelectedIndexChanged="uxcbbFollowUp_OnClientSelectedIndexChanged" meta:resourcekey="uxcbbFollowUpResource1">
                                <Items>
                                    <tek:RadComboBoxItem Value="ALL" Text="ALL" meta:resourcekey="RadComboBoxItemResource11" />
                                    <tek:RadComboBoxItem Value="TODAY" Text="Today" meta:resourcekey="RadComboBoxItemResource12" />
                                    <tek:RadComboBoxItem Value="YESTERDAY" Text="Yesterday" meta:resourcekey="RadComboBoxItemResource13" />
                                    <tek:RadComboBoxItem Value="THISWEEK" Text="This Week" meta:resourcekey="RadComboBoxItemResource14" />
                                    <tek:RadComboBoxItem Value="LASTWEEK" Text="Last Week" meta:resourcekey="RadComboBoxItemResource15" />
                                    <tek:RadComboBoxItem Value="LASTMONTH" Text="Last Month" meta:resourcekey="RadComboBoxItemResource16" />
                                    <tek:RadComboBoxItem Value="NEXTWEEK" Text="Next Week" meta:resourcekey="RadComboBoxItemResource17" />
                                    <tek:RadComboBoxItem Value="NEXTMONTH" Text="Next Month" meta:resourcekey="RadComboBoxItemResource18" />
                                    <tek:RadComboBoxItem Value="DATERANGE" Text="Date Range" meta:resourcekey="RadComboBoxItemResource19" />
                                    <tek:RadComboBoxItem Value="PASTDUE" Text="Past Due" meta:resourcekey="RadComboBoxItemResource20" />
                                    <tek:RadComboBoxItem Value="SET" Text="Set" meta:resourcekey="RadComboBoxItemResource21" />
                                    <tek:RadComboBoxItem Value="NOTSET" Text="Not Set" meta:resourcekey="RadComboBoxItemResource22" />
                                </Items>
                            </as:RadComboBox>
                        </div>
                        <div id="pnlFollowUpDate" class="filter-block" style="display: none;">
                            <div class="filter-item">
                                <as:ASRadDatePicker ID="uxDateRangeFollowupFrom" runat="server" Width="100px" ShowPopupOnFocus="true"
                                    onkeypress="OnClientKeyPressing(this, event)" meta:resourcekey="uxDateRangeFollowupFromResource1">
                                    <Calendar ID="Calendar3" FastNavigationStep="12" ShowRowHeaders="false" runat="server">
                                    </Calendar>
                                    <DateInput ID="DateInput3" DateFormat="MM/dd/yyyy" runat="Server" LabelWidth="64px" Width="">
                                        <EmptyMessageStyle Resize="None"></EmptyMessageStyle>

                                        <ReadOnlyStyle Resize="None"></ReadOnlyStyle>

                                        <FocusedStyle Resize="None"></FocusedStyle>

                                        <DisabledStyle Resize="None"></DisabledStyle>

                                        <InvalidStyle Resize="None"></InvalidStyle>

                                        <HoveredStyle Resize="None"></HoveredStyle>

                                        <EnabledStyle Resize="None"></EnabledStyle>
                                    </DateInput>

                                    <DatePopupButton ImageUrl="" HoverImageUrl="" CssClass=""></DatePopupButton>
                                </as:ASRadDatePicker>
                            </div>
                            <div class="filter-item">
                                <as:ASRadDatePicker ID="uxDateRangeFollowupTo" runat="server" Width="100px" ShowPopupOnFocus="true"
                                    onkeypress="OnClientKeyPressing(this, event)" meta:resourcekey="uxDateRangeFollowupToResource1">
                                    <Calendar ID="Calendar4" FastNavigationStep="12" ShowRowHeaders="false" runat="server">
                                    </Calendar>
                                    <DateInput ID="DateInput4" DateFormat="MM/dd/yyyy" runat="Server" LabelWidth="64px" Width="">
                                        <EmptyMessageStyle Resize="None"></EmptyMessageStyle>

                                        <ReadOnlyStyle Resize="None"></ReadOnlyStyle>

                                        <FocusedStyle Resize="None"></FocusedStyle>

                                        <DisabledStyle Resize="None"></DisabledStyle>

                                        <InvalidStyle Resize="None"></InvalidStyle>

                                        <HoveredStyle Resize="None"></HoveredStyle>

                                        <EnabledStyle Resize="None"></EnabledStyle>
                                    </DateInput>

                                    <DatePopupButton ImageUrl="" HoverImageUrl="" CssClass=""></DatePopupButton>
                                </as:ASRadDatePicker>
                            </div>
                        </div>
                        <div class="filter-item">
                            <as:Button ID="uxSearchButton" runat="server" Text="Search" CssClass="btn btn-default"
                                OnClientClick="return onSearchEscalationQueue();" meta:resourcekey="uxSearchButtonResource1" />
                            <as:Button ID="uxProxyButton" runat="server" IsStandardButton="true" OnClick="uxSearchButton_OnClick"
                                CssClass="display-none" meta:resourcekey="uxProxyButtonResource1" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12 text-center" data-target=".report-filter-panel" data-toggle="collapse">
                <span class="btn btn-link btn-report-filter">
                    <asp:Literal ID="uxLiteralFilter" runat="server" meta:resourcekey="uxLiteralFilterResource1"> FILTER</asp:Literal></span>
            </div>
        </div>
        <!--End Filtering Options-->
        <uc:PageTitle ID="uxReportTitle" runat="server" ReportTitle="Escalation Queue" meta:resourcekey="uxReportTitleResource1" />

        <uc:UxExport ID="uxExporterTop" runat="server" GridID="uxReportGrid" IsOnTop="true" ShowPDF="false"
            GridHeader="Risk Management - Risk Analysis - Escalation Queue" meta:resourcekey="uxExporterTopResource1" />
        <as:ASGrid ID="uxReportGrid" runat="server" AutoGenerateColumns="false" Visible="true" IsAutoExportTemplate="true"
            ShowHeader="true" AllowPaging="true" PageSize="40" ASPagingMethod="SPASingleMethod" HeaderStyle-Width="90px"
            AllowSorting="true" IsIntruder="true" IntruderSourceName="uxReportGridSummary"
            AllowSortFilterWhenExport="true" ShowFooter="false" CssClass="in" meta:resourcekey="uxReportGridResource1">
            <MasterTableView>
                <Columns>
                    <as:ASGridBoundColumn HeaderText="Tkt" DataField="EscalationID" UniqueName="EscalationID" HeaderStyle-Width="60px"
                        SortExpression="EscalationID" HeaderTooltip="Ticket" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource1">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Merchant ID" DataField="MerchantNumber" UniqueName="MerchantNumber"
                        Display="false" HeaderStyle-Width="140px" meta:resourcekey="ASGridBoundColumnResource2">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center" Width="140px"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Merchant Name" DataField="MerchantName" UniqueName="MerchantName" HeaderStyle-Width="160px"
                        ItemStyle-CssClass="word-break"
                        SortExpression="MerchantName" HeaderTooltip="Merchant Name" ASFormat="DynamicString" meta:resourcekey="ASGridBoundColumnResource3">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Profile" DataField="Profile" UniqueName="Profile"
                        SortExpression="Profile" HeaderTooltip="Profile" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource4">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Closed" DataField="Closed" UniqueName="Closed"
                        HeaderTooltip="Closed" SortExpression="Closed" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource5">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Watch" DataField="Watch" UniqueName="Watch" SortExpression="Watch"
                        HeaderTooltip="Watch" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource6">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Multi-Watch" DataField="MultiWatch" UniqueName="MultiWatch"
                        SortExpression="MultiWatch" HeaderTooltip="Multi-Watch" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource7">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Current Status" DataField="Status" UniqueName="Status"
                        HeaderTooltip="Current Status" ASFormat="StaticString" SortExpression="Status" meta:resourcekey="ASGridBoundColumnResource8">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center" Width="200px"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Reason" DataField="Reason" UniqueName="Reason" HeaderStyle-Width="160px"
                        ItemStyle-CssClass="word-break"
                        SortExpression="Reason" HeaderTooltip="Reason" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource9">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Resolution" DataField="Resolution" UniqueName="Resolution"
                        HeaderTooltip="Resolution" ASFormat="StaticString" SortExpression="Resolution" meta:resourcekey="ASGridBoundColumnResource10">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center" Width="200px"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Savings/Loss Amt" DataField="SavingsLossAmt" UniqueName="SavingLoss" HeaderStyle-Width="110px"
                        HeaderStyle-HorizontalAlign="Center" ASFormat="Currency" HeaderTooltip="Savings/Loss Amt"
                        SortExpression="SavingsLossAmt" ItemStyle-HorizontalAlign="Right" Visible="false" meta:resourcekey="SavingLossResource1">
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Assigned To" DataField="AssignedTo" UniqueName="AssignedTo" HeaderStyle-Width="130px"
                        HeaderTooltip="Assigned To" ASFormat="DynamicString" SortExpression="AssignedTo" meta:resourcekey="ASGridBoundColumnResource11">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Open Date" DataField="EscalationDate" UniqueName="EscalationDate"
                        HeaderTooltip="Open Date" ASFormat="DateAndTime12Hours" SortExpression="EscalationDate"
                        meta:resourcekey="ASGridBoundColumnResource12">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Last Update" DataField="LastUpdated" UniqueName="LastUpdated"
                        HeaderTooltip="Last Update" ASFormat="DateAndTime12Hours" SortExpression="LastUpdated" HeaderStyle-Width="110px"
                        meta:resourcekey="ASGridBoundColumnResource13">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Follow Up Date" DataField="FollowupDate" UniqueName="FollowupDate"
                        HeaderStyle-HorizontalAlign="Center" ASFormat="Date" HeaderTooltip="Follow Up Date"
                        SortExpression="FollowupDate" HeaderStyle-Width="100px" meta:resourcekey="ASGridBoundColumnResource14">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                </Columns>
            </MasterTableView>
        </as:ASGrid>
        <as:Button ID="btnProcess" runat="server" IsStandardButton="true" CssClass="display-none"
            OnClick="btnProcess_Click" meta:resourcekey="btnProcessResource1" />
        <as:HiddenField ID="hddProcessData" runat="server" />
        <as:HiddenField ID="hdRebind" Value="0" runat="server" />
    </as:PlaceHolder>
    <tek:RadCodeBlock ID="JavaScript" runat="server">
        <script type="text/javascript">
            var rm_EscalationQueue_uxFilterSearchKeyText = '<%= uxFilterSearchKeyText.ClientID %>';
            var rm_EscalationQueue_uxcbbFollowUp = '<%= uxcbbFollowUp.ClientID %>';
            var rm_EscalationQueue_uxSearchButton = '<%= uxSearchButton.ClientID %>';
            var rm_EscalationQueue_hddProcessData = '<%= hddProcessData.ClientID %>';
            var rm_EscalationQueue_btnProcess = '<%= btnProcess.ClientID %>';
            var rm_EscalationQueue_uxFilterOpenClosed = '<%= uxFilterOpenClosed.ClientID %>';
            var rm_EscalationQueue_uxOpenCloseFromDate = '<%= uxOpenCloseFromDate.ClientID %>';
            var rm_EscalationQueue_uxOpenCloseToDate = '<%= uxOpenCloseToDate.ClientID %>';
            var rm_EscalationQueue_uxDateRangeFollowupFrom = '<%= uxDateRangeFollowupFrom.ClientID %>';
            var rm_EscalationQueue_uxDateRangeFollowupTo = '<%= uxDateRangeFollowupTo.ClientID %>';
            var rm_EscalationQueue_uxFilterOption = '<%= uxFilterOption.ClientID %>';
            var rm_EscalationQueue_uxProxyButton = '<%= uxProxyButton.ClientID %>';
            var rm_EscalationQueue_uxSavingsLossFrom = '<%= uxSavingsLossFrom.ClientID %>';
            var rm_EscalationQueue_uxSavingsLossTo = '<%= uxSavingsLossTo.ClientID %>';

            var ReportFilter_V3 = '<%=Resources.MessageManager.ReportFilter_V3%>';
            var ReportFilter_ReportDate_InvaidDate = '<%=Resources.MessageManager.ReportFilter_ReportDate_InvaidDate%>';
            var ReportFilter_V10 = '<%=Resources.MessageManager.ReportFilter_V10%>';
            var rm_EscalationQueue_js_Alert1 = '<%=GetLocalResourceObject("rm_EscalationQueue_js_Alert1").ToString()%>'
            var rm_EscalationQueue_js_Alert2 = '<%=GetLocalResourceObject("rm_EscalationQueue_js_Alert2").ToString()%>'
            var rm_EscalationQueue_js_Alert3 = '<%=GetLocalResourceObject("rm_EscalationQueue_js_Alert3").ToString()%>'
            var rm_EscalationQueue_js_Alert4 = '<%=GetLocalResourceObject("rm_EscalationQueue_js_Alert4").ToString()%>'
            var rm_EscalationQueue_js_SavingsLossFromToRequired = '<%=GetLocalResourceObject("SavingsLossFromToRequired_text").ToString()%>'
            var rm_EscalationQueue_js_SavingsLossFromRequired = '<%=GetLocalResourceObject("SavingsLossFromRequired_text").ToString()%>'
            var rm_EscalationQueue_js_SavingsLossToRequired = '<%=GetLocalResourceObject("SavingsLossToRequired_text").ToString()%>'
            var rm_EscalationQueue_js_SavingsLossToGreaterThanFrom_text = '<%=GetLocalResourceObject("SavingsLossToGreaterThanFrom_text").ToString()%>'
            
        </script>
        <script src="<%=ResolveUrl("~")%>res/js/risk_MCF/rm_MCF_EscalationQueue.js"></script>
    </tek:RadCodeBlock>
</asp:Content>
