function ajaxRequestStart(sender, args) {
    UxExporter_OnRequestStart(sender, args);
}
function ajaxOnResponseEnd(sender, args) {
    UxExporter_OnResponseEnd(sender, args);
}
function uxFilterStatus_Checked() {
    document.getElementById(uxChangeFilterStatus_ClientID).click();
}

function AddResolution(btn) {
    var statusControl = document.getElementById(uxAddResolutionText_ClientID);
    if (!ValidateDataOnControls(statusControl)) {
        return false;
    }
    __doPostBack(btn.name, '');
    return true;
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



function DefaultEnterOnTextBoxResolution(e) {
    var isEnter = false;
    var isIE = /MSIE/.test(navigator.userAgent);
    var keyCode = isIE ? e.keyCode : e.which;
    isEnter = (keyCode == 13);

    if (isEnter) {
        $get(uxAddResolution_ClientID).focus();
        $get(uxAddResolution_ClientID).click();
        return false;
    }
}
function ShowMsg(msg) {
    alert(msg);
}
function UpdateResolution(statusControlID) {
    var statusControl = document.getElementById(statusControlID);
    if (!ValidateDataOnControls(statusControl)) {
        return false;
    }
    return true;
}


function ActivateDeactivate(statusID, isActive) {
    if (isActive) // need to check before deactivating
        PageMethods.GetEscalationCountByResolution(statusID, isActive, GetEscalationCountByResolution_Callback);
    else
        UpdateIsActive(statusID, isActive);
}

function GetEscalationCountByResolution_Callback(result) {
    if (result[0] == 0) //no active Resolution
        UpdateIsActive(result[1], result[2]);
    else
        ShowPopupModal("rm_Resolution_ActiveModal.aspx?" + result[1], 'auto');
}


function UpdateIsActive(statusID, isActive) {
    $get(uxActivateDeactivateData_ClientID).value = statusID + ";" + isActive;
    $get(uxActivateDeactivate_ClientID).click();
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

// Validate when editing
function doValidInput(ele) {
    if ($.trim($(ele).parents('tr').find('.txtEditResolution').val()).length == 0) {
        alert(rm_ResolutionMaintenance_js_Resolution + ' ' + Resources_ValMsg_Required);
        return false;
    }
    else if ($.trim($(ele).parents('tr').find('.txtEditResolution').val()).indexOf('<') != -1 || $.trim($(ele).parents('tr').find('.txtEditResolution').val()).indexOf('>') != -1) {
        alert(rm_ResolutionMaintenance_js_Resolution + ' ' + Resources_ValMsg_InvalidCharacter);
        return false;
    }
    else {
        return true;
    }
}