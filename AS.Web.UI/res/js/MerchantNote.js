/*
--Ticket: 39919 - VW - CMS Integration with Risk Application
--Author: HaoDang create new
*/

var limitNum = 7000;
var message = AddNote_js_msg1 + ' ' + limitNum + ' ' + AddNote_js_Characters + '!';
var counter = $get('counter');
function OnClientLoad(editor, args) {
    rtfEditor = editor;
    AttachHandlers();
    LimitLength();

    editor.add_modeChange(function (sender, args) {
        AttachHandlers();
        LimitLength();
    });

    editor.add_commandExecuted(function (sender, args) {
        if (args.get_commandName().toLocaleLowerCase() == "paste") {
            LimitLength();
        }
    });

    // Set background color
    var style = editor.get_contentArea().style;
    style.backgroundColor = "#ffffff";
    style.paddingTop = "10px";

    //Disable menu context
    var toolAdapter = editor.get_toolAdapter();
    if (toolAdapter) {
        toolAdapter.enableContextMenus(false);
    }
}

function LimitLength() {
    setTimeout(function () {
        var rtfEditor = $find(MerchantProfile_uxComment);
        var oValue = rtfEditor.get_text(true).trim();
        counter.innerHTML = "<label class=\"text-muted\">" + AddNote_js_Characters + ": </label><strong><strong>" + oValue.length + " / " + limitNum + "</strong>";
    }, 500);
}

function AttachHandlers() {
    var rtfEditor = $find(MerchantProfile_uxComment),
        textContentArea = rtfEditor.get_contentArea();

    if (rtfEditor) {
        rtfEditor.attachEventHandler("onkeyup", LimitLength);
        rtfEditor.attachEventHandler("onpaste", LimitLength);
        rtfEditor.attachEventHandler("onblur", LimitLength);
    }
    if (textContentArea) {
        textContentArea.addEventListener("keyup", LimitLength);
        textContentArea.addEventListener("paste", LimitLength);
        textContentArea.addEventListener("blur", LimitLength);
    }
}

function Validate() {
    var rtfEditor = $find(MerchantProfile_uxComment);
    var oValue = rtfEditor.get_text(true).trim();
    if (oValue.length == 0) {
        $("#ciCommentsMsg").removeClass("display-none");
        $("#ciCommentsMsg").text(AddNote_js_Required);
        return false;
    }
    if (oValue.length > limitNum) {
        $("#ciCommentsMsg").removeClass("display-none");
        $("#ciCommentsMsg").text(message);
        //AdjustWindowSize(657);
        return false;
    }
    $("#ciCommentsMsg").addClass("display-none");
    CheckSensitiveData();
}

function reCreateMultiSelector() {
    $('#' + MerchantNote_uxSourceList).chosen('');
    $('#' + MerchantNote_uxRoleList).chosen('');
    $('#' + MerchantNote_uxAddedByList).chosen('');
    $('[data-hover="dropdown"]').dropdownHover();
}

//function masterAjax_responseEnd(sender, args) {
//    reCreateMultiSelector();
//    $("#" + AddNote_uxApplyFilter).prop("disabled", false);
//    if (typeof (UxExporter_OnResponseEnd) == 'function')
//        UxExporter_OnResponseEnd(sender, args);
//    //TK:39919 - #35650
//    window.scrollTo(x, y) // Fix issue auto resize content height on Chrome
//}


function masterAjax_requestStart(sender, args) {
    //$("#" + AddNote_uxApplyFilter).prop("disabled", true);
    if (typeof (UxExporter_OnRequestStart) == 'function')
        UxExporter_OnRequestStart(sender, args);

    x = window.scrollX || window.pageXOffset || document.body.scrollLeft;
    y = window.scrollY || window.pageYOffset || document.body.scrollTop;
}

//[43454] - Encrypting the comments field in VW Case Management 
function CheckSensitiveData() {
    var rtfEditor = $find(MerchantProfile_uxComment);
    var oValue = rtfEditor.get_text(true).trim();

    var data = {};
    data["comment"] = oValue;
    var url = getUrl() + "/CheckSensitiveData";
    postServer(url, data, function (result) {
        var cardValid = result.d;
        if (cardValid != "" && cardValid != null) {
            window.cardNumbers = cardValid;
            ShowPopupModal(rootURL + 'SensitiveDataDetectedModal.aspx', 'auto');
        } else {
            __doPostBack(AddNote_uxSubmit, '');
        }
    })
    return false;
}

var isClientClose = false;

parent.SubmitAddNote = function (cardKeyList, isClose) {
    isClientClose = isClose;
    HidePopupModal();
    document.getElementById(hdCardDetected).value = cardKeyList;
    __doPostBack(AddNote_uxSubmit, '');
    isClientClose = false;
}

////OnClientClose
//var isSubmitAddDefaultSetting = false;
//function PopupModalClose(sender, eventArgs) {
//    if (sender._navigateUrl == "SensitiveDataDetectedModal.aspx") {
//        if (!isClientClose) {
//            parent.SubmitAddNote('', false);
//        }
//    }
//    if (sender._navigateUrl.indexOf("DefaultSettingModal.aspx") >= 0) {
//        if (isSubmitAddDefaultSetting) {
//            $("#" + uxFinishSaveDefaultSettingMN_ClientID).click();
//            isSubmitAddDefaultSetting = false;
//        }
//    }
//}

//Optimize filter merchant note - PROP ISSUE
function postServer(url, data, callback) {
    $.ajax({
        type: "post",
        url: url,
        async: true,
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

function buildMutilChoose(elementObj, obj) {
    var result = "";
    var jsonObj = JSON.parse(obj);
    for (var i = 0; i < jsonObj.length; i++) {
        result += "<option value='" + jsonObj[i].ID + "'>" + jsonObj[i].Value + "</option>";
    }
    elementObj.html(result);
    elementObj.chosen('destroy');
    elementObj.chosen({ search_contains: true });
}

function getUrl() {
    var result = "";
    var url = window.location.href;
    if (!!url) {
        if (url.indexOf("rm_RiskReport.aspx") > 0) {
            result = "rm_RiskReport.aspx";
        }
        if (url.indexOf("rm_MCF_RiskReport.aspx") > 0) {
            result = "rm_MCF_RiskReport.aspx";
        }
        if (url.indexOf("MerchantProfile.aspx") > 0) {
            result = "MerchantProfile.aspx";
        }
        if (url.indexOf("MerchantInformation.aspx") > 0) {
            result = "MerchantInformation.aspx";
        }
    }
    return result;
}

function setHeightIconLoading(element) {
    var pHeight = element.parent().css("height");
    element.css("height", pHeight);
}

//Call when default setting modal is closed
function bindSourceAndRole(isTabSelected = false) {    
    setHeightIconLoading($("#uxProgress"));
    let data = {};
    let url = getUrl() + "/GetSourceAndRole";
    setTimeout(
        function () {
            postServer(url, data, function (result) {
                buildMutilChoose($("#" + MerchantNote_uxSourceList), result.d[1]);
                buildMutilChoose($("#" + MerchantNote_uxRoleList), result.d[2]);
                buildMutilChoose($("#" + MerchantNote_uxAddedByList), result.d[3]);
                keepRole();
                keepUser();
                let defaultSources = result.d[0] ? result.d[0].split(',') : "";
                $("#" + MerchantNote_uxSourceList).val(defaultSources).trigger("chosen:updated");
                if (!isTabSelected) {
                    document.getElementById(uxFinishSaveDefaultSettingMN_ClientID).click();
                }
            })
        }, 0);
}

$("#" + MerchantNote_uxSourceList).change(function () {
    bindRoleAndUser(true);
})

$("#" + MerchantNote_uxRoleList).change(function () {
    bindUser(true);
})


$("#" + MerchantNote_uxAddedByList).change(function () {
    keepUser();
})

function bindRoleAndUser(isShowLoading) {
    if (isShowLoading) {
        setHeightIconLoading($("#uxProgress"));
        $("#uxProgress").removeClass("hide");
    }
    var data = {};
    var sources = $("#" + MerchantNote_uxSourceList).val();
    if (!!sources)
        data["sources"] = sources.toString();
    else
        data["sources"] = "";

    var url = getUrl() + "/GetRoleAndAddedBy";
    setTimeout(
        function () {
            postServer(url, data, function (result) {
                var roles = $("#" + MerchantNote_uxRoleList).val();
                var users = $("#" + MerchantNote_uxAddedByList).val();
                buildMutilChoose($("#" + MerchantNote_uxRoleList), result.d[0]);
                buildMutilChoose($("#" + MerchantNote_uxAddedByList), result.d[1]);
                keepRole();
                keepUser();
                if (isShowLoading) {
                    $("#uxProgress").addClass("hide");
                } else {
                    if (roles) {
                        $('#' + uxhdRole_ClientID).val(roles.join(','));
                        $("#" + MerchantNote_uxRoleList).val(roles).trigger("chosen:updated");
                    }
                    if (users) {
                        $('#' + uxhdAddedBy_ClientID).val(users.join(','));
                        $("#" + MerchantNote_uxAddedByList).val(users).trigger("chosen:updated");
                    }
                }
            })
        }, 0);
}

function bindRole() {
    var data = {};
    var sources = $("#" + MerchantNote_uxSourceList).val();
    if (!!sources)
        data["sources"] = sources.toString();
    else
        data["sources"] = "";

    var url = getUrl() + "/GetRoles";
    postServer(url, data, function (result) {
        var roles = $("#" + MerchantNote_uxRoleList).val();
        buildMutilChoose($("#" + MerchantNote_uxRoleList), result.d);
        keepRole();
        keepUser();
        if (roles) {
            $('#' + uxhdRole_ClientID).val(roles.join(','));
            $("#" + MerchantNote_uxRoleList).val(roles).trigger("chosen:updated");
        }
    });
}

function bindUser(isShowLoading) {
    if (isShowLoading) {
        setHeightIconLoading($("#uxProgress"));
        $("#uxProgress").removeClass("hide");
    }
    var data = {};
    var sources = $("#" + MerchantNote_uxSourceList).val();
    var roles = $("#" + MerchantNote_uxRoleList).val();

    if (!!sources)
        data["sources"] = sources.toString();
    else
        data["sources"] = "";

    if (!!roles)
        data["roles"] = roles.toString();
    else
        data["roles"] = "";

    var url = getUrl() + "/GetAddedBy";
    setTimeout(
        function () {
            postServer(url, data, function (result) {
                var users = $("#" + MerchantNote_uxAddedByList).val();
                buildMutilChoose($("#" + MerchantNote_uxAddedByList), result.d);
                keepRole();
                keepUser();
                if (isShowLoading) {
                    $("#uxProgress").addClass("hide");
                } else {
                    $('#' + uxhdAddedBy_ClientID).val(users.join(','));
                    $("#" + MerchantNote_uxAddedByList).val(users).trigger("chosen:updated");
                }
            })
        }, 0);
}

function triggerApplyFilter() {
    document.getElementById(AddNote_uxApplyFilter).click();
}

function keepRole() {
    //Keep role
    if ($("#" + MerchantNote_uxRoleList).val() != null) {
        $('#' + uxhdRole_ClientID).val($("#" + MerchantNote_uxRoleList).val().join(','));
    }
    else {
        $('#' + uxhdRole_ClientID).val('');
    }
}

function keepUser() {
    //Keep user
    if ($("#" + MerchantNote_uxAddedByList).val() != null) {
        $('#' + uxhdAddedBy_ClientID).val($("#" + MerchantNote_uxAddedByList).val().join(','));
    }
    else {
        $('#' + uxhdAddedBy_ClientID).val('');
    }
}

//44894 - VW- Merchant Note Default Preferences via User Mgmt Settings
function openDefaulSettingModalMN() {
    return ShowPopupModal(rootURL + 'DefaultSettingModal.aspx?' + openDefaultSettingUrlMN, 'auto');
    //return ShowPopupModal(rootURL + 'DefaultSettingModal.aspx?' + openDefaultSettingUrlMN, 'auto', 'auto', '100px', '100px');
}

function rebindNoteFromRiskReport() {
    document.getElementById(uxBtnRefresh).click();
}

$(document).ready(function () {
    if ($("#merchantnotes").find(".report-export a").css("display") == "none") {
        $("#merchantnotes").find("#defaultSetting").css("right", "20px");
    }
})