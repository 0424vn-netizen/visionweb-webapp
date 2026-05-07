<%@ Page Language="C#" MasterPageFile="~/MasterPagePopup.master" AutoEventWireup="true" CodeFile="rm_MCF_Filter_HRCode_Modal.aspx.cs" Inherits="rm_MCF_Filter_HRCode_Modal" meta:resourcekey="PageResource1" Title="Select HR Code" %>

<%@ Register Src="~/UserControls/rm_MCF_Filter_HRCode_Modal.ascx" TagName="HR_CODE" TagPrefix="uc1" %>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPage" runat="Server">
    <as:ASModalContainer ID="uxModalContainer" runat="server" WidthCssClass="modal-lg" ContainerCssClass="container" Width="">
        <div class="row">
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
                <uc1:HR_CODE ID="uxHRCode" runat="server" WidthUC="900" HeightUC="500" />
            </div>
        </div>
        <div class="row">
            <div class="col-md-12 form-action-container text-right">
                <as:Button ID="btnClose" runat="server" Text="Close" OnClick="btnClose_Click" CssClass="btn btn-default" meta:resourcekey="btnCloseResource1" />
            </div>
        </div>
    </as:ASModalContainer>
    <as:ASRadCodeBlock ID="radCodeBlock" runat="server">
        <script type="text/javascript">
            parent.setModalID("HRCodeModal");
        </script>
    </as:ASRadCodeBlock>
</asp:Content>


