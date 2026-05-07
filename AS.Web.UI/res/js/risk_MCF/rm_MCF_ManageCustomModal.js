function UpdateDefaultCustomView(e, customViewID, pageMode, assignmentID) {
    $('.RadGrid :input[type="radio"]').each(function (index) {
        this.checked = false;
    })
    e.checked = true;
    $("#" + rm_MCF_ManageCustomModal_hddCustomViewDefault).val(customViewID);
    parent.applyCustomView(customViewID);
    if (pageMode == "DetectionQueue")
        PageMethods.UpdateDefaultCustomView(customViewID, pageMode, assignmentID);
};
function GetManageCustomViewsGrid() {
    parent.ClosePopupModal(2);
}
function onCloseModal() {
    if (rm_MCF_ManageCustomModal_PageMode == "DetectionQueue")
        parent.HidePopupModal();
    else
        parent.ClosePopupModal(1);
}
function doOpenSubPopup(encodeURL) {
    registerCloseSubModalEvent();
    if (encodeURL) {
        return parent.ShowPopupModalChild(2, encodeURL, 'auto');
    }
    return parent.ShowPopupModalChild(2, 'rm_MCF_CreateManageCustomViewModal.aspx', 'auto');
}
function doOpenEditSubPopup(encodeURL) {
    registerCloseSubModalEvent();
    return parent.ShowPopupModalChild(2, encodeURL, 'auto');
}
function doOpenEditFullViewPopup(encodeURL) {
    registerCloseSubModalEvent();
    return parent.ShowPopupModalChild(2, encodeURL, 'auto');
}
var isdelete = false;
function registerCloseSubModalEvent() {
    parent.master_closeModalEvent = function () {
        var display = $(parent.document.getElementById("RadWindowWrapper_ctl00_RadWindow3")).css('display');
        if (display != undefined && display != 'none') {
            return;
        }
        if (!isdelete)
            document.getElementById(Risk_RebindCustomViewsGrid).click();
        isdelete = false;
        parent.master_closeModalEvent = null;
        parent.OnSubMitDataMesssage = function () {
            isdelete = true;
            document.getElementById(uxDeleteCustomViewID).click();
        }
    }
}
parent.OnSubMitDataMesssage = function () {
    isdelete = true;
    document.getElementById(uxDeleteCustomViewID).click();
}
$(document).ready(function () {
    $("#" + panel_uxDisplayViewType + ' :input[type="radio"]').on('change', function (e) {
        document.getElementById(Risk_RebindCustomViewsGrid).click();
    });
})
manageCustomModal = {
    doOpenEditMessagePopup: function (encodeURL, customview) {
        $("#" + hddDeleteValueID).val(customview);
        registerCloseSubModalEvent();
        return parent.ShowPopupModalChild(3, encodeURL, 'auto');
    }
}