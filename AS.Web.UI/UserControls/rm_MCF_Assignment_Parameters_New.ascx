<%@ Control Language="C#" AutoEventWireup="true" CodeFile="rm_MCF_Assignment_Parameters_New.ascx.cs" Inherits="UserControls_rm_MCF_Assignment_Parameters_New" %>
<%@ Register TagName="UxExport" Src="~/UserControls/UxExport.ascx" TagPrefix="uc" %>
<%@ Register TagName="UxTransCodeFilter" Src="~/UserControls/rm_MCF_ParameterFilter_TransactionCode.ascx" TagPrefix="uc" %>
<%@ Register TagName="UxACHReturnCodeFilter" Src="~/UserControls/rm_MCF_ParameterFilter_ACHReturnCode.ascx" TagPrefix="uc" %>

<as:Literal ID="lblMessage" runat="server" Visible="False" meta:resourcekey="lblMessageResource1"></as:Literal>
<as:Validator ID="ParameterFilterValidator" runat="server" MessageType="Inline"
    ValidationFunction="validateRiskScoreValues" MessageContainerClientID="" meta:resourcekey="ParameterFilterValidatorResource1">
    <Items>
        <as:BasicValidationItem Rule="Number" ResMessage="Resources.ValMsg.Number"
            ControlToValidateID="txtRCFrom" />
        <as:CustomValidationItem ResMessage="Resources.ValMsg.To_Greater" ClientValidationFunction="ValidateRiskScoreFromTo" ControlToValidateID="txtRCFrom" />
        <as:CustomValidationItem ResMessage="Resources.LanguageResource.RiskScoreFromRequired" ClientValidationFunction="ValidateRiskScoreFromRequired" ControlToValidateID="txtRCFrom" />
        <as:CustomValidationItem ResMessage="Resources.LanguageResource.RiskScoreToRequired" ClientValidationFunction="ValidateRiskScoreToRequired" ControlToValidateID="txtRCTo" />
    </Items>
</as:Validator>

<div class="row">
    <div class="col-md-12" data-toggle="collapse" data-target="#uxAssignmentParamsGrid">
        <h2 class="grid-title">
            <%= Mode == WebSiteEnums.ParamFilterMode.Assignment ? (GetLocalResourceObject("Risk_Assignment_Parameters_ascx_Assignment").ToString() + " " + GetLocalResourceObject("Risk_Assignment_Parameters_ascx_cs_Parameters").ToString()) : (GetLocalResourceObject("Risk_Assignment_Parameters_ascx_Adhoc").ToString() + " " + GetLocalResourceObject("Risk_Assignment_Parameters_ascx_cs_Parameters").ToString())%> 
        </h2>
    </div>
</div>
<div class="row" id="ucParameterType">
    <div class="col-md-12">
        <div class="control-inline ">
            <as:RadioButton ID="rdMatchSpecific" runat="server" TabIndex="4" GroupName="rdMatch" Text="Match specific parameters" tracking-key="MatchAParameterOrGroup" tracking-type="radio" tracking-required="true"
                Checked="True" meta:resourcekey="rdMatchSpecificResource1" OnCheckedChanged="rdMatch_CheckedChanged" Value="Specific" AutoPostBack="true" />
        </div>
        <div class="control-inline">
            <as:RadioButton ID="rdMatchAll" runat="server" TabIndex="4" GroupName="rdMatch" tracking-key="MatchEveryParameter" tracking-type="radio" tracking-required="true"
                Text="Match all parameters" meta:resourcekey="rdMatchAllResource1" OnCheckedChanged="rdMatch_CheckedChanged" Value="All" AutoPostBack="true" />
        </div>
    </div>
</div>

<as:PlaceHolder ID="uxPhRiskScore" runat="server">
    <div class="mt-5x mb-6x" id="ucParameterRiskScore">
        <as:CheckBox ID="chkRiskScore" tracking-required="true" tracking-key="RiskScore" tracking-type="checkbox" runat="server" onclick="chbRiskScore_click();" meta:resourcekey="chkRiskScoreResource1" />
        <as:ValidatorLabel ID="VltLblRiskScore" runat="server" Text="Risk Score" CssClass="ml-1x" ApplyFor="txtRCFrom" meta:resourcekey="VltLblRiskScoreResource1"></as:ValidatorLabel>
        <div class="control-inline">
            <label>
                <as:Literal ID="Literal3" runat="server" Text="From" meta:resourcekey="lblFromResource"></as:Literal>
            </label>
            <as:ASRadNumericTextBox ID="txtRCFrom" tracking-key="RiskScoreFrom" runat="Server" Type="Number" MaxLength="9" Width="80px" CssClass="form-control">
                <NumberFormat PositivePattern="n" NegativePattern="n" DecimalDigits="0" />
            </as:ASRadNumericTextBox>
        </div>
        <div class="control-inline">
            <label>
                <as:Literal ID="Literal4" runat="server" Text="To" meta:resourcekey="lbToResource"></as:Literal></label>
            <as:ASRadNumericTextBox ID="txtRCTo" tracking-key="RiskScoreTo" runat="Server" Type="Number" MaxLength="9" Width="80px" CssClass="form-control">
                <NumberFormat PositivePattern="n" NegativePattern="n" DecimalDigits="0" />
            </as:ASRadNumericTextBox>
        </div>
        <div class="bottom-error">
            <as:ValidatorMessage runat="server" ID="txtRCFromMsg" ApplyFor="txtRCFrom" Message="" ShowOnLoad="False" />
            <as:ValidatorMessage runat="server" ID="txtRCToMsg" ApplyFor="txtRCTo" Message="" ShowOnLoad="False" />
        </div>
    </div>
</as:PlaceHolder>

<div class="row">
    <div class="col-md-12 in" id="uxAssignmentParamsGrid">
        <div class="header-card group-nrt">
            <div class="item w-checkbox j-center "></div>
            <div class="groups">
                <div class="group">
                    <div class="item flex-grow-1 item-nrt"></div>
                    <div class="item w-control-center header-group-nrt"><%= GetLocalResourceObject("groupHeaderResource1.Text") %></div>
                    <div class="item w-control-center header-group-nrt"><%= GetLocalResourceObject("groupHeaderResource2.Text") %></div>
                    <div class="item w-links item-nrt"></div>
                </div>
            </div>
        </div>
        <div class="header-card">
            <div id="divGroupTitle" class="item w-checkbox j-center "><%= GetLocalResourceObject("Literal2Resource1.Text") %></div>
            <div class="groups">
                <div class="group">
                    <div class="item flex-grow-1 item-nrt"><%= GetLocalResourceObject("Literal2Resource2.Text") %></div>
                    <div class="item w-control-center item-nrt content-center"><%= GetLocalResourceObject("Literal1Resource1.Text") %></div>
                    <div class="item w-control-center item-nrt content-center"><%= GetLocalResourceObject("ltThresholdResource1.Text") %></div>
                    <div class="item w-control-center item-nrt content-center"><%= GetLocalResourceObject("Literal1Resource1.Text") %></div>
                    <div class="item w-control-center item-nrt content-center"><%= GetLocalResourceObject("ltThresholdResource1.Text") %></div>
                    <div class="item w-links item-nrt"><%= GetLocalResourceObject("Literal6Resource1.Text") %></div>
                </div>
            </div>
        </div>
        <div id="uxParameterContent">
            <asp:Repeater ID="uxGroupParameterRepeater" runat="server" OnItemDataBound="uxGroupParameterRepeater_ItemDataBound">
                <ItemTemplate>
                    <div runat="server" id="divGroup" class="row-card">
                        <div class="item w-checkbox j-center <%# MatchAllMode %>">
                            <as:CheckBox ID="chkParam" runat="server" onclick="onCheckValidGroup(this)" meta:resourcekey="chkGroupResource1" />
                        </div>
                        <div class="groups">
                            <asp:Repeater ID="uxParameterInGroupRepeater" runat="server" OnItemDataBound="uxParameterInGroupRepeater_ItemDataBound">
                                <ItemTemplate>
                                    <div id='param_<%# GetValue(Eval("ParameterKey")) %>' data-key="<%# WebServices.SecurityServices.EncryptText(GetValue(Eval("ParameterKey"))) %>" data-order="<%# GetValue(Eval("RowNumber")) %>" data-is-source="<%# GetValue(Eval("IsSourceParam")) %>" data-is-model-type="<%# !string.IsNullOrEmpty(GetValue(Eval("FilterTypeModal"))) ? "true" : "false" %>" class="group">
                                        <as:HiddenField ID="uxCriteriaValueString" runat="server" Value='<%# GetValue(Eval("ParameterKey")) %>' />
                                        <as:HiddenField ID="uxParameterPrecision" runat="server" Value='<%# GetValue(Eval("ParameterPrecision")) %>' />
                                        <as:HiddenField ID="uxIsThresholdNegative" runat="server" Value='<%# Eval("IsThresholdNegative") %>' />
                                        <as:HiddenField ID="uxIsIndicatorNagative" runat="server" Value='<%# Eval("IsIndicatorNegative") %>' />
                                        <as:HiddenField ID="uxParameterThresholdType" runat="server" Value='<%# Eval("ParameterThresholdType") %>' />
                                        <div class="item flex-grow-1 item-nrt parameter">
                                            <span title='<%# AS.Common.VeraCodeSolution.DoVeraCode(System.Web.HttpUtility.HtmlEncode(Eval("ParameterDescription").ToString())) %>'><%# Eval("ParameterName")%></span>
                                            <as:PlaceHolder runat="server" ID="uxSpecialPanel" Visible="False">
                                                <div class="risklightgrayrow child-item">
                                                    <img src="../res/images/icon_subitem.png" />
                                                    <as:Container ID="asContainer" runat="server" Width="100%" HeaderText="ReasonCode"
                                                        TemplateName="riskparamfilterbox_new.tpl" FooterControlID="" FooterText="" HeaderControlID="">
                                                    </as:Container>
                                                </div>
                                            </as:PlaceHolder>
                                        </div>

                                        <div class="item w-control-center item-nrt">
                                            <as:PlaceHolder ID="uxIndicatorNomal" runat="server">
                                                <div class="control-center">
                                                    <label runat="server" id="lblIndicatorText">
                                                        <asp:Literal ID="lblIndicatorDollar" runat="server" Text="&nbsp;"></asp:Literal>
                                                    </label>
                                                    <div class="w-input">
                                                        <as:ASRadNumericTextBox NumberFormat-AllowRounding="false" NumberFormat-KeepNotRoundedValue="true" NumberFormat-KeepTrailingZerosOnFocus="true"
                                                            ID="txtParameterValue" runat="server" Width="60px" text='<%# GetParameterNumber(Eval("ParameterValue").ToString()) %>' CssClass="form-control edit"
                                                            Culture="en-US" DbValueFactor="1" LabelCssClass="" LabelWidth="64px"
                                                            tracking-key='<%# string.Format("{0}_FAI", Eval("ParameterKey")) %>'
                                                            tracking-text='<%# string.Format("{0} {1}", Eval("ParameterKey"), "FirstAlertIndicator") %>'>
                                                            <NegativeStyle Resize="None" />
                                                            <NumberFormat PositivePattern="n" NegativePattern="n" DecimalDigits="0" ZeroPattern="n" />
                                                            <EmptyMessageStyle Resize="None" />
                                                            <ReadOnlyStyle Resize="None" />
                                                            <FocusedStyle Resize="None" />
                                                            <DisabledStyle Resize="None" />
                                                            <InvalidStyle Resize="None" />
                                                            <HoveredStyle Resize="None" />
                                                            <EnabledStyle Resize="None" />
                                                        </as:ASRadNumericTextBox>
                                                        <label class="non-edit" id="ucParameterValue" runat="server">
                                                            <as:Literal ID="lbParameterValue" runat="server" Text='<%# GetParameterNumberForLabel(Eval("ParameterValue").ToString(), Eval("ParameterPrecision").ToString()) %>' meta:resourcekey="lblParamValue"></as:Literal>
                                                        </label>
                                                        <as:Literal ID="lblIndicatorNA" runat="server" Text="" Visible="False" meta:resourcekey="lblIndicatorNAResource1_"></as:Literal>
                                                    </div>
                                                    <label>
                                                        <span id="spanErrMsg" class="error" runat="server"></span>
                                                        <span>
                                                            <as:Literal ID="lblParameterType" runat="server" Text='<%# GetValue(Eval("ParameterDataType")) %>'></as:Literal></span>
                                                    </label>
                                                </div>
                                            </as:PlaceHolder>
                                            <!-- Indicator FromTo-->
                                            <as:PlaceHolder ID="uxIndicatorHigh" runat="server" Visible="False">

                                                <div class="control-center">
                                                    <label class="mr-5">
                                                        <as:Literal ID="Literal41" runat="server" Text="From" meta:resourcekey="lblFromResource"></as:Literal></label>
                                                    <div class="w-input w-input">
                                                        <as:ASRadNumericTextBox NumberFormat-AllowRounding="false" NumberFormat-KeepNotRoundedValue="true" NumberFormat-KeepTrailingZerosOnFocus="true"
                                                            ID="txtFrom" runat="server" Text='<%# string.Format("{0:n0}", GetParameterNumber(Eval("ParameterValue").ToString())) %>'
                                                            Width="60px" CssClass="form-control edit" Culture="en-US" DbValueFactor="1" LabelCssClass="" LabelWidth="64px"
                                                            tracking-key='<%# string.Format("{0}_FAI", Eval("ParameterKey")) %>'
                                                            tracking-text='<%# string.Format("{0} {1}", Eval("ParameterKey"), "FirstAlertIndicator") %>'
                                                            tracking-refer="txtTo">
                                                            <NegativeStyle Resize="None" />
                                                            <NumberFormat PositivePattern="n" NegativePattern="n" DecimalDigits="0" ZeroPattern="n" />
                                                            <EmptyMessageStyle Resize="None" />
                                                            <ReadOnlyStyle Resize="None" />
                                                            <FocusedStyle Resize="None" />
                                                            <DisabledStyle Resize="None" />
                                                            <InvalidStyle Resize="None" />
                                                            <HoveredStyle Resize="None" />
                                                            <EnabledStyle Resize="None" />
                                                        </as:ASRadNumericTextBox>
                                                        <label class="non-edit" id="ucFrom" runat="server">
                                                            <as:Literal ID="lbFrom" runat="server" Text='<%# GetParameterNumberForLabel(Eval("ParameterValue").ToString(), Eval("ParameterPrecision").ToString()) %>' meta:resourcekey="lblFrom"></as:Literal>
                                                        </label>
                                                    </div>
                                                    <label></label>
                                                </div>
                                                <div class="control-center">
                                                    <label class="mr-5">
                                                        <as:Literal ID="Literal51" runat="server" Text="To" meta:resourcekey="lbToResource"></as:Literal></label>
                                                    <div class="w-input">
                                                        <as:ASRadNumericTextBox NumberFormat-AllowRounding="false" NumberFormat-KeepNotRoundedValue="true" NumberFormat-KeepTrailingZerosOnFocus="true"
                                                            ID="txtTo" runat="server" Text='<%# GetParameterNumber(Eval("ParameterValueHigh").ToString()) %>'
                                                            Width="60px" CssClass="form-control edit" Culture="en-US" DbValueFactor="1" LabelCssClass="" LabelWidth="64px"
                                                            refer-key='<%# string.Format("{0}_FAI", Eval("ParameterKey")) %>'>
                                                            <NegativeStyle Resize="None" />
                                                            <NumberFormat PositivePattern="n" NegativePattern="n" DecimalDigits="0" ZeroPattern="n" />
                                                            <EmptyMessageStyle Resize="None" />
                                                            <ReadOnlyStyle Resize="None" />
                                                            <FocusedStyle Resize="None" />
                                                            <DisabledStyle Resize="None" />
                                                            <InvalidStyle Resize="None" />
                                                            <HoveredStyle Resize="None" />
                                                            <EnabledStyle Resize="None" />
                                                        </as:ASRadNumericTextBox>
                                                        <label class="non-edit" id="ucTo" runat="server">
                                                            <as:Literal ID="lbTo" runat="server" Text='<%# GetParameterNumberForLabel(Eval("ParameterValueHigh").ToString()) %>' meta:resourcekey="lbTo"></as:Literal>
                                                        </label>
                                                    </div>
                                                    <label>
                                                        <as:Literal ID="Literal2" runat="server" Text='<%# GetValue(Eval("ParameterDataType")) %>'></as:Literal>
                                                    </label>
                                                </div>
                                            </as:PlaceHolder>
                                            <div class="msgError error text-space text-left"></div>
                                        </div>

                                        <div class="item w-control-center item-nrt">
                                            <as:PlaceHolder ID="lblNA" runat="server" Visible="False">
                                                <asp:Label ID="lblThresholdNA" runat="server" Text="" CssClass="thresholdNA">
                                                    <as:Literal ID="Literal71" runat="server" Text="" meta:resourcekey="Literal71Resource1_"></as:Literal>
                                                </asp:Label>
                                            </as:PlaceHolder>
                                            <as:PlaceHolder ID="uxThresholdNormal" runat="server">
                                                <div class="control-center">
                                                    <label>
                                                        <as:Literal ID="lblDollar" runat="server" Text="&nbsp;" meta:resourcekey="lblDollar71Resource1"></as:Literal>
                                                    </label>
                                                    <div class="w-input">
                                                        <as:ASRadNumericTextBox NumberFormat-AllowRounding="false" NumberFormat-KeepNotRoundedValue="true" NumberFormat-KeepTrailingZerosOnFocus="true"
                                                            ID="txtThreshold" runat="server" Text='<%# GetParameterNumber(Eval("ParameterThreshold").ToString()) %>'
                                                            Width="60px" CssClass="form-control edit" Culture="en-US" DbValueFactor="1" LabelCssClass="" LabelWidth="44px"
                                                            tracking-key='<%# string.Format("{0}_FAT", Eval("ParameterKey")) %>'
                                                            tracking-text='<%# string.Format("{0} {1}", Eval("ParameterKey"), "FirstAlertThreshold") %>'>
                                                            <NegativeStyle Resize="None" />
                                                            <NumberFormat PositivePattern="n" NegativePattern="n" DecimalDigits="0" ZeroPattern="n" />
                                                            <EmptyMessageStyle Resize="None" />
                                                            <ReadOnlyStyle Resize="None" />
                                                            <FocusedStyle Resize="None" />
                                                            <DisabledStyle Resize="None" />
                                                            <InvalidStyle Resize="None" />
                                                            <HoveredStyle Resize="None" />
                                                            <EnabledStyle Resize="None" />
                                                        </as:ASRadNumericTextBox>
                                                        <label class="non-edit" id="ucThreshold" runat="server">
                                                            <as:Literal ID="lbThreshold" runat="server" Text='<%# GetParameterNumber(Eval("ParameterThreshold").ToString()) %>' meta:resourcekey="lbThresholdResource"></as:Literal>
                                                        </label>
                                                    </div>
                                                    <label>
                                                        <span id="spanErrMsgThs" class="error" runat="server"></span>
                                                        <span>
                                                            <as:Literal ID="lblpercent" runat="server" Text='<%# GetValue(Eval("ThresholdType")) %>'></as:Literal></span>
                                                    </label>
                                                </div>
                                            </as:PlaceHolder>
                                            <as:PlaceHolder ID="uxThresholdLowHigh" runat="server" Visible="False">
                                                <div class="control-center">
                                                    <label class="mr-5">
                                                        <as:Literal ID="Literal8" runat="server" Text="Low" meta:resourcekey="Literal81Resource1"></as:Literal>&nbsp<as:Literal ID="lblDollarThresholdLow" runat="server" Text="&nbsp;" meta:resourcekey="lblDollarThresholdLow1Resource1"></as:Literal>
                                                    </label>
                                                    <div class="w-input">
                                                        <as:RadNumericTextBox NumberFormat-AllowRounding="false" NumberFormat-KeepNotRoundedValue="true" NumberFormat-KeepTrailingZerosOnFocus="true"
                                                            ID="txtThresholdLow" runat="server" Text='<%# GetParameterNumber(Eval("ParameterThreshold").ToString()) %>'
                                                            Width="60px" CssClass="form-control edit" Culture="en-US" DbValueFactor="1" LabelCssClass="" LabelWidth="64px"
                                                            tracking-key='<%# string.Format("{0}_FAT", Eval("ParameterKey")) %>'
                                                            tracking-text='<%# string.Format("{0} {1}", Eval("ParameterKey"),"FirstAlertThreshold") %>'
                                                            tracking-refer="txtThresholdHigh">

                                                            <NegativeStyle Resize="None" />
                                                            <NumberFormat PositivePattern="n" NegativePattern="n" DecimalDigits="0" ZeroPattern="n" />
                                                            <EmptyMessageStyle Resize="None" />
                                                            <ReadOnlyStyle Resize="None" />
                                                            <FocusedStyle Resize="None" />
                                                            <DisabledStyle Resize="None" />
                                                            <InvalidStyle Resize="None" />
                                                            <HoveredStyle Resize="None" />
                                                            <EnabledStyle Resize="None" />
                                                        </as:RadNumericTextBox>
                                                        <label class="non-edit" id="ucThresholdLow" runat="server">
                                                            <as:Literal ID="lbThresholdLow" runat="server" Text='<%# GetParameterNumberForLabel(Eval("ParameterThreshold").ToString(), Eval("ParameterPrecision").ToString()) %>' meta:resourcekey="lbThresholdResource"></as:Literal>
                                                        </label>
                                                    </div>
                                                    <label>
                                                        <span id="spanErrMsgThresholdLow" class="error" runat="server"></span>
                                                        <span>
                                                            <as:Literal ID="lblPercentThresholdLow" runat="server" Text='<%# GetValue(Eval("ThresholdType")) %>'></as:Literal></span>
                                                    </label>
                                                </div>
                                                <div class="control-center">
                                                    <label class="mr-5">
                                                        <as:Literal ID="Literal9" runat="server" Text="High" meta:resourcekey="Literal91Resource1"></as:Literal>&nbsp<as:Literal ID="lblDollarThresholdHigh" runat="server" Text="&nbsp;" meta:resourcekey="lblDollarThresholdHigh1Resource1"></as:Literal>
                                                    </label>
                                                    <div class="w-input">
                                                        <as:ASRadNumericTextBox NumberFormat-AllowRounding="false" NumberFormat-KeepNotRoundedValue="true" NumberFormat-KeepTrailingZerosOnFocus="true"
                                                            ID="txtThresholdHigh" runat="server" Text='<%# GetParameterNumber(Eval("ParameterThresholdHigh").ToString()) %>'
                                                            Width="60px" CssClass="form-control edit" Culture="en-US" DbValueFactor="1" LabelCssClass="" LabelWidth="44px"
                                                            refer-key='<%# string.Format("{0}_FAT", Eval("ParameterKey")) %>'>
                                                            <NegativeStyle Resize="None" />
                                                            <NumberFormat PositivePattern="n" NegativePattern="n" DecimalDigits="0" ZeroPattern="n" />
                                                            <EmptyMessageStyle Resize="None" />
                                                            <ReadOnlyStyle Resize="None" />
                                                            <FocusedStyle Resize="None" />
                                                            <DisabledStyle Resize="None" />
                                                            <InvalidStyle Resize="None" />
                                                            <HoveredStyle Resize="None" />
                                                            <EnabledStyle Resize="None" />
                                                        </as:ASRadNumericTextBox>
                                                        <label class="non-edit" id="ucThresholdHigh" runat="server">
                                                            <as:Literal ID="lbThresholdHigh" runat="server" Text='<%# GetParameterNumberForLabel(Eval("ParameterThresholdHigh").ToString(), Eval("ParameterPrecision").ToString()) %>' meta:resourcekey="lbThresholdResource"></as:Literal>
                                                        </label>
                                                    </div>
                                                    <label>
                                                        <span id="spanErrMsgThresholdHigh" class="error" runat="server"></span>
                                                        <span>
                                                            <as:Literal ID="lblPercentThresholdHigh" runat="server" Text='<%# GetValue(Eval("ThresholdType")) %>'></as:Literal></span>
                                                    </label>

                                                </div>
                                            </as:PlaceHolder>
                                            <div class="msgError error text-space text-left"></div>
                                        </div>

                                        <div class="item w-control-center item-nrt">
                                            <as:PlaceHolder ID="uxNRTIndicatorNomal" runat="server">
                                                <div class="control-center">
                                                    <label id="lblMCFIndicatorText" runat="server">
                                                        <asp:Literal ID="lblNRTIndicatorDollar" runat="server" Text="&nbsp;"></asp:Literal>
                                                    </label>
                                                    <div class="w-input">
                                                        <as:ASRadNumericTextBox NumberFormat-AllowRounding="false" NumberFormat-KeepNotRoundedValue="true" NumberFormat-KeepTrailingZerosOnFocus="true"
                                                            ID="txtNRTParameterValue" runat="server" Width="60px" text='<%# GetParameterNumber(Eval("ReAlertParameterIndicator").ToString()) %>' CssClass="form-control edit" Culture="en-US" DbValueFactor="1" LabelCssClass="" LabelWidth="64px"
                                                            tracking-key='<%# string.Format("{0}_RAI", Eval("ParameterKey")) %>'
                                                            tracking-text='<%# string.Format("{0} {1}", Eval("ParameterKey"),"Re-AlertIndicator") %>'>
                                                            <NegativeStyle Resize="None" />
                                                            <NumberFormat PositivePattern="n" NegativePattern="n" DecimalDigits="0" ZeroPattern="n" />
                                                            <EmptyMessageStyle Resize="None" />
                                                            <ReadOnlyStyle Resize="None" />
                                                            <FocusedStyle Resize="None" />
                                                            <DisabledStyle Resize="None" />
                                                            <InvalidStyle Resize="None" />
                                                            <HoveredStyle Resize="None" />
                                                            <EnabledStyle Resize="None" />
                                                        </as:ASRadNumericTextBox>
                                                        <label class="non-edit" id="ucNRTParameterValue" runat="server">
                                                            <as:Literal ID="lbNRTParameterValue" runat="server" Text='<%# GetParameterNumberForLabel(Eval("ReAlertParameterIndicator").ToString(), Eval("ParameterPrecision").ToString()) %>' meta:resourcekey="lblParamValue"></as:Literal>
                                                        </label>
                                                        <as:Literal ID="lblNRTIndicatorNA" runat="server" Text="" Visible="False" meta:resourcekey="lblIndicatorNAResource1"></as:Literal>
                                                    </div>
                                                    <label>
                                                        <span id="spanNRTErrMsg" class="error" runat="server"></span>
                                                        <span>
                                                            <as:Literal ID="lblNRTParameterType" runat="server" Text='<%# GetValue(Eval("ParameterDataType")) %>'></as:Literal></span>
                                                    </label>
                                                </div>
                                            </as:PlaceHolder>
                                            <!-- Indicator FromTo-->
                                            <as:PlaceHolder ID="uxNRTIndicatorHigh" runat="server" Visible="False">

                                                <div class="control-center">
                                                    <label>
                                                        <as:Literal ID="LiteralNRT41" runat="server" Text="From" meta:resourcekey="lblFromResource"></as:Literal></label>
                                                    <div class="w-input w-input">
                                                        <as:ASRadNumericTextBox NumberFormat-AllowRounding="false" NumberFormat-KeepNotRoundedValue="true" NumberFormat-KeepTrailingZerosOnFocus="true"
                                                            ID="txtNRTFrom" runat="server" Text='<%# string.Format("{0:n0}", GetParameterNumber(Eval("ReAlertParameterIndicator").ToString())) %>'
                                                            Width="60px" CssClass="form-control edit" Culture="en-US" DbValueFactor="1" LabelCssClass="" LabelWidth="64px"
                                                            tracking-key='<%# string.Format("{0}_RAI", Eval("ParameterKey")) %>'
                                                            tracking-text='<%# string.Format("{0} {1}", Eval("ParameterKey"), "Re-AlertIndicator") %>'
                                                            tracking-refer="txtNRTTo">
                                                            <NegativeStyle Resize="None" />
                                                            <NumberFormat PositivePattern="n" NegativePattern="n" DecimalDigits="0" ZeroPattern="n" />
                                                            <EmptyMessageStyle Resize="None" />
                                                            <ReadOnlyStyle Resize="None" />
                                                            <FocusedStyle Resize="None" />
                                                            <DisabledStyle Resize="None" />
                                                            <InvalidStyle Resize="None" />
                                                            <HoveredStyle Resize="None" />
                                                            <EnabledStyle Resize="None" />
                                                        </as:ASRadNumericTextBox>
                                                        <label class="non-edit" id="ucNRTFrom" runat="server">
                                                            <as:Literal ID="lbNRTFrom" runat="server" Text='<%# GetParameterNumberForLabel(Eval("ReAlertParameterIndicator").ToString(), Eval("ParameterPrecision").ToString()) %>' meta:resourcekey="lblFrom"></as:Literal>
                                                        </label>
                                                    </div>
                                                    <label></label>
                                                </div>
                                                <div class="control-center">
                                                    <label>
                                                        <as:Literal ID="LiteralNRT51" runat="server" Text="To" meta:resourcekey="lbToResource"></as:Literal></label>
                                                    <div class="w-input">
                                                        <as:ASRadNumericTextBox NumberFormat-AllowRounding="false" NumberFormat-KeepNotRoundedValue="true" NumberFormat-KeepTrailingZerosOnFocus="true"
                                                            ID="txtNRTTo" runat="server" Text='<%# GetParameterNumber(Eval("ReAlertParameterIndicatorHigh").ToString()) %>'
                                                            Width="60px" CssClass="form-control edit" Culture="en-US" DbValueFactor="1" LabelCssClass="" LabelWidth="64px"
                                                            refer-key='<%# string.Format("{0}_RAI", Eval("ParameterKey")) %>'>
                                                            <NegativeStyle Resize="None" />
                                                            <NumberFormat PositivePattern="n" NegativePattern="n" DecimalDigits="0" ZeroPattern="n" />
                                                            <EmptyMessageStyle Resize="None" />
                                                            <ReadOnlyStyle Resize="None" />
                                                            <FocusedStyle Resize="None" />
                                                            <DisabledStyle Resize="None" />
                                                            <InvalidStyle Resize="None" />
                                                            <HoveredStyle Resize="None" />
                                                            <EnabledStyle Resize="None" />
                                                        </as:ASRadNumericTextBox>
                                                        <label class="non-edit" id="ucNRTTo" runat="server">
                                                            <as:Literal ID="lbNRTTo" runat="server" Text='<%# GetParameterNumberForLabel(Eval("ReAlertParameterIndicatorHigh").ToString()) %>' meta:resourcekey="lbTo"></as:Literal>
                                                        </label>
                                                    </div>
                                                    <label>
                                                        <as:Literal ID="LiteralNRT61" runat="server" Text="day(s)" meta:resourcekey="Literal113Resource1"></as:Literal>
                                                    </label>
                                                </div>
                                            </as:PlaceHolder>
                                            <div class="msgError error text-space text-left"></div>
                                        </div>

                                        <div class="item w-control-center item-nrt">
                                            <as:PlaceHolder ID="lblNRTNA" runat="server" Visible="False">
                                                <asp:Label ID="lblMCFThresholdNA" runat="server" Text="" CssClass="thresholdNA">
                                                    <as:Literal ID="LiteralNRT71" runat="server" Text="" meta:resourcekey="Literal71Resource1_"></as:Literal>
                                                </asp:Label>
                                            </as:PlaceHolder>
                                            <as:PlaceHolder ID="uxNRTThresholdNormal" runat="server">
                                                <div class="control-center">
                                                    <label>
                                                        <as:Literal ID="lblNRTDollar" runat="server" Text="&nbsp;" meta:resourcekey="lblDollar71Resource1"></as:Literal>
                                                    </label>
                                                    <div class="w-input">
                                                        <as:ASRadNumericTextBox NumberFormat-AllowRounding="false" NumberFormat-KeepNotRoundedValue="true" NumberFormat-KeepTrailingZerosOnFocus="true"
                                                            ID="txtNRTThreshold" runat="server" Text='<%# GetParameterNumber(Eval("ReAlertParameterThreshold").ToString()) %>'
                                                            Width="60px" CssClass="form-control edit" Culture="en-US" DbValueFactor="1" LabelCssClass="" LabelWidth="44px"
                                                            tracking-key='<%# string.Format("{0}_RAT", Eval("ParameterKey")) %>'
                                                            tracking-text='<%# string.Format("{0} {1}", Eval("ParameterKey"), "Re-AlertThreshold") %>'>
                                                            <NegativeStyle Resize="None" />
                                                            <NumberFormat PositivePattern="n" NegativePattern="n" DecimalDigits="0" ZeroPattern="n" />
                                                            <EmptyMessageStyle Resize="None" />
                                                            <ReadOnlyStyle Resize="None" />
                                                            <FocusedStyle Resize="None" />
                                                            <DisabledStyle Resize="None" />
                                                            <InvalidStyle Resize="None" />
                                                            <HoveredStyle Resize="None" />
                                                            <EnabledStyle Resize="None" />
                                                        </as:ASRadNumericTextBox>
                                                        <label class="non-edit" id="ucNRTThreshold" runat="server">
                                                            <as:Literal ID="lbNRTThreshold" runat="server" Text='<%# GetParameterNumber(Eval("ReAlertParameterThreshold").ToString()) %>' meta:resourcekey="lbThresholdResource"></as:Literal>
                                                        </label>
                                                    </div>
                                                    <label>
                                                        <span id="spanNRTErrMsgThs" class="error" runat="server"></span>
                                                        <span>
                                                            <as:Literal ID="lblNRTpercent" runat="server" Text='<%# GetValue(Eval("ThresholdType")) %>'></as:Literal></span>
                                                    </label>
                                                </div>
                                            </as:PlaceHolder>
                                            <as:PlaceHolder ID="uxNRTThresholdLowHigh" runat="server" Visible="False">
                                                <div class="control-center">
                                                    <label>
                                                        <as:Literal ID="LiteralNRT8" runat="server" Text="Low" meta:resourcekey="Literal81Resource1"></as:Literal>&nbsp<as:Literal ID="lblNRTDollarThresholdLow" runat="server" Text="&nbsp;" meta:resourcekey="lblDollarThresholdLow1Resource1"></as:Literal>
                                                    </label>
                                                    <div class="w-input">
                                                        <as:RadNumericTextBox NumberFormat-AllowRounding="false" NumberFormat-KeepNotRoundedValue="true" NumberFormat-KeepTrailingZerosOnFocus="true"
                                                            ID="txtNRTThresholdLow" runat="server" Text='<%# GetParameterNumber(Eval("ReAlertParameterThreshold").ToString()) %>'
                                                            Width="60px" CssClass="form-control edit" Culture="en-US" DbValueFactor="1" LabelCssClass="" LabelWidth="64px"
                                                            tracking-key='<%# string.Format("{0}_RAT", Eval("ParameterKey")) %>'
                                                            tracking-text='<%# string.Format("{0} {1}", Eval("ParameterKey"),  "Re-AlertThreshold") %>'
                                                            tracking-refer="txtNRTThresholdHigh">
                                                            <NegativeStyle Resize="None" />
                                                            <NumberFormat PositivePattern="n" NegativePattern="n" DecimalDigits="0" ZeroPattern="n" />
                                                            <EmptyMessageStyle Resize="None" />
                                                            <ReadOnlyStyle Resize="None" />
                                                            <FocusedStyle Resize="None" />
                                                            <DisabledStyle Resize="None" />
                                                            <InvalidStyle Resize="None" />
                                                            <HoveredStyle Resize="None" />
                                                            <EnabledStyle Resize="None" />
                                                        </as:RadNumericTextBox>
                                                        <label class="non-edit" id="ucNRThresholdLow" runat="server">
                                                            <as:Literal ID="lbNRTThresholdLow" runat="server" Text='<%# GetParameterNumberForLabel(Eval("ReAlertParameterThreshold").ToString(), Eval("ParameterPrecision").ToString()) %>' meta:resourcekey="lbThresholdResource"></as:Literal>
                                                        </label>
                                                    </div>
                                                    <label>
                                                        <span id="spanNRTErrMsgThresholdLow" class="error" runat="server"></span>
                                                        <span>
                                                            <as:Literal ID="lblNRTPercentThresholdLow" runat="server" Text='<%# GetValue(Eval("ThresholdType")) %>'></as:Literal></span>
                                                    </label>
                                                </div>
                                                <div class="control-center">
                                                    <label>
                                                        <as:Literal ID="LiteralNRT9" runat="server" Text="High" meta:resourcekey="Literal91Resource1"></as:Literal>&nbsp<as:Literal ID="lblNRTDollarThresholdHigh" runat="server" Text="&nbsp;" meta:resourcekey="lblDollarThresholdHigh1Resource1"></as:Literal>
                                                    </label>
                                                    <div class="w-input">
                                                        <as:ASRadNumericTextBox NumberFormat-AllowRounding="false" NumberFormat-KeepNotRoundedValue="true" NumberFormat-KeepTrailingZerosOnFocus="true"
                                                            ID="txtNRTThresholdHigh" runat="server" Text='<%# GetParameterNumber(Eval("ReAlertParameterThresholdHigh").ToString()) %>'
                                                            Width="60px" CssClass="form-control edit" Culture="en-US" DbValueFactor="1" LabelCssClass="" LabelWidth="44px"
                                                            refer-key='<%# string.Format("{0}_RAT", Eval("ParameterKey")) %>'>
                                                            <NegativeStyle Resize="None" />
                                                            <NumberFormat PositivePattern="n" NegativePattern="n" DecimalDigits="0" ZeroPattern="n" />
                                                            <EmptyMessageStyle Resize="None" />
                                                            <ReadOnlyStyle Resize="None" />
                                                            <FocusedStyle Resize="None" />
                                                            <DisabledStyle Resize="None" />
                                                            <InvalidStyle Resize="None" />
                                                            <HoveredStyle Resize="None" />
                                                            <EnabledStyle Resize="None" />
                                                        </as:ASRadNumericTextBox>
                                                        <label class="non-edit" id="ucNRTThresholdHigh" runat="server">
                                                            <as:Literal ID="lbNRTThresholdHigh" runat="server" Text='<%# GetParameterNumberForLabel(Eval("ReAlertParameterThresholdHigh").ToString(), Eval("ParameterPrecision").ToString()) %>' meta:resourcekey="lbThresholdResource"></as:Literal>
                                                        </label>
                                                    </div>
                                                    <label>
                                                        <span id="spanNRTErrMsgThresholdHigh" class="error" runat="server"></span>
                                                        <span>
                                                            <as:Literal ID="lblNRTPercentThresholdHigh" runat="server" Text='<%# GetValue(Eval("ThresholdType")) %>'></as:Literal></span>
                                                    </label>

                                                </div>
                                            </as:PlaceHolder>
                                            <div class="msgError error text-space text-left"></div>
                                        </div>

                                        <div id="divActionLink" class="item action-link w-links">
                                            <as:PlaceHolder runat="server" ID="uxPanelMatchAll">
                                                <as:LinkButton runat="server" ID="uxUnGroup" OnClientClick="return onUnGroup(this)" Text="Ungroup" CssClass="<%# hideUnGroupBntClass %>" meta:resourcekey="btnUnGroupResource1"></as:LinkButton>
                                                <span runat="server" id="uxStandAloneIcon" class="icon-warning"></span>
                                                <as:LinkButton runat="server" ID="uxDuplicate" OnClientClick="return onDuplicate(this)" Text="Duplicate" meta:resourcekey="btnDuplicateResource1"></as:LinkButton>
                                            </as:PlaceHolder>
                                            <as:LinkButton runat="server" ID="btnDelete" OnClientClick="return confirmDeleteParameter(this)" Text="Remove" meta:resourcekey="btnRemoveResource1"></as:LinkButton>
                                        </div>
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>
        <as:PlaceHolder ID="plAudit" runat="server" Visible="false">
            <div class="d-flex mt-4x">
                <div>
                    <as:Literal ID="ltAutitReportDetail" runat="server"></as:Literal>
                    <as:HiddenField ID="hdLinkAudit" runat="server" />
                </div>
                <div class="ml-4x">
                    <a id="" href="#" class="heading link-back" onclick="return showAudit();">
                        <as:Literal ID="Literal1" runat="server" Text="Assignment Audit Report" meta:resourcekey="lbAssignmentAuditReport"></as:Literal></a>
                </div>
            </div>
        </as:PlaceHolder>
        <div class="mt-6x">
            <as:HiddenField ID="uxJson" runat="server" />
            <as:PlaceHolder runat="server" ID="uxAddMoreParams">
                <a href="#" class="heading link-back" onclick="return parameter_Add('rm_MCF_ParameterListModal.aspx');">
                    <as:Literal ID="Literal14" runat="server" Text="+ Add parameters" meta:resourcekey="lbAddParameter"></as:Literal>
                </a>
            </as:PlaceHolder>            
        </div>
        <div class="display-none">
            <!--invisible buttons-->
            <as:Button ID="btnRefreshAssParam" runat="server" OnClick="uxClose_Click"
                IsStandardButton="True" meta:resourcekey="btnRefreshAssParamResource1" />
            <as:Button ID="btnRefreshParamList" runat="server" OnClick="btnRefreshParamList_Click"
                IsStandardButton="True" meta:resourcekey="btnRefreshParamListResource1" />
            <as:HiddenField runat="server" ID="uxCurrentParam" />
            <as:HiddenField runat="server" ID="uxPramKey" />
            <as:HiddenField runat="server" ID="CountItemRepeater" />
            <as:HiddenField runat="server" ID="uxAssignmentID" />
            <as:HiddenField runat="server" ID="uxFilterID" />
            <as:HiddenField runat="server" ID="uxMode" />
            <as:HiddenField runat="server" ID="uxTransactionCode" />
            <as:HiddenField ID="uxModalType" runat="server" />
        </div>
    </div>
</div>

<as:ASRadCodeBlock ID="ASRadCodeBlock1" runat="server">
    <script>
        var Risk_Assignment_Parameters_chkRiskScore = '<%=chkRiskScore.ClientID %>';
        var Risk_Assignment_Parameters_txtRCFrom = '<%=txtRCFrom.ClientID %>';
        var Risk_Assignment_Parameters_txtRCTo = '<%=txtRCTo.ClientID %>';
        var Risk_Assignment_Parameters_uxCurrentParam = '<%=uxCurrentParam.ClientID %>';
        var Risk_Assignment_Parameters_uxPramKey = '<%=uxPramKey.ClientID %>';
        var Risk_Assignment_Parameters_CountItemRepeater = '<%=CountItemRepeater.ClientID %>';
        var Risk_Assignment_Parameters_uxJson = '<%=uxJson.ClientID %>';
        var Risk_Assignment_Parameters_QueryString = '?<%=QueryString %>';
        var Risk_Assignment_Parameters_btnRefreshParamList = '<%=btnRefreshParamList.ClientID %>';
        var Risk_Assignment_Parameters_btnRefreshAssParam = '<%=btnRefreshAssParam.ClientID %>';
        var Risk_Assignment_Parameters_uxTransactionCode = '<%=uxTransactionCode.ClientID %>';
        var Risk_Assignment_Parameters_rdMatchSpecific = '<%=rdMatchSpecific.ClientID %>';
        var Risk_Assignment_Parameters_rdMatchAll = '<%=rdMatchAll.ClientID %>';

    </script>
    <!-- parameter for Risk_Parameter.js -->
    <script type="text/javascript">
        var ERR_MAXEXCEED = '<%= Resources.MessageManager.RiskParameter_js_ValueMayNotExceed %>' + ' ';
        var ERR_MINEXCEED = '<%= Resources.MessageManager.RiskParameter_js_ValueMayNotLowerThan %>' + ' ';
        var ERR_NUMONLY = '<%= Resources.MessageManager.RiskParameter_js_OnlyNumbers  %>';
        var ERR_REQUIREDFIELD = ' : ' + '<%=Resources.MessageManager.RiskParameter_js_RequiredField %>';
        var ERR_INTNUMBER = '<%=Resources.MessageManager.RiskParameter_js_IntergerNumber  %>';
        var ERR_TOLOWERFROM = '<%=Resources.MessageManager.RiskParameter_js_ToMustBeGreaterThanFrom  %>';
        var ERR_LOWGTHIGH = '<%=Resources.MessageManager.RiskParameter_js_HighMustBeGreaterThanLow  %>';
        var RiskParameter_js_MustBeSelected = '<%=Resources.MessageManager.RiskParameter_js_MustBeSelected  %>';
        var RiskParameter_js_ThePartMustBeLessThan = '<%=Resources.MessageManager.RiskParameter_js_ThePartMustBeLessThan  %>';
        var RiskParameter_js_MsgDisablingParameter = '<%=Resources.MessageManager.RiskParameter_js_MsgDisablingParameter  %>';
        var RiskParameter_js_Msg1DisablingParameter = '<%=Resources.MessageManager.RiskParameter_js_Msg1DisablingParameter  %>';
        var RiskParameter_js_MonitoringParameter = '<%=Resources.MessageManager.RiskParameter_js_MonitoringParameter  %>';
        var RiskParameter_js_RemoveTheParameter = '<%=Resources.MessageManager.RiskParameter_js_RemoveTheParameter  %>';
        var RiskParameter_js_AssignmentMonitoringParam = '<%=Resources.MessageManager.RiskParameter_js_AssignmentMonitoringParam  %>';
        var RiskParameter_js_AssignmentDisablingParam = '<%=Resources.MessageManager.RiskParameter_js_AssignmentDisablingParam  %>';
        var RiskParameter_js_RiskScoreDisablingParam = '<%=Resources.MessageManager.RiskParameter_js_RiskScoreDisablingParam  %>';
        var RiskParameter_js_IndicatorFrom = '<%=Resources.MessageManager.RiskParameter_js_IndicatorFrom  %>';
        var RiskParameter_js_IndicatorTo = '<%=Resources.MessageManager.RiskParameter_js_IndicatorTo  %>';
        var RiskParameter_js_Threshold = '<%=Resources.MessageManager.RiskParameter_js_Threshold  %>';
        var RiskParameter_js_Indicator = '<%=Resources.MessageManager.RiskParameter_js_Indicator  %>';
        var RiskParameter_js_Indicator_Min = '<%= Resources.MessageManager.RiskParameter_js_Indicator_Min%>';
        var Risk_Assignment_Parameters_js_msg1 = '<%= GetLocalResourceObject("Risk_Assignment_Parameters_js_msg1").ToString() %>';
        var Risk_Assignment_Parameters_js_msg2 = '<%= GetLocalResourceObject("Risk_Assignment_Parameters_js_msg2").ToString() %>';
        var Risk_Assignment_Parameters_js_msg3 = '<%= GetLocalResourceObject("Risk_Assignment_Parameters_js_msg3").ToString() %>';
        var Risk_Assignment_Parameters_js_group = '<%= GetLocalResourceObject("Literal2Resource1.Text").ToString() %>';
        var Risk_Assignment_Parameters_uxPrimaryID_ClientID = '<%=uxAssignmentID.ClientID %>';
        var Risk_Assignment_Parameters_uxMode_ClientID = '<%=uxMode.ClientID %>';
        var Risk_Assignment_Parameters_uxFilterID_ClientID = '<%=uxFilterID.ClientID %>';
        var RiskParameter_js_Indicator_ReAlert01 = "<%= Resources.MessageManager.RiskParameter_js_Indicator_ReAlert01%>";
        var RiskParameter_js_Indicator_ReAlert02 = "<%=Resources.MessageManager.RiskParameter_js_Indicator_ReAlert02%>";
        var RiskParameter_js_Threshold_ReAlert01 = "<%= Resources.MessageManager.RiskParameter_js_Threshold_ReAlert01%>";
        var RiskParameter_js_Threshold_ReAlert02 = "<%=Resources.MessageManager.RiskParameter_js_Threshold_ReAlert02%>";
        var RiskParameter_js_Threshold_ReAlertCM = "<%= Resources.MessageManager.RiskParameter_js_Threshold_ReAlertCM%>";
        var Risk_Assignment_Parameters_hdLinkAudit_ClientID = '<%=hdLinkAudit.ClientID %>';
    </script>
    <script src="<%= ResolveUrl("~/")%>res/js/common/riskparameter.js"></script>
    <script src="<%= ResolveUrl("~/")%>res/js/risk_MCF/rm_MCF_Assignment_Parameters_New.js"></script>
</as:ASRadCodeBlock>
