function ValidateSendMessage(id) {
    if (isCancelSubmit) {
        return false;
    }
    if (isIncludeSpecialCharacters(document.getElementById(txtMessage_ClientID))) {
        $('#' + txtMessage_ClientID).trigger('blur');
        return false;
    }
    if (!ValidateMessageInfo())
        return false;

    var messageID = $("#" + uxMessageID_ClientID);
    PageMethods.ValidateHasRecipients(id, messageID.val(), ValidateRecipients_success)

    return false;
}

function ValidateRecipients_success(res) {
    if (res[0] == "true") {
        __doPostBack(res[1], '');
    } else {
        alert(ServiceMessage_CreateNew_js_SelectRecipient);
    }
}


function checkComment() {
    return limitChars($('#' + txtMessage_ClientID), 1000, $('#uxTextLimit'));
}

function limitChars(textobj, limit, infodivobj) {
    var text = textobj.val();
    var textlength = text.length;
    if (textlength > limit) {
        infodivobj.html('0');
        textobj.val(text.substr(0, limit));
        return false;
    }
    else {
        infodivobj.html(limit - textlength);
        return true;
    }
}


function doHeaderCheck(sender) {
    var uxIsAdded = $("#" + uxIsAdded_ClientID);
    var uxIsSelectAll = $("#" + uxIsSelectAll_ClientID);
    var uxValueCode = $("#" + uxValueCode_ClientID);
    var uxAllFlag = $("#" + uxAllFlag_ClientID);

    uxIsSelectAll.val(IS_SELECT_ALL_FLAG);
    uxIsAdded.val(sender.checked);
    uxValueCode.val("");

    if (sender.checked) {
        uxAllFlag.val(IS_SELECT_ALL_FLAG);
        $("input[id*=chkItem]").attr("checked", "checked");
    } else {
        uxAllFlag.val(IS_DESELECT_ALL_FLAG);
        $("input[id*=chkItem]").removeAttr("checked");
    }
    $("#" + btnHidden_ClientID).click();
}

function doItemCheck(sender, val) {

    var uxIsAdded = $("#" + uxIsAdded_ClientID);
    var uxIsSelectAll = $("#" + uxIsSelectAll_ClientID);
    var uxValueCode = $("#" + uxValueCode_ClientID);

    uxIsSelectAll.val(IS_DESELECT_ALL_FLAG);
    uxIsAdded.val(sender.checked);
    uxValueCode.val(val);

    if (!sender.checked) {
        $("input[id*=chkHeader]").removeAttr("checked");
        $("#" + uxAllFlag_ClientID).val(IS_DESELECT_ALL_FLAG);
    }

    $("#" + btnHidden_ClientID).click();
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
function validationSendMessage(id) {
    var txtInput = $("#" + id).val() ?? '';
    const txtMsg = txtInput.replace('\n', ' ');
    var expression = /^((?!(<[^ \t]))(?!(&#)).)*$/;
    var regex = new RegExp(expression);
    return regex.test(txtMsg);

}