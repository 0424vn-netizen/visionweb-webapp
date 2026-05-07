<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CardHistoryNavigator.ascx.cs" Inherits="UserControls_CardHistoryNavigator" %>

<div class="affix">
    <ul class="nav nav-list merchant-sidenav">
        <li>
            <a href="#NavCardHistory">
                <i class="icon-chevron-right"></i>
                <asp:Literal ID="Literal1" runat="server" Text="Card" meta:resourcekey="LiteralResource1" />
            </a>
        </li>
        <li>
            <a href="#NavAuthorizationHistory">
                <i class="icon-chevron-right"></i>
                <asp:Literal ID="Literal2" runat="server" Text="Authorization" meta:resourcekey="LiteralResource2" />                
            </a>
        </li>
        <li>
            <a href="#NavChargebackHistory">
                <i class="icon-chevron-right"></i>
                <asp:Literal ID="Literal3" runat="server" Text="Chargeback" meta:resourcekey="LiteralResource3" />                
            </a>
        </li>
        <li>
            <a href="#NavRetrievalHistoty">
                <i class="icon-chevron-right"></i>
                <asp:Literal ID="Literal4" runat="server" Text="Retrieval" meta:resourcekey="LiteralResource4" />                
            </a>
        </li>
    </ul>
</div>

