<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPagePopup.master" CodeFile="ViewOrganizationModal.aspx.cs" Inherits="ViewOrganizationModal" meta:resourcekey="PageResource1" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="Server">
    <as:ASModalContainer ID="uxModalContainer" runat="server" WidthCssClass="modal-md" ContainerCssClass="container">
        <div>

            <h2 class="modal-title">
                <asp:Literal ID="Literal8" runat="server" Text="View Organizations" meta:resourcekey="LiteralResource1" /></h2>

            <div class="row mt-5x display-flex align-center">
                <div class="col-xs-7">
                    <div class="font-20 text-dark-gray">
                        <asp:Literal ID="Literal9" runat="server" Text="Organizations" meta:resourcekey="LiteralResource2" />
                    </div>
                </div>
            </div>


            <div class="height-6"></div>
            <div id="uxOrganization" class="box padding-6x ptb-0 height-300 overflow-auto ">
                <div class="pt-9  pb-12">
                    <asp:Label runat="server" CssClass="hint-text" ID="uxNoDataMessage" meta:resourcekey="uxNoDataMessage" Visible="false"></asp:Label>
                    <asp:Repeater runat="server" ID="uxOrganizationList">
                        <ItemTemplate>
                            <div class="pb-10">
                                <%# Eval("OrganizationName") %>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
            </div>

            <div class="row">
                <div class="col-xs-12 form-action-container text-right">
                    <as:Button class="btn btn-default" ID="btnClose" Text="Close" OnClientClick="ClosePopupModal();" runat="server" meta:resourcekey="bntClose" />
                </div>
            </div>
        </div>
    </as:ASModalContainer>
</asp:Content>
