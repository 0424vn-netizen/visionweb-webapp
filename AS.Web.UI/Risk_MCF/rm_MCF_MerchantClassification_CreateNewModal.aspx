<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPagePopup.master" CodeFile="rm_MCF_MerchantClassification_CreateNewModal.aspx.cs" Inherits="rm_MCF_MerchantClassification_CreateNewModal"
    Culture="auto" ValidateRequest="false" meta:resourcekey="PageResource1" UICulture="auto" %>

<%@ Register Src="~/UserControls/rm_MCF_MerchantClassification_Attribute.ascx" TagName="ucMCA" TagPrefix="uc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPage" runat="Server">
    <as:RadAjaxManagerProxy ID="RadAjaxManager1" runat="server">
        <AjaxSettings>
            <tek:AjaxSetting AjaxControlID="lnkAddAttribute">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="uxAttribute1" LoadingPanelID="uxLoadingPanelCustom" />
                </UpdatedControls>
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="lnkAddAttribute" LoadingPanelID="uxLoadingPanelCustom" />
                </UpdatedControls>
            </tek:AjaxSetting>
            <tek:AjaxSetting AjaxControlID="uxAttribute1">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="uxAttribute1" LoadingPanelID="uxLoadingPanelCustom" />
                </UpdatedControls>
            </tek:AjaxSetting>
        </AjaxSettings>
    </as:RadAjaxManagerProxy>
    <as:ASModalContainer ID="uxModalContainer" runat="server" WidthCssClass="modal-xxl" ContainerCssClass="container" Width="">
        <div class="row">
            <div class="col-md-12">
                <h2 class="modal-title">
                    <asp:Literal ID="litTitle" runat="server" meta:resourcekey="uxTitle"></asp:Literal>
                </h2>

            </div>
        </div>
        <div class="row">
            <!-- Classification  !-->
            <div class="col-md-12">
                <div class="risk-form-container mt-m-1x">
                    <div class="risk-form text-center">
                        <div class="risk-form-row">
                            <as:ValidatorLabel ID="uxClassificationLabel" runat="server" Text="Classification Name" ApplyFor="uxClassificationText" CssClass="control-label" meta:resourcekey="uxClassificationLabelResource1" />
                            <div class="control-inline">
                                <as:RadTextBox CssClass="form-control" Width="360px" MaxLength="50" Font-Size="14px" Height="26px"
                                    ID="uxClassificationText" runat="server" ValidationGroup="Validate" HintCss="hint" EmptyMessage='<%# GetLocalResourceObject("ClassificationName.Text") %>' LabelCssClass="" LabelWidth="64px">
                                </as:RadTextBox>
                                <div class="bottom-error text-left" style="Width: 360px">
                                    <as:ValidatorMessage ID="ValidatorMessage3" runat="server" ApplyFor="uxClassificationText" Message="" ShowOnLoad="False">
                                    </as:ValidatorMessage>
                                </div>
                            </div>
                            <div class="inline-block valign-top ml-8">
                                <as:ValidatorLabel ID="uxMultiplierLabel" runat="server" Text="Multiplier" ApplyFor="uxMultiplier" CssClass="control-label" meta:resourcekey="uxMultiplierLabelResource1" />
                            </div>
                            <div class="control-inline valign-top">
                                <as:TextBox CssClass="form-control" Width="160px" MaxLength="10" TextMode="SingleLine" EmptyMessage=""
                                    ID="uxMultiplier" runat="server" Font-Size="14px" ValidationGroup="Validate" meta:resourcekey="uxMultiplierResource1" LabelCssClass="" LabelWidth="64px">
                                    
                                </as:TextBox>
                                <div class="bottom-error text-left" style="Width: 160px">
                                    <as:ValidatorMessage ID="ValidatorMessage4" runat="server" ApplyFor="uxMultiplier" Message="" ShowOnLoad="False">
                                    </as:ValidatorMessage>
                                </div>
                            </div>

                        </div>
                    </div>

                </div>
            </div>
            <!-- End of Classification  !-->
        </div>

        <!-- Attributes!-->

        <div class="row">
            <div class="col-md-12">
                <h2 class="modal-title mt-7x">
                    <asp:Literal ID="litAttribute" runat="server" meta:resourcekey="uxAttribute" Text="Assign Attribute(s)"></asp:Literal>
                </h2>

            </div>

        </div>
        <!-- Attribute list !-->
        <div class="row">
            <div class="col-md-12">
                <uc:ucMCA ID="uxAttribute1" runat="server" />
            </div>
        </div>
        <div class="row">
            <div class="col-md-12">
                <div class="ipmt mt-3x">
                    <div class="dark-blue">
                        <as:LinkButton ID="lnkAddAttribute" CssClass="link-back" runat="server" OnClick="lnkAddAttribute_Click" meta:resourcekey="uxAddAttribute" Text="+ Add Attribute"></as:LinkButton>
                    </div>
                </div>
            </div>
        </div>
        <div style="display: none">
            <as:TextBox ID="txtText" runat="server"></as:TextBox>
        </div>
        <div class="bottom-error text-center">
            <as:ValidatorMessage ID="ValidatorMessage5" runat="server" ApplyFor="txtText" Message="" ShowOnLoad="False">
            </as:ValidatorMessage>
        </div>
        <div class="row">
            <div class="col-md-12 text-right form-action-container mt-1x">
                <as:Button ID="uxSubmit" runat="server" Text="Submit" OnClick="uxSubmit_Click" OnClientClick="if(!ValidateData()){AdjustModalSize(); return false;}" class="btn btn-default" meta:resourcekey="uxSubmitResource" UseSubmitBehavior="false" />
                <asp:Button ID="uxCancel" OnClientClick="parent.HidePopupModal();" Text="Cancel" class="btn btn-default" UseSubmitBehavior="false" runat="server" meta:resourcekey="uxCancelResource"></asp:Button>
            </div>
        </div>
        <asp:HiddenField ID="hidClassificationID" Value="" runat="server" />
    </as:ASModalContainer>
    <as:Validator ID="Validator1" runat="server" ValidationFunction="ValidateInput" MessageType="Inline" meta:resourcekey="Validator1Resource1" IgnoreServerValidation="False">
        <Items>
            <as:CustomValidationItem ControlToValidateID="uxClassificationText" ClientValidationFunction="ValidateClassificationName" meta:resourcekey="uxClassificationText_Required" />
            <as:CustomValidationItem ResMessage="Resources.RiskMessageManager.Assignment_GeneralInformation_SpecialCharacters"
                ResParams="Classification Name" ClientValidationFunction="ValidateSpecialCharacters"
                ControlToValidateID="uxClassificationText" />


            <as:RegExValidationItem ControlToValidateID="uxClassificationText" RegularExpression="^((?!(<[^ \t]))(?!(&#)).)*$" meta:resourcekey="uxClassificationText_InvalidText" />

            <as:CustomValidationItem ControlToValidateID="uxClassificationText" ClientValidationFunction="ValidateExistingClassificationName" meta:resourcekey="uxClassificationText_Duplicate" />
            <as:BasicValidationItem ControlToValidateID="uxMultiplier" Rule="Required" meta:resourcekey="uxMultiplier_Required" />
            <as:BasicValidationItem ControlToValidateID="uxMultiplier" Rule="Number" ResMessage="Resources.MessageManager.Generic_NumbericOnly" />
            <as:BasicValidationItem ControlToValidateID="uxMultiplier" Rule="GreaterThan" MinValue="0" Message="The Multiplier must be greater than 0." meta:resourcekey="uxClassificationText_GreateThan0" />
            <as:CustomValidationItem ControlToValidateID="uxMultiplier" ClientValidationFunction="ValidateOnlyNumber" meta:resourcekey="uxMultiplier_Decimal" />
            <as:CustomValidationItem ControlToValidateID="txtText" ClientValidationFunction="CheckAtleastOneAttribute" Message="The Classification must have at least 1 Attribute" meta:resourcekey="Atleast1Attribute" />
        </Items>
    </as:Validator>
    <as:Validator ID="Validator2" runat="server" ValidationFunction="ValidateInputClassificationName" MessageType="Inline" meta:resourcekey="Validator1Resource1" IgnoreServerValidation="False">
        <Items>
            <as:CustomValidationItem ResMessage="Resources.RiskMessageManager.Assignment_GeneralInformation_SpecialCharacters"
                ResParams="Classification Name" ClientValidationFunction="ValidateSpecialCharacters" ControlToValidateID="uxClassificationText" />
        </Items>
    </as:Validator>
    <as:Validator ID="Validator3" runat="server" ValidationFunction="ValidateMultiplier" MessageType="Inline" meta:resourcekey="Validator1Resource1" IgnoreServerValidation="False">
        <Items>
            <as:BasicValidationItem ControlToValidateID="uxMultiplier" Rule="Number" ResMessage="Resources.MessageManager.Generic_NumbericOnly" />
        </Items>
    </as:Validator>

    <tek:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <script type="text/javascript">
            var rm_ClassificationID = '<%= hidClassificationID.ClientID %>';
            var rm_ClassificationName = '<%= uxClassificationText.ClientID %>';
            var rm_Multiplier = '<%= uxMultiplier.ClientID %>';
            var rm_Classification_Hint = '<%= GetLocalResourceObject("ClassificationName.Text") %>'
            var rootURL = '<%= ResolveUrl("~") %>';
            var msgitemsSelected = '<%= GetLocalResourceObject("LiteralResourceFindMoreitem").ToString() %>';
         
        </script>
        <script src="<%= ResolveUrl("~/")%>res/js/risk_MCF/rm_MCF_MerchantClassification_Modal.js"></script>
    </tek:RadCodeBlock>


</asp:Content>
