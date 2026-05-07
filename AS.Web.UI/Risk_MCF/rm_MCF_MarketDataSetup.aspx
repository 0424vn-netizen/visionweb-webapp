<%@ Page Title="Market Data Setup" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="rm_MCF_MarketDataSetup.aspx.cs" Inherits="rm_MCF_MarketDataSetup" meta:resourcekey="PageResource1" %>

<%@ Register TagName="PageTile" Src="~/UserControls/PageTitle.ascx" TagPrefix="uc" %>
<%@ Register TagName="UxParameter" Src="~/UserControls/rm_MCF_Parameter_MarketData.ascx" TagPrefix="uc" %>
<%--<%@ Register TagName="SiteID_Selector" Src="~/UserControls/SiteID_Selector.ascx" TagPrefix="uc" %>--%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .rgGroupCol {
            background-color: Transparent !important;
            border-left: solid 1px #7E7E7E !important;
        }

        td.rgGroupCol, th.rgGroupCol {
            padding-left: 4px !important;
            padding-right: 4px !important;
        }

        .RadGrid td > input {
            margin-right: 18px !important;
        }

        .RadGrid_Default .rgMasterTable td.rgGroupCol, .RadGrid_Default .rgMasterTable td.rgExpandCol {
            background: none !important;
        }

        .RadGrid .rgDetailTable .rgRow td {
            border-width: 0px !important;
            padding: 0 !important;
        }

        .rgGroupCol {
            border-left: 0px !important;
        }

        .RadGrid_Default .rgMasterTable td.rgGroupCol, .RadGrid_Default .rgMasterTable td.rgExpandCol {
            border: 0 none !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="Server">
    <style type="text/css">
        .rgAltRow .rgDetailTable, .rgRow .rgDetailTable {
            background-color: White;
            border-width: 1px !important;
            border-collapse: separate !important;
        }
    </style>
    <as:RadAjaxManagerProxy ID="ram" runat="server">
        <AjaxSettings>
            <tek:AjaxSetting AjaxControlID="uxParam">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="uxParam" />
                </UpdatedControls>
            </tek:AjaxSetting>
        </AjaxSettings>
        <AjaxSettings>
            <tek:AjaxSetting AjaxControlID="uxBntSave">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="uxParam" />
                </UpdatedControls>
            </tek:AjaxSetting>
        </AjaxSettings>
    </as:RadAjaxManagerProxy>
    <as:PlaceHolder ID="sds" runat="server">
        <uc:PageTile ID="uxPageTitle" runat="server" meta:resourcekey="uxPageTitleResource1" ReportTitle="RISK MANAGEMENT - ASSIGNMENTS - MARKET DATA SETUP" />
        <%--<uc:SiteID_Selector runat="server" ID="uxSiteIDSelector"/>--%>
        <br />
        <div class="PanelGreyPadding" style="font-style: italic">
            <div>
                <asp:Literal ID="Literal1" runat="server" meta:resourcekey="Literal1Resource1" Text="
            To update each market data element's risk parameters, select the market data from
            the dropdown, update the risk parameters and click the &quot;Save&quot; button provided to
            save the information for the chosen market data."></asp:Literal>
            </div>
            <br />
            <div>
                <as:RadComboBox runat="server" ID="uxComboMarketDataType" Width="300px" MaxHeight="250"
                    AutoPostBack="true" Height="250" OnSelectedIndexChanged="uxComboMarketDataType_SelectedIndexChanged" meta:resourcekey="uxComboMarketDataTypeResource1" />
            </div>
        </div>
        <br />
        <br />
        <as:Panel ID="FirstFilter" runat="server" meta:resourcekey="FirstFilterResource1">
            <table>
                <tr>
                    <td>
                        <as:CheckBox runat="server" ID="uxChbRiskScore" Checked="false" onclick="uxChbRiskScore_CheckChanged(this);" meta:resourcekey="uxChbRiskScoreResource1" />
                    </td>
                    <td>
                        <asp:Literal ID="Literal2" runat="server" meta:resourcekey="Literal2Resource1">Risk Score</asp:Literal>
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
                        <asp:Literal ID="Literal3" runat="server" meta:resourcekey="Literal3Resource1"> From</asp:Literal>
                    </td>
                    <td style="padding-left: 10px;">
                        <as:RadNumericTextBox ID="txtRiskScoreFrom" runat="Server" Type="Number" MaxLength="9" Height="15px" LabelCssClass="" meta:resourcekey="txtRiskScoreFromResource1">
                            <ClientEvents OnKeyPress="RadNumericTextBox_KeyPress" />
                            <NegativeStyle Resize="None"></NegativeStyle>

                            <NumberFormat PositivePattern="n" DecimalDigits="0" />

                            <EmptyMessageStyle Resize="None"></EmptyMessageStyle>

                            <ReadOnlyStyle Resize="None"></ReadOnlyStyle>

                            <FocusedStyle Resize="None"></FocusedStyle>

                            <DisabledStyle Resize="None"></DisabledStyle>

                            <InvalidStyle Resize="None"></InvalidStyle>

                            <HoveredStyle Resize="None"></HoveredStyle>

                            <EnabledStyle Resize="None"></EnabledStyle>
                        </as:RadNumericTextBox>
                    </td>
                    <td style="padding-left: 30px;">To
                    </td>
                    <td style="padding-left: 10px;">
                        <as:RadNumericTextBox ID="txtRiskScoreTo" runat="Server" Type="Number" MaxLength="9" Height="15px" LabelCssClass="" meta:resourcekey="txtRiskScoreToResource1">
                            <ClientEvents OnKeyPress="RadNumericTextBox_KeyPress" />
                            <NegativeStyle Resize="None"></NegativeStyle>

                            <NumberFormat PositivePattern="n" DecimalDigits="0" />

                            <EmptyMessageStyle Resize="None"></EmptyMessageStyle>

                            <ReadOnlyStyle Resize="None"></ReadOnlyStyle>

                            <FocusedStyle Resize="None"></FocusedStyle>

                            <DisabledStyle Resize="None"></DisabledStyle>

                            <InvalidStyle Resize="None"></InvalidStyle>

                            <HoveredStyle Resize="None"></HoveredStyle>

                            <EnabledStyle Resize="None"></EnabledStyle>
                        </as:RadNumericTextBox>
                    </td>
                </tr>
            </table>
            <table cellpadding="0" cellspacing="0">
                <tr>
                    <td style="padding-left: 7px;">
                        <asp:Literal ID="Literal4" runat="server" meta:resourcekey="Literal4Resource1">  Match All Parameters</asp:Literal>
                    </td>
                    <td style="padding-left: 20px;">
                        <as:RadioButton ID="radioNo" runat="server" Checked="true" GroupName="MatchAll" Text="No" meta:resourcekey="radioNoResource1" />
                    </td>
                    <td style="padding-left: 10px;">
                        <as:RadioButton runat="server" ID="radioYes" GroupName="MatchAll" Text="Yes" meta:resourcekey="radioYesResource1" />
                    </td>
                </tr>
            </table>
        </as:Panel>

        <uc:UxParameter ID="uxParam" runat="Server" ShowRowNumberColumn="false" OnNeedDataSource="uxParam_NeedDataSource"
            OnNeedExportConfig="uxParam_NeedExportConfig" ShowExport="true" TempDisableDuplicates="false"
            OnProcessSave="uxParam_ProcessSave" />
        <br />
        <br />
        <br />
        <br />
        <div style="position: fixed; z-index: 100; width: 100%; bottom: 0px; height: 30px;"
            id="pnStatus">
            <div style="background-color: #a6a6a6; height: 50px; margin-top: -8px;" id="uxFooter" runat="server">
                <div style="padding: 8px 10px;" align="right">
                    <as:Button ID="uxBntValidate" runat="server" Text="Save" OnClientClick="checkValidData(); return false;" IsStandardButton="False" meta:resourcekey="uxBntValidateResource1" />
                </div>
            </div>
        </div>

        <div style="visibility: hidden; display: none;">
            <as:Button ID="uxBtnSave" runat="server" OnClick="uxBntSave_Click" IsStandardButton="False" meta:resourcekey="uxBtnSaveResource1" />
            <as:Button ID="uxBntSaveFirtFilter" runat="server" OnClick="uxBntSaveFirtFilter_Click" IsStandardButton="False" meta:resourcekey="uxBntSaveFirtFilterResource1" />
        </div>
    </as:PlaceHolder>
    <as:RadCodeBlock ID="RadCodeBlock1" runat="server">     
        <script language="javascript" type="text/javascript" src="../res/js/common/riskparameter.js"></script>
        <script language="javascript" type="text/javascript">
            var msgAlert1 = '<%= GetLocalResourceObject("msgAlert1").ToString() %>';
            var msgAlert2 = '<%= GetLocalResourceObject("msgAlert2").ToString() %>';
            var msgAlert3 = '<%= GetLocalResourceObject("msgAlert3").ToString() %>';
            var msgAlert4 = '<%= GetLocalResourceObject("msgAlert4").ToString() %>';
            function ajaxRequestStart(sender, args) {
                UxExporter_OnRequestStart(sender, args);
            }
            function ajaxOnResponseEnd(sender, args) {
                UxExporter_OnResponseEnd(sender, args);
            }

            function checkValidData() {
                var isChecked = validateRiskScoreCheck();
                if (isChecked) {
                    var validRC = validateRiskScoreValues();
                    if (!validRC)
                        return;
                }
                document.getElementById("<%= uxBtnSave.ClientID %>").click();
            }

            function btnSaveClick(AssignemntOfMarketData) {
                var isChecked = validateRiskScoreCheck();
                /*if (isChecked) {
                    //validate riskscore
                    var validRC = validateRiskScoreValues();
                    if (!validRC)
                        return false;
                }*/
                if (AssignemntOfMarketData != '') {
                    var pm = checkParameterValid1();
                    if (!isChecked && !pm) {
                        alert(msgAlert1+' :\n' +
                        '\t' + AssignemntOfMarketData + '\n' +
                        ' ' + msgAlert2)
                        return false;
                    }
                }
                document.getElementById("<%= uxBntSaveFirtFilter.ClientID %>").click();
                doSaveParameterList(false);
            }

            function displayRiskScore() {
                var chk = $get("<%=uxChbRiskScore.ClientID %>");
                var txtFrom = $find("<%=txtRiskScoreFrom.ClientID %>");
                var txtTo = $find("<%=txtRiskScoreTo.ClientID %>");
                if (chk != null && chk.checked) {
                    txtFrom.enable();
                    txtTo.enable();
                } else {
                    txtFrom.disable();
                    txtTo.disable();
                }
            }

            function CheckMatchAllParameter(value) {
                if (value) {
                    document.getElementById("radioYes.ClientID").checked = true;
                }
                else {
                    document.getElementById("radioYes.ClientID").checked = false;
                }
            }
            function uxChbRiskScore_CheckChanged(object) {
                if (object.checked) {
                    var txtFrom = $find("<%=txtRiskScoreFrom.ClientID %>");
                    var txtTo = $find("<%=txtRiskScoreTo.ClientID %>");
                    txtFrom.enable();
                    txtTo.enable();
                }
                else {
                    var txtFrom = $find("<%=txtRiskScoreFrom.ClientID %>");
                    var txtTo = $find("<%=txtRiskScoreTo.ClientID %>");

                    txtFrom.clear();
                    txtTo.clear();

                    txtFrom.disable();
                    txtTo.disable();
                }
            }

            function validateRiskScoreCheck() {
                var chk = $get("<%=uxChbRiskScore.ClientID %>");
                if (chk != null && chk.checked) {
                    return true;
                }
                return false;
            }

            function validateRiskScoreValues() {
                var chk = $get("<%=uxChbRiskScore.ClientID %>");
                if (chk != null && chk.checked) {
                    var rcFrom = $find("<%=txtRiskScoreFrom.ClientID %>");
                    var rcTo = $find("<%=txtRiskScoreTo.ClientID %>");

                    if (rcFrom.get_value() < 0) {
                        alert(ERR_NUMONLY);
                        rcFrom.focus();
                        return false;
                    }

                    if (rcTo.get_value() < 0) {
                        alert(ERR_NUMONLY);
                        rcTo.focus();
                        return false;
                    }


                    if (rcFrom.get_textBoxValue().trim() != "" && rcTo.get_textBoxValue().trim() != "") {
                        if (rcFrom.get_value() > rcTo.get_value())
                            alert(ERR_TOLOWERFROM);

                        return (rcFrom.get_value() <= rcTo.get_value());
                    }
                    else {
                        if (rcFrom.get_textBoxValue().trim() == "") {
                            alert(msgAlert3 + ERR_REQUIREDFIELD);
                            rcFrom.focus();
                        }
                        else if (rcTo.get_textBoxValue().trim() == "") {
                            alert(msgAlert4 + ERR_REQUIREDFIELD);
                            rcTo.focus();
                        }

                        return false;
                    }
                }

                return true;
            }

            function check_RiskScoreChecked() {
                var chk = $get("<%=uxChbRiskScore.ClientID %>");
                if (chk != null && chk.checked) { return true; }
                else return false;
            }
        </script>
    </as:RadCodeBlock>
</asp:Content>

