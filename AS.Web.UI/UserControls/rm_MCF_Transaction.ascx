<%@ Control Language="C#" AutoEventWireup="true" CodeFile="rm_MCF_Transaction.ascx.cs" Inherits="UserControls_rm_MCF_Transaction" %>

<%@ Register TagName="ASPager" Src="~/UserControls/ASPager.ascx" TagPrefix="uc" %>
<as:Panel ID="uxPanelTransaction" runat="server" meta:resourcekey="uxPanelTransactionResource1">
    <div class="row">
        <div class="col-xs-12">
            <h2 class="grid-title" data-toggle="collapse" data-target="#cidTransGrid">
                <span class="text-muted">
                    <asp:Literal runat="server" ID="litHeaderTransactionToday" Text="Transactions (Today)" meta:resourcekey="litHeaderTransactionTodayResource1" /></span>
            </h2>
        </div>
    </div>

    <div id="cidTransGrid" class="in">
        <!--Transaction-->
        <asp:PlaceHolder ID="plhReportGridTransactions" runat="server">
            <table class="ASTable grid-transaction freeze-table-no-pager in freeze-table" id="transactionTbl">
                <colgroup>
                    <col style="width: 150px;" />
                    <col />
                    <col />
                    <col />
                    <col />
                    <col />
                    <col />
                    <col />
                    <col />
                    <col />
                    <col />
                    <col />
                </colgroup>
                <tr>
                    <as:TableColumnHeader ID="TableColumnHeader24" Visible="false" HeaderText="" meta:resourcekey="ASGridBoundColumnResource23" runat="server" />
                    <as:TableColumnHeader ID="TableColumnHeader25" HeaderText="" meta:resourcekey="ASGridBoundColumnResource24" runat="server" />
                    <as:TableColumnHeader ID="TableColumnHeader30" HeaderText="" meta:resourcekey="ASGridBoundColumnResource25" runat="server" />
                    <as:TableColumnHeader ID="TableColumnHeader31" HeaderText="" meta:resourcekey="ASGridBoundColumnResource45" runat="server" />
                    <as:TableColumnHeader ID="TableColumnHeader32" HeaderText="" meta:resourcekey="ASGridBoundColumnResource26" runat="server" />
                    <as:TableColumnHeader ID="TableColumnHeader33" HeaderText="" meta:resourcekey="ASGridBoundColumnResource27" runat="server" />
                    <as:TableColumnHeader ID="TableColumnHeader34" HeaderText="" meta:resourcekey="ASGridBoundColumnResource28" runat="server" />
                    <as:TableColumnHeader ID="TableColumnHeader35" HeaderTooltipID="TableColumnHeader35" HeaderText="" meta:resourcekey="ASGridBoundColumnResource46" runat="server" />
                    <as:TableColumnHeader ID="TableColumnHeader36" HeaderText="" meta:resourcekey="ASGridBoundColumnResource29" runat="server" />
                    <as:TableColumnHeader ID="TableColumnHeader37" HeaderText="" meta:resourcekey="ASGridBoundColumnResource30" runat="server" />
                    <as:TableColumnHeader ID="TableColumnHeader38" HeaderText="" meta:resourcekey="ASGridBoundColumnResource31" runat="server" />
                    <as:TableColumnHeader ID="TableColumnHeader39" HeaderText="" meta:resourcekey="ASGridBoundColumnResource32" runat="server" />
                    <as:TableColumnHeader ID="TableColumnHeader40" HeaderText="" meta:resourcekey="ASGridBoundColumnResource33" runat="server" />
                    <as:TableColumnHeader ID="TableColumnHeader41" HeaderText="" Visible="false" meta:resourcekey="ASGridBoundColumnResource34" runat="server" />
                    <as:TableColumnHeader ID="TableColumnHeader42" HeaderText="" Visible="false" meta:resourcekey="ASGridBoundColumnResource35" runat="server" />
                </tr>
                 <!--TransactionDetail-->
                <as:ASRepeater ID="uxReportGridTransactions" runat="server" NumberOfColumns="12" OnItemDataBound="uxReportGridTransactions_ItemDataBound" OnPreRender="uxReportGridTransactions_PreRender">
                    <ItemTemplate>
                        <tr class='<%# Container.ItemIndex %2 == 0 ? "Row" : "AltRow" %>'>
                            <as:TableColumnContent ID="uxAccountNumber" UniqueName="AccountNumber" Visible="false" ASFormat="Auto" Alignment="Center" runat="server" />
                            <as:TableColumnContent ID="uxPartialAccountNumber" UniqueName="PartialAccountNumber" ASFormat="Auto" Alignment="Center" runat="server" />
                            <as:TableColumnContent ID="uxCardType" UniqueName="CardType" ASFormat="Auto" Alignment="Center" runat="server" />
                            <as:TableColumnContent ID="uxCountryCode" UniqueName="CountryCode" Alignment="Center" Text="" runat="server" />
                            <as:TableColumnContent ID="uxFileSource" UniqueName="FileSource" Alignment="Center" Text="" runat="server" />
                            <as:TableColumnContent ID="uxTransactionDate" UniqueName="TransactionDate" Alignment="Center" Text="" runat="server" />
                            <as:TableColumnContent ID="uxTransactionTime" UniqueName="TransactionTime" ASFormat="Auto" Alignment="Center" runat="server" />
                            <as:TableColumnContent ID="uxDupeCount" UniqueName="DupeCount" Alignment="Center" Text="" runat="server" />
                            <as:TableColumnContent ID="uxAuthorizationNumber" UniqueName="AuthorizationNumber" Alignment="Center" Text="" runat="server" />
                            <as:TableColumnContent ID="uxKeyed" UniqueName="Keyed" Alignment="Center" Text="" runat="server" />
                            <as:TableColumnContent ID="uxTransactionType" UniqueName="TransactionDescription" ASFormat="Auto" Alignment="Center" runat="server" />
                            <as:TableColumnContent ID="uxMatch" UniqueName="MatchCode" Alignment="Center" Text="" runat="server" />
                            <as:TableColumnContent ID="uxTransactionAmount" UniqueName="TransactionAmount" Alignment="Right" Text="" runat="server" />
                            <as:TableColumnContent ID="uxDuplicateFlag" Visible="false" UniqueName="DuplicateFlag" Alignment="Right" Text="" runat="server" />
                            <as:TableColumnContent ID="uxHighestTransactionAmountFlag" Visible="false" UniqueName="HighestTransactionAmountFlag" Alignment="Right" Text="" runat="server" />
                        </tr>
                    </ItemTemplate>
                </as:ASRepeater>
                 <!--/TransactionDetail-->
                <asp:PlaceHolder ID="uxReportGridTransactionsFooter" runat="server">
                    <tr class="Footer">
                        <as:TableColumnContent ID="uxPartialAccountNumberTotal" UniqueName="PartialAccountNumberTotal" Alignment="Left" Text="" runat="server" />
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                        <as:TableColumnContent ID="uxTransKeyedTotal" UniqueName="TransKeyedTotal" Alignment="Center" Text="" runat="server" />
                        <td></td>
                        <td></td>
                        <as:TableColumnContent ID="uxTransactionAmountFooterTotal" UniqueName="TransactionAmountTotal" Alignment="Right" Text="" runat="server" />
                    </tr>
                </asp:PlaceHolder>
            </table>
        </asp:PlaceHolder>
        <div id="transPagerWrapper">
        <asp:Panel ID="uxPnlPager" runat="server"></asp:Panel>
        </div>
        <as:ASRadToolTip ID="tltDupeCount" meta:resourcekey="DupeCount" RenderMode="Lightweight" Position="BottomCenter" RelativeTo="Element"
            TargetControlID="TableColumnHeader35" runat="server" IsClientID="true" RenderInPageRoot="true" CssClass="RadTooltip" AutoCloseDelay="0">
        </as:ASRadToolTip>
    </div>
</as:Panel>

