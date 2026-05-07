<%@ Control Language="C#" AutoEventWireup="true" CodeFile="RiskReassignUser.ascx.cs"
    Inherits="UserControls_RiskReassignUser" %>

<div id="dvOuter" runat="server" class="text-center">
    <div class="risk-form text-right">
        <div class="risk-form-row">
            <as:ValidatorLabel ID="uxUserListLabel" runat="server" Text="User Name:" ApplyFor="uxUserList" CssClass="control-label" meta:resourcekey="uxUserListLabelResource1" />
            <asp:Label ID="uxUserName" runat="server" Visible="False" Width="303px" CssClass="reassignment-username" meta:resourcekey="uxUserNameResource1" />
            <div class="control-inline">
                <as:RadComboBox ID="uxUserList" runat="server" AutoPostBack="true" DataTextField="UserText" DataValueField="UserID"
                    OnSelectedIndexChanged="uxUserList_SelectedIndexChanged" OnClientSelectedIndexChanged="LoadReassignList" Width="295px" />
                <div class="label-error-left">
                    <as:ValidatorMessage ID="uxUserListErrMsg" runat="server" ApplyFor="uxUserList" Message="" ShowOnLoad="False"></as:ValidatorMessage>
                </div>
            </div>
        </div>
        <div class="risk-form-row">
            <as:ValidatorLabel ID="uxReassignListlabel" runat="server" Text="Reassign To:" ApplyFor="uxReassignList" CssClass="control-label" meta:resourcekey="uxReassignListLabelResource1" />
            <div class="control-inline">
                <as:RadComboBox ID="uxReassignList" runat="server" DataTextField="UserText" DataValueField="UserID" Width="295px" />
                <div class="label-error-left">
                    <as:ValidatorMessage ID="uxReassignListErrMsg" runat="server" ApplyFor="uxReassignList" Message="" ShowOnLoad="False"></as:ValidatorMessage>
                </div>
            </div>
        </div>
        <div class="risk-form-row pos-relative">
            <as:ValidatorLabel ID="uxSDateLabel" runat="server" Text="From:" ApplyFor="uxSDate" CssClass="control-label" meta:resourcekey="uxSDateLabelResource1" />
            <div class="control-inline valign-top">
                <as:RadDatePicker ID="uxSDate" Style="vertical-align: middle;"
                    runat="server" Skin="Default" Width="126px" meta:resourcekey="uxReportDateResource1">
                    <calendar id="Calendar2" fastnavigationstep="12" showrowheaders="false" runat="server">
                            </calendar>
                    <dateinput id="DateInput1" runat="server" onclick="ShowCalendar(this)" onkeypress="return DefaultEnterOnTextBox(event);" />
                </as:RadDatePicker>
                <div class="bottom-error text-left">
                    <as:ValidatorMessage ID="uxSDateErrMsg" runat="server" ApplyFor="uxSDate" Message="" ShowOnLoad="False"></as:ValidatorMessage>
                </div>
            </div>
            <as:ValidatorLabel ID="uxEDateLabel" CssClass="control-inline-label" runat="server" Text="To:" ApplyFor="uxEDate" meta:resourcekey="uxEDateLabelResource1" />
            <div class="control-inline">
                <as:RadDatePicker ID="uxEDate" runat="server" Width="127px" meta:resourcekey="uxReportDateResource1">
                    <calendar id="Calendar1" fastnavigationstep="12" showrowheaders="false" runat="server">
                            </calendar>
                    <dateinput id="DateInput2" runat="server" onclick="ShowCalendar(this)" onkeypress="return DefaultEnterOnTextBox(event);" />
                </as:RadDatePicker>
                <asp:HiddenField ID="uxBothDate" runat="server" />
                <div class="label-error-left">
                    <as:ValidatorMessage ID="ValidatorMessage1" runat="server" ApplyFor="uxBothDate" Message="" ShowOnLoad="False"></as:ValidatorMessage>
                </div>
                <div class="pos-relative" id="uxEDateErrMsgContainer">
                    <div class="bottom-error text-left pos-absolute text-nowrap">
                        <as:ValidatorMessage ID="uxEDateErrMsg" runat="server" ApplyFor="uxEDate" Message="" ShowOnLoad="False"></as:ValidatorMessage>
                    </div>
                </div>
            </div>
        </div>
        <div class="risk-form-action">
            <asp:Button runat="server" ID="uxUpdate" Text="Submit" CommandName="Update" CssClass="btn btn-default" OnClick="uxSubmit_Click" meta:resourcekey="uxUpdateResource1" />
            <asp:Button runat="server" ID="uxCancel" CommandName="Cancel" Text="Cancel" CausesValidation="False" CssClass="btn btn-default" OnClick="uxCancel_Click" meta:resourcekey="uxCancelResource1" />
        </div>
    </div>
</div>
<as:Validator ID="Validator1" runat="server" ValidationFunction="doValidationUserListUpdateMode" MessageType="Inline" MessageContainerClientID="" meta:resourcekey="Validator1Resource1">
    <items>
        <as:CustomValidationItem ControlToValidateID="uxReassignList" ClientValidationFunction="CheckAssignUserUpdate" ResMessage="Resources.UserMaintenanceMessage.ReassignUser_ReassignToEqualUserName" />
    </items>
</as:Validator>
<as:Validator ID="Validator2" runat="server" ValidationFunction="doValidationUserListCreateMode" MessageType="Inline" MessageContainerClientID="" meta:resourcekey="Validator2Resource1">
    <items>
        <as:CustomValidationItem ControlToValidateID="uxUserList" ClientValidationFunction="CheckUserListRequiredCreate" ResMessage="Resources.UserMaintenanceMessage.Required" />
        <as:CustomValidationItem ControlToValidateID="uxReassignList" ClientValidationFunction="CheckAssignUserCreate" ResMessage="Resources.UserMaintenanceMessage.ReassignUser_ReassignToEqualUserName" />
    </items>
</as:Validator>
<as:Validator ID="ctrlValidator" runat="server" ValidationFunction="doValidation" MessageType="Inline" MessageContainerClientID="" meta:resourcekey="ctrlValidatorResource1">
    <items>
        <as:CustomValidationItem ControlToValidateID="uxReassignList" ClientValidationFunction="CheckReassignListRequired" ResMessage="Resources.UserMaintenanceMessage.Required" />
        <as:CustomValidationItem ControlToValidateID="uxSDate" ClientValidationFunction="CheckStartDate_Required" ResMessage="Resources.UserMaintenanceMessage.Required" />
        <as:CustomValidationItem ControlToValidateID="uxEDate" ClientValidationFunction="CheckEndDate_Required" ResMessage="Resources.UserMaintenanceMessage.Required" />
        <as:CustomValidationItem ControlToValidateID="uxSDate" ClientValidationFunction="CheckStartDate" ResMessage="Resources.UserMaintenanceMessage.ReassignUser_InvalidDate" />
        <as:CustomValidationItem ControlToValidateID="uxEDate" ClientValidationFunction="CheckEndDate" ResMessage="Resources.UserMaintenanceMessage.ReassignUser_InvalidDate" />
        <as:CustomValidationItem ControlToValidateID="uxBothDate" ClientValidationFunction="validateDatePast" ResMessage="Resources.UserMaintenanceMessage.ReassignUser_PastDate" />
        <as:CustomValidationItem ControlToValidateID="uxEDate" ClientValidationFunction="CompareStartDate_EndDate" ResMessage="Resources.UserMaintenanceMessage.ReassignUser_FromDateGreaterToDate" />
    </items>
</as:Validator>
<as:Validator ID="Validator3" runat="server" ValidationFunction="doValidationStartDate" MessageType="Inline" MessageContainerClientID="" meta:resourcekey="ctrlValidatorResource1">
    <Items>
       <as:CustomValidationItem ControlToValidateID="uxSDate" ClientValidationFunction="CheckStartDate" ResMessage="Resources.UserMaintenanceMessage.ReassignUser_InvalidDate" />
    </Items>
</as:Validator>
<as:Validator ID="Validator4" runat="server" ValidationFunction="doValidationEndDate" MessageType="Inline" MessageContainerClientID="" meta:resourcekey="ctrlValidatorResource1">
    <Items>
        <as:CustomValidationItem ControlToValidateID="uxEDate" ClientValidationFunction="CheckEndDate" ResMessage="Resources.UserMaintenanceMessage.ReassignUser_InvalidDate" />
    </Items>
</as:Validator>

<as:RadAjaxManagerProxy runat="server" ID="uxRadAjaxManager1">
    <ajaxsettings>
        <tek:AjaxSetting AjaxControlID="uxUserName">
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="uxReassignList" />
            </UpdatedControls>
        </tek:AjaxSetting>
    </ajaxsettings>
</as:RadAjaxManagerProxy>

<as:RadCodeBlock runat="server" ID="RadCodeBlock1">
    <script type="text/javascript">
        var RiskReassignUser_uxSDate_ClientID = '<%= uxSDate.ClientID%>';
        var RiskReassignUser_uxEDate_ClientID = '<%= uxEDate.ClientID%>';
        var RiskReassignUser_uxUserList_ClientID = '<%= uxUserList.ClientID%>';
        var RiskReassignUser_uxReassignList_ClientID = '<%= uxReassignList.ClientID%>';
        var RiskReassignUser_uxUserName_ClientID = '<%= uxUserName.ClientID%>';
        var RiskReassignUser_uxUpdate_ClientID = '<%= uxUpdate.ClientID %>';   
    </script>
    <script type="text/javascript" src="<%= ResolveUrl("~/")%>res/js/usermaintenance/RiskReassignUser.js"></script>
</as:RadCodeBlock>
