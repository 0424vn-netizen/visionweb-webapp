<%--Create by: Hao Dang
Ticket: 43784 Queue Enhancements --%>

<%@ Page Title="DETECTION QUEUE DETAILS" Language="C#" MasterPageFile="~/MasterPagePopup.master"
    AutoEventWireup="true" CodeFile="rm_MCF_DQReasonModal.aspx.cs" Inherits="As.VisionWeb.Web.RiskManagementDetectionQueueReasonModal" meta:resourcekey="PageResource1" %>

<%@ Register Src="~/UserControls/PageTitle.ascx" TagName="PageTitle" TagPrefix="uc" %>
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
    <as:PlaceHolder ID="dfd" runat="server">
        <uc:PageTitle ID="uxPageTitle" runat="server" meta:resourcekey="uxPageTitleResource1" ReportTitle="RISK MANAGEMENT - RISK ANALYSIS - DETECTION QUEUE DETAILS" Visible="False" />

        <div class="row">
            <div class="col-md-12">
                <h3 class="modal-title title-auto-queue font-35">
                    <as:Literal ID="ltrMerchantInfor" runat="server" meta:resourcekey="ltrMerchantInforResource1"></as:Literal>
                </h3>
            </div>
        </div>
        <div class="height-10"></div>
        <div class="row">
            <div class="col-xs-10">
                <h2 id="uxh2Export" runat="server" class="grid-title on-top" data-toggle="collapse"></h2>
            </div>
            <div class="col-xs-2">
                <div class="report-export dropdown pull-right" runat="server" id="divExport">
                    <a href="#" data-toggle="dropdown" data-hover="dropdown" class="dropdown-toggle" id="litExport" runat="server">
                        <as:Literal ID="ltExport" runat="server" Text="EXPORT" meta:resourcekey="lblExport"></as:Literal></a>
                    <ul class="dropdown-menu">
                        <li id="uxLiExcel" runat="server">
                            <asp:LinkButton ID="imgExcel" runat="server" OnClick="uxbtnExport_Click" OnClientClick="doResetTimeOut()" meta:resourcekey="imgExcelResource1">Excel</asp:LinkButton></li>
                    </ul>
                </div>
            </div>
        </div>
        <as:ASGrid ID="uxReportGrid" runat="server" AutoGenerateColumns="false" ASPagingMethod="None"
            AllowExportAtWebServices="false" AllowPaging="false" AllowSorting="true" CssClass="in" IsCacheTemplateFile="false"
            meta:resourcekey="uxReportGridResource1" Width="100%" IsAutoExportTemplate="true" OnItemDataBound="uxReportGrid_ItemDataBound">
            <MasterTableView>
                <Columns>
                    <as:ASGridTemplateColumn HeaderText="Wk" DataField="CheckBoxColumn" UniqueName="CheckBoxColumn" HeaderStyle-HorizontalAlign="Center"
                        ItemStyle-HorizontalAlign="Center" meta:resourcekey="ASGridBoundColumnResource01" AllowSorting="true" SortExpression="WorkStateID"
                        ItemStyle-Width="70px">
                        <ItemTemplate>
                            <asp:Label ID="uxlblWork" runat="server" Text=""></asp:Label>
                            <%--                            <as:ASMCFWorkContent runat="server" ID="workItem"></as:ASMCFWorkContent>--%>
                        </ItemTemplate>

                        <HeaderStyle HorizontalAlign="Center" Width="70px"></HeaderStyle>

                        <ItemStyle HorizontalAlign="Center" Width="70px"></ItemStyle>
                    </as:ASGridTemplateColumn>
                    <as:ASGridBoundColumn HeaderText="WK" DataField="CurrentStatusDesc" UniqueName="CurrentStatusDesc"
                        HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left" AllowSorting="true"
                        SortExpression="CurrentStatusDesc" ItemStyle-Wrap="true" meta:resourcekey="ASGridBoundColumnResource01"
                        Visible="false">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                        <ItemStyle HorizontalAlign="Center" Wrap="True"></ItemStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Alerted On" DataField="AlertDate" UniqueName="AlertDate"
                        HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" AllowSorting="true"
                        SortExpression="AlertDate" ASFormat="DateTimeShortTime" ItemStyle-Wrap="true" meta:resourcekey="ASGridBoundColumnResource02">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                        <ItemStyle HorizontalAlign="Center" Wrap="True"></ItemStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="AssignmentName" DataField="AssignmentName" HeaderStyle-Width="150px" UniqueName="AssignmentName"
                        HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left" AllowSorting="true"
                        SortExpression="AssignmentName" ItemStyle-Wrap="true" meta:resourcekey="ASGridBoundColumnResource03">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                        <ItemStyle HorizontalAlign="Left" Wrap="True" CssClass="word-wrapped"></ItemStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="P#" DataField="ParameterKey" UniqueName="ParameterKey"
                        HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left" AllowSorting="false"
                        SortExpression="ParameterName" ItemStyle-Wrap="true" meta:resourcekey="ASGridBoundColumnResource04">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                        <ItemStyle HorizontalAlign="Left" Wrap="True"></ItemStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Parameter Name" DataField="Parameter" UniqueName="Parameter"
                        HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left" AllowSorting="false"
                        SortExpression="ParameterName" ItemStyle-Wrap="true" meta:resourcekey="ASGridBoundColumnResource05">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                        <ItemStyle HorizontalAlign="Left" Wrap="True"></ItemStyle>
                    </as:ASGridBoundColumn>

                    <as:ASGridBoundColumn HeaderText="Re-Alert" DataField="ReAlert"
                        SortExpression="ReAlert" UniqueName="ReAlert" HeaderStyle-HorizontalAlign="Center" ASFormat="DynamicString"
                        ItemStyle-HorizontalAlign="Right" ASDefaultNullValue="N/A" AllowSorting="false"
                        EmptyDataText="N/A" meta:resourcekey="ASGridBoundColumnResource14">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </as:ASGridBoundColumn>

                    <as:ASGridBoundColumn HeaderText="Parameter Indicator" DataField="ParameterValue"
                        SortExpression="ParameterValue" UniqueName="ParameterValue" HeaderStyle-HorizontalAlign="Center"
                        ItemStyle-HorizontalAlign="Right" ASDefaultNullValue="N/A" AllowSorting="false"
                        EmptyDataText="N/A" meta:resourcekey="ASGridBoundColumnResource06">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Actual Indicator" DataField="ActualValue" SortExpression="ActualValue"
                        AllowSorting="false" HeaderStyle-HorizontalAlign="Center" UniqueName="ActualValue"
                        ItemStyle-HorizontalAlign="Right" EmptyDataText="N/A" meta:resourcekey="ASGridBoundColumnResource07">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Parameter Threshold" DataField="ParameterThreshold"
                        AllowSorting="false" SortExpression="ParameterThreshold" UniqueName="ParameterThreshold"
                        HeaderStyle-HorizontalAlign="Center" EmptyDataText="N/A"
                        ItemStyle-HorizontalAlign="Right" meta:resourcekey="ASGridBoundColumnResource08">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Actual Threshold" DataField="ActualThreshold"
                        AllowSorting="false" SortExpression="ActualThreshold" UniqueName="ActualThreshold"
                        HeaderStyle-HorizontalAlign="Center" EmptyDataText="N/A"
                        ItemStyle-HorizontalAlign="Right" meta:resourcekey="ASGridBoundColumnResource09">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn UniqueName="Disposition" HeaderText="Disposition" DataField="Disposition"
                        ASFormat="StaticString" AllowSorting="true" meta:resourcekey="ASGridBoundColumnResource10">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Worked On" DataField="WorkedDate" UniqueName="WorkedDate"
                        HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" AllowSorting="true"
                        SortExpression="WorkedDate" ASFormat="DateTimeShortTime" ItemStyle-Wrap="true" meta:resourcekey="ASGridBoundColumnResource11">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                        <ItemStyle HorizontalAlign="Center" Wrap="True"></ItemStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="User Name" DataField="UserName" UniqueName="UserName"
                        HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left" AllowSorting="true"
                        SortExpression="UserName" ASFormat="StaticString" ItemStyle-Wrap="true" meta:resourcekey="ASGridBoundColumnResource12">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                        <ItemStyle HorizontalAlign="Left" Wrap="True"></ItemStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="File Type" DataField="FileType" UniqueName="FileType"
                        HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left" AllowSorting="false"
                        SortExpression="FileType" ASFormat="StaticString" ItemStyle-Wrap="true" meta:resourcekey="ASGridBoundColumnResource13">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                        <ItemStyle HorizontalAlign="Left" Wrap="True"></ItemStyle>
                    </as:ASGridBoundColumn>
                </Columns>
            </MasterTableView>
        </as:ASGrid>
    </as:PlaceHolder>
    <as:ASRadCodeBlock ID="uxRadCode" runat="server">
        <script type="text/javascript">
            var IsIEBrowser = "<%=GeneralFuncsLib.GetIEBrowserMode().ToString()%>";
        </script>
        <script type="text/javascript" src="<%=ResolveUrl("~") %>res/js/risk_MCF/rm_MCF_DQReasonModal.js"></script>
    </as:ASRadCodeBlock>
</asp:Content>
