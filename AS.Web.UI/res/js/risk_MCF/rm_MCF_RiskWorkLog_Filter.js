
function ChangeDateOption(chk) {
    var now = new Date();
    var dateOpt = document.getElementById("divDate");
    var dateRangeOpt = document.getElementById("divDateRange");
    if (chk.value == 'uxRange') {
        dateOpt.style.display = "none";
        dateRangeOpt.style.display = "inline";

        var picker1 = $find(uxFromDate_ClientID);
        var picker2 = $find(uxEndDate_ClientID);


        picker2.set_selectedDate(now);
        now.setDate(1);
        picker1.set_selectedDate(now);
    }
    else {

        dateOpt.style.display = "inline";
        dateRangeOpt.style.display = "none";
        var picker = $find(uxDate_ClientID);
        //if (picker.get_selectedDate() == null) {
        //now.setDate(1);
        picker.set_selectedDate(now);
        //}
    }
} function KeepDateOption() {
    var rangeDate = document.getElementById(uxRange_ClientID);
    if (rangeDate.checked) {
        ChangeDateOption(rangeDate);
    }
}
try {
    KeepDateOption();
}
catch (ex) { }

function ValidateData() {
    var combo = $find(rm_MCF_uxMerchantName);
    if (!!combo) {
        if (!CheckMerchantNameIsScript(combo.get_text())) {
            alert(merchantName_js_ValidationSpecialCharacter);
            combo.set_text("");
            return false
        }
    }
    var merchantID = $find(rm_MCF_uxMerchantID).get_textBoxValue().trim();
    if (!CheckMerchantNumber(merchantID)) {
        $find(rm_MCF_uxMerchantID).focus();
        return false;
    }
    if (!CheckDate()) {
        //$('#' + uxHierarchyValue_ClientID).chosen('');
        return false;
    }
    return true;

}

function uxMerchantName_OnClientTextChange(sender, args) {
    var combo = $find(rm_MCF_uxMerchantName);
    if (!!combo && !CheckMerchantNameIsScript(combo.get_text())) {
        alert(merchantName_js_ValidationSpecialCharacter);
        combo.set_text("");
        return false;
    }
}

function CheckMerchantNameIsScript(value) {
    if (!!value) {
        var reg = /^((?!(\<|\>))(?!(\&\#)).)*$/g;
        if (reg.test(value) == false) {
            return false;
        }
    }
    return true;
}

function CheckMerchantNumber(value) {
    var merchantNumber = value;

    if (trim(merchantNumber).length > 0) {
        var reg = /^[\-a-z0-9]+$/i;
        if (!reg.test(merchantNumber)) {
            alert(String.format(merchantID_InvalidFormatMerchantID_js));
            return false;
        }
    }
    return true;
}

var IsSelectedMerchant = false;
function uxMerchantName_OnClientSelectedIndexChanged(sender, args) {
    if (args.get_item() != null) {
        var value = args.get_item().get_value().trim();
        var uxMerchantID = $find(rm_MCF_uxMerchantID);
        uxMerchantID.set_value(value);
        IsSelectedMerchant = true;
    }
}
function CompareToday(datePicker, type) {
    var currentDate = new Date();
    var datePickerValue = datePicker.get_textBox(uxDate_ClientID).value;
    var isValid = isDate(datePickerValue);
    if (!isValid) {
        alert(Msg_V1);
        return false;
    }
    var selectedDate = new Date(datePickerValue);
    if (selectedDate > currentDate) {
        alert(Msg_V10);
        return false;
    }

    return true;
}

function CheckDate() {
    var beginDate = document.getElementById(uxFromDate_ClientID);
    var endDate = document.getElementById(uxEndDate_ClientID);

    var fromDate = $find(uxFromDate_ClientID);
    var toDate = $find(uxEndDate_ClientID);
    var uxDate = $find(uxDate_ClientID);

    //RadioButton           

    beginDate.value = fromDate.get_textBox().value;
    endDate.value = toDate.get_textBox().value;

    if (uxDaily && uxDaily.checked) {
        if (uxDate.get_textBox().value != "") {
            if (!CompareToday(uxDate, 0))
                return false;
        }
        else {
            alert(Msg_V2);
            return false;
        }
    }
    else if (uxDateRange && uxDateRange.checked) {
        if (fromDate.get_textBox().value == "" || toDate.get_textBox().value == "") {
            alert(Msg_V2);
            return false;
        }
        if (!CompareToday(fromDate, 2))
            return false;
        if (!CompareToday(toDate, 2))
            return false;

        if (fromDate.get_selectedDate() > toDate.get_selectedDate()) {
            alert(Msg_V3);
            return false;
        }
    }
    return true;
}
//Show calendar when user click on date text

function ShowCalendar(type) {
    if (type == '1')
        $find(uxDate_ClientID).showPopup();
    else if (type == '2')
        $find(uxFromDate_ClientID).showPopup();
    else
        $find(uxEndDate_ClientID).showPopup();
}
function SearchEnterOnTextbox(e) {
    var isEnter = false;
    var isIE = /MSIE/.test(navigator.userAgent);
    var keyCode = isIE ? e.keyCode : e.which;
    isEnter = (keyCode == 13);
    if (isEnter) {
        document.getElementById(uxSearch_ClientID).focus();
        setTimeout('doClick()', 100);
        return false;
    }
}

function uxAdministratorName_OnClientKeyPressing(sender, args) {
    var uxAdministratorName = $find(uxAdministratorName_ClientID);
    uxAdministratorName.showDropDown();
}

//Validation Custom
function required(id) {
    var obj = $("#" + id);
    if (obj == null || obj.hasClass("hide"))
        return true;
    else {
        if (!obj.val())
            return false;
    }
    return true;
}

function numberOnly(id) {
    var regx = /^[\d]+$/gm;
    var obj = $("#" + id);
    if (obj == null || obj.hasClass("hide"))
        return true;
    else {
        if (obj.val() && !regx.test(obj.val())) {
            return false;
        }
    }
    return true;
}

function alphaNumberic(id) {
    var regx = /^[\-a-z0-9]+$/i;
    var obj = $("#" + id);
    if (obj == null || obj.hasClass("hide"))
        return true;
    else {
        if (obj.val() && !regx.test(obj.val())) {
            return false;
        }
    }
    return true;
}
function required3digit(id) {
    var value = $find(uxHierarchy_ClientID).get_value();
    if (value == "PARTIALMERCHNUMBER" && $("#" + id).val().length < 3) {
        return false;
    }
    return true;
}

function specialCharacter(id) {
    var obj = $("#" + id);
    if (obj == null || obj.hasClass("hide"))
        return true;
    else {
        var text = obj.val();
        if (text.indexOf('<') != -1 ||
            text.indexOf('>') != -1 ||
            text.indexOf('#') != -1 ||
            text.indexOf('&') != -1) {
            return false;
        }
    }
    return true;
}

function uxHierarchy_OnClientKeyPressing(sender, args) {
    var keyCode = args.get_domEvent().keyCode;
    if (keyCode == 13) {
        return false;
    }
}

$(document).ready(function () {
    addCheckSpecialCharactersForDate();
    checkAndRemoveSpecialCharacters();
});

var isCancelEnterRiskWorkLog = false;
function checkAndRemoveSpecialCharacters() {
    $('#' + rm_MCF_uxMerchantID).change(function (e) {
        if (isIncludeSpecialCharacters(this)) {
            isCancelEnterRiskWorkLog = true;
            CheckMerchantNumber(this.value);

            var newValue = removeSpecialCharacters(this);
            var uxMerchantID = $find(rm_MCF_uxMerchantID);
            uxMerchantID.set_value(newValue);
            setTimeout('isCancelEnterRiskWorkLog = false;', 500);
            this.focus();
        }
    });

    $('#' + rm_MCF_uxMerchantID).keypress(function (e) {
        var code = e.keyCode || e.which;
        if (code == 13) {
            if (isCancelEnterRiskWorkLog) {
                return false;
            }
            document.getElementById(uxSearch_ClientID).focus();
            document.getElementById(uxSearch_ClientID).click();
            return false;
        }
    });

    $('#' + rm_MCF_uxUser + '_Input').change(function (e) {
        if (isIncludeSpecialCharacters(this)) {
            alert(content.AlertMsgSpecialCharacters);
            var newValue = removeSpecialCharacters(this);
            var uxUser = $find(rm_MCF_uxUser);
            uxUser.set_value(newValue);
            uxUser.set_text(newValue);
            if (uxUser._enableLoadOnDemand) {
                uxUser.requestItems(newValue, false);
            }
            this.focus();
        }
    });
}