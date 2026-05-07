<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UxExportTable.ascx.cs" Inherits="UserControls_UxExportTable" %>


<div class="row">
    <div class="col-xs-10" data-toggle="collapse" data-target='<%= "#" + GridID %>'>
        <h2 class="grid-title" runat="server" id="h2Title"><asp:Literal id="litTitle" runat="server" meta:resourcekey="litTitleResource1"></asp:Literal> <span class="text-muted"><asp:Literal id="litSubTitle" runat="server" meta:resourcekey="litSubTitleResource1"></asp:Literal> </span>
        </h2>
    </div>
    <div class="col-xs-2">
        <div class="report-export dropdown pull-right" runat="server" id="divExport">
            <a href="#" data-toggle="dropdown" data-hover="dropdown" class="dropdown-toggle" id="litExport" runat="server"><as:Literal ID="ltExport" runat="server" Text="EXPORT" meta:resourcekey="ltExportResource1"></as:Literal></a>
            <ul class="dropdown-menu">                
                <li id="uxLiExcel" runat="server"><asp:LinkButton ID="imgExcel" runat="server" OnClick="ExportButton_Click" OnClientClick="doResetTimeOut()" meta:resourcekey="imgExcelResource1">Excel</asp:LinkButton></li>
                <li id="uxLiCSV" runat="server"><asp:LinkButton ID="imgCSV" runat="server" OnClick="ExportButton_Click" OnClientClick="doResetTimeOut()" meta:resourcekey="imgCSVResource1">CSV</asp:LinkButton></li>
                <li id="uxLiWord" runat="server"><asp:LinkButton ID="imgWord" runat="server" OnClick="ExportButton_Click" OnClientClick="doResetTimeOut()" meta:resourcekey="imgWordResource1">Word</asp:LinkButton></li>
                <li id="uxLiPDF" runat="server"><asp:LinkButton ID="imgPDF" runat="server" OnClick="ExportButton_Click" OnClientClick="doResetTimeOut()" meta:resourcekey="imgPDFResource1">PDF</asp:LinkButton></li>               
            </ul>
        </div>
    </div>
</div>


