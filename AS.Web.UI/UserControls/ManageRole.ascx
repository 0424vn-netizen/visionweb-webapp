<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ManageRole.ascx.cs" Inherits="UserControls_ManageRole" %>
<%@ Register TagPrefix="uc" TagName="AccessFunction" Src="~/UserControls/AccessFuntionControl.ascx" %>

<tek:RadAjaxManagerProxy ID="RadAjaxManager1" runat="server">
    <AjaxSettings>
        <tek:AjaxSetting AjaxControlID="uxRoleListSelected">
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="uxRoleListSelected" />
                <tek:AjaxUpdatedControl ControlID="uxHddSelectedRole" />
            </UpdatedControls>
        </tek:AjaxSetting>
        <tek:AjaxSetting AjaxControlID="uxbtnCheckRole1099">
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="uxPlc1099KRole" />
            </UpdatedControls>
        </tek:AjaxSetting>
        <tek:AjaxSetting AjaxControlID="uxbtnCheckPCI">
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="uxPlcPCIRole" />
            </UpdatedControls>
        </tek:AjaxSetting>
        <tek:AjaxSetting AjaxControlID="uxCheckAllMenu">
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="uxPlcPCIRole" />
                <tek:AjaxUpdatedControl ControlID="uxPlc1099KRole" />
            </UpdatedControls>
        </tek:AjaxSetting>
    </AjaxSettings>
</tek:RadAjaxManagerProxy>
<div class="row" id="ux_Header">
    <div class="col-md-12">
        <h3 class="modal-title">
            <asp:Literal ID="lblHeader" runat="server" Text="Define User Role" meta:resourcekey="lblHeaderResource1"></asp:Literal>
        </h3>
    </div>
</div>
<div class="row" id="Step1">
    <div class="col-md-12">
        <p>
            <asp:Literal ID="Literal1" runat="server" Text="
                This wizard will guide you through the creation of the user role. Please complete
                        both the User Role and the Description to move to the next screen."
                meta:resourcekey="LiteralResource1" />
        </p>
        <table class="ASTable form-inline">
            <colgroup>
                <col width="150px" />
                <col />
            </colgroup>
            <tr class="Row">
                <td class="heading">
                    <as:ValidatorLabel ID="uxRoleNameLabel" runat="server" Text="User Role:" ApplyFor="uxRoleName" CssClass="control-label" meta:resourcekey="ManageRoleASCX_ValidatorLabel_UserRole" />
                </td>
                <td>
                    <as:TextBox ID="uxRoleName" runat="server" MaxLength="100" CssClass="w-60" HintCss="hint" onblur="onRoleNameBlur(this, event)" meta:resourcekey="uxRoleNameResource1" />
                    <as:ValidatorMessage runat="server" ID="uxRoleNameErrMsg" ApplyFor="uxRoleName" Message="" ShowOnLoad="False" />
                </td>
            </tr>
            <tr class="AltRow">
                <td class="heading">
                    <as:ValidatorLabel ID="ValidatorLabel1" runat="server" Text="Description:" ApplyFor="uxRoleDes" CssClass="control-label" meta:resourcekey="ManageRoleASCX_ValidatorLabel_Description" />
                </td>
                <td>
                    <as:TextBox ID="uxRoleDes" runat="server" MaxLength="300" TextMode="MultiLine" CssClass="w-60 form-control" Rows="8" HintCss="hint"
                         onblur="onRoleDesBlur(this, event)" meta:resourcekey="uxRoleDesResource1" />
                    <as:ValidatorMessage runat="server" ID="ValidatorMessage1" ApplyFor="uxRoleDes" Message="" ShowOnLoad="False" />
                </td>
            </tr>
        </table>
        <asp:PlaceHolder ID="uxPlViewChangeLogStep1" runat="server">
            <div class="row">
                <div class="col-md-12 mt-3x text-size-sm">
                    <asp:Label ID="uxViewLogTextStep1" CssClass="text-default-gray" runat="server"></asp:Label>
                    <as:LinkButton ID="uxViewLogStep1" CssClass="inline-block ml-8" runat="server"></as:LinkButton>
                </div>
            </div>
        </asp:PlaceHolder>
        <div class="row">
            <div class="col-md-12 form-action-container text-right">
                <as:Button ID="uxCheckUnique" runat="server" CssClass="btn btn-default" Text="Continue" OnClick="uxCheckUnique_Click" OnClientClick="return ValidateData();" IsStandardButton="False" meta:resourcekey="uxCheckUniqueResource1" />
                <as:Button runat="server" CssClass="btn btn-default" ID="uxCancel" Text="Cancel" OnClientClick="return closeMe();" IsStandardButton="False" meta:resourcekey="uxCancelResource1" />
            </div>
        </div>
        <as:Validator ID="Validator1" runat="server" ValidationFunction="ValidateInput" MessageContainerClientID="" MessageType="Inline" meta:resourcekey="Validator1Resource1">
            <Items>
                <as:BasicValidationItem ControlToValidateID="uxRoleName" Rule="Required" Message="This is a required field." meta:resourcekey="ManageRoleASCX_ValidatorMessage_RequiredField" />
                <as:BasicValidationItem ControlToValidateID="uxRoleDes" Rule="Required" Message="This is a required field." meta:resourcekey="ManageRoleASCX_ValidatorMessage_RequiredField" />
                <as:BasicValidationItem ControlToValidateID="uxRoleName" Rule="Minlength" MinLength="2"
                    Message="You must enter at least 2 characters." meta:resourcekey="ManageRoleASCX_ValidatorMessage_AtLeast2Chars" />
                <as:CustomValidationItem ClientValidationFunction="CheckRoleName" ControlToValidateID="uxRoleName" ResMessage="Resources.MessageManager.ReportFilter_V6" ResParams="" />
                <as:BasicValidationItem ControlToValidateID="uxRoleDes" Rule="Minlength" MinLength="2"
                    Message="You must enter at least 2 characters." meta:resourcekey="ManageRoleASCX_ValidatorMessage_AtLeast2Chars" />
            </Items>
        </as:Validator>
        <as:Validator ID="ValidatorRoleName" runat="server" ValidationFunction="ValidateRoleName" MessageType="Inline" MessageContainerClientID="" meta:resourcekey="ValidatorResource1">
            <Items>
                <as:RegExValidationItem ControlToValidateID="uxRoleName" RegularExpression="^((?!(<[^ \t]))(?!(&#)).)*$" ResMessage="Resources.LanguageResource.AS_CommonJS_Msg_IncludedSpeciaCharacters" />
            </Items>
        </as:Validator>
        <as:Validator ID="ValidatorRoleDes" runat="server" ValidationFunction="ValidateRoleDes" MessageType="Inline" MessageContainerClientID="" meta:resourcekey="ValidatorResource1">
            <Items>
                <as:RegExValidationItem ControlToValidateID="uxRoleDes" RegularExpression="^((?!(<[^ \t]))(?!(&#)).)*$" ResMessage="Resources.LanguageResource.AS_CommonJS_Msg_IncludedSpeciaCharacters" />
            </Items>
        </as:Validator>
    </div>
</div>
<div class="clsRole" id="Step2" style="display: none;">
    <div class="row">
        <div class="col-md-12">
            <h3 class="modal-title no-margin-bottom">
                <asp:Literal ID="Literal2" runat="server" Text="Permission" meta:resourcekey="LiteralResource2" /></h3>
        </div>
    </div>
    <div class="height-8"></div>
    <div class="row">
        <div class="col-md-12 relative">
            <!-- CS GROUP -->
            <div class="row row-table">
                <div class="col-xs-6">
                    <h4>Menus</h4>
                </div>
                <div class="col-xs-6 text-right td-bottom-10">
                    <a class="link-back" href="#" onclick="checkAllCheckbox('clientCsMenu'); return false;">
                        <asp:Literal ID="Literal3" runat="server" Text="Select all" meta:resourcekey="LiteralResource3" /></a>
                </div>
            </div>
            <div class="box fixed-height-block height-300">
                <div id="clientCsMenu">
                    <tek:RadTreeView runat="server" TriStateCheckBoxes="true" Width="90%" OnNodeDataBound="uxTreeMenuCS_NodeDataBound"
                        ID="uxTreeMenuCS" CheckBoxes="true" CheckChildNodes="true" OnClientNodeChecked="onCSMenuNodeChecked" />
                </div>
                <div>
                </div>
            </div>
            <div class="param-settings" id="pnlAltRole">
                <div class="w-55 reset-line-height">
                    <as:PlaceHolder ID="uxPlc1099KRole" runat="server" Visible="false">
                        <table>
                            <tr>
                                <td class="text-left w-55">
                                    <label class="dark-blue">
                                        <b>
                                            <asp:Literal ID="Literal4" runat="server" Text="1099-K CompliAssure:" meta:resourcekey="LiteralResource8" /></b></label>
                                </td>
                                <td class="text-right">
                                    <as:RadComboBox ID="ux1099KRole" runat="server" Width="140px" EnableEmbeddedSkins="false"
                                        EnableEmbeddedBaseStylesheet="false" MaxHeight="280px">
                                    </as:RadComboBox>
                                </td>
                            </tr>
                        </table>
                    </as:PlaceHolder>
                    <div id="Div1" class="height-8" style="display: none;"></div>

                    <!-- PCI Admin -->
                    <as:PlaceHolder ID="uxPlcPCIRole" runat="server" Visible="False">
                        <div style="display: none;">

                            <div id="uxheight1" runat="server" class="height-8"></div>
                        </div>

                        <table id="uxPlcPCIRole_table" runat="server">
                            <tr>
                                <td class="text-left w-55">
                                    <label class="dark-blue">
                                        <b>
                                            <asp:Literal ID="Literal5" runat="server" Text="PCI Admin:" /></b></label>
                                </td>
                                <td class="text-right">
                                    <as:RadComboBox ID="uxPCIRole" runat="server" Width="140px" EnableEmbeddedSkins="false"
                                        EnableEmbeddedBaseStylesheet="false" MaxHeight="280px">
                                    </as:RadComboBox>
                                </td>
                            </tr>
                        </table>
                    </as:PlaceHolder>

                </div>
            </div>

            <div class="height-8"></div>
            <div class="row row-table">
                <div class="col-xs-6">
                    <h4>
                        <asp:Literal ID="Literal6" runat="server" Text="Access Functions" meta:resourcekey="LiteralResource4" />
                    </h4>
                </div>
                <div class="col-xs-6 text-right td-bottom-10">
                    <a class="link-back" href="#" onclick="checkAllCheckboxAccessFunction('clientAccessFunction'); return false;">
                        <asp:Literal ID="Literal7" runat="server" Text="Select all" meta:resourcekey="LiteralResource5" /></a>
                </div>
            </div>
            <div class="box ptb-0 fixed-height-block height-200">
                <div id="clientAccessFunction" class="pb-10 pt-9">
                    <uc:AccessFunction ID="uxAccessFuncControl" runat="server" />
                    <as:LinkButton ID="uxLnkAssignActivityGroups" runat="server" Text="Task Permissions"
                        CssClass="link-table" OnClientClick="parent.ShowPopupModalChild(1,'AssignActivityGroupModal.aspx?type=role','auto'); return false;" meta:resourcekey="uxLnkAssignActivityGroupsResource1" />
                </div>
            </div>


            <div id="Div3" class="height-8" style=""></div>
            <as:PlaceHolder ID="PlaceHolder1" runat="server">
                <table class="w-100">
                    <tr>
                        <td class="text-left w-100">
                            <h4>
                                <asp:Literal ID="Literal10" runat="server" Text="Default Landing Page:" meta:resourcekey="ManageRole_ascxDefaultLandingPage" />
                            </h4>
                        </td>
                    </tr>
                    <tr>
                        <td class="text-left">
                            <tek:RadDropDownTree runat="server" ID="uxDefaultLandingPage" OnClientDropDownClosed="DefaultLandingPageOnClientDropDownClosed"
                                DropDownSettings-CssClass="dropdown-tree" EnableScreenBoundaryDetection="true" EnableDirectionDetection="true" OnNodeDataBound="uxDefaultLandingPage_NodeDataBound" Width="55%"
                                DefaultMessage="Choose a destination" DefaultValue="0" DropDownSettings-CloseDropDownOnSelection="true" meta:resourcekey="uxDefaultLandingPage_DefaultMessage">
                            </tek:RadDropDownTree>
                            <div>
                                <as:ValidatorMessage runat="server" ID="ValidatorMessageuxDefaultLandingPage" ApplyFor="uxDefaultLandingPage_fake" Message="" ShowOnLoad="False" />

                            </div>
                            <div class="hide">
                                <tek:RadComboBox ID="uxDefaultLandingPage_fake" runat="server" />
                                <asp:HiddenField ID="uxDefaultLandingPageSelectedValue" runat="server" />
                            </div>
                        </td>
                    </tr>
                </table>

                <as:Validator ID="Validator2" runat="server" ValidationFunction="ValidateDefaultLandingPageInput" MessageContainerClientID="" MessageType="Inline" meta:resourcekey="Validator1Resource1">
                    <Items>
                        <as:CustomValidationItem ControlToValidateID="uxDefaultLandingPage_fake" ClientValidationFunction="validateDropdownTree" Message="This is a required field." meta:resourcekey="ManageRoleASCX_ValidatorMessage_RequiredField" />
                    </Items>
                </as:Validator>

            </as:PlaceHolder>
        </div>
    </div>
    <asp:PlaceHolder ID="uxPlViewChangeLogStep2" runat="server">
        <div class="row">
            <div class="col-md-12 mt-3x text-size-sm">
                <asp:Label ID="uxViewLogTextStep2" CssClass="text-default-gray" runat="server"></asp:Label>
                <as:LinkButton ID="uxViewLogStep2" CssClass="inline-block ml-8" runat="server"></as:LinkButton>
            </div>
        </div>
    </asp:PlaceHolder>
    <div class="row">
        <div class="form-action-container text-right">
            <div class="inline-block">
                <as:Button runat="server" class="btn btn-default" ID="uxCancelAtStep2" Text="Cancel"
                    OnClientClick="return closeMe();" IsStandardButton="False" meta:resourcekey="uxCancelAtStep2Resource1" />
            </div>
            <div class="inline-block">
                <as:Button runat="server" class="btn btn-default" ID="uxBackStep1" Text="Back"
                    OnClientClick="return moveStep(1);" IsStandardButton="False" meta:resourcekey="uxBackStep1Resource1" />
            </div>
            <div id="cidDivSave" class="inline-block">
                <as:Button ID="uxSave1" runat="server" Text="Finish" OnClientClick="return ValidateDefaultLandingPage();" OnClick="uxSave_Click" CssClass="btn btn-default" IsStandardButton="False" meta:resourcekey="uxSave1Resource1" />
            </div>
            <div id="cidDivContinueStep" class="inline-block">
                <as:Button runat="server" ID="uxContinueStep3" class="btn btn-default" Text="Continue"
                    OnClientClick="return ValidateDefaultLandingPage(function(){ moveStep('3'); });" IsStandardButton="False" meta:resourcekey="uxContinueStep3Resource1" />
            </div>
        </div>
    </div>
    <div class="display-none">
        <as:HiddenField ID="uxhdRole1099" runat="server" />
        <as:Button ID="uxbtnCheckRole1099" runat="server" OnClick="CheckRole1099K" IsStandardButton="False" meta:resourcekey="uxbtnCheckRole1099Resource1" />
        <as:HiddenField ID="uxhdRolePCI" runat="server" />
        <as:Button ID="uxbtnCheckPCI" runat="server" OnClick="CheckRolePCI" IsStandardButton="False" meta:resourcekey="uxbtnCheckPCIResource1" />
        <as:Button ID="uxCheckAllMenu" runat="server" OnClick="CheckPCIAnd1099K" IsStandardButton="False" meta:resourcekey="uxCheckAllMenuResource1" />

    </div>
</div>
<div class="clsRole" id="Step3" style="display: none;">

    <div class="row">
        <div class="col-md-12">
            <h3 class="modal-title no-margin-bottom">
                <asp:Literal ID="Literal8" runat="server" Text="Assign User Roles" meta:resourcekey="LiteralResource6" /></h3>
            <div class="height-8"></div>
            <h4 class="no-margin-bottom">
                <asp:Literal ID="Literal9" runat="server" Text="CS User Roles" meta:resourcekey="LiteralResource7" /></h4>
            <div class="height-6"></div>
            <div class="row">
                <div class="col-md-12 text-center">
                    <as:MultiSelector ID="uxRoleListSelected" runat="server" DataValueField="HierarchyID"
                        DataTextField="HierarchyName" AddText=">>" RemoveText="<<" WidthSelector="261"
                        HeightSelector="320" WidthButton="25" OnMovedData="uxRoleListSelected_MovedData"
                        ShowFilter="false" SelectionMode="Multiple"
                        CSSAddAllButton="btn-to-right-all" CSSRemoveAllButton="btn-to-left-all" CSSAddButton="btn-to-right" CSSRemoveButton="btn-to-left"
                        HideAddAll="true" HideRemoveAll="true" ShowTooltip="false"
                        CheckExisted="true" EnableEmbeddedSkins="False" meta:resourcekey="uxRoleListSelectedResource1" />
                    <%-- ListBoxLeft="" ListBoxRight="" SortExpression=""--%>
                </div>
            </div>
        </div>
    </div>
    <asp:PlaceHolder ID="uxPlViewChangeLogStep3" runat="server">
        <div class="row">
            <div class="col-md-12 mt-3x text-size-sm">
                <asp:Label ID="uxViewLogTextStep3" CssClass="text-default-gray" runat="server"></asp:Label>
                <as:LinkButton ID="uxViewLogStep3" CssClass="inline-block ml-8" runat="server"></as:LinkButton>
            </div>
        </div>
    </asp:PlaceHolder>
    <div class="row">
        <div class="form-action-container text-right">
            <div class="inline-block">
                <as:Button runat="server" class="btn btn-default" ID="Button1" Text="Cancel" OnClientClick="return closeMe();" IsStandardButton="False" meta:resourcekey="Button1Resource1" />
            </div>
            <div class="inline-block">
                <as:Button runat="server" class="btn btn-default" ID="Button2" Text="Back" OnClientClick="return moveStep(2);" IsStandardButton="False" meta:resourcekey="Button2Resource1" />
            </div>
            <div id="Div2" class="inline-block">
                <as:Button ID="uxSave" runat="server" Text="Finish" OnClick="uxSave_Click" OnClientClick="return CheckSelectRole();" CssClass="btn btn-default" IsStandardButton="False" meta:resourcekey="uxSaveResource1" />
            </div>
        </div>
    </div>
    <asp:HiddenField ID="uxHddSelectedRole" runat="server" />
</div>

<as:RadCodeBlock ID="uxRadCodeBlock1" runat="server">

    <script type="text/javascript">
        var uxhdRole1099_ClientID  = '<%=uxhdRole1099.ClientID %>';
        var uxhdRolePCI_ClientID  = '<%=uxhdRolePCI.ClientID %>';
        var uxCheckAllMenu_ClientID = '<%=uxCheckAllMenu.ClientID %>';
        var uxTreeMenuCS_ClientID  = '<%=uxTreeMenuCS.ClientID %>';
        var uxHddSelectedRole_ClientID = '<%=uxHddSelectedRole.ClientID %>';
        var uxbtnCheckRole1099_ClientID  = '<%=uxbtnCheckRole1099.ClientID %>';
        var uxbtnCheckPCI_ClientID = '<%=uxbtnCheckPCI.ClientID %>';
        var moveNextStep = <%= moveNextStep.ToString().ToLower() %>;  
        var  uxheight1 = '<%= uxheight1.ClientID %>';
        var _siteJump_1099k_client = '<%=WebSiteConstants.SEC_PERMISSION_SITE_JUMP_1099K %>';
        var ux1099KRole_ClientID = '<%= ux1099KRole.ClientID %>';
        var uxPlcPCIRole_table_ClientID = '<%= uxPlcPCIRole_table.ClientID %>';
        var uxAccessFunctionList_ClientID = '<%= uxAccessFuncControl.ClientID %>';
        var uxLnkAssignActivityGroups_ClientID = '<%= uxLnkAssignActivityGroups.ClientID %>';
        var ManageRole_js_SelectUserRole = '<%= GetLocalResourceObject("ManageRole_js_SelectUserRole").ToString()%>';
        var uxDefaultLandingPage_ClientID = '<%=uxDefaultLandingPage.ClientID %>';
        var uxDefaultLandingPageSelectedValue_ClientID = '<%= uxDefaultLandingPageSelectedValue.ClientID%>';
        var uxRoleName_ClientID = '<%=uxRoleName.ClientID %>';
        var uxRoleDes_ClientID = '<%=uxRoleDes.ClientID %>';

    </script>
    <script src="<%= ResolveUrl("~/")%>res/js/usermaintenance/ManageRole.js"></script>
</as:RadCodeBlock>

