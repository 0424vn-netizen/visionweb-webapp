function EnterSignIn(e) {
    if ({ 13: 1 }[e.which || e.keyCode]) {
        e.preventDefault ? e.preventDefault() : e.returnValue = false;
        e.cancel = true;
        document.getElementById(uxLogin_ClientID).click();
        return true;
    }
}
var dialogs = null;
function doOpenForgetPassword() {
    if (dialogs == null) dialogs = $find(uxWindow_ClientID).get_windows();
    dialogs[0].setUrl('freeaccess/forgotPassword1.aspx');
    dialogs[0].show();
    return false;
}
function doOpenForgotWndMsg(url) {
    dialogs = $find(uxWindow_ClientID).get_windows();
    dialogs[3].setUrl('freeaccess/' + url);
    dialogs[3].show();
    return false;
}
checkSessionTimeout();
$(function () {
    $('[autofocus]:not(:focus)').eq(0).focus();
});
//Old browser warning
$('.old-browser-warning button').click(function () {
    $('.old-browser-warning-container').hide();
});

//Get local timezone
function getClientTimezone() {
    var t = new Date().getTimezoneOffset() * -1;
    $("#" + uxHddClientTimezone_ClientID).val((t / 60 - GetDayLightSavingTime()));
    $("#" + uxHddDayLightSaving_ClientID).val(checkTimeZoneHaveDLST());
}
function createCookie(name, value, days) {
    var expires = "";
    if (days) {
        var date = new Date();
        date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
        expires = "expires=" + date.toGMTString();
    }
    document.cookie = name + "=" + value + ";" + expires;
}

Date.prototype.stdTimeZoneOffset = function () {
    var jan = new Date(this.getFullYear(), 0, 1);
    var jul = new Date(this.getFullYear(), 6, 1);
    return Math.max(jan.getTimezoneOffset(), jul.getTimezoneOffset());
}
Date.prototype.dts = function () {
    return this.getTimezoneOffset() < this.stdTimeZoneOffset();
}
function GetDayLightSavingTime() {
    var today = new Date();
    if (today.dts())
        return 1;
    return 0;
}

function checkTimeZoneHaveDLST() {
    var today = new Date();
    var jan = new Date(today.getFullYear(), 0, 1);
    var jul = new Date(today.getFullYear(), 6, 1);
    if (jan.getTimezoneOffset() == jul.getTimezoneOffset())
        return 0;
    return 1;
}

getClientTimezone();

function performCheck() {
    if (!Page_ClientValidate("loginUserValidationGroup")) {
        if ($("input[type='password']").length > 0)
            $("input[type='password']")[0].value = "";
        $('.login-alert').html(msg_ValidationError)
        return false;
    }
    $('.login-alert').html("")
    return true;
}

function getBrowserName() {
    var browser = (function (agent) {
        switch (true) {
            case agent.indexOf("edge") > -1: return "edge";
            case agent.indexOf("edg") > -1 && !!window.chrome: return "edge";
            case agent.indexOf("opr") > -1 && !!window.opr: return "opera";
            case agent.indexOf("chrome") > -1 && !!window.chrome: return "chrome";
            case agent.indexOf("trident") > -1: return "ie";
            case agent.indexOf("firefox") > -1: return "firefox";
            case agent.indexOf("safari") > -1: return "safari";
            default: return "other";
        }
    })(window.navigator.userAgent.toLowerCase());
    return browser;
}

$(document).ready(function () {
    var browserName = getBrowserName();
    if (browserName !== 'edge' && browserName !== 'chrome' && browserName !== 'firefox') {
        $('.old-browser-warning-container').removeClass('hide');
    }
});

function closeUnsupportModal() {
    $('.old-browser-warning-container').addClass('hide');
}