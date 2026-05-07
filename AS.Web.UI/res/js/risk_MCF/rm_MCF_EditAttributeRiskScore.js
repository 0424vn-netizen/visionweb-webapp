function clearErrorEdit() {
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


function doValidFromToEdit() {
    if (isCancelSubmit) {
        return false;
    }
    clearErrorEdit();
    if ((doValidationAttributeEdit() && doValidationFromToEdit() && ValidateGreaterOrEqualThanEdit())) {
        __doPostBack(attrEdit_RiskScore_uxUpdate_ClientID, '');
    }
    return false;
}
function doValidOperandEdit() {
    if (isCancelSubmit) {
        return false;
    }
    clearErrorEdit();
    if ((doValidationAttributeEdit() && doValidationOperandEdit())) {
        __doPostBack(attrEdit_RiskScore_uxUpdate_ClientID, '');
    }
    return false;
}



function doValidOperandEditCombobox() {
    if (isCancelSubmit) {
        return false;
    }
    clearErrorEdit();
    if ((doValidationAttributeEdit() && doValidationOperandEdit() && doValidatorMetricComboboxEdit())) {
        __doPostBack(attrEdit_RiskScore_uxUpdate_ClientID, '');
    }
    return false;
}

function doValidOperandEditModal() {
    if (isCancelSubmit) {
        return false;
    }
    clearErrorEdit();
    if ((doValidationAttributeEdit() && doValidationOperandEdit() && doValidatorMetricModalEdit())) {
        __doPostBack(attrEdit_RiskScore_uxUpdate_ClientID, '');
    }
    return false;
}

function doValidMetricType3Edit() {
    clearError();
    if ((doValidationAttributeEdit() && doValidationOperandEdit() && doValidateCutomEdit())) {
        __doPostBack(attrEdit_RiskScore_uxUpdate_ClientID, '');
    }
}

function doValidateCutomEdit() {
    var txtFromMetric = $find(uxMetricFromEdit_ClientID);
    var txtToMetric = $find(uxMetricToEdit_ClientID);
    var txtScore = $find(uxScoreEdit_ClientID);
    var metricMinValue = $('#' + hhdMinValueMetricEdit_ClientID).val();
    var metricMaxValue = $('#' + hhdMaxValueMetricEdit_ClientID).val();
    var isValid = true;

    if (txtFromMetric != undefined) {
        if (txtFromMetric.get_value() === "") {
            //show message
            if (txtToMetric != undefined)
                ShowErrorMessageEdit(txtFromMetric, messageRequired, false);
            else {

                ShowErrorMessageEdit(txtFromMetric, messageRequired, false);
            }
            isValid = false;
        } else if (!isInteger(txtFromMetric.get_value()) || parseInt(txtFromMetric.get_value()) < 0 || parseInt(txtFromMetric.get_value()) > metricMaxValue) {
            if (txtToMetric != undefined)
                ShowErrorMessageEdit(txtFromMetric, messageMetric_From.replace("[minvalue]", metricMinValue).replace("[maxvalue]", metricMaxValue), false);
            else {
                ShowErrorMessageEdit(txtFromMetric, messageMetric_3.replace("[minvalue]", metricMinValue).replace("[maxvalue]", metricMaxValue), false);
            }
            isValid = false;
        }
        else {
            if (txtToMetric != undefined)
                HideErroMessageEdit(txtFromMetric, false);
            else
                $($(txtFromMetric).closest('td').find("label")[0]).hide();
        }
        if (txtToMetric != undefined) {
            if (txtToMetric.get_value() === "") {
                //show message
                ShowErrorMessageEdit(txtToMetric, messageRequired, false);
                isValid = false;
            } else if (!isInteger(txtToMetric.get_value()) || parseInt(txtToMetric.get_value()) < 0 || parseInt(txtToMetric.get_value()) > metricMaxValue) {
                ShowErrorMessageEdit(txtToMetric, String.format(messageMetric_To.replace("[minvalue]", metricMinValue).replace("[maxvalue]", metricMaxValue)), false);
                isValid = false;
            }
            else if (txtFromMetric != undefined && (Number(txtFromMetric.get_value())) > Number(txtToMetric.get_value())) {
                //show message
                ShowErrorMessageEdit(txtToMetric, messageToMustGreaterThanorEqualFrom, false);
                isValid = false;
            }
            else {
                HideErroMessageEdit(txtToMetric, false);
            }
        }
    }

    if (txtScore.get_value() === "") {
        ShowErrorMessageEdit(txtScore, messageScoreRequired, false);
        isValid = false;
    }
    else if (!isInteger(txtScore.get_value()) || parseInt(txtScore.get_value()) < 0 || parseInt(txtScore.get_value()) > 999999999) {
        ShowErrorMessageEdit(txtScore, messageScoreGreaterOrEqual, false);
        isValid = false;
    }
    else {
        HideErroMessageEdit(txtScore, false);
    }

    return isValid;
}

function ShowErrorMessageEdit(control, message, isInput) {
    if (isInput) {
        $("label[for='" + control.get_id() + "_Input']").show();
        $("label[for='" + control.get_id() + "_Input']").html(message);
    } else {

        $("label[for='" + control.get_id() + "'][class='error']").show();
        $("label[for='" + control.get_id() + "'][class='error']").html(message);

        $("label[for='" + control.get_id() + "'][class*='control-inline-label']").addClass('label-error');
    }
}

function HideErroMessageEdit(control, isInput) {
    if (control != null) {
        if (isInput) {
            $("label[for='" + control.get_id() + "_Input']").hide();
        } else {
            $("label[for='" + control.get_id() + "'][class='error']").hide();
            $("label[for='" + control.get_id() + "'][class*='control-inline-label']").removeClass('label-error');
        }
    }
}

function uxMetricListText_ValidateInput() {
    return $('#' + uxMetricListTextEditHide_ClientID).val().length > 0;
}


function Metric_ValidateInputEdit() {
    var combo = $find(uxMetricEdit_ClientID);
    if (combo._checkBoxes)
        return combo.get_checkedItems().length > 0;
    else
        return combo.get_selectedItem() != null;
}
function ValidateGreaterOrEqualThanEdit() {
    var from = $find(uxFromEdit_ClientID);
    var to = $find(uxToEdit_ClientID);
    var minVaue = $('#' + hhdMinValueFromToEdit_ClientID).val();
    var maxVaue = $('#' + hhdMaxValueFromToEdit_ClientID).val();

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
function checkRangeNameEdit() {
    var reg = new RegExp("^((?!(<[^ \t]))(?!(&#)).)*$");
    return reg.test($('#' + uxRangeNameEdit_ClientID).val());
}
function ValidateAttributeNameEdit() {
    var uxAttributeNameList = $find(uxAttributeNameListEdit_ClientID);
    if (uxAttributeNameList.get_selectedItem() == null) {
        return false;
    }
    return true;
}
function GetSelectedMetricEdit() {
    return $('#' + uxMetricListTextEditHide_ClientID).val();
}
function SetSelectedMetricEdit(val) {
    $('#' + uxMetricListTextEdit_ClientID).val(val);
    $('#' + uxMetricListTextEditHide_ClientID).val(val);

    if (val.split(';').length > 2) {
        $('#' + uxMetricListTextEdit_ClientID).val(val.split(';').length + ' ' + msgitemsSelected);
    }
}
function uxMetricTo_ValidateInputFrom() {
    var from = $find(uxMetricFromEdit_ClientID);
    var to = $find(uxMetricToEdit_ClientID);
    var valuefrom = from.get_value();
    var tofrom = to.get_value();
    if (valuefrom != undefined && valuefrom != "" && tofrom != undefined || tofrom != "") {
        if (parseInt(tofrom) <= parseInt(valuefrom)) {
            return false;
        }
    }
    return true;
}

function doValidationAttributeEditCustom() {
    if (isCancelSubmit) {
        return false;
    }
    return doValidationAttributeEdit();
}

var isCancelSubmit = false;
function addCheckSpecialCharacters() {
    $("#" + uxRangeNameEdit_ClientID).change(function (e) {
        if (isIncludeSpecialCharacters(this)) {
            isCancelSubmit = true;
            doValidationOperandEdit();
            removeSpecialCharacters(this);
            setTimeout('isCancelSubmit = false;', 500);
        }
    });
}

$(document).ready(function () {
    addCheckSpecialCharacters();
});