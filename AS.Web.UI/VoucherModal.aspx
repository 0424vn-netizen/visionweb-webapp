<%@ Page Language="C#" MasterPageFile="~/MasterPagePopup.master" AutoEventWireup="true" CodeFile="VoucherModal.aspx.cs" Inherits="VoucharModal"
    Title="VOUCHER" meta:resourcekey="PageResource1" %>

<%@ Register TagName="PageTitle" Src="~/UserControls/PageTitle.ascx" TagPrefix="uc" %>

<asp:Content ID="uxContent" ContentPlaceHolderID="ContentPage" runat="Server">
    <as:ASModalContainer ID="uxModalContainer" runat="server" WidthCssClass="modal-sm" ContainerCssClass="container" Width="">
        <div class="row">
            <div class="col-md-12">
                <div class="box">
                    <p>
                        <strong>
                            <asp:Literal ID="lblMerchantName" runat="server" meta:resourcekey="lblMerchantNameResource1"></asp:Literal><br />
                            <asp:Literal ID="lblAddress" runat="server" meta:resourcekey="lblAddressResource1"></asp:Literal><br />
                            <asp:Literal ID="lblCity" runat="server" meta:resourcekey="lblCityResource1"></asp:Literal>
                        </strong>
                    </p>
                    <asp:Label ID="lblInfo" runat="server" meta:resourcekey="lblInfoResource1"></asp:Label>
                    <br />
                    <%= GetLocalResourceObject("VoucherModal_aspx_Notice").ToString() %>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12 form-action-container text-right">
                <as:Button ID="uxClose" runat="server" Text="Close" class="btn btn-default " OnClientClick="parent.HidePopupModalChild(1)" meta:resourcekey="uxCloseResource1" />
            </div>
        </div>
    </as:ASModalContainer>
</asp:Content>

