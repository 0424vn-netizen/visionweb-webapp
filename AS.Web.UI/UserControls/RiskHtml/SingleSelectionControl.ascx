<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SingleSelectionControl.ascx.cs" Inherits="UserControls_RiskHtml_SingleSelectionControl" %>

<%--Single Selection--%>
<%
    var singleSelection = KeywordItems;
%>
<% 
    if (singleSelection != null)
    {
        foreach (var single in singleSelection)
        {%>
<div type="SingleSelection" class="field" name="grp<%=single.Key%>" data-display-name="<%=single.Display%>" data-keyword="<%=single.Key%>"
    data-is-get-datasource="<%= single.IsGetDataSource %>" data-parent-key="<%= single.ParentKey %>">
    <div class="go-back">
        <a class="back-btn"><span class="icon-left-arrow"></span><%= GetGlobalResourceObject("AdvancedFilterResource","ButtonBack") %></a>
        <span class="suggestion-title"><%= single.Display %></span>
        <a class="pull-right close-btn"><%= GetGlobalResourceObject("AdvancedFilterResource","ButtonClose") %></a>
    </div>
    <div class="items items-single">
        <%if (single.ValueItems != null)
          {
              foreach (var item in single.ValueItems)
              {
                  if (item.IsHidden)
                  {
                      continue;
                  }%>
        <div value="<%= HttpUtility.HtmlEncode(item.Value)%>" keyword="<%= HttpUtility.HtmlEncode(item.Key)%>" data-display-value="<%= HttpUtility.HtmlEncode(item.Display) %>">
            <label><%=  HttpUtility.HtmlEncode(item.Display)%></label>
        </div>
        <% }
                                                      }%>
    </div>
</div>
<%}
                                                }%>
