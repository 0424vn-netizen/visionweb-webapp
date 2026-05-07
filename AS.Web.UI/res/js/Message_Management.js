function ChangeDateOption(chk) {
    var now = new Date();
    var dateOpt = document.getElementById("divDate");
    var dateRangeOpt = document.getElementById("divDateRange");
    if (chk.value == 'uxRange') {
        dateOpt.style.display = "none";
        dateRangeOpt.style.display = "inline";
        var picker1 = $find(Message_Management_uxFromDate);
        var picker2 = $find(Message_Management_uxEndDate);
        picker2.set_selectedDate(now);
        if (Message_Management_FILTERING_OPTIONS_DATERANGE == "True") {
            now.setDate(now.getDate() - 90);
        }
        else {
            now.setDate(1);
        }
        picker1.set_selectedDate(now);
    }
    else {

        dateOpt.style.display = "inline";
        dateRangeOpt.style.display = "none";
        var picker = $find(Message_Management_uxDate);
        picker.set_selectedDate(now);
    }
} function KeepDateOption() {
    var rangeDate = document.getElementById(Message_Management_uxRange);
    if (rangeDate.checked) {
        ChangeDateOption(rangeDate);
    }
}
try {
    KeepDateOption();
}
catch (ex) { }

function CompareToday(datePicker, type) {
    var currentDate = new Date();
    var datePickerValue = datePicker.get_textBox().value;
    var isValid = isDate(datePickerValue);
    if (!isValid) {
        alert(Message_Management_ReportFilter_V1);
        return false;
    }
    var selectedDate = new Date(datePickerValue);
    if (selectedDate > currentDate) {
        var object;
        switch (type) {
            case 0: object = MessageManagement_js_Daily_text; break;
            case 1: object = MessageManagement_js_Monthly_text; break;
            case 2: object = MessageManagement_js_Date_range_text; break;
        }
        alert(String.format(Message_Management_ReportFilter_V9, object));
        return false;
    }

    return true;
}

function CheckDate() {
    var beginDate = document.getElementById(Message_Management_uxFromDate);
    var endDate = document.getElementById(Message_Management_uxEndDate);

    var fromDate = $find(Message_Management_uxFromDate);
    var toDate = $find(Message_Management_uxEndDate);
    var uxDate = $find(Message_Management_uxDate);

    //RadioButton           
    var uxDaily = document.getElementById(Message_Management_uxDaily);
    var uxMonthly = document.getElementById(Message_Management_uxMonthly);
    var uxDateRange = document.getElementById(Message_Management_uxRange);

    beginDate.value = fromDate.get_textBox().value;
    endDate.value = toDate.get_textBox().value;

    if (uxDaily && uxDaily.checked) {
        if (uxDate.get_textBox().value != "") {
            if (!CompareToday(uxDate, 0))
                return false;
        }
        else {
            alert("Daily: " + Message_Management_ReportFilter_ReportDate_InvaidDate);
            return false;
        }
    }
    else if (uxMonthly && uxMonthly.checked) {
        if (uxDate.get_textBox().value != "") {
            if (!CompareToday(uxDate, 1))
                return false;
        }
        else {
            alert("Monthly: " + Message_Management_ReportFilter_ReportDate_InvaidDate);
            return false;
        }
    }
    else if (uxDateRange && uxDateRange.checked) {
        if (fromDate.get_textBox().value == "" || toDate.get_textBox().value == "") {
            alert("Date range: " + Message_Management_ReportFilter_ReportDate_InvaidDate);
            return false;
        }
        if (!CompareToday(fromDate, 2))
            return false;
        if (!CompareToday(toDate, 2))
            return false;

        if (fromDate.get_selectedDate() > toDate.get_selectedDate()) {
            alert(Message_Management_ReportFilter_V3);
            return false;
        }
    }
    return true;
}
//Show calendar when user click on date text
function ShowCalendar(type) {
    if (type == '1')
        $find(Message_Management_uxDate).showPopup();
    else if (type == '2')
        $find(Message_Management_uxFromDate).showPopup();
    else
        $find(Message_Management_uxEndDate).showPopup();
}
function SearchEnterOnTextbox(e) {
    var isEnter = false;
    var isIE = /MSIE/.test(navigator.userAgent);
    var keyCode = isIE ? e.keyCode : e.which;
    isEnter = (keyCode == 13);
    if (isEnter) {
        document.getElementById(Message_Management_uxSearch).focus();
        setTimeout('doClick()', 100);
        return false;
    }
}
function doClick() {
    document.getElementById(Message_Management_uxSearch).click();
}
function ValidateData() {
    if (CheckDate())
        return true;
    return false;
}
function master_closeModalEvent() {
    //setTimeout('doClick()', 100);
}
function CloseModalAndRebindGrid() {
    HidePopupModal();
    document.getElementById(Message_Management_uxRebind).click();
}

$(document).ready(function () {
    addCheckSpecialCharactersForDate();
});