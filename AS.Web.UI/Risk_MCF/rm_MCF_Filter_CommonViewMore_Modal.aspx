<%@ Page Language="C#" MasterPageFile="~/MasterPagePopup.master" AutoEventWireup="true" CodeFile="rm_MCF_Filter_CommonViewMore_Modal.aspx.cs" Inherits="rm_MCF_Filter_CommonViewMore_Modal" meta:resourcekey="PageResource1" %>

<%@ Register Src="~/UserControls/rm_MCF_FilterHierarchy.ascx" TagName="Hierarchy" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="Server">
    <as:ASModalContainer ID="uxModalContainer" runat="server" WidthCssClass="modal-sm" ContainerCssClass="container" Width="">
        <as:Panel ID="pnTitle" runat="server" Visible="false">
            <div class="row">
                <div class="col-md-12">
                    <div class="modal-title">
                        <h3><as:Literal ID="Literal1" runat="server" Text="Type:"></as:Literal>
                            <as:Literal ID="ltTitle" runat="server"></as:Literal>
                        </h3>
                    </div>
                </div>
            </div>
        </as:Panel>
        <div class="row">
            <div class="col-md-12">
                <asp:ListBox runat="server" ID="uxContent" Height="300px" SelectionMode="Multiple" CssClass="ASListBox" meta:resourcekey="uxContentResource1" />
            </div>

        </div>
        <div class="row">
            <div class="col-md-12 action-container text-right">
                <as:Button runat="server" ID="uxClose" Text="Close" OnClientClick="return parent.ClosePopupModal(1);" CssClass="btn btn-default" meta:resourcekey="uxCloseResource1" />
            </div>
        </div>
    </as:ASModalContainer>
    <as:ASRadCodeBlock ID="radCodeBlock" runat="server">
        <script type="text/javascript">

        </script>
    </as:ASRadCodeBlock>
</asp:Content>
