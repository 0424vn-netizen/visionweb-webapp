<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ManageASUser.ascx.cs" Inherits="UserControls_ManageASUser" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<as:RadAjaxManagerProxy ID="RadAjaxManager" runat="server">
    <AjaxSettings>
        <tek:AjaxSetting AjaxControlID="uxSaveEditHide">
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="uxSaveEditHide" LoadingPanelID="uxloading" />
            </UpdatedControls>
        </tek:AjaxSetting>

    </AjaxSettings>
</as:RadAjaxManagerProxy>
<asp:Panel ID="uxloading" runat="server"></asp:Panel>
<asp:PlaceHolder ID="uxform" runat="server">
    <div class="row">
        <div class="col-md-12">
            <h3 class="modal-title">User Information</h3>

            <table class="ASTable form-inline">
                <tbody id="tblManageUserMain">
                    <tr class="Row">
                        <td class="heading" style="width: 20%;">
                            <as:ValidatorLabel ID="txtUsernameLabel" runat="server" Text="User Name:" ApplyFor="uxUsername" CssClass="control-label" meta:resourcekey="ManageASUserASCX_Text_UserName" />
                        </td>
                        <td>
                            <as:TextBox ID="uxUsernameOld" runat="server" Width="50%" Enabled="False" HintCss="hint" meta:resourcekey="uxUsernameOldResource1" autocomplete="off" autocorrect="off" />
                            <asp:PlaceHolder ID="uxCreateMode" runat="server">
                                <as:TextBox ID="uxUsername" runat="server" Width="50%" MaxLength="10" meta:resourcekey="uxUsernameResource1" />
                            </asp:PlaceHolder>
                            <as:ValidatorMessage runat="server" ID="txtUserNameErrMsg" ApplyFor="uxUsername" Message="" ShowOnLoad="False" />
                        </td>
                    </tr>
                    <tr class="AltRow">
                        <td class="heading">
                            <as:ValidatorLabel ID="ValidatorLabel1" runat="server" Text="First Name:" ApplyFor="uxFirstName" CssClass="control-label" meta:resourcekey="ManageASUserASCX_Text_FirstName" />

                        </td>
                        <td>
                            <as:TextBox ID="uxFirstName" runat="server" Width="50%" MaxLength="30" HintCss="hint" meta:resourcekey="uxFirstNameResource1" />
                            <as:ValidatorMessage runat="server" ID="ValidatorMessage1" ApplyFor="uxFirstName" Message="" ShowOnLoad="False" />
                        </td>
                    </tr>
                    <tr class="Row">
                        <td class="heading">
                            <as:ValidatorLabel ID="ValidatorLabel2" runat="server" Text="Last Name:" ApplyFor="uxLastName" CssClass="control-label" meta:resourcekey="ManageASUserASCX_Text_LastName" />
                        </td>
                        <td>
                            <as:TextBox ID="uxLastName" runat="server" Width="50%" MaxLength="30" HintCss="hint" meta:resourcekey="uxLastNameResource1" />
                            <as:ValidatorMessage runat="server" ID="ValidatorMessage2" ApplyFor="uxLastName" Message="" ShowOnLoad="False" />
                        </td>
                    </tr>
                    <tr class="AltRow">
                        <td class="heading">
                            <as:ValidatorLabel ID="ValidatorLabel3" runat="server" Text="Email:" ApplyFor="uxEmail" CssClass="control-label" meta:resourcekey="ManageASUserASCX_Text_Email" />
                        </td>
                        <td>
                            <as:TextBox ID="uxEmail" runat="server" Width="50%" MaxLength="100" HintCss="hint" meta:resourcekey="uxEmailResource1" />
                            <as:ValidatorMessage runat="server" ID="ValidatorMessage3" ApplyFor="uxEmail" Message="" ShowOnLoad="False" />
                        </td>
                    </tr>
                    <tr class="Row">
                        <td class="heading">
                            <as:ValidatorLabel ID="ValidatorLabel4" runat="server" Text="Role:" ApplyFor="uxRole" CssClass="control-label" meta:resourcekey="ManageASUserASCX_Text_Role" />
                        </td>
                        <td>
                            <as:Literal ID="uxRole" runat="server" Text="System Administrator" meta:resourcekey="uxEmailResource1" />
                            <as:ValidatorMessage runat="server" ID="ValidatorMessage4" ApplyFor="uxRole" Message="" ShowOnLoad="False" />
                        </td>
                    </tr>
                    <tr class="AltRow">
                        <td class="heading">
                            <asp:Literal ID="Literal1" runat="server" Text="Status:" meta:resourcekey="LiteralResource1" />
                        </td>
                        <td>
                            <as:RadioButton ID="uxActive" runat="server" Text="Active" GroupName="Active" Checked="True" Enabled="false"
                                onclick="AllowToChangeStatus('Active');" meta:resourcekey="uxActiveResource1" Value="" />
                            <as:RadioButton ID="uxInactive" runat="server" Text="Inactive" GroupName="Active" Enabled="false"
                                onclick="AllowToChangeStatus('Inactive');" meta:resourcekey="uxInactiveResource1" Value="" />
                        </td>
                    </tr>
            </table>

        </div>
    </div>
    <div class="height-22"></div>
    <div class="row">
        <div class="col-md-6">
            <h3 class="modal-title">Access Functions </h3>
        </div>
        <div class="col-xs-6 text-right">
            <a class="link-back control-inline" href="#" onclick="checkOrClearAllCheckboxAccessFunction('AccessFunctionPermissions', true); return false;">
                <asp:Literal ID="Literal7" runat="server" Text="Select all" meta:resourcekey="SelectallResource" /></a>
            <a class="link-back" href="#" onclick="checkOrClearAllCheckboxAccessFunction('AccessFunctionPermissions', false); return false;">
                <asp:Literal ID="Literal2" runat="server" Text="Clear all" meta:resourcekey="ClearallResource" /></a>
        </div>
        <div class="col-xs-12">
            <div class="box" id="AccessFunctionPermissions">
                <as:CheckBoxList ID="uxAccessFunctionPermissions" CssClass="checkbox-list checkbox-list-span" runat="server" AutoPostBack="false"
                    DataTextField="Description" DataValueField="PermissionId" meta:resourcekey="uxAccessListFunctionResource1">
                </as:CheckBoxList>
            </div>
        </div>
    </div>

    <div class="row">
        <div id="pnlbuttons" class="col-md-12 action-container text-right" runat="server">
            <as:Button ID="uxSave" CssClass="btn btn-default" runat="server" Text="Submit"
                ValidationGroup="ValidatePage" OnClick="uxSave_Click" OnClientClick="return ValidateInputCreate();" IsStandardButton="False" meta:resourcekey="uxSaveResource1" />
            <as:Button ID="uxSaveEdit" CssClass="btn btn-default" runat="server" Text="Submit"
                ValidationGroup="ValidatePage" OnClientClick="return ValidateInputEdit();" IsStandardButton="False" meta:resourcekey="uxSaveResource1" />

            <as:LinkButton ID="uxCancel" CssClass="btn btn-default" Text="Cancel" runat="server" PostBackUrl="~/ManageASUsers.aspx" IsStandardButton="False" meta:resourcekey="uxCancelResource1" />
        </div>
    </div>
    <as:Button ID="uxSaveEditHide" CssClass="btn btn-default hide" OnClick="uxSave_Click" runat="server" Text="Submit"
        ValidationGroup="ValidatePage" IsStandardButton="False" meta:resourcekey="uxSaveResource1" />

    <asp:HyperLink ID="uxLinktoUserList" runat="server" CssClass="hide" NavigateUrl="~/ManageASUsers.aspx" />
</asp:PlaceHolder>
<as:Validator ID="Validator1" runat="server" ValidationFunction="ValidateInput" MessageType="Inline" MessageContainerClientID="" meta:resourcekey="ValidatorResource1">
    <Items>
        <as:BasicValidationItem ControlToValidateID="uxUsername" Rule="Required" Message="This field is required and must be unique." meta:resourcekey="ManageASUserASCX_Text_RequiredAndUnique" />
        <as:BasicValidationItem ControlToValidateID="uxFirstName" Rule="Required" Message="This is a required field." meta:resourcekey="ManageASUserASCX_Text_Required" />
        <as:BasicValidationItem ControlToValidateID="uxLastName" Rule="Required" Message="This is a required field." meta:resourcekey="ManageASUserASCX_Text_Required" />
        <as:BasicValidationItem ControlToValidateID="uxEmail" Rule="Required" Message="This is a required field." meta:resourcekey="ManageASUserASCX_Text_Required" />
        <as:BasicValidationItem ControlToValidateID="uxUsername" Rule="Minlength" MinLength="7"
            Message="You must enter at least 7 characters." meta:resourcekey="ManageASUserASCX_Text_AtLeast7Chars" />
        <as:BasicValidationItem ControlToValidateID="uxUsername" Rule="AlphaNumeric" Message="Invalid user name" meta:resourcekey="ManageASUserASCX_Text_InvalidUsername" />
        <as:BasicValidationItem ControlToValidateID="uxFirstName" Rule="Minlength" MinLength="2"
            Message="You must enter at least 2 characters." meta:resourcekey="ManageASUserASCX_Text_AtLeast2Chars" />
        <as:BasicValidationItem ControlToValidateID="uxLastName" Rule="Minlength" MinLength="2"
            Message="You must enter at least 2 characters." meta:resourcekey="ManageASUserASCX_Text_AtLeast2Chars" />
        <as:BasicValidationItem ControlToValidateID="uxEmail" Rule="Minlength" MinLength="2"
            Message="You must enter at least 2 characters." meta:resourcekey="ManageASUserASCX_Text_AtLeast2Chars" />
        <as:BasicValidationItem ControlToValidateID="uxEmail" Rule="Email" Message="You have entered an incorrect email address format.  Please try again." meta:resourcekey="ManageASUserASCX_Text_IncorrectEmail" />
    </Items>
</as:Validator>
<as:Validator ID="Validator2" runat="server" ValidationFunction="ValidateUsername" MessageType="Inline" MessageContainerClientID="" meta:resourcekey="ValidatorResource1">
    <Items>
        <as:BasicValidationItem ControlToValidateID="uxUsername" Rule="Minlength" MinLength="7" Message="You must enter at least 7 characters." meta:resourcekey="ManageASUserASCX_Text_AtLeast7Chars" />
        <as:BasicValidationItem ControlToValidateID="uxUsername" Rule="AlphaNumeric" Message="Invalid user name" meta:resourcekey="ManageASUserASCX_Text_InvalidUsername" />
    </Items>
</as:Validator>
<as:Validator ID="Validator3" runat="server" ValidationFunction="ValidateFirstName" MessageType="Inline" MessageContainerClientID="" meta:resourcekey="ValidatorResource1">
    <Items>
        <as:RegExValidationItem ControlToValidateID="uxFirstName" RegularExpression="^((?!(<[^ \t]))(?!(&#)).)*$" ResMessage="Resources.LanguageResource.AS_CommonJS_Msg_IncludedSpeciaCharacters" />
    </Items>
</as:Validator>
<as:Validator ID="Validator4" runat="server" ValidationFunction="ValidateLastName" MessageType="Inline" MessageContainerClientID="" meta:resourcekey="ValidatorResource1">
    <Items>
        <as:RegExValidationItem ControlToValidateID="uxLastName" RegularExpression="^((?!(<[^ \t]))(?!(&#)).)*$" ResMessage="Resources.LanguageResource.AS_CommonJS_Msg_IncludedSpeciaCharacters" />
    </Items>
</as:Validator>
<as:Validator ID="Validator5" runat="server" ValidationFunction="ValidateEmail" MessageType="Inline" MessageContainerClientID="" meta:resourcekey="ValidatorResource1">
    <Items>
        <as:RegExValidationItem ControlToValidateID="uxEmail" RegularExpression="^((?!(<[^ \t]))(?!(&#)).)*$" meta:resourcekey="ManageASUserASCX_Text_IncorrectEmail" />
    </Items>
</as:Validator>

<tek:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <script type="text/javascript">
        var uxUsernameId = '<%= uxUsername.ClientID%>';
        var uxFirstNameId = '<%= uxFirstName.ClientID%>';
        var uxLastNameId = '<%= uxLastName.ClientID%>';
        var uxEmailId = '<%= uxEmail.ClientID%>';

        var ManageASUser_uxSaveEditHide = '<%= uxSaveEditHide.UniqueID%>';
        var ManageASUser_uxLinktoUserList = '<%= uxLinktoUserList.ClientID%>';
        var ManageASUser_confirm_url = '<%= "EditASUserConfirmModal.aspx?" + Page.BuildSecureQueryString("u=" + EditUserName + "&action=" + WebSiteEnums.ASUserAction.Edit) %>';

        function ValidateInputCreate() {
            if (isDisabledSubmitAdd) {
                return false;
            }
            if (ValidateInput()) {
                __doPostBack(ManageASUser_uxSaveEditHide, '');
            }
            return false;
        }

        function ValidateInputEdit() {
            if (isDisabledSubmitAdd) {
                return false;
            }
            if (ValidateInput()) {
                return ShowPopupModal(ManageASUser_confirm_url, 'auto');
            }
            return false;
        }

        function SaveChanged() {
            HidePopupModal();
            __doPostBack(ManageASUser_uxSaveEditHide, '');
        }


        function DoCloseCreatedUser() {
            window.location.href = '<%= ResolveUrl("~/")%>ManageASUsers.aspx';
            // HidePopupModal();
        }

    </script>
    <script src="<%= ResolveUrl("~/")%>res/js/usermaintenance/ManageASUser.js"></script>
</tek:RadCodeBlock>
