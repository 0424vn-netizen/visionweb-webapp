<%@ Page Title="Escalation Resolution Maintenance" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="rm_MCF_ResolutionMaintenance.aspx.cs" Inherits="rm_MCF_ResolutionMaintenance" meta:resourcekey="PageResource1" %>

<%@ Register Src="~/UserControls/PageTitle.ascx" TagName="ReportTitle" TagPrefix="uc" %>
<%@ Register TagName="UxExport" Src="~/UserControls/UxExport.ascx" TagPrefix="uc" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="Server">

    <div class="row">
        <div class="col-md-12 no-margin-bottom">
            <uc:ReportTitle ID="uxReportTitle" HasFilteringOption="false" runat="server" ReportTitle="Resolution Maintenance" meta:resourcekey="uxReportTitleResource1" />
        </div>
    </div>
    <div class="height-18"></div>
    <as:Panel ID="uxFilterStatusContainer" runat="server" meta:resourcekey="uxFilterStatusContainerResource1">
        <div class="row">
            <div class="col-md-12 dark-blue">
                <div class="control-inline">
                    <label class="first">
                        <asp:Literal ID="LiteralStatus" runat="server" meta:resourcekey="LiteralStatusResource1"> Status:</asp:Literal></label>
                </div>
                <as:RadioButton ID="uxFilterStatusAll" CssClass="control-inline" runat="server" Text="All"
                    GroupName="uxFilterStatus"
                    xValue="-1" onclick="uxFilterStatus_Checked()" meta:resourcekey="uxFilterStatusAllResource1" />
                <as:RadioButton ID="uxFilterStatusActive" CssClass="control-inline" runat="server"
                    Text="Active" GroupName="uxFilterStatus"
                    xValue="1" Checked="true" onclick="uxFilterStatus_Checked()" meta:resourcekey="uxFilterStatusActiveResource1" />
                <as:RadioButton ID="uxFilterStatusInactive" CssClass="control-inline" runat="server"
                    Text="Inactive" GroupName="uxFilterStatus"
                    xValue="0" onclick="uxFilterStatus_Checked()" meta:resourcekey="uxFilterStatusInactiveResource1" />
                <as:Button ID="uxChangeFilterStatus" runat="server" IsStandardButton="true" OnClick="uxChangeFilterStatus_Click"
                    Style="display: none" meta:resourcekey="uxChangeFilterStatusResource1"></as:Button>
            </div>
        </div>
    </as:Panel>
    <as:HiddenField ID="hddCheckBoxResolution" runat="Server" />

    <%-- Create new Resolution --%>
    <div class="height-6"></div>
    <div class="row">
        <div class="col-md-12 no-margin-action-container">
            <as:LinkButton runat="server" ID="uxCreateNew" CssClass="btn btn-default" OnClick="uxCreateNew_Click" meta:resourcekey="uxCreateNewResource1">Create New Resolution</as:LinkButton>
        </div>
    </div>
    <div visible="false" id="pnlAddResolution" runat="server">
        <div class="row">
            <div class="col-md-12">
                <div class="risk-form-container">
                    <div class="risk-form text-center">
                        <div class="risk-form-row">
                            <as:ValidatorLabel ID="uxAddResolutionTextLabel" runat="server" Text="Status:" meta:resourcekey="uxAddResolutionTextLabelResource1" ApplyFor="uxAddResolutionText" CssClass="control-label" />
                            <div class="control-inline">
                                <as:TextBox Width="400px" CssClass="form-control"
                                    MaxLength="100" ID="uxAddResolutionText" runat="server" ValidationGroup="Validate" HintCss="hint" meta:resourcekey="uxAddResolutionTextResource1" />
                                <div class="bottom-error text-left">
                                    <as:ValidatorMessage ID="uxAddResolutionTextErrMsg" runat="server" ApplyFor="uxAddResolutionText" Message="" ShowOnLoad="False">
                                    </as:ValidatorMessage>
                                </div>
                            </div>
                            <div class="risk-form-action-inline valign-top">
                                <as:Button OnClientClick="return validBeforeSubmitAdd();" CssClass="btn btn-default" ID="uxAddResolution"
                                    Text="Submit" OnClick="uxAddResolution_Click" runat="server" IsStandardButton="False" meta:resourcekey="uxAddResolutionResource1"></as:Button>
                                <as:Button ID="uxAddCancel" Text="Cancel" CssClass="btn btn-default" CausesValidation="False"
                                    OnClick="uxAddResolutionCancel_Click" runat="server" IsStandardButton="False" meta:resourcekey="uxAddCancelResource1"></as:Button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="height-24"></div>
    <%-- end Create new Resolution --%>

    <div id="uxDivPannel" runat="server" onkeypress="return DefaultEnterOnDiv(event);">
        <uc:UxExport ID="uxExportTop" IsOnTop="true" runat="server" GridID="uxGrid" meta:resourcekey="uxExportTopResource1" FileName="Risk Management - List Management - Resolution Maintenance" ShowPDF="false"/>
        <as:ASGrid ID="uxGrid" runat="server" GridLines="None" Width="100%" AllowPaging="True" IsAutoExportTemplate="true"
            AllowSorting="True" AutoGenerateColumns="False" AllowAutomaticUpdates="True" InsertTempColumnAtTheEnd="false" 
            OnItemCommand="uxGrid_ItemCommand" GridName="Risk Management - List Management - Resolution Maintenance" CssClass="in" meta:resourcekey="uxGridResource1">
            <MasterTableView DataKeyNames="ResolutionID" CommandItemDisplay="None" EditMode="InPlace">
                <Columns>
                    <as:ASGridTemplateColumn HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left" SortExpression="Resolution"
                        DataField="Resolution" HeaderText="Resolution" UniqueName="Resolution" HeaderTooltip="Resolution" meta:resourcekey="ASGridTemplateColumnResource1">
                        <ItemTemplate>
                            <asp:Literal Text='<%# VeraCodeExtensions.DoVeraCode(System.Web.HttpUtility.HtmlEncode(Eval("Resolution").ToString())) %>'
                                ID="LiteralResolution" runat="server" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:HiddenField runat="server" ID="txtResolutionID" Value='<%# Eval("ResolutionID").ToString() %>' />
                            <asp:HiddenField runat="server" ID="txtActiveValue" Value='<%# Eval("IsActive").ToString() %>' />
                            <asp:TextBox ID="txtEditResolution" MaxLength="100" runat="server" Text='<%# Eval("Resolution").ToString() %>'
                                CssClass="txtEditResolution max-width" meta:resourcekey="txtEditResolutionResource1" />
                        </EditItemTemplate>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
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
                    <as:ASGridTemplateColumn HeaderText="Edit" HeaderStyle-Width="170px" HeaderTooltip="Click to edit resolution"
                        HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="Edit" meta:resourcekey="ASGridTemplateColumnResource2">
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
        <as:Button ID="uxActivateDeactivate" IsStandardButton="True" runat="server" OnClick="uxActivateDeactivate_Click"
            CssClass="display-none" meta:resourcekey="uxActivateDeactivateResource1" />
    </div>
    <as:HiddenField ID="uxActivateDeactivateData" runat="server" />

    <as:Validator ID="Validator1" runat="server" ValidationFunction="doValidation" MessageType="Inline" MessageContainerClientID="" meta:resourcekey="Validator1Resource1">
        <Items>
            <as:BasicValidationItem ControlToValidateID="uxAddResolutionText" Rule="Required"
                ResMessage="Resources.MessageManager.Field_RequireAndUnique" />
            <as:BasicValidationItem ControlToValidateID="uxAddResolutionText" Rule="Maxlength"
                MaxLength="100" ResMessage="Resources.ValMsg.MaxLength" ResParams="100" />
            <as:BasicValidationItem ControlToValidateID="uxAddResolutionText"
                Rule="StringUnaccept" Pattern="<>" ResMessage="Resources.ValMsg.InvalidCharacter" />
            <as:RegExValidationItem ControlToValidateID="uxAddResolutionText" 
                RegularExpression="^((?!(<[^ \t]))(?!(&#)).)*$" ResMessage="Resources.ValMsg.InvalidCharacter" />
        </Items>
    </as:Validator>

    <tek:RadCodeBlock runat="server" ID="RadCodeBlock">

        <script type="text/javascript">
            var uxChangeFilterStatus_ClientID = '<%=uxChangeFilterStatus.ClientID %>';
            var uxAddResolutionText_ClientID = '<%=uxAddResolutionText.ClientID%>';
            var uxAddResolution_ClientID = '<%=uxAddResolution.ClientID%>';
            var uxActivateDeactivateData_ClientID = '<%=uxActivateDeactivateData.ClientID%>';
            var uxActivateDeactivate_ClientID = '<%=uxActivateDeactivate.ClientID%>';
            var Resources_ValMsg_Required = '<%=  Resources.ValMsg.Required %>';
            var Resources_ValMsg_InvalidCharacter = '<%= Resources.ValMsg.InvalidCharacter %>';
            var rm_ResolutionMaintenance_js_Resolution = '<%=GetLocalResourceObject("rm_ResolutionMaintenance_js_Resolution").ToString()%>';
        </script>
        <script type="text/javascript" src="<%= ResolveUrl("~/") %>res/js/risk_MCF/rm_MCF_ResolutionMaintenance.js"></script>
    </tek:RadCodeBlock>
</asp:Content>

