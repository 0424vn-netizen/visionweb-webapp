<%@ Page Title="Merchant Profile Maintenance" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="rm_MCF_ProfileMaintenance.aspx.cs" Inherits="rm_MCF_ProfileMaintenance" meta:resourcekey="PageResource1" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="Server">
    <as:PlaceHolder ID="dfd" runat="server">
        <div class="row">
            <div class="col-md-12 no-margin-bottom">
                <uc:PageTitle ID="uxReportTitle" ReportTitle="Merchant Profile Maintenance" HasFilteringOption="false" meta:resourcekey="uxReportTitleResource1"
                    runat="server" />
            </div>
        </div>
        <div class="height-18"></div>
        <div id="uxFilterStatusContainer" runat="server">
            <div class="row">
                <div class="col-md-12 dark-blue">
                    <div class="control-inline">
                        <label class="first">
                            <asp:Literal ID="LiteralStatus" runat="server" meta:resourcekey="LiteralStatusResource1"> Status:</asp:Literal></label>
                    </div>
                    <div class="control-inline">
                        <as:RadioButton ID="uxFilterStatusAll" runat="server" Text="All" GroupName="uxFilterStatus"
                            xValue="-1" onclick="uxFilterStatus_Checked()" meta:resourcekey="uxFilterStatusAllResource1" />
                    </div>
                    <div class="control-inline">
                        <as:RadioButton ID="uxFilterStatusActive" runat="server" Text="Active" GroupName="uxFilterStatus"
                            xValue="1" Checked="true" onclick="uxFilterStatus_Checked()" meta:resourcekey="uxFilterStatusActiveResource1" />
                    </div>
                    <div class="control-inline">
                        <as:RadioButton ID="uxFilterStatusInactive" runat="server" Text="Inactive" GroupName="uxFilterStatus"
                            xValue="0" onclick="uxFilterStatus_Checked()" meta:resourcekey="uxFilterStatusInactiveResource1" />
                    </div>
                    <asp:Button ID="uxChangeFilterStatus" runat="server" OnClick="uxChangeFilterStatus_Click"
                        CssClass="display-none" meta:resourcekey="uxChangeFilterStatusResource1"></asp:Button>
                </div>
            </div>
        </div>
        <%-- Create new Reason --%>
        <div class="height-6"></div>
        <div class="row">
            <div class="col-md-12 no-margin-action-container">
                <as:LinkButton runat="server" ID="uxCreateMode" CssClass="btn btn-default" OnClick="uxCreateMode_Click" meta:resourcekey="uxCreateModeResource1">Create New Profile</as:LinkButton>
            </div>
        </div>
        <div id="pnlAddGroup" runat="server" visible="false">
            <div class="row">
                <div class="col-md-12">
                    <div class="risk-form-container">
                        <div class="risk-form text-right">
                            <div class="risk-form-row">
                                <as:ValidatorLabel ID="uxAddGroupTextLabel" ApplyFor="uxAddGroupText" runat="server"
                                    Text="Profile:" meta:resourcekey="uxAddGroupTextLabelResource1" />
                                <div class="control-inline">
                                    <as:TextBox Width="300px" onkeypress="return DefaultEnterOnTextBox(event);" MaxLength="50"
                                        ID="uxAddGroupText" CssClass="form-control" runat="server" meta:resourcekey="uxAddGroupTextResource1">
                                    </as:TextBox>
                                    <div class="label-error-left" style="width: 300px">
                                        <as:ValidatorMessage runat="server" ID="uxAddGroupTextErrMsg" ApplyFor="uxAddGroupText" />
                                    </div>
                                </div>
                            </div>
                            <div class="risk-form-row">
                                <span class="valign-top">
                                    <as:ValidatorLabel ID="uxAddDescriptionLabel" ApplyFor="uxAddDescription" runat="server"
                                        Text="Description:" meta:resourcekey="uxAddDescriptionLabelResource1" />
                                </span>
                                <div class="control-inline">
                                    <as:TextBox Width="300px" CssClass="form-control" TextMode="MultiLine" Rows="6" ID="uxAddDescription"
                                        runat="server"
                                        MaxLength="500" onKeyUp="return checkMaxLengthInput();" meta:resourcekey="uxAddDescriptionResource1">
                                    </as:TextBox>
                                    <div class="label-error-left" style="width: 300px">
                                        <as:ValidatorMessage runat="server" ID="uxAddDescriptionErrMsg" ApplyFor="uxAddDescription" />
                                    </div>
                                </div>
                            </div>
                            <as:PlaceHolder ID="uxPlAddShowIEReport" runat="server" Visible="false">
                                <div class="risk-form-row">
                                    <span class="valign-top">&nbsp;
                                    </span>
                                    <div class="control-inline">
                                        <div style="width: 300px;" class="text-left">
                                            <as:CheckBox ID="uxAddSendToIEReport" runat="server" Text="Send to I/E Report" meta:resourcekey="uxAddSendToIEReportResource1" />
                                        </div>
                                    </div>
                                </div>
                            </as:PlaceHolder>
                            <div class="risk-form-action">
                                <as:Button OnClientClick="return validBeforeSubmitAdd();" ID="uxAddGroup" CssClass="btn btn-default"
                                    Text="Submit" OnClick="uxAddGroup_Click" runat="server" meta:resourcekey="uxAddGroupResource1"></as:Button>
                                <as:Button ID="uxAddCancel" Text="Cancel" OnClick="uxAddGroupCancel_Click" CssClass="btn btn-default"
                                    runat="server" meta:resourcekey="uxAddCancelResource1"></as:Button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="height-24"></div>
        <%-- end Create new Reason --%>
        <div id="uxDivPannel" runat="server" onkeypress="return DefaultEnterOnDiv(event);">
            <uc:UxExport ID="uxExportTop" runat="server" GridID="uxGroupList" IsOnTop="true" ShowPDF="false"
                GridHeader="Risk Management - List Management - Merchant Profile Maintenance"  meta:resourcekey="uxExportTopResource1"/>
            <as:ASGrid ID="uxGroupList" runat="server" GridLines="None" AllowPaging="True" AllowAutomaticUpdates="True" IsAutoExportTemplate="true"
                AllowSorting="True" AutoGenerateColumns="False" AllowFilteringByColumn="false"  InsertTempColumnAtTheEnd="false"
                OnItemCommand="uxGroupList_ItemCommand" AllowSortFilterWhenExport="true" CssClass="in" meta:resourcekey="uxGroupListResource1">
                <MasterTableView DataKeyNames="RecordID" CommandItemDisplay="None" EditMode="InPlace">
                    <Columns>
                        <%--Profile column --%>
                        <as:ASGridTemplateColumn HeaderText="Profile" HeaderTooltip="Profile" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Left" UniqueName="ProfileName" AllowEncodeWhenExport="true" SortExpression="ProfileName"
                            DataField="ProfileName" meta:resourcekey="ASGridTemplateColumnResource1">
                            <ItemTemplate>
                                <asp:Literal ID="LiteralProfileName" Text='<%# VeraCodeExtensions.DoVeraCode(System.Web.HttpUtility.HtmlEncode(Eval("ProfileName").ToString())) %>'
                                    runat="server" />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:HiddenField ID="txtRecordID" runat="server" Value='<%# Eval("RecordID").ToString() %>' />
                                <asp:TextBox ID="txtEditProfileName" runat="server" MaxLength="50" Text='<%# Eval("ProfileName").ToString() %>'
                                    CssClass="txtEditProfileName max-width" meta:resourcekey="txtEditProfileNameResource1" />
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
                                <asp:TextBox ID="txtEditProfileDescription" runat="server" MaxLength="500" Text='<%# Eval("Description").ToString() %>'
                                    CssClass="txtEditProfileDescription max-width" meta:resourcekey="txtEditProfileDescriptionResource1" />
                            </EditItemTemplate>

                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                        </as:ASGridTemplateColumn>
                        <%-- I/E Report column --%>
                        <as:ASGridTemplateColumn HeaderText="I/E Report" HeaderTooltip="I/E Report" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" UniqueName="IEReport" SortExpression="IsSendToIEReport"
                            DataField="IsSendToIEReport" meta:resourcekey="ASGridTemplateColumnResource3">
                            <ItemTemplate>
                                <asp:Literal ID="LiteralIEReport" Text='<%# Eval( "IsSendToIEReport" ).ToString()== "True" ? GetLocalResourceObject("rm_ProfileMaintenance_aspx_cs_Yes").ToString() : GetLocalResourceObject("rm_ProfileMaintenance_aspx_cs_No").ToString() %>'
                                    runat="server" />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <as:CheckBox ID="uxSendToIEReport" Checked='<%# Eval( "IsSendToIEReport" ).ToString()== "True" ? true : false %>' runat="server" meta:resourcekey="uxSendToIEReportResource1" />
                            </EditItemTemplate>

                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                        </as:ASGridTemplateColumn>
                        <%--Active column--%>
                        <as:ASGridBoundColumn HeaderText="Active" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" DataField="IsActiveText" UniqueName="ActivateDeactivateHidden"
                            Display="false" meta:resourcekey="ASGridBoundColumnResource1">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text=""></ModelErrorMessage>
                            </ColumnValidationSettings>

                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                        </as:ASGridBoundColumn>
                        <tek:GridHyperLinkColumn HeaderText="Activate/Deactivate" HeaderTooltip="Click to activate/deactivate status"
                            HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="ActivateDeactivate" meta:resourcekey="GridHyperLinkColumnResource1">
                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                        </tek:GridHyperLinkColumn>
                        <as:ASGridTemplateColumn HeaderText="Edit" HeaderStyle-Width="170px" HeaderTooltip="Click to edit profile"
                            HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="Edit" meta:resourcekey="ASGridTemplateColumnResource4">
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
        <asp:Button ID="uxActivateDeactivate" runat="server" OnClick="uxActivateDeactivate_Click"
            CssClass="display-none" meta:resourcekey="uxActivateDeactivateResource1" />
        <asp:HiddenField ID="uxActivateDeactivateData" runat="server" />
        <as:Validator ID="pnlAddGroupValidator" runat="server" ValidationFunction="doValidation" meta:resourcekey="pnlAddGroupValidatorResource1">
            <Items>
                <as:BasicValidationItem ControlToValidateID="uxAddGroupText" Rule="Required" ResMessage="Resources.MessageManager.Field_RequireAndUnique" />
                <as:BasicValidationItem ControlToValidateID="uxAddGroupText" Rule="Maxlength" MaxLength="50"
                    ResMessage="Resources.MessageManager.Generic_FieldLengthExceeded" ResParams="50" />
                <as:BasicValidationItem ControlToValidateID="uxAddDescription" Rule="Maxlength" MaxLength="500"
                    ResMessage="Resources.MessageManager.Generic_FieldLengthExceeded" ResParams="500" />
                <as:BasicValidationItem ControlToValidateID="uxAddGroupText"
                    Rule="StringUnaccept" Pattern="<>" ResMessage="Resources.ValMsg.InvalidCharacter" />
                <as:BasicValidationItem ControlToValidateID="uxAddDescription"
                    Rule="StringUnaccept" Pattern="<>" ResMessage="Resources.ValMsg.InvalidCharacter" />
            </Items>
        </as:Validator>
        <as:Validator ID="Validator1" runat="server" ValidationFunction="checkInputLength" meta:resourcekey="Validator1Resource1">
            <Items>
                <as:BasicValidationItem ControlToValidateID="uxAddDescription" Rule="Maxlength" MaxLength="500"
                    ResMessage="Resources.MessageManager.Generic_FieldLengthExceeded" ResParams="500" />
            </Items>
        </as:Validator>
        <as:Validator ID="Validator2" runat="server" ValidationFunction="ValidateAddGroupText" MessageType="Inline" meta:resourcekey="Validator1Resource1">
            <Items>
                <as:RegExValidationItem ControlToValidateID="uxAddGroupText" RegularExpression="^[0-9a-zA-Z\.\\s-]{1,50}$" ResMessage="Resources.ValMsg.InvalidCharacter" />
            </Items>
        </as:Validator>
        <as:Validator ID="Validator3" runat="server" ValidationFunction="ValidateAddDescription" MessageType="Inline" meta:resourcekey="Validator1Resource1">
            <Items>
                <as:RegExValidationItem ControlToValidateID="uxAddDescription" RegularExpression="^((?!(<[^ \t]))(?!(&#)).)*$" ResMessage="Resources.ValMsg.InvalidCharacter" />
            </Items>
        </as:Validator>
    </as:PlaceHolder>
    <tek:RadCodeBlock runat="server" ID="RadCodeBlock">
        <script type="text/javascript">
            var uxAddGroupText_ClientID = '<%=uxAddGroupText.ClientID%>';
            var uxAddDescription_ClientID = '<%=uxAddDescription.ClientID%>';
            var rm_ProfileMaintenance_uxAddGroup = "<%=uxAddGroup.ClientID%>";
            var rm_ProfileMaintenance_uxChangeFilterStatus = "<%=uxChangeFilterStatus.ClientID%>";
            var rm_ProfileMaintenance_uxActivateDeactivateData = "<%=uxActivateDeactivateData.ClientID%>";
            var rm_ProfileMaintenance_uxActivateDeactivate = "<%=uxActivateDeactivate.ClientID%>";
            var Resources_ValMsg_Required = '<%=  Resources.ValMsg.Required %>';
            var Resources_ValMsg_InvalidCharacter = '<%= Resources.ValMsg.InvalidCharacter %>';
            var rm_ProfileMaintenance_js_String1 = '<%=GetLocalResourceObject("rm_ProfileMaintenance_js_String1").ToString()%>';
            var rm_ProfileMaintenance_js_String2 = '<%=GetLocalResourceObject("rm_ProfileMaintenance_js_String2").ToString()%>';
            var rm_ProfileMaintenance_js_String3 = '<%=GetLocalResourceObject("rm_ProfileMaintenance_js_String3").ToString()%>';
        </script>
        <script src="<%=ResolveUrl("~")%>res/js/risk_MCF/rm_MCF_ProfileMaintenance.js"></script>
    </tek:RadCodeBlock>
</asp:Content>


