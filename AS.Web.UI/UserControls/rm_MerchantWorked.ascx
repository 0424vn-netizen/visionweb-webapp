<%@ Control Language="C#" AutoEventWireup="true" CodeFile="rm_MerchantWorked.ascx.cs" Inherits="UserControls_rm_MerchantWorked" %>
<%@ Register Src="~/UserControls/UxExport.ascx" TagName="UxExport" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/PageTitle.ascx" TagName="PageTitle" TagPrefix="uc" %>

<tek:RadAjaxManagerProxy runat="server" ID="uxRadAjaxManager">
    <AjaxSettings>
        <tek:AjaxSetting AjaxControlID="uxGroupList">
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="uxGroupList" />
            </UpdatedControls>
        </tek:AjaxSetting>
    </AjaxSettings>
     <AjaxSettings>
        <tek:AjaxSetting AjaxControlID="uxUserList">
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="uxGroupList" />
            </UpdatedControls>
        </tek:AjaxSetting>
    </AjaxSettings>
</tek:RadAjaxManagerProxy>
<uc:UxExport ID="uxExporter" runat="server" GridID="uxGroupList" OnNeedExportConfig="uxExporter_NeedExportConfig" ShowPDF="false"  GridHeader="" meta:resourcekey="uxExportTopResource1" />
<tek:RadComboBox  runat="server" ID="uxUserList" OnSelectedIndexChanged="uxUserList_SelectedIndexChanged" Visible="false" AutoPostBack="true">
</tek:RadComboBox>
<as:PlaceHolder ID="uxPlaceHolder" runat="server">
    <as:ASGrid ID="uxGroupList" runat="server" AutoGenerateColumns="false" AllowSorting="true" ASPagingMethod="SPASingleMethod"
        OnNeedDataSource="uxGroupList_NeedDataSource" OnItemDataBound="uxGroupList_ItemDataBound" IsAutoExportTemplate="true"
        GridLines="None" AllowPaging="true" ShowPageTotal="false" meta:resourcekey="uxDrilldownGridResource1">
        <MasterTableView>
            <Columns>
                <%--User Name--%>
                <as:ASGridBoundColumn HeaderText="User Name" DataField="UserName" UniqueName="UserName" Visible="false"
                    SortExpression="UserName" HeaderTooltip="UserName" ASFormat="StaticString" meta:resourcekey="ASGridTemplateColumnResource1">
                </as:ASGridBoundColumn>
                <%--Time Worked--%>
                <as:ASGridBoundColumn HeaderText="Time Stamp" HeaderTooltip="Time Stamp" UniqueName="TimeWorked" SortExpression="TimeWorked"
                    ASFormat="StaticString" DataField="TimeWorked" meta:resourcekey="ASGridTemplateColumnResource2">
                </as:ASGridBoundColumn>
                <%--Merchant Name--%>
                <as:ASGridBoundColumn HeaderText="Merchant Name" HeaderTooltip="Merchant Name" UniqueName="MerchantName" SortExpression="MerchantName"
                    ASFormat="DynamicString" DataField="MerchantName" meta:resourcekey="ASGridTemplateColumnResource3">
                </as:ASGridBoundColumn>
                <%--Merchant ID--%>
                <as:ASGridBoundColumn HeaderText="Merchant ID" HeaderTooltip="Merchant ID" UniqueName="MerchantNumber" SortExpression="MerchantNumber" HeaderStyle-Width="150px"
                    ASFormat="StaticString" DataField="MerchantNumber" meta:resourcekey="ASGridTemplateColumnResource4">
                </as:ASGridBoundColumn>
                <%--Volume--%>
                <as:ASGridBoundColumn HeaderText="Today's Volume" HeaderTooltip="Today's Volume" UniqueName="Volume" SortExpression="Volume"
                    ASFormat="StaticString" DataField="Volume" meta:resourcekey="ASGridTemplateColumnResource5">
                </as:ASGridBoundColumn>
                <%--Highest Ticket--%>
                <as:ASGridBoundColumn HeaderText="Highest Ticket" HeaderTooltip="Highest Ticket" UniqueName="HighestTicket" SortExpression="HighestTicket"
                    ASFormat="StaticString" DataField="HighestTicket" meta:resourcekey="ASGridTemplateColumnResource6">
                </as:ASGridBoundColumn>
            </Columns>
        </MasterTableView>
    </as:ASGrid>
     <as:RadCodeBlock ID="JavaScript" runat="server">
        <script type="text/javascript">
            function CloseModal(e) {
                parent.doAssign();
                parent.ClosePopupModal(0);
                window.location.href = e.href;
            }
        </script>
    </as:RadCodeBlock>
</as:PlaceHolder>
