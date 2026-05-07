<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Title="Merchant Profile"
    CodeFile="MerchantInformation.aspx.cs" Inherits="As.VisionWeb.Web.MerchantInformation" EnableEventValidation="false" meta:resourcekey="PageResource1" %>

<%@ Register Src="~/UserControls/UxExport.ascx" TagName="UxExport" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/UxExportQueue.ascx" TagName="UxExportQueue" TagPrefix="uc" %>
<%@ Register Src="UserControls/PageTitle.ascx" TagName="PageTitle" TagPrefix="uc" %>
<%@ Register Src="UserControls/ReportFiltering.ascx" TagName="ReportFiltering" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/MessageEditor.ascx" TagName="MessageEditor" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/MecchantProfileNavigator.ascx" TagPrefix="uc" TagName="MecchantProfileNavigator" %>
<%--TK39919 - Add Merchant Note--%>
<%@ Register Src="~/UserControls/MerchantNote.ascx" TagPrefix="uc" TagName="MerchantNote" %>
<%--44894 - VW- Merchant Note Default Preferences via User Mgmt Settings--%>
<%@ Register Src="~/UserControls/uxCaseHistory.ascx" TagPrefix="uc" TagName="CaseHistory" %>
<%@ Register Src="UserControls/MIF_MerchantDetails_Generic.ascx" TagName="MIF_MerchantDetails_Generic" TagPrefix="uc" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="Server">
    <tek:RadAjaxManagerProxy runat="server" ID="uxRadAjaxManager">
        <AjaxSettings>
            <tek:AjaxSetting AjaxControlID="uxGridCSRComment">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="uxGridCSRComment" />
                </UpdatedControls>
            </tek:AjaxSetting>
        </AjaxSettings>
        <AjaxSettings>
            <tek:AjaxSetting AjaxControlID="uxAddComment">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="uxGridCSRComment" />
                    <tek:AjaxUpdatedControl ControlID="uxExporterCommentTop" />
                </UpdatedControls>
            </tek:AjaxSetting>
        </AjaxSettings>
        <AjaxSettings>
            <tek:AjaxSetting AjaxControlID="uxMerchantListGrid">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="uxMerchantListGrid" />
                </UpdatedControls>
            </tek:AjaxSetting>
        </AjaxSettings>
    </tek:RadAjaxManagerProxy>

    <%--TK39919 - add action when filter--%>
    <uc:ReportFiltering ID="uxReportFiltering" runat="server" Mode="Mode1" OnFiltering="uxReportFiltering_Filtering" />

    <asp:PlaceHolder ID="uxPanelMerchantList" runat="server">
        <div class="row">
            <div class="col-xs-9">
                <uc:PageTitle ID="uxPageTitle2" runat="server" ReportTitle="" HasShowHierarchy="true"
                    HasFilteringOption="true" />

                <uc:PageTitle ID="uxPageTitleCM2" Visible="false" runat="server" PageTitle="Merchant Profile"
                    ReportTitle="Merchant Profile" meta:resourcekey="uxPageTitleCM2Resource1" />
            </div>
            <div class="col-xs-3 text-right">
                <asp:HyperLink ID="uxGoBack2" runat="server" NavigateUrl="#" Visible="false" Text=""
                    CssClass="link-back" meta:resourcekey="uxGoBack2Resource1" />
            </div>
        </div>
        <uc:UxExport ID="uxExportTop" GridID="uxMerchantListGrid" runat="server" IsOnTop="true" />
        <uc:UxExportQueue ID="uxExportQueueTop" GridID="uxMerchantListGrid" runat="server" IsOnTop="true" Visible="false" PageName="MerchantProfile" />
        <as:ASGrid ID="uxMerchantListGrid" runat="server" AllowPaging="True" AllowSorting="True" IsCacheTemplateFile="false"
            AutoGenerateColumns="False" AllowAutoCalculateTotalOnExport="false" AllowFilteringByColumn="false" OnPreRender="uxMerchantListGrid_PreRender"
            ASPagingMethod="SPASingleMethod" ShowPageTotal="false" ShowReportTotal="false" IsAutoExportTemplate="true"
            AllowSortFilterWhenExport="true" CssClass="in" meta:resourcekey="uxDrilldownGridResource1" InsertTempColumnAtTheEnd="false">
            <MasterTableView>
                <Columns>
                </Columns>
            </MasterTableView>
        </as:ASGrid>
    </asp:PlaceHolder>

    <asp:PlaceHolder ID="uxPanelDetail" runat="server" Visible="False">
        <div class="mif-container">
            <div class="mif-sidebar-container">
                <uc:MecchantProfileNavigator runat="server" ID="MecchantProfileNavigator" />
            </div>
            <div class="mif-content-container">
                <div class="row">
                    <div class="col-xs-9">
                        <uc:PageTitle ID="uxPageTitle" runat="server" ReportTitle="" HasShowHierarchy="true"
                            HasFilteringOption="true" HasMarginBottom="true" />

                        <uc:PageTitle ID="uxPageTitleCM" Visible="false" runat="server" PageTitle="Merchant Profile"
                            ReportTitle="Merchant Profile" HasMarginBottom="true" meta:resourcekey="uxPageTitleCMResource1" />
                    </div>
                    <div class="col-xs-3 text-right">
                        <asp:HyperLink ID="uxGoBack" runat="server" NavigateUrl="#" Visible="false" Text=""
                            CssClass="link-back" meta:resourcekey="uxGoBackResource1" />
                    </div>
                </div>

                <div class="pos-relative">
                    <div class="pull-right">
                        <as:Button ID="btnExportMulti" runat="server" OnClientClick="return ShowExportModal(this)" Text="Export Multi Section" CssClass="btn btn-default report-export" meta:resourcekey="btnExportMultiResource" />
                        <as:Button ID="btnExportMultiSections" runat="server" />
                        <as:HiddenField runat="server" ID="hdfExportMultiSections" />
                    </div>
                    <%-- MerchantDetail --%>
                    <uc:MIF_MerchantDetails_Generic ID="uxMIF_MerchantDetails_Generic" runat="server" />
                </div>
                <%-- MemoSection --%>
                <div class="row" id="memosection">
                    <div class="col-md-12">
                        <as:PlaceHolder runat="server" ID="uxMemoSection">
                            <uc:UxExport ID="uxExportMemolistTop" runat="server" GridID="uxMemoGrid" ShowPDF="false"
                                ShowCSV="false" GridHeader="Merchant Memos" ShowWord="false" ShowExcel="true" meta:resourcekey="uxExportMemolistTopResource1" />
                            <as:ASGrid ID="uxMemoGrid" runat="server" ASPagingMethod="SPASingleMethod" AutoGenerateColumns="false"
                                AllowPaging="True" AllowSorting="True" PageSize="10" ShowPageTotal="false" ShowReportTotal="false" CssClass="in" meta:resourcekey="uxMemoGridResource1">
                                <MasterTableView>
                                    <Columns>
                                        <as:ASGridBoundColumn SortExpression="DateLastActive" UniqueName="DateLastActive"
                                            HeaderText="Date Last Active" DataField="DateLastActive" ASFormat="Date" HeaderTooltip="Date Last Active"
                                            HeaderStyle-Width="100px" meta:resourcekey="DateLastActive">
                                            <ColumnValidationSettings>
                                                <ModelErrorMessage Text=""></ModelErrorMessage>
                                            </ColumnValidationSettings>

                                            <HeaderStyle HorizontalAlign="Center" Width="100px"></HeaderStyle>
                                        </as:ASGridBoundColumn>
                                        <as:ASGridBoundColumn SortExpression="SeqNo" UniqueName="SeqNo" HeaderText="Seq No"
                                            ASFormat="StaticString" DataField="SeqNo" HeaderTooltip="Seq No" meta:resourcekey="SeqNo">
                                            <ColumnValidationSettings>
                                                <ModelErrorMessage Text=""></ModelErrorMessage>
                                            </ColumnValidationSettings>

                                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                        </as:ASGridBoundColumn>
                                        <as:ASGridBoundColumn SortExpression="ClerkCode" UniqueName="ClerkCode" HeaderText="Clerk Code"
                                            ASFormat="StaticString" DataField="ClerkCode" HeaderTooltip="Clerk Code" meta:resourcekey="ClerkCode">
                                            <ColumnValidationSettings>
                                                <ModelErrorMessage Text=""></ModelErrorMessage>
                                            </ColumnValidationSettings>

                                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                        </as:ASGridBoundColumn>
                                        <as:ASGridBoundColumn SortExpression="MemoData" UniqueName="MemoData" HeaderText="Memo Data"
                                            ASFormat="DynamicString" DataField="MemoData" HeaderTooltip="Memo Data" ItemStyle-HorizontalAlign="Left" meta:resourcekey="MemoData">
                                            <ColumnValidationSettings>
                                                <ModelErrorMessage Text=""></ModelErrorMessage>
                                            </ColumnValidationSettings>

                                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </as:ASGridBoundColumn>
                                        <as:ASGridBoundColumn SortExpression="TimeEntered" UniqueName="TimeEntered" HeaderText="Time Entered"
                                            ASFormat="StaticString" DataField="TimeEntered" HeaderTooltip="Time Entered" meta:resourcekey="TimeEntered">
                                            <ColumnValidationSettings>
                                                <ModelErrorMessage Text=""></ModelErrorMessage>
                                            </ColumnValidationSettings>

                                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                        </as:ASGridBoundColumn>
                                        <as:ASGridBoundColumn SortExpression="TermID" UniqueName="TermID" HeaderText="Term ID"
                                            ASFormat="StaticString" DataField="TermID" HeaderTooltip="Term ID" meta:resourcekey="TermID">
                                            <ColumnValidationSettings>
                                                <ModelErrorMessage Text=""></ModelErrorMessage>
                                            </ColumnValidationSettings>

                                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                        </as:ASGridBoundColumn>
                                        <as:ASGridBoundColumn SortExpression="OpCode" UniqueName="OpCode" HeaderText="Op Code"
                                            DataField="OpCode" ASFormat="StaticString" HeaderTooltip="Op Code" meta:resourcekey="OpCode">
                                            <ColumnValidationSettings>
                                                <ModelErrorMessage Text=""></ModelErrorMessage>
                                            </ColumnValidationSettings>

                                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                        </as:ASGridBoundColumn>
                                        <as:ASGridBoundColumn SortExpression="LastAction" UniqueName="LastAction" HeaderText="Last Action"
                                            DataField="LastAction" ASFormat="StaticString" HeaderTooltip="Last Action" meta:resourcekey="LastAction">
                                            <ColumnValidationSettings>
                                                <ModelErrorMessage Text=""></ModelErrorMessage>
                                            </ColumnValidationSettings>

                                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                        </as:ASGridBoundColumn>
                                        <as:ASGridBoundColumn SortExpression="LastUpdated" UniqueName="LastUpdated" HeaderText="Last Updated"
                                            DataField="LastUpdated" ASFormat="Date" HeaderTooltip="Last Updated" HeaderStyle-Width="100px" meta:resourcekey="LastUpdated">
                                            <ColumnValidationSettings>
                                                <ModelErrorMessage Text=""></ModelErrorMessage>
                                            </ColumnValidationSettings>

                                            <HeaderStyle HorizontalAlign="Center" Width="100px"></HeaderStyle>
                                        </as:ASGridBoundColumn>
                                    </Columns>
                                </MasterTableView>
                            </as:ASGrid>
                        </as:PlaceHolder>
                    </div>
                </div>
                <%--TK39919 - Merchant note --%>
                <div class="row">
                    <div class="col-md-12">
                        <uc:MerchantNote ID="ucMerchantNote" runat="server" />
                    </div>
                </div>
                <asp:PlaceHolder ID="uxPanelCaseHistory" runat="server">
                    <div class="row" id="casehistory">
                        <div class="col-md-12">
                            <uc:CaseHistory runat="server" ID="ucCaseHistory" />
                        </div>
                    </div>
                </asp:PlaceHolder>

            </div>
        </div>
        <asp:Button ID="uxReloadHierachy" OnClick="uxReloadHierachy_Click" runat="server" CssClass="hide" />
    </asp:PlaceHolder>

    <!-- -->
    <div id="cidNonKeepFilterStatePage"></div>
    <% //CR #7485 - Dont mantain filter state of MIF page %>
    <script type="text/javascript">
        var uxSourceList = '<%= ucMerchantNote.FindControl("uxSourceList").ClientID%>'
        var uxRoleList = '<%= ucMerchantNote.FindControl("uxRoleList").ClientID%>'
        var uxAddedByList = '<%= ucMerchantNote.FindControl("uxAddedByList").ClientID%>'
        var uxStatuses = '<%= ucCaseHistory.FindControl("uxStatuses").ClientID%>'
        var uxTypes = '<%= ucCaseHistory.FindControl("uxTypes").ClientID%>'
        var uxPriorityLevel = '<%= ucCaseHistory.FindControl("uxPriorityLevel").ClientID%>'
        var AddNote_uxApplyFilter = '<%= ucMerchantNote.FindControl("uxApplyFilter").ClientID%>';
        var uxFinishSaveDefaultSettingCH = '<%= ucCaseHistory.FindControl("uxFinishSaveDefaultSettingCH").ClientID%>';
        var uxFinishSaveDefaultSettingMN = '<%= ucMerchantNote.FindControl("uxFinishSaveDefaultSettingMN").ClientID%>';
        var uxReloadHierachyId = '<%=uxReloadHierachy.ClientID%>';
        var currentPageURL = "<%=ResolveUrl("~/MerchantInformation.aspx") %>";
    </script>
    <script type="text/javascript" src="<%=ResolveUrl("~") %>res/js/MerchantProfile.js"></script>
    <script src="<% =ResolveUrl("~")%>res/js/common/sidenav.js"></script>

    <style>
        .RadGrid_Default .rgRow td,
        .RadGrid_Default .rgAltRow td {
            overflow-wrap: break-word;
        }
    </style>

</asp:Content>
