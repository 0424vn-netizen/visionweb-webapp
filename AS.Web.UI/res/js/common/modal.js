$(document).ready(function () {
    var isOpera = !!window.opera || navigator.userAgent.indexOf(' OPR/') >= 0;
    var isFirefox = typeof InstallTrigger !== 'undefined';
    var isIE = /*@cc_on!@*/false || !!document.documentMode;
    var isChrome = !!window.chrome && !isOpera;
});

function ShowPopupModal(url, width, height, posX, posY) {
    OpenPopupModal(0, url, width, height, posX, posY);
    return false;
}
function HidePopupModal() {
    ClosePopupModal(0);
    return false;
}

function HideSiteJumpModal() {
    HidePopupModal();
}

var PopupModal = null;
var coreSub, coreSubUrl, coreSubWidth, coreSubHeight, coreSubPosX, coreSubPosY;
function ShowPopupModalChild(index, url, width, height, posX, posY) {
    coreSub = index;
    coreSubUrl = url;
    coreSubWidth = width;
    coreSubHeight = height;
    coreSubPosX = posX;
    coreSubPosY = posY;
    setTimeout('DoCheatOpenSubModalChild()', 500);
    return false;

}

function DoCheatOpenSubModalChild() {
    OpenPopupModal(coreSub, coreSubUrl, coreSubWidth, coreSubHeight, coreSubPosX, coreSubPosY);
}
var count = 0;
var left = 0;
function FixPageSizeModal(dialog) {
    var bounds = dialog.getWindowBounds();
    if (count == 0) {
        left = bounds.x;
        count++;
    }
    var leftPre = left == bounds.x ? bounds.x : bounds.x - 9;
    dialog.MoveTo(leftPre, bounds.y);
}
function FixPageSizeModalInModal(dialog) {
    var bounds = dialog.getWindowBounds();
    var top = 80;
    if (count == 0) {
        left = bounds.x;
        count++;
    }
    var leftPre = left == bounds.x ? bounds.x : bounds.x + 9;
    dialog.MoveTo(leftPre, top);
}
function FixPageSize(dialog) {
    var widthPage = screen.width; //width resolution
    var heightPage = screen.height; //heigth resolution
    widthPage = widthPage * 0.75;
    heightPage = heightPage * 0.75;
    dialog.SetWidth(widthPage);
    dialog.SetHeight(heightPage);
    //Get scroll of parent page to 0,0
    window.parent.scroll(0, 0);

    var bounds = dialog.getWindowBounds();
    var top = 130;
    var left = bounds.x;
    dialog.MoveTo(left, top);

}

function HidePopupModalChild(index) {
    ClosePopupModal(index);
    return false;
}

//*************************Get Width, Height of Screen*************************

function getScreenWidth() {
    return document.compatMode == 'CSS1Compat' && !window.opera ? document.documentElement.clientWidth : document.body.clientWidth;
}

function getScreenHeight() {
    return document.compatMode == 'CSS1Compat' && !window.opera ? document.documentElement.clientHeight : document.body.clientHeight;
}

//*************************End Get Width, Height of Screen*************************

//*************************Popup Modal*************************
//Get Rad Window
function GetRadWindow() {
    var oWindow = null;
    if (window.radWindow) oWindow = window.radWindow; //Will work in Moz in all cases, including clasic dialog
    else if (window.frameElement != null && window.frameElement.radWindow) oWindow = window.frameElement.radWindow; //IE (and Moz az well)
    return oWindow;
}
function closeWindow() {
    try {
        GetRadWindow().Close();
        return false;
    } catch (ex) { };
}
function GetPopupModal(index) {
    if (windowManagerID == '')
        windowManagerID = 'ctl00_ctl00_uxWindowManager';
    var windowManager = $find(windowManagerID);
    windowManager = windowManager.get_windows();
    var modalWindow = null;
    if (windowManager != null) {
        modalWindow = windowManager[index];
    }
    return modalWindow;
}
/*
function SizeToFit() {
    var modal = GetRadWindow();
    if (modal != null) {
        modal.autoSize(true);
        var obj = modal._iframe;
        if (modal.isIE) {
            var myNav = navigator.userAgent.toLowerCase();
            if (myNav.indexOf('msie 8') > 0) {
                var iDoc = obj.contentWindow.document ? obj.contentWindow.document : obj.contentDocument;
                modal._iframe.style.height = (iDoc.body.clientHeight) + 'px';
            }
        }

    }
}
*/

function OpenPopupModal(index, url, width, height, posX, posY) {
    if (windowManagerID == '')
        windowManagerID = 'ctl00_ctl00_uxWindowManager';
    var windowManager = $find(windowManagerID);
    windowManager = windowManager.get_windows();
    if (windowManager != null) {
        var modalWindow = windowManager[index];
        modalWindow.allowAutoSize = false;
        if (typeof width == 'string') {

            modalWindow.setUrl(url);
            modalWindow.show();
            if (width == 'max') {
                modalWindow.maximize();
            }
            else if (width == 'auto') {
                modalWindow.allowAutoSize = true;
            }
        }
        else {
            //Set width, height of modal window
            var scrW = getScreenWidth();
            var scrH = getScreenHeight();
            var sizeRate = 0.95;
            if (index == 2)
                sizeRate = 0.9;
            else if (index == 3)
                sizeRate = 0.85;
            else if (index == 4)
                sizeRate = 0.8;
            if (width == null)
                width = scrW * sizeRate;
            if (height == null)
                height = scrH * sizeRate;
            modalWindow.setSize(width, height);
            //Set position(x,y) of modal window
            if (posX == null && posY == null) {
                posX = (scrW - width) / 2 + Math.max(document.body.scrollWidth, document.documentElement.scrollWidth);
                posY = (scrH - height) / 2 + Math.max(document.body.scrollHeight, document.documentElement.scrollHeight);
            }
            modalWindow.MoveTo(posX, posX);

            modalWindow.setUrl(url);
            modalWindow.show();
        }
        count = 0;
    }
    // Add Class PopupModal
    $('body').addClass('popupmodal-open');
}
var windowManagerID = null;
//Fix page size for popup(radwindow)
function FixPageSizeWidthHeight(dialog, width, height) {
    dialog.SetWidth(width);
    dialog.SetHeight(height);
    //Get scroll of parent page to 0,0
    window.parent.scroll(0, 0);

    var bounds = dialog.getWindowBounds();
    var top = 80;
    var left = bounds.x;
    dialog.MoveTo(left, top);
}

function ClosePopupModal(index) {
    // Remove Class PopupModal
    var idPopupModalOne = $('#RadWindowWrapper_ctl00_RadWindow1');
    if ($(idPopupModalOne).css('display') == 'none') {
        $('body').removeClass('popupmodal-open');
    }
    if (index == null)
        GetRadWindow().Close();
    else {
        if (windowManagerID == '')
            windowManagerID = 'ctl00_ctl00_uxWindowManager';
        var windowManager = $find(windowManagerID);
        windowManager = windowManager.get_windows();
        if (windowManager != null)
            windowManager[index].close();
        else
            GetRadWindow().Close();
    }
    count = 0;
}

function PopupModalClose(sender, eventArgs) {
    // Remove Class PopupModal
    var idPopupModalOne = $('#RadWindowWrapper_ctl00_RadWindow1');
    if ($(idPopupModalOne).css('display') == 'none') {
        $('body').removeClass('popupmodal-open');
    }
    var myParent = getRealParent();
    if (myParent != null && myParent.master_closeModalEvent != null) myParent.master_closeModalEvent();
    count = 0;
}
function PopupModalShow(sender, eventArgs) {
    var myParent = getRealParent();
    if (doResetTimeOut) doResetTimeOut();
    if (myParent != null && myParent.master_showModalEvent != null) myParent.master_showModalEvent();
}

//function SetPrintContent() {
//    var radWindowManager = $find(windowManagerID);
//    if (radWindowManager != null) {
//        var isOpera = !!window.opera || navigator.userAgent.indexOf(' OPR/') >= 0;
//        var isChrome = !!window.chrome && !isOpera;
//        var activeModal = radWindowManager.getActiveWindow();
//        if (activeModal != null) {
//            $('body').addClass('no-print');
//            $('.temp-print').addClass('print');
//            //If content frame not available just ignore
//            if (!activeModal.get_contentFrame()) return;

//            var tempHTML = $(activeModal.get_contentFrame().contentWindow.document.body.firstElementChild).find('.container');
//            printTitle = activeModal.get_contentFrame().contentWindow.document.title;
//            tempHTML.find('script').remove();
//            $('.temp-print').html(tempHTML.html());
//        }
//        else {
//            $('body').removeClass('no-print');
//            $('.temp-print').removeClass("print");
//            $('.temp-print').html("");

//        }
//    }
//}
//*************************End Popup Modal*************************
function AdjustModalSize() {
    var modal = null;
    if (window.radWindow)
        modal = window.radWindow;
    else if (window.frameElement != null && window.frameElement.radWindow)
        modal = window.frameElement.radWindow;

    if (modal != null) {
        modal.autoSize();

        if (!window.chrome) { //Chrome is autosize good. IE & FF need some adjustment
            var sWidth = modal.BrowserWindow.innerWidth;
            var sHeight = modal.BrowserWindow.innerHeight;
            var mWidth = document.body.scrollWidth + 16;
            var mHeight = document.body.scrollHeight + 40;

            //When modal is smaller than screen
            modal.SetWidth((sWidth > mWidth) ? mWidth : sWidth);
            modal.SetHeight((sHeight > mHeight) ? mHeight : sHeight);

            //When modal bigger than screen
            if ((mHeight - 1 > sHeight) && (mWidth < sWidth)) {
                modal.SetWidth(mWidth + 18);
            }
            if ((mWidth > sWidth) && (mHeight < sHeight)) {
                modal.SetHeight(mHeight + 18);
            }
        }

    }
}

function SizeToFit() {
    /*
    setTimeout(function () {
        var modal = GetRadWindow();
        
        if (modal != null) {
            var sWidth = modal.BrowserWindow.getScreenWidth(),
                sHeight = modal.BrowserWindow.getScreenHeight(),
                mWidth = $('#cidModalContainer').width() + 16, //16 = RadWindow frame (16px)
                mHeight = $('#cidModalContainer').height() + 80; //80 = RadWindow frame (40px) + body padding (40px)

            //When modal is smaller than screen
            modal.SetWidth((sWidth > mWidth) ? mWidth : sWidth);
            modal.SetHeight((sHeight > mHeight) ? mHeight : sHeight);

            //When modal bigger than screen
            if (!/Android|webOS|iPhone|iPad|iPod|BlackBerry|IEMobile|Opera Mini/i.test(navigator.userAgent)) {
                if ((mHeight - 1 > sHeight) && (mWidth < sWidth)) {
                    modal.SetWidth(mWidth + 17); //17 is desktop scrollbar width, on mobile device scrollbar overlay the content
                }
                if ((mWidth > sWidth) && (mHeight < sHeight)) {
                    modal.SetHeight(mHeight + 17);
                }
            }
            //To make sure overlay layers show when open many modals during a short time
        }
    }, 400);
    */
}

//****************************End Validate Date*************************************

function openPopupWindow(url, windowName, width, height) {
    if (width == null) width = screen.availWidth * 95 / 100;
    if (height == null) height = 710;
    var left = (screen.availWidth / 2) - (width / 2);
    var pwin = window.open(url, windowName, "scrollbars=1,menubar=0,toolbar=0,resizable=1, width=" + width.toString() + ",height=" + height.toString() + ",left=" + left.toString() + ",top=100,location=0", true);
    if (pwin != null) {
        pwin.focus();
    }
}