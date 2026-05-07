function closeMe(reload) {
    if (reload) parent.doRebindUserList();
    return parent.HidePopupModal();
}
function checkOrClearAllCheckboxAccessFunction(containerId, checked) {
    var checkboxes = document.getElementById(containerId).getElementsByTagName('input');
    for (var i = 0; i < checkboxes.length; i++) {
        if (checkboxes[i].type.toLowerCase() == 'checkbox') {
            if (!checkboxes[i].disabled)
                checkboxes[i].checked = checked;
        }
    }

}

var isDisabledSubmitAdd = false;
function addCheckSpecialCharacters() {
    $('#' + uxUsernameId).change(function (e) {
        if (isIncludeSpecialCharacters(this)) {
            isDisabledSubmitAdd = true;
            ValidateUsername();
            removeSpecialCharacters(this);
            setTimeout('isDisabledSubmitAdd = false;', 500);
        }
    });
    // First Name
    $('#' + uxFirstNameId).change(function (e) {
        if (isIncludeSpecialCharacters(this)) {
            isDisabledSubmitAdd = true;
            ValidateFirstName();
            removeSpecialCharacters(this);
            setTimeout('isDisabledSubmitAdd = false;', 500);
        }
    });
    // Last Name
    $('#' + uxLastNameId).change(function (e) {
        if (isIncludeSpecialCharacters(this)) {
            isDisabledSubmitAdd = true;
            ValidateLastName();
            removeSpecialCharacters(this);
            setTimeout('isDisabledSubmitAdd = false;', 500);
        }
    });
    // Email
    $('#' + uxEmailId).change(function (e) {
        if (isIncludeSpecialCharacters(this)) {
            isDisabledSubmitAdd = true;
            ValidateEmail();
            removeSpecialCharacters(this);
            setTimeout('isDisabledSubmitAdd = false;', 500);
        }
    });
}

$(document).ready(function () {
    addCheckSpecialCharacters();
});
 