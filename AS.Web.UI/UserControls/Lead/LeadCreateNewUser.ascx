<%@ Control Language="C#" AutoEventWireup="true" CodeFile="LeadCreateNewUser.ascx.cs"
    Inherits="UserControls_Lead_LeadCreateNewUser" %>

<!-- Rad Ajax-->
<tek:RadAjaxManagerProxy ID="RadAjaxManager1" runat="server">
    <AjaxSettings>
        <tek:AjaxSetting AjaxControlID="uxRole">
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="create_user_panel" LoadingPanelID="uxInvisiblePanel" />
                <tek:AjaxUpdatedControl ControlID="lblBankLabel" LoadingPanelID="uxInvisiblePanel" />
                <tek:AjaxUpdatedControl ControlID="uxAFDocRepoAccess" LoadingPanelID="uxInvisiblePanel" />
                <tek:AjaxUpdatedControl ControlID="uxAFLeadAccess" LoadingPanelID="uxInvisiblePanel" />
            </UpdatedControls>
        </tek:AjaxSetting>
    </AjaxSettings>
</tek:RadAjaxManagerProxy>

<tr id="uxtrDocRepoAccess" runat="server">
    <td class="heading">
        <asp:Literal ID="Literal16" runat="server" Text="Document Repository Access Functions:" meta:resourcekey="AccessFuntionControlGroupFuncLabel_DocRepoAccess" />
    </td>
    <td>
        <as:CheckBoxList ID="uxAFDocRepoAccess" CssClass="checkbox-list checkbox-list-span" runat="server"
            DataTextField="Description" DataValueField="PermissionId" meta:resourcekey="uxAccessListFunctionResource1">
        </as:CheckBoxList>
    </td>
</tr>

<tr id="uxtrLeadAccess" runat="server">
    <td class="heading">
        <asp:Literal ID="Literal1" runat="server" Text="Leads Access Functions:" meta:resourcekey="AccessFuntionControlGroupFuncLabel_LeadAccess" />
    </td>
    <td>
        <as:CheckBoxList ID="uxAFLeadAccess" CssClass="checkbox-list checkbox-list-span" runat="server"
            DataTextField="Description" DataValueField="PermissionId" meta:resourcekey="uxAccessListFunctionResource1">
        </as:CheckBoxList>
    </td>
</tr>


<tr runat="server" id="TIB_TR_Bank">
    <td class="heading">
        <as:ValidatorLabel ID="lblBankLabel" runat="server" Text="Bank" ApplyFor="hdfSelectedBank" CssClass="control-label" meta:resourcekey="Label_Bank" />
    </td>
    <td>

        <asp:Panel ID="create_user_panel" runat="server">
           
                <div class="w-30 inline-block">
                    <as:HiddenField ID="hdfSelectedBank" runat="server" ClientIDMode="Static" />
                    <as:LinkButton runat="server" ID="uxAddBank"  Text="Add Bank" meta:resourcekey="Hyperlink_Add_Bank"></as:LinkButton>
                    <as:LinkButton runat="server" ID="uxViewBank"  Text="View Bank" meta:resourcekey="Hyperlink_View_Bank"  js-ignore-disable-rule=""></as:LinkButton>

                </div>
                <div class="w-30 inline-block">
                    <asp:Label ID="LiteralCountSelectedBank" runat="server" ClientIDMode="Static" />
                    <asp:Label ID="lbCountSelectedBank" Text=" Bank(s) Added" runat="server" meta:resourcekey="Lablel_Bank_Selected" />
                </div>
                <div class=" w-30 inline-block">
                    <as:ValidatorMessage runat="server" ID="hdfSelectedBankErrMsg"  ApplyFor="hdfSelectedBank" Message="This is a required field." ShowOnLoad="False" meta:resourcekey="Validator_Bank" />
                </div>
           
        </asp:Panel>
    </td>
</tr>

<as:Validator ID="Validator_Bank" Visible="false" runat="server" ValidationFunction="ValidateInputTIB_Bank" MessageContainerClientID="" MessageType="Inline">
    <Items>
        <as:CustomValidationItem IsInAjaxPanel="true" ControlToValidateID="hdfSelectedBank" ClientValidationFunction="ValidationBankAssgined" Message="This is a required field." meta:resourcekey="Validator_Bank" />
    </Items>
</as:Validator>
<as:Validator ID="Validator_Branch" Visible="false" runat="server" ValidationFunction="ValidateInputTIB_Branch" MessageContainerClientID="" MessageType="Inline">
    <Items>
        <as:CustomValidationItem IsInAjaxPanel="true" ControlToValidateID="hdfSelectedBank" ClientValidationFunction="ValidationBankAssgined" Message="This is a required field." meta:resourcekey="Validator_Branch" />
    </Items>
</as:Validator>
<tek:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <script type="text/javascript">

        var SelectedBank_ClientID = '<% = hdfSelectedBank.ClientID%>';
    </script>
    <script type="text/javascript" src="<%= ResolveUrl("~/") %>res/js/lead/LeadCreateNewUser.js"></script>
</tek:RadCodeBlock>
