<%@ Page Language="C#" MasterPageFile="~/MasterPagePopup.master" AutoEventWireup="true" CodeFile="rm_MCF_AutoQueueMonitoringViewAllChanges.aspx.cs" Inherits="rm_MCF_AutoQueueMonitoringViewAllChanges" Title="VIEW ALL CHANGES" meta:resourcekey="PageTitleResource1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPage" runat="Server">
    <as:ASModalContainer ID="uxModalContainer" runat="server" WidthCssClass="modal-md">
        <h4 class="title-auto-queue mt-m-1x">
            <asp:Literal ID="uxTilteResource1" runat="server" meta:resourcekey="TitleResource1"> All Changes</asp:Literal></h4>
        <div class="list-group-border-white">
            <as:ASRepeater runat="server" ID="uxChangeLog">
                <ItemTemplate>
                    <div class="item">
                        <div class="row">
                            <div class="col-xs-7">
                                <span class="text-gray-light"><%# Eval("ActionTypeDesc")%></span>
                            </div>
                            <div class="col-xs-5 text-right">
                                <span><%# Eval("CreatedDTS")%></span>
                            </div>
                        </div>
                    </div>
                </ItemTemplate>
            </as:ASRepeater>
        </div>

        <div class="text-right mt-4x">
            <asp:Button ID="uxClose" runat="server" CssClass="btn btn-default" Text="Close" meta:resourcekey="uxCloseResource1"
                OnClientClick="parent.HidePopupModal(); return false;" />
        </div>
    </as:ASModalContainer>
</asp:Content>
