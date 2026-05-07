<%@ Page Title="Temporarily Exclude Work Queue" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="rm_MCF_TemporarilyExcludeWorkQueue.aspx.cs" Inherits="rm_MCF_TemporarilyExcludeWorkQueue" EnableEventValidation="false" meta:resourcekey="PageResource1" %>

<%@ Register Src="~/UserControls/PageTitle.ascx" TagName="PageTitle" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/RiskTemporarilyExcludeWorkQueue.ascx" TagName="ExcludeWorkQueue" TagPrefix="uc" %>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="Server">
    <uc:PageTitle ID="uxPageTitle" runat="server" HasFilteringOption="false" ReportTitle="Temporarily Exclude Work Queue"
        meta:resourcekey="uxPageTitleResource1"/> 

    <%--start Create new Exclude--%>
    <as:LinkButton runat="server" ID="uxCreateMode" class="btn btn-default mt-m-2x " OnClick="uxCreateMode_Click"
        meta:resourcekey="uxCreateModeResource1">Create New Exclusion</as:LinkButton>
    <div class="row">
        <div class="col-md-12">
            <uc:ExcludeWorkQueue ID="uxExcludeWorkQueueCreate" runat="server" OnAfterSubmit="uxExcludeWorkQueue_AfterSubmit"
                OnAfterCancel="uxExcludeWorkQueue_AfterCancel" Visible="False" ContainerCss="risk-form-container" />
        </div>
    </div>
    <div class="height-12"></div>
    <%--end Create new Exclude--%>

    <div class="row ">
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

    <div class="row mt-5x">
        <div class="col-md-12">
            <as:ASGrid ID="uxReportGrid" runat="server" AllowPaging="True"
                AllowSorting="True" AutoGenerateColumns="False" OnNeedDataSource="uxReportGrid_NeedDataSource"
                OnUpdateCommand="uxReportGrid_UpdateCommand" OnItemDataBound="uxReportGrid_ItemDataBound"
                OnPageIndexChanged="uxReportGrid_PageIndexChanged" CssClass="in" meta:resourcekey="uxReportGridResource1">
                <MasterTableView>
                    <Columns>
                        <tek:GridEditCommandColumn UniqueName="EditCommandColumn" HeaderStyle-Width="45px" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" meta:resourcekey="GridEditCommandColumnResource1">
                        </tek:GridEditCommandColumn>
                        <as:ASGridBoundColumn UniqueName="WorkQueueName" HeaderText="Exclude Work Queue" DataField="WorkQueueName"
                            SortExpression="WorkQueueName" ASFormat="StaticString" HeaderTooltip="Exclude Work Queue"
                            AllowEncodeOnExporting="true" meta:resourcekey="ASGridBoundColumnResource1">
                        </as:ASGridBoundColumn>
                        <as:ASGridBoundColumn UniqueName="AutoQueueNames" HeaderText="Auto Queue Name" DataField="AutoQueueNames"
                            AllowEncodeOnExporting="true" SortExpression="AutoQueueNames" HeaderTooltip="Auto Queue Name"
                            ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource2">
                        </as:ASGridBoundColumn>
                        <as:ASGridBoundColumn UniqueName="FromDate" HeaderText="From" DataField="FromDate"
                            ASFormat="Date" HeaderTooltip="Start date" SortExpression="FromDate" HeaderStyle-Width="100px"
                            meta:resourcekey="ASGridBoundColumnResource3">
                        </as:ASGridBoundColumn>
                        <as:ASGridBoundColumn UniqueName="ToDate" HeaderText="To" DataField="ToDate"
                            ASFormat="Date" SortExpression="ToDate" HeaderTooltip="End date" HeaderStyle-Width="100px"
                            meta:resourcekey="ASGridBoundColumnResource4">
                        </as:ASGridBoundColumn>
                        <as:ASGridTemplateColumn DataField="AssignmentID" HeaderStyle-Width="60px" UniqueName="Delete" 
                            ItemStyle-HorizontalAlign="Center"  HeaderStyle-HorizontalAlign="Center"
                             meta:resourcekey="ASGridTemplateColumnResource1" >
                            <ItemTemplate>
                                <as:LinkButton ID="uxDelete" runat="server" Text="Delete"
                                    OnCommand="DeleteExclude_Command" CommandArgument='<%# Eval("ExclusionID") %>'
                                    OnClientClick='return DeleteExclude();' meta:resourcekey="uxDeleteResource1"></as:LinkButton>
                            </ItemTemplate>
                        </as:ASGridTemplateColumn>
                    </Columns>
                    <EditFormSettings EditFormType="Template">
                        <EditColumn UniqueName="EditCommandColumn1" FilterControlAltText="Filter EditCommandColumn1 column"></EditColumn>
                        <FormTemplate>
                            <uc:ExcludeWorkQueue ID="uxEditExcludeWorkQueue" runat="server" UpdateMode="true" 
                                OnAfterSubmit="uxExcludeWorkQueue_AfterSubmit" OnAfterCancel="uxExcludeWorkQueue_AfterCancel" />
                        </FormTemplate>
                    </EditFormSettings>
                </MasterTableView>
            </as:ASGrid>
        </div>
    </div>

    <tek:RadCodeBlock runat="server" ID="RadCodeBlock2">
        <script type="text/javascript">
            var rm_TemporarilyExcludeWorkQueue_ConfirmExclude = '<%=Resources.UserMaintenanceMessage.TemporaryExcludeWorkQueue_ConfirmExclude %>';
            var rm_TemporarilyExcludeWorkQueue_js_ConfirmDelete = '<%=GetLocalResourceObject("rm_TemporarilyExcludeWorkQueue_js_ConfirmDelete").ToString()%>';
        </script>
        <script src="<%= ResolveUrl("~/")%>res/js/usermaintenance/rm_TemporarilyExcludeWorkQueue.js"></script>
    </tek:RadCodeBlock>
</asp:Content>
