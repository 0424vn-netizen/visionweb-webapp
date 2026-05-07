<%@ Page Language="C#" MasterPageFile="~/MasterPagePopup.master" AutoEventWireup="true" CodeFile="rm_MCF_AdhocCreate.aspx.cs" Inherits="rm_MCF_AdhocCreate" Title="MANAGE ADHOC" meta:resourcekey="PageResource1" %>

<%@ Register Src="~/UserControls/rm_MCF_Assignment_CancelButton.ascx" TagName="CancelButton" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/rm_MCF_Assignment_Filter_Adhocs.ascx" TagName="uxFilters" TagPrefix="uc" %>

<%--<%@ Register Src="~/UserControls/Risk_Assignment_Parameters.ascx" TagName="Parameters"
    TagPrefix="uc" %>--%>
<%@ Register Src="~/UserControls/rm_MCF_Assignment_Parameters_Adhoc.ascx" TagName="Parameters"
    TagPrefix="uc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="Server">


    <as:ASModalContainer ID="uxModalContainer" runat="server" WidthCssClass="modal-xxl" ContainerCssClass="container" Width="">
        <as:RadAjaxManagerProxy ID="ram" runat="server">
            <AjaxSettings>
                <tek:AjaxSetting AjaxControlID="uxParam">
                    <UpdatedControls>
                        <tek:AjaxUpdatedControl ControlID="uxParam" />
                    </UpdatedControls>
                </tek:AjaxSetting>
            </AjaxSettings>
        </as:RadAjaxManagerProxy>

        <div class="row">
            <div class="col-md-12">
                <div class="box">
                    <i>
                        <asp:Literal ID="rm_AdhocCreate_aspx_" runat="server" meta:resourcekey="rm_AdhocCreate_aspx_Resource1">Perform an Adhoc Report search by filter(s).<br />
                        Filter:<br />
                        Choose Merchant to see the risk history for a single merchant based on the date selector.
                        <br />
                        Choose other filters to see the risk report by day.  Only those merchants with possible risk activity will appear.
                        </asp:Literal>
                    </i>
                </div>
            </div>
        </div>

        <span id="markupFilter" />
        <uc:uxFilters ID="uxFilters" runat="Server" Mode="Adhoc" />
        <span id="markupParam" />
        <uc:Parameters ID="uxParam" runat="Server" Mode="Adhoc" />


        <as:Button ID="uxSaveAssignment" runat="server" OnClick="uxSave_Click" CssClass="display-none" meta:resourcekey="uxSaveAssignmentResource1" />
        <div id="fixedHeight"></div>
    </as:ASModalContainer>

    <div id="fixedPanel">
        <div id="fixedShadowBox"></div>
        <div class="form-action-container" id="fixedContain">
            <span id="pnStatus" class="pos-relative">  
                <a href="#" id="ucGroup" class="heading link-back btn-fixed-bottom hide" onclick="return onGroup();">
                <as:Literal ID="lbGroup" runat="server" Text="Group" meta:resourcekey="ucGroup"></as:Literal></a>    
                <as:Button ID="uxSave" runat="server" Text="Finish" OnClientClick="return ValidateCriteriaFilter();" CssClass="btn btn-default" IsStandardButton="False" meta:resourcekey="uxSaveResource1" />
                <uc:CancelButton runat="server" ID="uxCancel" />
            </span>
        </div>
        <div id="fixedFooter"></div>
    </div>
    <as:ASRadCodeBlock ID="ASRadCodeBlock1" runat="server">
        <script type="text/javascript">
            var rm_AdhocCreate_uxSaveAssignment = '<%=uxSaveAssignment.ClientID %>';
            var rm_AdhocCreate_uxSave = '<%=uxSave.ClientID %>';

        </script>
        <script src="<%= ResolveUrl("~/")%>res/js/risk_MCF/rm_MCF_AdhocCreate.js"></script>
    </as:ASRadCodeBlock>

</asp:Content>
