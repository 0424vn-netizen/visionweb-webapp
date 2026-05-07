
function ValidateData() {

    var isValid = ValidateInput() & ValidateClassificationName();
    isValid &= ValidateDataAttribute() & CheckDuplicateAttribute();
    return isValid;
}

function ValidateExistingClassificationName() {
    var classId = document.getElementById(rm_ClassificationID);
    var className = document.getElementById(rm_ClassificationName);

    var result = false;
    var actionUrl = rootURL + "Risk_MCF/rm_MCF_MerchantClassification_CreateNewModal.aspx/CheckDuplicateClassificationName";
    $.ajax({
        type: "POST",
        async: false,
        url: actionUrl,
        data: '{"classID":"' + classId.value + '","name":"' + className.value + '"}',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            if (msg.d == 'true') {
                result = true;
            }
        }
    });
    return result;
}

function ValidateClassificationName() {

    var name = $get(rm_ClassificationName).value;

    if (name.trim() == rm_Classification_Hint || name.trim() == '') {
        return false;
    } else
        return true;
}

function ValidateOnlyNumber() {
    var multiplier = document.getElementById(rm_Multiplier);
    var reg = /^[0-9]+(\.[0-9])?$/;

    return reg.test(multiplier.value);
}
//Check special character
function ValidateSpecialCharacters() {
    var classificationName = document.getElementById(rm_ClassificationName);
    var classificationNameValue = trim(classificationName.value);

    var valid_chars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz_- ";
    var validHexValue = '';
    for (var i = 0; i < valid_chars.length; i++) {
        validHexValue += '\\x' + valid_chars.charCodeAt(i).toString(16).toUpperCase();
    }
    var m = classificationNameValue.match(new RegExp('[' + validHexValue + ']', 'ig'));
    if (m != null) {
        return m.length == classificationNameValue.length;
    }
    return false;
}

$(document).ready(function () {
    addCheckSpecialCharacters();
});

function addCheckSpecialCharacters() {
    $("#" + rm_ClassificationName).change(function (e) {
        if (isIncludeSpecialCharacters(this)) {
            ValidateInputClassificationName();
            removeSpecialCharacters(this);
        }
    });
    $("#" + rm_Multiplier).change(function (e) {
        if (isIncludeSpecialCharacters(this)) {
            ValidateMultiplier();
            removeSpecialCharacters(this);
        }
    });
}