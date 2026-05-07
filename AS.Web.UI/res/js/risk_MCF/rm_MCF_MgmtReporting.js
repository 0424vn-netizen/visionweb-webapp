//Switch between Daily, Monthly, Date Range
function ChangeDateOption(chk) {

    let now = new Date();
    let dateOpt = $("#" + RiskMgmtReporting_divDate);
    let dateRangeOpt = $("#" + RiskMgmtReporting_divDateRange);

    if (chk.value == 'uxDateRange') {
        dateOpt.addClass("display-none");
        dateRangeOpt.removeClass("display-none");
        let picker1 = $find(RiskMgmtReporting_uxDateRangeFrom);
        let picker2 = $find(RiskMgmtReporting_uxDateRangeTo);
        if (picker2 != null) picker2.set_selectedDate(now);
        now.setDate(1);
        if (picker1 != null) picker1.set_selectedDate(now);

    }
    else {
        dateOpt.removeClass("display-none");
        dateRangeOpt.addClass("display-none");        
        let picker = $find(RiskMgmtReporting_uxDate);
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

    let uxCboMerchant = $find(RiskMgmtReporting_uxCboMerchant);
    if (uxCboMerchant) {
        let uxCboMerchantValue = uxCboMerchant.get_selectedItem().get_value();
        if (uxCboMerchantValue == "MERCHANTNUMBER") {
            let haveMerchant = $('#' + RiskhaveMerchant_ClientId).val();            
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
    let uxCboMerchant = $find(RiskMgmtReporting_uxCboMerchant);
    let uxCboMerchantValue = uxCboMerchant.get_selectedItem().get_value();
    // 42907 - REORDER FILTER FOR EXTRACTS REPORT
    let uxSearchValueId = $('#' + RiskMgmtReporting_uxSearchValue);
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

    let dateOpt = $("#" + RiskMgmtReporting_divDate);
    let dateRangeOpt = $("#" + RiskMgmtReporting_divDateRange);
    let uxDateRange = document.getElementById(RiskMgmtReporting_uxDateRange);
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
    let haveDate = $('#' + RiskhaveDate_ClientId).val();
    if (haveDate == '1') {
        let uxDateRange = document.getElementById(RiskMgmtReporting_uxDateRange);
        if (uxDateRange != null && !uxDateRange.checked) {
            let date = $find(RiskMgmtReporting_uxDate).get_textBox().value;
            return isDate(date);
        }
    }

    return true;
}

function StartDate_GreaterThanToDay() {
    let haveDate = $('#' + RiskhaveDate_ClientId).val();
    if (haveDate == '1') {
        let uxDateRange = document.getElementById(RiskMgmtReporting_uxDateRange);
        if (uxDateRange != null && !uxDateRange.checked) {
            let date = new Date($find(RiskMgmtReporting_uxDate).get_textBox().value);
            let now = new Date();
            if (date > now) {
                return false;
            }
        }
    }
    
    return true;
}

function FromIsDate() {
    let haveDate = $('#' + RiskhaveDate_ClientId).val();
    if (haveDate == '1') {
        let uxDateRange = document.getElementById(RiskMgmtReporting_uxDateRange);
        if (uxDateRange != null && uxDateRange.checked) {
            let date = $find(RiskMgmtReporting_uxDateRangeFrom).get_textBox().value;
            return isDate(date);
        }
    }

    return true;
}

function FromDate_GreaterThanToDay() {
    let haveDate = $('#' + RiskhaveDate_ClientId).val();
    if (haveDate == '1') {
        let uxDateRange = document.getElementById(RiskMgmtReporting_uxDateRange);
        if (uxDateRange != null && uxDateRange.checked) {
            let date = new Date($find(RiskMgmtReporting_uxDateRangeFrom).get_textBox().value);
            let now = new Date();
            if (date > now) {
                return false;
            }
        }
    }

    return true;
}

function EndIsDate() {
    let uxDateRange = document.getElementById(RiskMgmtReporting_uxDateRange);
    if (uxDateRange != null && uxDateRange.checked) {
        let date = $find(RiskMgmtReporting_uxDateRangeTo).get_textBox().value;
        return isDate(date);
    }
    return true;
}

function EndDate_GreaterThanToDay() {
    let haveDate = $('#' + RiskhaveDate_ClientId).val();
    if (haveDate == '1') {
        let uxDateRange = document.getElementById(RiskMgmtReporting_uxDateRange);
        if (uxDateRange != null && uxDateRange.checked) {
            let date = new Date($find(RiskMgmtReporting_uxDateRangeTo).get_textBox().value);
            let now = new Date();
            if (date > now) {
                return false;
            }
        }
    }

    return true;
}

function EndDate_GreaterThanFromDate() {
    let haveDate = $('#' + RiskhaveDate_ClientId).val();
    if (haveDate == '1') {
        let uxDateRange = document.getElementById(RiskMgmtReporting_uxDateRange);
        if (uxDateRange != null && uxDateRange.checked) {
            let fromDate = new Date($find(RiskMgmtReporting_uxDateRangeFrom).get_textBox().value);
            let endDate = new Date($find(RiskMgmtReporting_uxDateRangeTo).get_textBox().value);
            if (fromDate > endDate) {
                return false;
            }
        }
    }

    return true;
}

function LockFilterForm() {
    if (window.isDownloadingReport) {
        return;
    }
    let btn = document.getElementById(RiskMgmtReporting_searchBtn);
    if (!!btn) {
        btn.disabled = true;
    }
    let mCbo = $find(RiskMgmtReporting_uxCboMerchant);
    if (mCbo) {
        mCbo.disable();
    }
    let rCbo = $find(RiskMgmtReporting_uxCboReportType);
    if (rCbo) {
        rCbo.disable();
    }
    let dDate = $find(RiskMgmtReporting_uxDate);
    if (dDate) {
        dDate.set_enabled(false);
    }
    let dDateFrom = $find(RiskMgmtReporting_uxDateRangeFrom);
    if (dDateFrom) {
        dDateFrom.set_enabled(false);
    }
    let dDateTo = $find(RiskMgmtReporting_uxDateRangeTo);
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
    let url = "rm_MCF_MgmtReport_Extracts.aspx/GetCriteria";
    let uxCboReportType = $find(RiskMgmtReporting_uxCboReportType);
    let uxCboReportTypeValue = uxCboReportType.get_selectedItem().get_value();
    $.ajax({
        type: "POST",
        url: url,
        data: '{"reportType":"' + uxCboReportTypeValue + '"}',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            let data = result.d;
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

function ShowHideMerchantDateCriteria()
{
    let haveMerchant = $('#' + RiskhaveMerchant_ClientId).val();
    let haveDate = $('#' + RiskhaveDate_ClientId).val();
    let optionDate = $('#' + RiskMgmtReporting_optionDate).val();
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
        else if (optionDate == '1') { // daily option
            $('.month-js').hide();
            $('.daterange-js').hide();
            document.getElementById(RiskMgmtReporting_uxDaily).checked = true;
            ChangeDateOption(document.getElementById(RiskMgmtReporting_uxDaily));
        } else if (optionDate == '3') { // date range option
            $('.daily-js').hide();
            $('.month-js').hide();
            document.getElementById(RiskMgmtReporting_uxDateRange).checked = true;
            ChangeDateOption(document.getElementById(RiskMgmtReporting_uxDateRange));
        }
    }
    else
        $('#' + RiskPnlDateCriteria_ClientId).hide();

}

$(document).ready(function () {
    setTimeout('KeepDateOption()', 50);
    setTimeout('KeepMerchantOption()', 50);
    setTimeout('GetCriteriaReportType()', 50);
    addCheckSpecialCharactersForDate();
});

window.onbeforeunload = LockFilterForm;