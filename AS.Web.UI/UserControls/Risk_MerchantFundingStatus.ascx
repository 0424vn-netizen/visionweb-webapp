<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Risk_MerchantFundingStatus.ascx.cs" Inherits="UserControls_Risk_MerchantFundingStatus" %>
<as:MultiSelector ID="uxMerchantFundingStatus" runat="server" DataTextField="DataText" XmlFilterItemsFilePath="~/App_Data/AssigmentFilters.xml" DataValueField="DataKey"
    AddText=">" RemoveText="<" WidthButton="26" WidthSelector="252" HeightSelector="320"
    HideAddAll="true" HideRemoveAll="true"
    ShowTooltip="false" CheckExisted="true"
    OnMovedData="MultiSelector_OnMovedData"
    CSSAddAllButton="btn-to-right-all" CSSRemoveAllButton="btn-to-left-all"
    CSSAddButton="btn-to-right" CSSRemoveButton="btn-to-left" EnableEmbeddedSkins="False" meta:resourcekey="uxOwnerLastNameResource1" SortExpression="" />