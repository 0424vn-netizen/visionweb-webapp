function OnchangeExpirationDate(op) {
    var expirationDate = $find(Risk_Assignment_Info_uxExpirationDate);
    if (op == 1) {
        expirationDate.set_enabled(false);
    }
    else {
        expirationDate.set_enabled(true);
    }
}

function info_closeModalEvent(modalID) {
    switch (modalID) {
        case "AssignmentUserModal":
            document.getElementById(Risk_Assignment_Info_uxRebindAssignmentUsers).click();
            break;
        case "AssignmentGroupModal":
            document.getElementById(Risk_Assignment_Info_uxRebindAssignmentGroups).click();
            break;
    }
}

//Check special character
function ValidateSpecialCharacters() {
    var assignmentName = document.getElementById(Risk_Assignment_Info_uxAssignmentName);
    var assignmentNameValue = trim(assignmentName.value);

    var valid_chars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz_- ";
    var validHexValue = '';
    for (var i = 0; i < valid_chars.length; i++) {
        validHexValue += '\\x' + valid_chars.charCodeAt(i).toString(16).toUpperCase();
    }
    var m = assignmentNameValue.match(new RegExp('[' + validHexValue + ']', 'ig'));
    if (m != null) {
        return m.length == assignmentNameValue.length;
    }
    return false;
}

function ValidateExpiredDate() {
    var uxradExpirationDate = $get(Risk_Assignment_Info_uxRadExpirationDate);
    if (uxradExpirationDate.Checked) {
    }

}

function ValidateValidExpiredDate() {
    var uxradExpirationDate = $get(Risk_Assignment_Info_uxRadExpirationDate);
    if (uxradExpirationDate.checked) {
        var expirationDate = $find(Risk_Assignment_Info_uxExpirationDate).get_textBox();
        expirationDateValue = trim(expirationDate.value);
        var reg = /([1-9]|[10]|[11][12])\/\d{1,2}\/\d{4}/;
        if (expirationDateValue.search(reg) == -1) {
            expirationDate.focus();
            return false;
        }
        if (!isDate(expirationDateValue)) {
            expirationDate.focus();
            return false;
        }
    }
    return true;
}
function ValidateLessTodayExpiredDate() {
    var uxradExpirationDate = $get(Risk_Assignment_Info_uxRadExpirationDate);
    if (uxradExpirationDate.checked) {
        var expirationDate = $find(Risk_Assignment_Info_uxExpirationDate).get_textBox();
        expirationDateValue = trim(expirationDate.value);
        //Check Expiration Date >= Today
        var today = new Date();
        today = new Date(today.toDateString());
        var selectedDate = new Date(expirationDateValue);
        selectedDate = new Date(selectedDate.toDateString());
        if (selectedDate < today) {
            return false;
        }
    }
    return true;
}
function ValidateAssignmentGeneralInformation() {
    //Check selecting at least one user or group selecting
    if (!ValidateAssignmentGeneralInformationByValidator()) {
        return false;
    }
    var uxGroups = $(Risk_Assignment_Info_divAssignmentGroups).html().trim();
    var uxUsers = $(Risk_Assignment_Info_divAssignmentUsers).html().trim();
    if (uxGroups == "N/A" && uxUsers == "N/A") {
        alert(Risk_Assignment_Info_Assignment_GroupUser_Required);
        return false;
    }
    return true;
}
function CallFilterModal(modal, width, height) {
    return doOpenSubPopup(modal + Risk_Assignment_Info_QueryString, 'auto');
}

function RadTextBox_OnKeyPress(sender, eventArgs) {

    if (eventArgs.get_keyCode() == 13) {

        eventArgs.set_cancel(true);
    }
}

function SearchEnterOnRadNumeric(sender, eventArgs) {

    if (eventArgs.get_keyCode() == 13) {

        eventArgs.set_cancel(true);
    }
}
function ValidateAssignmentMerchantRange() {
    var assignmentName = document.getElementById(Risk_Assignment_Info_uxAssignmentName).value;
    if (!ValidateAssignmentGeneralInformationByValidator())
        return false;
    var assignmentTypeCbo = document.getElementById(Risk_Assignment_uxComboAssignmentType_ClientID);
    if ((assignmentTypeCbo == null
        || assignmentTypeCbo.value == Risk_Assignment_enumAssTypeDescription_Client)) {
        return false;
    }
    return true;
}

$(document).ready(function () {
    addCheckSpecialCharacters();
});

function addCheckSpecialCharacters() {
    // add check when lost focus for AssignmentName
    $("#" + Risk_Assignment_Info_uxAssignmentName).change(function (e) {
        if (isIncludeSpecialCharacters(this)) {
            ValidateAssignmentName();
            removeSpecialCharacters(this);
        }
    });
    // add check when lost focus for ExpirationDate
    $("#" + Risk_Assignment_Info_uxExpirationDate + "_dateInput").change(function (e) {
        if (isIncludeSpecialCharacters(this)) {
            ValidateExpirationDate();
            removeSpecialCharacters(this);
        }
    });
}