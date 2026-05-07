<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="ManageUsers.aspx.cs" Inherits="_mps_ManageUsers" ValidateRequest="false" Title="Manage Users" meta:resourcekey="PageResource1" %>

<%@ Register TagName="UxExport" Src="~/UserControls/UxExport.ascx" TagPrefix="uc" %>
<%@ Register TagName="PageTitle" Src="~/UserControls/PageTitle.ascx" TagPrefix="uc" %>
<%@ Register TagName="ManageUserFiltering" Src="~/UserControls/ManageUserFiltering.ascx"
    TagPrefix="uc" %>
<%@ Register Src="~/UserControls/ManageUsersMasterUserControl.ascx" TagName="ManageUsersMasterUserControl" TagPrefix="uc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPage" runat="Server">
    <!-- Rad Ajax-->
    <tek:RadAjaxManagerProxy runat="server" ID="uxRadAjaxManager">
        <AjaxSettings>
            <tek:AjaxSetting AjaxControlID="uxReportGrid">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="uxReportGrid" />
                    <tek:AjaxUpdatedControl ControlID="uxExporter" />
                </UpdatedControls>
            </tek:AjaxSetting>
        </AjaxSettings>
        <AjaxSettings>
            <tek:AjaxSetting AjaxControlID="uxSiteJumpLinkBtn">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="uxReportGrid" />
                </UpdatedControls>
            </tek:AjaxSetting>
        </AjaxSettings>
    </tek:RadAjaxManagerProxy>

    <!-- Filtering  -->
    <asp:PlaceHolder ID="uxPlaceHolderReportFilter" runat="server">
        <uc:ManageUserFiltering ID="uxReportFiltering" runat="server" />
    </asp:PlaceHolder>

    <div class="row">
        <div class="col-md-12 no-margin-bottom">
            <uc:PageTitle ID="uxReportTitle" HasFilteringOption="true" runat="server" ReportTitle="Manage Users" meta:resourcekey="uxReportTitleResource1" />
            <div class="height-22"></div>
            <i runat="server" id="idSubTitle">
                <as:Literal ID="ltWhatsup" runat="server" Text="** Click on a User Name to Edit User" meta:resourcekey="ltWhatsupResource1"></as:Literal></i>
        </div>
    </div>
    <div class="row">
        <div class="col-md-12 action-container">
            <a id="uxCreateNewUser" class="btn btn-default" runat="server" href="#">
                <as:Literal ID="Literal1" runat="server" Text="Create New User" meta:resourcekey="Literal1Resource1"></as:Literal></a><asp:Literal
                    ID="uxFullUser" runat="server" meta:resourcekey="uxFullUserResource1" />
        </div>
    </div>
    <div class="height-24">
    </div>


    <!--Rad Grid-->
    <div class="row">
        <div class="col-md-12">
            <uc:UxExport ID="uxExporter" IsOnTop="true" runat="server" GridHeader="User List"
                GridID="uxReportGrid" meta:resourcekey="uxExporterResource1" /> 
            <as:ASGrid ID="uxReportGrid" runat="server" GridLines="None" AllowPaging="True" AllowSorting="True"
                AutoGenerateColumns="false" AllowSortFilterWhenExport="true" BorderWidth="1" XOverFlowable="true" Width="100%"
                IntruderSourceName="uxReportGrid" IsIntruder="true" ASPagingMethod="SPASingleMethod" AllowExportAtWebServices="false" HeaderStyle-Width="100px"
                ShowFooter="false" AllowFilteringByColumn="true" CssClass="in" meta:resourcekey="uxReportGridResource1"
                CustomBuildFilterExpressionWithSquareBrackets ="true">
                <MasterTableView>
                    <Columns>
                        <as:ASGridBoundColumn HeaderText="User Name" DataField="UserID" UniqueName="UserID" HeaderStyle-Width="190px"
                            FilterControlWidth="100px" SortExpression="UserID" ASFormat="StaticString" HeaderTooltip="User Name" meta:resourcekey="ASGridBoundColumnResource1">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text=""></ModelErrorMessage>
                            </ColumnValidationSettings>

                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            <ItemStyle HorizontalAlign="left" />
                        </as:ASGridBoundColumn>
                        <as:ASGridBoundColumn HeaderText="First Name" DataField="UserNameFirst" UniqueName="UserNameFirst" HeaderStyle-Width="130px"
                            SortExpression="UserNameFirst" ASFormat="StaticString" HeaderTooltip="First Name"
                            AllowEncodeOnExporting="true" meta:resourcekey="ASGridBoundColumnResource2">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text=""></ModelErrorMessage>
                            </ColumnValidationSettings>

                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            <ItemStyle HorizontalAlign="left" />
                        </as:ASGridBoundColumn>
                        <as:ASGridBoundColumn HeaderText="Last Name" DataField="UserNameLast" UniqueName="UserNameLast" HeaderStyle-Width="130px"
                            SortExpression="UserNameLast" ASFormat="StaticString" HeaderTooltip="Last Name"
                            AllowEncodeOnExporting="true" meta:resourcekey="ASGridBoundColumnResource3">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text=""></ModelErrorMessage>
                            </ColumnValidationSettings>

                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            <ItemStyle HorizontalAlign="left" />
                        </as:ASGridBoundColumn>
                        <as:ASGridBoundColumn HeaderText="Email" AllowFiltering="false" DataField="Email" HeaderStyle-Width="250px"
                            UniqueName="Email" ASFormat="StaticString" HeaderTooltip="Email address" SortExpression="Email"
                            AllowEncodeOnExporting="true" meta:resourcekey="ASGridBoundColumnResource4">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text=""></ModelErrorMessage>
                            </ColumnValidationSettings>

                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            <ItemStyle HorizontalAlign="left" />
                        </as:ASGridBoundColumn>
                        <as:ASGridBoundColumn HeaderText="User Type" DataField="FullUserType" UniqueName="FullUserType"
                            ASFormat="StaticString" HeaderTooltip="User Type" meta:resourcekey="ASGridBoundColumnResource5">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text=""></ModelErrorMessage>
                            </ColumnValidationSettings>

                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        </as:ASGridBoundColumn>
                        <as:ASGridBoundColumn HeaderText="User Role" DataField="HierarchyName" UniqueName="HierarchyName" HeaderStyle-Width="130px"
                            ASFormat="StaticString" HeaderTooltip="User Role" SortExpression="HierarchyName" meta:resourcekey="ASGridBoundColumnResource6">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text=""></ModelErrorMessage>
                            </ColumnValidationSettings>

                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            <ItemStyle HorizontalAlign="left" />
                        </as:ASGridBoundColumn>
                        <as:ASGridBoundColumn AllowFiltering="false" HeaderText="First Log in" UniqueName="FirstLogin"
                            DataField="FirstLogin" ASFormat="Date" HeaderTooltip="First Log in" meta:resourcekey="ASGridBoundColumnResource7">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text=""></ModelErrorMessage>
                            </ColumnValidationSettings>

                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        </as:ASGridBoundColumn>
                        <as:ASGridBoundColumn AllowFiltering="false" HeaderText="Latest Log in" UniqueName="LastLoginDTS"
                            DataField="LastLoginDTS" ASFormat="Date" HeaderTooltip="Latest Log in" meta:resourcekey="ASGridBoundColumnResource8">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text=""></ModelErrorMessage>
                            </ColumnValidationSettings>

                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        </as:ASGridBoundColumn>
                        <as:ASGridBoundColumn AllowFiltering="false" HeaderText="Locked Out" UniqueName="LockedOut"
                            ASFormat="Date" DataField="LockedOut" HeaderTooltip="Locked Out" meta:resourcekey="ASGridBoundColumnResource9">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text=""></ModelErrorMessage>
                            </ColumnValidationSettings>

                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        </as:ASGridBoundColumn>
                        <as:ASGridBoundColumn AllowFiltering="false" HeaderText="Reset PWD" UniqueName="ResetPassword" HeaderStyle-Width="170px"
                            ASFormat="StaticString" HeaderTooltip="Reset Password" ItemStyle-Wrap="false" meta:resourcekey="ASGridBoundColumnResource10">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text=""></ModelErrorMessage>
                            </ColumnValidationSettings>

                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                            <ItemStyle Wrap="False"></ItemStyle>
                        </as:ASGridBoundColumn>
                        <as:ASGridTemplateColumn AllowFiltering="false" HeaderText="Site Access" UniqueName="SiteAccess" HeaderStyle-Width="95px"
                            HeaderTooltip="Site Access" Visible="false" ItemStyle-Wrap="false" meta:resourcekey="ASGridTemplateColumnResource1">
                            <ItemTemplate>
                                <asp:LinkButton ID="uxSiteJumpLinkBtn" runat="server" OnCommand="uxSiteAccess_Click" Visible="false"
                                    CommandArgument='<%# Eval("UserID") %>' meta:resourcekey="uxSiteJumpLinkBtnResource1">
                                Site Access                            
                                </asp:LinkButton>
                            </ItemTemplate>

                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                        </as:ASGridTemplateColumn>
                        <as:ASGridTemplateColumn HeaderText="Status" UniqueName="ActiveStatusDescription" DataField="ActiveStatusDescription" HeaderStyle-Width="120px"
                            HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderTooltip="Active/Inactive User"
                            SortExpression="ActiveStatusDescription" meta:resourcekey="ASGridTemplateColumnResource2">
                            <ItemTemplate>
                                <as:LinkButton ID="uxActiveStatus" runat="server" Text='<%# Eval("ActiveStatusDescription") %>'
                                    OnCommand="ActivateDeactivateCommand" meta:resourcekey="uxActiveStatusResource1"></as:LinkButton>

                                <asp:Label runat="server" ID="uxLiteralActiveStatus"></asp:Label>

                                <as:HiddenField ID="uxHierarchyCode" runat="server" Value='<%# WebServices.SecurityServices.EncryptText(Eval("HierarchyCode").ToString())%>' />
                            </ItemTemplate>

                            <HeaderStyle HorizontalAlign="Center" Width="120px"></HeaderStyle>

                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                        </as:ASGridTemplateColumn>
                    </Columns>
                </MasterTableView>
            </as:ASGrid>
        </div>
    </div>

    <uc:ManageUsersMasterUserControl runat="server" ID="uxManageUsersMasterUserControl" />

    <asp:Button ID="uxRebinData" runat="server" OnClick="uxRebinData_Click" class="hide" meta:resourcekey="uxRebinDataResource1" />
    <asp:HiddenField ID="hddActiveUserID" runat="server" />
    <asp:HiddenField ID="hddSystemId" runat="server" />
    <asp:HiddenField ID="hddIsSecondaryUser" runat="server" />
    <asp:Button ID="uxActiveDecactive" runat="server" class="hide" OnCommand="uxActiveDecactive_Click" meta:resourcekey="uxActiveDecactiveResource1"></asp:Button>
    <tek:RadCodeBlock ID="radCodeBlock" runat="server">
        <script type="text/javascript">
            var uxRebinData_ClientID = '<%= uxRebinData.ClientID %>';
            var uxReportGrid_ClientID = '<%=uxReportGrid.ClientID %>';
            var hddActiveUserID_ClientID = '<%=hddActiveUserID.ClientID %>';
            var uxActiveDecactive_ClientID = '<%=uxActiveDecactive.ClientID %>';
            var ManageUsers_js_Deactive = '<%= GetLocalResourceObject("ManageUsers_js_Deactive").ToString() %>';
            var ManageUsers_js_Active = '<%= GetLocalResourceObject("ManageUsers_js_Active").ToString() %>';
            var hddSystemId_ClientID = '<%=hddSystemId.ClientID %>';
            var hddIsSecondaryUser_ClientID = '<%=hddIsSecondaryUser.ClientID %>';
        </script>
        <script type="text/javascript" src="<%= ResolveUrl("~/")%>res/js/usermaintenance/ManageUsers.js"></script>
    </tek:RadCodeBlock>
</asp:Content>
