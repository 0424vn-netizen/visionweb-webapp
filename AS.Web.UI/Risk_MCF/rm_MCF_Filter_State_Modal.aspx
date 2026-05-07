<%@ Page Language="C#" MasterPageFile="~/MasterPagePopup.master" AutoEventWireup="true" CodeFile="rm_MCF_Filter_State_Modal.aspx.cs" Inherits="rm_MCF_Filter_State_Modal" Title="Select State, Territory, or Province" meta:resourcekey="PageResource1" %>

<%@ Register Src="~/UserControls/rm_MCF_FilterState.ascx" TagName="Risk_FilterState" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="Server">
    <as:ASModalContainer ID="uxModalContainer" runat="server" WidthCssClass="modal-md" ContainerCssClass="container" Width="">
        <div class="row">
            <div class="col-md-12">
                <uc1:Risk_FilterState ID="uxFilterState" runat="server"/>
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
            parent.setModalID('StateModal');
        </script>
    </as:ASRadCodeBlock>
</asp:Content>

