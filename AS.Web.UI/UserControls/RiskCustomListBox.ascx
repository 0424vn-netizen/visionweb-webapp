<%@ Control Language="C#" AutoEventWireup="true" CodeFile="RiskCustomListBox.ascx.cs" Inherits="CustomListBox" %>
<div id="<%=this.ClientID %>" class="RadListBox RadListBox_Default rlbFixedHeight customeListBox" >
    <div class="rlbGroup customListBox-container">
        <div class="rlbList list-group list-group-action">
            <asp:Repeater runat="server" ID="uxRepeater" OnItemDataBound="uxRepeater_ItemDataBound">
                <ItemTemplate>
                    <div class="rlbItem item" id="listItem" runat="server">
                        <label runat="server" id="uxLabel"/>
                        <asp:CheckBox runat="server" ID="uxIsSelected" OnCheckedChanged="uxIsSelected_btn" />
                        <asp:HiddenField runat="server" ID="uxValue" />
                        <asp:HiddenField runat="server" ID="uxFocus" />
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>
    </div>
</div>


