<%@ Control Language="C#" AutoEventWireup="true" CodeFile="OrderByControl.ascx.cs" Inherits="UserControls_RiskHtml_OrderByControl" %>

<%--Order By--%>
<%
    var items = KeywordItems;
%>
<% 
    if (items != null)
    {
        foreach (var item in items)
        {%>
<div type="OrderBy" class="field" name="grp<%=item.Key%>" data-display-name="<%=item.Display%>" 
    data-keyword="<%=item.Key%>" data-is-get-datasource="<%= item.IsGetDataSource %>" data-parent-key="<%= item.ParentKey %>">
    <div class="go-back">
        <a class="back-btn"><span class="icon-left-arrow"></span><%= GetGlobalResourceObject("AdvancedFilterResource","ButtonBack") %></a>
        <span class="suggestion-title"><%= item.Display %></span>
        <a class="pull-right done-btn"><%= GetGlobalResourceObject("AdvancedFilterResource","ButtonDone") %></a>
    </div>
    <div class="items items-range-radio">
        <div value="text" class="mt-2x mb-2x" keyword="<%= HttpUtility.HtmlEncode(item.Key)%>" data-display-value="<%= HttpUtility.HtmlEncode(item.Display) %>">
            <div class="radio radio-item">
                <label>
                    <input type="radio" name="<%=(item.Key) %>" checked value="Asc" display-value="<%= GetGlobalResourceObject("AdvancedFilterResource","LblAscending") %>" />
                    <span><%= GetGlobalResourceObject("AdvancedFilterResource","LblAscending") %></span>
                </label>
            </div>
            <div class="radio radio-item">
                <label>
                    <input type="radio" name="<%=(item.Key) %>" value="Desc" display-value="<%= GetGlobalResourceObject("AdvancedFilterResource","LblDescending") %>" />
                    <span><%= GetGlobalResourceObject("AdvancedFilterResource","LblDescending") %></span>
                </label>
            </div>
            <div class="ui-widget row mt-10">
                <div class="col-xs-2 lbl-order-by"><label><%= GetGlobalResourceObject("AdvancedFilterResource","lblOrderBy") %></label></div>
                <div class="col-xs-10" data-combo="true"></div>
            </div>
            <div data-validation-msg="true" class="error hide ptb-10x"></div>
        </div>
    </div>
</div>
<%}
    }%>
