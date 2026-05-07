<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MIF_MerchantDetails_MPS.ascx.cs" Inherits="UserControls_MIF_MerchantDetails_MPS" %>



<style type="text/css">
    table.MPSBorder, table.Dashboard, table.MPSBorder tr.MPSBorderAltRow > td, table.MPSBorder tr.MPSBorderRow > td, .MPSBorder th, table.Dashboard td, table.Dashboard th
    {
        border-style: none !important;
    }
</style>
<as:Panel ID="uxPnlDetail" runat="server" Visible="true">
    <div style="font-weight: bold">
        <asp:Literal ID="Literal1" runat="server" Text="Merchant Profile" />
    </div>
    <br />
    <as:Container HeaderText="Status" runat="server" ID="AsContainer4" Width="100%" TemplateName="ascontainer_whitebordernopadding.tpl" meta:resourcekey="AsContainer4Resource1">
        <table cellpadding="0" cellspacing="0" class="MPSBorder" width="100%">
            <tr class="MPSBorderRow">
                <td style="width: 20%">
                    <asp:Literal ID="Literal2" runat="server" Text="MMerchant Status:" meta:resourcekey="LiteralResource2"/>
                </td>
                <td style="width: 30%">
                    <as:Literal ID="uxAS_MerchantStatus" runat="server" />
                </td>
                <td style="width: 20%">
                    <asp:Literal ID="Literal3" runat="server" Text="MDate Opened:" meta:resourcekey="LiteralResource3"/>
                </td>
                <td style="width: 30%">
                    <as:Literal ID="uxAS_DateOpened" runat="server" />
                </td>
            </tr>
            <tr class="MPSBorderAltRow">
                <td>
                    <asp:Literal ID="Literal4" runat="server" Text="Seasonal Code:" meta:resourcekey="LiteralResource4"/>
                </td>
                <td>
                    <as:Literal ID="uxAS_SeasonalCode" runat="server" />
                </td>
                <td>
                    <asp:Literal ID="Literal5" runat="server" Text="Date Closed:" meta:resourcekey="LiteralResource5"/>
                </td>
                <td>
                    <as:Literal ID="uxAS_DateClosed" runat="server" />
                </td>
            </tr>
            <tr class="MPSBorderRow">
                <td valign="middle">
                    <asp:Literal ID="Literal6" runat="server" Text="Date Last Active:" meta:resourcekey="LiteralResource6" />
                </td>
                <td valign="middle">
                    <as:Literal ID="uxAS_DateLastActive" runat="server" />
                </td>
                <td valign="middle">
                    <asp:Literal ID="Literal7" runat="server" Text="Site Access:" meta:resourcekey="LiteralResource7"/>
                </td>
                <td>
                    <table>
                        <tr>
                            <td>
                                <as:Literal ID="uxAS_OptIn" runat="server" />
                            </td>
                            <td style="padding-left: 10px;">
                                <as:Button ID="uxAS_ButtonOpt" runat="server" Text="" Visible="false" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <as:HiddenField ID="uxHddOptOut" runat="server" />
    </as:Container>
    <br />
    <as:Container HeaderText="Business Information" runat="server" ID="Container1" Width="100%"
        TemplateName="ascontainer_whitebordernopadding.tpl" meta:resourcekey="Container1Resource1">
        <table width="100%" class="MPSBorder" cellpadding="0" cellspacing="0">
            <tr class="MPSBorderRow">
                <td valign="middle" style="width: 20%">
                    <asp:Literal ID="Literal8" runat="server" Text="Merchant ID:" meta:resourcekey="LiteralResource8"/>
                </td>
                <td valign="middle" style="width: 30%">
                    <as:Literal ID="uxMI_MerchantNumber" runat="server" />
                </td>
                <td valign="middle">
                    <asp:Literal ID="Literal9" runat="server" Text="Processor:" meta:resourcekey="LiteralResource9"/>
                </td>
                <td valign="middle">
                    <as:Literal ID="uxMI_Processor" runat="server"></as:Literal>
                </td>
            </tr>
            <tr class="MPSBorderAltRow">
                <td valign="middle">
                    <asp:Literal ID="Literal10" runat="server" Text="Merchant Name:" meta:resourcekey="LiteralResource10"/>
                </td>
                <td valign="middle">
                    <as:Literal ID="uxMI_MerchantName" runat="server" />
                </td>
                <td valign="middle" style="width: 20%">
                    <asp:Literal ID="Literal11" runat="server" Text="Owner:" meta:resourcekey="LiteralResource11"/>
                </td>
                <td valign="middle" style="width: 30%">
                    <as:Literal ID="uxOI_OwnerName" runat="server" />
                </td>
                <%--<td valign="top">
                    Phone:
                </td>
                <td valign="top">
                    <as:Literal ID="uxMI_Phone" runat="server" />
                </td>--%>
            </tr>
            <tr class="MPSBorderRow">
                <td valign="middle">
                    <asp:Literal ID="Literal12" runat="server" Text="Merchant Address:" meta:resourcekey="LiteralResource12"/>
                </td>
                <td valign="middle">
                    <as:Literal ID="uxMI_MerchantAddress1" runat="server" />
                </td>
                <td valign="middle">
                    <asp:Literal ID="Literal13" runat="server" Text="Chain:" meta:resourcekey="LiteralResource13"/>
                </td>
                <td valign="middle">
                    <as:PlaceHolder ID="uxHChain" runat="server">
                        <a href="#" id="uxLinkChain" runat="server">
                            <as:Literal ID="uxHI_Chain" runat="server" /></a>
                    </as:PlaceHolder>
                    <as:Literal ID="uxLHChain" runat="server" />
                </td>
            </tr>
            <tr class="MPSBorderAltRow">
                <td valign="middle">
                    &nbsp;
                </td>
                <td valign="middle">
                    <as:Literal ID="uxMI_MerchantAddress2" runat="server" />
                </td>
                <td valign="middle">
                    <asp:Literal ID="Literal14" runat="server" Text="Phone:" meta:resourcekey="LiteralResource14"/>
                </td>
                <td valign="middle">
                    <as:Literal ID="uxMI_Phone" runat="server" />
                </td>
            </tr>
            <tr class="MPSBorderRow">
                <td valign="middle">
                    &nbsp;
                </td>
                <td valign="middle">
                    <as:Literal ID="uxMI_MerchantCityStateZip" runat="server" />
                </td>
                <td valign="middle">
                    <asp:Literal ID="Literal15" runat="server" Text="Fax:" meta:resourcekey="LiteralResource15"/>
                </td>
                <td valign="middle">
                    <as:Literal ID="uxMI_Fax" runat="server" />
                </td>
            </tr>
            <tr class="MPSBorderAltRow">
                <td valign="middle">
                    <asp:Literal ID="Literal16" runat="server" Text="Billing Address:" meta:resourcekey="LiteralResource16"/>
                </td>
                <td valign="middle">
                    <as:Literal ID="uxMI_CooperateName" runat="server" />
                </td>
                <td valign="middle">
                    <asp:Literal ID="Literal17" runat="server" Text="1st Deposit Date:" meta:resourcekey="LiteralResource17"/>
                </td>
                <td valign="middle">
                    <as:Literal ID="uxMI_1stDepositDate" runat="server" />
                </td>
            </tr>
            <tr class="MPSBorderRow">
                <td valign="middle">
                    &nbsp;
                </td>
                <td valign="middle">
                    <as:Literal ID="uxMI_CooperateAddressLine" runat="server" />
                </td>
                <td valign="middle">
                    <asp:Literal ID="Literal18" runat="server" Text="Federal Tax ID:" meta:resourcekey="LiteralResource18"/>
                </td>
                <td valign="middle">
                    <as:Literal ID="uxMI_TaxID" runat="server" />
                </td>
            </tr>
            <tr class="MPSBorderAltRow">
                <td valign="middle">
                    &nbsp;
                </td>
                <td valign="middle">
                    <as:Literal ID="uxMI_CooperateCityStateZip" runat="server" />
                </td>
                <td valign="middle">
                    <asp:Literal ID="Literal19" runat="server" Text="SIC/MCC Code:" meta:resourcekey="LiteralResource19"/>
                </td>
                <td valign="middle">
                    <as:Literal ID="uxMI_SICMCCCode" runat="server" />
                </td>
            </tr>
            
            <tr class="MPSBorderRow">
                <td valign="middle">
                    &nbsp;
                </td>
                <td valign="middle">
                    <as:Literal ID="Literal20" runat="server" />
                </td>
                <td valign="middle">
                    <asp:Literal ID="Literal21" runat="server" Text="Market Data:" meta:resourcekey="LiteralResource20"/>
                </td>
                <td valign="middle">
                    <as:Literal ID="uxMarketData" runat="server" />
                </td>
            </tr>
            
            <tr class="MPSBorderAltRow" runat="server" id="pnlRelationshipmanager">
                <td valign="top" class="bold_text right">
                    <as:Panel runat="server" ID="uxpnlRMLabel" Style="margin-top: 2px;">
                        <asp:Literal ID="Literal22" runat="server" Text="Relationship Manager:" meta:resourcekey="LiteralResource21"/>
                    </as:Panel>
                </td>
                <td valign="top">
                    <as:Panel runat="server" ID="uxpnlRMCtrls" Style="margin-top: 2px;">
                        <as:Literal runat="server" ID="uxlbRelationshipManager"></as:Literal>
                        <as:LinkButton ID="uxbtnAdd" OnClick="uxbtnAdd_click" Text="Add RM" runat="server" meta:resourcekey="LiteralResource22"/>
                        <as:LinkButton ID="uxbtnEdit" OnClick="uxbtnEdit_click" runat="server" Text="Edit" meta:resourcekey="LiteralResource23"/>
                        <as:Panel ID="uxpnlSaveCancel" runat="server" Visible="false">
                            <as:TextBox runat="server" ID="uxtxtRelationShipManager" MaxLength="30" />
                            <as:LinkButton ID="uxbtnSave" OnClick="uxbtnSave_click" OnClientClick="return CheckPrefix();"
                                runat="server" Text="Save" meta:resourcekey="LiteralResource28"/>
                            /
                            <as:LinkButton ID="uxbtnCancel" OnClick="uxbtnCancel_click" runat="server" Text="Cancel" meta:resourcekey="LiteralResource4"/>

                            <script type="text/javascript">
                                var Text_AtLeast30NoSpecialChars = '<%=GetLocalResourceObject("MIF_MerchantDetails_MPSJS_Text_AtLeast30NoSpecialChars").ToString()%>';
                                var Text_UnallowPrefixSuffix = '<%=GetLocalResourceObject("MIF_MerchantDetail_MPSJS_Text_UnallowPrefixSuffix").ToString()%>';

                                function CheckPrefix() {
                                    var regName1 = /^(mr|ms|sir|jr|mrs)+[\s+.]+(\w+|\s)/i;
                                    var regName2 = /^[A-Za-z0-9\s\.\-]+[\s|.]+(mr|ms|sir|jr|mrs|mr.|ms.|sir.|jr.|mrs.|mr\s+.|ms\s+.|sir\s+.|jr\s+.|mrs\s+.)$/i;
                                    var regName3 = /^(mr|ms|sir|jr|mrs|mr.|ms.|sir.|jr.|mrs.|mr\s+.|ms\s+.|sir\s+.|jr\s+.|mrs\s+.)/i;
                                    var reg2 = /^[A-Za-z0-9\-\s+.]*$/;
                                    if (!regName1.test($.trim($('#<%= uxtxtRelationShipManager.ClientID %>').val().toLowerCase())) && !regName2.test($.trim($('#<%= uxtxtRelationShipManager.ClientID %>').val().toLowerCase())) && !regName3.test($.trim($('#<%= uxtxtRelationShipManager.ClientID %>').val().toLowerCase()))) {
                                        if (reg2.test($.trim($('#<%= uxtxtRelationShipManager.ClientID %>').val()))) {
                                            return true;
                                        }
                                        else {
                                            alert(Text_AtLeast30NoSpecialChars);
                                            return false;

                                        }
                                    }
                                    else
                                        alert(Text_UnallowPrefixSuffix);
                                    return false;
                                }
                            </script>

                        </as:Panel>
                    </as:Panel>
                </td>
                <td valign="middle">
                </td>
                <td valign="middle">
                </td>
            </tr>
        </table>
    </as:Container>
    <br />
    <table id="uxMerchantBankInformation" style="width: 50%;" cellpadding="0" cellspacing="0">
        <tr>
            <%--<td valign="top" style="width: 48%">
                <as:Container HeaderText="Hierarchy Information" runat="server" ID="Container2" Width="100%"
                    TemplateName="ascontainer_whitebordernopadding.tpl">
                    <table cellpadding="0" cellspacing="0" width="100%" class="MPSBorder">                        
                        <tr class="rgRow">
                            <td style="width: 42%">
                                Corporation:
                            </td>
                            <td style="width: 58%">
                                <as:Literal ID="uxHI_Corporation" runat="server" />
                            </td>
                        </tr>
                        <tr class="rgAltRow">
                            <td>
                                Region:
                            </td>
                            <td>
                                <as:Literal ID="uxHI_Region" runat="server" />
                            </td>
                        </tr>
                        <tr class="rgRow">
                            <td>
                                Principal:
                            </td>
                            <td>
                                <as:Literal ID="uxHI_Pricipal" runat="server" />
                            </td>
                        </tr>
                        <tr class="rgAltRow">
                            <td>
                                Associate:
                            </td>
                            <td>
                                <as:Literal ID="uxHI_Associate" runat="server" />
                            </td>
                        </tr>
                        <tr class="rgRow">
                            <td>
                                Chain:
                            </td>
                            <td>
                                <as:Literal ID="uxHI_Chain" runat="server" />
                            </td>
                        </tr>
                    </table>
                </as:Container>
            </td>
            <td>
                &nbsp;
            </td>--%>
            <td valign="top" style="width: 50%">
                <as:Container HeaderText="Bank Information" runat="server" ID="Container3" Width="100%"
                    TemplateName="ascontainer_whitebordernopadding.tpl" meta:resourcekey="Container3Resource1">
                    <table cellpadding="0" cellspacing="0" width="100%" class="MPSBorder">
                        <tr class="MPSBorderRow">
                            <td style="width: 40%">
                                <asp:Literal ID="Literal23" runat="server" Text="Bank Name:" meta:resourcekey="LiteralResource25"/>
                            </td>
                            <td style="width: 60%">
                                <as:Literal ID="uxBankName" runat="server" />
                            </td>
                        </tr>
                        <tr class="MPSBorderAltRow">
                            <td>
                                <asp:Literal ID="Literal24" runat="server" Text="Routing #:" meta:resourcekey="LiteralResource26"/>
                            </td>
                            <td>
                                <as:Literal ID="uxRoutingNumber" runat="server" />
                            </td>
                        </tr>
                        <tr class="MPSBorderRow">
                            <td>
                                <asp:Literal ID="Literal25" runat="server" Text="DDA #:" meta:resourcekey="LiteralResource27"/>
                            </td>
                            <td>
                                <as:Literal ID="uxDDANumber" runat="server" />
                            </td>
                        </tr>
                    </table>
                </as:Container>
            </td>
        </tr>
    </table>
    <br />
</as:Panel>
<as:Panel ID="uxNorecords" runat="server" Visible="False">
    <asp:Literal ID="Literal26" runat="server" Text="No data found." meta:resourcekey="LiteralResource1" />
</as:Panel>
