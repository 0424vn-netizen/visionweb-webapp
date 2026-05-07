
$(function () {
    $("#ciMsgInfo tr:odd").addClass("AltRow");
    $("#ciMsgInfo tr:even").addClass("Row");
    $("#ciMsgInfo tr:first-child").removeClass("Row");
});
parent._modalID = '';
parent.setModalID = function (id) {
    parent._modalID = id;
}

parent._clientIDbtn = ''
parent.setClientIDbtn = function (id) {
    parent._clientIDbtn = id;
}

function filter_closeModalEvent(modalID, clientIDbtn) {
    switch (modalID) {
        case 'MessageHierarchyModal':
            document.getElementById(clientIDbtn).click();
            break;
    }
}
function doOpenSubPopup(url, width, height) {
    var scrW = getScreenWidth();
    var scrH = getScreenHeight();
    var sizeRate = 0.70;
    if (width == null)
        width = scrW * sizeRate;
    if (height == null)
        height = scrH * sizeRate;


    parent.master_closeModalEvent = function () {

        filter_closeModalEvent(parent._modalID, parent._clientIDbtn);

        parent.master_closeModalEvent = null;
    }
    return parent.ShowPopupModalChild(1, url, width, height);
}

function CallHierarchyFilterModal(modal, width, height) {
    return doOpenSubPopup(modal, width, height);
}

function ValidateSendMessage() {
    if (isCancelSubmit) {
        return false;
    }
    if (isIncludeSpecialCharacters(document.getElementById(txtMessage_ClientID))) {
        $('#' + txtMessage_ClientID).trigger('blur');
        return false;
    }
    if (!ValidateMessageInfo())
        return false;
    var filterCount = parseInt(document.getElementById(Message_CreateNew_uxCountSelectedFilter).value);
    if (filterCount == 0) {
        alert(Message_CreateNew_js_msg1);
        return false;
    }
    return true;
}
function checkComment() {
    return limitChars($(Message_CreateNew_txtMessage), 1000, $('#ciTextLimit'));
}

function txt_OnKeyPress(e) {
    var isEnter = false;
    var isIE = /MSIE/.test(navigator.userAgent);
    var keyCode = isIE ? e.keyCode : e.which;
    isEnter = (keyCode == 13);
    if (isEnter) {
        return false;
    }
}

function limitChars(textobj, limit, infodivobj) {
    var text = textobj.val();
    var textlength = text.length;

    //if (textlength > limit && !ValidateMessageBody()) {
 if (textlength > limit) {  

        infodivobj.html('0');
        textobj.val(text.substr(0, limit));
        return false;
    }
    else {
        infodivobj.html(limit - textlength);
        //ValidateMessageBody();
        return true;
    }
}

var isCancelSubmit = false;
function onPostedByBlur(element, e) {
    var ignore = e.relatedTarget || e.rangeParent;
    if (ignore == undefined) {
        if (e.originalEvent != undefined)
            ignore = e.originalEvent.explicitOriginalTarget;
    }
    if (ValidatePostedBy() == false || isIncludeSpecialCharacters(element)) {
        isCancelSubmit = true;
        removeSpecialCharacters(element);
        setTimeout('isCancelSubmit = false;', 500);
        if (ignore != null) {
            handleClickElement(ignore);
        }
        AdjustModalSize();
    }
}

function onMessageBlur(element, e) {
    var ignore = e.relatedTarget || e.rangeParent;
    if (ignore == undefined) {
        if (e.originalEvent != undefined)
            ignore = e.originalEvent.explicitOriginalTarget;
    }
    if (ValidateMessage() == false || isIncludeSpecialCharacters(element)) {
        isCancelSubmit = true;
        removeSpecialCharacters(element);
        setTimeout('isCancelSubmit = false;', 500);
        if (ignore != null) {
            handleClickElement(ignore);
        }
        AdjustModalSize();
    }
}

function handleClickElement(element) {
    if ($(element).val() == 'Cancel') {
        $(element).click();
    }
    else if ($(element).val() == 'uxActive' || $(element).val() == 'uxDeActive') {
        element.checked = true;
    }
}