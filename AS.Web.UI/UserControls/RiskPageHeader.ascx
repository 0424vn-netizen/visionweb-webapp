<%@ Control Language="C#" AutoEventWireup="true" CodeFile="RiskPageHeader.ascx.cs" Inherits="UserControls_RiskPageHeader" %>

<table cellpadding="0" cellspacing="5" border="0">
    <tr>
        <td class="AlignLeft LabelTextBold" style="width: 150px;">
            &nbsp;
        </td>
        <td class="AlignLeft" align="left">
            <div style="width: 250px; text-align: left; float: left;">
                <as:RadComboBox ID="uxSiteList" runat="server" MaxHeight="250px" Width="250" 
                OnClientSelectedIndexChanged="uxSiteList_SelectedIndexChanged" EnableEmbeddedSkins="false" 
                EnableEmbeddedBaseStylesheet="false" Label="Risk Sites:" Visible="false" meta:resourcekey="uxSiteListResource1"/>
            </div>
        </td>
        <td>
            &nbsp;
        </td>
        <td align="center">
            <as:Button ID="uxSubmit" Text="Submit" OnClick="uxSubmit_Click" runat="server" Visible= "False" IsStandardButton="False" meta:resourcekey="uxSubmitResource1" />
        </td>
    </tr>
</table>

<br />
    
<as:HiddenField ID="hf2" runat="Server" /> 

<as:RadCodeBlock ID="ScriptManagement" runat="server">

    <script type="text/javascript" language="javascript">
       

        function uxSiteList_SelectedIndexChanged(sender, eventArgs) {
            var item = eventArgs.get_item();
            document.getElementById("ctl00_ContentPage_uxRiskPageHeader_uxSiteList").value = item.get_text();
            $get("<%=hf2.ClientID%>").value = item.get_text();
        }
        
    </script>

</as:RadCodeBlock>
