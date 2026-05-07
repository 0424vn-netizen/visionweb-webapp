<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Title="Rejects"
    CodeFile="Rejects.aspx.cs" Inherits="Rejects" meta:resourcekey="PageResource1" %>

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
        <as:ASGrid ID="uxDrilldownGrid" runat="server" GridLines="None" AutoGenerateColumns="false"
            AllowSorting="true" AllowPaging="true" ShowPageTotal="false" Visible="false" AppendHeaderforPrinter="true" 
            Width="100%" ASPagingMethod="SPASingleMethod" meta:resourcekey="uxDrilldownGridResource1" IsAutoExportTemplate="true">
            <MasterTableView>
                <Columns>
                    <as:ASGridBoundColumn DataField="Entity" UniqueName="DrilldownColumn" HeaderText=""
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
                    <as:ASGridBoundColumn UniqueName="TransactionCount" HeaderText="# Trans" DataField="TransCount"
                        ASFormat="Integer" HeaderTooltip="Transaction Count" ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource3">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn UniqueName="SaleAmount" HeaderText="Sales" DataField="SaleAmount"
                        ASFormat="Currency" ASIsTotalColumn="true" HeaderTooltip="Sales Amount" meta:resourcekey="ASGridBoundColumnResource4">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn UniqueName="ReturnAmount" HeaderText="Returns" DataField="ReturnAmount"
                        ASFormat="Currency" ASIsTotalColumn="true" HeaderTooltip="Returns Amount" meta:resourcekey="ASGridBoundColumnResource5">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn UniqueName="NetAmount" HeaderText="Net" DataField="NetAmount"
                        ASFormat="Currency" ASIsTotalColumn="true" HeaderTooltip="Net Amount" meta:resourcekey="ASGridBoundColumnResource6">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn UniqueName="RejectsCount" HeaderText="# Rejects" DataField="RejectsCount"
                        ASFormat="Integer" HeaderTooltip="Rejects Count" ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource7">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn UniqueName="RejectsAmount" HeaderText="Rejects" DataField="RejectsAmount"
                        ASFormat="Currency" HeaderTooltip="Rejects Amount" ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource8">
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
        <as:ASGrid ID="uxReportGrid" runat="server" AutoGenerateColumns="false" AllowSorting="true"  AppendHeaderforPrinter="true" 
            AllowPaging="true" ShowPageTotal="false" Visible="false" ASPagingMethod="SPASingleMethod" 
            meta:resourcekey="uxReportGridResource1" IsAutoExportTemplate="true">
            <MasterTableView>
                <Columns>
                    <as:ASGridBoundColumn DataField="TransactionDate" UniqueName="TransactionDate" HeaderText="Trans Date"
                        ASFormat="Date" HeaderTooltip="Transaction Date" HeaderStyle-Width="100px" meta:resourcekey="ASGridBoundColumnResource9">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center" Width="100px"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="TransactionTime" UniqueName="TransactionTime" HeaderText="Trans Time"
                        ASFormat="StaticString" HeaderTooltip="Transaction Time" meta:resourcekey="ASGridBoundColumnResource10">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="TransactionCode" UniqueName="TransactionCode" HeaderText="Trans Code"
                        ASFormat="StaticString" HeaderTooltip="Transaction Code" meta:resourcekey="ASGridBoundColumnResource11">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="CardType" UniqueName="CardType" HeaderText="Card Type"
                        ASFormat="StaticString" HeaderTooltip="Card Type" meta:resourcekey="ASGridBoundColumnResource12">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="AccountNumber" UniqueName="AccountNumber" HeaderText="Card #" HeaderStyle-Width="150px"
                        ASFormat="StaticString" HeaderTooltip="Card Number" SortExpression="PartialCardNumber"
                        Visible="false" meta:resourcekey="ASGridBoundColumnResource13">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="PartialCardNumber" UniqueName="PartialCardNumber" HeaderStyle-Width="150px"
                        HeaderText="Card #" ASFormat="StaticString" HeaderTooltip="Card Number" SortExpression="PartialCardNumber" meta:resourcekey="ASGridBoundColumnResource14">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="ReferenceNumber" UniqueName="ReferenceNumber" HeaderStyle-Width="180px"
                        HeaderText="Reference #" ASFormat="StaticString" HeaderTooltip="Reference Number" meta:resourcekey="ASGridBoundColumnResource15">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="AuthorizationNumber" UniqueName="AuthorizationNumber"
                        HeaderText="Auth #" ASFormat="StaticString" HeaderTooltip="Authorization Number" meta:resourcekey="ASGridBoundColumnResource16">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="ReasonCodes" UniqueName="ReasonCodes" HeaderText="RC"
                        ASFormat="StaticString" HeaderTooltip="Reason Codes" meta:resourcekey="ASGridBoundColumnResource17">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="TransactionAmount" UniqueName="TransactionAmount"
                        HeaderText="Trans Amount" ASFormat="Currency" HeaderTooltip="Transaction Amount"
                        ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource18">
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
