<%@ Control Language="C#" AutoEventWireup="true" CodeFile="rm_MCF_Filter_HRCode_Modal.ascx.cs" Inherits="UserControls_rm_MCF_Filter_HRCode_Modal" %>

<as:MultiSelector ID="uxFilterHRCode" runat="server" DataTextField="DataText" XmlFilterItemsFilePath="~/App_Data/AssigmentFilters.xml" DataValueField="DataKey"
    AddText=">" RemoveText="<" WidthButton="26" WidthSelector="330" HeightSelector="320"
    HideAddAll="true" HideRemoveAll="true"
    ShowTooltip="false" CheckExisted="true"
    OnMovedData="MultiSelector_OnMovedData"
    CSSAddAllButton="btn-to-right-all" CSSRemoveAllButton="btn-to-left-all"
    CSSAddButton="btn-to-right" CSSRemoveButton="btn-to-left" EnableEmbeddedSkins="False" meta:resourcekey="uxProfilesResource1" SortExpression="ObjectID" />
