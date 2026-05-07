<%@ Page Language="C#" MasterPageFile="~/MasterPagePopup.master" AutoEventWireup="true"
    CodeFile="rm_MCF_CreateEditAutoQueueModal.aspx.cs" Inherits="rm_MCF_CreateEditAutoQueueModal" %>

<%@ Import Namespace="AS.Common.Utilities" %>

<%@ Register Src="~/UserControls/rm_MCF_CustomListBox.ascx" TagName="RiskCustomListBox" TagPrefix="uc" %>

<%--head--%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>

<%--content--%>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="Server">
    <as:ASModalContainer ID="ASModalContainer1" runat="server" WidthCssClass="modal-lg">
        <div class="box padding-6x">
            <div class="row mb-3x">
                <div class="col-xs-3">
                    <as:ValidatorLabel ID="lblName" runat="server" CssClass="control-label" ApplyFor="txtName" meta:resourcekey="lblName"></as:ValidatorLabel>
                    <span class="red">*</span>
                </div>
                <div class="col-xs-9">
                    <as:ASRadTextBox ID="txtName" runat="server" MaxLength="50" Width="100%" CssClass="form-control" onblur="onNameBlur(this, event)">
                    </as:ASRadTextBox>
                    <asp:HiddenField runat="server" ID="txtNameClientState"/>
                    <as:ValidatorMessage  runat="server" ID="valAssignmentNameMsg" ApplyFor="txtName" Message="Message valid" ShowOnLoad="false" />
                </div>
            </div>
            <div class="row">
                <div class="col-xs-3">
                    <as:ValidatorLabel ID="lblBriefDescription" runat="server" CssClass="control-label" meta:resourcekey="lblBriefDescription"
                        ApplyFor="txtBriefDescription">
                    </as:ValidatorLabel>
                </div>
                <div class="col-xs-9">
                    <div>
                        <as:ASRadTextBox TextMode="MultiLine" ID="txtBriefDescription" MaxLength="150" runat="server" Rows="1"
                            Width="100%" CssClass="form-control textarea-min-height" onblur="onBriefDescriptionBlur(this, event)">
                        </as:ASRadTextBox>
                       
                        <as:ValidatorMessage runat="server" ID="valBriefDescription" ApplyFor="txtBriefDescription" Message="Message valid valBriefDescription" ShowOnLoad="False" />
                    </div>
                    <div>
                        <as:RadioButton ID="rdActive" runat="server" Text="Active" GroupName="Date" CssClass="control-inline"
                            onkeypress="javascript:return false;" Value="" meta:resourcekey="rdActive" />
                        <as:RadioButton ID="rdInactive" runat="server" Checked="true" Text="Inactive" GroupName="Date" CssClass="control-inline"
                            onkeypress="javascript:return false;" Value="" meta:resourcekey="rdInActive" />
                    </div>
                    <%--<div>
                        <as:CheckBox runat="server" ID="chkAggregate" Text="Aggregate Queue" CssClass="control-inline" meta:resourcekey="chkAggregate" /> 
                    </div>--%>
                </div>
            </div>
        </div>

        <div class="row mt-2x">
            <div class="col-xs-6">
                <h4 class="title-auto-queue">
                    <as:Literal runat="server" meta:resourcekey="lbAssignments"></as:Literal>
                    <span class="red">*</span></h4>
                <uc:RiskCustomListBox runat="server" ID="lbAssigment" OnItemDataBound="lbAssigment_OnItemDataBound" DataFieldValue="AssignmentID" DataFieldText="AssignmentName" />

                <div class=""></div>
            </div>
            <div class="col-xs-6">
                <h4 class="title-auto-queue">
                    <as:Literal ID="Literal1" runat="server" meta:resourcekey="lbWorkQueue"></as:Literal>
                    <span class="red">*</span>
                </h4>

                <uc:RiskCustomListBox runat="server" OnItemDataBound="lbWorkQueue_OnItemDataBound" ID="lbWorkQueue" DataFieldValue="AssignmentID" DataFieldText="AssignmentName" />

            </div>
        </div>
        <div class="row">
            <div class="col-xs-6">
                <as:Button ID="btnDelete" UseSubmitBehavior="False" runat="server" CssClass="display-none" class="btn btn-default mt-4x" OnClick="btnDelete_OnClick" />
                <as:Button runat="server" UseSubmitBehavior="False" meta:resourcekey="btnDeleteCover"  ID="btnDeleteCover" class="btn btn-default mt-4x" OnClientClick="return mdlCreateEdit.onDeleteAutoQueue()" />
            </div>
            <div class="col-xs-6">
                <div class="text-right">
                    <as:HiddenField ID="uxHdRequiredBe" runat="server" /> 
                    <as:Button runat="server" ID="btnSubmitCover" class="btn btn-default mt-4x" OnClientClick="return mdlCreateEdit.onSubmitClick(event,this)" />
                    <as:Button ID="btnSubmit" UseSubmitBehavior="False" OnClick="btnSubmit_OnClick" runat="server" class="btn btn-default mt-4x ml-4x" CssClass="display-none" />
                    <as:Button ID="btnReloadAssignments" UseSubmitBehavior="False" OnClick="btnReloadAssignments_OnClick" runat="server" class="btn btn-default ml-4x" CssClass="display-none" />
                    <as:Button ID="btnCancel" UseSubmitBehavior="False" OnClientClick="return ClosePopupModal();" class="btn btn-default ml-4x mt-4x " runat="server" meta:resourcekey="btnCancel"
                        CausesValidation="false" />

                    <as:ASCommandControl runat="server" CallServerFunc="checkNameAvailable" ClientSuccessCallbackFunc="mdlCreateEdit.checkNameAvailableSuccess"
                        ID="btnCheckNameAvailable" OnCreateResponseData="btnCheckNameAvailable_CreateResponseData" IsRefreshPostData="true" />

                    <as:ASCommandControl runat="server" CallServerFunc="checkDeleteAvailable" ClientSuccessCallbackFunc="mdlCreateEdit.checkDeleteAvailableSuccess"
                        ID="ASCommandControl1" OnCreateResponseData="btnCheckDeleteAvailable_CreateResponseData" IsRefreshPostData="true" />
                </div>
            </div>
        </div>
        <div class="mt-3x">
            <span class="red">* </span>
            <as:Literal ID="Literal2" runat="server" meta:resourcekey="titleRequireField"></as:Literal>
        </div>
        <as:Validator ID="Validator1" runat="server" ValidationFunction="ValidateInput" MessageContainerClientID="" MessageType="Inline">
            <Items>
                <as:CustomValidationItem ResMessage="Resources.ValMsg.Required_Unique"
                    ClientValidationFunction="customValidate"
                    ControlToValidateID="txtName" />
                <as:BasicValidationItem ControlToValidateID="txtName" Rule="Maxlength" MaxLength="50" ResMessage="Resources.ValMsg.MaxLength" />
                <as:BasicValidationItem ControlToValidateID="txtBriefDescription" Rule="Maxlength" MaxLength="100" ResMessage="Resources.ValMsg.MaxLength" />
            </Items>
        </as:Validator>
        <as:Validator ID="Validator2" runat="server" ValidationFunction="ValidateInputSpecialCharacters" MessageContainerClientID="" MessageType="Inline">
            <Items>
                <as:CustomValidationItem ResMessage="Resources.ValMsg.ValidateSpecialCharacters"
                    ClientValidationFunction="validateSpecialCharacters"
                    ControlToValidateID="txtName" />
            </Items>
        </as:Validator>
        <as:Validator ID="Validator3" runat="server" ValidationFunction="ValidateInputSpecialCharactersDescription" MessageContainerClientID="" MessageType="Inline">
            <Items>
                <as:CustomValidationItem ResMessage="Resources.ValMsg.ValidateSpecialCharactersDescription"
                    ClientValidationFunction="validateSpecialCharactersDescription"
                    ControlToValidateID="txtBriefDescription" />
            </Items>
        </as:Validator>
    </as:ASModalContainer>

    <as:ASRadCodeBlock ID="ASRadCodeBlock1" runat="server">
        <script>
            var lbAssigmentId = "<%=lbAssigment.ClientID%>";
            var lbWorkQueueId = "<%=lbWorkQueue.ClientID%>";
            var txtNameId = "<%=txtName.ClientID%>";
            var txtDescriptionId = "<%=txtBriefDescription.ClientID%>";
            var btnSubmitId = "<%=btnSubmit.ClientID%>";
            var rdActiveId = "<%=rdActive.ClientID%>";
            var autoQueueMode = "<%=AutoQueueMode.ToString()%>";
            var btnSubmitCoverId = "<%=btnSubmitCover.ClientID%>";
            var btnDeleteCoverId = "<%=btnDeleteCover.ClientID%>";
            var btnDeleteId = "<%=btnDelete.ClientID%>";
            var btnReloadAssigmentId = "<%=btnReloadAssignments.ClientID%>";
            var autoQueueName = '<%=txtName.Text.ToJString()%>';
            var createMessage = '<%=GetLocalResourceObject("scriptCreateMessage.Text")%>';
            var anotherAssignmentMessage = '<%=GetLocalResourceObject("scriptAnotherAssignmentMessage.Text")%>';
            var successMessage = '<%=GetLocalResourceObject("successMessage.Text")%>';
            var assignmentWorkqueueMessage = '<%=GetLocalResourceObject("assignmentWorkqueueMessage.Text")%>';
            var workQueueDeleteMessage = '<%=GetLocalResourceObject("workQueueDeleteMessage.Text")%>';
            var modalConfirm = '<%=Resources.ValMsg.modalConfirm %>';
            var modalSuccess = '<%=Resources.ValMsg.modalSucess %>';
            var modalWarning = '<%=Resources.ValMsg.modalWarning %>';
            var updateMessage = '<%=string.Format(GetLocalResourceObject("scriptUpdateMessage.Text").ToString(),txtName.Text.ToJString())%>';
            var deleteMessage = '<%=string.Format(GetLocalResourceObject("scriptDeleteMessage.Text").ToString(),txtName.Text.ToJString())%>';
            var isRequiredBe = "<%= uxHdRequiredBe.ClientID%>";
        </script>

        <script src="<%= ResolveUrl("~/")%>res/js/risk_MCF/rm_MCF_AutoQueueCustomListBox.js"></script>
        <script src="<%= ResolveUrl("~/")%>res/js/risk_MCF/rm_MCF_CreateEditAutoQueueModal.js"></script>
    </as:ASRadCodeBlock>

</asp:Content>