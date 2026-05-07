<%@ Control Language="C#" AutoEventWireup="true" CodeFile="LeadUserProfile.ascx.cs"
    Inherits="UserControls_Lead_LeadUserProfile" %>


<tr class="AltRow" runat="server" id="trBranchs" visible="false">
    <td class="heading w-30">
        <asp:Literal ID="Literal1" runat="server" Text="Branch Default Selection" meta:resourcekey="BranchDefault" />
    </td>
    <td>
          <as:RadComboBox ID="uxListBranch" runat="server"  Width="50%" MaxHeight="260px" EmptyMessage="Select Default Branch"
              DataTextField="Text" DataValueField="Value" meta:resourcekey="uxListValidQuestionResource1"  />
    </td>
</tr>
