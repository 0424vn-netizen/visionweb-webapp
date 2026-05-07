<%@ Page Language="C#" AutoEventWireup="true" CodeFile="rm_MCF_RiskWorkLog.aspx.cs" Inherits="rm_MCF_RiskWorkLog" MasterPageFile="~/MasterPage.master" meta:resourcekey="PageResource1" %>

<%@ Register Src="~/UserControls/rm_MCF_RiskWorkLog_Filter.ascx" TagName="ReportFilter"
    TagPrefix="uc" %>
<%@ Register Src="~/UserControls/PageTitle.ascx" TagName="PageTitle" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/UxExport.ascx" TagName="UxExport" TagPrefix="uc" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="Server">
    <tek:RadAjaxManagerProxy runat="server" ID="uxRadAjaxManager">
        <AjaxSettings>
            <tek:AjaxSetting AjaxControlID="uxRiskWorkLog">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="uxRiskWorkLog" LoadingPanelID="uxLoadingPanelCustom" />
                </UpdatedControls>
            </tek:AjaxSetting>
        </AjaxSettings>
    </tek:RadAjaxManagerProxy>
    <as:PlaceHolder ID="uxPlContainer" runat="server">
        <uc:ReportFilter ID="uxReportFilter" runat="server" OnSubmitFiltering="uxReportFilter_SubmitFiltering"
            VisibleAssignmentFilter="true" VisibleParameterFilter="true" VisibleMerchantFilter="true"></uc:ReportFilter>

        <uc:UxExport ID="uxExporter" runat="server" GridID="uxRiskWorkLog" ShowPDF="false" ShowWord="false" IsOnTop="true" />
        <div class="row" runat="server" id="ux_RiskWorkLogTitle">
            <div class="col-xs-10">
                <h2 class="grid-title on-top">
                    <%=GetLocalResourceObject("uxPageTitleResource.Title") %>
                </h2>
            </div>
        </div>
        <as:ASGrid ID="uxRiskWorkLog" runat="server" AutoGenerateColumns="false" HeaderStyle-Width="80px"
            ASPagingMethod="SPASingleMethod" AllowSorting="true" AllowPaging="true" GridLines="None" AllowExportAtWebServices="false" IsAutoExportTemplate="true"
            OnInit="uxRiskWorkLog_Init" AllowCustomPaging="true" OnNeedDataSource="uxRiskWorkLog_NeedDataSource" ShowToolTip="true" CssClass="in" meta:resourcekey="uxRiskWorkLogGridResource2">
            <MasterTableView>
                <Columns>
                    <as:ASGridBoundColumn SortExpression="CreatedDTS" UniqueName="CreatedDTS" HeaderText="Date & Time" HeaderTooltip="Date & Time"
                        DataField="CreatedDTS" ASFormat="DateAndTime12Hours" HeaderStyle-Width="150px" meta:resourcekey="DateAndTimeResource">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center" Width="150px"></HeaderStyle>
                    </as:ASGridBoundColumn>

                    <as:ASGridBoundColumn SortExpression="UserNameFull" UniqueName="UserNameFull" HeaderText="User" HeaderTooltip="User"
                        DataField="UserNameFull" ASFormat="DynamicString" HeaderStyle-Width="90px" meta:resourcekey="UserResource">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center" Width="90px"></HeaderStyle>
                    </as:ASGridBoundColumn>

                    <as:ASGridBoundColumn SortExpression="ReportDate" UniqueName="ReportDate" HeaderText="Report Date" HeaderTooltip="Report Date"
                        DataField="ReportDate" ASFormat="Date" HeaderStyle-Width="120px" meta:resourcekey="ReportDateResource">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center" Width="120px"></HeaderStyle>
                    </as:ASGridBoundColumn>

                    <as:ASGridBoundColumn SortExpression="AssignmentName" UniqueName="AssignmentName" HeaderText="Assignment Name" HeaderTooltip="Assignment Name"
                        DataField="AssignmentName" ASFormat="DynamicString" HeaderStyle-Width="200px" meta:resourcekey="AssignmentNameResource">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center" Width="200px"></HeaderStyle>
                    </as:ASGridBoundColumn>

                    <as:ASGridBoundColumn SortExpression="ViewDesc" UniqueName="ViewDesc" HeaderText="View" HeaderTooltip="View"
                        DataField="ViewDesc" ASFormat="DynamicString" HeaderStyle-Width="110px" meta:resourcekey="ViewDescResource">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center" Width="110px"></HeaderStyle>
                    </as:ASGridBoundColumn>

                    <as:ASGridBoundColumn SortExpression="MerchantNumber" UniqueName="MerchantNumber" HeaderText="Merchant ID" HeaderTooltip="Merchant ID"
                        DataField="MerchantNumber" ASFormat="DynamicString" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="140px" meta:resourcekey="MerchantIDResource">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center" Width="140px"></HeaderStyle>
                    </as:ASGridBoundColumn>

                    <as:ASGridBoundColumn SortExpression="MerchantName" UniqueName="MerchantName" HeaderText="Merchant Name" HeaderTooltip="Merchant Name"
                        DataField="MerchantName" ASDefaultNullValue="—" ASFormat="DynamicString" HeaderStyle-Width="200px" meta:resourcekey="MerchantNameResource">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center" Width="200px"></HeaderStyle>
                    </as:ASGridBoundColumn>

                    <as:ASGridBoundColumn SortExpression="PreviousWorkStateCodeDesc" UniqueName="PreviousWorkStateCodeDesc" HeaderText="Previous Work State" HeaderTooltip="Previous Work State"
                        DataField="PreviousWorkStateCodeDesc" ASDefaultNullValue="—" ItemStyle-HorizontalAlign="Center" ASFormat="DynamicString" HeaderStyle-Width="100px" meta:resourcekey="PreviousWorkStateResource">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center" Width="100px"></HeaderStyle>
                    </as:ASGridBoundColumn>

                    <as:ASGridBoundColumn SortExpression="ActionCodeDesc" UniqueName="ActionCodeDesc" HeaderText="Action" HeaderTooltip="Action"
                        DataField="ActionCodeDesc" ASDefaultNullValue="—" ASFormat="DynamicString" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="100px" meta:resourcekey="ActionCodeDescResource">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center" Width="100px"></HeaderStyle>
                    </as:ASGridBoundColumn>

                    <as:ASGridBoundColumn SortExpression="NewWorkStateCodeDesc" UniqueName="NewWorkStateCodeDesc" HeaderText="New Work State" HeaderTooltip="New Work State"
                        DataField="NewWorkStateCodeDesc" ASDefaultNullValue="—" ASFormat="DynamicString" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="100px" meta:resourcekey="NewWorkStateCodeDescResource">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center" Width="100px"></HeaderStyle>
                    </as:ASGridBoundColumn>

                    <as:ASGridBoundColumn SortExpression="PreviousDisposition" UniqueName="PreviousDisposition" HeaderText="Previous Dispositions" HeaderTooltip="Previous Dispositions"
                        DataField="PreviousDisposition" ASDefaultNullValue="—" ASFormat="DynamicString" HeaderStyle-Width="100px" meta:resourcekey="PreviousDispositionResource">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center" Width="100px"></HeaderStyle>
                    </as:ASGridBoundColumn>


                    <as:ASGridBoundColumn SortExpression="NewDisposition" UniqueName="NewDisposition" HeaderText="New Dispositions" HeaderTooltip="New Dispositions"
                        DataField="NewDisposition" ASDefaultNullValue="—" ASFormat="DynamicString" HeaderStyle-Width="200px" meta:resourcekey="NewDispositionResource">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center" Width="200px"></HeaderStyle>
                    </as:ASGridBoundColumn>

                    <as:ASGridBoundColumn SortExpression="PreviousWQ" UniqueName="PreviousWQ" HeaderText="Previous WQ" HeaderTooltip="Previous WQ"
                        DataField="PreviousWQ" ASFormat="DynamicString" ASDefaultNullValue="—" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="120px" meta:resourcekey="PreviousWQResource">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center" Width="120px"></HeaderStyle>
                    </as:ASGridBoundColumn>

                    <as:ASGridBoundColumn SortExpression="NewWQ" UniqueName="NewWQ" HeaderText="New WQ" HeaderTooltip="New WQ"
                        DataField="NewWQ" ASDefaultNullValue="—" ASFormat="DynamicString" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="100px" meta:resourcekey="NewWQResource">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center" Width="100px"></HeaderStyle>
                    </as:ASGridBoundColumn>

                </Columns>
            </MasterTableView>
        </as:ASGrid>
    </as:PlaceHolder>
</asp:Content>

