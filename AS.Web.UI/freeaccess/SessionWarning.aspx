<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SessionWarning.aspx.cs" MasterPageFile="~/MasterPagePopup.master" Inherits="page_SessionWarning" Title="Session Expiration" meta:resourcekey="PageResource1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPage" runat="server">
        <div class="row">
            <div class="col-md-12 text-center">
                <div class="box">
                   <strong><as:Literal ID="Literal1" runat="server" Text="WARNING" meta:resourcekey="Literal1Resource1" /></strong>
                    <br />
                    <br />
                    <span class="session-warning"><as:Literal ID="Literal2" runat="server" Text="Your session is about to expire!" meta:resourcekey="Literal2Resource1" /> </span>
                    <br />
                    <br />
                    <i class="text-muted"><as:Literal ID="Literal3" runat="server" Text="Your session has been inactive for 15 minutes." meta:resourcekey="Literal3Resource1" />
                    <br /> 
                        <as:Literal ID="Literal4" runat="server" Text="If you would like to keep your session active, please click on the" meta:resourcekey="Literal4Resource1" />
                    <br />
                        <as:Literal ID="Literal5" runat="server" Text="<Continue> button." meta:resourcekey="Literal5Resource1" />
                    </i>
                    <br />
                    <strong><as:Literal ID="Literal6" runat="server" Text="Time Left:" meta:resourcekey="Literal6Resource1" /> &nbsp;<span id="cidTime" class="time-out"></span>
                    </strong> 
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12 action-container text-right">
                <as:Button ID="uxButton1" CssClass="btn btn-default" runat="server" Text="Continue" OnClientClick="return closeSessionWarningWindow()" meta:resourcekey="uxButton1Resource1" />
            </div>
        </div>
        <div class="hide">
            <as:Button ID="uxButton" runat="server" Text="" OnClick="uxButton_Click" />
        </div>
        <script src="<%=ResolveUrl("~/") %>res/js/common/sessiontimeout.js" type="text/javascript"></script>
        <script type="text/javascript">
            displayTimePanel = 'cidTime';
            submitTimeoutButton = '<%=uxButton.ClientID %>';
            $(document).ready(function () {
                countDown();
            });
            setTimeout('window.focus()', 1000);
        </script>
</asp:Content>
