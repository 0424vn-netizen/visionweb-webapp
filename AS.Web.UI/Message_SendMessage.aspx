<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="Message_SendMessage.aspx.cs" Inherits="gen_Message_SendMessage"
    Title="MESSAGES" meta:resourcekey="PageResource1" %>

<%@ Register TagName="MessageGrid" Src="~/UserControls/MessageGrid.ascx" TagPrefix="uc" %>
<%@ Register TagName="PageTitle" Src="~/UserControls/PageTitle.ascx" TagPrefix="uc" %>
<%@ Register TagName="MessageEditor" Src="~/UserControls/MessageEditor.ascx" TagPrefix="uc" %>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPage" runat="Server">
    <div>
        
        <table width="100%">
            <tr>
                <td>
                    <uc:PageTitle ID="uxPageTitle" runat="server" ReportTitle="MESSAGES" meta:resourcekey="uxPageTitleResource1" />
                </td>
            </tr>
            <tr>
                <td>
                    <uc:MessageGrid ID="uxGridMessage" runat="server" />
                </td>
            </tr>
                <tr>
                    <td align="right" style="padding: 0px 1px 0px 0px">
                        <as:Button ID="uxBtnPastMessages" runat="server" Text="Past Messages" OnClientClick=" return ShowPopupModal ('Message_PastMessage.aspx',1000,745);" IsStandardButton="False" meta:resourcekey="uxBtnPastMessagesResource1" />
                    </td>
                </tr>
        </table>
        <div style="width: 70%;">
            <table width="100%">
                <tr>
                    <td style="padding:0px 5px 0px 0px">
                        <uc:MessageEditor ID="uxMessageEditor" HeaderText="<b>New Message</b>" MaxCharacter="2000"
                            runat="server" />
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        <as:Button ID="uxBtnSendMessage" runat="server" Text="Send" OnClick="uxBtnSendMessage_Click" OnClientClick="return MessageEditor_ValidateMessage();" Width="90px" IsStandardButton="False" meta:resourcekey="uxBtnSendMessageResource1"/>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <as:RadAjaxManagerProxy runat="server" ID="uxRadAjaxManager">
        <AjaxSettings>
            <tek:AjaxSetting AjaxControlID="uxBtnPastMessages">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="uxBtnPastMessages" />
                </UpdatedControls>
            </tek:AjaxSetting>
        </AjaxSettings>
    </as:RadAjaxManagerProxy>
</asp:Content>
