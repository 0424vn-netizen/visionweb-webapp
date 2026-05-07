<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ManageDocumentTypes.aspx.cs" Inherits="ManageDocumentTypes" meta:resourcekey="pageTitleresource" %>

<%@ Register TagName="UxExport" Src="~/UserControls/UxExport.ascx" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/PageTitle.ascx" TagName="ReportTitle" TagPrefix="uc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPage" runat="Server">

    <as:RadAjaxManagerProxy runat="server" ID="uxRadManager">
        <AjaxSettings>
            <tek:AjaxSetting AjaxControlID="uxAliceCMDocumebntGrid">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="uxAliceCMDocumebntGrid" />
                </UpdatedControls>
            </tek:AjaxSetting>
            <tek:AjaxSetting AjaxControlID="uxReloadGrid">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="uxAliceCMDocumebntGrid" />
                    <tek:AjaxUpdatedControl ControlID="uxExportAliceDocumebntGrid" />
                </UpdatedControls>
            </tek:AjaxSetting> 
        </AjaxSettings>
    </as:RadAjaxManagerProxy>


    <div class="row">
        <div class="col-md-12 no-margin-bottom">
            <uc:ReportTitle ID="ReportTitle1" runat="server" ReportTitle="Manage Document Types" HasFilteringOption="false" meta:resourcekey="uxReportTitleResourceManageDocumentTypes" />
        </div>
    </div>
    <div class="height-8"></div>
    <div id="uxSourceFilterContainer" runat="server" class="row dark-blue">
        <div class="col-md-12">
            <label class="control-inline">
                <asp:Literal runat="server" Text="Source:" meta:resourcekey="ltDocumentSource"></asp:Literal></label>
            <asp:RadioButton runat="server" ID="uxSourceAll" CssClass="control-inline" Text="All" GroupName="source" Checked="true" ValidationGroup="source" AutoPostBack="True" meta:resourcekey="uxSourceAllResource1" OnCheckedChanged="uxSourceAll_CheckedChanged" />
            <asp:RadioButton runat="server" ID="uxSourceAlice" CssClass="control-inline" Text="Alice" GroupName="source" ValidationGroup="source" AutoPostBack="True" meta:resourcekey="uxSourceAliceResource1" OnCheckedChanged="uxSourceAll_CheckedChanged" />
            <asp:RadioButton runat="server" ID="uxSourceCM" CssClass="control-inline" Text="Case Management" GroupName="source" ValidationGroup="source" AutoPostBack="True" meta:resourcekey="uxSourceCMResource1" OnCheckedChanged="uxSourceAll_CheckedChanged" />
        </div>
    </div>
    <div class="height-8"></div>
    <!--Create New Document Type -->
    <div class="row">
        <div class="col-md-12 no-margin-action-container" data-target=".create-rs-form" data-toggle="collapse">
            <as:LinkButton runat="server" ID="uxCreateDocumentType" OnClientClick="ShowPopupModal('CreateEditManageDocumentTypes.aspx', 'auto'); return false;" CssClass="btn btn-default" meta:resourcekey="uxCreateNewDocumentType">Create New Document Type</as:LinkButton>
        </div>
    </div>
    <!--  -->
    <div id="ciAliceDocument" class="in">

        <!--end Create New Document Type -->
        <div id="uxDivPannel" runat="server" onkeypress="return DefaultEnterOnDiv(event);">
            <uc:UxExport ID="uxExportAliceDocumebntGrid" runat="server" GridID="uxAliceCMDocumebntGrid" FileName="Alice Document Type" OnNeedExportConfig="uxExportAliceDocumebntGrid_NeedExportConfig"
                GridHeader="Manage Document Types" IsOnTop="true" meta:resourcekey="uxExportTopResource1" />
            <as:ASGrid ID="uxAliceCMDocumebntGrid" runat="server" ASPagingMethod="SPASingleMethod1" AllowPaging="True" GridLines="None"
                AllowSorting="True" AutoGenerateColumns="False" OnNeedDataSource="uxAliceDocumebntGrid_NeedDataSource"
                AllowAutomaticDeletes="True" AllowAutomaticUpdates="True" IsAutoExportTemplate="true" IsCacheTemplateFile="false"
                CssClass="in" meta:resourcekey="uxRadGridResource1" OnItemDataBound="uxAliceDocumebntGrid_ItemDataBound">
                <MasterTableView DataKeyNames="DocumentTypeID" GridLines="None" ShowFooter="false">
                    <Columns>
                        <as:ASGridBoundColumn HeaderText="Source" DataField="SourceName" UniqueName="SourceName"
                            SortExpression="SourceName" ASFormat="DynamicString" HeaderTooltip="Source"
                            meta:resourcekey="SourceResource" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="200px" ItemStyle-Width="200px">
                        </as:ASGridBoundColumn>
                        <as:ASGridBoundColumn HeaderText="Document Type" DataField="DocumentType" UniqueName="DocumentType"
                            SortExpression="DocumentType" ASFormat="DynamicString" HeaderTooltip="Document Type" ItemStyle-Wrap="true" 
                            meta:resourcekey="DocumentTypeResource1" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="350px" ItemStyle-Width="350px">
                        </as:ASGridBoundColumn>
                        <as:ASGridBoundColumn HeaderText="Description" DataField="Description" UniqueName="Description"
                            SortExpression="Description" ASFormat="DynamicString" HeaderTooltip="Description"
                            AllowEncodeOnExporting="true" meta:resourcekey="DescriptionResource1" />
                        <as:ASGridBoundColumn HeaderText="Active" DataField="ActiveStatus" UniqueName="ActiveStatus" ASFormat="StaticString" ItemStyle-Wrap="true" 
                            SortExpression="ActiveStatus" HeaderTooltip="Active" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="100px" ItemStyle-Width="100px"
                            meta:resourcekey="ActiveResource1" ItemStyle-HorizontalAlign="Center">
                        </as:ASGridBoundColumn>
                        <as:ASGridTemplateColumn HeaderText="Edit" UniqueName="Edit" DataField="Edit" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="100px" ItemStyle-Width="100px"
                            HeaderTooltip="Edit"
                            ItemStyle-HorizontalAlign="Center" meta:resourcekey="EditColumnResourcekey">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkEdit" runat="server" Text="Edit" meta:resourcekey="LinkButtonEditResource" />
                            </ItemTemplate>
                        </as:ASGridTemplateColumn>
                    </Columns>
                </MasterTableView>
            </as:ASGrid>
        </div>
    </div>
    <div class="hide">
        <asp:Button ID="uxReloadGrid" runat="server" OnClick="uxReloadGrid_Click" />
    </div>
    <as:ASRadCodeBlock ID="RadCodeBlock" runat="server">
        <script type="text/javascript">
            var uxAliceDocumebntGrid_ClientID = '<%= uxAliceCMDocumebntGrid.ClientID%>';
            var uxReloadGrid_ClientID = '<%= uxReloadGrid.ClientID%>'; 
            function ajaxRequestStart(sender, args) {
                var target = sender.__EVENTTARGET;
                if (target.indexOf("imgExcel") >= 0 || target.indexOf("imgCSV") >= 0 || target.indexOf("imgPDF") >= 0) {
                    args.set_enableAjax(false);
                }
            }
        </script>
        <script src="<%= ResolveUrl("~/")%>res/js/ManageDocumentTypes.js"> 
        </script>
    </as:ASRadCodeBlock>
</asp:Content>

