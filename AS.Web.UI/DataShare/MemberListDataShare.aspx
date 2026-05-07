<%@ Page Language="C#" MasterPageFile="~/MasterPagePopup.master" AutoEventWireup="true" CodeFile="MemberListDataShare.aspx.cs" Inherits="MemberListDataShare" Title="MEMBER LIST" Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" %>

<%@ Register TagName="UxExport" Src="~/UserControls/UxExport.ascx" TagPrefix="uc" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="Server">
    <as:ASModalContainer ID="uxModalContainer" runat="server" Width="900px">
        <uc:UxExport ID="uxExporter" IsOnTop="true" runat="server" GridID="uxReportGrid" 
            GridHeader="Member List" GridTitle="Member List" meta:resourcekey="uxExporterResource1" />

        <div class="row" runat="server" id="phdManageUser">
            <div class="col-xs-12 text-left" runat="server">
                <a runat="server" id="uxManageGroupUserList" href="#" class="link-back">
                    <as:Literal ID="ltDocument" runat="server" Text="Manage User List" meta:resourcekey="ltDocumentResource1"></as:Literal></a>
            </div>
        </div>

        <as:ASGrid ID="uxReportGrid" runat="server" GridLines="None" Width="100%" AllowPaging="True" IsAutoExportTemplate="true"
            AllowSorting="True" AutoGenerateColumns="false" AllowSortFilterWhenExport="true"
            ASPagingMethod="SPASingleMethod1" ShowFooter="false" meta:resourcekey="uxReportGridResource1" OnNeedDataSource="uxReportGrid_NeedDataSource">
            <MasterTableView NoMasterRecordsText="Data not found" meta:resourcekey="MasterTableViewResource1">
                <Columns>
                    <as:ASGridBoundColumn HeaderText="User ID" DataField="UserID" UniqueName="UserID"
                        SortExpression="UserID" ASFormat="StaticString" meta:resourcekey="UserLoginNameResource1">
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="User Full Name" DataField="UserName" UniqueName="UserName"
                        SortExpression="UserName" ASFormat="StaticString" HeaderTooltip="User Full Name"
                        AllowEncodeOnExporting="true" meta:resourcekey="UserNameFullResource1" />
                </Columns>
            </MasterTableView>
        </as:ASGrid>
        <div runat="server" id="pnlbuttonok" class="row">
            <div class="col-xs-12 form-action-container text-right">
                <as:Button ID="btnCancel" runat="server" OnClientClick="DoClose();" Text="Back" class="btn btn-default" IsStandardButton="False" meta:resourcekey="btnCancelResource1" />
            </div>
        </div>
        <asp:Button ID="uxRefreshGrid" runat="server" OnClick="uxRefreshGrid_Click" Style="display: none;" meta:resourcekey="uxRefreshGridResource1" />
    </as:ASModalContainer>
    <tek:RadCodeBlock runat="server" ID="RadCodeBlock">
        <script type="text/javascript">
            var Membership_uxRefreshGrid = '<%= uxRefreshGrid.ClientID %>';
        </script>
        <script type="text/javascript" src="<%= ResolveUrl("~/") %>res/js/datashare/memberList.js"></script>
    </tek:RadCodeBlock>
</asp:Content>

