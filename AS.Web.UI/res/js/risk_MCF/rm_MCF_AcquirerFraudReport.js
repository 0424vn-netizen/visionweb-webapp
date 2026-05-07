acquirerFraudFilter = (function () {
    function changeDateOption(chk) {
        var now = new Date();
        var dateOpt = document.getElementById("divDate");
        var dateRangeOpt = document.getElementById("divDateRange");
        if (chk.value === 'uxRange') {
            dateOpt.style.display = "none";
            dateRangeOpt.style.display = "inline";

            var picker1 = $find(uxFromDate_ClientID);
            var picker2 = $find(uxEndDate_ClientID);

            if (!picker2.get_selectedDate())
                picker2.set_selectedDate(now);
            now.setDate(now.getDate() - 29);
            if (!picker1.get_selectedDate())
                picker1.set_selectedDate(now);
        }
        else {

            dateOpt.style.display = "inline";
            dateRangeOpt.style.display = "none";
            var picker = $find(uxDate_ClientID);
            if (!picker.get_selectedDate())
                picker.set_selectedDate(now);
        }
    }
    function keepDateOption() {
        var rangeDate = document.getElementById(uxRange_ClientID);
        if (rangeDate.checked) {
            changeDateOption(rangeDate);
        }
    }
    function compareToday(datePicker) {
        var currentDate = new Date();
        var datePickerValue = datePicker.get_textBox(uxDate_ClientID).value;
        var isValid = isDate(datePickerValue);
        if (!isValid) {
            alert(invalidDateMsg);
            return false;
        }
        var selectedDate = new Date(datePickerValue);
        if (selectedDate > currentDate) {
            alert(dateMustNotBeGreaterThanTodayMsg);
            return false;
        }
        return true;
    }
    function checkDate() {
        var beginDate = document.getElementById(uxFromDate_ClientID);
        var endDate = document.getElementById(uxEndDate_ClientID);

        var fromDate = $find(uxFromDate_ClientID);
        var toDate = $find(uxEndDate_ClientID);
        var uxDate = $find(uxDate_ClientID);

        beginDate.value = fromDate.get_textBox().value;
        endDate.value = toDate.get_textBox().value;

        if (uxDaily && uxDaily.checked) {
            if (uxDate.get_textBox().value !== "") {
                if (!compareToday(uxDate))
                    return false;
            }
            else {
                alert(invalidDateMsg);
                return false;
            }
        }
        else if (uxMonthly && uxMonthly.checked) {
            if (uxDate.get_textBox().value !== "") {
                if (!compareToday(uxDate))
                    return false;
            }
            else {
                alert(invalidDateMsg);
                return false;
            }
        }
        else if (uxDateRange && uxDateRange.checked) {
            if (fromDate.get_textBox().value === "" || toDate.get_textBox().value === "") {
                alert(invalidDateMsg);
                return false;
            }

            if (!compareToday(fromDate))
                return false;

            if (!compareToday(toDate))
                return false;

            if (fromDate.get_selectedDate() > toDate.get_selectedDate()) {
                alert(endDateMustBeGreaterThanFromDateMsg);
                return false;
            }

            const fromDateObj = new Date(fromDate.get_selectedDate());
            const endDateObj = new Date(toDate.get_selectedDate());

            const diffTime = Math.abs(endDateObj - fromDateObj);
            const diffDays = Math.ceil(diffTime / (1000 * 60 * 60 * 24));
            if (diffDays > 30) {
                alert(dateRangeNotExceedDayMsg);
                return false;
            }
        }
        return true;
    }
    function showCalendar(type) {
        if (type === '1')
            $find(uxDate_ClientID).showPopup();
        else if (type === '2')
            $find(uxFromDate_ClientID).showPopup();
        else
            $find(uxEndDate_ClientID).showPopup();
    }
    function validateData() {
        if (acquirerFraudFilter.CheckDate())
            return true;
        return false;
    }
    function validateData() {
        if (acquirerFraudFilter.checkDate())
            return true;
        return false;
    }
    return {
        changeDateOption: changeDateOption,
        keepDateOption: keepDateOption,
        compareToday: compareToday,
        checkDate: checkDate,
        showCalendar: showCalendar,
        validateData: validateData
    };
})();


function uxAcquirerList_OnClientTextChange() {
    var combo = $find(uxAcquirerList_ClientID);
    if (!!combo && !checkMerchantNameIsScript(combo.get_text())) {
        alert(fieldMustNotIncludeScriptTagSpecialChars);
        combo.set_text("");
        return false;
    }
}

function checkMerchantNameIsScript(value) {
    if (!!value) {
        var reg = /^((?!(\<|\>))(?!(\&\#)).)*$/g;
        if (reg.test(value) == false) {
            return false;
        }
    }
    return true;
}

function uxAcquirerList_OnClientKeyPressing(sender, eventArgs) {
    var isIE = /MSIE/.test(navigator.userAgent);
    if (eventArgs.get_domEvent().keyCode == 13 && isSelectedAcquirer) {
        if (isIE) {
            setTimeout("uxSearchButton.click()", 200);
        }
        else {
            $get(uxSearchButton).click();
        }
        return false;
    }
}

var isSelectedAcquirer = false;
function uxAcquirerList_OnClientSelectedIndexChanged(sender, args) {
    if (args.get_item() != null) {
        isSelectedAcquirer = true;
    }
}

function Callback(sender, e) {
    UxExport_OnResponseEnd(sender, e);
}

function ajaxRequestStart(sender, args) {
    var target = sender.__EVENTTARGET;
    if (target.indexOf("imgExcel") >= 0 || target.indexOf("imgCSV") >= 0) {
        args.set_enableAjax(false);
    }
}

function WhenResponseEnd(sender, eventArgs) {
    var gridID = uxReportGrid_ClientID;
    ShowHideExportControl(gridID);
}

FilteringByColumnWithDataFieldModule.setInvalidMessage('Invalid value. Please enter a valid value.');
FilteringByColumnWithDataFieldModule.extendValidators({
    IntegerGreaterThanEqualZero: function (value) {
        return !!(this.Integer(value) && value >= 0);
    }
});

$(document).ready(function () {
    acquirerFraudFilter.keepDateOption();
    addCheckSpecialCharactersForDate();
});