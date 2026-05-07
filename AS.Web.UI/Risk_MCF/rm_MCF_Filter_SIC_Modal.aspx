<%@ Page Language="C#" MasterPageFile="~/MasterPagePopup.master" AutoEventWireup="true" CodeFile="rm_MCF_Filter_SIC_Modal.aspx.cs" Inherits="rm_MCF_Filter_SIC_Modal" Title="Select SIC" meta:resourcekey="PageResource1" %>

<%@ Register Src="~/UserControls/rm_MCF_FilterSIC.ascx" TagName="Risk_SICCode" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="Server">
    <as:ASModalContainer ID="uxModalContainer" runat="server" WidthCssClass="modal-xl" ContainerCssClass="container" Width="">
        <div class="row">
            <div class="col-md-12">
                <uc1:Risk_SICCode ID="uxRisk_SIC" runat="server" />
            </div>
        </div>
    </as:ASModalContainer>
    <as:ASRadCodeBlock ID="radCodeBlock" runat="server">
        <script type="text/javascript">
            parent.setModalID('SICModal');
        </script>
    </as:ASRadCodeBlock>
</asp:Content>

