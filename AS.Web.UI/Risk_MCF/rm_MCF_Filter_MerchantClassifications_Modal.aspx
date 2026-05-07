<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPagePopup.master" AutoEventWireup="true" meta:resourcekey="PageResource1" CodeFile="rm_MCF_Filter_MerchantClassifications_Modal.aspx.cs" Inherits="rm_MCF_Filter_MerchantClassifications_Modal" %>

<%@ Register Src="~/UserControls/rm_MCF_Filter_MerchantClassifications_Modal.ascx" TagName="MerchantClassifications" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="Server">
    <as:ASModalContainer ID="uxModalContainer" runat="server" WidthCssClass="modal-md">
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
                <uc:MerchantClassifications id="uxMerchantClassifications" runat="server" />
            </div>
        </div>
        <div class="row">
            <div class="col-md-12 form-action-container text-right">
                <as:Button ID="uxClose" runat="server" Text="Close" OnClick="btnClose_Click" CssClass="btn btn-default" meta:resourcekey="uxCloseResource1" />
            </div>
        </div>
    </as:ASModalContainer>
    <as:ASRadCodeBlock ID="radCodeBlock" runat="server">
        <script type="text/javascript">
            var isInEx = '<%= IsIncludeExcludeItem %>';
            if (isInEx.toLowerCase() == 'false') {
                parent.setModalID('MerchantClassificationsModal');
                parent.setClientIDbtn('<%= ClientIDbtn %>');
            }
        </script>
    </as:ASRadCodeBlock>
</asp:Content>

