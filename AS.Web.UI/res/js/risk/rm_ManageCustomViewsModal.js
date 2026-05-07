
function UpdateDefaultCustomView(customViewID, e) {
    $(':input[type="radio"]').each(function (index) {
        this.checked = false;
    })
    e.checked = true;
    PageMethods.UpdateDefaultCustomView(customViewID);

};

function GetManageCustomViewsGrid() {
    parent.ClosePopupModal(2);
    document.getElementById(Risk_RebindCustomViewsGrid).click(); 
}

function CloseManageCustomModal() {

    if (parent.refreshCustomView) {
        parent.refreshCustomView();
    }
    parent.HidePopupModal();
}

function confirmDelete() {
    if (!confirm(message_confirm)) {
        return false;
    }
}

function doOpenSubPopup() {
    registerCloseSubModalEvent();
    return parent.ShowPopupModalChild(1, 'rm_CreateCustomViewModal.aspx', 'auto');
}

function doOpenEditSubPopup(encodeURL) {
    registerCloseSubModalEvent();
    return parent.ShowPopupModalChild(1, encodeURL, 'auto');
}

function doOpenEditFullViewPopup(encodeURL) {
    registerCloseSubModalEvent();
    return parent.ShowPopupModalChild(1, encodeURL, 'auto');
}

function registerCloseSubModalEvent() {
    parent.master_closeModalEvent = function () {
        document.getElementById(Risk_RebindCustomViewsGrid).click();

    }
}

function registerCloseEventModal() {
    // reset close event for parent modal
    parent.master_closeModalEvent = function () {

        if (typeof (window.parent.GetButtonRebind) == "function") {
            var btnRebind = window.parent.GetButtonRebind();
            if (!!btnRebind) {
                btnRebind.click();
            }
        }
        CloseManageCustomModal();
    }

}