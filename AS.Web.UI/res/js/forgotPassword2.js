function OpenForgotWndMsg(url) {
    GetRadWindow().BrowserWindow.doOpenForgotWndMsg(url);
}
function GetRadWindow() {
    var oWindow = null;
    if (window.radWindow) oWindow = window.radWindow;
    else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
    return oWindow;
}

function ValidateAndAdjustModal() {
    var result = ValidateInput();
    if (!result) {
        setTimeout("AdjustModalSize();", 200);
    }
    return result;
}