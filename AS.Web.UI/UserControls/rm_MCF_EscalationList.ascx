<%@ Control Language="C#" AutoEventWireup="true" CodeFile="rm_MCF_EscalationList.ascx.cs"
    Inherits="UserControls_rm_MCF_EscalationList" %>
<%@ Register TagName="UxExport" Src="UxExport.ascx" TagPrefix="uc" %>

<tek:RadAjaxManagerProxy runat="server" ID="uxRadAjaxManagerProxy">
    <AjaxSettings>
        <tek:AjaxSetting AjaxControlID="uxEscalationGrid">
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="uxEscalationGrid" LoadingPanelID="LoadingPannel" />
            </UpdatedControls>
        </tek:AjaxSetting>
    </AjaxSettings>
</tek:RadAjaxManagerProxy>
<tek:RadAjaxLoadingPanel ID="LoadingPannel" BackgroundPosition="Top" runat="server">
</tek:RadAjaxLoadingPanel>
<uc:UxExport ID="uxExporter" runat="server" GridID="uxEscalationGrid" ShowPDF="true"
    ShowWord="false" OnNeedExportConfig="uxExport_NeedExportConfig" />
<as:ASGrid ID="uxEscalationGrid"
    runat="server" AutoGenerateColumns="False" AllowSorting="true" AllowPaging="false"
    GridLines="None" NoMasterRecordsText="Data not found." IntruderSourceName="uxEscalationGrid"
    IsIntruder="true"
    OnNeedDataSource="uxGrid_NeedDataSource"
    OnItemDataBound="uxGrid_ItemDataBound" CssClass="in" meta:resourcekey="uxEscalationGridResource1">
    <MasterTableView Width="100%">
        <Columns>
            <as:ASGridBoundColumn HeaderText="Tkt"
                DataField="EscalationID"
                UniqueName="EscalationID"
                HeaderTooltip="Ticket"
                ASFormat="StaticString"
                SortExpression="EscalationID" meta:resourcekey="ASGridBoundColumnResource1">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
            </as:ASGridBoundColumn>
            <as:ASGridBoundColumn HeaderText="Merchant Number"
                DataField="MerchantNumber"
                UniqueName="MerchantNumber"
                Visible="false" meta:resourcekey="ASGridBoundColumnResource2">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
            </as:ASGridBoundColumn>
            <as:ASGridBoundColumn HeaderText="Merchant Name"
                DataField="MerchantName"
                UniqueName="MerchantName"
                HeaderTooltip="Merchant Name"
                ASFormat="DynamicString"
                SortExpression="MerchantName" meta:resourcekey="ASGridBoundColumnResource3">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
            </as:ASGridBoundColumn>
            <as:ASGridBoundColumn HeaderText="Profile"
                DataField="Profile"
                UniqueName="Profile"
                HeaderTooltip="Profile"
                ASFormat="StaticString"
                SortExpression="Profile" meta:resourcekey="ASGridBoundColumnResource4">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
            </as:ASGridBoundColumn>
            <as:ASGridBoundColumn HeaderText="Current Status"
                DataField="Status"
                UniqueName="Status"
                HeaderTooltip="Current Status"
                ASFormat="StaticString"
                SortExpression="Status" meta:resourcekey="ASGridBoundColumnResource5">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
            </as:ASGridBoundColumn>
            <as:ASGridBoundColumn HeaderText="Resolution"
                DataField="Resolution"
                UniqueName="Resolution"
                HeaderTooltip="Resolution"
                ASFormat="StaticString"
                SortExpression="Resolution" meta:resourcekey="ASGridBoundColumnResource6">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
            </as:ASGridBoundColumn>
            <as:ASGridBoundColumn HeaderText="Assigned To"
                DataField="AssignedTo"
                UniqueName="AssignedTo"
                HeaderTooltip="Assigned To"
                ASFormat="StaticString"
                SortExpression="AssignedTo" meta:resourcekey="ASGridBoundColumnResource7">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
            </as:ASGridBoundColumn>
            <as:ASGridBoundColumn HeaderText="Open Date"
                DataField="EscalationDate"
                UniqueName="EscalationDate"
                HeaderTooltip="Open Date"
                SortExpression="EscalationDate"
                ASFormat="DateAndTime12Hours" meta:resourcekey="ASGridBoundColumnResource8">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
            </as:ASGridBoundColumn>
            <as:ASGridBoundColumn HeaderText="Last Update"
                DataField="LastUpdated"
                UniqueName="LastUpdated"
                HeaderTooltip="Last Update"
                SortExpression="LastUpdated"
                ASFormat="DateAndTime12Hours" meta:resourcekey="ASGridBoundColumnResource9">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text=""></ModelErrorMessage>
                </ColumnValidationSettings>

                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
            </as:ASGridBoundColumn>
        </Columns>
    </MasterTableView>
</as:ASGrid>
<uc:UxExport ID="uxExporterBottom" runat="server" GridID="uxEscalationGrid" IsBottom="true"
    ShowPDF="true" ShowWord="false" OnNeedExportConfig="uxExport_NeedExportConfig" />
