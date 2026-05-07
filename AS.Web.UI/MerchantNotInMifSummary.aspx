<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MerchantNotInMifSummary.aspx.cs" Inherits="MerchantNotInMifSummary" MasterPageFile="~/MasterPage.master" Title="" meta:resourcekey="PageTitle" %>

<%@ Register Src="~/UserControls/UxExport.ascx" TagName="UxExport" TagPrefix="uc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPage" runat="Server">

    <tek:RadAjaxManagerProxy runat="server" ID="uxRadAjaxManager">
        <AjaxSettings>
            <tek:AjaxSetting AjaxControlID="uxHierarchy">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="pnlHierarchyValue"/>
                </UpdatedControls>
            </tek:AjaxSetting>
            <tek:AjaxSetting AjaxControlID="uxReportGrid">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="uxReportGrid" />
                </UpdatedControls>
            </tek:AjaxSetting>
        </AjaxSettings>
    </tek:RadAjaxManagerProxy>

    <div class="row collapse report-filter-panel">
        <div class="col-md-12 report-filter">
            <div class="filter-block">
                <table>
                    <tr>
                        <td class="text-right">
                            <div class="filter-item">
                                <as:RadioButton ID="uxDaily" runat="server" Text="Daily" GroupName="Date" onclick="ChangeDateOption(this)"
                                    onkeypress="javascript:return false;" CssClass="date-item" meta:resourcekey="uxDailyResource1" Value="" />
                                <as:RadioButton ID="uxMonthly" runat="server" Text="Monthly" GroupName="Date" onclick="ChangeDateOption(this)"
                                    onkeypress="javascript:return false;" CssClass="date-item" meta:resourcekey="uxMonthlyResource1" Value="" />
                                <as:RadioButton ID="uxRange" runat="server" Text="Date Range" GroupName="Date" onclick="ChangeDateOption(this)"
                                    onkeypress="javascript:return false;" CssClass="date-item" meta:resourcekey="uxRangeResource1" Value="" />
                            </div>
                        </td>
                        <td colspan="2" class="text-left">
                            <div class="filter-item" id="divDate">
                                <as:RadDatePicker ID="uxDate" runat="server" Width="128px">
                                    <Calendar FastNavigationStep="12" ShowRowHeaders="false" />
                                    <DateInput ID="DateInput1" runat="server" onclick="ShowCalendar('1')" onkeypress="return SearchEnterOnTextbox(event);" />
                                </as:RadDatePicker>
                            </div>
                            <div id="divDateRange" style="display: none;">
                                <div class="filter-item">
                                    <as:RadDatePicker ID="uxFromDate" runat="server" Width="128px">
                                        <Calendar FastNavigationStep="12" ShowRowHeaders="false" />
                                        <DateInput ID="DateInput2" runat="server" onclick="ShowCalendar('2')" onkeypress="return SearchEnterOnTextbox(event);" />
                                    </as:RadDatePicker>
                                </div>
                                <div class="filter-item">
                                    <as:RadDatePicker ID="uxEndDate" runat="server" Width="128px">
                                        <Calendar FastNavigationStep="12" ShowRowHeaders="false" />
                                        <DateInput ID="DateInput3" runat="server" onclick="ShowCalendar('3')" onkeypress="return SearchEnterOnTextbox(event);" />
                                    </as:RadDatePicker>
                                </div>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div class="filter-item">
                                <as:RadComboBox ID="uxHierarchy" OnClientKeyPressing="uxHierarchy_OnClientKeyPressing" OnSelectedIndexChanged="uxHierarchy_SelectedIndexChanged" AutoPostBack="true" runat="server" Width="248px" MaxHeight="200px" Filter="Contains"></as:RadComboBox>
                            </div>
                        </td>
                        <td>
                            <as:Panel ID="pnlHierarchyValue" runat="server">
                                <div class="filter-item">
                                    <as:TextBox ID="uxMerchantNameValue" CssClass="rf_TextBox hide" MaxLength="50" runat="server" Width="248px" MaxHeight="200px"></as:TextBox>
                                    <as:TextBox ID="uxMerchantNumValue" CssClass="rf_TextBox hide" MaxLength="16" runat="server" Width="248px" MaxHeight="200px"></as:TextBox>
                                    <div class="multichooser-wrapper text-left" id="ucHierarchyValue" runat="server">
                                        <as:MultiChooser IsSearchContains="true" Width="248px" CssClass="form-control text-left" ID="uxHierarchyValue" runat="server" Placeholder=" " meta:resourcekey="uxHierarchyValueResource1"></as:MultiChooser>
                                    </div>
                                </div>
                            </as:Panel>
                        </td>
                        <td class="text-left">
                            <div class="filter-item">
                                <as:Button ID="uxSearch" runat="server" Text="Search" OnClientClick="return ValidateData();"
                                    OnClick="uxSearch_Click" CssClass="btn btn-default" IsStandardButton="False" meta:resourcekey="uxSearchResource1" />
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%--<div class="filter-item"></div>--%>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-md-12 text-center" data-target=".report-filter-panel" data-toggle="collapse">
            <span class="btn btn-link btn-report-filter">
                <as:Literal ID="Literal1" runat="server" Text="FILTER" meta:resourcekey="Literal1Resource1"></as:Literal></span>
        </div>
    </div>

    <uc:UxExport ID="uxExporter" runat="server" GridID="uxReportGrid" meta:resourcekey="uxGridTitleResource" />

    <as:ASGrid ID="uxReportGrid" runat="server" AllowFilteringByColumn="false" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" AllowSortFilterWhenExport="true"
        ASPagingMethod="SPASingleMethod" OnItemDataBound="uxReportGrid_ItemDataBound" GridName="Not In MIF Summary" CssClass="in" XOverFlowable="false" ShowReportTotal="true"
        ItemStyle-CssClass="no-backgound" meta:resourcekey="uxReportGridResource1" IsAutoExportTemplate="true">
        <MasterTableView>
            <Columns>
                <as:ASGridBoundColumn UniqueName="ReportDate" HeaderText="Date/Time" DataField="ReportDate"
                    HeaderStyle-Width="150px" ASFormat="Date" HeaderTooltip="Date/Time"
                    SortExpression="ReportDate" meta:resourcekey="ASGridBoundColumnResource1">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn UniqueName="FileTypeDescription" HeaderText="File Type" DataField="FileTypeDescription" ItemStyle-HorizontalAlign="Center"
                    AllowEncodeOnExporting="true" ASFormat="StaticString" HeaderTooltip="File Type"
                    SortExpression="FileTypeDescription" meta:resourcekey="ASGridBoundColumnResource2">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn UniqueName="FileSource" HeaderText="File Source" DataField="FileSource"
                    AllowEncodeOnExporting="true" ASFormat="StaticString" HeaderTooltip="FileSource" ItemStyle-HorizontalAlign="Center"
                    SortExpression="FileSource" meta:resourcekey="ASGridBoundColumnResource3">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn UniqueName="MerchantNumber" HeaderText="Merchant ID" DataField="MerchantNumber"
                    AllowEncodeOnExporting="true" ASFormat="StaticString" HeaderTooltip="Merchant ID" ItemStyle-HorizontalAlign="Center"
                    SortExpression="MerchantNumber" meta:resourcekey="ASGridBoundColumnResource4">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn UniqueName="MerchantName" HeaderText="MerchantName" DataField="MerchantName"
                    AllowEncodeOnExporting="true" ASFormat="DynamicString" HeaderTooltip="Merchant Name"
                    SortExpression="MerchantName" meta:resourcekey="ASGridBoundColumnResource5">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn UniqueName="TransactionCount" HeaderText="Trans #" DataField="TransactionCount" ASIsTotalColumn="true"
                    AllowEncodeOnExporting="true" ASFormat="Integer" HeaderTooltip="Trans #" ItemStyle-HorizontalAlign="Right"
                    SortExpression="TransactionCount" meta:resourcekey="ASGridBoundColumnResource6">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn UniqueName="TotalAmount" HeaderText="Total Amount" DataField="TotalAmount"
                    AllowEncodeOnExporting="true" ASFormat="Currency" HeaderTooltip="Total Amount" ASIsTotalColumn="true"
                    SortExpression="TotalAmount" meta:resourcekey="ASGridBoundColumnResource7">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn UniqueName="Address" HeaderText="Address" DataField="Address"
                    AllowEncodeOnExporting="true" ASFormat="DynamicString" HeaderTooltip="Address" ItemStyle-HorizontalAlign="Left"
                    SortExpression="Address" meta:resourcekey="ASGridBoundColumnResource8">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn UniqueName="PhoneNumber" HeaderText="Phone Number" DataField="PhoneNumber"
                    AllowEncodeOnExporting="true" ASFormat="StaticString" HeaderTooltip="Phone Number" ItemStyle-HorizontalAlign="Center"
                    SortExpression="PhoneNumber" meta:resourcekey="ASGridBoundColumnResource9">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn UniqueName="State" HeaderText="State" DataField="State" ItemStyle-HorizontalAlign="Center"
                    AllowEncodeOnExporting="true" ASFormat="StaticString" HeaderTooltip="State"
                    SortExpression="State" meta:resourcekey="ASGridBoundColumnResource10">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn UniqueName="Zip" HeaderText="Zip" DataField="Zip" ItemStyle-HorizontalAlign="Center"
                    AllowEncodeOnExporting="true" ASFormat="StaticString" HeaderTooltip="Zip"
                    SortExpression="Zip" meta:resourcekey="ASGridBoundColumnResource11">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
            </Columns>
        </MasterTableView>
    </as:ASGrid>

    <as:Validator ID="Validator1" runat="server" ValidationFunction="ValidateInput" MessageType="AlertBox" MessageContainerClientID="" meta:resourcekey="Validator1Resource1">
        <Items>
            <%--Merchant ID--%>
            <as:CustomValidationItem ControlToValidateID="uxMerchantNumValue" ClientValidationFunction="alphaNumberic" ResMessage="Resources.ValMsg.MerchantNumberNumericOnly" IsInAjaxPanel="true"/>
            <as:CustomValidationItem ControlToValidateID="uxMerchantNumValue" ClientValidationFunction="required3digit" meta:resourcekey="MerchantID_RequestLength3" IsInAjaxPanel="true"/>
            <%--Merchant Name--%>
            <as:CustomValidationItem ControlToValidateID="uxMerchantNameValue" ClientValidationFunction="required" ResMessage="Resources.ValMsg.MerchantNameRequiredMsg" IsInAjaxPanel="true"/>
            <as:CustomValidationItem ControlToValidateID="uxMerchantNameValue" ClientValidationFunction="specialCharacter" ResMessage="Resources.ValMsg.MerchantNameStringUnacceptMsg" IsInAjaxPanel="true"/>
        </Items>
    </as:Validator>

    <tek:RadCodeBlock ID="uxRadCodeBlock" runat="server">
        <script type="text/javascript">
            var uxFromDate_ClientID = "<%=uxFromDate.ClientID%>";
            var uxEndDate_ClientID = "<%=uxEndDate.ClientID%>";
            var uxRange_ClientID = "<%=uxRange.ClientID %>";
            var uxDate_ClientID = "<%= uxDate.ClientID %>";
            var Msg_V1 = "<%=Resources.MessageManager.ReportFilter_V1%>";
            var Msg_V9 = "<%= GetLocalResourceObject("NotInMifSummary_Js_EndGreaterToday").ToString()%>";
            var uxDaily_ClientID = "<%=uxDaily.ClientID %>";
            var uxMonthly_ClientID = "<%=uxMonthly.ClientID %>";
            var uxRange_ClientID = "<%=uxRange.ClientID %>";
            var uxSearch_ClientID = "<%=uxSearch.ClientID%>";
            var uxDaily = document.getElementById(uxDaily_ClientID);
            var uxMonthly = document.getElementById(uxMonthly_ClientID);
            var uxDateRange = document.getElementById(uxRange_ClientID);
            var Msg_V2 = "<%=Resources.MessageManager.ReportFilter_ReportDate_InvaidDate %>";
            var Msg_V3 = "<%=Resources.MessageManager.ReportFilter_V3%>";
            var Msg_V10 = "<%=Resources.MessageManager.ValidationMessages_V10%>";

            var uxHierarchyValue_ClientID = "<%= uxHierarchyValue.ClientID%>";
            var uxHierarchy_ClientID = "<%= uxHierarchy.ClientID%>";
            var uxMerchantNameValue_ClientID = "<%= uxMerchantNameValue.ClientID%>";
            var uxMerchantNumValue_ClientID = "<%= uxMerchantNumValue.ClientID%>";

            var uxDailyResource1_text = '<%= GetLocalResourceObject("uxDailyResource1.Text").ToString()%>';
            var uxMonthlyResource1_text = '<%= GetLocalResourceObject("uxMonthlyResource1.Text").ToString()%>';
            var uxRangeResource1_text = '<%= GetLocalResourceObject("uxRangeResource1.Text").ToString()%>';
             
        </script>
        <script src="<%= ResolveUrl("~/")%>res/js/MerchantNotInMifSummary.js"></script>
    </tek:RadCodeBlock>
</asp:Content>
