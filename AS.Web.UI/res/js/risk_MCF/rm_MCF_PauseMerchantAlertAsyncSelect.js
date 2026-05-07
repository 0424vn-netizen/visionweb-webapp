
var pauseMerchantAsyncSelect = (function () {

    var $ = window.kendo.jQuery;

    // Multiselect store
    var storeKey = window.name + "_multiSelectStore";

    window.parent[storeKey] = window.valueStoreServer || window.parent[storeKey] || {};

    function getStore() {
        return window.parent[storeKey];
    }

    function setStore(value) {
        return window.parent[storeKey] = value;
    }


    function setValueToStore(index, value) {
        return window.parent[storeKey][index] = value;
    }


    // Multiselect events
    function onChange(sender) {
        jQuery = window.kendo.jQuery;

        var values = sender.get_value();
        var rowIndex = getSelectRowIndex(sender);
        var items = sender.get_kendoWidget().dataItems();

        var fullDataItems = values.map(function (value) {
            return items.find(e => e.DataKey === value)
        }).filter(Boolean).map(function (item) {
            return {
                DataKey: item.DataKey,
                DataText: item.DataText
            }
        });

        setValueToStore(rowIndex, fullDataItems);
    }

    function onSelect(sender, event) {

        var dataItem = event.get_dataItem();

        // Prevent click to select selected item
        if (isSelected(dataItem)) {
            event.set_cancel(true);
        }
    }

    function onDeselect(sender, event) {
        var dataItem = event.get_dataItem();

        // Prevent Enter to deselect disabled item
        if (isKeydownEvent() && isSelected(dataItem)) {
            event.set_cancel(true);
        }
    }

    function disableItems(sender) {
        var liElements = Array.from(sender.get_items());

        liElements.forEach(function (li) {
            var dataItem = getDataItemFromElement(sender, li);

            if (dataItem && dataItem.Disabled) {
                $(li).addClass('pointer-events-none')
            }
            else {
                $(li).removeClass('pointer-events-none')
            }
        });

    }

    function onDataBound(sender) {
        disableItems(sender);
    }

    function onOpen(sender) {
        jQuery = window.kendo.jQuery;
    }


    function onLoad(sender) {
        var rowIndex = getSelectRowIndex(sender);
        var selectedValues = getStore()[rowIndex] || [];

        if (selectedValues.length) {
            var arrayValues = Array.from(selectedValues.map(function (e) {
                return e.DataKey;
            }));

            sender.set_value(arrayValues);
        }

        pauseMerchantAlert.disableAllMerchantsViewMode();
    }

    function valueMapper(options) {
        var selectedIds = options.value;
        var dataItemsMap = getSelectedMap();

        var items = selectedIds.map(function (key) {
            return dataItemsMap[key];
        }).filter(Boolean);

        var mapped = items.map(function (item) {
            item.toJSON = function () {
                return item;
            }

            return item;
        });

        options.success(mapped);
    }

    // Custom events handlers
    $(document).on('click', '.remove-condition', function () {

        var $row = $(this).closest('table').closest('tbody tr');
        var rowIndex = $row.index();

        var store = getStore();
        var keys = Object.keys(store);
        var values = Object.values(store);
        var newData = {};

        keys.forEach(function (key, index) {
            if (index < rowIndex) {
                return newData[key] = values[index];
            }

            newData[key] = values[index + 1] || [];
        });

        setStore(newData);
    });

    // Before sending search ajax
    function onCustomParameter(sender, args) {
        if (sender._currentPageIndex === args.get_data().page)
            return;

        var postData = args.get_data();

        postData.selectedDataKeys = [];

        // used with POST request to pass in body
        args.set_parameterFormat(JSON.stringify({ customfilterstring: JSON.stringify(postData) }))
    }

    function onDataParse(sender, args) {
        var selectedMap = getSelectedMap();
        args._response.d.Data = args._response.d.Data.map(function (item) {
            var Disabled = !!selectedMap[item.DataKey];

            return Object.assign({}, item, { Disabled });
        });

        args.set_parsedData(args._response.d);
    }

    function getSelectedMap() {
        var selectedMap = Object.values(getStore()).flat().reduce(function (acc, item) {
            acc[item.DataKey] = item;
            return acc;
        }, {});

        return selectedMap;
    }

    function isSelected(dataItem) {
        var selectedMap = getSelectedMap();
        return !!selectedMap[dataItem.DataKey];
    }

    function getSelectRowIndex(sender) {
        var rows = sender.get_element().closest('table').closest('tbody tr');
        return $(rows).index();
    }

    function getDataItemFromElement(sender, li) {
        var kendoMultiselect = sender.get_kendoWidget();
        return kendoMultiselect.dataSource.getByUid(li.dataset.uid);
    }

    function isKeydownEvent() {
        const err = new Error();
        const stack = err.stack.trim();

        return stack && stack.includes('_keydown');
    }

    return {
        valueMapper,
        onCustomParameter,
        onOpen,
        onLoad,
        onChange,
        onSelect,
        onDeselect,
        onDataBound,
        onDataParse
    }
})();


// Expose functions for RadMultiSelect
Object.keys(pauseMerchantAsyncSelect).forEach(function (functionName) {
    window['pauseMerchantAsyncSelect_' + functionName] = pauseMerchantAsyncSelect[functionName];
});

// Expose global helpers
function isEmptyMerchants() {
    return $('#tbl-filter-date .k-multiselect .k-reset:empty').length > 0;
}

function disableAllMerchantSelects() {
    $('#tbl-filter-date .k-multiselect select').each(function () {
        var kendoMultiSelect = window.kendo.jQuery(this).data('kendoMultiSelect');

        if (!kendoMultiSelect) return;

        kendoMultiSelect.enable(false);
    });
}