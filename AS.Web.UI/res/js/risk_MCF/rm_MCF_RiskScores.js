var parentIsThresholdNegative = 0;
var parentIsIndicatorNegative = 0;
var const_ValidCharsIndicator = '0123456789';
var const_ValidCharsThreshold = '0123456789';


function ajaxRequestStart(sender, args) {
    UxExporter_OnRequestStart(sender, args);
}
function ajaxOnResponseEnd(sender, args) {
    UxExporter_OnResponseEnd(sender, args);
}
function hasInvalidCharacters(input, invalid_chars) {
    var invalidHexValue = '';
    for (var i = 0; i < invalid_chars.length; i++) {
        invalidHexValue += '\\x' + invalid_chars.charCodeAt(i).toString(16).toUpperCase();
    }
    return input.search(new RegExp('[' + invalidHexValue + ']', 'ig')) >= 0;
}

function hasOnlyCharacters(input, valid_chars) {
    var validHexValue = '';
    for (var i = 0; i < valid_chars.length; i++) {
        validHexValue += '\\x' + valid_chars.charCodeAt(i).toString(16).toUpperCase();
    }
    var m = input.match(new RegExp('[' + validHexValue + ']', 'ig'));

    if (m != null) {
        return m.length == input.length;
    }

    return false;
}

function focusTextbox(textbox) {
    textbox.focus();
    textbox.select();
}




function ValidateExisting_Success(res) {
    if (res[0] == "true") {
        __doPostBack(res[1], '');
    }
    else {
        alert('<%=Resources.RiskMessageManager.Risk_ValidationMessages_V15 %>');
    }
}
function DefaultEnterOnDiv(e) {
    var isEnter = false;
    var isIE = /MSIE/.test(navigator.userAgent);
    var keyCode = isIE ? e.keyCode : e.which;
    isEnter = (keyCode == 13);
    if (isEnter) {
        return false;
    }
}
//Search when user focus on textbox
function doClick(btnID) {
    document.getElementById(btnID).click();
}
function SearchEnterOnTextbox(e, btnID) {
    var isEnter = false;
    var isIE = /MSIE/.test(navigator.userAgent);
    var keyCode = isIE ? e.keyCode : e.which;
    isEnter = (keyCode == 13);

    if (keyCode > 31 && (keyCode < 48 || keyCode > 57))
        return false;
    if (isEnter) {
        document.getElementById(btnID).focus();
        setTimeout("doClick('" + btnID + "')", 100);
        return false;
    }

}
function HideCreateRiskScore(id) {
    document.getElementById(id).style.display = "none";
}

function SearchEnterOnDecimalTextbox(e, btnID, precision) {
    var isIE = /MSIE/.test(navigator.userAgent);
    var keyCode = isIE ? e.keyCode : e.which;
    isEnter = (keyCode == 13);

    if (keyCode > 31 && keyCode !== 46 && (keyCode < 48 || keyCode > 57)) {
        return false;
    }

    if (!isEnter) {
        const currentValue = e.target.value;
        const cursorStart = e.target.selectionStart;
        const cursorEnd = e.target.selectionEnd;

        const nextValue =
            currentValue.slice(0, cursorStart) +
            e.key +
            currentValue.slice(cursorEnd);

        var precisionData = precision ?? 4;
        const strRegex = `^\\d{1,9}(\\.\\d{0,${precisionData}})?$`;
        const regex = new RegExp(strRegex);
        if (!regex.test(nextValue)) {
            return false;
        }
    }

    if (isEnter) {
        document.getElementById(btnID).focus();
        setTimeout("doClick('" + btnID + "')", 100);
        return false;
    }
}