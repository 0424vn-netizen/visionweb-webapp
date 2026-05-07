<%@ Page Title="ACTIVE ASSIGNMENTS" Language="C#" MasterPageFile="~/MasterPagePopup.master"
    AutoEventWireup="true" CodeFile="rm_MCF_Group_ActiveModal.aspx.cs" Inherits="rm_MCF_Group_ActiveModal" meta:resourcekey="PageResource1" %>

<%@ Register TagName="UxExport" Src="~/UserControls/UxExport.ascx" TagPrefix="uc" %>
<%@ Register TagName="ProgressBar" Src="~/UserControls/ProgressBar.ascx" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="Server">
    <as:ASRadAjaxManagerProxy runat="server" ID="uxAjaxManager">
        <AjaxSettings>
            <tek:AjaxSetting AjaxControlID="uxReportGrid">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="uxReportGrid" />
                </UpdatedControls>
            </tek:AjaxSetting>
        </AjaxSettings>
    </as:ASRadAjaxManagerProxy>
    <as:ASModalContainer runat="server" ID="uxModalContainer" WidthCssClass="modal-xl" ContainerCssClass="container" Width="">
        <div class="row">
            <div class="col-md-12 on-top">
                <i>
                    <asp:Literal ID="rm_Group_ActiveModal_aspx_Text1" runat="server" meta:resourcekey="rm_Group_ActiveModal_aspx_Text1Resource1"> Selected Group cannot be deactivated because the following assignments are still
                    active.</asp:Literal></i>
            </div>
        </div>
        <div class="height-20"></div>
        <uc:UxExport ID="uxExporter" runat="server" GridID="uxReportGrid" IsOnTop="true" />
        <as:ASGrid ID="uxReportGrid" runat="server" AllowPaging="true" GridLines="None" AutoGenerateColumns="false"
            ASPagingMethod="SPASingleMethod" MasterTableView-TableLayout="Auto" CssClass="in" meta:resourcekey="uxReportGridResource1">
            <MasterTableView AllowSorting="true">
                <Columns>
                    <as:ASGridBoundColumn HeaderText="Assignment Name" DataField="AssignmentName" UniqueName="AssignmentName"
                        ASFormat="DynamicString" HeaderTooltip="Assignment Name" SortExpression="AssignmentName" meta:resourcekey="ASGridBoundColumnResource1">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Total Merch" DataField="TotalMerchantCount" UniqueName="TotalMerchantCount"
                        Visible="true" ASFormat="Integer" HeaderTooltip="# of merchants matching assignment filters"
                        SortExpression="TotalMerchantCount" meta:resourcekey="ASGridBoundColumnResource2">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Alert" DataField="AlertMerchantCount" UniqueName="AlertMerchantCount"
                        Visible="true" ASFormat="Integer" SortExpression="AlertMerchantCount" HeaderTooltip="# of merchants matching assignment filters and parameters" meta:resourcekey="ASGridBoundColumnResource3">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Volume" DataField="TotalMerchantAmount" UniqueName="Volume"
                        ASFormat="Currency" HeaderTooltip="Today’s volume for merchants matching assignment"
                        SortExpression="TotalMerchantAmount" meta:resourcekey="ASGridBoundColumnResource4">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="#" DataField="WorkedMerchantCount" UniqueName="WorkedMerchantCount"
                        Visible="true" ASFormat="Integer" SortExpression="WorkedMerchantCount" HeaderTooltip="# of merchants worked from this assignment" meta:resourcekey="ASGridBoundColumnResource5">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Volume" DataField="WorkedMerchantAmount" UniqueName="WorkedMerchantAmount"
                        Visible="true" ASFormat="Currency" SortExpression="WorkedMerchantAmount" HeaderTooltip="Volume for merchants worked from this assignment" meta:resourcekey="ASGridBoundColumnResource6">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <tek:GridTemplateColumn DataField="CompletedPercent" UniqueName="CompleteTemplate"
                        HeaderText="<span title='% Complete'>% Complete</span>" HeaderStyle-HorizontalAlign="Center"
                        ItemStyle-HorizontalAlign="Center" SortExpression="CompletedPercent" ItemStyle-CssClass="percent-column" meta:resourcekey="GridTemplateColumnResource1">
                        <ItemTemplate>
                            <uc1:ProgressBar ID="progressBar" runat="server" Value='<%# Eval("CompletedPercent") %>' />
                        </ItemTemplate>
                        <FooterTemplate>
                            <uc1:ProgressBar ID="progressBarFooter" runat="server" />
                        </FooterTemplate>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                        <ItemStyle HorizontalAlign="Center" CssClass="percent-column"></ItemStyle>
                    </tek:GridTemplateColumn>
                    <as:ASGridBoundColumn UniqueName="CompletedPercent" DataField="CompletedPercent"
                        HeaderText="% Complete" ASFormat="Percentage" ItemStyle-HorizontalAlign="Center"
                        Visible="false" meta:resourcekey="ASGridBoundColumnResource7">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </as:ASGridBoundColumn>
                </Columns>
            </MasterTableView>
        </as:ASGrid>
    </as:ASModalContainer>
    <tek:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <script type="text/javascript">
            var rm_Group_ActiveModal_uxReportGrid = "<%= uxReportGrid.ClientID %>";
        </script>
        <script type="text/javascript" src="<%=ResolveUrl("~") %>res/js/risk_MCF/rm_MCF_Group_ActiveModal.js"></script>
    </tek:RadCodeBlock>
</asp:Content>
