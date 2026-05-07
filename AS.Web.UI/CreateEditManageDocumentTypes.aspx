<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPagePopup.master" AutoEventWireup="true" CodeFile="CreateEditManageDocumentTypes.aspx.cs" Inherits="CreateEditManageDocumentTypes" meta:resourcekey="pageTitleresource" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="Server">
    <as:ASModalContainer ID="uxModalContainer" runat="server" WidthCssClass="modal-xxl" Width="800px" ContainerCssClass="container">
        <div class="row">
            <div class="col-xs-12">
                <table class="ASTable form-inline">
                    <colgroup>
                        <col style="width:150px;" />
                        <col/>
                    </colgroup>
                    <tbody>
                        <tr id="uxRowSource" class="RawRow" runat="server">
                            <td class="heading" style="width: 150px;">
                                <as:ValidatorLabel runat="server" ApplyFor="uxSource" Text="Source:" meta:resourcekey="uxuxSourceLbl" />
                            </td>
                            <td class="control">
                                <div class="control-inline last">
                                    <as:MultiChooser runat="server" ID="uxSource" Width="350" Placeholder="Select a source" meta:resourcekey="uxSourceMultiChooserResource"></as:MultiChooser>
                                    <asp:Literal ID="uxSourceName" runat="server" Visible="false" />
                                    <asp:HiddenField ID="uxDocumentTypeID" runat="server" />
                                </div>
                                <as:ValidatorMessage runat="server" ID="uxSourceMsg" ApplyFor="uxSource" />
                            </td>
                        </tr>
                        <tr class="RawRow">
                            <td class="heading">
                                <as:ValidatorLabel ID="uxlbuxDocumentType" runat="server" ApplyFor="uxDocumentTypeName" Text="Document Type:" meta:resourcekey="uxlbDocumentTypeResource" />
                            </td>
                            <td class="control">
                                <div class="control-inline last">
                                    <as:TextBox ID="uxDocumentTypeName" CssClass="form-control" Style="width: 350px;" MaxLength="250" runat="server" onblur="onDocumentTypeBlur(this, event)" />
                                </div>
                                <as:ValidatorMessage ID="uxDocumentTypeMsg" runat="server" ApplyFor="uxDocumentTypeName" />
                            </td>
                        </tr>
                        <tr class="RawRow">
                            <td class="heading">
                                <as:ValidatorLabel ID="uxDescriptionID" runat="server" ApplyFor="uxDescription" Text="Description:" meta:resourcekey="uxDescriptionIDResource" />
                            </td>
                            <td class="control">
                                <as:TextBox ID="uxDescription" CssClass="form-control" runat="server" TextMode="MultiLine" MaxLength="1000" Style="width: 350px; height: 100px;" 
                                    meta:resourcekey="uxDescriptionResource1" onblur="onDescriptionBlur(this, event)" />
                                <as:ValidatorMessage ID="uxDescriptionMsg" runat="server" ApplyFor="uxDescription" />
                            </td>
                        </tr>
                        <tr class="RawRow">
                            <td class="heading">
                                <as:Literal ID="ltDocument1" runat="server" Text="Active:" meta:resourcekey="ltActiveResource1"></as:Literal></td>
                            <td class="control">
                                <as:RadioButton ID="uxActive" runat="server" Checked="true" GroupName="StatusActive" Text="Yes" meta:resourcekey="uxActiveResource1" />&nbsp;&nbsp;&nbsp;&nbsp;
                                <as:RadioButton ID="uxDeActive" runat="server" GroupName="StatusActive" Text="No" meta:resourcekey="uxDeActiveResource1" />
                            </td>
                        </tr>
                    </tbody>
                </table>

                <div class="row">
                    <div class="col-xs-12 form-action-container text-right">
                        <as:Button ID="uxSubmit" CssClass="btn btn-default" runat="server" OnClick="uxSubmit_Click" Text="Submit" OnClientClick="return doValidaInput();" meta:resourcekey="uxSubmitResource1" />
                        <as:Button ID="btnCancel" CssClass="btn btn-default" runat="server" OnClientClick="parent.DoClose(); return false;" Text="Cancel" meta:resourcekey="btnCancelResource1" />
                    </div>
                </div>

            </div>
        </div>
    </as:ASModalContainer>
    <as:Validator ID="uxValidator" runat="server" MessageType="Inline" ValidationFunction="ValidaInput" IgnoreServerValidation="False" MessageContainerClientID="" meta:resourcekey="Validator1Resource1">
        <Items>
            <as:BasicValidationItem Rule="Required" ControlToValidateID="uxSource" ResMessage="Resources.MessageManager.RiskParameter_js_RequiredField" />
            <as:BasicValidationItem Rule="Required" ControlToValidateID="uxDocumentTypeName" ResMessage="Resources.MessageManager.RiskParameter_js_RequiredField" />
            <as:BasicValidationItem Rule="Maxlength" MaxLength="1000" ControlToValidateID="uxDescription" meta:resourcekey="Notexceed" />
            <as:RegExValidationItem ControlToValidateID="uxDocumentTypeName" RegularExpression="^((?!(<[^ \t]))(?!(&#)).)*$" ResMessage="Resources.LanguageResource.AS_CommonJS_Msg_IncludedSpeciaCharacters" />
        </Items>
    </as:Validator>
    <as:Validator ID="Validator1" runat="server" ValidationFunction="ValidateSpecialCharactersDocumentType" MessageContainerClientID="" MessageType="Inline">
        <Items>
            <as:RegExValidationItem ControlToValidateID="uxDocumentTypeName" RegularExpression="^((?!(<[^ \t]))(?!(&#)).)*$" 
                ResMessage="Resources.LanguageResource.AS_CommonJS_Msg_IncludedSpeciaCharacters" />
        </Items>
    </as:Validator>
    <as:Validator ID="Validator2" runat="server" ValidationFunction="ValidateSpecialCharactersDescription" MessageContainerClientID="" MessageType="Inline">
        <Items>
          <as:CustomValidationItem ClientValidationFunction="validateDescription" ControlToValidateID="uxDescription" ResMessage="Resources.LanguageResource.AS_CommonJS_Msg_IncludedSpeciaCharacters" />
        </Items>
    </as:Validator>

    <script type="text/javascript"> 
        var uxDocumentTypeNameId = "<%=uxDocumentTypeName.ClientID%>";

        $().ready(function () {
            $('.RawRow:even').addClass('AltRow');
            $('.RawRow:odd').addClass('Row');
        });

        function doValidaInput() {
            if (isCancelSubmit) {
                return false;
            }
            if (isIncludeSpecialCharacters(document.getElementById(uxDocumentTypeNameId))) {
                $('#' + uxDocumentTypeNameId).trigger('blur');
                return false;
            }
            return ValidaInput() && ValidateSpecialCharactersDescription();
        }

        var isCancelSubmit = false;
        function onDocumentTypeBlur(element, e) {
            var ignore = e.relatedTarget || e.rangeParent;
            if (ignore == undefined) {
                if (e.originalEvent != undefined)
                    ignore = e.originalEvent.explicitOriginalTarget;
            }
            if (ValidateSpecialCharactersDocumentType() == false || isIncludeSpecialCharacters(element)) {
                isCancelSubmit = true;
                removeSpecialCharacters(element);
                setTimeout('isCancelSubmit = false;', 500);
                if (ignore != null) {
                    handleClickElement(ignore);
                }
            }
        }

        function onDescriptionBlur(element, e) {
            var ignore = e.relatedTarget || e.rangeParent;
            if (ignore == undefined) {
                if (e.originalEvent != undefined)
                    ignore = e.originalEvent.explicitOriginalTarget;
            }

            if (!ValidateSpecialCharactersDescription()) {
                isCancelSubmit = true;
                removeSpecialCharacters(element);
                setTimeout('isCancelSubmit = false;', 500);
                if (ignore != null) {
                    handleClickElement(ignore);
                }
            }
        }

        function validateDescription(ele) {
            return !isIncludeSpecialCharacters(document.getElementById(ele));
        }

        function handleClickElement(element) {
            if ($(element).val() == 'Cancel') {
                $(element).click();
            } else if ($(element).val() == 'uxActive' || $(element).val() == 'uxDeActive') {
                element.checked = true;
            }
        }

    </script>
</asp:Content>

