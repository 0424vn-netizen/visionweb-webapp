function ajaxRequestStart(sender, args) {
    UxExporter_OnRequestStart(sender, args);
}
function ajaxOnResponseEnd(sender, args) {
    UxExporter_OnResponseEnd(sender, args);
}
function uxFilterStatus_Checked() {
    document.getElementById(rm_ProfileMaintenance_uxChangeFilterStatus).click();
}

function DefaultEnterOnTextBox(e) {
    var isEnter = false;
    var isIE = /MSIE/.test(navigator.userAgent);
    var keyCode = isIE ? e.keyCode : e.which;
    isEnter = (keyCode == 13);
    if (isEnter) {
        document.getElementById(rm_ProfileMaintenance_uxAddGroup).focus();
        document.getElementById(rm_ProfileMaintenance_uxAddGroup).click();
        return false;
    }
}

function DefaultEnterOnDiv(e) {
    var isEnter = false;
    var isIE = /MSIE/.test(navigator.userAgent);
    var keyCode = isIE ? e.keyCode : e.which;
    isEnter = (keyCode == 13);
    if (isEnter) {
        return false;
    }
}
function ShowMsg(msg) {
    alert(msg);
}

//Search when user focus on textbox
function doClick(btnID) {
    document.getElementById(btnID).click();
}

function ActivateDeactivate(statusID, isActive) {
    UpdateIsActive(statusID, isActive);
}

function UpdateIsActive(statusID, isActive) {
    $get(rm_ProfileMaintenance_uxActivateDeactivateData).value = statusID + ";" + isActive;
    $get(rm_ProfileMaintenance_uxActivateDeactivate).click();
}

function doValidInput(ele) {
    if ($.trim($(ele).parents('tr').find('.txtEditProfileName').val()).length == 0) {
        alert(rm_ProfileMaintenance_js_String1+' ' + Resources_ValMsg_Required);
        return false;
    }
    //else if (!reggroupName.test($.trim($(ele).parents('tr').find('.txtEditProfileName').val()))) {
    //    alert('Profile name: ' + Resources_ValMsg_InvalidCharacter);
    //    return false;
    //}
    else if ($.trim($(ele).parents('tr').find('.txtEditProfileDescription').val()).indexOf('<') != -1 || $.trim($(ele).parents('tr').find('.txtEditProfileDescription').val()).indexOf('>') != -1) {
        alert(rm_ProfileMaintenance_js_String2+' ' + Resources_ValMsg_InvalidCharacter);
        return false;
    }
    else if ($.trim($(ele).parents('tr').find('.txtEditProfileName').val()).indexOf('<') != -1 || $.trim($(ele).parents('tr').find('.txtEditProfileName').val()).indexOf('>') != -1) {
        alert(rm_ProfileMaintenance_js_String3+' ' + Resources_ValMsg_InvalidCharacter);
        return false;
    }
    else {
        return true;
    }
}

function checkMaxLengthInput() {
    if (isDisabledSubmitAdd) {
        return false;
    }
    return checkInputLength();
}

function validBeforeSubmitAdd() {
    if (isDisabledSubmitAdd) {
        isDisabledSubmitAdd = false;
        return false;
    } else {
        return doValidation();
    }
}

var isDisabledSubmitAdd = false;
function addCheckSpecialCharacters() {
    $('#' + uxAddGroupText_ClientID).change(function (e) {
        if (isIncludeSpecialCharacters(this)) {
            isDisabledSubmitAdd = true;
            ValidateAddGroupText();
            removeSpecialCharacters(this);
            this.focus();
            setTimeout('isDisabledSubmitAdd = false;', 500);
        }
    });
    $('#' + uxAddDescription_ClientID).change(function (e) {
        if (isIncludeSpecialCharacters(this)) {
            isDisabledSubmitAdd = true;
            ValidateAddDescription();
            removeSpecialCharacters(this);
            this.focus();
            setTimeout('isDisabledSubmitAdd = false;', 500);

        } else {
            checkInputLength();
        }
    });
    // Mode edit
    $(".txtEditProfileName").each(function () {
        $(this).change(function (e) {
            if (isIncludeSpecialCharacters(this)) {
                alert(rm_ProfileMaintenance_js_String3 + ' ' + Resources_ValMsg_InvalidCharacter);
                removeSpecialCharacters(this);
                this.focus();
            }
        })
    });
    $(".txtEditProfileDescription").each(function () {
        $(this).change(function (e) {
            if (isIncludeSpecialCharacters(this)) {
                alert(rm_ProfileMaintenance_js_String2 + ' ' + Resources_ValMsg_InvalidCharacter);
                removeSpecialCharacters(this);
                this.focus();
            }
        })
    });
}

$(document).ready(function () {
    addCheckSpecialCharacters();
});