function ajaxRequestStart(sender, args) {
    if (typeof (UxExporter_OnRequestStart) == 'function')
        UxExporter_OnRequestStart(sender, args);
}
function ajaxResponseEnd(sender, args) {
    if (typeof (UxExporter_OnResponseEnd) == 'function')
        UxExporter_OnResponseEnd(sender, args);
}

function deleteStatementReport(fileName, actionID) {   
    showRadConfirm("confirm", mesConfirmDelete, function (arg) {
        if (arg) {
            doDelete(actionID);
            return false;
        }
    }, mesConfirm, mesOk, mesCancel);
    return false;
}

function doDelete(actionID) {
    document.getElementById(actionID).click();
    //document.getElementById(btnRefreshGrid).click();
}

function doDownLoad() {
    document.getElementById(btnDownLoad).click();
}

function onDownLoadAll() {
    checkFileAvailable();
    return false;
}

// Refresh statement grid after 1 minute
function onRefreshStatementGrid() {
    var intervalRefresh = setInterval(function () {
        document.getElementById(btnRefreshGrid).click();
        clearInterval(intervalRefresh);
    }, 60000);
}

function checkFileAvailableSuccess(response) {
    if (response !== "") {

        showRadConfirm("confirm", response, function (arg) {
            if (arg) {
                doDownLoad();
            }
        }, mesWarning, mesYes, mesNo);
    } else {
        doDownLoad();
    }

    return false;
}

function showMessageError() {
    setTimeout(function () {
        alert(messageError);
    }, 500);    
}

