<%@ Control Language="C#" AutoEventWireup="true" Inherits="UserControls_UxExportStatement" CodeFile="~/UserControls/UxExportStatement.cs" %>

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
    <div class="col-xs-12">
        <h2 class="grid-title" runat="server" id="h2GridTitle" data-toggle="collapse">
            <asp:Literal ID="litGridTitle" runat="server" /><span class="text-muted"><asp:Literal ID="litGridSubTitle" runat="server" /></span>
        </h2>
    </div>    
</div>
