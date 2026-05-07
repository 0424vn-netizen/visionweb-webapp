<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Risk_ParameterFilter_TransactionCode.ascx.cs"
    Inherits="UserControls_Risk_ParameterFilter_TransactionCode" %>


<as:MultiSelector ID="uxHierarchyFilter" runat="server" DataValueField="DataKey"
    DataTextField="DataText"
    AddText=">"
    RemoveText="<" WidthButton="25" WidthSelector="250" HeightSelector="320"
    HideAddAll="true" HideRemoveAll="true"
    ShowTooltip="false" CheckExisted="true"
    OnMovedData="MultiSelector_OnMovedData"
    CSSAddAllButton="btn-to-right-all" CSSRemoveAllButton="btn-to-left-all" CSSAddButton="btn-to-right" CSSRemoveButton="btn-to-left" meta:resourcekey="uxHierarchyFilterResource1" />
