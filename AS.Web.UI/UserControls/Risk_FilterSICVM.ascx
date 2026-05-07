<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Risk_FilterSICVM.ascx.cs"
    Inherits="UserControls_Risk_FilterSICVM" %>


<div class="modal-title">
    <h3><as:Literal ID="Literal1" runat="server" Text="Type:" meta:resourcekey="Literal1Resource1"></as:Literal>
        <as:Literal ID="uxInclude" runat="server" meta:resourcekey="uxIncludeResource1"></as:Literal>
    </h3>
</div>
<as:ASGrid ID="uxGridSIC" runat="server" AutoGenerateColumns="false"
    AllowPaging="true" ASPagingMethod="SPASingleMethod" AllowSorting="true" AllowFilteringByColumn="true"
    OnNeedDataSource="uxGridSIC_NeedDataSource" OnInit="uxGridSIC_Init">
    <MasterTableView>
        <Columns>
            <as:ASGridBoundColumn DataField="SICCode" UniqueName="SICCode" HeaderText="SIC Code"
                ItemStyle-Width="15%" HeaderStyle-Width="15%" ASFormat="StaticString" ItemStyle-HorizontalAlign="Center"
                HeaderStyle-HorizontalAlign="Center">
            </as:ASGridBoundColumn>
            <as:ASGridBoundColumn DataField="Description" UniqueName="Description" HeaderText="SIC Description"
                ASFormat="DynamicString" FilterControlWidth="85%" ItemStyle-Width="85%" HeaderStyle-Width="80%">
            </as:ASGridBoundColumn>
        </Columns>
    </MasterTableView>
</as:ASGrid>