var chkIsNewMerchantOnly = $get(Risk_FistBatchDate_chkIsNewMerchantOnly);
var chkEstablishedMerchants = $get(Risk_FistBatchDate_chkIsEstablishedMerchants);

$(document).ready(function () {
    setTimeout("InitFilter();", 300);
});

function InitFilter() {
    var cbxNewMerchantOnly = $find(Risk_FistBatchDate_cbNewMerchantOnly);
    var cbxEstablishedMerchants = $find(Risk_FistBatchDate_cbEstablishedMerchants);
    $find(Risk_FistBatchDate_cbEstablishedMerchants).set_visible(true);
    $find(Risk_FistBatchDate_cbNewMerchantOnly).set_visible(true);

    $("#" + cbxNewMerchantOnly._element.id).css({ "width": "248px" });
    $("#" + cbxEstablishedMerchants._element.id).css({ "width": "248px" });

    if ($get(Risk_FistBatchDate_chkIsNewMerchantOnly).checked) {
        cbxNewMerchantOnly.enable();
    }

    if ($get(Risk_FistBatchDate_chkIsEstablishedMerchants).checked) {
        cbxEstablishedMerchants.enable();
    }
}


function FirstBatchNewMerchantOnlyChecked() {
    var cbxNewMerchantOnly = $find(Risk_FistBatchDate_cbNewMerchantOnly);
    var cbxEstablishedMerchants = $find(Risk_FistBatchDate_cbEstablishedMerchants);

    if ($get(Risk_FistBatchDate_chkIsNewMerchantOnly).checked) {
        cbxNewMerchantOnly.set_visible(true);
        cbxNewMerchantOnly.enable();
    }
    else {
        cbxNewMerchantOnly.disable();
    }
}

function FirstBatchEstablishedMerchantsChecked() {
    var cbxNewMerchantOnly = $find(Risk_FistBatchDate_cbNewMerchantOnly);
    var cbxEstablishedMerchants = $find(Risk_FistBatchDate_cbEstablishedMerchants);

    if ($get(Risk_FistBatchDate_chkIsEstablishedMerchants).checked) {
        cbxEstablishedMerchants.set_visible(true);
        cbxEstablishedMerchants.enable();
    }
    else {
        cbxEstablishedMerchants.disable();
    }
}


function fistBatchDateChecked() {
    var result = true;
    if (!$get(Risk_FistBatchDate_chkIsNewMerchantOnly).checked && !$get(Risk_FistBatchDate_chkIsEstablishedMerchants).checked) {
        result = false;
    }
    return result;
}



function CancelDropDown(sender, eventArgs) {
    eventArgs.set_cancel(true);
}

function CancelKeyPress(sender, eventArgs) {
    eventArgs.get_domEvent().keyCode = null;
}