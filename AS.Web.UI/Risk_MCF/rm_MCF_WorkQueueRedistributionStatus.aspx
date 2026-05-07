<%@ Page Language="C#" AutoEventWireup="true" CodeFile="rm_MCF_WorkQueueRedistributionStatus.aspx.cs" MasterPageFile="~/MasterPagePopup.master" Inherits="rm_MCF_WorkQueueRedistributionStatus" EnableEventValidation="false" meta:resourcekey="PageTitleResource1" %>

<%@ Register Src="~/UserControls/PageTitle.ascx" TagName="PageTitle" TagPrefix="uc" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="Server">
    <as:RadAjaxManagerProxy ID="RadAjaxManager1" runat="server">
            <AjaxSettings>
                <tek:AjaxSetting AjaxControlID="uxGrid">
                    <UpdatedControls>
                        <tek:AjaxUpdatedControl ControlID="uxGrid" />
                    </UpdatedControls>
                </tek:AjaxSetting>
                <tek:AjaxSetting AjaxControlID="chbxQueued">
                    <UpdatedControls>
                        <tek:AjaxUpdatedControl ControlID="uxGrid" />
                    </UpdatedControls>
                </tek:AjaxSetting>
                <tek:AjaxSetting AjaxControlID="chbxInProgress">
                    <UpdatedControls>
                        <tek:AjaxUpdatedControl ControlID="uxGrid" />
                    </UpdatedControls>
                </tek:AjaxSetting>
                <tek:AjaxSetting AjaxControlID="chbxCompleted">
                    <UpdatedControls>
                        <tek:AjaxUpdatedControl ControlID="uxGrid" />
                    </UpdatedControls>
                </tek:AjaxSetting>
                <tek:AjaxSetting AjaxControlID="uxSubmitAjax">
                    <UpdatedControls>
                        <tek:AjaxUpdatedControl ControlID="uxGrid" />
                    </UpdatedControls>
                </tek:AjaxSetting>
            </AjaxSettings>
        </as:RadAjaxManagerProxy>
    <as:ASModalContainer ID="uxModalContainer" runat="server" WidthCssClass="modal-xxl">
        <div class="row">
            <div class="col-md-12">
                <h2 class="modal-title">
                    <asp:Literal ID="uxTitle" Text="Work Queue Redistribution Processing Status" runat="server"
                        meta:resourcekey="uxTitleResource1"> </asp:Literal></h2>
            </div>
        </div>
        <div class="row radio-button-list dark-blue mb-4x">
            <div class="col-md-12">
                <div class="control-inline">
                    <label class="first">
                        <asp:Literal ID="uxStatusText" runat="server" Text="View:" meta:resourcekey="uxStatusTextResource1"></asp:Literal>
                    </label>
                </div>
                <div class="control-inline">
                    <asp:CheckBox ID="chbxQueued" runat="server" Text="Queued" AutoPostBack="true" Checked="true" 
                        OnCheckedChanged="chbxQueued_CheckedChanged" meta:resourcekey="chbxQueuedResource1" />
                </div>
                <div class="control-inline">
                    <asp:CheckBox ID="chbxInProgress" runat="server" Text="In Progress" AutoPostBack="true"
                        OnCheckedChanged="chbxQueued_CheckedChanged" Checked="true" meta:resourcekey="chbxInProgressResource1" />
                </div>
                <div class="control-inline">
                    <asp:CheckBox ID="chbxCompleted" runat="server" Text="Completed" AutoPostBack="true"
                        OnCheckedChanged="chbxQueued_CheckedChanged" meta:resourcekey="chbxCompletedResource1" />
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12">
                <as:ASGrid ID="uxGrid" runat="server" AutoGenerateColumns="False" PageSize="10" OnNeedDataSource="uxGrid_NeedDataSource"
                    OnItemDataBound="uxGrid_ItemDataBound" AllowSorting="True" AllowPaging="true" GridLines="None" CssClass="in" 
                    meta:resourcekey="uxGridResource1">
                    <MasterTableView>
                        <Columns>
                            <as:ASGridBoundColumn HeaderText="Auto Queue" DataField="AutoQueueName" UniqueName="AutoQueueName"
                                HeaderTooltip="Auto Queue" ASFormat="StaticString" SortExpression="AutoQueueName"
                                meta:resourcekey="ASGridBoundColumnResource1">
                            </as:ASGridBoundColumn>
                            <as:ASGridBoundColumn HeaderText="From Work Queue(s)" DataField="SourceAssignmentNames" UniqueName="FromWorkQueue"
                                ASFormat="StaticString" HeaderTooltip="From Work Queue(s)" SortExpression="SourceAssignmentNames" 
                                meta:resourcekey="ASGridBoundColumnResource2">
                            </as:ASGridBoundColumn>
                            <as:ASGridBoundColumn HeaderText="To Work Queue(s)" DataField="DestinationAssignmentNames" UniqueName="ToWorkQueue"
                                ASFormat="StaticString" HeaderTooltip="To Work Queue(s)" SortExpression="DestinationAssignmentNames"
                                meta:resourcekey="ASGridBoundColumnResource3">
                            </as:ASGridBoundColumn>
                            <as:ASGridBoundColumn HeaderText="Description" DataField="QueueDesc" UniqueName="Description"
                                ASFormat="StaticString" HeaderTooltip="Description" SortExpression="QueueDesc"
                                meta:resourcekey="ASGridBoundColumnResource4">
                            </as:ASGridBoundColumn>
                            <as:ASGridBoundColumn HeaderText="Status" DataField="QueueStatusDesc" UniqueName="Status" HeaderTooltip="Status"
                                SortExpression="QueueStatusDesc" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource5">
                            </as:ASGridBoundColumn>
                            <as:ASGridBoundColumn HeaderText="Processed DTS" DataField="RedistributedDTS" HeaderStyle-Width="150px" 
                                UniqueName="ProcessedOn" HeaderTooltip="ProcessedOn" SortExpression="RedistributedDTS"
                                ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource6">
                            </as:ASGridBoundColumn>
                        </Columns>
                    </MasterTableView>
                </as:ASGrid>
            </div>
        </div>
        <div class=" mt-4x text-right">
            <asp:Button ID="uxClose" runat="server" CssClass="btn btn-default" Text="Close" meta:resourcekey="uxCloseResource1" 
                OnClientClick="parent.HidePopupModal(); return false;" />
            </div>
        <div class="display-none">
            <as:Button ID="uxSubmitAjax" runat="server" Text="Submit" OnClick="uxSubmitAjax_Click" />
        </div>
    </as:ASModalContainer>
    <as:RadCodeBlock ID="JavaScript" runat="server">
        <script type="text/javascript">
            var rm_WorkQueueRedistributionStatus_uxSubmitAjax = '<%= uxSubmitAjax.ClientID %>';
        </script>
        <script src="<%= ResolveUrl("~/")%>res/js/risk_MCF/rm_MCF_WorkQueueRedistributionStatus.js"></script>
    </as:RadCodeBlock>
</asp:Content>
