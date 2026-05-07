<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MultiChooseControl.ascx.cs" Inherits="UserControls_RiskHtml_MultiChooseControl" %>

<%--Multi Choose--%>
<%
    var multiChoose = KeywordItems;
%>
<% 
    if (multiChoose != null)
    {
        foreach (var mul in multiChoose)
        {%>
<div type="MultiChoose" class="field" name="grp<%=mul.Key%>" data-display-name="<%=mul.Display%>" data-keyword="<%=mul.Key%>" is-required-when-date-change="<%= mul.IsRequiredWhenChangeDate%>"
    data-is-get-datasource="<%= mul.IsGetDataSource %>" data-parent-key="<%= mul.ParentKey %>">
    <div class="go-back">
        <a class="back-btn"><span class="icon-left-arrow"></span><%= GetGlobalResourceObject("AdvancedFilterResource","ButtonBack") %></a>
        <span class="suggestion-title"><%= mul.Display %></span>
        <a class="pull-right close-btn"><%= GetGlobalResourceObject("AdvancedFilterResource","ButtonClose") %></a>
    </div>
    <div class="items adv-multi-choose">
        <label class='title-multi-choose'><%= mul.TitleMultiChoose %></label>
        <div class="data-multi-item">
            <input class="multiSelect" />
        </div>
    </div>
</div>
<%}
}%>
