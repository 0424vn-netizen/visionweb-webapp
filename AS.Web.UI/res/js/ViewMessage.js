function ChangeDateOption(chk) {
    var now = new Date();
    var dateOpt = document.getElementById("divDate");
    var dateRangeOpt = document.getElementById("divDateRange");
    if (chk.value == 'uxRange') {
        dateOpt.style.display = "none";
        dateRangeOpt.style.display = "inline";

        var picker1 = $find(uxFromDate_ClientID);
        var picker2 = $find(uxEndDate_ClientID);


        picker2.set_selectedDate(now);
        if (FILTERING_OPTIONS_DATERANGE) {
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
        var picker = $find(uxDate_ClientID);
        //if (picker.get_selectedDate() == null) {
        //now.setDate(1);
        picker.set_selectedDate(now);
        //}
    }
} function KeepDateOption() {
    var rangeDate = document.getElementById(uxRange_ClientID);
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
        alert(Msg_ReportFilter_V1);
        return false;
    }
    var selectedDate = new Date(datePickerValue);
    if (selectedDate > currentDate) {
        var object;
        switch (type) {
            case 0: object = uxDailyResource1_Text; break;
            case 1: object = uxMonthlyResource1_Text; break;
            case 2: object = uxRangeResource1_Text; break;
        }
        alert(String.format(Msg_ReportFilter_V9, object));
        return false;
    }
    return true;
}

function CheckDate() {
    var beginDate = document.getElementById(uxFromDate_ClientID);
    var endDate = document.getElementById(uxEndDate_ClientID);

    var fromDate = $find(uxFromDate_ClientID);
    var toDate = $find(uxEndDate_ClientID);
    var uxDate = $find(uxDate_ClientID);

    //RadioButton           
    var uxDaily = document.getElementById(uxDaily_ClientID);
    var uxMonthly = document.getElementById(uxMonthly_ClientID);
    var uxDateRange = document.getElementById(uxRange_ClientID);

    beginDate.value = fromDate.get_textBox().value;
    endDate.value = toDate.get_textBox().value;

    if (uxDaily && uxDaily.checked) {
        if (uxDate.get_textBox().value != "") {
            if (!CompareToday(uxDate, 0))
                return false;
        }
        else {
            alert(uxDailyResource1_Text + ': ' + Msg_ReportDate_InvaidDate);
            return false;
        }
    }
    else if (uxMonthly && uxMonthly.checked) {
        if (uxDate.get_textBox().value != "") {
            if (!CompareToday(uxDate, 1))
                return false;
        }
        else {
            alert(uxMonthlyResource1_Text + ': ' + Msg_ReportDate_InvaidDate);
            return false;
        }
    }
    else if (uxDateRange && uxDateRange.checked) {
        if (fromDate.get_textBox().value == "" || toDate.get_textBox().value == "") {
            alert(uxRangeResource1_Text + ': ' + Msg_ReportDate_InvaidDate);
            return false;
        }
        if (!CompareToday(fromDate, 2))
            return false;
        if (!CompareToday(toDate, 2))
            return false;

        if (fromDate.get_selectedDate() > toDate.get_selectedDate()) {
            alert(Msg_ReportFilter_V3);
            return false;
        }
    }
    return true;
}
//Show calendar when user click on date text
function ShowCalendar(type) {
    if (type == '1')
        $find(uxDate_ClientID).showPopup();
    else if (type == '2')
        $find(uxFromDate_ClientID).showPopup();
    else
        $find(uxEndDate_ClientID).showPopup();
}

function ValidateData() {
    if (CheckDate())
        return true;
    return false;
}
$(function () {
    // Add event click enter from kb
    $('.filter-block input[type=text]').keypress(function (e) {
        var code = e.keyCode || e.which;
        if (code == 13) {
            setTimeout('doClick()', 100);
            return false;
        }
    });
});
function doClick() {
    document.getElementById(uxSearch_ClientID).click();
}