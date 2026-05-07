var advancedFilter = {
    url: {
        saveFilter: null,
        getSavedFilters: null,
        getFilterById: null,
        renameFilterById: null,
        getRecentFilters: null,
        deleteFilterById: null,
        saveAppliedFilter: null,
        updateFilter: null,
        advancedFilterName: null,
    },

    constants: {
        searchBarItem: 'searchbaritem_',
        specialValue: ':',
        savedFilterSection: 'SavedFilter',
        recentFilterSection: 'RecentFilter'
    },

    create: function (options) {
        // set info
        advancedFilter.currentFilter._sectionName = options.sectionName;
        advancedFilter.currentFilter._btnRefresh = options.btnRefresh;
        advancedFilter.currentFilter._hddApplyFilterId = options.hddApplyFilterId;
        advancedFilter.currentFilter._hddDateAvailableFilter = options.hddDateAvailableFilter;
        advancedFilter.currentFilter._totalRecords = options.totalRecords;

        // init advanced filter function
        advancedFilter.currentFilter.initAF();

        return advancedFilter.currentFilter;
    },

    currentFilter: {
        _sectionName: null,
        _ctrl: null,
        _currentFilterSelected: '',
        _config: null,
        hiddenApplyFilter: [],
        sectionElmID: null,
        _btnRefresh: null,
        _hddApplyFilterId: null,
        _hddDateAvailableFilter: null,
        _isShowSaveAsForm: false,
        _totalRecords: null,

        //------------------------------------------------------------Save Filter functions
        toggleSaveForm: function (isShow) {
            var saveForm = $('#saveFilterLayout_' + this._sectionName);
            // if isShow para is not pass to function
            if (isShow === undefined) {
                isShow = saveForm.hasClass('hide');
            }
            advancedFilter.currentFilter.showHideSection(saveForm, isShow);

            // handle event
            if (isShow) {
                saveForm.find('input').val('');
                saveForm.find('input').focus();

                // handle event press enter
                saveForm.find('input').off('keyup').on('keyup', function (e) {
                    if (e.keyCode == 13) {
                        advancedFilter.currentFilter.saveFilter();
                    }
                });
            }
        },

        saveFilter: function () {
            var valSaveFilterNameInput = $('#saveFilterNameInput').val().trim();
            var errorElm = $('#error-save-filter');
            if (!advancedFilter.currentFilter.checkDataInput(valSaveFilterNameInput, errorElm)) return false;
            var isValid = true;

            var data = {};
            data["page"] = advancedFilter.currentFilter._sectionName;
            data["nameFilter"] = valSaveFilterNameInput;
            data["filterContent"] = JSON.stringify(advancedFilter.currentFilter.getAFValue());
            var url = rootURL + "AdvancedFilterPage.aspx/SaveFilter";

            advancedFilter.postServer(url, data, function (res) {
                if (res.d.ErrorMessage == "0") {
                    advancedFilter.currentFilter.clearFilter();
                    advancedFilter.currentFilter.reloadSavedFilters();
                    window.FilterSearchId = res.d.Id;
                    window.FilterSearchSection = advancedFilter.constants.savedFilterSection;
                    advancedFilter.currentFilter.continueLoadSavedFilter();
                }
                else {
                    var msg = '';
                    if (res.d.ErrorMessage == "1") {
                        msg = msg_FilterName_Only10Item;
                    }
                    else if (res.d.ErrorMessage == "2") {
                        msg = msg_FilterName_Unique;
                    }

                    errorElm.removeClass('hide');
                    errorElm.html(msg);
                }
            });

            return false;
        },

        // ------------------------------------------------------ Rename Filter

        toggleRenameForm: function (id, isShow) {
            var renameForm = $('li[data-filter-id="' + id + '"] .rename-filter-container');
            // if isShow para is not pass to function
            if (isShow === undefined) {
                isShow = renameForm.hasClass('hide');
            }
            advancedFilter.currentFilter.showHideSection(renameForm, isShow);

            // handle event
            if (isShow) {
                var filterName = $('li[data-filter-id=' + id + ']')[0].getAttribute('data-filter-name');
                renameForm.find('input').val(filterName);
                renameForm.find('input').focus();

                renameForm.find('.error').addClass('hide');

                // handle event press enter
                renameForm.find('input').off('keyup').on('keyup', function (e) {
                    if (e.keyCode == 13) {
                        advancedFilter.currentFilter.renameFilterById(id);
                    }
                });
                advancedFilter.currentFilter.calculatePositionRenameForm(renameForm);
            }
        },

        toggleRenameFormTop: function (isShow) {
            var id = window.FilterSearchId;
            var renameForm = $('#filter-name-top .rename-filter-container');
            // if isShow para is not pass to function
            if (isShow === undefined) {
                isShow = renameForm.hasClass('hide');
            }
            advancedFilter.currentFilter.showHideSection(renameForm, isShow);

            // handle event
            if (isShow) {
                var filterName = $('li[data-filter-id=' + id + ']')[0].getAttribute('data-filter-name');
                renameForm.find('input').val(filterName);
                renameForm.find('input').focus();

                // handle event press enter
                renameForm.find('input').off('keyup').on('keyup', function (e) {
                    if (e.keyCode == 13) {
                        advancedFilter.currentFilter.renameFilterById(id, true);
                    }
                });
            }
        },

        showFilterNameTop: function (id, section) {
            window.FilterSearchId = id;
            // Hide rename and delete icon when filter is recent
            if (section == advancedFilter.constants.recentFilterSection) {
                $('#filter-name-top .action-more')[0].style.display = 'none';
            }
            else {
                $('#filter-name-top .action-more')[0].style.display = 'block';
            }

            // Show filter name on top
            $('#filter-name-top').removeClass('hide');
            $('#filter-name-top [data-name]')[0].innerHTML = advancedFilter.currentFilter._currentFilterSelected.Name;
            $('#filter-name-top [data-name]')[0].setAttribute('title', advancedFilter.currentFilter._currentFilterSelected.Name);

            // Register event for Rename and Delete link
            $('#filter-name-top [data-rename-ontop]')[0].onclick = function (e) {
                advancedFilter.currentFilter.toggleRenameFormTop();
            };
            $('#filter-name-top [data-delete-ontop]')[0].onclick = function (e) {
                advancedFilter.currentFilter.showDeleteFilterModalOnTop();
            };

            // Register event for Save and Cancel button in Rename Form
            $('#filter-name-top [data-save-rename-filter-ontop]')[0].onclick = function (e) {
                var id = window.FilterSearchId;
                advancedFilter.currentFilter.renameFilterById(id, true);
            };
            $('#filter-name-top [data-cancel-rename-filter-ontop]')[0].onclick = function (e) {
                advancedFilter.currentFilter.toggleRenameFormTop(false);
            };
        },

        renameFilterById: function (id, isTop) {
            var container = isTop ? $('#filter-name-top .rename-filter-container') : $('li[data-filter-id="' + id + '"] .rename-filter-container');
            var renameInput = container.find('input');
            var valRenameInput = renameInput.val().trim();
            // Validation
            var errorElm = container.find('.error');
            if (!advancedFilter.currentFilter.checkDataInput(valRenameInput, errorElm)) return false;

            var data = {};
            data["id"] = id;
            data["name"] = valRenameInput;
            var url = rootURL + "AdvancedFilterPage.aspx/RenameFilterById";

            advancedFilter.postServer(url, data, function (res) {
                if (res.d == "0") {
                    // For list
                    advancedFilter.currentFilter.reloadSavedFilters(id);

                    // On Top
                    $('#filter-name-top span[data-name]')[0].innerHTML = valRenameInput;
                    $('#filter-name-top span[data-name]')[0].setAttribute('title', valRenameInput);
                    advancedFilter.currentFilter.toggleRenameFormTop(false);
                }
                else {
                    var msg = '';
                    if (res.d == "2") {
                        msg = msg_FilterName_Unique;
                    }

                    errorElm.removeClass('hide');
                    errorElm.html(msg);
                }
            })
        },

        //---------------------------------------------------------Delete Filter

        showDeleteFilterModal: function (id) {
            window.FilterSearchId = id
            parent.ShowPopupModal(rootURL + 'Risk_MCF/AdvancedFilter/DeleteFilterConfirmModal.aspx', 'auto');
        },

        showDeleteFilterModalOnTop: function () {
            advancedFilter.currentFilter.showDeleteFilterModal(window.FilterSearchId);
        },

        deleteFilterById: function () {
            parent.HidePopupModal();

            var data = {};
            data["id"] = window.FilterSearchId;
            var url = rootURL + "AdvancedFilterPage.aspx/DeleteFilterById";

            advancedFilter.postServer(url, data, function () {
                advancedFilter.currentFilter.clearFilter();
                advancedFilter.currentFilter.reloadSavedFilters();
            });
        },

        reloadSavedFilters: function () {
            var data = {};
            data["page"] = advancedFilter.currentFilter._sectionName;
            data["filterSection"] = advancedFilter.constants.savedFilterSection;
            var url = rootURL + "AdvancedFilterPage.aspx/GenerateSavedFilterList";
            advancedFilter.postServer(url, data, function (res) {
                $('#list-saved-filter-items')[0].innerHTML = res.d;
                preventDefaultInput();

            })
        },

        reloadRecentFilters: function () {
            var data = {};
            data["page"] = advancedFilter.currentFilter._sectionName;
            data["filterSection"] = advancedFilter.constants.recentFilterSection;
            var url = rootURL + "AdvancedFilterPage.aspx/GenerateSavedFilterList";

            advancedFilter.postServer(url, data, function (res) {
                $('#list-recent-items')[0].innerHTML = res.d;

            });
        },

        getFilterInfoById: function (id, section, callback) {
            if (id === undefined) {
                return '';
            }
            if (section === undefined) {
                section = advancedFilter.constants.savedFilterSection;
            }

            // Get detail filter
            var isRecent = section == advancedFilter.constants.recentFilterSection;
            var url = rootURL + "AdvancedFilterPage.aspx/GetInfoFilterItem";
            var data = {};
            data["page"] = advancedFilter.currentFilter._sectionName;
            data["id"] = window.FilterSearchId;
            data["isRecent"] = isRecent;

            advancedFilter.postServer(url, data, function (res) {
                advancedFilter.currentFilter._currentFilterSelected = res.d;
                advancedFilter.currentFilter._currentFilterSelected.IsRecent = isRecent;
                advancedFilter.currentFilter._config = new Object();
                advancedFilter.currentFilter._config.KeywordItems = res.d.FilterItems;
                //var name = !isRecent ? $('#list-saved-filter-items li[data-filter-id="' + id + '"]')[0].getAttribute('data-filter-name') :
                //    $('#list-recent-items li[data-filter-id="' + id + '"]')[0].getAttribute('data-filter-name');
                advancedFilter.currentFilter._currentFilterSelected.Name = res.d.FilterName;
                if (callback) callback();
            });
        },

        checkDataInput: function (valRenameInput, errorElm) {
            var isValid = true;

            if (valRenameInput.length < 1) {
                errorElm.html(msg_FilterName_Require);
                isValid = false;
            }
            else if (!aperia.searchControl.validate.NoSpecialCharacters(valRenameInput)) {
                errorElm.html(msg_FilterName_SpecialCharacter);
                isValid = false;
            } else if (valRenameInput.length > 25) {
                errorElm.html(msg_FilterName_Msg25CharacterLimit);
                isValid = false;
            } else if (!aperia.searchControl.validate.CharactersLimit(valRenameInput)) {
                errorElm.html(msg_FilterName_MsgCharacterLimit);
                isValid = false;
            }
            if (!isValid) {
                errorElm.removeClass('hide');
                return false;
            }
            return true;
        },
        //------------------------------------------------------------end save filter functions

        //------------------------------------------------------------control functions
        // add search item, type data is JSON object of FilterItem
        // using for only saved filters and recent filter
        addSearchItem: function (item) {
            // Add item to search control
            var configItem = this.getConfigFilterByKey(item.Key)
            var isParent = configItem && configItem.IsParent;

            var display = item.DisplayValue;
            var displayReal = item.DisplayValueReal;
            display = advancedFilter.currentFilter._ctrl.replaceSpecialChar(display);
            displayReal = advancedFilter.currentFilter._ctrl.replaceSpecialChar(displayReal);

            var displayName = isParent ? item.ParentName : item.DisplayName;

            var div = document.createElement('div');
            div.className = 'item parent-item';
            div.setAttribute('item-search-name', item.Key);
            div.setAttribute('data-value', item.Value);
            div.setAttribute('data-display-Name', displayName);
            div.setAttribute('data-display-value', display);
            div.setAttribute('data-display-value-real', displayReal);
            div.setAttribute('data-display-autocomplete', item.DisplayAutoComplete);
            div.setAttribute('data-is-parent', isParent ? 'true' : 'false');
            div.root = advancedFilter.currentFilter._ctrl;
            div.innerHTML = '<label class="display-name">' + displayName + ':</label>';

            switch (configItem.InputTypeValue) {
                case "RangeRadio":
                    var data = item.Value.split(div.root.constants.charDataBetween);
                    var lbl = data[0] == 'Between' ? msg_LblBetween : data[0] == 'GreaterThan' ? msg_LblGreaterThan : msg_LblLessThan;

                    var displayLabelType = document.createElement('label');
                    displayLabelType.className = 'type-value';
                    displayLabelType.setAttribute("type-value", "true")
                    displayLabelType.textContent = lbl;
                    div.appendChild(displayLabelType);

                    var displayFromValueLink = document.createElement('a');
                    displayFromValueLink.className = 'display-value between-value';
                    displayFromValueLink.setAttribute("first-value", "true")
                    displayFromValueLink.textContent = data[1];
                    div.appendChild(displayFromValueLink);
                    displayFromValueLink.onclick = function () {
                        //ctrl.editItem(div, this);
                        advancedFilter.currentFilter._ctrl.editItem(div, this);
                    };

                    if (data[0] == 'Between') {
                        var displayCharacter = document.createElement('label');
                        displayCharacter.setAttribute("character", "true")
                        displayCharacter.textContent = '-';
                        div.appendChild(displayCharacter);

                        var displayToValueLink = document.createElement('a');
                        displayToValueLink.className = 'display-value between-value';
                        displayToValueLink.setAttribute("first-value", "false")
                        displayToValueLink.textContent = data[2];
                        div.appendChild(displayToValueLink);
                        displayToValueLink.onclick = function () {
                            advancedFilter.currentFilter._ctrl.editItem(div, this);
                        };
                    }
                    break;
                case "OrderBy":
                    var data = item.Value.split(' ');
                    var dataDislay = item.DisplayValue;
                    var lbl = data[1] == 'Desc' ? msg_UxAdvancedFilter_LblDescending : msg_UxAdvancedFilter_LblAscending;

                    var displayLabelType = document.createElement('label');
                    displayLabelType.className = 'type-value';
                    displayLabelType.setAttribute("type-value", "true")
                    displayLabelType.textContent = lbl + " ";
                    div.appendChild(displayLabelType);

                    var displayValueLink = document.createElement('a');
                    displayValueLink.className = 'display-value between-value';
                    displayValueLink.textContent = dataDislay;
                    div.appendChild(displayValueLink);
                    displayValueLink.onclick = function () {
                        advancedFilter.currentFilter._ctrl.editItem(div, this);
                    };
                    break;
                default:
                    var displayValueLink = document.createElement('a');
                    displayValueLink.className = 'display-value';
                    displayValueLink.textContent = item.DisplayValue;
                    div.appendChild(displayValueLink);
                    displayValueLink.onclick = function () {
                        // For case sub filter
                        if (div.getAttribute('data-is-parent') == 'true') {
                            div.root.inputText.onfocus();
                            div.setAttribute('data-is-parent-open', "true");
                        }
                        else
                            advancedFilter.currentFilter._ctrl.editItem(div, this);
                    };
            }

            var iconClose = document.createElement('i');
            iconClose.className = 'icon-close';
            div.appendChild(iconClose);
            iconClose.onclick = function () {
                if (div.getAttribute('data-is-parent') == 'true') {
                    $('#txt-filter-item').removeClass("hide");
                    $('#displayList').removeClass("remove-border");
                    advancedFilter.currentFilter._ctrl.showAvaiableDateFilter(null);
                }
                advancedFilter.currentFilter._ctrl.removeItemNotKeepVal(div);
            };
            div.removeButton = iconClose;

            if (isParent) {
                advancedFilter.currentFilter._ctrl.subFilter(div);

                if (item.SubFilterItems && item.SubFilterItems.length > 0) {
                    div.className += ' has-sub';
                    $.each(item.SubFilterItems, function () {
                        div.childItemsWraper.root = div;
                        advancedFilter.currentFilter._ctrl.addChildSubItem(div.childItemsWraper, this)
                    });

                    div.collapseLink.style.display = '';
                    div.collapseLink.style.visibility = '';
                    div.addSubLink.style.display = 'none';
                    div.addSubLink.style.visibility = 'hidden';
                } else {
                    div.addSubLink.style.display = '';
                    div.addSubLink.style.visibility = '';
                }

                $('#txt-filter-item').addClass("hide");
                $('#displayList').addClass("remove-border");
            }

            $(this._ctrl.displayContainer).find('#displayList').append(div);

        },

        removeSeachItem: function (searchKey) {
            // validate search key before remove
            if (searchKey === undefined || searchKey == '') {
                return false;
            }
            var item = $(this._ctrl.displayContainer).find('div.item[item-search-name="' + searchKey + '"]')[0];
            $(item).remove();
        },

        removeAllSeachItem: function () {
            var items = $(advancedFilter.currentFilter._ctrl.displayContainer).find('div.item');
            $.each(items, function () {
                advancedFilter.currentFilter.removeSeachItem($(this).attr('item-search-name'));
            });
        },

        getAFValue: function () {
            var results = [], searchBoxVal = [];
            $.each($(advancedFilter.currentFilter._ctrl.displayContainer).find('div.item'), function () {
                var element = {}, childEle = [];
                element.Key = $(this).attr('item-search-name');
                element.Value = $(this).attr('data-value');
                element.DisplayName = $(this).attr('data-display-Name');
                element.DisplayValue = $(this).attr('data-display-value');

                var childElements = $(this).find('div.child-item');
                if (childElements && childElements.length > 0) {
                    $.each(childElements, function () {
                        var child = {};
                        child.Key = $(this).attr('item-search-name');
                        child.Value = $(this).attr('data-value');
                        child.DisplayName = $(this).attr('data-display-Name');
                        child.DisplayValue = $(this).attr('data-display-value');

                        childEle.push(child);
                    });

                    element.SubFilterItems = childEle;
                }

                searchBoxVal.push(element);
                results.push(element);
            });

            // add query string value
            if (advancedFilter.currentFilter && advancedFilter.currentFilter.hiddenApplyFilter) {
                $.each(advancedFilter.currentFilter.hiddenApplyFilter, function (i) {
                    var item = advancedFilter.currentFilter.hiddenApplyFilter[i];
                    var checkExist = advancedFilter.currentFilter.getValFilterItemByKey(searchBoxVal, item.Key);
                    if (checkExist == '' || checkExist.indexOf(item.Value) == -1) {
                        results.push(this);
                    }
                });
            }

            return results;
        },

        getConfigFilterByKey: function (key) {
            var result = '';
            $.each(advancedFilter.currentFilter._config.KeywordItems, function () {
                if (key == this.Key) {
                    result = this;
                    return;
                }
            });
            return result;
        },
        //------------------------------------------------------------end control functions

        //------------------------------------------------------------update filter functions
        cancelEdit: function () {
            advancedFilter.currentFilter.removeAllSeachItem();
            $.each(advancedFilter.currentFilter._currentFilterSelected.FilterItems, function () {
                advancedFilter.currentFilter.addSearchItem(this);
            });
            advancedFilter.currentFilter.resetAF();
        },

        updateFilter: function () {
            var data = {};
            data["page"] = advancedFilter.currentFilter._sectionName;
            data["id"] = window.FilterSearchId;
            data["filter"] = JSON.stringify(advancedFilter.currentFilter.getAFValue());;
            var url = rootURL + "AdvancedFilterPage.aspx/UpdateFilterById";

            advancedFilter.postServer(url, data, function () {
                advancedFilter.currentFilter.reloadSavedFilters();

                // Update info current Filter
                advancedFilter.currentFilter.getFilterInfoById(window.FilterSearchId, advancedFilter.currentFilter._sectionName);

                advancedFilter.currentFilter.resetAF(true, true);
            });
        },
        //------------------------------------------------------------end update filter functions

        refeshGrid: function (id) {
            $('#' + this._hddApplyFilterId).val(id);

            //// get advanced filters
            var afValue = this.getAFValue();

            var filterItem = $('[data-show-adv-filter-item="true"]');
            filterItem.removeClass('hide');

            var item = filterItem.find('[data-filter-item="true"]');
            item.html(afValue.length + ' ' + ((afValue.length > 1) ? msg_LblFiltersApplied : msg_LblFilterApplied));

            document.getElementById(this._btnRefresh).click();
        },

        applyFilter: function (isGenerate) {

            if (isGenerate == undefined) isGenerate = false;

            // log applied filter
            var appliedFilter = this.getAFValue(); if (appliedFilter.length == 0) return;

            var isChanged = !this.compareFilter(appliedFilter, advancedFilter.currentFilter._currentFilterSelected.FilterItems);
            var appliedFilterId = window.FilterSearchId;
            var sectionFilter = window.FilterSearchSection == undefined ? advancedFilter.constants.savedFilterSection : window.FilterSearchSection;

            if (isChanged)
                appliedFilterId = '0';

            if (isGenerate && parseInt($('#' + advancedFilter.currentFilter._totalRecords).val()) >= parseInt(uxAdvancedFilter_LimitGenerateStatisticsReport)) {
                parent.ShowPopupModal(rootURL + 'Risk_MCF/AdvancedFilter/ConfirmLimitItemModal.aspx', 'auto');
                return;
            }

            var data = {};
            data["page"] = advancedFilter.currentFilter._sectionName;
            data["id"] = appliedFilterId;
            data["filterContent"] = JSON.stringify(appliedFilter);
            data["filterSection"] = sectionFilter;
            data["isGenerate"] = isGenerate;
            var url = rootURL + "AdvancedFilterPage.aspx/SaveAppliedFilter";

            advancedFilter.postServer(url, data, function (res) {
                if (res.d.ErrorMessage == "4") {
                    parent.ShowPopupModal(rootURL + 'Risk_MCF/AdvancedFilter/ConfirmNodataModal.aspx', 'auto');
                }
                else {
                    core.ui.closeSidebar('advFilter' + advancedFilter.currentFilter._sectionName);
                    advancedFilter.currentFilter.reloadRecentFilters();
                    advancedFilter.currentFilter.refeshGrid(res.d.Id);
                    if (isGenerate) {
                        advancedFilter.currentFilter.clearFilter();
                    }
                }
            });
        },

        clearFilter: function () {
            this.removeAllSeachItem();
            this.toggleSaveForm(false);
            if (!$('#collapseAF').hasClass('hide')) {
                $('#collapseAF').addClass('hide');
                $('#afDisplayContainer').collapse('show');
            }
            advancedFilter.currentFilter._currentFilterSelected = '';
            this.resetAF();
            $('#filter-name-top').addClass('hide');
            advancedFilter.currentFilter._ctrl.showAvaiableDateFilter(null);
        },

        compareFilter: function (filter1, filter2) {
            if (filter1 === undefined || filter2 === undefined || filter1.length != filter2.length) {
                return false;
            }
            var result = true;
            $.each(filter1, function (i) {
                if (this.Key != filter2[i].Key || this.Value != filter2[i].Value) {
                    result = false;
                    return;
                }
                else {
                    //compare subItem
                    if ((!this.SubFilterItems && filter2[i].SubFilterItems && filter2[i].SubFilterItems.length > 0)
                        || (this.SubFilterItems && filter2[i].SubFilterItems && this.SubFilterItems.length != filter2[i].SubFilterItems.length)) {
                        result = false;
                    }
                    else {
                        if (this.SubFilterItems && this.SubFilterItems.length > 0) {
                            $.each(this.SubFilterItems, function (idx) {
                                if (this.Key != filter2[i].SubFilterItems[idx].Key || this.Value != filter2[i].SubFilterItems[idx].Value) {
                                    result = false;
                                }
                            });
                        }
                    }
                }
            });
            return result;
        },

        getValFilterItemByKey: function (filter, key) {
            var values = [];
            if (filter === undefined) {
                return values;
            }
            $.each(filter, function (i) {
                if (this.Key.toLowerCase() == key.toLowerCase()) {
                    values.push(this.Value);
                }
            });
            return values;
        },

        selectFilter: function (id, section) {
            advancedFilter.currentFilter.removeAllSeachItem();
            // get on local
            this.getFilterInfoById(id, section, function () {
                $.each(advancedFilter.currentFilter._currentFilterSelected.FilterItems, function () {
                    advancedFilter.currentFilter.addSearchItem(this);
                });

                advancedFilter.currentFilter.showFilterNameTop(id, section);
                aperia.searchControl.current.updateHeightFilterItem();
                advancedFilter.currentFilter.resetAF();
            });
        },

        showLoadSavedFilterModal: function () {
            parent.ShowPopupModal(rootURL + 'Risk_MCF/AdvancedFilter/LoadSavedFilterConfirmModal.aspx', 'auto')
        },

        continueLoadSavedFilter: function () {
            parent.HidePopupModal();
            advancedFilter.currentFilter.selectFilter(window.FilterSearchId, window.FilterSearchSection);
            $('#collapseAF').click();
        },

        clearApplyAdvFilter: function () {
            $('[data-show-adv-filter-item="true"]').addClass('hide');
            $('#' + this._hddApplyFilterId).val('');
            document.getElementById(this._btnRefresh).click();
            advancedFilter.currentFilter.clearFilter()
        },

        resetAF: function (forceFocus, isUpdated) {
            var dataFilter = advancedFilter.currentFilter.getAFValue();
            var currentFilteInfo = advancedFilter.currentFilter._currentFilterSelected;
            var isChanged = isUpdated ? false : !this.compareFilter(dataFilter, currentFilteInfo.FilterItems);

            // check show save filter link
            var isShowSaveLink = dataFilter != null && dataFilter != '' && (currentFilteInfo == '' || currentFilteInfo.IsRecent)
                && advancedFilter.currentFilter._ctrl.checkHasValueMerchant(advancedFilter.currentFilter._ctrl);
            advancedFilter.currentFilter.showHideSection($('#' + this.sectionElmID + ' .js-save-link'), isShowSaveLink);

            // check show clear filter link
            var isShowClearLink = dataFilter != null && dataFilter != '';
            advancedFilter.currentFilter.showHideSection($('#' + this.sectionElmID + ' .js-clear-link'), isShowClearLink);

            // check show update filter link
            var isShowUpdateLink = dataFilter != null && dataFilter != '' && currentFilteInfo && (!currentFilteInfo.IsRecent || currentFilteInfo.IsRecent == undefined) && isChanged
                && advancedFilter.currentFilter._ctrl.checkHasValueMerchant(advancedFilter.currentFilter._ctrl);;
            advancedFilter.currentFilter.showHideSection($('#' + this.sectionElmID + ' .js-update-link'), isShowUpdateLink);

            // check show cancel edit filter link
            var isShowCancelLink = isShowUpdateLink;
            advancedFilter.currentFilter.showHideSection($('#' + this.sectionElmID + ' .js-canceledit-link'), isShowCancelLink);

            // check show save as filter link
            var isShowSaveAsLink = dataFilter != null && dataFilter != '' && currentFilteInfo && !currentFilteInfo.IsRecent
                && advancedFilter.currentFilter._ctrl.checkHasValueMerchant(advancedFilter.currentFilter._ctrl);;
            advancedFilter.currentFilter.showHideSection($('#' + this.sectionElmID + ' .js-saveas-link'), isShowSaveAsLink);

            if (!forceFocus) {
                setTimeout(function () {
                    aperia.searchControl.current.inputText.blur();
                }, 1);
            }
            var items = advancedFilter.currentFilter._ctrl.displayContainer.querySelectorAll('div.item');
            if (items.length == 0) {
                $(advancedFilter.currentFilter._ctrl.displayContainer).find('#displayList').addClass('hide');
                $('#btnApplyAdvFilter').attr('disabled', true);
            } else {
                if (!advancedFilter.currentFilter._ctrl.checkHasValueMerchant(advancedFilter.currentFilter._ctrl)) {
                    $('#btnApplyAdvFilter').attr('disabled', true);
                } else {
                    $('#btnApplyAdvFilter').attr('disabled', false);
                }
                $(advancedFilter.currentFilter._ctrl.displayContainer).find('#displayList').removeClass('hide');
            }

            // Show No item available filter
            var filterItems = advancedFilter.currentFilter._ctrl.fieldList;
            if (dataFilter != null && dataFilter != '' && dataFilter[0].SubFilterItems != null && dataFilter[0].SubFilterItems != '') {
                if (!$('#txt-filter-item').hasClass('hide')) {
                    $('#txt-filter-item').addClass('hide');
                    $('#displayList').addClass("remove-border");
                }
            }
            else {
                if (items.length == filterItems.length) {
                    $('#txt-filter-item').addClass('hide');
                    $('#no-filter-item').removeClass('hide');
                    $('#displayList').addClass("remove-border");
                } else {
                    $('#txt-filter-item').removeClass('hide');
                    $('#no-filter-item').addClass('hide');
                    $('#displayList').removeClass("remove-border");
                }
            }
        },

        initAF: function () {
            // set variable
            this.sectionElmID = 'advFilter' + this._sectionName;
            this._ctrl = aperia.searchControl('advInput' + this._sectionName);
            aperia.searchControl.current = this._ctrl;

            //click current input
            this._ctrl.events.addItemCallback = function (forceFocus) {
                advancedFilter.currentFilter.resetAF(forceFocus);
            };

            this._ctrl.events.onFocusTextField = function () {
                $('#savedFilters_' + advancedFilter.currentFilter._sectionName).collapse('hide');
                $('#recentFilters_' + advancedFilter.currentFilter._sectionName).collapse('hide');
            };

            $('#' + this.sectionElmID).click(function (e) {
                var closeCallback = false;
                if (aperia.searchControl.current.events.onCloseCallBack && e.target != aperia.searchControl.current.inputText) {
                    closeCallback = true;
                    if (aperia.searchControl.current.currentParentItem && e.target == aperia.searchControl.current.currentParentItem.currentInput) {
                        closeCallback = false;
                    }
                }

                if (closeCallback) {
                    aperia.searchControl.current.events.onCloseCallBack(e);
                }

                // Hide all toggle rename and save form
                var elm = e.target;
                if (!elm.hasAttribute('data-none-close-toggle')) {
                    var renameForm = $('[data-rename-form="true"]');
                    renameForm.each(function () {
                        advancedFilter.currentFilter.showHideSection($(this), false);
                    });
                }
            });

            // select saved filter event
            $('#list-saved-filter-items').off('click').on('click', 'span.filter-item', function (e) {
                var filters = $('#afDisplayContainer').find('div.item');
                var filterId = $(e.target).parents('li').attr('data-filter-id');

                window.FilterSearchId = filterId;
                window.FilterSearchSection = advancedFilter.constants.savedFilterSection;

                if (filters && filters.length > 0) {
                    advancedFilter.currentFilter.showLoadSavedFilterModal();
                }
                else {
                    advancedFilter.currentFilter.continueLoadSavedFilter();
                }
            });

            // select recent filter event
            $('#recentFilters_' + this._sectionName).off('click').on('click', 'span.filter-item', function (e) {
                var filters = $('#afDisplayContainer').find('div.item');
                var filterId = $(e.target).parents('li').attr('data-filter-id');

                window.FilterSearchId = filterId;
                window.FilterSearchSection = advancedFilter.constants.recentFilterSection;

                if (filters && filters.length > 0) {
                    advancedFilter.currentFilter.showLoadSavedFilterModal();
                }
                else {
                    advancedFilter.currentFilter.continueLoadSavedFilter();
                }
            });

            //click apply filter button:
            $('#btnApplyAdvFilter').click(function (e) {
                var isGenerate = this.getAttribute('data-is-generate') == 'True';
                aperia.searchControl.current.events.onCloseCallBack(e);
                advancedFilter.currentFilter.applyFilter(isGenerate);
            });

            $('#btnApplyAdvFilter').attr('disabled', true);

            //click close button:
            $('#btnCloseAdvFilter').click(function () {
                core.ui.closeSidebar('advFilter' + advancedFilter.currentFilter._sectionName);
            });

            $('#savedFilters_' + this._sectionName).off('show.bs.collapse').on('show.bs.collapse', function () {
                advancedFilter.currentFilter.calculateFilterItem();

                var items = $(advancedFilter.currentFilter._ctrl.displayContainer).find('div.item');
                if (items && items.length > 0) {
                    $('#afDisplayContainer').collapse('hide');
                    $('#collapseAF').removeClass('hide');
                }
                $('#recentFilters_' + advancedFilter.currentFilter._sectionName).collapse('hide');
                $(this).blur();
            });

            // Show Rename/Delete toggle
            $('#savedFilters_' + this._sectionName).off('click').on('click', 'div.action-more', function (e) {
                e.stopPropagation(); 
                var dropdown = $(this).closest('div.dropdown');
                if (dropdown.hasClass('open')) {
                    dropdown.removeClass('open');
                } else {                    
                    $('div.dropdown.open').removeClass('open');
                    dropdown.addClass('open');
                    advancedFilter.currentFilter.calculatePositionSelection(dropdown);
                }
            });

            $('#afDisplayContainer').off('show.bs.collapse').on('show.bs.collapse', function () {
                var filterItems = $(advancedFilter.currentFilter._ctrl.displayContainer).find('div.item');

                if (filterItems && filterItems.length > 0) {
                    $('#savedFilters_' + advancedFilter.currentFilter._sectionName).collapse('hide');
                    $('#recentFilters_' + advancedFilter.currentFilter._sectionName).collapse('hide');
                }
            });

            $('#collapseAF').on('click', function () {
                $('#collapseAF').addClass('hide');
                $('#afDisplayContainer').collapse('show');
                $('#afDisplayContainer').trigger('show.bs.collapse');
            });

            $('#recentFilters_' + this._sectionName).off('show.bs.collapse').on('show.bs.collapse', function () {
                advancedFilter.currentFilter.calculateFilterItem();

                var items = $(advancedFilter.currentFilter._ctrl.displayContainer).find('div.item');
                if (items && items.length > 0) {
                    $('#afDisplayContainer').collapse('hide');
                    $('#collapseAF').removeClass('hide');
                }
                $('#savedFilters_' + advancedFilter.currentFilter._sectionName).collapse('hide');
            });

            $(this._ctrl.displayContainer).on('onRemoveItemComplete', function () {
                advancedFilter.currentFilter.resetAF();
            });

            // Show Rename/Delete toggle for Filter Name on Top
            $('#filter-name-top').off('click').on('click', 'div.dropdown', function (e) {
                if ($(this).hasClass('open')) $(this).removeClass('open');
                else {
                    $(this).addClass("open");
                }
            });

            $('#savedFilters_' + advancedFilter.currentFilter._sectionName).scroll(function (e) {
                // Hide Rename/Delete and RenameForm.
                var elm = $(e.target).find('div.dropdown');
                if (elm.length) {
                    elm.removeClass('open');
                }
                elm = e.target;
                if (!elm.hasAttribute('data-none-close-toggle')) {
                    var renameForm = $('[data-rename-form="true"]');
                    renameForm.each(function () {
                        advancedFilter.currentFilter.showHideSection($(this), false);
                    });
                }
            });

            //Remove current class when hover suggest-item.
            var itemSuggest = $(advancedFilter.currentFilter._ctrl.fieldContainer).find('div.items-suggest div');
            $(itemSuggest).each(function () {
                this.onmouseover = function () {
                    var itemCurrent = $(advancedFilter.currentFilter._ctrl.fieldContainer).find('div.items-suggest div.current');
                    $(itemCurrent).removeClass('current');
                    advancedFilter.currentFilter._ctrl.fieldContainer.currentIndex = -1;
                    $(this).addClass('current');
                }
            });
        },

        calculatePositionSelection: function (element) {
            var ulSelection = element.find('ul');
            if (ulSelection.length) {
                ulSelection.css('position', 'FIXED');
                ulSelection.css('right', '40px');
                ulSelection.css('top', (element.offset().top + 22) + 'px');
            }
        },

        calculatePositionRenameForm: function (element) {
            element.css('position', 'FIXED');
            element.css('right', '60px');
            element.css('top', (element.parent().offset().top + 22) + 'px');
        },

        calculateFilterItem: function () {
            var filters = $('#afDisplayContainer').find('div.item');
            if (filters && filters.length > 0) {
                var textInfo = filters.length == 1 ? '1 ' + msg_UxAdvancedFilter_LblFilterOneItem : filters.length + " " + msg_UxAdvancedFilter_LblFilterMoreItem;
                $('#afInfo').text(textInfo);
            } else {
                $('#afInfo').text('');
            }
        },

        show: function () {
            core.ui.openSidebar('advFilter' + this._sectionName);
            this._ctrl.showAvaiableDateFilter(this._hddDateAvailableFilter);
            aperia.searchControl.current.updateHeightFilterList("calc-height");
        },

        events: {
            closeCallback: function (res) { }
        },

        showHideSection: function (element, isShow) {
            var hideclass = 'hide';
            if (isShow) {
                if (element.hasClass(hideclass)) {
                    element.removeClass(hideclass);
                }
            }
            else {
                if (!element.hasClass(hideclass)) {
                    element.addClass(hideclass);
                    element.find('.error').addClass('hide')
                }
            }
        },

        txtValue_KeyPress: function (e) {
            var evt = window.event ? window.event : e;
            var code = evt.keyCode ? evt.keyCode : e.which;

            if ((code >= 48 && code <= 57) ||
                code == 39 || code == 8 || code == 9)  //left arrow, righ arrow, del, backspace, tab
                return true;
            else {
                e.preventDefault ? e.preventDefault() : e.returnValue = false;
                return false;
            }
        },
    },

    postServer: function (url, data, callback, async) {
        if (async == undefined) async = true;
        $.ajax({
            type: "post",
            url: url,
            async: async,
            data: JSON.stringify(data),
            contentType: "application/json",
            dataType: "json",
            success: function (result) {
                if (callback) {
                    callback(result);
                }
            },
            error: function (result) {
                //console.log(result.responseText);
            }
        });
    },

    datePicker: {
        initPreDate: function (dateInput) {
            var widgetHeader = dateInput.datepicker("widget").find(".ui-datepicker-header");
            var prevMonth = $("<a class='ui-change prev-month'><span class='ui-icon ui-icon-circle-triangle-pm'></span></a>");
            var nextMonth = $("<a class='ui-change next-month'><span class='ui-icon ui-icon-circle-triangle-nm'></span></a>");
            var prevYear = $("<a class='ui-change prev-year'><span class='ui-icon ui-icon-circle-triangle-py'></span></a>");
            var nextYear = $("<a class='ui-change next-year'><span class='ui-icon ui-icon-circle-triangle-ny'></span></a>");

            prevYear.bind("click", function () {
                $.datepicker._adjustDate(dateInput, -1, 'Y');
                advancedFilter.datePicker.initPreDate(dateInput);
            });
            nextYear.bind("click", function () {
                $.datepicker._adjustDate(dateInput, +1, 'Y');
                advancedFilter.datePicker.initPreDate(dateInput);
            });
            prevMonth.bind("click", function () {
                $.datepicker._adjustDate(dateInput, -1, 'M');
                advancedFilter.datePicker.initPreDate(dateInput);
            });
            nextMonth.bind("click", function () {
                $.datepicker._adjustDate(dateInput, +1, 'M');
                advancedFilter.datePicker.initPreDate(dateInput);
            });

            prevYear.appendTo(widgetHeader);
            nextYear.appendTo(widgetHeader);
            prevMonth.appendTo(widgetHeader);
            nextMonth.appendTo(widgetHeader);
        }

    }
}


$(document).ready(function () {
    preventDefaultInput();
});

// Fix case enter input: reload page
function preventDefaultInput() {
    $('[data-adv-filter="true"] input[type="text"]').each(function () {
        $(this).off('keypress').on('keypress', function (e) {
            var evt = window.event ? window.event : e;
            var code = evt.keyCode ? evt.keyCode : e.which;
            if (code == 13) {
                e.preventDefault ? e.preventDefault() : e.returnValue = false;
                return false;
            }
        });
    });
}