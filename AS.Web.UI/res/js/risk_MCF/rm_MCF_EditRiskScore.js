

var indicatorNegative = 1;
var thresholdNegative = 1;

var vObjThresholdHigh = $get(EditRiskScore_uxParameterThresholdHigh);
var vObjThreshold = $get(EditRiskScore_uxParameterThreshold);
var const_ValidCharsIndicator = '0123456789.';
var const_ValidCharsThreshold = '0123456789';
const rmMCFScoreTypeToEdit = 'To';
const rmMCFScoreTypeFromEdit = 'From';
const editParameterIndicatorFromMsg = $('#uxParameterIndicatorFromEditPrecisionMsg');
const editParameterIndicatorToMsg = $('#uxParameterIndicatorToEditPrecisionMsg');

window.onload = function () {
    var vObjIsIndicatorNegative = $get(EditRiskScore_uxParameterIsIndicatorNegative);
    var parameterPrecision = $get(EditRiskScore_hddParameterPrecision);

    if (vObjIsIndicatorNegative != null && vObjIsIndicatorNegative.value == "1") {
        indicatorNegative = -1;
    }
    else {
        indicatorNegative = parentIsIndicatorNegative ? -1 : 1;
    }
    var vObjIsThresholdNegative = $get(EditRiskScore_uxParameterIsThresholdNegative);


    if (vObjIsThresholdNegative != null && vObjIsThresholdNegative.value == "1") {
        thresholdNegative = -1;
    }
    else {
        thresholdNegative = parentIsThresholdNegative ? -1 : 1;
    }

    //[42397] - Fixbug 37367
    if (parameterPrecision.value == 1) {
        const_ValidCharsIndicator = '0123456789.';
    }
}


function GetPrecision() {
    return $("#" + EditRiskScore_uxParameterPrecision).val().trim().length > 0 ? $("#" + EditRiskScore_uxParameterPrecision).val().trim() : 0;
}

function CheckValidFromPrecisionForEdit() {
    if (paramIndicatorFromValidForEdit()) {
        var precision = GetPrecision();
        if (precision > 0) {
            var regexPrecision = new RegExp("^\\d+(\\.\\d{0," + precision + "})?$");
            var vObjFrom = $get(EditRiskScore_uxParameterIndicatorFrom);
            if (!regexPrecision.test(vObjFrom.value)) {
                var precision_2 = parseInt(precision) + 1;
                $('#uxParameterIndicatorFromEditPrecisionMsg').text(EditRiskScore_uxParameterIndicatorFromPrecisionMsg.replace("{0}", precision_2));
                $('#uxParameterIndicatorFromEditPrecisionMsg').show();
                return false;
            }
            else {
                editParameterIndicatorFromMsg.text('');
                hideErrorMessage(editParameterIndicatorFromMsg);
                return true;
            }
        }
        else {
            editParameterIndicatorFromMsg.text('');
            hideErrorMessage(editParameterIndicatorFromMsg);
            return true;
        }
    }
    return true;
}


function CheckValidToPrecisionForEdit() {
    if (paramIndicatorToValidForEdit()) {
        var precision = GetPrecision();
        if (precision > 0) {
            var regexPrecision = new RegExp("^\\d+(\\.\\d{0," + precision + "})?$");
            var vObjTo = $get(EditRiskScore_uxParameterIndicatorTo);
            if (!regexPrecision.test(vObjTo.value)) {
                var precision_2 = parseInt(precision) + 1;
                $('#uxParameterIndicatorToEditPrecisionMsg').text(EditRiskScore_uxParameterIndicatorToPrecisionMsg.replace("{0}", precision_2));
                $('#uxParameterIndicatorToEditPrecisionMsg').show();
                return false;
            }
            else {
                editParameterIndicatorToMsg.text('');
                hideErrorMessage(editParameterIndicatorToMsg);
                return true;
            }
        }
        else {
            editParameterIndicatorToMsg.text('');
            hideErrorMessage(editParameterIndicatorToMsg);
            return true;
        }
    }
    return true;
}


function checkNotNullIndicatorForEdit() {
    var itemValue = $get(EditRiskScore_uxParameterIsNullIndicator).value;
    if (itemValue != null && itemValue == "1") {
        return true;
    }
    return false;
}



function paramIndicatorFromRequiredForEdit() {
    if (checkNotNullIndicatorForEdit()) {
        var vObjFrom = $get(EditRiskScore_uxParameterIndicatorFrom);
        if (trim(vObjFrom.value) == '') {
            let fromMsg = editParameterIndicatorFromMsg;
            fromMsg.text(RiskScore_uxParameterIndicatorFromEmptyMsg);
            fromMsg.show();

            focusTextbox(vObjFrom);
            return false;
        } else {
            editParameterIndicatorFromMsg.text('');
            hideErrorMessage(editParameterIndicatorFromMsg);
            return true;
        }
    }
    return true;
}

function paramIndicatorFromValidForEdit() {
    hideErrorMessage(editParameterIndicatorFromMsg);

    if (!paramIndicatorFromRequiredForEdit() || !CheckIndicatorFromGreaterZeroForEdit())
        return false;

    if (checkNotNullIndicatorForEdit()) {
        var vObjFrom = $get(EditRiskScore_uxParameterIndicatorFrom);
        if (!hasOnlyCharacters(vObjFrom.value, const_ValidCharsIndicator) || isNaN(vObjFrom.value)) {
            focusTextbox(vObjFrom);
            return false;
        }
        return true;
    }
    return true;
}

function paramIndicatorToRequiredForEdit() {
    hideErrorMessage(editParameterIndicatorToMsg);

    if (checkNotNullIndicatorForEdit()) {
        var vObjTo = $get(EditRiskScore_uxParameterIndicatorTo);
        if (trim(vObjTo.value) == '') {
            let toMsg = editParameterIndicatorToMsg;
            toMsg.text(RiskScore_uxParameterIndicatorToEmptyMsg);
            toMsg.show();

            focusTextbox(vObjTo);
            return false;
        } else {
            editParameterIndicatorToMsg.text('');
            hideErrorMessage(editParameterIndicatorToMsg);
            return true;
        }
    }
    return true;
}

function paramIndicatorToValidForEdit() {
    if (!paramIndicatorToRequiredForEdit() || !CheckIndicatorToGreaterZeroForEdit())
        return false;

    if (checkNotNullIndicatorForEdit()) {
        var vObjTo = $get(EditRiskScore_uxParameterIndicatorTo);
        if (!hasOnlyCharacters(vObjTo.value, const_ValidCharsIndicator) || isNaN(vObjTo.value)) {
            focusTextbox(vObjTo);
            return false;
        }
        return true;
    }
    return true;
}

function CheckIndicatorFromGreaterZeroForEdit() {
    hideErrorMessage(editParameterIndicatorFromMsg);

    let selectedValue = document.getElementById(EditRiskScore_uxIsAllowDecimalValue).value.trim();
    if (Boolean(selectedValue)) {
        if (checkNotNullIndicatorForEdit()) {
            let vObjFrom = $get(EditRiskScore_uxParameterIndicatorFrom);

            if (!isNaN(vObjFrom.value) && parseFloat(vObjFrom.value) === 0) {
                editParameterIndicatorFromMsg.text(getTextResouceGreaterZeroByEdit(rmMCFScoreTypeFromEdit));
                editParameterIndicatorFromMsg.show();
                focusTextbox(vObjFrom);
                return false;
            }
            else {
                editParameterIndicatorFromMsg.text('');
                hideErrorMessage(editParameterIndicatorFromMsg);
                return true;
            }
        }
    }
    return true;
}

function CheckIndicatorToGreaterZeroForEdit() {
    hideErrorMessage(editParameterIndicatorToMsg);

    let selectedValue = document.getElementById(EditRiskScore_uxIsAllowDecimalValue).value.trim();
    if (Boolean(selectedValue)) {
        if (checkNotNullIndicatorForEdit()) {
            let vObjTo = $get(EditRiskScore_uxParameterIndicatorTo);

            if (!isNaN(vObjTo.value) && parseFloat(vObjTo.value) === 0) {
                editParameterIndicatorToMsg.text(getTextResouceGreaterZeroByEdit(rmMCFScoreTypeToEdit));
                editParameterIndicatorToMsg.show();
                focusTextbox(vObjTo);
                return false;
            }
            else {
                editParameterIndicatorToMsg.text('');
                hideErrorMessage(editParameterIndicatorToMsg);
                return true;
            }
        }
    }
    return true;
}

function getTextResouceGreaterZeroByEdit(type) {
    const precision = GetPrecision() ?? "0";
    const precisionInt = parseInt(precision);
    const precisionStr = '0.'.padEnd(precisionInt + 1, '0') + '1';
    switch (type) {
        case rmMCFScoreTypeFrom:
            return editRiskScore_uxParameterIndicatorFromGreaterMinByPrecisionMsg.replace("{0}", precisionStr);
        case rmMCFScoreTypeTo:
            return editRiskScore_uxParameterIndicatorToGreaterMinByPrecisionMsg.replace("{0}", precisionStr);
        default:
            return '';
    }
}
function compareFromToForEdit() {
    if (checkNotNullIndicatorForEdit()) {
        var vObjFrom = $get(EditRiskScore_uxParameterIndicatorFrom);
        var vObjTo = $get(EditRiskScore_uxParameterIndicatorTo);
        if (vObjTo.value * indicatorNegative <= vObjFrom.value * indicatorNegative) {
            $('label[for=' + EditRiskScore_uxParameterIndicatorFrom + ']').addClass('label-error');
            focusTextbox(vObjTo);
            return false;
        }
        else {
            return true;
        }
    }
    return true;
}

function paramThresholdValidForEdit() {
    if (trim(vObjThreshold.value) != '' && !hasOnlyCharacters(vObjThreshold.value, const_ValidCharsThreshold)) {
        if (trim(vObjThreshold.value) != 'N/A') {
            focusTextbox(vObjThreshold);
            return false;
        }
    }
    return true;
}

function paramThresholdHighValidForEdit() {
    var isHighThreadhold = document.getElementById(EditRiskScore_uxHasHighThreadhold);
    if (vObjThresholdHigh != null && isHighThreadhold.value == "1") {
        if (trim(vObjThresholdHigh.value) != '' && !hasOnlyCharacters(vObjThresholdHigh.value, const_ValidCharsThreshold)) {
            if (vObjThresholdHigh.value != 'N/A') {
                return false;
            }
        }
    }
    return true;
}

function paramThresholdLowRequiredForEdit() {
    var isHighThreadhold = document.getElementById(EditRiskScore_uxHasHighThreadhold);
    if (vObjThresholdHigh != null && isHighThreadhold.value == "1") {
        if (vObjThreshold.value == '' && vObjThresholdHigh.value != '') {
            return false;
        }
    }
    return true;
}

function paramThresholdHighRequiredForEdit() {
    var isHighThreadhold = document.getElementById(EditRiskScore_uxHasHighThreadhold);
    if (vObjThresholdHigh != null && isHighThreadhold.value == "1") {
        if (trim(vObjThreshold.value) != '' && vObjThresholdHigh.value == '') {
            return false;
        }
    }
    return true;
}

function paramThresholdLowGreaterThanZero_ForEdit() {
    var isHighThreadhold = document.getElementById(EditRiskScore_uxHasHighThreadhold);
    if (vObjThresholdHigh != null && isHighThreadhold.value == "1") {
        if (trim(vObjThreshold.value) * 1 == 0 && trim(vObjThresholdHigh.value) != '') {
            return false;
        }
    }
    return true;
}

function paramThresholdHigh_gt_Low_ForEdit() {
    var isHighThreadhold = document.getElementById(EditRiskScore_uxHasHighThreadhold);
    if (vObjThresholdHigh != null && isHighThreadhold.value == "1" && (vObjThreshold.value != - '' && vObjThresholdHigh.value != '')) {
        if (vObjThreshold.value * 1 > vObjThresholdHigh.value * 1) {
            return false;
        }
    }
    return true;
}

function paramIndicatorScoreRequiredForEdit() {
    var vObjScore = $get(EditRiskScore_uxParameterScore);
    if (trim(vObjScore.value) == '') {
        focusTextbox(vObjScore);
        return false;
    }
    return true;
}

function paramIndicatorScoreValidForEdit() {
    var vObjScore = $get(EditRiskScore_uxParameterScore);
    if (trim(vObjScore.value) != '' && !hasOnlyCharacters(vObjScore.value, const_ValidCharsIndicator)) {
        focusTextbox(vObjScore);
        return false;
    }
    return true;
}

function isInterger(n) {
    return n >>> 0 === parseFloat(n);
}


function ValidateIntergerForEdit() {
    var vObjScore = $get(EditRiskScore_uxParameterScore);

    return isInterger(vObjScore.value);
}


function ValidateExistingForEdit() {
    var result = true;

    if (compareFromToForEdit() && paramIndicatorToValidForEdit() && paramIndicatorFromValidForEdit()
        && CheckValidFromPrecisionForEdit() && CheckValidToPrecisionForEdit()
    ) {
        result = false;
        var commandID = EditRiskScore_uxSubmit;
        var vObRecordID = document.getElementById(EditRiskScore_uxRecordID);
        var vObjParam = document.getElementById(EditRiskScore_uxParameterKey);
        var vObjFrom = $get(EditRiskScore_uxParameterIndicatorFrom);
        var vObjTo = $get(EditRiskScore_uxParameterIndicatorTo);

        var vObjNegative = $get(EditRiskScore_uxParameterIndicatorNegative);


        var fromValue = vObjFrom.value;
        var toValue = vObjTo.value;

        if (vObjNegative.value == 1) {
            fromValue = -fromValue;
            toValue = -toValue;
        }

        var actionUrl = rootURL + "risk_MCF/rm_MCF_RiskScores.aspx/ValidateExisting";
        $.ajax({
            type: "POST",
            async: false,
            url: actionUrl,
            data: '{"commandID":"' + commandID + '","recordID":"' + vObRecordID.value
                + '","parameterKey":"' + vObjParam.value + '","from":"' + fromValue + '","to":"' + toValue + '","riskSite":"' + rm_RiskScores_uxHdRiskSite.value + '"}',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                if (msg.d[0] == 'true') {
                    result = true;
                    //__doPostBack(msg.d[1], '');
                }
            }
        });
    }
    return result;
}

function clearError() {
    $('#uxParameterIndicatorFromEditPrecisionMsg').text('');
    $('#uxParameterIndicatorFromEditPrecisionMsg').addClass('hide');
    $('#uxParameterIndicatorToEditPrecisionMsg').text('');
    $('#uxParameterIndicatorToEditPrecisionMsg').addClass('hide');
}

function hideErrorMessage(element) {
    if (element !== null && !element.text()) {
        element.hide();
    }
    return;
}
