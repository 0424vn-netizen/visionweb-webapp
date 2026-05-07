<%@ Page Title="Manage Custom Views" Language="C#" MasterPageFile="~/MasterPagePopup.master" meta:resourcekey="PageResource1"
    AutoEventWireup="true" CodeFile="rm_MCF_ManageCustomViewsModal.aspx.cs" Inherits="rm_MCF_ManageCustomViewsModal" %>

<asp:Content ContentPlaceHolderID="ContentPage" runat="Server" ID="uxMainModalContent">
    <tek:RadAjaxManagerProxy ID="RadAjaxManager1" runat="server">
        <AjaxSettings>
            <tek:AjaxSetting AjaxControlID="rbDisplayType">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="uxCustomViewsGrid" LoadingPanelID="uxLoadingPanelCustom" />
                </UpdatedControls>
            </tek:AjaxSetting>
            <tek:AjaxSetting AjaxControlID="btnDelete">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="uxCustomViewsGrid" LoadingPanelID="uxLoadingPanelCustom" />
                </UpdatedControls>
            </tek:AjaxSetting>
            <tek:AjaxSetting AjaxControlID="uxRebindCustomViewsGrid">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="uxCustomViewsGrid" LoadingPanelID="uxLoadingPanelCustom" />
                </UpdatedControls>
            </tek:AjaxSetting>
        </AjaxSettings>
    </tek:RadAjaxManagerProxy>

    <as:ASModalContainer ID="uxModalContainer" runat="server" ContainerCssClass="container" WidthCssClass="modal-lg">
        <asp:Panel ID="uxDisplayViewType" runat="server" CssClass="mb-20">
            <span class="title-auto-queue">
                <asp:Label runat="server" ID="lblViewType" Text="Show" meta:resourcekey="txtViewType"></asp:Label>
            </span>
            <as:RadioButton Checked="true" ID="rdAllView" runat="server" Text="All" GroupName="ViewTypeStatus" CssClass="control-inline" meta:resourcekey="rdAllViewResource" />
            <as:RadioButton ID="rdPublicView" runat="server" Text="Public" GroupName="ViewTypeStatus" CssClass="control-inline" meta:resourcekey="rdPublicViewResource" />
            <as:RadioButton ID="rdPrivateView" runat="server" Text="Private" GroupName="ViewTypeStatus" CssClass="control-inline" meta:resourcekey="rdPrivateViewResource" />
        </asp:Panel>
        <as:ASGrid ID="uxCustomViewsGrid" runat="server" AllowPaging="false" AllowSorting="True" AllowAutomaticUpdates="True"
            AutoGenerateColumns="False" AllowAutoCalculateTotalOnExport="false" AllowFilteringByColumn="false"
            ASPagingMethod="SPASingleMethod" ShowPageTotal="false" ShowReportTotal="false" OnItemDataBound="uxCustomViewsGrid_ItemDataBound"
            AllowSortFilterWhenExport="true" CssClass="in">
            <MasterTableView>
                <Columns>
                    <as:ASGridTemplateColumn HeaderText="Default" UniqueName="IsDefault" HeaderStyle-Width="54px" meta:resourcekey="ASGridBoundColumnResource3"
                        ItemStyle-CssClass="text-center" DataField="IsDefault" HeaderTooltip="Default">
                        <ItemTemplate>
                            <as:RadioButton ID="rbDisplayType" runat="server" />
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridTemplateColumn>
                    <as:ASGridBoundColumn UniqueName="ViewName" ItemStyle-HorizontalAlign="Left" HeaderText="View Name" HeaderTooltip="View Name" meta:resourcekey="ASGridBoundColumnResource1"
                        DataField="ViewName" ASFormat="StaticString" SortExpression="ViewName" HeaderStyle-Width="280px">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn UniqueName="ViewType" ItemStyle-HorizontalAlign="Left" HeaderText="Type" HeaderTooltip="Type" meta:resourcekey="ASGridBoundColumnResource2"
                        DataField="ViewTypeDesc" ASFormat="StaticString" SortExpression="ViewTypeDesc" HeaderStyle-Width="82px">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn UniqueName="UpdatedDTS" HeaderText="Last Updated" HeaderTooltip="Last Updated" meta:resourcekey="ASGridBoundColumnResource4"
                        DataField="UpdatedDTS" ASFormat="StaticString" SortExpression="UpdatedDTS" HeaderStyle-Width="152px">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridTemplateColumn HeaderText="Delete" UniqueName="IsDefault" HeaderStyle-Width="80px" meta:resourcekey="ASGridBoundColumnResource5"
                        ItemStyle-CssClass="text-center" DataField="IsDefault" HeaderTooltip="Delete">
                        <ItemTemplate>
                            <as:LinkButton CssClass="remove-item vertical-middle" Text="" ID="btnDelete" runat="server" OnClick="btnDelete_Click" />
                            <as:HiddenField ID="CustomViewID" runat="server" Value='<%# CryptorServices.Current.EncryptText(Eval("CustomViewID").ToString())%>' />
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridTemplateColumn>
                </Columns>
            </MasterTableView>
        </as:ASGrid>
        <div class="display-flex ipmt  align-center space-between mt-4x">
            <span class="link dark-blue">
                <as:Button ID="btnCreateCustomView" IsStandardButton="False" OnClientClick="doOpenSubPopup();return false;" meta:resourcekey="btnCreateCustomViewResource" runat="server" Text="Create Custom View"></as:Button>
            </span>
            <div>
                <as:Button runat="server" class="btn btn-default" ID="uxClose" meta:resourcekey="uxCloseResource" Text="Close" OnClientClick="return CloseManageCustomModal()" IsStandardButton="False" />
            </div>
        </div>
        <div class="display-none">
            <as:Button ID="uxRebindCustomViewsGrid" runat="server" OnClick="uxRebindCustomViewsGrid_Click" IsStandardButton="False" OnClientClick="registerCloseEventModal();" />
            <as:Button ID="uxDeleteCustomView" runat="server" OnClick="uxDeleteCustomView_Click" />
            <as:HiddenField ID="hddDeleteValue" runat="server" />
        </div>
    </as:ASModalContainer>
    <tek:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <script type="text/javascript">
            var uxCustomViewsGrid = '<%= uxCustomViewsGrid.ClientID %>';
            var Risk_RebindCustomViewsGrid = '<%= uxRebindCustomViewsGrid.ClientID %>';
            var uxDeleteCustomViewID = '<%= uxDeleteCustomView.ClientID %>';
            var hddDeleteValueID = '<%= hddDeleteValue.ClientID %>';
            var panel_uxDisplayViewType = '<%=uxDisplayViewType.ClientID%>'
        </script>
        <script type="text/javascript" src="<%= ResolveUrl("~/") %>res/js/risk_MCF/rm_MCF_ManageCustomViewsModal.js"></script>
    </tek:RadCodeBlock>
</asp:Content>
