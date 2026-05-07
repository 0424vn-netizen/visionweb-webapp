<%@ Page Language="C#" MasterPageFile="~/MasterPagePopup.master" AutoEventWireup="true" Title="Site Access"
    CodeFile="SiteJump.aspx.cs" Inherits="page_SiteJump" meta:resourcekey="PageResource1" %>

<%@ Register TagName="PageTitle" Src="~/UserControls/PageTitle.ascx" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPage" runat="Server">
    <as:ASModalContainer ID="uxModalContainer" runat="server" WidthCssClass="modal-sm">

        <!--Header Page-->
        <div class="row">
            <div class="col-md-12">
                <h3 class="modal-title">
                    <asp:Literal ID="lblHeader" runat="server" Text="Site Access" meta:resourcekey="uxPageTitleResource1"></asp:Literal>
                </h3>
            </div>
        </div>
         
        <div class="row"> 
            <div class="col-md-12">
                <div class="form-inline dark-blue display-flex">
                    <div class="control-inline last valign-top text-left w-25">
                        <as:ValidatorLabel ID="txtUsernameLabel" CssClass="valign-middle first" runat="server" Text="User Name:" ApplyFor="uxUserID" meta:resourcekey="txtUsernameLabelResource1" />
                    </div>
                    <div class="control-inline w-60">
                        <as:TextBox ID="uxUserID" CssClass="form-control" Width="100%" runat="server" MaxLength="50" HintCss="hint" 
                            meta:resourcekey="uxUserIDResource1" autocomplete="off"  autocorrect="off"/>
                        <div class="ml-m-2x"><as:ValidatorMessage runat="server" ID="ltrMsg" ApplyFor="uxUserID" Message="" ShowOnLoad="False" /></div>
                    </div>
                    <div class="control-inline w-15 mr-0">
                        <as:Button ID="uxGo" runat="server" CssClass="btn btn-default ml-0-i" Text="Submit" OnClick="uxGo_Click"
                            OnClientClick="if (!ValidateInput()){AdjustModalSize(); return false;}" IsStandardButton="False" meta:resourcekey="uxGoResource1" />
                    </div>
                </div>
            </div>
        </div>
        <as:Validator ID="Validator1" runat="server" ValidationFunction="ValidateInput" MessageContainerClientID="" MessageType="Inline" meta:resourcekey="Validator1Resource1">
            <Items>
                <as:BasicValidationItem ControlToValidateID="uxUserID" Rule="Required" Message="This is a required field" IsInAjaxPanel="False" ResMessage="" ResParams="" meta:resourcekey="SiteJump_aspx_BV_Required" />
                <as:BasicValidationItem ControlToValidateID="uxUserID" Rule="Minlength" MinLength="2"
                    Message="You must enter at least 2 characters." IsInAjaxPanel="False" ResMessage="" ResParams="" meta:resourcekey="SiteJump_aspx_BV_AtLeastCharacter" />                 
                <as:BasicValidationItem ControlToValidateID="uxUserID" Pattern="[`~!@#$%^&*()+=|\\{[}]:';\\{[}]:;\,.?<>/ ]" Rule="StringUnaccept" Message="This field must be alpha characters or numbers, and the length must be from 2 to 50 characters." meta:resourcekey="SiteJump_aspx_StringUnaccept"/>
                <as:CustomValidationItem ControlToValidateID="uxUserID" Message="This field must be alpha characters or numbers, and the length must be from 2 to 50 characters."
                    ClientValidationFunction="doValidation" meta:resourcekey="SiteJump_aspx_StringUnaccept" />
                 
            </Items>
        </as:Validator>

    </as:ASModalContainer>

    <script type="text/javascript">
        var uxUserID_ClientID = '<%=uxUserID.ClientID%>';  
    </script>
    <script src="<%= ResolveUrl("~/")%>res/js/SiteJump.js"></script>
</asp:Content>
