<%@ Control Language="C#" AutoEventWireup="true" CodeFile="rm_MCF_MgmtReport_MerchantFilter.ascx.cs"
    Inherits="UserControls_rm_MCF_MgmtReport_MerchantFilter" %>

<div class="row">
    <asp:PlaceHolder runat="server" ID="dfsdf">
    <div class="col-md-12">
        <as:ASGrid ID="uxMerchantGrid" runat="server" AutoGenerateColumns="false" AllowPaging="true"
            ASPagingMethod="SPASingleMethod" AllowFilteringByColumn="true" GridLines="None"
            OnNeedDataSource="uxMerchantGrid_NeedDataSource" OnInit="uxMerchantGrid_OnInit"
            OnDataSourceReady="uxMerchantGrid_DataSourceReady" CssClass="in" meta:resourcekey="uxMerchantGridResource1">
            <MasterTableView>
                <Columns>
                    <as:ASGridTemplateColumn AllowFiltering="false" HeaderStyle-Width="45px"  ItemStyle-HorizontalAlign="Center" meta:resourcekey="ASGridTemplateColumnResource1">
                        <ItemTemplate>
                            <input type="checkbox"
                                onclick="doSetSelectedMerchants(this.value, this.checked);" value='<%# Eval("Entity") %>' <%# uxSelectedMerchants.Value.IndexOf(","+Eval("Entity")+",")>=0?"checked=\"true\"":"" %>="" />
                        </ItemTemplate>

<ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </as:ASGridTemplateColumn>
                    <as:ASGridBoundColumn DataField="Entity" UniqueName="Entity" HeaderText="Merchant #"
                        HeaderTooltip="Merchant Number"
                        HeaderStyle-Width="19%" FilterControlWidth="75%" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource1">
<ColumnValidationSettings>
<ModelErrorMessage Text=""></ModelErrorMessage>
</ColumnValidationSettings>

<HeaderStyle HorizontalAlign="Center" Width="19%"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="EntityName" UniqueName="EntityName" HeaderText="Merchant Name"
                        HeaderTooltip="Merchant Name" HeaderStyle-Width="80%"
                        FilterControlWidth="75%" ASFormat="DynamicString" meta:resourcekey="ASGridBoundColumnResource2">
<ColumnValidationSettings>
<ModelErrorMessage Text=""></ModelErrorMessage>
</ColumnValidationSettings>

<HeaderStyle HorizontalAlign="Center" Width="80%"></HeaderStyle>
                    </as:ASGridBoundColumn>
                </Columns>
            </MasterTableView>
        </as:ASGrid>
    </div>
    </asp:PlaceHolder>
</div>
<div class="row">
    
    <div class="col-md-12 form-action-container text-right">
        <as:Button ID="uxSubmit" runat="server" Text="Add" OnClientClick="doSubmit();" CssClass="btn btn-default" IsStandardButton="False" meta:resourcekey="uxSubmitResource1" />
        <as:Button ID="uxClose" runat="server" Text="Cancel" OnClientClick="parent.HidePopupModal();"
            CssClass="btn btn-default" IsStandardButton="False" meta:resourcekey="uxCloseResource1" />
       
    </div>
</div>

<asp:UpdatePanel runat="server" ID="UpdatePanel1">
    <ContentTemplate>
        <asp:HiddenField ID="uxSelectedMerchants" runat="server" />
    </ContentTemplate>
</asp:UpdatePanel>

<tek:RadAjaxManagerProxy runat="server" ID="uxRadAjaxManager">
    <AjaxSettings>
        <tek:AjaxSetting AjaxControlID="uxMerchantGrid">
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="uxMerchantGrid" />
                <tek:AjaxUpdatedControl ControlID="uxOptionPanel" />
            </UpdatedControls>
        </tek:AjaxSetting>
    </AjaxSettings>
</tek:RadAjaxManagerProxy>

<tek:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <script type="text/javascript">
        var MgmtReport_MerchantFilter_uxSelectedMerchants = '<%=uxSelectedMerchants.ClientID %>';
        
    </script>
    <script type="text/javascript" src="<%=ResolveUrl("~") %>res/js/risk_MCF/rm_MCF_MgmtReport_MerchantFilter.js"></script>
</tek:RadCodeBlock>
