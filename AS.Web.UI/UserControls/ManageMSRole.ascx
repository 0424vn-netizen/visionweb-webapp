<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ManageMSRole.ascx.cs"
    Inherits="UserControls_ManageMSRole" %>
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
        <%--<tek:AjaxSetting AjaxControlID="uxbtnCheckPCI">
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="uxPlcPCIRole" />
            </UpdatedControls>
        </tek:AjaxSetting>--%>
        <tek:AjaxSetting AjaxControlID="uxCheckAllMenu">
            <UpdatedControls>
                <%--<tek:AjaxUpdatedControl ControlID="uxPlcPCIRole" />--%>
                <tek:AjaxUpdatedControl ControlID="uxPlc1099KRole" />
            </UpdatedControls>
        </tek:AjaxSetting>
        <tek:AjaxSetting AjaxControlID="uxLoadAssignMSUserRole">
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="uxRoleListSelected" />
            </UpdatedControls>
        </tek:AjaxSetting>
    </AjaxSettings>
</tek:RadAjaxManagerProxy>

<!-- STEP 1-->
<div id="Step1">
    <div class="row" id="ux_Header">
        <div class="col-md-12">
            <h3 class="modal-title">
                <asp:Literal ID="lblHeader" runat="server" Text="Define MS User Role" meta:resourcekey="lblHeaderResource1"></asp:Literal>
            </h3>
        </div>
    </div>
    <div class="row">

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
                        <as:ValidatorLabel ID="uxRoleNameLabel" runat="server" Text="User Role:" ApplyFor="uxRoleName" CssClass="control-label" meta:resourcekey="ManageMSRoleASCX_ValidatorLabel_UserRole" />
                    </td>
                    <td>
                        <as:TextBox ID="uxRoleName" runat="server" MaxLength="100" CssClass="w-60" HintCss="hint" 
                            onblur="onRoleNameBlur(this, event)" meta:resourcekey="uxRoleNameResource1" />
                        <as:ValidatorMessage runat="server" ID="uxRoleNameErrMsg" ApplyFor="uxRoleName" Message="" ShowOnLoad="False" />
                    </td>
                </tr>
                <tr class="AltRow">
                    <td class="heading">
                        <as:ValidatorLabel ID="ValidatorLabel1" runat="server" Text="Description:" ApplyFor="uxRoleDes" CssClass="control-label" meta:resourcekey="ManageMSRoleASCX_ValidatorLabel_Description" />
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
                    <as:Button ID="uxContinureStep2" runat="server" Text="Continue" OnClick="uxContinureStep2_Click"
                        OnClientClick="return ValidateData();" CssClass="btn btn-default" IsStandardButton="False" meta:resourcekey="uxContinueStep2Resource1" />
                    <as:Button runat="server" ID="uxCancelStep1" Text="Cancel" OnClientClick="return closeMe();" CssClass="btn btn-default" IsStandardButton="False" meta:resourcekey="uxCancelAtStep2Resource1" />
                </div>
            </div>
            <as:Validator ID="ValidatorUniqueRoleName" runat="server" ValidationFunction="ValidateInput" MessageContainerClientID="" MessageType="Inline" meta:resourcekey="ValidatorUniqueRoleNameResource1">
                <Items>
                    <as:BasicValidationItem ControlToValidateID="uxRoleName" Rule="Required" Message="This is a required field." meta:resourcekey="ManageMSRoleASCX_ValidatorMessage_RequiredField" />
                    <as:BasicValidationItem ControlToValidateID="uxRoleDes" Rule="Required" Message="This is a required field." meta:resourcekey="ManageMSRoleASCX_ValidatorMessage_RequiredField" />
                    <as:BasicValidationItem ControlToValidateID="uxRoleName" Rule="Minlength" MinLength="2"
                        Message="You must enter at least 2 characters." meta:resourcekey="ManageMSRoleASCX_ValidatorMessage_AtLeast2Chars" />
                    <as:CustomValidationItem ClientValidationFunction="checkRoleName" ControlToValidateID="uxRoleName"
                        meta:resourcekey="ManageMSRoleASCX_ValidatorMessage_NotSpecialChars" />
                    <as:BasicValidationItem ControlToValidateID="uxRoleDes" Rule="Minlength" MinLength="2"
                        Message="You must enter at least 2 characters." meta:resourcekey="ManageMSRoleASCX_ValidatorMessage_AtLeast2Chars" />
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
</div>
<!-- STEP 2: Hierarchy Level-->
<div class="clsRole" id="Step2" style="display: none;">

    <div class="row">
        <div class="col-md-12">
            <h3 class="modal-title no-margin-bottom">
                <asp:Literal ID="Literal2" runat="server" Text="Select the level of Hierarchy" meta:resourcekey="LiteralResource2" /></h3>
        </div>
    </div>
    <div class="height-8"></div>
    <div class="height-8"></div>
    <div class="row">
        <div class="col-md-12">
            <p><i>
                <asp:Literal ID="Literal3" runat="server" Text="Please select the level of hierarchy at which this MS role shall be used." meta:resourcekey="LiteralResource3" /></i></p>
        </div>

    </div>
    <div class="height-8"></div>
    <div class="row">
        <div class="col-md-12">
            <div class="control-inline">
                <asp:Literal ID="Literal15" runat="server" Text="Hierarchy Level:" meta:resourcekey="LiteralResource15" />
            </div>
            <div class="control-inline">
                <as:RadComboBox ID="uxHierarchyLevel" runat="server" Width="200px" EnableEmbeddedSkins="false"
                    EnableEmbeddedBaseStylesheet="false" MaxHeight="150px">
                </as:RadComboBox>
            </div>
        </div>
    </div>
    <div class="height-100"></div>
    <asp:PlaceHolder ID="uxPlViewChangeLogStep2" runat="server">
        <div class="row">
            <div class="col-md-12 mt-3x text-size-sm">
                <asp:Label ID="uxViewLogTextStep2" CssClass="text-default-gray" runat="server"></asp:Label>
                <as:LinkButton ID="uxViewLogStep2" CssClass="inline-block ml-8" runat="server"></as:LinkButton>
            </div>
        </div>
    </asp:PlaceHolder>
    <div class="row">
        <div class="col-md-12 form-action-container text-right">
            <div class="inline-block">
                <as:Button runat="server" class="btn btn-default" ID="uxCancelAtStep2" Text="Cancel"
                    OnClientClick="return closeMe();" IsStandardButton="False" meta:resourcekey="uxCancelAtStep2Resource1" />
            </div>
            <div class="inline-block">
                <as:Button runat="server" class="btn btn-default" ID="uxBackStep1" Text="Back"
                    OnClientClick="return moveStep(1);" IsStandardButton="False" meta:resourcekey="uxBackStep1Resource1" />
            </div>
            <div class="inline-block">
                <as:Button runat="server" ID="uxContinueStep3" class="btn btn-default" Text="Continue"
                    OnClientClick="return checkHierarchyLevel();" IsStandardButton="False" meta:resourcekey="uxContinueStep2Resource1" />
            </div>
        </div>
    </div>
    <div style="display: none;">
        <as:HiddenField ID="uxOldHierarchyLevel" runat="server" />
    </div>
</div>
<!-- STEP 3: User type-->
<div class="clsRole" id="Step3" style="display: none;">
    <div class="row">
        <div class="col-md-12">
            <h3 class="modal-title no-margin-bottom">
                <asp:Literal ID="Literal4" runat="server" Text="Select Hierarchy User Type" meta:resourcekey="LiteralResource4" /></h3>
        </div>
    </div>
    <div class="height-8"></div>
    <table width="100%" border="0">
        <colgroup>
            <col width="150px" />
            <col />
        </colgroup>
        <tr>
            <td colspan="3" style="padding: 10px 0px">
                <i>
                    <asp:Literal ID="Literal5" runat="server" Text="Please select the type of MS Hierarchy User you want this role to apply to." meta:resourcekey="LiteralResource5" /></i>
            </td>
        </tr>
        <tr>
            <td style="padding: 10px 0px">
                <asp:Literal ID="Literal16" runat="server" Text="User Type:" meta:resourcekey="LiteralResourceUserType" />

            </td>
            <td>
                <as:RadioButton GroupName="UserType" runat="server" Text="Primary User" ID="uxRadPrimaryUser"
                    Checked="True" meta:resourcekey="uxRadPrimaryUserResource1" Value="" />
            </td>
            <td style="padding-left: 25px;">
                <as:RadioButton GroupName="UserType" runat="server" Text="Secondary User" ID="uxRadSecondaryUser" meta:resourcekey="uxRadSecondaryUserResource1" Value="" />
            </td>
        </tr>
        <as:Panel ID="uxPnlExample" runat="server" Visible="false">
            <tr>
                <td align="right" style="padding-bottom: 50px;"></td>
                <td style="padding: 0px 0px 50px 1px;">E.g., BNK8714
                </td>
                <td style="padding: 0px 0px 50px 26px;">E.g., BNK8714-dtaylor
                </td>
            </tr>
        </as:Panel>
    </table>
    <asp:PlaceHolder ID="uxPlViewChangeLogStep3" runat="server">
        <div class="row">
            <div class="col-md-12 mt-3x text-size-sm">
                <asp:Label ID="uxViewLogTextStep3" CssClass="text-default-gray" runat="server"></asp:Label>
                <as:LinkButton ID="uxViewLogStep3" CssClass="inline-block ml-8" runat="server"></as:LinkButton>
            </div>
        </div>
    </asp:PlaceHolder>
    <div class="row">
        <div class="col-md-12 form-action-container text-right">
            <div class="inline-block">
                <as:Button runat="server" class="btn btn-default" ID="uxCancelAtStep3" Text="Cancel"
                    OnClientClick="return closeMe();" IsStandardButton="False" meta:resourcekey="uxCancelAtStep2Resource1" />
            </div>
            <div class="inline-block">
                <as:Button runat="server" class="btn btn-default" ID="uxBackStep2" Text="Back"
                    OnClientClick="return moveStep(2);" IsStandardButton="False" meta:resourcekey="uxBackStep1Resource1" />
            </div>
            <div id="Div2" class="inline-block">
                <as:Button runat="server" ID="uxContinueStep4" class="btn btn-default" Text="Continue"
                    OnClientClick="return validateUserType()" IsStandardButton="False" meta:resourcekey="uxContinueStep2Resource1" />
            </div>
        </div>
    </div>
    <div style="display: none;">
        <as:Button ID="uxLoadMenuAndHierarchy" runat="server" OnClick="LoadMenuAndAssignableHierarchy" IsStandardButton="False" meta:resourcekey="uxLoadMenuAndHierarchyResource1" />
        <as:HiddenField ID="uxOldUserType" runat="server" />
    </div>
</div>
<!-- STEP 4: Permission-->
<div class="clsRole" id="Step4" style="display: none;">
    <div class="row">
        <div class="col-md-12">
            <h3 class="modal-title no-margin-bottom">
                <asp:Literal ID="Literal6" runat="server" Text="Permission" meta:resourcekey="LiteralResource6" /></h3>
        </div>
    </div>
    <div class="height-8"></div>
    <div class="row">
        <!-- CS GROUP -->
        <div class="col-md-12 relative">
            <div class="row row-table">
                <div class="col-xs-6">
                    <h4>
                        <asp:Literal ID="Literal7" runat="server" Text="Menus" meta:resourcekey="LiteralResource7" /></h4>
                </div>
                <div class="col-xs-6 text-right td-bottom-10">
                    <a class="link-back" href="#" onclick="checkAllCheckbox('clientCsMenu'); return false;">
                        <asp:Literal ID="Literal8" runat="server" Text="Select all" meta:resourcekey="LiteralResource8" /></a>
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
            <!-- Access Function -->
            <div class="height-8"></div>
            <div class="row row-table">
                <div class="col-xs-6">
                    <h4>
                        <asp:Literal ID="Literal9" runat="server" Text="Access Functions" meta:resourcekey="LiteralResource9" />
                    </h4>
                </div>
                <div class="col-xs-6 text-right td-bottom-10">
                    <a class="link-back" href="#" onclick="checkAllCheckboxAccessFunction('clientAccessFunction'); return false;">
                        <asp:Literal ID="Literal10" runat="server" Text="Select all" meta:resourcekey="LiteralResource10" /></a>
                </div>
            </div>
            <div class="box ptb-0 fixed-height-block height-100">
                <div id="clientAccessFunction" class="pb-10 pt-9"> 
                    <uc:AccessFunction ID="uxAccessFuncControl" runat="server" />
                </div>
            </div>
            <div class="param-settings" id="pnlAltRole" runat="server">
                <div class="w-55 reset-line-height">
                    <as:PlaceHolder ID="uxPlc1099KRole" runat="server" Visible="False">
                        <table>
                            <tr>
                                <td class="text-left w-55">
                                    <label class="dark-blue"><b>
                                        <asp:Literal ID="Literal11" runat="server" Text="1099-K CompliAssure:" meta:resourcekey="LiteralResource11" /></b></label>
                                </td>
                                <td class="text-right">
                                    <as:RadComboBox ID="ux1099KRole" runat="server" Width="140px" EnableEmbeddedSkins="false"
                                        EnableEmbeddedBaseStylesheet="false" MaxHeight="280px" meta:resourcekey="ux1099KRoleResource1">
                                    </as:RadComboBox>
                                </td>
                            </tr>
                        </table>
                    </as:PlaceHolder>
                    <!-- PCI Admin -->
                    <%--<as:PlaceHolder ID="uxPlcPCIRole" runat="server" Visible="False">
                        <div id="uxheight1" runat="server" class="height-8"></div>
                        <table id="uxPlcPCIRole_table" runat="server">
                            <tr>
                                <td class="text-left w-55">
                                    <label class="dark-blue"><b>
                                        <asp:Literal ID="Literal12" runat="server" Text="PCI Admin:" /></b></label>
                                </td>
                                <td class="text-right">
                                    <as:RadComboBox ID="uxPCIRole" runat="server" Width="140px" EnableEmbeddedSkins="false"
                                        EnableEmbeddedBaseStylesheet="false" MaxHeight="280px">
                                    </as:RadComboBox>
                                </td>
                            </tr>
                        </table>
                    </as:PlaceHolder>--%>
                </div>
            </div>


            <div id="Div3" class="height-8" style=""></div>
            <as:PlaceHolder ID="PlaceHolder1" runat="server">
                <table>
                    <tr>
                        <td class="text-left w-55">
                            <h4>
                                <asp:Literal ID="Literal17" runat="server" Text="Default Landing Page:" meta:resourcekey="ManageRole_ascxDefaultLandingPage" />
                            </h4>
                        </td>
                    </tr>
                    <tr>
                        <td class="text-left w-55">
                            <tek:RadDropDownTree runat="server" ID="uxDefaultLandingPage" OnClientDropDownClosed="DefaultLandingPageOnClientDropDownClosed" EnableScreenBoundaryDetection="true" EnableDirectionDetection="true" OnNodeDataBound="uxDefaultLandingPage_NodeDataBound" Width="100%" DefaultMessage="Choose a destination" DefaultValue="0" DropDownSettings-CloseDropDownOnSelection="true" meta:resourcekey="uxDefaultLandingPage_DefaultMessage">
                            </tek:RadDropDownTree>
                            <as:ValidatorMessage runat="server" ID="ValidatorMessageuxDefaultLandingPage" ApplyFor="uxDefaultLandingPage_fake" Message="" ShowOnLoad="False" />
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
    <asp:PlaceHolder ID="uxPlViewChangeLogStep4" runat="server">
        <div class="row">
            <div class="col-md-12 mt-3x text-size-sm">
                <asp:Label ID="uxViewLogTextStep4" CssClass="text-default-gray" runat="server"></asp:Label>
                <as:LinkButton ID="uxViewLogStep4" CssClass="inline-block ml-8" runat="server"></as:LinkButton>
            </div>
        </div>
    </asp:PlaceHolder>
    <div class="row">
        <div class="col-md-12 form-action-container text-right">
            <div class="inline-block">
                <as:Button runat="server" class="btn btn-default" ID="uxCancelAtStep4" Text="Cancel"
                    OnClientClick="return closeMe();" IsStandardButton="False" meta:resourcekey="uxCancelAtStep2Resource1" />
            </div>
            <div class="inline-block">
                <as:Button runat="server" class="btn btn-default" ID="uxBackStep3" Text="Back"
                    OnClientClick="return customMoveStep4();" IsStandardButton="False" meta:resourcekey="uxBackStep1Resource1" />
                &nbsp;&nbsp;&nbsp
            </div>
            <div id="cidDivSave" class="inline-block">
                <as:Button ID="uxSave" runat="server" Text="Finish" OnClick="uxSave_Click" CssClass="btn btn-default" IsStandardButton="False" meta:resourcekey="uxSaveResource1" OnClientClick="return ValidateDefaultLandingPage();" />
                &nbsp;&nbsp;&nbsp
            </div>
            <div id="cidDivContinueStep" class="inline-block">
                <as:Button runat="server" ID="uxContinueStep5" class="btn btn-default" Text="Continue"
                    OnClientClick="return ValidateDefaultLandingPage(function(){ moveStep(5); });" IsStandardButton="False" meta:resourcekey="uxContinueStep2Resource1" />
                &nbsp;&nbsp;&nbsp
            </div>
        </div>
    </div>
    <div style="display: none;">
        <as:HiddenField ID="uxhdRole1099" runat="server" />
        <as:Button ID="uxbtnCheckRole1099" runat="server" OnClick="CheckRole1099K" IsStandardButton="False" meta:resourcekey="uxbtnCheckRole1099Resource1" />
        <as:HiddenField ID="uxhdRolePCI" runat="server" />
        <%--<as:Button ID="uxbtnCheckPCI" runat="server" OnClick="CheckRolePCI" IsStandardButton="False" meta:resourcekey="uxbtnCheckPCIResource1" />--%>
        <as:Button ID="uxCheckAllMenu" runat="server" OnClick="CheckPCIAnd1099K" IsStandardButton="False" meta:resourcekey="uxCheckAllMenuResource1" />
    </div>
</div>
<!-- STEP 5: Assign User Role-->
<div class="clsRole" id="Step5" style="display: none;">

    <div class="row">
        <div class="col-md-12">
            <h3 class="modal-title no-margin-bottom">
                <asp:Literal ID="Literal13" runat="server" Text="Assign MS User Roles" meta:resourcekey="LiteralResource12" /></h3>
            <div class="height-8"></div>
            <h4 class="no-margin-bottom">
                <asp:Literal ID="Literal14" runat="server" Text="MS User Roles" meta:resourcekey="LiteralResource13" /></h4>
            <div class="height-6"></div>
            <div class="row">
                <div class="col-md-12 text-center">
                    <as:MultiSelector ID="uxRoleListSelected" runat="server" DataValueField="HierarchyID"
                        DataTextField="HierarchyName" AddText=">>" RemoveText="<<" WidthSelector="261"
                        HeightSelector="320" WidthButton="25" OnMovedData="uxRoleListSelected_MovedData"
                        ShowFilter="false" SelectionMode="Multiple"
                        CSSAddAllButton="btn-to-right-all" CSSRemoveAllButton="btn-to-left-all" CSSAddButton="btn-to-right" CSSRemoveButton="btn-to-left"
                        HideAddAll="true" HideRemoveAll="true" ShowTooltip="false"
                        CheckExisted="true" EnableEmbeddedSkins="False" />
                    <%--ListBoxLeft="" ListBoxRight="" SortExpression=""--%>
                </div>
            </div>
        </div>
    </div>
    <asp:PlaceHolder ID="uxPlViewChangeLogStep5" runat="server">
        <div class="row">
            <div class="col-md-12 mt-3x text-size-sm">
                <asp:Label ID="uxViewLogTextStep5" CssClass="text-default-gray" runat="server"></asp:Label>
                <as:LinkButton ID="uxViewLogStep5" CssClass="inline-block ml-8" runat="server"></as:LinkButton>
            </div>
        </div>
    </asp:PlaceHolder>
    <div class="row">
        <div class="col-md-12 form-action-container text-right">
            <div class="inline-block">
                <as:Button runat="server" class="btn btn-default" ID="Button1" Text="Cancel" OnClientClick="return closeMe();" IsStandardButton="False" meta:resourcekey="uxCancelAtStep2Resource1" />
            </div>
            <div class="inline-block">
                <as:Button runat="server" class="btn btn-default" ID="Button2" Text="Back" OnClientClick="return moveStep(4);" IsStandardButton="False" meta:resourcekey="uxBackStep1Resource1" />
            </div>
            <div class="inline-block">
                <as:Button ID="uxSave2" runat="server" Text="Finish" OnClick="uxSave_Click" OnClientClick="return checkSelectRole();"
                    CssClass="btn btn-default" IsStandardButton="False" meta:resourcekey="uxSaveResource1" />
            </div>
        </div>
    </div>
    <asp:HiddenField ID="uxHddSelectedRole" runat="server" />
</div>
<as:RadCodeBlock ID="uxRadCodeBlock1" runat="server">

    <script type="text/javascript">
        var Text_RequiredHierarchy = '<%=GetLocalResourceObject("ManageMSRoleJS_Text_RequiredHierarchy").ToString()%>';
        var Text_RequiredUserType = '<%=GetLocalResourceObject("ManageMSRoleJS_Text_RequiredUserType").ToString()%>';
        var Text_RequiredAtLeastOneRole = '<%=GetLocalResourceObject("ManageMSRoleJS_Text_AtLeastOneRole").ToString()%>';
        var DisableSecondaryUserRoleByEntities = '<%=DisableSecondaryUserRoleByEntities %>';
        var backStepFromStep4 = '<%=DisableStepCreateMSRole ? 1 : 3 %>';
        var uxHierarchyLevel = '<%=uxHierarchyLevel.ClientID%>';
        var uxRadSecondaryUser = '<%=uxRadSecondaryUser.ClientID%>';
        var uxRadPrimaryUser = '<%=uxRadPrimaryUser.ClientID%>';
        var uxLoadMenuAndHierarchy = '<%=uxLoadMenuAndHierarchy.ClientID %>';
        var uxhdRole1099 = '<%=uxhdRole1099.ClientID %>';
        var uxhdRolePCI = '<%=uxhdRolePCI.ClientID %>';
        var uxTreeMenuCS = '<%=uxTreeMenuCS.ClientID %>';
        var uxCheckAllMenu = '<%=uxCheckAllMenu.ClientID %>';
        var uxbtnCheckRole1099 = '<%=uxbtnCheckRole1099.ClientID %>';
        var uxHddSelectedRole = '<%=uxHddSelectedRole.ClientID %>';
        var uxRoleName_ClientID = '<%=uxRoleName.ClientID %>';
        var uxRoleDes_ClientID = '<%=uxRoleDes.ClientID %>';

        var HL_SELECT_HIERARCHY_VALUE = '<%=HL_SELECT_HIERARCHY_VALUE %>';
        var SEC_PERMISSION_SITE_JUMP_1099K = '<%=WebSiteConstants.SEC_PERMISSION_SITE_JUMP_1099K %>';
        var SEC_PERMISSION_HIERARCHY_SITE_ACCESS_PCI = '<%=WebSiteConstants.SEC_PERMISSION_HIERARCHY_SITE_ACCESS_PCI %>';
        var SEC_PERMISSION_MAN_USER_MS = '<%=WebSiteConstants.SEC_PERMISSION_MAN_USER_MS %>';

        var uxAccessFunctionList_ClientID = '<%= uxAccessFuncControl.ClientID %>';
        var uxDefaultLandingPage_ClientID = '<%=uxDefaultLandingPage.ClientID %>';
        var uxDefaultLandingPageSelectedValue_ClientID = '<%= uxDefaultLandingPageSelectedValue.ClientID%>';
        var DisableSecondaryUserByClient = '<%=GeneralFuncsLib.GetDataOfExtendedSetting("DisableSecondaryUserInCreateMSRole").Equals("true") %>';

        var IsEdit = '<%= IsEditMode%>';
        <%=MoveSecondStep ? "setTimeout('moveStep(2)',500);" : ""%>
        <%=MoveFourthStep ? "setTimeout('moveStep(4)',100);" : ""%>
    </script>
    <script src="<%= ResolveUrl("~/")%>res/js/usermaintenance/ManageMSRoles.js"></script>

</as:RadCodeBlock>
