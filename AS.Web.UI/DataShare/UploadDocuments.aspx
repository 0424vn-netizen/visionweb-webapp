<%@ Page Title="Upload Documents" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="UploadDocuments.aspx.cs" Inherits="UploadDocuments" EnableEventValidation="false" meta:resourcekey="PageResource1" %>

<%@ Register Src="~/UserControls/PageTitle.ascx" TagName="PageTitle" TagPrefix="uc" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="Server">
    <div class="row">
        <div class="col-md-12">
            <uc:PageTitle ID="uxPageTitle" runat="server" HasFilteringOption="false" ReportTitle="Upload Documents" meta:resourcekey="uxPageTitleResource1" />
        </div>
    </div>
    <div class="row">
        <div class="col-md-12">
            <as:Button runat="server" ID="uxCreateMode" class="btn btn-default" Text="CREATE" OnClientClick="return openUploadDocumentModal()"
                meta:resourcekey="uxCreateModeResource"></as:Button>
        </div>
    </div>
    <div class="height-15"></div>
    <div class="row">
        <div class="col-md-12">
            <as:Panel ID="uxFilterStatusContainer" runat="server" meta:resourcekey="uxFilterStatusContainerResource1">
                <div class="row">
                    <div class="col-md-12">
                        <as:RadioButton ID="uxFilterStatusAll" CssClass="control-inline" runat="server" Text="All"
                            GroupName="uxFilterStatus"
                            xValue="0" onclick="uxFilterStatus_Checked()" meta:resourcekey="uxFilterStatusAllResource1" Value="" />
                        <as:RadioButton ID="uxFilterStatusActive" runat="server" CssClass="control-inline"
                            Text="Active" GroupName="uxFilterStatus"
                            xValue="1" Checked="True" onclick="uxFilterStatus_Checked()" meta:resourcekey="uxFilterStatusActiveResource1" Value="" />
                        <as:RadioButton ID="uxFilterStatusExpired" runat="server" CssClass="control-inline"
                            Text="Expired" GroupName="uxFilterStatus"
                            xValue="02" onclick="uxFilterStatus_Checked()" meta:resourcekey="uxFilterStatusExpiredResource1" Value="" />
                        <asp:Button ID="uxChangeFilterStatus" runat="server" OnClick="uxChangeFilterStatus_Click"
                            Style="display: none" meta:resourcekey="uxChangeFilterStatusResource1"></asp:Button>
                    </div>
                </div>
            </as:Panel>
        </div>
    </div>
    <div class="height-15"></div>
    <div class="row">
        <div class="col-md-12">
            <as:ASGrid ID="uxReportGrid" runat="server" AllowPaging="True" IsAutoExportTemplate="true" ASPagingMethod="SPASingleMethod1"
                AllowSorting="True" AutoGenerateColumns="False" OnNeedDataSource="uxReportGrid_NeedDataSource"
                OnItemDataBound="uxReportGrid_ItemDataBound" CssClass="in" meta:resourcekey="uxReportGridResource1">
                <MasterTableView>
                    <Columns>
                        <as:ASGridTemplateColumn HeaderText="Document Name" DataField="DocumentName" UniqueName="DocumentName"
                                SortExpression="DocumentName" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="center"
                                ItemStyle-CssClass="ellipsis text-left" HeaderStyle-Width="200px"  meta:resourcekey="ASGridBoundColumnResource11">
                                <ItemTemplate>
                                    <asp:Label ID="uxDocumentName" runat="server" Text='<%# Eval("DocumentName") %>' ToolTip='<%# Eval("DocumentName") %>'></asp:Label>
                                </ItemTemplate>
                            </as:ASGridTemplateColumn>
                        <as:ASGridTemplateColumn HeaderText="Description" DataField="Description" UniqueName="Description"
                                SortExpression="Description" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="center"
                                ItemStyle-CssClass="ellipsis text-left" HeaderStyle-Width="200px"  meta:resourcekey="ASGridBoundColumnResource12">
                                <ItemTemplate>
                                    <asp:Label ID="uxDescription" runat="server" Text='<%# Eval("Description") %>' ToolTip='<%# Eval("Description") %>'></asp:Label>
                                </ItemTemplate>
                            </as:ASGridTemplateColumn>
                        <as:ASGridBoundColumn UniqueName="CreatedDate" HeaderText="Creation Date" DataField="CreatedDate"
                            ASFormat="Date" SortExpression="CreatedDate" HeaderStyle-Width="130px" 
                            meta:resourcekey="ASGridBoundColumnResource3">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text=""></ModelErrorMessage>
                            </ColumnValidationSettings>
                        </as:ASGridBoundColumn>
                        <as:ASGridBoundColumn UniqueName="ExpirationDate" HeaderText="Expiration Date" DataField="ExpirationDate"
                            ASFormat="Date" SortExpression="ExpirationDate" HeaderTooltip="Expiration Date" HeaderStyle-Width="130px" 
                            meta:resourcekey="ASGridBoundColumnResource4">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text=""></ModelErrorMessage>
                            </ColumnValidationSettings>
                        </as:ASGridBoundColumn>
                        <as:ASGridTemplateColumn HeaderText="Share With Users" DataField="ShareWithUsers" UniqueName="ShareWithUsers"
                            SortExpression="ShareWithUsers" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="center"
                            ItemStyle-CssClass="ellipsis text-left"  meta:resourcekey="ASGridBoundColumnResource5">
                            <ItemTemplate>
                                    <asp:Label ID="uxShareWithUsers" runat="server" Text='<%# Eval("ShareWithUsers") %>' ToolTip='<%# Eval("ShareWithUsers") %>'></asp:Label>
                            </ItemTemplate>
                        </as:ASGridTemplateColumn>
                        <as:ASGridTemplateColumn HeaderText="Share With Groups" DataField="ShareWithGroups" UniqueName="ShareWithGroups"
                                SortExpression="ShareWithGroups" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="center"
                                ItemStyle-CssClass="ellipsis text-left"   meta:resourcekey="ASGridBoundColumnResource6">
                                <ItemTemplate>
                                   <asp:Label ID="uxShareWithGroups" runat="server" Text='<%# Eval("ShareWithGroups") %>' ToolTip='<%# Eval("ShareWithGroups") %>'></asp:Label>
                                </ItemTemplate>
                            </as:ASGridTemplateColumn>
                        <as:ASGridTemplateColumn UniqueName="EditOrDelete" ItemStyle-HorizontalAlign="Center" HeaderText="Edit/Delete" HeaderStyle-HorizontalAlign="Center"
                            HeaderTooltip="Edit/Delete"  meta:resourcekey="ASGridBoundColumnResource7" HeaderStyle-Width="100px" >
                            <ItemTemplate>
                                <as:LinkButton runat="server" ID="uxEdit" Text="Edit" meta:resourcekey="lnkEdit"></as:LinkButton>
                                &nbsp;
                                <as:LinkButton runat="server" ID="uxDelete" Text="Delete" OnClientClick='<%# "return confirmDelete(\""+Eval("DocID")+"\")"%>' meta:resourcekey="lnkDelete"></as:LinkButton>
                            </ItemTemplate>
                        </as:ASGridTemplateColumn>
                    </Columns>
                </MasterTableView>
            </as:ASGrid>
        </div>
    </div>
    <as:HiddenField ID="hdDeleteDocument" Value="0" runat="server" />
    <asp:Button ID="btnDelete" runat="server" OnClick="btnDelete_Click" CssClass="hide" ></asp:Button>
    <as:RadAjaxManagerProxy ID="RadAjaxManager" runat="server">
        <AjaxSettings>
            <tek:AjaxSetting AjaxControlID="uxChangeFilterStatus">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="uxReportGrid" />
                </UpdatedControls>
            </tek:AjaxSetting>
            <tek:AjaxSetting AjaxControlID="btnDelete">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="uxReportGrid" />
                </UpdatedControls>
            </tek:AjaxSetting>
        </AjaxSettings>
    </as:RadAjaxManagerProxy>
    <tek:RadCodeBlock runat="server" ID="RadCodeBlock2">
        <script type="text/javascript">
            var uxChangeFilterStatus_ClientID = '<%= uxChangeFilterStatus.ClientID %>';
            var hdDeleteDocument = '<%= hdDeleteDocument.ClientID %>';
            var btnDelete = '<%= btnDelete.ClientID %>';
        </script>
        <script src="<%= ResolveUrl("~/")%>res/js/dataShare/uploadDocuments.js"></script>
    </tek:RadCodeBlock>
</asp:Content>
