
function ShowHideAltRole(op, op2) {
    if (op && op2) {
        if ($('#' + uxPlcPCIRole_table_ClientID).length > 0)
            $('#Div1').show();
        else {
            $('#Div1').hide();
        }
    } else {
        $('#Div1').hide();
    }

    if (op) {
        $('#pnlAltRole').show();
        //if (op2) {
        //    $('#pnlAltRole table').addClass("max-width");
        //} else {
        //    $('#pnlAltRole table').removeClass("max-width");
        //}
    } else {
        if (!op2) {
            $('#pnlAltRole').hide();
        }
    }
}

$(document).ready(
    function () {
        if ($('#' + uxPlcPCIRole_table_ClientID).length > 0 && $('#tb1099KRole').length > 0) {
            ShowHideAltRole(true, true);
        }
        else if ($('#' + uxPlcPCIRole_table_ClientID).length > 0 || $('#tb1099KRole').length > 0) {
            $('#pnlAltRole').show();
        }
        else {
            $('#pnlAltRole').hide();
        }
    });