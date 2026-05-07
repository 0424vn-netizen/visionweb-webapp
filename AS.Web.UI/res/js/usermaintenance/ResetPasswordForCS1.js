function closeMe(reload) {
    if (reload) parent.doRebindUserList();
    return parent.HidePopupModal();
}
$(function () {
    setTimeout(function () {
        $('#' + uxEmail_ClientID).focus();
    }, 500);
    $('#' + uxEmail_ClientID).select();
    $('#' + uxModalContainer_ClientID + ' input[type=text]').keypress(function (e) {
        var code = e.keyCode || e.which;
        if (code == 13) {
            $('#' + uxContinue_ClientID).click();
            return false;
        }
    });
});

function ValidateData() {
    if (isDisabledSubmitAdd) {
        return false;
    }
    else if (ValidateInput()) {
        return true;
    }
    else {
        AdjustModalSize();
        return false;
    }
}

var isDisabledSubmitAdd = false;
function addCheckSpecialCharacters() {
    $('#' + uxEmail_ClientID).change(function (e) {
        if (ValidateInput() == false || isIncludeSpecialCharacters(this)) {
            isDisabledSubmitAdd = true;
            removeSpecialCharacters(this);
            this.focus();
            setTimeout('isDisabledSubmitAdd = false;', 500);
        }
    });
}

$(document).ready(function () {
    addCheckSpecialCharacters();
});