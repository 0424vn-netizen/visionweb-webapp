<%@ Control Language="C#" AutoEventWireup="true" CodeFile="rm_MCF_FilterMarketData.ascx.cs" Inherits="UserControls_rm_MCF_FilterMarketData" %>

<table style="text-align:left; margin-bottom: 5px;" width="100%">
    <tr >
        <td align="left" style="width:130px;">
            <as:Literal ID="ltMarketDataCode" runat="server" Text="Market Data Code Set:" meta:resourcekey="ltMarketDataCodeResource1"></as:Literal>
        </td>
        <td align="left">
            <as:RadComboBox ID="uxComboCodeSet" runat="server" EnableEmbeddedSkins="false" EnableEmbeddedBaseStylesheet="false"
                Width="50" Style="float:left; padding-left: 10px; margin-right: 20px;" AutoPostBack="false">
                <Items>
                    <as:RadComboBoxItem Text="1" Value="1" Selected="true" />
                    <as:RadComboBoxItem Text="2" Value="2" />
                    <as:RadComboBoxItem Text="3" Value="3" />
                    <as:RadComboBoxItem Text="4" Value="4" />
                    <as:RadComboBoxItem Text="5" Value="5" />
                    <as:RadComboBoxItem Text="6" Value="6" />
                    <as:RadComboBoxItem Text="7" Value="7" />
                    <as:RadComboBoxItem Text="8" Value="8" />
                    <as:RadComboBoxItem Text="9" Value="9" />
                </Items>
            </as:RadComboBox>
        </td>
    </tr>    
</table>

<as:Container ID="asContainer" runat="server" HeaderText="Market Data Type" TemplateName="ascontainer_greyborder.tpl" FooterControlID="" FooterText="" HeaderControlID="">
    <center>
        <as:MultiSelector ID="uxMarketDataFilter" runat="server" DataValueField="DataKey" DataTextField="DataText" FilterImageUrl="../res/images/Filter.png" 
            AddText=">" RemoveText="<" HeightSelector="180" WidthButton="47" CssClassTextBoxFilter="TextBox" HideAddAll="true" HideRemoveAll="true"
            ShowTooltip="false" CheckExisted="true" OnMovedData="MultiSelector_OnMovedData" CSSAddButton="ShortSubmitButton" CSSRemoveButton="ShortSubmitButton"
            XmlFilterItemsFilePath="~/App_Data/AssigmentFilters.xml" meta:resourcekey="uxMarketDataFilterResource1"/>
    </center>
</as:Container>

<as:RadAjaxManagerProxy ID="RadAjaxManagerProxy1" runat="server">
    <AjaxSettings>
        <tek:AjaxSetting AjaxControlID="uxComboCodeSet">
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="uxMarketDataFilter" />
            </UpdatedControls>
        </tek:AjaxSetting>
    </AjaxSettings>
</as:RadAjaxManagerProxy>