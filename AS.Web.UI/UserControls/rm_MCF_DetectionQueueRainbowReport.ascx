<%@ Control Language="C#" AutoEventWireup="true" CodeFile="rm_MCF_DetectionQueueRainbowReport.ascx.cs"
    Inherits="UserControls_rm_MCF_DetectionQueueRainbowReport" %>

<%@ Register TagPrefix="uc" TagName="UxExport" Src="~/UserControls/rm_MCF_UxDetectionQueueExport.ascx" %>
<%@ Register TagName="DetectionQueueAssignmentList" Src="~/UserControls/rm_MCF_DetectionQueueAssignmentList.ascx" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/rm_MCF_CustomView.ascx" TagName="CustomView" TagPrefix="uc" %>

<as:RadAjaxManagerProxy ID="RadAjaxManager" runat="server">
    <AjaxSettings>
        <tek:AjaxSetting AjaxControlID="uxReportGrid">
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="uxReportGrid" />
            </UpdatedControls>
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="optGrid" UpdatePanelRenderMode="Inline" LoadingPanelID="uxInvisiblePanel" />
            </UpdatedControls>
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="optCard" UpdatePanelRenderMode="Inline" LoadingPanelID="uxInvisiblePanel" />
            </UpdatedControls>
        </tek:AjaxSetting>
        <tek:AjaxSetting AjaxControlID="uxOpenWarning">
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="uxOpenWarning" />
            </UpdatedControls>
        </tek:AjaxSetting>

        <tek:AjaxSetting AjaxControlID="optGrid">
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="uxReportGrid" LoadingPanelID="uxLoadingPanelCustom" />
            </UpdatedControls>
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="optGrid" UpdatePanelRenderMode="Inline" LoadingPanelID="uxInvisiblePanel" />
            </UpdatedControls>
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="optCard" UpdatePanelRenderMode="Inline" LoadingPanelID="uxInvisiblePanel" />
            </UpdatedControls>
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="chkHeaderCV" UpdatePanelRenderMode="Inline" LoadingPanelID="uxInvisiblePanel" />
            </UpdatedControls>

            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="uxExporterTop" UpdatePanelRenderMode="Inline" LoadingPanelID="uxInvisiblePanel" />
            </UpdatedControls>

        </tek:AjaxSetting>
        <tek:AjaxSetting AjaxControlID="optCard">
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="uxReportGrid" LoadingPanelID="uxLoadingPanelCustom" />
            </UpdatedControls>
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="optGrid" UpdatePanelRenderMode="Inline" LoadingPanelID="uxInvisiblePanel" />
            </UpdatedControls>
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="optCard" UpdatePanelRenderMode="Inline" LoadingPanelID="uxInvisiblePanel" />
            </UpdatedControls>
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="chkHeaderCV" UpdatePanelRenderMode="Inline" LoadingPanelID="uxInvisiblePanel" />
            </UpdatedControls>
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="uxExporterTop" UpdatePanelRenderMode="Inline" LoadingPanelID="uxInvisiblePanel" />
            </UpdatedControls>
        </tek:AjaxSetting>
        <tek:AjaxSetting AjaxControlID="btnFilterWorkingStatus">
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="uxReportGrid" />
                <tek:AjaxUpdatedControl ControlID="btnAddWorkQueue" LoadingPanelID="uxInvisiblePanel" />
                <tek:AjaxUpdatedControl ControlID="btnRemoveWorkQueue" LoadingPanelID="uxInvisiblePanel" />
                <tek:AjaxUpdatedControl ControlID="chkHeaderCV" LoadingPanelID="uxInvisiblePanel" />
            </UpdatedControls>
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="uxExporterTop" UpdatePanelRenderMode="Inline" LoadingPanelID="uxInvisiblePanel" />
            </UpdatedControls>
        </tek:AjaxSetting>
        <tek:AjaxSetting AjaxControlID="btnRefreshRainbow">
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="uxReportGrid" />
                <tek:AjaxUpdatedControl ControlID="pnlAssignmentList"  />
            </UpdatedControls>
        </tek:AjaxSetting>
        <tek:AjaxSetting AjaxControlID="btnRebindReportGrid">
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="uxReportGrid" />
                <tek:AjaxUpdatedControl ControlID="chkHeaderCV" LoadingPanelID="uxInvisiblePanel" />
            </UpdatedControls>
        </tek:AjaxSetting>
        <tek:AjaxSetting AjaxControlID="uxRefreshBtn">
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="pnlAssignmentList" />
            </UpdatedControls>
        </tek:AjaxSetting>
        <tek:AjaxSetting AjaxControlID="uxReloadAssignment">
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="pnlAssignmentList" />
            </UpdatedControls>
        </tek:AjaxSetting>
        <tek:AjaxSetting AjaxControlID="btnRemoveWorkQueue">
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="btnRemoveWorkQueue" />
            </UpdatedControls>
        </tek:AjaxSetting>
    </AjaxSettings>
</as:RadAjaxManagerProxy>

<div class="row d-flex mb-5x space-between">
    <div class="col-xs-10">
        <asp:Label ID="ucAssignmentSummary" CssClass="assignment-sub-title" Text="Assignment Summary" runat="server" meta:resourcekey="AssignmentSummaryResource1"></asp:Label>
    </div>
    <div class="col-xs-2">
        <asp:LinkButton ID="uxRefreshBtn" CssClass="pull-right mt-10" Text="Refresh" runat="server" OnClick="uxRefreshBtn_Click" meta:resourcekey="RefreshSummaryResource"></asp:LinkButton>
    </div>
</div>

<as:Panel ID="pnlAssignmentList" runat="server" CssClass="pos-relative">
    <uc:DetectionQueueAssignmentList ID="grdAssignment" runat="Server" AutoBindData="false" FromPage="Barometer" />
</as:Panel>
<div class="row mt-7x" id="uxWorkedNotWorked">
    <div class="col-xs-10 dark-blue d-flex">
        <div class="mr-10" runat="server" id="optAll">
            <a href="#" onclick="ChangeWorkedStatusOption(this, -1); return false;" class="btn-bp-merchant btn-work-selected btn-work-js">
                <asp:Literal ID="Literal2" runat="server" meta:resourcekey="optAllMerchantResource"></asp:Literal></a>
        </div>

        <div class="mr-10" runat="server" id="optNotWorked">
            <a href="#" onclick="ChangeWorkedStatusOption(this, 0); return false;" class="btn-bp-merchant btn-work-js">
                <asp:Literal ID="Literal1" runat="server" meta:resourcekey="optNotWorkedMerchantResource"></asp:Literal></a>
        </div>
        <div class="mr-10" runat="server" id="optWorked">
            <a href="#" onclick="return ChangeWorkedStatusOption(this, 1);" class="btn-bp-merchant btn-work-js">
                <asp:Literal ID="Literal3" runat="server" meta:resourcekey="optWorkedMerchantResource"></asp:Literal></a>
        </div>
        <div class="mr-10" runat="server" id="optWIPByMe">
            <a href="#" onclick="return ChangeWorkedStatusOption(this, 2);" class="btn-bp-merchant btn-work-js">
                <asp:Literal ID="Literal4" runat="server" meta:resourcekey="optWIPByMeMerchantResource"></asp:Literal></a>
        </div>
        <div class="mr-10" runat="server" id="optWIPByOrthers">
            <a href="#" onclick="return ChangeWorkedStatusOption(this, 3);" class="btn-bp-merchant btn-work-js">
                <asp:Literal ID="Literal5" runat="server" meta:resourcekey="optWIPByOrthersMerchantResource"></asp:Literal></a>
        </div>
        <div class="mr-10" runat="server" id="optRequeued">
            <a href="#" onclick="return ChangeWorkedStatusOption(this, 4);" class="btn-bp-merchant btn-work-js">
                <asp:Literal ID="Literal6" runat="server" meta:resourcekey="optRequeuedMerchantResource"></asp:Literal></a>
        </div>
        <div class="ml-10 hide" id="btnRequeueBoundary">
            <as:Button ID="btnAddWorkQueue" runat="server" Text="Add To Work Queue" CssClass="btn btn-default as-inline mb-2" meta:resourcekey="btnAddToWorkQueueResource" />
        </div>
        <div class="ml-10 hide" id="divRemoveWorkQueue">
            <as:Button ID="btnRemoveWorkQueue" runat="server" Text="Remove From Work Queue" CssClass="btn btn-default as-inline mb-2" meta:resourcekey="btnRemoveFromQueueResource" />
        </div>
    </div>
    <as:Button ID="btnFilterWorkingStatus" CssClass="hide" runat="server" OnClick="btnFilterWorkingStatus_Click" />
    <as:HiddenField runat="server" ID="filterWorkingStatus" />
</div>
<div class="height-24"></div>
<div class="title-page">
    <%--    <a href="#" onclick="return refreshDataEvent();" class="btn-refresh-grid">
        <asp:Literal ID="Literal7" runat="server" meta:resourcekey="Literal7Resource"></asp:Literal>
    </a>--%>
    <uc:UxExport ID="uxExporterTop" IsOnTop="true" runat="server" GridID="uxReportGrid" ShowPDF="false" OnNeedExportConfig="uxExport_OnNeedExportConfig" />
</div>
<div class="list-type-view mt-2x">
    <div class="row">
        <div class="col-md-8">
            <uc:CustomView runat="server" ID="uxCustomView" PageMode="DetectionQueue" />
        </div>
        <div class="col-md-4">
            <div class="btn-group view-data-options  pull-right" role="group" aria-label="Default btn-group">
                <div id="GridView" runat="server" class="btn btn-default pos-relative mr-m-1 btn-option btn-option-gridview">
                    <as:RadioButton ID="optGrid" runat="server" GroupName="GroupView" Text="GRIDS" OnCheckedChanged="ChangeView" AutoPostBack="True" meta:resourcekey="ASGroupViewGrids" />
                </div>
                <div id="CardView" runat="server" class="btn btn-default pos-relative pull-right btn-option btn-option-cardview">
                    <as:RadioButton ID="optCard" runat="server" GroupName="GroupView" Text="CARDS" OnCheckedChanged="ChangeView" AutoPostBack="True" meta:resourcekey="ASGroupViewCards" />
                </div>
            </div>
            <div class="btn-group pull-right mr-5x">
                <as:PlaceHolder ID="uxPlaceHolderColorLegend" runat="server">
                    <a href="#item" onclick="return OpenFlagColorTable();" class="btn btn-default">
                        <asp:Literal ID="rm_DetectionQueue_aspx_ColorLegend" runat="server" meta:resourcekey="rm_DetectionQueue_aspx_ColorLegendResource1">Color Legend</asp:Literal></a>
                </as:PlaceHolder>
            </div>
        </div>
    </div>
    <div class="row ">
        <div class="col-md-12">
            <as:CheckBox runat="server" ID="chkHeaderCV" Text="Re-queue All" CssClass="control-inline text-requeue-all" meta:resourcekey="ASCheckBoxResource1" />
        </div>
    </div>
</div>
<div class="height-8"></div>

<as:ASGrid ID="uxReportGrid" runat="server" AllowFilteringByColumn="false" AllowPaging="True"
    AllowSorting="True" AutoGenerateColumns="False" AllowSortFilterWhenExport="true"
    ASPagingMethod="SPASingleMethod1" GridName="Risk Management - Risk Analysis - Barometer Report"
    IsAutoExportTemplate="true" IsCacheTemplateFile="false" OnPreRender="uxReportGrid_PreRender"
    OnItemCommand="uxReportGrid_ItemCommand" OnSortCommand="uxReportGrid_SortCommand" OnNeedDataSource="uxReportGrid_NeedDataSource"
    OnItemDataBound="uxReportGrid_ItemDataBound" OnDataSourceReady="uxReportGrid_DataSourceReady"
    OnInit="uxReportGrid_Init" ClientSettings-EnableAlternatingItems="true" SortingSettings-SortToolTip=""
    HeaderStyle-Width="60px" CssClass="in" meta:resourcekey="uxReportGridResource1" ClientSettings-ClientEvents-OnDataBound="gridDatabound()">
    <MasterTableView>
        <Columns>
            <as:ASGridTemplateColumn HeaderText="Work" UniqueName="CheckBoxColumn" HeaderStyle-HorizontalAlign="Center"
                ItemStyle-HorizontalAlign="Center" meta:resourcekey="CheckBoxColumn" HeaderStyle-Width="68px" ItemStyle-Width="68px">
                <ItemTemplate>
                    <as:ASMCFWorkContent runat="server" ID="workItem"></as:ASMCFWorkContent>
                </ItemTemplate>

                <HeaderStyle HorizontalAlign="Center" Width="68px"></HeaderStyle>

                <ItemStyle HorizontalAlign="Center" Width="68px"></ItemStyle>
            </as:ASGridTemplateColumn>
            <as:ASGridBoundColumn HeaderText="wkExport" DataField="CurrentStatusDesc" Display="false"
                UniqueName="CurrentStatusDesc" meta:resourcekey="CheckBoxColumn">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>
                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
            </as:ASGridBoundColumn>
            <as:ASGridBoundColumn HeaderText="WKD #" ItemStyle-CssClass="item-worked" DataField="Worked" UniqueName="Worked"
                HeaderStyle-Width="47px" meta:resourcekey="Worked" ASFormat="Integer">
                <HeaderStyle HorizontalAlign="Center" />
            </as:ASGridBoundColumn>

            <as:ASGridBoundColumn HeaderText="PW #" ItemStyle-CssClass="item-para-worked" DataField="ParametersWorked" UniqueName="ParametersWorked"
                HeaderStyle-Width="40px" meta:resourcekey="ParametersWorked" ASFormat="Integer">
            </as:ASGridBoundColumn>

            <as:ASGridBoundColumn HeaderText="IsRequeuedExport" DataField="IsRequeuedDesc" Display="false"
                UniqueName="IsRequeuedDesc" meta:resourcekey="RQCheckbox">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>
                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
            </as:ASGridBoundColumn>

            <as:ASGridTemplateColumn HeaderText="RQ" UniqueName="RQCheckbox" HeaderStyle-HorizontalAlign="Center" Visible="false"
                ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="40px" ItemStyle-Width="40px"
                HeaderTooltip="Check All Merchants (Checkbox to Assign)" meta:resourcekey="RQCheckbox">
                <HeaderTemplate>
                    <span title="" id='<%#GetLocalResourceObject("RQCheckbox.HeaderText").ToString() %>'>
                        <%#GetLocalResourceObject("RQCheckbox.HeaderText").ToString() %>
                    </span>
                    <input type="checkbox" id="chkHeader" runat="server" />
                </HeaderTemplate>
                <ItemTemplate>
                    <input type="checkbox" id="chkItem" runat="server" />
                </ItemTemplate>

                <HeaderStyle HorizontalAlign="Center" Wrap="true" Width="40px"></HeaderStyle>

                <ItemStyle HorizontalAlign="Center" Width="40px"></ItemStyle>
            </as:ASGridTemplateColumn>

            <as:ASGridBoundColumn AllowSorting="true" HeaderText="Merchant Name" DataField="MerchantName"
                UniqueName="MerchantName" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="left"
                Visible="true" HeaderStyle-Wrap="false" ItemStyle-Wrap="true" SortExpression="MerchantName"
                HeaderTooltip="Merchant Name" ItemStyle-CssClass="merchantName" HeaderStyle-Width="250px" meta:resourcekey="MerchantName">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>
                <HeaderStyle HorizontalAlign="Center" Wrap="False" Width="250px"></HeaderStyle>
                <ItemStyle HorizontalAlign="Left" Wrap="True" CssClass="merchantName" Width="250px"></ItemStyle>
            </as:ASGridBoundColumn>

            <tek:GridBoundColumn HeaderText="Merchant Number" DataField="MerchantNumber" UniqueName="MerchantNumber"
                Visible="false" HeaderStyle-Width="200px" meta:resourcekey="MerchantNumber">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>
            </tek:GridBoundColumn>

            <as:ASGridTemplateColumn DataField="CardView" UniqueName="CardView" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-CssClass="td-card-view">
                <ItemTemplate>
                    <div class="row card-view">
                        <div class="col-xs-12 auto-grid-nrt">
                            <div class="item itemcheck rdHeader" runat="server" id="colWorkStatus">
                                <as:ASMCFWorkContent runat="server" ID="workItemCV"></as:ASMCFWorkContent>
                            </div>

                            <div class="item" id="wkCV" runat="server">
                                <asp:LinkButton ID="lbtSort_Worked" CssClass="rdHeader" runat="server" CommandName="Sort_Worked"><%# GetLocalResourceObject("Worked.HeaderText") %></asp:LinkButton>
                                <span class="icon-sort"></span>
                                <div runat="server" class="item-worked"><%# Eval("Worked") %></div>
                            </div>

                            <div class="item" id="Div1" runat="server">
                                <asp:LinkButton ID="lbtSort_ParametersWorked" CssClass="rdHeader" runat="server" CommandName="Sort_ParametersWorked"><%# GetLocalResourceObject("ParametersWorked.HeaderText") %></asp:LinkButton>
                                <span class="icon-sort"></span>
                                <div id="Div2" class="item-para-worked" runat="server"><%# Eval("ParametersWorked") %></div>
                            </div>

                            <div class="item itemcheck rdHeader" id="colRQColumn" runat="server">
                                <asp:Label ID="lbl_colRQColumn" CssClass="rdHeader" runat="server"><%# GetLocalResourceObject("RQCheckbox.HeaderText") %></asp:Label>
                                <div id="bgChkItemCV" runat="server">
                                    <input type="checkbox" id="chkItemCV" runat="server" title="Checkbox to assign" />
                                </div>
                            </div>
                            <div class="item w20" id="colMerchantName" runat="server">
                                <asp:LinkButton ID="lbtSort_MerchantName" CssClass="rdHeader" runat="server" Text="Merchant Name" CommandName="Sort_MerchantName" meta:resourcekey="ASGridBoundColumnCardResource1"></asp:LinkButton>
                                <span class="icon-sort"></span>
                                <div runat="server" id="merchantName"><%# Eval("MerchantName") %></div>
                            </div>
                            <asp:Repeater runat="server" ID="uxRpt_Grid" OnItemDataBound="uxRpt_Grid_ItemDataBound" OnItemCommand="uxRpt_Grid_ItemCommand">
                                <ItemTemplate>
                                    <div class="item">
                                        <asp:LinkButton ID="lbtSort_Column" CommandArgument='<%# Eval("key") %>'
                                            CommandName="Sort_Column" CssClass="rdHeader" runat="server" Text="AT%" meta:resourcekey="ASGridBoundColumnCardResource4"></asp:LinkButton>
                                        <span class="icon-sort"></span>
                                        <div id="column" runat="server"></div>
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                    </div>
                </ItemTemplate>
            </as:ASGridTemplateColumn>
        </Columns>
    </MasterTableView>
    <HeaderStyle Width="60px"></HeaderStyle>
</as:ASGrid>

<asp:LinkButton ID="uxOpenWarning" CssClass="hide" runat="server" OnClick="uxOpenWarning_Click"></asp:LinkButton>
<as:HiddenField ID="hddExportOption" runat="server" />
<as:HiddenField ID="hddMessage" runat="server" />
<as:Button ID="btnRefreshRainbow" CssClass="hide" runat="server" OnClick="btnRefreshRainbow_Click" />
<as:Button ID="btnRebindReportGrid" CssClass="hide" runat="server" OnClick="btnRebindReportGrid_Click" />
<asp:Button ID="uxReloadAssignment" CssClass="hide" runat="server" OnClick="uxRefreshBtn_Click"></asp:Button>
<as:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <script type="text/javascript">
        var detectionQueue_hddExportOption = "<%= hddExportOption.ClientID %>";
        var detectionQueueRainbowReport_uxReportGrid = "<%=uxReportGrid.ClientID%>";
        var uxOpenWarning_ClientID = "<%=uxOpenWarning.ClientID%>";
        var hddMessage_ClientID = "<%=hddMessage.ClientID%>";
        var pnlAssignmentList_ClientID = "<%=pnlAssignmentList.ClientID%>";
        var lblWorkStatus = "<%=RM_MCF_GeneralFuncsLib.GetResourceValue("WorkStatus_Text").ToString()%>";
        var lblWorkInProgressStatus = "<%=RM_MCF_GeneralFuncsLib.GetResourceValue("WorkInProgressStatus_Text").ToString()%>";
        var lblWorkedStatus = "<%=RM_MCF_GeneralFuncsLib.GetResourceValue("WorkedStatus_Text").ToString()%>";
        var rm_MCF_DetectionQueueRainbowReport_btnFilterWorkingStatus = '<%= btnFilterWorkingStatus.ClientID %>';
        var rm_MCF_DetectionQueueRainbowReport_btnRefreshRainbow = '<%= btnRefreshRainbow.ClientID %>';
        var rm_MCF_DetectionQueueRainbowReport_filterWorkingStatus = '<%= filterWorkingStatus.ClientID %>';
        var rm_MCF_DetectionQueueRainbowReport_btnRebindReportGrid = '<%= btnRebindReportGrid.ClientID %>';
        var rm_MCF_DQRainbow_optCard = '<%=optCard.ClientID%>';
        var rm_MCF_DQRainbow_chkHeaderCV = '<%=chkHeaderCV.ClientID%>';
        var applyFilterId_value = '<%= Page.SecureQueryString["ApplyFilterId"]%>';
        var rm_MCF_DetectionQueueRainbowReport_uxReloadAssignment = '<%= uxReloadAssignment.ClientID %>';
    </script>
    <script type="text/javascript" src="<%=ResolveUrl("~") %>res/js/risk_MCF/rm_MCF_DetectionQueueRainbowReport.js">             
    </script>
</as:RadCodeBlock>
