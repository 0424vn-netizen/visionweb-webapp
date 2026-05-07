<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Title="Manage Roles"
    CodeFile="ManageRoles.aspx.cs" Inherits="_mps_ManageRoles" ValidateRequest="false" meta:resourcekey="PageResource1" %>

<%@ Register TagName="UxExport" Src="~/UserControls/UxExport.ascx" TagPrefix="uc" %>
<%@ Register TagName="PageTitle" Src="~/UserControls/PageTitle.ascx" TagPrefix="uc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPage" runat="Server">
    <tek:RadAjaxManagerProxy ID="RadAjaxManager1" runat="server">
        <AjaxSettings>
            <tek:AjaxSetting AjaxControlID="uxRebinData">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="uxReportGrid" />
                </UpdatedControls>
            </tek:AjaxSetting>
        </AjaxSettings>
    </tek:RadAjaxManagerProxy>

    <div class="row">
        <div class="col-md-12 no-margin-bottom">
            <uc:PageTitle ID="uxReportTitle" runat="server" HasFilteringOption="false" ReportTitle="Manage Roles" meta:resourcekey="uxReportTitleResource1" />
            <div class="height-22"></div>
            <i>
                <as:Literal ID="ltClickOnUserToEdit" runat="server" Text="** Click on a User Role to Edit the Role" meta:resourcekey="ltClickOnUserToEditResource1"></as:Literal></i>
        </div>
    </div>
    <div class="row">
        <div class="col-md-12 action-container">
            <asp:PlaceHolder ID="uxCreateCSRolePlaceHolder" runat="server">
                <div class="control-inline">
                    <a href="#" class="btn btn-default" onclick="return ShowPopupModal('CreateRole_Modal.aspx',800);">
                        <%=HasMSUserManagement ? GetLocalResourceObject("ManageRoles_aspx_CreateNewCSRole").ToString() : GetLocalResourceObject("ManageRoles_aspx_CreateNewRole").ToString() %>
                    </a>
                </div>
            </asp:PlaceHolder>
            <asp:PlaceHolder ID="uxCreateMSRolePlaceHolder" runat="server" Visible="False">
                <div class="control-inline">
                    <a href="#" class="btn btn-default" onclick="return ShowPopupModal('CreateMSRole_Modal.aspx','auto');">
                        <as:Literal ID="ltCreateNewMSRole" runat="server" Text="Create
                    New MS User Role"
                            meta:resourcekey="ltCreateNewMSRoleResource1"></as:Literal>
                    </a>
                </div>
            </asp:PlaceHolder>
        </div>
    </div>
    <div class="height-24"></div>
    <!--Rad Grid-->
    <div class="row">
        <div class="col-md-12">
            <uc:UxExport ID="uxExporter" IsOnTop="true" runat="server" GridHeader="User Roles"
                GridID="uxReportGrid" meta:resourcekey="uxExporterResource1" />
            <!--TK:41265 – FIS Client Maintenance for 2018 -->
            <as:ASGrid ID="uxReportGrid" runat="server" GridLines="None" Width="100%" AllowPaging="True" IsAutoExportTemplate="true"
                AllowSorting="True" AutoGenerateColumns="false" AllowSortFilterWhenExport="true" HeaderStyle-Width="100px"
                IntruderSourceName="uxReportGrid" IsIntruder="true" ASPagingMethod="None" ShowFooter="false"
                OnItemCommand="uxReportGrid_ItemCommand" AllowExportAtWebServices="false">
                <MasterTableView DataKeyNames="HierarchyID">
                    <PagerStyle AlwaysVisible="true" />
                    <Columns>
                        <as:ASGridBoundColumn HeaderText="User Role" DataField="HierarchyName" UniqueName="HierarchyName"
                            ASFormat="DynamicString" SortExpression="HierarchyName" HeaderTooltip="User Role"
                            AllowEncodeOnExporting="true" meta:resourcekey="ASGridBoundColumnResource1">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text=""></ModelErrorMessage>
                            </ColumnValidationSettings>

                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        </as:ASGridBoundColumn>
                        <as:ASGridBoundColumn HeaderText="User Role Type" DataField="UserRoleType" UniqueName="UserRoleType"
                            ASFormat="StaticString" SortExpression="UserRoleType" HeaderTooltip="User Role Type"
                            Visible="false" meta:resourcekey="ASGridBoundColumnResource2">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text=""></ModelErrorMessage>
                            </ColumnValidationSettings>

                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        </as:ASGridBoundColumn>
                        <as:ASGridBoundColumn HeaderText="Description" DataField="HierarchyDescription" UniqueName="HierarchyDescription"
                            ASFormat="DynamicString" AllowEncodeOnExporting="true" SortExpression="HierarchyDescription"
                            HeaderTooltip="Description" meta:resourcekey="ASGridBoundColumnResource3">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text=""></ModelErrorMessage>
                            </ColumnValidationSettings>

                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        </as:ASGridBoundColumn>
                        <as:ASGridBoundColumn HeaderText="# Users" DataField="UserCount" UniqueName="UserCount"
                            ASFormat="Integer" SortExpression="UserCount" AllowSorting="false"
                            HeaderTooltip="# Users assigned to the Role" meta:resourcekey="ASGridBoundColumnResource4" />
                        <%-- <as:ASGridBoundColumn HeaderText="Delete" UniqueName="Delete"
                        ASFormat="StaticString" HeaderTooltip="Click to delete role" Visible="false" />--%>
                        <as:ASGridTemplateColumn HeaderText="Delete" UniqueName="Delete" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center"
                            HeaderTooltip="Click to delete role" Visible="false" meta:resourcekey="ASGridTemplateColumnResource1">
                            <ItemTemplate>
                                <as:LinkButton runat="server" ID="uxDelete" Text="Delete" CommandName="DeleteMSRole" OnClientClick="return DeleteConfirmation();" meta:resourcekey="uxDeleteResource1"></as:LinkButton>
                            </ItemTemplate>
                        </as:ASGridTemplateColumn>
                    </Columns>
                </MasterTableView>
            </as:ASGrid>
            <asp:Button ID="uxRebinData" runat="server" OnClick="uxRebinData_Click" Style="display: none" meta:resourcekey="uxRebinDataResource1" />
        </div>
    </div>

    <tek:RadCodeBlock ID="radCodeBlock" runat="server">
        <script type="text/javascript">
            var dialogs = null;
            var ManageRole_uxRebinData_ClientID = '<%=uxRebinData.ClientID %>';
            var MangaRole_uxReportGrid_ClientID = '<%=uxReportGrid.ClientID %>';
            var ManageRoles_js_DeleteRole = '<%= GetLocalResourceObject("ManageRoles_js_DeleteRole").ToString() %>';
            var ManageRole_PrefinedRole = '<%=Resources.MessageManager.ManageRole_PrefinedRole%>';
        </script>
        <script src="<%= ResolveUrl("~/")%>res/js/usermaintenance/ManageRoles.js"></script>
    </tek:RadCodeBlock>
    <!-- Rad Ajax-->
    <tek:RadAjaxManagerProxy runat="server" ID="uxRadAjaxManager">
        <AjaxSettings>
            <tek:AjaxSetting AjaxControlID="uxReportGrid">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="uxReportGrid" />
                </UpdatedControls>
            </tek:AjaxSetting>
        </AjaxSettings>
    </tek:RadAjaxManagerProxy>
</asp:Content>
