<%@ Page Language="C#" MasterPageFile="~/MasterPagePopup.master" AutoEventWireup="true" CodeFile="rm_MCF_Assignment_CreateNew_MarketData.aspx.cs" Inherits="rm_MCF_Assignment_CreateNew_MarketData" Title="MANAGE ASSIGNMENTS" meta:resourcekey="PageResource1" %>
<%@ Register Src="~/UserControls/rm_MCF_Assignment_CancelButton.ascx" TagName="CancelButton" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/rm_MCF_Assignment_Info.ascx" TagName="AssInfo" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/rm_MCF_Assignment_MarketData_Filters.ascx" TagName="uxFilters" TagPrefix="uc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <meta http-equiv="X-UA-Compatible" content="IE=7" />
    <style type="text/css">
        input[type=button], input[type=submit]
        {
            height:24px !important;                      
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" Runat="Server">
    <tek:RadAjaxManagerProxy ID="ram" runat="server">
    </tek:RadAjaxManagerProxy>
    <div style="padding: 20px;">
        <center>
            <span style="font-size: 19px; font-weight: bold;"><asp:Literal ID="rm_Assignment_CreateNew_MarketData_aspx1" runat="server" meta:resourcekey="rm_Assignment_CreateNew_MarketData_aspx1Resource1" Text="RISK MANAGEMENT - ASSIGNMENTS - MANAGE ASSIGNMENTS"></asp:Literal> </span>
        </center>
        <br style="clear: both;" />
        <br />
        <uc:AssInfo ID="uxAssInfo" runat="Server" Mode="Assignment" FeatureMode="Edit" />
        <br />
        <uc:uxFilters ID="uxFilters" runat="Server" Mode="Assignment" FeatureMode="Edit" />
        <br />
        <br />
        <br />
    </div>
    <div style="table-layout: auto;">
    </div>
    <div style="position: fixed; width: 100%; bottom: 0px; left: 0;"
        id="pnStatus">
        <div class="PanelFooterScroll" style="margin-left:23px; margin-right:23px;">
            <div style="float: right;">                
                <as:Button ID="uxSave" runat="server" OnClick="uxCheckDuplicateName_Click" Text="Finish"
                    OnClientClick="return ValidateCriteriaFilter();" IsStandardButton="False" meta:resourcekey="uxSaveResource1" />
                <span style="visibility: hidden; display: none;">
                    <as:Button ID="uxSaveAssignment" runat="server" OnClick="uxSave_Click" IsStandardButton="False" meta:resourcekey="uxSaveAssignmentResource1" />
                </span>
                <uc:CancelButton runat="server" ID="uxCancel" />
            </div>
        </div>
    </div>
    <as:RadCodeBlock ID="radCodeBlock" runat="server">

        <script type="text/javascript">

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
                    info_closeModalEvent(parent._modalID);
                    parent.master_closeModalEvent = null;
                }
                return parent.ShowPopupModalChild(1, url, width, height);
            }
            parent._saveConfirm = false;
            parent._isDuplicate = false;
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

            parent.saveAssignment = function() {
                var btn = document.getElementById("<%=uxSaveAssignment.ClientID %>");
                btn.click();
            }



            function checkDuplicateAssignmentName(isDuplicate) {

                if (isDuplicate == 1) {
                    alert(String.format("<%=Resources.RiskMessageManager.Assignment_GeneralInformation_Required_Unique %>", "<%= GetLocalResourceObject("rm_Assignment_CreateNew_MarketData_aspx1_AssignmentName").ToString() %>"));
                    return false;
                }
                else {
                    //count the number of selected merchant and get confirm from user 
                    //before processing saving process
                    parent.setSaveConfirm(true);
                    btnCountClick();
                    return false;
                }
            }

            function ValidateCriteriaFilter() {
                var result = false;
                if (ValidateAssignmentGeneralInformation() == false
	                 || ValidateAssignmentFilters() == false
	                 ) {
                    return false;
                }
                return true;
            }
           
        </script>

    </as:RadCodeBlock>
</asp:Content>

