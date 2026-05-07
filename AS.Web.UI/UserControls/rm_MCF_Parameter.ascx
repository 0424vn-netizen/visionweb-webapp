<%@ Control Language="C#" AutoEventWireup="true" CodeFile="rm_MCF_Parameter.ascx.cs"
    Inherits="UserControls_rm_MCF_Parameter" %>

<%@ Register TagName="Export" Src="~/UserControls/UxExport.ascx" TagPrefix="uc" %>

<as:RadAjaxManagerProxy ID="ram" runat="server">
    <AjaxSettings>
        <tek:AjaxSetting AjaxControlID="uxParameterList">
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="uxParameterList" />
            </UpdatedControls>
        </tek:AjaxSetting>
    </AjaxSettings>
</as:RadAjaxManagerProxy>
<uc:Export ID="uxExportTop" runat="server" IsOnTop="true" GridID="uxParameterList"
    OnNeedExportConfig="uxExport_OnNeedExportConfig" ShowPDF="false" />
<as:ASGrid ID="uxParameterList" runat="server" AutoGenerateColumns="false" CssClass="ASTable parameters-table"
    AllowPaging="false" PageSize="30" ShowFooter="true" AllowSortFilterWhenExport="true" IsAutoExportTemplate="true"
    AllowSorting="true" AllowMultiRowEdit="true" OnNeedDataSource="uxParameterList_NeedDataSource" IsCacheTemplateFile="false"
    OnItemDataBound="uxParameterList_ItemDataBound" OnColumnCreated="uxParameterList_ColumnCreated" XOverFlowable="false"
    ASPagingMethod="None" AllowExportAtWebServices="false" meta:resourcekey="uxParameterListResource1">
    <MasterTableView ShowFooter="false" EditMode="InPlace" ExpandCollapseColumn-Display="false"
        GroupHeaderItemStyle-Width="0" ExpandCollapseColumn-Visible="false">
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

        <ExpandCollapseColumn Visible="False" Display="False"></ExpandCollapseColumn>
        <Columns>
            <as:ASGridTemplateColumn UniqueName="ParameterCheckBox" HeaderStyle-HorizontalAlign="Center"
                ItemStyle-HorizontalAlign="Center" HeaderTooltip="Activate/Deactivate" meta:resourcekey="ASGridTemplateColumnResource1">
                <HeaderTemplate>
                    <input type="checkbox" id="chkAllParameters" onclick="chkAllParameters_Click(this);" />
                </HeaderTemplate>
                <ItemTemplate>
                    <input runat="server" type="checkbox" id="chkRowIndex" value='<%# Eval("ParameterRowNumber") %>'
                        checked='<%# Convert.ToBoolean(Eval("ActivityStatus")) %>' tabindex="0" />
                </ItemTemplate>

                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                <ItemStyle HorizontalAlign="Center"></ItemStyle>
            </as:ASGridTemplateColumn>
            <as:ASGridBoundColumn DataField="ParameterRowNumber" HeaderText="#" ItemStyle-HorizontalAlign="center"
                HeaderStyle-HorizontalAlign="Center" UniqueName="ParameterRowNumber" Visible="true" meta:resourcekey="ASGridBoundColumnResource1">

                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                <ItemStyle HorizontalAlign="Center"></ItemStyle>
            </as:ASGridBoundColumn>
            <as:ASGridBoundColumn DataField="ParameterGroupNameDummy" UniqueName="GroupNameDummy"
                Display="false" meta:resourcekey="ASGridBoundColumnResource2">

                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
            </as:ASGridBoundColumn>
            <as:ASGridBoundColumn DataField="ParameterGroupName" UniqueName="GroupName" Display="false" meta:resourcekey="ASGridBoundColumnResource3">

                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
            </as:ASGridBoundColumn>
            <as:ASGridBoundColumn DataField="ParameterRowNumber" UniqueName="ParamRowNumber"
                Display="false" meta:resourcekey="ASGridBoundColumnResource4">

                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
            </as:ASGridBoundColumn>
            <as:ASGridBoundColumn DataField="ParameterID" UniqueName="ParameterID" Display="false" meta:resourcekey="ASGridBoundColumnResource5">

                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
            </as:ASGridBoundColumn>
            <as:ASGridBoundColumn DataField="ParameterKey" HeaderText="#" UniqueName="ParameterKey"
                Visible="true" ItemStyle-CssClass="parameterkey"
                HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" meta:resourcekey="ASGridBoundColumnResource6">

                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                <ItemStyle HorizontalAlign="Center"></ItemStyle>
            </as:ASGridBoundColumn>
            <as:ASGridBoundColumn DataField="IsAssigned" UniqueName="IsAssigned" Display="false" meta:resourcekey="ASGridBoundColumnResource7">

                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
            </as:ASGridBoundColumn>
            <as:ASGridBoundColumn DataField="ActivityStatus" UniqueName="ActivityStatus" Display="false" meta:resourcekey="ASGridBoundColumnResource8">

                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
            </as:ASGridBoundColumn>
            <as:ASGridBoundColumn DataField="IsSelected" UniqueName="IsSelected" Display="false" meta:resourcekey="ASGridBoundColumnResource9">

                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
            </as:ASGridBoundColumn>
            <as:ASGridBoundColumn DataField="ParameterCode" UniqueName="ParameterCode"
                SortExpression="ParameterCode" HeaderText="Code" ItemStyle-HorizontalAlign="Left"
                HeaderStyle-HorizontalAlign="Left" HeaderTooltip="Code" meta:resourcekey="ASGridBoundColumnResource10">

                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>

                <ItemStyle HorizontalAlign="Left"></ItemStyle>
            </as:ASGridBoundColumn>
            <as:ASGridBoundColumn HeaderText="Parameter" DataField="ParameterName" UniqueName="ParameterName"
                HeaderTooltip="Parameter" ItemStyle-HorizontalAlign="Left" SortExpression="ParameterName"
                HeaderStyle-Width="20%" meta:resourcekey="ASGridBoundColumnResource11" ItemStyle-CssClass="parameter">

                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center" Width="20%"></HeaderStyle>

                <ItemStyle HorizontalAlign="Left"></ItemStyle>
            </as:ASGridBoundColumn>

            <as:ASGridBoundColumn HeaderText="ParameterDescription" DataField="ParameterDescription" UniqueName="ParameterDescription"
                HeaderTooltip="ParameterDescription" ItemStyle-HorizontalAlign="Left" SortExpression="ParameterDescription"
                HeaderStyle-Width="50%" meta:resourcekey="ASGridBoundColumnResource17" ItemStyle-CssClass="parameter">

                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center" Width="50%"></HeaderStyle>

                <ItemStyle HorizontalAlign="Left"></ItemStyle>
            </as:ASGridBoundColumn>
            <as:ASGridBoundColumn HeaderText="Parameter Description" DataField="ParameterDescription" UniqueName="ParameterDescriptionExport"
                Display="false" meta:resourcekey="ASGridBoundColumnResource17">

                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                <ItemStyle HorizontalAlign="Left"></ItemStyle>
            </as:ASGridBoundColumn>

            <as:ASGridBoundColumn HeaderText="Indicator" DataField="ParameterValueExport" UniqueName="ParameterValueExport"
                Display="false" meta:resourcekey="ASGridBoundColumnResource12">

                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
            </as:ASGridBoundColumn>

            <as:ASGridBoundColumn HeaderText="Indicator" DataField="ParameterValueExportCSV" UniqueName="ParameterValueExportCSV"
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

            <as:ASGridBoundColumn HeaderText="Re Alert Indicator" DataField="ParameterValueExportNRT" UniqueName="ParameterValueExportNRT"
                Display="false" meta:resourcekey="ASGridBoundColumnResource15">

                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
            </as:ASGridBoundColumn>

            <as:ASGridBoundColumn HeaderText="Indicator" DataField="ParameterValueExportCSVNRT" UniqueName="ParameterValueExportCSVNRT"
                Display="false" meta:resourcekey="ASGridBoundColumnResource15">

                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
            </as:ASGridBoundColumn>

            <as:ASGridBoundColumn HeaderText="Re Alert Threshold" DataField="ParameterThresholdExportNRT"
                UniqueName="ParameterThresholdExportNRT" Display="false" meta:resourcekey="ASGridBoundColumnResource16">

                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
            </as:ASGridBoundColumn>

            <as:ASGridTemplateColumn HeaderText="Indicator" HeaderStyle-HorizontalAlign="Center"
                SortExpression="ParameterValue" UniqueName="ParameterValue"
                HeaderTooltip="Indicator" ItemStyle-CssClass="text-nowrap" meta:resourcekey="ASGridTemplateColumnResource2">
                <ItemTemplate>
                    <div id="div1" runat="server" class="text-left">
                        <asp:Label ID="IndicatorType" runat="server" Text="">
                            <label class="no-margin">
                                <asp:Label ID="lblIndicatorDollar" runat="server" meta:resourcekey="lblIndicatorDollarResource1"></asp:Label>
                            </label>
                            <div class="form-inline control-inline last">
                                <asp:TextBox ID="txtParameterValue" runat="server" onpaste="return false" onkeypress="return txtParameterValue_KeyPress(event);"
                                    Text='<%# GetParameterNumber(Eval("ParameterValue").ToString()) %>'
                                    Width="100px"
                                    CssClass="form-control" meta:resourcekey="txtParameterValueResource1"></asp:TextBox>
                            </div>
                        </asp:Label>
                        <span id="spanErrMsg" runat="server"></span>

                        <label class="after-label">
                            <asp:Literal ID="lblParameterType" runat="server" Text='<%# setParamterType(Eval("ParameterDataType")) %>'></asp:Literal>
                        </label>

                        <div class="display-none">
                            <asp:TextBox ID="txtParameterID" runat="server" ReadOnly="True" Text='<%# Eval("ParameterKey").ToString() %>' meta:resourcekey="txtParameterIDResource1"></asp:TextBox>
                        </div>
                    </div>
                    <div id="uxMerchantFilter" runat="server" visible="False">
                        <div class="row">
                            <div class="control-inline">
                                <label>
                                    &nbsp&nbsp&nbsp<as:Literal ID="ltFrom" runat="server" Text="From" meta:resourcekey="ltFromResource1"></as:Literal></label>
                                <asp:TextBox ID="txtFrom" runat="server" Width="100px" MaxLength="16" onpaste="return false" onkeypress="return txtParameterValue_KeyPress(event);"
                                    CssClass="form-control" Text='<%# GetParameterNumber(Eval("ParameterValue").ToString()) %>' meta:resourcekey="txtFromResource1"></asp:TextBox>
                            </div>
                            <div class="control-inline">
                                <label>
                                    <as:Literal ID="Literal1" runat="server" Text="To" meta:resourcekey="Literal1Resource1"></as:Literal></label>

                                <asp:TextBox ID="txtTo" runat="server" Width="100px" MaxLength="16" onpaste="return false" onkeypress="return txtParameterValue_KeyPress(event);"
                                    CssClass="form-control" Text='<%# GetParameterNumber(Eval("ParameterValueHigh").ToString()) %>' meta:resourcekey="txtToResource1"></asp:TextBox>
                                <label>
                                    <asp:Literal ID="Literal5" runat="server" Text='<%# setParamterType(Eval("ParameterDataType")) %>'></asp:Literal>
                                    <%--<as:Literal ID="Literal2" runat="server" Text="day(s)" meta:resourcekey="Literal2Resource1"></as:Literal>--%>
                                </label>
                            </div>

                        </div>
                    </div>
                    <div id="msgError" class="error text-space text-left"></div>
                </ItemTemplate>

                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                <ItemStyle VerticalAlign="Top" CssClass="text-center text-nowrap"></ItemStyle>
            </as:ASGridTemplateColumn>
            <as:ASGridTemplateColumn HeaderText="Threshold" SortExpression="ParameterThreshold"
                HeaderStyle-Width="250px" HeaderStyle-HorizontalAlign="Center"
                UniqueName="ParameterThreshold" HeaderTooltip="Threshold" ItemStyle-HorizontalAlign="Center"
                ItemStyle-CssClass="text-center text-nowrap" meta:resourcekey="ASGridTemplateColumnResource3">
                <ItemTemplate>
                    <div id="dvThreshold" runat="server" class="text-left">
                        <as:PlaceHolder ID="lblNA" runat="server" Visible="False">
                            <div>
                                <b>
                                    <as:Literal ID="Literal22" runat="server" Text="" meta:resourcekey="Literal2Resource2_"></as:Literal></b>
                            </div>
                        </as:PlaceHolder>
                        <as:PlaceHolder ID="uxThresholdNormal" runat="server">
                            <label class="no-margin">
                                <asp:Label ID="lblDollar" runat="server" Text="&nbsp;$" meta:resourcekey="lblDollarResource1"></asp:Label>
                            </label>
                            <div class="form-inline control-inline last">
                                <asp:TextBox ID="txtThreshold" runat="server" onpaste="return false" Text='<%# GetParameterNumber(Eval("ParameterThreshold").ToString()) %>'
                                    CssClass="form-control" Width="100px"
                                    onkeypress="return txtThreshold_KeyPress(event);" meta:resourcekey="txtThresholdResource1"></asp:TextBox>
                            </div>
                            <span id="spanErrMsgThs" runat="server"></span>
                            <label class="after-label">
                                <asp:Label ID="lblpercent" runat="server" Text='<%# setParamterType(Eval("ThresholdType")) %>' meta:resourcekey="lblpercentResource1"></asp:Label>
                            </label>
                        </as:PlaceHolder>
                        <as:PlaceHolder ID="uxThresholdLowHigh" runat="server" Visible="False">

                            <div class="row">
                                <label class="before-label">
                                    &nbsp<as:Literal ID="Literal3" runat="server" Text="Low" meta:resourcekey="Literal3Resource1"></as:Literal>&nbsp<asp:Label ID="lblDollarThresholdLow" runat="server" Text="&nbsp;" meta:resourcekey="lblDollarThresholdLowResource1"></asp:Label>
                                </label>

                                <asp:TextBox ID="txtThresholdLow" runat="server" Text='<%# GetParameterNumber(Eval("ParameterThreshold").ToString()) %>'
                                    Width="100px"
                                    onpaste="return false"
                                    onkeypress="return txtThreshold_KeyPress(event);"
                                    CssClass="form-control" meta:resourcekey="txtThresholdLowResource1"></asp:TextBox>
                                <span id="spanErrMsgThresholdLow" runat="server"></span>
                                <label class="after-label">
                                    <asp:Label ID="lblPercentThresholdLow" runat="server" Text='<%# setParamterType(Eval("ThresholdType")) %>' meta:resourcekey="lblPercentThresholdLowResource1"></asp:Label>
                                </label>

                            </div>
                            <div class="height-4">
                            </div>
                            <div class="row">
                                <label class="before-label">
                                    &nbsp<as:Literal ID="Literal4" runat="server" Text="High" meta:resourcekey="Literal4Resource1"></as:Literal>&nbsp<asp:Label ID="lblDollarThresholdHigh" runat="server" Text="&nbsp;" meta:resourcekey="lblDollarThresholdHighResource1"></asp:Label>
                                </label>

                                <asp:TextBox ID="txtThresholdHigh" runat="server" Text='<%# GetParameterNumber(Eval("ParameterThresholdHigh").ToString()) %>'
                                    Width="100px"
                                    onpaste="return false"
                                    onkeypress="return txtThreshold_KeyPress(event);"
                                    CssClass="form-control" meta:resourcekey="txtThresholdHighResource1"></asp:TextBox>
                                <span id="spanErrMsgThresholdHigh" runat="server"></span>
                                <label class="after-label">
                                    <asp:Label ID="lblPercentThresholdHigh" runat="server" Text='<%# setParamterType(Eval("ThresholdType")) %>' meta:resourcekey="lblPercentThresholdHighResource1"></asp:Label>
                                </label>

                            </div>
                        </as:PlaceHolder>
                    </div>
                    <div id="msgError" class="error text-space text-left"></div>
                </ItemTemplate>

                <HeaderStyle HorizontalAlign="Center" Width="250px"></HeaderStyle>

                <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" CssClass="text-center text-nowrap"></ItemStyle>
            </as:ASGridTemplateColumn>

            <as:ASGridTemplateColumn HeaderText="Indicator" HeaderStyle-HorizontalAlign="Center"
                SortExpression="ReAlertParameterIndicator" UniqueName="ReAlertParameterIndicator" ItemStyle-VerticalAlign="Top"
                HeaderTooltip="Indicator" ItemStyle-CssClass="text-nowrap" meta:resourcekey="ASGridTemplateColumnResource2">
                <ItemTemplate>
                    <div id="div1_NRT" runat="server" class="text-left">
                        <asp:Label ID="indicatorMCFType" runat="server" Text="">
                            <label class="no-margin">
                                <asp:Label ID="lblNRTIndicatorDollar" runat="server" meta:resourcekey="lblIndicatorDollarResource1"></asp:Label>
                            </label>
                            <div class="form-inline control-inline last">
                                <asp:TextBox ID="txtNRTParameterValue" runat="server" onpaste="return false" onkeypress="return txtParameterValue_KeyPress(event, true);"
                                    Text='<%# GetParameterNumber(Eval("ReAlertParameterIndicator").ToString()) %>'
                                    Width="100px"
                                    CssClass="form-control" meta:resourcekey="txtParameterValueResource1"></asp:TextBox>
                            </div>
                        </asp:Label>
                        <span id="spanNRTErrMsg" runat="server"></span>
                        <label class="after-label">
                            <asp:Literal ID="lblNRTParameterType" runat="server" Text='<%# setParamterType(Eval("ParameterDataType")) %>'></asp:Literal>
                        </label>

                        <div class="display-none">
                            <asp:TextBox ID="txtNRTParameterID" runat="server" ReadOnly="True" Text='<%# Eval("ParameterKey").ToString() %>' meta:resourcekey="txtParameterIDResource1"></asp:TextBox>
                        </div>
                    </div>
                    <div id="uxNRTMerchantFilter" runat="server" visible="False">
                        <div class="row">
                            <div class="control-inline">
                                <label>
                                    &nbsp&nbsp&nbsp<as:Literal ID="ltNRTFrom" runat="server" Text="From" meta:resourcekey="ltFromResource1"></as:Literal></label>
                                <asp:TextBox ID="txtNRTFrom" runat="server" Width="40px" MaxLength="16" onpaste="return false" onkeypress="return txtParameterValue_KeyPress(event, true);"
                                    CssClass="form-control" Text='<%# GetParameterNumber(Eval("ReAlertParameterIndicator").ToString()) %>' meta:resourcekey="txtFromResource1"></asp:TextBox>
                            </div>
                            <div class="control-inline">
                                <label>
                                    <as:Literal ID="LiteralNRT1" runat="server" Text="To" meta:resourcekey="Literal1Resource1"></as:Literal></label>

                                <asp:TextBox ID="txtNRTTo" runat="server" Width="40px" MaxLength="16" onpaste="return false" onkeypress="return txtParameterValue_KeyPress(event, true);"
                                    CssClass="form-control" Text='<%# GetParameterNumber(Eval("ReAlertParameterIndicatorHigh").ToString()) %>' meta:resourcekey="txtToResource1"></asp:TextBox>
                                <label>
                                    <as:Literal ID="LiteralNRT2" runat="server" Text="day(s)" meta:resourcekey="Literal2Resource1"></as:Literal>
                                </label>
                            </div>

                        </div>
                    </div>
                    <div id="msgError" class="error text-space text-left"></div>
                </ItemTemplate>

                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                <ItemStyle VerticalAlign="Top" CssClass="text-center text-nowrap"></ItemStyle>
            </as:ASGridTemplateColumn>
            <as:ASGridTemplateColumn HeaderText="Threshold" SortExpression="ReAlertParameterThreshold"
                HeaderStyle-Width="250px" HeaderStyle-HorizontalAlign="Center"
                UniqueName="ReAlertParameterThreshold" HeaderTooltip="Threshold" ItemStyle-HorizontalAlign="Center"
                ItemStyle-CssClass=" text-nowrap" meta:resourcekey="ASGridTemplateColumnResource3">
                <ItemTemplate>
                    <div id="dvThresholdNRT" runat="server" class="text-left">
                        <as:PlaceHolder ID="lblNRTNA" runat="server" Visible="False">
                            <div>
                                <b>
                                    <as:Literal ID="LiteralNRT22" runat="server" Text="" meta:resourcekey="Literal2Resource2_"></as:Literal></b>
                            </div>
                        </as:PlaceHolder>
                        <as:PlaceHolder ID="uxNRTThresholdNormal" runat="server">
                            <label class="no-margin">
                                <asp:Label ID="lblNRTDollar" runat="server" Text="&nbsp;$" meta:resourcekey="lblDollarResource1"></asp:Label>
                            </label>
                            <div class="form-inline control-inline last">
                                <asp:TextBox ID="txtNRTThreshold" runat="server" onpaste="return false" Text='<%# GetParameterNumber(Eval("ReAlertParameterThreshold").ToString()) %>'
                                    CssClass="form-control" Width="100px"
                                    onkeypress="return txtThreshold_KeyPress(event, true);" meta:resourcekey="txtThresholdResource1"></asp:TextBox>
                            </div>
                            <span id="spanNRTErrMsgThs" runat="server"></span>
                            <label class="after-label">
                                <asp:Label ID="lblNRTpercent" runat="server" Text='<%# setParamterType(Eval("ThresholdType")) %>' meta:resourcekey="lblpercentResource1"></asp:Label>
                            </label>

                        </as:PlaceHolder>
                        <as:PlaceHolder ID="uxNRTThresholdLowHigh" runat="server" Visible="False">

                            <div class="row">
                                <label class="before-label">
                                    <as:Literal ID="LiteralNRT3" runat="server" Text="Low" meta:resourcekey="Literal3Resource1"></as:Literal>&nbsp<asp:Label ID="lblNRTDollarThresholdLow" runat="server" Text="&nbsp;" meta:resourcekey="lblDollarThresholdLowResource1"></asp:Label>
                                </label>

                                <asp:TextBox ID="txtNRTThresholdLow" runat="server" Text='<%# GetParameterNumber(Eval("ReAlertParameterThreshold").ToString()) %>'
                                    Width="100px"
                                    onpaste="return false"
                                    onkeypress="return txtThreshold_KeyPress(event, true);"
                                    CssClass="form-control" meta:resourcekey="txtThresholdLowResource1"></asp:TextBox>
                                <span id="spanNRTErrMsgThresholdLow" runat="server"></span>
                                <label class="after-label">
                                    <asp:Label ID="lblNRTPercentThresholdLow" runat="server" Text='<%# setParamterType(Eval("ThresholdType")) %>' meta:resourcekey="lblPercentThresholdLowResource1"></asp:Label>
                                </label>

                            </div>
                            <div class="height-4">
                            </div>
                            <div class="row">
                                <label class="before-label">
                                    <as:Literal ID="ReLiteral4" runat="server" Text="High" meta:resourcekey="Literal4Resource1"></as:Literal>&nbsp<asp:Label ID="lblNRTDollarThresholdHigh" runat="server" Text="&nbsp;" meta:resourcekey="lblDollarThresholdHighResource1"></asp:Label>
                                </label>

                                <asp:TextBox ID="txtNRTThresholdHigh" runat="server" Text='<%# GetParameterNumber(Eval("ReAlertParameterThresholdHigh").ToString()) %>'
                                    Width="100px"
                                    onpaste="return false"
                                    onkeypress="return txtThreshold_KeyPress(event, true);"
                                    CssClass="form-control" meta:resourcekey="txtThresholdHighResource1"></asp:TextBox>
                                <span id="spanNRTErrMsgThresholdHigh" runat="server"></span>
                                <label class="after-label">
                                    <asp:Label ID="lblNRTPercentThresholdHigh" runat="server" Text='<%# setParamterType(Eval("ThresholdType")) %>' meta:resourcekey="lblPercentThresholdHighResource1"></asp:Label>
                                </label>

                            </div>
                        </as:PlaceHolder>
                    </div>
                    <div id="msgError" class="error text-space text-left"></div>
                </ItemTemplate>

                <HeaderStyle HorizontalAlign="Center" Width="250px"></HeaderStyle>

                <ItemStyle VerticalAlign="Top" HorizontalAlign="Center" CssClass="text-center text-nowrap"></ItemStyle>
            </as:ASGridTemplateColumn>

            <as:ASGridBoundColumn DataField="UsedByAssignments" UniqueName="UsedByAssignments"
                Display="false" EmptyDataText="" meta:resourcekey="ASGridBoundColumnResource14">

                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
            </as:ASGridBoundColumn>
            <as:ASGridBoundColumn DataField="UsedByRiskScores" UniqueName="UsedByRiskScores"
                Display="false"
                EmptyDataText="" meta:resourcekey="ASGridBoundColumnResource15">

                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
            </as:ASGridBoundColumn>
            <as:ASGridBoundColumn DataField="ParameterValue" UniqueName="ParameterValueHide"
                Display="false" meta:resourcekey="ASGridBoundColumnResource16">

                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
            </as:ASGridBoundColumn>

            <as:ASGridBoundColumn DataField="ReAlertParameterIndicator" UniqueName="ParameterValueHideNRT"
                Display="false" meta:resourcekey="ASGridBoundColumnResource16">

                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
            </as:ASGridBoundColumn>
            <as:ASGridBoundColumn DataField="ParameterThreshold" UniqueName="ParameterThresholdHide"
                Display="false" EmptyDataText="" meta:resourcekey="ASGridBoundColumnResource17">

                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
            </as:ASGridBoundColumn>

            <as:ASGridBoundColumn DataField="ReAlertParameterThreshold" UniqueName="ParameterThresholdHideNRT"
                Display="false" EmptyDataText="" meta:resourcekey="ASGridBoundColumnResource17">

                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
            </as:ASGridBoundColumn>
            <as:ASGridBoundColumn DataField="ParameterDataType" UniqueName="ParameterDataTypeHide"
                Display="false" meta:resourcekey="ASGridBoundColumnResource18">

                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
            </as:ASGridBoundColumn>
            <as:ASGridBoundColumn DataField="ParameterName" UniqueName="ParameterNameHide" Display="false" meta:resourcekey="ASGridBoundColumnResource19">

                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
            </as:ASGridBoundColumn>
            <as:ASGridBoundColumn DataField="ParameterThresholdType" UniqueName="ParameterThresholdType"
                Display="false" meta:resourcekey="ASGridBoundColumnResource20">

                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
            </as:ASGridBoundColumn>
            <as:ASGridBoundColumn DataField="ThresholdMin" UniqueName="ThresholdMinValue" Display="false" meta:resourcekey="ASGridBoundColumnResource21">

                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
            </as:ASGridBoundColumn>
            <as:ASGridBoundColumn DataField="ThresholdMax" UniqueName="ThresholdMaxValue" Display="false" meta:resourcekey="ASGridBoundColumnResource22">

                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
            </as:ASGridBoundColumn>
            <as:ASGridBoundColumn DataField="IndicatorMin" UniqueName="IndicatorMin" Display="false" meta:resourcekey="ASGridBoundColumnResource23">

                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
            </as:ASGridBoundColumn>
            <as:ASGridBoundColumn DataField="IndicatorMax" UniqueName="IndicatorMax" Display="false" meta:resourcekey="ASGridBoundColumnResource24">

                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
            </as:ASGridBoundColumn>

            <as:ASGridBoundColumn DataField="IsEditReAlertParameterIndicator" UniqueName="IsEditReAlertParameterIndicator" Display="false" meta:resourcekey="IsEditIndicatorResource1">

                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
            </as:ASGridBoundColumn>
            <as:ASGridBoundColumn DataField="IsEditReAlertParameterThreshold" UniqueName="IsEditReAlertParameterThreshold" Display="false" meta:resourcekey="IsEditThresholdResource1">

                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
            </as:ASGridBoundColumn>

            <as:ASGridBoundColumn DataField="ReAlertParameterThresholdMin" UniqueName="ReAlertParameterThresholdMin" Display="false" meta:resourcekey="ASGridBoundColumnResource21">

                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
            </as:ASGridBoundColumn>
            <as:ASGridBoundColumn DataField="ReAlertParameterThresholdMax" UniqueName="ReAlertParameterThresholdMax" Display="false" meta:resourcekey="ASGridBoundColumnResource22">

                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
            </as:ASGridBoundColumn>
            <as:ASGridBoundColumn DataField="ReAlertParameterIndicatorMin" UniqueName="ReAlertParameterIndicatorMin" Display="false" meta:resourcekey="ASGridBoundColumnResource23">

                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
            </as:ASGridBoundColumn>
            <as:ASGridBoundColumn DataField="ReAlertParameterIndicatorMax" UniqueName="ReAlertParameterIndicatorMax" Display="false" meta:resourcekey="ASGridBoundColumnResource24">

                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
            </as:ASGridBoundColumn>

<%--            <as:ASGridBoundColumn DataField="ParameterDescription" UniqueName="ParameterDescription"
                Display="false" meta:resourcekey="ASGridBoundColumnResource17">

                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                 <ItemStyle HorizontalAlign="Left"></ItemStyle>
            </as:ASGridBoundColumn>--%>
            <%-- <as:ASGridBoundColumn DataField="IsSelected" UniqueName="IsSelected" Display="false"
                EmptyDataText="">
            </as:ASGridBoundColumn>--%>
            <as:ASGridBoundColumn DataField="IsActive" UniqueName="IsActive" Display="false"
                EmptyDataText="" meta:resourcekey="ASGridBoundColumnResource26">

                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
            </as:ASGridBoundColumn>
            <as:ASGridBoundColumn DataField="IndicatorNegative" UniqueName="IndicatorNegative"
                Display="false" EmptyDataText="" meta:resourcekey="ASGridBoundColumnResource27">

                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
            </as:ASGridBoundColumn>
            <as:ASGridBoundColumn DataField="ThresholdNegative" UniqueName="ThresholdNegative"
                Display="false" EmptyDataText="" meta:resourcekey="ASGridBoundColumnResource28">

                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
            </as:ASGridBoundColumn>
        </Columns>

        <GroupHeaderItemStyle Width="0px"></GroupHeaderItemStyle>

    </MasterTableView>
</as:ASGrid>
<div class="display-none">
    <as:Button ID="btSave" runat="server" OnClick="btSave_Click" IsStandardButton="False" meta:resourcekey="btSaveResource1" />
</div>
<as:HiddenField ID="hfSelectedKeys" runat="server" />

<as:ASRadCodeBlock ID="radCodeBlock" runat="server">
    <script type="text/javascript">
        var Risk_Parameter_btSave = '<%=btSave.ClientID %>';
        var Risk_Parameter_js_ParameterMustBeSelected = '<%= GetLocalResourceObject("Risk_Parameter_js_ParameterMustBeSelected").ToString() %>';
    </script>
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
        var RiskParameter_js_Indicator_Min = '<%= Resources.MessageManager.RiskParameter_js_Indicator_Min%>';
        var RiskParameter_js_Indicator_ReAlert01 = "<%= Resources.MessageManager.RiskParameter_js_Indicator_ReAlert01%>";
        var RiskParameter_js_Indicator_ReAlert02 = "<%=Resources.MessageManager.RiskParameter_js_Indicator_ReAlert02%>";
        var RiskParameter_js_Threshold_ReAlert01 = "<%= Resources.MessageManager.RiskParameter_js_Threshold_ReAlert01%>";
        var RiskParameter_js_Threshold_ReAlert02 = "<%=Resources.MessageManager.RiskParameter_js_Threshold_ReAlert02%>";
        var RiskParameter_js_Threshold_ReAlertCM = "<%= Resources.MessageManager.RiskParameter_js_Threshold_ReAlertCM%>";

        var Param_js_Alert = '<%= GetLocalResourceObject("Param_Group_Alert").ToString()%>';
        var Param_js_ReAlert = '<%= GetLocalResourceObject("Param_Group_ReAlert").ToString()%>';
        var Param_js_Indicator = '<%= GetLocalResourceObject("ASGridTemplateColumnResource2.HeaderText").ToString()%>';
        var Param_js_Threshold = '<%= GetLocalResourceObject("ASGridTemplateColumnResource3.HeaderText").ToString()%>';
        var uxParameterListID = '<%=uxParameterList.ClientID %>';


    </script>
    <script src="<%= ResolveUrl("~/")%>res/js/risk_MCF/rm_MCF_Parameter.js"></script>
    <script src="<%= ResolveUrl("~/")%>res/js/common/riskparameter.js"></script>
</as:ASRadCodeBlock>


