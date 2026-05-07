<%@ Page Language="C#" MasterPageFile="~/MasterPagePopup.master" AutoEventWireup="true"
    CodeFile="rm_MCF_Assignment_CreateNew.aspx.cs" Inherits="rm_MCF_Assignment_CreateNew"
    Title="MANAGE ASSIGNMENTS" meta:resourcekey="PageResource1" %>

<%@ Register Src="~/UserControls/rm_MCF_Assignment_CancelButton.ascx" TagName="CancelButton"
    TagPrefix="uc" %>
<%@ Register Src="~/UserControls/rm_MCF_Assignment_Info.ascx" TagName="AssInfo" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/rm_MCF_Assignment_Filters.ascx" TagName="uxFilters" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/rm_MCF_Assignment_Parameters_New.ascx" TagName="Parameters" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/rm_MCF_CustomView.ascx" TagName="CustomView" TagPrefix="uc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="Server">
    <as:RadAjaxManagerProxy ID="ram" runat="server">
        <AjaxSettings>
            <tek:AjaxSetting AjaxControlID="uxParam">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="uxParam" />
                </UpdatedControls>
            </tek:AjaxSetting>
            <tek:AjaxSetting AjaxControlID="uxLoadAssignmentFilters">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="pnlFilters" />
                </UpdatedControls>
            </tek:AjaxSetting>
            <tek:AjaxSetting AjaxControlID="uxLoadAssignmentParameters">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="pnlParams" />
                </UpdatedControls>
            </tek:AjaxSetting>
        </AjaxSettings>

    </as:RadAjaxManagerProxy>

    <as:ASModalContainer ID="ASModalContainer1" runat="server" WidthCssClass="modal-xxl" ContainerCssClass="container" Width="">
        <span id="markupAssinfo" />
        <uc:AssInfo ID="uxAssInfo" runat="Server" Mode="Assignment" />
        <span id="markupFilter" />
        <as:Panel ID="pnlFilters" runat="server" CssClass="pos-relative">
            <uc:uxFilters ID="uxFilters" runat="Server" Mode="Assignment" />
        </as:Panel>
        <as:Panel ID="pnlParams" runat="server" CssClass="pos-relative">
            <span id="markupParam" />
            <uc:Parameters ID="uxParam" runat="Server" Mode="Assignment" />
        </as:Panel>
        <as:PlaceHolder ID="plAuditMaster" runat="server" Visible="false">
            <div class="d-flex mt-4x">
                <div>
                    <as:Literal ID="ltAutitReportDetail" runat="server"></as:Literal>
                    <as:HiddenField ID="hdLinkAuditMaster" runat="server" />
                </div>
                <div class="ml-4x">
                    <a id="" href="#" class="heading link-back" onclick="return showAuditCreateNew();">
                        <as:Literal ID="Literal1" runat="server" Text="Assignment Audit Report" meta:resourcekey="lbAssignmentAuditReport"></as:Literal></a>
                </div>
            </div>
        </as:PlaceHolder>
        <div class="height-10"></div>
        <as:PlaceHolder ID="uxPhCustomView" runat="server" Visible="true">
             <uc:CustomView runat="server" ID="uxCustomView" PageMode="Assignment" PageSection="Assignment" OnPreRender="uxCustomView_PreRender" />
         </as:PlaceHolder>        

        <%--49841 - fix bug registerCloseCustomeViewModelEvent--%>
        <div class="custom-view" id="uxCustomViewModeReadOnly" runat="server" visible="false">
            <label>
                <as:Literal ID="lblCustomView" runat="server" Text="Barometer View: " meta:resourcekey="uxCustomViewReadOnLy"></as:Literal>
            </label>
            <div class="inline-block">
                <as:ASRadComboBox ID="uxCustomViewReadOnly" runat="server" Filter="Contains" MarkFirstMatch="true" Width="250px" ExpandDirection="Up" AutoPostBack="true"
                    DataValueField="CustomViewID" DataTextField="ViewName" tracking-key="BarometerView" tracking-type="combobox">
                </as:ASRadComboBox>

            </div>
        </div>

        <div id="fixedHeight"></div>
    </as:ASModalContainer>
    <div id="fixedPanel">
        <div id="fixedShadowBox"></div>
        <div class="form-action-container text-right" id="fixedContain">
            <span id="pnStatus" class="pos-relative">
                <a href="#" id="ucGroup" class="heading link-back btn-fixed-bottom hide" onclick="return onGroup();">
                    <as:Literal ID="lbGroup" runat="server" Text="Group" meta:resourcekey="ucGroup"></as:Literal></a>
                <as:Button ID="uxSave" runat="server" OnClick="uxCheckDuplicateName_Click" Text="Finish"
                    OnClientClick="return ValidateCriteriaFilter();" CssClass="btn btn-default" IsStandardButton="False" meta:resourcekey="uxSaveResource1" />
                <uc:CancelButton runat="server" ID="uxCancel" />
            </span>
        </div>
        <div id="fixedFooter"></div>
    </div>
    <as:Button ID="uxSaveAssignment" runat="server" OnClick="uxSave_Click" CssClass="display-none" IsStandardButton="False" meta:resourcekey="uxSaveAssignmentResource1" />
    <as:Button ID="uxLoadAssignmentFilters" runat="server" OnClick="uxLoadAssignmentFilters_Click" CssClass="display-none" IsStandardButton="False" />
    <as:Button ID="uxLoadAssignmentParameters" runat="server" OnClick="uxLoadAssignmentParameters_Click" CssClass="display-none" IsStandardButton="False" />
    <as:HiddenField ID="uxParamActions" runat="server" />
    <as:HiddenField ID="uxAssignmentTrackingJson" runat="server" />
    <as:HiddenField ID="uxAssignmentTrackingJsonTemp" runat="server" />
    <as:ASRadCodeBlock ID="ASRadCodeBlock1" runat="server">
        <script type="text/javascript">
            var rm_Assignment_CreateNew_uxSaveAssignment = '<%=uxSaveAssignment.ClientID %>';
            var rm_Assignment_CreateNew_isDetectionQueueAssignment_Client = '<%=IsDetectionQueueAssignment%>';
            var rm_Assignment_CreateNew_uxSave = '<%=uxSave.ClientID %>';
            var rm_Assignment_CreateNew_uxLoadAssignmentFilters = '<%=uxLoadAssignmentFilters.ClientID %>';
            var rm_Assignment_CreateNew_uxLoadAssignmentParameters = '<%=uxLoadAssignmentParameters.ClientID %>';

            var uxCustomViewReadOnly_ClientID ='<%=uxCustomViewReadOnly.ClientID %>';
            var rm_Assignment_CreateNew_IsFirstLoad = '<%=IsFirstLoad %>' == 'True';
            var rm_Assignment_CreateNew_uxParamActions = '<%=uxParamActions.ClientID %>';

            var rm_Assignment_CreateNew_IsCreateNewAssignment = '<%= IsCreateNewAssignment%>' == 'True';

            function doOpenNewPopup(encodeURL) {
                return parent.ShowPopupModalChild(1, encodeURL, 'auto');
            }

            var rm_Assignment_CreateNew_Included = '<%= GetLocalResourceObject("IncludedText").ToString() %>';
            var rm_Assignment_CreateNew_Excluded = '<%= GetLocalResourceObject("ExcludedText").ToString() %>';
            var rm_Assignment_CreateNew_Multiple = "MultipleActionParameter"; //No need multi-language
            var rm_Assignment_CreateNew_uxAssignmentTrackingJson = '<%= uxAssignmentTrackingJson.ClientID %>';
            var rm_Assignment_CreateNew_uxAssignmentTrackingJsonTemp = '<%= uxAssignmentTrackingJsonTemp.ClientID %>';
            var rm_Assignment_CreateNew_AuditLink = '<%= hdLinkAuditMaster.ClientID %>';
            var assignmentTrackingObj = [];
        </script>
        <script src="<%= ResolveUrl("~/")%>res/js/risk_MCF/rm_MCF_Assignment_CreateNew.js"></script>
    </as:ASRadCodeBlock>

</asp:Content>
