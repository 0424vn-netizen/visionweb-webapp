<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPagePopup.master" AutoEventWireup="true" CodeFile="ReferenceNumberDetail.aspx.cs" Inherits="Risk_ReferenceNumberDetail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="Server">

    <as:ASModalContainer ID="ASModalContainer1" runat="server" WidthCssClass="modal-md">
        <uc:PageTitle ID="uxReportTitle" runat="server" ReportTitle="Detail" meta:resourcekey="uxReportTitleResource1" />
        <div class="row">
            <div class="col-xs-12">
            </div>
        </div>

        <div class="row">
            <div class="col-xs-12 form-action-container text-right">
                <as:Button class="btn btn-default" ID="Button1" Text="Close" OnClientClick="ClosePopupModal();" runat="server" meta:resourcekey="bntCancelResource" />
            </div>
        </div>
    </as:ASModalContainer>
</asp:Content>

