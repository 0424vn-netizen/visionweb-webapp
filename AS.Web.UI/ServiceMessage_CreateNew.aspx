<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPagePopup.master" AutoEventWireup="true"
    CodeFile="ServiceMessage_CreateNew.aspx.cs" Inherits="ServiceMessage_CreateNew" meta:resourcekey="PageResource1" %>

<%@ Register Src="~/UserControls/UxExport.ascx" TagName="UxExport" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="Server">
    <as:ASModalContainer ID="uxModalContainer" runat="server" WidthCssClass="modal-xxl" ContainerCssClass="container" Width="">
        <div class="row">
            <div class="col-md-12">
                <h2 class="grid-title on-top" data-toggle="collapse" data-target="#ciMessage">
                    <as:Literal ID="ltMessages" runat="server" Text="Messages" meta:resourcekey="ltMessagesResource1"></as:Literal>
                </h2>
            </div>
        </div>
        <div class="in" id="ciMessage">
            <table class="ASTable">
                <colgroup>
                    <col width="180px" />
                    <col />
                    <as:RadCodeBlock ID="RadCodeBlock6" runat="server">
                        <%=FeatureMode == WebSiteEnums.FeatureMode.Edit ? "<col  width=\"45px\"/>" : ""%>
                    </as:RadCodeBlock>
                </colgroup>
                <tr class="AltRow">
                    <td class="heading">
                        <as:ValidatorLabel ID="ValidatorLabel1" ApplyFor="txtPostedBy" Text="Posted by:" meta:resourcekey="ValidatorLabel1Resource1"
                            runat="server">
                        </as:ValidatorLabel>
                    </td>
                    <td>
                        <as:PlaceHolder ID="uxPostedByEdit" runat="server">
                            <as:TextBox ID="txtPostedBy" CssClass="form-control" runat="server" Width="60%" MaxLength="50"
                                onblur="onPostedByBlur(this, event)" meta:resourcekey="txtPostedByResource1"></as:TextBox>
                            <as:ValidatorMessage ID="ValidatorMessage1" ApplyFor="txtPostedBy" runat="server">
                            </as:ValidatorMessage>
                            <div class="text-muted">
                                <i>
                                    <as:Literal ID="ltSomethingToShow" runat="server" Text='(This name will be displayed as "Posted By" along with the message.)' meta:resourcekey="ltSomethingToShowResource1"></as:Literal></i>
                            </div>
                        </as:PlaceHolder>
                        <as:Literal ID="ltrPostedBy" runat="server" Visible="false" meta:resourcekey="ltrPostedByResource1"></as:Literal>
                    </td>
                </tr>
                <tr class="Row">
                    <td class="heading">
                        <as:ValidatorLabel ID="ValidatorLabel2" ApplyFor="txtMessage" Text="Message:" runat="server" meta:resourcekey="ValidatorLabel2Resource1">
                        </as:ValidatorLabel>
                    </td>
                    <td class="valign-top">
                        <as:TextBox ID="txtMessage" runat="server" CssClass="form-control" Width="60%" MaxLength="1000"
                            TextMode="MultiLine"
                            Height="100px" onkeyup="checkComment()" meta:resourcekey="txtMessageResource1"
                            onblur="onMessageBlur(this, event)"></as:TextBox>
                        <as:ValidatorMessage ID="ValidatorMessage2" ApplyFor="txtMessage" runat="server">
                        </as:ValidatorMessage>
                        <as:Literal ID="ltrMessage" runat="server" Visible="false" meta:resourcekey="ltrMessageResource1"></as:Literal>
                        <%=FeatureMode == WebSiteEnums.FeatureMode.Edit ? ("<div class=\"default-text-color\">"+GetLocalResourceObject("ServiceMessage_CreateNew_aspx_YouHaveLimitText").ToString()+"</div>") : ""%>
                    </td>
                </tr>
            </table>
        </div>
        <div class="row">
            <div class="col-md-12">
                <h2 class="grid-title" data-toggle="collapse" data-target="#UserList">
                    <as:Literal ID="Literal1" runat="server" Text="Message Recipient Information" meta:resourcekey="Literal1Resource1"></as:Literal></h2>
            </div>
        </div>
        <as:RadCodeBlock ID="RadCodeBlock7" runat="server">
            <div class="in" id="UserList">
                <as:ASGrid ID="uxUserList" runat="server" AutoGenerateColumns="false" AllowMultiRowSelection="true"
                    AllowSorting="True" AllowPaging="true" ASPagingMethod="SPASingleMethod" AllowFilteringByColumn="True"
                    OnItemCommand="uxUserList_ItemCommand" CssClass="in" meta:resourcekey="uxUserListResource1">
                    <MasterTableView Width="100%">
                        <Columns>
                            <as:ASGridTemplateColumn UniqueName="Assigned" AllowFiltering="false" ItemStyle-Width="5%"
                                HeaderStyle-Width="5%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" meta:resourcekey="ASGridTemplateColumnResource1">
                                <HeaderTemplate>
                                    <as:CheckBox ID="chkHeader" CssClass="valign-middle" runat="server" Visible="False" meta:resourcekey="chkHeaderResource1" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <as:CheckBox ID="chkItem" runat="server" CssClass="checkbox-middle" meta:resourcekey="chkItemResource1" />
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Center" Width="5%"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center" Width="5%"></ItemStyle>
                            </as:ASGridTemplateColumn>
                            <tek:GridBoundColumn HeaderText="UserID" DataField="UserID" UniqueName="UserID" HeaderStyle-HorizontalAlign="Center"
                                ItemStyle-HorizontalAlign="center" HeaderStyle-Width="30%" ItemStyle-Width="30%"
                                FilterControlWidth="80%" meta:resourcekey="GridBoundColumnResource1">
                                <ColumnValidationSettings>
                                    <ModelErrorMessage Text=""></ModelErrorMessage>
                                </ColumnValidationSettings>
                                <HeaderStyle HorizontalAlign="Center" Width="30%"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center" Width="30%"></ItemStyle>
                            </tek:GridBoundColumn>
                            <tek:GridBoundColumn HeaderText="User Name" DataField="UserName" HtmlEncode="true"
                                UniqueName="UserName" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="center"
                                HeaderStyle-Width="35%" ItemStyle-Width="35%" FilterControlWidth="80%" meta:resourcekey="GridBoundColumnResource2">
                                <ColumnValidationSettings>
                                    <ModelErrorMessage Text=""></ModelErrorMessage>
                                </ColumnValidationSettings>
                                <HeaderStyle HorizontalAlign="Center" Width="35%"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center" Width="35%"></ItemStyle>
                            </tek:GridBoundColumn>
                            <tek:GridBoundColumn HeaderText="Role" DataField="HierarchyName" HtmlEncode="true"
                                UniqueName="HierarchyName" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="center"
                                HeaderStyle-Width="30%" ItemStyle-Width="30%" FilterControlWidth="80%" meta:resourcekey="GridBoundColumnResource3">
                                <ColumnValidationSettings>
                                    <ModelErrorMessage Text=""></ModelErrorMessage>
                                </ColumnValidationSettings>
                                <HeaderStyle HorizontalAlign="Center" Width="30%"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center" Width="30%"></ItemStyle>
                            </tek:GridBoundColumn>
                        </Columns>
                    </MasterTableView>
                </as:ASGrid>
            </div>
        </as:RadCodeBlock>
        <as:PlaceHolder ID="uxPlhBottom" runat="server">
            <div class="row">
                <div class="col-md-12 form-action-container text-right">
                    <as:Button ID="uxSave" runat="server" Text="Send Message" CssClass="btn btn-default"
                        OnClick="uxSave_Click"
                        OnClientClick="return ValidateSendMessage(this.id);" meta:resourcekey="uxSaveResource1" />
                    <as:Button ID="uxCancel" runat="server" Text="Cancel" CssClass="btn btn-default"
                        OnClick="uxCancel_Click" meta:resourcekey="uxCancelResource1" />
                </div>
            </div>
        </as:PlaceHolder>

        <asp:UpdatePanel runat="server" ID="UpdatePanel1">
            <ContentTemplate>
                <as:Button ID="btnHidden" runat="server" IsStandardButton="True" Style="display: none;"
                    OnClick="btnHidden_Click" meta:resourcekey="btnHiddenResource1" />
                <as:HiddenField ID="uxIsAdded" runat="server" />
                <as:HiddenField ID="uxIsSelectAll" runat="server" />
                <as:HiddenField ID="uxValueCode" runat="server" />
                <as:HiddenField ID="uxAllFlag" runat="server" />
                <as:HiddenField ID="uxMessageID" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>

        <as:Validator ID="uxValidator" runat="server" ValidationFunction="ValidateMessageInfo"
            MessageType="Inline" meta:resourcekey="uxValidatorResource1">
            <Items>
                <as:BasicValidationItem ControlToValidateID="txtPostedBy" Rule="Required" ResMessage="Resources.RiskMessageManager.Assignment_GeneralInformation_Required" />
                <as:BasicValidationItem ControlToValidateID="txtPostedBy" Rule="Minlength" MinLength="2"
                    ResMessage="Resources.RiskMessageManager.Assignment_GeneralInformation_MinLength"
                    ResParams="2" />
                <as:BasicValidationItem ControlToValidateID="txtPostedBy" Rule="Maxlength" MaxLength="50"
                    ResMessage="Resources.RiskMessageManager.Assignment_GeneralInformation_MaxLength"
                    ResParams="50" />
                <as:BasicValidationItem ControlToValidateID="txtMessage" Rule="Required" ResMessage="Resources.RiskMessageManager.Assignment_GeneralInformation_Required" />
                <as:BasicValidationItem ControlToValidateID="txtMessage" Rule="Maxlength" MaxLength="1000"
                    ResMessage="Resources.RiskMessageManager.Assignment_GeneralInformation_MaxLength"
                    ResParams="1000" />
            </Items>
        </as:Validator>

        <as:Validator ID="ValidatorPostedBy" runat="server" ValidationFunction="ValidatePostedBy" MessageType="Inline" MessageContainerClientID="" meta:resourcekey="ValidatorResource1">
            <Items>
                <as:RegExValidationItem ControlToValidateID="txtPostedBy" RegularExpression="^((?!(<[^ \t]))(?!(&#)).)*$" ResMessage="Resources.LanguageResource.AS_CommonJS_Msg_IncludedSpeciaCharacters" />
            </Items>
        </as:Validator>
        <as:Validator ID="ValidatorMessage" runat="server" ValidationFunction="ValidateMessage" MessageType="Inline" MessageContainerClientID="" meta:resourcekey="ValidatorResource1">
            <Items>
                <as:CustomValidationItem ControlToValidateID="txtMessage" ClientValidationFunction="validationSendMessage" ResMessage="Resources.LanguageResource.AS_CommonJS_Msg_IncludedSpeciaCharacters" />
            </Items>
        </as:Validator>

    </as:ASModalContainer>
    <as:RadAjaxManagerProxy ID="uxRadAjaxManagerProxy" runat="server">
        <AjaxSettings>
            <tek:AjaxSetting AjaxControlID="uxUserList">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="uxUserList" />
                </UpdatedControls>
            </tek:AjaxSetting>
        </AjaxSettings>
    </as:RadAjaxManagerProxy>

    <as:RadCodeBlock ID="radCodeBlock" runat="server">
        <script type="text/javascript">
            var txtMessage_ClientID = '<%=txtMessage.ClientID %>';
            var uxMessageID_ClientID = '<%=uxMessageID.ClientID %>';
            var uxIsAdded_ClientID = '<%=uxIsAdded.ClientID %>';
            var uxIsSelectAll_ClientID = '<%=uxIsSelectAll.ClientID %>';
            var uxValueCode_ClientID = '<%=uxValueCode.ClientID %>';
            var uxAllFlag_ClientID = '<%=uxAllFlag.ClientID %>';
            var IS_SELECT_ALL_FLAG = '<%=IS_SELECT_ALL_FLAG%>';
            var IS_DESELECT_ALL_FLAG = '<%=IS_DESELECT_ALL_FLAG%>';
            var btnHidden_ClientID = '<%=btnHidden.ClientID %>';
            var Generic_FieldLengthExceeded = '<%=Resources.MessageManager.Generic_FieldLengthExceeded%>'; 
            var ServiceMessage_CreateNew_js_SelectRecipient = '<%= GetLocalResourceObject("ServiceMessage_CreateNew_js_SelectRecipient").ToString()%>';
        </script>
        <script type="text/javascript" src="<%= ResolveUrl("~/") %>res/js/ServiceMessage_CreateNew.js"> </script>
    </as:RadCodeBlock>
</asp:Content>

