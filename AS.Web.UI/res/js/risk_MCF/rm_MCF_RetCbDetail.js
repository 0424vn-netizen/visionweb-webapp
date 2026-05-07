
function uxGoBack_Click() {
    $get(rm_RetCbDetail_uxButtonGoBack).click();
}

function ValidateDate() {
    var datePickerFrom = $find(rm_RetCbDetail_uxDateFilter);
    if (!CompareToday(datePickerFrom, "From")) return false;
    return true;
}

function CompareToday(datePicker, strCaption) {
    var datePickerValue = trim(datePicker.get_textBox().value);
    var currentDate = new Date(rm_RetCbDetail_uxDateFilter_MaxDate);

    //Check Required
    if (datePickerValue == "") {
        alert(rm_RetCbDetail_js_Alert1);
        return false;
    }
    //Check Invalid
    var isValid = isDate(datePickerValue);
    if (!isValid) {
        alert(rm_RetCbDetail_js_Alert2);
        return false;
    }
    //Check greater than Current Date 
    var selectedDate = new Date(datePickerValue);
    if (selectedDate > currentDate) {
        alert(rm_RetCbDetail_js_Alert3);
        return false;
    }
    return true;
}

$(document).ready(function () {
    addCheckSpecialCharactersForDate();
});