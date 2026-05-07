<%@ Page Language="C#" MasterPageFile="~/MasterPagePopup.master" Title="CARD VOLUME"
    AutoEventWireup="true" CodeFile="CardTypeModal.aspx.cs" Inherits="CardTypeModal" meta:resourcekey="PageResource1" %>

<%@ Register TagName="UxExportTable" Src="~/UserControls/UxExportTable.ascx" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPage" runat="Server">
    <as:ASModalContainer ID="uxModalContainer" runat="server" WidthCssClass="modal-xl" ContainerCssClass="container" Width="">
        
        <uc:UxExportTable ID="uxExportCardVolume" runat="server" OnExportExcel="uxExportCardVolume_ExportExcel" ShowCSV="false" IsOnTop="true" />

        <table id="tblCardVolume" class="ASTable">
            <tr>
                <th>
                    <as:Literal ID="ltCardTypes" runat="server" Text="Card Types" meta:resourcekey="ltCardTypesResource1"></as:Literal>
                </th>
                <th>
                    <as:Literal ID="ltToday" runat="server" Text="Previous day" meta:resourcekey="ltTodayResource1"></as:Literal>
                </th>
                <th>
                    <as:Literal ID="ltMTD" runat="server" Text="MTD" meta:resourcekey="ltMTDResource1"></as:Literal>
                </th>
                <th>
                    <as:Literal ID="ltYTD" runat="server" Text="YTD" meta:resourcekey="ltYTDResource1"></as:Literal>
                </th>
            </tr>
            <asp:Repeater ID="rptCardVolume" runat="server">
                <ItemTemplate>
                    <tr class="Row">
                        <td class="static-string">
                            <%# Eval("CardType")%>
                        </td>
                        <td class="currency">
                            <%# FormatCurrency(Eval("TodayNetVolume"))%>
                        </td>
                        <td class="currency">
                            <%# FormatCurrency(Eval("MTDNetVolume"))%>
                        </td>
                        <td class="currency">
                            <%# FormatCurrency(Eval("YTDNetVolume"))%>
                        </td>
                    </tr>
                </ItemTemplate>
                <AlternatingItemTemplate>
                    <tr class="AltRow">
                        <td class="static-string">
                            <%# Eval("CardType")%>
                        </td>
                        <td class="currency">
                            <%# FormatCurrency(Eval("TodayNetVolume"))%>
                        </td>
                        <td class="currency">
                            <%# FormatCurrency(Eval("MTDNetVolume"))%>
                        </td>
                        <td class="currency">
                            <%# FormatCurrency(Eval("YTDNetVolume"))%>
                        </td>
                    </tr>
                </AlternatingItemTemplate>
            </asp:Repeater>
        </table>
        <asp:PlaceHolder ID="plhNoRecords" runat="server" Visible="false">
            <div class="NoRecords">
                <as:Literal ID="Literal1" runat="server" Text="No records to display." meta:resourcekey="Literal1Resource1"></as:Literal>
            </div>
        </asp:PlaceHolder>
        
    </as:ASModalContainer>
</asp:Content>
