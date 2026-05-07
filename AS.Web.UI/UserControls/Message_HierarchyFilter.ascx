<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Message_HierarchyFilter.ascx.cs"
    Inherits="UserControls_Message_HierarchyFilter" %>
<div class="row">
    <div class="col-md-12 on-top">
        <as:RadCodeBlock ID="rad1" runat="server">
            <i>
                <asp:Literal ID="Literal1" runat="server" Text="Please select one or more" meta:resourcekey="LiteralResource1" />
                <%= HierarchyFilterText %>(s)
                <asp:Literal ID="Literal2" runat="server" Text="values to include or exclude." meta:resourcekey="LiteralResource2" />
            </i>
        </as:RadCodeBlock>
    </div>
</div>
<div class="height-14"></div>
<div class="row">
    <div class="col-xs-6 text-left">
        <tek:RadAjaxPanel runat="server" ID="RadAjaxPanel1">
            <as:RadioButtonList ID="uxRadioMode" runat="server" RepeatDirection="horizontal"
                AutoPostBack="true" OnSelectedIndexChanged="uxRadioMode_SelectedIndexChanged"
                CssClass="radio-button-list dark-blue">
                <asp:ListItem Text="Include" meta:resourcekey="Message_HierarchyFilterCS_Text_Include"></asp:ListItem>
                <asp:ListItem Text="Exclude" meta:resourcekey="Message_HierarchyFilterCS_Text_Exclude"></asp:ListItem>
            </as:RadioButtonList>
            <as:Button ID="btnHidden" runat="server" OnClick="btnHidden_Click" CssClass="display-none" />
        </tek:RadAjaxPanel>
    </div>
    <div class="col-xs-6 text-right">
        <span class="dark-blue merchant-count">
            <as:Literal ID="lblCount" runat="server" Text="Selected Count: 0" meta:resourcekey="lblCountResource1"></as:Literal></span>
    </div>
</div>
<div class="height-8"></div>
<div class="row">
    <div class="col-md-12">
        <asp:PlaceHolder runat="server" ID="plhuxGrid">
            <as:ASGrid ID="uxGrid" runat="server" AutoGenerateColumns="false" AllowPaging="true"
                OnInit="uxGrid_Init" ASPagingMethod="SPASingleMethod" AllowFilteringByColumn="true"
                OnNeedDataSource="uxGrid_NeedDataSource" OnItemDataBound="uxGrid_ItemDataBound"
                OnItemCommand="uxGrid_ItemCommand" CssClass="in" meta:resourcekey="uxGridResource1">
                <MasterTableView>
                    <Columns>
                        <as:ASGridTemplateColumn AllowFiltering="false" ItemStyle-HorizontalAlign="Center" meta:resourcekey="ASGridTemplateColumnResource1" HeaderStyle-Width="30px">
                            <HeaderTemplate>
                                <as:CheckBox ID="chkHeader" runat="server" CssClass="valign-middle" Visible="False" meta:resourcekey="chkHeaderResource1" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <as:CheckBox ID="chkItem" CssClass="checkbox-middle" runat="server" meta:resourcekey="chkItemResource1" />
                            </ItemTemplate>

                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                        </as:ASGridTemplateColumn>
                        <as:ASGridBoundColumn DataField="Assigned" UniqueName="Assigned" Visible="false" meta:resourcekey="ASGridBoundColumnResource1">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text=""></ModelErrorMessage>
                            </ColumnValidationSettings>

                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        </as:ASGridBoundColumn>
                        <as:ASGridBoundColumn DataField="DataKey" UniqueName="DataKey" HeaderText="" ASFormat="StaticString"
                            HeaderStyle-HorizontalAlign="Center" meta:resourcekey="ASGridBoundColumnResource2">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text=""></ModelErrorMessage>
                            </ColumnValidationSettings>

                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        </as:ASGridBoundColumn>
                        <as:ASGridBoundColumn DataField="DataText" UniqueName="DataText" HeaderText="" Visible="false"
                            ASFormat="DynamicString" meta:resourcekey="ASGridBoundColumnResource3">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text=""></ModelErrorMessage>
                            </ColumnValidationSettings>

                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        </as:ASGridBoundColumn>
                    </Columns>
                </MasterTableView>
            </as:ASGrid>
        </asp:PlaceHolder>
    </div>
</div>
<div class="row">
    <div class="col-md-12 form-action-container text-right">
        <as:Button ID="btnClearAll" runat="server" Text="Clear All" OnClick="btnClearAll_Click"
            CssClass="btn btn-default" OnClientClick="ClearAll();" IsStandardButton="False" meta:resourcekey="btnClearAllResource1" />
        <as:Button ID="uxBntClose" runat="server" Text="Close"
            OnClientClick="parent.ClosePopupModal(1);" CssClass="btn btn-default" IsStandardButton="False" meta:resourcekey="uxBntCloseResource1" />
    </div>
</div>
<asp:UpdatePanel runat="server" ID="UpdatePanel1">
    <contenttemplate>
        <as:HiddenField ID="uxIsAdded" runat="server" />
        <as:HiddenField ID="uxIsSelectAll" runat="server" />
        <as:HiddenField ID="uxValueCode" runat="server" />
        <as:HiddenField ID="uxAllFlag" runat="server" />
    </contenttemplate>
</asp:UpdatePanel>
<as:RadAjaxManagerProxy runat="server" ID="uxRadAjaxManager">
    <AjaxSettings>
        <tek:AjaxSetting AjaxControlID="uxGrid">
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="uxGrid" />
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
                <tek:AjaxUpdatedControl ControlID="uxGrid" />
                <tek:AjaxUpdatedControl ControlID="uxRadioMode" />
            </UpdatedControls>
        </tek:AjaxSetting>
    </AjaxSettings>
</as:RadAjaxManagerProxy>
<as:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <script type="text/javascript">
        var uxIsAddedClientID = "<%=uxIsAdded.ClientID %>";
        var uxIsSelectAllClientID = "<%=uxIsSelectAll.ClientID %>";
        var uxValueCodeClientID = "<%=uxValueCode.ClientID %>";
        var uxAllFlagClientID = "<%=uxAllFlag.ClientID %>";
        var btnHiddenClientID = "<%=btnHidden.ClientID %>";
        var IS_SELECT_ALL_FLAG = "<%=IS_SELECT_ALL_FLAG%>";
        var IS_DESELECT_ALL_FLAG = "<%=IS_DESELECT_ALL_FLAG%>";   
        
    </script>
    <script src="<%=ResolveUrl("~")%>res/js/Message_HierarchyFilter.js"></script>
</as:RadCodeBlock>
