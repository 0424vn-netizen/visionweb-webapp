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

/*
function OnPopupOpening(sender, args) {
    var currentDate = new Date();
    var maxDate = currentDate;
    var uxDate = $find(RiskMgmtReporting_uxDate);
    var uxBeginDate = $find(RiskMgmtReporting_uxDateRangeFrom);
    var uxEndDate = $find(RiskMgmtReporting_uxDateRangeTo);
    uxDate.set_maxDate(maxDate);
    uxBeginDate.set_maxDate(maxDate);
    uxEndDate.set_maxDate(maxDate);
}
*/
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

function uxCboMerchant_ClientSelectedIndexChanged() {
    KeepMerchantOption();
}

function KeepMerchantOption() {
    var uxCboMerchant = $find(RiskMgmtReporting_uxCboMerchant);

    var uxCboMerchantValue = uxCboMerchant.get_selectedItem().get_value();
    // 42907 - REORDER FILTER FOR EXTRACTS REPORT
    var uxSearchValueId = $('#' + RiskMgmtReporting_uxSearchValue);
    if (uxCboMerchantValue == "MERCHANTNUMBER") {
        uxSearchValueId.removeAttr("disabled");
        uxSearchValueId.focus();
    }
    else {
        uxSearchValueId.attr("disabled", "disabled");
        uxSearchValueId.val('');
    }
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
    if (haveDate == '1') {
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
    if (haveDate == '1') {
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
    if (haveDate == '1') {
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
    if (haveDate == '1') {
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
    if (haveDate == '1') {
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
    if (haveDate == '1') {
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

function LockFilterForm() {
    if (window.isDownloadingReport) {
        //window.isDownloadingReport = false;
        return;
    }
    var btn = document.getElementById(RiskMgmtReporting_searchBtn);
    if (!!btn) {
        btn.disabled = true;
    }
    var mCbo = $find(RiskMgmtReporting_uxCboMerchant);
    if (mCbo) {
        mCbo.disable();
    }
    var rCbo = $find(RiskMgmtReporting_uxCboReportType);
    if (rCbo) {
        rCbo.disable();
    }
    var dDate = $find(RiskMgmtReporting_uxDate);
    if (dDate) {
        dDate.set_enabled(false);
    }
    var dDateFrom = $find(RiskMgmtReporting_uxDateRangeFrom);
    if (dDateFrom) {
        dDateFrom.set_enabled(false);
    }
    var dDateTo = $find(RiskMgmtReporting_uxDateRangeTo);
    if (dDateTo) {
        dDateTo.set_enabled(false);
    }

    $('#' + RiskMgmtReporting_uxDaily).attr('disabled', 'disabled');
    $('#' + RiskMgmtReporting_uxMonthly).attr('disabled', 'disabled');
    $('#' + RiskMgmtReporting_uxDateRange).attr('disabled', 'disabled'); 
}

function beforeDownloading() {
    window.isDownloadingReport = true;
}

function ShowMessage(msg) {
    KeepDateOption();
    KeepMerchantOption();
    alert(msg);
}

//42924 – VW - Filter Collapse Issue
function uxRCBReportType_ClientSelectedIndexChanged() {
    GetCriteriaReportType();
}

function GetCriteriaReportType() {
    var url = "rm_MgmtReport_Extracts.aspx/GetCriteria";
    var uxCboReportType = $find(RiskMgmtReporting_uxCboReportType);

    var uxCboReportTypeValue = uxCboReportType.get_selectedItem().get_value();
    $.ajax({
        type: "POST",
        url: url,
        data: '{"reportType":"' + uxCboReportTypeValue + '"}',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            var data = result.d;
            if (!!data) {
                $('#' + RiskhaveMerchant_ClientId).val(data[0]);
                $('#' + RiskhaveDate_ClientId).val(data[1]);
                $('#' + RiskMgmtReporting_optionDate).val(data[2]);

                ShowHideMerchantDateCriteria();
            }
        },
        error: function (result) {
            console.log(result);
        }
    });
}

function ShowHideMerchantDateCriteria() {
    var haveMerchant = $('#' + RiskhaveMerchant_ClientId).val();
    var haveDate = $('#' + RiskhaveDate_ClientId).val();
    var optionDate = $('#' + RiskMgmtReporting_optionDate).val();
    $('.daily-js').show();
    $('.month-js').show();
    $('.daterange-js').show();

    if (haveMerchant == '1')
        $('#' + RiskPnlMerchantCriteria_ClientId).show();
    else
        $('#' + RiskPnlMerchantCriteria_ClientId).hide();
    if (haveDate == '1') {
        $('#' + RiskPnlDateCriteria_ClientId).show();
        if (optionDate == '2') { // Monthly option
            $('.daily-js').hide();
            $('.daterange-js').hide();
            document.getElementById(RiskMgmtReporting_uxMonthly).checked = true;
            ChangeDateOption(document.getElementById(RiskMgmtReporting_uxMonthly));
        }
    }
    else
        $('#' + RiskPnlDateCriteria_ClientId).hide();

}

$(document).ready(function () {
    setTimeout('KeepDateOption()', 50);
    setTimeout('KeepMerchantOption()', 50);
    setTimeout('GetCriteriaReportType()', 50);
});

window.onbeforeunload = LockFilterForm;