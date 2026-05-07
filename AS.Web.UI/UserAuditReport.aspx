<%@ Page Title="User Audit Report" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="UserAuditReport.aspx.cs" Inherits="UserAuditReport" meta:resourcekey="PageResource1" %>

<%@ Register Src="~/UserControls/UxExport.ascx" TagName="UxExport" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/PageTitle.ascx" TagName="PageTitle" TagPrefix="uc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="Server">

    <tek:RadAjaxManagerProxy runat="server" ID="uxRadAjaxManager">
        <AjaxSettings>
            <tek:AjaxSetting AjaxControlID="uxReportGrid">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="uxReportGrid" LoadingPanelID="uxLoadingPanelCustom" />
                </UpdatedControls>
            </tek:AjaxSetting>
        </AjaxSettings>
    </tek:RadAjaxManagerProxy>
    <!--Filtering Options-->
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
                                    <DateInput ID="DateInput1" runat="server" onclick="showCalendar('1')" onkeypress="return SearchEnterOnTextbox(event);" />
                                </as:RadDatePicker>
                            </div>
                            <div id="divDateRange" style="display: none;">
                                <div class="filter-item">
                                    <as:RadDatePicker ID="uxFromDate" runat="server" Width="128px">
                                        <Calendar FastNavigationStep="12" ShowRowHeaders="false" />
                                        <DateInput ID="DateInput2" runat="server" onclick="showCalendar('2')" onkeypress="return SearchEnterOnTextbox(event);" />
                                    </as:RadDatePicker>
                                </div>
                                <div class="filter-item">
                                    <as:RadDatePicker ID="uxEndDate" runat="server" Width="128px">
                                        <Calendar FastNavigationStep="12" ShowRowHeaders="false" />
                                        <DateInput ID="DateInput3" runat="server" onclick="showCalendar('3')" onkeypress="return SearchEnterOnTextbox(event);" />
                                    </as:RadDatePicker>
                                </div>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div class="filter-item text-right mr-5x">
                                <as:Literal ID="ltChangedEntity" runat="server" Text="Changed Entity:" meta:resourcekey="ltChangedEntity"></as:Literal>
                                <div class="date-item ml-10">
                                    <as:RadioButton ID="uxRdEqual" runat="server" Text="Equal To" GroupName="ChangeEntitySearch" CssClass="" meta:resourcekey="uxSearchEqualResource1" Value="Equal" />
                                    <as:RadioButton ID="uxRdContain" runat="server" Text="Contains" GroupName="ChangeEntitySearch" Checked="true" CssClass="date-item mr-10" meta:resourcekey="uxSearchContainResource1" Value="Contains" />
                                </div>

                                <as:TextBox ID="uxChangedEntity" MaxLength="255" runat="server" Width="248px" CssClass="rf_TextBox" MaxHeight="200px"></as:TextBox>
                            </div>
                        </td>
                        <td class="text-right">
                            <div class="filter-item">
                                <as:Literal ID="ltChangedBy" runat="server" Text="Changed By:" meta:resourcekey="ltChangedBy"></as:Literal>
                                <as:RadComboBox ID="uxAdministratorName" runat="server" EnableEmbeddedBaseStylesheet="False"
                                    DataValueField="DataKey" DataTextField="DataText" Width="300px"
                                    AppendDataBoundItems="True" EnableLoadOnDemand="True" Filter="Contains" MarkFirstMatch="true"
                                    EmptyMessage="Enter at least 3 characters." OnItemsRequested="uxAdministratorName_ItemsRequested" ShowDropDownOnTextboxClick="true"
                                    MaxLength="200" OnClientKeyPressing="OnClientKeyPressing" meta:resourcekey="uxChangeByResource1">
                                    <Items>
                                        <tek:RadComboBoxItem runat="server" meta:resourcekey="RadComboBoxItemResource1" />
                                    </Items>
                                </as:RadComboBox>
                            </div>
                        </td>
                        <td class="text-left">
                            <div class="filter-item">
                                <as:Button ID="uxSearch" runat="server" Text="Search" OnClientClick="return ValidateData();"
                                    OnClick="uxSearch_Click" CssClass="btn btn-default" IsStandardButton="False" meta:resourcekey="uxSearchResource1" />
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
                <as:Literal ID="Literal1" runat="server" Text="FILTER" meta:resourcekey="Literal1Resource1"></as:Literal></span>
        </div>
    </div>
    <uc:PageTitle ID="uxPageTitle" runat="server" HasFilteringOption="true" ReportTitle="User Audit Report" meta:resourcekey="uxPageTitleResource1" />
    <as:PlaceHolder ID="uxViewMessagePlaceHolder" runat="server">
        <uc:UxExport ID="uxExporter" runat="server" IsOnTop="true" GridID="uxReportGrid" />
        <as:ASGrid ID="uxReportGrid" runat="server" AutoGenerateColumns="false" AllowPaging="true" IsAutoExportTemplate="true"
            AllowSorting="true" ASPagingMethod="SPASingleMethod" CssClass="in" meta:resourcekey="uxReportGridResource1" OnItemDataBound="uxGridUserAuditReport_OnItemDataBound">
            <MasterTableView>
                <Columns>
                    <as:ASGridBoundColumn UniqueName="ReportDate" HeaderText="Date/Time" DataField="ReportDate"
                        HeaderStyle-Width="150px" ASFormat="DateAndTime12Hours" HeaderTooltip="Date/Time"
                        SortExpression="ReportDate" meta:resourcekey="ASGridBoundColumnResource1">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center" Width="150px"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn UniqueName="ChangedByUser" HeaderText="Changed By" DataField="ChangedByUser"
                        AllowEncodeOnExporting="true" ASFormat="DynamicString" HeaderTooltip="Changed By"
                        SortExpression="ChangedByUser" meta:resourcekey="ASGridBoundColumnResource2">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn UniqueName="ChangedByUserRole" HeaderText="Changed By User Role"
                        DataField="ChangedByUserRole"
                        HeaderTooltip="Changed By User Role" SortExpression="ChangedByUserRole" ASFormat="StaticString"
                        AllowEncodeOnExporting="true" meta:resourcekey="ASGridBoundColumnResource3">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn UniqueName="ChangedUserName" HeaderText="Changed Entity" DataField="ChangedUserName" ItemStyle-CssClass="word-break"
                        HeaderTooltip="Changed Entity" SortExpression="ChangedUserName" ASFormat="StaticString" ItemStyle-HorizontalAlign="Left"
                        AllowEncodeOnExporting="true" meta:resourcekey="ASGridBoundColumnResource4">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn UniqueName="ChangedFirstName" HeaderText="First Name" DataField="ChangedFirstName"
                        HeaderTooltip="First Name" SortExpression="ChangedFirstName" ASFormat="DynamicString"
                        AllowEncodeOnExporting="true" meta:resourcekey="ASGridBoundColumnResource5">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn UniqueName="ChangedLastName" HeaderText="Last Name" DataField="ChangedLastName"
                        HeaderTooltip="Last Name" SortExpression="ChangedLastName" ASFormat="DynamicString"
                        AllowEncodeOnExporting="true" meta:resourcekey="ASGridBoundColumnResource6">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn UniqueName="ChangeType" HeaderText="Change Type" DataField="ChangeType"
                        HeaderTooltip="Change Type" SortExpression="ChangeType" ASFormat="DynamicString"
                        AllowEncodeOnExporting="true" meta:resourcekey="ASGridBoundColumnResource7">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn UniqueName="FieldName" HeaderText="Field" DataField="FieldName"
                        HeaderTooltip="Field" SortExpression="FieldName" ASFormat="DynamicString"
                        AllowEncodeOnExporting="true" meta:resourcekey="ASGridBoundColumnResource8">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <%--                 <as:ASGridBoundColumn UniqueName="OldValue" HeaderText="Old Value" DataField="OldValue"
                        HeaderTooltip="Old Value" SortExpression="OldValue" ASFormat="DynamicString"
                        AllowEncodeOnExporting="true" meta:resourcekey="ASGridBoundColumnResource9">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>--%>
                    <as:ASGridTemplateColumn UniqueName="OldValue" HeaderTooltip="Old Value" meta:resourcekey="ASGridBoundColumnResource9"
                        HeaderText="Old Value" DataField="OldValue" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="border-right">
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        <ItemTemplate>
                            <as:Literal ID="OldValue" runat="server"></as:Literal>
                        </ItemTemplate>
                    </as:ASGridTemplateColumn>
                    <%--     <as:ASGridBoundColumn UniqueName="NewValue" ItemStyle-Wrap="true" HeaderText="New Value"
                        DataField="NewValue" ItemStyle-CssClass="word-break"
                        HeaderTooltip="Old Value" SortExpression="NewValue" ASFormat="DynamicString"
                        AllowEncodeOnExporting="true" meta:resourcekey="ASGridBoundColumnResource10">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                        <ItemStyle Wrap="True">
                        </ItemStyle>
                    </as:ASGridBoundColumn>--%>
                    <as:ASGridTemplateColumn UniqueName="NewValue" HeaderTooltip="" meta:resourcekey="ASGridBoundColumnResource10"
                        HeaderText="New Value" DataField="NewValue" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="border-right">
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        <ItemTemplate>
                            <as:Literal ID="NewValue" runat="server"></as:Literal>
                        </ItemTemplate>
                    </as:ASGridTemplateColumn>
                </Columns>
            </MasterTableView>
        </as:ASGrid>
    </as:PlaceHolder>
    <as:Validator ID="Validator1" runat="server" ValidationFunction="ValidateInput" MessageType="AlertBox" MessageContainerClientID="" meta:resourcekey="Validator1Resource1">
        <Items>
            <as:CustomValidationItem ResMessage="Resources.ValMsg.ChangedEntity"
                ClientValidationFunction="validateSpecialCharacterChangedEntity"
                ControlToValidateID="uxChangedEntity" />
             <as:CustomValidationItem ResMessage="Resources.ValMsg.ChangedBy"
                ClientValidationFunction="validateSpecialCharacterChangedBy"
                ControlToValidateID="uxAdministratorName" />
        </Items>
    </as:Validator>
    <tek:RadCodeBlock ID="uxRadCodeBlock" runat="server">
        <script type="text/javascript">
            var uxFromDate_ClientID = "<%=uxFromDate.ClientID%>";
            var uxEndDate_ClientID = "<%=uxEndDate.ClientID%>";
            var uxRange_ClientID = "<%=uxRange.ClientID %>";
            var uxChangedEntity_ClientID = "<%= uxChangedEntity.ClientID %>";
            var uxAdministratorName_ClientID = "<%= uxAdministratorName.ClientID %>";
            var uxDate_ClientID = "<%= uxDate.ClientID %>";
            var Msg_V1 = "<%=Resources.MessageManager.ReportFilter_V1%>";
            var Msg_V9 = "<%=Resources.MessageManager.ReportFilter_V9%>";
            var uxDaily_ClientID = "<%=uxDaily.ClientID %>";
            var uxMonthly_ClientID = "<%=uxMonthly.ClientID %>";
            var uxRange_ClientID = "<%=uxRange.ClientID %>";
            var uxSearch_ClientID = "<%=uxSearch.ClientID%>";
            var uxDaily = document.getElementById(uxDaily_ClientID);
            var uxMonthly = document.getElementById(uxMonthly_ClientID);
            var uxDateRange = document.getElementById(uxRange_ClientID);
            var Msg_V2 = "<%=Resources.MessageManager.ReportFilter_ReportDate_InvaidDate %>";
            var Msg_V3 = "<%=Resources.MessageManager.ReportFilter_V3%>";

            var uxDailyResource1_text = '<%= GetLocalResourceObject("uxDailyResource1.Text").ToString()%>';
            var uxMonthlyResource1_text = '<%= GetLocalResourceObject("uxMonthlyResource1.Text").ToString()%>';
            var uxRangeResource1_text = '<%= GetLocalResourceObject("uxRangeResource1.Text").ToString()%>';
            var uxAdministratorName_ClientID = '<%= uxAdministratorName.ClientID%>';
        </script>
        <script src="<%= ResolveUrl("~/")%>res/js/usermaintenance/UserAuditReport.js"></script>
        <script src="<%= ResolveUrl("~/")%>res/js/common/common.js"></script>
    </tek:RadCodeBlock>
</asp:Content>

