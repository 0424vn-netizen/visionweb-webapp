<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MIF_MerchantDetails_TSYS.ascx.cs"
    Inherits="UserControls_MIF_MerchantDetails_TSYS" %>
<%@ Register TagPrefix="uc" TagName="RiskInfo" Src="~/UserControls/MIF_RiskInformationSection.ascx" %>

<div id="toolTipLayer" style="position: absolute; left: 0; right: 0; color: Black;">
</div>
<as:RadCodeBlock ID="ScriptManagement" runat="server">

    <script type="text/javascript" language="javascript">
        function doOutInClick(url, width, height) {
            ShowPopupModal(url, width, height);
            return false;
        }

        function limitChars(textobj, limit, infodivobj) {
            var text = textobj.val();
            var textlength = text.length;
            if (textlength > limit) {
                infodivobj.html('0');
                textobj.val(text.substr(0, limit));
                alert(String.format('Comment: ' + '<%=Resources.MessageManager.Generic_FieldLengthExceeded%>', 1000));
                return false;
            }
            else {
                infodivobj.html(limit - textlength);
                return true;
            }
        }
        function isValidEmailAddress(email) {
            if (email.length != 0) {
                var reg = /^([A-Za-z0-9_\-\.])+\@([A-Za-z0-9_\-\.])+\.([A-Za-z]{2,6})$/;
                return reg.test(email);
            }
            else {
                return false;
            }
        }

        //browser detection
        var agt = navigator.userAgent.toLowerCase();
        var is_major = parseInt(navigator.appVersion);
        var is_minor = parseFloat(navigator.appVersion);

        var is_nav = ((agt.indexOf('mozilla') != -1) && (agt.indexOf('spoofer') == -1)
                && (agt.indexOf('compatible') == -1) && (agt.indexOf('opera') == -1)
                && (agt.indexOf('webtv') == -1) && (agt.indexOf('hotjava') == -1));
        var is_nav4 = (is_nav && (is_major == 4));
        var is_nav6 = (is_nav && (is_major == 5));
        var is_nav6up = (is_nav && (is_major >= 5));
        var is_ie = ((agt.indexOf("msie") != -1) && (agt.indexOf("opera") == -1));

        //tooltip Position
        var offsetX = 0;
        var offsetY = 5;
        var opacity = 100;
        var toolTipSTYLE;
        initToolTips();
        function initToolTips() {
            if (document.getElementById) {
                toolTipSTYLE = document.getElementById("toolTipLayer").style;
            }
            if (is_ie || is_nav6up) {
                toolTipSTYLE.visibility = "visible";
                toolTipSTYLE.display = "none";
                document.onmousemove = moveToMousePos;
            }
        }
        function moveToMousePos(e) {
            if (!is_ie) {
                x = e.pageX;
                y = e.pageY;
            } else {
                x = event.x + document.body.scrollLeft;
                y = event.y + document.body.scrollTop;
            }

            if (x >= 1220) {
                toolTipSTYLE.left = (x - 100) + offsetX + 'px';
                toolTipSTYLE.top = y + offsetY + 'px';
            }
            else {
                toolTipSTYLE.left = x + offsetX + 'px';
                toolTipSTYLE.top = y + offsetY + 'px';
            }
            return true;
        }


        function toolTip(msg) {
            if (toolTip.arguments.length < 1) // if no arguments are passed then hide the tootip
            {
                if (is_nav4)
                    toolTipSTYLE.visibility = "hidden";
                else
                    toolTipSTYLE.display = "none";
            }
            else // show
            {
                fg = "black";
                bg = "white";
                var content = '<table border="0" cellspacing="0" cellpadding="0" class="toolTip"><tr><td bgcolor="' + fg + '">' +
                                  '<table border="0" id = "tblToolTip" cellspacing="1" cellpadding="0"<tr><td bgcolor="' + bg + '">' +
                                   msg +
                                  '</td></tr></table>' +
                                  '</td></tr></table>';

                if (is_nav4) {
                    toolTipSTYLE.document.write(content);
                    toolTipSTYLE.document.close();
                    toolTipSTYLE.visibility = "visible";
                }

                else if (is_ie || is_nav6up) {
                    document.getElementById("toolTipLayer").innerHTML = content;
                    //                if(msg.split(' ').length >40)
                    //                document.getElementById("tblToolTip").width = "800px";
                    toolTipSTYLE.display = 'block';
                }
            }
        }

        $(document).ready(function () {           
            $('table#tblBusinessInfo tr:odd').addClass('AltRow');
            $('table#tblBusinessInfo tr:even').addClass('Row');
        });

    </script>

</as:RadCodeBlock>

<asp:PlaceHolder ID="phdMerchantDetail" runat="server">
    <div class="row" id="merchinfo">        
        <div class="col-md-9" data-toggle="collapse" data-target="#ciMerchantInformation">
            <h2 class="grid-title mt-3x">
                <asp:Literal ID="Literal45" runat="server" Text="Merchant Information" meta:resourcekey="LiteralResource45" />
            </h2>
        </div>
    </div>
    <div class="row">
        <div class="col-md-12 in" id="ciMerchantInformation">
            <table class="ASTable" id="tblMerchantInformation">
                <colgroup>
                    <col style="width: 150px" />
                    <col style="width: 350px" />
                    <col style="width: 150px" />
                </colgroup>
                <tr class="Row">
                    <td class="heading valign-top">
                        <asp:Literal ID="Literal1" runat="server" Text="Merchant ID:" meta:resourcekey="LiteralResource1" />
                    </td>
                    <td>
                        <div class="display-flex space-between">
                            <span><%=BindValue("MerchantNumber")%></span>
                            <span>
                                <as:PlaceHolder ID="uxPlhSwitchFDRMID" runat="server">
                                    <asp:HyperLink ID="uxSwitchFDRMID" runat="server" meta:resourcekey="uxLiteralSwitchToFDResource" Text="Switch to FD MID"></asp:HyperLink>                                    
                                </as:PlaceHolder>
                            </span>
                        </div>
                    </td>
                    <td class="heading valign-top">
                        <asp:Literal ID="Literal2" runat="server" Text="Contact:" meta:resourcekey="LiteralResource2" />
                    </td>
                    <td>
                        <%= BindValue("Contact")%>
                    </td>
                    <td class="heading valign-top">
                        <asp:Literal ID="Literal3" runat="server" Text="Status:" meta:resourcekey="LiteralResource3" />
                    </td>
                    <td>
                        <%= BindValue("Status")%>
                    </td>
                </tr>
                <tr class="AltRow">
                    <td class="heading valign-top">
                        <asp:Literal ID="Literal4" runat="server" Text="Merchant Name:" meta:resourcekey="LiteralResource4" />
                    </td>
                    <td>
                        <%= BindValue("MerchantName")%>
                    </td>
                    <td class="heading valign-top">
                        <asp:Literal ID="Literal5" runat="server" Text="Phone:" meta:resourcekey="LiteralResource5" />
                    </td>
                    <td>
                        <%= FormatPhone(BindValue("Phone"))%>
                    </td>
                    <td class="heading valign-top text-nowrap">
                        <asp:Literal ID="Literal6" runat="server" Text="Last Batch Activity:" meta:resourcekey="LiteralResource6" />
                    </td>
                    <td>
                        <asp:LinkButton runat="server" ID="lnkLastBatch" Text="" OnClick="lnkLastBatch_Click" />
                    </td>
                </tr>
                <tr class="Row">
                    <td class="heading valign-top">
                        <asp:Literal ID="Literal7" runat="server" Text="Address:" meta:resourcekey="LiteralResource7" />
                    </td>
                    <td>
                        <%= BindAddress(BindValue("Address1"), BindValue("Address2"), BindValue("Address3"), BindValue("City"), BindValue("State"), BindValue("Zip"))%>
                    </td>
                    <td class="heading valign-top">
                        <asp:Literal ID="Literal8" runat="server" Text="Email:" meta:resourcekey="LiteralResource8" />
                    </td>
                    <td colspan="3">
                        <%= BindValue("Email")%>
                    </td>
                </tr>
                <% if (uxpnlRMLabel.Visible)
                   { %>
                <tr class="AltRow">
                    <td class="heading valign-middle">
                        <as:Panel runat="server" ID="uxpnlRMLabel">
                            <asp:Literal ID="Literal9" runat="server" Text="Relationship Manager: " meta:resourcekey="LiteralResource9" />
                        </as:Panel>
                    </td>
                    <td colspan="5" class="valign-middle">
                        <as:Panel runat="server" ID="uxpnlRMCtrls">
                            <as:Literal runat="server" ID="uxlbRelationshipManager"></as:Literal>
                            <as:LinkButton ID="uxbtnAdd" OnClick="uxbtnAdd_click" Text="Add RM" runat="server" meta:resourcekey="uxbtnAddResource1" />
                            <as:LinkButton ID="uxbtnEdit" OnClick="uxbtnEdit_click" runat="server" Text="Edit" meta:resourcekey="uxbtnEditResource1" />
                            <as:Panel ID="uxpnlSaveCancel" runat="server" Visible="false">
                                <as:TextBox runat="server" ID="uxtxtRelationShipManager" MaxLength="30" />
                                <as:LinkButton ID="uxbtnSave" OnClick="uxbtnSave_click" OnClientClick="return CheckPrefix();" runat="server" Text="Save" meta:resourcekey="uxbtnSaveResource1" />
                                <as:LinkButton ID="uxbtnCancel" OnClick="uxbtnCancel_click" runat="server" Text="Cancel" meta:resourcekey="uxbtnCancelResource1" />
                                <script type="text/javascript">

                                    var Text_AtLeast30NoSpecialChars = '<%=GetLocalResourceObject("MIF_MerchantDetails_TSYSJS_Text_AtLeast30NoSpecialChars").ToString()%>';
                                    var Text_UnallowPrefixSuffix = '<%=GetLocalResourceObject("MIF_MerchantDetail_TSYSJS_Text_UnallowPrefixSuffix").ToString()%>';
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
                </tr>
                <%} %>
                <asp:Panel ID="uxPnlUserID" runat="server" Visible="false">
                    <tr id="trUserID">
                        <td class="heading text-nowrap valign-middle">
                            <asp:Literal ID="uxlblUserID" runat="server" Text="User ID:" />
                        </td>
                        <td>
                            <div class="opt-in-out-text mt-1x">
                                <%=BindValue("UserID")%>
                            </div>
                            <div class="pull-right opt-in-out-button">
                                <asp:Panel ID="uxPnlSiteAccess" runat="server">
                                    <as:Button ID="uxSiteAccess" runat="server" Text="Site Access" OnClick="uxSiteAccess_click" IsStandardButton="False" CssClass="btn btn-default" meta:resourcekey="uxSiteAccessResource1" />
                                </asp:Panel>
                            </div>
                        </td>
                        <td colspan="4"></td>
                    </tr>
                </asp:Panel>
            </table>
        </div>
    </div>
    <asp:Repeater ID="rptMerchantInfo" runat="server" EnableViewState="true">
        <ItemTemplate>
            <div id="format" class="row">
                <div class="col-md-6">
                    <div class="row" id="businessinfo">
                        <div class="col-xs-12" data-toggle="collapse" data-target="#ciBusinessInformation">
                            <h2 class="grid-title">
                                <asp:Literal ID="Literal11" runat="server" Text="Business Information" meta:resourcekey="LiteralResource11" /></h2>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12 in" id="ciBusinessInformation">
                            <table class="ASTable" id="tblBusinessInfo">                             
                                    <tr>
                                        <td class="heading">
                                            <asp:Literal ID="Literal12" runat="server" Text="Status:" meta:resourcekey="LiteralResource12" />
                                        </td>
                                        <td>
                                            <%# Eval("Status")%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="heading">
                                            <asp:Literal ID="Literal13" runat="server" Text="Date:" meta:resourcekey="LiteralResource13" />
                                        </td>
                                        <td>
                                            <%# FormatDate(Eval("OpenDate"))%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="heading">
                                            <asp:Literal ID="Literal14" runat="server" Text="Federal Tax ID:" meta:resourcekey="LiteralResource14" />
                                        </td>
                                        <td>
                                            <%# CheckPermisson(Eval("TaxID"), WebSiteConstants.SEC_PERMISSION_TAX_ID)%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="heading valign-middle">
                                            <asp:Literal ID="Literal15" runat="server" Text="Site Access:" meta:resourcekey="LiteralResource43" />
                                        </td>
                                        <td>
                                            <div class="<%# HasMSProductEnvironment()? "mt-1x" :"" %>">
                                                <asp:Literal ID="ltrSiteAccess" runat="server" Text='<%# HasMSProductEnvironment()?Eval("SiteAccess"):GetLocalResourceObject("MIF_MerchantDetails_TSYSJS_Text_NA").ToString()%>' />
                                            </div>
                                            <div class="pull-right opt-in-out-button">
                                                <as:PlaceHolder runat="server" ID="PlaceHolder1" Visible='<%# !IsCaseManagement %>'>
                                                    <as:Button ID="uxSiteAccess" runat="server" Text='<%# SetStatusText(Eval("SiteAccess"))%>' CssClass="btn btn-default"
                                                        Visible='<%# SetVisible(Eval("SiteAccess"))%>' OnClientClick='<%# SetURLForSiteAccessButton()%>' IsStandardButton="False" meta:resourcekey="uxSiteAccessResource1"/>
                                                </as:PlaceHolder>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="heading">
                                            <asp:Literal ID="Literal16" runat="server" Text="Average Sales Amount:" meta:resourcekey="LiteralResource15" />
                                        </td>
                                        <td>
                                            <%# Eval("AVGSalesAmount")%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="heading">
                                            <asp:Literal ID="Literal17" runat="server" Text="Customer Type:" meta:resourcekey="LiteralResource16" />
                                        </td>
                                        <td>
                                            <%# Eval("CustomerType")%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="heading">
                                            <asp:Literal ID="Literal18" runat="server" Text="Lease Number:" meta:resourcekey="LiteralResource17" />
                                        </td>
                                        <td>
                                            <%# Eval("LeaseNumber")%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="heading">
                                            <asp:Literal ID="Literal19" runat="server" Text="Lessor:" meta:resourcekey="LiteralResource18" />
                                        </td>
                                        <td>
                                            <%# Eval("Lessor")%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="heading">
                                            <asp:Literal ID="Literal20" runat="server" Text="Gift Card Customer Number:" meta:resourcekey="LiteralResource19" />
                                        </td>
                                        <td>
                                            <%# Eval("GiftCardCustomerNumber")%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="heading">
                                            <asp:Literal ID="Literal21" runat="server" Text="Gold (CDP):" meta:resourcekey="LiteralResource20" />
                                        </td>
                                        <td>
                                            <%# Eval("Gold")%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="heading">
                                            <asp:Literal ID="Literal22" runat="server" Text="Social Security #:" meta:resourcekey="LiteralResource21" />
                                        </td>
                                        <td>
                                            <%# Eval("SocialSecurityNumber")%>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td class="heading">
                                            <asp:Literal ID="Literal23" runat="server" Text="Risk Plan:" meta:resourcekey="LiteralResource22" />
                                        </td>
                                        <td>
                                            <%# Eval("RiskPlan")%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="heading">
                                            <asp:Literal ID="Literal24" runat="server" Text="Lease Date:" meta:resourcekey="LiteralResource23" />
                                        </td>
                                        <td>
                                            <%# Eval("LeaseDate")%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="heading">
                                            <asp:Literal ID="Literal25" runat="server" Text="Remaining Payment:" meta:resourcekey="LiteralResource24" />
                                        </td>
                                        <td>
                                            <%# Eval("RemainingPayment")%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="heading">
                                            <asp:Literal ID="Literal26" runat="server" Text="On Hold:" meta:resourcekey="LiteralResource25" />
                                        </td>
                                        <td>
                                            <%# Eval("OnHold")%>
                                        </td>
                                    </tr>
                            </table>
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                    <!-- Risk Info -->
                    <uc:RiskInfo ID="uxRiskInfo" runat="server" />
                    <asp:PlaceHolder ID="phdHierarchy" runat="server">
                        <div class="row" id="hierarchyinfo">
                            <div class="col-xs-12" data-toggle="collapse" data-target="#ciHierarchyInformation">
                                <h2 class="grid-title">
                                    <asp:Literal ID="Literal28" runat="server" Text="Hierarchy Information" meta:resourcekey="LiteralResource27" /></h2>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12 in" id="ciHierarchyInformation">
                                <table class="ASTable">
                                    <tr class="Row">
                                        <td class="heading">
                                            <asp:Literal ID="Literal29" runat="server" Text="Client Name:" meta:resourcekey="LiteralResource28" />
                                        </td>
                                        <td colspan="3">
                                            <%# Eval("ClientName")%>
                                        </td>
                                    </tr>
                                    <tr class="AltRow">
                                        <td class="heading">
                                            <asp:Literal ID="Literal30" runat="server" Text="Client Login:" meta:resourcekey="LiteralResource29" />
                                        </td>
                                        <td>
                                            <%# Eval("ClientLogin")%>
                                        </td>
                                        <td class="heading">
                                            <asp:Literal ID="Literal31" runat="server" Text="ISO:" meta:resourcekey="LiteralResource30" />
                                        </td>
                                        <td>
                                            <%# Eval("ISO")%>
                                        </td>
                                    </tr>
                                    <tr class="Row">
                                        <td class="heading">
                                            <asp:Literal ID="Literal32" runat="server" Text="Sales Office:" meta:resourcekey="LiteralResource31" />
                                        </td>
                                        <td>
                                            <%# Eval("SalesOffice")%>
                                        </td>
                                        <td class="heading">
                                            <asp:Literal ID="Literal33" runat="server" Text="Bank Number:" meta:resourcekey="LiteralResource32" />
                                        </td>
                                        <td>
                                            <%# Eval("BankNumber")%>
                                        </td>
                                    </tr>
                                    <tr class="AltRow">
                                        <td class="heading">
                                            <asp:Literal ID="Literal34" runat="server" Text="Merchant Type:" meta:resourcekey="LiteralResource33" />
                                        </td>
                                        <td>
                                            <%# Eval("MerchantType")%>
                                        </td>
                                        <td class="heading">
                                            <asp:Literal ID="Literal35" runat="server" Text="Chain Number:" meta:resourcekey="LiteralResource34" />
                                        </td>
                                        <td>
                                            <as:PlaceHolder ID="uxHChainNumber" runat="server" Visible='<%# CheckHierarchy("CHAINNUMBER") %>'>
                                                <a href="#" id="uxLinkHChainNumber" runat="server"><%# Eval("ChainNumber")%></a>
                                            </as:PlaceHolder>
                                            <as:Literal ID="uxLHChainNumber" runat="server" Visible='<%# !CheckHierarchy("CHAINNUMBER") %>' Text='<%# Eval("ChainNumber")%>'></as:Literal>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </asp:PlaceHolder>
                    <div class="row" id="bankinfo">
                        <div class="col-xs-12" data-toggle="collapse" data-target="#ciBankInformation">
                            <h2 class="grid-title">
                                <asp:Literal ID="Literal37" runat="server" Text="Bank Information" meta:resourcekey="LiteralResource36" /></h2>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12 in" id="ciBankInformation">
                            <table class="ASTable">
                                <tr class="Row">
                                    <td class="heading">
                                        <asp:Literal ID="Literal38" runat="server" Text="Transit / Routing #:" meta:resourcekey="LiteralResource37" />
                                    </td>
                                    <td>
                                        <%# GetRoutingNumber(Eval("RoutingNumber").ToString(),Eval("PartialRoutingNumber").ToString())%>
                                    </td>
                                </tr>
                                <tr class="AltRow">
                                    <td class="heading">
                                        <asp:Literal ID="Literal39" runat="server" Text="Acct #:" meta:resourcekey="LiteralResource38" />
                                    </td>
                                    <td>
                                        <%#CheckPermisson(Eval("AccountNumber"), WebSiteConstants.SEC_PERMISSION_DDA)%>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                    <div class="row" id="terminalinfo">
                        <div class="col-xs-12" data-toggle="collapse" data-target="#ciTerminalInformation">
                            <h2 class="grid-title">
                                <asp:Literal ID="Literal41" runat="server" Text="Terminal Information" meta:resourcekey="LiteralResource40" /></h2>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12 in" id="ciTerminalInformation">
                            <table class="ASTable">
                                <tr class="Row">
                                    <td class="heading">
                                        <asp:Literal ID="Literal42" runat="server" Text="Terminal:" meta:resourcekey="LiteralResource41" />
                                    </td>
                                    <td>
                                        <%# Eval("Terminal")%>
                                    </td>
                                </tr>
                                <tr class="AltRow">
                                    <td class="heading">
                                        <asp:Literal ID="Literal43" runat="server" Text="Other Card Types:" meta:resourcekey="LiteralResource42" />
                                    </td>
                                    <td>
                                        <%# Eval("OtherCardType")%>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </div>
            </div>
        </ItemTemplate>
    </asp:Repeater>
    <as:RadCodeBlock ID="RadCodeBlock1" runat="server">        
        <script type="text/javascript" src="<%=ResolveUrl("~") %>res/js/MIF_MerchantDetails_TSYS.js"></script>
    </as:RadCodeBlock>
</asp:PlaceHolder>
