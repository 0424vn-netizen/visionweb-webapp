function doValidation() {
    var site = document.getElementById(uxUserID_ClientID).value;
    if (!(/\s/.test(site))) {
        return true;
    }
    else {
        AdjustModalSize();
    }
    return false;
}
function validateExpression(input, valid_expre) {

    var m = input.match(new RegExp(valid_expre, 'ig'));
    if (m != null) {
        return m.length == input.length;
    }
    return false;
}