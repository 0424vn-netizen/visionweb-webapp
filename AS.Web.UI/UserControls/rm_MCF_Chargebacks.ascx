<%@ Control Language="C#" AutoEventWireup="true" CodeFile="rm_MCF_Chargebacks.ascx.cs" Inherits="UserControls_rm_MCF_Chargebacks" %>

<%--DO not remove this comment--%>
<!--Content-->
<as:Panel ID="uxPanelChargeback" runat="server" meta:resourcekey="uxPanelChargebackResource1">
    <div class="row">
        <div class="col-xs-10">
            <h2 class="grid-title" data-toggle="collapse" data-target="#cidChargeBackGrid">
                <span class="text-muted">
                    <asp:Literal runat="server" ID="litHeaderChargeback90days" Text="Chargebacks (90 days)" meta:resourcekey="litHeaderChargeback90daysResource1" /></span>
            </h2>
        </div>
        <div class="col-xs-2 mt-6x">
            <div id="uxExportPannel" runat="server" class="report-export on-top dropdown pull-right">
                <a href="#" data-toggle="dropdown" data-hover="dropdown" class="dropdown-toggle">
                    <asp:Literal ID="rm_DQNextQReportPopup_aspx_Text2" runat="server" meta:resourcekey="rm_DQNextQReportPopup_aspx_Text2Resource1" Text="EXPORT"></asp:Literal></a>
                <ul class="dropdown-menu">
                    <li>
                        <asp:LinkButton runat="server" ID="uxExportExcel" OnClick="ExportButtonExcel_Click" Text="Excel" meta:resourcekey="uxExportExcelResource1"></asp:LinkButton>
                    </li>
                </ul>
            </div>
        </div>
    </div>
    <div id="cidChargeBackGrid" class="in">
        <asp:PlaceHolder ID="plhuxChargeback" runat="server">
            <table class="ASTable grid-transaction freeze-table-no-pager in freeze-table">
                <colgroup>
                    <col style="width: 150px;" />
                    <col />
                    <col />
                    <col />
                    <col />
                </colgroup>
                <tr>
                    <as:TableColumnHeader ID="uxAccountNumberHeader" Visible="false" HeaderText="" meta:resourcekey="ASGridBoundColumnResource36" runat="server" />
                    <as:TableColumnHeader ID="uxPartialAccountNumberHeader" HeaderText="" meta:resourcekey="ASGridBoundColumnResource37" runat="server" />
                    <as:TableColumnHeader ID="TableColumnHeader44" HeaderText="" meta:resourcekey="ASGridBoundColumnResource47" runat="server" />
                    <as:TableColumnHeader ID="TableColumnHeader45" HeaderText="" meta:resourcekey="ASGridBoundColumnResource48" runat="server" />
                    <as:TableColumnHeader ID="TableColumnHeader26" HeaderText="" meta:resourcekey="ASGridBoundColumnResource38" runat="server" />
                    <as:TableColumnHeader ID="TableColumnHeader27" HeaderText="" meta:resourcekey="ASGridBoundColumnResource39" runat="server" />
                    <as:TableColumnHeader ID="TableColumnHeader28" HeaderText="" meta:resourcekey="ASGridBoundColumnResource40" runat="server" />
                    <as:TableColumnHeader ID="TableColumnHeader29" HeaderText="" meta:resourcekey="ASGridBoundColumnResource41" runat="server" />
                </tr>
                <as:ASRepeater ID="uxChargeback" runat="server" NumberOfColumns="7" OnItemDataBound="uxChargeback_ItemDataBound" OnPreRender="uxChargeback_PreRender">
                    <ItemTemplate>
                        <tr class='<%# Container.ItemIndex %2 == 0 ? "Row" : "AltRow" %>'>
                            <as:TableColumnContent ID="uxAccountNumber" UniqueName="AccountNumber" Visible="false" ASFormat="Auto" Alignment="Center" runat="server" />
                            <as:TableColumnContent ID="uxPartialAccountNumber" UniqueName="PartialAccountNumber" ASFormat="Auto" Alignment="Center" runat="server" />
                            <as:TableColumnContent ID="uxCardType" UniqueName="CardType" Alignment="Center" runat="server" />
                            <as:TableColumnContent ID="uxReportDate" UniqueName="ReportDate" ASFormat="Date" Alignment="Center" runat="server" />
                            <as:TableColumnContent ID="uxTransactionDate" UniqueName="TransactionDate" ASFormat="Date" Alignment="Center" runat="server" />
                            <as:TableColumnContent ID="uxReasonCode" UniqueName="ReasonCode" Alignment="Left" Text="" runat="server" />
                            <as:TableColumnContent ID="uxKeyed" UniqueName="Keyed" Alignment="Center" Text="" runat="server" />
                            <as:TableColumnContent ID="uxTransactionAmount" UniqueName="TransactionAmount" Alignment="Right" Text="" runat="server" />
                        </tr>
                    </ItemTemplate>
                </as:ASRepeater>
                <asp:PlaceHolder ID="uxChargebackFooter" runat="server">
                    <tr class="Footer">
                        <as:TableColumnContent ID="uxPartialAccountNumberTotal" UniqueName="PartialAccountNumberTotal" Alignment="Left" Text="" runat="server" />
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                        <as:TableColumnContent ID="uxKeyedTotal" UniqueName="KeyedTotal" Alignment="Center" Text="" runat="server" />
                        <as:TableColumnContent ID="uxTransactionAmountTotal" UniqueName="TransactionAmountTotal" Alignment="Right" Text="" runat="server" />
                    </tr>
                </asp:PlaceHolder>
            </table>
        </asp:PlaceHolder>
    </div>
</as:Panel>
<%--DO not remove this comment--%>
<!--/Content-->
