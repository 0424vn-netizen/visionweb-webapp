var uxFilterSearchKeyText;
var uxFilterSearchKey;
var pnlFollowUpDate;
var pnlFollowUpOption;

var uxcbbFollowUp;
var uxFilterOption;
var uxFilterOpenClosed;
var uxOpenCloseFromDate;
var uxOpenCloseToDate;
var uxDateRangeFollowupFrom;
var uxDateRangeFollowupTo;

var hddProcessData;
var btnProcess;

Sys.Application.add_load(InitRadControl);
$(function () {
    InitControl();
});

$(document).ready(function () {
    // Add check and remove special character when leave input item
    addCheckSpecialCharactersForDate();
    addCheckSpecialCharacters();
});

function addCheckSpecialCharacters() {
    $('#' + rm_EscalationQueue_uxFilterSearchKeyText).keypress(function (e) {
        var code = e.keyCode || e.which;
        var searchKeyTextValue = uxFilterSearchKeyText.value.trim();
        if (code == 13 && searchKeyTextValue.length > 0) {
            if (isIncludeSpecialCharacters(this)) {
                removeSpecialCharacters(this);
            }
            else {
                if (ValidateData()) {
                    document.getElementById(rm_EscalationQueue_uxProxyButton).click();
                }
            }
            return false;
        }
    });
    $('#' + rm_EscalationQueue_uxFilterSearchKeyText).change(function (e) {
        if (isIncludeSpecialCharacters(this)) {
            ValidateData();
            removeSpecialCharacters(this);
            this.focus();
        }
    });
    $('#' + rm_EscalationQueue_uxSavingsLossFrom).change(function (e) {
        if (isIncludeSpecialCharacters(this)) {
            ValidateData();
            removeSpecialCharacters(this);
            this.focus();
        }
    });
    $('#' + rm_EscalationQueue_uxSavingsLossTo).change(function (e) {
        if (isIncludeSpecialCharacters(this)) {
            ValidateData();
            removeSpecialCharacters(this);
            this.focus();
        }
    });
    $('#' + rm_EscalationQueue_uxDateRangeFollowupFrom).change(function (e) {
        if (isIncludeSpecialCharacters(this)) {
            ValidateData();
            removeSpecialCharacters(this);
            this.focus();
        }
    });
    $('#' + rm_EscalationQueue_uxDateRangeFollowupTo).change(function (e) {
        if (isIncludeSpecialCharacters(this)) {
            ValidateData();
            removeSpecialCharacters(this);
            this.focus();
        }
    });
}
function onSearchEscalationQueue() {
    if (ValidateData()) {
        document.getElementById(rm_EscalationQueue_uxProxyButton).click();
    }
    return false;
}

function OnClientKeyPressing(sender, e) {
    var isIE = /MSIE/.test(navigator.userAgent);
    e = e || window.event;
    if (e.keyCode == 13) {
        if (isIE) {
            return false;
        }
        else {
            if (ValidateData())
            {
                document.getElementById(rm_EscalationQueue_uxProxyButton).click();
            }
        }
    }
}

function InitControl() {
    uxFilterSearchKeyText = document.getElementById(rm_EscalationQueue_uxFilterSearchKeyText);
    uxFilterSearchKey = $get("uxFilterSearchKey");
    pnlFollowUpDate = $get('pnlFollowUpDate');
    pnlFollowUpOption = $get("pnlFollowUpOption");
    hddProcessData = $get(rm_EscalationQueue_hddProcessData);
    btnProcess = $get(rm_EscalationQueue_btnProcess);
}
function InitRadControl() {
    uxcbbFollowUp = $find(rm_EscalationQueue_uxcbbFollowUp);
    uxFilterOption = $find(rm_EscalationQueue_uxFilterOption);
    uxFilterOpenClosed = $find(rm_EscalationQueue_uxFilterOpenClosed);
    uxOpenCloseFromDate = $find(rm_EscalationQueue_uxOpenCloseFromDate);
    uxOpenCloseToDate = $find(rm_EscalationQueue_uxOpenCloseToDate);
    uxDateRangeFollowupFrom = $find(rm_EscalationQueue_uxDateRangeFollowupFrom);
    uxDateRangeFollowupTo = $find(rm_EscalationQueue_uxDateRangeFollowupTo);
    uxSavingsLossFrom = $find(rm_EscalationQueue_uxSavingsLossFrom);
    uxSavingsLossTo = $find(rm_EscalationQueue_uxSavingsLossTo);
}
//Search when user focus on textbox
function DefaultEnterOnTextBox(e) {
    alert('11');
    var isEnter = false;
    var isIE = /MSIE/.test(navigator.userAgent);
    var keyCode = isIE ? e.keyCode : e.which;
    isEnter = (keyCode == 13);
    uxSearchButton = document.getElementById(rm_EscalationQueue_uxSearchButton);
    if (isEnter) {
        alert('11');
        if (uxSearchButton) {
            uxSearchButton.focus();
            if (isIE)
                setTimeout("uxSearchButton.click()", 200);
            else
                uxSearchButton.click();
        }
        return false;
    }
}

function escalation_Click(escalationNumber) {
    hddProcessData.value = "escalation;" + escalationNumber;
    btnProcess.click();
    return true;
}

function merchant_Click(merchantNumber) {
    hddProcessData.value = "merchant;" + merchantNumber;
    btnProcess.click();
    return true;
}
function LimitText(fieldObj, maxChars) {
    var result = true;
    var text = document.getElementById(fieldObj);
    if (text.value.length >= maxChars) { result = false; }
    if (window.event) { window.event.returnValue = result; return result; }
}
function uxFilterOpenClosed_OnClientSelectedIndexChanged(sender, args) {
    //var ocFilterValue = args.get_item().get_value();
    var filterOpenClosedValue = uxFilterOpenClosed.get_value();

    $get("uxOpenCloseDateRange").style.display = (filterOpenClosedValue == "OpenBetween"
    || filterOpenClosedValue == "ClosedBetween" ? "inline-block" : "none");
    if (uxOpenCloseFromDate && resetvalue)
        uxOpenCloseFromDate.set_selectedDate(new Date());
    if (uxOpenCloseToDate && resetvalue)
        uxOpenCloseToDate.set_selectedDate(new Date());

    $get("uxSavingsLossRange").style.display = (filterOpenClosedValue == "SavingLossAmtBetween" ? "inline-block" : "none");
    if (uxSavingsLossFrom && resetvalue)
        uxSavingsLossFrom.set_value('');
    if (uxSavingsLossTo && resetvalue)
        uxSavingsLossTo.set_value('');
}

function uxcbbFollowUp_OnClientSelectedIndexChanged(sender, args) {
    if (uxcbbFollowUp.get_value() == 'DATERANGE') {
        pnlFollowUpDate.style.display = "inline-block";
        if (resetvalue) {
            uxDateRangeFollowupFrom.set_selectedDate(new Date());
            uxDateRangeFollowupTo.set_selectedDate(new Date());
        }
    }
    else {
        pnlFollowUpDate.style.display = "none";
    }
}

function uxFilterOption_OnClientSelectedIndexChanged() {
    var filterCbbValue = uxFilterOption.get_value();
    switch (filterCbbValue) {
        case "":
            uxFilterSearchKey.style.display = "none";
            pnlFollowUpOption.style.display = "none";
            pnlFollowUpDate.style.display = "none";
            break;
        case "TNO":
            uxFilterSearchKey.style.display = "inline-block";
            pnlFollowUpOption.style.display = "none";
            pnlFollowUpDate.style.display = "none";
            uxFilterSearchKeyText.maxLength = 9;
            if (resetvalue) {
                uxFilterSearchKeyText.value = '';
            }
            uxFilterSearchKeyText.focus();
            break;
        case "MNO":
            uxFilterSearchKey.style.display = "inline-block";
            pnlFollowUpOption.style.display = "none";
            pnlFollowUpDate.style.display = "none";
            uxFilterSearchKeyText.maxLength = 16;
            if (resetvalue) {
                uxFilterSearchKeyText.value = '';
            }
            uxFilterSearchKeyText.focus();
            break;
        case "MNP":
            uxFilterSearchKey.style.display = "inline-block";
            pnlFollowUpOption.style.display = "none";
            pnlFollowUpDate.style.display = "none";
            if (resetvalue) {
                uxFilterSearchKeyText.value = '';
            }
            uxFilterSearchKeyText.maxLength = 100;
            uxFilterSearchKeyText.focus();
            break;
        case "MNAME":
            uxFilterSearchKey.style.display = "inline-block";
            pnlFollowUpOption.style.display = "none";
            pnlFollowUpDate.style.display = "none";
            if (resetvalue) {
                uxFilterSearchKeyText.value = '';
            }
            uxFilterSearchKeyText.maxLength = 100;
            uxFilterSearchKeyText.focus();
            break;
        case "CNAME":
            uxFilterSearchKey.style.display = "inline-block";
            pnlFollowUpOption.style.display = "none";
            pnlFollowUpDate.style.display = "none";
            if (resetvalue) {
                uxFilterSearchKeyText.value = '';
            }
            uxFilterSearchKeyText.maxLength = 100;
            uxFilterSearchKeyText.focus();
            break;
        case "FOLLOWUP":
            pnlFollowUpOption.style.display = "inline-block";
            uxFilterSearchKey.style.display = "none";
            if (uxcbbFollowUp.get_value() == 'DATERANGE') {
                pnlFollowUpDate.style.display = "inline-block";
            }
            break;
    }
    resetvalue = true;
}

function SelectAllOrNone(id, checked) {
    var listBox = $find(id);

    listBox.trackChanges();

    for (var idx = 0; idx < listBox.get_items().get_count() ; idx++) {
        var item = listBox.get_items().getItem(idx);
        item.set_checked(checked);
    }
    listBox.commitChanges();
}

function rf_ValidateFormat(input, validHexValue) {
    var m = input.match(new RegExp(validHexValue, 'ig'));
    if (m != null) {
        return m == trim(input);
    }
    return false;
}
function ValidateData() {
    var filterOpenClosedValue = uxFilterOpenClosed.get_value();
    var searchKeyTextValue = uxFilterSearchKeyText.value.trim();
    var openCloseFromDate = uxOpenCloseFromDate.get_selectedDate();
    var openCloseToDate = uxOpenCloseToDate.get_selectedDate();

    var followFromDate = uxDateRangeFollowupFrom.get_selectedDate();
    var followToDate = uxDateRangeFollowupTo.get_selectedDate();
    var savingsLossFrom = $find(rm_EscalationQueue_uxSavingsLossFrom).get_value();
    var savingsLossTo = $find(rm_EscalationQueue_uxSavingsLossTo).get_value()
    var uxcbbFollowUpValue = uxcbbFollowUp.get_value();

    var numberOnlyReg = "[0-9]+";
    var alphanumbericReg = '[\-a-z0-9]+$';
    if (isIncludeSpecialCharacters(uxFilterSearchKeyText)) {
        removeSpecialCharacters(uxFilterSearchKeyText);
    }

    if (filterOpenClosedValue == "OpenBetween" || filterOpenClosedValue == "ClosedBetween") {
        var dF = new Date(openCloseFromDate);
        var dT = new Date(openCloseToDate);

        if (dF > new Date() || dT > new Date()) {
            alert(ReportFilter_V10);
            return false;
        }
        else if (dF > dT) {
            alert(ReportFilter_V3);
            return false;
        }
    }
    if (filterOpenClosedValue == 'SavingLossAmtBetween') {
        if ($find(rm_EscalationQueue_uxSavingsLossFrom).get_textBoxValue() == '' && $find(rm_EscalationQueue_uxSavingsLossTo).get_textBoxValue() == '') {
            alert(rm_EscalationQueue_js_SavingsLossFromToRequired);
            return false;
        }
        if ($find(rm_EscalationQueue_uxSavingsLossFrom).get_textBoxValue() == '') {
            alert(rm_EscalationQueue_js_SavingsLossFromRequired);
            return false;
        }
        else if ($find(rm_EscalationQueue_uxSavingsLossTo).get_textBoxValue() == '') {
            alert(rm_EscalationQueue_js_SavingsLossToRequired);
            return false;
        }
        else if (savingsLossFrom > savingsLossTo) {
            alert(rm_EscalationQueue_js_SavingsLossToGreaterThanFrom_text);
            return false;
        }
    }
    var filterCbbValue = uxFilterOption.get_value();
    switch (filterCbbValue) {
        case "TNO":
            if (!rf_ValidateFormat(searchKeyTextValue, numberOnlyReg)) {
                alert(rm_EscalationQueue_js_Alert1);
                uxFilterSearchKeyText.focus();
                return false;
            }
            break;
        case "MNO":
            if (!rf_ValidateFormat(searchKeyTextValue, alphanumbericReg)) {
                alert(rm_EscalationQueue_js_Alert2);
                uxFilterSearchKeyText.focus();
                return false;
            }
            break;
        case "MNP":
            if (searchKeyTextValue.length < 3) {
                alert(rm_EscalationQueue_js_Alert4);
                uxFilterSearchKeyText.focus();
                return false;
            }
            else if (!rf_ValidateFormat(searchKeyTextValue, alphanumbericReg)) {
                alert(rm_EscalationQueue_js_Alert2);
                uxFilterSearchKeyText.focus();
                return false;
            }
            break;
        case "MNAME":
            if (searchKeyTextValue.length == 0) {
                alert(rm_EscalationQueue_js_Alert3);
                uxFilterSearchKeyText.focus();
                return false;
            }
            else
            {
                var reg = new RegExp("(<[^ \t])");
                var isInclude = false;
                if (searchKeyTextValue.indexOf("&#") > -1) {
                    isInclude = true;
                }
                if (reg.test(searchKeyTextValue)) {
                    isInclude = true;
                }
                if (isInclude) {
                    var resultValue = searchKeyTextValue;
                    var reg = new RegExp("(<[^ \t])");
                    var regBlankOrTab = new RegExp("([^ \t])");

                    if (resultValue.indexOf("&#") > -1) {
                        while (resultValue.indexOf("&#") > -1) {
                            resultValue = resultValue.replace(/&#/g, "");
                        }
                    }
                    if (reg.test(resultValue)) {
                        var arrValue = resultValue.split("");
                        for (i = 0; i < arrValue.length - 1; i++) {
                            if (arrValue[i] == "<" && regBlankOrTab.test(arrValue[i + 1])) {
                                arrValue[i] = "";
                            }
                        }
                        resultValue = arrValue.join("");
                    }
                    if (resultValue.length == 0) {
                        alert(rm_EscalationQueue_js_Alert3);
                    }
                    else {
                        alert(content.AlertMsgSpecialCharacters);
                    }
                    uxFilterSearchKeyText.focus();
                    return false;
                }
            }
            break;
        case "FOLLOWUP":
            if (uxcbbFollowUpValue == 'DATERANGE') {

                if (followFromDate == null) {
                    alert(ReportFilter_ReportDate_InvaidDate);
                    return false;
                }
                else if (followToDate == null) {
                    alert(ReportFilter_ReportDate_InvaidDate);
                    return false;
                }
                else if (new Date(followFromDate) > new Date(followToDate)) {
                    alert(ReportFilter_V3);
                    return false;
                }
            }
            break;
    }
    return true;
}
var resetvalue = false;
setTimeout("uxFilterOpenClosed_OnClientSelectedIndexChanged(); uxFilterOption_OnClientSelectedIndexChanged();", 500);
function ajaxRequestStart(sender, args) {
    if (args.get_eventTarget().indexOf('uxExporterTop') != -1) {
        args.set_enableAjax(false);
    }
}
function ajaxResponseEnd(sender, args) {
    UxExporter_OnResponseEnd(sender, args);
    $('[data-hover="dropdown"]').dropdownHover();
}