<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SubFilterControl.ascx.cs" Inherits="UserControls_RiskHtml_SubFilterControl" %>

<%-- Sub Item --%>
<%
    var subFilter = KeywordItems;
%>
<% 
    if (subFilter != null)
    {
        foreach (var item in subFilter)
        {%>
<div type="SubFilter" class="field" name="grp<%=item.Key%>" data-display-name="<%=item.Display%>" data-keyword="<%=item.Key%>"
    data-is-get-datasource="<%= item.IsGetDataSource %>"  data-value="<%= item.Value %>" data-parent-name="<%= item.ParentName %>">
    <div class="go-back">
        <a class="back-btn"><span class="icon-left-arrow"></span><%= GetGlobalResourceObject("AdvancedFilterResource","ButtonBack") %></a>
        <span class="suggestion-title"><%= item.Display %></span>
        <a class="pull-right close-btn"><%= GetGlobalResourceObject("AdvancedFilterResource","ButtonClose") %></a>
    </div>
    <div class="items items-single">
        <%if (item.KeywordItems != null)
          {
              foreach (var sub in item.KeywordItems)
              {%>
        <div value="<%= HttpUtility.HtmlEncode(sub.Key)%>" keyword="<%= HttpUtility.HtmlEncode(sub.Key)%>" data-display-value="<%= HttpUtility.HtmlEncode(sub.Display) %>"
           data-is-default ="<%= sub.IsDefaultFilter %>" data-default-display-value ="<%= sub.DefaultDisplayValue %>" data-default-value ="<%= sub.DefaultValue %>" data-input-type="<%= sub.InputType %>">
            <label><%=  HttpUtility.HtmlEncode(sub.Display)%></label>
        </div>
        <% }
                                                      }%>
    </div>
</div>
<%}
                                                }%>
