<%@ Page Title="Manage Theme" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="ManageTheme.aspx.cs" Inherits="ManageTheme" %>

<%@ Register Src="~/UserControls/ColorPicker.ascx" TagName="ColorPicker" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPage" runat="Server">
    <style type="text/css">
        .table-branding-theme
        {
            width: 100%;
        }
        .table-branding-theme td
        {
            padding: 3px;
        }
        .table-branding-theme td:first-child
        {
            width: 250px;
            text-align: right;
            padding-right: 10px;
            vertical-align: middle;
        }
        .RadUpload .ruFakeInput
        {
            height: 22px !important;
            width: 211px !important;
            border-color: #999999 !important;
        }
        .ruBrowse
        {
            margin-top: 2px !important;
        }
    </style>
    <as:Validator ID="uxValdator" MessageType="AlertBox" ValidationFunction="FormValidate"
        runat="server">
        <Items>
            <as:BasicValidationItem Rule="Required" Message="Theme Name: This is a required field."
                ControlToValidateID="uxThemeName" />
            <as:BasicValidationItem Rule="Required" Message="Theme Name: This is a required field."
                ControlToValidateID="uxThemeList" />
        </Items>
    </as:Validator>
    <div style="width: 900px; margin-top: 15px;">
        <div style="margin-bottom: 15px; margin-top: 10px; text-align:center">
            <b>Status </b>
            <asp:RadioButton Checked="true" ID="uxRadioCreate" OnCheckedChanged="ChangeFormMode"
                AutoPostBack="true" Text="Create new theme" GroupName="Mode" runat="server" />
            <asp:RadioButton ID="uxRadioEdit" Text="Edit theme" OnCheckedChanged="ChangeFormMode"
                AutoPostBack="true" GroupName="Mode" runat="server" />
        </div>
        <table cellpadding="0" cellspacing="0" class="table-branding-theme">
            <tr>
                <td>
                    Theme Name <span class="RequireLabel">(*)</span>:
                </td>
                <td style="width: 290px">
                    <asp:TextBox ID="uxThemeName" runat="server" Width="220px" MaxLength="50" />
                    <div style="margin-left: 3px;">
                        <tek:RadComboBox ID="uxThemeList" AutoPostBack="true" OnSelectedIndexChanged="uxThemeList_SelectedIndexChanged"
                            Visible="false" Width="220px" runat="server" EnableEmbeddedBaseStylesheet="false"
                            EnableEmbeddedSkins="false">
                        </tek:RadComboBox>
                    </div>
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td>
                    Logo Image <span class="RequireLabel">(*)</span>:
                </td>
                <td>
                    <div class="TaxInputOutLine" style="width: 221px; height: 28px !important">
                        <tek:RadUpload ID="uxLogoImage" Width="270px" AllowedFileExtensions=".png,.jpg,.bmp"
                            runat="server" InitialFileInputsCount="1" ControlObjectsVisibility="None" Localization-Select="Browse">
                        </tek:RadUpload>
                    </div>
                </td>
                <td>
                    <span class="RequireLabel">(300px x 70px, *.png only)</span>
                </td>
            </tr>
            <tr>
                <td>
                    Statement Logo Image <span class="RequireLabel">(*)</span>:
                </td>
                <td>
                    <div class="TaxInputOutLine" style="width: 221px; height: 28px !important">
                        <tek:RadUpload ID="uxStatementLogoImage" Width="270px" AllowedFileExtensions=".png,.jpg,.bmp"
                            runat="server" InitialFileInputsCount="1" ControlObjectsVisibility="None" Localization-Select="Browse">
                        </tek:RadUpload>
                    </div>
                </td>
                <td>
                    <span class="RequireLabel">(169px x 41px, *.png, *.jpg, *.bmp only)</span>
                </td>
            </tr>
            <tr>
                <td colspan="3" style="text-align: left">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td>
                    Primary Color <span class="RequireLabel">(*)</span>:
                </td>
                <td>
                    <uc:ColorPicker ID="uxPrimaryColor" runat="server" />
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td>
                    Primary Text Color <span class="RequireLabel">(*)</span>:
                </td>
                <td>
                    <uc:ColorPicker ID="uxPrimaryTextColor" runat="server" DefaultColor="#FFFFFF" />
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td>
                    Secondary Color <span class="RequireLabel">(*)</span>:
                </td>
                <td>
                    <uc:ColorPicker ID="uxSecondaryColor" runat="server" />
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td colspan="3" style="text-align: left">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td>
                    Menu Mouse Hover Color <span class="RequireLabel">(*)</span>:
                </td>
                <td>
                    <uc:ColorPicker ID="uxMenuMouseHoverColor" runat="server" />
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td>
                    Menu Mouse Hover Sub Color <span class="RequireLabel">(*)</span>:
                </td>
                <td>
                    <uc:ColorPicker ID="uxMenuMouseHoverSubColor" runat="server" />
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td>
                    Menu Mouse Hover Text Color <span class="RequireLabel">(*)</span>:
                </td>
                <td>
                    <uc:ColorPicker ID="uxMenuMouseHoverTextColor" runat="server" DefaultColor="#FFFFFF" />
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td colspan="3" style="text-align: left">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td>
                    Grid/Table Primary Color <span class="RequireLabel">(*)</span>:
                </td>
                <td>
                    <uc:ColorPicker ID="uxGridTablePrimaryColor" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    Grid/Table Primary Text Color <span class="RequireLabel">(*)</span>:
                </td>
                <td>
                    <uc:ColorPicker ID="uxGridTablePrimaryTextColor" runat="server" DefaultColor="#FFFFFF" />
                </td>
            </tr>
            <tr>
                <td>
                    Grid/Table Border Color <span class="RequireLabel">(*)</span>:
                </td>
                <td>
                    <uc:ColorPicker ID="uxGridTableBorderColor" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    Grid/Table Alt Row Color <span class="RequireLabel">(*)</span>:
                </td>
                <td>
                    <uc:ColorPicker ID="uxGridTableAltRowColor" runat="server" />
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td>
                    Grid/Table Alt Row Text Color <span class="RequireLabel">(*)</span>:
                </td>
                <td>
                    <uc:ColorPicker ID="uxGridTableAltRowTextColor" runat="server" DefaultColor="#333333" />
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td>
                    Grid/Table Sub Header Color <span class="RequireLabel">(*)</span>:
                </td>
                <td>
                    <uc:ColorPicker ID="uxGridTableSubHeaderColor" runat="server" />
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td>
                    Grid/Table Sub Header Text Color <span class="RequireLabel">(*)</span>:
                </td>
                <td>
                    <uc:ColorPicker ID="uxGridTableSubHeaderTextColor" runat="server" DefaultColor="#FFFFFF" />
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td>
                    Grid Report Total Color <span class="RequireLabel">(*)</span>:
                </td>
                <td>
                    <uc:ColorPicker ID="uxGridReportTotalColor" runat="server" />
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td>
                    Grid Report Total Text Color <span class="RequireLabel">(*)</span>:
                </td>
                <td>
                    <uc:ColorPicker ID="uxGridReportTotalTextColor" runat="server" DefaultColor="#333333"/>
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td colspan="3" style="text-align: left">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td>
                    Button Text Color <span class="RequireLabel">(*)</span>:
                </td>
                <td>
                    <uc:ColorPicker ID="uxButtonTextColor" runat="server" DefaultColor="#FFFFFF"/>
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td>
                    Button Primary Color <span class="RequireLabel">(*)</span>:
                </td>
                <td>
                    <uc:ColorPicker ID="uxButtonPrimaryColor" runat="server" />
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td>
                    Button Sub Color <span class="RequireLabel">(*)</span>:
                </td>
                <td>
                    <uc:ColorPicker ID="uxButtonSubColor" runat="server" />
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td>
                    Panel Footer Scroll <span class="RequireLabel">(*)</span>:
                </td>
                <td>
                    <uc:ColorPicker ID="uxPanelFooterScrollColor" runat="server" DefaultColor="#A6A6A6" />
                </td>
                <td>
                </td>
            </tr>
        </table>
        <div style="margin-left:100px; margin-top: 15px;" id="uxPanelOption" runat="server">
            <asp:CheckBox Text="Generate Theme" Checked="true" ID="uxChkGenerate" runat="server" />
            <br />
            <asp:CheckBox Text="Save Theme Information" Checked="true" ID="uxChkSaveTheme" runat="server" />
        </div>
        <div style="text-align: center; margin-top: 15px;">
            <as:Button ID="uxSubmit" runat="server" Text="Submit" OnClick="uxSubmit_Click" OnClientClick="return Validate();" />
        </div>
        <br />
    </div>

    <script type="text/javascript">
        function DoAlert(message) {
            setTimeout('alert("' + message + '");', 100);
        }

        function Validate() {
            if (!FormValidate()) {
                return false;
            }           
            if ($('div[id$="uxSelectedColor"]:empty').length > 0) {
                alert('Please choose all required fields');
                return false;
            }
            return true;
        }
        
        function uxColorPicker_UpdateSelectedColor(sender, args) {
            var labelID = sender.get_id().replace('uxColorPicker', 'uxSelectedColor');
            var uxLabel = document.getElementById(labelID);
            uxLabel.innerHTML = sender.get_selectedColor();

            if (sender.get_id().indexOf('uxPrimaryColor') != -1) {
                $find(sender.get_id().replace('uxPrimaryColor', 'uxGridTablePrimaryColor')).set_selectedColor(sender.get_selectedColor());
                $find(sender.get_id().replace('uxPrimaryColor', 'uxGridTableBorderColor')).set_selectedColor(sender.get_selectedColor());
                $find(sender.get_id().replace('uxPrimaryColor', 'uxButtonPrimaryColor')).set_selectedColor(sender.get_selectedColor());
                $find(sender.get_id().replace('uxPrimaryColor', 'uxButtonSubColor')).set_selectedColor(sender.get_selectedColor());
            }
            if (sender.get_id().indexOf('uxPrimaryTextColor') != -1) {
                $find(sender.get_id().replace('uxPrimaryTextColor', 'uxMenuMouseHoverTextColor')).set_selectedColor(sender.get_selectedColor());
                $find(sender.get_id().replace('uxPrimaryTextColor', 'uxGridTablePrimaryTextColor')).set_selectedColor(sender.get_selectedColor());
                $find(sender.get_id().replace('uxPrimaryTextColor', 'uxGridTableSubHeaderTextColor')).set_selectedColor(sender.get_selectedColor());
                $find(sender.get_id().replace('uxPrimaryTextColor', 'uxButtonTextColor')).set_selectedColor(sender.get_selectedColor());
            }
            if (sender.get_id().indexOf('uxSecondaryColor') != -1) {
                $find(sender.get_id().replace('uxSecondaryColor', 'uxMenuMouseHoverColor')).set_selectedColor(sender.get_selectedColor());
                $find(sender.get_id().replace('uxSecondaryColor', 'uxMenuMouseHoverSubColor')).set_selectedColor(sender.get_selectedColor());
                $find(sender.get_id().replace('uxSecondaryColor', 'uxGridTableAltRowColor')).set_selectedColor(sender.get_selectedColor());
                $find(sender.get_id().replace('uxSecondaryColor', 'uxGridTableSubHeaderColor')).set_selectedColor(sender.get_selectedColor());
                $find(sender.get_id().replace('uxSecondaryColor', 'uxGridReportTotalColor')).set_selectedColor(sender.get_selectedColor());                

            }
        }

    </script>

</asp:Content>
