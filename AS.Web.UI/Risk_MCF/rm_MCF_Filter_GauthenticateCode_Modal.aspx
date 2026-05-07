<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPagePopup.master" CodeFile="rm_MCF_Filter_GauthenticateCode_Modal.aspx.cs" Inherits="rm_MCF_Filter_GauthenticateCode_Modal" Title="Select Gauthenticate Code" meta:resourcekey="PageResource1" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="Server">
    <as:ASModalContainer ID="uxModalContainer" runat="server" WidthCssClass="modal-md" ContainerCssClass="container" Width="">
        <div class="row">
            <div class="col-md-12 on-top">
                <as:RadioButtonList ID="uxRadioMode" runat="server" RepeatDirection="horizontal" CssClass="radio-button-list dark-blue"
                    AutoPostBack="true" OnSelectedIndexChanged="uxRadioMode_SelectedIndexChanged" meta:resourcekey="uxRadioModeResource1">
                    <asp:ListItem Text="Include" Selected="True" meta:resourcekey="ListItemResource1"></asp:ListItem>
                    <asp:ListItem Text="Exclude" meta:resourcekey="ListItemResource2"></asp:ListItem>
                </as:RadioButtonList>
            </div>
        </div>
        <div class="height-16"></div>
        <div class="row">
            <div class="col-md-12">
                <as:MultiSelector ID="uxGauthenticateCode" runat="server" DataTextField="DataText" XmlFilterItemsFilePath="~/App_Data/AssigmentFilters.xml" DataValueField="DataKey"
                    AddText=">" RemoveText="<" WidthButton="26" WidthSelector="252" HeightSelector="320"
                    HideAddAll="true" HideRemoveAll="true"
                    ShowTooltip="false" CheckExisted="true"
                    OnMovedData="MultiSelector_OnMovedData"
                    CSSAddAllButton="btn-to-right-all" CSSRemoveAllButton="btn-to-left-all"
                    CSSAddButton="btn-to-right" CSSRemoveButton="btn-to-left" EnableEmbeddedSkins="False" meta:resourcekey="uxOwnerLastNameResource1" SortExpression="" />
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
            parent.setModalID("GauthenticateCodeModal");
        </script>
    </as:ASRadCodeBlock>
</asp:Content>
