<%@ Control Language="C#" AutoEventWireup="true" CodeFile="TransactionSearch_Filtering.ascx.cs" Inherits="UserControls_TransactionSearch_Filtering" %>


<as:MPSReportFilter runat="server" ID="uxReportFilter" DateOptionVisible="false" 
                                        CustomTemplate="~/App_Data/FilteringOption_TransactionSearch.tpl"
                                        ButtonSubmitCss="display-none" OnReportFilterAction="uxReportFilter_ReportFilterAction">

                                    </as:MPSReportFilter>