
function ConfirmDelete() {
    if (!confirm(messageConfirm)) {
        return false;
    }
}

function OnKeyPress(sender, args) {
    var keycode = args.get_keyCode();
    if (keycode == 8) {
        return;
    }
    else {
        var text = sender.get_value() + args.get_keyCharacter();
        if (!text.match('^[0-9]+$')) {
            args.set_cancel(true);
        }
    }

}

function ValidateDataAttribute() {
    var isValid = true;
    var grid = $find(client_Grid_Attribute_Id);
    var masterView = grid.get_masterTableView();
    var rows = masterView.get_dataItems();
    for (var i = 0; i < rows.length; i++) {
        var row = rows[i];
        var cbAttr = row.findControl("uxAttributeList");
        var cbOperand = row.findControl("uxOperand");
        var cbMetric = row.findControl("uxMetric");
        // Text box from
        var txtTo = $(row.get_element()).find("input[id*='uxAttributeTo']").get(0);
        var txtFrom = $(row.get_element()).find("input[id*='uxAttributeFrom']").get(0);
        var fromToMinValue = $(row.get_element()).find("input[id*='hddMinValueFromTo']").get(0).value;
        var fromToMaxValue = $(row.get_element()).find("input[id*='hddMaxValueFromTo']").get(0).value;
        var isFromToCustom = fromToMinValue != "" && fromToMaxValue != "";
        //
        var txtMetric = $(row.get_element()).find("input[id*='uxMetricTextList']").get(0);

        //fromto Metric
        var txtFromMetric = $(row.get_element()).find("input[id*='uxMetricFrom']").get(0);
        var txtToMetric = $(row.get_element()).find("input[id*='uxMetricTo']").get(0);
        var metricMinValue = $(row.get_element()).find("input[id*='hddMinValueMetric']").get(0).value;
        var metricMaxValue = $(row.get_element()).find("input[id*='hddMaxValueMetric']").get(0).value;

        if (!txtFrom.disabled) {
            if (txtFrom.value === "") {
                //show message
                ShowErrorMessage(txtFrom, String.format(messageRequired, messageFrom), false);
                isValid = false;
            } else if (!isInteger(txtFrom.value)) {
                ShowErrorMessage(txtFrom, String.format(messageFromNumber), false);
                isValid = false;
            } else {
                HideErroMessage(txtFrom, false);
            }
        }
        if (!txtTo.disabled) {
            if (txtTo.value === "") {
                //show message
                ShowErrorMessage(txtTo, String.format(messageRequired, messageTo), false);
                isValid = false;
            } else if (!isInteger(txtTo.value)) {
                ShowErrorMessage(txtTo, String.format(messageToNumber), false);
                isValid = false;
            }
            else if (!txtFrom.disabled) {
                if (isFromToCustom) {
                    if (Number(txtFrom.value) > Number(txtTo.value)) {
                        //show message
                        ShowErrorMessage(txtTo, messageToMustGreaterThanorEqualFrom, false);
                        isValid = false;
                    }
                    else {
                        HideErroMessage(txtTo, false);
                    }
                }
                else {
                    if (Number(txtFrom.value) >= Number(txtTo.value)) {
                        //show message
                        ShowErrorMessage(txtTo, messageToMustGreaterThanFrom, false);
                        isValid = false;
                    }
                    else {
                        HideErroMessage(txtTo, false);
                    }
                }
            }
        }


        if (cbOperand.get_enabled()) {
            if (cbOperand.get_selectedItem().get_value() == "-1") {
                //show message
                ShowErrorMessage(cbOperand, String.format(messageRequired, messageOperand), true);
                isValid = false;

            } else {
                HideErroMessage(cbOperand, true);
            }
        }
        if (cbMetric != null && cbMetric.get_enabled()) {
            if (cbOperand.get_selectedItem().get_text() == "=") {
                // Check multi select
                var items = cbMetric.get_checkedItems();
                var itemSS = cbMetric.get_selectedItem() != null ? cbMetric.get_selectedItem().get_value() : "-9";
                if (items.length <= 0 && itemSS == "-9") {
                    //show message
                    ShowErrorMessage(cbMetric, String.format(messageRequired, messageMetric), true);
                    isValid = false;
                } else {
                    HideErroMessage(cbMetric, true);
                }

            } else if (!cbMetric.get_selectedItem() || cbMetric.get_selectedItem().get_value() == "-1") {
                ShowErrorMessage(cbMetric, String.format(messageRequired, messageMetric), true);
                isValid = false;
            } else {
                HideErroMessage(cbMetric, true);
            }
        }
        else if (txtFromMetric != undefined) {
            if (txtFromMetric.value === "") {
                //show message
                if (txtToMetric != undefined)
                    ShowErrorMessage(txtFromMetric, String.format(messageRequired, messageFrom), false);
                else {
                    $($(txtFromMetric).closest('td').find("label")[0]).show();
                    $($(txtFromMetric).closest('td').find("label")[0]).html(String.format(messageRequired, messageMetric));
                }
                isValid = false;
            } else if (!isInteger(txtFromMetric.value) || parseInt(txtFromMetric.value) < 0 || parseInt(txtFromMetric.value) > metricMaxValue) {
                if (txtToMetric != undefined)
                    ShowErrorMessage(txtFromMetric, messageMetric_From.replace("[minvalue]", metricMinValue).replace("[maxvalue]", metricMaxValue), false);
                else {
                    $($(txtFromMetric).closest('td').find("label")[0]).show();
                    $($(txtFromMetric).closest('td').find("label")[0]).html(messageMetric_3.replace("[minvalue]", metricMinValue).replace("[maxvalue]", metricMaxValue));
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
                if (txtToMetric.value === "") {
                    //show message
                    ShowErrorMessage(txtToMetric, String.format(messageRequired, messageTo), false);
                    isValid = false;
                } else if (!isInteger(txtToMetric.value) || parseInt(txtToMetric.value) < 0 || parseInt(txtToMetric.value) > metricMaxValue) {
                    ShowErrorMessage(txtToMetric, String.format(messageMetric_To.replace("[minvalue]", metricMinValue).replace("[maxvalue]", metricMaxValue)), false);
                    isValid = false;
                }
                else if (txtFromMetric != undefined && (Number(txtFromMetric.value)) > Number(txtToMetric.value)) {
                    //show message
                    ShowErrorMessage(txtToMetric, messageToMustGreaterThanorEqualFrom, false);
                    isValid = false;
                }
                else {
                    HideErroMessage(txtToMetric, false);
                }
            }
        }
        else {
            if (txtMetric != null && txtMetric.value === "") {
                //show message
                ShowErrorMessage(txtMetric, String.format(messageRequired, messageMetric), false);
                isValid = false;
            } else {

                HideErroMessage(txtMetric, false);
            }
        }
    }

    return isValid;
}

function ShowErrorMessage(control, message, isInput) {
    if (isInput) {
        $("label[for='" + control.get_id() + "_Input']").show();
        $("label[for='" + control.get_id() + "_Input']").html(message);
    } else {
        $("label[for='" + control.id + "']").show();
        $("label[for='" + control.id + "']").html(message);
    }

}

function HideErroMessage(control, isInput) {
    if (control != null) {
        if (isInput) {
            $("label[for='" + control.get_id() + "_Input']").hide();
        } else {
            $("label[for='" + control.id + "']").hide();
        }
    }
}

function CheckDuplicateAttribute() {
    var isValid = true;
    var grid = $find(client_Grid_Attribute_Id);
    var masterView = grid.get_masterTableView();
    var rows = masterView.get_dataItems();
    if (rows.length == 0)
        return true;
    for (var i = 0; i < rows.length; i++) {
        var row = rows[i];
        var cbAttr = row.findControl("uxAttributeList");
        HideErroMessage(cbAttr, true);
    }
    for (var i = 0; i < (rows.length - 1); i++) {
        var row = rows[i];
        var cbAttr = row.findControl("uxAttributeList");
        if (!!cbAttr.get_selectedItem() && cbAttr.get_selectedItem().get_value() != "-1") {
            for (var j = i + 1; j < rows.length; j++) {
                var row2 = rows[j];
                var cbAttr2 = row2.findControl("uxAttributeList");
                if (!!cbAttr2.get_selectedItem() && cbAttr2.get_selectedItem().get_value() != "-1") {
                    if (cbAttr.get_selectedItem().get_value() == cbAttr2.get_selectedItem().get_value()) {
                        ShowErrorMessage(cbAttr2, messageDuplidate, true);
                        isValid = false;
                        break;
                    }
                }

            }
        }
    }

    return isValid;
}

function CheckAtleastOneAttribute() {
    var isValid = false;
    var grid = $find(client_Grid_Attribute_Id);
    var masterView = grid.get_masterTableView();
    var rows = masterView.get_dataItems();
    if (rows.length == 0)
        return false;
    for (var i = 0; i < rows.length; i++) {
        var row = rows[i];
        var cbAttr = row.findControl("uxAttributeList");

        if (!!cbAttr.get_selectedItem() && cbAttr.get_selectedItem().get_value() != "-1") {
            isValid = true;
            break;
        }
    }

    return isValid;
}

function GetSelectedMetric(controlID) {
    return $('#' + controlID).val();
}
function SetSelectedMetric(controlID, val) {
    $('#' + controlID).val(val);
    var ctr = $('#' + controlID).parents('td').find("input[id*='uxMetricTextList'][type='text']");
    if (val.split(';').length > 2) {
        var count = val.split(';').length;
        $(ctr).val(count + ' ' + msgitemsSelected);
    }
    else {
        $(ctr).val(val);
    }
}

function InputOnTextBox(e) {
    var isIE = /MSIE/.test(navigator.userAgent);
    var keyCode = isIE ? e.keyCode : e.which;
    if (keyCode > 31 && (keyCode < 48 || keyCode > 57))
        return false;

    return true;
}


function MetricClassificationItemCheck(sender, args) {
    sender.get_attributes().setAttribute("MetricText", sender.get_text());
}

function MetricClassificationOnBlur(sender, args) {
    var metricText = sender.get_attributes().getAttribute("MetricText");
    if (metricText) {
        sender.set_text(metricText);
    } else {
        sender.set_text("");
    }
}

function checkInputCharacter(e) {
    var val = e.value;
    if (isNaN(val))
        e.value = "";
}
