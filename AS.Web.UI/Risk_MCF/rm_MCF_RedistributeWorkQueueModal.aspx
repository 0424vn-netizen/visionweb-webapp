<%@ Page Language="C#" MasterPageFile="~/MasterPagePopup.master" AutoEventWireup="true"
    CodeFile="rm_MCF_RedistributeWorkQueueModal.aspx.cs" Inherits="rm_MCF_RedistributeWorkQueueModal"
    meta:resourcekey="Title" %>

<%@ Register Src="~/UserControls/rm_MCF_CustomListBox.ascx" TagName="RiskCustomListBox" TagPrefix="uc" %>

<%--head--%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>

<%--content--%>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="Server">

    <as:ASModalContainer ID="ASModalContainer1" runat="server" WidthCssClass="modal-xl">
        <as:ASRadAjaxPanel ID="ASRadAjaxPanel1" LoadingPanelID="uxLoadingPanelCustom" runat="server">
            <div class="row">
                <div class="col-xs-4">
                    <div class="mb-4x">
                        <span class="font-20">1.</span>
                        <as:Literal ID="Literal1" runat="server" meta:resourcekey="lbAutoQueue"></as:Literal>
                    </div>
                </div>
                <div class="col-xs-4">
                    <div class="mb-4x">
                        <span class="font-20">2.</span>
                        <as:Literal ID="Literal2" runat="server" meta:resourcekey="lbWorkQueueRes"></as:Literal>
                    </div>
                </div>
                <div class="col-xs-4">
                    <div class="mb-4x">
                        <span class="font-20">3.</span>
                        <as:Literal ID="Literal4" runat="server" meta:resourcekey="lbWorkQueueDes"></as:Literal>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-xs-4  select-two">
                    <uc:RiskCustomListBox isAutoPostBack="true" runat="server" ID="lbAutoQueue" OnItemCommand="lbAutoQueue_OnItemCommand" DataFieldValue="AutoQueueID" DataFieldText="AutoQueueName" />
                </div>

                <div class="col-xs-4">
                    <uc:RiskCustomListBox isAutoPostBack="true" runat="server" ID="lbWorkQueueRedis" OnItemDataBound="lbWorkQueueRedis_OnItemDataBound" OnItemCommand="lbWorkQueueRedis_OnItemCommand" DataFieldValue="AssignmentID" DataFieldText="AssignmentName" />
                </div>

                <div class="col-xs-4">
                    <uc:RiskCustomListBox runat="server" ID="lbWorkQueueDes" OnItemDataBound="lbWorkQueueDest_OnItemDataBound" DataFieldValue="AssignmentID" DataFieldText="AssignmentName" />
                </div>
            </div>
        </as:ASRadAjaxPanel>
        <as:ASCommandControl runat="server" CallServerFunc="checkRedistributeAvailable" ClientSuccessCallbackFunc="mdlRedistribute.checkRedistributeAvailableSuccess"
            ID="btnCheckRedistributeAvailable" OnCreateResponseData="btnCheckRedistributeAvailable_CreateResponseData" />
        <as:ASRadAjaxPanel ID="ASRadAjaxPanel2" runat="server" LoadingPanelID="uxLoadingPanelCustom">
            <div class="mt-4x text-right">
                <as:Button ID="btnSubmitCover" OnClientClick="return mdlRedistribute.onSubmitClick()" disabled="disabled" runat="server" UseSubmitBehavior="false" class="btn btn-default" meta:resourcekey="btnRedistribute" />
                <as:Button ID="btnSubmit" OnClick="btnSubmit_OnClick" runat="server" class="btn btn-default" CssClass="display-none ml-4x" />
                <as:Button ID="btnCancel" OnClientClick="parent.HidePopupModal();" class="btn btn-default ml-4x" runat="server" meta:resourcekey="btnCancel"
                    CausesValidation="false" UseSubmitBehavior="false" />
            </div>
        </as:ASRadAjaxPanel>

    </as:ASModalContainer>

    <as:ASRadCodeBlock ID="ASRadCodeBlock1" runat="server">
        <script>
            var lbWorkQueueRedisId = "<%= lbWorkQueueRedis.ClientID %>";
            var lbWorkQueueDesId = "<%= lbWorkQueueDes.ClientID %>";
            var lbAutoQueueId = "<%= lbAutoQueue.ClientID %>"
            var btnSubmitCoverId = "<%=btnSubmitCover.ClientID%>";
            var btnSubmitId = "<%=btnSubmit.ClientID%>";
            var messageConfirm = '<%=GetLocalResourceObject("scriptMessageConfirm")%>'
            var messageInvalidRedistribute = '<%=GetLocalResourceObject("scriptMessageInvalidRedistribute")%>';
            var modalConfirm = '<%=Resources.ValMsg.modalConfirm %>';
            var modalSuccess = '<%=Resources.ValMsg.modalSucess %>';
            var modalWarning = '<%=Resources.ValMsg.modalWarning %>';
        </script>
        <script src="<%= ResolveUrl("~/")%>res/js/risk_MCF/rm_MCF_AutoQueueCustomListBox.js"></script>
        <script src="<%= ResolveUrl("~/")%>res/js/risk_MCF/rm_MCF_RedistributeWorkQueueModal.js"></script>
    </as:ASRadCodeBlock>
</asp:Content>
