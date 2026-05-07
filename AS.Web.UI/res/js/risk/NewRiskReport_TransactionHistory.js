function ajaxRequestStart(sender, args) {
    if (args.get_eventTarget().indexOf('uxExportTransactionHistory') != -1) {
        args.set_enableAjax(false);
    }
}

$(document).ready(function () {
    registerEventsForOrderByOptions();
    renderOrderByOptions();
});

function registerEventsForOrderByOptions(){
    $(".sort_ipmt .radio-option").each(function (index, element) {
        var $this = $(element);
        $this.on("click", function () {
            $(this).addClass("hide");
            $(this).parent().find(".radio-option").not($this).removeClass("hide");

        });
    });
}

function renderOrderByOptions() {
    $(".sort_ipmt .radio-option").each(function (index, element) {
        var $this = $(element);
        var orderby = $this.parent();
        
        if (!$(orderby).find("input[type='text']").val()) {
            $this.addClass("hide");
        }
        else {
            if ($this.find("input[type='radio']:checked").length > 0) {
                $this.addClass("hide");
            } else {
                $this.removeClass("hide");
            }
        }
    });
}

function uxOrderBy_ClientSelectedIndexChanged(sender, event)
{
    var senderId = sender.get_id(),
        $this = $("#" + senderId),
        cb = $find(senderId),
        listCb = cb.get_itemData();
        

    var listradio = $this.parents(".group-sort").find("input[type='radio']"),
            groupsort = $this.parents(".group-sort").find(".radio-option"),
            sortdefault;
    
    if (!cb.get_value())
    {
        groupsort.addClass("hide");
    }
    else
    {
        listCb.some(function (item) {
            if (item.value && item.value == cb.get_value()) {
                sortdefault = item.attributes.Default;
                return true;
            }
        });

        listradio.each(function (i, item_sort) {
            if (item_sort.value.toUpperCase().indexOf(sortdefault.toUpperCase()) > -1) {
                item_sort.checked = true;
                $(item_sort).parents(".radio-option").addClass("hide");
            }
            else {
                item_sort.checked = false;
                $(item_sort).parents(".radio-option").removeClass("hide");
            }
        });
    }
}