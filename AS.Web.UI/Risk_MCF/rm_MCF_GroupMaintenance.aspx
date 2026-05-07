<%@ Page Title="Group Maintenance" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="rm_MCF_GroupMaintenance.aspx.cs" Inherits="rm_MCF_GroupMaintenance" meta:resourcekey="PageResource1" %>

<%@ Register Src="~/UserControls/PageTitle.ascx" TagName="ReportTitle" TagPrefix="uc" %>
<%@ Register TagName="UxExport" Src="~/UserControls/UxExport.ascx" TagPrefix="uc" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="Server">
    
    <as:PlaceHolder ID="sdfd" runat="server">
        <div class="row">
            <div class="col-md-12 no-margin-bottom">
                <uc:ReportTitle ID="uxReportTitle" ReportTitle="Group Maintenance" HasFilteringOption="false" meta:resourcekey="uxReportTitleResource1"
                    runat="server" />
            </div>
        </div>
        <div class="height-18"></div>

        <as:Panel ID="uxFilterStatusContainer" runat="server" meta:resourcekey="uxFilterStatusContainerResource1">
            <div class="row">
                <div class="col-md-12 dark-blue">
                    <div class="control-inline">
                        <label class="first">
                            <asp:Literal ID="Literal1" runat="server" meta:resourcekey="Literal1Resource1" Text=" Status:"></asp:Literal></label>
                    </div>
                    <as:RadioButton ID="uxFilterStatusAll" CssClass="control-inline" runat="server" Text="All"
                        GroupName="uxFilterStatus"
                        xValue="-1" onclick="uxFilterStatus_Checked()" meta:resourcekey="uxFilterStatusAllResource1" Value="" />
                    <as:RadioButton ID="uxFilterStatusActive" runat="server" CssClass="control-inline"
                        Text="Active" GroupName="uxFilterStatus"
                        xValue="1" Checked="True" onclick="uxFilterStatus_Checked()" meta:resourcekey="uxFilterStatusActiveResource1" Value="" />
                    <as:RadioButton ID="uxFilterStatusInactive" runat="server" CssClass="control-inline"
                        Text="Inactive" GroupName="uxFilterStatus"
                        xValue="0" onclick="uxFilterStatus_Checked()" meta:resourcekey="uxFilterStatusInactiveResource1" Value="" />
                    <asp:Button ID="uxChangeFilterStatus" runat="server" OnClick="uxChangeFilterStatus_Click"
                        Style="display: none" meta:resourcekey="uxChangeFilterStatusResource1"></asp:Button>
                </div>
            </div>
        </as:Panel>
        <%--Create new Group --%>
        <div class="height-6"></div>
        <div class="row">
            <div class="col-md-12 no-margin-action-container">
                <as:LinkButton CssClass="btn btn-default" runat="server" ID="uxCreateMode" OnClick="uxCreateMode_Click" meta:resourcekey="uxCreateModeResource1">Create New Group</as:LinkButton>
            </div>
            <div class="col-md-12">
                <asp:ValidationSummary ID="valSummary" runat="server" HeaderText="The following errors occured:" Font-Size="Small" CssClass="val-summary" meta:resourcekey="valSummaryResource1" />
            </div>
        </div>

        <div id="pnlAddGroup" runat="server" visible="false" class="text-center risk-form-container">
            <div class="row">
                <div class="col-md-12">
                    <div class="risk-form text-right">
                        <div class="risk-form-row">
                            <as:ValidatorLabel ID="uxGroupLabel" runat="server" Text="Group:" ApplyFor="uxAddGroupText" CssClass="control-label" meta:resourcekey="uxGroupLabelResource1" />
                            <div class="control-inline">
                                <as:TextBox CssClass="form-control" Width="350px" onkeypress="return DefaultEnterOnTextBox(event);"
                                    MaxLength="50" Font-Size="12px"
                                    ID="uxAddGroupText" runat="server" ValidationGroup="Validate" HintCss="hint" meta:resourcekey="uxAddGroupTextResource1"></as:TextBox>
                                <div class="label-error-left">
                                    <as:ValidatorMessage ID="uxAddGroupErrMsg" runat="server" ApplyFor="uxAddGroupText" Message="" ShowOnLoad="False">
                                    </as:ValidatorMessage>
                                </div>
                            </div>
                        </div>
                        <div class="risk-form-row">
                            <as:ValidatorLabel ID="uxDescriptionLabel" runat="server" Text="Description:" ApplyFor="uxAddDescription" CssClass="control-label" meta:resourcekey="uxDescriptionLabelResource1" />
                            <div class="control-inline">
                                <as:TextBox CssClass="form-control" Width="350px" MaxLength="500" TextMode="MultiLine"
                                    Rows="6" ID="uxAddDescription" runat="server" Font-Size="12px"
                                    ValidationGroup="Validate" HintCss="hint" meta:resourcekey="uxAddDescriptionResource1"></as:TextBox>
                                <div class="label-error-left">
                                    <as:ValidatorMessage ID="uxuxAddDescriptionErrMsg" runat="server" ApplyFor="uxAddDescription" Message="" ShowOnLoad="False">
                                    </as:ValidatorMessage>
                                </div>
                            </div>
                        </div>
                        <div class="risk-form-action">
                            <as:Button CssClass="btn btn-default" OnClientClick="return validBeforeSubmitAdd();" ID="uxAddGroup"
                                Text="Submit" OnClick="uxAddGroup_Click" runat="server" IsStandardButton="False" meta:resourcekey="uxAddGroupResource1"></as:Button>
                            <as:Button ID="uxAddCancel" CssClass="btn btn-default" Text="Cancel" OnClick="uxAddGroupCancel_Click"
                                runat="server" IsStandardButton="False" meta:resourcekey="uxAddCancelResource1"></as:Button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="height-24"></div>
        <%--end Create new Group --%>
        <uc:UxExport ID="uxExportTop" IsOnTop="true" runat="server" GridID="uxGroupList" ShowPDF="false"
            GridHeader="Risk Management - List Management - Group Maintenance" meta:resourcekey="uxExportTopResource1" />

        <div class="row" id="uxDivPannel" runat="server" onkeypress="return DefaultEnterOnDiv(event);">
            <div class="col-md-12">
                <as:ASGrid ID="uxGroupList" runat="server" GridLines="None" AllowPaging="True" IsAutoExportTemplate="true"
                    AllowAutomaticUpdates="True" AllowSorting="True" AutoGenerateColumns="False"  CssClass="grid-scroll in"
                    AllowFilteringByColumn="false" OnItemCommand="uxGroupList_ItemCommand"  InsertTempColumnAtTheEnd="false"
                    AllowSortFilterWhenExport="true" meta:resourcekey="uxGroupListResource1">
                    <MasterTableView DataKeyNames="GroupID" CommandItemDisplay="None" EditMode="InPlace">
                        <Columns>
                            <%--Group column --%>
                            <as:ASGridTemplateColumn HeaderText="Group" HeaderTooltip="Group" HeaderStyle-HorizontalAlign="Center"
                                ItemStyle-HorizontalAlign="Left" UniqueName="GroupName" AllowEncodeWhenExport="true" SortExpression="GroupName"
                                DataField="GroupName" meta:resourcekey="ASGridTemplateColumnResource1">
                                <ItemTemplate>
                                    <asp:Literal ID="LiteralGroupName" Text='<%# VeraCodeExtensions.DoVeraCode(System.Web.HttpUtility.HtmlEncode(Eval("GroupName").ToString())) %>'
                                        runat="server" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:HiddenField ID="txtGroupID" runat="server" Value='<%# Eval("GroupID").ToString() %>' />
                                    <asp:TextBox ID="txtEditGroupName" runat="server" MaxLength="50" Text='<%# Eval("GroupName").ToString() %>'
                                        CssClass="txtEditGroupName max-width" meta:resourcekey="txtEditGroupNameResource1" />
                                </EditItemTemplate>

                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                                <ItemStyle HorizontalAlign="Left"></ItemStyle>
                            </as:ASGridTemplateColumn>

                            <%--Description column --%>
                            <as:ASGridTemplateColumn HeaderText="Description" HeaderTooltip="Description" HeaderStyle-HorizontalAlign="Center"
                                ItemStyle-HorizontalAlign="Left" UniqueName="Description" SortExpression="Description"
                                DataField="Description" meta:resourcekey="ASGridTemplateColumnResource2">
                                <ItemTemplate>
                                    <asp:Literal ID="LiteralDescription" Text='<%# VeraCodeExtensions.DoVeraCode(System.Web.HttpUtility.HtmlEncode(Eval("Description").ToString())) %>'
                                        runat="server" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:HiddenField ID="txtActiveValue" runat="server" Value='<%# Eval("IsActive").ToString() %>' />
                                    <asp:TextBox ID="txtEditGroupDescription" runat="server" MaxLength="500" Text='<%# Eval("Description").ToString() %>'
                                        CssClass="txtEditGroupDescription max-width" meta:resourcekey="txtEditGroupDescriptionResource1" />
                                </EditItemTemplate>

                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                                <ItemStyle HorizontalAlign="Left"></ItemStyle>
                            </as:ASGridTemplateColumn>

                            <%--#Members column--%>
                            <as:ASGridBoundColumn HeaderText="# Members" HeaderTooltip="# users who belong to the group"
                                HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Wrap="false"
                                DataField="MemberCount" UniqueName="MemberCount" ASFormat="Integer" ReadOnly="true" meta:resourcekey="ASGridBoundColumnResource1">
                                <ColumnValidationSettings>
                                    <ModelErrorMessage Text=""></ModelErrorMessage>
                                </ColumnValidationSettings>

                                <HeaderStyle HorizontalAlign="Center" Wrap="False"></HeaderStyle>

                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            </as:ASGridBoundColumn>

                            <%--Active column--%>
                            <as:ASGridBoundColumn HeaderText="Active" HeaderStyle-HorizontalAlign="Center"
                                ItemStyle-HorizontalAlign="Center" DataField="IsActiveText" UniqueName="ActivateDeactivateHidden"
                                Display="false" meta:resourcekey="ASGridBoundColumnResource2">
                                <ColumnValidationSettings>
                                    <ModelErrorMessage Text=""></ModelErrorMessage>
                                </ColumnValidationSettings>

                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            </as:ASGridBoundColumn>

                            <%--Activate/Deactivate column--%>
                            <tek:GridHyperLinkColumn HeaderText="Activate/Deactivate" HeaderTooltip="Click to activate/deactivate status"
                                HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="ActivateDeactivate" meta:resourcekey="GridHyperLinkColumnResource1">
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            </tek:GridHyperLinkColumn>

                            <as:ASGridTemplateColumn HeaderText="Edit" HeaderStyle-Width="170px" HeaderTooltip="Click to edit group"
                                HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="Edit" meta:resourcekey="ASGridTemplateColumnResource3">
                                <ItemTemplate>
                                    <asp:LinkButton ID="LinkButton1" runat="server" CommandName="Edit" Text="Edit" meta:resourcekey="LinkButton1Resource1" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <div class="grid-action-container">
                                        <asp:LinkButton ID="LinkButton2" CssClass="btn btn-default btnUpdate" OnClientClick="return doValidInput(this);"
                                            runat="server" CommandName="Update" Text="Submit" meta:resourcekey="LinkButton2Resource1" />
                                        <asp:LinkButton ID="LinkButton3" CssClass="btn btn-default" runat="server" CommandName="Cancel"
                                            Text="Cancel" meta:resourcekey="LinkButton3Resource1" />
                                    </div>
                                </EditItemTemplate>

                                <HeaderStyle HorizontalAlign="Center" Width="170px"></HeaderStyle>

                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            </as:ASGridTemplateColumn>
                        </Columns>
                    </MasterTableView>
                </as:ASGrid>
            </div>
        </div>

        <asp:Button ID="uxActivateDeactivate" runat="server" OnClick="uxActivateDeactivate_Click"
            CssClass="display-none" meta:resourcekey="uxActivateDeactivateResource1" />
        <as:HiddenField ID="uxActivateDeactivateData" runat="server" />

        <as:Validator ID="Validator1" runat="server" ValidationFunction="ValidateInput" MessageType="Inline" MessageContainerClientID="" meta:resourcekey="Validator1Resource1">
            <Items>
                <as:BasicValidationItem ControlToValidateID="uxAddGroupText" Rule="Required" ResMessage="Resources.MessageManager.Field_RequireAndUnique" />
                <as:RegExValidationItem ControlToValidateID="uxAddGroupText" RegularExpression="^[0-9a-zA-Z\.\\s-]{1,50}$"
                    ResMessage="Resources.ValMsg.InvalidCharacter" />
                <as:BasicValidationItem ControlToValidateID="uxAddDescription" Rule="StringUnaccept"
                    Pattern="<>" ResMessage="Resources.ValMsg.InvalidCharacter" />
                <as:BasicValidationItem ControlToValidateID="uxAddDescription" Rule="Maxlength" MaxLength="500"
                    ResMessage="Resources.ValMsg.MaxLength" ResParams="500" />
            </Items>
        </as:Validator>
        <as:Validator ID="Validator2" runat="server" ValidationFunction="ValidateAddGroupText" MessageType="Inline" MessageContainerClientID="" meta:resourcekey="Validator1Resource1">
            <Items>
                <as:RegExValidationItem ControlToValidateID="uxAddGroupText" RegularExpression="^[0-9a-zA-Z\.\\s-]{1,50}$" ResMessage="Resources.ValMsg.InvalidCharacter" />
            </Items>
        </as:Validator>
        <as:Validator ID="Validator3" runat="server" ValidationFunction="ValidateAddDescription" MessageType="Inline" MessageContainerClientID="" meta:resourcekey="Validator1Resource1">
            <Items>
                <as:RegExValidationItem ControlToValidateID="uxAddDescription" RegularExpression="^((?!(<[^ \t]))(?!(&#)).)*$" ResMessage="Resources.ValMsg.InvalidCharacter" />
            </Items>
        </as:Validator>

    </as:PlaceHolder>
    <tek:RadCodeBlock runat="server" ID="RadCodeBlock">
        <script type="text/javascript">
            var uxChangeFilterStatus_ClientID = '<%=uxChangeFilterStatus.ClientID %>';
            var uxAddGroupText_ClientID = '<%=uxAddGroupText.ClientID%>';
            var uxAddDescription_ClientID = '<%=uxAddDescription.ClientID%>';
            var uxAddGroup_ClientID = '<%=uxAddGroup.ClientID%>';
            var uxActivateDeactivateData_ClientID = '<%=uxActivateDeactivateData.ClientID%>';
            var uxActivateDeactivate_ClientID = '<%=uxActivateDeactivate.ClientID%>';
            var Resources_ValMsg_Required = '<%=  Resources.ValMsg.Required %>';
            var Resources_ValMsg_InvalidCharacter = '<%= Resources.ValMsg.InvalidCharacter %>';
            var reggroupName = /^[0-9a-zA-Z\.\s-]{1,50}$/i;
            var rm_GroupMaintenance_aspx_cs_GroupName = '<%= GetLocalResourceObject("rm_GroupMaintenance_aspx_cs_GroupName").ToString()%>';
            var rm_GroupMaintenance_aspx_cs_Description = '<%= GetLocalResourceObject("rm_GroupMaintenance_aspx_cs_Description").ToString()%>';
        </script>
        <script type="text/javascript" src="<%= ResolveUrl("~/") %>res/js/risk_MCF/rm_MCF_GroupMaintenance.js"></script>
    </tek:RadCodeBlock>
</asp:Content>

