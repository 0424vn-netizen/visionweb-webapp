<%@ Control Language="C#" AutoEventWireup="true" CodeFile="rm_MCF_FilterHierarchy.ascx.cs" Inherits="UserControls_rm_MCF_FilterHierarchy" %>

<as:VWMultiSelector ID="uxHierarchyFilter" runat="server" DataValueField="DataKey" DataTextField="DataText" 
    AddText=">" RemoveText="<" WidthButton="26" WidthSelector="252" HeightSelector="320"
    HideAddAll="true" HideRemoveAll="true"
    ShowTooltip="true" CheckExisted="true" OnMovedData="MultiSelector_OnMovedData" XmlFilterItemsFilePath="~/App_Data/AssigmentFilters.xml"  
    CSSAddAllButton="btn-to-right-all" CSSRemoveAllButton="btn-to-left-all" CSSAddButton="btn-to-right" CSSRemoveButton="btn-to-left" EnableEmbeddedSkins="False" meta:resourcekey="uxHierarchyFilterResource1" SortExpression=""  />
