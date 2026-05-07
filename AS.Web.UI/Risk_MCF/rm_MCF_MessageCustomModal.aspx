<%@ Page Title="Confirm" Language="C#" MasterPageFile="~/MasterPagePopup.master" meta:resourcekey="PageResource1"
    AutoEventWireup="true" CodeFile="rm_MCF_MessageCustomModal.aspx.cs" Inherits="rm_MCF_MessageCustomModal" %>

<asp:Content ContentPlaceHolderID="ContentPage" runat="server" ID="uxMainModalContent">
    <as:ASModalContainer ID="uxModalContainer" runat="server" ContainerCssClass="container" WidthCssClass="modal-md">
        <div class="row customizeModal ">
            <div class="col-xs-12">
                <div>
                    <as:Literal ID="lblMessage" runat="server" Text="Message"></as:Literal>
                </div>
                <div>
                    <ul class="message-list">
                        <as:ASRepeater runat="server" ID="uxRptMessage">
                            <ItemTemplate>
                                <li><%# Eval("Text")%></li>
                            </ItemTemplate>
                        </as:ASRepeater>
                    </ul>
                </div>
                <div>
                    <as:Literal ID="lblMesssageFooter" runat="server" Text="Message Footer"></as:Literal>
                </div>
            </div>
        </div>
        <div class="row">
            <as:HiddenField ID="txtViewName" runat="server" />
            <div class="col-xs-12 text-right form-action-container">
                <asp:Button ID="uxSubmit" meta:resourcekey="uxSubmitResource" Width="65" class="btn btn-default" OnClientClick="onSubmitData()" runat="server" Text="OK"></asp:Button>
                <as:Button runat="server" meta:resourcekey="uxCancelResource" class="btn btn-default" ID="uxCancel" Text="Cancel" OnClientClick="parent.ClosePopupModal(3);" IsStandardButton="False" />
            </div>
        </div>
        <tek:RadCodeBlock ID="uxRadCodeBlock" runat="server">
            <script type="text/javascript">
                var msg_custom_Modal_more ="<%=GetLocalResourceObject("MessagageCustomModal_More").ToString() %>"
            </script>
            <script type="text/javascript" src="<%=ResolveUrl("~") %>res/js/risk_MCF/rm_MCF_MessageCustomModal.js"></script>
        </tek:RadCodeBlock>
    </as:ASModalContainer>
</asp:Content>
