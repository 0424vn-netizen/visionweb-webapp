<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Risk_Parameter_ReadOnly.ascx.cs"
    Inherits="UserControls_Risk_Parameter_ReadOnly" %>

<%@ Register TagName="UxExport" Src="~/UserControls/UxExport.ascx" TagPrefix="uc" %>

<as:RadAjaxManagerProxy ID="ram" runat="server">
    <AjaxSettings>
        <tek:AjaxSetting AjaxControlID="uxParameterList">
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="uxParameterList" />
            </UpdatedControls>
        </tek:AjaxSetting>
    </AjaxSettings>
</as:RadAjaxManagerProxy>

<as:ASGrid ID="uxParameterList" runat="server" AutoGenerateColumns="false" CssClass="ASTable"  XOverFlowable="false"
    AllowPaging="false" PageSize="30" ShowFooter="false" OnPreRender="ProcessGridGroupSplitterColumn"
    AllowSorting="true" AllowMultiRowEdit="true" OnNeedDataSource="uxParameterList_NeedDataSource"
    ShowHeader="false"
    OnItemDataBound="uxParameterList_ItemDataBound" GridName="Risk Management - Assignments - Parameters" meta:resourcekey="uxParameterListResource1">
    <MasterTableView NoMasterRecordsText="No data found.">
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
        <Columns>
            <as:ASGridBoundColumn DataField="ParameterGroupNameDummy" UniqueName="GroupNameDummy"
                Visible="false" meta:resourcekey="ASGridBoundColumnResource1">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
            </as:ASGridBoundColumn>
            <as:ASGridBoundColumn DataField="ParameterGroupName" UniqueName="GroupName" Visible="false" meta:resourcekey="ASGridBoundColumnResource2">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
            </as:ASGridBoundColumn>
            <as:ASGridBoundColumn DataField="ParameterGroupColor" UniqueName="ParameterColor"
                SortExpression="ParameterCode" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"
                HeaderTooltip="Code" HeaderText=" " meta:resourcekey="ASGridBoundColumnResource3">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>

                <ItemStyle HorizontalAlign="Left"></ItemStyle>
            </as:ASGridBoundColumn>
            <as:ASGridBoundColumn HeaderText="" DataField="ParameterName" UniqueName="ParameterName"
                HeaderTooltip="Parameter" ASFormat="DynamicString" HeaderStyle-Width="95%"
                SortExpression="ParameterName" meta:resourcekey="ASGridBoundColumnResource4">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center" Width="95%"></HeaderStyle>
            </as:ASGridBoundColumn>
            <as:ASGridBoundColumn HeaderText="% / # / $" SortExpression="ParameterValue" UniqueName="ParameterValue"
                DataField="ParameterValue" HeaderTooltip="%/#/$" meta:resourcekey="ASGridBoundColumnResource5">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
            </as:ASGridBoundColumn>
            <as:ASGridBoundColumn HeaderText="Threshold" SortExpression="ParameterThreshold"
                DataField="ParameterThreshold"
                ItemStyle-Width="15%" HeaderStyle-Width="15%" HeaderStyle-HorizontalAlign="Center"
                UniqueName="ParameterThreshold" HeaderTooltip="Threshold" ItemStyle-HorizontalAlign="Center" meta:resourcekey="ASGridBoundColumnResource6">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center" Width="15%"></HeaderStyle>

                <ItemStyle HorizontalAlign="Center" Width="15%"></ItemStyle>
            </as:ASGridBoundColumn>
            <as:ASGridBoundColumn DataField="ParameterDataType" UniqueName="ParameterDataType"
                Visible="false" EmptyDataText="" meta:resourcekey="ASGridBoundColumnResource7">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
            </as:ASGridBoundColumn>
            <as:ASGridBoundColumn DataField="ParameterDescription" UniqueName="ParameterDescription"
                Visible="false" EmptyDataText="" meta:resourcekey="ASGridBoundColumnResource8">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
            </as:ASGridBoundColumn>
        </Columns>
    </MasterTableView>
</as:ASGrid>