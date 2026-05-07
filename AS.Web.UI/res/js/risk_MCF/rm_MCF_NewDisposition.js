function ValidateSpecialCharacters() {
    var dispositionName = document.getElementById(uxDispositionName_ClientID);
    var dispositionNameValue = trim(dispositionName.value);
    var reg = new RegExp("^((?!(<[^ \t]))(?!(&#)).)*$");
    if (reg.test(dispositionNameValue) == true) {
        return true;
    }
    return false;
}

function submitClick() {
    if (!ValidateInput()) {
        AdjustModalSize();
        return false;
    };
    return true;
}