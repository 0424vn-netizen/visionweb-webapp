function ajaxRequestStart(sender, args) {
    UxExporter_OnRequestStart(sender, args);
}
function ajaxOnResponseEnd(sender, args) {
    UxExporter_OnResponseEnd(sender, args);
}
function uxFilterStatus_Checked() {
    document.getElementById(uxChangeFilterStatus_ClientID).click();
}



function AddGroup(btn) {
    var groupControl = document.getElementById(uxAddGroupText_ClientID);
    var descriptionControl = document.getElementById(uxAddDescription_ClientID);
    if (!ValidateDataOnControls(groupControl, descriptionControl)) {
        return false;
    }
    __doPostBack(btn.name, '');
    return true;
}

function DefaultEnterOnTextBox(e) {
    var isEnter = false;
    var isIE = /MSIE/.test(navigator.userAgent);
    var keyCode = isIE ? e.keyCode : e.which;
    isEnter = (keyCode == 13);
    if (isEnter) {
        if (!isIncludeSpecialCharacters(document.getElementById(uxAddGroupText_ClientID))) {
            document.getElementById(uxAddGroup_ClientID).focus();
            document.getElementById(uxAddGroup_ClientID).click();
        } else {
            document.getElementById(uxAddDescription_ClientID).focus();
        }
        
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
function UpdateGroup(groupControlID, descriptionControlID) {
    var groupControl = document.getElementById(groupControlID);
    var descriptionControl = document.getElementById(descriptionControlID);
    if (!ValidateDataOnControls(groupControl, descriptionControl)) {
        return false;
    }
    return true;
}



function ActivateDeactivate(statusID, isActive, deleteAllowed, params) {
    /*
    if (isActive && !deleteAllowed)
        ShowPopupModal("rm_Group_ActiveModal.aspx?" + params, 'auto');
    else
    */
        UpdateIsActive(statusID, isActive);
}



function UpdateIsActive(statusID, isActive) {
    $get(uxActivateDeactivateData_ClientID).value = statusID + ";" + isActive;
    $get(uxActivateDeactivate_ClientID).click();
}

function ShowMemberList(params) {
    ShowPopupModal("rm_MCF_Group_MemberModal.aspx?" + params,'auto');
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
    if ($.trim($(ele).parents('tr').find('.txtEditGroupName').val()).length == 0) {
        alert(rm_GroupMaintenance_aspx_cs_GroupName+' ' + Resources_ValMsg_Required);
        return false;
    }
    else if (!reggroupName.test($.trim($(ele).parents('tr').find('.txtEditGroupName').val()))) {
        alert(rm_GroupMaintenance_aspx_cs_GroupName + ' ' + Resources_ValMsg_InvalidCharacter);
        return false;
    }
    else if ($.trim($(ele).parents('tr').find('.txtEditGroupDescription').val()).length > 500 || $.trim($(ele).parents('tr').find('.txtEditGroupDescription').val()).indexOf('<') != -1 || $.trim($(ele).parents('tr').find('.txtEditGroupDescription').val()).indexOf('>') != -1) {
        alert(rm_GroupMaintenance_aspx_cs_Description+' ' + Resources_ValMsg_InvalidCharacter);
        return false;
    }
    else {
        return true;
    }
}

function validBeforeSubmitAdd() {
    if (isDisabledSubmitAdd) {
        isDisabledSubmitAdd = false;
        return false;
    } else {
        return ValidateInput(); 
    }
}

var isDisabledSubmitAdd = false;
function addCheckSpecialCharacters() {
    $('#' + uxAddGroupText_ClientID).change(function (e) {
        if (isIncludeSpecialCharacters(this)) {
            isDisabledSubmitAdd = true;
            ValidateAddGroupText();
            removeSpecialCharacters(this);
            this.focus();
            setTimeout('isDisabledSubmitAdd = false;', 500);
        }
    });
    $('#' + uxAddDescription_ClientID).change(function (e) {
        if (isIncludeSpecialCharacters(this)) {
            isDisabledSubmitAdd = true;
            ValidateAddDescription();
            removeSpecialCharacters(this);
            this.focus();
            setTimeout('isDisabledSubmitAdd = false;', 500);
        }
    });
    // edit Group Maintenance
    $(".txtEditGroupName").each(function () {
        $(this).change(function (e) {
            if (isIncludeSpecialCharacters(this)) {
                alert(rm_GroupMaintenance_aspx_cs_GroupName + ' ' + Resources_ValMsg_InvalidCharacter);
                removeSpecialCharacters(this);
                this.focus();
            }
        })
    });
    $(".txtEditGroupDescription").each(function () {
        $(this).change(function (e) {
            if (isIncludeSpecialCharacters(this)) {
                alert(rm_GroupMaintenance_aspx_cs_Description + ' ' + Resources_ValMsg_InvalidCharacter);
                removeSpecialCharacters(this);
                this.focus();
            }
        })
    });
}

$(document).ready(function () {
    addCheckSpecialCharacters();
});