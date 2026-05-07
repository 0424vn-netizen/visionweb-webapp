<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MIF_MerchantDetails_PLANET.ascx.cs"
    Inherits="UserControls_MIF_MerchantDetails_PLANET" %>
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

    </script>

</as:RadCodeBlock>
<asp:PlaceHolder ID="phdMerchantDetail" runat="server">
    <div class="row" id="merchinfo">
        <div class="col-md-9" data-toggle="collapse" data-target="#ciMerchantInformation">
            <h2 class="grid-title  mt-3x">
                <asp:Literal ID="Literal24" runat="server" Text="Merchant Information" meta:resourcekey="LiteralResource24" />
            </h2>
        </div>
    </div>
    <div class="row">
        <div class="col-md-12 in" id="ciMerchantInformation">
            <table id="info" class="ASTable">
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
                        <%= BindValue("MerchantNumber")%>
                    </td>
                    <td class="heading valign-top">
                        <asp:Literal ID="Literal2" runat="server" Text="Phone:" meta:resourcekey="LiteralResource2" />
                    </td>
                    <td>
                        <%= FormatPhone(BindValue("Phone"))%>
                    </td>
                    <td class="heading valign-top">
                        <asp:Literal ID="Literal3" runat="server" Text="Status:" meta:resourcekey="LiteralResource22" />
                    </td>
                    <td>
                        <%= BindValue("Status")%>
                    </td>
                </tr>
                <tr class="AltRow">
                    <td class="heading valign-top">
                        <asp:Literal ID="Literal4" runat="server" Text="Merchant Name:" meta:resourcekey="LiteralResource3" />
                    </td>
                    <td>
                        <%= BindValue("MerchantName")%>
                    </td>
                    <td class="heading valign-top">
                        <asp:Literal ID="Literal5" runat="server" Text="Email:" meta:resourcekey="LiteralResource4" />
                    </td>
                    <td>
                        <%= BindValue("Email")%>
                    </td>
                    <td class="heading valign-top">
                        <asp:Literal ID="Literal6" runat="server" Text="Last Batch Activity:" meta:resourcekey="LiteralResource5" />
                    </td>
                    <td>
                        <asp:LinkButton runat="server" ID="lnkLastBatch" Text="" OnClick="lnkLastBatch_Click" />
                    </td>
                </tr>
                <tr class="Row">
                    <td class="heading valign-top">
                        <asp:Literal ID="Literal7" runat="server" Text="Address:" meta:resourcekey="LiteralResource6" />
                    </td>
                    <td>
                        <%= BindAddress(BindValue("Address1"), BindValue("Address2"), BindValue("Address3"), BindValue("City"), BindValue("State"), BindValue("Zip"))%>
                    </td>
                    <td class="heading valign-top">
                        <as:Panel runat="server" ID="uxpnlRMLabel">
                            <asp:Literal ID="Literal8" runat="server" Text="Relationship Manager: " meta:resourcekey="LiteralResource7" />
                        </as:Panel>
                    </td>
                    <td colspan="3">
                        <as:Panel runat="server" ID="uxpnlRMCtrls" Style="margin-top: 2px;">
                            <as:Literal runat="server" ID="uxlbRelationshipManager"></as:Literal>
                            <as:LinkButton ID="uxbtnAdd" OnClick="uxbtnAdd_click" Text="Add RM" runat="server" meta:resourcekey="uxbtnAddResource1" />
                            <as:LinkButton ID="uxbtnEdit" OnClick="uxbtnEdit_click" runat="server" Text="Edit" meta:resourcekey="uxbtnEditResource1" />
                            <as:Panel ID="uxpnlSaveCancel" runat="server" Visible="false">
                                <as:TextBox runat="server" ID="uxtxtRelationShipManager" MaxLength="30" />
                                <as:LinkButton ID="uxbtnSave" OnClick="uxbtnSave_click" OnClientClick="return CheckPrefix();" runat="server" Text="Save" meta:resourcekey="uxbtnSaveResource1" />
                                <as:LinkButton ID="uxbtnCancel" OnClick="uxbtnCancel_click" runat="server" Text="Cancel" meta:resourcekey="uxbtnCancelResource1" />
                                <script type="text/javascript">
                                    var Text_AtLeast30NoSpecialChars = '<%=GetLocalResourceObject("MIF_MerchantDetails_PLANETJS_Text_AtLeast30NoSpecialChars").ToString()%>';
                                    var Text_UnallowPrefixSuffix = '<%=GetLocalResourceObject("MIF_MerchantDetail_PLANETJS_Text_UnallowPrefixSuffix").ToString()%>';

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
                <asp:Panel ID="uxPnlUserID" runat="server" Visible="false">
                    <tr class="AltRow">
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
                                <asp:Literal ID="Literal10" runat="server" Text="Business Information" meta:resourcekey="LiteralResource9" />
                            </h2>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12 in" id="ciBusinessInformation">
                            <table class="ASTable">
                                <tr class="Row">
                                    <td class="heading">
                                        <asp:Literal ID="Literal11" runat="server" Text="Merchant Name:" meta:resourcekey="LiteralResource10" />
                                    </td>
                                    <td>
                                        <%# Eval("MerchantName")%>
                                    </td>
                                </tr>
                                <tr class="AltRow">
                                    <td class="heading">
                                        <asp:Literal ID="Literal12" runat="server" Text="Fax:" meta:resourcekey="LiteralResource11" />
                                    </td>
                                    <td>
                                        <%# FormatPhone(Eval("Fax"))%>   
                                    </td>
                                </tr>
                                <tr class="Row">
                                    <td class="heading">
                                        <asp:Literal ID="Literal13" runat="server" Text="Open Date:" meta:resourcekey="LiteralResource12" />
                                    </td>
                                    <td>
                                        <%# FormatDate(Eval("OpenDate"))%>                                        
                                    </td>
                                </tr>
                                <tr class="AltRow">
                                    <td class="heading">
                                        <asp:Literal ID="Literal14" runat="server" Text="Closed Date:" meta:resourcekey="LiteralResource13" />
                                    </td>
                                    <td>
                                        <%# FormatDate(Eval("ClosedDate"))%>
                                    </td>
                                </tr>
                                <tr class="Row">
                                    <td class="heading">
                                        <asp:Literal ID="Literal15" runat="server" Text="Status:" meta:resourcekey="LiteralResource14" />
                                    </td>
                                    <td>
                                        <%# Eval("Status")%>
                                    </td>
                                </tr>
                                <tr class="AltRow">
                                    <td class="heading valign-middle">
                                        <asp:Literal ID="Literal16" runat="server" Text="Site Access:" meta:resourcekey="LiteralResource23" />
                                    </td>
                                    <td>
                                        <div class="<%# HasMSProductEnvironment()? "mt-1x" :"" %>">
                                            <asp:Literal ID="ltrSiteAccess" runat="server" Text='<%# HasMSProductEnvironment()?Eval("SiteAccess"):GetLocalResourceObject("MIF_MerchantDetails_PLANETJS_Text_NA").ToString()%>' />
                                        </div>
                                        <div class="pull-right opt-in-out-button">
                                            <as:PlaceHolder ID="PlaceHolder1" runat="server" Visible='<%# !IsCaseManagement %>'>
                                                <as:Button ID="uxSiteAccess" runat="server" Text='<%# SetStatusText(Eval("SiteAccess"))%>'
                                                    Visible='<%# SetVisible(Eval("SiteAccess"))%>' OnClientClick='<%# SetURLForSiteAccessButton()%>' />
                                            </as:PlaceHolder>
                                        </div>
                                    </td>
                                </tr>
                                <tr class="Row">
                                    <td class="heading">
                                        <asp:Literal ID="Literal17" runat="server" Text="SIC/MCC:" meta:resourcekey="LiteralResource16" />
                                    </td>
                                    <td>
                                        <%#FormatSIC(Eval("SICMCC"), Eval("SICDescription"))%>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                    <!-- Risk Info -->
                    <uc:RiskInfo ID="uxRiskInfo" runat="server" />
                    <asp:PlaceHolder ID="PlaceHolder2" runat="server">
                        <div class="row" id="hierarchyinfo">
                            <div class="col-xs-12" data-toggle="collapse" data-target="#ciHierarchyInformation">
                                <h2 class="grid-title">
                                    <asp:Literal ID="Literal19" runat="server" Text="Hierarchy Information" meta:resourcekey="LiteralResource18" />
                                </h2>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12 in" id="ciHierarchyInformation">
                                <table class="ASTable">
                                    <tr class="Row">
                                        <td class="heading">
                                            <asp:Literal ID="Literal20" runat="server" Text="Client Name:" meta:resourcekey="LiteralResource19" />
                                        </td>
                                        <td colspan="3">
                                            <%# Eval("ClientName")%>
                                        </td>
                                    </tr>
                                    <tr class="AltRow">
                                        <td class="heading">
                                            <asp:Literal ID="Literal21" runat="server" Text="Client Login:" meta:resourcekey="LiteralResource20" />
                                        </td>
                                        <td colspan="3">
                                            <%# Eval("ClientLogin")%>
                                        </td>
                                    </tr>
                                    <tr class="Row">
                                        <td class="heading">
                                            <asp:Literal ID="Literal22" runat="server" Text="Sales Unit:" meta:resourcekey="LiteralResource21" />
                                        </td>
                                        <td>
                                            <as:PlaceHolder ID="uxHSaleUnit" runat="server" Visible='<%# CheckHierarchy("SALESUNIT") %>'>
                                                <a href="#" onclick="return rf_SubmitReportFilterValues('96', 'SALESUNIT', '<%# Eval("HierarchyID1")%>');">
                                                    <%# Eval("HierarchyID1")%></a>
                                            </as:PlaceHolder>
                                            <as:Literal ID="uxLHSaleUnit" runat="server" Visible='<%# !CheckHierarchy("SALESUNIT") %>'
                                                Text='<%# Eval("HierarchyID1")%>'></as:Literal>
                                        </td>
                                        <td class="heading">
                                            <asp:Literal ID="Literal23" runat="server" Text="Agent:" meta:resourcekey="LiteralResource26" />
                                        </td>
                                        <td>
                                            <as:PlaceHolder ID="uxHAgent" runat="server" Visible='<%# CheckHierarchy("PLANETAGENT") %>'>
                                                <a href="#" onclick="return rf_SubmitReportFilterValues('97', 'PLANETAGENT', '<%# Eval("HierarchyID2")%>');">
                                                    <%# Eval("HierarchyID2")%></a>
                                            </as:PlaceHolder>
                                            <as:Literal ID="uxLHAgent" runat="server" Visible='<%# !CheckHierarchy("PLANETAGENT") %>'
                                                Text='<%# Eval("HierarchyID2")%>'></as:Literal>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </asp:PlaceHolder>
                </div>
            </div>
        </ItemTemplate>
    </asp:Repeater>
</asp:PlaceHolder>
