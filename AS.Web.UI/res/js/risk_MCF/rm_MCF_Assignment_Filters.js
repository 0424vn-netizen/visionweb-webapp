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
        case 'LeadSourceModal':
            document.getElementById(rm_MCF_Assignment_Filter_Source_Hierarchy_btnRefreshLeadSource).click();
            break;
        case 'ReferralSourceModal':
            document.getElementById(rm_MCF_Assignment_Filter_Source_Hierarchy_btnRefreshReferralSource).click();
            break;
        case 'GverifyCodeModal':
            document.getElementById(Risk_Assignment_Filters_btnRefreshGverifyCode).click();
            break;
        case 'GauthenticateCodeModal':
            document.getElementById(Risk_Assignment_Filters_btnGauthenticateCode).click();
            break;
        case 'G2CompassModal':
            document.getElementById(Risk_Assignment_Filters_btnG2Compass).click();
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
    var approvalDate = approvalDateChecked();
    var fistBatchDate = fistBatchDateChecked();
    var isWatchStatusNA = typeof (watchStatusNAIsChecked) != "undefined" ? watchStatusNAIsChecked(): true;
    var selectedHierarchy = parseInt(document.getElementById(Risk_Assignment_Filters_hdnCountSelectedFilter).value);
    var allMerchants = document.getElementById(Risk_Assignment_Filters_chkAllMerchants);

    var transactionalFilter = ValidateTransactionalFilter();
    var creditScoreFilter = ValidateCreditScore();
    var ecommerceFilter = ValidateThreeNewFilters();
    var sourceHierarchy = ValidateSourceHierarchy();
    var daystoFundFilter = ValidateDaystoFundFilter();
    var daystoFundStandardFilter = ValidateDaystoFundStandardFilter();
    var gCode = ValidateGCode();
    var creditScoreCustomFilter = ValidateCreditScoreCustom();
    var isFutureDeliveryStatusNA = watchFutureDeliveryStatusNAIsChecked();
    var isMerchantClosedStatus = document.getElementById(Risk_Assignment_Filters_MerchantClosedStatus);
    var isValidPauseMerchantAlert = pauseMerchantAlert.validatePauseMerchantAlert();
    var creditScoreStandardFilter = ValidateCreditScoreStandard();
    if (transactionalFilter == 'invalid') {

        result = false;
    }
    if (daystoFundFilter == 'invalid') {

        result = false;
    }
    if (daystoFundStandardFilter == 'invalid') {

        result = false;
    }
    if (!ValidateEcomerceFilter()) {
        result = false;
    }
    
    if (!isValidPauseMerchantAlert) {    
        return false;
    }

    if (allMerchants != null) {
        if (selectedHierarchy == 0
            && isWatchStatusNA
            && approvalDate == false
            && fistBatchDate == false
            && allMerchants.checked == false
            && transactionalFilter == 'none'
            && creditScoreFilter == 'none'
            && ecommerceFilter == 'none'
            && sourceHierarchy == 'none'
            && gCode == 'none'
            && daystoFundFilter == 'none'
            && creditScoreCustomFilter == 'none'
            && isFutureDeliveryStatusNA
            && isMerchantClosedStatus.checked == false
            && creditScoreStandardFilter == 'none'
        ) {

            alert(Risk_Assignment_Filters_Assignment_Filter_Required);
            return false;

        }

    } else {
        if (selectedHierarchy == 0
            && isWatchStatusNA
            && approvalDate == false
            && fistBatchDate == false
            && transactionalFilter == 'none'
            && creditScoreFilter == 'none'
            && ecommerceFilter == 'none'
            && sourceHierarchy == 'none'
            && gCode == 'none'
            && daystoFundFilter == 'none'
            && creditScoreCustomFilter == 'none'
            && isFutureDeliveryStatusNA
            && isMerchantClosedStatus.checked == false
        ) {

            alert(Risk_Assignment_Filters_Assignment_Filter_Required);
            return false;

        }

    }

    return result;
}

function MerchantCountConfirm(NumberOfMerchant, delay) {
    if (parent.getSaveConfirm() == true) {
        var message = Risk_Assignment_Filters_Resource_Message;
        message = message.replace('_NumberOfMerchant_', NumberOfMerchant);

        var delayTime = delay || 0;

        setTimeout(function () {
            var result = window.confirm(message);
            if (!result) {
                parent.setSaveConfirm(false);
                document.getElementById(Risk_Assignment_Parameters_btnRefreshParamList).click();
                if (typeof (loadControl) == 'function') {
                    loadControl();
                    loadControlDaytoFundStandard();
                }
            } else {
                parent.saveAssignment();
            }
        }, delayTime);
    }

    checkEnabledControl();
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
    visibleCb(e, cbCreditScore);
}

function visibleCb(e, cb) {
    var cbObjID = $("div[id$=" + cb + "]").attr("id");
    var radObj = $find(cbObjID);
    if (e.checked) {
        radObj.set_visible(true);
        radObj.enable();
    }
    else {
        radObj.disable();
    }
}

function ValidateCreditScore() {
    var result = 'none';
    var creditScoreIsFrom = document.getElementById(Risk_Assignment_Filters_uxChkIsFrom);
    var creditScoreIsTo = document.getElementById(Risk_Assignment_Filters_uxChkIsTo);
    if ((creditScoreIsFrom != undefined && creditScoreIsFrom.checked) || (creditScoreIsTo != undefined && creditScoreIsTo.checked)) {
        result = 'valid';
    }
    return result;
}

function ValidateGCode() {
    var result = 'none';
    var lblRefreshGverifyCode = $get(Risk_Assignment_Filters_lblRefreshGverifyCode);
    var lblGauthenticateCode = $get(Risk_Assignment_Filters_lblGauthenticateCode);
    var lblG2Compass = $get(Risk_Assignment_Filters_lblG2Compass);
    if ((lblRefreshGverifyCode != undefined && (lblRefreshGverifyCode.textContent.trim() != 'N/A' && lblRefreshGverifyCode.textContent.trim() != "")) ||
        (lblGauthenticateCode != undefined && (lblGauthenticateCode.textContent.trim() != 'N/A' && lblGauthenticateCode.textContent.trim() != "")) ||
        (lblG2Compass != undefined && (lblG2Compass.textContent.trim() != 'N/A' && lblG2Compass.textContent.trim() != "")) ) {
        result = 'valid';
    }
    return result;
}

function checkEnabledControl() {
    if (Risk_Assignment_Filters_uxChkIsFrom != "" && $get(Risk_Assignment_Filters_uxChkIsFrom) != undefined
        && $get(Risk_Assignment_Filters_uxChkIsFrom).checked) {
        visibleCreditScoreCb($get(Risk_Assignment_Filters_uxChkIsFrom), 'uxCbCreditScoreFrom')
    }
    if (Risk_Assignment_Filters_uxChkIsTo != "" && $get(Risk_Assignment_Filters_uxChkIsTo) != undefined
        && $get(Risk_Assignment_Filters_uxChkIsTo).checked) {
        visibleCreditScoreCb($get(Risk_Assignment_Filters_uxChkIsTo), 'uxCbCreditScoreTo')
    }

    if ($get(Risk_FistBatchDate_chkIsNewMerchantOnly).checked) {
        visibleCb($get(Risk_FistBatchDate_chkIsNewMerchantOnly), 'cbNewMerchantOnly1')
    }

    if ($get(Risk_FistBatchDate_chkIsEstablishedMerchants).checked) {
        visibleCb($get(Risk_FistBatchDate_chkIsEstablishedMerchants), 'cbEstablishedMerchants1')
    }

    if ($get(Risk_ApprovalDate_chkIsNewMerchantOnly).checked) {
        visibleCb($get(Risk_ApprovalDate_chkIsNewMerchantOnly), 'cbNewMerchantOnly')
    }

    if ($get(Risk_ApprovalDate_chkIsEstablishedMerchants).checked) {
        visibleCb($get(Risk_ApprovalDate_chkIsEstablishedMerchants), 'cbEstablishedMerchants')
    }

    if (Risk_Assignment_Filters_uxChkIsFromCustom != "" && $get(Risk_Assignment_Filters_uxChkIsFromCustom) != undefined
        && $get(Risk_Assignment_Filters_uxChkIsFromCustom).checked) {
        visibleCreditScoreCb($get(Risk_Assignment_Filters_uxChkIsFromCustom), 'uxCbCreditScoreFromCustom')
    }
    if (Risk_Assignment_Filters_uxChkIsToCustom != "" && $get(Risk_Assignment_Filters_uxChkIsToCustom) != undefined
        && $get(Risk_Assignment_Filters_uxChkIsToCustom).checked) {
        visibleCreditScoreCb($get(Risk_Assignment_Filters_uxChkIsToCustom), 'uxCbCreditScoreToCustom')
    }
}