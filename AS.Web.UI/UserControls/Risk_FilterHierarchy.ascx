<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Risk_FilterHierarchy.ascx.cs" Inherits="UserControls_Risk_FilterHierarchy" %>

<as:MultiSelector ID="uxHierarchyFilter" runat="server" DataValueField="DataKey" DataTextField="DataText" 
    AddText=">" RemoveText="<" WidthButton="26" WidthSelector="252" HeightSelector="320"
    HideAddAll="true" HideRemoveAll="true"
    ShowTooltip="false" CheckExisted="true" OnMovedData="MultiSelector_OnMovedData" XmlFilterItemsFilePath="~/App_Data/AssigmentFilters.xml"  
    CSSAddAllButton="btn-to-right-all" CSSRemoveAllButton="btn-to-left-all" CSSAddButton="btn-to-right" CSSRemoveButton="btn-to-left" EnableEmbeddedSkins="False" meta:resourcekey="uxHierarchyFilterResource1" SortExpression=""  />
