<%@ Page Language="C#" MasterPageFile="~/MasterPagePopup.master" AutoEventWireup="true" CodeFile="rm_MCF_Filter_CampaignID_Modal.aspx.cs" Inherits="rm_MCF_Filter_CampaignID_Modal" Title="Select Risk Level" meta:resourcekey="PageResource1" %>

<%@ Register Src="~/UserControls/rm_MCF_FilterCampaignID.ascx" TagName="Risk_FilterCampaignID" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="Server">
    <as:ASModalContainer ID="uxModalContainer" runat="server" WidthCssClass="modal-md" ContainerCssClass="container" Width=""><div class="row">
            <div class="col-md-12 on-top">
                <as:RadioButtonList ID="uxRadioMode" runat="server" RepeatDirection="horizontal" CssClass="radio-button-list dark-blue"
                    AutoPostBack="true" OnSelectedIndexChanged="uxRadioMode_SelectedIndexChanged" meta:resourcekey="uxRadioModeResource1">
                    <asp:ListItem Text="Include" meta:resourcekey="ListItemResource1"></asp:ListItem>
                    <asp:ListItem Text="Exclude" meta:resourcekey="ListItemResource2"></asp:ListItem>
                </as:RadioButtonList>
            </div>
        </div>
        <div class="height-16"></div>
        <div class="row">
            <div class="col-md-12">
                <uc1:Risk_FilterCampaignID ID="uxFilterCampaignID" runat="server" WidthUC="600" HeightUC="500"/>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12 form-action-container text-right">
                <as:Button ID="uxClose" runat="server" Text="Close" OnClick="uxClose_Click" CssClass="btn btn-default" meta:resourcekey="uxCloseResource1" />
            </div>
        </div>
    </as:ASModalContainer>
    <as:ASRadCodeBlock ID="radCodeBlock" runat="server">
        <script type="text/javascript">
            parent.setModalID('CampaignIDModal');
        </script>
    </as:ASRadCodeBlock>
</asp:Content>

