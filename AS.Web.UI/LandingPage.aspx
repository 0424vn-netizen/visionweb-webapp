<%@ Page Title="Landing Page" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="LandingPage.aspx.cs" 
    Inherits="LandingPage" meta:resourcekey="PageResource1" %>
    
<%@ Register TagName="PageTitle" Src="~/UserControls/PageTitle.ascx" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" Runat="Server">

<uc:PageTitle ID="uxPageTitle" runat="server" ReportTitle="Landing Page" HasFilteringOption="false" meta:resourcekey="uxPageTitleResource1" />

    <p>
        <as:Literal ID="ltClickToChooseClient" runat="server" Text="Click on a logo to have access to a specific client." meta:resourcekey="ltClickToChooseClientResource1"></as:Literal>
    </p>
    <div class="row">
        <div class="col-md-12 text-center">
            <asp:DataList ID="uxClientList" runat="server" OnItemDataBound="uxClient_ItemDataBound" RepeatColumns="6" CssClass="inline-block" meta:resourcekey="uxClientListResource1">
                <ItemTemplate>
                    <div class="landing-item-container">
                        <as:LinkButton ID="uxSiteJump" CssClass="landing-item-logo" runat="server" OnCommand="SiteJumpCommand"
                            CommandArgument='<%# Eval("ASClient") %>' meta:resourcekey="uxSiteJumpResource1" />
                        <div class='landing-item-edit'>
                            <img src="res/images/Landing/edit_icon.png" alt="Edit" style="border: 0px none; cursor: pointer;"
                                onclick='return ShowPopupModal(&#039;ClientSetting.aspx?<%# GetClientIDParam(Eval("ASClient")) %>&#039;,&#039;auto&#039;);' />
                        </div>
                    </div>
                </ItemTemplate>
            </asp:DataList>
        </div>
    </div>
    
    <div class="display-none">
        <as:Button ID="uxReLoad" runat="server" OnClick="uxReload" IsStandardButton="False" meta:resourcekey="uxReLoadResource1" />
    </div>
    <script type="text/javascript">
        function master_closeModalEvent() {
            document.getElementById('<%=uxReLoad.ClientID%>').click();
        }
    </script>
</asp:Content>

