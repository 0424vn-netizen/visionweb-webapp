<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Title="Reset MS Password"
    CodeFile="ResetMSUsers.aspx.cs" Inherits="ResetMSUsers" ValidateRequest="false" meta:resourcekey="PageResource1" %>

<%@ Register Src="~/UserControls/PageTitle.ascx" TagName="PageTitle" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPage" runat="Server">
    <div class="row">
        <div class="col-md-12">
            <uc:PageTitle ID="uxPageTitle" HasFilteringOption="false" runat="server" ReportTitle="Reset MS Password" meta:resourcekey="uxPageTitleResource1" />
        </div>
    </div>
    <asp:Panel ID="pnlPreventEnter" runat="server" DefaultButton="btnSubmit" meta:resourcekey="pnlPreventEnterResource1">

        <div class="form-inline dark-blue">
            <div class="control-inline last valign-top">
                <as:ValidatorLabel ID="txtUsernameLabel" CssClass="valign-middle" runat="server" Text="MS User Name:" ApplyFor="txtUsername" meta:resourcekey="txtUsernameLabelResource1" />
            </div>
            <div class="inline-block">
                <asp:TextBox ID="txtUsername" CssClass="form-control" runat="server" Width="200px" MaxLength="50" 
                    meta:resourcekey="txtUsernameResource1" autocomplete="off" autocorrect="off"/>
                <as:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="btn btn-default valign-top"
                    ValidationGroup="ValidatePage"
                    OnClick="btnSubmit_Click" 
                    OnClientClick="return ValidateResetMSUser();" IsStandardButton="False" meta:resourcekey="btnSubmitResource1" />

                <div class="">
                    <as:ValidatorMessage runat="server" ID="uxMsg" ApplyFor="txtUsername" Message="" ShowOnLoad="False" />
                </div>
            </div> 
        </div>

    </asp:Panel>
    <as:Validator ID="Validator1" runat="server" ValidationFunction="ValidateInput" MessageContainerClientID="" MessageType="Inline" meta:resourcekey="Validator1Resource1">
        <Items>
            <as:BasicValidationItem ControlToValidateID="txtUsername" Rule="Required" Message="This is a required field." meta:resourcekey="BasicValidator1Resource1Resource1"/>
            <as:BasicValidationItem ControlToValidateID="txtUsername" Pattern="[`~!@#$%^&*()+=|\\{[}]:';\\{[}]:;\,.?<>/ ]" Rule="StringUnaccept" Message="This field must be alpha characters or numbers, and the length must be from 1 to 50 characters." meta:resourcekey="BasicValidator1Resource2Resource1"/>
            <as:CustomValidationItem ControlToValidateID="txtUsername" ClientValidationFunction="CheckUsername" Message="This field must be alpha characters or numbers, and the length must be from 1 to 50 characters." meta:resourcekey="BasicValidator1Resource2Resource1"/>
        </Items>
    </as:Validator>
    <script type="text/javascript">
        var txtUsername_ClientID = '<%=txtUsername.ClientID%>'; 
    </script>
    <script src="<%= ResolveUrl("~/")%>res/js/usermaintenance/ResetMSUserPwd.js"></script>
</asp:Content>
<%--&#34;--%>