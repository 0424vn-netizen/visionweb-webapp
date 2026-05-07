<%@ Page Language="C#" AutoEventWireup="true" CodeFile="rm_MCF_FirstGenuineBatchAmount.aspx.cs" Inherits="rm_MCF_FirstGenuineBatchAmount" MasterPageFile="~/MasterPage.master" meta:resourcekey="PageResource1" %>


<%@ Register Src="~/UserControls/PageTitle.ascx" TagName="ReportTitle" TagPrefix="uc" %>

<asp:Content ID="uxContentpage" ContentPlaceHolderID="ContentPage" runat="server">
    <tek:RadAjaxManagerProxy runat="server" ID="uxRadAjaxManager">
        <AjaxSettings>
            <tek:AjaxSetting AjaxControlID="uxBntSaveBatchAmount">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="uxPanelGenuineBatchAmount" />
                    <tek:AjaxUpdatedControl ControlID="uxPlAddAmount" LoadingPanelID="uxInvisiblePanel"/>
                </UpdatedControls>
            </tek:AjaxSetting>
            <tek:AjaxSetting AjaxControlID="uxGenuineBatchAmountGrid">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="uxGenuineBatchAmountGrid" />
                </UpdatedControls>
            </tek:AjaxSetting>
        </AjaxSettings>
    </tek:RadAjaxManagerProxy>

    <uc:ReportTitle ID="uxReportTitle" ReportTitle="First Genuine Batch Amount" HasFilteringOption="false" meta:resourcekey="uxPageTitleResource" runat="server" />
    <as:Panel ID="uxPlAddAmount" runat="server">
        <div class="row">
            <div class="col-md-12">
                <div class="control-inline">
                    <label class="first">
                        <asp:Literal ID="Literal1" runat="server" meta:resourcekey="GenuineBatchAmountResource1" Text="First Genuine Batch Amount:"></asp:Literal>
                        <asp:Literal ID="Literal2" runat="server" meta:resourcekey="AssociationIDResource2" Text="$"></asp:Literal>
                    </label>
                    <as:TextBox ID="uxBatchAmount" runat="server" onchange="checkDecimal()" onkeyup="checkDecimal()" CssClass="form-control as-inline" Width="300px"></as:TextBox>
                    <as:LinkButton ID="uxBntSaveBatchAmount" runat="server" OnClientClick="return onAddBathAmount();" OnClick="uxBntSaveBatchAmount_Click" Text="Save" CssClass="btn btn-default mt-m-1x ml-2x" meta:resourcekey="BntSaveResource1"></as:LinkButton>
                </div>
            </div>
        </div>
    </as:Panel>
    <as:Panel ID="uxPanelGenuineBatchAmount" runat="server">
        <as:HiddenField ID="uxHdCurrentValue" runat="server" />
        <as:HiddenField ID="uxHdSettingId" runat="server" />
        <uc:UxExport ID="uxExporter" runat="server" GridID="uxGenuineBatchAmountGrid" ShowPDF="false" meta:resourcekey="uxGridTitleResource" />
        <as:ASGrid ID="uxGenuineBatchAmountGrid" runat="server" AllowPaging="true" GridLines="None" AllowSorting="True" AllowSortFilterWhenExport="true" OnNeedDataSource="uxGenuineBatchAmountGrid_NeedDataSource"
            AutoGenerateColumns="false" ASPagingMethod="SPASingleMethod" CssClass="in" meta:resourcekey="uxFirstGenuineBatchReportGridResource1" XOverFlowable="true" HeaderStyle-Width="120px">
            <MasterTableView>
                <Columns>
                    <as:ASGridBoundColumn HeaderText="Date/Time" DataField="DateTime" UniqueName="DateTime" ItemStyle-HorizontalAlign="Center"
                        SortExpression="DateTime" HeaderTooltip="Date/Time" ASFormat="DynamicString" meta:resourcekey="ASGridBoundColumnResource1">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Changed By" DataField="ChangedBy" UniqueName="ChangedBy" ItemStyle-HorizontalAlign="Left"
                        SortExpression="ChangedBy" HeaderTooltip="Changed By" ASFormat="DynamicString" meta:resourcekey="ASGridBoundColumnResource2">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Value" DataField="Value" UniqueName="Value" ItemStyle-HorizontalAlign="Right"
                        HeaderTooltip="Value" SortExpression="Value" ASFormat="Currency" meta:resourcekey="ASGridBoundColumnResource3">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                </Columns>
            </MasterTableView>
        </as:ASGrid>
    </as:Panel>
    <as:RadCodeBlock ID="radCodeBlock2" runat="server">
        <script>
            var geaterThanZero = "<%= GetLocalResourceObject("GeaterThanZero")%>"
            var requiredMsg = "<%= GetLocalResourceObject("RequiredMgs").ToString() %>";
            var GenuineBatchAmount_uxBatchAmount = "<%=uxBatchAmount.ClientID %>";
            var GenuineBatchAmount_uxHdCurrentValue = "<%=uxHdCurrentValue.ClientID %>";
            var GenuineBatchAmount_uxBntSaveBatchAmount = "<%=uxBntSaveBatchAmount.UniqueID %>";
        </script>
        <script type="text/javascript" src="<%=ResolveUrl("~") %>res/js/risk_MCF/rm_MCF_GenuineBatchAmount.js"></script>
    </as:RadCodeBlock>
</asp:Content>
