<%@ Control Language="C#" AutoEventWireup="true" CodeFile="DatePickerControl.ascx.cs" Inherits="UserControls_RiskHtml_DatePickerControl" %>

<%--Date Time--%>
<%
    var dateTime = KeywordItems;
%>
<% 
    if (dateTime != null)
    {
        foreach (var item in dateTime)
        {%>
<div type="DatePicker" class="field" name="grp<%=item.Key%>" data-display-name="<%=item.Display%>" 
    data-keyword="<%=item.Key%>" data-parent-key="<%= item.ParentKey %>" >
    <div class="go-back">
        <a class="back-btn"><span class="icon-left-arrow"></span><%= GetGlobalResourceObject("AdvancedFilterResource","ButtonBack") %></a>
        <span class="suggestion-title"><%= item.Display %></span>
        <a class="pull-right done-btn"><%= GetGlobalResourceObject("AdvancedFilterResource","ButtonDone") %></a>
    </div>
    <div class="items">
        <div value="text" class="date-container mt-2x" keyword="<%= HttpUtility.HtmlEncode(item.Key)%>" data-display-value="<%= HttpUtility.HtmlEncode(item.Display) %>">
            <div class="radio radio-item">
                <label>
                    <input type="radio" name="DateFilter" value="Daily" display-value="<%= GetGlobalResourceObject("AdvancedFilterResource","Daily") %>" />
                    <span><%= GetGlobalResourceObject("AdvancedFilterResource","Daily") %></span>
                </label>
                <label class="ml-4x">
                    <input type="radio" name="DateFilter" value="DateRange" display-value="<%= GetGlobalResourceObject("AdvancedFilterResource","DateRangeDisplay") %>" />
                    <span><%= GetGlobalResourceObject("AdvancedFilterResource","DateRange") %></span>
                </label>
            </div>
            <div class="row val-merchant-msg">
                <div class="col-md-12">
                    <div class="error-msg ptb-10x"><%= GetGlobalResourceObject("AdvancedFilterResource","ValidateMerchantDateChangeMsg") %></div>
                </div>
            </div>
            <div class="row date-item daily hide" data-daily-textbox="true">
                <div class="col-md-12">
                    <input type="text" class="form-control-adv date-picker-control" data-daily-date-value="true" name="Daily<%= item.Key %>" onkeypress="advancedFilter.currentFilter.txtValue_KeyPress(event)"
                        keyword="<%= item.Key %>" data-display-value="<%= HttpUtility.HtmlEncode(item.Display) %>" />
                    <div data-validation-msg="true" class="error hide ptb-10x"></div>
                </div>
            </div>
            <div class="row date-item date-range hide" data-range-textbox="true">
                <div class="col-md-12">
                    <label class="form-range-label" for="From<%= item.Key %>"><%= GetGlobalResourceObject("AdvancedFilterResource","LblFrom") %></label>
                    <input type="text" class="form-control-adv date-picker-control" data-from-date-value="true" name="From<%= item.Key %>" onkeypress="advancedFilter.currentFilter.txtValue_KeyPress(event)"
                        keyword="<%= item.Key %>" data-display-value="<%= HttpUtility.HtmlEncode(item.Display) %>" />
                    <label class="to-range-label to-date" for="To<%= item.Key %>"><%= GetGlobalResourceObject("AdvancedFilterResource","LblTo") %></label>
                    <input type="text" class="form-control-adv date-picker-control" data-to-date-value="true" name="To<%= item.Key %>" onkeypress="advancedFilter.currentFilter.txtValue_KeyPress(event)"
                        keyword="<%= item.Key %>" data-display-value="<%= HttpUtility.HtmlEncode(item.Display) %>" />
                    <div data-validation-msg="true" class="error hide ptb-10x"></div>
                </div>
            </div>
        </div>
    </div>
</div>
<%}
  }%>