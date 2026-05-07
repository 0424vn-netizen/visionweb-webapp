
//DateFiler: Radio Button           
var optDaily;
var optMonthly;
var optWeekly;
var optDateRange;

var optSpecMerchant;
var optAllMerchant;

//Button Search
var uxSearch;
//Merchant Group
var uxAgentPanel;
var uxGroupPanel;
var uxMerchantNumber;
var uxMerchantName;
//
var uxDateRangeFrom, uxDateRangeTo;

var _textObj;

var isFilterAllMerchant = false;
function InitControls() {
    //Radio Button
    optDaily = document.getElementById(MerchantAlertHistory_ReportFilter_optDaily);
    optMonthly = document.getElementById(MerchantAlertHistory_ReportFilter_optMonthly);
    optDateRange = document.getElementById(MerchantAlertHistory_ReportFilter_optDateRange);

    optSpecMerchant = document.getElementById(MerchantAlertHistory_ReportFilter_optSpecMerchant);
    optAllMerchant = document.getElementById(MerchantAlertHistory_ReportFilter_optAllMerchant);

    uxSearch = document.getElementById(MerchantAlertHistory_ReportFilter_uxSearch);

    uxMerchantNumber = document.getElementById(MerchantAlertHistory_ReportFilter_uxMerchantNumber);
    uxMerchantName = document.getElementById(MerchantAlertHistory_ReportFilter_uxMerchantName);
    uxDateRangeFrom = document.getElementById(MerchantAlertHistory_ReportFilter_uxFromDate);
    uxDateRangeTo = document.getElementById(MerchantAlertHistory_ReportFilter_uxEndDate);

}

$(document).ready(function () {
    InitControls();
    var rangeDate = document.getElementById(MerchantAlertHistory_ReportFilter_optDateRange);
    var daily = document.getElementById(MerchantAlertHistory_ReportFilter_optDaily);
    if (rangeDate.checked) {
        ChangeDateOption(rangeDate, true);
    } else {
        ChangeDateOption(daily, true);
    }

    if ($(optAllMerchant).prop('checked')) {
        ChangeMerchantFilterOption(optAllMerchant, true)
    }

    // check specialCharacters
    addCheckSpecialCharactersForDate();
    checkAndRemoveSpecialCharacters();
});

function checkAndRemoveSpecialCharacters() {
    $('#' + MerchantAlertHistory_ReportFilter_uxMerchantNumber).change(function (e) {
        if (isIncludeSpecialCharacters(this)) {
            CheckMerchantNumber();
            removeSpecialCharacters(this);
        }
    });
    $('#' + MerchantAlertHistory_ReportFilter_uxMerchantName).change(function (e) {
        if (isIncludeSpecialCharacters(this)) {
            CheckMerchantName();
            removeSpecialCharacters(this);
        }
    });
}

//Switch between Daily, Monthly, Date Range
function ChangeDateOption(chk, isReload) {
    var now = new Date();
    var dateOpt = document.getElementById("divDate");
    var dateRangeOpt = document.getElementById("divDateRange");
    if (chk.value == 'optDateRange') {
        dateOpt.style.display = "none";
        dateRangeOpt.style.display = "";

        var picker1 = $find(MerchantAlertHistory_ReportFilter_uxFromDate);
        var picker2 = $find(MerchantAlertHistory_ReportFilter_uxEndDate);
        var dateFormValue = document.getElementById(hhdDateFrom_ClientID).value;
        var dateToValue = document.getElementById(hhdDateTo_ClientID).value;
        if (isReload) {
            picker2.set_selectedDate(dateToValue.trim().length > 0 ? new Date(dateToValue) : now);
            now.setDate(1);
            picker1.set_selectedDate(dateFormValue.trim().length > 0 ? new Date(dateFormValue) : now);
        } else {
            if (picker2)
                picker2.set_selectedDate(now);
            now.setDate(1);
            if (picker1)
                picker1.set_selectedDate(now);
        }
    }
    else {
        dateOpt.style.display = "";
        dateRangeOpt.style.display = "none";
        var picker = $find(MerchantAlertHistory_ReportFilter_uxDate);
        picker.set_selectedDate(now);
    }
}


function CompareToday(datePicker, type) {
    var msg_Prefix = "";
    type = parseInt(type);
    switch (type) {
        case 1:
            msg_Prefix = Text_Daily;
            break;
        case 2:
            msg_Prefix = Text_Monthly;
            break;
        case 3:
            msg_Prefix = Text_DateRange;
            break;
        case 5:
            msg_Prefix = Text_Weekly;
            break;
    }
    var msg_InvalidDate = String.format(MerchantAlertHistory_ReportFilter_ReportFilter_V2, msg_Prefix);
    var msg_GreaterThanToday = String.format(MerchantAlertHistory_ReportFilter_ReportFilter_V9, msg_Prefix);

    var datePickerValue = trim(datePicker.get_textBox().value);
    var currentDate = new Date();
    //Check Required
    if (datePickerValue == "") {
        alert(msg_InvalidDate);
        return false;
    }
    //Check Invalid
    var isValid = isDate(datePickerValue);
    if (!isValid) {
        alert(MerchantAlertHistory_ReportFilter_ReportDate_InvaidDate);
        return false;
    }
    //Check greater than Current Date 
    var selectedDate = new Date(datePickerValue);
    if (selectedDate > currentDate) {
        alert(msg_GreaterThanToday);
        return false;
    }

    return true;
}

//Validate Date range
function CheckDate() {
    var currentDate = new Date();
    var datePicker;

    //Validate Daily
    if (optDaily != null && optDaily.checked) {
        datePicker = $find(MerchantAlertHistory_ReportFilter_uxDate);
        if (!CompareToday(datePicker, 1))
            return false;
    }
    //Validate Monthly
    if (optMonthly != null && optMonthly.checked) {
        datePicker = $find(MerchantAlertHistory_ReportFilter_uxDate);
        if (!CompareToday(datePicker, 2))
            return false;
    }

    //Validate Date Range
    if (optDateRange != null && optDateRange.checked) {
        //From Date
        var fromDate = $find(MerchantAlertHistory_ReportFilter_uxFromDate);
        if (!CompareToday(fromDate, 3)) return false;
        //To Date
        var toDate = $find(MerchantAlertHistory_ReportFilter_uxEndDate);
        if (!CompareToday(toDate, 3)) return false;
        //Compare From Date & To Date
        var fromDate = uxDateRangeFrom.value;
        var toDate = uxDateRangeTo.value;
        if (fromDate > toDate) {
            alert(MerchantAlertHistory_ReportFilter_ReportFilter_V3);
            return false;
        }
        //Allow filter in 30 days
        var ticks = ((new Date(toDate)) - (new Date(fromDate)));
        var days = Math.floor(ticks / (24 * 60 * 60 * 1000));
        if (days > 90) {
            alert(MerchantAlertHistory_ReportFilter_Limit90Days);
            return false;
        }
    }

    return true;
}

function WarningLargeDataReport() {

    //Validate Date Range
    if (optDateRange != null && optDateRange.checked && MerchantAlertHistory_ReportFilter_LimitDaysWarningMsg > 0) {
        
        //From Date
        var fromDate = $find(MerchantAlertHistory_ReportFilter_uxFromDate);
        var toDate = $find(MerchantAlertHistory_ReportFilter_uxEndDate);

        //Allow filter in 30 days
        var ticks = ((new Date(toDate.get_textBox().value)) - (new Date(fromDate.get_textBox().value)));
        var days = Math.floor(ticks / (24 * 60 * 60 * 1000));

        if (days > MerchantAlertHistory_ReportFilter_LimitDaysWarningMsg) {
            if (confirm(MerchantAlertHistory_ReportFilter_LargeDataSetWarningMsg)) {
                return true;
            } else {
                ResetReportFilter();
                return false;
            }
        }
        else {
            return true;
        }
    }
    else {
        return true;
    }
}

function ChangeMerchantFilterOption(e, isInit) {
    isFilterAllMerchant = e.value === 'optAllMerchant';

    if (isFilterAllMerchant) {
        $('.merchant-desc-label').hide();
        $('.btn-filter-merchant').addClass('disabled');
        $('#' + MerchantAlertHistory_ReportFilter_uxMerchantNumber).attr('disabled', '');
        $(uxMerchantName).attr('disabled', '');
    }
    else {
        $('.merchant-desc-label').show();
        $('.btn-filter-merchant').removeClass('disabled')
        $('#' + MerchantAlertHistory_ReportFilter_uxMerchantNumber).removeAttr('disabled');
        $(uxMerchantName).removeAttr('disabled');
    }

    if (!isInit) {
        ResetReportFilter();
    }    
}

function ResetReportFilter() {
    $('.report-filter .rf_TextBox').val('');
    $('.report-filter .search-choice-close').trigger('click');    
    $('.report-filter .date-item input[value="optDaily"]').trigger('click');
}


function ValidateData() {
    if (CheckDate() == false) return false;
    if (!isFilterAllMerchant && CheckMerchantNumber() == false) return false;
    if (!isFilterAllMerchant && CheckMerchantName() == false) return false;
    if (!isFilterAllMerchant && !checkExistsMidOrMerchantName()) return false;

    return true;
}

//Search when user focus on textbox
function SearchEnterOnTextbox(e) {
    var isEnter = false;
    var isIE = /MSIE/.test(navigator.userAgent);
    var keyCode = isIE ? e.keyCode : e.which;
    isEnter = (keyCode == 13);
    if (isEnter) {
        if (isIncludeSpecialCharacters(e.currentTarget)) {
            uxSearch.focus();
            return false;
        }
        if (uxSearch) {
            uxSearch.focus();
            if (isIE)
                setTimeout("uxSearch.click()", 200);
            else
                uxSearch.click();
        }
        return false;
    }
}

function ChangeMerchantOption(chk) {

    if (chk.value == 'optAllMerchants') {
        uxMerchantValue.style.display = 'none';
    } else {
        uxMerchantValue.style.display = '';
        uxMerchantValue.focus();
        uxMerchantValue.select();
        uxMerchantValue.value = "";
    }
}
function ChangeAssignmentOption(chk) {
    if (chk.value == 'optAllAssignments') {
        uxAssignmentValue.style.display = 'none';
    } else {
        uxAssignmentValue.style.display = '';
    }
}

function ChangeOtherOption(chk) {
    if (chk.value == 'optAllUsers') {
        uxUserValue.style.display = 'none';
    } else {
        uxUserValue.style.display = '';
    }
}

function CheckMerchantNumber() {
    uxMerchantNumber = document.getElementById(MerchantAlertHistory_ReportFilter_uxMerchantNumber);
    if (uxMerchantNumber) {
        if (MerchantAlertHistory_ReportFilter_js_MerchantNumber.length > 0 && MerchantAlertHistory_ReportFilter_js_MerchantNumber[MerchantAlertHistory_ReportFilter_js_MerchantNumber.length - 1] == ':') {
            MerchantAlertHistory_ReportFilter_js_MerchantNumber = MerchantAlertHistory_ReportFilter_js_MerchantNumber.substring(0, MerchantAlertHistory_ReportFilter_js_MerchantNumber.length - 1);
        }
        _textObj = MerchantAlertHistory_ReportFilter_js_MerchantNumber;
        var reg = /^[0-9\-, *]*$/;
        if (reg.test(uxMerchantNumber.value) == false) {
            alert(String.format(MerchantAlertHistory_ReportFilter_ReportFilter_V7, _textObj));
            uxMerchantNumber.focus();
            uxMerchantNumber.select();
            return false;
        }
    }
    return true;
}

function CheckMerchantName() {
    if (uxMerchantName) {
        if (MerchantAlertHistory_ReportFilter_js_MerchantName.length > 0 && MerchantAlertHistory_ReportFilter_js_MerchantName[MerchantAlertHistory_ReportFilter_js_MerchantName.length - 1] == ':') {
            MerchantAlertHistory_ReportFilter_js_MerchantName = MerchantAlertHistory_ReportFilter_js_MerchantName.substring(0, MerchantAlertHistory_ReportFilter_js_MerchantName.length - 1);
        }

        _textObj = MerchantAlertHistory_ReportFilter_js_MerchantName;
        var reg = new RegExp("^((?!(<[^ \t]))(?!(&#)).)*$");
        if (reg.test(uxMerchantName.value) == false) {
            alert(String.format(MerchantAlertHistory_ReportFilter_ReportFilter_V11));
            uxMerchantName.focus();
            uxMerchantName.select();
            return false;
        }
    }
    return true;
}

function checkExistsMidOrMerchantName() {
    var merchantName = uxMerchantName.value;
    var merchantID = uxMerchantNumber.value;
    if (merchantName.trim().length <= 0 && merchantID.trim().length <= 0) {
        alert(String.format(MinimumOfMerchantIDOrMerchantNameMsg));
        return false;
    }
    return true;
}

function RebindMerchantFilter(value) {
    document.getElementById(MerchantAlertHistory_ReportFilter_hddMerchantList).value = value;
    document.getElementById(MerchantAlertHistory_ReportFilter_uxRefresh).click();
}
function doUserGroupSelectdIndexChanged(sender, ev) {
    var combo = sender;
    if (combo.get_selectedItem().get_value() == 'optGroup') {
        uxAgentPanel.style.display = 'none';
        uxGroupPanel.style.display = '';
    } else {
        uxGroupPanel.style.display = 'none';
        uxAgentPanel.style.display = '';
    }

}