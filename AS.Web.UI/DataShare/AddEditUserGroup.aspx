<%@ Page Language="C#" MasterPageFile="~/MasterPagePopup.master" AutoEventWireup="true" CodeFile="AddEditUserGroup.aspx.cs" Inherits="AddEditUserGroup" Title="Untitled Page" Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="Server">
    <as:ASModalContainer ID="uxModalContainer" runat="server" WidthCssClass="modal-xxl" Width="800px" ContainerCssClass="container">
        <div class="row">
            <div class="col-xs-12">
                <table class="ASTable">
                    <tbody>
                        <tr class="Row">
                            <td class="heading" style="width: 130px">
                                <as:ValidatorLabel ID="uxlbUserGroup" runat="server" ApplyFor="uxUserGroup" Text="User Group:" meta:resourcekey="uxlbUserGroupResource" />
                            </td>
                            <td class="control">
                                <div class="control-inline last">
                                    <as:TextBox ID="uxUserGroup" CssClass="form-control" Style="width: 300px;" MaxLength="250" runat="server" />
                                </div>
                                <as:ValidatorMessage ID="uxUserGroupMsg" runat="server" ApplyFor="uxUserGroup" />
                            </td>
                        </tr>
                        <tr class="AltRow">
                            <td class="heading">
                                <as:ValidatorLabel ID="uxlbDes" runat="server" ApplyFor="uxDescription" Text="Description:" meta:resourcekey="uxlbDescriptionResource" />
                                <td class="control">
                                    <div class="control-inline last">
                                        <as:TextBox ID="uxDescription" CssClass="form-control" runat="server" TextMode="MultiLine" MaxLength="1000" Style="width: 300px; height: 100px;" meta:resourcekey="uxDescriptionResource1" />
                                    </div>
                                    <as:ValidatorMessage ID="ValidatorMessage1" runat="server" ApplyFor="uxDescription" />
                                </td>
                        </tr>
                        <tr class="AltRow">
                            <td class="heading">
                                <as:Literal ID="ltDocument3" runat="server" Text="Active:" meta:resourcekey="ltDocument3Resource1"></as:Literal></td>
                            <td class="control">
                                <as:RadioButton ID="uxActive" runat="server" Checked="true" GroupName="StatusActive" Text="Yes" meta:resourcekey="uxActiveResource1" />&nbsp;&nbsp;&nbsp;&nbsp;
                                <as:RadioButton ID="uxDeActive" runat="server" GroupName="StatusActive" Text="No" meta:resourcekey="uxDeActiveResource1" />
                            </td>
                        </tr>
                    </tbody>
                </table>
                <div class="row">
                    <div class="col-xs-12 form-action-container text-right">
                        <as:Button ID="uxSubmit" CssClass="btn btn-default" runat="server" Text="Submit" OnClientClick="return validateSubmit();" OnClick="uxSubmit_Click" meta:resourcekey="uxSubmitResource1" />
                        <as:Button ID="btnCancel" CssClass="btn btn-default" runat="server" OnClientClick="parent.HidePopupModal();" Text="Cancel" meta:resourcekey="btnCancelResource1" />
                    </div>
                </div>
            </div>
        </div>
    </as:ASModalContainer>
    <as:Validator ID="Validator1" runat="server" ValidationFunction="ValidaInput" IgnoreServerValidation="False" MessageContainerClientID="" meta:resourcekey="Validator1Resource1">
        <Items>
            <as:BasicValidationItem Rule="Required" ControlToValidateID="uxUserGroup" ResMessage="Resources.MessageManager.Field_RequireAndUnique" />
            <as:BasicValidationItem Rule="Maxlength" MaxLength="250" ControlToValidateID="uxUserGroup" ResMessage="Resources.ValMsg.MaxLength" ResParams="250" />
            <as:BasicValidationItem Rule="Maxlength" MaxLength="1000" ControlToValidateID="uxDescription" ResMessage="Resources.ValMsg.MaxLength" ResParams="1000" />
            <as:CustomValidationItem ControlToValidateID="uxUserGroup" IsInAjaxPanel="true" ClientValidationFunction="validateGroupSpecialCharacter" meta:resourcekey="ValidateGroupsSpecialCharacters" />
            <as:CustomValidationItem ControlToValidateID="uxDescription" IsInAjaxPanel="true" ClientValidationFunction="validateDesSpecialCharacter" meta:resourcekey="ValidateDesSpecialCharacters" />
        </Items>
    </as:Validator>
    <tek:RadCodeBlock runat="server" ID="RadCodeBlock">
        <script type="text/javascript">
            var uxUserGroup = '<%= uxUserGroup.ClientID %>'
            var uxDescription = '<%= uxDescription.ClientID %>'
        </script>
        <script type="text/javascript" src="<%= ResolveUrl("~/") %>res/js/datashare/addEditUserGroup.js"></script>
    </tek:RadCodeBlock>
</asp:Content>

