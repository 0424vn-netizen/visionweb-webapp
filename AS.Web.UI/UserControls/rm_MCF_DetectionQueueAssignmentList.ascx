<%@ Control Language="C#" AutoEventWireup="true" CodeFile="rm_MCF_DetectionQueueAssignmentList.ascx.cs"
    Inherits="UserControls_rm_MCF_DetectionQueueAssignmentList" %>

<%@ Register TagName="ProgressBar" TagPrefix="uc" Src="~/UserControls/ProgressBar.ascx" %>

<as:RadAjaxManagerProxy ID="RadAjaxManager1" runat="server">
    <AjaxSettings>
        <tek:AjaxSetting AjaxControlID="uxAssignmentList">
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="uxAssignmentList" />
            </UpdatedControls>
        </tek:AjaxSetting>
        <tek:AjaxSetting AjaxControlID="btnViewAssignment">
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
    <MasterTableView>
        <Columns>
            <as:ASGridBoundColumn HeaderText="" DataField="AssignmentID" UniqueName="AssignmentID"
                Visible="false" meta:resourcekey="ASGridBoundColumnResource1">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
            </as:ASGridBoundColumn>

            <as:ASGridTemplateColumn HeaderText="Assignment Name" UniqueName="AssignmentName" HeaderStyle-Width="230px"
                HeaderStyle-HorizontalAlign="Center" AllowSorting="false"
                DataField="AssignmentName" HeaderTooltip="The assignment name hyperlink navigates to the Manage Assignment modal" meta:resourcekey="ASGridTemplateColumnResource1">
                <ItemTemplate>
                    <asp:LinkButton ID="lnkViewAssignment" runat="server" OnClientClick='<%# "viewAssignment(" + Eval("AssignmentID") + "); return false;" %>'
                        Text='<%# Eval("AssignmentName") %>'></asp:LinkButton>
                    <input style="display: none;" id="uxWorkedVolumeValue" value='<%# Eval("WorkedVolume") %>' />
                </ItemTemplate>

                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
            </as:ASGridTemplateColumn>
            <as:ASGridBoundColumn HeaderText="Type" DataField="AssignmentType" UniqueName="AssignmentType" AllowSorting="false"
                Visible="true" ASFormat="StaticString" HeaderTooltip="Assignment Type" SortExpression="AssignmentType" meta:resourcekey="ASGridBoundColumnResource2">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
            </as:ASGridBoundColumn>
            <as:ASGridBoundColumn HeaderText="Count" DataField="TotalMerchantCount" ItemStyle-HorizontalAlign="Right" UniqueName="TotalMerchantCount"
                Visible="true" ASFormat="Integer" HeaderTooltip="Number of distinct merchants that qualify for an assignment based on the assignment filters selected (a merchant can re-alert for an assignment, but is counted as eligible once)"
                SortExpression="TotalMerchantCount" AllowSorting="false" meta:resourcekey="ASGridBoundColumnResource13" ASDefaultNullValue="&mdash;">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>
                <ItemStyle CssClass="eligible" />
                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
            </as:ASGridBoundColumn>
            <as:ASGridBoundColumn HeaderText="Count" DataField="AlertMerchantCount"
                UniqueName="AlertMerchantCount" Visible="true" ASFormat="Integer" ASDefaultNullValue="&mdash;"
                HeaderTooltip="Number of distinct merchants that have alerted or re-alerted based on the assignment parameter configurations"
                SortExpression="AlertMerchantCount" ItemStyle-CssClass="alertCount" AllowSorting="false" meta:resourcekey="ASGridBoundColumnResource14">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                <ItemStyle CssClass="alertCount"></ItemStyle>
            </as:ASGridBoundColumn>

            <as:ASGridBoundColumn HeaderText="Net Amount" DataField="AlertMerchantVolume" UniqueName="AlertMerchantVolume"
                Visible="true" ASFormat="Currency" HeaderTooltip="Cumulative amount associated with the Alerted Count"
                AllowSorting="false" meta:resourcekey="ASGridBoundColumnResource15" ASDefaultNullValue="N/A">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>
                <ItemStyle CssClass="merchantVolume" />
                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
            </as:ASGridBoundColumn>
            <as:ASGridBoundColumn HeaderText="Count" DataField="WKCount" ASDefaultNullValue="&mdash;"
                UniqueName="WKCount" Visible="true" ASFormat="Integer" ItemStyle-CssClass="wipCount"
                SortExpression="WKCount" HeaderTooltip="Number of distinct merchants that have alerted or re-alerted, but have not been worked"
                AllowSorting="false" meta:resourcekey="ASGridBoundColumnResource16">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                <ItemStyle CssClass="wipCount"></ItemStyle>
            </as:ASGridBoundColumn>
            <as:ASGridBoundColumn HeaderText="Net Amount" DataField="WKVolume" ASDefaultNullValue="&mdash;"
                UniqueName="WKVolume" Visible="true" ASFormat="Currency" ItemStyle-CssClass="wipVolume"
                SortExpression="WKVolume" HeaderTooltip="Cumulative amount associated with the Ready to Work - Count"
                AllowSorting="false" meta:resourcekey="ASGridBoundColumnResource17">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                <ItemStyle CssClass="wipVolume"></ItemStyle>
            </as:ASGridBoundColumn>
            <as:ASGridBoundColumn HeaderText="Count" DataField="WIPCount" ASDefaultNullValue="&mdash;"
                UniqueName="WIPCount" Visible="true" ASFormat="Integer" ItemStyle-CssClass="wipCount"
                SortExpression="WIPCount" HeaderTooltip="Number of distinct merchants marked as Work in Progress"
                AllowSorting="false" meta:resourcekey="ASGridBoundColumnResource11">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                <ItemStyle CssClass="wipCount"></ItemStyle>
            </as:ASGridBoundColumn>
            <as:ASGridBoundColumn HeaderText="Net Amount" DataField="WIPVolume" ASDefaultNullValue="&mdash;"
                UniqueName="WIPVolume" Visible="true" ASFormat="Currency" ItemStyle-CssClass="wipVolume"
                SortExpression="WIPVolume" HeaderTooltip="Cumulative amount associated with the Work in Progress - Count"
                AllowSorting="false" meta:resourcekey="ASGridBoundColumnResource18">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                <ItemStyle CssClass="wipVolume"></ItemStyle>
            </as:ASGridBoundColumn>


            <as:ASGridBoundColumn HeaderText="Count" DataField="WorkedCount" ASDefaultNullValue="&mdash;"
                UniqueName="WorkedCount" Visible="true" ASFormat="Integer" ItemStyle-CssClass="workedCount"
                SortExpression="WorkedCount" HeaderTooltip="Number of distinct merchants worked "
                AllowSorting="false" meta:resourcekey="ASGridBoundColumnResource19">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                <ItemStyle CssClass="workedCount"></ItemStyle>
            </as:ASGridBoundColumn>
            <as:ASGridBoundColumn HeaderText="Net Amount" DataField="WorkedVolume" ASDefaultNullValue="&mdash;"
                UniqueName="WorkedVolume" Visible="true" ASFormat="Currency" ItemStyle-CssClass="workedVolume"
                SortExpression="WorkedVolume" HeaderTooltip="Cumulative amount associated with the Worked - Count"
                AllowSorting="false" meta:resourcekey="ASGridBoundColumnResource20">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                <ItemStyle CssClass="workedVolume"></ItemStyle>
            </as:ASGridBoundColumn>
            <as:ASGridTemplateColumn DataField="CompletedPercent" UniqueName="CompleteTemplate" ASDefaultNullValue="&mdash;"
                HeaderText="Percent Worked" HeaderStyle-HorizontalAlign="Center" ItemStyle-CssClass="percent-column" AllowSorting="false"
                HeaderTooltip="Percent of merchants worked in relation to the number of merchants alerted = Worked Count  / Alerted Count" meta:resourcekey="ASGridTemplateColumnResource21">
                <ItemTemplate>
                    <uc:ProgressBar ID="progressBar" runat="server" Value='<%# Eval("CompletePercent") %>' />
                </ItemTemplate>

                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                <ItemStyle CssClass="percent-column"></ItemStyle>
            </as:ASGridTemplateColumn>
            <as:ASGridBoundColumn HeaderText="Merchants Alerted and Re-Alerted" DataField="AlertMerchantCountAllCycles" UniqueName="AlertMerchantCountAllCycles"
                ASFormat="Integer" SortExpression="AlertMerchantCountAllCycles" HeaderTooltip="Cumulative number of merchants alerted and re-alerted. Example: If a merchant is alerted once, then re-alerted twice, it will be counted three times."
                meta:resourcekey="ASGridBoundColumnResource22" HeaderStyle-Width="120px" AllowSorting="false">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
            </as:ASGridBoundColumn>
            <as:ASGridBoundColumn HeaderText="Count" DataField="RequeueCount" UniqueName="RequeueCount" ASDefaultNullValue="&mdash;"
                ASFormat="Integer" ItemStyle-CssClass="requeuedCount" SortExpression="RequeueCount" AllowSorting="false"
                HeaderTooltip="Number of distinct merchants that have been added to a Work Queue Assignment from a Detection Queue Assignment using the Re-Queue or Auto Queue feature" meta:resourcekey="ASGridBoundColumnResource8">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                <ItemStyle HorizontalAlign="Right"></ItemStyle>
            </as:ASGridBoundColumn>
            <as:ASGridBoundColumn HeaderText="Net Amount" DataField="RequeueVolume" UniqueName="RequeueVolume" ASDefaultNullValue="&mdash;"
                ASFormat="Currency" ItemStyle-CssClass="requeuedVolume" SortExpression="RequeueVolume" AllowSorting="false"
                HeaderTooltip="Cumulative amount associated with the Re-queued Count" meta:resourcekey="ASGridBoundColumnResource23">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                <ItemStyle HorizontalAlign="Right"></ItemStyle>
            </as:ASGridBoundColumn>
            <%--<as:ASGridBoundColumn UniqueName="CompletedPercent" DataField="CompletedPercent" ASDefaultNullValue="N/A"
                AllowSorting="false"
                Visible="false" meta:resourcekey="ASGridBoundColumnResource10">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
            </as:ASGridBoundColumn>--%>
        </Columns>
    </MasterTableView>

</as:ASGrid>
<div class="hide">
    <uc:ProgressBar runat="server" />
</div>
<as:HiddenField ID="hddAssignmentValue" runat="server" />
<as:Button ID="btnViewAssignment" CssClass="hide" runat="server" OnClick="btnViewAssignment_Click" />
<as:ASRadCodeBlock ID="radCodeBlock" runat="server">
    <script type="text/javascript">
        var rm_AssignmentList_uxAssignmentList = '<%= uxAssignmentList.ClientID %>';
        var rm_AssignmentList_enumDetectionQueue_Client = '<%=(int)WebSiteEnums.AssignmentType.DetectionQueue %>';
        var rm_AssignmentList_uxAssignmentList_ClientID = '<%= uxAssignmentList.ClientID %>';
        var rm_AssignmentList_enumWorkQueue_Client = '<%=(int)WebSiteEnums.AssignmentType.WorkQueue %>';
        var rm_AssignmentList_enumDetectionQueueDistinct_Client = '<%=(int)WebSiteEnums.AssignmentType.DetectionQueueDistinct %>';
        var rm_AssignmentList_FromPage = '<%= FromPage %>';
        //multi-language
        var rm_MCF_DetectionQueueAssignmentList_hasQueuingMechanism = '<%= GeneralFuncsLib.HasQueuingMechanismFeature %>'.toLowerCase() == 'true';
        var Text_Wip = '<%= this.GetLocalResourceObject("DectectionQueueAssignmentListJS_Text_Wip").ToString()%>';
        var Text_Worked = '<%= this.GetLocalResourceObject("DectectionQueueAssignmentListJS_Text_Worked").ToString()%>';
        var Text_MerchantCount = '<%= this.GetLocalResourceObject("DectectionQueueAssignmentListJS_Text_MerchantCount").ToString()%>';
        var Text_ReQueued = '<%= this.GetLocalResourceObject("DectectionQueueAssignmentListJS_Text_ReQueued").ToString()%>';

        var Text_Eligible = '<%= this.GetLocalResourceObject("DectectionQueueAssignmentListJS_Text_Eligible").ToString()%>';
        var Text_Alerted = '<%= this.GetLocalResourceObject("DectectionQueueAssignmentListJS_Text_Alerted").ToString()%>';
        var Text_ReadyToWork = '<%= this.GetLocalResourceObject("DectectionQueueAssignmentListJS_Text_ReadyToWork").ToString()%>';
        var Text_DistinctMerchantsCurrentStatus = '<%= this.GetLocalResourceObject("DectectionQueueAssignmentListJS_Text_DistinctMerchantsCurrentStatus").ToString()%>';
        var rm_AssignmentList_btnViewAssignment = '<%= btnViewAssignment.ClientID %>';
        var rm_AssignmentList_hddAssignmentValue = '<%= hddAssignmentValue.ClientID %>';
    </script>
    <script src="<%= ResolveUrl("~/")%>res/js/risk_MCF/rm_MCF_DetectionQueueAssignmentList.js"></script>
</as:ASRadCodeBlock>
