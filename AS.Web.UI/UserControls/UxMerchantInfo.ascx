<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UxMerchantInfo.ascx.cs" Inherits="UserControls_UxMerchantInfo" %>

<style type="text/css">
    .right
    {
        padding-right: 7px;
        text-align: right;
    }
    .bold_text
    {
        font-weight: bold;
    }
    .bgHeader
    {
        background-color: #C2C2C2;
        height: 28px;
        text-align: center;
        vertical-align: middle;
        text-decoration: underline;
        font-weight: bold;
    }
  
    .ScrollX
    {
        overflow-x: auto;
        overflow-y: hidden !important;
    }
    .toolTip
    {
        filter: alpha(opacity=80);
        -moz-opacity: 0.8;
        opacity: 0.8; /* comment the above 3 line if you don't want transparency*/
    }
    .altbg
    {
        border-right-width: 0px;
        background-color: #F3F3F3 !important;
    }
    .hd
    {
        height: 28px;
        text-align: left;
        font-family: Arial,Helvetica,Sans-serif;
        font-size: 12px;
        font-weight: bold;
    }
    .hddt
    {
        height: 20px;
        padding: 4px 7px;
        padding-left: 10px !important;
    }
    .bgHeader
    {
        font-weight: bold;
        text-align: center;
        text-decoration: none !important;
    }
    .RadGrid_Default, .RadGrid_Default .rgMasterTable
    {
        font: 12px "Arial, Helvetica, Sans-serif";
    }
    .rgRow
    {
        height: 30px;
        padding: 4px 7px;
    }
    .RadGrid .rgRow td, .RadGrid .rgAltRow td, .RadGrid .rgEditRow td, .RadGrid .rgFooter td, .RadGrid .rgFilterRow td, .RadGrid .rgHeader
    {
        height: 10px;
        padding: 7px 7px;
    }
</style>
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
                alert(String.format('<%= GetLocalResourceObject("UxMerchantInfo_ascx_Comment").ToString()%>' + ' ' + '<%=Resources.MessageManager.Generic_FieldLengthExceeded%>', 1000));
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
    </script>

</as:RadCodeBlock>
<asp:HiddenField runat="server" ID="uxComment" />
<asp:HiddenField runat="server" ID="uxEmails" />
<asp:HiddenField runat="server" ID="uxMaxSec" />
<asp:PlaceHolder ID="phdMerchantDetail" runat="server">
    <asp:Repeater ID="rptMerchantInfo" runat="server" EnableViewState="true">
        <ItemTemplate>
            <as:Container ID="ascontainer" runat="server" HeaderText="Merchant Information" Width="100%" FooterControlID="" FooterText="" HeaderControlID="" TemplateName="ascontainer_greyborder.tpl" meta:resourcekey="ascontainerResource1">
            <table id="info" align="center" width="100%">
                <tr>
                    <td width="13%" class="bold_text right">
                        <as:Literal ID="ltMerchantNumber" runat="server" Text="Merchant ID:" meta:resourcekey="ltMerchantNumberResource1"></as:Literal>
                    </td>
                    <td width="23%">
                        <%# Eval("MerchantNumber")%>
                    </td>
                    <td width="13%" class="bold_text right">
                        <as:Literal ID="Literal1" runat="server" Text="Contact:" meta:resourcekey="Literal1Resource1"></as:Literal>
                    </td>
                    <td width="23%">
                        <%# Eval("Contact")%>
                    </td>
                    <td width="13%" class="bold_text right">
                        <as:Literal ID="Literal2" runat="server" Text="Status:" meta:resourcekey="Literal2Resource1"></as:Literal>
                    </td>
                    <td>
                        <%# Eval("Status")%>
                    </td>
                </tr>
                <tr>
                    <td class="bold_text right">
                        <as:Literal ID="Literal3" runat="server" Text="Merchant Name:" meta:resourcekey="Literal3Resource1"></as:Literal>
                    </td>
                    <td>
                        <%# Eval("MerchantName")%>
                    </td>
                    <td class="bold_text right">
                        <as:Literal ID="Literal4" runat="server" Text="Phone:" meta:resourcekey="Literal4Resource1"></as:Literal>
                    </td>
                    <td>
                        <%# FormatPhone(Eval("Phone"))%>
                    </td>
                    <td class="bold_text right">
                        <as:Literal ID="Literal5" runat="server" Text="Last Batch Activity:" meta:resourcekey="Literal5Resource1"></as:Literal>
                    </td>
                    <td>
                        <asp:LinkButton runat="server" ID="lnkLastBatch" Text='<%# FormatDate(Eval("LastBactchActivity")) %>'
                            OnCommand="lnkGrid_Command" CommandName="lnkLastBatch" CommandArgument='<%# FormatDate(Eval("LastBactchActivity")) %>' meta:resourcekey="lnkLastBatchResource1" />
                    </td>
                </tr>
                <tr>
                    <td valign="top" class="bold_text right">
                        <as:Literal ID="Literal6" runat="server" Text="Address:" meta:resourcekey="Literal6Resource1"></as:Literal>
                    </td>
                    <td>
                        <%# BindAddress(Eval("Address1"), Eval("Address2"), Eval("Address3"),  Eval("City"), Eval("State"), Eval("Zip"))%>
                    </td>
                    <td valign="top" class="bold_text right">
                        <as:Literal ID="Literal7" runat="server" Text="Email:" meta:resourcekey="Literal7Resource1"></as:Literal>
                    </td>
                    <td valign="top">
                        <%# Eval("Email")%>
                    </td>
                    <td>
                    </td>
                    <td>
                    </td>
                </tr>
            </table>
            </as:Container>
            <br />
            <div id="format">
                <table cellpadding="0" cellspacing="0" style="width: 100%; border: 0px">
                    <tr>
                        <td style="width: 51%; border-right: 0px solid #014165; border-left: 0px solid #014165; vertical-align: top">
                            <table cellpadding="0" cellspacing="0" width="100%">
                                <tr>
                                    <td>
                                        <div class="RadGrid RadGrid_Default">
                                            <table cellpadding="0" cellspacing="0" class="rgMasterTable" width="100%">
                                                <tr class="ContainerPanelHeader hd">
                                                    <th colspan="2" class="hddt" align="left">
                                                        <as:Literal ID="Literal8" runat="server" Text="Business Information" meta:resourcekey="Literal8Resource1"></as:Literal>
                                                    </th>
                                                </tr>
                                                <tr class="rgRow">
                                                    <td width="40%" valign="top" style="border-left-width: 1px; border-top-width: 0px;
                                                        border-bottom-width: 0px">
                                                        <as:Literal ID="Literal9" runat="server" Text="Corporate Name:" meta:resourcekey="Literal9Resource1"></as:Literal>
                                                    </td>
                                                    <td style="border-left-width: 1px; border-top-width: 0px; border-bottom-width: 0px">
                                                        <%#Eval("CorporateName")%>
                                                    </td>
                                                </tr>
                                                <tr class="rgRow altbg">
                                                    <td style="border-top-width: 0px; border-bottom-width: 0px">
                                                        <as:Literal ID="Literal10" runat="server" Text="Corporate Address:" meta:resourcekey="Literal10Resource1"></as:Literal>
                                                    </td>
                                                    <td style="border-left-width: 1px; border-top-width: 0px; border-bottom-width: 0px">
                                                        <%#Eval("CorporateAddress")%>
                                                    </td>
                                                </tr>
                                                <tr class="rgRow">
                                                    <td>
                                                        <as:Literal ID="Literal11" runat="server" Text="Fax:" meta:resourcekey="Literal11Resource1"></as:Literal>
                                                    </td>
                                                    <td>
                                                        <%#FormatPhone(Eval("Fax"))%>
                                                    </td>
                                                </tr>
                                                <tr class="rgRow altbg">
                                                    <td>
                                                        <as:Literal ID="Literal12" runat="server" Text="Open Date:" meta:resourcekey="Literal12Resource1"></as:Literal>
                                                    </td>
                                                    <td>
                                                        <%#FormatDate(Eval("OpenDate"))%>
                                                    </td>
                                                </tr>
                                                <tr class="rgRow">
                                                    <td>
                                                        <as:Literal ID="Literal13" runat="server" Text="Closed Date:" meta:resourcekey="Literal13Resource1"></as:Literal>
                                                    </td>
                                                    <td>
                                                        <%#FormatDate(Eval("ClosedDate"))%>
                                                    </td>
                                                </tr>
                                                <tr class="rgRow altbg">
                                                    <td>
                                                        <as:Literal ID="Literal14" runat="server" Text="Status:" meta:resourcekey="Literal14Resource1"></as:Literal>
                                                    </td>
                                                    <td>
                                                        <%# Eval("Status")%>
                                                    </td>
                                                </tr>
                                                <tr class="rgRow">
                                                    <td class="height">
                                                        <as:Literal ID="Literal15" runat="server" Text="Site Access:" meta:resourcekey="Literal15Resource1"></as:Literal>
                                                    </td>
                                                    <td valign="middle">
                                                        <%#Eval("SiteAccess")%>
                                                    </td>
                                                </tr>
                                                <tr class="rgRow altbg">
                                                    <td>
                                                        <as:Literal ID="Literal16" runat="server" Text="Tax ID:" meta:resourcekey="Literal16Resource1"></as:Literal>
                                                    </td>
                                                    <td>
                                                        <%#CheckPermisson(Eval("TaxID"),WebSiteConstants.SEC_PERMISSION_TAX_ID)%>
                                                    </td>
                                                </tr>
                                                <tr class="rgRow">
                                                    <td>
                                                        <as:Literal ID="Literal17" runat="server" Text="SIC/MCC:" meta:resourcekey="Literal17Resource1"></as:Literal>
                                                    </td>
                                                    <td>
                                                        <%#Eval("SICDescription")%>
                                                    </td>
                                                </tr>
                                                <tr class="rgRow altbg">
                                                    <td>
                                                        <as:Literal ID="Literal18" runat="server" Text="Deposit Type:" meta:resourcekey="Literal18Resource1"></as:Literal>
                                                    </td>
                                                    <td>
                                                        <%#Eval("DepositType")%>
                                                    </td>
                                                </tr>
                                                <tr class="rgRow">
                                                    <td>
                                                        <as:Literal ID="Literal19" runat="server" Text="ETC Type:" meta:resourcekey="Literal19Resource1"></as:Literal>
                                                    </td>
                                                    <td>
                                                        <%# Eval("ETCType")%>
                                                    </td>
                                                </tr>
                                                <tr class="rgRow altbg">
                                                    <td>
                                                        <as:Literal ID="Literal20" runat="server" Text="ETC Cutoff:" meta:resourcekey="Literal20Resource1"></as:Literal>
                                                    </td>
                                                    <td>
                                                        <%# Eval("ETCCutoff")%>
                                                    </td>
                                                </tr>
                                                <tr class="rgRow">
                                                    <td>
                                                        <as:Literal ID="Literal21" runat="server" Text="Seasonal Merchant:" meta:resourcekey="Literal21Resource1"></as:Literal>
                                                    </td>
                                                    <td>
                                                        <%#Eval("SeasonalMerchant")%>
                                                    </td>
                                                </tr>
                                                <tr class="rgRow">
                                                    <td>
                                                        <as:Literal ID="Literal22" runat="server" Text="Statement I/C Print Options:" meta:resourcekey="Literal22Resource1"></as:Literal>
                                                    </td>
                                                    <td>
                                                        <span title='<%# Eval("StatementICPrintOptionsDesc") %>'>
                                                            <%#Eval("StatementICPrintOptions")%>&nbsp;&nbsp;&nbsp; </span>
                                                    </td>
                                                </tr>
                                                <tr class="rgRow altbg">
                                                    <td>
                                                        <as:Literal ID="Literal23" runat="server" Text="Statement Online Debit Fee Print Option:" meta:resourcekey="Literal23Resource1"></as:Literal>
                                                    </td>
                                                    <td>
                                                        <span title='<%# Eval("StatementOnlineDebitFeePrintOptionDesc") %>'>
                                                            <%#Eval("StatementOnlineDebitFeePrintOption")%>&nbsp;&nbsp;&nbsp; </span>
                                                    </td>
                                                </tr>
                                                <tr class="rgRow">
                                                    <td>
                                                        <as:Literal ID="Literal24" runat="server" Text="Statement Hold Flag:" meta:resourcekey="Literal24Resource1"></as:Literal>
                                                    </td>
                                                    <td>
                                                        <%#Eval("StatementHoldFlag")%>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td style="width: 3%">
                        </td>
                        <td style="width: 47%; vertical-align: top">
                            <asp:PlaceHolder ID="phdHierarchy" runat="server">
                                <table cellpadding="0" cellspacing="0" width="100%">
                                    <tr>
                                        <td>
                                            <div class="RadGrid RadGrid_Default">
                                                <table cellpadding="0" cellspacing="0" class="rgMasterTable" width="100%">
                                                    <tr class="ContainerPanelHeader hd">
                                                        <th colspan="4" class="hddt" align="left">
                                                            <as:Literal ID="Literal25" runat="server" Text="Hierarchy Information" meta:resourcekey="Literal25Resource1"></as:Literal>
                                                        </th>
                                                    </tr>
                                                    <tr class="rgRow">
                                                        <td><as:Literal ID="Literal26" runat="server" Text="Client Name:" meta:resourcekey="Literal26Resource1"></as:Literal></td>
                                                        <td colspan="3"><%# Eval("ClientName")%></td>
                                                    </tr>
                                                    <tr class="rgRow altbg">
                                                        <td><as:Literal ID="Literal27" runat="server" Text="Client Login:" meta:resourcekey="Literal27Resource1"></as:Literal></td>
                                                        <td><%# Eval("ClientLogin")%></td>
                                                        <td><as:Literal ID="Literal28" runat="server" Text="Sales Agent:" meta:resourcekey="Literal28Resource1"></as:Literal></td>
                                                        <td><%# Eval("SalesAgent")%></td>
                                                    </tr>
                                                    <tr class="rgRow">
                                                        <td><as:Literal ID="Literal29" runat="server" Text="Sys/Prin/Agent:" meta:resourcekey="Literal29Resource1"></as:Literal></td>
                                                        <td><%# Eval("SysPrinAgent")%></td>
                                                        <td><as:Literal ID="Literal30" runat="server" Text="Headqrtr Merchant:" meta:resourcekey="Literal30Resource1"></as:Literal></td>
                                                        <td><%# Eval("Headquarter")%></td>
                                                    </tr>
                                                    <tr class="rgRow altbg">
                                                        <td></td>
                                                        <td></td>
                                                        <td><as:Literal ID="Literal31" runat="server" Text="Chain Code:" meta:resourcekey="Literal31Resource1"></as:Literal></td>
                                                        <td><%# Eval("ChainCode")%></td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                                <br />
                            </asp:PlaceHolder>
                            <table cellpadding="0" cellspacing="0" width="100%">
                                <tr>
                                    <td>
                                        <div class="RadGrid RadGrid_Default">
                                            <table cellpadding="0" cellspacing="0" class="rgMasterTable" width="100%">
                                                <tr class="ContainerPanelHeader hd">
                                                    <th colspan="2" class="hddt" align="left">
                                                        <as:Literal ID="Literal32" runat="server" Text="Bank Information" meta:resourcekey="Literal32Resource1"></as:Literal>
                                                    </th>
                                                </tr>
                                                <tr class="rgRow">
                                                    <td width="40%"><as:Literal ID="Literal33" runat="server" Text="Bank Name:" meta:resourcekey="Literal33Resource1"></as:Literal></td>
                                                    <td><%# Eval("BankName")%></td>
                                                </tr>
                                                <tr class="rgRow altbg">
                                                    <td><as:Literal ID="Literal34" runat="server" Text="Routing #:" meta:resourcekey="Literal34Resource1"></as:Literal></td>
                                                    <td><%# Eval("RoutingNumber")%></td>
                                                </tr>
                                                <tr class="rgRow">
                                                    <td><as:Literal ID="Literal35" runat="server" Text="DDA #:" meta:resourcekey="Literal35Resource1"></as:Literal></td>
                                                    <td><%#CheckPermisson(Eval("DDANumber"), WebSiteConstants.SEC_PERMISSION_DDA)%></td>
                                                </tr>
                                                <tr class="rgRow altbg">
                                                    <td><as:Literal ID="Literal36" runat="server" Text="Days Hold:" meta:resourcekey="Literal36Resource1"></as:Literal></td>
                                                    <td><%# Eval("DaysHold")%></td>
                                                </tr>
                                            </table>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <br />
                            <table cellpadding="0" cellspacing="0" width="100%">
                                <tr>
                                    <td>
                                        <div class="RadGrid RadGrid_Default">
                                            <table cellpadding="0" cellspacing="0" class="rgMasterTable" width="100%">
                                                <tr class="ContainerPanelHeader hd">
                                                    <th colspan="4" class="hddt" align="left">
                                                        <as:Literal ID="Literal37" runat="server" Text="Other Card Information" meta:resourcekey="Literal37Resource1"></as:Literal>
                                                    </th>
                                                </tr>
                                                <tr class="rgRow">
                                                    <td width="25%"><as:Literal ID="Literal38" runat="server" Text="AMEX" meta:resourcekey="Literal38Resource1"></as:Literal></td>
                                                    <td width="25%"><%# Eval("AMEX")%></td>
                                                    <td width="25%"><as:Literal ID="Literal39" runat="server" Text="Pin Debit" meta:resourcekey="Literal39Resource1"></as:Literal></td>
                                                    <td width="25%"><%# Eval("PinDebit")%></td>
                                                </tr>
                                                <tr class="rgRow altbg">
                                                    <td><as:Literal ID="Literal40" runat="server" Text="AMEX ONEPOINT" meta:resourcekey="Literal40Resource1"></as:Literal></td>
                                                    <td><%# Eval("AMEXONPOINT")%></td>
                                                    <td><as:Literal ID="Literal41" runat="server" Text="JCB" meta:resourcekey="Literal41Resource1"></as:Literal></td>
                                                    <td><%# Eval("JCB")%></td>
                                                </tr>
                                                <tr class="rgRow">
                                                    <td><as:Literal ID="Literal42" runat="server" Text="Discover" meta:resourcekey="Literal42Resource1"></as:Literal></td>
                                                    <td><%# Eval("Discover")%></td>
                                                    <td><as:Literal ID="Literal43" runat="server" Text="Wright Express" meta:resourcekey="Literal43Resource1"></as:Literal></td>
                                                    <td><%# Eval("WrightExpress")%></td>
                                                </tr>
                                                <tr class="rgRow altbg">
                                                    <td><as:Literal ID="Literal44" runat="server" Text="Discover Full AQC" meta:resourcekey="Literal44Resource1"></as:Literal></td>
                                                    <td><%# Eval("DiscoverFullAQC")%></td>
                                                    <td><as:Literal ID="Literal45" runat="server" Text="Voyager" meta:resourcekey="Literal45Resource1"></as:Literal></td>
                                                    <td><%# Eval("Voyager")%></td>
                                                </tr>
                                            </table>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </div>
        </ItemTemplate>
    </asp:Repeater>
</asp:PlaceHolder>
