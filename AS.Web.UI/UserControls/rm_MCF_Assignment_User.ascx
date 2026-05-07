<%@ Control Language="C#" AutoEventWireup="true" CodeFile="rm_MCF_Assignment_User.ascx.cs" Inherits="UserControls_rm_MCF_Assignment_User" %>

<as:MultiSelector ID="uxUsersSelector" runat="server" DataValueField="UserID" DataTextField="DisplayName"
    AddText=">" RemoveText="<" ShowTooltip="false" CheckExisted="true"
    OnMovedData="uxUserSelector_OnMovedData" HideAddAll="true" HideRemoveAll="true"  WidthButton="26" WidthSelector="252" HeightSelector="320"
    CSSAddAllButton="btn-to-right-all" CSSRemoveAllButton="btn-to-left-all" CSSAddButton="btn-to-right" CSSRemoveButton="btn-to-left" 
    XmlFilterItemsFilePath="~/App_Data/AssigmentFilters.xml" EnableEmbeddedSkins="False" meta:resourcekey="uxUsersSelectorResource1" SortExpression="" />
