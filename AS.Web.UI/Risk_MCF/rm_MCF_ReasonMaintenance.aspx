<%@ Page Title="Escalation Reason Maintenance" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="rm_MCF_ReasonMaintenance.aspx.cs" Inherits="rm_MCF_ReasonMaintenance" meta:resourcekey="PageResource1" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="Server">
    <as:PlaceHolder ID="ghg" runat="server">
        <div class="row">
            <div class="col-md-12 no-margin-bottom">
                <uc:PageTitle ID="uxReportTitle" runat="server" ReportTitle="Escalation Reason Maintenance" meta:resourcekey="uxReportTitleResource1"
                    HasFilteringOption="false" />
            </div>
        </div>
        <div class="height-18"></div>
        <as:Panel ID="uxFilterStatusContainer" runat="server" meta:resourcekey="uxFilterStatusContainerResource1">
            <div class="row">
                <div class="col-md-12 dark-blue">
                    <div class="control-inline">
                        <label class="first">
                            <asp:Literal ID="LiteralStatus" runat="server" meta:resourcekey="LiteralStatusResource1"> Status:</asp:Literal>
                        </label>
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
        </as:Panel>
        <%-- Create new Reason --%>
        <div class="height-6"></div>
        <div class="row">
            <div class="col-md-12 no-margin-action-container">
                <as:LinkButton runat="server" ID="uxCreateNew" OnClick="uxCreateNew_Click" CssClass="btn btn-default" meta:resourcekey="uxCreateNewResource1">Create New Reason</as:LinkButton>
            </div>
        </div>
        <div id="pnlAddEscalation" runat="server" visible="false">
            <div class="row">
                <div class="col-md-12">
                    <div class="risk-form-container text-center">
                        <div class="risk-form">
                            <div class="risk-form-row">
                                <as:ValidatorLabel ID="uxAddEscalationTextLabel" runat="server" ApplyFor="uxAddEscalationText" meta:resourcekey="uxAddEscalationTextLabelResource1" 
                                    Text="Reason:">
                                </as:ValidatorLabel>
                                <div class="control-inline">
                                    <as:TextBox Width="400px" CssClass="form-control" onkeypress="return DefaultEnterOnTextBoxEscalation(event);"
                                        MaxLength="100" ID="uxAddEscalationText" runat="server" ValidationGroup="Validate" meta:resourcekey="uxAddEscalationTextResource1"></as:TextBox>
                                    <div class="bottom-error text-left">
                                        <as:ValidatorMessage ID="uxAddEscalationTextErrMsg" runat="server" ApplyFor="uxAddEscalationText">
                                        </as:ValidatorMessage>
                                    </div>
                                </div>
                                <div class="risk-form-action-inline valign-top">
                                    <as:Button OnClientClick="return validBeforeSubmitAdd();" ID="uxAddEscalation" Text="Submit"
                                        OnClick="uxAddEscalation_Click" runat="server" CssClass="btn btn-default" meta:resourcekey="uxAddEscalationResource1"></as:Button>
                                    <as:Button ID="uxAddCancel" Text="Cancel" CausesValidation="false"
                                        OnClick="uxAddEscalationCancel_Click" runat="server" CssClass="btn btn-default" meta:resourcekey="uxAddCancelResource1"></as:Button>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="height-24"></div>
        <%-- end Create new Reason --%>
        <div id="uxDivPannel" runat="server" onkeypress="return DefaultEnterOnDiv(event);">
            <uc:UxExport ID="uxExportTop" runat="server" GridID="uxGrid" IsOnTop="true" meta:resourcekey="uxExportTopResource1"
                GridHeader="Risk Management - List Management - Escalation Reason Maintenance" ShowPDF="false"/>
            <as:ASGrid ID="uxGrid" runat="server" GridLines="None" AllowPaging="True"  InsertTempColumnAtTheEnd="false"
                AllowSorting="True" AutoGenerateColumns="False" ASPagingMethod="None" AllowAutomaticUpdates="True"
                OnItemCommand="uxGrid_ItemCommand" CssClass="in" meta:resourcekey="uxGridResource1" IsAutoExportTemplate="true">
                <MasterTableView DataKeyNames="EscalationReasonID" CommandItemDisplay="None" EditMode="InPlace">
                    <Columns>
                        <%--Reason Column--%>
                        <as:ASGridTemplateColumn HeaderText="Reason" HeaderTooltip="Reason" HeaderStyle-HorizontalAlign="Center" SortExpression="Reason"
                            UniqueName="Reason" DataField="Reason" ItemStyle-HorizontalAlign="Left" ItemStyle-CssClass="word-wrapped" meta:resourcekey="ASGridTemplateColumnResource1">
                            <ItemTemplate>
                                <asp:Literal ID="LiteralReason" runat="server"
                                    Text='<%# VeraCodeExtensions.DoVeraCode(System.Web.HttpUtility.HtmlEncode(Eval("Reason").ToString())) %>' />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:HiddenField runat="server" ID="txtEscalationReasonID" Value='<%# Eval("EscalationReasonID").ToString() %>' />
                                <asp:TextBox runat="server" ID="txtEditReasonName" MaxLength="100" Text='<%# Eval("Reason").ToString() %>'
                                    CssClass="txtEditReasonName max-width" meta:resourcekey="txtEditReasonNameResource1" />
                            </EditItemTemplate>

                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                            <ItemStyle HorizontalAlign="Left" CssClass="word-wrapped"></ItemStyle>
                        </as:ASGridTemplateColumn>

                        <as:ASGridBoundColumn HeaderText="Active" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                            DataField="IsActiveText" UniqueName="ActivateDeactivateHidden" Display="false" meta:resourcekey="ASGridBoundColumnResource1">
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

                        <%--Edit Column--%>
                        <as:ASGridTemplateColumn HeaderText="Edit" HeaderStyle-Width="170px" HeaderTooltip="Click to edit reason"
                            UniqueName="Edit" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" meta:resourcekey="ASGridTemplateColumnResource2">
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

        <as:Validator ID="pnlAddEscalationValidator" runat="server" ValidationFunction="doValidation" meta:resourcekey="pnlAddEscalationValidatorResource1">
            <Items>
                <as:BasicValidationItem ControlToValidateID="uxAddEscalationText" Rule="Required"
                    ResMessage="Resources.MessageManager.Field_RequireAndUnique" />
                <as:BasicValidationItem ControlToValidateID="uxAddEscalationText" Rule="Maxlength"
                    MaxLength="100" ResMessage="Resources.MessageManager.Field_NotExcessed" ResParams="100" />
                <as:BasicValidationItem ControlToValidateID="uxAddEscalationText"
                    Rule="StringUnaccept" Pattern="<>" ResMessage="Resources.ValMsg.InvalidCharacter" />
                <as:RegExValidationItem ControlToValidateID="uxAddEscalationText" 
                    RegularExpression="^((?!(<[^ \t]))(?!(&#)).)*$" ResMessage="Resources.ValMsg.InvalidCharacter" />
            </Items>
        </as:Validator>
    </as:PlaceHolder>
    <tek:RadCodeBlock runat="server" ID="RadCodeBlock">
        <script type="text/javascript">
            var uxAddEscalationText_ClientID = "<%=uxAddEscalationText.ClientID%>";
            var rm_ReasonMaintenance_uxActivateDeactivateData = "<%=uxActivateDeactivateData.ClientID%>";
            var rm_ReasonMaintenance_uxActivateDeactivate = "<%=uxActivateDeactivate.ClientID%>";
            var rm_ReasonMaintenance_uxAddEscalation = "<%=uxAddEscalation.ClientID%>";
            var rm_ReasonMaintenance_uxChangeFilterStatus = "<%=uxChangeFilterStatus.ClientID%>";
            var Resources_ValMsg_Required = '<%=  Resources.ValMsg.Required %>';
            var Resources_ValMsg_MaxLength = '<%= Resources.ValMsg.MaxLength %>';
            var Resources_ValMsg_InvalidCharacter = '<%= Resources.ValMsg.InvalidCharacter %>';
            var rm_ReasonMaintenance_js_Reason = '<%=GetLocalResourceObject("rm_ReasonMaintenance_js_Reason").ToString()%>';
        </script>
        <script src="<%=ResolveUrl("~")%>res/js/risk_MCF/rm_MCF_ReasonMaintenance.js"></script>
    </tek:RadCodeBlock>
</asp:Content>
