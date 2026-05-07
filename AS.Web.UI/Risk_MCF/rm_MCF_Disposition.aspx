<%@ Page Language="C#" AutoEventWireup="true" CodeFile="rm_MCF_Disposition.aspx.cs" Inherits="rm_MCF_Disposition" MasterPageFile="~/MasterPage.master" meta:resourcekey="PageResource1" %>

<%@ Register Src="~/UserControls/PageTitle.ascx" TagName="PageTitle" TagPrefix="uc" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="Server">
    <as:RadAjaxManagerProxy ID="RadAjaxManager1" runat="server">
        <AjaxSettings>
            <tek:AjaxSetting AjaxControlID="uxCreateMode">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="uxCreateMode" />
                </UpdatedControls>
            </tek:AjaxSetting>

            <tek:AjaxSetting AjaxControlID="btnReFresh">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="uxDispositionGrid" />
                </UpdatedControls>
            </tek:AjaxSetting>


            <tek:AjaxSetting AjaxControlID="uxUpdatePosition">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="uxUpdatePosition" />
                </UpdatedControls>
            </tek:AjaxSetting>
        </AjaxSettings>
    </as:RadAjaxManagerProxy>
    <div class="disposition-page">
        <div class="row">
            <div class="col-md-12 no-margin-bottom">
                <uc:PageTitle ID="uxPageTitle" runat="server" HasFilteringOption="false" ReportTitle="Disposition" meta:resourcekey="uxPageTitleResource1" />
            </div>
        </div>
        <div class="height-14"></div>
        <%--Create new Assignment--%>
        <div class="row">
            <div class="col-md-12 ">
                <as:LinkButton runat="server" ID="uxCreateMode" class="btn btn-default" OnClick="uxCreateMode_Click" meta:resourcekey="uxCreateModeResource1">Create New Reassignment</as:LinkButton>
            </div>
        </div>
        <div class="height-24"></div>
        <div class="row">
            <div class="col-md-12 ">
                <div class="box">
                    <i>
                        <as:Literal ID="ltAssignment" runat="server" Text="Default Disposition:" meta:resourcekey="ltAssignmentResource1"></as:Literal><br />
                        <as:Literal ID="ltPurpose" runat="server" Text="At least one active disposition must be selected as the default." meta:resourcekey="ltPurposeResource1"></as:Literal>
                    </i>
                </div>
            </div>
        </div>

        <div class="height-24"></div>
        <%--end Create new Assignment--%>

        <div class="row">
            <div class="col-md-12 dark-blue">
                <div class="control-inline">
                    <label class="first">
                        <as:Literal ID="ltStatus" runat="server" Text="Status:" meta:resourcekey="ltStatusResource1"></as:Literal></label>
                </div>
                <div class="control-inline">
                    <as:RadioButton ID="uxRabAll" runat="server" GroupName="StatusOpts" Checked="True"
                        Text="All" AutoPostBack="false" onclick="refreshData();" meta:resourcekey="uxAllResource1" Value="" />
                </div>
                <div class="control-inline">
                    <as:RadioButton ID="uxRabActive" runat="server" GroupName="StatusOpts" Text="Active"
                        AutoPostBack="false" onclick="refreshData();" meta:resourcekey="uxActiveResource1" Value="" />
                </div>
                <div class="control-inline">
                    <as:RadioButton ID="uxRabInactive" runat="server" GroupName="StatusOpts" Text="Inactive"
                        AutoPostBack="false" onclick="refreshData();" meta:resourcekey="uxInactiveResource1" Value="" />
                </div>
                <asp:LinkButton runat="server" ID="btnReFresh" CssClass="link-back hide" OnClick="btnReFresh_Click"></asp:LinkButton>

            </div>
        </div>
        <div class="row">
            <div class="col-md-12">
                <div class="group-disposition display-flex">
                    <div class="item"></div>
                    <div class="item">
                        <as:Literal ID="ucDefault" runat="server" meta:resourcekey="uxGridHeaderResource1"></as:Literal>
                    </div>
                    <div class="item item-name">
                        <as:Literal ID="ucDispositionName" runat="server" meta:resourcekey="uxGridHeaderResource2"></as:Literal>
                    </div>
                    <div class="item item-status">
                        <as:Literal ID="ucStatus" runat="server" meta:resourcekey="uxGridHeaderResource3"></as:Literal>
                    </div>
                    <div class="item">
                        <as:Literal ID="ucAction" runat="server" meta:resourcekey="uxGridHeaderResource4"></as:Literal>
                    </div>
                </div>
                <div id="listBoxDataFound" class="disposition-no-data">
                    <as:Literal ID="ucNoDataMsg" runat="server" meta:resourcekey="NoDetailRecords"></as:Literal>
                </div>
                <as:RadListBox runat="server" ID="uxDispositionGrid" Width="100%" CssClass="uxRightGrid-background" TransferMode="Move"
                    OnClientLoad="DispositionColumn.uxGridListBox_OnClientLoad" AllowAutomaticUpdates="true"
                    EnableDragAndDrop="true" OnClientDropping="DispositionColumn.uxGridListBox_OnClientDropping" OnClientDragging="DispositionColumn.uxGridListBox_OnClientDragging"
                    OnClientDragStart="DispositionColumn.uxGridListBox_OnClientDragStart" OnItemDataBound="uxDispositionGrid_ItemDataBound"
                    OnClientReordered="DispositionColumn.uxGridListBox_OnClientReordered" PersistClientChanges="true"
                    AllowReorder="true" ButtonSettings-ShowReorder="false" DataTextField="DispositionName" DataValueField="DispositionID">
                    <ItemTemplate>
                        <div class="display-flex space-between">
                            <div class="dropped-item disposition-dropped-item">
                                <span class="item-left-drop-ico DroppedItem disposition-show-ico">
                                    <a class="drop-col-item"></a>
                                </span>
                                <span class="item-left-ico">
                                    <a class="DragItem drag-col-item <%# (Eval("IsActive").ToString().ToLower().Equals("true"))? string.Empty :"hide" %>" style="max-width: 20px"></a>
                                </span>
                            </div>
                            <div class="disposition-checkmark <%#(Eval("IsDefault").ToString().ToLower().Equals("true"))?string.Empty:"visible" %>"></div>
                            <span class="col-name col-name-style disposition-col-name"><%# DataBinder.Eval(Container,"Text") %></span>
                            <div class="disposition-col-status">
                                <span><%# (Eval("IsActive").ToString().ToLower().Equals("true"))? GetLocalResourceObject("uxActiveResource1.Text")
                                      :GetLocalResourceObject("uxInactiveResource1.Text") %></span>
                            </div>
                            <div class="item action-link w-links disposition-col-action">
                                <as:LinkButton runat="server" ID="btnEdit" Text="Edit" meta:resourcekey="btnEditResource1"></as:LinkButton>
                            </div>
                        </div>
                    </ItemTemplate>
                </as:RadListBox>
            </div>

        </div>
        <as:Literal ID="uxMsg" runat="server" meta:resourcekey="uxMsgResource1"></as:Literal>
        <as:HiddenField ID="hdListDisposition" runat="server" />
        <asp:Button CssClass="hide" ID="uxUpdatePosition" OnClick="uxUpdatePosition_Click" runat="server" Text="Button" />
    </div>
    <tek:RadCodeBlock runat="server" ID="RadCodeBlock2">
        <script>
            var uxDispositionGridListBox = '<%= uxDispositionGrid.ClientID%>';
            var hdListDisposition_ClientID = '<%=hdListDisposition.ClientID%>';
            var uxUpdatePosition_ClientID = '<%=uxUpdatePosition.ClientID%>';
            var btnReFreshID = '<%= btnReFresh.ClientID%>';
        </script>
        <script type="text/javascript" src="<%=ResolveUrl("~") %>res/js/risk_MCF/rm_MCF_Disposition.js"></script>
    </tek:RadCodeBlock>
</asp:Content>
