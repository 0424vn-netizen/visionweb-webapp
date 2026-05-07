
$(document).ready(function () {
    InitMouseEnter();

});

function InitMouseEnter() {
    $("li[class='rlbItem']").on("mouseenter", HideDragDroppedIcon);
};

function HideDragDroppedIcon() {
    var displayedCol = $find(uxDispositionGridListBox);

    if (!!displayedCol) {
        var drag = $(displayedCol.get_element()).find(".hide-ico, .show-ico"),
            dragArray = Array.prototype.slice.call(drag);
        dragArray.forEach(function (item) {
            $(item).removeClass("hide-ico");
            $(item).removeClass("show-ico");
        });
    }
};

function refreshData() {
    document.getElementById(btnReFreshID).click();
}

function setNoDataFound(hasData) {
    if (hasData) {
        $("#listBoxDataFound").addClass("active");
    } else {
        $("#listBoxDataFound").removeClass("active");
    }
}

DispositionColumn = (function () {

    var isAllowDrag;

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

    var CreateCssClass = function (parentNode, className, cssText) {
        if (!parentNode || !className || !cssText) {
            return "";
        }
        var styleElement = document.createElement("style");
        styleElement.type = "text/css";
        styleElement.innerHTML = "." + className + "{" + cssText + "}";
        var modal = $("body");

        if (!!modal) {
            var parent = modal.find(".rlbDragClue");
            parent[0].appendChild(styleElement);
        }
        return className;
    };

    var uxGridListBox_OnClientDragging = function (sender, event) {
        var rlbGroupRights = document.getElementById(uxDispositionGridListBox),
            rlbDragClue = $(".rlbDragClue"),
            newStyle = "position: absolute; opacity:0.7; ";

        if (!!rlbGroupRights && !!rlbDragClue) {
            var rlbGroupRight = $(rlbGroupRights).offset();
            newStyle += "left:" + rlbGroupRight.left + "px !important; " + "width:" + $(rlbGroupRights).width() + "px !important;";
            var css = CreateCssClass(".rlbDragClue", "radlistbox-dragging-container", newStyle);
            $(rlbDragClue).addClass(css);
            $(rlbDragClue).find(".DroppedItem").addClass("show-ico")
        }

        if (!isAllowDrag) {
            event.set_cancel(true);
        }
        var htmlElement = event.get_htmlElement(),
            domEvent = event.get_domEvent();

        if ((!!htmlElement && htmlElement.outerHTML.indexOf("uxDispositionGrid") >= 0) && htmlElement.outerHTML.indexOf("uxLeftGrid") >= 0) {
            event.set_cancel(true);
        }
    };

    var uxGridListBox_OnClientDragStart = function (sender, event) {
        isAllowDrag = true;
        var domEvent = event.get_domEvent();

        if (!!domEvent && (domEvent.target.tagName.toLowerCase() != "a" || domEvent.target.className.trim() != "DragItem drag-col-item")) {
            isAllowDrag = false;
        }
    };

    var uxGridListBox_OnClientDropping = function (sender, event) {
        var itemDestination = (event.get_destinationItem());
        if (itemDestination) {
            var startus = ($(itemDestination._element).find(".disposition-col-status").find("span")[0].innerText);
            if (startus.toLowerCase() == "inactive") {
                event.set_cancel(true);
            }
        }
    }

    var uxGridListBox_OnClientReordered = function (sender, event) {
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
            var gridItems = ($find(uxDispositionGridListBox).get_items());
            var dispositionList = "";
            gridItems.forEach(function (item) {
                dispositionList += item.get_value() + ",";
            })
            dispositionList = dispositionList.slice(0, -1);

            $("#" + hdListDisposition_ClientID).val(dispositionList);
            $get(uxUpdatePosition_ClientID).click();
            //$("#" + uxUpdatePosition_ClientID).click();
        }
    };

    return {
        uxGridListBox_OnClientLoad: uxGridListBox_OnClientLoad,
        uxGridListBox_OnClientDragging: uxGridListBox_OnClientDragging,
        uxGridListBox_OnClientDragStart: uxGridListBox_OnClientDragStart,
        uxGridListBox_OnClientReordered: uxGridListBox_OnClientReordered,
        uxGridListBox_OnClientDropping: uxGridListBox_OnClientDropping
    }
})()