function LoadReassignList(combo, eventarqs) {
    document.getElementById(RiskReassignUser_uxUpdate_ClientID).disabled = true;
    var reassignList = $find(RiskReassignUser_uxReassignList_ClientID);
    reassignList.set_text(" ");
    setTimeout("document.getElementById(RiskReassignUser_uxUpdate_ClientID).disabled= false;", 300);
}
var uxSubmit = document.getElementById(RiskReassignUser_uxUpdate_ClientID);
function DefaultEnterOnTextBox(e) {

    var isEnter = false;
    var isIE = /MSIE/.test(navigator.userAgent);
    var keyCode = isIE ? e.keyCode : e.which;
    isEnter = (keyCode == 13);
    if (isEnter) {
        if (isIncludeSpecialCharacters(e.currentTarget)) {
            return false;
        }
        if (isIE)
            setTimeout("uxSubmit.click()", 200);
        else {
            uxSubmit.focus();
            uxSubmit.click();
        }
        return false;
    }
}
function ValidateData(sDate, eDate, user, reassign, mode) {
    var reassignObj = $find(reassign);
    var reassignText = trim(reassignObj.get_text());
    var userText;

    var isValidUserList = true;
    var isValidOther;

    if (mode != 'update') {
        // RadCombobox
        var userObj = $find(user);
        userText = trim(userObj.get_text());
        isValidUserList = doValidationUserListCreateMode();
    }
    else {
        // Label
        var userObj = document.getElementById(user);
        userText = userObj.innerText;
        isValidUserList = doValidationUserListUpdateMode();
    }
    isValidOther = doValidation(false);

    if (isValidOther && isValidUserList) {
        var msg = rm_TempReassignUser_ReassignUser_ConfirmAssign;
        msg = msg.replace('<User Name>', userText);
        msg = msg.replace('<Reassign To>', reassignText);
        msg = msg.replace('<From>', document.getElementById(sDate + '_dateInput').value);
        msg = msg.replace('<To>', document.getElementById(eDate + '_dateInput').value);
        return confirm(msg);
    }
    else {
        //cheat to show correct layout for error message belong to "To" textbox
        var errToDate = $("label[for='" + RiskReassignUser_uxEDate_ClientID + "'].error").text();
        if (errToDate != "") {
            $("#uxEDateErrMsgContainer").addClass("height-20");
        }
        return false;
    }
}

function addCheckSpecialCharacters() {
    $('#' + RiskReassignUser_uxSDate_ClientID + '_dateInput').change(function (e) {
        if (isIncludeSpecialCharacters(this)) {
            doValidationStartDate();
            removeSpecialCharacters(this);
        }
    });
    $('#' + RiskReassignUser_uxEDate_ClientID + '_dateInput').change(function (e) {
        if (isIncludeSpecialCharacters(this)) {
            doValidationEndDate();
            removeSpecialCharacters(this);
            //cheat to show correct layout for error message belong to "To" textbox
            var errToDate = $("label[for='" + RiskReassignUser_uxEDate_ClientID + "'].error").text();
            if (errToDate != "") {
                $("#uxEDateErrMsgContainer").addClass("height-20");
            }
        }
    });
}

$(document).ready(function () {
    addCheckSpecialCharacters();
});