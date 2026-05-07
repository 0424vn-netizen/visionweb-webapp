//Trim string
String.prototype.trim = function () {
    a = this.replace(/^\s+/, '');
    return a.replace(/\s+$/, '');
};

//Check special character
function IsSpecialCharacter(sText) {
    return coreHasInvalidCharacters(sText, '`~!@#$%^&*()-_+=|\\{[}]:;"\'<,>.?/ ');
}

function IsSpecialCharacterAcceptSpace(sText) {
    return coreHasInvalidCharacters(sText, '`~!@#$%^&*()-_+=|\\{[}]:;"\'<,>.?/');
}
//special characters: '&#' and there must be a space after ‘<’.
function checkSpecialCharacter(sText) {
  var regex = new RegExp(/^((?!(<[^ \t]))(?!(&#)).)*$/);
  return regex.test(sText);
}

function isContainsScriptTagSpecialCharacter(value) {
    if (value) {
        const reg = /^((?!(\<|\>))(?!(\&\#)).)*$/g;
        return !reg.test(value);
    }
    return false;
}

function coreValidate(clientId, maxL, minL, onlyEx, exceptEx, formatEx, maxMsg, minMsg, invalidMsg, invalidFormat) {
    var obj = document.getElementById(clientId);

    var ret = true;
    if (obj.disabled)
        return ret;
    var inputValue = obj.value;
    if (maxL != null) {
        if (inputValue.length > maxL) {
            alert(maxMsg);
            ret = false;
        }
    }
    if (minL != null && ret) {
        var tempValue = inputValue; //trim(inputValue);
        if (minL == 0 && tempValue.length == 0) return true;
        if (tempValue.length < minL) {
            alert(minMsg);
            ret = false;
        }
    }
    if (onlyEx != null && ret) {
        if (!hierarchyHasOnlyCharacters(inputValue, onlyEx)) {
            alert(invalidMsg);
            ret = false;
        }
    }

    if (exceptEx != null && ret) {
        if (hierarchyHasInvalidCharacters(inputValue, exceptEx)) {
            alert(invalidMsg);
            ret = false;
        }
    }

    if (formatEx != null && ret) {
        if (!hierarchyValidateFormat(inputValue, formatEx)) {
            alert(invalidFormat);
            ret = false;
        }
    }

    if (!ret) {
        obj.focus();
        obj.select();
    }
    return ret;
}

function coreHasInvalidCharacters(input, invalid_chars) {
    if (invalid_chars == null) return true;
    if (invalid_chars == '') return true;
    var invalidHexValue = '';
    for (var i = 0; i < invalid_chars.length; i++) {
        invalidHexValue += '\\x' + invalid_chars.charCodeAt(i).toString(16).toUpperCase();
    }
    return input.search(new RegExp('[' + invalidHexValue + ']', 'ig')) >= 0;
}
function coreHasOnlyCharacters(input, valid_chars) {
    if (valid_chars == null) return true;
    if (valid_chars == '') return true;
    var validHexValue = '';
    for (var i = 0; i < valid_chars.length; i++) {
        validHexValue += '\\x' + valid_chars.charCodeAt(i).toString(16).toUpperCase();
    }
    var m = input.match(new RegExp('[' + validHexValue + ']', 'ig'));
    if (m != null) {
        return m.length == input.length;
    }
    return false;
}

function coreValidateFormat(input, validHexValue) {
    if (validHexValue == null) return true;
    if (validHexValue == '') return true;
    var m = input.match(new RegExp(validHexValue, 'ig'));
    if (m != null) {
        return m == input;
    }
    return false;
}
//*************************Global Functions*************************
//var defaultButtonID = ''
//$(document).ready(function() {
//    $("input").keypress(function(e) {
//        if ((e.which && e.which == 13) || (e.keyCode && e.keyCode == 13)) {
//            if (defaultButtonID != '') {
//                var button = document.getElementById(defaultButtonID);
//                if (button != null) {
//                    button.click();
//                    return true;
//                }
//            }
//            return false;
//        }
//    });
//});

//-------------------------------------------------------------------------------------------------------
//<summary>
//	Remove all spaces at the beginning of a string
//	Author: HoangDinh
//</summary>
//-------------------------------------------------------------------------------------------------------
function trimLeft(s) {
    var i;
    i = 0;
    var n;
    n = s.length;
    while ((i < n) && (s.charAt(i) == ' ')) i++;
    s = s.substring(i);
    return (s);
}

//-------------------------------------------------------------------------------------------------------
//<summary>
//	Remove all spaces at the end of a string
//	Author: HoangDinh
//</summary>
//-------------------------------------------------------------------------------------------------------
function trimRight(s) {
    var n;
    n = s.length;
    var i;
    i = s.length - 1;
    while ((i >= 0) && (s.charAt(i) == ' ')) i--;
    s = s.substring(0, i + 1);
    return (s);
}

//-------------------------------------------------------------------------------------------------------
//<summary>
//	 Remove all leading and trailing spaces in a string
//	Author: HoangDinh
//</summary>
//-------------------------------------------------------------------------------------------------------
function trim(s) {
    s = trimLeft(s);
    s = trimRight(s);
    var ex = /\s\s+/g
    s = s.replace(ex, " ")

    return (s);
}

function PrintPage() {
    if (window.print)
        try { setTimeout('window.print()', 500); } catch (ex) { }
    else
        alert(AS_CommonJS_Msg_PrintPage_Alert);
}
function IsNumberCommaSpaceKey(evt) {
    ///<summary>
    ///Only allow user to type number, space and comma and obsolutely, backspace :)
    ///</summary>
    //[48-57]: number, 44: comma, 32: space, 8: backspace
    var charCode = (evt.which) ? evt.which : event.keyCode;
    if (charCode >= 47 && charCode <= 57 || charCode == 44 || charCode == 32 || charCode == 8)
        return true;
    return false;
}

//check number:0-9
function IsNumeric(strString) {
    var strValidChars = "0123456789";
    var strChar;
    var blnResult = true;
    if (strString.length == 0) return false;
    // test strString consists of valid characters listed above
    for (i = 0; i < strString.length && blnResult == true; i++) {
        strChar = strString.charAt(i);
        if (strValidChars.indexOf(strChar) == -1) {
            blnResult = false;
        }
    }
    return blnResult;
}

//****************************Validate Date*************************************
var dtCh = "/";
var minYear = 1900;
var maxYear = 2100;

function isInteger(s) {
    var i;
    for (i = 0; i < s.length; i++) {
        // Check that current character is number.
        var c = s.charAt(i);
        if (((c < "0") || (c > "9"))) return false;
    }
    // All characters are numbers.
    return true;
}

function stripCharsInBag(s, bag) {
    var i;
    var returnString = "";
    // Search through string's characters one by one.
    // If character is not in bag, append to returnString.
    for (i = 0; i < s.length; i++) {
        var c = s.charAt(i);
        if (bag.indexOf(c) == -1) returnString += c;
    }
    return returnString;
}
function daysInFebruary(year) {
    // February has 29 days in any year evenly divisible by four,
    // EXCEPT for centurial years which are not also divisible by 400.
    return (((year % 4 == 0) && ((!(year % 100 == 0)) || (year % 400 == 0))) ? 29 : 28);
}
function DaysArray(n) {
    for (var i = 1; i <= n; i++) {
        this[i] = 31
        if (i == 4 || i == 6 || i == 9 || i == 11) { this[i] = 30 }
        if (i == 2) { this[i] = 29 }
    }
    return this
}
function isDate(dtStr) {
    dtStr = dtStr.toString();
    var daysInMonth = DaysArray(12)
    var pos1 = dtStr.indexOf(dtCh)
    var pos2 = dtStr.indexOf(dtCh, pos1 + 1)
    var strMonth = dtStr.substring(0, pos1)
    var strDay = dtStr.substring(pos1 + 1, pos2)
    var strYear = dtStr.substring(pos2 + 1)
    strYr = strYear
    if (strDay.charAt(0) == "0" && strDay.length > 1) strDay = strDay.substring(1)
    if (strMonth.charAt(0) == "0" && strMonth.length > 1) strMonth = strMonth.substring(1)
    for (var i = 1; i <= 3; i++) {
        if (strYr.charAt(0) == "0" && strYr.length > 1) strYr = strYr.substring(1)
    }
    month = parseInt(strMonth)
    day = parseInt(strDay)
    year = parseInt(strYr)
    if (pos1 == -1 || pos2 == -1) {
        //alert("The date format should be : mm/dd/yyyy")
        return false
    }
    if (strMonth.length < 1 || month < 1 || month > 12) {
        //alert("Please enter a valid month")
        return false
    }
    if (strDay.length < 1 || day < 1 || day > 31 || (month == 2 && day > daysInFebruary(year)) || day > daysInMonth[month]) {
        //alert("Please enter a valid day")
        return false
    }
    if (strYear.length != 4 || year == 0 || year < minYear || year > maxYear) {
        //alert("Please enter a valid 4 digit year between "+minYear+" and "+maxYear)
        return false
    }
    if (dtStr.indexOf(dtCh, pos2 + 1) != -1 || isInteger(stripCharsInBag(dtStr, dtCh)) == false) {
        //alert("Please enter a valid date")
        return false
    }
    return true
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

function setCookie(name, value, days) {
    if (days) {
        var date = new Date();
        date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
        var expires = '; expires=' + date.toGMTString();
    }
    else
        var expires = '';
    document.cookie = name + '=' + value + expires + '; path=/';
}

function getRealParent() {
    var myParent = parent;
    try {
        var mydomain = myParent.window.document.domain;
    }
    catch (ex) {
        myParent = self;
    }

    return myParent;
}

function ShowCalendar(obj) {
    $find(obj.parentNode.parentNode.parentNode.parentNode.parentNode.parentNode.getElementsByTagName('input')[0].id).showPopup();
}

// Set width for filter control of grid
$().ready(function () {
    setFilterWidth();
});
function setFilterWidth() {
    $('.rgFilterBox').each(function () {
        $(this).width($(this).parent().width() - 32);
    });
}
function closeMe() {
    parent.doRebindUserList();
    return parent.HidePopupModal();
}
function HideCreatePanel(id) {
    if (document.getElementById(id) != null) {
        document.getElementById(id).style.display = "none";
    }
}

//Auto orientation for main navigation
$(document).ready(function () {
    //Filter panel
    var filterPanel = $('.report-filter-panel');
    //Default is Collaspe
    var settingValueFilter = window.UserSetting && window.UserSetting.FiltersDefaultView == "Expand" ? "Expand" : "Collapse";
    if (localStorage) {
        localStorage.setItem("Expand", settingValueFilter == "Expand");
    }

    if (filterPanel.length > 0) {
        if (settingValueFilter == "Expand") {
            filterPanel.removeClass("collapse").addClass("in");
        } else {
            filterPanel.removeClass("in").addClass("collapse");
        }
    }

    if (settingValueFilter == "Expand") {
        $.cookie('FilterCollapseState', '0', { path: '/' });
    } else if (settingValueFilter == "Collapse") {
        $.cookie('FilterCollapseState', '1', { path: '/' });
    }

    //Filter panel
    var graphPanel = $('.report-graphs-panel');
    var subgraphPanel = $('.graphs-panel');
    //Default is Expand
    var settingValueGraphs = window.UserSetting && window.UserSetting.GraphsDefaultView == "Collapse" ? "Collapse" : "Expand";
    if (graphPanel.length > 0) {
        if (settingValueGraphs == "Expand") {
            graphPanel.removeClass("collapse").addClass("in");
            graphPanel.data("refreshed", true);
        } else {
            graphPanel.removeClass("in").addClass("collapse");
            subgraphPanel.addClass("collapsed");
        }

        graphPanel.on('shown.bs.collapse', function () {
            //If refreshed already, just 
            if (graphPanel.data("refreshed")) {
                return;
            }
            var charts = $(this).find('.k-chart');
            if (charts.length == 0) return;

            charts.each(function () {
                var chartInstance = $(this).data("kendoChart");
                if (chartInstance) {
                    if (chartInstance.dataSource.data().length == 0) {
                        chartInstance.dataSource.read();
                    }
                    chartInstance.refresh();
                }
            });
            graphPanel.data("refreshed", true);
        });
    }

    //Hide caret in textbox while dropdown menu is shown on IE browser
    HideCaretOnIEBrowser();

    //Print layout
    var isOpera = !!window.opera || navigator.userAgent.indexOf(' OPR/') >= 0;
    var isChrome = !!window.chrome && !isOpera;
    if (isChrome) {
        var mediaQueryList = window.matchMedia("print");
        if (mediaQueryList) {
            mediaQueryList.addListener(function (mql) {
                if (mql.matches) {
                    beforePrint();
                }
                else {
                    afterPrint();
                }
            });
        }
    }
    else {
        window.onbeforeprint = function () {
            $("body").not(".body-modal").addClass("width-xxl");
            beforePrint();
            $("body").not(".body-modal").removeClass("width-xxl");
        }
        window.onafterprint = function () {
            afterPrint();
        }
    }


});

//function requestHeader() {
//    $.ajax({
//        type: "GET",
//        url: "Header.aspx",
//        async: false,
//        contentType: "application/json; charset=utf-8",
//        dataType: "json"
//    });
//}

function beforePrint() {
    var allchart = $('div[type="kendoChart"]');
    var obj = null;

    if (allchart == null)
        return;

    for (var i = 0; i < allchart.length; i++) {
        obj = allchart[i];
        var dataobj = $(obj).data("kendoChart");
        dataobj.options.transitions = false;
        dataobj.element.resize();
    }
}

function afterPrint() {
    var allchart = $('div[type="kendoChart"]');

    if (allchart == null)
        return;
    var obj = null;
    for (var i = 0; i < allchart.length; i++) {
        obj = allchart[i];
        var dataobj = $(obj).data("kendoChart");
        dataobj.options.transitions = true;
        dataobj.element.resize();
    }
}

function AdjustWindowSize(width) {
    setTimeout(function () {
        var screenWidth = screen.availWidth;
        var actualWidth = width - 2; //2 of borders
        var height = $('#uxLastest_panel_bottom').offset().top + 116;

        if (height > screen.availHeight) {
            height = screen.availHeight;
        }
        //console.log(actualWidth + ":" + height + ":" + IsIEBrowser);
        if (navigator.userAgent.search("Firefox") > -1) {
            height = height + 10;
        }
        ResizeWindow(actualWidth, height);

        function ResizeWindow(width, height) {
            var isInIFrame = (window.location != window.parent.location);
            if (!isInIFrame) {
                var left = (screen.availWidth - width) / 2;
                var top = (screen.availHeight - height) / 2;

                window.resizeTo(width, height);
                window.moveTo(left, top);
            }
        }
    }, 500);
}
function HideCaretOnIEBrowser() {
    var curFocusedElement;
    $('.dropdown').hover(function () {
        var ua = window.navigator.userAgent;
        var msie = ua.indexOf('MSIE');
        var trident = ua.indexOf('Trident/');
        var edge = ua.indexOf('Edge/');
        if (msie > 0 || trident > 0 || edge > 0) {
            curFocusedElement = $('input[type=text]:focus');
            if (curFocusedElement != null && curFocusedElement.length > 0) {
                curFocusedElement.blur();
            }
        }
    }, function () {
        if (curFocusedElement != null && curFocusedElement.length > 0) {
            curFocusedElement.focus();
            var value = curFocusedElement.val();
            if (value != null && value.length > 0) {
                curFocusedElement.val('');
                curFocusedElement.val(value);
            }
        }
    });

}

// Handle html pasted through Editor
function refixHtmlString(html) {
    var allowedTags = ["br", "<p.*?>",
        "<i.*?>", "<em.*?>",
        "<b.*?>", "<strong.*?>",
        "<u.*?>", "<span.*? style='.*?text-decoration: underline.*?'.*?>",
        "<span.*? style='.*?color:.*?'.*?>", "<span.*? style='.*?background-color:.*?'.*?>", "<span.*? style='.*?background:.*?'.*?>"],
        tags = /<\/?([a-z][a-z0-9]*)\b[^>]*>/gi;
    var htmlTags = html.replace(tags, function ($0, $1) {
        return (allowedTags.indexOf("<" + $1.toLowerCase() + ".*?>") > -1
            || allowedTags.indexOf("<" + $1.toLowerCase() + ">") > -1
            || allowedTags.indexOf("<" + $1.toLowerCase() + ".*?" + " style='.*?color:.*?'.*?" + ">") > -1
            || allowedTags.indexOf("<" + $1.toLowerCase() + ".*?" + " style='.*?background-color:.*?'.*?" + ">") > -1
            || allowedTags.indexOf("<" + $1.toLowerCase() + ".*?" + " style='.*?background:.*?'.*?" + ">") > -1
            || allowedTags.indexOf("<" + $1.toLowerCase() + ".*?" + "text-decoration: underline.*?'.*?" + ">") > -1) ? $0 : "";
    });
    if (!!htmlTags) {
        var htmlString = htmlTags
            .replace(/;;/g, ";")
            .replace(/font-size:.*?;/g, "")
            .replace(/font-family:.*?&quot;.*?;/g, "")
            .replace(/font-family:.*?;/g, "")
            .replace(/margin.*?;/g, "")
            .replace(/padding.*?;/g, "")
            .replace(/class=".*?"/g, "")
            .replace(/style=""/g, "")
            .replace(/style=" "/g, "");
        return htmlString;
    }
    return htmlTags;
}

function removeSpecialChars(str) {
    var arrChars = str.split('');
    var newStr = '';
    for (var i = 0; i < arrChars.length; i++) {
        if (arrChars[i].charCodeAt() != 5 && arrChars[i].charCodeAt() > 31 && arrChars[i].charCodeAt() != 127 && arrChars[i].charCodeAt() != 65533) {
            newStr += arrChars[i];
        }
        //console.log(arrChars[i].charCodeAt());
    }
    return newStr.trim();
}

function onHtmlPaste(editor, args) {

    if (args.get_commandName() === "Paste") {
        var s = args.get_value(),
            pureHtml = s;


        var htmlTags = s.match(/<[^>]*>/g);

        // If free text without html tag
        if (!htmlTags) {
            // remove special chars 
            pureHtml = removeSpecialChars(pureHtml);
            return args.set_value(pureHtml);
        }
        // If any html tag more than br tag
        var nonBrTag = htmlTags.some(function (e, index) {
            if (e !== "<br>" && e !== "</br>") {
                return true;
            }
        })
        if (!!nonBrTag) {
            pureHtml = refixHtmlString(s);
        }
        // remove special chars 
        pureHtml = removeSpecialChars(pureHtml);
        // If br tag only
        return args.set_value(pureHtml);
    }
}

// Show message and confirm with telerik
function showRadMessage(modalType, msg, fnCallback, title, width, height) {
    if (modalType == 'alert') {
        var oAlert = radalert(msg, width, height, title, fnCallback);
        var oElement = $(oAlert.get_popupElement());
        oElement.addClass("customeRadConfirm");
        oAlert.show();

    } else {
        var oConfirm = radconfirm(msg, fnCallback, width, height, null, title);
        var oElement = $(oConfirm.get_popupElement());
        oElement.addClass("customeRadConfirm");
        oConfirm.show();
    }
}

function showRadConfirm(modalType, msg, fnCallback, title, yesString, noString, width, height) {
    var oConfirm = radconfirm(msg, fnCallback, width, height, null, title);
    var oElement = $(oConfirm.get_popupElement());
    oElement.addClass("customeRadConfirm");
    var obj = $(oConfirm.get_contentElement()).find('.rwInnerSpan');
    obj[0].innerHTML = yesString;
    obj[1].innerHTML = noString;
    oConfirm.show();
}

/*
 * Aperia's Animated Tab Strip
 */
(function ($) {
    $.fn.asAnimatedTab = function () {
        var $elem = this.find("ul");
        if ($elem.find("#mct-active").length == 0) {
            $elem.append("<li id='mct-active'></li>");
        }
        var $activeIndicator = $elem.find("#mct-active"),
            $currentTab = $elem.find(".rtsSelected .rtsIn").first();
        var $position = $elem.find(".rtsFirst");

        $activeIndicator.css({ "width": $currentTab.width(), "left": $currentTab.position().left })
                        .data("oriLeft", $activeIndicator.position().left)
                        .data("oriWidth", $activeIndicator.css("width"));

        $elem.find("li.rtsLI").unbind('hover');
        $elem.find("li.rtsLI").hover(function () {
            var textInside = $(this).find(".rtsIn");
            leftPos = textInside.offset().left - textInside.parents("ul").offset().left;
            newWidth = textInside.width();

            $activeIndicator.stop().animate({
                left: leftPos,
                width: newWidth
            });
        }, function () {
            $currentTab = $elem.find(".rtsSelected .rtsIn").first();
            $activeIndicator.stop().animate({
                left: $currentTab.offset().left - $currentTab.parents("ul").offset().left,
                width: $currentTab.width()
            });
        });

        return this;
    };
}(jQuery));

function refreshChart() {
    var panel = $('.report-graphs-panel');
    if (panel.hasClass('collapse')) {
        setTimeout('refreshKendoChart()', 100);
    }
}

function appendHeaderforPrinter(sender, args) {
    var $gridElm

    if (typeof sender === 'string') {
        $gridElm = $("#" + sender);
    } else {
        $gridElm = $(sender.get_element());
    }
    gridHeader = $gridElm.find(".rgHeaderDiv thead").html(),
    gridFooter = $gridElm.find(".rgFooterDiv tbody").html(),
    $gridDataTable = $gridElm.find(".rgDataDiv .rgMasterTable"),
    footer = $("<tfoot></tfoot>").append(gridFooter);

    $gridDataTable.find("thead").html(gridHeader);

    if ($gridDataTable.find("tfoot").length === 0) {
        $gridDataTable.append(footer);
    } else {
        $gridDataTable.find("tfoot").empty().append(gridFooter);
    }
}

function obpHandler() {
    var $dataGrid = $('.rgDataDiv').find(".rgMasterTable"),
        maxWidth = 0;

    $("body").addClass("before-print");

    if ($dataGrid.length > 1) {
        $dataGrid.each(function () {
            if ($(this).width() > maxWidth) {
                maxWidth = $(this).width();
            }
        });
    } else {
        maxWidth = $dataGrid.width();
    }

    $dataGrid.width(maxWidth);

    if (maxWidth > 1200) {
        $("form").css("zoom", 1100 / maxWidth);

    } else {
        $("form").css("zoom", "0.92");
    }

    $("body").removeClass("before-print").hide().show();

}

function oapHandler() {
    $('.rgDataDiv').find(".rgMasterTable").css("width", "100%");
    $("form").css("zoom", "").hide().show();
}

if (/Trident/.test(navigator.userAgent)) {
    window.addEventListener('beforeprint', obpHandler);
    window.addEventListener('afterprint', oapHandler);
}

Array.prototype.includes = function (search) {

    if (this.length == 0) {
        return false;
    }
    else {
        for (var i = 0; i < this.length; i++) {
            if (this[i].trim() === search.trim()) {
                return true;
            }
        }
        return false;
    }
};

function OnClientItemsRequestingHierachy(sender, eventArgs) {

    //var len = sender.get_attributes().getAttribute("OnClientItemsRequestingLength"); 
    //if (eventArgs.get_text().length < len)
    //    eventArgs.set_cancel(true);
    //else
    //    eventArgs.set_cancel(false);
}

//38605 - Move from MasterPage.js to Common.js
function openPopupWindowOnMenu(e, url, windowName, iWidth, iHeight) {
    var width = screen.availWidth;
    var height = screen.availHeight - 40;

    if (iWidth) width = iWidth;
    else if (screen.availWidth > 1220) width = 1220;
    if (iHeight) height = iHeight;

    var left = (screen.availWidth / 2) - (width / 2);
    var pwin = window.open(url, windowName, "scrollbars=1,menubar=0,toolbar=0,resizable=1, width=" + width.toString() + ",height=" + height.toString() + ",left=" + left.toString() + ",top=0,location=0", true);

    setTimeout(function () {
        if (pwin != null) {
            pwin.focus();
        }
    }, 100);
    return false;
}
function showLoading() {
    var loadingPanel = $find(uxLoadingPanel_ClientID);
    if (loadingPanel != null) {
        loadingPanel.show('aspnetForm');
    }
}

function hideLoading() {
    var loadingPanel = $find(uxLoadingPanel_ClientID);
    if (loadingPanel != null) {
        loadingPanel.hide('aspnetForm');
    }
}

function isIncludeSpecialCharacters(emelent) {
    var reg = new RegExp("(<[^ \t])");
    var isInclude = false;

    if (emelent.value.indexOf("&#") > -1) {
        isInclude = true;
    }
    if (reg.test(emelent.value)) {
        isInclude = true;
    }
    return isInclude;
}

function removeSpecialCharacters(emelent) {
    var resultValue = emelent.value;
    var reg = new RegExp("(<[^ \t])");
    var regBlankOrTab = new RegExp("([^ \t])");

    if (resultValue.indexOf("&#") > -1) {
        while (resultValue.indexOf("&#") > -1) {
            resultValue = resultValue.replace(/&#/g, "");
        }
    }
    if (reg.test(resultValue)) {
        var arrValue = resultValue.split("");
        for (i = 0; i < arrValue.length - 1; i++) {
            if (arrValue[i] == "<" && regBlankOrTab.test(arrValue[i + 1])) {
                arrValue[i] = "";
            }
        }
        resultValue = arrValue.join("");
    }
    emelent.value = resultValue;
    return resultValue;
}
var isCancelEnterPress = false;
function addCheckSpecialCharactersForDate() {
    $(".filter-right .rcInputCell .riTextBox, .filter-item .rcInputCell .riTextBox").each(function () {
        $(this).change(function (e) {
            if (isIncludeSpecialCharacters(this)) {
                isCancelEnterPress = true;
                alert(content.AlertMsgInvalidDate);
                removeSpecialCharacters(this);
                setTimeout('isCancelEnterPress = false;', 300);
            }
        });
        $(this).keypress(function (e) {
            var isIE = /MSIE/.test(navigator.userAgent);
            var keyCode = isIE ? e.keyCode : e.which;

            if (keyCode == 13 && isCancelEnterPress) {
                isCancelEnterPress = false;
                e.preventDefault();
                return false;
            }
        });
    });
}
function addValidationMultiSelectorFilter(element, event, currentUrl) {
    if (isIncludeSpecialCharacters(element)) {
        var activeWindow = parent.GetRadWindowManager().GetActiveWindow();
        if (activeWindow != null) {
            var url = activeWindow.get_navigateUrl();
            if (url.indexOf(currentUrl) >= 0) {
                if (activeWindow.isMinimized() || activeWindow.isPinned() || activeWindow.isMaximized()) {
                    removeSpecialCharacters(element);
                } else {
                    removeSpecialCharacters(element);
                    alert(content.AlertMsgSpecialCharacters);
                    event.preventDefault();
                    element.focus();
                }
            }
        } else {
            removeSpecialCharacters(element);
            alert(content.AlertMsgSpecialCharacters);
            event.preventDefault();
            element.focus();
        }
    }
}

function addValidationSpecialCharactersForDateByClassNames(classNames, invalidMsg) {
    $(classNames).each(function () {
        $(this).change(function (e) {
            if (isIncludeSpecialCharacters(this)) {
                alert(invalidMsg);
                removeSpecialCharacters(this);
                this.focus();
            }
        })
    });
}

function selectedDateCannotGreaterCurrentDate(dateControlId, invalidMsg, greaterTodayMsg) {

    // Validate Date
    let datePickerValue = trim($find(dateControlId).get_textBox().value);
    let currentDate = new Date();

    //Check Required
    if (datePickerValue == "") {
        alert(invalidMsg);
        return false;
    }

    //Check Invalid
    let isValid = isDate(datePickerValue);
    if (!isValid) {
        alert(invalidMsg);
        return false;
    }

    //Check greater than Current Date 
    let selectedDate = new Date(datePickerValue);
    if (selectedDate > currentDate) {
        alert(greaterTodayMsg);
        return false;
    }

    return true;
}

(function () {
    'use strict';
    $(document).ajaxSend(function (event, jqXHR, settings) {
        var timestamp = new Date().getTime();
        jqXHR.setRequestHeader('X-Client-Timestamp', timestamp);
        if (window._lastClickTimestamp) {
            jqXHR.setRequestHeader('X-Client-Click-Timestamp', window._lastClickTimestamp);
            window._lastClickTimestamp = null;
        }
        const timeZone = Intl?.DateTimeFormat()?.resolvedOptions()?.timeZone;
        jqXHR.setRequestHeader('X-Client-TimeZone', timeZone);
    });
    document.addEventListener('click', function () {
        window._lastClickTimestamp = new Date().getTime();
    }, true);
})();