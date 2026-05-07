<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="ManageASUsers.aspx.cs" Inherits="ManageASUsers" ValidateRequest="false" Title="Manage Users" meta:resourcekey="PageResource1" %>

<%@ Register TagName="UxExport" Src="~/UserControls/UxExport.ascx" TagPrefix="uc" %>
<%@ Register TagName="PageTitle" Src="~/UserControls/PageTitle.ascx" TagPrefix="uc" %>
<%@ Register TagName="ManageASUserFiltering" Src="~/UserControls/ManageASUserFiltering.ascx"
    TagPrefix="uc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPage" runat="Server">
    <tek:RadAjaxManagerProxy ID="RadAjaxManagerProxy1" runat="server">
        <AjaxSettings>
            <tek:AjaxSetting AjaxControlID="uxReportGrid">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="uxReportGrid" />
                </UpdatedControls>
            </tek:AjaxSetting>
        </AjaxSettings>
    </tek:RadAjaxManagerProxy>
    <!-- Filtering  -->
    <asp:PlaceHolder ID="uxPlaceHolderReportFilter" runat="server">
        <uc:ManageASUserFiltering ID="uxReportFiltering" runat="server" />
    </asp:PlaceHolder>
    <uc:PageTitle ID="uxReportTitle" runat="server" ReportTitle="Manage Aperia Users" meta:resourcekey="uxReportTitleResource1" />
    <asp:HyperLink ID="uxbtnCreateUser" Style="cursor: pointer" runat="server" CssClass="btn btn-default" Text="Create User" meta:resourcekey="Literal1Resource1" NavigateUrl="CreateNewASUser.aspx">
    </asp:HyperLink>
    <br />
    <br />
    <div class="row radio-button-list dark-blue mb-4x">
        <div class="col-md-12">
            <div class="control-inline">
                <label class="first">
                    <asp:Literal ID="uxStatusText" runat="server" Text="Status:" meta:resourcekey="uxStatusTextResource1"></asp:Literal>
                </label>
            </div>
            <div class="control-inline">
                <asp:RadioButton ID="chbxActive" runat="server" GroupName="StatusGroup" OnCheckedChanged="chbxActive_CheckedChanged" Text="Active" AutoPostBack="true" Checked="true" meta:resourcekey="chbxActiveResource" />
            </div>
            <div class="control-inline">
                <asp:RadioButton ID="chbxInActive" runat="server" GroupName="StatusGroup" OnCheckedChanged="chbxActive_CheckedChanged" Text="InActive" AutoPostBack="true" meta:resourcekey="chbxInActiveResource" />
            </div>
            <div class="control-inline">
                <asp:RadioButton ID="chbxAll" runat="server" GroupName="StatusGroup" OnCheckedChanged="chbxActive_CheckedChanged" Text="All" AutoPostBack="true" meta:resourcekey="chbxAllResource" />
            </div>
        </div>
    </div>
    <!--Rad Grid-->
    <uc:UxExport ID="uxExporter" runat="server" GridHeader="User List" IsOnTop="true" GridID="uxReportGrid" meta:resourcekey="uxExporterResource1" />
    <as:ASGrid ID="uxReportGrid" runat="server" GridLines="None" Width="100%" AllowPaging="True"
        AllowSorting="True" AutoGenerateColumns="false" AllowSortFilterWhenExport="true" BorderWidth="1"
        IntruderSourceName="uxReportGrid" IsIntruder="true" ASPagingMethod="None" ShowFooter="false" AllowFilteringByColumn="true"
        OnItemCommand="uxReportGrid_ItemCommand" CssClass="in" meta:resourcekey="uxReportGridResource1">
        <MasterTableView DataKeyNames="UserID">
            <Columns>
                <as:ASGridBoundColumn HeaderText="Username" DataField="UserID" UniqueName="UserID" AllowFiltering="true"
                    SortExpression="UserID" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left"
                    HeaderTooltip="User Name" meta:resourcekey="ASGridBoundColumnResource1" HeaderStyle-Width="120px">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn HeaderText="First Name" DataField="UserNameFirst" UniqueName="UserNameFirst" AllowFiltering="true"
                    SortExpression="UserNameFirst" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="center"
                    HeaderTooltip="First Name" AllowEncodeOnExporting="true" meta:resourcekey="ASGridBoundColumnResource2">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn HeaderText="Last Name" DataField="UserNameLast" UniqueName="UserNameLast" AllowFiltering="true"
                    SortExpression="UserNameLast" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="center"
                    HeaderTooltip="Last Name" AllowEncodeOnExporting="true" meta:resourcekey="ASGridBoundColumnResource3">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn HeaderText="Email" DataField="Email" UniqueName="Email" HeaderStyle-HorizontalAlign="Center"
                    ItemStyle-HorizontalAlign="center" HeaderTooltip="Email address" SortExpression="Email" AllowFiltering="true"
                    AllowEncodeOnExporting="true" meta:resourcekey="ASGridBoundColumnResource4">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn HeaderText="Status" HeaderStyle-HorizontalAlign="Center" DataField="ActiveStatus" HeaderStyle-Width="100px" AllowFiltering="true" UniqueName="ActiveStatus"
                    ItemStyle-HorizontalAlign="center" HeaderTooltip="Active Status" SortExpression="ActiveStatus" meta:resourcekey="ASGridTemplateColumnResource1">
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                </as:ASGridBoundColumn>
                <as:ASGridTemplateColumn HeaderText="Actions" HeaderStyle-HorizontalAlign="Center" DataField="ActiveStatus" AllowFiltering="false" UniqueName="EditAction"
                    ItemStyle-HorizontalAlign="Center" HeaderTooltip="Edit" AllowSorting="false" meta:resourcekey="ASGridTemplateColumnActionResource">
                    <ItemTemplate>
                        <asp:PlaceHolder ID="pnlEditLink" runat="server" Visible="false">
                            <span style="width: 70px; display: inline-block; text-align: center;">
                                <asp:HyperLink runat="server" ID="uxEditLink" meta:resourcekey="uxEditActionResource"> </asp:HyperLink>
                            </span>
                        </asp:PlaceHolder>
                        <asp:PlaceHolder ID="pnlResetPasswordLink" runat="server" Visible="false">
                            <span style="width: 70px; display: inline-block; text-align: center;">
                                <asp:Literal runat="server" ID="uxResetPasswordLink" meta:resourcekey="ResetPasswordResource" />
                            </span>
                        </asp:PlaceHolder>
                        <asp:PlaceHolder ID="pnlActiveStatus" runat="server" Visible="false">
                            <span style="width: 70px; display: inline-block; text-align: center;">
                                <asp:LinkButton runat="server" ID="uxActiveStatus" CommandName="Active" meta:resourcekey="ActiveStatusResource" />
                            </span>
                        </asp:PlaceHolder>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                </as:ASGridTemplateColumn>
            </Columns>
        </MasterTableView>
    </as:ASGrid>
    <div style="display: none;">
        <asp:Button ID="uxReload" runat="server" Text="Reload" OnClick="uxReload_Click" meta:resourcekey="uxReloadResource1" />
        <tek:RadComboBox runat="server" />
    </div>
    <tek:RadCodeBlock ID="RadCodeBlock1" runat="server">

        <script type="text/javascript">
            function doRebindUserList() {
                var btnSearch = document.getElementById("<%=uxReload.ClientID %>");
                if (btnSearch) {
                    btnSearch.click();
                }
            }
            function validateActive(controlid) {
                HidePopupModal();
                __doPostBack(controlid, '');
            }
        </script>

    </tek:RadCodeBlock>
</asp:Content>
