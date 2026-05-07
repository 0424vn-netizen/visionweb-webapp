
parent.applyCustomView = function (customViewId) {
    $("#" + rm_MCF_CustomView_hdCustomViewID).val(customViewId);
}

parent.registerCloseCustomViewModalEvent = function () {
    parent.master_closeModalEvent = function (e) {
        if (rm_MCF_CustomView_PageSection == 'SecurityReport') {
            SecurityRebind_CustomViewChange();
        } else {
            $get(rm_MCF_CustomView_btnRebind).click();
            //$('#' + rm_MCF_CustomView_btnRebind).click();
        }
        parent.master_closeModalEvent = null;
    }
}

function doOpenCustomViewPopup(url) {
    parent.registerCloseCustomViewModalEvent();
    return doOpenNewPopup(url);
}

