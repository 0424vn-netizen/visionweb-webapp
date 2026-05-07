EditColumnModal = (function () {
    var defaultOptions = {
        AfterSubmited: undefined
    };
    var _options = {};
    var isAllowDrag;

    var CloseEditCustomViewModal = function () {
        parent.ClosePopupModal(1);
        //parent.master_closeModalEvent();
    };

    var ValidateDisplayedColumn = function () {
        var displayedCol = $find(uxRightGridListBox);
        if (!!displayedCol) {
            var items = displayedCol.get_items();
            if (!!items) {
                if (items.get_count() < 1) {
                    return false;
                }
            }
            return true;
        }
        return false;
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

    var RemoveItem = function (e) {
        if (!!ViewType && ViewType != "0") {
            var displayedCol = $find(uxRightGridListBox);
            if (displayedCol.get_items().get_count() == 1) {
                alert(js_TextShowMinColumn);
                return false;
            }
            var parentSpans = $(e).closest("li.rlbItem");
            if (!!parentSpans && !!displayedCol) {
                var colName = $(parentSpans[0]).find(".col-name:first");
                var selectedItem = displayedCol.findItemByText(colName[0].innerText);
                if (!!selectedItem) {
                    displayedCol.trackChanges();
                    displayedCol.get_items().remove(selectedItem);
                    displayedCol.commitChanges();
                    EditColumnModal.uxGridListBox_OnClientLoad(displayedCol, null);
                }
            }
        }
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
           // $(rlbDragClue).find(".DroppedItem").addClass("show-ico");
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
        var domEvent = event.get_item();
        if (!!domEvent && !!domEvent.get_element()) {
            var dragIconElement = $(domEvent.get_element()).find(".DragItem"),
                removeIconElement = $(domEvent.get_element()).find(".RemoveItem"),
                dropIconElement = $(domEvent.get_element()).find(".DroppedItem");
            //if (!!dragIconElement) {
            //    $(removeIconElement).addClass("show-ico");
            //    $(dragIconElement).addClass("show-ico");
            //}
            //if (!!dropIconElement) {
            //    //$(dropIconElement).addClass("show-ico");
            //}
        }
    };
    return {
        CloseEditCustomViewModal: CloseEditCustomViewModal,
        uxGridListBox_OnClientLoad: uxGridListBox_OnClientLoad,
        RemoveItem: RemoveItem,
        uxGridListBox_OnClientDragging: uxGridListBox_OnClientDragging,
        uxGridListBox_OnClientDragStart: uxGridListBox_OnClientDragStart,
        uxGridListBox_OnClientDropped: uxGridListBox_OnClientDropped,
        ValidateDisplayedColumn: ValidateDisplayedColumn,
    }
})()