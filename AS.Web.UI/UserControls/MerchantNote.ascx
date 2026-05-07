<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MerchantNote.ascx.cs" Inherits="UserControls_MerchantNote" %>
<%@ Register Src="~/UserControls/UxExport.ascx" TagName="UxExport" TagPrefix="uc" %>
<as:RadAjaxManagerProxy ID="RadAjaxManager" runat="server">
    <AjaxSettings>
        <tek:AjaxSetting AjaxControlID="uxApplyFilter">
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="uxPanelMerchantNoteGrid" />
                <tek:AjaxUpdatedControl ControlID="uxPanelMerchantNoteGridExport" LoadingPanelID="uxInvisiblePanel" />
            </UpdatedControls>
        </tek:AjaxSetting>
        <tek:AjaxSetting AjaxControlID="uxSubmit">
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="uxPanelAddComment" />
                <tek:AjaxUpdatedControl ControlID="uxPanelMerchantNoteGrid" />
                <tek:AjaxUpdatedControl ControlID="uxPanelMerchantNoteGridExport" LoadingPanelID="uxInvisiblePanel" />
            </UpdatedControls>
        </tek:AjaxSetting>
        <tek:AjaxSetting AjaxControlID="uxBtnRefresh">
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="uxPanelMerchantNoteGrid" />
                <tek:AjaxUpdatedControl ControlID="uxPanelMerchantNoteGridExport" LoadingPanelID="uxInvisiblePanel" />
            </UpdatedControls>
        </tek:AjaxSetting>
        <tek:AjaxSetting AjaxControlID="uxPanelMerchantNoteGrid">
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="uxPanelMerchantNoteGrid" />
            </UpdatedControls>
        </tek:AjaxSetting>
        <tek:AjaxSetting AjaxControlID="hdSubmit">
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="uxPanelAddComment" />
                <tek:AjaxUpdatedControl ControlID="uxPanelMerchantNoteGrid" />
            </UpdatedControls>
        </tek:AjaxSetting>
        <tek:AjaxSetting AjaxControlID="uxFinishSaveDefaultSettingMN">
            <UpdatedControls>
                <%--<tek:AjaxUpdatedControl ControlID="uxSourceList" />
                <tek:AjaxUpdatedControl ControlID="uxRoleList" />
                <tek:AjaxUpdatedControl ControlID="uxAddedByList" />--%>
                <tek:AjaxUpdatedControl ControlID="uxPanelMerchantNoteGrid" />
                <tek:AjaxUpdatedControl ControlID="uxPanelMerchantNoteGridExport" LoadingPanelID="uxInvisiblePanel" />
            </UpdatedControls>
        </tek:AjaxSetting>
    </AjaxSettings>
</as:RadAjaxManagerProxy>

<div class="row" id="addnote" runat="server">
    <div class="col-md-12">
        <h2 class="grid-title" data-toggle="collapse" data-target="#ciAddNote">
            <as:Literal ID="Literal1" runat="server" Text="Add Note" meta:resourcekey="uxAddNoteTitleResource1"></as:Literal>
        </h2>
        <div class="box-add-note in" id="ciAddNote">
            <div class="box-editor">
                <span class="text-node-editor">
                    <as:Literal ID="ltDocument1" runat="server" Text="Enter new comment (Limit 7000 characters per comment)" meta:resourcekey="ltDocument1Resource1"></as:Literal></span>
                <asp:Panel runat="server" ID="uxPanelAddComment">
                    <tek:RadEditor ID="uxComment" runat="server" EditModes="Design" ContentFilters="ConvertCharactersToEntities, ConvertToXhtml, FixEnClosingP"
                        StripFormattingOptions="MSWordRemoveAll" OnClientLoad="OnClientLoad"
                        ToolsFile="~/App_Data/RadEditorConfig.xml" Height="200px" Width="100%" Font-Names="Arial" OnClientPasteHtml="onHtmlPaste">
                        <CssFiles>
                            <tek:EditorCssFile Value="~/res/css/Editor.css" />
                        </CssFiles>
                    </tek:RadEditor>
                </asp:Panel>
                <div class="bottom-error">
                    <label class="error display-none" id="ciCommentsMsg"></label>
                </div>
            </div>

            <div class="box-footer-editor">
                <span id="counter"></span>
                <div class="ml-auto text-right">
                    <asp:Button ID="uxSubmit" runat="server" Text="Submit" OnClick="uxSubmit_Click" OnClientClick="return Validate();" class="btn btn-default" meta:resourcekey="uxSubmitResource1" />
                    <%--[43454] - Encrypting the comments field in VW Case Management --%>
                    <asp:HiddenField ID="hdCardDetected" runat="server" />
                </div>
            </div>

        </div>
    </div>
</div>
<div class="row" id="merchantnotes">
    <div class="col-md-12">
        <div class="mn-default-link" id="defaultSetting">
            <as:LinkButton runat="server" ID="uxDefaultSettingLink" Text="DEFAULT SETTING" CssClass="link-back" meta:resourcekey="uxDefaultSettingLinkResource1"></as:LinkButton>
        </div>
        <asp:Panel runat="server" ID="uxPanelMerchantNoteGridExport">
            <uc:UxExport ID="uxExportTop" ShowPDF="false" GridID="uxMerchantNoteGrid" GridTitle="Merchant Notes" OnNeedExportConfig="uxExportTop_NeedExportConfig" runat="server" meta:resourcekey="uxMerchantNoteTitleResource1" />
        </asp:Panel>
        <div class="box-filter" id="boxFilter">
            <div id="uxProgress" class="hide" style="padding: auto; position: absolute; vertical-align: middle; text-align: center; z-index: 9999; height: 45px; width: 99%; top: -1px; left: 5px; bottom: 0px; right: 5px; background: #FFF url('/res/Images/loading.gif') no-repeat center center">
            </div>
            <div class="form-group w-30" id="multiSourceList">
                <asp:Label ID="lbSource" CssClass="control-label mn-filter-label" runat="server" Text="Source:" meta:resourcekey="lbSourceResource1"></asp:Label>
                <div class="multichooser-wrapper">
                    <as:MultiChooser IsSearchContains="true" Width="100%" CssClass="form-control" ID="uxSourceList" AutoPostBack="false" runat="server" Placeholder=" " meta:resourcekey="uxSourceResource1"></as:MultiChooser>
                </div>
            </div>
            <div class="form-group w-30">
                <asp:Label ID="lbRole" CssClass="control-label mn-filter-label" runat="server" Text="Role:" meta:resourcekey="lbRoleResource1"></asp:Label>
                <div class="multichooser-wrapper">
                    <as:MultiChooser IsSearchContains="true" Width="100%" CssClass="form-control" ID="uxRoleList" AutoPostBack="false" runat="server" Placeholder=" " meta:resourcekey="uxRoleResource1"></as:MultiChooser>
                </div>
            </div>
            <div class="form-group w-30">
                <asp:Label ID="lbAddedBy" CssClass="control-label mn-filter-label" runat="server" Text="Added by:" meta:resourcekey="lbAddedByResource1"></asp:Label>
                <div class="multichooser-wrapper">
                    <as:MultiChooser IsSearchContains="true" Width="100%" CssClass="form-control" ID="uxAddedByList" AutoPostBack="false" runat="server" Placeholder=" " meta:resourcekey="uxAddedByResource1"></as:MultiChooser>
                </div>
            </div>
            <as:Button runat="server" ID="uxApplyFilter" CssClass="btn btn-default" Text="Apply" OnClick="uxApplyFilter_Click" meta:resourcekey="uxApplyFilterResource1" />
        </div>
        <asp:Panel runat="server" ID="uxPanelMerchantNoteGrid">
            <div class="grid-card-view">
                <as:ASGrid ID="uxMerchantNoteGrid" runat="server" AllowFilteringByColumn="false" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" AllowSortFilterWhenExport="true" ShowHeader="false"
                    ASPagingMethod="SPASingleMethod1" OnItemDataBound="uxMerchantNoteGrid_ItemDataBound" GridName="Merchant Notes" OnNeedDataSource="uxMerchantNoteGrid_NeedDataSource" CssClass="in" XOverFlowable="false"
                    OnPageIndexChanged="uxMerchantNoteGrid_PageIndexChanged" OnPageSizeChanged="uxMerchantNoteGrid_PageSizeChanged" OnItemCommand="uxMerchantNoteGrid_ItemCommand" ItemStyle-CssClass="no-backgound" meta:resourcekey="uxMerchantNoteReportGridResource1"
                    IsAutoExportTemplate="true">
                    <MasterTableView>
                        <Columns>
                            <as:ASGridBoundColumn HeaderText="Notes Source" DataField="NotesSourceDesc" UniqueName="NotesSourceDesc" Visible="false"
                                SortExpression="NotesSourceDesc" HeaderTooltip="Note Sources" ASFormat="StaticString" meta:resourcekey="ASGridTemplateColumnResource2"
                                HeaderStyle-Width="150px">
                            </as:ASGridBoundColumn>
                            <as:ASGridBoundColumn HeaderText="Note Title" DataField="NoteTitle" UniqueName="NoteTitle" Visible="false"
                                SortExpression="NoteTitle" HeaderTooltip="Note Title" ASFormat="StaticString" meta:resourcekey="ASGridTemplateColumnResource1"
                                HeaderStyle-Width="150px">
                            </as:ASGridBoundColumn>
                            <as:ASGridBoundColumn HeaderText="Case ID - Case Title" DataField="CaseNumberTitle" UniqueName="CaseNumberTitle" Visible="false"
                                SortExpression="CaseNumberTitle" HeaderTooltip="Case ID - Case Title" ASFormat="StaticString" meta:resourcekey="ASGridTemplateColumnResource6"
                                HeaderStyle-Width="150px">
                            </as:ASGridBoundColumn>
                            <as:ASGridBoundColumn HeaderText="Comment" DataField="Comment" UniqueName="Comment" Visible="false"
                                SortExpression="Comment" HeaderTooltip="Comment" ASFormat="StaticString" meta:resourcekey="ASGridTemplateColumnResource3"
                                HeaderStyle-Width="150px">
                            </as:ASGridBoundColumn>
                            <as:ASGridBoundColumn HeaderText="Created On" DataField="CreationDate" UniqueName="CreationDate" Visible="false"
                                SortExpression="CreationDate" HeaderTooltip="Creation Date" ASFormat="StaticString" meta:resourcekey="ASGridTemplateColumnResource5"
                                HeaderStyle-Width="150px">
                            </as:ASGridBoundColumn>
                            <as:ASGridBoundColumn HeaderText="User Name" DataField="UserNameFull" UniqueName="UserNameFull" Visible="false"
                                SortExpression="UserNameFull" HeaderTooltip="UserNameFull" ASFormat="StaticString" meta:resourcekey="ASGridTemplateColumnResource4"
                                HeaderStyle-Width="150px">
                            </as:ASGridBoundColumn>
                            <as:ASGridBoundColumn HeaderText="Hierarchy Name" DataField="HierarchyName" UniqueName="HierarchyName" Visible="false"
                                SortExpression="HierarchyName" HeaderTooltip="Hierarchy Name" ASFormat="StaticString" meta:resourcekey="ASGridTemplateColumnResource7"
                                HeaderStyle-Width="150px">
                            </as:ASGridBoundColumn>
                            <as:ASGridTemplateColumn DataField="CardView" UniqueName="CardView" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-CssClass="td-card-view">
                                <ItemTemplate>
                                    <div class="card-view-container">
                                        <%--Sprint 5 - VIS-25 change UI design--%>
                                        <div class="">
                                            <div class="row">
                                                <div class="col-md-3">
                                                    <%--Source--%>
                                                    <as:Button ID="uxSortIcon_NotesSourceDesc" runat="server" CssClass="rgSortAsc hide" />
                                                    <asp:LinkButton ID="lbtSort_NotesSourceDesc" CssClass="rdHeader mn-link mn-w80" runat="server" Text="Source:" CommandName="Sort_NotesSourceDesc" meta:resourcekey="ASGridTemplateColumnCardResource1"></asp:LinkButton>
                                                    <span class="text-group"><%# Eval("NotesSourceDesc") %></span>
                                                </div>
                                                <div runat="server" class="col-md-3" id="ucCase">
                                                    <%--Case Id - Case title--%>
                                                    <as:Button ID="uxSortIcon_CaseNumber" runat="server" CssClass="rgSortAsc hide" />
                                                    <asp:LinkButton ID="lbtSort_CaseNumber" CssClass="rdHeader mn-link" runat="server" Text="Case:" CommandName="Sort_CaseNumber" meta:resourcekey="ASGridTemplateColumnCardResource4"></asp:LinkButton>
                                                    <span id="caseIdCol" class="text-label-link" runat="server"><%# Eval("CaseID") %></span>
                                                </div>
                                                <div class="col-md-3">
                                                    <%--Creation On--%>
                                                    <as:Button ID="uxSortIcon_CreationDate" runat="server" CssClass="rgSortAsc hide" />
                                                    <asp:LinkButton ID="lbtSort_CreationDate" CssClass="rdHeader mn-link mn-w80" runat="server" Text="Created On:" CommandName="Sort_CreationDate" meta:resourcekey="ASGridTemplateColumnCardResource3"></asp:LinkButton>
                                                    <span class="text-group"><%# Eval("CreationDate") %></span>
                                                </div>
                                                <div class="col-md-3">
                                                    <%--Added by--%>
                                                    <as:Button ID="uxSortIcon_UserNameFull" runat="server" CssClass="rgSortAsc hide" />
                                                    <asp:LinkButton ID="lbtSort_UserNameFull" CssClass="rdHeader mn-link mn-w80" runat="server" Text="Added By:" CommandName="Sort_UserNameFull" meta:resourcekey="ASGridTemplateColumnCardResource2"></asp:LinkButton>
                                                    <span runat="server" id="ucAddedBy" class="text-group"></span>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-md-12 mt-5x edit-note-comment-overflow-text">
                                                    <%--Comment--%>
                                                    <div id="ucComment" class="comment-text" runat="server"></div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </ItemTemplate>
                            </as:ASGridTemplateColumn>
                        </Columns>
                    </MasterTableView>
                </as:ASGrid>
            </div>
        </asp:Panel>
        <asp:Button ID="uxBtnRefresh" runat="server" Text="Refresh" OnClick="uxBtnRefresh_Click" class="btn btn-default hide" meta:resourcekey="uxSubmitResource1" />
    </div>
</div>
<asp:HiddenField ID="uxhdAddedBy" runat="server" />
<asp:HiddenField ID="uxhdRole" runat="server" />
<asp:Button ID="uxFinishSaveDefaultSettingMN" OnClick="uxFinishSaveDefaultSettingMN_Click" runat="server" CssClass="hide" />

<as:RadCodeBlock ID="radCodeBlock2" runat="server">
    <script type="text/javascript">
        var MerchantNote_uxSourceList = "<%=uxSourceList.ClientID %>";
        var MerchantNote_uxRoleList = "<%=uxRoleList.ClientID %>";
        var MerchantNote_uxAddedByList = "<%=uxAddedByList.ClientID %>";
        var uxMerchantNoteGrid = "<%= uxMerchantNoteGrid.ClientID%>";
        var MerchantProfile_uxComment = "<%=uxComment.ClientID %>";
        var AddNote_js_msg1 = '<%= GetLocalResourceObject("AddNote_js_msg1").ToString() %>';
        var AddNote_js_Characters = '<%= GetLocalResourceObject("AddNote_js_Characters").ToString() %>';
        var AddNote_js_Required = '<%= GetLocalResourceObject("AddNote_js_Required").ToString() %>';
        var AddNote_uxSubmit = "<%=uxSubmit.UniqueID %>";
        var AddNote_uxApplyFilter = "<%=uxApplyFilter.ClientID %>";
        var hdCardDetected = '<%=hdCardDetected.ClientID %>';
        var urlCheckValidCard = '<%= ResolveUrl("~/" + this.Request.CurrentExecutionFilePath)%>' + "/CheckSensitiveData";
        var uxhdAddedBy_ClientID = '<%= uxhdAddedBy.ClientID%>';
        var uxhdRole_ClientID = '<%= uxhdRole.ClientID%>';
        var uxDefaultSettingLinkMN_ClientID = '<%= uxDefaultSettingLink.ClientID%>';
        var openDefaultSettingUrlMN = '<%= OpenDefaultSettingUrlForMerchantNote%>';
        var uxFinishSaveDefaultSettingMN_ClientID = '<%= uxFinishSaveDefaultSettingMN.ClientID%>';
        var uxBtnRefresh = "<%= uxBtnRefresh.ClientID%>";
    </script>
    <script type="text/javascript" src="<%=ResolveUrl("~") %>res/js/MerchantNote.js"></script>
</as:RadCodeBlock>
