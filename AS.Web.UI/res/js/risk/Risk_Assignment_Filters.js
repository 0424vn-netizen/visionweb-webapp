function filter_closeModalEvent(modalID, clientIDbtn) {
    switch (modalID) {
        case 'HierarchyModal':
            document.getElementById(clientIDbtn).click();
            break;
        case 'SICModal':
            document.getElementById(Risk_Assignment_Filters_btnRefreshSIC).click();
            break;
        case 'StateModal':
            document.getElementById(Risk_Assignment_Filters_btnRefreshState).click();
            break;
        case 'ProfileModal':
            document.getElementById(Risk_Assignment_Filters_btnRefreshProfile).click();
            break;
        case 'HRCodeModal':
            document.getElementById(Risk_Assignment_Filters_btnRefreshHighRisk).click();
            break;
        case 'MerchantRankModal':
            document.getElementById(Risk_Assignment_Filters_btnRefreshMerchantRank).click();
            break;
        case 'ACHHoldDaysModal':
            document.getElementById(Risk_Assignment_Filters_btnRefreshACHHoldDays).click();
            break;
        case 'OwnerLastNameModal':
            document.getElementById(Risk_Assignment_Filters_btnRefreshOwnerLastName).click();
            break;
        case 'ZipCodeModal':
            document.getElementById(Risk_Assignment_Filters_btnRefreshZipCode).click();
            break;
        case 'MerchantFundingStatusModal':
            document.getElementById(Risk_Assignment_Filters_btnRefreshMerchantFundingStatus).click();
            break;
        case 'MerchantClassificationsModal':
            document.getElementById(Risk_Assignment_Filters_btnbtnRefreshMC).click();
            break;
    }
}

function CallFilterModal1(modal, width, height) {
    if (!ValidateAssignmentGeneralInformationByValidator()) {
        moveTo('#markupAssinfo');
        return false;
    }
    return doOpenSubPopup(modal + Risk_Assignment_Filters_QueryString, 'auto');
}

function CallFilterModal2(modal, width, height) {
    if (!ValidateAssignmentGeneralInformationByValidator()) {
        return false;
    }
    return doOpenSubPopup(modal, 'auto');
}

function CallHierarchyFilterModal(modal, width, height) {
    if (!ValidateAssignmentGeneralInformationByValidator()) {
        moveTo('#markupAssinfo');
        return false;
    }
    return doOpenSubPopup(modal, 'auto');
}

function DisplayMerchantCountOnFilter(merchantCountOnFilter) {
    $get(Risk_Assignment_Filters_pnlMerchantCountOnFilter).innerHTML = addCommas(merchantCountOnFilter);
}

function addCommas(nStr) {
    nStr += '';
    x = nStr.split('.');
    x1 = x[0];
    x2 = x.length > 1 ? '.' + x[1] : '';
    var rgx = /(\d+)(\d{3})/;
    while (rgx.test(x1)) {
        x1 = x1.replace(rgx, '$1' + ',' + '$2');
    }
    return x1 + x2;
}

function ValidateMerchantRangeFromTo() {
    var isValid = true;
    var merchantRangeFrom = document.getElementById(Risk_Assignment_Filters_txtMerchantRangeFrom).value;
    var merchantRangeTo = document.getElementById(Risk_Assignment_Filters_txtMerchantRangeTo).value;
    if (merchantRangeFrom.length > 0 && merchantRangeTo.length > 0) {
        //Step 2: Check From > To
        var fromValue = parseFloat(merchantRangeFrom);
        var toValue = parseFloat(merchantRangeTo);

        if (fromValue > toValue)
            isValid = false;
    }

    return isValid;
}

function ValidateMerchantRangeFromRequired() {
    var isValid = true;
    var merchantRangeFrom = document.getElementById(Risk_Assignment_Filters_txtMerchantRangeFrom).value;
    var merchantRangeTo = document.getElementById(Risk_Assignment_Filters_txtMerchantRangeTo).value;
    if ((merchantRangeFrom.length > 0 && merchantRangeTo.length > 0) ||
        (merchantRangeFrom.length == 0 && merchantRangeTo.length == 0)) {
        isValid = true;
    }
    else if (merchantRangeFrom.length == 0 && merchantRangeTo.length > 0) {
        document.getElementById(Risk_Assignment_Filters_txtMerchantRangeFrom).focus();
        isValid = false;
    }
    return isValid;
}

function ValidateMerchantRangeToRequired() {
    var isValid = true;

    var merchantRangeFrom = document.getElementById(Risk_Assignment_Filters_txtMerchantRangeFrom).value;
    var merchantRangeTo = document.getElementById(Risk_Assignment_Filters_txtMerchantRangeTo).value;
    if ((merchantRangeFrom.length > 0 && merchantRangeTo.length > 0) ||
        (merchantRangeFrom.length == 0 && merchantRangeTo.length == 0)) {
        isValid = true;
    }
    else if (merchantRangeFrom.length > 0 && merchantRangeTo.length == 0) {
        document.getElementById(Risk_Assignment_Filters_txtMerchantRangeTo).focus();
        $('label[for=' + Risk_Assignment_Filters_txtMerchantRangeFrom + ']').addClass('label-error');
        isValid = false;
    }
    return isValid;
}



function ValidateMerchantRange() {

    var merchantRangeStatus = 'none';
    //Merchant Range
    var merchantRangeFrom = document.getElementById(Risk_Assignment_Filters_txtMerchantRangeFrom).value;
    var merchantRangeTo = document.getElementById(Risk_Assignment_Filters_txtMerchantRangeTo).value;

    if (ValidateMerchantRangeNumber() == false) {
        merchantRangeStatus = 'invalid';
        return merchantRangeStatus;
    }

    if (merchantRangeFrom.length > 0 && merchantRangeTo.length > 0) {
        //Step 1: Numeric only

        //Step 2: Check From > To
        var fromValue = parseFloat(merchantRangeFrom);
        var toValue = parseFloat(merchantRangeTo);
        var isValid = true;
        if (fromValue > toValue)
            isValid = false;
        if (!isValid) {
            //alert('The “To” value must be greater than the “From” value.');
        }
        if (isValid)
            merchantRangeStatus = 'valid';
        else
            merchantRangeStatus = 'invalid';
    }

    return merchantRangeStatus;
}


function ValidateAssignmentFilters() {
    var result = true;
    //var merchantRangeStatus = ValidateMerchantRange();
    //if (merchantRangeStatus == 'invalid') {
    //    result = false;
    //}
    var approvalDate = approvalDateChecked();
    var isWatchStatusNA = watchStatusNAIsChecked();
    var selectedHierarchy = parseInt(document.getElementById(Risk_Assignment_Filters_hdnCountSelectedFilter).value);
    var allMerchants = document.getElementById(Risk_Assignment_Filters_chkAllMerchants);
    var transactionalFilter = ValidateTransactionalFilter();
    if (transactionalFilter == 'invalid') {

        result = false;
    }

    if (allMerchants != null) {
        if (selectedHierarchy == 0
        && isWatchStatusNA 
        && approvalDate == false
        && allMerchants.checked == false
        && transactionalFilter == 'none'
        ) {

            alert(Risk_Assignment_Filters_Assignment_Filter_Required);
            return false;

        }
    } else {
        if (selectedHierarchy == 0
        && isWatchStatusNA
        && approvalDate == false
        && transactionalFilter == 'none'
        ) {

            alert(Risk_Assignment_Filters_Assignment_Filter_Required);
            return false;

        }
    }

    return result;
}



function MerchantCountConfirm(NumberOfMerchant) {
    if (parent.getSaveConfirm() == true) {
        var message = Risk_Assignment_Filters_Resource_Message;
        message = message.replace('_NumberOfMerchant_', NumberOfMerchant);
        var result = window.confirm(message);
        if (!result) {
            parent.setSaveConfirm(false);
            document.getElementById(Risk_Assignment_Parameters_btnRefreshParamList).click();
        } else {
            parent.saveAssignment();
        }
    }
}



function MerchantRange_OnKeyPress(e) {
    var evt = window.event ? window.event : e;
    var code = evt.keyCode ? evt.keyCode : e.which;
    if ((code >= 48 && code <= 57) ||
        code == 37 || code == 39 || code == 46 || code == 8 || code == 9)  //left arrow, righ arrow, del, backspace, tab
        return true;
    else {
        e.preventDefault ? e.preventDefault() : e.returnValue = false;
        return false;
    }
}

function CalculateMerchantCount() {
    if (!ValidateAssignmentGeneralInformationByValidator()) {
        return false;
    }
    var result = ValidateAssignmentFilters();
    if (result == true) {
        btnCountClick();
    }
    return result;
}

function btnCountClick() {
    document.getElementById(Risk_Assignment_Filters_btnCalculateMerchantCount).click();
}

function visibleCreditScoreCb(e, cbCreditScore) {
    var cbObjID = $("div[id$=" + cbCreditScore + "]").attr("id");
    var radObj = $find(cbObjID);
    if (e.checked) {
        radObj.set_visible(true);
        radObj.enable();
    }
    else {
        radObj.disable();
    }
}