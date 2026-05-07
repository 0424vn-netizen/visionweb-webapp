<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Risk_Assignment_User.ascx.cs" Inherits="UserControls_Risk_Assignment_User" %>

<as:MultiSelector ID="uxUsersSelector" runat="server" DataValueField="UserID" DataTextField="DisplayName"
    AddText=">" RemoveText="<" ShowTooltip="false" CheckExisted="true"
    OnMovedData="uxUserSelector_OnMovedData" HideAddAll="true" HideRemoveAll="true"  WidthButton="26" WidthSelector="252" HeightSelector="320"
    CSSAddAllButton="btn-to-right-all" CSSRemoveAllButton="btn-to-left-all" CSSAddButton="btn-to-right" CSSRemoveButton="btn-to-left" 
    XmlFilterItemsFilePath="~/App_Data/AssigmentFilters.xml" EnableEmbeddedSkins="False" meta:resourcekey="uxUsersSelectorResource1" SortExpression="" />
