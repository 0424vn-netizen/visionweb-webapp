<%@ Page Title="Risk Scores" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="rm_MCF_RiskScores.aspx.cs" Inherits="rm_MCF_RiskScores" meta:resourcekey="PageResource1" %>

<%@ Register Src="~/UserControls/rm_MCF_Score.ascx" TagName="RiskScore" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/rm_MCF_AttributeRiskScore.ascx" TagName="AttributeRiskScore" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/rm_MCF_EditRiskScore.ascx" TagName="EditRiskScore" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/rm_MCF_EditAttributeRiskScore.ascx" TagName="EditAttributeRiskScore" TagPrefix="uc" %>

<%@ Register Src="~/UserControls/PageTitle.ascx" TagName="ReportTitle" TagPrefix="uc" %>
<%@ Register TagName="UxExport" Src="~/UserControls/UxExport.ascx" TagPrefix="uc" %>
<%--<%@ Register TagName="SiteID_Selector" Src="~/UserControls/SiteID_Selector.ascx" TagPrefix="uc" %>--%>

<asp:content id="Content1" contentplaceholderid="ContentPage" runat="Server">

    <as:RadAjaxManagerProxy runat="server" ID="uxRadManager">
        <AjaxSettings>
            <tek:AjaxSetting AjaxControlID="uxAttributeGrid">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="uxAttributeGrid" />
               <%--      <tek:AjaxUpdatedControl ControlID="pnlAttributeRiskScore" />--%>
                </UpdatedControls>
            </tek:AjaxSetting>
        </AjaxSettings>
    </as:RadAjaxManagerProxy>


    <div class="row">
         <div class="col-md-12 no-margin-bottom no-toggle">
            <uc:ReportTitle ID="ReportTitle1" runat="server" ReportTitle="Risk Score Management" HasFilteringOption="false" meta:resourcekey="uxReportTitleResource1" />
    </div>
         </div>
    <div class="row">
        <div class="col-md-12">
            <h2 class="grid-title inline-block no-toggle" runat="server">
               <span> <asp:Literal runat="server" ID="Literal2" Text="Parameter Risk Score" meta:resourcekey="ResourceParameterRiskScore" /></span>
            </h2>
        </div>
    </div>
    <!--  -->
    <div class="row">
        <div class="col-xs-12">
            <div class="height-8"></div>
            <i class="text-muted">
                <as:Literal ID="Literal3" runat="server" Text="" meta:resourcekey="ltEnterIntergerRiskScoreResource"></as:Literal></i>
        </div>
    </div>

    <div id="ciParameterRiskScore" class="in">
        <!--Create new RiskScore -->
        <div class="row">
            <div class="col-md-12 no-margin-action-container" data-target=".create-rs-form" data-toggle="collapse">
                <as:LinkButton runat="server" ID="uxCreateMode" OnClientClick="return false;" CssClass="btn btn-default rm_riskscore_create_button" meta:resourcekey="uxCreateModeResource1">Create Parameter Risk Score</as:LinkButton>
            </div>
        </div>
        <div class="create-rs-form collapse">
            <uc:RiskScore ID="uxRiskScore" runat="server" OnAfterSubmit="uxRiskScore_AfterSubmit"
                OnBeforeSubmit="uxRiskScore_BeforeSubmit" />
        </div>
        <div class="height-24"></div>
        <!--end Create new RiskScore -->
        <div id="uxDivPannel" runat="server" onkeypress="return DefaultEnterOnDiv(event);">
            <uc:UxExport ID="uxExportTop" runat="server" GridID="uxRadGrid" ShowPDF="false"
                GridHeader="RISK MANAGEMENT - ASSIGNMENTS - RISK SCORES" IsOnTop="true" meta:resourcekey="uxExportTopResource1" />
            <as:ASGrid ID="uxRadGrid" runat="server" AllowPaging="True" GridLines="None"
                AllowSorting="True" AutoGenerateColumns="False" OnPreRender="uxRadGrid_PreRender"
                OnUpdateCommand="uxRadGrid_UpdateCommand" OnDeleteCommand="uxRadGrid_DeleteCommand"
                AllowAutomaticDeletes="True" AllowAutomaticUpdates="True" IsAutoExportTemplate="true"
                OnItemCommand="uxRadGrid_ItemCommand" CssClass="in" meta:resourcekey="uxRadGridResource1">
                <MasterTableView DataKeyNames="RecordID" GridLines="None" ShowFooter="false">
                    <Columns> 
                        <as:ASGridBoundColumn Visible="false" UniqueName="RecordID" HeaderText="RecordID"
                            DataField="RecordID" meta:resourcekey="ASGridBoundColumnResource1">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text=""></ModelErrorMessage>
                            </ColumnValidationSettings>

                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        </as:ASGridBoundColumn>
                        <as:ASGridBoundColumn Visible="false" UniqueName="ParameterKey" HeaderText="ParameterKey"
                            DataField="ParameterKey" meta:resourcekey="ASGridBoundColumnResource2">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text=""></ModelErrorMessage>
                            </ColumnValidationSettings>

                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        </as:ASGridBoundColumn>
                        <as:ASGridBoundColumn UniqueName="Group" HeaderText="Group" DataField="GroupDescription"
                            ASFormat="DynamicString" HeaderTooltip="Parameter Type" meta:resourcekey="ASGridBoundColumnResource3">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text=""></ModelErrorMessage>
                            </ColumnValidationSettings>

                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        </as:ASGridBoundColumn>
                        <as:ASGridBoundColumn UniqueName="ParameterName" HeaderText="Parameter" DataField="ParameterName"
                            ASFormat="DynamicString" HeaderTooltip="Risk Parameter" Visible="false" meta:resourcekey="ASGridBoundColumnResource4">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text=""></ModelErrorMessage>
                            </ColumnValidationSettings>

                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        </as:ASGridBoundColumn>
                        <as:ASGridBoundColumn UniqueName="ParameterDisplayName" HeaderText="Parameter"
                            DataField="ParameterDisplayName" ASFormat="DynamicString" HeaderTooltip="Risk Parameter" meta:resourcekey="ASGridBoundColumnResource5">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text=""></ModelErrorMessage>
                            </ColumnValidationSettings>

                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        </as:ASGridBoundColumn>

                          <as:ASGridBoundColumn UniqueName="ParameterDescription" HeaderText="Parameter Description" Display="false"
                            DataField="ParameterDescription" ASFormat="DynamicString" HeaderTooltip="Parameter Description" meta:resourcekey="ASGridBoundColumnResource14">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text=""></ModelErrorMessage>
                            </ColumnValidationSettings>

                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        </as:ASGridBoundColumn>
                        <as:ASGridBoundColumn UniqueName="ParameterIndicatorFrom" HeaderText="From (%/#/$)"
                            DataField="ParameterIndicatorFrom" ItemStyle-HorizontalAlign="Right"
                            HeaderTooltip="Minimum Range" meta:resourcekey="ASGridBoundColumnResource6">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text=""></ModelErrorMessage>
                            </ColumnValidationSettings>

                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                        </as:ASGridBoundColumn>
                        <as:ASGridBoundColumn UniqueName="ParameterIndicatorFromText" HeaderText="From (%/#/$)"
                            DataField="ParameterIndicatorFromText" ItemStyle-HorizontalAlign="Right"
                            Visible="false" meta:resourcekey="ASGridBoundColumnResource7">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text=""></ModelErrorMessage>
                            </ColumnValidationSettings>

                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                        </as:ASGridBoundColumn>
                        <as:ASGridBoundColumn UniqueName="ParameterIndicatorTo" HeaderText="To (%/#/$)"
                            DataField="ParameterIndicatorTo" ItemStyle-HorizontalAlign="Right"
                            HeaderTooltip="Maximum Range" meta:resourcekey="ASGridBoundColumnResource8">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text=""></ModelErrorMessage>
                            </ColumnValidationSettings>

                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                        </as:ASGridBoundColumn>
                        <as:ASGridBoundColumn UniqueName="ParameterIndicatorToText" HeaderText="To (%/#/$)"
                            DataField="ParameterIndicatorToText" ItemStyle-HorizontalAlign="Right"
                            Visible="false" meta:resourcekey="ASGridBoundColumnResource9">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text=""></ModelErrorMessage>
                            </ColumnValidationSettings>

                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                        </as:ASGridBoundColumn>
                        <as:ASGridBoundColumn UniqueName="ParameterThreshold" HeaderText="Threshold"
                            DataField="ParameterThreshold" ItemStyle-HorizontalAlign="Right"
                            HeaderTooltip="Minimum Dollar Amount" meta:resourcekey="ASGridBoundColumnResource10">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text=""></ModelErrorMessage>
                            </ColumnValidationSettings>

                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                        </as:ASGridBoundColumn>
                        <as:ASGridBoundColumn UniqueName="ParameterThreshold" HeaderText="Threshold"
                            DataField="ParameterThreshold" Visible="false" meta:resourcekey="ASGridBoundColumnResource11">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text=""></ModelErrorMessage>
                            </ColumnValidationSettings>

                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        </as:ASGridBoundColumn>
                        <as:ASGridBoundColumn UniqueName="ParameterThresholdText" HeaderText="Threshold"
                            DataField="ParameterThresholdText" ItemStyle-HorizontalAlign="Right"
                            Visible="false" meta:resourcekey="ASGridBoundColumnResource12">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text=""></ModelErrorMessage>
                            </ColumnValidationSettings>

                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                        </as:ASGridBoundColumn>
                        <as:ASGridBoundColumn UniqueName="ParameterScore" HeaderText="Score" DataField="ParameterScore"
                            ASFormat="Integer" HeaderTooltip="Risk Score" meta:resourcekey="ASGridBoundColumnResource13">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text=""></ModelErrorMessage>
                            </ColumnValidationSettings>

                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        </as:ASGridBoundColumn>

                        <tek:GridEditCommandColumn UniqueName="EditCommandColumn" HeaderText="" meta:resourcekey="GridEditCommandColumnResource1"
                            HeaderTooltip="" ItemStyle-HorizontalAlign="Center"
                            HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="45px">
                            <HeaderStyle HorizontalAlign="Center" Width="45px"></HeaderStyle>

                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                        </tek:GridEditCommandColumn>

                        <tek:GridButtonColumn UniqueName="DeleteColumn" Text="Delete" CommandName="Delete"
                            HeaderStyle-Width="60px"
                            ConfirmDialogType="Classic" ConfirmText='Are you sure you would like to delete this Parameter Risk Score record?'
                            ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderTooltip="Click to delete risk score" meta:resourcekey="GridButtonColumnResource1">
                            <HeaderStyle HorizontalAlign="Center" Width="60px"></HeaderStyle>

                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                        </tek:GridButtonColumn>
                        <as:ASGridBoundColumn UniqueName="ParameterPrecision" DataField="ParameterPrecision"
                            Visible="false" meta:resourcekey="ASGridBoundColumnResource14">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text=""></ModelErrorMessage>
                            </ColumnValidationSettings>

                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        </as:ASGridBoundColumn>
                    </Columns>
                    <EditFormSettings EditFormType="Template">
                        <EditColumn UniqueName="EditCommandColumn1" FilterControlAltText="Filter EditCommandColumn1 column"></EditColumn>
                        <FormTemplate>
                            <uc:EditRiskScore ID="uxEditRiskScore" runat="server" CommandSubmit="Update" />
                        </FormTemplate>
                    </EditFormSettings>
                    <ExpandCollapseColumn ButtonType="ImageButton" Visible="False" UniqueName="ExpandColumn">
                        <HeaderStyle Width="19px"></HeaderStyle>
                    </ExpandCollapseColumn>
                </MasterTableView>
            </as:ASGrid>
        </div>
        <as:HiddenField runat="server" ID="uxHdRiskSite" Value="-1" />
    </div>
    <!-- -->
    <!--  -->
    <asp:PlaceHolder ID="uxpnlAttributeRiskScore" runat="server">
        <div class="row">
            <div class="col-xs-12">
                <h2 class="grid-title inline-block no-toggle">                
                        <asp:Literal runat="server" ID="Literal1" Text="Attribute Risk Score" meta:resourcekey="ResourceAttributeRiskScore" />
                </h2>
                <div class="height-8"></div>
                <i class="text-muted">
                    <as:Literal ID="Literal4" runat="server" Text="Please enter integer values for all parameters and scores." meta:resourcekey="ltEnterIntergerResource1"></as:Literal></i>
            </div>
        </div>

        <div id="ciAttributeRiskScore" class="in" runat="server" >
            <!--Create new Attribute RiskScore -->
            <div class="row">
                <div class="col-md-12 no-margin-action-container" data-target=".create-attribute-rs-form" data-toggle="collapse">
                    <as:LinkButton runat="server" ID="btnCreateNewAttr" OnClientClick="return false;" CssClass="btn btn-default rm_riskscore_create_button" meta:resourcekey="uxCreateModeResourceAttributeRiskScore">Create New Attribute Risk Score</as:LinkButton>
                </div>
            </div>
            <div class="create-attribute-rs-form collapse" runat="server" id="pnlAttributeRiskScore">
                <uc:AttributeRiskScore ID="RiskScore1" runat="server" />
            </div>
            <div class="row">
                <div class="col-md-12" runat="server" onkeypress="return DefaultEnterOnDiv(event);">
                    <uc:UxExport ID="uxExportAttributeGrid" runat="server" GridID="uxAttributeGrid" GridHeader="Attribute Risk Score" 
                        OnNeedExportConfig="uxExportAttributeGrid_NeedExportConfig" meta:resourcekey="uxExportAttributeGridResource" ShowPDF="false" />
                    <as:ASGrid ID="uxAttributeGrid" runat="server" AllowPaging="True" GridLines="None" OnNeedDataSource="uxAttributeGrid_NeedDataSource"
                        AllowSorting="True" AutoGenerateColumns="False" OnPreRender="uxAttributeGrid_PreRender"
                        OnUpdateCommand="uxAttributeGrid_UpdateCommand" OnDeleteCommand="uxAttributeGrid_DeleteCommand"
                        AllowAutomaticDeletes="True" AllowAutomaticUpdates="True" OnItemDataBound="uxAttributeGrid_ItemDataBound"
                        OnItemCommand="uxAttributeGrid_ItemCommand" CssClass="in" meta:resourcekey="uxRadGridResource1" IsAutoExportTemplate="true">
                        <MasterTableView DataKeyNames="RecordID" GridLines="None" ShowFooter="false">
                            <Columns>
                               
                                <as:ASGridBoundColumn Visible="false" UniqueName="AttributeID" HeaderText="AttributeID"
                                    DataField="AttributeID" meta:resourcekey="ASGridBoundColumnResource1">
                                    <ColumnValidationSettings>
                                        <ModelErrorMessage Text=""></ModelErrorMessage>
                                    </ColumnValidationSettings>
                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                </as:ASGridBoundColumn>

                                <as:ASGridBoundColumn UniqueName="AttributeName" HeaderText="Attribute Name" DataField="AttributeName"
                                    ASFormat="DynamicString" HeaderTooltip="Attribute Name" meta:resourcekey="ASGridBoundColumnResourceAttributeName">
                                    <ColumnValidationSettings>
                                        <ModelErrorMessage Text=""></ModelErrorMessage>
                                    </ColumnValidationSettings>

                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                </as:ASGridBoundColumn>

                                <as:ASGridBoundColumn UniqueName="RangeName" HeaderText="Range Name" DataField="RangeName"
                                    ASFormat="DynamicString" HeaderTooltip="Range Name" meta:resourcekey="ASGridBoundColumnResourceRangeName">
                                    <ColumnValidationSettings>
                                        <ModelErrorMessage Text=""></ModelErrorMessage>
                                    </ColumnValidationSettings>

                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                </as:ASGridBoundColumn>

                                <as:ASGridBoundColumn UniqueName="FromValue" HeaderText="From"
                                    DataField="FromValue" ItemStyle-HorizontalAlign="Right" ASFormat="Integer"
                                    HeaderTooltip="From" meta:resourcekey="ASGridBoundColumnResourceFrom">
                                    <ColumnValidationSettings>
                                        <ModelErrorMessage Text=""></ModelErrorMessage>
                                    </ColumnValidationSettings>

                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                </as:ASGridBoundColumn>

                                <as:ASGridBoundColumn UniqueName="ToValue" HeaderText="To"
                                    DataField="ToValue" ItemStyle-HorizontalAlign="Right" ASFormat="Integer"
                                    HeaderTooltip="To" meta:resourcekey="ASGridBoundColumnResourceTo">
                                    <ColumnValidationSettings>
                                        <ModelErrorMessage Text=""></ModelErrorMessage>
                                    </ColumnValidationSettings>

                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                </as:ASGridBoundColumn>

                                <as:ASGridBoundColumn UniqueName="Operand" HeaderText="Operand"
                                    DataField="Operand" ItemStyle-HorizontalAlign="Right"
                                    HeaderTooltip="Operand" meta:resourcekey="ASGridBoundColumnResourceOperand">
                                    <ColumnValidationSettings>
                                        <ModelErrorMessage Text=""></ModelErrorMessage>
                                    </ColumnValidationSettings>

                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                </as:ASGridBoundColumn>
                                <as:ASGridBoundColumn UniqueName="MetricText" HeaderText="Metric" ItemStyle-CssClass="word-wrapped"
                                    DataField="MetricText" meta:resourcekey="ASGridBoundColumnResourceMetric">
                                    <ColumnValidationSettings>
                                        <ModelErrorMessage Text=""></ModelErrorMessage>
                                    </ColumnValidationSettings>

                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                </as:ASGridBoundColumn>

                                <as:ASGridBoundColumn UniqueName="Score" HeaderText="Score" DataField="Score"
                                    ASFormat="Integer" HeaderTooltip="Score" meta:resourcekey="ASGridBoundColumnResourceScore">
                                    <ColumnValidationSettings>
                                        <ModelErrorMessage Text=""></ModelErrorMessage>
                                    </ColumnValidationSettings>

                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                </as:ASGridBoundColumn>

                                 <tek:GridEditCommandColumn UniqueName="EditCommandColumn" HeaderText=""  meta:resourcekey="GridEditCommandColumnResource1"
                                    HeaderTooltip="" ItemStyle-HorizontalAlign="Center"
                                    HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="45px">
                                    <HeaderStyle HorizontalAlign="Center" Width="45px"></HeaderStyle>

                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                </tek:GridEditCommandColumn>

                                <tek:GridButtonColumn UniqueName="DeleteColumn" Text="Delete" CommandName="Delete"
                                    HeaderStyle-Width="60px"
                                    ConfirmDialogType="Classic" ConfirmText='Are you sure you would like to delete this Attribute Risk Score record?'
                                    ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderTooltip="Click to delete risk score" meta:resourcekey="GridButtonColumnResourceAttr">
                                    <HeaderStyle HorizontalAlign="Center" Width="60px"></HeaderStyle>

                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                </tek:GridButtonColumn>
                            </Columns>
                            <EditFormSettings EditFormType="Template">
                                <EditColumn UniqueName="EditCommandColumn1" FilterControlAltText="Filter EditCommandColumn1 column"></EditColumn>
                                <FormTemplate>
                                    <uc:EditAttributeRiskScore ID="uxEditAttributeRiskScore" runat="server" OnSubmit="uxEditAttributeRiskScore_OnSubmit" />

                                </FormTemplate>
                            </EditFormSettings>
                            <ExpandCollapseColumn ButtonType="ImageButton" Visible="False" UniqueName="ExpandColumn">
                                <HeaderStyle Width="19px"></HeaderStyle>
                            </ExpandCollapseColumn>
                        </MasterTableView>
                    </as:ASGrid>

                </div>
            </div>
        </div>

        <!-- -->
    </asp:PlaceHolder>

    <as:ASRadCodeBlock ID="RadCodeBlock" runat="server">
        <script type="text/javascript">
            var rm_RiskScores_uxHdRiskSite = document.getElementById("<%=uxHdRiskSite.ClientID%>");
        </script>
        <script src="<%= ResolveUrl("~/")%>res/js/risk_MCF/rm_MCF_RiskScores.js"> 
        </script>

    </as:ASRadCodeBlock>
</asp:content>
