<%@ Page Title="Group Maintenance" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="UserGroupMaintenance.aspx.cs" Inherits="UserGroupMaintenance" meta:resourcekey="PageResource1" %>

<%@ Register Src="~/UserControls/PageTitle.ascx" TagName="ReportTitle" TagPrefix="uc" %>
<%@ Register TagName="UxExport" Src="~/UserControls/UxExport.ascx" TagPrefix="uc" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="Server">

    <as:PlaceHolder ID="sdfd" runat="server">
        <div class="row">
            <div class="col-md-12 no-margin-bottom">
                <uc:ReportTitle ID="uxReportTitle" ReportTitle="User Group Maintenance" HasFilteringOption="false" meta:resourcekey="uxReportTitleResource1"
                    runat="server" />
            </div>
        </div>
        <div class="height-15"></div>

        <as:Panel ID="uxFilterStatusContainer" runat="server" meta:resourcekey="uxFilterStatusContainerResource1">
            <div class="row">
                <div class="col-md-12 dark-blue">
                    <div class="control-inline">
                        <label class="first">
                            <asp:Literal ID="Literal1" runat="server" meta:resourcekey="Literal1Resource1" Text=" Status:"></asp:Literal></label>
                    </div>
                    <as:RadioButton ID="uxFilterStatusAll" CssClass="control-inline" runat="server" Text="All"
                        GroupName="uxFilterStatus"
                        xValue="0" onclick="uxFilterStatus_Checked()" meta:resourcekey="uxFilterStatusAllResource1" Value="" />
                    <as:RadioButton ID="uxFilterStatusActive" runat="server" CssClass="control-inline"
                        Text="Active" GroupName="uxFilterStatus"
                        xValue="1" Checked="True" onclick="uxFilterStatus_Checked()" meta:resourcekey="uxFilterStatusActiveResource1" Value="" />
                    <as:RadioButton ID="uxFilterStatusInactive" runat="server" CssClass="control-inline"
                        Text="Inactive" GroupName="uxFilterStatus"
                        xValue="2" onclick="uxFilterStatus_Checked()" meta:resourcekey="uxFilterStatusInactiveResource1" Value="" />
                    <asp:Button ID="uxChangeFilterStatus" runat="server" OnClick="uxChangeFilterStatus_Click"
                        Style="display: none" meta:resourcekey="uxChangeFilterStatusResource1"></asp:Button>
                </div>
            </div>
        </as:Panel>
        <div class="height-15"></div>
        <div class="maintance-link">
            <div id="pnlCreateLink" runat="server">
                <div class="row link-container">
                    <div class="col-xs-12">
                        <a id="btnCreateUserGroup" class="btn btn-default" href="#">
                            <as:Literal ID="ltDocument" runat="server" Text="Create New User Group" meta:resourcekey="ltDocumentResource1"></as:Literal>
                        </a>
                    </div>
                </div>
            </div>
        </div>
        <uc:UxExport ID="uxExportTop" IsOnTop="true" runat="server" GridID="uxUserGroupList"
            GridHeader="User Group Maintenance" meta:resourcekey="uxExportTopResource1" />
        <div class="row">
            <div class="col-md-12">
                <as:ASGrid ID="uxUserGroupList" runat="server" GridLines="None" AllowPaging="True" IsAutoExportTemplate="true"
                    AllowAutomaticUpdates="True" AllowSorting="True" AutoGenerateColumns="False" CssClass="grid-scroll in"
                    AllowFilteringByColumn="false" InsertTempColumnAtTheEnd="false" ASPagingMethod="SPASingleMethod1"
                    AllowSortFilterWhenExport="true" meta:resourcekey="uxGroupListResource1">
                    <MasterTableView DataKeyNames="UserGroupID" CommandItemDisplay="None">
                        <Columns>
                            <%--Group column --%>
                            <as:ASGridTemplateColumn HeaderText="User Group" HeaderTooltip="User Group" HeaderStyle-HorizontalAlign="Center"
                                ItemStyle-HorizontalAlign="Left" UniqueName="UserGroupName" AllowEncodeWhenExport="true" SortExpression="UserGroupName"
                                DataField="UserGroupName" meta:resourcekey="ASGridTemplateColumnResource1">
                                <ItemTemplate>
                                    <asp:Literal ID="LiteralGroupName" Text='<%# VeraCodeExtensions.DoVeraCode(System.Web.HttpUtility.HtmlEncode(Eval("UserGroupName").ToString())) %>'
                                        runat="server" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:HiddenField ID="txtGroupID" runat="server" Value='<%# Eval("UserGroupID").ToString() %>' />
                                    <asp:TextBox ID="txtEditGroupName" runat="server" MaxLength="50" Text='<%# Eval("UserGroupName").ToString() %>'
                                        CssClass="txtEditGroupName max-width" meta:resourcekey="txtEditGroupNameResource1" />
                                </EditItemTemplate>

                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                                <ItemStyle HorizontalAlign="Left"></ItemStyle>
                            </as:ASGridTemplateColumn>

                            <%--Description column --%>
                            <as:ASGridTemplateColumn HeaderText="Description" HeaderTooltip="Description" HeaderStyle-HorizontalAlign="Center"
                                ItemStyle-HorizontalAlign="Left" UniqueName="Description" SortExpression="Description"
                                DataField="Description" meta:resourcekey="ASGridTemplateColumnResource2">
                                <ItemTemplate>
                                    <asp:Literal ID="LiteralDescription" Text='<%# VeraCodeExtensions.DoVeraCode(System.Web.HttpUtility.HtmlEncode(Eval("Description").ToString())) %>'
                                        runat="server" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:HiddenField ID="txtActiveValue" runat="server" Value='<%# Eval("IsActive").ToString() %>' />
                                    <asp:TextBox ID="txtEditGroupDescription" runat="server" MaxLength="500" Text='<%# Eval("Description").ToString() %>'
                                        CssClass="txtEditGroupDescription max-width" meta:resourcekey="txtEditGroupDescriptionResource1" />
                                </EditItemTemplate>

                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                                <ItemStyle HorizontalAlign="Left"></ItemStyle>
                            </as:ASGridTemplateColumn>

                            <%--#Members column--%>
                            <as:ASGridBoundColumn HeaderText="# Members" HeaderTooltip="# users who belong to the group"
                                HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right" HeaderStyle-Wrap="false"
                                DataField="Members" UniqueName="Members" ASFormat="Integer" ReadOnly="true" meta:resourcekey="ASGridBoundColumnResource1">
                                <ColumnValidationSettings>
                                    <ModelErrorMessage Text=""></ModelErrorMessage>
                                </ColumnValidationSettings>

                                <HeaderStyle HorizontalAlign="Center" Wrap="False"></HeaderStyle>

                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                            </as:ASGridBoundColumn>

                            <%--Active column--%>
                            <as:ASGridBoundColumn HeaderText="Active" HeaderStyle-HorizontalAlign="Center"
                                ItemStyle-HorizontalAlign="Center" DataField="ActiveText" UniqueName="ActiveText"
                                meta:resourcekey="ASGridBoundColumnResource2">
                                <ColumnValidationSettings>
                                    <ModelErrorMessage Text=""></ModelErrorMessage>
                                </ColumnValidationSettings>

                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            </as:ASGridBoundColumn>

                            <as:ASGridBoundColumn HeaderText="Edit" AllowSorting="false"
                                HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Wrap="false"
                                DataField="Edit" UniqueName="Edit" meta:resourcekey="ASGridTemplateColumnResource3">
                                <ColumnValidationSettings>
                                    <ModelErrorMessage Text=""></ModelErrorMessage>
                                </ColumnValidationSettings>

                                <HeaderStyle HorizontalAlign="Center" Wrap="False"></HeaderStyle>

                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            </as:ASGridBoundColumn>
                        </Columns>
                    </MasterTableView>
                </as:ASGrid>
            </div>
        </div>

    </as:PlaceHolder>
    <as:RadAjaxManagerProxy ID="RadAjaxManager" runat="server">
        <AjaxSettings>
            <tek:AjaxSetting AjaxControlID="uxUserGroupList">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="uxUserGroupList" />
                </UpdatedControls>
            </tek:AjaxSetting>
        </AjaxSettings>
    </as:RadAjaxManagerProxy>
    <tek:RadCodeBlock runat="server" ID="RadCodeBlock">
        <script type="text/javascript">
            var uxChangeFilterStatus_ClientID = '<%= uxChangeFilterStatus.ClientID %>';
        </script>
        <script type="text/javascript" src="<%= ResolveUrl("~/") %>res/js/datashare/userGroupMaintenance.js"></script>
    </tek:RadCodeBlock>
</asp:Content>

