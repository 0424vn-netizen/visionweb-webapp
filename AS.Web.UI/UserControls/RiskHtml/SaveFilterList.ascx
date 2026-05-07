<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SaveFilterList.ascx.cs" Inherits="UserControls_RiskHtml_SaveFilterList" %>

<%if (FilterSearchs != null && FilterSearchs.Count > 0)
  {
      foreach (var item in FilterSearchs)
      { %>
<li class="list-filter-item display-flex space-between" data-filter-id="<%= item.FilterSearchID %>" data-filter-name="<%= item.FilterName %>" data-row-id="<%= item.FilterSearchID %>">
    <span class="filter-item"><%= item.FilterName %></span>
    <div class="dropdown">
        <div class="action-more dropdown-toggle" id="action_<%= item.FilterSearchID %>" data-toggle="dropdown">
            <div class="item-discussion"></div>
            <div class="item-discussion"></div>
            <div class="item-discussion"></div>
        </div>
        <ul class="dropdown-menu dropdown-menu-right" aria-labelledby="action_<%= item.FilterSearchID %>">
            <li><a data-none-close-toggle="true" onclick="advancedFilter.currentFilter.toggleRenameForm('<%= item.FilterSearchID %>')"><%= GetGlobalResourceObject("AdvancedFilterResource","ActionRename") %></a></li>
            <li><a onclick="advancedFilter.currentFilter.showDeleteFilterModal('<%= item.FilterSearchID %>')"><%= GetGlobalResourceObject("AdvancedFilterResource","ActionDelete") %></a></li>
        </ul>
        <div class="rename-filter-container hide" data-rename-form="true">
            <div class="flex">
                <input data-none-close-toggle="true" maxlength="250" class="form-control" id="rename_input_<%= item.FilterSearchID %>" name="rename_input" type="text" value="">
                <a data-none-close-toggle="true" href="javascript:void(0)" class="action-link action-item" onclick="advancedFilter.currentFilter.renameFilterById('<%= item.FilterSearchID%>')"><%= GetGlobalResourceObject("AdvancedFilterResource","ButtonSave") %></a>
                <a href="javascript:void(0)" class="action-link action-item" onclick="advancedFilter.currentFilter.toggleRenameForm('<%= item.FilterSearchID%>', false)"><%= GetGlobalResourceObject("AdvancedFilterResource","ButtonCancel") %></a>
            </div>
            <p class="error"></p>
        </div>
    </div>
</li>
<%} %>
<%}
  else
  { %>
<p class="no-data-message"><%= GetGlobalResourceObject("AdvancedFilterResource","NoSavedFilterData") %></p>
<% }%>
