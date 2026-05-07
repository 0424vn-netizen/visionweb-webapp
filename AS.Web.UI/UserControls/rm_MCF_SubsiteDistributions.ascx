<%@ Control Language="C#" AutoEventWireup="true" CodeFile="rm_MCF_SubsiteDistributions.ascx.cs" Inherits="As.VisionWeb.Web.SubsiteDistributionsUserControl" %>
<div class="height-16"></div>
<div class="row">
    <div class="col-md-12">
        <as:VWMultiSelector ID="uxHierarchyFilter" runat="server" DataValueField="DataKey" DataTextField="DataText"
            AddText=">" RemoveText="<" WidthButton="26" WidthSelector="252" HeightSelector="320"
            HideAddAll="true" HideRemoveAll="true"
            ShowTooltip="false" CheckExisted="true" OnMovedData="MultiSelector_OnMovedData" XmlFilterItemsFilePath="~/App_Data/AssigmentFilters.xml"
            CSSAddAllButton="btn-to-right-all" CSSRemoveAllButton="btn-to-left-all" CSSAddButton="btn-to-right" CSSRemoveButton="btn-to-left"
            EnableEmbeddedSkins="False" meta:resourcekey="uxHierarchyFilterResource1" SortExpression="" />
    </div>
</div>

