var IsStopProcess = false;
var isScroll = false;

Sys.Application.add_load(function (sender, args) {
    var textId = $("#lblAqId").html();
    if (textId == "" || textId == "0") {
        aqModule.HideBoder(1);
    } else {
        aqModule.HideBoder(0);
    }

    aqModule.onScrollToSelected(true);
    aqModule.onScrollToSelected(false);
});

var AutoQueueModule = function (options) {
    var self = this;
    self.doReload = function () {
        document.getElementById(btnDoReload).click();
    }

    self.onActiveRowClick=function() {
        aqActive = true;
        IsStopProcess = false;
        isScroll = false;
    }

    self.onInActiveRowClick = function () {
        aqActive = false;
        isScroll = false;
    }

    self.onScrollToSelected = function (isActived) {
        if (!isScroll) return;
        var grid = isActived ? $find(ucActivedGrid):$find(ucInActivedGrid);
        var scrollArea = document.getElementById(grid.get_element().id + "_GridData");
        var row = grid.get_masterTableView().get_selectedItems()[0];
        if (row) {
            var rowPos = row.get_element();
            var curPos = rowPos.offsetTop - scrollArea.scrollTop;
            if (curPos + rowPos.offsetHeight + 20 > scrollArea.offsetHeight) {
                scrollArea.scrollTop = scrollArea.scrollTop + (curPos + rowPos.offsetHeight - scrollArea.offsetHeight) + rowPos.offsetHeight;               
            } else {
                scrollArea.scrollTop = rowPos.offsetTop;
            }
        }
    }
   
    self.OpenAutoQueueModal = function (urlLink,isCreate) {        
        ShowPopupModal(urlLink, 'auto');
        if (isCreate) isScroll = true;
        return false;
    }

    self.OpenAqMsg = function (msgString) {
        showRadMessage("alert", msgString, null, MesWarning, 'auto');
    }

    self.HideBoder = function (isHide) {
        if (isHide == "1") {
            $("#" + ucDetail).removeClass("box-gray");
        } else {
            $("#" + ucDetail).addClass("box-gray");
        }
    }
}

function doReloadManageAutoQueue() {
    document.getElementById(btnDoReloadData).click();
}

