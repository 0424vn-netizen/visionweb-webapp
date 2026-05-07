<%@ Page Title="Chain Statement" Language="C#" MasterPageFile="~/MasterPagePopup.master" AutoEventWireup="true"
    CodeFile="StatementDetails_Chain.aspx.cs" Inherits="StatementDetails_Chain" meta:resourcekey="PageResource2" %>

<%@ Register Src="~/UserControls/UxExport.ascx" TagName="UxExport" TagPrefix="uc" %>
<%@ Register TagName="PageTitle" Src="~/UserControls/PageTitle.ascx" TagPrefix="uc" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="Server">
    <uc:PageTitle ID="uxReportTitle" runat="server" ReportTitle="<img src='App_Themes/Orion/images/logo.jpg' alt='logo' />" meta:resourcekey="uxReportTitleResource1" />
    <br style="clear: both;" />
    <div style="min-width: 900px;">
        <as:Container ID="uxContainerDates" runat="server" Width="100%" HeaderText="Statement For Chain {0}" FooterControlID="" FooterText="" HeaderControlID="" TemplateName="ascontainer_greyborder.tpl" meta:resourcekey="uxContainerDatesResource1">
            <table width="100%" cellpadding="0" cellspacing="0" border="0">
                <tr>
                    <td width="100px" style="padding-left: 20px;">
                        <as:Literal ID="ltReportDate" runat="server" Text="Report Date" meta:resourcekey="ltReportDateResource1"></as:Literal>
                    </td>
                    <td width="230px">
                        <as:RadComboBox ID="uxReportDate" runat="server" Width="200px" meta:resourcekey="uxReportDateResource1">
                        </as:RadComboBox>
                    </td>
                    <td>
                        <as:Button runat="server" ID="uxShowStatement" Text="View Statement" CssClass="FormButtonLong"
                            OnClick="uxShowStatement_Click" meta:resourcekey="uxShowStatementResource1" />
                    </td>
                </tr>
            </table>
        </as:Container>
        <br />
        <uc:UxExport ID="uxExportTop" GridID="uxReportGrid" runat="server" />
        <as:ASGrid ID="uxReportGrid" runat="server" AllowPaging="True" AllowSorting="True"
            Visible="false" AutoGenerateColumns="False" AllowAutoCalculateTotalOnExport="false"
            AllowFilteringByColumn="false" ASPagingMethod="SPASingleMethod" ShowPageTotal="false"
            ShowReportTotal="true" ShowFooter="true" AllowSortFilterWhenExport="true" meta:resourcekey="uxReportGridResource1">
            <MasterTableView>
                <Columns>
                    <as:ASGridBoundColumn HeaderText="Merchant ID" HeaderTooltip="Merchant ID" DataField="MerchantNumber"
                        SortExpression="MerchantNumber" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource1">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn UniqueName="MerchantName" HeaderText="Merchant Name" HeaderTooltip="Merchant Name"
                        DataField="MerchantName" SortExpression="MerchantName" ASFormat="DynamicString" meta:resourcekey="ASGridBoundColumnResource2">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn UniqueName="ActiveStatus" ASFormat="StaticString" DataField="ActiveStatus"
                        HeaderTooltip="Status" HeaderText="Status" SortExpression="ActiveStatus" meta:resourcekey="ASGridBoundColumnResource3">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn UniqueName="TransactionCount" ASFormat="Integer" DataField="TransactionCount" ASIsTotalColumn="true"
                        HeaderTooltip="Transaction Count" HeaderText="Transaction Count" SortExpression="TransactionCount" meta:resourcekey="ASGridBoundColumnResource4">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn UniqueName="Sales" ASFormat="Currency" DataField="Sales" HeaderTooltip="Sales"
                        HeaderText="Sales" SortExpression="Sales" ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource5">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn UniqueName="Credits" ASFormat="Currency" DataField="Credits"
                        HeaderTooltip="Credits" HeaderText="Credits" SortExpression="Credits" ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource6">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn UniqueName="Deposits" ASFormat="Currency" DataField="Deposits"
                        HeaderTooltip="Deposit Total" HeaderText="Deposit Total" SortExpression="Deposits" ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource7">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn UniqueName="Fees" ASFormat="Currency" DataField="Fees" HeaderTooltip="Total Fees"
                        HeaderText="Total Fees" SortExpression="Fees" ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource8">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                </Columns>
            </MasterTableView>
        </as:ASGrid>
        <uc:UxExport ID="uxExportBot" IsBottom="true" GridID="uxReportGrid" runat="server" />
    </div>
</asp:Content>
