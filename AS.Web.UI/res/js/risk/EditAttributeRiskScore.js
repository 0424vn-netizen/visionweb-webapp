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
    clearErrorEdit();
    if ((doValidationAttributeEdit() && doValidationFromToEdit())) {
        __doPostBack(attrEdit_RiskScore_uxUpdate_ClientID, '');
    }
    return false;
}
function doValidOperandEdit() {
    clearErrorEdit();
    if ((doValidationAttributeEdit() && doValidationOperandEdit())) {
        __doPostBack(attrEdit_RiskScore_uxUpdate_ClientID, '');
    }
    return false;
}



function doValidOperandEditCombobox() {
    clearErrorEdit();
    if ((doValidationAttributeEdit() && doValidationOperandEdit() && doValidatorMetricComboboxEdit())) {
        __doPostBack(attrEdit_RiskScore_uxUpdate_ClientID, '');
    }
    return false;
}

function doValidOperandEditModal() {
    clearErrorEdit();
    if ((doValidationAttributeEdit() && doValidationOperandEdit() && doValidatorMetricModalEdit())) {
        __doPostBack(attrEdit_RiskScore_uxUpdate_ClientID, '');
    }
    return false;
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
    if (parseInt(to.get_value()) <= parseInt(from.get_value())) {
        $('label[for=' + uxFromEdit_ClientID + ']').addClass('label-error');
        return false;
    }
    else
        return true;
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
