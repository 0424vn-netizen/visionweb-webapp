<%@ Control Language="C#" AutoEventWireup="true" CodeFile="LeadManageBank.ascx.cs" Inherits="UserControls_Lead_LeadManageBank" %>

<tek:RadAjaxManagerProxy ID="RadAjaxManager12" runat="server">
    <AjaxSettings>
        <tek:AjaxSetting AjaxControlID="uxBankListSelected">
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="uxBankListSelected" />
                <tek:AjaxUpdatedControl ControlID="uxSubmit" />
                <tek:AjaxUpdatedControl ControlID="LiteralSeletedBank" />
                <%-- <tek:AjaxUpdatedControl ControlID="uxHddSelectedRole" />--%>
            </UpdatedControls>
        </tek:AjaxSetting>
    </AjaxSettings>
</tek:RadAjaxManagerProxy>
<as:Panel ID="panelAddBank" runat="server">

    <h3 class="modal-title">
        <as:Literal runat="server" ID="lblBankTitle" ClientIDMode="Static" meta:resourcekey="Label_Assign_Bank_Title" />
    </h3>

    <div class="row">
        <div class="col-md-12">
            <div class="w-50 inline-block">
                <div class="w-60 inline-block">
                    <h4>
                        <as:Literal ID="lblAvailableBank" ClientIDMode="Static" runat="server" meta:resourcekey="Label_Available_Bank_Title" />
                    </h4>
                </div>
                <div class="w-30 inline-block text-right">
                    <as:LinkButton ID="uxSelectAll" CssClass="link-back" Text="Select All" runat="server" OnClick="uxSelectAll_Click" meta:resourcekey="LinkButton_Select_All" />
                </div>
            </div>
            <div class="w-40 inline-block ml-6x">
                <h4>
                    <as:Literal ID="lblSelectedBank" ClientIDMode="Static" runat="server" meta:resourcekey="Label_Bank_Selected_Title" />
                </h4>
            </div>

            <div class="height-6"></div>
            <div class="row">
                <div class="col-md-12 text-center">
                    <as:MultiSelector ID="uxBankListSelected" runat="server" DataValueField="BankId" ClientIDMode="Static" CssClassTextBoxFilter="js-textbox-filter"
                        DataTextField="BankName" AddText=">>" RemoveText="<<" WidthSelector="261"
                        HeightSelector="150" WidthButton="25"
                        ShowFilter="true" SelectionMode="Multiple" OnPreRender="uxBankListSelected_PreRender"
                        CSSAddAllButton="btn-to-right-all" CSSRemoveAllButton="btn-to-left-all" CSSAddButton="btn-to-right" CSSRemoveButton="btn-to-left"
                        HideAddAll="true" HideRemoveAll="true" ShowTooltip="false"
                        CheckExisted="true" EnableEmbeddedSkins="False" meta:resourcekey="uxRoleListSelectedResource1" />
                </div>
            </div>
            <div class="height-6"></div>
            <div class="row">
                <div class="col-md-12">
                    <div class="inline-block">
                        <asp:Label ID="lb" Text="Selected Count: " runat="server" meta:resourcekey="Label_Selected_Count" CssClass="inline-block" /></div>
                    <div class="inline-block">
                        <asp:Literal ID="LiteralSeletedBank" runat="server" /></div>
                </div>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-md-12 form-action-container text-right">
            <div class="inline-block">
                <as:Button ID="uxSubmit" runat="server" Text="Submit" CssClass="btn btn-default" IsStandardButton="False" OnClick="uxSubmit_Click" meta:resourcekey="Button_Submit" />
            </div>
            <div class="inline-block">
                <as:Button runat="server" class="btn btn-default" ID="uxCancel" Text="Cancel" OnClientClick="return CloseMe();" IsStandardButton="False" meta:resourcekey="Button_Cancel" />
            </div>
        </div>
    </div>
</as:Panel>
<as:Panel ID="panelViewBank" runat="server">
    <div class="row">
        <div class="col-md-12">
            <h3 class="modal-title">
                <as:Literal ID="ltrViewBankTitle" runat="server" meta:resourcekey="Label_View_Bank_Title" />
            </h3>

            <h4>
                <as:Literal ID="ltrViewBankSubTitle" runat="server" meta:resourcekey="Label_Bank_Title" />
            </h4>
        </div>
    </div>
    <div class="height-4"></div>

    <div class="box padding-6x ptb-0 height-200 overflow-auto">
        <div class="pt-9  pb-12">
            <as:ASRepeater ID="uxRepeaterViewBank" runat="server" NumberOfColumns="1">
                <ItemTemplate>
                    <div class="pb-10"><%# Eval(ViewBankDisplayField) %> </div>
                </ItemTemplate>
            </as:ASRepeater>
        </div>
    </div>

    <div class="row">
        <div class="col-md-12 form-action-container text-right">
            <div class="inline-block">
                <as:Button runat="server" class="btn btn-default" ID="uxClose" Text="Close" OnClientClick="return CloseMe();" IsStandardButton="False" meta:resourcekey="Button_Close" />
            </div>
        </div>
    </div>
</as:Panel>
<tek:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <script type="text/javascript" src="<%= ResolveUrl("~/") %>res/js/lead/LeadManageBank.js"></script>
</tek:RadCodeBlock>
