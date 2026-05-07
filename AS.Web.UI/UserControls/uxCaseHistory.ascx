<%@ Control Language="C#" AutoEventWireup="true" CodeFile="uxCaseHistory.ascx.cs" Inherits="UserControls_uxCaseHistory" %>
<%@ Register Src="~/UserControls/UxExport.ascx" TagName="UxExport" TagPrefix="uc" %>

<as:RadAjaxManagerProxy ID="RadAjaxManager" runat="server">
    <AjaxSettings>
        <tek:AjaxSetting AjaxControlID="uxApply">
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="uxReportGridCaseHistory" />
                <tek:AjaxUpdatedControl ControlID="uxPanelCaseHistoryGridExport" LoadingPanelID="uxInvisiblePanel" />
            </UpdatedControls>
        </tek:AjaxSetting>
        <tek:AjaxSetting AjaxControlID="uxReportGridCaseHistory">
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="uxReportGridCaseHistory" />
            </UpdatedControls>
        </tek:AjaxSetting>
        <tek:AjaxSetting AjaxControlID="btn_Hidden">
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="uxStatuses" />
                <tek:AjaxUpdatedControl ControlID="uxPriorityLevel" />
            </UpdatedControls>
        </tek:AjaxSetting>
        <tek:AjaxSetting AjaxControlID="uxFinishSaveDefaultSettingCH">
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="uxStatuses" />
                <tek:AjaxUpdatedControl ControlID="uxTypes" />
                <tek:AjaxUpdatedControl ControlID="uxPriorityLevel" />
                <tek:AjaxUpdatedControl ControlID="uxReportGridCaseHistory" />
                <tek:AjaxUpdatedControl ControlID="uxPanelCaseHistoryGridExport" LoadingPanelID="uxInvisiblePanel" />
            </UpdatedControls>
        </tek:AjaxSetting>
    </AjaxSettings>
</as:RadAjaxManagerProxy>

<div class="row">
    <div class="col-md-12">
        <div class="mn-default-link" id="defaultSetting">
            <as:LinkButton runat="server" ID="uxDefaultSettingLink" Text="DEFAULT SETTING" CssClass="link-back" meta:resourcekey="uxDefaultSettingLinkResource1"></as:LinkButton>
        </div>
        <asp:Panel runat="server" ID="uxPanelCaseHistoryGridExport">
            <uc:UxExport ID="uxExporter" GridID="uxReportGridCaseHistory" GridTitle="Case History" OnNeedExportConfig="uxExportTop_NeedExportConfig" runat="server" meta:resourcekey="uxCaseHistoryTitleResource1" />
        </asp:Panel>
    </div>
</div>

<div class="row mb-5x">
    <div class="col-xs-12">
        <as:Button ID="uxbtnOpenNewCase2" runat="server" Text="Open New Case" CssClass="btn btn-default mb-20" Visible="false" meta:resourcekey="uxbtnOpenNewCaseResourceKey" class="btn btn-default" IsStandardButton="False" />
    </div>
</div>

<div class="row">
    <div class="col-md-12">
        <div class="box-filter" id="boxFilter">
            <div class="form-group w-30">
                <asp:Label ID="lbRole" CssClass="control-label mn-filter-label" runat="server" Text="Type:" meta:resourcekey="uxCaseHistoryTypeFilter"></asp:Label>
                <div class="multichooser-wrapper">
                    <as:MultiChooser  IsSearchContains="true" Width="100%" CssClass="form-control" ID="uxTypes" runat="server" Placeholder=" " meta:resourcekey="uxRoleResource1"></as:MultiChooser>
                </div>
            </div>
            <div class="form-group w-30" id="multiSourceList">
                <asp:Label ID="Literal3" CssClass="control-label mn-filter-label" runat="server" Text="Status:" meta:resourcekey="uxCaseHistoryStatusFilter"></asp:Label>
                <div class="multichooser-wrapper">
                    <as:MultiChooser IsInTelerikAjax="true" IsSearchContains="true" Width="100%" CssClass="form-control" ID="uxStatuses" AutoPostBack="false" runat="server" Placeholder=" " meta:resourcekey="uxSourceResource1"></as:MultiChooser>
                </div>
            </div>

            <div class="form-group w-30">
                <asp:Label ID="lbAddedBy" CssClass="control-label mn-filter-label" runat="server" Text="Added by:" meta:resourcekey="uxCaseHistoryPriorityFilter"></asp:Label>
                <div class="multichooser-wrapper">
                    <as:MultiChooser IsInTelerikAjax="true" IsSearchContains="true" Width="100%" CssClass="form-control" ID="uxPriorityLevel" runat="server" Placeholder=" " meta:resourcekey="uxAddedByResource1"></as:MultiChooser>
                </div>
            </div>
            <as:Button runat="server" CssClass="btn btn-default" ID="uxApply" Text="Apply" OnClick="uxApply_Click" IsStandardButton="False" meta:resourcekey="uxCaseHistoryApplyFilter" />
            <as:Button ID="btn_Hidden" CssClass="btn btn-default hide" runat="server" OnClick="ChangeCurrentType" Text="Cancel" meta:resourcekey="btnCancelResource1" />
        </div>
    </div>
</div>
<div class="section-case-history-card-view">
    <as:ASGrid ID="uxReportGridCaseHistory" runat="server" AllowFilteringByColumn="false" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" AllowSortFilterWhenExport="true" ShowHeader="false"
        ASPagingMethod="SPASingleMethod" OnItemDataBound="ItemDataBoundHistory" GridName="Case History" OnNeedDataSource="uxReportGridCaseHistory_NeedDataSource" CssClass="in" XOverFlowable="false"
        OnDataSourceReady="uxReportGridCaseHistory_DataSourceReady" OnItemCommand="uxReportGridCaseHistory_ItemCommand" ItemStyle-CssClass="no-backgound"
        meta:resourcekey="uxCaseHistoryReportGridResource1" IsAutoExportTemplate="true">

        <MasterTableView>
            <Columns>
                <as:ASGridBoundColumn HeaderText="Case ID - Case Title" Visible="False" DataField="CaseNumberTitle" UniqueName="CaseNumberTitle"
                    SortExpression="CaseNumberTitle" HeaderTooltip="Case Number" ItemStyle-HorizontalAlign="Left"
                    HeaderStyle-Width="60px" meta:resourcekey="ExportCaseIdTitle">
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn HeaderText="Status" DataField="Status" Visible="False" UniqueName="Status" ItemStyle-HorizontalAlign="Left"
                    SortExpression="Status" ASFormat="StaticString" meta:resourcekey="StatusResourcekey" />
                <as:ASGridBoundColumn SortExpression="SortOrderNegative" Visible="False" HeaderStyle-Width="100px" HeaderText="Priority"
                    DataField="PrioritySLR" UniqueName="Prioritylevel" ItemStyle-HorizontalAlign="Left" meta:resourcekey="PrioritylevelResourcekey">
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn HeaderText="Ownership Group" Visible="False" DataField="OwnershipGroup" UniqueName="OwnershipGroup"
                    SortExpression="OwnershipGroup" ASFormat="StaticString" HeaderTooltip="Ownership Group" meta:resourcekey="OwnershipGroupResourcekey" />
                <as:ASGridBoundColumn HeaderText="Assigned To" Visible="False" DataField="Assignedto" UniqueName="Assignedto"
                    SortExpression="Assignedto" ASFormat="StaticString" HeaderTooltip="Assigned To" meta:resourcekey="AssignedtoResourcekey" />
                <as:ASGridBoundColumn HeaderText="Opened By" Visible="False" HeaderStyle-Width="150px" DataField="OpenedBy"
                    UniqueName="OpenedBy" SortExpression="OpenedBy" ASFormat="StaticString" HeaderTooltip="Opened By" meta:resourcekey="OpenedByResourcekey" />
                <as:ASGridBoundColumn HeaderText="Role" Visible="False" HeaderStyle-Width="150px" DataField="Role"
                    UniqueName="Role" SortExpression="Role" ASFormat="StaticString" HeaderTooltip="Role" meta:resourcekey="RoleResourcekey" />
                <as:ASGridBoundColumn HeaderText="Type" Visible="False" HeaderStyle-Width="150px" DataField="CaseTypeDescription"
                    UniqueName="CaseType" SortExpression="CaseType" ASFormat="StaticString" meta:resourcekey="TypeExport" />
                <as:ASGridBoundColumn HeaderText="Opened On" Visible="False" DataField="OpenedOn" UniqueName="OpenedOn"
                    SortExpression="OpenedOn" HeaderStyle-Width="150px" ASFormat="DateAndTime12Hours" meta:resourcekey="OpenedOnResourcekey" />
                <as:ASGridBoundColumn HeaderText="Follow-up On" Visible="False" DataField="FollowUpOn" UniqueName="FollowUpOn"
                    SortExpression="FollowUpOn" HeaderStyle-Width="110px" ASFormat="Date" HeaderTooltip="Follow-up On" meta:resourcekey="FollowUpOnResourcekey" />
                <as:ASGridBoundColumn HeaderText="Last Updated On" Visible="False" DataField="LastUpdateOn" UniqueName="LastUpdateOn"
                    SortExpression="LastUpdateOn" HeaderStyle-Width="150px" ASFormat="DateAndTime12Hours"
                    HeaderTooltip="Last Updated On" meta:resourcekey="LastUpdateOnResourcekey" />
                <as:ASGridBoundColumn HeaderText="Closed On" Visible="False" DataField="ClosedOn" UniqueName="ClosedOn"
                    SortExpression="ClosedOn" HeaderStyle-Width="150px" ASFormat="DateAndTime12Hours"
                    HeaderTooltip="Closed On" meta:resourcekey="ClosedOnResourcekey" />
                <as:ASGridBoundColumn HeaderText="Reopened On" Visible="False" DataField="ReopenedDTS" UniqueName="ReopenedOn"
                    SortExpression="ReopenedOn" HeaderStyle-Width="150px" ASFormat="DateAndTime12Hours"
                    HeaderTooltip="Reopened On" meta:resourcekey="ReopenedOnResourceKey" />
                <as:ASGridBoundColumn HeaderText="Last Closed On" Visible="False" DataField="LastClosedOn" UniqueName="LastClosedOn"
                    SortExpression="LastClosedOn" HeaderStyle-Width="150px" ASFormat="DateAndTime12Hours"
                    meta:resourcekey="LastClosedOnResourceKey" />

                <as:ASGridTemplateColumn DataField="CardView" UniqueName="CardView" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-CssClass="td-card-view">
                    <ItemTemplate>
                        <div class="card-view-container">
                            <div class="card-header">
                                <div class="row flex-box">
                                    <div runat="server" id="CaseNumber" class="col-xs-6 text-label-link"></div>
                                    <div class="col-xs-6">
                                        <div class="flex-box">
                                            <span class="text-label">
                                                <as:Button ID="uxSortIcon_Status" runat="server" CssClass="hide" />
                                                <asp:LinkButton ID="lbtSort_Status" CssClass="rdHeader sort" runat="server" Text="Status:" CommandName="Sort_Status" meta:resourcekey="uxCaseHistoryStatusFilter"></asp:LinkButton>
                                            </span>
                                            <span class="text-group"><%# Eval("Status") %></span>
                                            <div runat="server" id="divPriorityLevel" class="ml-auto max-60"></div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="card-content">
                                <div class="row card-view">
                                    <div class="col-xs-3">
                                        <div class="form-group">
                                            <span class="text-label mw-116">
                                                <as:Button ID="uxSortIcon_OwnershipGroup" runat="server" CssClass="hide" />
                                                <asp:LinkButton ID="lbtSort_OwnershipGroup" CssClass="rdHeader sort" runat="server" Text="Ownership Group:" CommandName="Sort_OwnershipGroup" meta:resourcekey="ltOwnershipResource"></asp:LinkButton>
                                            </span>
                                            <div class="text-group" runat="server" id="OwnerShipGroup"><%# Eval("OwnershipGroup") %></div>
                                        </div>
                                    </div>
                                    <div class="col-xs-3">
                                        <div class="form-group">
                                            <span class="text-label mw-78">
                                                <as:Button ID="uxSortIcon_Assignedto" runat="server" CssClass="hide" />
                                                <asp:LinkButton ID="lbtSort_Assignedto" CssClass="rdHeader sort" runat="server" Text="Assigned To:" CommandName="Sort_Assignedto" meta:resourcekey="ltCaseHistoryAssignedToResource"></asp:LinkButton>
                                            </span>
                                            <div class="text-group" runat="server" id="Div3"><%# Eval("Assignedto") %></div>
                                        </div>
                                    </div>
                                    <div class="col-xs-3">
                                        <div class="form-group">
                                            <span class="text-label mw-90">
                                                <as:Button ID="uxSortIcon_OpenedBy" runat="server" CssClass="hide" />
                                                <asp:LinkButton ID="lbtSort_OpenedBy" CssClass="rdHeader sort" runat="server" Text="Opened By:" CommandName="Sort_OpenedBy" meta:resourcekey="OpenedByResource"></asp:LinkButton>
                                            </span>
                                            <div class="text-group" runat="server" id="Div4"><%# Eval("OpenedBy") %></div>
                                        </div>
                                    </div>
                                    <div class="col-xs-3">
                                        <div class="form-group">
                                            <span class="text-label mw-100">
                                                <as:Button ID="uxSortIcon_Role" runat="server" CssClass="hide" />
                                                <asp:LinkButton ID="lbtSort_Role" CssClass="rdHeader sort" runat="server" Text="Role:" CommandName="Sort_Role" meta:resourcekey="RoleResource"></asp:LinkButton>
                                            </span>
                                            <div class="text-group" runat="server" id="Div5"><%# Eval("Role") %></div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row card-view">
                                    <div class="col-xs-3">
                                        <div class="form-group">
                                            <span class="text-label mw-116">
                                                <as:Button ID="uxSortIcon_CaseTypeDescription" runat="server" CssClass="hide" />
                                                <asp:LinkButton ID="lbtSort_CaseTypeDescription" CssClass="rdHeader sort" runat="server" Text="Type:" CommandName="Sort_CaseTypeDescription" meta:resourcekey="uxCaseHistoryTypeFilter"></asp:LinkButton>
                                            </span>
                                            <div class="text-group" runat="server" id="Div6"><%# Eval("CaseTypeDescription") %></div>
                                        </div>
                                    </div>
                                    <div class="col-xs-3">
                                        <div class="form-group">
                                            <span class="text-label mw-78">
                                                <as:Button ID="uxSortIcon_OpenedOn" runat="server" CssClass="hide" />
                                                <asp:LinkButton ID="lbtSort_OpenedOn" CssClass="rdHeader sort" runat="server" Text="Opened On:" CommandName="Sort_OpenedOn" meta:resourcekey="OpenedOnResource"></asp:LinkButton>
                                            </span>
                                            <div class="text-group" runat="server" id="Div7"><%# Eval("OpenedOn") %></div>
                                        </div>
                                    </div>
                                    <div class="col-xs-3">
                                        <div class="form-group">
                                            <span class="text-label mw-90">
                                                <as:Button ID="uxSortIcon_FollowUpOn" runat="server" CssClass="hide" />
                                                <asp:LinkButton ID="lbtSort_FollowUpOn" CssClass="rdHeader sort" runat="server" Text="Follow-up On:" CommandName="Sort_FollowUpOn" meta:resourcekey="FollowupResource"></asp:LinkButton>
                                            </span>
                                            <div class="text-group" runat="server" id="Div8"><%# Eval("FollowUpOn") %></div>
                                        </div>
                                    </div>
                                    <div class="col-xs-3">
                                        <div class="form-group">
                                            <span class="text-label mw-100">
                                                <as:Button ID="uxSortIcon_LastUpdateOn" runat="server" CssClass="hide" />
                                                <asp:LinkButton ID="lbtSort_LastUpdateOn" CssClass="rdHeader sort" runat="server" Text="Last Update On:" CommandName="Sort_LastUpdateOn" meta:resourcekey="LastUpdateOnResource"></asp:LinkButton>
                                            </span>
                                            <div class="text-group" runat="server" id="Div9"><%# Eval("LastUpdateOn") %></div>
                                        </div>
                                    </div>
                                </div>

                                <div class="row card-view">
                                    <div class="col-xs-3"></div>
                                    <div class="col-xs-3">
                                        <div class="form-group last">
                                            <span class="text-label mw-78">
                                                <as:Button ID="uxSortIcon_ClosedOn" runat="server" CssClass="hide" />
                                                <asp:LinkButton ID="lbtSort_ClosedOn" CssClass="rdHeader sort" runat="server" Text="Closed On:" CommandName="Sort_ClosedOn" meta:resourcekey="ClosedOnResource"></asp:LinkButton>
                                            </span>
                                            <div class="text-group" runat="server" id="Div10"><%# Eval("ClosedOn") %></div>
                                        </div>
                                    </div>
                                    <div class="col-xs-3">
                                        <div class="form-group last">
                                            <span class="text-label mw-90">
                                                <as:Button ID="uxSortIcon_ReopenedDTS" runat="server" CssClass="hide" />
                                                <asp:LinkButton ID="lbtSort_ReopenedDTS" CssClass="rdHeader sort" runat="server" Text="Reopened On:" CommandName="Sort_ReopenedDTS" meta:resourcekey="ReopenedOnResource"></asp:LinkButton>
                                            </span>
                                            <div class="text-group" runat="server" id="Div11"><%# Eval("ReopenedDTS") %></div>
                                        </div>
                                    </div>
                                    <div class="col-xs-3">
                                        <div class="form-group last">
                                            <span class="text-label mw-100">
                                                <as:Button ID="uxSortIcon_LastClosedOn" runat="server" CssClass="hide" />
                                                <asp:LinkButton ID="lbtSort_LastClosedOn" CssClass="rdHeader sort" runat="server" Text="Last Closed On:" CommandName="Sort_LastClosedOn" meta:resourcekey="LastClosedOnResource"></asp:LinkButton>
                                            </span>
                                            <div class="text-group" runat="server" id="Div12"><%# Eval("LastClosedOn") %></div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                    </ItemTemplate>
                </as:ASGridTemplateColumn>
            </Columns>
        </MasterTableView>

    </as:ASGrid>
    <asp:Button ID="uxFinishSaveDefaultSettingCH" OnClick="uxFinishSaveDefaultSettingCH_Click" runat="server" CssClass="hide" />
</div>
<as:RadCodeBlock ID="radCodeBlock2" runat="server">
    <script type="text/javascript">
        var isCMSubmitAddDefaultSetting = false;
        var uxFinishSaveDefaultSettingCH_ClientID = '<%= uxFinishSaveDefaultSettingCH.ClientID%>'
        function openDefaulSettingModal() {
            var openDefaultSettingUrl = "<%= OpenDefaultSettingUrlForCH%>";
            var url = rootURL + 'DefaultSettingModal.aspx?' + openDefaultSettingUrl;
            return ShowPopupModal(url, 'auto');
        }

        var isCMSubmitAddDefaultSetting = false;
        function closeCHDefaultSettingModal() {
            if (isCMSubmitAddDefaultSetting) {
                document.getElementById(uxFinishSaveDefaultSettingCH_ClientID).click();
                isCMSubmitAddDefaultSetting = false;
            }
        }


        $(document).ready(function () {
            if ($("#casehistory").find(".report-export a").css("display") == "none") {
                $("#casehistory").find(".mn-default-link").css("right", "20px");

            }
           $('#<%=uxTypes.ClientID%>').on('change', function (e,params) {
                $("#<%=btn_Hidden.ClientID%>").click();
            });
        })
    </script>
</as:RadCodeBlock>
