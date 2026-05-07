var maxColSelect = 50;

if (rm_MCF_CustomColumnsModal_PageMode === "RiskReport") {
    maxColSelect = 20;
}
else if (rm_MCF_CustomColumnsModal_PageMode === "TransactionHistory") {
    maxColSelect = 16;
}

var initObject = {
    MaxColumn: maxColSelect,
    MinColumn: 1
};
$(document).ready(function () {
    InitMouseEnter();
    CustomColumnModal.initOptions(initObject);
    CountDisplayedColumn();
});

function InitMouseEnter() {
    $("li[class='rlbItem']").on("mouseenter", HideDragDroppedIcon);
};

function CountDisplayedColumn() {
    var displayedCol = $('#' + uxRightGridListBox + " .rlbList li").length;
    if (displayedCol) {
        document.getElementById(ucCountColumnSelected).innerText = displayedCol;

        let maxCol = maxColSelect;
        if (displayedCol == maxCol) {
            var moveToRight = $('.MoveToRight');
            //moveToRight.removeAttr('onclick');
            moveToRight.removeProp('onclick');
            var linkIcon = moveToRight.find('.add-col-item');
            linkIcon.removeClass('add-col-item');
            linkIcon.addClass('add-col-item-disable');
        }
    }
}

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

function clientBeforeShow(sender, eventArgs) {
    var x = ($("span[data-tooltip=" + sender.get_id() + "]:hover")).length;
    if (x == 0) {
        eventArgs.set_cancel(true);
    }
}
parent.OnSubMitDataMesssage = function () {
    document.getElementById(js_button_uxSaveEdit).click();
}
function hoverUnused(e) {
    $("#RadToolTipWrapper_" + $(e).attr("data-tooltip")).hide();

}
CustomColumnModal = (function () {
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
        let maxCol = maxColSelect;
        if (!!displayedCol) {
            var items = displayedCol.get_items();
            if (!!items) {
                if (items.get_count() < 1) {
                    alert(js_TextShowMinColumn);
                    return false;
                }
                if (items.get_count() > maxCol) {
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
        parent.ClosePopupModal(2);
        return false;
    };
    var SubmitColumnConfigurationModal = function () {
        var radWindows = parent.GetRadWindowManager().get_windows();
        for (var i = 0; i < radWindows.length; i++) {
            if (radWindows[i].GetUrl()) {
                if (radWindows[i].GetUrl().indexOf('rm_MCF_ManageCustomModal') != -1 || radWindows[i].GetUrl().indexOf('rm_MCF_ManageCustomViewsModal') != -1) {
                    if (radWindows[i].get_contentFrame().contentWindow.GetManageCustomViewsGrid)
                        radWindows[i].get_contentFrame().contentWindow.GetManageCustomViewsGrid();
                }
            }
        }
    };
    var SubmitTransactionHistoryColumnConfigurationModal = function () {
        var radWindows = parent.GetRadWindowManager().get_windows();
        for (var i = 0; i < radWindows.length; i++) {
            if (radWindows[i].GetUrl()) {
                if (radWindows[i].GetUrl().indexOf('rm_MCF_ManageCustomModal') != -1 || radWindows[i].GetUrl().indexOf('rm_MCF_ManageTransactionHistoryCustomViewsModal') != -1) {
                    if (radWindows[i].get_contentFrame().contentWindow.GetTransactionHistoryManageCustomViewsGrid)
                        radWindows[i].get_contentFrame().contentWindow.GetTransactionHistoryManageCustomViewsGrid();
                }
            }
        }
    };
    var uxGridListBox_OnClientLoad = function (sender, arg) {
        var items = sender.get_items();
        if (!!items && items.get_count() > 0) {
            var i = 0;
            items.forEach(function (item) {
                item.bindTemplate();

                var idTooltip = item.get_element().getAttribute("idtooltip"),
                    tagertID = item.get_element().getElementsByClassName("col-name")[0].getAttribute("id");

                if (idTooltip == null || idTooltip.trim() == '') {
                    idTooltip = $(item.get_element()).find('.col-name:first').data('tooltip');
                }

                setTooltipForGird(idTooltip, tagertID);

                $(item.get_element()).removeClass("even-row");
                if (i % 2 != 0) {
                    $(item.get_element()).addClass("even-row");
                }
                i++;
            })
        }

        sender.commitChanges();
    };

    var setTooltipForGird = function (idTooltip, targetID) {

        var toolTip = $find(idTooltip);
        if (toolTip) {
            toolTip.hide();
            toolTip.set_targetControlID("");
            toolTip.set_targetControlID(targetID);
        }
    }

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
                //return false;
                CustomColumnModal.uxGridListBox_OnClientLoad(unusedCol, null);
                RegisterRadTooltip(selectedItem.get_attributes('IdTooltip')._data["IdTooltip"], colName.attr("id"), "right");

            }
        }
        CountDisplayedColumn();
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
                CustomColumnModal.uxGridListBox_OnClientLoad(unusedCol, null);
                RegisterRadTooltip(selectedItem.get_attributes('IdTooltip')._data["IdTooltip"], colName.attr("id"), "left");
            }
        }
        CountDisplayedColumn();
    };

    var RegisterRadTooltip = function (id, element, position) {
        var toolTip = $find(id);
        toolTip.hide();
        toolTip.set_targetControlID("");
        toolTip.set_targetControlID(element);
        if (position) {
            if (position == "left") {
                toolTip.set_offsetX(30);
                toolTip.set_position(Telerik.Web.UI.ToolTipPosition.MiddleRight);
            } else if (position == "right") {
                toolTip.set_offsetX(43);
                toolTip.set_position(Telerik.Web.UI.ToolTipPosition.MiddleLeft);
            }
        }
    }

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

        if ((!!htmlElement && htmlElement.outerHTML.indexOf("uxRightGrid") >= 0) && htmlElement.outerHTML.indexOf("uxLeftGrid") >= 0) {
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

    var uxGridListBox_OnClientDropping = function (sender, event) {
        var itemDestination = (event.get_destinationItem());
        if (itemDestination) {
            if (itemDestination._attributes._data.IsDefault) {
                event.set_cancel(true);
            }
        }

    }

    var uxGridListBox_OnClientDropped = function (sender, event) {
        HideDragDroppedIcon();
        var domEvent = event.get_item();

        if (!!domEvent && !!domEvent.get_element()) {
            RegisterRadTooltip(domEvent.get_attributes('IdTooltip')._data["IdTooltip"],
                domEvent.get_element().getElementsByClassName("col-name")[0].getAttribute("id"));

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
    var changeCustomView = function () {
        if (parent.changeCustomView)
            parent.changeCustomView();
    }
    function doOpenEditMessagePopup(encodeURL) {
        return parent.ShowPopupModalChild(3, encodeURL, 'auto');
    }

    return {
        ValidateDisplayedColumn: ValidateDisplayedColumn,
        CloseColumnConfigurationModal: CloseColumnConfigurationModal,
        SubmitColumnConfigurationModal: SubmitColumnConfigurationModal,
        SubmitTransactionHistoryColumnConfigurationModal: SubmitTransactionHistoryColumnConfigurationModal,
        uxGridListBox_OnClientLoad: uxGridListBox_OnClientLoad,
        MoveToRight: MoveToRight,
        MoveToLeft: MoveToLeft,
        uxGridListBox_OnClientDragging: uxGridListBox_OnClientDragging,
        uxGridListBox_OnClientDragStart: uxGridListBox_OnClientDragStart,
        uxGridListBox_OnClientDropped: uxGridListBox_OnClientDropped,
        uxGridListBox_OnClientDropping: uxGridListBox_OnClientDropping,
        initOptions: initOptions,
        info_closeModalEvent: info_closeModalEvent,
        changeCustomView: changeCustomView,
        doOpenEditMessagePopup: doOpenEditMessagePopup
    }
})()