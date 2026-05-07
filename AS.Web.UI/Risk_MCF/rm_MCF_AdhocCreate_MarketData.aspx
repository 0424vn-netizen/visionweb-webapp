<%@ Page Language="C#" MasterPageFile="~/MasterPagePopup.master" AutoEventWireup="true" CodeFile="rm_MCF_AdhocCreate_MarketData.aspx.cs" Inherits="rm_MCF_AdhocCreate_MarketData" Title="MANAGE ADHOC" meta:resourcekey="PageResource1" %>
<%@ Register Src="~/UserControls/rm_MCF_Assignment_CancelButton.ascx" TagName="CancelButton" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/rm_MCF_Assignment_MarketData_Filter_Adhocs.ascx" TagName="uxFilters" TagPrefix="uc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" Runat="Server">
    <tek:RadAjaxManagerProxy ID="ram" runat="server"></tek:RadAjaxManagerProxy>
    <div style="padding: 20px;">
        <center>
            <span style="font-size:19px; font-weight:bold;">
                <asp:Literal ID="rm_AdhocCreate_MarketData_aspx1" runat="server" meta:resourcekey="rm_AdhocCreate_MarketData_aspx1Resource1" Text="RISK MANAGEMENT - RISK ANALYSIS - ADHOC REPORT" ></asp:Literal>
            </span>
        </center>
        <br style="clear: both;" />
        <br />
        <div style="text-align:left;">
            <div style="padding-bottom: 6px;">
                 <asp:Literal ID="rm_AdhocCreate_MarketData_aspx2" runat="server" meta:resourcekey="rm_AdhocCreate_MarketData_aspx2Resource1" Text="Perform an Adhoc Report search by filter(s)." ></asp:Literal>
            </div>
            <span style="font-style: italic; text-decoration: underline;"> <asp:Literal ID="rm_AdhocCreate_MarketData_aspx3" runat="server" meta:resourcekey="rm_AdhocCreate_MarketData_aspx3Resource1" Text="Filter" ></asp:Literal></span>:<br />
            &nbsp;&nbsp;&nbsp;&nbsp; <asp:Literal ID="rm_AdhocCreate_MarketData_aspx4" runat="server" meta:resourcekey="rm_AdhocCreate_MarketData_aspx4Resource1" Text="
                Choose Merchant to see the risk history for a single merchant based on the date selector.
                " ></asp:Literal>
            <br />
            &nbsp;&nbsp;&nbsp;&nbsp; <asp:Literal ID="rm_AdhocCreate_MarketData_aspx5" runat="server" meta:resourcekey="rm_AdhocCreate_MarketData_aspx5Resource1" Text="Choose other filters to see the risk report by day.  Only those merchants with possible risk activity will appear." ></asp:Literal>
            
        </div>
        <br />
        <br />
        <uc:uxFilters ID="uxFilters" runat="Server" Mode="Adhoc" />
        <br />
        <br />
        <br />
    </div>
    <div style="position: fixed; width: 100%; bottom: 0px; left: 0;" id="pnStatus">
        <div class="PanelFooterScroll" style="margin-left:23px; margin-right:23px;">
            <div style="float: right;">
                <as:Button ID="uxSave" runat="server" Text="Finish" OnClientClick="return ValidateCriteriaFilter();" IsStandardButton="False" meta:resourcekey="uxSaveResource1" />
                <span style="visibility:hidden; display:none;">
                    <as:Button ID="uxSaveAssignment" runat="server" OnClick="uxSave_Click" IsStandardButton="False" meta:resourcekey="uxSaveAssignmentResource1" />
                </span>
                <uc:CancelButton runat="server" ID="uxCancel" />
            </div>
        </div>
    </div>
    <as:RadCodeBlock ID="radCodeBlock" runat="server">

        <script language="JavaScript" type="text/javascript">
  <!--
            function disableRiskScoreTextBoxes() { };
            function doOpenSubPopup(url, width, height) {
                var scrW = getScreenWidth();
                var scrH = getScreenHeight();
                var sizeRate = 0.70;
                if (width == null)
                    width = scrW * sizeRate;
                if (height == null)
                    height = scrH * sizeRate;
             
                parent.master_closeModalEvent = function() {

                    filter_closeModalEvent(parent._modalID, parent._clientIDbtn);
                    //parameter_closeModalEvent(parent._modalID);
                    parent.master_closeModalEvent = null;
                }
                return parent.ShowPopupModalChild(1, url, width, height);
            }
            parent._saveConfirm = false;
            parent._modalID = '';
            parent.setModalID = function(id) {
                parent._modalID = id;
            }

            parent._clientIDbtn = ''
            parent.setClientIDbtn = function(id) {
                parent._clientIDbtn = id;
            }
            
            parent.setSaveConfirm = function(value) {
                parent._saveConfirm = value;
            }
            parent.getSaveConfirm = function() {
                return parent._saveConfirm;
            }

            parent.master_showModalEvent = function() {
            }
            
            
            parent.saveAssignment = function() {
                var btn = document.getElementById("<%=uxSaveAssignment.ClientID %>");
                btn.click();
            }



            function ValidateCriteriaFilter() {
                var result = false;
                if (ValidateAssignmentFilters() == false) {
                    return false;
                }

                //count the number of selected merchant and get confirm from user before processing saving process
                parent.setSaveConfirm(true);
                btnCountClick();
                return false;
            }
  //-->
        </script>
    </as:RadCodeBlock>
</asp:Content>

