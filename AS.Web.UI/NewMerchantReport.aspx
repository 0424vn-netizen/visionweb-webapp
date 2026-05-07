<%@ Page Title="NEW MERCHANTS" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="NewMerchantReport.aspx.cs" Inherits="NewMerchantReport" meta:resourcekey="PageResource1" %>

<%@ Register Src="~/UserControls/PageTitle.ascx" TagName="PageTitle" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/ReportFiltering.ascx" TagName="ReportFiltering" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/UxExport.ascx" TagName="UxExport" TagPrefix="uc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="Server">
    <uc:PageTitle ID="uxPageTitle" runat="server" ReportTitle="OTHER REPORTS - NEW MERCHANTS" meta:resourcekey="uxPageTitleResource1" />
    <uc:ReportFiltering ID="uxReportFiltering" runat="server" />
    <as:PlaceHolder ID="plhContain" runat="server">
        <uc:UxExport ID="uxExporterTop" runat="server" GridID="uxMerchantGrid" GridHeader="New Merchant List" meta:resourcekey="uxExporterTopResource1" />
        <as:ASGrid ID="uxMerchantGrid" runat="server" AutoGenerateColumns="false" AllowSorting="false"
            AllowPaging="true" Width="100%" ASPagingMethod="SPASingleMethod" CssClass="in" meta:resourcekey="uxMerchantGridResource1">
            <MasterTableView>
                <Columns>
                    <as:ASGridBoundColumn DataField="MerchantNumber" UniqueName="MerchantNumber" HeaderText="Merchant ID" ASFormat="StaticString"
                        HeaderTooltip="Merchant ID" AllowSorting="true" meta:resourcekey="ASGridBoundColumnResource1">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="MerchantName" UniqueName="MerchantName" HeaderText="Merchant Name" ASFormat="DynamicString"
                        HeaderTooltip="Merchant Name" meta:resourcekey="ASGridBoundColumnResource2">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <%-- <as:ASGridBoundColumn DataField="Corporate" UniqueName="Corporate" HeaderText="Corp" ASFormat="StaticString"
                        HeaderTooltip="Corporate">
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="Region" UniqueName="Region" HeaderText="Regn" ASFormat="StaticString"
                        HeaderTooltip="Region">
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="Principal" UniqueName="Principal" HeaderText="Prin" ASFormat="StaticString"
                        HeaderTooltip="Principal">
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="Association" UniqueName="Associate" HeaderText="Asso" ASFormat="StaticString"
                        HeaderTooltip="Associate">
                    </as:ASGridBoundColumn>--%>
                    <as:ASGridBoundColumn DataField="ChainNumber" UniqueName="Chain" HeaderText="Chain" HeaderTooltip="Chain" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource3">
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
                    <as:ASGridBoundColumn DataField="ProcessingDate" UniqueName="ProcessingDate" HeaderText="Processing Date"
                        HeaderTooltip="Processing Date" ASFormat="Date" meta:resourcekey="ASGridBoundColumnResource6">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                </Columns>
            </MasterTableView>
        </as:ASGrid>
        <uc:UxExport ID="uxExporterBottom" runat="server" GridID="uxMerchantGrid" IsBottom="true" />
    </as:PlaceHolder>

    <as:Button ID="uxProcess" runat="server" Style="display: none;" IsStandardButton="True" OnClick="uxProcess_Click" meta:resourcekey="uxProcessResource1" />
    <as:HiddenField ID="uxProcessData" runat="server" />
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
            <tek:AjaxSetting AjaxControlID="uxProcess">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="uxMerchantGrid" />
                </UpdatedControls>
            </tek:AjaxSetting>
        </AjaxSettings>
    </tek:RadAjaxManagerProxy>
</asp:Content>

