<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ASPager.ascx.cs" Inherits="UserControls_ASPager" %>

<div class="RadGrid RadGrid_Default in freeze-table">
    <div class="rgDataDiv">
        <table class="rgMasterTable rgClipCells" style="width: 100%; table-layout: fixed; overflow: hidden; empty-cells: show;">
            <thead style="display: none;">
                <tr>
                    <th scope="col"></th>
                </tr>
            </thead>
            <tbody>
                <tr class="rgPager customPager">
                    <td colspan="5">
                        <div class="row">
                            <div class="col-xs-3">
                                <span class="rgPageText">
                                    <asp:Literal ID="Literal1" runat="server" Text="Page:" meta:resourcekey="LiteralResource1" />
                                </span>
                                <asp:Repeater runat="server" ID="pagerList" OnItemDataBound="uxPager_ItemDataBound">
                                    <HeaderTemplate>
                                        <input type="button" onclick="javascript:<%# FuncPageChange%>(<%# CurrentPageIndex %>)" <%# CurrentPageIndex == 0 ? " disabled='disabled' class='aspNetDisabled rgPagePrev'" : " class='rgPagePrev'" %> />
                                        <asp:Literal ID="uxPreviousGroup" runat="server" Visible="false" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <a href="javascript:<%# FuncPageChange%>(<%# Container.DataItem %>)" class="<%# CurrentPageIndex+1==(int)Container.DataItem?"rgPageNum rgCurrentPage":"rgPageNum" %>"><%# Container.DataItem %></a>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Literal ID="uxNextGroup" runat="server" Visible="false" />
                                        <input type="button" onclick="javascript:<%# FuncPageChange%>(<%# CurrentPageIndex+2 %>)" <%# CurrentPageIndex < PageCount - 1 ? " class='rgPageNext'" : "disabled='disabled' class='aspNetDisabled rgPageNext'" %> />
                                    </FooterTemplate>
                                </asp:Repeater>
                            </div>
                            <div class="col-xs-6 text-center">
                                <span class="rgPageText">
                                    <asp:Literal runat="server" ID="pagerInformationLabel" meta:resourcekey="pagerInformationLabelResource1" /></span>
                            </div>
                        </div>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
</div>
