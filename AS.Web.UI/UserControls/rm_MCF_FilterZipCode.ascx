<%@ Control Language="C#" AutoEventWireup="true" CodeFile="rm_MCF_FilterZipCode.ascx.cs" Inherits="UserControls_rm_MCF_FilterZipCode" %>

<as:MultiSelector ID="uxZipCode" runat="server" DataTextField="DataText" XmlFilterItemsFilePath="~/App_Data/AssigmentFilters.xml" DataValueField="DataKey"
    AddText=">" RemoveText="<" WidthButton="26" WidthSelector="252" HeightSelector="320"
    HideAddAll="true" HideRemoveAll="true"
    ShowTooltip="false" CheckExisted="true"
    OnMovedData="MultiSelector_OnMovedData"
    CSSAddAllButton="btn-to-right-all" CSSRemoveAllButton="btn-to-left-all"
    CSSAddButton="btn-to-right" CSSRemoveButton="btn-to-left" EnableEmbeddedSkins="False" meta:resourcekey="uxOwnerLastNameResource1" SortExpression="" />