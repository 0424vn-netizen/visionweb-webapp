//Switch between Daily, Monthly, Date Range
function ChangeDateOption(chk) {

    var now = new Date();
    var dateOpt = $("#" + RiskMgmtReporting_divDate);
    var dateRangeOpt = $("#" + RiskMgmtReporting_divDateRange);

    if (chk.value == 'uxDateRange') {
        //dateOpt.style.display = "none";
        //dateRangeOpt.style.display = "";
        dateOpt.addClass("display-none");
        dateRangeOpt.removeClass("display-none");
        var picker1 = $find(RiskMgmtReporting_uxDateRangeFrom);
        var picker2 = $find(RiskMgmtReporting_uxDateRangeTo);
        if (picker2 != null) picker2.set_selectedDate(now);
        now.setDate(1);
        if (picker1 != null) picker1.set_selectedDate(now);

    }
    else {
        dateOpt.removeClass("display-none");
        dateRangeOpt.addClass("display-none");        
        var picker = $find(RiskMgmtReporting_uxDate);
        if (picker != null) picker.set_selectedDate(now);
    }
}

//Show calendar when user click on date text
function ShowCalendar(type) {
    if (type == "1")
        $find(RiskMgmtReporting_uxDate).showPopup();
    if (type == "2")
        $find(RiskMgmtReporting_uxDateRangeFrom).showPopup();
    if (type == "3")
        $find(RiskMgmtReporting_uxDateRangeTo).showPopup();
}

function ValidateData() {
    window.isDownloadingReport = false;
    if (!ValidateDate()) {
        return false;
    }

    var uxCboMerchant = $find(RiskMgmtReporting_uxCboMerchant);
    if (uxCboMerchant) {
        var uxCboMerchantValue = uxCboMerchant.get_selectedItem().get_value();
        if (uxCboMerchantValue == "MERCHANTNUMBER") {
            var haveMerchant = $('#' + RiskhaveMerchant_ClientId).val();            
            if (haveMerchant == '1' && !ValidateMerchant()) {
                return false;
            }
        }
    }
    return true;
}

function KeepMerchantOption() {
    var uxCboMerchant = $find(RiskMgmtReporting_uxCboMerchant);

    var uxCboMerchantValue = uxCboMerchant.get_selectedItem().get_value();
    if (uxCboMerchantValue == "MERCHANTNUMBER") {
        $("#cidSearchValue").removeClass("display-none");        
        document.getElementById(RiskMgmtReporting_uxSearchValue).focus();
    }
    else
        $("#cidSearchValue").addClass("display-none");
}

function KeepDateOption() {
    var dateOpt = $("#" + RiskMgmtReporting_divDate);
    var dateRangeOpt = $("#" + RiskMgmtReporting_divDateRange);
    var uxDateRange = document.getElementById(RiskMgmtReporting_uxDateRange);
    if (uxDateRange != null && uxDateRange.checked) {
        if (uxDateRange.value == 'uxDateRange') {
            dateOpt.addClass("display-none");
            dateRangeOpt.removeClass("display-none");
        }
        else {
            dateOpt.removeClass("display-none");
            dateRangeOpt.addClass("display-none");
        }
    } else {
        dateOpt.removeClass("display-none");
        dateRangeOpt.addClass("display-none");
    }
}

//Validate data
function StartIsDate() {
    var haveDate = $('#' + RiskhaveDate_ClientId).val();
    if (haveDate == '1')
    {
        var uxDateRange = document.getElementById(RiskMgmtReporting_uxDateRange);
        if (uxDateRange != null && !uxDateRange.checked) {
            var date = $find(RiskMgmtReporting_uxDate).get_textBox().value;
            return isDate(date);
        }
    }
    
    return true;
}

function StartDate_GreaterThanToDay() {
    var haveDate = $('#' + RiskhaveDate_ClientId).val();
    if (haveDate == '1')
    {
        var uxDateRange = document.getElementById(RiskMgmtReporting_uxDateRange);
        if (uxDateRange != null && !uxDateRange.checked) {
            var date = new Date($find(RiskMgmtReporting_uxDate).get_textBox().value);
            var now = new Date();
            if (date > now) {
                return false;
            }
        }
    }
    
    return true;
}

function FromIsDate() {
    var haveDate = $('#' + RiskhaveDate_ClientId).val();
    if (haveDate == '1')
    {
        var uxDateRange = document.getElementById(RiskMgmtReporting_uxDateRange);
        if (uxDateRange != null && uxDateRange.checked) {
            var date = $find(RiskMgmtReporting_uxDateRangeFrom).get_textBox().value;
            return isDate(date);
        }
    }
    
    return true;
}

function FromDate_GreaterThanToDay() {
    var haveDate = $('#' + RiskhaveDate_ClientId).val();
    if (haveDate == '1')
    {
        var uxDateRange = document.getElementById(RiskMgmtReporting_uxDateRange);
        if (uxDateRange != null && uxDateRange.checked) {
            var date = new Date($find(RiskMgmtReporting_uxDateRangeFrom).get_textBox().value);
            var now = new Date();
            if (date > now) {
                return false;
            }
        }
    }
    
    return true;
}

function EndIsDate() {
    var uxDateRange = document.getElementById(RiskMgmtReporting_uxDateRange);
    if (uxDateRange != null && uxDateRange.checked) {
        var date = $find(RiskMgmtReporting_uxDateRangeTo).get_textBox().value;
        return isDate(date);
    }
    return true;
}

function EndDate_GreaterThanToDay() {
    var haveDate = $('#' + RiskhaveDate_ClientId).val();
    if (haveDate == '1')
    {
        var uxDateRange = document.getElementById(RiskMgmtReporting_uxDateRange);
        if (uxDateRange != null && uxDateRange.checked) {
            var date = new Date($find(RiskMgmtReporting_uxDateRangeTo).get_textBox().value);
            var now = new Date();
            if (date > now) {
                return false;
            }
        }
    }
    
    return true;
}

function EndDate_GreaterThanFromDate() {
    var haveDate = $('#' + RiskhaveDate_ClientId).val();
    if (haveDate == '1')
    {
        var uxDateRange = document.getElementById(RiskMgmtReporting_uxDateRange);
        if (uxDateRange != null && uxDateRange.checked) {
            var fromDate = new Date($find(RiskMgmtReporting_uxDateRangeFrom).get_textBox().value);
            var endDate = new Date($find(RiskMgmtReporting_uxDateRangeTo).get_textBox().value);
            if (fromDate > endDate) {
                return false;
            }
        }
    }
    
    return true;
}

function beforeDownloading() {
    window.isDownloadingReport = true;
}

function ShowMessage(msg) {
    KeepDateOption();
    KeepMerchantOption();
    alert(msg);
}


$(document).ready(function () {
    setTimeout('KeepDateOption()', 50);
    setTimeout('KeepMerchantOption()', 50);
  
})