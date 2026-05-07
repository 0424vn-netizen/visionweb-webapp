<%@ Page Language="C#" MasterPageFile="~/MasterPagePopup.master" AutoEventWireup="true"
    CodeFile="Message_CreateNew.aspx.cs" Inherits="Message_CreateNew" Title="Send New Message" meta:resourcekey="PageResource1" %>

<%@ Register Src="~/UserControls/Message_HierarchyFilter.ascx" TagName="Hierarchy"
    TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <meta http-equiv="X-UA-Compatible" content="IE=7" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="Server">
    <as:ASModalContainer runat="server" ID="uxModalContainer" Width="modal-xxl">
        <as:RadAjaxManagerProxy ID="RadAjaxManagerProxyReview" runat="server">
            <AjaxSettings>
                <tek:AjaxSetting AjaxControlID="cbxIsMerchant">
                    <UpdatedControls>
                        <tek:AjaxUpdatedControl ControlID="cbxIsMerchant" />
                    </UpdatedControls>
                </tek:AjaxSetting>
            </AjaxSettings>
        </as:RadAjaxManagerProxy>
        <div class="height-10"></div>
        <h2 class="grid-title on-top" data-toggle="collapse" data-target="#ciMessage"><as:Literal ID="ltMessages" runat="server" Text="Messages" meta:resourcekey="ltMessagesResource1"></as:Literal>
        </h2>
        <div class="row">
            <div class="col-md-12">
                <table class="ASTable in" id="ciMessage">
                    <colgroup>
                        <col style="width: 140px" />
                        <col />
                    </colgroup>
                    <tr class="AltRow">
                        <td class="heading">
                            <as:ValidatorLabel ID="ValidatorLabel1" ApplyFor="txtPostedBy" Text="Posted by:" runat="server" meta:resourcekey="uxtxtPostedByResource1"></as:ValidatorLabel>
                        </td>
                        <td>
                            <as:PlaceHolder ID="uxPostedByEdit" runat="server">
                                <as:TextBox ID="txtPostedBy" runat="server" Width="60%" MaxLength="50" onkeypress="return txt_OnKeyPress(event);" 
                                    onblur="onPostedByBlur(this, event)" CssClass="form-control"></as:TextBox>

                                <as:ValidatorMessage ID="ValidatorMessage1" ApplyFor="txtPostedBy" runat="server"></as:ValidatorMessage>

                                <div class="text-muted">
                                    <i><as:Literal ID="Literal1" runat="server" Text="" meta:resourcekey="Literal1Resource1"></as:Literal></i>
                                </div>
                            </as:PlaceHolder>
                            <as:Literal ID="ltrPostedBy" runat="server" Visible="false"></as:Literal>
                        </td>
                    </tr>
                    <tr class="Row">
                        <td class="heading">
                            <as:ValidatorLabel ID="ValidatorLabel2" ApplyFor="txtMessage" Text="Message:" runat="server" meta:resourcekey="uxValidatorLabelResource2"></as:ValidatorLabel>
                        </td>
                        <td class="valign-top">
                            <as:TextBox ID="txtMessage" runat="server" Width="60%" MaxLength="1000" TextMode="MultiLine"
                                CssClass="form-control" Height="100px" onkeyup="checkComment()" 
                                onblur="onMessageBlur(this, event)"></as:TextBox>

                            <as:ValidatorMessage ID="ValidatorMessage2" ApplyFor="txtMessage" runat="server"></as:ValidatorMessage>

                            <as:Literal ID="ltrMessage" runat="server" Visible="false"></as:Literal>
                            <%=FeatureMode == WebSiteEnums.FeatureMode.Edit ? GetLocalResourceObject("Message_CreateNew_aspx_TextLimit").ToString() : ""%>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12">
                <h2 class="grid-title" data-toggle="collapse" data-target="#ciMsgInfo"><as:Literal ID="Literal2" runat="server" Text="Message Recipient
                    Information" meta:resourcekey="Literal2Resource2"></as:Literal></h2>
            </div>
        </div>
        <as:Panel runat="server" ID="Container1" TemplateName="ascontainer_noborder.tpl">
            <table class="ASTable in" id="ciMsgInfo">
                <colgroup>
                    <col style="width: 140px" />
                    <col style="width: 80px" />
                    <col />
                    <as:RadCodeBlock ID="RadCodeBlock2" runat="server">
                        <%=FeatureMode == WebSiteEnums.FeatureMode.Edit ? "<col style=\"width:80px\"/>" : ""%>
                    </as:RadCodeBlock>
                </colgroup>
                <tr>
                    <th></th>
                    <th><as:Literal ID="Literal3" runat="server" Text="Merchant" meta:resourcekey="Literal3Resource2"></as:Literal><sup>(*)</sup>
                    </th>
                    <th></th>
                    <as:RadCodeBlock ID="RadCodeBlock3" runat="server">
                        <%=FeatureMode == WebSiteEnums.FeatureMode.Edit ? "<th></th>" : ""%>
                    </as:RadCodeBlock>
                </tr>
                <asp:Repeater ID="uxHierarchyFilterRepeater" runat="server" OnItemDataBound="uxHierarchyFilterRepeater_ItemDataBound">
                    <ItemTemplate>
                        <asp:PlaceHolder ID="uxGroupHierarchyEdit" runat="server">
                            <tr>
                                <td colspan="4" class="section-heading">
                                    <asp:Literal ID="uxGroupHierarchyTitleEdit" runat="server"></asp:Literal>
                                </td>
                            </tr>
                        </asp:PlaceHolder>
                        <asp:PlaceHolder ID="uxGroupHierarchyView" runat="server">
                            <tr>
                                <td colspan="3" class="section-heading">
                                    <asp:Literal ID="uxGroupHierarchyTitleView" runat="server"></asp:Literal>
                                </td>
                            </tr>
                        </asp:PlaceHolder>
                        <tr>
                            <td class="heading">
                                <%#Eval("DisplayedText") %>:
                            </td>
                            <td class="text-center">
                                <as:HiddenField ID="hddHierarchyFilterMode" runat="server" Value='<%#Eval("HierarchyFilterMode")%>' />
                                <as:CheckBox ID="cbxIsMerchant" runat="server" AutoPostBack="true" OnCheckedChanged="cbxIsMerchant_CheckedChanged" />
                            </td>
                            <td>
                                <as:Panel ID="divHierarchy" runat="server">
                                    <as:Literal ID="lblHierarchy" runat="server" Text="N/A" meta:resourcekey="lblHierarchyResource2"></as:Literal>
                                </as:Panel>
                            </td>
                            <as:PlaceHolder runat="server" ID="plhHierarchy">
                                <td class="text-center">
                                    <as:Panel ID="pnlHierarchyFilterModal" runat="server">
                                        <a class="grid-toggle"
                                            onclick="return CallHierarchyFilterModal('Message_HierarchyFilterModal.aspx?<%#HierarchyFilterQueryString(Eval("HierarchyFilterMode").ToString(),"ctl00_ContentPage_uxHierarchyFilterRepeater_ctl" + GetItemIndexAsString(Container.ItemIndex) + "_btnRefreshHierarchy")%>', 'auto'); return false;"><as:Literal ID="Literal31" runat="server" Text="Edit" meta:resourcekey="Literal31Resource2"></as:Literal></a>
                                        <as:Button ID="btnRefreshHierarchy" runat="server" OnCommand="btnRefreshHierarchy_Command"
                                            CssClass="display-none" CommandArgument='<%#Eval("HierarchyFilterMode") %>' IsStandardButton="true" />
                                    </as:Panel>
                                </td>
                            </as:PlaceHolder>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
            <div class="display-none">
                <!--invisible buttons-->
                <as:HiddenField ID="uxCountSelectedFilter" runat="server" Value="0" />
            </div>
        </as:Panel>

        <div class="row row-table">
            <div class="col-xs-8 merchant-count">
                <label>
                    <i><as:Literal ID="Literal4" runat="server" Text="(*) Select this option to send this message to Merchants belonging to the selected
                        Recipients" meta:resourcekey="Literal4Resource2"></as:Literal></i>
                </label>
            </div>
            <div class="col-xs-4">
                <div class="row">
                    <div class="col-xs-12 form-action-container">
                        <as:PlaceHolder ID="uxPlhBottom" runat="server">
                            <as:Button ID="uxSave" runat="server" Text="Send Message" OnClick="uxSave_Click"
                                OnClientClick="return ValidateSendMessage();" CssClass="btn btn-default" meta:resourcekey="uxSaveResource2" />
                            <as:Button ID="uxCancel" runat="server" Text="Cancel" CssClass="btn btn-default"
                                OnClick="uxCancel_Click" meta:resourcekey="uxCancelResource2"/>
                        </as:PlaceHolder>
                    </div>
                </div>
            </div>
        </div>
        <as:Validator ID="uxValidator" runat="server" ValidationFunction="ValidateMessageInfo"
            MessageType="Inline">
            <Items>
                <as:BasicValidationItem ControlToValidateID="txtPostedBy" Rule="Required" ResMessage="Resources.RiskMessageManager.Assignment_Required" />
                <as:BasicValidationItem ControlToValidateID="txtMessage" Rule="Required" ResMessage="Resources.RiskMessageManager.Assignment_Required" />
                <as:BasicValidationItem ControlToValidateID="txtMessage" Rule="Maxlength" MaxLength="1000"
                    ResMessage="Resources.ValMsg.MaxLength" ResParams="1000" />
                <as:BasicValidationItem ControlToValidateID="txtMessage" Rule="Minlength" MinLength="2"
                    ResMessage="Resources.ValMsg.MinLength" ResParams="2" />
                <as:BasicValidationItem ControlToValidateID="txtMessage" Rule="StringUnaccept" Pattern="<>"
                    ResMessage="Resources.ValMsg.InvalidCharacter" />
            </Items>
        </as:Validator>

        <as:Validator ID="uxValidateMessageBody" runat="server" ValidationFunction="ValidateMessageBody"
            MessageType="Inline">
            <Items>
                <as:BasicValidationItem ControlToValidateID="txtMessage" Rule="Required" ResMessage="Resources.RiskMessageManager.Assignment_Required" />
                <as:BasicValidationItem ControlToValidateID="txtMessage" Rule="Maxlength" MaxLength="1000"
                    ResMessage="Resources.ValMsg.MaxLength" ResParams="1000" />
                <as:BasicValidationItem ControlToValidateID="txtMessage" Rule="Minlength" MinLength="2"
                    ResMessage="Resources.ValMsg.MinLength" ResParams="2" />
                <as:BasicValidationItem ControlToValidateID="txtMessage" Rule="StringUnaccept" Pattern="<>"
                    ResMessage="Resources.ValMsg.InvalidCharacter" />
            </Items>
        </as:Validator>
        <as:Validator ID="ValidatorPostedBy" runat="server" ValidationFunction="ValidatePostedBy" MessageType="Inline" MessageContainerClientID="" meta:resourcekey="ValidatorResource1">
            <Items>
                <as:RegExValidationItem ControlToValidateID="txtPostedBy" RegularExpression="^((?!(<[^ \t]))(?!(&#)).)*$" ResMessage="Resources.LanguageResource.AS_CommonJS_Msg_IncludedSpeciaCharacters" />
            </Items>
        </as:Validator>
        <as:Validator ID="ValidatorMessage" runat="server" ValidationFunction="ValidateMessage" MessageType="Inline" MessageContainerClientID="" meta:resourcekey="ValidatorResource1">
            <Items>
                <as:RegExValidationItem ControlToValidateID="txtMessage" RegularExpression="^((?!(<[^ \t]))(?!(&#)).)*$" ResMessage="Resources.LanguageResource.AS_CommonJS_Msg_IncludedSpeciaCharacters" />
            </Items>
        </as:Validator>
        <as:RadCodeBlock ID="radCodeBlock" runat="server">
            <script type="text/javascript">
                var Message_CreateNew_txtMessage = "#<%= txtMessage.ClientID %>";
                var txtMessage_ClientID = "<%= txtMessage.ClientID %>";
                var Message_CreateNew_uxCountSelectedFilter = "<%= uxCountSelectedFilter.ClientID %>";
                var Message_CreateNew_js_msg1 = '<%= GetLocalResourceObject("Message_CreateNew_js_msg1").ToString() %>';
            </script>
            <script type="text/javascript" src="<%=ResolveUrl("~") %>res/js/Message_CreateNew.js"></script>
        </as:RadCodeBlock>
    </as:ASModalContainer>
</asp:Content>
