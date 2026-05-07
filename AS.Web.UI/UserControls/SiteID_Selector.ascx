<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SiteID_Selector.ascx.cs" Inherits="UserControls_SiteID_Selector" %>
<div class="PanelGreyPadding">
    <table cellpadding="3px">
        <tr>
            <td style="font-style:normal;"><as:Literal ID="ltPorfolio" runat="server" Text="Portfolio:" meta:resourcekey="ltPorfolioResource1"></as:Literal> </td>
            <td>
                <as:RadComboBox ID="uxSiteIDs" runat="server"  MaxHeight="100px" Width="300px" Height="50px" DataTextField="ClientName" DataValueField="SiteID" />
            </td>
            <td><as:Button ID="uxSubmit" runat="server" Text="Submit" OnClick="uxSubmit_Click" IsStandardButton="False" meta:resourcekey="uxSubmitResource1" /></td>
        </tr>
    </table>
     
</div>