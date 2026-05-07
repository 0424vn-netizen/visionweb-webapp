<%@ Control Language="C#" AutoEventWireup="true" CodeFile="RecentFilterList.ascx.cs" Inherits="UserControls_RiskHtml_RecentFilterList" %>
<%if (FilterSearchs != null && FilterSearchs.Count > 0)
  {
      foreach (var item in FilterSearchs)
      { %>
<li class="list-filter-item" data-filter-id="<%= item.FilterSearchID %>" data-filter-name="<%= item.FilterName %>" data-row-id="<%= item.FilterSearchID %>">
    <span class="filter-item"><%= item.FilterName %></span>
</li>
<%} %>
<%}
  else
  { %>
<p class="no-data-message"><%= GetGlobalResourceObject("AdvancedFilterResource","NoRecentFilterData") %></p>
<% }%>
