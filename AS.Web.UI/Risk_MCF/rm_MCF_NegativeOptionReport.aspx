<%@ Page Title="Negative Option Report" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="rm_MCF_NegativeOptionReport.aspx.cs" Inherits="rm_MCF_NegativeOptionReport" meta:resourcekey="PageResource1" %>

<%@ Register Src="~/UserControls/UxExport.ascx" TagName="UxExport" TagPrefix="uc" %>
<asp:Content ID="Content" ContentPlaceHolderID="ContentPage" runat="Server">
    <tek:RadAjaxManagerProxy runat="server" ID="RadAjaxManager">
        <AjaxSettings>
            <tek:AjaxSetting AjaxControlID="uxReportGrid">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="uxReportGrid" />
                    <tek:AjaxUpdatedControl ControlID="uxExport" />
                </UpdatedControls>
            </tek:AjaxSetting>

        </AjaxSettings>
    </tek:RadAjaxManagerProxy>

    <div class="main-content">

        <%-- Filter --%>
        <div runat="server" id="uxFilterOption">
            <div class="row collapse report-filter-panel">
                <div class="col-md-12 report-filter">
                    <div class="filter-block">
                        <table>
                            <tr>
                                <td class="text-right">
                                    <label class="filter-label">
                                        <asp:Literal ID="Literal1" runat="server" Text="Card Type:"></asp:Literal></label>
                                </td>
                                <td class="text-left">
                                    <div class="filter-item">
                                        <as:ASRadComboBox ID="uxCardType" Filter="Contains" MarkFirstMatch="true" runat="server" Width="197" AutoPostBack="false"></as:ASRadComboBox>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td class="text-right">
                                    <label class="filter-label">
                                        <asp:Literal ID="Literal2" runat="server" Text="60-Day Rolling:"></asp:Literal></label>
                                </td>
                                <td class="text-left">
                                    <div class="filter-item">
                                        <as:RadDatePicker ID="uxReportDate" runat="server" ShowPopupOnFocus="true" Width="128px" AutoPostBack="false">
                                            <Calendar FastNavigationStep="12" ShowRowHeaders="false">
                                            </Calendar>
                                            <DateInput ID="DateInput2" DateFormat="MM/dd/yyyy" runat="Server"
                                                LabelWidth="64px" Width="">
                                                <EmptyMessageStyle Resize="None"></EmptyMessageStyle>

                                                <ReadOnlyStyle Resize="None"></ReadOnlyStyle>

                                                <FocusedStyle Resize="None"></FocusedStyle>

                                                <DisabledStyle Resize="None"></DisabledStyle>

                                                <InvalidStyle Resize="None"></InvalidStyle>

                                                <HoveredStyle Resize="None"></HoveredStyle>

                                                <EnabledStyle Resize="None"></EnabledStyle>
                                            </DateInput>

                                            <DatePopupButton ImageUrl="" HoverImageUrl="" CssClass=""></DatePopupButton>
                                        </as:RadDatePicker>
                                    </div>

                                    <div class="filter-item">
                                        <asp:Button ID="uxBtnSubmit" runat="server" Text="Search" CssClass="btn btn-default"
                                            OnClientClick="return validateFilters();" OnClick="uxBtnSubmit_Click" />
                                    </div>
                                </td>

                            </tr>
                        </table>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-12 text-center" data-target=".report-filter-panel" data-toggle="collapse">
                    <span class="btn btn-link btn-report-filter">
                        <asp:Literal ID="Literal3" runat="server" Text=" FILTER"></asp:Literal>
                    </span>
                </div>
            </div>


        </div>

        <%-- Grid --%>
        <div class="row">
            <div class="col-md-12">
                <uc:UxExport ID="uxExport"
                    ShowExcel="true" ShowCSV="true" ShowPDF="false" ShowWord="false"
                    GridID="uxReportGrid"
                    GridTitle="Negative Option Report" runat="server"
                    OnNeedExportConfig="uxExport_OnNeedExportConfig"/>

                <as:ASGrid ID="uxReportGrid" runat="server" AutoGenerateColumns="false" ShowFooter="false"
                    GridLines="None" AllowPaging="True" ASPagingMethod="SPASingleMethod1" ShowPageTotal="false"
                    ShowReportTotal="false"
                    XOverFlowable="true" HeaderStyle-Width="100px" IsAutoExportTemplate="true" AllowSortFilterWhenExport="true"
                    CssClass="in" meta:resourcekey="uxReportGridResource1"
                    AllowSorting="true" AllowFilteringByColumn="true"
                    OnItemDataBound ="uxReportGrid_ItemDataBound"
                    OnSortCommand="uxReportGrid_SortCommand"
                    FilteringByColumnWithDataFieldAllow="true"
                    FilteringByColumnWithDataFieldConfigFile="App_Data\FilteringByColumnWithDataFieldConfig\NegativeOptionReportGridConfig.json"
                    BuildFilterExpressionWithSquareBrackets="true"
                    EnableBuildFilterExpressionEnhancement="true"                
                    EnableFilterItemsPersistence="true"
                    EnableSortItemsPersistence="true"
                    OnPreRender="uxReportGrid_PreRender"
                    visiblereporttotal="true"
                    visiblepagetotal="false"
                    OnDataSourceReady="uxReportGrid_DataSourceReady">
                    <MasterTableView>
                        <Columns>
                            <as:ASGridBoundColumn UniqueName="CardNumber" DataField="CardNumber" HeaderText="Card Number" HeaderTooltip="Card Number" ASFormat="StaticString" HeaderStyle-Width="130px"
                                HeaderStyle-CssClass="CardNumber" AllowFiltering="false" AllowSorting="false">
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" />
                            </as:ASGridBoundColumn>

                            <as:ASGridBoundColumn UniqueName="PartialCardNumber" DataField="PartialCardNumber" HeaderText="Card Number" HeaderTooltip="Card Number" ASFormat="StaticString" HeaderStyle-Width="130px"
                                HeaderStyle-CssClass="PartialCardNumber" AllowFiltering="false" AllowSorting="false">
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" />
                            </as:ASGridBoundColumn>                          

                            <as:ASGridBoundColumn UniqueName="TransactionCount" DataField="TransactionCount" HeaderText="Transaction Count" HeaderTooltip="Transaction Count"
                                ASFormat="Integer" DataType="System.Int16"
                                HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="130px"
                                ItemStyle-HorizontalAlign="Right">                               
                            </as:ASGridBoundColumn>

                            <as:ASGridBoundColumn UniqueName="MerchantCount" DataField="MerchantCount" HeaderText="Merchant Count" HeaderTooltip="Merchant Count"
                                ASFormat="Integer" DataType="System.Int16"
                                HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="130px"
                                ItemStyle-HorizontalAlign="Right">
                            </as:ASGridBoundColumn>

                            <as:ASGridTemplateColumn UniqueName="Detail" HeaderText="Detail" HeaderTooltip="Detail"
                                HeaderStyle-HorizontalAlign="Center" 
                                ItemStyle-HorizontalAlign="Center" AllowFiltering="false" AllowSorting="false">
                                <ItemTemplate>
                                    <a href="#">Details</a>
                                </ItemTemplate>
                            </as:ASGridTemplateColumn>

                        </Columns>
                    </MasterTableView>
                </as:ASGrid>
            </div>
        </div>

    </div>
    <as:RadCodeBlock ID="JavaScript" runat="server">
        <script type="text/javascript">
            var rm_MCF_NegativeOptionReport_uxReportDate = '<%=uxReportDate.ClientID%>';
            var rm_MCF_NegativeOptionReport_js_ReportedDate_Invalid = '<%= GetGlobalResourceObject("MessageManager", "ReportFilter_V1") %>';
            var rm_MCF_NegativeOptionReport_js_ReportedDate_GreaterToday = '<%= GetGlobalResourceObject("MessageManager", "ReportFilter_V10")%>';
            var uxReportGrid_ClientID = '<%=uxReportGrid.ClientID %>';
        </script>
        <script src="<%=ResolveUrl("~")%>res/js/risk_MCF/rm_MCF_NegativeOptionReport.js"></script>
    </as:RadCodeBlock>
</asp:Content>
