const FORWARD_DELIVERY_NDX_MAX_VALUE = 9999;
const FORWARD_DELIVERY_NDX_PERCENT_MAX_VALUE = 100;
$(document).ready(function () {
    registerInputEvent();
});

function ValidateCalculate() {
    CleartMessageError();
    let days = $("#" + uxDays).val();
    let ndx = $("#" + uxNDX).val();
    let isValidDays = true;
    let isValidNDX = true;
    if (days.length == 0) {
        $("#daysError").text(rm_ForwardDelivery_require);
        isValidDays = false;
    }
    else if (parseInt(days) < 1 || parseInt(days) > FORWARD_DELIVERY_NDX_MAX_VALUE) {
        $("#daysError").text(rm_ForwardDelivery_msg);
        isValidDays = false;
    }
    if (ndx.length == 0) {
        $("#ndxError").text(rm_ForwardDelivery_require);
        isValidNDX = false;
    } else if (parseInt(ndx) < 1 || parseInt(ndx) > FORWARD_DELIVERY_NDX_MAX_VALUE) {
        $("#ndxError").text(rm_ForwardDelivery_msg);
        isValidNDX = false;
    }
    if (!isValidDays) {
        $("#daysError").removeClass("display-none");
        $(".days").addClass("error");
    }
    else {
        $("#daysError").addClass("display-none");
        $(".days").removeClass("error");
    }

    if (!isValidNDX) {
        $("#ndxError").removeClass("display-none");
        $(".ndx").addClass("error");
    }
    else {
        $("#ndxError").addClass("display-none");
        $(".ndx").removeClass("error");
    }

    let isValidNDXPercent = NDXPercentToMaxValue();

    if (!isValidDays || !isValidNDX || !isValidNDXPercent) {
        return false;
    }
    __doPostBack(uxCalculate, '');
}

function preventNonNumbersInInput(event) {
    var characters = String.fromCharCode(event.which);
    if (!(/[0-9]/.test(characters))) {
        event.preventDefault();
    }
}

function pasteTest(event) {
    window.setTimeout(() => {
        var characters = event.target.value;
        window.setTimeout(() => {
            if (!(/^\d+$/.test(characters))) {
                event.target.value = event.target.value.replace(/\D/g, '');
            }
        });
    });
}

NDXPercentToMaxValue = function () {
    let val = $("#" + uxNDXPercent).val();
    if (val != '' && (parseInt(val) < 1 || parseInt(val) > FORWARD_DELIVERY_NDX_PERCENT_MAX_VALUE)) {
        $("#ndxPercentError").text(rm_ForwardDelivery_NDXPercent_Err);
        $("#ndxPercentError").removeClass("display-none");
        $(".ndx-percent").addClass("error");
        return false;
    }
    $("#ndxPercentError").addClass("display-none");
    $(".ndx-percent").removeClass("error");
    return true;
}
CleartMessageError = function () {

    $("#ndxError").text('');
    $("#ndxError").addClass("display-none");
    $(".ndx").removeClass("error");

    $("#ndxPercentError").text('');
    $("#ndxPercentError").addClass("display-none");
    $(".ndx-percent").removeClass("error");

    $("#daysError").text('');
    $("#daysError").removeClass("display-none");
    $(".days").removeClass("error");
    return true;
}

function ValidateClear() {
    CleartMessageError();
    let days = $("#" + uxDaysValue).text();
    if (days !== '—') { // calculate already and have value
        parent.ShowPopupModal('rm_MCF_ClearForwardDeliveryConfirmModal.aspx', 'auto')
    }
    else { // remove value from input
        $("#" + uxDays).val("");
        $("#" + uxNDX).val("");
        $("#" + uxNDXPercent).val("");
    }
}

function ClearForwardDelivery() {
    $("#" + uxClear).click();
    parent.HidePopupModal();
}

function registerInputEvent() {
    $("#" + uxDays).on("keypress", preventNonNumbersInInput);
    $("#" + uxNDX).on("keypress", preventNonNumbersInInput);
    $("#" + uxNDXPercent).on("keypress", preventNonNumbersInInput);
    $("#" + uxDays).on("paste", pasteTest);
    $("#" + uxNDX).on("paste", pasteTest);
    $("#" + uxNDXPercent).on("paste", pasteTest);
}