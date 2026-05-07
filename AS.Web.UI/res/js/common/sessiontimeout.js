//session time out
var mins = -1;
var timeOutThread = null;
var isExpiredSession = false;

function doOpenTimeoutWarning() {
    var urlPage = 'freeaccess/SessionWarning.aspx';

    var sUserAgent = window.navigator.userAgent;
    var iWidth = 497;
    var iHeight = 320;
    if (/Chrome[\/\s](\d+\.\d+)/.test(sUserAgent)) {
        iWidth = 507;
        iHeight = 325;
    } else if (/Firefox[\/\s](\d+\.\d+)/.test(sUserAgent)) {
        iWidth = 497;
        iHeight = 325;
    }


    doOpenPopupWindow(rootURL + urlPage, iWidth, iHeight);
}
function showTimeout() {
    /*
    doOpenTimeoutWarning();
    doResetTimeOut();
    */
    if (isExpiredSession == true) {
        return;
    }   
    if (mins == -1) {
        mins = 0;
        timeOutThread = setTimeout("showTimeout()", 900000); //15 minutes - 900000 - temporary 5m
        
    }
    else {
        mins++;
        if (mins > 0) {
            checkTimeOut(function (isActive) {
                if (!isActive) {                    
                    doOpenTimeoutWarning();                    
                }
                mins = -1;
                showTimeout();
            });            
        }
    }    
}

function checkTimeOut(callback) {
    if (typeof rootURL === "undefined" || rootURL == null || rootURL == "")
        callback(false);
    else {
        var url = rootURL + "freeaccess/ManageSessionTimeout.aspx/IsActive";
        $.ajax({
            type: "POST",
            url: url,
            data: "",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            cache: false,
            async: true,
            success: function (response) {
                callback(response.d);
            }
        });
    }
}


function doResetTimeOut() {
    if (isIframeSupported == 'True')
        return;

    try {
        //Find down parent high level
        var realParent = parent;
        var realWindow = window;
        var loopCounter = 0;
        var maxLoops = 100;

        while (realWindow !== realParent.window && loopCounter < maxLoops) {
            realWindow = realParent.window;
            realParent = realParent.parent;
            loopCounter++;
        }

        if (!realParent.opener) {
            //Only one windows
            realParent.clearTimeout(realParent.timeOutThread);
            realParent.mins = -1;
            realParent.showTimeout();
        } else {
            //Find down opener high level
            var realOpener = realParent.opener;
            var seenWindows = new Set();
            while (realOpener && !seenWindows.has(realOpener) && loopCounter < maxLoops) {
                seenWindows.add(realOpener);
                realOpener = realOpener.opener;
                loopCounter++;

                if (seenWindows.has(realOpener)) {
                    break;
                }
            }
            realOpener.clearTimeout(realOpener.timeOutThread);
            realOpener.mins = -1;
            realOpener.showTimeout();
        }
    } catch (err) {
        realParent.clearTimeout(realParent.timeOutThread);
        realParent.mins = -1;
        realParent.showTimeout();
    }
}
/*------------------------------------------------------*/
var cmin = 2;
var csec = 0;
var timeout=null;
var submitTimeoutButton = null;
var displayTimePanel = null;
function countDown()
{            
    csec--;
    if(csec==-1)
    {
        csec=59;  
        cmin--;          
    }            
    document.getElementById('cidTime').innerHTML = getTimeDisplay(cmin,csec);            
    if( (cmin == 0) && (csec == 0) )
    {
        timeUp();
    }
    else
    {
        timeout = setTimeout('countDown()', 1000);	
    }
    setTimeout('window.focus()',1000);            
}

function getTimeDisplay(min, sec)
{
    var display;            
    if(sec<10)
        display= '0' + min + ' : ' + '0' +sec;
    else
        display= '0' + min + ' : ' +sec;
    
    return (display);
}

function closeSessionWarningWindow()
{        	        	      
    self.close();
    doResetTimeOut();
    return false;
}

function SSOLogout() {
    //Fix bugs: Hide confirm popup close browser on IE
    var obj = window.self;
    obj.open('', '_self', '');
    obj.close();
    //self.close();
}

function timeUp()
{
    $.removeCookie('FilterCollapseState', { path: '/' }); //Remove cookie for collapse state when user logout
    
    doOpenTimeoutPopup(false, 'ExpiredSession.aspx');
    document.getElementById(submitTimeoutButton).click();     	
}
function doReturnLoginPage(login_url){
    self.close();
	window.opener.window.location.href=login_url;
}


function checkSessionTimeout() {
    switch (window.logoutOpt) {
        //case '1': //session timeout normal
        //    doOpenTimeoutPopup();
        //    break;
        case '2': //session timeout jumpsite
            doOpenTimeoutPopup(true);
            break;
        case '3': //jumpsite log out
            setCookie('logout_opt', '0');
            self.close();
            break;
        case '4':
            var obj = window.self;
            obj.open('', '_self', '');
            obj.close();
            break;
    }
}

function doOpenTimeoutPopup(sj, url) {
    setCookie('logout_opt', '0');

    var sUserAgent = window.navigator.userAgent;
    var iWidth = 475;
    var iHeight = 240;
    if (/Chrome[\/\s](\d+\.\d+)/.test(sUserAgent)) {
        iWidth = 495;
        iHeight = 245;
    } else if (/Firefox[\/\s](\d+\.\d+)/.test(sUserAgent)) {
        iWidth = 475;
        iHeight = 245;
    }
    if (url != null && url.length > 0) {
        doOpenPopupWindow(url, iWidth, iHeight);
    }
    else {
        doOpenPopupWindow('freeaccess/ExpiredSession.aspx', iWidth, iHeight);
    }
    if (sj) {
        self.close();
    }
}
function doOpenPopupWindow(url, width, height) {
     
    var widthPage = screen.width; //width resolution
    var heightPage = screen.height; //heigth resolution
    var leftpage = (widthPage - 400) / 2;
    var toppage = (heightPage - 200) / 2;
    
    window.open(url, '_blank', 'width=' + width + ', height=' + height + ', toolbar=no, location=no, directories=no, status=no, menubar=no, scrollbars=no, resizable=no, left=' + leftpage + ',top=' + toppage);
}
