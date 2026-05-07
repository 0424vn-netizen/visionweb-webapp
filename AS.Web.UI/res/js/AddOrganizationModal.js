function GetOrgs(e) {
    $("#" + btnSubmit).click();
    ClosePopupModal();
    return false;
}

function checkHasItem() {
    var grid = $find(uxAvailableOrganization_ClientID);
    return grid.get_masterTableView().get_dataItems().length > 0;
}

//43410 - Error when creating multiple users on the FE
function onSaveOrgSuccess() {
    var oWnd = GetRadWindow();
    var dialogA = oWnd.get_windowManager().getWindowByName("RadWindow1");
    dialogA.get_contentFrame().contentWindow.reBindOrgs();
}