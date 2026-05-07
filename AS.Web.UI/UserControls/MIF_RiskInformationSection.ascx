<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MIF_RiskInformationSection.ascx.cs" Inherits="UserControls_MIF_RiskInformationSection" %>




<div class="row" id="RiskScoreInfo">
    <div class="col-xs-8" data-toggle="collapse" data-target="#ciRiskInformation">
        <h2 class="grid-title">
            <asp:Literal ID="Literal44" runat="server" Text="Risk Information" meta:resourcekey="RiskInformationResource" /></h2>
    </div>
    <div class="col-xs-4 ipmt dropdown on-top mb-3x">
        <h1 class="dark-blue pull-right" runat="server" id="pnlViewRiskReportLink">
            <asp:HyperLink CssClass="link-back font-size-default" ID="btnViewRiskReport" runat="server" NavigateUrl="~/Risk/rm_RiskReport.aspx" meta:resourcekey="ViewRiskReportResource"></asp:HyperLink>

        </h1>
    </div>

</div>
<div class="row">
    <div class="col-md-12 in" id="ciRiskInformation">
        <table class="ASTable">
            <colgroup>
                <col style="width: 160px" />
                <col />
            </colgroup>
            <tr class="Row">
                <td class="heading">
                    <asp:Literal ID="Literal1" runat="server" Text="Classification Name:" meta:resourcekey="ClassificationNameResource" /></td>
                <td>
                    <%= BindValue("MerchantClassificationName")%>
                </td>
            </tr>
            <tr class="AltRow">
                <td class="heading">
                    <asp:Literal ID="Literal2" runat="server" Text="Multiplier:" meta:resourcekey="MultiplierResource" /></td>
                <td>
                    <%= BindValue("Multiplier")%>
                </td>
            </tr>
             <tr class="Row">
                <td class="heading">
                    <asp:Literal ID="Literal4" runat="server" Text="Risk Score:" meta:resourcekey="RiskScoreResource" /></td>
                <td>
                    <%= BindValue("TotalRS")%>
                </td>
            </tr>

            <tr class="AltRow" id="uxGVerifyCode" runat="server" visible="false">
                <td class="heading">
                    <asp:Literal ID="Literal3" runat="server" Text="Gverify Code:" meta:resourcekey="GverifyCodeResource" /></td>
                <td>
                    <%= BindValue("GVerifyCode")%>
                </td>
            </tr>
           
            <tr class="Row"  id="uxGauthenticate" runat="server" visible="false">
                <td class="heading">
                    <asp:Literal ID="Literal5" runat="server" Text="Gauthenticate Code:" meta:resourcekey="GauthenticateCodeResource" /></td>
                <td>
                    <%= BindValue("GAuthenticateCode")%>
                </td>
            </tr>
            <tr class="AltRow"  id="uxG2CompassAutoApprovalIndicator" runat="server" visible="false">
                <td class="heading">
                    <asp:Literal ID="Literal6" runat="server" Text="Auto Approval Indicator:" meta:resourcekey="AutoApprovalIndicatorResource" /></td>
                <td>
                    <%= BindValue("G2CompassAutoApprovalIndicator")%>
                </td>
            </tr>
        </table>
    </div>
</div>
