<%@ Page Title="Business Websites" Language="C#" MasterPageFile="~/MasterPagePopup.master"
    AutoEventWireup="true" CodeFile="WebsiteModal.aspx.cs" Inherits="WebsiteModal" meta:resourcekey="ModalTitle" %>
<%@ Register TagName="UxExport" Src="~/UserControls/UxExport.ascx" TagPrefix="uc" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="Server">
    <as:ASModalContainer ID="ASModalContainer1" runat="server" ContainerCssClass="container" WidthCssClass="modal-xl">
        
        <div runat="server" id="sdasdasdas">
            <a id="NavCardHistory" class="anchor-link-no-padding"></a>
            <uc:UxExport ID="uxExporterTop" runat="server" GridID="uxReportGrid" GridTitle="Business Websites" meta:resourcekey="PageResource1"
                IsOnTop="true" ShowExcel="true" ShowPDF="true" ShowWord="false"/>
            <as:ASGrid ID="uxReportGrid" runat="server" GridLines="None" AllowPaging="True" AppendHeaderforPrinter="true"
            AllowSorting="True" AutoGenerateColumns="False" AllowFilteringByColumn="false" HeaderStyle-Width="100px"
            ShowFooter="true" ASPagingMethod="SPASingleMethod" ShowPageTotal="false" VisiblePageTotal="false" IsAutoExportTemplate="true"
            VisibleReportTotal="true" AllowSortFilterWhenExport="true" MasterTableView-ShowFooter="false" CssClass="in" meta:resourcekey="uxReportGridResource1" >
                <MasterTableView>
                    <PagerStyle AlwaysVisible="true" />
                    <Columns>
                        <as:ASGridBoundColumn HeaderText="Site ID" DataField="SiteID" HeaderTooltip="Site ID" ASDefaultNullValue="—"
                            UniqueName="SiteID" HeaderStyle-Width="50px" meta:resourcekey="SiteID" ASFormat="StaticString" ItemStyle-HorizontalAlign="Center">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text=""></ModelErrorMessage>
                            </ColumnValidationSettings>

                            <HeaderStyle HorizontalAlign="Center" Width="50px"></HeaderStyle>
                        </as:ASGridBoundColumn>
                        <as:ASGridBoundColumn HeaderText="Website URL" DataField="WebsiteURL" HeaderTooltip="Website URL" ASDefaultNullValue="—"
                            UniqueName="WebsiteURL" HeaderStyle-Width="150px" meta:resourcekey="WebsiteURL" ASFormat="StaticString" ItemStyle-HorizontalAlign="Center">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text=""></ModelErrorMessage>
                            </ColumnValidationSettings>

                            <HeaderStyle HorizontalAlign="Center" Width="150px"></HeaderStyle>
                        </as:ASGridBoundColumn>
                        <as:ASGridBoundColumn HeaderText="Descriptor" DataField="Descriptor" HeaderTooltip="Descriptor" ASDefaultNullValue="—"
                            UniqueName="Descriptor" HeaderStyle-Width="120px" meta:resourcekey="Descriptor" ASFormat="StaticString" ItemStyle-HorizontalAlign="Center">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text=""></ModelErrorMessage>
                            </ColumnValidationSettings>

                            <HeaderStyle HorizontalAlign="Center" Width="120px"></HeaderStyle>
                        </as:ASGridBoundColumn>
                        <as:ASGridBoundColumn HeaderText="Currency Type" DataField="CurrencyType" HeaderTooltip="Currency Type" ASDefaultNullValue="—"
                            UniqueName="CurrencyType" HeaderStyle-Width="100px" meta:resourcekey="CurrencyType" ASFormat="StaticString" ItemStyle-HorizontalAlign="Center">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text=""></ModelErrorMessage>
                            </ColumnValidationSettings>

                            <HeaderStyle HorizontalAlign="Center" Width="100px"></HeaderStyle>
                        </as:ASGridBoundColumn>
                        <as:ASGridBoundColumn HeaderText="Prefix" DataField="Prefix" HeaderTooltip="Prefix" ASDefaultNullValue="—"
                            UniqueName="Prefix" HeaderStyle-Width="40px" meta:resourcekey="Prefix" ASFormat="StaticString" ItemStyle-HorizontalAlign="Center">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text=""></ModelErrorMessage>
                            </ColumnValidationSettings>

                            <HeaderStyle HorizontalAlign="Center" Width="40px"></HeaderStyle>
                        </as:ASGridBoundColumn>
                        <as:ASGridBoundColumn HeaderText="Phone Number" DataField="PhoneNumber" ASDefaultNullValue="—"
                            HeaderTooltip="Phone Number" UniqueName="PhoneNumber" ASFormat="Phone" meta:resourcekey="PhoneNumber" ItemStyle-HorizontalAlign="Center">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text=""></ModelErrorMessage>
                            </ColumnValidationSettings>

                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        </as:ASGridBoundColumn>
                        <as:ASGridBoundColumn HeaderText="Status" DataField="Status" HeaderTooltip="Status" ASFormat="StaticString"
                            UniqueName="Status" HeaderStyle-Width="60px" meta:resourcekey="Status" ASDefaultNullValue="—" ItemStyle-HorizontalAlign="Center">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text=""></ModelErrorMessage>
                            </ColumnValidationSettings>

                            <HeaderStyle HorizontalAlign="Center" Width="60px"></HeaderStyle>
                        </as:ASGridBoundColumn>
                        <as:ASGridBoundColumn HeaderText="Activation Date" DataField="ActivationDate" HeaderTooltip="Activation Date" meta:resourcekey="ActivationDate"
                            UniqueName="ActivationDate" ASFormat="Date" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="100px" ASDefaultNullValue="—">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text=""></ModelErrorMessage>
                            </ColumnValidationSettings>

                            <HeaderStyle HorizontalAlign="Center" Width="100px"></HeaderStyle>
                        </as:ASGridBoundColumn>

                    </Columns>
                </MasterTableView>
            </as:ASGrid>
        </div>
        <div class="row">
            <div class="col-md-12 form-action-container text-right">
                <as:Button runat="server" CssClass="btn btn-default" ID="uxCancel" Text="Close" OnClientClick="return closeMe();" IsStandardButton="False" meta:resourcekey="uxCancelResource1" />
            </div>
        </div>
    </as:ASModalContainer>
    <tek:RadCodeBlock ID="uxRadCodeBlock1" runat="server">
        <script type="text/javascript">
        </script>
        <script type="text/javascript" src="<%= ResolveUrl("~")%>res/js/WebsiteModal.js"> </script>
    </tek:RadCodeBlock>
</asp:Content>
