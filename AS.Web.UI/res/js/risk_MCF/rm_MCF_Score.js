var uxSubmit = document.getElementById("<%=uxUpdate.ClientID%>");
const rmMCFScoreTypeTo = 'To';
const rmMCFScoreTypeFrom = 'From';
const parameterIndicatorFromMsg = $('#uxParameterIndicatorFromPrecisionMsg');
const parameterIndicatorToMsg = $('#uxParameterIndicatorToPrecisionMsg');

window.onload = function () {
    $("#uxLabThresholdHigh").hide();
    $('#risk-form-table').css("width", "680px");
    $("#uxLow").hide();

    let vObj = $find(riskScore_uxParamList);
    let selectedItem = vObj.get_selectedItem();
    let selectedValue = selectedItem?.get_value();
    let parameterPrecision = selectedItem.get_attributes().getAttribute('ParameterPrecision');
    let parameterDataType = selectedItem.get_attributes().getAttribute("ParameterDataType") ?? "";

    setParamIndicatorRule(selectedValue, parameterPrecision, parameterDataType);
}

function checkDecimalInputCharacterScore(e, precisionData) {
    let val = e.value;

    const precision = precisionData ?? e.getAttribute('precision') ?? 4;
    const strRegex = `^\\d{1,9}(\\.\\d{0,${precision}})?$`;
    const regex = new RegExp(strRegex);
    if (isNaN(val) || !regex.test(val))
        e.value = "";
}

function checkInputCharacterScore(e) {
    var val = e.value;
    if (isNaN(val))
        e.value = "";
}

function InputOnTextBox(e) {
    var isIE = /MSIE/.test(navigator.userAgent);
    var keyCode = isIE ? e.keyCode : e.which;

    if (keyCode > 31 && (keyCode < 48 || keyCode > 57)) {
        e.preventDefault();
        return false;
    }

    return DefaultEnterOnTextBox(e);
}

function InputAllowDecimalTextBox(e) {
    var isIE = /MSIE/.test(navigator.userAgent);
    var keyCode = isIE ? e.keyCode : e.which;

    if (keyCode > 31 && keyCode !== 46 && (keyCode < 48 || keyCode > 57)) {
        e.preventDefault();
        return false;
    }

    const currentValue = e.target.value;
    const cursorStart = e.target.selectionStart;
    const cursorEnd = e.target.selectionEnd;

    const nextValue =
        currentValue.slice(0, cursorStart) +
        e.key +
        currentValue.slice(cursorEnd);

    const precision = e.target.attributes.precision.value ?? 4;
    const strRegex = `^\\d{1,9}(\\.\\d{0,${precision}})?$`;
    const regex = new RegExp(strRegex);
    if (!regex.test(nextValue)) {
        e.preventDefault();
        return false;
    }

    return DefaultEnterOnTextBox(e);
}

function DefaultEnterOnTextBox(e) {
    var isEnter = false;
    var isIE = /MSIE/.test(navigator.userAgent);
    var keyCode = isIE ? e.keyCode : e.which;
    isEnter = (keyCode == 13);
    if (isEnter) {
        if (isIE)
            setTimeout("uxSubmit.click()", 200);
        else {
            uxSubmit.focus();
            uxSubmit.click();
        }
        return false;
    }
}

function OnInputtedTextBox(sender, e) {
    const regex = /^\d{1,9}(\.\d{0,4})?$/;
    if (!regex.test(sender.get_value())) {
        e.preventDefault();
        return false;
    }
}

function clearText() {
    $find(riskScore_uxParamList).clearSelection();
    document.getElementById(riskScore_uxParameterIndicatorFrom).value = '';
    document.getElementById(riskScore_uxParameterIndicatorTo).value = '';
    document.getElementById(riskScore_uxParameterThreshold).value = '';
    document.getElementById(riskScore_uxParameterThresholdHigh).value = '';
    document.getElementById(riskScore_uxParameterScore).value = '';
    clearError();
}

function clearError() {
    parameterIndicatorFromMsg.text('');
    parameterIndicatorToMsg.text('');
}

var const_ValidCharsIndicator = '0123456789.';
var const_ValidCharsThreshold = '0123456789';

function uxParamList_OnClientSelectedIndexChanged(sender, args) {
    var item = args.get_item();

    var vObj = $find(riskScore_uxParamList);
    if (vObj.get_value() != 0) {
        $('label[for=' + riskScore_uxParamList + '_Input]').removeClass('label-error');
        $('label[for=' + riskScore_uxParamList + '_Input]').parent().find(".error").css("display", "none");

    }
    
    parentIsIndicatorNegative = (item.get_attributes().getAttribute("IsIndicatorNegative") == 1);
    parentIsThresholdNegative = (item.get_attributes().getAttribute("IsThresholdNegative") == 1);
    //Bug #37829: [QAI][44432]
    let parameterPrecision = (item.get_attributes().getAttribute("ParameterPrecision") > 0);
    let parameterDataType = item.get_attributes().getAttribute("ParameterDataType") ?? "";

    var indicatorNegative = 1;
    var thresholdNegative = 1;

    indicatorNegative = parentIsIndicatorNegative ? -1 : 1;
    thresholdNegative = parentIsThresholdNegative ? -1 : 1;

    //[42397] - Fixbug 37367
    if (parameterPrecision) {
        const_ValidCharsIndicator = '0123456789.';
    }

    var parameterThresholdEnabled = (item.get_attributes().getAttribute("ParameterThresholdEnabled") == 1);
    ////
    var parameterThresholdHigh = (item.get_attributes().getAttribute("ParameterThresholdHigh") == 'LowHigh');

    var uxParameterThreshold = document.getElementById(riskScore_uxParameterThreshold);

    let uxParameterIndicatorFrom = document.getElementById(riskScore_uxParameterIndicatorFrom);
    let uxParameterIndicatorTo = document.getElementById(riskScore_uxParameterIndicatorTo);


    parentIsNullParameterIndicator = (item.get_attributes().getAttribute("IsNullParameterIndicator") == 1);
    if (!parentIsNullParameterIndicator) {
        document.getElementById(riskScore_uxParameterIndicatorTo).value = '';
        document.getElementById(riskScore_uxParameterIndicatorFrom).value = '';
        uxParameterIndicatorFrom.disabled = true;
        uxParameterIndicatorTo.disabled = true;
    }
    else {
        uxParameterIndicatorFrom.disabled = false;
        uxParameterIndicatorTo.disabled = false;

    }
    if (parentIsIndicatorNegative) {
        $("#risk-form-table").css("width", "720px");
        $("#" + riskScore_uxParameterIndicatorFrom).addClass('red');
        $("#" + riskScore_uxParameterIndicatorTo).addClass('red');

        $("#uxOpenBracketFrom").removeClass('display-none');
        $("#uxCloseBracketFrom").removeClass('display-none');
        $("#uxOpenBracketTo").removeClass('display-none');
        $("#uxCloseBracketTo").removeClass('display-none');
    }
    else {
        $("#risk-form-table").css("width", "680px");

        $("#" + riskScore_uxParameterIndicatorFrom).removeClass('red');
        $("#" + riskScore_uxParameterIndicatorTo).removeClass('red');
        $("#" + riskScore_uxParameterScore).removeClass('red');

        $("#uxOpenBracketFrom").addClass('display-none');
        $("#uxCloseBracketFrom").addClass('display-none');
        $("#uxOpenBracketTo").addClass('display-none');
        $("#uxCloseBracketTo").addClass('display-none');

    }
    if (parentIsThresholdNegative && parameterThresholdEnabled) {
        $("#" + riskScore_uxParameterThreshold).addClass('red');
        $("#uxOpenBracketThreshold").removeClass('display-none');
        $("#uxCloseBracketThreshold").removeClass('display-none');
    }
    else {
        $("#" + riskScore_uxParameterThreshold).removeClass('red');
        $("#uxOpenBracketThreshold").addClass('display-none');
        $("#uxCloseBracketThreshold").addClass('display-none');
    }
    //ParameterThresholdHigh
    if (parameterThresholdHigh) {
        $("#uxLabThresholdHigh").show();
        $('#risk-form-table').css("width", "900px");
        $("#uxLow").show();
        var uxParameterThresholdHigh = document.getElementById(riskScore_uxParameterThresholdHigh);
        uxParameterThresholdHigh.disabled = !parameterThresholdEnabled;
        //$("#uxLabScore").css("padding-top", "10px");
        //$("#uxLabScore").css("float", "left");
        $("#" + riskScore_uxHasHighThreadhold).val("1");
        if (parentIsThresholdNegative && parameterThresholdEnabled) {
            $("#uxOpenBracketThresholdHigh").removeClass('display-none');
            $("#uxCloseBracketThresholdHigh").removeClass('display-none');
            $("#" + riskScore_uxParameterThresholdHigh).addClass('red');
        }
        else {
            $("#uxOpenBracketThresholdHigh").addClass('display-none');
            $("#uxCloseBracketThresholdHigh").addClass('display-none');
            $("#" + riskScore_uxParameterThresholdHigh).removeClass('red');
        }
    }
    else {
        $("#uxLabThresholdHigh").hide();
        $('#risk-form-table').css("width", "680px");
        $("#uxLow").hide();

        //$("#uxLabScore").css("padding-top", "0px");
        //$("#uxLabScore").css("float", "right");
        $("#" + riskScore_uxHasHighThreadhold).val("0");
    }
    ////
    uxParameterThreshold.disabled = !parameterThresholdEnabled;
    if (parameterThresholdEnabled) {
        $(uxParameterThreshold).removeClass("aspNetDisabled");
    }
    if (!parameterThresholdEnabled) uxParameterThreshold.value = "";

    let selectedValue = item.get_value().trim();
    let parameterPrecisionValue = item.get_attributes().getAttribute('ParameterPrecision');
    setParamIndicatorRule(selectedValue, parameterPrecisionValue, parameterDataType);
}


//var uxParameterIndicatorFrom = document.getElementById(riskScore_uxParameterIndicatorFrom);
//var uxParameterIndicatorTo = document.getElementById(riskScore_uxParameterIndicatorTo);
//var uxParameterThreshold = document.getElementById(riskScore_uxParameterThreshold);
//var uxParameterThresholdHigh = document.getElementById(riskScore_uxParameterThresholdHigh);

//var uxParameterScore = document.getElementById(riskScore_uxParameterScore);
//var uxSubmit = document.getElementById(riskScore_uxUpdate);

var isParameterSelected = false;
var vObjThresholdHigh = $get(riskScore_uxParameterThresholdHigh);
var vObjThreshold = $get(riskScore_uxParameterThreshold);

function GetPrecision() {
    var vObj = $find(riskScore_uxParamList);
    var item = vObj.get_selectedItem();
    if (item != null) {
        return item.get_attributes().getAttribute("ParameterPrecision");
    }
    else {
        return 0;
    }
}


function CheckValidFromPrecisionForCreate() {
    if (paramIndicatorFromValidForCreate()) {
        var precision = GetPrecision();
        if (precision > 0) {
            var regexPrecision = new RegExp("^\\d+(\\.\\d{0," + precision + "})?$");
            var vObjFrom = $get(riskScore_uxParameterIndicatorFrom);
            var precision_2 = parseInt(precision) + 1;

            if (!regexPrecision.test(vObjFrom.value)) {
                parameterIndicatorFromMsg.text(RiskScore_uxParameterIndicatorFromPrecisionMsg.replace("{0}", precision_2));
                parameterIndicatorFromMsg.show();
                return false;
            }
            else {
                parameterIndicatorFromMsg.text('');
                hideErrorMessage(parameterIndicatorFromMsg);
                return true;
            }
        }
    }
    return true;
}


function CheckValidToPrecisionForCreate() {
    if (paramIndicatorToValidForCreate()) {
        var precision = GetPrecision();
        if (precision > 0) {
            var regexPrecision = new RegExp("^\\d+(\\.\\d{0," + precision + "})?$");
            var vObjTo = $get(riskScore_uxParameterIndicatorTo);
            var precision_2 = parseInt(precision) + 1;
            if (!regexPrecision.test(vObjTo.value)) {
                $(parameterIndicatorToMsg).text(RiskScore_uxParameterIndicatorToPrecisionMsg.replace("{0}", precision_2));
                $(parameterIndicatorToMsg).show();

                return false;
            }
            else {
                $(parameterIndicatorToMsg).text('');
                $(parameterIndicatorToMsg).hide();
                return true;
            }
        }
        else {
            return true;
        }
    }
    return true;
}

function CheckComboBox() {
    var vObj = $find(riskScore_uxParamList);
    if (vObj.get_value() == 0) {
        setTimeout("ShowParameterDropDown()", 500);
        isParameterSelected = false;
        return false;
    }
    isParameterSelected = true;
    return true;
}

function ShowParameterDropDown() {
    var vObj = $find(riskScore_uxParamList);
    vObj.showDropDown();
}


function checkNotNullIndicatorForCreate() {
    var itemValue;
    var vObjIsNullIndicator = $find(riskScore_uxParamList);
    var item = vObjIsNullIndicator.get_items().getItem(vObjIsNullIndicator.get_selectedIndex());
    itemValue = item.get_attributes().getAttribute("IsNullParameterIndicator");
    if (itemValue != null && itemValue == "1") {
        return true;
    }
    return false;
}



function paramIndicatorFromRequiredForCreate() {
    if (checkNotNullIndicatorForCreate()) {
        var vObjFrom = $get(riskScore_uxParameterIndicatorFrom);
        if (trim(vObjFrom.value) == '') {
            let fromMsg = parameterIndicatorFromMsg;
            fromMsg.text(RiskScore_uxParameterIndicatorFromEmptyMsg);
            fromMsg.show();

            focusTextbox(vObjFrom);
            return false;
        }
        else {
            parameterIndicatorFromMsg.text('');
            hideErrorMessage(parameterIndicatorFromMsg);
        }
    }
    return true;
}

function paramIndicatorFromValidForCreate() {
    hideErrorMessage(parameterIndicatorFromMsg);
    if (!paramIndicatorFromRequiredForCreate() || !CheckIndicatorFromGreaterZero())
        return false;

    if (checkNotNullIndicatorForCreate()) {
        var vObjFrom = $get(riskScore_uxParameterIndicatorFrom);

        if (!hasOnlyCharacters(vObjFrom.value, const_ValidCharsIndicator) || isNaN(vObjFrom.value)) {
            focusTextbox(vObjFrom);
            return false;
        }
        else {
            return true;
        }
    }
    else {
        return true;
    }
}

function CheckIndicatorFromGreaterZero() {
    hideErrorMessage(parameterIndicatorFromMsg);
    let selectedValue = $find(riskScore_uxParamList).get_value().trim();
    if (riskParameterKeysAllowDecimal.indexOf(selectedValue) !== -1) {
        if (checkNotNullIndicatorForCreate()) {
            let vObjFrom = $get(riskScore_uxParameterIndicatorFrom);

            if (!isNaN(vObjFrom.value) && parseFloat(vObjFrom.value) === 0) {
                let fromMsg = parameterIndicatorFromMsg;
                fromMsg.text(getTextResouceGreaterZero(rmMCFScoreTypeFrom));
                fromMsg.show();
                focusTextbox(vObjFrom);
                return false;
            }
            else {
                parameterIndicatorFromMsg.text('');
                hideErrorMessage(parameterIndicatorFromMsg);
            }
        }
    }
    return true;
}

function CheckIndicatorToGreaterZero() {
    let toMsg = $(parameterIndicatorToMsg);
    toMsg.hide();
    let selectedValue = $find(riskScore_uxParamList).get_value().trim();
    if (riskParameterKeysAllowDecimal.indexOf(selectedValue) !== -1) {
        if (checkNotNullIndicatorForCreate()) {
            let vObjTo = $get(riskScore_uxParameterIndicatorTo);

            if (!isNaN(vObjTo.value) && parseFloat(vObjTo.value) === 0) {
                toMsg.text(getTextResouceGreaterZero(rmMCFScoreTypeTo));
                toMsg.show();
                focusTextbox(vObjTo);
                return false;
            }
            else {
                $(parameterIndicatorToMsg).text('');
                hideErrorMessage(parameterIndicatorToMsg);
            }
        }
    }
    return true;
}

function getTextResouceGreaterZero(type) {
    const precision = GetPrecision() ?? "0";
    const precisionInt = parseInt(precision);
    const precisionStr = '0.'.padEnd(precisionInt + 1, '0') + '1';
    switch (type) {
        case rmMCFScoreTypeFrom:
            return riskScore_ascx_IndicatorFromNumberGreaterMinByPrecisionMsg.replace("{0}", precisionStr);
        case rmMCFScoreTypeTo:
            return riskScore_ascx_IndicatorToNumberGreaterMinByPrecisionMsg.replace("{0}", precisionStr);
        default:
            return '';
    }
}
function paramIndicatorToRequiredForCreate() {
    if (checkNotNullIndicatorForCreate()) {
        var vObjTo = $get(riskScore_uxParameterIndicatorTo);
        if (trim(vObjTo.value) == '') {
            let toMsg = $(parameterIndicatorToMsg);
            toMsg.text(RiskScore_uxParameterIndicatorToEmptyMsg);
            toMsg.show();

            focusTextbox(vObjTo);
            return false;
        }

        $(parameterIndicatorToMsg).text('');
        hideErrorMessage(parameterIndicatorToMsg);
        return true;
    }
    else {
        return true;
    }

}

function paramIndicatorToValidForCreate() {
    $(parameterIndicatorToMsg).hide();
    if (checkNotNullIndicatorForCreate()) {
        var vObjTo = $get(riskScore_uxParameterIndicatorTo);
        if (!hasOnlyCharacters(vObjTo.value, const_ValidCharsIndicator) || isNaN(vObjTo.value)) {
            focusTextbox(vObjTo);
            return false;
        }
        return true;
    }
    return true;
}

function compareFromToForCreate() {
    if (checkNotNullIndicatorForCreate()) {
        var indicatorNegative = 1;
        indicatorNegative = parentIsIndicatorNegative ? -1 : 1;
        var vObjFrom = $get(riskScore_uxParameterIndicatorFrom);
        var vObjTo = $get(riskScore_uxParameterIndicatorTo);
        if (vObjTo.value * indicatorNegative <= vObjFrom.value * indicatorNegative) {
            $('label[for=' + riskScore_uxParameterIndicatorFrom + ']').addClass('label-error');
            focusTextbox(vObjTo);
            return false;
        }
        else {
            return true;
        }
    }
    else {
        return true;
    }
}

function paramThresholdValidForCreate() {
    if (trim(vObjThreshold.value) != '' && !hasOnlyCharacters(vObjThreshold.value, const_ValidCharsThreshold)) {
        if (trim(vObjThreshold.value) != RiskScore_js_na) {
            focusTextbox(vObjThreshold);
            return false;
        }
    }
    return true;
}

function paramThresholdHighValidForCreate() {
    var isHighThreadhold = document.getElementById(riskScore_uxHasHighThreadhold);
    if (vObjThresholdHigh != null && isHighThreadhold.value == "1") {
        if (trim(vObjThresholdHigh.value) != '' && !hasOnlyCharacters(vObjThresholdHigh.value, const_ValidCharsThreshold)) {
            if (vObjThresholdHigh.value != RiskScore_js_na) {
                return false;
            }
        }
    }
    return true;
}

function paramThresholdLowRequiredForCreate() {
    var isHighThreadhold = document.getElementById(riskScore_uxHasHighThreadhold);
    if (vObjThresholdHigh != null && isHighThreadhold.value == "1") {
        if (vObjThreshold.value == '' && vObjThresholdHigh.value != '') {
            return false;
        }
    }
    return true;
}

function paramThresholdHighRequiredForCreate() {
    var isHighThreadhold = document.getElementById(riskScore_uxHasHighThreadhold);
    if (vObjThresholdHigh != null && isHighThreadhold.value == "1") {
        if (trim(vObjThreshold.value) != '' && vObjThresholdHigh.value == '') {
            return false;
        }
    }
    return true;
}

function paramThresholdLowGreaterThanZero_ForCreate() {
    var isHighThreadhold = document.getElementById(riskScore_uxHasHighThreadhold);
    if (vObjThresholdHigh != null && isHighThreadhold.value == "1") {
        if (trim(vObjThreshold.value) * 1 == 0 && trim(vObjThresholdHigh.value) != '') {
            return false;
        }
    }
    return true;
}

function paramThresholdHigh_gt_Low_ForCreate() {
    var isHighThreadhold = document.getElementById(riskScore_uxHasHighThreadhold);
    if (vObjThresholdHigh != null && isHighThreadhold.value == "1" && (vObjThreshold.value != '' && vObjThresholdHigh.value != '')) {
        if (vObjThreshold.value * 1 > vObjThresholdHigh.value * 1) {
            return false;
        }
    }
    return true;
}



function paramIndicatorScoreRequiredForCreate() {
    var vObjScore = $get(riskScore_uxParameterScore);
    if (trim(vObjScore.value) == '') {
        //focusTextbox(vObjScore);
        return false;
    }
    return true;
}

function paramIndicatorScoreValidForCreate() {
    var vObjScore = $get(riskScore_uxParameterScore);
    if (trim(vObjScore.value) != '' && !hasOnlyCharacters(vObjScore.value, const_ValidCharsIndicator)) {
        focusTextbox(vObjScore);
        return false;
    }
    return true;
}

function isInterger(n) {
    return n >>> 0 === parseFloat(n);
}


function ValidateIntergerForCreate() {
    var vObjScore = $get(riskScore_uxParameterScore);

    return isInterger(vObjScore.value);
}

function ValidateExistingForCreate() {
    var result = true;

    if (compareFromToForCreate() &&
        paramIndicatorToValidForCreate() &&
        paramIndicatorFromValidForCreate() &&
        CheckValidFromPrecisionForCreate() && CheckValidToPrecisionForCreate()
    ) {
        result = false;
        var commandID = riskScore_uxUpdate;
        var recordID = 0;
        var vObj = $find(riskScore_uxParamList);
        var parameterKey = vObj.get_value();
        var vObjFrom = $get(riskScore_uxParameterIndicatorFrom);
        var vObjTo = $get(riskScore_uxParameterIndicatorTo);

        var fromValue = vObjFrom.value;
        var toValue = vObjTo.value;

        var negative = GetAttributeValue("IsIndicatorNegative");
        if (negative == 1) {
            fromValue = -fromValue;
            toValue = -toValue;
        }

        var actionUrl = rootURL + "risk_MCF/rm_MCF_RiskScores.aspx/ValidateExisting";
        $.ajax({
            type: "POST",
            async: false,
            url: actionUrl,
            data: '{"commandID":"' + commandID + '","recordID":"' + recordID
                + '","parameterKey":"' + parameterKey + '","from":"' + fromValue + '","to":"' + toValue + '","riskSite":"' + rm_RiskScores_uxHdRiskSite.value + '"}',
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

function GetAttributeValue(name) {
    var vObj = $find(riskScore_uxParamList);
    var item = vObj.get_selectedItem();
    if (item != null) {
        return item.get_attributes().getAttribute(name);
    }
    else {
        return 0;
    }
}

function clearErrorMsg() {
    $('.bottom-error').find('label').addClass('hide');
}

function hideErrorMessage(element) {
    if (element !== null && !element.text()) {
        element.hide();
    }
    return;
}

function setParamIndicatorRule(parameterSelectedValue, parameterPrecision, parameterDataType) {
    let uxParameterIndicatorFrom = document.getElementById(riskScore_uxParameterIndicatorFrom);
    let uxParameterIndicatorTo = document.getElementById(riskScore_uxParameterIndicatorTo);

    if (riskParameterKeysAllowDecimal.indexOf(parameterSelectedValue) !== -1) {
        if (parameterPrecision > 0) {
            const_ValidCharsIndicator = '0123456789.';
        }

        $('#uxDecimalNoteToDataType').text(riskScore_ascx_uxDecimalNoteToDataTypeMsg.replaceAll("{0}", parameterDataType));
        $("#uxDecimalNote").removeClass('display-none');
        uxParameterIndicatorFrom.setAttribute("Precision", parameterPrecision);
        uxParameterIndicatorTo.setAttribute("Precision", parameterPrecision);
        uxParameterIndicatorFrom.removeEventListener("keypress", InputOnTextBox);
        uxParameterIndicatorTo.removeEventListener("keypress", InputOnTextBox);
        uxParameterIndicatorFrom.addEventListener("keypress", InputAllowDecimalTextBox);
        uxParameterIndicatorTo.addEventListener("keypress", InputAllowDecimalTextBox);

        uxParameterIndicatorFrom.removeEventListener("blur", function () { checkInputCharacterScore(uxParameterIndicatorFrom) });
        uxParameterIndicatorTo.removeEventListener("blur", function () { checkInputCharacterScore(uxParameterIndicatorFrom) });
        uxParameterIndicatorFrom.addEventListener("blur", function () { checkDecimalInputCharacterScore(uxParameterIndicatorFrom, parameterPrecision) });
        uxParameterIndicatorTo.addEventListener("blur", function () { checkDecimalInputCharacterScore(uxParameterIndicatorTo, parameterPrecision) });
    } else {
        $("#uxDecimalNote").addClass('display-none');
        uxParameterIndicatorFrom.removeEventListener("keypress", InputAllowDecimalTextBox);
        uxParameterIndicatorTo.removeEventListener("keypress", InputAllowDecimalTextBox);
        uxParameterIndicatorFrom.addEventListener("keypress", InputOnTextBox);
        uxParameterIndicatorTo.addEventListener("keypress", InputOnTextBox);

        uxParameterIndicatorFrom.removeEventListener("blur", function () { checkDecimalInputCharacterScore(uxParameterIndicatorFrom, 0) });
        uxParameterIndicatorTo.removeEventListener("blur", function () { checkDecimalInputCharacterScore(uxParameterIndicatorFrom, 0) });
        uxParameterIndicatorFrom.addEventListener("blur", function () { checkInputCharacterScore(uxParameterIndicatorFrom) });
        uxParameterIndicatorTo.addEventListener("blur", function () { checkInputCharacterScore(uxParameterIndicatorTo) });
    }
}
