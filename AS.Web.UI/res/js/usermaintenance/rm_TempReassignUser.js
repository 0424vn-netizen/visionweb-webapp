function ShowCalendar(obj) {
    $find(obj.parentNode.parentNode.parentNode.parentNode.parentNode.parentNode.getElementsByTagName('input')[0].id).showPopup();
}
function DeleteReAssignment() {
    if (confirm(rm_TempResassignUser_js_ConfirmDelete)) {
        return true;
    }
    else
        return false;
}
function validPastDate(datePicker) {
    var currentDate = new Date();
    currentDate.setHours(0, 0, 0, 0);
    var datePickerValue = datePicker.get_textBox().value;
    var selectedDate = new Date(datePickerValue);
    if (selectedDate < currentDate) {
        return false;
    } else {
        return true;
    }
}
function CheckStartDate_Required() {
    if (document.getElementById(RiskReassignUser_uxSDate_ClientID + "_dateInput").value == '') {
        return false;
    }
    return true;
}
function CheckEndDate_Required() {
    if (document.getElementById(RiskReassignUser_uxEDate_ClientID + "_dateInput").value == '') {
        return false;
    }
    return true;
}

function CheckStartDate() {
    var fromTextBox = document.getElementById(RiskReassignUser_uxSDate_ClientID + "_dateInput");
    if (!isDate(fromTextBox.value) && !fromTextBox.disabled) {
        return false;
    }
    return true;
}
function CheckEndDate() {
    var toTextBox = document.getElementById(RiskReassignUser_uxEDate_ClientID + "_dateInput");
    if (!isDate(toTextBox.value)) {
        return false;
    }
    return true;
}

function CheckReassignListRequired() {
    var reassignObj = $find(RiskReassignUser_uxReassignList_ClientID);
    var reassignText = trim(reassignObj.get_text());
    if (reassignText == '') {
        reassignObj.showDropDown();
        return false;
    }
    return true;
}
//function validateStartDatePast()
//{
//    var dateFrom = $find(RiskReassignUser_uxSDate_ClientID);
//    var fromTextBox = document.getElementById(RiskReassignUser_uxSDate_ClientID + "_dateInput");
//    if (!fromTextBox.disabled) {
//        if (!validPastDate(dateFrom)) return false;
//    }
//    return true;
//}
//function validateEndDatePast() {
//    var dateTo = $find(RiskReassignUser_uxEDate_ClientID);
//    var fromTextBox = document.getElementById(RiskReassignUser_uxSDate_ClientID + "_dateInput");
//    if (!fromTextBox.disabled) {
//        if (!validPastDate(dateTo)) return false;
//    }
//    return true;
//}
function validateDatePast() {
    var dateFrom = $find(RiskReassignUser_uxSDate_ClientID);
    var dateTo = $find(RiskReassignUser_uxEDate_ClientID);
    var fromTextBox = document.getElementById(RiskReassignUser_uxSDate_ClientID + "_dateInput");
    if (!fromTextBox.disabled) {
        if (!validPastDate(dateFrom) || !validPastDate(dateTo)) {
            //set highlight From,To caption
            $("label[for='" + RiskReassignUser_uxSDate_ClientID + "']").addClass("label-error");
            $("label[for='" + RiskReassignUser_uxEDate_ClientID + "']").addClass("label-error");
            return false;
        }
    }
    return true;
}

function CompareStartDate_EndDate() {
    var begindate = document.getElementById(RiskReassignUser_uxSDate_ClientID).value;
    var enddate = document.getElementById(RiskReassignUser_uxEDate_ClientID).value;
    if (begindate != '' && enddate != '') {
        if (begindate > enddate) {
            return false;
        }
    }
    return true;
}

function CheckAssignUserUpdate() {
    var userObj = document.getElementById(RiskReassignUser_uxUserName_ClientID);
    var userText = userObj.innerText;
    var reassignObj = $find(RiskReassignUser_uxReassignList_ClientID);
    var reassignText = trim(reassignObj.get_text());

    if (userText.toLowerCase() == reassignText.toLowerCase()) {
        return false;
    }
    return true;
}

function CheckUserListRequiredCreate() {
    var userObj = $find(RiskReassignUser_uxUserList_ClientID);
    var userText = trim(userObj.get_text());
    if (userText == '') {
        userObj.showDropDown();
        return false;
    }
    return true;
}

function CheckAssignUserCreate() {
    var userObj = $find(RiskReassignUser_uxUserList_ClientID);
    var userText = trim(userObj.get_text());
    var reassignObj = $find(RiskReassignUser_uxReassignList_ClientID);
    var reassignText = trim(reassignObj.get_text());

    if (userText.toLowerCase() == reassignText.toLowerCase()) {
        return false;
    }
    return true;
}

function hideNewReassignment() {
    $("#pnlReassignUser").css("display", "none");
};