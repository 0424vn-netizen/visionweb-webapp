<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AssignActivityGroupModal.aspx.cs" MasterPageFile="~/MasterPagePopup.master" Inherits="AssignActivityGroupModal" meta:resourcekey="PageResource1" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="Server">
    <as:ASModalContainer ID="uxModalContainer" runat="server" WidthCssClass="modal-md" ContainerCssClass="container">
        <%--<div class="row">
            <div class="col-md-12">
                <h3 class="modal-title">
                    <asp:Literal ID="lblHeader" runat="server" Text="Task Permissions" meta:resourcekey="lblHeader" ></asp:Literal>
                </h3>
            </div>
        </div>--%>
        <div id="groupPers">
             <table class="ASTable form-inline table-checkbox" id="uiTable">
                    <tbody>
                        <tr class="nobackground">
                            <th></th>
                            <th class="checkbox-block view"> 
                                <tek:RadCheckBox ID="Checkall_View" OnClientLoad="onCheckAllViewClientLoad" OnClientClicked="onCheckAll" Value="view" as-group="view" as-checkall="true" AutoPostBack="false" runat="server" meta:resourcekey="lblHeaderResource1" />
                            </th>
                            <th class="checkbox-block edit">
                                <tek:RadCheckBox ID="Checkall_Edit" OnClientLoad="onCheckAllEditClientLoad" OnClientClicked="onCheckAll" Value="edit" as-group="edit" as-checkall="true" AutoPostBack="false" runat="server" meta:resourcekey="lblHeaderResource2" />
                            </th>
                            <th class="checkbox-block manage">
                                <tek:RadCheckBox ID="Checkall_Manage" OnClientClicked="onCheckAll" Value="manage" as-group="manage" as-checkall="true" AutoPostBack="false" runat="server" meta:resourcekey="lblHeaderResource3" />
                            </th>
                        </tr>
                        <asp:Repeater runat="server" ID="uxPermissionsList" OnItemDataBound="uxPermissionsList_ItemDataBound">
                            <ItemTemplate>
                                <tr class="<%# Eval("IsVisible") %>">
                                    <td><%# Eval("GroupName") %></td>
                                    <td class="text-left view">
                                        <tek:RadCheckBox ID="uxView" Value='<%# Eval("View") %>' OnClientClicked="onCheckedChange" as-group="view" AutoPostBack="false" runat="server" />
                                    </td>
                                    <td class="text-left edit">
                                        <tek:RadCheckBox ID="uxEdit" Value='<%# Eval("Edit") %>' OnClientClicked="onCheckedChange" OnClientLoad="onClientLoad" as-group="edit" AutoPostBack="false" runat="server" />
                                    </td>
                                    <td class="text-left manage">
                                        <tek:RadCheckBox ID="uxManage" Value='<%# Eval("Manage") %>' OnClientClicked="onCheckedChange" OnClientLoad="onClientLoad" as-group="manage" AutoPostBack="false" runat="server" />
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </tbody>
                </table>           
        </div>
        <div class="row">
            <div class="col-md-12 form-action-container text-right">
                <as:HiddenField runat="server" ID="hdSelectedPers" />                
                <as:Button class="hidden" ID="btnSubmit" Text="Submit" OnClick="btnSubmit_Click" runat="server" />

                <as:Button class="btn btn-default" ID="btnSave" Text="Submit" OnClientClick=" GetPers(this); return false;" runat="server" meta:resourcekey="btnSave"  />
                <as:Button class="btn btn-default" ID="btnCancel" Text="Cancel" OnClientClick="ClosePopupModal();" runat="server" meta:resourcekey="btnCancel" />
            </div>
        </div>
    </as:ASModalContainer>
    <tek:RadCodeBlock runat="server" ID="RadCodeBlock">
        <script type="text/javascript">
            var hdSelectedPers = '<%= hdSelectedPers.ClientID%>';
            var checkall_view = '<%= Checkall_View.ClientID%>';
            var checkall_edit = '<%= Checkall_Edit.ClientID%>';
            var checkall_manage = '<%= Checkall_Manage.ClientID%>';
            var btnSubmit = '<%= btnSubmit.ClientID%>';
        </script>
        <script type="text/javascript" src="<%=ResolveUrl("~/") %>res/js/AssignActivityGroupModal.js"></script>
    </tek:RadCodeBlock>
</asp:Content>
