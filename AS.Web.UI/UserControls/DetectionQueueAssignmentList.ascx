<%@ Control Language="C#" AutoEventWireup="true" CodeFile="DetectionQueueAssignmentList.ascx.cs"
    Inherits="UserControls_DetectionQueueAssignmentList" %>

<%@ Register TagName="ProgressBar" TagPrefix="uc" Src="~/UserControls/ProgressBar.ascx" %>

<as:RadAjaxManagerProxy ID="RadAjaxManager1" runat="server">
    <AjaxSettings>
        <tek:AjaxSetting AjaxControlID="uxAssignmentList">
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="uxAssignmentList" />
            </UpdatedControls>
        </tek:AjaxSetting>
    </AjaxSettings>
</as:RadAjaxManagerProxy>


<as:ASGrid ID="uxAssignmentList" runat="server" AutoGenerateColumns="false"
    AllowSorting="false" OnNeedDataSource="uxAssignmentList_NeedDataSource" ShowPager="false"
    ShowFooter="false"
    AllowPaging="true" OnItemDataBound="uxAssignmentList_ItemDataBound" Width="100%" CssClass="in" meta:resourcekey="uxAssignmentListResource1">
    <MasterTableView AllowCustomSorting="false">
        <Columns>
            <as:ASGridBoundColumn HeaderText="" DataField="AssignmentID" UniqueName="AssignmentID"
                Visible="false" meta:resourcekey="ASGridBoundColumnResource1">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
            </as:ASGridBoundColumn>

            <as:ASGridTemplateColumn HeaderText="Assignment Name" UniqueName="AssignmentName"  HeaderStyle-Width="230px"
                HeaderStyle-HorizontalAlign="Center"
                DataField="AssignmentName" HeaderTooltip="Click here to view the assignment" meta:resourcekey="ASGridTemplateColumnResource1">
                <ItemTemplate>
                    <asp:LinkButton ID="lnkViewAssignment" runat="server" OnCommand="lnkViewAssignment_Command"
                        CommandName="AssignmentID" CommandArgument='<%# Eval("AssignmentID") %>' Text='<%# Eval("AssignmentName") %>'></asp:LinkButton>
                    <input style="display: none;" id="uxWorkedVolumeValue" value='<%# Eval("WorkedVolume") %>' />
                </ItemTemplate>

                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
            </as:ASGridTemplateColumn>
            <as:ASGridBoundColumn HeaderText="Type" DataField="AssignmentType" UniqueName="AssignmentType"
                Visible="true" ASFormat="StaticString" HeaderTooltip="Assignment Type" SortExpression="AssignmentType" meta:resourcekey="ASGridBoundColumnResource2">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
            </as:ASGridBoundColumn>
            <as:ASGridBoundColumn HeaderText="Total Merch" DataField="TotalMerchantCount" UniqueName="TotalMerchantCount"
                Visible="true" ASFormat="Integer" HeaderTooltip="# of merchants matching assignment filters"
                SortExpression="TotalMerchantCount" AllowSorting="false" meta:resourcekey="ASGridBoundColumnResource3">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
            </as:ASGridBoundColumn>
            <as:ASGridBoundColumn HeaderText="Alert" DataField="AlertMerchantCount"
                UniqueName="AlertMerchantCount" Visible="true" ASFormat="Integer"
                HeaderTooltip="# of merchants matching assignment filters and parameters"
                SortExpression="AlertMerchantCount" ItemStyle-CssClass="alertCount" AllowSorting="false" meta:resourcekey="ASGridBoundColumnResource4">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                <ItemStyle CssClass="alertCount"></ItemStyle>
            </as:ASGridBoundColumn>

            <as:ASGridBoundColumn HeaderText="Volume" DataField="TotalMerchantVolume" UniqueName="TotalMerchantVolume"
                Visible="true" ASFormat="Currency" HeaderTooltip="Today's volume for merchants matching assignment"
                AllowSorting="false" meta:resourcekey="ASGridBoundColumnResource5">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
            </as:ASGridBoundColumn>
            <as:ASGridBoundColumn HeaderText="#" DataField="WorkedCount"
                UniqueName="WorkedCount" Visible="true" ASFormat="Integer" ItemStyle-CssClass="workedCount"
                SortExpression="WorkedCount" HeaderTooltip="# of merchants worked from this assignment"
                AllowSorting="false" meta:resourcekey="ASGridBoundColumnResource6">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                <ItemStyle CssClass="workedCount"></ItemStyle>
            </as:ASGridBoundColumn>
            <as:ASGridBoundColumn HeaderText="Volume" DataField="WorkedVolume"
                UniqueName="WorkedVolume" Visible="true" ASFormat="Currency" ItemStyle-CssClass="workedVolume"
                SortExpression="WorkedVolume" HeaderTooltip="Volume for merchants worked from this assignment"
                AllowSorting="false" meta:resourcekey="ASGridBoundColumnResource7">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                <ItemStyle CssClass="workedVolume"></ItemStyle>
            </as:ASGridBoundColumn>
            <as:ASGridBoundColumn HeaderText="#" DataField="RequeueCount" UniqueName="RequeueCount"
                ASFormat="Integer" ItemStyle-CssClass="requeuedCount" SortExpression="RequeueCount"
                HeaderTooltip="# of merchants re-queued from this assignment" meta:resourcekey="ASGridBoundColumnResource8">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                <ItemStyle CssClass="requeuedCount"></ItemStyle>
            </as:ASGridBoundColumn>
            <as:ASGridBoundColumn HeaderText="Volume" DataField="RequeueVolume" UniqueName="RequeueVolume"
                ASFormat="Currency" ItemStyle-CssClass="requeuedVolume" SortExpression="RequeueVolume"
                HeaderTooltip="Volume for merchants re-queued from this assignment" meta:resourcekey="ASGridBoundColumnResource9">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                <ItemStyle CssClass="requeuedVolume"></ItemStyle>
            </as:ASGridBoundColumn>
            <as:ASGridTemplateColumn DataField="CompletedPercent" UniqueName="CompleteTemplate"
                HeaderText="% Complete" HeaderStyle-HorizontalAlign="Center" ItemStyle-CssClass="percent-column"
                HeaderTooltip="% Complete" meta:resourcekey="ASGridTemplateColumnResource2">
                <ItemTemplate>
                    <uc:ProgressBar ID="progressBar" runat="server" Value='<%# Eval("CompletePercent") %>' />
                </ItemTemplate>

                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                <ItemStyle CssClass="percent-column"></ItemStyle>
            </as:ASGridTemplateColumn>
            <as:ASGridBoundColumn UniqueName="CompletedPercent" DataField="CompletedPercent"
                AllowSorting="false"
                Visible="false" meta:resourcekey="ASGridBoundColumnResource10">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
            </as:ASGridBoundColumn>
        </Columns>
    </MasterTableView>

</as:ASGrid>

<as:ASRadCodeBlock ID="radCodeBlock" runat="server">
    <script type="text/javascript">
        var rm_AssignmentList_uxAssignmentList = '<%= uxAssignmentList.ClientID %>';
        var rm_AssignmentList_assignmentType_Client = '<%=(int)_AssignmentType %>';
        var rm_AssignmentList_hasQueuingMechanism_Client = '<%=HasQueuingMechanism %>'.toLowerCase() == 'true';
        var rm_AssignmentList_enumDetectionQueue_Client = '<%=(int)WebSiteEnums.AssignmentType.DetectionQueue %>';
        var rm_AssignmentList_uxAssignmentList_ClientID = '<%= uxAssignmentList.ClientID %>';
        var rm_AssignmentList_enumWorkQueue_Client = '<%=(int)WebSiteEnums.AssignmentType.WorkQueue %>';
        var rm_AssignmentList_enumDetectionQueueDistinct_Client = '<%=(int)WebSiteEnums.AssignmentType.DetectionQueueDistinct %>';
    
        //multi-language
        var Text_Worked = '<%= this.GetLocalResourceObject("DectectionQueueAssignmentListJS_Text_Worked").ToString()%>';
        var Text_MerchantCount = '<%= this.GetLocalResourceObject("DectectionQueueAssignmentListJS_Text_MerchantCount").ToString()%>';
        var Text_ReQueued = '<%= this.GetLocalResourceObject("DectectionQueueAssignmentListJS_Text_ReQueued").ToString()%>';
    </script>
    <script src="<%= ResolveUrl("~/")%>res/js/risk/DetectionQueueAssignmentList.js"></script>
</as:ASRadCodeBlock>
