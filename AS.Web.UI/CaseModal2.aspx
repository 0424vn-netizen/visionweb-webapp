<%@ Page Language="C#" MasterPageFile="~/MasterPagePopup.master" AutoEventWireup="true" CodeFile="CaseModal2.aspx.cs" Inherits="CaseModal2" Title="Case Management" meta:resourcekey="PageResource1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" Runat="Server">
    <div style="padding: 10px; width: 300px; height: 110px;">
        <as:Container ID="asContainer" runat="server" HeaderText="Case Management" Width="100%" FooterControlID="" FooterText="" HeaderControlID="" TemplateName="ascontainer_greyborder.tpl" meta:resourcekey="asContainerResource1">
            <div style="text-align: center;">
                <as:Literal ID="ltTicket" runat="server" Text="Are you sure you would like to close this ticket?" meta:resourcekey="ltTicketResource1"></as:Literal>
            </div>
        </as:Container>
        <div style="text-align: right; margin-top: 10px;">
            <as:Button ID="uxYes" runat="server" Text="Yes" OnClick="uxYes_Click" IsStandardButton="False" meta:resourcekey="uxYesResource1" />
            &nbsp;&nbsp;&nbsp;
            <as:Button ID="uxNo" runat="server" Text="No" OnClientClick="parent.HidePopupModal();" IsStandardButton="False" meta:resourcekey="uxNoResource1" />
        </div>
    </div>
</asp:Content>

