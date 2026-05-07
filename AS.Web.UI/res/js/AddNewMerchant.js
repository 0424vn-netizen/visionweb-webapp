/*Open CM and Redirect to MID*/
function addMerchantCallback(url) {
    if (IsOpenCase == 'True') {
        openPopupWindow(url, 'OpenNewCase');
    }
    window.location = rootURL + "MerchantProfile.aspx";
}

/*Add New Merchant*/
function ValidateAddNewMerchant() {
    var result = true;
    var isExist = true;
    var isValidState = true;
    //Check is exist merchant
    var data = {};
    data["merchantNum"] = $("#" + uxMerchantNum_ClientID).val();
    postServer("AddNewMerchant.aspx/CheckMerchantExist", data, function (res) {
        if (res.d) {
            $("#lbMsgMerchantNum").text(MgsExistMerchant);
            isExist = false;
        } else
            $("#lbMsgMerchantNum").text("");
        if (!ValidateInput()) {
            result = false;
        }
    });
    //Check invalid state
    var dataState = {};
    var countryObj = $find(uxCountry_ClientID);
    var stateObj = $find(uxStateProvince_ClientID);
    var countryText = $("#" + uxStateProvince_ClientID).val();
    if (!!countryObj && !!countryText) {
        dataState["countryCode"] = countryObj.get_selectedItem().get_value();
        dataState["state"] = stateObj.get_selectedItem().get_value();
        postServer("AddNewMerchant.aspx/CheckStateIsBelongToCountry", dataState, function (res) {
            if (!res.d) {
                $("#mgsErrorState").text(MgsInvalidState);
                isValidState = false;
            } else {
                $("#mgsErrorState").text("");
            }
        });
    }

    if (!isExist) {
        $("label[for='" + uxMerchantNum_ClientID + "']").addClass("label-error");
        return false;
    }

    if (!isValidState) {
        $("label[for='" + uxStateProvince_ClientID + "_Input']").addClass("label-error");
        return false;
    } else {
        $("label[for='" + uxStateProvince_ClientID + "_Input']").removeClass("label-error");
    }


    return result;
}

/*Remove Business Website*/
function removeBusinessWebsite(e) {
    var parentObj = $(e).parents("tr")

    var ipId = parentObj.data("bw");
    var trId = parentObj.data("id");
    $("#" + hdBusinessWebId_ClientID).val(ipId);
    $("#" + hdTrBusinessWebId_ClientID).val(trId);
    $("#" + uxRemoveBusinessWeb_ClientID).click();

    addClassRowOrAltRow();
    return false;
}

/*Add Business Website*/
function addBusinessWebsite(e) {
    var trObjs = $("#tblBusinessWebsite").find("tr.b-website");
    var idx = 0;
    for (var i = 0; i < trObjs.length; i++) {
        if ($(trObjs[i]).hasClass("hide")) {
            idx = i;
            break;
        }
    }

    var inputObj = $(trObjs[idx]);
    var ipId = inputObj.data("bw");
    var trId = inputObj.data("id");
    $("#" + hdBusinessWebId_ClientID).val(ipId);
    $("#" + hdTrBusinessWebId_ClientID).val(trId);

    //Disable/Enabled add link
    var bwHide = $("#tblBusinessWebsite > tbody > tr.hide");
    $("#" + hdHideTrCount_ClientID).val(bwHide.length);

    $("#" + uxAddBusinessWeb_ClientID).click();
    addClassRowOrAltRow();
    return false;
}

function masterAjax_responseEnd(sender, args) {
    addClassRowOrAltRow();
}

$(document).ready(function ($) {
    $("input[numeric-only='true']").change(function () {
        allowOnlyNumber(this);
    });
    $("input[numeric-only='true']").keyup(function () {
        allowOnlyNumber(this);
    });
    $("input[display-masked='Phone']").mask("(000) 000-0000");
    addClassRowOrAltRow();
});

function uxCountry_OnClientKeyPressing(sender, args) {
    var uxCountry = $find(uxCountry_ClientID);
    uxCountry.showDropDown();
}

function uxStateProvince_OnClientKeyPressing(sender, args) {
    var uxStateProvince = $find(uxStateProvince_ClientID);
    uxStateProvince.showDropDown();
}

function uxSicMcc_OnClientKeyPressing(sender, args) {
    var uxSicMcc = $find(uxSicMcc_ClientID);
    uxSicMcc.showDropDown();
}

/*Helper*/

function addClassRowOrAltRow() {
    $("#add-merchant table.ASTable > tbody > tr:even").addClass("AltRow");
    $("#add-merchant table.ASTable > tbody > tr:odd").addClass("Row");
}

function postServer(url, data, callback) {
    $.ajax({
        type: "post",
        url: url,
        async: false,
        data: JSON.stringify(data),
        contentType: "application/json",
        dataType: "json",
        success: function (result) {
            if (callback) {
                callback(result);
            }
        },
        error: function (result) {
            //console.log(result.responseText);
        }
    });
}

/*Validation*/

function allowOnlyNumber(e) {

    var obj = $(e);
    var rexg = /^([0-9]+)/g;
    if (obj.attr("start-with-zero") == "false") {
        rexg = /^(?!0)\d+$/g;
    }
    var val = obj.val();
    val = rexg.exec(val);
    if (val) {
        obj.val(val[0]);
    } else {
        obj.val("");
    }
}

function checkFixed5() {
    var merchantNumVal = $("#" + uxMerchantNum_ClientID).val();
    if (merchantNumVal.length != 5)
        return false;
    return true;
}

function checkMerchantExist() {
    var merchantNumVal = $("#" + uxMerchantNum_ClientID).val();
    var data = {};
    data["merchantNum"] = merchantNumVal;
    postServer("AddNewMerchant.aspx/CheckMerchantExist", data, function (res) {
        console.log(res.d);
        return res.d;
    });
}

function validatePhone() {
    var objPhone = $("#" + uxResellerPhone_ClientID);
    if (objPhone == null) return true;

    var reg = /\D?(\d{3})\D?\D?(\d{3})\D?(\d{4})$/;
    if (objPhone.val() != '') {
        if (reg.test(objPhone.val()))
            return true;
        return false;
    }
    return true;
}

function businessWebRequired(id) {
    var obj = $("#" + id);
    var trParent = obj.parents("tr");
    if (trParent.hasClass("hide"))
        return true;
    if (obj == null)
        return true;
    else {
        if (obj.val() == "")
            return false;
    }
    return true;
}

function validationURL(id) {
    //var expression = /[-a-zA-Z0-9_]{2,256}\.[a-z]{2,4}\b(\/[-a-zA-Z0-9_]*)?/gi;
    var expression = /^(http:\/\/www\.|https:\/\/www\.|http:\/\/|https:\/\/)?[a-z0-9A-Z]+([\-\_\.]{1}[a-z0-9A-Z]+)*\.[a-z]{1,5}(:[0,9]{1,5})?([\/\.A-Za-z0-9\?\_\=\-]+)?$/gm;
    var regex = new RegExp(expression);
    var obj = $("#" + id);
    var trParent = obj.parents("tr");
    if (trParent.hasClass("hide") || obj == null) {
        return true;
    }
    else {
        return regex.test(obj.val());
    }
    return true;
}

/*End Custom Validation*/
