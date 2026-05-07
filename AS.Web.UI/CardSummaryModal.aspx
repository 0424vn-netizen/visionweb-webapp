<%@ Page Language="C#" MasterPageFile="~/MasterPagePopup.master" AutoEventWireup="true" CodeFile="CardSummaryModal.aspx.cs"
    Inherits="CardSummaryModal" Title="Card Summary" meta:resourcekey="PageResource1" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="Server">

    <tek:RadAjaxManagerProxy runat="server" ID="uxRadAjaxManager">
        <AjaxSettings>
            <tek:AjaxSetting AjaxControlID="uxReportGrid">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="uxReportGrid" />
                </UpdatedControls>
            </tek:AjaxSetting>
        </AjaxSettings>
    </tek:RadAjaxManagerProxy>
    <as:ASModalContainer ID="uxModalContainer" runat="server" WidthCssClass="modal-xl" ContainerCssClass="container" Width="">

        <h3 class="modal-title">
            <asp:Label ID="ltrHeaderTop" runat="server" meta:resourcekey="ltrHeaderTopResource1"></asp:Label>
        </h3>
        <uc:UxExport ID="uxExporter" runat="server" GridID="uxReportGrid" ShowWord="false" IsOnTop="true" />
            <as:ASGrid ID="uxReportGrid" runat="server" GridLines="None" IsAutoExportTemplate="true"
                AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False"  AppendHeaderforPrinter="true" 
                ShowFooter="false" ASPagingMethod="SPASingleMethod" ShowPageTotal="false"
                GridName="Card Summary" VisiblePageTotal="false" VisibleReportTotal="true" AllowSortFilterWhenExport="true" CssClass="in" meta:resourcekey="uxReportGridResource1">
                <MasterTableView>
                    <Columns>
                        <as:ASGridBoundColumn UniqueName="CardType" HeaderText="Card Type" DataField="CardType"
                            SortExpression="CardType" HeaderTooltip="Card Type" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource1">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text=""></ModelErrorMessage>
                            </ColumnValidationSettings>

                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        </as:ASGridBoundColumn>
                        <as:ASGridBoundColumn UniqueName="SaleCount" HeaderText="# Sales" DataField="SaleCount"
                            HeaderTooltip="Sales Transaction Count" SortExpression="SaleCount" ASFormat="Integer"
                            ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource2">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text=""></ModelErrorMessage>
                            </ColumnValidationSettings>

                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        </as:ASGridBoundColumn>
                        <as:ASGridBoundColumn UniqueName="SaleAmount" HeaderText="Sales" DataField="SaleAmount"
                            HeaderTooltip="Sales Amount" SortExpression="SaleAmount" ASIsTotalColumn="true" ASFormat="Currency" meta:resourcekey="ASGridBoundColumnResource3">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text=""></ModelErrorMessage>
                            </ColumnValidationSettings>

                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        </as:ASGridBoundColumn>
                        <as:ASGridBoundColumn UniqueName="ReturnCount" HeaderText="# Returns" DataField="ReturnCount"
                            HeaderTooltip="Returns Transaction Count" SortExpression="ReturnCount" ASFormat="Integer"
                            ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource4">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text=""></ModelErrorMessage>
                            </ColumnValidationSettings>

                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        </as:ASGridBoundColumn>
                        <as:ASGridBoundColumn UniqueName="ReturnAmount" HeaderText="Returns" DataField="ReturnAmount"
                            HeaderTooltip="Returns Amount" SortExpression="ReturnAmount" ASFormat="Currency"
                            ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource5">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text=""></ModelErrorMessage>
                            </ColumnValidationSettings>

                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        </as:ASGridBoundColumn>
                        <as:ASGridBoundColumn UniqueName="NetAmount" HeaderText="Net" DataField="NetAmount"
                            HeaderTooltip="Net Amount" SortExpression="NetAmount"
                            ASIsTotalColumn="true" ASFormat="Currency" meta:resourcekey="ASGridBoundColumnResource6">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text=""></ModelErrorMessage>
                            </ColumnValidationSettings>

                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        </as:ASGridBoundColumn>
                    </Columns>
                </MasterTableView>
            </as:ASGrid>
    </as:ASModalContainer>
</asp:Content>

