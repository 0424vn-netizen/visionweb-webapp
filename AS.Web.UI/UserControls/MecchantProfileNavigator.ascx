<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MecchantProfileNavigator.ascx.cs" Inherits="UserControls_MecchantProfileNavigator" %>
<div class="affix" >
    <ul class="nav nav-list merchant-sidenav">
        <as:PlaceHolder ID="uxPlaceHolderNavigator" runat="server">
        </as:PlaceHolder>        
        <li runat="server" id="uxCaseHistory" visible="false">
            <a href="#casehistory">
                <i class="icon-chevron-right"></i>
                <asp:Literal ID="Literal1" runat="server" Text="Case History" meta:resourcekey="LiteralResource1" />
            </a>
        </li>
    </ul>
</div>
