<%@ Control Language="C#" AutoEventWireup="true" CodeFile="rm_MCF_PauseMerchantAlert.ascx.cs" Inherits="UserControls_rm_MCF_PauseMerchantAlert" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
 
<table id="tbl-filter-date">
    <asp:Repeater ID="uxPauseMerchantAlertFilterRepeater" runat="server"
        OnItemDataBound="uxPauseMerchantAlertFilterRepeater_ItemDataBound"
        OnItemCommand="uxPauseMerchantAlertFilterRepeater_ItemCommand">
        <ItemTemplate>
            <tr class="Row pause-merchant-alert-row">
                <td class="text-left pl-0 pr-0 pt-0">
                    <table>
                        <tr>
                            <td style="padding-right: 9px; padding-left: 0;">
                                <asp:Label data-component="lbTitleDateRange" CssClass="whitespace-nowrap" ID="lbTitleDateRange" runat="server" Text="Date Range"></asp:Label>
                            </td>
                            <td class="pl-0">
                                <as:RadDatePicker ID="uxFromDate" runat="server">
                                    <Calendar FastNavigationStep="12" ShowRowHeaders="false" />
                                    <DateInput data-component="uxFromDate" ID="DateInput1" runat="server" onclick="ShowCalendar(this)" />                                    
                                </as:RadDatePicker>
                            </td>
                            <td>
                                <as:RadDatePicker ID="uxToDate" runat="server">
                                    <Calendar FastNavigationStep="12" ShowRowHeaders="false" />
                                    <DateInput data-component="uxToDate" ID="DateInput2" data-pause-merchant-alert-date="to" runat="server" onclick="ShowCalendar(this)" />
                                </as:RadDatePicker>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3" class="pl-0 pt-0" style="line-height: 1.4;">
                                <asp:Label data-component="lbErrorDateRange" ID="lbErrorDateRange" CssClass="fs-11 line-height-0" runat="server" Text="" ForeColor="Red"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>

                <td class="text-left pt-0">
                    <table>
                        <tr class="Row">
                            <td style="padding-top: 7px;padding-right: 3px;">
                                <asp:Label data-component="lbTitleMerchants" ID="lbTitleMerchants" runat="server" Text="Merchants"></asp:Label>
                            </td>
                            <td>
                                <telerik:RadMultiSelect runat="server" Filter="Contains" EnforceMinLength="true" AutoPostBack="false" 
                                    AutoClose="true" DataTextField="DataText" DataValueField="DataKey"
                                    Width="378px" ID="RadMultiSelect1" Placeholder="" Delay="400" MinLength="3"
                                    >
                                    <ItemTemplate>
                                        <div class="#: Disabled ? 'disabled-option' : '' #">
                                            #: DataText #
                                        </div>
                                    </ItemTemplate>
                                    <VirtualSettings ItemHeight="26" MapValueTo="dataItem" ValueMapper="pauseMerchantAsyncSelect_valueMapper" />
                                    <ClientEvents 
                                        onChange="pauseMerchantAsyncSelect_onChange" 
                                        OnDataBound="pauseMerchantAsyncSelect_onDataBound" 
                                        OnOpen="pauseMerchantAsyncSelect_onOpen" 
                                        onLoad="pauseMerchantAsyncSelect_onLoad"
                                        onDeselect="pauseMerchantAsyncSelect_onDeselect"
                                        onSelect="pauseMerchantAsyncSelect_onSelect" />
                                    <WebServiceClientDataSource EnableServerFiltering="true" AllowPaging="true" PageSize="31" EnableServerPaging="true">
                                        <ClientEvents 
                                            OnCustomParameter="pauseMerchantAsyncSelect_onCustomParameter" 
                                            OnDataParse="pauseMerchantAsyncSelect_onDataParse" />
                                        <WebServiceSettings ServiceType="Default" BaseUrl="/PauseMerchantAlert.aspx/">
                                            <Select Url="GetMerchants" RequestType="Post" DataType="JSON" ContentType="application/json; charset=utf-8" />
                                        </WebServiceSettings>
                                        <Schema DataName="Data" TotalName="Count" ResponseType="JSON">
                                            <Model>
                                                <telerik:ClientDataSourceModelField DataType="String" FieldName="DataText" />
                                                <telerik:ClientDataSourceModelField DataType="String" FieldName="DataKey" />
                                                <telerik:ClientDataSourceModelField DataType="Boolean" FieldName="Disabled" />
                                            </Model>
                                        </Schema>
                                    </WebServiceClientDataSource>
                                </telerik:RadMultiSelect>
                            </td>
                            <td style="padding-right: 8px;padding-top: 7px;">
                                <as:LinkButton runat="server" ID="btnDelete" class="remove-condition" CommandName="Delete"
                                    CommandArgument='<%# Eval("RowGUID") + ";" + Eval("AssignmentID")%>' Text="Remove"></as:LinkButton>
                            </td>
                        </tr>
                        <tr>
                            <td></td>
                            <td colspan="2" style="padding-top: 0;">
                                <div style="line-height: 1.5;">
                                    <as:Literal ID="ltMerchants" runat="server" Text="Enter at least 3 characters of the Merchant Name or Merchant ID"></as:Literal>
                                </div>
                                <div style="line-height: 1.4;">
                                    <asp:Label data-component="lbErrorMerchants" ID="lbErrorMerchants" CssClass="fs-11 line-height-0" runat="server" Text="" ForeColor="Red"></asp:Label>
                                </div>
                            </td>
                        </tr>
                    </table>
                </td>
                <as:HiddenField ID="hddRowGuid" runat="server" />
                <as:HiddenField ID="hddMerchantsSelected" runat="server" />                
            </tr>
        </ItemTemplate>
    </asp:Repeater>
</table>
<as:HiddenField ID="hddIsViewMode" runat="server" />
<style>
    @import url('<%= ResolveUrl("~/")%>res/css/pauseMerchantAlert/pauseMerchantAlert.css');

    tr.Row {
        vertical-align: top
    }

    .RadPicker_Default {
        width: 116px !important;
    }
</style>
<as:ASRadCodeBlock ID="ASRadCodeBlock1" runat="server">
    <script type="text/javascript">
        var FeatureMode = '<%= FeatureMode %>';
        var Risk_Assignment_Filters_hddIsViewMode_ClientID = '<%= hddIsViewMode.ClientID %>';
    </script>
    <script src="<%= ResolveUrl("~/")%>res/js/risk_MCF/rm_MCF_PauseMerchantAlertAsyncSelect.js"></script>
    <script src="<%= ResolveUrl("~/")%>res/js/risk_MCF/rm_MCF_PauseMerchantAlert.js"></script>
</as:ASRadCodeBlock>
 