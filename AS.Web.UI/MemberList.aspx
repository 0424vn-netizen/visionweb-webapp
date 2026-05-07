<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPagePopup.master" CodeFile="MemberList.aspx.cs" Inherits="MemberList" meta:resourcekey="PageResource1" %>

<%@ Register Src="~/UserControls/UxExport.ascx" TagName="UxExport" TagPrefix="uc" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="Server">
    <tek:RadAjaxManagerProxy ID="RadAjaxManager1" runat="server">
        <AjaxSettings>
            <tek:AjaxSetting AjaxControlID="uxMemberGrid">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="uxMemberGrid" />
                </UpdatedControls>
            </tek:AjaxSetting>
        </AjaxSettings>
    </tek:RadAjaxManagerProxy>
    <as:ASModalContainer ID="uxModalContainer" runat="server" WidthCssClass="modal-lg" ContainerCssClass="container">
        <uc:UxExport ID="uxExportTop" GridID="uxMemberGrid" runat="server" IsOnTop="true" />
        <as:ASGrid ID="uxMemberGrid" runat="server" AllowPaging="True" AllowSorting="True" OnNeedDataSource="uxMemberGrid_NeedDataSource"
            AutoGenerateColumns="False" AllowAutoCalculateTotalOnExport="false" AllowFilteringByColumn="false"
            ASPagingMethod="SPASingleMethod" ShowPageTotal="false" ShowReportTotal="false" IsAutoExportTemplate="true"
            AllowSortFilterWhenExport="true" CssClass="in" meta:resourcekey="uxReportGridResource1">
            <MasterTableView>
                <Columns>
                    <as:ASGridBoundColumn UniqueName="UserLoginName" HeaderText="User ID" HeaderTooltip="User ID"
                        DataField="UserLoginName" SortExpression="UserLoginName" ASFormat="StaticString" HeaderStyle-Width="130px" meta:resourcekey="UserLoginNameResource1">
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn UniqueName="UserNameFull" HeaderText="User Full Name" HeaderTooltip="User Full Name" HeaderStyle-Width="130px" meta:resourcekey="UserNameFullResource1"
                        DataField="UserNameFull" ASFormat="StaticString">
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                </Columns>
            </MasterTableView>
        </as:ASGrid>
        <div class="row">
            <div class="col-xs-12 form-action-container text-right">
                <as:Button class="btn btn-default" ID="btnClose" Text="Back" OnClientClick="ClosePopupModal();" runat="server" meta:resourcekey="uxBackResource" />
            </div>
        </div>
    </as:ASModalContainer>
</asp:Content>
