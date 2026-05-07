<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Page_Login"
    MasterPageFile="~/MasterPageLogin.master" meta:resourcekey="PageResource1" %>

<asp:Content ID="Content2" ContentPlaceHolderID="Head" runat="Server">
    <%
        SessionManager.CurrentClient = WebSiteConstants.TOTAL_CLIENT;
    %>
    <tek:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <style>
            .login_address {
                background: url("res/login/TOTAL/login_address.png") no-repeat scroll center center transparent;
                display: inline-block;
                width: 305px;
                height: 177px;
            }

            .login-info {
                padding: 0px 30px !important;
            }

            .total .login-top:before {
                background-size: 100%;
            }

            .total .login-container .login-form .login-header {
                font-size: 28px;
                margin-top: 0;
                margin-bottom: 20px;
            }

            /*.total .login-container .login-form .forgot-pass {
                line-height: 20px;
                display: block;
                margin-top: -5px;
                margin-bottom: 27px;
            }*/

            h1.login-logo.logo.total {
                margin-top: 13px;
            }

                h1.login-logo.logo.total.mt-43 {
                    margin-top: 43px;
                }

            /*.total .login-container .login-form .form div.form-group:nth-child(2) {
                margin-bottom:10px;
            }*/

            .total .login-logo.logo {
                margin-top: 43px;
                margin-bottom: 0;
            }

            .total .login-box {
                margin-top: 12px;
            }

            .total .login-wrapper {
                padding: 50px 0;
            }

            .total .login-container .login-form {
                padding-top: 10px;
            }

            .total .login-container .login-info {
                padding-bottom: 13px !Important;
            }

            .login-box.total:before {
                content: '';
                background: white;
                position: absolute;
                width: 100%;
                height: 60px;
                left: 0;
                z-index: -1;
            }

            .sso-warning {
                margin-top: 18px;
                margin-bottom: -12px;
                height: 48px;
                font-weight: 700;
                color: rgb(77, 77, 236);
            }

            .login-signup {
                display: flex;
                align-items: center;
                justify-content: space-between;
            }
        </style>
        <script type="text/javascript">
            var uxLogin_ClientID = '<%=uxLogin.ClientID%>';
        </script>
    </tek:RadCodeBlock>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPage" runat="Server">
    <asp:PlaceHolder ID="uxLoginboxContainer" runat="server">
        <as:PlaceHolder ID="uxWelcomeBarPanel" runat="server" Visible="False">
            <div class="welcomebar navbar navbar-inverse navbar-fixed-top qa">
                <p class="navbar-text pull-left"></p>
            </div>
        </as:PlaceHolder>
        <h1 class="login-logo logo" runat="server" id="uxLogo">
            <as:Literal ID="ltAperiaSolutions" runat="server" Text="Aperia Solutions" meta:resourcekey="ltAperiaSolutionsResource1"></as:Literal></h1>
        <asp:Panel ID="uxLoginboxPanel" runat="server" class="login-box total">
            <div class="login-top"></div>
            <div class="login-wrapper">
                <div class="login-container">
                    <as:PlaceHolder runat="server" ID="uxContactInfoColumn">
                        <div class="login-info">
                            <as:PlaceHolder ID="uxContactInfoPanel" runat="server">
                                <as:PlaceHolder ID="uxPlaceHolderHeader" Visible="false" runat="server">
                                    <h4>
                                        <strong>
                                            <as:Literal runat="server" ID="uxCSHeader" Visible="false" Text="" meta:resourcekey="uxCSHeaderResource1"></as:Literal></strong>
                                    </h4>
                                </as:PlaceHolder>
                                  <as:PlaceHolder ID="uxHeaderNotePanel" runat="server" Visible="false">
                                      <p style="padding-bottom:22px">
                                         <as:Literal runat="server" ID="uxHeaderNote" meta:resourcekey="uxHeaderNoteResource1"></as:Literal>
                                     </p>                               
                                 </as:PlaceHolder> 
                                <as:PlaceHolder ID="uxPanelCompanyName" runat="server">
                                    <p>
                                        <as:Literal runat="server" ID="uxCompanyName" meta:resourcekey="uxCompanyNameResource1"></as:Literal>
                                    </p>
                                </as:PlaceHolder>
                                <as:PlaceHolder ID="uxHideAddress" runat="server">
                                    <div class="login_address"></div>
                                    <as:PlaceHolder ID="uxContactMessage" runat="server" Visible="true">
                                        <p>
                                            <br />
                                            <as:Literal runat="server" ID="uxMes" Text="" meta:resourcekey="uxMesResource1"></as:Literal>
                                        </p>
                                    </as:PlaceHolder>
                                    <p class="text-nowrap">
                                        <as:Literal runat="server" ID="uxCSAddr01" Text="" Visible="false" meta:resourcekey="uxCSAddr01Resource1"></as:Literal>
                                    </p>
                                    <as:PlaceHolder ID="uxAdd02Info" runat="server">
                                        <p class="text-left">
                                            <as:Literal runat="server" ID="uxCSAddr02" Text="" meta:resourcekey="uxCSAddr02Resource1"></as:Literal>
                                        </p>
                                    </as:PlaceHolder>
                                    <p>
                                        <as:Literal runat="server" ID="uxCSAddr03" Text="" Visible="false" meta:resourcekey="uxCSAddr03Resource1"></as:Literal>

                                    </p>
                                    <br />
                                    <p>
                                        <as:Literal ID="Literal1" runat="server" Text="Phone:" meta:resourcekey="Literal1Resource1"></as:Literal>
                                        <as:Literal runat="server" ID="uxCSPhone" Text="" meta:resourcekey="uxCSPhoneResource1"></as:Literal>
                                    </p>
                                    <as:PlaceHolder ID="uxPanelFax" runat="server" Visible="false">
                                        <p>
                                            <as:Literal ID="Literal2" runat="server" Text="Fax:" meta:resourcekey="Literal2Resource1"></as:Literal>
                                            <as:Literal runat="server" ID="uxFax" Text="" meta:resourcekey="uxFaxResource1"></as:Literal>
                                        </p>
                                    </as:PlaceHolder>

                                </as:PlaceHolder>
                                <as:PlaceHolder ID="uxContactEmail" runat="server" Visible="true">
                                    <p>
                                        <as:Literal ID="Literal3" runat="server" Text="Email:" meta:resourcekey="Literal3Resource1"></as:Literal>
                                        <a href="mailto:<%=ClientEmail %>" style="font-size: 14px">
                                            <%=ClientEmail%></a>
                                    </p>
                                </as:PlaceHolder>
                            </as:PlaceHolder>

                            <as:PlaceHolder ID="uxPhoneNumber" runat="server" Visible="False">
                                <p>
                                    <as:Literal ID="uxFinancialIntitutions" runat="server" meta:resourcekey="uxFinancialIntitutionsResource1"></as:Literal>
                                </p>
                                <p>
                                    <as:Literal ID="uxDirectMerchants" runat="server" meta:resourcekey="uxDirectMerchantsResource1"></as:Literal>
                                </p>
                                <p>
                                    <as:ClientResourceSetting runat="server" ID="uxInstructions" Visible="false">
                                    </as:ClientResourceSetting>
                                </p>
                            </as:PlaceHolder>
                        </div>
                    </as:PlaceHolder>
                    <div class="login-form pad-top0">
                        <div class="form" runat="server" id="uxLoginInfo">
                            <div id="uxPanelLoginEnvironment" class="form-group" runat="server">
                                <label class="control-label enviroment-type">
                                    <as:Literal ID="uxCurrentEnvironment" runat="server" Text="QA Environment"></as:Literal>
                                </label>
                            </div>
                            <h2 class="login-header" runat="server" id="uxLoginHeader" visible="False">Customer Service</h2>
                            <div class="form-group">
                                <label class="control-label">
                                    <as:Literal runat="server" ID="Literal4" Text="USERNAME:" meta:resourcekey="Literal4Resource1"></as:Literal>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ValidationGroup="loginUserValidationGroup"
                                        ErrorMessage="*" CssClass="ValidateText" ControlToValidate="txtUserName" meta:resourcekey="RequiredFieldValidator1Resource1" /></label>
                                <asp:TextBox ID="txtUserName" runat="server" MaxLength="30" autocomplete="off" onkeypress="EnterSignIn(event);" onblur="validateSSOUser()" ValidationGroup="loginUserValidationGroup"
                                    CssClass="form-control" autofocus="on" meta:resourcekey="txtUserNameResource1" autocorrect="off"></asp:TextBox>
                            </div>
                            <div id="aperiaForm">
                                <div class="form-group">
                                    <label class="control-label">
                                        <as:Literal runat="server" ID="Literal5" Text="PASSWORD:" meta:resourcekey="Literal5Resource1"></as:Literal>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ValidationGroup="loginUserValidationGroup"
                                            ErrorMessage="*" CssClass="ValidateText" ControlToValidate="txtPassword" meta:resourcekey="RequiredFieldValidator2Resource1" /></label>
                                    <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" autocomplete="off" ValidationGroup="loginUserValidationGroup"
                                        MaxLength="50" onkeypress="EnterSignIn(event);" CssClass="form-control" meta:resourcekey="txtPasswordResource1" autocorrect="off"></asp:TextBox>
                                </div>
                                <div class="login-signup">
                                    <div class="text-left">
                                        <asp:Button ID="uxLogin" runat="server" OnClientClick="performCheck();" Text="SIGN IN" OnClick="uxLogin_Click" CssClass="btn btn-primary btn-login pull-right" ValidationGroup="loginUserValidationGroup"
                                            onkeypress="EnterSignIn(event);" meta:resourcekey="uxLoginResource1" />
                                    </div>
                                    <div class="text-right">
                                        <a href="#" class="forgot-pass" onclick="doOpenForgetPassword(); return false;">
                                            <as:Literal runat="server" ID="Literal13" Text="Forgot password?"
                                                meta:resourcekey="Literal6Resource1"></as:Literal></a>
                                    </div>
                                </div>
                                <p class="login-alert">
                                    <span id="error-client"></span>
                                    <asp:Literal ID="uxError" runat="server" meta:resourcekey="uxErrorResource1" />
                                </p>
                            </div>
                            <div id="ssoForm" class="display-none">
                                <div class="text-center">
                                    <p class="sso-warning">
                                        <asp:Literal ID="Literal12" runat="server" Text="It seems that you are a My Accounts User" />
                                    </p>
                                </div>
                                <div class="row">
                                    <div class="col-xs-12 text-center">
                                        <asp:Button ID="Button1" runat="server" Text="GO TO MY ACCOUNTS" OnClick="uxGoToAccount_Click" CssClass="btn btn-primary btn-login" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row" runat="server" id="uxMsLink" visible="False" style="margin-top: 20px;">
                            <div class="col-xs-12 text-center">In the wrong place? <a href="<%= GeneralFuncsLib.GetDataOfExtendedSetting("MS_SITE_URL") %>">Login as a Merchant.</a></div>
                        </div>

                    </div>
                </div>
            </div>
        </asp:Panel>
        <br />
        <div class="login_footer"></div>



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

    <as:RadCodeBlock ID="ScriptManagement" runat="server">
        <script type="text/javascript">
            var isTMSSiteJump = "<%=IsTMSSiteJump%>";
            var msg_ValidationError = "<%= GetLocalResourceObject("Login_aspx_cs_InvalidUsernamePassword").ToString() %>";
            $(document).ready(function () {
                if (isTMSSiteJump == "True") {
                    var id = "<%=txtUserName.ClientID%>";
                    $("#" + id).keydown(function (e) {
                        if (e.which == 9) {
                            var userId = $(this).val();
                            isSSOUser(userId);
                        }
                    });
                }
            });

            function validateSSOUser() {
                if (isTMSSiteJump == "True") {
                    var id = "<%=txtUserName.ClientID%>";
                    var userId = $("#" + id).val();
                    isSSOUser(userId);
                }
            }

            function isSSOUser(userId) {
                var url = "LoginTotal.aspx/CheckSSOUser"; // document.URL.replace('#', '') + '/CheckSSOUser';
                $.ajax({
                    type: "POST",
                    url: url,
                    data: '{"userId":"' + userId + '"}',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (result) {
                        var val = result.d[0];
                        if (val == "true") {
                            $("#aperiaForm").addClass("display-none");
                            $("#ssoForm").removeClass("display-none");
                        }
                    },
                    error: function (result) {
                        console.log(result);
                    }
                });
            }
        </script>
    </as:RadCodeBlock>
</asp:Content>
