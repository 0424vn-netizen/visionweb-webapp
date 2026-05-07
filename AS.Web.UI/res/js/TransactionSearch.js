Sys.Application.add_load(InitControl1);

var _full;
var _first;
var _last;
var _entityTextbox;
var _entityType;
var _transOperators;
var _transFrom;
var _authorizationTextbox;
var _transTo;
var _textObj;
var _cardOption;
//var _hierarchyList;
//var _clientList;
//var _clientModes;
var functionValidateForEntityValue;
var functionFormatInputValue;
var alertMessage = TransactionSearch_js_Alert1;
var alertMessage2 = TransactionSearch_js_Alert2;
var alertMesInvalidTransAmount = TransactionSearch_js_Alert3;
var isMerchant = TransactionSearch_js_Mode_IsMerchant;
function InitControl() {
    _full = document.getElementById(TransactionSearch_txtFullCard);
    _first = document.getElementById(TransactionSearch_uxFirst6);
    _last = document.getElementById(TransactionSearch_uxLast4);
    _firstRouting = document.getElementById(TransactionSearch_txtRouting);
    _lastAccount = document.getElementById(TransactionSearch_txtAccount);
    _authorizationTextbox = document.getElementById(TransactionSearch_uxAuthNumber);
    _entityType = document.getElementById(TransactionSearch_uxFilterOption + '_uxReportFilter');
    var _divTransactionFilter = $('#js-TransactionFilter');
    if (isMerchant == 'True') {
        _divTransactionFilter.removeClass('border');
    }
    if (!_full) {
        if (_entityType) {
            alertMessage = TransactionSearch_js_Alert4;
            alertMessage2 = TransactionSearch_js_Alert5;
        }
        else {
            alertMessage = TransactionSearch_js_Alert6;
            alertMessage2 = TransactionSearch_js_Alert7;
        }
    }
}
function InitControl1() {
    _transFrom = $find(TransactionSearch_uxTransAmountFrom);
    _transTo = $find(TransactionSearch_uxTransAmountTo);
    _transOperators = $find(TransactionSearch_uxTransOperators);

    var valueOfTransOperators = _transOperators.get_selectedItem().get_value();

    //46652 - AW Multi-currency Transaction Display
    _transFrom.get_numberFormat().NegativePattern = _transTo.get_numberFormat().NegativePattern = TransactionSearch_NegativePattern;
    //handleBehaviorForHierarchyList(hierarchyMode);
    handleBehaviorForTransactionAmount(valueOfTransOperators);
}

function ChangeDateOption(chk) {
    var now = new Date();
    var dateOpt = document.getElementById("divDate");
    var dateRangeOpt = document.getElementById("divDateRange");
    if (chk.value == 'uxRange') {
        dateOpt.style.display = "none";
        dateRangeOpt.style.display = "";
        var picker1 = $find(TransactionSearch_uxFromDate);
        var picker2 = $find(TransactionSearch_uxEndDate);
        picker2.set_selectedDate(now);
        if (TransactionSearch_FILTERING_OPTIONS_DATERANGE == 'True') {
            now.setDate(now.getDate() - 90);
        }
        else {
            now.setDate(1);
        }
        picker1.set_selectedDate(now);
    }
    else {
        dateOpt.style.display = "";
        dateRangeOpt.style.display = "none";
        var picker = $find(TransactionSearch_uxDate);
        picker.set_selectedDate(now);
    }
} function KeepDateOption() {
    var rangeDate = document.getElementById(TransactionSearch_uxRange);
    if (rangeDate.checked) {
        ChangeDateOption(rangeDate);
    }
}
try {
    KeepDateOption();
}
catch (ex) { }

function CompareToday(datePicker, type, fieldName) {
    var currentDate = new Date();
    var datePickerValue = datePicker.get_textBox().value;
    var isValid = isDate(datePickerValue);

    if (!isValid) {
        alert(fieldName + ": " + TransactionSearch_ReportFilter_V1);
        return false;
    }
    var selectedDate = new Date(datePickerValue);
    if (selectedDate > currentDate) {
        var object;
        switch (type) {
            case 0: object = TransactionSearch_js_Daily; break;
            case 1: object = TransactionSearch_js_Monthly; break;
            case 2: object = TransactionSearch_js_Range; break;
        }
        alert(String.format(TransactionSearch_ReportFilter_V9, object));
        return false;
    }
    return true;
}

function CheckDate(isCheckToDate = false) {
    var beginDate = document.getElementById(TransactionSearch_uxFromDate);
    var endDate = document.getElementById(TransactionSearch_uxEndDate);

    var fromDate = $find(TransactionSearch_uxFromDate);
    var toDate = $find(TransactionSearch_uxEndDate);
    var uxDate = $find(TransactionSearch_uxDate);

    //RadioButton           
    var uxDaily = document.getElementById(TransactionSearch_uxDaily);
    var uxMonthly = document.getElementById(TransactionSearch_uxMonthly);
    var uxDateRange = document.getElementById(TransactionSearch_uxRange);

    beginDate.value = fromDate.get_textBox().value;
    endDate.value = toDate.get_textBox().value;

    if (uxDaily && uxDaily.checked) {
        if (uxDate.get_textBox().value != "") {
            if (!CompareToday(uxDate, 0, TransactionSearch_js_Daily))
                return false;
        }
        else {
            alert(TransactionSearch_js_Daily + ": " + TransactionSearch_ReportFilter_ReportDate_InvaidDate);
            return false;
        }
    }
    else if (uxMonthly && uxMonthly.checked) {
        if (uxDate.get_textBox().value != "") {
            if (!CompareToday(uxDate, 1, TransactionSearch_js_Monthly))
                return false;
        }
        else {
            alert(TransactionSearch_js_Monthly + ": " + TransactionSearch_ReportFilter_ReportDate_InvaidDate);
            return false;
        }
    }
    else if (uxDateRange && uxDateRange.checked) {
        if (fromDate.get_textBox().value == "" || toDate.get_textBox().value == "") {
            alert(TransactionSearch_js_Range + ": " + TransactionSearch_ReportFilter_ReportDate_InvaidDate);
            return false;
        }
        if (!isCheckToDate && !CompareToday(fromDate, 2, TransactionSearch_js_FromDate))
            return false;
        if (!CompareToday(toDate, 2, TransactionSearch_js_ToDate))
            return false;

        if (fromDate.get_selectedDate() > toDate.get_selectedDate()) {
            alert(TransactionSearch_js_Range + ": " + TransactionSearch_ReportFilter_V3);
            return false;
        }
    }    

    return true;
}

function validateCardNumber(itemCheck = "") {
    var regFull = /^[0-9\s*]{1,20}$/; var regPartial = /^([0-9]+)$/;
    if ((itemCheck == "" || itemCheck == "fullCard") && _full) {
        var fullCardValue = _full.value;
        if (trim(fullCardValue) != '') {
            if (TransactionSearch_js_FCN.length > 0 && TransactionSearch_js_FCN[TransactionSearch_js_FCN.length - 1] == ':') {
                TransactionSearch_js_FCN = TransactionSearch_js_FCN.substring(0, TransactionSearch_js_FCN.length - 1);
            }
            _textObj = TransactionSearch_js_FCN;
            if (regFull.test(fullCardValue) == false) {
                alert(String.format(TransactionSearch_ReportFilter_V7, _textObj));
                _full.focus();
                _full.select();
                return false;
            }
            if (fullCardValue.length > 20 || fullCardValue.length < 12) {
                alert(String.format('{0}: ' + TransactionSearch_js_TheLengthMustBe12_20, _textObj));
                _full.focus();
                _full.select();
                return false;
            }
        }
    }
    if ((itemCheck == "" || itemCheck == "firstCard") && trim(_first.value) != '' && trim(_last.value) != '') {
        _textObj = TransactionSearch_js_CardNumberF6L4;
        if (regPartial.test(_first.value) == false && regPartial.test(_last.value) == false) {
            alert(String.format(TransactionSearch_ReportFilter_V7, _textObj));
            _first.focus();
            _first.select();
            return false;
        }
        if (_first.value.length != 6 && _last.value.length != 4) {
            alert(String.format(TransactionSearch_ReportFilter_V8, _textObj));
            _first.focus();
            _first.select();
            return false;
        }
    }
    var first6Message = '';
    if (TransactionSearch_js_CardFirst6.length > 0) {
        first6Message = TransactionSearch_js_CardFirst6.substring(0, TransactionSearch_js_CardFirst6.length - 1);

    }
    _textObj = first6Message;
    if ((itemCheck == "" || itemCheck == "firstCard") && trim(_first.value) != '') {
        if (regPartial.test(_first.value) == false) {
            alert(String.format(TransactionSearch_ReportFilter_V7, _textObj));
            _first.focus();
            _first.select();
            return false;
        }
        if (trim(_first.value).length != 6 & trim(_first.value).length != 8) {
            alert(String.format(reportFilter_CardNumberLength, _textObj, 6, 8));
            _first.focus();
            _first.select();
            return false;
        }
    }
    if ((itemCheck == "" || itemCheck == "lastCard") && trim(_last.value) != '') {
        _textObj = TransactionSearch_js_CardNumberL4;
        if (regPartial.test(_last.value) == false) {
            alert(String.format(TransactionSearch_ReportFilter_V7, _textObj));
            _last.focus();
            _last.select();
            return false;
        }
        if (trim(_last.value).length != 4) {
            alert(String.format(TransactionSearch_ReportFilter_V21, _textObj, 4));
            _last.focus();
            _last.select();
            return false;
        }
    }
    return true;
}

function validateRoutingAccountNumber(itemCheck = "") {
    if (isRouting) {
        if ((itemCheck == "" || itemCheck == "firstRouting") && trim(_firstRouting.value) != '' && trim(_lastAccount.value) != '') {
            _textObj = TransactionSearch_js_RoutingNumberF6L4;
            if (regPartial.test(_firstRouting.value) == false && regPartial.test(_lastAccount.value) == false) {
                alert(String.format(TransactionSearch_ReportFilter_V7, _textObj));
                _firstRouting.focus();
                _firstRouting.select();
                return false;
            }
            if (_firstRouting.value.length != 6 && _lastAccount.value.length != 4) {
                alert(String.format(TransactionSearch_ReportFilter_V8, _textObj));
                _firstRouting.focus();
                _firstRouting.select();
                return false;
            }
        }

        _textObj = TransactionSearch_js_RoutingFirst6;
        if ((itemCheck == "" || itemCheck == "firstRouting") && trim(_firstRouting.value) != '') {
            if (regPartial.test(_firstRouting.value) == false) {
                alert(String.format(TransactionSearch_ReportFilter_V7, _textObj));
                _firstRouting.focus();
                _firstRouting.select();
                return false;
            }
            if (trim(_firstRouting.value).length != 6) {
                alert(String.format(TransactionSearch_ReportFilter_V21, _textObj, 6));
                _firstRouting.focus();
                _firstRouting.select();
                return false;
            }
        }
        if ((itemCheck == "" || itemCheck == "lastAccount") && trim(_lastAccount.value) != '') {
            _textObj = TransactionSearch_js_AccountNumberL4;
            if (regPartial.test(_lastAccount.value) == false) {
                alert(String.format(TransactionSearch_ReportFilter_V7, _textObj));
                _lastAccount.focus();
                _lastAccount.select();
                return false;
            }
            if (trim(_lastAccount.value).length != 4) {
                alert(String.format(TransactionSearch_ReportFilter_V21, _textObj, 4));
                _lastAccount.focus();
                _lastAccount.select();
                return false;
            }
        }
    }
    return true;
}

function validateAmount() {
    if (_transOperators.get_value() == 'Between') {
        if (_transFrom.get_textBoxValue() != '' && _transTo.get_textBoxValue() == '') {
            _textObj = TransactionSearch_js_TransactionAmountTo;
            alert(String.format(TransactionSearch_ReportFilter_V2, _textObj));
            _transTo.focus();
            return false;
        }
        if (_transFrom.get_textBoxValue() == '' && _transTo.get_textBoxValue() != '') {
            _textObj = TransactionSearch_js_TransactionAmountFrom;
            alert(String.format(TransactionSearch_ReportFilter_V2, _textObj));
            _transFrom.focus();
            return false;
        }
        if (_transFrom.get_textBoxValue() != '' && _transTo.get_textBoxValue() != ''
                        && parseFloat(_transFrom.get_value()) > parseFloat(_transTo.get_value())) {
            alert(TransactionSearch_Generic_FromGreaterTo);
            _transFrom.focus();
            _transFrom.selectAllText();
            return false;
        }
    }
    return true;
}

function validateAuthoriztion() {
    var authorNumber = trim(_authorizationTextbox.value);
    if (authorNumber != '') {
        var reg = /^[a-zA-Z0-9]{1,16}$/;
        if (reg.test(authorNumber) == false) {

            if (TransactionSearch_js_AuthNumber.length > 0 && TransactionSearch_js_AuthNumber[TransactionSearch_js_AuthNumber.length - 1] == ':') {
                TransactionSearch_js_AuthNumber = TransactionSearch_js_AuthNumber.substring(0, TransactionSearch_js_AuthNumber.length - 1);
            }
            _textObj = TransactionSearch_js_AuthNumber;
            alert(String.format(TransactionSearch_ReportFilter_V6, _textObj));
            _authorizationTextbox.focus();
            _authorizationTextbox.select();
            return false;
        }
    }
    return true;
}

function validateBussiness() {

    var fromDate = $find(TransactionSearch_uxFromDate);
    var toDate = $find(TransactionSearch_uxEndDate);
    var uxDate = $find(TransactionSearch_uxDate);

    //RadioButton           
    var uxDaily = document.getElementById(TransactionSearch_uxDaily);
    var uxMonthly = document.getElementById(TransactionSearch_uxMonthly);
    var uxDateRange = document.getElementById(TransactionSearch_uxRange);

    var fullCardValue = "";
    var entityValue = "";

    var ONE_DAY = 1000 * 60 * 60 * 24;

    if (_full)
        fullCardValue = trim(_full.value);
    //
    if (_entityType)
        entityValue = rf_getHierarchyValue();

    if (_full) {

        if (trim(_first.value) == '' && trim(_last.value) == '' && trim(_full.value) == '' && isMerchant != 'True'
            && trim(entityValue) == '' && trim(_authorizationTextbox.value) == ''
            && (_transOperators.get_value() != 'EqualTo' || _transFrom.get_textBoxValue() == '') && IsMSSystems == 'False') {
            alert(alertMessage);
            return false;
        }
        if (trim(_first.value) == '' && trim(_last.value) == '' && trim(_full.value) == ''
            && trim(entityValue) == '' && trim(_authorizationTextbox.value) == ''
            && uxDateRange && uxDateRange.checked && (toDate.get_selectedDate() - fromDate.get_selectedDate() > 90 * ONE_DAY)) {
            alert(alertMessage2);
            return false;
        }

    }
    else {
        if (_entityType) {
            if (isRouting) {
                if (trim(_first.value) == '' && trim(_last.value) == '' && trim(_firstRouting.value) == '' && trim(_lastAccount.value) == '' && trim(entityValue) == '' && isMerchant != 'True'
                    && trim(_authorizationTextbox.value) == '' && (_transOperators.get_value() != 'EqualTo' || _transFrom.get_textBoxValue() == '') && IsMSSystems == 'False') {
                    alert(alertMessage);
                    return false;
                }
                if (trim(_first.value) == '' && trim(_last.value) == '' && trim(_firstRouting.value) == '' && trim(_lastAccount.value) == '' && trim(entityValue) == '' && trim(_authorizationTextbox.value) == ''
                    && uxDateRange && uxDateRange.checked && (toDate.get_selectedDate() - fromDate.get_selectedDate() > 90 * ONE_DAY)) {
                    alert(alertMessage2);
                    return false;
                }
            }
            else {
                if (trim(_first.value) == '' && trim(_last.value) == '' && trim(entityValue) == '' && isMerchant != 'True'
                    && trim(_authorizationTextbox.value) == '' && (_transOperators.get_value() != 'EqualTo' || _transFrom.get_textBoxValue() == '') && IsMSSystems == 'False') {
                    alert(alertMessage);
                    return false;
                }
                if (trim(_first.value) == '' && trim(_last.value) == '' && trim(entityValue) == '' && trim(_authorizationTextbox.value) == ''
                    && uxDateRange && uxDateRange.checked && (toDate.get_selectedDate() - fromDate.get_selectedDate() > 90 * ONE_DAY)) {
                    alert(alertMessage2);
                    return false;
                }
            }
        }
        else {
            if (isRouting) {
                if (trim(_first.value) == '' && trim(_last.value) == '' && trim(_firstRouting.value) == '' && trim(_lastAccount.value) == '' && isMerchant != 'True'
                    && trim(_authorizationTextbox.value) == '' && (_transOperators.get_value() != 'EqualTo' || _transFrom.get_textBoxValue() == '') && IsMSSystems == 'False') {
                    alert(alertMessage);
                    return false;
                }
                if (trim(_first.value) == '' && trim(_last.value) == '' && trim(_firstRouting.value) == '' && trim(_lastAccount.value) == '' && trim(_authorizationTextbox.value) == ''
                    && uxDateRange && uxDateRange.checked && (toDate.get_selectedDate() - fromDate.get_selectedDate() > 90 * ONE_DAY)) {
                    alert(alertMessage2);
                    return false;
                }
            }
            else {
                if (trim(_first.value) == '' && trim(_last.value) == '' && isMerchant != 'True'
                    && trim(_authorizationTextbox.value) == '' && (_transOperators.get_value() != 'EqualTo' || _transFrom.get_textBoxValue() == '') && IsMSSystems == 'False') {
                    alert(alertMessage);
                    return false;
                }
                if (trim(_first.value) == '' && trim(_last.value) == '' && trim(_authorizationTextbox.value) == ''
                    && uxDateRange && uxDateRange.checked && (toDate.get_selectedDate() - fromDate.get_selectedDate() > 90 * ONE_DAY)) {
                    alert(alertMessage2);
                    return false;
                }
            }
        }
    }
    return true;
}

var allowClick = true;
function ValidateData(idOfItemCheck = "") {
    if (!allowClick) return false;
    allowClick = false;

    if (idOfItemCheck == "" && !CheckDate()) {
        allowClick = true;
        return false;
    }

    var checkCardNumber = "";
    if (idOfItemCheck.includes(TransactionSearch_txtFullCard))
        checkCardNumber = "fullCard";
    else if (idOfItemCheck.includes(TransactionSearch_uxFirst6))
        checkCardNumber = "firstCard";
    else if (idOfItemCheck.includes(TransactionSearch_uxLast4))
        checkCardNumber = "lastCard";

    if ((idOfItemCheck == "" || checkCardNumber != "") && !validateCardNumber(checkCardNumber)) {
        allowClick = true;
        return false;
    }

    var checkRoutingAccount = "";
    if (idOfItemCheck.includes(TransactionSearch_txtRouting))
        checkRoutingAccount = "firstRouting";
    else if (idOfItemCheck.includes(TransactionSearch_txtAccount))
        checkRoutingAccount = "lastAccount";

    if ((idOfItemCheck == "" || checkRoutingAccount != "") && !validateRoutingAccountNumber(checkRoutingAccount)) {
        allowClick = true;
        return false;
    }

    if ((idOfItemCheck == "" || idOfItemCheck.includes(TransactionSearch_uxAuthNumber)) && !validateAuthoriztion()) {
        allowClick = true;
        return false;
    }
    if (!validateAmount()) {
        allowClick = true;
        return false;
    }
    if (!validateBussiness()) {
        allowClick = true;
        return false;
    }

    if (_entityType) {
        SubmitFilter();
        //Do not perform search action
        allowClick = true;
        return false;
    }
    return true;
}


function SubmitFilter() {
    document.getElementById(TransactionSearch_uxFilterOption + "_uxReportFilter_btSubmit").click();
    return false;
}

//Search when user focus on textbox
function doClick() {
    if (!isCancelEnter) {
        document.getElementById(TransactionSearch_uxSearch).click();
    }
}
function SearchEnterOnTextbox(e) {
    var isEnter = false;
    var isIE = /MSIE/.test(navigator.userAgent);
    var keyCode = isIE ? e.keyCode : e.which;
    isEnter = (keyCode == 13);
    if (isEnter) {
        document.getElementById(TransactionSearch_uxSearch).focus();
        setTimeout('doClick()', 100);
        return false;
    }
}
function SearchEnterOnRadNumeric(sender, eventArgs) {
    if (/MSIE/.test(navigator.userAgent))
        return true;
    if (eventArgs.get_keyCode() == 13) {
        document.getElementById(TransactionSearch_uxSearch).focus();
        document.getElementById(TransactionSearch_uxSearch).click();
        eventArgs.set_cancel(true);
    }
}

function transAmount_OnClientSelectedIndexChanged(sender, evt) {
    var value = evt.get_item().get_value();
    _transFrom.clear();
    _transTo.clear();
    handleBehaviorForTransactionAmount(value);
}

function handleBehaviorForTransactionAmount(value) {
    switch (value) {
        case "Between":
            _transFrom.set_visible(true);
            _transTo.set_visible(true);
            $('.option').removeClass("hide");
            break;
        case "None":
            _transFrom.set_visible(false);
            _transTo.set_visible(false);
            _transFrom.clear();
            _transTo.clear();
            $('.option').addClass("hide");
            break;
        default:
            _transFrom.set_visible(true);
            _transTo.set_visible(false);
            $('.option').addClass("hide");
            break;
    }
}

function getIEVersion() {
    var rv = null;
    if (navigator.appName == 'Microsoft Internet Explorer') {
        var ua = navigator.userAgent;
        var reg = new RegExp("MSIE ([0-9]{1,}[\.0-9]{0,})");
        if (reg.exec(ua) != null)
            rv = parseFloat(RegExp.$1);
    }
    return rv;
}

var isCancelEnter = false;
function addCheckSpecialCharacters() {
    // add check when lost focus for Date items
    $(".filter-item .riTextBox").each(function () {
        $(this).change(function (e) {
            if (isIncludeSpecialCharacters(this)) {
                isCancelEnter = true;
                var isEndDate = this.id.includes(TransactionSearch_uxEndDate) ? true : false;
                CheckDate(isEndDate);
                removeSpecialCharacters(this);
                setTimeout('isCancelEnter = false;', 200);
            }
        });
    });
    // add check when lost focus for other items
    $(".filter-item .rf_TextBox").each(function () {
        $(this).change(function (e) {
            var invalidMsg = $(this).attr("invalidmsg");
            if (invalidMsg == undefined || invalidMsg.trim() == "") {
                invalidMsg = $(this).attr("invalidformat");
            }
            // no trigger because be handle in MPSReportFilter
            if (invalidMsg != undefined && invalidMsg.trim() != "") {
                return;
            }
            if (isIncludeSpecialCharacters(this)) {
                isCancelEnter = true;
                ValidateData(this.id);
                removeSpecialCharacters(this);
                setTimeout('isCancelEnter = false;', 200);
            }
        })
    });
}

$(document).ready(function () {
    InitControl();
    addCheckSpecialCharacters();
});