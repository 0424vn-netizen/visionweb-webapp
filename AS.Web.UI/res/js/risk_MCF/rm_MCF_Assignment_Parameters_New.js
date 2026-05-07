var ROW_CARD_TEMPLATE = "<div runat='server' id='divGroup' class='row-card'>"
    + "<div style='width: 34px' class='item w-checkbox j-center'>"
    + "<input id='chkGroup' type='checkbox' onclick='onCheckValidGroup(this);'>";
var GROUP_TEMPLATE = "</div><div class='groups'>";


function parameter_Add(modal) {
    var pageName = window.location.href;
    var assignmentName = document.getElementById(Risk_Assignment_Info_uxAssignmentName).value;
    if (!ValidateAssignmentGeneralInformationByValidator()) {
        moveTo('#markupAssinfo');
        return false;
    }
    saveParameters(modal);
    return false;
}

function pauseMerchantAlert_Add() {
    if (!ValidateAssignmentGeneralInformationByValidator()) {
        moveTo('#markupAssinfo');
        return false;
    }
    return true;
}

function parameter_closeModalEvent(modalID) {
    switch (modalID) {
        case 'ParameterFilter_TransactionCodeModal':
            var primaryID = $("#" + Risk_Assignment_Parameters_uxPrimaryID_ClientID).val();
            var modeID = $("#" + Risk_Assignment_Parameters_uxMode_ClientID).val();
            var filterID = $("#" + Risk_Assignment_Parameters_uxFilterID_ClientID).val();
            var paramKey = $("#" + Risk_Assignment_Parameters_uxPramKey).val();
            var postData = { mode: modeID, assignmentID: primaryID, paramKey: paramKey, filterID: filterID };
            var url = "";
            if (isAdhoc())
                url = rootURL + "risk_MCF/rm_MCF_AdhocCreate.aspx/GetTransactionCode";
            else
                url = rootURL + "risk_MCF/rm_MCF_Assignment_CreateNew.aspx/GetTransactionCode";
            postServer(url, postData, function (result) {
                $("#uxParameterContent").find("div[id*='" + paramKey.toLowerCase() + "']").each(function () {
                    $(this).find("span[id='transactionCode']").each(function () {
                        $(this).text(result.d["Label"]);
                    });
                    $(this).find("span[id='tracking-TransactionCode']").each(function () {
                        $(this).text(result.d["Tracking"]);
                    });
                });
            });
            break;
        case 'ParameterFilter_ACHReturnCodeModal':
            var primaryACHID = $("#" + Risk_Assignment_Parameters_uxPrimaryID_ClientID).val();
            var modeACHID = $("#" + Risk_Assignment_Parameters_uxMode_ClientID).val();
            var filterACHID = $("#" + Risk_Assignment_Parameters_uxFilterID_ClientID).val();
            var paramACHKey = $("#" + Risk_Assignment_Parameters_uxPramKey).val();
            var postACHData = { mode: modeACHID, assignmentID: primaryACHID, paramKey: paramACHKey, filterID: filterACHID };
            var urlACH = "";
            if (isAdhoc())
                urlACH = rootURL + "risk_MCF/rm_MCF_AdhocCreate.aspx/GeACHReturnCode";
            else
                urlACH = rootURL + "risk_MCF/rm_MCF_Assignment_CreateNew.aspx/GeACHReturnCode";
            postServer(urlACH, postACHData, function (result) {
                $("#uxParameterContent").find("div[id*='" + paramACHKey.toLowerCase() + "']").each(function () {
                    $(this).find("span[id='achReturnCode']").each(function () {
                        $(this).text("" + result.d);
                    });
                });
            });
            break;
    }
    return false;
}

parent.ReloadParamList = function () {
    document.getElementById(Risk_Assignment_Parameters_btnRefreshParamList).click();
}

parent.updateParameterActions = function (action) {
    updateParameterActions(action);
}



function chbRiskScore_click() {
    if ($get(Risk_Assignment_Parameters_chkRiskScore).checked == false) {
        $find(Risk_Assignment_Parameters_txtRCFrom).clear();
        $find(Risk_Assignment_Parameters_txtRCTo).clear();

        $find(Risk_Assignment_Parameters_txtRCFrom).disable();
        $find(Risk_Assignment_Parameters_txtRCTo).disable();
    }
    else {
        $find(Risk_Assignment_Parameters_txtRCFrom).enable();
        $find(Risk_Assignment_Parameters_txtRCTo).enable();
    }
}

function ValidateRiskScoreFromTo() {
    var isValid = true;
    var chk = $get(Risk_Assignment_Parameters_chkRiskScore);
    if (chk != null && chk.checked) {
        var rcFrom = $find(Risk_Assignment_Parameters_txtRCFrom);
        var rcTo = $find(Risk_Assignment_Parameters_txtRCTo);

        if ((rcFrom.get_textBoxValue().trim() != "" && rcTo.get_textBoxValue().trim() != "")) {
            if (rcFrom.get_value() > rcTo.get_value())
                isValid = false;
        }
    }
    return isValid;
}

function ValidateRiskScoreToRequired() {
    var isValid = true;
    var chk = $get(Risk_Assignment_Parameters_chkRiskScore);
    var rcFrom = $find(Risk_Assignment_Parameters_txtRCFrom);
    var rcTo = $find(Risk_Assignment_Parameters_txtRCTo);
    if (chk != null && chk.checked && rcFrom.get_textBoxValue().trim() != "") {


        if ((rcFrom.get_textBoxValue().trim() != "" && rcTo.get_textBoxValue().trim() != "")) {
            isValid = true;
        }
        else if (rcTo.get_textBoxValue().trim() == "") {
            isValid = false;
            rcTo.focus();
            $('label[for=' + Risk_Assignment_Parameters_txtRCFrom + ']').addClass('label-error');
        }

    }
    return isValid;
}

function ValidateRiskScoreFromRequired() {
    var isValid = true;
    var chk = $get(Risk_Assignment_Parameters_chkRiskScore);
    if (chk != null && chk.checked) {
        var rcFrom = $find(Risk_Assignment_Parameters_txtRCFrom);
        var rcTo = $find(Risk_Assignment_Parameters_txtRCTo);

        if ((rcFrom.get_textBoxValue().trim() != "" && rcTo.get_textBoxValue().trim() != "")) {
            isValid = true;
        }
        else if (rcFrom.get_textBoxValue().trim() == "") {
            isValid = false;
            rcFrom.focus();
        }
    }

    return isValid;
}

function validateRiskScoreCheck() {
    let chk = $get(Risk_Assignment_Parameters_chkRiskScore);
    if (chk != null && chk.checked) {
        return true;
    }
    return false;
}

function ValidateAssignmentParameters() {
    //Validate riskscore
    let itemsRepeater = $("#uxParameterContent").find("div[id*='param']").length;
    let validRC = validateRiskScoreValues();
    if (!validRC)
        return false;

    let isChecked = validateRiskScoreCheck();
    if (!isChecked) {
        //Validate parameters
        if (itemsRepeater == 0) {
            let assignmentType = $find(Risk_Assignment_uxComboAssignmentType_ClientID);

            //check type is subsite
            if (assignmentType && assignmentType.get_selectedItem().get_value() == "4") {
                alert(Risk_Assignment_Parameters_js_msg3);
            }
            else {
                alert(Risk_Assignment_Parameters_js_msg1);
            }
            return false;
        }
    }
    return true;
}

function parameter_ShowFilter(url, index, paramkey, width, height) {
    document.getElementById(Risk_Assignment_Parameters_uxPramKey).value = paramkey;
    doOpenSubPopup(url, 'auto');
}

function isAdhoc() {
    if (window.location.href.indexOf("Adhoc") > -1)
        return true;
    return false;
}

/****ON CHECKBOX CHECK EVENT****/
function onCheckValidGroup(e) {
    //When click on any checkbox of parameter disable all other same parameter
    var isCheck = $(e).prop("checked");
    var groupObj = $(e).parents("div[id*='divGroup']");
    //Find all parameter in this group
    groupObj.find("div[id*='param']").each(function () {
        //With each parameter check the exist of its parameter in other group, if have disable it
        $("#uxAssignmentParamsGrid").find("div[id='" + $(this).prop("id") + "']").each(function () {
            if ($(this).parents("div[id*='divGroup']").find("input[type='checkbox']").prop("checked") == false) {
                $(this).parents("div[id*='divGroup']").find("input[type='checkbox']").attr("disabled", isCheck);
            }
        });
    });
    visibleCheckbox();
    showGroupButton();
    return false;
}

/******SHOW/HIDE CHECKBOX*******/
function visibleCheckbox() {
    $("#uxAssignmentParamsGrid").find("input[type='checkbox']").prop("disabled", false);
    var ckCheckeds = $("#uxAssignmentParamsGrid").find("input[type='checkbox']:checked");
    if (ckCheckeds.length > 0) {
        ckCheckeds.each(function () {
            $(this).parents("div[id*='divGroup']").find("div[id*='param']").each(function () {
                $("#uxAssignmentParamsGrid").find("div[id='" + $(this).prop("id") + "']").parents("div[id*='divGroup']").find("input[type='checkbox']").each(function () {
                    if ($(this).prop("checked") == false) {
                        $(this).prop("disabled", true);
                    }
                });
            });
        });
    }
}

/******SHOW/HIDE GROUP BUTTON*******/
function showGroupButton() {
    var count = 0;
    $("#uxAssignmentParamsGrid").find("input[type='checkbox']").each(function () {
        if ($(this).prop("checked")) {
            count++;
            $(this).parents("div[id*='divGroup']").addClass("active");
        } else {
            $(this).parents("div[id*='divGroup']").removeClass("active");
        }
    });
    if (count >= 2)
        $("#ucGroup").removeClass("hide");
    else
        $("#ucGroup").addClass("hide");
}

/******RADIO BUTTON MATCH CHANGE******/
function onMatchChange(isMatchAll) {
    if (isMatchAll == "True") {
        $("#divGroupTitle").addClass("hide");
        $("#uxAssignmentParamsGrid").addClass("hide-checkbox");
        $("#uxAssignmentParamsGrid").find(".group div.item.flex-grow-1.item-nrt").css("max-width", "none");
        if ($("#" + rm_Assignment_CreateNew_uxParamActions).val() !== "AddParameter") {
            $("#" + rm_Assignment_CreateNew_uxParamActions).val("");
        }
    }
}

/*****GROUP PARAMETERS*****/
function onGroup() {
    var jsonData = [];
    //Find all group is checked get all parameter put into json array
    $("#uxParameterContent").find("div[id*='divGroup']").each(function () {
        var ckObj = $(this).find("input[type='checkbox']");
        if (ckObj.prop("checked")) {
            $(this).find("div[id*='param']").each(function () {
                jsonData.push({ "Order": parseInt($(this).attr("data-order")), "Object": $(this) });
            });
        }
    });

    if (jsonData.length == 0)
        return;

    var orderArr = [];
    for (var i = 0; i < jsonData.length; i++) {
        orderArr.push(jsonData[i].Order);
    }
    orderArr = orderArr.sort(function (a, b) { return a - b });

    //Get first parameter in array set is root
    var index = orderArr[0];
    var obj = getObject(jsonData, index);
    if (obj != null) {
        for (var j = orderArr.length - 1; j > 0; j--) {
            var ob = getObject(jsonData, orderArr[j]);
            ob.find("a[id*='uxUnGroup']").removeClass("hide");
            //Insert parameter in array after root in order
            ob.insertAfter(obj);
        }
        obj.parents("div[id*='divGroup']").find("input[type='checkbox']").prop("checked", false);
        obj.find("a[id*='uxUnGroup']").removeClass("hide");
    }

    //Remove parameter after they are moved to group
    $("#uxParameterContent").find("div[id*='divGroup']").each(function () {
        var ckObj = $(this).find("input[type='checkbox']");
        if (ckObj.prop("checked")) {
            $(this).remove();
        }
    });
    //Enable all checkbox after group   
    $("#uxParameterContent").find("input[type='checkbox']").each(function () {
        $(this).attr("disabled", false);
    });

    showGroupButton();
    showFinishButton();

    updateParameterActions("GroupParameter");
    return false;
}

/*****UNGROUP PARAMETERS*****/
function onUnGroup(e) {
    var groupObj = $(e).parents("div[id*='divGroup']");
    var paramObj = $(e).parents("div[id*='param']");

    if (groupObj.find("div[id*='param']").length > 2) {
        //If group have more than 2 parameter hide ungroup button
        paramObj.find("a[id*='uxUnGroup']").addClass("hide");
    }
    else {
        //Hide ungroup button for all parameter
        groupObj.find("div[id*='param']").each(function () {
            $(this).find("a[id*='uxUnGroup']").addClass("hide");
        });
    }
    var div = $(ROW_CARD_TEMPLATE);
    var group = $(GROUP_TEMPLATE);
    group.append(paramObj[0]);
    div.append(group);
    div.insertAfter(groupObj[0]);

    $("#uxParameterContent").find("input[type='checkbox']").attr("disabled", false);
    showFinishButton();
    visibleCheckbox();
    updateParameterActions("UnGroupParameter");
    return false;
}

/*****DUPLICATE PARAMETERS******/
function onDuplicate(e) {
    var groupObj = $(e).parents("div[id*='divGroup']");
    var paramObj = $(e).parents("div[id*='param']");
    var ckObj = groupObj.find("input[type='checkbox']");
    if (groupObj.find("div[id*='param']").length > 1) {
        //Clone this parameter and create new group add this parameter
        paramDupObj = $(ROW_CARD_TEMPLATE + GROUP_TEMPLATE + paramObj.clone().prop("outerHTML") + "</div>");

    } else {
        //Clone this group
        paramDupObj = $(groupObj.clone());
    }
    //Hide ungroup butoon
    if (paramDupObj.find("a[id*='uxUnGroup']")) {
        paramDupObj.find("a[id*='uxUnGroup']").addClass("hide");
    }

    if (ckObj.prop("checked")) {
        //If parameter is checked disable and uncheck its duplicate parameter
        paramDupObj.find("input[type='checkbox']").prop("checked", false);
        paramDupObj.find("input[type='checkbox']").attr("disabled", true);
    }
    if (paramDupObj.find("div[id='" + paramObj.prop('id') + "']").attr("data-is-source") == "true") {
        paramDupObj.find("div[id='" + paramObj.prop('id') + "']").find(".edit").parents("span").remove();
        paramDupObj.find("div[id='" + paramObj.prop('id') + "']").find("input[type='hidden']").remove();
        paramDupObj.find("div[id='" + paramObj.prop('id') + "']").find(".non-edit").removeClass("hide");
        paramDupObj.find("div[id='" + paramObj.prop('id') + "']").find("a[id='edit-transactioncode']").addClass("hide");
        paramDupObj.find("div[id='" + paramObj.prop('id') + "']").find("a[id='view-transactioncode']").removeClass("hide");
    }
    paramDupObj.find("div[id='" + paramObj.prop('id') + "']").attr("data-is-source", "false");
    groupObj.after(paramDupObj.clone().prop("outerHTML"));
    visibleCheckbox();
    showFinishButton();
    updateParameterActions("DuplicateParameter");
    return false;
}

/***** DELETE PARAMETERS *****/
function confirmDeleteParameter(e) {
    //if (!ValidateAssignmentMerchantRange()) {
    //    return false;
    //}
    var agree = confirm(Risk_Assignment_Parameters_js_msg2);
    if (agree) {
        //In case: group has more than 2 parameters only check duplicate and remove on FE
        var groupObj = $(e).parents("div[id*='divGroup']");
        var paramObj = $(e).parents("div[id*='param']");
        var removeObj = null;
        var unGroup = false;
        var removeGroup = false;

        var actionUrl = rootURL + "risk_MCF/rm_MCF_Assignment_CreateNew.aspx/RemoveParametersToStagging";
        var primaryID = $("#" + Risk_Assignment_Parameters_uxPrimaryID_ClientID).val();
        var isModelType = paramObj.data('is-model-type');
        if (isModelType) {
            var postData = { assignmentID: primaryID, paramKey: paramObj.data('key') };
            postServer(actionUrl, postData);
        }
        
        if (groupObj.find("input[type='checkbox']").prop("checked") == true) {
            $("#uxAssignmentParamsGrid").find("div[id='" + paramObj.prop("id") + "']").each(function () {
                $(this).parents("div[id*='divGroup']").find("input[type='checkbox']").attr("disabled", false);
            });
        }

        if (groupObj.find("div[id*='param']").length == 1) {
            removeGroup = true;
        }
        if (groupObj.find("div[id*='param']").length == 2) {
            unGroup = true;
        }
        if ($("#uxAssignmentParamsGrid").find("div[id='" + paramObj.prop("id") + "']").length == 1) {
            //Call ajax remove on stagging            
            removeObj = paramObj;
            removeObj.remove();
            //saveParameters();
        }
        else {
            //In case: remove source param, find duplicate param replace it by source param and remove source param
            if (paramObj.attr("data-is-source") == "true") {
                var dupParamObj = $($("#uxAssignmentParamsGrid").find("div[id='" + paramObj.prop("id") + "'][data-is-source='false']")[0]);
                if (paramObj.find("a[id*='uxUnGroup']").length > 0) {
                    paramObj.find("a[id*='uxUnGroup']").addClass("hide");
                }
                if (dupParamObj.parents("div[id*='divGroup']").find("div[id*='param']").length > 1) {
                    paramObj.find("a[id*='uxUnGroup']").removeClass("hide");
                }
                paramObj.insertAfter(dupParamObj);
                removeObj = dupParamObj;
            }
            else {
                removeObj = paramObj;
            }
        }
        if (removeGroup == true) {
            groupObj.remove();
        }
        if (unGroup == true) {
            groupObj.find("a[id*='uxUnGroup']").addClass("hide");
        }
        removeObj.remove();
        showGroupButton();
        showFinishButton();
        visibleCheckbox();
        updateParameterActions("RemoveParameter");
    }
    return false;
}

/*****HELPER METHODS****/
function saveParameters(modal) {
    var actionUrl = "";
    if (isAdhoc())
        actionUrl = rootURL + "risk_MCF/rm_MCF_AdhocCreate.aspx/SaveParametersToStagging";
    else
        actionUrl = rootURL + "risk_MCF/rm_MCF_Assignment_CreateNew.aspx/SaveParametersToStagging";
    var primaryID = $("#" + Risk_Assignment_Parameters_uxPrimaryID_ClientID).val();
    var modeID = $("#" + Risk_Assignment_Parameters_uxMode_ClientID).val();
    var params = buildJson(modal);
    if (!params.IsSuccess)
        return false;
    var postData = { mode: modeID, assignmentID: primaryID, parameters: params.data };
    postServer(actionUrl, postData, function (result) {
        if (modal) {
            doOpenSubPopup(modal + Risk_Assignment_Parameters_QueryString, 'auto');
        }
    });
    return true;
}


/******BUILD JSON DATA*******/
function buildJson(modal) {
    var data = [];
    var isSuccess = true;
    var groups = $("#uxParameterContent").find("div[id*='divGroup']");
    var groupId = 1;
    if (groups.length > 0) {
        groups.each(function () {
            $(this).find("div[id*='param']").each(function () {
                var itemdata = getJsonItem(groupId, $(this), modal);
                data.push(itemdata);
                if (isSuccess && !itemdata.IsSuccess) {
                    isSuccess = false;
                    moveTo("#" + this.id);
                }
            });
            groupId++;
        })
    }
    return { data: data, IsSuccess: isSuccess };
}

/******GET JSON DATA OF ITEM*******/
function getJsonItem(groupId, paramObj, modal) {
    var isError = true;
    var isSourceParam = 0;
    var isParameterValueVisible = false;
    var thresholdLow = reAlertThresholdLow = thresholdHigh = reAlertParameterThreshold = reAlertParameterThresholdHigh = threshold
        = reAlertParameterThreshold = parameterValue = reAlertParameterIndicator = from = fromNRT = to = toNRT = "0";
    var parameterPrecision = isThresholdNegative = isIndicatorNagative = parameterThresholdType = "0";
    if (paramObj.attr("data-is-source") == "true") {
        if (paramObj.find("input[id*='txtParameterValue']").length > 0) {
            isParameterValueVisible = true;
        }
        isSourceParam = 1;
        thresholdLow = paramObj.find("input[id*='txtThresholdLow']").val();
        thresholdHigh = paramObj.find("input[id*='txtThresholdHigh']").val();
        threshold = paramObj.find("input[id$='txtThreshold']").val();
        thresholdValueDisplay = (paramObj.find("input[id*='txtThreshold']").length > 0) ? $find(paramObj.find("input[id*='txtThreshold']").attr("id")).get_value() : null;
        parameterValue = paramObj.find("input[id*='txtParameterValue']").val();
        parameterValueDisplay = (paramObj.find("input[id*='txtParameterValue']").length > 0) ? $find(paramObj.find("input[id*='txtParameterValue']").attr("id")).get_value() : null;
        from = paramObj.find("input[id*='txtFrom']").val();
        to = paramObj.find("input[id*='txtTo']").val();

        parameterKey = paramObj.find("input[id*='uxCriteriaValueString']").val();
        parameterPrecision = paramObj.find("input[id*='uxParameterPrecision']").val();
        isThresholdNegative = paramObj.find("input[id*='uxIsThresholdNegative']").val();
        isIndicatorNagative = paramObj.find("input[id*='uxIsIndicatorNagative']").val();
        parameterThresholdType = paramObj.find("input[id*='uxParameterThresholdType']").val();

        reAlertParameterIndicator = paramObj.find("input[id*='txtNRTParameterValue']").val();
        reAlertParameterIndicatorDisplay = (paramObj.find("input[id*='txtNRTParameterValue']").length > 0) ? $find(paramObj.find("input[id*='txtNRTParameterValue']").attr("id")).get_value() : null;
        reAlertThresholdLow = paramObj.find("input[id*='txtNRTThresholdLow']").val();
        reAlertParameterThreshold = paramObj.find("input[id*='txtNRTThreshold']").val();
        reAlertParameterThresholdHigh = paramObj.find("input[id*='txtNRTThresholdHigh']").val();
        fromNRT = paramObj.find("input[id*='txtNRTFrom']").val();
        toNRT = paramObj.find("input[id*='txtNRTTo']").val();
        if (!modal) {
            //valitaion 
            var isMCF = false;
            var txtParameterValue = paramObj.find("input[id*='txtParameterValue']");
            if (txtParameterValue && txtParameterValue.length > 0) {
                var isErrortemp = checkInputValue(txtParameterValue[0], isMCF, true, true, parameterValueDisplay);
                if (isErrortemp) {
                    removemessage(txtParameterValue[0]);
                }
                if (isError && !isErrortemp)
                    isError = isErrortemp;
            }
            else {
                //from -> to
                var indFromID = paramObj.find("input[id*='txtFrom']");
                var indToID = paramObj.find("input[id*='txtTo']");
                if ((indFromID && indFromID.length > 0) || (indToID & indToID.length > 0)) {
                    var isErrortemp = checkInputValue(indFromID[0], isMCF, true);
                    if (isErrortemp)
                        isErrortemp = checkInputValue(indToID[0], isMCF, true);
                    if (isErrortemp) {
                        removemessage(indFromID[0]);
                    }
                    if (isError && !isErrortemp)
                        isError = isErrortemp;
                }
            }
            var thsClientId = paramObj.find("input[id$='txtThreshold']");
            if (thsClientId && thsClientId.length > 0) {
                var isErrortemp = checkInputValue(thsClientId[0], isMCF, true, true, thresholdValueDisplay);
                if (isErrortemp) {
                    removemessage(thsClientId[0]);
                }
                if (isError && !isErrortemp)
                    isError = isErrortemp;
            }
            else {
                //low -> high
                var thsHighID = paramObj.find("input[id*='txtThresholdHigh']");
                var thsLowID = paramObj.find("input[id*='txtThresholdLow']");
                if ((thsHighID && thsHighID.length > 0) || (thsLowID && thsLowID.length > 0)) {
                    var isErrortemp = checkInputValue(thsLowID[0], isMCF, true);
                    if (isErrortemp)
                        isErrortemp = checkInputValue(thsHighID[0], isMCF, true);
                    if (isErrortemp) {
                        removemessage(thsHighID[0]);
                    }
                    if (isError && !isErrortemp)
                        isError = isErrortemp;
                }
            }
            //MCF
            isMCF = true;
            var isErrortemp = checkValidationReAlert(paramObj, parameterValue, threshold, reAlertParameterIndicatorDisplay);
            if (isError && !isErrortemp)
                isError = isErrortemp;
            var parameter = paramObj.find('.parameter');

            if (!isError) {
                $(parameter).addClass("red font-weight-bold");
                paramObj.find('.item-nrt').each(function () {
                    if (!$(this).hasClass('parameter'))
                        $(this).css({ "align-content": "space-between" });
                })
            }
            else {
                $(parameter).removeClass("red font-weight-bold");
                paramObj.find('.item-nrt').each(function () {
                    $(this).css({ "align-content": "" });
                })
            }
        }

    }

    return item = {
        "ParameterKey": paramObj.attr("data-key"),
        "ParameterPrecision": parameterPrecision,
        "IsThresholdNegative": isThresholdNegative,
        "IsIndicatorNegative": isIndicatorNagative,
        "ParameterThresholdType": parameterThresholdType,
        "ThresholdLow": thresholdLow,
        "ThresholdHigh": thresholdHigh,
        "Threshold": threshold,
        "ParameterValue": parameterValue,
        "From": from,
        "To": to,
        "GroupID": groupId.toString(),
        "IsSourceParam": isSourceParam.toString(),
        "IsParameterValueVisible": isParameterValueVisible,
        "ReAlertParameterIndicator": reAlertParameterIndicator,
        "ReAlertParameterThreshold": reAlertParameterThreshold,
        "ReAlertThresholdLow": reAlertThresholdLow,
        "ReAlertParameterThresholdHigh": reAlertParameterThresholdHigh,
        "FromNRT": fromNRT,
        "ToNRT": toNRT,
        "IsSuccess": isError
    };
}

function getObject(arr, index) {
    for (var i = 0; i < arr.length; i++) {
        if (arr[i].Order == index)
            return arr[i].Object;
    }
    return null;
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

function resetTimeout() {
    //Reset on VW
    doResetTimeOut();
    //Reset on other site
    var url = "";
    var data = {};
    if (isAdhoc())
        url = rootURL + "risk_MCF/rm_MCF_AdhocCreate.aspx/ResetTimeOut";
    else
        url = rootURL + "risk_MCF/rm_MCF_Assignment_CreateNew.aspx/ResetTimeOut";
    postServer(url, data, null);
}


function removemessage(id) {
    if (id == null || id == undefined)
        return;
    $($(id).parents(".item-nrt").find(".msgError")[0]).html("");
}

function checkValidationReAlert(paramObj, parameterValue, threshold, displayValue) {
    var indicatorNRT = paramObj.find("input[id*='txtNRTParameterValue']");
    if (indicatorNRT && indicatorNRT.length > 0)
        indicatorNRT = indicatorNRT[0]
    else
        indicatorNRT = null;
    var thsClientIdeNRT = paramObj.find("input[id*='txtNRTThreshold']");
    if (thsClientIdeNRT && thsClientIdeNRT.length > 0)
        thsClientIdeNRT = thsClientIdeNRT[0]
    else
        thsClientIdeNRT = null;

    //check if indicator or threshold orther null
    if (!checkValueNullOrEmpty($(indicatorNRT).val()) || !checkValueNullOrEmpty($(thsClientIdeNRT).val())) {
        var isErrorIntemp = (indicatorNRT) ? checkInputValue(indicatorNRT, true, true, true, displayValue) : true;
        var errorIn = checkvalidation(indicatorNRT, parameterValue, threshold, true, displayValue);
        if (errorIn && isErrorIntemp)
            removemessage(indicatorNRT);
        var isErrorThtemp = (thsClientIdeNRT) ? checkInputValue(thsClientIdeNRT, true, true, true) : true;
        var errorTh = checkvalidation(thsClientIdeNRT, parameterValue, threshold, false, displayValue);
        if (errorTh && isErrorThtemp)
            removemessage(thsClientIdeNRT);
        if (!errorIn || !isErrorIntemp || !errorTh || !isErrorThtemp)
            return false;
    }
    removemessage(indicatorNRT);
    removemessage(thsClientIdeNRT);
    return true;
}
function checkvalidation(parameterNRT, parameterIn, parameterTh, isIndicator, displayValue) {
    if (parameterNRT == null || $(parameterNRT) == null || $(parameterNRT) == undefined)
        return true
    var parameterValueNRT = getValueNumber(parameterNRT);
    var parameterValueIn = getValueNumber(parameterIn);
    var parameterValueTh = getValueNumber(parameterTh);
    if (parameterValueIn === 0 && parameterValueTh === 0 && getValueNumber(parameterNRT) !== 0) {
        showMessageError(parameterNRT, true, "", RiskParameter_js_Threshold_ReAlertCM);
        return false
    }
    var parameter = isIndicator ? parameterValueIn : parameterValueTh;
    if (parameter === 0 && parameterValueNRT !== 0) {
        if (isIndicator)
            showMessageError(parameterNRT, true, "", RiskParameter_js_Indicator_ReAlert01);
        else
            showMessageError(parameterNRT, true, "", RiskParameter_js_Threshold_ReAlert01);
        return false
    }
    if (parameter > 0 && parameterValueNRT === "") {
        if (isIndicator)
            showMessageError(parameterNRT, true, "", RiskParameter_js_Indicator_ReAlert02);
        else
            showMessageError(parameterNRT, true, "", RiskParameter_js_Threshold_ReAlert02);
        return false
    }
    return checkMinMax(parameterNRT, true, true);;
}

function checkValueNullOrEmpty(para) {
    if (para == null || para == undefined || para == "")
        return true
    return false

}
function getValueNumber(parameter) {
    var val = "";
    if (parameter == null || parameter == undefined)
        return val;
    if (typeof (parameter) === "string")
        val = parameter;
    else
        val = $(parameter).val();
    if (checkValueNullOrEmpty(val))
        return "";
    var currentValueStr = removeCommas(checkStartWithCommas(val));
    var currentValue = (currentValueStr.trim().length == 0) ? "" : parseFloat(currentValueStr, 10);
    return currentValue;
}

function updateParameterActions(action) {
    var actions = $("#" + rm_Assignment_CreateNew_uxParamActions).val();
    if (actions === "") {
        actions = action;
    } else {
        var actionArr = actions.split(',');
        var isExist = false;
        for (var i = 0; i < actionArr.length; i++) {
            if (actionArr[i] === action) {
                isExist = true;
            }
        }
        actions = isExist ? actions : actions + "," + action;
    }
    $("#" + rm_Assignment_CreateNew_uxParamActions).val(actions);
}
function showAudit() {
    let parameter = $get(Risk_Assignment_Parameters_hdLinkAudit_ClientID).value;
    url = rootURL + "risk_MCF/rm_MCF_Assignment_AuditReport.aspx?" + parameter;
    openPopupWindow(url, "window2");
}

function displayGridParams() {
    let isMatchAll = $('#' + Risk_Assignment_Parameters_rdMatchAll).prop('checked');
    onMatchChange(isMatchAll ? "True" : "false");
}