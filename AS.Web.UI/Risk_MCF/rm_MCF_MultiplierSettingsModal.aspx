<%@ Page Title="Multiplier Settings" Language="C#" MasterPageFile="~/MasterPagePopup.master" AutoEventWireup="true" CodeFile="rm_MCF_MultiplierSettingsModal.aspx.cs" Inherits="rm_MCF_MultiplierSettingsModal" meta:resourcekey="PageResource1" Culture="auto" UICulture="auto" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="Server">
    <as:ASModalContainer ID="uxModalContainer" runat="server" WidthCssClass="modal-lg" ContainerCssClass="container" Width="800px">
        <div class="row">
            <div class="col-md-12">
                <h3 class="modal-title">
                    <asp:Literal ID="litTitle" runat="server" meta:resourcekey="uxTitle"></asp:Literal>
                </h3>
                <div class="height-12"></div>

                <span>
                    <asp:Literal ID="litSubTitle" runat="server" meta:resourcekey="uxSubtitle"></asp:Literal>
                </span>

            </div>
        </div>    
        <div class="height-6"> </div>    
        <div class="row">            
            <div class="col-md-12">
                <as:RadioButtonList ID="uxRadioMode" runat="server" CssClass="radio-button-list dark-blue" meta:resourcekey="uxRadioModeResource1">
                    <asp:ListItem Text="Highest" meta:resourcekey="ListItemResource1"></asp:ListItem>
                    <asp:ListItem Text="Lowest" meta:resourcekey="ListItemResource2"></asp:ListItem>
                </as:RadioButtonList>
            </div>
        </div>
        <div class="height-4"></div>
        <div class="row">
            <div class="col-md-12 text-right form-action-container">
                <as:Button ID="uxSubmit" runat="server" Text="Submit" OnClick="uxSubmit_Click" class="btn btn-default" meta:resourcekey="uxSubmitResource" UseSubmitBehavior="false"/>
                <asp:Button ID="uxCancel" OnClientClick="parent.HidePopupModal(); return false;" Text="Close" class="btn btn-default" runat="server" meta:resourcekey="uxCancelResource"></asp:Button>
            </div>
        </div>

    </as:ASModalContainer>
</asp:Content>
