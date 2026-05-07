var fromCheckPast = true; //check From Date is Past
var fromCheckRequire = true;

function ShowCalendar(obj) {
    $find(obj.parentNode.parentNode.parentNode.parentNode.parentNode.parentNode.getElementsByTagName('input')[0].id).showPopup();
}
function DeleteExclude() {
    if (confirm(rm_TemporarilyExcludeWorkQueue_js_ConfirmDelete)) {
        return true;
    }
    else
        return false;
}
function validPastDate(datePicker, checkToDay) {
    var currentDate = new Date();
    currentDate.setHours(0, 0, 0, 0);
    var datePickerValue = datePicker.get_textBox().value;
    var selectedDate = new Date(datePickerValue);
    if ((checkToDay && selectedDate <= currentDate) || (selectedDate < currentDate)) {
        return false;
    } else {
        return true;
    }
}
function CheckStartDate_Required() {
    if (document.getElementById(TemporarilyExcludeWorkQueue_uxSDate_ClientID + "_dateInput").value == '')
        fromCheckRequire = false;
    else
        fromCheckRequire = true;
    return fromCheckRequire;
}
function CheckEndDate_Required() {
    if (document.getElementById(TemporarilyExcludeWorkQueue_uxEDate_ClientID + "_dateInput").value == '') {
        return false;
    }
    return true;
}

function CheckStartDate() {
    var fromTextBox = document.getElementById(TemporarilyExcludeWorkQueue_uxSDate_ClientID + "_dateInput");
    if (!isDate(fromTextBox.value) && !fromTextBox.disabled) 
        return false;
    return true;
}
function CheckEndDate() {
    var toTextBox = document.getElementById(TemporarilyExcludeWorkQueue_uxEDate_ClientID + "_dateInput");
    if (!isDate(toTextBox.value)) {
        return false;
    }
    return true;
}

function ValidateFromDate_Past() {
    var dateFrom = $find(TemporarilyExcludeWorkQueue_uxSDate_ClientID);    
    var fromTextBox = document.getElementById(TemporarilyExcludeWorkQueue_uxSDate_ClientID + "_dateInput");

    if (fromTextBox.disabled)
        fromCheckPast = true;
    else
        fromCheckPast = (validPastDate(dateFrom, true))
    return fromCheckPast;
}

function ValidateToDate_Past() {
    if (!fromCheckPast || !fromCheckRequire) return true;
    var dateTo = $find(TemporarilyExcludeWorkQueue_uxEDate_ClientID);
    var fromTextBox = document.getElementById(TemporarilyExcludeWorkQueue_uxSDate_ClientID + "_dateInput");
    var toTextBox = document.getElementById(TemporarilyExcludeWorkQueue_uxEDate_ClientID + "_dateInput");
    return validPastDate(dateTo, !fromTextBox.disabled);
}

function CompareStartDate_EndDate() {
    var begindate = $find(TemporarilyExcludeWorkQueue_uxSDate_ClientID).get_textBox().value;
    var enddate = $find(TemporarilyExcludeWorkQueue_uxEDate_ClientID).get_textBox().value;
    begindate = new Date(begindate);
    enddate = new Date(enddate);
    if (begindate != '' && enddate != '') {
        if (begindate > enddate) {
            return false;
        }
    }
    return true;
}

function CheckExcludeWorkQueueRequired() {
    var excludeObj = $find(TemporarilyExcludeWorkQueue_uxExcludeWorkQueue_ClientID);
    var excludeText = trim(excludeObj.get_text());
    if (excludeText == '') {
        excludeObj.showDropDown();
        return false;
    }
    return true;
}

function CheckAutoQueueNameRequired() {
    var autoQueueObj = $('#' + TemporarilyExcludeWorkQueue_uxAutoQueueName_ClientID);
    var autoQueueText = autoQueueObj.val();
    if (autoQueueText == null || autoQueueText == '') {
        return false;
    }
    return true;
}
