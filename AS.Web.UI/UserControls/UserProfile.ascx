<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserProfile.ascx.cs" Inherits="As.VisionWeb.Web.UserProfileControl" %>
<!-- Rad Ajax-->

<%@ Register Src="~/UserControls/UserProfileMasterUserControl.ascx" TagName="UserProfileMasterUserControl" TagPrefix="uc" %>

<as:Container ID="Container1" runat="server" Width="100%" FooterControlID="" FooterText="" HeaderControlID="" HeaderText="" TemplateName="ascontainer_greyborder.tpl">

    <div class="row">
        <div class="col-md-8">
            <p id="uxWarning1" runat="server" visible="false" class="red">
                <i>
                    <as:Literal ID="ltPleaseChangePw" runat="server" Text="Your password has expired.
                Please change your password now."
                        meta:resourcekey="ltPleaseChangePwResource1"></as:Literal></i>
            </p>
            <p id="uxWarning2" runat="server" visible="false" class="red">
                <i>
                    <as:Literal ID="ltWarning2" runat="server" Text="You are logging in
                with temporary password. Please change your password now."
                        meta:resourcekey="ltWarning2Resource1"></as:Literal></i>
            </p>
            <p id="uxWarning3" runat="server" visible="false" class="red">
                <i><%= string.Format(GetLocalResourceObject("UserProfile_ascx_cs_WarningPwExpire").ToString(), _dayRemained) %></i>
            </p>
        </div>
    </div>
    <div class="row">
        <div class="col-md-8">
            <table class="ASTable form-inline" id="table1">
                <as:PlaceHolder ID="IsUserSignOn" runat="server" Visible="false">
                    <tr id="Tr1" class="AltRow">
                        <td class="heading w-30"></td>
                        <td class="heading">
                            <as:CheckBox ID="ckChangeUserName" runat="server" Text="Change User Name" onclick="checkBoxChangeUserName();" meta:resourcekey="lbChangeUserName" />
                        </td>
                    </tr>
                    <tr>
                        <td class="heading valign-middle w-30">
                            <as:Literal ID="lbOriginalUserName" runat="server" Text="Original User Name:" meta:resourcekey="lbOriginalUserName"></as:Literal>
                        </td>
                        <td>
                            <as:TextBox ID="uxOriginalUserName" CssClass="form-control" runat="server" Width="70%" Enabled="false" />
                        </td>
                    </tr>
                    <tr>
                        <td class="heading valign-middle w-30">
                            <as:ValidatorLabel ID="lbNewUserName" runat="server" ApplyFor="uxNewUserName" Text="New User Name:" meta:resourcekey="lbNewUserName" />
                        </td>
                        <td>
                            <as:TextBox ID="uxNewUserName" CssClass="form-control" runat="server" Width="70%" ReadOnly="false" />
                            <as:ValidatorMessage runat="server" ID="txtNewUserNameErrMsg" ApplyFor="uxNewUserName" Message="" ShowOnLoad="False" />
                        </td>
                    </tr>
                </as:PlaceHolder>
                <as:PlaceHolder ID="IsNotUserSignOn" runat="server" Visible="false">
                    <tr>
                        <td class="heading w-30">
                            <as:Literal ID="ltUsername" runat="server" Text="User Name:" meta:resourcekey="ltUsernameResource1"></as:Literal>
                        </td>
                        <td>
                            <as:TextBox ID="uxUserName" CssClass="form-control" runat="server" Width="70%" Enabled="false" meta:resourcekey="uxUserNameResource1" />
                        </td>
                    </tr>
                </as:PlaceHolder>
                <tr>
                    <td class="heading">
                        <as:ValidatorLabel ID="txtFirstNameLabel" runat="server" Text="First Name:" ApplyFor="uxFirstName" meta:resourcekey="txtFirstNameLabelResource1" />
                    </td>
                    <td runat="server" id="tdFirstName">
                        <as:TextBox ID="uxFirstName" CssClass="form-control" ReadOnly="false" runat="server"
                            Width="70%" MaxLength="50" meta:resourcekey="uxFirstNameResource1" />
                        <as:ValidatorMessage runat="server" ID="txtFirstNameErrMsg" ApplyFor="uxFirstName" />
                    </td>
                    <td runat="server" id="tdFirstNameDisabled" visible="false">
                        <as:TextBox ID="uxFirstNameDisabled" CssClass="form-control" ReadOnly="false" Enabled="false" runat="server"
                            Width="70%" MaxLength="50" />
                    </td>
                </tr>
                <tr>
                    <td class="heading">
                        <as:ValidatorLabel ID="txtLastNameLabel" runat="server" Text="Last Name:" ApplyFor="uxLastName" meta:resourcekey="txtLastNameLabelResource1" />
                    </td>
                    <td runat="server" id="tdLastName">
                        <as:TextBox ID="uxLastName" CssClass="form-control" ReadOnly="false" runat="server"
                            Text="" Width="70%" MaxLength="50" meta:resourcekey="uxLastNameResource1" />
                        <as:ValidatorMessage runat="server" ID="txtLastNameErrMsg" ApplyFor="uxLastName" />
                    </td>
                    <td runat="server" id="tdLastNameDisabled" visible="false">
                        <as:TextBox ID="uxLastNameDisabled" CssClass="form-control" ReadOnly="false" Enabled="false" runat="server"
                            Width="70%" MaxLength="50" />
                    </td>
                </tr>
                <tr>
                    <td class="heading w-30">
                        <as:ValidatorLabel ID="txtEmailLabel" runat="server" Text="Email:" ApplyFor="uxEmail" meta:resourcekey="txtEmailLabelResource1" />
                    </td>
                    <td runat="server" id="tdEmail">
                        <as:TextBox ID="uxEmail" ReadOnly="false" CssClass="form-control" runat="server"
                            Text="" Width="70%" MaxLength="200" meta:resourcekey="uxEmailResource1" />
                        <as:ValidatorMessage runat="server" ID="txtEmailErrMsg" ApplyFor="uxEmail" />
                    </td>
                    <td runat="server" id="tdEmailDisabled" visible="false">
                        <as:TextBox ID="uxEmailDisabled" CssClass="form-control" ReadOnly="false" Enabled="false" runat="server"
                            Width="70%" MaxLength="200" />
                    </td>
                </tr>
                <tr id="uxPhoneSMSContainer" runat="server">
                    <td class="heading w-30">
                        <as:ValidatorLabel ID="txtPhoneLabel" runat="server" Text="Email:" ApplyFor="uxPhone" meta:resourcekey="txtPhoneLabelResource1" />
                    </td>
                    <td runat="server" id="tdPhone">
                        <as:TextBox ID="uxPhone" CssClass="form-control" runat="server" Width="70%" MaxLength="15" autocomplete="off" display-masked='Phone' />
                        <as:ValidatorMessage runat="server" ID="uxPhoneErrMsg" ApplyFor="uxPhone" />
                    </td>
                    <td runat="server" id="tdPhoneDisabled" visible="false">
                        <as:TextBox ID="uxPhoneDisabled" CssClass="form-control" ReadOnly="false" Enabled="false" runat="server"
                            Width="70%" MaxLength="200" />
                    </td>
                </tr>
                <tr id="uxEmailContactContainer" runat="server">
                    <td class="heading w-30">
                        <as:ValidatorLabel ID="txtEmailContactLabel" runat="server" Text="Email:" ApplyFor="uxEmailContact" meta:resourcekey="txtEmailContactLabelResource1" />
                    </td>
                    <td runat="server" id="tdEmailContact">
                        <as:TextBox ID="uxEmailContact" ReadOnly="false" CssClass="form-control" runat="server"
                            Text="" Width="70%" MaxLength="200" meta:resourcekey="uxEmailResource1" />
                        <as:ValidatorMessage runat="server" ID="txtEmailContactErrMsg" ApplyFor="uxEmailContact" />
                    </td>
                    <td runat="server" id="tdEmailContactDisabled" visible="false">
                        <as:TextBox ID="uxEmailContactDisabled" CssClass="form-control" ReadOnly="false" Enabled="false" runat="server"
                            Width="70%" MaxLength="200" />
                    </td>
                </tr>

            </table>
            <div class="height-12"></div>
            <as:PlaceHolder ID="uxTMSAccount" runat="server" Visible="false">
                <div>
                    <i>
                        <as:Literal runat="server" ID="uxTMSNote"></as:Literal>
                    </i>
                </div>
            </as:PlaceHolder>
            <as:PlaceHolder ID="uxPanelEmailNote" runat="server" Visible="false">
                <div>
                    <i>
                        <as:Literal runat="server" ID="uxEmailNote" meta:resourcekey="uxEmailNoteResource1"></as:Literal>
                    </i>
                </div>
                <div class="height-10"></div>
            </as:PlaceHolder>
            <i>
                <asp:Literal ID="uxpasswordrule" runat="server" Text="The password length must be a minimum of 12 and a maximum of 50 characters, contain at least 1 number, and may not include the < or > characters." meta:resourcekey="uxpasswordruleResource1"></asp:Literal></i>
            <div class="height-8"></div>
            <table class="ASTable form-inline" id="table2">
                <tr id="cidCheckChangePassRow">
                    <td class="heading w-30"></td>
                    <td class="heading">
                        <as:CheckBox ID="uxCheckChangePass" runat="server" onclick="checkBoxChangePass();"
                            Text="Change Password" meta:resourcekey="uxCheckChangePassResource1" />
                    </td>
                </tr>
                <tr>
                    <td class="heading w-30">
                        <as:ValidatorLabel ID="txtOldPassLabel" runat="server" Text="Old Password:" ApplyFor="uxOldPass" meta:resourcekey="txtOldPassLabelResource1" />
                    </td>
                    <td>
                        <as:TextBox ID="uxOldPass" runat="server" CssClass="form-control" TextMode="Password"
                            MaxLength="50" Enabled="false" autocomplete="off"
                            Width="70%" meta:resourcekey="uxOldPassResource1" autocorrect="off" />

                           <as:TextBox ID="uxOldTempPassword" runat="server" CssClass="form-control" TextMode="Password"
                            MaxLength="50" autocomplete="off" Visible="false" Enabled="false"
                            Width="70%" meta:resourcekey="uxOldPassResource1" autocorrect="off" />
                        <as:ValidatorMessage runat="server" ID="txtOldPassErrMsg" ApplyFor="uxOldPass" />
                    </td>
                </tr>
                <tr>
                    <td class="heading w-30">
                        <as:ValidatorLabel ID="txtNewPassLabel" runat="server" Text="New Password:" ApplyFor="uxPassword" meta:resourcekey="txtNewPassLabelResource1" />
                    </td>
                    <td>
                        <as:TextBox ID="uxPassword" runat="server" CssClass="form-control" TextMode="Password"
                            MaxLength="50" Enabled="false" autocomplete="off"
                            Width="70%" meta:resourcekey="uxPasswordResource1" autocorrect="off" />
                        <div class="error custom-error" runat="server" id="errorPassword"></div>
                        <div class="hide">
                            <as:ValidatorMessage runat="server" ID="txtNewPassErrMsg" ApplyFor="uxPassword" />
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="heading w-30">
                        <as:ValidatorLabel ID="txtConfirmPassLabel" runat="server" Text="Confirm New Password:"
                            ApplyFor="uxConfirmPass" meta:resourcekey="txtConfirmPassLabelResource1" />
                    </td>
                    <td>
                        <as:TextBox ID="uxConfirmPass" TextMode="Password" CssClass="form-control" MaxLength="50"
                            runat="server" autocomplete="off"
                            Enabled="false" Width="70%" meta:resourcekey="uxConfirmPassResource1" autocorrect="off" />
                        <as:ValidatorMessage runat="server" ID="txtConfirmPassErrMsg" ApplyFor="uxConfirmPass" />
                    </td>
                </tr>
            </table>
            <div class="height-12"></div>
            <as:PlaceHolder ID="uxPanelConfirmPassNote" runat="server" Visible="false">
                <div>
                    <i>
                        <as:Literal runat="server" ID="uxConfirmPassNote" meta:resourcekey="uxConfirmPassNoteResource1"></as:Literal>
                    </i>
                </div>
            </as:PlaceHolder>
            <div class="height-12"></div>
            <as:PlaceHolder ID="uxPldQuestion" runat="server">
                <table class="ASTable form-inline" id="table3">
                    <tr id="cidCheckChangeQuestionRow" class="AltRow">
                        <td class="heading w-30">
                        <td class="heading">
                            <as:CheckBox ID="uxCheckChangeQuestion" runat="server" onclick="checkBoxChangeQuestion();"
                                Text="Change Validation Question/Answer" meta:resourcekey="uxCheckChangeQuestionResource1" />
                        </td>
                    </tr>
                    <tr class="Row">
                        <td class="heading w-30">
                            <as:ValidatorLabel ID="ValidatorLabel5" runat="server" Text="Validation Question:"
                                ApplyFor="uxListValidQuestion" meta:resourcekey="ValidatorLabel5Resource1" />

                        </td>
                        <td>
                            <as:RadComboBox ID="uxListValidQuestion" runat="server" Enabled="false" Width="70%"
                                DataTextField="Text" DataValueField="Value" meta:resourcekey="uxListValidQuestionResource1" />
                        </td>
                    </tr>
                    <tr class="AltRow">
                        <td class="heading w-30">
                            <as:ValidatorLabel ID="txtAnswerLabel" runat="server" Text="Answer:" ApplyFor="uxAnswer" meta:resourcekey="txtAnswerLabelResource1" />
                        </td>
                        <td>
                            <as:TextBox runat="server" ID="uxAnswer" CssClass="form-control" Enabled="false"
                                Width="70%" MaxLength="50" autocomplete="off" meta:resourcekey="uxAnswerResource1" />
                            <as:ValidatorMessage runat="server" ID="txtAnswerErrMsg" ApplyFor="uxAnswer" />
                        </td>
                    </tr>
                </table>
            </as:PlaceHolder>
            <div class="height-12"></div>
            <div class="height-12"></div>
            <table class="ASTable form-inline" id="table4">
                <tr>
                    <td class="heading" colspan="2" style="background-color: #DDD;">
                        <as:Literal ID="ltManageSetting" runat="server" Text="Manage Settings" meta:resourcekey="ltManageSettingResource1"></as:Literal>
                    </td>
                </tr>
                <!-- 44758 - VW CMS Case Type Default Preference -  -->
                <tr runat="server" id="uxRiskCaseTypeDefault">
                    <td class="heading w-30">
                        <label class="control-label">
                            <as:Literal ID="Literal3" runat="server" Text="Case Type Default:" meta:resourcekey="ltCaseTypeDefaultResource" /></label>
                    </td>
                    <td>
                        <asp:RadioButtonList ID="RadioButtonCaseTypeDefault" runat="server" RepeatDirection="Horizontal" CssClass="noBorderTable w-min-180 table-no-border">
                            <asp:ListItem Value="RISK" Text="RISK" Selected="True" meta:resourcekey="ltiCaseTypeDefault_RISK_Resource" />
                            <asp:ListItem Value="CMS" Text="CMS" meta:resourcekey="ltiCaseTypeDefault_CMS_Resource" />
                        </asp:RadioButtonList>
                    </td>
                </tr>

                <tr>
                    <td class="heading w-30">
                        <label class="control-label">
                            <as:Literal ID="ltFilterDefault" runat="server" Text="Filters Default View:" meta:resourcekey="ltFilterDefaultResource1" /></label>
                    </td>
                    <td>
                        <asp:RadioButtonList ID="RadioButtonFilterDefault" runat="server" RepeatDirection="Horizontal" CssClass="noBorderTable w-min-180 table-no-border">
                            <asp:ListItem Value="Expand" Text="Expand" meta:resourcekey="ltiExpandResource1" />
                            <asp:ListItem Value="Collapse" Text="Collapse" Selected="True" meta:resourcekey="ltiCollapseResource1" />
                        </asp:RadioButtonList>
                    </td>
                </tr>
                <tr>
                    <td class="heading w-30">
                        <label class="control-label">
                            <as:Literal ID="ltGraphDefault" runat="server" Text="Graphs Default View:" meta:resourcekey="ltGraphDefaultResource1" />
                        </label>
                    </td>
                    <td>
                        <asp:RadioButtonList ID="RadioButtonGraphDefault" runat="server" RepeatDirection="Horizontal" CssClass="noBorderTable w-min-180 table-no-border">
                            <asp:ListItem Value="Expand" Text="Expand" Selected="True" meta:resourcekey="ltiExpandResource1" />
                            <asp:ListItem Value="Collapse" Text="Collapse" meta:resourcekey="ltiCollapseResource1" />
                        </asp:RadioButtonList>
                    </td>
                </tr>
                <tr runat="server" id="uxRiskDetectionQueueDefaultView">
                    <td class="heading w-30">
                        <label class="control-label">
                            <as:Literal ID="Literal1" runat="server" Text="Risk Detection Queue Default View:" meta:resourcekey="ASLiteralResource1" /></label>
                    </td>
                    <td>
                        <asp:RadioButtonList ID="RadioButtonDetectionQueueDefault" runat="server" RepeatDirection="Horizontal" CssClass="noBorderTable w-min-180 table-no-border">
                            <asp:ListItem Value="Grid" Text="Grid" Selected="True" meta:resourcekey="ASListItemResource1" />
                            <asp:ListItem Value="Card" Text="Card" meta:resourcekey="ASListItemResource2" />
                        </asp:RadioButtonList>
                    </td>
                </tr>
                <tr>
                    <td class="heading w-30">
                        <div class="checkbox-inline">
                            <as:CheckBox ID="ckTimeZone" onclick="onAutoTimezoneCheck();" Text="Set Time Zone Automatically" meta:resourcekey="uxAutoTimeZoneResource1" runat="server" />
                        </div>
                    </td>
                    <td>
                        <tek:RadComboBox runat="server" ID="drSysTimeZone" AutoPostBack="false" DataTextField="TimeZoneName" DataValueField="TimeZoneID" Width="70%"></tek:RadComboBox>
                    </td>
                </tr>

                <tr runat="server" id="uxPnlEnvironmentIndicator">
                    <td class="heading w-30">
                        <label class="control-label">
                            <as:Literal ID="Literal2" runat="server" Text="Display Environment Indicator:" meta:resourcekey="DisplayEnvironmentIndicatorLiteralResource" /></label>
                    </td>
                    <td>
                        <asp:RadioButtonList ID="uxRadioButtonListDisplayEnvironmentIndicator" runat="server" RepeatDirection="Horizontal" CssClass="noBorderTable w-min-180 table-no-border">
                            <asp:ListItem Value="Yes" Text="Yes" meta:resourcekey="DisplayEnvironmentIndicatorLiteralResourceYes" />
                            <asp:ListItem Value="No" Text="No" Selected="True" meta:resourcekey="DisplayEnvironmentIndicatorLiteralResourceNo" />
                        </asp:RadioButtonList>
                    </td>
                </tr>
                <%--39251 – VW - Add Default Landing Page On Update My Profile Page--%>
                <tr>
                    <td class="heading w-30">
                        <label class="control-label">
                            <as:Literal ID="ltDefaultLandingPage" runat="server" Text="Default Landing Page:" meta:resourcekey="DefaultLandingPageLiteralResource1" /></label>
                    </td>
                    <td class="text-left">
                        <tek:RadDropDownTree runat="server" ID="uxDefaultLandingPage" OnClientDropDownClosed="DefaultLandingPageOnClientDropDownClosed"
                            DropDownSettings-CssClass="dropdown-tree" EnableScreenBoundaryDetection="true" EnableDirectionDetection="true" OnNodeDataBound="uxDefaultLandingPage_NodeDataBound" Width="70%"
                            DefaultMessage="Choose a destination" DefaultValue="0" DropDownSettings-CloseDropDownOnSelection="true" meta:resourcekey="uxDefaultLandingPage_DefaultMessage">
                        </tek:RadDropDownTree>

                        <div class="hide">
                            <asp:HiddenField ID="uxDefaultLandingPageSelectedValue" runat="server" />
                        </div>
                    </td>
                </tr>
                <uc:UserProfileMasterUserControl runat="server" ID="uxUserProfileMasterUserControl" />
            </table>

            <div class="row">
                <div class="col-md-12 form-action-container text-right">
                    <as:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="btn btn-default"
                        OnClick="btnSubmit_Click"
                        OnClientClick="return ValidateUpdateProfile();" meta:resourcekey="btnSubmitResource1" />
                </div>
            </div>

        </div>
    </div>

     <tek:RadAjaxManagerProxy runat="server" ID="uxRadAjaxManager">
        <AjaxSettings>
            <tek:AjaxSetting AjaxControlID="uxFlagLoginLink">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="uxFlagLoginLink" />
                </UpdatedControls>
            </tek:AjaxSetting>

            
            <tek:AjaxSetting AjaxControlID="uxLogoutButton">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="uxLogoutButton" />
                </UpdatedControls>
            </tek:AjaxSetting>
        </AjaxSettings>
    </tek:RadAjaxManagerProxy>
</as:Container>
<as:Validator ID="Validator1" runat="server" ValidationFunction="ValidateInput" MessageType="Inline" MessageContainerClientID="" meta:resourcekey="Validator1Resource1">
    <Items>
        <as:BasicValidationItem ControlToValidateID="uxFirstName" Rule="Required" ResMessage="Resources.UserMaintenanceMessage.Required" />
        <as:BasicValidationItem ControlToValidateID="uxLastName" Rule="Required" ResMessage="Resources.UserMaintenanceMessage.Required" />
        <as:BasicValidationItem ControlToValidateID="uxEmail" Rule="Required" ResMessage="Resources.UserMaintenanceMessage.Required" />
        <as:BasicValidationItem ControlToValidateID="uxFirstName" Rule="Minlength" MinLength="2"
            Message="You must enter at least 2 characters." meta:resourcekey="BasicValidationItem1Resource1" />
        <as:BasicValidationItem ControlToValidateID="uxLastName" Rule="Minlength" MinLength="2"
            Message="You must enter at least 2 characters." meta:resourcekey="BasicValidationItem1Resource1" />
        <as:CustomValidationItem ClientValidationFunction="validateEmail" ControlToValidateID="uxEmail" Message="You have entered an incorrect email address format.  Please try again." meta:resourcekey="UserProfile_ascx_cs_MSG_InvalidEmailFormat" />
         <as:CustomValidationItem ClientValidationFunction="validateEmailContact" ControlToValidateID="uxEmailContact" Message="You have entered an incorrect email address format.  Please try again." meta:resourcekey="UserProfile_ascx_cs_MSG_InvalidEmailFormat" />
        <as:CustomValidationItem ClientValidationFunction="validatePhone" ControlToValidateID="uxPhone" ResMessage="Resources.ValMsg.ManageProfile_InvalidFormat" ResParams="Phone" />
        <as:BasicValidationItem ControlToValidateID="uxNewUserName" Rule="Required" Message="This field is required and must be unique." meta:resourcekey="ManageUserASCX_Text_RequiredAndUnique" />
        <as:BasicValidationItem ControlToValidateID="uxNewUserName" Rule="Minlength" MinLength="7"
            Message="You must enter at least 7 characters." meta:resourcekey="ManageUserASCX_Text_AtLeast7Chars" />        
        <as:BasicValidationItem ControlToValidateID ="uxNewUserName" Rule="StringAccept" Pattern="0123456789qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM.-_" Message="Invalid user name" meta:resourcekey="ManageUserASCX_Text_InvalidUsername" />
    </Items>
</as:Validator>

<as:Validator ID="uxValidatorPasswordRule" runat="server" ValidationFunction="doValidationUserPass"
    MessageType="Inline" MessageContainerClientID="" meta:resourcekey="uxValidatorPasswordRuleResource1">
    <Items>
        <as:BasicValidationItem ControlToValidateID="uxOldPass" Rule="Required" ResMessage="Resources.UserMaintenanceMessage.Required" />        
        <as:BasicValidationItem ControlToValidateID="uxOldPass" Rule="Maxlength" MaxLength="50"
            ResMessage="Resources.UserMaintenanceMessage.MaxLength" ResParams="50" />
        
        <as:CustomValidationItem ControlToValidateID="uxPassword" ClientValidationFunction="CheckValidPassword" Message=" " />

        <as:BasicValidationItem ControlToValidateID="uxConfirmPass" Rule="Maxlength" MaxLength="50"
            ResMessage="Resources.UserMaintenanceMessage.MaxLength" ResParams="50" />
        <as:CompareValidationItem ControlToValidateID="uxConfirmPass" ControlToCompare="uxPassword"
            ResMessage="Resources.UserMaintenanceMessage.ManageProfile_ConfirmPassNotMatch" />
    </Items>
</as:Validator>
<as:Validator ID="Validator3" runat="server" ValidationFunction="doValidationUserQuestion"
    MessageType="Inline" MessageContainerClientID="" meta:resourcekey="Validator3Resource1">
    <Items>
        <as:BasicValidationItem ControlToValidateID="uxAnswer" Rule="Required" ResMessage="Resources.UserMaintenanceMessage.Required" />
        <as:BasicValidationItem ControlToValidateID="uxAnswer" Rule="Maxlength" MaxLength="50"
            ResMessage="Resources.UserMaintenanceMessage.MaxLength" ResParams="50" />
    </Items>
</as:Validator>

<as:Validator ID="ValidatorNewUserName" runat="server" ValidationFunction="validationNewUserName" MessageType="Inline" MessageContainerClientID="" meta:resourcekey="Validator3Resource1">
    <Items>
        <as:RegExValidationItem ControlToValidateID="uxNewUserName" RegularExpression="^((?!(<[^ \t]))(?!(&#)).)*$" 
            ResMessage="Resources.LanguageResource.AS_CommonJS_Msg_IncludedSpeciaCharacters" />
        <as:BasicValidationItem ControlToValidateID="uxNewUserName" Rule="Minlength" MinLength="7" 
            ResMessage="Resources.ValMsg.At_Least_7_Chars" />
    </Items>
</as:Validator>
<as:Validator ID="ValidatorFirstName" runat="server" ValidationFunction="validationFirstName" MessageType="Inline" MessageContainerClientID="" meta:resourcekey="Validator3Resource1">
    <Items>
        <as:RegExValidationItem ControlToValidateID="uxFirstName" RegularExpression="^((?!(<[^ \t]))(?!(&#)).)*$" ResMessage="Resources.LanguageResource.AS_CommonJS_Msg_IncludedSpeciaCharacters" />
    </Items>
</as:Validator>
<as:Validator ID="ValidatorLastName" runat="server" ValidationFunction="validationLastName" MessageType="Inline" MessageContainerClientID="" meta:resourcekey="Validator3Resource1">
    <Items>
        <as:RegExValidationItem ControlToValidateID="uxLastName" RegularExpression="^((?!(<[^ \t]))(?!(&#)).)*$" ResMessage="Resources.LanguageResource.AS_CommonJS_Msg_IncludedSpeciaCharacters" />
    </Items>
</as:Validator>
<as:Validator ID="ValidatorEmail" runat="server" ValidationFunction="validationEmail" MessageType="Inline" MessageContainerClientID="" meta:resourcekey="Validator3Resource1">
    <Items>
        <as:RegExValidationItem ControlToValidateID="uxEmail" RegularExpression="^((?!(<[^ \t]))(?!(&#)).)*$" ResMessage="Resources.ValMsg.InvalidEmailFormat" />
        <as:BasicValidationItem ControlToValidateID="uxEmail" Rule="Email" ResMessage="Resources.ValMsg.InvalidEmailFormat" />
    </Items>
</as:Validator>
<as:Validator ID="ValidatorEmailContact" runat="server" ValidationFunction="validationEmailContact" MessageType="Inline" MessageContainerClientID="" meta:resourcekey="Validator3Resource1">
    <Items>
        <as:RegExValidationItem ControlToValidateID="uxEmailContact" RegularExpression="^((?!(<[^ \t]))(?!(&#)).)*$" ResMessage="Resources.ValMsg.InvalidEmailFormat" />
        <as:BasicValidationItem ControlToValidateID="uxEmailContact" Rule="Email" ResMessage="Resources.ValMsg.InvalidEmailFormat" />
    </Items>
</as:Validator>
<as:Validator ID="ValidatorOldPass" runat="server" ValidationFunction="validationOldPass" MessageType="Inline" MessageContainerClientID="" meta:resourcekey="Validator3Resource1">
    <Items>
        <as:RegExValidationItem ControlToValidateID="uxOldPass" RegularExpression="^((?!(<[^ \t]))(?!(&#)).)*$" 
            ResMessage="Resources.LanguageResource.AS_CommonJS_Msg_IncludedSpeciaCharacters" />       
    </Items>
</as:Validator>
<as:Validator ID="ValidatorPassword" runat="server" ValidationFunction="validationPassword" MessageType="Inline" MessageContainerClientID="" meta:resourcekey="Validator3Resource1">
    <Items>
        <as:CustomValidationItem ControlToValidateID="uxPassword" ClientValidationFunction="CheckValidPasswordOnBlur" Message=" " />
    </Items>
</as:Validator>
<as:Validator ID="ValidatorConfirmPass" runat="server" ValidationFunction="validationConfirmPass" MessageType="Inline" MessageContainerClientID="" meta:resourcekey="Validator3Resource1">
    <Items>
        <as:RegExValidationItem ControlToValidateID="uxConfirmPass" RegularExpression="^((?!(<[^ \t]))(?!(&#)).)*$" 
            ResMessage="Resources.UserMaintenanceMessage.ManageProfile_ConfirmPassNotMatch" />
    </Items>
</as:Validator>
<as:Validator ID="ValidatorAnswer" runat="server" ValidationFunction="validationAnswer" MessageType="Inline" MessageContainerClientID="" meta:resourcekey="Validator3Resource1">
    <Items>
        <as:RegExValidationItem ControlToValidateID="uxAnswer" RegularExpression="^((?!(<[^ \t]))(?!(&#)).)*$" ResMessage="Resources.LanguageResource.AS_CommonJS_Msg_IncludedSpeciaCharacters" />
    </Items>
</as:Validator>


<as:HiddenField ID="uxHddChkPass" runat="server" Value="false" />
<as:HiddenField ID="uxHddChkQuestion" runat="server" Value="false" />
<as:HiddenField ID="uxFlagLoginLink" runat="server" Value="0" />
<tek:RadButton ID="uxLogoutButton" CssClass="hide" runat="server" OnClick="uxLogoutButton_Click"></tek:RadButton>
<as:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <script type="text/javascript" src="<%= ResolveUrl("~/") %>res/js/common/PasswordValidation.js"></script>
    <script type="text/javascript" src="<%= ResolveUrl("~/")%>res/js/jquery/jquery.mask.min.js"></script>
    <script type="text/javascript">
        var isGRP_TOTAL = "<%=IsGRP_TOTAL%>";
        var uxCheckChangePass_ClientID = '<%=uxCheckChangePass.ClientID %>';
        var uxPassword_ClientID = '<%=uxPassword.ClientID%>';
        var uxConfirmPass_ClientID = '<%=uxConfirmPass.ClientID%>';
        var uxOldPass_ClientID = '<%=uxOldPass.ClientID%>';
        var uxCheckChangeQuestion_ClientID = '<%= uxCheckChangeQuestion.ClientID %>';
        var uxAnswer_ClientID = '<%=uxAnswer.ClientID%>';
        var uxListValidQuestion_ClientID = '<%=uxListValidQuestion.ClientID%>';
        var uxHddChkPass_ClientID = '<%=uxHddChkPass.ClientID%>';
        var uxHddChkQuestion_ClientID = '<%=uxHddChkQuestion.ClientID%>';
        var uxPassword_ClientID = '<%= uxPassword.ClientID %>';
        var uxNewUserName_ClientID = '<%= uxNewUserName.ClientID %>';
        var uxOriginalUserName_ClientID = '<%= uxOriginalUserName.ClientID %>';
        var ckChangeUserName_ClientID = '<%= ckChangeUserName.ClientID %>';
        var lbNewUserName_ClientID = '<%= lbNewUserName.ClientID %>';
        var txtNewUserNameErrMsg_ClientID = '<%= txtNewUserNameErrMsg.ClientID %>';
        var uxPhone_ClientID = '<%=uxPhone.ClientID %>';
        var uxEmail_ClientID = '<%=uxEmail.ClientID%>';
        var uxEmailContactID = '<%= uxEmailContact.ClientID%>';
        var drSysTimeZone = '<%=drSysTimeZone.ClientID%>';
        var ckTimeZone = '<%=ckTimeZone.ClientID%>';
        var uxDefaultLandingPage_ClientID = '<%=uxDefaultLandingPage.ClientID %>';
        var uxDefaultLandingPageSelectedValue_ClientID = '<%= uxDefaultLandingPageSelectedValue.ClientID%>';
        var uxFlagLoginLink_ID = '<%= uxFlagLoginLink.ClientID%>';
        var uxLogoutButton_ID = '<%= uxLogoutButton.ClientID%>';
        var uxFirstName_ClientID = '<%= uxFirstName.ClientID%>';
        var uxLastName_ClientID = '<%= uxLastName.ClientID%>';

        var UserNameMaxLengthForUserProfile = <%= GetUserNameMaxLengthForUserProfile() %>;

        var MinPasswordLen = <%= GetPwdValidationRuleToValue(AS.Web.Business.Shared.Constants.UserProfileConstants.KEY_MIN_PASSWORD_LENGTH) %>;

        var MaxPasswordLen = <%= GetPwdValidationRuleToValue(AS.Web.Business.Shared.Constants.UserProfileConstants.KEY_MAX_PASSWORD_LENGTH) %>;

        var NumberOfUpperCaseCharacters = <%= string.IsNullOrEmpty(((Validation)WebSiteSettings.PwdValidationRule.Get("NumberOfUpperCaseCharacters")).Key) ? 0: ((Validation)WebSiteSettings.PwdValidationRule.Get("NumberOfUpperCaseCharacters")).Value %>;

        var NumberOfLowerCaseCharacters = <%= string.IsNullOrEmpty(((Validation)WebSiteSettings.PwdValidationRule.Get("NumberOfLowerCaseCharacters")).Key) ? 0: ((Validation)WebSiteSettings.PwdValidationRule.Get("NumberOfLowerCaseCharacters")).Value %>;

        var NumberOfNumberic = <%= string.IsNullOrEmpty(((Validation)WebSiteSettings.PwdValidationRule.Get("NumberOfNumberic")).Key) ? 0: ((Validation)WebSiteSettings.PwdValidationRule.Get("NumberOfNumberic")).Value %>;

        var IsRequiredSpecialCharacters = <%= string.IsNullOrEmpty(((Validation)WebSiteSettings.PwdValidationRule.Get("NumberOfSpecialCharacters")).Key) ? "false": "true"  %>;

        var NumberOfSpecialCharacters = <%= string.IsNullOrEmpty(((Validation)WebSiteSettings.PwdValidationRule.Get("NumberOfSpecialCharacters")).Key) ? 0: ((Validation)WebSiteSettings.PwdValidationRule.Get("NumberOfSpecialCharacters")).Value %>;

        var errorPassword = '<%= errorPassword.ClientID%>';

         var MinPasswordMsg = '<%= GetPwdValidationRuleToMessage(AS.Web.Business.Shared.Constants.UserProfileConstants.KEY_MIN_PASSWORD_LENGTH) %>';

        var MaxPasswordMsg = '<%= GetPwdValidationRuleToMessage(AS.Web.Business.Shared.Constants.UserProfileConstants.KEY_MAX_PASSWORD_LENGTH)  %>';

        var NumberOfUpperCaseCharactersMsg = '<%= string.IsNullOrEmpty(((Validation)WebSiteSettings.PwdValidationRule.Get("NumberOfUpperCaseCharacters")).Key) ? GetLocalResourceObject("UserProfile_ascx_cs_IncorrectPasswordFormat").ToString() : ((Validation)WebSiteSettings.PwdValidationRule.Get("NumberOfUpperCaseCharacters")).Msg %>';

        var NumberOfLowerCaseCharactersMsg = '<%= string.IsNullOrEmpty(((Validation)WebSiteSettings.PwdValidationRule.Get("NumberOfLowerCaseCharacters")).Key) ? GetLocalResourceObject("UserProfile_ascx_cs_IncorrectPasswordFormat").ToString() : ((Validation)WebSiteSettings.PwdValidationRule.Get("NumberOfLowerCaseCharacters")).Msg %>';

        var NumberOfNumbericMsg = '<%= string.IsNullOrEmpty(((Validation)WebSiteSettings.PwdValidationRule.Get("NumberOfNumberic")).Key) ? GetLocalResourceObject("UserProfile_ascx_cs_IncorrectPasswordFormat").ToString() : ((Validation)WebSiteSettings.PwdValidationRule.Get("NumberOfNumberic")).Msg %>';

        var msgValidateHTMTag = '<%=GetLocalResourceObject("msgValidateHTMTag").ToString() %>';
        var msgValidateEncodeTag = '<%=GetLocalResourceObject("msgValidateEncodeTag").ToString() %>';


    </script>
    <script src="<%= ResolveUrl("~/")%>res/js/usermaintenance/UserProfile.js"></script>
</as:RadCodeBlock>
