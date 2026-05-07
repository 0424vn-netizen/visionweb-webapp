
var filterTypeValue = 0;

function uxFilterType_OnClientSelectedIndexChanged(sender, args) {
    var result = false;
    filterTypeValue = $find(uxFilterTypeID).get_value();
    if (filterTypeValue == "ALL"
        || filterTypeValue == "MERCHANT"
        || filterTypeValue == "0"
        || filterTypeValue == "SELECT_ONE") { // All, Merchant, None ore default SelectOne
        if (filterTypeValue == "ALL" || filterTypeValue == "MERCHANT" || filterTypeValue == "SELECT_ONE") { // All or Merchant

            document.getElementById(uxReloadGridsID).click();
            result = false;
        }

    }
    else {
        for (var i = 0; i < FilterTypeList.length; i++) {
            if (FilterTypeList[i].Type == filterTypeValue) {
                $get("cidSearchValuePrompt").innerHTML = FilterTypeList[i].Prompt;
            }
        }
        result = true;
    }

    var display = ((filterTypeValue != "ALL" && filterTypeValue != "MERCHANT" && filterTypeValue != "0" && filterTypeValue != "SELECT_ONE") ? "" : "none");

    $get("cidSearchValue").style.display = display;
    $get("cidSubmitPanel").style.display = display;

    return result;
}

function ValidateData() {

    if ($find(uxSearchValueCommonKeyID).get_value().length == 0) {

        for (var i = 0; i < FilterTypeList.length; i++) {
            if (FilterTypeList[i].Type == filterTypeValue) {
                alert(FilterTypeList[i].TypeName + rm_MultiMerchantWatch_js_String1);
            }
        }
        $find(uxSearchValueCommonKeyID).showDropDown();
        return false;
    }
    return true;
}


//for Multi watch type
function getInternetExplorerVersion() {
    var rv = -1; // Return value assumes failure.
    if (navigator.appName == 'Microsoft Internet Explorer') {
        var ua = navigator.userAgent;
        var re = new RegExp("MSIE ([0-9]{1,}[\.0-9]{0,})");
        if (re.exec(ua) != null)
            rv = parseFloat(RegExp.$1);
    }
    return rv;
}
function checkVersion() {
    var msg = rm_MultiMerchantWatch_js_String2;
    var ver = getInternetExplorerVersion();
    if (ver > -1) {
        if (ver >= 8.0)
            msg = "8"
        else
            msg = "7";
    }
    return (msg);
}

//for Multi-merchant on watch
function merchant_Click(merchantNumber) {
    $get(hddProcessDataID).value = "merchant;" + merchantNumber;
    $get(btnProcessID).click();
    return true;
}
function loadList(filterType, filterValue) {
    $get(hddProcessDataID).value = "loadList;" + filterType + ";" + filterValue;
    $get(btnLoadListID).click();

}
function loadListByMode(filterType) {
    $get(hddProcessDataID).value = "loadListByMode;" + filterType;
    $get(btnLoadListID).click();
}
function ajaxRequestStart(sender, args) {
    if (args.get_eventTarget().indexOf('uxExport') != -1) {
        args.set_enableAjax(false);
    }
}
function initialize() {
    filterTypeValue = $find(uxFilterTypeID).get_value();
    var display = ((filterTypeValue != "ALL" && filterTypeValue != "MERCHANT" && filterTypeValue != "0" && filterTypeValue != "SELECT_ONE") ? "" : "none");

    $get("cidSearchValue").style.display = display;
    $get("cidSubmitPanel").style.display = display;
}
setTimeout("initialize()", 500);

function addCheckSpecialCharacters() {
    $("#" + uxSearchValueCommonKeyID + "_Input").change(function (e) {
        if (isIncludeSpecialCharacters(this)) {
            alert(content.AlertMsgSpecialCharacters);
            var newValue = removeSpecialCharacters(this);
            var radCombobox = $find(uxSearchValueCommonKeyID);
            radCombobox.set_text(newValue);
            this.focus();
        }
    });
}

$(document).ready(function () {
    addCheckSpecialCharacters();
});