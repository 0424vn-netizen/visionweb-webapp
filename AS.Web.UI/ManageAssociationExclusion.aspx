<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ManageAssociationExclusion.aspx.cs" Inherits="ManageAssociationExclusion" MasterPageFile="~/MasterPage.master" meta:resourcekey="PageResource1" %>

<%@ Register Src="~/UserControls/PageTitle.ascx" TagName="ReportTitle" TagPrefix="uc" %>

<asp:Content ID="uxContentpage" ContentPlaceHolderID="ContentPage" runat="server">
    <tek:RadAjaxManagerProxy runat="server" ID="uxRadAjaxManager">
        <AjaxSettings>
            <tek:AjaxSetting AjaxControlID="uxBntAddAssoID">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="uxPanelManaAssoExclusion" />
                    <tek:AjaxUpdatedControl ControlID="uxPnAddAssosiation" LoadingPanelID="uxInvisiblePanel"/>
                </UpdatedControls>
            </tek:AjaxSetting>
            <tek:AjaxSetting AjaxControlID="uxExclusionAssoGrid">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="uxExclusionAssoGrid" />
                </UpdatedControls>
            </tek:AjaxSetting>
        </AjaxSettings>
    </tek:RadAjaxManagerProxy>

    <uc:ReportTitle ID="uxReportTitle" ReportTitle="Manage Association Exclusion" HasFilteringOption="false" meta:resourcekey="uxPageTitleResource" runat="server" />
    <as:Panel ID="uxPnAddAssosiation" runat="server">
        <div class="row">
            <div class="col-md-12">
                <div class="control-inline">
                    <label class="first">
                        <asp:Literal ID="Literal1" runat="server" meta:resourcekey="AssociationIDResource1" Text="Association ID:"></asp:Literal>
                        <asp:Literal ID="Literal2" runat="server" meta:resourcekey="AssociationIDResource2" Text="ASSO"></asp:Literal>
                    </label>
                    <as:TextBox ID="uxAssoID" runat="server" onchange="allowOnlyNumber()" onkeyup="allowOnlyNumber()" Width="300px" CssClass="form-control as-inline" MaxLength="11"></as:TextBox>
                    <as:LinkButton ID="uxBntAddAssoID" runat="server" OnClientClick="return onAddAssociation()" OnClick="uxBntAddAssoID_Click" Text="Add" CssClass="btn btn-default mt-m-1x ml-2x" meta:resourcekey="BntAddResource1"></as:LinkButton>
                </div>
            </div>
        </div>
    </as:Panel>
    <as:Panel ID="uxPanelManaAssoExclusion" runat="server">
        <uc:UxExport ID="uxExporter" runat="server" GridID="uxExclusionAssoGrid" meta:resourcekey="uxGridTitleResource" />
        <as:ASGrid ID="uxExclusionAssoGrid" runat="server" AllowPaging="true" GridLines="None" AllowSorting="True" AllowSortFilterWhenExport="true" OnNeedDataSource="uxExclusionAssoGrid_NeedDataSource"
            AutoGenerateColumns="false" ASPagingMethod="SPASingleMethod" CssClass="in" meta:resourcekey="uxMerchantNoteReportGridResource1" XOverFlowable="true" HeaderStyle-Width="120px">
            <MasterTableView>
                <Columns>
                    <as:ASGridBoundColumn HeaderText="Association ID" DataField="AssociationID" UniqueName="AssociationID" ItemStyle-HorizontalAlign="Center"
                        SortExpression="AssociationID" HeaderTooltip="Association ID" ASFormat="DynamicString" meta:resourcekey="ASGridBoundColumnResource1">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Entity Name" DataField="UserNameFull" UniqueName="UserNameFull" ItemStyle-HorizontalAlign="Center"
                        SortExpression="UserNameFull" HeaderTooltip="Entity Name" ASFormat="DynamicString" meta:resourcekey="ASGridBoundColumnResource2">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Created Date Time" DataField="CreatedDTS" UniqueName="CreatedDTS" ItemStyle-HorizontalAlign="Center"
                        HeaderTooltip="Created Date Time" SortExpression="CreatedDTS" ASFormat="DateAndTime12Hours" meta:resourcekey="ASGridBoundColumnResource3">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridTemplateColumn HeaderText="" UniqueName="Functions" HeaderStyle-HorizontalAlign="Center" DataField="Functions" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:LinkButton ID="uxRemove" OnCommand="uxRemove_Command" OnClientClick="return onRemoveAssociation(this)" Text="Remove" runat="server" CommandName='<%# Eval("AssociationID") %>' CommandArgument='<%# Eval("AssociationID") %>' meta:resourcekey="BntRemoveResource1"></asp:LinkButton>
                        </ItemTemplate>
                    </as:ASGridTemplateColumn>
                </Columns>
            </MasterTableView>
        </as:ASGrid>
    </as:Panel>
    <as:RadCodeBlock ID="radCodeBlock2" runat="server">
        <script>
            var requiredMsg = "<%= GetLocalResourceObject("RequiredMgs").ToString() %>";
            var removeMsg = "<%= GetLocalResourceObject("RemoveMsg").ToString() %>";
            var existMsg = "<%= GetLocalResourceObject("AssoExistedMgs").ToString() %>";
            var passValidationMsg = "<%= GetLocalResourceObject("PassValidationMgs").ToString() %>";
            var comfirmationTitle = "<%= GetLocalResourceObject("Confirmation").ToString() %>";
            var Ok = "<%= GetLocalResourceObject("OkResource").ToString() %>";
            var Cancel = "<%= GetLocalResourceObject("CancelResource").ToString() %>";
            var ManageAssociationExclusion_uxAssoID = "<%=uxAssoID.ClientID %>";
            var ManageAssociationExclusion_uxBntAddAssoID = "<%=uxBntAddAssoID.UniqueID %>";
            var ManageAssociationExclusion_uxExclusionAssoGrid = "<%=uxExclusionAssoGrid.ClientID %>";
        </script>
        <script type="text/javascript" src="<%=ResolveUrl("~") %>res/js/ManageAssociationExclusion.js"></script>
    </as:RadCodeBlock>
</asp:Content>
