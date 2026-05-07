<%@ Page Title="MEMBER LIST" Language="C#" MasterPageFile="~/MasterPagePopup.master" AutoEventWireup="true" CodeFile="rm_MCF_Group_MemberModal.aspx.cs" Inherits="rm_MCF_Group_MemberModal" meta:resourcekey="PageResource1" %>

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
    <as:ASModalContainer ID="uxModalContainer" runat="server" Width="800px" ContainerCssClass="container" WidthCssClass="modal-xxl">
        <div class="row">
            <div class="col-md-12">
                <h3 class="modal-title">
                    <asp:Literal runat="server" ID="header" Text="Member List" meta:resourcekey="headerResource1" />
                </h3>
                <uc:UxExport ID="uxExportTop" IsOnTop="true" runat="server" GridID="uxReportGrid" />
                <as:ASGrid ID="uxReportGrid" runat="server" GridLines="None" AllowPaging="True" IsAutoExportTemplate="true"
                    PageSize="10" AllowSorting="True" AutoGenerateColumns="False" AllowFilteringByColumn="false"
                    AllowSortFilterWhenExport="true" CssClass="in" meta:resourcekey="uxReportGridResource1">
                    <ClientSettings>
                        <Scrolling AllowScroll="false" UseStaticHeaders="false" />
                    </ClientSettings>
                    <MasterTableView>
                        <Columns>
                            <as:ASGridBoundColumn HeaderText="User ID" HeaderTooltip="User ID" DataField="UserID"
                                UniqueName="UserID" ASFormat="DynamicString" SortExpression="UserID" meta:resourcekey="ASGridBoundColumnResource1">
                                <ColumnValidationSettings>
                                    <ModelErrorMessage Text=""></ModelErrorMessage>
                                </ColumnValidationSettings>

                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            </as:ASGridBoundColumn>
                            <as:ASGridBoundColumn HeaderText="User Full Name" HeaderTooltip="User Full Name"
                                DataField="UserName" ASFormat="DynamicString" UniqueName="UserName"
                                SortExpression="UserName" meta:resourcekey="ASGridBoundColumnResource2">
                                <ColumnValidationSettings>
                                    <ModelErrorMessage Text=""></ModelErrorMessage>
                                </ColumnValidationSettings>

                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            </as:ASGridBoundColumn>
                        </Columns>
                    </MasterTableView>
                </as:ASGrid>
            </div>
        </div>
    </as:ASModalContainer>
</asp:Content>

