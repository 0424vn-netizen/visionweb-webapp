<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Page_Login" meta:resourcekey="PageResource1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Login</title>
    <tek:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link type="text/css" rel="Stylesheet" href="<%=ResolveUrl("~/") %>res/login/<%=_loginResFolder %>/login.css" />
        <!--[if lt IE 9]>
          <script src="res/js/bootstrap/html5shiv.js"></script>
          <script src="res/js/bootstrap/respond.min.js"></script>
        <![endif]-->
        <script src="res/js/jquery/jquery-3.6.0.min.js" type="text/javascript"></script>
        <script src="res/js/jquery/jquery-migrate-3.3.2.min.js" type="text/javascript"></script>
        <script src="res/js/jquery/jquery.cookie.js" type="text/javascript"></script>
        <script src="res/js/bootstrap/bootstrap.min.js" type="text/javascript"></script>
        <script src="res/js/common/common.js" type="text/javascript"></script>
        <script src="res/js/common/sessiontimeout.js" type="text/javascript"></script>
        <script type="text/javascript">
            window.logoutOpt = '<%= Request.Cookies.AllKeys.Contains("logout_opt") ? Request.Cookies["logout_opt"].Value : "0" %>';
        </script>
    </tek:RadCodeBlock>
</head>
<body style="margin: 0; background-color: #fff">
    <asp:PlaceHolder ID="uxLoginboxContainer" runat="server">
        <as:PlaceHolder ID="uxWelcomeBarPanel" runat="server" Visible="False"></as:PlaceHolder>
        <h1 class="login-logo logo" runat="server" id="uxLogo" visible="False"></h1>
        <div class="LoginBanner">
            <div class="LoginBannerLogo">
            </div>
        </div>
        <form id="form1" runat="server">
            <div align="center">
                <table width="960px" cellpadding="10">
                    <tr>
                        <td valign="top" align="left" style="width: 410px;">
                            <div class="SloganLeft">
                                <div class="Message">
                                    <as:PlaceHolder ID="uxContactMessage" runat="server">
                                        <as:Literal runat="server" ID="uxMes" Text="" meta:resourcekey="uxMesResource1"></as:Literal>&#8203
                                <as:Literal ID="ltat" runat="server" Text="at" meta:resourcekey="ltatResource1"></as:Literal>
                                        <as:Literal runat="server" ID="uxCSPhone" Text="" meta:resourcekey="uxCSPhoneResource1" Visible="false"></as:Literal>
                                        <%--<as:Literal ID="Literal1" runat="server" Text="or" meta:resourcekey="Literal1Resource1"></as:Literal>--%>
                                        <as:PlaceHolder ID="uxContactEmail" runat="server" Visible="true">
                                            <a href="mailto:<%=ClientEmail %>">
                                                <%=ClientEmail%></a>
                                        </as:PlaceHolder>
                                    </as:PlaceHolder>
                                </div>
                            </div>
                            <asp:Panel ID="uxLoginboxPanel" runat="server" class="login-box" Visible="False">
                                <as:PlaceHolder runat="server" ID="uxContactInfoColumn"></as:PlaceHolder>
                            </asp:Panel>
                            <div style="display: none;">
                                <as:PlaceHolder ID="uxContactInfoPanel" runat="server">
                                    <span style="font-weight: bold; font-size: 17px">
                                        <as:Literal runat="server" ID="uxCSHeader" Text="" meta:resourcekey="uxCSHeaderResource1"></as:Literal><br />
                                    </span>
                                      <as:PlaceHolder ID="uxHeaderNotePanel" runat="server" Visible="false">
                                          <p style="padding-bottom:22px">
                                             <as:Literal runat="server" ID="uxHeaderNote" meta:resourcekey="uxHeaderNoteResource1"></as:Literal>
                                         </p>                               
                                     </as:PlaceHolder>      
                                    <as:PlaceHolder ID="uxPanelCompanyName" runat="server">
                                        <as:Literal runat="server" ID="uxCompanyName" meta:resourcekey="uxCompanyNameResource1"></as:Literal>
                                    </as:PlaceHolder>
                                    <as:PlaceHolder ID="uxHideAddress" runat="server">
                                        <as:Literal runat="server" ID="uxCSAddr01" Text="" meta:resourcekey="uxCSAddr01Resource1"></as:Literal><br />
                                        <as:PlaceHolder ID="uxAdd02Info" runat="server">
                                            <as:Literal runat="server" ID="uxCSAddr02" Text="" meta:resourcekey="uxCSAddr02Resource1"></as:Literal><br />
                                        </as:PlaceHolder>
                                        <as:Literal runat="server" ID="uxCSAddr03" Text="" meta:resourcekey="uxCSAddr03Resource1"></as:Literal><br />
                                        <br />
                                        <as:PlaceHolder ID="uxPanelFax" runat="server">
                                            <as:Literal runat="server" ID="uxFax" Text="" meta:resourcekey="uxFaxResource1"></as:Literal><br />
                                        </as:PlaceHolder>
                                        <br />
                                    </as:PlaceHolder>
                                    <br />
                                </as:PlaceHolder>
                                <as:PlaceHolder ID="uxPhoneNumber" runat="server" Visible="False">
                                    <as:Literal ID="uxFinancialIntitutions" runat="server" meta:resourcekey="uxFinancialIntitutionsResource1"></as:Literal><br />
                                    <as:Literal ID="uxDirectMerchants" runat="server" meta:resourcekey="uxDirectMerchantsResource1"></as:Literal>
                                    <br />
                                    <br />
                                </as:PlaceHolder>
                                <as:ClientResourceSetting runat="server" ID="uxInstructions" Visible="False" ResourceKey="" RootURLProcess="False">
                                </as:ClientResourceSetting>
                            </div>
                        </td>
                        <td valign="top" align="left">
                            <h2 class="login-header" runat="server" id="uxLoginHeader" visible="False">Customer Service</h2>
                            <div id="uxLoginInfo" runat="server">
                                <div class="LoginForm">
                                    <div id="uxPanelLoginEnvironment" class="form-group" runat="server">
                                        <label class="control-label enviroment-type">
                                            <as:Literal ID="uxCurrentEnvironment" runat="server" Text="QA Environment"></as:Literal>
                                        </label>
                                    </div>
                                    <div>
                                        <div class="LoginFormTitle">
                                        </div>
                                        <div class="form-label">
                                            <as:Literal ID="Literal2" runat="server" Text="User name:" meta:resourcekey="Literal2Resource1"></as:Literal>
                                        </div>
                                        <div class="form-input">
                                            <asp:TextBox ID="txtUserName" runat="server" MaxLength="30" autocomplete="off" onkeypress="EnterSignIn(event);"
                                                CssClass="normal" Width="370px" meta:resourcekey="txtUserNameResource1" autocorrect="off"></asp:TextBox>
                                        </div>
                                        <div class="form-label">
                                            <as:Literal ID="Literal3" runat="server" Text="Password:" meta:resourcekey="Literal3Resource1"></as:Literal>
                                        </div>
                                        <div class="form-input">
                                            <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" autocomplete="off"
                                                MaxLength="50" onkeypress="EnterSignIn(event);" CssClass="normal" Width="370px"
                                                meta:resourcekey="txtPasswordResource1" autocorrect="off"></asp:TextBox>
                                        </div>
                                        <div class="form-input">
                                            <asp:Button ID="uxLogin" runat="server" OnClick="uxLogin_Click" Width="108px"
                                                Height="30px" CssClass="LoginButton" onkeypress="EnterSignIn(event);" meta:resourcekey="uxLoginResource1" />
                                            <div class="highlight-text forgot-pw">
                                                <a href="#" onclick="doOpenForgetPassword(); return false;">
                                                    <as:Literal ID="Literal4" runat="server" Text="Forgot your password?" meta:resourcekey="Literal4Resource1"></as:Literal></a>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="LoginFormMiddle">
                                        <asp:Literal ID="uxError" runat="server" meta:resourcekey="uxErrorResource1" />
                                    </div>
                                </div>
                                <div style="padding: 20px 5px 20px 35px; color: Red; text-align: left;" class="highlight-text">
                                    <as:Literal ID="Literal5" runat="server" Text="The login session will automatically timeout after being inactive for 15 minutes." meta:resourcekey="Literal5Resource1"></as:Literal>
                                </div>
                            </div>
                            <div class="row" runat="server" id="uxMsLink" visible="False"></div>

                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <div class="SloganBottom">
                            </div>
                        </td>
                    </tr>
                </table>
            </div>


            <asp:ScriptManager ID="ScriptManager1" runat="server" />
            <tek:RadWindowManager Modal="true" runat="server" ID="uxWindow" VisibleStatusbar="false"
                ReloadOnShow="true">
                <Windows>
                    <tek:RadWindow ID="radForgetWindow" runat="server" AutoSize="true" />
                    <tek:RadWindow ID="radForceWindow" runat="server" AutoSize="true" />
                    <tek:RadWindow ID="radPwdExpiredAlert" runat="server" AutoSize="true" />
                    <tek:RadWindow ID="radForgotWndMsg" runat="server" AutoSize="true" />
                </Windows>
            </tek:RadWindowManager>
            <tek:RadCodeBlock ID="RadCodeBlock2" runat="server">
                <script type="text/javascript" language="javascript">

                    function EnterSignIn(e) {
                        if ({ 13: 1 }[e.which || e.keyCode]) {
                            e.preventDefault ? e.preventDefault() : e.returnValue = false;
                            e.cancel = true;
                            document.getElementById("<%=uxLogin.ClientID%>").click();
                            return true;
                        }
                    }
                    var dialogs = null;
                    function doOpenForgetPassword() {
                        if (dialogs == null) dialogs = $find('<%=uxWindow.ClientID %>').get_windows();
                        dialogs[0].setUrl('freeaccess/forgotPassword1.aspx');
                        dialogs[0].show();
                        return false;
                    }
                    function doOpenForgotWndMsg(url) {
                        dialogs = $find('<%=uxWindow.ClientID %>').get_windows();
                        dialogs[3].setUrl('freeaccess/' + url);

                        dialogs[3].show();

                        //dialogs[0].Maximize();
                        return false;
                    }
                    checkSessionTimeout();
            // --> 
                </script>

            </tek:RadCodeBlock>
        </form>
    </asp:PlaceHolder>
    <asp:PlaceHolder ID="uxMaintenanceInfo" runat="server" Visible="False">
        <div class="maintenance">
            <h1 id="H1" class="login-logo logo" runat="server"></h1>
            <div class="login-content">
                <div class="icon">
                    <img src="../res/images/icon_setting.png" alt="Icon" />
                </div>
                 <h3 class="title">We are performing scheduled maintenance.</h3>
                <p class="description">The application will be available soon. Thank you for your patience.</p>
            </div>
        </div>
    </asp:PlaceHolder>




</body>
</html>
