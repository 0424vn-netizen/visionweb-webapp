function DoClose() {
    try {
        parent.ClosePopupModal(0);
        if (parent != null) {
            parent.doPostBack();
        }
    }
    catch (err) { }
}
function openManageUserModal(url) {
    parent.ShowPopupModalChild(1, url, 200, 200, 200, 200);
    return false;
}

parent.refreshGrid = function () {
    $('#' + Membership_uxRefreshGrid).click();
}

$telerik.$(document).ready(function () {
    // close button
    var oWnd = GetRadWindow();
    if (oWnd != null) {
        var oElement = $(oWnd.get_popupElement());
        var closeButton = $telerik.$(".rwCloseButton", oElement);
        closeButton.click(function () {
            DoClose();
        });
    }
});