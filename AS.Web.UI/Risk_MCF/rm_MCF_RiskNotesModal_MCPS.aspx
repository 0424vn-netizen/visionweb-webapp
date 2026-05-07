<%@ Page Language="C#" MasterPageFile="~/MasterPagePopup.master" AutoEventWireup="true" CodeFile="rm_MCF_RiskNotesModal_MCPS.aspx.cs" Inherits="rm_MCF_RiskNotesModal_MCPS" Title="MCPS RISK NOTES" meta:resourcekey="PageResource1" %>

<%@ Register TagName="UxExport" Src="~/UserControls/UxExport.ascx" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/PageTitle.ascx" TagName="ReportTitle" TagPrefix="uc" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="Server">
    <div>
        <tek:RadAjaxManagerProxy runat="server" ID="RadAjaxManager">
            <AjaxSettings>
                <tek:AjaxSetting AjaxControlID="uxReportGrid">
                    <UpdatedControls>
                        <tek:AjaxUpdatedControl ControlID="uxReportGrid" />
                    </UpdatedControls>
                </tek:AjaxSetting>
            </AjaxSettings>
        </tek:RadAjaxManagerProxy>
       
            <div class="row">
                <div class="col-md-12">
                    <h3 class="modal-title">
                        <asp:Literal ID="LiteralMCPSRiskNote" runat="server" meta:resourcekey="LiteralMCPSRiskNoteResource1"> MCPS Risk Notes</asp:Literal></h3>
                </div>
            </div>
            <uc:UxExport ID="uxExporter" runat="server" GridID="uxReportGrid" IsOnTop="true"
                GridHeader="MCPS Risk Notes" FileName="MCPS Risk Notes - Merchant ID"
                ShowWord="false" ShowPDF="true" />
            <as:ASGrid ID="uxReportGrid" runat="server" ShowPageTotal="false" AllowPaging="true" IsAutoExportTemplate="true"
                AutoGenerateColumns="False" ShowHeader="true" ASPagingMethod="SPASingleMethod"
                GridLines="None" GridName="MCPS Risk Notes - Merchant ID" CssClass="in" meta:resourcekey="uxReportGridResource1">
                <MasterTableView>
                    <Columns>
                        <as:ASGridBoundColumn HeaderText="Date/Time" DataField="DateTime" UniqueName="DateTime"
                            HeaderTooltip="" SortExpression="DateTime" ASFormat="DateAndTime12Hours" meta:resourcekey="ASGridBoundColumnResource1">

                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        </as:ASGridBoundColumn>
                        <as:ASGridBoundColumn HeaderText="Owner Group" DataField="OwnerGroup" UniqueName="OwnerGroup"
                            HeaderTooltip="" SortExpression="OwnerGroup" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource2">

                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        </as:ASGridBoundColumn>
                        <as:ASGridBoundColumn HeaderText="Comments" DataField="Comments" UniqueName="Comments"
                            SortExpression="Comments" ASFormat="DynamicString" meta:resourcekey="ASGridBoundColumnResource3">

                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        </as:ASGridBoundColumn>
                    </Columns>
                </MasterTableView>
            </as:ASGrid>      
    </div>

    <tek:RadCodeBlock ID="uxRadCodeBlock" runat="server">
        <script type="text/javascript">
            var rm_AddRiskCommentModal_IsIEBrowser = "<%=IsIEBrowser.ToString()%>";
        </script>
        <script type="text/javascript" src="<%=ResolveUrl("~") %>res/js/risk_MCF/rm_MCF_RiskNotesModal_MCPS.js"></script>
    </tek:RadCodeBlock>
</asp:Content>

