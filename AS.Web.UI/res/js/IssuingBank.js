function checkSpecialCharacter(val) {
    if (!!val) {
        //var reg = /^[ ,.A-Za-z0-9]*$/;
        var reg = new RegExp("^((?!(<[^ \t]))(?!(&#)).)*$");
        if (reg.test(val) == false) {
            return false;
        }
    }
    return true;
}

function validateInputValue() {
    var data = document.getElementById(uxBinNumber_ID).value;
    if (!checkSpecialCharacter(data)) {
        alert(binNumberSpecialResource)
        return false;
    }

    if (data == '' || data.trim() == '' || (data.length != 6 && data.length != 8) || !IsNumeric(data)) {
        alert(binNumberRequired)
        return false;
    }

    return true;
}
function addCheckSpecialCharacters() {
    $('#' + uxBinNumber_ID).change(function (e) {
        if (isIncludeSpecialCharacters(this)) {
            validateInputValue();
            removeSpecialCharacters(this);
        }
    })
}

$(document).ready(function () {
        $('#' + uxBinNumber_ID).attr('placeholder', first6_Or8);
        addCheckSpecialCharacters();
})