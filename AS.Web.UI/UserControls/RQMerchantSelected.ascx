<%@ Control Language="C#" AutoEventWireup="true" CodeFile="RQMerchantSelected.ascx.cs" Inherits="UserControls_RQMerchantSelected" %>

<as:ASGrid ID="uxMerchantSelected" runat="server" AllowPaging="true" GridLines="None" AllowSorting="True" AllowSortFilterWhenExport="true" OnNeedDataSource="uxMerchantSelected_NeedDataSource" OnItemDataBound="uxMerchantSelected_ItemDataBound"
    AutoGenerateColumns="false" ASPagingMethod="SPASingleMethod" CssClass="in" meta:resourcekey="uxMerchantSelectedReportGridResource1" XOverFlowable="true" HeaderStyle-Width="120px">
    <MasterTableView>
        <Columns>
            <as:ASGridBoundColumn HeaderText="Merchant ID" DataField="MerchantNumber" UniqueName="MerchantNumber" ItemStyle-HorizontalAlign="Center"
                SortExpression="MerchantNumber" HeaderTooltip="Merchant ID" ASFormat="DynamicString" meta:resourcekey="ASGridBoundColumnResource1">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
            </as:ASGridBoundColumn>
            <as:ASGridBoundColumn HeaderText="Merchant Name" DataField="MerchantName" UniqueName="MerchantName" ItemStyle-HorizontalAlign="Left"
                SortExpression="MerchantName" HeaderTooltip="Entity Name" ASFormat="DynamicString" meta:resourcekey="ASGridBoundColumnResource2">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
            </as:ASGridBoundColumn>
            <as:ASGridBoundColumn HeaderText="Current Work Queue" DataField="CurrentWQ" UniqueName="CurrentWQ" ItemStyle-HorizontalAlign="Center"
                HeaderTooltip="Current Work Queue" SortExpression="CurrentWQ" ASFormat="DynamicString" meta:resourcekey="ASGridBoundColumnResource3">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
            </as:ASGridBoundColumn>
        </Columns>
    </MasterTableView>
</as:ASGrid>