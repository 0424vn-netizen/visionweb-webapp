<%@ Control Language="C#" AutoEventWireup="true" CodeFile="TextboxControl.ascx.cs" Inherits="UserControls_RiskHtml_TextboxControl" %>

<%--Textbox--%>
<%
    var textList = KeywordItems;
%>
<% 
    if (textList != null)
    {
        foreach (var text in textList)
        {%>
<div type="Text" class="field" name="grp<%=text.Key%>" data-display-name="<%=text.Display%>" data-keyword="<%=text.Key%>" data-parent-key="<%= text.ParentKey %>">
    <div class="go-back">
        <a class="back-btn"><span class="icon-left-arrow"></span><%= GetGlobalResourceObject("AdvancedFilterResource","ButtonBack") %></a>
        <span class="suggestion-title"><%= text.Display %></span>
        <a class="pull-right done-btn"><%= GetGlobalResourceObject("AdvancedFilterResource","ButtonDone") %></a>
    </div>
    <div class="items">
        <div value="text" class="mt-2x mb-2x" keyword="<%= HttpUtility.HtmlEncode(text.Key)%>" data-display-value="<%= HttpUtility.HtmlEncode(text.Display) %>">
            <input type="text" class="form-control-adv" maxlength="9" name="<%= text.Key %>" onkeypress="advancedFilter.currentFilter.txtValue_KeyPress(event)"
                keyword="<%= text.Key %>" data-display-value="<%= HttpUtility.HtmlEncode(text.Display) %>" />
            <div data-validation-msg="true" class="error hide ptb-10x"></div>
        </div>
    </div>
</div>
<%}
    }%>
