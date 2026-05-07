<%@ Page Title="Export Queue" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="rm_MCF_MgmtReport_Exports.aspx.cs" Inherits="rm_MCF_MgmtReport_Exports" EnableEventValidation="false" EnableSessionState="ReadOnly" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="Server">
    <div class="main-content">
        <tek:RadAjaxManagerProxy runat="server" ID="uxRadAjaxManager">
            <AjaxSettings>
                <tek:AjaxSetting AjaxControlID="uxExportQueueGrid">
                    <UpdatedControls>
                        <tek:AjaxUpdatedControl ControlID="uxExportQueueGrid" LoadingPanelID="uxLoadingPanelCustom" />
                    </UpdatedControls>
                </tek:AjaxSetting>
                <tek:AjaxSetting AjaxControlID="btnRefreshGrid">
                    <UpdatedControls>
                        <tek:AjaxUpdatedControl ControlID="uxExportQueueGrid" />
                    </UpdatedControls>
                </tek:AjaxSetting>
            </AjaxSettings>
        </tek:RadAjaxManagerProxy>
        <as:PlaceHolder ID="uxPlContainer" runat="server">
            <div class="row">
                <div class="col-xs-10">
                    <h2 class="grid-title" data-toggle="collapse" data-target="#uxExportQueueGrid">Export Queue </h2>
                </div>
            </div>
            <as:Button runat="server" ID="btnRefreshGrid" CssClass="display-none" OnClick="btnRefreshGrid_Click" />
            <as:ASGrid ID="uxExportQueueGrid" runat="server" AutoGenerateColumns="false" AllowPaging="True"
                AllowSorting="false" PageSize="10"
                CssClass="in" ASPagingMethod="SPASingleMethod2"
                OnItemDataBound="uxExportQueueGrid_ItemDataBound"
                OnNeedDataSource="uxExportQueueGrid_NeedDataSource">
                <MasterTableView AllowSorting="false">
                    <Columns>
                        <as:ASGridBoundColumn HeaderText="Export Type" UniqueName="ReportType" DataField="ReportType"
                            HeaderStyle-HorizontalAlign="Center" HeaderTooltip="Export Type">
                        </as:ASGridBoundColumn>
                        <as:ASGridBoundColumn HeaderText="Export Option" UniqueName="FileType" DataField="FileType"
                            HeaderStyle-HorizontalAlign="Center" HeaderTooltip="Export Option">
                        </as:ASGridBoundColumn>
                        <as:ASGridTemplateColumn UniqueName="Download" HeaderText="Date/Time"
                            ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderTooltip="Date/Time">
                            <ItemTemplate>
                                <asp:LinkButton runat="server" ID="uxDownloadReport"
                                    OnClientClick='<%# ";doDownloadFile(\""+Eval("ListDocId")+"\",\""+Eval("FileName")+"\")"%>'
                                    Text='<%# String.Format("{0:MM/dd/yyyy hh:mm:ss tt}", Eval("CreatedDate")) %>'></asp:LinkButton>
                                <asp:Literal runat="server" ID="uxLitReportName" Text='<%# String.Format("{0:MM/dd/yyyy hh:mm:ss tt}",Eval("CreatedDate")) %>'></asp:Literal>
                            </ItemTemplate>
                        </as:ASGridTemplateColumn>
                        <as:ASGridBoundColumn DataField="StatusDesc" UniqueName="StatusDesc"
                            HeaderText="Status" ASFormat="StaticString">
                            <HeaderStyle HorizontalAlign="Center" />
                        </as:ASGridBoundColumn>
                        <as:ASGridTemplateColumn UniqueName="Delete" ItemStyle-HorizontalAlign="Center"
                            HeaderText="Delete" HeaderStyle-HorizontalAlign="Center" HeaderTooltip="Delete" HeaderStyle-Width="60px">
                            <ItemTemplate>
                                <asp:LinkButton runat="server" ID="uxDelete" Text="Delete"
                                    CommandArgument='<%# Eval("ProcessLogId") %>'
                                    OnCommand="DeleteExportQueue"
                                    OnClientClick="return ConfirmDelete();" />
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
                var exports_ConfirmDelete = "<%=Resources.MessageManager.ConfirmDeleteFile%>";
                var btnRefreshGrid = '<%= btnRefreshGrid.ClientID%>';
                var uxhdDocId = '<%= uxDocId.ClientID%>';
                var uxhdFileName = '<%= uxFileName.ClientID%>';
                var uxbtnDownload = '<%= uxbtnDownload.ClientID%>';
            </script>
            <script src="<%=ResolveUrl("~") %>res/js/risk_MCF/rm_MCF_MgmtReport_Exports.js"></script>
        </as:RadCodeBlock>
    </div>
</asp:Content>
