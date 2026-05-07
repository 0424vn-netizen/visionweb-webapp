var initObject = {
    MaxColumn: 14,
    MinColumn: 1
};
$(document).ready(function () {
    InitMouseEnter();
    CustomizeColumnModal.initOptions(initObject);
});

function InitMouseEnter() {
    $("li[class='rlbItem']").on("mouseenter", HideDragDroppedIcon);
};

function HideDragDroppedIcon() {
    var displayedCol = $find(uxRightGridListBox);
    var displayedColLeft = $find(uxLeftGridListBox);
    if (!!displayedCol) {
        if (displayedCol.get_items().get_count() >= initObject.MaxColumn) {
            var itemLeft = $(displayedColLeft.get_element()).find(".rlbTemplate");
            itemLeft.each(function () {
                var moveToRight = $(this).find('.MoveToRight');
                moveToRight.removeAttr('onclick');
                var linkIcon = moveToRight.find('.add-col-item');
                linkIcon.removeClass('add-col-item');
                linkIcon.addClass('add-col-item-disable');
                linkIcon.attr('title', js_TextShowNoteMaxColumn);
            });
        }
        var drag = $(displayedCol.get_element()).find(".hide-ico, .show-ico"),
            dragArray = Array.prototype.slice.call(drag);
        dragArray.forEach(function (item) {
            $(item).removeClass("hide-ico");
            $(item).removeClass("show-ico");
        });
    }
};
function CheckFullView() {
    var viewName = $get(txtNameView_Client_ID).value;
    return !(viewName.toLowerCase().trim() == full_View_Text);
};
parent.OnSubMitDataMesssage = function () {
    document.getElementById(js_button_uxSaveEdit).click();
}
CustomizeColumnModal = (function () {
    var defaultOptions = {
        MaxColumn: 0,
        MinColumn: 0
    };
    var info_closeModalEvent = function () {
        parent.GetManageCustomViewsGrid();
    }
    var _options = {};
    var isAllowDrag;
    var ValidateDisplayedColumn = function () {
        if (!ValidateInput()) {
            AdjustModalSize();
            return false;
        };
        var displayedCol = $find(uxRightGridListBox);
        if (!!displayedCol) {
            var items = displayedCol.get_items();
            if (!!items) {
                if (items.get_count() < 1) {
                    alert(js_TextShowMinColumn);
                    return false;
                }
                if (items.get_count() > 14) {
                    alert(js_TextShowMaxColumn);
                    return false;
                }
            }
            return true;
        }
        return false;
    };
    var CloseColumnConfigurationModal = function (isNeedConfirm) {
        if (isNeedConfirm == true) {
            if (!confirm(js_TextComfirmCancel)) {
                return false;
            }
        }
        parent.ClosePopupModal(1);
        return false;
    };
    var SubmitColumnConfigurationModal = function () {
        var radWindows = parent.GetRadWindowManager().get_windows();
        for (var i = 0; i < radWindows.length; i++) {
            if (radWindows[i].GetUrl() != null && radWindows[i].GetUrl().indexOf('rm_MCF_ManageCustomViewsModal') != -1) {
                if (radWindows[i].get_contentFrame().contentWindow.GetManageCustomViewsGrid)
                    radWindows[i].get_contentFrame().contentWindow.GetManageCustomViewsGrid();
            }
        }
        // parent.GetManageCustomViewsGrid(); 
    };
    var uxGridListBox_OnClientLoad = function (sender, arg) {
        var items = sender.get_items();
        if (!!items && items.get_count() > 0) {
            var i = 0;
            items.forEach(function (item) {
                item.bindTemplate();
                $(item.get_element()).removeClass("even-row");
                if (i % 2 != 0) {
                    $(item.get_element()).addClass("even-row");
                }
                i++;
            })
        }
        sender.commitChanges();
    };
    var MoveToRight = function (e) {
        var unusedCol = $find(uxLeftGridListBox),
            displayedCol = $find(uxRightGridListBox);
        if (displayedCol.get_items().get_count() >= _options.MaxColumn) {
            alert(js_TextShowMaxColumn);
            return false;
        }
        var parentSpans = $(e).closest("li.rlbItem");
        if (!!parentSpans && !!unusedCol && !!displayedCol) {
            var colName = $(parentSpans[0]).find(".col-name:first");
            var selectedItem = unusedCol.findItemByText(colName[0].innerText);
            if (!!selectedItem) {
                TransferItem(selectedItem, unusedCol, displayedCol);
                CustomizeColumnModal.uxGridListBox_OnClientLoad(unusedCol, null);
            }
        }
    };
    var MoveToLeft = function (e) {
        var unusedCol = $find(uxLeftGridListBox),
            displayedCol = $find(uxRightGridListBox);
        if (displayedCol.get_items().get_count() == _options.MinColumn) {
            alert(js_TextShowMinColumn);
            return false;
        }
        var parentSpans = $(e).closest("li.rlbItem");
        if (!!parentSpans && !!unusedCol && !!displayedCol) {
            var colName = $(parentSpans[0]).find(".col-name:first");
            var selectedItem = displayedCol.findItemByText(colName[0].innerText);
            if (!!selectedItem) {
                TransferItem(selectedItem, displayedCol, unusedCol);
                CustomizeColumnModal.uxGridListBox_OnClientLoad(unusedCol, null);
            }
        }
    };

    var TransferItem = function (item, source, target) {
        var itemsTarget = target.get_items();
        target.trackChanges();
        target.transferItem(item, source, target);
        item.bindTemplate();
        target.commitChanges();
        InitMouseEnter();
    };
    var CreateCssClass = function (parentNode, className, cssText) {
        if (!parentNode || !className || !cssText) {
            return "";
        }
        var styleElement = document.createElement("style");
        styleElement.type = "text/css";
        styleElement.innerHTML = "." + className + "{" + cssText + "}";
        var modal = $("body.body-modal");

        if (!!modal) {
            var parent = modal.find("div.rlbDragClue");
            parent[0].appendChild(styleElement);
        }
        return className;
    };
    var uxGridListBox_OnClientDragging = function (sender, event) {
        var rlbGroupRights = document.getElementById(uxRightGridListBox),
            rlbDragClue = $(".rlbDragClue"),
            newStyle = "position: absolute; opacity:0.7; ";
        if (!!rlbGroupRights && !!rlbDragClue) {
            var rlbGroupRight = $(rlbGroupRights).offset();
            newStyle += "left:" + rlbGroupRight.left + "px !important; " + "width:" + $(rlbGroupRights).width() + "px !important;";
            var css = CreateCssClass("div.rlbDragClue", "radlistbox-dragging-container", newStyle);
            $(rlbDragClue).addClass(css);
            $(rlbDragClue).find(".DroppedItem").addClass("show-ico")
        }

        if (!isAllowDrag) {
            event.set_cancel(true);
        }
        var htmlElement = event.get_htmlElement(),
            domEvent = event.get_domEvent();
        if ((!!htmlElement && htmlElement.outerHTML.indexOf("uxLeftGrid") >= 0)) {
            event.set_cancel(true);
        }
    };
    var uxGridListBox_OnClientDragStart = function (sender, event) {
        isAllowDrag = true;
        var domEvent = event.get_domEvent();
        if (!!domEvent && (domEvent.target.tagName.toLowerCase() != "a" || domEvent.target.className != "DragItem drag-col-item")) {
            isAllowDrag = false;
        }
    };
    var uxGridListBox_OnClientDropped = function (sender, event) {
        HideDragDroppedIcon();
        var domEvent = event.get_item();
        if (!!domEvent && !!domEvent.get_element()) {
            var dragIconElement = $(domEvent.get_element()).find(".DragItem"),
                removeIconElement = $(domEvent.get_element()).find(".MoveToLeft"),
                dropIconElement = $(domEvent.get_element()).find(".DroppedItem");
            if (!!dragIconElement) {
                $(removeIconElement).addClass("show-ico");
                $(dragIconElement).addClass("show-ico");
            }
            if (!!dropIconElement) {
                //$(dropIconElement).addClass("show-ico");
            }
        }
    };

    function initOptions(options) {
        _options = $.extend({}, defaultOptions, options);
    }
    function doOpenEditMessagePopup(encodeURL) {
        return parent.ShowPopupModalChild(3, encodeURL, 'auto');
    }
    return {
        ValidateDisplayedColumn: ValidateDisplayedColumn,
        CloseColumnConfigurationModal: CloseColumnConfigurationModal,
        SubmitColumnConfigurationModal: SubmitColumnConfigurationModal,
        uxGridListBox_OnClientLoad: uxGridListBox_OnClientLoad,
        MoveToRight: MoveToRight,
        MoveToLeft: MoveToLeft,
        uxGridListBox_OnClientDragging: uxGridListBox_OnClientDragging,
        uxGridListBox_OnClientDragStart: uxGridListBox_OnClientDragStart,
        uxGridListBox_OnClientDropped: uxGridListBox_OnClientDropped,
        initOptions: initOptions,
        info_closeModalEvent: info_closeModalEvent,
        doOpenEditMessagePopup: doOpenEditMessagePopup
    }
})()