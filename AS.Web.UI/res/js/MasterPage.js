$(document).ready(function() {   
    doResetTimeOut();
    
});

function showUnreadMessages() {
    ShowPopupModal(messageViewUnreadMessagesModal, 'auto');
}

function openPopupWindowOnMenu(e, url, windowName, iWidth, iHeight) {
    $("#" + uxIsWindowOpened).val("1");
             
    var width = screen.availWidth;
    var height = screen.availHeight - 40;
            
    if(iWidth) width = iWidth;
    else if(screen.availWidth > 1220) width = 1220;            
    if(iHeight) height = iHeight;
            
    var left = (screen.availWidth / 2) - (width / 2);
    // 56063: Resize pages of CaseManagement except administration page
    if (url.indexOf("JumpToCase") > -1) {
        if (url.indexOf("type=9") < 0) {
            width = screen.availWidth;
            height = screen.availHeight - 40;
            left = (screen.availWidth / 2) - (width / 2);
        }
    }
    var pwin = window.open(url, windowName, "scrollbars=1,menubar=0,toolbar=0,resizable=1, width=" + width.toString() + ",height=" + height.toString() + ",left=" + left.toString() + ",top=0,location=0", true);
           
    setTimeout(function(){ 
        if (pwin != null) {
            pwin.focus();
        }
    }, 100);            
    return false;
}
       
function openLogoutPopupWindow(url) {
    var width = 10;
    var height = 10;
    var left = -300;
    var top = -300;
    window.open(url, 'CaseManagement_Wnd', "scrollbars=1,menubar=0,toolbar=0,resizable=1, width=" + width.toString() + ",height=" + height.toString() + ",left=" + left.toString() + ",top=" + top.toString() + ",location=0", true);
    return false;
}
function doLogOff() {
    if (hasCMSession == 'true' || $("#" + uxIsWindowOpened).val() == "1") {
        //VW and CMS are sharing session, so not neccesary to reset session on CM
        //openLogoutPopupWindow(urlCMLogout);
    }
    $.removeCookie('FilterCollapseState', { path: '/' }); //Remove cookie for collapse state when user logout
    return true;
}

function customLoadingPanel_OnClientShowing(sender, args) {
    var updatedControlWrapper = args.get_updatedElement();//get reference to the updated control's wrapper element
    var loadingElement = args.get_loadingElement();//get reference to the loading panel's element
    
    //size and position the loading panel
    var divParent = $("div[id*=" + (args._updatedElement.id + 'Panel') + "]");
    loadingElement.style.width = divParent.width() + 15 + "px";
    loadingElement.style.left = divParent.offset().left + "px";
}

//Get local timezone
function getClientTimezone() {
    //Clear cookie
    setSecureCookie("ClientTimezone", -1);
    setSecureCookie("DayLightSaving", -1);
    //Create cookie
    var t = new Date().getTimezoneOffset() * -1;
    setSecureCookie("ClientTimezone", (t / 60 - GetDayLightSavingTime()));
    setSecureCookie("DayLightSaving", checkTimeZoneHaveDLST());
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

function readCookie(name) {
    var i, x, y, allCookies = document.cookie.split(';');
    for (i = 0; i < allCookies.length; i++) {
        x = allCookies[i].substr(0, allCookies[i].indexOf('='));
        y = allCookies[i].substr(allCookies[i].indexOf('=') + 1);
        x = x.replace(/^\s+|\s+$/g, '');
        if (x == name) {
            return unescape(y);
        }
    }
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


//47574 - Web Application Security Standard Upgrade - VisionWeb
function setSecureCookie(name, value) {
    var data = {};
    data["key"] = name;
    data["value"] = value;
    $.ajax({
        type: "post",
        url: "/Header.aspx/CreateCookie",
        async: false,
        data: JSON.stringify(data),
        contentType: "application/json",
        dataType: "json",
        success: function (res) {
            //console.log(res);
        },
        error: function (result) {
            //console.log(result.responseText);
        }
    })
}

$(document).ready(function () {
    if (isOpenRecurringSystemMessage == "True") {
        var widthPage = screen.width; 
        var heightPage = screen.height;
        var leftpage = (widthPage - 718) / 2;
        var toppage = (heightPage - 490) / 2;
        var url = rootURL + "freeaccess/RecurringSystemMessageModal.aspx";
        window.open(url, 'comingsoon', 'width=' + 718 + ', height=' + 490 + ', toolbar=no, location=no, directories=no, status=no, menubar=1, scrollbars=no, resizable=no, left=' + leftpage + ',top=' + toppage);
    }
});
