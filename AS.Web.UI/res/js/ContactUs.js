function clearComment() {
    document.getElementById(ContactUs_txtBusinessName).value = '';
    var midorVT = document.getElementById(ContactUs_txtMIDorVT);
    if (midorVT != undefined)
        midorVT.value = '';
    document.getElementById(ContactUs_txtContactName).value = '';
    document.getElementById(ContactUs_txtContactNumber).value = '';
    document.getElementById(ContactUs_uxContactEmail).value = '';
    document.getElementById(ContactUs_uxDescOfIssue).value = '';
    var arrSubjects = $find(ContactUs_uxSubjectCustom).get_items();
    if (arrSubjects.get_count() > 0) {
        arrSubjects.getItem(0).select();
    }
}

function maskContactNumber() {
    $("input[display-masked='ContactNumber']").mask("(000) 000-0000");
}

function validateContactNumber() {
    var objContactNumber = document.getElementById(ContactUs_txtContactNumber);
    if (objContactNumber == null) return true;

    var reg = /\D?(\d{3})\D?\D?(\d{3})\D?(\d{4})$/;
    if (objContactNumber.value != '') {
        if (reg.test(objContactNumber.value))
            return true;
        return false;
    }
    return true;
}

$(function () {
    maskContactNumber();
})

function ValidateContactUs() {
    if (isDisabledSubmitAdd) {
        return false;
    }
    var rs = ValidateEmailPartContent();
    if (!rs) {
        $("label[for='" + ContactUs_txtMIDorVT + "'].error").html(ContactUs_EntityDisplayName_RequiredMessage)
    }
    return rs;
}

var isDisabledSubmitAdd = false;
// Body
function onBodyBlur(element, e) {
    var ignore = e.relatedTarget || e.rangeParent;
    if (ignore == undefined) {
        if (e.originalEvent != undefined)
            ignore = e.originalEvent.explicitOriginalTarget;
    }
    if (ignore != null) {
        handleClickElement(ignore);
    }
    if (ValidateBody() == false || isIncludeSpecialCharacters(element)) {
        isDisabledSubmitAdd = true;
        removeSpecialCharacters(element);
        setTimeout('isDisabledSubmitAdd = false;', 500);
    }
}
// DescOfIssue
function onDescOfIssueBlur(element, e) {
    var ignore = e.relatedTarget || e.rangeParent;
    if (ignore == undefined) {
        if (e.originalEvent != undefined)
            ignore = e.originalEvent.explicitOriginalTarget;
    }
    if (ignore != null) {
        handleClickElement(ignore);
    }
    if (ValidateBody() == false || isIncludeSpecialCharacters(element)) {
        isDisabledSubmitAdd = true;
        removeSpecialCharacters(element);
        setTimeout('isDisabledSubmitAdd = false;', 500);
    }
}
// From Mail
function onEmailFromBlur(element, e) {
    var ignore = e.relatedTarget || e.rangeParent;
    if (ignore == undefined) {
        if (e.originalEvent != undefined)
            ignore = e.originalEvent.explicitOriginalTarget;
    }
    if (ignore != null) {
        handleClickElement(ignore);
    }
    if (ValidateEmailFrom() == false || isIncludeSpecialCharacters(element)) {
        isDisabledSubmitAdd = true;
        removeSpecialCharacters(element);
        setTimeout('isDisabledSubmitAdd = false;', 500);
    }
}
// Business Name
function onBusinessNameBlur(element, e) {
    var ignore = e.relatedTarget || e.rangeParent;
    if (ignore == undefined) {
        if (e.originalEvent != undefined)
            ignore = e.originalEvent.explicitOriginalTarget;
    }
    if (ignore != null) {
        handleClickElement(ignore);
    }
    if (ValidateBusinessName() == false || isIncludeSpecialCharacters(element)) {
        isDisabledSubmitAdd = true;
        removeSpecialCharacters(element);
        setTimeout('isDisabledSubmitAdd = false;', 500);
    }
}
// MID or VT
function onMIDorVTBlur(element, e) {
    var ignore = e.relatedTarget || e.rangeParent;
    if (ignore == undefined) {
        if (e.originalEvent != undefined)
            ignore = e.originalEvent.explicitOriginalTarget;
    }
    if (ignore != null) {
        handleClickElement(ignore);
    }
    if (ValidateMIDorVT() == false || isIncludeSpecialCharacters(element)) {
        isDisabledSubmitAdd = true;
        removeSpecialCharacters(element);
        setTimeout('isDisabledSubmitAdd = false;', 500);
    }
}
// Merchant ID
function onMerchantIDBlur(element, e) {
    var ignore = e.relatedTarget || e.rangeParent;
    if (ignore == undefined) {
        if (e.originalEvent != undefined)
            ignore = e.originalEvent.explicitOriginalTarget;
    }
    if (ignore != null) {
        handleClickElement(ignore);
    }
    if (ValidateMerchantID() == false || isIncludeSpecialCharacters(element)) {
        isDisabledSubmitAdd = true;
        removeSpecialCharacters(element);
        setTimeout('isDisabledSubmitAdd = false;', 500);
    }
}
// Contact Name
function onContactNameBlur(element, e) {
    var ignore = e.relatedTarget || e.rangeParent;
    if (ignore == undefined) {
        if (e.originalEvent != undefined)
            ignore = e.originalEvent.explicitOriginalTarget;
    }
    if (ignore != null) {
        handleClickElement(ignore);
    }
    if (ValidateContactName() == false || isIncludeSpecialCharacters(element)) {
        isDisabledSubmitAdd = true;
        removeSpecialCharacters(element);
        setTimeout('isDisabledSubmitAdd = false;', 500);
    }
}
// Contact Number
function onContactNumberBlur(element, e) {
    var ignore = e.relatedTarget || e.rangeParent;
    if (ignore == undefined) {
        if (e.originalEvent != undefined)
            ignore = e.originalEvent.explicitOriginalTarget;
    }
    if (ignore != null) {
        handleClickElement(ignore);
    }
    if (ValidateContactNumber2() == false || isIncludeSpecialCharacters(element)) {
        isDisabledSubmitAdd = true;
        removeSpecialCharacters(element);
        setTimeout('isDisabledSubmitAdd = false;', 500);
    }
}
// Contact Email
function onContactEmailBlur(element, e) {
    var ignore = e.relatedTarget || e.rangeParent;
    if (ignore == undefined) {
        if (e.originalEvent != undefined)
            ignore = e.originalEvent.explicitOriginalTarget;
    }
    if (ignore != null) {
        handleClickElement(ignore);
    }
    if (ValidateContactEmail() == false || isIncludeSpecialCharacters(element)) {
        isDisabledSubmitAdd = true;
        removeSpecialCharacters(element);
        setTimeout('isDisabledSubmitAdd = false;', 500);
    }
}

function handleClickElement(element) {
    if ($(element).val() == 'Clear') {
        $(element).click();
    }
}