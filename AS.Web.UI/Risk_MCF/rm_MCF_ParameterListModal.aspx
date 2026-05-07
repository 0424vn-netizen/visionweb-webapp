<%@ Page Language="C#" MasterPageFile="~/MasterPagePopup.master" AutoEventWireup="true" CodeFile="rm_MCF_ParameterListModal.aspx.cs" Inherits="rm_MCF_ParameterListModal" Title="Add Parameter" meta:resourcekey="PageResource1" %>

<%@ Register Src="~/UserControls/rm_MCF_FilterParameter.ascx" TagName="Parameter"
    TagPrefix="uc" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="Server">
    <as:PlaceHolder ID="dfgd" runat="server">
      <as:ASModalContainer ID="uxModalContainer" ClientIDMode="Static" runat="server" WidthCssClass="modal-lg">

        <uc:Parameter ID="ucParameterList" Mode="Assignment" runat="server" /> 
        <div id="fixedHeight"></div>
    </as:ASModalContainer>
    
    <div id="fixedPanel" class="footer-modal">       
        <div class="shadow-box"></div>
        <div class="form-action-container text-right content-footer">
            <span id="pnStatus">        
                <as:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click" CssClass="btn btn-default" meta:resourcekey="btnSubmitResource1" />
            </span>
        </div>
        <div id="fixedFooter"></div>
    </div>
        </as:PlaceHolder>
    <as:ASRadCodeBlock ID="radCodeBlock" runat="server">
        <script type="text/javascript">
        </script>
        <script src="<%= ResolveUrl("~/")%>res/js/risk_MCF/rm_MCF_ParameterListModal.js"></script>
    </as:ASRadCodeBlock>
</asp:Content>

