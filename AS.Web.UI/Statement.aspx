<%@ Page Title="Statements" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="Statement.aspx.cs" Inherits="As.VisionWeb.Web.Statement" meta:resourcekey="PageResource1" %>

<%@ Register TagName="ReportingFiltering" Src="~/UserControls/ReportFiltering.ascx"
    TagPrefix="uc" %>
<%@ Register TagName="PageTitle" Src="~/UserControls/PageTitle.ascx" TagPrefix="uc" %>
<%@ Register TagName="UxExport" Src="~/UserControls/UxExport.ascx" TagPrefix="uc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="Server">
    <uc:ReportingFiltering ID="uxReportFiltering" runat="server" />
    <uc:PageTitle ID="idTitle" ReportTitle="Statements" runat="server" HasFilteringOption="true" meta:resourcekey="idTitleResource1" />
    <div class="chainStatements" id="uxPlaceHolderStatementName" runat="server" visible="false">
        <uc:UxExport ID="uxExportDrillDownForAllStatements" runat="server" GridID="uxGridAllOfStatement"
            IsOnTop="true" />
        <as:ASGrid ID="uxGridAllOfStatement" runat="server" AllowPaging="true" GridLines="None" IsAutoExportTemplate="true" IsCacheTemplateFile="false"
            ASPagingMethod="SPASingleMethod1" OnDataSourceReady="uxGridAllOfStatement_DataSourceReady" CssClass="in" meta:resourcekey="uxFileGridAllOfStatementResource" AutoGenerateColumns="false" OnItemDataBound="uxGridAllOfStatement_OnItemDataBound">
            <MasterTableView>
                <PagerStyle AlwaysVisible="true" />
                <Columns>
                    <as:ASGridTemplateColumn HeaderText="Chain ID" DataField="EntityNumber"
                        UniqueName="EntityNumber" HeaderTooltip="Chain ID" meta:resourcekey="ASGridBoundColumnResourceChainID">
                        <ItemTemplate>
                            <as:Literal ID="lblViewIdChain" runat="server"></as:Literal>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Center" Width="600px"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Center" />
                    </as:ASGridTemplateColumn>
                    <as:ASGridTemplateColumn HeaderText="Last Statement" DataField="LastStatement"
                        UniqueName="LastStatement" HeaderTooltip="Last Statement" meta:resourcekey="ASGridBoundColumnResourceLastStatement">
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        <ItemTemplate>
                            <as:Literal ID="lblLastStatement" runat="server"></as:Literal>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Center" />
                    </as:ASGridTemplateColumn>
                </Columns>
            </MasterTableView>
        </as:ASGrid>
        <br />
    </div>
    <div class="chainID" id="uxPlaceIDChainName" runat="server" visible="false">
        <div>
            <h2><span class="chainIDTitle"
                id="uxTitleChainStatement" runat="server"></span></h2>
        </div>
        <as:PlaceHolder ID="PlaceHolder1" runat="server">
            <div class="row">
                <div class="col-md-12">
                    <div class="form-inline dark-blue">
                        <label class="valign-middle">
                            <as:Literal ID="Literal3" runat="server" Text="Report Date:" meta:resourcekey="ltReportDateResource1"></as:Literal>
                        </label>
                        <as:RadComboBox ID="cbChainStatement" runat="server" Width="230px" meta:resourcekey="uxReportDateResource1">
                        </as:RadComboBox>
                        <as:Button runat="server" ID="btnViewChain" Text="View Statement" CssClass="btn btn-default"
                            OnClick="btnViewChain_Click" meta:resourcekey="btnSearchResource1" />
                    </div>
                </div>
            </div>
        </as:PlaceHolder>
        <br />
    </div>
    <div id="divNoChainStatement" visible="false" runat="server">
        <div>
            <h2>
                <span id="uxTitleChainNoData" runat="server" class="chainIDTitle"></span>
            </h2>
        </div>
        <as:PlaceHolder runat="server" ID="PlaceHolder2">
            <div class="row">
                <div class="col-md-12">
                    <table class="ASTable">
                        <tr>
                            <td>
                                <as:Literal ID="Literal4" runat="server" Text="There are no statements available at this time." meta:resourcekey="uxMessageResource1"></as:Literal>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </as:PlaceHolder>
        <br />
    </div>
    <div runat="server" id="uxViewChainStatementPanel" visible="true">
        <as:LinkButton runat="server" ID="uxViewChainStatement" Text="View Chain Statement" meta:resourcekey="uxViewChainStatementResource1"></as:LinkButton>
    </div>
    <as:PlaceHolder runat="server" ID="uxReportGridPanel">
        <div class="row" runat="server" id="divHeaderDetail">
            <div class="col-md-12">
                <h2 class="grid-title on-top">
                    <span class="text-muted">
                        <asp:Label ID="uxHeaderDetail" runat="Server" meta:resourcekey="uxHeaderDetailResource1"></asp:Label></span>
                </h2>
            </div>
        </div>
        <div class="height-10"></div>
        <as:PlaceHolder ID="pnlStatement" runat="server">
            <div class="row">
                <div class="col-md-12">
                    <div class="form-inline dark-blue">
                        <label class="valign-middle">
                            <as:Literal ID="ltReportDate" runat="server" Text="Report Date:" meta:resourcekey="ltReportDateResource1"></as:Literal>
                        </label>
                        <%-- 44353 - MPS Conversion - Phase 5 -  9/13/2018 --%>
                        <as:RadComboBox ID="uxReportDate" runat="server" Width="230px" meta:resourcekey="uxReportDateResource1">
                        </as:RadComboBox>
                        <as:Button runat="server" ID="btnSearch" Text="View Statement" CssClass="btn btn-default"
                            OnClick="btnSearch_Click" meta:resourcekey="btnSearchResource1" />
                    </div>
                </div>
            </div>
        </as:PlaceHolder>
        <as:PlaceHolder runat="server" ID="uxPlaceMss" Visible="false">
            <div class="row">
                <div class="col-md-12">
                    <table class="ASTable">
                        <tr>
                            <td>
                                <as:Literal ID="uxMessage" runat="server" Text="There are no statements available at this time." meta:resourcekey="uxMessageResource1"></as:Literal>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </as:PlaceHolder>
    </as:PlaceHolder>
    <!-- ++ Merchant Name ++ -->
    <div runat="server" id="uxPlaceHolderMerchantName" visible="true">
        <uc:UxExport ID="uxExportDrilldownGridTop" runat="server" GridID="uxDrilldownGrid"
            IsOnTop="true" />
        <as:ASGrid ID="uxDrilldownGrid" runat="server" AllowPaging="true" GridLines="None" IsAutoExportTemplate="true"
            AutoGenerateColumns="false" ASPagingMethod="SPASingleMethod" CssClass="in" meta:resourcekey="uxDrilldownGridResource1">
            <MasterTableView>
                <PagerStyle AlwaysVisible="true" />
                <Columns>
                    <as:ASGridBoundColumn HeaderStyle-HorizontalAlign="Center" HeaderText="Merchant ID"
                        DataField="Entity" UniqueName="DrilldownColumn" HeaderTooltip="Merchant ID"
                        SortExpression="Entity" ASFormat="StaticString" HeaderStyle-Width="140px" meta:resourcekey="ASGridBoundColumnResource1">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center" Width="140px"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderStyle-HorizontalAlign="Center" HeaderText="Merchant Name"
                        DataField="EntityName" UniqueName="MerchantName" SortExpression="EntityName"
                        ASFormat="DynamicString" HeaderTooltip="Merchant Name" meta:resourcekey="ASGridBoundColumnResource2">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderStyle-HorizontalAlign="Center" HeaderText="Location Address"
                        DataField="Address" UniqueName="Address" HeaderTooltip="Location Address"
                        ASFormat="DynamicString" SortExpression="Address" meta:resourcekey="ASGridBoundColumnResource3">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn UniqueName="MerchantStatus" HeaderText="Merchant Status"
                        HeaderTooltip="Merchant Status" DataField="MerchantStatus" SortExpression="MerchantStatus"
                        ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource4">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Status" DataField="Status" UniqueName="Status"
                        HeaderStyle-Width="80px" HeaderTooltip="MS Site Access Status"
                        ASFormat="StaticString" SortExpression="Status" meta:resourcekey="ASGridBoundColumnResource5">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center" Width="80px"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn UniqueName="LastStatementExport" HeaderStyle-HorizontalAlign="Center"  SortExpression="LastStatementExport"
                        HeaderText="Last Statement " meta:resourcekey="ASGridBoundColumnResourceStatement" HeaderTooltip="Last Statement Export" DataField="LastStatementExport">
                    </as:ASGridBoundColumn>
                    <as:ASGridTemplateColumn UniqueName="ViewStatement" HeaderStyle-HorizontalAlign="Center" SortExpression="LastStatement"
                        HeaderText="Last Statement" HeaderTooltip="Last Available Statement" DataField="LastStatement"
                        ASExportFormat="Date" meta:resourcekey="ASGridBoundColumnResourceStatement">
                        <ItemTemplate>
                            <as:LinkButton ID="lbtViewStatement" runat="server" OnCommand="btnViewStatement_OnCommand"></as:LinkButton>
                            <as:Literal ID="ltViewStatement" runat="server" Visible="false"></as:Literal>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Center" />
                    </as:ASGridTemplateColumn>
                </Columns>
            </MasterTableView>
        </as:ASGrid>
    </div>

    <div runat="server" id="divdownload">
        <h2 class="grid-title">
            <as:Literal ID="Literal2" runat="server" meta:resourcekey="lblDownLoadAll"></as:Literal>
        </h2>

        <div class="row mb-3x">
            <div class="col-md-2 w-10">
                <as:Literal ID="Literal1" runat="server" meta:resourcekey="lblReportDate"></as:Literal>
            </div>
            <div class="col-md-4">
                <as:RadComboBox runat="server" ID="cbbReportDate" CssClass="mr-0" />
                <as:Button runat="server" meta:resourcekey="btnDownLoad" UseSubmitBehavior="False" OnClientClick="return onDownLoadAll()" CssClass="btn btn-default ml-3x" />
                <as:Button runat="server" ID="btnDownLoad" CssClass="display-none" OnClick="btnDownLoad_Click" />
                <as:Button runat="server" ID="btnRefreshGrid" CssClass="display-none" OnClick="btnRefreshGrid_Click" />
            </div>
        </div>
        <as:ASGrid ID="uxGridStatement" OnItemCommand="uxGridStatement_OnItemCommand" runat="server" AllowPaging="true" GridLines="None"
            OnNeedDataSource="uxGridStatement_OnNeedDataSource" meta:resourcekey="uxFileGridResource1" IsAutoExportTemplate="true"
            AutoGenerateColumns="false" ASPagingMethod="SPASingleMethod" CssClass="in" OnItemDataBound="uxGridStatement_OnItemDataBound">
            <MasterTableView>
                <PagerStyle AlwaysVisible="true" />
                <Columns>
                    <as:ASGridTemplateColumn UniqueName="FileName1" HeaderStyle-HorizontalAlign="Center" meta:resourcekey="ASGridBoundColumnFileName"
                        HeaderText="File Name" HeaderTooltip="File Name" DataField="FileName" SortExpression="FileName">
                        <ItemTemplate>
                            <as:Literal ID="lblDownLoad" runat="server"></as:Literal>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="left" />
                    </as:ASGridTemplateColumn>

                    <as:ASGridBoundColumn DataField="ServerDocID" UniqueName="ServerDocID" Display="False"
                        ItemStyle-HorizontalAlign="Center">
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="RecordID" UniqueName="RecordID" Display="False"
                        ItemStyle-HorizontalAlign="Center">
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="FileName" UniqueName="FileName" Display="False"
                        ItemStyle-HorizontalAlign="Center">
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>

                    <as:ASGridBoundColumn DataField="TotalMerchant" UniqueName="TotalMerchant" HeaderTooltip="Total Merchants" meta:resourcekey="ASGridBoundColumnMerchant"
                        HeaderText="# Merchants" ItemStyle-HorizontalAlign="Center" AllowSorting="True">
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="Date" UniqueName="ReportDate" HeaderTooltip="Report Date" meta:resourcekey="ASGridBoundColumnReportDate"
                        HeaderText="Report Date" ItemStyle-HorizontalAlign="Center" AllowSorting="True">
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="Status" UniqueName="Status" HeaderTooltip="Status" meta:resourcekey="ASGridBoundColumnStatus"
                        HeaderText="Status" ItemStyle-HorizontalAlign="Center" AllowSorting="True">
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>

                    <as:ASGridTemplateColumn HeaderTooltip="Delete" meta:resourcekey="ASGridBoundColumnDelete"
                        HeaderText="Delete" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="border-right">
                        <HeaderStyle HorizontalAlign="Center" Width="150px"></HeaderStyle>
                        <ItemTemplate>
                            <as:Literal ID="lblAction" runat="server"></as:Literal>
                            <as:Button ID="btnDelete" runat="server" CssClass="display-none" />
                        </ItemTemplate>
                    </as:ASGridTemplateColumn>
                </Columns>
            </MasterTableView>
        </as:ASGrid>


    </div>

    <tek:RadAjaxManagerProxy runat="server" ID="uxRadAjaxManager">
        <AjaxSettings>
            <tek:AjaxSetting AjaxControlID="uxDrilldownGrid">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="uxDrilldownGrid" />
                </UpdatedControls>
            </tek:AjaxSetting>
            <tek:AjaxSetting AjaxControlID="btnDownLoad">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="uxGridStatement" />
                </UpdatedControls>
            </tek:AjaxSetting>
            <tek:AjaxSetting AjaxControlID="btnRefreshGrid">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="uxGridStatement" />
                </UpdatedControls>
            </tek:AjaxSetting>
            <tek:AjaxSetting AjaxControlID="uxGridStatement">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="uxGridStatement" />
                </UpdatedControls>
            </tek:AjaxSetting>
            <tek:AjaxSetting AjaxControlID="uxGridAllOfStatement">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="uxGridAllOfStatement" />
                </UpdatedControls>
            </tek:AjaxSetting>
        </AjaxSettings>
    </tek:RadAjaxManagerProxy>
    <as:ASCommandControl runat="server" CallServerFunc="checkFileAvailable" ClientSuccessCallbackFunc="checkFileAvailableSuccess"
        ID="btnCheckFileAvailable" OnCreateResponseData="btnCheckFileAvailable_OnCreateResponseData" IsRefreshPostData="true" />
    <asp:HiddenField runat="server" ID="uxMerchantNumber" />

    <tek:RadCodeBlock runat="server" ID="RadCodeBlock">
        <script>
            var btnDownLoad = '<%= btnDownLoad.ClientID%>';
            var btnRefreshGrid = '<%= btnRefreshGrid.ClientID%>';
            var mesWarning = '<%= GetLocalResourceObject("textWarning") %>';
            var mesConfirm = '<%= GetLocalResourceObject("textConfirm") %>';
            var mesConfirmDelete = '<%= GetLocalResourceObject("textConfirmDelete") %>';
            var mesYes = '<%= Resources.MessageManager.textYes %>';
            var mesNo = '<%= Resources.MessageManager.textNo %>';
            var mesOk = '<%= Resources.MessageManager.textOk %>';
            var mesCancel = '<%= Resources.MessageManager.TextCancel %>';
            var messageError = '<%= GetLocalResourceObject("uxMessageResource1.Text").ToString() %>';

        </script>
        <script src="<%=ResolveUrl("~") %>res/js/Statement.js" type="text/javascript"></script>
    </tek:RadCodeBlock>
</asp:Content>

