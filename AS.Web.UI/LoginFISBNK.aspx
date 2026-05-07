<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Page_Login"
    MasterPageFile="~/MasterPageLogin.master" meta:resourcekey="PageResource1" %>

<asp:Content ID="Content2" ContentPlaceHolderID="Head" runat="Server">
    <tek:RadCodeBlock ID="RadCodeBlock1" runat="server">
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
            <as:Literal ID="ltAperiaSolutions" runat="server" Text="Aperia Solutions" meta:resourcekey="ltAperiaSolutionsResource1"></as:Literal></h1>
        <asp:Panel ID="uxLoginboxPanel" runat="server" class="login-box">
            <div class="login-top"></div>
            <div class="login-wrapper">
                <div class="login-container">
                    <as:PlaceHolder runat="server" ID="uxContactInfoColumn">
                        <div class="login-info">
                            <as:PlaceHolder ID="PlaceHolder1" runat="server" Visible="False">
                                <as:PlaceHolder ID="uxContactInfoPanel" runat="server" Visible="false">
                                    <span style="font-weight: bold;">
                                        <as:Literal runat="server" ID="uxCSHeader" Text="" meta:resourcekey="uxCSHeaderResource1"></as:Literal><br />
                                    </span>
                                    <br />
                                      <as:PlaceHolder ID="uxHeaderNotePanel" runat="server" Visible="false">
                                          <p style="padding-bottom:22px">
                                             <as:Literal runat="server" ID="uxHeaderNote" meta:resourcekey="uxHeaderNoteResource1"></as:Literal>
                                         </p>                               
                                     </as:PlaceHolder> 
                                    <as:PlaceHolder ID="uxPanelCompanyName" runat="server">
                                        <div style="margin-bottom: 10px;">
                                            <as:Literal runat="server" ID="uxCompanyName" meta:resourcekey="uxCompanyNameResource1"></as:Literal>
                                        </div>
                                    </as:PlaceHolder>
                                    <as:PlaceHolder ID="uxHideAddress" runat="server">
                                        <div style="display: none">
                                            <as:Literal runat="server" ID="uxCSAddr01" Text="" meta:resourcekey="uxCSAddr01Resource1"></as:Literal><br />
                                        </div>
                                        <as:PlaceHolder ID="uxAdd02Info" runat="server">
                                            <as:Literal runat="server" ID="uxCSAddr02" Text="" meta:resourcekey="uxCSAddr02Resource1"></as:Literal><br />
                                        </as:PlaceHolder>
                                        <div style="display: none">
                                            <as:Literal runat="server" ID="uxCSAddr03" Text="" meta:resourcekey="uxCSAddr03Resource1"></as:Literal><br />
                                        </div>
                                        <as:Literal runat="server" ID="Literal12" Text="Phone:" meta:resourcekey="Literal12Resource1"></as:Literal>
                                        <as:Literal runat="server" ID="uxCSPhone" Text="" meta:resourcekey="uxCSPhoneResource1"></as:Literal><br />
                                        <as:PlaceHolder ID="uxPanelFax" runat="server">
                                            <as:Literal runat="server" ID="Literal13" Text="Fax:" meta:resourcekey="Literal13Resource1"></as:Literal><br />
                                            <as:Literal runat="server" ID="uxFax" Text="" meta:resourcekey="uxFaxResource1"></as:Literal><br />
                                        </as:PlaceHolder>
                                    </as:PlaceHolder>
                                    <as:PlaceHolder ID="uxContactEmail" runat="server" Visible="true">
                                        Email: <a href="mailto:<%=ClientEmail %>" style="font-size: 14px">
                                            <%=ClientEmail%></a><br />
                                        <br />
                                    </as:PlaceHolder>
                                    <br />
                                </as:PlaceHolder>
                                <as:PlaceHolder ID="uxContactMessage" runat="server" Visible="true">
                                    <span style="font-style: italic;">
                                        <as:Literal runat="server" ID="uxMes" Text="" meta:resourcekey="uxMesResource1"></as:Literal><br />
                                    </span>
                                    <br />
                                </as:PlaceHolder>
                                <as:PlaceHolder ID="uxPhoneNumber" runat="server" Visible="false">
                                    <as:Literal ID="uxFinancialIntitutions" runat="server" meta:resourcekey="uxFinancialIntitutionsResource1"></as:Literal><br />
                                    <br />
                                    <as:Literal ID="uxDirectMerchants" runat="server" meta:resourcekey="uxDirectMerchantsResource1"></as:Literal>
                                    <br />
                                    <br />
                                </as:PlaceHolder>
                            </as:PlaceHolder>

                            <as:ClientResourceSetting runat="server" ID="uxInstructions" ResourceKey="LOGIN_INSTRUCTIONS_" RootURLProcess="False">
                            </as:ClientResourceSetting>
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
                                    <as:Literal ID="Literal1" runat="server" Text="USERNAME:" meta:resourcekey="Literal1Resource1"></as:Literal>
                                    <asp:RequiredFieldValidator ValidationGroup="loginUserValidationGroup" ID="RequiredFieldValidator1" runat="server"
                                        ErrorMessage="*" CssClass="ValidateText" ControlToValidate="txtUserName" meta:resourcekey="RequiredFieldValidator1Resource1" /></label>
                                <asp:TextBox ID="txtUserName" runat="server" MaxLength="30" autocomplete="off" onkeypress="EnterSignIn(event);"
                                    CssClass="form-control" autofocus="on" meta:resourcekey="txtUserNameResource1" autocorrect="off"></asp:TextBox>
                            </div>
                            <div class="form-group">
                                <label class="control-label">
                                    <as:Literal ID="Literal2" runat="server" Text="PASSWORD:" meta:resourcekey="Literal2Resource1"></as:Literal>
                                    <asp:RequiredFieldValidator ValidationGroup="loginUserValidationGroup" ID="RequiredFieldValidator2" runat="server"
                                        ErrorMessage="*" CssClass="ValidateText" ControlToValidate="txtPassword" meta:resourcekey="RequiredFieldValidator2Resource1" /></label>
                                <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" autocomplete="off"
                                    MaxLength="50" onkeypress="EnterSignIn(event);" CssClass="form-control" meta:resourcekey="txtPasswordResource1" autocorrect="off"></asp:TextBox>
                            </div>
                            <div class="display-flex align-center space-between">
                                <div>
                                    <asp:Button OnClientClick="performCheck();" ID="uxLogin" runat="server" Text="SIGN IN" OnClick="uxLogin_Click" CssClass="btn btn-primary btn-login pull-right"
                                        onkeypress="EnterSignIn(event);" meta:resourcekey="uxLoginResource1" />
                                </div>
                                <div>
                                    <a href="#" class="forgot-pass" onclick="doOpenForgetPassword(); return false;">
                                        <as:Literal ID="Literal3" runat="server" Text="Forgot your password?" meta:resourcekey="Literal3Resource1"></as:Literal></a>
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
                <div id="login_note" class="login-note">
                    <as:Literal ID="Literal9" runat="server" Text="The login session will automatically timeout after being inactive for 15 minutes." meta:resourcekey="Literal9Resource1"></as:Literal>
                </div>
            </div>
        </asp:Panel>

        <div id="browser_statement" class="site-info">
            <as:Literal ID="Literal10" runat="server" Text="The site has been optimized for screen resolution of 1280 X 1024 and for the latest version of "
                meta:resourcekey="Literal10Resource1"></as:Literal>
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


</asp:Content>
