<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" CodeFile="rm_MCF_MgmtReport_G2MoneyLaundering.aspx.cs"
    Inherits="rm_MCF_MgmtReport_G2MoneyLaundering" EnableEventValidation="false" meta:resourcekey="PageResource1" %>

<%@ Register Src="~/UserControls/rm_MCF_G2MoneyLaundering_Filter.ascx" TagName="ReportFilter" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/UxExport.ascx" TagName="UxExport" TagPrefix="uc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPage" runat="Server">

    <tek:RadAjaxManagerProxy runat="server" ID="uxRadAjaxManager">
        <AjaxSettings>
            <tek:AjaxSetting AjaxControlID="uxGroupList">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="uxGroupList" />
                </UpdatedControls>
            </tek:AjaxSetting>
        </AjaxSettings>
    </tek:RadAjaxManagerProxy>

    <uc:ReportFilter ID="uxReportFilter" runat="server" OnSearch="uxReportFilter_Search"></uc:ReportFilter>
    <uc:UxExport ID="uxExporter" runat="server" GridID="uxGroupList" IsOnTop="true" ShowPDF="false"
        GridHeader="G2 MONEY LAUNDERING" meta:resourcekey="uxExportTopResource1" />
    <as:PlaceHolder ID="uxPlaceHolder" runat="server">
        <as:ASGrid ID="uxGroupList" runat="server" AutoGenerateColumns="false" AllowSorting="true" ASPagingMethod="SPASingleMethod1"
            GridLines="None" AllowPaging="true" ShowPageTotal="false" meta:resourcekey="uxGroupListResource1" IsAutoExportTemplate="true">
            <MasterTableView>
                <Columns>
                    <%--Merchant ID --%>
                    <as:ASGridBoundColumn HeaderText="Merchant ID" DataField="MerchantNumber" UniqueName="MerchantNumber"
                        SortExpression="MerchantNumber" HeaderTooltip="MerchantNumber" ASFormat="StaticString" meta:resourcekey="ASGridTemplateColumnResource1"
                        HeaderStyle-Width="150px">
                    </as:ASGridBoundColumn>
                    <%--Merchant Name --%>
                    <as:ASGridBoundColumn HeaderText="Merchant Name" HeaderTooltip="Merchant Name" UniqueName="MerchantName" SortExpression="MerchantName"
                        ASFormat="StaticString" DataField="MerchantName" meta:resourcekey="ASGridTemplateColumnResource2">
                    </as:ASGridBoundColumn>
                    <%--Card Number --%>
                     <as:ASGridBoundColumn HeaderText="Card Number" HeaderTooltip="Card Number" UniqueName="PartialCardNumber" SortExpression="PartialCardNumber"
                        ASFormat="StaticString" DataField="PartialCardNumber" meta:resourcekey="ASGridTemplateColumnResource3" HeaderStyle-Width="150px">
                    </as:ASGridBoundColumn>
                    <%--G2 Expiration Date --%>
                    <as:ASGridBoundColumn HeaderText="G2 Expiration Date" HeaderTooltip="G2 Expiration Date" UniqueName="G2_ExpirationDate" SortExpression="G2_ExpirationDate" 
                        ASFormat="StaticString" DataField="G2_ExpirationDate" meta:resourcekey="ASGridTemplateColumnResource4">
                    </as:ASGridBoundColumn>
                    <%--G2 Date Transaction --%>
                    <as:ASGridBoundColumn HeaderText="G2 Auth Date" HeaderTooltip="G2 Auth Date" UniqueName="G2_TransactionDate" SortExpression="G2_TransactionDate" 
                        ASFormat="Date" DataField="G2_TransactionDate" meta:resourcekey="ASGridTemplateColumnResource5">
                    </as:ASGridBoundColumn>
                    <%--Aperia Date Transaction --%>
                    <as:ASGridBoundColumn HeaderText="Auth Date" HeaderTooltip="Aperia Auth Date" UniqueName="TransactionDate" SortExpression="TransactionDate" 
                        ASFormat="Date" DataField="TransactionDate" meta:resourcekey="ASGridTemplateColumnResource6">
                    </as:ASGridBoundColumn>
                    <%--Aperia Time Transaction --%>
                    <as:ASGridBoundColumn HeaderText="Auth Time" HeaderTooltip="Aperia Auth Time" UniqueName="TransactionTime" SortExpression="TransactionTime" 
                        ASFormat="StaticString" DataField="TransactionTime" meta:resourcekey="ASGridTemplateColumnResource11">
                    </as:ASGridBoundColumn>
                    <%--Aperia Transaction Amount --%>
                    <as:ASGridBoundColumn HeaderText="Auth Amount" HeaderTooltip="Aperia Auth Amount" UniqueName="AuthorizationAmount" SortExpression="AuthorizationAmount"
                        ASFormat="Currency" DataField="AuthorizationAmount" meta:resourcekey="ASGridTemplateColumnResource7">
                    </as:ASGridBoundColumn>

                    <%--Authorization or Sales Transaction --%>
                    <%--<as:ASGridBoundColumn HeaderText="Authorization or Sales Transaction" HeaderTooltip="Authorization or Sales Transaction" UniqueName="TransactionType" SortExpression="TransactionType"
                         ASFormat="StaticString" DataField="TransactionType" meta:resourcekey="ASGridTemplateColumnResource8">
                    </as:ASGridBoundColumn>--%>
                    <%--Aperia Authorization Number --%>
                    <%--<as:ASGridBoundColumn HeaderText="Aperia Authorization Number" HeaderTooltip="Aperia Authorization Number" UniqueName="AuthorizationNumber" SortExpression="AuthorizationNumber"
                         ASFormat="StaticString" DataField="AuthorizationNumber" meta:resourcekey="ASGridTemplateColumnResource9">
                    </as:ASGridBoundColumn>--%>


                    <%--Aperia Report Date --%>
                    <as:ASGridBoundColumn HeaderText="Report Date" HeaderTooltip="Aperia Report Date" UniqueName="ReportDate" SortExpression="ReportDate" ASFormat="Date"
                        DataField="ReportDate" meta:resourcekey="ASGridTemplateColumnResource10">
                    </as:ASGridBoundColumn>
                </Columns>
            </MasterTableView>
        </as:ASGrid>
    </as:PlaceHolder>
</asp:Content>
