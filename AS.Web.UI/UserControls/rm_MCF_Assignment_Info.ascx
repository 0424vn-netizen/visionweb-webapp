<%@ Control Language="C#" AutoEventWireup="true" CodeFile="rm_MCF_Assignment_Info.ascx.cs"
    Inherits="UserControls_rm_MCF_Assignment_Info" %>
<%@ Register Assembly="AS.Controls" Namespace="AS.Controls.Validators" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/rm_MCF_Assignment_User.ascx" TagName="User" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/rm_MCF_Assignment_Group.ascx" TagName="Group" TagPrefix="uc" %>
<style type="text/css">
    </style>
<as:RadAjaxManagerProxy ID="RadAjaxManagerProxy1" runat="server">
    <AjaxSettings>
        <tek:AjaxSetting AjaxControlID="uxRebindAssignmentGroups">
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="divAssignmentGroups" />
            </UpdatedControls>
        </tek:AjaxSetting>
        <tek:AjaxSetting AjaxControlID="uxRebindAssignmentUsers">
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="divAssignmentUsers" />
            </UpdatedControls>
        </tek:AjaxSetting>
        <tek:AjaxSetting AjaxControlID="uxRebindRebindDistribution">
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="divSubsiteDistribution" />
            </UpdatedControls>
        </tek:AjaxSetting>
    </AjaxSettings>
</as:RadAjaxManagerProxy>


<as:Validator ID="Validator1" MessageType="Inline" ValidationFunction="ValidateAssignmentGeneralInformationByValidator"
    runat="server" MessageContainerClientID="" meta:resourcekey="Validator1Resource1">
    <Items>
        <as:BasicValidationItem Rule="Required" ResMessage="Resources.ValMsg.Required_Unique" ResParams="Assignment Name" ControlToValidateID="uxAssignmentName" />

        <as:CustomValidationItem ResMessage="Resources.RiskMessageManager.Assignment_GeneralInformation_SpecialCharacters"
            ResParams="Assignment Name" ClientValidationFunction="ValidateSpecialCharacters"
            ControlToValidateID="uxAssignmentName" />
        <as:CustomValidationItem ResMessage="Resources.ValMsg.Assignment_SpecialCharacters" ResParams="Assignment Name" ClientValidationFunction="ValidateSpecialCharacters" ControlToValidateID="uxAssignmentName" />
        <as:BasicValidationItem Rule="Required" ResMessage="Resources.ValMsg.Required" ResParams="Start Date" ControlToValidateID="uxStartDate" />
        <as:BasicValidationItem Rule="Required" ResMessage="Resources.ValMsg.Required" ResParams="Expiration Date" ControlToValidateID="uxExpirationDate" />
        <as:CustomValidationItem ResMessage="Resources.ValMsg.InvalidDateFormat" ClientValidationFunction="ValidateValidExpiredDate" ControlToValidateID="uxExpirationDate" />
        <as:CustomValidationItem ResMessage="Resources.ValMsg.Assignment_SelectedDate_Less_Today" ClientValidationFunction="ValidateLessTodayExpiredDate" ControlToValidateID="uxExpirationDate" />
        <as:CustomValidationItem ResMessage="Resources.ValMsg.Assignment_SelectedDate_Less_StartDate" ClientValidationFunction="ValidateLessStartDateByExpiredDate" ControlToValidateID="uxExpirationDate" />
    </Items>
</as:Validator>
<as:Validator ID="Validator3" MessageType="Inline" ValidationFunction="ValidateAssignmentName"
    runat="server" MessageContainerClientID="" meta:resourcekey="Validator1Resource1">
    <Items>
        <as:CustomValidationItem ResMessage="Resources.ValMsg.Assignment_SpecialCharacters" ResParams="Assignment Name" ClientValidationFunction="ValidateSpecialCharacters" ControlToValidateID="uxAssignmentName" />
    </Items>
</as:Validator>
<as:Validator ID="Validator4" MessageType="Inline" ValidationFunction="ValidateExpirationDate"
    runat="server" MessageContainerClientID="" meta:resourcekey="Validator1Resource1">
    <Items>
        <as:BasicValidationItem Rule="Required" ResMessage="Resources.ValMsg.Required" ResParams="Expiration Date" ControlToValidateID="uxExpirationDate" />
        <as:CustomValidationItem ResMessage="Resources.ValMsg.InvalidDateFormat" ClientValidationFunction="ValidateValidExpiredDate" ControlToValidateID="uxExpirationDate" />
        <as:CustomValidationItem ResMessage="Resources.ValMsg.Assignment_SelectedDate_Less_Today" ClientValidationFunction="ValidateLessTodayExpiredDate" ControlToValidateID="uxExpirationDate" />
        <as:CustomValidationItem ResMessage="Resources.ValMsg.Assignment_SelectedDate_Less_StartDate" ClientValidationFunction="ValidateLessStartDateByExpiredDate" ControlToValidateID="uxExpirationDate" />
    </Items>
</as:Validator>

<div class="row">
    <div class="col-md-12" data-toggle="collapse" data-target="#uxAssignmentInfoGrid">
        <h2 class="grid-title on-top">
            <as:Literal ID="ltAssignmentInfo" runat="server" Text="Assignment Information" meta:resourcekey="ltAssignmentInfoResource1"></as:Literal>
        </h2>
    </div>
</div>


<div class="row">
    <as:PlaceHolder runat="server" ID="uxPlAssignmentInfo">
        <div class="col-md-12 in" id="uxAssignmentInfoGrid">
            <table class="ASTable">
                <tr class="Row">
                    <td class="heading" style="width: 17%;">
                        <as:ValidatorLabel ID="VltLblAssignmentName" runat="server" CssClass="control-label" Text="Assignment Name:" ApplyFor="uxAssignmentName" meta:resourcekey="VltLblAssignmentNameResource1"></as:ValidatorLabel>
                    </td>
                    <td>
                        <div class="control-inline last">
                            <asp:TextBox ID="uxAssignmentName" tracking-key="AssignmentName" runat="server" Width="400px" MaxLength="50" CssClass="form-control" meta:resourcekey="uxAssignmentNameResource1"></asp:TextBox>
                        </div>
                        <as:ValidatorMessage runat="server" ID="uxAssignmentNameMsg" ApplyFor="uxAssignmentName" Message="" ShowOnLoad="False" />
                    </td>
                    <%=FeatureMode == WebSiteEnums.FeatureMode.Edit ? "<td class=\"action-column\">&nbsp;</td>" : ""%>
                </tr>
                <as:PlaceHolder ID="uxPlaceHolderAssignmentType" runat="server" Visible="False">
                    <tr class="MPSBorderAltRow" align="left">
                        <td style="width: 17%" class="heading">
                            <div class="control-inline">
                                <label class="control-label">
                                    <as:Literal ID="Literal1" runat="server" Text="Assignment Type:" meta:resourcekey="Literal1Resource1"></as:Literal></label>
                            </div>
                        </td>
                        <td>
                            <div class="control-inline">
                                <as:RadComboBox ID="uxComboAssignmentType" runat="server" Width="200px" EnableEmbeddedSkins="false" tracking-key="AssignmentType" tracking-type="combobox"
                                    OnSelectedIndexChanged="uxComboAssignmentType_SelectedIndexChanged" OnClientSelectedIndexChanging="uxComboAssignmentType_Changing" AutoPostBack="true" meta:resourcekey="uxComboAssignmentTypeResource1" />
                            </div>
                            <as:PlaceHolder ID="uxPlaceHolderDistinctExclude" runat="server" Visible="false">
                                <div class="control-inline last">
                                    <as:CheckBox ID="uxChkDistinctExclude" Width="200px" Text="Exclude from DQ Distinct" runat="server" tracking-key="AssignmentTypeExclude"
                                        tracking-type="checkbox" tracking-required="true" meta:resourcekey="uxChkDistinctExcludeResource1" />
                                </div>
                            </as:PlaceHolder>

                        </td>
                        <%=FeatureMode == WebSiteEnums.FeatureMode.Edit ? "<td >&nbsp;</td>" : ""%>
                    </tr>
                </as:PlaceHolder>
                <as:PlaceHolder ID="uxPlaceHolderStartDate" runat="server" Visible="false">
                    <tr class="Row">
                        <td class="heading">
                            <as:ValidatorLabel ID="VltLblStartDate" runat="server" CssClass="control-label" Text="Start Date:" ApplyFor="uxStartDate" meta:resourcekey="VltLblStartDateResource1"></as:ValidatorLabel>
                        </td>
                        <td>
                            <div class="control-inline last">
                                <as:RadDatePicker ID="uxStartDate" runat="server" ShowPopupOnFocus="true" Width="100px"
                                    ClientEvents-OnDateSelected="uxStartDate_SelectedChange"
                                    meta:resourcekey="uxStartDateResource1">
                                    <Calendar FastNavigationStep="12" ShowRowHeaders="false"></Calendar>
                                    <DateInput ID="DateInput2" DateFormat="MM/dd/yyyy" runat="Server" LabelWidth="64px" Width="" tracking-key="StartDate">
                                        <EmptyMessageStyle Resize="None"></EmptyMessageStyle>
                                        <ReadOnlyStyle Resize="None"></ReadOnlyStyle>
                                        <FocusedStyle Resize="None"></FocusedStyle>
                                        <DisabledStyle Resize="None"></DisabledStyle>
                                        <InvalidStyle Resize="None"></InvalidStyle>
                                        <HoveredStyle Resize="None"></HoveredStyle>
                                        <EnabledStyle Resize="None"></EnabledStyle>
                                    </DateInput>
                                    <DatePopupButton ImageUrl="" HoverImageUrl="" CssClass=""></DatePopupButton>
                                </as:RadDatePicker>
                            </div>
                            <as:ValidatorMessage runat="server" ID="uxStartDateMsg" ApplyFor="uxStartDate" />
                        </td>
                        <%=FeatureMode == WebSiteEnums.FeatureMode.Edit ? "<td class=\"action-column\">&nbsp;</td>" : ""%>
                    </tr>
                </as:PlaceHolder>
                <tr class="Row">
                    <td class="heading">
                        <as:ValidatorLabel ID="ValidatorLabel1" runat="server" CssClass="control-label" Text="Expiration Date:" ApplyFor="uxExpirationDate" meta:resourcekey="ValidatorLabel1Resource1"></as:ValidatorLabel>
                    </td>
                    <td>
                        <asp:PlaceHolder ID="uxNeverExpire" runat="server">
                            <div class="control-inline">
                                <asp:RadioButton ID="uxradNerverExpirationDate" runat="server" Text=" Never Expire" tracking-key="ExpirationDate" tracking-more="NeverExpire"
                                    GroupName="Expire" onclick="OnchangeExpirationDate(1);" meta:resourcekey="uxradNerverExpirationDateResource1" />
                            </div>
                        </asp:PlaceHolder>
                        <asp:PlaceHolder ID="uxExpireDate" runat="server">
                            <div class="control-inline last ">
                                <asp:RadioButton ID="uxradExpirationDate" runat="server" Text="" GroupName="Expire" tracking-key="ExpirationDate" tracking-type="radio" tracking-refer="uxExpirationDate"
                                    tracking-refer-type="date" onclick="OnchangeExpirationDate(2);" meta:resourcekey="uxradExpirationDateResource1" CssClass="inline-block" />
                                <span class="last middle">
                                    <as:RadDatePicker ID="uxExpirationDate" runat="server" ShowPopupOnFocus="true"  ClientEvents-OnDateSelected="uxExpirationDate_SelectedChange" meta:resourcekey="uxExpirationDateResource1">
                                        <Calendar FastNavigationStep="12" ShowRowHeaders="false"></Calendar>
                                        <DateInput ID="DateInput1" DateFormat="MM/dd/yyyy" runat="Server" LabelWidth="64px" Width="">
                                            <EmptyMessageStyle Resize="None"></EmptyMessageStyle>

                                            <ReadOnlyStyle Resize="None"></ReadOnlyStyle>

                                            <FocusedStyle Resize="None"></FocusedStyle>

                                            <DisabledStyle Resize="None"></DisabledStyle>

                                            <InvalidStyle Resize="None"></InvalidStyle>

                                            <HoveredStyle Resize="None"></HoveredStyle>

                                            <EnabledStyle Resize="None"></EnabledStyle>
                                        </DateInput>

                                        <DatePopupButton ImageUrl="" HoverImageUrl="" CssClass=""></DatePopupButton>
                                    </as:RadDatePicker>
                                </span>
                            </div>
                            <as:ValidatorMessage runat="server" ID="uxExpirationDateMsg" ApplyFor="uxExpirationDate" />
                        </asp:PlaceHolder>
                    </td>
                    <%=FeatureMode == WebSiteEnums.FeatureMode.Edit ? "<td class=\"action-column\">&nbsp;</td>" : ""%>
                </tr>
                <as:PlaceHolder ID="uxPhUserGroup" runat="server" Visible="False">
                    <tr class="AltRow">
                        <td class="heading">
                            <as:Literal ID="Literal2" runat="server" Text="Group:" meta:resourcekey="Literal2Resource1"></as:Literal></td>
                        <td runat="server" id="tdGroups">
                            <div id="divAssignmentGroups" runat="server">
                                <asp:Label tracking-key="GroupInfo" tracking-type="hierarchy" ID="uxLabelAssignmentGroups" runat="server" meta:resourcekey="uxLabelAssignmentGroupsResource1" />
                            </div>
                        </td>
                        <asp:PlaceHolder ID="plhGroups" runat="server">
                            <td class='action-column'>
                                <a href="#" onclick="return CallFilterModal('rm_MCF_AssignmentGroups_Modal.aspx');">
                                    <as:Literal ID="Literal3" runat="server" Text="Edit" meta:resourcekey="Literal3Resource1"></as:Literal></a>
                            </td>
                        </asp:PlaceHolder>
                    </tr>
                    <tr class="Row">
                        <td class="heading">
                            <as:Literal ID="Literal5" runat="server" Text="User:" meta:resourcekey="Literal5Resource1"></as:Literal></td>
                        <td runat="server" id="tdUsers">
                            <div id="divAssignmentUsers" runat="server">
                                <asp:Label tracking-key="User" tracking-type="hierarchy" ID="uxLabelAssignmentUsers" runat="server" meta:resourcekey="uxLabelAssignmentUsersResource1" />
                            </div>
                        </td>
                        <asp:PlaceHolder ID="plhUsers" runat="server">
                            <td class='action-column'>
                                <a href="#" onclick="return CallFilterModal('rm_MCF_AssignmentUsers_Modal.aspx');">
                                    <as:Literal ID="Literal4" runat="server" Text="Edit" meta:resourcekey="Literal4Resource1"></as:Literal></a>
                            </td>
                        </asp:PlaceHolder>
                    </tr>
                </as:PlaceHolder>

                <as:PlaceHolder ID="uxPhSubsiteDistribution" runat="server" Visible="False">
                    <tr class="AltRow">
                        <td class="heading">
                            <as:Literal ID="Literal6" runat="server" Text="Subsite Distribution:" meta:resourcekey="SubsiteDistribution"></as:Literal>
                        </td>
                        <td runat="server" id="tdSubsiteDistribution">
                            <div id="divSubsiteDistribution" runat="server">
                                <asp:Label tracking-key="SubsiteDistribution" tracking-type="hierarchy" ID="uxLabelSubsiteDistribution" runat="server" meta:resourcekey="uxLabelAssignmentGroupsResource1" />
                            </div>
                        </td>
                        <asp:PlaceHolder ID="phSubsite" runat="server">
                            <td class='action-column'>
                                <a href="#" onclick="return CallFilterModal('rm_MCF_SubsiteDistributions_Modal.aspx');">
                                    <as:Literal ID="Literal7" runat="server" Text="Edit" meta:resourcekey="Literal3Resource1"></as:Literal></a>
                            </td>
                        </asp:PlaceHolder>
                    </tr>
                </as:PlaceHolder>
            </table>
        </div>
    </as:PlaceHolder>
</div>

<div class="display-none">
    <!--invisible buttons-->
    <as:Button ID="uxRebindAssignmentGroups" runat="server" OnClick="uxRebindAssignmentGroups_Click" IsStandardButton="False" meta:resourcekey="uxRebindAssignmentGroupsResource1" />
    <as:Button ID="uxRebindAssignmentUsers" runat="server" OnClick="uxRebindAssignmentUsers_Click" IsStandardButton="False" meta:resourcekey="uxRebindAssignmentUsersResource1" />
    <as:Button ID="uxRebindRebindDistribution" runat="server" OnClick="uxRebindRebindDistribution_Click" IsStandardButton="False" />
</div>


<as:RadCodeBlock ID="JavaScript" runat="server">
    <script type="text/javascript">
        var Risk_Assignment_Info_uxAssignmentName = '<%= uxAssignmentName.ClientID %>';
        var Risk_Assignment_Info_uxStartDate = '<%= uxStartDate.ClientID %>';
        var Risk_Assignment_Info_uxExpirationDate = '<%= uxExpirationDate.ClientID %>';
        var Risk_Assignment_Info_uxRadExpirationDate = '<%= uxradExpirationDate.ClientID %>';
        var Risk_Assignment_Info_uxRadNeverExpire = '<%= uxradNerverExpirationDate.ClientID %>';

        var Risk_Assignment_Info_uxRebindAssignmentGroups = '<%= uxRebindAssignmentGroups.ClientID %>';
        var Risk_Assignment_Info_uxRebindAssignmentUsers = '<%= uxRebindAssignmentUsers.ClientID %>';
        var Risk_Assignment_Info_uxRebindRebindDistribution = '<%= uxRebindRebindDistribution.ClientID %>';
        var Risk_Assignment_Info_divAssignmentGroups = '#<%=divAssignmentGroups.ClientID %>';
        var Risk_Assignment_Info_divAssignmentUsers = '#<%=divAssignmentUsers.ClientID %>';
        var Risk_Assignment_Info_Assignment_GroupUser_Required = '<%=Resources.ValMsg.Assignment_GroupUser_Required %>';


        var Risk_Assignment_Info_QueryString = '?<%=QueryString %>';
        var Risk_Assignment_uxComboAssignmentType_ClientID = '<%=uxComboAssignmentType.ClientID %>';
        var Risk_Assignment_enumAssTypeDescription_Client = '<%=WebSiteEnums.GetEnumAssTypeDescription(WebSiteEnums.AssignmentType.DetectionQueue) %>';
        var Risk_Assignment_Info_DateTime_MaxValue = '<%= DateTime.MaxValue.ToString("MM/dd/yyyy") %>';
        var Risk_Assignment_Info_DateTimeToNow = '<%= GetDateToNow().ToString("MM/dd/yyyy") %>'
    </script>
    <script src="<%= ResolveUrl("~/")%>res/js/risk_MCF/rm_MCF_Assignment_Info.js"></script>
</as:RadCodeBlock>
