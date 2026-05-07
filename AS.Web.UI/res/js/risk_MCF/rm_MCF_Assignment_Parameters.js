
function chbRiskScore_click() {
    if ($get(Risk_Assignment_Parameters_chkRiskScore).checked == false) {
        $find(Risk_Assignment_Parameters_txtRCFrom).clear();
        $find(Risk_Assignment_Parameters_txtRCTo).clear();

        $find(Risk_Assignment_Parameters_txtRCFrom).disable();
        $find(Risk_Assignment_Parameters_txtRCTo).disable();
    }
    else {
        $find(Risk_Assignment_Parameters_txtRCFrom).enable();
        $find(Risk_Assignment_Parameters_txtRCTo).enable();
    }
}


function ValidateRiskScoreFromTo() {
    var isValid = true;
    var chk = $get(Risk_Assignment_Parameters_chkRiskScore);
    if (chk != null && chk.checked) {
        var rcFrom = $find(Risk_Assignment_Parameters_txtRCFrom);
        var rcTo = $find(Risk_Assignment_Parameters_txtRCTo);

        if ((rcFrom.get_textBoxValue().trim() != "" && rcTo.get_textBoxValue().trim() != "")) {
            if (rcFrom.get_value() > rcTo.get_value())
                isValid = false;
        }
    }
    return isValid;
}

function ValidateRiskScoreToRequired() {
    var isValid = true;
    var chk = $get(Risk_Assignment_Parameters_chkRiskScore);
    var rcFrom = $find(Risk_Assignment_Parameters_txtRCFrom);
    var rcTo = $find(Risk_Assignment_Parameters_txtRCTo);
    if (chk != null && chk.checked && rcFrom.get_textBoxValue().trim() != "") {


        if ((rcFrom.get_textBoxValue().trim() != "" && rcTo.get_textBoxValue().trim() != "")) {
            isValid = true;
        }
        else if (rcTo.get_textBoxValue().trim() == "") {
            isValid = false;
            rcTo.focus();
            $('label[for=' + Risk_Assignment_Parameters_txtRCFrom + ']').addClass('label-error');
        }

    }
    return isValid;
}

function ValidateRiskScoreFromRequired() {
    var isValid = true;
    var chk = $get(Risk_Assignment_Parameters_chkRiskScore);
    if (chk != null && chk.checked) {
        var rcFrom = $find(Risk_Assignment_Parameters_txtRCFrom);
        var rcTo = $find(Risk_Assignment_Parameters_txtRCTo);

        if ((rcFrom.get_textBoxValue().trim() != "" && rcTo.get_textBoxValue().trim() != "")) {
            isValid = true;
        }
        else if (rcFrom.get_textBoxValue().trim() == "") {
            isValid = false;
            rcFrom.focus();
        }
    }

    return isValid;
}


function validateRiskScoreCheck() {
    var chk = $get(Risk_Assignment_Parameters_chkRiskScore);
    if (chk != null && chk.checked) {
        return true;
    }
    return false;
}
function ValidateAssignmentParameters() {


    //Validate riskscore
    var itemsRepeater = parseInt(document.getElementById(Risk_Assignment_Parameters_CountItemRepeater).value);
    var validRC = validateRiskScoreValues();
    //var validRC = setTimeout("validateRiskScoreValues();", 300);
    if (!validRC)
        return false;
    var isChecked = validateRiskScoreCheck();
    //var isChecked = setTimeout("validateRiskScoreCheck();", 300);
    if (!isChecked) {
        //Validate parameters
        if (itemsRepeater == 0) {
            alert(Risk_Assignment_Parameters_js_msg1);
            return false;
        }
    }
    return true;
}

function parameter_ShowFilter(url, index, paramkey, width, height) {
    document.getElementById(Risk_Assignment_Parameters_uxCurrentParam).value = index;
    document.getElementById(Risk_Assignment_Parameters_uxPramKey).value = paramkey;
    doOpenSubPopup(url, 'auto');

}
function parameter_Add(modal) {
    document.getElementById(Risk_Assignment_Parameters_btnSaveWorkingParameters).click();
    return doOpenSubPopup(modal + Risk_Assignment_Parameters_QueryString, 'auto');

}

function parameter_closeModalEvent(modalID) {
    switch (modalID) {
        case 'ParameterFilter_TransactionCodeModal':
            document.getElementById(Risk_Assignment_Parameters_btnRefreshAssParam).click();
            break;
    }
}

parent.ReloadParamList = function () {
    document.getElementById(Risk_Assignment_Parameters_btnRefreshParamList).click();
}
function confirmDeleteParameter() {
    if (!ValidateAssignmentMerchantRange()) {
        return false;
    }
    var agree = confirm(Risk_Assignment_Parameters_js_msg2);
    if (!agree) {
        return false;
    }
    return true;
}