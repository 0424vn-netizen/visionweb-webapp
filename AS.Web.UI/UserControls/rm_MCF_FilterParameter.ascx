<%@ Control Language="C#" AutoEventWireup="true" CodeFile="rm_MCF_FilterParameter.ascx.cs" Inherits="UserControls_rm_MCF_FilterParameter" %>


<div class="row">
    <div class="col-md-12 on-top">
        <table class="ASTable">
            <tr>
                <th>
                   <as:CheckBox ID="uxCheckAll" runat="server" Visible="False" meta:resourcekey="uxCheckAllResource1"/>
                </th>
                <th>
                    <as:Literal ID="ltSharp" runat="server" Text="#" meta:resourcekey="ltSharpResource1"></as:Literal>
                </th>
                <th style="width:200px">
                    <as:Literal ID="Literal1" runat="server" Text="Parameter" meta:resourcekey="Literal1Resource1"></as:Literal>
                </th>

                 <th>
                    <as:Literal ID="Literal3" runat="server" Text="Parameter Description" meta:resourcekey="Literal3Resource1"></as:Literal>
                </th>
            </tr>
            <asp:Repeater ID="uxParameterRepeater" runat="server" OnItemDataBound="rptParameterRepeater_ItemDataBound">
                <ItemTemplate>
                    <asp:PlaceHolder runat="server" ID="uxSpecialPanel" Visible="False">
                        <tr class="section-heading">
                            <td colspan="4" class="text-center">
                                <as:Literal ID="ltGroupName" runat="server" meta:resourcekey="ltGroupNameResource2"/>
                            </td>
                        </tr>
                    </asp:PlaceHolder>
                    <tr id="paramAssignment" runat="server" class="Row">
                        <td class="text-center" runat="server">
                            <as:CheckBox ID="checkBoxID" runat="server"/>
                            <as:HiddenField ID="hdParameterKey" runat="server" />
                        </td>
                        <td class="text-center" runat="server">
                            <as:Literal ID="ltIndex" runat="server"/>
                        </td>
                        <td runat="server">
                            <as:Literal ID="ltParameterName" runat="server"/>
                        </td>

                        <td runat="server">
                            <as:Literal ID="ltParameterDescription" runat="server"/>
                        </td>
                    </tr>
                </ItemTemplate>
                <AlternatingItemTemplate>
                    <asp:PlaceHolder runat="server" ID="uxSpecialPanel" Visible="False">
                        <tr class="section-heading">
                            <td colspan="4" class="text-center">
                                <asp:Literal ID="ltGroupName" runat="server" meta:resourcekey="ltGroupNameResource1"/>
                            </td>
                        </tr>
                    </asp:PlaceHolder>
                    <tr id="paramAssignment" runat="server" class="AltRow">
                        <td class="text-center" runat="server">
                            <asp:CheckBox ID="checkBoxID" runat="server"/>
                            <asp:HiddenField ID="hdParameterKey" runat="server" />
                        </td>
                        <td class="text-center" runat="server">
                            <asp:Literal ID="ltIndex" runat="server"/>
                        </td>
                        <td runat="server">
                            <asp:Literal ID="ltParameterName" runat="server"/>
                        </td>
                          <td runat="server">
                            <as:Literal ID="ltParameterDescription" runat="server"/>
                        </td>
                    </tr>
                </AlternatingItemTemplate>
            </asp:Repeater>
        </table>
        <asp:Panel runat="server" ID="pnlMessage" Visible="False" meta:resourcekey="pnlMessageResource1">
            <table class="ASTable">
            <tr><td><asp:Literal ID="uxMessage" runat="server" Visible="False" meta:resourcekey="uxMessageResource1"/></td></tr>
        </table>
        </asp:Panel>
    </div>
</div>

<asp:HiddenField ID="hdParamSelected" runat="server" />
<asp:HiddenField ID="previousGroupName" runat="server" />

<as:ASRadCodeBlock ID="radCodeBlock" runat="server">
    <script type="text/javascript">
        var ParameterKeyList = "";

        var Assignment_Filter_Parameters_hdParamSelected = '<%=hdParamSelected.ClientID %>';
        var Assignment_Filter_Parameters_uxCheckAll = '<%=uxCheckAll.ClientID %>';
       
    </script>
    <script src="<%= ResolveUrl("~/")%>res/js/risk_MCF/rm_MCF_FilterParameter.js"></script>
</as:ASRadCodeBlock>
