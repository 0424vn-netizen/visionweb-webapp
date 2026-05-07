<%@ Page Title="ACTIVE PROFILES" Language="C#" MasterPageFile="~/MasterPagePopup.master"
    AutoEventWireup="true" CodeFile="rm_MCF_MerchantsInProfile.aspx.cs" Inherits="rm_MCF_MerchantsInProfile" meta:resourcekey="PageResource1" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="Server">
    <as:PlaceHolder ID="dfd" runat="server">
    <as:ASModalContainer ID="ASModalContainer1" runat="server">
          <h3 class="modal-title"><asp:Literal ID="Literal1" runat="server" meta:resourcekey="Literal1Resource1"> Selected Merchant Profile cannot be deactivated because the profile is still in use for the following merchants.</asp:Literal>
        </h3>
        <uc:UxExport ID="uxExportTop" GridID="uxReportGrid" runat="server" FileName="ActiveProfiles"
            IsOnTop="true" />
        <as:ASGrid ID="uxReportGrid" runat="server" AllowPaging="True" AllowSorting="True"
            AutoGenerateColumns="False" AllowAutoCalculateTotalOnExport="false" AllowFilteringByColumn="false"
            ASPagingMethod="SPASingleMethod" ShowPageTotal="false" ShowReportTotal="false"
            AllowSortFilterWhenExport="true" CssClass="in" meta:resourcekey="uxReportGridResource1">
            <MasterTableView>
                <Columns>
                    <as:ASGridBoundColumn UniqueName="MerchantNumber" HeaderText="Merchant ID" HeaderTooltip="Merchant ID"
                        DataField="MerchantNumber" SortExpression="MerchantNumber" ASFormat="StaticString"
                        HeaderStyle-Width="140px" meta:resourcekey="ASGridBoundColumnResource1">
<ColumnValidationSettings>
<ModelErrorMessage Text=""></ModelErrorMessage>
</ColumnValidationSettings>

<HeaderStyle HorizontalAlign="Center" Width="140px"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn UniqueName="MerchantName" HeaderText="Merchant Name" HeaderTooltip="Merchant Name"
                        DataField="MerchantName" SortExpression="MerchantName" ASFormat="DynamicString" meta:resourcekey="ASGridBoundColumnResource2">
<ColumnValidationSettings>
<ModelErrorMessage Text=""></ModelErrorMessage>
</ColumnValidationSettings>

<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn UniqueName="ProfileName" HeaderText="Profile" HeaderTooltip="ProfileName"
                        DataField="ProfileName" SortExpression="ProfileName" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource3">
<ColumnValidationSettings>
<ModelErrorMessage Text=""></ModelErrorMessage>
</ColumnValidationSettings>

<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn UniqueName="StatusDescription" HeaderText="Current Status of Merchant"
                        HeaderTooltip="Current Status of Merchant" DataField="StatusDescription"
                        SortExpression="StatusDescription" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource4">
<ColumnValidationSettings>
<ModelErrorMessage Text=""></ModelErrorMessage>
</ColumnValidationSettings>

<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                </Columns>
            </MasterTableView>
        </as:ASGrid>
    </as:ASModalContainer>
        </as:PlaceHolder>
    <tek:RadCodeBlock runat="server">
        <script type="text/javascript">
            var linkRiskReport = '<%=ResolveUrl("~/risk_MCF/rm_MCF_RiskReport.aspx") %>?';            
        </script>
        <script src="<%=ResolveUrl("~")%>res/js/risk_MCF/rm_MCF_MerchantsInProfile.js"></script>
    </tek:RadCodeBlock>

    <tek:RadAjaxManagerProxy ID="RadAjaxManager" runat="server">
        <AjaxSettings>
            <tek:AjaxSetting AjaxControlID="uxReportGrid">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="uxReportGrid" />
                </UpdatedControls>
            </tek:AjaxSetting>
        </AjaxSettings>
    </tek:RadAjaxManagerProxy>
</asp:Content>
