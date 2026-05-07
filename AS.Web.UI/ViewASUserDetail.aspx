<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ViewASUserDetail.aspx.cs" Inherits="ViewASUserDetail" %>

<%@ Register TagName="PageTitle" Src="~/UserControls/PageTitle.ascx" TagPrefix="uc" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="Server">
    <uc:PageTitle ID="uxReportTitle" runat="server" HasFilteringOption="false" ReportTitle="Aperia User Details" meta:resourcekey="uxReportTitleResource1" />
    <div class="row">
        <div class="col-md-12">
            <div id="pnlbtnEditUser" class="control-inline" runat="server">
                <asp:HyperLink ID="btnEditUser" CssClass="btn btn-default" runat="server" Text="Edit" meta:resourcekey="uxEditResource" />
            </div>
            <div id="pnlbtnDeleteUser" class="control-inline" runat="server">
                <asp:LinkButton ID="btnDeleteUser" CssClass="btn btn-default" runat="server" Text="Delete" meta:resourcekey="uxDeleteResource" />
                <asp:Button ID="btnDodeleteUser" OnClick="btnDodeleteUser_Click" runat="server" CssClass="hide" Text="DoDelete" />
            </div>
        </div>
        <div class="clearfix"></div>
        <br />
        <div class="height-22"></div>
        <div class="col-md-12">
            <h3 class="modal-title">User Information</h3>
        </div>
        <div class="col-md-12">
            <table class="ASTable form-inline">
                <tbody id="tblManageUserMain">
                    <tr class="Row">
                        <td class="heading" style="width: 20%;">
                            <asp:Label ID="txtUsernameLabel" runat="server" Text="User Name:" CssClass="control-label" meta:resourcekey="ManageASUserASCX_Text_UserName" />
                        </td>
                        <td>
                            <asp:Literal ID="uxUsername" runat="server" />
                        </td>
                    </tr>
                    <tr class="AltRow">
                        <td class="heading">
                            <asp:Label ID="Label1" runat="server" Text="First Name:" CssClass="control-label" meta:resourcekey="ManageASUserASCX_Text_FirstName" />

                        </td>
                        <td>
                            <asp:Literal ID="uxFirstName" runat="server" />
                        </td>
                    </tr>
                    <tr class="Row">
                        <td class="heading">
                            <asp:Label ID="Label2" runat="server" Text="Last Name:" CssClass="control-label" meta:resourcekey="ManageASUserASCX_Text_LastName" />
                        </td>
                        <td>
                            <asp:Literal ID="uxLastName" runat="server" />

                        </td>
                    </tr>
                    <tr class="AltRow">
                        <td class="heading">
                            <asp:Label ID="Label3" runat="server" Text="Email:" CssClass="control-label" meta:resourcekey="ManageASUserASCX_Text_Email" />
                        </td>
                        <td>
                            <asp:Literal ID="uxEmail" runat="server" />

                        </td>
                    </tr>

                    <tr class="Row">
                        <td class="heading">
                            <asp:Label ID="Label4" runat="server" Text="Role:" CssClass="control-label" meta:resourcekey="ManageASUserASCX_Text_Role" />
                        </td>
                        <td>
                            <as:Literal ID="uxRole" runat="server" Text="System Administrator" />

                        </td>
                    </tr>
                    <tr class="AltRow">
                        <td class="heading">
                            <asp:Literal ID="Literal1" runat="server" Text="Status:" />
                        </td>
                        <td>
                            <asp:Literal ID="uxStatus" runat="server" />
                        </td>
                    </tr>
            </table>
        </div>
    </div>
    <div class="height-22"></div>
    <div class="row">
        <div class="col-md-12">
            <h3 class="modal-title">Access Functions </h3>
            <div class="box ">
                <as:CheckBoxList ID="uxAccessFunctionPermissions" Enabled="false" CssClass="checkbox-list-check-icon checkbox-list-span" runat="server" AutoPostBack="false"
                    DataTextField="Description" DataValueField="PermissionId" meta:resourcekey="uxAccessListFunctionResource1">
                </as:CheckBoxList>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        var ViewASUserDetail_btnDodeleteUser = '<%= btnDodeleteUser.ClientID%>';
        function DeleteUser() {
            $('#' + ViewASUserDetail_btnDodeleteUser).click();
        }
    </script>
</asp:Content>

