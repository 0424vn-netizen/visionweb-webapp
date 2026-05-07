var dialogs = null;
function doRebindUserList() {
    var bt = document.getElementById(uxRebinData_ClientID);
    if (bt)
        bt.click();
}

function Callback(sender, e) {
    UxExporter_OnResponseEnd(sender, e);
}
function ajaxRequestStart(sender, args) {
    var target = sender.__EVENTTARGET;
    if (target.indexOf("imgExcel") >= 0 || target.indexOf("imgCSV") >= 0 || target.indexOf("imgPDF") >= 0)
    {
        args.set_enableAjax(false);
    }
}
function WhenResponseEnd(sender, eventArgs) {
    var gridID = uxReportGrid_ClientID;
    ShowHideExportControl(gridID);
}
function doSetStatus(user, currentStatus, systemId, isSecondaryUser) {
    var mess = "";
    if (currentStatus == "Y") {
        mess = ManageUsers_js_Deactive;
    }
    else {
        mess = ManageUsers_js_Active;
    }
    var answer = confirm(mess);
    if (answer) {
        document.getElementById(hddSystemId_ClientID).value = systemId;
        document.getElementById(hddIsSecondaryUser_ClientID).value = isSecondaryUser;
        document.getElementById(hddActiveUserID_ClientID).value = user;
        document.getElementById(uxActiveDecactive_ClientID).click();
    }
    return false;
}
function DoCloseCreatedUser() {
    doRebindUserList();
    return HidePopupModal();
}