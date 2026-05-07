
function MerchantNumber_hyperLink_Click(merchantNumber, RecordID) {
    $get(rm_PortfolioCredits_uxHiddenMerchantWorked).value = "true;" + merchantNumber + ";" + RecordID;
    $get(rm_PortfolioCredits_uxChangeMerchantWorked).click();
    return true;
}

function BatchNumber_hyperLink_Click(merchantNumber, RecordID, BatchNumber, TerminalNumber, ReportDate) {
    $get(rm_PortfolioCredits_uxHiddenMerchantWorked).value = "true;" + merchantNumber + ";" + RecordID + ";" + BatchNumber + ";" + TerminalNumber + ";" + ReportDate;
    $get(rm_PortfolioCredits_uxChangeMerchantWorked).click();
    return true;
}

function ChangeMerchantWorked(sender) {
    var merchantInfo = sender.checked + ";dummymerchantnumber;" + sender.value;
    var row = $(sender).parents('tr');
    setTimeout(function () { SubmitMerchantWorked(merchantInfo, row, sender.checked); }, 300);
    return true;
}

function SubmitMerchantWorked(merchantInfo, row, isWorked) {
    PageMethods.ChangeMerchantWorkedStatus(merchantInfo);
    if (isWorked) {
        row.children(".cardNumber").find("span").css("border-bottom", "2px solid #ffb6c1");
        row.find("input.workedBox").attr("checked", true);
    }
    else {
        row.children(".cardNumber").find("span").css("border-bottom", "2px solid Transparent");
        row.find("input.workedBox").removeAttr("checked");
    }
}

function ValidateData() {
    if (!CheckDate()) {
        return false;
    }
    if (!CheckAmount()) {
        return false;
    }
}

function AccountClick(param, merchantNumber) {
    $get(rm_PortfolioCredits_uxHdAccount).value = param;
    $get(rm_PortfolioCredits_uxHiddenMerchantNumber).value = merchantNumber;
    $get(rm_PortfolioCredits_uxAccount).click();
}

function CheckDate() {

    var beginDate = $find(rm_PortfolioCredits_uxBeginDate);
    var toDate = $find(rm_PortfolioCredits_uxEndDate);

    if (beginDate.get_textBox().value == "") {
        alert(String.format(rm_PortfolioCredits_ValidationMessages_V2, rm_PortfolioCredits_js_String3));
        beginDate.get_dateInput().focus();
        return false;
    }
    if (toDate.get_textBox().value == "") {
        alert(String.format(rm_PortfolioCredits_ValidationMessages_V2, rm_PortfolioCredits_js_String3));
        toDate.get_dateInput().focus();
        return false;
    }
    if (!CompareToday(beginDate, 2))
        return false;
    if (!CompareToday(toDate, 2))
        return false;

    if (beginDate.get_selectedDate() > toDate.get_selectedDate()) {
        alert(rm_PortfolioCredits_ReportFilter_V3);
        toDate.get_dateInput().focus();
        return false;
    }

    return true;
}

function CompareToday(datePicker) {
    var currentDate = new Date();
    var datePickerValue = datePicker.get_textBox().value;
    var isValid = isDate(datePickerValue);
    if (!isValid) {
        alert(rm_PortfolioCredits_ReportFilter_ReportDate_InvaidDate);
        datePicker.get_dateInput().focus();
        return false;
    }
    var selectedDate = new Date(datePickerValue);
    if (selectedDate > currentDate) {
        alert(String.format(rm_PortfolioCredits_ValidationMessages_V1b, rm_PortfolioCredits_js_String3));
        datePicker.get_dateInput().focus();
        return false;
    }
    return true;
}
function CheckAmount() {
    var TransactionAmount = document.getElementById(rm_PortfolioCredits_uxTransactionAmount);
    var reg = /^[0-9]+$/;
    if (trim(TransactionAmount.value) == "") {
        alert(rm_PortfolioCredits_js_String1);
        TransactionAmount.focus();
        return false;
    }
    else if (reg.test(TransactionAmount.value) == false) {
        alert(rm_PortfolioCredits_js_String2);
        TransactionAmount.focus();
        return false;
    }
    return true;
}


var isCancelEnterPortfolioCredits = false;

$(function () {
    // Add event click enter from kb
    $('#' + rm_PortfolioCredits_uxFilteringTable + ' input[type=text]').keypress(function (e) {
        var code = e.keyCode || e.which;
        if (code == 13) {
            if (isIncludeSpecialCharacters(this)) {
                if (this.id.includes(rm_PortfolioCredits_uxTransactionAmount)) {
                    CheckAmount();
                }
                removeSpecialCharacters(this);
                return false;
            }
            // case input specialCharacters on DateItem and press Enter: only check at Lostfocus, don't trigger click submit
            if (!this.id.includes(rm_PortfolioCredits_uxTransactionAmount) && isCancelEnterPortfolioCredits) {
                isCancelEnterPortfolioCredits = false;
                e.preventDefault();
                return false;
            }
            $get(rm_PortfolioCredits_uxSearch).click();
            //$("#" + rm_PortfolioCredits_uxSearch).click();
            return false;
        }
    });
    // Add check valid when lostFocus for TransactionAmount
    $('#' + rm_PortfolioCredits_uxTransactionAmount).change(function (e) {
        if (isIncludeSpecialCharacters(this)) {
            CheckAmount();
            removeSpecialCharacters(this);
        }
    });
    // Add check valid when lostFocus for Date items
    $(".filter-item .riTextBox").each(function () {
        $(this).change(function (e) {
            if (isIncludeSpecialCharacters(this)) {
                isCancelEnterPortfolioCredits = true;
                removeSpecialCharacters(this);
                alert(content.AlertMsgInvalidDate);
                setTimeout('isCancelEnterPortfolioCredits = false;', 100);
            }
        });
    });
});

function ChangeWorkedStatusOption() {
    $get(rm_PortfolioCredits_btnChangeWorkedStatusOption).click();
}