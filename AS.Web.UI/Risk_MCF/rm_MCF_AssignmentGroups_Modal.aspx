<%@ Page Language="C#" MasterPageFile="~/MasterPagePopup.master" AutoEventWireup="true" CodeFile="rm_MCF_AssignmentGroups_Modal.aspx.cs" Inherits="rm_MCF_AssignmentGroups_Modal" Title="Select Group" meta:resourcekey="PageResource1" %>
<%@ Register Src="~/UserControls/rm_MCF_Assignment_Group.ascx" TagName="Group" TagPrefix="uc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" Runat="Server">
    <as:ASModalContainer ID="uxModalContainer" runat="server" WidthCssClass="modal-md" ContainerCssClass="container" Width="">
        <div class="row">
            <div class="col-md-12">
                <uc:Group ID="uxAssignmentGroup" runat="server" />
            </div>
        </div>
        <div class="row">
            <div class="col-md-12 form-action-container text-right">
                <asp:Button ID="btnSave" runat="server" Text="Close" OnClick="btnSave_Click" CssClass="btn btn-default" meta:resourcekey="btnSaveResource1" />
            </div>
        </div>
    </as:ASModalContainer>
    <as:ASRadCodeBlock ID="radCodeBlock" runat="server">
        <script type="text/javascript">
            parent.setModalID('AssignmentGroupModal');
        </script>
    </as:ASRadCodeBlock>
</asp:Content>

