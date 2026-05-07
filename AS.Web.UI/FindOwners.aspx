<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPagePopup.master" AutoEventWireup="true" CodeFile="FindOwners.aspx.cs" Inherits="FindOwners" meta:resourcekey="PageTitleResourceOwner" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="Server">
    <as:ASModalContainer ID="uxModalContainer" runat="server" WidthCssClass="modal-xl" ContainerCssClass="container">
        <div id="grid-row">
            <as:ASGrid ID="uxOwnerGrid" runat="server" AutoGenerateColumns="false" AllowPaging="true" OnNeedDataSource="uxOwnerGrid_NeedDataSource"
                ASPagingMethod="SPASingleMethod" AllowFilteringByColumn="true" GridLines="None" OnInit="uxOwnerGrid_Init"
                CssClass="in">
                <MasterTableView>
                    <Columns>
                        <as:ASGridTemplateColumn AllowFiltering="false" HeaderStyle-Width="45px" ItemStyle-Width="45px" ItemStyle-HorizontalAlign="Center" meta:resourcekey="ASGridTemplateColumnResource1">
                            <ItemTemplate>
                                <input type="checkbox"
                                    onclick="doSetSelectedOwners(this.value, this.checked);" value='<%# Eval("DataKey") %>' <%# uxSelectedOwners.Value.IndexOf(","+Eval("DataKey")+",")>= 0? "checked=\"true\"":"" %> />
                            </ItemTemplate>

                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                        </as:ASGridTemplateColumn>
                        <as:ASGridBoundColumn DataField="DataText" UniqueName="DataText" HeaderText="Owner"
                            HeaderTooltip="Owner" HeaderStyle-Width="100%"
                            FilterControlWidth="75%" ASFormat="DynamicString" meta:resourcekey="ASGridBoundColumnResourceOwner">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text=""></ModelErrorMessage>
                            </ColumnValidationSettings>
                            <HeaderStyle HorizontalAlign="Center" Width="80%"></HeaderStyle>
                        </as:ASGridBoundColumn>
                    </Columns>
                </MasterTableView>
            </as:ASGrid>
        </div>
        <div class="row">
            <div class="col-md-12 form-action-container text-right">
                <as:Button ID="uxSubmit" runat="server" Text="Add" OnClientClick="doSubmit(); return false;" CssClass="btn btn-default" IsStandardButton="False" meta:resourcekey="uxSubmitResource1" />
                <as:Button ID="uxClose" runat="server" Text="Cancel" OnClientClick="parent.HidePopupModal();"
                    CssClass="btn btn-default" IsStandardButton="False" meta:resourcekey="uxCloseResource1" />
            </div>
        </div>

        <asp:HiddenField ID="uxSelectedOwners" runat="server" />


        <tek:RadCodeBlock ID="RadCodeBlock1" runat="server">
            <script type="text/javascript">
                var uxSelectedOwnersClientID = '<%= uxSelectedOwners.ClientID%>'; 
                var isEditMode = <%= FromEdit.ToString().ToLower() %>;
                var MetricControlID = '<%= MetricControlID %>'; 
            </script>
            <script type="text/javascript" src="<%=ResolveUrl("~/") %>res/js/FindOwners.js"></script>
        </tek:RadCodeBlock>

        <tek:RadAjaxManagerProxy runat="server" ID="uxRadAjaxManager">
            <AjaxSettings>
                <tek:AjaxSetting AjaxControlID="uxOwnerGrid">
                    <UpdatedControls>
                        <tek:AjaxUpdatedControl ControlID="uxOwnerGrid" />
                    </UpdatedControls>
                </tek:AjaxSetting>
            </AjaxSettings>
        </tek:RadAjaxManagerProxy>
    </as:ASModalContainer>
</asp:Content>

