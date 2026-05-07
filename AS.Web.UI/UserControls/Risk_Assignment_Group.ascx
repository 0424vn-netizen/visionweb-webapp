<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Risk_Assignment_Group.ascx.cs" Inherits="UserControls_Risk_Assignment_Group" %>
       <as:MultiSelector ID="uxGroupsSelector" runat="server" DataValueField="GroupID" DataTextField="GroupName"
            AddText=">" RemoveText="<"
            ShowTooltip="false" CheckExisted="true" OnMovedData="uxGroupsSelector_OnMovedData"  WidthButton="26" WidthSelector="252" HeightSelector="320"
            HideAddAll="true" HideRemoveAll="true" 
            CSSAddAllButton="btn-to-right-all" CSSRemoveAllButton="btn-to-left-all" CSSAddButton="btn-to-right" CSSRemoveButton="btn-to-left" 
            XmlFilterItemsFilePath="~/App_Data/AssigmentFilters.xml" EnableEmbeddedSkins="False" meta:resourcekey="uxGroupsSelectorResource1" SortExpression=""/>
  
