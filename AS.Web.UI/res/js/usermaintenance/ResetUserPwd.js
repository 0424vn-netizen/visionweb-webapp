function CheckUsername() {
    var objUsername =  document.getElementById(txtUsername_ClientID).value.trim(); 
    if (objUsername.length < 1 || objUsername.length > 50) {
        return false;
    }

    if (IsSpecialCharacter1(objUsername)) {
        return false;
    }
    return true;
}
function IsSpecialCharacter1(sText) {
    return coreHasInvalidCharacters(sText, '`~!@#$%^&*()+=|\\{[}]:;"\'<,>.?/ ');
}