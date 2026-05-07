<%@ Control Language="C#" AutoEventWireup="true" CodeFile="rm_MCF_UxDetectionQueueExport.ascx.cs" Inherits="UserControls_rm_MCF_UxDetectionQueueExport" %>

<tek:RadAjaxManagerProxy ID="radAjaxManagerProxy" runat="server">
    <AjaxSettings>
        <tek:AjaxSetting AjaxControlID="imgExcel">
        </tek:AjaxSetting>
        <tek:AjaxSetting AjaxControlID="imgCSV">
        </tek:AjaxSetting>
        <tek:AjaxSetting AjaxControlID="imgWord">
        </tek:AjaxSetting>
        <tek:AjaxSetting AjaxControlID="imgPDF">
        </tek:AjaxSetting>
    </AjaxSettings>
</tek:RadAjaxManagerProxy>

<div class="row">
    <div class="col-xs-10">
        <h2 class="grid-title " runat="server" id="h2GridTitle" data-toggle="collapse">
            <asp:Literal ID="litGridTitle" runat="server" meta:resourcekey="litGridTitleResource1" />
        </h2>
         <span class="text-muted hierarchy-title pointer hide" id="h2litGridSubTitle" runat="server" data-toggle="collapse">
            <asp:Literal ID="litGridSubTitle" runat="server" meta:resourcekey="litGridSubTitleResource1" />
        </span>
        
    </div>
    <div class="col-xs-2">
        <div class="report-export dropdown pull-right" runat="server" id="divExport">
            <a href="#" data-toggle="dropdown" data-hover="dropdown" class="dropdown-toggle" id="litExport" runat="server">
                <as:Literal ID="ltExport" runat="server" Text="EXPORT" meta:resourcekey="ltExportResource1"></as:Literal></a>
            <ul class="dropdown-menu">
                <li id="Li1" runat="server">
                    <asp:LinkButton ID="LinkButton1" runat="server" OnClientClick="return ShowExportModal(this, 'EXCEL')" meta:resourcekey="imgExcelResource1">Excel</asp:LinkButton></li>
                <li id="Li2" runat="server">
                    <asp:LinkButton ID="LinkButton2" runat="server" OnClientClick="return ShowExportModal(this, 'CSV')" meta:resourcekey="imgCSVResource1">CSV</asp:LinkButton></li>
                <li id="Li4" runat="server">
                    <asp:LinkButton ID="LinkButton4" runat="server" Visible="false" OnClientClick="return ShowExportModal(this, 'PDF')" meta:resourcekey="imgPDFResource1">PDF</asp:LinkButton></li>
            </ul>
            <ul class="dropdown-menu hide">
                <li id="uxLiExcel" runat="server">
                    <asp:LinkButton ID="imgExcel" runat="server" OnClientClick="doResetTimeOut()" meta:resourcekey="imgExcelResource1">Excel</asp:LinkButton></li>
                <li id="uxLiCSV" runat="server">
                    <asp:LinkButton ID="imgCSV" runat="server" OnClientClick="doResetTimeOut()" meta:resourcekey="imgCSVResource1">CSV</asp:LinkButton></li>
                <li id="uxLiWord" runat="server">
                    <asp:LinkButton ID="imgWord" runat="server" OnClientClick="doResetTimeOut()" meta:resourcekey="imgWordResource1">Word</asp:LinkButton></li>
                <li id="uxLiPDF" runat="server" visible="false">
                    <asp:LinkButton ID="imgPDF" runat="server" OnClientClick="doResetTimeOut()" meta:resourcekey="imgPDFResource1">PDF</asp:LinkButton></li>
            </ul>
            <as:HiddenField runat="server" ID="hddExportType" />
        </div>
    </div>
</div>
<as:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <script type="text/javascript">
        var uxCustomizeColumnsExportTop_imgExcelHide = '<%= imgExcel.ClientID %>';
        var uxCustomizeColumnsExportTop_imgCSVHide = '<%= imgCSV.ClientID %>';
        var uxCustomizeColumnsExportTop_imgPDFHide = '<%= imgPDF.ClientID %>';
        var uxCustomizeColumnsExportTop_hddExportType = '<%= hddExportType.ClientID%>'
    </script>
    <script type="text/javascript" src="<%=ResolveUrl("~") %>res/js/risk_MCF/rm_MCF_UxDetectionQueueExport.js"></script>
</as:RadCodeBlock>
