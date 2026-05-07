///Search control
if (window.aperia == null) {
    window.aperia = function () { };
}
aperia.searchControl = function (ctrl) {
    if (ctrl == null) {
        var scripts = document.getElementsByTagName('script');
        ctrl = scripts[scripts.length - 1].parentNode;
    }
    else if (typeof ctrl == 'string') {
        ctrl = document.getElementById(ctrl);
    }
    var childDivs = ctrl.getElementsByTagName('div');
    var isHasSelectedItem = false;
    for (var i = 0; i < childDivs.length; i++) {
        switch (childDivs[i].getAttribute('type')) {
            case 'display':
                {
                    var childElems = childDivs[i].getElementsByTagName('input');
                    for (var m = 0; m < childElems.length; m++) {
                        var $childElems = $(childElems[m]);
                        if ($childElems.hasClass('input')) {
                            ctrl.inputText = childElems[m];
                        } else if ($childElems.hasClass('fake')) {
                            ctrl.fakeInputText = childElems[m];
                            //$(ctrl.fakeInputText).css('text-transform', 'capitalize');
                            ctrl.fakeInputText.defaultValue = childElems[m].value;
                            //ctrl.fakeInputText.placeholder = 'Click here to type or select new filter';
                        } else {
                            isHasSelectedItem = true;
                            $childElems.attr('name', 'searchbaritem_' + $childElems.attr('name'));
                            $childElems.parent().on('dblclick', function () {
                                ctrl.removeItem(this);
                            });
                        }
                    }
                    var divDisplayMultipleSelection = childDivs[i].querySelector('div[class*="display-multiple-selection"]');
                    if (divDisplayMultipleSelection) {
                        ctrl.fakeDisplayMultipleSelection = divDisplayMultipleSelection;
                        ctrl.fakeDisplayMultipleSelection.displayText = divDisplayMultipleSelection.querySelector('span');
                        ctrl.fakeDisplayMultipleSelection.displayValue = divDisplayMultipleSelection.querySelector('a');
                    }

                    ctrl.inputText.root = ctrl;
                    ctrl.inputText.forceFocus = function () {
                        //this.blur();
                        //this.focus();
                    }
                    ctrl.fakeInputText.root = ctrl;
                    ctrl.fakeInputText.onfocus = function () {
                        this.root.inputText.focus();
                    };
                    ctrl.displayContainer = childDivs[i];
                }
                break;
            case 'field':
                ctrl.fieldContainer = childDivs[i];
                ctrl.fieldContainer.root = ctrl;
                //ctrl.fieldContainer.url = ctrl.fieldContainer.getAttribute('url');
                break;
            case 'values':
                ctrl.valuesContainer = childDivs[i];
                ctrl.valuesContainer.root = ctrl;
                break;
            case 'validation':
                ctrl.validationContainer = childDivs[i];
                ctrl.validationContainer.root = ctrl;
                break;
            case 'buttons':
                ctrl.buttons = [];
                //bind event click delete all item
                ctrl.buttons.delete = childDivs[i].querySelector('span[data-type="delete"]');
                ctrl.buttons.delete.style.display = isHasSelectedItem ? '' : 'none';
                ctrl.buttons.delete.onclick = function () {
                    var len = ctrl.inputFieldList.length - 1;
                    for (var i = 0; i < len; i++) {
                        ctrl.removeLastItem();
                        ctrl.inputText.blur();
                        ctrl.fakeInputText.placeholder = msg_UxAdvancedFilter_PlaceHolderSelectFilter;
                        ctrl.reset();
                    }
                }
                ctrl.buttons.root = ctrl;
                break;
        }
    }

    ctrl.constants = {
        charDisplayValue: '#char#',
        charDataValue: ',',
        charDataBetween: '-',
        charQuoteEncode: '&apos'
    }

    ctrl.defaultSearchItems = ctrl.querySelectorAll('div[class="item"]');
    ctrl.name = ctrl.getAttribute('name');
    ctrl.distinct = (ctrl.getAttribute('distinct') || 'false') == 'true';
    ctrl.fieldContainer.style.width = ctrl.displayContainer.parentNode.clientWidth + 'px';
    if (ctrl.valuesContainer != null) ctrl.valuesContainer.style.width = ctrl.displayContainer.clientWidth + 'px';
    ctrl.fieldContainer.currentIndex = -1;
    ctrl.inValueMode = false;

    ctrl.updateInputTextSize = function () {
        var MIN_WIDTH = 60;
        this.inputText.parentNode.style.width = '0px';

        var computedStyle = window.getComputedStyle(this.displayContainer);
        var padLeft = parseInt(computedStyle.getPropertyValue('padding-left').replace('px', ''));
        var padRight = parseInt(computedStyle.getPropertyValue('padding-right').replace('px', ''));

        var computedStyleParentNode = window.getComputedStyle(this.displayContainer.parentNode);
        var padRightParentNode = parseInt(computedStyleParentNode.getPropertyValue('padding-right').replace('px', ''));

        var maxWidth = this.displayContainer.clientWidth - padLeft;

        var usedWidth = this.inputText.parentNode.offsetLeft - 23 + 12;
        var newSize = maxWidth - usedWidth;
        if (newSize < MIN_WIDTH) newSize = maxWidth
        this.inputText.parentNode.style.width = newSize + 'px';
    };
    /// multi value
    ctrl.isMultiValue = function (fieldName) {
        var suggestItems = this.fieldList;
        for (var i = 0; i < suggestItems.length; i++) {
            if (suggestItems[i].getAttribute('keyword') == fieldName)
                return suggestItems[i].getAttribute('data-is-multi-value') == 'True' && !(suggestItems[i].input && suggestItems[i].input.multiSelect);
        }
        return false;
    };
    ctrl.getSelectedItems = function (fieldName) {
        var selectedItems = [];
        var items = this.displayContainer.querySelectorAll("div.item");
        if (items.length > 0) {
            for (var i = 0; i < items.length; i++) {
                var values = items[i].getElementsByTagName("input")[0].value.split(":");
                var value = values.length <= 1 ? "" : values[1];
                if (values[0].toLocaleLowerCase() == fieldName.toLocaleLowerCase()) {
                    selectedItems.push({ name: values[0], value: value });
                }
            }
        }
        return selectedItems;
    };
    ctrl.getValueList = function (field) {
        var values = field.getAttribute("values");
        if (values == null || values.length == 0) {
            return null;
        }

        var divs = this.valuesContainer.getElementsByClassName("field");
        for (var i = 0, length = divs.length; i < length; i++) {
            if (divs[i].getAttribute("name") !== values) {
                continue;
            }

            var listContainer = divs[i].getElementsByClassName("items");
            if (listContainer != null && listContainer.length > 0) {
                return listContainer[0].getElementsByTagName("div");
            }
        }

        return null;
    };
    /// END multi value

    ctrl.updateDisplayContainerHeight = function (isScrollBottom) {
        var displayList = $(this.displayContainer).find('#displayList');
        var itemHeight = 0;

        var totalHeight = $('.content-searchcontrol').outerHeight();
        displayList.css('max-height', totalHeight / 3);

        $.each(displayList.find('[item-search-name]'), function (index, item) {
            itemHeight += $(item).outerHeight();
        });

        if (isScrollBottom) {
            displayList.animate({ scrollTop: displayList.offset().top + itemHeight }, 500);
        }
    };

    ctrl.updateSuggestionContainerHeight = function (isShow) {
        var currentHeight = 0;
        var totalHeight = $("div[data-selector*='content-select']").outerHeight();
        var topSearchControl = $("div[data-selector*='content-select']").offset().top;
        var topSuggested = $("div[data-selector*='suggested-select'] .items").offset().top;
        $("div[data-selector*='suggested-select'] .items").css({ "max-height": totalHeight + topSearchControl - topSuggested - 20 + "px", "overflow-y": "auto" })
    };

    ctrl.updateMultiSelectHeight = function () {
        var totalHeight = $("div[data-selector*='content-select']").outerHeight();
        var items = $(this.currentInput).find(".items");
        var topSearchControl = $("div[data-selector*='content-select']").offset().top;
        if (items.length > 0) {
            var maxHeight = items.offset().top - topSearchControl;
            items.css({ "max-height": totalHeight - maxHeight - 20 + "px" })
        }

    }

    ctrl.calculateContainerHeight = function (currentHeight) {
        var displayList = $(this.displayContainer).find('#displayList');
        if (displayList.children().length > 0) {
            displayList.removeClass('hide');
        }
        var totalHeight = $('.content-searchcontrol').outerHeight();
        var displayListHeight = 0;
        $.each(displayList.children(), function (index, elm) {
            displayListHeight += $(elm).outerHeight();
        });
        if (displayListHeight > totalHeight / 2) {
            displayListHeight = totalHeight / 2;
        }
        currentHeight += displayListHeight;

        this.updateDisplayContainerHeight();
        return false;
    };

    ctrl.updateValueContainerHeight = function () {
        var valueBoxHeight = 0;
        $.each($(this.valuesContainer).children(':visible'), function (index, elm) {
            valueBoxHeight += $(elm).outerHeight();
        });
        ctrl.calculateContainerHeight(valueBoxHeight);
        ctrl.updateMultiSelectHeight();
    };

    ctrl.updateHeightFilterList = function (classCalc) {
        if (!$("div[data-selector*='content-select']").hasClass("calc-height calc-height-loaded")) {
            $("div[data-selector*='content-select']").addClass(classCalc);
            var totalHeight = $("div[data-selector*='content-select']").outerHeight();
            var topSearchControl = $("div[data-selector*='content-select']").offset().top;
            // 366: $("div[data-selector*='filter-content']").offset().top when filter input collapse
            var headerHeight = 366 - topSearchControl;
            var filterHeight = totalHeight - headerHeight - 130;
            $("div[data-selector*='save-filter']").css({ "max-height": filterHeight - 10 + "px", "overflow-y": "hidden" })
            $("div[data-selector*='recent-filter']").css({ "max-height": filterHeight - 10 + "px", "overflow-y": "hidden" })
        }
    }

    ctrl.updateHeightMultiChosen = function () {
        var totalHeight = $("div[data-selector*='content-select']").outerHeight();
        var topSearchControl = $("div[data-selector*='content-select']").offset().top;
        var top = $(".chosen-container").offset().top;
        var height = 200;
        height = totalHeight + topSearchControl - top - 30;

        $(".chosen-container .chosen-choices").css({ "max-height": height * 0.45 + "px" })
        $(".chosen-container .chosen-results").css({ "max-height": height * 0.55 + "px" })
        $(".chosen-container .chosen-drop").css({ "max-height": height * 0.55 + "px" })
    }

    ctrl.updateHeightFilterItem = function () {
        var displayList = $(this.displayContainer).find('#displayList');
        var totalHeight = $('.content-searchcontrol').outerHeight();
        displayList.css('max-height', totalHeight / 3);
    }

    ctrl.updateDatePickerContainerHeight = function (isShow) {
        var currentHeight = 0;
        if (isShow) {
            currentHeight = $(aperia.searchControl.datePickerWidget).outerHeight();
            var datePickerContainerHeight = $(aperia.searchControl.datePickerWidget).parent().outerHeight();
            if (datePickerContainerHeight > currentHeight) {
                currentHeight = datePickerContainerHeight;
            }
        }
        ctrl.calculateContainerHeight(currentHeight);
    };

    ctrl.updateWidthTxtAutoComplete = function (val) {
        var wid = "25px";
        if (val) {
            wid = ((val.length + 2) * 10) + 'px';
        }

        $('#inputValue').css("width", wid);
    }

    ctrl.hideFieldSuggestion = function () {
        this.fieldContainer.style.display = 'none';
    };

    ctrl.showValueSuggestion = function (reset) {
        if (reset == null) reset = true;
        if (this.valuesContainer != null) {
            var selectedItems = this.getSelectedItems(this.inputField);
            if (reset && this.currentInput != null) {
                if (this.currentInput.multiSelect && this.currentInput.resetCheckboxes) {
                    //reset checkbox
                    this.currentInput.resetCheckboxes();

                    if (this.currentParentItem) {
                        this.currentParentItem.collapseLink.className = '';
                        this.currentParentItem.toggleHiddenInputField(true);
                    } else {
                        this.toggleHiddenInputField(true);
                    }
                }

                //Show hide back button
                if (this.currentInput.toggleEditMode) {
                    this.currentInput.toggleEditMode(false);
                }

                // Bind Item List
                ctrl.bindSuggestItems();

                var suggestItems = this.currentInput.valueList;
                for (var i = 0; i < suggestItems.length; i++) {
                    suggestItems[i].style.display = 'block';
                    suggestItems[i].className = '';

                    var value = suggestItems[i].getAttribute("value");
                    for (var j = 0; j < selectedItems.length; j++) {
                        if (this.inputField == selectedItems[j].name && value == selectedItems[j].value) {
                            suggestItems[i].style.display = 'none';
                        }
                    }
                }
                this.currentInput.currentIndex = -1;
            }

            this.valuesContainer.style.display = 'block';
            this.currentInput.style.display = 'block';
            ctrl.updateValueContainerHeight();
            $(this.fieldContainer).css('height', '');
            $(".container-field").addClass("show-effect");
        }
    };

    ctrl.hideValueSuggestion = function (input) {
        if (this.valuesContainer != null) {
            this.valuesContainer.style.display = 'none';
            ctrl.toggleEffect(false);
        }
    };
    ctrl.hideValueItemSuggestion = function (input) {
        if (input) {
            switch (input.type) {
                case 'MultiSelection':
                    var items = input.valueList;
                    for (var i = 0; i < items.length; i++) {
                        items[i].style.display = 'none';
                    }
                    input.style.display = 'none';
                    break;
            }
        }
    };
    ctrl.toggleEffect = function (isShow) {
        if (isShow) {
            $(".container-field").addClass("show-effect");
        }
        else {
            $(".container-field").removeClass("show-effect");

        }
    };

    ctrl.toggleDisplay = function (isShow) {
        if (isShow) {
            $(".container-field").addClass("show-imp");
        }
        else {
            $(".container-field").removeClass("show-imp");
        }
    };
    ctrl.moveSuggestionItem = function (up) {
        var suggestItems = null;
        var currentIndex = -1;
        if (this.inValueMode) {
            if (this.currentInput != null && this.currentInput.valueList != null) {
                suggestItems = this.currentInput.valueList;
                currentIndex = this.currentInput.currentIndex;
            }
            else {
                return;
            }
        }
        else {
            suggestItems = this.fieldList;
            currentIndex = this.fieldContainer.currentIndex;
        }

        var step = 1;
        if (up) {
            step = -1;
        }
        var oldIndex = currentIndex;
        do {
            currentIndex += step;
        } while (currentIndex >= 0 && currentIndex <= suggestItems.length - 1 && suggestItems[currentIndex].style.display == 'none');
        if (suggestItems[currentIndex] != null) {
            //clear current high light
            var itemCurrent = $(ctrl.fieldContainer).find('div.items-suggest div.current');
            $(itemCurrent).removeClass('current');

            suggestItems[currentIndex].className = 'current';
            var suggestedValue = suggestItems[currentIndex].getAttribute('data-display-value');
        }
        else {
            currentIndex = oldIndex;
        }
        if (this.inValueMode) {
            this.currentInput.currentIndex = currentIndex;
        }
        else {
            this.fieldContainer.currentIndex = currentIndex;
        }

        // Auto scroll
        var currentItem = $(this.currentInput).find('.items');
        currentItem.scrollTop(0); //set to top
        currentItem.scrollTop($(suggestItems[currentIndex]).offset().top - currentItem.height())

        return currentIndex;
    };

    ctrl.suggestValue = function () {
        var result = false;

        if (!this.currentParentItem && !this.isAddSubItem && !this.currentAddingMultiSelectItem) {
            this.fakeInputText.value = ' ';
        }
        this.currentInput.listContainer.style.display = 'block'; //.show();
        this.currentInput.messageItem.innerHTML = this.currentInput.messageItem.defaultMsg;
        this.suggestValueItem = null;
        this.analyseInput();
        var inputValue = this.inputValue;
        if (inputValue != null) {
            var items = this.currentInput.valueList;
            var selectedItems = this.getSelectedItems(this.inputField);
            // input value is blank
            if (inputValue == '') {
                for (var i = 0; i < items.length; i++) {
                    items[i].style.display = 'block';
                    items[i].className = '';
                    var value = items[i].getAttribute("value");
                    for (var j = 0; j < selectedItems.length; j++) {
                        if (this.inputField.toLocaleLowerCase() == selectedItems[j].name.toLocaleLowerCase()
                            && value == selectedItems[j].value) {
                            items[i].style.display = 'none';
                        }
                    }
                }
            }
            else {
                //find macthed
                this.suggestValueItem = null;
                var suggestValue = '';
                for (var i = 0; i < items.length; i++) {
                    var selectedFlag = false;
                    suggestValue = items[i].getAttribute('data-display-value');
                    var value = items[i].getAttribute("value");
                    // check exist in selected items
                    for (var j = 0; j < selectedItems.length; j++) {
                        if (this.inputField.toLocaleLowerCase() == selectedItems[j].name.toLocaleLowerCase()
                            && value == selectedItems[j].value) {
                            selectedFlag = true;
                            break;
                        }
                    }
                    if (suggestValue.toLowerCase().startsWith(inputValue.toLowerCase()) && !selectedFlag) {
                        if (this.suggestValueItem == null) {
                            if (this.currentInput.currentIndex >= 0)
                                items[this.currentInput.currentIndex].className = '';
                            this.currentInput.currentIndex = i;
                            this.suggestValueItem = items[i];
                            this.suggestValueItem.className = 'current';
                            //this.suggestValueItem.scrollIntoView();

                            var value = this.inputText.value + suggestValue.substr(inputValue.length);
                            if (this.currentParentItem && this.isAddSubItem && this.currentParentItem.fakeInput) {
                                //this.currentParentItem.fakeInput.value = value;
                            } else {
                                //this.fakeInputText.value = value;
                            }

                            result = true;
                        }
                        items[i].style.display = 'block';
                    }
                    else {
                        items[i].style.display = 'none';
                    }
                    if (items[i].input != null)
                        items[i].input.style.display = 'none';
                }
                if (this.suggestValueItem == null) {
                    this.currentInput.listContainer.style.display = 'none';
                    this.currentInput.messageItem.innerHTML = 'No item is matched';
                }
            }
        }

        return result;
    };
    ctrl.analyseInput = function () {
        var inputText = this.inputText.value;
        var pos = inputText.lastIndexOf(':');
        if (pos > 0) {
            if (pos == inputText.length - 1) {
                this.inputText.value = inputText + ' ';
            }
            this.inputField = inputText.substr(0, pos);
            this.inputValue = inputText.substr(pos + 1).trim();
            this.inputKeyword = this.inputText.getAttribute('data-keyword');
        }
        else {
            this.inputField = inputText;
            this.inputValue = null;
            this.inputKeyword = this.inputText.getAttribute('data-keyword');
        }
    }
    ctrl.suggestField = function () {
        var result = false,
            isEmptyList = true;

        if (this.currentParentItem && this.isAddSubItem && this.currentParentItem.fakeInput) {
            this.currentParentItem.fakeInput.value = '';
        } else {
            this.fakeInputText.value = ' ';
        }
        this.fieldContainer.listContainer.show();
        this.analyseInput();
        var items = this.fieldList;

        if (this.inputField == '') {
            if (this.isAddSubItem && this.currentParentItem) {
                var parentKey = this.currentParentItem.getAttribute('item-search-name');
                if (parentKey) {
                    for (var i = 0; i < items.length; i++) {
                        if (this.distinct && this.isFieldSelected(items[i], true)) {
                            items[i].style.display = 'none';
                        }
                        else {
                            if (items[i].getAttribute('data-parent-key') != parentKey) {
                                items[i].style.display = 'none';
                            } else {
                                items[i].style.display = 'block';
                                isEmptyList = false;
                            }
                        }
                        items[i].className = '';
                    }
                }

            } else {
                for (var i = 0; i < items.length; i++) {
                    if (this.distinct && this.isFieldSelected(items[i])) {
                        items[i].style.display = 'none';
                    }
                    else {
                        if (this.isChildItem(items[i])) {
                            items[i].style.display = 'none';
                        } else {
                            items[i].style.display = 'block';
                            isEmptyList = false;
                        }
                    }
                    items[i].className = '';
                }
            }

            // Hide modifiers list when no more items left
            if (isEmptyList) {
                this.fieldContainer.style.display = 'none';
                this.fakeInputText.placeholder = "";
            } else {
                this.fakeInputText.placeholder = msg_UxAdvancedFilter_PlaceHolderSelectFilter;
            }
        }
        else {
            var inputValue = this.inputField;
            var inputKeyword = this.isAddSubItem && this.currentParentItem ? this.currentParentItem.currentInput.getAttribute('data-keyword') : this.inputKeyword;

            //find macthed field
            this.matchedField = null;

            var parentKey = this.isAddSubItem && this.currentParentItem ? this.currentParentItem.getAttribute('item-search-name') : null;

            for (var i = 0; i < items.length; i++) {
                var fieldName = items[i].getAttribute('data-display-value');
                var fieldNameParent = items[i].getAttribute('data-parent-key');
                var keyword = items[i].getAttribute('keyword');
                if ((!this.distinct || ((!parentKey && !this.isFieldSelected(items[i]))
                    || (parentKey && !this.isFieldSelected(items[i], true))))
                    && ((!inputKeyword && fieldName.toLowerCase().indexOf(inputValue.toLowerCase()) !== -1)
                        || (inputKeyword && inputKeyword.toLowerCase() == keyword.toLowerCase()))
                    && ((parentKey && fieldNameParent && fieldNameParent == parentKey)
                        || (!parentKey && !fieldNameParent))) {

                    if (this.matchedField == null) {
                        if (this.fieldContainer.currentIndex >= 0)
                            items[this.fieldContainer.currentIndex].className = '';
                        this.fieldContainer.currentIndex = i;
                        this.matchedField = items[i];
                        this.matchedField.className = 'current';
                        //  this.matchedField.scrollIntoView();
                        // Bug #27316 
                        if (this.currentParentItem && this.isAddSubItem && this.currentParentItem.fakeInput) {
                            //this.currentParentItem.fakeInput.value = fieldName;
                        } else {
                            //this.fakeInputText.value = fieldName;
                        }
                        if (this.inputText.value.lastIndexOf(':') < 0) {
                            //this.inputText.value = fieldName.substring(0, inputValue.length);
                        }
                        result = true;
                    }
                    items[i].style.display = 'block';
                }
                else {
                    items[i].style.display = 'none';
                }
                if (items[i].input != null)
                    items[i].input.style.display = 'none';
            }

            if (this.matchedField == null) {
                this.hideFieldSuggestion();
                this.toggleDisplay(false);
            }
            else {
                if (this.matchedField.input != null) {
                    this.currentInput = this.matchedField.input;
                }
                else {
                    this.currentInput = null;
                    if (this.inputText.value && this.inputText.value.lastIndexOf(':') > -1) {
                        this.hideFieldSuggestion();
                        this.toggleDisplay(false);
                    }
                }
            }
        }
        return result;
    };
    ctrl.showFieldSuggestion = function (reset) {
        if (reset == null) reset = true;

        this.hideValueSuggestion();

        if (reset) {
            var suggestItems = this.fieldList;

            for (var i = 0; i < suggestItems.length; i++) {
                suggestItems[i].style.display = 'block';
                suggestItems[i].className = '';
            }
            this.fieldContainer.currentIndex = -1;
        }

        if (!this.inputText.value || this.inputText.value.lastIndexOf(':') == -1) {
            this.fieldContainer.style.display = 'block';
            ctrl.updateSuggestionContainerHeight(true);
            ctrl.toggleDisplay(true);
        }
    };

    ctrl.removeItem = function (item) {
        if (this.isParentItem(item)) {
            //remove add sub filter link
            var link = item.querySelector('a[class="add-sub-filter-link"]');
            if (link) {
                item.removeChild(link);
            }
            this.inputText.style.display = '';
            this.currentParentItem = null;
            this.isAddSubItem = false;
        }
        var inputText = item.textContent || item.innerText || '';
        this.inputText.value = inputText;
        $(item).remove();
        this.inputText.focus();
        //this.updateInputTextSize();
        var items = this.displayContainer.querySelectorAll('div.item');
        if (items.length == 0) {
            this.buttons.delete.style.display = 'none';
            $(ctrl.displayContainer).find('#displayList').addClass('hide');
        }
    };

    ctrl.editingItem = null;
    ctrl.editItem = function (item, obj) {
        // Close suggestion field of Parent Item if exist
        var itemParent = $(ctrl.displayContainer).find("div[data-is-parent='true']");
        var attrParent = itemParent != undefined ? itemParent.attr('data-is-parent-open') : '';
        if (attrParent == 'true') {
            // Close suggestion field for parent item
            ctrl.reset();
            itemParent.removeAttr('data-is-parent-open');
        }

        // Remove item is empty
        this.onBlurEditMultipleValue();

        var inputName = item.getAttribute('data-display-name');
        var inputKey = item.getAttribute('item-search-name');
        var inputParent = item.getAttribute('data-parent-key');

        this.inputField = inputName;
        this.hideAllValueSuggestions();
        this.hideFieldSuggestion();

        //get current input
        var items = this.fieldList;
        var currentItem = items.filter(function (idx, field) {
            var fieldKey = field.getAttribute('keyword');
            var fieldParent = field.getAttribute('data-parent-key');
            return field && fieldKey == inputKey && ((inputParent && inputParent == fieldParent) || (!inputParent));
        });

        if (!currentItem || currentItem.length == 0) return false;

        var currentInput = currentItem[0].input;
        this.currentInput = currentInput;

        // Bind value of Parameter
        ctrl.bindSuggestItems();

        var result = false;
        item.input = currentInput;
        if (currentInput.toggleEditMode) {
            // Show title or Back button
            currentInput.toggleEditMode(true);
        }

        // Show value
        this.valuesContainer.style.display = 'block';
        currentInput.style.display = 'block';

        this.editingItem = item;
        if (item.getAttribute('data-parent-key') != undefined) {
            ctrl.toggleDisplay(false);
            this.editingItem.editingChildItem = item;
        }

        // Caculate text width
        function textWidth(text) {
            var tempHtml = "<div style='visibility: hidden;'>" + text + "</div>";
            $("body").append(tempHtml);
            var width = $("body").find("div:last").css("width");
            $("body").find("div:last").remove();
            return width;
        }

        ctrl.updateValueContainerHeight(true);

        if (currentInput != null) {
            currentInput.resetValue();

            switch (currentInput.type) {
                case 'DatePicker':
                    {
                        var value = $(item).attr('data-value');
                        var displayValue = $(item).attr('data-display-value');
                        var data = value.split('-');
                        if (data[0] == 'DateRange') {
                            currentInput.fromTextbox.value = data[1].trim();
                            currentInput.toTextbox.value = data[2].trim();

                            currentInput.changeContent(true);

                            if ($(obj).attr('first-value') == 'true') {
                                currentInput.fromTextbox.focus();
                            }
                            else {
                                currentInput.toTextbox.focus();
                            }

                            $(currentInput).find('input:radio[value="DateRange"]')[0].checked = true;
                        }
                        else {
                            currentInput.changeContent(false);
                            currentInput.dailyTextbox.value = data[1].trim();
                            currentInput.dailyTextbox.focus();
                        }

                        $(currentInput).find('input:radio[value="' + data[0].trim() + '"]')[0].checked = true;
                        break;
                    }
                case 'MultiSelection':
                    {
                        if (this.valuesContainer != null) {
                            var suggestItems = currentInput.valueList;

                            for (var i = 0; i < suggestItems.length; i++) {
                                suggestItems[i].style.display = 'block';
                                suggestItems[i].className = '';
                                var checkbox = suggestItems[i].querySelector('input[type="checkbox"]');

                                var value = suggestItems[i].getAttribute("value");
                                var selectedItem = $(item).attr('data-value');
                                var valueArr = selectedItem ? selectedItem.split(',') : [];
                                var filter = valueArr.filter(function (val) {
                                    return value && val && value == val;
                                });

                                checkbox.checked = filter && filter.length > 0;
                            }

                            currentInput.changeLinkContent();
                        }
                    }
                    break;
                case 'Radio':
                    {
                        var value = $(item).attr('data-value');

                        var radios = currentInput.radios;
                        for (var index = 0; index < radios.length; index++) {
                            radios[index].checked = radios[index].getAttribute("value") == value;
                        }
                    }
                    break;
                case 'RangeValue':
                    {
                        var dataValue = $(item).attr('data-value');
                        var data = dataValue.split('-');

                        if ($(obj).attr('first-value') == 'true') {
                            currentInput.fromTextbox.focus();
                        }
                        else {
                            currentInput.toTextbox.focus();
                        }

                        currentInput.fromTextbox.value = data[0].trim();
                        currentInput.toTextbox.value = data[1].trim();
                    }
                    break;
                case 'RangeRadio':
                    {
                        var value = $(item).attr('data-value');
                        var displayValue = $(item).attr('data-display-value');
                        var data = value.split('-');

                        if (data[0] == 'Between') {
                            currentInput.fromTextbox.value = data[1].trim();
                            currentInput.toTextbox.value = data[2].trim();

                            currentInput.changeContent(true);

                            if ($(obj).attr('first-value') == 'true') {
                                currentInput.fromTextbox.focus();
                            }
                            else {
                                currentInput.toTextbox.focus();
                            }

                            $(currentInput).find('input:radio[value="Between"]')[0].checked = true;
                        }
                        else {
                            currentInput.changeContent(false);
                            currentInput.textbox.value = data[1].trim();
                            currentInput.textbox.focus();
                        }

                        $(currentInput).find('input:radio[value="' + data[0].trim() + '"]')[0].checked = true;
                    }
                    break;
                case 'SingleSelection':
                    {
                        var value = $(item).attr('data-value');

                        var listItem = currentInput.list;
                        for (var i = 0; i < listItem.length; i++) {
                            if (listItem[i].getAttribute("value") == value)
                                $(listItem[i]).addClass('selected');
                        }
                    }
                    break;
                case 'Text':
                    {
                        var value = $(item).attr('data-value');
                        currentInput.textbox.value = value;
                    }
                    break;
                case 'MultiChoose':
                    {
                        var value = $(item).attr('data-value');
                        currentInput.multiChoose.val(value.split(','));
                        currentInput.multiChoose.trigger("chosen:updated");
                    }
                    break;
                case 'OrderBy':
                    {
                        var value = $(item).attr('data-value').split(' ');
                        currentInput.orderCombo.val(value[0]);

                        $(currentInput).find('input:radio[value="' + value[1].trim() + '"]')[0].checked = true;
                    }
                    break;
                case 'AutoComplete':
                    {
                        var value = $(item).attr('data-value').split(',');
                        var display = $(item).attr('data-display-autocomplete').split('#;');

                        for (var i = 0; i < value.length; i++) {
                            if ((ctrl.currentParentItem || this.editingItem.editingChildItem) && ctrl.checkRequiredMerchant()) {
                                var widthtemp = textWidth(display[i]);
                                $(currentInput).find("#inputValue").val(display[i]);
                                $(currentInput).find("#inputValue").css("width", widthtemp);
                            } else {
                                var liTag = document.createElement('li');
                                liTag.className = 'search-choice';

                                var spanTag = document.createElement('span');
                                spanTag.textContent = display[i];

                                var iconCloseTag = document.createElement('a');
                                iconCloseTag.className = 'search-choice-close';
                                iconCloseTag.setAttribute('item-value', value[i]);
                                iconCloseTag.onclick = function () {
                                    ctrl.removeAutoCompleteItem(this);
                                };

                                $(liTag).append(spanTag);
                                $(liTag).append(iconCloseTag);
                                $($(currentInput).find("ul.chosen-choices li:last-child")[0]).before(liTag);
                            }
                        }

                    }
                    break;
            }
        }
    };

    ctrl.resetEditingItem = function () {
        if (this.editingItem) {
            var removeEditing = false;
            if (this.isParentItem(this.editingItem) && this.editingItem.editingChildItem) {
                removeEditing = this.editingItem.editingChildItem.input.multiSelect;

                this.editingItem.editingChildItem.input.style.display = 'none'
                this.editingItem.editingChildItem = null;
            }

            if (this.editingItem.input) {
                switch (this.editingItem.input.type) {
                    case 'MultiSelection':
                        var items = this.editingItem.input.valueList;
                        for (var i = 0; i < items.length; i++) {
                            items[i].style.display = 'none';
                        }
                        this.editingItem.input.style.display = 'none';
                        break;
                    case 'Radio':
                        var items = this.editingItem.input.valueList;
                        for (var i = 0; i < items.length; i++) {
                            items[i].style.display = 'none';
                        }
                        this.editingItem.input.style.display = 'none';
                        break;
                }
            }

            if (this.editingItem.onmouseover && this.editingItem.getListChildItems && this.editingItem.getListChildItems().length == 0) {
                this.editingItem.onmouseover();
            }

            //remove editing item if mutiselect
            if (removeEditing) {
                this.editingItem = null;
            }
        }
    }

    ctrl.hideAllValueSuggestions = function () {
        var suggestions = this.valuesContainer.querySelectorAll('div[class="field"]');
        for (var i = 0; i < suggestions.length; i++) {
            suggestions[i].style.display = 'none';
        }
    };

    ctrl.removeItemNotKeepVal = function (item) {
        var inputText = '';
        this.inputText.value = inputText;
        this.hideValueInput(item);
        $(item).remove();
        this.events.sendRemoveItemCallback();

    };

    ctrl.removeRangeItem = function (item) {
        var inputText = '';
        this.inputText.value = inputText;
        this.hideValueInput(item);
        $(item).remove();
        this.events.sendRemoveItemCallback();
    };

    ctrl.removeLastItem = function () {
        //var items = this.displayContainer.getElementsByTagName('div');
        //if (items.length >= 2) {
        //    this.removeItem(items[items.length - 2]);
        //}
        if (this.currentParentItem && this.isAddSubItem) {
            if (this.currentParentItem.getListChildItems().length > 0) {
                this.currentParentItem.removeLastChildItem();
            } else {
                this.removeItem(this.currentParentItem);
            }
        } else {
            var items = this.displayContainer.querySelectorAll('div.item');
            if (items.length > 0) {
                //check if last item is parent item
                var lastItem = items[items.length - 1];


                if (this.isParentItem(lastItem) && lastItem.getListChildItems().length > 0) {
                    var childItems = lastItem.getListChildItems();
                    var lastChildItem = childItems[childItems.length - 1];
                    var inputName = lastChildItem.getAttribute('data-display-name');
                    var currentValue = lastChildItem.getAttribute('data-display-value');

                    //get current input
                    var currentItem = this.fieldList.filter(function (idx, field) {
                        return field && field.getAttribute('data-display-value') == inputName;
                    });
                    var currentInput = currentItem[0].input;

                    if (currentInput && currentInput.multiSelect) {
                        lastItem.editItem(lastChildItem);
                        this.isKeyHandled = true;
                        this.fakeInputText.value = '';
                        this.inputText.blur();
                    }
                    else {
                        lastItem.removeLastChildItem();
                    }
                }
                else {

                    var inputName = lastItem.getAttribute('data-display-name');
                    var currentValue = lastItem.getAttribute('data-display-value');

                    //get current input
                    var currentItem = this.fieldList.filter(function (idx, field) {
                        return field && field.getAttribute('data-display-value') == inputName;
                    });
                    var currentInput = currentItem[0].input;

                    if (currentInput && currentInput.multiSelect) {
                        this.editItem(lastItem);
                        this.isKeyHandled = true;
                        //this.fakeInputText.placeholder = 'Click here to type or select new filter';
                        this.fakeInputText.value = '';
                        this.inputText.blur();
                    } else {
                        this.removeItem(lastItem);
                    }
                }
            }
        }
    };

    ctrl.removeAutoCompleteItem = function (item) {
        var value = (ctrl.currentParentItem && ctrl.currentParentItem.fakeDisplayMultipleSelection)
            ? ctrl.currentParentItem.fakeDisplayMultipleSelection.getAttribute('data-value')
            : (ctrl.currentAddingMultiSelectItem ? ctrl.currentAddingMultiSelectItem.getAttribute('data-value') : '');
        var displayVal = (ctrl.currentParentItem && ctrl.currentParentItem.fakeDisplayMultipleSelection)
            ? ctrl.currentParentItem.fakeDisplayMultipleSelection.displayValue.textContent
            : (ctrl.currentAddingMultiSelectItem ? ctrl.currentAddingMultiSelectItem.getAttribute('data-display-value') : '');
        var displayAutoComplete = (ctrl.currentParentItem && ctrl.currentParentItem.fakeDisplayMultipleSelection)
            ? ctrl.currentParentItem.fakeDisplayMultipleSelection.getAttribute('data-display-autocomplete')
            : (ctrl.currentAddingMultiSelectItem ? ctrl.currentAddingMultiSelectItem.getAttribute('data-display-autocomplete') : '');


        if (ctrl.editingItem) {
            value = ctrl.editingItem.getAttribute('data-value');
            displayVal = ctrl.editingItem.getAttribute('data-display-value');
            displayAutoComplete = ctrl.editingItem.getAttribute('data-display-autocomplete');
        }

        var valueArr = value ? value.split(',') : [];
        var displayValArr = displayVal ? displayVal.split(', ') : [];
        var displayAutoCompleteArr = displayAutoComplete ? displayAutoComplete.split('#;') : [];

        var removedValIndex = valueArr.indexOf(item.getAttribute('item-value'));
        if (removedValIndex > -1) {
            displayValArr.splice(removedValIndex, 1);
            valueArr.splice(removedValIndex, 1);
            displayAutoCompleteArr.splice(removedValIndex, 1);
        }

        var dataValue = valueArr.length > 0 ? valueArr.join(',') : '';
        var dataDisplayValue = displayValArr.length > 0 ? displayValArr.join(', ') : '';
        var dataDisplayAutoComplete = displayAutoCompleteArr.length > 0 ? displayAutoCompleteArr.join('#;') : [];

        if (ctrl.editingItem) {
            var displayValueLink = ctrl.editingItem.editingChildItem ? ctrl.editingItem.editingChildItem.querySelector('a[class="display-value"]') : ctrl.editingItem.querySelector('a[class="display-value"]');
            displayValueLink.textContent = dataDisplayValue;

            ctrl.editingItem.setAttribute('data-value', dataValue);
            ctrl.editingItem.setAttribute('data-display-value', dataDisplayValue);
            ctrl.editingItem.setAttribute('data-display-autocomplete', dataDisplayAutoComplete);
        }
        else {
            if (ctrl.currentParentItem && ctrl.currentParentItem.fakeDisplayMultipleSelection) {
                ctrl.currentParentItem.fakeDisplayMultipleSelection.displayValue.textContent = dataDisplayValue;
                ctrl.currentParentItem.fakeDisplayMultipleSelection.setAttribute('data-value', dataValue);
                ctrl.currentParentItem.fakeDisplayMultipleSelection.setAttribute('data-display-value', dataDisplayValue);
                ctrl.currentParentItem.fakeDisplayMultipleSelection.setAttribute('data-display-autocomplete', dataDisplayAutoComplete);
            }
            else {
                ctrl.displayValue(dataDisplayValue, dataValue, "", dataDisplayAutoComplete);
            }
        }

        ctrl.events.addItemCallback(true);
        ctrl.updateValueContainerHeight();

        $(item.parentNode).remove();
    };

    ctrl.isFieldSelected = function (field, isSub) {
        var name = field.getAttribute('data-display-value');
        var keyword = field.input.keyword;

        var items = isSub ? this.querySelectorAll('div.child-item') : this.displayContainer.querySelectorAll('div.item');
        if (items.length > 0) {
            var countOrderBy = 0;
            for (var i = 0; i < items.length; i++) {
                if (keyword == items[i].getAttribute('item-search-name')) {
                    if (keyword == "OrderBy") {
                        if (countOrderBy == 1)
                            return true;

                        countOrderBy++;
                    }
                    else
                        return true;
                }
            }
        }

        return false;
    };

    ctrl.addItem = function (item) {
        this.buttons.delete.style.display = '';
        var inputValue = this.inputText.value;
        var seperatePos = inputValue.lastIndexOf(':');
        var prefix = '', displayValue = '', value = '';

        var prefix = (this.editingItem ? this.editingItem.getAttribute('data-display-name') : '');
        if (inputValue) {
            prefix = inputValue.substr(0, seperatePos);
        }
        else {
            var editItem = this.editingItem && this.editingItem.editingChildItem ? this.editingItem.editingChildItem : this.editingItem;
            prefix = editItem ? editItem.getAttribute('data-display-name') : '';
        }

        //is select list
        if (item) {
            if (this.editingItem && this.editingItem.editingChildItem) {
                value = this.editingItem.fakeDisplayMultipleSelection.getAttribute('value');
                displayValue = this.editingItem.fakeDisplayMultipleSelection.displayValue.textContent;
            }
            else {
                value = (this.currentParentItem && this.currentParentItem.fakeDisplayMultipleSelection)
                    ? this.currentParentItem.fakeDisplayMultipleSelection.getAttribute('value')
                    : '';
                displayValue = (this.currentParentItem && this.currentParentItem.fakeDisplayMultipleSelection)
                    ? this.currentParentItem.fakeDisplayMultipleSelection.displayValue.textContent
                    : '';
            }
        }
        else {
            if (item != null) {
                displayValue = item.getAttribute('data-display-value');
                if (item.getAttribute('value') != null) {
                    value = item.getAttribute('value');
                }
            }
            else {
                displayValue = value = aperia.searchControl.validate.correctInputDate(inputValue.substring(seperatePos + 1));
                if (this.suggestValueItem != null && displayValue.toLowerCase() == this.suggestValueItem.getAttribute('data-display-value').toLowerCase()
                    && this.suggestValueItem.getAttribute('value') != null) {
                    value = this.suggestValueItem.getAttribute('value');
                }
            }
        }

        //Check empty search criteria
        if (value.trim().length == 0) {
            if (this.editingItem && this.editingItem.input && this.editingItem.input.type == 'text') {
                this.editingItem = null;
            }
            return;
        }

        var items = this.fieldList;
        var result = false;
        for (var i = 0; i < items.length; i++) {
            var fieldName = items[i].getAttribute('data-display-value');
            var isParent = items[i].getAttribute('data-is-parent');
            if (fieldName.toLowerCase() == prefix.toLowerCase() && (!this.distinct || !this.isFieldSelected(items[i]) || this.editingItem)) {
                var valFunc = items[i].getAttribute('valFunc');
                if (valFunc != null) {
                    valFunc = eval(valFunc + '(value,items[i])');
                }
                //validate date
                if (this.currentInput != null && this.currentInput.type == 'datepicker') {
                    if (valFunc == null && this.currentInput.selectedDates != null && this.currentInput.mode != aperia.datePickerWidget.MODE_DATERANGE) {
                        value = this.currentInput.selectedDates.display('MM/dd/yyyy');
                        if (displayValue.startsWith('before')) {
                            value = 'before ' + value;
                        }
                        else if (displayValue.startsWith('after')) {
                            value = 'after ' + value;
                        }
                        else if (this.currentInput.mode == aperia.datePickerWidget.MODE_MULTIPLE) {
                            if (this.currentInput.selectedDates.length >= 2) {
                                displayValue = 'on ' + this.currentInput.selectedDates[0].display('MM/dd/yyyy') + ' ...';
                            }
                        }
                    }
                }
                if (valFunc == null) {
                    if (this.editingItem && (!this.editingItem.input || this.editingItem.input.type != 'text')) {
                        //hide date picker on IE
                        if (this.editingItem.input && this.editingItem.input.type == 'datepicker' && this.editingItem.input.hide) {
                            this.editingItem.input.hide(true);
                        }
                        //editing child item
                        if (this.editingItem.getAttribute('item-search-name') != items[i].getAttribute('keyword')
                            && this.editingItem.getAttribute('item-search-name') == items[i].getAttribute('data-parent-key')
                            && !this.editingItem.editingChildItem) {
                            this.editingItem.editingChildItem = this.editingItem.querySelector('div[item-search-name="' + items[i].getAttribute('keyword') + '"]');
                        }

                        if (this.editingItem.editingChildItem) {
                            if (this.editingItem.editingChildItem.input && this.editingItem.editingChildItem.input.type == 'datepicker') {
                                aperia.searchControl.datePickerWidget.currentEdit = null;
                                if (aperia.searchControl.datePickerWidget.mode == aperia.datePickerWidget.MODE_DATERANGE || aperia.searchControl.datePickerWidget.mode == aperia.datePickerWidget.MODE_SINGLE) {
                                    this.editingItem.editingChildItem.innerHTML = '';
                                    ctrl.addItemEml(this.editingItem.editingChildItem, items[i].attributes['keyword'].value, fieldName, displayValue);
                                }
                            } else {
                                var displayValueLink = this.editingItem.editingChildItem.querySelector('a[class="display-value"]');
                                displayValueLink.textContent = displayValue;
                            }

                            this.editingItem.editingChildItem.setAttribute('data-value', value);
                            this.editingItem.editingChildItem.setAttribute('data-display-value', displayValue);

                            //reset
                            this.editingItem.resetEditing();
                        }
                        else {
                            //edit item
                            if (this.editingItem.input && this.editingItem.input.type == 'datepicker') {
                                aperia.searchControl.datePickerWidget.currentEdit = null;
                                if (aperia.searchControl.datePickerWidget.mode == aperia.datePickerWidget.MODE_DATERANGE || aperia.searchControl.datePickerWidget.mode == aperia.datePickerWidget.MODE_SINGLE) {
                                    this.editingItem.innerHTML = '';
                                    this.editingItem.setAttribute('data-display-value', displayValue);
                                    ctrl.addItemEml(this.editingItem, items[i].attributes['keyword'].value, fieldName, displayValue);
                                    result = true;
                                    break;
                                }
                            }
                            var displayValueLink = this.editingItem.querySelector('a[class="display-value"]');
                            var editInputValue = this.editingItem.querySelector('input[type="hidden"]');

                            displayValueLink.textContent = displayValue;
                            this.editingItem.setAttribute('data-value', value);
                            this.editingItem.setAttribute('data-display-value', displayValue);

                            var inputArr = editInputValue.value && editInputValue.value.lastIndexOf(':') > -1 ? editInputValue.value.split(':') : [];
                            if (inputArr && inputArr.length > 0) {
                                if (inputArr[0] == fieldName) {
                                    if (inputArr.length > 1) {
                                        inputArr[1] = value;
                                    } else {
                                        inputArr.push(value);
                                    }
                                    editInputValue.value = inputArr.join(':');
                                }
                            }
                            else {
                                editInputValue.value = fieldName + ':' + value
                            }
                        }
                        result = true;
                        break;

                    }
                    else {
                        var div = document.createElement('div');
                        if (this.isAddSubItem && this.currentParentItem) {
                            div.className = 'child-item';
                            div.setAttribute('item-search-name', items[i].attributes['keyword'].value);
                            div.setAttribute('data-value', value);
                            div.setAttribute('data-display-Name', fieldName);
                            div.setAttribute('data-display-value', displayValue);
                            div.setAttribute('data-is-parent', isParent);

                            ctrl.addItemEml(div, items[i].attributes['keyword'].value, fieldName, displayValue);

                            div.root = this.currentParentItem;

                            this.currentParentItem.childItemsWraper.appendChild(div);
                            this.currentParentItem.removeInputField();
                            this.currentParentItem.toggleHiddenInputField();
                            this.currentParentItem.collapseLink.style.display = '';
                            this.currentParentItem.collapseLink.style.visibility = '';
                            this.currentParentItem.toggleHasSubClass();
                        }
                        else {
                            div.className = 'item parent-item';
                            div.setAttribute('item-search-name', items[i].attributes['keyword'].value);
                            div.setAttribute('data-value', value);
                            div.setAttribute('data-display-Name', fieldName);
                            div.setAttribute('data-display-value', displayValue);
                            div.setAttribute('data-is-parent', isParent);

                            ctrl.addItemEml(div, items[i].attributes['keyword'].value, fieldName, displayValue);


                            if (isParent == 'True') {
                                this.subFilter(div);
                            }
                            div.root = this;

                            $(this.displayContainer).find('#displayList').append(div);
                        }
                        result = true;
                        break;
                    }
                }
                else {
                    result = true;
                    break;
                }
            }
        }

        if (result) {//item was added
            this.reset();
            //this.updateInputTextSize();
            this.inputText.forceFocus();
            ctrl.updateValueContainerHeight();
        }
        this.events.addItemCallback();

        return result;
    };

    ctrl.addSubItem = function (item) {
        var currentInput = this.editingItem ? this.editingItem : this.currentInput;
        var div = document.createElement('div');
        div.className = 'item parent-item';
        div.setAttribute('item-search-name', currentInput.keyword);
        div.setAttribute('data-value', currentInput.getAttribute('data-value'));
        div.setAttribute('data-display-name', currentInput.getAttribute('data-parent-name'));
        div.setAttribute('data-display-value', currentInput.keyName);
        div.setAttribute('data-is-parent', true);

        ctrl.addItemEml(div, currentInput.keyword, currentInput.getAttribute('data-parent-name'), currentInput.keyName);

        item.root.subFilter(div);

        div.root = item.root;
        div.className += ' has-sub';
        div.childItemsWraper.root = div;

        var itemSubSortByKey = [];
        // Find item default
        $(currentInput.list).each(function () {
            var itemSub = [];
            if (this.getAttribute('data-is-default') == 'True') {
                itemSub.Key = this.getAttribute('keyword');
                itemSub.DisplayName = this.getAttribute('data-display-value');
                itemSub.IsParent = false;
                itemSub.ParentKey = currentInput.keyword;
                itemSub.InputTypeValue = this.getAttribute('data-input-type');
                itemSub.Value = this.getAttribute('data-default-value');
                itemSub.DisplayValue = this.getAttribute('data-default-display-value');;

                if (this.getAttribute('data-input-type') == 'DatePicker') {
                    itemSub.Value = "Daily-" + this.getAttribute('data-default-value');
                    itemSub.DisplayValue = "Daily-" + this.getAttribute('data-default-value');;
                }
                itemSubSortByKey.push(itemSub);

            }
        });
        itemSubSortByKey = itemSubSortByKey.sort(function (a, b) {
            var ax = a.Key.toLowerCase();
            var bx = b.Key.toLowerCase();

            if (ax < bx) return -1;
            if (ax > bx) return 1;
            return 0;
        });

        $(itemSubSortByKey).each(function () {
            ctrl.addChildSubItem(div.childItemsWraper, this);
        })

        div.collapseLink.style.display = '';
        div.collapseLink.style.visibility = '';
        div.addSubLink.style.display = 'none';
        div.addSubLink.style.visibility = 'hidden';

        $(item.root.displayContainer).find('#displayList').append(div);
        $('#txt-filter-item').addClass("hide");
        $('#displayList').addClass("remove-border");
        ctrl.showAvaiableDateFilter(null);
        this.reset();
        this.events.addItemCallback();
        return true;
    };

    ctrl.addChildSubItem = function (parent, item) {
        var div = document.createElement('div');
        div.className = 'child-item';
        div.setAttribute('item-search-name', item.Key);
        div.setAttribute('data-value', item.Value);
        div.setAttribute('data-display-Name', item.DisplayName);
        div.setAttribute('data-display-value', item.DisplayValue);
        div.setAttribute('data-display-value-real', item.DisplayValueReal);
        div.setAttribute('data-display-autocomplete', item.DisplayAutoComplete);
        div.setAttribute('data-is-parent', item.IsParent ? 'True' : 'False');
        div.setAttribute('data-parent-key', item.ParentKey);

        var displayNameLbl = document.createElement('label');
        displayNameLbl.className = 'display-name';
        displayNameLbl.textContent = item.DisplayName + ':';
        div.appendChild(displayNameLbl);
        switch (item.InputTypeValue) {
            case "DatePicker":
                {
                    var data = item.Value.split('-');
                    var dataDisplay = data[0] == 'Daily' ? lbDaily_DateFilter : lbDateRange_DateFilter;

                    var displayLabelType = document.createElement('label');
                    displayLabelType.className = 'type-value';
                    displayLabelType.setAttribute("type-value", "true")
                    displayLabelType.textContent = dataDisplay;
                    div.appendChild(displayLabelType);

                    var displayFromValueLink = document.createElement('a');
                    displayFromValueLink.className = 'display-value between-value';
                    displayFromValueLink.setAttribute("first-value", "true")
                    displayFromValueLink.textContent = data[1];
                    div.appendChild(displayFromValueLink);
                    displayFromValueLink.onclick = function () {
                        ctrl.editItem(div, this);
                    };
                    if (data[0] == 'DateRange') {
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
                            ctrl.editItem(div, this);
                        };
                    }

                    ctrl.showAvaiableDateFilter(data[1]);
                    break;
                }
            case "RangeRadio":
                var data = item.Value.split('-');
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
                    ctrl.editItem(div, this);
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
                        ctrl.editItem(div, this);
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
                    //div.root.editItem(div, this);
                    ctrl.editItem(div, this);
                };
                break;
            default:
                {
                    var displayValueLink = document.createElement('a');
                    displayValueLink.className = 'display-value'; //TODO: add class
                    displayValueLink.textContent = item.DisplayValue;
                    displayValueLink.onclick = function () {
                        //div.root.editItem(div);
                        ctrl.editItem(div);
                    };

                    div.appendChild(displayValueLink);
                }
        }


        var dateTime = $(parent).find("div[item-search-name=DateTime]").attr("data-value");
        var isDateRange = dateTime != undefined ? dateTime.indexOf("DateRange") >= 0 : false;

        // Not delete default item
        if (!(item.Key == "DateTime" || item.Key == "Export" || item.Key == "CaseType" || (isDateRange && item.Key == "Merchant"))) {
            var iconClose = document.createElement('i');
            iconClose.className = 'icon-close';

            div.appendChild(iconClose);

            iconClose.onclick = function () {
                advancedFilter.currentFilter._ctrl.removeItemNotKeepVal(div);
            };
        }

        div.root = parent.root;
        div.removeButton = iconClose;
        parent.appendChild(div);
    };

    ctrl.addItemEml = function (div, fieldId, fieldName, displayValue, value) {
        if (this.currentInput.getAttribute("is-required-when-date-change") == 'True') {
            var icon = document.createElement('span');
            icon.className = 'required-icon hide';
            div.appendChild(icon);
        }

        var displayNameLbl = document.createElement('label');
        displayNameLbl.className = 'display-name';
        displayNameLbl.textContent = fieldName + ':';


        div.appendChild(displayNameLbl);

        switch (this.currentInput.type) {
            case 'RangeValue':
                var data = displayValue.split('-');

                var displayFromValueLink = document.createElement('a');
                displayFromValueLink.className = 'display-value between-value';
                displayFromValueLink.setAttribute("first-value", "true")
                displayFromValueLink.textContent = data[0];
                div.appendChild(displayFromValueLink);
                displayFromValueLink.onclick = function () {
                    div.root.editItem(div, this);
                };

                displayFromValueLink.after('-')

                var displayToValueLink = document.createElement('a');
                displayToValueLink.className = 'display-value between-value';
                displayToValueLink.setAttribute("first-value", "false")
                displayToValueLink.textContent = data[1];
                div.appendChild(displayToValueLink);
                displayToValueLink.onclick = function () {
                    //div.root.editItem(div, this);
                    ctrl.editItem(div, this);
                };
                break
            case 'RangeRadio':
                var data = value.split('-');
                var dataDisplay = displayValue.split('-');

                var displayLabelType = document.createElement('label');
                displayLabelType.className = 'type-value';
                displayLabelType.setAttribute("type-value", "true")
                displayLabelType.textContent = dataDisplay[0];
                div.appendChild(displayLabelType);

                var displayFromValueLink = document.createElement('a');
                displayFromValueLink.className = 'display-value between-value';
                displayFromValueLink.setAttribute("first-value", "true")
                displayFromValueLink.textContent = data[1];
                div.appendChild(displayFromValueLink);
                displayFromValueLink.onclick = function () {
                    //div.root.editItem(div, this);
                    ctrl.editItem(div, this);
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
                        //div.root.editItem(div, this);
                        ctrl.editItem(div, this);
                    };
                }
                break;
            case 'DatePicker':
                var data = value.split('-');
                var dataDisplay = displayValue.split('-');

                var displayLabelType = document.createElement('label');
                displayLabelType.className = 'type-value';
                displayLabelType.setAttribute("type-value", "true")
                displayLabelType.textContent = dataDisplay[0];
                div.appendChild(displayLabelType);

                var displayFromValueLink = document.createElement('a');
                displayFromValueLink.className = 'display-value between-value';
                displayFromValueLink.setAttribute("first-value", "true")
                displayFromValueLink.textContent = data[1];
                div.appendChild(displayFromValueLink);
                displayFromValueLink.onclick = function () {
                    ctrl.editItem(div, this);
                };

                if (data[0] == 'DateRange') {
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
                        ctrl.editItem(div, this);
                    };
                }
                break;
            case 'OrderBy':
                var dataDisplay = displayValue.split(ctrl.constants.charDisplayValue);

                var displayLabelType = document.createElement('label');
                displayLabelType.className = 'type-value';
                displayLabelType.setAttribute("type-value", "true")
                displayLabelType.textContent = dataDisplay[0] + " ";
                div.appendChild(displayLabelType);

                var displayValueLink = document.createElement('a');
                displayValueLink.className = 'display-value between-value';
                displayValueLink.textContent = dataDisplay[1];
                div.appendChild(displayValueLink);
                displayValueLink.onclick = function () {
                    //div.root.editItem(div, this);
                    ctrl.editItem(div, this);
                };

                break;
            default:
                var displayValueLink = document.createElement('a');
                displayValueLink.className = 'display-value';
                displayValueLink.textContent = displayValue;
                div.appendChild(displayValueLink);
                displayValueLink.onclick = function () {
                    // For case sub filter
                    if (div.getAttribute('data-is-parent') == 'true') {
                        ctrl.inputText.onfocus();
                        div.setAttribute('data-is-parent-open', "true");
                    }
                    else {
                        ctrl.editItem(div);
                    }
                };
                if (this.currentInput.getAttribute("is-required-when-date-change") == 'True') {
                    var selectMerchantLink = document.createElement("a");
                    selectMerchantLink.className = 'select-merchant hide'
                    selectMerchantLink.text = text_UxAdvancedFilter_lbSelectMerchant;
                    div.appendChild(selectMerchantLink);
                    selectMerchantLink.onclick = function (ctr) {
                        div.setAttribute("data-value", "");
                        div.setAttribute("data-display-value", "");
                        div.setAttribute("data-display-autocomplete", "");
                        ctrl.editItem(div);
                        $('#autocompleteID li.search-choice').remove();
                    }
                }
                break
        }

        var iconClose = document.createElement('i');
        iconClose.className = 'icon-close';
        div.appendChild(iconClose);
        iconClose.onclick = function () {
            if (div.getAttribute('data-is-parent') == 'true') {
                $('#txt-filter-item').removeClass("hide");
                $('#displayList').removeClass("remove-border");
                ctrl.showAvaiableDateFilter(null);
            }
            div.root.removeItemNotKeepVal(div);
        };
        div.removeButton = iconClose;

        ctrl.updateDisplayContainerHeight(true);
    };

    ctrl.displayValue = function (displayValue, value, displayReal, displayAutoComplete) {
        this.fakeDisplayMultipleSelection.displayText.textContent = '';
        this.fakeDisplayMultipleSelection.displayValue.textContent = '';
        this.fakeDisplayMultipleSelection.setAttribute('data-value', '');
        this.fakeDisplayMultipleSelection.style.display = 'none';
        this.fakeDisplayMultipleSelection.style.visibility = 'hidden';

        // Clear value input
        this.fakeInputText.value = '';
        this.inputText.style.display = '';
        this.inputText.style.visibility = '';
        this.inputText.value = '';

        var keyword = this.editingItem && this.editingItem.editingChildItem ? this.editingItem.editingChildItem.input.keyword : this.currentInput.keyword;
        var keyName = this.editingItem && this.editingItem.editingChildItem ? this.editingItem.editingChildItem.input.keyName : this.currentInput.keyName;
        var isParent = this.editingItem && this.editingItem.editingChildItem ? this.editingItem.editingChildItem.input.getAttribute('data-is-parent') : this.currentInput.getAttribute('data-is-parent');
        var currentInput = this.editingItem && this.editingItem.editingChildItem ? this.editingItem.editingChildItem.input : this.currentInput;
        var editingItem = this.editingItem && this.editingItem.editingChildItem ? this.editingItem.editingChildItem : this.editingItem;

        if (this.editingItem) {
            switch (currentInput.type) {
                case "RangeValue":
                    var data = displayValue.split('-');

                    var displayFromValueLink = editingItem.querySelector('a[first-value="true"]');
                    var displayToValueLink = editingItem.querySelector('a[first-value="false"]');

                    displayFromValueLink.textContent = data[0];
                    displayToValueLink.textContent = data[1];
                    editingItem.setAttribute('data-value', value);
                    editingItem.setAttribute('data-display-value', displayValue);
                    break;
                case "RangeRadio":
                    var data = value.split('-');
                    var dataDisplay = displayValue.split('-');

                    var displayLabelType = editingItem.querySelector('label[type-value="true"]');
                    var displayCharacter = editingItem.querySelector('label[character="true"]');
                    var displayFromValueLink = editingItem.querySelector('a[first-value="true"]');
                    var displayToValueLink = editingItem.querySelector('a[first-value="false"]');

                    if (data[0] == 'Between' && displayCharacter == null && displayToValueLink == null) {
                        var displayCharacter = document.createElement('label');
                        displayCharacter.setAttribute("character", "true")
                        displayCharacter.textContent = '-';
                        $(displayCharacter).insertAfter(displayFromValueLink);

                        var displayToValueLink = document.createElement('a');
                        displayToValueLink.className = 'display-value between-value';
                        displayToValueLink.setAttribute("first-value", "false")
                        displayToValueLink.textContent = data[2];
                        $(displayToValueLink).insertAfter(displayCharacter);
                        displayToValueLink.onclick = function () {
                            ctrl.editItem(editingItem, this);
                        };
                    }

                    displayLabelType.textContent = dataDisplay[0];
                    if (displayCharacter != null) displayCharacter.textContent = data[0] == 'Between' ? ' - ' : '';
                    if (displayFromValueLink != null) displayFromValueLink.textContent = data[1];
                    if (displayToValueLink != null) displayToValueLink.textContent = data.length > 2 ? data[2] : '';

                    editingItem.setAttribute('data-value', value);
                    editingItem.setAttribute('data-display-value', displayValue);
                    break;
                case "DatePicker":
                    var data = value.split('-');
                    var dataDisplay = displayValue.split('-');

                    var displayLabelType = editingItem.querySelector('label[type-value="true"]');
                    var displayCharacter = editingItem.querySelector('label[character="true"]');
                    var displayFromValueLink = editingItem.querySelector('a[first-value="true"]');
                    var displayToValueLink = editingItem.querySelector('a[first-value="false"]');

                    if (data[0] == 'DateRange' && displayCharacter == null && displayToValueLink == null) {
                        var displayCharacter = document.createElement('label');
                        displayCharacter.setAttribute("character", "true")
                        displayCharacter.textContent = '-';
                        $(displayCharacter).insertAfter(displayFromValueLink);

                        var displayToValueLink = document.createElement('a');
                        displayToValueLink.className = 'display-value between-value';
                        displayToValueLink.setAttribute("first-value", "false")
                        displayToValueLink.textContent = data[2];
                        $(displayToValueLink).insertAfter(displayCharacter);
                        displayToValueLink.onclick = function () {
                            ctrl.editItem(editingItem, this);
                        };
                    }

                    displayLabelType.textContent = dataDisplay[0];
                    if (displayCharacter != null) displayCharacter.textContent = data[0] == 'DateRange' ? ' - ' : '';
                    if (displayFromValueLink != null) displayFromValueLink.textContent = data[1];
                    if (displayToValueLink != null) displayToValueLink.textContent = data.length > 2 ? data[2] : '';

                    editingItem.setAttribute('data-value', value);
                    editingItem.setAttribute('data-display-value', displayValue);
                    break;
                case "OrderBy":
                    var dataDisplay = displayValue.split(ctrl.constants.charDisplayValue);
                    var displayLabelType = editingItem.querySelector('label[type-value="true"]');
                    var displayValueLink = editingItem.querySelector('a.display-value');

                    displayLabelType.textContent = dataDisplay[0];
                    displayValueLink.textContent = dataDisplay[1];

                    editingItem.setAttribute('data-value', value);
                    editingItem.setAttribute('data-display-value', displayValue);
                    break;
                default:
                    var editingItem = this.editingItem.editingChildItem ? this.editingItem.editingChildItem : this.editingItem;
                    var displayValueLink = editingItem.querySelector('a[class="display-value"]');
                    displayValueLink.textContent = displayValue;

                    editingItem.setAttribute('data-value', value);
                    editingItem.setAttribute('data-display-value', displayValue);
                    editingItem.setAttribute('data-display-value-real', displayReal);
                    editingItem.setAttribute('data-display-autocomplete', displayAutoComplete);
                    break;
            }
            result = true;
        }
        else {
            if (this.currentAddingMultiSelectItem) {
                var displayValueLink = this.currentAddingMultiSelectItem.querySelector('a[class="display-value"]');
                displayValueLink.textContent = displayValue;
                this.currentAddingMultiSelectItem.setAttribute('data-value', value);
                this.currentAddingMultiSelectItem.setAttribute('data-display-value', displayValue);
                this.currentAddingMultiSelectItem.setAttribute('data-display-value-real', displayReal);
                this.currentAddingMultiSelectItem.setAttribute('data-display-autocomplete', displayAutoComplete);
            }
            else {
                // Add new
                var div = document.createElement('div');
                if (this.currentParentItem) {
                    if (this.currentParentItem.currentAddingMultiSelectItem) {
                        var displayValueLink = this.currentParentItem.currentAddingMultiSelectItem.querySelector('a[class="display-value"]');
                        displayValueLink.textContent = displayValue;
                        this.currentParentItem.currentAddingMultiSelectItem.setAttribute('data-value', value);
                        this.currentParentItem.currentAddingMultiSelectItem.setAttribute('data-display-value', displayValue);
                        this.currentParentItem.currentAddingMultiSelectItem.setAttribute('data-display-value-real', displayReal);
                        this.currentParentItem.currentAddingMultiSelectItem.setAttribute('data-display-autocomplete', displayAutoComplete);
                    }
                    else {
                        div.className = 'child-item';
                        div.setAttribute('item-search-name', keyword);
                        div.setAttribute('data-value', value);
                        div.setAttribute('data-display-name', keyName);
                        div.setAttribute('data-display-value', displayValue);
                        div.setAttribute('data-display-value-real', displayReal);
                        div.setAttribute('data-display-autocomplete', displayAutoComplete);
                        div.setAttribute('data-is-parent', isParent);
                        div.setAttribute('data-parent-key', this.currentParentItem.getAttribute('item-search-name'));

                        ctrl.addItemEml(div, keyword, keyName, displayValue, value);

                        div.root = this.currentParentItem;

                        this.currentParentItem.childItemsWraper.appendChild(div);
                        this.currentParentItem.removeInputField();
                        this.currentParentItem.toggleHiddenInputField();
                        this.currentParentItem.collapseLink.style.display = '';
                        this.currentParentItem.collapseLink.style.visibility = '';
                        this.currentParentItem.toggleHasSubClass();
                    }
                    result = true;
                }
                else {
                    div.className = 'item parent-item';
                    div.setAttribute('item-search-name', keyword);
                    div.setAttribute('data-value', value);
                    div.setAttribute('data-display-name', keyName);
                    div.setAttribute('data-display-value', displayValue);
                    div.setAttribute('data-display-value-real', displayReal);
                    div.setAttribute('data-display-autocomplete', displayAutoComplete);
                    div.setAttribute('data-is-parent', isParent);

                    ctrl.addItemEml(div, keyword, keyName, displayValue, value);

                    div.root = this;

                    $(this.displayContainer).find('#displayList').append(div);

                    if (currentInput.type == "MultiChoose" || currentInput.type == "MultiSelection" || currentInput.type == "AutoComplete") {
                        this.currentAddingMultiSelectItem = div;
                    }
                    result = true;
                }
            }
        }

        if (result) {
            if (!((this.currentAddingMultiSelectItem || (this.editingItem && this.editingItem.editingChildItem)) && (currentInput.type == "MultiChoose" || currentInput.type == "MultiSelection" || currentInput.type == "AutoComplete"))) {
                this.reset();
            }
            ctrl.updateValueContainerHeight();
            this.events.addItemCallback(true);
        }
    };

    ctrl.currentAddingMultiSelectItem = null;

    ctrl.addMultiSelectItem = function (displayValue, value, displayValueReal) {
        this.fakeDisplayMultipleSelection.displayText.textContent = '';
        this.fakeDisplayMultipleSelection.displayValue.textContent = '';
        this.fakeDisplayMultipleSelection.setAttribute('data-value', '');
        this.fakeDisplayMultipleSelection.style.display = 'none';
        this.fakeDisplayMultipleSelection.style.visibility = 'hidden';

        var inputValue = this.inputText.value;
        // Clear value input
        this.fakeInputText.value = '';
        this.inputText.style.display = '';
        this.inputText.style.visibility = '';
        this.inputText.value = '';

        var seperatePos = inputValue.lastIndexOf(':');
        var prefix = '';

        var prefix = (this.editingItem ? this.editingItem.getAttribute('data-display-name') : '');
        if (inputValue) {
            prefix = inputValue.substr(0, seperatePos);
        }

        var items = this.fieldList;
        var result = false;
        var keyword = this.currentInput.keyword;
        var keyName = this.currentInput.keyName;

        var div = document.createElement('div');
        div.className = 'item parent-item';
        div.setAttribute('item-search-name', keyword);
        div.setAttribute('data-value', value);
        div.setAttribute('data-display-Name', keyName);
        div.setAttribute('data-display-value', displayValue);
        div.setAttribute('data-display-value-real', displayValueReal);
        //div.setAttribute('data-is-parent', isParent);
        var iconClose = document.createElement('i');
        iconClose.className = 'icon-close';
        div.innerHTML = '<label>' + keyName + ':</label>';
        var displayValueLink = document.createElement('a');
        displayValueLink.className = 'display-value'; //TODO: add class
        displayValueLink.textContent = displayValue;

        div.appendChild(displayValueLink);

        div.appendChild(iconClose);
        //var input = document.createElement('input');
        //input.type = 'hidden';
        //input.value = keyName + ':' + value;
        //input.name = 'searchbaritem_' + keyword;
        //input.setAttribute('data-display-name', keyName);
        div.root = this;

        displayValueLink.onclick = function () {
            //div.root.removeItem(div);
            div.root.editItem(div);
        };

        iconClose.onclick = function () {
            div.root.removeItemNotKeepVal(div);
        };

        div.removeButton = iconClose;

        //div.appendChild(input);
        $(this.displayContainer).find('#displayList').append(div);

        this.currentAddingMultiSelectItem = div;

        this.events.addItemCallback();
        this.calculateContainerHeight();
        this.updateDisplayContainerHeight(true);
    };

    ctrl.getFilterItem = function (keyWord, parentKey) {
        for (var i = 0; i < ctrl.fieldList.length; i++) {
            if (ctrl.fieldList[i].getAttribute("KeyWord") === keyWord && ctrl.fieldList[i].getAttribute("data-parent-key") === parentKey) {
                return ctrl.fieldList[i];
            }
        }
    }

    ctrl.addChildItemToDisplaySection = function (filterItem) {
        var div = document.createElement('div');
        var keyName = filterItem.getAttribute("KeyWord");
        var displayName = filterItem.getAttribute('data-display-value');

        div.className = 'child-item';
        div.setAttribute('item-search-name', keyName);
        div.setAttribute('data-value', "");
        div.setAttribute('data-display-name', displayName);
        div.setAttribute('data-display-value', "");
        div.setAttribute('data-display-value-real', "");
        div.setAttribute('data-display-autocomplete', "");
        div.setAttribute('data-is-parent', false);
        div.setAttribute('data-parent-key', filterItem.getAttribute('data-parent-key'));

        var iconRequired = document.createElement('span');
        var selectMerchantLink = document.createElement("a");
        if (filterItem.getAttribute("is-required-when-date-change") == 'True') {
            iconRequired.className = 'required-icon hide';
            div.appendChild(iconRequired);
            selectMerchantLink.className = 'select-merchant hide'
            selectMerchantLink.text = text_UxAdvancedFilter_lbSelectMerchant;
            selectMerchantLink.onclick = function (ctr) {
                div.setAttribute("data-value", "");
                div.setAttribute("data-display-value", "");
                div.setAttribute("data-display-autocomplete", "");
                ctrl.editItem(div);
                $('#autocompleteID li.search-choice').remove();
            }
        }

        var displayText = document.createElement("label");
        displayText.textContent = displayName + ':';
        displayText.className = "display-name"
        div.appendChild(displayText);

        var displayValueLink = document.createElement('a');
        displayValueLink.className = 'display-value'; //TODO: add class
        displayValueLink.textContent = "";

        div.appendChild(displayValueLink);

        div.appendChild(selectMerchantLink);


        div.root = this;

        displayValueLink.onclick = function () {
            div.root.editItem(div);
        };

        $(this.displayContainer).find('#displayList .child-items-wraper').append(div);

        //this.currentAddingMultiSelectItem = div;

        this.events.addItemCallback();

        this.calculateContainerHeight();
        this.updateDisplayContainerHeight();
    }

    ctrl.refreshSuggestFieldList = function () {
        var suggestItems = $(this.fieldContainer).find('.items div');
        this.fieldContainer.listContainer = $(this.fieldContainer).find('.items');
        var keywordCount = suggestItems.length;

        for (var k = 0; k < keywordCount; k++) {
            var i = 0;
            suggestItems[k].root = this;
            if (this.valuesContainer != null) {
                var name = suggestItems[k].getAttribute('values');
                var field_parent_key = suggestItems[k].getAttribute('data-parent-key');
                var isParent = suggestItems[k].getAttribute('data-is-parent') == 'True';
                if (name != null) {
                    var divs = this.valuesContainer.getElementsByTagName('div');
                    for (var ii = 0; ii < divs.length; ii++) {
                        var value_parent_key = divs[ii].getAttribute('data-parent-key');
                        if ((divs[ii].getAttribute('name') === name && field_parent_key == null)
                            || (field_parent_key != null && field_parent_key == value_parent_key && divs[ii].getAttribute('name') === name)) {
                            suggestItems[k].input = divs[ii];
                            divs[ii].type = divs[ii].getAttribute('type');
                            divs[ii].keyword = divs[ii].getAttribute('data-keyword');
                            divs[ii].keyName = divs[ii].getAttribute('data-display-name');
                            //EditLy
                            divs[ii].isGetDataSource = (divs[ii].getAttribute('data-is-get-datasource') || 'False') == 'True';
                            switch (divs[ii].type) {
                                case 'MultiSelection':
                                    {
                                        divs[ii].url = divs[ii].getAttribute('url');
                                        divs[ii].root = this;
                                        divs[ii].multiSelect = true;
                                        divs[ii].refreshSuggestValueList = function () {
                                            this.listContainer = this.querySelector('.items');
                                            var list = this.listContainer.querySelectorAll('div[value]');

                                            this.checkBoxes = this.listContainer.querySelectorAll('input[type="checkbox"]');
                                            this.selectAllLink = this.querySelector('a[class*="select-all"]');
                                            this.selectAllLink.style.display = (list.length == 0) ? "none" : "block";

                                            this.changeLinkContent = function () {
                                                var checkboxes = this.checkBoxes;
                                                var isSelectAll = true;
                                                var atLeastOneSelected = false;
                                                for (var index = 0; index < checkboxes.length; index++) {
                                                    if (checkboxes[index].checked == false) {
                                                        isSelectAll = false;
                                                    } else {
                                                        atLeastOneSelected = true;
                                                    }
                                                }

                                                this.selectAllLink.textContent = isSelectAll ? msg_UxAdvancedFilter_LinkDeselectAll : msg_UxAdvancedFilter_LinkSelectAll;
                                            };

                                            this.updateSelectionValue = function (checkbox, isAddItem) {

                                                if (checkbox.value == "" || checkbox.value == null) return;

                                                var isChecked = checkbox.checked;
                                                var value = (this.root.currentParentItem && this.root.currentParentItem.fakeDisplayMultipleSelection)
                                                    ? this.root.currentParentItem.fakeDisplayMultipleSelection.getAttribute('data-value')
                                                    : (this.root.currentAddingMultiSelectItem ? this.root.currentAddingMultiSelectItem.getAttribute('data-value') : '');
                                                var displayVal = (this.root.currentParentItem && this.root.currentParentItem.fakeDisplayMultipleSelection)
                                                    ? this.root.currentParentItem.fakeDisplayMultipleSelection.displayValue.textContent
                                                    : (this.root.currentAddingMultiSelectItem ? this.root.currentAddingMultiSelectItem.getAttribute('data-display-value') : '');
                                                var displayValReal = (this.root.currentParentItem && this.root.currentParentItem.fakeDisplayMultipleSelection)
                                                    ? this.root.currentParentItem.fakeDisplayMultipleSelection.getAttribute('data-display-value-real')
                                                    : (this.root.currentAddingMultiSelectItem ? this.root.currentAddingMultiSelectItem.getAttribute('data-display-value-real') : '');

                                                if (this.root.editingItem) {
                                                    value = this.root.editingItem.editingChildItem ? this.root.editingItem.editingChildItem.getAttribute('data-value') : this.root.editingItem.getAttribute('data-value');
                                                    displayVal = this.root.editingItem.editingChildItem ? this.root.editingItem.editingChildItem.getAttribute('data-display-value') : this.root.editingItem.getAttribute('data-display-value');
                                                    displayValReal = this.root.editingItem.editingChildItem ? this.root.editingItem.editingChildItem.getAttribute('data-display-value-real') : this.root.editingItem.getAttribute('data-display-value-real');
                                                }

                                                var valueArr = value ? value.split(ctrl.constants.charDataValue) : [];
                                                var displayValArr = displayValReal ? displayValReal.split(ctrl.constants.charDisplayValue) : [];

                                                if (isChecked) {
                                                    if (valueArr.indexOf(checkbox.value) < 0) {
                                                        // Fix bug value include character '
                                                        var display = checkbox.getAttribute('display-value');
                                                        display = ctrl.replaceSpecialChar(display);
                                                        displayValArr.push(display);
                                                        valueArr.push(checkbox.value);
                                                    }
                                                }
                                                else {
                                                    var removedValIndex = valueArr.indexOf(checkbox.value);
                                                    if (removedValIndex > -1) {
                                                        displayValArr.splice(removedValIndex, 1);
                                                        valueArr.splice(removedValIndex, 1);
                                                    }
                                                }

                                                var dataValue = valueArr.length > 0 ? valueArr.join(ctrl.constants.charDataValue) : '';
                                                var dataDisplayValue = displayValArr.length > 0 ? displayValArr.join(', ') : '';
                                                var dataDisplayReal = displayValArr.length > 0 ? displayValArr.join(ctrl.constants.charDisplayValue) : '';

                                                if (ctrl.editingItem) {
                                                    var displayValueLink = ctrl.editingItem.editingChildItem ? ctrl.editingItem.editingChildItem.querySelector('a[class="display-value"]') : ctrl.editingItem.querySelector('a[class="display-value"]');
                                                    displayValueLink.textContent = dataDisplayValue;

                                                    ctrl.editingItem.setAttribute('data-value', dataValue);
                                                    ctrl.editingItem.setAttribute('data-display-value', dataDisplayValue);
                                                    ctrl.editingItem.setAttribute('data-display-value-real', dataDisplayReal);
                                                }
                                                else {
                                                    if (ctrl.currentParentItem && ctrl.currentParentItem.fakeDisplayMultipleSelection) {
                                                        ctrl.currentParentItem.fakeDisplayMultipleSelection.displayValue.textContent = dataDisplayValue;
                                                        ctrl.currentParentItem.fakeDisplayMultipleSelection.setAttribute('data-value', dataValue);
                                                        ctrl.currentParentItem.fakeDisplayMultipleSelection.setAttribute('data-display-value', dataDisplayValue);
                                                        ctrl.currentParentItem.fakeDisplayMultipleSelection.setAttribute('data-display-value-real', dataDisplayReal);
                                                    }
                                                    else {
                                                        ctrl.displayValue(dataDisplayValue, dataValue, dataDisplayReal);
                                                    }
                                                }

                                                ctrl.events.addItemCallback(true);
                                                ctrl.updateValueContainerHeight();
                                            }

                                            this.resetValue = function () {
                                                var checkboxes = this.checkBoxes;
                                                for (var index = 0; index < checkboxes.length; index++) {
                                                    checkboxes[index].checked = false;
                                                }
                                                this.selectAllLink.textContent = msg_UxAdvancedFilter_LinkSelectAll;
                                            };

                                            if (this.selectAllLink) {
                                                this.selectAllLink.parent = this;
                                                this.selectAllLink.onclick = function () {
                                                    var checkboxes = this.parent.checkBoxes;
                                                    var isSelectAll = this.textContent == msg_UxAdvancedFilter_LinkSelectAll;
                                                    for (var index = 0; index < checkboxes.length; index++) {
                                                        checkboxes[index].checked = isSelectAll;
                                                        this.parent.updateSelectionValue(checkboxes[index]);
                                                    }

                                                    this.textContent = isSelectAll ? msg_UxAdvancedFilter_LinkDeselectAll : msg_UxAdvancedFilter_LinkSelectAll;
                                                };
                                            }

                                            ctrl.loadActionButton(this);

                                            //bind onchange event
                                            for (var index = 0; index < this.checkBoxes.length; index++) {
                                                this.checkBoxes[index].parent = this;
                                                this.checkBoxes[index].root = this.root;
                                                this.checkBoxes[index].onchange = function () {
                                                    this.parent.updateSelectionValue(this);
                                                    this.parent.changeLinkContent();
                                                };
                                            }

                                            this.changeLinkContent();

                                            this.valueList = list;
                                        };
                                        if (divs[ii].listContainer == null) divs[ii].refreshSuggestValueList();
                                    }
                                    break;
                                case 'RangeRadio':
                                    divs[ii].root = this;
                                    divs[ii].refreshSuggestValueList = function () {
                                        this.listContainer = this.querySelector('.items');
                                        var list = this.listContainer.querySelectorAll('div[value]');

                                        this.radios = this.listContainer.querySelectorAll('input[type="radio"]');
                                        var fromTextbox = this.querySelector('input[data-from-value="true"]');
                                        var toTextbox = this.querySelector('input[data-to-value="true"]');
                                        var textbox = this.querySelector('input[data-value="true"]');
                                        this.divFromTxt = this.querySelector('div[data-from-to-textbox="true"]');
                                        this.divTxt = this.querySelector('div[data-textbox="true"]');
                                        this.validateMsg = this.querySelector('div[data-validation-msg="true"]');

                                        this.changeContent = function (isBetween) {
                                            var divFromTxt = this.divFromTxt;
                                            var divTxt = this.divTxt;

                                            if (isBetween) {
                                                $(divFromTxt).removeClass('hide');
                                                var inputRangeItems = $(divFromTxt).find('input[type="text"]');
                                                inputRangeItems.each(function () {
                                                    $(this).prop('disabled', false);
                                                })
                                                $(divTxt).addClass('hide');
                                                var inputItems = $(divTxt).find('input[type="text"]');
                                                inputItems.each(function () {
                                                    $(this).prop('disabled', true);
                                                })
                                                this.textbox.value = '';
                                            }
                                            else {
                                                $(divFromTxt).addClass('hide');
                                                var inputRangeItems = $(divFromTxt).find('input[type="text"]');
                                                inputRangeItems.each(function () {
                                                    $(this).prop('disabled', true);
                                                })
                                                $(divTxt).removeClass('hide');
                                                var inputItems = $(divTxt).find('input[type="text"]');
                                                inputItems.each(function () {
                                                    $(this).prop('disabled', false);
                                                })

                                                this.fromTextbox.value = '';
                                                this.toTextbox.value = '';
                                                this.textbox.value = '';
                                            }

                                            ctrl.showHideValidation(false, this);
                                        };

                                        this.updateSelectionValue = function () {
                                            var isValid = ctrl.validationControl();
                                            if (isValid) {
                                                var displayValue = '', value = '';
                                                var currentInput = this.root.currentInput;

                                                var keyword = currentInput.keyword;
                                                var keyName = currentInput.keyName;

                                                var fromItem = currentInput.fromTextbox.value;
                                                var toItem = currentInput.toTextbox.value;
                                                var textbox = currentInput.textbox.value;

                                                var radios = currentInput.radios;
                                                var isBetween = true;
                                                for (var i = 0; i < radios.length; i++) {
                                                    if (radios[i].checked) {
                                                        switch (radios[i].value) {
                                                            case "Between":
                                                                displayValue = radios[i].getAttribute('display-value') + ctrl.constants.charDataBetween + fromItem + ctrl.constants.charDataBetween + toItem;
                                                                value = "Between" + ctrl.constants.charDataBetween + fromItem + ctrl.constants.charDataBetween + toItem;
                                                                isBetween = true;
                                                                break;
                                                            case "GreaterThan":
                                                                displayValue = radios[i].getAttribute('display-value') + ctrl.constants.charDataBetween + textbox;
                                                                value = "GreaterThan" + ctrl.constants.charDataBetween + textbox;
                                                                isBetween = false;
                                                                break;
                                                            case "LessThan":
                                                                displayValue = radios[i].getAttribute('display-value') + ctrl.constants.charDataBetween + textbox;
                                                                value = "LessThan" + ctrl.constants.charDataBetween + textbox;
                                                                isBetween = false;
                                                                break;
                                                        }
                                                    }
                                                }

                                                ctrl.displayValue(displayValue, value);
                                            }
                                        }

                                        this.resetValue = function () {
                                            var radios = this.radios;
                                            var divFromTxt = this.divFromTxt;
                                            var divTxt = this.divTxt;

                                            for (var index = 0; index < radios.length; index++) {
                                                if (radios[index].value == 'Between') {
                                                    radios[index].checked = true;
                                                    $(divFromTxt).removeClass('hide');
                                                    var inputRangeItems = $(divFromTxt).find('input[type="text"]');
                                                    inputRangeItems.each(function () {
                                                        $(this).prop('disabled', false);
                                                    })
                                                    $(divTxt).addClass('hide');
                                                    var inputItems = $(divTxt).find('input[type="text"]');
                                                    inputItems.each(function () {
                                                        $(this).prop('disabled', true);
                                                    })
                                                }
                                                else {
                                                    radios[index].checked = false;
                                                }
                                            }

                                            this.fromTextbox.value = '';
                                            this.toTextbox.value = '';
                                            this.textbox.value = '';
                                            ctrl.showHideValidation(false, this);
                                        };

                                        ctrl.loadActionButton(this);

                                        //bind onchange event
                                        for (var index = 0; index < this.radios.length; index++) {
                                            this.radios[index].parent = this;
                                            this.radios[index].root = this.root;
                                            this.radios[index].onchange = function () {
                                                var isBetween = this.value == 'Between';
                                                this.parent.changeContent(isBetween);
                                            };
                                        }

                                        if (fromTextbox) {
                                            this.fromTextbox = fromTextbox;
                                            fromTextbox.parent = this;
                                            fromTextbox.root = this.root;
                                            fromTextbox.onkeydown = function (evt) {
                                                if (evt.keyCode == 13) {
                                                    this.parent.updateSelectionValue();
                                                }
                                            };
                                        }

                                        if (toTextbox) {
                                            this.toTextbox = toTextbox;
                                            toTextbox.parent = this;
                                            toTextbox.root = this.root;
                                            toTextbox.onkeydown = function (evt) {
                                                if (evt.keyCode == 13) {
                                                    this.parent.updateSelectionValue();
                                                }
                                            };
                                        }

                                        if (textbox) {
                                            this.textbox = textbox;
                                            textbox.parent = this;
                                            textbox.root = this.root;
                                            textbox.onkeydown = function (evt) {
                                                if (evt.keyCode == 13) {
                                                    this.parent.updateSelectionValue(list);
                                                }
                                            };
                                        }
                                    };

                                    if (divs[ii].listContainer == null) divs[ii].refreshSuggestValueList();
                                    break;
                                case 'SingleSelection':
                                    divs[ii].root = this;
                                    divs[ii].refreshSuggestValueList = function () {
                                        this.listContainer = this.querySelector('.items');
                                        this.list = this.listContainer.querySelectorAll('div[value]');

                                        this.updateSelectionValue = function (obj) {
                                            var listItem = this.root.currentInput.list;
                                            for (var i = 0; i < listItem.length; i++) {
                                                $(listItem[i]).removeClass('selected');
                                                if (listItem[i] === obj) {
                                                    $(listItem[i]).addClass('selected');
                                                }
                                            }

                                            var displayValue = obj.getAttribute('data-display-value');
                                            displayValue = ctrl.replaceSpecialChar(displayValue);
                                            var value = obj.getAttribute('value');

                                            ctrl.displayValue(displayValue, value);

                                        };

                                        this.resetValue = function () {
                                            var listItem = this.list;
                                            for (var i = 0; i < listItem.length; i++) {
                                                $(listItem[i]).removeClass('selected');
                                            }
                                        };

                                        ctrl.loadActionButton(this);

                                        for (var index = 0; index < this.list.length; index++) {
                                            var list = this.list;
                                            list[index].root = this.root;
                                            list[index].parent = this;
                                            list[index].onclick = function (e) {
                                                this.parent.updateSelectionValue(this);
                                            }
                                        }
                                    };

                                    if (divs[ii].listContainer == null) divs[ii].refreshSuggestValueList();
                                    break;
                                case 'Text':
                                    divs[ii].root = this;
                                    divs[ii].refreshSuggestValueList = function () {
                                        this.listContainer = this.querySelector('.items');
                                        var list = this.listContainer.querySelectorAll('div[value]');
                                        var textbox = this.querySelector('input[type="text"]');
                                        this.validateMsg = this.querySelector('div[data-validation-msg="true"]');

                                        this.updateSelectionValue = function () {
                                            var isValid = ctrl.validationControl();

                                            if (isValid) {
                                                var displayValue = '', value = '';

                                                currentInput = this.root.editingItem && this.root.editingItem.editingChildItem ? this.root.editingItem.editingChildItem.input : this.root.currentInput;
                                                var fromItem = currentInput.textbox.value;

                                                displayValue = fromItem;
                                                value = fromItem;

                                                ctrl.displayValue(displayValue, value);
                                            }
                                        }

                                        this.resetValue = function () {
                                            this.textbox.value = '';
                                            ctrl.showHideValidation(false, this);
                                        };

                                        ctrl.loadActionButton(this);

                                        if (textbox) {
                                            this.textbox = textbox;
                                            textbox.parent = this;
                                            textbox.root = this.root;
                                            textbox.onkeydown = function (evt) {
                                                if (evt.keyCode == 13) {
                                                    this.parent.updateSelectionValue();
                                                }
                                            };
                                        }
                                    };

                                    if (divs[ii].listContainer == null) divs[ii].refreshSuggestValueList();
                                    break;
                                case 'MultiChoose':
                                    divs[ii].root = this;
                                    divs[ii].multiSelect = true;
                                    divs[ii].refreshSuggestValueList = function () {
                                        this.listContainer = this.querySelector('.items');
                                        var list = this.listContainer.querySelectorAll('div[value]');

                                        $('#' + this.keyword).chosen({ search_contains: true });

                                        this.multiChoose = $('#' + this.keyword);

                                        this.multiChoose.bind("chosen:showing_dropdown", function (evt) {
                                            ctrl.updateHeightMultiChosen();
                                        });

                                        this.multiChoose.change(function (e, param) {
                                            $(".chosen-container .chosen-drop").css({ "max-height": 0 + "px" })
                                            var isValid = ctrl.validationControl();
                                            if (isValid) {
                                                var value = (ctrl.currentParentItem && ctrl.currentParentItem.fakeDisplayMultipleSelection)
                                                    ? ctrl.currentParentItem.fakeDisplayMultipleSelection.getAttribute('data-value')
                                                    : (ctrl.currentAddingMultiSelectItem ? ctrl.currentAddingMultiSelectItem.getAttribute('data-value') : '');
                                                var displayVal = "";

                                                //Get real display value is selected.
                                                $('#' + this.id + ' option:selected').each(function () {
                                                    displayVal += $(this).attr('display') + ", ";
                                                })

                                                // Remove the end character: ", "
                                                displayVal = displayVal.substring(0, displayVal.length - 2);

                                                if (ctrl.editingItem) {
                                                    value = ctrl.editingItem.getAttribute('data-value');
                                                }

                                                var valueArr = value ? value.split(',') : [];
                                                var displayValArr = displayVal ? displayVal.split(', ') : [];

                                                if (param.selected) {
                                                    if (valueArr.indexOf(param.selected) < 0) {
                                                        valueArr.push(param.selected);
                                                    }
                                                }
                                                else {
                                                    var removedValIndex = valueArr.indexOf(param.deselected);
                                                    if (removedValIndex > -1) {
                                                        displayValArr.splice(removedValIndex, 1);
                                                        valueArr.splice(removedValIndex, 1);
                                                    }
                                                }

                                                var dataValue = valueArr.length > 0 ? valueArr.join(ctrl.constants.charDataValue) : '';
                                                var dataDisplayValue = displayValArr.length > 0 ? displayValArr.join(', ') : '';

                                                if (ctrl.editingItem) {
                                                    var displayValueLink = ctrl.editingItem.editingChildItem ? ctrl.editingItem.editingChildItem.querySelector('a[class="display-value"]') : ctrl.editingItem.querySelector('a[class="display-value"]');
                                                    displayValueLink.textContent = dataDisplayValue;

                                                    ctrl.editingItem.setAttribute('data-value', dataValue);
                                                    ctrl.editingItem.setAttribute('data-display-value', dataDisplayValue);
                                                }
                                                else {
                                                    if (ctrl.currentParentItem && ctrl.currentParentItem.fakeDisplayMultipleSelection) {
                                                        ctrl.currentParentItem.fakeDisplayMultipleSelection.displayValue.textContent = dataDisplayValue;
                                                        ctrl.currentParentItem.fakeDisplayMultipleSelection.setAttribute('data-value', dataValue);
                                                        ctrl.currentParentItem.fakeDisplayMultipleSelection.setAttribute('data-display-value', dataDisplayValue);
                                                    }
                                                    else {
                                                        ctrl.displayValue(dataDisplayValue, dataValue);
                                                    }
                                                }

                                                ctrl.events.addItemCallback(true);
                                                ctrl.updateValueContainerHeight();
                                            }
                                        });

                                        this.resetValue = function () {
                                            ctrl.showHideValidation(false, this);
                                        };

                                        ctrl.loadActionButton(this);
                                    };

                                    if (divs[ii].listContainer == null) divs[ii].refreshSuggestValueList();
                                    break;
                                case 'DatePicker':
                                    divs[ii].root = this;
                                    divs[ii].refreshSuggestValueList = function () {
                                        this.listContainer = this.querySelector('.items');
                                        var list = this.listContainer.querySelectorAll('div[value]');
                                        this.radios = this.listContainer.querySelectorAll('input[type="radio"]');
                                        this.divDaily = this.querySelector('div[data-daily-textbox="true"]');
                                        this.divRange = this.querySelector('div[data-range-textbox="true"]');
                                        var dailyTextbox = this.querySelector('input[data-daily-date-value="true"]');
                                        var fromTextbox = this.querySelector('input[data-from-date-value="true"]');
                                        var toTextbox = this.querySelector('input[data-to-date-value="true"]');
                                        var valMerchantMsg = this.querySelector('div.val-merchant-msg');

                                        this.changeContent = function (isRange) {
                                            var divRange = this.divRange;
                                            var divDaily = this.divDaily;
                                            if (isRange) {
                                                $(divRange).removeClass('hide');
                                                $(divDaily).addClass('hide');

                                                var inputRangeItems = $(divRange).find('input[type="text"]');
                                                inputRangeItems.each(function () {
                                                    $(this).prop('disabled', false);
                                                })

                                                var inputItems = $(divDaily).find('input[type="text"]');
                                                inputItems.each(function () {
                                                    $(this).prop('disabled', true);
                                                })
                                                this.dailyTextbox.value = '';
                                            }
                                            else {
                                                $(divRange).addClass('hide');
                                                $(divDaily).removeClass('hide');

                                                var inputRangeItems = $(divRange).find('input[type="text"]');
                                                inputRangeItems.each(function () {
                                                    $(this).prop('disabled', true);
                                                })

                                                var inputItems = $(divDaily).find('input[type="text"]');
                                                inputItems.each(function () {
                                                    $(this).prop('disabled', false);
                                                })

                                                this.fromTextbox.value = '';
                                                this.toTextbox.value = '';
                                                this.dailyTextbox.value = '';
                                            }
                                            ctrl.showHideValidation(false, this);
                                        }

                                        this.updateSelectionValue = function () {
                                            var isValid = ctrl.validationControl();
                                            if (isValid) {
                                                var displayValue = '', value = '';
                                                var currentInput = this.root.currentInput;

                                                var keyword = currentInput.keyword;
                                                var keyName = currentInput.keyName;

                                                var fromItem = currentInput.fromTextbox.value;
                                                var toItem = currentInput.toTextbox.value;
                                                var textbox = currentInput.dailyTextbox.value;
                                                var valDate = "";

                                                var radios = currentInput.radios;
                                                var isDateRangeMode = false;

                                                //Remove Merchant item
                                                $(".child-items-wraper").find('div[item-search-name="Merchant"]').remove();

                                                for (var i = 0; i < radios.length; i++) {
                                                    if (radios[i].checked) {
                                                        switch (radios[i].value) {
                                                            case "DateRange":
                                                                displayValue = radios[i].getAttribute('display-value') + ctrl.constants.charDataBetween + fromItem + ctrl.constants.charDataBetween + toItem;
                                                                value = "DateRange" + ctrl.constants.charDataBetween + fromItem + ctrl.constants.charDataBetween + toItem;
                                                                isDateRangeMode = true;
                                                                valDate = fromItem;
                                                                break;
                                                            case "Daily":
                                                                displayValue = radios[i].getAttribute('display-value') + ctrl.constants.charDataBetween + textbox;
                                                                value = "Daily" + ctrl.constants.charDataBetween + textbox;
                                                                valDate = textbox;
                                                                break;
                                                        }
                                                    }
                                                }

                                                function addMerchantItem() {
                                                    var parentKey = $(ctrl.displayContainer).find(".parent-item").attr("item-search-name")
                                                    var merchantItem = ctrl.getFilterItem("Merchant", parentKey);
                                                    if (merchantItem) {
                                                        ctrl.addChildItemToDisplaySection(merchantItem);
                                                    }
                                                }

                                                ctrl.displayValue(displayValue, value);

                                                //Daily mode and merchant item doesn't add before
                                                if (ctrl.checkRequiredMerchant()) {
                                                    addMerchantItem()

                                                    $(".select-merchant").removeClass("hide");
                                                    if (isDateRangeMode) {
                                                        $("#btnApplyAdvFilter").attr("disabled", true);
                                                        $("div[item-search-name='Merchant']").find(".display-value").text('');
                                                        $(".required-icon").removeClass("hide");
                                                    }
                                                    else {
                                                        $("#btnApplyAdvFilter").removeAttr("disabled");
                                                        $(".required-icon").addClass("hide");
                                                    }
                                                }

                                                // Check avaiable datetime have data to search.
                                                ctrl.showAvaiableDateFilter(valDate);
                                            }
                                        }

                                        this.resetValue = function () {
                                            var radios = this.radios;
                                            var divFromTxt = this.divRange;
                                            var divTxt = this.divDaily;

                                            for (var index = 0; index < radios.length; index++) {
                                                if (radios[index].value == 'Daily') {
                                                    radios[index].checked = true;
                                                    $(divTxt).removeClass('hide');

                                                    $(divFromTxt).addClass('hide');
                                                    var inputRangeItems = $(divFromTxt).find('input[type="text"]');
                                                    inputRangeItems.each(function () {
                                                        $(this).prop('disabled', true);
                                                    })

                                                    var inputItems = $(divTxt).find('input[type="text"]');
                                                    inputItems.each(function () {
                                                        $(this).prop('disabled', false);
                                                    })
                                                }
                                                else {
                                                    radios[index].checked = false;
                                                }
                                            }

                                            this.fromTextbox.value = '';
                                            this.toTextbox.value = '';
                                            this.dailyTextbox.value = '';
                                            ctrl.showHideValidation(false, this);
                                            $(valMerchantMsg).addClass('hide');
                                        };

                                        ctrl.loadActionButton(this);

                                        // Init datetime picker
                                        $(".date-picker-control").datepicker({
                                            showOn: "button",
                                            buttonImage: rootURL + "res/img/calendar_ico.png",
                                            buttonText: '',
                                            buttonImageOnly: true,
                                            showButtonPaner: true,
                                            dayNamesMin: ["S", "M", "T", "W", "T", "F", "S"],
                                            showOtherMonths: true,
                                            selectOtherMonths: true,
                                            beforeShow: function (dateCtr) {
                                                setTimeout(function () {
                                                    advancedFilter.datePicker.initPreDate($(dateCtr));
                                                    if ($(".ui-datepicker-calendar").find(".ui-state-highlight").length > 0
                                                        && $(".ui-datepicker-calendar").find(".ui-state-active").length > 0) {
                                                        $(".ui-datepicker-calendar").find(".ui-state-highlight").removeClass("ui-state-highlight");
                                                    }
                                                }, 1);
                                            },
                                        });


                                        //bind onchange event
                                        for (var index = 0; index < this.radios.length; index++) {
                                            this.radios[index].parent = this;
                                            this.radios[index].root = this.root;
                                            this.radios[index].onchange = function () {
                                                if ($(".child-items-wraper").find('div[item-search-name="Merchant"]').length == 1) {
                                                    $(valMerchantMsg).removeClass('hide');
                                                }
                                                else {
                                                    $(valMerchantMsg).addClass('hide');
                                                }

                                                var isRange = this.value == 'DateRange';
                                                this.parent.changeContent(isRange);
                                            };
                                        }

                                        if (fromTextbox) {
                                            this.fromTextbox = fromTextbox;
                                            fromTextbox.parent = this;
                                            fromTextbox.root = this.root;
                                            fromTextbox.onkeydown = function (evt) {
                                                if (evt.keyCode == 13) {
                                                    this.parent.updateSelectionValue();
                                                }
                                            };
                                        }

                                        if (toTextbox) {
                                            this.toTextbox = toTextbox;
                                            toTextbox.parent = this;
                                            toTextbox.root = this.root;
                                            toTextbox.onkeydown = function (evt) {
                                                if (evt.keyCode == 13) {
                                                    this.parent.updateSelectionValue();
                                                }
                                            };
                                        }

                                        if (dailyTextbox) {
                                            this.dailyTextbox = dailyTextbox;
                                            dailyTextbox.parent = this;
                                            dailyTextbox.root = this.root;
                                            dailyTextbox.onkeydown = function (evt) {
                                                if (evt.keyCode == 13) {
                                                    this.parent.updateSelectionValue(list);
                                                }
                                            };
                                        }
                                    };

                                    if (divs[ii].listContainer == null) divs[ii].refreshSuggestValueList();
                                    break;
                                case 'SubFilter':
                                    divs[ii].root = this;
                                    divs[ii].refreshSuggestValueList = function () {
                                        this.listContainer = this.querySelector('.items');
                                        this.list = this.listContainer.querySelectorAll('div[value]');
                                        this.validateMsg = this.querySelector('div[data-validation-msg="true"]');

                                        this.updateSelectionValue = function (obj) {
                                            this.root.addItem(obj);
                                        };

                                        this.resetValue = function () {

                                        };

                                        ctrl.loadActionButton(this);

                                        for (var index = 0; index < this.list.length; index++) {
                                            var list = this.list;
                                            list[index].root = this.root;
                                            list[index].parent = this;
                                            list[index].onclick = function (e) {
                                                this.parent.updateSelectionValue(this);
                                            }
                                        }
                                    };

                                    if (divs[ii].listContainer == null) divs[ii].refreshSuggestValueList();
                                    break;
                                case 'OrderBy':
                                    divs[ii].root = this;
                                    divs[ii].refreshSuggestValueList = function () {
                                        this.listContainer = this.querySelector('.items');
                                        var list = this.listContainer.querySelectorAll('div[value]');

                                        this.radios = this.listContainer.querySelectorAll('input[type="radio"]');
                                        this.validateMsg = this.querySelector('div[data-validation-msg="true"]');
                                        this.orderCombo = $('#ComboboxID');

                                        $(function () {
                                            $.widget('custom.combobox', {
                                                _create: function () {
                                                    this.wrapper = $("<span>").addClass("custom-combobox").insertAfter(this.element);
                                                    this.element.hide();
                                                    this._createAutoComplete();
                                                    this._createShowAllButton();
                                                },

                                                _createAutoComplete: function () {
                                                    var selected = this.element.children(":selected"),
                                                        value = selected.val() ? selected.text() : "";

                                                    this.input = $("<input>")
                                                        .appendTo(this.wrapper)
                                                        .val(value)
                                                        .attr("title", "")
                                                        .addClass("custom-combobox-input ui-widget ui-widget-content ui-state-default ui-corner-left")
                                                        .autocomplete({
                                                            delay: 0,
                                                            minLength: 0,
                                                            source: $.proxy(this, "_source")
                                                        });

                                                    this._on(this.input, {
                                                        autocompleteselect: function (event, ui) {
                                                            ui.item.option.selected = true;
                                                            this._trigger("selected", event, { item: ui.item.option });
                                                        },
                                                        autocompletechange: "_removeIfInvalid",
                                                        autocompleteopen: function () {
                                                            var totalHeight = $("div[data-selector*='content-select']").outerHeight();
                                                            var displayHeight = $("#displayList").outerHeight();
                                                            var height = 200;
                                                            height = totalHeight - displayHeight - 30;

                                                            $('.ui-autocomplete').css({ "max-height": height * 0.45 + "px" })
                                                        }
                                                    });
                                                },

                                                _createShowAllButton: function () {
                                                    var input = this.input,
                                                        wasOpen = false;

                                                    var element = $("<a id='icon-down'>")
                                                        .attr("tabIndex", -1)
                                                        .attr("role", "button")
                                                        //.tooltip()
                                                        .appendTo(this.wrapper)
                                                        .removeClass("ui-corner-all")
                                                        .addClass("ui-button ui-widget ui-button-icon-only custom-combobox-toggle ui-corner-right")
                                                        .on("mousedown", function () {
                                                            wasOpen = input.autocomplete("widget").is(":visible");
                                                            console.log(wasOpen);
                                                        })
                                                        .on("click", function () {
                                                            input.trigger("focus");
                                                            if (wasOpen) return;

                                                            input.autocomplete("search", "");
                                                        });

                                                    $("#icon-down").append('<span class="ui-button-icon ui-icon ui-icon-triangle-1-s"></span>')
                                                        .append('<span class="ui-button-icon-space"> </span>');

                                                },

                                                _source: function (request, response) {
                                                    var matcher = new RegExp($.ui.autocomplete.escapeRegex(request.term), "i");
                                                    response(this.element.children("option").map(function () {
                                                        var text = $(this).text();
                                                        if (this.value && (!request.term || matcher.test(text)))
                                                            return {
                                                                label: text,
                                                                value: text,
                                                                option: this
                                                            };
                                                    }));
                                                },

                                                _removeIfInvalid: function (event, ui) {
                                                    if (ui.item) return;
                                                    var value = this.input.val(),
                                                        valueLowerCase = value.toLowerCase(),
                                                        valid = false;
                                                    this.element.children("option").each(function () {
                                                        if ($(this).text().toLowerCase() === valueLowerCase) {
                                                            this.selected = valid = true;
                                                            return false;
                                                        }
                                                    });

                                                    if (valid) return;

                                                    this.input.val("").attr("title", value + "didn't match any item")
                                                    //.tooltip("open");
                                                    this.element.val("");
                                                    //this._delay(function () {
                                                    //    this.input.tooltip("close").attr("title", "");
                                                    //}, 2500);
                                                    this.input.autocomplete("instance").term = "";
                                                },

                                                _destroy: function () {
                                                    this.wrapper.remove();
                                                    this.element.show();
                                                }

                                            });

                                            $("#ComboboxID").combobox();
                                        });

                                        this.updateSelectionValue = function () {
                                            var isValid = ctrl.validationControl();
                                            if (isValid) {
                                                var displayValue = '', value = '';

                                                var currentInput = this.root.currentInput;
                                                var keyword = currentInput.keyword;
                                                var keyName = currentInput.keyName;

                                                var val = $('#ComboboxID option:selected').val();
                                                var text = $('#ComboboxID option:selected').text();

                                                var radios = currentInput.radios;
                                                for (var i = 0; i < radios.length; i++) {
                                                    if (radios[i].checked) {
                                                        displayValue = radios[i].getAttribute('display-value') + ctrl.constants.charDisplayValue + text;
                                                        value = val + " " + radios[i].value;
                                                    }
                                                }

                                                ctrl.displayValue(displayValue, value);
                                            }
                                        }

                                        this.resetValue = function () {
                                            var radios = this.radios;
                                            radios[0].checked = true;

                                            ctrl.showHideValidation(false, this);
                                        };

                                        ctrl.loadActionButton(this);

                                    };

                                    if (divs[ii].listContainer == null) divs[ii].refreshSuggestValueList();
                                    break;
                                case 'AutoComplete':
                                    divs[ii].root = this;
                                    divs[ii].multiSelect = true;
                                    divs[ii].refreshSuggestValueList = function () {
                                        this.listContainer = this.querySelector('.items');
                                        var list = this.listContainer.querySelectorAll('div[value]');

                                        function split(val) {
                                            return val.split(/,\s*/);
                                        }

                                        function extractLast(term) {
                                            return split(term).pop();
                                        }

                                        $("#inputValue")
                                            .on("keydown", function (event) {
                                                this.style.width = ((this.value.length + 2) * 10) + 'px';
                                                $('#autoCompleteNoResult').addClass('hide');
                                                if (event.keyCode === $.ui.keyCode.TAB
                                                    && $(this).autocomplete("instance").menu.active) {
                                                    event.preventDefault();
                                                }
                                            })
                                            .on("paste", function (evt) {
                                                var elem = "";
                                                if (evt.originalEvent && evt.originalEvent.clipboardData) {
                                                    elem = evt.originalEvent.clipboardData.getData("text");
                                                }
                                                else if (window.clipboardData) {
                                                    elem = window.clipboardData.getData('Text');
                                                }

                                                this.style.width = ((elem.length + 1) * 10) + 'px';
                                            })
                                            .autocomplete({
                                                minLength: 3,
                                                multiselect: true,
                                                source: function (request, response) {
                                                    var curr = ctrl.currentInput;
                                                    var value = (ctrl.currentParentItem && ctrl.currentParentItem.fakeDisplayMultipleSelection)
                                                        ? ctrl.currentParentItem.fakeDisplayMultipleSelection.getAttribute('data-value')
                                                        : (ctrl.currentAddingMultiSelectItem ? ctrl.currentAddingMultiSelectItem.getAttribute('data-value') : '');

                                                    if (ctrl.editingItem) {
                                                        value = ctrl.editingItem.getAttribute('data-value');
                                                    }

                                                    //var isSingleMerchant = ctrl.checkRequiredMerchant() && (ctrl.currentParentItem || (ctrl.editingItem && ctrl.editingItem.editingChildItem));

                                                    var data = {};
                                                    data["page"] = advancedFilter.currentFilter._sectionName;
                                                    data["type"] = curr.type;
                                                    data["key"] = curr.keyword;
                                                    data["subValue"] = curr.getAttribute('data-parent-key');
                                                    data["filterText"] = request.term;
                                                    data["ignoreValue"] = ctrl.checkRequiredMerchant() ? "" : value;
                                                    $.ajax({
                                                        type: "post",
                                                        url: rootURL + "AdvancedFilterPage.aspx/GetDataAutoComplete",
                                                        async: true,
                                                        data: JSON.stringify(data),
                                                        contentType: "application/json",
                                                        dataType: "json",
                                                        success: function (result) {
                                                            var data = result.d;
                                                            response($.map(data, function (item) {
                                                                return {
                                                                    label: item.Value,
                                                                    value: item.Key,
                                                                    display: item.DisplayValue
                                                                }
                                                            }));
                                                            if (data.length <= 0) {
                                                                $('#autoCompleteNoResult').removeClass('hide');
                                                            }
                                                        },
                                                    });

                                                },
                                                select: function (evt, ui) {

                                                    // If the mode of Date Time filter is date range, merchant allow select one
                                                    // After selected, the panel selection will disappear
                                                    var terms = split(this.value);
                                                    // remove the current input
                                                    terms.pop();
                                                    // add the selected item
                                                    terms.push(ui.item.value);
                                                    // add placeholder to get the comma-and-space at the end

                                                    var isSingleMerchant = ctrl.checkRequiredMerchant();// && (ctrl.currentParentItem || (ctrl.editingItem && ctrl.editingItem.editingChildItem));

                                                    if (isSingleMerchant) {
                                                        this.value = ui.item.label;

                                                        // Update length of textbox
                                                        var len = ((this.value.length + 2) * 10) + 'px';
                                                        $('#inputValue').css("width", len);

                                                        ctrl.updateWidthTxtAutoComplete(this.value);
                                                    }
                                                    else {
                                                        this.value = "";
                                                        var liTag = document.createElement('li');
                                                        liTag.className = 'search-choice';

                                                        var spanTag = document.createElement('span');
                                                        spanTag.textContent = ui.item.label;

                                                        var iconCloseTag = document.createElement('a');
                                                        iconCloseTag.className = 'search-choice-close';
                                                        iconCloseTag.setAttribute('item-value', ui.item.value);
                                                        iconCloseTag.onclick = function () {
                                                            ctrl.removeAutoCompleteItem(this);
                                                        };

                                                        $(liTag).append(spanTag);
                                                        $(liTag).append(iconCloseTag);
                                                        $("ul.chosen-choices li:last-child").before(liTag);

                                                        ctrl.updateWidthTxtAutoComplete();
                                                    }

                                                    var isValid = ctrl.validationControl();
                                                    if (isValid) {
                                                        var value = (ctrl.currentParentItem && ctrl.currentParentItem.fakeDisplayMultipleSelection)
                                                            ? ctrl.currentParentItem.fakeDisplayMultipleSelection.getAttribute('data-value')
                                                            : (ctrl.currentAddingMultiSelectItem ? ctrl.currentAddingMultiSelectItem.getAttribute('data-value') : '');
                                                        var displayVal = (ctrl.currentParentItem && ctrl.currentParentItem.fakeDisplayMultipleSelection)
                                                            ? ctrl.currentParentItem.fakeDisplayMultipleSelection.displayValue.textContent
                                                            : (ctrl.currentAddingMultiSelectItem ? ctrl.currentAddingMultiSelectItem.getAttribute('data-display-value') : '');
                                                        var displayAutoComplete = (ctrl.currentParentItem && ctrl.currentParentItem.fakeDisplayMultipleSelection)
                                                            ? ctrl.currentParentItem.fakeDisplayMultipleSelection.getAttribute('data-display-autocomplete')
                                                            : (ctrl.currentAddingMultiSelectItem ? ctrl.currentAddingMultiSelectItem.getAttribute('data-display-autocomplete') : '');

                                                        if (ctrl.editingItem) {
                                                            value = ctrl.editingItem.getAttribute('data-value');
                                                            displayVal = ctrl.editingItem.getAttribute('data-display-value');
                                                            displayAutoComplete = ctrl.editingItem.getAttribute('data-display-autocomplete');
                                                        }

                                                        var valueArr = (value && !isSingleMerchant) ? value.split(',') : [];
                                                        var displayValArr = (displayVal && !isSingleMerchant) ? displayVal.split(', ') : [];
                                                        var displayAutoCompleteArr = (displayAutoComplete && !isSingleMerchant) ? displayAutoComplete.split('#;') : [];

                                                        displayValArr.push(ui.item.display);
                                                        valueArr.push(ui.item.value);
                                                        displayAutoCompleteArr.push(ui.item.label);

                                                        var dataValue = valueArr.length > 0 ? valueArr.join(',') : '';
                                                        var dataDisplayValue = displayValArr.length > 0 ? displayValArr.join(', ') : '';
                                                        var dataDisplayAutoComplete = displayAutoCompleteArr.length > 0 ? displayAutoCompleteArr.join('#;') : '';

                                                        if (ctrl.editingItem) {
                                                            var displayValueLink = ctrl.editingItem.editingChildItem ? ctrl.editingItem.editingChildItem.querySelector('a[class="display-value"]') : ctrl.editingItem.querySelector('a[class="display-value"]');
                                                            displayValueLink.textContent = dataDisplayValue;

                                                            ctrl.editingItem.setAttribute('data-value', dataValue);
                                                            ctrl.editingItem.setAttribute('data-display-value', dataDisplayValue);
                                                            ctrl.editingItem.setAttribute('data-display-autocomplete', dataDisplayAutoComplete);
                                                        }
                                                        else {
                                                            if (ctrl.currentParentItem && ctrl.currentParentItem.fakeDisplayMultipleSelection) {
                                                                ctrl.currentParentItem.fakeDisplayMultipleSelection.displayValue.textContent = dataDisplayValue;
                                                                ctrl.currentParentItem.fakeDisplayMultipleSelection.setAttribute('data-value', dataValue);
                                                                ctrl.currentParentItem.fakeDisplayMultipleSelection.setAttribute('data-display-value', dataDisplayValue);
                                                                ctrl.currentParentItem.fakeDisplayMultipleSelection.setAttribute('data-display-autocomplete', dataDisplayAutoComplete);
                                                            }
                                                            else {
                                                                ctrl.displayValue(dataDisplayValue, dataValue, "", dataDisplayAutoComplete);
                                                            }
                                                        }

                                                        ctrl.events.addItemCallback(true);
                                                        ctrl.updateValueContainerHeight();

                                                        if (!($(".select-merchant").hasClass("hide"))) {
                                                            $(".select-merchant").addClass("hide");
                                                            $("#btnApplyAdvFilter").removeAttr("disabled");
                                                            $(".required-icon").addClass("hide");
                                                        }
                                                    }

                                                    return false;

                                                },
                                                search: function () {
                                                    var term = extractLast(this.value);
                                                    if (term.length < 3) {
                                                        return false
                                                    };
                                                },
                                                focus: function () {
                                                    return false;
                                                },
                                                open: function (event, ui) {
                                                    updateWidthAutoComplete();
                                                }
                                            }).focus(function () {
                                                window.pageIndex = 0;
                                            })


                                        this.resetValue = function () {
                                            $('#autoCompleteNoResult').addClass('hide');
                                            $('#autoCompleteNoResult').html(msg_UxAdvancedFilter_NoResults);
                                            $('#inputValue').val('');
                                            $('#autocompleteID li.search-choice').each(function () {
                                                $(this).remove();
                                            });

                                            ctrl.showHideValidation(false, this);
                                        };

                                        $('#autocompleteID').click(function () {
                                            $("#inputValue").focus();
                                        })

                                        ctrl.loadActionButton(this);
                                    };

                                    if (divs[ii].listContainer == null) divs[ii].refreshSuggestValueList();
                            }
                        }
                    }
                }
            }

            suggestItems[k].onclick = function () {
                ctrl.suggestionItemClick(this);
            };
        }

        this.fieldList = suggestItems;
    };

    ctrl.reset = function () {
        this.hideValueSuggestion();
        this.hideFieldSuggestion();
        this.isKeyHandled = false;
        this.inValueMode = false;
        this.isAddSubItem = false;
        this.currentParentItem = null;
        this.enterEvent = false;

        if (this.editingItem) {
            if (this.editingItem.input) {
                this.hideValueItemSuggestion(this.editingItem.input);
            }

            if (this.editingItem.onmouseover && this.editingItem.getListChildItems && this.editingItem.getListChildItems().length == 0) {
                this.editingItem.onmouseover();
            }

            this.editingItem = null;
        }

        this.toggleHiddenInputField();
        var items = this.fieldList;
        if (this.inputText.value == '') {
            for (var i = 0; i < items.length; i++) {
                if (this.distinct && this.isFieldSelected(items[i])) {
                    items[i].style.display = 'none';
                }
                else {
                    items[i].style.display = 'block';
                }
                items[i].className = '';
            }
        }
        this.suggestValueItem = null;
        this.fieldContainer.currentIndex = -1;

        if (this.currentInput != null) {
            this.currentInput.currentEdit = null;
            if (this.currentInput.valueList != null) {

                this.currentInput.currentIndex = -1;
                items = this.currentInput.valueList;
                for (var i = 0; i < items.length; i++) {
                    items[i].style.display = 'block';
                    items[i].className = '';
                }
            }
            else if (this.currentInput.hide) {
                this.currentInput.hide(true);
            }

            var curr = this.currentInput;
            ctrl.setContentItems(curr, "");
        }

        this.hideValueItemSuggestion(this.currentInput);
        this.currentInput = null;
        this.inputText.value = '';

        $(this.inputText).removeAttr('data-keyword');
        this.fakeInputText.value = '';
        this.updateDisplayContainerHeight();
        ctrl.toggleDisplay(false);
    };

    ctrl.animation = function () {
        $(".container-field").addClass("animation-simulation");
        setTimeout(function () {
            $(".container-field").removeClass("animation-simulation");
        }, 350);
    };

    ctrl.enterEvent = false;
    ctrl.inputText.onkeydown = function (evt) {
        this.root.isKeyHandled = false;
        if (this.root.keyupTimer) clearTimeout(this.root.keyupTimer);
        if (this.root.ajaxRequestor != null) this.root.ajaxRequestor.cancel();
        //dhn.debug(evt.keyCode);
        switch (evt.keyCode) {
            case 38://up key
                {
                    this.root.moveSuggestionItem(true);
                    this.root.isKeyHandled = true;
                    return false;
                }
                break;
            case 40://down key
                {
                    this.root.moveSuggestionItem(false);
                    this.root.isKeyHandled = true;
                    return false;
                }
                break;
            case 9://tab key
                if (this.root.isAddSubItem && this.root.currentParentItem) {

                    if (this.root.currentParentItem.fakeInput
                        && this.root.currentParentItem.fakeInput.value
                        && this.root.currentParentItem.fakeInput.value.trim() != '') {

                        this.value = this.root.currentParentItem.fakeInput.value + (this.root.inValueMode ? '' : ':');

                        this.root.isKeyHandled = true;
                        if (!this.root.inValueMode) this.root.suggestField();
                        this.root.currentParentItem.fakeInput.value = ' ';
                        this.onkeyup();
                        this.root.currentParentItem.setValueChildItemInput(this.value);
                    }

                    return false;
                }
                else if (this.root.fakeInputText.value != this.root.fakeInputText.defaultValue) {

                    if (this.root.fakeInputText.value.trim() != '') {
                        this.value = this.root.fakeInputText.value + (this.root.inValueMode ? '' : ':');

                        this.root.isKeyHandled = true;
                        if (!this.root.inValueMode) this.root.suggestField();
                        this.root.fakeInputText.value = ' ';
                        this.onkeyup();
                        //if (!this.root.currentInput.multiSelect) {
                        this.focus();
                        //}
                    }

                    return false;
                }

                break;
            case 59://; or : key (FF)
            case 186://; or : key
                if (!evt.shiftKey) {
                    this.root.addItem();
                    this.root.isKeyHandled = true;
                    this.root.inputText.onkeyup();
                    return false;
                }
                else {
                    if (this.root.matchedField == null) {
                        this.root.isKeyHandled = true;
                        return false;
                    }
                }
                break;
            case 13:
                var currentIndex = this.root.fieldContainer.currentIndex;
                var item = this.root.fieldList[currentIndex];
                if (item) {
                    ctrl.enterEvent = true;
                    ctrl.suggestionItemClick(item);
                }
                return false;
                break;
            case 8://backspace key
                if (this.value == '') {
                    //this.root.removeLastItem();
                    return false;
                }
                break;
        }

        return aperia.searchControl.validate.helper.convertKeyCode(evt);
    };

    ctrl.inputText.onkeyup = function (evt) {
        // Use: move suggestion item
        if (this.root.isKeyHandled) {
            this.root.isKeyHandled = false;
            return;
        }

        // Check Input to show suggestion field or load item
        this.root.analyseInput();

        if (this.root.inputValue != null) {
            this.root.isSelectedDate = false;
            // Show suggest field
            this.root.suggestField();

            this.root.inValueMode = true;
            if (this.root.currentInput != null) { // Show detail item
                switch (this.root.currentInput.type) {
                    case 'SubFilter':
                        // Remove current filter if exist
                        var items = this.root.displayContainer.querySelectorAll('div.item');
                        if (items.length > 0) {
                            ctrl.removeItemNotKeepVal(items[0]);
                        }

                        // Add new sub filter
                        this.root.addSubItem(this.root.currentInput);

                        //Clear data
                        this.root.hideFieldSuggestion();
                        ctrl.updateValueContainerHeight();
                        $(this.fieldContainer).css('height', '');
                        $(".container-field").addClass("show-effect");
                        break;
                    default:
                        // Bind data
                        ctrl.bindSuggestItems();

                        //Clear data
                        this.root.hideFieldSuggestion();
                        this.root.currentInput.toggleEditMode(false);
                        this.root.valuesContainer.style.display = 'block';
                        this.root.currentInput.style.display = 'block';
                        this.root.currentInput.resetValue();

                        // Show value
                        if (this.root.currentParentItem) {
                            this.root.currentParentItem.collapseLink.className = '';
                            this.root.currentParentItem.toggleHiddenInputField(true);
                        } else {
                            this.root.toggleHiddenInputField(true);
                        }

                        this.root.fakeInputText.value = ' ';
                        ctrl.updateValueContainerHeight();
                        $(this.fieldContainer).css('height', '');
                        $(".container-field").addClass("show-effect");
                        break;
                }
            }
        }
        else {
            // Load Suggestion Field
            if (this.root.currentInput != null) {
                //if (this.root.currentInput.type == 'datepicker') {
                //    this.root.currentInput.hide(true);
                //} else {
                this.root.currentInput.style.display = 'none';
                //}
            }
            //if (!this.root.isSelectedDate) { //fix for IE
            this.root.suggestField();
            this.root.showFieldSuggestion(false);
            //}
            //this.root.isSelectedDate = false;
        }
    };

    ctrl.inputText.onfocus = function () {
        // Edit mode: Clear parameter is empty value
        this.root.onBlurEditMultipleValue();

        clearTimeout(this.root.hideTimer);

        this.root.inputText.onkeyup();

        //collapse all
        this.root.events.onFocusTextField();
    };

    ctrl.inputText.onblur = function () {
        var isHoverContainer = ctrl.fieldContainer.parentNode.querySelector(':hover') === ctrl.fieldContainer,
            isHoverValueContainer = ctrl.valuesContainer.parentNode.querySelector(':hover') === ctrl.valuesContainer;
        this.root.hideTimer = setTimeout(function (root) {
            if ((!isHoverContainer && !isHoverValueContainer && !ctrl.enterEvent)) {
                ctrl.enterEvent = false;
                if (root.ajaxRequestor != null) root.ajaxRequestor.cancel();

                if (!root.editingItem || (root.editingItem && !root.editingItem.editingChildItem)) {
                    if (root.isAddSubItem && root.currentParentItem) {
                        if (root.editingItem) {
                            root.currentParentItem.removeInputBox();
                            //root.reset();
                        } else {
                            //remove and reset
                            root.currentParentItem.removeInputField();
                            //root.reset();
                        }
                    } else {
                        root.reset();
                        if (root.editingItem) {
                            root.inputText.value = '';
                            root.fakeInputText.value = '';
                            root.hideFieldSuggestion();
                        } else {
                            if (!root.currentInput) {
                                root.reset();
                            }
                        }
                    }
                }
            }
        }, 100, this.root);
    };

    ctrl.fakeInputText.onfocus = function () {
        if (ctrl.inputText.style.display == 'none') {
            ctrl.inputText.style.display == '';
            ctrl.inputText.style.visibility == '';
            ctrl.onBlurEditMultipleValue();

            if (ctrl.currentParentItem) {
                ctrl.currentParentItem.toggleHiddenInputField();
                ctrl.currentParentItem.removeInputField();
                ctrl.currentParentItem = null;
            }

            ctrl.currentInput = null;
            setTimeout(function () {
                ctrl.inputText.focus();
            }, 400);
        }
    };

    ctrl.toggleHiddenInputField = function (isHidden) {
        if (isHidden) {
            this.fakeDisplayMultipleSelection.displayText.textContent = this.inputText.value;
            this.fakeDisplayMultipleSelection.displayValue.textContent = '';
            this.fakeDisplayMultipleSelection.setAttribute('data-value', '');
            this.fakeDisplayMultipleSelection.setAttribute('data-display-value-real', '');
            this.fakeDisplayMultipleSelection.style.display = '';
            this.fakeDisplayMultipleSelection.style.visibility = '';
            this.inputText.style.display = 'none';
            this.inputText.style.visibility = 'hidden';
            this.fakeInputText.value = '';
        } else {
            this.fakeDisplayMultipleSelection.displayText.textContent = '';
            this.fakeDisplayMultipleSelection.displayValue.textContent = '';
            this.fakeDisplayMultipleSelection.setAttribute('data-value', '');
            this.fakeDisplayMultipleSelection.setAttribute('data-display-value-real', '');
            this.fakeDisplayMultipleSelection.style.display = 'none';
            this.fakeDisplayMultipleSelection.style.visibility = 'hidden';
            this.inputText.style.display = '';
            this.inputText.style.visibility = '';
            this.currentAddingMultiSelectItem = null;
            //if (this.currentParentItem) {
            //    this.currentParentItem.currentAddingMultiSelectItem = null;
            //}
        }
    };

    ctrl.events = {
        resetMultiSelection: function () {
            if (ctrl.currentParentItem) {
                ctrl.currentParentItem.toggleHiddenInputField();
            } else {
                ctrl.toggleHiddenInputField();
            }
        },
        onCloseCallBack: function (e) {
            var isHoverContainer = ctrl.fieldContainer.parentNode.querySelector(':hover') === ctrl.fieldContainer,
                isHoverValueContainer = ctrl.valuesContainer.parentNode.querySelector(':hover') === ctrl.valuesContainer;
            if (!isHoverContainer && !isHoverValueContainer) {
                if (ctrl.editingItem) {
                    if (ctrl.editingItem.editingChildItem && ctrl.editingItem.editingChildItem.input) {
                        if (e.target && (e.target != ctrl.editingItem.editingChildItem)
                            && (!e.target.parentNode || e.target.parentNode != ctrl.editingItem.editingChildItem)
                            && e.target != ctrl.inputText) {
                            switch (ctrl.editingItem.editingChildItem.input.type) {
                                case 'MultiSelection':
                                case 'MultiChoose':
                                case 'AutoComplete':
                                    var isResetEditItem = false;
                                    if (ctrl.editingItem.editingChildItem.input.closeButton) {
                                        isResetEditItem = true;
                                        ctrl.editingItem.input.closeButton.onclick();
                                    }
                                    break;
                                default:
                                    ctrl.editingItem.editingChildItem = null;
                                    ctrl.editingItem = null;
                                    ctrl.reset();
                            }
                        }

                    } else if (ctrl.editingItem.input && ctrl.editingItem.input.type != 'text') {
                        if (e.target && (e.target != ctrl.editingItem)
                            && (!e.target.parentNode || e.target.parentNode != ctrl.editingItem)) {
                            switch (ctrl.editingItem.input.type) {
                                case 'MultiSelection':
                                case 'MultiChoose':
                                case 'AutoComplete':
                                    var isResetEditItem = false;

                                    if (ctrl.editingItem.input.closeButton) {
                                        isResetEditItem = true;
                                        ctrl.editingItem.input.closeButton.onclick();
                                    }

                                    if (!isResetEditItem) {
                                        ctrl.editingItem.input.style.display = 'none';
                                        ctrl.valuesContainer.style.display = 'none';
                                        ctrl.toggleDisplay(false);
                                        ctrl.toggleEffect(false);
                                        ctrl.updateSuggestionContainerHeight();
                                    }
                                    break;
                                default:
                                    ctrl.editingItem = null;
                                    ctrl.reset();
                            }
                        }
                    } else if (ctrl.editingItem.input && ctrl.editingItem.input.type == 'text') {
                        ctrl.addItem();
                    }
                }
                else {
                    //add item
                    if (ctrl.currentInput && ctrl.currentInput.multiSelect) {
                        if (ctrl.currentParentItem) {
                            var value = ctrl.currentParentItem.fakeDisplayMultipleSelection.getAttribute('data-value');
                            if (!value || value.split(',').length == 0) {
                                if (ctrl.currentInput.backButton) {
                                    ctrl.currentInput.backButton.onclick();
                                    if (ctrl.currentParentItem) {
                                        ctrl.currentParentItem.removeInputField();
                                    }
                                    ctrl.reset();
                                }
                            }
                            else if (value.split(',').length > 0) {
                                if (ctrl.currentInput.closeButton) {
                                    ctrl.currentInput.closeButton.onclick();
                                }
                            }
                        }
                        else {
                            //add item
                            if (ctrl.currentAddingMultiSelectItem) {
                                var value = ctrl.currentAddingMultiSelectItem.getAttribute('data-value');
                                if (!value || value.split(',').length == 0) {
                                    $(ctrl.currentAddingMultiSelectItem).remove();
                                    ctrl.events.sendRemoveItemCallback();
                                }

                                if (ctrl.currentInput.closeButton) {
                                    ctrl.currentInput.closeButton.click();
                                }

                            } else {
                                if (ctrl.currentInput) {
                                    ctrl.reset();
                                }
                            }
                        }
                    }
                    else if (ctrl.currentInput) {
                        if (ctrl.currentInput.backButton) {
                            if (ctrl.currentParentItem) {
                                ctrl.currentInput.backButton.onclick();
                                ctrl.currentParentItem.removeInputField();
                            }
                            ctrl.reset();
                        }
                    }
                    else {
                        // find parent item
                        var parentItem = $(ctrl.displayContainer).find("div[data-is-parent='true']");
                        var attrParent = parentItem != undefined ? parentItem.attr('data-is-parent-open') : '';
                        if (!e.target.parentNode.hasAttribute('data-is-parent') && attrParent == 'true') {
                            // Close suggestion field for parent item
                            ctrl.reset();
                            parentItem.removeAttr('data-is-parent-open');
                        }
                    }
                }
            }
        },
        sendRemoveItemCallback: function (e) {
            $(ctrl.displayContainer).trigger('onRemoveItemComplete');
        }
    };

    ctrl.onBlurEditMultipleValue = function () {
        if (ctrl.editingItem) {
            if (ctrl.editingItem.editingChildItem && ctrl.editingItem.editingChildItem.input) {
                switch (ctrl.editingItem.editingChildItem.input.type) {
                    case 'MultiSelection':
                    case 'MultiChoose':
                    case 'AutoComplete':
                        if (ctrl.checkRequiredMerchant() && ctrl.editingItem.editingChildItem.input.keyword == "Merchant") {
                            return;
                        }
                        var value = ctrl.editingItem.editingChildItem.getAttribute('data-value');
                        if (!value || value.split(',').length == 0) {
                            ctrl.removeItemNotKeepVal(ctrl.editingItem.editingChildItem);
                        }
                        break;
                }
            } else if (ctrl.editingItem.input) {
                switch (ctrl.editingItem.input.type) {
                    case 'MultiSelection':
                    case 'MultiChoose':
                    case 'AutoComplete':
                        var isResetEditItem = false;
                        var value = ctrl.editingItem.getAttribute('data-value');
                        if (!value || value.split(',').length == 0) {
                            ctrl.removeItemNotKeepVal(ctrl.editingItem);
                        }
                        break;
                }
            }

            if (ctrl.editingItem && ctrl.editingItem.onmouseover && ctrl.editingItem.getListChildItems && ctrl.editingItem.getListChildItems().length == 0) {
                ctrl.editingItem.onmouseover();
            }
        }
        else if (ctrl.currentInput) {
            if (ctrl.currentParentItem) {
                var value = ctrl.currentParentItem.fakeDisplayMultipleSelection.getAttribute('data-value');
                if (!value || value.split(',').length == 0) {
                    this.inputText.value = '';
                    this.fakeInputText.value = '';

                    ctrl.currentParentItem.fakeDisplayMultipleSelection.displayText.textContent = '';
                    ctrl.currentParentItem.fakeDisplayMultipleSelection.displayValue.textContent = '';
                    ctrl.currentParentItem.fakeDisplayMultipleSelection.setAttribute('data-value', '');
                    ctrl.currentParentItem.fakeDisplayMultipleSelection.setAttribute('data-display-value-real', '');
                    ctrl.currentParentItem.fakeDisplayMultipleSelection.setAttribute('data-display-autocomplete', '');
                    ctrl.currentParentItem.fakeDisplayMultipleSelection.style.display = 'none';
                    ctrl.currentParentItem.fakeDisplayMultipleSelection.style.visibility = 'hidden';
                    ctrl.currentAddingMultiSelectItem = null;
                }
                else if (value.split(',').length > 0) {
                    if (ctrl.currentInput.closeButton) {
                        ctrl.currentInput.closeButton.onclick();
                    }
                }
            }
            else {
                //add item
                if (ctrl.currentAddingMultiSelectItem) {
                    var value = ctrl.currentAddingMultiSelectItem.getAttribute('data-value');
                    if (!value || value.split(',').length == 0) {
                        $(ctrl.currentAddingMultiSelectItem).remove();
                        ctrl.events.sendRemoveItemCallback();
                    }

                    if (ctrl.currentInput.closeButton) {
                        ctrl.currentInput.closeButton.click();
                    }
                }
            }
        }
    }

    ctrl.hideValueInput = function (item) {
        if (item.input) {
            item.input.style.display = 'none';
            switch (item.input.type) {
                case 'MultiSelection':
                    var items = item.input.valueList;
                    for (var i = 0; i < items.length; i++) {
                        items[i].style.display = 'none';
                    }
                    break;
            }
        }
    };

    ctrl.isAddSubItem = false;
    ctrl.currentParentItem = null;
    ctrl.isChildItem = function (item) {
        var result = item.getAttribute('data-parent-key') || false;
        return result;
    };

    ctrl.isParentItem = function (item) {
        var isParent = (item.getAttribute('data-is-parent') || 'False') == 'True';
        return isParent;
    };

    ctrl.subFilter = function (item) {
        item.currentInput = null;
        item.addSubLink = null;
        item.childFieldList = null;
        item.arrow = null;
        item.isExpanded = true;
        item.input = null;
        item.fakeDisplayMultipleSelection = null;
        item.childItemsWraper = null;
        item.getItemInput = function () {
            var inputName = this.getAttribute('data-display-name');

            //get current input
            var items = ctrl.fieldList;
            var currentItem = items.filter(function (idx, field) {
                return field && field.getAttribute('data-display-value') == inputName;
            });

            if (!currentItem || currentItem.length == 0) return false;

            this.input = currentItem[0].input;
        };
        item.isCollapsed = false;
        item.addSubFilterLink = function () {
            var addSubFilter = document.createElement('a');
            addSubFilter.className = 'add-sub-filter-link';
            addSubFilter.textContent = uxAdvancedFilter_LblAddSubFilters;
            addSubFilter.onclick = function () {
                ctrl.onBlurEditMultipleValue();
                if (ctrl.editingItem && ctrl.editingItem.input && ctrl.editingItem.input.type == 'text') {
                    ctrl.addItem();
                }
                if (ctrl.isAddSubItem && ctrl.currentParentItem && ctrl.currentParentItem.removeInputBox) {
                    ctrl.currentParentItem.removeInputBox();
                    ctrl.currentParentItem.toggleHiddenInputField();
                    ctrl.currentInput = null;
                }
                ctrl.isAddSubItem = true;
                ctrl.currentParentItem = item;
                if (item.isCollapsed) {
                    item.collapseLink.onclick();
                    $(childItemsWraper).collapse('show');
                }
                if (ctrl.editingItem) {
                    if (ctrl.editingItem.input) {
                        ctrl.hideValueItemSuggestion(ctrl.editingItem.input);
                    }

                    ctrl.editingItem.editingChildItem = null;
                }
                item.addSubFilter();
                item.toggleHasSubClass();
            };
            //addSubFilter.style.display = 'none';

            var itemInfo = document.createElement('span');
            itemInfo.className = 'item-info';
            itemInfo.style.display = 'none';

            var collapseId = this.getAttribute('item-search-name') + '_' + this.getAttribute('data-value');
            var collapseLink = document.createElement('a');
            collapseLink.setAttribute('data-toggle', 'collapse');
            collapseLink.setAttribute('href', '#' + collapseId);
            var arrow = document.createElement('span');
            arrow.className = 'icon-up-arrow';
            collapseLink.style.display = 'none';
            collapseLink.style.visibility = 'hidden';
            collapseLink.appendChild(arrow);

            collapseLink.parent = this;
            collapseLink.onclick = function (e) {
                ctrl.onBlurEditMultipleValue();
                this.parent.toggleHiddenInputField();
                this.parent.hideParentValueSuggestion();
                this.parent.removeInputField();
                var children = this.parent.getListChildItems();

                if (this.parent.querySelectorAll('div.child-item').length > 0) {
                    this.parent.collapseLink.style.display = '';
                    this.parent.collapseLink.style.visibility = '';
                    this.parent.collapseLink.className = '';
                    if (this.parent.isSelectedAll()) {
                        this.parent.addSubLink.style.display = 'none';
                        this.parent.addSubLink.style.visibility = 'hidden';
                    } else {
                        this.parent.addSubLink.style.display = '';
                        this.parent.addSubLink.style.visibility = '';
                    }
                } else {
                    this.parent.collapseLink.style.display = 'none';
                    this.parent.collapseLink.style.visibility = 'hidden';
                    this.parent.addSubLink.style.display = '';
                    this.parent.addSubLink.style.visibility = '';
                }

                if (!children || children.length == 0) {
                    e.stopPropagation();
                    e.preventDefault();
                    return false;
                } else {

                    if (this.parent.childItemsWraper && this.parent.childItemsWraper.className.indexOf('collapse in') > -1) {
                        this.parent.arrow.className = 'icon-down-arrow';
                        this.parent.itemInfo.textContent = children.length == 1 ? '1 sub filter' : children.length + ' sub filters';
                        //this.parent.itemInfo.style.display = '';
                        //this.parent.itemInfo.style.visibility = '';
                        //this.parent.hideAddSubLink();
                        this.parent.isCollapsed = true;
                        $(this.parent).removeClass('has-sub');
                    } else {
                        this.parent.arrow.className = 'icon-up-arrow';
                        this.parent.itemInfo.style.display = 'none';
                        this.parent.itemInfo.style.visibility = 'hidden';
                        this.parent.itemInfo.textContent = '';
                        this.parent.isCollapsed = false;
                        this.parent.onmouseover(e);
                        $(this.parent).addClass('has-sub');
                    }
                }
            };

            var childItemsWraper = document.createElement('div');
            childItemsWraper.className = 'child-items-wraper collapse in';
            childItemsWraper.setAttribute('id', collapseId);

            $(childItemsWraper).on('shown.bs.collapse', function () {
                if (item.currentInput) {
                    item.currentInput.onfocus();
                }
            });

            this.childItemsWraper = childItemsWraper;

            this.arrow = arrow;
            this.collapseLink = collapseLink;
            this.addSubLink = addSubFilter;
            this.itemInfo = itemInfo;
            this.appendChild(addSubFilter);
            this.appendChild(itemInfo);
            this.appendChild(childItemsWraper);
            this.insertBefore(collapseLink, this.childNodes[0]);
        };

        item.addFakeDisplaySelection = function () {
            var div = document.createElement('div');
            div.className = 'display-multiple-selection';

            var span = document.createElement('span');
            span.className = 'display-text';
            var displayLink = document.createElement('a');
            displayLink.className = 'display-value';

            //div.appendChild(icon);
            div.appendChild(span);
            div.appendChild(displayLink);

            this.fakeDisplayMultipleSelection = div;
            this.fakeDisplayMultipleSelection.displayText = span;
            this.fakeDisplayMultipleSelection.displayValue = displayLink;
            this.fakeDisplayMultipleSelection.style.display = 'none';
            this.fakeDisplayMultipleSelection.style.visibility = 'hidden';

            this.appendChild(div);
        };

        item.addSubFilter = function () {
            this.hideParentValueSuggestion();
            ctrl.editingItem = null;
            var div = document.createElement('div');
            div.className = 'child-item input-box';
            var spanText = document.createElement('span');
            spanText.className = 'text';
            var input = document.createElement('input');
            input.type = 'text';
            input.autocomplete = 'off';
            input.className = 'input';

            var fakeInput = document.createElement('input');
            fakeInput.type = 'text';
            fakeInput.autocomplete = 'off';
            fakeInput.className = 'fake';
            $(fakeInput).css('text-transform', 'capitalize');
            spanText.appendChild(fakeInput);
            spanText.appendChild(input);
            div.appendChild(spanText);
            this.appendChild(div);

            input.root = div;
            //bind event
            input.onfocus = function () {
                ctrl.inputText.style.display = 'none';
                ctrl.inputText.value = input.value;
                ctrl.isAddSubItem = true;
                ctrl.currentParentItem = item;
                ctrl.inputText.onfocus();
            };

            input.onkeydown = function (evt) {
                ctrl.inputText.style.display = 'none';
                ctrl.inputText.value = input.value;
                ctrl.isAddSubItem = true;
                ctrl.currentParentItem = item;

                return ctrl.inputText.onkeydown(evt);
            };

            input.onkeyup = function (evt) {
                ctrl.inputText.style.display = 'none';
                ctrl.inputText.value = input.value;
                ctrl.inputText.onkeyup(evt);
                ctrl.isAddSubItem = true;
                ctrl.currentParentItem = item;
            };

            input.onblur = function (evt) {
                if (!ctrl.currentInput || !ctrl.currentInput.multiSelect) {
                    if (item.querySelectorAll('div.child-item').length > 0) {
                        item.addSubLink.style.display = 'none';
                        item.addSubLink.style.visibility = 'hidden';
                    } else {
                        item.addSubLink.style.display = '';
                        item.addSubLink.style.visibility = '';
                    }
                }

                ctrl.inputText.onblur(evt);
            };
            item.currentInput = input;
            item.fakeInput = fakeInput;
            item.currentInput.parent = div;
            item.addSubLink.style.display = 'none';
            item.addSubLink.style.visibility = 'hidden';
            item.collapseLink.style.display = '';
            item.collapseLink.style.visibility = '';
            item.collapseLink.className = 'disabled';
            input.focus();
            ctrl.fakeInputText.value = '';
        };

        item.removeInputField = function () {
            var div = this.querySelector('div.child-item.input-box');
            if (div) {
                this.removeChild(div);
                this.currentInput = null;
                item.collapseLink.className = '';
            }

            if (this.querySelectorAll('div.child-item').length == 0) {
                this.collapseLink.style.display = 'none';
                this.collapseLink.style.visibility = 'hidden';
                this.addSubLink.style.display = '';
                this.addSubLink.style.visibility = '';
            }

            this.toggleHasSubClass();

            this.root.inputText.style.display = '';
            this.root.fakeInputText.placeholder = msg_UxAdvancedFilter_PlaceHolderSelectFilter;
            //if (!this.root.currentInput || !this.root.currentInput.multiSelect) {
            //if (!this.root.currentInput) {
            setTimeout(function (root) {
                //if (!ctrl.editingItem && ((!ctrl.isAddSubItem && !ctrl.currentParentItem))) {
                //    //ctrl.reset();
                //}
                if (ctrl.currentInput == undefined)
                    ctrl.reset();
                else if (!(ctrl.currentInput.type == "MultiSelection" || ctrl.currentInput.type == "MultiChoose" || ctrl.currentInput.type == "AutoComplete")) {
                    ctrl.reset();
                }
            }, 1, this);
            //}
        };

        item.setValueChildItemInput = function (value) {
            if (this.currentInput) {
                this.currentInput.value = value;
                this.currentInput.focus();
            }
        };

        item.getListChildItems = function () {
            var listChilds = this.querySelectorAll('div.child-item:not(.input-box)');

            return listChilds;
        };

        item.removeLastChildItem = function () {
            var childItems = this.getListChildItems();
            if (childItems && childItems.length > 0) {
                var lastChild = childItems[childItems.length - 1];

                //remove input item
                var divs = this.querySelectorAll('div.child-item');
                for (var i = 0; i < divs.length; i++) {
                    if (divs[i].querySelector('input')) {
                        this.removeChild(divs[i]);
                    }
                }

                var inputText = lastChild.textContent || lastChild.innerText || '';
                //this.inputText.value = inputText;
                this.childItemsWraper.removeChild(lastChild);
                this.addSubFilter();
                this.currentInput.value = inputText;
                this.currentInput.focus();
                this.toggleHasSubClass();
            }
        };

        item.removeItem = function (item) {
            var inputText = item.textContent || item.innerText || '';
            this.removeInputBox();
            this.childItemsWraper.removeChild(item);
            this.addSubFilter();
            this.currentInput.value = inputText;
            this.currentInput.onfocus();
            this.toggleHasSubClass();
        };

        item.removeItemNotKeepVal = function (item) {
            this.removeInputBox();
            ctrl.hideValueInput(item);
            this.childItemsWraper.removeChild(item);
            this.toggleHasSubClass();

            var children = this.getListChildItems();

            if (children && children.length == 0) {
                this.collapseLink.style.display = 'none';
                this.collapseLink.style.visibility = 'hidden';
                this.addSubLink.style.display = '';
                this.addSubLink.style.visibility = '';
            }
        };

        item.resetEditing = function () {
            this.editingChildItem = null;
        }

        item.hideParentValueSuggestion = function () {
        };

        item.editingChildItem = null;

        item.getSelectedItems = function (fieldName) {
            var selectedItems = [];
            var items = this.querySelectorAll("div.child-item");
            if (items.length > 0) {
                for (var i = 0; i < items.length; i++) {
                    var dataValue = items[i].getAttribute('data-value');
                    var displayValue = items[i].getAttribute('data-display-name');
                    if (displayValue.toLowerCase() == fieldName.toLowerCase()) {
                        selectedItems.push({ name: displayValue, value: dataValue });
                    }
                }
            }
            return selectedItems;
        };

        item.removeInputBox = function () {
            var divInput = this.querySelector('div.child-item.input-box');
            if (divInput) {
                this.removeChild(divInput);
                this.collapseLink.className = '';
            }

            if (this.querySelectorAll('div.child-item').length == 0) {
                this.collapseLink.style.display = 'none';
                this.collapseLink.style.visibility = 'hidden';
                this.addSubLink.style.display = '';
                this.addSubLink.style.visibility = '';
            }

            this.toggleHasSubClass();
        };

        item.editItem = function (childItem) {
            if (ctrl.editingItem && ctrl.editingItem.input && ctrl.editingItem.input.type == 'text') {
                ctrl.addItem();
            }

            if (ctrl.isAddSubItem && ctrl.currentParentItem && ctrl.currentParentItem.removeInputBox) {
                ctrl.currentParentItem.removeInputBox();
            }

            ctrl.toggleDisplay(false);
            this.hideParentValueSuggestion();
            ctrl.onBlurEditMultipleValue();
            this.resetEditing();
            ctrl.resetEditingItem();
            ctrl.inputText.value = '';
            ctrl.fakeInputText.value = '';
            this.hideAddSubLink();
            ctrl.hideValueSuggestion();
            this.removeInputBox();
            var inputName = childItem.getAttribute('data-display-name');
            var currentValue = childItem.getAttribute('data-display-value');
            ctrl.inputField = inputName;

            //get current input
            var items = ctrl.fieldList;
            var currentItem = items.filter(function (idx, field) {
                return field && field.getAttribute('data-display-value') == inputName;
            });

            if (!currentItem || currentItem.length == 0) return false;

            var currentInput = currentItem[0].input;
            childItem.input = currentInput;

            // Bind value of Parameter
            //ctrl.bindSuggestItems();

            var result = false;
            item.input = currentInput;
            if (currentInput.toggleEditMode) {
                // Show title or Back button
                currentInput.toggleEditMode(true);
            }

            ctrl.updateSuggestionContainerHeight(true);
            ctrl.valuesContainer.style.display = 'block';
            currentInput.style.display = 'block';
            this.editingChildItem = childItem;
            ctrl.editingItem = this;

            switch (currentInput.type) {
                case 'text':
                    {
                        var value = $(item).attr('data-value');
                        currentInput.textbox.value = value;
                    }
                    break
            }
        };

        item.getChildFiedList = function () {
            var items = ctrl.fieldList;
            var searchKey = item.getAttribute('item-search-name');
            if (searchKey) {
                this.childFieldList = items.filter(function (idx, field) {
                    var parentKey = field.getAttribute('data-parent-key');
                    return parentKey == searchKey;
                });
            }
        };

        item.expand = function () {
        };

        item.collapse = function () {
        };

        item.hideAddSubLink = function () {
            this.addSubLink.style.display = 'none';
            this.addSubLink.style.visibility = 'hidden';
        };

        item.isSelectedAll = function () {
            var isSelectAll = true;
            if (this.childFieldList && this.childFieldList.length > 0) {
                for (var i = 0; i < this.childFieldList.length; i++) {
                    if (!ctrl.isFieldSelected(this.childFieldList[i], true)) {
                        isSelectAll = false;
                    }
                }
            }

            return isSelectAll;
        };

        item.init = function () {
            item.getItemInput();
            item.addSubFilterLink();
            item.addFakeDisplaySelection();
            item.getChildFiedList();
        };

        item.toggleHiddenInputField = function (isHidden) {
            if (isHidden) {
                this.hideAddSubLink();
                this.fakeDisplayMultipleSelection.displayText.textContent = this.currentInput.value;
                this.fakeDisplayMultipleSelection.displayValue.textContent = '';
                this.fakeDisplayMultipleSelection.setAttribute('data-value', '');
                this.fakeDisplayMultipleSelection.setAttribute('data-display-value-real', '');
                this.fakeDisplayMultipleSelection.setAttribute('data-display-autocomplete', '');
                this.fakeDisplayMultipleSelection.style.display = '';
                this.fakeDisplayMultipleSelection.style.visibility = '';
                if (this.currentInput) {
                    this.currentInput.parent.style.display = 'none';
                    this.currentInput.parent.style.visibility = 'hidden';
                }
            } else {
                this.fakeDisplayMultipleSelection.displayText.textContent = '';
                this.fakeDisplayMultipleSelection.displayValue.textContent = '';
                this.fakeDisplayMultipleSelection.setAttribute('data-value', '');
                this.fakeDisplayMultipleSelection.setAttribute('data-display-value-real', '');
                this.fakeDisplayMultipleSelection.setAttribute('data-display-autocomplete', '');
                this.fakeDisplayMultipleSelection.style.display = 'none';
                this.fakeDisplayMultipleSelection.style.visibility = 'hidden';
                if (this.currentInput) {
                    this.currentInput.parent.style.display = '';
                    this.currentInput.parent.style.visibility = '';
                    this.currentInput.focus();
                }
                this.currentAddingMultiSelectItem = null;
            }
        };

        item.toggleHasSubClass = function () {
            var children = this.querySelectorAll('div.child-item');

            if (children && children.length > 0) {
                $(this).addClass('has-sub');
            } else {
                $(this).removeClass('has-sub');
            }
        };

        item.init();

        item.hideRemoveButton = function () {
            var removeButtons = this.querySelectorAll('i.icon-close');

            if (removeButtons && removeButtons.length > 0) {
                for (var i = 0; i < removeButtons.length; i++) {
                    removeButtons[i].style.display = 'none';
                    //removeButtons[i].style.visibility = 'hidden';
                }
            }
        };

        item.onmouseover = function (e) {
            //show class add link
            var isSelectAll = item.isSelectedAll();

            //var isAddingChild = item.querySelector('div.child-item.input-box') || false;

            var isEditing = false;

            if (ctrl.editingItem && ctrl.editingItem.input && ctrl.editingItem.input.type != 'text') {
                if (ctrl.editingItem.editingChildItem && ctrl.editingItem.editingChildItem.input) {
                    isEditing = ctrl.editingItem.editingChildItem.input.style.display != 'none';
                } else if (ctrl.editingItem.input) {
                    if (ctrl.editingItem.input.style) {
                        isEditing = ctrl.editingItem.input.style.display != 'none';
                    }
                }
            }

            if (!isSelectAll && !ctrl.currentInput && !this.editingChildItem && (this != ctrl.editingItem || !isEditing)) {
                //show add sub link
                item.addSubLink.style.display = '';
                item.addSubLink.style.visibility = '';
            }

            if (this.isCollapsed) {
                this.itemInfo.style.display = 'none';
                this.itemInfo.style.visibility = 'hidden';
                this.collapseLink.style.display = '';
                this.collapseLink.style.visibility = '';
            }

            this.hideRemoveButton();
            if (e && e.target && e.target.tagName) {

                if (e.target.tagName == 'DIV' && e.target.removeButton) {
                    e.target.removeButton.style.display = 'block';
                    e.target.removeButton.style.visibility = '';
                } else {
                    var ele = $(e.target.parentNode).closest('div').length > 0 ? $(e.target.parentNode).closest('div')[0] : null;

                    if (ele && ele.tagName.toUpperCase() == 'DIV' && ele.removeButton) {
                        ele.removeButton.style.display = 'block';
                        ele.removeButton.style.visibility = ''
                    }
                }
            }



        };

        item.onmouseleave = function () {
            if (this.getListChildItems().length > 0) {
                this.hideAddSubLink();
            }

            if (this.isCollapsed) {
                this.itemInfo.style.display = '';
                this.itemInfo.style.visibility = '';
                this.collapseLink.style.display = 'none';
                this.collapseLink.style.visibility = 'hidden';
            }
            this.hideRemoveButton();
        };
    };

    ctrl.loadActionButton = function (obj) {
        var backButton = obj.querySelector('a[class*="back-btn"]');
        var closeButton = obj.querySelector('a[class*="close-btn"]');
        var doneButton = obj.querySelector('a[class*="done-btn"]');

        if (backButton) {
            obj.backButton = backButton;
            backButton.parent = obj;
            backButton.root = obj.root;
            backButton.onclick = function () {
                if (this.root.currentParentItem && this.root.currentParentItem.currentInput) {
                    this.root.hideValueSuggestion();
                    this.root.currentParentItem.currentInput.value = '';
                    this.root.currentParentItem.currentInput.setAttribute('data-keyword', '');
                    this.root.currentParentItem.currentInput.focus();
                    this.root.currentParentItem.toggleHiddenInputField();
                } else {
                    // current is multiselect
                    if (this.root.currentAddingMultiSelectItem) {
                        $(this.root.currentAddingMultiSelectItem).remove();
                        this.root.currentAddingMultiSelectItem = null;
                    }

                    this.parent.resetValue();
                    this.root.events.addItemCallback(true);
                    this.root.reset();
                    this.root.inputText.focus();
                }
            };
        }

        if (closeButton) {
            obj.closeButton = closeButton;
            closeButton.parent = obj;
            closeButton.root = obj.root;
            closeButton.onclick = function () {
                ctrl.toggleDisplay(false);
                if (this.root.editingItem) {
                    if (ctrl.editingItem.editingChildItem && ctrl.editingItem.editingChildItem.input) {
                        switch (ctrl.editingItem.editingChildItem.input.type) {
                            case 'MultiSelection':
                            case 'MultiChoose':
                            case 'AutoComplete':
                                if (ctrl.checkRequiredMerchant() && ctrl.editingItem.editingChildItem.input.keyword == "Merchant") {
                                    ctrl.reset();
                                    return;
                                }
                                var value = ctrl.editingItem.editingChildItem.getAttribute('data-value');
                                if (!value || value.split(',').length == 0) {
                                    ctrl.removeItemNotKeepVal(ctrl.editingItem);
                                }

                                ctrl.reset();
                                break;
                            default:
                                ctrl.reset();
                        }
                    } else if (ctrl.editingItem.input) {
                        switch (ctrl.editingItem.input.type) {
                            case 'MultiSelection':
                            case 'MultiChoose':
                            case 'AutoComplete':
                                var value = ctrl.editingItem.getAttribute('data-value');
                                if (!value || value.split(',').length == 0) {
                                    ctrl.removeItemNotKeepVal(ctrl.editingItem);
                                }
                                ctrl.reset();
                                break;
                            default:
                                ctrl.reset();
                        }
                    }
                }
                else if (ctrl.currentParentItem && ctrl.currentParentItem.currentInput) {
                    switch (ctrl.currentInput.type) {
                        case 'MultiSelection':
                        case 'MultiChoose':
                        case 'AutoComplete':
                            var value = ctrl.currentParentItem.fakeDisplayMultipleSelection.getAttribute('data-value');
                            if (!value || value.split(',').length == 0) {
                                ctrl.currentParentItem.currentInput.value = '';
                                ctrl.currentParentItem.toggleHiddenInputField();
                                ctrl.currentParentItem.removeInputField();
                                ctrl.reset();
                            } else if (value.split(',').length > 0) {
                                // Add child item
                                var displayVal = ctrl.currentParentItem.fakeDisplayMultipleSelection.displayValue.textContent;
                                var displayValReal = ctrl.currentParentItem.fakeDisplayMultipleSelection.getAttribute('data-display-value-real');
                                var displayAutoComplete = ctrl.currentParentItem.fakeDisplayMultipleSelection.getAttribute('data-display-autocomplete');

                                ctrl.displayValue(displayVal, value, displayValReal, displayAutoComplete);
                            }
                            break;
                        default:
                            ctrl.currentParentItem.currentInput.value = '';
                            ctrl.currentParentItem.toggleHiddenInputField();
                            ctrl.currentParentItem.removeInputField();
                            ctrl.reset();
                    }

                }
                else if (ctrl.currentInput) {
                    ctrl.reset();
                }
            };
        }

        if (doneButton) {
            obj.doneButton = closeButton;
            doneButton.parent = obj;
            doneButton.root = obj.root;
            doneButton.onclick = function () {
                this.parent.updateSelectionValue();
            };
        }

        var sugguestValueTitle = obj.querySelector('span[class*="title"]');

        if (sugguestValueTitle) {
            obj.sugguestValueTitle = sugguestValueTitle;
            obj.sugguestValueTitle.style.display = 'none';
            obj.sugguestValueTitle.style.visibility = 'hidden';
        }

        // Show hide back button and show hide title suggest
        obj.toggleEditMode = function (isEditing) {
            obj.isEditMode = isEditing;
            if (isEditing && sugguestValueTitle) {
                obj.sugguestValueTitle.style.display = '';
                obj.sugguestValueTitle.style.visibility = '';
                if (obj.backButton) {
                    obj.backButton.style.display = 'none';
                    obj.backButton.style.visibility = 'hidden';
                }
            } else {
                obj.sugguestValueTitle.style.display = 'none';
                obj.sugguestValueTitle.style.visibility = 'hidden';
                if (obj.backButton) {
                    obj.backButton.style.display = '';
                    obj.backButton.style.visibility = '';
                }
            }
        }
    };

    ctrl.bindSuggestItems = function () {
        var curr = this.currentInput;
        if (curr.isGetDataSource) {
            var data = {};
            data["page"] = advancedFilter.currentFilter._sectionName;
            data["type"] = curr.type;
            data["key"] = curr.keyword;
            data["subValue"] = curr.getAttribute('data-parent-key');
            $.ajax({
                type: "post",
                url: rootURL + "AdvancedFilterPage.aspx/GenerateItemSource",
                async: false,
                data: JSON.stringify(data),
                contentType: "application/json",
                dataType: "json",
                success: function (result) {
                    ctrl.setContentItems(curr, result.d);
                    curr.refreshSuggestValueList();
                },
            });
        }
    };

    ctrl.validationControl = function () {
        var isValid = true;
        var currentInput = this.editingItem && this.editingItem.editingChildItem ? this.editingItem.editingChildItem.input : this.currentInput;
        var keyword = currentInput.keyword;
        $(currentInput).find('.error').html('');

        var validation = $(this.validationContainer).find('div[data-validation-key="' + keyword + '"]');
        var validationItems = $(validation).find('div');
        if (currentInput.type == "OrderBy") {
            var val = currentInput.orderCombo.val();
            if ((val == '' || val == null) && validationItems[0] != undefined) {
                ctrl.showHideValidation(true, currentInput, currentInput.orderCombo, null, validationItems[0].innerText)
                isValid = false;
            }
        }
        else {
            // Find all input item
            var inputItems = $(currentInput).find('input[type="text"]:enabled');
            ctrl.showHideValidation(false, currentInput);

            inputItems.each(function () {
                var item = this;
                for (var k = 0; k < validationItems.length; k++) {
                    var controlID = validationItems[k].getAttribute('data-control-id-to-validate');
                    if (item.getAttribute('name') == controlID) {
                        var objectControlID = validationItems[k].getAttribute('data-object-control-id-to-validate');
                        var controlValue = item.value.trim(), objControl = '', objectControlValue = '', valFunc = '';

                        if (objectControlID != "") {
                            objControl = $(currentInput).find('input[type="text"][name="' + objectControlID + '"]')[0];
                            objectControlValue = objControl.value.trim();
                        }

                        valFunc = "aperia.searchControl.validate." + validationItems[k].getAttribute('data-rule');
                        valFunc = eval(valFunc + '(controlValue, objectControlValue)');
                        if (!valFunc) {
                            ctrl.showHideValidation(true, currentInput, item, objControl, validationItems[k].innerText)
                            isValid = false;
                            break;
                        }
                    }
                }
            });
        }
        return isValid;
    };

    ctrl.showHideValidation = function (isShow, elm, item, objectItem, msg) {
        var elmError = $(elm).find('.error');
        var lstLabel = $(elm).find('label[for]');
        if (isShow) {
            elmError.removeClass('hide');
            var error = document.createElement('div');
            error.className = 'error-item';
            error.textContent = msg;
            $(elmError).append(error);

            for (var i = 0; i < lstLabel.length; i++) {
                if (lstLabel[i].getAttribute('for') == item.getAttribute('name')
                    || (objectItem != "" && lstLabel[i].getAttribute('for') == objectItem.getAttribute('name'))) {
                    $(lstLabel[i]).addClass('error-label');
                }
            }
        } else {
            for (var i = 0; i < lstLabel.length; i++) {
                $(lstLabel[i]).removeClass('error-label');
            }
            if (!elmError.hasClass('hide')) {
                elmError.removeClass('hide');
                elmError.html('');

            }
        }
    };

    ctrl.checkRequiredMerchant = function (obj) {
        if (obj == undefined) obj = ctrl;
        var parentKey = $(obj.displayContainer).find(".parent-item").attr("item-search-name");
        merchantItem = obj.getFilterItem("Merchant", parentKey);
        if (merchantItem) {
            var dateTime = $(obj.displayContainer).find("div[item-search-name=DateTime]").attr("data-value");
            var isDateRange = dateTime != undefined ? dateTime.indexOf("DateRange") >= 0 : false;

            return isDateRange;
        }
        else {
            return false;
        }
    };

    ctrl.showAvaiableDateFilter = function (reportDate) {
        var msgItem = $('#msg-date-available');
        if (reportDate == null || reportDate == "") {
            msgItem.addClass('hide');
            return;
        }

        var fDate = new Date(reportDate);
        var tDate = new Date(uxAdvancedFilter_DateAvaiableFilter);
        if (fDate < tDate) {
            msgItem.removeClass('hide');
            var msg = msg_DateAvailableFilter.replace('{0}', uxAdvancedFilter_DateAvaiableFilter);
            msgItem.text(msg);
        } else {
            msgItem.addClass('hide');
        }
    };

    ctrl.checkHasValueMerchant = function (obj) {
        if (obj.checkRequiredMerchant(obj)) {
            var val = $(obj.displayContainer).find("div[item-search-name=Merchant]").attr("data-value");
            return !(val == null || val == "" || val == undefined);
        } else return true;
    };

    ctrl.setContentItems = function (curr, data) {
        if (curr.isGetDataSource) {
            switch (curr.type) {
                case "OrderBy":
                    var content = $(curr).find('div[data-combo="true"]');
                    content[0].innerHTML = data;
                    break;
                case "MultiChoose":
                    $(curr).find("div.data-multi-item")[0].innerHTML = data;
                    break;
                case "AutoComplete":
                    $(curr).find('[data-id="autocomplete"]')[0].innerHTML = data;
                    break;
                case "MultiSelection": {
                    var content = $(curr).find("div.items");
                    content[0].innerHTML = '';
                    if (data === '') {
                        content[0].innerHTML = '';
                    } else {
                        var itemsData = JSON.parse(data);

                        content.css('height', '200px');
                        var loadingHtml = '<div class="loading-bg" style="background: white; z-index: 100; display: flex; justify-content: center; align-items: center;"><div class="loader-adv-control"></div></div>';
                        content.append(loadingHtml);

                        var itemsHtml = [];
                        itemsData.forEach(function (element) {
                            var itemHtml = '<div value="' + element.DataKey + '" keyword="' + element.DataKey + '" data-display-value="' + element.DataItem + '">'
                                + '<div class="checkbox">'
                                + '<label>'
                                + '<input type="checkbox" name="' + element.DataKey + '" value="' + element.DataKey + '" display-value="' + element.DataItem + '" />'
                                + '<span>' + element.DataItem + '</span>'
                                + '</label>'
                                + '</div>'
                                + '</div>';
                            itemsHtml.push(itemHtml);
                        });

                        content.append(itemsHtml.join(''));
                        var timeout = 0;
                        if (itemsData.lenght > 100) {
                            timeout = 2000;
                        }

                        setTimeout(function () {
                            content.css('height', '');
                            content.find('.loading-bg').remove();
                        }, timeout);
                    }
                    break;
                }
                default:
                    var content = $(curr).find("div.items");
                    content[0].innerHTML = data;
            }
        }
    };

    ctrl.suggestionItemClick = function (item) {
        clearTimeout(ctrl.hideTimer);
        ctrl.editingItem = null;
        var value = item.getAttribute('data-display-value') + ':';
        if (ctrl.isAddSubItem && ctrl.currentParentItem) {
            ctrl.currentParentItem.currentInput.value = value;
            ctrl.currentParentItem.currentInput.setAttribute('data-keyword', item.getAttribute('keyword'));
            ctrl.currentParentItem.currentInput.onkeyup();
        } else {
            ctrl.inputText.value = value;
            ctrl.inputText.setAttribute('data-keyword', item.getAttribute('keyword'));
            ctrl.inputText.onkeyup();
        }
    };

    ctrl.replaceSpecialChar = function (text) {
        // Fix bug value include character '
        if (text && text.indexOf(ctrl.constants.charQuoteEncode) != -1)
            text = text.replace(ctrl.constants.charQuoteEncode, "'");

        return text;
    };

    ctrl.refreshSuggestFieldList();

    return ctrl;
};

aperia.searchControl.ajaxFieldCallback = function (sender) {
    aperia.searchControl.current.refreshSuggestFieldList();
    aperia.searchControl.current.suggestField();
    aperia.searchControl.current.ajaxRequestor = null;
};
aperia.searchControl.ajaxValueCallback = function (sender) {
    aperia.searchControl.current.currentInput.messageItem = null;
    aperia.searchControl.current.currentInput.refreshSuggestValueList();
    aperia.searchControl.current.suggestValue();
    aperia.searchControl.current.ajaxRequestor = null;
}
aperia.searchControl.dateSelectedCallback = function (isSelected, date, sender) {
    var value = sender.selectedDates.display('MM/dd/yyyy');
    if (aperia.searchControl.current.inputValue) {
        if (aperia.searchControl.current.inputValue.startsWith('before')) {
            value = 'before ' + value;
        }
        else if (aperia.searchControl.current.inputValue.startsWith('after')) {
            value = 'after ' + value;
        }
    }
    var inputVal = aperia.searchControl.current.inputField + ':' + value;
    aperia.searchControl.current.isSelectedDate = true;//fix for IE
    if (aperia.searchControl.current.isAddSubItem && aperia.searchControl.current.currentParentItem) {
        aperia.searchControl.current.inputText.value = inputVal;
        aperia.searchControl.current.currentParentItem.setValueChildItemInput(inputVal);
    } else if (aperia.searchControl.current.editingItem && sender.mode == aperia.datePickerWidget.MODE_DATERANGE) {
        if (sender.selectedDates.to != null) {
            var strValue = aperia.searchControl.validate.correctInputDate(value);
            $(aperia.searchControl.current.editingItem).attr('data-display-value', strValue);
            $(aperia.searchControl.current.editingItem).attr('value', strValue);
            aperia.searchControl.current.addItem(aperia.searchControl.current.editingItem);
            aperia.searchControl.current.isKeyHandled = true;
            aperia.searchControl.current.inputText.onkeyup();
        }
    }
    else {
        aperia.searchControl.current.inputText.value = inputVal;
        aperia.searchControl.current.inputText.focus();
    }

    if (sender.mode == aperia.datePickerWidget.MODE_SINGLE || (sender.mode == aperia.datePickerWidget.MODE_DATERANGE && sender.selectedDates.to != null)) {
        aperia.searchControl.current.inputText.onkeydown({ keyCode: 59 });
    }
    aperia.searchControl.current.toggleDisplay(false);
}

aperia.searchControl.searchHandler = function (input) {

    if (typeof input == "undefined")
        return false;
    try {
        //IE
        var keyboardEvent = document.createEvent("KeyEvent");
    } catch (e) {
        //Orther Browser
        var keyboardEvent = document.createEvent("KeyboardEvent");
    }

    var initMethod = typeof keyboardEvent.initKeyboardEvent !== 'undefined' ? "initKeyboardEvent" : "initKeyEvent";
    keyboardEvent[initMethod](
        "keypress", // event type : keydown, keyup, keypress
        true, // bubbles
        true, // cancelable
        window, // viewArg: should be window
        false, // ctrlKeyArg
        false, // altKeyArg
        false, // shiftKeyArg
        false, // metaKeyArg
        13, // keyCodeArg : unsigned long the virtual key code, else 0
        0 // charCodeArgs : unsigned long the Unicode character associated with the depressed key, else 0
    );
    // set Key for Chrome
    Object.defineProperty(keyboardEvent, 'keyCode', { get: function () { return 13; } });
    Object.defineProperty(keyboardEvent, 'charCode', { get: function () { return this.charCodeVal; } });
    //call event keypress
    input.dispatchEvent(keyboardEvent);
}
aperia.searchControl.validate =
{
    helper: {
        convertKeyCode: function (e) {
            var _to_ascii = {
                '188': '44',
                '109': '45',
                '190': '46',
                '191': '47',
                '192': '96',
                '220': '92',
                '222': '39',
                '221': '93',
                '219': '91',
                '173': '45',
                '187': '61', //IE Key codes
                '186': '59', //IE Key codes
                '189': '45' //IE Key codes
            };

            var shiftUps = {
                "96": "~",
                "49": "!",
                "50": "@",
                "51": "#",
                "52": "$",
                "53": "%",
                "54": "^",
                "55": "&",
                "56": "*",
                "57": "(",
                "48": ")",
                "45": "_",
                "61": "+",
                "91": "{",
                "93": "}",
                "92": "|",
                "59": ":",
                "39": "\"",
                "44": "<",
                "46": ">",
                "47": "?"
            };

            var c = e.which;

            //normalize keyCode 
            if (_to_ascii.hasOwnProperty(c)) {
                c = _to_ascii[c];
            }

            if (!e.shiftKey && (c >= 65 && c <= 90)) {
                c = String.fromCharCode(c + 32);
            } else if (e.shiftKey && shiftUps.hasOwnProperty(c)) {
                //get shifted keyCode value
                c = shiftUps[c];
            } else {
                c = String.fromCharCode(c);
            }

            return true;
        },
        //Date
        validDate: function (date) {
            var matches = (/^([0-9]{2}|[1-9]{1})\/([0-9]{2}|[1-9]{1})\/([0-9]{4})$/.exec(date));
            if (matches == null) return false;

            var d = matches[2], m = matches[1] - 1; y = matches[3];
            var composedDate = new Date(y, m, d);
            return composedDate.getDate() == d && composedDate.getMonth() == m && composedDate.getFullYear() == y;
        },
        validSingleDate: function (date, mode) {
            //mode has dataas: date=1,time=2
            mode = mode || 1;

            var arr = aperia.searchControl.validate.helper.truncateString(date).split(' ');
            if (arr.length > 2)
                return false;

            var date = '';
            switch (arr[0]) {
                case 'before':
                    date = arr[1];
                    break;
                case 'after':
                    date = arr[1];
                    break;
                default:
                    date = arr[0];
            }
            var valid = false;
            if (mode == 1)//for date
                valid = aperia.searchControl.validate.helper.validDate(date);
            if (mode == 2)//for time
                valid = aperia.searchControl.validate.helper.validTime(date);
            return valid;
        },
        validRangeDate: function (date, mode) {
            //mode has dataas: date=1,time=2
            mode = mode || 1;
            var arr = aperia.searchControl.validate.helper.truncateString(date).split(' ');
            if (arr.length != 3)
                return false;

            var valid = false;
            if (mode == 1)//for date
                valid = aperia.searchControl.validate.helper.validDate(arr[1]) && aperia.searchControl.validate.helper.validDate(arr[2]);
            if (mode == 2)//for time
                valid = aperia.searchControl.validate.helper.validTime(arr[1]) && aperia.searchControl.validate.helper.validTime(arr[2]);
            return valid;
        },
        //Time
        validTime: function (time) {
            var result = false, m;
            m = time.match(/^([0-9]{2}|[1-9]{1})\:([0-9]{2}|[0-9]{1})$/);

            if (m == null) return false;
            if (Number(m[1]) > 24 || Number(m[1]) < 0 || Number(m[2]) > 60 || Number(m[2] < 0) || (Number(m[1]) == 24 && Number(m[2]) > 0)) return false;
            var m1 = (m[1].length === 2 ? "" : "0") + m[1];
            var m2 = (m[2].length === 2 ? "" : "0") + m[2];

            result = m1 + ":" + m2;

            return result;
        },
        truncateString: function (value) {
            return value.replace(/\s\s+/g, ' ');
        },
    },
    noValidate: function () {
        return null;
    },
    validateDateFormat: function (value) {
        var arrValue = aperia.searchControl.validate.helper.truncateString(value).split(' ');
        var option = arrValue[0].toLowerCase();
        var isValid = true;
        switch (option) {
            case 'on':
                isValid = aperia.searchControl.validate.helper.validSingleDate(value);
                break;
            case 'before':
                isValid = aperia.searchControl.validate.helper.validSingleDate(value);
                break;
            case 'after':
                isValid = aperia.searchControl.validate.helper.validSingleDate(value);
                break;
            case 'range':
                isValid = aperia.searchControl.validate.helper.validRangeDate(value);
                break;
            default:
                isValid = aperia.searchControl.validate.helper.validSingleDate(value);
                break;
        }

        if (!isValid) {
            return "Invalid date format: [option(on|before|after) date] or [option(range) fromdate todate]";
        }

        return null;
    },
    validateTimeFormat: function (value) {
        var arrValue = aperia.searchControl.validate.helper.truncateString(value).split(' ');
        var option = arrValue[0].toLowerCase();
        var isValid = true;
        switch (option) {
            case 'on':
                isValid = aperia.searchControl.validate.helper.validSingleDate(value, 2);
                break;
            case 'before':
                isValid = aperia.searchControl.validate.helper.validSingleDate(value, 2);
                break;
            case 'after':
                isValid = aperia.searchControl.validate.helper.validSingleDate(value, 2);
                break;
            case 'range':
                isValid = aperia.searchControl.validate.helper.validRangeDate(value, 2);
                break;
            default:
                isValid = aperia.searchControl.validate.helper.validSingleDate(value, 2);
                break;
        }

        if (!isValid) {
            return "Invalid time format: [option(on|before|after) time] or [option(range) fromtime totime]";
        }
        return null;
    },
    validateDigitOnly: function (value, root) {
        var isValid = false;
        if (typeof root != 'undefined') {
            var selector = root.currentInput.querySelector('.items').querySelectorAll('div[keyword="' + value + '"]');
            isValid = selector.length > 0;
        }

        if (isValid)
            return null;
        else
            return 'Digits only, please input value as Yes/No';
    },
    ValidateNumber: function (value) {
        if (!isNaN(parseInt(value)) && isFinite(value) && value >= 0)
            return true;

        return false;
    },
    ValidateRequired: function (value) {
        if (value)
            return true;

        return false;
    },
    validateInteger: function (value) {
        if (!isNaN(parseInt(value, 10)) && isFinite(value) && parseFloat(value) === parseInt(value, 10) && value.indexOf(".") == -1)
            return true;
        return false;
    },
    NoSpecialCharacters: function (value) {
        var reg = new RegExp("^((?!(<[^ \t]))(?!(&#)).)*$");
        return reg.test(value);
    },

    CharactersLimit: function (value) {
        var reg = new RegExp("^[a-z0-9-A-Z ._\s-]*$");
        return reg.test(value);
    },
    selection: function (value, field) {
        var suggestName = field.getAttribute('values');
        var isValid = false;
        var selectedItems = aperia.searchControl.current.getSelectedItems(field.getAttribute('data-display-value'));
        $.each($(aperia.searchControl.current.valuesContainer).find('div[name="' + suggestName + '"] div.items div'), function () {
            // check valua is selected
            var isSelected = false;
            $.each(selectedItems, function () {
                if (this.value == value) {
                    isSelected = true;
                    return;
                }
            });

            if (!isSelected && this.getAttribute('value') == value) {
                // null is success
                isValid = null;
                return;
            }
        });
        return isValid;
    },
    multipleSelection: function (value, field) {
        var valueArr = value.split(',');
        if (valueArr.length == 0) return false;

        var suggestName = field.getAttribute('values');

        $.each($(aperia.searchControl.current.valuesContainer).find('div[name="' + suggestName + '"] div.items div'), function () {
            // check value is selected
            var isChecked = $(this).find('input[type="checkbox"]').is(':checked');
            if (isChecked) {
                var value = this.getAttribute('value');
                var filter = valueArr.filter(function (val) {
                    return val == value;
                });

                if (!filter || filter.length == 0) return false;
            }
        });

        return true;
    },
    correctInputDate: function (strDate) {
        var result = '';
        var strValues = strDate.split(/[ ,-]/);
        for (i = 0; i < strValues.length; i++) {
            if (strValues[i].length > 0) {
                result += strValues[i] + ' ';
            }
        }

        return result.trim();
    },
    ValidateGreaterThan: function (controlValue, objectControlValue) {
        return !isNaN(controlValue) && !isNaN(objectControlValue) && parseInt(controlValue) > parseInt(objectControlValue) && controlValue >= 0 && objectControlValue >= 0;
    },

    ValidateValidateDate: function (val) {
        return isDate(val);
    },

    ValidateCompareToday: function (val) {
        var currentDate = new Date();
        var selectedDate = new Date(val);
        if (selectedDate > currentDate)
            return false;
        return true;
    },
    ValidateCompare24Month: function (fromVal, toVal) {
        return true;
    },
    ValidateCompareToDateFromDate: function (fromVal, toVal) {
        var fDate = new Date(fromVal);
        var tDate = new Date(toVal);
        if (fDate > tDate)
            return false;
        return true;
    }
}
aperia.defaultValidate = function (value) {
    if (/(.*)[<>](.*)+$/.test(value))
        return 'No special character is accepted';
    return null;
}

//end search control


var core = {
    ui: {
        openSidebar: function (sidebarId) {
            var $sideBar = $("#" + sidebarId),
                $mainContainer = $("#" + PageContentClientID),
                scrollTop = $(window).scrollTop(),
                heightWelcombar = $(".navbar-inverse").outerHeight();
            /* check scroll at position bottom */
            if (heightWelcombar == undefined || heightWelcombar <= 0) {
                heightWelcombar = 30;
            }
            if ($sideBar.length) {
                var mainTop = $mainContainer.offset().top,
                    mainBot = mainTop + $mainContainer.outerHeight(),
                    winBot = scrollTop + $(window).height();

                var paddingBot = winBot - mainBot >= 0 ? winBot - mainBot : 0;
                var _heightFilter = 0;
                if (scrollTop - mainTop + heightWelcombar <= 0) {
                    _heightFilter = mainTop - scrollTop + paddingBot;
                    $sideBar.css({ height: 'calc(100vh - ' + _heightFilter + 'px)', top: 0 });
                    //$sideBar.css({ height: '100%', top: 0 });
                } else {
                    _heightFilter = heightWelcombar + paddingBot;
                    $sideBar.css({ height: 'calc(100vh - ' + _heightFilter + 'px)', top: (scrollTop - mainTop + heightWelcombar - 1) });
                    //$sideBar.css({ height: '100%', top: (scrollTop - mainTop + heightWelcombar - 1) });
                }

                $sideBar.addClass("is-open").css({ 'visibility': 'visible' });
                $('body').addClass('overflow-hidden');
                $("#popupContentOverlay").removeClass("hide").addClass("transparent-bg").click(function (e) {
                    core.ui.closeSidebar(sidebarId);
                    if (aperia.searchControl.current && aperia.searchControl.current.events && aperia.searchControl.current.events.onCloseCallBack) {
                        aperia.searchControl.current.events.onCloseCallBack(e);
                    }
                });
            }
        },
        closeSidebar: function (sidebarId) {
            var $sideBar = $("#" + sidebarId);

            if ($sideBar.length) {
                $('body').removeClass('overflow-hidden');
                $sideBar.removeClass("is-open");
                $("#popupContentOverlay").addClass("hide").removeClass("transparent-bg");
                setTimeout(function () {
                    $sideBar.css({ 'height': '', 'top': '', 'visibility': 'hidden' });
                }, 300);
            }
        },
    }
}
$.extend($.ui.autocomplete.prototype, {
    _renderMenu: function (ul, items) {
        $(ul).unbind("scroll");
        this._scrollMenu(ul, items);
    },
    _scrollMenu: function (ul, items) {
        var self = this;
        var maxShow = 300;
        var results = [];
        var pages = Math.ceil(items.length / maxShow);
        results = items.slice(0, maxShow);

        if (pages > 1) {
            $(ul).scroll(function () {
                if (isScrollbarBottom($(ul))) {
                    ++window.pageIndex;
                    if (window.pageIndex >= pages) return;

                    results = items.slice(window.pageIndex * maxShow, window.pageIndex * maxShow + maxShow);

                    // apend item to ul
                    $.each(results, function (index, item) {
                        self._renderItemData(ul, item);
                    });

                    // refresh menu
                    //self.menu.deactivate();
                    self.menu.refresh();

                    // size and position menu
                    ul.show();
                    self._resizeMenu();
                    // ul.position($.extend({
                    // of: self.element
                    // }, self.options.option));

                    if (self.options.autoFocus) {
                        self.menu.next(new $.Event("mouseover"));
                    }

                    updateWidthAutoComplete();
                }
            });
        }

        $.each(results, function (index, item) {
            self._renderItemData(ul, item);
        })
    }
});

function isScrollbarBottom(container) {
    var height = container.outerHeight();
    var scrollHeight = container[0].scrollHeight;
    var scrollTop = container.scrollTop();
    if (scrollTop >= scrollHeight - height) {
        return true;
    }
    return false;
}

function updateWidthAutoComplete() {
    var autocomplete = $(".ui-autocomplete");
    var autocompleteID = $("#autocompleteID");
    var oldLeft = autocompleteID.offset().left;
    var oldWidth = $("#autocompleteID").width();
    autocomplete.css("left", oldLeft);
    autocomplete.css("width", oldWidth);

    // Set max-height
    var totalHeight = $("div[data-selector*='content-select']").outerHeight();
    var displayHeight = $("#displayList").outerHeight();
    var top = $(".chosen-container").outerHeight();
    var height = 200;
    height = totalHeight - displayHeight - top - 30;

    autocomplete.css({ "max-height": height * 0.45 + "px" })
}
