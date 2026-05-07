<%@ Control Language="C#" AutoEventWireup="true" CodeFile="rm_MCF_MerchantFraudReport.ascx.cs" Inherits="UserControls_rm_MCF_MerchantFraudReport" %>

<%@ Register Src="~/UserControls/UxExport.ascx" TagName="UxExport" TagPrefix="uc" %>
<style>
    .tbMerchantNumberHint::placeholder
    {
        font-style:italic;
    }
</style>
<tek:RadAjaxManagerProxy runat="server" ID="RadAjaxManager">
    <AjaxSettings>
        <tek:AjaxSetting AjaxControlID="uxReportGrid">
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="uxReportGrid" LoadingPanelID="uxLoadingPanelCustom" />
                <tek:AjaxUpdatedControl ControlID="uxExport" LoadingPanelID="uxLoadingPanelCustom" />
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
                        <div id="cidAssignmentOption" runat="server">
                            <div class="filter-item">
                                <as:RadioButton runat="server" ID="uxDaily" GroupName="dateFilterOption"
                                    onclick="merchantFraudFilter.changeDateOption(this)" CssClass="date-item first" meta:resourcekey="OptDaily" />
                                <as:RadioButton runat="server" ID="uxMonthly" GroupName="dateFilterOption"
                                    onclick="merchantFraudFilter.changeDateOption(this)" CssClass="date-item" meta:resourcekey="OptDayRolling" />
                                <as:RadioButton runat="server" ID="uxRange" GroupName="dateFilterOption"
                                    onclick="merchantFraudFilter.changeDateOption(this)" CssClass="date-item" meta:resourcekey="OptDateRange" />
                            </div>
                        </div>
                    </td>
                    <td class="text-left">
                        <div id="divDate">
                            <div class="filter-item">
                                <as:RadDatePicker ID="uxDate" runat="server" Width="128px">
                                    <Calendar FastNavigationStep="12" ShowRowHeaders="false" />
                                    <DateInput ID="DateInput1" runat="server" onclick="merchantFraudFilter.showCalendar('1')" />
                                </as:RadDatePicker>
                            </div>
                        </div>
                        <div id="divDateRange" style="display: none;">
                            <div class="filter-item">
                                <as:RadDatePicker ID="uxFromDate" runat="server" Width="128px">
                                    <Calendar FastNavigationStep="12" ShowRowHeaders="false" />
                                    <DateInput ID="DateInput2" runat="server" onclick="merchantFraudFilter.showCalendar('2')" />
                                </as:RadDatePicker>
                            </div>
                            <div class="filter-item">
                                <as:RadDatePicker ID="uxEndDate" runat="server" Width="128px">
                                    <Calendar FastNavigationStep="12" ShowRowHeaders="false" />
                                    <DateInput ID="DateInput3" runat="server" onclick="merchantFraudFilter.showCalendar('3')" />
                                </as:RadDatePicker>
                            </div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="text-right">
                        <label class="filter-label">
                            <asp:Literal ID="uxAcquirerListTitle" runat="server"></asp:Literal>
                        </label>
                        <div class="filter-item">
                            <as:RadComboBox ID="uxPaymentEntitieTypes" runat="server" EnableEmbeddedBaseStylesheet="False"
                                Width="180px" OnClientSelectedIndexChanged="uxPaymentEntitieTypes_OnClientSelectedIndexChanged">
                            </as:RadComboBox>
                        </div>
                    </td>
                    <td class="text-left">
                        <div class="filter-item">
                            <%--Search value input--%>
                            <as:RadComboBox ID="uxAcquirerList" runat="server" EnableEmbeddedBaseStylesheet="False"
                                DataValueField="DataKey" DataTextField="DataText" Width="370px"
                                AppendDataBoundItems="True" EnableLoadOnDemand="True"
                                OnItemsRequested="uxAcquirerList_OnItemsRequested" 
                                OnClientKeyPressing="radComboBox_OnClientKeyPressing"
                                OnClientTextChange="radComboBox_OnClientTextChange" 
                                OnClientSelectedIndexChanged="radComboBox_OnClientSelectedIndexChanged"
                                MaxLength="100">
                                <Items>
                                    <tek:RadComboBoxItem runat="server" meta:resourcekey="RadComboBoxItemResource1" />
                                </Items>
                            </as:RadComboBox>

                            <as:RadComboBox ID="uxMerchantList" runat="server" EnableEmbeddedBaseStylesheet="False"
                                DataValueField="DataKey" DataTextField="DataText" Width="370px"
                                AppendDataBoundItems="True" EnableLoadOnDemand="True"                                
                                OnItemsRequested="uxMerchantList_OnItemsRequested"
                                OnClientKeyPressing="radComboBox_OnClientKeyPressing"
                                OnClientTextChange="radComboBox_OnClientTextChange" 
                                OnClientSelectedIndexChanged="radComboBox_OnClientSelectedIndexChanged"
                                MaxLength="16">
                                <Items>
                                    <tek:RadComboBoxItem runat="server" meta:resourcekey="RadComboBoxItemResource1" />
                                </Items>
                            </as:RadComboBox>

                            <as:TextBox ID="uxMerchantNumber" runat="server" placeholder="enter at least 3 letters of the merchant numbers"                                
                                Text="" Width="370px" CssClass="rf_TextBox tbMerchantNumberHint" MaxLength="16"></as:TextBox>

                        </div>
                        <div class="filter-item">
                            <asp:Button ID="uxSearchButton" runat="server" OnClick="uxSearchButton_Click" OnClientClick="return merchantFraudFilter.validateData();" CssClass="btn btn-default" CausesValidation="False" meta:resourcekey="BtnSearch" />
                        </div>
                    </td>
                </tr>
            </table>
        </div>
    </div>
</div>
<div class="row">
    <div class="col-md-12 text-center" data-target=".report-filter-panel" data-toggle="collapse">
        <span class="btn btn-link btn-report-filter">
            <asp:Literal ID="rm_AcquirerFraudFilter_aspx_Filter" runat="server" meta:resourcekey="BtnFilter"></asp:Literal>
        </span>
    </div>
</div>

<uc:UxExport ID="uxExport" GridID="uxReportGrid" IsBottom="false" ShowCSV="true" ShowExcel="true" ShowPDF="false" ShowWord="false" runat="server" Visible="False" OnNeedExportConfig="uxExporter_NeedExportConfig" />
<as:PlaceHolder ID="uxPlActivationReport" runat="server">
    <div class="height-20"></div>
    <as:ASGrid ID="uxReportGrid" runat="server" AutoGenerateColumns="false" AllowPaging="true" IsCacheTemplateFile="false" IsAutoExportTemplate="true"
        AllowSorting="true" AllowExportAtWebServices="true" AllowSortFilterWhenExport="true"
        AllowFilteringByColumn="true" OnItemDataBound="uxReportGrid_ItemDataBound" OnNeedDataSource="uxReportGrid_NeedDataSource"
        ASPagingMethod="SPASingleMethod2" CssClass="in" meta:resourcekey="uxReportGridResource1"
        OnSortCommand="uxReportGrid_SortCommand"
        FilteringByColumnWithDataFieldAllow="true"
        FilteringByColumnWithDataFieldConfigFile="App_Data\FilteringByColumnWithDataFieldConfig\MerchantFraudReport.json"
        BuildFilterExpressionWithSquareBrackets="true"
        EnableBuildFilterExpressionEnhancement="true"
        EnableFilterItemsPersistence="true"
        EnableSortItemsPersistence="true"
        OnPreRender="uxReportGrid_PreRender"
        OnDataSourceReady="uxReportGrid_DataSourceReady">
        <MasterTableView>
            <Columns>
                <as:ASGridBoundColumn UniqueName="AcquirerID" DataField="AcquirerID" HeaderStyle-Width="220px" ItemStyle-Width="220px"
                    AllowEncodeOnExporting="true" ASFormat="DynamicString" ItemStyle-HorizontalAlign="Left"
                    SortExpression="AcquirerID" meta:resourcekey="GridColumnAcquirer">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>

                <as:ASGridBoundColumn UniqueName="MerchantNumber" DataField="MerchantNumber" HeaderStyle-Width="150px" ItemStyle-Width="150px"
                    AllowEncodeOnExporting="true" ASFormat="StaticString" ItemStyle-HorizontalAlign="Center"
                    SortExpression="MerchantNumber" meta:resourcekey="GridColumnMerchantNumber">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>

                <as:ASGridBoundColumn UniqueName="FraudAmount" DataField="FraudAmount" HeaderStyle-Width="220px" ItemStyle-Width="220px"
                    AllowEncodeOnExporting="true" ASFormat="Currency" ItemStyle-HorizontalAlign="Right" DataType="System.Decimal"
                    SortExpression="FraudAmount" meta:resourcekey="GridColumnFraudAmount" ASIsTotalColumn="true">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>

                <as:ASGridBoundColumn UniqueName="FraudVolumeRatio" DataField="FraudVolumeRatio" HeaderStyle-Width="220px" ItemStyle-Width="220px"
                    AllowEncodeOnExporting="true" ASFormat="Percentage" ItemStyle-HorizontalAlign="Right" DataType="System.Decimal"
                    SortExpression="FraudVolumeRatio" meta:resourcekey="GridColumnFraudRatio">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>

                <as:ASGridBoundColumn UniqueName="TransactionCount" DataField="TransactionCount" HeaderStyle-Width="220px" ItemStyle-Width="220px"
                    AllowEncodeOnExporting="true" ASFormat="Integer" ItemStyle-HorizontalAlign="Right" DataType="System.Int16"
                    SortExpression="TransactionCount" meta:resourcekey="GridColumnTransactionCount" ASIsTotalColumn="true">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>

                <as:ASGridBoundColumn UniqueName="Detail" DataField="Detail" HeaderStyle-Width="150px" ItemStyle-Width="150px"
                    AllowEncodeOnExporting="true" ASFormat="StaticString" ItemStyle-HorizontalAlign="Center" AllowFiltering="false"
                    AllowSorting="false" meta:resourcekey="GridColumnDetail">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>

            </Columns>
        </MasterTableView>
    </as:ASGrid>
</as:PlaceHolder>


<tek:RadCodeBlock ID="uxRadCodeBlock" runat="server">
    <script type="text/javascript">
        var uxFromDate_ClientID = "<%=uxFromDate.ClientID%>";
        var uxEndDate_ClientID = "<%=uxEndDate.ClientID%>";
        var uxDate_ClientID = "<%= uxDate.ClientID %>";
        var uxPaymentEntitieTypes_ClientID = '<%= uxPaymentEntitieTypes.ClientID %>';
        var uxAcquirerList_ClientID = '<%= uxAcquirerList.ClientID %>';
        var uxMerchantList_ClientID = '<%= uxMerchantList.ClientID %>';
        var uxMerchantNumber_ClientID = '<%= uxMerchantNumber.ClientID %>';

        var uxDaily_ClientID = "<%=uxDaily.ClientID %>";
        var uxMonthly_ClientID = "<%=uxMonthly.ClientID %>";
        var uxRange_ClientID = "<%=uxRange.ClientID %>";

        var uxDaily = document.getElementById(uxDaily_ClientID);
        var uxMonthly = document.getElementById(uxMonthly_ClientID);
        var uxDateRange = document.getElementById(uxRange_ClientID);

        var invalidDateMsg = "<%=Resources.MessageManager.ReportFilter_ReportDate_InvaidDate %>";
        var endDateMustBeGreaterThanFromDateMsg = "<%=Resources.MessageManager.ReportFilter_V3%>";
        var dateMustNotBeGreaterThanTodayMsg = "<%=Resources.MessageManager.ReportFilter_V10%>";
        var dateRangeNotExceedDayMsg = '<%= GetLocalResourceObject("DateRangeNotExceedDay").ToString()%>';
        var fieldMustNotIncludeScriptTagSpecialChars = "<%=Resources.MessageManager.FieldMustNotIncludeScriptTagSpecialChars %>";
        var validationMIDLessThan3Chars = '<%= GetLocalResourceObject("ValidationMIDLessThan3Chars").ToString() %>';
        var inValidationMessagesMerchantNumber = "<%=Resources.MessageManager.ValidationMessagesMerchantNumber %>";

        var uxReportGrid_ClientID = '<%=uxReportGrid.ClientID %>';
    </script>
    <script src="<%= ResolveUrl("~/")%>res/js/risk_MCF/rm_MCF_MerchantFraudReport.js"></script>
</tek:RadCodeBlock>