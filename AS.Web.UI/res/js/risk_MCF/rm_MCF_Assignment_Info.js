const ASSIGNMENT_INFO_EVENT_FIELD_START_DATE = 0;
const ASSIGNMENT_INFO_EVENT_FIELD_EXPIRATION_DATE = 1;
const ASSIGNMENT_INFO_EVENT_FIELD_NEVER_EXPIRE_DATE = 2;
function OnchangeExpirationDate(op) {
    var expirationDate = $find(Risk_Assignment_Info_uxExpirationDate);
    if (op == 1) {
        expirationDate.set_enabled(false);
        RefreshCalendarStartDate(ASSIGNMENT_INFO_EVENT_FIELD_NEVER_EXPIRE_DATE);
    }
    else {
        expirationDate.set_enabled(true);
        RefreshCalendarStartDate(ASSIGNMENT_INFO_EVENT_FIELD_EXPIRATION_DATE);
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
        case "AssignmentSubsiteDistributionsModal":
            document.getElementById(Risk_Assignment_Info_uxRebindRebindDistribution).click();
            break;
    }
}

//Check special character
function ValidateSpecialCharacters() {
    var assignmentName = document.getElementById(Risk_Assignment_Info_uxAssignmentName);
    var assignmentNameValue = trim(assignmentName.value);

    var valid_chars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz_-$/#%*& ";
    var validHexValue = '';
    for (var i = 0; i < valid_chars.length; i++) {
        validHexValue += '\\x' + valid_chars.charCodeAt(i).toString(16).toUpperCase();
    }
    var m = assignmentNameValue.match(new RegExp('[' + validHexValue + ']', 'ig'));
    if (m != null) {
        return m.length == assignmentNameValue.length && checkSpecialCharacter(assignmentNameValue);
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
    let uxGroups = '';
    let uxUsers = '';

    if ($(Risk_Assignment_Info_divAssignmentGroups).length > 0) {
        uxGroups = $(Risk_Assignment_Info_divAssignmentGroups).html().trim();
    }

    if ($(Risk_Assignment_Info_divAssignmentUsers).length > 0) {
        uxUsers = $(Risk_Assignment_Info_divAssignmentUsers).html().trim();
    }

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
function uxComboAssignmentType_Changing(sender, args) {
    var isValid = ValidateAssignmentGeneralInformationByValidator();

    if (isValid == false) {
        args.set_cancel(true);
    }
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

function uxStartDate_SelectedChange() {
    RefreshCalendarStartDate(ASSIGNMENT_INFO_EVENT_FIELD_START_DATE);
}
uxExpirationDate_SelectedChange = function () {
    RefreshCalendarStartDate(ASSIGNMENT_INFO_EVENT_FIELD_EXPIRATION_DATE);
}
RefreshCalendarStartDate = function(typeField) {
    var fieldCalc = typeField ?? ASSIGNMENT_INFO_EVENT_FIELD_START_DATE;
    var today = new Date(Risk_Assignment_Info_DateTimeToNow);
    //0: StartDate, 1: ExpirationDate, 2: Never Expire
    switch (fieldCalc) {
        case ASSIGNMENT_INFO_EVENT_FIELD_START_DATE:
            var uxradExpirationDate = $get(Risk_Assignment_Info_uxRadExpirationDate);
            if (uxradExpirationDate.checked) {
                var uxStartDate = $find(Risk_Assignment_Info_uxStartDate);
                var newMinDate = new Date(uxStartDate.get_dateInput().get_value()) ?? today;

                //Set max date for ExpirationDate
                var uxExpirationDate = $find(Risk_Assignment_Info_uxExpirationDate);
                uxExpirationDate.set_minDate(newMinDate);
            }
            break;
        case ASSIGNMENT_INFO_EVENT_FIELD_EXPIRATION_DATE:
            var uxExpirationDate = $find(Risk_Assignment_Info_uxExpirationDate);
            var currentDate = uxExpirationDate.get_dateInput().get_value();
            if (currentDate != '') {
                var newMaxDate = new Date(uxExpirationDate.get_dateInput().get_value());
                //Set max date for StartDate
                var uxStartDate = $find(Risk_Assignment_Info_uxStartDate);
                var currentStartDate = new Date(uxStartDate.get_dateInput().get_value()) ?? today;
                uxStartDate.set_maxDate(newMaxDate < currentStartDate ? currentStartDate : newMaxDate);

                //Set min date for ExpirationDate
                uxExpirationDate.set_minDate(currentStartDate);
            }
            break;
        case ASSIGNMENT_INFO_EVENT_FIELD_NEVER_EXPIRE_DATE:
            var uxStartDate = $find(Risk_Assignment_Info_uxStartDate);
            var newMaxDate = new Date(Risk_Assignment_Info_DateTime_MaxValue);
            uxStartDate.set_maxDate(newMaxDate);
            break;
        default:
    }
}
ValidateLessStartDateByExpiredDate = function () {
    var assignmentTypeCbo = document.getElementById(Risk_Assignment_uxComboAssignmentType_ClientID);
    var uxradExpirationDate = $get(Risk_Assignment_Info_uxRadExpirationDate);
    if (uxradExpirationDate.checked && assignmentTypeCbo != null && assignmentTypeCbo.value == Risk_Assignment_enumAssTypeDescription_Client) {
        var expirationDate = $find(Risk_Assignment_Info_uxExpirationDate).get_textBox();
        expirationDateValue = trim(expirationDate.value);
        //Check Expiration Date >= Start date
        var today = new Date();
        today = new Date(today.toDateString());
        var uxStartDate = $find(Risk_Assignment_Info_uxStartDate);
        var currentStartDate = new Date(uxStartDate.get_dateInput().get_value()) ?? today;

        var selectedDate = new Date(expirationDateValue);
        selectedDate = new Date(selectedDate.toDateString());
        if (selectedDate < currentStartDate) {
            return false;
        }
    }
    return true;
}