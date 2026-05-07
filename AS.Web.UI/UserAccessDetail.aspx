<%@ Page Title="Log In Session Details" Language="C#" MasterPageFile="~/MasterPagePopup.master"
    AutoEventWireup="true" CodeFile="UserAccessDetail.aspx.cs" Inherits="UserAccessDetail" meta:resourcekey="PageResource1" %>

<%@ Register TagName="UxExport" Src="~/UserControls/UxExport.ascx" TagPrefix="uc" %>
<%@ Register TagName="PageTitle" Src="~/UserControls/PageTitle.ascx" TagPrefix="uc" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="Server">
    <as:ASModalContainer ID="uxModalContainer" runat="server" WidthCssClass="modal-lg" ContainerCssClass="container" Width="">
        <div class="row">
            <div class="col-md-12">
                <h3 class="modal-title">
                    <asp:Literal runat="server" ID="header" meta:resourcekey="headerResource1" />
                </h3>
                <uc:UxExport runat="server" GridHeader="" IsOnTop="true" GridID="uxReportGrid"/>
                <as:ASGrid ID="uxReportGrid" runat="server" GridLines="None" AllowPaging="True" IsAutoExportTemplate="true"
                    AllowSorting="True" AutoGenerateColumns="false" AllowSortFilterWhenExport="true"
                    BorderWidth="1" IntruderSourceName="uxReportGrid" IsIntruder="true" ASPagingMethod="SPASingleMethod"
                    ShowFooter="false" AllowFilteringByColumn="false" CssClass="in" meta:resourcekey="uxReportGridResource1">
                    <MasterTableView>
                        <Columns>
                            <as:ASGridBoundColumn HeaderText="Login DTS" AllowFiltering="false" DataField="StartDTS"
                                UniqueName="StartDTS" ASFormat="StaticString" HeaderTooltip="Login Date Time Stamp"
                                SortExpression="StartDTS"
                                AllowEncodeOnExporting="true" HeaderStyle-Width="150px" meta:resourcekey="ASGridBoundColumnResource1">
                                <ColumnValidationSettings>
                                    <ModelErrorMessage Text=""></ModelErrorMessage>
                                </ColumnValidationSettings>

                                <HeaderStyle HorizontalAlign="Center" Width="150px"></HeaderStyle>
                            </as:ASGridBoundColumn>
                            <as:ASGridBoundColumn AllowFiltering="false" HeaderText="Session Length" UniqueName="SessionLength"
                                DataField="SessionLength" HeaderTooltip="Session Length" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource2">
                                <ColumnValidationSettings>
                                    <ModelErrorMessage Text=""></ModelErrorMessage>
                                </ColumnValidationSettings>

                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            </as:ASGridBoundColumn>
                            <as:ASGridBoundColumn AllowFiltering="false" HeaderText="Log In Success" DataField="LogInSuccess"
                                UniqueName="LogInSuccess" ASFormat="StaticString" HeaderTooltip="Log In Success" meta:resourcekey="ASGridBoundColumnResource3">
                                <ColumnValidationSettings>
                                    <ModelErrorMessage Text=""></ModelErrorMessage>
                                </ColumnValidationSettings>

                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            </as:ASGridBoundColumn>
                        </Columns>
                    </MasterTableView>
                </as:ASGrid>
            </div>
        </div>
    </as:ASModalContainer>
</asp:Content>
