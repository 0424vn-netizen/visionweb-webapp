<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="CaseManagement.aspx.cs" Inherits="CaseManagement" Title="CASE MANAGEMENT" meta:resourcekey="PageResource1" %>

<%@ Register TagName="PageTile" Src="~/UserControls/PageTitle.ascx" TagPrefix="uc" %>
<%@ Register TagName="MerchantInfo" Src="~/UserControls/UxMerchantInfo.ascx" TagPrefix="uc" %>
<%@ Register TagName="MIF_FDR" Src="~/UserControls/MIF_MerchantDetails_FDR.ascx"
    TagPrefix="uc" %>
<%@ Register TagName="MIF_PLANET" Src="~/UserControls/MIF_MerchantDetails_PLANET.ascx"
    TagPrefix="uc" %>
<%@ Register TagName="MIF_TSYS" Src="~/UserControls/MIF_MerchantDetails_TSYS.ascx"
    TagPrefix="uc" %>
<%@ Register TagName="MIF_ORION" Src="~/UserControls/MIF_MerchantDetails_ORI.ascx"
    TagPrefix="uc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="Server">
    <tek:RadAjaxManagerProxy ID="RadAjaxManagerProxy1" runat="server">
        <AjaxSettings>
            <tek:AjaxSetting AjaxControlID="uxCommentList">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="uxCommentList" />
                </UpdatedControls>
            </tek:AjaxSetting>
        </AjaxSettings>
    </tek:RadAjaxManagerProxy>
    <tek:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server"></tek:RadAjaxLoadingPanel>

    <uc:PageTile ID="uxReportTitle" runat="server" ReportTitle="CASE MANAGEMENT" meta:resourcekey="uxReportTitleResource1" />
    <div style="text-align: right;">
        <as:Button runat="server" ID="uxSearch" Text="Search" OnClientClick="goToCaseSearch();return false;" IsStandardButton="False" meta:resourcekey="uxSearchResource1" />
    </div>
    <br />
    <as:Container ID="asContainer1" runat="server" HeaderText="Ticket Details" Width="100%" meta:resourcekey="asContainer1Resource1"
        TemplateName="ascontainer_whiteborder.tpl" FooterControlID="" FooterText="" HeaderControlID="">
        <div style="padding: 0 5px 0 5px;">

            <table cellspacing="0" cellpadding="0" border="0" width="100%">
                <tr>
                    <td style="padding-left: 5px; padding-top: 5px; width: 10%" class="strong" align="left">
                        <as:Literal ID="ltTicket" runat="server" Text="Ticket #:" meta:resourcekey="ltTicketResource1"></as:Literal>
                    </td>
                    <td style="width: 10%; padding-left: 5px; padding-top: 5px;">
                        <as:Literal runat="server" ID="uxTicketNumber" meta:resourcekey="uxTicketNumberResource1" />
                    </td>
                    <as:Panel runat="server" ID="uxPanel1" Visible="false" meta:resourcekey="uxPanel1Resource1">
                        <td style="padding-left: 20px; padding-top: 5px; width: 10%">&nbsp;</td>
                        <td style="padding-left: 5px; padding-top: 5px; width: 10%">&nbsp;</td>
                        <td style="padding-left: 20px; padding-top: 5px; width: 10%">&nbsp;</td>
                        <td style="padding-left: 5px; padding-top: 5px; width: 10%">&nbsp;</td>
                    </as:Panel>
                    <td>&nbsp;</td>
                    <td style="padding-left: 5px; padding-top: 5px; width: 10%" class="strong" align="left">
                        <as:Literal ID="ltOpenDate" runat="server" Text="Open Date:" meta:resourcekey="ltOpenDateResource1"></as:Literal>
                    </td>
                    <td style="padding-left: 5px; padding-top: 5px; width: 15%">
                        <as:Literal runat="server" ID="uxOpenDate" meta:resourcekey="uxOpenDateResource1" />
                    </td>
                </tr>
                <tr>
                    <td style="padding-left: 5px; padding-top: 5px" class="strong">
                        <as:Literal ID="ltAssignedTo" runat="server" Text="Assigned To:" meta:resourcekey="ltAssignedToResource1"></as:Literal>
                    </td>
                    <td style="padding-left: 5px; padding-top: 5px">
                        <as:RadComboBox runat="server" ID="uxUserAssigned" MaxHeight="200px" AllowCustomText="true"
                            DataTextField="KeyValue" DataValueField="KeyName" Width="200px" meta:resourcekey="uxUserAssignedResource1" />
                    </td>
                    <asp:Panel runat="server" ID="uxEditPanel" Visible="false" meta:resourcekey="uxEditPanelResource1">
                        <td class="strong" style="padding-left: 60px; padding-top: 5px;">
                            <as:Literal ID="ltStatus" runat="server" Text="Status:" meta:resourcekey="ltStatusResource1"></as:Literal>
                        </td>
                        <td class="strong" style="padding-top: 5px;">
                            <as:RadComboBox runat="server" ID="uxStatus" OnSelectedIndexChanged="uxStatus_SelectedIndexChanged"
                                AutoPostBack="true" DataTextField="KeyValue"
                                DataValueField="KeyName" Width="150px" meta:resourcekey="uxStatusResource1" />
                        </td>
                        <td class="strong" style="padding-left: 50px; padding-top: 5px;">
                            <as:Literal ID="ltResolution" runat="server" Text="Resolution:" meta:resourcekey="ltResolutionResource1"></as:Literal>
                        </td>
                        <td class="strong" style="padding-left: 10px; padding-top: 5px;">
                            <as:RadComboBox runat="server" ID="uxResolution" Width="150px" DataTextField="KeyValue"
                                DataValueField="KeyName" meta:resourcekey="uxResolutionResource1" />
                        </td>
                    </asp:Panel>
                    <td>&nbsp;</td>
                    <td style="padding-left: 5px; padding-top: 5px;" class="strong" align="left">
                        <as:Literal ID="ltCloseDate" runat="server" Text="Close&nbsp;Date:" meta:resourcekey="ltCloseDateResource1"></as:Literal>
                    </td>
                    <td style="padding-left: 5px; padding-top: 5px;">
                        <as:Literal runat="server" ID="uxCloseDate" meta:resourcekey="uxCloseDateResource1" />
                    </td>
                </tr>
                <tr>
                    <td class="strong"></td>
                    <td>&nbsp;</td>
                    <as:Panel runat="server" ID="uxPanel2" Visible="false" meta:resourcekey="uxPanel2Resource1">
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                    </as:Panel>
                    <td>&nbsp;</td>
                    <td style="padding-left: 5px; padding-top: 7px;" class="strong" align="left">
                        <as:Literal ID="ltLastUpdated" runat="server" Text="Last Updated:" meta:resourcekey="ltLastUpdatedResource1"></as:Literal>
                    </td>
                    <td style="padding-left: 5px; padding-top: 7px;">
                        <as:Literal runat="server" ID="uxLastUpdated" meta:resourcekey="uxLastUpdatedResource1" />
                    </td>
                </tr>
            </table>

        </div>
    </as:Container>
    <br />
    <br />
    <uc:MIF_FDR ID="uxMerchantInfo_FDR" runat="server" Visible="False" IsCaseManagement="true" />
    <uc:MIF_PLANET ID="uxMerchantInfo_PLANET" runat="server" Visible="False" IsCaseManagement="true" />
    <uc:MIF_TSYS ID="uxMerchantInfo_TSYS" runat="server" Visible="False" IsCaseManagement="true" />
    <uc:MIF_ORION ID="uxMerchantInfo_ORION" runat="server" Visible="False" IsCaseManagement="true" />

    <br style="clear: both;" />
    <br />
    <as:Container ID="asContainer2" runat="server" HeaderText="Issue(s)" Width="100%"
        TemplateName="ascontainer_whiteborder.tpl" FooterControlID="" FooterText="" HeaderControlID="" meta:resourcekey="asContainer2Resource1">
        <table cellpadding="0" border="0" width="100%" cellspacing="5">
            <tr>
                <td class="strong bottomLine" style="padding-left: 5px; width: 33%">
                    <as:Literal ID="ltCustomerService" runat="server" Text="Customer Service" meta:resourcekey="ltCustomerServiceResource1"></as:Literal>
                </td>
                <td class="strong bottomLine" style="padding-left: 5px; width: 33%">
                    <as:Literal ID="ltActivationOutcome" runat="server" Text="Activation Outcome" meta:resourcekey="ltActivationOutcomeResource1"></as:Literal>
                </td>
                <td class="strong bottomLine" style="padding-left: 5px; width: 34%">
                    <as:Literal ID="ltTerminalDownload" runat="server" Text="Terminal Download" meta:resourcekey="ltTerminalDownloadResource1"></as:Literal>
                </td>
            </tr>
            <tr>
                <td valign="top" style="padding-left: 5px">
                    <as:RadTreeView ID="uxTreeCS" runat="server" CheckBoxes="True" OnClientNodeCollapsing="onNodeCollapsing"
                        OnClientNodeChecked="doNodeCheck" OnClientMouseOver="onNodeMouseOver" OnClientNodeClicked="onNodeClicked" meta:resourcekey="uxTreeCSResource1">
                    </as:RadTreeView>
                </td>
                <td valign="top" style="padding-left: 5px">
                    <as:RadTreeView ID="uxTreeAO" runat="server" CheckBoxes="True" OnClientNodeCollapsing="onNodeCollapsing"
                        OnClientNodeChecked="doNodeCheck" OnClientMouseOver="onNodeMouseOver" OnClientNodeClicked="onNodeClicked" meta:resourcekey="uxTreeAOResource1">
                    </as:RadTreeView>
                </td>
                <td valign="top" style="padding-left: 5px">
                    <as:RadTreeView ID="uxTreeTD" runat="server" CheckBoxes="True" OnClientNodeCollapsing="onNodeCollapsing"
                        OnClientNodeChecked="doNodeCheck" OnClientMouseOver="onNodeMouseOver" OnClientNodeClicked="onNodeClicked" meta:resourcekey="uxTreeTDResource1">
                    </as:RadTreeView>
                </td>
            </tr>
        </table>
    </as:Container>
    <br style="clear: both;" />
    <as:Container ID="asContainer3" runat="server" HeaderText="Comments" Width="100%"
        TemplateName="ascontainer_whiteborder.tpl" FooterControlID="" FooterText="" HeaderControlID="" meta:resourcekey="asContainer3Resource1">
        <div style="background-color: White; padding: 0 10px 0 10px;">
            <div style="font-weight: bold;">
                <as:Literal ID="ltCommentHistory" runat="server" Text="Comment History:" meta:resourcekey="ltCommentHistoryResource1"></as:Literal>
            </div>
            <as:ASGrid ID="uxCommentList" runat="server" GridLines="None" Width="100%" AllowPaging="true"
                AllowSorting="true"
                ASPagingMethod="SPASingleMethod" AutoGenerateColumns="false" meta:resourcekey="uxCommentListResource1">
                <MasterTableView>
                    <Columns>
                        <as:ASGridBoundColumn SortExpression="CreatedDate" UniqueName="CreatedDate" ItemStyle-HorizontalAlign="Center"
                            HeaderText="Date/Time" DataField="CreatedDate" ASFormat="None" DataFormatString="{0:MM/dd/yyyy h:mm:ss tt}"
                            HeaderTooltip="Date/Time" meta:resourcekey="ASGridBoundColumnResource1">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text=""></ModelErrorMessage>
                            </ColumnValidationSettings>

                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                        </as:ASGridBoundColumn>
                        <as:ASGridBoundColumn SortExpression="Author" UniqueName="Author" HeaderText="Posted By"
                            ItemStyle-HorizontalAlign="Center" DataField="Author" HeaderTooltip="Posted By" meta:resourcekey="ASGridBoundColumnResource2">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text=""></ModelErrorMessage>
                            </ColumnValidationSettings>

                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                        </as:ASGridBoundColumn>
                        <as:ASGridBoundColumn SortExpression="CommentText" UniqueName="CommentText" HeaderText="Comment"
                            DataField="CommentText" ItemStyle-HorizontalAlign="Left" HeaderTooltip="Comment" meta:resourcekey="ASGridBoundColumnResource3">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text=""></ModelErrorMessage>
                            </ColumnValidationSettings>

                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                        </as:ASGridBoundColumn>
                    </Columns>
                </MasterTableView>
            </as:ASGrid>
            <br />
            <as:Panel ID="uxCommentPanel" runat="server" meta:resourcekey="uxCommentPanelResource1">
                <div style="font-weight: bold;">
                    <as:Literal ID="ltComments" runat="server" Text="Comments:" meta:resourcekey="ltCommentsResource1"></as:Literal>
                </div>
                <as:TextBox ID="uxCommentText" runat="server" TextMode="MultiLine" MaxLength="7000"
                    onkeyup="callBack()" Width="98%" Height="120" meta:resourcekey="uxCommentTextResource1"></as:TextBox>
                <div id="uxTextLimit">
                    <as:Literal ID="ltTexLimit" runat="server" Text="You have 7000 characters remaining for your message." meta:resourcekey="ltTexLimitResource1"> </as:Literal>
                </div>
            </as:Panel>
        </div>
    </as:Container>
    <br />
    <div style="text-align: right">
        <as:Button runat="server" ID="uxSave" Text="Submit" OnClick="uxSave_Click" OnClientClick="return doValidation();" IsStandardButton="False" meta:resourcekey="uxSaveResource1" />
    </div>
    <br />
    <tek:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <style type="text/css">
            .strong {
                font-weight: bold;
            }

            .bottomLine {
                border-bottom: solid 2px #000;
            }

            div.RadTreeView .rtMinus, div.RadTreeView .rtPlus {
                display: none;
            }

            div.RadTreeView .rtTop, div.RadTreeView .rtMid, div.RadTreeView .rtBot {
                padding: 0;
            }

            .RadTreeView_Default_disabled .rtIn, .RadTreeView_Default .rtDisabled .rtIn {
                color: #9F9F9F;
            }

            .RadTreeView .rtUL div.rtSelected {
                background-color: White !important;
            }

                .RadTreeView .rtUL div.rtSelected .rtIn {
                    border: 0 !important;
                    margin-left: 2px;
                    background-color: White !important;
                    color: Black !important;
                }
        </style>
        <script type="text/javascript" language="javascript">
            var _textFromRes = '<%= GetLocalResourceObject("CaseManagement_js_RemainingCharacter").ToString() %>';
            function goToCaseSearch() {
                self.location = "CaseSearch.aspx";
            }

            function goToCaseSearch() {
                self.location = "CaseSearch.aspx";
            }

            function onNodeClicked(sender, args) {
                var node = args.get_node();
                if (node.get_checked()) {
                    node.uncheck();
                }
                else {
                    node.check();
                }
                doNodeCheck(sender, args);
            }

            function onNodeMouseOver(sender, args) {
                args.get_node().unhighlight();
            }

            function onNodeCollapsing(sender, args) {
                var node = args.get_node();
                if (node.get_level() == 0)
                    args.set_cancel(true);
            }

            function doNodeCheck(sender, args) {
                var node = args.get_node();
                if (node.get_level() == 0) {  //   root node
                    if (node.get_checked()) { // if checked, enable all child nodes if any
                        for (var i = 0; i < node.get_nodes().get_count() ; i++) {
                            node.get_nodes().getNode(i).enable();

                        }
                    }
                    else {  // if unchecked, disable and uncheck all child nodes if any
                        for (var i = 0; i < node.get_nodes().get_count() ; i++) {
                            node.get_nodes().getNode(i).uncheck();
                            node.get_nodes().getNode(i).disable();

                        }
                    }
                }

            }
            function doValidation() {
                return checkComment();
            }

            function resetStatus() {
                $('#uxTextLimit').html('<%= GetLocalResourceObject("CaseManagement_aspx_7000").ToString() %>');

            }
            function checkComment() {
                if ($('#<%= uxCommentText.ClientID %>').val().length > 7000) {
                    alert(String.format('<%= GetLocalResourceObject("CaseManagement_aspx_Comment").ToString() %>' + ': ' + '<%=Resources.MessageManager.Generic_FieldLengthExceeded%>', 7000));
                    return false;
                }
                //__doPostBack('ctl00$ContentPage$btnAddComment', '')
                return true;
            }
            function callBack() {
                limitChars('<%= uxCommentText.ClientID %>', 7000, 'uxTextLimit');
            }
            
            function limitChars(textid, limit, infodiv) {
                var text = $('#' + textid).val();
                var textlength = text.length;
                if (textlength > limit) {
                    $('#' + infodiv).html('0');
                    $('#' + textid).val(text.substr(0, limit));
                    alert(String.format('<%= GetLocalResourceObject("CaseManagement_aspx_Comment").ToString() %>' + ': ' + '<%=Resources.MessageManager.Generic_FieldLengthExceeded%>', 7000));
                    return false;
                }
                else {
                    $('#' + infodiv).html(_textFromRes.replace('[REMAINING_NUMBER]',parseInt(limit - textlength)));
                }
            }

            function OpenMessageWindow(url) {
                ShowPopupModal(url, 'auto');
            }
        </script>
    </tek:RadCodeBlock>
</asp:Content>

