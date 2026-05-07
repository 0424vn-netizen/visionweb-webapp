<%@ Control Language="C#" AutoEventWireup="true" CodeFile="IssuingBank.ascx.cs" Inherits="UserControls_IssuingBank" %>
<as:Literal ID="uxMessage" Text="No data found." EnableViewState="False" Visible="False" runat="server" meta:resourcekey="uxMessageResource1"></as:Literal>

<div class="issuing-bank">
    <div class="RadGrid RadGrid_Default" runat="server" id="ux4Report" enableviewstate="False" visible="False">
        <div align="left" class="binnumber">
            <label runat="server" class="binnumber"><%=GetLocalResourceObject("BankIdentification")%></label>
            - <%=BinNumberText %>
        </div>
        <div>
            <asp:Repeater ID="uxIssuingBank4" runat="server" EnableViewState="False">
                <ItemTemplate>
                    <div class="row mt-10">
                        <div class="col-md-6">
                            <table class="table1">
                                <colgroup>
                                    <col style="width: 17%" />
                                    <col style="width: 33%" />
                                    <col style="width: 20%" />
                                    <col style="width: 30%" />
                                </colgroup>
                                <tr>
                                    <td colspan="4" class="data IssuingBankAndCardInformation">
                                        <asp:Literal ID="Literal2" runat="server" Text="Issuing Bank" meta:resourcekey="IssuingBank" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="data2">
                                        <asp:Literal ID="Literal1" runat="server" Text="Name:" meta:resourcekey="NameOfBank" />
                                    </td>
                                    <td class="data"><%# string.IsNullOrEmpty(Eval("IssuingBank").ToString().Trim()) ? WebSiteConstants.HTML_EM_DASH_ENCODE : Eval("IssuingBank")%></td>
                                    <td class="data2">
                                        <asp:Literal ID="Literal5" runat="server" Text="Website:" meta:resourcekey="WebsiteOfBank" />
                                    </td>
                                    <td class="data">
                                        <%# string.IsNullOrEmpty(Eval("Website").ToString().Trim()) ? 
                                                             WebSiteConstants.HTML_EM_DASH_ENCODE :
                                                             string.Format("<a target='_blank' class='lnk-bin' rel='noopener noreferer' href='{0}'>{1}</a>",Eval("Website"),Eval("Website"))%>

                                    </td>
                                </tr>
                                <tr>
                                    <td class="data2">
                                        <asp:Literal ID="Literal3" runat="server" Text="Country:" meta:resourcekey="CountryOfBank" /></td>
                                    <td class="data"><%# string.IsNullOrEmpty(Eval("IssuingCountryIsoName").ToString().Trim()) ? WebSiteConstants.HTML_EM_DASH_ENCODE : Eval("IssuingCountryIsoName")%></td>
                                    <td class="data2">
                                        <asp:Literal ID="Literal11" runat="server" Text="Country:" meta:resourcekey="BINType" />
                                    </td>
                                    <td class="data">
                                        <%# string.IsNullOrEmpty(Eval("BINType").ToString().Trim()) ? WebSiteConstants.HTML_EM_DASH_ENCODE : Eval("BINType")%>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="data2">
                                        <asp:Literal ID="Literal4" runat="server" Text="Phone Number:" meta:resourcekey="PhoneOfBank" /></td>
                                    <td class="data"><%# string.IsNullOrEmpty(Eval("Phone").ToString().Trim()) ? WebSiteConstants.HTML_EM_DASH_ENCODE : Eval("Phone")%></td>
                                    <td class="data2">
                                        <asp:Literal ID="Literal12" runat="server" Text="Country:" meta:resourcekey="BINRegulated" />
                                    </td>
                                    <td class="data">
                                       <%# string.IsNullOrEmpty(Eval("BINRegulated").ToString().Trim()) ? WebSiteConstants.HTML_EM_DASH_ENCODE : Eval("BINRegulated")%>
                                    </td>
                                </tr>

                            </table>
                        </div>
                        <div class="col-md-6">
                            <table class="table1">
                                <colgroup>
                                    <col style="width: 30%" />
                                    <col style="width: 20%" />
                                    <col style="width: 30%" />
                                    <col style="width: 20%" />
                                </colgroup>
                                <tr>
                                    <td colspan="4" class="data IssuingBankAndCardInformation">
                                        <asp:Literal ID="Literal6" runat="server" Text="Card Information" meta:resourcekey="CardInformation" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="data2 ">
                                        <asp:Literal ID="Literal7" runat="server" Text="Brand:" meta:resourcekey="BrandOfBank" /></td>
                                    <td class="data"><%# string.IsNullOrEmpty(Eval("CardBrand").ToString().Trim()) ? WebSiteConstants.HTML_EM_DASH_ENCODE : Eval("CardBrand")%></td>
                                    <td class="data2">
                                        <asp:Literal ID="Literal9" runat="server" Text="Foreign Card:" meta:resourcekey="ForeignCard" /></td>
                                    <td class="data"><%# string.IsNullOrEmpty(Eval("ForeignCardIndicator").ToString().Trim()) ? WebSiteConstants.HTML_EM_DASH_ENCODE : Eval("ForeignCardIndicator")%></td>
                                </tr>
                                <tr>
                                    <td class="data2">
                                        <asp:Literal ID="Literal8" runat="server" Text="Type:" meta:resourcekey="TypeOfCard" /></td>
                                    <td class="data"><%# string.IsNullOrEmpty(Eval("TypeCard").ToString().Trim()) ? WebSiteConstants.HTML_EM_DASH_ENCODE : Eval("TypeCard")%></td>
                                    <td class="data2">
                                        <asp:Literal ID="Literal13" runat="server" Text="Country:" meta:resourcekey="MaxPANLength" />
                                    </td>
                                    <td class="data">
                                        <%# string.IsNullOrEmpty(Eval("MaxPANLength").ToString().Trim()) ? WebSiteConstants.HTML_EM_DASH_ENCODE : Eval("MaxPANLength")%>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="data2" runat="server">
                                        <asp:Literal ID="Literal10" runat="server" Text="Category:" meta:resourcekey="Category" />                                        
                                    </td>
                                    <td class="data"><%# string.IsNullOrEmpty(Eval("CategoryCard").ToString().Trim()) ? WebSiteConstants.HTML_EM_DASH_ENCODE : Eval("CategoryCard")%></td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>
    </div>
</div>
