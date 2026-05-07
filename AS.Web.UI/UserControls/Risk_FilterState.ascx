<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Risk_FilterState.ascx.cs" Inherits="UserControls_Risk_FilterState" %>

<as:MultiSelector ID="uxFilterState" runat="server" DataValueField="DataKey" DataTextField="DataText"
    AddText=">" RemoveText="<" WidthButton="26" WidthSelector="252" HeightSelector="320"
    HideAddAll="true" HideRemoveAll="true"
    ShowTooltip="false" CheckExisted="true"
    OnMovedData="MultiSelector_OnMovedData"
    CSSAddAllButton="btn-to-right-all" CSSRemoveAllButton="btn-to-left-all" CSSAddButton="btn-to-right" CSSRemoveButton="btn-to-left"
    XmlFilterItemsFilePath="~/App_Data/AssigmentFilters.xml" EnableEmbeddedSkins="False" meta:resourcekey="uxFilterStateResource1" SortExpression="" />
