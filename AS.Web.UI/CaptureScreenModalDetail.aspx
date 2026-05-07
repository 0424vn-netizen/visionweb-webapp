<%@ Page Title="Capture Details" Language="C#" MasterPageFile="~/MasterPagePopup.master"
    AutoEventWireup="true" CodeFile="CaptureScreenModalDetail.aspx.cs" Inherits="CaptureScreenModalDetail" meta:resourcekey="PageResource1" %>

<%@ Register Src="~/UserControls/PageTitle.ascx" TagName="PageTitle" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="Server">
    <style type="text/css">
        .right
        {
            padding-right: 7px;
            text-align: right;
        }
        .bold_text
        {
            font-weight: bold;
        }
        .bgHeader
        {
            background-color: #C2C2C2;
            height: 28px;
            text-align: center;
            vertical-align: middle;
            text-decoration: underline;
            font-weight: bold;
        }       
        .altbg
        {
            border-right-width: 0px;
        }
        .hd
        {
            height: 28px;
            text-align: left;
            font-family: Arial,Helvetica,Sans-serif;
            font-size: 12px;
            font-weight: bold;
        }
        .hddt
        {
            padding-left: 10px !important;
        }
        .RadGrid_Default .rgRow td, .RadGrid_Default .rgAltRow td, .RadGrid_Default .rgEditRow td, .RadGrid_Default .rgFooter td
        {
            border-style: solid;
        }
        .RadGrid_Default
        {
            background: none repeat scroll 0 0 #FFFFFF;
            border: 1px solid #828282;
            color: #333333;
        }
        table.MPSBorder td, table.MPSBorder th 
        {
            height: 22px !important;
        }
    </style>
    <uc:PageTitle ID="uxPageTitle" runat="server" ReportTitle="Capture Details" meta:resourcekey="uxPageTitleResource1" />
    <div style="width: 900px;">
        <asp:Repeater ID="uxCaptureDetail" runat="server">
            <ItemTemplate>
                <div id="format" class="RadGrid RadGrid_Default">
                    <table cellpadding="0" cellspacing="0" class="rgMasterTable" width="100%" style="font-size: 12px;">
                        <tr class="ContainerPanelHeader hd">
                            <th colspan="2" class="hddt" align="left" style="border-right-width: 1px">
                                <as:Literal ID="ltTerminal" runat="server" Text="Terminal" meta:resourcekey="ltTerminalResource1"></as:Literal>
                            </th>
                        </tr>
                        <tr class="rgRow" align="left">
                            <td width="18%" valign="top" align="right" style="border-left-width: 1px; border-top-width: 0px;
                                border-bottom-width: 0px">
                                <as:Literal ID="ltChainNumber" runat="server" Text="Chain Number" meta:resourcekey="ltChainNumberResource1"></as:Literal>
                            </td>
                            <td style="border-left-width: 1px; border-top-width: 0px; border-bottom-width: 0px">
                                <%# Eval("ChainNumber")%>&nbsp;
                            </td>
                        </tr>
                        <tr class="rgRow altbg MPSBorderAltRow" align="left">
                            <td align="right" style="border-top-width: 0px; border-bottom-width: 0px">
                                <as:Literal ID="ltOwner" runat="server" Text="Owner" meta:resourcekey="ltOwnerResource1"></as:Literal>
                            </td>
                            <td style="border-left-width: 1px; border-top-width: 0px; border-bottom-width: 0px;">
                                <%# Eval("Owner")%>&nbsp;
                            </td>
                        </tr>
                        <tr class="rgRow" align="left">
                            <td align="right">
                                <as:Literal ID="ltPlan" runat="server" Text="Plan" meta:resourcekey="ltPlanResource1"></as:Literal>
                            </td>
                            <td>
                                <%# Eval("Plan")%>&nbsp;
                            </td>
                        </tr>
                        <tr class="ContainerPanelHeader hd">
                            <th colspan="2" class="hddt" align="left" style="border-right-width: 1px">
                                <as:Literal ID="ltInterchange" runat="server" Text="Interchange" meta:resourcekey="ltInterchangeResource1"></as:Literal>
                            </th>
                        </tr>
                        <tr class="rgRow" align="left">
                            <td width="30%" valign="top" align="right" style="border-left-width: 1px; border-top-width: 0px;
                                border-bottom-width: 0px">
                                <as:Literal ID="ltTranID" runat="server" Text="Tran ID" meta:resourcekey="ltTranIDResource1"></as:Literal>
                            </td>
                            <td style="border-left-width: 1px; border-top-width: 0px; border-bottom-width: 0px">
                                <%# Eval("TransID")%>&nbsp;
                            </td>
                        </tr>
                        <tr class="rgRow altbg MPSBorderAltRow" align="left">
                            <td align="right" style="border-top-width: 0px; border-bottom-width: 0px">
                                <as:Literal ID="ltAuthSource" runat="server" Text="Auth Source" meta:resourcekey="ltAuthSourceResource1"></as:Literal>
                            </td>
                            <td style="border-left-width: 1px; border-top-width: 0px; border-bottom-width: 0px;">
                                <%# Eval("AuthorizationSource")%>&nbsp;
                            </td>
                        </tr>
                        <tr class="rgRow" align="left">
                            <td align="right">
                                <as:Literal ID="ltAVSResponseCode" runat="server" Text="AVS Response Code" meta:resourcekey="ltAVSResponseCodeResource1"></as:Literal>
                            </td>
                            <td>
                                <%# Eval("AVSResponseCode")%>&nbsp;
                            </td>
                        </tr>
                        <tr class="rgRow altbg MPSBorderAltRow" align="left">
                            <td align="right" style="border-top-width: 0px; border-bottom-width: 0px">
                                <as:Literal ID="ltAuthResponseCode" runat="server" Text="Auth Response Code" meta:resourcekey="ltAuthResponseCodeResource1"></as:Literal>
                            </td>
                            <td style="border-left-width: 1px; border-top-width: 0px; border-bottom-width: 0px;">
                                <%# Eval("AuthResponse")%>&nbsp;
                            </td>
                        </tr>
                        <tr class="rgRow" align="left">
                            <td align="right">
                                <as:Literal ID="ltMarketDataCode" runat="server" Text="Market Data Code" meta:resourcekey="ltMarketDataCodeResource1"></as:Literal>
                            </td>
                            <td>
                                <%# Eval("MarketDataCode")%>&nbsp;
                            </td>
                        </tr>
                        <tr class="rgRow altbg MPSBorderAltRow" align="left">
                            <td align="right" style="border-top-width: 0px; border-bottom-width: 0px">
                                <as:Literal ID="ltCVVV" runat="server" Text="CVC2/CVV2/CID" meta:resourcekey="ltCVVVResource1"></as:Literal>
                            </td>
                            <td style="border-left-width: 1px; border-top-width: 0px; border-bottom-width: 0px;">
                                <%# Eval("CVVDescription")%>&nbsp;
                            </td>
                        </tr>
                        <tr class="rgRow" align="left">
                            <td align="right">
                                <as:Literal ID="ltProductID" runat="server" Text="Product ID" meta:resourcekey="ltProductIDResource1"></as:Literal>
                            </td>
                            <td>
                                <%# Eval("ProductID")%>&nbsp;
                            </td>
                        </tr>
                        <tr class="rgRow altbg MPSBorderAltRow" align="left">
                            <td align="right" style="border-top-width: 0px; border-bottom-width: 0px">
                                <as:Literal ID="ltAcquirerReferenceData" runat="server" Text="Acquirer Reference Data" meta:resourcekey="ltAcquirerReferenceDataResource1"></as:Literal>
                            </td>
                            <td style="border-left-width: 1px; border-top-width: 0px; border-bottom-width: 0px;">
                                <%# Eval("AcquirerReferenceID")%>&nbsp;
                            </td>
                        </tr>
                        <tr class="ContainerPanelHeader hd">
                            <th colspan="2" class="hddt" align="left" style="border-right-width: 1px">
                                <as:Literal ID="ltProcessing" runat="server" Text="Processing" meta:resourcekey="ltProcessingResource1"></as:Literal>
                            </th>
                        </tr>
                        <tr class="rgRow" align="left">
                            <td align="right" width="30%" valign="top" style="border-left-width: 1px; border-top-width: 0px;
                                border-bottom-width: 0px">
                                <as:Literal ID="ltCountryCode" runat="server" Text="Country Code" meta:resourcekey="ltCountryCodeResource1"></as:Literal>
                            </td>
                            <td style="border-left-width: 1px; border-top-width: 0px; border-bottom-width: 0px">
                                <%# Eval("CountryCode")%>&nbsp;
                            </td>
                        </tr>
                        <tr class="rgRow altbg MPSBorderAltRow" align="left">
                            <td align="right" style="border-top-width: 0px; border-bottom-width: 0px">
                                <as:Literal ID="ltIssuerID" runat="server" Text="Issuer ID" meta:resourcekey="ltIssuerIDResource1"></as:Literal>
                            </td>
                            <td style="border-left-width: 1px; border-top-width: 0px; border-bottom-width: 0px;">
                                <%# Eval("IssuerID")%>&nbsp;
                            </td>
                        </tr>
                        <tr class="rgRow" align="left">
                            <td align="right">
                                <as:Literal ID="ltCurrencyCode" runat="server" Text="Currency Code" meta:resourcekey="ltCurrencyCodeResource1"></as:Literal>
                            </td>
                            <td>
                                <%# Eval("CurrencyCode")%>&nbsp;
                            </td>
                        </tr>
                        <tr class="rgRow altbg MPSBorderAltRow" align="left">
                            <td align="right">
                                <as:Literal ID="ltSourceID" runat="server" Text="Source ID" meta:resourcekey="ltSourceIDResource1"></as:Literal>
                            </td>
                            <td>
                                <%# Eval("SourceID")%>&nbsp;
                            </td>
                        </tr>
                    </table>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>
</asp:Content>
