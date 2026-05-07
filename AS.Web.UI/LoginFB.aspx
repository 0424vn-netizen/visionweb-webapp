<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Page_Login" Title=""
    MasterPageFile="~/MasterPageLogin.master" meta:resourcekey="PageResource1" %>

<asp:Content ID="Content2" ContentPlaceHolderID="Head" runat="Server">
    <tek:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <style>
            .login_footer {
                background: url("res/login/FULTON/login_footer.png") no-repeat scroll center center transparent;
                height: 100px;
            }

            .logo_footer {
                background: url("res/login/FULTON/logo_footer.png") no-repeat scroll center center transparent;
                height: 100px;
            }
        </style>
        <script type="text/javascript">
            var uxLogin_ClientID = '<%=uxLogin.ClientID%>';
            var msg_ValidationError = "<%= GetLocalResourceObject("Login_aspx_cs_InvalidUsernamePassword").ToString() %>";
        </script>
    </tek:RadCodeBlock>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPage" runat="Server">
    <asp:PlaceHolder ID="uxLoginboxContainer" runat="server">
        <as:PlaceHolder ID="uxWelcomeBarPanel" runat="server" Visible="False"></as:PlaceHolder>
        <h1 class="login-logo logo" runat="server" id="uxLogo">
            <as:Literal ID="ltHeaderLogoText" runat="server" Text="Aperia Solutions" meta:resourcekey="ltHeaderLogoTextResource1"></as:Literal></h1>
        <asp:Panel ID="uxLoginboxPanel" runat="server" class="login-box">
            <div class="login-top"></div>
            <div class="login-wrapper">
                <div class="login-container">
                    <as:PlaceHolder runat="server" ID="uxContactInfoColumn">
                        <div class="login-info">
                            <as:PlaceHolder ID="uxContactInfoPanel" runat="server">
                                <h4>
                                    <strong>
                                        <as:Literal runat="server" ID="uxCSHeader" Text="" meta:resourcekey="uxCSHeaderResource1"></as:Literal></strong>
                                </h4>
                                  <as:PlaceHolder ID="uxHeaderNotePanel" runat="server" Visible="false">
                                      <p style="padding-bottom:22px">
                                            <br />
                                         <as:Literal runat="server" ID="uxHeaderNote" meta:resourcekey="uxHeaderNoteResource1"></as:Literal>
                                     </p>                               
                                 </as:PlaceHolder> 
                                <as:PlaceHolder ID="uxPanelCompanyName" runat="server">
                                    <p>
                                        <as:Literal runat="server" ID="uxCompanyName" meta:resourcekey="uxCompanyNameResource1"></as:Literal>
                                    </p>
                                </as:PlaceHolder>
                                <as:PlaceHolder ID="uxHideAddress" runat="server">
                                    <p class="text-nowrap">
                                        <as:Literal runat="server" ID="uxCSAddr01" Text="" meta:resourcekey="uxCSAddr01Resource1"></as:Literal>
                                    </p>
                                    <as:PlaceHolder ID="uxAdd02Info" runat="server">
                                        <p class="text-left">
                                            <as:Literal runat="server" ID="uxCSAddr02" Text="" meta:resourcekey="uxCSAddr02Resource1"></as:Literal>
                                        </p>
                                    </as:PlaceHolder>
                                    <p>
                                        <as:Literal runat="server" ID="uxCSAddr03" Text="" meta:resourcekey="uxCSAddr03Resource1"></as:Literal>

                                    </p>
                                    <p>
                                        Customer Service Number:
                                        <as:Literal runat="server" ID="uxCSPhone" Text="" meta:resourcekey="uxCSPhoneResource1"></as:Literal>
                                    </p>
                                    <as:PlaceHolder ID="uxPanelFax" runat="server">
                                        <p>
                                            Fax Number:
                                            <as:Literal runat="server" ID="uxFax" Text="" meta:resourcekey="uxFaxResource1"></as:Literal>
                                        </p>
                                    </as:PlaceHolder>

                                </as:PlaceHolder>
                                <as:PlaceHolder ID="uxContactEmail" runat="server" Visible="true">
                                    <p>
                                        Email: <a href="mailto:<%=ClientEmail %>" style="font-size: 14px"><%=ClientEmail%></a>
                                    </p>
                                </as:PlaceHolder>
                            </as:PlaceHolder>
                            <as:PlaceHolder ID="uxContactMessage" runat="server">
                                <br />
                                <i>
                                    <as:Literal runat="server" ID="uxMes" Text="" meta:resourcekey="uxMesResource1"></as:Literal>
                                </i>
                            </as:PlaceHolder>
                            <as:PlaceHolder ID="uxPhoneNumber" runat="server" Visible="False">
                                <p>
                                    <as:Literal ID="uxFinancialIntitutions" runat="server" meta:resourcekey="uxFinancialIntitutionsResource1"></as:Literal>
                                </p>
                                <p>
                                    <as:Literal ID="uxDirectMerchants" runat="server" meta:resourcekey="uxDirectMerchantsResource1"></as:Literal>
                                </p>
                                <div>
                                    <br />
                                    <p>
                                        <as:Literal ID="ltImportance" runat="server" Text="IMPORTANT NOTICE:" meta:resourcekey="ltImportanceResource1"></as:Literal>
                                    </p>
                                    <i><%= GetLocalResourceObject("LoginFB_aspx_TheLink").ToString() %></i>
                                    <as:ClientResourceSetting runat="server" ID="uxInstructions" Visible="false"></as:ClientResourceSetting>
                                </div>
                            </as:PlaceHolder>
                        </div>
                    </as:PlaceHolder>
                    <div class="login-form pad-top0">
                        <h2 class="login-header" runat="server" id="uxLoginHeader" visible="False">Customer Service</h2>
                        <div class="form" runat="server" id="uxLoginInfo">
                            <div id="uxPanelLoginEnvironment" class="form-group" runat="server">
                                <label class="control-label enviroment-type">
                                    <as:Literal ID="uxCurrentEnvironment" runat="server" Text="QA Environment"></as:Literal>
                                </label>
                            </div>
                            <div class="form-group">
                                <label class="control-label">
                                    <as:Literal ID="ltUSERNAME" runat="server" Text="USERNAME:" meta:resourcekey="ltUSERNAMEResource1"></as:Literal>
                                    <asp:RequiredFieldValidator ValidationGroup="loginUserValidationGroup" ID="RequiredFieldValidator1" runat="server"
                                        ErrorMessage="*" CssClass="ValidateText" ControlToValidate="txtUserName" meta:resourcekey="RequiredFieldValidator1Resource1" /></label>
                                <asp:TextBox ID="txtUserName" runat="server" MaxLength="30" autocomplete="off" onkeypress="EnterSignIn(event);"
                                    CssClass="form-control" autofocus="on" meta:resourcekey="txtUserNameResource1" autocorrect="off"></asp:TextBox>
                            </div>
                            <div class="form-group">
                                <label class="control-label">
                                    <as:Literal ID="ltPASSWORD" runat="server" Text="PASSWORD:" meta:resourcekey="ltPASSWORDResource1"></as:Literal>
                                    <asp:RequiredFieldValidator ValidationGroup="loginUserValidationGroup" ID="RequiredFieldValidator2" runat="server"
                                        ErrorMessage="*" CssClass="ValidateText" ControlToValidate="txtPassword" meta:resourcekey="RequiredFieldValidator2Resource1" /></label>
                                <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" autocomplete="off"
                                    MaxLength="50" onkeypress="EnterSignIn(event);" CssClass="form-control" meta:resourcekey="txtPasswordResource1" autocorrect="off"></asp:TextBox>
                            </div>
                            <div class="display-flex align-center space-between">
                                <div>
                                    <asp:Button ID="uxLogin" runat="server" OnClientClick="performCheck();" Text="SIGN IN" OnClick="uxLogin_Click" CssClass="btn btn-primary btn-login pull-right"
                                        onkeypress="EnterSignIn(event);" meta:resourcekey="uxLoginResource1" />
                                </div>
                                <div>
                                    <a href="#" class="forgot-pass" onclick="doOpenForgetPassword(); return false;">
                                        <as:Literal ID="ltForgotPassword" runat="server" Text="Forgot
                                your password?"
                                            meta:resourcekey="ltForgotPasswordResource1"></as:Literal></a>
                                </div>
                            </div>
                            <p class="login-alert">
                                <span id="error-client"></span>
                                <asp:Literal ID="uxError" runat="server" meta:resourcekey="uxErrorResource1" />
                            </p>
                        </div>
                        <div class="row" runat="server" id="uxMsLink" visible="False"></div>

                    </div>
                </div>
                <br />
                 <div class="logo_footer"></div>
                <div id="login_note" class="login-note">
                    <as:Literal ID="ltSessionTimeOut" runat="server" Text="The login session will automatically timeout after being inactive for 15 minutes." meta:resourcekey="ltSessionTimeOutResource1"></as:Literal>
                </div>               
            </div>
        </asp:Panel>        
        <div class="login_footer"></div>

        <div id="browser_statement" class="site-info">
            <as:Literal ID="ltBrowsers" runat="server" Text="The site has been optimized for screen resolution of 1280 X 1024 and for the latest version of "
                meta:resourcekey="ltBrowsersResource1" />
            <img src="../res/img/EdgeLogo.png" class="supportBrowser">
            Edge, 
        <img src="../res/img/Chrome.png" class="supportBrowser">
            Chrome, and 
        <img src="../res/img/Firefox.png" class="supportBrowser">
            Firefox.

        </div>
    </asp:PlaceHolder>
    <asp:PlaceHolder ID="uxMaintenanceInfo" runat="server" Visible="False">
        <div class="maintenance">
            <h1 id="H1" class="logo_footer" runat="server"></h1>
            <div class="login-content">
                <div class="icon">
                    <img src="../res/images/icon_setting.png" alt="Icon" />
                </div>
                <h3 class="title">We are performing scheduled maintenance.</h3>
                <p class="description">The application will be available soon. Thank you for your patience.</p>
            </div>
        </div>
    </asp:PlaceHolder>
</asp:Content>
