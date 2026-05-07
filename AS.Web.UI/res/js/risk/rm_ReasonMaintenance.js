function ajaxRequestStart(sender, args) {
    UxExporter_OnRequestStart(sender, args);
}
function ajaxOnResponseEnd(sender, args) {
    UxExporter_OnResponseEnd(sender, args);
}
function uxFilterStatus_Checked() {
    document.getElementById(rm_ReasonMaintenance_uxChangeFilterStatus).click();
}

function DefaultEnterOnTextBoxEscalation(e) {
    var isEnter = false;
    var isIE = /MSIE/.test(navigator.userAgent);
    var keyCode = isIE ? e.keyCode : e.which;
    isEnter = (keyCode == 13);
    if (isEnter) {
        $get(rm_ReasonMaintenance_uxAddEscalation).focus();
        $get(rm_ReasonMaintenance_uxAddEscalation).click();
        return false;
    }
}

function DefaultEnterOnDiv(e) {
    var isEnter = false;
    var isIE = /MSIE/.test(navigator.userAgent);
    var keyCode = isIE ? e.keyCode : e.which;
    isEnter = (keyCode == 13);
    if (isEnter) {
        return false;
    }
}
function ShowMsg(msg) {
    alert(msg);
}

function ActivateDeactivate(statusID, isActive) {
    UpdateIsActive(statusID, isActive);
}

function UpdateIsActive(statusID, isActive) {
    $get(rm_ReasonMaintenance_uxActivateDeactivateData).value = statusID + ";" + isActive;
    $get(rm_ReasonMaintenance_uxActivateDeactivate).click();
}

//Search when user focus on textbox
function doClick(btnID) {
    document.getElementById(btnID).click();
}
function SearchEnterOnTextbox(e, btnID) {
    var isEnter = false;
    var isIE = /MSIE/.test(navigator.userAgent);
    var keyCode = isIE ? e.keyCode : e.which;
    isEnter = (keyCode == 13);
    if (isEnter) {
        document.getElementById(btnID).focus();
        setTimeout("doClick('" + btnID + "')", 100);
        return false;
    }

}
function HideCreatePanel(id) {
    if (document.getElementById(id) != null) {
        document.getElementById(id).style.display = "none";
    }
}

function doValidInput(ele) {
    if ($.trim($(ele).parents('tr').find('.txtEditReasonName').val()).length == 0) {
        alert(rm_ReasonMaintenance_js_Reason + ' ' + Resources_ValMsg_Required);
        return false;
    }
    else if ($.trim($(ele).parents('tr').find('.txtEditReasonName').val()).length > 100) {
        alert(String.format(rm_ReasonMaintenance_js_Reason + ' ' + Resources_ValMsg_MaxLength, 100));
        return false;
    }
    else if ($.trim($(ele).parents('tr').find('.txtEditReasonName').val()).indexOf('<') != -1 || $.trim($(ele).parents('tr').find('.txtEditReasonName').val()).indexOf('>') != -1) {
        alert(rm_ReasonMaintenance_js_Reason+' ' + Resources_ValMsg_InvalidCharacter);
        return false;
    }
    else {
        return true;
    }
}