<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Title="Downgraded Transactions"
    CodeFile="DowngradedTransactions.aspx.cs" Inherits="DowngradedTransactions" meta:resourcekey="PageResource1" %>

<%@ Register Src="~/UserControls/PageTitle.ascx" TagName="PageTitle" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/ReportFiltering.ascx" TagName="ReportFiltering"
    TagPrefix="uc" %>
<%@ Register Src="~/UserControls/UxExport.ascx" TagName="UxExport" TagPrefix="uc" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="server">
    <tek:RadAjaxManagerProxy runat="server" ID="uxRadAjaxManager">
        <AjaxSettings>
            <tek:AjaxSetting AjaxControlID="uxDrilldownGrid">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="uxDrilldownGrid" />
                </UpdatedControls>
            </tek:AjaxSetting>
        </AjaxSettings>
        <AjaxSettings>
            <tek:AjaxSetting AjaxControlID="uxReportGrid">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="uxReportGrid" />
                </UpdatedControls>
            </tek:AjaxSetting>
        </AjaxSettings>
    </tek:RadAjaxManagerProxy>

    <uc:ReportFiltering ID="uxReportFiltering" runat="server" />
    <uc:PageTitle ID="uxPageTitle" runat="server" ReportTitle="" HasShowHierarchy="true"
        HasFilteringOption="true" />

    <!-- Drilldown -->
    <as:PlaceHolder ID="uxDrilldownPlaceHolder" runat="server">
        <uc:UxExport ID="uxExporterDrilldown" runat="server" GridID="uxDrilldownGrid" IsOnTop="true" />
        <as:ASGrid ID="uxDrilldownGrid" runat="server" AutoGenerateColumns="false" AllowSorting="true"
            GridLines="None" AllowPaging="true" ShowPageTotal="false" Visible="false" XOverFlowable="false"
            ASPagingMethod="SPASingleMethod" meta:resourcekey="uxDrilldownGridResource1">
            <MasterTableView>
                <Columns>
                    <as:ASGridBoundColumn DataField="Entity" UniqueName="DrilldownColumn" HeaderText="" HeaderStyle-Width="130px"
                        HeaderTooltip="" ASFormat="StaticString" SortExpression="Entity" meta:resourcekey="ASGridBoundColumnResource1">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Entity Name" DataField="EntityName" UniqueName="EntityName"
                        SortExpression="EntityName" HeaderTooltip="Entity Name" ASFormat="DynamicString"
                        Visible="false" meta:resourcekey="ASGridBoundColumnResource2">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn UniqueName="DowngradesCount" HeaderText="# Downgrades" DataField="DowngradeCount"
                        ASFormat="Integer" HeaderTooltip="Downgrades Count" ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource3">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn UniqueName="TransactionCount" HeaderText="# Trans" DataField="TransCount"
                        ASFormat="Integer" HeaderTooltip="Transaction Count" ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource4">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn UniqueName="SaleAmount" HeaderText="Sales" DataField="SaleAmount"
                        ASFormat="Currency" ASIsTotalColumn="true" HeaderTooltip="Sales Amount" meta:resourcekey="ASGridBoundColumnResource5">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn UniqueName="ReturnAmount" HeaderText="Returns" DataField="ReturnAmount"
                        ASFormat="Currency" ASIsTotalColumn="true" HeaderTooltip="Returns Amount" meta:resourcekey="ASGridBoundColumnResource6">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn UniqueName="NetAmount" HeaderText="Net" DataField="NetAmount"
                        ASFormat="Currency" ASIsTotalColumn="true" HeaderTooltip="Net Amount" meta:resourcekey="ASGridBoundColumnResource7">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                </Columns>
            </MasterTableView>
        </as:ASGrid>
    </as:PlaceHolder>

    <!-- Detail view -->
    <as:PlaceHolder ID="uxReporterPlaceHolder" runat="server">
        <uc:UxExport ID="uxExporter" runat="server" GridID="uxReportGrid" IsOnTop="true" />
        <as:ASGrid ID="uxReportGrid" runat="server" AutoGenerateColumns="false" AllowSorting="true"
            AllowPaging="true" Width="100%" ShowPageTotal="false" Visible="false" ASPagingMethod="SPASingleMethod" meta:resourcekey="uxReportGridResource1">
            <MasterTableView>
                <Columns>
                    <as:ASGridBoundColumn DataField="ReportDate" UniqueName="ReportDate" HeaderText="Report Date"
                        ASFormat="Date" HeaderTooltip="Report Date" HeaderStyle-Width="90px" meta:resourcekey="ASGridBoundColumnResource8">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="TransactionDate" UniqueName="TransactionDate" HeaderText="Trans Date"
                        ASFormat="Date" HeaderTooltip="Transaction Date" HeaderStyle-Width="90px" meta:resourcekey="ASGridBoundColumnResource9">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="TransactionCode" UniqueName="TransactionCode" HeaderText="Trans Code"
                        ASFormat="StaticString" HeaderTooltip="Transaction Code" meta:resourcekey="ASGridBoundColumnResource10">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="QualificationCode" UniqueName="QualificationCode"
                        HeaderText="Qual Code" ASFormat="StaticString" HeaderTooltip="Qualification Code" meta:resourcekey="ASGridBoundColumnResource11">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="DownGradeReason1" UniqueName="DownGradeReason1"
                        HeaderText="DGR 1" ASFormat="StaticString" HeaderTooltip="DownGrade Reason 1" meta:resourcekey="ASGridBoundColumnResource12">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="DownGradeReason2" UniqueName="DownGradeReason2"
                        HeaderText="DGR 2" ASFormat="StaticString" HeaderTooltip="DownGrade Reason 2" meta:resourcekey="ASGridBoundColumnResource13">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="DownGradeReason3" UniqueName="DownGradeReason3"
                        HeaderText="DGR 3" ASFormat="StaticString" HeaderTooltip="DownGrade Reason 3" meta:resourcekey="ASGridBoundColumnResource14">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="ReasonCodes" UniqueName="ReasonCodes" HeaderText="RC"
                        ASFormat="StaticString" HeaderTooltip="Reason Codes" meta:resourcekey="ASGridBoundColumnResource15">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="AccountNumber" UniqueName="AccountNumber" HeaderText="Card #" HeaderStyle-Width="140px"
                        ASFormat="StaticString" HeaderTooltip="Card Number" SortExpression="PartialCardNumber"
                        Visible="false" meta:resourcekey="ASGridBoundColumnResource16">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="PartialCardNumber" UniqueName="PartialCardNumber" HeaderStyle-Width="140px"
                        HeaderText="Card #" ASFormat="StaticString" HeaderTooltip="Card Number" SortExpression="PartialCardNumber" meta:resourcekey="ASGridBoundColumnResource17">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="ReferenceNumber" UniqueName="ReferenceNumber" HeaderText="Reference #" HeaderStyle-Width="180px"
                        ASFormat="StaticString" HeaderTooltip="Reference Number" meta:resourcekey="ASGridBoundColumnResource18">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="AuthorizationDate" UniqueName="AuthorizationDate"
                        HeaderText="Auth Date" ASFormat="StaticString" HeaderTooltip="Authorization Date" meta:resourcekey="ASGridBoundColumnResource19">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="AuthorizationNumber" UniqueName="AuthorizationNumber"
                        HeaderText="Auth #" ASFormat="StaticString" HeaderTooltip="Authorization Number" meta:resourcekey="ASGridBoundColumnResource20">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="AuthorizationAmount" UniqueName="AuthorizationAmount"
                        ASIsTotalColumn="true" HeaderText="Auth Amount" ASFormat="Currency" HeaderTooltip="Authorization Amount" meta:resourcekey="ASGridBoundColumnResource21">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="TransactionAmount" UniqueName="TransactionAmount"
                        ASIsTotalColumn="true" HeaderText="Trans Amount" ASFormat="Currency" HeaderTooltip="Transaction Amount" meta:resourcekey="ASGridBoundColumnResource22">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                </Columns>
            </MasterTableView>
        </as:ASGrid>
    </as:PlaceHolder>

</asp:Content>
