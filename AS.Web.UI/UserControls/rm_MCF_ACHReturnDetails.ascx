<%@ Control Language="C#" AutoEventWireup="true" CodeFile="rm_MCF_ACHReturnDetails.ascx.cs" Inherits="UserControls_rm_MCF_ACHReturnDetails" %>
<%@ Register TagName="NewUxExport" Src="~/UserControls/rm_MCF_Report_UxExport.ascx" TagPrefix="uc" %>
<tek:RadAjaxManagerProxy ID="RadAjaxManager1" runat="server">
    <AjaxSettings>
        <tek:AjaxSetting AjaxControlID="uxACHReturnDetailRange">
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="uxACHReturnDetailGrid" LoadingPanelID="uxLoadingPanelCustom" />
                <tek:AjaxUpdatedControl ControlID="uxExportACHReturnDetail" LoadingPanelID="uxLoadingPanelCustom" />
            </UpdatedControls>
        </tek:AjaxSetting>
        <tek:AjaxSetting AjaxControlID="uxACHReturnDetailGrid">
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="uxExportACHReturnDetail" LoadingPanelID="uxLoadingPanelCustom" />
                <tek:AjaxUpdatedControl ControlID="uxACHReturnDetailGrid" LoadingPanelID="uxLoadingPanelCustom" />
            </UpdatedControls>
        </tek:AjaxSetting>
    </AjaxSettings>
</tek:RadAjaxManagerProxy>
<div class="row">
    <div class="col-md-10">
        <h2 class="grid-title">
            <span data-toggle="collapse" data-target="#achReturnDetail">
                <as:Literal ID="Literal1" runat="server" Text="ACH Return Detail" meta:resourcekey="LiteralResource1"></as:Literal>
            </span>
        </h2>
    </div>
    <div class="col-md-2 text-right">
        <uc:NewUxExport ID="uxExportACHReturnDetail" runat="server" GridID="uxACHReturnDetailGrid" OnNeedExportConfig="uxExportACHReturnDetail_NeedExportConfig"
            GridHeader="ACH Return Detail" FileName="Risk Management - Merchant Information - Risk Report - ACH Return Detail" GridTitle="ACH Return Detail" meta:resourcekey="uxACHReturnDetailResource"
            ShowWord="false" ShowPDF="false"/>
    </div>
</div>

<div id="batchhistory" class="in">
    <div class="text-right mt-2x">
        <div class="control-inline mt-1x">
            <as:Literal ID="Literal2" runat="server" Text="Days:" meta:resourcekey="LiteralResource2" ></as:Literal>
        </div>
        <div class="pull-right mb-3x ">
            <tek:RadComboBox ID="uxACHReturnDetailRange" runat="server" OnSelectedIndexChanged="uxACHReturnDetailRange_SelectedIndexChanged"
                MarkFirstMatch="true" EnableEmbeddedBaseStylesheet="false" Width="60px" AutoPostBack="true">
            </tek:RadComboBox>
        </div> 
    </div>
    <as:ASGrid ID="uxACHReturnDetailGrid" runat="server" AutoGenerateColumns="False" ASPagingMethod="SPASingleMethod1" meta:resourcekey="uxACHReturnDetailGridResource"
        AllowSorting="true" AllowPaging="true" GridLines="None" IsIntruder="true" IntruderSourceName="uxACHReturnDetailGrid"
        GridName="Risk Management - Merchant Information - Risk Report - ACH Return Detail" OnNeedDataSource="uxACHReturnDetailGrid_NeedDataSource"
        ShowPageTotal="False" XOverFlowable="true" HeaderStyle-Width="100px" CssClass="in" IsAutoExportTemplate="true">
        <MasterTableView>
            <Columns>
                <as:ASGridBoundColumn HeaderText="Report Date" DataField="ReportDate" UniqueName="ReportDate"
                    HeaderTooltip="Report Date" ASFormat="Date" HeaderStyle-Width="100px" meta:resourcekey="ASGridBoundColumnResource1">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center" Width="100px"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn HeaderText="ACH Date" DataField="ACHDate" UniqueName="ACHDate"
                    HeaderTooltip="ACH Date" ASFormat="Date" HeaderStyle-Width="100px" meta:resourcekey="ASGridBoundColumnResource2">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center" Width="100px"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn HeaderText="Return Amount" DataField="ReturnAmount" UniqueName="ReturnAmount" ASFormat="Currency"
                    HeaderTooltip="Return Amount" HeaderStyle-Width="80px" meta:resourcekey="ASGridBoundColumnResource3">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center" Width="80px"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn HeaderText="Return Code" DataField="ReturnCode" UniqueName="ReturnCode"
                    HeaderTooltip="Return Code" ASFormat="StaticString" HeaderStyle-Width="80px" meta:resourcekey="ASGridBoundColumnResource4">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center" Width="80px"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn HeaderText="Return Reason" DataField="ReturnReason" UniqueName="ReturnReason"
                    HeaderTooltip="Return Reason" ASFormat="DynamicString" HeaderStyle-Width="100px" meta:resourcekey="ASGridBoundColumnResource5">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center" Width="100px"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn HeaderText="Routing #" DataField="RoutingNumber" meta:resourcekey="ASGridBoundColumnResource6"
                    UniqueName="RoutingNumber" HeaderTooltip="Routing Number" ASFormat="StaticString" ItemStyle-HorizontalAlign="Center">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn HeaderText="Routing #" DataField="PartialRoutingNumber" meta:resourcekey="ASGridBoundColumnResource6"
                    UniqueName="PartialRoutingNumber" HeaderTooltip="Routing Number" ASFormat="StaticString" Visible="false">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn HeaderText="DDA #" DataField="DDANumber" meta:resourcekey="ASGridBoundColumnResource7"
                    UniqueName="DDANumber" HeaderTooltip="DDA #" ASFormat="StaticString">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn HeaderText="DDA #" DataField="PartialDDANumber" meta:resourcekey="ASGridBoundColumnResource7"
                    UniqueName="PartialDDANumber" HeaderTooltip="DDA #" ASFormat="StaticString" Visible="false">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
            </Columns>
        </MasterTableView>

        <HeaderStyle Width="100px"></HeaderStyle>

    </as:ASGrid>
</div>
