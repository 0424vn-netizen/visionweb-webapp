<%@ Page Title="RET/CB Report" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="rm_MCF_RetCb.aspx.cs" Inherits="rm_MCF_RetCb" meta:resourcekey="PageResource1" %>

<%@ Register Src="~/UserControls/PageTitle.ascx" TagName="PageTitle" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/ReportFiltering.ascx" TagName="ReportFiltering"
    TagPrefix="uc" %>
<%@ Register Src="~/UserControls/UxExport.ascx" TagName="UxExport" TagPrefix="uc" %>
<%@ Register TagName="SiteID_Selector" Src="~/UserControls/SiteID_Selector.ascx"
    TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPage" runat="Server">

    <tek:RadAjaxManagerProxy runat="server" ID="uxRadAjaxManager">
        <AjaxSettings>

            <tek:AjaxSetting AjaxControlID="uxReportGrid">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="uxReportGrid" LoadingPanelID="uxLoadingPanelCustom" />
                </UpdatedControls>
            </tek:AjaxSetting>

            <tek:AjaxSetting AjaxControlID="uxChangeMerchantWorked">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="uxChangeMerchantWorked" />
                    <%--<tek:AjaxUpdatedControl ControlID="uxReportGrid" />      --%>
                </UpdatedControls>
            </tek:AjaxSetting>
            <tek:AjaxSetting AjaxControlID="uxChangeWorkType">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="uxReportGrid" LoadingPanelID="uxLoadingPanelCustom" />
                    <tek:AjaxUpdatedControl ControlID="uxExporter" />
                </UpdatedControls>
            </tek:AjaxSetting>
        </AjaxSettings>
    </tek:RadAjaxManagerProxy>
    <as:PlaceHolder ID="dgj" runat="server">
        <uc:ReportFiltering ID="uxReportFiltering" runat="server" OnFiltering="uxReportFiltering_Filtering" />
        <div class="row">
            <div class="col-md-12 no-margin-bottom">
                <uc:PageTitle ID="uxPageTitle" runat="server" ReportTitle="Retrievals/Chargebacks" meta:resourcekey="uxPageTitleResource1" />
            </div>
        </div>
        <as:Button ID="uxChangeWorkType" runat="server" IsStandardButton="true" OnClick="uxChangeWorkType_Click"
            CssClass="display-none" meta:resourcekey="uxChangeWorkTypeResource1"></as:Button>
        <div class="height-16"></div>
        <div class="row">
            <div class="col-md-12 dark-blue">
                <label class="first">
                    <asp:Literal ID="LiteralWorkStatus" runat="server" meta:resourcekey="LiteralWorkStatusResource1"> Reviewe Status:</asp:Literal></label>
                <div class="control-inline">
                    <as:RadioButton ID="uxRadNotReviewed" runat="server" Text="Not Reviewed" xValue="1" GroupName="grpReviewStatus"
                        onclick="ChangeReviewedType_Click('1')" meta:resourcekey="uxRadNotWorkedResource1" />
                </div>
                <div class="control-inline">
                    <as:RadioButton ID="uxRadReviewed" runat="server" Text="Reviewed" xValue="2" GroupName="grpReviewStatus"
                        onclick="ChangeReviewedType_Click('2')" meta:resourcekey="uxRadWorkedResource1" />
                </div>
                <div class="control-inline">
                    <as:RadioButton ID="uxRadAll" runat="server" Text="All" xValue="0" Checked="true"
                        GroupName="grpReviewStatus" onclick="ChangeReviewedType_Click('0')" meta:resourcekey="uxRadAllResource1" />
                </div>
            </div>
        </div>
        <div class="height-12"></div>
    </as:PlaceHolder>
    <as:PlaceHolder ID="uxPlaceHolder" runat="server">
        <uc:UxExport ID="uxExporter" runat="server" GridID="uxReportGrid" IsOnTop="true" ShowPDF="false" />
        <as:ASGrid ID="uxReportGrid" runat="server" AllowPaging="true" AllowSorting="True" IsAutoExportTemplate="true"
            AutoGenerateColumns="False" ASPagingMethod="SPASingleMethod1" ShowPageTotal="false" AllowExportAtWebServices="true"
            GridName="Retrievals/Chargebacks" GridLines="None" OnInit="OnInitUxReportGrid" Width="100%" XOverFlowable="true"
            HeaderStyle-Width="100px" CssClass="in" meta:resourcekey="uxReportGridResource1">
            <MasterTableView>
                <Columns>
                    <as:ASGridTemplateColumn HeaderText=" " UniqueName="IsReviewed" ItemStyle-HorizontalAlign="Center" meta:resourcekey="ASGridTemplateColumnResource1">
                        <ItemTemplate>
                            <span class="checkbox-middle">
                                <input runat="server" type="checkbox" class="workedBox" id="chkMerchantReviewed" value='<%# Eval("MerchantNumber") %>'
                                    onclick="return ChangeMerchantReviewed_Click(this);" checked='<%# Convert.ToBoolean(Eval("IsReviewed")) %>' />
                                &nbsp;</span>
                        </ItemTemplate>
                        <HeaderStyle Width="40px" />

                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </as:ASGridTemplateColumn>
                    <as:ASGridBoundColumn HeaderText="Reviewed" DataField="ReviewedText" UniqueName="ReviewedText"
                        ASFormat="StaticString" Visible="false" meta:resourcekey="ASGridBoundColumnResource1">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="MerchantName" UniqueName="MerchantName" HeaderText="Merchant Name"
                        HeaderTooltip="Merchant Name" SortExpression="MerchantName"
                        ItemStyle-CssClass="merchantName" ASFormat="DynamicString" HeaderStyle-Width="210px" meta:resourcekey="ASGridBoundColumnResource2">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center" Width="210px"></HeaderStyle>

                        <ItemStyle CssClass="merchantName"></ItemStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="MerchantNumber" UniqueName="MerchantNumber" HeaderText="Merchant ID" HeaderTooltip="Merchant ID"
                        SortExpression="MerchantNumber" ASFormat="StaticString" HeaderStyle-Width="140px" meta:resourcekey="ASGridBoundColumnResource3">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center" Width="140px"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="ApprovalDate" UniqueName="ApprovalDate" HeaderText="Approval Date"
                        HeaderTooltip="Approval Date" HeaderStyle-Width="100px" Visible="false" ASFormat="Date" meta:resourcekey="ASGridBoundColumnResource4">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center" Width="100px"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="RetrievalCount" UniqueName="RetrievalCount" HeaderText="# Ret"
                        HeaderTooltip="Retrievals Transaction Count" SortExpression="RetrievalCount"
                        ASFormat="Integer" ASIsTotalColumn="true" HeaderStyle-Width="60px" meta:resourcekey="ASGridBoundColumnResource5">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center" Width="60px"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="RetrievalAmount" UniqueName="RetrievalAmount" HeaderText="Ret"
                        ASIsTotalColumn="true" ASFormat="Currency" HeaderTooltip="Retrievals Amount"
                        SortExpression="RetrievalAmount" meta:resourcekey="ASGridBoundColumnResource6">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="RepresentedCBCount" UniqueName="RepresentedCBCount"
                        HeaderText="# Rep CB" HeaderTooltip="Represented Chargebacks Transaction Count"
                        SortExpression="RepresentedCBCount" ASFormat="Integer" ASIsTotalColumn="true" HeaderStyle-Width="65px" meta:resourcekey="ASGridBoundColumnResource7">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center" Width="65px"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="ChargebackCount" UniqueName="ChargebackCount" HeaderText="# 1st CB"
                        HeaderTooltip="1st Chargebacks Transaction Count" SortExpression="ChargebackCount"
                        ASFormat="Integer" ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource8">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="ChargebackAmount" UniqueName="ChargebackAmount"
                        HeaderText="1st CB" ASIsTotalColumn="true" HeaderTooltip="1st Chargebacks Amount"
                        SortExpression="ChargebackAmount" ASFormat="Currency" meta:resourcekey="ASGridBoundColumnResource9">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>

                    <as:ASGridBoundColumn UniqueName="FirstChargebackRDRCount" HeaderText="RDR #"
                        DataField="FirstChargebackRDRCount" HeaderTooltip="RDR Count" SortExpression="FirstChargebackRDRCount" Visible="false"
                        ASFormat="Integer" ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource19">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>

                    <as:ASGridBoundColumn HeaderText="RDR $" DataField="FirstChargebackRDRAmount" UniqueName="FirstChargebackRDRAmount"
                        HeaderTooltip="RDR Amount" SortExpression="FirstChargebackRDRAmount" ASFormat="Currency" Visible="false"
                        ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource20">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>

                    <as:ASGridBoundColumn UniqueName="PostChargebackRDRCount" HeaderText="RDR #"
                        DataField="PostChargebackRDRCount" HeaderTooltip="Post RDR Count" SortExpression="PostChargebackRDRCount" Visible="false"
                        ASFormat="Integer" ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource21">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>

                    <as:ASGridBoundColumn HeaderText="Post RDR $" DataField="PostChargebackRDRAmount" UniqueName="PostChargebackRDRAmount"
                        HeaderTooltip="RDR Amount" SortExpression="PostChargebackRDRAmount" ASFormat="Currency" Visible="false"
                        ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource22">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>

                    <%--30 Day--%>
                    <as:ASGridBoundColumn DataField="ThirtyDaysVolume" UniqueName="NetAmount30Day" HeaderText="Net"
                        HeaderTooltip="Net Amount" ASFormat="Currency" ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource10">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="ThirtyDaysChargebackAmount" UniqueName="CBAmount30Day"
                        HeaderText="CB" HeaderTooltip="Chargebacks Amount" ASFormat="Currency" ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource11">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="ThirtyDaysChargebackPercent" UniqueName="ChargebackPercentOfSales30Day"
                        HeaderText="CB%" HeaderTooltip="Chargebacks % of Sales" ASFormat="Percentage"
                        ASIsTotalColumn="true" HeaderStyle-Width="60px" meta:resourcekey="ASGridBoundColumnResource12" ASTotalFormat="Percentage">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center" Width="60px"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn UniqueName="ThirtyDaysFirstChargebackRDRCount" HeaderText="RDR #"
                        DataField="ThirtyDaysFirstChargebackRDRCount" HeaderTooltip="RDR Count" Visible="false"
                        ASFormat="Integer" ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource19">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>

                    <as:ASGridBoundColumn HeaderText="RDR $" DataField="ThirtyDaysFirstChargebackRDRAmount" UniqueName="ThirtyDaysFirstChargebackRDRAmount"
                        HeaderTooltip="RDR Amount" SortExpression="ThirtyDaysFirstChargebackRDRAmount" ASFormat="Currency" Visible="false"
                        ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource20">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>

                    <as:ASGridBoundColumn UniqueName="ThirtyDaysPostChargebackRDRCount" HeaderText="RDR #"
                        DataField="ThirtyDaysPostChargebackRDRCount" HeaderTooltip="Post RDR Count" SortExpression="ThirtyDaysPostChargebackRDRCount" Visible="false"
                        ASFormat="Integer" ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource21">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>

                    <as:ASGridBoundColumn HeaderText="Post RDR $" DataField="ThirtyDaysPostChargebackRDRAmount" UniqueName="ThirtyDaysPostChargebackRDRAmount"
                        HeaderTooltip="RDR Amount" SortExpression="ThirtyDaysPostChargebackRDRAmount" ASFormat="Currency" Visible="false"
                        ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource22">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <%--90 Day--%>
                    <as:ASGridBoundColumn DataField="NinetyDaysVolume" UniqueName="NetAmount90Day" HeaderText="Net"
                        HeaderTooltip="Net Amount" ASFormat="Currency" ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource13">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="NinetyDaysChargebackAmount" UniqueName="CBAmount90Day"
                        HeaderText="CB" HeaderTooltip="Chargebacks Amount" ASFormat="Currency" ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource14">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="NinetyDaysChargebackPercent" UniqueName="ChargebackPercentOfSales90Day"
                        HeaderText="CB%" HeaderTooltip="Chargebacks % of Sales" ASFormat="Percentage"
                        ASIsTotalColumn="true" HeaderStyle-Width="60px" meta:resourcekey="ASGridBoundColumnResource15" ASTotalFormat="Percentage">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center" Width="60px"></HeaderStyle>
                    </as:ASGridBoundColumn>

                    <as:ASGridBoundColumn UniqueName="NinetyDaysFirstChargebackRDRCount" HeaderText="RDR #"
                        DataField="NinetyDaysFirstChargebackRDRCount" HeaderTooltip="RDR Count" SortExpression="NinetyDaysFirstChargebackRDRCount" Visible="false"
                        ASFormat="Integer" ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource19">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>

                    <as:ASGridBoundColumn HeaderText="RDR $" DataField="NinetyDaysFirstChargebackRDRAmount" UniqueName="NinetyDaysFirstChargebackRDRAmount"
                        HeaderTooltip="RDR Amount" SortExpression="NinetyDaysFirstChargebackRDRAmount" ASFormat="Currency" Visible="false"
                        ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource20">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>

                    <as:ASGridBoundColumn UniqueName="NinetyDaysPostChargebackRDRCount" HeaderText="RDR #"
                        DataField="NinetyDaysPostChargebackRDRCount" HeaderTooltip="Post RDR Count" SortExpression="NinetyDaysPostChargebackRDRCount" Visible="false"
                        ASFormat="Integer" ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource21">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>

                    <as:ASGridBoundColumn HeaderText="Post RDR $" DataField="NinetyDaysPostChargebackRDRAmount" UniqueName="NinetyDaysPostChargebackRDRAmount"
                        HeaderTooltip="RDR Amount" SortExpression="NinetyDaysPostChargebackRDRAmount" ASFormat="Currency" Visible="false"
                        ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource22">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <%--Last Batch--%>
                    <as:ASGridBoundColumn DataField="ReportDate" UniqueName="Date" HeaderText="Date"
                        HeaderTooltip="Report Date" ASFormat="Date" HeaderStyle-Width="90px" meta:resourcekey="ASGridBoundColumnResource16">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center" Width="90px"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="Amount" UniqueName="Amount" HeaderText="Amount"
                        ASIsTotalColumn="true" HeaderTooltip="Amount" ASFormat="Currency" meta:resourcekey="ASGridBoundColumnResource17">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn UniqueName="Detail" HeaderText="Detail" HeaderTooltip="Retrieval/Chargeback Details"
                        ASFormat="StaticString" HeaderStyle-Width="100px" meta:resourcekey="ASGridBoundColumnResource18">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                </Columns>
            </MasterTableView>

            <HeaderStyle Width="100px"></HeaderStyle>

        </as:ASGrid>

    </as:PlaceHolder>
    <as:HiddenField ID="uxHiddenMerchantWorked" runat="server" />
    <as:HiddenField ID="uxHiddenWorkedType" runat="server" Value="0" />
    <as:HiddenField ID="uxHiddenItems" runat="server" />
    <as:HiddenField ID="uxHiddenRefreshHF" runat="server" Value="1" />
    <as:RadCodeBlock ID="uxRadCodeBlock" runat="server">
        <script type="text/javascript">
            var rm_RetCb_uxHiddenWorkedType = "<%=uxHiddenWorkedType.ClientID %>";
            var rm_RetCb_uxChangeWorkType = "<%=uxChangeWorkType.ClientID %>";
            var rm_RetCb_uxHiddenMerchantWorked = "<%=uxHiddenMerchantWorked.ClientID %>";
            var rm_RetCb_uxReportGrid = "<%=uxReportGrid.ClientID %>";

            var rm_RetCb_FilterTextboxElementClientID = "<%= ExtendedHierarchyID %>";
            var rm_RetCb_RefreshIncludeExclude = "";
            var rm_RetCb_HierarchyFilterText = "<%= ExtendHierarchyFilterText %>";
            var rm_RetCb_FilterExtendCmd = "<%= EditIncludeExcludeItems %>";
            var rm_RetCb_FilterExtendVM = "<%= ViewMoreIncludeExcludeItems %>";
            var rm_RetCb_NAValue = "<%= NAValue %>";
            var rm_RetCb_uxHiddenItems = "<%= uxHiddenItems.ClientID %>";
            var rm_RetCb_uxHiddenCurrentHF = "<%= uxHiddenRefreshHF.ClientID %>";

            var rm_RetCb_js_30Day = '<%= GetLocalResourceObject("rm_RetCb_js_30Day").ToString() %>';
            var rm_RetCb_js_90Day = '<%= GetLocalResourceObject("rm_RetCb_js_90Day").ToString() %>';
            var rm_RetCb_js_LastBatch = '<%= GetLocalResourceObject("rm_RetCb_js_LastBatch").ToString() %>';
            var rm_RetCb_URL = '<%=ResolveUrl("~/risk_MCF/rm_MCF_RetCb.aspx") %>';
            var rm_BeginDate = '<% = BeginDate.Ticks %>';
            var rm_EndDate = '<% = EndDate.Ticks %>';
            var uxRadNotReviewed_ClientId = '<% = uxRadNotReviewed.ClientID%>';
            var uxRadReviewed_ClientId = '<% = uxRadReviewed.ClientID%>';
            var currentCliendId = '<%= SessionManager.CurrentClient.ToString() %>';
            var show_RDR = '<%= GeneralFuncsLib.GetDataOfExtendedSetting("SHOW_RDR").ToLower() %>';

        </script>
        <script type="text/javascript" src="<%=ResolveUrl("~") %>res/js/risk_MCF/rm_MCF_RetCb.js"></script>
    </as:RadCodeBlock>
</asp:Content>
