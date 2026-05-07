<%@ Control Language="C#" AutoEventWireup="true" CodeFile="rm_MCF_CustomView.ascx.cs" Inherits="UserControls_rm_MCF_CustomView" %>

<as:RadAjaxManagerProxy ID="ram" runat="server">
        <AjaxSettings>
            <tek:AjaxSetting AjaxControlID="uxCustomView">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="uxCustomizeColumnLink" LoadingPanelID="uxInvisiblePanel" />
                </UpdatedControls>
            </tek:AjaxSetting>
             <tek:AjaxSetting AjaxControlID="btnRebind">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="uxCustomView" />
                    <tek:AjaxUpdatedControl ControlID="uxCustomizeColumnLink" LoadingPanelID="uxInvisiblePanel" />
                    <tek:AjaxUpdatedControl ControlID="hdCustomViewID" />
                </UpdatedControls>
            </tek:AjaxSetting>
            <tek:AjaxSetting AjaxControlID="btnApply">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="uxCustomView" />
                    <tek:AjaxUpdatedControl ControlID="uxCustomizeColumnLink" LoadingPanelID="uxInvisiblePanel" />
                </UpdatedControls>
            </tek:AjaxSetting>
        </AjaxSettings>

    </as:RadAjaxManagerProxy>
<div class="custom-view" id="uxDivCustomView">
    <label>
        <as:Literal ID="lblCustomView" runat="server" Text="Barometer View: " meta:resourcekey="uclblCustomView"></as:Literal>
    </label>
    <div class="inline-block">
        <as:ASRadComboBox ID="uxCustomView" runat="server" Filter="Contains" MarkFirstMatch="true" Width="250px" ExpandDirection="Up" AutoPostBack="true"
            DataValueField="CustomViewID" DataTextField="ViewName" OnSelectedIndexChanged="uxCustomView_SelectedIndexChanged" tracking-key="BarometerView" tracking-type="combobox" >
        </as:ASRadComboBox>
        
    </div>
    <span class="dark-blue ml-4x inline-block">
        <asp:LinkButton runat="server" ID="uxCustomizeColumnLink" CssClass="link-back" Text="Customize" meta:resourcekey="Literal16Resource1"></asp:LinkButton>
    </span>
    <asp:Button runat="server" ID="btnRebind" CssClass="hide" OnClick="btnRebind_Click" />
    <as:HiddenField ID="hdCustomViewID" runat="server" />
</div>
<div runat="server" id="uxLoadJs" onload="uxLoadJs_Load"  class="hide"></div>
