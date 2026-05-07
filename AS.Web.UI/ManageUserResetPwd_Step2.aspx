<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ManageUserResetPwd_Step2.aspx.cs"
    MasterPageFile="~/MasterPagePopup.master" Inherits="_mps_ResetPasswordForCS2"
    Title="Reset Password" meta:resourcekey="PageResource1" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="Server">
    <as:ASModalContainer ID="uxModalContainer" runat="server" WidthCssClass="modal-sm" ContainerCssClass="container" Width="">
        <div class="row">
            <div class="col-md-12 ASModal">
                <div class="box text-center">
                    <p><%= string.Format(GetLocalResourceObject("ManageUserResetPwd_Step2_aspx_PasswordTemporary").ToString(),timeToExpired) %></p>
                    <div class="tmp-password">
                        <as:Literal ID="uxPassword" runat="server" meta:resourcekey="uxPasswordResource1"></as:Literal>
                    </div>
                    <p class="text-muted"><as:Literal ID="ltNote" runat="server" Text="NOTE: Passwords are Case Sensitive." meta:resourcekey="ltNoteResource1"></as:Literal></p>
                    <i class="italic-text text-left">
                        <as:Literal ID="ltNote1" runat="server" Text="NOTE: This function automatically sends the user an email with the temporary password." meta:resourcekey="ltNote1Resource1"></as:Literal>
                        <br />
                    </i>
                </div>
                <div class="row">
                    <div class="col-md-12 form-action-container text-right">
                        <as:Button ID="uxCancel" Text="Close Window" runat="server" class="btn btn-default pull-right" OnClientClick="return closeMe();" meta:resourcekey="uxCancelResource1" />
                    </div>
                </div>
            </div>
        </div>
    </as:ASModalContainer>
    <asp:HiddenField ID="uxHidReload" runat="server" />
    <tek:RadCodeBlock ID="radCodeBlock" runat="server">
        <script type="text/javascript">
            var uxhidReload_ClientID = '<%= uxHidReload.ClientID %>';
        </script>
        <script src="<%= ResolveUrl("~/")%>res/js/usermaintenance/ResetPasswordForCS2.js"></script>
    </tek:RadCodeBlock>
</asp:Content>
