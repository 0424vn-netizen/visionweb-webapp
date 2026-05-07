<%@ Page Language="C#" MasterPageFile="~/MasterPagePopup.master" AutoEventWireup="true" CodeFile="rm_MCF_Filter_MarketData_Modal.aspx.cs" Inherits="rm_MCF_Filter_MarketData_Modal" Title="Select Market Data" meta:resourcekey="PageResource1" %>
<%@ Register src="~/UserControls/rm_MCF_FilterMarketData.ascx" tagname="MarketData" tagprefix="uc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" Runat="Server">
    <script language="javascript" type="text/javascript">
        parent.setModalID("MarketDataModal");
    </script>

    <div style="padding: 10px; height:660px; width:650px;">
        <center>
            <div style="text-align: left; margin-bottom: 10px; font-style: italic;">
               <asp:Literal ID="Literal1" runat="server" meta:resourcekey="Literal1Resource1" Text=" This Market Data Code Set is combined with Market Data Type to determine selected
                Parameters."></asp:Literal>
            </div>
            <div style="text-align: left;">
                <uc:MarketData ID="uxMarketDataFilter" runat="server" WidthUC="630" HeightUC="500" />
            </div>
            <div style="text-align: right; margin-top: 5px;">
                <as:Button ID="btnClose" runat="server" Text="Close" OnClick="btnClose_Click" IsStandardButton="False" meta:resourcekey="btnCloseResource1" />
            </div>
        </center>
    </div>
    <as:RadCodeBlock runat="server" ID="RadCodeBlock1">
        <script type="text/javascript">
            function checkMarketData(str) {
                alert(str);
            }
        </script>
    </as:RadCodeBlock>
</asp:Content>

