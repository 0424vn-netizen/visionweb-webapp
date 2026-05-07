<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ActivationReport_Filtering.ascx.cs" Inherits="UserControls_ActivationReport_Filtering" %>

<as:MPSReportFilter runat="server" ID="uxReportFilter" DateOptionVisible="false" DateOptionDateRangeVisible="true" DateOptionMonthlyVisible="true"
    CustomTemplate="~/App_Data/FilteringOption_ActivationReport.tpl"
    ButtonSubmitCss="display-none" OnReportFilterAction="uxReportFilter_ReportFilterAction"></as:MPSReportFilter>
