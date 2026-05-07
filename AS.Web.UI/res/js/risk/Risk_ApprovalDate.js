var chkIsNewMerchantOnly = $get(Risk_ApprovalDate_chkIsNewMerchantOnly);
var chkEstablishedMerchants = $get(Risk_ApprovalDate_chkIsEstablishedMerchants);

$(document).ready(function () {
    setTimeout("InitFilter();", 300);
});

function InitFilter() {
    var cbxNewMerchantOnly = $find(Risk_ApprovalDate_cbNewMerchantOnly);
    var cbxEstablishedMerchants = $find(Risk_ApprovalDate_cbEstablishedMerchants);
    $find(Risk_ApprovalDate_cbEstablishedMerchants).set_visible(true);
    $find(Risk_ApprovalDate_cbNewMerchantOnly).set_visible(true);

    $("#" + cbxNewMerchantOnly._element.id).css({ "width": "248px" });
    $("#" + cbxEstablishedMerchants._element.id).css({ "width": "248px" });

    if ($get(Risk_ApprovalDate_chkIsNewMerchantOnly).checked) {
        cbxNewMerchantOnly.enable();
    }

    if ($get(Risk_ApprovalDate_chkIsEstablishedMerchants).checked) {
        cbxEstablishedMerchants.enable();
    }
}


function NewMerchantOnlyChecked() {
    var cbxNewMerchantOnly = $find(Risk_ApprovalDate_cbNewMerchantOnly);
    var cbxEstablishedMerchants = $find(Risk_ApprovalDate_cbEstablishedMerchants);

    if ($get(Risk_ApprovalDate_chkIsNewMerchantOnly).checked) {
        cbxNewMerchantOnly.set_visible(true);
        cbxNewMerchantOnly.enable();
    }
    else {
        cbxNewMerchantOnly.disable();
    }
}

function EstablishedMerchantsChecked() {
    var cbxNewMerchantOnly = $find(Risk_ApprovalDate_cbNewMerchantOnly);
    var cbxEstablishedMerchants = $find(Risk_ApprovalDate_cbEstablishedMerchants);

    if ($get(Risk_ApprovalDate_chkIsEstablishedMerchants).checked) {
        cbxEstablishedMerchants.set_visible(true);
        cbxEstablishedMerchants.enable();
    }
    else {
        cbxEstablishedMerchants.disable();
    }
}


function approvalDateChecked() {
    var result = true;
    if (!$get(Risk_ApprovalDate_chkIsNewMerchantOnly).checked && !$get(Risk_ApprovalDate_chkIsEstablishedMerchants).checked) {
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