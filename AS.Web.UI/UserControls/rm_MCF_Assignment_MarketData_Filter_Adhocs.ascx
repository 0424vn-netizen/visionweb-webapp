<%@ Control Language="C#" AutoEventWireup="true" CodeFile="rm_MCF_Assignment_MarketData_Filter_Adhocs.ascx.cs" Inherits="UserControls_rm_MCF_Assignment_MarketData_Filter_Adhocs" %>
<%@ Register Src="~/UserControls/rm_MCF_ApprovalDate.ascx" TagName="AppDate" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/rm_MCF_FilterHierarchy.ascx" TagName="Hierarchy" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/rm_MCF_FilterState.ascx" TagName="State" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/rm_MCF_FilterSIC.ascx" TagName="SICCode" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/rm_MCF_FilterZipCode.ascx" TagName="ZipCode" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/rm_MCF_FilterProfile.ascx" TagName="Profile" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/rm_MCF_FilterMarketData.ascx" TagName="MarketData" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/rm_MCF_Assignment_Filter_Transactional.ascx" TagName="Transactional" TagPrefix="uc" %>

<style>
    .GroupHierarchy
    {
        font-size:15px !important;
        padding-left:40px !important;        
        font-weight:bold !important;
    }
</style>
<as:RadAjaxManagerProxy ID="RadAjaxManagerProxyReview" runat="server">
    <AjaxSettings>
        <tek:AjaxSetting AjaxControlID="btnRefreshState">
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="divState" />
            </UpdatedControls>
        </tek:AjaxSetting>
        <tek:AjaxSetting AjaxControlID="btnRefreshSIC">
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="divSIC" />
            </UpdatedControls>
        </tek:AjaxSetting>
        <tek:AjaxSetting AjaxControlID="btnRefreshProfile">
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="divProfile" />
            </UpdatedControls>
        </tek:AjaxSetting>
        <tek:AjaxSetting AjaxControlID="btnRefreshMarketData">
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="uxMarketData" />
                <tek:AjaxUpdatedControl ControlID="divMarketData" />
            </UpdatedControls>
        </tek:AjaxSetting>
        <tek:AjaxSetting AjaxControlID="uxBntMarketData">
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="uxMarketData" />
                <tek:AjaxUpdatedControl ControlID="divMarketData" />
            </UpdatedControls>
        </tek:AjaxSetting>
        <tek:AjaxSetting AjaxControlID="btnCalculateMerchantCount">
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="pnlMerchantCountOnFilter" />
            </UpdatedControls>
        </tek:AjaxSetting>
        <tek:AjaxSetting AjaxControlID="uxBtnMerchantCount">
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="pnlMerchantCountOnFilter" />
            </UpdatedControls>
        </tek:AjaxSetting>
    </AjaxSettings>
</as:RadAjaxManagerProxy>

<as:Validator ID="HieararchyFilterValidator" runat="server"
    ValidationFunction="ValidateMerchantRangeNumber" MessageContainerClientID="" meta:resourcekey="HieararchyFilterValidatorResource1">
    <Items>
        <as:BasicValidationItem Rule="Number" Message="Only numbers are allowed in this field."
            ControlToValidateID="txtMerchantRangeFrom" />
        <as:BasicValidationItem Rule="Number" Message="Only numbers are allowed in this field."
            ControlToValidateID="txtMerchantRangeTo" />
        <as:BasicValidationItem Rule="Number" Message="Only numbers are allowed in Merchant Number field." 
            ControlToValidateID="txtMerchantNumber" />
    </Items>
</as:Validator>
<%--<as:Validator ID="Validator1" runat="server" MessageType="AlertBox"
    ValidationFunction="ValidateReportDate">
    <Items>
        <as:BasicValidationItem Rule="Required" Message="Report Date is a required field." ControlToValidateID="uxReportDate" />
    </Items>
</as:Validator>--%>

<div style="text-align:left;">
<as:Container HeaderText="Filters" runat="server" ID="uxAS1" Width="100%" TemplateName="ascontainer_noborder.tpl" FooterControlID="" FooterText="" HeaderControlID="">
<table width="100%" border="1" cellpadding="0" class="MPSBorder" style="min-width:800px; table-layout:fixed;">
    <colgroup>       
        <col width="180px" />
        <col />
        <as:RadCodeBlock ID="RadCodeBlock1" runat="server">
            <%=FeatureMode == WebSiteEnums.FeatureMode.Edit ? "<col  width=\"45px\"/>" : ""%>
        </as:RadCodeBlock>
    </colgroup>
    <tr style="display:none;">
        <td></td>
        <td></td>
        <as:RadCodeBlock ID="RadCodeBlock2" runat="server">
                <%=FeatureMode == WebSiteEnums.FeatureMode.Edit ? "<td></td>" : ""%>
        </as:RadCodeBlock>
    </tr>
    <tr align="left" class="MPSBorderAltRow">       
        <td rowspan="4" align="center" >
            <div style="font-weight: bold; padding: 10px 0 10px 0;"><as:Literal ID="ltMerchant" runat="server" Text="Merchant Count:" meta:resourcekey="ltMerchantResource1"></as:Literal></div>
            <div id="pnlMerchantCountOnFilter" runat="server" style="font-size: large; font-weight: bold; padding-bottom: 15px;">
                0
            </div>
            <as:PlaceHolder ID="plhRefesh" runat="server">
                <div>
                    <as:Button runat="server" ID="uxBtnMerchantCount" OnClientClick="CalculateMerchantCount(); return false;" Text="Refresh" meta:resourcekey="uxBtnMerchantCountResource1" />
                    <span style="visibility:hidden; display:none;">
                        <as:Button ID="btnCalculateMerchantCount" runat="server" OnClick="btnCalculateMerchantCount_Click" meta:resourcekey="btnCalculateMerchantCountResource1" />
                    </span>
                </div>
            </as:PlaceHolder>
        </td>
        <%--<as:RadCodeBlock ID="RadCodeBlock5" runat="server">
            <td colspan="<%=FeatureMode == WebSiteEnums.FeatureMode.Edit ?"2":"1"%>" class="riskrow">
                <div style="width: 162px; float: left; font-weight:bold; padding-top:8px; font-weight:bold;">
                    Report Date</div>
                <div style="float: left">
                     <as:RadDatePicker ID="uxReportDate" runat="server" Width="208px" ShowPopupOnFocus="true">
                            <Calendar FastNavigationStep="12" ShowRowHeaders="false" ></Calendar>
                            <DateInput ID="DateInput1" DateFormat="MM/dd/yyyy" runat="Server" />
                    </as:RadDatePicker>
                </div>
            </td>
        </as:RadCodeBlock>--%>
        <as:RadCodeBlock ID="RadCodeBlock11" runat="server">
            <td colspan="<%=FeatureMode == WebSiteEnums.FeatureMode.Edit ?"2":"1"%>" >
                <table>
                    <tr>
                        <td style="font-weight:bold; padding:0;"><as:Literal ID="Literal1" runat="server" Text="Merchants On Watch" meta:resourcekey="Literal1Resource1"></as:Literal></td>
                        <td style="padding:0 0 0 10px;">
                            <as:CheckBox ID="chkIsMerchantsOnWatch" runat="server" TabIndex="4" meta:resourcekey="chkIsMerchantsOnWatchResource1" />
                        </td>
                    </tr>
                </table>
            </td>
        </as:RadCodeBlock>
    </tr>
   <%-- <tr align="left" class="MPSBorderAltRow">
        
    </tr>--%>
    <tr align="left" class="MPSBorderAltRow">
        <as:RadCodeBlock ID="RadCodeBlock6" runat="server">
            <td colspan="<%=FeatureMode == WebSiteEnums.FeatureMode.Edit ?"2":"1"%>" >
                <table>
                    <tr>
                        <td style="font-weight:bold; padding:0;"><as:Literal ID="Literal2" runat="server" Text="Merchants Number" meta:resourcekey="Literal2Resource1"></as:Literal></td>
                        <td style="padding:0 0 0 10px;">
                            <as:TextBox ID="txtMerchantNumber" runat="server" MaxLength="20" Width="180px" onkeypress="MerchantRange_OnKeyPress(event);" meta:resourcekey="txtMerchantNumberResource1"/></span>
                        </td>
                    </tr>
                </table>
            </td>
        </as:RadCodeBlock>
    </tr>
    <tr align="left" class="MPSBorderAltRow">  
    <as:RadCodeBlock ID="RadCodeBlock3" runat="server">        
        <td colspan="<%=FeatureMode == WebSiteEnums.FeatureMode.Edit ?"2":"1"%>" >
            <table>
                <tr>
                    <td style="font-weight:bold; padding:0;"><as:Literal ID="Literal3" runat="server" Text="Merchant # Range:" meta:resourcekey="Literal3Resource1"></as:Literal></td>
                    <td style="padding:0 0 0 10px;"><as:Literal ID="Literal4" runat="server" Text="From" meta:resourcekey="Literal4Resource1"></as:Literal></td>
                    <td style="padding:0 0 0 10px;">
                        <as:TextBox ID="txtMerchantRangeFrom" runat="server" Width="180px" MaxLength="16"
                            onblur="ValidateMerchantRange();" onkeypress="MerchantRange_OnKeyPress(event);" meta:resourcekey="txtMerchantRangeFromResource1"></as:TextBox>
                    </td>
                    <td style="padding:0 0 0 52px;"><as:Literal ID="Literal5" runat="server" Text="To" meta:resourcekey="Literal5Resource1"></as:Literal></td>
                    <td style="padding:0 0 0 8px;">
                        <as:TextBox ID="txtMerchantRangeTo" runat="server" Width="180px" MaxLength="16"
                            onblur="ValidateMerchantRange();" onkeypress="MerchantRange_OnKeyPress(event);" meta:resourcekey="txtMerchantRangeToResource1"></as:TextBox>
                    </td>
                </tr>
            </table>
        </td>
    </as:RadCodeBlock>
    </tr>
    <tr align="left" class="MPSBorderAltRow">
        
         <as:RadCodeBlock ID="RadCodeBlock4" runat="server">
            <td colspan="<%=FeatureMode == WebSiteEnums.FeatureMode.Edit ?"2":"1"%>">
                <uc:AppDate ID="uxApprovalDate" runat="server" />
            </td>
         </as:RadCodeBlock>
       
    </tr>
    
    <asp:Repeater ID="uxHierarchyFilterRepeater" runat="server" OnItemDataBound="uxHierarchyFilterRepeater_ItemDataBound">
            <ItemTemplate>
                 <tr class="MPSBorderRow">        
                    <td align="left" style="font-weight:bold; ">
                        <%#Eval("HierarchyFilterText") %>
                    </td>
                    <td>
                        <div>
                            <as:Panel ID="divHierarchy" style="text-align:justify;" runat="server" meta:resourcekey="divHierarchyResource1">
                                <as:Literal ID="lblHierarchy" runat="server" Text="N/A" meta:resourcekey="lblHierarchyResource1"></as:Literal>
                            </as:Panel>
                        </div>
                    </td>
                    <as:PlaceHolder runat="server" ID="plhHierarchy">
                        <td align="center">
                            <div class="EditButtonAssignment" onclick='return CallHierarchyFilterModal(&#039;rm_MCF_Filter_Hierarchy_Modal.aspx?<%# HierarchyFilterQueryString(Eval("HierarchyFilterMode").ToString(),"ctl00_ContentPage_uxFilters_uxHierarchyFilterRepeater_ctl" + GetItemIndexAsString(Container.ItemIndex) + "_btnRefreshHierarchy") %>&#039;, 700, 725); return false;'>
                            </div>
                            <as:Button ID="btnRefreshHierarchy" runat="server" OnCommand="btnRefreshHierarchy_Command" style="display:none;" CommandArgument='<%# Eval("HierarchyFilterMode") %>'  IsStandardButton="True" meta:resourcekey="btnRefreshHierarchyResource1"/>
                        </td>
                    </as:PlaceHolder>
                </tr>
             </ItemTemplate>
        </asp:Repeater>
        
       
        <tr class="MPSBorderRow">
            <td align="left" style="font-weight:bold;">
                <as:RadCodeBlock ID="RadCodeBlock5" runat="server">
                    <%=  GeneralFuncsLib.GetDataOfExtendedSetting("STATE_LABEL_TEXT") == string.Empty ? GetLocalResourceObject("Risk_Assignment_MarketData_Filter_Adhocs_ascx_State").ToString() : GeneralFuncsLib.GetDataOfExtendedSetting("STATE_LABEL_TEXT")%> 
                </as:RadCodeBlock>
            </td>
            <td>
                <div>
                    <div id="divState" style="text-align:justify;" runat="server">
                        <as:Literal ID="lblState" runat="server" Text="N/A" meta:resourcekey="lblStateResource1"></as:Literal>
                    </div>
                </div>
            </td>
            <as:PlaceHolder runat="server" ID="plhState">
                <td align="center">
                    <div class="EditButtonAssignment" onclick="return CallFilterModal1('rm_MCF_Filter_State_Modal.aspx', 480, 725); return false;">
                    </div>
                </td>
            </as:PlaceHolder>
        </tr>
        <tr class="MPSBorderRow">
            <td align="left" style="font-weight:bold;">
                <as:Literal ID="Literal6" runat="server" Text="SIC" meta:resourcekey="Literal6Resource1"></as:Literal>
            </td>
            <td>
                <div>
                    <div id="divSIC" style="text-align:justify;" runat="server">
                        <as:Literal ID="lblSIC" runat="server" Text="N/A" meta:resourcekey="lblSICResource1"></as:Literal>
                    </div>
                </div>
            </td>
            <as:PlaceHolder runat="server" ID="plhSIC">
            <td align="center">
                <div class="EditButtonAssignment" onclick="return CallFilterModal1('rm_MCF_Filter_SIC_Modal.aspx', 1000, 550); return false;">
                </div>
            </td>
            </as:PlaceHolder>
        </tr>
        <tr class="MPSBorderRow">
            <td align="left" style="font-weight:bold;">
                <as:Literal ID="Literal7" runat="server" Text="Profile" meta:resourcekey="Literal7Resource1"></as:Literal>
            </td>
            <td>
                <div>
                    <div id="divProfile" style="text-align:justify;" runat="server">
                        <as:Literal ID="lblProfile" runat="server" Text="N/A" meta:resourcekey="lblProfileResource1"></as:Literal>
                    </div>
                </div>
            </td>
            <as:PlaceHolder runat="server" ID="plhProfile">
                <td align="center">
                    <div class="EditButtonAssignment" onclick="return CallFilterModal1('rm_MCF_Filter_Profile_Modal.aspx', 550, 725); return false;">
                    </div>
                </td>
            </as:PlaceHolder>
        </tr>
        <tr class="MPSBorderRow">
            <td align="left" style="font-weight: bold;">
                <as:Literal ID="Literal8" runat="server" Text="Market Data" meta:resourcekey="Literal8Resource1"></as:Literal>
            </td>
            <td>
                <div>
                    <div id="divMarketData" style="text-align: justify;" runat="server">
                        <as:Literal ID="lblMarketData" runat="server" Text="N/A" meta:resourcekey="lblMarketDataResource1"></as:Literal>
                    </div>
                    <span style="visibility: hidden; display: none;">
                        <as:Button ID="btnRefreshMarketData" runat="server" OnClick="btnRefreshMarketData_Click" meta:resourcekey="btnRefreshMarketDataResource1" />
                    </span>
                </div>
            </td>
            <as:PlaceHolder runat="server" ID="plhMarketData">
                <td align="center">
                    <div class="EditButtonAssignment" onclick="MarketData_Click(); return false;">
                    </div>
                </td>
            </as:PlaceHolder>
        </tr>
        <uc:Transactional ID="uxTransactionalFilter" runat="server"></uc:Transactional>
    </table>
    <div style="display:none;"><!--invisible buttons-->

    
    <as:Button ID="btnRefreshState" runat="server" OnClick="btnRefreshState_Click" meta:resourcekey="btnRefreshStateResource1" />
    <as:Button ID="btnRefreshSIC" runat="server" OnClick="btnRefreshSIC_Click" meta:resourcekey="btnRefreshSICResource1" />
    <as:Button ID="uxBntMarketData" runat="server" OnClick="uxBntMarketData_Click" meta:resourcekey="uxBntMarketDataResource1" />
    <as:Button ID="btnRefreshProfile" runat="server" OnClick="btnRefreshProfile_Click" meta:resourcekey="btnRefreshProfileResource1"/>
    </div>
</as:Container>
</div>


<as:HiddenField runat="server" ID="uxMarketData" Value="1" />

<as:RadCodeBlock ID="radCodeBlock" runat="server">
    <script language="javascript" type="text/javascript">
        function MarketData_Click() {
            document.getElementById('<%=uxBntMarketData.ClientID %>').click();
        }
        
        function filter_closeModalEvent(modalID, clientIDbtn) {
            switch (modalID) {
                case 'HierarchyModal':
                    document.getElementById(clientIDbtn).click();
                    break;
                case 'SICModal':
                    document.getElementById('<%=btnRefreshSIC.ClientID %>').click();
                    break;
                case 'StateModal':
                    document.getElementById('<%=btnRefreshState.ClientID %>').click();
                    break;

                case 'ProfileModal':
                    document.getElementById('<%=btnRefreshProfile.ClientID %>').click();
                    break;
                case 'MarketDataModal':
                    document.getElementById('<%=btnRefreshMarketData.ClientID %>').click();
                    break;
            }
        }

        function CallFilterModal1(modal, width, height) {
            /*return doOpenSubPopup(modal + '?<%=QueryString %>', width, height);*/
            return doOpenSubPopup(modal + '?<%=QueryString %>', 'auto');
        }

        function CallFilterModal2(modal, width, height) {
            /*return doOpenSubPopup(modal, width, height);*/
            return doOpenSubPopup(modal, 'auto');
        }

        function CallHierarchyFilterModal(modal, width, height) {
            /*return doOpenSubPopup(modal, width, height);*/
            return doOpenSubPopup(modal, 'auto');
        }
        

        function DisplayMerchantCountOnFilter(merchantCountOnFilter) {
            $get("<%=pnlMerchantCountOnFilter.ClientID %>").innerHTML = addCommas(merchantCountOnFilter);
        }

        function addCommas(nStr) {
            nStr += '';
            x = nStr.split('.');
            x1 = x[0];
            x2 = x.length > 1 ? '.' + x[1] : '';
            var rgx = /(\d+)(\d{3})/;
            while (rgx.test(x1)) {
                x1 = x1.replace(rgx, '$1' + ',' + '$2');
            }
            return x1 + x2;
        }


        function ValidateMerchantRange() {
            
            var merchantRangeStatus = 'none';
            //Merchant Range
            var merchantRangeFrom = document.getElementById("<%=txtMerchantRangeFrom.ClientID %>").value;
            var merchantRangeTo = document.getElementById("<%=txtMerchantRangeTo.ClientID %>").value;

            if (merchantRangeFrom.length > 0 && merchantRangeTo.length > 0) {
                //Step 1: Numeric only
                if(ValidateMerchantRangeNumber() == false){
                    merchantRangeStatus = 'invalid';
                }
                //Step 2: Check From > To
                var fromValue = parseFloat(merchantRangeFrom);
                var toValue = parseFloat(merchantRangeTo);
                var isValid = true;
                if (fromValue > toValue)
                    isValid = false;
                if (!isValid)
                    alert('<%= GetLocalResourceObject("Risk_Assignment_MarketData_Filter_Adhocs_ascx_ToGreaterThanFrom").ToString() %>');
                if (isValid)
                    merchantRangeStatus = 'valid';
                else
                    merchantRangeStatus = 'invalid';
            }

            return merchantRangeStatus;
        }

        function ValidateAssignmentFilters() {
            if (!ValidateMerchantRangeNumber()) {
                return false;
            }
            
            var merchantRangeStatus = ValidateMerchantRange();
            if (merchantRangeStatus == 'invalid') {
                return false;
            }

            var approvalDate = approvalDateChecked();
            

            var uxMarketData = $("#<%=divMarketData.ClientID %>").html().trim();
            if (uxMarketData == '<%= GetLocalResourceObject("lblMarketDataResource1.Text").ToString() %>') {
                alert('<%= GetLocalResourceObject("Risk_Assignment_MarketData_Filter_Adhocs_ascx_SelectAtLeastOne").ToString() %>');
                return false;
            }

            var transactionalFilter = ValidateTransactionalFilter();
            if (transactionalFilter == 'invalid') {

                return false;
            }
            
            return true;
        }

        function MerchantCountConfirm(NumberOfMerchant) {
            if (parent.getSaveConfirm() == true) {
                var message = '<%= string.Format(GetLocalResourceObject("Risk_Assignment_MarketData_Filter_Adhocs_ascx_WorkingWithUpTo").ToString(), "_NumberOfMerchant_") %>';
                message = message.replace('_NumberOfMerchant_',NumberOfMerchant);
                var result = window.confirm(message);
                if (!result) {
                    parent.setSaveConfirm(false);
                } else {
                    parent.saveAssignment();
                }
            }  
        }

       
        
        function MerchantRange_OnKeyPress(e) {
            var evt = window.event ? window.event : e;
            var code = evt.keyCode ? evt.keyCode : e.which;
            if ((code >= 48 && code <= 57) ||
                code == 37 || code == 39 || code == 46 || code == 8 || code == 9)  //left arrow, righ arrow, del, backspace, tab
                return true;
            else {
                e.preventDefault ? e.preventDefault() : e.returnValue = false;
                return false;
            }
        }

        function CalculateMerchantCount() {
            btnCountClick();
        }
        
        function btnCountClick(){
            document.getElementById("<%=btnCalculateMerchantCount.ClientID %>").click();
        }
        
    </script>
</as:RadCodeBlock>