function UpdateDefaultCustomView(customViewID, e) {
    $('.RadGrid :input[type="radio"]').each(function (index) {
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
    parent.HidePopupModal();
}

function doOpenSubPopup(encodeURL) {
    registerCloseSubModalEvent();
    return parent.ShowPopupModalChild(2, encodeURL, 'auto');
}

function doOpenEditSubPopup(encodeURL) {
    registerCloseSubModalEvent();
    return parent.ShowPopupModalChild(2, encodeURL, 'auto');
}

function doOpenEditFullViewPopup(encodeURL) {
    registerCloseSubModalEvent();
    return parent.ShowPopupModalChild(2, encodeURL, 'auto');
}

function registerCloseSubModalEvent() {
    parent.master_closeModalEvent = function () {
        document.getElementById(Risk_RebindCustomViewsGrid).click();
        parent.OnSubMitDataMesssage = function () {
            document.getElementById(uxDeleteCustomViewID).click();
        }
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
function doOpenEditMessagePopup(encodeURL, cusstomview) {
    $("#" + hddDeleteValueID).val(cusstomview);
    registerCloseSubModalEvent();
    parent.OnSubMitDataMesssage = function () {
        document.getElementById(uxDeleteCustomViewID).click();
    }
    return parent.ShowPopupModalChild(3, encodeURL, 'auto');
}
parent.OnSubMitDataMesssage = function () {
    document.getElementById(uxDeleteCustomViewID).click();
}

$(document).ready(function () {
    $("#" + panel_uxDisplayViewType + ' :input[type="radio"]').on('change', function (e) {
        document.getElementById(Risk_RebindCustomViewsGrid).click();
    });
})