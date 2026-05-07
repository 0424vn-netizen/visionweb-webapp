<%@ Page Language="C#" MasterPageFile="~/MasterPagePopup.master" AutoEventWireup="true"
    CodeFile="Message_HierarchyFilter_VMModal.aspx.cs" Inherits="Message_HierarchyFilter_VMModal"
    Title="Untitled Page" meta:resourcekey="PageResource1" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="Server">
    <as:ASModalContainer ID="ASModalContainer1" runat="server" WidthCssClass="modal-xl" ContainerCssClass="container" Width="">
        <h2 class="grid-title on-top">
            <span class="text-muted no-margin">
                <as:Literal ID="ltType" runat="server" Text="Type:" meta:resourcekey="ltTypeResource1"></as:Literal>
                <as:Literal ID="uxInclude" runat="server" meta:resourcekey="uxIncludeResource1"></as:Literal>
            </span>
        </h2>
        <as:ASGrid ID="uxGrid" runat="server" AutoGenerateColumns="false" AllowPaging="true"
            ASPagingMethod="SPASingleMethod" AllowSorting="true" AllowFilteringByColumn="true"
            OnNeedDataSource="uxGrid_NeedDataSource" OnInit="uxGrid_Init" CssClass="in" meta:resourcekey="uxGridResource1">
            <MasterTableView>
                <Columns>
                    <as:ASGridBoundColumn DataField="DataKey" UniqueName="DataKey" HeaderText="" ASFormat="StaticString"
                        ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" meta:resourcekey="ASGridBoundColumnResource1">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="DataText" UniqueName="DataText" HeaderText="" Visible="false"
                        ASFormat="DynamicString" meta:resourcekey="ASGridBoundColumnResource2">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                </Columns>
            </MasterTableView>
        </as:ASGrid>
        <div class="row">
            <div class="col-md-12 form-action-container text-right">
                <as:Button runat="server" Text="Close" ID="uxBtnClose" OnClientClick="parent.ClosePopupModal(1); return false;"
                    CssClass="btn btn-default" meta:resourcekey="uxBtnCloseResource1" />
            </div>
        </div>
    </as:ASModalContainer>
    <as:RadAjaxManagerProxy runat="server" ID="uxRadAjaxManager">
        <AjaxSettings>
            <tek:AjaxSetting AjaxControlID="uxGrid">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="uxGrid" />
                </UpdatedControls>
            </tek:AjaxSetting>
        </AjaxSettings>
    </as:RadAjaxManagerProxy>
</asp:Content>
