<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPagePopup.master" CodeFile="ViewRoleInfo.aspx.cs" Inherits="ViewRoleInfo" meta:resourcekey="PageResource1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPage" runat="Server">
    <as:ASModalContainer ID="uxModalContainer" runat="server" Width="660px" ContainerCssClass="container" WidthCssClass="modal-xxl">
        <div class="row">
            <div class="col-md-12">
                <h3 class="modal-title no-margin-bottom">
                    <asp:Literal ID="Literal2" runat="server" Text="Permissions" meta:resourcekey="LiteralResource2" /></h3>
            </div>
            <div class="col-md-12 no-margin-bottom">
                <div class="height-10"></div>
                <i>
                    <as:Literal ID="ltClickOnUserToEdit" runat="server" Text="This role is a pre-defined role and is not editable" meta:resourcekey="ltMgsNotEditPreRoleResource1"></as:Literal></i>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12 relative">
                <h4>Menus</h4>
                <div class="box fixed-height-block height-300">
                    <div id="clientCsMenu" class="treeview-check-icon">
                        <tek:RadTreeView runat="server" TriStateCheckBoxes="true" Width="90%" OnNodeDataBound="uxTreeMenuCS_NodeDataBound"
                            ID="uxTreeMenuCS" CheckBoxes="true" CheckChildNodes="true" Enabled="false" />
                    </div>
                </div>
                <div class="param-settings" id="pnlAltRole">
                    <div class=" reset-line-height">
                        <as:PlaceHolder ID="uxPlc1099KRole" runat="server" Visible="false">
                            <table class="w-75 font-arial font-12" id="tb1099KRole">
                                <tr>
                                    <td class="text-left w-40">
                                        <label class="dark-blue">
                                            <b>
                                                <asp:Literal ID="Literal4" runat="server" Text="1099-K CompliAssure:" meta:resourcekey="LiteralResource8" /></b></label>
                                    </td>
                                    <td>
                                        <asp:Literal ID="lb1099K" runat="server" Text="Manager" />
                                    </td>
                                </tr>
                            </table>
                        </as:PlaceHolder>
                    </div>
                    <div id="Div1" class="height-8" style="display: none;"></div>

                    <!-- PCI Admin -->
                    <div class="reset-line-height">
                        <as:PlaceHolder ID="uxPlcPCIRole" runat="server" Visible="true">
                            <div style="display: none;">
                                <div id="uxheight1" runat="server" class="height-8"></div>
                            </div>

                            <table class="w-75 font-arial font-12" id="uxPlcPCIRole_table" runat="server">
                                <tr>
                                    <td class="text-left w-40">
                                        <label class="dark-blue">
                                            <b>
                                                <asp:Literal ID="Literal5" runat="server" Text="PCI Admin:" /></b></label>
                                    </td>
                                    <td>
                                        <asp:Literal ID="lbPCIAdmin" runat="server" Text="Admin" />
                                    </td>
                                </tr>
                            </table>
                        </as:PlaceHolder>
                    </div>
                </div>
            </div>
        </div>
        <as:Panel runat="server" ID="uxPlAccessFunction">
            <div class="row row-table">
                <div class="col-xs-12">
                    <h4>
                        <asp:Literal ID="Literal6" runat="server" Text="Access Functions" meta:resourcekey="LiteralResource4" />
                    </h4>
                </div>
            </div>
            <div class="box ptb-0 fixed-height-block height-150">
                <div id="clientAccessFunction" class="pb-10 pt-9 treeview-check-icon">
                    <div class="treeview treeview-disabled">
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
                                        <as:CheckBox ID="uxPermission" runat="server" AutoPostBack="false" Enabled="false" />
                                    </div>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                    <%--<asp:Label ID="uxLnkAssignActivityGroups" Text="Task Permissions" runat="server" meta:resourcekey="uxLnkAssignActivityGroupsResource1"></asp:Label>--%>
                </div>
            </div>
        </as:Panel>
        <as:PlaceHolder ID="PlaceHolder1" runat="server">
            <table class="w-100">
                <tr>
                    <td>
                        <h4>
                            <asp:Literal ID="Literal10" runat="server" Text="Default Landing Page:" meta:resourcekey="ManageRole_ascxDefaultLandingPage" />
                        </h4>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="uxDefaultLandingPage" runat="server"></asp:Label>
                    </td>
                </tr>
            </table>
        </as:PlaceHolder>
        <div class="row">
            <div class="col-md-12 form-action-container text-right">
                <div class="inline-block">
                    <as:Button runat="server" class="btn btn-default" ID="Button1" Text="Close" OnClientClick="return parent.HidePopupModal();" IsStandardButton="False" meta:resourcekey="uxClose" />
                </div>
            </div>
        </div>
    </as:ASModalContainer>
    <tek:RadCodeBlock ID="radCodeBlock" runat="server">
        <script>
            var ux1099KRole_ClientID = '<%= lb1099K.ClientID %>';
            var uxPlcPCIRole_table_ClientID = '<%= uxPlcPCIRole_table.ClientID %>';
        </script>
        <script type="text/javascript" src="<%= ResolveUrl("~/") %>res/js/usermaintenance/ViewRoleInfo.js"></script>
        <%--<script type="text/javascript" src="<%= ResolveUrl("~/") %>res/js/usermaintenance/AccessFuntionControl.js"></script>--%>
    </tek:RadCodeBlock>
</asp:Content>
