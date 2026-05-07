<%@ Control Language="C#" AutoEventWireup="true" CodeFile="RangeRadioControl.ascx.cs" Inherits="UserControls_RiskHtml_RangeRadioControl" %>
<%--Range Radio--%>
<%
    var rangeRadio = KeywordItems;
%>
<% 
    if (rangeRadio != null)
    {
        foreach (var range in rangeRadio)
        {%>
<div type="RangeRadio" class="field" name="grp<%=range.Key%>" data-display-name="<%=range.Display%>" data-keyword="<%=range.Key%>" data-parent-key="<%= range.ParentKey %>">
    <div class="go-back">
        <a class="back-btn"><span class="icon-left-arrow"></span><%= GetGlobalResourceObject("AdvancedFilterResource","ButtonBack") %></a>
        <span class="suggestion-title"><%= range.Display %></span>
        <a class="pull-right done-btn"><%= GetGlobalResourceObject("AdvancedFilterResource","ButtonDone") %></a>
    </div>
    <div class="items items-range-radio">
        <div value="rangeRadio" keyword="<%= HttpUtility.HtmlEncode(range.Key)%>" data-display-value="<%= HttpUtility.HtmlEncode(range.Display) %>">
            <div class="radio radio-item">
                <label>
                    <input type="radio" name="<%=(range.Key) %>" value="Between" display-value="<%= GetGlobalResourceObject("AdvancedFilterResource","LblBetween") %>" />
                    <span><%= GetGlobalResourceObject("AdvancedFilterResource","LblBetween") %></span>
                </label>
            </div>
            <div class="radio radio-item">
                <label>
                    <input type="radio" name="<%=(range.Key) %>" value="GreaterThan" display-value="<%= GetGlobalResourceObject("AdvancedFilterResource","LblGreaterThan") %>" />
                    <span><%= GetGlobalResourceObject("AdvancedFilterResource","LblGreaterThan") %></span>
                </label>
            </div>
            <div class="radio radio-item">
                <label>
                    <input type="radio" name="<%=(range.Key) %>" value="LessThan" display-value="<%= GetGlobalResourceObject("AdvancedFilterResource","LblLessThan") %>" />
                    <span><%= GetGlobalResourceObject("AdvancedFilterResource","LblLessThan") %></span>
                </label>
            </div>

            <div data-from-to-textbox="true" class="hide from-to-padding">
                <label class="form-range-label" for="From<%= range.Key %>"><%= GetGlobalResourceObject("AdvancedFilterResource","LblFrom") %></label>
                <input type="text" maxlength="9" class="form-range form-control-adv" name="From<%= range.Key %>" data-from-value="true" onkeypress="advancedFilter.currentFilter.txtValue_KeyPress(event)"
                    keyword="<%= range.Key %>" data-display-value="<%= HttpUtility.HtmlEncode(range.Display) %>" />
                <label class="to-range-label" for="To<%= range.Key %>"><%= GetGlobalResourceObject("AdvancedFilterResource","LblTo") %></label>
                <input type="text" maxlength="9" class="form-range form-control-adv" name="To<%= range.Key %>" data-to-value="true" onkeypress="advancedFilter.currentFilter.txtValue_KeyPress(event)"
                    keyword="<%= range.Key %>" data-display-value="<%= HttpUtility.HtmlEncode(range.Display) %>" />
            </div>
            <div data-textbox="true" class="hide from-to-padding" data-option="GreaterOrLess">
                <input type="text" class="form-control-adv" maxlength="9" name="Txt<%= range.Key %>" data-value="true" onkeypress="advancedFilter.currentFilter.txtValue_KeyPress(event)"
                    keyword="<%= range.Key %>" data-display-value="<%= HttpUtility.HtmlEncode(range.Display) %>" />
            </div>
            <div data-validation-msg="true" class="error pl-0x hide"></div>
        </div>
    </div>
</div>
<%}
                                                }%>