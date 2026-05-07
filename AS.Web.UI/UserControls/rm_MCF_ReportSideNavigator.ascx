<%@ Control Language="C#" AutoEventWireup="true" CodeFile="rm_MCF_ReportSideNavigator.ascx.cs" Inherits="UserControls_rm_MCF_ReportSideNavigator" %>
<div class="affix" >
    <ul class="nav nav-list merchant-sidenav">
        <as:PlaceHolder ID="plhInvestigation" runat="server">
            <li class="inline">
                <a id="addComment" href="#" class="inline"><as:Literal ID="ltAddInvestigation" runat="server" Text="Add Investigation" meta:resourcekey="ltAddInvestigationResource1"></as:Literal></a>
            </li>
        </as:PlaceHolder>
        <li>
            <a href="#merchinfo">
                <i class="icon-chevron-right"></i>
                <as:Literal ID="Literal1" runat="server" Text="Merch Info" meta:resourcekey="Literal1Resource1"></as:Literal>
            </a>
        </li>
        <li>
            <a href="#riskmgmt">
                <i class="icon-chevron-right"></i>
                <as:Literal ID="Literal2" runat="server" Text="Risk MGMT" meta:resourcekey="Literal2Resource1"></as:Literal>
            </a>
        </li>
        <li>
            <a href="#salesdata">
                <i class="icon-chevron-right"></i>
                <as:Literal ID="Literal3" runat="server" Text="Sales Data" meta:resourcekey="Literal3Resource1"></as:Literal>
            </a>
        </li>
        <li>
            <a href="#swipekeyrate">
                <i class="icon-chevron-right"></i>
                <as:Literal ID="Literal4" runat="server" Text="Swipe-Key Rate" meta:resourcekey="Literal4Resource1"></as:Literal>
            </a>
        </li>
        <li>
            <a href="#authorization">
                <i class="icon-chevron-right"></i>
                <as:Literal ID="Literal5" runat="server" Text="Authorization" meta:resourcekey="Literal5Resource1"></as:Literal>
            </a>
        </li>
        <li>
            <a href="#batchhistory">
                <i class="icon-chevron-right"></i>
                <as:Literal ID="Literal6" runat="server" Text="Batch History" meta:resourcekey="Literal6Resource1"></as:Literal>
            </a>
        </li>
        <li>
            <a href="#transaction">
                <i class="icon-chevron-right"></i>
                <as:Literal ID="Literal7" runat="server" Text="Transaction" meta:resourcekey="Literal7Resource1"></as:Literal>
            </a>
        </li>
        <li>
            <a href="#investigation">
                <i class="icon-chevron-right"></i>
                <as:Literal ID="Literal8" runat="server" Text="Investigation" meta:resourcekey="Literal8Resource1"></as:Literal>
            </a>
        </li>
        <li>
            <a href="#riskcomments">
                <i class="icon-chevron-right"></i>
                <as:Literal ID="Literal9" runat="server" Text="Risk Comments" meta:resourcekey="Literal9Resource1"></as:Literal>
            </a>
        </li>
        
    </ul>
</div>