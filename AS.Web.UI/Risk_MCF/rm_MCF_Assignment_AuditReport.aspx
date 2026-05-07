<%@ Page Title="Assignment Audit Report" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="rm_MCF_Assignment_AuditReport.aspx.cs" Inherits="rm_MCF_Assignment_AuditReport" meta:resourcekey="PageResource1" %>

<%@ Register Src="~/UserControls/UxExport.ascx" TagName="UxExport" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/PageTitle.ascx" TagName="PageTitle" TagPrefix="uc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="Server">

    <tek:RadAjaxManagerProxy runat="server" ID="uxRadAjaxManager">
        <AjaxSettings>
            <tek:AjaxSetting AjaxControlID="uxReportGrid">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="uxReportGrid" LoadingPanelID="uxLoadingPanelCustom" />
                </UpdatedControls>
            </tek:AjaxSetting>
        </AjaxSettings>
    </tek:RadAjaxManagerProxy>
    <!--Filtering Options-->
    <div class="row collapse report-filter-panel">
        <div class="col-md-12 report-filter">
            <div class="filter-block">
                <table>
                    <tr class="valign-middle">
                        <td></td>
                        <td class="text-right">
                            <div class="filter-item">
                                <as:RadioButton ID="uxDaily" runat="server" Text="Daily" GroupName="Date" onclick="Assignment_AuditReport.ChangeDateOption(this)"
                                    onkeypress="javascript:return false;" CssClass="date-item" meta:resourcekey="uxDailyResource1" Value="" />
                                <as:RadioButton ID="uxMonthly" runat="server" Text="Monthly" GroupName="Date" onclick="Assignment_AuditReport.ChangeDateOption(this)"
                                    onkeypress="javascript:return false;" CssClass="date-item" meta:resourcekey="uxMonthlyResource1" Value="" />
                                <as:RadioButton ID="uxRange" runat="server" Text="Date Range" GroupName="Date" onclick="Assignment_AuditReport.ChangeDateOption(this)"
                                    onkeypress="javascript:return false;" CssClass="date-item" meta:resourcekey="uxRangeResource1" Value="" />
                            </div>
                        </td>
                        <td class="text-left" style="width: 274px">
                            <div id="divDate">
                                <div class="filter-item">
                                    <as:RadDatePicker ID="uxDate" runat="server" Width="128px">
                                        <Calendar FastNavigationStep="12" ShowRowHeaders="false" />
                                        <DateInput ID="DateInput1" runat="server" onclick="Assignment_AuditReport.ShowCalendar('1')" onkeypress="return SearchEnterOnTextbox(event);" />
                                    </as:RadDatePicker>
                                </div>
                            </div>
                            <div id="divDateRange" style="display: none;">
                                <div class="filter-item">
                                    <as:RadDatePicker ID="uxFromDate" runat="server" Width="128px">
                                        <Calendar FastNavigationStep="12" ShowRowHeaders="false" />
                                        <DateInput ID="DateInput2" runat="server" onclick="Assignment_AuditReport.ShowCalendar('2')" onkeypress="return SearchEnterOnTextbox(event);" />
                                    </as:RadDatePicker>
                                </div>
                                <div class="filter-item">
                                    <as:RadDatePicker ID="uxEndDate" runat="server" Width="128px">
                                        <Calendar FastNavigationStep="12" ShowRowHeaders="false" />
                                        <DateInput ID="DateInput3" runat="server" onclick="Assignment_AuditReport.ShowCalendar('3')" onkeypress="return SearchEnterOnTextbox(event);" />
                                    </as:RadDatePicker>
                                </div>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="text-right valign-top">
                            <label class="filter-label">
                                <asp:Literal ID="Literal7" runat="server" Text="Changed By:" meta:resourcekey="LiteralResource7" /></label></td>
                        <td colspan="2" class="text-left">
                            <div class="filter-item risk-mcf-audit-chosen">
                                <as:MultiChooser ID="uxUserNameList" runat="server" Placeholder=" " meta:resourcekey="uxUserNameListResource1"  IsSearchContains="true">
                                </as:MultiChooser>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="text-right valign-top">
                            <label class="filter-label">
                                <asp:Literal ID="Literal4" runat="server" Text="Assignment Name:" meta:resourcekey="LiteralResource3" /></label></td>
                        <td colspan="2" class="text-left">
                            <div class="filter-item risk-mcf-audit-chosen">
                                <as:MultiChooser ID="uxAssignmentList" runat="server" Placeholder=" " CssClass="rf_TextBox" meta:resourcekey="uxAssignmentListResource1"  IsSearchContains="true">
                                </as:MultiChooser>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="text-right valign-middle">
                            <div class="filter-item text-right">
                                <as:Literal ID="ltFieldAction" runat="server" Text="Field/Action:" meta:resourcekey="ltFieldAction"></as:Literal>
                                <div class="date-item">
                                    <as:RadioButton ID="uxRdEqual" runat="server" Text="Equal To" GroupName="ChangeEntitySearch" CssClass="" meta:resourcekey="uxSearchEqualResource1" Value="Equal" />
                                    <as:RadioButton ID="uxRdContain" runat="server" Text="Contains" GroupName="ChangeEntitySearch" Checked="true" CssClass="date-item" meta:resourcekey="uxSearchContainResource1" Value="Contains" />
                                </div>
                            </div>
                        </td>
                        <td colspan="2" class="text-left">
                            <div class="filter-item">
                                <as:TextBox ID="uxFieldAction" MaxLength="255" Width="535px" runat="server" CssClass="rf_TextBox" MaxHeight="200px"></as:TextBox>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
            <div class="filter-block valign-bottom">
                <div class="filter-item ">
                    <asp:Button ID="uxSearch" runat="server" CssClass="btn btn-default" Text="Search"
                        OnClientClick="return Assignment_AuditReport.ValidateData();" OnClick="uxSearch_Click" meta:resourcekey="uxSearchResource1" />
                </div>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-md-12 text-center" data-target=".report-filter-panel" data-toggle="collapse">
            <span class="btn btn-link btn-report-filter">
                <as:Literal ID="Literal1" runat="server" Text="FILTER" meta:resourcekey="Literal1Resource1"></as:Literal></span>
        </div>
    </div>
    <as:PlaceHolder ID="uxViewMessagePlaceHolder" runat="server">
        <uc:UxExport ShowCSV="false"  ShowPDF="false" ShowWord="false" ID="uxExporter" runat="server" IsOnTop="true" GridID="uxReportGrid" />
        <as:ASGrid ID="uxReportGrid" runat="server" AutoGenerateColumns="false" AllowPaging="true" IsAutoExportTemplate="true"
            AllowSorting="true" ASPagingMethod="SPASingleMethod" CssClass="in" meta:resourcekey="uxReportGridResource1" OnItemDataBound="uxReportGrid_ItemDataBound">
            <MasterTableView>
                <Columns>
                    <as:ASGridBoundColumn UniqueName="ReportDate" HeaderText="Date/Time" DataField="ReportDate"
                        HeaderStyle-Width="150px" ASFormat="DateTimeShortTime" HeaderTooltip="Date/Time" ItemStyle-HorizontalAlign="Center"
                        SortExpression="ReportDate" meta:resourcekey="ASGridBoundColumnResource1">
                        <HeaderStyle HorizontalAlign="Center" Width="150px"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn UniqueName="ChangedByUser" HeaderText="Changed By" DataField="ChangedByUser"
                        AllowEncodeOnExporting="true" ASFormat="DynamicString" HeaderTooltip="Changed By"
                        SortExpression="ChangedByUser" meta:resourcekey="ASGridBoundColumnResource2">
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn UniqueName="AssignmentName" HeaderText="Assignment Name"
                        DataField="AssignmentName" ItemStyle-HorizontalAlign="Left"
                        HeaderTooltip="Assignment Nam" SortExpression="AssignmentName" ASFormat="StaticString"
                        AllowEncodeOnExporting="true" meta:resourcekey="ASGridBoundColumnResource3">
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn UniqueName="FieldAction" HeaderText="Field/Action" DataField="FieldAction"
                        HeaderTooltip="Field /Action" SortExpression="FieldAction" ASFormat="StaticString" ItemStyle-HorizontalAlign="Left"
                        AllowEncodeOnExporting="true" meta:resourcekey="ASGridBoundColumnResource4">
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn UniqueName="PreviousValue" HeaderText="Previous Value" DataField="PreviousValue" ItemStyle-CssClass="word-break"
                        HeaderTooltip="Previous Value" SortExpression="PreviousValue" ASFormat="DynamicString" ItemStyle-HorizontalAlign="Center"
                        AllowEncodeOnExporting="true" meta:resourcekey="ASGridBoundColumnResource5"  ASDefaultNullValue="&mdash;">
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn UniqueName="NewValue" ItemStyle-Wrap="true" HeaderText="New Value"
                        DataField="NewValue" ItemStyle-CssClass="word-break" ItemStyle-HorizontalAlign="Center"
                        HeaderTooltip="New Value" SortExpression="NewValue" ASFormat="DynamicString"
                        AllowEncodeOnExporting="true" meta:resourcekey="ASGridBoundColumnResource6" ASDefaultNullValue="&mdash;">
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                </Columns>
            </MasterTableView>
        </as:ASGrid>
    </as:PlaceHolder>
    <tek:RadCodeBlock ID="uxRadCodeBlock" runat="server">
        <script type="text/javascript">
            var uxFromDate_ClientID = "<%=uxFromDate.ClientID%>";
            var uxEndDate_ClientID = "<%=uxEndDate.ClientID%>";
            var uxDate_ClientID = "<%= uxDate.ClientID %>";

            var uxDaily_ClientID = "<%=uxDaily.ClientID %>";
            var uxMonthly_ClientID = "<%=uxMonthly.ClientID %>";
            var uxRange_ClientID = "<%=uxRange.ClientID %>";
            var uxSearch_ClientID = "<%=uxSearch.ClientID%>";
            var uxDaily = document.getElementById(uxDaily_ClientID);
            var uxMonthly = document.getElementById(uxMonthly_ClientID);
            var uxDateRange = document.getElementById(uxRange_ClientID);
            var uxFieldAction_ClientID = "<%=uxFieldAction.ClientID%>";
            var uxDailyResource1_text = '<%= GetLocalResourceObject("uxDailyResource1.Text").ToString()%>';
            var uxMonthlyResource1_text = '<%= GetLocalResourceObject("uxMonthlyResource1.Text").ToString()%>';
            var uxRangeResource1_text = '<%= GetLocalResourceObject("uxRangeResource1.Text").ToString()%>';
            var Msg_V2 = "<%=Resources.MessageManager.ReportFilter_ReportDate_InvaidDate %>";
            var Msg_V3 = "<%=Resources.MessageManager.ReportFilter_V3%>";
            var Mgs_FieldAction = "<%= GetLocalResourceObject("msg_FieldAction_specialCharacter") %>"
            var Msg_V1 = "<%=Resources.MessageManager.ReportFilter_V1%>";
            var Msg_V9 = "<%=Resources.MessageManager.ReportFilter_V9%>";
            var funcValidate = 'ValidateData';
        </script>
        <script src="<%= ResolveUrl("~/")%>res/js/risk_MCF/rm_MCF_Assignment_AuditReport.js"></script>
    </tek:RadCodeBlock>
</asp:Content>

