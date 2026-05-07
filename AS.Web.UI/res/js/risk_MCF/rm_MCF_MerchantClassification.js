function ajaxRequestStart(sender, args) {
    UxExporter_OnRequestStart(sender, args);
}
function ajaxOnResponseEnd(sender, args) {
    UxExporter_OnResponseEnd(sender, args);
}
function uxFilterStatus_Checked() {
    document.getElementById(rm_Btn_Search).click();
}

function SearchEnterOnTextbox(e) {
    var isEnter = false;
    var isIE = /MSIE/.test(navigator.userAgent);
    var keyCode = isIE ? e.keyCode : e.which;
    isEnter = (keyCode == 13);
    if (isEnter) {
        document.getElementById(rm_Txt_Classification_Name).focus();
        setTimeout('doClick()', 100);
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

//Search when user focus on textbox
function doClick() {
    document.getElementById(rm_Btn_Search).click();
}

function CheckClassificationInUse(classId) {

    var result = false;
    var actionUrl = rootURL + "Risk_MCF/rm_MCF_MerchantClassificationConfiguration.aspx/CheckClassificationInUsing";
    $.ajax({
        type: "POST",
        async: false,
        url: actionUrl,
        data: '{"classId":"' + classId + '"}',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            if (msg.d[0] == 'true') {
                result = true;
            }
            else {
                alert(String.format(rm_Message_Classification_InUse, msg.d[1]));
            }
        }
    });
    return result;
}

function ActivateDeactivate(statusID, isActive) {

    if (!isActive) {
        if (CheckClassificationInUse(statusID)) {
            UpdateIsActive(statusID, isActive);
        }
    }
    else {
        UpdateIsActive(statusID, isActive);
    }

}

function UpdateIsActive(statusID, isActive) {
    $get(rm_MerchantClassification_uxActivateDeactivateData).value = statusID + ";" + isActive;
    $get(rm_MerchantClassification_uxActivateDeactivate).click();
}

function OnClientItemChecked(sender, args) {
    var item = args.get_item();
    var checkedItems = sender.get_checkedItems();
    if (checkedItems.length <= 0) {
        sender.set_emptyMessage(rm_Empty_Message_Attributes);
    }
}

function RebindGrid() {

    HidePopupModal();
    document.getElementById(rm_Btn_Rebind_ClientId).click();
}


function GetSelectedMetric(controlID) {
    var wind = GetPopupModal(0);
    var val = wind.GetContentFrame().contentWindow.GetSelectedMetric(controlID);
    return val;
}
function SetSelectedMetric(controlID, val) {
    var wind = GetPopupModal(0);
    var val = wind.GetContentFrame().contentWindow.SetSelectedMetric(controlID, val);
}

$(document).ready(function () {
    addCheckSpecialCharacters();
});

function addCheckSpecialCharacters() {
    $("#" + rm_Txt_Classification_Name).change(function (e) {
        if (isIncludeSpecialCharacters(this)) {
            ValidateInputFilter();
            removeSpecialCharacters(this);
        }
    });
    // InputFilter of Attribute combobox
    $("#" + rm_Atributes_ClientID + "_Input").change(function (e) {
        if (isIncludeSpecialCharacters(this)) {
            var newValue = removeSpecialCharacters(this);
            var radCombobox = $find(rm_Atributes_ClientID);
            radCombobox.set_text(newValue);
        }
    });
}