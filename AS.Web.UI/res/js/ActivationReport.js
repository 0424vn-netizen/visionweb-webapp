var DATE = "Date";
var MONTH = "Month";
var DATE_RANGE = "DateRange";


function ValidateData() {
    //if (!ValidateInput())
    //    return false;
    var allowSubmit = false;
    var isValidateSecondDate = $("#" + uxAddFilter_ClientID).length == 0;
    if (!isValidateSecondDate) {
        allowSubmit = CheckDate(uxDateFirstFilter_ClientID, uxDateFirst_ClientID, uxFromDateFirst_ClientID, uxToDateFirst_ClientID);
    }
    else {
        allowSubmit = CheckDate(uxDateFirstFilter_ClientID, uxDateFirst_ClientID, uxFromDateFirst_ClientID, uxToDateFirst_ClientID) &&
            CheckDate(uxDateSecondFilter_ClientID, uxDateSecond_ClientID, uxFromDateSecond_ClientID, uxToDateSecond_ClientID);
    }
    if (allowSubmit) {
        SubmitHierarchyFilter();
    }
    return false;
}

function SubmitHierarchyFilter() {
    document.getElementById(uxActivationHirarchyFilter_ClientID + "_uxReportFilter_btSubmit").click();
    return false;
}

function CheckDate(dateComboId, dateId, fromDateId, toDateId) {
    var beginDate = document.getElementById(fromDateId);
    var endDate = document.getElementById(toDateId);

    var fromDate = $find(fromDateId);
    var toDate = $find(toDateId);
    var uxDate = $find(dateId);

    var dateCombo = $find(dateComboId);
    var dateValue = dateCombo.get_selectedItem().get_value();
    //RadioButton           

    beginDate.value = fromDate.get_textBox().value;
    endDate.value = toDate.get_textBox().value;

    if (dateValue == DATE) {
        if (uxDate.get_textBox().value != "") {
            if (!CompareToday(uxDate, 0))
                return false;
        }
        else {
            alert(Msg_V2);
            return false;
        }
    }
    else if (dateValue == MONTH) {
        if (uxDate.get_textBox().value != "") {
            if (!CompareToday(uxDate, 1))
                return false;
        }
        else {
            alert(Msg_V2);
            return false;
        }
    }
    else if (dateValue == DATE_RANGE) {
        if (fromDate.get_textBox().value == "" || toDate.get_textBox().value == "") {
            alert(Msg_V2);
            return false;
        }
        if (!CompareToday(fromDate, 2))
            return false;
        if (!CompareToday(toDate, 2))
            return false;

        if (fromDate.get_selectedDate() > toDate.get_selectedDate()) {
            alert(uxRangeResource1_text + ": " + Msg_V3);
            return false;
        }
    }
    return true;
}
//Show calendar when user click on date text


function ShowCalendar(type, isFirst) {
    if (isFirst) {
        if (type == '1')
            $find(uxDateFirst_ClientID).showPopup();
        else if (type == '2')
            $find(uxFromDateFirst_ClientID).showPopup();
        else
            $find(uxToDateFirst_ClientID).showPopup();
    } else {
        if (type == '1')
            $find(uxDateSecond_ClientID).showPopup();
        else if (type == '2')
            $find(uxFromDateSecond_ClientID).showPopup();
        else
            $find(uxToDateSecond_ClientID).showPopup();
    }
}

function SearchEnterOnTextbox(e) {
    var isEnter = false;
    var isIE = /MSIE/.test(navigator.userAgent);
    var keyCode = isIE ? e.keyCode : e.which;
    isEnter = (keyCode == 13);
    if (isEnter) {
        var uxSearchButton = document.getElementById(uxSearch_ClientID);
        uxSearchButton.focus();
        if (isIE)
            setTimeout("uxSearchButton.click()", 200);
        else
            uxSearchButton.click();
        return false;
    }
}

function ChangeDateOption(sender, args) {
    var elementId = sender.get_id();
    var now = new Date();
    var divDateId = $("#" + elementId).data("date-id");
    var divDateRangeId = $("#" + elementId).data("date-range-id");

    var dateValue = sender.get_value()

    if (dateValue == "DateRange") {
        $("#" + divDateId).css("display", "none");
        $("#" + divDateRangeId).css("display", "inline");

        var pickerFrom = elementId.indexOf("First") >= 0 ? $find(uxFromDateFirst_ClientID) : $find(uxFromDateSecond_ClientID);
        var pickerTo = elementId.indexOf("First") >= 0 ? $find(uxToDateFirst_ClientID) : $find(uxToDateSecond_ClientID);

        pickerTo.set_selectedDate(now);
        now.setDate(1);
        pickerFrom.set_selectedDate(now);
    }
    else {
        $("#" + divDateId).css("display", "inline");
        $("#" + divDateRangeId).css("display", "none");
        var picker = elementId.indexOf("First") >= 0 ? $find(uxDateFirst_ClientID) : $find(uxDateSecond_ClientID);
        picker.set_selectedDate(now);
    }
}


function KeepDateOption() {
    var firstDateValue = $("#" + uxDateFirstFilter_ClientID + "_Input").val();
    var secondDateValue = $("#" + uxDateSecondFilter_ClientID + "_Input").val();
    if (firstDateValue == DateRangeText) {
        $("#divDateFirst").css("display", "none");
        $("#divDateRangeFirst").css("display", "inline");
        $("#tblHieararchyFilter").css("margin-left", "68px");
    }
    else {
        $("#tblHieararchyFilter").css("margin-left", "-3px");
    }

    if (secondDateValue == DateRangeText) {
        $("#divDateSecond").css("display", "none");
        $("#divDateRangeSecond").css("display", "inline");
    }

    if (secondDateValue) {
        $("#tblHieararchyFilter").css("margin-left", "68px");
    } else {
        $("#tblHieararchyFilter").css("margin-left", "-3px");
    }
}

$(document).ready(function () {
    KeepDateOption();
    addCheckSpecialCharactersForDate();
})

function ajaxRequestStart(sender, args) {
    if (typeof (UxExporter_OnRequestStart) == 'function')
        UxExporter_OnRequestStart(sender, args);
}

function masterAjax_responseEnd(sender, args) {
    KeepDateOption();
    if (typeof (UxExporter_OnResponseEnd) == 'function')
        UxExporter_OnResponseEnd(sender, args);
    if (IsAddFilter)
    {
        $("#tblHieararchyFilter").css("margin-left", "68px");
        IsAddFilter = false;
    }
    if (IsDeleteFilter)
    {
        $("#tblHieararchyFilter").css("margin-left", "-3px");
        IsDeleteFilter = false;
    }
}

function CompareToday(datePicker, type) {
    var currentDate = new Date();
    var datePickerValue = datePicker.get_textBox(uxDateFirst_ClientID).value;
    var isValid = isDate(datePickerValue);
    if (!isValid) {
        alert(Msg_V1);
        return false;
    }
    var selectedDate = new Date(datePickerValue);

    var object;
    switch (type) {
        case 0: object = uxDailyResource1_text; break;
        case 1: object = uxMonthlyResource1_text; break;
        case 2: object = uxRangeResource1_text; break;
    }

    if (selectedDate > currentDate) {
        alert(String.format(Msg_V9, object));
        return false;
    }

    return true;
}

IsDeleteFilter = false;
IsUpdateAmount = false;
function PopupModalClose(sender, eventArgs) {
    if (IsDeleteFilter) {
        document.getElementById(uxBtnDeleteFilter_ClientID).click();
    }
    if (IsUpdateAmount) {
        document.getElementById(uxBtnUpdateBatchAmount_ClientID).click();
        IsUpdateAmount = false;
    }
}

IsAddFilter = false;
function OnAddFilter()
{
    IsAddFilter = true;
    return true;
}

