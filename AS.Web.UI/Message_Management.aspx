<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Title=""
    CodeFile="Message_Management.aspx.cs" Inherits="Message_Management" meta:resourcekey="PageResource1" %>

<%@ Register TagName="ReportFiltering" Src="~/UserControls/ReportFiltering.ascx"
    TagPrefix="uc" %>
<%@ Register TagName="PageTile" Src="~/UserControls/PageTitle.ascx" TagPrefix="uc" %>
<%@ Register TagName="Export" Src="~/UserControls/UxExport.ascx" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/UxExport.ascx" TagName="UxExport" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="Server">
    <as:RadAjaxManagerProxy ID="RadAjaxManager1" runat="server">
        <AjaxSettings>
            <tek:AjaxSetting AjaxControlID="uxLnkCreateMessage">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="uxLnkCreateMessage" />
                </UpdatedControls>
            </tek:AjaxSetting>
            <tek:AjaxSetting AjaxControlID="uxReportGrid">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="uxReportGrid" />
                </UpdatedControls>
            </tek:AjaxSetting>
        </AjaxSettings>
    </as:RadAjaxManagerProxy>

    <div class="row collapse report-filter-panel">
        <div class="col-md-12 report-filter">
            <div class="filter-block text-left">
                <table>
                    <tr>
                        <td>
                            <div class="filter-item">
                                <as:RadioButton ID="uxDaily" runat="server" Text="Daily" GroupName="Date" onclick="ChangeDateOption(this)"
                                    onkeypress="javascript:return false;" CssClass="date-item" meta:resourcekey="uxDailyResource1" Value="" />
                                <as:RadioButton ID="uxMonthly" runat="server" Text="Monthly" GroupName="Date" onclick="ChangeDateOption(this)"
                                    onkeypress="javascript:return false;" CssClass="date-item" meta:resourcekey="uxMonthlyResource1" Value="" />
                                <as:RadioButton ID="uxRange" runat="server" Text="Date Range" GroupName="Date" onclick="ChangeDateOption(this)"
                                    onkeypress="javascript:return false;" CssClass="date-item" meta:resourcekey="uxRangeResource1" Value="" />
                            </div>
                        </td>
                        <td>
                            <div class="filter-item" id="divDate">
                                <as:RadDatePicker ID="uxDate" Width="130" runat="server" Skin="Default">
                                    <Calendar FastNavigationStep="12" ShowRowHeaders="false" />
                                    <DateInput ID="DateInput1" runat="server" onclick="ShowCalendar('1')" onkeypress="return SearchEnterOnTextbox(event);" />
                                </as:RadDatePicker>
                            </div>
                            <div id="divDateRange" style="display: none;" class="filter-item">
                                <div class="filter-item">
                                    <as:RadDatePicker ID="uxFromDate" Width="130" runat="server">
                                        <Calendar FastNavigationStep="12" ShowRowHeaders="false" />
                                        <DateInput ID="DateInput2" runat="server" onclick="ShowCalendar('2')" onkeypress="return SearchEnterOnTextbox(event);" />
                                    </as:RadDatePicker>
                                </div>
                                <div class="filter-item">
                                    <as:RadDatePicker ID="uxEndDate" Width="130" runat="server" Skin="Default">
                                        <Calendar FastNavigationStep="12" ShowRowHeaders="false" />
                                        <DateInput ID="DateInput3" runat="server" onclick="ShowCalendar('3')" onkeypress="return SearchEnterOnTextbox(event);" />
                                    </as:RadDatePicker>
                                </div>
                            </div>
                        </td>
                        <td>
                            <div class="filter-item valign-bottom">
                                <as:Button ID="uxSearch" runat="server" Text="Search" OnClientClick="return ValidateData();"
                                    OnClick="uxSearch_Click" CssClass="btn btn-default" IsStandardButton="False" meta:resourcekey="uxSearchResource1" />
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-md-12 text-center" data-target=".report-filter-panel" data-toggle="collapse">
            <span class="btn btn-link btn-report-filter">
                <as:Literal ID="ltFILTER" runat="server" Text="FILTER" meta:resourcekey="ltFILTERResource1"></as:Literal></span>
        </div>
    </div>
    <uc:PageTile ID="uxPageTitle" runat="server" />

    <as:PlaceHolder ID="uxMessagePlaceHolder" runat="server">
        <uc:UxExport ID="uxExporter" runat="server" GridID="uxReportGrid" GridHeader="Sent Messages"
            IsOnTop="true" meta:resourcekey="uxExporterResource1" />
        <as:ASGrid ID="uxReportGrid" runat="server" AutoGenerateColumns="false" AllowPaging="true" IsAutoExportTemplate="true"
            AllowSorting="true" ASPagingMethod="SPASingleMethod" CssClass="in" meta:resourcekey="uxReportGridResource1">
            <MasterTableView>
                <Columns>
                    <as:ASGridBoundColumn UniqueName="DateTime" HeaderText="Date/Time" DataField="DateTime"
                        ASFormat="DateAndTime12Hours" HeaderTooltip="Date/Time" SortExpression="DateTime"
                        HeaderStyle-Width="150px" meta:resourcekey="ASGridBoundColumnResource1">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center" Width="150px"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn UniqueName="PostedBy" HeaderText="Posted By" DataField="PostedBy"
                        AllowEncodeOnExporting="true" ASFormat="StaticString" HeaderTooltip="Posted By"
                        SortExpression="PostedBy" meta:resourcekey="ASGridBoundColumnResource2">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn UniqueName="Comment" HeaderText="Message Text" DataField="Comment"
                        HeaderTooltip="Message Text" SortExpression="Comment" ASFormat="DynamicString"
                        AllowEncodeOnExporting="true" meta:resourcekey="ASGridBoundColumnResource3">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn UniqueName="Author" HeaderText="Author" DataField="Author"
                        AllowEncodeOnExporting="true" HeaderTooltip="Author" SortExpression="Author"
                        ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource4">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridTemplateColumn UniqueName="Recipients" HeaderText="Recipient(s)" HeaderTooltip="Recipient(s)"
                        HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" meta:resourcekey="ASGridTemplateColumnResource1">
                        <ItemTemplate>
                            <as:LinkButton ID="lnkViewMessage" runat="server" OnCommand="lnkViewMessage_Command"
                                CommandArgument='<%# Eval("MessageID") %>' Text="View List" meta:resourcekey="lnkViewMessageResource1"></as:LinkButton>
                        </ItemTemplate>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </as:ASGridTemplateColumn>
                </Columns>
            </MasterTableView>
        </as:ASGrid>
    </as:PlaceHolder>
    <div class="row">
        <div class="col-md-12 action-container">
            <%-- Hyperlink Create--%>
            <as:LinkButton ID="uxLnkCreateMessage" runat="server" Text="Send New Message"
                OnClick="uxLnkCreateMessage_Click" CssClass="btn btn-default" meta:resourcekey="uxLnkCreateMessageResource1" />
        </div>
    </div>
    <div class="display-none">
        <as:Button ID="uxRebind" runat="server" IsStandardButton="True" OnClick="uxRebind_Click" meta:resourcekey="uxRebindResource1" />
    </div>
    <tek:RadCodeBlock ID="uxRadCodeBlock" runat="server">
        <script type="text/javascript">
            var Message_Management_uxFromDate = "<%=uxFromDate.ClientID%>";
            var Message_Management_uxEndDate = "<%=uxEndDate.ClientID%>";
            var Message_Management_uxDate = "<%=uxDate.ClientID%>";
            var Message_Management_uxRange = "<%=uxRange.ClientID%>";
            var Message_Management_uxSearch = "<%=uxSearch.ClientID%>";
            var Message_Management_uxDaily = "<%=uxDaily.ClientID%>";
            var Message_Management_uxMonthly = "<%=uxMonthly.ClientID%>";
            var Message_Management_uxRebind = "<%=uxRebind.ClientID%>";

            var Message_Management_ReportFilter_V3 = "<%=Resources.MessageManager.ReportFilter_V3%>";
            var Message_Management_ReportFilter_ReportDate_InvaidDate = "<%=Resources.MessageManager.ReportFilter_ReportDate_InvaidDate %>";
            var Message_Management_ReportFilter_V1 = "<%=Resources.MessageManager.ReportFilter_V1%>";
            var Message_Management_ReportFilter_V9 = "<%=Resources.MessageManager.ReportFilter_V9%>";
            var Message_Management_FILTERING_OPTIONS_DATERANGE = "<%= GeneralFuncsLib.HasExtendedSetting("FILTERING_OPTIONS_DATERANGE") %>";

            var MessageManagement_js_Daily_text = '<%= GetLocalResourceObject("uxDailyResource1.Text").ToString()%>';
            var MessageManagement_js_Monthly_text = '<%= GetLocalResourceObject("uxMonthlyResource1.Text").ToString()%>';
            var MessageManagement_js_Date_range_text = '<%= GetLocalResourceObject("uxRangeResource1.Text").ToString()%>';
            var funcValidate = 'ValidateData';
        </script>
        <script type="text/javascript" src="<%=ResolveUrl("~") %>res/js/Message_Management.js"></script>
    </tek:RadCodeBlock>
</asp:Content>
