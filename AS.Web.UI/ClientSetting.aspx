<%@ Page Title="Client Setting" Language="C#" MasterPageFile="~/MasterPagePopup.master" AutoEventWireup="true"
    CodeFile="ClientSetting.aspx.cs" Inherits="ClientSetting" meta:resourcekey="PageResource1" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="Server">
    <as:ASModalContainer ID="uxModalContainer" runat="server" WidthCssClass="modal-lg" ContainerCssClass="container" Width="">
        <div class="row">
            <div class="col-md-12">
                <h3 class="modal-title">
                    <asp:Literal ID="lblHeader" runat="server" Text="Client Setting" meta:resourcekey="ClientSettingTitle"></asp:Literal>
                </h3>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12">
                <table class="ASTable form-inline">
                    <tbody>
                        <tr class="AltRow">
                            <td class="heading valign-middle" style="width: 30%;">
                                <as:Literal ID="ltClientName" runat="server" Text="Client Name:" meta:resourcekey="ltClientNameResource1"></as:Literal>
                            </td>
                            <td>
                                <as:TextBox ID="uxClientName" runat="server" Width="100%" meta:resourcekey="uxClientNameResource1"></as:TextBox>
                            </td>
                        </tr>
                        <tr class="Row">
                            <td class="heading valign-middle">
                                <as:Literal ID="ltClientDescription" runat="server" Text="Client Description:" meta:resourcekey="ltClientDescriptionResource1"></as:Literal>
                            </td>
                            <td>
                                <as:TextBox ID="uxDescription" runat="server" MaxLength="200" Width="100%" meta:resourcekey="uxDescriptionResource1"></as:TextBox>
                            </td>
                        </tr>
                        <tr class="AltRow">
                            <td class="heading valign-middle">
                                <as:Literal ID="ltClientAddress1" runat="server" Text="Client Address 1:" meta:resourcekey="ltClientAddress1Resource1"></as:Literal>
                            </td>
                            <td>
                                <as:TextBox ID="uxClientAdd1" runat="server" MaxLength="50" Width="100%" meta:resourcekey="uxClientAdd1Resource1"></as:TextBox>
                            </td>
                        </tr>
                        <tr class="Row">
                            <td class="heading valign-middle">
                                <as:Literal ID="ltClientAddress2" runat="server" Text="Client Address 2:" meta:resourcekey="ltClientAddress2Resource1"></as:Literal>
                            </td>
                            <td>
                                <as:TextBox ID="uxClientAdd2" runat="server" MaxLength="50" Width="100%" meta:resourcekey="uxClientAdd2Resource1"></as:TextBox>
                            </td>
                        </tr>
                        <tr class="AltRow">
                            <td class="heading valign-middle">
                                <as:Literal ID="ltClientCode" runat="server" Text="Client City, State, Zip Code:" meta:resourcekey="ltClientCodeResource1"></as:Literal>
                            </td>
                            <td>
                                <as:TextBox ID="uxClientZip" runat="server" MaxLength="50" Width="100%" meta:resourcekey="uxClientZipResource1"></as:TextBox>
                            </td>
                        </tr>
                        <tr class="Row">
                            <td class="heading valign-middle">
                                <as:Literal ID="ltClientPhone" runat="server" Text="Client Phone:" meta:resourcekey="ltClientPhoneResource1"></as:Literal>
                            </td>
                            <td>
                                <as:TextBox ID="uxClientPhone" runat="server" MaxLength="20" Width="100%" meta:resourcekey="uxClientPhoneResource1"></as:TextBox>
                            </td>
                        </tr>
                        <tr class="AltRow">
                            <td class="heading valign-middle">
                                <as:Literal ID="ltClientFax" runat="server" Text="Client Fax:" meta:resourcekey="ltClientFaxResource1"></as:Literal>
                            </td>
                            <td>
                                <as:TextBox ID="uxClientFax" runat="server" MaxLength="20" Width="100%" meta:resourcekey="uxClientFaxResource1"></as:TextBox>
                            </td>
                        </tr>
                        <tr class="Row">
                            <td class="heading valign-middle">
                                <as:Literal ID="ltClientEmail" runat="server" Text="Client Email:" meta:resourcekey="ltClientEmailResource1"></as:Literal>
                            </td>
                            <td class="text-mail">
                                <as:TextBox ID="uxClientEmail" runat="server" MaxLength="50" Width="100%" meta:resourcekey="uxClientEmailResource1"></as:TextBox>
                            </td>
                        </tr>
                        <tr class="AltRow">
                            <td class="heading valign-middle">
                                <as:Literal ID="ltContactEmail" runat="server" Text="Contact Email From:" meta:resourcekey="ltContactEmailResource1"></as:Literal>
                            </td>
                            <td class="text-mail">
                                <as:TextBox ID="uxContactEmail" runat="server" MaxLength="50" Width="50%" meta:resourcekey="uxContactEmailResource1"></as:TextBox>
                                <div id="uxContactEmailSuffixPanel" runat="server">
                                    <as:Literal ID="uxContactEmailSuffix" runat="server" Text=""></as:Literal>
                                </div>
                            </td>
                        </tr>
                        <tr class="Row">
                            <td class="heading valign-middle">
                                <as:Literal ID="ltContactEmailTo" runat="server" Text="Contact Email To:" meta:resourcekey="ltContactEmailToResource1"></as:Literal>
                            </td>
                            <td class="text-mail">
                                <as:TextBox ID="uxContactEmailTo" runat="server" MaxLength="50" Width="100%" meta:resourcekey="uxContactEmailToResource1"></as:TextBox>
                            </td>
                        </tr>
                        <tr class="AltRow">
                            <td class="heading valign-middle">
                                <as:Literal ID="ltNoreplyEmail" runat="server" Text="No reply Email:" meta:resourcekey="ltNoreplyEmailResource1"></as:Literal>
                            </td>
                            <td class="text-mail">
                                <as:TextBox ID="uxNoReply" runat="server" MaxLength="50" Width="50%" meta:resourcekey="uxNoReplyResource1"></as:TextBox>
                                <div id="uxNoReplySuffixPanel" runat="server">
                                    <as:Literal ID="uxNoReplySuffix" runat="server" Text=""></as:Literal>
                                </div>
                            </td>
                        </tr>
                        <tr class="Row">
                            <td class="heading valign-middle">
                                <as:Literal ID="ltProductName" runat="server" Text="Product Name:" meta:resourcekey="ltProductNameResource1"></as:Literal>
                            </td>
                            <td>
                                <as:TextBox ID="uxProductName" runat="server" MaxLength="20" Width="100%" meta:resourcekey="uxProductNameResource1" ></as:TextBox>
                            </td>
                        </tr>
                        <tr class="AltRow">
                            <td class="heading valign-middle">
                                <as:Literal ID="ltRestrictionPassword" runat="server" Text="Restriction on Previous Passwords Use:" meta:resourcekey="ltRestrictionPasswordResource1"></as:Literal>
                            </td>
                            <td>
                                <as:TextBox ID="uxRestrictionPassword" runat="server" MaxLength="2" Width="100%" meta:resourcekey="uxRestrictionPasswordResource1"></as:TextBox>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12 form-action-container text-right">
                <as:Button class="btn btn-default" ID="btnSave" runat="server" Text="Submit" OnClick="SaveClientInfo" OnClientClick="return validInputs();" IsStandardButton="False" meta:resourcekey="btnSaveResource1" />
                <as:Button class="btn btn-default" ID="btnCancel" runat="server" Text="Cancel" OnClientClick="ClosePopupModal();" IsStandardButton="False" meta:resourcekey="btnCancelResource1" />
            </div>
        </div>
    </as:ASModalContainer>
    <as:Validator ID="uxValidator" runat="server" ValidationFunction="ValidateVariables" MessageContainerClientID="" MessageType="Inline" meta:resourcekey="uxValidatorResource1">
        <Items>
            <as:BasicValidationItem Rule="Required" ControlToValidateID="uxClientName" Message="Client Name: This is a required field." meta:resourcekey="ClientSetting_aspx_BasicValidator_RequiredField" />
            <as:BasicValidationItem Rule="StringUnaccept" Pattern="<>#&" ControlToValidateID="uxClientName"
                Message="Special characters like &ldquo;<&rdquo;, &ldquo;>&rdquo; , &ldquo;#&rdquo;, or &ldquo;&&rdquo; are not allowed in Client Name." meta:resourcekey="ClientSetting_aspx_BasicValidator_StringUnaccept" />
            <as:BasicValidationItem Rule="StringUnaccept" Pattern="<>#&" ControlToValidateID="uxClientAdd1"
                Message="Special characters like &ldquo;<&rdquo;, &ldquo;>&rdquo; , &ldquo;#&rdquo;, or &ldquo;&&rdquo; are not allowed in Client Address 1." meta:resourcekey="ClientSetting_aspx_BasicValidator_StringUnaccept1" />
            <as:BasicValidationItem Rule="StringUnaccept" Pattern="<>#&" ControlToValidateID="uxClientAdd2"
                Message="Special characters like &ldquo;<&rdquo;, &ldquo;>&rdquo; , &ldquo;#&rdquo;, or &ldquo;&&rdquo; are not allowed in Client Address 2." meta:resourcekey="ClientSetting_aspx_BasicValidator_StringUnaccept2" />
            <as:BasicValidationItem Rule="StringUnaccept" Pattern="<>#&" ControlToValidateID="uxClientZip"
                Message="Special characters like &ldquo;<&rdquo;, &ldquo;>&rdquo; , &ldquo;#&rdquo;, or &ldquo;&&rdquo; are not allowed in Client Zip code." meta:resourcekey="ClientSetting_aspx_BasicValidator_StringUnaccept3" />
            <as:BasicValidationItem Rule="StringAccept" Pattern="0123456789()- " ControlToValidateID="uxClientPhone"
                Message="Only numbers are allowed in Client Phone Number." meta:resourcekey="ClientSetting_aspx_BasicValidator_StringAccept1" />

            <as:BasicValidationItem Rule="StringAccept" Pattern="0123456789()- " ControlToValidateID="uxClientFax"
                Message="Only numbers are allowed in Client Fax Number." meta:resourcekey="ClientSetting_aspx_BasicValidator_StringAccept2" />

            <as:BasicValidationItem ControlToValidateID="uxClientEmail" Rule="Required" meta:resourcekey="ClientSetting_aspx_BasicValidator_RequireEmail" />
            <as:BasicValidationItem Rule="StringUnaccept" Pattern="<>#&" ControlToValidateID="uxClientEmail" meta:resourcekey="ClientSetting_aspx_BasicValidator_StringUnaccept4" />
            <as:BasicValidationItem ControlToValidateID="uxClientEmail" Rule="Email" Message="(*)" meta:resourcekey="ClientSetting_aspx_BasicValidator_InvalidEmail" />

            <as:BasicValidationItem ControlToValidateID="uxContactEmailTo" Rule="Required" meta:resourcekey="ClientSetting_aspx_BasicValidator_RequireEmail" />
            <as:BasicValidationItem Rule="StringUnaccept" Pattern="<>#&" ControlToValidateID="uxContactEmailTo" meta:resourcekey="ClientSetting_aspx_BasicValidator_StringUnaccept7" />
            <as:BasicValidationItem ControlToValidateID="uxContactEmailTo" Rule="Email" Message="(*)" meta:resourcekey="ClientSetting_aspx_BasicValidator_InvalidEmail" />

            <as:BasicValidationItem ControlToValidateID="uxContactEmail" Rule="Required" meta:resourcekey="ClientSetting_aspx_BasicValidator_RequireEmail" />
            <as:BasicValidationItem Rule="StringUnaccept" Pattern="<>#&@" ControlToValidateID="uxContactEmail"
                Message="(*)" meta:resourcekey="ClientSetting_aspx_BasicValidator_StringUnaccept5" />
            <as:BasicValidationItem Rule="Email" ControlToValidateID="uxContactEmail" Message="(*)" meta:resourcekey="ClientSetting_aspx_BasicValidator_InvalidEmail" />

            <as:BasicValidationItem ControlToValidateID="uxNoReply" Rule="Required" meta:resourcekey="ClientSetting_aspx_BasicValidator_RequireEmail" />
            <as:BasicValidationItem Rule="StringUnaccept" Pattern="<>#&@" ControlToValidateID="uxNoReply" Message="(*)" meta:resourcekey="ClientSetting_aspx_BasicValidator_StringUnaccept6" />
            <as:BasicValidationItem Rule="Email" ControlToValidateID="uxNoReply" Message="(*)" meta:resourcekey="ClientSetting_aspx_BasicValidator_InvalidEmail" />
            
            <as:BasicValidationItem Rule="Required" ControlToValidateID="uxProductName" Message="Product Name: This is a required field." meta:resourcekey="ClientSetting_aspx_ProductName_RequiredField" />
            <as:BasicValidationItem Rule="StringUnaccept" Pattern="<>#&" ControlToValidateID="uxProductName"
                Message="Special characters like &ldquo;<&rdquo;, &ldquo;>&rdquo; , &ldquo;#&rdquo;, or &ldquo;&&rdquo; are not allowed in Product Name." meta:resourcekey="ClientSetting_aspx_ProductName_StringUnaccept" />

            <as:BasicValidationItem Rule="Required" ControlToValidateID="uxRestrictionPassword" Message="This is a required field." meta:resourcekey="RestrictionPassword_aspx_BasicValidator_RequiredField" />
            <as:BasicValidationItem Rule="StringAccept" Pattern="0123456789()- " ControlToValidateID="uxRestrictionPassword"
                Message="Only 0 or positive numbers are allowed in this field." meta:resourcekey="RestrictionPassword_aspx_BasicValidator_StringAccept1" />
            <as:BasicValidationItem ControlToValidateID="uxRestrictionPassword" Rule="LessOrEqualThan" MaxValue="10" ResMessage="Resources.MessageManager.ValidationMessages_RestrictionPasswordLessOrEqualThanField" />
            <as:BasicValidationItem ControlToValidateID="uxRestrictionPassword" Rule="GreaterOrEqualThan" MinValue="0" ResMessage="Resources.MessageManager.ValidationMessages_RestrictionPasswordGreaterOrEqualThanField" />
        </Items>
    </as:Validator>
    <script type="text/javascript">
        function validInputs() {
            if (ValidateVariables(true)) {
                return true;
            }
            else {
                AdjustModalSize();
                return false;
            }
        }
    </script>
</asp:Content>

