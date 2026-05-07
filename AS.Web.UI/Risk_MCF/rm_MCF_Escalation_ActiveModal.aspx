<%@ Page Title="OPEN ESCALATIONS" Language="C#" MasterPageFile="~/MasterPagePopup.master"
    AutoEventWireup="true" CodeFile="rm_MCF_Escalation_ActiveModal.aspx.cs" Inherits="rm_MCF_Escalation_ActiveModal" meta:resourcekey="PageResource1" %>

<%@ Register TagName="UxExport" Src="~/UserControls/UxExport.ascx" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="Server">
    <tek:RadAjaxManagerProxy runat="server" ID="uxRadAjaxManagerProxy">
        <AjaxSettings>
            <tek:AjaxSetting AjaxControlID="uxEscalationGrid">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="uxEscalationGrid" LoadingPanelID="LoadingPannel" />
                </UpdatedControls>
            </tek:AjaxSetting>
        </AjaxSettings>
    </tek:RadAjaxManagerProxy>
    <as:ASModalContainer ID="uxModalContainer" runat="server" Width="1000px" ContainerCssClass="container" WidthCssClass="modal-xxl">
        <p>
            <i>
                <asp:Literal ID="rm_Escalation_ActiveModal_aspx_Text1" runat="server" meta:resourcekey="rm_Escalation_ActiveModal_aspx_Text1Resource1"> Selected Escalation Status cannot be deactivated because the following escalations
                are still open.</asp:Literal></i>
        </p>
        <div class="row">
            <div class="col-md-12">
                <uc:UxExport ID="uxExporter" IsOnTop="true" runat="server" GridID="uxReportGrid" />
                <as:ASGrid ID="uxReportGrid" runat="server" AllowPaging="true" GridLines="None" AutoGenerateColumns="false"
                    ASPagingMethod="SPASingleMethod" IntruderSourceName="uxReportGrid" IsIntruder="true" CssClass="in" meta:resourcekey="uxReportGridResource1">
                    <MasterTableView>
                        <Columns>
                            <as:ASGridBoundColumn HeaderText="Tkt" DataField="EscalationID" UniqueName="EscalationID"
                                HeaderTooltip="Ticket" ASFormat="StaticString" SortExpression="EscalationID" meta:resourcekey="ASGridBoundColumnResource1">
                                <ColumnValidationSettings>
                                    <ModelErrorMessage Text=""></ModelErrorMessage>
                                </ColumnValidationSettings>

                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            </as:ASGridBoundColumn>
                            <as:ASGridBoundColumn HeaderText="Merchant ID" DataField="MerchantNumber" UniqueName="MerchantNumber"
                                Visible="false" meta:resourcekey="ASGridBoundColumnResource2">
                                <ColumnValidationSettings>
                                    <ModelErrorMessage Text=""></ModelErrorMessage>
                                </ColumnValidationSettings>

                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            </as:ASGridBoundColumn>
                            <as:ASGridBoundColumn HeaderText="Merchant Name" DataField="MerchantName" UniqueName="MerchantName"
                                HeaderTooltip="Merchant Name" ASFormat="DynamicString" SortExpression="MerchantName" meta:resourcekey="ASGridBoundColumnResource3">
                                <ColumnValidationSettings>
                                    <ModelErrorMessage Text=""></ModelErrorMessage>
                                </ColumnValidationSettings>

                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            </as:ASGridBoundColumn>
                            <as:ASGridBoundColumn HeaderText="Profile" DataField="Profile" UniqueName="Profile"
                                HeaderTooltip="Profile" ASFormat="StaticString" SortExpression="Profile" meta:resourcekey="ASGridBoundColumnResource4">
                                <ColumnValidationSettings>
                                    <ModelErrorMessage Text=""></ModelErrorMessage>
                                </ColumnValidationSettings>

                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            </as:ASGridBoundColumn>
                            <as:ASGridBoundColumn HeaderText="Current Status" DataField="Status" UniqueName="Status"
                                HeaderTooltip="Current Status" ASFormat="StaticString" SortExpression="Status" meta:resourcekey="ASGridBoundColumnResource5">
                                <ColumnValidationSettings>
                                    <ModelErrorMessage Text=""></ModelErrorMessage>
                                </ColumnValidationSettings>

                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            </as:ASGridBoundColumn>
                            <as:ASGridBoundColumn HeaderText="Resolution" DataField="Resolution" UniqueName="Resolution"
                                HeaderTooltip="Resolution" ASFormat="StaticString" SortExpression="Resolution" meta:resourcekey="ASGridBoundColumnResource6">
                                <ColumnValidationSettings>
                                    <ModelErrorMessage Text=""></ModelErrorMessage>
                                </ColumnValidationSettings>

                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            </as:ASGridBoundColumn>
                            <as:ASGridBoundColumn HeaderText="Assigned To" DataField="AssignedTo" UniqueName="AssignedTo"
                                HeaderTooltip="Assigned To" ASFormat="StaticString" SortExpression="AssignedTo" meta:resourcekey="ASGridBoundColumnResource7">
                                <ColumnValidationSettings>
                                    <ModelErrorMessage Text=""></ModelErrorMessage>
                                </ColumnValidationSettings>

                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            </as:ASGridBoundColumn>
                            <as:ASGridBoundColumn HeaderText="Open Date" DataField="EscalationDate" UniqueName="EscalationDate"
                                HeaderTooltip="Open Date" SortExpression="EscalationDate" ASFormat="DateAndTime12Hours"
                                HeaderStyle-Width="150px" meta:resourcekey="ASGridBoundColumnResource8">
                                <ColumnValidationSettings>
                                    <ModelErrorMessage Text=""></ModelErrorMessage>
                                </ColumnValidationSettings>

                                <HeaderStyle HorizontalAlign="Center" Width="150px"></HeaderStyle>
                            </as:ASGridBoundColumn>
                            <as:ASGridBoundColumn HeaderText="Last Update" DataField="LastUpdated" UniqueName="LastUpdated"
                                HeaderTooltip="Last Update" SortExpression="LastUpdated" ASFormat="DateAndTime12Hours"
                                HeaderStyle-Width="150px" meta:resourcekey="ASGridBoundColumnResource9">
                                <ColumnValidationSettings>
                                    <ModelErrorMessage Text=""></ModelErrorMessage>
                                </ColumnValidationSettings>

                                <HeaderStyle HorizontalAlign="Center" Width="150px"></HeaderStyle>
                            </as:ASGridBoundColumn>
                        </Columns>
                    </MasterTableView>
                </as:ASGrid>
            </div>
        </div>
    </as:ASModalContainer>
</asp:Content>
