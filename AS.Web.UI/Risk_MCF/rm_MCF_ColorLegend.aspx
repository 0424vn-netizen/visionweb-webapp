<%@ Page Language="C#" MasterPageFile="~/MasterPagePopup.master" AutoEventWireup="true"
    CodeFile="rm_MCF_ColorLegend.aspx.cs" Inherits="rm_MCF_ColorLegend" Title="Color Legend" meta:resourcekey="PageResource1" %>

<%--<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">--%>
<%@ Register Src="~/UserControls/rm_MCF_Parameter_ReadOnly.ascx" TagName="UxParameterRo" TagPrefix="uc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="Server">
    <as:PlaceHolder ID="sdf" runat="server">
    <as:RadAjaxManagerProxy ID="ram" runat="server">
        <AjaxSettings>
            <tek:AjaxSetting AjaxControlID="uxParam">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="uxParam" />
                </UpdatedControls>
            </tek:AjaxSetting>
        </AjaxSettings>
    </as:RadAjaxManagerProxy>

    <as:ASModalContainer ID="uxModalContainer" runat="server" WidthCssClass="modal-lg">

        <as:PlaceHolder ID="pnlMerchantNumberColor" runat="server" Visible="true">
            <!-- signifies a merchant on Watch -->             
            <div class="height-8"></div>
            <p>(<span class="red">*</span>) <asp:Literal ID="rm_ColorLegend_aspx_Text5" runat="server" meta:resourcekey="rm_ColorLegend_aspx_Text5Resource1">signifies a merchant on Watch</asp:Literal></p>
            <div class="height-8"></div>
            <h3 class="modal-title"><asp:Literal ID="rm_ColorLegend_aspx_Text6" runat="server" meta:resourcekey="rm_ColorLegend_aspx_Text6Resource1">Parameter</asp:Literal></h3>
        </as:PlaceHolder>
        <uc:UxParameterRo ID="uxParam" runat="server" OnNeedDataSource="uxParam_NeedDataSource"
            IsColorMode="true" />
    </as:ASModalContainer>
        </as:PlaceHolder>
</asp:Content>
