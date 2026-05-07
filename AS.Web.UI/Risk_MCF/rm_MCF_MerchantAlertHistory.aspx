<%@ Page Title="Merchant Alert Historical" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="rm_MCF_MerchantAlertHistory.aspx.cs" Inherits="rm_MCF_MerchantAlertHistory" meta:resourcekey="PageResource1" %>

<%@ Register Src="~/UserControls/rm_MCF_MerchantAlertHistory_ReportFilter.ascx" TagName="ReportFilter"
    TagPrefix="uc" %>
<%@ Register Src="~/UserControls/PageTitle.ascx" TagName="PageTitle" TagPrefix="uc" %>
<%--<%@ Register Src="~/UserControls/UxExport.ascx" TagName="UxExport" TagPrefix="uc" %>--%>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="Server">
    <div class="main-content">
        <tek:RadAjaxManagerProxy runat="server" ID="uxRadAjaxManager">
            <AjaxSettings>
                <tek:AjaxSetting AjaxControlID="uxReportGrid">
                    <UpdatedControls>
                        <tek:AjaxUpdatedControl ControlID="uxReportGrid" LoadingPanelID="uxLoadingPanelCustom" />
                    </UpdatedControls>
                </tek:AjaxSetting>
                <tek:AjaxSetting AjaxControlID="btnRefreshGrid">
                    <UpdatedControls>
                        <tek:AjaxUpdatedControl ControlID="uxReportGrid" />
                    </UpdatedControls>
                </tek:AjaxSetting>
            </AjaxSettings>
        </tek:RadAjaxManagerProxy>
        <as:PlaceHolder ID="uxPlContainer" runat="server">
            <uc:ReportFilter ID="uxReportFilter" runat="server" OnSubmitFiltering="OnSearchEvent" VisibleAssignmentFilter="true" VisibleParameterFilter="true" VisibleMerchantFilter="true"></uc:ReportFilter>
            <div class="row" id="merchant-alert-title">
                <div class="col-xs-10">
                    <h2 class="grid-title on-top" data-toggle="collapse" data-target="#ucMerchantAlertGrid" aria-expanded="true">
                        <as:Literal ID="litGridTitle" runat="server" meta:resourcekey="litGridTitleResource1" />
                        <span class="text-muted">
                            <as:Literal ID="litGridSubTitle" runat="server" meta:resourcekey="litGridSubTitleResource1" />
                        </span>
                    </h2>
                </div>
                <%--           <uc:UxExport ID="uxExporter" runat="server" GridID="uxReportGrid" ShowPDF="true"
                    ShowWord="false" IsOnTop="true" />--%>
            </div>
            <div class="row">
                <div class="col-xs-12 alert-by-cycle">
                    <asp:Literal ID="lblSubTitle" runat="server" meta:resourcekey="AlertByRiskCycle"></asp:Literal>
                </div>
            </div>
            <as:Button runat="server" ID="btnRefreshGrid" CssClass="display-none" OnClick="btnRefreshGrid_Click" />
            <as:ASGrid ID="uxReportGrid" runat="server" AutoGenerateColumns="false" AllowPaging="false" Width="100%" ASPagingMethod="None"
                ShowPageTotal="false" OnItemDataBound="uxReportGrid_ItemDataBound" OnNeedDataSource="uxReportGrid_NeedDataSource" meta:resourcekey="uxReportGridResource1">
                <MasterTableView ShowFooter="false" AllowPaging="false" AllowSorting="false">
                    <Columns>
                        <as:ASGridTemplateColumn HeaderText="Report Name" DataField="FileName" UniqueName="FileName" HeaderStyle-HorizontalAlign="Center"
                            SortExpression="FileName" HeaderTooltip="Report Name" meta:resourcekey="HierarchyTemplateColumnResource4">
                            <ItemTemplate>
                                <asp:LinkButton runat="server" OnClientClick='<%# "doDowloadFile(\""+Eval("ListDocId")+"\",\""+Eval("FileName")+"\")"%>'
                                    ID="uxDownloadReport" Text='<%# String.Format("{0}",Eval("FileName")) %>'></asp:LinkButton>
                                <asp:Literal runat="server" ID="uxLitReportName" Text='<%# String.Format("{0}",Eval("FileName")) %>'></asp:Literal>
                            </ItemTemplate>
                        </as:ASGridTemplateColumn>
                        <as:ASGridTemplateColumn UniqueName="Download" HeaderText="Date/Time" HeaderTooltip="Date/Time" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" meta:resourcekey="HierarchyTemplateColumnResource1">
                            <ItemTemplate>
                                <asp:Literal runat="server" ID="uxLitReportDate" Text='<%# String.Format("{0:MM/dd/yyyy hh:mm:ss tt}",Eval("CreatedDate")) %>'></asp:Literal>
                            </ItemTemplate>
                        </as:ASGridTemplateColumn>

                        <as:ASGridBoundColumn HeaderText="Status" DataField="Status" UniqueName="Status" Visible="false">
                        </as:ASGridBoundColumn>
                        <as:ASGridBoundColumn HeaderText="Status" DataField="StatusDesc" UniqueName="StatusDesc"
                            ASFormat="StaticString" SortExpression="StatusDesc" HeaderTooltip="Status" meta:resourcekey="HierarchyTemplateColumnResource2">
                        </as:ASGridBoundColumn>
                        <as:ASGridTemplateColumn UniqueName="Delete" ItemStyle-HorizontalAlign="Center" HeaderText="Delete" HeaderStyle-HorizontalAlign="Center" HeaderTooltip="Delete" meta:resourcekey="HierarchyTemplateColumnResource3" HeaderStyle-Width="140px">
                            <ItemTemplate>
                                <asp:LinkButton runat="server" ID="uxDelete" Text="Delete" CommandArgument='<%#Eval("LogId") + "-" + Eval("ListDocId")%>' OnCommand="DeleteFileExport" OnClientClick="return ConfirmDelete();" meta:resourcekey="LinkButton2Resource"></asp:LinkButton>
                                <asp:LinkButton runat="server" ID="uxRetry" Text="Retry" CommandArgument='<%#Eval("LogId")%>' OnCommand="RetryReport" CssClass="retry-button"  meta:resourcekey="LinkButtonRetry"></asp:LinkButton>
                            </ItemTemplate>
                        </as:ASGridTemplateColumn>
                    </Columns>
                </MasterTableView>
            </as:ASGrid>

        </as:PlaceHolder>
        <asp:HiddenField runat="server" ID="uxDocId" />
        <asp:HiddenField runat="server" ID="uxFileName" />
        <as:Button runat="server" ID="uxbtnDownload" CssClass="display-none" OnClick="uxbtnDownload_Click" />
        <as:RadCodeBlock ID="ScriptManagement" runat="server">
            <script>
                var uxReportGridID = "<%=uxReportGrid.ClientID%>";
                var rm_MgmtReport_Extracts_ConfirmDelete = "<%=Resources.MessageManager.ConfirmDeleteFile%>";
                var btnRefreshGrid = '<%= btnRefreshGrid.ClientID%>';
                var uxhdDocId = '<%= uxDocId.ClientID%>';
                var uxhdFileName = '<%= uxFileName.ClientID%>';
                var uxbtnDownload = '<%= uxbtnDownload.ClientID%>';
            </script>
            <script src="<%=ResolveUrl("~") %>res/js/risk_MCF/rm_MCF_MerchantAlertHistory.js"></script>
        </as:RadCodeBlock>
        <style>
            .retry-button{
                margin-left: 5px;
            }
        </style>
    </div>
</asp:Content>

