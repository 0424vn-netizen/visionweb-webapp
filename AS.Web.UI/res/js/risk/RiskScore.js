var uxSubmit = document.getElementById("<%=uxUpdate.ClientID%>");
window.onload = function () {
    $("#uxLabThresholdHigh").hide();
    $('#risk-form-table').css("width", "680px");
    $("#uxLow").hide();
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

function clearText() {
    document.getElementById(riskScore_uxParameterIndicatorFrom).value = '';
    document.getElementById(riskScore_uxParameterIndicatorTo).value = '';
    document.getElementById(riskScore_uxParameterThreshold).value = '';
    document.getElementById(riskScore_uxParameterThresholdHigh).value = '';
    document.getElementById(riskScore_uxParameterScore).value = '';
    clearError();
}

function clearError() {
    $("label").each(function () {
        if ($(this).hasClass("label-error")) {
            $(this).removeClass("label-error");
        }
        if ($(this).hasClass("error")) {
            $(this).text("");
            $(this).css('display', 'none');
        }
    });
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
    var parameterPrecision = (item.get_attributes().getAttribute("ParameterPrecision") > 0);

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

    var uxParameterIndicatorFrom = document.getElementById(riskScore_uxParameterIndicatorFrom);
    var uxParameterIndicatorTo = document.getElementById(riskScore_uxParameterIndicatorTo);


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
    if (!parameterThresholdEnabled) uxParameterThreshold.value = "";
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
    var precision = GetPrecision();
    if (precision > 0) {
        var regexPrecision = new RegExp("^\\d+(\\.\\d{1," + precision + "})?$");
        var vObjFrom = $get(riskScore_uxParameterIndicatorFrom);
        return regexPrecision.test(vObjFrom.value);
    }
    else {
        return true;
    }
}


function CheckValidToPrecisionForCreate() {
    var precision = GetPrecision();
    if (precision > 0) {
        var regexPrecision = new RegExp("^\\d+(\\.\\d{1," + precision + "})?$");
        var vObjTo = $get(riskScore_uxParameterIndicatorTo);
        return regexPrecision.test(vObjTo.value);
    }
    else {
        return true;
    }
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
            focusTextbox(vObjFrom);
            return false;
        }
    }
    return true;
}

function paramIndicatorFromValidForCreate() {

    if (checkNotNullIndicatorForCreate()) {
        var vObjFrom = $get(riskScore_uxParameterIndicatorFrom);

        if (!hasOnlyCharacters(vObjFrom.value, const_ValidCharsIndicator)) {
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

function paramIndicatorToRequiredForCreate() {

    if (checkNotNullIndicatorForCreate()) {
        var vObjTo = $get(riskScore_uxParameterIndicatorTo);
        if (trim(vObjTo.value) == '') {
            focusTextbox(vObjTo);
            return false;
        }

        return true;
    }
    else {
        return true;
    }

}

function paramIndicatorToValidForCreate() {
    if (checkNotNullIndicatorForCreate()) {
        var vObjTo = $get(riskScore_uxParameterIndicatorTo);
        if (!hasOnlyCharacters(vObjTo.value, const_ValidCharsIndicator)) {
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
    if (trim(vObjScore.value) == '' && isParameterSelected) {
        focusTextbox(vObjScore);
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

    if (compareFromToForCreate() && paramIndicatorToValidForCreate() && paramIndicatorFromValidForCreate()) {
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

        var actionUrl = rootURL + "Risk/rm_RiskScores.aspx/ValidateExisting";
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

