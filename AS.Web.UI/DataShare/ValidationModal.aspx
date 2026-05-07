<%@ Page Language="C#" MasterPageFile="~/MasterPagePopup.master" AutoEventWireup="true" CodeFile="ValidationModal.aspx.cs" Inherits="ValidationModal" 
    Title="Validation" Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="Server">
    <as:ASModalContainer ID="uxModalContainer" runat="server" WidthCssClass="modal-md">
        <div class="row" id="uxMessages-container">
            <div class="col-xs-12">
                <div class="box">
                    <%--<p>
                        <as:Literal ID="ltlDocument" runat="server" Text="The following must be completed to save this case." meta:resourcekey="ltlDocumentResource1"></as:Literal>
                    </p>--%>

                    <ul id="uxMessages" class="text-left ul-default ">
                    </ul>
                </div>
                <div class="box hide" id="Message">
                </div>
            </div>
            <div class="col-xs-12 form-action-container text-right">
                <as:Button runat="server" CssClass="btn btn-default" ID="uxContinue" Text="Continue" OnClientClick="return parent.HidePopupModal();" meta:resourcekey="uxContinueResource1" />
            </div>
        </div>
    </as:ASModalContainer>
    <script type="text/javascript">
        $(document).ready(function () {
            var message = parent.getMessages();
            if (message !== "")
                $("#uxMessages").html(message);
            else
                $("#uxMessages").parent().hide();
          
            var message = parent.getMessagesUpload();
            if (message.trim().length > 0) {
                  $("#Message").removeClass("hide");
            }
            $("#Message").html(message);
            $("#uxMessages-container").css("height", $("#uxMessages-container").height());
        });
    </script>
</asp:Content>
