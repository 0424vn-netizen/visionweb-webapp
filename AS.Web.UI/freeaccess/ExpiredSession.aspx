<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ExpiredSession.aspx.cs" MasterPageFile="~/MasterPagePopup.master" Inherits="page_ExpiredSession" meta:resourcekey="PageResource1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPage" runat="Server">
        <div class="row">
            <div class="col-md-12 text-center">
                <div class="box">
                    <span class="session-warning"><as:Literal ID="Literal1" runat="server" Text="Your session has expired." meta:resourcekey="Literal1Resource1" /></span>
                    <br />
                    <br />
                    <i class="text-muted"><as:Literal ID="Literal2" runat="server" Text="When a session is inactive for 15 minutes the session will expire. Please log into the site to begin another session." meta:resourcekey="Literal2Resource1" />
                
                    </i>
                </div>
            </div>
        </div>

        <div class="row">
            <div class="col-md-12 action-container text-right">
                <input type="button" value="<%= GetLocalResourceObject("ExpiredSession_aspx_CloseWindow").ToString() %>" class="btn btn-default" name="close" onclick="self.close();" id="uxclose" />
            </div>
        </div>
        <script type="text/javascript">
            isExpiredSession = true;
         </script>
        <script src="<%= ResolveUrl("~/")%>/res/js/ExpiredSession.js"></script>
</asp:Content>
