<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ManageUser.ascx.cs" Inherits="As.VisionWeb.Web.ManageUserControl" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Src="~/UserControls/CreateNewUserMasterUserControl.ascx" TagName="CreateNewUserMasterUserControl" TagPrefix="uc" %>

<!-- Rad Ajax-->
<tek:RadAjaxManagerProxy ID="RadAjaxManager1" runat="server">
    <AjaxSettings>
        <tek:AjaxSetting AjaxControlID="uxRebindOrgSelected">
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="lbNumSelectedOrg" />
            </UpdatedControls>
        </tek:AjaxSetting>
        <tek:AjaxSetting AjaxControlID="uxRiskGroup">
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="uxRiskGroup" />
            </UpdatedControls>
        </tek:AjaxSetting>
        <tek:AjaxSetting AjaxControlID="uxRiskManagement">
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="uxRiskGroup" />
            </UpdatedControls>
        </tek:AjaxSetting>
        <tek:AjaxSetting AjaxControlID="uxSave">
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="plhWrapper" />
            </UpdatedControls>
        </tek:AjaxSetting>
        <tek:AjaxSetting AjaxControlID="linkViewOrgazition">
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="linkViewOrgazition" />
            </UpdatedControls>
        </tek:AjaxSetting>
        <tek:AjaxSetting AjaxControlID="uxAccessListFunction">
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="lbNumSelectedOrg" />
            </UpdatedControls>
        </tek:AjaxSetting>
        <tek:AjaxSetting AjaxControlID="uxAFExternalAccess">
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="txtAPIPassword" />
                <tek:AjaxUpdatedControl ControlID="btnShowPassword" LoadingPanelID="uxInvisiblePanel" />
                <tek:AjaxUpdatedControl ControlID="lblApiPasswordText" LoadingPanelID="uxInvisiblePanel" />
            </UpdatedControls>
        </tek:AjaxSetting>
        <tek:AjaxSetting AjaxControlID="uxRole">
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="uxAccessListFunction" LoadingPanelID="uxInvisiblePanel" />
                <tek:AjaxUpdatedControl ControlID="uxLnkAssignActivityGroups" LoadingPanelID="uxInvisiblePanel" />
                <tek:AjaxUpdatedControl ControlID="uxSaleRepCode" LoadingPanelID="uxInvisiblePanel" />
                <tek:AjaxUpdatedControl ControlID="txtSalesRepCodeErrMsg" LoadingPanelID="uxInvisiblePanel" />
                <tek:AjaxUpdatedControl ControlID="uxSaleRepCodelable" LoadingPanelID="uxInvisiblePanel" />
                <tek:AjaxUpdatedControl ControlID="lbNumSelectedOrg" LoadingPanelID="uxInvisiblePanel" />
                <tek:AjaxUpdatedControl ControlID="uxPlcRiskMng" LoadingPanelID="uxInvisiblePanel" />
                <tek:AjaxUpdatedControl ControlID="uxAFMerchantProfileAccess" LoadingPanelID="uxInvisiblePanel" />
                <tek:AjaxUpdatedControl ControlID="uxAFRiskAccess" LoadingPanelID="uxInvisiblePanel" />
                <tek:AjaxUpdatedControl ControlID="uxAFSecurityAccess" LoadingPanelID="uxInvisiblePanel" />
                <tek:AjaxUpdatedControl ControlID="uxAFAliceAccess" LoadingPanelID="uxInvisiblePanel" />
                <tek:AjaxUpdatedControl ControlID="trOwnershipGroup" LoadingPanelID="uxInvisiblePanel" />
                <tek:AjaxUpdatedControl ControlID="uxApiAccess" LoadingPanelID="uxInvisiblePanel" />
                <tek:AjaxUpdatedControl ControlID="txtAPIPassword" LoadingPanelID="uxInvisiblePanel" />
                <tek:AjaxUpdatedControl ControlID="uxAFExternalAccess" LoadingPanelID="uxInvisiblePanel" />
                <tek:AjaxUpdatedControl ControlID="btnShowPassword" LoadingPanelID="uxInvisiblePanel" />
                <tek:AjaxUpdatedControl ControlID="lblApiPasswordText" LoadingPanelID="uxInvisiblePanel" />
                <tek:AjaxUpdatedControl ControlID="uxPnlStatus" LoadingPanelID="uxInvisiblePanel" />
                <%--TK: 41877 - ALICE PH2 - Climate Control Updates --%>
                <tek:AjaxUpdatedControl ControlID="uxFirstName" LoadingPanelID="uxInvisiblePanel" />
                <tek:AjaxUpdatedControl ControlID="uxLastName" LoadingPanelID="uxInvisiblePanel" />
                <tek:AjaxUpdatedControl ControlID="uxEmail" LoadingPanelID="uxInvisiblePanel" />
                <%--41945 - Notifications - Enhancements to Notifications Admin settings.--%>
                <tek:AjaxUpdatedControl ControlID="uxNotificationAccess" LoadingPanelID="uxInvisiblePanel" />
                <%--45535 - FIS - Activation Report--%>
                <tek:AjaxUpdatedControl ControlID="uxProcessingDateAccess" LoadingPanelID="uxInvisiblePanel" />
                 <tek:AjaxUpdatedControl ControlID="uxManageDocumentTypesAccess" LoadingPanelID="uxInvisiblePanel" />
                <tek:AjaxUpdatedControl ControlID="uxSponsorAccess" LoadingPanelID="uxInvisiblePanel" />
            </UpdatedControls>
        </tek:AjaxSetting>
    </AjaxSettings>
</tek:RadAjaxManagerProxy>


<table class="ASTable form-inline" id="lbManageUserTable">
    <tbody id="tblManageUserMain">
        <tr>
            <td class="heading" style="width: 20%;">
                <as:ValidatorLabel ID="txtUsernameLabel" runat="server" Text="User Name:" ApplyFor="uxUsername" CssClass="control-label" meta:resourcekey="ManageUserASCX_Text_Username" />
            </td>
            <td>
                <div class="w-50">
                    <as:TextBox ID="uxUsernameOld" runat="server" Enabled="False" ReadOnly="True" CssClass="form-control"
                        Width="100%" HintCss="hint" meta:resourcekey="uxUsernameOldResource1" autocomplete="off" autocorrect="off" />
                </div>
                <div class="w-50 inline-block valign-middle" runat="server" id="uxCreateMode">

                    <as:PlaceHolder ID="IsUserSignOn" runat="server" Visible="false">
                        <as:TextBox ID="uxUsernameSignOn" runat="server" CausesValidation="True"
                            CssClass="form-control" Width="100%" HintCss="hint" meta:resourcekey="uxUsernameResource1" />
                    </as:PlaceHolder>
                    <as:PlaceHolder ID="IsNotUserSignOn" runat="server" Visible="false">
                        <table class="checkbox-list max-width">
                            <tr>
                                <td id="uxPrefix" runat="server" style="width: 30px;">
                                    <div style="white-space: nowrap;">
                                        <b>
                                            <as:Literal ID="uxuserNamePrefix" runat="server" meta:resourcekey="uxuserNamePrefixResource1"></as:Literal>
                                        </b>
                                    </div>
                                </td>
                                <td>
                                    <as:TextBox ID="uxUsername" runat="server" CausesValidation="True"
                                        CssClass="form-control" Width="100%" HintCss="hint" meta:resourcekey="uxUsernameResource1" autocomplete="off" autocorrect="off" />
                                </td>
                            </tr>
                        </table>
                    </as:PlaceHolder>
                </div>
                <as:ValidatorMessage runat="server" ID="txtUserNameErrMsg" ApplyFor="uxUsername" Message="" ShowOnLoad="False" />
            </td>
        </tr>
        <asp:PlaceHolder ID="uxUserNameTips" runat="server" Visible="False">
            <tr>
                <td align="center" colspan="2" class="privacy" style="padding-left: 60px; padding-right: 60px;">
                    <asp:Literal ID="lbUserNameDescription" runat="server" Text="User Name must include between 7 and 10 alphanumeric characters after the account number." />
                    <br />
                    <asp:Literal ID="Literal2" runat="server" Text="Typically this will include some combination of first and last name." meta:resourcekey="LiteralResource2" />
                </td>
            </tr>
        </asp:PlaceHolder>
        <tr>
            <td class="heading">
                <as:ValidatorLabel ID="Validatorlabel1" runat="server" Text="First Name:" ApplyFor="uxFirstName" CssClass="control-label" meta:resourcekey="ManageUserASCX_Text_Firstname" />
            </td>
            <td>
                <div class="w-50 inline-block valign-middle">
                    <as:TextBox ID="uxFirstName" runat="server" CssClass="form-control" MaxLength="30"
                        Width="100%" HintCss="hint" meta:resourcekey="uxFirstNameResource1" />
                </div>
                <as:ValidatorMessage runat="server" ID="ValidatorMessage1" ApplyFor="uxFirstName" Message="" ShowOnLoad="False" />
            </td>

        </tr>
        <tr>
            <td class="heading">
                <as:ValidatorLabel ID="Validatorlabel2" runat="server" Text="Last Name:" ApplyFor="uxLastName" CssClass="control-label" meta:resourcekey="ManageUserASCX_Text_Lastname" />
            </td>
            <td>
                <div class="w-50 inline-block valign-middle">
                    <as:TextBox ID="uxLastName" runat="server" MaxLength="30" CssClass="form-control"
                        Width="100%" HintCss="hint" meta:resourcekey="uxLastNameResource1" />
                </div>
                <as:ValidatorMessage runat="server" ID="ValidatorMessage2" ApplyFor="uxLastName" Message="" ShowOnLoad="False" />
            </td>

        </tr>
        <tr>
            <td class="heading">
                <as:ValidatorLabel ID="Validatorlabel3" runat="server" Text="Email:" ApplyFor="uxEmail" CssClass="control-label" meta:resourcekey="ManageUserASCX_Text_Email" />

            </td>
            <td>
                <div class="w-50 inline-block valign-middle">
                    <as:TextBox ID="uxEmail" runat="server" CssClass="form-control" MaxLength="100" Width="100%" HintCss="hint" meta:resourcekey="uxEmailResource1" />
                </div>
                <as:ValidatorMessage runat="server" ID="ValidatorMessage3" ApplyFor="uxEmail" Message="" ShowOnLoad="False" />
            </td>
        </tr>
        <tr id="uxTrRiskRole" runat="server">
            <td class="heading">
                <as:ValidatorLabel ID="Validatorlabel4" runat="server" Text="Role:" ApplyFor="uxRole" CssClass="control-label" meta:resourcekey="ManageUserASCX_Text_Role" />
            </td>
            <td>
                <div class="w-50">
                    <as:RadComboBox ID="uxRole" runat="server" Width="100%" AutoPostBack="true" MaxHeight="102px"
                        OnSelectedIndexChanged="uxRole_SelectedIndexChanged" />
                </div>
                <as:ValidatorMessage runat="server" ID="ValidatorMessage4" ApplyFor="uxRole" Message="" ShowOnLoad="False" />
            </td>
        </tr>
        <%--TK 36296 - Climate Control--%>
        <tr id="uxTrSaleRepCode" style="display: none">
            <td class="heading">
                <div class="ml-m-2x">
                    <as:ValidatorLabel ID="uxSaleRepCodelable" runat="server" Text="Sales Rep Code:" ApplyFor="uxSaleRepCode" CssClass="" meta:resourcekey="ManageUserASCX_Text_SaleRepCode" />
                </div>
            </td>
            <td>
                <div class="w-50 inline-block valign-middle">
                    <as:TextBox ID="uxSaleRepCode" runat="server" MaxLength="4" CssClass="form-control" Width="100%" HintCss="hint" meta:resourcekey="uxSaleRepCodeResource1" />
                </div>
                <div class="display-inline">
                    <as:ValidatorMessage runat="server" ID="txtSalesRepCodeErrMsg" ApplyFor="uxSaleRepCode" Message="" ShowOnLoad="False" />
                </div>
                <div class="w-100 inline-block valign-middle">
                    <asp:Literal ID="Literal16" runat="server" Text="" meta:resourcekey="ManageUserASCX_Text_SaleRepNote" />
                </div>
            </td>
        </tr>
        <tr id="uxTrOrgazition" style="display: none">
            <td class="heading">
                <asp:Literal ID="Literal8" runat="server" Text="Organization:" meta:resourcekey="ManageUserASCX_Text_Organizations" />
            </td>
            <td>
                <div class="w-30 inline-block valign-middle">
                    <as:LinkButton ID="btnAddOrg" runat="server" Text="Add Organizations" meta:resourcekey="uxLnkAddOrganizationResource1" />
                </div>
                <div class="w-50 inline-block valign-right" id="divSeletedOrg">
                    <asp:Literal ID="lbNumSelectedOrg" Text="" runat="server" />
                </div>
            </td>
        </tr>
        <tr id="uxTrViewOrgazition" style="display: none">
            <td class="heading">
                <asp:Literal ID="Literal9" runat="server" Text="Organization:" meta:resourcekey="ManageUserASCX_Text_Organizations" />
            </td>
            <td>
                <div class="w-30 inline-block valign-middle">
                    <as:LinkButton ID="linkViewOrgazition" runat="server" Text="View Organizations" OnClick="linkViewOrgazition_Click" meta:resourcekey="uxLnkViewOrganizationResource1" />
                </div>
                <div class="w-50 inline-block valign-right" id="divAssociatedOrg">
                    <asp:Label ID="lbNumAssociatedOrg" runat="server"></asp:Label>
                </div>
            </td>
        </tr>
        <tr id="uxTrGroup" style="display: none">
            <td class="heading">
                <as:ValidatorLabel ID="Validatorlabel7" runat="server" Text="1099K Group:" ApplyFor="uxRiskGroup" CssClass="control-label" meta:resourcekey="ManageUserASCX_Text_1099KGroup" />
            </td>
            <td>
                <div class="w-50">
                    <as:RadComboBox ID="uxGroup" runat="server" Width="100%" DataTextField="GroupName"
                        DataValueField="GroupUserID" AutoPostBack="false" MaxHeight="102px" />
                </div>
            </td>
        </tr>

        <tr id="uxTrRiskGroup" style="display: none">
            <td class="heading">
                <as:ValidatorLabel ID="Validatorlabel5" runat="server" Text="Risk Group:" ApplyFor="uxRiskGroup" CssClass="control-label" meta:resourcekey="ManageUserASCX_Text_RiskGroup" />
            </td>
            <td>
                <div class="w-50">
                    <as:RadComboBox ID="uxRiskGroup" runat="server" Width="100%" DataTextField="GroupName"
                        DataValueField="GroupID" AutoPostBack="true" OnSelectedIndexChanged="uxRiskGroup_SelectedIndexChanged"
                        Height="102px" />
                </div>
            </td>

        </tr>
        <tr id="uxApproveGroupRow" class="hide" runat="server">
            <td class="heading">
                 <asp:Label ID="ValidatorLabel6" runat="server" CssClass="" meta:resourcekey="ApproveGroup"></asp:Label>
            </td>
            <td>
                 <div class="row">
                    <div class="col-xs-12">
                        <strong><asp:Label ID="Literal20" runat="server" meta:resourcekey="SelectApproveGroup"></asp:Label></strong>
                        <div class="checkbox-col3" id="uxApproveGroupList-Wrapper">
                            <asp:Repeater runat="server" ID="uxApproveGroupList" OnItemDataBound="uxApproveGroupList_ItemDataBound">
                                <ItemTemplate>
                                    <div class="checkbox-default">
                                        <input runat="server" type="checkbox" class="ck-approver-group-js" id="ckApproveGroup"
                                            checked='<%# Eval("IsAssigned") %>' value='<%# Eval("Id") %>' />
                                        <span><%# Eval("ApproverGroupName") %></span>                                       
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                    </div>
                </div>
            </td>
        </tr>
        <tr id="trOwnershipGroup" style="display: none">
            <td class="heading" id="trOwnershipGroupLable">
                <as:ValidatorLabel ID="lblOwnershipGroup" runat="server" ApplyFor="uxOwnershipGroupDefault" Text="Ownership Group:" meta:resourcekey="ManageUserASCX_Text_OwnershipGroup"></as:ValidatorLabel>
            </td>
            <td id="uxOwnershipGroup">
                <div class="row">
                    <div class="col-xs-12">
                        <span class="control-label">
                            <b>
                                <asp:Literal ID="Literal10" runat="server" Text="Select Ownership Group" meta:resourcekey="ManageUserASCX_Text_SelectOwnershipGroup"></asp:Literal></b>
                        </span>
                        <div class="checkbox-col3" id="uiOwnershipGroupList">
                            <asp:Repeater runat="server" ID="uxOwnershipGroupList" OnItemDataBound="uxOwnershipGroupList_ItemDataBound">
                                <ItemTemplate>
                                    <div class="checkbox-default">
                                        <input runat="server" type="checkbox" id="ckOwnership" checked='<%# Eval("IsChecked") %>' value='<%# Eval("OwnershipGroupID") %>'
                                            onclick="visibleOwnershipGroupDropdown(this)" />
                                        <asp:HyperLink href="#" ID="uxLinkOwnershipGroup" CssClass="ml-1x" Text='<%# Eval("OwnershipGroup") %>' Value='<%# Eval("OwnershipGroupID") %>' runat="server"></asp:HyperLink>
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                    </div>
                </div>
                <hr class="line" />
                <div class="row">
                    <div class="col-xs-12">
                        <div class="mb-1x">
                            <b class="control-label">
                                <asp:Literal ID="Literal11" runat="server" Text="Select Default Ownership Group when opening a new case" meta:resourcekey="ManageUserASCX_Text_SelectDropDownOwnershipGroup"></asp:Literal>
                            </b>
                        </div>
                        <as:RadComboBox runat="server" ID="uxOwnershipGroupDefault" DataValueField="OwnershipGroupID" Width="40%" CssClass="mb-2x" Enabled="false"
                            ChangeTextOnKeyBoardNavigation="false" OnItemDataBound="uxOwnershipGroupDefault_ItemDataBound" DataTextField="OwnershipGroup" EmptyMessage="Ownership Group" meta:resourcekey="uxOwnershipGroupEmptyMessage">
                        </as:RadComboBox>
                        <as:ValidatorMessage runat="server" ID="lblRequiredOwnershipMessage" ApplyFor="uxOwnershipGroupDefault" Message="" ShowOnLoad="False" />
                        <div class="hide">
                            <as:TextBox ID="txtOwnershipGroup" runat="server" />
                        </div>
                    </div>
                </div>
            </td>
        </tr>
        <tr>
            <td class="heading">
                <asp:Literal ID="Literal3" runat="server" Text="Active:" meta:resourcekey="ManageUserASCX_Text_Active" /></td>
            <td>
                <asp:Panel ID="uxPnlStatus" runat="server">
                    <as:RadioButton ID="uxActive" runat="server" Text="Active" GroupName="Active" Checked="True"
                        meta:resourcekey="uxActiveResource1" Value="" />
                    <as:RadioButton ID="uxInactive" runat="server" Text="Inactive" GroupName="Active"
                        meta:resourcekey="uxInactiveResource1" Value="" />
                </asp:Panel>
            </td>

        </tr>

        <as:Panel ID="uxPhdAccessFunction" runat="server">
            <tr id="uxtrMerchantProfileAccess" runat="server">
                <td class="heading">
                    <asp:Literal ID="Literal12" runat="server" Text="Access Functions:" meta:resourcekey="AccessFuntionControlGroupFuncLabel_MerchantProfileAccess" />
                </td>
                <td>
                    <as:CheckBoxList ID="uxAFMerchantProfileAccess" CssClass="checkbox-list checkbox-list-span" runat="server" AutoPostBack="true"
                        DataTextField="Description" DataValueField="PermissionId" meta:resourcekey="uxAccessListFunctionResource1">
                    </as:CheckBoxList>
                </td>
            </tr>
            <tr id="uxtrRiskAccess" runat="server">
                <td class="heading">
                    <asp:Literal ID="Literal13" runat="server" Text="Access Functions:" meta:resourcekey="AccessFuntionControlGroupFuncLabel_RiskAccess" />
                </td>
                <td>
                    <as:PlaceHolder ID="uxPlcRiskMng" runat="server">
                        <as:CheckBox ID="uxRiskManagement" runat="server" Text="Risk Management" AutoPostBack="true" OnCheckedChanged="UserControls_RiskManagement_CheckedChanged" meta:resourcekey="uxRiskManagementResource1" />
                    </as:PlaceHolder>
                    <as:CheckBoxList ID="uxAFRiskAccess" CssClass="checkbox-list checkbox-list-span" runat="server" AutoPostBack="true"
                        DataTextField="Description" DataValueField="PermissionId" meta:resourcekey="uxAccessListFunctionResource1">
                    </as:CheckBoxList>
                </td>
            </tr>
            <tr id="uxtrSecurityAccess" runat="server">
                <td class="heading">
                    <asp:Literal ID="Literal14" runat="server" Text="Access Functions:" meta:resourcekey="AccessFuntionControlGroupFuncLabel_SecurityAccess" />
                </td>
                <td>
                    <as:CheckBoxList ID="uxAFSecurityAccess" CssClass="checkbox-list checkbox-list-span" runat="server"
                        AutoPostBack="true" DataTextField="Description" DataValueField="PermissionId" meta:resourcekey="uxAccessListFunctionResource1">
                    </as:CheckBoxList>
                </td>
            </tr>
            <tr id="uxtrProcessingDataAccess" runat="server">
                <td class="heading">
                    <asp:Literal ID="Literal19" runat="server" Text="Access Functions:" meta:resourcekey="AccessFuntionControlGroupFuncLabel_ProcessingDataAccess" />
                </td>
                <td>
                    <as:CheckBoxList ID="uxProcessingDateAccess" CssClass="checkbox-list checkbox-list-span" runat="server"
                        AutoPostBack="false" DataTextField="Description" DataValueField="PermissionId" meta:resourcekey="uxAccessListFunctionResource1">
                    </as:CheckBoxList>
                </td>
            </tr>
            <tr id="uxtrAliceAccess" runat="server">
                <td class="heading">
                    <asp:Literal ID="Literal15" runat="server" Text="Access Functions:" meta:resourcekey="AccessFuntionControlGroupFuncLabel_AliceAccess" />
                </td>
                <td>
                    <as:CheckBoxList ID="uxAFAliceAccess" CssClass="checkbox-list checkbox-list-span" runat="server" AutoPostBack="true"
                        DataTextField="Description" DataValueField="PermissionId" meta:resourcekey="uxAccessListFunctionResource1">
                    </as:CheckBoxList>
                    <div class="mt-1x"></div>
                    <as:LinkButton ID="uxLnkAssignActivityGroups" runat="server" Text="Task Permissions"
                        OnClientClick="parent.ShowPopupModalChild(1,'AssignActivityGroupModal.aspx?type=user','auto'); return false;" meta:resourcekey="uxLnkAssignActivityGroupsResource1" />
                </td>
            </tr>
            <tr id="uxtrExternalAccess" runat="server">
                <td class="heading">
                    <asp:Literal ID="Literal4" runat="server" Text="External Access" meta:resourcekey="uxExternalAccess" />
                </td>
                <td>
                    <as:CheckBoxList ID="uxAFExternalAccess" CssClass="checkbox-list checkbox-list-span" runat="server" OnSelectedIndexChanged="uxAFExternalAccess_SelectedIndexChanged"
                        AutoPostBack="true" DataTextField="Description" DataValueField="PermissionId">
                    </as:CheckBoxList>
                    <div id="divApiPasswordText" class="control-password ml-6x">
                        <asp:Label ID="lblApiPasswordText" Text="Reporting API Password:" runat="server"></asp:Label>
                    </div>
                    <div id="divApiPassword" class="control-password ml-6x">
                        <div class="pass">
                            <asp:Label ID="txtAPIPassword" runat="server" CssClass="hide"></asp:Label>
                            <span id="txtHideApiPassword"></span>
                        </div>
                        <as:LinkButton ID="btnShowPassword" AutoPostBack="false" class="icon-pass" runat="server" OnClientClick="showApiPassword_Click(this)"></as:LinkButton>
                    </div>
                </td>
            </tr>
            <tr id="uxTrNotification" runat="server">
                <td class="heading">
                    <asp:Literal ID="Literal17" runat="server" Text="Notification Access Functions" meta:resourcekey="AccessFuntionControlGroupFuncLabel_NotificationAccess" />
                </td>
                <td>
                    <as:CheckBoxList ID="uxNotificationAccess" CssClass="checkbox-list checkbox-list-span" runat="server" AutoPostBack="false"
                        DataTextField="Description" DataValueField="PermissionId" meta:resourcekey="uxAccessListFunctionResource1">
                    </as:CheckBoxList>
                    <div class="mt-1x"></div>
                </td>
            </tr>
            <tr runat="server" id="uxRowSponsorAccessFunction">
                <td class="heading">
                    <asp:Literal ID="Literal21" runat="server" meta:resourcekey="SponsorAccessFunctions" />
                </td>
                <td>
                    <as:CheckBoxList ID="uxSponsorAccess" CssClass="checkbox-list checkbox-list-span" runat="server" AutoPostBack="false"
                        DataTextField="Description" DataValueField="PermissionId" 
                        meta:resourcekey="uxAccessListFunctionResource1">
                    </as:CheckBoxList>
                </td>
            </tr>
            <tr id="uxTrDocumentType" runat="server">
                <td class="heading">
                    <asp:Literal ID="Literal18" runat="server" Text="Manage Document Types Access Functions" meta:resourcekey="AccessFuntionControlGroupFuncLabel_DocumentTypeAccess" />
                </td>
                <td>
                    <as:CheckBoxList ID="uxManageDocumentTypesAccess" CssClass="checkbox-list checkbox-list-span" runat="server" AutoPostBack="false"
                        DataTextField="Description" DataValueField="PermissionId" meta:resourcekey="uxAccessListFunctionResource1">
                    </as:CheckBoxList>
                    <div class="mt-1x"></div>
                </td>
            </tr>

        </as:Panel>
        <uc:CreateNewUserMasterUserControl runat="server" ID="uxMasterUserControl" />
    </tbody>
</table>
<asp:PlaceHolder ID="uxCreateUserTip" runat="server" Visible="False">
    <table width="100%" class="ASTable">
        <tr>
            <td align="center" class="privacy">
                <p>
                    <asp:Literal ID="Literal5" runat="server" Text="After selecting submit, a temporary 72 hour password will be displayed." meta:resourcekey="LiteralResource5" />
                    <br />
                    <asp:Literal ID="Literal6" runat="server" Text="Please copy or record this temporary 72 hour password and send it in a secure manner
                        to the new user along with the user name."
                        meta:resourcekey="LiteralResource6" />
                </p>
                <p>
                    <asp:Literal ID="Literal7" runat="server" Text="The new user must log on within 72 hours and will be prompted to change the temporary password." meta:resourcekey="LiteralResource7" />
                </p>
            </td>
        </tr>
    </table>
</asp:PlaceHolder>

<div class="row">
    <div class="col-md-12 form-action-container text-right">
        <as:Button ID="uxSave" CssClass="btn btn-default" runat="server" Text="Submit" ValidationGroup="ValidatePage"
            OnClick="uxSave_Click" UseSubmitBehavior="False" OnClientClick="if(!ValidateData()){return false;} this.disabled=true; this.value='Processing'; 
                $('#' + uxCancel_ClientID).attr('disabled', true);"
            IsStandardButton="False" meta:resourcekey="uxSaveResource1" />
        <as:Button ID="uxCancel" CssClass="btn btn-default" Text="Cancel" runat="server" OnClientClick="return closeMe();" IsStandardButton="False" meta:resourcekey="uxCancelResource1" />
    </div>
</div>
<div class="display-none">
    <as:Button ID="uxRebindOrgSelected" runat="server" OnClick="uxRebindOrgSelected_Click" IsStandardButton="true" meta:resourcekey="uxRebindAssignmentGroupsResource1" />
</div>

<as:ASCommandControl ID="uxGetAssociatedOrg" runat="server" CallServerFunc="GetAssociatedOrg" ClientSuccessCallbackFunc="getAssociatedOrgResponse" OnCreateResponseData="uxGetAssociatedOrg_CreateResponseData"></as:ASCommandControl>
<asp:HiddenField ID="uxHddAllowToSetStatus" runat="server" Value="N" />
<asp:HiddenField ID="uxHddUrlParameter" runat="server" />
<asp:HiddenField ID="hdnCheckBoxItem" runat="server" />

<as:Validator ID="Validator1" runat="server" ValidationFunction="ValidateInput" MessageContainerClientID="" MessageType="Inline" meta:resourcekey="Validator1Resource1">
    <Items>
        <as:BasicValidationItem ControlToValidateID="uxUsername" Rule="Required" Message="This field is required and must be unique." meta:resourcekey="ManageUserASCX_Text_RequiredAndUnique" />
        <as:BasicValidationItem ControlToValidateID="uxUsernameSignOn" Rule="Required" Message="This field is required and must be unique." meta:resourcekey="ManageUserASCX_Text_RequiredAndUnique" />
        <as:BasicValidationItem ControlToValidateID="uxFirstName" Rule="Required" Message="This is a required field." meta:resourcekey="ManageUserASCX_Text_Required" IsInAjaxPanel="true" />
        <as:BasicValidationItem ControlToValidateID="uxFirstName" Rule="StringUnaccept" Pattern="<>" Message="Contains invalid characters. Please re-enter your First Name." meta:resourcekey="ManageUserASCX_Text_InvalidFirstName" IsInAjaxPanel="true" />
        <as:BasicValidationItem ControlToValidateID="uxLastName" Rule="Required" Message="This is a required field." meta:resourcekey="ManageUserASCX_Text_Required" IsInAjaxPanel="true" />
        <as:BasicValidationItem ControlToValidateID="uxLastName" Rule="StringUnaccept" Pattern="<>" Message="Contains invalid characters. Please re-enter your Last Name." meta:resourcekey="ManageUserASCX_Text_InvalidLastName" IsInAjaxPanel="true" />
        <as:BasicValidationItem ControlToValidateID="uxEmail" Rule="Required" Message="This is a required field." meta:resourcekey="ManageUserASCX_Text_Required" IsInAjaxPanel="true" />
        <as:BasicValidationItem ControlToValidateID="uxUsername" Rule="Minlength" MinLength="7"
            Message="You must enter at least 7 characters." meta:resourcekey="ManageUserASCX_Text_AtLeast7Chars" />
        <as:BasicValidationItem ControlToValidateID="uxEmail" Rule="Required" Message="This is a required field." meta:resourcekey="ManageUserASCX_Text_Required" IsInAjaxPanel="true" />
        <as:BasicValidationItem ControlToValidateID="uxUsernameSignOn" Rule="Minlength" MinLength="7"
            Message="You must enter at least 7 characters." meta:resourcekey="ManageUserASCX_Text_AtLeast7Chars" />       
        
        <as:BasicValidationItem ControlToValidateID ="uxUsername" Rule="StringAccept" Pattern="0123456789qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM.-_" Message="Invalid user name" meta:resourcekey="ManageUserASCX_Text_InvalidUsername" />      
        
        <as:BasicValidationItem ControlToValidateID ="uxUsernameSignOn" Rule="StringAccept" Pattern="0123456789qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM.-_" Message="Invalid user name" meta:resourcekey="ManageUserASCX_Text_InvalidUsername" />
        <as:BasicValidationItem ControlToValidateID="uxFirstName" Rule="Minlength" MinLength="2"
            Message="You must enter at least 2 characters." meta:resourcekey="ManageUserASCX_Text_AtLeast2Chars" IsInAjaxPanel="true" />
        <as:BasicValidationItem ControlToValidateID="uxLastName" Rule="Minlength" MinLength="2"
            Message="You must enter at least 2 characters." meta:resourcekey="ManageUserASCX_Text_AtLeast2Chars" IsInAjaxPanel="true" />
        <as:BasicValidationItem ControlToValidateID="uxEmail" Rule="Minlength" MinLength="2"
            Message="You must enter at least 2 characters." meta:resourcekey="ManageUserASCX_Text_AtLeast2Chars" IsInAjaxPanel="true" />
        <as:CustomValidationItem ClientValidationFunction="validateEmail" ControlToValidateID="uxEmail" 
            Message="You have entered an incorrect email address format. Please try again." meta:resourcekey="ManageUserASCX_Text_IncorrectEmail" IsInAjaxPanel="true" />
        <as:CustomValidationItem ClientValidationFunction="validateOwnership" ControlToValidateID="uxOwnershipGroupDefault" Message="This is a required field." meta:resourcekey="ManageUserASCX_Text_Required" />
    </Items>
</as:Validator>

<as:Validator ID="Validator2" runat="server" ValidationFunction="ValidateInput1" MessageContainerClientID="" MessageType="Inline" meta:resourcekey="Validator1Resource1">
    <Items>
        <as:CustomValidationItem IsInAjaxPanel="true" ClientValidationFunction="validateSaleRepCode" ControlToValidateID="uxSaleRepCode" Message="This is a required field."
            meta:resourcekey="ManageUserASCX_Text_Required" />
        <as:BasicValidationItem ControlToValidateID="uxSaleRepCode" Rule="Digits" IsOnlyValidateWhenVisible="true" IsInAjaxPanel="true"
            Message="Please enter no fewer than 4 digits." meta:resourcekey="ManageUserASCX_Text_AtLeast4DigitChars" />
        <as:BasicValidationItem ControlToValidateID="uxSaleRepCode" Rule="Minlength" MinLength="4" IsOnlyValidateWhenVisible="true" IsInAjaxPanel="true"
            Message="Please enter no fewer than 4 digits." meta:resourcekey="ManageUserASCX_Text_AtLeast4DigitChars" />
        <as:BasicValidationItem ControlToValidateID="uxSaleRepCode" Rule="Maxlength" MaxLength="4" IsOnlyValidateWhenVisible="true" IsInAjaxPanel="true"
            Message="Please enter no fewer than 4 digits." meta:resourcekey="ManageUserASCX_Text_AtLeast4DigitChars" />

    </Items>
</as:Validator>
<as:Validator ID="ValidatorUsername" runat="server" ValidationFunction="ValidateUsername" MessageType="Inline" MessageContainerClientID="" meta:resourcekey="ValidatorResource1">
    <Items>
        <as:BasicValidationItem ControlToValidateID="uxUsername" Rule="Minlength" MinLength="7" Message="You must enter at least 7 characters." meta:resourcekey="ManageUserASCX_Text_AtLeast7Chars" />        
        <as:BasicValidationItem ControlToValidateID ="uxUsername" Rule="StringAccept" Pattern="0123456789qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM.-_" Message="Invalid user name" meta:resourcekey="ManageUserASCX_Text_InvalidUsername" />
    </Items>
</as:Validator>
<as:Validator ID="ValidatorUsernameSignOn" runat="server" ValidationFunction="ValidateUsernameSignOn" MessageType="Inline" MessageContainerClientID="" meta:resourcekey="ValidatorResource1">
    <Items>
        <as:BasicValidationItem ControlToValidateID="uxUsernameSignOn" Rule="Minlength" MinLength="7" Message="You must enter at least 7 characters." meta:resourcekey="ManageUserASCX_Text_AtLeast7Chars" />
        <%--<as:BasicValidationItem ControlToValidateID="uxUsernameSignOn" Rule="AlphaNumeric" Message="Invalid user name" meta:resourcekey="ManageUserASCX_Text_InvalidUsername" />--%>
        <as:BasicValidationItem ControlToValidateID ="uxUsernameSignOn" Rule="StringAccept" Pattern="0123456789qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM.-_" Message="Invalid user name" meta:resourcekey="ManageUserASCX_Text_InvalidUsername" />
    </Items>
</as:Validator>
<as:Validator ID="Validator4" runat="server" ValidationFunction="ValidateFirstName" MessageType="Inline" MessageContainerClientID="" meta:resourcekey="ValidatorResource1">
    <Items>
        <as:RegExValidationItem ControlToValidateID="uxFirstName" IsInAjaxPanel="true" RegularExpression="^((?!(<[^ \t]))(?!(&#)).)*$" meta:resourcekey="ManageUserASCX_Text_InvalidFirstName" />
    </Items>
</as:Validator>
<as:Validator ID="Validator5" runat="server" ValidationFunction="ValidateLastName" MessageType="Inline" MessageContainerClientID="" meta:resourcekey="ValidatorResource1">
    <Items>
        <as:RegExValidationItem ControlToValidateID="uxLastName" IsInAjaxPanel="true" RegularExpression="^((?!(<[^ \t]))(?!(&#)).)*$" meta:resourcekey="ManageUserASCX_Text_InvalidLastName" />
    </Items>
</as:Validator>
<as:Validator ID="Validator6" runat="server" ValidationFunction="ValidateEmail" MessageType="Inline" MessageContainerClientID="" meta:resourcekey="ValidatorResource1">
    <Items>
        <as:RegExValidationItem ControlToValidateID="uxEmail" IsInAjaxPanel="true" RegularExpression="^((?!(<[^ \t]))(?!(&#)).)*$" meta:resourcekey="ManageUserASCX_Text_IncorrectEmail" />
    </Items>
</as:Validator>

<tek:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <script type="text/javascript">

        var IsFromWindowPopup = <% =IsFromWindowPopup.ToString().ToLower() %>;
        var uxActive_ClientID = '<%=uxActive.ClientID %>';
        var uxHddUrlParameter_ClientID = '<%=uxHddUrlParameter.ClientID%>';
        var uxActive_ClientID = '<%=uxActive.ClientID %>'; 
        var uxSave_ClientID = '<%=uxSave.ClientID %>';
        var uxCancel_ClientID = '<%=uxCancel.ClientID %>';
        var uxRole_ClientID = '<%=uxRole.ClientID %>';
        var uxSaleRepCode= '<%=uxSaleRepCode.ClientID%>';
        var uxRebindOrgSelected= '<%=uxRebindOrgSelected.ClientID%>';
        var lbNumSelectedOrg = '<%=lbNumSelectedOrg.ClientID%>';
        var uxOwnershipGroupDefault_ClientID = '<%=uxOwnershipGroupDefault.ClientID %>';
        var txtOwnershipGroup_ClientID = '<%=txtOwnershipGroup.ClientID %>';
        var uxOwnershipGroupDefault_EmptyMessage = '<%= uxOwnershipGroupDefault.EmptyMessage  %>';
        var uxOwnershipGroupList_ClientID = '<%=uxOwnershipGroupList.ClientID %>';
        var lbOrganizationAssociated = '<%=GetLocalResourceObject("lbOrganizationAssociated")%>';
        var lbNumAssociatedOrg_ClientID = '<%=lbNumAssociatedOrg.ClientID%>';
        var currentSaleRepCode = 0;
        var uxAFSecurityAccess_ClientID = '<%=uxAFSecurityAccess.ClientID%>';
        var txtAPIPassword_ClientID = '<%=txtAPIPassword.ClientID%>';
        var lblApiPasswordText_ClientID = '<%=lblApiPasswordText.ClientID%>';
        var UMMasterUserControl_ValidationFunction = '<%= uxMasterUserControl.ClientIDValidationFunction %>';
        var uxEmail_ClientID = '<%= uxEmail.ClientID %>';
        var uxUsernameId = '<%= uxUsername.ClientID%>';
        var uxUsernameSignOnId = '<%= uxUsernameSignOn.ClientID%>';
        var uxFirstNameId = '<%= uxFirstName.ClientID%>';
        var uxLastNameId = '<%= uxLastName.ClientID%>';
        var uxApproveGroupRow = '<%= uxApproveGroupRow.ClientID%>';
    </script>
    <script src="<%= ResolveUrl("~/")%>res/js/usermaintenance/ManageUser.js"></script>
</tek:RadCodeBlock>


