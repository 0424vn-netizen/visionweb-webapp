<%@ Page Language="C#" MasterPageFile="~/MasterPagePopup.master" AutoEventWireup="true" CodeFile="rm_MCF_ParameterFilter_TransactionCodeModal.aspx.cs" Inherits="rm_MCF_ParameterFilter_TransactionCodeModal" meta:resourcekey="PageResource1"  %>
<%@ Register src="~/UserControls/rm_MCF_ParameterFilter_TransactionCode.ascx" tagname="Hierarchy" tagprefix="uc" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" Runat="Server">

     <as:ASModalContainer ID="uxModalContainer" runat="server" WidthCssClass="modal-md" ContainerCssClass="container" Width="">
        <div class="row">
            <div class="col-md-12">
                <uc:Hierarchy ID="uxHierarchy" runat="server" WidthUC="770" HeightUC="500" /> 
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
            parent.setModalID('ParameterFilter_TransactionCodeModal');

            function CloseModal() {
                parent.HidePopupModal();
                parent.ReloadParamList();
            }
        </script>
    </as:ASRadCodeBlock>
</asp:Content>

