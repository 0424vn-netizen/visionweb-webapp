<%@ Control Language="C#" AutoEventWireup="true" CodeFile="rm_MCF_AccessFuntionControl.ascx.cs" Inherits="UserControls_rm_MCF_AccessFuntionControl" %>

<div class="treeview">
    <asp:Repeater ID="uxAccessFuncList" runat="server" OnItemDataBound="uxAccessFuncList_ItemDataBound">
        <ItemTemplate>
            <div id="uxPnlGroupLabel" runat="server" class="parent pointer minus">
                <span class="icon"></span>
                <span>
                    <asp:Literal ID="uxGroupLabel" runat="server"></asp:Literal>
                </span>
            </div>
            <div id="uxPnlGroupChecbox" runat="server" class="child">
                <div class="checkbox">
                    <as:CheckBox ID="uxPermission" runat="server" AutoPostBack="false" />
                </div>
            </div>
        </ItemTemplate>
    </asp:Repeater>
</div>
<script type="text/javascript" src="<%= ResolveUrl("~/") %>res/js/usermaintenance/AccessFuntionControl.js"></script>








