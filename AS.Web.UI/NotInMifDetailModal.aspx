<%@ Page Language="C#" AutoEventWireup="true" CodeFile="NotInMifDetailModal.aspx.cs" MasterPageFile="~/MasterPagePopup.master" Title="Not In Mif Detail" Inherits="NotInMifDetailModal" meta:resourcekey="PageResource1" %>

<%@ Register TagName="NotInMif_Auth" Src="~/UserControls/NotInMifDetail_AUTH.ascx" TagPrefix="uc" %>
<%@ Register TagName="NotInMif_Batch" Src="~/UserControls/NotInMifDetail_BATCH.ascx" TagPrefix="uc" %>
<%@ Register TagName="NotInMif_Chargeback" Src="~/UserControls/NotInMifDetail_CHARGEBACK.ascx" TagPrefix="uc" %>
<%@ Register TagName="NotInMif_Retrieval" Src="~/UserControls/NotInMifDetail_RETRIEVAL.ascx" TagPrefix="uc" %>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPage" runat="Server">
    <tek:RadAjaxManagerProxy runat="server" ID="uxRadAjaxManager">
        <AjaxSettings>
            <tek:AjaxSetting AjaxControlID="uxNotInMifDetailGrid">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="uxNotInMifDetailGrid" />
                </UpdatedControls>
            </tek:AjaxSetting>
        </AjaxSettings>
    </tek:RadAjaxManagerProxy>
    <as:ASModalContainer ID="ASModalContainer1" runat="server" WidthCssClass="modal-xxl">
        <div class="row">
            <div class="col-xs-12">
                <uc:NotInMif_Auth ID="uxNotInMifAuth" runat="server" />
                <uc:NotInMif_Batch ID="uxNotInMifBatch" runat="server" />
                <uc:NotInMif_Chargeback ID="uxNotInMifChargeback" runat="server" />
                <uc:NotInMif_Retrieval ID="uxNotInMifRetrieval" runat="server" />
            </div>
        </div>

        <div class="row">
            <div class="col-xs-12 form-action-container text-right">
                <as:Button class="btn btn-default" ID="Button1" Text="Close" OnClientClick="ClosePopupModal();" runat="server" meta:resourcekey="bntCancelResource" />
            </div>
        </div>
    </as:ASModalContainer>
</asp:Content>
