
//DateFiler: Radio Button           
var optDaily;
var optMonthly;
var optWeekly;
var optDateRange;


//Panel DatePicker
var pnlDaily;
var pnlWeekly;
var pnlMonthly;
var pnlDateRange;

//Date Picker:
var uxDaily;
var uxWeekly;
var uxMonthly;
var uxDateRangeFrom;
var uxDateRangeTo;

//Button Search
var uxSearch;
//Merchant Group
var uxAgentPanel;
var uxGroupPanel;
var uxMerchantNumber;
var uxMerchantName;

var _textObj;
function InitControls() {
    //Radio Button
    optDaily = document.getElementById(MgmtReport_ReportFilter_optDaily);
    optWeekly = document.getElementById(MgmtReport_ReportFilter_optWeekly);
    optMonthly = document.getElementById(MgmtReport_ReportFilter_optMonthly);
    optDateRange = document.getElementById(MgmtReport_ReportFilter_optDateRange);

    //Panel DataPicker
    pnlDaily = document.getElementById(MgmtReport_ReportFilter_pnlDaily);
    pnlWeekly = document.getElementById(MgmtReport_ReportFilter_pnlWeekly);
    pnlMonthly = document.getElementById(MgmtReport_ReportFilter_pnlMonthly);
    pnlDateRange = document.getElementById(MgmtReport_ReportFilter_pnlDateRange);

    //DatePicker
    uxDaily = document.getElementById(MgmtReport_ReportFilter_uxDaily);
    uxWeekly = document.getElementById(MgmtReport_ReportFilter_uxWeekly);
    uxMonthly = document.getElementById(MgmtReport_ReportFilter_uxMonthly);
    uxDateRangeFrom = document.getElementById(MgmtReport_ReportFilter_uxDateRangeFrom);
    uxDateRangeTo = document.getElementById(MgmtReport_ReportFilter_uxDateRangeTo);

    uxSearch = document.getElementById(MgmtReport_ReportFilter_uxSearch);

    uxAgentPanel = document.getElementById(MgmtReport_ReportFilter_uxAgentPanel);
    uxGroupPanel = document.getElementById(MgmtReport_ReportFilter_uxGroupPanel);

    uxMerchantNumber = document.getElementById(MgmtReport_ReportFilter_uxMerchantNumber);
    uxMerchantName = document.getElementById(MgmtReport_ReportFilter_uxMerchantName);
}
$(document).ready(function () {
    InitControls();
});


//Show calendar when user click on date text
function ShowCalendar(type) {
    if (type == "1")
        $find(MgmtReport_ReportFilter_uxDaily).showPopup();
    if (type == "2")
        $find(MgmtReport_ReportFilter_uxMonthly).showPopup();
    if (type == "3")
        $find(MgmtReport_ReportFilter_uxDateRangeFrom).showPopup();
    if (type == "4")
        $find(MgmtReport_ReportFilter_uxDateRangeTo).showPopup();
    if (type == "5")
        $find(MgmtReport_ReportFilter_uxWeekly).showPopup();
}

function HideAllDatePicker() {
    pnlDaily.style.display = "none";
    pnlWeekly.style.display = "none";
    pnlMonthly.style.display = "none";
    pnlDateRange.style.display = "none";
}

//Switch between Daily, Monthly, Date Range
function ChangeDateOption(chk) {
    HideAllDatePicker();
    var today = new Date();
    if (chk.value == 'optDaily') {
        pnlDaily.style.display = "";
        var picker = $find(MgmtReport_ReportFilter_uxDaily);
        if (picker != null) {
            picker.set_selectedDate(today);
        }
    }
    else if (chk.value == 'optWeekly') {
        pnlWeekly.style.display = "";
        var picker = $find(MgmtReport_ReportFilter_uxWeekly);
        if (picker != null) {
            picker.set_selectedDate(today);
        }
    }
    else if (chk.value == 'optMonthly') {
        pnlMonthly.style.display = "";
        var picker = $find(MgmtReport_ReportFilter_uxMonthly);
        if (picker != null) {
            picker.set_selectedDate(today);
        }
    }
    else if (chk.value == 'optDateRange') {
        pnlDateRange.style.display = "";
        var picker1 = $find(MgmtReport_ReportFilter_uxDateRangeFrom);
        var picker2 = $find(MgmtReport_ReportFilter_uxDateRangeTo);
        if (picker2 != null)
            picker2.set_selectedDate(today);
        today.setDate(1);
        if (picker1 != null)
            picker1.set_selectedDate(today);
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
    var msg_InvalidDate = String.format(MgmtReport_ReportFilter_ReportFilter_V2, msg_Prefix);
    var msg_GreaterThanToday = String.format(MgmtReport_ReportFilter_ReportFilter_V9, msg_Prefix);

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
        alert(MgmtReport_ReportFilter_ReportDate_InvaidDate);
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
        datePicker = $find(MgmtReport_ReportFilter_uxDaily);
        if (!CompareToday(datePicker, 1)) return false;
    }
    //Validate Weekly
    if (optWeekly != null && optWeekly.checked) {
        datePicker = $find(MgmtReport_ReportFilter_uxWeekly);
        if (!CompareToday(datePicker, 5)) return false;
    }
    //Validate Monthly
    if (optMonthly != null && optMonthly.checked) {
        datePicker = $find(MgmtReport_ReportFilter_uxMonthly);
        if (!CompareToday(datePicker, 2)) return false;
    }

    //Validate Date Range
    if (optDateRange != null && optDateRange.checked) {
        //From Date
        var fromDate = $find(MgmtReport_ReportFilter_uxDateRangeFrom);
        if (!CompareToday(fromDate, 3)) return false;
        //To Date
        var toDate = $find(MgmtReport_ReportFilter_uxDateRangeTo);
        if (!CompareToday(toDate, 3)) return false;
        //Compare From Date & To Date
        var fromDate = uxDateRangeFrom.value;
        var toDate = uxDateRangeTo.value;
        if (fromDate > toDate) {
            alert(MgmtReport_ReportFilter_ReportFilter_V3);
            return false;
        }
    }

    return true;
}
function ValidateData() {
    if (CheckDate() == false) return false;
    if (CheckMerchantNumber() == false) return false;
    if (CheckMerchantName() == false) return false;
    return true;
}

//Search when user focus on textbox
function SearchEnterOnTextbox(e) {
    var isEnter = false;
    var isIE = /MSIE/.test(navigator.userAgent);
    var keyCode = isIE ? e.keyCode : e.which;
    isEnter = (keyCode == 13);
    if (isEnter) {
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

function ChangeAgentGroupOption(rad) {
    if (rad.value == 'optGroup') {
        uxAgentPanel.style.display = 'none';
        uxGroupPanel.style.display = '';
    } else {
        uxGroupPanel.style.display = 'none';
        uxAgentPanel.style.display = '';
    }
}

function CheckMerchantNumber() {
    uxMerchantNumber = document.getElementById(MgmtReport_ReportFilter_uxMerchantNumber);
    if (uxMerchantNumber) {
        if (mgmtReport_ReportFilter_js_MerchantNumber.length > 0 && mgmtReport_ReportFilter_js_MerchantNumber[mgmtReport_ReportFilter_js_MerchantNumber.length - 1] == ':') {
            mgmtReport_ReportFilter_js_MerchantNumber = mgmtReport_ReportFilter_js_MerchantNumber.substring(0, mgmtReport_ReportFilter_js_MerchantNumber.length - 1);
        }
        _textObj = mgmtReport_ReportFilter_js_MerchantNumber;
        var reg = /^[-a-zA-Z0-9,]*$/;
        if (reg.test(uxMerchantNumber.value) == false) {
            alert(MgmtReport_ReportFilter_MsgInvalidMerchantID);
            uxMerchantNumber.focus();
            uxMerchantNumber.select();
            return false;
        }
        else if (uxMerchantNumber.value.length > 0 && uxMerchantNumber.value.length < 3) {
            alert(msgMIDLessThan3Chars);
            uxMerchantNumber.focus();
            uxMerchantNumber.select();
            return false;
        }
    }
    return true;
}

function CheckMerchantName() {
    if (uxMerchantName) {
        if (mgmtReport_ReportFilter_js_MerchantName.length > 0 && mgmtReport_ReportFilter_js_MerchantName[mgmtReport_ReportFilter_js_MerchantName.length - 1] == ':') {
            mgmtReport_ReportFilter_js_MerchantName = mgmtReport_ReportFilter_js_MerchantName.substring(0, mgmtReport_ReportFilter_js_MerchantName.length - 1);
        }

        _textObj = mgmtReport_ReportFilter_js_MerchantName;
        //var reg = /^[^`~!@#$%^&*()_+=|\][{}:;"'?\/\\><.,-]*$/;
        var reg = new RegExp("^((?!(<[^ \t]))(?!(&#)).)*$");
        if (reg.test(uxMerchantName.value) == false) {
            alert(String.format(MgmtReport_ReportFilter_ReportFilter_V11));
            uxMerchantName.focus();
            uxMerchantName.select();
            return false;
        }
    }
    return true;
}

function RebindMerchantFilter(value) {
    document.getElementById(MgmtReport_ReportFilter_hddMerchantList).value = value;
    document.getElementById(MgmtReport_ReportFilter_uxRefresh).click();
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