<%@ Page Title="Temporary Reassign Users" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="rm_MCF_TempReassignUser.aspx.cs" Inherits="rm_MCF_TempReassignUser" EnableEventValidation="false" meta:resourcekey="PageResource1" %>

<%@ Register Src="~/UserControls/UxExport.ascx" TagPrefix="uc" TagName="UxExport" %>
<%@ Register Src="~/UserControls/PageTitle.ascx" TagName="PageTitle" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/RiskReassignUser.ascx" TagName="ReassignUser" TagPrefix="uc" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="Server">
    <div class="row">
        <div class="col-md-12">
            <uc:PageTitle ID="uxPageTitle" runat="server" HasFilteringOption="false" ReportTitle="Temporarily Reassign User" meta:resourcekey="uxPageTitleResource1" />
        </div>
    </div>
    <div class="row">
        <div class="col-md-12">
            <div class="box">
                <i>
                    <as:Literal ID="ltAssignment" runat="server" Text="Only those assignments that are assigned directly to this user will be reassigned. Those assignments that are assigned to the user's group will remain with the group." meta:resourcekey="ltAssignmentResource1"></as:Literal><br />
                    <as:Literal ID="ltPurpose" runat="server" Text="The purpose of this tool is to ensure that all assignments are worked in the case of a user's absence or vacation." meta:resourcekey="ltPurposeResource1"></as:Literal>
                </i>
            </div>
        </div>
    </div>
    <%--Create new Assignment--%>
    <div class="row">
        <div class="col-md-12 action-container">
            <as:LinkButton runat="server" ID="uxCreateMode" class="btn btn-default" OnClick="uxCreateMode_Click" meta:resourcekey="uxCreateModeResource1">Create New Reassignment</as:LinkButton>
        </div>
    </div>
    <div class="hide">
        <as:RadComboBox ID="RadComboBox1" runat="server" />
        <as:RadDatePicker ID="RadDatePicker1" runat="server"></as:RadDatePicker>
    </div>
    <div class="row">
        <div class="col-md-12" id="pnlReassignUser">
            <uc:ReassignUser ID="uxReassignUser" runat="server" OnAfterSubmit="uxReassignUser_AfterSubmit"
                OnAfterCancel="uxReassignUser_AfterCancel" Visible="False" ContainerCss="risk-form-container" />
        </div>
    </div>
    <div class="height-12"></div>
    <%--end Create new Assignment--%>

    <div class="row">
        <div class="col-md-12 dark-blue">
            <div class="control-inline">
                <label class="first">
                    <as:Literal ID="ltView" runat="server" Text="View:" meta:resourcekey="ltViewResource1"></as:Literal></label>
            </div>
            <div class="control-inline">
                <as:RadioButton ID="uxViewActiveOnly" runat="server" GroupName="ViewOpts" Checked="True"
                    Text="View Active" AutoPostBack="True" OnCheckedChanged="uxViewActiveOnly_CheckedChanged" meta:resourcekey="uxViewActiveOnlyResource1" Value="" />
            </div>
            <div class="control-inline">
                <as:RadioButton ID="uxViewAll" runat="server" GroupName="ViewOpts" Text="View All"
                    AutoPostBack="True" OnCheckedChanged="uxViewAll_CheckedChanged" meta:resourcekey="uxViewAllResource1" Value="" />
            </div>
        </div>
    </div>
    <uc:UxExport ID="uxExporter" IsOnTop="true" runat="server" GridID="uxReportGrid"
        ShowPDF="true"
        ShowWord="false" />
    <div class="row">
        <div class="col-md-12">
            <as:ASGrid ID="uxReportGrid" runat="server" AllowPaging="True" IsAutoExportTemplate="true"
                AllowSorting="True" AutoGenerateColumns="False" OnNeedDataSource="uxReportGrid_NeedDataSource"
                OnUpdateCommand="uxReportGrid_UpdateCommand" OnItemDataBound="uxReportGrid_ItemDataBound"
                OnPageIndexChanged="uxReportGrid_PageIndexChanged" CssClass="in" meta:resourcekey="uxReportGridResource1">
                <MasterTableView>
                    <Columns>
                        <tek:GridEditCommandColumn UniqueName="EditCommandColumn" HeaderTooltip="Click to edit reassignment"
                            HeaderText="Edit" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="45px"
                            ItemStyle-HorizontalAlign="Center" meta:resourcekey="GridEditCommandColumnResource1">
                            <HeaderStyle HorizontalAlign="Center" Width="45px"></HeaderStyle>

                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                        </tek:GridEditCommandColumn>
                        <as:ASGridBoundColumn UniqueName="UserName" HeaderText="User" DataField="UserName"
                            SortExpression="UserName" ASFormat="StaticString" HeaderTooltip="User Name"
                            AllowEncodeOnExporting="true" meta:resourcekey="ASGridBoundColumnResource1">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text=""></ModelErrorMessage>
                            </ColumnValidationSettings>

                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        </as:ASGridBoundColumn>
                        <as:ASGridBoundColumn UniqueName="ReassignedTo" HeaderText="Reassigned To" DataField="ReassignedTo"
                            AllowEncodeOnExporting="true" SortExpression="ReassignedTo" HeaderTooltip="Reassign to User Name"
                            ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource2">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text=""></ModelErrorMessage>
                            </ColumnValidationSettings>

                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        </as:ASGridBoundColumn>
                        <as:ASGridBoundColumn UniqueName="FromDate" HeaderText="From" DataField="FromDate"
                            ASFormat="Date" HeaderTooltip="Start date" SortExpression="FromDate" HeaderStyle-Width="100px" meta:resourcekey="ASGridBoundColumnResource3">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text=""></ModelErrorMessage>
                            </ColumnValidationSettings>

                            <HeaderStyle HorizontalAlign="Center" Width="100px"></HeaderStyle>
                        </as:ASGridBoundColumn>
                        <as:ASGridBoundColumn UniqueName="ToDate" HeaderText="To" DataField="ToDate"
                            ASFormat="Date" SortExpression="ToDate" HeaderTooltip="End date" HeaderStyle-Width="100px" meta:resourcekey="ASGridBoundColumnResource4">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text=""></ModelErrorMessage>
                            </ColumnValidationSettings>

                            <HeaderStyle HorizontalAlign="Center" Width="100px"></HeaderStyle>
                        </as:ASGridBoundColumn>
                        <as:ASGridTemplateColumn HeaderText="Delete" DataField="AssignmentID" HeaderStyle-Width="60px"
                            UniqueName="Delete" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                            HeaderTooltip="Click to delete reassignment" meta:resourcekey="ASGridTemplateColumnResource1">
                            <ItemTemplate>
                                <as:LinkButton ID="uxDelete" runat="server" Text="Delete"
                                    OnCommand="DeleteReassignment_Command" CommandArgument='<%# Eval("ReassignmentID") %>'
                                    OnClientClick='return DeleteReAssignment();' meta:resourcekey="uxDeleteResource1"></as:LinkButton>
                            </ItemTemplate>

                            <HeaderStyle HorizontalAlign="Center" Width="60px"></HeaderStyle>

                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                        </as:ASGridTemplateColumn>
                    </Columns>
                    <EditFormSettings EditFormType="Template">
                        <EditColumn UniqueName="EditCommandColumn1" FilterControlAltText="Filter EditCommandColumn1 column"></EditColumn>
                        <FormTemplate>
                            <uc:ReassignUser ID="uxEditReassignUser" runat="server" UpdateMode="true" />
                        </FormTemplate>
                    </EditFormSettings>
                    <ExpandCollapseColumn ButtonType="ImageButton" Visible="False" UniqueName="ExpandColumn">
                        <HeaderStyle Width="19px"></HeaderStyle>
                    </ExpandCollapseColumn>
                </MasterTableView>
            </as:ASGrid>
        </div>
    </div>
    <as:Literal ID="uxMsg" runat="server" meta:resourcekey="uxMsgResource1"></as:Literal>
    <as:HiddenField ID="hddReAssignmentID" runat="Server" />
    <tek:RadCodeBlock runat="server" ID="RadCodeBlock2">
        <script type="text/javascript">
            var rm_TempReassignUser_ReassignUser_ReassignToEqualUserName = '<%=Resources.UserMaintenanceMessage.ReassignUser_ReassignToEqualUserName %>';
            var rm_TempReassignUser_ReassignUser_ConfirmAssign = '<%=Resources.UserMaintenanceMessage.ReassignUser_ConfirmAssign %>';
            var rm_TempResassignUser_js_ConfirmDelete = '<%=GetLocalResourceObject("rm_TempResassignUser_js_ConfirmDelete").ToString()%>'
        </script>
        <script src="<%= ResolveUrl("~/")%>res/js/usermaintenance/rm_TempReassignUser.js"></script>
    </tek:RadCodeBlock>
</asp:Content>
