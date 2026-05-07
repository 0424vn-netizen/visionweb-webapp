<%@ Page Title="ActivatePwd" Language="C#" MasterPageFile="~/MasterPagePopup.master"
    AutoEventWireup="true" CodeFile="ActivatePwd.aspx.cs" Inherits="ActivatePwd" %>



<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="Server">
    <div id="uxProgress" runat="server" style="padding: auto; position: fixed; vertical-align: middle; text-align: center; z-index: 9999; height: 100%; width: 100%; top: 0px; left: 0px; bottom: 0px; right: 0px; background: #FFF url('/res/Images/loading.gif') no-repeat center center">
    </div>
    <div>Login failed</div>
    <div class="hide">
        <asp:Button ID="uxLogin" runat="server" Text="Button" OnClick="uxLogin_Click" />
        <asp:Button ID="uxLogout" runat="server" Text="Button" OnClick="uxLogOut_Click" />
    </div>

    <tek:RadAjaxManagerProxy runat="server" ID="uxRadAjaxManager">
        <AjaxSettings>
            <tek:AjaxSetting AjaxControlID="uxLogout">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="uxLogout" />
                </UpdatedControls>
            </tek:AjaxSetting>

            <tek:AjaxSetting AjaxControlID="uxLogin">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="uxLogin" />
                    <tek:AjaxUpdatedControl ControlID="uxProgress" />
                </UpdatedControls>
            </tek:AjaxSetting>
        </AjaxSettings>
    </tek:RadAjaxManagerProxy>
    <as:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <script type="text/javascript">
            var uxLogin_ClientID = "<%= uxLogin.ClientID%>";
            var uxLogout_ClientID = "<%= uxLogout.ClientID%>";

            $(document).ready(function () {
                document.getElementById(uxLogout_ClientID).click();
                document.getElementById(uxLogin_ClientID).click();
            });

        </script>

    </as:RadCodeBlock>
</asp:Content>
