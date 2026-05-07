<%@ Control Language="C#" AutoEventWireup="true" CodeFile="RiskAssignmentListSimple.ascx.cs"
    Inherits="UserControls_RiskAssignmentListSimple" %>
<%@ Register TagName="ProgressBar" TagPrefix="uc1" Src="ProgressBar.ascx" %>
<%@ Register TagName="UxExport" Src="UxExport.ascx" TagPrefix="uc" %>
<style type="text/css">
    .colspancolumn {
        background-color: #000;
        color: #fff;
    }
</style>
<tek:RadAjaxManagerProxy runat="server" ID="uxRadAjaxManagerProxy">
    <AjaxSettings>
        <tek:AjaxSetting AjaxControlID="uxReportGrid">
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="uxReportGrid" LoadingPanelID="LoadingPannel" />
            </UpdatedControls>
        </tek:AjaxSetting>
    </AjaxSettings>

</tek:RadAjaxManagerProxy>
<tek:RadAjaxLoadingPanel ID="LoadingPannel" BackgroundPosition="Top" runat="server">
</tek:RadAjaxLoadingPanel>
<uc:UxExport ID="uxExporter" runat="server" GridID="uxReportGrid" ShowPDF="true"
    ShowWord="false" OnNeedExportConfig="uxExport_NeedExportConfig" />
<as:ASGrid ID="uxReportGrid" runat="server" AutoGenerateColumns="false" AllowSorting="true"
    AllowPaging="false"
    ShowFooter="true" OnNeedDataSource="uxGrid_NeedDataSource" OnItemDataBound="uxGrid_ItemDataBound" CssClass="in" meta:resourcekey="uxReportGridResource1">
    <MasterTableView NoMasterRecordsText="Data not found.">
        <Columns>
            <as:ASGridBoundColumn HeaderText="Assignment Name" DataField="AssignmentName"
                UniqueName="AssignmentName" ASFormat="StaticString" HeaderTooltip="Assignment Name"
                SortExpression="AssignmentName" meta:resourcekey="ASGridBoundColumnResource1">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
            </as:ASGridBoundColumn>

            <as:ASGridBoundColumn HeaderText="Total Merch" DataField="TotalMerchantCount"
                UniqueName="TotalMerchantCount" Visible="true" ASFormat="Integer"
                HeaderTooltip="# of merchants matching assignment filters"
                SortExpression="TotalMerchantCount" meta:resourcekey="ASGridBoundColumnResource2">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
            </as:ASGridBoundColumn>
            <as:ASGridBoundColumn HeaderText="Alert" DataField="AlertMerchantCount"
                UniqueName="AlertMerchantCount" Visible="true" ASFormat="Integer"
                SortExpression="AlertMerchantCount" HeaderTooltip="# of merchants matching assignment filters and parameters" meta:resourcekey="ASGridBoundColumnResource3">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
            </as:ASGridBoundColumn>
            <as:ASGridBoundColumn HeaderText="Volume" DataField="TotalMerchantAmount" UniqueName="Volume"
                ASFormat="Currency"
                HeaderTooltip="Today’s volume for merchants matching assignment" SortExpression="TotalMerchantAmount" meta:resourcekey="ASGridBoundColumnResource4">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
            </as:ASGridBoundColumn>
            <as:ASGridBoundColumn HeaderText="#" DataField="WorkedMerchantCount"
                UniqueName="WorkedMerchantCount" Visible="true" ASFormat="Integer"
                SortExpression="WorkedMerchantCount" HeaderTooltip="# of merchants worked from this assignment" meta:resourcekey="ASGridBoundColumnResource5">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
            </as:ASGridBoundColumn>
            <as:ASGridBoundColumn HeaderText="Volume" DataField="WorkedMerchantAmount"
                UniqueName="WorkedMerchantAmount" Visible="true" ASFormat="Currency"
                SortExpression="WorkedMerchantAmount" HeaderTooltip="Volume for merchants worked from this assignment" meta:resourcekey="ASGridBoundColumnResource6">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
            </as:ASGridBoundColumn>
            <tek:GridTemplateColumn DataField="CompletedPercent" UniqueName="CompleteTemplate"
                HeaderText="<div title='% Complete'>% Complete</div>" HeaderStyle-HorizontalAlign="Center"
                ItemStyle-HorizontalAlign="Center" SortExpression="CompletedPercent" meta:resourcekey="GridTemplateColumnResource1">
                <ItemTemplate>
                    <uc1:ProgressBar ID="progressBar" runat="server" Width="80" Blocks="5" Value='<%# Eval("CompletedPercent") %>' />
                </ItemTemplate>
                <FooterTemplate>
                    <uc1:ProgressBar ID="progressBarFooter" runat="server" Width="80" Blocks="5" />
                </FooterTemplate>

                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                <ItemStyle HorizontalAlign="Center"></ItemStyle>
            </tek:GridTemplateColumn>
            <as:ASGridBoundColumn UniqueName="CompletedPercent" DataField="CompletedPercent"
                HeaderText="% Complete" ASFormat="Percentage" ItemStyle-HorizontalAlign="Center"
                Visible="false" meta:resourcekey="ASGridBoundColumnResource7">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                <ItemStyle HorizontalAlign="Center"></ItemStyle>
            </as:ASGridBoundColumn>
        </Columns>
    </MasterTableView>
</as:ASGrid>
<uc:UxExport ID="uxExporterBottom" runat="server" GridID="uxReportGrid" IsBottom="true"
    ShowPDF="true" ShowWord="false" OnNeedExportConfig="uxExport_NeedExportConfig" />
<tek:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <script type="text/javascript">

        $(document).ready(function () {
            SetHeaderGrid();
        });
        function ajaxResponseEnd(sender, args) {
            SetHeaderGrid();
        }
        function SetHeaderGrid() {
            addGroupHeadersForStaticRadGrid('<%= uxReportGrid.ClientID %>',
                        [['', 1, 'rgHeader'],
                        ['<%= GetLocalResourceObject("RiskAssignmentListSimple_ascx_MerchantCount").ToString() %>', 2, 'rgHeader'],
                        ['', 1, 'rgHeader'],
                        ['<%= GetLocalResourceObject("RiskAssignmentListSimple_ascx_Worked").ToString() %>', 2, 'rgHeader'],
                        ['', 1, 'rgHeader']]);
        }
    </script>
</tek:RadCodeBlock>
