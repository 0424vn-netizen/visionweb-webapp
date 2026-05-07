$(function () {
    $("#table1 > tbody > tr:even").addClass("AltRow");
    $("#table1 > tbody > tr:odd").addClass("Row");
    $("#table2 > tbody > tr:even").addClass("AltRow");
    $("#table2 > tbody > tr:odd").addClass("Row");
    $("#table4 > tbody > tr:even").addClass("AltRow");
    $("#table4 > tbody > tr:odd").addClass("Row");
    maskPhone();
    CheckChangePWD();
});


function CheckChangePWD() {
    var objChangePass = document.getElementById(uxCheckChangePass_ClientID);
    if (objChangePass.checked) {
        var objPassword = $("#" + uxPassword_ClientID);
        var objConfirmPass = $("#" + uxConfirmPass_ClientID);
        var objOldPass = $("#" + uxOldPass_ClientID);
        objOldPass.removeAttr("disabled");
        objPassword.removeAttr("disabled");
        objConfirmPass.removeAttr("disabled");
    }
}

function CheckValidPassword() {
    var isValid = true;
    var msgError = "";
    var breakLine = "</br>";
    if (!$('#' + uxPassword_ClientID).MinLength(MinPasswordLen)) {
        msgError += breakLine + MinPasswordMsg;
        isValid = false;
    }

    if (!$('#' + uxPassword_ClientID).MaxLength(MaxPasswordLen)) {
        msgError += breakLine + MaxPasswordMsg;
        isValid = false;
    }

    if (!$('#' + uxPassword_ClientID).LatestContaintUpperCase(NumberOfUpperCaseCharacters)) {
        msgError += breakLine + NumberOfUpperCaseCharactersMsg;
        isValid = false;
    }
    if (!$('#' + uxPassword_ClientID).LatestContaintLowerCase(NumberOfLowerCaseCharacters)) {
        msgError += breakLine + NumberOfLowerCaseCharactersMsg;
        isValid = false;

    }

    if (!$('#' + uxPassword_ClientID).LatestContaintNumberic(NumberOfNumberic)) {
        msgError += breakLine + NumberOfNumbericMsg;
        isValid = false;
    }

    if (!$('#' + uxPassword_ClientID).CheckHTMLTag()) {
        msgError += breakLine + msgValidateHTMTag;
        isValid = false;
    }

    if (!$('#' + uxPassword_ClientID).CheckEncodeTag()) {
        msgError += breakLine + msgValidateEncodeTag;
        isValid = false;
    }

    if (!isValid) {
        document.getElementById(errorPassword).innerHTML = msgError.substring(5, msgError.length);
        $('#' + uxPassword_ClientID).focus();
    }
    else {
        document.getElementById(errorPassword).innerHTML = "";
    }

    return isValid;
}

function CheckValidPasswordOnBlur() {
    var isValid = true;
    var breakLine = "</br>";
    var msgError = "";
    if (!$('#' + uxPassword_ClientID).CheckHTMLTag()) {
        msgError += breakLine + msgValidateHTMTag;
        isValid = false;
    }

    if (!$('#' + uxPassword_ClientID).CheckEncodeTag()) {
        msgError += breakLine + msgValidateEncodeTag;
        isValid = false;
    }

    if (!isValid) {
        document.getElementById(errorPassword).innerHTML = msgError.substring(5, msgError.length);
        $('#' + uxPassword_ClientID).focus();
    }

    return isValid;
}

function CheckNumberOfSpecialCharacters() {
    if (IsRequiredSpecialCharacters && !$('#' + uxPassword_ClientID).LatestContaintSpecialChar(NumberOfSpecialCharacters)) {
        $('#' + uxPassword_ClientID).focus();
        return false;

    }
    else {
        return true;
    }
}

function checkBoxChangePass() {
    //get CheckBox
    var checkChangePass = $("#" + uxCheckChangePass_ClientID);
    if (checkBoxChangePass != null) {
        //get TextBox (Password)
        var objPassword = $("#" + uxPassword_ClientID);
        var objConfirmPass = $("#" + uxConfirmPass_ClientID);
        var objOldPass = $("#" + uxOldPass_ClientID);

        if (checkChangePass.is(":checked")) {
            objOldPass.removeClass("aspNetDisabled");
            objPassword.removeClass("aspNetDisabled");
            objConfirmPass.removeClass("aspNetDisabled");
            objOldPass.removeAttr("disabled");
            objPassword.removeAttr("disabled");
            objConfirmPass.removeAttr("disabled")
        }
        else {
            objOldPass.value = "";
            objPassword.value = "";
            objConfirmPass.value = "";
            objOldPass.attr("disabled", "disabled");
            objPassword.attr("disabled", "disabled");
            objConfirmPass.attr("disabled", "disabled");
            objOldPass.addClass("aspNetDisabled");
            objPassword.addClass("aspNetDisabled");
            objConfirmPass.addClass("aspNetDisabled");
        }
    }
}

function checkBoxChangeQuestion() {

    var isCheck = false;
    //get CheckBox
    var checkQuestion = $("#" + uxCheckChangeQuestion_ClientID);

    //get TextBox (Answer)
    var objAnswer = $("#" + uxAnswer_ClientID);

    //get ComboBox
    var combo = $find(uxListValidQuestion_ClientID);


    if (checkQuestion != null) {

        if (checkQuestion.is(":checked")) {
            isCheck = true;
            objAnswer.removeAttr("disabled");
            combo.enable();
        }
        else {
            isCheck = false;
            objAnswer.attr("disabled", "disabled");
            combo.disable();

        }

    }

}

function checkBoxChangeUserName() {

    var isCheck = false;
    var objNewUserName = $("#" + uxNewUserName_ClientID);
    var checkChangeUser = $("#" + ckChangeUserName_ClientID);
    var objOriginalUser = $("#" + uxOriginalUserName_ClientID);

    var lbNewUserName = $(".js-lbNewUserName");
    var txtNewUserNameErrMsg = $('label[for="' + uxNewUserName_ClientID + '"]').filter('.error');

    if (checkChangeUser != null) {
        if (checkChangeUser.is(":checked")) {
            isCheck = true;
            objNewUserName.prop("readonly", false);
            objNewUserName.prop("maxlength", UserNameMaxLengthForUserProfile);
            objNewUserName.val(objOriginalUser.val());
        }
        else {
            isCheck = false;
            objNewUserName.prop("readonly", true);
            objNewUserName.val(objOriginalUser.val());
            objNewUserName.prop("maxlength", UserNameMaxLengthForUserProfile);
            lbNewUserName.removeClass("label-error");
            txtNewUserNameErrMsg.hide();
        }
    }

}

function ShowAlert(message) {
    alert(message);
}

function IsInvalidSpecialCharacters(sText) {
    return coreHasInvalidCharacters(sText, '<>');
}
function HasSpecialCharacters(sText) {
    return coreHasInvalidCharacters(sText, '`~!@#$%^&*()-_+=|\\{[}]:;"\',.?/ ');
}

function ValidateUpdateProfile() {
    if (isDisabledSubmitAdd) {
        return false;
    }
    if (isGRP_TOTAL == "False") {
        if (!ValidateInput()) {
            return false;
        }
    }

    var objChangePass = document.getElementById(uxCheckChangePass_ClientID);
    var objChangeQuestion = document.getElementById(uxCheckChangeQuestion_ClientID);
    //Change Password:
    if (objChangePass.checked) {
        if (!doValidationUserPass()) {
            return false;
        }

    }

    //Question/Answer
    if (objChangeQuestion != null && objChangeQuestion.checked) {

        if (!doValidationUserQuestion()) {
            return false;
        }
    }

    return true;
}


function EnableTextbox() {
    checkBoxChangePass();
    checkBoxChangeQuestion();
}
function HideCheckbox() {
    var checkChangePass = $("#" + uxCheckChangePass_ClientID);
    var checkQuestion = $("#" + uxCheckChangeQuestion_ClientID);

    if ($("#" + uxHddChkPass_ClientID).val().toLowerCase() == "true") {
        checkChangePass.attr("checked", "checked");
    }
    if ($("#" + uxHddChkQuestion_ClientID).val().toLowerCase() == "true") {
        checkQuestion.attr("checked", "checked");
    }
    $("#cidCheckChangePassRow").hide();
    $("#cidCheckChangeQuestionRow").hide();

    EnableTextbox();
}
function maskPhone() {
    $("input[display-masked='Phone']").mask("(000) 000-0000");
}
function validatePhone() {
    var objPhone = document.getElementById(uxPhone_ClientID);
    if (objPhone == null) return true;

    var reg = /\D?(\d{3})\D?\D?(\d{3})\D?(\d{4})$/;
    if (objPhone.value != '') {
        if (reg.test(objPhone.value))
            return true;
        return false;
    }
    return true;
}

var userProfile_regEmail = /^\w+([-+.']+\w+)*@\w+([-.]+\w+)*\.\w+([-.]\w+)*$/;
function validateEmail() {
    var objEmail = document.getElementById(uxEmail_ClientID);
    if (objEmail == null) return true;

    if (objEmail.value != '') {
        if (userProfile_regEmail.test(objEmail.value))
            return true;
        return false;
    }
    return true;
}
function validateEmailContact() {
    var objEmail = document.getElementById(uxEmailContactID);
    if (objEmail == null) return true;

    if (objEmail.value != '') {
        if (userProfile_regEmail.test(objEmail.value))
            return true;
        return false;
    }
    return true;
}
//Timezone
function onAutoTimezoneCheck() {
    var isCheck = $("#" + ckTimeZone).prop("checked");
    var dropdown = $find(drSysTimeZone);
    if (isCheck) {
        dropdown.disable();
    }
    else {
        dropdown.enable();
    }
}

//39251 – VW - Add Default Landing Page On Update My Profile Page
function DefaultLandingPageOnClientDropDownClosed(sender, args) {
    var tree_UM = $find(uxDefaultLandingPage_ClientID);
    $('#' + uxDefaultLandingPageSelectedValue_ClientID).val(tree_UM._selectedValue);
}

$(document).ready(function () {
    setTimeout(function () {
        onCheckPermissionLanding(false);
        redirectLoginpage();
    }, 500);

    addCheckSpecialCharacters();
});

var isDisabledSubmitAdd = false;
function addCheckSpecialCharacters() {
    $('input[type=text], input[type=password]').each(function () {
        $(this).change(function (e) {
            if (this.id.includes(uxNewUserName_ClientID)) {
              validationNewUserName();
            } else if (this.id.includes(uxFirstName_ClientID)) {
              validationFirstName();
            } else if (this.id.includes(uxLastName_ClientID)) {
              validationLastName();
            } else if (this.id == uxEmail_ClientID) {
              validationEmail();
            } else if (this.id == uxEmailContactID) {
              validationEmailContact();
            } else if (this.id.includes(uxOldPass_ClientID)) {
              validationOldPass();
            } else if (this.id.includes(uxPassword_ClientID)) {
              validationPassword();
            } else if (this.id.includes(uxConfirmPass_ClientID)) {
              validationConfirmPass();
            } else if (this.id.includes(uxAnswer_ClientID)) {
              validationAnswer();
            }
        
            if (isIncludeSpecialCharacters(this)) {
                isDisabledSubmitAdd = true;
                removeSpecialCharacters(this);
                this.focus();
                setTimeout('isDisabledSubmitAdd = false;', 500);
            }
        });
    });
}


function redirectLoginpage() {
    var flag = document.getElementById(uxFlagLoginLink_ID).value;
    if (flag == "1") {
        document.getElementById(uxLogoutButton_ID).click();
    }
}

var tree_Landing_nodes = null;

function onCheckPermissionLanding(refersh) {
    var tree_Landing = $find(uxDefaultLandingPage_ClientID);

    if (tree_Landing_nodes == null) {
        tree_Landing_nodes = tree_Landing.get_embeddedTree().get_allNodes();
    }

    for (var j = 0; j < tree_Landing_nodes.length; j++) {
        var nodeLanding = tree_Landing_nodes[j];

        if (nodeLanding != null) {
            if (nodeLanding._hasChildren() == true) {
                nodeLanding.set_enabled(false);
            }
            else {
                nodeLanding.set_enabled(true);
            }
        }
    }
}
