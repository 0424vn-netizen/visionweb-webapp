<%@ Page Title="History" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="rm_MCF_EscalationHistory.aspx.cs" Inherits="rm_MCF_EscalationHistory" meta:resourcekey="PageResource1" %>

<%@ Register Src="~/UserControls/PageTitle.ascx" TagName="ReportTitle" TagPrefix="uc" %>
<%@ Register TagName="UxExport" Src="~/UserControls/UxExport.ascx" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPage" runat="Server">
    <as:PlaceHolder ID="dfh" runat="server">
        <div class="row row-table">
            <div class="col-md-12">
                <uc:PageTitle ID="uxReportTitle" runat="server" ReportTitle="Escalation Queue" HasFilteringOption="false" meta:resourcekey="uxReportTitleResource1" />
            </div>
        </div>
        <div class="row">
            <div class="col-xs-6 text-left" data-toggle="collapse" data-target="#info">
                <h2 class="grid-title on-top">
                    <asp:Literal ID="rm_EscalationHistory_aspx_Text1" runat="server" meta:resourcekey="rm_EscalationHistory_aspx_Text1Resource1" Text="Escalation Queue Status"></asp:Literal></h2>
            </div>
            <div class="col-xs-6 text-right">
                <asp:HyperLink ID="uxGoBack" runat="server" NavigateUrl="#" Visible="False"
                    CssClass="link-back" meta:resourcekey="uxGoBackResource1" />
            </div>
        </div>
        <div class="row">
            <div class="col-md-8">
                <table id="info" class="ASTable in">
                    <colgroup>
                        <col style="width: 135px" />
                        <col />
                        <col style="width: 105px" />
                        <col />
                    </colgroup>
                    <tr class="Row">
                        <td class="heading">
                            <asp:Literal ID="rm_EscalationHistory_aspx_Text2" runat="server" meta:resourcekey="rm_EscalationHistory_aspx_Text2Resource1" Text="Escalation Number:"></asp:Literal></td>
                        <td>
                            <as:Literal ID="uxEscalationID" runat="server" meta:resourcekey="uxEscalationIDResource1" />
                        </td>
                        <td class="heading">
                            <asp:Literal ID="rm_EscalationHistory_aspx_Text3" runat="server" meta:resourcekey="rm_EscalationHistory_aspx_Text3Resource1" Text="Reason:"></asp:Literal></td>
                        <td>
                            <as:Literal ID="uxReason" runat="server" meta:resourcekey="uxReasonResource1" />
                        </td>
                    </tr>
                    <tr class="rgAltRow">
                        <td class="heading">
                            <asp:Literal ID="rm_EscalationHistory_aspx_Text4" runat="server" meta:resourcekey="rm_EscalationHistory_aspx_Text4Resource1" Text="Merchant ID:"></asp:Literal></td>
                        <td>
                            <as:Literal ID="uxMerchantNumber" runat="server" meta:resourcekey="uxMerchantNumberResource1" />
                        </td>
                        <td class="heading">
                            <asp:Literal ID="rm_EscalationHistory_aspx_Text5" runat="server" meta:resourcekey="rm_EscalationHistory_aspx_Text5Resource1" Text="Current Status:"></asp:Literal></td>
                        <td>
                            <asp:Label ID="uxStatus" runat="server" meta:resourcekey="uxStatusResource1" />
                        </td>
                    </tr>
                    <tr class="Row">
                        <td class="heading">
                            <asp:Literal ID="rm_EscalationHistory_aspx_Text6" runat="server" meta:resourcekey="rm_EscalationHistory_aspx_Text6Resource1" Text="Merchant Name:"></asp:Literal></td>
                        <td>
                            <as:Literal ID="uxMerchantName" runat="server" meta:resourcekey="uxMerchantNameResource1" />
                        </td>
                        <td class="heading">
                            <asp:Literal ID="rm_EscalationHistory_aspx_Text7" runat="server" meta:resourcekey="rm_EscalationHistory_aspx_Text7Resource1" Text="Closed:"></asp:Literal></td>
                        <td>
                            <as:Literal ID="uxClosed" runat="server" meta:resourcekey="uxClosedResource1" />
                        </td>
                    </tr>
                    <tr class="rgAltRow">
                        <td class="heading">
                            <asp:Literal ID="rm_EscalationHistory_aspx_Text8" runat="server" meta:resourcekey="rm_EscalationHistory_aspx_Text8Resource1" Text="Open Date:"></asp:Literal></td>
                        <td>
                            <as:Literal ID="uxEscalationDate" runat="server" meta:resourcekey="uxEscalationDateResource1" />
                        </td>
                        <td class="heading">
                            <asp:Literal ID="rm_EscalationHistory_aspx_Text9" runat="server" meta:resourcekey="rm_EscalationHistory_aspx_Text9Resource1" Text="Closed Date:"></asp:Literal></td>
                        <td>
                            <as:Literal ID="uxClosedDate" runat="server" meta:resourcekey="uxClosedDateResource1" />
                        </td>
                    </tr>
                    <tr class="Row">
                        <td class="heading">
                            <asp:Literal ID="rm_EscalationHistory_aspx_Text10" runat="server" meta:resourcekey="rm_EscalationHistory_aspx_Text10Resource1" Text="Assigned To:"></asp:Literal></td>
                        <td>
                            <as:Literal ID="uxAssignedTo" runat="server" meta:resourcekey="uxAssignedToResource1" />
                        </td>
                        <td class="heading">
                            <asp:Literal ID="rm_EscalationHistory_aspx_Text11" runat="server" meta:resourcekey="rm_EscalationHistory_aspx_Text11Resource1" Text="Closed By:"></asp:Literal></td>
                        <td>
                            <as:Literal ID="uxClosedBy" runat="server" meta:resourcekey="uxClosedByResource1" />
                        </td>
                    </tr>
                    <tr class="rgAltRow">
                        <td class="heading">
                            <asp:Literal ID="rm_EscalationHistory_aspx_Text12" runat="server" meta:resourcekey="rm_EscalationHistory_aspx_Text12Resource1" Text="Follow Up Date:"></asp:Literal></td>
                        <td>
                            <as:Literal ID="uxFollowupDate" runat="server" meta:resourcekey="uxFollowupDateResource1" />
                        </td>
                        <td></td>
                        <td></td>
                    </tr>
                </table>
            </div>
        </div>
        <uc:UxExport ID="uxExporter" runat="server" GridID="uxReportGrid" GridTitle="Escalation Queue History"
            GridHeader="Risk Management - Risk Analysis - Escalation Queue - History" meta:resourcekey="uxExporterResource1" />
        <as:ASGrid ID="uxReportGrid" runat="server" AutoGenerateColumns="false" AllowPaging="true" IsAutoExportTemplate="true"
            ASPagingMethod="SPASingleMethod" AllowSorting="true" IsIntruder="true" IntruderSourceName="uxReportGrid"
            AllowSortFilterWhenExport="true" CssClass="in" meta:resourcekey="uxReportGridResource1">
            <MasterTableView>
                <Columns>
                    <as:ASGridBoundColumn HeaderText="Change Date" DataField="ChangeDate" UniqueName="ChangeDate"
                        SortExpression="ChangeDate" HeaderTooltip="Change Date" ASFormat="DateAndTime12Hours"
                        HeaderStyle-Width="150px" meta:resourcekey="ASGridBoundColumnResource1">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center" Width="150px"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Changed By" DataField="ChangedBy" UniqueName="ChangedBy"
                        HeaderTooltip="Changed By" SortExpression="ChangedBy" ASFormat="DynamicString" meta:resourcekey="ASGridBoundColumnResource2">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Reason" DataField="Reason" UniqueName="Reason"
                        HeaderTooltip="Reason" SortExpression="Reason" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource3">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Assigned To" DataField="AssignedTo" UniqueName="AssignedTo"
                        HeaderTooltip="Assigned To" SortExpression="AssignedTo" ASFormat="DynamicString" meta:resourcekey="ASGridBoundColumnResource4">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Follow Up Date" DataField="FollowupDate" UniqueName="FollowupDate"
                        HeaderTooltip="Follow Up Date" SortExpression="FollowupDate" ASFormat="Date" meta:resourcekey="ASGridBoundColumnResource5">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Status" DataField="Status" UniqueName="Status"
                        HeaderTooltip="Status" SortExpression="Status" ASFormat="StaticString" HeaderStyle-Width="80px" meta:resourcekey="ASGridBoundColumnResource6">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center" Width="80px"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Activity" DataField="Activity" UniqueName="Activity"
                        HeaderTooltip="Activity" SortExpression="Activity" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource7">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Resolution" DataField="Resolution" UniqueName="Resolution"
                        HeaderTooltip="Resolution" SortExpression="Resolution" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource8">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Savings/Loss" DataField="SavingsLossAmt" UniqueName="SavingLoss"
                        HeaderStyle-HorizontalAlign="Center" ASFormat="Currency" HeaderTooltip="Savings/Loss"
                        SortExpression="SavingsLossAmt" ItemStyle-HorizontalAlign="Right" Visible="false" meta:resourcekey="SavingLossResource1">
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Comments" DataField="Comments" UniqueName="Comments"
                        HeaderTooltip="Comments" SortExpression="Comments" ASFormat="DynamicString" meta:resourcekey="ASGridBoundColumnResource9">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                </Columns>
            </MasterTableView>
        </as:ASGrid>

    </as:PlaceHolder>
    <tek:RadAjaxManagerProxy ID="RadAjaxManager1" runat="server">
        <AjaxSettings>
            <tek:AjaxSetting AjaxControlID="uxReportGrid">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="uxReportGrid" />
                </UpdatedControls>
            </tek:AjaxSetting>
        </AjaxSettings>
    </tek:RadAjaxManagerProxy>
</asp:Content>
