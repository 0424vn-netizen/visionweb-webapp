<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ReportFiltering.ascx.cs"
    Inherits="ReportFiltering" %>



<asp:PlaceHolder ID="PlaceHolder1" runat="server">
    <div class="row collapse report-filter-panel" id="reportFilter">
        <div class="col-md-12">
            <as:MPSReportFilter ID="uxReportFilter" runat="server" ButtonSubmitText="SEARCH"
                ButtonSubmitCss="btn btn-default" OnReportFilterAction="OnReportFilterActionUxReportFilter" />
        </div>
    </div>
    <div class="row">
        <div class="col-md-12 text-center" data-target=".report-filter-panel" data-toggle="collapse" id="containFilter">
            <span class="btn btn-link btn-report-filter"><asp:Literal ID="Literal1" runat="server" Text="FILTER" meta:resourcekey="ReportFilteringASCX_Text_Filter" /></span>
        </div>
    </div>
</asp:PlaceHolder>
<script type="text/javascript">
    var rm_RetCb_uxReportFiltering = "<%= uxReportFilter.ClientID %>";
    var funcValidate = 'rf_Submit';
</script>

