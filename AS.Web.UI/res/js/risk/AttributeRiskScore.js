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
    $find(uxMetric_ClientID).clearSelection();
    $find(uxMetric_ClientID).disable();
    $find(uxOperand_ClientID).clearSelection();
    $find(uxOperand_ClientID).disable();
    $find(uxScore_ClientID).set_value('');
    $find(uxScore_ClientID).disable();
}

function doValidFromTo() {
    clearError();
    if ((doValidationAttribute() && doValidationFromTo())) {
        __doPostBack(attr_RiskScore_uxUpdate_ClientID, '');

    }
}
function doValidOperand() {
    clearError();
    if ((doValidationAttribute() && doValidationOperand())) {
        __doPostBack(attr_RiskScore_uxUpdate_ClientID, '');
    }
}

function doValidOperandCombobox() {
    clearError();
    if ((doValidationAttribute() && doValidationOperand() && doValidatorMetricCombobox())) {
        __doPostBack(attr_RiskScore_uxUpdate_ClientID, '');

    }
}

function doValidOperandModal() {
    clearError();
    if ((doValidationAttribute() && doValidationOperand() && doValidatorMetricModal())) {
        __doPostBack(attr_RiskScore_uxUpdate_ClientID, '');

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
    if (parseInt(to.get_value()) <= parseInt(from.get_value())) {
        $('label[for=' + uxFrom_ClientID + ']').addClass('label-error');
        return false;
    }
    else
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