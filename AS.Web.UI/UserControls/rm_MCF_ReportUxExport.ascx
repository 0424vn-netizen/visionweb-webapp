<%@ Control Language="C#" AutoEventWireup="true" Inherits="UserControls_rm_MCF_ReportUxExport" CodeFile="~/UserControls/rm_MCF_ReportUxExport.ascx.cs" %>


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

<h2 class="grid-title" runat="server" id="h2GridTitle" data-toggle="collapse" visible="false">
    <asp:Literal ID="litGridTitle" runat="server" meta:resourcekey="litGridTitleResource1" /><span class="text-muted"><asp:Literal ID="litGridSubTitle" runat="server" meta:resourcekey="litGridSubTitleResource1" /></span>
</h2>

<div class="report-export dropdown pull-right" runat="server" id="divExport">
    <a href="#" data-toggle="dropdown" data-hover="dropdown" class="dropdown-toggle" id="litExport" runat="server">
        <as:Literal ID="ltExport" runat="server" Text="EXPORT" meta:resourcekey="ltExportResource1"></as:Literal></a>
    <ul class="dropdown-menu">
        <li id="uxLiExcel" runat="server">
            <asp:LinkButton ID="imgExcel" runat="server" OnClick="ButtonExcel_Click" OnClientClick="doResetTimeOut()" meta:resourcekey="imgExcelResource1">Excel</asp:LinkButton></li>
        <li id="uxLiCSV" runat="server">
            <asp:LinkButton ID="imgCSV" runat="server" OnClick="ButtonCSV_Click" OnClientClick="doResetTimeOut()" meta:resourcekey="imgCSVResource1">CSV</asp:LinkButton></li>
        <li id="uxLiWord" runat="server">
            <asp:LinkButton ID="imgWord" runat="server" OnClick="ButtonWord_Click" OnClientClick="doResetTimeOut()" meta:resourcekey="imgWordResource1">Word</asp:LinkButton></li>
        <li id="uxLiPDF" runat="server">
            <asp:LinkButton ID="imgPDF" runat="server" OnClick="ButtonPDF_Click" OnClientClick="doResetTimeOut()" meta:resourcekey="imgPDFResource1">PDF</asp:LinkButton></li>
    </ul>

</div>


