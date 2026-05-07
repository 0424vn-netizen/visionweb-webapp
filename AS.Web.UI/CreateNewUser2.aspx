<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CreateNewUser2.aspx.cs" Title="Create User"
    Inherits="_mps_CreateNewUser2" MasterPageFile="~/MasterPagePopup.master" meta:resourcekey="PageResource1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="Server">
    <as:ASModalContainer ID="uxModalContainer" runat="server" WidthCssClass="modal-sm" ContainerCssClass="container" Width=""> 
        <div class="row">
            <div class="col-md-12">
                <div class="box text-center">
                    <as:Literal ID="ltTheuseraccount" runat="server" Text="The user account" meta:resourcekey="ltTheuseraccountResource1"></as:Literal> <span class="red">
                        <asp:Literal ID="uxUserName" runat="server" meta:resourcekey="uxUserNameResource1" /></span> 
                    <%= String.Format(GetLocalResourceObject("CreateNewUser2_aspx_MsgPasswordExpired").ToString(),timeToExpired) %>
                            <br />
                    <div class="tmp-password">
                        <asp:Literal ID="uxPassword" runat="server" meta:resourcekey="uxPasswordResource1" /></div>
                </div>
            </div> 
        </div>
        <div class="row">
            <div class="col-md-12 action-container text-right">
                <as:Button ID="tbnClose" runat="server" CssClass="btn btn-default" Text="Close" OnClientClick="return parent.DoCloseCreatedUser();" meta:resourcekey="tbnCloseResource1"/>
            </div>
        </div>
    </as:ASModalContainer>
     <tek:RadCodeBlock ID="radCodeBlock" runat="server">
          <script type="text/javascript">
              var isFromCreateChainModal = '<%= isFromCreateNewChainModal %>';
              
        </script>
        <script src="<%= ResolveUrl("~/")%>res/js/usermaintenance/CreateUser2.js"></script>
    </tek:RadCodeBlock>
</asp:Content>
