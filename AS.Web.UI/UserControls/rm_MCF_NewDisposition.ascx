<%@ Control Language="C#" AutoEventWireup="true" CodeFile="rm_MCF_NewDisposition.ascx.cs" Inherits="UserControls_rm_MCF_NewDisposition" %>

<as:Container ID="Container1" runat="server" Width="100%" FooterControlID="" FooterText="" HeaderControlID="" HeaderText="">
    <as:RadAjaxManagerProxy ID="RadAjaxManager1" runat="server">
        <AjaxSettings>
            <tek:AjaxSetting AjaxControlID="uxSubmit">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="txtNameErrMsg" />
                    <tek:AjaxUpdatedControl ControlID="uxDefaultMessage" />
                </UpdatedControls>
            </tek:AjaxSetting>
        </AjaxSettings>
    </as:RadAjaxManagerProxy>
    <div class="row create-disposition">
        <div class="col-md-12">
            <div class="disposition-name display-flex space-between">
                <asp:Literal ID="uxTextName" runat="server"></asp:Literal>
                <as:ValidatorLabel ID="txtNameLabel" runat="server" Text="Name:" ApplyFor="uxDispositionName" meta:resourcekey="txtNameLabelResource1" />

                <div class="disposition-form">
                    <as:TextBox ID="uxDispositionName" CssClass="form-control" ReadOnly="false" runat="server"
                        Width="100%" MaxLength="50" />

                    <div class="bottom-error">
                        <as:ValidatorMessage runat="server" ID="txtNameErrMsg" ApplyFor="uxDispositionName" />
                    </div>
                </div>
            </div>

            <div class="disposition-status display-flex space-between">
                <div class="control-inline">
                    <label class="first">
                        <as:Literal ID="ltStatus" runat="server" Text="Status:" meta:resourcekey="ltStatusResource1"></as:Literal></label>
                </div>
                <div class="disposition-form">
                    <div class="control-inline">
                        <as:RadioButton ID="uxRabAction" runat="server" GroupName="StatusOpts" Text="Active" Checked="True"
                            AutoPostBack="false" meta:resourcekey="uxActiveResource1" Value="Active" />
                    </div>
                    <div class="control-inline">
                        <as:RadioButton ID="uxRabInactive" runat="server" GroupName="StatusOpts" Text="Inactive"
                            AutoPostBack="false" meta:resourcekey="uxInactiveResource1" Value="Inactive" />
                    </div>
                </div>
            </div>

            <div class="disposition-default display-flex space-between">
                <div class="control-inline">
                    <label class="first">
                        <as:Literal ID="Literal1" runat="server" Text="Status:" meta:resourcekey="ltDefaultResource1"></as:Literal></label>
                </div>
                <div class="disposition-form">
                    <div class="control-inline">
                        <as:RadioButton ID="uxRabDefault" runat="server" GroupName="DefaultOpts" Text="Yes"
                            AutoPostBack="false" meta:resourcekey="uxDefaultResource1" Value="Yes" />
                    </div>
                    <div class="control-inline">
                        <as:RadioButton ID="uxRabNotDefault" runat="server" GroupName="DefaultOpts" Text="No" Checked="True"
                            AutoPostBack="false" meta:resourcekey="uxNotDefaultResource1" Value="No" />
                    </div>
                    <div class="bottom-error">
                        <as:ValidatorMessage runat="server" ID="uxDefaultMessage" ApplyFor="Literal1" />
                    </div>
                </div>

            </div>
        </div>
        <div class="height-24"></div>
        <div class="col-md-12">
            <div class="display-flex space-between">
                <div class="errors">
                    <asp:Label ID="ucRequireMsg" runat="server" Text="* Required field" meta:resourcekey="txtRequiredResource1"></asp:Label>
                </div>
                <div class="form-action-container">
                    <asp:Button ID="uxSubmit" meta:resourcekey="uxSubmitResource" runat="server" class="btn btn-default no-margin-top"
                        OnClientClick="if(!submitClick()){ return false;}" OnClick="uxSubmit_Click" Text="Submit" />
                    <asp:Button ID="uxCancel" meta:resourcekey="uxCancelResource" OnClientClick="ClosePopupModal(); return false;"
                        class="btn btn-default no-margin-top no-margin-right" runat="server" Text="Cancel"
                        CausesValidation="false" UseSubmitBehavior="false"></asp:Button>
                </div>
            </div>
        </div>
    </div>
    <as:Validator ID="Validator1" runat="server" ValidationFunction="ValidateInput" MessageType="Inline" MessageContainerClientID="" meta:resourcekey="Validator1Resource1">
        <Items>
            <as:BasicValidationItem ControlToValidateID="uxDispositionName" Rule="Required" ResMessage="Resources.RiskMessageManager.Disposition_Name_Required" />
            <as:CustomValidationItem ClientValidationFunction="ValidateSpecialCharacters" ControlToValidateID="uxDispositionName" ResMessage="Resources.RiskMessageManager.Disposition_GeneralInformation_SpecialCharacters" />
        </Items>
    </as:Validator>

    <tek:RadCodeBlock runat="server" ID="RadCodeBlock2">
        <script>
            uxDispositionName_ClientID = '<%= uxDispositionName.ClientID%>';
        </script>
        <script type="text/javascript" src="<%=ResolveUrl("~") %>res/js/risk_MCF/rm_MCF_NewDisposition.js"></script>
    </tek:RadCodeBlock>
</as:Container>

