<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ManageASUserFiltering.ascx.cs" Inherits="UserControls_ManageASUserFiltering" %>
<div class="row collapse report-filter-panel">
    <div class="col-md-12 report-filter">
        <div class="filter-block" runat="server" id="uxFilteringTable">
            <table>
                <colgroup>
                    <col />
                    <col />
                    <col style="width: 30px;" />
                </colgroup>
             
                <tr>
                    <td class="text-right">
                        <div class="filter-item">
                            <asp:Literal ID="Literal1" runat="server" Text="Search Criteria:" meta:resourcekey="LiteralResource1" />
                        </div>
                    </td>
                    <td class="text-right">
                        <as:RadComboBox runat="server" Width="248px" ID="uxSearchType" CssClass="filter-item" OnClientSelectedIndexChanged="SearchTypeOnClientSelectedIndexChanged">
                            <Items>
                                <as:RadComboBoxItem Value="All" Text="ALL" meta:resourcekey="ManageUserFilteringASCX_ItemCombobox_All"/> 
                                <as:RadComboBoxItem Value="USERID" Text="Username" meta:resourcekey="ManageUserFilteringASCX_ItemCombobox_UserID"/>
                                <as:RadComboBoxItem Value="FIRSTNAME" Text="First Name" meta:resourcekey="ManageUserFilteringASCX_ItemCombobox_Firstname"/>
                                <as:RadComboBoxItem Value="LASTNAME" Text="Last Name" meta:resourcekey="ManageUserFilteringASCX_ItemCombobox_Lastname"/>
                                <as:RadComboBoxItem Value="EMAIL" Text="Email" meta:resourcekey="ManageUserFilteringASCX_ItemCombobox_Email"/>
                            </Items>
                        </as:RadComboBox>
                    </td>
                    <td></td>
                </tr>
                <tr id="uxtr" runat="server">
                    <td class="text-right" colspan="2">
                        <div id="pnlcbbSearch" style="display: none;" runat="server">
                            <as:RadComboBox runat="server" CssClass="filter-item" Width="248px" ID="uxcbbSearchValue">
                            </as:RadComboBox>
                        </div>
                        <div id="pnltxtSearchValue" style="display: none;" runat="server">
                            <div class="filter-item">
                                <as:TextBox ID="uxSearchValue" CssClass="rf_TextBox" runat="server" Width="248px" HintCss="hint" meta:resourcekey="uxSearchValueResource1" />
                            </div>
                        </div>
                        <div id="pnlcbbSearchRole" style="display: none;" runat="server">
                            <as:RadComboBox runat="server" CssClass="filter-item" Width="248px" ID="uxcbbUserRole" MaxHeight="210px"></as:RadComboBox>
                        </div>
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td></td>
                    <td class="text-right">
                        <div class="filter-item">
                            <as:Button runat="server" CssClass="btn btn-default" Text="Search" ID="uxbtnSearch" OnClientClick="return validation();" IsStandardButton="False" meta:resourcekey="uxbtnSearchResource1" />
                        </div>
                    </td>
                    <td></td>
                </tr>
            </table>

        </div>
    </div>
</div>
<!-- -->
<div class="row">
    <div class="col-md-12 text-center" data-target=".report-filter-panel" data-toggle="collapse">
        <span class="btn btn-link btn-report-filter"><asp:Literal ID="Literal2" runat="server" Text="FILTER" meta:resourcekey="LiteralResource2" /></span>
    </div>
</div>

<script type="text/javascript">

    var uxtr_ClientID = '<%= uxtr.ClientID %>';

    var pnltxtSearchValue_ClientID = "<%= pnltxtSearchValue.ClientID %>";
    var pnlcbbSearch_ClientID = "<%= pnlcbbSearch.ClientID %>";

    var pnlcbbSearch_ClientID = '<%= pnlcbbSearch.ClientID %>';
    var uxSearchValue_ClientID = '<%= uxSearchValue.ClientID %>';


    var uxFilteringTable_ClientID = '<%= uxFilteringTable.ClientID %>';
    var uxbtnSearch_ClientID = '<%= uxbtnSearch.ClientID %>';
    var uxSearchType_ClientID = '<%= uxSearchType.ClientID %>';
    var pnlcbbSearchRole_ClientID = '<%= pnlcbbSearchRole.ClientID %>';

    var Text_RequiredField = '<%=GetLocalResourceObject("ManageUserFiteringJS_Text_RequiredField").ToString()%>';
    var Text_UserID = '<%=GetLocalResourceObject("ManageUserFilteringASCX_ItemCombobox_UserID.Text").ToString()%>';
    var Text_FirstName = '<%=GetLocalResourceObject("ManageUserFilteringASCX_ItemCombobox_Firstname.Text").ToString()%>';
    var Text_LastName = '<%=GetLocalResourceObject("ManageUserFilteringASCX_ItemCombobox_Lastname.Text").ToString()%>';
    var Text_Email = '<%=GetLocalResourceObject("ManageUserFilteringASCX_ItemCombobox_Email.Text").ToString()%>';
    var funcValidate = 'validation';
</script>
<script src="<%= ResolveUrl("~/")%>res/js/usermaintenance/ManageASUserFiltering.js"></script>