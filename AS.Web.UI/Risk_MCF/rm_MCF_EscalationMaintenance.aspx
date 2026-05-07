<%@ Page Title="Escalation Status Maintenance" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="rm_MCF_EscalationMaintenance.aspx.cs" Inherits="rm_MCF_EscalationMaintenance" meta:resourcekey="PageResource1" %>

<%@ Register Src="~/UserControls/PageTitle.ascx" TagName="ReportTitle" TagPrefix="uc" %>
<%@ Register TagName="UxExport" Src="~/UserControls/UxExport.ascx" TagPrefix="uc" %>
<asp:Content ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="Server">
    <as:PlaceHolder ID="sfdf" runat="server">
        <div class="row">
            <div class="col-md-12 no-margin-bottom">
                <uc:ReportTitle ID="uxReportTitle" runat="server" HasFilteringOption="false" meta:resourcekey="uxReportTitleResource1" ReportTitle="Escalation Status Maintenance" />
            </div>
        </div>
        <div class="height-18"></div>

        <as:Panel ID="uxFilterStatusContainer" runat="server" meta:resourcekey="uxFilterStatusContainerResource1">
            <div class="row">
                <div class="col-md-12 dark-blue">
                    <div class="control-inline">
                        <label class="first">
                            <asp:Literal ID="rm_EscalationMaintenance_aspx_Status" runat="server" meta:resourcekey="rm_EscalationMaintenance_aspx_StatusResource1"> Status:</asp:Literal></label>
                    </div>
                    <as:RadioButton ID="uxFilterStatusAll" runat="server" CssClass="control-inline" Text="All"
                        GroupName="uxFilterStatus"
                        xValue="-1" onclick="uxFilterStatus_Checked()" meta:resourcekey="uxFilterStatusAllResource1" />
                    <as:RadioButton ID="uxFilterStatusActive" CssClass="control-inline" runat="server"
                        Text="Active" GroupName="uxFilterStatus"
                        xValue="1" Checked="true" onclick="uxFilterStatus_Checked()" meta:resourcekey="uxFilterStatusActiveResource1" />
                    <as:RadioButton ID="uxFilterStatusInactive" CssClass="control-inline" runat="server"
                        Text="Inactive" GroupName="uxFilterStatus"
                        xValue="0" onclick="uxFilterStatus_Checked()" meta:resourcekey="uxFilterStatusInactiveResource1" />
                    <asp:Button ID="uxChangeFilterStatus" runat="server" OnClick="uxChangeFilterStatus_Click"
                        Style="display: none" meta:resourcekey="uxChangeFilterStatusResource1"></asp:Button>
                </div>
            </div>
        </as:Panel>

        <%-- Create new Status --%>
        <div class="height-6"></div>
        <div class="row">
            <div class="col-md-12 no-margin-action-container">
                <as:LinkButton CssClass="btn btn-default" runat="server" ID="uxCreateNew" OnClick="uxCreateNew_Click" meta:resourcekey="uxCreateNewResource1">Create New Status</as:LinkButton>
            </div>
        </div>

        <div visible="false" id="pnlAddEscalation" runat="server">
            <div class="row">
                <div class="col-md-12">
                    <div class="risk-form-container">
                        <div class="risk-form text-center">
                            <div class="risk-form-row">
                                <as:ValidatorLabel ID="uxAddEscalationLabel" ApplyFor="uxAddEscalationText" runat="server"
                                    meta:resourcekey="uxAddEscalationLabelResource1" Text="Status:" />
                                <div class="control-inline">
                                    <asp:TextBox Width="400px" CssClass="form-control"
                                        MaxLength="100" ID="uxAddEscalationText" runat="server" ValidationGroup="Validate" meta:resourcekey="uxAddEscalationTextResource1"></asp:TextBox>
                                    <div class="bottom-error text-left">
                                        <as:ValidatorMessage runat="server" ID="uxAddEscalationTextMsg" ApplyFor="uxAddEscalationText" />
                                    </div>
                                </div>
                                <div class="risk-form-action-inline valign-top">
                                    <asp:Button OnClientClick="return validBeforeSubmitAdd();" CssClass="btn btn-default"
                                        ID="uxAddEscalation" Text="Submit" OnClick="uxAddEscalation_Click" runat="server" meta:resourcekey="uxAddEscalationResource1"></asp:Button>
                                    <asp:Button CssClass="btn btn-default" ID="uxAddCancel" Text="Cancel" CausesValidation="false"
                                        OnClick="uxAddEscalationCancel_Click" runat="server" meta:resourcekey="uxAddCancelResource1" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="height-24"></div>
        <%-- end Create new Status --%>

        <div id="uxDivPannel" runat="server" onkeypress="return DefaultEnterOnDiv(event);">
            <uc:UxExport ID="uxExportTop" runat="server" IsOnTop="true" GridID="uxGrid" meta:resourcekey="uxExportTopResource1"
                GridHeader="Risk Management - List Management - Escalation Status Maintenance" ShowPDF="false" />
            <as:ASGrid ID="uxGrid" runat="server" GridLines="None" AllowPaging="True" AllowSorting="True"
                AutoGenerateColumns="False" ASPagingMethod="None" AllowAutomaticUpdates="True"
                OnItemCommand="uxGrid_ItemCommand" IsAutoExportTemplate="true"
                OnNextClick="uxGrid_PagerEventClick"
                OnPrevClick="uxGrid_PagerEventClick"
                OnGoClick="uxGrid_PagerEventClick"  InsertTempColumnAtTheEnd="false"
                GridName="Risk Management - List Management - Escalation Status Maintenance" CssClass="in" meta:resourcekey="uxGridResource1">
                <MasterTableView DataKeyNames="EscalationStatusID" CommandItemDisplay="None" EditMode="InPlace">
                    <Columns>
                        <%--Status Column--%>
                        <as:ASGridTemplateColumn HeaderText="Status" HeaderTooltip="Status" HeaderStyle-HorizontalAlign="Center" SortExpression="Status"
                            UniqueName="Status" DataField="Status" ItemStyle-HorizontalAlign="Left" ItemStyle-CssClass="word-wrapped" meta:resourcekey="ASGridTemplateColumnResource1">
                            <ItemTemplate>
                                <asp:Literal ID="LiteralStatus" runat="server"
                                    Text='<%# VeraCodeExtensions.DoVeraCode(System.Web.HttpUtility.HtmlEncode(Eval("Status").ToString())) %>' />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:HiddenField runat="server" ID="txtEscalationStatusID" Value='<%# Eval("EscalationStatusID").ToString() %>' />
                                <asp:HiddenField runat="server" ID="txtActiveValue" Value='<%# Eval("IsActive").ToString() %>' />
                                <asp:TextBox runat="server" ID="txtEditStatusName" MaxLength="100" Text='<%# Eval("Status").ToString() %>'
                                    CssClass="txtEditStatusName max-width" meta:resourcekey="txtEditStatusNameResource1" />
                            </EditItemTemplate>

                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                            <ItemStyle HorizontalAlign="Left" CssClass="word-wrapped"></ItemStyle>
                        </as:ASGridTemplateColumn>

                        <%--Active Column--%>
                        <as:ASGridBoundColumn HeaderText="Active" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                            DataField="IsActiveText" UniqueName="ActivateDeactivateHidden" Display="false" meta:resourcekey="ASGridBoundColumnResource1">

                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                        </as:ASGridBoundColumn>
                        <tek:GridHyperLinkColumn HeaderText="Activate/Deactivate" HeaderTooltip="Click to activate/deactivate status"
                            HeaderStyle-Width="120px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                            UniqueName="ActivateDeactivate" meta:resourcekey="GridHyperLinkColumnResource1">
                            <HeaderStyle HorizontalAlign="Center" Width="120px"></HeaderStyle>

                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                        </tek:GridHyperLinkColumn>

                        <%--Edit Column--%>
                        <as:ASGridTemplateColumn HeaderText="Edit" HeaderStyle-Width="170px" HeaderTooltip="Click to edit status"
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

        <as:Validator ID="Validator1" runat="server" ValidationFunction="doValidation" MessageType="Inline" meta:resourcekey="Validator1Resource1">
            <Items>
                <as:BasicValidationItem ControlToValidateID="uxAddEscalationText" Rule="Required"
                    ResMessage="Resources.MessageManager.Field_RequireAndUnique" />
                <as:BasicValidationItem ControlToValidateID="uxAddEscalationText" Rule="Maxlength"
                    MaxLength="100" ResMessage="Resources.ValMsg.MaxLength" ResParams="100" />
                <as:BasicValidationItem ControlToValidateID="uxAddEscalationText"
                    Rule="StringUnaccept" Pattern="<>" ResMessage="Resources.ValMsg.InvalidCharacter" />
                <as:RegExValidationItem ControlToValidateID="uxAddEscalationText" 
                    RegularExpression="^((?!(<[^ \t]))(?!(&#)).)*$" ResMessage="Resources.ValMsg.InvalidCharacter" />
            </Items>
        </as:Validator>
    </as:PlaceHolder>
    <asp:HiddenField ID="uxActivateDeactivateData" runat="server" />
    <tek:RadCodeBlock runat="server" ID="RadCodeBlock">
        <script type="text/javascript">
            var uxChangeFilterStatus_ClientID = '<%=uxChangeFilterStatus.ClientID %>';
            var uxAddEscalationText_ClientID = '<%=uxAddEscalationText.ClientID%>';
            var uxAddEscalation_ClientID = '<%=uxAddEscalation.ClientID%>';
            var uxActivateDeactivateData_ClientID = '<%=uxActivateDeactivateData.ClientID%>';
            var uxActivateDeactivate_ClientID = '<%=uxActivateDeactivate.ClientID%>';

            var Resources_ValMsg_Required = '<%=  Resources.ValMsg.Required %>';
            var Resources_ValMsg_InvalidCharacter = '<%= Resources.ValMsg.InvalidCharacter %>';
            var rm_EscalationMaintenance_js_Escalation='<%= GetLocalResourceObject("rm_EscalationMaintenance_js_Escalation").ToString()%>'
        </script>
        <script type="text/javascript" src="<%= ResolveUrl("~/") %>res/js/risk_MCF/rm_MCF_EscalationMaintenance.js"></script>
    </tek:RadCodeBlock>
</asp:Content>

