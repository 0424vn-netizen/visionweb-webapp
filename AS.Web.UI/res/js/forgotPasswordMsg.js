function OpenForgotWndMsg() {
    GetRadWindow().BrowserWindow.doOpenForgotWndMsg();
}

function GetRadWindow() {
    var oWindow = null;
    if (window.radWindow) oWindow = window.radWindow;
    else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
    return oWindow;
}

function CloseMsgBox() {
    GetRadWindow().close();
    parent.window.dialogs[0].close();
}