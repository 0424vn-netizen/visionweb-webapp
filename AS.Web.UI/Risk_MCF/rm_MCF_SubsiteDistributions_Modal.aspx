<%@ Page Language="C#" MasterPageFile="~/MasterPagePopup.master" AutoEventWireup="true" CodeFile="rm_MCF_SubsiteDistributions_Modal.aspx.cs" Inherits="As.VisionWeb.Web.SubsiteDistributionsModal" Title="Select User" meta:resourcekey="PageResource1" %>

<%@ Register Src="~/UserControls/rm_MCF_SubsiteDistributions.ascx" TagName="SubsiteDistributions" TagPrefix="uc" %>
    
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" Runat="Server">
    <as:ASModalContainer ID="uxModalContainer" runat="server" WidthCssClass="modal-md" ContainerCssClass="container" Width="">
          <uc:SubsiteDistributions ID="uxSubsiteDistributions" runat="server" WidthUC="770" HeightUC="500" />
        <div class="row">
            <div class="col-md-12 form-action-container text-right">
                <as:Button ID="uxClose" runat="server" Text="Close" OnClick="btnClose_Click" CssClass="btn btn-default" meta:resourcekey="uxCloseResource1" />
            </div>
        </div>
    </as:ASModalContainer>
    <as:ASRadCodeBlock ID="radCodeBlock" runat="server">
        <script type="text/javascript">
            parent.setModalID('AssignmentSubsiteDistributionsModal');
        </script>
    </as:ASRadCodeBlock>
</asp:Content>

