<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="rm_MCF_MgmtReport_Extracts.aspx.cs" Inherits="rm_MCF_MgmtReport_Extracts" EnableEventValidation="false" meta:resourcekey="PageResource1" %>

<%@ Register TagName="PageTitle" Src="~/UserControls/PageTitle.ascx" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/rm_MCF_MgmtReporting.ascx" TagName="ReportFilter"
    TagPrefix="uc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPage" runat="Server">
    <tek:RadAjaxManagerProxy runat="server" ID="uxRadAjaxManager">
        <AjaxSettings>
            <tek:AjaxSetting AjaxControlID="btnRefreshGrid">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="uxReportGrid" />
                </UpdatedControls>
            </tek:AjaxSetting>
            <tek:AjaxSetting AjaxControlID="uxReportGrid">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="uxReportGrid" />
                </UpdatedControls>
            </tek:AjaxSetting>
        </AjaxSettings>
    </tek:RadAjaxManagerProxy>

    <as:Button runat="server" ID="btnRefreshGrid" CssClass="display-none" OnClick="btnRefreshGrid_Click" />
    <uc:ReportFilter ID="uxReportFilter" OnSearch="uxReportFilter_Search" runat="server"></uc:ReportFilter>
    <div class="row">
        <div class="col-md-12">
            <uc:PageTitle ID="uxReportTitle" HasFilteringOption="true" runat="server" ReportTitle="Extracts" meta:resourcekey="uxReportTitleResource1" />
        </div>
    </div>
    <as:ASGrid ID="uxReportGrid" runat="server" AutoGenerateColumns="false" AllowPaging="false" Width="100%" ASPagingMethod="None"
        ShowPageTotal="false" OnNeedDataSource="uxReportGrid_NeedDataSource" OnItemDataBound="uxReportGrid_ItemDataBound" meta:resourcekey="uxReportGridResource1">
        <MasterTableView ShowFooter="false" AllowPaging="false" AllowSorting="false">
            <Columns>
                <as:ASGridBoundColumn HeaderText="Report Type" DataField="ReportType" UniqueName="ReportType"
                    ASFormat="StaticString" SortExpression="ReportType" HeaderTooltip="Report Type" meta:resourcekey="HierarchyTemplateColumnResource4">
                </as:ASGridBoundColumn>
                <as:ASGridTemplateColumn UniqueName="Download" HeaderText="Date/Time" HeaderTooltip="Date/Time" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" meta:resourcekey="HierarchyTemplateColumnResource1">
                    <ItemTemplate> 
                        <asp:LinkButton runat="server" OnClientClick='<%# "beforeDownloading();doDowloadFile(\""+Eval("DocId")+"\",\""+Eval("FileName")+"\")"%>'
                            ID="uxDownloadReport" Text='<%# String.Format("{0:MM/dd/yyyy hh:mm:ss tt}",Eval("CreatedDate")) %>'></asp:LinkButton>
                        <asp:Literal runat="server" ID="uxLitReportDate" Text='<%# String.Format("{0:MM/dd/yyyy hh:mm:ss tt}",Eval("CreatedDate")) %>'></asp:Literal>
                    </ItemTemplate>
                </as:ASGridTemplateColumn>
                <as:ASGridBoundColumn HeaderText="Status" DataField="Status" UniqueName="Status" Visible="false">
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn HeaderText="Status" DataField="StatusDesc" UniqueName="StatusDesc"
                    ASFormat="StaticString" SortExpression="StatusDesc" HeaderTooltip="Status" meta:resourcekey="HierarchyTemplateColumnResource2">
                </as:ASGridBoundColumn>
                <as:ASGridTemplateColumn UniqueName="Delete" ItemStyle-HorizontalAlign="Center" HeaderText="Delete" HeaderStyle-HorizontalAlign="Center" HeaderTooltip="Delete" meta:resourcekey="HierarchyTemplateColumnResource3" HeaderStyle-Width="60px"> 
                    <ItemTemplate>
                        <asp:LinkButton runat="server" ID="uxDelete" Text="Delete" CommandArgument='<%#Eval("LogId") + "," + Eval("DocId")%>' OnCommand="DeleteFileExport" OnClientClick="return ConfirmDelete();" meta:resourcekey="LinkButton2Resource1"></asp:LinkButton>
                    </ItemTemplate>
                </as:ASGridTemplateColumn>
            </Columns>
        </MasterTableView>
    </as:ASGrid>
    <asp:HiddenField runat="server" ID="uxDocId" />
    <asp:HiddenField runat="server" ID="uxFileName" />
    <as:Button runat="server" ID="uxbtnDownload" CssClass="display-none" OnClick="uxbtnDownload_Click" />

    <tek:RadCodeBlock runat="server" ID="RadCodeBlock">
        <script type="text/javascript">
            var rm_MgmtReport_Extracts_ConfirmDelete = "<%=Resources.MessageManager.ConfirmDeleteFile%>";
            var btnRefreshGrid = '<%= btnRefreshGrid.ClientID%>';
            var uxhdDocId = '<%= uxDocId.ClientID%>';
            var uxhdFileName = '<%= uxFileName.ClientID%>';
            var uxbtnDownload = '<%= uxbtnDownload.ClientID%>';
            
        </script>
        <script type="text/javascript" src="<%=ResolveUrl("~/") %>res/js/risk_MCF/rm_MCF_MgmtReport_Extracts.js"></script>
    </tek:RadCodeBlock>
</asp:Content>
