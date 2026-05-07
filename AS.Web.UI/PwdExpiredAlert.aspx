<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PwdExpiredAlert.aspx.cs"
    Inherits="_mps_PwdExpiredAlert" MasterPageFile="~/MasterPagePopup.master" meta:resourcekey="PageResource1" %>
 
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPage" runat="Server">
    <as:ASModalContainer ID="uxModalContainer" runat="server" WidthCssClass="modal-sm" ContainerCssClass="container" Width="">

        <div style="<%=!IsPostBack?"display: none; ": "" %>" id="div" class="loading-bg">
            <div class="loading-image">
                <img src="res/img/progresswithprocessingtext.gif" />
            </div>
        </div>
        <div class="row">
            <div class="col-md-12 text-center">
                <div class="box">
                    <as:Literal ID="ltYourPwdExpired" runat="server" Text="Your password will expire in" meta:resourcekey="ltYourPwdExpiredResource1"></as:Literal> <span class="em">
                        <asp:Literal ID="uxDaysRemained" runat="server" meta:resourcekey="uxDaysRemainedResource1"></asp:Literal></span> <asp:Literal ID="Literal1" runat="server" Text="days." meta:resourcekey="Literal1Resource1"></asp:Literal>
                    <br />
                    <br />
                    <as:Literal ID="ltResetNow" runat="server" Text="Either select [Reset Now] to reset password now" meta:resourcekey="ltResetNowResource1"></as:Literal><br />
                    <as:Literal ID="ltNow" runat="server" Text="or select [Continue] if you plan to reset password later." meta:resourcekey="ltNowResource1"></as:Literal>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12 action-container text-right">
                <asp:Button ID="uxResetPwd" runat="server" Text="Reset Now"  CssClass="btn btn-default"
                    OnClick="uxResetPwd_Click" meta:resourcekey="uxResetPwdResource1" />
                <asp:Button ID="uxContinue" runat="server" Text="Continue"  CssClass="btn btn-default"
                    OnClick="uxContinue_Click" OnClientClick="return openModal();" meta:resourcekey="uxContinueResource1" />
            </div>
        </div>
    </as:ASModalContainer>
    <tek:RadCodeBlock ID="radCodeBlk" runat="server">
        <script src="<%= ResolveUrl("~/")%>res/js/PwdExpiredAlert.js"> </script>
    </tek:RadCodeBlock> 
</asp:Content>