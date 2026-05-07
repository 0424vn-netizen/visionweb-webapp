function validateSubmit() {
    var isValid = ValidaInput();
    if (!isValid) {
        AdjustModalSize()
        return false;
    }

    return true;
}

function DoClose() {
    try {
        parent.HidePopupModal();
        if (parent != null) {
            parent.doPostBack();
        }
    }
    catch (err) { }
}

function validateGroupSpecialCharacter() {
    var userGroups = $('#' + uxUserGroup).val();
    return checkSpecialCharacter(userGroups);
}

function validateDesSpecialCharacter() {
    var description = $('#' + uxDescription).val();
    return checkSpecialCharacter(description);
}