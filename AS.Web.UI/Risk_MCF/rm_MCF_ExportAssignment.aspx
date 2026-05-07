<%@ Page Title="Manage Assignments" Language="C#" AutoEventWireup="true" EnableViewState="false" CodeFile="rm_MCF_ExportAssignment.aspx.cs" Inherits="rm_MCF_ExportAssignment" meta:resourcekey="PageResource1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>
        <asp:Literal ID="uxTitleVoucher" runat="server" meta:resourcekey="uxTitleVoucherResource1"> VOUCHER </asp:Literal></title>
</head>
<body>
    <form id="form1" runat="server">
        <style type="text/css">
            .RowTitle {
                font-size: 14px;
                font-weight: bold;
                border-bottom: 1px solid black;
            }

            .Header {
                border-right: 1px solid black;
                border-bottom: 1px solid black;
            }

            .FirstHeader {
                border-left: 1px solid black;
                border-right: 1px solid black;
                border-bottom: 1px solid black;
            }

            .FirstData {
                border-left: 1px solid black;
                border-right: 1px solid black;
                border-bottom: 1px solid black;
            }

            .Data {
                border-right: 1px solid black;
                border-bottom: 1px solid black;
            }
        </style>
        <div>
            <b>
                <asp:Literal ID="Literal1" runat="server" meta:resourcekey="Literal1Resource1" Text="RISK MANAGEMENT - ASSIGNMENTS - MANAGE ASSIGNMENTS"></asp:Literal></b>
        </div>
        <table cellspacing='0' cellpadding="3" border='0' style='width: 100%;'>
            <asp:Repeater runat="server" ID="uxReportRepeater">
                <ItemTemplate>
                    <thead>
                        <tr>
                            <td colspan='15' class="RowTitle">
                                <%# Eval("RowTitle") %>&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td class="FirstHeader" align="center">
                                <asp:Literal ID="uxAssignmentName" runat="server" meta:resourcekey="uxAssignmentNameResource1" Text=" Assignment Name"></asp:Literal>
                            </td>
                            <td class="Header" align="center">
                                <asp:Literal ID="Literal2" runat="server" meta:resourcekey="Literal2Resource1" Text=" Merchant Count: Eligible"></asp:Literal>
                            </td>
                            <td class="Header" align="center">
                                <asp:Literal ID="Literal3" runat="server" meta:resourcekey="Literal3Resource1" Text=" Merchant Count: Alert "></asp:Literal>
                            </td>
                            <td class="Header" align="center">
                                <asp:Literal ID="Literal4" runat="server" meta:resourcekey="Literal4Resource1" Text=" Merchant Count: Amount "></asp:Literal>
                            </td>
                             <td class="Header" align="center">
                                <asp:Literal ID="Literal31" runat="server" meta:resourcekey="Literal25Resource1_" Text=" Ready to Work Count "></asp:Literal>
                            </td>
                            <td class="Header" align="center">
                                <asp:Literal ID="Literal32" runat="server" meta:resourcekey="Literal26Resource1_" Text=" Ready to Work Net Amount "></asp:Literal>
                            </td>
                            <td class="Header" align="center">
                                <asp:Literal ID="Literal27" runat="server" meta:resourcekey="Literal25Resource1" Text=" Work In Progress Count "></asp:Literal>
                            </td>
                            <td class="Header" align="center">
                                <asp:Literal ID="Literal28" runat="server" meta:resourcekey="Literal26Resource1" Text=" Work In Progress Net Amount "></asp:Literal>
                            </td>
                            <td class="Header" align="center">
                                <asp:Literal ID="Literal5" runat="server" meta:resourcekey="Literal5Resource1" Text=" Worked Count"></asp:Literal>
                            </td>
                            <td class="Header" align="center">
                                <asp:Literal ID="Literal6" runat="server" meta:resourcekey="Literal6Resource1" Text=" Worked Net Amount"></asp:Literal>
                            </td>
                            <td class="Header" align="center">
                                <asp:Literal ID="Literal9" runat="server" meta:resourcekey="Literal9Resource1" Text=" Percent Worked"></asp:Literal>
                            </td>
                            <td class="Header" align="center">
                                <asp:Literal ID="Literal10" runat="server" meta:resourcekey="Literal10Resource1_" Text=" Merchants Alerted and Re-Alerted"></asp:Literal>
                            </td>
                             <td class="Header" align="center">
                                <asp:Literal ID="Literal7" runat="server" meta:resourcekey="Literal10Resource1" Text=" Expiration Date"></asp:Literal>
                            </td>
                            <td class="Header" align="center">
                                <asp:Literal ID="Literal11" runat="server" meta:resourcekey="Literal11Resource1" Text=" Proc Status"></asp:Literal>
                            </td>
                            <td class="Header" align="center">
                                <asp:Literal ID="Literal12" runat="server" meta:resourcekey="Literal12Resource1" Text=" Last Proc Status Date"></asp:Literal>
                            </td>
                        </tr>
                    </thead>
                    <tbody>
                        <asp:Repeater runat="server" ID="uxDataRepeater">
                            <ItemTemplate>
                                <tr>
                                    <td class="FirstData" align='center'><%# Eval("AssignmentName")%>&nbsp;</td>
                                    <td class="Data" align='right'><%# AS.Common.Formater.FormatData.FormatInteger(Eval("TotalMerchantCount"))%>&nbsp;</td>
                                    <td class="Data" align='right'><%# AS.Common.Formater.FormatData.FormatInteger(Eval("AlertMerchantCount"))%>&nbsp;</td>
                                    <td class="Data" align='right'><%# AS.Common.Formater.FormatData.FormatCurrency(Eval("AlertMerchantVolume"))%>&nbsp;</td>
                                    <td class="Data" align='right'><%# AS.Common.Formater.FormatData.FormatInteger(Eval("WKCount"))%>&nbsp;</td>
                                    <td class="Data" align='right'><%# AS.Common.Formater.FormatData.FormatCurrency(Eval("WKVolume"))%>&nbsp;</td>
                                    <td class="Data" align='right'><%# AS.Common.Formater.FormatData.FormatInteger(Eval("WIPCount"))%>&nbsp;</td>
                                    <td class="Data" align='right'><%# AS.Common.Formater.FormatData.FormatCurrency(Eval("WIPVolume"))%>&nbsp;</td>
                                    <td class="Data" align='right'><%# AS.Common.Formater.FormatData.FormatInteger(Eval("WorkedCount"))%>&nbsp;</td>
                                    <td class="Data" align='right'><%# AS.Common.Formater.FormatData.FormatCurrency(Eval("WorkedVolume"))%>&nbsp;</td>
                                    <td class="Data" align='right'><%# AS.Common.Formater.FormatData.FormatPercent(Eval("CompletePercent"))%>&nbsp;</td>
                                    <td class="Data" align='right'><%# AS.Common.Formater.FormatData.FormatInteger(Eval("AlertMerchantCountAllCycles"))%>&nbsp;</td>
                                    <td class="Data" align='center'><%# ((DateTime)Eval("ExpirationDate")).Year == 2999 ? "Never Expire" : FormatDate(Eval("ExpirationDate"))%>&nbsp;</td>
                                    <td class="Data" align='center'><%# Eval("ProcessingStatus")%>&nbsp;</td>
                                    <td class="Data" align='center'><%# FormatDateTime(Eval("ProcessingStatusDate"))%>&nbsp;</td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </tbody>
                </ItemTemplate>
            </asp:Repeater>
            <asp:Repeater runat="server" ID="uxRequeuedReportRepeater">
                <ItemTemplate>
                    <thead>
                        <tr>
                            <td colspan='18' class="RowTitle">
                                <%# Eval("RowTitle") %>&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td class="FirstHeader" align="center">
                                <asp:Literal ID="uxAssignment1" runat="server" meta:resourcekey="uxAssignmentNameResource1" Text=" Assignment Name"></asp:Literal>
                            </td>
                            <td class="Header" align="center">
                                <asp:Literal ID="Literal13" runat="server" meta:resourcekey="Literal13Resource1" Text=" Type"></asp:Literal>
                            </td>
                            <td class="Header" align="center">
                                <asp:Literal ID="Literal14" runat="server" meta:resourcekey="Literal2Resource1" Text=" Merchant Count: Total Merch"></asp:Literal>
                            </td>
                            <td class="Header" align="center">
                                <asp:Literal ID="Literal15" runat="server" meta:resourcekey="Literal15Resource1" Text="  Merchant Count: Alert "></asp:Literal>
                            </td>
                            <td class="Header" align="center">
                                <asp:Literal ID="Literal16" runat="server" meta:resourcekey="Literal4Resource1" Text="Merchant Count: Amount "></asp:Literal>
                            </td>
                            <td class="Header" align="center">
                                <asp:Literal ID="Literal29" runat="server" meta:resourcekey="Literal25Resource1_" Text="Ready to Work Count"></asp:Literal>
                            </td>
                            <td class="Header" align="center">
                                <asp:Literal ID="Literal30" runat="server" meta:resourcekey="Literal26Resource1_" Text="Ready to Work Net Amount"></asp:Literal>
                            </td>
                            <td class="Header" align="center">
                                <asp:Literal ID="Literal27" runat="server" meta:resourcekey="Literal25Resource1" Text=" Work In Progress Count"></asp:Literal>
                            </td>
                            <td class="Header" align="center">
                                <asp:Literal ID="Literal28" runat="server" meta:resourcekey="Literal26Resource1" Text=" Work In Progress Net Amount"></asp:Literal>
                            </td>
                            <td class="Header" align="center">
                                <asp:Literal ID="Literal17" runat="server" meta:resourcekey="Literal5Resource1" Text=" Worked Count"></asp:Literal>
                            </td>
                            <td class="Header" align="center">
                                <asp:Literal ID="Literal18" runat="server" meta:resourcekey="Literal6Resource1" Text=" Worked Net Amount"></asp:Literal>
                            </td>
                            <td class="Header" align="center">
                                <asp:Literal ID="Literal23" runat="server" meta:resourcekey="Literal9Resource1" Text=" Percent Worked"></asp:Literal>
                            </td>
                            <td class="Header" align="center">
                                <asp:Literal ID="Literal19" runat="server" meta:resourcekey="Literal19Resource1" Text=" Re-queued Count"></asp:Literal>
                            </td>
                            <td class="Header" align="center">
                                <asp:Literal ID="Literal20" runat="server" meta:resourcekey="Literal20Resource1" Text=" Re-queued Net Amount"></asp:Literal>
                            </td>
                            <td class="Header" align="center">
                                <asp:Literal ID="Literal10" runat="server" meta:resourcekey="Literal10Resource1_" Text=" Merchants Alerted and Re-Alerted"></asp:Literal>
                            </td>
                            <td class="Header" align="center">
                                <asp:Literal ID="Literal24" runat="server" meta:resourcekey="Literal24Resource1" Text="Expiration Date"></asp:Literal>
                            </td>
                            <td class="Header" align="center">
                                <asp:Literal ID="Literal25" runat="server" meta:resourcekey="Literal11Resource1" Text=" Proc Status"></asp:Literal>
                            </td>
                            <td class="Header" align="center">
                                <asp:Literal ID="Literal26" runat="server" meta:resourcekey="Literal12Resource1" Text=" Last Proc Status Date"></asp:Literal>
                            </td>
                        </tr>
                    </thead>
                    <tbody>
                        <asp:Repeater runat="server" ID="uxDataRepeater">
                            <ItemTemplate>
                                <tr>
                                    <td class="FirstData" align='center'><%# Eval("AssignmentName")%>&nbsp;</td>
                                    <td class="Data" align='right'><%# Eval("AssignmentType")%>&nbsp;</td>
                                    <td class="Data" align='right'><%# Eval("TotalMerchantCount")%>&nbsp;</td>
                                    <td class="Data" align='right'><%# AS.Common.Formater.FormatData.FormatInteger(Eval("AlertMerchantCount"))%>&nbsp;</td>
                                    <td class="Data" align='right'><%# AS.Common.Formater.FormatData.FormatCurrency(Eval("AlertMerchantVolume"), SessionManager.CurrencyFortmat)%>&nbsp;</td>
                                    <td class="Data" align='right'><%# AS.Common.Formater.FormatData.FormatInteger(Eval("WKCount"))%>&nbsp;</td>
                                    <td class="Data" align='right'><%# AS.Common.Formater.FormatData.FormatCurrency(Eval("WKVolume"))%>&nbsp;</td>
                                    <td class="Data" align='right'><%# AS.Common.Formater.FormatData.FormatInteger(Eval("WIPCount"))%>&nbsp;</td>
                                    <td class="Data" align='right'><%# AS.Common.Formater.FormatData.FormatCurrency(Eval("WIPVolume"))%>&nbsp;</td>
                                    <td class="Data" align='right'><%# AS.Common.Formater.FormatData.FormatInteger(Eval("WorkedCount"))%>&nbsp;</td>
                                    <td class="Data" align='right'><%# AS.Common.Formater.FormatData.FormatCurrency(Eval("WorkedVolume"), SessionManager.CurrencyFortmat)%>&nbsp;</td>
                                    <td class="Data" align='right'><%# AS.Common.Formater.FormatData.FormatPercent(Eval("CompletePercent"))%>&nbsp;</td>
                                    <td class="Data" align='right'><%# AS.Common.Formater.FormatData.FormatInteger(Eval("RequeueCount"))%>&nbsp;</td>
                                    <td class="Data" align='right'><%# AS.Common.Formater.FormatData.FormatCurrency(Eval("RequeueVolume"))%>&nbsp;</td>
                                    <td class="Data" align='right'><%# AS.Common.Formater.FormatData.FormatInteger(Eval("AlertMerchantCountAllCycles"))%>&nbsp;</td>
                                    <td class="Data" align='center'><%# Eval("ExpirationDateExport")%>&nbsp;</td>
                                    <td class="Data" align='center'><%# Eval("ProcessingStatus")%>&nbsp;</td>
                                    <td class="Data" align='center'><%# FormatDateTime(Eval("ProcessingStatusDate"))%>&nbsp;</td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </tbody>
                </ItemTemplate>
            </asp:Repeater>
        </table>
    </form>
</body>
</html>
