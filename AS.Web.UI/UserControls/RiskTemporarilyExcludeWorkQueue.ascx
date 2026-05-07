<%@ Control Language="C#" AutoEventWireup="true" CodeFile="RiskTemporarilyExcludeWorkQueue.ascx.cs"
    Inherits="UserControls_RiskTemporarilyExcludeWorkQueue" %>

<div id="dvOuter" runat="server" class="text-center">
    <div class="risk-form form-horizontal text-left">
        <div class="form-group">
            <as:ValidatorLabel ID="uxExcludeWorkQueueLabel" runat="server" Text="Exclude Work Queue:" ApplyFor="uxExcludeWorkQueue"
                CssClass="control-label  col-xs-5" meta:resourcekey="uxExcludeWorkQueueLabelResource1" />
            <asp:Label ID="uxExcludeWorkQueueName" runat="server" Visible="False" CssClass="reassignment-username"
                meta:resourcekey="uxExcludeWorkQueueLabelResource1" />
            <div class="control-inline col-xs-7">
                <as:RadComboBox ID="uxExcludeWorkQueue" runat="server" AutoPostBack="true" DataTextField="AssignmentName" Width="100%"
                    DataValueField="AssignmentID" OnSelectedIndexChanged="uxExcludeWorkQueue_SelectedIndexChanged">
                </as:RadComboBox>
                <b>
                    <asp:Label ID="uxExcludeWorkQueueEdit" runat="server" CssClass="mt-1x display-block"></asp:Label>
                </b>
                <div class="label-error-left">
                    <as:ValidatorMessage ID="uxExcludeWorkQueueErrMsg" runat="server" ApplyFor="uxExcludeWorkQueue"
                        Message="" ShowOnLoad="False">
                    </as:ValidatorMessage>
                </div>
            </div>
        </div>
        <div class="form-group">
            <as:ValidatorLabel ID="uxAutoQueueLabel" runat="server" Text="Auto Queue Name:" ApplyFor="uxAutoQueueName"
                CssClass="control-label col-xs-5" meta:resourcekey="uxAutoQueueLabelResource1" />
            <div class="control-inline col-xs-7">
                <as:MultiChooser IsSearchContains="true" ID="uxAutoQueueName" runat="server" Width="100%" IsInTelerikAjax="true" Placeholder=" " />
                <div class="label-error-left">
                    <as:ValidatorMessage ID="uxAutoQueueNameErrMsg" runat="server" ApplyFor="uxAutoQueueName" Message="" ShowOnLoad="False">
                    </as:ValidatorMessage>
                </div>
            </div>
        </div>
        <div class="form-group from-to">
            <as:ValidatorLabel ID="uxSDateLabel" runat="server" Text="From:" ApplyFor="uxSDate" CssClass="control-label col-xs-5"
                meta:resourcekey="uxSDateLabelResource1" />
            <div class="control-inline col-xs-3">
                <as:RadDatePicker ID="uxSDate" Style="vertical-align: middle;" Width="100%"
                    runat="server" Skin="Default" meta:resourcekey="uxReportDateResource1">
                    <Calendar ID="Calendar2" FastNavigationStep="12" ShowRowHeaders="false" runat="server">
                    </Calendar>
                    <DateInput ID="DateInput1" runat="server" onclick="ShowCalendar(this)" onkeypress="return DefaultEnterOnTextBox(event);" />
                </as:RadDatePicker>

                <div class="pos-relative">
                    <div class="bottom-error text-left">
                        <as:ValidatorMessage ID="uxSDateErrMsg" runat="server" ApplyFor="uxSDate" Message="" ShowOnLoad="False">
                        </as:ValidatorMessage>
                    </div>
                </div>
            </div>
            <as:ValidatorLabel ID="uxEDateLabel" CssClass="control-inline-label col-xs-1 text-right" runat="server" Text="To:"
                ApplyFor="uxEDate" meta:resourcekey="uxEDateLabelResource1" />
            <div class="control-inline col-xs-3">
                <as:RadDatePicker ID="uxEDate" runat="server" meta:resourcekey="uxReportDateResource1" Width="100%">
                    <Calendar ID="Calendar1" FastNavigationStep="12" ShowRowHeaders="false" runat="server">
                    </Calendar>
                    <DateInput ID="DateInput2" runat="server" onclick="ShowCalendar(this)" onkeypress="return DefaultEnterOnTextBox(event);" />
                </as:RadDatePicker>
                <asp:HiddenField ID="uxBothDate" runat="server" />
                <div class="label-error-left">
                    <as:ValidatorMessage ID="ValidatorMessage1" runat="server" ApplyFor="uxBothDate" Message="" ShowOnLoad="False">
                    </as:ValidatorMessage>
                </div>
                <div class="pos-relative" id="uxEDateErrMsgContainer">
                    <div class="bottom-error text-left text-nowrap">
                        <as:ValidatorMessage ID="uxEDateErrMsg" runat="server" ApplyFor="uxEDate" Message="" ShowOnLoad="False">
                        </as:ValidatorMessage>
                    </div>
                </div>
            </div>
        </div>
        <div class="risk-form-action text-right">
            <div class="control-inline">
                <asp:Button runat="server" ID="uxUpdate" Text="Submit" CommandName="Update" CssClass="btn btn-default" 
                    meta:resourcekey="uxUpdateResource1" />
            </div>
            <asp:Button runat="server" ID="uxCancel" CommandName="Cancel" Text="Cancel" CausesValidation="False" CssClass="btn btn-default"
                OnClick="uxCancel_Click" meta:resourcekey="uxCancelResource1" />
            <asp:Button runat="server" ID="uxCreate" Text="Submit" CssClass="hide" OnClick="uxSubmit_Click" />
        </div>
    </div>
</div>

<as:Validator ID="ctrlValidator" runat="server" ValidationFunction="doValidation" MessageType="Inline" MessageContainerClientID=""
    meta:resourcekey="ctrlValidatorResource1">
    <Items>
        <as:CustomValidationItem ControlToValidateID="uxExcludeWorkQueue" ClientValidationFunction="CheckExcludeWorkQueueRequired"
            ResMessage="Resources.UserMaintenanceMessage.Required" />
        <as:CustomValidationItem ControlToValidateID="uxAutoQueueName" ClientValidationFunction="CheckAutoQueueNameRequired"
            ResMessage="Resources.UserMaintenanceMessage.Required" IsInAjaxPanel="true" />
        <as:CustomValidationItem ControlToValidateID="uxSDate" ClientValidationFunction="CheckStartDate_Required"
            ResMessage="Resources.UserMaintenanceMessage.Required" />
        <as:CustomValidationItem ControlToValidateID="uxEDate" ClientValidationFunction="CheckEndDate_Required"
            ResMessage="Resources.UserMaintenanceMessage.Required" />
        <as:CustomValidationItem ControlToValidateID="uxSDate" ClientValidationFunction="CheckStartDate"
            ResMessage="Resources.UserMaintenanceMessage.TemporaryExcludeWorkQueue_InvalidDate" />
        <as:CustomValidationItem ControlToValidateID="uxSDate" ClientValidationFunction="ValidateFromDate_Past"
            ResMessage="Resources.UserMaintenanceMessage.TemporaryExcludeWorkQueue_PastDate" />
        <as:CustomValidationItem ControlToValidateID="uxEDate" ClientValidationFunction="CheckEndDate"
            ResMessage="Resources.UserMaintenanceMessage.TemporaryExcludeWorkQueue_InvalidDate" />
        <as:CustomValidationItem ControlToValidateID="uxEDate" ClientValidationFunction="CompareStartDate_EndDate"
            ResMessage="Resources.UserMaintenanceMessage.TemporaryExcludeWorkQueue_FromDateGreaterToDate" />
        <as:CustomValidationItem ControlToValidateID="uxEDate" ClientValidationFunction="ValidateToDate_Past"
            ResMessage="Resources.UserMaintenanceMessage.TemporaryExcludeWorkQueue_CurrentAndPastDate" />
    </Items>
</as:Validator>
<as:Validator ID="Validator1" runat="server" ValidationFunction="doValidationStartDate" MessageType="Inline" MessageContainerClientID="" meta:resourcekey="ctrlValidatorResource1">
    <Items>
        <as:CustomValidationItem ControlToValidateID="uxSDate" ClientValidationFunction="CheckStartDate"
            ResMessage="Resources.UserMaintenanceMessage.TemporaryExcludeWorkQueue_InvalidDate" />
    </Items>
</as:Validator>
<as:Validator ID="Validator2" runat="server" ValidationFunction="doValidationEndDate" MessageType="Inline" MessageContainerClientID="" meta:resourcekey="ctrlValidatorResource1">
    <Items>
        <as:CustomValidationItem ControlToValidateID="uxEDate" ClientValidationFunction="CheckEndDate"
            ResMessage="Resources.UserMaintenanceMessage.TemporaryExcludeWorkQueue_InvalidDate" />
    </Items>
</as:Validator>

<asp:HiddenField ID="hddAssignmentID" runat="server" />
<as:RadAjaxManagerProxy runat="server" ID="uxRadAjaxManager1">
    <AjaxSettings>
        <tek:AjaxSetting AjaxControlID="uxExcludeWorkQueue">
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="uxAutoQueueName" />
            </UpdatedControls>
        </tek:AjaxSetting>
    </AjaxSettings>
</as:RadAjaxManagerProxy>

<as:RadCodeBlock runat="server" ID="RadCodeBlock1">
    <script type="text/javascript">
        var TemporarilyExcludeWorkQueue_uxSDate_ClientID = '<%= uxSDate.ClientID%>';
        var TemporarilyExcludeWorkQueue_uxEDate_ClientID = '<%= uxEDate.ClientID%>';
        var TemporarilyExcludeWorkQueue_uxExcludeWorkQueue_ClientID = '<%= uxExcludeWorkQueue.ClientID%>';
        var TemporarilyExcludeWorkQueue_uxAutoQueueName_ClientID = '<%= uxAutoQueueName.ClientID%>';
        var TemporarilyExcludeWorkQueue_uxExcludeWorkQueueName_ClientID = '<%= uxExcludeWorkQueueName.ClientID%>';
        var TemporarilyExcludeWorkQueue_uxUpdate_ClientID = '<%= uxUpdate.ClientID %>';
        var TemporarilyExcludeWorkQueue_uxCreate_ClientID = '<%= uxCreate.ClientID %>';
        var modalTitleConfirm = '<%=Resources.ValMsg.modalConfirm %>';
    </script>
    <script type="text/javascript" src="<%= ResolveUrl("~/")%>res/js/usermaintenance/RiskTemporarilyExcludeWorkQueue.js"></script>
</as:RadCodeBlock>
