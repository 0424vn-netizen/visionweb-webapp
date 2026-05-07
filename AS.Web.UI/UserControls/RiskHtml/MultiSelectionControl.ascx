<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MultiSelectionControl.ascx.cs" Inherits="UserControls_RiskHtml_MultiSelectionControl" %>

<%--Multi Select--%>
<%
    var multiSelects = KeywordItems;
%>
<% 
    if (multiSelects != null)
    {
        foreach (var multiSelect in multiSelects)
        {%>
<div type="MultiSelection" class="field" name="grp<%=multiSelect.Key%>" data-display-name="<%=multiSelect.Display%>" data-keyword="<%=multiSelect.Key%>"
    data-is-get-datasource="<%= multiSelect.IsGetDataSource %>" data-parent-key="<%= multiSelect.ParentKey %>">
    <div class="go-back">
        <a class="back-btn"><span class="icon-left-arrow"></span><%= GetGlobalResourceObject("AdvancedFilterResource","ButtonBack") %></a>
        <span class="suggestion-title"><%= multiSelect.Display %></span>
        <a class="pull-right close-btn"><%= GetGlobalResourceObject("AdvancedFilterResource","ButtonClose") %></a>
    </div>
    <a class="select-all"><%= GetGlobalResourceObject("AdvancedFilterResource","LinkSelectedAll") %></a>
    <div class="items  items-multiSelect">
        <%if (multiSelect.ValueItems != null)
          {
              foreach (var item in multiSelect.ValueItems)
              {
                  if (item.IsHidden)
                  {
                      continue;
                  }%>
        <div value="<%= HttpUtility.HtmlEncode(item.Value)%>" keyword="<%= HttpUtility.HtmlEncode(item.Key)%>" data-display-value="<%= HttpUtility.HtmlEncode(item.Display) %>">
            <div class="checkbox">
                <label>
                    <input type="checkbox" name="<%=(item.Key) %>" value="<%= HttpUtility.HtmlEncode(item.Value) %>" display-value="<%= HttpUtility.HtmlEncode(item.Display) %>" />
                    <span><%= HttpUtility.HtmlEncode(item.Display) %></span>
                </label>
            </div>
        </div>
        <% }
                                                      }%>
    </div>

</div>
<%}
                                                }%>
