window.onload = function () {
    setTimeout(loadRiskCategoryControl, 100);
}


function checkInputCharacterCampaignID(e) {
    var val = e.value;
    if (isNaN(val))
        e.value = "";
}

function InputOnTextBox(e) {
    var isIE = /MSIE/.test(navigator.userAgent);
    var keyCode = isIE ? e.keyCode : e.which;
    if (keyCode > 31 && (keyCode < 48 || keyCode > 57))
        return false;
}


function loadRiskCategoryControl() {
}

function info_closeModalEvent(modalID) {
    switch (modalID) {
        case "RiskLevelModal":
            document.getElementById(Risk_Assignment_Info_uxRebindRiskLevel).click();
            break;
        case "RiskCategoryModal":
            document.getElementById(Risk_Assignment_Info_uxRebindRiskCategory).click();
            break;
        case "CampaignIDModal":
            document.getElementById(Risk_Assignment_Info_uxRebindCampaignID).click();
            break;
    }
}

function cbCampaignID_Changed() {
}

function validateRiskCampaign() {
    return true;
}

function ValidateRiskCampaignRequired() {
    let isValid = true;
    return isValid;
}
