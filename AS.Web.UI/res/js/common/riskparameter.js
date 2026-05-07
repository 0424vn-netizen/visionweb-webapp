
//==================================== startup =================================================


var validatingLowHighThreshold = false;
var validatingLowHighThreshold_id = '';

var validatingFromToIndicator = false;
var validatingFromToIndicator_id = '';

var validatingFromToIndicatorNRT = false;
var validatingFromToIndicator_idNRT = '';
var _isShowinLine = false;

$(function () {
    /*setTimeout("disableRiskScoreTextBoxes();", 300);*/
});

//==================================== riskscore ===============================================

function RadNumericTextBox_KeyPress(sender, eventArgs) {
    if (eventArgs.get_keyCode() == 13 || eventArgs.get_keyCode() == 45 || eventArgs.get_keyCharacter() === sender.get_numberFormat().DecimalSeparator) {
        eventArgs.set_cancel(true);
    }
}

function countSelectedParameters() {
    var count = 0;
    for (var i = 0; i < ParameterInfo.length; i++) {
        var paramInfo = ParameterInfo[i];
        var cb = $get(paramInfo.CbClientId);
        if (cb.checked)
            count++;
    }

    return count;
}


function checkAtLeastOneParameter() {
    var count = countSelectedParameters();
    if (count == 0) {
        alert(RiskParameter_js_MustBeSelected);
        return false;
    }
    return true;
}

function maxLengthCheck(e) {
    var maxValue = ($(e).attr("maxvalue") && $(e).attr("maxvalue").trim().length > 0) ? parseFloat($(e).attr("maxvalue")) : 70368744177664;
    if (parseFloat(e.value) > maxValue) {
        e.value = e.value.toString().slice(0, maxValue.toString().length);
    }
}
//================================ parameter textbox ======================================
function txtParameterValue_KeyPress(e) {
    validatingFromToIndicator = false;
    validatingFromToIndicator_id = '';
    validatingFromToIndicatorNRT = false;
    validatingFromToIndicator_idNRT = '';

    var evt = window.event ? window.event : e;
    var code = evt.keyCode ? evt.keyCode : e.which;
    if ((code >= 48 && code <= 57) ||
        /*code == 37 ||*/ code == 39 || code == 46 || code == 8 || code == 9)  //left arrow, righ arrow, del, backspace, tab
        return true;
    else {
        e.preventDefault ? e.preventDefault() : e.returnValue = false;
        return false;
    }

}

function txtParameterValue_Blur(e, isMCF) {
    var sender = window.event ? event.srcElement : e.target;
    if (checkInputValue(sender, isMCF)) {
        var value = sender.value;
        //value = value.replace(/,/g, "");
        value = removeCommas(value);
        if (parseInt(value) <= 999999999 || isMCF) {
            var precision = parseInt($(sender).attr("precision"));
            if (precision > 0) {
                value = parseFloat(value).toFixed(precision);
            }
            value = addCommas(value);
            var parent = $(e.target).parents("div[id*='param']");
            var label = $(e.target).parents("div[class*='w-input']").find("label[class*='non-edit']");
            if (label.length > 0) {
                var id = label.prop("id").split("_");
                $("body").find("div[id='" + parent.prop("id") + "']").each(function () {
                    if (sender.defaultValue.indexOf("(") != -1) {
                        $($(this).find("label[id*='" + id[id.length - 1] + "']")).css("color", "red");
                        $($(this).find("label[id*='" + id[id.length - 1] + "']")).text("(" + value + ")");
                    }
                    else
                        $($(this).find("label[id*='" + id[id.length - 1] + "']")).text(value);
                });
            }
        }
    }
}

function txtValue_Blur(e, isMCF) {
    var sender = window.event ? event.srcElement : e.target;
    //if (checkInputValue(sender, isMCF, true)) {
    var value = sender.value;
    value = removeCommas(value);
    if (parseInt(value) <= 999999999 || isMCF) {
        var precision = parseInt($(sender).attr("precision"));
        if (precision > 0) {
            value = parseFloat(value).toFixed(precision);
        }
        value = addCommas(value);
        var parent = $(e.target).parents("div[id*='param']");
        var label = $(e.target).parents("div[class*='w-input']").find("label[class*='non-edit']");
        if (label.length > 0) {
            var id = label.prop("id").split("_");
            $("body").find("div[id='" + parent.prop("id") + "']").each(function () {
                if (sender.defaultValue.indexOf("(") != -1) {
                    $($(this).find("label[id*='" + id[id.length - 1] + "']")).css("color", "red");
                    $($(this).find("label[id*='" + id[id.length - 1] + "']")).text("(" + value + ")");
                }
                else
                    $($(this).find("label[id*='" + id[id.length - 1] + "']")).text(value);
            });
        }
    }
    //}
}


function txtParameterValue_Focus(e) {
    var sender = window.event ? event.srcElement : e.target;

    if (trim(sender.value) != "")
        sender.value = removeCommas(sender.value);

    var errCtrlID = "";
    try { errCtrlID = sender.attributes["ErrControlID"].value; } catch (e) { errCtrlID = ""; }
    ShowError(errCtrlID, false);

    moveCaretToEnd(sender);
}

function moveCaretToEnd(sender) {
    if (sender.createTextRange) {
        var r = sender.createTextRange();
        r.moveStart('character', (sender.value.length));
        r.collapse();
        r.select();
    }
}

function ShowError(ctrlID, isShow) {
    if (_isShowinLine)
        return;
    var errCtrl = $get(ctrlID);
    if (errCtrl != null) {
        errCtrl.innerHTML = isShow ? "<font color='red'>*</font>" : "";
    }
}

function setTextBoxFocus(id) {
    var txt = $get(id);
    txt.focus();
}

function checkInputValue(txt, isMCF, showInline, isNotCheckFromTo, valueDisplay) {
    _isShowinLine = showInline;
    ///<summary>
    ///Check the input value of textbox
    ///</summary>
    //if (txt.value != "")
    txt.value = (valueDisplay) ? valueDisplay : txt.value.replace("-", "");
    var isThreshold = (txt.id.indexOf("txtThreshold") > 0);
    var currentValueStr = removeCommas(checkNegative(checkStartWithCommas(txt.value)));
    var currentValue = (isMCF && currentValueStr.trim().length == 0) ? "" : parseFloat(currentValueStr, 10);
    var currentValueInt = (isMCF && currentValueStr.trim().length == 0) ? "" : parseInt(currentValueStr, 10);
    var precision = (txt.attributes["Precision"] == null ? 0 : parseInt(txt.attributes["Precision"].value, 10));
    var errCtrlID = "";

    //IndicatorFrom, To
    var isIndFrom = (txt.id.indexOf("txtFrom") > 0);
    var isIndTo = (txt.id.indexOf("txtTo") > 0);
    if (isNaN(precision) || precision == null)
        precision = 0;

    //get error control
    try {
        errCtrlID = txt.attributes["ErrControlID"].value;
    }
    catch (e) {
        errCtrlID = "";
    }

    //========= process for threshold textbox =========
    if ((isIndFrom && currentValueStr.length == 0) && (isNaN(currentValue) || currentValue == null)) {
        ShowError(errCtrlID, true);
        showMessageError(txt, showInline, RiskParameter_js_IndicatorFrom, ERR_REQUIREDFIELD)
        return false;
    }
    else if ((isIndTo && currentValueStr.length == 0) && (isNaN(currentValue) || currentValue == null)) {
        ShowError(errCtrlID, true);
        showMessageError(txt, showInline, RiskParameter_js_IndicatorTo, ERR_REQUIREDFIELD)
        return false;
    }
    else if (
        !isMCF && (((isThreshold && currentValueStr.length == 0) && (isNaN(currentValue) || currentValue == null))
            || (!isThreshold && (isNaN(currentValue) || currentValue == null)))
    ) {
        ShowError(errCtrlID, true);
        showMessageError(txt, showInline, isThreshold ? RiskParameter_js_Threshold : RiskParameter_js_Indicator, ERR_REQUIREDFIELD)
        return false;
    } else if (!isThreshold && (isNaN(currentValue))) {
        ShowError(errCtrlID, true);
        showMessageError(txt, showInline, isThreshold ? RiskParameter_js_Threshold : RiskParameter_js_Indicator, ERR_REQUIREDFIELD)
        return false;
    }

    if (isThreshold && (isNaN(currentValue) || currentValue == null))
        currentValue = "";


    if (precision == 0 && (currentValue.toString().split(".").length > 0 && parseInt(currentValue.toString().split(".")[1]) > 0)) {
        ShowError(errCtrlID, true);
        showMessageError(txt, showInline, "", ERR_INTNUMBER)
        return false;
    }
    else if (precision > 0) {
        currentValueStr = currentValue.toString();
        if (currentValueStr.indexOf(".") >= 0) {
            currentValueStr = currentValueStr.substring(currentValueStr.indexOf(".") + 1);
            if (currentValueStr.length > precision) {
                ShowError(errCtrlID, true);
                showMessageError(txt, showInline, "", RiskParameter_js_ThePartMustBeLessThan + ' ' + (precision + 1) + '.')
                return false;
            }
        }
    }

    if (isNotCheckFromTo)
        return checkMinMax(txt, showInline, isMCF)

    //verify Threshold low - high
    var arrayID = txt.id.split("_");
    var tempId = "";
    //-2 use for rad textbox
    for (var i = 0; i < arrayID.length - 2; i++) {
        tempId += arrayID[i] + "_";
    }

    var from = document.getElementById(tempId + "txtThresholdLow");
    var to = document.getElementById(tempId + "txtThresholdHigh");
    //for normal textbox
    if (from == null && to == null) {
        tempId = "";
        for (var i = 0; i < arrayID.length - 1; i++) {
            tempId += arrayID[i] + "_";
        }
        from = document.getElementById(tempId + "txtThresholdLow");
        to = document.getElementById(tempId + "txtThresholdHigh");
    }

    if (from != null && to != null) {

        var fromValue = removeCommas(checkStartWithCommas(from.value));
        var toValue = removeCommas(checkStartWithCommas(to.value));

        if (validatingLowHighThreshold) {
            if (validatingLowHighThreshold_id == txt.id && parseInt(fromValue) > parseInt(toValue)) {
                ShowError(errCtrlID, true);
                showMessageError(txt, showInline, "", ERR_LOWGTHIGH)
                return false;
            }
            else {
                return true;
            }
        }
        else if (parseInt(fromValue) > parseInt(toValue)) {
            validatingLowHighThreshold = true;
            validatingLowHighThreshold_id = txt.id;
            ShowError(errCtrlID, true);
            showMessageError(txt, showInline, "", ERR_LOWGTHIGH)
            return false;
        }
    }
    tempId = "";
    //-2 use for rad textbox
    for (var i = 0; i < arrayID.length - 2; i++) {
        tempId += arrayID[i] + "_";
    }

    var fromInd = document.getElementById(tempId + "txtFrom");
    var toInd = document.getElementById(tempId + "txtTo");
    //for normal textbox
    if (fromInd == null && toInd == null) {
        tempId = "";
        for (var i = 0; i < arrayID.length - 1; i++) {
            tempId += arrayID[i] + "_";
        }
        fromInd = document.getElementById(tempId + "txtFrom");
        toInd = document.getElementById(tempId + "txtTo");
    }

    if (fromInd != null && toInd != null) {

        var fromIndValue = removeCommas(checkStartWithCommas(fromInd.value));
        var toIndValue = removeCommas(checkStartWithCommas(toInd.value));
        if (validatingFromToIndicator) {
            const fromIndValToPrecision = getValueToPrecision(fromIndValue, precision);
            const toIndValToPrecision = getValueToPrecision(toIndValue, precision);
            if (validatingFromToIndicator_id == txt.id && fromIndValToPrecision > toIndValToPrecision) {
                ShowError(errCtrlID, true);
                showMessageError(txt, showInline, "", ERR_TOLOWERFROM)
                return false;
            }
            else {
                return true;
            }
        }
        else {
            const fromIndValToPrecision = getValueToPrecision(fromIndValue, precision);
            const toIndValToPrecision = getValueToPrecision(toIndValue, precision);
            if (fromIndValToPrecision > toIndValToPrecision) {
                validatingFromToIndicator = true;
                validatingFromToIndicator_id = txt.id;
                ShowError(errCtrlID, true);
                showMessageError(txt, showInline, "", ERR_TOLOWERFROM)
                return false;
            }
        }
    }

    return checkMinMax(txt, showInline, isMCF)

}
function checkMinMax(txt, showInline, isMCF) {
    //============ process for indicator textbox ==============
    //check min max
    var currentValueStr = removeCommas(checkNegative(checkStartWithCommas(txt.value)));
    var currentValue = (isMCF && currentValueStr.trim().length == 0) ? "" : parseFloat(currentValueStr, 10);
    var isThreshold = (txt.id.indexOf("txtThreshold") > 0);
    var errCtrlID = "";

    //get error control
    try {
        errCtrlID = txt.attributes["ErrControlID"].value;
    }
    catch (e) {
        errCtrlID = "";
    }


    var minValue = -1;
    var maxValue = -1;
    var intMinValue = 0;
    try {
        minValue = parseFloat(removeCommas(checkStartWithCommas(txt.attributes["MinValue"].value), 10));
        intMinValue = parseInt(removeCommas(checkStartWithCommas(txt.attributes["MinValue"].value), 10));
    } catch (e) {
        minValue = -1;
    }
    try {
        var attr = txt.attributes["Precision"];
        var precision = attr != null ? parseInt(attr.value) : 0;
        if (precision >= 1) {
            maxValue = parseFloat(removeCommas(checkStartWithCommas(txt.attributes["MaxValue"].value), 10));
        }
        else {
            maxValue = parseInt(removeCommas(checkStartWithCommas(txt.attributes["MaxValue"].value), 10));
        }

    } catch (e) {
        maxValue = -1;
    }

    //empty cell, not check min max
    if (minValue == -1 && maxValue == -1) {
        return true;
    }

    if (currentValue == -1 || (maxValue > -1 && currentValue > maxValue)) {
        ShowError(errCtrlID, true);
        showMessageError(txt, showInline, "", ERR_MAXEXCEED + addCommas(maxValue.toString()) + ".")
        return false;
    }

    //Validate minvalue for Indication only
    if (!isThreshold && !isMCF) {
        if (currentValue < minValue) {
            ShowError(errCtrlID, true);
            showMessageError(txt, showInline, "", RiskParameter_js_Indicator_Min + " " + Math.round(minValue) + ".")
            return false;
        }
    }
    //add commas for value of textbox
    if (txt.value.startsWith("("))
        currentValue = "(" + currentValue + ")";
    txt.value = addCommas(currentValue.toString());
    return true;

}
function showMessageError(sender, showInline, field, message) {
    if (!showInline) {
        alert(field + message);
        setTimeout("setTextBoxFocus('" + sender.id + "');", 200);
    }
    else {
        message = message.trim();
        if (message.indexOf(":") == 0)
            message = message.substring(1, message.length).trim();
        var parentTD = $(sender).parents("td");
        if (parentTD && parentTD.length > 0) {
            var msgError = $(parentTD).find("div[id='msgError']");
            if (msgError && msgError.length > 0) {
                $(msgError[0]).html(message);
            }
        }
        else {
            var parentItem = $(sender).parents(".item");
            if (parentItem && parentItem.length > 0) {
                var msgError = $(parentItem).find(".msgError");
                if (msgError && msgError.length > 0) {
                    $(msgError[0]).html(message);
                }
            }
        }
    }
}

function checkStartWithCommas(val) {
    if (val.startsWith("."))
        return trim("0" + val);
    return trim(val);
}
function checkNegative(val) {
    if (val.startsWith("("))
        return trim(val.replace("(", "").replace(")", ""));
    return trim(val);
}
//=================================== threshold textbox ===================================

function txtThreshold_KeyPress(e) {
    validatingLowHighThreshold = false;
    validatingLowHighThreshold_id = '';
    return txtParameterValue_KeyPress(e);
}

function txtThreshold_Blur(e, isMCF) {
    txtParameterValue_Blur(e, isMCF);
}

function txtThresholdLow_Blur(e, isMCF) {
    txtParameterValue_Blur(e, isMCF);
}

function txtThresholdHigh_Blur(e, isMCF) {
    txtParameterValue_Blur(e, isMCF);
}

function txtFrom_Blur(e, isMCF) {
    txtParameterValue_Blur(e, isMCF);
}

function txtTo_Blur(e, isMCF) {
    txtParameterValue_Blur(e, isMCF);
}


function txtThreshold_Focus(e) {
    txtParameterValue_Focus(e);
}
//=================================== checkbox controls ====================================

function chkAllParameters_Click(chk) {
    if (chk.checked) {
        for (var i = 0; i < ParameterInfo.length; i++) {
            var paramInfo = ParameterInfo[i];
            var cb = $get(paramInfo.CbClientId);
            if (!cb.checked) {
                cb.checked = true;
                disableItem(paramInfo, !paramInfo.IsUseThs, false);
            }
        }
    }
    else {
        // Get All Parameter have assigment list
        // get Assignment list of Parameter if exist
        var paramList = "";
        var url = "rm_MCF_Parameters.aspx/GetParamInfo";
        $.ajax({
            type: "POST",
            url: url,
            async: false,
            data: '{"parameterKey":"' + "" + '",isCheckAll:"' + true + '"}',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (res) {
                paramList = res.d[0];
            },
            error: function (result) {
            }
        });

        if (!showRiskScore) {
            if (paramList != "") {
                if (usedMarketData)
                    alert(RiskParameter_js_MsgDisablingParameter);
                else
                    alert(RiskParameter_js_Msg1DisablingParameter);
            }
        }


        var arrParam = paramList.split(",");
        for (var i = 0; i < ParameterInfo.length; i++) {
            var paramInfo = ParameterInfo[i];
            if (jQuery.inArray(paramInfo.ParamKey, arrParam) == -1) {
                var cb = $get(paramInfo.CbClientId);
                if (cb.checked) {
                    cb.checked = false;
                    disableItem(paramInfo, !paramInfo.IsUseThs, true);
                }
            }
        }

        populateCheckBoxAllCtrl();
    }
}

function hasUncheckableParameter() {
    for (var i = 0; i < ParameterInfo.length; i++) {
        var paramInfo = ParameterInfo[i];
        if (paramInfo.AssList != '') {
            return true;
        }
        if (paramInfo.RiskScoreList == 'true') {
            return true;
        }
    }

    return false;
}

function populateCheckBoxAllCtrl() {
    //check for not checked cb
    var allAreChecked = true;

    for (var i = 0; i < ParameterInfo.length; i++) {
        var paramInfo = ParameterInfo[i];
        var cb = $get(paramInfo.CbClientId);
        if (cb != null && !cb.checked) {
            allAreChecked = false;
            break;
        }
    }


    if (allAreChecked) {
        //check checkboxall ctrl
        $get('chkAllParameters').checked = true;
        //$('#chkAllParameters').attr({ checked: true });

        return;
    }
}

function htmlEncode(html) {
    return $('<div/>').text(html).html();
}

var showRiskScore = null;
var ParameterInfo = null;
function disableParameter(checkBoxId, isNotUseThreshold, isDisable, isShowAlert) {
    if (isDisable) {
        $('#' + checkBoxId).parent().parent().next().find('.EditButtonAssignment').css('display', 'none');

    } else {
        $('#' + checkBoxId).parent().parent().next().find('.EditButtonAssignment').css('display', 'inline');
    }

    for (var i = 0; i < ParameterInfo.length; i++) {
        var paramInfo = ParameterInfo[i];

        if (paramInfo.CbClientId == checkBoxId) {
            //check for question "is there any assignments used this parameter?"
            //AND check for question "is there any riskscore for this parameter?"
            if (isDisable && !showRiskScore) {

                // get Assignment list or Risk Score of Parameter if exist
                var assList = "";
                var riskScoreList = "";
                var url = "rm_MCF_Parameters.aspx/GetParamInfo";
                $.ajax({
                    type: "POST",
                    url: url,
                    async: false,
                    data: '{"parameterKey":"' + paramInfo.ParamKey + '",isCheckAll:"' + false + '"}',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (res) {
                        assList = res.d[0];
                        riskScoreList = res.d[1];
                    },
                    error: function (result) {
                    }
                });

                if (assList != "") {
                    var chkBoxObj = $get(checkBoxId);
                    chkBoxObj.checked = true;
                    if (isShowAlert) {
                        if (usedMarketData)
                            alert(RiskParameter_js_MonitoringParameter + " ('" + assList + "').  " + RiskParameter_js_RemoveTheParameter);
                        else
                            alert(RiskParameter_js_AssignmentMonitoringParam + " ('" + assList + "').  " + RiskParameter_js_AssignmentDisablingParam);
                    }
                    return false;
                }

                if (riskScoreList == "true") {
                    var chkBoxObj = $get(checkBoxId);
                    chkBoxObj.checked = true;
                    if (isShowAlert) {
                        alert(RiskParameter_js_RiskScoreDisablingParam);
                    }
                    return false;
                }
            }

            disableItem(paramInfo, isNotUseThreshold, isDisable);
        }
    }
    return true;
}

function disableItem(paramInfo, isNotUseThreshold, isDisable) {
    var indicatorObj = $('#' + paramInfo.IndClientId);
    indicatorObj.attr({ disabled: isDisable });

    var indicatorFromObj = $('#' + paramInfo.IndFromID);
    indicatorFromObj.attr({ disabled: isDisable });

    var indicatorToObj = $('#' + paramInfo.IndToID);
    indicatorToObj.attr({ disabled: isDisable });

    var defineFilterObj = $('#' + paramInfo.DefineFilterClientId);
    defineFilterObj.attr({ disabled: isDisable });

    if (!isNotUseThreshold) {
        var thresholdObj = $('#' + paramInfo.ThsClientId);
        thresholdObj.attr({ disabled: isDisable });

        var lowThresholdObj = $('#' + paramInfo.ThsLowID);
        lowThresholdObj.attr({ disabled: isDisable });

        var highThresholdObj = $('#' + paramInfo.ThsHighID);
        highThresholdObj.attr({ disabled: isDisable });
    }
}


function parameterCheckBox_Click(chkBoxObj, parameterKey, paramValueId, thresholdId, defineFilterID, isNotUseThreshold) {
    if (chkBoxObj.checked) {
        disableParameter(chkBoxObj.id, isNotUseThreshold, false, true);
        populateCheckBoxAllCtrl();
    }
    else {
        var result = disableParameter(chkBoxObj.id, isNotUseThreshold, true, true);
        if (result) {
            $get('chkAllParameters').checked = false;
        }
    }
}

//===================================== utilities functions ======================================
function removeCommas(nStr) {
    ///<summary>
    ///Remove commas from number string.
    ///</summary>
    var resultStr = "";
    for (var i = 0; i < nStr.length; i++) {
        var chr = nStr.charAt(i);
        if (chr != ',')
            resultStr += chr.toString();
    }
    return resultStr;
}

function addCommas(nStr) {
    ///<summary>
    ///Add commas to number string.
    ///</summary>
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

function getValueToPrecision(inputValue, precision) {
    const precisionConst = precision ?? 0;
    return precisionConst >= 1 ? parseFloat(inputValue) : parseInt(inputValue);
}
