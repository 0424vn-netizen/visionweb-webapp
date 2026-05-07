<%@ Page Title="User Activity" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="UserActivity.aspx.cs" Inherits="UserActivity" meta:resourcekey="PageResource1" %>

<%@ Register Src="~/UserControls/PageTitle.ascx" TagName="PageTitle" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/UxExport.ascx" TagName="UxExport" TagPrefix="uc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="Server">
    <tek:RadAjaxManagerProxy ID="RadAjaxManager" runat="server">
        <AjaxSettings>
            <tek:AjaxSetting AjaxControlID="uxUserActivityGrid">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="uxUserActivityGrid" />
                </UpdatedControls>
            </tek:AjaxSetting>
        </AjaxSettings>
    </tek:RadAjaxManagerProxy>
    <div class="row collapse report-filter-panel">
        <div class="col-md-12 report-filter">
            <div class="filter-block">
                <table>
                    <tr>
                        <td class="text-right">
                            <as:Literal ID="ltDate" runat="server" Text="Date:" meta:resourcekey="ltDateResource1"></as:Literal>
                        </td>
                        <td class="text-left">
                            <div class="filter-item">
                                <as:RadDatePicker ID="uxDate" runat="server">
                                    <DateInput ID="DateInput2" runat="server" />
                                </as:RadDatePicker>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="text-right">
                            <as:Literal ID="Literal1" runat="server" Text="Risk User:" meta:resourcekey="Literal1Resource1"></as:Literal>
                        </td>
                        <td class="text-left">
                            <as:RadComboBox ID="uxUserList" runat="server" Width="250px" DataValueField="UserId"
                                CssClass="filter-item"
                                DataTextField="UserName">
                            </as:RadComboBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="text-right">
                            <as:Literal ID="Literal2" runat="server" Text="Merchant ID:" meta:resourcekey="Literal2Resource1"></as:Literal>
                        </td>
                        <td class="text-left">
                            <div class="filter-item">
                                <as:TextBox ID="uxMerchantNumber" runat="server" Width="250px" HintCss="hint" meta:resourcekey="uxMerchantNumberResource1"></as:TextBox>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <div class="filter-item pull-right">
                                <as:Button ID="uxSubmit" runat="server" Text="Search" CssClass="btn btn-default"
                                    OnClick="uxSubmit_Click" OnClientClick="return ValidateData();" IsStandardButton="False" meta:resourcekey="uxSubmitResource1" />
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
                <as:Literal ID="Literal3" runat="server" Text="FILTER" meta:resourcekey="Literal3Resource1"></as:Literal></span>
        </div>
    </div>

    <uc:PageTitle ID="uxPageTitle" runat="server" ReportTitle="User Activity" meta:resourcekey="uxPageTitleResource1" />
    <uc:UxExport ID="uxExporterTop" runat="server" GridID="uxUserActivityGrid" IsOnTop="true" />
    <div class="row">
        <div class="col-md-12">
            <as:Literal runat="server" ID="uxHeaderTotal" meta:resourcekey="uxHeaderTotalResource1"></as:Literal>
        </div>
    </div>
    <as:ASGrid ID="uxUserActivityGrid" runat="server" AutoGenerateColumns="false" AllowSorting="true"
        AllowPaging="true" ASPagingMethod="SPASingleMethod" CssClass="in" meta:resourcekey="uxUserActivityGridResource1">
        <MasterTableView>
            <Columns>
                <as:ASGridBoundColumn DataField="MerchantNumber" UniqueName="MerchantNumber" HeaderText="Merchant ID"
                    ASFormat="StaticString" HeaderTooltip="Merchant ID" HeaderStyle-Width="140px" meta:resourcekey="ASGridBoundColumnResource1">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center" Width="140px"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn DataField="MerchantName" UniqueName="MerchantName" HeaderText="Merchant Name"
                    ASFormat="DynamicString" HeaderTooltip="Merchant Name" meta:resourcekey="ASGridBoundColumnResource2">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn DataField="Activity" UniqueName="Activity" HeaderText="Activity"
                    HeaderTooltip="Activity" ASFormat="DynamicString" meta:resourcekey="ASGridBoundColumnResource3">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn DataField="EscalationDate" UniqueName="EscalationDate" HeaderText="Escalation Date"
                    HeaderTooltip="Escalation Date" ASFormat="DateAndTime" HeaderStyle-Width="150px" meta:resourcekey="ASGridBoundColumnResource4">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center" Width="150px"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn DataField="ActivityTime" UniqueName="ActivityTime" HeaderText="Activity Time"
                    HeaderTooltip="ActivityTime" ASFormat="DateAndTime" HeaderStyle-Width="150px" meta:resourcekey="ASGridBoundColumnResource5">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center" Width="150px"></HeaderStyle>
                </as:ASGridBoundColumn>
            </Columns>
        </MasterTableView>
    </as:ASGrid>
    <tek:RadCodeBlock ID="uxRadCodeBlock" runat="server">

        <script type="text/javascript">
            var UserActivity_uxUserList = "<%= uxUserList.ClientID %>";
            var UserActivity_uxDate = "<%= uxDate.ClientID %>";
            var UserActivity_ValidationMessages_V1 = "<%=Resources.MessageManager.ValidationMessages_V1 %>";
            var UserActivity_ValidationMessages_V2 = "<%=Resources.MessageManager.ValidationMessages_V2 %>";
            var UserActivity_uxMerchantNumber = "<%= uxMerchantNumber.ClientID %>";
            var UserActivity_ReportFilter_V2 = "<%=Resources.MessageManager.ReportFilter_V2%>";
            var UserActivity_ReportFilter_V4 = "<%=Resources.MessageManager.ReportFilter_V4%>";
            var UserActivity_ReportFilter_V7 = "<%=Resources.MessageManager.ReportFilter_V7%>";
            var UserActivity_ReportFilter_V9 = "<%=Resources.MessageManager.ReportFilter_V9%>";
            var UserActivity_js_RiskUser = '<%= GetLocalResourceObject("UserActivity_js_RiskUser").ToString()%>';
            var UserActivity_js_DateFilter = '<%= GetLocalResourceObject("UserActivity_js_DateFilter").ToString() %>';
            var UserActivity_js_MerchantNumber = '<%= GetLocalResourceObject("UserActivity_js_MerchantNumber").ToString() %>';
            
        </script>
        <script type="text/javascript" src="<%=ResolveUrl("~") %>res/js/UserActivity.js"></script>

    </tek:RadCodeBlock>
</asp:Content>

