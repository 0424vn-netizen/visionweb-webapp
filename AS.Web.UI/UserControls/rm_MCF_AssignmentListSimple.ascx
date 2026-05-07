<%@ Control Language="C#" AutoEventWireup="true" CodeFile="rm_MCF_AssignmentListSimple.ascx.cs"
    Inherits="UserControls_rm_MCF_AssignmentListSimple" %>
<%@ Register TagName="ProgressBar" TagPrefix="uc1" Src="~/UserControls/ProgressBar.ascx" %>

<as:ASGrid ID="uxGrid" runat="server" AutoGenerateColumns="false" AllowSorting="true"
    AllowPaging="false"
    PageSize="10" OnNeedDataSource="uxGrid_NeedDataSource" CssClass="in" meta:resourcekey="uxGridResource1">
    <MasterTableView>
        <Columns>
            <as:ASGridBoundColumn HeaderText="Assignment Name" DataField="AssignmentName" UniqueName="AssignmentName"
                HeaderTooltip="Click here to view the assignment"
                SortExpression="AssignmentName" meta:resourcekey="ASGridBoundColumnResource1">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
            </as:ASGridBoundColumn>
            <as:ASGridBoundColumn HeaderText="Total Merch" DataField="TotalMerchantCount" UniqueName="TotalMerchantCount"
                HeaderTooltip="# of merchants matching assignment filters" ASFormat="Integer"
                SortExpression="TotalMerchantCount" meta:resourcekey="ASGridBoundColumnResource2">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
            </as:ASGridBoundColumn>
            <as:ASGridBoundColumn HeaderText="Alert" DataField="AlertMerchantCount" UniqueName="AlertMerchantCount"
                ASFormat="Integer" HeaderTooltip="# of merchants matching assignment filters and parameters"
                SortExpression="AlertMerchantCount" meta:resourcekey="ASGridBoundColumnResource3">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
            </as:ASGridBoundColumn>
            <as:ASGridBoundColumn HeaderText="Volume" DataField="TotalMerchantAmount" UniqueName="TotalMerchantAmount"
                HeaderTooltip="Last Cycle’s volume for merchants matching assignment" ASFormat="Currency"
                SortExpression="TotalMerchantAmount" meta:resourcekey="ASGridBoundColumnResource4">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
            </as:ASGridBoundColumn>
            <as:ASGridBoundColumn HeaderText="#" DataField="WorkedMerchantCount" UniqueName="WorkedMerchantCount"
                ASFormat="Integer" HeaderTooltip="# of merchants worked from this assignment"
                SortExpression="WorkedMerchantCount" meta:resourcekey="ASGridBoundColumnResource5">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
            </as:ASGridBoundColumn>
            <as:ASGridBoundColumn HeaderText="Volume" DataField="WorkedMerchantAmount" UniqueName="WorkedMerchantAmount"
                HeaderTooltip="Volume for merchants worked from this assignment" ASFormat="Currency"
                SortExpression="WorkedMerchantAmount" meta:resourcekey="ASGridBoundColumnResource6">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
            </as:ASGridBoundColumn>
            <tek:GridTemplateColumn DataField="CompletePercent" UniqueName="CompleteTemplate"
                HeaderText="Complete" HeaderStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Bottom"
                ItemStyle-HorizontalAlign="Center" SortExpression="CompletePercent"
                FooterStyle-Width="120px" HeaderTooltip="% Complete" meta:resourcekey="GridTemplateColumnResource1">
                <ItemTemplate>
                    <uc1:ProgressBar ID="progressBar" runat="server" Width="80" Blocks="5" Value='<%# Eval("CompletedPercent") %>' />
                </ItemTemplate>
                <FooterTemplate>
                    <uc1:ProgressBar ID="progressBarFooter" runat="server" Width="80" Blocks="5" />
                </FooterTemplate>

                <FooterStyle Width="120px"></FooterStyle>

                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Bottom"></HeaderStyle>

                <ItemStyle HorizontalAlign="Center"></ItemStyle>
            </tek:GridTemplateColumn>
        </Columns>
    </MasterTableView>
</as:ASGrid>

<tek:RadCodeBlock runat="server" ID="RadCodeBlock1">
    <script type="text/javascript">
        var uxGrid_ClientID = '<%= uxGrid.ClientID %>';
        var AssignmentListSimple_js_MerchantCount = '<%=GetLocalResourceObject("AssignmentListSimple_js_MerchantCount").ToString()%>';
        var AssignmentListSimple_js_Worked = '<%= GetLocalResourceObject("AssignmentListSimple_js_Worked").ToString()%>';
    </script>
    <script src="<%= ResolveUrl("~/")%>res/js/AssignmentListSimple.js">
        doAdjustHeader(); 
    </script>
</tek:RadCodeBlock>
