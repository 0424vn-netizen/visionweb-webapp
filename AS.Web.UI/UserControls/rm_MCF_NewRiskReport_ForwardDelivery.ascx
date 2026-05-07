<%@ Control Language="C#" AutoEventWireup="true" CodeFile="rm_MCF_NewRiskReport_ForwardDelivery.ascx.cs" Inherits="UserControls_rm_MCF_NewRiskReport_ForwardDelivery" %>

<as:RadAjaxManagerProxy ID="RadAjaxProxy" runat="server">
    <AjaxSettings>
        <tek:AjaxSetting AjaxControlID="uxCalculate">
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="uxInputForwardDelivery" />
            </UpdatedControls>
        </tek:AjaxSetting>
        <tek:AjaxSetting AjaxControlID="uxClear">
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="uxInputForwardDelivery" />
            </UpdatedControls>
        </tek:AjaxSetting>

    </AjaxSettings>

</as:RadAjaxManagerProxy>
<div id="uxForwardDelivery" class="in">
    <%-----------------------Forward Delivery--------------------------------%>
    <h2 class="grid-title">
        <span>
            <as:Literal ID="Literal1" runat="server" Text="Forward Delivery" meta:resourcekey="Literal7Resource1"></as:Literal>
        </span>
    </h2>
    <div id="uxInputForwardDelivery" runat="server">
        <div class="mt-6x">
            <div class="row input-control">
                <div class="control-inline days">
                    <as:Literal ID="Literal2" runat="server" Text="Credit Timeliness Days:" meta:resourcekey="lblViewResource"></as:Literal>
                </div>
                <div class="control-inline">
                    <as:TextBox ID="uxDays" runat="server" InputType="Number" Width="250px" CssClass="rf_TextBox" LabelCssClass="" LabelWidth="64px" meta:resourcekey="uxMerchantNumberResource1" Resize="None">
                    </as:TextBox>
                </div>
                <div class="control-inline">
                    <span class="error display-none" id="daysError"></span>
                </div>
            </div>
            <div class="row input-control">
                <div class="control-inline ndx">
                    <as:Literal ID="Literal3" runat="server" Text="NDX:" meta:resourcekey="lblViewResource"></as:Literal>
                </div>
                <div class="control-inline">
                    <as:TextBox ID="uxNDX" runat="server" InputType="Number" Width="250px" CssClass="rf_TextBox" LabelCssClass="" LabelWidth="64px" meta:resourcekey="uxMerchantNumberResource1" Resize="None">
                    </as:TextBox>
                </div>
                <div class="control-inline">
                    <span class="error display-none" id="ndxError"></span>
                </div>
            </div>
            <as:Panel ID="uxPnNDXPercent" runat="server" class="row input-control">
                <div class="control-inline ndx-percent">
                    <as:Literal ID="Literal13" runat="server" Text="% of NDX:" meta:resourcekey="lblViewResource"></as:Literal>
                </div>
                <div class="control-inline">
                    <as:TextBox ID="uxNDXPercent" runat="server" inputtype="Number" MaxLength="3"  Width="250px" CssClass="rf_TextBox" labelcssclass="" labelwidth="64px" meta:resourcekey="uxMerchantNumberResource1" resize="None">
                    </as:TextBox>
                </div>
                <div class="control-inline">
                    <span class="error display-none" id="ndxPercentError"></span>
                </div>
            </as:Panel>
            <div class="row input-control">
                <div class="control-inline">
                    <as:Literal ID="Literal4" runat="server" Text="Current Period From:" meta:resourcekey="lblViewResource"></as:Literal>
                </div>
                <div class="control-inline">
                    <div class="current-period">
                        <as:TextBox ID="uxPeriodFrom" runat="server" Width="103px" Enabled="false" CssClass="rf_TextBox" LabelCssClass="" LabelWidth="64px" meta:resourcekey="uxMerchantNumberResource1" Resize="None">
                        <EmptyMessageStyle Resize="None" />
                            <readonlystyle resize="None" />
                        <FocusedStyle Resize="None" />
                        <DisabledStyle Resize="None" />
                        <InvalidStyle Resize="None" />
                        <HoveredStyle Resize="None" />
                        <EnabledStyle Resize="None" />
                        </as:TextBox>
                        <span class="period-to">
                            <as:Literal ID="Literal5" runat="server" Text="To:" meta:resourcekey="lblViewResource"></as:Literal></span>
                        <as:TextBox ID="uxPeriodTo" runat="server" Width="104px" Enabled="false" CssClass="rf_TextBox" LabelCssClass="" LabelWidth="64px" meta:resourcekey="uxMerchantNumberResource1" Resize="None">
                        <EmptyMessageStyle Resize="None" />
                        <ReadOnlyStyle Resize="None" />
                        <FocusedStyle Resize="None" />
                        <DisabledStyle Resize="None" />
                        <InvalidStyle Resize="None" />
                        <HoveredStyle Resize="None" />
                        <EnabledStyle Resize="None" />
                        </as:TextBox>
                    </div>
                </div>
            </div>
            <div class="row input-control">
                <div class="control-inline">
                </div>
                <div class="control-inline">
                    <asp:Button ID="uxCalculate" runat="server" Text="Calculate Risk Amount" OnClick="uxCalculate_Click" OnClientClick="return ValidateCalculate();" class="btn btn-default mr-10" meta:resourcekey="uxSubmitResource1" />
                    <button type="button" onclick="return ValidateClear();" class="btn btn-default" meta:resourcekey="uxSubmitResource1">Clear</button>
                    <asp:Button ID="uxClear" CssClass="display-none" runat="server" Text="Clear" OnClick="uxClear_Click" class="btn btn-default" meta:resourcekey="uxSubmitResource1" />
                </div>
            </div>
        </div>
        <div class="line">
        </div>
        <div class="data-delivery" id="uxDeliveryData" runat="server">
            <div class="row">
                <div class="col-md-4">
                    <div class="item">
                        <label>
                            <as:Literal ID="ltCreditTimelinessDays" runat="server" Text="Credit Timeliness Days:" meta:resourcekey="lblViewResource"></as:Literal></label>
                        <span id="uxCreditTimelinessDays" runat="server" meta:resourcekey="uxHRCodeResource1" />
                    </div>
                    <div class="item">
                        <label>
                            <as:Literal ID="Literal6" runat="server" Text="NDX:" meta:resourcekey="lblViewResource"></as:Literal></label>
                        <span id="uxNDXValue" runat="server" meta:resourcekey="uxHRCodeResource1" />
                    </div>
                    <as:Panel ID="uxPnNDXPercentValue" runat="server"  class="item">
                        <label>
                            <as:Literal ID="Literal15" runat="server" Text="% of NDX:" meta:resourcekey="lblViewResource"></as:Literal>
                        </label>
                        <span id="uxNDXPercentValue" runat="server" meta:resourcekey="uxHRCodeResource1" />
                    </as:Panel>
                    <div class="item">
                        <label>
                            <as:Literal ID="Literal8" runat="server" Text="Actual Sale Volume:" meta:resourcekey="lblViewResource"></as:Literal>
                        </label>
                        <as:Literal ID="uxActualSaleVolume" Text="—" runat="server" meta:resourcekey="uxHRCodeResource1" />
                    </div>
                    <div class="item">
                        <label>
                            <as:Literal ID="Literal10" runat="server" Text="30-Day Credit Ratio:" meta:resourcekey="lblViewResource"></as:Literal></label>
                        <as:Literal ID="uxMTDCreditRatio" Text="—" runat="server" meta:resourcekey="uxHRCodeResource1" />
                    </div>
                    <div class="item">
                        <label>
                            <as:Literal ID="Literal12" runat="server" Text="30-Day Chargeback Ratio:" meta:resourcekey="lblViewResource"></as:Literal></label>
                        <as:Literal ID="uxMTDChargebackRatio" Text="—" runat="server" meta:resourcekey="uxHRCodeResource1" />
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="item">
                        <label>
                            <as:Literal ID="Literal7" runat="server" Text="Credit Risk Amount:" meta:resourcekey="lblViewResource"></as:Literal></label>
                        <as:Literal ID="uxCreditRiskAmount" Text="—" runat="server" meta:resourcekey="uxHRCodeResource1" />
                    </div>
                    <div class="item">
                        <label>
                            <as:Literal ID="Literal11" runat="server" Text="Chargeback Risk Amount:" meta:resourcekey="lblViewResource"></as:Literal></label>
                        <as:Literal ID="uxChargebackRiskAmount" Text="—" runat="server" meta:resourcekey="uxHRCodeResource1" />
                    </div>
                    <div class="item">
                        <label>
                            <as:Literal ID="Literal14" runat="server" Text="NDX Risk Amount:" meta:resourcekey="lblViewResource"></as:Literal></label>
                        <as:Literal ID="uxNDXRiskAmount" runat="server" Text="—" meta:resourcekey="uxHRCodeResource1" />
                    </div>
                    <div class="item">
                        <label>
                            <as:Literal ID="Literal16" runat="server" Text="Total Risk Amount:" meta:resourcekey="lblViewResource"></as:Literal></label>
                        <as:Literal ID="uxTotalRiskAmount" runat="server" Text="—" meta:resourcekey="uxHRCodeResource1" />
                    </div>
                </div>
            </div>
            <div class="row last-update">
                <div class="col-md-12">
                    <as:Literal ID="Literal9" runat="server" Text="" meta:resourcekey="uxHRCodeResource1" />
                </div>
            </div>
        </div>
    </div>

    <as:RadCodeBlock ID="JavaScript" runat="server">
        <script type="text/javascript">
            var rm_ForwardDelivery_require = '<%= GetLocalResourceObject("rm_MCF_NewRiskReport_ForwardDelivery_RequireField").ToString() %>';
            var rm_ForwardDelivery_msg = '<%= GetLocalResourceObject("rm_MCF_NewRiskReport_ForwardDelivery_Validate").ToString() %>';
            var rm_ForwardDelivery_NDXPercent_Err = '<%= GetLocalResourceObject("rm_MCF_NewRiskReport_ForwardDelivery_NDXPercent").ToString() %>';
            var uxDays = "<%=uxDays.ClientID %>";
            var uxNDX = "<%=uxNDX.ClientID %>";
            var uxNDXPercent = "<%=uxNDXPercent.ClientID %>";
            var uxCalculate = "<%=uxCalculate.UniqueID %>";
            var uxClear = "<%=uxClear.ClientID %>";
            var uxDaysValue = "<%=uxCreditTimelinessDays.ClientID %>";
        </script>
        <script type="text/javascript" src="<%=ResolveUrl("~") %>res/js/risk_MCF/rm_MCF_NewRiskReport_ForwardDelivery.js"></script>
    </as:RadCodeBlock>
</div>
