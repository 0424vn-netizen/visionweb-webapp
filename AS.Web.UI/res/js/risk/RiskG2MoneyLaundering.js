//Show calendar when user click on date text
function ShowCalendar(type) {
    if (type == "1")
        $find(RiskMgmtReporting_uxDateRangeFrom).showPopup();
    if (type == "2")
        $find(RiskMgmtReporting_uxDateRangeTo).showPopup();
}

function ValidateData() {
    if (!ValidateDate()) {
        return false;
    }
    return true;
}

function FromIsDate() {
    var date = $find(RiskMgmtReporting_uxDateRangeFrom).get_textBox().value;
    return isDate(date);
}

function FromDate_GreaterThanToDay() {
    var date = new Date($find(RiskMgmtReporting_uxDateRangeFrom).get_textBox().value);
    var now = new Date();
    if (date > now) {
        return false;
    }
    return true;
}

function EndIsDate() {
    var date = $find(RiskMgmtReporting_uxDateRangeTo).get_textBox().value;
    return isDate(date);
}

function EndDate_GreaterThanToDay() {
    var date = new Date($find(RiskMgmtReporting_uxDateRangeTo).get_textBox().value);
    var now = new Date();
    if (date > now) {
        return false;
    }
    return true;
}

function EndDate_GreaterThanFromDate() {
    var fromDate = new Date($find(RiskMgmtReporting_uxDateRangeFrom).get_textBox().value);
    var endDate = new Date($find(RiskMgmtReporting_uxDateRangeTo).get_textBox().value);
    if (fromDate > endDate) {
        return false;
    }
    return true;
}