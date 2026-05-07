<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ActivationReportEditAmountModal.aspx.cs" Inherits="ActivationReportEditAmountModal"
    MasterPageFile="~/MasterPagePopup.master" meta:resourcekey="PageResource1" Title="Edit Minimum Batch Amount" %>


<asp:Content ID="Content3" ContentPlaceHolderID="ContentPage" runat="Server">
    <style type="text/css">
        #divBatchAmount label {
            display: inline-block;
            margin-left: 14px;
        }
        #divBatchAmount input {
            width: 375px;
        }
    </style>
    <as:ASModalContainer ID="ASModalContainer1" runat="server" WidthCssClass="modal-sm">
        <div class="row">
            <div class="col-xs-12">
                <as:Literal ID="ltEditAmoutInfo" runat="server" Text="Edit the minimum batch amount below. The updated batch amount will be applied when a new transaction file is sent." meta:resourcekey="ltEditAmoutInfoResources"></as:Literal>
            </div>
        </div>
        <div class="height-8"></div>
        <div class="row pos-relative" id="divBatchAmount">
            <div class="col-xs-12">
                <as:Literal ID="ltDolar" runat="server" Text="$"></as:Literal>
                <as:TextBox ID="uxAmount" MaxLength="11" onchange="uxAmount_OnKeyUp()" onkeyup="uxAmount_OnKeyUp(this)" CssClass="ml-2x" runat="server"></as:TextBox>
                <as:ValidatorMessage ID="uxAmountMsg" ApplyFor="uxAmount" runat="server"></as:ValidatorMessage>
            </div>
        </div>
        <div class="row">
            <div class="col-xs-12 form-action-container text-right">
                <as:Button class="btn btn-default" ID="uxApply" OnClick="uxApply_Click" Text="Apply" OnClientClick="return onUpdateAmount()" runat="server" meta:resourcekey="btnApplyResource" />
                <as:Button class="btn btn-default" ID="Button1" Text="Cancel" OnClientClick="ClosePopupModal();" runat="server" meta:resourcekey="bntCancelResource" />
            </div>
        </div>
        <as:Validator ID="Validator1" runat="server" ValidationFunction="ValidateInput" MessageType="Inline" MessageContainerClientID="" meta:resourcekey="Validator1Resource1">
            <Items>
                <as:BasicValidationItem ControlToValidateID="uxAmount" Rule="Required" ResMessage="Resources.ValMsg.Required" />
                <as:BasicValidationItem ControlToValidateID="uxAmount" Rule="Maxlength" MaxLength="11" ResMessage="Resources.ValMsg.MaxLength" ResParams="11" />
                <as:BasicValidationItem ControlToValidateID="uxAmount" Rule="Range" MinValue="0" MaxValue="99999999.99" ResMessage="Resources.ValMsg.MaxValueMsg"/>
            </Items>
        </as:Validator>
        <%--<div class="height-100"></div>--%>
    </as:ASModalContainer>
    <script>
        function onUpdateAmount() {
            if (!ValidateInput()) {
                AdjustModalSize();
                return false;
            }
            else
                parent.IsUpdateAmount = true;
            return true;
        }

        function uxAmount_OnKeyUp(e) {
            uxAmount_TextChange();
            var obj = $("#" + "<%= uxAmount.ClientID%>");
            var val = obj.val();
            var rexg = /(^(\d+)?$)|(^\d+[\.](\d+)?$)/gi;
            var isMatch = rexg.test(val);
            if (!isMatch) {
                obj.val(val.replace(val[val.length - 1], ""));
            }
            allow2Decimals(obj.val());
        }

        function uxAmount_TextChange()
        {
            var obj = $("#" + "<%= uxAmount.ClientID%>");
            var val = obj.val();
            var rexg = /^([0-9\.]+)/g;
            val = rexg.exec(val);
            if (val) {
                obj.val(val[0]);
            } else {
                obj.val("");
            }
        }

        function allow2Decimals(val)
        {
            var obj = $("#" + "<%= uxAmount.ClientID%>");
            var arr = val.split('.');
            if (arr.length > 2)
            {
                arr.pop();
            }
            val = arr.join('.');

            if (val.indexOf('.') > -1 && (val.split('.')[1]).length > 2)
            {
                val = val.substring(0, val.length - 1);
            }
            obj.val(val);
        }
    </script>
</asp:Content>
