<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPagePopup.master" CodeFile="AddOrganizationModal.aspx.cs" Inherits="AddOrganizationModal" meta:resourcekey="PageResource1" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="Server">
    <tek:RadAjaxManagerProxy ID="RadAjaxManager1" runat="server">
        <AjaxSettings>
            <tek:AjaxSetting AjaxControlID="uxAvailableOrganization">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="uxAvailableOrganization" />
                    <tek:AjaxUpdatedControl ControlID="uxOrganizationSelected" />
                    <tek:AjaxUpdatedControl ControlID="lbSelectedOrg" />
                </UpdatedControls>
            </tek:AjaxSetting>
            <tek:AjaxSetting AjaxControlID="uxOrganizationSelected">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="uxOrganizationSelected" />
                    <tek:AjaxUpdatedControl ControlID="uxAvailableOrganization" />
                    <tek:AjaxUpdatedControl ControlID="lbSelectedOrg" />
                </UpdatedControls>
            </tek:AjaxSetting>
            <tek:AjaxSetting AjaxControlID="uxSelectAll">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="uxOrganizationSelected" />
                    <tek:AjaxUpdatedControl ControlID="uxAvailableOrganization" />
                    <tek:AjaxUpdatedControl ControlID="lbSelectedOrg" />
                </UpdatedControls>
            </tek:AjaxSetting>
            <tek:AjaxSetting AjaxControlID="uxClearAll">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="uxOrganizationSelected" />
                    <tek:AjaxUpdatedControl ControlID="uxAvailableOrganization" />
                    <tek:AjaxUpdatedControl ControlID="lbSelectedOrg" />
                </UpdatedControls>
            </tek:AjaxSetting>
            <tek:AjaxSetting AjaxControlID="btnSubmit">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="btnSubmit" />
                </UpdatedControls>
            </tek:AjaxSetting>
        </AjaxSettings>
    </tek:RadAjaxManagerProxy>
    <as:ASModalContainer ID="uxModalContainer" runat="server" WidthCssClass="modal-xl" ContainerCssClass="container">
        <div>
            <div class="row">
                <div class="col-xs-6">
                    <div class="row title-auto-queue">
                        <div class="col-xs-7">
                            <div class="font-20">
                                <asp:Literal ID="Literal1" runat="server" Text="Available Organizations" meta:resourcekey="LiteralResource1" />
                            </div>
                        </div>
                        <div class="col-xs-5 text-right">
                            <asp:LinkButton ID="uxSelectAll" runat="server" Text="Select All" OnClientClick="return checkHasItem();" OnClick="uxSelectAll_Click" CssClass="mt-2x display-inline link-back "
                                meta:resourcekey="uxSelectAllResource"></asp:LinkButton>
                        </div>
                    </div>
                </div>
                <div class="col-xs-6">
                    <div class="row title-auto-queue">
                        <div class="col-xs-7">
                            <div class="font-20">
                                <asp:Literal ID="Literal2" runat="server" Text="Organizations Selected" meta:resourcekey="LiteralResource2" />
                            </div>
                        </div>
                        <div class="col-xs-5 text-right">
                            <asp:LinkButton ID="uxClearAll" runat="server" Text="Clear All" OnClick="uxClearAll_Click" CssClass="mt-2x display-inline link-back hover-show"
                                meta:resourcekey="uxClearAllResource"></asp:LinkButton>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-xs-6">
                    <div class="list-group-action-table list-organizations-left">
                        <as:ASGrid runat="server" ID="uxAvailableOrganization" OnNeedDataSource="uxAvailableOrganization_NeedDataSource" AllowFilteringByColumn="true"
                            OnItemCommand="uxAvailableOrganization_ItemCommand" OnItemDataBound="uxAvailableOrganization_ItemDataBound" CssClass="in"
                            AutoGenerateColumns="false" meta:resourcekey="uxAvailableOrganizationResource">
                            <MasterTableView AllowCustomSorting="false">
                                <Columns>
                                    <as:ASGridTemplateColumn HeaderText="" UniqueName="OrganizationName" HeaderStyle-HorizontalAlign="Center" AllowFiltering="true"
                                        DataField="OrganizationName" meta:resourcekey="uxAvailableOrganizationColumnResource1">
                                        <ItemTemplate>
                                            <%# Eval("OrganizationName") %>
                                            <asp:Button ID="uxSelect" OnCommand="uxSelect_Command" Text="Add" runat="server" CommandName='<%# Eval("OrganizationName") %>' CommandArgument='<%# Eval("OrganizationID") %>' />
                                        </ItemTemplate>
                                    </as:ASGridTemplateColumn>
                                </Columns>
                            </MasterTableView>
                        </as:ASGrid>
                    </div>
                </div>
                <div class="col-xs-6">
                    <div class="list-group-action-table list-organizations-right">
                        <as:ASGrid runat="server" ID="uxOrganizationSelected" OnNeedDataSource="uxOrganizationSelected_NeedDataSource"
                            CssClass="in" AutoGenerateColumns="false">
                            <MasterTableView AllowCustomSorting="false">
                                <Columns>
                                    <as:ASGridTemplateColumn HeaderText="" UniqueName="OrganizationName" HeaderStyle-HorizontalAlign="Center"
                                        DataField="OrganizationName">
                                        <ItemTemplate>
                                            <%# Eval("OrganizationName") %>
                                            <asp:Button ID="uxRemove" OnCommand="uxRemove_Command" Text="Remove" runat="server" CommandName='<%# Eval("OrganizationName") %>' CommandArgument='<%# Eval("OrganizationID") %>' />
                                        </ItemTemplate>
                                    </as:ASGridTemplateColumn>
                                </Columns>
                            </MasterTableView>
                        </as:ASGrid>
                    </div>
                </div>
            </div>

            <div class="row">
                <div class="col-xs-6 class mt-5x">
                    <asp:Literal ID="lbSelectedCount" runat="server" Text="Selected Count: " meta:resourcekey="lbSelectedCount" />
                    <div class="inline-block font-weight-bold"><asp:Literal ID="lbSelectedOrg" runat="server" Text="0" /></div>
                </div>
                <div class="col-xs-6 form-action-container text-right">
                    <as:Button class="hidden" ID="btnSubmit" Text="Submit" OnClick="btnSubmit_Click" runat="server" />

                    <as:Button class="btn btn-default" ID="btnSave" Text="Submit" OnClientClick=" GetOrgs(this); return false;" runat="server" meta:resourcekey="bntSubmit" />
                    <as:Button class="btn btn-default" ID="btnCancel" Text="Cancel" OnClientClick="ClosePopupModal();" runat="server" meta:resourcekey="bntCancel" />
                </div>
            </div>
        </div>
    </as:ASModalContainer>
    <tek:RadCodeBlock runat="server" ID="RadCodeBlock">
        <script type="text/javascript">
            var btnSubmit = '<%= btnSubmit.ClientID%>';
            var uxAvailableOrganization_ClientID = '<%=uxAvailableOrganization.ClientID%>';
        </script>
        <script type="text/javascript" src="<%=ResolveUrl("~/") %>res/js/AddOrganizationModal.js"></script>
    </tek:RadCodeBlock>
</asp:Content>
