<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" 
    CodeFile="MyDocuments.aspx.cs" Inherits="DataShare_MyDocuments" meta:resourcekey="PageResource1" %>

<%@ Register Src="~/UserControls/PageTitle.ascx" TagName="PageTitle" TagPrefix="uc" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" Runat="Server">
    <div class="row">
        <div class="col-md-12">
            <uc:PageTitle ID="uxPageTitle" runat="server" HasFilteringOption="false" ReportTitle="My Documents" meta:resourcekey="uxPageTitleResource1" />
        </div>
    </div>
    <div class="row">
        <div class="col-md-12">
            <as:ASGrid ID="uxMyDocumentGrid" runat="server" OnNeedDataSource="uxMyDocumentGrid_NeedDataSource"
            AllowPaging="true" AllowSorting="true" ASPagingMethod="SPASingleMethod1" AutoGenerateColumns="false" AllowSortFilterWhenExport="true" CssClass="in" meta:resourcekey="uxReportGridResource1">
            <MasterTableView>
                <Columns>
                    <as:ASGridTemplateColumn  HeaderStyle-Width="250px" UniqueName="DocumentName" 
                        SortExpression="DocumentName" HeaderText="Document Name"
                        ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="text-left" HeaderStyle-HorizontalAlign="Center" meta:resourcekey="ASGridBoundColumnResource1">
                        <ItemTemplate>
                            <div class="ellipsis" title=" <%#Eval("DocumentName") %>">
                                <asp:LinkButton runat="server" OnClientClick='<%# "doDowloadFile(\""+Eval("DocID")+"\",\""+Eval("DocumentName")+"\")"%>'
                                    ID="uxDownloadReportName" Text='<%# String.Format("{0}",Eval("DocumentName")) %>'></asp:LinkButton>
                            </div>
                        </ItemTemplate>
                    </as:ASGridTemplateColumn>

                    <as:ASGridTemplateColumn UniqueName="Description" HeaderStyle-Width="300px"  SortExpression="Description" 
                        HeaderText="Document Description" ItemStyle-CssClass="ellipsis text-left"  ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" meta:resourcekey="ASGridBoundColumnResource2">
                        <ItemTemplate>
                            <asp:Label ID="uxDescription" runat="server" Text='<%# Eval("description") %>' ToolTip='<%# Eval("description") %>'></asp:Label>
                        </ItemTemplate>
                    </as:ASGridTemplateColumn>

                    <as:ASGridBoundColumn HeaderStyle-Width="100px" HeaderText="Creation Date" DataField="CreatedDate" UniqueName="CreatedDate"
                        SortExpression="CreatedDate" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="center"
                        HeaderTooltip="Creation Date" ASFormat="Date" AllowEncodeOnExporting="true" meta:resourcekey="ASGridBoundColumnResource3" />

                    <as:ASGridBoundColumn HeaderStyle-Width="100px" HeaderText="Expiration Date" DataField="ExpirationDate" UniqueName="ExpirationDate"
                        SortExpression="ExpirationDate" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="center"
                        ASFormat="Date" AllowEncodeOnExporting="true" meta:resourcekey="ASGridBoundColumnResource4" />

                    <as:ASGridTemplateColumn UniqueName="UploadedBy" HeaderStyle-Width="200px"  SortExpression="UploadedBy"
                        HeaderText="Uploaded By" ItemStyle-CssClass="ellipsis text-left" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" meta:resourcekey="ASGridBoundColumnResource5">
                        <ItemTemplate>
                            <asp:Label ID="uxUploadedBy" runat="server" Text='<%# Eval("UploadedBy") %>' ToolTip='<%# Eval("UploadedBy") %>'></asp:Label>
                        </ItemTemplate>
                    </as:ASGridTemplateColumn>

                </Columns>
            </MasterTableView>
        </as:ASGrid>
        </div>
    </div>
    <as:RadAjaxManagerProxy ID="RadAjaxManager" runat="server">
        <AjaxSettings>
            <tek:AjaxSetting AjaxControlID="uxMyDocumentGrid">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="uxMyDocumentGrid" />
                </UpdatedControls>
            </tek:AjaxSetting>
        </AjaxSettings>
    </as:RadAjaxManagerProxy>
    <asp:HiddenField runat="server" ID="uxDocId" />
    <asp:HiddenField runat="server" ID="uxFileName" />
    <asp:Button runat="server" ID="uxbtnDownload" CssClass="printOnly hide" OnClick="uxbtnDownload_Click" />
    <tek:RadCodeBlock runat="server" ID="RadCodeBlock2">
        <script type="text/javascript">
            var uxhdDocId = '<%= uxDocId.ClientID%>';
            var uxhdFileName = '<%= uxFileName.ClientID%>';
            var uxbtnDownload = '<%= uxbtnDownload.ClientID%>';
        </script>
        <script src="<%= ResolveUrl("~/")%>res/js/dataShare/myDocuments.js"></script>
    </tek:RadCodeBlock>
</asp:Content>

