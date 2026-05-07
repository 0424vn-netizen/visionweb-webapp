<%@ Control Language="C#" AutoEventWireup="true" CodeFile="rm_MCF_NewRiskReportTransactionVolumeAnalysis.ascx.cs"
    Inherits="UserControls_rm_MCF_NewRiskReportTransactionVolumeAnalysis" %>
<%@ Register TagName="NewUxExport" Src="~/UserControls/rm_MCF_RiskReport_UxExport.ascx" TagPrefix="uc" %>
<tek:RadAjaxManagerProxy ID="RadAjaxManager1" runat="server">
    <AjaxSettings>
        <tek:AjaxSetting AjaxControlID="uxTransVolumeAnalysisDataGrid">
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="divTransVolumeAnalysis" LoadingPanelID="uxLoadingPanelCustom" />
            </UpdatedControls>
        </tek:AjaxSetting>
        <tek:AjaxSetting AjaxControlID="uxViewColumns">
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="divTransVolumeAnalysis" LoadingPanelID="uxLoadingPanelCustom" />
            </UpdatedControls>
        </tek:AjaxSetting>
        <tek:AjaxSetting AjaxControlID="btnRebind">
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="divTransVolumeAnalysis" LoadingPanelID="uxLoadingPanelCustom" />
            </UpdatedControls>
        </tek:AjaxSetting>
    </AjaxSettings>
</tek:RadAjaxManagerProxy>
<div class="row" id="transvolumne">
    <div class="col-md-10">
        <h2 class="grid-title control-inline" style="cursor: default;">
            <as:Literal ID="Literal5" runat="server" Text="Transaction Volume Analysis" meta:resourcekey="Literal15Resource1"></as:Literal>
        </h2>
        <span class="dark-blue">
            <asp:Label ID="lblContractExpected" meta:resourcekey="lblContractExpectedResource" runat="server" Text="Contract Expected"></asp:Label>
        </span>

        <asp:Label ID="iconContractExpectedInfo" CssClass="" runat="server">
<a class="image-link" href="#" style="cursor:pointer" onclick="return false;"><img src="/res/img/information.png" border="0" alt=""></a>
        </asp:Label>
    </div>
    <div class="col-md-2 text-right">
        <uc:NewUxExport ID="uxExportTransVolumeAnalysis" runat="server" GridID="uxTransVolumeAnalysisDataGrid" OnNeedExportConfig="uxExportTransVolumeAnalysis_NeedExportConfig"
            GridHeader="Transaction Volume Analysis Data" FileName="Risk Management - Merchant Information - Risk Report - Transaction Volume Analysis Data" GridTitle="Transaction Volume Analysis"
            ShowWord="false" ShowPDF="false" meta:resourcekey="uxExportTransVolumeAnalysisResourcekey" />
    </div>
</div>
<div id="divTransVolumeAnalysis" runat="server">
    <div class="row ipmt mt-5x mb-24">
        <div class="col-md-4">
            <span class="title control-inline narrow">
                <as:Literal ID="Literal9" runat="server" Text="View:" meta:resourcekey="lblViewResource"></as:Literal></span>
            <div class="inline-block">
                <as:RadComboBox ID="uxViewColumns" Filter="Contains" MarkFirstMatch="true" runat="server" AutoPostBack="true" OnSelectedIndexChanged="uxViewColumns_SelectedIndexChanged"
                    DataValueField="CustomViewID" DataTextField="ViewName" Width="200px">
                </as:RadComboBox>

            </div>
            <b class="dark-blue ml-4x">
                <asp:LinkButton runat="server" ID="uxCustomizeColumnLink" CssClass="link-back" Text="Customize" meta:resourcekey="Literal16Resource1"></asp:LinkButton></b>
        </div>
        <div style="display: none">
            <asp:Button runat="server" ID="btnRebind" OnClick="btnRebind_Click" />
        </div>
    </div>
    <div id="uxTransVolumeAnalysisGrid" class="in">
        <as:HiddenField ID="hddExportOption" runat="server" />
        <as:ASGrid ID="uxTransVolumeAnalysisDataGrid" runat="server" ShowPageTotal="false" AllowPaging="false" FooterStyle-CssClass="freeze-table-footer" InsertTempColumnAtTheEnd="false"
            AutoGenerateColumns="False" AllowSorting="False" ASPagingMethod="None" AllowExportAtWebServices="false" ShowHeader="true" ShowPager="true" OnPreRender="uxTransVolumeAnalysisDataGrid_PreRender"
            GridLines="None" GridName="Transaction Volume Analysis" OnNeedDataSource="uxTransVolumeAnalysisDataGrid_NeedDataSource" OnItemDataBound="uxTransVolumeAnalysisDataGrid_ItemDataBound"
            meta:resourcekey="uxGridTransVolumeAnalysisResource" IsAutoExportTemplate="true" IsCacheTemplateFile="false">
            <MasterTableView>
                <Columns>
                    <as:ASGridTemplateColumn HeaderText="Time" UniqueName="Time" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="160px" DataField="Time">
                        <HeaderTemplate>
                            <span id="ASGridBoundColumnResource37" title="Time"><%#GetLocalResourceObject("Time.HeaderText").ToString() %></span>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Literal ID="litTime" Text='<%# VeraCodeExtensions.DoVeraCode(System.Web.HttpUtility.HtmlEncode(Eval("Time").ToString())) %>' runat="server"></asp:Literal>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Center" Width="160px"></HeaderStyle>
                    </as:ASGridTemplateColumn>
                    <as:ASGridTemplateColumn HeaderText="Sales #" UniqueName="SalesCount" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="160px" DataField="SalesCount"
                        ASExportFormat="Integer" ASDefaultNullValue="N/A">
                        <HeaderTemplate>
                            <span id="ASGridBoundColumnResource38" title="Sales #"><%#GetLocalResourceObject("SalesCount.HeaderText").ToString() %></span>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Literal ID="litSalesCount" Text='<%# FormatContractForCount(Eval("SalesCount")) %>' runat="server"></asp:Literal>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Center" Width="80px"></HeaderStyle>
                    </as:ASGridTemplateColumn>
                    <as:ASGridTemplateColumn HeaderText="Net Volume" UniqueName="Volume" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="160px" DataField="Volume"
                        ASExportFormat="Currency" ASDefaultNullValue="N/A">
                        <HeaderTemplate>
                            <span id="ASGridBoundColumnResource39" title="Net Volume"><%#GetLocalResourceObject("Volume.HeaderText").ToString() %></span>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Literal ID="litVolume" Text='<%# FormatContract(Eval("Volume")) %>' runat="server"></asp:Literal>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Center" Width="100px"></HeaderStyle>
                    </as:ASGridTemplateColumn>
                    <as:ASGridTemplateColumn HeaderText="Avg Ticket" UniqueName="AVGTicket" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="160px" DataField="AVGTicket"
                        ASExportFormat="Currency" ASDefaultNullValue="N/A">
                        <HeaderTemplate>
                            <span id="ASGridBoundColumnResource40" title="Avg Ticket"><%#GetLocalResourceObject("AVGTicket.HeaderText").ToString() %></span>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Literal ID="litAVGTicket" Text='<%# FormatContract(Eval("AVGTicket")) %>' runat="server"></asp:Literal>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Center" Width="100px"></HeaderStyle>
                    </as:ASGridTemplateColumn>
                    <as:ASGridTemplateColumn HeaderText="Auth #" UniqueName="AuthCount" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="160px" DataField="AuthCount"
                        ASExportFormat="Integer" ASDefaultNullValue="N/A">
                        <HeaderTemplate>
                            <span id="ASGridBoundColumnResource41" title="Auth #"><%#GetLocalResourceObject("AuthCount.HeaderText").ToString() %></span>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Literal ID="litAuthCount" Text='<%#  FormatContractForCount(Eval("AuthCount")) %>' runat="server"></asp:Literal>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Center" Width="80px"></HeaderStyle>
                    </as:ASGridTemplateColumn>
                    <as:ASGridTemplateColumn HeaderText="Auth Appr %" UniqueName="AuthApprovalPercent" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="80px"
                        DataField="AuthApprovalPercent" ASExportFormat="Percentage" ASDefaultNullValue="N/A">
                        <HeaderTemplate>
                            <span id="ASGridBoundColumnResource42" title="Auth Appr %"><%#GetLocalResourceObject("AuthApprovalPercent.HeaderText").ToString() %></span>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Literal ID="litAuthApprovalPercent" Text='<%# MyPercentageConvert(Eval("AuthApprovalPercent")) %>' runat="server"></asp:Literal>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Center" Width="80px"></HeaderStyle>
                    </as:ASGridTemplateColumn>
                    <as:ASGridTemplateColumn HeaderText="Keyed %" UniqueName="KeyedPercent" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="100px"
                        DataField="KeyedPercent" ASExportFormat="Percentage" ASDefaultNullValue="N/A">
                        <HeaderTemplate>
                            <span id="ASGridBoundColumnResource43" title="Keyed %"><%#GetLocalResourceObject("KeyedPercent.HeaderText").ToString() %></span>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Literal ID="litKeyedPercent" Text='<%# MyPercentageConvert(Eval("KeyedPercent")) %>' runat="server"></asp:Literal>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Center" Width="100px"></HeaderStyle>
                    </as:ASGridTemplateColumn>
                    <as:ASGridTemplateColumn HeaderText="Returns" UniqueName="Returns" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="100px"
                        DataField="Returns" ASExportFormat="Currency" ASDefaultNullValue="N/A">
                        <HeaderTemplate>
                            <span id="ASGridBoundColumnResource44" title="Returns"><%#GetLocalResourceObject("Returns.HeaderText").ToString() %></span>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Literal ID="litReturns" Text='<%# FormatContract(Eval("Returns")) %>' runat="server"></asp:Literal>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Center" Width="100px"></HeaderStyle>
                    </as:ASGridTemplateColumn>
                    <as:ASGridTemplateColumn HeaderText="Returns %" UniqueName="ReturnPercent" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="100px"
                        DataField="ReturnPercent" ASExportFormat="Percentage" ASDefaultNullValue="N/A">
                        <HeaderTemplate>
                            <span id="ASGridBoundColumnResource45" title="Returns %"><%#GetLocalResourceObject("ReturnPercent.HeaderText").ToString() %></span>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Literal ID="litReturnPercent" Text='<%# MyPercentageConvert(Eval("ReturnPercent")) %>' runat="server"></asp:Literal>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Center" Width="100px"></HeaderStyle>
                    </as:ASGridTemplateColumn>
                    <as:ASGridTemplateColumn HeaderText="CB%" UniqueName="ChargeBackPercent" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="100px"
                        DataField="ChargeBackPercent" ASExportFormat="Percentage" ASDefaultNullValue="N/A">
                        <HeaderTemplate>
                            <span id="ASGridBoundColumnResource46" title="CB%"><%#GetLocalResourceObject("ChargeBackPercent.HeaderText").ToString() %></span>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Literal ID="litChargeBackPercent" Text='<%# MyPercentageConvert(Eval("ChargeBackPercent")) %>' runat="server"></asp:Literal>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Center" Width="100px"></HeaderStyle>
                    </as:ASGridTemplateColumn>
                    <as:ASGridTemplateColumn HeaderText="CB$" UniqueName="ChargeBackVolume" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="100px"
                        DataField="ChargeBackVolume" ASExportFormat="Currency" ASDefaultNullValue="N/A">
                        <HeaderTemplate>
                            <span id="ASGridBoundColumnResource47" title="CB$"><%#GetLocalResourceObject("ChargeBackVolume.HeaderText").ToString() %></span>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Literal ID="litChargeBackAmount" Text='<%# FormatContract(Eval("ChargeBackVolume")) %>' runat="server"></asp:Literal>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Center" Width="100px"></HeaderStyle>
                    </as:ASGridTemplateColumn>
                    <as:ASGridTemplateColumn HeaderText="Retrievals" UniqueName="Retrievals" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="100px"
                        DataField="Retrievals" ASExportFormat="Currency" ASDefaultNullValue="N/A">
                        <HeaderTemplate>
                            <span id="ASGridBoundColumnResource48" title="Retrievals"><%#GetLocalResourceObject("Retrievals.HeaderText").ToString() %></span>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Literal ID="litRetrievals" Text='<%# FormatContract(Eval("Retrievals")) %>' runat="server"></asp:Literal>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Center" Width="100px"></HeaderStyle>
                    </as:ASGridTemplateColumn>
                    <as:ASGridTemplateColumn HeaderText="Foreign" UniqueName="Foreign" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="100px"
                        DataField="Foreign" ASExportFormat="Currency" ASDefaultNullValue="N/A">
                        <HeaderTemplate>
                            <span id="ASGridBoundColumnResource49" title="Foreign"><%#GetLocalResourceObject("Foreign.HeaderText").ToString() %></span>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Literal ID="litForeign" Text='<%# FormatContract(Eval("Foreign")) %>' runat="server"></asp:Literal>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Center" Width="100px"></HeaderStyle>
                    </as:ASGridTemplateColumn>
                    <as:ASGridTemplateColumn HeaderText="ACH Returns" UniqueName="ACHRejects" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="100px"
                        DataField="ACHRejects" ASExportFormat="Currency" ASDefaultNullValue="N/A">
                        <HeaderTemplate>
                            <span id="ASGridBoundColumnResource50" title="ACH Returns"><%#GetLocalResourceObject("ACHRejects.HeaderText").ToString() %></span>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Literal ID="litACHRejects" Text='<%# FormatContract(Eval("ACHRejects")) %>' runat="server"></asp:Literal>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Center" Width="100px"></HeaderStyle>
                    </as:ASGridTemplateColumn>
                    <as:ASGridTemplateColumn HeaderText="Large Trans" UniqueName="LargeTrans" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="100px"
                        DataField="LargeTrans" ASExportFormat="Currency" ASDefaultNullValue="N/A">
                        <HeaderTemplate>
                            <span id="ASGridBoundColumnResource51" title="Large Trans"><%#GetLocalResourceObject("LargeTrans.HeaderText").ToString() %></span>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Literal ID="litLargeTrans" Text='<%# FormatContract(Eval("LargeTrans")) %>' runat="server"></asp:Literal>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Center" Width="100px"></HeaderStyle>
                    </as:ASGridTemplateColumn>
                    <as:ASGridTemplateColumn HeaderText="Returns #" UniqueName="ReturnCount" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="80px"
                        DataField="ReturnCount" ASExportFormat="Integer" ASDefaultNullValue="N/A">
                        <HeaderTemplate>
                            <span id="ASGridBoundColumnResource52" title="Return #"><%#GetLocalResourceObject("ReturnCount.HeaderText").ToString() %></span>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Literal ID="litReturnCount" Text='<%# FormatContractForCount(Eval("ReturnCount")) %>' runat="server"></asp:Literal>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Center" Width="80px"></HeaderStyle>
                    </as:ASGridTemplateColumn>
                    <as:ASGridTemplateColumn HeaderText="CB #" UniqueName="ChargeBackCount" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="80px"
                        DataField="ChargeBackCount" ASExportFormat="Integer" ASDefaultNullValue="N/A">
                        <HeaderTemplate>
                            <span id="ASGridBoundColumnResource53" title="CB #"><%#GetLocalResourceObject("ChargeBackCount.HeaderText").ToString() %></span>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Literal ID="litChargeBackCount" Text='<%# FormatContractForCount(Eval("ChargeBackCount")) %>' runat="server"></asp:Literal>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Center" Width="80px"></HeaderStyle>
                    </as:ASGridTemplateColumn>
                    <as:ASGridTemplateColumn HeaderText="Sales $" UniqueName="SaleAmount" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="100px"
                        DataField="SaleAmount" ASExportFormat="Currency" ASDefaultNullValue="N/A">
                        <HeaderTemplate>
                            <span id="ASGridBoundColumnResource54" title="Sales $"><%#GetLocalResourceObject("SaleAmount.HeaderText").ToString() %></span>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Literal ID="litSaleAmount" Text='<%# FormatContract(Eval("SaleAmount")) %>' runat="server"></asp:Literal>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Center" Width="100px"></HeaderStyle>
                    </as:ASGridTemplateColumn>
                    <as:ASGridTemplateColumn HeaderText="CB # %" UniqueName="ChargeBackCountPercent" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="100px"
                        DataField="ChargeBackCountPercent" ASExportFormat="Percentage" ASDefaultNullValue="N/A">
                        <HeaderTemplate>
                            <span id="ASGridBoundColumnResource55" title="CB # %"><%#GetLocalResourceObject("ChargeBackCountPercent.HeaderText").ToString() %></span>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Literal ID="litChargeBackCountPercent" Text='<%# MyPercentageConvert(Eval("ChargeBackCountPercent")) %>' runat="server"></asp:Literal>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Center" Width="100px"></HeaderStyle>
                    </as:ASGridTemplateColumn>
                    <as:ASGridTemplateColumn HeaderText="Returns # %" UniqueName="ReturnCountPercent" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="100px"
                        DataField="ReturnCountPercent" ASExportFormat="Percentage" ASDefaultNullValue="N/A">
                        <HeaderTemplate>
                            <span id="ASGridBoundColumnResource56" title="Returns # %"><%#GetLocalResourceObject("ReturnCountPercent.HeaderText").ToString() %></span>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Literal ID="litReturnCountPercent" Text='<%# MyPercentageConvert(Eval("ReturnCountPercent")) %>' runat="server"></asp:Literal>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Center" Width="100px"></HeaderStyle>
                    </as:ASGridTemplateColumn>
                    <as:ASGridTemplateColumn HeaderText="Foreign # %" UniqueName="ForeignCardCountPercent" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="100px"
                        DataField="ForeignCardCountPercent" ASExportFormat="Percentage" ASDefaultNullValue="N/A">
                        <HeaderTemplate>
                            <span id="ASGridBoundColumnResource57" title="Foreign # %"><%#GetLocalResourceObject("ForeignCardCountPercent.HeaderText").ToString() %></span>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Literal ID="litForeignCardCountPercent" Text='<%# MyPercentageConvert(Eval("ForeignCardCountPercent")) %>' runat="server"></asp:Literal>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Center" Width="100px"></HeaderStyle>
                    </as:ASGridTemplateColumn>
                    <as:ASGridTemplateColumn HeaderText="Auth Decl $" UniqueName="DeclinedAuthorizationAmount" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="100px"
                        DataField="DeclinedAuthorizationAmount" ASExportFormat="Currency" ASDefaultNullValue="N/A">
                        <HeaderTemplate>
                            <span id="ASGridBoundColumnResource58" title="Auth Decl $"><%#GetLocalResourceObject("DeclinedAuthorizationAmount.HeaderText").ToString() %></span>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Literal ID="litDeclinedAuthorizationAmount" Text='<%# FormatContract(Eval("DeclinedAuthorizationAmount")) %>' runat="server"></asp:Literal>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Center" Width="100px"></HeaderStyle>
                    </as:ASGridTemplateColumn>
                    <as:ASGridTemplateColumn HeaderText="Auth Decl #" UniqueName="DeclinedAuthorizationCount" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="100px"
                        DataField="DeclinedAuthorizationCount" ASExportFormat="Integer" ASDefaultNullValue="N/A">
                        <HeaderTemplate>
                            <span id="ASGridBoundColumnResource59" title="Auth Decl #"><%#GetLocalResourceObject("DeclinedAuthorizationCount.HeaderText").ToString() %></span>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Literal ID="litDeclinedAuthorizationCount" Text='<%# FormatContractForCount(Eval("DeclinedAuthorizationCount")) %>' runat="server"></asp:Literal>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Center" Width="100px"></HeaderStyle>
                    </as:ASGridTemplateColumn>
                    <as:ASGridTemplateColumn HeaderText="Auth Decl $ %" UniqueName="DeclinedAuthorizationAmountPercent" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="100px"
                        DataField="DeclinedAuthorizationAmountPercent" ASExportFormat="Percentage" ASDefaultNullValue="N/A">
                        <HeaderTemplate>
                            <span id="ASGridBoundColumnResource60" title="Auth Decl $ %"><%#GetLocalResourceObject("DeclinedAuthorizationAmountPercent.HeaderText").ToString() %></span>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Literal ID="litDeclinedAuthorizationAmountPercent" Text='<%# MyPercentageConvert(Eval("DeclinedAuthorizationAmountPercent")) %>' runat="server"></asp:Literal>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Center" Width="100px"></HeaderStyle>
                    </as:ASGridTemplateColumn>
                    <as:ASGridTemplateColumn HeaderText="Auth Decl # %" UniqueName="DeclinedAuthorizationCountPercent" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="100px"
                        DataField="DeclinedAuthorizationCountPercent" ASExportFormat="Percentage" ASDefaultNullValue="N/A">
                        <HeaderTemplate>
                            <span id="ASGridBoundColumnResource61" title="Auth Decl # %"><%#GetLocalResourceObject("DeclinedAuthorizationCountPercent.HeaderText").ToString() %></span>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Literal ID="litDeclinedAuthorizationCountPercent" Text='<%# MyPercentageConvert(Eval("DeclinedAuthorizationCountPercent")) %>' runat="server"></asp:Literal>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Center" Width="100px"></HeaderStyle>
                    </as:ASGridTemplateColumn>
                    <as:ASGridTemplateColumn HeaderText="Retrieval # %" UniqueName="RetrievalCountPercent" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="100px"
                        DataField="RetrievalCountPercent" ASExportFormat="Percentage" ASDefaultNullValue="N/A">
                        <HeaderTemplate>
                            <span id="ASGridBoundColumnResource62" title="Retrieval # %"><%#GetLocalResourceObject("RetrievalCountPercent.HeaderText").ToString() %></span>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Literal ID="litRetrievalCountPercent" Text='<%# MyPercentageConvert(Eval("RetrievalCountPercent")) %>' runat="server"></asp:Literal>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Center" Width="100px"></HeaderStyle>
                    </as:ASGridTemplateColumn>
                    <as:ASGridTemplateColumn HeaderText="Retrieval $ %" UniqueName="RetrievalAmountPercent" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="100px"
                        DataField="RetrievalAmountPercent" ASExportFormat="Percentage" ASDefaultNullValue="N/A">
                        <HeaderTemplate>
                            <span id="ASGridBoundColumnResource63" title="Retrieval $ %"><%#GetLocalResourceObject("RetrievalAmountPercent.HeaderText").ToString() %></span>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Literal ID="litRetrievalAmountPercent" Text='<%# MyPercentageConvert(Eval("RetrievalAmountPercent")) %>' runat="server"></asp:Literal>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Center" Width="100px"></HeaderStyle>
                    </as:ASGridTemplateColumn>
                    <as:ASGridTemplateColumn HeaderText="Retrieval #" UniqueName="FirstRetrievalCount" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="100px"
                        DataField="FirstRetrievalCount" ASExportFormat="Integer" ASDefaultNullValue="N/A">
                        <HeaderTemplate>
                            <span id="ASGridBoundColumnResource64" title="Retrieval #"><%#GetLocalResourceObject("FirstRetrievalCount.HeaderText").ToString() %></span>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Literal ID="litFirstRetrievalCount" Text='<%# FormatContractForCount(Eval("FirstRetrievalCount")) %>' runat="server"></asp:Literal>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Center" Width="100px"></HeaderStyle>
                    </as:ASGridTemplateColumn>
                    <as:ASGridTemplateColumn HeaderText="PrepaidCardSalesPercent" UniqueName="PrepaidCardSalesPercent" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="100px"
                        DataField="PrepaidCardSalesPercent" ASExportFormat="Percentage" ASDefaultNullValue="N/A">
                        <HeaderTemplate>
                            <span id="ASGridBoundColumnResource65" title="Retrieval $ %"><%#GetLocalResourceObject("PrepaidCardSalesPercent.HeaderText").ToString() %></span>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Literal ID="litPrepaidCardSalesPercent" Text='<%# MyPercentageConvert(Eval("PrepaidCardSalesPercent ")) %>' runat="server"></asp:Literal>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Center" Width="100px"></HeaderStyle>
                    </as:ASGridTemplateColumn>

                    <as:ASGridTemplateColumn HeaderText="RDR #" UniqueName="FirstChargebackRDRCount" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="100px"
                        DataField="FirstChargebackRDRCount" ASExportFormat="Integer" ASDefaultNullValue="N/A">
                        <HeaderTemplate>
                            <span id="ASGridBoundColumnResource66" title="RDR #"><%#GetLocalResourceObject("FirstChargebackRDRCount.HeaderText").ToString() %></span>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Literal ID="litFirstChargebackRDRCount" Text='<%# FormatContractForCount(Eval("FirstChargebackRDRCount")) %>' runat="server"></asp:Literal>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Center" Width="100px"></HeaderStyle>
                    </as:ASGridTemplateColumn>

                    <as:ASGridTemplateColumn HeaderText="RDR $" UniqueName="FirstChargebackRDRAmount" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="100px"
                        DataField="FirstChargebackRDRAmount" ASExportFormat="Currency" ASDefaultNullValue="N/A">
                        <HeaderTemplate>
                            <span id="ASGridBoundColumnResource67" title="RDR $"><%#GetLocalResourceObject("FirstChargebackRDRAmount.HeaderText").ToString() %></span>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Literal ID="litFirstChargebackRDRAmount" Text='<%# FormatContract(Eval("FirstChargebackRDRAmount")) %>' runat="server"></asp:Literal>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Center" Width="100px"></HeaderStyle>
                    </as:ASGridTemplateColumn>

                    <as:ASGridTemplateColumn HeaderText="Post RDR #" UniqueName="PostChargebackRDRCount" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="100px"
                        DataField="PostChargebackRDRCount" ASExportFormat="Integer" ASDefaultNullValue="N/A">
                        <HeaderTemplate>
                            <span id="ASGridBoundColumnResource68" title="RDR #"><%#GetLocalResourceObject("PostChargebackRDRCount.HeaderText").ToString() %></span>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Literal ID="litPostChargebackRDRCount" Text='<%# FormatContractForCount(Eval("PostChargebackRDRCount")) %>' runat="server"></asp:Literal>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Center" Width="100px"></HeaderStyle>
                    </as:ASGridTemplateColumn>

                    <as:ASGridTemplateColumn HeaderText="Post RDR $" UniqueName="PostChargebackRDRAmount" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="100px"
                        DataField="PostChargebackRDRAmount" ASExportFormat="Currency" ASDefaultNullValue="N/A">
                        <HeaderTemplate>
                            <span id="ASGridBoundColumnResource69" title="RDR $"><%#GetLocalResourceObject("PostChargebackRDRAmount.HeaderText").ToString() %></span>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Literal ID="litPostChargebackRDRAmount" Text='<%# FormatContract(Eval("PostChargebackRDRAmount")) %>' runat="server"></asp:Literal>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Center" Width="100px"></HeaderStyle>
                    </as:ASGridTemplateColumn>
                    <as:ASGridTemplateColumn HeaderText="Post RDR # %" UniqueName="PostChargebackRDRCountPercent" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="100px"
                        DataField="PostChargebackRDRCountPercent" ASExportFormat="Percentage" ASDefaultNullValue="N/A">
                        <HeaderTemplate>
                            <span id="ASGridBoundColumnResource70" title="RDR # %"><%#GetLocalResourceObject("PostChargebackRDRCountPercent.HeaderText").ToString() %></span>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Literal ID="litPostChargebackRDRCountPercent" Text='<%# MyPercentageConvert(Eval("PostChargebackRDRCountPercent")) %>' runat="server"></asp:Literal>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Center" Width="100px"></HeaderStyle>
                    </as:ASGridTemplateColumn>

                    <as:ASGridTemplateColumn HeaderText="Post RDR $ %" UniqueName="PostChargebackRDRAmountPercent" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="100px"
                        DataField="PostChargebackRDRAmountPercent" ASExportFormat="Percentage" ASDefaultNullValue="N/A">
                        <HeaderTemplate>
                            <span id="ASGridBoundColumnResource71" title="RDR $ %"><%#GetLocalResourceObject("PostChargebackRDRAmountPercent.HeaderText").ToString() %></span>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Literal ID="litPostChargebackRDRAmountPercent" Text='<%# MyPercentageConvert(Eval("PostChargebackRDRAmountPercent")) %>' runat="server"></asp:Literal>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Center" Width="100px"></HeaderStyle>
                    </as:ASGridTemplateColumn>
                </Columns>
            </MasterTableView>
        </as:ASGrid>
        <as:ASRadToolTip ID="tltTime" meta:resourcekey="ASGridBoundColumnResource37" RenderMode="Lightweight" Position="BottomCenter" RelativeTo="Element"
            TargetControlID="ASGridBoundColumnResource37" runat="server" IsClientID="true" CssClass="RadTooltip" AutoCloseDelay="0">
        </as:ASRadToolTip>

        <as:ASRadToolTip ID="tltSalesCount" meta:resourcekey="SalesCount" RenderMode="Lightweight" Position="BottomCenter" RelativeTo="Element"
            TargetControlID="ASGridBoundColumnResource38" runat="server" IsClientID="true" CssClass="RadTooltip" AutoCloseDelay="0">
        </as:ASRadToolTip>

        <as:ASRadToolTip ID="tltVolume" meta:resourcekey="ASGridBoundColumnResource39" RenderMode="Lightweight" Position="BottomCenter" RelativeTo="Element"
            TargetControlID="ASGridBoundColumnResource39" runat="server" IsClientID="true" CssClass="RadTooltip" AutoCloseDelay="0">
        </as:ASRadToolTip>

        <as:ASRadToolTip ID="tltAVGTicket" meta:resourcekey="ASGridBoundColumnResource40" RenderMode="Lightweight" Position="BottomCenter" RelativeTo="Element"
            TargetControlID="ASGridBoundColumnResource40" runat="server" IsClientID="true" RenderInPageRoot="true" CssClass="RadTooltip" AutoCloseDelay="0">
        </as:ASRadToolTip>

        <as:ASRadToolTip ID="tltAuthCount" meta:resourcekey="ASGridBoundColumnResource41" RenderMode="Lightweight" Position="BottomCenter" RelativeTo="Element"
            TargetControlID="ASGridBoundColumnResource41" runat="server" IsClientID="true" RenderInPageRoot="false" CssClass="RadTooltip" AutoCloseDelay="0">
        </as:ASRadToolTip>

        <as:ASRadToolTip ID="tltAuthApprovalPercent" meta:resourcekey="ASGridBoundColumnResource42" RenderMode="Lightweight" Position="BottomCenter" RelativeTo="Element"
            TargetControlID="ASGridBoundColumnResource42" runat="server" IsClientID="true" RenderInPageRoot="true" CssClass="RadTooltip" AutoCloseDelay="0">
        </as:ASRadToolTip>


        <as:ASRadToolTip ID="tltKeyedPercent" meta:resourcekey="ASGridBoundColumnResource43" RenderMode="Lightweight" Position="BottomCenter" RelativeTo="Element"
            TargetControlID="ASGridBoundColumnResource43" runat="server" IsClientID="true" RenderInPageRoot="true" CssClass="RadTooltip" AutoCloseDelay="0">
        </as:ASRadToolTip>

        <as:ASRadToolTip ID="tltReturns" meta:resourcekey="ASGridBoundColumnResource44" RenderMode="Lightweight" Position="BottomCenter" RelativeTo="Element"
            TargetControlID="ASGridBoundColumnResource44" runat="server" IsClientID="true" RenderInPageRoot="true" CssClass="RadTooltip" AutoCloseDelay="0">
        </as:ASRadToolTip>

        <as:ASRadToolTip ID="tltReturnPercent" meta:resourcekey="ASGridBoundColumnResource45" RenderMode="Lightweight" Position="BottomCenter" RelativeTo="Element"
            TargetControlID="ASGridBoundColumnResource45" runat="server" IsClientID="true" RenderInPageRoot="true" CssClass="RadTooltip" AutoCloseDelay="0">
        </as:ASRadToolTip>


        <as:ASRadToolTip ID="tltChargeBackPercent" meta:resourcekey="ASGridBoundColumnResource46" RenderMode="Lightweight" Position="BottomCenter" RelativeTo="Element"
            TargetControlID="ASGridBoundColumnResource46" runat="server" IsClientID="true" RenderInPageRoot="true" CssClass="RadTooltip" AutoCloseDelay="0">
        </as:ASRadToolTip>

        <as:ASRadToolTip ID="tltChargeBackVolume" meta:resourcekey="ASGridBoundColumnResource47" RenderMode="Lightweight" Position="BottomCenter" RelativeTo="Element"
            TargetControlID="ASGridBoundColumnResource47" runat="server" IsClientID="true" RenderInPageRoot="true" CssClass="RadTooltip" AutoCloseDelay="0">
        </as:ASRadToolTip>

        <as:ASRadToolTip ID="tltRetrievals" meta:resourcekey="ASGridBoundColumnResource48" RenderMode="Lightweight" Position="BottomCenter" RelativeTo="Element"
            TargetControlID="ASGridBoundColumnResource48" runat="server" IsClientID="true" RenderInPageRoot="true" CssClass="RadTooltip" AutoCloseDelay="0">
        </as:ASRadToolTip>

        <as:ASRadToolTip ID="tltForeign" meta:resourcekey="ASGridBoundColumnResource49" RenderMode="Lightweight" Position="BottomCenter" RelativeTo="Element"
            TargetControlID="ASGridBoundColumnResource49" runat="server" IsClientID="true" RenderInPageRoot="true" CssClass="RadTooltip" AutoCloseDelay="0">
        </as:ASRadToolTip>

        <as:ASRadToolTip ID="tltACHRejects" meta:resourcekey="ASGridBoundColumnResource50" RenderMode="Lightweight" Position="BottomCenter" RelativeTo="Element"
            TargetControlID="ASGridBoundColumnResource50" runat="server" IsClientID="true" RenderInPageRoot="true" CssClass="RadTooltip" AutoCloseDelay="0">
        </as:ASRadToolTip>

        <as:ASRadToolTip ID="tltLargeTrans" meta:resourcekey="ASGridBoundColumnResource51" RenderMode="Lightweight" Position="BottomCenter" RelativeTo="Element"
            TargetControlID="ASGridBoundColumnResource51" runat="server" IsClientID="true" RenderInPageRoot="true" CssClass="RadTooltip" AutoCloseDelay="0">
        </as:ASRadToolTip>

        <as:ASRadToolTip ID="tltReturnCount" meta:resourcekey="ASGridBoundColumnResource52" RenderMode="Lightweight" Position="BottomCenter" RelativeTo="Element"
            TargetControlID="ASGridBoundColumnResource52" runat="server" IsClientID="true" RenderInPageRoot="true" CssClass="RadTooltip" AutoCloseDelay="0">
        </as:ASRadToolTip>

        <as:ASRadToolTip ID="tltChargeBackCount" meta:resourcekey="ASGridBoundColumnResource53" RenderMode="Lightweight" Position="BottomCenter" RelativeTo="Element"
            TargetControlID="ASGridBoundColumnResource53" runat="server" IsClientID="true" RenderInPageRoot="true" CssClass="RadTooltip" AutoCloseDelay="0">
        </as:ASRadToolTip>

        <as:ASRadToolTip ID="tltSaleAmount" meta:resourcekey="ASGridBoundColumnResource54" RenderMode="Lightweight" Position="BottomCenter" RelativeTo="Element"
            TargetControlID="ASGridBoundColumnResource54" runat="server" IsClientID="true" RenderInPageRoot="true" CssClass="RadTooltip" AutoCloseDelay="0">
        </as:ASRadToolTip>

        <as:ASRadToolTip ID="tltChargeBackCountPercent" meta:resourcekey="ASGridBoundColumnResource55" RenderMode="Lightweight" Position="BottomCenter" RelativeTo="Element"
            TargetControlID="ASGridBoundColumnResource55" runat="server" IsClientID="true" RenderInPageRoot="true" CssClass="RadTooltip" AutoCloseDelay="0">
        </as:ASRadToolTip>

        <as:ASRadToolTip ID="tltReturnCountPercent" meta:resourcekey="ASGridBoundColumnResource56" RenderMode="Lightweight" Position="BottomCenter" RelativeTo="Element"
            TargetControlID="ASGridBoundColumnResource56" runat="server" IsClientID="true" RenderInPageRoot="true" CssClass="RadTooltip" AutoCloseDelay="0">
        </as:ASRadToolTip>

        <as:ASRadToolTip ID="tltForeignCardCountPercent" meta:resourcekey="ASGridBoundColumnResource57" RenderMode="Lightweight" Position="BottomCenter" RelativeTo="Element"
            TargetControlID="ASGridBoundColumnResource57" runat="server" IsClientID="true" RenderInPageRoot="true" CssClass="RadTooltip" AutoCloseDelay="0">
        </as:ASRadToolTip>

        <as:ASRadToolTip ID="tltDeclinedAuthorizationAmount" meta:resourcekey="ASGridBoundColumnResource58" RenderMode="Lightweight" Position="BottomCenter" RelativeTo="Element"
            TargetControlID="ASGridBoundColumnResource58" runat="server" IsClientID="true" RenderInPageRoot="true" CssClass="RadTooltip" AutoCloseDelay="0">
        </as:ASRadToolTip>


        <as:ASRadToolTip ID="tltDeclinedAuthorizationCount" meta:resourcekey="ASGridBoundColumnResource59" RenderMode="Lightweight" Position="BottomCenter" RelativeTo="Element"
            TargetControlID="ASGridBoundColumnResource59" runat="server" IsClientID="true" RenderInPageRoot="true" CssClass="RadTooltip" AutoCloseDelay="0">
        </as:ASRadToolTip>

        <as:ASRadToolTip ID="tltDeclinedAuthorizationAmountPercent" meta:resourcekey="ASGridBoundColumnResource60" RenderMode="Lightweight" Position="BottomCenter" RelativeTo="Element"
            TargetControlID="ASGridBoundColumnResource60" runat="server" IsClientID="true" RenderInPageRoot="true" CssClass="RadTooltip" AutoCloseDelay="0">
        </as:ASRadToolTip>

        <as:ASRadToolTip ID="tltDeclinedAuthorizationCountPercent" meta:resourcekey="ASGridBoundColumnResource61" RenderMode="Lightweight" Position="BottomCenter" RelativeTo="Element"
            TargetControlID="ASGridBoundColumnResource61" runat="server" IsClientID="true" RenderInPageRoot="true" CssClass="RadTooltip" AutoCloseDelay="0">
        </as:ASRadToolTip>

        <as:ASRadToolTip ID="tltRetrievalCountPercent" meta:resourcekey="ASGridBoundColumnResource62" RenderMode="Lightweight" Position="BottomCenter" RelativeTo="Element"
            TargetControlID="ASGridBoundColumnResource62" runat="server" IsClientID="true" RenderInPageRoot="true" CssClass="RadTooltip" AutoCloseDelay="0">
        </as:ASRadToolTip>

        <as:ASRadToolTip ID="tltRetrievalAmountPercent" meta:resourcekey="ASGridBoundColumnResource63" RenderMode="Lightweight" Position="BottomCenter" RelativeTo="Element"
            TargetControlID="ASGridBoundColumnResource63" runat="server" IsClientID="true" RenderInPageRoot="true" CssClass="RadTooltip" AutoCloseDelay="0">
        </as:ASRadToolTip>

        <as:ASRadToolTip ID="tltFirstRetrievalCount" meta:resourcekey="ASGridBoundColumnResource64" RenderMode="Lightweight" RelativeTo="Element" Position="BottomCenter"
            TargetControlID="ASGridBoundColumnResource64" runat="server" IsClientID="true" RenderInPageRoot="true" CssClass="RadTooltip" AutoCloseDelay="0">
        </as:ASRadToolTip>

        <as:ASRadToolTip ID="tltContractExpected" RenderMode="Lightweight" Position="BottomCenter" RelativeTo="Element"
            TargetControlID="iconContractExpectedInfo" runat="server" CssClass="RadTooltip" AutoCloseDelay="0">
        </as:ASRadToolTip>
        <as:ASRadToolTip ID="tltPrepaidCardSalesPercent" meta:resourcekey="ASGridBoundColumnResource65" RenderMode="Lightweight" Position="BottomCenter" RelativeTo="Element"
            TargetControlID="ASGridBoundColumnResource65" runat="server" IsClientID="true" RenderInPageRoot="true" CssClass="RadTooltip" AutoCloseDelay="0">
        </as:ASRadToolTip>
        <as:ASRadToolTip ID="tltFirstChargebackRDRCount" meta:resourcekey="ASGridBoundColumnResource66" RenderMode="Lightweight" Position="BottomCenter" RelativeTo="Element"
            TargetControlID="ASGridBoundColumnResource66" runat="server" IsClientID="true" RenderInPageRoot="true" CssClass="RadTooltip" AutoCloseDelay="0">
        </as:ASRadToolTip>
        <as:ASRadToolTip ID="tltFirstChargebackRDRAmount" meta:resourcekey="ASGridBoundColumnResource67" RenderMode="Lightweight" Position="BottomCenter" RelativeTo="Element"
            TargetControlID="ASGridBoundColumnResource67" runat="server" IsClientID="true" RenderInPageRoot="true" CssClass="RadTooltip" AutoCloseDelay="0">
        </as:ASRadToolTip>
        <as:ASRadToolTip ID="tltPostChargebackRDRCount" meta:resourcekey="ASGridBoundColumnResource68" RenderMode="Lightweight" Position="BottomCenter" RelativeTo="Element"
            TargetControlID="ASGridBoundColumnResource68" runat="server" IsClientID="true" RenderInPageRoot="true" CssClass="RadTooltip" AutoCloseDelay="0">
        </as:ASRadToolTip>
        <as:ASRadToolTip ID="tltPostChargebackRDRAmount" meta:resourcekey="ASGridBoundColumnResource69" RenderMode="Lightweight" Position="BottomCenter" RelativeTo="Element"
            TargetControlID="ASGridBoundColumnResource69" runat="server" IsClientID="true" RenderInPageRoot="true" CssClass="RadTooltip" AutoCloseDelay="0">
        </as:ASRadToolTip>
        <as:ASRadToolTip ID="tltPostChargebackRDRCountPercent" meta:resourcekey="ASGridBoundColumnResource70" RenderMode="Lightweight" Position="BottomCenter" RelativeTo="Element"
            TargetControlID="ASGridBoundColumnResource70" runat="server" IsClientID="true" RenderInPageRoot="true" CssClass="RadTooltip" AutoCloseDelay="0">
        </as:ASRadToolTip>
        <as:ASRadToolTip ID="tltPostChargebackRDRAmountPercent" meta:resourcekey="ASGridBoundColumnResource71" RenderMode="Lightweight" Position="BottomCenter" RelativeTo="Element"
            TargetControlID="ASGridBoundColumnResource71" runat="server" IsClientID="true" RenderInPageRoot="true" CssClass="RadTooltip" AutoCloseDelay="0">
        </as:ASRadToolTip>
    </div>
</div>
<as:RadCodeBlock ID="JavaScript" runat="server">
    <script type="text/javascript">
        var rm_RiskReport_TransactionVolumeAnalysis = '<%= uxTransVolumeAnalysisDataGrid.ClientID %>';
        var rm_Button_Rebind_ClientID = '<%= btnRebind.ClientID %>';
        var rm_MCF_NewRiskReportTransactionVolumeAnalysis_hddExportOption = "<%= hddExportOption.ClientID %>";
    </script>
    <script src="<% =ResolveUrl("~")%>res/js/risk_MCF/rm_MCF_NewRiskReportTransactionVolumeAnalysis.js"></script>
</as:RadCodeBlock>
