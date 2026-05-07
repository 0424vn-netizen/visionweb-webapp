var isRebindRole = false;

function doRebindUserRole() {
    var bt = document.getElementById(ManageRole_uxRebinData_ClientID);
    if (bt)
        bt.click();
}

function rebindRole() {
    HidePopupModal();
    doRebindUserRole();
    isRebindRole = true;
}

function Callback(sender, e) {
    UxExporter_OnResponseEnd(sender, e);
}

function WhenResponseEnd(sender, eventArgs) {
    ShowHideExportControl(MangaRole_uxReportGrid_ClientID);
}

function DeleteConfirmation() {
    if (confirm(ManageRoles_js_DeleteRole)) {
        return true;
    }
    else
        return false;
}

function PredefinedRole() {
    alert(ManageRole_PrefinedRole);
    return false;
}