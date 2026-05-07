<%@ Page Language="C#" MasterPageFile="~/MasterPagePopup.master" AutoEventWireup="true"
    CodeFile="Message_HierarchyFilterModal.aspx.cs" Inherits="Message_HierarchyFilterModal" Title="Untitled Page" meta:resourcekey="PageResource1" %>

<%@ Register Src="~/UserControls/Message_HierarchyFilter.ascx" TagName="Message_HierarchyFilter" TagPrefix="uc" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="Server">
    <as:ASModalContainer ID="uxModalContainer" runat="server" WidthCssClass="modal-xl" ContainerCssClass="container" Width="">
        <uc:Message_HierarchyFilter ID="uxMessage_HierarchyFilter" runat="server" />
    </as:ASModalContainer>

    <as:ASRadCodeBlock ID="ASRadCodeBlock1" runat="server">
        <script type="text/javascript">
            parent.setModalID('MessageHierarchyModal');
            parent.setClientIDbtn('<%= ClientIDbtn %>');
        </script>
    </as:ASRadCodeBlock>

</asp:Content>
