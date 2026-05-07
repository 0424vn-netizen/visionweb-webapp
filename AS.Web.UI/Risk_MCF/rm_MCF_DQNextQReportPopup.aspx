<%@ Page Title="Next Q Report" Language="C#" MasterPageFile="~/MasterPagePopup.master"
    AutoEventWireup="true" CodeFile="rm_MCF_DQNextQReportPopup.aspx.cs" Inherits="rm_MCF_DQNextQReportPopup" EnableEventValidation="false" meta:resourcekey="PageResource1" %>

<%@ Register Src="~/UserControls/rm_MCF_DQNextQReport.ascx" TagName="NextQReport" TagPrefix="uc" %>
<%@ Register TagName="DetectionQueueAssignmentList" Src="~/UserControls/rm_MCF_DetectionQueueAssignmentList.ascx" TagPrefix="uc" %>
<%@ Register TagName="ASPager" Src="~/UserControls/ASPager.ascx" TagPrefix="uc" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="Server">
    <as:RadAjaxManagerProxy runat="server" ID="uxRadManager">
        <AjaxSettings>
            <tek:AjaxSetting AjaxControlID="btnClickMerchantNumber">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="btnClickMerchantNumber" />
                </UpdatedControls>
            </tek:AjaxSetting>
            <tek:AjaxSetting AjaxControlID="uxOpenWarning">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="uxOpenWarning" />
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
        </AjaxSettings>
    </as:RadAjaxManagerProxy>
    <div class="row">
        <div class="col-md-12 no-margin-bottom">
            <uc:PageTitle ID="PageTitle1" runat="server" ReportTitle="Next Queue" HasFilteringOption="true" meta:resourcekey="PageResource1" />
        </div>
    </div>
 
    <as:Panel ID="pnlAssignmentList" runat="server" CssClass="pos-relative">
    </as:Panel>
    <div class="row">
        <div class="col-xs-10 title-nextqueue">
            <uc:PageTitle runat="server" ID="uxPageTitle" ReportTitle="Assignment" HasShowHierarchy="false" />

        </div>
    </div>
    <div class="row">
        <div class="col-xs-10">
            <asp:Label ID="uxMerchantWorkedReport" CssClass="navbar-link" Visible="false" runat="server">
                <a href="javascript:void(0)" onclick="ShowPopupModal('<%=ResolveUrl("~") %>risk_MCF/rm_MCF_MerchantWorkedReportPopup.aspx'); return false;" class="navbar-link"><%=GetLocalResourceObject("uxMerchantWorkedReportResource1.Text").ToString() %> </a>
            </asp:Label>
        </div>
    </div>

    <div class="row">
        <div class="col-xs-7 title-nextqueue">
            <h1 class="report-title mb-0">
                <asp:Literal ID="lblMerchantName" runat="server"></asp:Literal>
            </h1>
        </div>
        <div class="col-xs-5 text-right mt-20">
            <div class="control-inline">
                <as:Button ID="btnAddWorkQueue" runat="server" Text="Add To Work Queue" CssClass="btn btn-default as-inline" meta:resourcekey="btnAddToWorkQueueResource" />
            </div>
            <tek:RadToolTip runat="server" RenderMode="Lightweight" ShowCallout="false" ID="uxDispositionPop" TargetControlID="uxNext" Text=""
                ShowEvent="OnMouseOver" HideEvent="LeaveTargetAndToolTip" AutoCloseDelay="0" OffsetY="-5" OffsetX="-43" RelativeTo="Element" OnClientBeforeShow="workPopBeforeShow" Position="BottomCenter" CssClass="marker-diposition diposition-nextqueue">
            </tek:RadToolTip>
            <as:Button ID="uxNext" runat="server" Text="Set Disposition And Next" OnClick="uxNext_Click" Enabled="false" CssClass="btn btn-default" IsStandardButton="False" meta:resourcekey="uxNextResource1" />
            <as:Button ID="uxNextHide" runat="server" Text="Next" OnClick="uxNext_Click" CssClass="btn btn-default hide" IsStandardButton="False" meta:resourcekey="uxNextResource1" />
        </div>
    </div>

    <div class="row">
        <div class="col-xs-10">
            <h2 class="grid-title inline-block mt-0 mb-10" data-toggle="collapse" data-target="#cidMerchantInfo">
                <span class="text-muted">
                    <asp:Literal ID="Literal1" runat="server" meta:resourcekey="rm_DQNextQReportPopup_aspx_Text1Resource1" Text="Merchant Information"></asp:Literal></span>
            </h2>
        </div>
        <div class="col-xs-2">
            <div id="ctl00_ContentPage_uxExporter_divExport" class="report-export on-top dropdown pull-right">
                <a href="#" data-toggle="dropdown" data-hover="dropdown" class="dropdown-toggle">
                    <asp:Literal ID="rm_DQNextQReportPopup_aspx_Text2" runat="server" meta:resourcekey="rm_DQNextQReportPopup_aspx_Text2Resource1" Text="EXPORT"></asp:Literal></a>
                <ul class="dropdown-menu">
                    <li>
                        <asp:LinkButton runat="server" ID="uxExportExcel" OnClick="ImageButtonExcel_Click" Text="Excel" meta:resourcekey="uxExportExcelResource1"></asp:LinkButton>
                    </li>
                </ul>
            </div>
        </div>
    </div>
    <div id="cidSecurityReport" class="in">
        <uc:NextQReport ID="uxNextQReport" runat="server" />
    </div>

    <div id="uxProgress" style="display: none; padding: auto; position: fixed; vertical-align: middle; text-align: center; z-index: 9999; height: 100%; width: 100%; top: 0px; left: 0px; bottom: 0px; right: 0px; background: #FFF url('../res/images/loading.gif') no-repeat center center; opacity: .7;">
    </div>

    <as:Button ID="btnClickMerchantNumber" runat="server" OnClick="btnClickMerchantNumber_Click" CssClass="hide" IsStandardButton="True" meta:resourcekey="btnClickMerchantNumberResource1" />
    <asp:LinkButton ID="uxOpenWarning" CssClass="hide" runat="server" OnClick="uxOpenWarning_Click"></asp:LinkButton>
    <as:HiddenField ID="hddMessage" runat="server" />
    <a  class="hide" id="ucSelectMerchantWorked" target="_self" href="#"></a>
    <as:RadCodeBlock ID="JavaScript" runat="server">
        <script type="text/javascript">
            var rm_DQNextQReportPopup_uxNext = '<%=uxNext.ClientID %>';
            var rm_DQNextQReportPopup_btnClickMerchantNumber = '<%=btnClickMerchantNumber.ClientID %>';
            var rm_NextQueueModal_LENGHT_OF_QUEUE = '<%= LENGHT_OF_QUEUE%>';
            var rm_NextQueueModal_Risk_NextQueue_Worked = '<%= Resources.RiskMessageManager.Risk_NextQueue_Worked %>';
            var uxOpenWarning_ClientID = "<%=uxOpenWarning.ClientID%>";
            var hddMessage_ClientID = "<%=hddMessage.ClientID%>";
            var rm_DQNextQReportPopup_uxNextHide = '<%=uxNextHide.ClientID %>';
            var rm_DQNextQReportPopup_AssignmentID = '<%= AssignmentId %>';
            var rm_DQNextQReportPopup_ReportDate = '<%= ReportDate %>';
            var rm_DQNextQReportPopup_OrderBy = '<%= OrderBy %>';
            var rm_DQNextQReportPopup_ApplyFilterId = '<%= ApplyFilterId %>';
            var applyFilteredId = '<%=ApplyFilterId%>';
        </script>
        <script type="text/javascript" src="<%=ResolveUrl("~") %>res/js/risk_MCF/rm_MCF_DQNextQReportPopup.js"></script>
    </as:RadCodeBlock>
</asp:Content>