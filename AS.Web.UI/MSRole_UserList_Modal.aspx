<%@ Page Language="C#" MasterPageFile="~/MasterPagePopup.master" AutoEventWireup="true" CodeFile="MSRole_UserList_Modal.aspx.cs" Inherits="MSRole_UserList_Modal" Title="List of Users Assigned to this Role" meta:resourcekey="PageResource1" %>

<%@ Register TagName="UxExport" Src="~/UserControls/UxExport.ascx" TagPrefix="uc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="Server">
    <tek:RadAjaxManagerProxy ID="RadAjaxManager" runat="server">
        <AjaxSettings>
            <tek:AjaxSetting AjaxControlID="uxReportGrid">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="uxReportGrid" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </tek:AjaxSetting>
        </AjaxSettings>
    </tek:RadAjaxManagerProxy>
    <tek:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" BackgroundPosition="Top">
    </tek:RadAjaxLoadingPanel>
    <as:ASModalContainer ID="ASModalContainer1" runat="server" Width="790px" ContainerCssClass="container" WidthCssClass="modal-xxl">
        <table>
            <tr>
                <td>
                    <uc:UxExport ID="uxExportTop" runat="server" GridID="uxReportGrid" />
                    <as:ASGrid ID="uxReportGrid" runat="server" GridLines="None" AllowPaging="True"
                        PageSize="10" AllowSorting="True" AutoGenerateColumns="False" AllowFilteringByColumn="false"
                        AllowSortFilterWhenExport="true" Width="700px" CssClass="in" meta:resourcekey="uxReportGridResource1">
                        <ClientSettings>
                            <Scrolling AllowScroll="false" UseStaticHeaders="false" />
                        </ClientSettings>
                        <MasterTableView Width="100%">
                            <Columns>
                                <as:ASGridBoundColumn HeaderText="User ID" HeaderTooltip="User ID" DataField="UserID"
                                    UniqueName="UserID" ASFormat="DynamicString" SortExpression="UserID" meta:resourcekey="ASGridBoundColumnResource1">
                                    <ColumnValidationSettings>
                                        <ModelErrorMessage Text=""></ModelErrorMessage>
                                    </ColumnValidationSettings>

                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                </as:ASGridBoundColumn>
                                <as:ASGridBoundColumn HeaderText="First Name" HeaderTooltip="First Name"
                                    DataField="UserNameFirst" ASFormat="DynamicString" UniqueName="FirstName"
                                    SortExpression="UserNameFirst" meta:resourcekey="ASGridBoundColumnResource2">
                                    <ColumnValidationSettings>
                                        <ModelErrorMessage Text=""></ModelErrorMessage>
                                    </ColumnValidationSettings>

                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                </as:ASGridBoundColumn>
                                <as:ASGridBoundColumn HeaderText="Last Name" HeaderTooltip="Last Name"
                                    DataField="UserNameLast" ASFormat="DynamicString" UniqueName="LastName"
                                    SortExpression="UserNameLast" meta:resourcekey="ASGridBoundColumnResource3">
                                    <ColumnValidationSettings>
                                        <ModelErrorMessage Text=""></ModelErrorMessage>
                                    </ColumnValidationSettings>

                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                </as:ASGridBoundColumn>
                            </Columns>
                        </MasterTableView>
                    </as:ASGrid>
                    <%--<uc:UxExport ID="uxExportBottom" runat="server" GridID="uxReportGrid" IsBottom="true" />--%>
                </td>
            </tr>
        </table>
    </as:ASModalContainer>
    <as:ASRadCodeBlock ID="ASRadCodeBlock1" runat="server">
        <script src="<%=ResolveUrl("~")%>res/js/usermaintenance/MSRole_UserList_Modal.js"></script>
    </as:ASRadCodeBlock>
</asp:Content>

