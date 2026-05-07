<%@ Control Language="C#" AutoEventWireup="true" CodeFile="rm_MCF_FilterSIC.ascx.cs"
    Inherits="UserControls_rm_MCF_FilterSIC" %>

<asp:UpdatePanel runat="server" ID="UpdatePanel1">
    <ContentTemplate>
        <as:HiddenField ID="uxIsAdded" runat="server" />
        <as:HiddenField ID="uxIsSelectAll" runat="server" />
        <as:HiddenField ID="uxSICCode" runat="server" />
        <as:HiddenField ID="uxAllFlag" runat="server" />
    </ContentTemplate>
</asp:UpdatePanel>

<as:RadAjaxManagerProxy runat="server" ID="uxRadAjaxManager">
    <AjaxSettings>
        <tek:AjaxSetting AjaxControlID="uxGridSIC">
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="uxGridSIC" />
            </UpdatedControls>
        </tek:AjaxSetting>
        <tek:AjaxSetting AjaxControlID="btnHidden">
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="lblCount" />
            </UpdatedControls>
        </tek:AjaxSetting>

        <tek:AjaxSetting AjaxControlID="btnClearAll">
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="lblCount" />
                <tek:AjaxUpdatedControl ControlID="uxGridSIC" />
            </UpdatedControls>
        </tek:AjaxSetting>
    </AjaxSettings>
</as:RadAjaxManagerProxy>

<div class="row">
    <div class="col-xs-6 on-top">

        <as:ASRadAjaxPanel runat="server" ID="RadAjaxPanel1">
            <asp:RadioButtonList ID="uxRadioMode" runat="server" RepeatDirection="horizontal"
                AutoPostBack="true" OnSelectedIndexChanged="uxRadioMode_SelectedIndexChanged"
                CssClass="radio-button-list dark-blue">
                <asp:ListItem Text="Include" meta:resourcekey="lblIncludeResource1"></asp:ListItem>
                <asp:ListItem Text="Exclude" meta:resourcekey="lblExcludeResource1"></asp:ListItem>
            </asp:RadioButtonList>
        </as:ASRadAjaxPanel>
    </div>
    <div class="col-xs-6 on-top text-right">
        <span class="dark-blue">
            <asp:Literal ID="lblCount" runat="server" Text="Selected Count: <b>0</b>" meta:resourcekey="lblCountResource1"></asp:Literal></span>
    </div>

</div>
<div class="height-10"></div>
<div class="row">
    <div class="col-md-12">
        <as:ASGrid ID="uxGridSIC" runat="server" AutoGenerateColumns="false" AllowPaging="true"
            OnInit="uxGridSIC_Init" ASPagingMethod="SPASingleMethod" AllowFilteringByColumn="true"
            OnNeedDataSource="uxGridSIC_NeedDataSource" OnItemDataBound="uxGridSIC_ItemDataBound"
            OnItemCommand="uxGridSIC_ItemCommand">
            <MasterTableView>
                <Columns>
                    <as:ASGridTemplateColumn AllowFiltering="false">
                        <HeaderStyle Width="5%" HorizontalAlign="Center" />
                        <ItemStyle Width="5%" HorizontalAlign="Center" />
                        <HeaderTemplate>
                            <as:CheckBox ID="chkHeader" runat="server" Visible="false" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <as:CheckBox ID="chkItem" runat="server" />
                        </ItemTemplate>
                    </as:ASGridTemplateColumn>
                    <as:ASGridBoundColumn DataField="Assigned" UniqueName="Assigned" Visible="false">
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="SICCode" UniqueName="SICCode" HeaderText="SIC Code" meta:resourcekey="uxSicCodeResource1"
                        ItemStyle-Width="15%" HeaderStyle-Width="15%" ASFormat="StaticString" ItemStyle-HorizontalAlign="Center"
                        HeaderStyle-HorizontalAlign="Center">
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="Description" UniqueName="Description" HeaderText="SIC Description"  meta:resourcekey="uxSicDescriptionResource1"
                        ItemStyle-Width="80%" HeaderStyle-Width="80%"
                        ASFormat="DynamicString" FilterControlWidth="75%">
                    </as:ASGridBoundColumn>
                </Columns>
            </MasterTableView>
        </as:ASGrid>
    </div>
</div>
<div class="row">

    <div class="col-md-12 form-action-container text-right">
        <as:Button ID="btnClearAll" runat="server" Text="Clear All" OnClick="btnClearAll_Click"
            CssClass="btn btn-default
" IsStandardButton="False" meta:resourcekey="btnClearAllResource1" />
        <as:Button ID="uxBntClose" runat="server" Text="Close" OnClientClick="parent.ClosePopupModal(1);"
            CssClass="btn btn-default
" IsStandardButton="False" meta:resourcekey="uxBntCloseResource1" />
    </div>
</div>


<div class="display-none">
    <as:Button ID="btnHidden" runat="server" OnClick="btnHidden_Click" IsStandardButton="False" meta:resourcekey="btnHiddenResource1" />
</div>

<as:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <script type="text/javascript">
        var Risk_FilterSIC_uxIsAdded = '#<%=uxIsAdded.ClientID %>';
        var Risk_FilterSIC_uxIsSelectAll = '#<%=uxIsSelectAll.ClientID %>';
        var Risk_FilterSIC_uxSICCode = '#<%=uxSICCode.ClientID %>';
        var Risk_FilterSIC_uxAllFlag = '#<%=uxAllFlag.ClientID %>';
        var Risk_FilterSIC_btnHidden = '#<%=btnHidden.ClientID %>';
        var Risk_FilterSIC_IS_SELECT_ALL_FLAG = '<%=IS_SELECT_ALL_FLAG%>';
        var Risk_FilterSIC_IS_DESELECT_ALL_FLAG = '<%=IS_DESELECT_ALL_FLAG%>';
    </script>

    <script src="<%= ResolveUrl("~/")%>res/js/risk_MCF/rm_MCF_FilterSIC.js"></script>
</as:RadCodeBlock>
