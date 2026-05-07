<%@ Page Title="CLOSED MERCHANTS" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ClosedMerchantReport.aspx.cs" Inherits="ClosedMerchantReport" meta:resourcekey="PageResource1" %>

<%@ Register Src="~/UserControls/PageTitle.ascx" TagName="PageTitle" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/ReportFiltering.ascx" TagName="ReportFiltering" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/UxExport.ascx" TagName="UxExport" TagPrefix="uc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="Server">
    <uc:PageTitle ID="uxPageTitle" runat="server" ReportTitle="OTHER REPORTS - CLOSED MERCHANTS" meta:resourcekey="uxPageTitleResource2"/>
    <uc:ReportFiltering ID="uxReportFiltering" runat="server" />
    <as:PlaceHolder ID="plhContain" runat="server">
        <uc:UxExport ID="uxExporterTop" runat="server" GridID="uxMerchantGrid" GridHeader="New Merchant List" meta:resourcekey="uxExporterTopResource2"/>
        <as:ASGrid ID="uxMerchantGrid" runat="server" AutoGenerateColumns="false" AllowSorting="false"
            AllowPaging="true" Width="100%" ASPagingMethod="SPASingleMethod" ShowReportTotal="false" ShowPageTotal="false" CssClass="in" meta:resourcekey="uxMerchantGridResource1">
            <MasterTableView>
                <Columns>
                    <as:ASGridBoundColumn DataField="MerchantNumber" UniqueName="MerchantNumber" HeaderText="Merchant ID"
                        HeaderTooltip="Merchant ID" AllowSorting="true" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource1">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="MerchantName" UniqueName="MerchantName" HeaderText="Merchant Name"
                        HeaderTooltip="Merchant Name" ASFormat="DynamicString" meta:resourcekey="ASGridBoundColumnResource2">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <%--<as:ASGridBoundColumn DataField="Corporate" UniqueName="Corporate" HeaderText="Corporate" ASFormat="StaticString"
                        HeaderTooltip="Corporate">
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="Region" UniqueName="Region" HeaderText="Region" ASFormat="StaticString"
                        HeaderTooltip="Region">
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="Principal" UniqueName="Principal" HeaderText="Principal" ASFormat="StaticString"
                        HeaderTooltip="Principal">
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="Association" UniqueName="Association" HeaderText="Associate" ASFormat="StaticString"
                        HeaderTooltip="Associate">
                    </as:ASGridBoundColumn>--%>
                    <as:ASGridBoundColumn DataField="ChainNumber" UniqueName="ChainNumber" HeaderText="Chain" HeaderTooltip="Chain" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource3">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="ApprovalDate" UniqueName="OpenDate" HeaderText="Open Date"
                        HeaderTooltip="Open Date" ASFormat="Date" meta:resourcekey="ASGridBoundColumnResource4">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="FirstDepositDate" UniqueName="FirstDepositDate"
                        HeaderText="1st Deposit Date" HeaderTooltip="First Deposit Date" ASFormat="Date" meta:resourcekey="ASGridBoundColumnResource5">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="ClosedDate" UniqueName="CLosedDate" HeaderText="Closed Date"
                        HeaderTooltip="Closed Date" ASFormat="Date" meta:resourcekey="ASGridBoundColumnResource6">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>

                </Columns>
            </MasterTableView>
        </as:ASGrid>
        <uc:UxExport ID="uxExporterBottom" runat="server" GridID="uxMerchantGrid" GridHeader="New Merchant List" IsBottom="true" meta:resourcekey="uxExporterBottomResource1"/>
    </as:PlaceHolder>

    <as:Button ID="uxProcess" runat="server" Style="display: none;" IsStandardButton="True" OnClick="uxProcess_Click" meta:resourcekey="uxProcessResource1" />
    <asp:HiddenField ID="uxProcessData" runat="server" />
    <as:RadCodeBlock ID="RadCodeBlock1" runat="server">

        <script type="text/javascript">
            function Merchant_Click(merchantNumber) {
                $get('<%= uxProcessData.ClientID %>').value = merchantNumber;
                $get('<%= uxProcess.ClientID %>').click();
                return true;
            }
        </script>

    </as:RadCodeBlock>

    <tek:RadAjaxManagerProxy runat="server" ID="uxRadAjaxManager">
        <AjaxSettings>
            <tek:AjaxSetting AjaxControlID="uxMerchantGrid">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="uxMerchantGrid" />
                </UpdatedControls>
            </tek:AjaxSetting>
        </AjaxSettings>
    </tek:RadAjaxManagerProxy>
</asp:Content>

