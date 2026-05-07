<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MessageGrid.ascx.cs" Inherits="MessageGrid" %>
<%@ Register TagName="UxExport" Src="~/UserControls/UxExport.ascx" TagPrefix="uc" %>
<div>
    <uc:UxExport ID="uxExportTop" GridID="uxGrid"
        IsBottom="true" ShowCSV="true" ShowExcel="true" ShowPDF="true"
        ShowWord="false" runat="server" Visible="False" OnNeedExportConfig="uxExporter_NeedExportConfig" />
    <asp:PlaceHolder runat="server" ID="dsf">
        <as:ASGrid ID="uxGrid" runat="server" Visible="true" ASPagingMethod="SPASingleMethod"
            OnNeedDataSource="uxGrid_NeedDataSource" AutoGenerateColumns="false" AllowSorting="True"
            AllowPaging="True" PageSize="10" CssClass="in" meta:resourcekey="uxGridResource1">
            <MasterTableView>
                <Columns>
                    <as:ASGridBoundColumn HeaderText="RecordID" DataField="RecordID" UniqueName="RecordID"
                        HeaderStyle-HorizontalAlign="Center" Visible="false" meta:resourcekey="ASGridBoundColumnResource1">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Date/Time Sent" DataField="CreationDate" UniqueName="CreationDate"
                        HeaderStyle-HorizontalAlign="Center" ASFormat="DateAndTime" ItemStyle-Width="15%"
                        HeaderTooltip="Date/Time Sent"
                        HeaderStyle-Width="15%" meta:resourcekey="ASGridBoundColumnResource2">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center" Width="15%"></HeaderStyle>

                        <ItemStyle Width="15%"></ItemStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Author" DataField="UserID" UniqueName="UserID"
                        HeaderStyle-HorizontalAlign="Center"
                        ItemStyle-Width="15%" HeaderStyle-Width="15%" ASFormat="DynamicString" HeaderTooltip="Author" meta:resourcekey="ASGridBoundColumnResource3">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center" Width="15%"></HeaderStyle>

                        <ItemStyle Width="15%"></ItemStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Message" DataField="Message" HtmlEncode="true"
                        HeaderTooltip="Message"
                        UniqueName="Message" HeaderStyle-HorizontalAlign="Center" ASFormat="DynamicString"
                        ItemStyle-Width="60%" HeaderStyle-Width="60%" meta:resourcekey="ASGridBoundColumnResource4">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center" Width="60%"></HeaderStyle>

                        <ItemStyle Width="60%"></ItemStyle>
                    </as:ASGridBoundColumn>
                </Columns>
            </MasterTableView>
        </as:ASGrid>
    </asp:PlaceHolder>
    <uc:UxExport ID="uxExportBtm" GridID="uxGrid"
        IsBottom="true" ShowCSV="true" ShowExcel="true" ShowPDF="true"
        ShowWord="false" runat="server" Visible="False" OnNeedExportConfig="uxExporter_NeedExportConfig" />
</div>
