<%@ Page Title="Attribute Name Maintenance" meta:resourcekey="PageResource1" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="rm_MCF_AttributeNameMaintenance.aspx.cs" Inherits="rm_MCF_AttributeNameMaintenance" %>

<%@ Register TagName="UxExport" Src="~/UserControls/UxExport.ascx" TagPrefix="uc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="Server">
    <as:RadAjaxManagerProxy ID="RadAjaxManager" runat="server">
        <AjaxSettings>
            <tek:AjaxSetting AjaxControlID="uxListAttributeName">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="uxListAttributeName" />
                </UpdatedControls>
            </tek:AjaxSetting>
        </AjaxSettings>
    </as:RadAjaxManagerProxy>
    <div class="row">
        <div class="col-md-12">
            <h1 class="report-title-no-filter" id="uxTitle" runat="server">
                <asp:Literal ID="lblReportTitle" runat="server" Text="Attribute Name Maintenance" meta:resourcekey="uxReportTitleResource1"></asp:Literal>
            </h1>
        </div>
    </div>
    <!--  -->
    <div class="row">
        <div class="col-xs-3">
            <h4 class="no-marpad-im title-auto-queue" data-toggle="collapse" data-target="#list-attribute-name-container">
                <span class="on-top text-default-gray default-font-family">
                    <asp:Literal runat="server" ID="Literal2" Text="System Attribute Name" meta:resourcekey="ResourceParameterRiskScore" /></span>
            </h4>
        </div>
        <div class="col-xs-9">
            <h4 class="no-marpad-im title-auto-queue" data-toggle="collapse" data-target="#list-attribute-name-container">
                <span class="on-top text-default-gray default-font-family">
                    <asp:Literal runat="server" ID="Literal1" Text="Preferred Attribute Name" meta:resourcekey="ResourcePreferredAttributeNameRiskScore" /></span>
            </h4>
        </div>
    </div>
    <div id="list-attribute-name-container" class="mt-5x">
        <asp:Repeater ID="uxListAttributeName" runat="server" OnItemDataBound="uxListAttributeName_ItemDataBound" OnItemCommand="uxListAttributeName_ItemCommand">
            <ItemTemplate>
                <div class="row item-panel mb-5x">
                    <div class="col-xs-3 text-left">
                        <as:ValidatorLabel ID="uxSystemAttributeName" runat="server" ApplyFor="uxPreferredAttributeName" CssClass="text-dark-gray" />
                        <asp:HiddenField ID="uxSystemAttributeID" runat="server" />
                        <asp:HiddenField ID="uxRecordID" runat="server" />
                    </div>
                    <div class="col-xs-4 pad-rig mr-2x">
                        <asp:TextBox ID="uxPreferredAttributeName" CssClass="toggle-input form-control w-100" MaxLength="50" runat="server" />
                        <div class="label-error">
                            <as:ValidatorMessage runat="server" ID="uxPreferredAttributeNameMsg" ApplyFor="uxPreferredAttributeName" Message="" ShowOnLoad="False" />
                        </div>
                        <asp:TextBox ID="uxPreferredAttributeNameOld" CssClass="hide toggle-input-oldvalue form-control" runat="server" />
                    </div>
                    <div class="col-xs-4 text-left toggle-control no-marpad-im">
                        <as:Button runat="server" ID="uxUpdate" Text="Save" OnClientClick="return doValidation(this, '');" CommandName="Save" CssClass="btn btn-default ml-2x mr-2x hide" meta:resourcekey="uxUpdateResource" />
                        <as:Button runat="server" ID="uxCancel" CommandName="Cancel" OnClientClick="return false;" Text="Cancel" CausesValidation="False"
                            CssClass="btn btn-default hide btn-cancel ml-2x" data-toggle="collapse" IsStandardButton="False" meta:resourcekey="uxCancelResource" />
                    </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>
    <tek:RadCodeBlock runat="server">
        <script type="text/javascript">
            var requiredMsg = "<%= Resources.ValMsg.Required %>";
            var invalidMsg = "<%= GetLocalResourceObject("InvalidMsg") %>";
            var uniqueMsg = "<%= GetLocalResourceObject("uniqueMsg") %>";
            var sameMsg = "<%= GetLocalResourceObject("sameMsg") %>";
        </script>
        <script type="text/javascript" src='<%= ResolveUrl("~/") %>res/js/risk_MCF/rm_MCF_AttributeNameMaintenance.js'></script>
    </tek:RadCodeBlock>
</asp:Content>

