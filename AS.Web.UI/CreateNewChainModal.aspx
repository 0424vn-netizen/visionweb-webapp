<%@ Page Title="Create New Custom Chain" Language="C#" MasterPageFile="~/MasterPagePopup.master" AutoEventWireup="true" 
    CodeFile="CreateNewChainModal.aspx.cs" Inherits="CreateNewChainModal" meta:resourcekey="PageResource1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" Runat="Server">
    <as:ASModalContainer ID="ASModalContainer1" runat="server" Width="500px" ContainerCssClass="container" WidthCssClass="modal-xxl">
        <div class="row">
            <div class="col-md-12">
                <div class="box">
                    <%= String.Format(GetLocalResourceObject("CreateNewChainModal_aspx_AddMerchantToNewChain").ToString(),MerchantNumber) %>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12 text-right form-action-container">
                <as:Button ID="uxSubmit" runat="server" Text="Submit" OnClick="CreateChain_Click" CssClass="btn btn-default" meta:resourcekey="uxSubmitResource1" />
            </div>
        </div>
    </as:ASModalContainer>
    <as:RadCodeBlock ID="uxRadCodeBlock" runat="server">
        <script type="text/javascript" src="<%=ResolveUrl("~") %>res/js/CreateNewChainModal.js">
        </script>
    </as:RadCodeBlock>
</asp:Content>

