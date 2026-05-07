<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Risk_Parameter_MarketData.ascx.cs"
    Inherits="UserControls_Risk_Parameter_MarketData" %>

<%@ Register TagName="Export" Src="~/UserControls/UxExport.ascx" TagPrefix="uc" %>
<%@ Register TagName="RiskParameterFilter" Src="~/UserControls/Risk_ParameterFilter_TransactionCode.ascx"
    TagPrefix="uc" %>

<style type="text/css">
    .tbMerchantFilter td {
        border-width: 0px !important;
        height: 10px;
        padding: 0 0 0 10px !important;
    }

    .cellwidth {
        width: 30px;
    }
</style>
<%--<as:RadAjaxManagerProxy ID="ram" runat="server">
    <AjaxSettings>
        <tek:AjaxSetting AjaxControlID="uxParameterList">
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="uxParameterList" LoadingPanelID="ralp" />
            </UpdatedControls>
        </tek:AjaxSetting>
    </AjaxSettings>
</as:RadAjaxManagerProxy>--%>

<tek:RadAjaxLoadingPanel ID="ralp" runat="server" Skin="Default" BackgroundPosition="Top">
</tek:RadAjaxLoadingPanel>
<uc:Export ID="uxExportTop" runat="server" GridID="uxParameterList" OnNeedExportConfig="uxExport_OnNeedExportConfig" />
<as:ASGrid ID="uxParameterList" runat="server" AutoGenerateColumns="false" Width="100%"
    AllowPaging="false" PageSize="30" ShowFooter="true" AllowSortFilterWhenExport="true"
    AllowSorting="true" AllowMultiRowEdit="true" OnNeedDataSource="uxParameterList_NeedDataSource"
    OnItemDataBound="uxParameterList_ItemDataBound" ASPagingMethod="None" AllowExportAtWebServices="false"
    OnDetailTableDataBind="uxParameterList_DetailTableDataBind" OnPreRender="uxParameterList_PreRender" CssClass="in" meta:resourcekey="uxParameterListResource1">
    <MasterTableView Width="100%" ShowFooter="false" EditMode="InPlace" DataKeyNames="ParameterKey"
        AllowMultiColumnSorting="true" Name="ParentGrid" HierarchyDefaultExpanded="false"
        ExpandCollapseColumn-Display="false">
        <GroupByExpressions>
            <tek:GridGroupByExpression>
                <GroupByFields>
                    <tek:GridGroupByField FieldName="ParameterGroupNameDummy" FieldAlias="ParameterGroupNameDummy"
                        SortOrder="Ascending" meta:resourcekey="GridGroupByFieldResource2" />
                </GroupByFields>
                <SelectFields>
                    <tek:GridGroupByField FieldName="ParameterGroupNameDummy" FieldAlias="ParameterGroupNameDummy" meta:resourcekey="GridGroupByFieldResource1" />
                </SelectFields>
            </tek:GridGroupByExpression>
        </GroupByExpressions>
        <DetailTables>
            <tek:GridTableView Name="ParameterDetail" Width="100%" ShowHeadersWhenNoRecords="false"
                ShowHeader="false" AllowPaging="false"
                ExpandCollapseColumn-Display="false" ShowFooter="false" meta:resourcekey="GridTableViewResource1">
                <ExpandCollapseColumn Display="False"></ExpandCollapseColumn>
                <Columns>
                    <tek:GridTemplateColumn UniqueName="ReasonCode" meta:resourcekey="GridTemplateColumnResource1">
                        <ItemTemplate>
                            <as:Container ID="asContainer" runat="server" Width="100%" HeaderText="ReasonCode"
                                TemplateName="riskparamfilterbox.tpl" FooterControlID="" FooterText="" HeaderControlID="">
                                <div style="padding: 15px; text-align: justify;">
                                    <as:Literal ID="lblParamfilter" runat="server" meta:resourcekey="lblParamfilterResource1" />
                                </div>
                                <as:HiddenField ID="modalType" runat="server" />
                            </as:Container>
                        </ItemTemplate>
                    </tek:GridTemplateColumn>
                </Columns>
            </tek:GridTableView>
        </DetailTables>

        <ExpandCollapseColumn Display="False"></ExpandCollapseColumn>
        <Columns>
            <as:ASGridTemplateColumn UniqueName="ParameterCheckBox" HeaderStyle-HorizontalAlign="Left"
                ItemStyle-HorizontalAlign="Left" ItemStyle-BorderStyle="None" HeaderStyle-Width="3%"
                ItemStyle-Width="3%" HeaderTooltip="Activate/Deactivate" meta:resourcekey="ASGridTemplateColumnResource1">
                <HeaderTemplate>
                    <input type="checkbox" id="chkAllParameters" style="margin-left: 0px;" onclick="chkAllParameters_Click(this);" />
                </HeaderTemplate>
                <ItemTemplate>
                    <input runat="server" type="checkbox" id="chkRowIndex" style="margin-left: 6px;"
                        value='<%# Eval("ParameterRowNumber") %>' checked='<%# Convert.ToBoolean(Eval("ActivityStatus")) %>'
                        tabindex="0" />
                </ItemTemplate>

                <HeaderStyle HorizontalAlign="Left" Width="3%"></HeaderStyle>

                <ItemStyle HorizontalAlign="Left" BorderStyle="None" Width="3%"></ItemStyle>
            </as:ASGridTemplateColumn>
            <as:ASGridBoundColumn DataField="ParameterRowNumber" HeaderText="#" ItemStyle-HorizontalAlign="center"
                HeaderStyle-HorizontalAlign="Center" UniqueName="ParameterRowNumber" Visible="true"
                HeaderStyle-Width="7%" ItemStyle-Width="7%" meta:resourcekey="ASGridBoundColumnResource1">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center" Width="7%"></HeaderStyle>

                <ItemStyle HorizontalAlign="Center" Width="7%"></ItemStyle>
            </as:ASGridBoundColumn>
            <as:ASGridBoundColumn DataField="ParameterGroupNameDummy" UniqueName="GroupNameDummy"
                Visible="false" meta:resourcekey="ASGridBoundColumnResource2">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
            </as:ASGridBoundColumn>
            <as:ASGridBoundColumn DataField="ParameterGroupName" UniqueName="GroupName" Visible="false" meta:resourcekey="ASGridBoundColumnResource3">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
            </as:ASGridBoundColumn>
            <as:ASGridBoundColumn DataField="ParameterRowNumber" UniqueName="ParamRowNumber"
                Visible="false" meta:resourcekey="ASGridBoundColumnResource4">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
            </as:ASGridBoundColumn>
            <as:ASGridBoundColumn DataField="ParameterID" UniqueName="ParameterID" Visible="false" meta:resourcekey="ASGridBoundColumnResource5">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
            </as:ASGridBoundColumn>
            <as:ASGridBoundColumn DataField="ParameterKey" HeaderText="#" UniqueName="ParameterKey"
                Visible="true" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" meta:resourcekey="ASGridBoundColumnResource6">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                <ItemStyle HorizontalAlign="Center"></ItemStyle>
            </as:ASGridBoundColumn>
            <as:ASGridBoundColumn DataField="IsAssigned" UniqueName="IsAssigned" Visible="false" meta:resourcekey="ASGridBoundColumnResource7">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
            </as:ASGridBoundColumn>
            <as:ASGridBoundColumn DataField="ActivityStatus" UniqueName="ActivityStatus" Visible="false" meta:resourcekey="ASGridBoundColumnResource8">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
            </as:ASGridBoundColumn>
            <as:ASGridBoundColumn DataField="IsSelected" UniqueName="IsSelected" Visible="false" meta:resourcekey="ASGridBoundColumnResource9">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
            </as:ASGridBoundColumn>
            <as:ASGridBoundColumn DataField="ParameterCode" UniqueName="ParameterCode" SortExpression="ParameterCode"
                HeaderText="Code" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"
                ItemStyle-Width="5%" HeaderStyle-Width="5%" HeaderTooltip="Code" meta:resourcekey="ASGridBoundColumnResource10">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Left" Width="5%"></HeaderStyle>

                <ItemStyle HorizontalAlign="Left" Width="5%"></ItemStyle>
            </as:ASGridBoundColumn>
            <as:ASGridBoundColumn HeaderText="Parameter" DataField="ParameterName" UniqueName="ParameterName"
                HeaderTooltip="Parameter" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                SortExpression="ParameterName" ItemStyle-Width="60%" HeaderStyle-Width="60%" meta:resourcekey="ASGridBoundColumnResource11">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Left" Width="60%"></HeaderStyle>

                <ItemStyle HorizontalAlign="Left" Width="60%"></ItemStyle>
            </as:ASGridBoundColumn>
            <as:ASGridBoundColumn HeaderText="% / # / $" DataField="ParameterValueExport" UniqueName="ParameterValueExport"
                Display="false" meta:resourcekey="ASGridBoundColumnResource12">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
            </as:ASGridBoundColumn>
            <as:ASGridBoundColumn HeaderText="Threshold" DataField="ParameterThresholdExport"
                UniqueName="ParameterThresholdExport" Display="false" meta:resourcekey="ASGridBoundColumnResource13">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
            </as:ASGridBoundColumn>
            <as:ASGridTemplateColumn HeaderText="% / # / $" HeaderStyle-HorizontalAlign="Center"
                SortExpression="ParameterValue" UniqueName="ParameterValue" ItemStyle-VerticalAlign="Top"
                ItemStyle-Width="15%" HeaderStyle-Width="15%" HeaderTooltip="%/#/$" ItemStyle-HorizontalAlign="Center" meta:resourcekey="ASGridTemplateColumnResource2">
                <ItemTemplate>
                    <div id="div1" runat="server" style="text-align: left; padding-top: 2px; padding-bottom: 2px;">
                        <span style="width: 25px; float: left; padding-top: 3px; text-align: right; padding-right: 3px;">
                            <asp:Label ID="lblIndicatorDollar" runat="server" Text="&nbsp;&nbsp;" meta:resourcekey="lblIndicatorDollarResource1"></asp:Label>
                        </span>
                        <asp:TextBox ID="txtParameterValue" runat="server" onkeypress="return txtParameterValue_KeyPress(event);"
                            onblur="return txtParameterValue_Blur(event);" Text='<%# GetParameterNumber(Eval("ParameterValue").ToString()) %>'
                            Width="100px" onfocus="return txtParameterValue_Focus(event);"
                            Style="border: solid 1px gray;" meta:resourcekey="txtParameterValueResource1"></asp:TextBox>
                        <span id="spanErrMsg" runat="server" style="font-size: 14px;"></span>
                        <asp:Literal ID="lblParameterType" runat="server" Text='<%# setParamterType(Eval("ParameterDataType")) %>'></asp:Literal>
                        <div style="display: none">
                            <asp:TextBox ID="txtParameterID" runat="server" ReadOnly="True" Text='<%# Eval("ParameterKey").ToString() %>' meta:resourcekey="txtParameterIDResource1"></asp:TextBox>
                        </div>
                    </div>
                    <div id="uxMerchantFilter" runat="server" visible="False" style="text-align: left; padding-top: 0px; padding-bottom: 0px;">
                        <table border="0" cellpadding="0" cellspacing="0" class="tbMerchantFilter">
                            <tr>
                                <td>
                                    <asp:Literal ID="Literal1" runat="server" Text="From" meta:resourcekey="Literal1Resource1"></asp:Literal>
                                </td>
                                <td class="cellwidth">
                                    <asp:TextBox ID="txtFrom" runat="server" Width="100%" MaxLength="16" onkeypress="return txtParameterValue_KeyPress(event);"
                                        onblur="return txtParameterValue_Blur(event);" onfocus="return txtParameterValue_Focus(event);"
                                        Style="border: solid 1px gray;" meta:resourcekey="txtFromResource1"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:Literal ID="Literal2" runat="server" Text="To" meta:resourcekey="Literal2Resource1"></asp:Literal>
                                </td>
                                <td class="cellwidth">
                                    <asp:TextBox ID="txtTo" runat="server" Width="100%" MaxLength="16" onkeypress="return txtParameterValue_KeyPress(event);"
                                        onblur="return txtParameterValue_Blur(event);" onfocus="return txtParameterValue_Focus(event);"
                                        Style="border: solid 1px gray;" meta:resourcekey="txtToResource1"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:Literal ID="Literal3" runat="server" Text="day(s)" meta:resourcekey="Literal3Resource1"></asp:Literal>
                                </td>
                            </tr>
                        </table>
                    </div>
                </ItemTemplate>

                <HeaderStyle HorizontalAlign="Center" Width="15%"></HeaderStyle>

                <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" Width="15%"></ItemStyle>
            </as:ASGridTemplateColumn>
            <as:ASGridTemplateColumn HeaderText="Threshold" SortExpression="ParameterThreshold"
                ItemStyle-Width="15%" HeaderStyle-Width="15%" HeaderStyle-HorizontalAlign="Center"
                UniqueName="ParameterThreshold" HeaderTooltip="Threshold" ItemStyle-VerticalAlign="Top" meta:resourcekey="ASGridTemplateColumnResource3">
                <ItemTemplate>
                    <div id="dvThreshold" style="padding-top: 2px; padding-bottom: 2px;" runat="server">
                        <as:PlaceHolder ID="lblNA" runat="server" Visible="False">
                            <center>
                                <div style="padding-top: 4px;">
                                    <b><asp:Literal ID="Literal3" runat="server" Text="N/A" meta:resourcekey="Literal3Resource2"></asp:Literal> </b>
                                </div>
                            </center>
                        </as:PlaceHolder>
                        <as:PlaceHolder ID="uxThresholdNormal" runat="server">
                            <span style="width: 35px; float: left; padding-top: 3px; text-align: right; padding-right: 3px;">
                                <asp:Label ID="lblDollar" runat="server" Text="&nbsp;$" meta:resourcekey="lblDollarResource1"></asp:Label>
                            </span>
                            <span>
                                <asp:TextBox ID="txtThreshold" runat="server" Text='<%# GetParameterNumber(Eval("ParameterThreshold").ToString()) %>'
                                    Style="border: solid 1px gray;" Width="100px" onblur="return txtThreshold_Blur(event);"
                                    onfocus="return txtThreshold_Focus(event);" onkeypress="return txtThreshold_KeyPress(event);" meta:resourcekey="txtThresholdResource1"></asp:TextBox>
                            </span>
                            <span id="spanErrMsgThs" runat="server" style="font-size: 14px;"></span>
                            <span style="width: 10px; padding-top: 3px;">
                                <asp:Label ID="lblpercent" runat="server" Text='<%# setParamterType(Eval("ThresholdType")) %>' meta:resourcekey="lblpercentResource1"></asp:Label>
                            </span>
                            <div style="clear: both">
                            </div>
                        </as:PlaceHolder>
                        <as:PlaceHolder ID="uxThresholdLowHigh" runat="server" Visible="False">
                            <span style="width: 35px; float: left; padding-top: 3px; text-align: right; padding-right: 3px;">Low<asp:Label ID="lblDollarThresholdLow" runat="server" Text="&nbsp;" meta:resourcekey="lblDollarThresholdLowResource1"></asp:Label>
                            </span>
                            <span>
                                <asp:TextBox ID="txtThresholdLow" runat="server" Text='<%# GetParameterNumber(Eval("ParameterThreshold").ToString()) %>'
                                    Style="border: solid 1px gray;" Width="100px" onblur="return txtThreshold_Blur(event);"
                                    onfocus="return txtThreshold_Focus(event);" onkeypress="return txtThreshold_KeyPress(event);" meta:resourcekey="txtThresholdLowResource1"></asp:TextBox>
                            </span>
                            <span id="spanErrMsgThresholdLow" runat="server" style="font-size: 14px;"></span>
                            <span style="width: 10px; padding-top: 3px;">
                                <asp:Label ID="lblPercentThresholdLow" runat="server" Text='<%# setParamterType(Eval("ThresholdType")) %>' meta:resourcekey="lblPercentThresholdLowResource1"></asp:Label>
                            </span>
                            <div style="clear: both">
                            </div>
                            <div style="padding-top: 5px">
                                <span style="width: 35px; float: left; padding-top: 3px; text-align: right; padding-right: 3px;">
                                    <asp:Literal ID="Literal4" runat="server" Text="High" meta:resourcekey="Literal4Resource1"></asp:Literal><asp:Label ID="lblDollarThresholdHigh" runat="server" Text="&nbsp;" meta:resourcekey="lblDollarThresholdHighResource1"></asp:Label>
                                </span>
                                <span>
                                    <asp:TextBox ID="txtThresholdHigh" runat="server" Text='<%# GetParameterNumber(Eval("ParameterThresholdHigh").ToString()) %>'
                                        Style="border: solid 1px gray;" Width="100px" onblur="return txtThreshold_Blur(event);"
                                        onfocus="return txtThreshold_Focus(event);" onkeypress="return txtThreshold_KeyPress(event);" meta:resourcekey="txtThresholdHighResource1"></asp:TextBox>
                                </span>
                                <span id="spanErrMsgThresholdHigh" runat="server" style="font-size: 14px;"></span>
                                <span style="width: 10px; padding-top: 3px;">
                                    <asp:Label ID="lblPercentThresholdHigh" runat="server" Text='<%# setParamterType(Eval("ThresholdType")) %>' meta:resourcekey="lblPercentThresholdHighResource1"></asp:Label>
                                </span>
                            </div>
                            <div style="clear: both">
                            </div>
                        </as:PlaceHolder>
                    </div>
                </ItemTemplate>

                <HeaderStyle HorizontalAlign="Center" Width="15%"></HeaderStyle>

                <ItemStyle VerticalAlign="Top" Width="15%"></ItemStyle>
            </as:ASGridTemplateColumn>
            <as:ASGridBoundColumn DataField="UsedByAssignments" UniqueName="UsedByAssignments"
                Visible="false" EmptyDataText="" meta:resourcekey="ASGridBoundColumnResource14">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
            </as:ASGridBoundColumn>
            <as:ASGridBoundColumn DataField="UsedByRiskScores" UniqueName="UsedByRiskScores"
                Visible="false"
                EmptyDataText="" meta:resourcekey="ASGridBoundColumnResource15">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
            </as:ASGridBoundColumn>
            <as:ASGridBoundColumn DataField="ParameterValue" UniqueName="ParameterValueHide"
                Visible="false" meta:resourcekey="ASGridBoundColumnResource16">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
            </as:ASGridBoundColumn>
            <as:ASGridBoundColumn DataField="ParameterThreshold" UniqueName="ParameterThresholdHide"
                Visible="false" EmptyDataText="" meta:resourcekey="ASGridBoundColumnResource17">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
            </as:ASGridBoundColumn>
            <as:ASGridBoundColumn DataField="ParameterDataType" UniqueName="ParameterDataTypeHide"
                Visible="false" meta:resourcekey="ASGridBoundColumnResource18">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
            </as:ASGridBoundColumn>
            <as:ASGridBoundColumn DataField="ParameterName" UniqueName="ParameterNameHide" Visible="false" meta:resourcekey="ASGridBoundColumnResource19">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
            </as:ASGridBoundColumn>
            <as:ASGridBoundColumn DataField="ParameterThresholdType" UniqueName="ParameterThresholdType"
                Visible="false" meta:resourcekey="ASGridBoundColumnResource20">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
            </as:ASGridBoundColumn>
            <as:ASGridBoundColumn DataField="ThresholdMin" UniqueName="ThresholdMinValue" Visible="false" meta:resourcekey="ASGridBoundColumnResource21">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
            </as:ASGridBoundColumn>
            <as:ASGridBoundColumn DataField="ThresholdMax" UniqueName="ThresholdMaxValue" Visible="false" meta:resourcekey="ASGridBoundColumnResource22">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
            </as:ASGridBoundColumn>
            <as:ASGridBoundColumn DataField="IndicatorMin" UniqueName="IndicatorMin" Visible="false" meta:resourcekey="ASGridBoundColumnResource23">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
            </as:ASGridBoundColumn>
            <as:ASGridBoundColumn DataField="IndicatorMax" UniqueName="IndicatorMax" Visible="false" meta:resourcekey="ASGridBoundColumnResource24">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
            </as:ASGridBoundColumn>
            <as:ASGridBoundColumn DataField="ParameterDescription" UniqueName="ParameterDescription"
                Visible="false" meta:resourcekey="ASGridBoundColumnResource25">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
            </as:ASGridBoundColumn>
            <as:ASGridBoundColumn DataField="IsSelected" UniqueName="IsSelected" Visible="false"
                EmptyDataText="" meta:resourcekey="ASGridBoundColumnResource26">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
            </as:ASGridBoundColumn>
            <as:ASGridBoundColumn DataField="IsActive" UniqueName="IsActive" Visible="false"
                EmptyDataText="" meta:resourcekey="ASGridBoundColumnResource27">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
            </as:ASGridBoundColumn>
            <as:ASGridBoundColumn DataField="IndicatorNegative" UniqueName="IndicatorNegative"
                Visible="false" EmptyDataText="" meta:resourcekey="ASGridBoundColumnResource28">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
            </as:ASGridBoundColumn>
            <as:ASGridBoundColumn DataField="ThresholdNegative" UniqueName="ThresholdNegative"
                Visible="false" EmptyDataText="" meta:resourcekey="ASGridBoundColumnResource29">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
            </as:ASGridBoundColumn>
        </Columns>
    </MasterTableView>
</as:ASGrid>
<uc:Export ID="uxExportBottom" runat="server" GridID="uxParameterList" style="padding-right: 5px; float: right; display: inline;"
    IsBottom="true" OnNeedExportConfig="uxExport_OnNeedExportConfig" />
<div style="visibility: hidden; display: none;">
    <as:Button ID="btSave" runat="server" OnClick="btSave_Click" Style="display: none;" IsStandardButton="False" meta:resourcekey="btSaveResource1" />
</div>
<as:HiddenField ID="hfSelectedKeys" runat="server" />
<as:HiddenField runat="server" ID="uxCurrentParam" />
<as:HiddenField runat="server" ID="uxPramKey" />
<as:Button ID="btnRefreshParamList" runat="server" OnClick="uxClose_Click" Style="display: none"
    IsStandardButton="True" meta:resourcekey="btnRefreshParamListResource1" />
<as:RadCodeBlock ID="radCodeBlock" runat="server">
    <!-- parameter for Risk_Parameter.js -->
    <script type="text/javascript">
        var ERR_MAXEXCEED = '<%= Resources.MessageManager.RiskParameter_js_ValueMayNotExceed %>' + ' ';
        var ERR_MINEXCEED = '<%= Resources.MessageManager.RiskParameter_js_ValueMayNotLowerThan %>' + ' ';
        var ERR_NUMONLY = '<%= Resources.MessageManager.RiskParameter_js_OnlyNumbers %>';
        var ERR_REQUIREDFIELD = ' : ' + '<%= Resources.MessageManager.RiskParameter_js_RequiredField %>';
        var ERR_INTNUMBER = '<%= Resources.MessageManager.RiskParameter_js_IntergerNumber %>';
        var ERR_TOLOWERFROM = '<%= Resources.MessageManager.RiskParameter_js_ToMustBeGreaterThanFrom %>';
        var ERR_LOWGTHIGH = '<%= Resources.MessageManager.RiskParameter_js_HighMustBeGreaterThanLow %>';
        var RiskParameter_js_MustBeSelected = '<%= Resources.MessageManager.RiskParameter_js_MustBeSelected%>';
        var RiskParameter_js_ThePartMustBeLessThan = '<%= Resources.MessageManager.RiskParameter_js_ThePartMustBeLessThan%>';
        var RiskParameter_js_MsgDisablingParameter = '<%= Resources.MessageManager.RiskParameter_js_MsgDisablingParameter%>';
        var RiskParameter_js_Msg1DisablingParameter = '<%= Resources.MessageManager.RiskParameter_js_Msg1DisablingParameter%>';
        var RiskParameter_js_MonitoringParameter = '<%= Resources.MessageManager.RiskParameter_js_MonitoringParameter%>';
        var RiskParameter_js_RemoveTheParameter = '<%= Resources.MessageManager.RiskParameter_js_RemoveTheParameter%>';
        var RiskParameter_js_AssignmentMonitoringParam = '<%= Resources.MessageManager.RiskParameter_js_AssignmentMonitoringParam%>';
        var RiskParameter_js_AssignmentDisablingParam = '<%= Resources.MessageManager.RiskParameter_js_AssignmentDisablingParam%>';
        var RiskParameter_js_RiskScoreDisablingParam = '<%= Resources.MessageManager.RiskParameter_js_RiskScoreDisablingParam%>';
        var RiskParameter_js_IndicatorFrom = '<%= Resources.MessageManager.RiskParameter_js_IndicatorFrom%>';
        var RiskParameter_js_IndicatorTo = '<%= Resources.MessageManager.RiskParameter_js_IndicatorTo%>';
        var RiskParameter_js_Threshold = '<%= Resources.MessageManager.RiskParameter_js_Threshold%>';
        var RiskParameter_js_Indicator = '<%= Resources.MessageManager.RiskParameter_js_Indicator%>';
    </script>
    <script language="javascript" type="text/javascript" src="../res/js/common/riskparameter.js"></script>
    <script language="javascript" type="text/javascript">
        //==================================== API =====================================================
        function doSave() {
            var pm = checkParameterValid();
            if (!pm)
                return false;

            //do postback
            var bt = $get("<%=btSave.ClientID %>");
            bt.click();
        }

        function doSaveParameterList(isNeedSelectParam) {
            if (isNeedSelectParam) {
                var pm = checkParameterValid();
                if (!pm)
                    return false;
            }
            //do postback
            var bt = $get("<%=btSave.ClientID %>");
            bt.click();
        }

        function checkParameterValid() {
            var count = countSelectedParameters();
            if (count == 0) {
                alert('<%= GetLocalResourceObject("Risk_Parameter_MarketData_ascx_ParamMustBeSelected").ToString() %>');
                return false;
            }
            return true;
        }

        function checkParameterValid1() {
            var count = countSelectedParameters();
            if (count == 0) {
                return false;
            }
            return true;
        }
        function parameter_ShowFilter(url, index, paramkey, width, height) {
            document.getElementById('<%=uxCurrentParam.ClientID %>').value = index;
            document.getElementById('<%=uxPramKey.ClientID %>').value = paramkey;
            ShowPopupModal(url, 'auto');

        }
        parent._modalID = '';
        parent.setModalID = function (id) {
            parent._modalID = id;
        }
        parent.ReloadParamList = function () {
            document.getElementById('<%=btnRefreshParamList.ClientID %>').click();
        }
        window.onload = function () {
            setCssClass();
        }
        function setCssClass() {
            $('.rgDetailTable').each(function () {
                var className = $(this).parent().parent().prev().attr('class');
                $(this).parent().parent().addClass(className);
                $(this).parent().css("border-left-width", "1px");
            });
        }
    </script>

</as:RadCodeBlock>
