<%@ Control Language="C#" AutoEventWireup="true" CodeFile="~/UserControls/UxExportQueue.ascx.cs" Inherits="UserControls_UxExportQueue" %>

<asp:PlaceHolder ID="phTitleRow" runat="server">
    <div class="row">
        <div class="col-xs-10">
            <h2 runat="server" id="h2GridTitle" data-toggle="collapse">
                <asp:Literal ID="litGridTitle" runat="server" /><span class="text-muted"><asp:Literal ID="litGridSubTitle" runat="server" /></span>
            </h2>
        </div>
        <div class="col-xs-2">
</asp:PlaceHolder>

<div runat="server" id="divExportWrapper" class="pull-right" style="display: flex; align-items: center;">
    <a href="<%= ResolveUrl("~/Risk_MCF/rm_MCF_MgmtReport_Exports.aspx") %>" style="margin-right: 20px; font-size: 12px; font-weight: 700">VIEW EXPORT QUEUE</a>
    <div class="report-export dropdown" runat="server" id="divExport">
        <a href="#" data-toggle="dropdown" data-hover="dropdown" class="dropdown-toggle">EXPORT</a>
        <ul class="dropdown-menu">
            <li id="uxLiExcel" runat="server">
                <a href="#" onclick="doResetTimeOut(); UxExportQueue_DoExport('Excel'); return false;">Excel</a>
            </li>
            <li id="uxLiCSV" runat="server">
                <a href="#" onclick="doResetTimeOut(); UxExportQueue_DoExport('CSV'); return false;">CSV</a>
            </li>
            <li id="uxLiPDF" runat="server">
                <a href="#" onclick="doResetTimeOut(); UxExportQueue_DoExport('PDF'); return false;">PDF</a>
            </li>
        </ul>
    </div>
</div>

<asp:PlaceHolder ID="phTitleRowClose" runat="server"></div>
</div>
</asp:PlaceHolder>

<input type="hidden" runat="server" id="hddFilterParams" />

<as:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <script type="text/javascript">
        var UxExportQueue_PageName = '<%= PageName %>';
        var UxExportQueue_FileName = '<%= GeneralFuncsLib.FormatFileName(GridHeader ?? string.Empty) %>';
        var UxExportQueue_HandlerUrl = '<%= ResolveUrl("~/ExportQueueInsert.ashx") %>';
        var UxExportQueue_NotifyUrl = '<%= ResolveUrl("~/Risk_MCF/rm_MCF_ExportQueueNotifyModal.aspx") %>';
        var UxExportQueue_FilterParamsId = '<%= hddFilterParams.ClientID %>';
    </script>
    <script type="text/javascript" src="<%=ResolveUrl("~") %>res/js/risk_MCF/UxExportQueue.js"></script>
</as:RadCodeBlock>
