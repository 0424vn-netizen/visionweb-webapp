<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="ContactUs.aspx.cs" Inherits="As.VisionWeb.Web.ContactUs" EnableEventValidation="false" meta:resourcekey="PageResource1" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="Server">
    <uc:PageTitle ID="uxPageTitle" ReportTitle="Contact Us" runat="server" HasFilteringOption="false" meta:resourcekey="uxPageTitleResource1" />
    <div class="row">
        <div class="col-md-6" runat="server" id="uxContactInfo" >
            <h2 class="grid-title on-top" data-toggle="collapse" data-target="#uxContactUsGrid">
                <asp:Literal ID="uxCSHeader" runat="server" meta:resourcekey="uxCSHeaderResource1"></asp:Literal>
            </h2>
            <div class="in" id="uxContactUsGrid">
                <table class="ASTable">
                    <tr class="Row">
                        <td class="heading" style="width: 22%;">
                            <as:Literal ID="ltCustomerService" runat="server" Text="Customer Service:" meta:resourcekey="ltCustomerServiceResource1"></as:Literal></td>
                        <td>
                            <as:Literal runat="server" ID="uxCSPhoneNumber" Text="(800) 846-4472" meta:resourcekey="uxCSPhoneNumberResource1"></as:Literal>
                        </td>
                    </tr>
                    <tr class="AltRow" runat="server" id="trFax">
                        <td class="heading">
                            <as:Literal ID="ltFax" runat="server" Text="Fax:" meta:resourcekey="ltFaxResource1"></as:Literal></td>
                        <td>
                            <as:Literal runat="server" ID="uxCSFax" Text="(xxx) 247- x951" meta:resourcekey="uxCSFaxResource1"></as:Literal>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
    <asp:PlaceHolder ID="uxPlaceHolderMessage" runat="server">
        <table>
            <tr>
                <td style="padding-left: 380px; padding-right: 380px;" align="center" class="contentPart">
                    <asp:Literal ID="uxMessage" runat="server" meta:resourcekey="uxMessageResource1"></asp:Literal>
                </td>
            </tr>
        </table>
        <br />
    </asp:PlaceHolder>
    <div class="row">
        <div class="col-md-6">
            <h2 class="grid-title" id="uxContactTitle"  runat="server" data-toggle="collapse" data-target="#uxMailUsGrid">
                <as:Literal ID="ltContactUs" runat="server" Text="Contact Us" meta:resourcekey="ltContactUsResource1"></as:Literal></h2>
            <div class="in" id="uxMailUsGrid">
                <table class="ASTable">
                    <asp:PlaceHolder ID="phCustomContactUs" runat="server">
                        <tr class="AltRow">
                            <td class="heading" style="width: 22%;">
                                <as:ValidatorLabel ID="ValidatorLabel1" runat="server" ApplyFor="txtBusinessName" CssClass="control-label" meta:resourcekey="ltBusinessName" />
                            </td>
                            <td>
                                <as:TextBox ID="txtBusinessName" MaxLength="250" runat="server" CssClass="form-control" HintCss="hint" onblur="onBusinessNameBlur(this, event)"></as:TextBox>
                            </td>
                        </tr>
                        <tr class="Row" runat="server" id="trMIDorVT">
                            <td class="heading" style="width: 22%;">
                                <as:ValidatorLabel ID="ValidatorLabel2" runat="server" ApplyFor="txtMIDorVT" CssClass="control-label" meta:resourcekey="ltMIDorVT" />
                            </td>
                            <td>
                                <as:TextBox ID="txtMIDorVT" runat="server" MaxLength="250" CssClass="form-control" HintCss="hint" onblur="onMIDorVTBlur(this, event)"></as:TextBox>
                            </td>
                        </tr>
                        <tr class="AltRow">
                            <td class="heading" style="width: 22%;">
                                <as:ValidatorLabel ID="ValidatorLabel3" runat="server" ApplyFor="txtContactName" CssClass="control-label" meta:resourcekey="ltContactName" />
                            </td>
                            <td>
                                <as:TextBox ID="txtContactName" runat="server" MaxLength="250" CssClass="form-control" HintCss="hint" onblur="onContactNameBlur(this, event)"></as:TextBox>
                            </td>
                        </tr>
                        <tr class="Row">
                            <td class="heading" style="width: 22%;">
                                <as:ValidatorLabel ID="ValidatorLabel4" runat="server" ApplyFor="txtContactNumber" CssClass="control-label" meta:resourcekey="ltContactNumber" />
                            </td>
                            <td>
                                <as:TextBox ID="txtContactNumber" runat="server" MaxLength="15" display-masked='ContactNumber' autocomplete="off" CssClass="form-control" HintCss="hint" onblur="onContactNumberBlur(this, event)"></as:TextBox>
                            </td>
                        </tr>
                        <tr class="AltRow">
                            <td class="heading" style="width: 22%;">
                                <as:Literal ID="Literal2" runat="server" Text="To Mail:" meta:resourcekey="ltToMailResource1"></as:Literal>
                            </td>
                            <td>
                                <as:TextBox ID="TextBox1" runat="server" Text="Merchant Customer Service" CssClass="form-control" Enabled="False" HintCss="hint" meta:resourcekey="uxToResource1"></as:TextBox>
                            </td>
                        </tr>
                        <tr class="Row">
                            <td class="heading">
                                <as:ValidatorLabel ID="ValidatorLabel6" runat="server" ApplyFor="uxContactEmail" CssClass="control-label" meta:resourcekey="uxContactEmailLabel" />
                            </td>
                            <td>
                                <as:TextBox ID="uxContactEmail" CssClass="form-control" runat="server" MaxLength="250" HintCss="hint" onblur="onContactEmailBlur(this, event)"></as:TextBox>
                                <div class="bottom-error">
                                    <as:ValidatorMessage ID="ValidatorMessage2" runat="server" ApplyFor="uxContactEmail" Message="" ShowOnLoad="False"></as:ValidatorMessage>
                                </div>
                            </td>
                        </tr>
                        <tr class="AltRow">
                            <td class="heading">
                                <as:Literal ID="Literal1" runat="server" Text="Subject:" meta:resourcekey="ltSubjectResource1"></as:Literal>
                            </td>
                            <td>
                                <tek:RadComboBox ID="uxSubjectCustom" runat="server" Width="50%"></tek:RadComboBox>
                            </td>
                        </tr>
                        <tr class="Row">
                            <td class="heading">
                                <as:ValidatorLabel ID="ValidatorLabel5" runat="server" ApplyFor="uxDescOfIssue" CssClass="control-label" meta:resourcekey="uxDescForIssueLabel" />
                            </td>
                            <td>
                                <as:TextBox ID="uxDescOfIssue" runat="server" TextMode="MultiLine" Rows="7" MaxLength="2000"
                                    CssClass="form-control" HintCss="hint" onblur="onDescOfIssueBlur(this, event)"></as:TextBox>
                                <div class="bottom-error">
                                    <as:ValidatorMessage ID="ValidatorMessage1" runat="server" ApplyFor="uxDescOfIssue" Message="" ShowOnLoad="False"></as:ValidatorMessage>
                                </div>
                            </td>
                        </tr>
                    </asp:PlaceHolder>

                </table>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-md-6 action-container text-right">
            <as:Button runat="server" ID="uxSendEmail" Text="Send Email" OnClick="uxSendEmail_Click"
                OnClientClick="return ValidateContactUs();" CssClass="btn btn-default" IsStandardButton="False" meta:resourcekey="uxSendEmailResource1" />
            <as:Button runat="server" ID="uxClear" Text="Clear" OnClientClick="clearComment(); return false;" CssClass="btn btn-default" IsStandardButton="False" meta:resourcekey="uxClearResource1" />
        </div>
    </div>

    <as:Validator ID="validator1" runat="server" ValidationFunction="ValidateEmailPartContent" MessageType="Inline" MessageContainerClientID="" meta:resourcekey="validator1Resource1">
        <Items>
            <as:BasicValidationItem ControlToValidateID="txtBusinessName" Rule="Required" meta:resourcekey="ContactUs_aspx_Validator_RequiredBusinessName" />
            <as:BasicValidationItem ControlToValidateID="txtMIDorVT" Rule="Required" />
            <as:BasicValidationItem ControlToValidateID="txtContactName" Rule="Required" meta:resourcekey="ContactUs_aspx_Validator_RequiredContactName" />
            <as:BasicValidationItem ControlToValidateID="txtContactNumber" Rule="Required" meta:resourcekey="ContactUs_aspx_Validator_RequiredContactNumber" />
            <as:CustomValidationItem ClientValidationFunction="validateContactNumber" ControlToValidateID="txtContactNumber" meta:resourcekey="ContactUs_aspx_Validator_InvalidContactNumber" ResParams="ContactNumber" />
            <as:BasicValidationItem ControlToValidateID="uxDescOfIssue" Rule="Required" meta:resourcekey="ContactUs_aspx_Validator_RequiredDescofIssue" />
            <as:BasicValidationItem ControlToValidateID="uxDescOfIssue" Rule="Maxlength" MaxLength="2000" meta:resourcekey="Generic_CheckLengthOfDesc" />
            <as:BasicValidationItem ControlToValidateID="uxContactEmail" Rule="Required" meta:resourcekey="ContactUs_aspx_Validator_RequiredContactEmail" />
            <as:BasicValidationItem ControlToValidateID="uxContactEmail" Rule="Email" ResMessage="Resources.MessageManager.ContactUs_FromInvalidEmail" />
        </Items>
    </as:Validator>
    <as:Validator ID="validator2" runat="server" ValidationFunction="ValidateBody" MessageType="Inline" MessageContainerClientID="" meta:resourcekey="validator2Resource1">
        <Items>
            <as:BasicValidationItem ControlToValidateID="uxDescOfIssue" Rule="Maxlength" MaxLength="2000" meta:resourcekey="Generic_CheckLengthOfDesc" />
            <as:RegExValidationItem ControlToValidateID="uxDescOfIssue" RegularExpression="^((?!(<[^ \t]))(?!(&#)).)*$" ResMessage="Resources.LanguageResource.AS_CommonJS_Msg_IncludedSpeciaCharacters" />
        </Items>
    </as:Validator>
    <as:Validator ID="validator4" runat="server" ValidationFunction="ValidateBusinessName" MessageType="Inline" MessageContainerClientID="" meta:resourcekey="validator2Resource1">
        <Items>
            <as:RegExValidationItem ControlToValidateID="txtBusinessName" RegularExpression="^((?!(<[^ \t]))(?!(&#)).)*$" ResMessage="Resources.LanguageResource.AS_CommonJS_Msg_IncludedSpeciaCharacters" />
        </Items>
    </as:Validator>
    <as:Validator ID="validator5" runat="server" ValidationFunction="ValidateMIDorVT" MessageType="Inline" MessageContainerClientID="" meta:resourcekey="validator2Resource1">
        <Items>
            <as:RegExValidationItem ControlToValidateID="txtMIDorVT" RegularExpression="^((?!(<[^ \t]))(?!(&#)).)*$" ResMessage="Resources.LanguageResource.AS_CommonJS_Msg_IncludedSpeciaCharacters" />
        </Items>
    </as:Validator>
    <as:Validator ID="validator7" runat="server" ValidationFunction="ValidateContactName" MessageType="Inline" MessageContainerClientID="" meta:resourcekey="validator2Resource1">
        <Items>
            <as:RegExValidationItem ControlToValidateID="txtContactName" RegularExpression="^((?!(<[^ \t]))(?!(&#)).)*$" ResMessage="Resources.LanguageResource.AS_CommonJS_Msg_IncludedSpeciaCharacters" />
        </Items>
    </as:Validator>
    <as:Validator ID="validator8" runat="server" ValidationFunction="ValidateContactNumber2" MessageType="Inline" MessageContainerClientID="" meta:resourcekey="validator2Resource1">
        <Items>
            <as:RegExValidationItem ControlToValidateID="txtContactNumber" RegularExpression="^((?!(<[^ \t]))(?!(&#)).)*$" meta:resourcekey="ContactUs_aspx_Validator_InvalidContactNumber" ResParams="ContactNumber" />
        </Items>
    </as:Validator>
    <as:Validator ID="validator9" runat="server" ValidationFunction="ValidateContactEmail" MessageType="Inline" MessageContainerClientID="" meta:resourcekey="validator2Resource1">
        <Items>
            <as:RegExValidationItem ControlToValidateID="uxContactEmail" RegularExpression="^((?!(<[^ \t]))(?!(&#)).)*$" ResMessage="Resources.MessageManager.ContactUs_FromInvalidEmail" />
        </Items>
    </as:Validator>
    <tek:RadCodeBlock ID="rdBlock" runat="server">
        <script type="text/javascript">
            var ContactUs_txtBusinessName = "<%= txtBusinessName.ClientID%>";
            var ContactUs_txtMIDorVT = "<%= txtMIDorVT.ClientID%>";
            var ContactUs_txtContactName = "<%= txtContactName.ClientID%>";
            var ContactUs_txtContactNumber = "<%= txtContactNumber.ClientID%>";
            var ContactUs_uxContactEmail = "<%= uxContactEmail.ClientID%>";
            var ContactUs_uxDescOfIssue = "<%= uxDescOfIssue.ClientID%>";
            var ContactUs_uxSubjectCustom = "<%=uxSubjectCustom.ClientID%>";
            var ContactUs_EntityDisplayName_RequiredMessage = "<%= EntityDisplayNameText_RequiredMessage %>";
        </script>
        <script type="text/javascript" src="<%= ResolveUrl("~/")%>res/js/jquery/jquery.mask.min.js"></script>
        <script src="<%=ResolveUrl("~")%>res/js/ContactUs.js"></script>
    </tek:RadCodeBlock>
</asp:Content>
