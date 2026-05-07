<%@ Page Title="User Access Report" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="UserAccessReport.aspx.cs" Inherits="UserAccessReport" meta:resourcekey="PageResource1" %>

<%@ Register TagName="UxExport" Src="~/UserControls/UxExport.ascx" TagPrefix="uc" %>
<%@ Register TagName="PageTitle" Src="~/UserControls/PageTitle.ascx" TagPrefix="uc" %>
<%@ Register TagName="UserAccessFiltering" Src="~/UserControls/UserAccessFiltering.ascx"
    TagPrefix="uc" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="Server">
    <!-- Filtering  -->
    <uc:UserAccessFiltering ID="uxReportFiltering" OnFiltering="uxReportFiltering_Filtering" runat="server" />
    <div class="row">
        <div class="col-md-12 no-margin-bottom">
            <uc:PageTitle ID="uxReportTitle" runat="server" HasFilteringOption="true" ReportTitle="User Access Report" meta:resourcekey="uxReportTitleResource1" />
        </div>
    </div>
    <div class="height-16"></div>
    <!--Rad Grid-->
    <uc:UxExport ID="uxExporter" runat="server" GridHeader="" IsOnTop="true" OnNeedExportConfig="DoNeedExportConfig"
        GridID="uxReportGrid" />
    <as:ASGrid ID="uxReportGrid" runat="server" GridLines="None" AllowPaging="True" IsAutoExportTemplate="true"
        AllowSorting="True" AutoGenerateColumns="false" AllowSortFilterWhenExport="true" HeaderStyle-Width="110px" 
        BorderWidth="1" IntruderSourceName="uxReportGrid" IsIntruder="true" ASPagingMethod="SPASingleMethod1" AllowExportAtWebServices="false"
        ShowFooter="false" AllowFilteringByColumn="true" CssClass="in" meta:resourcekey="uxReportGridResource1">
        <MasterTableView>
            <Columns>
                <as:ASGridBoundColumn HeaderText="User Type" DataField="UserType" UniqueName="UserType"
                    ASFormat="StaticString" HeaderTooltip="User Type" SortExpression="UserType" meta:resourcekey="ASGridBoundColumnResource1">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn HeaderText="User ID" DataField="UserID" UniqueName="UserID"
                    SortExpression="UserID" ASFormat="StaticString" HeaderStyle-Width="140px"
                    HeaderTooltip="User ID" meta:resourcekey="ASGridBoundColumnResource2">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn HeaderText="User Name Full" DataField="UserNameFull" UniqueName="UserNameFull"
                    SortExpression="UserNameFull" ASFormat="DynamicString" HeaderTooltip="User Name Full" HeaderStyle-Width="200px"
                    AllowEncodeOnExporting="true" meta:resourcekey="ASGridBoundColumnResource3">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn HeaderText="Email Address" DataField="Email" UniqueName="Email" HeaderStyle-Width="250px"
                    ASFormat="DynamicString" HeaderTooltip="Email Address" SortExpression="Email" ItemStyle-Wrap="false"
                    AllowEncodeOnExporting="true" meta:resourcekey="ASGridBoundColumnResource4">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                    <ItemStyle Wrap="False"></ItemStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn HeaderText="User Create Date" AllowFiltering="false" DataField="CreatedDTS"
                    UniqueName="CreatedDTS" ASFormat="Date" HeaderTooltip="User Create Date" SortExpression="CreatedDTS"
                    AllowEncodeOnExporting="true" meta:resourcekey="ASGridBoundColumnResource5">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn HeaderText="1st Log in Date" AllowFiltering="false" DataField="FirstLogin"
                    UniqueName="FirstLogin" ASFormat="Date" HeaderTooltip="1st Log in Date" SortExpression="FirstLogin"
                    AllowEncodeOnExporting="true" meta:resourcekey="ASGridBoundColumnResource6">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn AllowFiltering="false" HeaderText="# Log Ins" UniqueName="LogInsCount"
                    DataField="LogInsCount" HeaderTooltip="Number Of Log Ins" ItemStyle-HorizontalAlign="Right"
                    ASFormat="Integer" meta:resourcekey="ASGridBoundColumnResource7">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn AllowFiltering="false" HeaderText="Last Log in" UniqueName="LastLoginDTS"
                    DataField="LastLoginDTS" ASFormat="Date" HeaderTooltip="Last Log in" meta:resourcekey="ASGridBoundColumnResource8">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn AllowFiltering="false" HeaderText="# Lock Outs" UniqueName="LockOutCount"
                    ASFormat="Integer" DataField="LockOutCount" HeaderTooltip="Number of Lock Outs" meta:resourcekey="ASGridBoundColumnResource9">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn AllowFiltering="false" HeaderText="Last Lock Out" UniqueName="LastLockedOutDTS"
                    ASFormat="Date" DataField="LastLockedOutDTS" HeaderTooltip="Last Lock Out" meta:resourcekey="ASGridBoundColumnResource10">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn AllowFiltering="false" HeaderText="# Password Resets" DataField="ResetPwdCount"
                    UniqueName="ResetPwdCount" ASFormat="Integer" HeaderTooltip="Number of Password Resets" meta:resourcekey="ASGridBoundColumnResource11">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn AllowFiltering="false" HeaderText="Last Password Reset" DataField="LastResetPwdDTS"
                    UniqueName="LastResetPwdDTS" ASFormat="Date" HeaderTooltip="Last Password Reset" meta:resourcekey="ASGridBoundColumnResource12">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn AllowFiltering="false" HeaderText="Status"
                    DataField="Status" UniqueName="Status" ASFormat="StaticString" HeaderTooltip="Status" meta:resourcekey="ASGridBoundColumnResource13">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
            </Columns>
        </MasterTableView>
    </as:ASGrid>
</asp:Content>

