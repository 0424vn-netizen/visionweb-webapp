<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UxAdvancedFilter.ascx.cs" Inherits="UserControls_UxAdvancedFilter" %>

<%@ Register TagName="SavedFilter" Src="~/UserControls/RiskHtml/SaveFilterList.ascx" TagPrefix="uc" %>
<%@ Register TagName="RecentFilter" Src="~/UserControls/RiskHtml/RecentFilterList.ascx" TagPrefix="uc" %>
<%@ Register TagName="TextboxControl" Src="~/UserControls/RiskHtml/TextboxControl.ascx" TagPrefix="uc" %>
<%@ Register TagName="SingleSelectionControl" Src="~/UserControls/RiskHtml/SingleSelectionControl.ascx" TagPrefix="uc" %>
<%@ Register TagName="MultiChooseControl" Src="~/UserControls/RiskHtml/MultiChooseControl.ascx" TagPrefix="uc" %>
<%@ Register TagName="MultiSelectionControl" Src="~/UserControls/RiskHtml/MultiSelectionControl.ascx" TagPrefix="uc" %>
<%@ Register TagName="RangeRadioControl" Src="~/UserControls/RiskHtml/RangeRadioControl.ascx" TagPrefix="uc" %>
<%@ Register TagName="SubFilterControl" Src="~/UserControls/RiskHtml/SubFilterControl.ascx" TagPrefix="uc" %>
<%@ Register TagName="OrderByControl" Src="~/UserControls/RiskHtml/OrderByControl.ascx" TagPrefix="uc" %>
<%@ Register TagName="DatePickerControl" Src="~/UserControls/RiskHtml/DatePickerControl.ascx" TagPrefix="uc" %>
<%@ Register TagName="AutoCompleteControl" Src="~/UserControls/RiskHtml/AutoCompleteControl.ascx" TagPrefix="uc" %>

<%
    string sectionName = FilterPage.ToString();
%>

<%--Search Content--%>
<div id="advFilter<%=sectionName%>" class="adv-filter-container" data-adv-filter="true">
    <div class="content-searchcontrol" data-selector="content-select">
        <!--Search control -------------------------------------------------------------------------------->
        <div id="idSearchControl">
            <h2 class="section-heading"><%= GetGlobalResourceObject("AdvancedFilterResource","TitleAdvancedFilter") %></h2>
            <div class="section" id="idInputFilter">
                <!-- Input area ------------------------------------------------------------------------->
                <div class="as-global-search-container">
                    <div class="search-container">
                        <div class="search-input-container">
                            <div class="pos-relative search-wrapper">
                                <div class="aperia-search" distinct="true" id="advInput<%=sectionName%>" name="advInput<%=sectionName%>">
                                    <div class="filter-name-top hide" id="filter-name-top">
                                        <div class="filter-item">
                                            <%= GetGlobalResourceObject("AdvancedFilterResource","LblFilterName") %>
                                            <span data-name></span>
                                        </div>
                                        <div class="dropdown">
                                            <div class="action-more dropdown-toggle" data-toggle="dropdown">
                                                <div class="item-discussion"></div>
                                                <div class="item-discussion"></div>
                                                <div class="item-discussion"></div>
                                            </div>
                                            <ul class="dropdown-menu dropdown-menu-right">
                                                <li><a data-none-close-toggle="true" data-rename-ontop="true"><%= GetGlobalResourceObject("AdvancedFilterResource","ActionRename") %></a></li>
                                                <li><a data-delete-ontop="true"><%= GetGlobalResourceObject("AdvancedFilterResource","ActionDelete") %></a></li>
                                            </ul>
                                            <div class="rename-filter-container hide" data-rename-form="true">
                                                <div class="flex">
                                                    <input data-none-close-toggle="true" class="form-control" maxlength="250" name="rename_input" type="text" value="">
                                                    <a data-none-close-toggle="true" href="#" class="action-link action-item" data-save-rename-filter-ontop="true"><%= GetGlobalResourceObject("AdvancedFilterResource","ButtonSave") %></a>
                                                    <a href="#" class="action-link action-item" data-cancel-rename-filter-ontop="true"><%= GetGlobalResourceObject("AdvancedFilterResource","ButtonCancel") %></a>
                                                </div>
                                                <p class="error hide"></p>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="display hide collapseAF" id="collapseAF">
                                        <div class="text"><span class="mark-text"><%= GetGlobalResourceObject("AdvancedFilterResource","PlaceHolderExpandFilter") %></span><span class="af-info float-right" id="afInfo"></span></div>
                                    </div>
                                    <div type="display" class="display collapse in" data-current-filteid="0" id="afDisplayContainer">
                                        <div id="displayList" class="display-list hide"></div>
                                        <span class="text" id="txt-filter-item">
                                            <input type="text" autocomplete="off" class="fake" value="" placeholder="<%= GetGlobalResourceObject("AdvancedFilterResource","PlaceHolderSelectFilter") %>">
                                            <input type="text" autocomplete="off" class="input">
                                            <div class="display-multiple-selection" style="display: none">
                                                <span display-text=""></span><a data-type="add-text" display-value></a>
                                            </div>
                                        </span>
                                        <span class="no-text hide" id="no-filter-item"><span class="mark-text"><%= GetGlobalResourceObject("AdvancedFilterResource","PlaceHolderNoAvailableFilter") %></span></span>
                                    </div>
                                    <div class="container-field">
                                        <div type="field" class="field item-field" data-selector="suggested-select">
                                            <span class="title"><%= GetGlobalResourceObject("AdvancedFilterResource","TitleSuggestedFilters") %></span>
                                            <div class="items items-suggest">
                                                <% if (AdvancedFilterConfig.KeywordItems != null)
                                                   { %>
                                                <%
                                                       foreach (KeywordItem item in AdvancedFilterConfig.KeywordItems)
                                                       {
                                                           if (item.IsHidden)
                                                           {
                                                               continue;
                                                           }
                                                           string valueFlag = string.Empty;
                                                           switch (item.InputType)
                                                           {
                                                               case InputTypeEnums.MultiSelection:
                                                                   valueFlag = string.Format("values=grp{0}", item.Key);
                                                                   break;
                                                               case InputTypeEnums.RangeRadio:
                                                                   valueFlag = string.Format("values=grp{0}", item.Key);
                                                                   break;
                                                               case InputTypeEnums.SingleSelection:
                                                                   valueFlag = string.Format("values=grp{0}", item.Key);
                                                                   break;
                                                               case InputTypeEnums.Text:
                                                                   valueFlag = string.Format("values=grp{0}", item.Key);
                                                                   break;
                                                               case InputTypeEnums.MultiChoose:
                                                                   valueFlag = string.Format("values=grp{0}", item.Key);
                                                                   break;
                                                               case InputTypeEnums.DatePicker:
                                                                   valueFlag = string.Format("values=grp{0}", item.Key);
                                                                   break;
                                                               case InputTypeEnums.SubFilter:
                                                                   valueFlag = string.Format("values=grp{0}", item.Key);
                                                                   break;
                                                               case InputTypeEnums.OrderBy:
                                                                   valueFlag = string.Format("values=grp{0}", item.Key);
                                                                   break;
                                                               case InputTypeEnums.AutoComplete:
                                                                   valueFlag = string.Format("values=grp{0}", item.Key);
                                                                   break;
                                                               default:
                                                                   break;
                                                           } %>
                                                <div keyword="<%= item.Key %>"
                                                    data-display-value="<%=HttpUtility.HtmlEncode(item.Display) %>"
                                                    valfunc="<%=item.ClientValidationFunction %>"
                                                    data-is-multi-value="<%= item.IsMultiValue %>"
                                                    data-is-parent="<%= item.IsParent %>"
                                                    <%= valueFlag %>>
                                                    <label><%= item.Display %></label>
                                                </div>
                                                <%
                                                           if (item.IsParent && item.KeywordItems.Count > 0)
                                                           {
                                                               foreach (KeywordItem childItem in item.KeywordItems)
                                                               {
                                                                   if (childItem.IsHidden)
                                                                   {
                                                                       continue;
                                                                   }
                                                                   valueFlag = string.Empty;
                                                                   switch (childItem.InputType)
                                                                   {
                                                                       case InputTypeEnums.MultiSelection:
                                                                           valueFlag = string.Format("values=grp{0}", childItem.Key);
                                                                           break;
                                                                       case InputTypeEnums.RangeRadio:
                                                                           valueFlag = string.Format("values=grp{0}", childItem.Key);
                                                                           break;
                                                                       case InputTypeEnums.SingleSelection:
                                                                           valueFlag = string.Format("values=grp{0}", childItem.Key);
                                                                           break;
                                                                       case InputTypeEnums.Text:
                                                                           valueFlag = string.Format("values=grp{0}", childItem.Key);
                                                                           break;
                                                                       case InputTypeEnums.MultiChoose:
                                                                           valueFlag = string.Format("values=grp{0}", childItem.Key);
                                                                           break;
                                                                       case InputTypeEnums.DatePicker:
                                                                           valueFlag = string.Format("values=grp{0}", childItem.Key);
                                                                           break;
                                                                       case InputTypeEnums.OrderBy:
                                                                           valueFlag = string.Format("values=grp{0}", childItem.Key);
                                                                           break;
                                                                       case InputTypeEnums.AutoComplete:
                                                                           valueFlag = string.Format("values=grp{0}", childItem.Key);
                                                                           break;
                                                                       default:
                                                                           break;
                                                                   }%>
                                                <div keyword="<%= childItem.Key %>" style="display: none;" class="display"
                                                    data-display-value="<%=HttpUtility.HtmlEncode(childItem.Display)%>"
                                                    valfunc="<%= childItem.ClientValidationFunction %>"
                                                    data-is-multi-value="<%= childItem.IsMultiValue %>"
                                                    data-parent-key="<%= item.Key %>"
                                                    data-is-parent="<%= childItem.IsParent %>"
                                                    is-required-when-date-change="<%= childItem.IsRequiredWhenChangeDate%>"
                                                    <%= valueFlag %>>
                                                    <label><%= childItem.Display %></label>
                                                </div>
                                                <% } %>
                                                <% } %>
                                                <% } %>
                                                <% } %>
                                            </div>
                                        </div>
                                        <div type="validation" class="hide">
                                            <% if (AdvancedFilterConfig.KeywordItems != null)
                                               {
                                                   foreach (KeywordItem item in AdvancedFilterConfig.KeywordItems)
                                                   {%>
                                            <div class="validation-item" data-validation-key="<%= item.Key %>">
                                                <% foreach (AdvancedValidationRule validate in item.ValidationRules)
                                                   { %>
                                                <div data-control-id-to-validate="<%= validate.ControlToValidateID %>" data-object-control-id-to-validate="<%= validate.ObjectControlToValidateID %>"
                                                    data-rule="<%= validate.Rule %>">
                                                    <%= validate.Message %>
                                                </div>
                                                <% } %>
                                            </div>
                                            <% if (item.IsParent)
                                                   foreach (KeywordItem sub in item.KeywordItems)
                                                   {%>
                                            <div class="validation-item" data-validation-key="<%= sub.Key %>">
                                                <% foreach (AdvancedValidationRule validate in sub.ValidationRules)
                                                   { %>
                                                <div data-control-id-to-validate="<%= validate.ControlToValidateID %>" data-object-control-id-to-validate="<%= validate.ObjectControlToValidateID %>"
                                                    data-rule="<%= validate.Rule %>">
                                                    <%= validate.Message %>
                                                </div>
                                                <% } %>
                                            </div>
                                            <%} %>
                                            <%}
                                               }
                                            %>
                                        </div>
                                        <div type="values" class="values item-field">
                                            <uc:MultiChooseControl ID="uxMultiChooseControl" runat="server" />
                                            <uc:MultiSelectionControl ID="uxMultiSelectionControl" runat="server" />
                                            <uc:RangeRadioControl ID="uxRangeRadioControl" runat="server" />
                                            <uc:SingleSelectionControl ID="uxSingleSelection" runat="server" />
                                            <uc:TextboxControl ID="uxTextControl" runat="server" />
                                            <uc:SubFilterControl ID="uxSubFilterControl" runat="server" />
                                            <uc:OrderByControl ID="uxOrderByControl" runat="server" />
                                            <uc:DatePickerControl ID="uxDatePickerControl" runat="server" />
                                            <uc:AutoCompleteControl ID="uxAutocompleteControl" runat="server" />
                                        </div>
                                    </div>


                                    <div type="buttons" class="close-btn">
                                        <span data-type="delete" class="fa fa-times-circle"></span>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <!-- End Input area ------------------------------------------------------------------------->
            </div>
        </div>
        <!--End Search control -------------------------------------------------------------------------------->

        <div class="msg-date-available hide" id="msg-date-available"></div>

        <%-- Save Filter form -------------------------------------------------------------------------%>
        <div class="section">
            <div class="action-filter-container">
                <div id="saveFilter_<%= sectionName %>" class="action-save-filter">
                    <a href="javascript:void(0)" data-none-close-toggle="true" class="action-link hide js-save-link" onclick="advancedFilter.currentFilter.toggleSaveForm()"><%= GetGlobalResourceObject("AdvancedFilterResource","LinkSaveFilter") %></a>
                    <a href="javascript:void(0)" data-none-close-toggle="true" class="action-link hide js-saveas-link" onclick="advancedFilter.currentFilter.toggleSaveForm()"><%= GetGlobalResourceObject("AdvancedFilterResource","LinkSaveAsFilter") %></a>
                    <div id="saveFilterLayout_<%= sectionName %>" class="save-filter-container hide" data-rename-form="true">
                        <div class="flex">
                            <input data-none-close-toggle="true" maxlength="250" type="text" id="saveFilterNameInput" autocomplete="off" class="input form-control action-input">
                            <a data-none-close-toggle="true" href="javascript:void(0)" class="action-link action-item" onclick="advancedFilter.currentFilter.saveFilter()"><%= GetGlobalResourceObject("AdvancedFilterResource","ButtonSave") %></a>
                            <a href="javascript:void(0)" class="action-link action-item" onclick="advancedFilter.currentFilter.toggleSaveForm()"><%= GetGlobalResourceObject("AdvancedFilterResource","ButtonCancel") %></a>
                        </div>
                        <p class="error hide" id="error-save-filter"></p>
                    </div>
                </div>
                <a href="javascript:void(0)" class="action-link hide js-update-link" onclick="advancedFilter.currentFilter.updateFilter()"><%= GetGlobalResourceObject("AdvancedFilterResource","LinkUpdateFilter") %></a>
                <a href="javascript:void(0)" class="action-link hide js-canceledit-link" onclick="advancedFilter.currentFilter.cancelEdit()"><%= GetGlobalResourceObject("AdvancedFilterResource","ButtonCancelEdit") %></a>
                <a href="javascript:void(0)" class="action-link hide js-clear-link" onclick="advancedFilter.currentFilter.clearFilter()"><%= GetGlobalResourceObject("AdvancedFilterResource","LinkClearAll") %></a>
            </div>
        </div>
        <%--End Save Filter form ---------------------------------------------------------------------%>

        <div class="adv-filter-content" data-selector="filter-content" id="tt">
            <%--Saved Filters -----------------------------------------------------------------------%>
            <div class="section-header">
                <h3 class="section-heading"><%= GetGlobalResourceObject("AdvancedFilterResource","TitleSavedFilter") %></h3>
                <a data-toggle="collapse" href="#savedFilters_<%= sectionName%>" class="collapsed">
                    <span class="exp-text"><%= GetGlobalResourceObject("AdvancedFilterResource","TitleExpand") %></span>
                    <span class="col-text"><%= GetGlobalResourceObject("AdvancedFilterResource","TitleCollapse") %></span>
                </a>
            </div>
            <div id="savedFilters_<%= sectionName %>" class="collapse-content collapse" data-selector="save-filter">
                <div class="list-filter-items items-saved-filter" id="list-saved-filter-items">
                    <uc:SavedFilter ID="uxSavedFilter" runat="server" />
                </div>
                <i class="help-text hide js-empty-msg"><%= GetGlobalResourceObject("AdvancedFilterResource","TitleNoItemFilter") %></i>
            </div>
            <%--End Saved Filters ---------------------------------------------------------------------%>

            <%--Recent Filters ------------------------------------------------------------------------%>
            <div class="section-header">
                <h3 class="section-heading"><%= GetGlobalResourceObject("AdvancedFilterResource","TitleRecentFilter") %></h3>
                <a data-toggle="collapse" href="#recentFilters_<%= sectionName%>" class="collapsed">
                    <span class="exp-text"><%= GetGlobalResourceObject("AdvancedFilterResource","TitleExpand") %></span>
                    <span class="col-text"><%= GetGlobalResourceObject("AdvancedFilterResource","TitleCollapse") %></span>
                </a>
            </div>
            <div id="recentFilters_<%= sectionName%>" class="collapse-content collapse" data-selector="recent-filter">
                <div class="list-filter-items" id="list-recent-items">
                    <uc:RecentFilter ID="uxRecentFilter" runat="server" />
                </div>
                <i class="help-text hide js-empty-msg"><%= GetGlobalResourceObject("AdvancedFilterResource","TitleNoItemFilter") %></i>
            </div>
            <%--End Recent Filters ---------------------------------------------------------------------%>
        </div>
    </div>

    <div class="action-container">
        <input type="button" id='<%= "btnApplyAdvFilter"%>' class="btn btn-primary" data-is-generate="<%= IsGenerate %>" value="<%= ApplyButtonText %>">
        <input type="button" id='<%= "btnCloseAdvFilter" %>' class="btn btn-default" value="<%= GetGlobalResourceObject("AdvancedFilterResource","btnClose") %>">
    </div>
</div>

<tek:RadCodeBlock ID="radCodeBlock" runat="server">
    <script type="text/javascript">
        var msg_FilterName_Msg25CharacterLimit = '<%= GetGlobalResourceObject("AdvancedFilterResource", "Msg25CharacterLimit")%>';
        var msg_FilterName_MsgCharacterLimit = '<%= GetGlobalResourceObject("AdvancedFilterResource", "MsgCharacterLimit")%>';
        var msg_FilterName_Require = '<%= GetGlobalResourceObject("AdvancedFilterResource", "Msg_FilterName_Require")%>';
        var msg_FilterName_SpecialCharacter = '<%= GetGlobalResourceObject("AdvancedFilterResource", "Msg_FilterName_SpecialCharacter")%>';
        var msg_FilterName_Unique = '<%= GetGlobalResourceObject("AdvancedFilterResource", "Msg_FilterName_Unique")%>'
        var msg_FilterName_Only10Item = '<%= GetGlobalResourceObject("AdvancedFilterResource", "Msg_FilterName_Only10Item")%>'
        var msg_LblFilterApplied = '<%= GetGlobalResourceObject("AdvancedFilterResource", "LblFilterApplied")%>'
        var msg_LblFiltersApplied = '<%= GetGlobalResourceObject("AdvancedFilterResource", "LblFiltersApplied")%>'
        var msg_DateAvailableFilter = '<%= GetGlobalResourceObject("AdvancedFilterResource", "MsgDateAvailableFilter")%>'
        var msg_LblBetween = '<%= GetGlobalResourceObject("AdvancedFilterResource", "LblBetween")%>'
        var msg_LblGreaterThan = '<%= GetGlobalResourceObject("AdvancedFilterResource", "LblGreaterThan")%>'
        var msg_LblLessThan = '<%= GetGlobalResourceObject("AdvancedFilterResource", "LblLessThan")%>'
        var msg_UxAdvancedFilter_LinkDeselectAll = '<%= GetGlobalResourceObject("AdvancedFilterResource", "LinkDeselectAll")%>'
        var msg_UxAdvancedFilter_LinkSelectAll = '<%= GetGlobalResourceObject("AdvancedFilterResource", "LinkSelectedAll")%>'
        var msg_UxAdvancedFilter_LblFilterOneItem = '<%= GetGlobalResourceObject("AdvancedFilterResource", "LblFilterOneItem")%>'
        var msg_UxAdvancedFilter_LblFilterMoreItem = '<%= GetGlobalResourceObject("AdvancedFilterResource", "LblFilterMoreItem")%>'
        var msg_UxAdvancedFilter_PlaceHolderSelectFilter = '<%= GetGlobalResourceObject("AdvancedFilterResource","PlaceHolderSelectFilter") %>';
        var lbDaily_DateFilter = '<%= GetGlobalResourceObject("AdvancedFilterResource","Daily")%>';
        var lbDateRange_DateFilter = '<%= GetGlobalResourceObject("AdvancedFilterResource","DateRangeDisplay")%>';
        var msg_UxAdvancedFilter_LblAscending = '<%= GetGlobalResourceObject("AdvancedFilterResource","LblAscending") %>'
        var msg_UxAdvancedFilter_LblDescending = '<%= GetGlobalResourceObject("AdvancedFilterResource","LblDescending") %>'
        var msg_UxAdvancedFilter_NoResults = '<%= GetGlobalResourceObject("AdvancedFilterResource","NoResults") %>'
        var text_UxAdvancedFilter_lbSelectMerchant = '<%= GetGlobalResourceObject("AdvancedFilterResource","SelectMerchantText")%>';
        var uxAdvancedFilter_DateAvaiableFilter = '<%= RiskSessionManager.DateAvailableFilter.Value.ToString("MM/dd/yyyy") %>';
        var uxAdvancedFilter_LblAddSubFilters = '<%= GetGlobalResourceObject("AdvancedFilterResource","LblAddSubFilters") %>';
        var uxAdvancedFilter_LimitGenerateStatisticsReport = '<%= LimitGenerateStatisticsReport %>';
    </script>
</tek:RadCodeBlock>
