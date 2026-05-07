
function onAddBathAmount() {
    var amount = $("#" + GenuineBatchAmount_uxBatchAmount).val();
    var currentAmount = $("#" + GenuineBatchAmount_uxHdCurrentValue).val();
    if (amount == "") {
        alert(requiredMsg);
        return false;
    }
    if (amount == "0") {
        alert(geaterThanZero);
        return false;
    }
    
    if (parseFloat(amount).toFixed(2).toString() != currentAmount) {
        __doPostBack(GenuineBatchAmount_uxBntSaveBatchAmount, '');
    }
    return false;
}

function checkDecimal() {

    var obj = $("#" + GenuineBatchAmount_uxBatchAmount);
    var val = obj.val();
    var rexg = /^([0-9]+[\.]?[0-9]?[0-9]?|[0-9]+)/g;
    val = rexg.exec(val);
    if (val) {
        obj.val(val[0]);
    } else {
        obj.val("");
    }
}

function masterAjax_requestStart(sender, args) {
    if (typeof (UxExporter_OnRequestStart) == 'function')
        UxExporter_OnRequestStart(sender, args);
}