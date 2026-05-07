<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Risk_Assignment_Parameters.ascx.cs"
    Inherits="UserControls_Risk_Assignment_Parameters" %>
<%@ Register TagName="UxExport" Src="~/UserControls/UxExport.ascx" TagPrefix="uc" %>
<%@ Register TagName="UxTransCodeFilter" Src="~/UserControls/Risk_ParameterFilter_TransactionCode.ascx"
    TagPrefix="uc" %>


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
<div class="row">
    <div class="col-md-12 in" id="uxAssignmentParamsGrid">
        <table class="ASTable">
            <tr>
                <th></th>
                <th></th>
                <th><as:Literal ID="Literal1" runat="server" Text="% / # / $" meta:resourcekey="Literal1Resource1"></as:Literal></th>
                <th><as:Literal ID="ltThreshold" runat="server" Text="Threshold" meta:resourcekey="ltThresholdResource1"></as:Literal></th>
                <%=FeatureMode == WebSiteEnums.FeatureMode.Edit ? ("<th class=\"action-column\">"+GetLocalResourceObject("Risk_Assignment_Parameters_ascx_Action").ToString()+"</th>") : ""%>
            </tr>
            <tr class="Row">
                <td class="heading" style="width: 17%;"><as:Literal ID="Literal2" runat="server" Text="Match all parameters?" meta:resourcekey="Literal2Resource1"></as:Literal></td>
                <td>
                    <div class="control-inline">
                        <as:RadioButton ID="rdMatchNo" runat="server" TabIndex="4" GroupName="rdMatch" Text="No"
                            Checked="True" meta:resourcekey="rdMatchNoResource1" Value="" />
                    </div>
                    <div class="control-inline">
                        <as:RadioButton ID="rdMatchYes" runat="server" TabIndex="4" GroupName="rdMatch" Text="Yes" meta:resourcekey="rdMatchYesResource1" Value="" />
                    </div>

                </td>
                <td></td>
                <td></td>
                <%=FeatureMode == WebSiteEnums.FeatureMode.Edit ? "<td></td>" : ""%>
            </tr>
            <tr class="AltRow">
                <td class="heading">
                    <as:ValidatorLabel ID="VltLblRiskScore" runat="server" CssClass="control-label" Text="Risk Score" ApplyFor="txtRCFrom" meta:resourcekey="VltLblRiskScoreResource1"></as:ValidatorLabel>
                </td>
                <td>
                    <div class="control-inline">
                        <as:CheckBox ID="chkRiskScore" runat="server" onclick="chbRiskScore_click();" meta:resourcekey="chkRiskScoreResource1" />
                    </div>
                    <div class="control-inline">
                        <label><as:Literal ID="Literal3" runat="server" Text="From" meta:resourcekey="Literal3Resource1"></as:Literal></label>
                        <as:ASRadNumericTextBox ID="txtRCFrom" runat="Server" Type="Number" MaxLength="9" Width="80px" CssClass="form-control">
                            <NumberFormat PositivePattern="n" NegativePattern="n" DecimalDigits="0" />
                        </as:ASRadNumericTextBox>
                    </div>
                    <div class="control-inline">
                        <label><as:Literal ID="Literal4" runat="server" Text="To" meta:resourcekey="Literal4Resource1"></as:Literal></label>
                        <as:ASRadNumericTextBox ID="txtRCTo" runat="Server" Type="Number" MaxLength="9" Width="80px" CssClass="form-control">
                            <NumberFormat PositivePattern="n" NegativePattern="n" DecimalDigits="0" />
                        </as:ASRadNumericTextBox>
                    </div>
                    <div class="bottom-error">
                        <as:ValidatorMessage runat="server" ID="txtRCFromMsg" ApplyFor="txtRCFrom" Message="" ShowOnLoad="False" />
                        <as:ValidatorMessage runat="server" ID="txtRCToMsg" ApplyFor="txtRCTo" Message="" ShowOnLoad="False" />
                    </div>
                </td>
                <td></td>
                <td></td>
                <%=FeatureMode == WebSiteEnums.FeatureMode.Edit ? "<td></td>" : ""%>
            </tr>
            <asp:Repeater ID="uxParameterRepeater" runat="server" OnItemDataBound="rptParameterRepeater_ItemDataBound"
                OnItemCommand="rptParameterRepeater_ItemCommand">
                <ItemTemplate>
                    <tr id="paramAssignment" runat="server" class="Row">
                        <td colspan="2" runat="server">
                            <%# Eval("ParameterName")%>
                            <as:HiddenField ID="criteriaValueString" runat="server" Value='<%# GetValue(Eval("ParameterKey")) %>' />
                            <as:HiddenField ID="ParameterPrecision" runat="server" Value='<%# GetValue(Eval("ParameterPrecision")) %>' />
                            <as:HiddenField ID="IsThresholdNegative" runat="server" Value='<%# Eval("IsThresholdNegative") %>' />
                            <as:HiddenField ID="IsIndicatorNagative" runat="server" Value='<%# Eval("IsIndicatorNegative") %>' />
                            <as:HiddenField ID="ParameterThresholdType" runat="server" Value='<%# Eval("ParameterThresholdType") %>' />
                        </td>
                        <td class="text-center" runat="server">
                            <as:PlaceHolder ID="uxIndicatorNomal" runat="server">
                                <label class="before-label">
                                    <asp:Literal ID="lblIndicatorDollar" runat="server" Text="&nbsp;"></asp:Literal>
                                </label>
                                <as:ASRadNumericTextBox ID="txtParameterValue" runat="server" text='<%# GetParameterNumber(Eval("ParameterValue").ToString()) %>'
                                    Width="80px" CssClass="form-control" Culture="en-US" DbValueFactor="1" LabelCssClass="" LabelWidth="64px">
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
                                <as:Literal ID="lblIndicatorNA" runat="server" Text="N/A" Visible="False" meta:resourcekey="lblIndicatorNAResource1"></as:Literal>
                                <span id="spanErrMsg" runat="server"></span>
                                <label class="after-label">
                                    <as:Literal ID="lblParameterType" runat="server" Text='<%# GetValue(Eval("ParameterDataType")) %>'></as:Literal>
                                </label>
                            </as:PlaceHolder>
                            <!-- Indicator FromTo-->
                            <as:PlaceHolder ID="uxIndicatorHigh" runat="server" Visible="False">
                                <div class="row">
                                    <div class="control-inline">
                                        <label><as:Literal ID="Literal41" runat="server" Text="From" meta:resourcekey="Literal41Resource1"></as:Literal></label>
                                        <as:ASRadNumericTextBox ID="txtFrom" runat="server" Text='<%# GetParameterNumber(Eval("ParameterValue").ToString()) %>'
                                            Width="30px" CssClass="form-control" Culture="en-US" DbValueFactor="1" LabelCssClass="" LabelWidth="64px">
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
                                    </div>
                                    <div class="control-inline">
                                        <label><as:Literal ID="Literal51" runat="server" Text="To" meta:resourcekey="Literal51Resource1"></as:Literal></label>
                                        <as:ASRadNumericTextBox ID="txtTo" runat="server" Text='<%# GetParameterNumber(Eval("ParameterValueHigh").ToString()) %>'
                                            Width="30px" CssClass="form-control" Culture="en-US" DbValueFactor="1" LabelCssClass="" LabelWidth="64px">
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
                                        <label>
                                            <as:Literal ID="Literal61" runat="server" Text="day(s)" meta:resourcekey="Literal61Resource1"></as:Literal>
                                        </label>
                                    </div>

                                </div>
                            </as:PlaceHolder>


                        </td>
                        <td class="text-center" runat="server">
                            <as:PlaceHolder ID="lblNA" runat="server" Visible="False">
                                <label class="before-label"></label>
                                <strong><as:Literal ID="Literal71" runat="server" Text="N/A" meta:resourcekey="Literal71Resource1"></as:Literal></strong>
                                <label class="after-label"></label>
                            </as:PlaceHolder>
                            <as:PlaceHolder ID="uxThresholdNormal" runat="server">
                                <label class="before-label">
                                    <as:Literal ID="lblDollar" runat="server" Text="&nbsp;" meta:resourcekey="lblDollar71Resource1"></as:Literal>
                                </label>
                                <as:ASRadNumericTextBox ID="txtThreshold" runat="server" Text='<%# GetParameterNumber(Eval("ParameterThreshold").ToString()) %>'
                                    Width="80px" CssClass="form-control" Culture="en-US" DbValueFactor="1" LabelCssClass="" LabelWidth="64px">
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
                                <span id="spanErrMsgThs" runat="server"></span>
                                <label class="after-label">
                                    <as:Literal ID="lblpercent" runat="server" Text='<%# GetValue(Eval("ThresholdType")) %>'></as:Literal>
                                </label>
                            </as:PlaceHolder>

                            <as:PlaceHolder ID="uxThresholdLowHigh" runat="server" Visible="False">
                                <div class="row">
                                    <label class="before-label">
                                        <as:Literal ID="Literal8" runat="server" Text="Low" meta:resourcekey="Literal81Resource1"></as:Literal>&nbsp<as:Literal ID="lblDollarThresholdLow" runat="server" Text="&nbsp;"  meta:resourcekey="lblDollarThresholdLow1Resource1"></as:Literal>
                                    </label>

                                    <as:RadNumericTextBox ID="txtThresholdLow" runat="server" Text='<%# GetParameterNumber(Eval("ParameterThreshold").ToString()) %>'
                                        Width="80px" CssClass="form-control" Culture="en-US" DbValueFactor="1" LabelCssClass="" LabelWidth="64px">
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
                                    <span id="spanErrMsgThresholdLow" runat="server"></span>
                                    <label class="after-label">
                                        <as:Literal ID="lblPercentThresholdLow" runat="server" Text='<%# GetValue(Eval("ThresholdType")) %>'></as:Literal>
                                    </label>

                                </div>
                                <div class="row">
                                    <label class="before-label">
                                        <as:Literal ID="Literal9" runat="server" Text="High" meta:resourcekey="Literal91Resource1"></as:Literal>&nbsp<as:Literal ID="lblDollarThresholdHigh" runat="server" Text="&nbsp;" meta:resourcekey="lblDollarThresholdHigh1Resource1"></as:Literal>
                                    </label>

                                    <as:ASRadNumericTextBox ID="txtThresholdHigh" runat="server" Text='<%# GetParameterNumber(Eval("ParameterThresholdHigh").ToString()) %>'
                                        Width="80px" CssClass="form-control" Culture="en-US" DbValueFactor="1" LabelCssClass="" LabelWidth="64px">
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
                                    <span id="spanErrMsgThresholdHigh" runat="server"></span>
                                    <label class="after-label">
                                        <as:Literal ID="lblPercentThresholdHigh" runat="server" Text='<%# GetValue(Eval("ThresholdType")) %>'></as:Literal>
                                    </label>

                                </div>
                            </as:PlaceHolder>
                        </td>
                        <td id="Td1" runat="server" visible="<%# FeatureMode == WebSiteEnums.FeatureMode.Edit %>" class="action-column">
                            <as:LinkButton runat="server" ID="btnDelete" CommandName="Delete" CommandArgument='<%# GetValue(Eval("ParameterKey")) %>'
                                OnClientClick="if(!confirmDeleteParameter()) return false;" Text="Delete" meta:resourcekey="btnDeleteResource1"></as:LinkButton>
                        </td>
                    </tr>
                    <as:PlaceHolder runat="server" ID="uxSpecialPanel" Visible="False">
                        <tr class="risklightgrayrow">
                            <td colspan="4">
                                <as:Container ID="asContainer" runat="server" Width="100%" HeaderText="ReasonCode"
                                    TemplateName="riskparamfilterbox.tpl" FooterControlID="" FooterText="" HeaderControlID="">
                                    <div >
                                        <as:Literal ID="lblParamfilter" runat="server" meta:resourcekey="lblParamfilterResource2" />
                                    </div>
                                    <as:HiddenField ID="modalType" runat="server" />
                                </as:Container>
                            </td>
                            <%=FeatureMode == WebSiteEnums.FeatureMode.Edit ? "<td style=\"text-align: center;border-top-width: 0px;\">&nbsp;</td>" : ""%>
                        </tr>
                    </as:PlaceHolder>
                </ItemTemplate>
                <AlternatingItemTemplate>
                    <tr id="paramAssignment" runat="server" class="AltRow">
                        <td colspan="2" runat="server">
                            <%# Eval("ParameterName")%>
                            <as:HiddenField ID="criteriaValueString" runat="server" Value='<%# GetValue(Eval("ParameterKey")) %>' />
                            <as:HiddenField ID="ParameterPrecision" runat="server" Value='<%# GetValue(Eval("ParameterPrecision")) %>' />
                            <as:HiddenField ID="IsThresholdNegative" runat="server" Value='<%# Eval("IsThresholdNegative") %>' />
                            <as:HiddenField ID="IsIndicatorNagative" runat="server" Value='<%# Eval("IsIndicatorNegative") %>' />
                            <as:HiddenField ID="ParameterThresholdType" runat="server" Value='<%# Eval("ParameterThresholdType") %>' />
                        </td>
                        <td class="text-center" runat="server">
                            <as:PlaceHolder ID="uxIndicatorNomal" runat="server">
                                <label class="before-label">
                                    <asp:Literal ID="lblIndicatorDollar" runat="server" Text="&nbsp;"></asp:Literal>
                                </label>
                                <as:ASRadNumericTextBox ID="txtParameterValue" runat="server" text='<%# GetParameterNumber(Eval("ParameterValue").ToString()) %>'
                                    Width="80px" CssClass="form-control" Culture="en-US" DbValueFactor="1" LabelCssClass="" LabelWidth="64px">
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
                                <as:Literal ID="lblIndicatorNA" runat="server" Text="N/A" Visible="False" meta:resourcekey="lblIndicatorNA3Resource1"></as:Literal>
                                <span id="spanErrMsg" runat="server"></span>
                                <label class="after-label">
                                    <as:Literal ID="lblParameterType" runat="server" Text='<%# GetValue(Eval("ParameterDataType")) %>'></as:Literal>
                                </label>
                            </as:PlaceHolder>
                            <!-- Indicator FromTo-->
                            <as:PlaceHolder ID="uxIndicatorHigh" runat="server" Visible="False">
                                <div class="row">
                                    <div class="control-inline">
                                        <label><as:Literal ID="Literal9" runat="server" Text="From" meta:resourcekey="Literal9NA3Resource1"></as:Literal></label>
                                        <as:ASRadNumericTextBox ID="txtFrom" runat="server" Text='<%# GetParameterNumber(Eval("ParameterValue").ToString()) %>'
                                            Width="30px" CssClass="form-control" Culture="en-US" DbValueFactor="1" LabelCssClass="" LabelWidth="64px">
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
                                    </div>
                                    <div class="control-inline">
                                        <label><as:Literal ID="Literal10" runat="server" Text="To" meta:resourcekey="Literal103Resource1"></as:Literal></label>
                                        <as:ASRadNumericTextBox ID="txtTo" runat="server" Text='<%# GetParameterNumber(Eval("ParameterValueHigh").ToString()) %>'
                                            Width="30px" CssClass="form-control" Culture="en-US" DbValueFactor="1" LabelCssClass="" LabelWidth="64px">
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
                                        <label>
                                            <as:Literal ID="Literal11" runat="server" Text="day(s)" meta:resourcekey="Literal113Resource1"></as:Literal>
                                        </label>
                                    </div>

                                </div>
                            </as:PlaceHolder>


                        </td>
                        <td class="text-center" runat="server">
                            <as:PlaceHolder ID="lblNA" runat="server" Visible="False">
                                <label class="before-label"></label>
                                <strong><as:Literal ID="Literal12" runat="server" Text="N/A" meta:resourcekey="Literal123Resource1"></as:Literal></strong>
                                <label class="after-label"></label>
                            </as:PlaceHolder>
                            <as:PlaceHolder ID="uxThresholdNormal" runat="server">
                                <label class="before-label">
                                    <as:Literal ID="lblDollar" runat="server" Text="&nbsp;"></as:Literal>
                                </label>
                                <as:ASRadNumericTextBox ID="txtThreshold" runat="server" Text='<%# GetParameterNumber(Eval("ParameterThreshold").ToString()) %>'
                                    Width="80px" CssClass="form-control" Culture="en-US" DbValueFactor="1" LabelCssClass="" LabelWidth="64px">
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
                                <span id="spanErrMsgThs" runat="server"></span>
                                <label class="after-label">
                                    <as:Literal ID="lblpercent" runat="server" Text='<%# GetValue(Eval("ThresholdType")) %>'></as:Literal>
                                </label>
                            </as:PlaceHolder>

                            <as:PlaceHolder ID="uxThresholdLowHigh" runat="server" Visible="False">
                                <div class="row">
                                    <label class="before-label">
                                        <as:Literal ID="Literal13" runat="server" Text="Low" meta:resourcekey="Literal133Resource1"></as:Literal>&nbsp<as:Literal ID="lblDollarThresholdLow" runat="server" Text="&nbsp;"></as:Literal>
                                    </label>

                                    <as:RadNumericTextBox ID="txtThresholdLow" runat="server" Text='<%# GetParameterNumber(Eval("ParameterThreshold").ToString()) %>'
                                        Width="80px" CssClass="form-control" Culture="en-US" DbValueFactor="1" LabelCssClass="" LabelWidth="64px">
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
                                    <span id="spanErrMsgThresholdLow" runat="server"></span>
                                    <label class="after-label">
                                        <as:Literal ID="lblPercentThresholdLow" runat="server" Text='<%# GetValue(Eval("ThresholdType")) %>'></as:Literal>
                                    </label>

                                </div>
                                <div class="row">
                                    <label class="before-label">
                                        <as:Literal ID="Literal14" runat="server" Text="High"></as:Literal>&nbsp<as:Literal ID="lblDollarThresholdHigh" runat="server" Text="&nbsp;"></as:Literal>
                                    </label>

                                    <as:ASRadNumericTextBox ID="txtThresholdHigh" runat="server" Text='<%# GetParameterNumber(Eval("ParameterThresholdHigh").ToString()) %>'
                                        Width="80px" CssClass="form-control" Culture="en-US" DbValueFactor="1" LabelCssClass="" LabelWidth="64px">
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
                                    <span id="spanErrMsgThresholdHigh" runat="server"></span>
                                    <label class="after-label">
                                        <as:Literal ID="lblPercentThresholdHigh" runat="server" Text='<%# GetValue(Eval("ThresholdType")) %>'></as:Literal>
                                    </label>

                                </div>
                            </as:PlaceHolder>
                        </td>
                        <td id="Td1" runat="server" visible="<%# FeatureMode == WebSiteEnums.FeatureMode.Edit %>" class="action-column">
                            <as:LinkButton runat="server" ID="btnDelete" CommandName="Delete" CommandArgument='<%# GetValue(Eval("ParameterKey")) %>'
                                OnClientClick="if(!confirmDeleteParameter()) return false;" Text="Delete" meta:resourcekey="btnDeleteResource1"></as:LinkButton>
                        </td>
                    </tr>
                    <as:PlaceHolder runat="server" ID="uxSpecialPanel" Visible="False">
                        <tr class="risklightgrayrow">
                            <td colspan="4">
                                <as:Container ID="asContainer" runat="server" Width="100%" HeaderText="ReasonCode"
                                    TemplateName="riskparamfilterbox.tpl" FooterControlID="" FooterText="" HeaderControlID="">
                                    <div >
                                        <as:Literal ID="lblParamfilter" runat="server" meta:resourcekey="lblParamfilterResource1" />
                                    </div>
                                    <as:HiddenField ID="modalType" runat="server" />
                                </as:Container>
                            </td>
                            <%=FeatureMode == WebSiteEnums.FeatureMode.Edit ? "<td style=\"text-align: center;border-top-width: 0px;\">&nbsp;</td>" : ""%>
                        </tr>
                    </as:PlaceHolder>
                </AlternatingItemTemplate>
            </asp:Repeater>
            <asp:PlaceHolder ID="plhParameter" runat="server">
                <tr class="Row">
                    <td colspan="<%=FeatureMode == WebSiteEnums.FeatureMode.Edit ? "5" : "4"%>">
                        <a href="#" class="heading" onclick="return parameter_Add('rm_ParameterListModal.aspx');"><as:Literal ID="Literal14" runat="server" Text="+ Please click here to add more parameters." meta:resourcekey="Literal14Resource1"></as:Literal></a>
                    </td>
                </tr>
            </asp:PlaceHolder>
        </table>

        <div class="display-none">
            <!--invisible buttons-->
            <as:Button ID="btnRefreshAssParam" runat="server" OnClick="uxClose_Click"
                IsStandardButton="True" meta:resourcekey="btnRefreshAssParamResource1" />
            <as:Button ID="btnRefreshParamList" runat="server" OnClick="btnRefreshParamList_Click"
                IsStandardButton="True" meta:resourcekey="btnRefreshParamListResource1" />
            <as:Button ID="btnSaveWorkingParameters" runat="server" OnClick="btnSaveWorkingParameters_Click"
                IsStandardButton="True" meta:resourcekey="btnSaveWorkingParametersResource1" />
            <as:HiddenField runat="server" ID="uxCurrentParam" />
            <as:HiddenField runat="server" ID="uxPramKey" />
            <as:HiddenField runat="server" ID="CountItemRepeater" />
        </div>

    </div>
</div>

<as:ASRadCodeBlock ID="ASRadCodeBlock1" runat="server">
    <script type="text/javascript">
        var Risk_Assignment_Parameters_chkRiskScore = '<%=chkRiskScore.ClientID %>';
        var Risk_Assignment_Parameters_txtRCFrom = '<%=txtRCFrom.ClientID %>';
        var Risk_Assignment_Parameters_txtRCTo = '<%=txtRCTo.ClientID %>';
        var Risk_Assignment_Parameters_CountItemRepeater = '<%=CountItemRepeater.ClientID %>';
        var Risk_Assignment_Parameters_uxCurrentParam = '<%=uxCurrentParam.ClientID %>';
        var Risk_Assignment_Parameters_uxPramKey = '<%=uxPramKey.ClientID %>';
        var Risk_Assignment_Parameters_btnSaveWorkingParameters = '<%=btnSaveWorkingParameters.ClientID %>';
        var Risk_Assignment_Parameters_btnRefreshParamList = '<%=btnRefreshParamList.ClientID %>';
        var Risk_Assignment_Parameters_btnRefreshAssParam = '<%=btnRefreshAssParam.ClientID %>';

        var Risk_Assignment_Parameters_QueryString = '?<%=QueryString %>';

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
        var Risk_Assignment_Parameters_js_msg1 = '<%= GetLocalResourceObject("Risk_Assignment_Parameters_js_msg1").ToString() %>';
        var Risk_Assignment_Parameters_js_msg2 = '<%= GetLocalResourceObject("Risk_Assignment_Parameters_js_msg2").ToString() %>';
    </script>
    <script src="<%= ResolveUrl("~/")%>res/js/common/riskparameter.js"></script>
    <script src="<%= ResolveUrl("~/")%>res/js/risk/Risk_Assignment_Parameters.js"></script>
</as:ASRadCodeBlock>
