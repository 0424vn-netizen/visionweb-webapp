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

function ClearFormValue() {
    $find(uxAttributeNameList_ClientID).clearSelection();
    $('#' + uxRangeName_ClientID).val('');
    $('#' + uxRangeName_ClientID).attr('disabled', 'disabled');
    $find(uxFrom_ClientID).set_value('');
    $('#' + uxFrom_ClientID).attr('disabled', 'disabled');
    $find(uxTo_ClientID).set_value('');
    $('#' + uxTo_ClientID).attr('disabled', 'disabled');
    if ($find(uxMetric_ClientID) != undefined) {
        $find(uxMetric_ClientID).clearSelection();
        $find(uxMetric_ClientID).disable();
    }
    if ($find(uxMetricFrom_ClientID) != undefined) {
        $('#' + uxMetricFrom_ClientID).attr('disabled', 'disabled');
        $('#' + uxMetricFrom_ClientID).css('width', "150px");
    }
    if ($find(uxMetricTo_ClientID) != undefined) {
        $('#' + uxMetricTo_ClientID).css('display', "none");
        $('#ctl00_' + uxMetricFrom_Label_ClientID + "Panel").css('display', "none");
        $('#ctl00_' + uxMetricTo_Label_ClientID + "Panel").css('display', "none");
    }
    $find(uxOperand_ClientID).clearSelection();
    $find(uxOperand_ClientID).disable();
    $find(uxScore_ClientID).set_value('');
    $find(uxScore_ClientID).disable();
}

function doValidFromTo() {
    if (isCancelSubmit) {
        return false;
    }
    clearError();
    if ((doValidationAttribute() && doValidationFromTo() && ValidateGreaterOrEqualThan())) {
        __doPostBack(attr_RiskScore_uxUpdate_ClientID, '');

    }
}
function doValidOperand() {
    if (isCancelSubmit) {
        return false;
    }
    clearError();
    if ((doValidationAttribute() && doValidationOperand())) {
        __doPostBack(attr_RiskScore_uxUpdate_ClientID, '');
    }
}

function doValidOperandCombobox() {
    if (isCancelSubmit) {
        return false;
    }
    clearError();
    if ((doValidationAttribute() && doValidationOperand() && doValidatorMetricCombobox())) {
        __doPostBack(attr_RiskScore_uxUpdate_ClientID, '');

    }
}

function doValidOperandModal() {
    if (isCancelSubmit) {
        return false;
    }
    clearError();
    if ((doValidationAttribute() && doValidationOperand() && doValidatorMetricModal())) {
        __doPostBack(attr_RiskScore_uxUpdate_ClientID, '');

    }
}

function doValidMetricType3() {
    if (isCancelSubmit) {
        return false;
    }
    clearError();
    if ((doValidationAttribute() && doValidationOperand() && doValidateCutom())) {
        __doPostBack(attr_RiskScore_uxUpdate_ClientID, '');

    }
}

function doValidateCutom() {
    var txtFromMetric = $find(uxMetricFrom_ClientID);
    var txtToMetric = $find(uxMetricTo_ClientID);
    var txtScore = $find(uxScore_ClientID);
    var metricMinValue = $('#' + hhdMinValueMetric_ClientID).val();
    var metricMaxValue = $('#' + hhdMaxValueMetric_ClientID).val();
    var isValid = true;

    if (txtFromMetric != undefined) {
       if (txtFromMetric.get_value() === "") {
            //show message
            if (txtToMetric != undefined)
                ShowErrorMessage(txtFromMetric, messageRequired, false);
            else {

                ShowErrorMessage(txtFromMetric, messageRequired, false);
            }
            isValid = false;
        } else if (!isInteger(txtFromMetric.get_value()) || parseInt(txtFromMetric.get_value()) < 0 || parseInt(txtFromMetric.get_value()) > metricMaxValue) {
            if (txtToMetric != undefined)
                ShowErrorMessage(txtFromMetric, messageMetric_From.replace("[minvalue]", metricMinValue).replace("[maxvalue]", metricMaxValue), false);
            else {
                ShowErrorMessage(txtFromMetric, messageMetric_3.replace("[minvalue]", metricMinValue).replace("[maxvalue]", metricMaxValue), false);
            }
            isValid = false;
        }
        else {
            if (txtToMetric != undefined)
                HideErroMessage(txtFromMetric, false);
            else
                $($(txtFromMetric).closest('td').find("label")[0]).hide();
        }
        if (txtToMetric != undefined) {
            if (txtToMetric.get_value() === "") {
                //show message
                ShowErrorMessage(txtToMetric,messageRequired, false);
                isValid = false;
            } else if (!isInteger(txtToMetric.get_value()) || parseInt(txtToMetric.get_value()) < 0 || parseInt(txtToMetric.get_value()) > metricMaxValue) {
                ShowErrorMessage(txtToMetric, String.format(messageMetric_To.replace("[minvalue]", metricMinValue).replace("[maxvalue]", metricMaxValue)), false);
                isValid = false;
            }
            else if (txtFromMetric != undefined && (Number(txtFromMetric.get_value())) > Number(txtToMetric.get_value())) {
                //show message
                ShowErrorMessage(txtToMetric, messageToMustGreaterThanorEqualFrom, false);
                isValid = false;
            }
            else {
                HideErroMessage(txtToMetric, false);
            }
        }
    }

    if (txtScore.get_value() === "") {
        ShowErrorMessage(txtScore, messageScoreRequired, false);
        isValid = false;
    }
    else if (!isInteger(txtScore.get_value()) || parseInt(txtScore.get_value()) < 0 || parseInt(txtScore.get_value()) > 999999999) {
        ShowErrorMessage(txtScore, messageScoreGreaterOrEqual, false);
        isValid = false;
    }
    else {
        HideErroMessage(txtScore, false);
    }

    return isValid;
}

function ShowErrorMessage(control, message, isInput) {
    if (isInput) {
        $("label[for='" + control.get_id() + "_Input']").show();
        $("label[for='" + control.get_id() + "_Input']").html(message);
    } else {

        $("label[for='" + control.get_id() + "'][class='error']").show();
        $("label[for='" + control.get_id() + "'][class='error']").html(message);

        $("label[for='" + control.get_id() + "'][class*='risk-scores-first']").addClass('label-error');
    }
}

function HideErroMessage(control, isInput) {
    if (control != null) {
        if (isInput) {
            $("label[for='" + control.get_id() + "_Input']").hide();
        } else {
            $("label[for='" + control.get_id() + "'][class='error']").hide();
            $("label[for='" + control.get_id() + "'][class*='risk-scores-first']").removeClass('label-error');
        }
    }
}

function Metric_ValidateInput() {
    if ($('#' + uxMetricListTextHide_ClientID).length == 0) {
        var combo = $find(uxMetric_ClientID);
        if (combo._checkBoxes)
            return combo.get_checkedItems().length > 0;
        else
            return combo.get_selectedItem() != null;
    }
    else {
        return $('#' + uxMetricListTextHide_ClientID).val().length > 0;
    }
}

function uxMetricListText_ValidateInput() {
    return $('#' + uxMetricListTextHide_ClientID).val().length > 0;
}

function ValidateGreaterOrEqualThan() {
    var from = $find(uxFrom_ClientID);
    var to = $find(uxTo_ClientID);
    var minVaue = $('#' + hhdMinValueFromTo_ClientID).val();
    var maxVaue = $('#' + hhdMaxValueFromTo_ClientID).val();

    if (from != undefined && to != undefined && from.get_value() != "" && to.get_value() != "") {
        if (minVaue != "" && maxVaue != "") {
            if (parseInt(to.get_value()) < parseInt(from.get_value())) {
                ShowErrorMessage(from, messageToMustGreaterThanorEqualFrom, false);
                return false;
            }
            else {
                HideErroMessage(from, false);
                return true;
            }
        }
        else {
            if (parseInt(to.get_value()) <= parseInt(from.get_value())) {
                ShowErrorMessage(from, messageToMustGreaterThanFrom, false);
                return false;
            }
            else {
                HideErroMessage(from, false);
                return true;
            }
        }
    } else {
        return true;
    }
}
function uxMetricTo_ValidateInputFrom() {
    var from = $find(uxMetricFrom_ClientID);
    var to = $find(uxMetricTo_ClientID);
    var valuefrom = from.get_value();
    var tofrom = to.get_value();
    if (valuefrom != undefined && valuefrom != "" && tofrom != undefined || tofrom != "") {
        if (parseInt(tofrom) <= parseInt(valuefrom)) {
            return false;
        }
    }
    return true;
}
function checkRangeName() {
    var reg = new RegExp("^((?!(<[^ \t]))(?!(&#)).)*$");
    return reg.test($('#' + uxRangeName_ClientID).val());
}
function ValidateAttributeName() {
    var uxAttributeNameList = $find(uxAttributeNameList_ClientID);
    if (uxAttributeNameList.get_selectedItem() == null) {
        return false;
    }
    return true;
}

function GetSelectedMetric() {
    return $('#' + uxMetricListTextHide_ClientID).val();
}
function SetSelectedMetric(val) {
    $('#' + uxMetricListText_ClientID).val(val);
    $('#' + uxMetricListTextHide_ClientID).val(val);
    if (val.split(';').length > 2) {
        $('#' + uxMetricListText_ClientID).val(val.split(';').length + ' ' + msgitemsSelected);
    }
}


function MetricItemCheck(sender, args) {

    sender.get_attributes().setAttribute("MetricText", sender.get_text());
}

function MetricOnBlur(sender, args) {
    var metricText = sender.get_attributes().getAttribute("MetricText");
    if (metricText) {
        sender.set_text(metricText);
    } else {
        sender.set_text("");
    }
}

function checkInputCharacter(sender, args) {
    var val = sender.get_value();
    if (isNaN(val))
        sender.set_value("");
}

function checkInputCharacterMetric() {
    var value = $('#' + uxMetricListTextHide_ClientID).val();
    $('#' + uxMetricListText_ClientID).val(value);
}

function doValidationAttributeCustom() {
    if (isCancelSubmit) {
        return false;
    }
    return doValidationAttribute();
}

var isCancelSubmit = false;
function addCheckSpecialCharacters() {
    $("#" + uxRangeName_ClientID).change(function (e) {
        if (isIncludeSpecialCharacters(this)) {
            isCancelSubmit = true;
            doValidationRangeName();
            removeSpecialCharacters(this);
            setTimeout('isCancelSubmit = false;', 500);
            e.preventDefault();
        }
    });
}

$(document).ready(function () {
    addCheckSpecialCharacters();
});